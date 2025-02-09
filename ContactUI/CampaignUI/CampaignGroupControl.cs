// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.CampaignUI.CampaignGroupControl
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.Calendar;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.ContactGroup;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.ContactGroup;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI.CampaignUI
{
  public class CampaignGroupControl : UserControl
  {
    private EllieMae.EMLite.Campaign.Campaign campaign;
    private ContactGroupCollection contactGroups;
    private ContactGroupMemberCursorCollection contactGroupCollection;
    private SortField[] contactGroupCollectionSortFields;
    private GridView gvCampaignGroup;
    private GridView gvContactGroup;
    private ContextMenuStrip ctxCampaignGroup;
    private ToolStripMenuItem mnuCampaignGroupView;
    private ToolStripMenuItem mnuCampaignGroupRemove;
    private ContextMenuStrip ctxContactGroup;
    private ToolStripMenuItem mnuContactGroupView;
    private ToolStripMenuItem mnuContactGroupAdd;
    private Label label1;
    private Label label2;
    private PageListNavigator navContactGroup;
    private BizCategoryUtil catUtil;
    private IContainer components;
    private ComboBox cboContactGroup;
    private Label lblCampaignContacts;
    private Button btnRemoveCampaign;
    private Button btnAddGroup;
    private Label lblContactGroup;
    private CheckBox chkPrimaryOnly;

    public CampaignGroupControl(EllieMae.EMLite.Campaign.Campaign campaign)
    {
      this.campaign = campaign;
      Cursor.Current = Cursors.WaitCursor;
      this.InitializeComponent();
      this.populateControls();
      Cursor.Current = Cursors.Default;
    }

    private void populateControls()
    {
      this.gvContactGroup.Columns.Clear();
      this.gvContactGroup.Columns.Add("Last Name", 82, ContentAlignment.MiddleLeft);
      this.gvContactGroup.Columns.Add("First Name", 82, ContentAlignment.MiddleLeft);
      if (this.campaign.ContactType == EllieMae.EMLite.ContactUI.ContactType.Borrower)
      {
        this.gvContactGroup.Columns.Add("Status", 68, ContentAlignment.MiddleLeft);
        this.gvContactGroup.Columns.Add("Owner", 56, ContentAlignment.MiddleLeft);
        this.chkPrimaryOnly.Visible = true;
      }
      else
      {
        this.gvContactGroup.Columns.Add("Category", 58, ContentAlignment.MiddleLeft);
        this.gvContactGroup.Columns.Add("Company", 66, ContentAlignment.MiddleLeft);
      }
      this.contactGroups = ContactGroupCollection.GetContactGroupCollection(new ContactGroupCollectionCriteria(this.campaign.UserId, this.campaign.ContactType, new ContactGroupType[1]), Session.SessionObjects);
      foreach (EllieMae.EMLite.ContactGroup.ContactGroup contactGroup in (CollectionBase) this.contactGroups)
        this.cboContactGroup.Items.Add((object) contactGroup.GroupName);
      if (this.cboContactGroup.Items.Count > 0)
        this.cboContactGroup.SelectedIndex = 0;
      this.getCampaignGroup();
    }

    private void getCampaignGroup()
    {
      this.lblCampaignContacts.Text = "Contacts Selected for Campaign: 0";
      this.gvCampaignGroup.Items.Clear();
      EllieMae.EMLite.ContactGroup.ContactGroup contactGroup = this.campaign.ContactGroup;
      if (contactGroup == null || contactGroup.GroupMembers.Count == 0)
        return;
      this.gvCampaignGroup.BeginUpdate();
      foreach (ContactGroupMember groupMember in (CollectionBase) contactGroup.GroupMembers)
        this.gvCampaignGroup.Items.Add(new GVItem(groupMember.LastName)
        {
          SubItems = {
            (object) groupMember.FirstName,
            (object) groupMember.DateAdded.ToShortDateString()
          },
          Tag = (object) groupMember
        });
      this.gvCampaignGroup.EndUpdate();
      this.lblCampaignContacts.Text = "Contacts Selected for Campaign: " + this.gvCampaignGroup.Items.Count.ToString();
    }

    private void getContactGroupCollection()
    {
      Cursor.Current = Cursors.WaitCursor;
      try
      {
        EllieMae.EMLite.ContactGroup.ContactGroup contactGroup = this.contactGroups[this.cboContactGroup.SelectedIndex];
        this.contactGroupCollection = ContactGroupMemberCursorCollection.NewContactGroupMemberCursorCollection(contactGroup.ContactType, contactGroup.GroupId, Session.SessionObjects);
        ArrayList arrayList = new ArrayList();
        if (this.chkPrimaryOnly.Checked)
          arrayList.Add((object) new OrdinalValueCriterion("Contact.PrimaryContact", (object) 1, OrdinalMatchType.Equals));
        this.contactGroupCollection.AddQueryCriteria((QueryCriterion[]) arrayList.ToArray(typeof (QueryCriterion)));
        this.contactGroupCollection.SetSortFields(this.contactGroupCollectionSortFields);
        this.navContactGroup.NumberOfItems = ((ICursor) this.contactGroupCollection).GetItemCount();
      }
      finally
      {
        Cursor.Current = Cursors.Default;
      }
    }

    private void viewContactGroupContact()
    {
      if (1 != this.gvContactGroup.SelectedItems.Count)
        return;
      Cursor.Current = Cursors.WaitCursor;
      ContactGroupMember tag = (ContactGroupMember) this.gvContactGroup.SelectedItems[0].Tag;
      AttendeeInfo attendeeInfo = new AttendeeInfo(false);
      attendeeInfo.AssignInfo(tag.ContactId, tag.ContactType, false);
      Cursor.Current = Cursors.Default;
      int num = (int) attendeeInfo.ShowDialog();
    }

    private void viewCampaignGroupContact()
    {
      if (1 != this.gvCampaignGroup.SelectedItems.Count)
        return;
      Cursor.Current = Cursors.WaitCursor;
      ContactGroupMember tag = (ContactGroupMember) this.gvCampaignGroup.SelectedItems[0].Tag;
      AttendeeInfo attendeeInfo = new AttendeeInfo(false);
      attendeeInfo.AssignInfo(tag.ContactId, tag.ContactType, false);
      Cursor.Current = Cursors.Default;
      int num = (int) attendeeInfo.ShowDialog();
    }

    private void addContactsToCampaignGroup()
    {
      this.lblCampaignContacts.Text = "Contacts Selected for Campaign: 0";
      Cursor.Current = Cursors.WaitCursor;
      this.gvContactGroup.BeginUpdate();
      this.gvCampaignGroup.BeginUpdate();
      foreach (GVItem selectedItem in this.gvContactGroup.SelectedItems)
      {
        ContactGroupMember tag = (ContactGroupMember) selectedItem.Tag;
        if (!this.campaign.ContactGroup.GroupMembers.Contains(tag))
        {
          ContactGroupMember contactGroupMember;
          if (tag.ContactType == EllieMae.EMLite.ContactUI.ContactType.Borrower)
          {
            contactGroupMember = (ContactGroupMember) BorrowerGroupMember.NewBorrowerGroupMember(this.campaign.ContactGroup.GroupId, (BorrowerGroupMember) tag);
            this.campaign.ContactGroup.GroupMembers.Add(contactGroupMember);
          }
          else
          {
            contactGroupMember = (ContactGroupMember) PartnerGroupMember.NewPartnerGroupMember(this.campaign.ContactGroup.GroupId, (PartnerGroupMember) tag);
            this.campaign.ContactGroup.GroupMembers.Add(contactGroupMember);
          }
          this.gvCampaignGroup.Items.Add(new GVItem(contactGroupMember.LastName)
          {
            SubItems = {
              (object) contactGroupMember.FirstName,
              (object) contactGroupMember.DateAdded.ToShortDateString()
            },
            Tag = (object) contactGroupMember
          });
        }
      }
      this.gvContactGroup.SelectedItems.Clear();
      this.gvCampaignGroup.EndUpdate();
      this.gvContactGroup.EndUpdate();
      Cursor.Current = Cursors.Default;
      this.btnAddGroup.Enabled = false;
      this.lblCampaignContacts.Text = "Contacts Selected for Campaign: " + this.gvCampaignGroup.Items.Count.ToString();
    }

    private void removeContactsFromCampaignGroup()
    {
      Cursor.Current = Cursors.WaitCursor;
      this.gvCampaignGroup.BeginUpdate();
      foreach (GVItem selectedItem in this.gvCampaignGroup.SelectedItems)
      {
        ContactGroupMember tag = (ContactGroupMember) selectedItem.Tag;
        if (this.campaign.ContactGroup.GroupMembers.Contains(tag))
        {
          this.campaign.ContactGroup.GroupMembers.Remove(tag);
          this.gvCampaignGroup.Items.Remove(selectedItem);
        }
      }
      this.gvCampaignGroup.SelectedItems.Clear();
      this.gvCampaignGroup.EndUpdate();
      Cursor.Current = Cursors.Default;
      this.btnRemoveCampaign.Enabled = false;
      this.lblCampaignContacts.Text = "Contacts Selected for Campaign: " + this.gvCampaignGroup.Items.Count.ToString();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void cboContactGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (-1 == this.cboContactGroup.SelectedIndex)
        return;
      this.contactGroupCollectionSortFields = (SortField[]) null;
      this.getContactGroupCollection();
      this.btnAddGroup.Enabled = false;
      Cursor.Current = Cursors.Default;
    }

    private void gvContactGroup_DoubleClick(object sender, EventArgs e)
    {
      this.viewContactGroupContact();
    }

    private void gvContactGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnAddGroup.Enabled = 0 < this.gvContactGroup.SelectedItems.Count;
    }

    private void gvContactGroup_SortItems(object source, GVColumnSortEventArgs e)
    {
      string fieldName = (string) null;
      if (this.campaign.ContactType == EllieMae.EMLite.ContactUI.ContactType.Borrower)
      {
        switch (e.Column)
        {
          case 0:
            fieldName = "Contact.LastName";
            break;
          case 1:
            fieldName = "Contact.FirstName";
            break;
          case 2:
            fieldName = "Contact.Status";
            break;
          case 3:
            fieldName = "Contact.OwnerId";
            break;
        }
      }
      else
      {
        switch (e.Column)
        {
          case 0:
            fieldName = "Contact.LastName";
            break;
          case 1:
            fieldName = "Contact.FirstName";
            break;
          case 2:
            fieldName = "Contact.CategoryId";
            break;
          case 3:
            fieldName = "Contact.CompanyName";
            break;
        }
      }
      this.contactGroupCollectionSortFields = new SortField[1];
      this.contactGroupCollectionSortFields[0] = new SortField(fieldName, SortOrder.Ascending == e.SortOrder ? FieldSortOrder.Ascending : FieldSortOrder.Descending);
      this.getContactGroupCollection();
    }

    private void chkPrimaryOnly_CheckedChanged(object sender, EventArgs e)
    {
      if (this.campaign.ContactType != EllieMae.EMLite.ContactUI.ContactType.Borrower)
        return;
      this.getContactGroupCollection();
    }

    private void btnAddGroup_Click(object sender, EventArgs e) => this.addContactsToCampaignGroup();

    private void ctxContactGroup_Opening(object sender, CancelEventArgs e)
    {
      this.mnuContactGroupView.Enabled = 1 == this.gvContactGroup.SelectedItems.Count;
      this.mnuContactGroupAdd.Enabled = this.btnAddGroup.Enabled;
    }

    private void mnuContactGroupView_Click(object sender, EventArgs e)
    {
      this.viewContactGroupContact();
    }

    private void mnuContactGroupAdd_Click(object sender, EventArgs e)
    {
      this.addContactsToCampaignGroup();
    }

    private void gvCampaignGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnRemoveCampaign.Enabled = 0 < this.gvCampaignGroup.SelectedItems.Count;
    }

    private void btnRemoveCampaign_Click(object sender, EventArgs e)
    {
      this.removeContactsFromCampaignGroup();
    }

    private void gvCampaignGroup_DoubleClick(object sender, EventArgs e)
    {
      this.viewCampaignGroupContact();
    }

    private void ctxCampaignGroup_Opening(object sender, CancelEventArgs e)
    {
      this.mnuCampaignGroupView.Enabled = 1 == this.gvCampaignGroup.SelectedItems.Count;
      this.mnuCampaignGroupRemove.Enabled = this.btnRemoveCampaign.Enabled;
    }

    private void mnuCampaignGroupView_Click(object sender, EventArgs e)
    {
      this.viewCampaignGroupContact();
    }

    private void mnuCampaignGroupRemove_Click(object sender, EventArgs e)
    {
      this.removeContactsFromCampaignGroup();
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      GVColumn gvColumn5 = new GVColumn();
      GVColumn gvColumn6 = new GVColumn();
      GVColumn gvColumn7 = new GVColumn();
      this.cboContactGroup = new ComboBox();
      this.lblCampaignContacts = new Label();
      this.btnRemoveCampaign = new Button();
      this.btnAddGroup = new Button();
      this.lblContactGroup = new Label();
      this.chkPrimaryOnly = new CheckBox();
      this.gvCampaignGroup = new GridView();
      this.ctxCampaignGroup = new ContextMenuStrip(this.components);
      this.mnuCampaignGroupView = new ToolStripMenuItem();
      this.mnuCampaignGroupRemove = new ToolStripMenuItem();
      this.gvContactGroup = new GridView();
      this.ctxContactGroup = new ContextMenuStrip(this.components);
      this.mnuContactGroupView = new ToolStripMenuItem();
      this.mnuContactGroupAdd = new ToolStripMenuItem();
      this.label1 = new Label();
      this.label2 = new Label();
      this.navContactGroup = new PageListNavigator();
      this.ctxCampaignGroup.SuspendLayout();
      this.ctxContactGroup.SuspendLayout();
      this.SuspendLayout();
      this.cboContactGroup.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboContactGroup.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cboContactGroup.Location = new Point(116, 12);
      this.cboContactGroup.Name = "cboContactGroup";
      this.cboContactGroup.Size = new Size(202, 22);
      this.cboContactGroup.TabIndex = 61;
      this.cboContactGroup.SelectedIndexChanged += new EventHandler(this.cboContactGroup_SelectedIndexChanged);
      this.lblCampaignContacts.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.lblCampaignContacts.AutoSize = true;
      this.lblCampaignContacts.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblCampaignContacts.Location = new Point(406, 16);
      this.lblCampaignContacts.Name = "lblCampaignContacts";
      this.lblCampaignContacts.Size = new Size(174, 14);
      this.lblCampaignContacts.TabIndex = 65;
      this.lblCampaignContacts.Text = "Contacts Selected for Campaign: 0";
      this.lblCampaignContacts.TextAlign = ContentAlignment.MiddleLeft;
      this.btnRemoveCampaign.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.btnRemoveCampaign.Enabled = false;
      this.btnRemoveCampaign.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnRemoveCampaign.ImageAlign = ContentAlignment.MiddleRight;
      this.btnRemoveCampaign.Location = new Point(326, 164);
      this.btnRemoveCampaign.Name = "btnRemoveCampaign";
      this.btnRemoveCampaign.Size = new Size(72, 23);
      this.btnRemoveCampaign.TabIndex = 64;
      this.btnRemoveCampaign.Text = "< Remove";
      this.btnRemoveCampaign.Click += new EventHandler(this.btnRemoveCampaign_Click);
      this.btnAddGroup.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.btnAddGroup.Enabled = false;
      this.btnAddGroup.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnAddGroup.ImageAlign = ContentAlignment.MiddleRight;
      this.btnAddGroup.Location = new Point(326, 132);
      this.btnAddGroup.Name = "btnAddGroup";
      this.btnAddGroup.Size = new Size(72, 23);
      this.btnAddGroup.TabIndex = 63;
      this.btnAddGroup.Text = "Add >";
      this.btnAddGroup.Click += new EventHandler(this.btnAddGroup_Click);
      this.lblContactGroup.AutoSize = true;
      this.lblContactGroup.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblContactGroup.Location = new Point(8, 16);
      this.lblContactGroup.Name = "lblContactGroup";
      this.lblContactGroup.Size = new Size(109, 14);
      this.lblContactGroup.TabIndex = 62;
      this.lblContactGroup.Text = "View Contact Group:";
      this.lblContactGroup.TextAlign = ContentAlignment.MiddleLeft;
      this.chkPrimaryOnly.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.chkPrimaryOnly.AutoSize = true;
      this.chkPrimaryOnly.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.chkPrimaryOnly.Location = new Point(8, 378);
      this.chkPrimaryOnly.Name = "chkPrimaryOnly";
      this.chkPrimaryOnly.Size = new Size(164, 18);
      this.chkPrimaryOnly.TabIndex = 67;
      this.chkPrimaryOnly.Text = "Show primary contacts only.";
      this.chkPrimaryOnly.Visible = false;
      this.chkPrimaryOnly.CheckedChanged += new EventHandler(this.chkPrimaryOnly_CheckedChanged);
      this.gvCampaignGroup.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "gvcLastName";
      gvColumn1.Text = "Last Name";
      gvColumn1.Width = 81;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "gvcFirstName";
      gvColumn2.Text = "First Name";
      gvColumn2.Width = 81;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "gvcDateAdded";
      gvColumn3.Text = "Date Added";
      gvColumn3.Width = 73;
      this.gvCampaignGroup.Columns.AddRange(new GVColumn[3]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3
      });
      this.gvCampaignGroup.ContextMenuStrip = this.ctxCampaignGroup;
      this.gvCampaignGroup.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvCampaignGroup.Location = new Point(406, 41);
      this.gvCampaignGroup.Name = "gvCampaignGroup";
      this.gvCampaignGroup.Size = new Size(257, 331);
      this.gvCampaignGroup.TabIndex = 69;
      this.gvCampaignGroup.SelectedIndexChanged += new EventHandler(this.gvCampaignGroup_SelectedIndexChanged);
      this.gvCampaignGroup.DoubleClick += new EventHandler(this.gvCampaignGroup_DoubleClick);
      this.ctxCampaignGroup.Items.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.mnuCampaignGroupView,
        (ToolStripItem) this.mnuCampaignGroupRemove
      });
      this.ctxCampaignGroup.Name = "ctxCampaignGroup";
      this.ctxCampaignGroup.Size = new Size(118, 48);
      this.ctxCampaignGroup.Opening += new CancelEventHandler(this.ctxCampaignGroup_Opening);
      this.mnuCampaignGroupView.Name = "mnuCampaignGroupView";
      this.mnuCampaignGroupView.Size = new Size(117, 22);
      this.mnuCampaignGroupView.Text = "View";
      this.mnuCampaignGroupView.Click += new EventHandler(this.mnuCampaignGroupView_Click);
      this.mnuCampaignGroupRemove.Name = "mnuCampaignGroupRemove";
      this.mnuCampaignGroupRemove.Size = new Size(117, 22);
      this.mnuCampaignGroupRemove.Text = "Remove";
      this.mnuCampaignGroupRemove.Click += new EventHandler(this.mnuCampaignGroupRemove_Click);
      this.gvContactGroup.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "gvcLastName";
      gvColumn4.Text = "Last Name";
      gvColumn4.Width = 82;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "gvcFirstName";
      gvColumn5.Text = "First Name";
      gvColumn5.Width = 82;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "gvcStatus";
      gvColumn6.Text = "Status";
      gvColumn6.Width = 68;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "gvcOwner";
      gvColumn7.Text = "Owner";
      gvColumn7.Width = 56;
      this.gvContactGroup.Columns.AddRange(new GVColumn[4]
      {
        gvColumn4,
        gvColumn5,
        gvColumn6,
        gvColumn7
      });
      this.gvContactGroup.ContextMenuStrip = this.ctxContactGroup;
      this.gvContactGroup.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvContactGroup.Location = new Point(8, 71);
      this.gvContactGroup.Name = "gvContactGroup";
      this.gvContactGroup.Size = new Size(310, 300);
      this.gvContactGroup.SortOption = GVSortOption.Owner;
      this.gvContactGroup.TabIndex = 70;
      this.gvContactGroup.SelectedIndexChanged += new EventHandler(this.gvContactGroup_SelectedIndexChanged);
      this.gvContactGroup.SortItems += new GVColumnSortEventHandler(this.gvContactGroup_SortItems);
      this.gvContactGroup.DoubleClick += new EventHandler(this.gvContactGroup_DoubleClick);
      this.ctxContactGroup.Items.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.mnuContactGroupView,
        (ToolStripItem) this.mnuContactGroupAdd
      });
      this.ctxContactGroup.Name = "ctxContactGroup";
      this.ctxContactGroup.Size = new Size(100, 48);
      this.ctxContactGroup.Opening += new CancelEventHandler(this.ctxContactGroup_Opening);
      this.mnuContactGroupView.Name = "mnuContactGroupView";
      this.mnuContactGroupView.Size = new Size(99, 22);
      this.mnuContactGroupView.Text = "View";
      this.mnuContactGroupView.Click += new EventHandler(this.mnuContactGroupView_Click);
      this.mnuContactGroupAdd.Name = "mnuContactGroupAdd";
      this.mnuContactGroupAdd.Size = new Size(99, 22);
      this.mnuContactGroupAdd.Text = "Add";
      this.mnuContactGroupAdd.Click += new EventHandler(this.mnuContactGroupAdd_Click);
      this.label1.Anchor = AnchorStyles.Bottom;
      this.label1.Location = new Point(39, 406);
      this.label1.Name = "label1";
      this.label1.Size = new Size(589, 35);
      this.label1.TabIndex = 71;
      this.label1.Text = "Contacts who have opted-out from phone calls, emails, mail and faxes will be automatically screened when you run the campaign.";
      this.label2.Anchor = AnchorStyles.Bottom;
      this.label2.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(5, 406);
      this.label2.Name = "label2";
      this.label2.Size = new Size(36, 18);
      this.label2.TabIndex = 72;
      this.label2.Text = "Note:";
      this.navContactGroup.Font = new Font("Arial", 8f);
      this.navContactGroup.Location = new Point(11, 41);
      this.navContactGroup.Name = "navContactGroup";
      this.navContactGroup.NumberOfItems = 0;
      this.navContactGroup.Size = new Size(307, 24);
      this.navContactGroup.TabIndex = 73;
      this.navContactGroup.PageChangedEvent += new PageListNavigator.PageChangedEventHandler(this.navContactGroup_PageChangedEvent);
      this.Controls.Add((Control) this.navContactGroup);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.gvContactGroup);
      this.Controls.Add((Control) this.gvCampaignGroup);
      this.Controls.Add((Control) this.chkPrimaryOnly);
      this.Controls.Add((Control) this.cboContactGroup);
      this.Controls.Add((Control) this.lblCampaignContacts);
      this.Controls.Add((Control) this.btnRemoveCampaign);
      this.Controls.Add((Control) this.btnAddGroup);
      this.Controls.Add((Control) this.lblContactGroup);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (CampaignGroupControl);
      this.Size = new Size(671, 448);
      this.ctxCampaignGroup.ResumeLayout(false);
      this.ctxContactGroup.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private void navContactGroup_PageChangedEvent(object sender, PageChangedEventArgs e)
    {
      this.loadGVContactGroupList(e.ItemIndex, e.ItemCount);
    }

    private void loadGVContactGroupList(int itemIndex, int itemCount)
    {
      this.Cursor = Cursors.WaitCursor;
      this.gvContactGroup.BeginUpdate();
      try
      {
        int num = 0;
        if (1 == this.gvContactGroup.SelectedItems.Count)
          num = this.campaign.ContactType != EllieMae.EMLite.ContactUI.ContactType.Borrower ? ((ContactGroupMember) this.gvContactGroup.SelectedItems[0].Tag).ContactId : ((ContactGroupMember) this.gvContactGroup.SelectedItems[0].Tag).ContactId;
        this.gvContactGroup.Items.Clear();
        if (-1 == itemIndex || itemCount == 0)
          return;
        object[] items = ((ICursor) this.contactGroupCollection).GetItems(itemIndex, itemCount);
        if (items.Length == 0)
          return;
        GVItem gvItem1 = (GVItem) null;
        foreach (object obj in items)
        {
          GVItem gvItem2 = new GVItem();
          if (this.campaign.ContactType == EllieMae.EMLite.ContactUI.ContactType.Borrower)
          {
            BorrowerGroupMember borrowerGroupMember = BorrowerGroupMember.NewBorrowerGroupMember(this.contactGroupCollection.GroupId, (BorrowerSummaryInfo) obj);
            if (borrowerGroupMember != null)
            {
              gvItem2.SubItems[0].Text = borrowerGroupMember.LastName;
              gvItem2.SubItems[1].Text = borrowerGroupMember.FirstName;
              gvItem2.SubItems[2].Text = borrowerGroupMember.Status;
              gvItem2.SubItems[3].Text = borrowerGroupMember.OwnerId;
              gvItem2.Tag = (object) borrowerGroupMember;
            }
            else
              continue;
          }
          else
          {
            if (this.catUtil == null)
              this.catUtil = new BizCategoryUtil(Session.SessionObjects);
            PartnerGroupMember partnerGroupMember = PartnerGroupMember.NewPartnerGroupMember(this.contactGroupCollection.GroupId, (BizPartnerSummaryInfo) obj);
            if (partnerGroupMember != null)
            {
              gvItem2.SubItems[0].Text = partnerGroupMember.LastName;
              gvItem2.SubItems[1].Text = partnerGroupMember.FirstName;
              gvItem2.SubItems[2].Text = this.catUtil.CategoryIdToName(partnerGroupMember.CategoryId);
              gvItem2.SubItems[3].Text = partnerGroupMember.CompanyName;
              gvItem2.Tag = (object) partnerGroupMember;
            }
            else
              continue;
          }
          if (gvItem1 == null)
            gvItem1 = gvItem2;
          if (num == ((ContactGroupMember) gvItem2.Tag).ContactId)
            gvItem1 = gvItem2;
          this.gvContactGroup.Items.Add(gvItem2);
        }
        if (gvItem1 == null)
          return;
        gvItem1.Selected = true;
        this.gvContactGroup.EnsureVisible(gvItem1.Index);
      }
      finally
      {
        this.gvContactGroup.EndUpdate();
        this.Cursor = Cursors.Default;
      }
    }
  }
}
