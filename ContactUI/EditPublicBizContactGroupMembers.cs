// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.EditPublicBizContactGroupMembers
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.ClientServer;
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
  public class EditPublicBizContactGroupMembers : Form
  {
    private BizPartnerReportFieldDefs contactFieldDefs;
    private GridViewLayoutManager gvLayoutManager;
    private ContactView currentView;
    private GridViewReportFilterManager gvFilterManager;
    private FieldFilterList advFilter;
    private ICursor contactCursor;
    private PageChangedEventArgs currentPagingArgument;
    private BizPartnerLoanReportFieldDefs contactLoanFieldDefs;
    private IContainer components;
    private Panel panel1;
    private GradientPanel gradientPanel1;
    private GroupContainer groupContainer1;
    private PageListNavigator navContacts;
    private GridView gvContactList;
    private Button btnCancel;
    private Button btnAdd;
    private Label label1;
    private Label lblFilter;
    private Button btnClear;
    private Button btnAdvSearch;

    public EditPublicBizContactGroupMembers()
    {
      this.InitializeComponent();
      this.initialGVContactList();
    }

    public int[] SelectedContactIDs
    {
      get
      {
        List<int> intList = new List<int>();
        foreach (GVItem selectedItem in this.gvContactList.SelectedItems)
          intList.Add(((BizPartnerSummaryInfo) selectedItem.Tag).ContactID);
        return intList.ToArray();
      }
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
      this.setCurrentView(new ContactView("DefaultView")
      {
        Layout = this.getDefaultTableLayout(),
        Filter = new FieldFilterList()
      });
    }

    private void gvFilterManager_FilterChanged(object sender, EventArgs e)
    {
      this.populateContacts(this.generateCriterion());
    }

    public void SetCurrentFilter(FieldFilterList filter)
    {
      this.advFilter = filter;
      this.gvFilterManager.ClearColumnFilters();
      this.buildQuerySummary(true);
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

    private void buildQuerySummary(bool populateLabel)
    {
      FieldFilterList fieldFilterList = this.gvFilterManager.ToFieldFilterList();
      if (this.advFilter != null)
        fieldFilterList.AddRange((IEnumerable<FieldFilter>) this.advFilter);
      this.lblFilter.Text = fieldFilterList.ToString(true);
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

    private void btnAdvSearch_Click(object sender, EventArgs e)
    {
      FieldFilterList fieldFilterList = this.gvFilterManager.ToFieldFilterList();
      if (this.advFilter != null)
        fieldFilterList.Merge(this.advFilter);
      if (this.contactLoanFieldDefs == null)
        this.contactLoanFieldDefs = BizPartnerLoanReportFieldDefs.GetFieldDefs(ContactType.PublicBiz);
      using (ContactAdvSearchDialog contactAdvSearchDialog = new ContactAdvSearchDialog((ReportFieldDefs) this.contactLoanFieldDefs, fieldFilterList))
      {
        if (contactAdvSearchDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.SetCurrentFilter(contactAdvSearchDialog.GetSelectedFilter());
      }
    }

    private void btnClear_Click(object sender, EventArgs e)
    {
      this.SetCurrentFilter((FieldFilterList) null);
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
      if (this.gvContactList.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select a contact to add to the group.");
      }
      else
        this.DialogResult = DialogResult.OK;
    }

    private void gvContactList_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      this.btnAdd_Click((object) null, (EventArgs) null);
    }

    private void EditPublicBizContactGroupMembers_KeyDown(object sender, KeyEventArgs e)
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
      this.panel1 = new Panel();
      this.groupContainer1 = new GroupContainer();
      this.navContacts = new PageListNavigator();
      this.gvContactList = new GridView();
      this.gradientPanel1 = new GradientPanel();
      this.btnClear = new Button();
      this.btnAdvSearch = new Button();
      this.lblFilter = new Label();
      this.label1 = new Label();
      this.btnCancel = new Button();
      this.btnAdd = new Button();
      this.panel1.SuspendLayout();
      this.groupContainer1.SuspendLayout();
      this.gradientPanel1.SuspendLayout();
      this.SuspendLayout();
      this.panel1.Controls.Add((Control) this.groupContainer1);
      this.panel1.Controls.Add((Control) this.gradientPanel1);
      this.panel1.Location = new Point(13, 22);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(728, 354);
      this.panel1.TabIndex = 0;
      this.groupContainer1.Controls.Add((Control) this.navContacts);
      this.groupContainer1.Controls.Add((Control) this.gvContactList);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.Location = new Point(0, 32);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(728, 322);
      this.groupContainer1.TabIndex = 4;
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
      this.gvContactList.Size = new Size(726, 295);
      this.gvContactList.TabIndex = 2;
      this.gvContactList.ItemDoubleClick += new GVItemEventHandler(this.gvContactList_ItemDoubleClick);
      this.gradientPanel1.Controls.Add((Control) this.btnClear);
      this.gradientPanel1.Controls.Add((Control) this.btnAdvSearch);
      this.gradientPanel1.Controls.Add((Control) this.lblFilter);
      this.gradientPanel1.Controls.Add((Control) this.label1);
      this.gradientPanel1.Dock = DockStyle.Top;
      this.gradientPanel1.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel1.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel1.Location = new Point(0, 0);
      this.gradientPanel1.Name = "gradientPanel1";
      this.gradientPanel1.Size = new Size(728, 32);
      this.gradientPanel1.Style = GradientPanel.PanelStyle.PageSubHeader;
      this.gradientPanel1.TabIndex = 0;
      this.btnClear.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnClear.Location = new Point(640, 4);
      this.btnClear.Name = "btnClear";
      this.btnClear.Size = new Size(75, 23);
      this.btnClear.TabIndex = 3;
      this.btnClear.Text = "Clear";
      this.btnClear.UseVisualStyleBackColor = true;
      this.btnClear.Click += new EventHandler(this.btnClear_Click);
      this.btnAdvSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAdvSearch.Location = new Point(516, 4);
      this.btnAdvSearch.Name = "btnAdvSearch";
      this.btnAdvSearch.Size = new Size(118, 23);
      this.btnAdvSearch.TabIndex = 2;
      this.btnAdvSearch.Text = "Advanced Search";
      this.btnAdvSearch.UseVisualStyleBackColor = true;
      this.btnAdvSearch.Click += new EventHandler(this.btnAdvSearch_Click);
      this.lblFilter.BackColor = Color.Transparent;
      this.lblFilter.Location = new Point(42, 8);
      this.lblFilter.Name = "lblFilter";
      this.lblFilter.Size = new Size(468, 20);
      this.lblFilter.TabIndex = 1;
      this.label1.AutoSize = true;
      this.label1.BackColor = Color.Transparent;
      this.label1.Location = new Point(3, 9);
      this.label1.Name = "label1";
      this.label1.Size = new Size(32, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Filter:";
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(666, 384);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 7;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnAdd.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnAdd.Location = new Point(585, 384);
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new Size(75, 23);
      this.btnAdd.TabIndex = 6;
      this.btnAdd.Text = "Add";
      this.btnAdd.UseVisualStyleBackColor = true;
      this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(753, 419);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnAdd);
      this.Controls.Add((Control) this.panel1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (EditPublicBizContactGroupMembers);
      this.Text = "Add Contacts";
      this.KeyDown += new KeyEventHandler(this.EditPublicBizContactGroupMembers_KeyDown);
      this.panel1.ResumeLayout(false);
      this.groupContainer1.ResumeLayout(false);
      this.gradientPanel1.ResumeLayout(false);
      this.gradientPanel1.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
