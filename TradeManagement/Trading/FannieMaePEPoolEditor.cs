// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.FannieMaePEPoolEditor
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
using EllieMae.EMLite.Trading.FannieMaePEMBS;
using EllieMae.EMLite.Trading.Notifications;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class FannieMaePEPoolEditor : UserControl, IMenuProvider, ITradeEditor, ITradeEditorBase
  {
    private string className = nameof (FannieMaePEPoolEditor);
    private const int ControlPadding = 5;
    private static MbsPoolLoanStatusEnumNameProvider tradeStatusNameProvider = new MbsPoolLoanStatusEnumNameProvider();
    private static string sw = Tracing.SwOutsideLoan;
    private MbsPoolInfo poolTrade;
    private LoanReportFieldDefs fieldDefs;
    private TradeAssignedLoanFieldDefs tradeAssLoanFieldDefs;
    private MbsPoolInfo lastPricingTradeInfo;
    private TradeFilter lastEvaluatedFilter;
    private string originalTradeName;
    private int originalContractId = -1;
    private MbsPoolMortgageType defaultMortgageType;
    private string investorTemplateName;
    private MbsPoolFieldsEditorBase ctlFieldEditor;
    private LoanListScreen ctlLoanList;
    private SecurityTradeAssignmentListScreen ctlSecurityTradeList;
    private GSEcmtAssignmentListScreen commitmentListScreen;
    private FannieMaeProductGrid productGrid;
    private Timer refreshTimer = new Timer();
    private bool subscribed;
    private bool isTradeLoanUpdateEnabled = Session.StartupInfo.EnableTradeLoanUpdateNotification && Utils.ParseBoolean(Session.StartupInfo.TradeSettings[(object) "Trade.TradeLoanUpdateKafka"]);
    private bool modified;
    private bool loanUpdatesRequired;
    private bool readOnly;
    private bool loading;
    private bool suspendEvents;
    private MbsPoolLoanAssignmentManager assignments;
    private IContainer components;
    private TabControl tabTrade;
    private TabPage tpDetails;
    private Panel pnlFilter;
    private AdvancedSearchControl ctlAdvancedSearch;
    private TabPage tpLoans;
    private TabPage tpHistory;
    private GridView gvHistory;
    private TextBox txtNotes;
    private Button btnDateStamp;
    private SimpleSearchControl ctlSimpleSearch;
    private ToolTip toolTips;
    private ComboBox cboSearchType;
    private BorderPanel grpEditor;
    private FlowLayoutPanel flowLayoutPanel1;
    private StandardIconButton btnSave;
    private Panel pnlDetails;
    private Button btnSavedSearchesAdv;
    private VerticalSeparator vsAdvSearch;
    private Button btnSavedSearchesSimple;
    private Panel pnlHistory;
    private GroupContainer grpHistory;
    private GroupContainer grpNotes;
    private StandardIconButton btnExportHistory;
    private StandardIconButton btnList;
    private GradientPanel gradientPanel1;
    private Label lblTradeName;
    private CollapsibleSplitter collapsibleSplitter1;
    private Panel pnlLeft;
    private Panel pnlRight;
    private CollapsibleSplitter collapsibleSplitter2;
    private Panel pnlRightBottom;
    private Panel panel1;
    private GroupContainer groupContainer1;
    private Panel panel2;
    private Label label1;
    private TabPage tpSetup;
    private Panel pnlEditor;
    private TextBox txtPoolMortgageType;
    private Panel pnlRightBottomDetails;
    private GroupContainer grpAddress;
    private InvestorAddressEditor ctlContactInfo;
    private Panel pnlGridView;
    private Panel pnlSecurityTrades;
    private Panel pnlGseCommitments;
    private Panel pnlLeftTop;
    private GroupContainer grpProductNames;
    private GridView gvPNs;
    private StandardIconButton btnRemovePN;
    private StandardIconButton btnAddPN;
    private CollapsibleSplitter collapsibleSplitter3;
    private Panel panel4;
    private Panel panel3;
    private FieldLockButton lbtnWeightedAvgprice;
    private StandardIconButton standardIconButton1;
    private TextBox txtServicer;
    private Label lblServicer;
    private Label lblCommitmentDate;
    private DatePicker dtCommitment;
    private Label lblPoolNumber;
    private TextBox txtPoolNumber;
    private Label lblNotificationDate;
    private DatePicker dtNotification;
    private ComboBox cmbSettlementMonth;
    private Label lblSettlementMonth;
    private Label lblSettlementDate;
    private DatePicker dtSettlement;
    private Label lblPercent;
    private Label lblWeightedAveragePrice;
    private TextBox txtWeightedAvgPrice;
    private Label lblTerm;
    private TextBox txtTerm;
    private ComboBox cmbAmortizationType;
    private Label lblAmortiztionType;
    private ComboBox cmbMortgageType;
    private Label lblMortgageType;
    private Label lblCUSIP;
    private TextBox txtCUSIP;
    private Label lblSuffixID;
    private TextBox txtSuffixID;
    private ComboBox cmbServicingType;
    private Label lblServiceType;
    private Label lblInvestorDeliveryDate;
    private ComboBox cmbTradeDesc;
    private Label lblActualDeliveryDate;
    private Label lblTargetDeliveryDate;
    private Label lblTradeDescription;
    private Label lblEarlyDeliveryDate;
    private ComboBox cmbCommitmentType;
    private Label lblPoolID;
    private DatePicker dtActualDelivery;
    private Label lblCommitmentType;
    private TextBox txtPoolID;
    private Label lblPurchaseDate;
    private DatePicker dtTargetDelivery;
    private DatePicker dtPurchase;
    private DatePicker dtEarlyDelivery;
    private Label lblInvestor;
    private TextBox txtInvestor;
    private DatePicker dtInvestorDelivery;
    private StandardIconButton btnInvestorTemplate;
    private Label lblCoupon;
    private TextBox txtCoupon;
    private Label lblPoolAmount;
    private TextBox txtAmount;
    private Panel pnlProductName;
    private TabPage tpPricing;
    private Panel pnlPricing;
    private Panel pnlBottom;
    private Panel pnlChooseSpecificProduct;
    private Label label3;
    private Label label2;
    private PriceAdjustmentListEditor ctlAdjustments;
    private GridView gvPricing;
    private VerticalSeparator vsPricingType;
    private OpenFileDialog openFileDialog1;
    private Panel pnlBottomGrid;
    private MBSPoolBuyUpDownEditor ctlBuyUpDownEditor;
    private Label label4;
    private ComboBox cmbGSEProducts;
    private Panel pnlPricingLeft;
    private MBSPoolAdvancedPricingEditor ctlAdvancedPricing;
    private Label lblContractNumber;
    private FlowLayoutPanel flowLayoutPanel2;
    private PictureBox pictPending;
    private MSRPricingEditor msrPricingEditor1;

    public bool DataModified
    {
      get
      {
        if (this.readOnly)
          return false;
        return this.modified || this.ctlContactInfo.DataModified || this.ctlAdvancedPricing.DataModified || this.ctlBuyUpDownEditor.DataModified || this.ctlAdjustments.DataModified || this.isSearchModified() || this.assignments != null && this.assignments.HasModifiedAssignments() || this.ctlLoanList.DataModified || this.ctlFieldEditor != null && this.ctlFieldEditor.DataModified || this.ctlSecurityTradeList.DataModified || this.commitmentListScreen.DataModified || this.msrPricingEditor1.DataModified;
      }
    }

    public bool LoanUpdatesRequired
    {
      get
      {
        if (this.readOnly)
          return false;
        return this.loanUpdatesRequired || this.ctlContactInfo.DataModified || this.ctlAdvancedPricing.DataModified || this.ctlAdjustments.DataModified || this.ctlFieldEditor.LoanUpdatableFieldChanged;
      }
      set => this.loanUpdatesRequired = value;
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

    public TradeInfoObj CurrentTradeInfo
    {
      get => this.poolTrade != null ? (TradeInfoObj) this.poolTrade : (TradeInfoObj) null;
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

    public Decimal TradeAmount
    {
      get
      {
        return !string.IsNullOrEmpty(this.txtAmount.Text) ? Utils.ParseDecimal((object) this.txtAmount.Text) : 0M;
      }
    }

    public bool RemovePendingLoanFromOtherTrades { get; set; }

    public TradeAssignmentByTradeBase[] GseCommitmentAssignments { get; set; }

    private string CommitmentContractNumber { get; set; }

    private new string ProductName { get; set; }

    public string CommitmentContractNumberLoanCal { get; set; }

    public string ProductNameLoanCal { get; set; }

    protected override void Dispose(bool disposing)
    {
      this.unSubscribeEventHandler();
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    public FannieMaePEPoolEditor()
    {
      this.InitializeComponent();
      TextBoxFormatter.Attach(this.txtTerm, TextBoxContentRule.NonNegativeInteger);
      TextBoxFormatter.Attach(this.txtAmount, TextBoxContentRule.NonNegativeDecimal, "#,##0");
      TextBoxFormatter.Attach(this.txtCoupon, TextBoxContentRule.NonNegativeDecimal, "#,##0.00000;;\"\"");
      TextBoxFormatter.Attach(this.txtWeightedAvgPrice, TextBoxContentRule.NonNegativeDecimal, "0.00000000;;\"\"");
      this.addSearchButtonsToControls();
      this.refreshConfigurableFieldOptions();
      this.resetFieldDefs();
      this.ctlSecurityTradeList = new SecurityTradeAssignmentListScreen((ITradeEditorBase) this);
      this.ctlSecurityTradeList.Dock = DockStyle.Fill;
      this.ctlSecurityTradeList.ModifiedEvent += new EventHandler(this.ctlSecurityTradeList_ModifiedEvent);
      this.pnlSecurityTrades.Controls.Clear();
      this.pnlSecurityTrades.Size = new Size(731, 200);
      this.pnlSecurityTrades.Controls.Add((Control) this.ctlSecurityTradeList);
      this.commitmentListScreen = new GSEcmtAssignmentListScreen((ITradeEditorBase) this);
      this.commitmentListScreen.Dock = DockStyle.Fill;
      this.pnlGseCommitments.Controls.Clear();
      this.pnlGseCommitments.Size = new Size(731, 200);
      this.pnlGseCommitments.Controls.Add((Control) this.commitmentListScreen);
      this.tpDetails_Resize((object) null, (EventArgs) null);
      this.CommitmentContractNumber = "";
      this.ProductName = "";
      this.ctlLoanList = new LoanListScreen((ITradeEditor) this);
      this.tpLoans.Controls.Clear();
      this.tpLoans.Controls.Add((Control) this.ctlLoanList);
      this.ctlLoanList.Dock = DockStyle.Fill;
      this.productGrid = new FannieMaeProductGrid();
      GridViewLayoutManager viewLayoutManager = new GridViewLayoutManager(this.gvPricing, (TableLayout) null, this.getDemoPricingTableLayout(), (TableLayout) null);
      this.RemovePendingLoanFromOtherTrades = false;
      this.ctlAdjustments.MakeBtnInvisible();
      this.ctlBuyUpDownEditor.MakeBtnInvisible();
      this.refreshTimer.Interval = 5000;
      this.refreshTimer.Tick += new EventHandler(this.Refresh_Tick);
    }

    private void ctlSecurityTradeList_ModifiedEvent(object sender, EventArgs e)
    {
      if (this.poolTrade == null)
        return;
      Decimal weightedAveragePrice = MbsPoolCalculation.CalculateWeightedAveragePrice(MbsPoolAssignment.Convert(this.ctlSecurityTradeList.GetCurrentAssignments()));
      if (this.lbtnWeightedAvgprice.Locked)
        return;
      this.txtWeightedAvgPrice.Text = weightedAveragePrice.ToString("0.00000000;;\"\"");
      this.poolTrade.WeightedAvgPrice = Utils.ParseDecimal((object) this.txtWeightedAvgPrice.Text);
    }

    public void RefreshData() => this.RefreshData(new MbsPoolInfo());

    public void RefreshLoans()
    {
      if (this.poolTrade == null)
        return;
      this.loading = true;
      this.loadLoans(Session.MbsPoolManager.GetTradeEditorScreenData(this.poolTrade.TradeID, this.ctlLoanList.GetAssignedLoanListFields(), false).AssignedLoans, false);
      this.loadProfitabilityData();
    }

    public void RefreshData(MbsPoolInfo poolTrade) => this.RefreshData(poolTrade, (string[]) null);

    public void RefreshData(MbsPoolInfo poolTrade, string[] loanGuids)
    {
      this.loading = false;
      this.modified = false;
      this.readOnly = false;
      this.loanUpdatesRequired = false;
      this.RemovePendingLoanFromOtherTrades = false;
      this.lastPricingTradeInfo = (MbsPoolInfo) null;
      this.lastEvaluatedFilter = (TradeFilter) null;
      this.originalTradeName = (string) null;
      this.originalContractId = -1;
      this.poolTrade = poolTrade;
      this.assignments = (MbsPoolLoanAssignmentManager) null;
      this.placePricingTypeDDLToControls();
      this.ctlLoanList.ResetWithdrawnLoanMessageFlag();
      if (this.poolTrade.TradeID <= 0 && this.poolTrade.Filter != null && this.poolTrade.Filter.DataLayout != null)
        this.poolTrade.Filter.DataLayout.InsertColumns(0, this.ctlLoanList.GetFixedEligibleLoanColumns());
      this.refreshConfigurableFieldOptions(false);
      this.loadTradeData();
      this.ctlLoanList.RefreshViews();
      if (this.poolTrade.TradeID <= 0)
      {
        this.ctlLoanList.RefreshData();
        this.tabTrade.SelectedTab = this.tpDetails;
      }
      if (loanGuids != null)
      {
        this.ctlLoanList.PlaceAssignedLoans(loanGuids);
        if (this.poolTrade.TradeID > 0)
          this.tabTrade.SelectedTab = this.tpLoans;
      }
      this.loanUpdatesRequired = false;
    }

    public bool SaveTrade() => this.SaveTrade(false, false);

    private void commitChanges()
    {
      this.modified = this.DataModified;
      this.loanUpdatesRequired = this.LoanUpdatesRequired;
      this.poolTrade.Name = this.txtPoolID.Text.Trim();
      this.poolTrade.PoolNumber = this.txtPoolNumber.Text.Trim();
      this.poolTrade.SuffixID = this.txtSuffixID.Text;
      this.poolTrade.CUSIP = this.txtCUSIP.Text;
      this.poolTrade.MortgageType = this.cmbMortgageType.Text;
      this.poolTrade.AmortizationType = this.cmbAmortizationType.Text;
      this.poolTrade.Term = Utils.ParseInt((object) this.txtTerm.Text, 0);
      this.poolTrade.TradeAmount = Utils.ParseDecimal((object) this.txtAmount.Text);
      this.poolTrade.CommitmentType = this.cmbCommitmentType.Text;
      this.poolTrade.TradeDescription = this.cmbTradeDesc.Text;
      this.poolTrade.InvestorName = this.txtInvestor.Text;
      this.poolTrade.Coupon = Utils.ParseDecimal((object) this.txtCoupon.Text);
      this.poolTrade.WeightedAvgPrice = Utils.ParseDecimal((object) this.txtWeightedAvgPrice.Text);
      this.poolTrade.WeightedAvgPriceLocked = this.lbtnWeightedAvgprice.Locked;
      this.poolTrade.Servicer = this.txtServicer.Text;
      this.poolTrade.CommitmentDate = this.dtCommitment.Value;
      this.poolTrade.InvestorDeliveryDate = this.dtInvestorDelivery.Value;
      this.poolTrade.EarlyDeliveryDate = this.dtEarlyDelivery.Value;
      this.poolTrade.TargetDeliveryDate = this.dtTargetDelivery.Value;
      this.poolTrade.ShipmentDate = this.dtActualDelivery.Value;
      this.poolTrade.PurchaseDate = this.dtPurchase.Value;
      this.poolTrade.SettlementDate = this.dtSettlement.Value;
      this.poolTrade.NotificationDate = this.dtNotification.Value;
      this.poolTrade.Notes = this.txtNotes.Text;
      switch (this.cmbServicingType.SelectedIndex)
      {
        case 0:
          this.poolTrade.ServicingType = ServicingType.ServicingReleased;
          break;
        case 1:
          this.poolTrade.ServicingType = ServicingType.ServicingRetained;
          break;
      }
      this.poolTrade.SettlementMonth = !(this.cmbSettlementMonth.Text != string.Empty) ? (!Utils.IsDate((object) this.dtSettlement.Value) || !(this.dtSettlement.Value != DateTime.MinValue) ? "" : this.dtSettlement.Value.ToString("MMMM", (IFormatProvider) CultureInfo.InvariantCulture)) : this.cmbSettlementMonth.Text;
      this.ctlContactInfo.CommitChanges();
      this.ctlFieldEditor.CurrentInTradeLoans = this.assignments.GetAssignedPipelineData();
      this.ctlFieldEditor.CommitChanges();
      this.msrPricingEditor1.CommitChanges();
      this.commitFannieMaeProductNames(this.poolTrade);
      this.GseCommitmentAssignments = this.commitmentListScreen.GetCurrentAssignments();
      this.poolTrade.CalcAllPricingDetails(((IEnumerable<TradeAssignmentByTradeBase>) this.GseCommitmentAssignments).Select<TradeAssignmentByTradeBase, GSECommitmentInfo>((System.Func<TradeAssignmentByTradeBase, GSECommitmentInfo>) (c => ((GSECommitmentAssignment) c).Trade)).ToList<GSECommitmentInfo>());
      this.saveSearch();
      this.ctlLoanList.DataModified = false;
      this.modified = false;
    }

    private void commitProductNameChanges()
    {
      this.poolTrade.ProductNames.Clear();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvPNs.Items)
        this.poolTrade.ProductNames.Add(gvItem.Tag as FannieMaeProduct);
      this.modified = false;
    }

    private void saveTradeInfo()
    {
      int tradeId1 = this.poolTrade.TradeID;
      if (tradeId1 < 0)
      {
        tradeId1 = Session.MbsPoolManager.CreateTrade(this.poolTrade);
      }
      else
      {
        try
        {
          Session.MbsPoolManager.UpdateTrade(this.poolTrade, false, true);
        }
        catch (TradeNotUpdateException ex)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return;
        }
      }
      this.ctlSecurityTradeList.Save();
      this.commitmentListScreen.Save();
      this.poolTrade = Session.MbsPoolManager.GetTrade(tradeId1);
      this.assignments.ApplyNewTradeID(tradeId1);
      TradeAssignmentByTradeBase[] assigments = SecurityTradeAssignmentListScreen.GetAssigments(this.poolTrade.TradeID);
      if (assigments != null)
      {
        foreach (TradeAssignmentByTradeBase assignmentByTradeBase in assigments)
        {
          int? assigneeTradeId = assignmentByTradeBase.AssigneeTradeID;
          if (assigneeTradeId.HasValue)
          {
            ISecurityTradeManager securityTradeManager = Session.SecurityTradeManager;
            assigneeTradeId = assignmentByTradeBase.AssigneeTradeID;
            int tradeId2 = assigneeTradeId.Value;
            Session.SecurityTradeManager.UpdateTradeAfterAssignLoanTrade(securityTradeManager.GetTrade(tradeId2));
          }
        }
      }
      this.ResetOriginalTradeData();
      this.assignments.WritePendingChangesToServer(this.RemovePendingLoanFromOtherTrades);
    }

    private bool commitTradeAssignments(bool forceUpdateOfAllLoans, bool selectedLoans)
    {
      try
      {
        List<GSECommitmentInfo> list = ((IEnumerable<TradeAssignmentByTradeBase>) this.GseCommitmentAssignments).Select<TradeAssignmentByTradeBase, GSECommitmentInfo>((System.Func<TradeAssignmentByTradeBase, GSECommitmentInfo>) (c => ((GSECommitmentAssignment) c).Trade)).ToList<GSECommitmentInfo>();
        foreach (MbsPoolLoanAssignment assignment in this.assignments)
        {
          MbsPoolLoanAssignment a = assignment;
          if (!string.IsNullOrEmpty(a.CommitmentContractNumber) && !string.IsNullOrEmpty(a.ProductName) && list.Any<GSECommitmentInfo>((System.Func<GSECommitmentInfo, bool>) (g => g.ContractNumber == a.CommitmentContractNumber)))
          {
            a.GuarantyFee = this.poolTrade.GetGuaranteeFee(list.Where<GSECommitmentInfo>((System.Func<GSECommitmentInfo, bool>) (g => g.ContractNumber == a.CommitmentContractNumber)).First<GSECommitmentInfo>(), a.ProductName);
            a.CPA = this.poolTrade.GetCPA(list.Where<GSECommitmentInfo>((System.Func<GSECommitmentInfo, bool>) (g => g.ContractNumber == a.CommitmentContractNumber)).FirstOrDefault<GSECommitmentInfo>(), a.ProductName);
          }
        }
        MbsPoolLoanAssignmentManager assignments = this.assignments;
        if (selectedLoans)
        {
          List<string> guids = ((IEnumerable<PipelineInfo>) this.ctlLoanList.GetSelectedAndPendingLoans()).Select<PipelineInfo, string>((System.Func<PipelineInfo, string>) (t => t.GUID)).ToList<string>();
          assignments.loans = ((IEnumerable<MbsPoolLoanAssignment>) assignments.GetAllAssignedPendingLoans()).Where<MbsPoolLoanAssignment>((System.Func<MbsPoolLoanAssignment, bool>) (t => guids.Contains(t.Guid))).ToDictionary<MbsPoolLoanAssignment, string, MbsPoolLoanAssignment>((System.Func<MbsPoolLoanAssignment, string>) (t => t.Guid), (System.Func<MbsPoolLoanAssignment, MbsPoolLoanAssignment>) (t => t));
          forceUpdateOfAllLoans = true;
        }
        new MbsPoolProcesses((ITradeEditor) this).Execute(MbsPoolProcesses.ActionType.Commit, this.poolTrade, assignments, forceUpdateOfAllLoans || this.requiresLoanUpdates(), this.poolTrade.WeightedAvgPrice);
        this.loanUpdatesRequired = false;
        return true;
      }
      catch (Exception ex)
      {
        Tracing.Log(FannieMaePEPoolEditor.sw, this.className, TraceLevel.Error, "Error applying loan status: " + (object) ex);
        int num = (int) Utils.Dialog((IWin32Window) this, "Unexpected error while attempting to update loans: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
    }

    private void commitFannieMaeProductNames(MbsPoolInfo info)
    {
      info.ProductNames = new FannieMaeProducts();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvPNs.Items)
      {
        FannieMaeProduct pairOff = new FannieMaeProduct(gvItem.Index, gvItem.SubItems[0].Text, gvItem.SubItems[1].Text, gvItem.SubItems[2].Text, new TradeAdvancedPricingItems(), new MbsPoolBuyUpDownItems());
        info.ProductNames.Add(pairOff);
      }
    }

    public List<string> GetFannieMaeProducts()
    {
      List<string> fannieMaeProducts = new List<string>();
      if (this.gvPNs.Items.Count > 0)
      {
        fannieMaeProducts = this.gvPNs.Items.Select<GVItem, string>((System.Func<GVItem, string>) (x => x.SubItems[0].ToString())).ToList<string>();
      }
      else
      {
        foreach (DataRow row in (InternalDataCollectionBase) Session.SessionObjects.ConfigurationManager.GetFannieMaeProductNames().Rows)
          fannieMaeProducts.Add(row[0].ToString());
      }
      fannieMaeProducts.Insert(0, "");
      return fannieMaeProducts;
    }

    public bool PreValidateCommit() => this.prevalidateCommit();

    private bool prevalidateCommit()
    {
      return this.ctlFieldEditor.ValidateChanges(true) && this.commitmentListScreen.ValidateChanges() && this.ctlSecurityTradeList.ValidateChanges() && this.ctlBuyUpDownEditor.ValidateBuyUpDown();
    }

    private bool validateTradeData()
    {
      string str = this.txtPoolID.Text.Trim();
      if (str.Length == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must enter a Pool ID for this MBS Pool before saving.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return false;
      }
      if (string.Compare(str, this.originalTradeName, true) != 0 && Session.MbsPoolManager.GetTradeByName(str) != null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "A MBS Pool with the Pool ID '" + str + "' already exists. You must enter a unique Pool ID for this pool.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return false;
      }
      string errMsg;
      return (!this.areIneligibleLoansAssigned(out errMsg) || Utils.Dialog((IWin32Window) this, errMsg, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes) && (this.poolTrade.Pricing.SimplePricingItems.Count <= 0 || !this.areUnpricedLoansAssigned() || Utils.Dialog((IWin32Window) this, "The MBS Pool has one or more loans assigned to it for which pricing cannot be determined based on your current pricing setup. Do you want to continue to save this pool?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes);
    }

    private void recalculateProfitability() => this.loadProfitabilityData();

    private void loadHistory(MbsPoolHistoryItem[] historyItems)
    {
      this.btnExportHistory.Enabled = false;
      this.gvHistory.Items.Clear();
      if (historyItems == null)
        return;
      foreach (MbsPoolHistoryItem historyItem in historyItems)
        this.gvHistory.Items.Add(this.createTradeHistoryListItem(historyItem));
      this.btnExportHistory.Enabled = this.gvHistory.Items.Count > 0;
    }

    private GVItem createTradeHistoryListItem(MbsPoolHistoryItem historyItem)
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

    private void loadFieldEditor(PipelineInfo[] currentInTradeLoans)
    {
      if (this.poolTrade == null)
        return;
      this.ctlLoanList.GetAssignedLoanListFields();
      MbsPoolFieldsEditorBase fieldsEditorBase = (MbsPoolFieldsEditorBase) new MbsPoolFannieMaeFieldsEditor(currentInTradeLoans, MbsPoolMortgageType.FannieMaePE);
      fieldsEditorBase.ReadOnly = this.readOnly;
      this.ctlFieldEditor = fieldsEditorBase;
      this.ctlFieldEditor.PoolTrade = this.poolTrade;
      this.ctlFieldEditor.LoadTradeData();
      this.ctlFieldEditor.ValueChanged += new ValueChangedEventHandler(this.ctlFieldEditor_ValueChanged);
      this.ctlFieldEditor.Dock = DockStyle.Fill;
      this.pnlEditor.Controls.Clear();
      this.pnlEditor.Controls.Add((Control) fieldsEditorBase);
    }

    private void loadSecurityTradeList()
    {
      if (this.poolTrade == null)
        return;
      this.ctlSecurityTradeList.RefreshData(SecurityTradeAssignmentListScreen.GetAssigments(this.poolTrade.TradeID), SecurityTradeAssignmentListScreen.GetUnassignedAssigments(this.poolTrade.TradeID));
    }

    private void LoadGSECommitmentsList()
    {
      if (this.poolTrade == null)
        return;
      this.GseCommitmentAssignments = GSEcmtAssignmentListScreen.GetAssigments(this.poolTrade.TradeID);
      this.commitmentListScreen.RefreshData(this.GseCommitmentAssignments, GSEcmtAssignmentListScreen.GetUnassignedAssigments(this.poolTrade.TradeID));
    }

    private void loadProductNamesList()
    {
      this.gvPNs.Items.Clear();
      if (this.poolTrade == null || this.poolTrade.ProductNames == null)
        return;
      this.gvPNs.Items.AddRange(this.productGrid.ConvertProductName(this.poolTrade.ProductNames));
    }

    private void loadSearch()
    {
      if (this.poolTrade.Filter == null)
      {
        this.switchToSimpleMode(new SimpleTradeFilter());
        this.ctlSimpleSearch.DataModified = false;
      }
      else if (this.poolTrade.Filter.FilterType == TradeFilterType.Advanced)
      {
        this.switchToAdvancedMode(this.poolTrade.Filter.GetAdvancedFilter());
        this.ctlSimpleSearch.DataModified = false;
      }
      else
      {
        this.switchToSimpleMode(this.poolTrade.Filter.GetSimpleFilter());
        this.ctlAdvancedSearch.DataModified = false;
      }
      if (this.poolTrade.Filter == null || !this.ctlLoanList.ViewEligibleChecked || this.poolTrade.Filter.DataLayout == null)
        return;
      this.ctlLoanList.ClearCurrentEligibilityCursor();
      this.ctlLoanList.ValidateTableLayout(this.poolTrade.Filter.DataLayout);
      this.ctlLoanList.ApplyLayout(this.poolTrade.Filter.DataLayout, false);
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

    private void saveSearch()
    {
      this.poolTrade.Filter = this.getCurrentFilter();
      this.ctlAdvancedSearch.DataModified = false;
      this.ctlSimpleSearch.DataModified = false;
    }

    private bool isSearchModified()
    {
      return this.cboSearchType.SelectedIndex == 0 ? this.ctlSimpleSearch.DataModified : this.ctlAdvancedSearch.DataModified;
    }

    private void addSearchButtonsToControls()
    {
      this.vsAdvSearch.Parent = (Control) null;
      this.ctlAdvancedSearch.AddControlToHeader((Control) this.vsAdvSearch);
      this.btnSavedSearchesAdv.Parent = (Control) null;
      this.ctlAdvancedSearch.AddControlToHeader((Control) this.btnSavedSearchesAdv);
      this.btnSavedSearchesSimple.Parent = (Control) null;
      this.ctlSimpleSearch.AddControlToHeader((Control) this.btnSavedSearchesSimple);
    }

    private void loadLoans(PipelineInfo[] pinfos, bool updateLoansTab = true)
    {
      if (pinfos == null)
        pinfos = new PipelineInfo[0];
      this.assignments = new MbsPoolLoanAssignmentManager(Session.SessionObjects, this.poolTrade.TradeID, pinfos);
      if (!updateLoansTab)
        return;
      this.ctlLoanList.RefreshData(pinfos);
    }

    private void loadTradeData()
    {
      if (this.poolTrade == null)
        return;
      this.loading = true;
      MbsPoolEditorScreenData editorScreenData = Session.MbsPoolManager.GetTradeEditorScreenData(this.poolTrade.TradeID, this.ctlLoanList.GetAssignedLoanListFields(), false);
      this.defaultMortgageType = this.poolTrade.PoolMortgageType;
      this.txtPoolMortgageType.Text = this.poolTrade.PoolMortgageType.ToDescription();
      this.txtPoolMortgageType.Enabled = false;
      this.lblTradeName.Text = this.poolTrade.TradeID > 0 ? "MBS Pool " + this.poolTrade.Name : "New MBS Pool";
      this.txtPoolID.Text = this.poolTrade.Name;
      this.txtPoolNumber.Text = this.poolTrade.PoolNumber;
      this.txtSuffixID.Text = this.poolTrade.SuffixID;
      this.txtCUSIP.Text = this.poolTrade.CUSIP;
      this.cmbMortgageType.Text = this.poolTrade.MortgageType;
      this.cmbAmortizationType.Text = this.poolTrade.AmortizationType;
      this.txtTerm.Text = this.poolTrade.Term.ToString("#0;;\"\"");
      this.txtAmount.Text = this.poolTrade.TradeAmount.ToString("#,##0;;\"\"");
      this.cmbCommitmentType.Text = this.poolTrade.CommitmentType;
      this.cmbTradeDesc.Text = this.poolTrade.TradeDescription;
      this.txtCoupon.Text = this.poolTrade.Coupon.ToString("#,##0.00000;;\"\"");
      this.txtWeightedAvgPrice.Text = this.poolTrade.WeightedAvgPrice.ToString("0.00000000;;\"\"");
      this.lbtnWeightedAvgprice.Locked = this.poolTrade.WeightedAvgPriceLocked;
      this.txtServicer.Text = this.poolTrade.Servicer;
      this.dtCommitment.Value = this.poolTrade.CommitmentDate;
      this.dtInvestorDelivery.Value = this.poolTrade.InvestorDeliveryDate;
      this.dtInvestorDelivery.Value = this.poolTrade.InvestorDeliveryDate;
      this.dtEarlyDelivery.Value = this.poolTrade.EarlyDeliveryDate;
      this.dtTargetDelivery.Value = this.poolTrade.TargetDeliveryDate;
      this.dtActualDelivery.Value = this.poolTrade.ShipmentDate;
      this.dtPurchase.Value = this.poolTrade.PurchaseDate;
      this.dtSettlement.Value = this.poolTrade.SettlementDate;
      this.dtNotification.Value = this.poolTrade.NotificationDate;
      switch (this.poolTrade.ServicingType)
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
      if (string.IsNullOrEmpty(this.poolTrade.SettlementMonth))
        this.cmbSettlementMonth.SelectedIndex = -1;
      else
        this.cmbSettlementMonth.Text = this.poolTrade.SettlementMonth;
      this.txtNotes.Text = this.poolTrade.Notes;
      this.loadInvestorData();
      this.loadSecurityTradeList();
      this.LoadGSECommitmentsList();
      this.loadSearch();
      this.loadProductNamesList();
      this.poolTrade.Pricing.IsAdvancedPricing = true;
      this.msrPricingEditor1.PricingItems = this.poolTrade.Pricing.MSRPricingItems;
      this.refreshPricing();
      this.tpPricing_Resize((object) null, (EventArgs) null);
      this.loadLoans(editorScreenData.AssignedLoans);
      this.loadFieldEditor(this.assignments.GetAssignedPipelineData());
      this.loadHistory(editorScreenData.TradeHistory);
      this.ResetOriginalTradeData();
      this.ReadOnly = this.poolTrade.Status == TradeStatus.Archived || this.poolTrade.Status == TradeStatus.Pending;
      this.MakePending(this.poolTrade.Status == TradeStatus.Pending);
      this.loading = false;
      this.modified = false;
      if (!this.poolTrade.IsCloned)
        return;
      this.modified = this.poolTrade.TradeID <= 0;
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
        this.commitmentListScreen.ReadOnly = true;
        this.ctlSecurityTradeList.ReadOnly = true;
        if (this.isTradeLoanUpdateEnabled)
        {
          this.subscribeEventHandler();
        }
        else
        {
          if (this.refreshTimer.Enabled)
            return;
          Tracing.Log(FannieMaePEPoolEditor.sw, this.className, TraceLevel.Info, "Starting timer for loan trade");
          this.refreshTimer.Start();
        }
      }
      else
      {
        this.gradientPanel1.GradientColor1 = Color.FromArgb(81, 123, 184);
        this.gradientPanel1.GradientColor2 = Color.FromArgb(167, 201, 239);
        this.pictPending.Visible = false;
        this.commitmentListScreen.ReadOnly = false;
        this.ctlSecurityTradeList.ReadOnly = false;
        this.unSubscribeEventHandler();
        if (!this.refreshTimer.Enabled)
          return;
        this.stopTimer();
      }
    }

    private void loadInvestorData()
    {
      this.txtInvestor.Text = this.poolTrade.InvestorName;
      this.ctlContactInfo.CurrentInvestor = this.poolTrade.Investor;
      this.ctlContactInfo.CurrentAssignee = this.poolTrade.Assignee;
    }

    private void refreshConfigurableFieldOptions(bool isInit = true)
    {
      ArrayList arrayList = new ArrayList();
      if (isInit)
      {
        arrayList = Session.ConfigurationManager.GetSecondaryFields(SecondaryFieldTypes.MortgageType);
      }
      else
      {
        arrayList.Clear();
        arrayList.AddRange((ICollection) this.cmbMortgageType.Items);
      }
      this.cmbMortgageType.Items.Clear();
      if (arrayList != null)
      {
        foreach (string str in arrayList)
          this.cmbMortgageType.Items.Add((object) str);
      }
      if (isInit)
      {
        arrayList = Session.ConfigurationManager.GetSecondaryFields(SecondaryFieldTypes.AmortizationType);
      }
      else
      {
        arrayList.Clear();
        arrayList.AddRange((ICollection) this.cmbAmortizationType.Items);
      }
      this.cmbAmortizationType.Items.Clear();
      if (arrayList != null)
      {
        foreach (string str in arrayList)
          this.cmbAmortizationType.Items.Add((object) str);
      }
      if (isInit)
      {
        arrayList = Session.ConfigurationManager.GetSecondaryFields(SecondaryFieldTypes.CommitmentTypeOption);
      }
      else
      {
        arrayList.Clear();
        arrayList.AddRange((ICollection) this.cmbCommitmentType.Items);
      }
      this.cmbCommitmentType.Items.Clear();
      if (arrayList != null)
      {
        foreach (string str in arrayList)
          this.cmbCommitmentType.Items.Add((object) str);
      }
      if (isInit)
      {
        arrayList = Session.ConfigurationManager.GetSecondaryFields(SecondaryFieldTypes.TradeDescriptionOption);
      }
      else
      {
        arrayList.Clear();
        arrayList.AddRange((ICollection) this.cmbTradeDesc.Items);
      }
      this.cmbTradeDesc.Items.Clear();
      if (arrayList != null)
      {
        foreach (string str in arrayList)
          this.cmbTradeDesc.Items.Add((object) str);
      }
      this.cmbServicingType.Items.Clear();
      this.cmbServicingType.Items.Add((object) "Service Released");
      this.cmbServicingType.Items.Add((object) "Service Retained");
    }

    private void refreshLoanLists()
    {
      bool refreshLoans = false;
      if (this.lastPricingTradeInfo == null || !MbsPoolInfo.ComparePricing(this.lastPricingTradeInfo, this.poolTrade))
        refreshLoans = true;
      else if (!TradeFilter.CompareFilters(this.lastEvaluatedFilter, this.poolTrade.Filter))
        refreshLoans = true;
      else if (string.Compare(this.lastPricingTradeInfo.InvestorName, this.poolTrade.InvestorName, true) != 0)
        refreshLoans = true;
      this.ctlLoanList.RefreshLoanList(refreshLoans);
      this.lastEvaluatedFilter = this.poolTrade.Filter;
      this.lastPricingTradeInfo = new MbsPoolInfo(this.poolTrade);
    }

    private void refreshPricing()
    {
      this.ctlAdvancedPricing.PoolType = this.poolTrade.PoolMortgageType;
      this.ctlAdvancedPricing.Coupon = this.poolTrade.Coupon;
      this.ctlAdvancedPricing.WeightedAvgPrice = this.poolTrade.WeightedAvgPrice;
      this.ctlAdvancedPricing.GuaranteeFee = this.poolTrade.BaseGuarantyFee;
      this.ctlAdvancedPricing.MinServicingFee = this.poolTrade.MinServicingFee;
      this.ctlAdvancedPricing.MaxBU = this.poolTrade.MaxBU;
      this.InitGSECommitmentList();
      this.LoadPricingData();
      this.ctlAdvancedPricing.Refresh();
    }

    private void InitGSECommitmentList()
    {
      this.gvPricing.Items.Clear();
      this.cmbGSEProducts.Items.Clear();
      this.lblContractNumber.Text = "";
      this.ctlAdvancedPricing.PricingInfo = new TradeAdvancedPricingInfo();
      this.ctlAdvancedPricing.DisableButtons(true);
      TradeAssignmentByTradeBase[] commitmentAssignments = this.GseCommitmentAssignments;
      if (commitmentAssignments == null)
        return;
      foreach (GSECommitmentAssignment commitmentAssignment in commitmentAssignments)
      {
        GVItem gvItem = new GVItem();
        gvItem.SubItems[0].Value = (object) commitmentAssignment.Trade.ContractNumber;
        gvItem.SubItems[0].Tag = (object) false;
        GVSubItem subItem1 = gvItem.SubItems[1];
        DateTime dateTime = commitmentAssignment.Trade.CommitmentDate;
        string str1 = dateTime.ToString("MM/dd/yyyy");
        subItem1.Value = (object) str1;
        gvItem.SubItems[2].Value = (object) commitmentAssignment.Trade.CommitmentAmount.ToString("#,##0.00;;\"\"");
        GVSubItem subItem2 = gvItem.SubItems[3];
        string str2;
        if (!string.IsNullOrEmpty(commitmentAssignment.Trade.IssueMonth))
        {
          dateTime = Utils.ToDate(commitmentAssignment.Trade.IssueMonth);
          str2 = dateTime.ToString("MM/yyyy");
        }
        else
          str2 = "";
        subItem2.Value = (object) str2;
        gvItem.SubItems[4].Value = (object) string.Join(", ", commitmentAssignment.Trade.ProductNames.Select<FannieMaeProduct, string>((System.Func<FannieMaeProduct, string>) (p => p.DisplayName)));
        gvItem.Tag = (object) commitmentAssignment.Trade;
        this.gvPricing.Items.Add(gvItem);
      }
    }

    private bool areIneligibleLoansAssigned(out string errMsg)
    {
      string errMsg1 = string.Empty;
      MbsPoolFilterEvaluator evaluator = (MbsPoolFilterEvaluator) this.getCurrentFilter().CreateEvaluator(typeof (MbsPoolFilterEvaluator));
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
        if (!this.poolTrade.IsNoteRateAllowed(info))
          return true;
      }
      return false;
    }

    private void resetFieldDefs()
    {
      this.fieldDefs = LoanReportFieldDefs.GetFieldDefs(Session.DefaultInstance, LoanReportFieldFlags.DatabaseFieldsNoAudit);
      this.tradeAssLoanFieldDefs = TradeAssignedLoanFieldDefs.GetFieldDefs();
      this.ctlAdvancedSearch.FieldDefs = (ReportFieldDefs) this.fieldDefs;
    }

    private void setReadOnly()
    {
      this.txtPoolID.ReadOnly = this.readOnly;
      this.txtPoolNumber.ReadOnly = this.readOnly;
      this.txtSuffixID.ReadOnly = this.readOnly;
      this.txtCUSIP.ReadOnly = this.readOnly;
      this.cmbMortgageType.Enabled = !this.readOnly;
      this.cmbAmortizationType.Enabled = !this.readOnly;
      this.txtTerm.ReadOnly = this.readOnly;
      this.txtAmount.ReadOnly = this.readOnly;
      this.cmbCommitmentType.Enabled = !this.readOnly;
      this.cmbTradeDesc.Enabled = !this.readOnly;
      this.txtInvestor.ReadOnly = this.readOnly;
      this.cmbServicingType.Enabled = !this.readOnly;
      this.txtCoupon.ReadOnly = this.readOnly;
      this.lbtnWeightedAvgprice.Visible = !this.readOnly;
      this.txtWeightedAvgPrice.ReadOnly = this.readOnly || !this.readOnly && !this.lbtnWeightedAvgprice.Locked;
      this.cmbSettlementMonth.Enabled = !this.readOnly;
      this.txtServicer.ReadOnly = this.readOnly;
      this.standardIconButton1.Enabled = !this.readOnly;
      this.btnInvestorTemplate.Enabled = !this.readOnly;
      this.btnAddPN.Enabled = !this.readOnly;
      this.dtCommitment.ReadOnly = this.readOnly;
      this.dtInvestorDelivery.ReadOnly = this.readOnly;
      this.dtEarlyDelivery.ReadOnly = this.readOnly;
      this.dtTargetDelivery.ReadOnly = this.readOnly;
      this.dtActualDelivery.ReadOnly = this.readOnly;
      this.dtPurchase.ReadOnly = this.readOnly;
      this.dtSettlement.ReadOnly = this.readOnly;
      this.dtNotification.ReadOnly = this.readOnly;
      this.btnExportHistory.Enabled = this.poolTrade.Status != TradeStatus.Pending;
      this.ctlContactInfo.ReadOnly = this.readOnly;
      this.cboSearchType.Enabled = !this.readOnly;
      this.btnSavedSearchesSimple.Visible = !this.readOnly;
      this.btnSavedSearchesAdv.Visible = !this.readOnly;
      this.ctlSimpleSearch.ReadOnly = this.readOnly;
      this.ctlAdvancedSearch.ReadOnly = this.readOnly;
      this.ctlAdvancedPricing.ReadOnly = this.readOnly;
      this.msrPricingEditor1.ReadOnly = this.readOnly;
      this.ctlAdjustments.ReadOnly = this.readOnly;
      this.ctlLoanList.ReadOnly = this.readOnly;
      this.txtNotes.ReadOnly = this.readOnly;
      this.btnDateStamp.Visible = !this.readOnly;
      this.ctlFieldEditor.ReadOnly = this.readOnly;
      this.btnSave.Enabled = !this.readOnly;
    }

    private static System.Type getSortTypeForFieldDef(LoanReportFieldDef fieldDef)
    {
      if (fieldDef.FieldType == EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsDate)
        return typeof (ListViewDateSort);
      return fieldDef.FieldType == EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsNumeric ? typeof (ListViewCurrencySort) : typeof (ListViewTextCaseInsensitiveSort);
    }

    private void ResetOriginalTradeData()
    {
      this.originalTradeName = this.poolTrade.Name.Trim();
      this.originalContractId = this.poolTrade.ContractID;
    }

    private void loadProfitabilityData()
    {
    }

    private void placePricingTypeDDLToControls()
    {
      this.vsPricingType.Parent = (Control) null;
      this.ctlAdvancedPricing.AddControlToHeader((Control) this.vsPricingType, true);
    }

    private TradeFilter getCurrentFilter()
    {
      return this.cboSearchType.SelectedIndex == 0 ? new TradeFilter(this.ctlSimpleSearch.GetCurrentFilter(), this.ctlLoanList.GetCurrentLayout()) : new TradeFilter(this.ctlAdvancedSearch.GetCurrentFilter(), this.ctlLoanList.GetCurrentLayout());
    }

    private bool requiresLoanUpdates()
    {
      return this.LoanUpdatesRequired && this.assignments.GetAssignedLoans().Length != 0;
    }

    private void applyInvestor(Investor investor)
    {
      if (investor != null)
        this.poolTrade.Investor.CopyFrom(investor);
      else
        this.poolTrade.Investor.Clear();
      this.loadInvestorData();
      this.recalculateProfitability();
      if (investor == null)
        this.dtInvestorDelivery.Value = DateTime.MinValue;
      else if (this.dtCommitment.Value != DateTime.MinValue && this.dtInvestorDelivery.Value == DateTime.MinValue && investor.DeliveryTimeFrame > 0)
        this.dtInvestorDelivery.Value = this.dtCommitment.Value.AddDays((double) investor.DeliveryTimeFrame);
      this.modified = true;
      this.loanUpdatesRequired = true;
    }

    public void LoadTradeData() => this.loadTradeData();

    public string[] GetPricingAndEligibilityFields()
    {
      return this.poolTrade.GetPricingAndEligibilityFields();
    }

    public bool IsNoteRateAllowed(PipelineInfo pinfo) => this.poolTrade.IsNoteRateAllowed(pinfo);

    public string GetTradeStatusDescription(LoanToTradeAssignmentBase assignmentInfo)
    {
      MbsPoolLoanAssignment poolLoanAssignment = (MbsPoolLoanAssignment) assignmentInfo;
      string name = FannieMaePEPoolEditor.tradeStatusNameProvider.GetName((object) poolLoanAssignment.Status);
      if (poolLoanAssignment.Status == MbsPoolLoanStatus.Unassigned)
        return "Removed - Pending";
      if (poolLoanAssignment.Pending)
        name += " - Pending";
      return name;
    }

    public string GetLoanStatusDescription(object value)
    {
      return FannieMaePEPoolEditor.tradeStatusNameProvider.GetName((object) (MbsPoolLoanStatus) Utils.ParseInt(value, 1));
    }

    public void CommitChanges() => this.commitChanges();

    public bool SaveTrade(bool forceUpdateOfLoans, bool updateSelectedLoans)
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
        bool flag1 = true;
        bool flag2 = this.assignments.HasPendingChanges() || this.requiresLoanUpdates();
        if (forceUpdateOfLoans)
          flag1 = this.commitTradeAssignments(true, updateSelectedLoans);
        else if (flag2)
        {
          string str = "MBS Pool";
          if (this.poolTrade.PoolMortgageType == MbsPoolMortgageType.FannieMaePE)
            str = "Fannie Mae PE " + str;
          if (Utils.Dialog((IWin32Window) this, "The " + str + " has been saved successfully." + Environment.NewLine + "Would you like to update the loan files with these recent changes?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            flag1 = this.commitTradeAssignments(true, updateSelectedLoans);
        }
        this.loadTradeData();
        return flag1;
      }
      catch (ObjectNotFoundException ex)
      {
        Tracing.Log(FannieMaePEPoolEditor.sw, this.className, TraceLevel.Error, ex.ToString());
        if (ex.ObjectType == ObjectType.Trade)
        {
          int num1 = (int) Utils.Dialog((IWin32Window) this, "The current MBS Pool has been deleted and cannot be saved. All changes made to this pool will be lost.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "The MBS Pool could not be saved due to the following error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        this.ReadOnly = true;
        this.modified = false;
        this.loanUpdatesRequired = false;
        return false;
      }
      catch (Exception ex)
      {
        Tracing.Log(FannieMaePEPoolEditor.sw, this.className, TraceLevel.Error, ex.ToString());
        if (ex.Message.Contains("The loan has been assigned to another trade or pool."))
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "The MBS Pool is saved successfully. However, the loan is not assigned to the pool, because the loan has been assigned to another trade or pool.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          this.loadTradeData();
        }
        else
        {
          int num3 = (int) Utils.Dialog((IWin32Window) this, "The MBS Pool could not be saved due to the following error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        return false;
      }
      finally
      {
        Cursor.Current = Cursors.Default;
      }
    }

    public Decimal CalculatePriceIndex(PipelineInfo info)
    {
      return this.poolTrade.CalculatePriceIndex(info, true, 0M);
    }

    public Decimal CalculatePriceIndex(PipelineInfo info, Decimal securityPrice)
    {
      LoanToTradeAssignmentBase tradeAssignmentBase = this.GetAllAssignedPendingLoans().Where<LoanToTradeAssignmentBase>((System.Func<LoanToTradeAssignmentBase, bool>) (a => a.PipelineInfo.GUID == info.GUID)).First<LoanToTradeAssignmentBase>();
      string contractNumber = ((MbsPoolLoanAssignment) tradeAssignmentBase).CommitmentContractNumber;
      string productName = ((MbsPoolLoanAssignment) tradeAssignmentBase).ProductName;
      List<GSECommitmentInfo> list = ((IEnumerable<TradeAssignmentByTradeBase>) this.GseCommitmentAssignments).Select<TradeAssignmentByTradeBase, GSECommitmentInfo>((System.Func<TradeAssignmentByTradeBase, GSECommitmentInfo>) (c => ((GSECommitmentAssignment) c).Trade)).ToList<GSECommitmentInfo>();
      Decimal cpa = 0M;
      if (contractNumber != null && !string.IsNullOrEmpty(contractNumber))
        cpa = this.poolTrade.GetCPA(list.Where<GSECommitmentInfo>((System.Func<GSECommitmentInfo, bool>) (g => g.ContractNumber == contractNumber)).FirstOrDefault<GSECommitmentInfo>(), productName);
      return this.poolTrade.CalculatePriceIndex(info, true, securityPrice, this.CommitmentContractNumberLoanCal, this.ProductNameLoanCal, cpa);
    }

    public Decimal CalculateProfit(PipelineInfo info, Decimal securityPrice)
    {
      return this.poolTrade.CalculateProfit(info, securityPrice);
    }

    public ICursor GetEligibleLoanCursor(
      string[] fields,
      PipelineData dataToInclude,
      SortField[] sortFields,
      string[] excludedGuids,
      EligibleLoanFilterOption filterOption = EligibleLoanFilterOption.All)
    {
      return Session.MbsPoolManager.GetEligibleLoanCursor(this.poolTrade, fields, dataToInclude, sortFields, excludedGuids, false, filterOption);
    }

    public List<TradeLoanUpdateError> AssignLoanToTrade(PipelineInfo[] pinfos)
    {
      List<TradeLoanUpdateError> trade = new List<TradeLoanUpdateError>();
      foreach (PipelineInfo pinfo in pinfos)
      {
        try
        {
          string empty1 = string.Empty;
          string empty2 = string.Empty;
          if (this.ctlLoanList.LoansToAssign != null && this.ctlLoanList.LoansToAssign.ContainsKey(pinfo.LoanNumber))
          {
            GVItem gvItem = this.ctlLoanList.LoansToAssign[pinfo.LoanNumber];
            if (gvItem.SubItems.Count == 2 || gvItem.SubItems.Count == 3)
              empty1 = gvItem.SubItems[1].Value as string;
            if (gvItem.SubItems.Count == 3)
              empty2 = gvItem.SubItems[2].Value as string;
          }
          this.assignments.AssignLoan(pinfo, empty1, empty2);
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
      foreach (MbsPoolLoanAssignment assignment in this.assignments)
        tradeAssignments.Add((LoanToTradeAssignmentBase) assignment);
      return tradeAssignments;
    }

    public PipelineInfo[] GetLoanToTradeAssignedPipelineData()
    {
      return this.assignments == null ? new PipelineInfo[0] : this.assignments.GetAssignedPipelineData();
    }

    public int GetLoanToTradePendingAssignmentCount()
    {
      int pendingAssignmentCount = 0;
      if (this.assignments == null)
        return pendingAssignmentCount;
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
        return pendingShipmentCount;
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
        return pendingPurchaseCount;
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
        return pendingRemovalCount;
      foreach (MbsPoolLoanAssignment pendingLoan in this.assignments.GetPendingLoans())
      {
        if (pendingLoan.PendingStatus == MbsPoolLoanStatus.Unassigned)
          ++pendingRemovalCount;
      }
      return pendingRemovalCount;
    }

    public bool ValidateLoanToTradeAssignment(
      LoanToTradeAssignmentBase assignment,
      out string errMsg)
    {
      errMsg = string.Empty;
      if (((MbsPoolLoanAssignment) assignment).AssignedStatus < MbsPoolLoanStatus.Shipped)
        return true;
      errMsg = "One or more of the selected loans is already marked as Shipped or Purchased. Are you sure you want to remove these loans from the pool?";
      return false;
    }

    public string GetInvestorText() => this.txtInvestor.Text;

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
      if (this.assignments == null)
        return;
      this.assignments.ModifyLoanStatus(loanGuid, MbsPoolLoanStatus.Shipped);
    }

    public void MarkLoanToTradeAssignmentStatusToPurchasedPending(string loanGuid)
    {
      if (this.assignments == null)
        return;
      this.assignments.ModifyLoanStatus(loanGuid, MbsPoolLoanStatus.Purchased);
    }

    public void CommitLoanToTradeAssignments(bool forceUpdateOfAllLoans, bool selectedLoans)
    {
      this.commitTradeAssignments(forceUpdateOfAllLoans, selectedLoans);
      this.RefreshLoans();
    }

    public bool IsLoanToTradeAssignmentPending(LoanToTradeAssignmentBase assignment)
    {
      return ((MbsPoolLoanAssignment) assignment).Pending;
    }

    public List<LoanToTradeAssignmentBase> GetLoanToTradeAssignedLoans()
    {
      List<LoanToTradeAssignmentBase> tradeAssignedLoans = new List<LoanToTradeAssignmentBase>();
      if (this.assignments == null)
        return tradeAssignedLoans;
      foreach (MbsPoolLoanAssignment assignedLoan in this.assignments.GetAssignedLoans())
        tradeAssignedLoans.Add((LoanToTradeAssignmentBase) assignedLoan);
      return tradeAssignedLoans;
    }

    private void onFieldValueChanged(object sender, EventArgs e) => this.modified = true;

    private void onLoanUpdatableFieldValueChanged(object sender, EventArgs e)
    {
      this.loanUpdatesRequired = true;
      this.onFieldValueChanged(sender, e);
    }

    private void ctlFieldEditor_ValueChanged(object sender, ValueChangeEventArgs eventArgs)
    {
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

    private void cboSearchType_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.cboSearchType.SelectedIndex == 0 && this.ctlAdvancedSearch.Visible)
      {
        if (!this.loading && this.ctlAdvancedSearch.GetCurrentFilter().Count > 0 && Utils.Dialog((IWin32Window) this, "If you switch to Simple Search, you will lose all of your search criteria.", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK)
        {
          this.cboSearchType.SelectedIndex = 1;
          return;
        }
        this.switchToSimpleMode(new SimpleTradeFilter());
      }
      else if (this.cboSearchType.SelectedIndex == 1 && this.ctlSimpleSearch.Visible)
      {
        FieldFilterList filters = (FieldFilterList) null;
        if (!this.loading)
          filters = this.ctlSimpleSearch.GetCurrentFilter().ConvertToFilterList();
        this.switchToAdvancedMode(filters);
      }
      this.onFieldValueChanged(sender, e);
    }

    private void tpLoans_Resize(object sender, EventArgs e)
    {
      this.ctlLoanList.AdjustLoanPanelDisplay();
    }

    private void btnSave_Click(object sender, EventArgs e) => this.SaveTrade();

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

    private void btnSavedSearches_Click(object sender, EventArgs e)
    {
      using (TradeFilterTemplateSelector templateSelector = new TradeFilterTemplateSelector())
      {
        if (templateSelector.ShowDialog((IWin32Window) this) == DialogResult.Cancel)
          return;
        this.poolTrade.Filter = ((TradeFilterTemplate) templateSelector.GetSelectedTemplate()).Filter;
        this.modified = true;
        this.loadSearch();
      }
    }

    private void tpDetails_Resize(object sender, EventArgs e)
    {
      int num = this.pnlRight.Height - 5;
      this.pnlFilter.Top = 0;
      this.pnlRightBottom.Height = Math.Max(num / 3 * 2, this.pnlRightBottom.MinimumSize.Height);
      this.pnlSecurityTrades.Height = Math.Max(this.pnlRightBottomDetails.Height / 3, this.pnlSecurityTrades.MinimumSize.Height);
      this.pnlGseCommitments.Height = Math.Max(this.pnlRightBottomDetails.Height / 3, this.pnlGseCommitments.MinimumSize.Height);
      this.pnlSecurityTrades.Width = Math.Max(this.pnlRightBottomDetails.Width - 4, this.pnlSecurityTrades.MinimumSize.Width);
      this.pnlGseCommitments.Width = Math.Max(this.pnlRightBottomDetails.Width - 4, this.pnlGseCommitments.MinimumSize.Width);
      this.pnlRightBottom.Location = new Point(0, this.pnlFilter.Height + this.collapsibleSplitter2.Height);
      this.pnlGseCommitments.Location = new Point(0, 0);
      this.pnlSecurityTrades.Location = new Point(0, this.pnlGseCommitments.Height);
      this.grpAddress.Location = new Point(0, this.pnlGseCommitments.Height + this.pnlSecurityTrades.Height);
    }

    private void tpPricing_Resize(object sender, EventArgs e)
    {
      this.ctlAdvancedPricing.Top = this.ctlBuyUpDownEditor.Top = 0;
      this.ctlAdvancedPricing.Left = this.ctlAdjustments.Left = 0;
      this.ctlBuyUpDownEditor.Height = Math.Max(0, this.pnlPricing.Height - 5);
      this.ctlAdvancedPricing.Height = this.ctlAdjustments.Height = this.pnlPricingLeft.Height / 2;
      this.ctlAdjustments.Top = this.ctlAdvancedPricing.Bottom;
      int num = Math.Max(0, (Math.Max(0, this.pnlPricing.Width) - 5) / 2);
      this.ctlBuyUpDownEditor.Width = Math.Max(num, this.ctlBuyUpDownEditor.MinimumSize.Width);
      this.ctlAdvancedPricing.Width = Math.Max(0, num);
      this.ctlAdjustments.Width = this.ctlAdvancedPricing.Width;
      this.ctlBuyUpDownEditor.Left = this.ctlAdvancedPricing.Right + 5;
      this.ctlBuyUpDownEditor.Visible = true;
      if (this.cmbServicingType.Text == "Service Retained")
      {
        this.ctlAdjustments.Width /= 2;
        this.msrPricingEditor1.Left = this.ctlAdjustments.Width + 5;
        this.msrPricingEditor1.Top = this.ctlAdjustments.Top;
        this.msrPricingEditor1.Height = this.ctlAdjustments.Height;
        this.msrPricingEditor1.Width = this.ctlAdjustments.Width - 5;
        this.msrPricingEditor1.Visible = true;
      }
      else
        this.msrPricingEditor1.Visible = false;
      this.ctlAdvancedPricing.Refresh();
      this.ctlAdvancedPricing.Validate();
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
        Tracing.Log(FannieMaePEPoolEditor.sw, this.className, TraceLevel.Error, "Error during export: " + (object) ex);
        int num = (int) Utils.Dialog((IWin32Window) this, "An error occurred while attempting to export the loans to Microsoft Excel. Ensure that you have Excel installed and it is working properly.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void btnList_Click(object sender, EventArgs e)
    {
      TradeManagementConsole.Instance.CloseMbsPool();
    }

    private void tabTrade_DeSelecting(object sender, TabControlCancelEventArgs e)
    {
      if (sender == null)
        return;
      if (((TabControl) sender).SelectedTab == this.tpSetup)
        e.Cancel = !this.ctlFieldEditor.ValidateChanges(false);
      if (((TabControl) sender).SelectedTab == this.tpPricing)
        e.Cancel = !this.ctlBuyUpDownEditor.ValidateBuyUpDown();
      if (((TabControl) sender).SelectedTab != this.tpDetails)
        return;
      e.Cancel = !this.ctlSecurityTradeList.ValidateChanges();
    }

    private void tabTrade_SelectedIndexChanged(object sender, EventArgs e)
    {
      Cursor.Current = Cursors.WaitCursor;
      this.commitChanges();
      if (this.tabTrade.SelectedTab == this.tpDetails)
        this.recalculateProfitability();
      else if (this.tabTrade.SelectedTab == this.tpLoans)
      {
        this.refreshLoanLists();
        this.ctlLoanList.DisplayWithdrawnLoanMessage();
      }
      else if (this.tabTrade.SelectedTab == this.tpPricing)
        this.refreshPricing();
      else if (this.tabTrade.SelectedTab == this.tpSetup)
        this.loadFieldEditor(this.assignments.GetAssignedPipelineData());
      Cursor.Current = Cursors.Default;
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

    private void dtSettlement_ValueChanged(object sender, EventArgs e)
    {
      if (Utils.IsDate((object) this.dtSettlement.Value) && this.dtSettlement.Value != DateTime.MinValue)
        this.cmbSettlementMonth.Text = this.dtSettlement.Value.ToString("MMMM", (IFormatProvider) CultureInfo.InvariantCulture);
      else
        this.cmbSettlementMonth.Text = "";
      this.onFieldValueChanged(sender, e);
    }

    private void onCommitmentDateChanged(object sender, EventArgs e)
    {
      if (this.dtCommitment.Value != DateTime.MinValue && this.poolTrade.Investor.DeliveryTimeFrame > 0)
        this.dtInvestorDelivery.Value = this.dtCommitment.Value.AddDays((double) this.poolTrade.Investor.DeliveryTimeFrame);
      this.onLoanUpdatableFieldValueChanged(sender, e);
    }

    public void MenuClicked(ToolStripItem menuItem)
    {
      switch (string.Concat(menuItem.Tag))
      {
        case "MBE_SaveTrade":
          this.SaveTrade();
          break;
        case "MBE_ExitTrade":
          TradeManagementConsole.Instance.CloseMbsPool();
          break;
      }
    }

    public bool SetMenuItemState(ToolStripItem menuItem)
    {
      Control stateControl = (Control) null;
      switch (string.Concat(menuItem.Tag))
      {
        case "MBE_SaveTrade":
          stateControl = (Control) this.btnSave;
          break;
        case "MBE_ExitTrade":
          stateControl = (Control) this.btnList;
          break;
      }
      if (stateControl == null)
        return true;
      ClientCommonUtils.ApplyControlStateToMenu(menuItem, stateControl);
      return stateControl.Visible;
    }

    private void LoadPricingData()
    {
      if (string.IsNullOrEmpty(this.CommitmentContractNumber))
        return;
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvPricing.Items)
      {
        if (gvItem.SubItems[0].Value as string == this.CommitmentContractNumber)
          gvItem.Selected = true;
      }
    }

    private TableLayout getDemoPricingTableLayout()
    {
      TableLayout pricingTableLayout = new TableLayout();
      pricingTableLayout.AddColumn(new TableLayout.Column("GseCommitmentDetails.ContractNumber", "Contract Number", HorizontalAlignment.Left, 110));
      pricingTableLayout.AddColumn(new TableLayout.Column("GseCommitmentDetails.CommitmentDate", "Commitment Date", HorizontalAlignment.Left, 100));
      pricingTableLayout.AddColumn(new TableLayout.Column("GseCommitmentDetails.CommitmentAmount", "Commitment Amount", HorizontalAlignment.Right, 150));
      pricingTableLayout.AddColumn(new TableLayout.Column("GseCommitmentDetails.IssueMonth", "Issue Month", HorizontalAlignment.Right, 110));
      pricingTableLayout.AddColumn(new TableLayout.Column("GseCommitmentDetails.Products", "Product(s)", HorizontalAlignment.Right, 130));
      return pricingTableLayout;
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
        this.poolTrade.InvestorName = this.txtInvestor.Text;
    }

    private void tpSetup_Resize(object sender, EventArgs e)
    {
      this.tpSetup.Controls[0].Controls[0].Controls[0].Controls[0].Width = Math.Min(981, Math.Max(0, this.tpSetup.Width - 5));
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

    private void lbtnWeightedAvgprice_Click(object sender, EventArgs e)
    {
      this.lbtnWeightedAvgprice.Locked = !this.lbtnWeightedAvgprice.Locked;
      this.txtWeightedAvgPrice.ReadOnly = !this.lbtnWeightedAvgprice.Locked;
      if (this.lbtnWeightedAvgprice.Locked)
        return;
      this.ctlSecurityTradeList_ModifiedEvent((object) null, (EventArgs) null);
    }

    private void btnAddPN_Click(object sender, EventArgs e)
    {
      using (FannieMaeProductNames fannieMaeProductNames = new FannieMaeProductNames())
      {
        if (fannieMaeProductNames.ShowDialog((IWin32Window) this) == DialogResult.Yes)
        {
          DataTable fannieMaeProductNamesTable = this.productGrid.ConvertProductName(this.gvPNs.Items);
          fannieMaeProductNamesTable.PrimaryKey = new DataColumn[1]
          {
            fannieMaeProductNamesTable.Columns["ProductName"]
          };
          fannieMaeProductNames.Selected.PrimaryKey = new DataColumn[1]
          {
            fannieMaeProductNames.Selected.Columns["ProductName"]
          };
          fannieMaeProductNamesTable.Merge(fannieMaeProductNames.Selected);
          GVItem[] items = this.productGrid.ConvertProductName(fannieMaeProductNamesTable);
          this.gvPNs.Items.Clear();
          this.gvPNs.Items.AddRange(items);
        }
      }
      this.modified = true;
    }

    private void btnRemovePN_Click(object sender, EventArgs e)
    {
      while (this.gvPNs.SelectedItems.Count > 0)
        this.gvPNs.Items.Remove(this.gvPNs.SelectedItems[0]);
      this.modified = true;
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
      this.btnRemovePN.Enabled = this.gvPNs.SelectedItems.Count > 0 && !this.readOnly;
    }

    private void gvPricing_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.ctlAdvancedPricing.GuaranteeFee = this.poolTrade.BaseGuarantyFee;
      this.ctlAdvancedPricing.ServiceFee = this.poolTrade.FixedServicingFeePercent;
      this.poolTrade.CalcAllPricingDetails(((IEnumerable<TradeAssignmentByTradeBase>) this.GseCommitmentAssignments).Select<TradeAssignmentByTradeBase, GSECommitmentInfo>((System.Func<TradeAssignmentByTradeBase, GSECommitmentInfo>) (c => ((GSECommitmentAssignment) c).Trade)).ToList<GSECommitmentInfo>());
      if (this.gvPricing.SelectedItems.Count == 0)
      {
        this.ctlAdvancedPricing.DisableButtons(true);
        this.ClearDetailedPricingGrid();
        this.cmbGSEProducts.Items.Clear();
        this.lblContractNumber.Text = "";
        this.ctlBuyUpDownEditor.BuyUpDownItems = new MbsPoolBuyUpDownItems();
        this.ctlAdjustments.Adjustments = new TradePriceAdjustments();
        this.ctlAdvancedPricing.PricingInfo = new TradeAdvancedPricingInfo();
      }
      else
      {
        this.ctlAdvancedPricing.DisableButtons(false);
        GSECommitmentInfo tag = (GSECommitmentInfo) this.gvPricing.SelectedItems[0].Tag;
        this.ctlBuyUpDownEditor.BuyUpDownItems = new MbsPoolBuyUpDownItems();
        this.ctlAdvancedPricing.BuyUpDownItems = new MbsPoolBuyUpDownItems();
        this.lblContractNumber.Text = tag.ContractNumber;
        this.CommitmentContractNumber = this.lblContractNumber.Text;
        this.ctlAdjustments.Adjustments = tag.PriceAdjustments;
        string[] array = tag.ProductNames.Select<FannieMaeProduct, string>((System.Func<FannieMaeProduct, string>) (m => m.ProductName)).ToArray<string>();
        this.cmbGSEProducts.Items.Clear();
        this.cmbGSEProducts.Items.AddRange((object[]) array);
        if (this.cmbGSEProducts.Items.Count > 0)
        {
          if (!string.IsNullOrEmpty(this.ProductName))
            this.cmbGSEProducts.SelectedIndex = this.cmbGSEProducts.Items.IndexOf((object) this.ProductName);
          else
            this.cmbGSEProducts.SelectedIndex = 0;
          this.ctlAdvancedPricing.DisableButtons(false);
        }
        else
        {
          this.ClearDetailedPricingGrid();
          this.ctlAdvancedPricing.DisableButtons(true);
        }
      }
    }

    private void cmbGSEProducts_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.ProductName = this.cmbGSEProducts.Text;
      this.poolTrade.CalcAllPricingDetails(((IEnumerable<TradeAssignmentByTradeBase>) this.GseCommitmentAssignments).Select<TradeAssignmentByTradeBase, GSECommitmentInfo>((System.Func<TradeAssignmentByTradeBase, GSECommitmentInfo>) (c => ((GSECommitmentAssignment) c).Trade)).ToList<GSECommitmentInfo>());
      GSECommitmentInfo tag = (GSECommitmentInfo) this.gvPricing.SelectedItems[0].Tag;
      this.ctlAdvancedPricing.gseContractNum = tag.ContractNumber;
      this.ctlAdvancedPricing.productName = this.ProductName;
      this.ctlAdvancedPricing.GuaranteeFee = this.poolTrade.GetGuaranteeFee(tag, this.ProductName);
      this.ctlAdvancedPricing.CPA = this.poolTrade.GetCPA(tag, this.ProductName);
      this.ctlAdvancedPricing.PricingInfo.PricingItems = this.poolTrade.Pricing.AdvancedPricingInfo.PricingItems;
      if (tag.ProductNames.Any<FannieMaeProduct>((System.Func<FannieMaeProduct, bool>) (p => p.ProductName == this.ProductName)))
      {
        this.ctlBuyUpDownEditor.BuyUpDownItems = tag.ProductNames.Where<FannieMaeProduct>((System.Func<FannieMaeProduct, bool>) (p => p.ProductName == this.ProductName)).First<FannieMaeProduct>().BuyUpDownItems;
        this.ctlAdvancedPricing.BuyUpDownItems = this.ctlBuyUpDownEditor.BuyUpDownItems;
      }
      this.ctlAdvancedPricing.loadPricingInfo(tag.ContractNumber, this.ProductName);
    }

    private void ClearDetailedPricingGrid()
    {
      this.ctlAdvancedPricing.PricingInfo = new TradeAdvancedPricingInfo();
    }

    private void CommitDetailedPricingGrid()
    {
      if (this.gvPricing.SelectedItems == null || this.cmbGSEProducts.SelectedIndex <= -1)
        return;
      GSECommitmentInfo tag = (GSECommitmentInfo) this.gvPricing.SelectedItems[0].Tag;
      this.poolTrade.Pricing.AdvancedPricingInfo.PricingItems = this.ctlAdvancedPricing.PricingInfo.PricingItems;
    }

    public void SetNoteHistoryTab() => this.tabTrade.SelectedTab = this.tpHistory;

    public List<LoanToTradeAssignmentBase> GetAllAssignedPendingLoans()
    {
      List<LoanToTradeAssignmentBase> assignedPendingLoans = new List<LoanToTradeAssignmentBase>();
      if (this.assignments == null)
        return assignedPendingLoans;
      foreach (MbsPoolLoanAssignment assignedPendingLoan in this.assignments.GetAllAssignedPendingLoans())
        assignedPendingLoans.Add((LoanToTradeAssignmentBase) assignedPendingLoan);
      return assignedPendingLoans;
    }

    private void tabTrade_Selected(object sender, TabControlEventArgs e)
    {
      if (this.tabTrade.SelectedTab != this.tpPricing)
        return;
      this.tpPricing_Resize(sender, (EventArgs) e);
    }

    public Decimal GetOpenAmount() => throw new NotImplementedException();

    private void stopTimer()
    {
      if (this.poolTrade.status == TradeStatus.Pending)
        return;
      this.refreshTimer.Stop();
      int num = (int) new TradeLoanUpdateNotificationDialog(this.poolTrade.Name, " completed").ShowDialog();
    }

    private void Refresh_Tick(object sender, EventArgs e)
    {
      if (Session.MbsPoolManager.GetTradeStatus(this.poolTrade.TradeID) == TradeStatus.Pending)
        return;
      this.RefreshData(Session.MbsPoolManager.GetTrade(this.poolTrade.TradeID));
    }

    private void subscribeEventHandler()
    {
      if (this.poolTrade.status != TradeStatus.Pending || this.subscribed)
        return;
      Tracing.Log(FannieMaePEPoolEditor.sw, this.className, TraceLevel.Info, "Subscribe onTradeLoanUpdateNotification for MBS PE Pool");
      TradeLoanUpdateNotificationClientListener.TradeLoanUpdateNotificationActivity += new TradeLoanUpdateNotificationEventHandler(this.onTradeLoanUpdateNotification);
      this.subscribed = true;
    }

    private void unSubscribeEventHandler()
    {
      if (!this.subscribed)
        return;
      Tracing.Log(FannieMaePEPoolEditor.sw, this.className, TraceLevel.Info, "UnSubscribe onTradeLoanUpdateNotification for MBS PE Pool");
      TradeLoanUpdateNotificationClientListener.TradeLoanUpdateNotificationActivity -= new TradeLoanUpdateNotificationEventHandler(this.onTradeLoanUpdateNotification);
      this.subscribed = false;
    }

    private void onTradeLoanUpdateNotification(object sender, TradeLoanUpdateArgs eventArgs)
    {
      Tracing.Log(FannieMaePEPoolEditor.sw, this.className, TraceLevel.Info, "In onTradeLoanUpdateNotification for " + this.className);
      if (TradeManagementConsole.Instance.CurrentScreen.ToString() != this.className || !string.IsNullOrEmpty(eventArgs.TradeId) && int.Parse(eventArgs.TradeId) != this.poolTrade.TradeID)
      {
        Tracing.Log(FannieMaePEPoolEditor.sw, this.className, TraceLevel.Info, string.Format("Do not need to refresh MBS PE Pool. Current TradeId: {0}, Received TradeId: {1}, CorrelationId: {2}, Date: {3}", (object) this.poolTrade.TradeID, (object) eventArgs.TradeId, (object) eventArgs.CorrelationId, (object) eventArgs.Timestamp.ToString()));
      }
      else
      {
        if (!(eventArgs.TradeStatus != TradeStatus.Pending.ToString()))
          return;
        MbsPoolInfo newTrade = Session.MbsPoolManager.GetTrade(this.poolTrade.TradeID);
        this.BeginInvoke((Delegate) (() => this.RefreshData(newTrade)));
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FannieMaePEPoolEditor));
      this.toolTips = new ToolTip(this.components);
      this.standardIconButton1 = new StandardIconButton();
      this.btnInvestorTemplate = new StandardIconButton();
      this.btnRemovePN = new StandardIconButton();
      this.btnAddPN = new StandardIconButton();
      this.btnExportHistory = new StandardIconButton();
      this.btnList = new StandardIconButton();
      this.btnSave = new StandardIconButton();
      this.openFileDialog1 = new OpenFileDialog();
      this.pnlGridView = new Panel();
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
      this.pnlRightBottomDetails = new Panel();
      this.pnlSecurityTrades = new Panel();
      this.pnlGseCommitments = new Panel();
      this.grpAddress = new GroupContainer();
      this.ctlContactInfo = new InvestorAddressEditor();
      this.collapsibleSplitter1 = new CollapsibleSplitter();
      this.pnlLeft = new Panel();
      this.panel1 = new Panel();
      this.groupContainer1 = new GroupContainer();
      this.pnlLeftTop = new Panel();
      this.panel4 = new Panel();
      this.panel3 = new Panel();
      this.lbtnWeightedAvgprice = new FieldLockButton();
      this.txtServicer = new TextBox();
      this.lblServicer = new Label();
      this.lblCommitmentDate = new Label();
      this.dtCommitment = new DatePicker();
      this.lblPoolNumber = new Label();
      this.txtPoolNumber = new TextBox();
      this.lblNotificationDate = new Label();
      this.dtNotification = new DatePicker();
      this.cmbSettlementMonth = new ComboBox();
      this.lblSettlementMonth = new Label();
      this.lblSettlementDate = new Label();
      this.dtSettlement = new DatePicker();
      this.lblPercent = new Label();
      this.lblWeightedAveragePrice = new Label();
      this.txtWeightedAvgPrice = new TextBox();
      this.lblTerm = new Label();
      this.txtTerm = new TextBox();
      this.cmbAmortizationType = new ComboBox();
      this.lblAmortiztionType = new Label();
      this.cmbMortgageType = new ComboBox();
      this.lblMortgageType = new Label();
      this.lblCUSIP = new Label();
      this.txtCUSIP = new TextBox();
      this.lblSuffixID = new Label();
      this.txtSuffixID = new TextBox();
      this.cmbServicingType = new ComboBox();
      this.lblServiceType = new Label();
      this.lblInvestorDeliveryDate = new Label();
      this.cmbTradeDesc = new ComboBox();
      this.lblActualDeliveryDate = new Label();
      this.lblTargetDeliveryDate = new Label();
      this.lblTradeDescription = new Label();
      this.lblEarlyDeliveryDate = new Label();
      this.cmbCommitmentType = new ComboBox();
      this.lblPoolID = new Label();
      this.dtActualDelivery = new DatePicker();
      this.lblCommitmentType = new Label();
      this.txtPoolID = new TextBox();
      this.lblPurchaseDate = new Label();
      this.dtTargetDelivery = new DatePicker();
      this.dtPurchase = new DatePicker();
      this.dtEarlyDelivery = new DatePicker();
      this.lblInvestor = new Label();
      this.txtInvestor = new TextBox();
      this.dtInvestorDelivery = new DatePicker();
      this.lblCoupon = new Label();
      this.txtCoupon = new TextBox();
      this.lblPoolAmount = new Label();
      this.txtAmount = new TextBox();
      this.collapsibleSplitter3 = new CollapsibleSplitter();
      this.pnlProductName = new Panel();
      this.grpProductNames = new GroupContainer();
      this.gvPNs = new GridView();
      this.panel2 = new Panel();
      this.txtPoolMortgageType = new TextBox();
      this.label1 = new Label();
      this.tpSetup = new TabPage();
      this.pnlEditor = new Panel();
      this.tpPricing = new TabPage();
      this.pnlPricing = new Panel();
      this.pnlBottom = new Panel();
      this.pnlBottomGrid = new Panel();
      this.pnlPricingLeft = new Panel();
      this.msrPricingEditor1 = new MSRPricingEditor();
      this.ctlAdvancedPricing = new MBSPoolAdvancedPricingEditor();
      this.ctlAdjustments = new PriceAdjustmentListEditor();
      this.ctlBuyUpDownEditor = new MBSPoolBuyUpDownEditor();
      this.pnlChooseSpecificProduct = new Panel();
      this.lblContractNumber = new Label();
      this.label4 = new Label();
      this.cmbGSEProducts = new ComboBox();
      this.label3 = new Label();
      this.label2 = new Label();
      this.gvPricing = new GridView();
      this.vsPricingType = new VerticalSeparator();
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
      ((ISupportInitialize) this.btnRemovePN).BeginInit();
      ((ISupportInitialize) this.btnAddPN).BeginInit();
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
      this.pnlRightBottomDetails.SuspendLayout();
      this.grpAddress.SuspendLayout();
      this.pnlLeft.SuspendLayout();
      this.panel1.SuspendLayout();
      this.groupContainer1.SuspendLayout();
      this.pnlLeftTop.SuspendLayout();
      this.panel4.SuspendLayout();
      this.panel3.SuspendLayout();
      this.pnlProductName.SuspendLayout();
      this.grpProductNames.SuspendLayout();
      this.panel2.SuspendLayout();
      this.tpSetup.SuspendLayout();
      this.tpPricing.SuspendLayout();
      this.pnlPricing.SuspendLayout();
      this.pnlBottom.SuspendLayout();
      this.pnlBottomGrid.SuspendLayout();
      this.pnlPricingLeft.SuspendLayout();
      this.pnlChooseSpecificProduct.SuspendLayout();
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
      this.standardIconButton1.Location = new Point(246, 405);
      this.standardIconButton1.MouseDownImage = (Image) null;
      this.standardIconButton1.Name = "standardIconButton1";
      this.standardIconButton1.Size = new Size(16, 16);
      this.standardIconButton1.StandardButtonType = StandardIconButton.ButtonType.RolodexButton;
      this.standardIconButton1.TabIndex = 224;
      this.standardIconButton1.TabStop = false;
      this.toolTips.SetToolTip((Control) this.standardIconButton1, "Select Servicer");
      this.standardIconButton1.Click += new EventHandler(this.standardIconButton1_Click);
      this.btnInvestorTemplate.BackColor = Color.Transparent;
      this.btnInvestorTemplate.Location = new Point(253, 251);
      this.btnInvestorTemplate.MouseDownImage = (Image) null;
      this.btnInvestorTemplate.Name = "btnInvestorTemplate";
      this.btnInvestorTemplate.Size = new Size(16, 16);
      this.btnInvestorTemplate.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnInvestorTemplate.TabIndex = 13;
      this.btnInvestorTemplate.TabStop = false;
      this.toolTips.SetToolTip((Control) this.btnInvestorTemplate, "Select Investor");
      this.btnInvestorTemplate.Click += new EventHandler(this.btnInvestorTemplate_Click);
      this.btnRemovePN.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnRemovePN.BackColor = Color.Transparent;
      this.btnRemovePN.Enabled = false;
      this.btnRemovePN.Location = new Point(279, 5);
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
      this.btnAddPN.Location = new Point(259, 5);
      this.btnAddPN.MouseDownImage = (Image) null;
      this.btnAddPN.Name = "btnAddPN";
      this.btnAddPN.Size = new Size(16, 16);
      this.btnAddPN.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAddPN.TabIndex = 1;
      this.btnAddPN.TabStop = false;
      this.toolTips.SetToolTip((Control) this.btnAddPN, "Add Product");
      this.btnAddPN.Click += new EventHandler(this.btnAddPN_Click);
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
      this.toolTips.SetToolTip((Control) this.btnList, "Exit MBS Pool");
      this.btnList.Click += new EventHandler(this.btnList_Click);
      this.btnSave.BackColor = Color.Transparent;
      this.btnSave.Location = new Point(574, 3);
      this.btnSave.MouseDownImage = (Image) null;
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(16, 16);
      this.btnSave.StandardButtonType = StandardIconButton.ButtonType.SaveButton;
      this.btnSave.TabIndex = 6;
      this.btnSave.TabStop = false;
      this.toolTips.SetToolTip((Control) this.btnSave, "Save MBS Pool");
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.openFileDialog1.FileName = "openFileDialog1";
      this.pnlGridView.Location = new Point(0, 0);
      this.pnlGridView.Name = "pnlGridView";
      this.pnlGridView.Size = new Size(200, 100);
      this.pnlGridView.TabIndex = 0;
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
      this.tabTrade.Controls.Add((Control) this.tpSetup);
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
      this.tabTrade.Deselecting += new TabControlCancelEventHandler(this.tabTrade_DeSelecting);
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
      this.pnlRight.Location = new Point(306, 0);
      this.pnlRight.Name = "pnlRight";
      this.pnlRight.Size = new Size(735, 619);
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
      this.pnlFilter.Size = new Size(735, 276);
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
      this.ctlSimpleSearch.Size = new Size(735, 276);
      this.ctlSimpleSearch.TabIndex = 2;
      this.ctlSimpleSearch.Title = "Eligible Loans";
      this.ctlAdvancedSearch.AllowDynamicOperators = false;
      this.ctlAdvancedSearch.Dock = DockStyle.Fill;
      this.ctlAdvancedSearch.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.ctlAdvancedSearch.Location = new Point(0, 0);
      this.ctlAdvancedSearch.Name = "ctlAdvancedSearch";
      this.ctlAdvancedSearch.Size = new Size(735, 276);
      this.ctlAdvancedSearch.TabIndex = 3;
      this.ctlAdvancedSearch.Title = "Filters";
      this.collapsibleSplitter2.AnimationDelay = 20;
      this.collapsibleSplitter2.AnimationStep = 20;
      this.collapsibleSplitter2.BorderStyle3D = Border3DStyle.Flat;
      this.collapsibleSplitter2.ControlToHide = (Control) this.pnlRightBottom;
      this.collapsibleSplitter2.Dock = DockStyle.Bottom;
      this.collapsibleSplitter2.ExpandParentForm = false;
      this.collapsibleSplitter2.Location = new Point(0, 276);
      this.collapsibleSplitter2.Name = "collapsibleSplitter2";
      this.collapsibleSplitter2.TabIndex = 1;
      this.collapsibleSplitter2.TabStop = false;
      this.collapsibleSplitter2.UseAnimations = false;
      this.collapsibleSplitter2.VisualStyle = VisualStyles.Encompass;
      this.pnlRightBottom.AutoScroll = true;
      this.pnlRightBottom.Controls.Add((Control) this.pnlRightBottomDetails);
      this.pnlRightBottom.Dock = DockStyle.Bottom;
      this.pnlRightBottom.Location = new Point(0, 279);
      this.pnlRightBottom.MinimumSize = new Size(0, 340);
      this.pnlRightBottom.Name = "pnlRightBottom";
      this.pnlRightBottom.Size = new Size(735, 340);
      this.pnlRightBottom.TabIndex = 0;
      this.pnlRightBottomDetails.AutoScroll = true;
      this.pnlRightBottomDetails.Controls.Add((Control) this.pnlSecurityTrades);
      this.pnlRightBottomDetails.Controls.Add((Control) this.pnlGseCommitments);
      this.pnlRightBottomDetails.Controls.Add((Control) this.grpAddress);
      this.pnlRightBottomDetails.Dock = DockStyle.Fill;
      this.pnlRightBottomDetails.Location = new Point(0, 0);
      this.pnlRightBottomDetails.Name = "pnlRightBottomDetails";
      this.pnlRightBottomDetails.Size = new Size(735, 340);
      this.pnlRightBottomDetails.TabIndex = 0;
      this.pnlSecurityTrades.Dock = DockStyle.Fill;
      this.pnlSecurityTrades.Location = new Point(0, 80);
      this.pnlSecurityTrades.MinimumSize = new Size(0, 80);
      this.pnlSecurityTrades.Name = "pnlSecurityTrades";
      this.pnlSecurityTrades.Padding = new Padding(0, 0, 0, 4);
      this.pnlSecurityTrades.Size = new Size(735, 80);
      this.pnlSecurityTrades.TabIndex = 7;
      this.pnlGseCommitments.Dock = DockStyle.Top;
      this.pnlGseCommitments.Location = new Point(0, 0);
      this.pnlGseCommitments.MinimumSize = new Size(0, 80);
      this.pnlGseCommitments.Name = "pnlGseCommitments";
      this.pnlGseCommitments.Padding = new Padding(0, 0, 0, 4);
      this.pnlGseCommitments.Size = new Size(735, 80);
      this.pnlGseCommitments.TabIndex = 7;
      this.grpAddress.Controls.Add((Control) this.ctlContactInfo);
      this.grpAddress.Dock = DockStyle.Bottom;
      this.grpAddress.HeaderForeColor = SystemColors.ControlText;
      this.grpAddress.Location = new Point(0, 160);
      this.grpAddress.MinimumSize = new Size(0, 180);
      this.grpAddress.Name = "grpAddress";
      this.grpAddress.Size = new Size(735, 180);
      this.grpAddress.TabIndex = 6;
      this.grpAddress.Text = "Addresses";
      this.ctlContactInfo.AssigneeNameTextBoxText = "";
      this.ctlContactInfo.AutoScroll = true;
      this.ctlContactInfo.BackColor = Color.Transparent;
      this.ctlContactInfo.CurrentAssignee = (ContactInformation) null;
      this.ctlContactInfo.CurrentDealer = (ContactInformation) null;
      this.ctlContactInfo.CurrentInvestor = (Investor) null;
      this.ctlContactInfo.Dock = DockStyle.Fill;
      this.ctlContactInfo.Location = new Point(1, 26);
      this.ctlContactInfo.Name = "ctlContactInfo";
      this.ctlContactInfo.Padding = new Padding(1, 1, 0, 0);
      this.ctlContactInfo.ReadOnly = false;
      this.ctlContactInfo.Size = new Size(733, 153);
      this.ctlContactInfo.TabIndex = 0;
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
      this.pnlLeft.Size = new Size(303, 619);
      this.pnlLeft.TabIndex = 6;
      this.panel1.Controls.Add((Control) this.groupContainer1);
      this.panel1.Dock = DockStyle.Fill;
      this.panel1.Location = new Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(303, 619);
      this.panel1.TabIndex = 2;
      this.groupContainer1.Controls.Add((Control) this.pnlLeftTop);
      this.groupContainer1.Controls.Add((Control) this.panel2);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(303, 619);
      this.groupContainer1.TabIndex = 0;
      this.groupContainer1.Text = "MBS Pool Info";
      this.pnlLeftTop.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.pnlLeftTop.Controls.Add((Control) this.panel4);
      this.pnlLeftTop.Controls.Add((Control) this.collapsibleSplitter3);
      this.pnlLeftTop.Controls.Add((Control) this.pnlProductName);
      this.pnlLeftTop.Dock = DockStyle.Fill;
      this.pnlLeftTop.Location = new Point(1, 58);
      this.pnlLeftTop.Name = "pnlLeftTop";
      this.pnlLeftTop.Size = new Size(301, 560);
      this.pnlLeftTop.TabIndex = 28;
      this.panel4.AutoScroll = true;
      this.panel4.AutoSize = true;
      this.panel4.Controls.Add((Control) this.panel3);
      this.panel4.Dock = DockStyle.Fill;
      this.panel4.Location = new Point(0, 0);
      this.panel4.Name = "panel4";
      this.panel4.Size = new Size(301, 383);
      this.panel4.TabIndex = 37;
      this.panel3.AutoScroll = true;
      this.panel3.BackColor = Color.WhiteSmoke;
      this.panel3.Controls.Add((Control) this.lbtnWeightedAvgprice);
      this.panel3.Controls.Add((Control) this.standardIconButton1);
      this.panel3.Controls.Add((Control) this.txtServicer);
      this.panel3.Controls.Add((Control) this.lblServicer);
      this.panel3.Controls.Add((Control) this.lblCommitmentDate);
      this.panel3.Controls.Add((Control) this.dtCommitment);
      this.panel3.Controls.Add((Control) this.lblPoolNumber);
      this.panel3.Controls.Add((Control) this.txtPoolNumber);
      this.panel3.Controls.Add((Control) this.lblNotificationDate);
      this.panel3.Controls.Add((Control) this.dtNotification);
      this.panel3.Controls.Add((Control) this.cmbSettlementMonth);
      this.panel3.Controls.Add((Control) this.lblSettlementMonth);
      this.panel3.Controls.Add((Control) this.lblSettlementDate);
      this.panel3.Controls.Add((Control) this.dtSettlement);
      this.panel3.Controls.Add((Control) this.lblPercent);
      this.panel3.Controls.Add((Control) this.lblWeightedAveragePrice);
      this.panel3.Controls.Add((Control) this.txtWeightedAvgPrice);
      this.panel3.Controls.Add((Control) this.lblTerm);
      this.panel3.Controls.Add((Control) this.txtTerm);
      this.panel3.Controls.Add((Control) this.cmbAmortizationType);
      this.panel3.Controls.Add((Control) this.lblAmortiztionType);
      this.panel3.Controls.Add((Control) this.cmbMortgageType);
      this.panel3.Controls.Add((Control) this.lblMortgageType);
      this.panel3.Controls.Add((Control) this.lblCUSIP);
      this.panel3.Controls.Add((Control) this.txtCUSIP);
      this.panel3.Controls.Add((Control) this.lblSuffixID);
      this.panel3.Controls.Add((Control) this.txtSuffixID);
      this.panel3.Controls.Add((Control) this.cmbServicingType);
      this.panel3.Controls.Add((Control) this.lblServiceType);
      this.panel3.Controls.Add((Control) this.lblInvestorDeliveryDate);
      this.panel3.Controls.Add((Control) this.cmbTradeDesc);
      this.panel3.Controls.Add((Control) this.lblActualDeliveryDate);
      this.panel3.Controls.Add((Control) this.lblTargetDeliveryDate);
      this.panel3.Controls.Add((Control) this.lblTradeDescription);
      this.panel3.Controls.Add((Control) this.lblEarlyDeliveryDate);
      this.panel3.Controls.Add((Control) this.cmbCommitmentType);
      this.panel3.Controls.Add((Control) this.lblPoolID);
      this.panel3.Controls.Add((Control) this.dtActualDelivery);
      this.panel3.Controls.Add((Control) this.lblCommitmentType);
      this.panel3.Controls.Add((Control) this.txtPoolID);
      this.panel3.Controls.Add((Control) this.lblPurchaseDate);
      this.panel3.Controls.Add((Control) this.dtTargetDelivery);
      this.panel3.Controls.Add((Control) this.dtPurchase);
      this.panel3.Controls.Add((Control) this.dtEarlyDelivery);
      this.panel3.Controls.Add((Control) this.lblInvestor);
      this.panel3.Controls.Add((Control) this.txtInvestor);
      this.panel3.Controls.Add((Control) this.dtInvestorDelivery);
      this.panel3.Controls.Add((Control) this.btnInvestorTemplate);
      this.panel3.Controls.Add((Control) this.lblCoupon);
      this.panel3.Controls.Add((Control) this.txtCoupon);
      this.panel3.Controls.Add((Control) this.lblPoolAmount);
      this.panel3.Controls.Add((Control) this.txtAmount);
      this.panel3.Dock = DockStyle.Fill;
      this.panel3.Location = new Point(0, 0);
      this.panel3.Name = "panel3";
      this.panel3.Size = new Size(301, 383);
      this.panel3.TabIndex = 37;
      this.lbtnWeightedAvgprice.Location = new Point(131, 448);
      this.lbtnWeightedAvgprice.LockedStateToolTip = "Use Default Value";
      this.lbtnWeightedAvgprice.MaximumSize = new Size(16, 17);
      this.lbtnWeightedAvgprice.MinimumSize = new Size(16, 17);
      this.lbtnWeightedAvgprice.Name = "lbtnWeightedAvgprice";
      this.lbtnWeightedAvgprice.Size = new Size(16, 17);
      this.lbtnWeightedAvgprice.TabIndex = 225;
      this.lbtnWeightedAvgprice.UnlockedStateToolTip = "Enter Data Manually";
      this.lbtnWeightedAvgprice.Click += new EventHandler(this.lbtnWeightedAvgprice_Click);
      this.txtServicer.Location = new Point(131, 401);
      this.txtServicer.MaxLength = 64;
      this.txtServicer.Name = "txtServicer";
      this.txtServicer.Size = new Size(104, 20);
      this.txtServicer.TabIndex = 223;
      this.txtServicer.TextAlign = HorizontalAlignment.Right;
      this.txtServicer.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.lblServicer.AutoSize = true;
      this.lblServicer.Location = new Point(8, 406);
      this.lblServicer.Name = "lblServicer";
      this.lblServicer.Size = new Size(48, 14);
      this.lblServicer.TabIndex = 222;
      this.lblServicer.Text = "Servicer";
      this.lblCommitmentDate.AutoSize = true;
      this.lblCommitmentDate.Location = new Point(8, 74);
      this.lblCommitmentDate.Name = "lblCommitmentDate";
      this.lblCommitmentDate.Size = new Size(89, 14);
      this.lblCommitmentDate.TabIndex = 221;
      this.lblCommitmentDate.Text = "Commitment Date";
      this.dtCommitment.BackColor = SystemColors.Window;
      this.dtCommitment.Location = new Point(131, 71);
      this.dtCommitment.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.dtCommitment.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dtCommitment.Name = "dtCommitment";
      this.dtCommitment.Size = new Size(104, 22);
      this.dtCommitment.TabIndex = 31;
      this.dtCommitment.ToolTip = "";
      this.dtCommitment.Value = new DateTime(0L);
      this.dtCommitment.ValueChanged += new EventHandler(this.onCommitmentDateChanged);
      this.lblPoolNumber.AutoSize = true;
      this.lblPoolNumber.Location = new Point(8, 30);
      this.lblPoolNumber.Name = "lblPoolNumber";
      this.lblPoolNumber.Size = new Size(67, 14);
      this.lblPoolNumber.TabIndex = 61;
      this.lblPoolNumber.Text = "Pool Number";
      this.txtPoolNumber.Location = new Point(131, 27);
      this.txtPoolNumber.MaxLength = 6;
      this.txtPoolNumber.Name = "txtPoolNumber";
      this.txtPoolNumber.Size = new Size(138, 20);
      this.txtPoolNumber.TabIndex = 20;
      this.txtPoolNumber.TextChanged += new EventHandler(this.onLoanUpdatableFieldValueChanged);
      this.lblNotificationDate.AutoSize = true;
      this.lblNotificationDate.Location = new Point(8, 516);
      this.lblNotificationDate.Name = "lblNotificationDate";
      this.lblNotificationDate.Size = new Size(85, 14);
      this.lblNotificationDate.TabIndex = 59;
      this.lblNotificationDate.Text = "Notification Date";
      this.dtNotification.BackColor = SystemColors.Window;
      this.dtNotification.Location = new Point(131, 510);
      this.dtNotification.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.dtNotification.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dtNotification.Name = "dtNotification";
      this.dtNotification.Size = new Size(104, 22);
      this.dtNotification.TabIndex = 220;
      this.dtNotification.ToolTip = "";
      this.dtNotification.Value = new DateTime(0L);
      this.dtNotification.ValueChanged += new EventHandler(this.onFieldValueChanged);
      this.cmbSettlementMonth.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbSettlementMonth.Enabled = false;
      this.cmbSettlementMonth.FormattingEnabled = true;
      this.cmbSettlementMonth.Items.AddRange(new object[13]
      {
        (object) "",
        (object) "January",
        (object) "February",
        (object) "March",
        (object) "April",
        (object) "May",
        (object) "June",
        (object) "July",
        (object) "August",
        (object) "September",
        (object) "October",
        (object) "November",
        (object) "December"
      });
      this.cmbSettlementMonth.Location = new Point(131, 489);
      this.cmbSettlementMonth.Name = "cmbSettlementMonth";
      this.cmbSettlementMonth.Size = new Size(131, 22);
      this.cmbSettlementMonth.TabIndex = 210;
      this.cmbSettlementMonth.SelectedIndexChanged += new EventHandler(this.onFieldValueChanged);
      this.lblSettlementMonth.AutoSize = true;
      this.lblSettlementMonth.Location = new Point(8, 494);
      this.lblSettlementMonth.Name = "lblSettlementMonth";
      this.lblSettlementMonth.Size = new Size(89, 14);
      this.lblSettlementMonth.TabIndex = 56;
      this.lblSettlementMonth.Text = "Settlement Month";
      this.lblSettlementDate.AutoSize = true;
      this.lblSettlementDate.Location = new Point(8, 472);
      this.lblSettlementDate.Name = "lblSettlementDate";
      this.lblSettlementDate.Size = new Size(82, 14);
      this.lblSettlementDate.TabIndex = 54;
      this.lblSettlementDate.Text = "Settlement Date";
      this.dtSettlement.BackColor = SystemColors.Window;
      this.dtSettlement.Location = new Point(131, 467);
      this.dtSettlement.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.dtSettlement.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dtSettlement.Name = "dtSettlement";
      this.dtSettlement.Size = new Size(104, 22);
      this.dtSettlement.TabIndex = 200;
      this.dtSettlement.ToolTip = "";
      this.dtSettlement.Value = new DateTime(0L);
      this.dtSettlement.ValueChanged += new EventHandler(this.dtSettlement_ValueChanged);
      this.lblPercent.AutoSize = true;
      this.lblPercent.Location = new Point((int) byte.MaxValue, 449);
      this.lblPercent.Name = "lblPercent";
      this.lblPercent.Size = new Size(17, 14);
      this.lblPercent.TabIndex = 53;
      this.lblPercent.Text = "%";
      this.lblWeightedAveragePrice.AutoSize = true;
      this.lblWeightedAveragePrice.Location = new Point(8, 450);
      this.lblWeightedAveragePrice.Name = "lblWeightedAveragePrice";
      this.lblWeightedAveragePrice.Size = new Size(123, 14);
      this.lblWeightedAveragePrice.TabIndex = 51;
      this.lblWeightedAveragePrice.Text = "Weighted Average Price";
      this.txtWeightedAvgPrice.Location = new Point(146, 445);
      this.txtWeightedAvgPrice.MaxLength = 180;
      this.txtWeightedAvgPrice.Name = "txtWeightedAvgPrice";
      this.txtWeightedAvgPrice.Size = new Size(104, 20);
      this.txtWeightedAvgPrice.TabIndex = 191;
      this.txtWeightedAvgPrice.TabStop = false;
      this.txtWeightedAvgPrice.TextAlign = HorizontalAlignment.Right;
      this.txtWeightedAvgPrice.TextChanged += new EventHandler(this.onLoanUpdatableFieldValueChanged);
      this.lblTerm.AutoSize = true;
      this.lblTerm.Location = new Point(8, 164);
      this.lblTerm.Name = "lblTerm";
      this.lblTerm.Size = new Size(57, 14);
      this.lblTerm.TabIndex = 49;
      this.lblTerm.Text = "Term (yrs)";
      this.txtTerm.Location = new Point(131, 159);
      this.txtTerm.MaxLength = 2;
      this.txtTerm.Name = "txtTerm";
      this.txtTerm.Size = new Size(50, 20);
      this.txtTerm.TabIndex = 80;
      this.txtTerm.TextAlign = HorizontalAlignment.Right;
      this.txtTerm.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.cmbAmortizationType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbAmortizationType.DropDownWidth = 170;
      this.cmbAmortizationType.FormattingEnabled = true;
      this.cmbAmortizationType.Location = new Point(131, 137);
      this.cmbAmortizationType.MaxLength = 30;
      this.cmbAmortizationType.Name = "cmbAmortizationType";
      this.cmbAmortizationType.Size = new Size(139, 22);
      this.cmbAmortizationType.TabIndex = 70;
      this.cmbAmortizationType.SelectedIndexChanged += new EventHandler(this.onLoanUpdatableFieldValueChanged);
      this.lblAmortiztionType.AutoSize = true;
      this.lblAmortiztionType.Location = new Point(8, 142);
      this.lblAmortiztionType.Name = "lblAmortiztionType";
      this.lblAmortiztionType.Size = new Size(93, 14);
      this.lblAmortiztionType.TabIndex = 48;
      this.lblAmortiztionType.Text = "Amortization Type";
      this.cmbMortgageType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbMortgageType.FormattingEnabled = true;
      this.cmbMortgageType.Location = new Point(131, 115);
      this.cmbMortgageType.Name = "cmbMortgageType";
      this.cmbMortgageType.Size = new Size(139, 22);
      this.cmbMortgageType.TabIndex = 60;
      this.cmbMortgageType.SelectedIndexChanged += new EventHandler(this.onLoanUpdatableFieldValueChanged);
      this.lblMortgageType.AutoSize = true;
      this.lblMortgageType.Location = new Point(8, 118);
      this.lblMortgageType.Name = "lblMortgageType";
      this.lblMortgageType.Size = new Size(78, 14);
      this.lblMortgageType.TabIndex = 46;
      this.lblMortgageType.Text = "Mortgage Type";
      this.lblCUSIP.AutoSize = true;
      this.lblCUSIP.Location = new Point(8, 96);
      this.lblCUSIP.Name = "lblCUSIP";
      this.lblCUSIP.Size = new Size(36, 14);
      this.lblCUSIP.TabIndex = 43;
      this.lblCUSIP.Text = "CUSIP";
      this.txtCUSIP.Location = new Point(131, 93);
      this.txtCUSIP.MaxLength = 15;
      this.txtCUSIP.Name = "txtCUSIP";
      this.txtCUSIP.Size = new Size(139, 20);
      this.txtCUSIP.TabIndex = 40;
      this.txtCUSIP.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.lblSuffixID.AutoSize = true;
      this.lblSuffixID.Location = new Point(8, 52);
      this.lblSuffixID.Name = "lblSuffixID";
      this.lblSuffixID.Size = new Size(48, 14);
      this.lblSuffixID.TabIndex = 41;
      this.lblSuffixID.Text = "Suffix ID";
      this.txtSuffixID.Location = new Point(131, 49);
      this.txtSuffixID.MaxLength = 3;
      this.txtSuffixID.Name = "txtSuffixID";
      this.txtSuffixID.Size = new Size(50, 20);
      this.txtSuffixID.TabIndex = 30;
      this.txtSuffixID.TextChanged += new EventHandler(this.onLoanUpdatableFieldValueChanged);
      this.cmbServicingType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbServicingType.FormattingEnabled = true;
      this.cmbServicingType.Items.AddRange(new object[2]
      {
        (object) "Service Released",
        (object) "Service Retained"
      });
      this.cmbServicingType.Location = new Point(131, 379);
      this.cmbServicingType.Name = "cmbServicingType";
      this.cmbServicingType.Size = new Size(131, 22);
      this.cmbServicingType.TabIndex = 180;
      this.cmbServicingType.SelectedIndexChanged += new EventHandler(this.onFieldValueChanged);
      this.lblServiceType.AutoSize = true;
      this.lblServiceType.Location = new Point(8, 384);
      this.lblServiceType.Name = "lblServiceType";
      this.lblServiceType.Size = new Size(78, 14);
      this.lblServiceType.TabIndex = 36;
      this.lblServiceType.Text = "Servicing Type";
      this.lblInvestorDeliveryDate.AutoSize = true;
      this.lblInvestorDeliveryDate.Location = new Point(8, 274);
      this.lblInvestorDeliveryDate.Name = "lblInvestorDeliveryDate";
      this.lblInvestorDeliveryDate.Size = new Size(113, 14);
      this.lblInvestorDeliveryDate.TabIndex = 26;
      this.lblInvestorDeliveryDate.Text = "Investor Delivery Date";
      this.cmbTradeDesc.FormattingEnabled = true;
      this.cmbTradeDesc.Location = new Point(131, 225);
      this.cmbTradeDesc.Name = "cmbTradeDesc";
      this.cmbTradeDesc.Size = new Size(139, 22);
      this.cmbTradeDesc.TabIndex = 110;
      this.cmbTradeDesc.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.lblActualDeliveryDate.AutoSize = true;
      this.lblActualDeliveryDate.Location = new Point(8, 340);
      this.lblActualDeliveryDate.Name = "lblActualDeliveryDate";
      this.lblActualDeliveryDate.Size = new Size(105, 14);
      this.lblActualDeliveryDate.TabIndex = 8;
      this.lblActualDeliveryDate.Text = "Actual Delivery Date";
      this.lblTargetDeliveryDate.AutoSize = true;
      this.lblTargetDeliveryDate.Location = new Point(8, 318);
      this.lblTargetDeliveryDate.Name = "lblTargetDeliveryDate";
      this.lblTargetDeliveryDate.Size = new Size(104, 14);
      this.lblTargetDeliveryDate.TabIndex = 6;
      this.lblTargetDeliveryDate.Text = "Target Delivery Date";
      this.lblTradeDescription.AutoSize = true;
      this.lblTradeDescription.Location = new Point(8, 230);
      this.lblTradeDescription.Name = "lblTradeDescription";
      this.lblTradeDescription.Size = new Size(92, 14);
      this.lblTradeDescription.TabIndex = 24;
      this.lblTradeDescription.Text = "Trade Description";
      this.lblEarlyDeliveryDate.AutoSize = true;
      this.lblEarlyDeliveryDate.Location = new Point(8, 296);
      this.lblEarlyDeliveryDate.Name = "lblEarlyDeliveryDate";
      this.lblEarlyDeliveryDate.Size = new Size(98, 14);
      this.lblEarlyDeliveryDate.TabIndex = 4;
      this.lblEarlyDeliveryDate.Text = "Early Delivery Date";
      this.cmbCommitmentType.FormattingEnabled = true;
      this.cmbCommitmentType.Location = new Point(131, 203);
      this.cmbCommitmentType.Name = "cmbCommitmentType";
      this.cmbCommitmentType.Size = new Size(139, 22);
      this.cmbCommitmentType.TabIndex = 100;
      this.cmbCommitmentType.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.lblPoolID.AutoSize = true;
      this.lblPoolID.Location = new Point(8, 8);
      this.lblPoolID.Name = "lblPoolID";
      this.lblPoolID.Size = new Size(39, 14);
      this.lblPoolID.TabIndex = 0;
      this.lblPoolID.Text = "Pool ID";
      this.dtActualDelivery.BackColor = SystemColors.Window;
      this.dtActualDelivery.Location = new Point(131, 335);
      this.dtActualDelivery.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.dtActualDelivery.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dtActualDelivery.Name = "dtActualDelivery";
      this.dtActualDelivery.Size = new Size(104, 22);
      this.dtActualDelivery.TabIndex = 160;
      this.dtActualDelivery.ToolTip = "";
      this.dtActualDelivery.Value = new DateTime(0L);
      this.dtActualDelivery.ValueChanged += new EventHandler(this.onLoanUpdatableFieldValueChanged);
      this.lblCommitmentType.AutoSize = true;
      this.lblCommitmentType.Location = new Point(8, 208);
      this.lblCommitmentType.Name = "lblCommitmentType";
      this.lblCommitmentType.Size = new Size(90, 14);
      this.lblCommitmentType.TabIndex = 22;
      this.lblCommitmentType.Text = "Commitment Type";
      this.txtPoolID.Location = new Point(131, 5);
      this.txtPoolID.MaxLength = 64;
      this.txtPoolID.Name = "txtPoolID";
      this.txtPoolID.Size = new Size(138, 20);
      this.txtPoolID.TabIndex = 10;
      this.txtPoolID.TextChanged += new EventHandler(this.onLoanUpdatableFieldValueChanged);
      this.lblPurchaseDate.AutoSize = true;
      this.lblPurchaseDate.Location = new Point(8, 362);
      this.lblPurchaseDate.Name = "lblPurchaseDate";
      this.lblPurchaseDate.Size = new Size(78, 14);
      this.lblPurchaseDate.TabIndex = 0;
      this.lblPurchaseDate.Text = "Purchase Date";
      this.dtTargetDelivery.BackColor = SystemColors.Window;
      this.dtTargetDelivery.Location = new Point(131, 313);
      this.dtTargetDelivery.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.dtTargetDelivery.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dtTargetDelivery.Name = "dtTargetDelivery";
      this.dtTargetDelivery.Size = new Size(104, 22);
      this.dtTargetDelivery.TabIndex = 150;
      this.dtTargetDelivery.ToolTip = "";
      this.dtTargetDelivery.Value = new DateTime(0L);
      this.dtTargetDelivery.ValueChanged += new EventHandler(this.onLoanUpdatableFieldValueChanged);
      this.dtPurchase.BackColor = SystemColors.Window;
      this.dtPurchase.Location = new Point(131, 356);
      this.dtPurchase.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.dtPurchase.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dtPurchase.Name = "dtPurchase";
      this.dtPurchase.Size = new Size(104, 22);
      this.dtPurchase.TabIndex = 170;
      this.dtPurchase.ToolTip = "";
      this.dtPurchase.Value = new DateTime(0L);
      this.dtPurchase.ValueChanged += new EventHandler(this.onLoanUpdatableFieldValueChanged);
      this.dtEarlyDelivery.BackColor = SystemColors.Window;
      this.dtEarlyDelivery.Location = new Point(131, 291);
      this.dtEarlyDelivery.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.dtEarlyDelivery.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dtEarlyDelivery.Name = "dtEarlyDelivery";
      this.dtEarlyDelivery.Size = new Size(104, 22);
      this.dtEarlyDelivery.TabIndex = 140;
      this.dtEarlyDelivery.ToolTip = "";
      this.dtEarlyDelivery.Value = new DateTime(0L);
      this.dtEarlyDelivery.ValueChanged += new EventHandler(this.onFieldValueChanged);
      this.lblInvestor.AutoSize = true;
      this.lblInvestor.Location = new Point(8, 252);
      this.lblInvestor.Name = "lblInvestor";
      this.lblInvestor.Size = new Size(46, 14);
      this.lblInvestor.TabIndex = 14;
      this.lblInvestor.Text = "Investor";
      this.txtInvestor.Location = new Point(131, 247);
      this.txtInvestor.MaxLength = 64;
      this.txtInvestor.Name = "txtInvestor";
      this.txtInvestor.Size = new Size(117, 20);
      this.txtInvestor.TabIndex = 120;
      this.txtInvestor.Leave += new EventHandler(this.txtInvestor_Leave);
      this.txtInvestor.MouseDown += new MouseEventHandler(this.txtInvestor_MouseDown);
      this.txtInvestor.MouseLeave += new EventHandler(this.txtInvestor_Leave);
      this.dtInvestorDelivery.BackColor = SystemColors.Window;
      this.dtInvestorDelivery.Location = new Point(131, 269);
      this.dtInvestorDelivery.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.dtInvestorDelivery.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dtInvestorDelivery.Name = "dtInvestorDelivery";
      this.dtInvestorDelivery.Size = new Size(104, 22);
      this.dtInvestorDelivery.TabIndex = 130;
      this.dtInvestorDelivery.ToolTip = "";
      this.dtInvestorDelivery.Value = new DateTime(0L);
      this.dtInvestorDelivery.ValueChanged += new EventHandler(this.onLoanUpdatableFieldValueChanged);
      this.lblCoupon.AutoSize = true;
      this.lblCoupon.Location = new Point(8, 428);
      this.lblCoupon.Name = "lblCoupon";
      this.lblCoupon.Size = new Size(44, 14);
      this.lblCoupon.TabIndex = 6;
      this.lblCoupon.Text = "Coupon";
      this.txtCoupon.Location = new Point(131, 423);
      this.txtCoupon.MaxLength = 170;
      this.txtCoupon.Name = "txtCoupon";
      this.txtCoupon.Size = new Size(131, 20);
      this.txtCoupon.TabIndex = 190;
      this.txtCoupon.TextAlign = HorizontalAlignment.Right;
      this.txtCoupon.TextChanged += new EventHandler(this.onLoanUpdatableFieldValueChanged);
      this.lblPoolAmount.AutoSize = true;
      this.lblPoolAmount.Location = new Point(8, 186);
      this.lblPoolAmount.Name = "lblPoolAmount";
      this.lblPoolAmount.Size = new Size(66, 14);
      this.lblPoolAmount.TabIndex = 4;
      this.lblPoolAmount.Text = "Pool Amount";
      this.txtAmount.Location = new Point(131, 181);
      this.txtAmount.MaxLength = 12;
      this.txtAmount.Name = "txtAmount";
      this.txtAmount.Size = new Size(139, 20);
      this.txtAmount.TabIndex = 90;
      this.txtAmount.TextAlign = HorizontalAlignment.Right;
      this.txtAmount.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.collapsibleSplitter3.AnimationDelay = 20;
      this.collapsibleSplitter3.AnimationStep = 20;
      this.collapsibleSplitter3.BorderStyle3D = Border3DStyle.Flat;
      this.collapsibleSplitter3.ControlToHide = (Control) this.pnlProductName;
      this.collapsibleSplitter3.Dock = DockStyle.Bottom;
      this.collapsibleSplitter3.ExpandParentForm = false;
      this.collapsibleSplitter3.Location = new Point(0, 383);
      this.collapsibleSplitter3.Name = "collapsibleSplitter3";
      this.collapsibleSplitter3.TabIndex = 29;
      this.collapsibleSplitter3.TabStop = false;
      this.collapsibleSplitter3.UseAnimations = false;
      this.collapsibleSplitter3.VisualStyle = VisualStyles.Encompass;
      this.pnlProductName.Controls.Add((Control) this.grpProductNames);
      this.pnlProductName.Dock = DockStyle.Bottom;
      this.pnlProductName.Location = new Point(0, 386);
      this.pnlProductName.Name = "pnlProductName";
      this.pnlProductName.Size = new Size(301, 174);
      this.pnlProductName.TabIndex = 107;
      this.grpProductNames.Controls.Add((Control) this.gvPNs);
      this.grpProductNames.Controls.Add((Control) this.btnRemovePN);
      this.grpProductNames.Controls.Add((Control) this.btnAddPN);
      this.grpProductNames.Dock = DockStyle.Fill;
      this.grpProductNames.HeaderForeColor = SystemColors.ControlText;
      this.grpProductNames.Location = new Point(0, 0);
      this.grpProductNames.Name = "grpProductNames";
      this.grpProductNames.Size = new Size(301, 174);
      this.grpProductNames.TabIndex = 36;
      this.grpProductNames.Text = "Select Product Names";
      this.gvPNs.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "ProductName";
      gvColumn1.Text = "Column";
      gvColumn1.Width = 299;
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
      this.gvPNs.Size = new Size(299, 147);
      this.gvPNs.TabIndex = 105;
      this.gvPNs.SelectedIndexChanged += new EventHandler(this.gvPNs_SelectedIndexChanged);
      this.panel2.BackColor = Color.LightGray;
      this.panel2.Controls.Add((Control) this.txtPoolMortgageType);
      this.panel2.Controls.Add((Control) this.label1);
      this.panel2.Dock = DockStyle.Top;
      this.panel2.Location = new Point(1, 26);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(301, 32);
      this.panel2.TabIndex = 24;
      this.txtPoolMortgageType.Location = new Point(131, 5);
      this.txtPoolMortgageType.MaxLength = 64;
      this.txtPoolMortgageType.Name = "txtPoolMortgageType";
      this.txtPoolMortgageType.ReadOnly = true;
      this.txtPoolMortgageType.Size = new Size(138, 20);
      this.txtPoolMortgageType.TabIndex = 11;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(9, 8);
      this.label1.Name = "label1";
      this.label1.Size = new Size(101, 14);
      this.label1.TabIndex = 0;
      this.label1.Text = "Pool Mortgage Type";
      this.tpSetup.Controls.Add((Control) this.pnlEditor);
      this.tpSetup.Location = new Point(4, 24);
      this.tpSetup.Name = "tpSetup";
      this.tpSetup.Size = new Size(1043, 623);
      this.tpSetup.TabIndex = 5;
      this.tpSetup.Text = "Setup";
      this.tpSetup.UseVisualStyleBackColor = true;
      this.tpSetup.Resize += new EventHandler(this.tpSetup_Resize);
      this.pnlEditor.Dock = DockStyle.Fill;
      this.pnlEditor.Location = new Point(0, 0);
      this.pnlEditor.Name = "pnlEditor";
      this.pnlEditor.Size = new Size(1043, 623);
      this.pnlEditor.TabIndex = 24;
      this.tpPricing.AutoScroll = true;
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
      this.pnlPricing.Controls.Add((Control) this.pnlBottom);
      this.pnlPricing.Controls.Add((Control) this.gvPricing);
      this.pnlPricing.Controls.Add((Control) this.vsPricingType);
      this.pnlPricing.Dock = DockStyle.Fill;
      this.pnlPricing.Location = new Point(0, 2);
      this.pnlPricing.Name = "pnlPricing";
      this.pnlPricing.Padding = new Padding(3);
      this.pnlPricing.Size = new Size(1041, 619);
      this.pnlPricing.TabIndex = 3;
      this.pnlBottom.Controls.Add((Control) this.pnlBottomGrid);
      this.pnlBottom.Controls.Add((Control) this.pnlChooseSpecificProduct);
      this.pnlBottom.Dock = DockStyle.Fill;
      this.pnlBottom.Location = new Point(3, 221);
      this.pnlBottom.Name = "pnlBottom";
      this.pnlBottom.Size = new Size(1035, 395);
      this.pnlBottom.TabIndex = 109;
      this.pnlBottomGrid.Controls.Add((Control) this.pnlPricingLeft);
      this.pnlBottomGrid.Controls.Add((Control) this.ctlBuyUpDownEditor);
      this.pnlBottomGrid.Dock = DockStyle.Fill;
      this.pnlBottomGrid.Location = new Point(0, 47);
      this.pnlBottomGrid.Name = "pnlBottomGrid";
      this.pnlBottomGrid.Size = new Size(1035, 348);
      this.pnlBottomGrid.TabIndex = 18;
      this.pnlPricingLeft.Controls.Add((Control) this.msrPricingEditor1);
      this.pnlPricingLeft.Controls.Add((Control) this.ctlAdvancedPricing);
      this.pnlPricingLeft.Controls.Add((Control) this.ctlAdjustments);
      this.pnlPricingLeft.Dock = DockStyle.Fill;
      this.pnlPricingLeft.Location = new Point(0, 0);
      this.pnlPricingLeft.Name = "pnlPricingLeft";
      this.pnlPricingLeft.Size = new Size(504, 348);
      this.pnlPricingLeft.TabIndex = 17;
      this.msrPricingEditor1.Location = new Point(184, 234);
      this.msrPricingEditor1.Name = "msrPricingEditor1";
      this.msrPricingEditor1.PricingItems = (TradePricingItems) null;
      this.msrPricingEditor1.ReadOnly = false;
      this.msrPricingEditor1.Size = new Size(305, 241);
      this.msrPricingEditor1.TabIndex = 17;
      this.ctlAdvancedPricing.BuyUpDownItems = (MbsPoolBuyUpDownItems) null;
      this.ctlAdvancedPricing.Dock = DockStyle.Top;
      this.ctlAdvancedPricing.Location = new Point(0, 0);
      this.ctlAdvancedPricing.Name = "ctlAdvancedPricing";
      this.ctlAdvancedPricing.PoolType = MbsPoolMortgageType.None;
      this.ctlAdvancedPricing.ReadOnly = false;
      this.ctlAdvancedPricing.Size = new Size(504, 176);
      this.ctlAdvancedPricing.TabIndex = 14;
      this.ctlAdjustments.AdjustmentfromPPE = false;
      this.ctlAdjustments.Adjustments = (TradePriceAdjustments) null;
      this.ctlAdjustments.Location = new Point(0, 175);
      this.ctlAdjustments.Name = "ctlAdjustments";
      this.ctlAdjustments.ReadOnly = false;
      this.ctlAdjustments.Size = new Size(504, 173);
      this.ctlAdjustments.TabIndex = 16;
      this.ctlBuyUpDownEditor.Dock = DockStyle.Right;
      this.ctlBuyUpDownEditor.Location = new Point(504, 0);
      this.ctlBuyUpDownEditor.Name = "ctlBuyUpDownEditor";
      this.ctlBuyUpDownEditor.ReadOnly = false;
      this.ctlBuyUpDownEditor.Size = new Size(531, 348);
      this.ctlBuyUpDownEditor.TabIndex = 16;
      this.pnlChooseSpecificProduct.BackColor = SystemColors.Control;
      this.pnlChooseSpecificProduct.Controls.Add((Control) this.lblContractNumber);
      this.pnlChooseSpecificProduct.Controls.Add((Control) this.label4);
      this.pnlChooseSpecificProduct.Controls.Add((Control) this.cmbGSEProducts);
      this.pnlChooseSpecificProduct.Controls.Add((Control) this.label3);
      this.pnlChooseSpecificProduct.Controls.Add((Control) this.label2);
      this.pnlChooseSpecificProduct.Dock = DockStyle.Top;
      this.pnlChooseSpecificProduct.Location = new Point(0, 0);
      this.pnlChooseSpecificProduct.Name = "pnlChooseSpecificProduct";
      this.pnlChooseSpecificProduct.Size = new Size(1035, 47);
      this.pnlChooseSpecificProduct.TabIndex = 17;
      this.lblContractNumber.AutoSize = true;
      this.lblContractNumber.Font = new Font("Arial", 10f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblContractNumber.Location = new Point(392, 15);
      this.lblContractNumber.Name = "lblContractNumber";
      this.lblContractNumber.Size = new Size(0, 16);
      this.lblContractNumber.TabIndex = 4;
      this.label4.AutoSize = true;
      this.label4.Location = new Point(502, 16);
      this.label4.Name = "label4";
      this.label4.Size = new Size(44, 14);
      this.label4.TabIndex = 3;
      this.label4.Text = "Product";
      this.cmbGSEProducts.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbGSEProducts.FormattingEnabled = true;
      this.cmbGSEProducts.Location = new Point(552, 13);
      this.cmbGSEProducts.Name = "cmbGSEProducts";
      this.cmbGSEProducts.Size = new Size(121, 22);
      this.cmbGSEProducts.TabIndex = 2;
      this.cmbGSEProducts.SelectedIndexChanged += new EventHandler(this.cmbGSEProducts_SelectedIndexChanged);
      this.label3.AutoSize = true;
      this.label3.Location = new Point(303, 16);
      this.label3.Name = "label3";
      this.label3.Size = new Size(91, 14);
      this.label3.TabIndex = 1;
      this.label3.Text = "Contract Number:";
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(14, 16);
      this.label2.Name = "label2";
      this.label2.Size = new Size(250, 14);
      this.label2.TabIndex = 0;
      this.label2.Text = "Choose the specific product to view pricing.";
      this.gvPricing.AllowMultiselect = false;
      this.gvPricing.Dock = DockStyle.Top;
      this.gvPricing.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvPricing.Location = new Point(3, 3);
      this.gvPricing.Name = "gvPricing";
      this.gvPricing.Size = new Size(1035, 218);
      this.gvPricing.TabIndex = 107;
      this.gvPricing.SelectedIndexChanged += new EventHandler(this.gvPricing_SelectedIndexChanged);
      this.vsPricingType.Location = new Point(192, 13);
      this.vsPricingType.MaximumSize = new Size(2, 16);
      this.vsPricingType.MinimumSize = new Size(2, 16);
      this.vsPricingType.Name = "vsPricingType";
      this.vsPricingType.Size = new Size(2, 16);
      this.vsPricingType.TabIndex = 12;
      this.vsPricingType.Text = "verticalSeparator1";
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
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column1";
      gvColumn2.Text = "Event Time";
      gvColumn2.Width = 125;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column2";
      gvColumn3.SpringToFit = true;
      gvColumn3.Text = "Event";
      gvColumn3.Width = 192;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column3";
      gvColumn4.Text = "By";
      gvColumn4.Width = 125;
      this.gvHistory.Columns.AddRange(new GVColumn[3]
      {
        gvColumn2,
        gvColumn3,
        gvColumn4
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
      this.flowLayoutPanel2.Location = new Point(9, 7);
      this.flowLayoutPanel2.Name = "flowLayoutPanel2";
      this.flowLayoutPanel2.Size = new Size(187, 22);
      this.flowLayoutPanel2.TabIndex = 7;
      this.lblTradeName.AutoSize = true;
      this.lblTradeName.BackColor = Color.Transparent;
      this.lblTradeName.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblTradeName.Location = new Point(3, 0);
      this.lblTradeName.Name = "lblTradeName";
      this.lblTradeName.Padding = new Padding(0, 2, 0, 0);
      this.lblTradeName.Size = new Size(84, 16);
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
      this.Name = nameof (FannieMaePEPoolEditor);
      this.Size = new Size(1053, 684);
      ((ISupportInitialize) this.standardIconButton1).EndInit();
      ((ISupportInitialize) this.btnInvestorTemplate).EndInit();
      ((ISupportInitialize) this.btnRemovePN).EndInit();
      ((ISupportInitialize) this.btnAddPN).EndInit();
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
      this.pnlRightBottomDetails.ResumeLayout(false);
      this.grpAddress.ResumeLayout(false);
      this.pnlLeft.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.groupContainer1.ResumeLayout(false);
      this.pnlLeftTop.ResumeLayout(false);
      this.pnlLeftTop.PerformLayout();
      this.panel4.ResumeLayout(false);
      this.panel3.ResumeLayout(false);
      this.panel3.PerformLayout();
      this.pnlProductName.ResumeLayout(false);
      this.grpProductNames.ResumeLayout(false);
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      this.tpSetup.ResumeLayout(false);
      this.tpPricing.ResumeLayout(false);
      this.pnlPricing.ResumeLayout(false);
      this.pnlBottom.ResumeLayout(false);
      this.pnlBottomGrid.ResumeLayout(false);
      this.pnlPricingLeft.ResumeLayout(false);
      this.pnlChooseSpecificProduct.ResumeLayout(false);
      this.pnlChooseSpecificProduct.PerformLayout();
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
