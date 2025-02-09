// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.GSECommitmentListScreen
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Reporting;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class GSECommitmentListScreen : UserControl, IMenuProvider
  {
    private const string className = "GSECommitmentListScreen";
    private static readonly string sw = Tracing.SwOutsideLoan;
    private GseCommitmentStatusEnumNameProvider tradeStatusNameProvider = new GseCommitmentStatusEnumNameProvider();
    private string standardViewName = "Standard View";
    private FileSystemEntry fsViewEntry;
    private FieldFilterList advFilter;
    private GridViewLayoutManager gvLayoutManager;
    private GridViewReportFilterManager gvFilterManager;
    private GSECommitmentReportFieldDefs tradeFieldDefs;
    private TradeView currentView;
    private bool suspendEvents;
    private bool suspendRefresh;
    private ICursor tradeCursor;
    private TableLayout tradeTableLayout;
    private IContainer components;
    private Button btnArchive;
    private Button btnLock;
    private Button btnMakeCurrent;
    private TableContainer tableContainer1;
    private FlowLayoutPanel flowLayoutPanel1;
    private VerticalSeparator verticalSeparator1;
    private StandardIconButton btnDelete;
    private StandardIconButton btnDuplicate;
    private StandardIconButton btnEdit;
    private StandardIconButton btnNew;
    private ToolTip toolTip1;
    private GradientPanel gradientPanel1;
    private StandardIconButton btnEditView;
    private StandardIconButton btnRefreshView;
    private StandardIconButton btnSaveView;
    private Label label2;
    private ComboBoxEx cBoxView;
    private GradientPanel gradientPanel2;
    private Button btnAdvSearch;
    private Button btnClearSearch;
    private Label lblFilter;
    private Label label3;
    private PageListNavigator navTrades;
    private GridView gvTradeList;
    private ImageList ilButtonImages;
    private StandardIconButton siBtnRefresh;
    private StandardIconButton siBtnPrint;
    private StandardIconButton siBtnExcel;
    private Label label1;
    private ComboBox cboFilterView;

    public bool IsReadOnly { get; set; }

    public GSECommitmentListScreen()
    {
      this.InitializeComponent();
      this.init();
    }

    private void init()
    {
      this.resetFieldDefs();
      this.loadViewList(Session.ConfigurationManager.GetTemplateDirEntries(EllieMae.EMLite.ClientServer.TemplateSettingsType.GSECommitmentView, FileSystemEntry.PrivateRoot(Session.UserID)));
      string privateProfileString = Session.GetPrivateProfileString("GSECommitment", "DefaultView");
      ClientCommonUtils.PopulateDropdown((ComboBox) this.cBoxView, (object) new FileSystemEntryListItem(FileSystemEntry.Parse("personal:\\" + this.standardViewName, Session.UserID)), false);
      if (privateProfileString != "" && privateProfileString != this.standardViewName && ClientCommonUtils.ItemExistInDropDown((ComboBox) this.cBoxView, (object) new FileSystemEntryListItem(FileSystemEntry.Parse(privateProfileString))))
        ClientCommonUtils.PopulateDropdown((ComboBox) this.cBoxView, (object) new FileSystemEntryListItem(FileSystemEntry.Parse(privateProfileString)), false);
      this.cboFilterView.SelectedIndex = 0;
    }

    public void SetReadOnly()
    {
      this.btnNew.Enabled = false;
      this.btnEdit.Enabled = false;
      this.btnDuplicate.Enabled = false;
      this.btnLock.Enabled = false;
      this.btnDelete.Enabled = false;
      this.btnArchive.Enabled = false;
    }

    private void loadViewList(FileSystemEntry[] fsEntries)
    {
      this.suspendEvents = true;
      try
      {
        this.cBoxView.Items.Clear();
        this.cBoxView.Dividers.Clear();
        foreach (FileSystemEntry fsEntry in fsEntries)
          this.cBoxView.Items.Add((object) new FileSystemEntryListItem(fsEntry));
        this.cBoxView.Dividers.Add(this.cBoxView.Items.Count);
        this.cBoxView.Items.Add((object) new FileSystemEntryListItem(new FileSystemEntry("\\" + this.standardViewName, FileSystemEntry.Types.File, Session.UserID)));
        if (this.currentView == null)
          return;
        ClientCommonUtils.PopulateDropdown((ComboBox) this.cBoxView, (object) new FileSystemEntryListItem(new FileSystemEntry("\\" + this.currentView.Name, FileSystemEntry.Types.File, Session.UserID)), false);
      }
      catch (Exception ex)
      {
        Tracing.Log(GSECommitmentListScreen.sw, nameof (GSECommitmentListScreen), TraceLevel.Error, "Error loading view list: " + (object) ex);
        ErrorDialog.Display(ex);
      }
      finally
      {
        this.suspendEvents = false;
      }
    }

    private void resetFieldDefs()
    {
      this.tradeFieldDefs = GSECommitmentReportFieldDefs.GetFieldDefs();
    }

    public void RefreshContents()
    {
      if (this.gvLayoutManager == null)
        this.init();
      this.RefreshList(true);
      this.showHideButtons();
    }

    public void RefreshList(bool preserveSelections)
    {
      if (this.suspendRefresh)
        return;
      using (CursorActivator.Wait())
      {
        using (PerformanceMeter performanceMeter = PerformanceMeter.StartNew("GSECommitmentListScreen.Refresh", "Refresh the Trade screen data", true, 139, nameof (RefreshList), "D:\\ws\\24.3.0.0\\EmLite\\TradeManagement\\Trading\\FannieMaePEMBS\\GSECommitmentListScreen.cs"))
        {
          this.retrieveTradeData();
          this.displayCurrentPage(preserveSelections);
          this.refreshFilterDescription();
          performanceMeter.AddVariable("TradeCount", (object) this.navTrades.NumberOfItems);
          performanceMeter.AddVariable("Columns", (object) this.gvTradeList.Columns.Count);
          performanceMeter.AddVariable("Filter", (object) this.lblFilter.Text);
          GVColumnSort[] sortOrder = this.gvTradeList.Columns.GetSortOrder();
          if (sortOrder.Length != 0)
            performanceMeter.AddVariable("Sort", this.gvTradeList.Columns[sortOrder[0].Column].Tag);
        }
      }
      if (!this.IsReadOnly)
        return;
      this.SetReadOnly();
    }

    public FieldFilterList GetCurrentFilter()
    {
      FieldFilterList fieldFilterList = this.gvFilterManager.ToFieldFilterList();
      if (this.advFilter != null)
        fieldFilterList.AddRange((IEnumerable<FieldFilter>) this.advFilter);
      return fieldFilterList;
    }

    private void refreshFilterDescription()
    {
      FieldFilterList currentFilter = this.GetCurrentFilter();
      if (currentFilter.Count == 0)
      {
        this.lblFilter.Text = "None";
        this.btnClearSearch.Enabled = false;
      }
      else
      {
        this.lblFilter.Text = currentFilter.ToString(true);
        this.btnClearSearch.Enabled = true;
      }
    }

    private void displayCurrentPage(bool preserveSelections)
    {
      int currentPageItemIndex = this.navTrades.CurrentPageItemIndex;
      int currentPageItemCount = this.navTrades.CurrentPageItemCount;
      GSECommitmentViewModel[] commitmentViewModelArray = new GSECommitmentViewModel[0];
      if (currentPageItemCount > 0)
        commitmentViewModelArray = (GSECommitmentViewModel[]) this.tradeCursor.GetItems(currentPageItemIndex, currentPageItemCount);
      Dictionary<int, bool> dictionary = new Dictionary<int, bool>();
      if (preserveSelections)
      {
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvTradeList.Items)
        {
          if (gvItem.Selected)
            dictionary[((TradeBase) gvItem.Tag).TradeID] = true;
        }
      }
      this.gvTradeList.Items.Clear();
      for (int index = 0; index < commitmentViewModelArray.Length; ++index)
      {
        GVItem gvItem = this.createGVItem(commitmentViewModelArray[index]);
        this.gvTradeList.Items.Add(gvItem);
        if (dictionary.ContainsKey(commitmentViewModelArray[index].TradeID))
          gvItem.Selected = true;
      }
      if (this.gvTradeList.Items.Count <= 0 || this.gvTradeList.SelectedItems.Count != 0)
        return;
      this.gvTradeList.Items[0].Selected = true;
    }

    private GVItem createGVItem(GSECommitmentViewModel tradeInfo)
    {
      GVItem gvItem = new GVItem();
      gvItem.Tag = (object) tradeInfo;
      for (int index = 0; index < this.gvTradeList.Columns.Count; ++index)
      {
        string columnId = ((TableLayout.Column) this.gvTradeList.Columns[index].Tag).ColumnID;
        object obj = (object) null;
        GSECommitmentReportFieldDef fieldByCriterionName = this.tradeFieldDefs.GetFieldByCriterionName(columnId);
        if (fieldByCriterionName != null && tradeInfo != null && tradeInfo[columnId] != null)
          obj = ReportFieldClientExtension.ToDisplayElement(fieldByCriterionName, columnId, (IPropertyDictionary) tradeInfo, (EventHandler) null);
        gvItem.SubItems[index].Value = obj;
      }
      return gvItem;
    }

    private void retrieveTradeData()
    {
      this.retrieveTradeData((QueryCriterion) null, (SortField[]) null);
    }

    private SortField[] generateSortFields()
    {
      List<SortField> sortFieldList = new List<SortField>();
      foreach (GVColumn column in this.gvTradeList.Columns)
      {
        if (column.SortOrder != SortOrder.None)
        {
          TableLayout.Column tag = (TableLayout.Column) column.Tag;
          sortFieldList.Add(new SortField(tag.ColumnID, SortOrder.Ascending == column.SortOrder ? FieldSortOrder.Ascending : FieldSortOrder.Descending));
        }
      }
      return sortFieldList.ToArray();
    }

    private SortField[] generateSortFields(GVColumnSort[] sortOrder)
    {
      List<SortField> sortFieldList = new List<SortField>();
      foreach (GVColumnSort gvColumnSort in sortOrder)
      {
        TableLayout.Column tag = (TableLayout.Column) this.gvTradeList.Columns[gvColumnSort.Column].Tag;
        sortFieldList.Add(new SortField(tag.ColumnID, SortOrder.Ascending == gvColumnSort.SortOrder ? FieldSortOrder.Ascending : FieldSortOrder.Descending));
      }
      return sortFieldList.ToArray();
    }

    private QueryCriterion generateQueryCriteria()
    {
      QueryCriterion criterion = (QueryCriterion) null;
      if (this.advFilter != null)
        criterion = this.advFilter.CreateEvaluator().ToQueryCriterion();
      QueryCriterion queryCriterion = this.gvFilterManager.ToQueryCriterion();
      QueryCriterion queryCriteria = (QueryCriterion) null;
      if (criterion != null)
        queryCriteria = queryCriteria == null ? criterion : queryCriteria.And(criterion);
      if (queryCriterion != null)
        queryCriteria = queryCriteria == null ? queryCriterion : queryCriteria.And(queryCriterion);
      return queryCriteria;
    }

    private string[] generateFieldList()
    {
      List<string> stringList = new List<string>();
      if (this.gvLayoutManager == null)
        this.createLayoutManager();
      foreach (TableLayout.Column column in this.gvLayoutManager.GetCurrentLayout())
      {
        GSECommitmentReportFieldDef fieldByCriterionName = this.tradeFieldDefs.GetFieldByCriterionName(column.ColumnID);
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

    private GridViewLayoutManager createLayoutManager()
    {
      GridViewLayoutManager layoutManager = new GridViewLayoutManager(this.gvTradeList, this.getFullTableLayout());
      layoutManager.LayoutChanged += new EventHandler(this.mngr_LayoutChanged);
      return layoutManager;
    }

    private void mngr_LayoutChanged(object sender, EventArgs e)
    {
      if (this.suspendEvents)
        return;
      if (this.gvFilterManager != null)
        this.gvFilterManager.CreateColumnFilters((ReportFieldDefs) this.tradeFieldDefs);
      this.tradeTableLayout = this.gvLayoutManager.GetCurrentLayout();
      this.gvFilterManager.CreateColumnFilters((ReportFieldDefs) this.tradeFieldDefs);
      this.setViewChanged(true);
      this.RefreshList(true);
    }

    private void setViewChanged(bool modified)
    {
      this.btnSaveView.Enabled = modified;
      this.btnRefreshView.Enabled = modified;
    }

    private TableLayout getFullTableLayout()
    {
      TableLayout fullTableLayout = new TableLayout();
      foreach (GSECommitmentReportFieldDef tradeFieldDef in (ReportFieldDefContainer) this.tradeFieldDefs)
      {
        if (fullTableLayout.GetColumnByID(tradeFieldDef.CriterionFieldName) == null)
          fullTableLayout.AddColumn(tradeFieldDef.ToTableLayoutColumn());
      }
      fullTableLayout.SortByDescription();
      return fullTableLayout;
    }

    private void retrieveTradeData(QueryCriterion filter, SortField[] sortFields)
    {
      Cursor.Current = Cursors.WaitCursor;
      try
      {
        string[] fieldList = this.generateFieldList();
        if (filter == null)
          filter = this.generateQueryCriteria();
        ListValueCriterion criterion;
        if (this.cboFilterView.SelectedIndex == 0)
          criterion = new ListValueCriterion("GseCommitmentDetails.Status", (Array) new int[3]
          {
            2,
            0,
            1
          }, true);
        else
          criterion = new ListValueCriterion("GseCommitmentDetails.Status", (Array) new int[1]
          {
            5
          }, true);
        filter = filter != null ? filter.And((QueryCriterion) criterion) : (QueryCriterion) criterion;
        if (sortFields == null)
          sortFields = this.generateSortFields();
        this.suspendEvents = true;
        if (this.tradeCursor != null)
        {
          this.tradeCursor.Dispose();
          this.tradeCursor = (ICursor) null;
        }
        if (filter == null)
          this.tradeCursor = Session.GseCommitmentManager.OpenTradeCursor(new QueryCriterion[0], sortFields, fieldList, true, false);
        else
          this.tradeCursor = Session.GseCommitmentManager.OpenTradeCursor(new QueryCriterion[1]
          {
            filter
          }, sortFields, fieldList, true, false);
        this.navTrades.NumberOfItems = this.tradeCursor.GetItemCount();
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Error loading Trades: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      finally
      {
        Cursor.Current = Cursors.Default;
        this.suspendEvents = false;
      }
    }

    private void cboView_SelectedIndexChanged(object sender, EventArgs e) => this.showHideButtons();

    private void showHideButtons()
    {
      bool flag = this.cboFilterView.SelectedIndex == 0;
      this.btnNew.Visible = flag;
      this.btnEdit.Visible = flag;
      this.btnLock.Visible = false;
      this.btnArchive.Visible = flag;
      this.btnMakeCurrent.Visible = !flag;
      this.siBtnExcel.Enabled = this.gvTradeList.Items.Count > 0;
    }

    private void btnNew_Click(object sender, EventArgs e)
    {
      TradeManagementConsole.Instance.OpenGSECommitment();
    }

    private void btnEdit_Click(object sender, EventArgs e)
    {
      if (this.gvTradeList.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must first select the trade from the list to be edited.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
        this.editTrade(this.gvTradeList.SelectedItems[0]);
    }

    private void editTrade(GVItem item)
    {
      GSECommitmentViewModel tag = (GSECommitmentViewModel) item.Tag;
      TradeManagementConsole.Instance.OpenGseCommitment(tag.TradeID);
    }

    private void btnDuplicate_Click(object sender, EventArgs e)
    {
      if (this.gvTradeList.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You must first select the commitment from the list to be duplicated.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        GSECommitmentViewModel tag = (GSECommitmentViewModel) this.gvTradeList.SelectedItems[0].Tag;
        GSECommitmentInfo trade1 = Session.GseCommitmentManager.GetTrade(tag.TradeID);
        if (tag == null)
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "The selected commitment has been deleted and cannot be duplicated.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          this.RefreshList(false);
        }
        else
        {
          GSECommitmentInfo trade2 = trade1.Duplicate();
          TradeManagementConsole.Instance.OpenGseCommitment(trade2);
        }
      }
    }

    private void btnLock_Click(object sender, EventArgs e)
    {
      if (this.gvTradeList.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You must first select the GSE Commitment(s) from the list to be locked or unlocked.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        bool locked = ((GSECommitmentViewModel) this.gvTradeList.SelectedItems[0].Tag).Locked;
        for (int index = 1; index < this.gvTradeList.SelectedItems.Count; ++index)
        {
          if (((GSECommitmentViewModel) this.gvTradeList.SelectedItems[index].Tag).Locked != locked)
          {
            int num2 = (int) Utils.Dialog((IWin32Window) this, "All of the selected  GSE Commitment(s) must be either locked or unlocked to perform this operation.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            return;
          }
        }
        if (Utils.Dialog((IWin32Window) this, "Change the status of the selected  GSE Commitment(s) to be " + (locked ? "Unlocked" : "Locked") + "?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
          return;
        try
        {
          Cursor.Current = Cursors.WaitCursor;
          foreach (GVItem selectedItem in this.gvTradeList.SelectedItems)
          {
            this.toggleLockState((GSECommitmentViewModel) selectedItem.Tag);
            this.RefreshList(true);
          }
        }
        catch (Exception ex)
        {
          int num3 = (int) Utils.Dialog((IWin32Window) this, "An unexpected error has occurred while attempting to lock these GSE Commitment(s): " + (object) ex, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        Cursor.Current = Cursors.Default;
      }
    }

    private void toggleLockState(GSECommitmentViewModel info)
    {
      GSECommitmentInfo trade = Session.GseCommitmentManager.GetTrade(info.TradeID);
      if (trade == null || trade.Locked != info.Locked)
        return;
      trade.Locked = !trade.Locked;
      Session.GseCommitmentManager.UpdateTrade(trade, false);
      info.Locked = trade.Locked;
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
      if (this.gvTradeList.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You must first select the GSE Commitment from the list to be deleted.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        if (Utils.Dialog((IWin32Window) this, "The selected  GSE Commitment(s) will be permanently deleted. Are you sure you want to proceed?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
          return;
        try
        {
          GSECommitmentViewModel[] state = new GSECommitmentViewModel[this.gvTradeList.SelectedItems.Count];
          for (int index = 0; index < this.gvTradeList.SelectedItems.Count; ++index)
            state[index] = (GSECommitmentViewModel) this.gvTradeList.SelectedItems[index].Tag;
          using (ProgressDialog progressDialog = new ProgressDialog("Deleting GSECommitment(s)", new AsynchronousProcess(this.deleteTradesAsync), (object) state, true))
          {
            int num2 = (int) progressDialog.ShowDialog();
          }
        }
        catch (Exception ex)
        {
          int num3 = (int) Utils.Dialog((IWin32Window) this, "An unexpected error has occurred while attempting to delete the GSE Commitment(s): " + (object) ex, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        this.RefreshContents();
      }
    }

    private DialogResult deleteTradesAsync(object tradesAsObj, IProgressFeedback feedback)
    {
      try
      {
        GSECommitmentViewModel[] commitmentViewModelArray = (GSECommitmentViewModel[]) tradesAsObj;
        int num1 = 0;
        feedback.ResetCounter(commitmentViewModelArray.Length);
        foreach (GSECommitmentViewModel info in commitmentViewModelArray)
        {
          if (feedback.Cancel)
            return DialogResult.Cancel;
          switch (this.deleteTrade(info, feedback))
          {
            case DialogResult.OK:
              ++num1;
              break;
            case DialogResult.Cancel:
              return DialogResult.Cancel;
          }
        }
        if (num1 == 1)
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "1 GSE Commitment has been successfully deleted.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        else
        {
          int num3 = (int) Utils.Dialog((IWin32Window) this, num1.ToString() + " GSE Commitments have been successfully deleted.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        return DialogResult.OK;
      }
      catch (Exception ex)
      {
        Tracing.Log(GSECommitmentListScreen.sw, nameof (GSECommitmentListScreen), TraceLevel.Error, "Error deleting GSE Commitments: " + (object) ex);
        int num = (int) Utils.Dialog((IWin32Window) this, "An unexpected error occurred while attempting to delete the selected GSE Commitment(s): " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return DialogResult.Abort;
      }
    }

    private DialogResult deleteTrade(GSECommitmentViewModel info, IProgressFeedback feedback)
    {
      try
      {
        feedback.Status = "Deleting GSE Commitment '" + info.Name + "'...";
        GSECommitmentInfo trade = Session.SessionObjects.GseCommitmentManager.GetTrade(info.TradeID);
        if (trade == null)
          return DialogResult.OK;
        Session.GseCommitmentManager.DeleteTrade(trade.TradeID);
        return DialogResult.OK;
      }
      catch (Exception ex)
      {
        Tracing.Log(GSECommitmentListScreen.sw, nameof (GSECommitmentListScreen), TraceLevel.Error, "Error processing GSE Commitment " + (object) info.TradeID + ": " + (object) ex);
        return Utils.Dialog((IWin32Window) this, "An unexpected error occurred while attempting to delete the GSE Commitment " + info.Name + ": " + ex.Message, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.Cancel ? DialogResult.Cancel : DialogResult.Abort;
      }
    }

    private bool isCurrentTrades(object sender) => this.cboFilterView.SelectedIndex == 0;

    private void btnArchive_Click(object sender, EventArgs e)
    {
      if (this.gvTradeList.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You must first select the GSE Commitment(s) to be " + (this.isCurrentTrades(sender) ? "archived" : "activated") + ".", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        if ((!this.isCurrentTrades(sender) ? Utils.Dialog((IWin32Window) this, "Are you sure you want to activate the selected GSE Commitment(s)?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) : Utils.Dialog((IWin32Window) this, "Are you sure you want to archive the selected GSE Commitment(s)?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation)) != DialogResult.Yes)
          return;
        List<GVItem> gvItemList = new List<GVItem>();
        try
        {
          Cursor.Current = Cursors.WaitCursor;
          bool archive = false;
          using (IEnumerator<GVItem> enumerator = this.gvTradeList.SelectedItems.GetEnumerator())
          {
            if (enumerator.MoveNext())
              archive = !((GSECommitmentViewModel) enumerator.Current.Tag).Archived;
          }
          foreach (GVItem selectedItem in this.gvTradeList.SelectedItems)
            this.archiveActivateTrade((GSECommitmentViewModel) selectedItem.Tag, archive);
        }
        catch (Exception ex)
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "An unexpected error has occurred while attempting to archive the GSE Commitment(s): " + (object) ex, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        this.RefreshList(false);
        Cursor.Current = Cursors.WaitCursor;
      }
    }

    private void archiveActivateTrade(GSECommitmentViewModel info, bool archive)
    {
      info.Archived = archive;
      if (archive)
        Session.GseCommitmentManager.SetTradeStatus(info.TradeID, TradeStatus.Archived);
      else
        Session.GseCommitmentManager.SetTradeStatus(info.TradeID, info.Status);
    }

    public void MenuClicked(ToolStripItem menuItem)
    {
      string s = string.Concat(menuItem.Tag);
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(s))
      {
        case 624708596:
          if (!(s == "GSE_Duplicate"))
            break;
          this.btnDuplicate_Click((object) null, (EventArgs) null);
          break;
        case 859130664:
          if (!(s == "GSE_ExportSelected"))
            break;
          this.siBtnExcel_Click((object) null, (EventArgs) null);
          break;
        case 1071146640:
          if (!(s == "GSE_ExportAll"))
            break;
          this.siBtnExcelAll_Click((object) null, (EventArgs) null);
          break;
        case 1502496952:
          if (!(s == "GSE_LockUnlock"))
            break;
          this.btnLock_Click((object) null, (EventArgs) null);
          break;
        case 2021000882:
          if (!(s == "GSE_Print"))
            break;
          this.siBtnPrint_Click((object) null, (EventArgs) null);
          break;
        case 2232260400:
          if (!(s == "GSE_Delete"))
            break;
          this.btnDelete_Click((object) null, (EventArgs) null);
          break;
        case 2895684171:
          if (!(s == "GSE_Archive"))
            break;
          this.btnArchive_Click((object) null, (EventArgs) null);
          break;
        case 3097329967:
          if (!(s == "GSE_Edit"))
            break;
          this.btnEdit_Click((object) null, (EventArgs) null);
          break;
        case 4119533827:
          if (!(s == "GSE_New"))
            break;
          this.btnNew_Click((object) null, (EventArgs) null);
          break;
      }
    }

    public bool SetMenuItemState(ToolStripItem menuItem)
    {
      Control stateControl = (Control) null;
      switch (string.Concat(menuItem.Tag))
      {
        case "GSE_Archive":
          stateControl = (Control) this.btnArchive;
          break;
        case "GSE_Delete":
          stateControl = (Control) this.btnDelete;
          break;
        case "GSE_Duplicate":
          stateControl = (Control) this.btnDuplicate;
          break;
        case "GSE_Edit":
          stateControl = (Control) this.btnEdit;
          break;
        case "GSE_ExportAll":
          menuItem.Enabled = this.gvTradeList.Items.Count > 0 && Session.ACL.IsAuthorizedForFeature(AclFeature.LoanMgmt_ExportToExcel);
          return true;
        case "GSE_ExportSelected":
          menuItem.Enabled = this.siBtnExcel.Enabled;
          break;
        case "GSE_LockUnlock":
          stateControl = (Control) this.btnLock;
          break;
        case "GSE_New":
          stateControl = (Control) this.btnNew;
          break;
      }
      if (stateControl == null)
        return true;
      ClientCommonUtils.ApplyControlStateToMenu(menuItem, stateControl);
      return stateControl.Visible;
    }

    private void btnEditView_Click(object sender, EventArgs e)
    {
      using (ViewManagementDialog managementDialog = new ViewManagementDialog(EllieMae.EMLite.ClientServer.TemplateSettingsType.GSECommitmentView, false, "GSECommitment.DefaultView"))
      {
        managementDialog.AddStaticView((BinaryConvertibleObject) this.getDefaultTradeView());
        int num = (int) managementDialog.ShowDialog((IWin32Window) this);
      }
      this.RefreshViews();
    }

    public void RefreshViews()
    {
      this.loadViewList(Session.ConfigurationManager.GetTemplateDirEntries(EllieMae.EMLite.ClientServer.TemplateSettingsType.GSECommitmentView, FileSystemEntry.PrivateRoot(Session.UserID)));
      if (this.cBoxView.Items.Count <= 0 || this.cBoxView.SelectedIndex >= 0)
        return;
      this.cBoxView.SelectedIndex = 0;
    }

    private void btnRefreshView_Click(object sender, EventArgs e)
    {
      if (Utils.Dialog((IWin32Window) this, "Are you sure you want to reset the selected view?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      this.setCurrentView(this.currentView);
    }

    private void btnSaveView_Click(object sender, EventArgs e) => this.saveCurrentView();

    private void saveCurrentView()
    {
      TradeView tradeView = new TradeView(this.currentView.Name);
      tradeView.Layout = this.gvLayoutManager.GetCurrentLayout();
      tradeView.Filter = this.GetCurrentFilter();
      tradeView.ViewActive = this.cboFilterView.SelectedIndex == 0;
      using (SaveViewTemplateDialog viewTemplateDialog = new SaveViewTemplateDialog(EllieMae.EMLite.ClientServer.TemplateSettingsType.GSECommitmentView, (object) tradeView, this.getViewNameList(), this.currentView.Name != this.standardViewName))
      {
        if (viewTemplateDialog.ShowDialog((IWin32Window) this) == DialogResult.Cancel)
          return;
        if (!viewTemplateDialog.SelectedEntry.Equals((object) this.fsViewEntry))
          this.updateCurrentView(tradeView, viewTemplateDialog.SelectedEntry);
      }
      this.currentView = tradeView;
      this.btnSaveView.Enabled = false;
      this.btnRefreshView.Enabled = false;
    }

    private void updateCurrentView(TradeView view, FileSystemEntry e)
    {
      this.suspendEvents = true;
      this.currentView = view;
      ClientCommonUtils.PopulateDropdown((ComboBox) this.cBoxView, (object) new FileSystemEntryListItem(e), true);
      this.suspendEvents = false;
    }

    private string[] getViewNameList()
    {
      List<string> stringList = new List<string>();
      foreach (object obj in this.cBoxView.Items)
        stringList.Add(obj.ToString());
      return stringList.ToArray();
    }

    private void cBoxView_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.suspendEvents)
        return;
      this.SetCurrentView(((FileSystemEntryListItem) this.cBoxView.SelectedItem).Entry);
    }

    private void btnAdvSearch_Click(object sender, EventArgs e)
    {
      FieldFilterList fieldFilterList = this.gvFilterManager.ToFieldFilterList();
      if (this.advFilter != null)
        fieldFilterList.Merge(this.advFilter);
      if (this.tradeFieldDefs == null)
        this.resetFieldDefs();
      using (ViewAdvSearchDialog viewAdvSearchDialog = new ViewAdvSearchDialog((ReportFieldDefs) this.tradeFieldDefs, fieldFilterList, "GSE Commitment View Advanced Search"))
      {
        if (viewAdvSearchDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.SetCurrentFilter(viewAdvSearchDialog.GetSelectedFilter());
      }
    }

    private void btnClearSearch_Click(object sender, EventArgs e)
    {
      this.SetCurrentFilter((FieldFilterList) null);
    }

    public void SetCurrentView(FileSystemEntry fsEntry)
    {
      try
      {
        if (fsEntry.Name == this.standardViewName)
        {
          TradeView defaultTradeView = this.getDefaultTradeView();
          this.fsViewEntry = fsEntry;
          this.setCurrentView(defaultTradeView);
        }
        else
        {
          TradeView templateSettings = (TradeView) Session.ConfigurationManager.GetTemplateSettings(EllieMae.EMLite.ClientServer.TemplateSettingsType.GSECommitmentView, fsEntry);
          if (templateSettings == null)
            throw new ArgumentException();
          this.btnEditView.Enabled = !fsEntry.IsPublic && !this.IsReadOnly;
          this.fsViewEntry = fsEntry;
          this.setCurrentView(templateSettings);
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(GSECommitmentListScreen.sw, nameof (GSECommitmentListScreen), TraceLevel.Error, "Error opening view: " + (object) ex);
        ErrorDialog.Display(ex);
      }
    }

    private void setCurrentView(TradeView view)
    {
      this.currentView = view;
      this.suspendEvents = true;
      this.applyTableLayout(view.Layout);
      this.cboFilterView.SelectedIndex = view.ViewActive ? 0 : 1;
      this.advFilter = view.Filter;
      this.gvFilterManager.ClearColumnFilters();
      this.setViewChanged(false);
      this.suspendEvents = false;
      this.SetCurrentFilter(view.Filter);
      this.btnSaveView.Enabled = false;
      this.btnRefreshView.Enabled = false;
    }

    public void SetCurrentFilter(FieldFilterList filter)
    {
      this.advFilter = filter;
      this.gvFilterManager.ClearColumnFilters();
      this.refreshFilterDescription();
      this.RefreshList(false);
      this.setViewChanged(true);
    }

    private void applyTableLayout(TableLayout layout)
    {
      if (this.gvLayoutManager == null)
        this.gvLayoutManager = this.createLayoutManager();
      this.gvLayoutManager.ApplyLayout(layout, false);
      if (this.gvFilterManager == null)
      {
        this.gvFilterManager = new GridViewReportFilterManager(Session.DefaultInstance, this.gvTradeList);
        this.gvFilterManager.FilterChanged += new EventHandler(this.gvFilterManager_FilterChanged);
      }
      this.gvFilterManager.CreateColumnFilters((ReportFieldDefs) this.tradeFieldDefs);
    }

    private void gvFilterManager_FilterChanged(object sender, EventArgs e)
    {
      this.RefreshList(false);
      this.setViewChanged(true);
    }

    private TradeView getDefaultTradeView()
    {
      TradeView defaultTradeView = new TradeView(this.standardViewName);
      TableLayout tableLayout = new TableLayout();
      tableLayout.AddColumn(new TableLayout.Column("GseCommitmentDetails.Name", "Commitment ID", HorizontalAlignment.Left, 150));
      tableLayout.AddColumn(new TableLayout.Column("GseCommitmentDetails.CommitmentDate", "Commitment Date", HorizontalAlignment.Left, 100));
      tableLayout.AddColumn(new TableLayout.Column("GseCommitmentDetails.ContractNumber", "Contract #", HorizontalAlignment.Left, 130));
      tableLayout.AddColumn(new TableLayout.Column("GseCommitmentDetails.TradeDescription", "Trade Description", HorizontalAlignment.Left, 100));
      tableLayout.AddColumn(new TableLayout.Column("GseCommitmentDetails.CommitmentAmount", "Commitment Amount", HorizontalAlignment.Right, 120));
      tableLayout.AddColumn(new TableLayout.Column("GseCommitmentDetails.OutstandingBalance", "Outstanding Balance", HorizontalAlignment.Right, 130));
      tableLayout.AddColumn(new TableLayout.Column("GseCommitmentDetails.IssueMonth", "Issue Month", HorizontalAlignment.Right, 90));
      tableLayout.AddColumn(new TableLayout.Column("GseCommitmentSummary.CompletionPercent", "Completion Percent", HorizontalAlignment.Right, 130));
      defaultTradeView.Layout = tableLayout;
      return defaultTradeView;
    }

    private bool exportTrades(bool exportAll)
    {
      if (this.gvTradeList.Columns.Count > ExcelHandler.GetMaximumColumnCount())
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You trade list cannot be exported because the number of columns exceeds the limit supported by Excel (" + (object) ExcelHandler.GetMaximumColumnCount() + ")", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      if ((!exportAll ? this.gvTradeList.SelectedItems.Count : this.tradeCursor.GetItemCount()) > ExcelHandler.GetMaximumRowCount() - 1)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You trade list cannot be exported because the number of rows exceeds the limit supported by Excel (" + (object) ExcelHandler.GetMaximumRowCount() + ")", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
      excelHandler.AddDataTable(this.gvTradeList, (ReportFieldDefs) this.tradeFieldDefs, true);
      excelHandler.CreateExcel();
    }

    private void exportAllLoansToExcel()
    {
      using (CursorActivator.Wait())
      {
        ExcelHandler excelHandler = new ExcelHandler();
        foreach (GVColumn c in this.gvTradeList.Columns.DisplaySequence)
          excelHandler.AddHeaderColumn(c.Text, excelHandler.GetColumnFormat(c, (ReportFieldDefs) this.tradeFieldDefs));
        if (this.tradeCursor.GetItemCount() == 0)
          return;
        foreach (GSECommitmentViewModel tradeInfo in (GSECommitmentViewModel[]) this.tradeCursor.GetItems(0, this.tradeCursor.GetItemCount()))
        {
          GVItem gvItem = this.createGVItem(tradeInfo);
          string[] data = new string[this.gvTradeList.Columns.Count];
          for (int index = 0; index < this.gvTradeList.Columns.Count; ++index)
            data[index] = gvItem.SubItems[this.gvTradeList.Columns.DisplaySequence[index].Index].Text;
          excelHandler.AddDataRow(data);
        }
        excelHandler.CreateExcel();
      }
    }

    private void gvTradeList_SelectedIndexChanged(object sender, EventArgs e)
    {
      int count = this.gvTradeList.SelectedItems.Count;
      this.btnEdit.Enabled = !this.IsReadOnly && count == 1;
      this.btnDuplicate.Enabled = !this.IsReadOnly && count == 1;
      this.btnDelete.Enabled = !this.IsReadOnly && count > 0;
      this.siBtnExcel.Enabled = count > 0;
      if (count == 0)
      {
        this.btnArchive.Enabled = false;
      }
      else
      {
        foreach (GVItem selectedItem in this.gvTradeList.SelectedItems)
        {
          if (((GSECommitmentViewModel) selectedItem.Tag).Archived || this.IsReadOnly)
          {
            this.btnArchive.Enabled = false;
            break;
          }
          this.btnArchive.Enabled = true;
        }
      }
      if (count == 0)
      {
        this.btnMakeCurrent.Enabled = false;
      }
      else
      {
        foreach (GVItem selectedItem in this.gvTradeList.SelectedItems)
        {
          if (!((GSECommitmentViewModel) selectedItem.Tag).Archived || this.IsReadOnly)
          {
            this.btnMakeCurrent.Enabled = false;
            break;
          }
          this.btnMakeCurrent.Enabled = true;
        }
      }
      this.btnLock.Enabled = count > 0;
    }

    private void gvTradeList_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      this.editTrade(e.Item);
    }

    private void siBtnRefresh_Click(object sender, EventArgs e) => this.RefreshList(true);

    private void siBtnExcelAll_Click(object sender, EventArgs e) => this.exportTrades(true);

    private void siBtnExcel_Click(object sender, EventArgs e) => this.exportTrades(false);

    private void siBtnPrint_Click(object sender, EventArgs e)
    {
      if (this.gvTradeList.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must first select one or more GSE Commitments from the list.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
        this.printSelectedTrades();
    }

    private void printSelectedTrades()
    {
      ExcelHandler excelHandler = new ExcelHandler();
      excelHandler.AddDataTable(this.gvTradeList, (ReportFieldDefs) this.tradeFieldDefs, true);
      excelHandler.Print();
    }

    private void gvTradeList_ColumnReorder(object source, GVColumnEventArgs e)
    {
      this.setViewChanged(true);
    }

    private void gvTradeList_ColumnResize(object source, GVColumnEventArgs e)
    {
      this.setViewChanged(true);
    }

    private void gvTradeList_SortItems(object source, GVColumnSortEventArgs e)
    {
      if (this.suspendEvents)
        return;
      using (CursorActivator.Wait())
      {
        this.retrieveTradeData(this.generateQueryCriteria(), this.generateSortFields(e.ColumnSorts));
        this.displayCurrentPage(true);
        this.setViewChanged(true);
      }
    }

    private void navTrades_PageChangedEvent(object sender, PageChangedEventArgs e)
    {
      if (this.suspendEvents)
        return;
      this.displayCurrentPage(false);
    }

    private void cboFilterView_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.RefreshList(true);
      this.showHideButtons();
      this.setViewChanged(true);
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
      this.toolTip1 = new ToolTip(this.components);
      this.btnDelete = new StandardIconButton();
      this.btnDuplicate = new StandardIconButton();
      this.btnEdit = new StandardIconButton();
      this.btnNew = new StandardIconButton();
      this.siBtnPrint = new StandardIconButton();
      this.siBtnExcel = new StandardIconButton();
      this.siBtnRefresh = new StandardIconButton();
      this.btnEditView = new StandardIconButton();
      this.btnRefreshView = new StandardIconButton();
      this.btnSaveView = new StandardIconButton();
      this.tableContainer1 = new TableContainer();
      this.gvTradeList = new GridView();
      this.navTrades = new PageListNavigator();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.btnMakeCurrent = new Button();
      this.btnArchive = new Button();
      this.btnLock = new Button();
      this.verticalSeparator1 = new VerticalSeparator();
      this.ilButtonImages = new ImageList(this.components);
      this.gradientPanel1 = new GradientPanel();
      this.label2 = new Label();
      this.cBoxView = new ComboBoxEx();
      this.gradientPanel2 = new GradientPanel();
      this.label1 = new Label();
      this.cboFilterView = new ComboBox();
      this.btnAdvSearch = new Button();
      this.btnClearSearch = new Button();
      this.lblFilter = new Label();
      this.label3 = new Label();
      ((ISupportInitialize) this.btnDelete).BeginInit();
      ((ISupportInitialize) this.btnDuplicate).BeginInit();
      ((ISupportInitialize) this.btnEdit).BeginInit();
      ((ISupportInitialize) this.btnNew).BeginInit();
      ((ISupportInitialize) this.siBtnPrint).BeginInit();
      ((ISupportInitialize) this.siBtnExcel).BeginInit();
      ((ISupportInitialize) this.siBtnRefresh).BeginInit();
      ((ISupportInitialize) this.btnEditView).BeginInit();
      ((ISupportInitialize) this.btnRefreshView).BeginInit();
      ((ISupportInitialize) this.btnSaveView).BeginInit();
      this.tableContainer1.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      this.gradientPanel1.SuspendLayout();
      this.gradientPanel2.SuspendLayout();
      this.SuspendLayout();
      this.btnDelete.BackColor = Color.Transparent;
      this.btnDelete.Enabled = false;
      this.btnDelete.Location = new Point(275, 4);
      this.btnDelete.Margin = new Padding(3, 4, 2, 3);
      this.btnDelete.MouseDownImage = (Image) null;
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new Size(16, 16);
      this.btnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDelete.TabIndex = 0;
      this.btnDelete.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnDelete, "Delete Commitment");
      this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
      this.btnDuplicate.BackColor = Color.Transparent;
      this.btnDuplicate.Enabled = false;
      this.btnDuplicate.Location = new Point(254, 4);
      this.btnDuplicate.Margin = new Padding(3, 4, 2, 3);
      this.btnDuplicate.MouseDownImage = (Image) null;
      this.btnDuplicate.Name = "btnDuplicate";
      this.btnDuplicate.Size = new Size(16, 16);
      this.btnDuplicate.StandardButtonType = StandardIconButton.ButtonType.DuplicateButton;
      this.btnDuplicate.TabIndex = 1;
      this.btnDuplicate.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnDuplicate, "Duplicate Commitment");
      this.btnDuplicate.Click += new EventHandler(this.btnDuplicate_Click);
      this.btnEdit.BackColor = Color.Transparent;
      this.btnEdit.Enabled = false;
      this.btnEdit.Location = new Point(233, 4);
      this.btnEdit.Margin = new Padding(3, 4, 2, 3);
      this.btnEdit.MouseDownImage = (Image) null;
      this.btnEdit.Name = "btnEdit";
      this.btnEdit.Size = new Size(16, 16);
      this.btnEdit.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.btnEdit.TabIndex = 2;
      this.btnEdit.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnEdit, "Edit Commitment");
      this.btnEdit.Click += new EventHandler(this.btnEdit_Click);
      this.btnNew.BackColor = Color.Transparent;
      this.btnNew.Location = new Point(212, 4);
      this.btnNew.Margin = new Padding(3, 4, 2, 3);
      this.btnNew.MouseDownImage = (Image) null;
      this.btnNew.Name = "btnNew";
      this.btnNew.Size = new Size(16, 16);
      this.btnNew.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnNew.TabIndex = 3;
      this.btnNew.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnNew, "New Commitment");
      this.btnNew.Click += new EventHandler(this.btnNew_Click);
      this.siBtnPrint.BackColor = Color.Transparent;
      this.siBtnPrint.Location = new Point(338, 4);
      this.siBtnPrint.Margin = new Padding(3, 4, 2, 3);
      this.siBtnPrint.MouseDownImage = (Image) null;
      this.siBtnPrint.Name = "siBtnPrint";
      this.siBtnPrint.Size = new Size(16, 16);
      this.siBtnPrint.StandardButtonType = StandardIconButton.ButtonType.PrintButton;
      this.siBtnPrint.TabIndex = 10;
      this.siBtnPrint.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.siBtnPrint, "Print Forms");
      this.siBtnPrint.Click += new EventHandler(this.siBtnPrint_Click);
      this.siBtnExcel.BackColor = Color.Transparent;
      this.siBtnExcel.Enabled = false;
      this.siBtnExcel.Location = new Point(317, 4);
      this.siBtnExcel.Margin = new Padding(3, 4, 2, 3);
      this.siBtnExcel.MouseDownImage = (Image) null;
      this.siBtnExcel.Name = "siBtnExcel";
      this.siBtnExcel.Size = new Size(16, 16);
      this.siBtnExcel.StandardButtonType = StandardIconButton.ButtonType.ExcelButton;
      this.siBtnExcel.TabIndex = 9;
      this.siBtnExcel.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.siBtnExcel, "Exports to Excel");
      this.siBtnExcel.Click += new EventHandler(this.siBtnExcel_Click);
      this.siBtnRefresh.BackColor = Color.Transparent;
      this.siBtnRefresh.Location = new Point(296, 4);
      this.siBtnRefresh.Margin = new Padding(3, 4, 2, 3);
      this.siBtnRefresh.MouseDownImage = (Image) null;
      this.siBtnRefresh.Name = "siBtnRefresh";
      this.siBtnRefresh.Size = new Size(16, 16);
      this.siBtnRefresh.StandardButtonType = StandardIconButton.ButtonType.RefreshButton;
      this.siBtnRefresh.TabIndex = 8;
      this.siBtnRefresh.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.siBtnRefresh, "Refresh");
      this.siBtnRefresh.Click += new EventHandler(this.siBtnRefresh_Click);
      this.btnEditView.BackColor = Color.Transparent;
      this.btnEditView.Location = new Point(388, 9);
      this.btnEditView.MouseDownImage = (Image) null;
      this.btnEditView.Name = "btnEditView";
      this.btnEditView.Size = new Size(16, 16);
      this.btnEditView.StandardButtonType = StandardIconButton.ButtonType.ManageButton;
      this.btnEditView.TabIndex = 10;
      this.btnEditView.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnEditView, "Manage Views");
      this.btnEditView.Click += new EventHandler(this.btnEditView_Click);
      this.btnRefreshView.BackColor = Color.Transparent;
      this.btnRefreshView.Enabled = false;
      this.btnRefreshView.Location = new Point(366, 9);
      this.btnRefreshView.MouseDownImage = (Image) null;
      this.btnRefreshView.Name = "btnRefreshView";
      this.btnRefreshView.Size = new Size(16, 16);
      this.btnRefreshView.StandardButtonType = StandardIconButton.ButtonType.ResetButton;
      this.btnRefreshView.TabIndex = 9;
      this.btnRefreshView.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnRefreshView, "Reset View");
      this.btnRefreshView.Click += new EventHandler(this.btnRefreshView_Click);
      this.btnSaveView.BackColor = Color.Transparent;
      this.btnSaveView.Enabled = false;
      this.btnSaveView.Location = new Point(344, 9);
      this.btnSaveView.MouseDownImage = (Image) null;
      this.btnSaveView.Name = "btnSaveView";
      this.btnSaveView.Size = new Size(16, 16);
      this.btnSaveView.StandardButtonType = StandardIconButton.ButtonType.SaveButton;
      this.btnSaveView.TabIndex = 8;
      this.btnSaveView.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnSaveView, "Save View");
      this.btnSaveView.Click += new EventHandler(this.btnSaveView_Click);
      this.tableContainer1.Borders = AnchorStyles.None;
      this.tableContainer1.Controls.Add((Control) this.gvTradeList);
      this.tableContainer1.Controls.Add((Control) this.navTrades);
      this.tableContainer1.Controls.Add((Control) this.flowLayoutPanel1);
      this.tableContainer1.Dock = DockStyle.Fill;
      this.tableContainer1.Location = new Point(0, 61);
      this.tableContainer1.Name = "tableContainer1";
      this.tableContainer1.Size = new Size(1014, 434);
      this.tableContainer1.Style = TableContainer.ContainerStyle.HeaderOnly;
      this.tableContainer1.TabIndex = 3;
      this.gvTradeList.AllowColumnReorder = true;
      this.gvTradeList.BorderStyle = BorderStyle.None;
      this.gvTradeList.Dock = DockStyle.Fill;
      this.gvTradeList.FilterVisible = true;
      this.gvTradeList.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvTradeList.Location = new Point(0, 25);
      this.gvTradeList.Name = "gvTradeList";
      this.gvTradeList.Size = new Size(1014, 409);
      this.gvTradeList.SortOption = GVSortOption.Owner;
      this.gvTradeList.TabIndex = 10;
      this.gvTradeList.SelectedIndexChanged += new EventHandler(this.gvTradeList_SelectedIndexChanged);
      this.gvTradeList.ColumnReorder += new GVColumnEventHandler(this.gvTradeList_ColumnReorder);
      this.gvTradeList.ColumnResize += new GVColumnEventHandler(this.gvTradeList_ColumnResize);
      this.gvTradeList.SortItems += new GVColumnSortEventHandler(this.gvTradeList_SortItems);
      this.gvTradeList.ItemDoubleClick += new GVItemEventHandler(this.gvTradeList_ItemDoubleClick);
      this.navTrades.BackColor = Color.Transparent;
      this.navTrades.Font = new Font("Arial", 8f);
      this.navTrades.Location = new Point(3, 1);
      this.navTrades.Name = "navTrades";
      this.navTrades.NumberOfItems = 0;
      this.navTrades.Size = new Size(254, 24);
      this.navTrades.TabIndex = 9;
      this.navTrades.PageChangedEvent += new PageListNavigator.PageChangedEventHandler(this.navTrades_PageChangedEvent);
      this.flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.flowLayoutPanel1.BackColor = Color.Transparent;
      this.flowLayoutPanel1.Controls.Add((Control) this.btnMakeCurrent);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnArchive);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnLock);
      this.flowLayoutPanel1.Controls.Add((Control) this.verticalSeparator1);
      this.flowLayoutPanel1.Controls.Add((Control) this.siBtnPrint);
      this.flowLayoutPanel1.Controls.Add((Control) this.siBtnExcel);
      this.flowLayoutPanel1.Controls.Add((Control) this.siBtnRefresh);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnDelete);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnDuplicate);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnEdit);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnNew);
      this.flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel1.Location = new Point(358, 0);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new Size(651, 24);
      this.flowLayoutPanel1.TabIndex = 2;
      this.flowLayoutPanel1.WrapContents = false;
      this.btnMakeCurrent.BackColor = SystemColors.Control;
      this.btnMakeCurrent.Enabled = false;
      this.btnMakeCurrent.Location = new Point(537, 1);
      this.btnMakeCurrent.Margin = new Padding(0, 1, 0, 3);
      this.btnMakeCurrent.Name = "btnMakeCurrent";
      this.btnMakeCurrent.Size = new Size(114, 22);
      this.btnMakeCurrent.TabIndex = 6;
      this.btnMakeCurrent.Text = "Move to &Current";
      this.btnMakeCurrent.UseVisualStyleBackColor = true;
      this.btnMakeCurrent.Click += new EventHandler(this.btnArchive_Click);
      this.btnArchive.BackColor = SystemColors.Control;
      this.btnArchive.Enabled = false;
      this.btnArchive.Location = new Point(460, 1);
      this.btnArchive.Margin = new Padding(0, 1, 0, 3);
      this.btnArchive.Name = "btnArchive";
      this.btnArchive.Size = new Size(77, 22);
      this.btnArchive.TabIndex = 6;
      this.btnArchive.Text = "&Archive";
      this.btnArchive.UseVisualStyleBackColor = true;
      this.btnArchive.Click += new EventHandler(this.btnArchive_Click);
      this.btnLock.BackColor = SystemColors.Control;
      this.btnLock.Enabled = false;
      this.btnLock.Location = new Point(364, 1);
      this.btnLock.Margin = new Padding(0, 1, 0, 0);
      this.btnLock.Name = "btnLock";
      this.btnLock.Size = new Size(96, 22);
      this.btnLock.TabIndex = 4;
      this.btnLock.Text = "&Lock/Unlock";
      this.btnLock.UseVisualStyleBackColor = true;
      this.btnLock.Click += new EventHandler(this.btnLock_Click);
      this.verticalSeparator1.Location = new Point(359, 3);
      this.verticalSeparator1.MaximumSize = new Size(2, 16);
      this.verticalSeparator1.MinimumSize = new Size(2, 16);
      this.verticalSeparator1.Name = "verticalSeparator1";
      this.verticalSeparator1.Size = new Size(2, 16);
      this.verticalSeparator1.TabIndex = 4;
      this.verticalSeparator1.Text = "verticalSeparator1";
      this.ilButtonImages.ColorDepth = ColorDepth.Depth8Bit;
      this.ilButtonImages.ImageSize = new Size(16, 16);
      this.ilButtonImages.TransparentColor = Color.White;
      this.gradientPanel1.BackColorGlassyStyle = true;
      this.gradientPanel1.Borders = AnchorStyles.None;
      this.gradientPanel1.Controls.Add((Control) this.btnEditView);
      this.gradientPanel1.Controls.Add((Control) this.btnRefreshView);
      this.gradientPanel1.Controls.Add((Control) this.btnSaveView);
      this.gradientPanel1.Controls.Add((Control) this.label2);
      this.gradientPanel1.Controls.Add((Control) this.cBoxView);
      this.gradientPanel1.Dock = DockStyle.Top;
      this.gradientPanel1.GradientColor1 = Color.FromArgb(81, 123, 184);
      this.gradientPanel1.GradientColor2 = Color.FromArgb(167, 201, 239);
      this.gradientPanel1.Location = new Point(0, 0);
      this.gradientPanel1.Name = "gradientPanel1";
      this.gradientPanel1.Size = new Size(1014, 30);
      this.gradientPanel1.Style = GradientPanel.PanelStyle.PageHeader;
      this.gradientPanel1.TabIndex = 9;
      this.label2.AutoSize = true;
      this.label2.BackColor = Color.Transparent;
      this.label2.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(14, 9);
      this.label2.Name = "label2";
      this.label2.Size = new Size(118, 14);
      this.label2.TabIndex = 5;
      this.label2.Text = "Commitments View";
      this.cBoxView.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cBoxView.FormattingEnabled = true;
      this.cBoxView.Items.AddRange(new object[2]
      {
        (object) "Current Contracts",
        (object) "Archived Contracts"
      });
      this.cBoxView.Location = new Point(179, 6);
      this.cBoxView.Name = "cBoxView";
      this.cBoxView.SelectedBGColor = SystemColors.Highlight;
      this.cBoxView.Size = new Size(158, 21);
      this.cBoxView.TabIndex = 7;
      this.cBoxView.SelectedIndexChanged += new EventHandler(this.cBoxView_SelectedIndexChanged);
      this.gradientPanel2.Borders = AnchorStyles.Top;
      this.gradientPanel2.Controls.Add((Control) this.label1);
      this.gradientPanel2.Controls.Add((Control) this.cboFilterView);
      this.gradientPanel2.Controls.Add((Control) this.btnAdvSearch);
      this.gradientPanel2.Controls.Add((Control) this.btnClearSearch);
      this.gradientPanel2.Controls.Add((Control) this.lblFilter);
      this.gradientPanel2.Controls.Add((Control) this.label3);
      this.gradientPanel2.Dock = DockStyle.Top;
      this.gradientPanel2.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel2.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel2.Location = new Point(0, 30);
      this.gradientPanel2.Name = "gradientPanel2";
      this.gradientPanel2.Size = new Size(1014, 31);
      this.gradientPanel2.Style = GradientPanel.PanelStyle.PageSubHeader;
      this.gradientPanel2.TabIndex = 10;
      this.label1.AutoSize = true;
      this.label1.BackColor = Color.Transparent;
      this.label1.Location = new Point(14, 8);
      this.label1.Name = "label1";
      this.label1.Size = new Size(36, 14);
      this.label1.TabIndex = 14;
      this.label1.Text = "View:";
      this.cboFilterView.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboFilterView.FormattingEnabled = true;
      this.cboFilterView.Items.AddRange(new object[2]
      {
        (object) "Current Commitments View",
        (object) "Archived Commitments"
      });
      this.cboFilterView.Location = new Point(56, 3);
      this.cboFilterView.Name = "cboFilterView";
      this.cboFilterView.Size = new Size(158, 22);
      this.cboFilterView.TabIndex = 13;
      this.cboFilterView.SelectedIndexChanged += new EventHandler(this.cboFilterView_SelectedIndexChanged);
      this.btnAdvSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAdvSearch.Location = new Point(835, 4);
      this.btnAdvSearch.Name = "btnAdvSearch";
      this.btnAdvSearch.Size = new Size(122, 23);
      this.btnAdvSearch.TabIndex = 10;
      this.btnAdvSearch.Text = "Advanced Search";
      this.btnAdvSearch.UseVisualStyleBackColor = true;
      this.btnAdvSearch.Click += new EventHandler(this.btnAdvSearch_Click);
      this.btnClearSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnClearSearch.Location = new Point(958, 4);
      this.btnClearSearch.Name = "btnClearSearch";
      this.btnClearSearch.Size = new Size(53, 23);
      this.btnClearSearch.TabIndex = 9;
      this.btnClearSearch.Text = "Clear";
      this.btnClearSearch.UseVisualStyleBackColor = true;
      this.btnClearSearch.Click += new EventHandler(this.btnClearSearch_Click);
      this.lblFilter.AutoSize = true;
      this.lblFilter.BackColor = Color.Transparent;
      this.lblFilter.Location = new Point(272, 8);
      this.lblFilter.Name = "lblFilter";
      this.lblFilter.Size = new Size(32, 14);
      this.lblFilter.TabIndex = 1;
      this.lblFilter.Text = "None";
      this.label3.AutoSize = true;
      this.label3.BackColor = Color.Transparent;
      this.label3.Location = new Point(232, 8);
      this.label3.Name = "label3";
      this.label3.Size = new Size(33, 14);
      this.label3.TabIndex = 0;
      this.label3.Text = "Filter:";
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.Controls.Add((Control) this.tableContainer1);
      this.Controls.Add((Control) this.gradientPanel2);
      this.Controls.Add((Control) this.gradientPanel1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (GSECommitmentListScreen);
      this.Size = new Size(1014, 495);
      ((ISupportInitialize) this.btnDelete).EndInit();
      ((ISupportInitialize) this.btnDuplicate).EndInit();
      ((ISupportInitialize) this.btnEdit).EndInit();
      ((ISupportInitialize) this.btnNew).EndInit();
      ((ISupportInitialize) this.siBtnPrint).EndInit();
      ((ISupportInitialize) this.siBtnExcel).EndInit();
      ((ISupportInitialize) this.siBtnRefresh).EndInit();
      ((ISupportInitialize) this.btnEditView).EndInit();
      ((ISupportInitialize) this.btnRefreshView).EndInit();
      ((ISupportInitialize) this.btnSaveView).EndInit();
      this.tableContainer1.ResumeLayout(false);
      this.flowLayoutPanel1.ResumeLayout(false);
      this.gradientPanel1.ResumeLayout(false);
      this.gradientPanel1.PerformLayout();
      this.gradientPanel2.ResumeLayout(false);
      this.gradientPanel2.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
