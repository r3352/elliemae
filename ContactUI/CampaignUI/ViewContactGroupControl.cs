// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.CampaignUI.ViewContactGroupControl
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.Calendar;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.ContactGroup;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI.CampaignUI
{
  public class ViewContactGroupControl : UserControl
  {
    private EllieMae.EMLite.ContactGroup.ContactGroup contactGroup;
    private EllieMae.EMLite.ContactGroup.ContactQuery contactQuery;
    private ContactType contactType;
    private ContactGroupMemberCursorCollection contactGroupCollection;
    private SortField[] contactGroupCollectionSortFields;
    private MenuItem mnuView;
    private BizCategoryUtil catUtil;
    private IContainer components;
    private CursorView cvwContactCollection;
    private ColumnHeader chdContactOwner;
    private ColumnHeader chdLastName;
    private ColumnHeader chdFirstName;
    private ColumnHeader chdHomePhone;
    private ColumnHeader chdPersonalEmail;
    private ColumnHeader chdContactType;
    private ColumnHeader chdContactStatus;
    private Label lblContactDescription;
    private Label lblContactCount;
    private ContextMenu ctxContactList;

    public ViewContactGroupControl(EllieMae.EMLite.ContactGroup.ContactGroup contactGroup)
    {
      this.contactGroup = contactGroup;
      this.contactType = contactGroup.ContactType;
      Cursor.Current = Cursors.WaitCursor;
      this.InitializeComponent();
      this.lblContactDescription.Text = "Group contains";
      this.populateControls();
      Cursor.Current = Cursors.Default;
    }

    public ViewContactGroupControl(EllieMae.EMLite.ContactGroup.ContactQuery contactQuery)
    {
      this.contactQuery = contactQuery;
      this.contactType = contactQuery.ContactType;
      Cursor.Current = Cursors.WaitCursor;
      this.InitializeComponent();
      this.lblContactDescription.Text = "Query returned";
      this.populateControls();
      Cursor.Current = Cursors.Default;
    }

    protected void populateControls()
    {
      this.cvwContactCollection.Columns.Clear();
      if (this.contactType == ContactType.Borrower)
      {
        this.cvwContactCollection.Columns.Add("Owner", 56, HorizontalAlignment.Left);
        this.cvwContactCollection.Columns.Add("Last Name", 138, HorizontalAlignment.Left);
        this.cvwContactCollection.Columns.Add("First Name", 138, HorizontalAlignment.Left);
        this.cvwContactCollection.Columns.Add("Home Phone", 84, HorizontalAlignment.Left);
        this.cvwContactCollection.Columns.Add("Home Email", 95, HorizontalAlignment.Left);
        this.cvwContactCollection.Columns.Add("Type", 56, HorizontalAlignment.Left);
        this.cvwContactCollection.Columns.Add("Status", 68, HorizontalAlignment.Left);
      }
      else
      {
        this.cvwContactCollection.Columns.Add("Company", 80, HorizontalAlignment.Left);
        this.cvwContactCollection.Columns.Add("Last Name", 138, HorizontalAlignment.Left);
        this.cvwContactCollection.Columns.Add("First Name", 138, HorizontalAlignment.Left);
        this.cvwContactCollection.Columns.Add("Work Phone", 84, HorizontalAlignment.Left);
        this.cvwContactCollection.Columns.Add("Work Email", 95, HorizontalAlignment.Left);
        this.cvwContactCollection.Columns.Add("Category", 58, HorizontalAlignment.Left);
        this.cvwContactCollection.Columns.Add("Public", 42, HorizontalAlignment.Left);
      }
      this.getContactGroupCollection();
      this.lblContactCount.Text = string.Format("{0} contacts", (object) this.cvwContactCollection.Items.Count);
    }

    private void getContactGroupCollection()
    {
      Cursor.Current = Cursors.WaitCursor;
      try
      {
        if (this.cvwContactCollection.DataSource != null)
        {
          this.cvwContactCollection.DataSource.Dispose();
          this.cvwContactCollection.DataSource = (ICursor) null;
        }
        this.contactGroupCollection = this.contactGroup == null ? ContactGroupMemberCursorCollection.NewContactGroupMemberCursorCollection(this.contactQuery, Session.SessionObjects) : ContactGroupMemberCursorCollection.NewContactGroupMemberCursorCollection(this.contactGroup.ContactType, this.contactGroup.GroupId, Session.SessionObjects);
        this.contactGroupCollection.AddQueryCriteria((QueryCriterion[]) new ArrayList().ToArray(typeof (QueryCriterion)));
        this.contactGroupCollection.SetSortFields(this.contactGroupCollectionSortFields);
        this.cvwContactCollection.DataSource = (ICursor) this.contactGroupCollection;
      }
      finally
      {
        Cursor.Current = Cursors.Default;
      }
    }

    private void viewContact()
    {
      if (1 != this.cvwContactCollection.SelectedItems.Count)
        return;
      Cursor.Current = Cursors.WaitCursor;
      ContactGroupMember tag = (ContactGroupMember) this.cvwContactCollection.SelectedItems[0].Tag;
      AttendeeInfo attendeeInfo = new AttendeeInfo(false);
      attendeeInfo.AssignInfo(tag.ContactId, tag.ContactType, false);
      Cursor.Current = Cursors.Default;
      int num = (int) attendeeInfo.ShowDialog();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void cvwContactCollection_DoubleClick(object sender, EventArgs e) => this.viewContact();

    private void cvwContactCollection_MouseDown(object sender, MouseEventArgs e)
    {
      if (MouseButtons.Right != e.Button)
        return;
      ListViewItem itemAt = this.cvwContactCollection.GetItemAt(e.X, e.Y);
      if (itemAt != null)
        itemAt.Selected = true;
      else
        this.cvwContactCollection.SelectedItems.Clear();
    }

    private void ctxContactList_Popup(object sender, EventArgs e)
    {
      this.mnuView.Enabled = 1 == this.cvwContactCollection.SelectedItems.Count;
    }

    private void mnuView_Click(object sender, EventArgs e) => this.viewContact();

    private void cvwContactCollection_PopulateItem(object sender, PopulateItemEventArgs e)
    {
      if (e.DataItem == null)
        return;
      if (this.contactType == ContactType.Borrower)
      {
        BorrowerGroupMember borrowerGroupMember = BorrowerGroupMember.NewBorrowerGroupMember(this.contactGroupCollection.GroupId, (BorrowerSummaryInfo) e.DataItem);
        e.ListItem.SubItems[0].Text = borrowerGroupMember.OwnerId;
        e.ListItem.SubItems[1].Text = borrowerGroupMember.LastName;
        e.ListItem.SubItems[2].Text = borrowerGroupMember.FirstName;
        e.ListItem.SubItems[3].Text = borrowerGroupMember.PhoneNumber;
        e.ListItem.SubItems[4].Text = borrowerGroupMember.EmailAddress;
        e.ListItem.SubItems[5].Text = borrowerGroupMember.BorrowerType.ToString();
        e.ListItem.SubItems[6].Text = borrowerGroupMember.Status;
        e.ListItem.Tag = (object) borrowerGroupMember;
      }
      else
      {
        if (this.catUtil == null)
          this.catUtil = new BizCategoryUtil(Session.SessionObjects);
        PartnerGroupMember partnerGroupMember = PartnerGroupMember.NewPartnerGroupMember(this.contactGroupCollection.GroupId, (BizPartnerSummaryInfo) e.DataItem);
        e.ListItem.SubItems[0].Text = partnerGroupMember.CompanyName;
        e.ListItem.SubItems[1].Text = partnerGroupMember.LastName;
        e.ListItem.SubItems[2].Text = partnerGroupMember.FirstName;
        e.ListItem.SubItems[3].Text = partnerGroupMember.PhoneNumber;
        e.ListItem.SubItems[4].Text = partnerGroupMember.EmailAddress;
        e.ListItem.SubItems[5].Text = this.catUtil.CategoryIdToName(partnerGroupMember.CategoryId);
        e.ListItem.SubItems[6].Text = ContactAccess.Public == partnerGroupMember.AccessLevel ? "Yes" : "No";
        e.ListItem.Tag = (object) partnerGroupMember;
      }
    }

    private void cvwContactCollection_SortItems(object sender, SortItemsEventArgs e)
    {
      string fieldName = (string) null;
      if (this.contactType == ContactType.Borrower)
      {
        switch (e.ColumnIndex)
        {
          case 0:
            fieldName = "OwnerId";
            break;
          case 1:
            fieldName = "LastName";
            break;
          case 2:
            fieldName = "FirstName";
            break;
          case 3:
            fieldName = "HomePhone";
            break;
          case 4:
            fieldName = "PersonalEmail";
            break;
          case 5:
            fieldName = "ContactType";
            break;
          case 6:
            fieldName = "Status";
            break;
        }
      }
      else
      {
        switch (e.ColumnIndex)
        {
          case 0:
            fieldName = "CompanyName";
            break;
          case 1:
            fieldName = "LastName";
            break;
          case 2:
            fieldName = "FirstName";
            break;
          case 3:
            fieldName = "WorkPhone";
            break;
          case 4:
            fieldName = "BizEmail";
            break;
          case 5:
            fieldName = "CategoryId";
            break;
          case 6:
            fieldName = "AccessLevel";
            break;
        }
      }
      this.contactGroupCollectionSortFields = new SortField[1];
      this.contactGroupCollectionSortFields[0] = new SortField(fieldName, SortOrder.Ascending == e.SortOrder ? FieldSortOrder.Ascending : FieldSortOrder.Descending);
      this.getContactGroupCollection();
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      this.lblContactDescription = new Label();
      this.cvwContactCollection = new CursorView();
      this.chdContactOwner = new ColumnHeader();
      this.chdLastName = new ColumnHeader();
      this.chdFirstName = new ColumnHeader();
      this.chdHomePhone = new ColumnHeader();
      this.chdPersonalEmail = new ColumnHeader();
      this.chdContactType = new ColumnHeader();
      this.chdContactStatus = new ColumnHeader();
      this.ctxContactList = new ContextMenu();
      this.mnuView = new MenuItem();
      this.lblContactCount = new Label();
      this.SuspendLayout();
      this.lblContactDescription.AutoSize = true;
      this.lblContactDescription.Location = new Point(8, 8);
      this.lblContactDescription.Name = "lblContactDescription";
      this.lblContactDescription.Size = new Size(80, 13);
      this.lblContactDescription.TabIndex = 55;
      this.lblContactDescription.Text = "Query returned:";
      this.lblContactDescription.TextAlign = ContentAlignment.MiddleLeft;
      this.cvwContactCollection.AllowColumnReorder = true;
      this.cvwContactCollection.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.cvwContactCollection.Columns.AddRange(new ColumnHeader[7]
      {
        this.chdContactOwner,
        this.chdLastName,
        this.chdFirstName,
        this.chdHomePhone,
        this.chdPersonalEmail,
        this.chdContactType,
        this.chdContactStatus
      });
      this.cvwContactCollection.ContextMenu = this.ctxContactList;
      this.cvwContactCollection.DataSource = (ICursor) null;
      this.cvwContactCollection.DoubleClickActivation = false;
      this.cvwContactCollection.FullRowSelect = true;
      this.cvwContactCollection.GridLines = true;
      this.cvwContactCollection.HideSelection = false;
      this.cvwContactCollection.Location = new Point(7, 32);
      this.cvwContactCollection.MultiSelect = false;
      this.cvwContactCollection.Name = "cvwContactCollection";
      this.cvwContactCollection.Size = new Size(657, 272);
      this.cvwContactCollection.SortColumn = 0;
      this.cvwContactCollection.SortOrder = SortOrder.None;
      this.cvwContactCollection.TabIndex = 56;
      this.cvwContactCollection.UseCompatibleStateImageBehavior = false;
      this.cvwContactCollection.View = View.Details;
      this.cvwContactCollection.PopulateItem += new PopulateItemEventHandler(this.cvwContactCollection_PopulateItem);
      this.cvwContactCollection.DoubleClick += new EventHandler(this.cvwContactCollection_DoubleClick);
      this.cvwContactCollection.MouseDown += new MouseEventHandler(this.cvwContactCollection_MouseDown);
      this.cvwContactCollection.SortItems += new SortItemsEventHandler(this.cvwContactCollection_SortItems);
      this.chdContactOwner.Text = "Owner";
      this.chdLastName.Text = "Last Name";
      this.chdLastName.Width = 138;
      this.chdFirstName.Text = "First Name";
      this.chdFirstName.Width = 138;
      this.chdHomePhone.Text = "Home Phone";
      this.chdHomePhone.Width = 84;
      this.chdPersonalEmail.Text = "Home Email";
      this.chdPersonalEmail.Width = 95;
      this.chdContactType.Text = "Type";
      this.chdContactStatus.Text = "Status";
      this.ctxContactList.MenuItems.AddRange(new MenuItem[1]
      {
        this.mnuView
      });
      this.ctxContactList.Popup += new EventHandler(this.ctxContactList_Popup);
      this.mnuView.Index = 0;
      this.mnuView.Text = "View";
      this.mnuView.Click += new EventHandler(this.mnuView_Click);
      this.lblContactCount.AutoSize = true;
      this.lblContactCount.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblContactCount.Location = new Point(88, 8);
      this.lblContactCount.Name = "lblContactCount";
      this.lblContactCount.Size = new Size(102, 13);
      this.lblContactCount.TabIndex = 57;
      this.lblContactCount.Text = "999999 contacts";
      this.lblContactCount.TextAlign = ContentAlignment.MiddleLeft;
      this.Controls.Add((Control) this.lblContactCount);
      this.Controls.Add((Control) this.cvwContactCollection);
      this.Controls.Add((Control) this.lblContactDescription);
      this.Name = nameof (ViewContactGroupControl);
      this.Size = new Size(671, 319);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
