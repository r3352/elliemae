// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.EditPublicBizContactGroup
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.ContactGroup;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
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
  public class EditPublicBizContactGroup : Form
  {
    private ContactGroupInfo exitingGroup;
    private ContactGroupInfo oriGroup;
    private BizPartnerReportFieldDefs contactFieldDefs;
    private GridViewLayoutManager gvLayoutManager;
    private ContactView currentView;
    private GridViewReportFilterManager gvFilterManager;
    private FieldFilterList advFilter;
    private ICursor contactCursor;
    private PageChangedEventArgs currentPagingArgument;
    private IContainer components;
    private Label label1;
    private TextBox txtGroupName;
    private Label label2;
    private GroupContainer groupContainer1;
    private StandardIconButton siBtnSearch;
    private StandardIconButton siBtnDelete;
    private GridView gvContactList;
    private Button btnSave;
    private Button btnCancel;
    private PageListNavigator navContacts;
    private ToolTip toolTip1;

    public EditPublicBizContactGroup()
      : this((ContactGroupInfo) null)
    {
    }

    public EditPublicBizContactGroup(ContactGroupInfo selectedGroup)
    {
      this.InitializeComponent();
      this.oriGroup = selectedGroup;
      this.exitingGroup = selectedGroup;
      if (this.exitingGroup != (ContactGroupInfo) null)
        this.txtGroupName.Text = this.exitingGroup.GroupName;
      this.initialGVContactList();
    }

    private void setupGVContactList() => this.initialGVContactList();

    private TableLayout getFullTableLayout()
    {
      TableLayout fullTableLayout = new TableLayout();
      foreach (BizPartnerReportFieldDef contactFieldDef in (ReportFieldDefContainer) this.contactFieldDefs)
      {
        if (fullTableLayout.GetColumnByID(contactFieldDef.CriterionFieldName) == null)
          fullTableLayout.AddColumn(new TableLayout.Column(contactFieldDef.CriterionFieldName, contactFieldDef.Name, contactFieldDef.Description, contactFieldDef.FieldType == FieldTypes.IsNumeric ? HorizontalAlignment.Right : HorizontalAlignment.Left, 100));
      }
      fullTableLayout.SortByDescription();
      return fullTableLayout;
    }

    private TableLayout getDefaultTableLayout()
    {
      TableLayout defaultTableLayout = new TableLayout();
      defaultTableLayout.AddColumn(new TableLayout.Column("publicgroupcount.GroupCount", "Group", HorizontalAlignment.Left, 100));
      defaultTableLayout.AddColumn(new TableLayout.Column("Contact.CategoryID", "Category", HorizontalAlignment.Left, 100));
      defaultTableLayout.AddColumn(new TableLayout.Column("Contact.CompanyName", "Company", HorizontalAlignment.Left, 100));
      defaultTableLayout.AddColumn(new TableLayout.Column("Contact.FirstName", "First Name", HorizontalAlignment.Left, 95));
      defaultTableLayout.AddColumn(new TableLayout.Column("Contact.LastName", "Last Name", HorizontalAlignment.Left, 90));
      defaultTableLayout.AddColumn(new TableLayout.Column("Contact.WorkPhone", "Work Phone", HorizontalAlignment.Left, 91));
      return defaultTableLayout;
    }

    private void initialGVContactList()
    {
      this.contactFieldDefs = BizPartnerReportFieldDefs.GetSelectableFieldDefs(Session.DefaultInstance, true, ContactType.PublicBiz);
      this.gvLayoutManager = this.createLayoutManager();
      this.gvFilterManager = new GridViewReportFilterManager(Session.DefaultInstance, this.gvContactList);
      this.gvFilterManager.FilterChanged += new EventHandler(this.gvFilterManager_FilterChanged);
      this.gvFilterManager.CreateColumnFilters((ReportFieldDefs) this.contactFieldDefs);
      ContactView view = new ContactView("DefaultView");
      view.Layout = this.getDefaultTableLayout();
      view.Filter = new FieldFilterList();
      if (this.exitingGroup != (ContactGroupInfo) null)
        view.Filter.Add(new FieldFilter(FieldTypes.IsString, "PublicContactGroupMngr.GroupName", "Group Name", OperatorTypes.Equals, this.exitingGroup.GroupName), JointTokens.And);
      else
        view.Filter.Add(new FieldFilter(FieldTypes.IsString, "PublicContactGroupMngr.GroupName", "Group Name", OperatorTypes.Equals, ""), JointTokens.And);
      this.setCurrentView(view);
    }

    private void gvFilterManager_FilterChanged(object sender, EventArgs e)
    {
      this.populateContacts(this.generateCriterion());
    }

    public void SetCurrentFilter(FieldFilterList filter)
    {
      this.advFilter = filter;
      this.gvFilterManager.ClearColumnFilters();
      this.populateContacts(this.generateCriterion());
    }

    private void applyTableLayout(TableLayout layout)
    {
      if (this.gvLayoutManager == null)
        this.gvLayoutManager = this.createLayoutManager();
      this.gvLayoutManager.ApplyLayout(layout, false);
      if (this.gvFilterManager == null)
      {
        this.gvFilterManager = new GridViewReportFilterManager(Session.DefaultInstance, this.gvContactList);
        this.gvFilterManager.FilterChanged += new EventHandler(this.gvFilterManager_FilterChanged);
      }
      this.gvFilterManager.CreateColumnFilters((ReportFieldDefs) this.contactFieldDefs);
    }

    private QueryCriterion[] generateCriterion()
    {
      QueryCriterion queryCriterion1 = (QueryCriterion) null;
      if (this.advFilter != null)
        queryCriterion1 = this.advFilter.CreateEvaluator().ToQueryCriterion();
      QueryCriterion queryCriterion2 = this.gvFilterManager.ToQueryCriterion();
      if (queryCriterion2 != null)
        queryCriterion1 = queryCriterion1 != null ? queryCriterion1.And(queryCriterion2) : queryCriterion2;
      return new QueryCriterion[1]{ queryCriterion1 };
    }

    private void populateContacts(QueryCriterion[] advCri)
    {
      Cursor.Current = Cursors.WaitCursor;
      try
      {
        if (this.contactCursor != null)
          this.contactCursor.Dispose();
        this.contactCursor = advCri == null || advCri.Length == 0 || advCri[0] == null || !(advCri[0].ToSQLClause() != "") ? Session.ContactManager.OpenPublicBizPartnerCursor(new QueryCriterion[0], RelatedLoanMatchType.None, this.generateSortFields(), this.generateFieldList(), true) : Session.ContactManager.OpenPublicBizPartnerCursor(advCri, RelatedLoanMatchType.None, this.generateSortFields(), this.generateFieldList(), true);
        this.navContacts.NumberOfItems = this.contactCursor.GetItemCount();
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Error loading Contacts: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      finally
      {
        Cursor.Current = Cursors.Default;
      }
    }

    private void navContacts_PageChangedEvent(object sender, PageChangedEventArgs e)
    {
      this.loadGVContactList(e.ItemIndex, e.ItemCount);
      this.currentPagingArgument = e;
    }

    private void loadGVContactList(int itemIndex, int itemCount)
    {
      this.Cursor = Cursors.WaitCursor;
      this.gvContactList.BeginUpdate();
      try
      {
        int contactId = 1 == this.gvContactList.SelectedItems.Count ? ((BizPartnerSummaryInfo) this.gvContactList.SelectedItems[0].Tag).ContactID : 0;
        this.gvContactList.Items.Clear();
        if (-1 == itemIndex || itemCount == 0)
          return;
        object[] items = this.contactCursor.GetItems(itemIndex, itemCount);
        if (items.Length == 0)
          return;
        GVItem gvItem1 = (GVItem) null;
        foreach (object obj in items)
        {
          if (obj is BizPartnerSummaryInfo contactInfo)
          {
            GVItem gvItem2 = this.createGVItem(contactInfo);
            if (gvItem1 == null)
              gvItem1 = gvItem2;
            if (contactId == ((BizPartnerSummaryInfo) gvItem2.Tag).ContactID)
              gvItem1 = gvItem2;
            this.gvContactList.Items.Add(gvItem2);
          }
        }
        if (gvItem1 == null)
          return;
        gvItem1.Selected = true;
        this.gvContactList.EnsureVisible(gvItem1.Index);
      }
      finally
      {
        this.gvContactList.EndUpdate();
        this.Cursor = Cursors.Default;
      }
    }

    private GVItem createGVItem(BizPartnerSummaryInfo contactInfo)
    {
      GVItem gvItem = new GVItem();
      gvItem.Tag = (object) contactInfo;
      for (int index = 0; index < this.gvContactList.Columns.Count; ++index)
      {
        string columnId = ((TableLayout.Column) this.gvContactList.Columns[index].Tag).ColumnID;
        object obj = (object) null;
        BizPartnerReportFieldDef fieldByCriterionName = this.contactFieldDefs.GetFieldByCriterionName(columnId);
        if (fieldByCriterionName != null && contactInfo[columnId] != null)
          obj = ReportFieldClientExtension.ToDisplayElement(fieldByCriterionName, columnId, (IPropertyDictionary) contactInfo, (EventHandler) null);
        gvItem.SubItems[index].Value = obj;
      }
      return gvItem;
    }

    private string[] generateFieldList()
    {
      List<string> stringList = new List<string>();
      foreach (TableLayout.Column column in this.gvLayoutManager.GetCurrentLayout())
      {
        BizPartnerReportFieldDef fieldByCriterionName = this.contactFieldDefs.GetFieldByCriterionName(column.ColumnID);
        if (!stringList.Contains(column.ColumnID))
          stringList.Add(column.ColumnID);
        if (fieldByCriterionName != null)
        {
          foreach (string relatedField in fieldByCriterionName.RelatedFields)
          {
            if (!stringList.Contains(relatedField))
              stringList.Add(relatedField);
          }
        }
      }
      return stringList.ToArray();
    }

    private SortField[] generateSortFields()
    {
      List<SortField> sortFieldList = new List<SortField>();
      foreach (GVColumn column in this.gvContactList.Columns)
      {
        if (column.SortOrder != SortOrder.None)
        {
          TableLayout.Column tag = (TableLayout.Column) column.Tag;
          sortFieldList.Add(new SortField(tag.ColumnID, SortOrder.Ascending == column.SortOrder ? FieldSortOrder.Ascending : FieldSortOrder.Descending));
        }
      }
      return sortFieldList.ToArray();
    }

    private GridViewLayoutManager createLayoutManager()
    {
      GridViewLayoutManager layoutManager = new GridViewLayoutManager(this.gvContactList, this.getFullTableLayout(), this.getDefaultTableLayout());
      layoutManager.LayoutChanged += new EventHandler(this.mngr_LayoutChanged);
      return layoutManager;
    }

    private void mngr_LayoutChanged(object sender, EventArgs e)
    {
      if (this.gvFilterManager != null)
        this.gvFilterManager.CreateColumnFilters((ReportFieldDefs) this.contactFieldDefs);
      this.gvFilterManager.CreateColumnFilters((ReportFieldDefs) this.contactFieldDefs);
      this.populateContacts(this.generateCriterion());
    }

    private void setCurrentView(ContactView view)
    {
      this.currentView = view;
      this.applyTableLayout(view.Layout);
      this.SetCurrentFilter(view.Filter);
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      if (!this.dataValidation())
        return;
      if (this.exitingGroup == (ContactGroupInfo) null)
        this.exitingGroup = new ContactGroupInfo(0, "admin", ContactType.PublicBiz, ContactGroupType.ContactGroup, this.txtGroupName.Text.Trim(), string.Empty, DateTime.Now, new int[0]);
      else
        this.exitingGroup.GroupName = this.txtGroupName.Text;
      Session.ContactGroupManager.SaveContactGroup(this.exitingGroup);
      this.DialogResult = DialogResult.OK;
    }

    private bool dataValidation()
    {
      bool flag = true;
      if (this.txtGroupName.Text.Trim() == "")
      {
        flag = false;
        int num = (int) Utils.Dialog((IWin32Window) this, "Please enter a group name.");
      }
      if (!flag)
        return flag;
      if (this.txtGroupName.Text.Trim().Length > 64)
      {
        flag = false;
        int num = (int) Utils.Dialog((IWin32Window) this, "Group name can not be longer than 64 characters.  Please enter another group name.");
      }
      if (!flag || (!(this.exitingGroup != (ContactGroupInfo) null) || !(this.exitingGroup.GroupName != this.txtGroupName.Text)) && !(this.exitingGroup == (ContactGroupInfo) null))
        return flag;
      foreach (ContactGroupInfo publicBizContactGroup in Session.ContactGroupManager.GetPublicBizContactGroups())
      {
        if ((this.exitingGroup != (ContactGroupInfo) null && this.exitingGroup.GroupId != publicBizContactGroup.GroupId || this.exitingGroup == (ContactGroupInfo) null) && string.Compare(publicBizContactGroup.GroupName, this.txtGroupName.Text, true) == 0)
        {
          flag = false;
          break;
        }
      }
      if (!flag)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "The group '" + this.txtGroupName.Text + "' already exists. Please enter another group name.");
      }
      return flag;
    }

    private void siBtnDelete_Click(object sender, EventArgs e)
    {
      if (this.gvContactList.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select a contact to remove from this contact group.");
      }
      else
      {
        List<int> intList = new List<int>();
        foreach (GVItem selectedItem in this.gvContactList.SelectedItems)
          intList.Add(((BizPartnerSummaryInfo) selectedItem.Tag).ContactID);
        this.exitingGroup.DeletedContactIds = intList.ToArray();
        this.exitingGroup = Session.ContactGroupManager.SaveContactGroup(this.exitingGroup);
        this.initialGVContactList();
      }
    }

    private void siBtnSearch_Click(object sender, EventArgs e)
    {
      if (this.exitingGroup == (ContactGroupInfo) null)
      {
        if (!this.dataValidation())
          return;
        this.exitingGroup = new ContactGroupInfo(0, Session.UserID, ContactType.PublicBiz, ContactGroupType.ContactGroup, this.txtGroupName.Text.Trim(), string.Empty, DateTime.Now, new int[0]);
        this.exitingGroup = Session.ContactGroupManager.SaveContactGroup(this.exitingGroup);
      }
      EditPublicBizContactGroupMembers contactGroupMembers = new EditPublicBizContactGroupMembers();
      if (DialogResult.Cancel == contactGroupMembers.ShowDialog((IWin32Window) this))
        return;
      int[] selectedContactIds = contactGroupMembers.SelectedContactIDs;
      this.exitingGroup.AddedContactIds = selectedContactIds;
      this.exitingGroup = Session.ContactGroupManager.SaveContactGroup(this.exitingGroup);
      Session.ContactManager.MakeBizPartnersPublic(selectedContactIds);
      this.initialGVContactList();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      if (this.exitingGroup != (ContactGroupInfo) null)
      {
        if (this.oriGroup == (ContactGroupInfo) null)
        {
          Session.ContactGroupManager.DeleteContactGroup(this.exitingGroup.GroupId);
        }
        else
        {
          List<int> intList1 = new List<int>();
          List<int> intList2 = new List<int>();
          foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvContactList.Items)
            intList1.Add(((BizPartnerSummaryInfo) gvItem.Tag).ContactID);
          this.oriGroup.DeletedContactIds = intList1.ToArray();
          this.oriGroup.AddedContactIds = this.oriGroup.ContactIds;
          Session.ContactGroupManager.SaveContactGroup(this.oriGroup);
        }
      }
      this.DialogResult = DialogResult.Cancel;
    }

    private void EditPublicBizContactGroup_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Escape)
        return;
      this.btnCancel.PerformClick();
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
      this.label1 = new Label();
      this.txtGroupName = new TextBox();
      this.label2 = new Label();
      this.groupContainer1 = new GroupContainer();
      this.navContacts = new PageListNavigator();
      this.gvContactList = new GridView();
      this.siBtnDelete = new StandardIconButton();
      this.siBtnSearch = new StandardIconButton();
      this.btnSave = new Button();
      this.btnCancel = new Button();
      this.toolTip1 = new ToolTip(this.components);
      this.groupContainer1.SuspendLayout();
      ((ISupportInitialize) this.siBtnDelete).BeginInit();
      ((ISupportInitialize) this.siBtnSearch).BeginInit();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Location = new Point(13, 13);
      this.label1.Name = "label1";
      this.label1.Size = new Size(67, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Group Name";
      this.txtGroupName.Location = new Point(98, 13);
      this.txtGroupName.Name = "txtGroupName";
      this.txtGroupName.Size = new Size(333, 20);
      this.txtGroupName.TabIndex = 1;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(13, 46);
      this.label2.Name = "label2";
      this.label2.Size = new Size(183, 13);
      this.label2.TabIndex = 2;
      this.label2.Text = "Add or remove contacts for the group";
      this.groupContainer1.Controls.Add((Control) this.navContacts);
      this.groupContainer1.Controls.Add((Control) this.gvContactList);
      this.groupContainer1.Controls.Add((Control) this.siBtnDelete);
      this.groupContainer1.Controls.Add((Control) this.siBtnSearch);
      this.groupContainer1.Location = new Point(13, 73);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(636, 322);
      this.groupContainer1.TabIndex = 3;
      this.navContacts.BackColor = Color.Transparent;
      this.navContacts.Font = new Font("Arial", 8f);
      this.navContacts.ItemsPerPage = 100;
      this.navContacts.Location = new Point(0, 2);
      this.navContacts.Name = "navContacts";
      this.navContacts.NumberOfItems = 0;
      this.navContacts.Size = new Size(254, 22);
      this.navContacts.TabIndex = 27;
      this.navContacts.PageChangedEvent += new PageListNavigator.PageChangedEventHandler(this.navContacts_PageChangedEvent);
      this.gvContactList.BorderStyle = BorderStyle.None;
      this.gvContactList.Dock = DockStyle.Fill;
      this.gvContactList.FilterVisible = true;
      this.gvContactList.Location = new Point(1, 26);
      this.gvContactList.Name = "gvContactList";
      this.gvContactList.Size = new Size(634, 295);
      this.gvContactList.TabIndex = 2;
      this.siBtnDelete.BackColor = Color.Transparent;
      this.siBtnDelete.Location = new Point(615, 5);
      this.siBtnDelete.Name = "siBtnDelete";
      this.siBtnDelete.Size = new Size(16, 16);
      this.siBtnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.siBtnDelete.TabIndex = 1;
      this.siBtnDelete.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.siBtnDelete, "Delete Contact");
      this.siBtnDelete.Click += new EventHandler(this.siBtnDelete_Click);
      this.siBtnSearch.BackColor = Color.Transparent;
      this.siBtnSearch.Location = new Point(594, 5);
      this.siBtnSearch.Name = "siBtnSearch";
      this.siBtnSearch.Size = new Size(16, 16);
      this.siBtnSearch.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.siBtnSearch.TabIndex = 0;
      this.siBtnSearch.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.siBtnSearch, "Add Contact");
      this.siBtnSearch.Click += new EventHandler(this.siBtnSearch_Click);
      this.btnSave.Location = new Point(493, 411);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(75, 23);
      this.btnSave.TabIndex = 4;
      this.btnSave.Text = "Save";
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(574, 411);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 5;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(661, 446);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnSave);
      this.Controls.Add((Control) this.groupContainer1);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.txtGroupName);
      this.Controls.Add((Control) this.label1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (EditPublicBizContactGroup);
      this.Text = "Group Details";
      this.KeyDown += new KeyEventHandler(this.EditPublicBizContactGroup_KeyDown);
      this.groupContainer1.ResumeLayout(false);
      ((ISupportInitialize) this.siBtnDelete).EndInit();
      ((ISupportInitialize) this.siBtnSearch).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
