// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.AssignedCorrespondentTradeList
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class AssignedCorrespondentTradeList : UserControl
  {
    private TradeStatusEnumNameProvider tradeStatusNameProvider = new TradeStatusEnumNameProvider();
    private ReportFieldDefs fieldDefs;
    private CorrespondentTradeReportFieldDefs tradeFieldDefs = new CorrespondentTradeReportFieldDefs();
    private FieldFilterList filters;
    private TableLayout tableLayout;
    private GridViewReportFilterManager gvFilterManager;
    private GridViewLayoutManager gvLayoutManager;
    private int cmcID;
    private ICursor tradeCursor;
    private bool allowPublishEvent;
    private IContainer components;
    private TableContainer tableContainer2;
    private ContractProfitControl ctlProfit;
    private GridView gvTrades;
    private PageListNavigator navTrades;

    protected string getTableLayoutFileName() => "CorrespondentTradeSmallScreenView";

    public FieldFilterList Filters => this.filters;

    public AssignedCorrespondentTradeList(bool allowPublishEvent)
    {
      this.InitializeComponent();
      this.allowPublishEvent = allowPublishEvent;
      this.fieldDefs = (ReportFieldDefs) CorrespondentTradeReportFieldDefs.GetFieldDefs(Session.DefaultInstance, allowPublishEvent, false);
    }

    protected ReportFieldDefs getFieldDefs() => this.fieldDefs;

    public void RefreshData(int Id, FieldFilterList filters = null)
    {
      this.cmcID = Id;
      if (this.cmcID <= 0)
        this.gvTrades.Items.Clear();
      this.loadPersonalLayout(filters);
      this.LoadTradeList(Id);
    }

    private void loadPersonalLayout(FieldFilterList filters = null)
    {
      try
      {
        BinaryObject userSettings = Session.User.GetUserSettings(this.getTableLayoutFileName());
        this.setLayout(userSettings == null ? this.getDemoTableLayout() : userSettings.ToObject<TableLayout>(), filters);
      }
      catch (Exception ex)
      {
      }
    }

    private void setLayout(TableLayout layOut, FieldFilterList filters = null)
    {
      this.tableLayout = layOut;
      this.applyTableLayout(layOut);
      this.gvFilterManager.ClearColumnFilters();
    }

    private void loadTrades(FieldFilterList filters = null)
    {
      CorrespondentTradeViewModel[] correspondentTradeViewModelArray = new CorrespondentTradeViewModel[0];
      this.gvFilterManager.ClearColumnFilters();
      if (this.cmcID <= 0)
        return;
      this.refreshTradeList(Session.CorrespondentTradeManager.GetTradeViewsByMasterId(this.cmcID));
    }

    public void SetMasterSummaryInfo(CorrespondentMasterSummaryInfo info)
    {
      this.ctlProfit.SetMasterSummaryInfo(info);
    }

    private void refreshTradeList(CorrespondentTradeViewModel[] trades)
    {
      this.gvTrades.Items.Clear();
      foreach (CorrespondentTradeViewModel trade in trades)
        this.gvTrades.Items.Add(this.createAssignmentGridItem(trade));
    }

    private GVItem createAssignmentGridItem(CorrespondentTradeViewModel tradeInfo)
    {
      GVItem assignmentGridItem = new GVItem();
      for (int index = 0; index < this.gvTrades.Columns.Count; ++index)
      {
        TableLayout.Column tag1 = (TableLayout.Column) this.gvTrades.Columns[index].Tag;
        string tag2 = tag1.Tag;
        string columnId = tag1.ColumnID;
        object obj = (object) null;
        CorrespondentTradeReportFieldDef fieldByCriterionName = (CorrespondentTradeReportFieldDef) this.getFieldDefs().GetFieldByCriterionName(tag1.ColumnID);
        if (fieldByCriterionName != null && tradeInfo[columnId] != null)
          obj = ReportFieldClientExtension.ToDisplayElement(fieldByCriterionName, columnId, (IPropertyDictionary) tradeInfo, (EventHandler) null);
        assignmentGridItem.SubItems[index].Value = obj;
      }
      return assignmentGridItem;
    }

    private void applyTableLayout(TableLayout layout)
    {
      if (this.gvLayoutManager == null)
        this.gvLayoutManager = this.createLayoutManager();
      this.gvLayoutManager.ApplyLayout(layout, false);
      if (this.gvFilterManager == null)
      {
        this.gvFilterManager = new GridViewReportFilterManager(Session.DefaultInstance, this.gvTrades);
        this.gvFilterManager.FilterChanged += new EventHandler(this.gvFilterManager_FilterChanged);
      }
      this.gvFilterManager.CreateColumnFilters(this.getFieldDefs());
    }

    private void gvFilterManager_FilterChanged(object sender, EventArgs e)
    {
      this.LoadTradeList(this.cmcID);
    }

    private GridViewLayoutManager createLayoutManager()
    {
      TableLayout nonCustomizableColumnLayout = new TableLayout();
      GridViewLayoutManager layoutManager = new GridViewLayoutManager(this.gvTrades, this.getFullTableLayout(), this.getDemoTableLayout(), nonCustomizableColumnLayout);
      layoutManager.LayoutChanged += new EventHandler(this.onLayoutChanged);
      return layoutManager;
    }

    private void onLayoutChanged(object sender, EventArgs e)
    {
      this.tableLayout = this.gvLayoutManager.GetCurrentLayout();
      using (BinaryObject data = new BinaryObject((IXmlSerializable) this.tableLayout))
        Session.User.SaveUserSettings(this.getTableLayoutFileName(), data);
      this.applyTableLayout(this.tableLayout);
      this.retrieveTradeData(this.generateQueryCriteria(), this.generateSortFields());
      this.refreshTradeList((CorrespondentTradeViewModel[]) this.tradeCursor.GetItems(0, this.tradeCursor.GetItemCount()));
    }

    private TableLayout getFullTableLayout()
    {
      TableLayout fullTableLayout = new TableLayout();
      foreach (ReportFieldDef fieldDef in (ReportFieldDefContainer) this.getFieldDefs())
      {
        if (fullTableLayout.GetColumnByID(fieldDef.CriterionFieldName) == null)
          fullTableLayout.AddColumn(fieldDef.ToTableLayoutColumn());
      }
      fullTableLayout.SortByDescription();
      return fullTableLayout;
    }

    private QueryCriterion generateQueryCriteria() => this.gvFilterManager.ToQueryCriterion();

    public void LoadTradeList(int cmcID)
    {
      this.cmcID = cmcID;
      this.gvTrades.Items.Clear();
      if (this.cmcID <= 0)
        return;
      this.retrieveTradeData(this.generateQueryCriteria(), this.generateSortFields());
      this.displayCurrentPage(false);
    }

    private void retrieveTradeData(QueryCriterion filter, SortField[] sortFields)
    {
      Cursor.Current = Cursors.WaitCursor;
      try
      {
        this.generateFieldList();
        if (filter == null)
          filter = this.generateQueryCriteria();
        ListValueCriterion criterion = new ListValueCriterion("CorrespondentTradeDetails.CorrespondentMasterID", (Array) new int[1]
        {
          this.cmcID
        }, true);
        filter = filter != null ? filter.And((QueryCriterion) criterion) : (QueryCriterion) criterion;
        if (sortFields == null)
          sortFields = this.generateSortFields();
        if (this.tradeCursor != null)
        {
          this.tradeCursor.Dispose();
          this.tradeCursor = (ICursor) null;
        }
        if (filter == null)
          this.tradeCursor = Session.CorrespondentTradeManager.OpenTradeCursor(new QueryCriterion[0], sortFields, false);
        else
          this.tradeCursor = Session.CorrespondentTradeManager.OpenTradeCursor(new QueryCriterion[1]
          {
            filter
          }, sortFields, false);
        this.navTrades.NumberOfItems = this.tradeCursor.GetItemCount();
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Error loading Trades: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
      CorrespondentTradeViewModel[] correspondentTradeViewModelArray = new CorrespondentTradeViewModel[0];
      if (currentPageItemCount > 0)
        correspondentTradeViewModelArray = (CorrespondentTradeViewModel[]) this.tradeCursor.GetItems(currentPageItemIndex, currentPageItemCount);
      Dictionary<int, bool> dictionary = new Dictionary<int, bool>();
      if (preserveSelections)
      {
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvTrades.Items)
        {
          if (gvItem.Selected)
            dictionary[((TradeBase) gvItem.Tag).TradeID] = true;
        }
      }
      this.gvTrades.Items.Clear();
      for (int index = 0; index < correspondentTradeViewModelArray.Length; ++index)
        this.gvTrades.Items.Add(this.createAssignmentGridItem(correspondentTradeViewModelArray[index]));
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

    private string[] generateFieldList()
    {
      List<string> stringList = new List<string>();
      if (this.gvLayoutManager == null)
        this.createLayoutManager();
      foreach (TableLayout.Column column in this.gvLayoutManager.GetCurrentLayout())
      {
        CorrespondentTradeReportFieldDef fieldByCriterionName = this.tradeFieldDefs.GetFieldByCriterionName(column.ColumnID);
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

    protected TableLayout getDemoTableLayout()
    {
      TableLayout demoTableLayout = new TableLayout();
      demoTableLayout.AddColumn(new TableLayout.Column("CorrespondentTradeDetails.Name", "Commitment #", HorizontalAlignment.Left, 150));
      demoTableLayout.AddColumn(new TableLayout.Column("CorrespondentTradeDetails.CommitmentDate", "Commitment Date", HorizontalAlignment.Left, 120));
      demoTableLayout.AddColumn(new TableLayout.Column("CorrespondentTradeDetails.DeliveryType", "Delivery Type", HorizontalAlignment.Left, 150));
      demoTableLayout.AddColumn(new TableLayout.Column("CorrespondentTradeDetails.TradeAmount", "Trade Amount", HorizontalAlignment.Right, 100));
      demoTableLayout.AddColumn(new TableLayout.Column("CorrespondentTradeDetails.ExpirationDate", "Expiration Date", HorizontalAlignment.Left, 100));
      demoTableLayout.AddColumn(new TableLayout.Column("CorrespondentTradeDetails.DeliveryExpirationDate", "Delivery Expiration Date", HorizontalAlignment.Left, 130));
      return demoTableLayout;
    }

    private void navTrades_PageChangedEvent(object sender, PageChangedEventArgs e)
    {
      this.displayCurrentPage(false);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      GVColumn gvColumn5 = new GVColumn();
      GVColumn gvColumn6 = new GVColumn();
      this.tableContainer2 = new TableContainer();
      this.gvTrades = new GridView();
      this.navTrades = new PageListNavigator();
      this.ctlProfit = new ContractProfitControl();
      this.tableContainer2.SuspendLayout();
      this.SuspendLayout();
      this.tableContainer2.Controls.Add((Control) this.navTrades);
      this.tableContainer2.Controls.Add((Control) this.ctlProfit);
      this.tableContainer2.Controls.Add((Control) this.gvTrades);
      this.tableContainer2.Dock = DockStyle.Fill;
      this.tableContainer2.Location = new Point(0, 0);
      this.tableContainer2.Name = "tableContainer2";
      this.tableContainer2.Size = new Size(673, 150);
      this.tableContainer2.TabIndex = 12;
      this.tableContainer2.Text = "Correspondent Trades";
      this.gvTrades.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Trade ID";
      gvColumn1.Width = 98;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Status";
      gvColumn2.Width = 85;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "Target Delivery";
      gvColumn3.Width = 95;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column4";
      gvColumn4.SortMethod = GVSortMethod.Numeric;
      gvColumn4.Text = "Trade Amt";
      gvColumn4.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn4.Width = 96;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column5";
      gvColumn5.SortMethod = GVSortMethod.Numeric;
      gvColumn5.Text = "Assigned Amt";
      gvColumn5.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn5.Width = 88;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Column6";
      gvColumn6.SortMethod = GVSortMethod.Numeric;
      gvColumn6.Text = "Net Profit";
      gvColumn6.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn6.Width = 78;
      this.gvTrades.Columns.AddRange(new GVColumn[6]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6
      });
      this.gvTrades.Dock = DockStyle.Fill;
      this.gvTrades.FilterVisible = true;
      this.gvTrades.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvTrades.Location = new Point(1, 26);
      this.gvTrades.Name = "gvTrades";
      this.gvTrades.Size = new Size(671, 98);
      this.gvTrades.TabIndex = 0;
      this.gvTrades.TabStop = false;
      this.navTrades.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.navTrades.BackColor = Color.Transparent;
      this.navTrades.Font = new Font("Arial", 8f);
      this.navTrades.Location = new Point(415, 2);
      this.navTrades.Name = "navTrades";
      this.navTrades.NumberOfItems = 0;
      this.navTrades.Size = new Size(254, 23);
      this.navTrades.TabIndex = 10;
      this.navTrades.PageChangedEvent += new PageListNavigator.PageChangedEventHandler(this.navTrades_PageChangedEvent);
      this.ctlProfit.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.ctlProfit.BackColor = Color.Transparent;
      this.ctlProfit.Location = new Point(3, 129);
      this.ctlProfit.Name = "ctlProfit";
      this.ctlProfit.Size = new Size(668, 16);
      this.ctlProfit.TabIndex = 3;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.tableContainer2);
      this.Name = nameof (AssignedCorrespondentTradeList);
      this.Size = new Size(673, 150);
      this.tableContainer2.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
