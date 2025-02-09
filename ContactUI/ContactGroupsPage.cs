// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.ContactGroupsPage
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.ContactGroup;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI
{
  public class ContactGroupsPage : Form
  {
    private Panel panel1;
    private IContainer components;
    public bool IsReadOnly;
    private ContactType contactType;
    private int currentContactId = -1;
    private BizPartnerInfo1Form parentForm;
    private GroupContainer gcGroup;
    private StandardIconButton btnRemove;
    private StandardIconButton btnAdd;
    private ToolTip toolTip1;
    private GridView listViewGroup;

    public event EventHandler GroupsModified;

    public ContactGroupsPage(BizPartnerInfo1Form parentForm, ContactType contactType)
    {
      this.InitializeComponent();
      this.parentForm = parentForm;
      this.contactType = contactType;
    }

    public ContactGroupsPage(ContactType contactType)
    {
      this.InitializeComponent();
      this.contactType = contactType;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      GVColumn gvColumn = new GVColumn();
      this.panel1 = new Panel();
      this.gcGroup = new GroupContainer();
      this.listViewGroup = new GridView();
      this.btnRemove = new StandardIconButton();
      this.btnAdd = new StandardIconButton();
      this.toolTip1 = new ToolTip(this.components);
      this.panel1.SuspendLayout();
      this.gcGroup.SuspendLayout();
      ((ISupportInitialize) this.btnRemove).BeginInit();
      ((ISupportInitialize) this.btnAdd).BeginInit();
      this.SuspendLayout();
      this.panel1.Controls.Add((Control) this.gcGroup);
      this.panel1.Dock = DockStyle.Fill;
      this.panel1.Location = new Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(299, 186);
      this.panel1.TabIndex = 5;
      this.gcGroup.Controls.Add((Control) this.listViewGroup);
      this.gcGroup.Controls.Add((Control) this.btnRemove);
      this.gcGroup.Controls.Add((Control) this.btnAdd);
      this.gcGroup.Dock = DockStyle.Fill;
      this.gcGroup.Location = new Point(0, 0);
      this.gcGroup.Name = "gcGroup";
      this.gcGroup.Size = new Size(299, 186);
      this.gcGroup.TabIndex = 8;
      this.gcGroup.Text = "My Groups";
      this.listViewGroup.BorderStyle = BorderStyle.None;
      gvColumn.ImageIndex = -1;
      gvColumn.Name = "Column1";
      gvColumn.Text = "Group Name";
      gvColumn.Width = 240;
      this.listViewGroup.Columns.AddRange(new GVColumn[1]
      {
        gvColumn
      });
      this.listViewGroup.Dock = DockStyle.Fill;
      this.listViewGroup.Location = new Point(1, 26);
      this.listViewGroup.Name = "listViewGroup";
      this.listViewGroup.Size = new Size(297, 159);
      this.listViewGroup.TabIndex = 7;
      this.listViewGroup.SelectedIndexChanged += new EventHandler(this.listViewGroup_SelectedIndexChanged);
      this.btnRemove.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnRemove.BackColor = Color.Transparent;
      this.btnRemove.Location = new Point(278, 4);
      this.btnRemove.Name = "btnRemove";
      this.btnRemove.Size = new Size(16, 16);
      this.btnRemove.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnRemove.TabIndex = 6;
      this.btnRemove.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnRemove, "Remove from Group");
      this.btnRemove.Click += new EventHandler(this.btnRemove_Click);
      this.btnAdd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAdd.BackColor = Color.Transparent;
      this.btnAdd.Location = new Point(258, 4);
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new Size(16, 16);
      this.btnAdd.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAdd.TabIndex = 5;
      this.btnAdd.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnAdd, "Add to Group");
      this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
      this.AutoScaleBaseSize = new Size(5, 13);
      this.BackColor = Color.White;
      this.ClientSize = new Size(299, 186);
      this.Controls.Add((Control) this.panel1);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Name = nameof (ContactGroupsPage);
      this.ShowInTaskbar = false;
      this.Text = "ContactAppointmentsPage";
      this.panel1.ResumeLayout(false);
      this.gcGroup.ResumeLayout(false);
      ((ISupportInitialize) this.btnRemove).EndInit();
      ((ISupportInitialize) this.btnAdd).EndInit();
      this.ResumeLayout(false);
    }

    public int CurrentContact
    {
      get => this.currentContactId;
      set
      {
        if (value == this.currentContactId)
          return;
        this.currentContactId = -1;
        this.disableControls();
        if (value >= 0)
        {
          this.currentContactId = value;
          this.loadForm();
          this.btnAdd.Enabled = true;
          this.btnRemove.Enabled = true;
          if (this.listViewGroup.SelectedItems.Count != 0)
            return;
          this.btnRemove.Enabled = false;
        }
        else
        {
          this.btnAdd.Enabled = false;
          this.btnRemove.Enabled = false;
        }
      }
    }

    public void RefreshData()
    {
      this.disableControls();
      if (this.currentContactId >= 0)
      {
        this.loadForm();
        this.btnAdd.Enabled = true;
        this.btnRemove.Enabled = true;
        if (this.listViewGroup.SelectedItems.Count != 0)
          return;
        this.btnRemove.Enabled = false;
      }
      else
      {
        this.btnAdd.Enabled = false;
        this.btnRemove.Enabled = false;
      }
    }

    public void disableForm() => this.disableControlsOnly();

    private void disableControlsOnly()
    {
      this.btnAdd.Enabled = false;
      this.btnRemove.Enabled = false;
    }

    private void disableControls()
    {
      this.clearForm();
      this.disableControlsOnly();
    }

    private void enableControls()
    {
      this.btnRemove.Enabled = true;
      this.btnAdd.Enabled = true;
    }

    private void loadForm()
    {
      if (!this.IsReadOnly)
        this.enableControls();
      this.loadGroupsForContact();
    }

    private void clearForm()
    {
      this.listViewGroup.Items.Clear();
      this.gcGroup.Text = "My Groups (0)";
    }

    public bool SaveChanges() => true;

    public void SetColors(Color backColor, Color btnColor, Color contactColor)
    {
    }

    private void loadGroupsForContact()
    {
      this.listViewGroup.Items.Clear();
      ContactGroupInfo[] groupsForContact = Session.ContactGroupManager.GetContactGroupsForContact(this.contactType, this.currentContactId);
      if (groupsForContact == null)
        return;
      List<GVItem> gvItemList = new List<GVItem>();
      for (int index = 0; index < groupsForContact.Length; ++index)
      {
        if (this.contactType == ContactType.PublicBiz || !(groupsForContact[index].UserId != Session.UserID))
          gvItemList.Add(new GVItem(new string[1]
          {
            groupsForContact[index].GroupName
          })
          {
            Tag = (object) groupsForContact[index]
          });
      }
      this.listViewGroup.Items.AddRange(gvItemList.ToArray());
      this.listViewGroup_SelectedIndexChanged((object) null, (EventArgs) null);
      this.gcGroup.Text = "My Groups (" + (object) this.listViewGroup.Items.Count + ")";
    }

    public ContactGroupInfo[] getGroupsOfCurrentContact()
    {
      if (this.listViewGroup.Items.Count == 0)
        return new ContactGroupInfo[0];
      ContactGroupInfo[] ofCurrentContact = new ContactGroupInfo[this.listViewGroup.Items.Count];
      for (int nItemIndex = 0; nItemIndex < this.listViewGroup.Items.Count; ++nItemIndex)
        ofCurrentContact[nItemIndex] = (ContactGroupInfo) this.listViewGroup.Items[nItemIndex].Tag;
      return ofCurrentContact;
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
      ContactGroupSelectionDlg groupSelectionDlg = new ContactGroupSelectionDlg(this.contactType, this.getGroupsOfCurrentContact(), true);
      if (DialogResult.Cancel == groupSelectionDlg.ShowDialog((IWin32Window) this))
        return;
      ContactGroupInfo[] selectedGroups = groupSelectionDlg.SelectedGroups;
      for (int index = 0; index < selectedGroups.Length; ++index)
      {
        selectedGroups[index].AddedContactIds = new int[1]
        {
          this.currentContactId
        };
        Session.ContactGroupManager.SaveContactGroup(selectedGroups[index]);
      }
      if (this.contactType == ContactType.PublicBiz)
      {
        BizPartnerInfo bizPartner = Session.ContactManager.GetBizPartner(this.currentContactId);
        if (bizPartner != null && bizPartner.AccessLevel == ContactAccess.Private)
        {
          bizPartner.OwnerID = "";
          this.parentForm.MakePublic();
          this.parentForm.ResetBizPartnerInfo = bizPartner;
        }
      }
      if (this.contactType != ContactType.Borrower && this.contactType != ContactType.BizPartner)
        this.parentForm.SaveChanges();
      this.loadGroupsForContact();
      if (this.GroupsModified == null)
        return;
      this.GroupsModified((object) null, (EventArgs) null);
    }

    private string getContactName()
    {
      if (this.contactType == ContactType.Borrower)
      {
        BorrowerInfo borrower = Session.ContactManager.GetBorrower(this.currentContactId);
        return borrower.FirstName + " " + borrower.LastName;
      }
      BizPartnerInfo bizPartner = Session.ContactManager.GetBizPartner(this.currentContactId);
      return bizPartner.FirstName + " " + bizPartner.LastName;
    }

    private void btnRemove_Click(object sender, EventArgs e)
    {
      if (this.listViewGroup.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) Session.MainForm, "Please select the groups you want to delete before clicking the Remove button.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        string contactName = this.getContactName();
        if (this.listViewGroup.SelectedItems.Count == 1)
        {
          if (DialogResult.No == Utils.Dialog((IWin32Window) Session.MainForm, "Are you sure you want to remove " + contactName + " from the group? ", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk))
            return;
        }
        else if (7 == (int) Utils.Dialog((IWin32Window) Session.MainForm, "Are you sure you want to remove " + contactName + " from the " + this.listViewGroup.SelectedItems.Count.ToString() + " groups? ", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk))
          return;
        bool flag1 = false;
        if (this.contactType == ContactType.PublicBiz && !Session.UserInfo.IsSuperAdministrator())
        {
          BizGroupRef[] contactGroupRefs = Session.AclGroupManager.GetBizContactGroupRefs(Session.UserID, true);
          foreach (GVItem gvItem in (IEnumerable<GVItem>) this.listViewGroup.Items)
          {
            if (!gvItem.Selected)
            {
              ContactGroupInfo tag = (ContactGroupInfo) gvItem.Tag;
              foreach (BizGroupRef bizGroupRef in contactGroupRefs)
              {
                if (bizGroupRef.BizGroupID == tag.GroupId)
                {
                  flag1 = true;
                  break;
                }
              }
            }
            if (flag1)
              break;
          }
          if (!flag1 && Utils.Dialog((IWin32Window) this, "If you remove the \"" + Session.ContactManager.GetBizPartner(this.currentContactId).FullName + "\" contact, it will no longer to available to you to view or edit\nDo you want to proceed?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
            return;
        }
        int num2 = -1;
        bool flag2 = false;
        foreach (GVItem selectedItem in this.listViewGroup.SelectedItems)
        {
          ContactGroupInfo tag = (ContactGroupInfo) selectedItem.Tag;
          if (this.contactType != ContactType.Borrower && num2 == tag.GroupId)
            flag2 = true;
          if (this.contactType != ContactType.PublicBiz || Session.AclGroupManager.GetBizContactGroupAccessRight(Session.UserInfo, tag.GroupId) != AclTriState.False)
          {
            tag.DeletedContactIds = new int[1]
            {
              this.currentContactId
            };
            Session.ContactGroupManager.SaveContactGroup(tag);
          }
        }
        this.loadGroupsForContact();
        if (!flag1 && this.contactType == ContactType.PublicBiz && !Session.UserInfo.IsSuperAdministrator())
          this.parentForm.RefreshContactList();
        else if (flag2)
          this.parentForm.RefreshContactList();
        if (this.GroupsModified == null)
          return;
        this.GroupsModified((object) null, (EventArgs) null);
      }
    }

    private void listViewGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.currentContactId == -1)
        return;
      if (this.listViewGroup.SelectedItems.Count == 0)
      {
        this.btnRemove.Enabled = false;
      }
      else
      {
        this.btnRemove.Enabled = true;
        if (ContactType.PublicBiz != this.contactType || Session.AclGroupManager.GetBizContactGroupAccessRight(Session.UserInfo, ((ContactGroupInfo) this.listViewGroup.SelectedItems[0].Tag).GroupId) != AclTriState.False)
          return;
        this.btnRemove.Enabled = false;
      }
    }
  }
}
