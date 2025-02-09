// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.ContactGroupSetupDlg
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.ContactGroup;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI
{
  public class ContactGroupSetupDlg : Form
  {
    private ContactType contactType;
    private EMHelpLink emHelpLink1;
    private bool inEditing;
    private System.ComponentModel.Container components;
    private ListViewSortManager sortMngr;
    private Button btnNew;
    private Button btnRename;
    private Button btnDelete;
    private Button btnOK;
    private ColumnHeader columnHeaderGroupName;
    private ColumnHeader columnHeaderNoContacts;
    private TextBox txtBoxGroupName;
    private Label label1;
    private Button btnDuplicate;
    private ListViewEx lvwGroups;

    public ContactGroupSetupDlg(ContactType contactType)
    {
      this.contactType = contactType;
      this.InitializeComponent();
      this.sortMngr = new ListViewSortManager((ListView) this.lvwGroups, new System.Type[2]
      {
        typeof (ListViewTextCaseInsensitiveSort),
        typeof (ListViewInt32Sort)
      });
      this.sortMngr.Sort(0, SortOrder.Descending);
      this.populateControls();
    }

    private void populateControls()
    {
      ContactGroupInfo[] contactGroupInfoArray = ContactType.PublicBiz != this.contactType ? Session.ContactGroupManager.GetContactGroupsForUser(new ContactGroupCollectionCriteria(Session.UserID, this.contactType, new ContactGroupType[1])) : this.GetFilteredContactGroup();
      if (contactGroupInfoArray != null)
      {
        ListViewItem[] items = new ListViewItem[contactGroupInfoArray.Length];
        int index = 0;
        foreach (ContactGroupInfo contactGroupInfo in contactGroupInfoArray)
        {
          int length = contactGroupInfo.ContactIds != null ? contactGroupInfo.ContactIds.Length : 0;
          items[index] = new ListViewItem(new string[2]
          {
            contactGroupInfo.GroupName,
            length.ToString()
          });
          items[index].Tag = (object) contactGroupInfo;
          ++index;
        }
        this.lvwGroups.Items.AddRange(items);
      }
      if (0 >= this.lvwGroups.Items.Count)
        return;
      this.lvwGroups.Items[0].Selected = true;
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

    private bool isDuplicateGroupName(string groupName, ListViewItem lvwItemToSkip)
    {
      if (this.lvwGroups.Items.Count == 0)
        return false;
      foreach (ListViewItem listViewItem in this.lvwGroups.Items)
      {
        if ((lvwItemToSkip == null || listViewItem != lvwItemToSkip) && string.Compare(groupName, listViewItem.SubItems[0].Text, true) == 0)
          return true;
      }
      return false;
    }

    private void startEditing(Control editControl, ListViewItem lvwItem, int lvwSubItem)
    {
      this.inEditing = true;
      lvwItem.EnsureVisible();
      this.lvwGroups.StartEditing(editControl, lvwItem, lvwSubItem);
    }

    private void startEditing(object parameters)
    {
      object[] objArray = (object[]) parameters;
      if (this.InvokeRequired)
        this.Invoke((Delegate) new WaitCallback(this.startEditing), parameters);
      else
        this.startEditing((Control) objArray[0], (ListViewItem) objArray[1], (int) objArray[2]);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void btnNew_Click(object sender, EventArgs e)
    {
      if (this.inEditing)
        this.lvwGroups.EndEditing(true);
      GroupNameDuplicate groupNameDuplicate1 = new GroupNameDuplicate("", false);
      if (groupNameDuplicate1.ShowDialog((IWin32Window) this) != DialogResult.OK)
        return;
      string getNewName;
      GroupNameDuplicate groupNameDuplicate2;
      for (getNewName = groupNameDuplicate1.GetNewName; this.isDuplicateGroupName(getNewName, (ListViewItem) null); getNewName = groupNameDuplicate2.GetNewName)
      {
        groupNameDuplicate2 = new GroupNameDuplicate(getNewName, false);
        if (groupNameDuplicate2.ShowDialog((IWin32Window) this) != DialogResult.OK)
        {
          this.lvwGroups.Focus();
          return;
        }
      }
      ContactGroupInfo contactGroupInfo = new ContactGroupInfo(0, Session.UserID, this.contactType, ContactGroupType.ContactGroup, getNewName, string.Empty, DateTime.Now, new int[0]);
      ListViewItem listViewItem = new ListViewItem(new string[2]
      {
        getNewName,
        "0"
      });
      listViewItem.Tag = (object) contactGroupInfo;
      this.lvwGroups.Items.Add(listViewItem);
      this.lvwGroups.SelectedItems.Clear();
      listViewItem.Selected = true;
      this.inEditing = true;
      listViewItem.EnsureVisible();
      this.lvwGroups_SubItemEndEditing((object) null, new SubItemEndEditingEventArgs(listViewItem, 1, listViewItem.Text));
      this.lvwGroups.Focus();
    }

    private void btnDuplicate_Click(object sender, EventArgs e)
    {
      if (this.inEditing)
        this.lvwGroups.EndEditing(true);
      if (1 != this.lvwGroups.SelectedItems.Count)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select the group you want to duplicate.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        this.lvwGroups.Focus();
      }
      else
      {
        ContactGroupInfo tag = (ContactGroupInfo) this.lvwGroups.SelectedItems[0].Tag;
        string str = "Copy of " + tag.GroupName;
        if (str.Length > 64)
        {
          GroupNameDuplicate groupNameDuplicate = new GroupNameDuplicate(str, false);
          if (groupNameDuplicate.ShowDialog((IWin32Window) this) == DialogResult.OK)
          {
            str = groupNameDuplicate.GetNewName;
          }
          else
          {
            this.lvwGroups.Focus();
            return;
          }
        }
        GroupNameDuplicate groupNameDuplicate1;
        for (; this.isDuplicateGroupName(str, (ListViewItem) null); str = groupNameDuplicate1.GetNewName)
        {
          groupNameDuplicate1 = new GroupNameDuplicate(str, false);
          if (groupNameDuplicate1.ShowDialog((IWin32Window) this) == DialogResult.OK)
            continue;
          this.lvwGroups.Focus();
          return;
        }
        ContactGroupInfo contactGroupInfo = new ContactGroupInfo(0, Session.UserID, this.contactType, ContactGroupType.ContactGroup, str, tag.GroupDesc, DateTime.Now, (int[]) null);
        contactGroupInfo.AddedContactIds = (int[]) tag.ContactIds.Clone();
        ListViewItem listViewItem = new ListViewItem(new string[2]
        {
          str,
          contactGroupInfo.AddedContactIds.Length.ToString()
        });
        listViewItem.Tag = (object) contactGroupInfo;
        this.lvwGroups.Items.Add(listViewItem);
        this.lvwGroups.SelectedItems.Clear();
        listViewItem.Selected = true;
        this.inEditing = true;
        listViewItem.EnsureVisible();
        this.lvwGroups_SubItemEndEditing((object) null, new SubItemEndEditingEventArgs(listViewItem, 0, listViewItem.Text));
        this.lvwGroups.Focus();
      }
    }

    private void btnRename_Click(object sender, EventArgs e)
    {
      if (1 != this.lvwGroups.SelectedItems.Count)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select the group you want to rename.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        ListViewItem selectedItem = this.lvwGroups.SelectedItems[0];
        GroupNameDuplicate groupNameDuplicate = new GroupNameDuplicate(selectedItem.Text, true);
        if (groupNameDuplicate.ShowDialog((IWin32Window) this) == DialogResult.OK)
        {
          for (string getNewName = groupNameDuplicate.GetNewName; selectedItem.Text != getNewName && this.isDuplicateGroupName(getNewName, (ListViewItem) null); getNewName = groupNameDuplicate.GetNewName)
          {
            groupNameDuplicate = new GroupNameDuplicate(getNewName, false);
            if (groupNameDuplicate.ShowDialog((IWin32Window) this) != DialogResult.OK)
              return;
          }
          selectedItem.Text = groupNameDuplicate.GetNewName;
          this.lvwGroups_SubItemEndEditing((object) null, new SubItemEndEditingEventArgs(selectedItem, 1, selectedItem.Text));
          this.lvwGroups.Focus();
        }
        else
          this.lvwGroups.Focus();
      }
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
      if (this.inEditing)
        this.lvwGroups.EndEditing(true);
      if (this.lvwGroups.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select the group(s) you want to delete.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        this.lvwGroups.Focus();
      }
      else if (DialogResult.No == Utils.Dialog((IWin32Window) this, "Are you sure you want to delete selected contact group(s)?", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk))
      {
        this.lvwGroups.Focus();
      }
      else
      {
        foreach (ListViewItem selectedItem in this.lvwGroups.SelectedItems)
          Session.ContactGroupManager.DeleteContactGroup(((ContactGroupInfo) selectedItem.Tag).GroupId);
        ListViewItem[] dest = new ListViewItem[this.lvwGroups.SelectedItems.Count];
        this.lvwGroups.SelectedItems.CopyTo((Array) dest, 0);
        for (int index = 0; index < dest.Length; ++index)
          this.lvwGroups.Items.Remove(dest[index]);
        this.lvwGroups.Focus();
      }
    }

    private void lvwGroups_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.lvwGroups.SelectedItems.Count == 0)
      {
        this.btnDuplicate.Enabled = false;
        this.btnRename.Enabled = false;
        this.btnDelete.Enabled = false;
      }
      else if (1 == this.lvwGroups.SelectedItems.Count)
      {
        this.btnDuplicate.Enabled = true;
        this.btnRename.Enabled = true;
        this.btnDelete.Enabled = true;
      }
      else
      {
        this.btnDuplicate.Enabled = false;
        this.btnRename.Enabled = false;
        this.btnDelete.Enabled = true;
      }
    }

    private void lvwGroups_SubItemClicked(object sender, SubItemEventArgs e)
    {
    }

    private void lvwGroups_SubItemEndEditing(object sender, SubItemEndEditingEventArgs e)
    {
      ListViewItem lvwItemToSkip = e.Item;
      string groupName = e.DisplayText.Trim();
      if (groupName == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Group name can not be empty", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        e.Cancel = true;
      }
      else if (this.isDuplicateGroupName(groupName, lvwItemToSkip))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The name already exists. Please enter a different name.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        e.Cancel = true;
        ThreadPool.QueueUserWorkItem(new WaitCallback(this.startEditing), (object) new object[3]
        {
          (object) this.txtBoxGroupName,
          (object) e.Item,
          (object) 0
        });
      }
      else
      {
        ContactGroupInfo tag = (ContactGroupInfo) lvwItemToSkip.Tag;
        tag.GroupName = groupName;
        ContactGroupInfo contactGroupInfo;
        try
        {
          contactGroupInfo = Session.ContactGroupManager.SaveContactGroup(tag);
        }
        catch (DuplicateObjectException ex)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "The name already exists. Please enter a different name.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          e.Cancel = true;
          ThreadPool.QueueUserWorkItem(new WaitCallback(this.startEditing), (object) new object[3]
          {
            (object) this.txtBoxGroupName,
            (object) e.Item,
            (object) 0
          });
          return;
        }
        lvwItemToSkip.SubItems[0].Text = contactGroupInfo.GroupName;
        lvwItemToSkip.Tag = (object) contactGroupInfo;
        this.inEditing = false;
      }
    }

    private void Help_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      JedHelp.ShowHelp(this.emHelpLink1.HelpTag);
    }

    private void InitializeComponent()
    {
      this.lvwGroups = new ListViewEx();
      this.columnHeaderGroupName = new ColumnHeader();
      this.columnHeaderNoContacts = new ColumnHeader();
      this.btnNew = new Button();
      this.btnRename = new Button();
      this.btnDelete = new Button();
      this.btnOK = new Button();
      this.btnDuplicate = new Button();
      this.txtBoxGroupName = new TextBox();
      this.label1 = new Label();
      this.emHelpLink1 = new EMHelpLink();
      this.SuspendLayout();
      this.lvwGroups.AllowColumnReorder = true;
      this.lvwGroups.Columns.AddRange(new ColumnHeader[2]
      {
        this.columnHeaderGroupName,
        this.columnHeaderNoContacts
      });
      this.lvwGroups.DoubleClickActivation = false;
      this.lvwGroups.FullRowSelect = true;
      this.lvwGroups.GridLines = true;
      this.lvwGroups.HideSelection = false;
      this.lvwGroups.Location = new Point(8, 40);
      this.lvwGroups.Name = "lvwGroups";
      this.lvwGroups.Size = new Size(446, 224);
      this.lvwGroups.TabIndex = 0;
      this.lvwGroups.UseCompatibleStateImageBehavior = false;
      this.lvwGroups.View = View.Details;
      this.lvwGroups.SubItemClicked += new SubItemEventHandler(this.lvwGroups_SubItemClicked);
      this.lvwGroups.SubItemEndEditing += new SubItemEndEditingEventHandler(this.lvwGroups_SubItemEndEditing);
      this.lvwGroups.SelectedIndexChanged += new EventHandler(this.lvwGroups_SelectedIndexChanged);
      this.columnHeaderGroupName.Text = "Group Name";
      this.columnHeaderGroupName.Width = 350;
      this.columnHeaderNoContacts.Text = "# of Contacts";
      this.columnHeaderNoContacts.TextAlign = HorizontalAlignment.Right;
      this.columnHeaderNoContacts.Width = 80;
      this.btnNew.Location = new Point(8, 8);
      this.btnNew.Name = "btnNew";
      this.btnNew.Size = new Size(72, 24);
      this.btnNew.TabIndex = 1;
      this.btnNew.Text = "New";
      this.btnNew.Click += new EventHandler(this.btnNew_Click);
      this.btnRename.Enabled = false;
      this.btnRename.Location = new Point(168, 8);
      this.btnRename.Name = "btnRename";
      this.btnRename.Size = new Size(72, 24);
      this.btnRename.TabIndex = 2;
      this.btnRename.Text = "Rename";
      this.btnRename.Click += new EventHandler(this.btnRename_Click);
      this.btnDelete.Enabled = false;
      this.btnDelete.Location = new Point(248, 8);
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new Size(72, 24);
      this.btnDelete.TabIndex = 3;
      this.btnDelete.Text = "Delete";
      this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
      this.btnOK.DialogResult = DialogResult.OK;
      this.btnOK.Location = new Point(386, 270);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(68, 24);
      this.btnOK.TabIndex = 4;
      this.btnOK.Text = "OK";
      this.btnDuplicate.Enabled = false;
      this.btnDuplicate.Location = new Point(88, 8);
      this.btnDuplicate.Name = "btnDuplicate";
      this.btnDuplicate.Size = new Size(72, 24);
      this.btnDuplicate.TabIndex = 5;
      this.btnDuplicate.Text = "Duplicate";
      this.btnDuplicate.Click += new EventHandler(this.btnDuplicate_Click);
      this.txtBoxGroupName.Location = new Point(280, 274);
      this.txtBoxGroupName.MaxLength = 23;
      this.txtBoxGroupName.Name = "txtBoxGroupName";
      this.txtBoxGroupName.Size = new Size(100, 20);
      this.txtBoxGroupName.TabIndex = 6;
      this.txtBoxGroupName.Visible = false;
      this.label1.Location = new Point(8, 276);
      this.label1.Name = "label1";
      this.label1.Size = new Size(324, 16);
      this.label1.TabIndex = 7;
      this.label1.Text = "Note: Deleting a group will not delete the contacts in that group.";
      this.emHelpLink1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.HelpTag = "Contact Group Setup";
      this.emHelpLink1.Location = new Point(364, 12);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 16);
      this.emHelpLink1.TabIndex = 8;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.btnOK;
      this.ClientSize = new Size(466, 303);
      this.Controls.Add((Control) this.emHelpLink1);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.txtBoxGroupName);
      this.Controls.Add((Control) this.btnDuplicate);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.btnDelete);
      this.Controls.Add((Control) this.btnRename);
      this.Controls.Add((Control) this.btnNew);
      this.Controls.Add((Control) this.lvwGroups);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.Name = nameof (ContactGroupSetupDlg);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Contact Group Setup";
      this.KeyDown += new KeyEventHandler(this.Help_KeyDown);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
