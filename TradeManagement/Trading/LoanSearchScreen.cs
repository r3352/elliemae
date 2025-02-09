// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.LoanSearchScreen
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Reporting;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class LoanSearchScreen : UserControl, IMenuProvider
  {
    private const string className = "LoanSearchScreen";
    private static readonly string sw = Tracing.SwOutsideLoan;
    public static readonly FileSystemEntry StandardView = new FileSystemEntry("\\Standard View", FileSystemEntry.Types.File, (string) null);
    private LoanReportFieldDefs fieldDefs;
    private FileSystemEntry currentView;
    private GridViewLayoutManager layoutMngr;
    private bool modified;
    private bool suspendEvents;
    private ICursor pipelineCursor;
    private bool forceRefresh;
    private System.Type filterType;
    private string standardViewName = "Standard View";
    private IContainer components;
    private GridView gvResults;
    private Label lblSelWAC;
    private Label label6;
    private Label lblSelAmount;
    private Label label4;
    private Label lblSelCount;
    private Label label1;
    private Button btnCreate;
    private Button btnAssign;
    private AdvancedSearchControl ctlAdvancedSearch;
    private SimpleSearchControl ctlSimpleSearch;
    private GradientPanel gradientPanel1;
    private BorderPanel grpSearch;
    private Label label2;
    private CollapsibleSplitter collapsibleSplitter1;
    private ComboBoxEx cboView;
    private StandardIconButton btnManageViews;
    private StandardIconButton btnResetView;
    private StandardIconButton btnSaveView;
    private TableContainer tableContainer1;
    private PageListNavigator navLoans;
    private ComboBox cboSearchType;
    private ToolTip toolTip1;
    private FlowLayoutPanel flowLayoutPanel1;
    private VerticalSeparator verticalSeparator1;
    private StandardIconButton btnExport;
    private Button btnClearSimple;
    private Button btnClearAdv;
    private ContextMenuStrip mnuLoans;
    private ToolStripMenuItem mnuExport;
    private ToolStripMenuItem mnuExportSelected;
    private ToolStripMenuItem mnuExportAll;
    private ToolStripMenuItem mnuCreateTrade;
    private ToolStripMenuItem mnuAssignToTrade;
    private Button btnAssignToPool;
    private Button btnCreateMbsPool;

    public bool IsReadOnly { get; set; }

    public LoanSearchScreen(System.Type filterType)
    {
      this.InitializeComponent();
      this.filterType = filterType;
      this.fieldDefs = LoanReportFieldDefs.GetFieldDefs(Session.DefaultInstance, LoanReportFieldFlags.AllDatabaseFields);
      this.ctlAdvancedSearch.FieldDefs = (ReportFieldDefs) this.fieldDefs;
      this.layoutMngr = new GridViewLayoutManager(this.gvResults, this.getFullTableLayout());
      this.layoutMngr.ApplyLayout(this.getDefaultTableLayout());
      this.layoutMngr.LayoutChanged += new EventHandler(this.layoutMngr_LayoutChanged);
      this.addSearchButtonsToControls();
      this.loadSearches();
      this.loadDefaultSearch();
    }

    public void RefreshContents(int sqlRead = 0)
    {
      this.btnCreateMbsPool.Visible = this.btnAssignToPool.Visible = Utils.ParseBoolean(Session.StartupInfo.TradeSettings[(object) "Trade.EnableMbsPool"]);
      if (!this.forceRefresh)
        return;
      this.performSearch(sqlRead);
      this.forceRefresh = false;
    }

    public void SetReadOnly()
    {
      this.btnAssign.Enabled = false;
      this.btnCreate.Enabled = false;
      this.btnCreateMbsPool.Enabled = false;
      this.btnAssignToPool.Enabled = false;
    }

    private void addSearchButtonsToControls()
    {
      this.ctlAdvancedSearch.AddControlToHeader((Control) new VerticalSeparator());
      Button c1 = new Button();
      c1.BackColor = SystemColors.Control;
      c1.UseVisualStyleBackColor = true;
      c1.Text = "&Search";
      c1.Height = this.btnClearAdv.Height;
      c1.Margin = new Padding(0);
      c1.Click += new EventHandler(this.btnSearch_Click);
      this.ctlAdvancedSearch.AddControlToHeader((Control) c1);
      this.btnClearAdv.Parent = (Control) null;
      this.ctlAdvancedSearch.AddControlToHeader((Control) this.btnClearAdv);
      Button c2 = new Button();
      c2.BackColor = SystemColors.Control;
      c2.UseVisualStyleBackColor = true;
      c2.Text = "&Search";
      c2.Height = this.btnClearSimple.Height;
      c2.Margin = new Padding(0);
      c2.Click += new EventHandler(this.btnSearch_Click);
      this.ctlSimpleSearch.AddControlToHeader((Control) c2);
      this.btnClearSimple.Parent = (Control) null;
      this.ctlSimpleSearch.AddControlToHeader((Control) this.btnClearSimple);
    }

    private void loadDefaultSearch()
    {
      string uri = Session.GetPrivateProfileString("Trading.LastSearch") ?? "";
      ClientCommonUtils.PopulateDropdown((ComboBox) this.cboView, (object) new FileSystemEntryListItem(FileSystemEntry.Parse("public:\\" + this.standardViewName)), false);
      if (uri != "" && uri != this.standardViewName && ClientCommonUtils.ItemExistInDropDown((ComboBox) this.cboView, (object) new FileSystemEntryListItem(FileSystemEntry.Parse(uri))))
        ClientCommonUtils.PopulateDropdown((ComboBox) this.cboView, (object) new FileSystemEntryListItem(FileSystemEntry.Parse(uri)), false);
      if (this.cboView.SelectedIndex >= 0)
        return;
      this.cboView.SelectedIndex = 0;
    }

    private void loadSearches()
    {
      FileSystemEntry[] settingsFileEntries = Session.ConfigurationManager.GetAllTemplateSettingsFileEntries(EllieMae.EMLite.ClientServer.TemplateSettingsType.TradeFilter, (string) null, false);
      this.suspendEvents = true;
      this.cboView.Items.Clear();
      this.cboView.Dividers.Clear();
      foreach (FileSystemEntry e in settingsFileEntries)
        this.cboView.Items.Add((object) new FileSystemEntryListItem(e));
      this.cboView.Dividers.Add(this.cboView.Items.Count);
      this.cboView.Items.Add((object) new FileSystemEntryListItem(LoanSearchScreen.StandardView));
      if (this.currentView != null)
        ClientCommonUtils.PopulateDropdown((ComboBox) this.cboView, (object) new FileSystemEntryListItem(this.currentView), false);
      this.suspendEvents = false;
    }

    public bool DataModified
    {
      get => this.modified;
      set => this.setViewModified(value);
    }

    private void gvResults_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.IsReadOnly)
        return;
      int count = this.gvResults.SelectedItems.Count;
      this.btnCreate.Enabled = count > 0;
      this.btnCreateMbsPool.Enabled = count > 0;
      this.btnAssign.Enabled = count > 0;
      this.btnExport.Enabled = count > 0;
      this.btnAssignToPool.Enabled = count > 0;
      this.updateSelectedItemData();
    }

    private void gvResults_ColumnReorder(object source, GVColumnEventArgs e)
    {
      this.setViewModified(true);
    }

    private void gvResults_ColumnResize(object source, GVColumnEventArgs e)
    {
      this.setViewModified(true);
    }

    private void gvResults_SortItems(object sender, GVColumnSortEventArgs e)
    {
      this.performSearch(this.getSortOrder(e.ColumnSorts), 0);
      this.setViewModified(true);
    }

    private GVItem createGVItemForPipelineInfo(PipelineInfo pinfo)
    {
      GVItem itemForPipelineInfo = new GVItem();
      foreach (GVColumn column in this.gvResults.Columns)
      {
        int index = column.Index;
        string tag = ((TableLayout.Column) column.Tag).Tag;
        object obj = (object) string.Concat(pinfo.Info[(object) tag]);
        LoanReportFieldDef fieldByCriterionName = this.fieldDefs.GetFieldByCriterionName(tag);
        if (fieldByCriterionName != null)
          obj = fieldByCriterionName.ToDisplayElement(tag, pinfo, (Control) this.gvResults);
        itemForPipelineInfo.SubItems[index].Value = obj;
      }
      itemForPipelineInfo.Tag = (object) pinfo;
      return itemForPipelineInfo;
    }

    private void layoutMngr_LayoutChanged(object sender, EventArgs e) => this.setViewModified(true);

    private void onFilterDataChange(object sender, EventArgs e) => this.setViewModified(true);

    private void btnSearch_Click(object sender, EventArgs e)
    {
      this.performSearch();
      if (this.gvResults.Items.Count != 0)
        return;
      int num = (int) Utils.Dialog((IWin32Window) this, "There are no loans that match the specified criteria.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    }

    private void performSearch(int sqlRead = 0)
    {
      GVColumnSort[] sortOrder = this.gvResults.Columns.GetSortOrder();
      if (sortOrder.Length != 0)
        this.performSearch(this.getSortOrder(sortOrder), sqlRead);
      else
        this.performSearch((SortField[]) null, sqlRead);
    }

    private void performSearch(SortField[] sortFields, int sqlRead)
    {
      Cursor.Current = Cursors.WaitCursor;
      TradeFilter filter = this.createFilter();
      string[] displayFields = this.getDisplayFields();
      this.clearPipelineCursor();
      this.pipelineCursor = Session.LoanManager.OpenPipeline(LoanInfo.Right.Access, displayFields, PipelineData.Trade, sortFields, filter.CreateEvaluator(this.filterType).ToQueryCriterion(), false, sqlRead);
      this.navLoans.NumberOfItems = this.pipelineCursor.GetItemCount(sqlRead);
      this.displayCurrentResultsPage(false, sqlRead);
      this.updateResultSetSummaryData();
      this.updateSelectedItemData();
      Cursor.Current = Cursors.Default;
    }

    private void displayCurrentResultsPage(bool preserveSelections, int sqlRead = 0)
    {
      Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
      if (preserveSelections)
      {
        foreach (GVItem selectedItem in this.gvResults.SelectedItems)
          dictionary[((PipelineInfo) selectedItem.Tag).GUID] = true;
      }
      this.gvResults.Items.Clear();
      PipelineInfo[] pipelineInfoArray = new PipelineInfo[0];
      if (this.navLoans.CurrentPageItemCount > 0)
        pipelineInfoArray = (PipelineInfo[]) this.pipelineCursor.GetItems(this.navLoans.CurrentPageItemIndex, this.navLoans.CurrentPageItemCount, false, sqlRead);
      foreach (PipelineInfo pinfo in pipelineInfoArray)
      {
        GVItem itemForPipelineInfo = this.createGVItemForPipelineInfo(pinfo);
        if (dictionary.ContainsKey(pinfo.GUID))
          itemForPipelineInfo.Selected = true;
        this.gvResults.Items.Add(itemForPipelineInfo);
      }
    }

    private void clearPipelineCursor()
    {
      if (this.pipelineCursor == null)
        return;
      this.pipelineCursor.Dispose();
      this.pipelineCursor = (ICursor) null;
    }

    private void clearResultsList()
    {
      this.gvResults.DataProvider = (IGVDataProvider) null;
      this.updateResultSetSummaryData();
      this.updateSelectedItemData();
    }

    private void updateResultSetSummaryData()
    {
    }

    private void updateSelectedItemData()
    {
      this.lblSelCount.Text = this.gvResults.SelectedItems.Count.ToString();
      Decimal num1 = 0M;
      Decimal num2 = 0M;
      foreach (GVItem selectedItem in this.gvResults.SelectedItems)
      {
        PipelineInfo tag = (PipelineInfo) selectedItem.Tag;
        Decimal num3 = Utils.ParseDecimal(tag.Info[(object) "Loan.TotalLoanAmount"], 0M);
        Decimal num4 = Utils.ParseDecimal(tag.Info[(object) "Loan.LoanRate"], 0M);
        num1 += num3;
        num2 += num3 * (num4 / 100M);
      }
      this.lblSelAmount.Text = "$" + num1.ToString("#,##0.00");
      if (num1 > 0M)
        this.lblSelWAC.Text = (num2 / num1 * 100M).ToString("0.000") + "%";
      else
        this.lblSelWAC.Text = "N/A";
    }

    private SortField[] getSortOrder(GVColumnSort[] sortColumns)
    {
      List<SortField> sortFieldList = new List<SortField>();
      for (int index = 0; index < sortColumns.Length; ++index)
      {
        string tag = ((TableLayout.Column) this.gvResults.Columns[sortColumns[index].Column].Tag).Tag;
        FieldSortOrder sortOrder = sortColumns[index].SortOrder == SortOrder.Ascending ? FieldSortOrder.Ascending : FieldSortOrder.Descending;
        LoanReportFieldDef fieldByCriterionName = this.fieldDefs.GetFieldByCriterionName(tag);
        DataConversion conversion = DataConversion.None;
        SortField sortField = fieldByCriterionName == null ? new SortField(tag, sortOrder, conversion) : new SortField(fieldByCriterionName.SortTerm, sortOrder, fieldByCriterionName.DataConversion);
        sortFieldList.Add(sortField);
      }
      return sortFieldList.ToArray();
    }

    private string[] getDisplayFields()
    {
      List<string> stringList = new List<string>();
      stringList.Add("Loan.Guid");
      stringList.Add("Loan.LoanRate");
      stringList.Add("Loan.TotalLoanAmount");
      foreach (GVColumn column in this.gvResults.Columns)
      {
        string tag = ((TableLayout.Column) column.Tag).Tag;
        if (tag != "" && !stringList.Contains(tag))
          stringList.Add(tag);
        LoanReportFieldDef fieldByCriterionName = this.fieldDefs.GetFieldByCriterionName(tag);
        if (fieldByCriterionName != null && fieldByCriterionName.RelatedFields != null)
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

    private TradeFilter createFilter()
    {
      return this.cboSearchType.SelectedIndex == 0 ? new TradeFilter(this.ctlSimpleSearch.GetCurrentFilter(), this.getCurrentResultsLayout()) : new TradeFilter(this.ctlAdvancedSearch.GetCurrentFilter(), this.getCurrentResultsLayout());
    }

    private TableLayout getCurrentResultsLayout() => this.layoutMngr.GetCurrentLayout();

    private void cboSearchType_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.cboSearchType.SelectedIndex == 0 && this.ctlAdvancedSearch.Visible)
      {
        FieldFilterList currentFilter = this.ctlAdvancedSearch.GetCurrentFilter();
        if (currentFilter.Count > 0 && Utils.Dialog((IWin32Window) this.ParentForm, "If you switch to Simple Search, you will lose all of your search criteria.", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK)
        {
          this.cboSearchType.SelectedIndex = 1;
        }
        else
        {
          this.switchToSimpleMode(new SimpleTradeFilter());
          if (currentFilter.Count <= 0)
            return;
          this.setViewModified(true);
        }
      }
      else
      {
        if (this.cboSearchType.SelectedIndex != 1 || !this.ctlSimpleSearch.Visible)
          return;
        this.switchToAdvancedMode(this.ctlSimpleSearch.GetCurrentFilter().ConvertToFilterList());
      }
    }

    private void setViewModified(bool value)
    {
      this.modified = this.btnSaveView.Enabled = this.btnResetView.Enabled = value;
    }

    private void switchToSimpleMode(SimpleTradeFilter filter)
    {
      bool modified = this.modified;
      this.ctlSimpleSearch.SetCurrentFilter(filter);
      this.clearResultsList();
      this.ctlSimpleSearch.Visible = true;
      this.ctlAdvancedSearch.Visible = false;
      this.cboSearchType.SelectedIndex = 0;
      this.setViewModified(modified);
    }

    private void switchToAdvancedMode(FieldFilterList filters)
    {
      bool modified = this.modified;
      this.ctlAdvancedSearch.SetCurrentFilter(filters);
      this.ctlAdvancedSearch.Visible = true;
      this.ctlSimpleSearch.Visible = false;
      this.cboSearchType.SelectedIndex = 1;
      this.setViewModified(modified);
    }

    private void btnSave_Click(object sender, EventArgs e) => this.saveSearch();

    private bool saveSearch()
    {
      using (SaveSearchDialog saveSearchDialog = new SaveSearchDialog(this.createFilter(), this.currentView, this.currentView != LoanSearchScreen.StandardView))
      {
        if (saveSearchDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return false;
        this.setViewModified(false);
        if (!saveSearchDialog.SelectedEntry.Equals((object) this.currentView))
        {
          this.currentView = saveSearchDialog.SelectedEntry;
          this.loadSearches();
        }
      }
      return true;
    }

    private void loadView(FileSystemEntry viewEntry)
    {
      if (viewEntry == LoanSearchScreen.StandardView)
      {
        this.startNewSearch();
      }
      else
      {
        if (viewEntry == null)
          return;
        this.loadSavedSearch(viewEntry);
      }
    }

    private void loadSavedSearch(FileSystemEntry fsEntry)
    {
      try
      {
        TradeFilterTemplate templateSettings = (TradeFilterTemplate) Session.ConfigurationManager.GetTemplateSettings(EllieMae.EMLite.ClientServer.TemplateSettingsType.TradeFilter, fsEntry);
        if (templateSettings == null)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "The specified search cannot be opened.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else
        {
          if (templateSettings.Filter.FilterType == TradeFilterType.Advanced)
            this.switchToAdvancedMode(templateSettings.Filter.GetAdvancedFilter());
          else
            this.switchToSimpleMode(templateSettings.Filter.GetSimpleFilter());
          this.currentView = fsEntry;
          if (templateSettings.Filter.DataLayout != null)
          {
            TableLayout dataLayout = templateSettings.Filter.DataLayout;
            this.validateTableLayout(dataLayout);
            this.layoutMngr.ApplyLayout(dataLayout);
          }
          this.setViewModified(false);
        }
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "An unexpected error occurred attempting to open the specified search: " + (object) ex, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }

    private void validateTableLayout(TableLayout layout)
    {
      List<TableLayout.Column> columnList = new List<TableLayout.Column>();
      foreach (TableLayout.Column column in layout)
      {
        ReportFieldDef fieldByCriterionName = (ReportFieldDef) this.fieldDefs.GetFieldByCriterionName(column.ColumnID);
        if (fieldByCriterionName != null)
          column.Title = fieldByCriterionName.Description;
        else
          columnList.Add(column);
      }
      foreach (TableLayout.Column column in columnList)
        layout.Remove(column);
    }

    private void startNewSearch()
    {
      this.clearResultsList();
      this.ctlAdvancedSearch.ClearFilters();
      this.ctlSimpleSearch.ClearFilter();
      this.cboSearchType.SelectedIndex = 0;
      this.createInitialSearchFilters();
      this.layoutMngr.ApplyLayout(this.getDefaultTableLayout(), false);
      this.setViewModified(false);
      this.currentView = LoanSearchScreen.StandardView;
    }

    private TradeFilterTemplate createStandardView()
    {
      return new TradeFilterTemplate(LoanSearchScreen.StandardView.Name, "", new TradeFilter(new SimpleTradeFilter(), this.getDefaultTableLayout()));
    }

    private FieldFilterList getStandardViewFilters()
    {
      FieldFilterList standardViewFilters = new FieldFilterList();
      standardViewFilters.Add(new FieldFilter(EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsOptionList, "2031", "Loan.InvestorStatus", "Investor Status", OperatorTypes.IsAnyOf, new string[1]
      {
        ""
      }, "Unassigned"));
      return standardViewFilters;
    }

    private void createInitialSearchFilters()
    {
      if (!this.ctlAdvancedSearch.Visible)
        return;
      this.ctlAdvancedSearch.AddFilters(this.getStandardViewFilters());
    }

    private bool confirmSaveCurrentSearch()
    {
      if (!this.modified)
        return true;
      switch (Utils.Dialog((IWin32Window) this, "The current search has been modified. Would you like to save these changes now?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
      {
        case DialogResult.Cancel:
          return false;
        case DialogResult.No:
          return true;
        default:
          return this.saveSearch();
      }
    }

    private string[] getSelectedLoanGuids()
    {
      List<string> stringList = new List<string>();
      foreach (GVItem selectedItem in this.gvResults.SelectedItems)
        stringList.Add(((PipelineInfo) selectedItem.Tag).GUID);
      return stringList.ToArray();
    }

    private void btnCreate_Click(object sender, EventArgs e)
    {
      LoanTradeInfo trade = new LoanTradeInfo();
      trade.Filter = this.createFilter();
      TradeManagementConsole.Instance.OpenTrade(trade, this.getSelectedLoanGuids(), 0);
    }

    private void btnCreatePool_Click(object sender, EventArgs e)
    {
      MbsPoolMortgageType poolMortgageType = MbsPoolMortgageType.None;
      using (MbsPoolMortgageTypeDialog mortgageTypeDialog = new MbsPoolMortgageTypeDialog())
      {
        if (mortgageTypeDialog.ShowDialog((IWin32Window) this) == DialogResult.OK)
          poolMortgageType = mortgageTypeDialog.PoolMortgageType;
      }
      if (poolMortgageType == MbsPoolMortgageType.None)
        return;
      MbsPoolInfo trade = new MbsPoolInfo();
      trade.PoolMortgageType = poolMortgageType;
      trade.Filter = this.createFilter();
      TradeManagementConsole.Instance.OpenMbsPool(trade, this.getSelectedLoanGuids());
    }

    private void btnAssign_Click(object sender, EventArgs e)
    {
      if (this.gvResults.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must first select at least one loan from the search results.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        bool flag = false;
        string str = "trade";
        foreach (GVItem selectedItem in this.gvResults.SelectedItems)
        {
          if (!object.Equals(((PipelineInfo) selectedItem.Tag).GetField("InvestorStatus"), (object) string.Empty) && !object.Equals(((PipelineInfo) selectedItem.Tag).GetField("InvestorStatus"), (object) "Rejected"))
          {
            flag = true;
            break;
          }
        }
        TradeType tradeType = TradeType.LoanTrade;
        if (sender != null && sender is Button && ((Control) sender).Name == "btnAssignToPool")
        {
          tradeType = TradeType.MbsPool;
          str = "pool";
        }
        if (flag && Utils.Dialog((IWin32Window) this, "One or more of the selected loans are already assigned to another trade or pool. Do you want to remove those loans from their existing trades/pools and assign them to this " + str + "?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
          return;
        using (TradeSelector tradeSelector = new TradeSelector(tradeType))
        {
          if (tradeSelector.ShowDialog((IWin32Window) this) != DialogResult.OK)
            return;
          TradeInfoObj trade = this.getTrade(tradeSelector.SelectedTrade.TradeID, tradeType);
          if (trade == null)
            return;
          switch (tradeType)
          {
            case TradeType.LoanTrade:
              TradeManagementConsole.Instance.OpenTrade(trade as LoanTradeInfo, this.getSelectedLoanGuids(), 0);
              break;
            case TradeType.MbsPool:
              TradeManagementConsole.Instance.OpenMbsPool(trade as MbsPoolInfo, this.getSelectedLoanGuids());
              break;
          }
          this.forceRefresh = true;
        }
      }
    }

    private TradeInfoObj getTrade(int tradeId, TradeType tradeType)
    {
      TradeInfoObj trade = (TradeInfoObj) null;
      switch (tradeType)
      {
        case TradeType.LoanTrade:
          trade = (TradeInfoObj) Session.LoanTradeManager.GetTrade(tradeId);
          break;
        case TradeType.MbsPool:
          trade = (TradeInfoObj) Session.MbsPoolManager.GetTrade(tradeId);
          break;
      }
      if (trade == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The specified trade can no longer be found.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      return trade;
    }

    private void btnExport_Click(object sender, EventArgs e)
    {
      if (this.gvResults.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must first select at least one loan from the search results.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
        this.exportLoans(false);
    }

    private bool exportLoans(bool exportAll)
    {
      if (this.gvResults.Columns.Count > ExcelHandler.GetMaximumColumnCount())
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You pipeline cannot be exported because the number of columns exceeds the limit supported by Excel (" + (object) ExcelHandler.GetMaximumColumnCount() + ")", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      if ((!exportAll ? this.gvResults.SelectedItems.Count : this.pipelineCursor.GetItemCount()) > ExcelHandler.GetMaximumRowCount() - 1)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You pipeline cannot be exported because the number of rows exceeds the limit supported by Excel (" + (object) ExcelHandler.GetMaximumRowCount() + ")", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      if (exportAll)
        this.exportAllLoansToExcel();
      else
        this.exportSelectedRowsToExcel();
      return true;
    }

    private void exportSelectedRowsToExcel()
    {
      ExcelHandler excelHandler = new ExcelHandler();
      excelHandler.AddDataTable(this.gvResults, (ReportFieldDefs) this.fieldDefs, true);
      excelHandler.CreateExcel();
    }

    private void exportAllLoansToExcel()
    {
      using (CursorActivator.Wait())
      {
        ExcelHandler excelHandler = new ExcelHandler();
        foreach (GVColumn c in this.gvResults.Columns.DisplaySequence)
          excelHandler.AddHeaderColumn(c.Text, excelHandler.GetColumnFormat(c, (ReportFieldDefs) this.fieldDefs));
        foreach (PipelineInfo pinfo in new EnumerableCursor(this.pipelineCursor, false))
        {
          GVItem itemForPipelineInfo = this.createGVItemForPipelineInfo(pinfo);
          string[] data = new string[this.gvResults.Columns.Count];
          for (int index = 0; index < this.gvResults.Columns.Count; ++index)
            data[index] = itemForPipelineInfo.SubItems[this.gvResults.Columns.DisplaySequence[index].Index].Text;
          excelHandler.AddDataRow(data);
        }
        excelHandler.CreateExcel();
      }
    }

    private TableLayout getDefaultTableLayout()
    {
      TableLayout defaultTableLayout = new TableLayout();
      defaultTableLayout.AddColumn(new TableLayout.Column("Loan.LoanNumber", "Loan Number", HorizontalAlignment.Left, 103));
      defaultTableLayout.AddColumn(new TableLayout.Column("Loan.InvestorStatus", "Investor Status", HorizontalAlignment.Left, 103));
      defaultTableLayout.AddColumn(new TableLayout.Column("Trade.Name", "Trade ID", HorizontalAlignment.Left, 103));
      defaultTableLayout.AddColumn(new TableLayout.Column("Loan.LoanProgram", "Loan Program", HorizontalAlignment.Left, 94));
      defaultTableLayout.AddColumn(new TableLayout.Column("Loan.CurrentMilestoneName", "Last Finished Milestone", HorizontalAlignment.Left, 103));
      defaultTableLayout.AddColumn(new TableLayout.Column("Loan.TotalLoanAmount", "Loan Amount", HorizontalAlignment.Right, 103));
      defaultTableLayout.AddColumn(new TableLayout.Column("Loan.LoanRate", "Note Rate", HorizontalAlignment.Right, 68));
      defaultTableLayout.AddColumn(new TableLayout.Column("Loan.Term", "Term", HorizontalAlignment.Right, 56));
      defaultTableLayout.AddColumn(new TableLayout.Column("Loan.LTV", "LTV", HorizontalAlignment.Right, 52));
      defaultTableLayout.AddColumn(new TableLayout.Column("Loan.CLTV", "CLTV", HorizontalAlignment.Right, 52));
      defaultTableLayout.AddColumn(new TableLayout.Column("Loan.DTITop", "Top", HorizontalAlignment.Right, 50));
      defaultTableLayout.AddColumn(new TableLayout.Column("Loan.DTIBottom", "Bottom", HorizontalAlignment.Right, 50));
      defaultTableLayout.AddColumn(new TableLayout.Column("Loan.CreditScore", "FICO", HorizontalAlignment.Right, 56));
      defaultTableLayout.AddColumn(new TableLayout.Column("Loan.OccupancyStatus", "Occupancy Type", HorizontalAlignment.Left, 101));
      defaultTableLayout.AddColumn(new TableLayout.Column("Loan.PropertyType", "Property Type", HorizontalAlignment.Left, 82));
      defaultTableLayout.AddColumn(new TableLayout.Column("Loan.State", "State", HorizontalAlignment.Left, 56));
      defaultTableLayout.AddColumn(new TableLayout.Column("Loan.LockExpirationDate", "Lock Expiration Date", HorizontalAlignment.Left, 124));
      defaultTableLayout.AddColumn(new TableLayout.Column("Loan.BorrowerLastName", "Last Name", HorizontalAlignment.Left, 104));
      return defaultTableLayout;
    }

    private TableLayout getFullTableLayout()
    {
      TableLayout defaultTableLayout = this.getDefaultTableLayout();
      foreach (LoanReportFieldDef fieldDef in (ReportFieldDefContainer) this.fieldDefs)
        defaultTableLayout.AddColumn(ReportFieldClientExtension.ToTableLayoutColumn(fieldDef));
      defaultTableLayout.SortByDescription();
      return defaultTableLayout;
    }

    private void cboView_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.suspendEvents || this.cboView.SelectedIndex < 0)
        return;
      FileSystemEntry entry = ((FileSystemEntryListItem) this.cboView.SelectedItem).Entry;
      if (this.currentView != null && !this.confirmSaveCurrentSearch())
      {
        this.suspendEvents = true;
        this.cboView.SelectedItem = (object) new FileSystemEntryListItem(this.currentView);
        this.suspendEvents = false;
      }
      else
      {
        this.cboView.SelectedItem = (object) new FileSystemEntryListItem(entry);
        if (entry.Equals((object) this.currentView))
          return;
        this.loadView(entry);
      }
    }

    private void navLoans_PageChangedEvent(object sender, PageChangedEventArgs e)
    {
      if (this.suspendEvents)
        return;
      this.displayCurrentResultsPage(false);
    }

    private void btnClearAdv_Click(object sender, EventArgs e)
    {
      this.ctlAdvancedSearch.ClearFilters();
      this.setViewModified(true);
    }

    private void btnClearSimple_Click(object sender, EventArgs e)
    {
      this.ctlSimpleSearch.ClearFilter();
      this.setViewModified(true);
    }

    private void btnResetView_Click(object sender, EventArgs e)
    {
      if (Utils.Dialog((IWin32Window) this, "Are you sure you want to reset the selected view?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      this.loadView(this.currentView);
    }

    private void btnManageViews_Click(object sender, EventArgs e)
    {
      using (ViewManagementDialog managementDialog = new ViewManagementDialog(EllieMae.EMLite.ClientServer.TemplateSettingsType.TradeFilter, true, "Trading.LastSearch"))
      {
        managementDialog.AddStaticView((BinaryConvertibleObject) this.createStandardView());
        int num = (int) managementDialog.ShowDialog((IWin32Window) this);
      }
      this.loadSearches();
      if (this.cboView.SelectedIndex >= 0)
        return;
      this.cboView.SelectedIndex = 0;
    }

    private void gvResults_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      PipelineInfo tag = e.Item.Tag as PipelineInfo;
      Session.Application.GetService<ILoanConsole>()?.OpenLoan(tag.GUID);
    }

    public void MenuClicked(ToolStripItem menuItem)
    {
      string s = string.Concat(menuItem.Tag);
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(s))
      {
        case 160265298:
          if (!(s == "LS_Clear"))
            break;
          if (this.cboSearchType.SelectedIndex == 0)
          {
            this.btnClearSimple_Click((object) null, (EventArgs) null);
            break;
          }
          this.btnClearAdv_Click((object) null, (EventArgs) null);
          break;
        case 388291461:
          if (!(s == "LS_CreateMbsPool"))
            break;
          this.btnCreatePool_Click((object) null, (EventArgs) null);
          break;
        case 460533273:
          if (!(s == "LS_Search"))
            break;
          this.btnSearch_Click((object) null, (EventArgs) null);
          break;
        case 1175634098:
          if (!(s == "LS_ExportSelected"))
            break;
          this.btnExport_Click((object) null, (EventArgs) null);
          break;
        case 1314960649:
          if (!(s == "LS_AssignToTrade"))
            break;
          this.btnAssign_Click((object) null, (EventArgs) null);
          break;
        case 1488349913:
          if (!(s == "LS_SaveView"))
            break;
          this.btnSave_Click((object) null, (EventArgs) null);
          break;
        case 1904734220:
          if (!(s == "LS_ManageViews"))
            break;
          this.btnManageViews_Click((object) null, (EventArgs) null);
          break;
        case 2134042785:
          if (!(s == "LS_ResetView"))
            break;
          this.btnResetView_Click((object) null, (EventArgs) null);
          break;
        case 2399732406:
          if (!(s == "LS_ExportAll"))
            break;
          this.mnuExportAll_Click((object) null, (EventArgs) null);
          break;
        case 3098948477:
          if (!(s == "LS_AssignToMbsPool"))
            break;
          this.btnAssign_Click((object) this.btnAssignToPool, (EventArgs) null);
          break;
        case 3410666481:
          if (!(s == "LS_CreateTrade"))
            break;
          this.btnCreate_Click((object) null, (EventArgs) null);
          break;
      }
    }

    public bool SetMenuItemState(ToolStripItem menuItem)
    {
      Control stateControl = (Control) null;
      switch (string.Concat(menuItem.Tag))
      {
        case "LS_AssignToMbsPool":
          stateControl = (Control) this.btnAssignToPool;
          break;
        case "LS_AssignToTrade":
          stateControl = (Control) this.btnAssign;
          break;
        case "LS_Clear":
          stateControl = this.cboSearchType.SelectedIndex == 0 ? (Control) this.btnClearSimple : (Control) this.btnClearAdv;
          break;
        case "LS_CreateMbsPool":
          stateControl = (Control) this.btnCreateMbsPool;
          break;
        case "LS_CreateTrade":
          stateControl = (Control) this.btnCreate;
          break;
        case "LS_ExportAll":
          menuItem.Enabled = this.gvResults.Items.Count > 0;
          return true;
        case "LS_ExportSelected":
          stateControl = (Control) this.btnExport;
          break;
        case "LS_ManageViews":
          stateControl = (Control) this.btnManageViews;
          break;
        case "LS_ResetView":
          stateControl = (Control) this.btnResetView;
          break;
        case "LS_SaveView":
          stateControl = (Control) this.btnSaveView;
          break;
        case "LS_Search":
          return true;
      }
      if (stateControl == null)
        return true;
      ClientCommonUtils.ApplyControlStateToMenu(menuItem, stateControl);
      return stateControl.Visible;
    }

    private void mnuLoans_Opening(object sender, CancelEventArgs e)
    {
      this.mnuCreateTrade.Enabled = this.btnCreate.Enabled;
      this.mnuAssignToTrade.Enabled = this.btnAssign.Enabled;
      this.mnuExportSelected.Enabled = this.btnExport.Enabled;
      this.mnuExportAll.Enabled = this.gvResults.Items.Count > 0;
    }

    private void mnuExportAll_Click(object sender, EventArgs e) => this.exportLoans(true);

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
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
      GVColumn gvColumn8 = new GVColumn();
      GVColumn gvColumn9 = new GVColumn();
      GVColumn gvColumn10 = new GVColumn();
      GVColumn gvColumn11 = new GVColumn();
      GVColumn gvColumn12 = new GVColumn();
      GVColumn gvColumn13 = new GVColumn();
      GVColumn gvColumn14 = new GVColumn();
      GVColumn gvColumn15 = new GVColumn();
      GVColumn gvColumn16 = new GVColumn();
      this.toolTip1 = new ToolTip(this.components);
      this.btnExport = new StandardIconButton();
      this.btnManageViews = new StandardIconButton();
      this.btnResetView = new StandardIconButton();
      this.btnSaveView = new StandardIconButton();
      this.tableContainer1 = new TableContainer();
      this.lblSelWAC = new Label();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.btnAssignToPool = new Button();
      this.btnAssign = new Button();
      this.btnCreateMbsPool = new Button();
      this.btnCreate = new Button();
      this.verticalSeparator1 = new VerticalSeparator();
      this.lblSelCount = new Label();
      this.label1 = new Label();
      this.navLoans = new PageListNavigator();
      this.gvResults = new GridView();
      this.mnuLoans = new ContextMenuStrip(this.components);
      this.mnuExport = new ToolStripMenuItem();
      this.mnuExportSelected = new ToolStripMenuItem();
      this.mnuExportAll = new ToolStripMenuItem();
      this.mnuCreateTrade = new ToolStripMenuItem();
      this.mnuAssignToTrade = new ToolStripMenuItem();
      this.label6 = new Label();
      this.lblSelAmount = new Label();
      this.label4 = new Label();
      this.collapsibleSplitter1 = new CollapsibleSplitter();
      this.grpSearch = new BorderPanel();
      this.btnClearAdv = new Button();
      this.btnClearSimple = new Button();
      this.cboSearchType = new ComboBox();
      this.ctlSimpleSearch = new SimpleSearchControl();
      this.ctlAdvancedSearch = new AdvancedSearchControl();
      this.gradientPanel1 = new GradientPanel();
      this.cboView = new ComboBoxEx();
      this.label2 = new Label();
      ((ISupportInitialize) this.btnExport).BeginInit();
      ((ISupportInitialize) this.btnManageViews).BeginInit();
      ((ISupportInitialize) this.btnResetView).BeginInit();
      ((ISupportInitialize) this.btnSaveView).BeginInit();
      this.tableContainer1.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      this.mnuLoans.SuspendLayout();
      this.grpSearch.SuspendLayout();
      this.gradientPanel1.SuspendLayout();
      this.SuspendLayout();
      this.btnExport.BackColor = Color.Transparent;
      this.btnExport.Enabled = false;
      this.btnExport.Location = new Point(85, 3);
      this.btnExport.MouseDownImage = (Image) null;
      this.btnExport.Name = "btnExport";
      this.btnExport.Size = new Size(16, 16);
      this.btnExport.StandardButtonType = StandardIconButton.ButtonType.ExcelButton;
      this.btnExport.TabIndex = 3;
      this.btnExport.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnExport, "Export to Excel");
      this.btnExport.Click += new EventHandler(this.btnExport_Click);
      this.btnManageViews.BackColor = Color.Transparent;
      this.btnManageViews.Location = new Point(375, 7);
      this.btnManageViews.MouseDownImage = (Image) null;
      this.btnManageViews.Name = "btnManageViews";
      this.btnManageViews.Size = new Size(16, 16);
      this.btnManageViews.StandardButtonType = StandardIconButton.ButtonType.ManageButton;
      this.btnManageViews.TabIndex = 12;
      this.btnManageViews.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnManageViews, "Manage Views");
      this.btnManageViews.Click += new EventHandler(this.btnManageViews_Click);
      this.btnResetView.BackColor = Color.Transparent;
      this.btnResetView.Location = new Point(355, 7);
      this.btnResetView.MouseDownImage = (Image) null;
      this.btnResetView.Name = "btnResetView";
      this.btnResetView.Size = new Size(16, 16);
      this.btnResetView.StandardButtonType = StandardIconButton.ButtonType.ResetButton;
      this.btnResetView.TabIndex = 11;
      this.btnResetView.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnResetView, "Reset View");
      this.btnResetView.Click += new EventHandler(this.btnResetView_Click);
      this.btnSaveView.BackColor = Color.Transparent;
      this.btnSaveView.Location = new Point(335, 7);
      this.btnSaveView.MouseDownImage = (Image) null;
      this.btnSaveView.Name = "btnSaveView";
      this.btnSaveView.Size = new Size(16, 16);
      this.btnSaveView.StandardButtonType = StandardIconButton.ButtonType.SaveButton;
      this.btnSaveView.TabIndex = 10;
      this.btnSaveView.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnSaveView, "Save View");
      this.btnSaveView.Click += new EventHandler(this.btnSave_Click);
      this.tableContainer1.Borders = AnchorStyles.Top;
      this.tableContainer1.Controls.Add((Control) this.lblSelWAC);
      this.tableContainer1.Controls.Add((Control) this.flowLayoutPanel1);
      this.tableContainer1.Controls.Add((Control) this.lblSelCount);
      this.tableContainer1.Controls.Add((Control) this.label1);
      this.tableContainer1.Controls.Add((Control) this.navLoans);
      this.tableContainer1.Controls.Add((Control) this.gvResults);
      this.tableContainer1.Controls.Add((Control) this.label6);
      this.tableContainer1.Controls.Add((Control) this.lblSelAmount);
      this.tableContainer1.Controls.Add((Control) this.label4);
      this.tableContainer1.Dock = DockStyle.Fill;
      this.tableContainer1.Location = new Point(0, 279);
      this.tableContainer1.Name = "tableContainer1";
      this.tableContainer1.Size = new Size(787, 275);
      this.tableContainer1.TabIndex = 13;
      this.lblSelWAC.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.lblSelWAC.AutoSize = true;
      this.lblSelWAC.BackColor = Color.Transparent;
      this.lblSelWAC.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblSelWAC.Location = new Point(523, 256);
      this.lblSelWAC.Name = "lblSelWAC";
      this.lblSelWAC.RightToLeft = RightToLeft.Yes;
      this.lblSelWAC.Size = new Size(25, 14);
      this.lblSelWAC.TabIndex = 19;
      this.lblSelWAC.Text = "N/A";
      this.flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.flowLayoutPanel1.BackColor = Color.Transparent;
      this.flowLayoutPanel1.Controls.Add((Control) this.btnAssignToPool);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnAssign);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnCreateMbsPool);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnCreate);
      this.flowLayoutPanel1.Controls.Add((Control) this.verticalSeparator1);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnExport);
      this.flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel1.Location = new Point(266, 2);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new Size(517, 22);
      this.flowLayoutPanel1.TabIndex = 21;
      this.btnAssignToPool.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAssignToPool.BackColor = SystemColors.Control;
      this.btnAssignToPool.Enabled = false;
      this.btnAssignToPool.Location = new Point(406, 0);
      this.btnAssignToPool.Margin = new Padding(0);
      this.btnAssignToPool.Name = "btnAssignToPool";
      this.btnAssignToPool.Size = new Size(111, 22);
      this.btnAssignToPool.TabIndex = 5;
      this.btnAssignToPool.Text = "Assign to MBS Pool";
      this.btnAssignToPool.UseVisualStyleBackColor = true;
      this.btnAssignToPool.Click += new EventHandler(this.btnAssign_Click);
      this.btnAssign.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAssign.BackColor = SystemColors.Control;
      this.btnAssign.Enabled = false;
      this.btnAssign.Location = new Point(301, 0);
      this.btnAssign.Margin = new Padding(0);
      this.btnAssign.Name = "btnAssign";
      this.btnAssign.Size = new Size(105, 22);
      this.btnAssign.TabIndex = 2;
      this.btnAssign.Text = "Assign to Trade";
      this.btnAssign.UseVisualStyleBackColor = true;
      this.btnAssign.Click += new EventHandler(this.btnAssign_Click);
      this.btnCreateMbsPool.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnCreateMbsPool.BackColor = SystemColors.Control;
      this.btnCreateMbsPool.Enabled = false;
      this.btnCreateMbsPool.Location = new Point(201, 0);
      this.btnCreateMbsPool.Margin = new Padding(0);
      this.btnCreateMbsPool.Name = "btnCreateMbsPool";
      this.btnCreateMbsPool.Size = new Size(100, 22);
      this.btnCreateMbsPool.TabIndex = 6;
      this.btnCreateMbsPool.Text = "Create MBS Pool";
      this.btnCreateMbsPool.UseVisualStyleBackColor = true;
      this.btnCreateMbsPool.Click += new EventHandler(this.btnCreatePool_Click);
      this.btnCreate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnCreate.BackColor = SystemColors.Control;
      this.btnCreate.Enabled = false;
      this.btnCreate.Location = new Point(112, 0);
      this.btnCreate.Margin = new Padding(0);
      this.btnCreate.Name = "btnCreate";
      this.btnCreate.Size = new Size(89, 22);
      this.btnCreate.TabIndex = 1;
      this.btnCreate.Text = "Create Trade";
      this.btnCreate.UseVisualStyleBackColor = true;
      this.btnCreate.Click += new EventHandler(this.btnCreate_Click);
      this.verticalSeparator1.Location = new Point(107, 3);
      this.verticalSeparator1.MaximumSize = new Size(2, 16);
      this.verticalSeparator1.MinimumSize = new Size(2, 16);
      this.verticalSeparator1.Name = "verticalSeparator1";
      this.verticalSeparator1.Size = new Size(2, 16);
      this.verticalSeparator1.TabIndex = 4;
      this.verticalSeparator1.Text = "verticalSeparator1";
      this.lblSelCount.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.lblSelCount.AutoSize = true;
      this.lblSelCount.BackColor = Color.Transparent;
      this.lblSelCount.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblSelCount.Location = new Point(108, 256);
      this.lblSelCount.Name = "lblSelCount";
      this.lblSelCount.Size = new Size(13, 14);
      this.lblSelCount.TabIndex = 15;
      this.lblSelCount.Text = "0";
      this.label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.label1.AutoSize = true;
      this.label1.BackColor = Color.Transparent;
      this.label1.Location = new Point(4, 256);
      this.label1.Name = "label1";
      this.label1.Size = new Size(107, 14);
      this.label1.TabIndex = 14;
      this.label1.Text = "# of Loans Selected:";
      this.navLoans.BackColor = Color.Transparent;
      this.navLoans.Font = new Font("Arial", 8f);
      this.navLoans.Location = new Point(0, 2);
      this.navLoans.Name = "navLoans";
      this.navLoans.NumberOfItems = 0;
      this.navLoans.Size = new Size(254, 22);
      this.navLoans.TabIndex = 20;
      this.navLoans.PageChangedEvent += new PageListNavigator.PageChangedEventHandler(this.navLoans_PageChangedEvent);
      this.gvResults.AllowColumnReorder = true;
      this.gvResults.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Tag = (object) "Loan.LoanNumber";
      gvColumn1.Text = "Loan #";
      gvColumn1.Width = 112;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Tag = (object) "Loan.BorrowerLastName";
      gvColumn2.Text = "Last Name";
      gvColumn2.Width = 107;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Tag = (object) "Loan.InvestorStatus";
      gvColumn3.Text = "Investor Status";
      gvColumn3.Width = 95;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column4";
      gvColumn4.Tag = (object) "Trade.Name";
      gvColumn4.Text = "Trade ID";
      gvColumn4.Width = 85;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column5";
      gvColumn5.Tag = (object) "Loan.LoanProgram";
      gvColumn5.Text = "Loan Program";
      gvColumn5.Width = 93;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Column6";
      gvColumn6.Tag = (object) "Loan.CurrentMilestoneName";
      gvColumn6.Text = "Last Completed Milestone";
      gvColumn6.Width = 100;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "Column7";
      gvColumn7.Tag = (object) "Loan.TotalLoanAmount";
      gvColumn7.Text = "Loan Amount";
      gvColumn7.Width = 91;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "Column8";
      gvColumn8.Tag = (object) "Loan.LoanRate";
      gvColumn8.Text = "Note Rate";
      gvColumn8.Width = 70;
      gvColumn9.ImageIndex = -1;
      gvColumn9.Name = "Column9";
      gvColumn9.Tag = (object) "Loan.Term";
      gvColumn9.Text = "Term";
      gvColumn9.Width = 61;
      gvColumn10.ImageIndex = -1;
      gvColumn10.Name = "Column10";
      gvColumn10.Tag = (object) "Loan.LTV";
      gvColumn10.Text = "LTV";
      gvColumn10.Width = 100;
      gvColumn11.ImageIndex = -1;
      gvColumn11.Name = "Column11";
      gvColumn11.Tag = (object) "Loan.DTITop";
      gvColumn11.Text = "DTI";
      gvColumn11.Width = 100;
      gvColumn12.ImageIndex = -1;
      gvColumn12.Name = "Column12";
      gvColumn12.Tag = (object) "Loan.CreditScore";
      gvColumn12.Text = "FICO";
      gvColumn12.Width = 63;
      gvColumn13.ImageIndex = -1;
      gvColumn13.Name = "Column13";
      gvColumn13.Tag = (object) "Loan.OccupancyStatus";
      gvColumn13.Text = "Occupancy";
      gvColumn13.Width = 82;
      gvColumn14.ImageIndex = -1;
      gvColumn14.Name = "Column14";
      gvColumn14.Tag = (object) "Loan.PropertyType";
      gvColumn14.Text = "Property Type";
      gvColumn14.Width = 90;
      gvColumn15.ImageIndex = -1;
      gvColumn15.Name = "Column15";
      gvColumn15.Tag = (object) "Loan.State";
      gvColumn15.Text = "State";
      gvColumn15.Width = 48;
      gvColumn16.ImageIndex = -1;
      gvColumn16.Name = "Column16";
      gvColumn16.Tag = (object) "Loan.LockExpirationDate";
      gvColumn16.Text = "Lock Expiration Date";
      gvColumn16.Width = 120;
      this.gvResults.Columns.AddRange(new GVColumn[16]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6,
        gvColumn7,
        gvColumn8,
        gvColumn9,
        gvColumn10,
        gvColumn11,
        gvColumn12,
        gvColumn13,
        gvColumn14,
        gvColumn15,
        gvColumn16
      });
      this.gvResults.ContextMenuStrip = this.mnuLoans;
      this.gvResults.Dock = DockStyle.Fill;
      this.gvResults.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvResults.Location = new Point(0, 26);
      this.gvResults.Name = "gvResults";
      this.gvResults.Size = new Size(787, 224);
      this.gvResults.SortOption = GVSortOption.Owner;
      this.gvResults.TabIndex = 3;
      this.gvResults.SelectedIndexChanged += new EventHandler(this.gvResults_SelectedIndexChanged);
      this.gvResults.ColumnReorder += new GVColumnEventHandler(this.gvResults_ColumnReorder);
      this.gvResults.ColumnResize += new GVColumnEventHandler(this.gvResults_ColumnResize);
      this.gvResults.SortItems += new GVColumnSortEventHandler(this.gvResults_SortItems);
      this.gvResults.ItemDoubleClick += new GVItemEventHandler(this.gvResults_ItemDoubleClick);
      this.mnuLoans.Items.AddRange(new ToolStripItem[3]
      {
        (ToolStripItem) this.mnuExport,
        (ToolStripItem) this.mnuCreateTrade,
        (ToolStripItem) this.mnuAssignToTrade
      });
      this.mnuLoans.Name = "mnuLoans";
      this.mnuLoans.ShowImageMargin = false;
      this.mnuLoans.Size = new Size(141, 70);
      this.mnuLoans.Opening += new CancelEventHandler(this.mnuLoans_Opening);
      this.mnuExport.DropDownItems.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.mnuExportSelected,
        (ToolStripItem) this.mnuExportAll
      });
      this.mnuExport.Name = "mnuExport";
      this.mnuExport.Size = new Size(140, 22);
      this.mnuExport.Text = "Export to Excel";
      this.mnuExportSelected.Name = "mnuExportSelected";
      this.mnuExportSelected.Size = new Size(199, 22);
      this.mnuExportSelected.Text = "Selected Loans Only...";
      this.mnuExportSelected.Click += new EventHandler(this.btnExport_Click);
      this.mnuExportAll.Name = "mnuExportAll";
      this.mnuExportAll.Size = new Size(199, 22);
      this.mnuExportAll.Text = "All Loans on All Pages...";
      this.mnuExportAll.Click += new EventHandler(this.mnuExportAll_Click);
      this.mnuCreateTrade.Name = "mnuCreateTrade";
      this.mnuCreateTrade.Size = new Size(140, 22);
      this.mnuCreateTrade.Text = "Create Trade...";
      this.mnuCreateTrade.Click += new EventHandler(this.btnCreate_Click);
      this.mnuAssignToTrade.Name = "mnuAssignToTrade";
      this.mnuAssignToTrade.Size = new Size(140, 22);
      this.mnuAssignToTrade.Text = "Assign to Trade...";
      this.mnuAssignToTrade.Click += new EventHandler(this.btnAssign_Click);
      this.label6.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.label6.AutoSize = true;
      this.label6.BackColor = Color.Transparent;
      this.label6.Location = new Point(378, 256);
      this.label6.Name = "label6";
      this.label6.Size = new Size(147, 14);
      this.label6.TabIndex = 18;
      this.label6.Text = "Weighted Average (Coupon):";
      this.lblSelAmount.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.lblSelAmount.AutoSize = true;
      this.lblSelAmount.BackColor = Color.Transparent;
      this.lblSelAmount.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblSelAmount.Location = new Point(271, 256);
      this.lblSelAmount.Name = "lblSelAmount";
      this.lblSelAmount.Size = new Size(19, 14);
      this.lblSelAmount.TabIndex = 17;
      this.lblSelAmount.Text = "$0";
      this.label4.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.label4.AutoSize = true;
      this.label4.BackColor = Color.Transparent;
      this.label4.Location = new Point(174, 256);
      this.label4.Name = "label4";
      this.label4.Size = new Size(98, 14);
      this.label4.TabIndex = 16;
      this.label4.Text = "Total Loan Amount:";
      this.collapsibleSplitter1.AnimationDelay = 20;
      this.collapsibleSplitter1.AnimationStep = 20;
      this.collapsibleSplitter1.BorderStyle3D = Border3DStyle.Flat;
      this.collapsibleSplitter1.ControlToHide = (Control) this.grpSearch;
      this.collapsibleSplitter1.Dock = DockStyle.Top;
      this.collapsibleSplitter1.ExpandParentForm = false;
      this.collapsibleSplitter1.Location = new Point(0, 272);
      this.collapsibleSplitter1.Name = "collapsibleSplitter1";
      this.collapsibleSplitter1.TabIndex = 11;
      this.collapsibleSplitter1.TabStop = false;
      this.collapsibleSplitter1.UseAnimations = false;
      this.collapsibleSplitter1.VisualStyle = VisualStyles.Encompass;
      this.grpSearch.Borders = AnchorStyles.Bottom;
      this.grpSearch.Controls.Add((Control) this.btnClearAdv);
      this.grpSearch.Controls.Add((Control) this.btnClearSimple);
      this.grpSearch.Controls.Add((Control) this.cboSearchType);
      this.grpSearch.Controls.Add((Control) this.ctlSimpleSearch);
      this.grpSearch.Controls.Add((Control) this.ctlAdvancedSearch);
      this.grpSearch.Dock = DockStyle.Top;
      this.grpSearch.Location = new Point(0, 31);
      this.grpSearch.Name = "grpSearch";
      this.grpSearch.Size = new Size(787, 241);
      this.grpSearch.TabIndex = 10;
      this.btnClearAdv.BackColor = SystemColors.Control;
      this.btnClearAdv.Location = new Point(488, 1);
      this.btnClearAdv.Margin = new Padding(0);
      this.btnClearAdv.Name = "btnClearAdv";
      this.btnClearAdv.Size = new Size(75, 22);
      this.btnClearAdv.TabIndex = 14;
      this.btnClearAdv.Text = "C&lear";
      this.btnClearAdv.UseVisualStyleBackColor = true;
      this.btnClearAdv.Click += new EventHandler(this.btnClearAdv_Click);
      this.btnClearSimple.BackColor = SystemColors.Control;
      this.btnClearSimple.Location = new Point(412, 1);
      this.btnClearSimple.Margin = new Padding(0);
      this.btnClearSimple.Name = "btnClearSimple";
      this.btnClearSimple.Size = new Size(75, 22);
      this.btnClearSimple.TabIndex = 13;
      this.btnClearSimple.Text = "C&lear";
      this.btnClearSimple.UseVisualStyleBackColor = true;
      this.btnClearSimple.Click += new EventHandler(this.btnClearSimple_Click);
      this.cboSearchType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboSearchType.FormattingEnabled = true;
      this.cboSearchType.Items.AddRange(new object[2]
      {
        (object) "Simple Search",
        (object) "Advanced Search"
      });
      this.cboSearchType.Location = new Point(116, 1);
      this.cboSearchType.Name = "cboSearchType";
      this.cboSearchType.Size = new Size(151, 22);
      this.cboSearchType.TabIndex = 10;
      this.cboSearchType.SelectedIndexChanged += new EventHandler(this.cboSearchType_SelectedIndexChanged);
      this.ctlSimpleSearch.BackColor = SystemColors.Window;
      this.ctlSimpleSearch.Borders = AnchorStyles.None;
      this.ctlSimpleSearch.DataModified = false;
      this.ctlSimpleSearch.Dock = DockStyle.Fill;
      this.ctlSimpleSearch.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.ctlSimpleSearch.Location = new Point(0, 0);
      this.ctlSimpleSearch.Name = "ctlSimpleSearch";
      this.ctlSimpleSearch.NoteRateMax = "";
      this.ctlSimpleSearch.NoteRateMin = "";
      this.ctlSimpleSearch.ReadOnly = false;
      this.ctlSimpleSearch.Size = new Size(787, 240);
      this.ctlSimpleSearch.TabIndex = 2;
      this.ctlSimpleSearch.Title = "Search Type";
      this.ctlSimpleSearch.DataChange += new EventHandler(this.onFilterDataChange);
      this.ctlAdvancedSearch.AllowDynamicOperators = false;
      this.ctlAdvancedSearch.Borders = AnchorStyles.None;
      this.ctlAdvancedSearch.Dock = DockStyle.Fill;
      this.ctlAdvancedSearch.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.ctlAdvancedSearch.Location = new Point(0, 0);
      this.ctlAdvancedSearch.Name = "ctlAdvancedSearch";
      this.ctlAdvancedSearch.Size = new Size(787, 240);
      this.ctlAdvancedSearch.TabIndex = 1;
      this.ctlAdvancedSearch.Title = "Filters";
      this.ctlAdvancedSearch.Visible = false;
      this.ctlAdvancedSearch.DataChange += new EventHandler(this.onFilterDataChange);
      this.gradientPanel1.BackColorGlassyStyle = true;
      this.gradientPanel1.Borders = AnchorStyles.Bottom;
      this.gradientPanel1.Controls.Add((Control) this.btnManageViews);
      this.gradientPanel1.Controls.Add((Control) this.btnResetView);
      this.gradientPanel1.Controls.Add((Control) this.btnSaveView);
      this.gradientPanel1.Controls.Add((Control) this.cboView);
      this.gradientPanel1.Controls.Add((Control) this.label2);
      this.gradientPanel1.Dock = DockStyle.Top;
      this.gradientPanel1.GradientColor1 = Color.FromArgb(81, 123, 184);
      this.gradientPanel1.GradientColor2 = Color.FromArgb(167, 201, 239);
      this.gradientPanel1.Location = new Point(0, 0);
      this.gradientPanel1.Name = "gradientPanel1";
      this.gradientPanel1.Size = new Size(787, 31);
      this.gradientPanel1.Style = GradientPanel.PanelStyle.PageHeader;
      this.gradientPanel1.TabIndex = 9;
      this.cboView.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboView.FormattingEnabled = true;
      this.cboView.Location = new Point(137, 4);
      this.cboView.Name = "cboView";
      this.cboView.SelectedBGColor = SystemColors.Highlight;
      this.cboView.Size = new Size(194, 21);
      this.cboView.TabIndex = 9;
      this.cboView.SelectedIndexChanged += new EventHandler(this.cboView_SelectedIndexChanged);
      this.label2.AutoSize = true;
      this.label2.BackColor = Color.Transparent;
      this.label2.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(7, 7);
      this.label2.Name = "label2";
      this.label2.Size = new Size(125, 16);
      this.label2.TabIndex = 8;
      this.label2.Text = "Loan Search View";
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.tableContainer1);
      this.Controls.Add((Control) this.collapsibleSplitter1);
      this.Controls.Add((Control) this.grpSearch);
      this.Controls.Add((Control) this.gradientPanel1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (LoanSearchScreen);
      this.Size = new Size(787, 554);
      ((ISupportInitialize) this.btnExport).EndInit();
      ((ISupportInitialize) this.btnManageViews).EndInit();
      ((ISupportInitialize) this.btnResetView).EndInit();
      ((ISupportInitialize) this.btnSaveView).EndInit();
      this.tableContainer1.ResumeLayout(false);
      this.tableContainer1.PerformLayout();
      this.flowLayoutPanel1.ResumeLayout(false);
      this.mnuLoans.ResumeLayout(false);
      this.grpSearch.ResumeLayout(false);
      this.gradientPanel1.ResumeLayout(false);
      this.gradientPanel1.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
