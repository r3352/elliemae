// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.CampaignUI.CampaignAddContactsDlg
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.Calendar;
using EllieMae.EMLite.Campaign;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Campaign;
using EllieMae.EMLite.ClientServer.ContactGroup;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.ContactGroup;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI.CampaignUI
{
  public class CampaignAddContactsDlg : Form
  {
    private EllieMae.EMLite.Campaign.Campaign campaign;
    private ContactGroupCollection contactGroups;
    private ContactGroupMemberCursorCollection contactGroupCollection;
    private SortField[] contactGroupCollectionSortFields;
    private BizCategoryUtil catUtil;
    private ActivityStatusNameProvider activityStatusNames = new ActivityStatusNameProvider();
    private TableContainer tableContainer1;
    private PageListNavigator navContacts;
    private ContactAccessEnumNameProvider accessNames = new ContactAccessEnumNameProvider(typeof (ContactAccess));
    private int selectedItemsCount;
    private int prevSelectedPageIndex = -1;
    private List<object> selectedItems = new List<object>();
    private Dictionary<int, List<object>> selectedItemsPageWise = new Dictionary<int, List<object>>();
    private IContainer components;
    private ComboBox cboContactGroup;
    private Label lblContactGroup;
    private Button btnOK;
    private Button btnCancel;
    private FormattedLabel lblContactsListed;
    private FormattedLabel lblContactsSelected;
    private GridView gvContacts;
    private ContextMenuStrip ctxContactsMenu;
    private ToolStripMenuItem mnuView;
    private ToolStripSeparator toolStripMenuItem1;
    private ToolStripMenuItem mnuSelectAll;

    public CampaignAddContactsDlg(EllieMae.EMLite.Campaign.Campaign campaign)
    {
      this.campaign = campaign;
      Cursor.Current = Cursors.WaitCursor;
      this.InitializeComponent();
      this.populateControls();
      Cursor.Current = Cursors.Default;
    }

    private void populateControls()
    {
      this.lblContactsListed.Text = "Contacts Listed: <b>0</b>";
      this.lblContactsSelected.Text = "Contacts Selected: <b>0</b>";
      this.gvContacts.Columns.Clear();
      if (this.campaign.ContactType == EllieMae.EMLite.ContactUI.ContactType.Borrower)
      {
        this.gvContacts.Columns.Add("Owner", 60, ContentAlignment.MiddleLeft);
        this.gvContacts.Columns.Add("Last Name", 110, ContentAlignment.MiddleLeft);
        this.gvContacts.Columns.Add("First Name", 110, ContentAlignment.MiddleLeft);
        this.gvContacts.Columns.Add("Home Phone", 85, ContentAlignment.MiddleLeft);
        this.gvContacts.Columns.Add("Home Email", 147, ContentAlignment.MiddleLeft);
        this.gvContacts.Columns.Add("Type", 70, ContentAlignment.MiddleLeft);
        this.gvContacts.Columns.Add("Status", 68, ContentAlignment.MiddleLeft);
      }
      else
      {
        this.gvContacts.Columns.Add("Category", 58, ContentAlignment.MiddleLeft);
        this.gvContacts.Columns.Add("Company", 90, ContentAlignment.MiddleLeft);
        this.gvContacts.Columns.Add("Last Name", 110, ContentAlignment.MiddleLeft);
        this.gvContacts.Columns.Add("First Name", 110, ContentAlignment.MiddleLeft);
        this.gvContacts.Columns.Add("Work Phone", 85, ContentAlignment.MiddleLeft);
        this.gvContacts.Columns.Add("Work Email", 147, ContentAlignment.MiddleLeft);
        this.gvContacts.Columns.Add("Public", 50, ContentAlignment.MiddleLeft);
      }
      this.contactGroups = ContactGroupCollection.GetContactGroupCollection(new ContactGroupCollectionCriteria(this.campaign.UserId, this.campaign.ContactType, new ContactGroupType[1]), Session.SessionObjects);
      foreach (EllieMae.EMLite.ContactGroup.ContactGroup contactGroup in (CollectionBase) this.contactGroups)
        this.cboContactGroup.Items.Add((object) contactGroup.GroupName);
      if (this.cboContactGroup.Items.Count <= 0)
        return;
      this.cboContactGroup.SelectedIndex = 0;
    }

    private void getContactGroup()
    {
      Cursor.Current = Cursors.WaitCursor;
      try
      {
        EllieMae.EMLite.ContactGroup.ContactGroup contactGroup = this.contactGroups[this.cboContactGroup.SelectedIndex];
        this.contactGroupCollection = ContactGroupMemberCursorCollection.NewContactGroupMemberCursorCollection(contactGroup.ContactType, contactGroup.GroupId, Session.SessionObjects);
        this.contactGroupCollection.SetSortFields(this.contactGroupCollectionSortFields);
        this.navContacts.NumberOfItems = ((ICursor) this.contactGroupCollection).GetItemCount();
      }
      finally
      {
        Cursor.Current = Cursors.Default;
      }
    }

    private void viewContact()
    {
      if (1 != this.gvContacts.SelectedItems.Count)
        return;
      Cursor.Current = Cursors.WaitCursor;
      ContactGroupMember tag = (ContactGroupMember) this.gvContacts.SelectedItems[0].Tag;
      AttendeeInfo attendeeInfo = new AttendeeInfo();
      attendeeInfo.AssignInfo(tag.ContactId, tag.ContactType);
      Cursor.Current = Cursors.Default;
      int num = (int) attendeeInfo.ShowDialog();
      if (!attendeeInfo.GoToContact)
        return;
      Session.MainScreen.NavigateToContact(attendeeInfo.SelectedContact);
      this.btnCancel.PerformClick();
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
      this.selectedItemsPageWise.Clear();
      this.contactGroupCollectionSortFields = (SortField[]) null;
      this.getContactGroup();
      this.lblContactsListed.Text = "Contacts Listed: <b>" + this.gvContacts.Items.Count.ToString() + "</b>";
      this.lblContactsSelected.Text = "Contacts Selected: <b>" + this.gvContacts.SelectedItems.Count.ToString() + "</b>";
    }

    private void gvContacts_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.lblContactsSelected.Text = "Contacts Selected: <b>" + (this.selectedItemsCount + this.gvContacts.SelectedItems.Count).ToString() + "</b>";
    }

    private void gvContacts_SortItems(object source, GVColumnSortEventArgs e)
    {
      string fieldName = (string) null;
      if (this.campaign.ContactType == EllieMae.EMLite.ContactUI.ContactType.Borrower)
      {
        switch (e.Column)
        {
          case 0:
            fieldName = "Owner.Last_Name";
            break;
          case 1:
            fieldName = "Contact.LastName";
            break;
          case 2:
            fieldName = "Contact.FirstName";
            break;
          case 3:
            fieldName = "Contact.HomePhone";
            break;
          case 4:
            fieldName = "Contact.PersonalEmail";
            break;
          case 5:
            fieldName = "Contact.ContactType";
            break;
          case 6:
            fieldName = "Contact.Status";
            break;
        }
      }
      else
      {
        switch (e.Column)
        {
          case 0:
            fieldName = "BizCategory.CategoryName";
            break;
          case 1:
            fieldName = "Contact.CompanyName";
            break;
          case 2:
            fieldName = "Contact.LastName";
            break;
          case 3:
            fieldName = "Contact.FirstName";
            break;
          case 4:
            fieldName = "Contact.WorkPhone";
            break;
          case 5:
            fieldName = "Contact.BizEmail";
            break;
          case 6:
            fieldName = "Contact.AccessLevel";
            break;
        }
      }
      this.contactGroupCollectionSortFields = new SortField[1];
      this.contactGroupCollectionSortFields[0] = new SortField(fieldName, SortOrder.Ascending == e.SortOrder ? FieldSortOrder.Ascending : FieldSortOrder.Descending);
      this.selectedItemsPageWise.Clear();
      this.getContactGroup();
    }

    private void gvContactGroup_DoubleClick(object sender, EventArgs e) => this.viewContact();

    private void ctxContactsMenu_Opening(object sender, CancelEventArgs e)
    {
      this.mnuView.Enabled = 1 == this.gvContacts.SelectedItems.Count;
      this.mnuSelectAll.Enabled = 0 < this.gvContacts.Items.Count;
    }

    private void mnuView_Click(object sender, EventArgs e) => this.viewContact();

    private void mnuSelectAll_Click(object sender, EventArgs e)
    {
      this.gvContacts.BeginUpdate();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvContacts.Items)
        gvItem.Selected = true;
      this.gvContacts.EndUpdate();
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      Cursor.Current = Cursors.WaitCursor;
      this.gvContacts.BeginUpdate();
      ConfirmReinsertDialog.SelectionTypes selectionTypes = ConfirmReinsertDialog.SelectionTypes.Unknown;
      List<int> intList = new List<int>();
      foreach (KeyValuePair<int, List<object>> keyValuePair in this.selectedItemsPageWise)
      {
        foreach (object obj in keyValuePair.Value)
        {
          intList.Add(((ContactGroupMember) obj).ContactId);
          ContactGroupMember groupMember = (ContactGroupMember) obj;
          CampaignContact campaignContact = this.campaign.CampaignContacts.Find(groupMember.ContactId);
          if (campaignContact == null)
            this.campaign.CampaignContacts.Add(CampaignContact.NewCampaignContact(groupMember.ContactId));
          else if (!campaignContact.HasActivityStatus(ActivityStatus.Expected) && ConfirmReinsertDialog.SelectionTypes.NoAll != selectionTypes)
          {
            if (ConfirmReinsertDialog.SelectionTypes.YesAll != selectionTypes)
            {
              ConfirmReinsertDialog confirmReinsertDialog = new ConfirmReinsertDialog(groupMember);
              int num = (int) confirmReinsertDialog.ShowDialog();
              selectionTypes = confirmReinsertDialog.UserSelection;
            }
            if (ConfirmReinsertDialog.SelectionTypes.Yes == selectionTypes || ConfirmReinsertDialog.SelectionTypes.YesAll == selectionTypes)
              campaignContact.IsReinsert = true;
          }
        }
      }
      this.gvContacts.SelectedItems.Clear();
      this.gvContacts.EndUpdate();
      Cursor.Current = Cursors.Default;
      this.DialogResult = DialogResult.OK;
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
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
      this.lblContactGroup = new Label();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.lblContactsListed = new FormattedLabel();
      this.lblContactsSelected = new FormattedLabel();
      this.gvContacts = new GridView();
      this.ctxContactsMenu = new ContextMenuStrip(this.components);
      this.mnuView = new ToolStripMenuItem();
      this.toolStripMenuItem1 = new ToolStripSeparator();
      this.mnuSelectAll = new ToolStripMenuItem();
      this.tableContainer1 = new TableContainer();
      this.navContacts = new PageListNavigator();
      this.ctxContactsMenu.SuspendLayout();
      this.tableContainer1.SuspendLayout();
      this.SuspendLayout();
      this.cboContactGroup.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboContactGroup.Location = new Point(118, 4);
      this.cboContactGroup.Name = "cboContactGroup";
      this.cboContactGroup.Size = new Size(208, 22);
      this.cboContactGroup.TabIndex = 67;
      this.cboContactGroup.SelectedIndexChanged += new EventHandler(this.cboContactGroup_SelectedIndexChanged);
      this.lblContactGroup.AutoSize = true;
      this.lblContactGroup.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblContactGroup.Location = new Point(10, 10);
      this.lblContactGroup.Name = "lblContactGroup";
      this.lblContactGroup.Size = new Size(106, 14);
      this.lblContactGroup.TabIndex = 68;
      this.lblContactGroup.Text = "View Contact Group";
      this.lblContactGroup.TextAlign = ContentAlignment.MiddleLeft;
      this.btnOK.Location = new Point(544, 344);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(64, 22);
      this.btnOK.TabIndex = 76;
      this.btnOK.Text = "OK";
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(618, 344);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(64, 22);
      this.btnCancel.TabIndex = 77;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.lblContactsListed.Location = new Point(10, 34);
      this.lblContactsListed.Name = "lblContactsListed";
      this.lblContactsListed.Size = new Size(116, 16);
      this.lblContactsListed.TabIndex = 78;
      this.lblContactsListed.Text = "Contacts Listed: <b>9999</b>";
      this.lblContactsSelected.Location = new Point(151, 34);
      this.lblContactsSelected.Name = "lblContactsSelected";
      this.lblContactsSelected.Size = new Size(130, 16);
      this.lblContactsSelected.TabIndex = 79;
      this.lblContactsSelected.Text = "Contacts Selected: <b>9999</b>";
      this.gvContacts.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "gvhOwner";
      gvColumn1.Text = "Owner";
      gvColumn1.Width = 60;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "gvhLastName";
      gvColumn2.Text = "Last Name";
      gvColumn2.Width = 110;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "gvhFirstName";
      gvColumn3.Text = "First Name";
      gvColumn3.Width = 110;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "gvhPhone";
      gvColumn4.Text = "Home Phone";
      gvColumn4.Width = 85;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "gvhEmail";
      gvColumn5.Text = "Home Email";
      gvColumn5.Width = 147;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "gvhType";
      gvColumn6.Text = "Type";
      gvColumn6.Width = 70;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "gvhStatus";
      gvColumn7.Text = "Status";
      gvColumn7.Width = 68;
      this.gvContacts.Columns.AddRange(new GVColumn[7]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6,
        gvColumn7
      });
      this.gvContacts.ContextMenuStrip = this.ctxContactsMenu;
      this.gvContacts.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvContacts.Location = new Point(0, 26);
      this.gvContacts.Name = "gvContacts";
      this.gvContacts.Size = new Size(668, (int) byte.MaxValue);
      this.gvContacts.SortOption = GVSortOption.Owner;
      this.gvContacts.TabIndex = 80;
      this.gvContacts.SelectedIndexChanged += new EventHandler(this.gvContacts_SelectedIndexChanged);
      this.gvContacts.SortItems += new GVColumnSortEventHandler(this.gvContacts_SortItems);
      this.gvContacts.DoubleClick += new EventHandler(this.gvContactGroup_DoubleClick);
      this.ctxContactsMenu.Items.AddRange(new ToolStripItem[3]
      {
        (ToolStripItem) this.mnuView,
        (ToolStripItem) this.toolStripMenuItem1,
        (ToolStripItem) this.mnuSelectAll
      });
      this.ctxContactsMenu.Name = "ctxContactMenu";
      this.ctxContactsMenu.Size = new Size(123, 54);
      this.ctxContactsMenu.Opening += new CancelEventHandler(this.ctxContactsMenu_Opening);
      this.mnuView.Name = "mnuView";
      this.mnuView.Size = new Size(122, 22);
      this.mnuView.Text = "View";
      this.mnuView.Click += new EventHandler(this.mnuView_Click);
      this.toolStripMenuItem1.Name = "toolStripMenuItem1";
      this.toolStripMenuItem1.Size = new Size(119, 6);
      this.mnuSelectAll.Name = "mnuSelectAll";
      this.mnuSelectAll.Size = new Size(122, 22);
      this.mnuSelectAll.Text = "Select All";
      this.mnuSelectAll.Click += new EventHandler(this.mnuSelectAll_Click);
      this.tableContainer1.Controls.Add((Control) this.navContacts);
      this.tableContainer1.Controls.Add((Control) this.gvContacts);
      this.tableContainer1.Location = new Point(13, 56);
      this.tableContainer1.Name = "tableContainer1";
      this.tableContainer1.Size = new Size(668, 282);
      this.tableContainer1.TabIndex = 81;
      this.navContacts.BackColor = Color.Transparent;
      this.navContacts.Font = new Font("Arial", 8f);
      this.navContacts.Location = new Point(0, 2);
      this.navContacts.Name = "navContacts";
      this.navContacts.NumberOfItems = 0;
      this.navContacts.Size = new Size(254, 22);
      this.navContacts.TabIndex = 81;
      this.navContacts.PageChangedEvent += new PageListNavigator.PageChangedEventHandler(this.navContacts_PageChangedEvent);
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(693, 371);
      this.Controls.Add((Control) this.tableContainer1);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.lblContactsListed);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.lblContactGroup);
      this.Controls.Add((Control) this.cboContactGroup);
      this.Controls.Add((Control) this.lblContactsSelected);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (CampaignAddContactsDlg);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Add Contacts";
      this.KeyUp += new KeyEventHandler(this.CampaignAddContactsDlg_KeyUp);
      this.ctxContactsMenu.ResumeLayout(false);
      this.tableContainer1.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private void CampaignAddContactsDlg_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Escape)
        return;
      this.btnCancel.PerformClick();
    }

    private void navContacts_PageChangedEvent(object sender, PageChangedEventArgs e)
    {
      this.selectedItemsCount = 0;
      this.selectedItems.Clear();
      List<object> objectList = (List<object>) null;
      if (this.prevSelectedPageIndex >= 0)
      {
        if (!this.selectedItemsPageWise.TryGetValue(this.prevSelectedPageIndex, out objectList))
          this.selectedItemsPageWise.Add(this.prevSelectedPageIndex, new List<object>());
        else
          this.selectedItemsPageWise[this.prevSelectedPageIndex] = new List<object>();
        foreach (GVItem selectedItem in this.gvContacts.SelectedItems)
          this.selectedItemsPageWise[this.prevSelectedPageIndex].Add(selectedItem.Tag);
        foreach (KeyValuePair<int, List<object>> keyValuePair in this.selectedItemsPageWise)
        {
          if (keyValuePair.Key != this.navContacts.CurrentPageIndex)
            this.selectedItemsCount += keyValuePair.Value.Count;
        }
      }
      this.loadGVContactGroupList(e.ItemIndex, e.ItemCount);
      this.prevSelectedPageIndex = this.navContacts.CurrentPageIndex;
    }

    private void loadGVContactGroupList(int itemIndex, int itemCount)
    {
      this.Cursor = Cursors.WaitCursor;
      this.gvContacts.BeginUpdate();
      List<object> source = (List<object>) null;
      this.selectedItemsPageWise.TryGetValue(this.navContacts.CurrentPageIndex, out source);
      try
      {
        int num = 0;
        if (1 == this.gvContacts.SelectedItems.Count)
          num = this.campaign.ContactType != EllieMae.EMLite.ContactUI.ContactType.Borrower ? ((ContactGroupMember) this.gvContacts.SelectedItems[0].Tag).ContactId : ((ContactGroupMember) this.gvContacts.SelectedItems[0].Tag).ContactId;
        this.gvContacts.Items.Clear();
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
            BorrowerGroupMember borrower = BorrowerGroupMember.NewBorrowerGroupMember(this.contactGroupCollection.GroupId, (BorrowerSummaryInfo) obj);
            if (borrower != null)
            {
              gvItem2.SubItems[0].Text = borrower.LastName;
              gvItem2.SubItems[1].Text = borrower.FirstName;
              gvItem2.SubItems[2].Text = borrower.Status;
              gvItem2.SubItems[3].Text = borrower.OwnerId;
              gvItem2.Tag = (object) borrower;
              if (source != null && source.Any<object>((Func<object, bool>) (item => ((ContactGroupMember) item).ContactId == borrower.ContactId)))
                gvItem2.Selected = true;
            }
            else
              continue;
          }
          else
          {
            if (this.catUtil == null)
              this.catUtil = new BizCategoryUtil(Session.SessionObjects);
            PartnerGroupMember partner = PartnerGroupMember.NewPartnerGroupMember(this.contactGroupCollection.GroupId, (BizPartnerSummaryInfo) obj);
            if (partner != null)
            {
              gvItem2.SubItems[0].Text = partner.LastName;
              gvItem2.SubItems[1].Text = partner.FirstName;
              gvItem2.SubItems[2].Text = this.catUtil.CategoryIdToName(partner.CategoryId);
              gvItem2.SubItems[3].Text = partner.CompanyName;
              gvItem2.Tag = (object) partner;
              if (source != null && source.Any<object>((Func<object, bool>) (item => ((ContactGroupMember) item).ContactId == partner.ContactId)))
                gvItem2.Selected = true;
            }
            else
              continue;
          }
          if (gvItem1 == null)
            gvItem1 = gvItem2;
          if (num == ((ContactGroupMember) gvItem2.Tag).ContactId)
            gvItem1 = gvItem2;
          this.gvContacts.Items.Add(gvItem2);
        }
        if (source == null)
          return;
        this.selectedItemsCount += source.Count;
      }
      finally
      {
        this.gvContacts.EndUpdate();
        this.Cursor = Cursors.Default;
      }
    }

    public class ListViewColumnSorter : IComparer
    {
      private int ColumnToSort;
      private SortOrder OrderOfSort;
      private CaseInsensitiveComparer ObjectCompare;

      public ListViewColumnSorter()
      {
        this.ColumnToSort = 0;
        this.OrderOfSort = SortOrder.None;
        this.ObjectCompare = new CaseInsensitiveComparer();
      }

      public int Compare(object x, object y)
      {
        int num = this.ObjectCompare.Compare((object) ((GVItem) x).SubItems[this.ColumnToSort].Text, (object) ((GVItem) y).SubItems[this.ColumnToSort].Text);
        if (this.OrderOfSort == SortOrder.Ascending)
          return num;
        return this.OrderOfSort == SortOrder.Descending ? -num : 0;
      }

      public int SortColumn
      {
        set => this.ColumnToSort = value;
        get => this.ColumnToSort;
      }

      public SortOrder Order
      {
        set => this.OrderOfSort = value;
        get => this.OrderOfSort;
      }
    }
  }
}
