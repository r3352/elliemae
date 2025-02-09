// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.ContactGroupSelectionDlg
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.ContactGroup;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI
{
  public class ContactGroupSelectionDlg : Form
  {
    private ContactType contactType;
    private ArrayList groupsToExclude;
    private bool newGroupCreated;
    private FlowLayoutPanel flowLayoutPanel1;
    private bool addGroup = true;
    public ContactGroupInfo[] SelectedGroups;
    private ListView listView1;
    private System.ComponentModel.Container components;
    private Button btnOK;
    private ColumnHeader columnHeaderGroupName;
    private Button btnCancel;
    private EMHelpLink emHelpLink1;
    private Label lblSubTitle;
    private Button btnNewGroup;

    public bool IsNewGroupCreated => this.newGroupCreated;

    public ContactGroupSelectionDlg(ContactType contactType, bool addGroup)
    {
      this.contactType = contactType;
      this.groupsToExclude = new ArrayList();
      this.InitializeComponent();
      this.addGroup = addGroup;
      this.initialize();
      this.enforceSecurity();
    }

    public ContactGroupSelectionDlg(
      ContactType contactType,
      ContactGroupInfo[] groupsToExclude,
      bool addGroup)
    {
      this.contactType = contactType;
      this.groupsToExclude = new ArrayList((ICollection) groupsToExclude);
      this.InitializeComponent();
      this.addGroup = addGroup;
      this.initialize();
      this.enforceSecurity();
    }

    private void initialize()
    {
      this.loadContactGroups((ContactGroupInfo) null);
      this.listView1_SelectedIndexChanged((object) null, (EventArgs) null);
      if (this.addGroup)
        return;
      this.btnNewGroup.Visible = false;
      this.lblSubTitle.Text = "Select the groups to remove contacts from.";
    }

    private void loadContactGroups(ContactGroupInfo groupToSelect)
    {
      this.listView1.Items.Clear();
      ContactGroupInfo[] contactGroupInfoArray = ContactType.PublicBiz != this.contactType ? Session.ContactGroupManager.GetContactGroupsForUser(new ContactGroupCollectionCriteria(Session.UserID, this.contactType, new ContactGroupType[1])) : this.GetFilteredContactGroup();
      if (contactGroupInfoArray == null)
        return;
      ArrayList arrayList = new ArrayList();
      for (int index = 0; index < contactGroupInfoArray.Length; ++index)
      {
        if (!this.groupsToExclude.Contains((object) contactGroupInfoArray[index]))
        {
          int num = 0;
          if (contactGroupInfoArray[index].ContactIds != null)
            num = contactGroupInfoArray[index].ContactIds.Length;
          ListViewItem listViewItem = new ListViewItem(new string[2]
          {
            contactGroupInfoArray[index].GroupName,
            num.ToString()
          });
          listViewItem.Tag = (object) contactGroupInfoArray[index];
          arrayList.Add((object) listViewItem);
          if (groupToSelect != (ContactGroupInfo) null && groupToSelect == contactGroupInfoArray[index])
            listViewItem.Selected = true;
        }
      }
      this.listView1.Items.AddRange((ListViewItem[]) arrayList.ToArray(typeof (ListViewItem)));
    }

    private ContactGroupInfo[] GetFilteredContactGroup()
    {
      ContactGroupInfo[] filteredContactGroup = Session.ContactGroupManager.GetPublicBizContactGroups();
      if (!Session.UserInfo.IsSuperAdministrator())
      {
        BizGroupRef[] contactGroupRefs = Session.AclGroupManager.GetBizContactGroupRefs(Session.UserID, true);
        ArrayList arrayList = new ArrayList();
        foreach (ContactGroupInfo contactGroupInfo in filteredContactGroup)
        {
          foreach (BizGroupRef bizGroupRef in contactGroupRefs)
          {
            if (bizGroupRef.BizGroupID == contactGroupInfo.GroupId && !arrayList.Contains((object) contactGroupInfo))
            {
              arrayList.Add((object) contactGroupInfo);
              break;
            }
          }
        }
        filteredContactGroup = (ContactGroupInfo[]) arrayList.ToArray(typeof (ContactGroupInfo));
      }
      return filteredContactGroup;
    }

    private void enforceSecurity()
    {
      if (ContactType.PublicBiz != this.contactType || Session.UserInfo.IsSuperAdministrator())
        return;
      this.btnNewGroup.Enabled = false;
    }

    private bool isDuplicateGroupName(ContactGroupInfo[] groupList, string groupName)
    {
      bool flag = false;
      if (groupList != null && groupList.Length != 0)
      {
        foreach (ContactGroupInfo group in groupList)
        {
          if (string.Compare(groupName, group.GroupName, true) == 0)
          {
            flag = true;
            break;
          }
        }
      }
      return flag;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (0 >= this.listView1.SelectedItems.Count)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select one or more contact groups.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        this.SelectedGroups = new ContactGroupInfo[this.listView1.SelectedItems.Count];
        for (int index = 0; index < this.listView1.SelectedItems.Count; ++index)
        {
          ListViewItem selectedItem = this.listView1.SelectedItems[index];
          this.SelectedGroups[index] = (ContactGroupInfo) selectedItem.Tag;
        }
        this.DialogResult = DialogResult.OK;
      }
    }

    private void listView1_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.listView1.SelectedItems.Count == 0)
        this.btnOK.Enabled = false;
      else
        this.btnOK.Enabled = true;
    }

    private void listView1_DoubleClick(object sender, EventArgs e)
    {
      if (this.listView1.SelectedItems.Count != 1)
        return;
      this.btnOK_Click((object) null, (EventArgs) null);
    }

    private void btnNewGroup_Click(object sender, EventArgs e)
    {
      ContactGroupInfo[] groupList = (ContactGroupInfo[]) null;
      switch (this.contactType)
      {
        case ContactType.Borrower:
        case ContactType.BizPartner:
          groupList = Session.ContactGroupManager.GetContactGroupsForUser(new ContactGroupCollectionCriteria(Session.UserID, this.contactType, new ContactGroupType[1]));
          break;
        case ContactType.PublicBiz:
          groupList = Session.ContactGroupManager.GetPublicBizContactGroups();
          break;
      }
      GroupNameDuplicate groupNameDuplicate1 = new GroupNameDuplicate("", false);
      if (groupNameDuplicate1.ShowDialog((IWin32Window) this) == DialogResult.Cancel)
        return;
      string getNewName;
      GroupNameDuplicate groupNameDuplicate2;
      for (getNewName = groupNameDuplicate1.GetNewName; this.isDuplicateGroupName(groupList, getNewName); getNewName = groupNameDuplicate2.GetNewName)
      {
        groupNameDuplicate2 = new GroupNameDuplicate(getNewName, false);
        if (groupNameDuplicate2.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
      }
      this.newGroupCreated = true;
      ContactGroupInfo contactGroupInfo = new ContactGroupInfo(0, Session.UserID, this.contactType, ContactGroupType.ContactGroup, getNewName, string.Empty, DateTime.Now, new int[0]);
      Session.ContactGroupManager.SaveContactGroup(contactGroupInfo);
      this.loadContactGroups(contactGroupInfo);
      this.listView1.Focus();
    }

    private void InitializeComponent()
    {
      this.listView1 = new ListView();
      this.columnHeaderGroupName = new ColumnHeader();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.lblSubTitle = new Label();
      this.btnNewGroup = new Button();
      this.emHelpLink1 = new EMHelpLink();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.flowLayoutPanel1.SuspendLayout();
      this.SuspendLayout();
      this.listView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.listView1.Columns.AddRange(new ColumnHeader[1]
      {
        this.columnHeaderGroupName
      });
      this.listView1.FullRowSelect = true;
      this.listView1.HeaderStyle = ColumnHeaderStyle.None;
      this.listView1.Location = new Point(8, 32);
      this.listView1.Name = "listView1";
      this.listView1.Size = new Size(309, 180);
      this.listView1.TabIndex = 3;
      this.listView1.UseCompatibleStateImageBehavior = false;
      this.listView1.View = View.Details;
      this.listView1.SelectedIndexChanged += new EventHandler(this.listView1_SelectedIndexChanged);
      this.listView1.DoubleClick += new EventHandler(this.listView1_DoubleClick);
      this.columnHeaderGroupName.Text = "Group Name";
      this.columnHeaderGroupName.Width = 244;
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.btnOK.Location = new Point(4, 0);
      this.btnOK.Margin = new Padding(0);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(60, 22);
      this.btnOK.TabIndex = 4;
      this.btnOK.Text = "OK";
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(178, 0);
      this.btnCancel.Margin = new Padding(3, 0, 0, 0);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(60, 22);
      this.btnCancel.TabIndex = 5;
      this.btnCancel.Text = "Cancel";
      this.lblSubTitle.Location = new Point(8, 12);
      this.lblSubTitle.Name = "lblSubTitle";
      this.lblSubTitle.Size = new Size(189, 16);
      this.lblSubTitle.TabIndex = 6;
      this.lblSubTitle.Text = "Select the groups to add contacts to.";
      this.btnNewGroup.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.btnNewGroup.Location = new Point(67, 0);
      this.btnNewGroup.Margin = new Padding(3, 0, 0, 0);
      this.btnNewGroup.Name = "btnNewGroup";
      this.btnNewGroup.Size = new Size(108, 22);
      this.btnNewGroup.TabIndex = 7;
      this.btnNewGroup.Text = "Create New Group";
      this.btnNewGroup.Click += new EventHandler(this.btnNewGroup_Click);
      this.emHelpLink1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.HelpTag = "Contact Group Selection";
      this.emHelpLink1.Location = new Point(227, 12);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 16);
      this.emHelpLink1.TabIndex = 8;
      this.flowLayoutPanel1.Controls.Add((Control) this.btnCancel);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnNewGroup);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnOK);
      this.flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel1.Location = new Point(79, 218);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new Size(238, 22);
      this.flowLayoutPanel1.TabIndex = 9;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(327, 248);
      this.Controls.Add((Control) this.flowLayoutPanel1);
      this.Controls.Add((Control) this.emHelpLink1);
      this.Controls.Add((Control) this.lblSubTitle);
      this.Controls.Add((Control) this.listView1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ContactGroupSelectionDlg);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Contact Group Selection";
      this.KeyUp += new KeyEventHandler(this.ContactGroupSelectionDlg_KeyUp);
      this.KeyDown += new KeyEventHandler(this.ContactGroupSelectionDlg_KeyDown);
      this.flowLayoutPanel1.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private void ContactGroupSelectionDlg_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Escape)
        return;
      this.btnCancel.PerformClick();
    }

    private void ContactGroupSelectionDlg_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      JedHelp.ShowHelp(this.emHelpLink1.HelpTag);
    }
  }
}
