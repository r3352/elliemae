// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.TradeSelector
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using EllieMae.EMLite.ClientServer;
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
namespace EllieMae.EMLite.Trading
{
  public class TradeSelector : Form
  {
    private TradeViewModel selectedTrade;
    private Dictionary<int, MasterContractSummaryInfo> contractLookup;
    private TradeStatusEnumNameProvider tradeStatusNameProvider = new TradeStatusEnumNameProvider();
    private TradeType theTradeType = TradeType.LoanTrade;
    private GridViewReportFilterManager gvFilterManager;
    private ICursor tradeCursor;
    private FieldFilterList advFilter;
    private ReportFieldDefs tradeFieldDefs;
    private GridViewLayoutManager gvLayoutManager;
    private string tradeTypeString = "Loan Trade";
    private IContainer components;
    private Label label1;
    private GridView gvTrades;
    private Button btnOK;
    private Button btnCancel;
    private PageListNavigator navTrades;

    public TradeViewModel SelectedTrade => this.selectedTrade;

    public TradeSelector(TradeType tradeType)
    {
      this.InitializeComponent();
      this.theTradeType = tradeType;
      if (tradeType == TradeType.MbsPool)
      {
        this.tradeTypeString = "MBS Pool";
        this.tradeFieldDefs = (ReportFieldDefs) MbsPoolReportFieldDefs.GetFieldDefs();
      }
      else
        this.tradeFieldDefs = (ReportFieldDefs) TradeReportFieldDefs.GetFieldDefs();
      this.Text = "Assign to " + this.tradeTypeString;
      this.label1.Text = "Select the " + this.tradeTypeString + " to which to assign the selected loans";
      this.SetCurrentView();
      this.LoadTrades();
    }

    private void SetCurrentView()
    {
      TradeView tradeView = new TradeView("Standard View");
      string str1 = "LoanTradeDetails";
      string str2 = "TradeLoanTradeSummary";
      string str3 = "TradesMasterContract";
      if (this.theTradeType == TradeType.MbsPool)
      {
        str1 = "MbsPoolDetails";
        str2 = "TradeMbsPoolSummary";
        str3 = "TradesMasterContract1";
      }
      TableLayout tableLayout = new TableLayout();
      tableLayout.AddColumn(new TableLayout.Column(str1 + ".Name", this.tradeTypeString + " #", HorizontalAlignment.Left, 120));
      tableLayout.AddColumn(new TableLayout.Column(str1 + ".InvestorName", "Investor", HorizontalAlignment.Left, 100));
      tableLayout.AddColumn(new TableLayout.Column(str1 + ".Status", "Status", HorizontalAlignment.Right, 110));
      tableLayout.AddColumn(new TableLayout.Column(str1 + ".TargetDeliveryDate", "Target Delivery Date", HorizontalAlignment.Left, 110));
      tableLayout.AddColumn(new TableLayout.Column(str1 + ".TradeAmount", this.tradeTypeString + " Amt", HorizontalAlignment.Right, 90));
      tableLayout.AddColumn(new TableLayout.Column(str2 + ".TotalAmount", "Assigned Amt", HorizontalAlignment.Right, 100));
      if (this.theTradeType == TradeType.CorrespondentTrade)
        tableLayout.AddColumn(new TableLayout.Column(str2 + ".CompletionPercent", "Assigned Percent", HorizontalAlignment.Left, 110));
      else
        tableLayout.AddColumn(new TableLayout.Column(str2 + ".CompletionPercent", "Completion Percent", HorizontalAlignment.Left, 110));
      tableLayout.AddColumn(new TableLayout.Column(str3 + ".ContractNumber", "Master #", HorizontalAlignment.Left, 110));
      tradeView.Layout = tableLayout;
      if (this.gvLayoutManager == null)
        this.gvLayoutManager = new GridViewLayoutManager(this.gvTrades, tradeView.Layout, false);
      this.gvLayoutManager.ApplyLayout(tradeView.Layout, false);
      if (this.gvFilterManager == null)
      {
        this.gvFilterManager = new GridViewReportFilterManager(Session.DefaultInstance, this.gvTrades);
        this.gvFilterManager.FilterChanged += new EventHandler(this.gvFilterManager_FilterChanged);
      }
      this.gvFilterManager.CreateColumnFilters(this.tradeFieldDefs);
    }

    private void LoadTrades()
    {
      this.gvTrades.SelectedItems.Clear();
      this.gvTrades.Items.Clear();
      this.refreshContractLookup();
      this.RefreshList(true);
    }

    private void RefreshList(bool preserveSelections)
    {
      using (CursorActivator.Wait())
      {
        using (PerformanceMeter performanceMeter = PerformanceMeter.StartNew("Trades.Refresh", "Refresh the Trade screen data", true, 140, nameof (RefreshList), "D:\\ws\\24.3.0.0\\EmLite\\TradeManagement\\Trading\\TradeSelector.cs"))
        {
          this.retrieveTradeData();
          this.displayCurrentPage(preserveSelections);
          performanceMeter.AddVariable("TradeCount", (object) this.navTrades.NumberOfItems);
          performanceMeter.AddVariable("Columns", (object) this.gvTrades.Columns.Count);
          GVColumnSort[] sortOrder = this.gvTrades.Columns.GetSortOrder();
          if (sortOrder.Length == 0)
            return;
          performanceMeter.AddVariable("Sort", this.gvTrades.Columns[sortOrder[0].Column].Tag);
        }
      }
    }

    private void retrieveTradeData()
    {
      this.retrieveTradeData((QueryCriterion) null, (SortField[]) null);
    }

    private void retrieveTradeData(QueryCriterion filter, SortField[] sortFields)
    {
      Cursor.Current = Cursors.WaitCursor;
      try
      {
        string[] array = new List<string>().ToArray();
        if (filter == null)
          filter = this.generateQueryCriteria();
        ListValueCriterion criterion;
        if (this.theTradeType == TradeType.MbsPool)
          criterion = new ListValueCriterion("MbsPoolDetails.Status", (Array) new int[5]
          {
            2,
            0,
            1,
            4,
            3
          }, true);
        else
          criterion = new ListValueCriterion("LoanTradeDetails.Status", (Array) new int[5]
          {
            2,
            0,
            1,
            4,
            3
          }, true);
        filter = filter != null ? filter.And((QueryCriterion) criterion) : (QueryCriterion) criterion;
        if (sortFields == null)
          sortFields = this.generateSortFields();
        if (this.tradeCursor != null)
        {
          this.tradeCursor.Dispose();
          this.tradeCursor = (ICursor) null;
        }
        if (this.theTradeType == TradeType.MbsPool)
        {
          if (filter == null)
            this.tradeCursor = Session.MbsPoolManager.OpenTradeCursor(new QueryCriterion[0], sortFields, array, true, false);
          else
            this.tradeCursor = Session.MbsPoolManager.OpenTradeCursor(new QueryCriterion[1]
            {
              filter
            }, sortFields, array, true, false);
        }
        else if (filter == null)
          this.tradeCursor = Session.LoanTradeManager.OpenTradeCursor(new QueryCriterion[0], sortFields, array, true, false);
        else
          this.tradeCursor = Session.LoanTradeManager.OpenTradeCursor(new QueryCriterion[1]
          {
            filter
          }, sortFields, array, true, false);
        this.navTrades.NumberOfItems = this.tradeCursor.GetItemCount();
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Error loading " + this.tradeTypeString + ": " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      finally
      {
        Cursor.Current = Cursors.Default;
      }
    }

    private void displayCurrentPage(bool preserveSelections)
    {
      int currentPageItemIndex = this.navTrades.CurrentPageItemIndex;
      int currentPageItemCount = this.navTrades.CurrentPageItemCount;
      Dictionary<int, bool> dictionary = new Dictionary<int, bool>();
      TradeViewModel[] tradeViewModelArray = (TradeViewModel[]) new LoanTradeViewModel[0];
      if (this.theTradeType == TradeType.MbsPool)
      {
        tradeViewModelArray = (TradeViewModel[]) new MbsPoolViewModel[0];
        if (currentPageItemCount > 0)
          tradeViewModelArray = (TradeViewModel[]) this.tradeCursor.GetItems(currentPageItemIndex, currentPageItemCount);
        if (preserveSelections)
        {
          foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvTrades.Items)
          {
            if (gvItem.Selected)
              dictionary[((TradeBase) gvItem.Tag).TradeID] = true;
          }
        }
      }
      else
      {
        if (currentPageItemCount > 0)
          tradeViewModelArray = (TradeViewModel[]) this.tradeCursor.GetItems(currentPageItemIndex, currentPageItemCount);
        if (preserveSelections)
        {
          foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvTrades.Items)
          {
            if (gvItem.Selected)
              dictionary[((TradeBase) gvItem.Tag).TradeID] = true;
          }
        }
      }
      this.gvTrades.Items.Clear();
      for (int index = 0; index < tradeViewModelArray.Length; ++index)
      {
        GVItem gvItem = this.createGVItem(tradeViewModelArray[index]);
        this.gvTrades.Items.Add(gvItem);
        if (dictionary.ContainsKey(tradeViewModelArray[index].TradeID))
          gvItem.Selected = true;
      }
      if (this.gvTrades.Items.Count <= 0 || this.gvTrades.SelectedItems.Count != 0)
        return;
      this.gvTrades.Items[0].Selected = true;
    }

    private GVItem createGVItem(TradeViewModel tradeInfo)
    {
      GVItem gvItem = new GVItem();
      gvItem.Tag = (object) tradeInfo;
      for (int index = 0; index < this.gvTrades.Columns.Count; ++index)
      {
        string columnId = ((TableLayout.Column) this.gvTrades.Columns[index].Tag).ColumnID;
        object obj = (object) null;
        ReportFieldDef fieldByCriterionName = this.tradeFieldDefs.GetFieldByCriterionName(columnId);
        if (fieldByCriterionName != null && tradeInfo != null)
        {
          if (this.theTradeType == TradeType.MbsPool)
          {
            if (tradeInfo[columnId] != null)
              obj = ReportFieldClientExtension.ToDisplayElement((TradeReportFieldDef) fieldByCriterionName, columnId, (IPropertyDictionary) tradeInfo, (EventHandler) null);
          }
          else if (tradeInfo[columnId] != null)
            obj = ReportFieldClientExtension.ToDisplayElement((TradeReportFieldDef) fieldByCriterionName, columnId, (IPropertyDictionary) tradeInfo, (EventHandler) null);
        }
        gvItem.SubItems[index].Value = obj;
        if (columnId.Contains("TargetDeliveryDate") && Utils.ToDate(obj.ToString()) != DateTime.MinValue && Utils.ToDate(obj.ToString()).Date < DateTime.Today)
        {
          gvItem.SubItems[index].Font = new Font(this.gvTrades.Font, FontStyle.Bold);
          gvItem.SubItems[index].ForeColor = TradeEditor.AlertColor;
        }
      }
      return gvItem;
    }

    private void refreshContractLookup()
    {
      this.contractLookup = new Dictionary<int, MasterContractSummaryInfo>();
      foreach (MasterContractSummaryInfo allContract in Session.MasterContractManager.GetAllContracts(false))
        this.contractLookup[allContract.ContractID] = allContract;
    }

    private SortField[] generateSortFields()
    {
      List<SortField> sortFieldList = new List<SortField>();
      foreach (GVColumn column in this.gvTrades.Columns)
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
        TableLayout.Column tag = (TableLayout.Column) this.gvTrades.Columns[gvColumnSort.Column].Tag;
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

    private void gvFilterManager_FilterChanged(object sender, EventArgs e)
    {
      this.RefreshList(false);
    }

    private void gvTrades_DoubleClick(object sender, EventArgs e)
    {
      GVHitTestInfo gvHitTestInfo = this.gvTrades.HitTest(this.gvTrades.PointToClient(Cursor.Position));
      if (gvHitTestInfo.RowIndex < 0)
        return;
      if (this.gvTrades.Items[gvHitTestInfo.RowIndex].Tag is LoanTradeViewModel)
        this.selectedTrade = (TradeViewModel) this.gvTrades.Items[gvHitTestInfo.RowIndex].Tag;
      else if (this.gvTrades.Items[gvHitTestInfo.RowIndex].Tag is MbsPoolViewModel)
        this.selectedTrade = (TradeViewModel) this.gvTrades.Items[gvHitTestInfo.RowIndex].Tag;
      this.DialogResult = DialogResult.OK;
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (this.gvTrades.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must first select a " + this.tradeTypeString + " from the list.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        if (this.gvTrades.SelectedItems[0].Tag is LoanTradeViewModel)
          this.selectedTrade = (TradeViewModel) this.gvTrades.SelectedItems[0].Tag;
        else if (this.gvTrades.SelectedItems[0].Tag is MbsPoolViewModel)
          this.selectedTrade = (TradeViewModel) this.gvTrades.SelectedItems[0].Tag;
        this.DialogResult = DialogResult.OK;
      }
    }

    private void navTrades_PageChangedEvent(object sender, PageChangedEventArgs e)
    {
      this.displayCurrentPage(false);
    }

    private void gvTrades_SortItems(object source, GVColumnSortEventArgs e)
    {
      using (CursorActivator.Wait())
      {
        this.retrieveTradeData(this.generateQueryCriteria(), this.generateSortFields(e.ColumnSorts));
        this.displayCurrentPage(true);
      }
    }

    private void form_Resize(object sender, EventArgs e)
    {
      this.navTrades.Left = this.gvTrades.Right - this.navTrades.Width;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.label1 = new Label();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.navTrades = new PageListNavigator();
      this.gvTrades = new GridView();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Location = new Point(9, 13);
      this.label1.Name = "label1";
      this.label1.Size = new Size(266, 14);
      this.label1.TabIndex = 0;
      this.label1.Text = "Select the trade to which to assign the selected loans";
      this.btnOK.Anchor = AnchorStyles.Bottom;
      this.btnOK.Location = new Point(514, 384);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 22);
      this.btnOK.TabIndex = 2;
      this.btnOK.Text = "&OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(597, 384);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 3;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.navTrades.BackColor = Color.Transparent;
      this.navTrades.Font = new Font("Arial", 8f);
      this.navTrades.Location = new Point(423, 4);
      this.navTrades.Name = "navTrades";
      this.navTrades.NumberOfItems = 0;
      this.navTrades.Size = new Size(254, 24);
      this.navTrades.TabIndex = 10;
      this.navTrades.PageChangedEvent += new PageListNavigator.PageChangedEventHandler(this.navTrades_PageChangedEvent);
      this.navTrades.Resize += new EventHandler(this.form_Resize);
      this.gvTrades.AllowMultiselect = false;
      this.gvTrades.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gvTrades.FilterVisible = true;
      this.gvTrades.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvTrades.Location = new Point(10, 32);
      this.gvTrades.Name = "gvTrades";
      this.gvTrades.Size = new Size(661, 342);
      this.gvTrades.SortOption = GVSortOption.Owner;
      this.gvTrades.TabIndex = 1;
      this.gvTrades.SortItems += new GVColumnSortEventHandler(this.gvTrades_SortItems);
      this.gvTrades.DoubleClick += new EventHandler(this.gvTrades_DoubleClick);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(681, 415);
      this.Controls.Add((Control) this.navTrades);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.gvTrades);
      this.Controls.Add((Control) this.label1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (TradeSelector);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Assign to Trade";
      this.Resize += new EventHandler(this.form_Resize);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
