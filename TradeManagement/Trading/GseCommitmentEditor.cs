// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.GseCommitmentEditor
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.UI.ReportFieldDefinitions;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Reporting;
using EllieMae.EMLite.Setup;
using EllieMae.EMLite.Trading.FannieMaePEMBS;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class GseCommitmentEditor : UserControl, IMenuProvider, ITradeEditor, ITradeEditorBase
  {
    private string className = nameof (GseCommitmentEditor);
    private const int ControlPadding = 5;
    private static GseCommitmentStatusEnumNameProvider tradeStatusNameProvider = new GseCommitmentStatusEnumNameProvider();
    private static string sw = Tracing.SwOutsideLoan;
    private string standardViewName = "Standard View";
    public static Color AlertColor = Color.FromArgb(204, 51, 51);
    public static Color HighlightColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 231);
    private GSECommitmentInfo gseCommitment;
    private LoanReportFieldDefs fieldDefs;
    private TradeAssignedLoanFieldDefs tradeAssLoanFieldDefs;
    private MbsPoolLoanAssignmentManager assignments;
    private bool loading;
    private bool modified;
    private bool readOnly;
    private bool loanUpdatesRequired;
    private bool poolPricingUpdateRequired;
    private GSECommitmentInfo lastPricingTradeInfo;
    private string originalTradeName;
    private string originalContractNum;
    private string originalPairOffAmt;
    private bool suspendEvents;
    private LoanListScreen ctlLoanList;
    private FannieMaeProductGrid productGrid;
    private FannieMaePEMbsPoolAssignmentListScreen ctlMbsPoolList;
    private PairOffControl pairOffControl;
    private IContainer components;
    private GradientPanel gradientPanel1;
    private Label lblTradeName;
    private FlowLayoutPanel flowLayoutPanel1;
    private StandardIconButton btnList;
    private StandardIconButton btnSave;
    private TabControl tabTrade;
    private TabPage tpDetails;
    private Panel pnlDetails;
    private Panel pnlRight;
    private Panel pnlFilter;
    private CollapsibleSplitter collapsibleSplitter2;
    private Panel pnlRightBottom;
    private Panel pnlMbsPools;
    private CollapsibleSplitter collapsibleSplitter1;
    private Panel pnlLeft;
    private Panel panel1;
    private GroupContainer groupContainer1;
    private Panel panel2;
    private Label label1;
    private TextBox txtCmtId;
    private Label label13;
    private Label label4;
    private TextBox txtAmount;
    private Label label3;
    private TabPage tpPricing;
    private Panel pnlPricing;
    private Button btnAdjustmentTemplate;
    private Button btnSRPTemplate;
    private PriceAdjustmentListEditor ctlAdjustments;
    private TabPage tpLoans;
    private TabPage tpHistory;
    private Panel pnlHistory;
    private GroupContainer grpHistory;
    private StandardIconButton btnExportHistory;
    private GridView gvHistory;
    private GroupContainer grpNotes;
    private TextBox txtNotes;
    private Button btnDateStamp;
    private GroupContainer grpProductNames;
    private GridView gvPNs;
    private StandardIconButton btnRemovePN;
    private StandardIconButton btnAddPN;
    private Label label6;
    private Label label5;
    private TextBox txtOutStandingBalance;
    private Label label22;
    private TextBox txtSellerNumber;
    private Label label14;
    private TextBox txtContractNumber;
    private ComboBox cmbTradeDesc;
    private Label label8;
    private TextBox txtMaxDeliveryAmnt;
    private Label label7;
    private TextBox txtMinDeliveryAmnt;
    private TextBox txtParticipationPercentage;
    private Label label11;
    private TextBox txtMaxBuyupAmount;
    private Label label16;
    private Label label17;
    private Label label18;
    private Label label19;
    private Label label21;
    private TextBox txtMaxRemainingAmnt;
    private Label label2;
    private TextBox txtMinRemainingAmnt;
    private TextBox txtRollFeeFactor;
    private Label label9;
    private TextBox txtPairOffFeeFactor;
    private Label label10;
    private Label label15;
    private TextBox txtPendingAmnt;
    private Label label20;
    private TextBox txtFulfilledAmnt;
    private Label label12;
    private ComboBox cboxServicingOption;
    private ComboBox cboxRemittanceCycle;
    private ComboBox cboxBuyUpDownGrid;
    private ToolTip toolTips;
    private MBSPoolBuyUpDownEditor ctlBuyUpDownEditor;
    private GroupContainer gcGuarantyFee;
    private Panel panel3;
    private GridView gvGuarantyFeePricing;
    private FlowLayoutPanel flowLayoutPanel3;
    private StandardIconButton siBtnDeleteGFee;
    private StandardIconButton siBtnAddGFee;
    private FieldLockButton btnPairedOffAmnt;
    private FieldLockButton btnOutstandingBalance;
    private Label label31;
    private ComboBox cboxBondType;
    private Label label30;
    private Label label29;
    private TextBox txtMinGfeeBuydown;
    private Label label28;
    private Label label27;
    private Label label26;
    private TextBox txtFalloutAmnt;
    private TextBox txtRolledFrom;
    private TextBox txtRolledTo;
    private TextBox txtRolledAmnt;
    private Label label25;
    private Label label24;
    private Label label23;
    private TextBox txtFees;
    private TextBox txtPairedOffAmnt;
    private DatePicker dtIssueMonth;
    private ComboBox txtRemittanceCycleMonth;
    private DatePicker dtCommitment;
    private Panel pnlProductSelectorContainer;
    private Panel pnlProductSelect;
    private ComboBox cmbProdct;
    private Label label32;
    private Panel panelPairOffEditor;

    public GseCommitmentEditor()
    {
      this.InitializeComponent();
      TextBoxFormatter.Attach(this.txtAmount, TextBoxContentRule.NonNegativeDecimal, "#,##0");
      TextBoxFormatter.Attach(this.txtFulfilledAmnt, TextBoxContentRule.NonNegativeDecimal, "#,##0.00");
      TextBoxFormatter.Attach(this.txtPendingAmnt, TextBoxContentRule.NonNegativeDecimal, "#,##0.00");
      TextBoxFormatter.Attach(this.txtFalloutAmnt, TextBoxContentRule.NonNegativeDecimal, "#,##0.00");
      TextBoxFormatter.Attach(this.txtRolledAmnt, TextBoxContentRule.NonNegativeDecimal, "#,##0.00");
      TextBoxFormatter.Attach(this.txtPairedOffAmnt, TextBoxContentRule.Decimal, "#,##0.00");
      TextBoxFormatter.Attach(this.txtMinRemainingAmnt, TextBoxContentRule.NonNegativeDecimal, "#,##0.00");
      TextBoxFormatter.Attach(this.txtMaxRemainingAmnt, TextBoxContentRule.NonNegativeDecimal, "#,##0.00");
      TextBoxFormatter.Attach(this.txtMinDeliveryAmnt, TextBoxContentRule.NonNegativeDecimal, "#,##0.00");
      TextBoxFormatter.Attach(this.txtMaxDeliveryAmnt, TextBoxContentRule.NonNegativeDecimal, "#,##0.00");
      TextBoxFormatter.Attach(this.txtFees, TextBoxContentRule.NonNegativeDecimal, "#,##0.00");
      TextBoxFormatter.Attach(this.txtMaxBuyupAmount, TextBoxContentRule.NonNegativeDecimal, "#,##0.00");
      TextBoxFormatter.Attach(this.txtMinGfeeBuydown, TextBoxContentRule.NonNegativeDecimal, "#,##0.00");
      TextBoxFormatter.Attach(this.txtParticipationPercentage, TextBoxContentRule.NonNegativeDecimal, "#,##0.0000000000");
      TextBoxFormatter.Attach(this.txtPairOffFeeFactor, TextBoxContentRule.NonNegativeDecimal, "#,##0.0000");
      TextBoxFormatter.Attach(this.txtRollFeeFactor, TextBoxContentRule.NonNegativeDecimal, "#,##0.0000");
      TextBoxFormatter.Attach(this.txtOutStandingBalance, TextBoxContentRule.NonNegativeDecimal, "#,##0.00");
      TextBoxFormatter.Attach(this.txtSellerNumber, TextBoxContentRule.NonNegativeInteger);
      this.addSearchButtonsToControls();
      this.refreshConfigurableFieldOptions();
      this.resetFieldDefs();
      this.ctlMbsPoolList = new FannieMaePEMbsPoolAssignmentListScreen((ITradeEditorBase) this);
      this.ctlMbsPoolList.ModifiedEvent += new EventHandler(this.CtlMbsPoolListOnModifiedEvent);
      this.pnlMbsPools.Controls.Clear();
      this.pnlMbsPools.Controls.Add((Control) this.ctlMbsPoolList);
      this.ctlMbsPoolList.Dock = DockStyle.Fill;
      this.ctlLoanList = new LoanListScreen((ITradeEditor) this);
      this.tpLoans.Controls.Clear();
      this.tpLoans.Controls.Add((Control) this.ctlLoanList);
      this.ctlLoanList.Dock = DockStyle.Fill;
      this.productGrid = new FannieMaeProductGrid();
      this.RemovePendingLoanFromOtherTrades = false;
      this.pairOffControl = new PairOffControl(PairOffType.GSECommitmentTrades);
      this.pairOffControl.DialogHeadTitle = "GSE Commitment Pair-Off";
      this.pairOffControl.DeleteItemText = "Commitment";
      this.pairOffControl.FieldDefs = this.fieldDefs;
      if (this.CurrentGseCommitmentInfo != null)
        this.pairOffControl.Locked = this.CurrentGseCommitmentInfo.Locked;
      this.pairOffControl.EditButtonClicked += new EventHandler(this.editButton_Clicked);
      this.pairOffControl.DeleteButtonClicked += new EventHandler(this.deleteButton_Clicked);
      this.pairOffControl.Dock = DockStyle.Fill;
      this.panelPairOffEditor.Controls.Clear();
      this.panelPairOffEditor.Controls.Add((Control) this.pairOffControl);
    }

    public TradeInfoObj CurrentTradeInfo
    {
      get => this.gseCommitment != null ? (TradeInfoObj) this.gseCommitment : (TradeInfoObj) null;
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

    public GSECommitmentInfo CurrentGseCommitmentInfo => this.gseCommitment;

    public Decimal TradeAmount
    {
      get
      {
        return !string.IsNullOrEmpty(this.txtAmount.Text) ? Utils.ParseDecimal((object) this.txtAmount.Text) : 0M;
      }
    }

    public bool RemovePendingLoanFromOtherTrades { get; set; }

    public TradeAssignmentByTradeBase[] FannieMaePEPoolAssignments { get; set; }

    public void RefreshData() => this.RefreshData(new GSECommitmentInfo());

    public void RefreshData(GSECommitmentInfo gseCommitment)
    {
      this.RefreshData(gseCommitment, (string[]) null);
    }

    public void RefreshData(GSECommitmentInfo gseCommitment, string[] loanGuids)
    {
      this.loading = false;
      this.modified = false;
      this.readOnly = false;
      this.loanUpdatesRequired = false;
      this.poolPricingUpdateRequired = false;
      this.ctlLoanList.ViewEligibleChecked = false;
      this.lastPricingTradeInfo = (GSECommitmentInfo) null;
      this.originalTradeName = (string) null;
      this.originalContractNum = (string) null;
      this.originalPairOffAmt = (string) null;
      this.assignments = (MbsPoolLoanAssignmentManager) null;
      this.gseCommitment = gseCommitment;
      this.gseCommitment.Filter = this.getCurrentFilter();
      this.loadTradeData();
      this.ctlLoanList.RefreshViews();
      if (this.gseCommitment.TradeID <= 0)
        this.tabTrade.SelectedTab = this.tpDetails;
      if (loanGuids != null)
      {
        this.ctlLoanList.PlaceAssignedLoans(loanGuids);
        if (this.gseCommitment.TradeID > 0)
          this.tabTrade.SelectedTab = this.tpLoans;
      }
      this.loanUpdatesRequired = false;
      this.poolPricingUpdateRequired = false;
    }

    private void CtlMbsPoolListOnModifiedEvent(object sender, EventArgs eventArgs)
    {
      if (((IEnumerable<TradeAssignmentByTradeBase>) this.ctlMbsPoolList.GetCurrentAssignments()).Count<TradeAssignmentByTradeBase>() < ((IEnumerable<TradeAssignmentByTradeBase>) this.FannieMaePEPoolAssignments).Count<TradeAssignmentByTradeBase>())
        this.onLoanUpdatableFieldValueChanged(sender, eventArgs);
      this.recalculateProfitability();
    }

    private void loanLayoutMngr_LayoutChanged(object sender, EventArgs e) => this.loadTradeData();

    private void resetFieldDefs()
    {
      this.fieldDefs = LoanReportFieldDefs.GetFieldDefs(Session.DefaultInstance, LoanReportFieldFlags.DatabaseFieldsNoAudit);
      this.tradeAssLoanFieldDefs = TradeAssignedLoanFieldDefs.GetFieldDefs();
    }

    public bool DataModified
    {
      get
      {
        if (this.readOnly)
          return false;
        return this.modified || this.ctlAdjustments.DataModified || this.ctlMbsPoolList.DataModified || this.ctlBuyUpDownEditor.DataModified;
      }
    }

    public bool LoanUpdatesRequired
    {
      get
      {
        if (this.readOnly || ((IEnumerable<MbsPoolLoanAssignment>) this.assignments.GetPendingAndAssignedLoans()).Count<MbsPoolLoanAssignment>() == 0)
          return false;
        return this.loanUpdatesRequired || this.PoolPricingUpdateRequired;
      }
    }

    public bool PoolPricingUpdateRequired => this.poolPricingUpdateRequired;

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
      this.txtCmtId.ReadOnly = this.readOnly;
      this.txtAmount.ReadOnly = this.readOnly;
      this.txtContractNumber.ReadOnly = this.readOnly;
      this.txtFalloutAmnt.ReadOnly = this.readOnly;
      this.txtFees.ReadOnly = this.readOnly;
      this.txtFulfilledAmnt.ReadOnly = this.readOnly;
      this.txtMaxBuyupAmount.ReadOnly = this.readOnly;
      this.txtMaxDeliveryAmnt.ReadOnly = this.readOnly;
      this.txtMaxRemainingAmnt.ReadOnly = this.readOnly;
      this.txtMinDeliveryAmnt.ReadOnly = this.readOnly;
      this.txtMinGfeeBuydown.ReadOnly = this.readOnly;
      this.txtMinRemainingAmnt.ReadOnly = this.readOnly;
      this.txtNotes.ReadOnly = this.readOnly;
      this.txtOutStandingBalance.ReadOnly = this.readOnly;
      this.txtPairedOffAmnt.ReadOnly = this.readOnly;
      this.txtPairOffFeeFactor.ReadOnly = this.readOnly;
      this.txtParticipationPercentage.ReadOnly = this.ReadOnly;
      this.txtPendingAmnt.ReadOnly = this.readOnly;
      this.txtRemittanceCycleMonth.Enabled = !this.readOnly;
      this.txtRolledAmnt.ReadOnly = this.readOnly;
      this.txtRolledFrom.ReadOnly = this.readOnly;
      this.txtRolledTo.ReadOnly = this.readOnly;
      this.txtRollFeeFactor.ReadOnly = this.readOnly;
      this.txtSellerNumber.ReadOnly = this.ReadOnly;
      this.cmbTradeDesc.Enabled = !this.readOnly;
      this.cmbProdct.Enabled = !this.readOnly;
      this.ctlMbsPoolList.Enabled = !this.readOnly;
      this.dtCommitment.Enabled = !this.readOnly;
      this.dtIssueMonth.Enabled = !this.readOnly;
      this.cboxBondType.Enabled = !this.readOnly;
      this.cboxBuyUpDownGrid.Enabled = !this.readOnly;
      this.cboxRemittanceCycle.Enabled = !this.readOnly;
      this.cboxServicingOption.Enabled = !this.readOnly;
      this.btnAddPN.Enabled = !this.readOnly;
      this.btnRemovePN.Enabled = !this.readOnly;
      this.pairOffControl.ReadOnly = this.readOnly;
      this.ctlAdjustments.ReadOnly = this.readOnly;
      this.btnAdjustmentTemplate.Visible = !this.readOnly;
      this.siBtnAddGFee.Enabled = !this.readOnly;
      this.siBtnDeleteGFee.Enabled = !this.readOnly;
      this.ctlBuyUpDownEditor.ReadOnly = !this.readOnly;
      this.ctlLoanList.ReadOnly = this.readOnly;
      this.txtNotes.ReadOnly = this.readOnly;
      this.btnDateStamp.Visible = !this.readOnly;
      this.btnSave.Enabled = !this.readOnly;
      this.pairOffControl.ReadOnly = this.readOnly;
    }

    private static System.Type getSortTypeForFieldDef(LoanReportFieldDef fieldDef)
    {
      if (fieldDef.FieldType == EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsDate)
        return typeof (ListViewDateSort);
      return fieldDef.FieldType == EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsNumeric ? typeof (ListViewCurrencySort) : typeof (ListViewTextCaseInsensitiveSort);
    }

    public void RemoveLoanFromTrade(string guid, bool rejected)
    {
      throw new NotImplementedException();
    }

    public string[] GetLoanToTradeAssignmentAllLoanGuids() => (string[]) null;

    public List<LoanToTradeAssignmentBase> GetLoanToTradeAssignments()
    {
      return new List<LoanToTradeAssignmentBase>();
    }

    public bool ValidateLoanToTradeAssignment(
      LoanToTradeAssignmentBase assignment,
      out string errMsg)
    {
      errMsg = string.Empty;
      return true;
    }

    public PipelineInfo[] GetLoanToTradeAssignedPipelineData() => (PipelineInfo[]) null;

    public int GetLoanToTradePendingAssignmentCount()
    {
      int pendingAssignmentCount = 0;
      if (this.assignments == null)
        return 0;
      foreach (MbsPoolLoanAssignment pendingLoan in this.assignments.GetPendingLoans())
      {
        if (pendingLoan.PendingStatus == MbsPoolLoanStatus.Assigned)
          ++pendingAssignmentCount;
      }
      return pendingAssignmentCount;
    }

    public int GetLoanToTradePendingShipmentCount()
    {
      int pendingShipmentCount = 0;
      if (this.assignments == null)
        return 0;
      foreach (MbsPoolLoanAssignment pendingLoan in this.assignments.GetPendingLoans())
      {
        if (pendingLoan.PendingStatus == MbsPoolLoanStatus.Shipped)
          ++pendingShipmentCount;
      }
      return pendingShipmentCount;
    }

    public int GetLoanToTradePendingPurchaseCount()
    {
      int pendingPurchaseCount = 0;
      if (this.assignments == null)
        return 0;
      foreach (MbsPoolLoanAssignment pendingLoan in this.assignments.GetPendingLoans())
      {
        if (pendingLoan.PendingStatus == MbsPoolLoanStatus.Purchased)
          ++pendingPurchaseCount;
      }
      return pendingPurchaseCount;
    }

    public int GetLoanToTradePendingRemovalCount()
    {
      int pendingRemovalCount = 0;
      if (this.assignments == null)
        return 0;
      foreach (MbsPoolLoanAssignment pendingLoan in this.assignments.GetPendingLoans())
      {
        if (pendingLoan.PendingStatus == MbsPoolLoanStatus.Unassigned)
          ++pendingRemovalCount;
      }
      return pendingRemovalCount;
    }

    public string[] GetLoanToTradeAssignedAndRejectedLoanGuids() => (string[]) null;

    public string[] GetLoanToTradeAssignedAndRejectedLoanNumbers() => (string[]) null;

    public void MarkLoanToTradeAssignmentStatusToShipped(string loanGuid)
    {
    }

    public void MarkLoanToTradeAssignmentStatusToPurchasedPending(string loanGuid)
    {
    }

    public void CommitLoanToTradeAssignments(bool forceUpdateOfAllLoans)
    {
      throw new NotImplementedException();
    }

    public bool IsLoanToTradeAssignmentPending(LoanToTradeAssignmentBase assignment) => false;

    public List<LoanToTradeAssignmentBase> GetLoanToTradeAssignedLoans()
    {
      List<LoanToTradeAssignmentBase> tradeAssignedLoans = new List<LoanToTradeAssignmentBase>();
      if (this.assignments == null)
        return tradeAssignedLoans;
      foreach (MbsPoolLoanAssignment assignedLoan in this.assignments.GetAssignedLoans())
        tradeAssignedLoans.Add((LoanToTradeAssignmentBase) assignedLoan);
      return tradeAssignedLoans;
    }

    private void resetOriginalTradeData()
    {
      this.originalTradeName = this.gseCommitment.Name.Trim();
      this.originalContractNum = this.gseCommitment.ContractNumber == null ? (string) null : this.gseCommitment.ContractNumber.Trim();
    }

    public bool IsNoteRateAllowed(PipelineInfo pinfo) => false;

    public string[] GetPricingAndEligibilityFields()
    {
      return this.gseCommitment.GetPricingAndEligibilityFields();
    }

    public void LoadTradeData() => this.loadTradeData();

    public string GetLoanStatusDescription(object value) => "";

    public Decimal CalculatePriceIndex(PipelineInfo info) => 0M;

    public Decimal CalculatePriceIndex(PipelineInfo info, Decimal securityPrice) => 0M;

    public ICursor GetEligibleLoanCursor(
      string[] fields,
      PipelineData dataToInclude,
      SortField[] sortFields,
      string[] excludedGuids,
      EligibleLoanFilterOption filterOption = EligibleLoanFilterOption.All)
    {
      throw new NotImplementedException();
    }

    public List<TradeLoanUpdateError> AssignLoanToTrade(PipelineInfo[] pinfos)
    {
      throw new NotImplementedException();
    }

    public void RefreshLoans()
    {
      if (this.gseCommitment == null)
        return;
      this.loading = true;
      this.loadLoans(Session.GseCommitmentManager.GetTradeEditorScreenData(this.gseCommitment.TradeID, this.ctlLoanList.GetAssignedLoanListFields(), false).AssignedLoans, false);
      this.loadProfitabilityData();
    }

    private void loadTradeData()
    {
      if (this.gseCommitment == null)
        return;
      this.loading = true;
      GseCommitmentEditorScreenData editorScreenData = Session.GseCommitmentManager.GetTradeEditorScreenData(this.gseCommitment.TradeID, this.ctlLoanList.GetAssignedLoanListFields(), false);
      this.loadBasicDetails();
      this.loadGuarantyFee();
      this.ctlAdjustments.Adjustments = this.gseCommitment.PriceAdjustments;
      this.LoadPricingData();
      this.ctlBuyUpDownEditor.BuyUpDownModified += new EventHandler(this.BuyUpDown_OnModified);
      this.loadTradeAssignments();
      this.loadProducts();
      this.loadPairOffs();
      this.loadHistory(editorScreenData.TradeHistory);
      this.loadLoans(editorScreenData.AssignedLoans);
      this.resetOriginalTradeData();
      this.ReadOnly = this.gseCommitment.Status == TradeStatus.Archived;
      this.loading = false;
      this.modified = false;
      this.loanUpdatesRequired = false;
      this.poolPricingUpdateRequired = false;
      if (!this.gseCommitment.IsCloned)
        return;
      this.modified = this.gseCommitment.TradeID <= 0;
    }

    private void loadBasicDetails()
    {
      this.lblTradeName.Text = this.gseCommitment.TradeID > 0 ? "GSE Commitment " + this.gseCommitment.Name : "New Commitment";
      this.txtCmtId.Text = this.gseCommitment.Name;
      this.txtContractNumber.Text = this.gseCommitment.ContractNumber;
      this.cmbTradeDesc.Text = this.gseCommitment.TradeDescription;
      this.dtCommitment.Value = this.gseCommitment.CommitmentDate;
      this.txtSellerNumber.Text = this.gseCommitment.SellerNumber;
      this.txtAmount.Text = this.gseCommitment.TradeAmount.ToString("#,##0;;\"\"");
      this.txtOutStandingBalance.Text = this.gseCommitment.OutstandingBalance.ToString("#,##0.00;;\"\"");
      this.dtIssueMonth.Text = this.gseCommitment.IssueMonth;
      this.txtMinDeliveryAmnt.Text = this.gseCommitment.MinDeliveryAmount.ToString("#,##0.00;;\"\"");
      this.txtMaxDeliveryAmnt.Text = this.gseCommitment.MaxDeliveryAmount.ToString("#,##0.00;;\"\"");
      this.txtFulfilledAmnt.Text = this.gseCommitment.FulfilledAmount.ToString("#,##0.00;;\"\"");
      this.txtPendingAmnt.Text = this.gseCommitment.PendingAmount.ToString("#,##0.00;;\"\"");
      this.txtMinRemainingAmnt.Text = this.gseCommitment.MinRemainingAmount.ToString("#,##0.00;;\"\"");
      this.txtMaxRemainingAmnt.Text = this.gseCommitment.MaxRemainingAmount.ToString("#,##0.00;;\"\"");
      this.txtPairedOffAmnt.Text = this.gseCommitment.PairOffAmount.ToString("#,##0.00;;\"\"");
      this.txtFees.Text = this.gseCommitment.Fees.ToString("#,##0.00;;\"\"");
      this.txtPairOffFeeFactor.Text = this.gseCommitment.PairOffFeeFactor.ToString("#,##0.0000;;\"\"");
      this.txtRolledAmnt.Text = this.gseCommitment.RolledAmount.ToString("#,##0.00;;\"\"");
      this.txtRollFeeFactor.Text = this.gseCommitment.RollFeeFactor.ToString("#,##0.0000;;\"\"");
      this.txtRolledTo.Text = this.gseCommitment.RolledTo;
      this.txtRolledFrom.Text = this.gseCommitment.RolledFrom;
      this.txtFalloutAmnt.Text = this.gseCommitment.FalloutAmount.ToString("#,##0.00;;\"\"");
      this.btnOutstandingBalance.Locked = this.gseCommitment.OutstandingBalanceLock;
      this.txtOutStandingBalance.Enabled = this.gseCommitment.OutstandingBalanceLock;
      this.loadOutstandingBalance();
      this.btnPairedOffAmnt.Locked = this.gseCommitment.PairOffAmountLock;
      this.txtPairedOffAmnt.Enabled = this.gseCommitment.PairOffAmountLock;
      this.loadProfitabilityData();
      if (string.IsNullOrEmpty(this.gseCommitment.RemittanceCycle))
        this.cboxRemittanceCycle.SelectedIndex = -1;
      else
        this.cboxRemittanceCycle.Text = this.gseCommitment.RemittanceCycle;
      ComboBox remittanceCycleMonth1 = this.txtRemittanceCycleMonth;
      Decimal num = this.gseCommitment.RemittanceDayOfMonth;
      string str1 = num.ToString("#,##0;;\"\"");
      remittanceCycleMonth1.Text = str1;
      if (this.gseCommitment.RemittanceDayOfMonth <= 0M)
        this.txtRemittanceCycleMonth.SelectedIndex = -1;
      else
        this.txtRemittanceCycleMonth.SelectedIndex = (int) Convert.ToInt16(this.gseCommitment.RemittanceDayOfMonth) - 1;
      ComboBox remittanceCycleMonth2 = this.txtRemittanceCycleMonth;
      num = this.gseCommitment.RemittanceDayOfMonth;
      string str2 = num.ToString("#,##0;;\"\"");
      remittanceCycleMonth2.Text = str2;
      if (string.IsNullOrEmpty(this.gseCommitment.ServicingOption))
        this.cboxServicingOption.SelectedIndex = -1;
      else
        this.cboxServicingOption.Text = this.gseCommitment.ServicingOption;
      if (string.IsNullOrEmpty(this.gseCommitment.BondType))
        this.cboxBondType.SelectedIndex = -1;
      else
        this.cboxBondType.Text = this.gseCommitment.BondType;
      TextBox participationPercentage = this.txtParticipationPercentage;
      num = this.gseCommitment.ParticipationPercent;
      string str3 = num.ToString("#,##0.0000000000;;\"\"");
      participationPercentage.Text = str3;
      if (string.IsNullOrEmpty(this.gseCommitment.BuyUpBuyDownGrid))
        this.cboxBuyUpDownGrid.SelectedIndex = -1;
      else
        this.cboxBuyUpDownGrid.Text = this.gseCommitment.BuyUpBuyDownGrid;
      TextBox txtMaxBuyupAmount = this.txtMaxBuyupAmount;
      num = this.gseCommitment.MaxBuyupAmount;
      string str4 = num.ToString("#,##0.00;;\"\"");
      txtMaxBuyupAmount.Text = str4;
      TextBox txtMinGfeeBuydown = this.txtMinGfeeBuydown;
      num = this.gseCommitment.MinGFeeAfterBuydown;
      string str5 = num.ToString("#,##0.00;;\"\"");
      txtMinGfeeBuydown.Text = str5;
      this.txtNotes.Text = this.gseCommitment.Notes;
    }

    private void loadLoans(PipelineInfo[] pinfos, bool updateLoansTab = true)
    {
      if (pinfos == null)
        pinfos = new PipelineInfo[0];
      this.assignments = new MbsPoolLoanAssignmentManager(Session.SessionObjects, this.gseCommitment.TradeID, pinfos);
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

    private void displayItemAlert(GVItem item, int subitemIndex)
    {
      item.SubItems[subitemIndex].Font = new Font(this.Font, FontStyle.Bold);
      item.SubItems[subitemIndex].ForeColor = TradeEditor.AlertColor;
    }

    public string GetTradeStatusDescription(LoanToTradeAssignmentBase assignmentInfo)
    {
      MbsPoolLoanAssignment poolLoanAssignment = (MbsPoolLoanAssignment) assignmentInfo;
      string name = GseCommitmentEditor.tradeStatusNameProvider.GetName((object) poolLoanAssignment.Status);
      if (poolLoanAssignment.Pending)
        name += " - Pending";
      return name;
    }

    private void loadHistory(GseCommitmentHistoryItem[] historyItems)
    {
      this.btnExportHistory.Enabled = false;
      this.gvHistory.Items.Clear();
      if (historyItems == null)
        return;
      foreach (GseCommitmentHistoryItem historyItem in historyItems)
        this.gvHistory.Items.Add(this.createTradeHistoryListItem(historyItem));
      this.btnExportHistory.Enabled = this.gvHistory.Items.Count > 0;
    }

    private GVItem createTradeHistoryListItem(GseCommitmentHistoryItem historyItem)
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
      List<CommonPairOff> commonPairOffList = new List<CommonPairOff>();
      for (int index = 0; index < this.gseCommitment.GSECommitmentPairOffs.Count; ++index)
        commonPairOffList.Add((CommonPairOff) this.gseCommitment.GSECommitmentPairOffs[index]);
      this.pairOffControl.LoadPairOffs(commonPairOffList.ToArray());
    }

    private void loadProducts()
    {
      this.gvPNs.Items.Clear();
      if (this.gseCommitment.ProductNames != null && this.gseCommitment.ProductNames.Count > 0)
      {
        for (int index = 0; index < this.gseCommitment.ProductNames.Count; ++index)
        {
          FannieMaeProduct productName = this.gseCommitment.ProductNames[index];
          this.gvPNs.Items.Add(new GVItem()
          {
            SubItems = {
              (object) productName.ProductName,
              (object) productName.DisplayName,
              (object) productName.ProductDescription
            },
            Tag = (object) productName
          });
        }
      }
      this.cmbProdct.Items.Clear();
      this.cmbProdct.Items.AddRange((object[]) this.gvPNs.Items.Select<GVItem, string>((System.Func<GVItem, string>) (p => ((FannieMaeProduct) p.Tag).ProductName)).ToArray<string>());
      if (this.cmbProdct.Items.Count > 0)
        this.cmbProdct.SelectedIndex = 0;
      else
        this.ctlBuyUpDownEditor.ReadOnly = true;
    }

    private void loadGuarantyFee()
    {
      this.gvGuarantyFeePricing.Items.Clear();
      if (this.gseCommitment.GuarantyFees == null)
        return;
      foreach (GuarantyFeeItem guarantyFee in this.gseCommitment.GuarantyFees)
        this.gvGuarantyFeePricing.Items.Add(this.CreateGVGuaranteeFeeItem(guarantyFee));
    }

    private void loadProfitabilityData()
    {
      if (this.btnPairedOffAmnt.Locked)
        return;
      this.txtPairedOffAmnt.Text = this.gseCommitment.GetTotalPairOffAmount().ToString("#,##0.00;;\"\"");
    }

    private void LoadPricingData()
    {
      this.ctlAdjustments.Adjustments = this.gseCommitment.PriceAdjustments;
      this.ctlBuyUpDownEditor.BuyUpDownItems = new MbsPoolBuyUpDownItems();
    }

    private void BuyUpDown_OnModified(object sender, EventArgs e)
    {
      this.cmbProdct_SelectedIndexChanged((object) null, (EventArgs) null);
    }

    private void loadTradeAssignments()
    {
      if (this.gseCommitment == null)
        return;
      this.FannieMaePEPoolAssignments = MbsPoolAssignmentListScreen.GetAssigmentsByGseCommitment(this.gseCommitment.TradeID);
      this.ctlMbsPoolList.RefreshData(this.FannieMaePEPoolAssignments, MbsPoolAssignmentListScreen.GetUnassignedAssigmentsByGseCommitment(this.gseCommitment.TradeID));
      this.loadOutstandingBalance();
    }

    private void addSearchButtonsToControls()
    {
      this.btnAdjustmentTemplate.Parent = (Control) null;
      this.ctlAdjustments.AddControlToHeader((Control) new VerticalSeparator());
      this.ctlAdjustments.AddControlToHeader((Control) this.btnAdjustmentTemplate);
    }

    public void CommitChanges() => this.commitChanges();

    private void commitChanges()
    {
      this.modified = this.DataModified;
      this.loanUpdatesRequired = this.LoanUpdatesRequired;
      if (this.ctlAdjustments.DataModified || this.ctlBuyUpDownEditor.DataModified)
        this.poolPricingUpdateRequired = true;
      this.gseCommitment.Name = this.txtCmtId.Text.Trim();
      this.gseCommitment.ContractNumber = this.txtContractNumber.Text.Trim();
      this.gseCommitment.TradeDescription = this.cmbTradeDesc.Text;
      this.gseCommitment.CommitmentDate = this.dtCommitment.Value;
      this.gseCommitment.SellerNumber = this.txtSellerNumber.Text.Trim();
      this.gseCommitment.TradeAmount = Utils.ParseDecimal((object) this.txtAmount.Text);
      this.gseCommitment.OutstandingBalance = Utils.ParseDecimal((object) this.txtOutStandingBalance.Text);
      this.gseCommitment.IssueMonth = this.dtIssueMonth.Text;
      this.gseCommitment.MinDeliveryAmount = Utils.ParseDecimal((object) this.txtMinDeliveryAmnt.Text.Trim());
      this.gseCommitment.MaxDeliveryAmount = Utils.ParseDecimal((object) this.txtMaxDeliveryAmnt.Text.Trim());
      this.gseCommitment.FulfilledAmount = Utils.ParseDecimal((object) this.txtFulfilledAmnt.Text.Trim());
      this.gseCommitment.PendingAmount = Utils.ParseDecimal((object) this.txtPendingAmnt.Text.Trim());
      this.gseCommitment.MinRemainingAmount = Utils.ParseDecimal((object) this.txtMinRemainingAmnt.Text.Trim());
      this.gseCommitment.MaxRemainingAmount = Utils.ParseDecimal((object) this.txtMaxRemainingAmnt.Text.Trim());
      this.gseCommitment.PairOffAmount = Utils.ParseDecimal((object) this.txtPairedOffAmnt.Text.Trim());
      this.gseCommitment.Fees = Utils.ParseDecimal((object) this.txtFees.Text.Trim());
      this.gseCommitment.PairOffFeeFactor = Utils.ParseDecimal((object) this.txtPairOffFeeFactor.Text.Trim());
      this.gseCommitment.RolledAmount = Utils.ParseDecimal((object) this.txtRolledAmnt.Text.Trim());
      this.gseCommitment.RollFeeFactor = Utils.ParseDecimal((object) this.txtRollFeeFactor.Text.Trim());
      this.gseCommitment.RolledTo = this.txtRolledTo.Text.Trim();
      this.gseCommitment.RolledFrom = this.txtRolledFrom.Text.Trim();
      this.gseCommitment.FalloutAmount = Utils.ParseDecimal((object) this.txtFalloutAmnt.Text.Trim());
      this.gseCommitment.RemittanceCycle = this.cboxRemittanceCycle.Text;
      this.gseCommitment.RemittanceDayOfMonth = Utils.ParseDecimal((object) this.txtRemittanceCycleMonth.Text.Trim());
      this.gseCommitment.ServicingOption = this.cboxServicingOption.Text;
      this.gseCommitment.BondType = this.cboxBondType.Text;
      this.gseCommitment.ParticipationPercent = Utils.ParseDecimal((object) this.txtParticipationPercentage.Text.Trim());
      this.gseCommitment.BuyUpBuyDownGrid = this.cboxBuyUpDownGrid.Text;
      this.gseCommitment.MaxBuyupAmount = Utils.ParseDecimal((object) this.txtMaxBuyupAmount.Text.Trim());
      this.gseCommitment.MinGFeeAfterBuydown = Utils.ParseDecimal((object) this.txtMinGfeeBuydown.Text.Trim());
      this.gseCommitment.OutstandingBalanceLock = this.btnOutstandingBalance.Locked;
      this.gseCommitment.PairOffAmountLock = this.btnPairedOffAmnt.Locked;
      this.ctlAdjustments.CommitChanges();
      GuarantyFeeItems guarantyFeeItems = new GuarantyFeeItems();
      this.gseCommitment.GuarantyFees = new GuarantyFeeItems();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvGuarantyFeePricing.Items)
        guarantyFeeItems.Add(gvItem.Tag as GuarantyFeeItem, false);
      this.gseCommitment.GuarantyFees = guarantyFeeItems;
      FannieMaeProducts fannieMaeProducts = new FannieMaeProducts();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvPNs.Items)
      {
        FannieMaeProduct tag = (FannieMaeProduct) gvItem.Tag;
        fannieMaeProducts.Add(tag);
      }
      this.gseCommitment.ProductNames = fannieMaeProducts;
      this.gseCommitment.Notes = this.txtNotes.Text;
      this.ctlBuyUpDownEditor.CommitChanges();
      this.FannieMaePEPoolAssignments = this.ctlMbsPoolList.GetCurrentAssignments();
      this.ctlLoanList.DataModified = false;
    }

    public bool PreValidateCommit() => this.prevalidateCommit();

    private bool prevalidateCommit()
    {
      return this.validateTradeData() && this.ctlMbsPoolList.ValidateChanges() && this.ctlBuyUpDownEditor.ValidateBuyUpDown();
    }

    private void btnSave_Click(object sender, EventArgs e) => this.SaveTrade();

    public bool SaveTrade() => this.SaveTrade(false, false);

    public bool SaveTrade(bool forceUpdateOfLoans, bool updatedSelectedLoans)
    {
      if (this.readOnly)
        return true;
      Cursor.Current = Cursors.WaitCursor;
      try
      {
        if (!this.prevalidateCommit())
          return false;
        this.commitChanges();
        if (!this.validateTradeData())
          return false;
        this.saveTradeInfo();
        bool flag = true;
        List<MbsPoolInfo> pools = new List<MbsPoolInfo>();
        if (this.PoolPricingUpdateRequired && ((IEnumerable<TradeAssignmentByTradeBase>) this.FannieMaePEPoolAssignments).Count<TradeAssignmentByTradeBase>() > 0)
        {
          foreach (int? nullable in ((IEnumerable<TradeAssignmentByTradeBase>) this.FannieMaePEPoolAssignments).Select<TradeAssignmentByTradeBase, int?>((System.Func<TradeAssignmentByTradeBase, int?>) (p => p.TradeID)).Distinct<int?>())
          {
            if (nullable.HasValue)
            {
              MbsPoolInfo trade = Session.SessionObjects.MbsPoolManager.GetTrade(nullable.Value);
              pools.Add(trade);
              trade.CalcAllPricingDetails(new List<GSECommitmentInfo>()
              {
                this.gseCommitment
              });
              Session.MbsPoolManager.UpdateTrade(trade, false, true);
            }
          }
        }
        if (forceUpdateOfLoans)
          flag = this.commitTradeAssignments(true, pools);
        else if (this.LoanUpdatesRequired && Utils.Dialog((IWin32Window) this, "The GSE Commitment has been saved successfully." + Environment.NewLine + "Would you like to update the loan files with these recent changes?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
          flag = this.commitTradeAssignments(false, pools);
        this.loadTradeData();
        return flag;
      }
      catch (ObjectNotFoundException ex)
      {
        Tracing.Log(GseCommitmentEditor.sw, this.className, TraceLevel.Error, ex.ToString());
        if (ex.ObjectType == ObjectType.Trade)
        {
          int num1 = (int) Utils.Dialog((IWin32Window) this, "The current commitment has been deleted and cannot be saved. All changes made to this commitment will be lost.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "The commitment could not be saved due to the following error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        this.ReadOnly = true;
        this.modified = false;
        this.ctlLoanList.DataModified = false;
        this.loanUpdatesRequired = false;
        this.poolPricingUpdateRequired = false;
        return false;
      }
      catch (Exception ex)
      {
        Tracing.Log(GseCommitmentEditor.sw, this.className, TraceLevel.Error, ex.ToString());
        if (ex.Message.Contains("The loan has been assigned to another trade or pool."))
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "The commitment is saved successfully. However, the loan is not assigned to the commitment, because the loan has been assigned to another trade or pool.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          this.loadTradeData();
        }
        else
        {
          int num3 = (int) Utils.Dialog((IWin32Window) this, "The commitment could not be saved due to the following error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        return false;
      }
      finally
      {
        Cursor.Current = Cursors.Default;
      }
    }

    private void saveTradeInfo()
    {
      int tradeId = this.gseCommitment.TradeID;
      if (tradeId < 0)
        tradeId = Session.GseCommitmentManager.CreateTrade(this.gseCommitment);
      else
        Session.GseCommitmentManager.UpdateTrade(this.gseCommitment, false);
      this.gseCommitment = Session.GseCommitmentManager.GetTrade(tradeId);
      this.gseCommitment.Filter = this.getCurrentFilter();
      this.ctlMbsPoolList.Save();
    }

    private bool commitTradeAssignments(bool forceUpdateOfAllLoans, List<MbsPoolInfo> pools)
    {
      try
      {
        MbsPoolLoanAssignment[] andAssignedLoans1 = this.assignments.GetPendingAndAssignedLoans();
        if (andAssignedLoans1 == null || ((IEnumerable<MbsPoolLoanAssignment>) andAssignedLoans1).Count<MbsPoolLoanAssignment>() == 0)
          return true;
        Dictionary<int, MbsPoolLoanAssignment[]> assignmentsByPool = new Dictionary<int, MbsPoolLoanAssignment[]>();
        foreach (int num1 in ((IEnumerable<MbsPoolLoanAssignment>) andAssignedLoans1).Select<MbsPoolLoanAssignment, int>((System.Func<MbsPoolLoanAssignment, int>) (a => a.TradeId)).Distinct<int>().ToList<int>())
        {
          int poolId = num1;
          PipelineInfo[] array = ((IEnumerable<MbsPoolLoanAssignment>) andAssignedLoans1).Where<MbsPoolLoanAssignment>((System.Func<MbsPoolLoanAssignment, bool>) (a => a.TradeId == poolId)).Select<MbsPoolLoanAssignment, PipelineInfo>((System.Func<MbsPoolLoanAssignment, PipelineInfo>) (a => a.PipelineInfo)).ToArray<PipelineInfo>();
          MbsPoolInfo mbsPoolInfo = pools == null || !pools.Any<MbsPoolInfo>((System.Func<MbsPoolInfo, bool>) (p => p.TradeID == poolId)) ? Session.SessionObjects.MbsPoolManager.GetTrade(poolId) : pools.Where<MbsPoolInfo>((System.Func<MbsPoolInfo, bool>) (p => p.TradeID == poolId)).First<MbsPoolInfo>();
          MbsPoolLoanAssignmentManager assignmentManager = new MbsPoolLoanAssignmentManager(Session.SessionObjects, poolId, array);
          MbsPoolLoanAssignment[] andAssignedLoans2 = assignmentManager.GetPendingAndAssignedLoans();
          bool flag1 = false;
          bool flag2 = false;
          foreach (MbsPoolLoanAssignment poolLoanAssignment in andAssignedLoans2)
          {
            MbsPoolLoanAssignment assignment = poolLoanAssignment;
            if (!((IEnumerable<TradeAssignmentByTradeBase>) this.FannieMaePEPoolAssignments).Any<TradeAssignmentByTradeBase>((System.Func<TradeAssignmentByTradeBase, bool>) (a =>
            {
              int? tradeId = a.TradeID;
              int num2 = poolId;
              return tradeId.GetValueOrDefault() == num2 & tradeId.HasValue;
            })))
            {
              assignmentManager.RemoveLoan(assignment.Guid);
              flag2 = true;
            }
            if (!string.Equals(this.gseCommitment.ContractNumber, this.originalContractNum, StringComparison.CurrentCultureIgnoreCase))
            {
              assignment.CommitmentContractNumber = this.gseCommitment.ContractNumber;
              flag2 = true;
            }
            if (assignment.ProductName != string.Empty && !this.gseCommitment.ProductNames.Any<FannieMaeProduct>((System.Func<FannieMaeProduct, bool>) (p => p.ProductName == assignment.ProductName)))
            {
              assignment.ProductName = string.Empty;
              flag2 = true;
            }
            if (this.PoolPricingUpdateRequired)
            {
              flag1 = true;
              if (!string.IsNullOrEmpty(assignment.ProductName))
                assignment.GuarantyFee = mbsPoolInfo.GetGuaranteeFee(this.gseCommitment, assignment.ProductName);
            }
          }
          if (flag2)
            assignmentManager.WriteCommitmentChangeToServer();
          if (flag2 | flag1)
            assignmentsByPool.Add(poolId, andAssignedLoans2);
        }
        if (assignmentsByPool.Count > 0)
          new MbsPoolProcesses().Execute(MbsPoolProcesses.ActionType.Commit, forceUpdateOfAllLoans || this.loanUpdatesRequired, assignmentsByPool);
        return true;
      }
      catch (Exception ex)
      {
        Tracing.Log(GseCommitmentEditor.sw, this.className, TraceLevel.Error, "Error applying loan status: " + (object) ex);
        int num = (int) Utils.Dialog((IWin32Window) this, "Unexpected error while attempting to update loans: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
    }

    private DialogResult commitTradeAssignmentsAsync(
      object forceUpdateOfAllLoans,
      IProgressFeedback feedback)
    {
      return DialogResult.Abort;
    }

    private bool validateTradeData()
    {
      string str1 = this.txtCmtId.Text.Trim();
      if (str1.Length == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must enter a name/number for this commitment before saving.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return false;
      }
      if (string.Compare(str1, this.originalTradeName, true) != 0 && Session.GseCommitmentManager.GetTradeByName(str1) != null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "A commitment with the name/number '" + str1 + "' already exists. You must enter a unique name for this commitment.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return false;
      }
      string str2 = this.txtContractNumber.Text.Trim();
      if (!string.IsNullOrEmpty(str2) && string.Compare(str2, this.originalContractNum, true) != 0 && Session.GseCommitmentManager.GetTradeByContractNumber(str2) != null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "A GSE Commitment Contract number '" + str2 + "' already exists. You must enter a unique name for this contract number", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return false;
      }
      return this.validateParticipationPercentage(this.txtParticipationPercentage.Text.Trim());
    }

    private void onFieldValueChanged(object sender, EventArgs e) => this.modified = true;

    private void tabTrade_SelectedIndexChanged(object sender, EventArgs e)
    {
      Cursor.Current = Cursors.WaitCursor;
      if (this.tabTrade.SelectedTab == this.tpPricing)
      {
        this.commitChanges();
        this.loadProducts();
      }
      if (this.tabTrade.SelectedTab == this.tpLoans)
        this.refreshLoanLists();
      Cursor.Current = Cursors.Default;
    }

    private void refreshLoanLists()
    {
      this.commitChanges();
      bool refreshLoans = false;
      if (this.lastPricingTradeInfo == null || !GSECommitmentInfo.ComparePricing(this.lastPricingTradeInfo, this.gseCommitment))
        refreshLoans = true;
      this.ctlLoanList.RefreshLoanList(refreshLoans);
      this.lastPricingTradeInfo = new GSECommitmentInfo(this.gseCommitment);
    }

    private void recalculateProfitability() => this.loadOutstandingBalance();

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

    private void tpLoans_Resize(object sender, EventArgs e)
    {
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

    private void tpDetails_Resize(object sender, EventArgs e)
    {
    }

    private void tpPricing_Resize(object sender, EventArgs e)
    {
      this.gcGuarantyFee.Top = this.ctlBuyUpDownEditor.Top = this.pnlProductSelectorContainer.Height;
      this.gcGuarantyFee.Left = this.ctlAdjustments.Left = 0;
      int num1 = Math.Max(0, this.pnlPricing.Height - 5);
      this.ctlBuyUpDownEditor.Height = num1;
      this.gcGuarantyFee.Height = Math.Max(num1 / 2, this.ctlBuyUpDownEditor.MinimumSize.Height);
      this.ctlAdjustments.Height = Math.Max(0, num1 - this.gcGuarantyFee.Height);
      this.ctlAdjustments.Top = this.gcGuarantyFee.Bottom + 5;
      int num2 = Math.Max(0, (Math.Max(0, this.pnlPricing.Width) - 5) / 2);
      this.ctlBuyUpDownEditor.Width = Math.Max(num2, this.ctlBuyUpDownEditor.MinimumSize.Width);
      this.gcGuarantyFee.Width = this.ctlAdjustments.Width = Math.Max(0, num2);
      this.ctlBuyUpDownEditor.Left = this.gcGuarantyFee.Right + 5;
      this.ctlBuyUpDownEditor.Visible = true;
      this.pnlProductSelect.Left = this.ctlBuyUpDownEditor.Left;
      this.pnlProductSelect.Width = this.ctlBuyUpDownEditor.Width;
    }

    private void onCommitmentDateChanged(object sender, EventArgs e) => this.modified = true;

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
        Tracing.Log(GseCommitmentEditor.sw, this.className, TraceLevel.Error, "Error during export: " + (object) ex);
        int num = (int) Utils.Dialog((IWin32Window) this, "An error occurred while attempting to export the loans to Microsoft Excel. Ensure that you have Excel installed and it is working properly.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void btnList_Click(object sender, EventArgs e)
    {
      TradeManagementConsole.Instance.CloseGseCommitment();
    }

    private DialogResult updateLoanTradeDataAsync(object stateNotUsed, IProgressFeedback feedback)
    {
      return DialogResult.Abort;
    }

    public void MenuClicked(ToolStripItem menuItem)
    {
      switch (string.Concat(menuItem.Tag))
      {
        case "GSEE_SaveTrade":
          this.SaveTrade();
          break;
        case "GSEE_ExitTrade":
          TradeManagementConsole.Instance.CloseGseCommitment();
          break;
      }
    }

    public bool SetMenuItemState(ToolStripItem menuItem)
    {
      Control stateControl = (Control) null;
      switch (string.Concat(menuItem.Tag))
      {
        case "GSEE_SaveTrade":
          stateControl = (Control) this.btnSave;
          break;
        case "GSEE_ExitTrade":
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

    private void deleteButton_Clicked(object sender, EventArgs e)
    {
      if (this.pairOffControl.SelectedPairOffs != null)
      {
        foreach (GSECommitmentPairOff selectedPairOff in this.pairOffControl.SelectedPairOffs)
          this.gseCommitment.GSECommitmentPairOffs.Remove(selectedPairOff);
      }
      this.modified = this.pairOffControl.Modified;
      this.loadPairOffs();
      this.loadProfitabilityData();
    }

    private void editButton_Clicked(object sender, EventArgs e)
    {
      if (this.pairOffControl.SelectedPairOffs != null)
      {
        CommonPairOff selectedPairOff = (CommonPairOff) this.pairOffControl.SelectedPairOffs[0];
        if (selectedPairOff.Index == -1)
        {
          this.gseCommitment.GSECommitmentPairOffs.Add(new GSECommitmentPairOff(selectedPairOff.Index, selectedPairOff.Date, selectedPairOff.TradeAmount, selectedPairOff.PairOffFeePercentage));
        }
        else
        {
          GSECommitmentPairOff commitmentPairOff = this.gseCommitment.GSECommitmentPairOffs[selectedPairOff.Index - 1];
          commitmentPairOff.Date = selectedPairOff.Date;
          commitmentPairOff.TradeAmount = selectedPairOff.TradeAmount;
          commitmentPairOff.PairOffFeePercentage = selectedPairOff.PairOffFeePercentage;
        }
      }
      this.modified = this.pairOffControl.Modified;
      this.loadPairOffs();
      this.loadProfitabilityData();
    }

    private void standardIconButton1_Click(object sender, EventArgs e)
    {
    }

    private void refreshConfigurableFieldOptions()
    {
      this.cmbTradeDesc.Items.Clear();
      ArrayList secondaryFields = Session.ConfigurationManager.GetSecondaryFields(SecondaryFieldTypes.TradeDescriptionOption);
      if (secondaryFields == null)
        return;
      foreach (string str in secondaryFields)
        this.cmbTradeDesc.Items.Add((object) str);
    }

    private void btnAddPN_Click(object sender, EventArgs e)
    {
      using (FannieMaeProductNames fannieMaeProductNames = new FannieMaeProductNames())
      {
        if (fannieMaeProductNames.ShowDialog((IWin32Window) this) != DialogResult.Yes)
          return;
        DataTable dataTable = this.productGrid.ConvertProductName(this.gvPNs.Items);
        dataTable.PrimaryKey = new DataColumn[1]
        {
          dataTable.Columns["ProductName"]
        };
        fannieMaeProductNames.Selected.PrimaryKey = new DataColumn[1]
        {
          fannieMaeProductNames.Selected.Columns["ProductName"]
        };
        foreach (DataRow row in (InternalDataCollectionBase) fannieMaeProductNames.Selected.Rows)
        {
          if (((IEnumerable<DataRow>) dataTable.Select("ProductName = '" + row[0].ToString() + "'")).Count<DataRow>() == 0)
          {
            GVItem gvItem = new GVItem();
            gvItem.SubItems.Add(new GVSubItem(row["ProductName"]));
            gvItem.SubItems.Add(new GVSubItem(row["DisplayName"]));
            gvItem.SubItems.Add(new GVSubItem(row["Description"]));
            FannieMaeProduct fannieMaeProduct = new FannieMaeProduct()
            {
              ProductName = row["ProductName"].ToString(),
              DisplayName = row["DisplayName"].ToString(),
              ProductDescription = row["Description"].ToString()
            };
            gvItem.Tag = (object) fannieMaeProduct;
            this.gvPNs.Items.Add(gvItem);
            this.modified = true;
          }
        }
      }
    }

    private void btnRemovePN_Click(object sender, EventArgs e)
    {
      this.deleteOption(this.gvPNs);
      this.poolPricingUpdateRequired = true;
      this.onLoanUpdatableFieldValueChanged(sender, e);
    }

    private void deleteOption(GridView listview)
    {
      if (listview.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select an option first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        int index = listview.SelectedItems[0].Index;
        listview.Items.Remove(listview.SelectedItems[0]);
        if (listview.Items.Count == 0)
          return;
        if (index + 1 > listview.Items.Count)
          listview.Items[listview.Items.Count - 1].Selected = true;
        else
          listview.Items[index].Selected = true;
      }
    }

    private void gvPNs_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnRemovePN.Enabled = this.gvPNs.SelectedItems.Count > 0;
    }

    private void siBtnAddGFee_Click(object sender, EventArgs e)
    {
      GuaranteeFeeDialog guaranteeFeeDialog = new GuaranteeFeeDialog(this.GetProductNames());
      if (guaranteeFeeDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
        return;
      this.gvGuarantyFeePricing.Items.Add(this.CreateGVGuaranteeFeeItem(guaranteeFeeDialog.GuarantyFee));
      this.poolPricingUpdateRequired = true;
      this.onLoanUpdatableFieldValueChanged(sender, e);
      if (!guaranteeFeeDialog.IsCreatingAnother)
        return;
      this.siBtnAddGFee_Click(sender, e);
    }

    private List<string> GetProductNames()
    {
      List<string> productNames = new List<string>();
      if (this.gvPNs.Items.Count > 0)
      {
        productNames = this.gvPNs.Items.Select<GVItem, string>((System.Func<GVItem, string>) (x => x.SubItems[0].ToString())).ToList<string>();
      }
      else
      {
        foreach (DataRow row in (InternalDataCollectionBase) Session.SessionObjects.ConfigurationManager.GetFannieMaeProductNames().Rows)
          productNames.Add(row[0].ToString());
      }
      return productNames;
    }

    private GVItem CreateGVGuaranteeFeeItem(GuarantyFeeItem gfitem)
    {
      GVItem guaranteeFeeItem = new GVItem();
      guaranteeFeeItem.SubItems.Add((object) gfitem.ProductName);
      if (gfitem.CouponMax.HasValue)
      {
        Decimal? couponMax = gfitem.CouponMax;
        Decimal couponMin = gfitem.CouponMin;
        if (!(couponMax.GetValueOrDefault() == couponMin & couponMax.HasValue))
        {
          GVSubItemCollection subItems = guaranteeFeeItem.SubItems;
          string str1 = gfitem.CouponMin.ToString("#0.000;;\"\"");
          couponMax = gfitem.CouponMax;
          string str2 = couponMax.Value.ToString("#0.000;;\"\"");
          string str3 = str1 + " - " + str2;
          subItems.Add((object) str3);
          goto label_4;
        }
      }
      guaranteeFeeItem.SubItems.Add((object) gfitem.CouponMin.ToString("#0.000;;\"\""));
label_4:
      guaranteeFeeItem.SubItems.Add((object) gfitem.GuarantyFee);
      guaranteeFeeItem.SubItems.Add((object) gfitem.CPA.ToString("#0.000;;\"\""));
      guaranteeFeeItem.Tag = (object) new GuarantyFeeItem(gfitem);
      return guaranteeFeeItem;
    }

    private void siBtnDeleteGFee_Click(object sender, EventArgs e)
    {
      this.gvGuarantyFeePricing.CancelEditing();
      foreach (GVItem selectedItem in this.gvGuarantyFeePricing.SelectedItems)
        this.gvGuarantyFeePricing.Items.Remove(selectedItem);
      this.gvGuarantyFeePricing.ReSort();
      this.poolPricingUpdateRequired = true;
      this.onLoanUpdatableFieldValueChanged(sender, e);
    }

    private void gvGuarantyFeePricing_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.siBtnDeleteGFee.Enabled = this.gvGuarantyFeePricing.SelectedItems.Count != 0;
    }

    private void gvGuarantyFeePricing_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      if (this.readOnly)
        return;
      GVHitTestInfo gvHitTestInfo = this.gvGuarantyFeePricing.HitTest(this.gvGuarantyFeePricing.PointToClient(Cursor.Position));
      if (gvHitTestInfo.RowIndex < 0)
        return;
      this.editGuarantFeePricing(this.gvGuarantyFeePricing.Items[gvHitTestInfo.RowIndex]);
    }

    private void editGuarantFeePricing(GVItem item)
    {
      using (GuaranteeFeeDialog guaranteeFeeDialog = new GuaranteeFeeDialog((GuarantyFeeItem) item.Tag, this.GetProductNames()))
      {
        if (guaranteeFeeDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        item.Tag = (object) guaranteeFeeDialog.GuarantyFee;
        this.RefreshGuarantyFeeList(item);
        this.poolPricingUpdateRequired = true;
        this.onLoanUpdatableFieldValueChanged((object) null, (EventArgs) null);
      }
    }

    private void RefreshGuarantyFeeList(GVItem item)
    {
      GuarantyFeeItem tag = (GuarantyFeeItem) item.Tag;
      item.SubItems[0].Text = tag.ProductName;
      if (tag.CouponMax.HasValue)
      {
        Decimal? couponMax = tag.CouponMax;
        Decimal couponMin = tag.CouponMin;
        if (!(couponMax.GetValueOrDefault() == couponMin & couponMax.HasValue))
        {
          GVSubItem subItem = item.SubItems[1];
          string str1 = tag.CouponMin.ToString("#0.000;;\"\"");
          couponMax = tag.CouponMax;
          string str2 = couponMax.Value.ToString("#0.000;;\"\"");
          string str3 = str1 + " - " + str2;
          subItem.Text = str3;
          goto label_4;
        }
      }
      item.SubItems[1].Text = tag.CouponMin.ToString("#0.000;;\"\"");
label_4:
      item.SubItems[2].Text = tag.GuarantyFee.ToString("#0.000;;\"\"");
      item.SubItems[3].Text = tag.CPA.ToString("#0.000;;\"\"");
    }

    private void tabTrade_DeSelecting(object sender, TabControlCancelEventArgs e)
    {
      if (sender == null || ((TabControl) sender).SelectedTab != this.tpPricing)
        return;
      e.Cancel = !this.ctlBuyUpDownEditor.ValidateBuyUpDown();
    }

    private void btnPairedOffAmnt_Click(object sender, EventArgs e)
    {
      FieldLockButton fieldLockButton = (FieldLockButton) sender;
      fieldLockButton.Locked = !fieldLockButton.Locked;
      this.loadProfitabilityData();
      this.txtPairedOffAmnt.Enabled = fieldLockButton.Locked;
      this.modified = true;
    }

    private void btnOutstandingBalance_Click(object sender, EventArgs e)
    {
      FieldLockButton fieldLockButton = (FieldLockButton) sender;
      fieldLockButton.Locked = !fieldLockButton.Locked;
      this.loadOutstandingBalance();
      this.txtOutStandingBalance.Enabled = fieldLockButton.Locked;
      this.modified = true;
    }

    private void onLoanUpdatableFieldValueChanged(object sender, EventArgs e)
    {
      this.loanUpdatesRequired = true;
      this.onFieldValueChanged(sender, e);
    }

    private void onCmtIdKeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar != ',')
        return;
      e.Handled = true;
    }

    private void txtPairedOffAmnt_TextChanged(object sender, EventArgs e)
    {
      TextBox textBox = (TextBox) sender;
      if (!string.IsNullOrEmpty(textBox.Text))
      {
        Decimal result;
        if (!Decimal.TryParse(textBox.Text, out result))
        {
          textBox.Text = this.originalPairOffAmt;
          return;
        }
        if (result > 0M)
        {
          textBox.Text = "-" + textBox.Text.Trim();
          textBox.Select(textBox.Text.Length, 0);
          textBox.Focus();
        }
      }
      this.loadOutstandingBalance();
      this.originalPairOffAmt = textBox.Text;
      this.modified = true;
    }

    private void onLoadOutstandingBalance(object sender, EventArgs e)
    {
      this.loadOutstandingBalance();
      this.modified = true;
    }

    private void loadOutstandingBalance()
    {
      if (this.btnOutstandingBalance.Locked)
        return;
      Decimal allocatedAmount = MbsPoolCalculation.CalculateAllocatedAmount(MbsPoolAssignment.Convert(this.ctlMbsPoolList.GetCurrentAssignments()));
      this.txtOutStandingBalance.Text = this.gseCommitment.GetOutstandingBalance(Utils.ParseDecimal((object) this.txtAmount.Text), Utils.ParseDecimal((object) this.txtPairedOffAmnt.Text), allocatedAmount).ToString("#,##0.00;;\"\"");
    }

    private TradeFilter getCurrentFilter()
    {
      return new TradeFilter(new SimpleTradeFilter(), this.ctlLoanList.GetCurrentLayout());
    }

    public Decimal CalculateProfit(PipelineInfo info, Decimal securityPrice)
    {
      return this.gseCommitment.CalculateProfit(info, securityPrice);
    }

    private void txtParticipationPercentage_Leave(object sender, EventArgs e)
    {
      TextBox textBox = (TextBox) sender;
      textBox.BackColor = SystemColors.Window;
      if (!this.modified || this.validateParticipationPercentage(textBox.Text))
        return;
      textBox.BackColor = Color.LightYellow;
      textBox.Focus();
    }

    private bool validateParticipationPercentage(string text)
    {
      bool flag = true;
      if (Utils.ParseDecimal((object) text) >= 10000M)
      {
        int num = (int) Utils.Dialog((IWin32Window) this.ParentForm, "Valid enter is a decimal less than 10000", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        flag = false;
      }
      return flag;
    }

    private void cmbProdct_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.ctlBuyUpDownEditor.CommitChanges();
      this.ctlBuyUpDownEditor.BuyUpDownItems = this.gseCommitment.ProductNames.Where<FannieMaeProduct>((System.Func<FannieMaeProduct, bool>) (p => p.ProductName == this.cmbProdct.Text)).ToList<FannieMaeProduct>()[0].BuyUpDownItems;
      this.ctlBuyUpDownEditor.ReadOnly = false;
    }

    public List<LoanToTradeAssignmentBase> GetAllAssignedPendingLoans()
    {
      List<LoanToTradeAssignmentBase> assignedPendingLoans = new List<LoanToTradeAssignmentBase>();
      if (this.assignments == null)
        return assignedPendingLoans;
      foreach (MbsPoolLoanAssignment assignedPendingLoan in this.assignments.GetAllAssignedPendingLoans())
        assignedPendingLoans.Add((LoanToTradeAssignmentBase) assignedPendingLoan);
      return assignedPendingLoans;
    }

    public void CommitLoanToTradeAssignments(bool forceUpdateOfAllLoans, bool selectedLoans)
    {
      throw new NotImplementedException();
    }

    public Decimal GetOpenAmount() => throw new NotImplementedException();

    public void MakePending(bool value) => throw new NotImplementedException();

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
      this.toolTips = new ToolTip(this.components);
      this.btnExportHistory = new StandardIconButton();
      this.btnList = new StandardIconButton();
      this.btnSave = new StandardIconButton();
      this.btnRemovePN = new StandardIconButton();
      this.btnAddPN = new StandardIconButton();
      this.siBtnDeleteGFee = new StandardIconButton();
      this.siBtnAddGFee = new StandardIconButton();
      this.tabTrade = new TabControl();
      this.tpDetails = new TabPage();
      this.pnlDetails = new Panel();
      this.pnlRight = new Panel();
      this.pnlFilter = new Panel();
      this.grpProductNames = new GroupContainer();
      this.gvPNs = new GridView();
      this.collapsibleSplitter2 = new CollapsibleSplitter();
      this.pnlRightBottom = new Panel();
      this.panelPairOffEditor = new Panel();
      this.pnlMbsPools = new Panel();
      this.collapsibleSplitter1 = new CollapsibleSplitter();
      this.pnlLeft = new Panel();
      this.panel1 = new Panel();
      this.groupContainer1 = new GroupContainer();
      this.panel2 = new Panel();
      this.dtCommitment = new DatePicker();
      this.txtRemittanceCycleMonth = new ComboBox();
      this.dtIssueMonth = new DatePicker();
      this.label31 = new Label();
      this.cboxBondType = new ComboBox();
      this.label30 = new Label();
      this.label29 = new Label();
      this.txtMinGfeeBuydown = new TextBox();
      this.label28 = new Label();
      this.label27 = new Label();
      this.label26 = new Label();
      this.txtFalloutAmnt = new TextBox();
      this.txtRolledFrom = new TextBox();
      this.txtRolledTo = new TextBox();
      this.txtRolledAmnt = new TextBox();
      this.label25 = new Label();
      this.btnPairedOffAmnt = new FieldLockButton();
      this.label24 = new Label();
      this.label23 = new Label();
      this.txtFees = new TextBox();
      this.txtPairedOffAmnt = new TextBox();
      this.btnOutstandingBalance = new FieldLockButton();
      this.cboxServicingOption = new ComboBox();
      this.cboxRemittanceCycle = new ComboBox();
      this.cboxBuyUpDownGrid = new ComboBox();
      this.txtParticipationPercentage = new TextBox();
      this.label11 = new Label();
      this.txtMaxBuyupAmount = new TextBox();
      this.label16 = new Label();
      this.label17 = new Label();
      this.label18 = new Label();
      this.label19 = new Label();
      this.label21 = new Label();
      this.txtMaxRemainingAmnt = new TextBox();
      this.label2 = new Label();
      this.txtMinRemainingAmnt = new TextBox();
      this.txtRollFeeFactor = new TextBox();
      this.label9 = new Label();
      this.txtPairOffFeeFactor = new TextBox();
      this.label10 = new Label();
      this.label15 = new Label();
      this.txtMaxDeliveryAmnt = new TextBox();
      this.label7 = new Label();
      this.txtMinDeliveryAmnt = new TextBox();
      this.label6 = new Label();
      this.label5 = new Label();
      this.txtOutStandingBalance = new TextBox();
      this.label22 = new Label();
      this.txtSellerNumber = new TextBox();
      this.label14 = new Label();
      this.txtContractNumber = new TextBox();
      this.cmbTradeDesc = new ComboBox();
      this.label8 = new Label();
      this.label1 = new Label();
      this.txtCmtId = new TextBox();
      this.label13 = new Label();
      this.txtPendingAmnt = new TextBox();
      this.label20 = new Label();
      this.txtFulfilledAmnt = new TextBox();
      this.label12 = new Label();
      this.label4 = new Label();
      this.txtAmount = new TextBox();
      this.label3 = new Label();
      this.tpPricing = new TabPage();
      this.pnlPricing = new Panel();
      this.pnlProductSelectorContainer = new Panel();
      this.pnlProductSelect = new Panel();
      this.cmbProdct = new ComboBox();
      this.label32 = new Label();
      this.gcGuarantyFee = new GroupContainer();
      this.panel3 = new Panel();
      this.gvGuarantyFeePricing = new GridView();
      this.flowLayoutPanel3 = new FlowLayoutPanel();
      this.ctlBuyUpDownEditor = new MBSPoolBuyUpDownEditor();
      this.btnAdjustmentTemplate = new Button();
      this.ctlAdjustments = new PriceAdjustmentListEditor();
      this.tpLoans = new TabPage();
      this.tpHistory = new TabPage();
      this.pnlHistory = new Panel();
      this.grpHistory = new GroupContainer();
      this.gvHistory = new GridView();
      this.grpNotes = new GroupContainer();
      this.txtNotes = new TextBox();
      this.btnDateStamp = new Button();
      this.btnSRPTemplate = new Button();
      this.gradientPanel1 = new GradientPanel();
      this.lblTradeName = new Label();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      ((ISupportInitialize) this.btnExportHistory).BeginInit();
      ((ISupportInitialize) this.btnList).BeginInit();
      ((ISupportInitialize) this.btnSave).BeginInit();
      ((ISupportInitialize) this.btnRemovePN).BeginInit();
      ((ISupportInitialize) this.btnAddPN).BeginInit();
      ((ISupportInitialize) this.siBtnDeleteGFee).BeginInit();
      ((ISupportInitialize) this.siBtnAddGFee).BeginInit();
      this.tabTrade.SuspendLayout();
      this.tpDetails.SuspendLayout();
      this.pnlDetails.SuspendLayout();
      this.pnlRight.SuspendLayout();
      this.pnlFilter.SuspendLayout();
      this.grpProductNames.SuspendLayout();
      this.pnlRightBottom.SuspendLayout();
      this.pnlLeft.SuspendLayout();
      this.panel1.SuspendLayout();
      this.groupContainer1.SuspendLayout();
      this.panel2.SuspendLayout();
      this.tpPricing.SuspendLayout();
      this.pnlPricing.SuspendLayout();
      this.pnlProductSelectorContainer.SuspendLayout();
      this.pnlProductSelect.SuspendLayout();
      this.gcGuarantyFee.SuspendLayout();
      this.panel3.SuspendLayout();
      this.flowLayoutPanel3.SuspendLayout();
      this.tpHistory.SuspendLayout();
      this.pnlHistory.SuspendLayout();
      this.grpHistory.SuspendLayout();
      this.grpNotes.SuspendLayout();
      this.gradientPanel1.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      this.SuspendLayout();
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
      this.toolTips.SetToolTip((Control) this.btnList, "Exit GSE Commitment");
      this.btnList.Click += new EventHandler(this.btnList_Click);
      this.btnSave.BackColor = Color.Transparent;
      this.btnSave.Location = new Point(574, 3);
      this.btnSave.MouseDownImage = (Image) null;
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(16, 16);
      this.btnSave.StandardButtonType = StandardIconButton.ButtonType.SaveButton;
      this.btnSave.TabIndex = 6;
      this.btnSave.TabStop = false;
      this.toolTips.SetToolTip((Control) this.btnSave, "Save GSE Commitment");
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.btnRemovePN.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnRemovePN.BackColor = Color.Transparent;
      this.btnRemovePN.Enabled = false;
      this.btnRemovePN.Location = new Point(711, 5);
      this.btnRemovePN.MouseDownImage = (Image) null;
      this.btnRemovePN.Name = "btnRemovePN";
      this.btnRemovePN.Size = new Size(16, 16);
      this.btnRemovePN.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnRemovePN.TabIndex = 2;
      this.btnRemovePN.TabStop = false;
      this.toolTips.SetToolTip((Control) this.btnRemovePN, "Delete Product");
      this.btnRemovePN.Click += new EventHandler(this.btnRemovePN_Click);
      this.btnAddPN.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAddPN.BackColor = Color.Transparent;
      this.btnAddPN.Location = new Point(691, 5);
      this.btnAddPN.MouseDownImage = (Image) null;
      this.btnAddPN.Name = "btnAddPN";
      this.btnAddPN.Size = new Size(16, 16);
      this.btnAddPN.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAddPN.TabIndex = 1;
      this.btnAddPN.TabStop = false;
      this.toolTips.SetToolTip((Control) this.btnAddPN, "Add Product");
      this.btnAddPN.Click += new EventHandler(this.btnAddPN_Click);
      this.siBtnDeleteGFee.BackColor = Color.Transparent;
      this.siBtnDeleteGFee.Enabled = false;
      this.siBtnDeleteGFee.Location = new Point(78, 3);
      this.siBtnDeleteGFee.MouseDownImage = (Image) null;
      this.siBtnDeleteGFee.Name = "siBtnDeleteGFee";
      this.siBtnDeleteGFee.Size = new Size(16, 16);
      this.siBtnDeleteGFee.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.siBtnDeleteGFee.TabIndex = 0;
      this.siBtnDeleteGFee.TabStop = false;
      this.toolTips.SetToolTip((Control) this.siBtnDeleteGFee, "Delete Guaranty Fee");
      this.siBtnDeleteGFee.Click += new EventHandler(this.siBtnDeleteGFee_Click);
      this.siBtnAddGFee.BackColor = Color.Transparent;
      this.siBtnAddGFee.Location = new Point(56, 3);
      this.siBtnAddGFee.MouseDownImage = (Image) null;
      this.siBtnAddGFee.Name = "siBtnAddGFee";
      this.siBtnAddGFee.Size = new Size(16, 16);
      this.siBtnAddGFee.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.siBtnAddGFee.TabIndex = 1;
      this.siBtnAddGFee.TabStop = false;
      this.toolTips.SetToolTip((Control) this.siBtnAddGFee, "Add Guaranty Fee");
      this.siBtnAddGFee.Click += new EventHandler(this.siBtnAddGFee_Click);
      this.tabTrade.Controls.Add((Control) this.tpDetails);
      this.tabTrade.Controls.Add((Control) this.tpPricing);
      this.tabTrade.Controls.Add((Control) this.tpLoans);
      this.tabTrade.Controls.Add((Control) this.tpHistory);
      this.tabTrade.Dock = DockStyle.Fill;
      this.tabTrade.ItemSize = new Size(44, 20);
      this.tabTrade.Location = new Point(0, 31);
      this.tabTrade.Name = "tabTrade";
      this.tabTrade.Padding = new Point(11, 3);
      this.tabTrade.SelectedIndex = 0;
      this.tabTrade.Size = new Size(1053, 653);
      this.tabTrade.TabIndex = 8;
      this.tabTrade.SelectedIndexChanged += new EventHandler(this.tabTrade_SelectedIndexChanged);
      this.tabTrade.Deselecting += new TabControlCancelEventHandler(this.tabTrade_DeSelecting);
      this.tabTrade.Resize += new EventHandler(this.tpLoans_Resize);
      this.tpDetails.Controls.Add((Control) this.pnlDetails);
      this.tpDetails.Location = new Point(4, 24);
      this.tpDetails.Name = "tpDetails";
      this.tpDetails.Padding = new Padding(0, 2, 2, 2);
      this.tpDetails.Size = new Size(1045, 625);
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
      this.pnlDetails.Size = new Size(1043, 621);
      this.pnlDetails.TabIndex = 6;
      this.pnlRight.Controls.Add((Control) this.pnlFilter);
      this.pnlRight.Controls.Add((Control) this.collapsibleSplitter2);
      this.pnlRight.Controls.Add((Control) this.pnlRightBottom);
      this.pnlRight.Dock = DockStyle.Fill;
      this.pnlRight.Location = new Point(310, 0);
      this.pnlRight.Name = "pnlRight";
      this.pnlRight.Size = new Size(733, 621);
      this.pnlRight.TabIndex = 8;
      this.pnlFilter.Controls.Add((Control) this.grpProductNames);
      this.pnlFilter.Dock = DockStyle.Fill;
      this.pnlFilter.Location = new Point(0, 0);
      this.pnlFilter.Name = "pnlFilter";
      this.pnlFilter.Size = new Size(733, 214);
      this.pnlFilter.TabIndex = 5;
      this.grpProductNames.Controls.Add((Control) this.gvPNs);
      this.grpProductNames.Controls.Add((Control) this.btnRemovePN);
      this.grpProductNames.Controls.Add((Control) this.btnAddPN);
      this.grpProductNames.Dock = DockStyle.Fill;
      this.grpProductNames.HeaderForeColor = SystemColors.ControlText;
      this.grpProductNames.Location = new Point(0, 0);
      this.grpProductNames.Name = "grpProductNames";
      this.grpProductNames.Size = new Size(733, 214);
      this.grpProductNames.TabIndex = 0;
      this.grpProductNames.Text = "Select Product Names";
      this.gvPNs.AllowMultiselect = false;
      this.gvPNs.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "ProductName";
      gvColumn1.SpringToFit = true;
      gvColumn1.Text = "Column";
      gvColumn1.Width = 731;
      this.gvPNs.Columns.AddRange(new GVColumn[1]
      {
        gvColumn1
      });
      this.gvPNs.Dock = DockStyle.Fill;
      this.gvPNs.HeaderHeight = 0;
      this.gvPNs.HeaderVisible = false;
      this.gvPNs.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvPNs.Location = new Point(1, 26);
      this.gvPNs.Name = "gvPNs";
      this.gvPNs.Size = new Size(731, 187);
      this.gvPNs.TabIndex = 105;
      this.gvPNs.SelectedIndexChanged += new EventHandler(this.gvPNs_SelectedIndexChanged);
      this.collapsibleSplitter2.AnimationDelay = 20;
      this.collapsibleSplitter2.AnimationStep = 20;
      this.collapsibleSplitter2.BorderStyle3D = Border3DStyle.Flat;
      this.collapsibleSplitter2.ControlToHide = (Control) this.pnlRightBottom;
      this.collapsibleSplitter2.Dock = DockStyle.Bottom;
      this.collapsibleSplitter2.ExpandParentForm = false;
      this.collapsibleSplitter2.Location = new Point(0, 214);
      this.collapsibleSplitter2.Name = "collapsibleSplitter2";
      this.collapsibleSplitter2.TabIndex = 1;
      this.collapsibleSplitter2.TabStop = false;
      this.collapsibleSplitter2.UseAnimations = false;
      this.collapsibleSplitter2.VisualStyle = VisualStyles.Encompass;
      this.pnlRightBottom.AutoScroll = true;
      this.pnlRightBottom.Controls.Add((Control) this.panelPairOffEditor);
      this.pnlRightBottom.Controls.Add((Control) this.pnlMbsPools);
      this.pnlRightBottom.Dock = DockStyle.Bottom;
      this.pnlRightBottom.Location = new Point(0, 221);
      this.pnlRightBottom.Name = "pnlRightBottom";
      this.pnlRightBottom.Size = new Size(733, 400);
      this.pnlRightBottom.TabIndex = 0;
      this.panelPairOffEditor.Dock = DockStyle.Top;
      this.panelPairOffEditor.Location = new Point(0, 206);
      this.panelPairOffEditor.Name = "panelPairOffEditor";
      this.panelPairOffEditor.Padding = new Padding(0, 0, 0, 4);
      this.panelPairOffEditor.Size = new Size(733, 194);
      this.panelPairOffEditor.TabIndex = 111;
      this.pnlMbsPools.Dock = DockStyle.Top;
      this.pnlMbsPools.Location = new Point(0, 0);
      this.pnlMbsPools.Name = "pnlMbsPools";
      this.pnlMbsPools.Padding = new Padding(0, 0, 0, 4);
      this.pnlMbsPools.Size = new Size(733, 206);
      this.pnlMbsPools.TabIndex = 110;
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
      this.pnlLeft.Dock = DockStyle.Left;
      this.pnlLeft.Location = new Point(0, 0);
      this.pnlLeft.Name = "pnlLeft";
      this.pnlLeft.Size = new Size(303, 621);
      this.pnlLeft.TabIndex = 6;
      this.panel1.Controls.Add((Control) this.groupContainer1);
      this.panel1.Dock = DockStyle.Fill;
      this.panel1.Location = new Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(303, 621);
      this.panel1.TabIndex = 2;
      this.groupContainer1.Controls.Add((Control) this.panel2);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(303, 621);
      this.groupContainer1.TabIndex = 0;
      this.groupContainer1.Text = "GSE Commitment Info";
      this.panel2.AutoScroll = true;
      this.panel2.Controls.Add((Control) this.dtCommitment);
      this.panel2.Controls.Add((Control) this.txtRemittanceCycleMonth);
      this.panel2.Controls.Add((Control) this.dtIssueMonth);
      this.panel2.Controls.Add((Control) this.label31);
      this.panel2.Controls.Add((Control) this.cboxBondType);
      this.panel2.Controls.Add((Control) this.label30);
      this.panel2.Controls.Add((Control) this.label29);
      this.panel2.Controls.Add((Control) this.txtMinGfeeBuydown);
      this.panel2.Controls.Add((Control) this.label28);
      this.panel2.Controls.Add((Control) this.label27);
      this.panel2.Controls.Add((Control) this.label26);
      this.panel2.Controls.Add((Control) this.txtFalloutAmnt);
      this.panel2.Controls.Add((Control) this.txtRolledFrom);
      this.panel2.Controls.Add((Control) this.txtRolledTo);
      this.panel2.Controls.Add((Control) this.txtRolledAmnt);
      this.panel2.Controls.Add((Control) this.label25);
      this.panel2.Controls.Add((Control) this.btnPairedOffAmnt);
      this.panel2.Controls.Add((Control) this.label24);
      this.panel2.Controls.Add((Control) this.label23);
      this.panel2.Controls.Add((Control) this.txtFees);
      this.panel2.Controls.Add((Control) this.txtPairedOffAmnt);
      this.panel2.Controls.Add((Control) this.btnOutstandingBalance);
      this.panel2.Controls.Add((Control) this.cboxServicingOption);
      this.panel2.Controls.Add((Control) this.cboxRemittanceCycle);
      this.panel2.Controls.Add((Control) this.cboxBuyUpDownGrid);
      this.panel2.Controls.Add((Control) this.txtParticipationPercentage);
      this.panel2.Controls.Add((Control) this.label11);
      this.panel2.Controls.Add((Control) this.txtMaxBuyupAmount);
      this.panel2.Controls.Add((Control) this.label16);
      this.panel2.Controls.Add((Control) this.label17);
      this.panel2.Controls.Add((Control) this.label18);
      this.panel2.Controls.Add((Control) this.label19);
      this.panel2.Controls.Add((Control) this.label21);
      this.panel2.Controls.Add((Control) this.txtMaxRemainingAmnt);
      this.panel2.Controls.Add((Control) this.label2);
      this.panel2.Controls.Add((Control) this.txtMinRemainingAmnt);
      this.panel2.Controls.Add((Control) this.txtRollFeeFactor);
      this.panel2.Controls.Add((Control) this.label9);
      this.panel2.Controls.Add((Control) this.txtPairOffFeeFactor);
      this.panel2.Controls.Add((Control) this.label10);
      this.panel2.Controls.Add((Control) this.label15);
      this.panel2.Controls.Add((Control) this.txtMaxDeliveryAmnt);
      this.panel2.Controls.Add((Control) this.label7);
      this.panel2.Controls.Add((Control) this.txtMinDeliveryAmnt);
      this.panel2.Controls.Add((Control) this.label6);
      this.panel2.Controls.Add((Control) this.label5);
      this.panel2.Controls.Add((Control) this.txtOutStandingBalance);
      this.panel2.Controls.Add((Control) this.label22);
      this.panel2.Controls.Add((Control) this.txtSellerNumber);
      this.panel2.Controls.Add((Control) this.label14);
      this.panel2.Controls.Add((Control) this.txtContractNumber);
      this.panel2.Controls.Add((Control) this.cmbTradeDesc);
      this.panel2.Controls.Add((Control) this.label8);
      this.panel2.Controls.Add((Control) this.label1);
      this.panel2.Controls.Add((Control) this.txtCmtId);
      this.panel2.Controls.Add((Control) this.label13);
      this.panel2.Controls.Add((Control) this.txtPendingAmnt);
      this.panel2.Controls.Add((Control) this.label20);
      this.panel2.Controls.Add((Control) this.txtFulfilledAmnt);
      this.panel2.Controls.Add((Control) this.label12);
      this.panel2.Controls.Add((Control) this.label4);
      this.panel2.Controls.Add((Control) this.txtAmount);
      this.panel2.Controls.Add((Control) this.label3);
      this.panel2.Dock = DockStyle.Fill;
      this.panel2.Location = new Point(1, 26);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(301, 594);
      this.panel2.TabIndex = 23;
      this.dtCommitment.BackColor = SystemColors.Window;
      this.dtCommitment.Location = new Point(141, 80);
      this.dtCommitment.Name = "dtCommitment";
      this.dtCommitment.Size = new Size(138, 22);
      this.dtCommitment.TabIndex = 4;
      this.dtCommitment.ToolTip = "";
      this.dtCommitment.Value = new DateTime(0L);
      this.dtCommitment.ValueChanged += new EventHandler(this.onCommitmentDateChanged);
      this.txtRemittanceCycleMonth.DropDownStyle = ComboBoxStyle.DropDownList;
      this.txtRemittanceCycleMonth.FormattingEnabled = true;
      this.txtRemittanceCycleMonth.Items.AddRange(new object[31]
      {
        (object) "1",
        (object) "2",
        (object) "3",
        (object) "4",
        (object) "5",
        (object) "6",
        (object) "7",
        (object) "8",
        (object) "9",
        (object) "10",
        (object) "11",
        (object) "12",
        (object) "13",
        (object) "14",
        (object) "15",
        (object) "16",
        (object) "17",
        (object) "18",
        (object) "19",
        (object) "20",
        (object) "21",
        (object) "22",
        (object) "23",
        (object) "24",
        (object) "25",
        (object) "26",
        (object) "27",
        (object) "28",
        (object) "29",
        (object) "30",
        (object) "31"
      });
      this.txtRemittanceCycleMonth.Location = new Point(141, 529);
      this.txtRemittanceCycleMonth.Name = "txtRemittanceCycleMonth";
      this.txtRemittanceCycleMonth.Size = new Size(138, 22);
      this.txtRemittanceCycleMonth.TabIndex = 24;
      this.txtRemittanceCycleMonth.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.dtIssueMonth.BackColor = SystemColors.Window;
      this.dtIssueMonth.CustomFormat = "MM/yyyy";
      this.dtIssueMonth.Location = new Point(141, 168);
      this.dtIssueMonth.Name = "dtIssueMonth";
      this.dtIssueMonth.Size = new Size(138, 22);
      this.dtIssueMonth.TabIndex = 8;
      this.dtIssueMonth.ToolTip = "";
      this.dtIssueMonth.Value = new DateTime(0L);
      this.dtIssueMonth.ValueChanged += new EventHandler(this.onFieldValueChanged);
      this.label31.AutoSize = true;
      this.label31.Location = new Point(6, 590);
      this.label31.Name = "label31";
      this.label31.Size = new Size(58, 14);
      this.label31.TabIndex = 119;
      this.label31.Text = "Bond Type";
      this.cboxBondType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboxBondType.FormattingEnabled = true;
      this.cboxBondType.Items.AddRange(new object[2]
      {
        (object) "None",
        (object) "Mortgage Revenue Bond"
      });
      this.cboxBondType.Location = new Point(141, 586);
      this.cboxBondType.Name = "cboxBondType";
      this.cboxBondType.Size = new Size(138, 22);
      this.cboxBondType.TabIndex = 26;
      this.cboxBondType.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.label30.AutoSize = true;
      this.label30.Location = new Point(6, 541);
      this.label30.Name = "label30";
      this.label30.Size = new Size(36, 14);
      this.label30.TabIndex = 117;
      this.label30.Text = "Month";
      this.label29.AutoSize = true;
      this.label29.Location = new Point(6, 691);
      this.label29.Name = "label29";
      this.label29.Size = new Size(132, 14);
      this.label29.TabIndex = 116;
      this.label29.Text = "Min G-Fee after Buydown";
      this.txtMinGfeeBuydown.Location = new Point(141, 688);
      this.txtMinGfeeBuydown.MaxLength = 12;
      this.txtMinGfeeBuydown.Name = "txtMinGfeeBuydown";
      this.txtMinGfeeBuydown.Size = new Size(138, 20);
      this.txtMinGfeeBuydown.TabIndex = 30;
      this.txtMinGfeeBuydown.TextAlign = HorizontalAlignment.Right;
      this.txtMinGfeeBuydown.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.label28.AutoSize = true;
      this.label28.Location = new Point(6, 481);
      this.label28.Name = "label28";
      this.label28.Size = new Size(77, 14);
      this.label28.TabIndex = 114;
      this.label28.Text = "Fallout Amount";
      this.label27.AutoSize = true;
      this.label27.Location = new Point(6, 459);
      this.label27.Name = "label27";
      this.label27.Size = new Size(63, 14);
      this.label27.TabIndex = 113;
      this.label27.Text = "Rolled From";
      this.label26.AutoSize = true;
      this.label26.Location = new Point(6, 437);
      this.label26.Name = "label26";
      this.label26.Size = new Size(50, 14);
      this.label26.TabIndex = 112;
      this.label26.Text = "Rolled To";
      this.txtFalloutAmnt.Location = new Point(141, 477);
      this.txtFalloutAmnt.MaxLength = 12;
      this.txtFalloutAmnt.Name = "txtFalloutAmnt";
      this.txtFalloutAmnt.Size = new Size(138, 20);
      this.txtFalloutAmnt.TabIndex = 22;
      this.txtFalloutAmnt.TextAlign = HorizontalAlignment.Right;
      this.txtFalloutAmnt.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.txtRolledFrom.Location = new Point(141, 455);
      this.txtRolledFrom.MaxLength = 6;
      this.txtRolledFrom.Name = "txtRolledFrom";
      this.txtRolledFrom.Size = new Size(138, 20);
      this.txtRolledFrom.TabIndex = 21;
      this.txtRolledFrom.TextAlign = HorizontalAlignment.Right;
      this.txtRolledFrom.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.txtRolledTo.Location = new Point(141, 433);
      this.txtRolledTo.MaxLength = 6;
      this.txtRolledTo.Name = "txtRolledTo";
      this.txtRolledTo.Size = new Size(138, 20);
      this.txtRolledTo.TabIndex = 20;
      this.txtRolledTo.TextAlign = HorizontalAlignment.Right;
      this.txtRolledTo.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.txtRolledAmnt.Location = new Point(141, 389);
      this.txtRolledAmnt.MaxLength = 12;
      this.txtRolledAmnt.Name = "txtRolledAmnt";
      this.txtRolledAmnt.Size = new Size(138, 20);
      this.txtRolledAmnt.TabIndex = 18;
      this.txtRolledAmnt.TextAlign = HorizontalAlignment.Right;
      this.txtRolledAmnt.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.label25.AutoSize = true;
      this.label25.Location = new Point(6, 392);
      this.label25.Name = "label25";
      this.label25.Size = new Size(75, 14);
      this.label25.TabIndex = 107;
      this.label25.Text = "Rolled Amount";
      this.btnPairedOffAmnt.Location = new Point(141, 326);
      this.btnPairedOffAmnt.LockedStateToolTip = "Use Default Value";
      this.btnPairedOffAmnt.MaximumSize = new Size(16, 17);
      this.btnPairedOffAmnt.MinimumSize = new Size(16, 17);
      this.btnPairedOffAmnt.Name = "btnPairedOffAmnt";
      this.btnPairedOffAmnt.Size = new Size(16, 17);
      this.btnPairedOffAmnt.TabIndex = 102;
      this.btnPairedOffAmnt.UnlockedStateToolTip = "Enter Data Manually";
      this.btnPairedOffAmnt.Click += new EventHandler(this.btnPairedOffAmnt_Click);
      this.label24.AutoSize = true;
      this.label24.Location = new Point(6, 348);
      this.label24.Name = "label24";
      this.label24.Size = new Size(31, 14);
      this.label24.TabIndex = 106;
      this.label24.Text = "Fees";
      this.label23.AutoSize = true;
      this.label23.Location = new Point(6, 326);
      this.label23.Name = "label23";
      this.label23.Size = new Size(109, 14);
      this.label23.TabIndex = 105;
      this.label23.Text = "Total Pair-Off Amount";
      this.txtFees.Location = new Point(141, 345);
      this.txtFees.MaxLength = 10;
      this.txtFees.Name = "txtFees";
      this.txtFees.Size = new Size(138, 20);
      this.txtFees.TabIndex = 16;
      this.txtFees.TextAlign = HorizontalAlignment.Right;
      this.txtFees.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.txtPairedOffAmnt.Location = new Point(161, 323);
      this.txtPairedOffAmnt.MaxLength = 12;
      this.txtPairedOffAmnt.Name = "txtPairedOffAmnt";
      this.txtPairedOffAmnt.Size = new Size(118, 20);
      this.txtPairedOffAmnt.TabIndex = 15;
      this.txtPairedOffAmnt.TextAlign = HorizontalAlignment.Right;
      this.txtPairedOffAmnt.TextChanged += new EventHandler(this.txtPairedOffAmnt_TextChanged);
      this.btnOutstandingBalance.Location = new Point(141, 149);
      this.btnOutstandingBalance.LockedStateToolTip = "Use Default Value";
      this.btnOutstandingBalance.MaximumSize = new Size(16, 17);
      this.btnOutstandingBalance.MinimumSize = new Size(16, 17);
      this.btnOutstandingBalance.Name = "btnOutstandingBalance";
      this.btnOutstandingBalance.Size = new Size(16, 17);
      this.btnOutstandingBalance.TabIndex = 101;
      this.btnOutstandingBalance.UnlockedStateToolTip = "Enter Data Manually";
      this.btnOutstandingBalance.Click += new EventHandler(this.btnOutstandingBalance_Click);
      this.cboxServicingOption.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboxServicingOption.FormattingEnabled = true;
      this.cboxServicingOption.Items.AddRange(new object[2]
      {
        (object) "Special",
        (object) "Regular"
      });
      this.cboxServicingOption.Location = new Point(141, 559);
      this.cboxServicingOption.Name = "cboxServicingOption";
      this.cboxServicingOption.Size = new Size(138, 22);
      this.cboxServicingOption.TabIndex = 25;
      this.cboxServicingOption.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.cboxRemittanceCycle.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboxRemittanceCycle.FormattingEnabled = true;
      this.cboxRemittanceCycle.Items.AddRange(new object[3]
      {
        (object) "Standard",
        (object) "Rapid Payment Method",
        (object) "MBS Express"
      });
      this.cboxRemittanceCycle.Location = new Point(141, 503);
      this.cboxRemittanceCycle.Name = "cboxRemittanceCycle";
      this.cboxRemittanceCycle.Size = new Size(138, 22);
      this.cboxRemittanceCycle.TabIndex = 23;
      this.cboxRemittanceCycle.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.cboxBuyUpDownGrid.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboxBuyUpDownGrid.FormattingEnabled = true;
      this.cboxBuyUpDownGrid.Items.AddRange(new object[2]
      {
        (object) "Early",
        (object) "Late"
      });
      this.cboxBuyUpDownGrid.Location = new Point(141, 638);
      this.cboxBuyUpDownGrid.Name = "cboxBuyUpDownGrid";
      this.cboxBuyUpDownGrid.Size = new Size(138, 22);
      this.cboxBuyUpDownGrid.TabIndex = 28;
      this.cboxBuyUpDownGrid.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.txtParticipationPercentage.Location = new Point(141, 612);
      this.txtParticipationPercentage.MaxLength = 14;
      this.txtParticipationPercentage.Name = "txtParticipationPercentage";
      this.txtParticipationPercentage.Size = new Size(138, 20);
      this.txtParticipationPercentage.TabIndex = 27;
      this.txtParticipationPercentage.TextAlign = HorizontalAlignment.Right;
      this.txtParticipationPercentage.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.txtParticipationPercentage.Leave += new EventHandler(this.txtParticipationPercentage_Leave);
      this.label11.AutoSize = true;
      this.label11.Location = new Point(6, 564);
      this.label11.Name = "label11";
      this.label11.Size = new Size(86, 14);
      this.label11.TabIndex = 0;
      this.label11.Text = "Servicing Option";
      this.txtMaxBuyupAmount.Location = new Point(141, 666);
      this.txtMaxBuyupAmount.MaxLength = 12;
      this.txtMaxBuyupAmount.Name = "txtMaxBuyupAmount";
      this.txtMaxBuyupAmount.Size = new Size(138, 20);
      this.txtMaxBuyupAmount.TabIndex = 29;
      this.txtMaxBuyupAmount.TextAlign = HorizontalAlignment.Right;
      this.txtMaxBuyupAmount.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.label16.AutoSize = true;
      this.label16.Location = new Point(6, 668);
      this.label16.Name = "label16";
      this.label16.Size = new Size(100, 14);
      this.label16.TabIndex = 0;
      this.label16.Text = "Max Buyup Amount";
      this.label17.AutoSize = true;
      this.label17.Location = new Point(5, 642);
      this.label17.Name = "label17";
      this.label17.Size = new Size(108, 14);
      this.label17.TabIndex = 0;
      this.label17.Text = "BuyupBuydown Grid";
      this.label18.AutoSize = true;
      this.label18.Location = new Point(6, 614);
      this.label18.Name = "label18";
      this.label18.Size = new Size(123, 14);
      this.label18.TabIndex = 0;
      this.label18.Text = "Participation Percentage";
      this.label19.AutoSize = true;
      this.label19.Location = new Point(6, 529);
      this.label19.Name = "label19";
      this.label19.Size = new Size(125, 14);
      this.label19.TabIndex = 0;
      this.label19.Text = "Remittance Cycle Day of";
      this.label21.AutoSize = true;
      this.label21.Location = new Point(6, 506);
      this.label21.Name = "label21";
      this.label21.Size = new Size(90, 14);
      this.label21.TabIndex = 0;
      this.label21.Text = "Remittance Cycle";
      this.txtMaxRemainingAmnt.Location = new Point(141, 301);
      this.txtMaxRemainingAmnt.MaxLength = 12;
      this.txtMaxRemainingAmnt.Name = "txtMaxRemainingAmnt";
      this.txtMaxRemainingAmnt.Size = new Size(138, 20);
      this.txtMaxRemainingAmnt.TabIndex = 14;
      this.txtMaxRemainingAmnt.TextAlign = HorizontalAlignment.Right;
      this.txtMaxRemainingAmnt.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(6, 282);
      this.label2.Name = "label2";
      this.label2.Size = new Size(114, 14);
      this.label2.TabIndex = 0;
      this.label2.Text = "Min Remaining Amount";
      this.txtMinRemainingAmnt.Location = new Point(141, 279);
      this.txtMinRemainingAmnt.MaxLength = 12;
      this.txtMinRemainingAmnt.Name = "txtMinRemainingAmnt";
      this.txtMinRemainingAmnt.Size = new Size(138, 20);
      this.txtMinRemainingAmnt.TabIndex = 13;
      this.txtMinRemainingAmnt.TextAlign = HorizontalAlignment.Right;
      this.txtMinRemainingAmnt.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.txtRollFeeFactor.Location = new Point(141, 411);
      this.txtRollFeeFactor.MaxLength = 15;
      this.txtRollFeeFactor.Name = "txtRollFeeFactor";
      this.txtRollFeeFactor.Size = new Size(138, 20);
      this.txtRollFeeFactor.TabIndex = 19;
      this.txtRollFeeFactor.TextAlign = HorizontalAlignment.Right;
      this.txtRollFeeFactor.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.label9.AutoSize = true;
      this.label9.Location = new Point(6, 415);
      this.label9.Name = "label9";
      this.label9.Size = new Size(79, 14);
      this.label9.TabIndex = 0;
      this.label9.Text = "Roll Fee Factor";
      this.txtPairOffFeeFactor.Location = new Point(141, 367);
      this.txtPairOffFeeFactor.MaxLength = 15;
      this.txtPairOffFeeFactor.Name = "txtPairOffFeeFactor";
      this.txtPairOffFeeFactor.Size = new Size(138, 20);
      this.txtPairOffFeeFactor.TabIndex = 17;
      this.txtPairOffFeeFactor.TextAlign = HorizontalAlignment.Right;
      this.txtPairOffFeeFactor.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.label10.AutoSize = true;
      this.label10.Location = new Point(6, 370);
      this.label10.Name = "label10";
      this.label10.Size = new Size(100, 14);
      this.label10.TabIndex = 0;
      this.label10.Text = "Pair-Off Fee Factor";
      this.label15.AutoSize = true;
      this.label15.Location = new Point(6, 303);
      this.label15.Name = "label15";
      this.label15.Size = new Size(118, 14);
      this.label15.TabIndex = 0;
      this.label15.Text = "Max Remaining Amount";
      this.txtMaxDeliveryAmnt.Location = new Point(141, 213);
      this.txtMaxDeliveryAmnt.MaxLength = 12;
      this.txtMaxDeliveryAmnt.Name = "txtMaxDeliveryAmnt";
      this.txtMaxDeliveryAmnt.Size = new Size(138, 20);
      this.txtMaxDeliveryAmnt.TabIndex = 10;
      this.txtMaxDeliveryAmnt.TextAlign = HorizontalAlignment.Right;
      this.txtMaxDeliveryAmnt.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.label7.AutoSize = true;
      this.label7.Location = new Point(6, 193);
      this.label7.Name = "label7";
      this.label7.Size = new Size(104, 14);
      this.label7.TabIndex = 0;
      this.label7.Text = "Min Delivery Amount";
      this.txtMinDeliveryAmnt.Location = new Point(141, 191);
      this.txtMinDeliveryAmnt.MaxLength = 12;
      this.txtMinDeliveryAmnt.Name = "txtMinDeliveryAmnt";
      this.txtMinDeliveryAmnt.Size = new Size(138, 20);
      this.txtMinDeliveryAmnt.TabIndex = 9;
      this.txtMinDeliveryAmnt.TextAlign = HorizontalAlignment.Right;
      this.txtMinDeliveryAmnt.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.label6.AutoSize = true;
      this.label6.Location = new Point(6, 172);
      this.label6.Name = "label6";
      this.label6.Size = new Size(65, 14);
      this.label6.TabIndex = 0;
      this.label6.Text = "Issue Month";
      this.label5.AutoSize = true;
      this.label5.Location = new Point(6, 150);
      this.label5.Name = "label5";
      this.label5.Size = new Size(107, 14);
      this.label5.TabIndex = 0;
      this.label5.Text = "Outstanding Balance";
      this.txtOutStandingBalance.Location = new Point(161, 147);
      this.txtOutStandingBalance.MaxLength = 12;
      this.txtOutStandingBalance.Name = "txtOutStandingBalance";
      this.txtOutStandingBalance.Size = new Size(118, 20);
      this.txtOutStandingBalance.TabIndex = 7;
      this.txtOutStandingBalance.TextAlign = HorizontalAlignment.Right;
      this.txtOutStandingBalance.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.label22.AutoSize = true;
      this.label22.Location = new Point(6, 105);
      this.label22.Name = "label22";
      this.label22.Size = new Size(74, 14);
      this.label22.TabIndex = 0;
      this.label22.Text = "Seller Number";
      this.txtSellerNumber.Location = new Point(141, 103);
      this.txtSellerNumber.MaxLength = 9;
      this.txtSellerNumber.Name = "txtSellerNumber";
      this.txtSellerNumber.Size = new Size(138, 20);
      this.txtSellerNumber.TabIndex = 5;
      this.txtSellerNumber.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.label14.AutoSize = true;
      this.label14.Location = new Point(6, 30);
      this.label14.Name = "label14";
      this.label14.Size = new Size(88, 14);
      this.label14.TabIndex = 0;
      this.label14.Text = "Contract Number";
      this.txtContractNumber.Location = new Point(141, 30);
      this.txtContractNumber.MaxLength = 6;
      this.txtContractNumber.Name = "txtContractNumber";
      this.txtContractNumber.Size = new Size(138, 20);
      this.txtContractNumber.TabIndex = 2;
      this.txtContractNumber.TextChanged += new EventHandler(this.onLoanUpdatableFieldValueChanged);
      this.cmbTradeDesc.Location = new Point(141, 54);
      this.cmbTradeDesc.Name = "cmbTradeDesc";
      this.cmbTradeDesc.Size = new Size(138, 22);
      this.cmbTradeDesc.TabIndex = 3;
      this.cmbTradeDesc.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.label8.AutoSize = true;
      this.label8.Location = new Point(6, 57);
      this.label8.Name = "label8";
      this.label8.Size = new Size(92, 14);
      this.label8.TabIndex = 0;
      this.label8.Text = "Trade Description";
      this.label1.AutoSize = true;
      this.label1.Location = new Point(6, 8);
      this.label1.Name = "label1";
      this.label1.Size = new Size(76, 14);
      this.label1.TabIndex = 0;
      this.label1.Text = "Commitment ID";
      this.txtCmtId.Location = new Point(141, 8);
      this.txtCmtId.MaxLength = 64;
      this.txtCmtId.Name = "txtCmtId";
      this.txtCmtId.Size = new Size(138, 20);
      this.txtCmtId.TabIndex = 1;
      this.txtCmtId.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.txtCmtId.KeyPress += new KeyPressEventHandler(this.onCmtIdKeyPress);
      this.label13.AutoSize = true;
      this.label13.Location = new Point(6, 82);
      this.label13.Name = "label13";
      this.label13.Size = new Size(89, 14);
      this.label13.TabIndex = 0;
      this.label13.Text = "Commitment Date";
      this.txtPendingAmnt.Location = new Point(141, 257);
      this.txtPendingAmnt.MaxLength = 12;
      this.txtPendingAmnt.Name = "txtPendingAmnt";
      this.txtPendingAmnt.Size = new Size(138, 20);
      this.txtPendingAmnt.TabIndex = 12;
      this.txtPendingAmnt.TextAlign = HorizontalAlignment.Right;
      this.txtPendingAmnt.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.label20.AutoSize = true;
      this.label20.Location = new Point(6, 260);
      this.label20.Name = "label20";
      this.label20.Size = new Size(84, 14);
      this.label20.TabIndex = 0;
      this.label20.Text = "Pending Amount";
      this.txtFulfilledAmnt.Location = new Point(141, 235);
      this.txtFulfilledAmnt.MaxLength = 12;
      this.txtFulfilledAmnt.Name = "txtFulfilledAmnt";
      this.txtFulfilledAmnt.Size = new Size(139, 20);
      this.txtFulfilledAmnt.TabIndex = 11;
      this.txtFulfilledAmnt.TextAlign = HorizontalAlignment.Right;
      this.txtFulfilledAmnt.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.label12.AutoSize = true;
      this.label12.Location = new Point(6, 237);
      this.label12.Name = "label12";
      this.label12.Size = new Size(82, 14);
      this.label12.TabIndex = 0;
      this.label12.Text = "Fulfilled Amount";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(6, 128);
      this.label4.Name = "label4";
      this.label4.Size = new Size(103, 14);
      this.label4.TabIndex = 0;
      this.label4.Text = "Commitment Amount";
      this.txtAmount.Location = new Point(141, 125);
      this.txtAmount.MaxLength = 12;
      this.txtAmount.Name = "txtAmount";
      this.txtAmount.Size = new Size(138, 20);
      this.txtAmount.TabIndex = 6;
      this.txtAmount.TextAlign = HorizontalAlignment.Right;
      this.txtAmount.TextChanged += new EventHandler(this.onLoadOutstandingBalance);
      this.label3.AutoSize = true;
      this.label3.Location = new Point(6, 214);
      this.label3.Name = "label3";
      this.label3.Size = new Size(108, 14);
      this.label3.TabIndex = 0;
      this.label3.Text = "Max Delivery Amount";
      this.tpPricing.Controls.Add((Control) this.pnlPricing);
      this.tpPricing.Location = new Point(4, 24);
      this.tpPricing.Name = "tpPricing";
      this.tpPricing.Padding = new Padding(0, 2, 2, 2);
      this.tpPricing.Size = new Size(1045, 625);
      this.tpPricing.TabIndex = 1;
      this.tpPricing.Tag = (object) "Pricing";
      this.tpPricing.Text = "Pricing";
      this.tpPricing.UseVisualStyleBackColor = true;
      this.tpPricing.Resize += new EventHandler(this.tpPricing_Resize);
      this.pnlPricing.Controls.Add((Control) this.pnlProductSelectorContainer);
      this.pnlPricing.Controls.Add((Control) this.gcGuarantyFee);
      this.pnlPricing.Controls.Add((Control) this.ctlBuyUpDownEditor);
      this.pnlPricing.Controls.Add((Control) this.btnAdjustmentTemplate);
      this.pnlPricing.Controls.Add((Control) this.ctlAdjustments);
      this.pnlPricing.Dock = DockStyle.Fill;
      this.pnlPricing.Location = new Point(0, 2);
      this.pnlPricing.Name = "pnlPricing";
      this.pnlPricing.Size = new Size(1043, 621);
      this.pnlPricing.TabIndex = 3;
      this.pnlProductSelectorContainer.BackColor = Color.WhiteSmoke;
      this.pnlProductSelectorContainer.Controls.Add((Control) this.pnlProductSelect);
      this.pnlProductSelectorContainer.Dock = DockStyle.Top;
      this.pnlProductSelectorContainer.Location = new Point(0, 0);
      this.pnlProductSelectorContainer.Name = "pnlProductSelectorContainer";
      this.pnlProductSelectorContainer.Size = new Size(1043, 38);
      this.pnlProductSelectorContainer.TabIndex = 17;
      this.pnlProductSelect.Controls.Add((Control) this.cmbProdct);
      this.pnlProductSelect.Controls.Add((Control) this.label32);
      this.pnlProductSelect.Location = new Point(481, 4);
      this.pnlProductSelect.Name = "pnlProductSelect";
      this.pnlProductSelect.Size = new Size(559, 30);
      this.pnlProductSelect.TabIndex = 0;
      this.cmbProdct.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbProdct.FormattingEnabled = true;
      this.cmbProdct.Location = new Point(179, 3);
      this.cmbProdct.Name = "cmbProdct";
      this.cmbProdct.Size = new Size(227, 22);
      this.cmbProdct.TabIndex = 2;
      this.cmbProdct.SelectedIndexChanged += new EventHandler(this.cmbProdct_SelectedIndexChanged);
      this.label32.AutoSize = true;
      this.label32.Font = new Font("Arial", 8.25f, FontStyle.Bold);
      this.label32.Location = new Point(3, 7);
      this.label32.Name = "label32";
      this.label32.Size = new Size(177, 14);
      this.label32.TabIndex = 0;
      this.label32.Text = "Select Product for BU && BD Grid";
      this.gcGuarantyFee.Controls.Add((Control) this.panel3);
      this.gcGuarantyFee.Controls.Add((Control) this.flowLayoutPanel3);
      this.gcGuarantyFee.HeaderForeColor = SystemColors.ControlText;
      this.gcGuarantyFee.Location = new Point(35, 25);
      this.gcGuarantyFee.Name = "gcGuarantyFee";
      this.gcGuarantyFee.Size = new Size(439, 254);
      this.gcGuarantyFee.TabIndex = 16;
      this.gcGuarantyFee.Text = "Guaranty Fee Pricing";
      this.panel3.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.panel3.Controls.Add((Control) this.gvGuarantyFeePricing);
      this.panel3.Location = new Point(1, 25);
      this.panel3.Name = "panel3";
      this.panel3.Size = new Size(437, 225);
      this.panel3.TabIndex = 3;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column1";
      gvColumn2.Text = "Product Name";
      gvColumn2.TextAlignment = ContentAlignment.BottomCenter;
      gvColumn2.Width = 145;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column2";
      gvColumn3.SortMethod = GVSortMethod.None;
      gvColumn3.Text = "Coupon";
      gvColumn3.TextAlignment = ContentAlignment.BottomCenter;
      gvColumn3.Width = 100;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column3";
      gvColumn4.SortMethod = GVSortMethod.None;
      gvColumn4.Text = "Guaranty Fee";
      gvColumn4.TextAlignment = ContentAlignment.BottomCenter;
      gvColumn4.Width = 100;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column4";
      gvColumn5.Text = "CPA";
      gvColumn5.TextAlignment = ContentAlignment.BottomCenter;
      gvColumn5.Width = 100;
      this.gvGuarantyFeePricing.Columns.AddRange(new GVColumn[4]
      {
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5
      });
      this.gvGuarantyFeePricing.Dock = DockStyle.Fill;
      this.gvGuarantyFeePricing.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvGuarantyFeePricing.Location = new Point(0, 0);
      this.gvGuarantyFeePricing.Name = "gvGuarantyFeePricing";
      this.gvGuarantyFeePricing.Size = new Size(437, 225);
      this.gvGuarantyFeePricing.TabIndex = 1;
      this.gvGuarantyFeePricing.SelectedIndexChanged += new EventHandler(this.gvGuarantyFeePricing_SelectedIndexChanged);
      this.gvGuarantyFeePricing.ItemDoubleClick += new GVItemEventHandler(this.gvGuarantyFeePricing_ItemDoubleClick);
      this.flowLayoutPanel3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flowLayoutPanel3.BackColor = Color.Transparent;
      this.flowLayoutPanel3.Controls.Add((Control) this.siBtnDeleteGFee);
      this.flowLayoutPanel3.Controls.Add((Control) this.siBtnAddGFee);
      this.flowLayoutPanel3.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel3.Location = new Point(341, 2);
      this.flowLayoutPanel3.Name = "flowLayoutPanel3";
      this.flowLayoutPanel3.Size = new Size(97, 22);
      this.flowLayoutPanel3.TabIndex = 0;
      this.ctlBuyUpDownEditor.Location = new Point(476, 25);
      this.ctlBuyUpDownEditor.Name = "ctlBuyUpDownEditor";
      this.ctlBuyUpDownEditor.ReadOnly = false;
      this.ctlBuyUpDownEditor.Size = new Size(482, 513);
      this.ctlBuyUpDownEditor.TabIndex = 15;
      this.btnAdjustmentTemplate.BackColor = SystemColors.Control;
      this.btnAdjustmentTemplate.Location = new Point(310, 308);
      this.btnAdjustmentTemplate.Margin = new Padding(0);
      this.btnAdjustmentTemplate.Name = "btnAdjustmentTemplate";
      this.btnAdjustmentTemplate.Size = new Size(70, 22);
      this.btnAdjustmentTemplate.TabIndex = 1;
      this.btnAdjustmentTemplate.Text = "Template";
      this.btnAdjustmentTemplate.UseVisualStyleBackColor = true;
      this.btnAdjustmentTemplate.Click += new EventHandler(this.btnAdjustmentTemplate_Click);
      this.ctlAdjustments.AdjustmentfromPPE = false;
      this.ctlAdjustments.Adjustments = (TradePriceAdjustments) null;
      this.ctlAdjustments.Location = new Point(35, 308);
      this.ctlAdjustments.Name = "ctlAdjustments";
      this.ctlAdjustments.ReadOnly = false;
      this.ctlAdjustments.Size = new Size(439, 244);
      this.ctlAdjustments.TabIndex = 2;
      this.tpLoans.Location = new Point(4, 24);
      this.tpLoans.Name = "tpLoans";
      this.tpLoans.Padding = new Padding(0, 2, 2, 2);
      this.tpLoans.Size = new Size(1045, 625);
      this.tpLoans.TabIndex = 2;
      this.tpLoans.Tag = (object) "Loans";
      this.tpLoans.Text = "Loans";
      this.tpLoans.UseVisualStyleBackColor = true;
      this.tpHistory.Controls.Add((Control) this.pnlHistory);
      this.tpHistory.Location = new Point(4, 24);
      this.tpHistory.Name = "tpHistory";
      this.tpHistory.Padding = new Padding(0, 2, 2, 2);
      this.tpHistory.Size = new Size(1045, 625);
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
      this.pnlHistory.Size = new Size(1043, 621);
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
      this.btnSRPTemplate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnSRPTemplate.BackColor = SystemColors.Control;
      this.btnSRPTemplate.Location = new Point(914, 8);
      this.btnSRPTemplate.Margin = new Padding(0);
      this.btnSRPTemplate.Name = "btnSRPTemplate";
      this.btnSRPTemplate.Size = new Size(70, 22);
      this.btnSRPTemplate.TabIndex = 1;
      this.btnSRPTemplate.Text = "Template";
      this.btnSRPTemplate.UseVisualStyleBackColor = true;
      this.gradientPanel1.BackColorGlassyStyle = true;
      this.gradientPanel1.Borders = AnchorStyles.Bottom;
      this.gradientPanel1.Controls.Add((Control) this.lblTradeName);
      this.gradientPanel1.Controls.Add((Control) this.flowLayoutPanel1);
      this.gradientPanel1.Dock = DockStyle.Top;
      this.gradientPanel1.GradientColor1 = Color.FromArgb(81, 123, 184);
      this.gradientPanel1.GradientColor2 = Color.FromArgb(167, 201, 239);
      this.gradientPanel1.Location = new Point(0, 0);
      this.gradientPanel1.Name = "gradientPanel1";
      this.gradientPanel1.Size = new Size(1053, 31);
      this.gradientPanel1.Style = GradientPanel.PanelStyle.PageHeader;
      this.gradientPanel1.TabIndex = 7;
      this.lblTradeName.AutoSize = true;
      this.lblTradeName.BackColor = Color.Transparent;
      this.lblTradeName.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblTradeName.Location = new Point(8, 8);
      this.lblTradeName.Name = "lblTradeName";
      this.lblTradeName.Size = new Size(84, 14);
      this.lblTradeName.TabIndex = 6;
      this.lblTradeName.Text = "<Trade Name>";
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
      this.Controls.Add((Control) this.tabTrade);
      this.Controls.Add((Control) this.gradientPanel1);
      this.Font = new Font("Arial", 8.25f);
      this.Name = nameof (GseCommitmentEditor);
      this.Size = new Size(1053, 684);
      ((ISupportInitialize) this.btnExportHistory).EndInit();
      ((ISupportInitialize) this.btnList).EndInit();
      ((ISupportInitialize) this.btnSave).EndInit();
      ((ISupportInitialize) this.btnRemovePN).EndInit();
      ((ISupportInitialize) this.btnAddPN).EndInit();
      ((ISupportInitialize) this.siBtnDeleteGFee).EndInit();
      ((ISupportInitialize) this.siBtnAddGFee).EndInit();
      this.tabTrade.ResumeLayout(false);
      this.tpDetails.ResumeLayout(false);
      this.pnlDetails.ResumeLayout(false);
      this.pnlRight.ResumeLayout(false);
      this.pnlFilter.ResumeLayout(false);
      this.grpProductNames.ResumeLayout(false);
      this.pnlRightBottom.ResumeLayout(false);
      this.pnlLeft.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.groupContainer1.ResumeLayout(false);
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      this.tpPricing.ResumeLayout(false);
      this.pnlPricing.ResumeLayout(false);
      this.pnlProductSelectorContainer.ResumeLayout(false);
      this.pnlProductSelect.ResumeLayout(false);
      this.pnlProductSelect.PerformLayout();
      this.gcGuarantyFee.ResumeLayout(false);
      this.panel3.ResumeLayout(false);
      this.flowLayoutPanel3.ResumeLayout(false);
      this.tpHistory.ResumeLayout(false);
      this.pnlHistory.ResumeLayout(false);
      this.grpHistory.ResumeLayout(false);
      this.grpNotes.ResumeLayout(false);
      this.grpNotes.PerformLayout();
      this.gradientPanel1.ResumeLayout(false);
      this.gradientPanel1.PerformLayout();
      this.flowLayoutPanel1.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
