// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.TradeEditor
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.UI.ReportFieldDefinitions;
using EllieMae.EMLite.ContactUI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.InputEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Reporting;
using EllieMae.EMLite.Setup;
using EllieMae.EMLite.Trading.Notifications;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class TradeEditor : UserControl, IMenuProvider, ITradeEditor, ITradeEditorBase
  {
    private string className = nameof (TradeEditor);
    private const int ControlPadding = 5;
    private static LoanTradeStatusEnumNameProvider tradeStatusNameProvider = new LoanTradeStatusEnumNameProvider();
    private static string sw = Tracing.SwOutsideLoan;
    private static string[] requiredEligibilityFields = new string[1]
    {
      "Loan.InvestorStatus"
    };
    private string standardViewName = "Standard View";
    public static Color AlertColor = Color.FromArgb(204, 51, 51);
    public static Color HighlightColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 231);
    private LoanTradeInfo loanTrade;
    private LoanReportFieldDefs fieldDefs;
    private TradeAssignedLoanFieldDefs tradeAssLoanFieldDefs;
    private TradeAssignmentManager assignments;
    private TabPage pricingTab;
    private bool loading;
    private bool modified;
    private bool readOnly;
    private bool loanUpdatesRequired;
    private bool isPricePageShow = true;
    private LoanTradeInfo lastPricingTradeInfo;
    private TradeFilter lastEvaluatedFilter;
    private string originalTradeName;
    private int originalContractId = -1;
    private SecurityTradeInfo secTradeObj;
    private bool suspendEvents;
    private string investorTemplateName;
    private LoanListScreen ctlLoanList;
    private bool isBulkDelivery;
    private Timer refreshTimer = new Timer();
    private bool subscribed;
    private bool isTradeLoanUpdateEnabled = Session.StartupInfo.EnableTradeLoanUpdateNotification && Utils.ParseBoolean(Session.StartupInfo.TradeSettings[(object) "Trade.TradeLoanUpdateKafka"]);
    private IContainer components;
    private TabControl tabTrade;
    private TabPage tpDetails;
    private TabPage tpPricing;
    private TabPage tpLoans;
    private TabPage tpHistory;
    private Label label8;
    private Label label6;
    private Label label5;
    private Label label4;
    private Label label3;
    private TextBox txtName;
    private Label label1;
    private ComboBox cboContract;
    private TextBox txtTolerance;
    private TextBox txtAmount;
    private TextBox txtInvestorTradeNum;
    private TextBox txtInvestor;
    private TextBox txtInvestorCommitNum;
    private Label label10;
    private Panel pnlFilter;
    private DatePicker dtPurchase;
    private Label label18;
    private DatePicker dtActualDelivery;
    private Label label17;
    private DatePicker dtTargetDelivery;
    private Label label16;
    private DatePicker dtEarlyDelivery;
    private Label label15;
    private DatePicker dtInvestorDelivery;
    private DatePicker dtCommitment;
    private Label label13;
    private Label label33;
    private Label label32;
    private TextBox txtBuyDown;
    private Label label31;
    private TextBox txtBuyUp;
    private Label label30;
    private TextBox txtRateAdjustment;
    private Label label29;
    private Button btnAdjustmentTemplate;
    private PriceAdjustmentListEditor ctlAdjustments;
    private Button btnSRPTemplate;
    private TradePricingEditor ctlPricing;
    private Button btnDateStamp;
    private TextBox txtNotes;
    private GridView gvHistory;
    private AdvancedSearchControl ctlAdvancedSearch;
    private SimpleSearchControl ctlSimpleSearch;
    private SRPTableEditor ctlSRP;
    private ToolTip toolTips;
    private TextBox txtContract;
    private ComboBox cboSearchType;
    private BorderPanel grpEditor;
    private FlowLayoutPanel flowLayoutPanel1;
    private StandardIconButton btnSave;
    private Label label7;
    private StandardIconButton btnInvestorTemplate;
    private Panel pnlDetails;
    private Button btnSavedSearchesAdv;
    private VerticalSeparator vsAdvSearch;
    private Button btnSavedSearchesSimple;
    private Panel pnlPricing;
    private GroupContainer grpBuyUpDown;
    private Panel pnlHistory;
    private GroupContainer grpHistory;
    private GroupContainer grpNotes;
    private StandardIconButton btnExportHistory;
    private StandardIconButton btnList;
    private GradientPanel gradientPanel1;
    private Label lblTradeName;
    private TextBox txtMaxAmt;
    private Label label20;
    private TextBox txtMinAmt;
    private Label label12;
    private Label label24;
    private CollapsibleSplitter collapsibleSplitter1;
    private Panel pnlLeft;
    private Panel pnlRight;
    private CollapsibleSplitter collapsibleSplitter2;
    private Panel pnlRightBottom;
    private Panel panel1;
    private CollapsibleSplitter collapsibleSplitter3;
    private Panel pnlSecurityTradeInfo;
    private GroupContainer groupContainer1;
    private Panel panel2;
    private ComboBox cmbCommitmentType;
    private ComboBox cmbTradeDesc;
    private Label label26;
    private Label label9;
    private SecurityTradeSmallEditor securityTradeInfo;
    private TextBox txtNetProfit;
    private Label label25;
    private TextBox txtMiscFee;
    private Label label23;
    private TextBox txtPairOffAmt;
    private Label label21;
    private TextBox txtGainLoss;
    private Label label19;
    private GroupContainer grpAddress;
    private InvestorAddressEditor ctlContactInfo;
    private TableContainer grpPairOffs;
    private FlowLayoutPanel flpPairOffs;
    private StandardIconButton btnEditPairOff;
    private GridView gvPairOffs;
    private Panel pnlPairOff;
    private TextBox txtServicer;
    private ComboBox cmbServicingType;
    private Label label11;
    private Label label2;
    private StandardIconButton standardIconButton1;
    private ComboBox cmbPricingType;
    private VerticalSeparator vsPricingType;
    private TradeAdvancedPricingEditor ctlAdvancedPricing;
    private FlowLayoutPanel flowLayoutPanel2;
    private PictureBox pictPending;
    private MSRPricingEditor msrPricingEditor1;
    private TextBox txt_WABP;
    private CheckBox checkbox_bulkDelivery;
    private Label label22;
    private Label label14;
    private FieldLockButton lbtn_WABP;

    protected override void Dispose(bool disposing)
    {
      this.unSubscribeEventHandler();
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    public TradeEditor()
    {
      this.InitializeComponent();
      TextBoxFormatter.Attach(this.txtAmount, TextBoxContentRule.NonNegativeDecimal, "#,##0");
      TextBoxFormatter.Attach(this.txtMinAmt, TextBoxContentRule.NonNegativeDecimal, "#,##0");
      TextBoxFormatter.Attach(this.txtMaxAmt, TextBoxContentRule.NonNegativeDecimal, "#,##0");
      TextBoxFormatter.Attach(this.txtTolerance, TextBoxContentRule.NonNegativeDecimal, "0.####;;\"\"");
      TextBoxFormatter.Attach(this.txtMiscFee, TextBoxContentRule.NonNegativeDecimal, "#,##0.00;;\"\"");
      if (Session.SessionObjects.Use10DecimalDigitInLockRequestSecondaryTradeAreas)
      {
        TextBoxFormatter.Attach(this.txtBuyUp, TextBoxContentRule.NonNegativeDecimal, "0.0000000000");
        TextBoxFormatter.Attach(this.txtBuyDown, TextBoxContentRule.NonNegativeDecimal, "0.0000000000");
        this.txtBuyUp.MaxLength += 7;
        this.txtBuyDown.MaxLength += 7;
      }
      else
      {
        TextBoxFormatter.Attach(this.txtBuyUp, TextBoxContentRule.NonNegativeDecimal, "0.000");
        TextBoxFormatter.Attach(this.txtBuyDown, TextBoxContentRule.NonNegativeDecimal, "0.000");
      }
      TextBoxFormatter.Attach(this.txtRateAdjustment, TextBoxContentRule.NonNegativeDecimal, "0.000");
      this.addSearchButtonsToControls();
      this.refreshConfigurableFieldOptions();
      this.resetFieldDefs();
      this.securityTradeInfo.ReadOnly = true;
      this.securityTradeInfo.SecurityCouponUpdated += new EventHandler(this.securityTradeInfo_SecurityCouponUpdated);
      this.securityTradeInfo.SecurityPriceUpdated += new EventHandler(this.securityTradeInfo_SecurityPriceUpdated);
      this.ctlLoanList = new LoanListScreen((ITradeEditor) this);
      this.tpLoans.Controls.Clear();
      this.tpLoans.Controls.Add((Control) this.ctlLoanList);
      this.ctlLoanList.Dock = DockStyle.Fill;
      this.RemovePendingLoanFromOtherTrades = false;
      this.refreshTimer.Interval = 5000;
      this.refreshTimer.Tick += new EventHandler(this.Refresh_Tick);
    }

    public TradeInfoObj CurrentTradeInfo
    {
      get => this.loanTrade != null ? (TradeInfoObj) this.loanTrade : (TradeInfoObj) null;
    }

    public bool Loading
    {
      get => this.loading;
      set => this.loading = value;
    }

    public bool SuspendEvents
    {
      get => this.suspendEvents;
      set => this.suspendEvents = value;
    }

    public object Assignments => (object) this.assignments;

    public TabPage LoanToTradeAssignmentTab => this.tpLoans;

    public ToolTip ToolTip => this.toolTips;

    public LoanTradeInfo CurrentLoanTradeInfo => this.loanTrade;

    public Decimal TradeAmount
    {
      get
      {
        return !string.IsNullOrEmpty(this.txtAmount.Text) ? Utils.ParseDecimal((object) this.txtAmount.Text) : 0M;
      }
    }

    public bool RemovePendingLoanFromOtherTrades { get; set; }

    public void RefreshData(int sqlRead) => this.RefreshData(new LoanTradeInfo(), sqlRead);

    public void RefreshData(LoanTradeInfo loanTrade, int sqlRead)
    {
      this.RefreshData(loanTrade, (string[]) null, (SecurityTradeInfo) null, sqlRead);
    }

    public void RefreshData(SecurityTradeInfo securityTrade, int sqlRead)
    {
      this.RefreshData(new LoanTradeInfo(), (string[]) null, securityTrade, sqlRead);
      SimpleTradeFilter currentFilter = this.ctlSimpleSearch.GetCurrentFilter();
      int maxValue = securityTrade.Term2 == 0 ? int.MaxValue : securityTrade.Term2;
      currentFilter.TermRange = new Range<int>(securityTrade.Term1, maxValue);
      this.ctlSimpleSearch.SetCurrentFilter(currentFilter);
      this.securityTradeInfo.RefreshContents();
    }

    public void RefreshData(LoanTradeInfo loanTrade, string[] loanGuids, int sqlRead)
    {
      this.RefreshData(loanTrade, loanGuids, (SecurityTradeInfo) null, sqlRead);
    }

    public void RefreshData(
      LoanTradeInfo loanTrade,
      string[] loanGuids,
      SecurityTradeInfo securityTrade,
      int sqlRead)
    {
      this.loading = false;
      this.modified = false;
      this.readOnly = false;
      this.loanUpdatesRequired = false;
      this.ctlLoanList.ViewEligibleChecked = false;
      this.ctlLoanList.ResetWithdrawnLoanMessageFlag();
      this.RemovePendingLoanFromOtherTrades = false;
      this.lastPricingTradeInfo = (LoanTradeInfo) null;
      this.lastEvaluatedFilter = (TradeFilter) null;
      this.originalTradeName = (string) null;
      this.originalContractId = -1;
      this.secTradeObj = (SecurityTradeInfo) null;
      this.assignments = (TradeAssignmentManager) null;
      this.secTradeObj = securityTrade;
      this.loanTrade = loanTrade;
      this.loadTradeData(sqlRead);
      this.ctlLoanList.RefreshViews();
      this.placePricingTypeDDLToControls();
      if (this.loanTrade.TradeID <= 0 && this.loanTrade.Filter != null && this.loanTrade.Filter.DataLayout != null)
        this.loanTrade.Filter.DataLayout.InsertColumns(0, this.getFixedEligibleLoanColumns());
      if (this.loanTrade.TradeID <= 0)
        this.tabTrade.SelectedTab = this.tpDetails;
      if (loanGuids != null)
      {
        this.ctlLoanList.PlaceAssignedLoans(loanGuids);
        if (this.loanTrade.TradeID > 0)
          this.tabTrade.SelectedTab = this.tpLoans;
      }
      this.loanUpdatesRequired = false;
    }

    private void securityTradeInfo_SecurityPriceUpdated(object sender, EventArgs e)
    {
      this.ctlAdvancedPricing.SecurityTradeSecurityPrice = Utils.ParseDecimal(sender, 0M);
    }

    private void securityTradeInfo_SecurityCouponUpdated(object sender, EventArgs e)
    {
      this.ctlAdvancedPricing.SecurityTradeCoupon = Utils.ParseDecimal(sender, 0M);
    }

    private void loanLayoutMngr_LayoutChanged(object sender, EventArgs e) => this.loadTradeData(0);

    private void resetFieldDefs()
    {
      this.fieldDefs = LoanReportFieldDefs.GetFieldDefs(Session.DefaultInstance, LoanReportFieldFlags.DatabaseFieldsNoAudit);
      this.tradeAssLoanFieldDefs = TradeAssignedLoanFieldDefs.GetFieldDefs();
      this.ctlAdvancedSearch.FieldDefs = (ReportFieldDefs) this.fieldDefs;
    }

    public bool DataModified
    {
      get
      {
        if (this.readOnly)
          return false;
        return this.modified || this.ctlContactInfo.DataModified || this.ctlPricing.DataModified || this.ctlAdvancedPricing.DataModified || this.ctlAdjustments.DataModified || this.ctlSRP.DataModified || this.ctlLoanList.DataModified || this.isSearchModified() || this.msrPricingEditor1.DataModified || this.assignments != null && this.assignments.HasModifiedAssignments() || this.securityTradeInfo.DataModified;
      }
    }

    public bool LoanUpdatesRequired
    {
      get
      {
        if (this.readOnly)
          return false;
        return this.loanUpdatesRequired || this.ctlContactInfo.DataModified || this.ctlPricing.DataModified || this.msrPricingEditor1.DataModified || this.ctlAdvancedPricing.DataModified || this.ctlAdjustments.DataModified || this.ctlSRP.DataModified;
      }
    }

    public bool ReadOnly
    {
      get => this.readOnly;
      set
      {
        this.readOnly = value;
        this.setReadOnly();
      }
    }

    private void setReadOnly()
    {
      this.txtName.ReadOnly = this.readOnly;
      this.txtContract.Text = string.Concat(this.cboContract.SelectedItem);
      this.txtContract.Visible = this.readOnly;
      this.cboContract.Visible = !this.readOnly;
      this.btnInvestorTemplate.Enabled = !this.readOnly;
      this.standardIconButton1.Enabled = !this.readOnly;
      this.txtInvestor.ReadOnly = this.readOnly;
      this.txtInvestorTradeNum.ReadOnly = this.readOnly;
      this.txtInvestorCommitNum.ReadOnly = this.readOnly;
      this.txt_WABP.ReadOnly = this.readOnly;
      this.checkbox_bulkDelivery.Enabled = !this.readOnly;
      this.lbtn_WABP.Enabled = !this.readOnly;
      this.txtAmount.ReadOnly = this.readOnly;
      this.txtTolerance.ReadOnly = this.readOnly;
      this.cmbCommitmentType.Enabled = !this.readOnly;
      this.cmbTradeDesc.Enabled = !this.readOnly;
      this.txtMiscFee.ReadOnly = this.readOnly;
      this.cmbServicingType.Enabled = !this.readOnly;
      this.txtServicer.ReadOnly = this.readOnly;
      this.btnEditPairOff.Enabled = !this.readOnly;
      this.ctlContactInfo.ReadOnly = this.readOnly;
      this.cboSearchType.Enabled = !this.readOnly;
      this.btnSavedSearchesSimple.Visible = !this.readOnly;
      this.btnSavedSearchesAdv.Visible = !this.readOnly;
      this.ctlSimpleSearch.ReadOnly = this.readOnly;
      this.ctlAdvancedSearch.ReadOnly = this.readOnly;
      this.dtCommitment.ReadOnly = this.readOnly;
      this.dtInvestorDelivery.ReadOnly = this.readOnly;
      this.dtEarlyDelivery.ReadOnly = this.readOnly;
      this.dtTargetDelivery.ReadOnly = this.readOnly;
      this.dtActualDelivery.ReadOnly = this.readOnly;
      this.dtPurchase.ReadOnly = this.readOnly;
      this.ctlPricing.ReadOnly = this.readOnly;
      this.msrPricingEditor1.ReadOnly = this.readOnly;
      this.ctlAdvancedPricing.ReadOnly = this.readOnly;
      this.txtRateAdjustment.ReadOnly = this.readOnly;
      this.txtBuyUp.ReadOnly = this.readOnly;
      this.txtBuyDown.ReadOnly = this.readOnly;
      this.ctlAdjustments.ReadOnly = this.readOnly;
      this.btnAdjustmentTemplate.Visible = !this.readOnly;
      this.ctlSRP.ReadOnly = this.readOnly;
      this.btnSRPTemplate.Visible = !this.readOnly;
      this.cmbPricingType.Enabled = !this.readOnly;
      this.btnExportHistory.Enabled = this.loanTrade.Status != TradeStatus.Pending;
      this.ctlLoanList.ReadOnly = this.readOnly;
      this.txtNotes.ReadOnly = this.readOnly;
      this.btnDateStamp.Visible = !this.readOnly;
      this.securityTradeInfo.ReadOnly = this.readOnly || this.secTradeObj != null;
      this.securityTradeInfo.RefreshContents();
      this.btnSave.Enabled = !this.readOnly;
    }

    private void refreshContracts(
      MasterContractSummaryInfo[] contracts,
      MasterContractSummaryInfo currentContract)
    {
      bool flag = true;
      this.cboContract.Items.Clear();
      this.cboContract.Items.Add((object) "N/A");
      foreach (MasterContractSummaryInfo contract in contracts)
      {
        if (currentContract != null && contract.ContractID == currentContract.ContractID)
          flag = false;
        this.cboContract.Items.Add((object) contract);
      }
      if (!flag || currentContract == null)
        return;
      this.cboContract.Items.Add((object) currentContract);
    }

    private static System.Type getSortTypeForFieldDef(LoanReportFieldDef fieldDef)
    {
      if (fieldDef.FieldType == EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsDate)
        return typeof (ListViewDateSort);
      return fieldDef.FieldType == EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsNumeric ? typeof (ListViewCurrencySort) : typeof (ListViewTextCaseInsensitiveSort);
    }

    public List<TradeLoanUpdateError> AssignLoanToTrade(PipelineInfo[] pinfos)
    {
      List<TradeLoanUpdateError> trade = new List<TradeLoanUpdateError>();
      foreach (PipelineInfo pinfo in pinfos)
      {
        try
        {
          Decimal totalPrice = 0M;
          if (this.IsCalculatedWABP())
          {
            if (this.ctlLoanList.LoansToAssign != null && this.ctlLoanList.LoansToAssign.ContainsKey(pinfo.LoanNumber))
            {
              GVItem gvItem = this.ctlLoanList.LoansToAssign[pinfo.LoanNumber];
              if (gvItem.SubItems.Count == 2 || gvItem.SubItems.Count == 3)
                totalPrice = Utils.ParseDecimal((object) (gvItem.SubItems[1].Value as string));
            }
          }
          else if (this.IsBulkPricing())
            totalPrice = Decimal.Parse(this.txt_WABP.Text.Trim());
          this.assignments.AssignLoan(pinfo, totalPrice);
          this.RefreshWeightAvgBulkPrice();
        }
        catch (Exception ex)
        {
          trade.Add(new TradeLoanUpdateError(pinfo.GUID, pinfo, ex.Message));
        }
      }
      this.modified = true;
      return trade;
    }

    public void RemoveLoanFromTrade(string guid, bool rejected)
    {
      this.assignments.RemoveLoan(guid, rejected);
    }

    public string[] GetLoanToTradeAssignmentAllLoanGuids() => this.assignments.GetAllLoanGuids();

    public List<LoanToTradeAssignmentBase> GetLoanToTradeAssignments()
    {
      List<LoanToTradeAssignmentBase> tradeAssignments = new List<LoanToTradeAssignmentBase>();
      if (this.assignments == null)
        return tradeAssignments;
      foreach (LoanTradeAssignment assignment in this.assignments)
        tradeAssignments.Add((LoanToTradeAssignmentBase) assignment);
      return tradeAssignments;
    }

    public bool ValidateLoanToTradeAssignment(
      LoanToTradeAssignmentBase assignment,
      out string errMsg)
    {
      errMsg = string.Empty;
      if (((LoanTradeAssignment) assignment).AssignedStatus < LoanTradeStatus.Shipped)
        return true;
      errMsg = "One or more of the selected loans is already marked as Shipped or Purchased. Are you sure you want to remove these loans from the trade?";
      return false;
    }

    public string GetInvestorText() => this.txtInvestor.Text;

    public PipelineInfo[] GetLoanToTradeAssignedPipelineData()
    {
      return this.assignments == null ? new PipelineInfo[0] : this.assignments.GetAssignedPipelineData();
    }

    public int GetLoanToTradePendingAssignmentCount()
    {
      int pendingAssignmentCount = 0;
      if (this.assignments == null)
        return 0;
      foreach (LoanTradeAssignment pendingLoan in this.assignments.GetPendingLoans())
      {
        if (pendingLoan.PendingStatus == LoanTradeStatus.Assigned)
          ++pendingAssignmentCount;
      }
      return pendingAssignmentCount;
    }

    public int GetLoanToTradePendingShipmentCount()
    {
      int pendingShipmentCount = 0;
      if (this.assignments == null)
        return 0;
      foreach (LoanTradeAssignment pendingLoan in this.assignments.GetPendingLoans())
      {
        if (pendingLoan.PendingStatus == LoanTradeStatus.Shipped)
          ++pendingShipmentCount;
      }
      return pendingShipmentCount;
    }

    public int GetLoanToTradePendingPurchaseCount()
    {
      int pendingPurchaseCount = 0;
      if (this.assignments == null)
        return 0;
      foreach (LoanTradeAssignment pendingLoan in this.assignments.GetPendingLoans())
      {
        if (pendingLoan.PendingStatus == LoanTradeStatus.Purchased)
          ++pendingPurchaseCount;
      }
      return pendingPurchaseCount;
    }

    public int GetLoanToTradePendingRemovalCount()
    {
      int pendingRemovalCount = 0;
      if (this.assignments == null)
        return 0;
      foreach (LoanTradeAssignment pendingLoan in this.assignments.GetPendingLoans())
      {
        if (pendingLoan.PendingStatus == LoanTradeStatus.Unassigned)
          ++pendingRemovalCount;
      }
      return pendingRemovalCount;
    }

    public string[] GetLoanToTradeAssignedAndRejectedLoanGuids()
    {
      return this.assignments != null ? this.assignments.GetAssignedAndRejectedLoanGuids() : (string[]) null;
    }

    public string[] GetLoanToTradeAssignedAndRejectedLoanNumbers()
    {
      return this.assignments != null ? this.assignments.GetAssignedAndRejectedLoanNumbers() : (string[]) null;
    }

    public void MarkLoanToTradeAssignmentStatusToShipped(string loanGuid)
    {
      if (this.assignments != null)
        this.assignments.ModifyLoanStatus(loanGuid, LoanTradeStatus.Shipped);
      if (!(this.loanTrade.ShipmentDate == DateTime.MinValue))
        return;
      this.dtActualDelivery.Value = DateTime.Now;
    }

    public void MarkLoanToTradeAssignmentStatusToPurchasedPending(string loanGuid)
    {
      if (this.assignments != null)
        this.assignments.ModifyLoanStatus(loanGuid, LoanTradeStatus.Purchased);
      if (!(this.loanTrade.PurchaseDate == DateTime.MinValue))
        return;
      this.dtPurchase.Value = DateTime.Now;
    }

    public bool IsLoanToTradeAssignmentPending(LoanToTradeAssignmentBase assignment)
    {
      return ((LoanTradeAssignment) assignment).Pending;
    }

    public void CommitLoanToTradeAssignments(bool forceUpdateOfAllLoans, bool updatedSelected)
    {
      this.commitTradeAssignments(forceUpdateOfAllLoans, updatedSelected);
      this.RefreshLoans();
    }

    public List<LoanToTradeAssignmentBase> GetLoanToTradeAssignedLoans()
    {
      List<LoanToTradeAssignmentBase> tradeAssignedLoans = new List<LoanToTradeAssignmentBase>();
      if (this.assignments == null)
        return tradeAssignedLoans;
      foreach (LoanTradeAssignment assignedLoan in this.assignments.GetAssignedLoans())
        tradeAssignedLoans.Add((LoanToTradeAssignmentBase) assignedLoan);
      return tradeAssignedLoans;
    }

    public bool IsBulkPricing() => this.checkbox_bulkDelivery.Checked;

    public bool IsCalculatedWABP() => this.IsBulkPricing() && !this.lbtn_WABP.Locked;

    public List<LoanToTradeAssignmentBase> GetAllAssignedPendingLoans()
    {
      List<LoanToTradeAssignmentBase> assignedPendingLoans = new List<LoanToTradeAssignmentBase>();
      if (this.assignments == null)
        return assignedPendingLoans;
      foreach (LoanTradeAssignment assignedPendingLoan in this.assignments.GetAllAssignedPendingLoans())
        assignedPendingLoans.Add((LoanToTradeAssignmentBase) assignedPendingLoan);
      return assignedPendingLoans;
    }

    private bool isSearchModified()
    {
      return this.cboSearchType.SelectedIndex == 0 ? this.ctlSimpleSearch.DataModified : this.ctlAdvancedSearch.DataModified;
    }

    private void resetOriginalTradeData()
    {
      this.originalTradeName = this.loanTrade.Name.Trim();
      this.originalContractId = this.loanTrade.ContractID;
    }

    public bool IsNoteRateAllowed(PipelineInfo pinfo) => this.loanTrade.IsNoteRateAllowed(pinfo);

    public string[] GetPricingAndEligibilityFields()
    {
      return this.loanTrade.GetPricingAndEligibilityFields();
    }

    public void LoadTradeData() => this.loadTradeData(0);

    public void LoadTradeData(int sqlRead) => this.loadTradeData(sqlRead);

    public string GetLoanStatusDescription(object value)
    {
      return TradeEditor.tradeStatusNameProvider.GetName((object) (LoanTradeStatus) Utils.ParseInt(value, 1));
    }

    public Decimal CalculatePriceIndex(PipelineInfo info)
    {
      return this.loanTrade.CalculatePriceIndex(info, true, 0M);
    }

    public Decimal CalculatePriceIndex(PipelineInfo info, Decimal securityPrice)
    {
      return this.loanTrade.CalculatePriceIndex(info, true, securityPrice);
    }

    public Decimal CalculateProfit(PipelineInfo info, Decimal securityPrice)
    {
      return this.loanTrade.CalculateProfit(info, securityPrice);
    }

    public ICursor GetEligibleLoanCursor(
      string[] fields,
      PipelineData dataToInclude,
      SortField[] sortFields,
      string[] excludedGuids,
      EligibleLoanFilterOption filterOption = EligibleLoanFilterOption.All)
    {
      return Session.LoanTradeManager.GetEligibleLoanCursor(this.loanTrade, fields, dataToInclude, sortFields, excludedGuids, false, filterOption);
    }

    public void RefreshLoans()
    {
      if (this.loanTrade == null)
        return;
      this.loading = true;
      this.loadLoans(Session.LoanTradeManager.GetTradeEditorScreenData(this.loanTrade.TradeID, this.ctlLoanList.GetAssignedLoanListFields(), false).AssignedLoans, false);
      this.loadSecurityTradeData();
      this.loadProfitabilityData();
    }

    private void loadTradeData(int sqlRead)
    {
      if (this.loanTrade == null)
        return;
      this.loading = true;
      TradeEditorScreenData editorScreenData = Session.LoanTradeManager.GetTradeEditorScreenData(this.loanTrade.TradeID, this.ctlLoanList.GetAssignedLoanListFields(), false, sqlRead);
      this.refreshContracts(editorScreenData.ActiveContracts, (MasterContractSummaryInfo) editorScreenData.AssignedContract);
      this.lblTradeName.Text = this.loanTrade.TradeID > 0 ? "Trade " + this.loanTrade.Name : "New Trade";
      this.txtName.Text = this.loanTrade.Name;
      this.cmbCommitmentType.Text = this.loanTrade.CommitmentType;
      this.cmbTradeDesc.Text = this.loanTrade.TradeDescription;
      this.txtInvestorTradeNum.Text = this.loanTrade.InvestorTradeNumber;
      this.txtInvestorCommitNum.Text = this.loanTrade.InvestorCommitmentNumber;
      this.txtAmount.Text = this.loanTrade.TradeAmount.ToString("#,##0;;\"\"");
      this.txtTolerance.Text = this.loanTrade.Tolerance == 0M ? "" : this.loanTrade.Tolerance.ToString("0.####;;\"\"");
      TextBox txtMiscFee = this.txtMiscFee;
      Decimal num = this.loanTrade.MiscAdjustment;
      string str1 = num.ToString("#,##0.00;;\"\"");
      txtMiscFee.Text = str1;
      TextBox txtRateAdjustment = this.txtRateAdjustment;
      num = this.loanTrade.RateAdjustment;
      string str2 = num.ToString("0.000;;\"\"");
      txtRateAdjustment.Text = str2;
      if (Session.SessionObjects.Use10DecimalDigitInLockRequestSecondaryTradeAreas)
      {
        TextBox txtBuyUp = this.txtBuyUp;
        num = this.loanTrade.BuyUpAmount;
        string str3 = num.ToString("0.0000000000;;\"\"");
        txtBuyUp.Text = str3;
        TextBox txtBuyDown = this.txtBuyDown;
        num = this.loanTrade.BuyDownAmount;
        string str4 = num.ToString("0.0000000000;;\"\"");
        txtBuyDown.Text = str4;
      }
      else
      {
        TextBox txtBuyUp = this.txtBuyUp;
        num = this.loanTrade.BuyUpAmount;
        string str5 = num.ToString("0.000;;\"\"");
        txtBuyUp.Text = str5;
        TextBox txtBuyDown = this.txtBuyDown;
        num = this.loanTrade.BuyDownAmount;
        string str6 = num.ToString("0.000;;\"\"");
        txtBuyDown.Text = str6;
      }
      switch (this.loanTrade.ServicingType)
      {
        case ServicingType.ServicingReleased:
          this.cmbServicingType.SelectedIndex = 0;
          break;
        case ServicingType.ServicingRetained:
          this.cmbServicingType.SelectedIndex = 1;
          break;
        default:
          this.cmbServicingType.SelectedIndex = -1;
          break;
      }
      this.txtServicer.Text = this.loanTrade.Servicer;
      this.txtNotes.Text = this.loanTrade.Notes;
      TextBox txtWabp = this.txt_WABP;
      num = this.loanTrade.WeightedAvgBulkPrice;
      string str7 = num.ToString("0.00000;;\"\"");
      txtWabp.Text = str7;
      this.isBulkDelivery = this.loanTrade.IsBulkDelivery;
      this.lbtn_WABP.Locked = this.loanTrade.IsWeightedAvgBulkPriceLocked;
      this.ctlPricing.PricingItems = this.loanTrade.Pricing.SimplePricingItems;
      this.msrPricingEditor1.PricingItems = this.loanTrade.Pricing.MSRPricingItems;
      this.ctlAdvancedPricing.PricingInfo = this.loanTrade.Pricing.AdvancedPricingInfo;
      this.ctlAdjustments.Adjustments = this.loanTrade.PriceAdjustments;
      this.ctlSRP.SRPTable = this.loanTrade.SRPTable;
      if (this.loanTrade.Pricing.IsAdvancedPricing)
        this.cmbPricingType.SelectedIndex = 1;
      else
        this.cmbPricingType.SelectedIndex = 0;
      this.dtCommitment.Value = this.loanTrade.CommitmentDate;
      this.dtInvestorDelivery.Value = this.loanTrade.InvestorDeliveryDate;
      this.dtEarlyDelivery.Value = this.loanTrade.EarlyDeliveryDate;
      this.dtTargetDelivery.Value = this.loanTrade.TargetDeliveryDate;
      this.dtActualDelivery.Value = this.loanTrade.ShipmentDate;
      this.dtPurchase.Value = this.loanTrade.PurchaseDate;
      this.loadContractData();
      this.loadInvestorData();
      this.loadPairOffs();
      this.loadSearch();
      this.loadHistory(editorScreenData.TradeHistory);
      this.loadLoans(editorScreenData.AssignedLoans);
      this.loadSecurityTradeData();
      this.loadProfitabilityData();
      this.SetupPricingGrid(this.cmbServicingType.Text);
      if (this.secTradeObj != null)
      {
        this.ctlAdvancedPricing.SecurityTradeCoupon = this.secTradeObj.Coupon;
        this.ctlAdvancedPricing.SecurityTradeSecurityPrice = this.secTradeObj.Price;
      }
      this.resetOriginalTradeData();
      this.ReadOnly = this.loanTrade.Status == TradeStatus.Archived || this.loanTrade.Status == TradeStatus.Pending;
      this.MakePending(this.loanTrade.Status == TradeStatus.Pending);
      this.loading = false;
      this.modified = false;
      if (this.loanTrade.IsCloned)
        this.modified = this.loanTrade.TradeID <= 0;
      this.checkbox_bulkDelivery.Checked = this.loanTrade.IsBulkDelivery;
      if (this.checkbox_bulkDelivery.Checked)
        this.ctlLoanList.DisableViewEligible();
      else
        this.ctlLoanList.EnableViewEligible();
      this.lbtn_WABP.Enabled = this.checkbox_bulkDelivery.Checked;
      if (this.lbtn_WABP.Enabled)
        this.txt_WABP.Enabled = this.lbtn_WABP.Locked;
      else
        this.txt_WABP.Enabled = false;
      this.DisplayPricingPage();
      this.RefreshWeightAvgBulkPrice();
    }

    public void MakePending(bool value)
    {
      if (value)
        this.ReadOnly = value;
      if (value)
      {
        this.gradientPanel1.GradientColor1 = Color.FromArgb((int) byte.MaxValue, 0, 0);
        this.gradientPanel1.GradientColor2 = Color.FromArgb((int) byte.MaxValue, 192, 192);
        this.pictPending.Visible = true;
        if (this.isTradeLoanUpdateEnabled)
        {
          this.subscribeEventHandler();
        }
        else
        {
          if (this.refreshTimer.Enabled)
            return;
          Tracing.Log(TradeEditor.sw, this.className, TraceLevel.Info, "Starting timer for loan trade");
          this.refreshTimer.Start();
        }
      }
      else
      {
        this.gradientPanel1.GradientColor1 = Color.FromArgb(81, 123, 184);
        this.gradientPanel1.GradientColor2 = Color.FromArgb(167, 201, 239);
        this.pictPending.Visible = false;
        this.unSubscribeEventHandler();
        if (!this.refreshTimer.Enabled)
          return;
        this.stopTimer();
      }
    }

    private void loadSecurityTradeData()
    {
      if (this.loanTrade.TradeID < 0)
      {
        if (this.secTradeObj == null)
          this.securityTradeInfo.Init((SecurityTradeInfo) null, (LoanTradeInfo) null);
        else
          this.securityTradeInfo.Init(this.secTradeObj, (LoanTradeInfo) null);
      }
      else
      {
        if (this.secTradeObj == null && this.loanTrade.SecurityTradeID > 0)
          this.secTradeObj = Session.SecurityTradeManager.GetTrade(this.loanTrade.SecurityTradeID);
        this.securityTradeInfo.Init(this.secTradeObj, this.loanTrade);
      }
    }

    private void loadContractData()
    {
      this.cboContract.SelectedIndex = 0;
      if (this.loanTrade.ContractID <= 0)
        return;
      for (int index = 1; index < this.cboContract.Items.Count; ++index)
      {
        MasterContractSummaryInfo contractSummaryInfo = (MasterContractSummaryInfo) this.cboContract.Items[index];
        if (contractSummaryInfo.ContractID == this.loanTrade.ContractID)
        {
          this.cboContract.SelectedItem = (object) contractSummaryInfo;
          break;
        }
      }
    }

    private void loadInvestorData()
    {
      this.txtInvestor.Text = this.loanTrade.InvestorName;
      this.ctlContactInfo.CurrentInvestor = this.loanTrade.Investor;
      this.ctlContactInfo.CurrentAssignee = this.loanTrade.Assignee;
    }

    private void loadLoans(PipelineInfo[] pinfos, bool updateLoansTab = true)
    {
      if (pinfos == null)
        pinfos = new PipelineInfo[0];
      this.assignments = new TradeAssignmentManager(Session.SessionObjects, this.loanTrade.TradeID, pinfos);
      if (!updateLoansTab)
        return;
      this.ctlLoanList.RefreshData(pinfos);
    }

    private string getPendingStatusDescription(
      int pendingAssignment,
      int pendingShipment,
      int pendingRemoval)
    {
      List<string> stringList = new List<string>();
      if (pendingAssignment > 0)
        stringList.Add(pendingAssignment.ToString() + " Assigned - Pending");
      if (pendingShipment > 0)
        stringList.Add(pendingShipment.ToString() + " Shipped - Pending");
      if (pendingRemoval > 0)
        stringList.Add(pendingRemoval.ToString() + " Removed - Pending");
      return string.Join(Environment.NewLine, stringList.ToArray());
    }

    private bool isAssignedLoanEligible(TradeFilterEvaluator evaluator, PipelineInfo pinfo)
    {
      if (!evaluator.Evaluate(pinfo, FilterEvaluationOption.NonVolatile))
        return false;
      return this.loanTrade.Pricing.SimplePricingItems.Count <= 0 || this.loanTrade.IsNoteRateAllowed(pinfo);
    }

    private void displayItemAlert(GVItem item, int subitemIndex)
    {
      item.SubItems[subitemIndex].Font = new Font(this.Font, FontStyle.Bold);
      item.SubItems[subitemIndex].ForeColor = TradeEditor.AlertColor;
    }

    private object translateFieldValue(
      PipelineInfo pInfo,
      string fieldName,
      string fieldValue,
      Control parentControl)
    {
      ReportFieldDef fieldByCriterionName = this.tradeAssLoanFieldDefs.GetFieldByCriterionName(fieldName);
      switch (fieldByCriterionName)
      {
        case null:
          return (object) fieldValue;
        case LoanReportFieldDef _:
          return ((LoanReportFieldDef) fieldByCriterionName).ToDisplayElement(fieldName, pInfo, parentControl);
        case TradeReportFieldDef _:
          return ((TradeReportFieldDef) fieldByCriterionName).ToDisplayElement(fieldName, pInfo.Info[(object) fieldName], (EventHandler) null);
        default:
          return (object) fieldByCriterionName.ToDisplayValue(fieldValue);
      }
    }

    public string GetTradeStatusDescription(LoanToTradeAssignmentBase assignmentInfo)
    {
      LoanTradeAssignment loanTradeAssignment = (LoanTradeAssignment) assignmentInfo;
      string name = TradeEditor.tradeStatusNameProvider.GetName((object) loanTradeAssignment.Status);
      if (loanTradeAssignment.Status == LoanTradeStatus.Unassigned)
        return "Removed - Pending";
      if (loanTradeAssignment.Pending)
        name += " - Pending";
      return name;
    }

    private void loadHistory(LoanTradeHistoryItem[] historyItems)
    {
      this.btnExportHistory.Enabled = false;
      this.gvHistory.Items.Clear();
      if (historyItems == null)
        return;
      foreach (LoanTradeHistoryItem historyItem in historyItems)
        this.gvHistory.Items.Add(this.createTradeHistoryListItem(historyItem));
      this.btnExportHistory.Enabled = this.gvHistory.Items.Count > 0;
    }

    private GVItem createTradeHistoryListItem(LoanTradeHistoryItem historyItem)
    {
      return new GVItem()
      {
        Text = historyItem.Timestamp.ToString("MM/dd/yyyy h:mm tt"),
        SubItems = {
          (object) historyItem.Description,
          (object) historyItem.UserName
        },
        Tag = (object) historyItem
      };
    }

    private void loadPairOffs()
    {
      this.gvPairOffs.Items.Clear();
      for (int count = this.loanTrade.LoanTradePairOffs.Count; count < 4; ++count)
        this.loanTrade.LoanTradePairOffs.Add(new LoanTradePairOff());
      for (int index = 0; index < this.loanTrade.LoanTradePairOffs.Count; ++index)
      {
        LoanTradePairOff loanTradePairOff = this.loanTrade.LoanTradePairOffs[index];
        GVItem gvItem = new GVItem("Pair-Off " + (object) (index + 1));
        if (loanTradePairOff.Date != DateTime.MinValue)
          gvItem.SubItems.Add((object) loanTradePairOff.Date.ToString("MM/dd/yyyy"));
        else
          gvItem.SubItems.Add((object) "");
        if (loanTradePairOff.TradeAmount > 0M)
          gvItem.SubItems.Add((object) loanTradePairOff.DisplayedTradeAmount.ToString("#,0.00#"));
        else
          gvItem.SubItems.Add((object) "");
        if (loanTradePairOff.PairOffFeePercentage != 0M)
          gvItem.SubItems.Add((object) loanTradePairOff.PairOffFeePercentage.ToString("N5"));
        else
          gvItem.SubItems.Add((object) "");
        if (loanTradePairOff.CalculatedPairOffFee != 0M)
          gvItem.SubItems.Add((object) loanTradePairOff.DisplayCalculatedPairOffFee.ToString("#,0.00#"));
        else
          gvItem.SubItems.Add((object) "");
        gvItem.Tag = (object) loanTradePairOff;
        this.gvPairOffs.Items.Add(gvItem);
      }
    }

    private void loadSearch()
    {
      if (this.loanTrade.Filter == null)
      {
        this.switchToSimpleMode(new SimpleTradeFilter());
        this.ctlSimpleSearch.DataModified = false;
      }
      else if (this.loanTrade.Filter.FilterType == TradeFilterType.Advanced)
      {
        this.switchToAdvancedMode(this.loanTrade.Filter.GetAdvancedFilter());
        this.ctlSimpleSearch.DataModified = false;
      }
      else
      {
        this.switchToSimpleMode(this.loanTrade.Filter.GetSimpleFilter());
        this.ctlAdvancedSearch.DataModified = false;
      }
      if (this.loanTrade.Filter == null || this.loanTrade.Filter.DataLayout == null)
        return;
      this.ctlLoanList.ClearCurrentEligibilityCursor();
      this.ctlLoanList.ValidateTableLayout(this.loanTrade.Filter.DataLayout);
      this.ctlLoanList.ApplyLayout(this.loanTrade.Filter.DataLayout, false);
    }

    private void switchToSimpleMode(SimpleTradeFilter filter)
    {
      bool dataModified = this.DataModified;
      this.ctlSimpleSearch.SetCurrentFilter(filter);
      this.ctlSimpleSearch.Visible = true;
      this.ctlAdvancedSearch.Visible = false;
      this.cboSearchType.SelectedIndex = 0;
      this.modified = dataModified;
    }

    private void switchToAdvancedMode(FieldFilterList filters)
    {
      bool dataModified = this.DataModified;
      this.ctlAdvancedSearch.SetCurrentFilter(filters);
      this.ctlAdvancedSearch.Visible = true;
      this.ctlSimpleSearch.Visible = false;
      this.cboSearchType.SelectedIndex = 1;
      this.modified = dataModified;
    }

    private void loadProfitabilityData()
    {
      this.txtPairOffAmt.Text = this.loanTrade.PairOffAmount.ToString("#,##0.00;;\"\"");
      List<PipelineInfo> pipelineInfoList = new List<PipelineInfo>();
      foreach (LoanTradeAssignment assignedLoan in this.assignments.GetAssignedLoans())
        pipelineInfoList.Add(assignedLoan.PipelineInfo);
      TextBox txtGainLoss = this.txtGainLoss;
      Decimal num = this.loanTrade.CalculateGainLoss(pipelineInfoList.ToArray(), this.secTradeObj) + this.loanTrade.LoanTradePairOffs.GetDisplayCalculatedPairOffFee();
      string str1 = num.ToString("#,##0.00;;\"\"");
      txtGainLoss.Text = str1;
      TextBox txtNetProfit = this.txtNetProfit;
      num = this.loanTrade.CalculateNetProfit(pipelineInfoList.ToArray(), this.secTradeObj) + this.loanTrade.LoanTradePairOffs.GetDisplayCalculatedPairOffFee();
      string str2 = num.ToString("#,##0.00;;\"\"");
      txtNetProfit.Text = str2;
    }

    private void addSearchButtonsToControls()
    {
      this.vsAdvSearch.Parent = (Control) null;
      this.ctlAdvancedSearch.AddControlToHeader((Control) this.vsAdvSearch);
      this.btnSavedSearchesAdv.Parent = (Control) null;
      this.ctlAdvancedSearch.AddControlToHeader((Control) this.btnSavedSearchesAdv);
      this.btnSavedSearchesSimple.Parent = (Control) null;
      this.ctlSimpleSearch.AddControlToHeader((Control) this.btnSavedSearchesSimple);
      this.btnAdjustmentTemplate.Parent = (Control) null;
      this.ctlAdjustments.AddControlToHeader((Control) new VerticalSeparator());
      this.ctlAdjustments.AddControlToHeader((Control) this.btnAdjustmentTemplate);
      this.btnSRPTemplate.Parent = (Control) null;
      this.ctlSRP.AddControlToHeader((Control) new VerticalSeparator());
      this.ctlSRP.AddControlToHeader((Control) this.btnSRPTemplate);
    }

    private void placePricingTypeDDLToControls()
    {
      this.cmbPricingType.Parent = (Control) null;
      this.vsPricingType.Parent = (Control) null;
      if (!this.loanTrade.Pricing.IsAdvancedPricing)
      {
        this.ctlPricing.AddControlToHeader((Control) this.cmbPricingType, true);
        this.ctlPricing.AddControlToHeader((Control) this.vsPricingType, true);
      }
      else
      {
        this.ctlAdvancedPricing.AddControlToHeader((Control) this.cmbPricingType, true);
        this.ctlAdvancedPricing.AddControlToHeader((Control) this.vsPricingType, true);
      }
    }

    private void cboSearchType_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.cboSearchType.SelectedIndex == 0 && this.ctlAdvancedSearch.Visible)
      {
        if (!this.loading && this.ctlAdvancedSearch.GetCurrentFilter().Count > 0 && Utils.Dialog((IWin32Window) this, "If you switch to Simple Search, you will lose all of your search criteria.", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK)
        {
          this.cboSearchType.SelectedIndex = 1;
        }
        else
        {
          this.switchToSimpleMode(new SimpleTradeFilter());
          this.modified = true;
        }
      }
      else
      {
        if (this.cboSearchType.SelectedIndex != 1 || !this.ctlSimpleSearch.Visible)
          return;
        FieldFilterList filters = (FieldFilterList) null;
        if (!this.loading)
          filters = this.ctlSimpleSearch.GetCurrentFilter().ConvertToFilterList();
        this.switchToAdvancedMode(filters);
        this.modified = true;
      }
    }

    public void CommitChanges() => this.commitChanges();

    private void commitChanges()
    {
      this.modified = this.DataModified;
      this.loanUpdatesRequired = this.LoanUpdatesRequired;
      this.loanTrade.Name = this.txtName.Text.Trim();
      this.loanTrade.CommitmentType = this.cmbCommitmentType.Text;
      this.loanTrade.TradeDescription = this.cmbTradeDesc.Text;
      this.loanTrade.InvestorName = this.txtInvestor.Text;
      this.loanTrade.InvestorTradeNumber = this.txtInvestorTradeNum.Text;
      this.loanTrade.InvestorCommitmentNumber = this.txtInvestorCommitNum.Text;
      this.loanTrade.TradeAmount = Utils.ParseDecimal((object) this.txtAmount.Text);
      this.loanTrade.Tolerance = Utils.ParseDecimal((object) this.txtTolerance.Text);
      this.loanTrade.MiscAdjustment = Utils.ParseDecimal((object) this.txtMiscFee.Text);
      this.loanTrade.RateAdjustment = Utils.ParseDecimal((object) this.txtRateAdjustment.Text);
      this.loanTrade.BuyUpAmount = Utils.ParseDecimal((object) this.txtBuyUp.Text);
      this.loanTrade.BuyDownAmount = Utils.ParseDecimal((object) this.txtBuyDown.Text);
      this.loanTrade.GainLossAmount = Utils.ParseDecimal((object) this.txtGainLoss.Text);
      this.loanTrade.NetProfit = Utils.ParseDecimal((object) this.txtNetProfit.Text);
      this.loanTrade.Notes = this.txtNotes.Text;
      this.loanTrade.Servicer = this.txtServicer.Text;
      this.loanTrade.IsWeightedAvgBulkPriceLocked = Utils.ParseBoolean((object) this.lbtn_WABP.Locked);
      this.loanTrade.IsBulkDelivery = Utils.ParseBoolean((object) this.checkbox_bulkDelivery.Checked);
      this.loanTrade.WeightedAvgBulkPrice = Utils.ParseDecimal((object) this.txt_WABP.Text);
      switch (this.cmbServicingType.SelectedIndex)
      {
        case 0:
          this.loanTrade.ServicingType = ServicingType.ServicingReleased;
          break;
        case 1:
          this.loanTrade.ServicingType = ServicingType.ServicingRetained;
          break;
      }
      if (this.cboContract.SelectedIndex <= 0)
      {
        this.loanTrade.ContractID = -1;
        this.loanTrade.ContractNumber = "";
      }
      else
      {
        MasterContractSummaryInfo selectedItem = (MasterContractSummaryInfo) this.cboContract.SelectedItem;
        this.loanTrade.ContractID = selectedItem.ContractID;
        this.loanTrade.ContractNumber = selectedItem.ContractNumber;
      }
      this.ctlContactInfo.CommitChanges();
      this.ctlPricing.CommitChanges();
      this.ctlAdvancedPricing.CommitChanges();
      this.ctlAdjustments.CommitChanges();
      this.ctlSRP.CommitChanges();
      this.msrPricingEditor1.CommitChanges();
      this.loanTrade.CommitmentDate = this.dtCommitment.Value;
      this.loanTrade.InvestorDeliveryDate = this.dtInvestorDelivery.Value;
      this.loanTrade.EarlyDeliveryDate = this.dtEarlyDelivery.Value;
      this.loanTrade.TargetDeliveryDate = this.dtTargetDelivery.Value;
      this.loanTrade.ShipmentDate = this.dtActualDelivery.Value;
      this.loanTrade.PurchaseDate = this.dtPurchase.Value;
      if (!this.securityTradeInfo.ReadOnly)
        this.securityTradeInfo.CommitChanges();
      this.saveSearch();
      this.ctlLoanList.DataModified = false;
    }

    private void saveSearch()
    {
      this.loanTrade.Filter = this.getCurrentFilter();
      this.ctlAdvancedSearch.DataModified = false;
      this.ctlSimpleSearch.DataModified = false;
    }

    private TradeFilter getCurrentFilter()
    {
      return this.cboSearchType.SelectedIndex == 0 ? new TradeFilter(this.ctlSimpleSearch.GetCurrentFilter(), this.ctlLoanList.GetCurrentLayout()) : new TradeFilter(this.ctlAdvancedSearch.GetCurrentFilter(), this.ctlLoanList.GetCurrentLayout());
    }

    private void btnSave_Click(object sender, EventArgs e) => this.SaveTrade();

    public bool SaveTrade() => this.SaveTrade(false, 0, false);

    public bool SaveTrade(int sqlRead) => this.SaveTrade(false, sqlRead, false);

    public bool SaveTrade(bool forceUpdateOfLoans, bool updatedSelectedLoans)
    {
      return this.SaveTrade(forceUpdateOfLoans, 0, updatedSelectedLoans);
    }

    public bool SaveTrade(bool forceUpdateOfLoans, int sqlRead, bool updatedSelectedLoans)
    {
      if (this.readOnly)
        return true;
      Cursor.Current = Cursors.WaitCursor;
      try
      {
        if (!this.prevalidateCommit() || !this.validateBulkDelivery())
          return false;
        this.commitChanges();
        if (!this.validateTradeData())
          return false;
        this.saveTradeInfo();
        bool flag1 = true;
        bool flag2 = this.assignments.HasPendingChanges() || this.requiresLoanUpdates();
        if (forceUpdateOfLoans)
          flag1 = this.commitTradeAssignments(true, updatedSelectedLoans);
        else if (flag2 && Utils.Dialog((IWin32Window) this, "The trade has been saved successfully." + Environment.NewLine + "Would you like to update the loan files with these recent changes?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
          flag1 = this.commitTradeAssignments(false, updatedSelectedLoans);
        this.loadTradeData(sqlRead);
        return flag1;
      }
      catch (ObjectNotFoundException ex)
      {
        Tracing.Log(TradeEditor.sw, this.className, TraceLevel.Error, ex.ToString());
        if (ex.ObjectType == ObjectType.Trade)
        {
          int num1 = (int) Utils.Dialog((IWin32Window) this, "The current trade has been deleted and cannot be saved. All changes made to this trade will be lost.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "The trade could not be saved due to the following error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        this.ReadOnly = true;
        this.modified = false;
        this.ctlLoanList.DataModified = false;
        this.loanUpdatesRequired = false;
        return false;
      }
      catch (TradeNotUpdateException ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      catch (Exception ex)
      {
        Tracing.Log(TradeEditor.sw, this.className, TraceLevel.Error, ex.ToString());
        if (ex.Message.Contains("The loan has been assigned to another trade or pool."))
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "The trade is saved successfully. However, the loan is not assigned to the trade, because the loan has been assigned to another trade or pool.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          this.loadTradeData(sqlRead);
        }
        else
        {
          int num3 = (int) Utils.Dialog((IWin32Window) this, "The trade could not be saved due to the following error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        return false;
      }
      finally
      {
        Cursor.Current = Cursors.Default;
      }
    }

    private bool prevalidateCommit()
    {
      if (this.ctlSRP.ItemModified)
      {
        this.tabTrade.SelectedTab = this.tpPricing;
        if (!this.ctlSRP.ValidateChanges())
          return false;
      }
      return this.securityTradeInfo.ValidateTradeData();
    }

    private bool validateBulkDelivery()
    {
      if (this.loanTrade.TradeID == -1 || this.isBulkDelivery || !this.checkbox_bulkDelivery.Checked)
        return true;
      switch (Utils.Dialog((IWin32Window) this, "Changing to the Bulk Delivery Type removes the Pricing tab and any of its previously entered parameters and any previously allocated loans may need to be re-priced. Do you wish to continue?", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation))
      {
        case DialogResult.OK:
          this.ClearPricingTab();
          this.loanUpdatesRequired = true;
          break;
        case DialogResult.Cancel:
          this.checkbox_bulkDelivery.Checked = false;
          return false;
      }
      return true;
    }

    private void ClearPricingTab()
    {
      this.loanTrade.Pricing.SimplePricingItems.Clear();
      this.loanTrade.PriceAdjustments.Clear();
      this.loanTrade.SRPTable.PricingItems.Clear();
      this.ctlPricing.PricingItems = this.loanTrade.Pricing.SimplePricingItems;
      this.ctlAdjustments.Adjustments = this.loanTrade.PriceAdjustments;
      this.ctlSRP.SRPTable = this.loanTrade.SRPTable;
      if (this.tabTrade.TabPages["tpPricing"] == null)
        return;
      this.pricingTab = this.tabTrade.TabPages["tpPricing"];
      this.tabTrade.TabPages.Remove(this.tabTrade.TabPages["tpPricing"]);
    }

    private bool requiresLoanUpdates()
    {
      return this.LoanUpdatesRequired && this.assignments.GetAssignedLoans().Length != 0;
    }

    private void saveTradeInfo()
    {
      int num1 = this.loanTrade.TradeID;
      if (num1 < 0)
      {
        num1 = Session.LoanTradeManager.CreateTrade(this.loanTrade);
      }
      else
      {
        try
        {
          Session.LoanTradeManager.UpdateTrade(this.loanTrade, true);
        }
        catch (TradeNotUpdateException ex)
        {
          throw;
        }
      }
      try
      {
        bool flag = false;
        if (num1 > 0 && this.secTradeObj != null)
        {
          if (this.loanTrade.SecurityTradeID != this.secTradeObj.TradeID)
            Session.SecurityTradeManager.AssignLoanTradeToTrade(this.secTradeObj.TradeID, num1);
          flag = true;
        }
        this.loanTrade = Session.LoanTradeManager.GetTrade(num1);
        this.assignments.ApplyNewTradeID(num1);
        if (this.securityTradeInfo.DataModified && this.securityTradeInfo.SaveTrade(this.loanTrade))
        {
          this.secTradeObj = this.securityTradeInfo.SecurityTrade;
          flag = true;
        }
        if (flag)
          Session.SecurityTradeManager.UpdateTradeAfterAssignLoanTrade(this.secTradeObj);
      }
      catch (Exception ex)
      {
        int num2 = ex.Message.Contains("The loan trade has been allocated to security trade") ? (int) Utils.Dialog((IWin32Window) this, "The loan trade is saved successfully. However, the loan trade is not assigned to the security trade you entered, because the loan trade has been assigned to another security trade " + ex.Message.Substring(ex.Message.IndexOf("\"")), MessageBoxButtons.OK, MessageBoxIcon.Exclamation) : throw ex;
      }
      this.resetOriginalTradeData();
      this.SetLoansTotalPrice();
      this.assignments.WritePendingChangesToServer(this.RemovePendingLoanFromOtherTrades);
      this.assignments.UpdateTotalPriceForAllLoans = false;
    }

    private void SetLoansTotalPrice()
    {
      if (!this.IsBulkPricing() || this.IsCalculatedWABP())
        return;
      this.assignments.UpdateTotalPriceForAllLoans = true;
      foreach (LoanTradeAssignment assignedPendingLoan in this.GetAllAssignedPendingLoans())
        assignedPendingLoan.TotalPrice = Utils.ParseDecimal((object) this.txt_WABP.Text.Trim());
    }

    private bool commitTradeAssignments(bool forceUpdateOfAllLoans, bool selectedLoans)
    {
      if (TradeProcesses2.IsTradeProcesses2Enabled())
      {
        try
        {
          TradeAssignmentManager assignments = this.assignments;
          if (selectedLoans)
          {
            List<string> guids = ((IEnumerable<PipelineInfo>) this.ctlLoanList.GetSelectedAndPendingLoans()).Select<PipelineInfo, string>((Func<PipelineInfo, string>) (t => t.GUID)).ToList<string>();
            assignments.loans = ((IEnumerable<LoanTradeAssignment>) assignments.GetAllAssignedPendingLoans()).Where<LoanTradeAssignment>((Func<LoanTradeAssignment, bool>) (t => guids.Contains(t.Guid))).ToDictionary<LoanTradeAssignment, string, LoanTradeAssignment>((Func<LoanTradeAssignment, string>) (t => t.Guid), (Func<LoanTradeAssignment, LoanTradeAssignment>) (t => t));
            forceUpdateOfAllLoans = true;
          }
          new TradeProcesses2((ITradeEditor) this).Execute(TradeProcesses2.ActionType.Commit, this.loanTrade, assignments, forceUpdateOfAllLoans || this.requiresLoanUpdates(), this.secTradeObj == null ? 0M : this.secTradeObj.Price);
          return true;
        }
        catch (TradeNotUpdateException ex)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return false;
        }
        catch (Exception ex)
        {
          Tracing.Log(TradeEditor.sw, this.className, TraceLevel.Error, "Error applying loan status: " + (object) ex);
          int num = (int) Utils.Dialog((IWin32Window) this, "Unexpected error while attempting to update loans: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return false;
        }
      }
      else
      {
        using (ProgressDialog progressDialog = new ProgressDialog("Trade Management", new AsynchronousProcess(this.commitTradeAssignmentsAsync), (object) (bool) (forceUpdateOfAllLoans ? 1 : (this.requiresLoanUpdates() ? 1 : 0)), true))
        {
          int num = (int) progressDialog.ShowDialog((IWin32Window) this);
          if (num == 1)
            this.loanUpdatesRequired = false;
          return num == 1;
        }
      }
    }

    private DialogResult commitTradeAssignmentsAsync(
      object forceUpdateOfAllLoans,
      IProgressFeedback feedback)
    {
      try
      {
        feedback.Status = "Preparing to update loans...";
        TradeProcesses tradeProcesses = new TradeProcesses();
        DialogResult dialogResult = tradeProcesses.CommitTradeAssignments(this.assignments, this.loanTrade, (bool) forceUpdateOfAllLoans, feedback, this.secTradeObj == null ? 0M : this.secTradeObj.Price);
        if (dialogResult != DialogResult.OK)
          return dialogResult;
        if (tradeProcesses.SuccessCount == 1)
        {
          int num1 = (int) Utils.Dialog((IWin32Window) this, "1 loan has been successfully updated.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        else if (tradeProcesses.SuccessCount > 0)
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, tradeProcesses.SuccessCount.ToString() + " loans have been successfully updated.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        return DialogResult.OK;
      }
      catch (Exception ex)
      {
        Tracing.Log(TradeEditor.sw, this.className, TraceLevel.Error, "Error applying loan status: " + (object) ex);
        int num = (int) Utils.Dialog((IWin32Window) this, "Unexpected error while attempting to update loans: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return DialogResult.Abort;
      }
    }

    private bool validateTradeData()
    {
      string str = this.txtName.Text.Trim();
      if (str.Length == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must enter a name/number for this trade before saving.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return false;
      }
      if (this.txt_WABP.Enabled && this.txt_WABP.Text.Trim() == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Weighted Average Bulk Price field cannot be blank.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return false;
      }
      if (string.Compare(str, this.originalTradeName, true) != 0 && Session.LoanTradeManager.GetTradeByName(str) != null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "A trade with the name/number '" + str + "' already exists. You must enter a unique name for this trade.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return false;
      }
      string errMsg;
      if (this.areIneligibleLoansAssigned(out errMsg) && errMsg.Trim() != string.Empty)
      {
        if (errMsg.Contains("withdrawn"))
        {
          int num = (int) Utils.Dialog((IWin32Window) this, errMsg, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return false;
        }
        if (Utils.Dialog((IWin32Window) this, errMsg, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
          return false;
      }
      return this.loanTrade.Pricing.SimplePricingItems.Count <= 0 || !this.areUnpricedLoansAssigned() || Utils.Dialog((IWin32Window) this, "The trade has one or more loans assigned to it for which pricing cannot be determined based on your current pricing setup. Do you want to continue to save this trade?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes;
    }

    private bool areIneligibleLoansAssigned(out string errMsg)
    {
      string errMsg1 = string.Empty;
      LoanTradeFilterEvaluator evaluator = (LoanTradeFilterEvaluator) this.getCurrentFilter().CreateEvaluator(typeof (LoanTradeFilterEvaluator));
      evaluator.MilestoneSettings = Session.StartupInfo.Milestones;
      foreach (PipelineInfo pinfo in this.assignments.GetAssignedPipelineData())
      {
        if (!evaluator.Evaluate(pinfo, FilterEvaluationOption.NonVolatile, out errMsg1))
        {
          errMsg = errMsg1;
          return true;
        }
      }
      errMsg = errMsg1;
      return false;
    }

    private bool areUnpricedLoansAssigned()
    {
      foreach (PipelineInfo info in this.assignments.GetAssignedPipelineData())
      {
        if (!this.loanTrade.IsNoteRateAllowed(info))
          return true;
      }
      return false;
    }

    private void cboContract_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.loading)
        return;
      this.onLoanUpdatableFieldValueChanged(sender, e);
      if (this.cboContract.SelectedIndex <= 0 || Utils.Dialog((IWin32Window) this, "Apply the selected contract's investor data to the current trade?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      this.commitChanges();
      MasterContractInfo contract = Session.MasterContractManager.GetContract(((MasterContractSummaryInfo) this.cboContract.SelectedItem).ContractID);
      if (contract == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The specified contract cannot be found. It may have been deleted by another user.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        this.txtInvestorCommitNum.Text = contract.InvestorContractNumber;
        this.applyInvestor(contract.Investor);
      }
    }

    private void onFieldValueChanged(object sender, EventArgs e)
    {
      Decimal num1 = 0M;
      try
      {
        num1 = Utils.ParseDecimal((object) this.txtAmount.Text);
      }
      catch
      {
      }
      Decimal num2 = 0M;
      try
      {
        num2 = Utils.ParseDecimal((object) this.txtTolerance.Text);
      }
      catch
      {
      }
      this.txtMinAmt.Text = (num1 - num1 * num2 / 100M).ToString("#,##0;;\"\"");
      this.txtMaxAmt.Text = (num1 + num1 * num2 / 100M).ToString("#,##0;;\"\"");
      this.modified = true;
    }

    private void onLoanUpdatableFieldValueChanged(object sender, EventArgs e)
    {
      this.modified = true;
      this.loanUpdatesRequired = true;
    }

    private void tabTrade_SelectedIndexChanged(object sender, EventArgs e)
    {
      Cursor.Current = Cursors.WaitCursor;
      if (this.tabTrade.SelectedTab == this.tpDetails)
      {
        this.recalculateProfitability();
        this.RefreshWeightAvgBulkPrice();
      }
      else if (this.tabTrade.SelectedTab == this.tpLoans)
      {
        this.refreshLoanLists();
        this.ctlLoanList.DisplayWithdrawnLoanMessage();
      }
      else
        this.commitChanges();
      Cursor.Current = Cursors.Default;
    }

    private void refreshLoanLists()
    {
      this.commitChanges();
      bool refreshLoans = false;
      if (this.lastPricingTradeInfo == null || !LoanTradeInfo.ComparePricing(this.lastPricingTradeInfo, this.loanTrade))
        refreshLoans = true;
      else if (!TradeFilter.CompareFilters(this.lastEvaluatedFilter, this.loanTrade.Filter))
        refreshLoans = true;
      else if (string.Compare(this.lastPricingTradeInfo.InvestorName, this.loanTrade.InvestorName, true) != 0)
        refreshLoans = true;
      this.ctlLoanList.RefreshLoanList(refreshLoans);
      this.lastEvaluatedFilter = this.loanTrade.Filter;
      this.lastPricingTradeInfo = new LoanTradeInfo(this.loanTrade);
    }

    private void recalculateProfitability()
    {
      this.commitChanges();
      this.loadProfitabilityData();
    }

    private void txtMiscFee_Validated(object sender, EventArgs e)
    {
      this.recalculateProfitability();
    }

    private void txtPairOffFee_Validated(object sender, EventArgs e)
    {
      this.recalculateProfitability();
    }

    private void btnDateStamp_Click(object sender, EventArgs e)
    {
      string str = Utils.CreateTimestamp(true) + " " + Session.UserInfo.FullName + "> ";
      if (this.txtNotes.Text.Length != 0)
        this.txtNotes.Text += Environment.NewLine;
      this.txtNotes.Text += str;
      this.txtNotes.SelectionStart = this.txtNotes.Text.Length;
      this.txtNotes.Focus();
    }

    private void tpHistory_Resize(object sender, EventArgs e)
    {
      int num = Math.Max(0, this.pnlHistory.Width - 5);
      this.grpNotes.Top = this.grpHistory.Top = 0;
      this.grpNotes.Height = this.grpHistory.Height = this.pnlHistory.Height;
      this.grpNotes.Left = 0;
      this.grpNotes.Width = num / 2;
      this.grpHistory.Width = Math.Max(0, num - this.grpNotes.Width);
      this.grpHistory.Left = this.grpNotes.Right + 5;
    }

    private void btnViewFilter_Click(object sender, EventArgs e)
    {
      string text = AdvancedSearchControl.GetFilterSummary(this.loanTrade.Filter.FilterType != TradeFilterType.Advanced ? this.loanTrade.Filter.GetSimpleFilter().ConvertToFilterList() : this.loanTrade.Filter.GetAdvancedFilter());
      if (text == "")
        text = "No criteria has been defined for the search filter.";
      int num = (int) Utils.Dialog((IWin32Window) this, text, MessageBoxButtons.OK, MessageBoxIcon.None);
    }

    private void tpLoans_Resize(object sender, EventArgs e)
    {
      this.ctlLoanList.AdjustLoanPanelDisplay();
    }

    private void btnAdjustmentTemplate_Click(object sender, EventArgs e)
    {
      using (PriceAdjustmentTemplateSelector templateSelector = new PriceAdjustmentTemplateSelector())
      {
        if (templateSelector.ShowDialog((IWin32Window) this) == DialogResult.Cancel)
          return;
        PriceAdjustmentTemplate selectedTemplate = (PriceAdjustmentTemplate) templateSelector.GetSelectedTemplate();
        if (!templateSelector.AppendTemplate)
          this.ctlAdjustments.ClearAdjustments();
        this.ctlAdjustments.AddAdjustments(selectedTemplate.PriceAdjustments);
      }
    }

    private void btnSRPTemplate_Click(object sender, EventArgs e)
    {
      using (SRPTemplateSelector templateSelector = new SRPTemplateSelector())
      {
        if (templateSelector.ShowDialog((IWin32Window) this) == DialogResult.Cancel)
          return;
        this.ctlSRP.ApplySRPTableData(((SRPTableTemplate) templateSelector.GetSelectedTemplate()).SRPTable);
      }
    }

    private void btnInvestorTemplate_Click(object sender, EventArgs e)
    {
      using (InvestorTemplateSelector templateSelector = new InvestorTemplateSelector(true))
      {
        if (templateSelector.ShowDialog((IWin32Window) this) == DialogResult.Cancel)
          return;
        this.applyInvestor(templateSelector.SelectedTemplate.CompanyInformation);
      }
    }

    private void applyInvestor(Investor investor)
    {
      if (investor != null)
        this.loanTrade.Investor.CopyFrom(investor);
      else
        this.loanTrade.Investor.Clear();
      this.loadInvestorData();
      this.recalculateProfitability();
      if (investor == null)
        this.dtInvestorDelivery.Value = DateTime.MinValue;
      else if (this.dtCommitment.Value != DateTime.MinValue && this.dtInvestorDelivery.Value == DateTime.MinValue && investor.DeliveryTimeFrame > 0)
        this.dtInvestorDelivery.Value = this.dtCommitment.Value.AddDays((double) investor.DeliveryTimeFrame);
      this.modified = true;
      this.loanUpdatesRequired = true;
    }

    private void btnSavedSearches_Click(object sender, EventArgs e)
    {
      using (TradeFilterTemplateSelector templateSelector = new TradeFilterTemplateSelector())
      {
        if (templateSelector.ShowDialog((IWin32Window) this) == DialogResult.Cancel)
          return;
        this.loanTrade.Filter = ((TradeFilterTemplate) templateSelector.GetSelectedTemplate()).Filter;
        this.modified = true;
        this.loadSearch();
      }
    }

    private void tpDetails_Resize(object sender, EventArgs e)
    {
      int height = this.pnlDetails.Height;
      this.pnlFilter.Top = 0;
    }

    private void tpPricing_Resize(object sender, EventArgs e)
    {
      this.ctlPricing.Visible = false;
      this.grpBuyUpDown.Visible = false;
      this.ctlAdvancedPricing.Visible = false;
      this.msrPricingEditor1.Visible = false;
      if (this.cmbServicingType.Text == "Service Retained" && this.cmbPricingType.Text == "Simple")
      {
        int num = Math.Max(0, this.pnlPricing.Height - 5);
        this.grpBuyUpDown.Visible = false;
        this.ctlSRP.Visible = false;
        this.msrPricingEditor1.Visible = true;
        this.ctlPricing.Visible = true;
        this.ctlPricing.Top = 0;
        this.ctlPricing.Left = this.ctlAdjustments.Left = 0;
        this.ctlPricing.Height = this.pnlPricing.Height / 2;
        this.ctlAdjustments.Top = this.ctlPricing.Bottom + 5;
        this.ctlAdjustments.Height = Math.Max(0, num - this.ctlPricing.Height);
        this.ctlAdjustments.Width = this.pnlPricing.Width / 2;
        this.ctlPricing.Width = this.ctlAdjustments.Width / 2;
        this.msrPricingEditor1.Top = 0;
        this.msrPricingEditor1.Left = this.ctlPricing.Width + 5;
        this.msrPricingEditor1.Width = this.ctlAdjustments.Width / 2 - 5;
        this.msrPricingEditor1.Height = this.ctlPricing.Height;
      }
      else if (this.cmbServicingType.Text == "Service Retained" && this.cmbPricingType.Text == "Advanced")
      {
        int num = Math.Max(0, this.pnlPricing.Height - 5);
        this.grpBuyUpDown.Visible = false;
        this.ctlSRP.Visible = false;
        this.msrPricingEditor1.Visible = true;
        this.ctlPricing.Visible = false;
        this.ctlAdvancedPricing.Visible = true;
        this.ctlAdvancedPricing.Top = 0;
        this.ctlAdvancedPricing.Left = this.ctlAdjustments.Left = 0;
        this.ctlAdvancedPricing.Height = this.pnlPricing.Height / 2;
        this.ctlAdjustments.Top = this.ctlAdvancedPricing.Bottom + 5;
        this.ctlAdjustments.Height = Math.Max(0, num - this.ctlAdvancedPricing.Height);
        this.ctlAdjustments.Width = this.pnlPricing.Width / 2;
        this.ctlAdvancedPricing.Width = this.ctlAdjustments.Width;
        this.msrPricingEditor1.Top = 0;
        this.msrPricingEditor1.Left = this.ctlAdvancedPricing.Width + 5;
        this.msrPricingEditor1.Width = this.ctlAdjustments.Width / 2 - 5;
        this.msrPricingEditor1.Height = this.ctlAdvancedPricing.Height;
      }
      else
      {
        this.ctlSRP.Visible = true;
        if (this.cmbPricingType.Text == "Simple")
        {
          this.ctlPricing.Visible = true;
          this.grpBuyUpDown.Visible = true;
          this.ctlPricing.Top = this.grpBuyUpDown.Top = this.ctlSRP.Top = 0;
          this.ctlPricing.Left = this.ctlAdjustments.Left = 0;
          this.ctlSRP.Height = this.pnlPricing.Height;
          int num1 = Math.Max(0, this.pnlPricing.Height - 5);
          TradePricingEditor ctlPricing = this.ctlPricing;
          GroupContainer grpBuyUpDown = this.grpBuyUpDown;
          int val1 = num1 / 2;
          int height = this.grpBuyUpDown.MinimumSize.Height;
          int num2;
          int num3 = num2 = Math.Max(val1, height);
          grpBuyUpDown.Height = num2;
          int num4 = num3;
          ctlPricing.Height = num4;
          this.ctlAdjustments.Height = Math.Max(0, num1 - this.ctlPricing.Height);
          this.ctlAdjustments.Top = this.ctlPricing.Bottom + 5;
          int num5 = Math.Max(0, this.pnlPricing.Width - 5);
          int num6 = Math.Max(0, (num5 - 5) / 2);
          this.grpBuyUpDown.Width = Math.Max(num6 / 2, this.grpBuyUpDown.MinimumSize.Width);
          this.ctlPricing.Width = Math.Max(0, num6 - this.grpBuyUpDown.Width);
          this.grpBuyUpDown.Left = this.ctlPricing.Right + 5;
          this.ctlAdjustments.Width = this.grpBuyUpDown.Width + this.ctlPricing.Width + 5;
          this.ctlSRP.Width = Math.Max(0, num5 - this.ctlAdjustments.Width);
          this.ctlSRP.Left = this.ctlAdjustments.Right + 5;
        }
        else
        {
          this.ctlAdvancedPricing.Visible = true;
          this.ctlAdvancedPricing.Top = this.grpBuyUpDown.Top = this.ctlSRP.Top = 0;
          this.ctlAdvancedPricing.Left = this.ctlAdjustments.Left = 0;
          this.ctlSRP.Height = this.pnlPricing.Height;
          int num7 = Math.Max(0, this.pnlPricing.Height - 5);
          TradeAdvancedPricingEditor ctlAdvancedPricing = this.ctlAdvancedPricing;
          GroupContainer grpBuyUpDown1 = this.grpBuyUpDown;
          int val1_1 = num7 / 2;
          Size minimumSize = this.grpBuyUpDown.MinimumSize;
          int height = minimumSize.Height;
          int num8;
          int num9 = num8 = Math.Max(val1_1, height);
          grpBuyUpDown1.Height = num8;
          int num10 = num9;
          ctlAdvancedPricing.Height = num10;
          this.ctlAdjustments.Height = Math.Max(0, num7 - this.ctlAdvancedPricing.Height);
          this.ctlAdjustments.Top = this.ctlAdvancedPricing.Bottom + 5;
          int num11 = Math.Max(0, this.pnlPricing.Width);
          int val2 = Math.Max(0, (num11 - 5) / 2);
          GroupContainer grpBuyUpDown2 = this.grpBuyUpDown;
          int val1_2 = val2 / 2;
          minimumSize = this.grpBuyUpDown.MinimumSize;
          int width = minimumSize.Width;
          int num12 = Math.Max(val1_2, width);
          grpBuyUpDown2.Width = num12;
          this.ctlAdvancedPricing.Width = Math.Max(0, val2);
          this.grpBuyUpDown.Left = this.ctlPricing.Right + 5;
          this.ctlAdjustments.Width = this.ctlAdvancedPricing.Width + 5;
          this.ctlSRP.Width = Math.Max(0, num11 - this.ctlAdjustments.Width);
          this.ctlSRP.Left = this.ctlAdjustments.Right + 5;
          this.ctlAdvancedPricing.Refresh();
          this.ctlAdvancedPricing.Validate();
        }
      }
    }

    private void onCommitmentDateChanged(object sender, EventArgs e)
    {
      if (this.dtCommitment.Value != DateTime.MinValue && this.loanTrade.Investor.DeliveryTimeFrame > 0)
        this.dtInvestorDelivery.Value = this.dtCommitment.Value.AddDays((double) this.loanTrade.Investor.DeliveryTimeFrame);
      this.onLoanUpdatableFieldValueChanged(sender, e);
    }

    private void btnExportHistory_Click(object sender, EventArgs e)
    {
      try
      {
        ExcelHandler excelHandler = new ExcelHandler();
        excelHandler.AddDataTable(this.gvHistory, false);
        excelHandler.CreateExcel();
      }
      catch (Exception ex)
      {
        Tracing.Log(TradeEditor.sw, this.className, TraceLevel.Error, "Error during export: " + (object) ex);
        int num = (int) Utils.Dialog((IWin32Window) this, "An error occurred while attempting to export the loans to Microsoft Excel. Ensure that you have Excel installed and it is working properly.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void btnList_Click(object sender, EventArgs e)
    {
      TradeManagementConsole.Instance.CloseTrade();
    }

    private DialogResult updateLoanTradeDataAsync(object stateNotUsed, IProgressFeedback feedback)
    {
      try
      {
        feedback.Status = "Preparing to update loans...";
        TradeProcesses tradeProcesses = new TradeProcesses();
        DialogResult dialogResult = tradeProcesses.RefreshTradeDataInLoans(this.assignments, this.loanTrade, feedback, this.secTradeObj == null ? 0M : this.secTradeObj.Price);
        if (dialogResult != DialogResult.OK)
          return dialogResult;
        this.loanUpdatesRequired = false;
        if (tradeProcesses.SuccessCount == 1)
        {
          int num1 = (int) Utils.Dialog((IWin32Window) this, "1 loan has been successfully updated.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        else if (tradeProcesses.SuccessCount > 0)
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, tradeProcesses.SuccessCount.ToString() + " loans have been successfully updated.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        return DialogResult.OK;
      }
      catch (Exception ex)
      {
        Tracing.Log(TradeEditor.sw, this.className, TraceLevel.Error, "Error updating loans: " + (object) ex);
        int num = (int) Utils.Dialog((IWin32Window) this, "Unexpected error while attempting to update loans: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return DialogResult.Abort;
      }
    }

    private TableLayout.Column[] getFixedEligibleLoanColumns()
    {
      return new List<TableLayout.Column>()
      {
        new TableLayout.Column("TradeAssignment.Status", "Status", HorizontalAlignment.Left, 103),
        new TableLayout.Column("Trade.Name", "Trade ID", HorizontalAlignment.Left, 103),
        new TableLayout.Column("Loan.LoanNumber", "Loan Number", HorizontalAlignment.Left, 103),
        new TableLayout.Column("Loan.TotalBuyPrice", "Loan Trade Total Buy Price", HorizontalAlignment.Right, 94),
        new TableLayout.Column("Loan.TotalSellPrice", "Loan Trade Total Sell Price", HorizontalAlignment.Right, 94),
        new TableLayout.Column("Loan.NetProfit", "Gain/Loss", HorizontalAlignment.Right, 94)
      }.ToArray();
    }

    private TableLayout getDefaultEligibleTableLayout()
    {
      TableLayout eligibleTableLayout = new TableLayout();
      foreach (TableLayout.Column eligibleLoanColumn in this.getFixedEligibleLoanColumns())
        eligibleTableLayout.AddColumn(eligibleLoanColumn);
      eligibleTableLayout.AddColumn(new TableLayout.Column("Loan.LoanProgram", "Loan Program", HorizontalAlignment.Left, 94));
      eligibleTableLayout.AddColumn(new TableLayout.Column("Loan.CurrentMilestoneName", "Last Finished Milestone", HorizontalAlignment.Left, 103));
      eligibleTableLayout.AddColumn(new TableLayout.Column("Loan.TotalLoanAmount", "Loan Amount", HorizontalAlignment.Right, 103));
      eligibleTableLayout.AddColumn(new TableLayout.Column("Loan.LoanRate", "Note Rate", HorizontalAlignment.Right, 68));
      eligibleTableLayout.AddColumn(new TableLayout.Column("Loan.Term", "Term", HorizontalAlignment.Right, 56));
      eligibleTableLayout.AddColumn(new TableLayout.Column("Loan.LTV", "LTV", HorizontalAlignment.Right, 52));
      eligibleTableLayout.AddColumn(new TableLayout.Column("Loan.CLTV", "CLTV", "Combined LTV", HorizontalAlignment.Right, 52));
      eligibleTableLayout.AddColumn(new TableLayout.Column("Loan.DTITop", "Top", "Top Ratio", HorizontalAlignment.Right, 50));
      eligibleTableLayout.AddColumn(new TableLayout.Column("Loan.DTIBottom", "Bottom", "Bottom Ratio", HorizontalAlignment.Right, 50));
      eligibleTableLayout.AddColumn(new TableLayout.Column("Loan.CreditScore", "FICO", HorizontalAlignment.Right, 56));
      eligibleTableLayout.AddColumn(new TableLayout.Column("Loan.OccupancyStatus", "Occupancy Type", HorizontalAlignment.Left, 101));
      eligibleTableLayout.AddColumn(new TableLayout.Column("Loan.PropertyType", "Property Type", HorizontalAlignment.Left, 82));
      eligibleTableLayout.AddColumn(new TableLayout.Column("Loan.State", "State", HorizontalAlignment.Left, 56));
      eligibleTableLayout.AddColumn(new TableLayout.Column("Loan.LockExpirationDate", "Lock Expiration Date", HorizontalAlignment.Left, 124));
      eligibleTableLayout.AddColumn(new TableLayout.Column("Loan.BorrowerLastName", "Last Name", HorizontalAlignment.Left, 104));
      return eligibleTableLayout;
    }

    public void MenuClicked(ToolStripItem menuItem)
    {
      switch (string.Concat(menuItem.Tag))
      {
        case "TE_SaveTrade":
          this.SaveTrade();
          break;
        case "TE_ExitTrade":
          TradeManagementConsole.Instance.CloseTrade();
          break;
      }
    }

    public bool SetMenuItemState(ToolStripItem menuItem)
    {
      Control stateControl = (Control) null;
      switch (string.Concat(menuItem.Tag))
      {
        case "TE_SaveTrade":
          stateControl = (Control) this.btnSave;
          break;
        case "TR_ExitTrade":
          stateControl = (Control) this.btnList;
          break;
      }
      if (stateControl == null)
        return true;
      ClientCommonUtils.ApplyControlStateToMenu(menuItem, stateControl);
      return stateControl.Visible;
    }

    private TradeView getDefaultTradeAssignedLoansView()
    {
      TradeView assignedLoansView = new TradeView(this.standardViewName);
      TableLayout tableLayout = new TableLayout();
      tableLayout.AddColumn(new TableLayout.Column("TradeAssignment.Status", "Trade Assignment Status", HorizontalAlignment.Left, 103));
      tableLayout.AddColumn(new TableLayout.Column("TradeAssignment.StatusDate", "Date", HorizontalAlignment.Left, 103));
      tableLayout.AddColumn(new TableLayout.Column("Loan.LoanNumber", "Loan Number", HorizontalAlignment.Left, 103));
      tableLayout.AddColumn(new TableLayout.Column("Loan.TotalBuyPrice", "Loan Trade Total Buy Price", HorizontalAlignment.Right, 94));
      tableLayout.AddColumn(new TableLayout.Column("Loan.TotalSellPrice", "Loan Trade Total Sell Price", HorizontalAlignment.Right, 94));
      tableLayout.AddColumn(new TableLayout.Column("Loan.NetProfit", "Gain/Loss", HorizontalAlignment.Right, 94));
      tableLayout.AddColumn(new TableLayout.Column("Loan.LoanProgram", "Loan Program", HorizontalAlignment.Left, 94));
      tableLayout.AddColumn(new TableLayout.Column("Loan.CurrentMilestoneName", "Last Finished Milestone", HorizontalAlignment.Left, 103));
      tableLayout.AddColumn(new TableLayout.Column("Loan.TotalLoanAmount", "Loan Amount", HorizontalAlignment.Right, 103));
      tableLayout.AddColumn(new TableLayout.Column("Loan.LoanRate", "Note Rate", HorizontalAlignment.Right, 68));
      tableLayout.AddColumn(new TableLayout.Column("Loan.Term", "Term", HorizontalAlignment.Right, 56));
      tableLayout.AddColumn(new TableLayout.Column("Loan.LTV", "LTV", HorizontalAlignment.Right, 52));
      tableLayout.AddColumn(new TableLayout.Column("Loan.CLTV", "CLTV", HorizontalAlignment.Right, 52));
      tableLayout.AddColumn(new TableLayout.Column("Loan.DTITop", "Top", "Top Ratio", HorizontalAlignment.Right, 50));
      tableLayout.AddColumn(new TableLayout.Column("Loan.DTIBottom", "Bottom", "Bottom Ratio", HorizontalAlignment.Right, 50));
      tableLayout.AddColumn(new TableLayout.Column("Loan.CreditScore", "FICO", HorizontalAlignment.Right, 56));
      tableLayout.AddColumn(new TableLayout.Column("Loan.OccupancyStatus", "Occupancy Type", HorizontalAlignment.Left, 101));
      tableLayout.AddColumn(new TableLayout.Column("Loan.PropertyType", "Property Type", HorizontalAlignment.Left, 82));
      tableLayout.AddColumn(new TableLayout.Column("Loan.State", "State", HorizontalAlignment.Left, 56));
      tableLayout.AddColumn(new TableLayout.Column("Loan.LockExpirationDate", "Lock Expiration Date", HorizontalAlignment.Left, 124));
      tableLayout.AddColumn(new TableLayout.Column("Loan.BorrowerLastName", "Last Name", HorizontalAlignment.Left, 104));
      assignedLoansView.Layout = tableLayout;
      return assignedLoansView;
    }

    private TableLayout getFullTableLayout()
    {
      TableLayout fullTableLayout = new TableLayout();
      foreach (ReportFieldDef tradeAssLoanFieldDef in (ReportFieldDefContainer) this.tradeAssLoanFieldDefs)
      {
        if (fullTableLayout.GetColumnByID(tradeAssLoanFieldDef.CriterionFieldName) == null)
          fullTableLayout.AddColumn(tradeAssLoanFieldDef.ToTableLayoutColumn());
      }
      fullTableLayout.SortByDescription();
      return fullTableLayout;
    }

    private void btnEditPairOff_Click(object sender, EventArgs e)
    {
      if (this.gvPairOffs.SelectedItems.Count < 1)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select a pair off to edit.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
        this.editPairOff((LoanTradePairOff) this.gvPairOffs.SelectedItems[0].Tag);
    }

    private void editPairOff(LoanTradePairOff pairOff)
    {
      using (LoanTradePairOffDialog tradePairOffDialog = new LoanTradePairOffDialog(pairOff))
      {
        tradePairOffDialog.ReadOnly = this.ReadOnly;
        if (!this.ReadOnly && this.loanTrade.Locked)
          tradePairOffDialog.ReadOnly = true;
        if (tradePairOffDialog.ShowDialog((IWin32Window) this) == DialogResult.Cancel || !tradePairOffDialog.DataModified)
          return;
        tradePairOffDialog.CommitChanges();
        this.loanTrade.LoanTradePairOffs[tradePairOffDialog.PairOff.Index - 1] = tradePairOffDialog.PairOff;
        this.modified = true;
        this.loadPairOffs();
        this.loadProfitabilityData();
      }
    }

    private void gvPairOffs_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      this.editPairOff((LoanTradePairOff) this.gvPairOffs.SelectedItems[0].Tag);
    }

    private void standardIconButton1_Click(object sender, EventArgs e)
    {
      RxContactInfo rxContact = new RxContactInfo();
      rxContact.CompanyName = this.txtServicer.Text;
      using (RxBusinessContact rxBusinessContact = new RxBusinessContact("Servicing", rxContact.CompanyName, "", rxContact, CRMRoleType.NotFound, false, ""))
      {
        if (rxBusinessContact.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        if (rxBusinessContact.GoToContact)
          Session.MainScreen.NavigateToContact(rxBusinessContact.SelectedContactInfo);
        else
          this.txtServicer.Text = rxBusinessContact.RxContactRecord.CompanyName;
      }
    }

    private void refreshConfigurableFieldOptions()
    {
      this.cmbCommitmentType.Items.Clear();
      ArrayList secondaryFields1 = Session.ConfigurationManager.GetSecondaryFields(SecondaryFieldTypes.CommitmentTypeOption);
      if (secondaryFields1 != null)
      {
        foreach (string str in secondaryFields1)
          this.cmbCommitmentType.Items.Add((object) str);
      }
      this.cmbTradeDesc.Items.Clear();
      ArrayList secondaryFields2 = Session.ConfigurationManager.GetSecondaryFields(SecondaryFieldTypes.TradeDescriptionOption);
      if (secondaryFields2 != null)
      {
        foreach (string str in secondaryFields2)
          this.cmbTradeDesc.Items.Add((object) str);
      }
      this.cmbServicingType.Items.Clear();
      this.cmbServicingType.Items.Add((object) "Service Released");
      this.cmbServicingType.Items.Add((object) "Service Retained");
    }

    private void securityTradeInfo_TermMonthsUpdated(object sender, TermMonthsUpdatedEventArgs e)
    {
      SimpleTradeFilter currentFilter = this.ctlSimpleSearch.GetCurrentFilter();
      int maxValue = e.Term2 == 0 ? int.MaxValue : e.Term2;
      if (Utils.ParseInt((object) e.Term1) > Utils.ParseInt((object) maxValue))
      {
        this.ctlSimpleSearch.MinTerm = e.Term1.ToString();
        this.ctlSimpleSearch.MaxTerm = maxValue.ToString();
        int num = (int) Utils.Dialog((IWin32Window) this.ParentForm, "This minimum term must be less than or equal to the maximum.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        this.ctlSimpleSearch.MinTerm = "";
        this.ctlSimpleSearch.MaxTerm = "";
      }
      else
      {
        currentFilter.TermRange = new Range<int>(e.Term1, maxValue);
        this.ctlSimpleSearch.SetCurrentFilter(currentFilter);
      }
    }

    private void cmbPricingType_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.cmbPricingType.Text == "Simple")
        this.loanTrade.Pricing.IsAdvancedPricing = false;
      else
        this.loanTrade.Pricing.IsAdvancedPricing = true;
      this.placePricingTypeDDLToControls();
      this.tpPricing_Resize((object) null, (EventArgs) null);
      this.modified = true;
    }

    private void txtInvestor_MouseDown(object sender, MouseEventArgs e)
    {
      this.investorTemplateName = this.txtInvestor.Text;
    }

    private void txtInvestor_Leave(object sender, EventArgs e)
    {
      if (!(this.investorTemplateName != this.txtInvestor.Text))
        return;
      this.investorTemplateName = this.txtInvestor.Text;
      this.applyInvestor((Investor) null);
      this.txtInvestor.Text = this.investorTemplateName;
      if (!(this.investorTemplateName != string.Empty))
        return;
      InvestorTemplate templateSettings = (InvestorTemplate) Session.SessionObjects.ConfigurationManager.GetTemplateSettings(EllieMae.EMLite.ClientServer.TemplateSettingsType.Investor, new FileSystemEntry("\\" + this.investorTemplateName, FileSystemEntry.Types.File, (string) null));
      if (templateSettings != null)
        this.applyInvestor(templateSettings.CompanyInformation);
      else
        this.loanTrade.InvestorName = this.txtInvestor.Text;
    }

    private void cmbServicingType_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.SetupPricingGrid(this.cmbServicingType.Text);
      this.onFieldValueChanged(sender, e);
    }

    private void SetupPricingGrid(string type)
    {
      if (type != "Service Retained")
        this.ctlPricing.ServiceFeeColumn(false);
      else
        this.ctlPricing.ServiceFeeColumn(true);
    }

    public void SetNoteHistoryTab() => this.tabTrade.SelectedTab = this.tpHistory;

    private void tabTrade_Selected(object sender, TabControlEventArgs e)
    {
      if (this.tabTrade.SelectedTab != this.tpPricing)
        return;
      this.tpPricing_Resize(sender, (EventArgs) e);
    }

    private void checkbox_bulkDelivery_CheckedChanged(object sender, EventArgs e)
    {
      this.DisplayPricingPage();
      if (this.checkbox_bulkDelivery.Checked)
      {
        this.lbtn_WABP.Enabled = true;
        this.txt_WABP.Enabled = this.lbtn_WABP.Locked;
        this.ctlLoanList.DisableViewEligible();
        this.RefreshWeightAvgBulkPrice();
      }
      else
      {
        this.lbtn_WABP.Enabled = false;
        this.txt_WABP.Enabled = false;
        this.txt_WABP.Text = "";
        this.ctlLoanList.EnableViewEligible();
      }
      this.modified = true;
    }

    private void DisplayPricingPage()
    {
      if (this.checkbox_bulkDelivery.Checked)
      {
        this.tabTrade.TabPages.Remove(this.tpPricing);
        this.isPricePageShow = false;
      }
      else
      {
        if (this.isPricePageShow)
          return;
        this.tabTrade.TabPages.Insert(1, this.tpPricing);
        this.isPricePageShow = true;
      }
    }

    private void lbtn_WABP_Click(object sender, EventArgs e)
    {
      if (this.readOnly)
        return;
      this.lbtn_WABP.Locked = !this.lbtn_WABP.Locked;
      this.txt_WABP.Enabled = this.lbtn_WABP.Locked;
      if (this.txt_WABP.Enabled)
        this.txt_WABP.Text = "";
      else
        this.RefreshWeightAvgBulkPrice();
    }

    public void RefreshWeightAvgBulkPrice()
    {
      if (!this.IsCalculatedWABP())
        return;
      List<LoanToTradeAssignmentBase> assignedPendingLoans = this.GetAllAssignedPendingLoans();
      this.loanTrade.IsWeightedAvgBulkPriceLocked = this.lbtn_WABP.Locked;
      this.loanTrade.IsBulkDelivery = this.checkbox_bulkDelivery.Checked;
      if (assignedPendingLoans != null && assignedPendingLoans.Count<LoanToTradeAssignmentBase>() > 0)
      {
        Decimal weightedAvgBulkPrice = LoanTradeCalculation.CalculateWeightedAvgBulkPrice(assignedPendingLoans.Sum<LoanToTradeAssignmentBase>((Func<LoanToTradeAssignmentBase, Decimal>) (a => ((LoanTradeAssignment) a).TotalPrice * (Decimal) a.PipelineInfo.Info[(object) "Loan.TotalLoanAmount"])), assignedPendingLoans.Sum<LoanToTradeAssignmentBase>((Func<LoanToTradeAssignmentBase, Decimal>) (a => (Decimal) a.PipelineInfo.Info[(object) "Loan.TotalLoanAmount"])), this.loanTrade);
        this.txt_WABP.Text = weightedAvgBulkPrice == 0M ? "0.00000" : Utils.ParseDecimal((object) weightedAvgBulkPrice, 5M).ToString("#,##0.00000;;\"\"");
      }
      else
        this.txt_WABP.Text = "0.00000";
    }

    public Decimal GetLoanTotalPrice(LoanTradeAssignment loanInfo)
    {
      return this.IsCalculatedWABP() ? loanInfo.TotalPrice : Utils.ParseDecimal((object) this.txt_WABP.Text.Trim());
    }

    private void txt_WABP_TextChanged(object sender, EventArgs e)
    {
      this.onLoanUpdatableFieldValueChanged(sender, e);
    }

    public Decimal GetOpenAmount() => throw new NotImplementedException();

    private void stopTimer()
    {
      if (this.loanTrade.status == TradeStatus.Pending)
        return;
      this.refreshTimer.Stop();
      int num = (int) new TradeLoanUpdateNotificationDialog(this.loanTrade.Name, " completed").ShowDialog();
    }

    private void Refresh_Tick(object sender, EventArgs e)
    {
      if (Session.LoanTradeManager.GetTradeStatus(this.loanTrade.TradeID) == TradeStatus.Pending)
        return;
      LoanTradeInfo trade = Session.LoanTradeManager.GetTrade(this.loanTrade.TradeID);
      this.RefreshData(trade, trade.Status == TradeStatus.Archived ? 1 : 0);
    }

    private void subscribeEventHandler()
    {
      if (this.loanTrade.status != TradeStatus.Pending || this.subscribed)
        return;
      Tracing.Log(TradeEditor.sw, this.className, TraceLevel.Info, "Subscribe onTradeLoanUpdateNotification for Loan Trade");
      TradeLoanUpdateNotificationClientListener.TradeLoanUpdateNotificationActivity += new TradeLoanUpdateNotificationEventHandler(this.onTradeLoanUpdateNotification);
      this.subscribed = true;
    }

    private void unSubscribeEventHandler()
    {
      if (!this.subscribed)
        return;
      Tracing.Log(TradeEditor.sw, this.className, TraceLevel.Info, "UnSubscribe onTradeLoanUpdateNotification for Loan Trade");
      TradeLoanUpdateNotificationClientListener.TradeLoanUpdateNotificationActivity -= new TradeLoanUpdateNotificationEventHandler(this.onTradeLoanUpdateNotification);
      this.subscribed = false;
    }

    private void onTradeLoanUpdateNotification(object sender, TradeLoanUpdateArgs eventArgs)
    {
      Tracing.Log(TradeEditor.sw, this.className, TraceLevel.Info, "In onTradeLoanUpdateNotification for " + this.className);
      if (TradeManagementConsole.Instance.CurrentScreen.ToString() != this.className || !string.IsNullOrEmpty(eventArgs.TradeId) && int.Parse(eventArgs.TradeId) != this.loanTrade.TradeID)
      {
        Tracing.Log(TradeEditor.sw, this.className, TraceLevel.Info, string.Format("Do not need to refresh loan trade. Current TradeId: {0}, Received TradeId: {1}, CorrelationId: {2}, Date: {3}", (object) this.loanTrade.TradeID, (object) eventArgs.TradeId, (object) eventArgs.CorrelationId, (object) eventArgs.Timestamp.ToString()));
      }
      else
      {
        if (!(eventArgs.TradeStatus != TradeStatus.Pending.ToString()))
          return;
        LoanTradeInfo newTrade = Session.LoanTradeManager.GetTrade(this.loanTrade.TradeID);
        this.BeginInvoke((Delegate) (() => this.RefreshData(newTrade, newTrade.Status == TradeStatus.Archived ? 1 : 0)));
        int num = (int) new TradeLoanUpdateNotificationDialog(newTrade.Name, " completed").ShowDialog();
      }
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (TradeEditor));
      this.toolTips = new ToolTip(this.components);
      this.standardIconButton1 = new StandardIconButton();
      this.btnInvestorTemplate = new StandardIconButton();
      this.btnExportHistory = new StandardIconButton();
      this.btnList = new StandardIconButton();
      this.btnSave = new StandardIconButton();
      this.grpEditor = new BorderPanel();
      this.tabTrade = new TabControl();
      this.tpDetails = new TabPage();
      this.pnlDetails = new Panel();
      this.pnlRight = new Panel();
      this.pnlFilter = new Panel();
      this.vsAdvSearch = new VerticalSeparator();
      this.btnSavedSearchesSimple = new Button();
      this.btnSavedSearchesAdv = new Button();
      this.cboSearchType = new ComboBox();
      this.ctlSimpleSearch = new SimpleSearchControl();
      this.ctlAdvancedSearch = new AdvancedSearchControl();
      this.collapsibleSplitter2 = new CollapsibleSplitter();
      this.pnlRightBottom = new Panel();
      this.grpAddress = new GroupContainer();
      this.ctlContactInfo = new InvestorAddressEditor();
      this.pnlPairOff = new Panel();
      this.grpPairOffs = new TableContainer();
      this.flpPairOffs = new FlowLayoutPanel();
      this.btnEditPairOff = new StandardIconButton();
      this.gvPairOffs = new GridView();
      this.collapsibleSplitter1 = new CollapsibleSplitter();
      this.pnlLeft = new Panel();
      this.panel1 = new Panel();
      this.groupContainer1 = new GroupContainer();
      this.panel2 = new Panel();
      this.lbtn_WABP = new FieldLockButton();
      this.txt_WABP = new TextBox();
      this.checkbox_bulkDelivery = new CheckBox();
      this.label22 = new Label();
      this.label14 = new Label();
      this.txtServicer = new TextBox();
      this.cmbServicingType = new ComboBox();
      this.label11 = new Label();
      this.label2 = new Label();
      this.txtNetProfit = new TextBox();
      this.label25 = new Label();
      this.txtMiscFee = new TextBox();
      this.label23 = new Label();
      this.txtPairOffAmt = new TextBox();
      this.label21 = new Label();
      this.txtGainLoss = new TextBox();
      this.label19 = new Label();
      this.label9 = new Label();
      this.label18 = new Label();
      this.cmbTradeDesc = new ComboBox();
      this.label17 = new Label();
      this.dtPurchase = new DatePicker();
      this.label16 = new Label();
      this.label26 = new Label();
      this.label15 = new Label();
      this.cmbCommitmentType = new ComboBox();
      this.label1 = new Label();
      this.txtContract = new TextBox();
      this.dtActualDelivery = new DatePicker();
      this.label24 = new Label();
      this.txtName = new TextBox();
      this.label8 = new Label();
      this.label13 = new Label();
      this.txtMaxAmt = new TextBox();
      this.dtTargetDelivery = new DatePicker();
      this.cboContract = new ComboBox();
      this.dtCommitment = new DatePicker();
      this.dtEarlyDelivery = new DatePicker();
      this.label7 = new Label();
      this.txtInvestor = new TextBox();
      this.dtInvestorDelivery = new DatePicker();
      this.label20 = new Label();
      this.label6 = new Label();
      this.txtMinAmt = new TextBox();
      this.label12 = new Label();
      this.txtInvestorTradeNum = new TextBox();
      this.label5 = new Label();
      this.txtInvestorCommitNum = new TextBox();
      this.label4 = new Label();
      this.txtAmount = new TextBox();
      this.label3 = new Label();
      this.label10 = new Label();
      this.txtTolerance = new TextBox();
      this.collapsibleSplitter3 = new CollapsibleSplitter();
      this.pnlSecurityTradeInfo = new Panel();
      this.securityTradeInfo = new SecurityTradeSmallEditor();
      this.tpPricing = new TabPage();
      this.pnlPricing = new Panel();
      this.msrPricingEditor1 = new MSRPricingEditor();
      this.ctlAdvancedPricing = new TradeAdvancedPricingEditor();
      this.vsPricingType = new VerticalSeparator();
      this.cmbPricingType = new ComboBox();
      this.btnAdjustmentTemplate = new Button();
      this.btnSRPTemplate = new Button();
      this.ctlAdjustments = new PriceAdjustmentListEditor();
      this.ctlSRP = new SRPTableEditor();
      this.grpBuyUpDown = new GroupContainer();
      this.label29 = new Label();
      this.label33 = new Label();
      this.txtRateAdjustment = new TextBox();
      this.label32 = new Label();
      this.label30 = new Label();
      this.txtBuyDown = new TextBox();
      this.txtBuyUp = new TextBox();
      this.label31 = new Label();
      this.ctlPricing = new TradePricingEditor();
      this.tpLoans = new TabPage();
      this.tpHistory = new TabPage();
      this.pnlHistory = new Panel();
      this.grpHistory = new GroupContainer();
      this.gvHistory = new GridView();
      this.grpNotes = new GroupContainer();
      this.txtNotes = new TextBox();
      this.btnDateStamp = new Button();
      this.gradientPanel1 = new GradientPanel();
      this.flowLayoutPanel2 = new FlowLayoutPanel();
      this.lblTradeName = new Label();
      this.pictPending = new PictureBox();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      ((ISupportInitialize) this.standardIconButton1).BeginInit();
      ((ISupportInitialize) this.btnInvestorTemplate).BeginInit();
      ((ISupportInitialize) this.btnExportHistory).BeginInit();
      ((ISupportInitialize) this.btnList).BeginInit();
      ((ISupportInitialize) this.btnSave).BeginInit();
      this.grpEditor.SuspendLayout();
      this.tabTrade.SuspendLayout();
      this.tpDetails.SuspendLayout();
      this.pnlDetails.SuspendLayout();
      this.pnlRight.SuspendLayout();
      this.pnlFilter.SuspendLayout();
      this.pnlRightBottom.SuspendLayout();
      this.grpAddress.SuspendLayout();
      this.pnlPairOff.SuspendLayout();
      this.grpPairOffs.SuspendLayout();
      this.flpPairOffs.SuspendLayout();
      ((ISupportInitialize) this.btnEditPairOff).BeginInit();
      this.pnlLeft.SuspendLayout();
      this.panel1.SuspendLayout();
      this.groupContainer1.SuspendLayout();
      this.panel2.SuspendLayout();
      this.pnlSecurityTradeInfo.SuspendLayout();
      this.tpPricing.SuspendLayout();
      this.pnlPricing.SuspendLayout();
      this.grpBuyUpDown.SuspendLayout();
      this.tpHistory.SuspendLayout();
      this.pnlHistory.SuspendLayout();
      this.grpHistory.SuspendLayout();
      this.grpNotes.SuspendLayout();
      this.gradientPanel1.SuspendLayout();
      this.flowLayoutPanel2.SuspendLayout();
      ((ISupportInitialize) this.pictPending).BeginInit();
      this.flowLayoutPanel1.SuspendLayout();
      this.SuspendLayout();
      this.standardIconButton1.BackColor = Color.Transparent;
      this.standardIconButton1.Location = new Point(241, 459);
      this.standardIconButton1.MouseDownImage = (Image) null;
      this.standardIconButton1.Name = "standardIconButton1";
      this.standardIconButton1.Size = new Size(16, 16);
      this.standardIconButton1.StandardButtonType = StandardIconButton.ButtonType.RolodexButton;
      this.standardIconButton1.TabIndex = 40;
      this.standardIconButton1.TabStop = false;
      this.toolTips.SetToolTip((Control) this.standardIconButton1, "Select Servicer");
      this.standardIconButton1.Click += new EventHandler(this.standardIconButton1_Click);
      this.btnInvestorTemplate.BackColor = Color.Transparent;
      this.btnInvestorTemplate.Location = new Point(253, 162);
      this.btnInvestorTemplate.MouseDownImage = (Image) null;
      this.btnInvestorTemplate.Name = "btnInvestorTemplate";
      this.btnInvestorTemplate.Size = new Size(16, 16);
      this.btnInvestorTemplate.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnInvestorTemplate.TabIndex = 13;
      this.btnInvestorTemplate.TabStop = false;
      this.toolTips.SetToolTip((Control) this.btnInvestorTemplate, "Select Investor");
      this.btnInvestorTemplate.Click += new EventHandler(this.btnInvestorTemplate_Click);
      this.btnExportHistory.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnExportHistory.BackColor = Color.Transparent;
      this.btnExportHistory.Location = new Point(422, 5);
      this.btnExportHistory.MouseDownImage = (Image) null;
      this.btnExportHistory.Name = "btnExportHistory";
      this.btnExportHistory.Size = new Size(16, 16);
      this.btnExportHistory.StandardButtonType = StandardIconButton.ButtonType.ExcelButton;
      this.btnExportHistory.TabIndex = 3;
      this.btnExportHistory.TabStop = false;
      this.toolTips.SetToolTip((Control) this.btnExportHistory, "Export to Excel");
      this.btnExportHistory.Click += new EventHandler(this.btnExportHistory_Click);
      this.btnList.BackColor = Color.Transparent;
      this.btnList.Location = new Point(595, 3);
      this.btnList.Margin = new Padding(2, 3, 0, 3);
      this.btnList.MouseDownImage = (Image) null;
      this.btnList.Name = "btnList";
      this.btnList.Size = new Size(16, 16);
      this.btnList.StandardButtonType = StandardIconButton.ButtonType.CloseButton;
      this.btnList.TabIndex = 7;
      this.btnList.TabStop = false;
      this.toolTips.SetToolTip((Control) this.btnList, "Exit Trade");
      this.btnList.Click += new EventHandler(this.btnList_Click);
      this.btnSave.BackColor = Color.Transparent;
      this.btnSave.Location = new Point(574, 3);
      this.btnSave.MouseDownImage = (Image) null;
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(16, 16);
      this.btnSave.StandardButtonType = StandardIconButton.ButtonType.SaveButton;
      this.btnSave.TabIndex = 6;
      this.btnSave.TabStop = false;
      this.toolTips.SetToolTip((Control) this.btnSave, "Save Trade");
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.grpEditor.BackColor = Color.Transparent;
      this.grpEditor.Borders = AnchorStyles.None;
      this.grpEditor.Controls.Add((Control) this.tabTrade);
      this.grpEditor.Dock = DockStyle.Fill;
      this.grpEditor.Location = new Point(0, 31);
      this.grpEditor.Name = "grpEditor";
      this.grpEditor.Padding = new Padding(2, 2, 0, 0);
      this.grpEditor.Size = new Size(1053, 653);
      this.grpEditor.TabIndex = 5;
      this.grpEditor.Text = "<Trade Name>";
      this.tabTrade.Controls.Add((Control) this.tpDetails);
      this.tabTrade.Controls.Add((Control) this.tpPricing);
      this.tabTrade.Controls.Add((Control) this.tpLoans);
      this.tabTrade.Controls.Add((Control) this.tpHistory);
      this.tabTrade.Dock = DockStyle.Fill;
      this.tabTrade.ItemSize = new Size(44, 20);
      this.tabTrade.Location = new Point(2, 2);
      this.tabTrade.Name = "tabTrade";
      this.tabTrade.Padding = new Point(11, 3);
      this.tabTrade.SelectedIndex = 0;
      this.tabTrade.Size = new Size(1051, 651);
      this.tabTrade.TabIndex = 1;
      this.tabTrade.SelectedIndexChanged += new EventHandler(this.tabTrade_SelectedIndexChanged);
      this.tabTrade.Selected += new TabControlEventHandler(this.tabTrade_Selected);
      this.tpDetails.Controls.Add((Control) this.pnlDetails);
      this.tpDetails.Location = new Point(4, 24);
      this.tpDetails.Name = "tpDetails";
      this.tpDetails.Padding = new Padding(0, 2, 2, 2);
      this.tpDetails.Size = new Size(1043, 623);
      this.tpDetails.TabIndex = 0;
      this.tpDetails.Tag = (object) "Details";
      this.tpDetails.Text = "Details";
      this.tpDetails.UseVisualStyleBackColor = true;
      this.tpDetails.Resize += new EventHandler(this.tpDetails_Resize);
      this.pnlDetails.Controls.Add((Control) this.pnlRight);
      this.pnlDetails.Controls.Add((Control) this.collapsibleSplitter1);
      this.pnlDetails.Controls.Add((Control) this.pnlLeft);
      this.pnlDetails.Dock = DockStyle.Fill;
      this.pnlDetails.Location = new Point(0, 2);
      this.pnlDetails.Name = "pnlDetails";
      this.pnlDetails.Size = new Size(1041, 619);
      this.pnlDetails.TabIndex = 6;
      this.pnlRight.Controls.Add((Control) this.pnlFilter);
      this.pnlRight.Controls.Add((Control) this.collapsibleSplitter2);
      this.pnlRight.Controls.Add((Control) this.pnlRightBottom);
      this.pnlRight.Dock = DockStyle.Fill;
      this.pnlRight.Location = new Point(310, 0);
      this.pnlRight.Name = "pnlRight";
      this.pnlRight.Size = new Size(731, 619);
      this.pnlRight.TabIndex = 8;
      this.pnlFilter.Controls.Add((Control) this.vsAdvSearch);
      this.pnlFilter.Controls.Add((Control) this.btnSavedSearchesSimple);
      this.pnlFilter.Controls.Add((Control) this.btnSavedSearchesAdv);
      this.pnlFilter.Controls.Add((Control) this.cboSearchType);
      this.pnlFilter.Controls.Add((Control) this.ctlSimpleSearch);
      this.pnlFilter.Controls.Add((Control) this.ctlAdvancedSearch);
      this.pnlFilter.Dock = DockStyle.Fill;
      this.pnlFilter.Location = new Point(0, 0);
      this.pnlFilter.Name = "pnlFilter";
      this.pnlFilter.Size = new Size(731, 287);
      this.pnlFilter.TabIndex = 5;
      this.vsAdvSearch.Location = new Point(396, 8);
      this.vsAdvSearch.MaximumSize = new Size(2, 16);
      this.vsAdvSearch.MinimumSize = new Size(2, 16);
      this.vsAdvSearch.Name = "vsAdvSearch";
      this.vsAdvSearch.Size = new Size(2, 16);
      this.vsAdvSearch.TabIndex = 6;
      this.vsAdvSearch.Text = "verticalSeparator2";
      this.btnSavedSearchesSimple.BackColor = SystemColors.Control;
      this.btnSavedSearchesSimple.Location = new Point(516, 4);
      this.btnSavedSearchesSimple.Margin = new Padding(0);
      this.btnSavedSearchesSimple.Name = "btnSavedSearchesSimple";
      this.btnSavedSearchesSimple.Padding = new Padding(2, 0, 0, 0);
      this.btnSavedSearchesSimple.Size = new Size(102, 22);
      this.btnSavedSearchesSimple.TabIndex = 12;
      this.btnSavedSearchesSimple.Text = "&Saved Searches";
      this.btnSavedSearchesSimple.UseVisualStyleBackColor = true;
      this.btnSavedSearchesSimple.Click += new EventHandler(this.btnSavedSearches_Click);
      this.btnSavedSearchesAdv.BackColor = SystemColors.Control;
      this.btnSavedSearchesAdv.Location = new Point(400, 4);
      this.btnSavedSearchesAdv.Margin = new Padding(0);
      this.btnSavedSearchesAdv.Name = "btnSavedSearchesAdv";
      this.btnSavedSearchesAdv.Padding = new Padding(2, 0, 0, 0);
      this.btnSavedSearchesAdv.Size = new Size(102, 22);
      this.btnSavedSearchesAdv.TabIndex = 6;
      this.btnSavedSearchesAdv.Text = "&Saved Searches";
      this.btnSavedSearchesAdv.UseVisualStyleBackColor = true;
      this.btnSavedSearchesAdv.Click += new EventHandler(this.btnSavedSearches_Click);
      this.cboSearchType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboSearchType.FormattingEnabled = true;
      this.cboSearchType.Items.AddRange(new object[2]
      {
        (object) "Simple",
        (object) "Advanced"
      });
      this.cboSearchType.Location = new Point(116, 2);
      this.cboSearchType.Name = "cboSearchType";
      this.cboSearchType.Size = new Size(85, 22);
      this.cboSearchType.TabIndex = 11;
      this.cboSearchType.SelectedIndexChanged += new EventHandler(this.cboSearchType_SelectedIndexChanged);
      this.ctlSimpleSearch.BackColor = SystemColors.Window;
      this.ctlSimpleSearch.DataModified = false;
      this.ctlSimpleSearch.Dock = DockStyle.Fill;
      this.ctlSimpleSearch.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.ctlSimpleSearch.Location = new Point(0, 0);
      this.ctlSimpleSearch.MaxTerm = "";
      this.ctlSimpleSearch.MinTerm = "";
      this.ctlSimpleSearch.Name = "ctlSimpleSearch";
      this.ctlSimpleSearch.NoteRateMax = "";
      this.ctlSimpleSearch.NoteRateMin = "";
      this.ctlSimpleSearch.ReadOnly = false;
      this.ctlSimpleSearch.Size = new Size(731, 287);
      this.ctlSimpleSearch.TabIndex = 2;
      this.ctlSimpleSearch.Title = "Eligible Loans";
      this.ctlAdvancedSearch.AllowDynamicOperators = false;
      this.ctlAdvancedSearch.DDMSetting = false;
      this.ctlAdvancedSearch.Dock = DockStyle.Fill;
      this.ctlAdvancedSearch.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.ctlAdvancedSearch.Location = new Point(0, 0);
      this.ctlAdvancedSearch.Name = "ctlAdvancedSearch";
      this.ctlAdvancedSearch.Size = new Size(731, 287);
      this.ctlAdvancedSearch.TabIndex = 3;
      this.ctlAdvancedSearch.Title = "Filters";
      this.collapsibleSplitter2.AnimationDelay = 20;
      this.collapsibleSplitter2.AnimationStep = 20;
      this.collapsibleSplitter2.BorderStyle3D = Border3DStyle.Flat;
      this.collapsibleSplitter2.ControlToHide = (Control) this.pnlRightBottom;
      this.collapsibleSplitter2.Dock = DockStyle.Bottom;
      this.collapsibleSplitter2.ExpandParentForm = false;
      this.collapsibleSplitter2.Location = new Point(0, 287);
      this.collapsibleSplitter2.Name = "collapsibleSplitter2";
      this.collapsibleSplitter2.TabIndex = 1;
      this.collapsibleSplitter2.TabStop = false;
      this.collapsibleSplitter2.UseAnimations = false;
      this.collapsibleSplitter2.VisualStyle = VisualStyles.Encompass;
      this.pnlRightBottom.AutoScroll = true;
      this.pnlRightBottom.Controls.Add((Control) this.grpAddress);
      this.pnlRightBottom.Controls.Add((Control) this.pnlPairOff);
      this.pnlRightBottom.Dock = DockStyle.Bottom;
      this.pnlRightBottom.Location = new Point(0, 294);
      this.pnlRightBottom.Name = "pnlRightBottom";
      this.pnlRightBottom.Size = new Size(731, 325);
      this.pnlRightBottom.TabIndex = 0;
      this.grpAddress.Controls.Add((Control) this.ctlContactInfo);
      this.grpAddress.Dock = DockStyle.Fill;
      this.grpAddress.HeaderForeColor = SystemColors.ControlText;
      this.grpAddress.Location = new Point(0, 125);
      this.grpAddress.Name = "grpAddress";
      this.grpAddress.Size = new Size(731, 200);
      this.grpAddress.TabIndex = 0;
      this.grpAddress.Text = "Addresses";
      this.ctlContactInfo.AssigneeNameTextBoxText = "";
      this.ctlContactInfo.BackColor = Color.Transparent;
      this.ctlContactInfo.CurrentAssignee = (ContactInformation) null;
      this.ctlContactInfo.CurrentDealer = (ContactInformation) null;
      this.ctlContactInfo.CurrentInvestor = (Investor) null;
      this.ctlContactInfo.Dock = DockStyle.Fill;
      this.ctlContactInfo.Location = new Point(1, 26);
      this.ctlContactInfo.Name = "ctlContactInfo";
      this.ctlContactInfo.Padding = new Padding(1, 1, 0, 0);
      this.ctlContactInfo.ReadOnly = false;
      this.ctlContactInfo.Size = new Size(729, 173);
      this.ctlContactInfo.TabIndex = 0;
      this.pnlPairOff.Controls.Add((Control) this.grpPairOffs);
      this.pnlPairOff.Dock = DockStyle.Top;
      this.pnlPairOff.Location = new Point(0, 0);
      this.pnlPairOff.Name = "pnlPairOff";
      this.pnlPairOff.Padding = new Padding(0, 0, 0, 4);
      this.pnlPairOff.Size = new Size(731, 125);
      this.pnlPairOff.TabIndex = 3;
      this.grpPairOffs.Controls.Add((Control) this.flpPairOffs);
      this.grpPairOffs.Controls.Add((Control) this.gvPairOffs);
      this.grpPairOffs.Dock = DockStyle.Fill;
      this.grpPairOffs.Location = new Point(0, 0);
      this.grpPairOffs.Margin = new Padding(3, 3, 3, 15);
      this.grpPairOffs.Name = "grpPairOffs";
      this.grpPairOffs.Size = new Size(731, 121);
      this.grpPairOffs.Style = TableContainer.ContainerStyle.HeaderOnly;
      this.grpPairOffs.TabIndex = 2;
      this.grpPairOffs.Text = "Pair-Offs";
      this.flpPairOffs.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.flpPairOffs.BackColor = Color.Transparent;
      this.flpPairOffs.Controls.Add((Control) this.btnEditPairOff);
      this.flpPairOffs.FlowDirection = FlowDirection.RightToLeft;
      this.flpPairOffs.Location = new Point(209, 2);
      this.flpPairOffs.Name = "flpPairOffs";
      this.flpPairOffs.Size = new Size(523, 22);
      this.flpPairOffs.TabIndex = 3;
      this.btnEditPairOff.BackColor = Color.Transparent;
      this.btnEditPairOff.Location = new Point(504, 3);
      this.btnEditPairOff.MouseDownImage = (Image) null;
      this.btnEditPairOff.Name = "btnEditPairOff";
      this.btnEditPairOff.Size = new Size(16, 16);
      this.btnEditPairOff.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.btnEditPairOff.TabIndex = 0;
      this.btnEditPairOff.TabStop = false;
      this.btnEditPairOff.Click += new EventHandler(this.btnEditPairOff_Click);
      this.gvPairOffs.AllowMultiselect = false;
      this.gvPairOffs.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Pair-Off #";
      gvColumn1.Width = 100;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Pair-Off Date";
      gvColumn2.Width = 100;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "Pair-Off Amount";
      gvColumn3.Width = 100;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column4";
      gvColumn4.Text = "Pair-Off Fee";
      gvColumn4.Width = 100;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column5";
      gvColumn5.Text = "Gain/Loss";
      gvColumn5.Width = 100;
      this.gvPairOffs.Columns.AddRange(new GVColumn[5]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5
      });
      this.gvPairOffs.Dock = DockStyle.Fill;
      this.gvPairOffs.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvPairOffs.Location = new Point(1, 26);
      this.gvPairOffs.Name = "gvPairOffs";
      this.gvPairOffs.Size = new Size(729, 94);
      this.gvPairOffs.SortOption = GVSortOption.None;
      this.gvPairOffs.TabIndex = 0;
      this.gvPairOffs.ItemDoubleClick += new GVItemEventHandler(this.gvPairOffs_ItemDoubleClick);
      this.collapsibleSplitter1.AnimationDelay = 20;
      this.collapsibleSplitter1.AnimationStep = 20;
      this.collapsibleSplitter1.BorderStyle3D = Border3DStyle.Flat;
      this.collapsibleSplitter1.ControlToHide = (Control) this.pnlLeft;
      this.collapsibleSplitter1.ExpandParentForm = false;
      this.collapsibleSplitter1.Location = new Point(303, 0);
      this.collapsibleSplitter1.Name = "collapsibleSplitter1";
      this.collapsibleSplitter1.TabIndex = 7;
      this.collapsibleSplitter1.TabStop = false;
      this.collapsibleSplitter1.UseAnimations = false;
      this.collapsibleSplitter1.VisualStyle = VisualStyles.Encompass;
      this.pnlLeft.Controls.Add((Control) this.panel1);
      this.pnlLeft.Controls.Add((Control) this.collapsibleSplitter3);
      this.pnlLeft.Controls.Add((Control) this.pnlSecurityTradeInfo);
      this.pnlLeft.Dock = DockStyle.Left;
      this.pnlLeft.Location = new Point(0, 0);
      this.pnlLeft.Name = "pnlLeft";
      this.pnlLeft.Size = new Size(303, 619);
      this.pnlLeft.TabIndex = 6;
      this.panel1.Controls.Add((Control) this.groupContainer1);
      this.panel1.Dock = DockStyle.Fill;
      this.panel1.Location = new Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(303, 325);
      this.panel1.TabIndex = 2;
      this.groupContainer1.Controls.Add((Control) this.panel2);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(303, 325);
      this.groupContainer1.TabIndex = 0;
      this.groupContainer1.Text = "Loan Trade Info";
      this.panel2.AutoScroll = true;
      this.panel2.Controls.Add((Control) this.lbtn_WABP);
      this.panel2.Controls.Add((Control) this.txt_WABP);
      this.panel2.Controls.Add((Control) this.checkbox_bulkDelivery);
      this.panel2.Controls.Add((Control) this.label22);
      this.panel2.Controls.Add((Control) this.label14);
      this.panel2.Controls.Add((Control) this.standardIconButton1);
      this.panel2.Controls.Add((Control) this.txtServicer);
      this.panel2.Controls.Add((Control) this.cmbServicingType);
      this.panel2.Controls.Add((Control) this.label11);
      this.panel2.Controls.Add((Control) this.label2);
      this.panel2.Controls.Add((Control) this.txtNetProfit);
      this.panel2.Controls.Add((Control) this.label25);
      this.panel2.Controls.Add((Control) this.txtMiscFee);
      this.panel2.Controls.Add((Control) this.label23);
      this.panel2.Controls.Add((Control) this.txtPairOffAmt);
      this.panel2.Controls.Add((Control) this.label21);
      this.panel2.Controls.Add((Control) this.txtGainLoss);
      this.panel2.Controls.Add((Control) this.label19);
      this.panel2.Controls.Add((Control) this.label9);
      this.panel2.Controls.Add((Control) this.label18);
      this.panel2.Controls.Add((Control) this.cmbTradeDesc);
      this.panel2.Controls.Add((Control) this.label17);
      this.panel2.Controls.Add((Control) this.dtPurchase);
      this.panel2.Controls.Add((Control) this.label16);
      this.panel2.Controls.Add((Control) this.label26);
      this.panel2.Controls.Add((Control) this.label15);
      this.panel2.Controls.Add((Control) this.cmbCommitmentType);
      this.panel2.Controls.Add((Control) this.label1);
      this.panel2.Controls.Add((Control) this.txtContract);
      this.panel2.Controls.Add((Control) this.dtActualDelivery);
      this.panel2.Controls.Add((Control) this.label24);
      this.panel2.Controls.Add((Control) this.txtName);
      this.panel2.Controls.Add((Control) this.label8);
      this.panel2.Controls.Add((Control) this.label13);
      this.panel2.Controls.Add((Control) this.txtMaxAmt);
      this.panel2.Controls.Add((Control) this.dtTargetDelivery);
      this.panel2.Controls.Add((Control) this.cboContract);
      this.panel2.Controls.Add((Control) this.dtCommitment);
      this.panel2.Controls.Add((Control) this.dtEarlyDelivery);
      this.panel2.Controls.Add((Control) this.label7);
      this.panel2.Controls.Add((Control) this.txtInvestor);
      this.panel2.Controls.Add((Control) this.dtInvestorDelivery);
      this.panel2.Controls.Add((Control) this.label20);
      this.panel2.Controls.Add((Control) this.btnInvestorTemplate);
      this.panel2.Controls.Add((Control) this.label6);
      this.panel2.Controls.Add((Control) this.txtMinAmt);
      this.panel2.Controls.Add((Control) this.label12);
      this.panel2.Controls.Add((Control) this.txtInvestorTradeNum);
      this.panel2.Controls.Add((Control) this.label5);
      this.panel2.Controls.Add((Control) this.txtInvestorCommitNum);
      this.panel2.Controls.Add((Control) this.label4);
      this.panel2.Controls.Add((Control) this.txtAmount);
      this.panel2.Controls.Add((Control) this.label3);
      this.panel2.Controls.Add((Control) this.label10);
      this.panel2.Controls.Add((Control) this.txtTolerance);
      this.panel2.Dock = DockStyle.Fill;
      this.panel2.Location = new Point(1, 26);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(301, 298);
      this.panel2.TabIndex = 23;
      this.lbtn_WABP.Location = new Point(140, 144);
      this.lbtn_WABP.LockedStateToolTip = "Use Default Value";
      this.lbtn_WABP.MaximumSize = new Size(16, 17);
      this.lbtn_WABP.MinimumSize = new Size(16, 17);
      this.lbtn_WABP.Name = "lbtn_WABP";
      this.lbtn_WABP.Size = new Size(16, 17);
      this.lbtn_WABP.TabIndex = 227;
      this.lbtn_WABP.UnlockedStateToolTip = "Enter Data Manually";
      this.lbtn_WABP.Click += new EventHandler(this.lbtn_WABP_Click);
      this.txt_WABP.Location = new Point(164, 140);
      this.txt_WABP.MaxLength = 18;
      this.txt_WABP.Name = "txt_WABP";
      this.txt_WABP.Size = new Size(84, 20);
      this.txt_WABP.TabIndex = 10;
      this.txt_WABP.TextAlign = HorizontalAlignment.Right;
      this.txt_WABP.TextChanged += new EventHandler(this.txt_WABP_TextChanged);
      this.checkbox_bulkDelivery.AutoSize = true;
      this.checkbox_bulkDelivery.Location = new Point(131, 124);
      this.checkbox_bulkDelivery.Name = "checkbox_bulkDelivery";
      this.checkbox_bulkDelivery.Size = new Size(15, 14);
      this.checkbox_bulkDelivery.TabIndex = 9;
      this.checkbox_bulkDelivery.UseVisualStyleBackColor = true;
      this.checkbox_bulkDelivery.CheckedChanged += new EventHandler(this.checkbox_bulkDelivery_CheckedChanged);
      this.label22.AutoSize = true;
      this.label22.Location = new Point(9, 145);
      this.label22.Name = "label22";
      this.label22.Size = new Size((int) sbyte.MaxValue, 14);
      this.label22.TabIndex = 42;
      this.label22.Text = "Weighted Avg. Bulk Price";
      this.label14.AutoSize = true;
      this.label14.Location = new Point(9, 123);
      this.label14.Name = "label14";
      this.label14.Size = new Size(69, 14);
      this.label14.TabIndex = 41;
      this.label14.Text = "Bulk Delivery";
      this.txtServicer.Location = new Point(131, 458);
      this.txtServicer.MaxLength = 64;
      this.txtServicer.Name = "txtServicer";
      this.txtServicer.Size = new Size(104, 20);
      this.txtServicer.TabIndex = 24;
      this.txtServicer.TextAlign = HorizontalAlignment.Right;
      this.txtServicer.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.cmbServicingType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbServicingType.FormattingEnabled = true;
      this.cmbServicingType.Items.AddRange(new object[2]
      {
        (object) "Service Released",
        (object) "Service Retained"
      });
      this.cmbServicingType.Location = new Point(131, 434);
      this.cmbServicingType.Name = "cmbServicingType";
      this.cmbServicingType.Size = new Size(131, 22);
      this.cmbServicingType.TabIndex = 23;
      this.cmbServicingType.SelectedIndexChanged += new EventHandler(this.cmbServicingType_SelectedIndexChanged);
      this.label11.AutoSize = true;
      this.label11.Location = new Point(9, 461);
      this.label11.Name = "label11";
      this.label11.Size = new Size(48, 14);
      this.label11.TabIndex = 37;
      this.label11.Text = "Servicer";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(9, 437);
      this.label2.Name = "label2";
      this.label2.Size = new Size(78, 14);
      this.label2.TabIndex = 36;
      this.label2.Text = "Servicing Type";
      this.txtNetProfit.Location = new Point(131, 546);
      this.txtNetProfit.Name = "txtNetProfit";
      this.txtNetProfit.ReadOnly = true;
      this.txtNetProfit.Size = new Size(131, 20);
      this.txtNetProfit.TabIndex = 28;
      this.txtNetProfit.TextAlign = HorizontalAlignment.Right;
      this.label25.AutoSize = true;
      this.label25.Location = new Point(9, 550);
      this.label25.Name = "label25";
      this.label25.Size = new Size(76, 14);
      this.label25.TabIndex = 34;
      this.label25.Text = "Total Net Profit";
      this.txtMiscFee.Location = new Point(131, 524);
      this.txtMiscFee.MaxLength = 12;
      this.txtMiscFee.Name = "txtMiscFee";
      this.txtMiscFee.Size = new Size(131, 20);
      this.txtMiscFee.TabIndex = 27;
      this.txtMiscFee.TextAlign = HorizontalAlignment.Right;
      this.txtMiscFee.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.txtMiscFee.Validated += new EventHandler(this.txtMiscFee_Validated);
      this.label23.AutoSize = true;
      this.label23.Location = new Point(9, 528);
      this.label23.Name = "label23";
      this.label23.Size = new Size(53, 14);
      this.label23.TabIndex = 33;
      this.label23.Text = "Misc. Fee";
      this.txtPairOffAmt.Location = new Point(131, 502);
      this.txtPairOffAmt.Name = "txtPairOffAmt";
      this.txtPairOffAmt.ReadOnly = true;
      this.txtPairOffAmt.Size = new Size(131, 20);
      this.txtPairOffAmt.TabIndex = 26;
      this.txtPairOffAmt.TextAlign = HorizontalAlignment.Right;
      this.label21.AutoSize = true;
      this.label21.Location = new Point(9, 506);
      this.label21.Name = "label21";
      this.label21.Size = new Size(109, 14);
      this.label21.TabIndex = 31;
      this.label21.Text = "Total Pair-Off Amount";
      this.txtGainLoss.Location = new Point(131, 480);
      this.txtGainLoss.Name = "txtGainLoss";
      this.txtGainLoss.ReadOnly = true;
      this.txtGainLoss.Size = new Size(131, 20);
      this.txtGainLoss.TabIndex = 25;
      this.txtGainLoss.TextAlign = HorizontalAlignment.Right;
      this.label19.AutoSize = true;
      this.label19.Location = new Point(9, 484);
      this.label19.Name = "label19";
      this.label19.Size = new Size(81, 14);
      this.label19.TabIndex = 29;
      this.label19.Text = "Total Gain/Loss";
      this.label9.AutoSize = true;
      this.label9.Location = new Point(9, 324);
      this.label9.Name = "label9";
      this.label9.Size = new Size(113, 14);
      this.label9.TabIndex = 26;
      this.label9.Text = "Investor Delivery Date";
      this.label18.AutoSize = true;
      this.label18.Location = new Point(9, 417);
      this.label18.Name = "label18";
      this.label18.Size = new Size(78, 14);
      this.label18.TabIndex = 10;
      this.label18.Text = "Purchase Date";
      this.cmbTradeDesc.FormattingEnabled = true;
      this.cmbTradeDesc.Location = new Point(131, 98);
      this.cmbTradeDesc.Name = "cmbTradeDesc";
      this.cmbTradeDesc.Size = new Size(139, 22);
      this.cmbTradeDesc.TabIndex = 6;
      this.cmbTradeDesc.SelectedIndexChanged += new EventHandler(this.onFieldValueChanged);
      this.cmbTradeDesc.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.label17.AutoSize = true;
      this.label17.Location = new Point(9, 395);
      this.label17.Name = "label17";
      this.label17.Size = new Size(105, 14);
      this.label17.TabIndex = 8;
      this.label17.Text = "Actual Delivery Date";
      this.dtPurchase.BackColor = SystemColors.Window;
      this.dtPurchase.Location = new Point(131, 411);
      this.dtPurchase.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.dtPurchase.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dtPurchase.Name = "dtPurchase";
      this.dtPurchase.Size = new Size(104, 22);
      this.dtPurchase.TabIndex = 22;
      this.dtPurchase.ToolTip = "";
      this.dtPurchase.Value = new DateTime(0L);
      this.dtPurchase.ValueChanged += new EventHandler(this.onLoanUpdatableFieldValueChanged);
      this.label16.AutoSize = true;
      this.label16.Location = new Point(9, 371);
      this.label16.Name = "label16";
      this.label16.Size = new Size(104, 14);
      this.label16.TabIndex = 6;
      this.label16.Text = "Target Delivery Date";
      this.label26.AutoSize = true;
      this.label26.Location = new Point(9, 101);
      this.label26.Name = "label26";
      this.label26.Size = new Size(92, 14);
      this.label26.TabIndex = 24;
      this.label26.Text = "Trade Description";
      this.label15.AutoSize = true;
      this.label15.Location = new Point(9, 348);
      this.label15.Name = "label15";
      this.label15.Size = new Size(98, 14);
      this.label15.TabIndex = 4;
      this.label15.Text = "Early Delivery Date";
      this.cmbCommitmentType.FormattingEnabled = true;
      this.cmbCommitmentType.Location = new Point(131, 74);
      this.cmbCommitmentType.Name = "cmbCommitmentType";
      this.cmbCommitmentType.Size = new Size(139, 22);
      this.cmbCommitmentType.TabIndex = 5;
      this.cmbCommitmentType.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(9, 8);
      this.label1.Name = "label1";
      this.label1.Size = new Size(47, 14);
      this.label1.TabIndex = 0;
      this.label1.Text = "Trade ID";
      this.txtContract.Location = new Point(131, 51);
      this.txtContract.MaxLength = 64;
      this.txtContract.Name = "txtContract";
      this.txtContract.ReadOnly = true;
      this.txtContract.Size = new Size(138, 20);
      this.txtContract.TabIndex = 3;
      this.txtContract.Visible = false;
      this.dtActualDelivery.BackColor = SystemColors.Window;
      this.dtActualDelivery.Location = new Point(131, 387);
      this.dtActualDelivery.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.dtActualDelivery.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dtActualDelivery.Name = "dtActualDelivery";
      this.dtActualDelivery.Size = new Size(104, 22);
      this.dtActualDelivery.TabIndex = 21;
      this.dtActualDelivery.ToolTip = "";
      this.dtActualDelivery.Value = new DateTime(0L);
      this.dtActualDelivery.ValueChanged += new EventHandler(this.onLoanUpdatableFieldValueChanged);
      this.label24.AutoSize = true;
      this.label24.Location = new Point(9, 78);
      this.label24.Name = "label24";
      this.label24.Size = new Size(90, 14);
      this.label24.TabIndex = 22;
      this.label24.Text = "Commitment Type";
      this.txtName.Location = new Point(131, 5);
      this.txtName.MaxLength = 64;
      this.txtName.Name = "txtName";
      this.txtName.Size = new Size(139, 20);
      this.txtName.TabIndex = 1;
      this.txtName.TextChanged += new EventHandler(this.onLoanUpdatableFieldValueChanged);
      this.label8.AutoSize = true;
      this.label8.Location = new Point(9, 53);
      this.label8.Name = "label8";
      this.label8.Size = new Size(93, 14);
      this.label8.TabIndex = 8;
      this.label8.Text = "Master Contract #";
      this.label13.AutoSize = true;
      this.label13.Location = new Point(9, 31);
      this.label13.Name = "label13";
      this.label13.Size = new Size(89, 14);
      this.label13.TabIndex = 0;
      this.label13.Text = "Commitment Date";
      this.txtMaxAmt.Location = new Point(131, 294);
      this.txtMaxAmt.MaxLength = 12;
      this.txtMaxAmt.Name = "txtMaxAmt";
      this.txtMaxAmt.ReadOnly = true;
      this.txtMaxAmt.Size = new Size(139, 20);
      this.txtMaxAmt.TabIndex = 17;
      this.txtMaxAmt.TextAlign = HorizontalAlignment.Right;
      this.dtTargetDelivery.BackColor = SystemColors.Window;
      this.dtTargetDelivery.Location = new Point(131, 363);
      this.dtTargetDelivery.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.dtTargetDelivery.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dtTargetDelivery.Name = "dtTargetDelivery";
      this.dtTargetDelivery.Size = new Size(104, 22);
      this.dtTargetDelivery.TabIndex = 20;
      this.dtTargetDelivery.ToolTip = "";
      this.dtTargetDelivery.Value = new DateTime(0L);
      this.dtTargetDelivery.ValueChanged += new EventHandler(this.onLoanUpdatableFieldValueChanged);
      this.cboContract.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboContract.FormattingEnabled = true;
      this.cboContract.Location = new Point(131, 50);
      this.cboContract.Name = "cboContract";
      this.cboContract.Size = new Size(139, 22);
      this.cboContract.TabIndex = 4;
      this.cboContract.SelectedIndexChanged += new EventHandler(this.cboContract_SelectedIndexChanged);
      this.dtCommitment.BackColor = SystemColors.Window;
      this.dtCommitment.Location = new Point(131, 27);
      this.dtCommitment.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.dtCommitment.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dtCommitment.Name = "dtCommitment";
      this.dtCommitment.Size = new Size(104, 22);
      this.dtCommitment.TabIndex = 2;
      this.dtCommitment.ToolTip = "";
      this.dtCommitment.Value = new DateTime(0L);
      this.dtCommitment.ValueChanged += new EventHandler(this.onCommitmentDateChanged);
      this.dtEarlyDelivery.BackColor = SystemColors.Window;
      this.dtEarlyDelivery.Location = new Point(131, 340);
      this.dtEarlyDelivery.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.dtEarlyDelivery.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dtEarlyDelivery.Name = "dtEarlyDelivery";
      this.dtEarlyDelivery.Size = new Size(104, 22);
      this.dtEarlyDelivery.TabIndex = 19;
      this.dtEarlyDelivery.ToolTip = "";
      this.dtEarlyDelivery.Value = new DateTime(0L);
      this.dtEarlyDelivery.ValueChanged += new EventHandler(this.onFieldValueChanged);
      this.label7.AutoSize = true;
      this.label7.Location = new Point(9, 165);
      this.label7.Name = "label7";
      this.label7.Size = new Size(46, 14);
      this.label7.TabIndex = 14;
      this.label7.Text = "Investor";
      this.txtInvestor.Location = new Point(131, 162);
      this.txtInvestor.MaxLength = 64;
      this.txtInvestor.Name = "txtInvestor";
      this.txtInvestor.Size = new Size(117, 20);
      this.txtInvestor.TabIndex = 11;
      this.txtInvestor.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.txtInvestor.Leave += new EventHandler(this.txtInvestor_Leave);
      this.txtInvestor.MouseDown += new MouseEventHandler(this.txtInvestor_MouseDown);
      this.dtInvestorDelivery.BackColor = SystemColors.Window;
      this.dtInvestorDelivery.Location = new Point(131, 317);
      this.dtInvestorDelivery.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.dtInvestorDelivery.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dtInvestorDelivery.Name = "dtInvestorDelivery";
      this.dtInvestorDelivery.Size = new Size(104, 22);
      this.dtInvestorDelivery.TabIndex = 18;
      this.dtInvestorDelivery.ToolTip = "";
      this.dtInvestorDelivery.Value = new DateTime(0L);
      this.dtInvestorDelivery.ValueChanged += new EventHandler(this.onLoanUpdatableFieldValueChanged);
      this.label20.AutoSize = true;
      this.label20.Location = new Point(9, 297);
      this.label20.Name = "label20";
      this.label20.Size = new Size(90, 14);
      this.label20.TabIndex = 20;
      this.label20.Text = "Maximum Amount";
      this.label6.AutoSize = true;
      this.label6.Location = new Point(9, 186);
      this.label6.Name = "label6";
      this.label6.Size = new Size(86, 14);
      this.label6.TabIndex = 6;
      this.label6.Text = "Investor Trade #";
      this.txtMinAmt.Location = new Point(131, 271);
      this.txtMinAmt.MaxLength = 12;
      this.txtMinAmt.Name = "txtMinAmt";
      this.txtMinAmt.ReadOnly = true;
      this.txtMinAmt.Size = new Size(139, 20);
      this.txtMinAmt.TabIndex = 16;
      this.txtMinAmt.TextAlign = HorizontalAlignment.Right;
      this.label12.AutoSize = true;
      this.label12.Location = new Point(9, 274);
      this.label12.Name = "label12";
      this.label12.Size = new Size(86, 14);
      this.label12.TabIndex = 18;
      this.label12.Text = "Minimum Amount";
      this.txtInvestorTradeNum.Location = new Point(131, 183);
      this.txtInvestorTradeNum.MaxLength = 64;
      this.txtInvestorTradeNum.Name = "txtInvestorTradeNum";
      this.txtInvestorTradeNum.Size = new Size(117, 20);
      this.txtInvestorTradeNum.TabIndex = 12;
      this.txtInvestorTradeNum.TextChanged += new EventHandler(this.onLoanUpdatableFieldValueChanged);
      this.label5.AutoSize = true;
      this.label5.Location = new Point(9, 207);
      this.label5.Name = "label5";
      this.label5.Size = new Size(115, 14);
      this.label5.TabIndex = 5;
      this.label5.Text = "Investor Commitment #";
      this.txtInvestorCommitNum.Location = new Point(131, 204);
      this.txtInvestorCommitNum.MaxLength = 64;
      this.txtInvestorCommitNum.Name = "txtInvestorCommitNum";
      this.txtInvestorCommitNum.Size = new Size(139, 20);
      this.txtInvestorCommitNum.TabIndex = 13;
      this.txtInvestorCommitNum.TextChanged += new EventHandler(this.onLoanUpdatableFieldValueChanged);
      this.label4.AutoSize = true;
      this.label4.Location = new Point(9, 230);
      this.label4.Name = "label4";
      this.label4.Size = new Size(74, 14);
      this.label4.TabIndex = 4;
      this.label4.Text = "Trade Amount";
      this.txtAmount.Location = new Point(131, 227);
      this.txtAmount.MaxLength = 12;
      this.txtAmount.Name = "txtAmount";
      this.txtAmount.Size = new Size(139, 20);
      this.txtAmount.TabIndex = 14;
      this.txtAmount.TextAlign = HorizontalAlignment.Right;
      this.txtAmount.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.label3.AutoSize = true;
      this.label3.Location = new Point(9, 251);
      this.label3.Name = "label3";
      this.label3.Size = new Size(54, 14);
      this.label3.TabIndex = 3;
      this.label3.Text = "Tolerance";
      this.label10.AutoSize = true;
      this.label10.Location = new Point(194, 210);
      this.label10.Name = "label10";
      this.label10.Size = new Size(17, 14);
      this.label10.TabIndex = 10;
      this.label10.Text = "%";
      this.txtTolerance.Location = new Point(131, 248);
      this.txtTolerance.MaxLength = 6;
      this.txtTolerance.Name = "txtTolerance";
      this.txtTolerance.Size = new Size(59, 20);
      this.txtTolerance.TabIndex = 15;
      this.txtTolerance.TextAlign = HorizontalAlignment.Right;
      this.txtTolerance.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.collapsibleSplitter3.AnimationDelay = 20;
      this.collapsibleSplitter3.AnimationStep = 20;
      this.collapsibleSplitter3.BorderStyle3D = Border3DStyle.Flat;
      this.collapsibleSplitter3.ControlToHide = (Control) this.pnlSecurityTradeInfo;
      this.collapsibleSplitter3.Dock = DockStyle.Bottom;
      this.collapsibleSplitter3.ExpandParentForm = false;
      this.collapsibleSplitter3.Location = new Point(0, 325);
      this.collapsibleSplitter3.Name = "collapsibleSplitter3";
      this.collapsibleSplitter3.TabIndex = 1;
      this.collapsibleSplitter3.TabStop = false;
      this.collapsibleSplitter3.UseAnimations = false;
      this.collapsibleSplitter3.VisualStyle = VisualStyles.Encompass;
      this.pnlSecurityTradeInfo.Controls.Add((Control) this.securityTradeInfo);
      this.pnlSecurityTradeInfo.Dock = DockStyle.Bottom;
      this.pnlSecurityTradeInfo.Location = new Point(0, 328);
      this.pnlSecurityTradeInfo.Name = "pnlSecurityTradeInfo";
      this.pnlSecurityTradeInfo.Size = new Size(303, 291);
      this.pnlSecurityTradeInfo.TabIndex = 0;
      this.securityTradeInfo.BackColor = Color.WhiteSmoke;
      this.securityTradeInfo.Dock = DockStyle.Fill;
      this.securityTradeInfo.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.securityTradeInfo.Location = new Point(0, 0);
      this.securityTradeInfo.Name = "securityTradeInfo";
      this.securityTradeInfo.ReadOnly = false;
      this.securityTradeInfo.Size = new Size(303, 291);
      this.securityTradeInfo.TabIndex = 100;
      this.securityTradeInfo.TermMonthsUpdated += new EventHandler<TermMonthsUpdatedEventArgs>(this.securityTradeInfo_TermMonthsUpdated);
      this.tpPricing.Controls.Add((Control) this.pnlPricing);
      this.tpPricing.Location = new Point(4, 24);
      this.tpPricing.Name = "tpPricing";
      this.tpPricing.Padding = new Padding(0, 2, 2, 2);
      this.tpPricing.Size = new Size(1043, 623);
      this.tpPricing.TabIndex = 1;
      this.tpPricing.Tag = (object) "Pricing";
      this.tpPricing.Text = "Pricing";
      this.tpPricing.UseVisualStyleBackColor = true;
      this.tpPricing.Resize += new EventHandler(this.tpPricing_Resize);
      this.pnlPricing.Controls.Add((Control) this.msrPricingEditor1);
      this.pnlPricing.Controls.Add((Control) this.ctlAdvancedPricing);
      this.pnlPricing.Controls.Add((Control) this.vsPricingType);
      this.pnlPricing.Controls.Add((Control) this.cmbPricingType);
      this.pnlPricing.Controls.Add((Control) this.btnAdjustmentTemplate);
      this.pnlPricing.Controls.Add((Control) this.btnSRPTemplate);
      this.pnlPricing.Controls.Add((Control) this.ctlAdjustments);
      this.pnlPricing.Controls.Add((Control) this.ctlSRP);
      this.pnlPricing.Controls.Add((Control) this.grpBuyUpDown);
      this.pnlPricing.Controls.Add((Control) this.ctlPricing);
      this.pnlPricing.Dock = DockStyle.Fill;
      this.pnlPricing.Location = new Point(0, 2);
      this.pnlPricing.Name = "pnlPricing";
      this.pnlPricing.Size = new Size(1041, 619);
      this.pnlPricing.TabIndex = 3;
      this.msrPricingEditor1.Location = new Point(129, 307);
      this.msrPricingEditor1.Name = "msrPricingEditor1";
      this.msrPricingEditor1.PricingItems = (TradePricingItems) null;
      this.msrPricingEditor1.ReadOnly = false;
      this.msrPricingEditor1.Size = new Size(305, 241);
      this.msrPricingEditor1.TabIndex = 14;
      this.ctlAdvancedPricing.Location = new Point(249, 128);
      this.ctlAdvancedPricing.Name = "ctlAdvancedPricing";
      this.ctlAdvancedPricing.PricingInfo = (TradeAdvancedPricingInfo) null;
      this.ctlAdvancedPricing.ReadOnly = false;
      this.ctlAdvancedPricing.Size = new Size(198, 102);
      this.ctlAdvancedPricing.TabIndex = 13;
      this.vsPricingType.Location = new Point(192, 13);
      this.vsPricingType.MaximumSize = new Size(2, 16);
      this.vsPricingType.MinimumSize = new Size(2, 16);
      this.vsPricingType.Name = "vsPricingType";
      this.vsPricingType.Size = new Size(2, 16);
      this.vsPricingType.TabIndex = 12;
      this.vsPricingType.Text = "verticalSeparator1";
      this.cmbPricingType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbPricingType.FormattingEnabled = true;
      this.cmbPricingType.Items.AddRange(new object[2]
      {
        (object) "Simple",
        (object) "Advanced"
      });
      this.cmbPricingType.Location = new Point(90, 9);
      this.cmbPricingType.Margin = new Padding(3, 0, 3, 0);
      this.cmbPricingType.Name = "cmbPricingType";
      this.cmbPricingType.Size = new Size(74, 22);
      this.cmbPricingType.TabIndex = 11;
      this.cmbPricingType.SelectedIndexChanged += new EventHandler(this.cmbPricingType_SelectedIndexChanged);
      this.btnAdjustmentTemplate.BackColor = SystemColors.Control;
      this.btnAdjustmentTemplate.Location = new Point(316, 240);
      this.btnAdjustmentTemplate.Margin = new Padding(0);
      this.btnAdjustmentTemplate.Name = "btnAdjustmentTemplate";
      this.btnAdjustmentTemplate.Size = new Size(70, 22);
      this.btnAdjustmentTemplate.TabIndex = 1;
      this.btnAdjustmentTemplate.Text = "Template";
      this.btnAdjustmentTemplate.UseVisualStyleBackColor = true;
      this.btnAdjustmentTemplate.Click += new EventHandler(this.btnAdjustmentTemplate_Click);
      this.btnSRPTemplate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnSRPTemplate.BackColor = SystemColors.Control;
      this.btnSRPTemplate.Location = new Point(912, 8);
      this.btnSRPTemplate.Margin = new Padding(0);
      this.btnSRPTemplate.Name = "btnSRPTemplate";
      this.btnSRPTemplate.Size = new Size(70, 22);
      this.btnSRPTemplate.TabIndex = 1;
      this.btnSRPTemplate.Text = "Template";
      this.btnSRPTemplate.UseVisualStyleBackColor = true;
      this.btnSRPTemplate.Click += new EventHandler(this.btnSRPTemplate_Click);
      this.ctlAdjustments.AdjustmentfromPPE = false;
      this.ctlAdjustments.Adjustments = (TradePriceAdjustments) null;
      this.ctlAdjustments.Location = new Point(16, 236);
      this.ctlAdjustments.Name = "ctlAdjustments";
      this.ctlAdjustments.ReadOnly = false;
      this.ctlAdjustments.Size = new Size(439, 244);
      this.ctlAdjustments.TabIndex = 2;
      this.ctlSRP.Location = new Point(484, 8);
      this.ctlSRP.Name = "ctlSRP";
      this.ctlSRP.ReadOnly = false;
      this.ctlSRP.Size = new Size(485, 556);
      this.ctlSRP.SRPfromPPE = false;
      this.ctlSRP.SRPTable = (SRPTable) null;
      this.ctlSRP.TabIndex = 2;
      this.grpBuyUpDown.Controls.Add((Control) this.label29);
      this.grpBuyUpDown.Controls.Add((Control) this.label33);
      this.grpBuyUpDown.Controls.Add((Control) this.txtRateAdjustment);
      this.grpBuyUpDown.Controls.Add((Control) this.label32);
      this.grpBuyUpDown.Controls.Add((Control) this.label30);
      this.grpBuyUpDown.Controls.Add((Control) this.txtBuyDown);
      this.grpBuyUpDown.Controls.Add((Control) this.txtBuyUp);
      this.grpBuyUpDown.Controls.Add((Control) this.label31);
      this.grpBuyUpDown.HeaderForeColor = SystemColors.ControlText;
      this.grpBuyUpDown.Location = new Point(248, 8);
      this.grpBuyUpDown.Margin = new Padding(0);
      this.grpBuyUpDown.MinimumSize = new Size(199, 116);
      this.grpBuyUpDown.Name = "grpBuyUpDown";
      this.grpBuyUpDown.Size = new Size(199, 116);
      this.grpBuyUpDown.TabIndex = 10;
      this.grpBuyUpDown.Text = "Buy Up/Down";
      this.label29.AutoSize = true;
      this.label29.Location = new Point(8, 40);
      this.label29.Name = "label29";
      this.label29.Size = new Size(85, 14);
      this.label29.TabIndex = 2;
      this.label29.Text = "Rate Adjustment";
      this.label33.AutoSize = true;
      this.label33.Location = new Point(186, 84);
      this.label33.Name = "label33";
      this.label33.Size = new Size(17, 14);
      this.label33.TabIndex = 9;
      this.label33.Text = "%";
      this.txtRateAdjustment.Location = new Point(100, 36);
      this.txtRateAdjustment.MaxLength = 8;
      this.txtRateAdjustment.Name = "txtRateAdjustment";
      this.txtRateAdjustment.Size = new Size(86, 20);
      this.txtRateAdjustment.TabIndex = 1;
      this.txtRateAdjustment.TextAlign = HorizontalAlignment.Right;
      this.txtRateAdjustment.TextChanged += new EventHandler(this.onLoanUpdatableFieldValueChanged);
      this.label32.AutoSize = true;
      this.label32.Location = new Point(186, 63);
      this.label32.Name = "label32";
      this.label32.Size = new Size(17, 14);
      this.label32.TabIndex = 8;
      this.label32.Text = "%";
      this.label30.AutoSize = true;
      this.label30.Location = new Point(8, 62);
      this.label30.Name = "label30";
      this.label30.Size = new Size(42, 14);
      this.label30.TabIndex = 4;
      this.label30.Text = "Buy Up";
      this.txtBuyDown.Location = new Point(100, 80);
      this.txtBuyDown.MaxLength = 6;
      this.txtBuyDown.Name = "txtBuyDown";
      this.txtBuyDown.Size = new Size(86, 20);
      this.txtBuyDown.TabIndex = 3;
      this.txtBuyDown.TextAlign = HorizontalAlignment.Right;
      this.txtBuyDown.TextChanged += new EventHandler(this.onLoanUpdatableFieldValueChanged);
      this.txtBuyUp.Location = new Point(100, 58);
      this.txtBuyUp.MaxLength = 6;
      this.txtBuyUp.Name = "txtBuyUp";
      this.txtBuyUp.Size = new Size(86, 20);
      this.txtBuyUp.TabIndex = 2;
      this.txtBuyUp.TextAlign = HorizontalAlignment.Right;
      this.txtBuyUp.TextChanged += new EventHandler(this.onLoanUpdatableFieldValueChanged);
      this.label31.AutoSize = true;
      this.label31.Location = new Point(8, 84);
      this.label31.Name = "label31";
      this.label31.Size = new Size(58, 14);
      this.label31.TabIndex = 6;
      this.label31.Text = "Buy Down";
      this.ctlPricing.Location = new Point(12, 8);
      this.ctlPricing.Margin = new Padding(0, 0, 5, 0);
      this.ctlPricing.Name = "ctlPricing";
      this.ctlPricing.PricingItems = (TradePricingItems) null;
      this.ctlPricing.ReadOnly = false;
      this.ctlPricing.Size = new Size(228, 214);
      this.ctlPricing.TabIndex = 1;
      this.tpLoans.Location = new Point(4, 24);
      this.tpLoans.Name = "tpLoans";
      this.tpLoans.Padding = new Padding(0, 2, 2, 2);
      this.tpLoans.Size = new Size(1043, 623);
      this.tpLoans.TabIndex = 2;
      this.tpLoans.Tag = (object) "Loans";
      this.tpLoans.Text = "Loans";
      this.tpLoans.UseVisualStyleBackColor = true;
      this.tpLoans.Resize += new EventHandler(this.tpLoans_Resize);
      this.tpHistory.Controls.Add((Control) this.pnlHistory);
      this.tpHistory.Location = new Point(4, 24);
      this.tpHistory.Name = "tpHistory";
      this.tpHistory.Padding = new Padding(0, 2, 2, 2);
      this.tpHistory.Size = new Size(1043, 623);
      this.tpHistory.TabIndex = 4;
      this.tpHistory.Tag = (object) "History";
      this.tpHistory.Text = "Notes/History";
      this.tpHistory.UseVisualStyleBackColor = true;
      this.tpHistory.Resize += new EventHandler(this.tpHistory_Resize);
      this.pnlHistory.Controls.Add((Control) this.grpHistory);
      this.pnlHistory.Controls.Add((Control) this.grpNotes);
      this.pnlHistory.Dock = DockStyle.Fill;
      this.pnlHistory.Location = new Point(0, 2);
      this.pnlHistory.Name = "pnlHistory";
      this.pnlHistory.Size = new Size(1041, 619);
      this.pnlHistory.TabIndex = 4;
      this.grpHistory.Controls.Add((Control) this.btnExportHistory);
      this.grpHistory.Controls.Add((Control) this.gvHistory);
      this.grpHistory.HeaderForeColor = SystemColors.ControlText;
      this.grpHistory.Location = new Point(460, 0);
      this.grpHistory.Name = "grpHistory";
      this.grpHistory.Size = new Size(444, 388);
      this.grpHistory.TabIndex = 4;
      this.grpHistory.Text = "History";
      this.gvHistory.BorderStyle = BorderStyle.None;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Column1";
      gvColumn6.Text = "Event Time";
      gvColumn6.Width = 125;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "Column2";
      gvColumn7.SpringToFit = true;
      gvColumn7.Text = "Event";
      gvColumn7.Width = 192;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "Column3";
      gvColumn8.Text = "By";
      gvColumn8.Width = 125;
      this.gvHistory.Columns.AddRange(new GVColumn[3]
      {
        gvColumn6,
        gvColumn7,
        gvColumn8
      });
      this.gvHistory.Dock = DockStyle.Fill;
      this.gvHistory.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvHistory.Location = new Point(1, 26);
      this.gvHistory.Name = "gvHistory";
      this.gvHistory.Size = new Size(442, 361);
      this.gvHistory.TabIndex = 2;
      this.grpNotes.Controls.Add((Control) this.txtNotes);
      this.grpNotes.Controls.Add((Control) this.btnDateStamp);
      this.grpNotes.HeaderForeColor = SystemColors.ControlText;
      this.grpNotes.Location = new Point(0, 0);
      this.grpNotes.Name = "grpNotes";
      this.grpNotes.Size = new Size(452, 452);
      this.grpNotes.TabIndex = 3;
      this.grpNotes.Text = "Notes";
      this.txtNotes.BorderStyle = BorderStyle.None;
      this.txtNotes.Dock = DockStyle.Fill;
      this.txtNotes.Location = new Point(1, 26);
      this.txtNotes.Multiline = true;
      this.txtNotes.Name = "txtNotes";
      this.txtNotes.ScrollBars = ScrollBars.Both;
      this.txtNotes.Size = new Size(450, 425);
      this.txtNotes.TabIndex = 2;
      this.txtNotes.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.btnDateStamp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDateStamp.BackColor = SystemColors.Control;
      this.btnDateStamp.Location = new Point(358, 2);
      this.btnDateStamp.Name = "btnDateStamp";
      this.btnDateStamp.Size = new Size(89, 22);
      this.btnDateStamp.TabIndex = 1;
      this.btnDateStamp.Text = "&Date Stamp";
      this.btnDateStamp.UseVisualStyleBackColor = true;
      this.btnDateStamp.Click += new EventHandler(this.btnDateStamp_Click);
      this.gradientPanel1.BackColorGlassyStyle = true;
      this.gradientPanel1.Borders = AnchorStyles.Bottom;
      this.gradientPanel1.Controls.Add((Control) this.flowLayoutPanel2);
      this.gradientPanel1.Controls.Add((Control) this.flowLayoutPanel1);
      this.gradientPanel1.Dock = DockStyle.Top;
      this.gradientPanel1.GradientColor1 = Color.FromArgb(81, 123, 184);
      this.gradientPanel1.GradientColor2 = Color.FromArgb(167, 201, 239);
      this.gradientPanel1.Location = new Point(0, 0);
      this.gradientPanel1.Name = "gradientPanel1";
      this.gradientPanel1.Size = new Size(1053, 31);
      this.gradientPanel1.Style = GradientPanel.PanelStyle.PageHeader;
      this.gradientPanel1.TabIndex = 6;
      this.flowLayoutPanel2.AutoSize = true;
      this.flowLayoutPanel2.BackColor = Color.Transparent;
      this.flowLayoutPanel2.Controls.Add((Control) this.lblTradeName);
      this.flowLayoutPanel2.Controls.Add((Control) this.pictPending);
      this.flowLayoutPanel2.Location = new Point(10, 7);
      this.flowLayoutPanel2.Name = "flowLayoutPanel2";
      this.flowLayoutPanel2.Size = new Size(223, 22);
      this.flowLayoutPanel2.TabIndex = 7;
      this.lblTradeName.AutoSize = true;
      this.lblTradeName.BackColor = Color.Transparent;
      this.lblTradeName.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblTradeName.Location = new Point(3, 0);
      this.lblTradeName.Name = "lblTradeName";
      this.lblTradeName.Padding = new Padding(0, 3, 0, 0);
      this.lblTradeName.Size = new Size(84, 17);
      this.lblTradeName.TabIndex = 6;
      this.lblTradeName.Text = "<Trade Name>";
      this.pictPending.Image = (Image) componentResourceManager.GetObject("pictPending.Image");
      this.pictPending.Location = new Point(93, 3);
      this.pictPending.Name = "pictPending";
      this.pictPending.Size = new Size(16, 16);
      this.pictPending.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pictPending.TabIndex = 7;
      this.pictPending.TabStop = false;
      this.flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.flowLayoutPanel1.BackColor = Color.Transparent;
      this.flowLayoutPanel1.Controls.Add((Control) this.btnList);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnSave);
      this.flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel1.Location = new Point(436, 4);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new Size(611, 22);
      this.flowLayoutPanel1.TabIndex = 5;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.Controls.Add((Control) this.grpEditor);
      this.Controls.Add((Control) this.gradientPanel1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (TradeEditor);
      this.Size = new Size(1053, 684);
      ((ISupportInitialize) this.standardIconButton1).EndInit();
      ((ISupportInitialize) this.btnInvestorTemplate).EndInit();
      ((ISupportInitialize) this.btnExportHistory).EndInit();
      ((ISupportInitialize) this.btnList).EndInit();
      ((ISupportInitialize) this.btnSave).EndInit();
      this.grpEditor.ResumeLayout(false);
      this.tabTrade.ResumeLayout(false);
      this.tpDetails.ResumeLayout(false);
      this.pnlDetails.ResumeLayout(false);
      this.pnlRight.ResumeLayout(false);
      this.pnlFilter.ResumeLayout(false);
      this.pnlRightBottom.ResumeLayout(false);
      this.grpAddress.ResumeLayout(false);
      this.pnlPairOff.ResumeLayout(false);
      this.grpPairOffs.ResumeLayout(false);
      this.flpPairOffs.ResumeLayout(false);
      ((ISupportInitialize) this.btnEditPairOff).EndInit();
      this.pnlLeft.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.groupContainer1.ResumeLayout(false);
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      this.pnlSecurityTradeInfo.ResumeLayout(false);
      this.tpPricing.ResumeLayout(false);
      this.pnlPricing.ResumeLayout(false);
      this.grpBuyUpDown.ResumeLayout(false);
      this.grpBuyUpDown.PerformLayout();
      this.tpHistory.ResumeLayout(false);
      this.pnlHistory.ResumeLayout(false);
      this.grpHistory.ResumeLayout(false);
      this.grpNotes.ResumeLayout(false);
      this.grpNotes.PerformLayout();
      this.gradientPanel1.ResumeLayout(false);
      this.gradientPanel1.PerformLayout();
      this.flowLayoutPanel2.ResumeLayout(false);
      this.flowLayoutPanel2.PerformLayout();
      ((ISupportInitialize) this.pictPending).EndInit();
      this.flowLayoutPanel1.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
