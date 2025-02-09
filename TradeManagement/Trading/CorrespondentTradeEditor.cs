// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.CorrespondentTradeEditor
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.ClientServer.Trading;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.UI.ReportFieldDefinitions;
using EllieMae.EMLite.ContactUI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.InputEngine;
using EllieMae.EMLite.LoanUtils.RateLocks;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Reporting;
using EllieMae.EMLite.Setup;
using EllieMae.EMLite.Trading.Notifications;
using EllieMae.EMLite.UI;
using Newtonsoft.Json;
using RestApiProxy.WebhookService;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class CorrespondentTradeEditor : UserControl, IMenuProvider, ITradeEditor, ITradeEditorBase
  {
    private string className = nameof (CorrespondentTradeEditor);
    private const int ControlPadding = 5;
    private static CorrespondentTradeStatusEnumNameProvider tradeStatusNameProvider = new CorrespondentTradeStatusEnumNameProvider();
    private static string sw = Tracing.SwOutsideLoan;
    private static string[] requiredEligibilityFields = new string[1]
    {
      "Loan.InvestorStatus"
    };
    private string standardViewName = "Standard View";
    private const string commitmentNumber = "TradeName";
    private const string masterCommitmentNumber = "ContractNumber";
    public static Color AlertColor = Color.FromArgb(204, 51, 51);
    public static Color HighlightColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 231);
    private CorrespondentTradeInfo trade;
    private LoanReportFieldDefs fieldDefs;
    private TradeAssignedLoanFieldDefs tradeAssLoanFieldDefs;
    private CorrespondentTradeLoanAssignmentManager assignments;
    private bool loading;
    private bool modified;
    private bool readOnly;
    private bool loanUpdatesRequired;
    private string originalCommitmentNumber = "";
    private CorrespondentTradeInfo lastPricingTradeInfo;
    private TradeFilter lastEvaluatedFilter;
    private string originalTradeName;
    private int originalMasterId = -1;
    private bool suspendEvents;
    private LoanListScreen ctlLoanList;
    private CorrespondentTradeEditorScreenData screenData;
    private bool isCMCTabEnabled;
    private TabPage pricingTab;
    private CorrespondentMasterInfo selectedMaster;
    private System.Web.UI.WebControls.ListItem selectedDeliveryType;
    private string selectedTolerance = string.Empty;
    private ExternalOriginatorManagementData tpoSettings;
    private Decimal originalTradeAmount;
    private CorrespondentMasterDeliveryType priorDeliveryType;
    private PairOffControl pairOffControl;
    private int iIsChangesSearch;
    private string isEnableCorrespondentMaster = "false";
    private Timer refreshTimer = new Timer();
    private bool subscribed;
    private bool isTradeLoanUpdateEnabled = Session.StartupInfo.EnableTradeLoanUpdateNotification && Utils.ParseBoolean(Session.StartupInfo.TradeSettings[(object) "Trade.TradeLoanUpdateKafka"]);
    private IContainer components;
    private TabControl tabTrade;
    private TabPage tpDetails;
    private TabPage tpPricing;
    private TabPage tpLoans;
    private TabPage tpHistory;
    private System.Windows.Forms.Panel pnlFilter;
    private System.Windows.Forms.Button btnAdjustmentTemplate;
    private PriceAdjustmentListEditor ctlAdjustments;
    private System.Windows.Forms.Button btnSRPTemplate;
    private TradePricingEditor ctlPricing;
    private EllieMae.EMLite.UI.GridView gvHistory;
    private AdvancedSearchControl ctlAdvancedSearch;
    private SelectEPPSLoanProgramControl ctlSimpleSearch;
    private SRPTableEditor ctlSRP;
    private ToolTip toolTips;
    private ComboBox cboSearchType;
    private BorderPanel grpEditor;
    private FlowLayoutPanel flowLayoutPanel1;
    private StandardIconButton btnSave;
    private System.Windows.Forms.Panel pnlDetails;
    private System.Windows.Forms.Button btnSavedSearchesAdv;
    private VerticalSeparator vsAdvSearch;
    private System.Windows.Forms.Button btnSavedSearchesSimple;
    private System.Windows.Forms.Panel pnlPricing;
    private System.Windows.Forms.Panel pnlHistory;
    private GroupContainer grpHistory;
    private StandardIconButton btnExportHistory;
    private GroupContainer grpHistoryDetails;
    private RichTextBox txtHistoryDetails;
    private StandardIconButton btnList;
    private GradientPanel gradientPanel1;
    private System.Windows.Forms.Label lblTradeName;
    private System.Windows.Forms.Panel pnlLeft;
    private System.Windows.Forms.Panel pnlRight;
    private System.Windows.Forms.Panel pnlRightBottom;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.Panel pnlPairOff;
    private GroupContainer groupContainer1;
    private System.Windows.Forms.Panel panel2;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.TextBox txtOrgId;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.TextBox txtTPOID;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.TextBox txtName;
    private ComboBox cmbDeliveryType;
    private System.Windows.Forms.Label label14;
    private System.Windows.Forms.TextBox txtCommitmentType;
    private System.Windows.Forms.TextBox txtPairOffAmt;
    private System.Windows.Forms.Label label21;
    private System.Windows.Forms.Label label16;
    private System.Windows.Forms.Label label15;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox txtContract;
    private System.Windows.Forms.Label label24;
    private System.Windows.Forms.TextBox txtCommitmentNumber;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.Label label13;
    private System.Windows.Forms.TextBox txtMaxAmt;
    private DatePicker dtDeliveryExpirationDate;
    private ComboBox cboMasters;
    private DatePicker dtCommitment;
    private System.Windows.Forms.Label label20;
    private System.Windows.Forms.TextBox txtMinAmt;
    private System.Windows.Forms.Label label12;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.TextBox txtAmount;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label10;
    private System.Windows.Forms.TextBox txtTolerance;
    private System.Windows.Forms.Panel pnlAotInfo;
    private GroupContainer grpAOTInformation;
    private System.Windows.Forms.Panel panel4;
    private CollapsibleSplitter collapsibleSplitter2;
    private CollapsibleSplitter collapsibleSplitter1;
    private CollapsibleSplitter collapsibleSplitter3;
    private TabControl tabControl1;
    private TabPage tabPage1;
    private System.Windows.Forms.Panel panel15;
    private System.Windows.Forms.Label label50;
    private DatePicker dtSettlementDate;
    private System.Windows.Forms.TextBox txtSecurityPrice;
    private System.Windows.Forms.Label label49;
    private System.Windows.Forms.TextBox txtSecurityCoupon;
    private System.Windows.Forms.Label label48;
    private ComboBox cboSecurityTerm;
    private System.Windows.Forms.Label label47;
    private ComboBox cboSecurityType;
    private System.Windows.Forms.Label label46;
    private StandardIconButton btnSelector;
    private System.Windows.Forms.TextBox txtOriginalTradeDealer;
    private System.Windows.Forms.Label label45;
    private System.Windows.Forms.Label label44;
    private DatePicker dtOriginalTradeDate;
    private TableLayoutPanel tableLayoutPanel1;
    private TableLayoutPanel tableLayoutPanel2;
    private System.Windows.Forms.TextBox txtGainLossAmount;
    private System.Windows.Forms.Label label17;
    private System.Windows.Forms.Label lblAuthorizedTrader;
    private ComboBox cmbAuthorizedTrader;
    private System.Windows.Forms.Label lblDeliveryDays;
    private FieldLockButton lbtnCommitmentNumber;
    private System.Windows.Forms.Label label18;
    private ComboBox cmbTradeDesc;
    private PictureBox pictPending;
    private FlowLayoutPanel flowLayoutPanel2;
    private System.Windows.Forms.TextBox txtWABP;
    private FieldLockButton lbtnWABP;
    private System.Windows.Forms.Label label19;
    private FieldLockButton lbtnTolerance;
    private System.Windows.Forms.Panel panel3;
    private System.Windows.Forms.Label label2;
    private DatePicker datePicker1;
    private System.Windows.Forms.Label label11;
    private System.Windows.Forms.Label label22;
    private System.Windows.Forms.Label label23;
    private StandardIconButton btnDocCustodian;
    private System.Windows.Forms.TextBox txtDocCustodian;
    private System.Windows.Forms.Label label25;
    private System.Windows.Forms.Label label26;
    private ComboBox cboxAgencyName;
    private ComboBox cboxRepWarrantType;
    private ComboBox cboxFundType;
    private DatePicker dtExpirationDate;
    private ComboBox cboxAgencyDeliveryType;
    private DatePicker dtTrPartyExecutionDate;
    private System.Windows.Forms.Label label9;
    private GroupContainer grpNotes;
    public EllieMae.EMLite.UI.GridView gvNotes;
    private StandardIconButton btnEdit;
    private StandardIconButton btnDelete;
    private StandardIconButton btnAddNotes;

    protected override void Dispose(bool disposing)
    {
      this.unSubscribeEventHandler();
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    public CorrespondentTradeEditor()
    {
      this.InitializeComponent();
      TextBoxFormatter.Attach(this.txtAmount, TextBoxContentRule.NonNegativeDecimal, "#,##0");
      TextBoxFormatter.Attach(this.txtMinAmt, TextBoxContentRule.NonNegativeDecimal, "#,##0");
      TextBoxFormatter.Attach(this.txtMaxAmt, TextBoxContentRule.NonNegativeDecimal, "#,##0");
      TextBoxFormatter.Attach(this.txtTolerance, TextBoxContentRule.NonNegativeDecimal, "#,##0.000000000;;\"\"");
      TextBoxFormatter.Attach(this.txtSecurityCoupon, TextBoxContentRule.NonNegativeDecimal, "#,##0.00000;;\"\"");
      TextBoxFormatter.Attach(this.txtSecurityPrice, TextBoxContentRule.NonNegativeDecimal, "#,##0.0000000;;\"\"");
      TextBoxFormatter.Attach(this.txtWABP, TextBoxContentRule.NonNegativeDecimal, "0.00000");
      this.addSearchButtonsToControls();
      this.resetFieldDefs();
      this.ctlLoanList = new LoanListScreen((ITradeEditor) this);
      this.tpLoans.Controls.Clear();
      this.tpLoans.Controls.Add((Control) this.ctlLoanList);
      this.ctlLoanList.Dock = DockStyle.Fill;
      this.ctlSimpleSearch.HideAdditionalDetails();
      this.RemovePendingLoanFromOtherTrades = false;
      this.pairOffControl = new PairOffControl(PairOffType.CorrespondentTrades);
      this.pairOffControl.PairOffType = PairOffType.CorrespondentTrades;
      this.pairOffControl.DialogHeadTitle = "Correspondent Trade Pair-Off";
      this.pairOffControl.DeleteItemText = "Trade";
      this.pairOffControl.FieldDefs = this.fieldDefs;
      if (this.CurrentCorrespondentTradeInfo != null)
        this.pairOffControl.Locked = this.CurrentCorrespondentTradeInfo.Locked;
      this.pairOffControl.EditButtonClicked += new EventHandler(this.editButton_Clicked);
      this.pairOffControl.DeleteButtonClicked += new EventHandler(this.deleteButton_Clicked);
      this.pairOffControl.Dock = DockStyle.Fill;
      this.pnlPairOff.Controls.Clear();
      this.pnlPairOff.Controls.Add((Control) this.pairOffControl);
      this.pairOffControl.TradeEditor = (ITradeEditor) this;
      this.refreshTimer.Interval = 5000;
      this.refreshTimer.Tick += new EventHandler(this.Refresh_Tick);
      this.isEnableCorrespondentMaster = Session.ConfigurationManager.GetCompanySetting("TRADE", "EnableCorrespondentMaster");
    }

    public TradeInfoObj CurrentTradeInfo
    {
      get => this.trade != null ? (TradeInfoObj) this.trade : (TradeInfoObj) null;
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

    public CorrespondentTradeInfo CurrentCorrespondentTradeInfo => this.trade;

    public Decimal TradeAmount
    {
      get
      {
        return !string.IsNullOrEmpty(this.txtAmount.Text) ? Utils.ParseDecimal((object) this.txtAmount.Text) : 0M;
      }
    }

    public bool RemovePendingLoanFromOtherTrades { get; set; }

    public void RefreshData() => this.RefreshData(new CorrespondentTradeInfo());

    public void RefreshData(CorrespondentTradeInfo trade)
    {
      this.RefreshData(trade, (string[]) null);
    }

    public void RefreshData(CorrespondentTradeInfo trade, string[] loanGuids)
    {
      this.loading = false;
      this.modified = false;
      this.readOnly = false;
      this.ctlLoanList.ViewEligibleChecked = false;
      this.ctlLoanList.ResetWithdrawnLoanMessageFlag();
      this.RemovePendingLoanFromOtherTrades = false;
      this.lastPricingTradeInfo = (CorrespondentTradeInfo) null;
      this.lastEvaluatedFilter = (TradeFilter) null;
      this.originalTradeName = (string) null;
      this.originalMasterId = -1;
      this.selectedMaster = (CorrespondentMasterInfo) null;
      this.selectedTolerance = string.Empty;
      this.isCMCTabEnabled = string.Equals(this.isEnableCorrespondentMaster, "true", StringComparison.CurrentCultureIgnoreCase);
      this.trade = trade;
      this.refreshConfigurableFieldOptions();
      this.assignments = (CorrespondentTradeLoanAssignmentManager) null;
      this.loadTradeData();
      this.ctlLoanList.RefreshViews();
      if (this.tabTrade.TabPages["tpPricing"] == null && this.pricingTab != null)
        this.tabTrade.TabPages.Insert(1, this.pricingTab);
      if (this.trade.TradeID <= 0)
        this.txtCommitmentNumber.Enabled = Session.ConfigurationManager.GetCompanySetting("TRADE", "ENABLEAUTOCOMMITMENTNUMBER") == "False";
      else
        this.txtCommitmentNumber.Enabled = false;
      if (this.trade.TradeID <= 0 && this.trade.Filter != null && this.trade.Filter.DataLayout != null)
        this.trade.Filter.DataLayout.InsertColumns(0, this.getFixedEligibleLoanColumns());
      this.DisplayPricingTab();
      if (this.trade.TradeID <= 0)
        this.tabTrade.SelectedTab = this.tpDetails;
      if (loanGuids != null)
      {
        this.ctlLoanList.PlaceAssignedLoans(loanGuids);
        if (this.trade.TradeID > 0)
          this.tabTrade.SelectedTab = this.tpLoans;
      }
      this.ReadOnly = this.trade.Status == TradeStatus.Archived || this.trade.Status == TradeStatus.Pending || this.trade.Status == TradeStatus.Voided;
      this.loanUpdatesRequired = false;
      this.SetDeliveryType();
      this.modified = false;
    }

    private void loanLayoutMngr_LayoutChanged(object sender, EventArgs e) => this.loadTradeData();

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
        return this.modified || this.IsCommitmentNumberChanged || this.ctlPricing.DataModified || this.ctlAdjustments.DataModified || this.ctlSRP.DataModified || this.ctlLoanList.DataModified || this.isSearchModified() || this.assignments != null && this.assignments.HasModifiedAssignments() || this.ctlSimpleSearch.DataModified;
      }
    }

    public bool LoanUpdatesRequired
    {
      get
      {
        if (this.readOnly)
          return false;
        return this.loanUpdatesRequired || this.ctlPricing.DataModified || this.ctlAdjustments.DataModified || this.ctlSRP.DataModified;
      }
    }

    public bool IsCommitmentNumberChanged
    {
      get
      {
        return !this.readOnly && this.trade.TradeID > 0 && string.Compare(this.originalCommitmentNumber, this.txtCommitmentNumber.Text, true) != 0;
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

    public string NoteDetails { get; set; }

    public string NotesUser { get; set; }

    public string NotesOpreation { get; set; }

    public string NotesDateTime { get; set; }

    private void setReadOnly()
    {
      this.txtCommitmentNumber.ReadOnly = this.readOnly;
      this.txtContract.Text = string.Concat(this.cboMasters.SelectedItem);
      this.txtContract.Visible = this.readOnly;
      this.cboMasters.Visible = !this.readOnly;
      this.txtAmount.ReadOnly = this.readOnly;
      this.txtTolerance.ReadOnly = this.readOnly;
      this.cmbDeliveryType.Enabled = !this.readOnly;
      this.txtWABP.ReadOnly = this.readOnly;
      this.lbtnWABP.Enabled = !this.readOnly;
      this.lbtnTolerance.Enabled = !this.readOnly;
      this.dtCommitment.ReadOnly = this.readOnly;
      this.txtOriginalTradeDealer.ReadOnly = this.readOnly;
      this.cboSecurityType.Enabled = !this.readOnly;
      this.cboSecurityTerm.Enabled = !this.readOnly;
      this.txtSecurityCoupon.ReadOnly = this.readOnly;
      this.txtSecurityPrice.ReadOnly = this.readOnly;
      this.dtSettlementDate.ReadOnly = this.readOnly;
      this.btnSelector.Visible = !this.readOnly;
      this.dtOriginalTradeDate.ReadOnly = this.readOnly;
      this.btnDocCustodian.Visible = !this.readOnly;
      this.cboSearchType.Enabled = !this.readOnly;
      this.btnSavedSearchesSimple.Visible = !this.readOnly;
      this.btnSavedSearchesAdv.Visible = !this.readOnly;
      this.ctlSimpleSearch.ReadOnly = this.readOnly;
      this.ctlAdvancedSearch.ReadOnly = this.readOnly;
      this.btnExportHistory.Enabled = this.CurrentCorrespondentTradeInfo.Status != TradeStatus.Pending;
      this.dtCommitment.ReadOnly = this.readOnly;
      this.dtExpirationDate.ReadOnly = this.readOnly;
      this.dtDeliveryExpirationDate.ReadOnly = this.readOnly;
      this.ctlPricing.ReadOnly = this.readOnly;
      this.ctlAdjustments.ReadOnly = this.readOnly;
      this.cmbAuthorizedTrader.Enabled = !this.readOnly;
      this.btnAdjustmentTemplate.Visible = !this.readOnly;
      this.ctlSRP.ReadOnly = this.readOnly;
      this.btnSRPTemplate.Visible = !this.readOnly;
      this.ctlLoanList.ReadOnly = this.readOnly;
      this.btnSave.Enabled = !this.readOnly;
      this.cmbTradeDesc.Enabled = !this.readOnly;
      this.pairOffControl.ReadOnly = this.readOnly;
      this.cboxFundType.Enabled = !this.readOnly;
      this.cboxRepWarrantType.Enabled = !this.readOnly;
      this.cboxAgencyName.Enabled = !this.readOnly;
      this.cboxAgencyDeliveryType.Enabled = !this.readOnly;
      this.txtDocCustodian.Enabled = !this.readOnly;
    }

    private void refreshMasters(
      CorrespondentMasterInfo[] masters,
      CorrespondentMasterInfo currentMasterInfo)
    {
      this.cboMasters.Items.Clear();
      this.cboMasters.Items.Add((object) "N/A");
      bool flag = false;
      if (this.isEnableCorrespondentMaster == "True")
      {
        List<CorrespondentMasterInfo> source = new List<CorrespondentMasterInfo>();
        if (masters != null)
        {
          foreach (CorrespondentMasterInfo master in masters)
          {
            if (currentMasterInfo != null && master.ID == currentMasterInfo.ID)
              flag = true;
            else
              source.Add(master);
          }
        }
        if (currentMasterInfo != null & flag)
          source.Add(currentMasterInfo);
        this.cboMasters.Items.AddRange((object[]) source.OrderBy<CorrespondentMasterInfo, string>((System.Func<CorrespondentMasterInfo, string>) (i => i.Name)).ToArray<CorrespondentMasterInfo>());
        if (currentMasterInfo != null & flag)
          this.cboMasters.SelectedItem = (object) currentMasterInfo;
      }
      if (this.selectedMaster != null)
      {
        foreach (object obj in this.cboMasters.Items)
        {
          if (obj is CorrespondentMasterInfo && ((MasterCommitmentBase) obj).ID == this.selectedMaster.ID)
          {
            this.cboMasters.SelectedItem = obj;
            break;
          }
        }
      }
      if (this.cboMasters.SelectedItem != null)
        return;
      this.cboMasters.SelectedItem = (object) "N/A";
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
            totalPrice = Utils.ParseDecimal((object) this.txtWABP.Text.Trim());
          this.assignments.AssignLoan(pinfo, totalPrice);
          this.RefreshWeightedAvgBulkPrice();
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
      foreach (CorrespondentTradeLoanAssignment assignment in this.assignments)
        tradeAssignments.Add((LoanToTradeAssignmentBase) assignment);
      return tradeAssignments;
    }

    public bool ValidateLoanToTradeAssignment(
      LoanToTradeAssignmentBase assignment,
      out string errMsg)
    {
      errMsg = string.Empty;
      return true;
    }

    public PipelineInfo[] GetLoanToTradeAssignedPipelineData()
    {
      return this.assignments == null ? new PipelineInfo[0] : this.assignments.GetAssignedPipelineData();
    }

    public int GetLoanToTradePendingAssignmentCount()
    {
      int pendingAssignmentCount = 0;
      if (this.assignments == null)
        return 0;
      foreach (CorrespondentTradeLoanAssignment pendingLoan in this.assignments.GetPendingLoans())
      {
        if (pendingLoan.PendingStatus == CorrespondentTradeLoanStatus.Assigned)
          ++pendingAssignmentCount;
      }
      return pendingAssignmentCount;
    }

    public int GetLoanToTradePendingShipmentCount() => 0;

    public int GetLoanToTradePendingRemovalCount()
    {
      int pendingRemovalCount = 0;
      if (this.assignments == null)
        return 0;
      foreach (CorrespondentTradeLoanAssignment pendingLoan in this.assignments.GetPendingLoans())
      {
        if (pendingLoan.PendingStatus == CorrespondentTradeLoanStatus.Unassigned)
          ++pendingRemovalCount;
      }
      return pendingRemovalCount;
    }

    public int GetLoanToTradePendingPurchaseCount() => 0;

    public string[] GetLoanToTradeAssignedAndRejectedLoanGuids()
    {
      return this.assignments == null ? (string[]) null : this.assignments.GetAssignedAndRejectedLoanGuids();
    }

    public string[] GetLoanToTradeAssignedAndRejectedLoanNumbers()
    {
      return this.assignments == null ? (string[]) null : this.assignments.GetAssignedAndRejectedLoanNumbers();
    }

    public void MarkLoanToTradeAssignmentStatusToShipped(string loanGuid)
    {
    }

    public void MarkLoanToTradeAssignmentStatusToPurchasedPending(string loanGuid)
    {
    }

    public bool IsLoanToTradeAssignmentPending(LoanToTradeAssignmentBase assignment)
    {
      return ((CorrespondentTradeLoanAssignment) assignment).Pending;
    }

    public void CommitLoanToTradeAssignments(bool forceUpdateOfAllLoans, bool selectedLoans)
    {
      if (this.LoanUpdatesRequired && Utils.Dialog((IWin32Window) this, "If you update these loans, existing loan data will be overwritten with the data in the Correspondent Trade.", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK)
        return;
      this.commitTradeAssignments(forceUpdateOfAllLoans, selectedLoans);
      this.RefreshLoans();
    }

    public void RefreshLoans()
    {
      if (this.CurrentCorrespondentTradeInfo == null)
        return;
      this.loading = true;
      this.screenData = Session.CorrespondentTradeManager.GetTradeEditorScreenData(this.CurrentCorrespondentTradeInfo.TradeID, this.ctlLoanList.GetAssignedLoanListFields(), false);
      this.loadLoans(this.screenData.AssignedLoans);
      this.loadProfitabilityData();
    }

    public List<LoanToTradeAssignmentBase> GetLoanToTradeAssignedLoans()
    {
      List<LoanToTradeAssignmentBase> tradeAssignedLoans = new List<LoanToTradeAssignmentBase>();
      if (this.assignments == null)
        return tradeAssignedLoans;
      foreach (CorrespondentTradeLoanAssignment assignedLoan in this.assignments.GetAssignedLoans())
        tradeAssignedLoans.Add((LoanToTradeAssignmentBase) assignedLoan);
      return tradeAssignedLoans;
    }

    public bool IsBulkPricing()
    {
      CorrespondentMasterDeliveryType selectedDeliveryType = this.getSelectedDeliveryType(this.cmbDeliveryType);
      return selectedDeliveryType == CorrespondentMasterDeliveryType.Bulk || selectedDeliveryType == CorrespondentMasterDeliveryType.BulkAOT;
    }

    public bool IsCalculatedWABP() => this.IsBulkPricing() && !this.lbtnWABP.Locked;

    public List<LoanToTradeAssignmentBase> GetAllAssignedPendingLoans()
    {
      List<LoanToTradeAssignmentBase> assignedPendingLoans = new List<LoanToTradeAssignmentBase>();
      if (this.assignments == null)
        return assignedPendingLoans;
      foreach (CorrespondentTradeLoanAssignment assignedPendingLoan in this.assignments.GetAllAssignedPendingLoans())
        assignedPendingLoans.Add((LoanToTradeAssignmentBase) assignedPendingLoan);
      return assignedPendingLoans;
    }

    private bool isSearchModified()
    {
      if (this.cboSearchType.SelectedIndex == 0 && this.iIsChangesSearch == 1)
        return this.ctlSimpleSearch.DataModified;
      if (this.cboSearchType.SelectedIndex != 1)
        return this.ctlAdvancedSearch.DataModified;
      int iIsChangesSearch = this.iIsChangesSearch;
      return this.ctlAdvancedSearch.DataModified;
    }

    private void resetOriginalTradeData()
    {
      this.originalTradeName = this.trade.Name.Trim();
      this.originalMasterId = this.trade.CorrespondentMasterID;
    }

    public bool IsNoteRateAllowed(PipelineInfo pinfo) => this.trade.IsNoteRateAllowed(pinfo);

    public string[] GetPricingAndEligibilityFields() => this.trade.GetPricingAndEligibilityFields();

    public void LoadTradeData()
    {
      this.loadTradeData();
      this.loanUpdatesRequired = false;
    }

    public string GetLoanStatusDescription(object value)
    {
      return CorrespondentTradeEditor.tradeStatusNameProvider.GetName((object) (CorrespondentTradeLoanStatus) Utils.ParseInt(value, 1));
    }

    public Decimal CalculatePriceIndex(PipelineInfo info)
    {
      return CorrespondentTradeCalculation.CalculatePriceIndex(info, this.trade, 0M, true);
    }

    public Decimal CalculatePriceIndex(PipelineInfo info, Decimal securityPrice)
    {
      return CorrespondentTradeCalculation.CalculatePriceIndex(info, this.trade, securityPrice, true);
    }

    public Decimal CalculateProfit(PipelineInfo info, Decimal securityPrice)
    {
      return CorrespondentTradeCalculation.CalculateProfit(info, this.trade, securityPrice);
    }

    public ICursor GetEligibleLoanCursor(
      string[] fields,
      PipelineData dataToInclude,
      SortField[] sortFields,
      string[] excludedGuids,
      EligibleLoanFilterOption filterOption = EligibleLoanFilterOption.All)
    {
      return Session.CorrespondentTradeManager.GetEligibleLoanCursor(this.trade, fields, dataToInclude, sortFields, excludedGuids, false, filterOption);
    }

    private void loadTradeData()
    {
      if (this.trade == null)
        return;
      this.loading = true;
      this.tpoSettings = Session.DefaultInstance.ConfigurationManager.GetExternalOrganization(false, this.trade.ExternalOriginatorManagementID);
      this.loadTPRODeliveryTypes();
      this.loadAuthorizedDealers();
      this.screenData = Session.CorrespondentTradeManager.GetTradeEditorScreenData(this.trade.TradeID, this.ctlLoanList.GetAssignedLoanListFields(), false);
      this.originalTradeAmount = this.trade.TradeAmount;
      this.lblTradeName.Text = this.trade.TradeID > 0 ? "Correspondent Trade " + this.trade.Name : "New Correspondent Trade";
      this.txtCommitmentNumber.Text = this.trade.Name;
      this.lbtnCommitmentNumber.Locked = this.trade.OverrideTradeName;
      if (this.trade.TradeID > 0)
        this.txtCommitmentNumber.Enabled = this.trade.OverrideTradeName;
      if (this.isEnableCorrespondentMaster == "True")
        this.screenData.ActiveContracts = Session.CorrespondentTradeManager.GetCorrespondentMasterInfos(this.trade);
      this.refreshMasters(this.screenData.ActiveContracts, this.screenData.AssignedContract);
      this.dtCommitment.Value = this.trade.CommitmentDate;
      this.cmbTradeDesc.Text = this.trade.TradeDescription;
      this.txtCommitmentType.Text = this.trade.CommitmentType.ToDescription();
      this.txtGainLossAmount.Text = this.trade.GainLossAmount.ToString();
      if (!string.IsNullOrEmpty(this.trade.AuthorizedTraderUserId))
      {
        IEnumerable<System.Web.UI.WebControls.ListItem> source = this.cmbAuthorizedTrader.Items.Cast<System.Web.UI.WebControls.ListItem>().Where<System.Web.UI.WebControls.ListItem>((System.Func<System.Web.UI.WebControls.ListItem, bool>) (item => item.Value.Equals(this.trade.AuthorizedTraderUserId)));
        if (source.Count<System.Web.UI.WebControls.ListItem>() > 0)
        {
          this.cmbAuthorizedTrader.SelectedItem = (object) source.First<System.Web.UI.WebControls.ListItem>();
        }
        else
        {
          ExternalUserInfo userInfoByContactId = Session.ConfigurationManager.GetExternalUserInfoByContactId(this.trade.AuthorizedTraderUserId);
          if ((UserInfo) userInfoByContactId != (UserInfo) null)
            this.addAuthorizedTraderListItem(userInfoByContactId, true);
        }
      }
      System.Windows.Forms.TextBox txtAmount = this.txtAmount;
      Decimal num = this.trade.TradeAmount;
      string str1 = num.ToString("#,##0;;\"\"");
      txtAmount.Text = str1;
      System.Windows.Forms.TextBox txtTolerance1 = this.txtTolerance;
      string str2;
      if (!(this.trade.Tolerance == 0M))
      {
        num = this.trade.Tolerance;
        str2 = num.ToString("#,##0.000000000;;\"\"");
      }
      else
        str2 = "";
      txtTolerance1.Text = str2;
      if (this.trade.Tolerance > 0M)
      {
        System.Windows.Forms.TextBox txtTolerance2 = this.txtTolerance;
        string str3;
        if (!(this.trade.Tolerance == 0M))
        {
          num = this.trade.Tolerance;
          str3 = num.ToString("#,##0.000000000;;\"\"");
        }
        else
          str3 = "";
        txtTolerance2.Text = str3;
      }
      else
        this.loadTolerance();
      if (this.trade.DeliveryType != CorrespondentMasterDeliveryType.None)
      {
        this.loading = true;
        this.cmbDeliveryType.SelectedItem = (object) new System.Web.UI.WebControls.ListItem(this.trade.DeliveryType.ToDescription(), ((int) this.trade.DeliveryType).ToString());
        if (this.cmbDeliveryType.SelectedItem == null)
        {
          System.Web.UI.WebControls.ListItem listItem = new System.Web.UI.WebControls.ListItem(this.trade.DeliveryType.ToDescription() + " (Inactive)", ((int) this.trade.DeliveryType).ToString());
          this.cmbDeliveryType.Items.Add((object) listItem);
          this.cmbDeliveryType.SelectedItem = (object) listItem;
        }
      }
      this.loading = true;
      this.LoadNotesGridView(this.trade.Notes);
      this.txtName.Text = this.trade.CompanyName;
      this.txtTPOID.Text = this.trade.TPOID;
      this.txtOrgId.Text = this.trade.OrganizationID;
      this.dtExpirationDate.Value = this.trade.ExpirationDate;
      this.dtDeliveryExpirationDate.Value = this.trade.DeliveryExpirationDate;
      this.cboxFundType.Text = this.trade.FundType;
      this.cboxRepWarrantType.Text = this.trade.OriginationRepWarrantType;
      this.cboxAgencyName.Text = this.trade.AgencyName;
      this.cboxAgencyDeliveryType.Text = this.trade.AgencyDeliveryType;
      this.txtDocCustodian.Text = this.trade.DocCustodian;
      this.txtOriginalTradeDealer.Text = this.trade.AOTOriginalTradeDealer;
      this.cboSecurityTerm.Text = this.trade.AOTSecurityTerm;
      System.Windows.Forms.TextBox txtSecurityCoupon = this.txtSecurityCoupon;
      num = this.trade.AOTSecurityCoupon;
      string str4 = num.ToString("#,##0.00000;;\"\"");
      txtSecurityCoupon.Text = str4;
      System.Windows.Forms.TextBox txtSecurityPrice = this.txtSecurityPrice;
      num = this.trade.AOTSecurityPrice;
      string str5 = num.ToString("#,##0.0000000;;\"\"");
      txtSecurityPrice.Text = str5;
      this.cboSecurityType.Text = this.trade.AOTSecurityType;
      this.dtSettlementDate.Value = this.trade.AOTSettlementDate;
      this.dtOriginalTradeDate.Value = this.trade.AOTOriginalTradeDate;
      this.txtOriginalTradeDealer.Text = this.trade.AOTOriginalTradeDealer;
      this.txtCommitmentType.Text = this.trade.CommitmentType.ToDescription();
      System.Windows.Forms.TextBox txtWabp = this.txtWABP;
      num = this.trade.WeightedAvgBulkPrice;
      string str6 = num.ToString("#,##0.00000;;\"\"");
      txtWabp.Text = str6;
      this.lbtnWABP.Locked = this.trade.IsWeightedAvgBulkPriceLocked;
      this.txtWABP.Enabled = this.lbtnWABP.Locked;
      this.lbtnTolerance.Locked = this.trade.IsToleranceLocked;
      if (this.lbtnTolerance.Locked)
        this.txtTolerance.Enabled = true;
      this.ctlPricing.PricingItems = this.trade.Pricing.SimplePricingItems;
      this.ctlAdjustments.Adjustments = this.trade.PriceAdjustments;
      this.ctlSRP.SRPTable = this.trade.SRPTable;
      this.ctlSRP.SRPfromPPE = this.trade.SRPfromPPE;
      this.ctlSRP.ShowPPEIndicator(TradeType.CorrespondentTrade, this.trade.DeliveryType);
      this.ctlAdjustments.AdjustmentfromPPE = this.trade.AdjustmentsfromPPE;
      this.ctlAdjustments.ShowPPEIndicator(TradeType.CorrespondentTrade, this.trade.DeliveryType);
      this.loadPairOffs();
      this.loadSearch();
      this.loadHistory(this.screenData.TradeHistory);
      this.loadLoans(this.screenData.AssignedLoans);
      this.loadProfitabilityData();
      this.resetOriginalTradeData();
      this.ReadOnly = this.trade.Status == TradeStatus.Archived || this.trade.Status == TradeStatus.Pending;
      this.MakePending(this.trade.Status == TradeStatus.Pending);
      this.loanUpdatesRequired = false;
      this.originalCommitmentNumber = this.trade.Name;
      if (this.trade.IsCloned)
        this.modified = this.trade.TradeID <= 0;
      this.priorDeliveryType = this.trade.DeliveryType;
      this.SetDeliveryType();
      this.SetLoanlistViewEligible();
      this.RefreshWeightedAvgBulkPrice();
      this.modified = false;
      this.loading = false;
      this.ctlSimpleSearch.DataModified = false;
      this.iIsChangesSearch = this.cboSearchType.SelectedIndex;
    }

    private void stopTimer()
    {
      if (this.trade.status == TradeStatus.Pending)
        return;
      this.refreshTimer.Stop();
      int num = (int) new TradeLoanUpdateNotificationDialog(this.trade.Name, "completed").ShowDialog();
    }

    private void Refresh_Tick(object sender, EventArgs e)
    {
      if (Session.CorrespondentTradeManager.GetTradeStatus(this.trade.TradeID) == TradeStatus.Pending)
        return;
      this.RefreshData(Session.CorrespondentTradeManager.GetTrade(this.trade.TradeID));
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
          Tracing.Log(CorrespondentTradeEditor.sw, this.className, TraceLevel.Info, "Starting timer for loan trade");
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

    private void loadLoans(PipelineInfo[] pinfos)
    {
      if (pinfos == null)
        pinfos = new PipelineInfo[0];
      this.assignments = new CorrespondentTradeLoanAssignmentManager(Session.SessionObjects, this.trade.TradeID, pinfos);
      if (this.CheckLoanAmountOfAssignedLoans())
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "This trade contains a loan with a null value in the loan amount field. Please note and take appropriate action.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
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
      return this.trade.Pricing.SimplePricingItems.Count <= 0 || this.trade.IsNoteRateAllowed(pinfo);
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
      CorrespondentTradeLoanAssignment tradeLoanAssignment = (CorrespondentTradeLoanAssignment) assignmentInfo;
      string name = CorrespondentTradeEditor.tradeStatusNameProvider.GetName((object) tradeLoanAssignment.Status);
      if (tradeLoanAssignment.Status == CorrespondentTradeLoanStatus.Unassigned)
        return "Removed - Pending";
      if (tradeLoanAssignment.Pending)
        name += " - Pending";
      return name;
    }

    private void loadHistory(CorrespondentTradeHistoryItem[] historyItems)
    {
      this.btnExportHistory.Enabled = false;
      this.gvHistory.Items.Clear();
      if (historyItems == null)
        return;
      foreach (CorrespondentTradeHistoryItem historyItem in historyItems)
        this.gvHistory.Items.Add(this.createTradeHistoryListItem(historyItem));
      this.btnExportHistory.Enabled = this.gvHistory.Items.Count > 0;
    }

    private GVItem createTradeHistoryListItem(CorrespondentTradeHistoryItem historyItem)
    {
      return new GVItem()
      {
        Text = historyItem.Timestamp.ToString("MM/dd/yyyy h:mm:ss:fff tt"),
        SubItems = {
          (object) historyItem.Description,
          (object) historyItem.UserName
        },
        Tag = (object) historyItem
      };
    }

    private void loadTolerance()
    {
      this.loading = true;
      if (!this.lbtnTolerance.Locked && !this.readOnly)
        this.txtTolerance.Enabled = true;
      if (this.tpoSettings != null && !this.lbtnTolerance.Locked)
      {
        if (this.tpoSettings.CommitmentMandatory && this.trade.CommitmentType == CorrespondentTradeCommitmentType.Mandatory)
        {
          if (this.tpoSettings.MandatoryTolerencePolicy == ExternalOriginatorCommitmentTolerancePolicy.FlatTolerance)
          {
            this.txtTolerance.Text = this.tpoSettings.MandatoryTolerancePct.ToString("#,##0.000000000;;\"\"");
            this.txtTolerance.Enabled = false;
          }
          else if (this.tpoSettings.MandatoryTolerencePolicy == ExternalOriginatorCommitmentTolerancePolicy.ConditionalTolerance && Utils.ParseDecimal((object) this.txtAmount.Text) > 0M)
          {
            Decimal val2 = Math.Round(this.tpoSettings.MandatoryToleranceAmt / Utils.ParseDecimal((object) this.txtAmount.Text) * 100M, 9);
            if (val2 > 0M && this.tpoSettings.MandatoryTolerancePct > 0M)
              this.txtTolerance.Text = Math.Min(this.tpoSettings.MandatoryTolerancePct, val2).ToString("#,##0.000000000;;\"\"");
            else if (val2 <= 0M && this.tpoSettings.MandatoryTolerancePct > 0M)
              this.txtTolerance.Text = this.tpoSettings.MandatoryTolerancePct.ToString("#,##0.000000000;;\"\"");
            else if (val2 > 0M && this.tpoSettings.MandatoryTolerancePct <= 0M)
              this.txtTolerance.Text = val2.ToString("#,##0.000000000;;\"\"");
            this.txtTolerance.Enabled = false;
          }
        }
        else if (this.tpoSettings.CommitmentUseBestEffortLimited && this.trade.CommitmentType == CorrespondentTradeCommitmentType.BestEfforts)
        {
          if (this.tpoSettings.BestEffortTolerencePolicy == ExternalOriginatorCommitmentTolerancePolicy.FlatTolerance)
          {
            this.txtTolerance.Text = this.tpoSettings.BestEffortTolerancePct.ToString("#,##0.000000000;;\"\"");
            this.txtTolerance.Enabled = false;
          }
          else if (this.tpoSettings.BestEffortTolerencePolicy == ExternalOriginatorCommitmentTolerancePolicy.ConditionalTolerance && Utils.ParseDecimal((object) this.txtAmount.Text) > 0M)
          {
            Decimal val2 = Math.Round(this.tpoSettings.BestEffortToleranceAmt / Utils.ParseDecimal((object) this.txtAmount.Text) * 100M, 9);
            if (val2 > 0M && this.tpoSettings.BestEffortTolerancePct > 0M)
              this.txtTolerance.Text = Math.Min(this.tpoSettings.BestEffortTolerancePct, val2).ToString("#,##0.000000000;;\"\"");
            else if (val2 <= 0M && this.tpoSettings.BestEffortTolerancePct > 0M)
              this.txtTolerance.Text = this.tpoSettings.BestEffortTolerancePct.ToString("#,##0.000000000;;\"\"");
            else if (val2 > 0M && this.tpoSettings.BestEffortTolerancePct <= 0M)
              this.txtTolerance.Text = val2.ToString("#,##0.000000000;;\"\"");
            this.txtTolerance.Enabled = false;
          }
        }
      }
      this.loading = false;
    }

    private void loadTPRODeliveryTypes()
    {
      this.loading = true;
      if (!this.isCMCTabEnabled || Convert.ToString(this.cboMasters.SelectedItem) == "N/A")
      {
        if (this.tpoSettings != null)
        {
          this.cmbDeliveryType.Items.Clear();
          if (this.tpoSettings.CommitmentUseBestEffort && this.trade.CommitmentType == CorrespondentTradeCommitmentType.BestEfforts)
            this.cmbDeliveryType.Items.Add((object) new System.Web.UI.WebControls.ListItem(CorrespondentMasterDeliveryType.IndividualBestEfforts.ToDescription(), 15.ToString()));
          if (!this.tpoSettings.CommitmentUseBestEffort && !this.tpoSettings.CommitmentMandatory && this.trade.CommitmentType == CorrespondentTradeCommitmentType.BestEfforts)
            this.cmbDeliveryType.Items.Add((object) new System.Web.UI.WebControls.ListItem(CorrespondentMasterDeliveryType.IndividualBestEfforts.ToDescription(), 15.ToString()));
          if (this.tpoSettings.CommitmentMandatory && this.trade.CommitmentType == CorrespondentTradeCommitmentType.Mandatory)
          {
            if (this.tpoSettings.IsCommitmentDeliveryIndividual)
              this.cmbDeliveryType.Items.Add((object) new System.Web.UI.WebControls.ListItem(CorrespondentMasterDeliveryType.IndividualMandatory.ToDescription(), 20.ToString()));
            if (this.tpoSettings.IsCommitmentDeliveryBulk)
              this.cmbDeliveryType.Items.Add((object) new System.Web.UI.WebControls.ListItem(CorrespondentMasterDeliveryType.Bulk.ToDescription(), 30.ToString()));
            if (this.tpoSettings.IsCommitmentDeliveryAOT)
              this.cmbDeliveryType.Items.Add((object) new System.Web.UI.WebControls.ListItem(CorrespondentMasterDeliveryType.AOT.ToDescription(), 5.ToString()));
            if (this.tpoSettings.IsCommitmentDeliveryBulkAOT)
              this.cmbDeliveryType.Items.Add((object) new System.Web.UI.WebControls.ListItem(CorrespondentMasterDeliveryType.BulkAOT.ToDescription(), 35.ToString()));
            if (this.tpoSettings.IsCommitmentDeliveryLiveTrade)
              this.cmbDeliveryType.Items.Add((object) new System.Web.UI.WebControls.ListItem(CorrespondentMasterDeliveryType.LiveTrade.ToDescription(), 25.ToString()));
            if (this.tpoSettings.IsCommitmentDeliveryCoIssue)
              this.cmbDeliveryType.Items.Add((object) new System.Web.UI.WebControls.ListItem(CorrespondentMasterDeliveryType.CoIssue.ToDescription(), 40.ToString()));
            if (this.tpoSettings.IsCommitmentDeliveryForward)
              this.cmbDeliveryType.Items.Add((object) new System.Web.UI.WebControls.ListItem(CorrespondentMasterDeliveryType.Forwards.ToDescription(), 10.ToString()));
          }
        }
        if (Convert.ToString(this.cboMasters.SelectedItem) != "N/A")
          this.cmbDeliveryType.Text = this.selectedDeliveryTypeText(this.trade.DeliveryType);
        else
          this.cmbDeliveryType.SelectedIndex = -1;
      }
      this.loading = false;
    }

    private void loadAuthorizedDealers()
    {
      this.loading = true;
      this.cmbAuthorizedTrader.Items.Clear();
      List<ExternalUserInfo> authorizedDealers = Session.ConfigurationManager.GetAllAuthorizedDealers(this.trade.ExternalOriginatorManagementID);
      if (authorizedDealers != null && authorizedDealers.Count > 0)
      {
        foreach (ExternalUserInfo user in authorizedDealers)
          this.addAuthorizedTraderListItem(user);
      }
      this.loading = false;
    }

    private void addAuthorizedTraderListItem(ExternalUserInfo user, bool selected = false)
    {
      System.Web.UI.WebControls.ListItem listItem = new System.Web.UI.WebControls.ListItem(user.FirstName + " " + user.LastName, user.ContactID);
      this.cmbAuthorizedTrader.Items.Add((object) listItem);
      if (!selected)
        return;
      this.cmbAuthorizedTrader.SelectedItem = (object) listItem;
    }

    private void loadPairOffs()
    {
      List<CommonPairOff> commonPairOffList = new List<CommonPairOff>();
      for (int index = 0; index < this.CurrentCorrespondentTradeInfo.CorrespondentTradePairOffs.Count; ++index)
        commonPairOffList.Add((CommonPairOff) this.CurrentCorrespondentTradeInfo.CorrespondentTradePairOffs[index]);
      this.pairOffControl.LoadPairOffs(commonPairOffList.ToArray());
    }

    private void loadSearch()
    {
      this.ctlSimpleSearch.SetEPPSLoanPrograms(this.trade.EPPSLoanProgramsFilter);
      if (this.trade.Filter == null)
      {
        this.ctlSimpleSearch.NoteRateMin = "";
        this.ctlSimpleSearch.NoteRateMax = "";
        this.switchToSimpleMode(new SimpleTradeFilter(false));
        this.ctlSimpleSearch.DataModified = false;
      }
      else if (this.trade.Filter.FilterType == TradeFilterType.Advanced)
      {
        this.switchToAdvancedMode(new FieldFilterList(this.trade.Filter.GetAdvancedFilter().Where<FieldFilter>((System.Func<FieldFilter, bool>) (f => !f.CriterionName.Contains("CurrentMilestoneName"))).ToArray<FieldFilter>()));
        this.ctlSimpleSearch.DataModified = false;
      }
      else
      {
        SimpleTradeFilter simpleFilter = this.trade.Filter.GetSimpleFilter();
        if (simpleFilter != null && simpleFilter.Milestones != null)
          simpleFilter.Milestones.Clear();
        this.switchToSimpleMode(simpleFilter);
        this.ctlAdvancedSearch.DataModified = false;
      }
      if (this.trade.Filter == null || !this.ctlLoanList.ViewEligibleChecked || this.trade.Filter.DataLayout == null)
        return;
      this.ctlLoanList.ClearCurrentEligibilityCursor();
      this.ctlLoanList.ValidateTableLayout(this.trade.Filter.DataLayout);
      this.ctlLoanList.ApplyLayout(this.trade.Filter.DataLayout, false);
    }

    private void switchToSimpleMode(SimpleTradeFilter filter)
    {
      bool dataModified = this.DataModified;
      this.ctlSimpleSearch.SetCurrentFilter(filter);
      this.ctlSimpleSearch.SetEPPSLoanPrograms(this.trade.EPPSLoanProgramsFilter);
      this.ctlSimpleSearch.Visible = true;
      this.ctlAdvancedSearch.Visible = false;
      this.cboSearchType.SelectedIndex = 0;
      this.modified = dataModified;
    }

    private void switchToAdvancedMode(FieldFilterList filters)
    {
      bool dataModified = this.DataModified;
      this.trade.EPPSLoanProgramsFilter = this.ctlSimpleSearch.GetEPPSLoanPorgramFilter();
      this.ctlAdvancedSearch.SetCurrentFilter(filters);
      if (filters != null && filters.Any<FieldFilter>((System.Func<FieldFilter, bool>) (filter => filter.FieldID.Equals("3"))))
      {
        FieldFilter fieldFilter = filters.Where<FieldFilter>((System.Func<FieldFilter, bool>) (filter => filter.FieldID.Equals("3"))).First<FieldFilter>();
        if (fieldFilter != null)
        {
          this.ctlSimpleSearch.NoteRateMin = fieldFilter.ValueFrom;
          this.ctlSimpleSearch.NoteRateMax = fieldFilter.ValueTo;
        }
      }
      this.ctlAdvancedSearch.Visible = true;
      this.ctlSimpleSearch.Visible = false;
      this.cboSearchType.SelectedIndex = 1;
      this.modified = dataModified;
    }

    private void loadProfitabilityData()
    {
      Decimal calculatedPairOffFee = this.trade.CorrespondentTradePairOffs.GetDisplayCalculatedPairOffFee();
      this.txtPairOffAmt.Text = this.trade.GetTotalPairOffAmount().ToString("#,##0.00;;\"\"");
      this.txtGainLossAmount.Text = (this.CalculateGainLoss() + calculatedPairOffFee).ToString("#,##0.00;;\"\"");
    }

    public Decimal GetOpenAmount()
    {
      List<LoanToTradeAssignmentBase> assignedPendingLoans = this.GetAllAssignedPendingLoans();
      Decimal assignedAmount = 0M;
      if (assignedPendingLoans != null && assignedPendingLoans.Count<LoanToTradeAssignmentBase>() > 0)
        assignedAmount = assignedPendingLoans.Sum<LoanToTradeAssignmentBase>((System.Func<LoanToTradeAssignmentBase, Decimal>) (a => (Decimal) a.PipelineInfo.Info[(object) "Loan.TotalLoanAmount"]));
      return CorrespondentTradeCalculation.CalculateOpenAmount(this.trade.TradeAmount, assignedAmount, this.trade.GetTotalPairOffAmount());
    }

    private Decimal CalculateGainLoss()
    {
      if (this.assignments == null)
        return 0M;
      List<PipelineInfo> loans = new List<PipelineInfo>();
      foreach (CorrespondentTradeLoanAssignment assignedLoan in this.assignments.GetAssignedLoans())
        loans.Add(assignedLoan.PipelineInfo);
      return CorrespondentTradeCalculation.CalculateGainLoss((IEnumerable<PipelineInfo>) loans, this.trade);
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
    }

    private void cboMasters_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.cboMasters.SelectedItem != null && Convert.ToString(this.cboMasters.SelectedItem) != "N/A" && ((MasterCommitmentBase) this.cboMasters.SelectedItem).ID != this.originalMasterId || this.originalMasterId != -1 && Convert.ToString(this.cboMasters.SelectedItem) == "N/A")
        this.modified = true;
      if (!this.isCMCTabEnabled || this.Loading)
        return;
      this.selectedDeliveryType = (System.Web.UI.WebControls.ListItem) this.cmbDeliveryType.SelectedItem;
      this.cmbDeliveryType.Items.Clear();
      this.txtTolerance.Text = "";
      this.lblDeliveryDays.Text = "";
      this.dtDeliveryExpirationDate.Text = "";
      if (Convert.ToString(this.cboMasters.SelectedItem) == "N/A")
      {
        this.loadTPRODeliveryTypes();
        if (this.selectedDeliveryType != null)
        {
          foreach (object objA in this.cmbDeliveryType.Items)
          {
            if (!object.Equals(objA, (object) "N/A") && ((System.Web.UI.WebControls.ListItem) objA).Value == this.selectedDeliveryType.Value)
            {
              this.cmbDeliveryType.SelectedItem = objA;
              break;
            }
          }
        }
        this.selectedMaster = (CorrespondentMasterInfo) null;
      }
      if (!(this.cboMasters.SelectedItem is CorrespondentMasterInfo))
        return;
      foreach (MasterCommitmentDeliveryInfo deliveryInfo in ((MasterCommitmentBase) this.cboMasters.SelectedItem).DeliveryInfos)
      {
        int type;
        if (this.trade.CommitmentType == CorrespondentTradeCommitmentType.BestEfforts && deliveryInfo.Type == CorrespondentMasterDeliveryType.IndividualBestEfforts)
        {
          this.cmbDeliveryType.Items.Clear();
          ComboBox.ObjectCollection items = this.cmbDeliveryType.Items;
          string description = deliveryInfo.Type.ToDescription();
          type = (int) deliveryInfo.Type;
          string str = type.ToString();
          System.Web.UI.WebControls.ListItem listItem = new System.Web.UI.WebControls.ListItem(description, str);
          items.Add((object) listItem);
          break;
        }
        if (deliveryInfo.Type != CorrespondentMasterDeliveryType.IndividualBestEfforts)
        {
          ComboBox.ObjectCollection items = this.cmbDeliveryType.Items;
          string description = deliveryInfo.Type.ToDescription();
          type = (int) deliveryInfo.Type;
          string str = type.ToString();
          System.Web.UI.WebControls.ListItem listItem = new System.Web.UI.WebControls.ListItem(description, str);
          items.Add((object) listItem);
        }
      }
      if (this.selectedDeliveryType != null)
      {
        foreach (object objA in this.cmbDeliveryType.Items)
        {
          if (!object.Equals(objA, (object) "N/A") && ((System.Web.UI.WebControls.ListItem) objA).Value == this.selectedDeliveryType.Value)
          {
            this.cmbDeliveryType.SelectedItem = objA;
            break;
          }
        }
      }
      this.selectedMaster = (CorrespondentMasterInfo) this.cboMasters.SelectedItem;
    }

    private void cboSearchType_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.cboSearchType.SelectedIndex == 0 && this.ctlAdvancedSearch.Visible)
      {
        if (!this.loading && this.ctlAdvancedSearch.GetCurrentFilter().Count > 0 && Utils.Dialog((IWin32Window) this, "If you switch to Simple Search, you will lose all of your Advanced search criteria.", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK)
        {
          this.cboSearchType.SelectedIndex = 1;
        }
        else
        {
          this.switchToSimpleMode(this.ctlSimpleSearch.GetCurrentFilter());
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
      this.trade.Name = this.txtCommitmentNumber.Text.Trim();
      this.trade.CommitmentDate = this.dtCommitment.Value;
      this.trade.TradeDescription = this.cmbTradeDesc.Text.Trim();
      if (this.cboMasters.SelectedIndex <= 0)
      {
        this.trade.ContractID = -1;
        this.trade.CorrespondentMasterCommitmentNumber = "";
      }
      else
        this.trade.CorrespondentMasterCommitmentNumber = ((TradeBase) this.cboMasters.SelectedItem).Name;
      if (this.cmbDeliveryType.SelectedIndex > -1)
        this.trade.DeliveryType = (CorrespondentMasterDeliveryType) Utils.ParseInt((object) ((System.Web.UI.WebControls.ListItem) this.cmbDeliveryType.SelectedItem).Value.ToString());
      this.ctlSRP.ShowPPEIndicator(TradeType.CorrespondentTrade, this.trade.DeliveryType);
      this.ctlAdjustments.ShowPPEIndicator(TradeType.CorrespondentTrade, this.trade.DeliveryType);
      this.trade.CompanyName = this.txtName.Text;
      this.trade.TPOID = this.txtTPOID.Text;
      this.trade.OrganizationID = this.txtOrgId.Text;
      this.trade.TradeAmount = Utils.ParseDecimal((object) this.txtAmount.Text);
      this.trade.Tolerance = Utils.ParseDecimal((object) this.txtTolerance.Text);
      this.trade.MinAmount = Utils.ParseDecimal((object) this.txtMinAmt.Text);
      this.trade.MaxAmount = Utils.ParseDecimal((object) this.txtMaxAmt.Text);
      this.trade.ExpirationDate = this.dtExpirationDate.Value;
      this.trade.DeliveryExpirationDate = this.dtDeliveryExpirationDate.Value;
      this.trade.PairOffAmount = Utils.ParseDecimal((object) this.txtPairOffAmt.Text);
      this.trade.Notes = this.GetNotesFromGridView();
      this.trade.AOTOriginalTradeDate = this.dtOriginalTradeDate.Value;
      this.trade.AOTOriginalTradeDealer = this.txtOriginalTradeDealer.Text;
      this.trade.AOTSecurityType = this.cboSecurityType.Text;
      this.trade.AOTSecurityTerm = this.cboSecurityTerm.Text;
      this.trade.AOTSecurityCoupon = Utils.ParseDecimal((object) this.txtSecurityCoupon.Text);
      this.trade.AOTSecurityPrice = Utils.ParseDecimal((object) this.txtSecurityPrice.Text);
      this.trade.AOTSettlementDate = this.dtSettlementDate.Value;
      this.trade.GainLossAmount = Utils.ParseDecimal((object) this.txtGainLossAmount.Text);
      this.trade.AuthorizedTraderUserId = this.cmbAuthorizedTrader.SelectedItem != null ? ((System.Web.UI.WebControls.ListItem) this.cmbAuthorizedTrader.SelectedItem).Value : string.Empty;
      this.trade.AuthorizedTraderName = this.cmbAuthorizedTrader.SelectedItem != null ? ((System.Web.UI.WebControls.ListItem) this.cmbAuthorizedTrader.SelectedItem).Text : string.Empty;
      this.trade.AdjustmentsfromPPE = this.ctlAdjustments.AdjustmentfromPPE;
      this.trade.SRPfromPPE = this.ctlSRP.SRPfromPPE;
      this.trade.WeightedAvgBulkPrice = Utils.ParseDecimal((object) this.txtWABP.Text);
      this.trade.IsWeightedAvgBulkPriceLocked = Utils.ParseBoolean((object) this.lbtnWABP.Locked);
      this.trade.IsToleranceLocked = Utils.ParseBoolean((object) this.lbtnTolerance.Locked);
      this.trade.FundType = this.cboxFundType.Text;
      this.trade.OriginationRepWarrantType = this.cboxRepWarrantType.Text;
      this.trade.AgencyName = this.cboxAgencyName.Text;
      this.trade.AgencyDeliveryType = this.cboxAgencyDeliveryType.Text;
      this.trade.DocCustodian = this.txtDocCustodian.Text;
      if (this.cboMasters.SelectedIndex <= 0)
      {
        this.trade.CorrespondentMasterID = -1;
        this.trade.CorrespondentMasterCommitmentNumber = string.Empty;
      }
      else
      {
        CorrespondentMasterInfo selectedItem = (CorrespondentMasterInfo) this.cboMasters.SelectedItem;
        this.trade.CorrespondentMasterID = selectedItem.ID;
        this.trade.CorrespondentMasterCommitmentNumber = selectedItem.Name;
      }
      this.ctlPricing.CommitChanges();
      this.ctlAdjustments.CommitChanges();
      this.ctlSRP.CommitChanges();
      this.trade.ExpirationDate = this.dtExpirationDate.Value;
      this.trade.DeliveryExpirationDate = this.dtDeliveryExpirationDate.Value;
      this.saveSearch();
      this.ctlLoanList.DataModified = false;
    }

    private void saveSearch()
    {
      this.trade.Filter = this.getCurrentFilter();
      this.ctlAdvancedSearch.DataModified = false;
      this.ctlSimpleSearch.DataModified = false;
      this.trade.EPPSLoanProgramsFilter = this.ctlSimpleSearch.GetEPPSLoanPorgramFilter();
    }

    private TradeFilter getCurrentFilter()
    {
      return this.cboSearchType.SelectedIndex == 0 ? new TradeFilter(this.ctlSimpleSearch.GetCurrentFilter(), this.ctlLoanList.GetCurrentLayout()) : new TradeFilter(this.ctlAdvancedSearch.GetCurrentFilter(), this.ctlLoanList.GetCurrentLayout());
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      bool silentLoanUpdate = false;
      if (this.IsCommitmentNumberChanged && this.DataModified)
        silentLoanUpdate = true;
      if (!this.DataModified)
        return;
      this.SaveCorrespondentTrade(silentLoanUpdate);
    }

    public bool SaveCorrespondentTrade(bool silentLoanUpdate = false)
    {
      return this.SaveTrade(silentLoanUpdate, false);
    }

    public void PerformPublishTradeCheck()
    {
      if (!string.Equals(Session.ConfigurationManager.GetCompanySetting("TRADE", "AllowPublishEvent"), "true", StringComparison.CurrentCultureIgnoreCase) || this.trade.Status != TradeStatus.Unpublished || this.trade.TradeID <= 0 || Utils.Dialog((IWin32Window) this, "The correspondent trade is unpublished. Do you want to publish the trade now?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
        return;
      Session.CorrespondentTradeManager.PublishTrade(this.trade, true);
      this.PublishWebhookEvent("publish", this.trade.TradeID);
    }

    private void PublishWebhookEvent(string eventType, int tradeId)
    {
      WebhookEventContract eventContract = new WebhookEventContract();
      eventContract.UserId = Session.UserInfo.Userid;
      eventContract.InstanceId = Session.DefaultInstance.ServerIdentity != null ? Session.DefaultInstance.ServerIdentity.InstanceName : "LOCALHOST";
      eventContract.EventType = eventType;
      eventContract.ResourceId = tradeId.ToString();
      if (string.Equals(eventType, "publish", StringComparison.InvariantCultureIgnoreCase))
      {
        this.trade = Session.CorrespondentTradeManager.GetTrade(tradeId);
        JsonSerializerSettings settings = new JsonSerializerSettings()
        {
          DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffZ"
        };
        eventContract.AddExpoObject("lastPublishedDateTime", (object) JsonConvert.SerializeObject((object) this.trade.LastPublishedDateTime, settings));
      }
      else
        eventContract.payload = (object) new ExpandoObject();
      try
      {
        new WebhookEventHelper(eventContract.InstanceId, Session.SessionObjects.SessionID, WebhookResource.trade).Publish(eventContract);
      }
      catch (Exception ex)
      {
        Tracing.Log(CorrespondentTradeEditor.sw, TraceLevel.Error, this.className, "Webhook publish failed for " + eventType + " event: " + ex.Message);
      }
    }

    public bool SaveTrade(bool forceUpdateOfLoans, bool updatedSelectedLoans)
    {
      if (this.readOnly || this.ConfirmRemovingPricingDialog())
        return true;
      Cursor.Current = Cursors.WaitCursor;
      try
      {
        if (!this.prevalidateCommit())
          return false;
        this.commitChanges();
        if (!this.validateTradeData())
        {
          this.trade.Name = this.originalTradeName;
          return false;
        }
        this.saveTradeInfo();
        bool flag = true;
        bool unsavedPendingLoan = this.UnsavedPendingLoan;
        if (forceUpdateOfLoans & unsavedPendingLoan)
          flag = this.commitTradeAssignments(true, updatedSelectedLoans);
        else if (unsavedPendingLoan && Utils.Dialog((IWin32Window) this, "The trade has been saved successfully." + Environment.NewLine + "Would you like to update the loan files with these recent changes?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
          flag = this.commitTradeAssignments(false, updatedSelectedLoans);
        this.txtCommitmentNumber.Enabled = false;
        this.loadTradeData();
        this.modified = false;
        return flag;
      }
      catch (ObjectNotFoundException ex)
      {
        Tracing.Log(CorrespondentTradeEditor.sw, this.className, TraceLevel.Error, ex.ToString());
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
        Tracing.Log(CorrespondentTradeEditor.sw, this.className, TraceLevel.Error, ex.ToString());
        if (ex.Message.Contains("The loan has been assigned to another correspondent trade."))
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "The correspondent trade is saved successfully. However, the loan is not assigned to the trade, because the loan has been assigned to another correspondent trade.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          this.loadTradeData();
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

    public bool PreValidateCommit() => this.prevalidateCommit();

    private bool prevalidateCommit()
    {
      if (this.ctlSRP.ItemModified)
      {
        this.tabTrade.SelectedTab = this.tpPricing;
        if (!this.ctlSRP.ValidateChanges())
          return false;
      }
      return true;
    }

    private bool requiresLoanUpdates()
    {
      return this.LoanUpdatesRequired && this.assignments.GetAssignedLoans().Length != 0;
    }

    public bool UnsavedPendingLoan
    {
      get => this.assignments.HasPendingChanges() || this.requiresLoanUpdates();
    }

    private void saveTradeInfo()
    {
      int tradeId = this.trade.TradeID;
      if (tradeId < 0)
      {
        tradeId = Session.CorrespondentTradeManager.CreateTrade(this.trade);
        Session.CorrespondentTradeManager.PublishKafkaEvent("create", tradeId, (Hashtable) null);
      }
      else
      {
        try
        {
          Session.CorrespondentTradeManager.UpdateTrade(this.trade, true);
          Session.CorrespondentTradeManager.PublishKafkaEvent("update", this.trade.TradeID, (Hashtable) null);
        }
        catch (TradeNotUpdateException ex)
        {
          throw;
        }
      }
      this.trade = Session.CorrespondentTradeManager.GetTrade(tradeId);
      this.assignments.ApplyNewTradeID(tradeId);
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
      foreach (CorrespondentTradeLoanAssignment assignedPendingLoan in this.GetAllAssignedPendingLoans())
        assignedPendingLoan.TotalPrice = Utils.ParseDecimal((object) this.txtWABP.Text.Trim());
    }

    private bool commitTradeAssignments(bool forceUpdateOfAllLoans, bool selectedLoans)
    {
      try
      {
        CorrespondentTradeLoanAssignmentManager assignments = this.assignments;
        if (selectedLoans)
        {
          List<string> guids = ((IEnumerable<PipelineInfo>) this.ctlLoanList.GetSelectedAndPendingLoans()).Select<PipelineInfo, string>((System.Func<PipelineInfo, string>) (t => t.GUID)).ToList<string>();
          assignments.loans = ((IEnumerable<CorrespondentTradeLoanAssignment>) assignments.GetAllAssignedPendingLoans()).Where<CorrespondentTradeLoanAssignment>((System.Func<CorrespondentTradeLoanAssignment, bool>) (t => guids.Contains(t.Guid))).ToDictionary<CorrespondentTradeLoanAssignment, string, CorrespondentTradeLoanAssignment>((System.Func<CorrespondentTradeLoanAssignment, string>) (t => t.Guid), (System.Func<CorrespondentTradeLoanAssignment, CorrespondentTradeLoanAssignment>) (t => t));
          forceUpdateOfAllLoans = true;
        }
        new CorrespondentTradeProcesses((ITradeEditor) this).Execute(CorrespondentTradeProcesses.ActionType.Commit, this.trade, assignments, forceUpdateOfAllLoans || this.requiresLoanUpdates(), 0M);
        return true;
      }
      catch (TradeNotUpdateException ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      catch (Exception ex)
      {
        Tracing.Log(CorrespondentTradeEditor.sw, this.className, TraceLevel.Error, "Error applying loan status: " + (object) ex);
        int num = (int) Utils.Dialog((IWin32Window) this, "Unexpected error while attempting to update loans: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
    }

    private bool validateTradeData()
    {
      string str = this.txtCommitmentNumber.Text.Trim();
      bool flag1 = Session.ConfigurationManager.GetCompanySetting("TRADE", "ENABLEAUTOCOMMITMENTNUMBER") == "True";
      if (!flag1 && this.trade.TradeID <= 0)
        this.txtCommitmentNumber.Enabled = true;
      List<LoanToTradeAssignmentBase> assignedPendingLoans = this.GetAllAssignedPendingLoans();
      Decimal num1 = 0M;
      if (assignedPendingLoans != null && assignedPendingLoans.Count<LoanToTradeAssignmentBase>() > 0)
        num1 = assignedPendingLoans.Sum<LoanToTradeAssignmentBase>((System.Func<LoanToTradeAssignmentBase, Decimal>) (a => (Decimal) a.PipelineInfo.Info[(object) "Loan.TotalLoanAmount"]));
      if (this.trade.Tolerance == 0M && num1 > this.trade.TradeAmount || this.trade.Tolerance > 0M && num1 > this.trade.MaxAmount)
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "The assigned amount cannot be more than the maximum amount for the correspondent trade.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return false;
      }
      if (str.Length == 0 && this.trade.TradeID < 0 && !flag1 || this.trade.TradeID > 0 && str.Length == 0)
      {
        int num3 = (int) Utils.Dialog((IWin32Window) this, "You must enter a commitment # for this trade before saving.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return false;
      }
      if (string.Compare(str, this.originalTradeName, true) != 0)
      {
        bool flag2 = Session.CorrespondentTradeManager.CheckTradeByName(str);
        if (Session.CorrespondentTradeManager.CheckExistingTradeByName(str))
        {
          int num4 = (int) Utils.Dialog((IWin32Window) this, "A correspondent trade with the commitment # '" + str + "' already exists. You must enter a unique commitment # for this correspondent trade.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          return false;
        }
        if (flag2)
        {
          int num5 = (int) Utils.Dialog((IWin32Window) this, "A correspondent trade with the commitment # '" + str + "' was previously deleted. You must enter a unique commitment # for this correspondent trade.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          return false;
        }
      }
      if (this.isCMCTabEnabled && (this.cboMasters.SelectedIndex == -1 || this.cboMasters.SelectedIndex == 0) && Utils.Dialog((IWin32Window) this, "A Master Commitment # is required based on the current Trade Management settings. Are you sure you want to proceed?", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.No)
      {
        this.tabTrade.SelectedTab = this.tabTrade.TabPages["tpDetails"];
        return false;
      }
      if (this.cmbDeliveryType.SelectedIndex == -1)
      {
        int num6 = (int) Utils.Dialog((IWin32Window) this, "The Delivery Type cannot be blank.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return false;
      }
      if (this.trade.IsForIndividualLoan() && this.ctlLoanList.CurrentInTradeCount > 1)
      {
        int num7 = (int) Utils.Dialog((IWin32Window) this, "This Delivery Type \"" + this.trade.DeliveryType.ToDescription() + "\" must only contain one allocated loan.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return false;
      }
      DateTime dateTime = new DateTime();
      if (!this.trade.CommitmentDate.Equals(dateTime) && !this.trade.ExpirationDate.Equals(dateTime) && this.trade.CommitmentDate > this.trade.ExpirationDate)
      {
        int num8 = (int) Utils.Dialog((IWin32Window) this, "The Commitment Date must be before the Expiration Date.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      if (this.isCMCTabEnabled && this.selectedMaster != null && Session.CorrespondentMasterManager.CheckCorrespondentMasterByMasterNumber(this.selectedMaster.Name) == null)
      {
        int num9 = (int) Utils.Dialog((IWin32Window) this, "The specified master commitment cannot be found. It may have been deleted by another user.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      Decimal num10 = 0M;
      if (Convert.ToString(this.cboMasters.SelectedItem) != "N/A" && ((MasterCommitmentBase) this.cboMasters.SelectedItem).ID == this.originalMasterId)
        num10 = this.originalTradeAmount;
      if (Convert.ToString(this.cboMasters.SelectedItem) != "N/A" && this.trade.TradeAmount - num10 > this.GetAvailableAmount((CorrespondentMasterInfo) this.cboMasters.SelectedItem, 0M))
      {
        int num11 = (int) Utils.Dialog((IWin32Window) this, "The Correspondent Trade Amount should be equal to or less than the Available amount of the Master Commitment.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      Decimal originalTradeAmount = this.trade.TradeID > 0 ? this.originalTradeAmount : 0M;
      MasterCommitmentType commitmentType = this.trade.CommitmentType == CorrespondentTradeCommitmentType.BestEfforts ? MasterCommitmentType.BestEfforts : MasterCommitmentType.Mandatory;
      if ((!this.tpoSettings.CommitmentUseBestEffort || this.trade.CommitmentType != CorrespondentTradeCommitmentType.BestEfforts) && this.tpoSettings.CommitmentTradePolicy == ExternalOriginatorCommitmentTradePolicy.DontAllowTradeCreation && this.trade.TradeAmount - originalTradeAmount > Session.CorrespondentTradeManager.CalculateTPOAvailableAmount(commitmentType, this.tpoSettings))
      {
        int num12 = (int) Utils.Dialog((IWin32Window) this, "Trade Amount exceeds available Commitment Authority. Trade cannot be saved.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      if ((this.trade.SRPfromPPE || this.trade.AdjustmentsfromPPE) && this.trade.EPPSLoanProgramsFilter.Count<EPPSLoanProgramFilter>() == 0)
      {
        int num13 = (int) Utils.Dialog((IWin32Window) this, "\"Adjustments from PPE\" and \"SRP from PPE\" features on the Trade Pricing tab cannot be enabled when there are no \"Select ICE PPE Loan Programs\" populated on the Eligible Loans section of the Trade's Details tab.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.ctlSRP.SRPfromPPE = false;
        this.ctlAdjustments.AdjustmentfromPPE = false;
        return false;
      }
      if (this.trade.SRPfromPPE && this.ctlSRP.SRPTable.PricingItems.Count<SRPTable.PricingItem>() > 0 || this.trade.AdjustmentsfromPPE && this.ctlAdjustments.Adjustments.Count<TradePriceAdjustment>() > 0)
      {
        if (Utils.Dialog((IWin32Window) this, "You have selected \"Adjustment from PPE\" and/or \"SRP from PPE\" indicators. This action will remove all of the current pricing contained in the associated section(s). Do you wish to continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
        {
          if (this.trade.SRPfromPPE && this.ctlSRP.SRPTable.PricingItems.Count<SRPTable.PricingItem>() > 0)
            this.ctlSRP.SRPfromPPE = false;
          if (this.trade.AdjustmentsfromPPE && this.ctlAdjustments.Adjustments.Count<TradePriceAdjustment>() > 0)
            this.ctlAdjustments.AdjustmentfromPPE = false;
          return false;
        }
        if (this.trade.SRPfromPPE && this.ctlSRP.SRPTable.PricingItems.Count<SRPTable.PricingItem>() > 0)
        {
          this.trade.SRPTable = new SRPTable();
          this.ctlSRP.SRPTable = this.trade.SRPTable;
        }
        if (this.trade.AdjustmentsfromPPE && this.ctlAdjustments.Adjustments.Count<TradePriceAdjustment>() > 0)
        {
          this.trade.PriceAdjustments = new TradePriceAdjustments();
          this.ctlAdjustments.Adjustments = this.trade.PriceAdjustments;
        }
      }
      this.ctlLoanList.RefreshViews();
      if (this.trade.MaxAmount < this.ctlLoanList.GetAssignedAmount)
      {
        int num14 = (int) Utils.Dialog((IWin32Window) this, "The assigned amount cannot be more than the maximum amount for the correspondent trade.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      string errMsg;
      if (this.areIneligibleLoansAssigned(out errMsg))
      {
        if (errMsg.Trim().Contains("withdrawn"))
        {
          int num15 = (int) Utils.Dialog((IWin32Window) this, errMsg, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return false;
        }
        if (Utils.Dialog((IWin32Window) this, errMsg, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
          return false;
      }
      if (this.trade.Pricing.SimplePricingItems.Count > 0 && this.areUnpricedLoansAssigned() && Utils.Dialog((IWin32Window) this, "The trade has one or more loans assigned to it for which pricing cannot be determined based on your current pricing setup. Do you want to continue to save this trade?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
        return false;
      if (flag1 && this.trade.TradeID < 0 && this.trade.Name == string.Empty)
      {
        string nextAutoNumber = Session.CorrespondentTradeManager.GenerateNextAutoNumber();
        if (nextAutoNumber.Length > 18 || string.IsNullOrEmpty(nextAutoNumber))
        {
          int num16 = (int) Utils.Dialog((IWin32Window) this, "Correspondent Trade cannot be saved because the maximum number of commitment numbers has been reached. Please go to settings and adjust the starting number", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          return false;
        }
        this.trade.Name = "use_autonumber_reserved";
      }
      if (Utils.ToDate(this.dtCommitment.Text.Trim()) == DateTime.MinValue)
      {
        int num17 = (int) Utils.Dialog((IWin32Window) this, "Commitment Date field cannot be blank.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      if (this.txtAmount.Text.Trim() == "" || this.txtAmount.Text.Trim() == "0")
      {
        int num18 = (int) Utils.Dialog((IWin32Window) this, "Trade Amount field cannot be blank.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      if (this.txtTolerance.Text.Trim() == "")
      {
        int num19 = (int) Utils.Dialog((IWin32Window) this, "Tolerance field cannot be blank.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      if (this.txtWABP.Enabled && this.txtWABP.Text.Trim() == "")
      {
        int num20 = (int) Utils.Dialog((IWin32Window) this, "Weighted Average Bulk Price field cannot be blank.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      if (Utils.ToDate(this.dtExpirationDate.Text.Trim()) == DateTime.MinValue)
      {
        int num21 = (int) Utils.Dialog((IWin32Window) this, "Expiration Date field cannot be blank.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      if (Utils.ToDate(this.dtDeliveryExpirationDate.Text.Trim()) == DateTime.MinValue)
      {
        int num22 = (int) Utils.Dialog((IWin32Window) this, "Delivery Expiration Date field cannot be blank.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      if (this.trade.TradeID < 0 && string.Equals(Session.ConfigurationManager.GetCompanySetting("TRADE", "AllowPublishEvent"), "true", StringComparison.CurrentCultureIgnoreCase))
        this.trade.Status = TradeStatus.Unpublished;
      return true;
    }

    private bool areIneligibleLoansAssigned(out string errMsg)
    {
      string errMsg1 = string.Empty;
      CorrespondentTradeFilterEvaluator evaluator = (CorrespondentTradeFilterEvaluator) this.getCurrentFilter().CreateEvaluator(typeof (CorrespondentTradeFilterEvaluator));
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
        if (!this.trade.IsNoteRateAllowed(info))
          return true;
      }
      return false;
    }

    private void cboContract_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.loading)
        return;
      this.onLoanUpdatableFieldValueChanged(sender, e);
      if (this.cboMasters.SelectedIndex <= 0 || Utils.Dialog((IWin32Window) this, "Apply the selected contract's investor data to the current trade?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      this.commitChanges();
      if (Session.CorrespondentMasterManager.CheckCorrespondentMasterByMasterNumber(this.cboMasters.SelectedText) != null)
        return;
      int num = (int) Utils.Dialog((IWin32Window) this, "The specified master commitment cannot be found. It may have been deleted by another user.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
        if (sender is System.Windows.Forms.TextBox && ((Control) sender).Name == "txtAmount")
          this.loadTolerance();
        num2 = Utils.ParseDecimal((object) this.txtTolerance.Text);
      }
      catch
      {
      }
      System.Windows.Forms.TextBox txtMinAmt = this.txtMinAmt;
      Decimal num3 = num1 - num1 * num2 / 100M;
      string str1 = num3.ToString("#,##0;;\"\"");
      txtMinAmt.Text = str1;
      System.Windows.Forms.TextBox txtMaxAmt = this.txtMaxAmt;
      num3 = num1 + num1 * num2 / 100M;
      string str2 = num3.ToString("#,##0;;\"\"");
      txtMaxAmt.Text = str2;
      if (sender is System.Windows.Forms.TextBox && ((Control) sender).Name == "txtAmount")
      {
        this.trade.TradeAmount = num1;
        this.loadProfitabilityData();
      }
      this.modified = true;
    }

    private void onLoanUpdatableFieldValueChanged(object sender, EventArgs e)
    {
      this.modified = true;
      this.loanUpdatesRequired = true;
    }

    private void onCommitmentNumberChanged(object sender, EventArgs e)
    {
      this.modified = true;
      this.loanUpdatesRequired = true;
    }

    private void onExpirationDateChanged(object sender, EventArgs e)
    {
      if (this.lblDeliveryDays.Text.Trim() != string.Empty && this.dtExpirationDate.Text != string.Empty)
        this.dtDeliveryExpirationDate.Text = Utils.ParseDate((object) this.dtExpirationDate.Text).AddDays((double) Utils.ParseInt((object) this.lblDeliveryDays.Text)).ToString("MM/dd/yyyy");
      this.onLoanUpdatableFieldValueChanged(sender, e);
    }

    private void tabTrade_SelectedIndexChanged(object sender, EventArgs e)
    {
      Cursor.Current = Cursors.WaitCursor;
      if (this.tabTrade.SelectedTab == this.tpDetails)
      {
        this.recalculateProfitability();
        this.RefreshWeightedAvgBulkPrice();
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
      if (this.lastPricingTradeInfo == null || !CorrespondentTradeInfo.ComparePricing(this.lastPricingTradeInfo, this.trade))
        refreshLoans = true;
      else if (!TradeFilter.CompareFilters(this.lastEvaluatedFilter, this.trade.Filter))
        refreshLoans = true;
      else if (string.Compare(this.lastPricingTradeInfo.InvestorName, this.trade.InvestorName, true) != 0)
        refreshLoans = true;
      this.ctlLoanList.RefreshLoanList(refreshLoans);
      this.lastEvaluatedFilter = this.trade.Filter;
      this.lastPricingTradeInfo = new CorrespondentTradeInfo(this.trade);
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

    private void tpHistory_Resize(object sender, EventArgs e) => this.ReSizeNotesPanel();

    private void ReSizeNotesPanel()
    {
      int num = Math.Max(0, this.pnlHistory.Width - 5);
      this.grpNotes.Top = this.grpHistory.Top = 0;
      this.grpNotes.Height = this.gvNotes.Height = this.grpHistory.Height = this.pnlHistory.Height;
      this.grpNotes.Left = 0;
      this.grpNotes.Width = this.gvNotes.Width = num / 2;
      this.grpHistory.Width = Math.Max(0, num - this.grpNotes.Width);
      this.grpHistory.Left = this.grpNotes.Right + 5;
    }

    private void btnViewFilter_Click(object sender, EventArgs e)
    {
      string text = AdvancedSearchControl.GetFilterSummary(this.trade.Filter.FilterType != TradeFilterType.Advanced ? this.trade.Filter.GetSimpleFilter().ConvertToFilterList() : this.trade.Filter.GetAdvancedFilter());
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
        this.trade.Investor.CopyFrom(investor);
      else
        this.trade.Investor.Clear();
      this.recalculateProfitability();
      this.modified = true;
      this.loanUpdatesRequired = true;
    }

    private void btnSavedSearches_Click(object sender, EventArgs e)
    {
      using (TradeFilterTemplateSelector templateSelector = new TradeFilterTemplateSelector())
      {
        if (templateSelector.ShowDialog((IWin32Window) this) == DialogResult.Cancel)
          return;
        this.trade.Filter = ((TradeFilterTemplate) templateSelector.GetSelectedTemplate()).Filter;
        this.modified = true;
        this.loadSearch();
      }
    }

    private void tpDetails_Resize(object sender, EventArgs e)
    {
      int num = this.pnlDetails.Height - 5;
      this.pnlFilter.Top = 0;
      this.pnlAotInfo.Height = Math.Max(num / 3, this.pnlAotInfo.MinimumSize.Height);
      this.pnlRightBottom.Height = Math.Max(num / 3, this.pnlRightBottom.MinimumSize.Height);
    }

    private void tpPricing_Resize(object sender, EventArgs e)
    {
    }

    private void onCommitmentDateChanged(object sender, EventArgs e)
    {
      this.CheckNonBizDay();
      this.trade.CommitmentDate = this.dtCommitment.Value;
      if (Session.ConfigurationManager.GetCompanySetting("TRADE", "ENABLECORRESPONDENTMASTER") == "True")
      {
        this.screenData.ActiveContracts = Session.CorrespondentTradeManager.GetCorrespondentMasterInfos(this.trade);
        this.refreshMasters(this.screenData.ActiveContracts, this.screenData.AssignedContract);
      }
      this.onFieldValueChanged(sender, e);
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
        Tracing.Log(CorrespondentTradeEditor.sw, this.className, TraceLevel.Error, "Error during export: " + (object) ex);
        int num = (int) Utils.Dialog((IWin32Window) this, "An error occurred while attempting to export the loans to Microsoft Excel. Ensure that you have Excel installed and it is working properly.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void btnList_Click(object sender, EventArgs e)
    {
      this.grpHistoryDetails.Visible = false;
      this.ReSizeNotesPanel();
      TradeManagementConsole.Instance.CloseCorrespondentTrade();
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
        case "LTE_Save":
          this.SaveTrade(false, false);
          break;
        case "LTE_Exit":
          TradeManagementConsole.Instance.CloseCorrespondentTrade();
          break;
      }
    }

    public bool SetMenuItemState(ToolStripItem menuItem)
    {
      Control stateControl = (Control) null;
      switch (string.Concat(menuItem.Tag))
      {
        case "LTE_Save":
          stateControl = (Control) this.btnSave;
          break;
        case "LTE_Exit":
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

    private void editButton_Clicked(object sender, EventArgs e)
    {
      if (this.pairOffControl.SelectedPairOffs != null)
      {
        CommonPairOff selectedPairOff = (CommonPairOff) this.pairOffControl.SelectedPairOffs[0];
        if (selectedPairOff.Index == -1)
        {
          this.CurrentCorrespondentTradeInfo.CorrespondentTradePairOffs.Add(new CorrespondentTradePairOff(selectedPairOff.Index, selectedPairOff.Date, selectedPairOff.TradeAmount, selectedPairOff.PairOffFeePercentage, selectedPairOff.RequestedBy, selectedPairOff.Comments));
        }
        else
        {
          CorrespondentTradePairOff loanTradePairOff = this.CurrentCorrespondentTradeInfo.CorrespondentTradePairOffs[selectedPairOff.Index - 1];
          loanTradePairOff.Date = selectedPairOff.Date;
          loanTradePairOff.TradeAmount = selectedPairOff.TradeAmount;
          loanTradePairOff.PairOffFeePercentage = selectedPairOff.PairOffFeePercentage;
          CorrespondentTradePairOffs correspondentTradePairOffs = this.CurrentCorrespondentTradeInfo.CopyOfCorrespondentTradePairOffs;
          if ((correspondentTradePairOffs != null ? correspondentTradePairOffs.FirstOrDefault<CorrespondentTradePairOff>((System.Func<CorrespondentTradePairOff, bool>) (x => x.Guid == loanTradePairOff.Guid)) : (CorrespondentTradePairOff) null) != null)
          {
            loanTradePairOff.TradeHistoryAction = loanTradePairOff.TradeAmount < 0M ? TradeHistoryAction.PairOffReversed : TradeHistoryAction.PairOffUpdated;
            loanTradePairOff.ActionDateTime = DateTime.Now;
          }
          else
          {
            loanTradePairOff.TradeHistoryAction = loanTradePairOff.TradeAmount < 0M ? TradeHistoryAction.PairOffReversed : TradeHistoryAction.PairOffCreated;
            loanTradePairOff.ActionDateTime = DateTime.Now;
          }
        }
      }
      this.modified = this.pairOffControl.Modified;
      this.loadPairOffs();
      this.loadProfitabilityData();
    }

    private void deleteButton_Clicked(object sender, EventArgs e)
    {
      if (this.pairOffControl.SelectedPairOffs != null)
      {
        foreach (object selectedPairOff in this.pairOffControl.SelectedPairOffs)
        {
          CorrespondentTradePairOff pairOff = (CorrespondentTradePairOff) selectedPairOff;
          CorrespondentTradePairOff correspondentTradePairOff = this.CurrentCorrespondentTradeInfo.CopyOfCorrespondentTradePairOffs.FirstOrDefault<CorrespondentTradePairOff>((System.Func<CorrespondentTradePairOff, bool>) (x => x.Guid == pairOff.Guid));
          if (correspondentTradePairOff != null)
          {
            correspondentTradePairOff.TradeHistoryAction = TradeHistoryAction.PairOffDeleted;
            correspondentTradePairOff.ActionDateTime = DateTime.Now;
          }
          this.CurrentCorrespondentTradeInfo.CorrespondentTradePairOffs.Remove((CorrespondentTradePairOff) selectedPairOff);
        }
      }
      this.modified = this.pairOffControl.Modified;
      this.loadPairOffs();
      this.loadProfitabilityData();
    }

    private void standardIconButton1_Click(object sender, EventArgs e)
    {
      RxContactInfo rxContact = new RxContactInfo();
      using (RxBusinessContact rxBusinessContact = new RxBusinessContact("Servicing", rxContact.CompanyName, "", rxContact, CRMRoleType.NotFound, false, ""))
      {
        if (rxBusinessContact.ShowDialog((IWin32Window) this) != DialogResult.OK || !rxBusinessContact.GoToContact)
          return;
        Session.MainScreen.NavigateToContact(rxBusinessContact.SelectedContactInfo);
      }
    }

    private void refreshConfigurableFieldOptions()
    {
      this.cboSecurityTerm.Items.Clear();
      ArrayList secondaryFields1 = Session.ConfigurationManager.GetSecondaryFields(SecondaryFieldTypes.SecurityTerm);
      if (secondaryFields1 != null)
      {
        foreach (string str in secondaryFields1)
          this.cboSecurityTerm.Items.Add((object) str);
      }
      this.cboSecurityType.Items.Clear();
      foreach (DataRow row in (InternalDataCollectionBase) Session.ConfigurationManager.GetSecondarySecurityTypes().Rows)
        this.cboSecurityType.Items.Add(row["Name"]);
      this.cboxFundType.Items.Clear();
      this.cboxFundType.Items.Add((object) "Flow");
      this.cboxFundType.Items.Add((object) "Bulk");
      this.cboxRepWarrantType.Items.Clear();
      this.cboxRepWarrantType.Items.Add((object) "Bifurcated");
      this.cboxRepWarrantType.Items.Add((object) "Non Bifurcated");
      this.cboxAgencyName.Items.Clear();
      this.cboxAgencyName.Items.Add((object) "FNMA");
      this.cboxAgencyName.Items.Add((object) "FHLMC");
      this.cboxAgencyName.Items.Add((object) "GNMA");
      this.cboxAgencyDeliveryType.Items.Clear();
      this.cboxAgencyDeliveryType.Items.Add((object) "MBS");
      this.cboxAgencyDeliveryType.Items.Add((object) "Cash Window");
      this.cmbTradeDesc.Items.Clear();
      ArrayList secondaryFields2 = Session.ConfigurationManager.GetSecondaryFields(SecondaryFieldTypes.TradeDescriptionOption);
      if (secondaryFields2 == null)
        return;
      foreach (string str in secondaryFields2)
        this.cmbTradeDesc.Items.Add((object) str);
    }

    private void cmbPricingType_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.trade.Pricing.IsAdvancedPricing = false;
      this.placePricingTypeDDLToControls();
      this.tpPricing_Resize((object) null, (EventArgs) null);
      this.modified = true;
    }

    public void ResizeSimpleSearchControl() => this.ctlSimpleSearch.HideAdditionalDetails();

    private void txtMinNoteRate_TextChanged(object sender, EventArgs e)
    {
    }

    private void txtMaxNoteRate_TextChanged(object sender, EventArgs e)
    {
    }

    private void txtMinNoteRate_Validating(object sender, CancelEventArgs e)
    {
    }

    private void txtMaxNoteRate_Validating(object sender, CancelEventArgs e)
    {
    }

    private void btnSelector_Click(object sender, EventArgs e) => this.showRolodex();

    private void showRolodex()
    {
      using (RxBusinessContact rxBusinessContact = new RxBusinessContact("Dealer", this.txtOriginalTradeDealer.Text, (string) null, (RxContactInfo) null, CRMRoleType.NotFound, false, ""))
      {
        if (rxBusinessContact.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        if (rxBusinessContact.GoToContact)
          Session.MainScreen.NavigateToContact(rxBusinessContact.SelectedContactInfo);
        else
          this.txtOriginalTradeDealer.Text = rxBusinessContact.RxContactRecord.CompanyName;
      }
    }

    private void btnDocCustodian_Click(object sender, EventArgs e)
    {
      using (RxBusinessContact rxBusinessContact = new RxBusinessContact("", this.txtOriginalTradeDealer.Text, (string) null, (RxContactInfo) null, CRMRoleType.NotFound, false, ""))
      {
        if (rxBusinessContact.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        if (rxBusinessContact.GoToContact)
          Session.MainScreen.NavigateToContact(rxBusinessContact.SelectedContactInfo);
        else
          this.txtDocCustodian.Text = rxBusinessContact.RxContactRecord.CompanyName;
      }
    }

    private void cmbDeliveryType_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.SetDeliveryType();
      System.Web.UI.WebControls.ListItem item = (System.Web.UI.WebControls.ListItem) this.cmbDeliveryType.SelectedItem;
      Decimal num;
      if (item != null && int.Parse(item.Value) == 40)
      {
        this.panel15.Visible = false;
        this.panel3.Visible = true;
        this.grpAOTInformation.Text = "Co-Issue Information";
        this.txtOriginalTradeDealer.Text = string.Empty;
        this.cboSecurityTerm.Text = string.Empty;
        this.txtSecurityCoupon.Text = string.Empty;
        this.txtSecurityPrice.Text = string.Empty;
        this.cboSecurityType.Text = string.Empty;
        this.dtSettlementDate.Value = DateTime.MinValue;
        this.dtOriginalTradeDate.Value = DateTime.MinValue;
        this.txtOriginalTradeDealer.Text = string.Empty;
        this.cboxFundType.Text = this.trade.FundType;
        this.cboxRepWarrantType.Text = this.trade.OriginationRepWarrantType;
        this.cboxAgencyName.Text = this.trade.AgencyName;
        this.cboxAgencyDeliveryType.Text = this.trade.AgencyDeliveryType;
        this.txtDocCustodian.Text = this.trade.DocCustodian;
      }
      else
      {
        this.panel15.Visible = true;
        this.panel3.Visible = false;
        this.grpAOTInformation.Text = "AOT Information";
        this.cboxFundType.Text = string.Empty;
        this.cboxRepWarrantType.Text = string.Empty;
        this.cboxAgencyName.Text = string.Empty;
        this.cboxAgencyDeliveryType.Text = string.Empty;
        this.txtDocCustodian.Text = string.Empty;
        this.txtOriginalTradeDealer.Text = this.trade.AOTOriginalTradeDealer;
        this.cboSecurityTerm.Text = this.trade.AOTSecurityTerm;
        System.Windows.Forms.TextBox txtSecurityCoupon = this.txtSecurityCoupon;
        num = this.trade.AOTSecurityCoupon;
        string str1 = num.ToString("#,##0.00000;;\"\"");
        txtSecurityCoupon.Text = str1;
        System.Windows.Forms.TextBox txtSecurityPrice = this.txtSecurityPrice;
        num = this.trade.AOTSecurityPrice;
        string str2 = num.ToString("#,##0.0000000;;\"\"");
        txtSecurityPrice.Text = str2;
        this.cboSecurityType.Text = this.trade.AOTSecurityType;
        this.dtSettlementDate.Value = this.trade.AOTSettlementDate;
        this.dtOriginalTradeDate.Value = this.trade.AOTOriginalTradeDate;
        this.txtOriginalTradeDealer.Text = this.trade.AOTOriginalTradeDealer;
      }
      if (this.loading)
        return;
      this.modified = true;
      if (this.isCMCTabEnabled && this.cboMasters.SelectedItem.ToString() != "N/A")
      {
        MasterCommitmentDeliveryInfo commitmentDeliveryInfo = ((MasterCommitmentBase) this.cboMasters.SelectedItem).DeliveryInfos.Where<MasterCommitmentDeliveryInfo>((System.Func<MasterCommitmentDeliveryInfo, bool>) (x => x.Type == (CorrespondentMasterDeliveryType) int.Parse(item.Value))).FirstOrDefault<MasterCommitmentDeliveryInfo>();
        if (commitmentDeliveryInfo != null)
        {
          System.Windows.Forms.TextBox txtTolerance = this.txtTolerance;
          num = commitmentDeliveryInfo.Tolerance;
          string str = num.ToString("#,##0.000000000;;\"\"");
          txtTolerance.Text = str;
          this.lblDeliveryDays.Text = commitmentDeliveryInfo.DeliveryDays.ToString();
          if (this.dtExpirationDate.Text != string.Empty)
            this.dtDeliveryExpirationDate.Text = Utils.ParseDate((object) this.dtExpirationDate.Text).AddDays((double) commitmentDeliveryInfo.DeliveryDays).ToString("MM/dd/yyyy");
        }
      }
      this.DisplayPricingTab();
      if (item != null && (int.Parse(item.Value) == 30 || int.Parse(item.Value) == 35))
        this.ctlLoanList.DisableViewEligible();
      else
        this.ctlLoanList.EnableViewEligible();
      this.onLoanUpdatableFieldValueChanged(sender, e);
      this.RefreshWeightedAvgBulkPrice();
    }

    private void SetDeliveryType()
    {
      System.Web.UI.WebControls.ListItem selectedItem = (System.Web.UI.WebControls.ListItem) this.cmbDeliveryType.SelectedItem;
      if (selectedItem == null || int.Parse(selectedItem.Value) != 30 && int.Parse(selectedItem.Value) != 35)
      {
        this.lbtnWABP.Enabled = false;
        this.txtWABP.Enabled = false;
        this.txtWABP.Text = "";
      }
      else
      {
        this.lbtnWABP.Enabled = true;
        this.txtWABP.Enabled = this.lbtnWABP.Locked;
      }
    }

    private void InsertPricingTab()
    {
      if (this.pricingTab == null || this.tabTrade.TabPages["tpPricing"] != null)
        return;
      this.tabTrade.TabPages.Insert(1, this.pricingTab);
    }

    private void ClearPricingTab()
    {
      switch (this.getSelectedDeliveryType(this.cmbDeliveryType))
      {
        case CorrespondentMasterDeliveryType.IndividualBestEfforts:
        case CorrespondentMasterDeliveryType.IndividualMandatory:
        case CorrespondentMasterDeliveryType.Bulk:
        case CorrespondentMasterDeliveryType.BulkAOT:
          this.trade.Pricing.SimplePricingItems.Clear();
          this.trade.PriceAdjustments.Clear();
          this.trade.SRPTable.PricingItems.Clear();
          this.ctlPricing.PricingItems = this.trade.Pricing.SimplePricingItems;
          this.ctlAdjustments.Adjustments = this.trade.PriceAdjustments;
          this.ctlSRP.SRPTable = this.trade.SRPTable;
          break;
      }
    }

    private void DisplayPricingTab()
    {
      switch (this.getSelectedDeliveryType(this.cmbDeliveryType))
      {
        case CorrespondentMasterDeliveryType.IndividualBestEfforts:
        case CorrespondentMasterDeliveryType.IndividualMandatory:
        case CorrespondentMasterDeliveryType.Bulk:
        case CorrespondentMasterDeliveryType.BulkAOT:
          if (this.tabTrade.TabPages["tpPricing"] == null)
            break;
          this.pricingTab = this.tabTrade.TabPages["tpPricing"];
          this.tabTrade.TabPages.Remove(this.tabTrade.TabPages["tpPricing"]);
          break;
        default:
          if (this.pricingTab == null || this.tabTrade.TabPages["tpPricing"] != null)
            break;
          this.tabTrade.TabPages.Insert(1, this.pricingTab);
          break;
      }
    }

    private bool ConfirmRemovingPricingDialog()
    {
      bool flag1 = false;
      bool flag2 = this.checkIfPricingTabRemoved(this.priorDeliveryType);
      bool flag3 = this.checkIfPricingTabRemoved(this.getSelectedDeliveryType(this.cmbDeliveryType));
      if (((this.trade.TradeID <= 0 ? 0 : (!flag2 ? 1 : 0)) & (flag3 ? 1 : 0)) != 0)
      {
        if (Utils.Dialog((IWin32Window) this, "Changing to the " + this.cmbDeliveryType.Text + " Delivery Type removes the Pricing tab and any of its previously entered parameters and any previously allocated loans may need to be re-priced.  Do you wish to continue?", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK)
        {
          this.cmbDeliveryType.Text = this.selectedDeliveryTypeText(this.priorDeliveryType);
          flag1 = true;
        }
        else
          this.priorDeliveryType = (CorrespondentMasterDeliveryType) int.Parse(((System.Web.UI.WebControls.ListItem) this.cmbDeliveryType.SelectedItem).Value);
      }
      this.ClearPricingTab();
      this.DisplayPricingTab();
      return flag1;
    }

    private bool checkIfPricingTabRemoved(CorrespondentMasterDeliveryType deliveryType)
    {
      return deliveryType == CorrespondentMasterDeliveryType.IndividualBestEfforts || deliveryType == CorrespondentMasterDeliveryType.IndividualMandatory || deliveryType == CorrespondentMasterDeliveryType.Bulk || deliveryType == CorrespondentMasterDeliveryType.BulkAOT;
    }

    public void InitializeDuplicateTrade(CorrespondentTradeInfo trade)
    {
      bool flag = false;
      int index;
      for (index = 0; index < this.cboMasters.Items.Count; ++index)
      {
        if (this.cboMasters.Items[index].GetType() == typeof (CorrespondentMasterInfo) && ((MasterCommitmentBase) this.cboMasters.Items[index]).ID == trade.CorrespondentMasterID)
        {
          flag = true;
          break;
        }
      }
      if (flag)
        this.cboMasters.SelectedIndex = index;
      this.cmbDeliveryType.SelectedItem = (object) new System.Web.UI.WebControls.ListItem(trade.DeliveryType.ToDescription(), ((int) trade.DeliveryType).ToString());
      this.originalTradeAmount = 0M;
    }

    private Decimal GetAvailableAmount(CorrespondentMasterInfo masterInfo, Decimal commitmentAmount)
    {
      return this.tpoSettings == null ? commitmentAmount : masterInfo.CommitmentAmount + CorrespondentMasterCalculation.CalculateAvailableAmountForCmc(commitmentAmount, this.tpoSettings.CommitmentUseBestEffortLimited, Session.CorrespondentTradeManager.GetTradeInfosByMasterId(masterInfo.ID));
    }

    private void lbtnCommitmentNumber_Click(object sender, EventArgs e)
    {
      if (this.readOnly)
        return;
      this.lbtnCommitmentNumber.Locked = !this.lbtnCommitmentNumber.Locked;
      this.txtCommitmentNumber.Enabled = this.lbtnCommitmentNumber.Locked;
      this.trade.OverrideTradeName = this.lbtnCommitmentNumber.Locked;
      if (this.lbtnCommitmentNumber.Locked)
      {
        if (this.txtCommitmentNumber.Text.Length <= 18)
          return;
        int num = (int) Utils.Dialog((IWin32Window) this, "The Commitment number for an auto-created Correspondent Trade can only be 18 digits long.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        this.txtCommitmentNumber.Text = "";
      }
      else
        this.txtCommitmentNumber.Text = this.trade.Name;
    }

    public bool SaveTrade() => throw new NotImplementedException();

    public void SetNoteHistoryTab() => this.tabTrade.SelectedTab = this.tpHistory;

    private void lbtnWABP_Click(object sender, EventArgs e)
    {
      if (this.readOnly)
        return;
      this.lbtnWABP.Locked = !this.lbtnWABP.Locked;
      this.txtWABP.Enabled = this.lbtnWABP.Locked;
      if (this.txtWABP.Enabled)
        this.txtWABP.Text = "";
      else
        this.RefreshWeightedAvgBulkPrice();
    }

    public void RefreshWeightedAvgBulkPrice()
    {
      if (!this.IsCalculatedWABP())
        return;
      List<LoanToTradeAssignmentBase> assignedPendingLoans = this.GetAllAssignedPendingLoans();
      this.SetDeliveryTypeToObject();
      if (assignedPendingLoans != null && assignedPendingLoans.Count<LoanToTradeAssignmentBase>() > 0)
      {
        Decimal weightedAvgBulkPrice = CorrespondentTradeCalculation.CalculateWeightedAvgBulkPrice(assignedPendingLoans.Sum<LoanToTradeAssignmentBase>((System.Func<LoanToTradeAssignmentBase, Decimal>) (a => ((CorrespondentTradeLoanAssignment) a).TotalPrice * (Decimal) a.PipelineInfo.Info[(object) "Loan.TotalLoanAmount"])), assignedPendingLoans.Sum<LoanToTradeAssignmentBase>((System.Func<LoanToTradeAssignmentBase, Decimal>) (a => (Decimal) a.PipelineInfo.Info[(object) "Loan.TotalLoanAmount"])), this.trade, true);
        this.txtWABP.Text = weightedAvgBulkPrice == 0M ? "0.00000" : Utils.ParseDecimal((object) weightedAvgBulkPrice, 5M).ToString("#,##0.00000;;\"\"");
      }
      else
        this.txtWABP.Text = "0.00000";
    }

    public Decimal GetLoanTotalPrice(CorrespondentTradeLoanAssignment loanInfo)
    {
      if (!this.IsBulkPricing())
        return 0M;
      return this.IsCalculatedWABP() ? loanInfo.TotalPrice : Utils.ParseDecimal((object) this.txtWABP.Text.Trim());
    }

    private void txtWABP_TextChanged(object sender, EventArgs e)
    {
      this.onLoanUpdatableFieldValueChanged(sender, e);
    }

    private void SetDeliveryTypeToObject()
    {
      if (this.cmbDeliveryType.SelectedIndex < 0)
        this.trade.DeliveryType = CorrespondentMasterDeliveryType.None;
      else
        this.trade.DeliveryType = (CorrespondentMasterDeliveryType) int.Parse(((System.Web.UI.WebControls.ListItem) this.cmbDeliveryType.SelectedItem).Value);
    }

    private void SetLoanlistViewEligible()
    {
      if (this.cmbDeliveryType.SelectedItem == null)
        return;
      if (int.Parse(((System.Web.UI.WebControls.ListItem) this.cmbDeliveryType.SelectedItem).Value) == 30 || int.Parse(((System.Web.UI.WebControls.ListItem) this.cmbDeliveryType.SelectedItem).Value) == 35)
        this.ctlLoanList.DisableViewEligible();
      else
        this.ctlLoanList.EnableViewEligible();
    }

    private void CheckNonBizDay()
    {
      BusinessCalendar expirationCalendar = LockDeskHoursUtils.GetLockExpirationCalendar(Session.StartupInfo, Session.SessionObjects);
      System.Web.UI.WebControls.ListItem selectedItem = (System.Web.UI.WebControls.ListItem) this.cmbDeliveryType.SelectedItem;
      CorrespondentMasterDeliveryType masterDeliveryType = CorrespondentMasterDeliveryType.None;
      if (selectedItem != null)
        masterDeliveryType = (CorrespondentMasterDeliveryType) Utils.ParseInt((object) selectedItem.Value.ToString());
      if (expirationCalendar == null || !(this.dtCommitment.Value != DateTime.MinValue) || masterDeliveryType != CorrespondentMasterDeliveryType.None && masterDeliveryType != CorrespondentMasterDeliveryType.AOT && masterDeliveryType != CorrespondentMasterDeliveryType.LiveTrade && masterDeliveryType != CorrespondentMasterDeliveryType.Forwards || expirationCalendar.IsBusinessDay(this.dtCommitment.Value) || Utils.Dialog((IWin32Window) this, "The Commitment Date entered falls on a non-business day. Do you wish to continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.No)
        return;
      this.dtCommitment.Value = DateTime.MinValue;
    }

    private void label13_DoubleClick(object sender, EventArgs e)
    {
      int num = (int) Utils.Dialog((IWin32Window) this, "Effective Trade Date Time : " + this.trade.CommitmentDateTime.ToString("MM/dd/yyyy hh:mm ") + "PM (ET)", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    }

    public void CommitSelectedLoansToTradeExtension(string tradeExtensionInfo)
    {
      try
      {
        CorrespondentTradeLoanAssignmentManager assignments = this.assignments;
        List<string> guids = ((IEnumerable<PipelineInfo>) this.ctlLoanList.GetSelectedAndPendingLoans()).Select<PipelineInfo, string>((System.Func<PipelineInfo, string>) (t => t.GUID)).ToList<string>();
        assignments.loans = ((IEnumerable<CorrespondentTradeLoanAssignment>) assignments.GetAllAssignedPendingLoans()).Where<CorrespondentTradeLoanAssignment>((System.Func<CorrespondentTradeLoanAssignment, bool>) (t => guids.Contains(t.Guid))).ToDictionary<CorrespondentTradeLoanAssignment, string, CorrespondentTradeLoanAssignment>((System.Func<CorrespondentTradeLoanAssignment, string>) (t => t.Guid), (System.Func<CorrespondentTradeLoanAssignment, CorrespondentTradeLoanAssignment>) (t => t));
        assignments.UpdateAssignementsWithTradeExtension(this.trade.TradeID, guids.ToArray(), tradeExtensionInfo);
        new CorrespondentTradeProcesses((ITradeEditor) this).Execute(CorrespondentTradeProcesses.ActionType.ExtendLock, new Dictionary<CorrespondentTradeInfo, CorrespondentTradeLoanAssignmentManager>()
        {
          {
            this.trade,
            assignments
          }
        }, new int[1]{ this.trade.TradeID }, false, 0M, tradeExtensionInfo: tradeExtensionInfo);
      }
      catch (TradeNotUpdateException ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      catch (Exception ex)
      {
        Tracing.Log(CorrespondentTradeEditor.sw, this.className, TraceLevel.Error, "Error extend lock for loans: " + (object) ex);
        int num = (int) Utils.Dialog((IWin32Window) this, "Unexpected error while attempting to extend lock for  loans: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void lbtnTolerance_Click(object sender, EventArgs e)
    {
      if (this.readOnly)
        return;
      this.lbtnTolerance.Locked = !this.lbtnTolerance.Locked;
      this.txtTolerance.Enabled = this.lbtnTolerance.Locked;
      if (this.txtTolerance.Enabled)
        this.txtTolerance.Text = "";
      else
        this.loadTolerance();
    }

    private void gvHistory_ItemClick(object source, GVItemEventArgs e)
    {
      this.grpHistoryDetails.Visible = true;
      int num = Math.Max(0, this.pnlHistory.Width - 5);
      this.grpNotes.Top = this.grpHistory.Top = 0;
      this.grpNotes.Height = this.gvNotes.Height = this.grpHistory.Height = this.grpHistoryDetails.Height = this.pnlHistory.Height;
      this.grpNotes.Left = 0;
      this.grpNotes.Width = this.gvNotes.Width = this.grpHistory.Width = num / 3;
      this.grpHistory.Left = this.grpNotes.Right + 5;
      this.grpHistoryDetails.Width = Math.Max(0, num - (this.grpHistory.Width + this.grpNotes.Width)) - 5;
      this.grpHistoryDetails.Left = this.grpHistory.Right + 5;
      this.ShowPairoffHistoryDetails((CorrespondentTradeHistoryItem) e.Item.Tag);
    }

    private void ShowPairoffHistoryDetails(CorrespondentTradeHistoryItem item)
    {
      this.txtHistoryDetails.Text = string.Empty;
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      string str1 = string.Empty;
      if (item.PriorTradeValues == null || item.PriorTradeValues.Count == 0)
      {
        StringBuilder stringBuilder = new StringBuilder();
        foreach (KeyValuePair<string, string> keyValuePair in (Dictionary<string, string>) item.Data)
        {
          if (keyValuePair.Key != "UserName")
          {
            string key = keyValuePair.Key;
            string str2 = keyValuePair.Value;
            if ((key == "MinAmount" || key == "MaxAmount" || key == "TradeAmount") && item.Action != TradeHistoryAction.PairOffCreated && item.Action != TradeHistoryAction.PairOffUpdated && item.Action != TradeHistoryAction.PairOffReversed && item.Action != TradeHistoryAction.PairOffDeleted)
              str2 = Math.Round(Convert.ToDecimal(str2), 0).ToString("c0");
            str1 = Regex.Replace(this.getDisplayText(key), "((?<=\\p{Ll})\\p{Lu})|((?!\\A)\\p{Lu}(?>\\p{Ll}))", " $0");
            if (str2 == "LiveTrade")
              str2 = "Direct Trade";
            stringBuilder.Append(this.getDisplayText(key) + " : " + str2 + " " + Environment.NewLine);
          }
        }
        this.txtHistoryDetails.Text = stringBuilder.ToString();
      }
      else
      {
        foreach (KeyValuePair<string, string> keyValuePair in (Dictionary<string, string>) item.Data)
        {
          if (keyValuePair.Key != "UserName")
          {
            string key = keyValuePair.Key;
            string str3 = keyValuePair.Value;
            if ((key == "MinAmount" || key == "MaxAmount" || key == "TradeAmount") && item.Action != TradeHistoryAction.PairOffCreated && item.Action != TradeHistoryAction.PairOffUpdated && item.Action != TradeHistoryAction.PairOffReversed && item.Action != TradeHistoryAction.PairOffDeleted)
            {
              this.txtHistoryDetails.SelectionColor = !item.PriorTradeValues.ContainsKey(key) || !(Math.Round(Convert.ToDecimal(item.PriorTradeValues[key]), 0) != Math.Round(Convert.ToDecimal(str3), 0)) ? Color.Black : Color.Red;
              str3 = Math.Round(Convert.ToDecimal(str3), 0).ToString("c0");
            }
            else
              this.txtHistoryDetails.SelectionColor = !item.PriorTradeValues.ContainsKey(key) || !(item.PriorTradeValues[key] != keyValuePair.Value) ? Color.Black : Color.Red;
            string str4 = Regex.Replace(this.getDisplayText(key), "((?<=\\p{Ll})\\p{Lu})|((?!\\A)\\p{Lu}(?>\\p{Ll}))", " $0");
            if (str3 == "LiveTrade")
              str3 = "Direct Trade";
            if (item.Action == TradeHistoryAction.PairOffReversed)
              this.txtHistoryDetails.SelectionColor = Color.Black;
            RichTextBox txtHistoryDetails = this.txtHistoryDetails;
            txtHistoryDetails.SelectedText = txtHistoryDetails.SelectedText + str4 + " : " + str3 + " " + Environment.NewLine;
          }
        }
      }
    }

    private string getDisplayText(string key)
    {
      string displayText = key;
      switch (key)
      {
        case "TradeName":
          displayText = "CommitmentNumber";
          break;
        case "ContractNumber":
          displayText = "MasterCommitment #";
          break;
      }
      return displayText;
    }

    private CorrespondentMasterDeliveryType getSelectedDeliveryType(ComboBox comboBoxDeliveryType)
    {
      if (comboBoxDeliveryType.SelectedItem == null)
        return CorrespondentMasterDeliveryType.None;
      return (CorrespondentMasterDeliveryType) Enum.Parse(typeof (CorrespondentMasterDeliveryType), ((System.Web.UI.WebControls.ListItem) comboBoxDeliveryType.SelectedItem).Value);
    }

    private string selectedDeliveryTypeText(CorrespondentMasterDeliveryType deliveryType)
    {
      foreach (System.Web.UI.WebControls.ListItem listItem in this.cmbDeliveryType.Items)
      {
        if (listItem.Value.Equals((object) (int) deliveryType))
          return listItem.Text;
      }
      return string.Empty;
    }

    public bool CheckLoanAmountOfAssignedLoans()
    {
      bool flag = false;
      foreach (CorrespondentTradeLoanAssignment assignedPendingLoan in this.assignments.GetAllAssignedPendingLoans())
      {
        if (assignedPendingLoan.PipelineInfo.Info[(object) "Loan.TotalLoanAmount"] is DBNull || assignedPendingLoan.PipelineInfo.Info[(object) "Loan.TotalLoanAmount"] == null)
        {
          assignedPendingLoan.PipelineInfo.Info[(object) "Loan.TotalLoanAmount"] = (object) 0.0M;
          flag = true;
        }
      }
      return flag;
    }

    private void gvNotes_DoubleClick(object sender, EventArgs e) => this.btnEdit_Click(sender, e);

    private void btnEdit_Click(object sender, EventArgs e)
    {
      if (this.gvNotes.SelectedItems.Count <= 0)
        return;
      this.NotesUser = Convert.ToString((object) this.gvNotes.SelectedItems[0].SubItems[2]);
      this.NoteDetails = Convert.ToString((object) this.gvNotes.SelectedItems[0].SubItems[1]);
      this.NotesDateTime = Convert.ToString((object) this.gvNotes.SelectedItems[0].SubItems[0]);
      Notes notes = new Notes(this.NoteDetails, this.NotesUser, "edit", this.NotesDateTime);
      int num = (int) notes.ShowDialog();
      this.NotesOpreation = notes.notesOpreation;
      this.gvNotes.SelectedItems[0].SubItems[1].Text = notes.noteDetails;
      this.modified = true;
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
      if (this.gvNotes.SelectedItems.Count < 1 || Utils.Dialog((IWin32Window) this, string.Format("Are you sure you want to remove the selected Notes Details?"), MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
        return;
      foreach (GVItem selectedItem in this.gvNotes.SelectedItems)
        this.gvNotes.Items.Remove(selectedItem);
      this.modified = true;
    }

    private void gvNotes_Click(object sender, EventArgs e)
    {
      this.btnEdit.Enabled = false;
      this.btnDelete.Enabled = false;
      if (this.gvNotes.SelectedItems.Count == 1)
      {
        this.btnEdit.Enabled = true;
        this.btnDelete.Enabled = true;
      }
      else if (this.gvNotes.SelectedItems.Count > 1)
      {
        this.btnEdit.Enabled = false;
        this.btnDelete.Enabled = true;
      }
      else
      {
        this.btnEdit.Enabled = false;
        this.btnDelete.Enabled = false;
      }
    }

    private void gvNotes_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.gvNotes_Click(sender, (EventArgs) null);
    }

    private string GetNotesFromGridView()
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvNotes.Items)
        stringBuilder.AppendLine(Convert.ToString((object) gvItem.SubItems[0]) + " " + this.getInitialUserName(gvItem.SubItems[2]) + "> " + Convert.ToString((object) gvItem.SubItems[1]));
      return TradeNoteUtils.SerializeTradeNotes(Convert.ToString((object) stringBuilder), Session.UserInfo.Userid, Session.UserInfo.FullName, Session.SessionObjects);
    }

    private void LoadNotesGridView(string tradeNotesJson)
    {
      TradeNoteModel[] tradeNoteModelArray = TradeNoteUtils.DeserializeTradeNotes(tradeNotesJson);
      this.gvNotes.Items.Clear();
      if (tradeNoteModelArray == null || tradeNoteModelArray.Length == 0)
        return;
      string empty = string.Empty;
      for (int index = 0; index < tradeNoteModelArray.Length; ++index)
      {
        GVItem gvItem = new GVItem();
        if (tradeNoteModelArray[index].CreatedTimeStamp.HasValue)
        {
          GVSubItemCollection subItems = gvItem.SubItems;
          DateTime? date = TradeNoteUtils.GetDate(tradeNoteModelArray[index].TimezoneAbbrev ?? string.Empty, tradeNoteModelArray[index].CreatedTimeStamp.Value, "from");
          ref DateTime? local = ref date;
          string str = (local.HasValue ? local.GetValueOrDefault().ToString("MM/dd/yyyy h:mm tt") : (string) null) + " (" + tradeNoteModelArray[index].TimezoneAbbrev + ")";
          subItems.Add((object) str);
        }
        else
          gvItem.SubItems.Add((object) string.Empty);
        gvItem.SubItems.Add((object) tradeNoteModelArray[index].Details);
        if (tradeNoteModelArray[index].CreatedTimeStamp.HasValue)
        {
          string entityName = tradeNoteModelArray[index].CreateBy.EntityName;
          GVSubItem gvSubItem = new GVSubItem((object) this.formatUserName(entityName));
          if (entityName.Contains(","))
            gvSubItem.Tag = (object) entityName;
          gvItem.SubItems.Add(gvSubItem);
        }
        else
          gvItem.SubItems.Add((object) string.Empty);
        this.gvNotes.Items.Add(gvItem);
      }
    }

    private void btnAddNotes_Click(object sender, EventArgs e)
    {
      Notes notes = new Notes();
      this.NotesOpreation = "new";
      int num = (int) notes.ShowDialog();
      this.NoteDetails = notes.noteDetails;
      this.NotesDateTime = notes.notesDateTime;
      if (this.NotesOpreation == "new" && notes.notesOpreation != "Cancel")
      {
        this.gvNotes.Items.Add(new GVItem()
        {
          SubItems = {
            (object) this.NotesDateTime,
            (object) this.NoteDetails,
            (object) Session.UserInfo.FullName
          }
        });
        this.modified = true;
      }
      else
      {
        if (!(notes.notesOpreation == "Edit"))
          return;
        this.gvNotes.SelectedItems[0].SubItems[1].Text = this.NoteDetails;
        this.modified = true;
      }
    }

    private void cmbTradeDesc_TextChanged(object sender, EventArgs e) => this.modified = true;

    private string formatUserName(string userName)
    {
      if (string.IsNullOrEmpty(userName))
        return string.Empty;
      string str = userName;
      if (userName.Contains(","))
      {
        string[] strArray = userName.Split(',');
        if (strArray.Length != 0)
          str = strArray[1].Trim() + " ";
        str += strArray[0].Trim();
      }
      return str;
    }

    private string getInitialUserName(GVSubItem item)
    {
      return item.Tag == null ? item.Text : item.Tag.ToString();
    }

    private void numField_KeyUp(object sender, KeyEventArgs e)
    {
      string.Concat(((Control) sender).Tag);
      System.Windows.Forms.TextBox textBox = (System.Windows.Forms.TextBox) sender;
      bool needsUpdate = false;
      string str = Utils.FormatInput(textBox.Text, FieldFormat.DECIMAL_9, ref needsUpdate);
      if (!needsUpdate)
        return;
      textBox.Text = str;
      textBox.SelectionStart = str.Length;
    }

    private void numField_KeyPress(object sender, KeyPressEventArgs e)
    {
      System.Windows.Forms.TextBox textBox = (System.Windows.Forms.TextBox) sender;
      textBox.MaxLength = 13;
      bool needsUpdate = false;
      if (char.IsControl(e.KeyChar))
      {
        if (e.KeyChar != '\b' && e.KeyChar != '.' || string.IsNullOrEmpty(textBox.Text) || !(textBox.Text != ".") || Convert.ToDouble(textBox.Text) > 100.0 || (textBox.SelectedText.Length != 0 || textBox.SelectionStart <= 0 || textBox.Text.Length <= 1 || !(textBox.Text.Remove(textBox.SelectionStart - 1, 1) != ".") || Convert.ToDouble(textBox.Text.Remove(textBox.SelectionStart - 1, 1)) <= 100.0) && (textBox.SelectedText.Length <= 0 || textBox.SelectedText.Length == textBox.Text.Length || !(textBox.Text.Remove(textBox.SelectionStart, textBox.SelectedText.Length) != ".") || Convert.ToDouble(textBox.Text.Remove(textBox.SelectionStart, textBox.SelectedText.Length)) <= 100.0))
          return;
        e.Handled = true;
      }
      else
      {
        if (!char.IsNumber(e.KeyChar) && e.KeyChar != '.')
          return;
        int selectionStart = textBox.SelectionStart;
        int startIndex = textBox.SelectionStart + textBox.SelectionLength;
        string str = Utils.FormatInput(textBox.Text.Substring(0, selectionStart) + e.KeyChar.ToString() + textBox.Text.Substring(startIndex), FieldFormat.DECIMAL_9, ref needsUpdate);
        e.Handled = needsUpdate;
        if (needsUpdate)
          textBox.MaxLength = str.Length;
        if (e.Handled || string.IsNullOrEmpty(textBox.Text) || Convert.ToDouble(str) <= 100.0)
          return;
        textBox.MaxLength = textBox.Text.Length;
        e.Handled = true;
      }
    }

    private void subscribeEventHandler()
    {
      if (this.trade.status != TradeStatus.Pending || this.subscribed)
        return;
      Tracing.Log(CorrespondentTradeEditor.sw, this.className, TraceLevel.Info, "Subscribe onTradeLoanUpdateNotification for Correspondent Trade");
      TradeLoanUpdateNotificationClientListener.TradeLoanUpdateNotificationActivity += new TradeLoanUpdateNotificationEventHandler(this.onTradeLoanUpdateNotification);
      this.subscribed = true;
    }

    private void unSubscribeEventHandler()
    {
      if (!this.subscribed)
        return;
      Tracing.Log(CorrespondentTradeEditor.sw, this.className, TraceLevel.Info, "UnSubscribe onTradeLoanUpdateNotification for Correspondent Trade");
      TradeLoanUpdateNotificationClientListener.TradeLoanUpdateNotificationActivity -= new TradeLoanUpdateNotificationEventHandler(this.onTradeLoanUpdateNotification);
      this.subscribed = false;
    }

    private void onTradeLoanUpdateNotification(object sender, TradeLoanUpdateArgs eventArgs)
    {
      Tracing.Log(CorrespondentTradeEditor.sw, this.className, TraceLevel.Info, "In onTradeLoanUpdateNotification for " + this.className);
      if (TradeManagementConsole.Instance.CurrentScreen.ToString() != this.className || !string.IsNullOrEmpty(eventArgs.TradeId) && int.Parse(eventArgs.TradeId) != this.trade.TradeID)
      {
        Tracing.Log(CorrespondentTradeEditor.sw, this.className, TraceLevel.Info, string.Format("Do not need to refresh correspondent trade. Current TradeId: {0}, Received TradeId: {1}, CorrelationId: {2}, Date: {3}", (object) this.trade.TradeID, (object) eventArgs.TradeId, (object) eventArgs.CorrelationId, (object) eventArgs.Timestamp.ToString()));
      }
      else
      {
        if (!(eventArgs.TradeStatus != TradeStatus.Pending.ToString()))
          return;
        CorrespondentTradeInfo newTrade = Session.CorrespondentTradeManager.GetTrade(this.trade.TradeID);
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
      GVColumn gvColumn5 = new GVColumn();
      GVColumn gvColumn6 = new GVColumn();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (CorrespondentTradeEditor));
      this.toolTips = new ToolTip(this.components);
      this.btnExportHistory = new StandardIconButton();
      this.btnList = new StandardIconButton();
      this.btnSave = new StandardIconButton();
      this.btnAddNotes = new StandardIconButton();
      this.btnDelete = new StandardIconButton();
      this.btnEdit = new StandardIconButton();
      this.grpEditor = new BorderPanel();
      this.tabTrade = new TabControl();
      this.tpDetails = new TabPage();
      this.pnlDetails = new System.Windows.Forms.Panel();
      this.pnlRight = new System.Windows.Forms.Panel();
      this.pnlFilter = new System.Windows.Forms.Panel();
      this.vsAdvSearch = new VerticalSeparator();
      this.btnSavedSearchesSimple = new System.Windows.Forms.Button();
      this.btnSavedSearchesAdv = new System.Windows.Forms.Button();
      this.cboSearchType = new ComboBox();
      this.ctlSimpleSearch = new SelectEPPSLoanProgramControl();
      this.ctlAdvancedSearch = new AdvancedSearchControl();
      this.collapsibleSplitter2 = new CollapsibleSplitter();
      this.pnlRightBottom = new System.Windows.Forms.Panel();
      this.pnlPairOff = new System.Windows.Forms.Panel();
      this.collapsibleSplitter1 = new CollapsibleSplitter();
      this.pnlLeft = new System.Windows.Forms.Panel();
      this.panel1 = new System.Windows.Forms.Panel();
      this.groupContainer1 = new GroupContainer();
      this.panel2 = new System.Windows.Forms.Panel();
      this.dtExpirationDate = new DatePicker();
      this.lbtnTolerance = new FieldLockButton();
      this.txtWABP = new System.Windows.Forms.TextBox();
      this.lbtnWABP = new FieldLockButton();
      this.label19 = new System.Windows.Forms.Label();
      this.label18 = new System.Windows.Forms.Label();
      this.cmbTradeDesc = new ComboBox();
      this.lbtnCommitmentNumber = new FieldLockButton();
      this.lblDeliveryDays = new System.Windows.Forms.Label();
      this.txtGainLossAmount = new System.Windows.Forms.TextBox();
      this.label17 = new System.Windows.Forms.Label();
      this.lblAuthorizedTrader = new System.Windows.Forms.Label();
      this.cmbAuthorizedTrader = new ComboBox();
      this.label5 = new System.Windows.Forms.Label();
      this.txtOrgId = new System.Windows.Forms.TextBox();
      this.label6 = new System.Windows.Forms.Label();
      this.txtTPOID = new System.Windows.Forms.TextBox();
      this.label7 = new System.Windows.Forms.Label();
      this.txtName = new System.Windows.Forms.TextBox();
      this.cmbDeliveryType = new ComboBox();
      this.label14 = new System.Windows.Forms.Label();
      this.txtCommitmentType = new System.Windows.Forms.TextBox();
      this.txtPairOffAmt = new System.Windows.Forms.TextBox();
      this.label21 = new System.Windows.Forms.Label();
      this.label16 = new System.Windows.Forms.Label();
      this.label15 = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.txtContract = new System.Windows.Forms.TextBox();
      this.label24 = new System.Windows.Forms.Label();
      this.txtCommitmentNumber = new System.Windows.Forms.TextBox();
      this.label8 = new System.Windows.Forms.Label();
      this.label13 = new System.Windows.Forms.Label();
      this.txtMaxAmt = new System.Windows.Forms.TextBox();
      this.dtDeliveryExpirationDate = new DatePicker();
      this.cboMasters = new ComboBox();
      this.dtCommitment = new DatePicker();
      this.label20 = new System.Windows.Forms.Label();
      this.txtMinAmt = new System.Windows.Forms.TextBox();
      this.label12 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.txtAmount = new System.Windows.Forms.TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this.label10 = new System.Windows.Forms.Label();
      this.txtTolerance = new System.Windows.Forms.TextBox();
      this.collapsibleSplitter3 = new CollapsibleSplitter();
      this.pnlAotInfo = new System.Windows.Forms.Panel();
      this.grpAOTInformation = new GroupContainer();
      this.panel4 = new System.Windows.Forms.Panel();
      this.tabControl1 = new TabControl();
      this.tabPage1 = new TabPage();
      this.panel15 = new System.Windows.Forms.Panel();
      this.dtTrPartyExecutionDate = new DatePicker();
      this.label9 = new System.Windows.Forms.Label();
      this.label50 = new System.Windows.Forms.Label();
      this.dtSettlementDate = new DatePicker();
      this.txtSecurityPrice = new System.Windows.Forms.TextBox();
      this.label49 = new System.Windows.Forms.Label();
      this.txtSecurityCoupon = new System.Windows.Forms.TextBox();
      this.label48 = new System.Windows.Forms.Label();
      this.cboSecurityTerm = new ComboBox();
      this.label47 = new System.Windows.Forms.Label();
      this.cboSecurityType = new ComboBox();
      this.label46 = new System.Windows.Forms.Label();
      this.btnSelector = new StandardIconButton();
      this.txtOriginalTradeDealer = new System.Windows.Forms.TextBox();
      this.label45 = new System.Windows.Forms.Label();
      this.label44 = new System.Windows.Forms.Label();
      this.dtOriginalTradeDate = new DatePicker();
      this.panel3 = new System.Windows.Forms.Panel();
      this.cboxAgencyDeliveryType = new ComboBox();
      this.cboxAgencyName = new ComboBox();
      this.cboxRepWarrantType = new ComboBox();
      this.cboxFundType = new ComboBox();
      this.label2 = new System.Windows.Forms.Label();
      this.label11 = new System.Windows.Forms.Label();
      this.label22 = new System.Windows.Forms.Label();
      this.label23 = new System.Windows.Forms.Label();
      this.btnDocCustodian = new StandardIconButton();
      this.txtDocCustodian = new System.Windows.Forms.TextBox();
      this.label25 = new System.Windows.Forms.Label();
      this.label26 = new System.Windows.Forms.Label();
      this.tpPricing = new TabPage();
      this.pnlPricing = new System.Windows.Forms.Panel();
      this.btnSRPTemplate = new System.Windows.Forms.Button();
      this.btnAdjustmentTemplate = new System.Windows.Forms.Button();
      this.tableLayoutPanel1 = new TableLayoutPanel();
      this.ctlSRP = new SRPTableEditor();
      this.tableLayoutPanel2 = new TableLayoutPanel();
      this.ctlAdjustments = new PriceAdjustmentListEditor();
      this.ctlPricing = new TradePricingEditor();
      this.tpLoans = new TabPage();
      this.tpHistory = new TabPage();
      this.pnlHistory = new System.Windows.Forms.Panel();
      this.grpHistory = new GroupContainer();
      this.gvHistory = new EllieMae.EMLite.UI.GridView();
      this.grpNotes = new GroupContainer();
      this.gvNotes = new EllieMae.EMLite.UI.GridView();
      this.grpHistoryDetails = new GroupContainer();
      this.txtHistoryDetails = new RichTextBox();
      this.gradientPanel1 = new GradientPanel();
      this.flowLayoutPanel2 = new FlowLayoutPanel();
      this.lblTradeName = new System.Windows.Forms.Label();
      this.pictPending = new PictureBox();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.datePicker1 = new DatePicker();
      ((ISupportInitialize) this.btnExportHistory).BeginInit();
      ((ISupportInitialize) this.btnList).BeginInit();
      ((ISupportInitialize) this.btnSave).BeginInit();
      ((ISupportInitialize) this.btnAddNotes).BeginInit();
      ((ISupportInitialize) this.btnDelete).BeginInit();
      ((ISupportInitialize) this.btnEdit).BeginInit();
      this.grpEditor.SuspendLayout();
      this.tabTrade.SuspendLayout();
      this.tpDetails.SuspendLayout();
      this.pnlDetails.SuspendLayout();
      this.pnlRight.SuspendLayout();
      this.pnlFilter.SuspendLayout();
      this.pnlRightBottom.SuspendLayout();
      this.pnlLeft.SuspendLayout();
      this.panel1.SuspendLayout();
      this.groupContainer1.SuspendLayout();
      this.panel2.SuspendLayout();
      this.pnlAotInfo.SuspendLayout();
      this.grpAOTInformation.SuspendLayout();
      this.panel4.SuspendLayout();
      this.tabControl1.SuspendLayout();
      this.tabPage1.SuspendLayout();
      this.panel15.SuspendLayout();
      ((ISupportInitialize) this.btnSelector).BeginInit();
      this.panel3.SuspendLayout();
      ((ISupportInitialize) this.btnDocCustodian).BeginInit();
      this.tpPricing.SuspendLayout();
      this.pnlPricing.SuspendLayout();
      this.tableLayoutPanel1.SuspendLayout();
      this.tableLayoutPanel2.SuspendLayout();
      this.tpHistory.SuspendLayout();
      this.pnlHistory.SuspendLayout();
      this.grpHistory.SuspendLayout();
      this.grpNotes.SuspendLayout();
      this.grpHistoryDetails.SuspendLayout();
      this.gradientPanel1.SuspendLayout();
      this.flowLayoutPanel2.SuspendLayout();
      ((ISupportInitialize) this.pictPending).BeginInit();
      this.flowLayoutPanel1.SuspendLayout();
      this.SuspendLayout();
      this.btnExportHistory.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnExportHistory.BackColor = Color.Transparent;
      this.btnExportHistory.Location = new Point(400, 5);
      this.btnExportHistory.MouseDownImage = (System.Drawing.Image) null;
      this.btnExportHistory.Name = "btnExportHistory";
      this.btnExportHistory.Size = new Size(16, 16);
      this.btnExportHistory.StandardButtonType = StandardIconButton.ButtonType.ExcelButton;
      this.btnExportHistory.TabIndex = 3;
      this.btnExportHistory.TabStop = false;
      this.toolTips.SetToolTip((Control) this.btnExportHistory, "Export to Excel");
      this.btnExportHistory.Click += new EventHandler(this.btnExportHistory_Click);
      this.btnList.BackColor = Color.Transparent;
      this.btnList.Location = new Point(786, 3);
      this.btnList.Margin = new Padding(2, 3, 0, 3);
      this.btnList.MouseDownImage = (System.Drawing.Image) null;
      this.btnList.Name = "btnList";
      this.btnList.Size = new Size(16, 16);
      this.btnList.StandardButtonType = StandardIconButton.ButtonType.CloseButton;
      this.btnList.TabIndex = 7;
      this.btnList.TabStop = false;
      this.toolTips.SetToolTip((Control) this.btnList, "Exit Correspondent Trade");
      this.btnList.Click += new EventHandler(this.btnList_Click);
      this.btnSave.BackColor = Color.Transparent;
      this.btnSave.Location = new Point(765, 3);
      this.btnSave.MouseDownImage = (System.Drawing.Image) null;
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(16, 16);
      this.btnSave.StandardButtonType = StandardIconButton.ButtonType.SaveButton;
      this.btnSave.TabIndex = 6;
      this.btnSave.TabStop = false;
      this.toolTips.SetToolTip((Control) this.btnSave, "Save Correspondent Trade");
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.btnAddNotes.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAddNotes.BackColor = Color.Transparent;
      this.btnAddNotes.Location = new Point(386, 5);
      this.btnAddNotes.MouseDownImage = (System.Drawing.Image) null;
      this.btnAddNotes.Name = "btnAddNotes";
      this.btnAddNotes.Size = new Size(16, 16);
      this.btnAddNotes.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAddNotes.TabIndex = 13;
      this.btnAddNotes.TabStop = false;
      this.toolTips.SetToolTip((Control) this.btnAddNotes, "New Note");
      this.btnAddNotes.Click += new EventHandler(this.btnAddNotes_Click);
      this.btnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDelete.BackColor = Color.Transparent;
      this.btnDelete.Enabled = false;
      this.btnDelete.Location = new Point(430, 4);
      this.btnDelete.Margin = new Padding(3, 3, 2, 3);
      this.btnDelete.MouseDownImage = (System.Drawing.Image) null;
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new Size(16, 16);
      this.btnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDelete.TabIndex = 12;
      this.btnDelete.TabStop = false;
      this.toolTips.SetToolTip((Control) this.btnDelete, "Delete Note");
      this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
      this.btnEdit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnEdit.BackColor = Color.Transparent;
      this.btnEdit.Enabled = false;
      this.btnEdit.Location = new Point(408, 4);
      this.btnEdit.MouseDownImage = (System.Drawing.Image) null;
      this.btnEdit.Name = "btnEdit";
      this.btnEdit.Size = new Size(16, 16);
      this.btnEdit.StandardButtonType = StandardIconButton.ButtonType.ManageButton;
      this.btnEdit.TabIndex = 11;
      this.btnEdit.TabStop = false;
      this.toolTips.SetToolTip((Control) this.btnEdit, "Edit Note");
      this.btnEdit.Click += new EventHandler(this.btnEdit_Click);
      this.grpEditor.BackColor = Color.Transparent;
      this.grpEditor.Borders = AnchorStyles.None;
      this.grpEditor.Controls.Add((Control) this.tabTrade);
      this.grpEditor.Dock = DockStyle.Fill;
      this.grpEditor.Location = new Point(0, 31);
      this.grpEditor.Name = "grpEditor";
      this.grpEditor.Padding = new Padding(2, 2, 0, 0);
      this.grpEditor.Size = new Size(1244, 771);
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
      this.tabTrade.Size = new Size(1242, 769);
      this.tabTrade.TabIndex = 1;
      this.tabTrade.SelectedIndexChanged += new EventHandler(this.tabTrade_SelectedIndexChanged);
      this.tpDetails.Controls.Add((Control) this.pnlDetails);
      this.tpDetails.Location = new Point(4, 24);
      this.tpDetails.Name = "tpDetails";
      this.tpDetails.Padding = new Padding(0, 2, 2, 2);
      this.tpDetails.Size = new Size(1234, 741);
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
      this.pnlDetails.Size = new Size(1232, 737);
      this.pnlDetails.TabIndex = 6;
      this.pnlRight.Controls.Add((Control) this.pnlFilter);
      this.pnlRight.Controls.Add((Control) this.collapsibleSplitter2);
      this.pnlRight.Controls.Add((Control) this.pnlRightBottom);
      this.pnlRight.Dock = DockStyle.Fill;
      this.pnlRight.Location = new Point(404, 0);
      this.pnlRight.Name = "pnlRight";
      this.pnlRight.Size = new Size(828, 737);
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
      this.pnlFilter.Size = new Size(828, 405);
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
      this.btnSavedSearchesSimple.TabIndex = 140;
      this.btnSavedSearchesSimple.Text = "&Saved Searches";
      this.btnSavedSearchesSimple.UseVisualStyleBackColor = true;
      this.btnSavedSearchesSimple.Click += new EventHandler(this.btnSavedSearches_Click);
      this.btnSavedSearchesAdv.BackColor = SystemColors.Control;
      this.btnSavedSearchesAdv.Location = new Point(400, 4);
      this.btnSavedSearchesAdv.Margin = new Padding(0);
      this.btnSavedSearchesAdv.Name = "btnSavedSearchesAdv";
      this.btnSavedSearchesAdv.Padding = new Padding(2, 0, 0, 0);
      this.btnSavedSearchesAdv.Size = new Size(102, 22);
      this.btnSavedSearchesAdv.TabIndex = 130;
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
      this.cboSearchType.TabIndex = 120;
      this.cboSearchType.SelectedIndexChanged += new EventHandler(this.cboSearchType_SelectedIndexChanged);
      this.ctlSimpleSearch.BackColor = SystemColors.Window;
      this.ctlSimpleSearch.DataModified = false;
      this.ctlSimpleSearch.Dock = DockStyle.Fill;
      this.ctlSimpleSearch.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.ctlSimpleSearch.Location = new Point(0, 0);
      this.ctlSimpleSearch.Name = "ctlSimpleSearch";
      this.ctlSimpleSearch.NoteRateMax = "";
      this.ctlSimpleSearch.NoteRateMin = "";
      this.ctlSimpleSearch.ReadOnly = false;
      this.ctlSimpleSearch.Size = new Size(828, 405);
      this.ctlSimpleSearch.TabIndex = 2;
      this.ctlAdvancedSearch.AllowDynamicOperators = false;
      this.ctlAdvancedSearch.DDMSetting = false;
      this.ctlAdvancedSearch.Dock = DockStyle.Fill;
      this.ctlAdvancedSearch.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.ctlAdvancedSearch.Location = new Point(0, 0);
      this.ctlAdvancedSearch.Name = "ctlAdvancedSearch";
      this.ctlAdvancedSearch.Size = new Size(828, 405);
      this.ctlAdvancedSearch.TabIndex = 3;
      this.ctlAdvancedSearch.Title = "Filters";
      this.collapsibleSplitter2.AnimationDelay = 20;
      this.collapsibleSplitter2.AnimationStep = 20;
      this.collapsibleSplitter2.BorderStyle3D = Border3DStyle.Flat;
      this.collapsibleSplitter2.ControlToHide = (Control) this.pnlRightBottom;
      this.collapsibleSplitter2.Dock = DockStyle.Bottom;
      this.collapsibleSplitter2.ExpandParentForm = false;
      this.collapsibleSplitter2.Location = new Point(0, 405);
      this.collapsibleSplitter2.Name = "collapsibleSplitter2";
      this.collapsibleSplitter2.TabIndex = 1;
      this.collapsibleSplitter2.TabStop = false;
      this.collapsibleSplitter2.UseAnimations = false;
      this.collapsibleSplitter2.VisualStyle = VisualStyles.Encompass;
      this.pnlRightBottom.AutoScroll = true;
      this.pnlRightBottom.Controls.Add((Control) this.pnlPairOff);
      this.pnlRightBottom.Dock = DockStyle.Bottom;
      this.pnlRightBottom.Location = new Point(0, 412);
      this.pnlRightBottom.Name = "pnlRightBottom";
      this.pnlRightBottom.Size = new Size(828, 325);
      this.pnlRightBottom.TabIndex = 100;
      this.pnlPairOff.Dock = DockStyle.Fill;
      this.pnlPairOff.Location = new Point(0, 0);
      this.pnlPairOff.Name = "pnlPairOff";
      this.pnlPairOff.Padding = new Padding(0, 0, 0, 4);
      this.pnlPairOff.Size = new Size(828, 325);
      this.pnlPairOff.TabIndex = 3;
      this.collapsibleSplitter1.AnimationDelay = 20;
      this.collapsibleSplitter1.AnimationStep = 20;
      this.collapsibleSplitter1.BorderStyle3D = Border3DStyle.Flat;
      this.collapsibleSplitter1.ControlToHide = (Control) this.pnlLeft;
      this.collapsibleSplitter1.ExpandParentForm = false;
      this.collapsibleSplitter1.Location = new Point(397, 0);
      this.collapsibleSplitter1.Name = "collapsibleSplitter1";
      this.collapsibleSplitter1.TabIndex = 7;
      this.collapsibleSplitter1.TabStop = false;
      this.collapsibleSplitter1.UseAnimations = false;
      this.collapsibleSplitter1.VisualStyle = VisualStyles.Encompass;
      this.pnlLeft.Controls.Add((Control) this.panel1);
      this.pnlLeft.Controls.Add((Control) this.collapsibleSplitter3);
      this.pnlLeft.Controls.Add((Control) this.pnlAotInfo);
      this.pnlLeft.Dock = DockStyle.Left;
      this.pnlLeft.Location = new Point(0, 0);
      this.pnlLeft.Name = "pnlLeft";
      this.pnlLeft.Size = new Size(397, 737);
      this.pnlLeft.TabIndex = 6;
      this.panel1.Controls.Add((Control) this.groupContainer1);
      this.panel1.Dock = DockStyle.Fill;
      this.panel1.Location = new Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(397, 405);
      this.panel1.TabIndex = 2;
      this.groupContainer1.Controls.Add((Control) this.panel2);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(397, 405);
      this.groupContainer1.TabIndex = 104;
      this.groupContainer1.Text = "Correspondent Trade";
      this.panel2.AutoScroll = true;
      this.panel2.Controls.Add((Control) this.dtExpirationDate);
      this.panel2.Controls.Add((Control) this.lbtnTolerance);
      this.panel2.Controls.Add((Control) this.txtWABP);
      this.panel2.Controls.Add((Control) this.lbtnWABP);
      this.panel2.Controls.Add((Control) this.label19);
      this.panel2.Controls.Add((Control) this.label18);
      this.panel2.Controls.Add((Control) this.cmbTradeDesc);
      this.panel2.Controls.Add((Control) this.lbtnCommitmentNumber);
      this.panel2.Controls.Add((Control) this.lblDeliveryDays);
      this.panel2.Controls.Add((Control) this.txtGainLossAmount);
      this.panel2.Controls.Add((Control) this.label17);
      this.panel2.Controls.Add((Control) this.lblAuthorizedTrader);
      this.panel2.Controls.Add((Control) this.cmbAuthorizedTrader);
      this.panel2.Controls.Add((Control) this.label5);
      this.panel2.Controls.Add((Control) this.txtOrgId);
      this.panel2.Controls.Add((Control) this.label6);
      this.panel2.Controls.Add((Control) this.txtTPOID);
      this.panel2.Controls.Add((Control) this.label7);
      this.panel2.Controls.Add((Control) this.txtName);
      this.panel2.Controls.Add((Control) this.cmbDeliveryType);
      this.panel2.Controls.Add((Control) this.label14);
      this.panel2.Controls.Add((Control) this.txtCommitmentType);
      this.panel2.Controls.Add((Control) this.txtPairOffAmt);
      this.panel2.Controls.Add((Control) this.label21);
      this.panel2.Controls.Add((Control) this.label16);
      this.panel2.Controls.Add((Control) this.label15);
      this.panel2.Controls.Add((Control) this.label1);
      this.panel2.Controls.Add((Control) this.txtContract);
      this.panel2.Controls.Add((Control) this.label24);
      this.panel2.Controls.Add((Control) this.txtCommitmentNumber);
      this.panel2.Controls.Add((Control) this.label8);
      this.panel2.Controls.Add((Control) this.label13);
      this.panel2.Controls.Add((Control) this.txtMaxAmt);
      this.panel2.Controls.Add((Control) this.dtDeliveryExpirationDate);
      this.panel2.Controls.Add((Control) this.cboMasters);
      this.panel2.Controls.Add((Control) this.dtCommitment);
      this.panel2.Controls.Add((Control) this.label20);
      this.panel2.Controls.Add((Control) this.txtMinAmt);
      this.panel2.Controls.Add((Control) this.label12);
      this.panel2.Controls.Add((Control) this.label4);
      this.panel2.Controls.Add((Control) this.txtAmount);
      this.panel2.Controls.Add((Control) this.label3);
      this.panel2.Controls.Add((Control) this.label10);
      this.panel2.Controls.Add((Control) this.txtTolerance);
      this.panel2.Dock = DockStyle.Fill;
      this.panel2.Location = new Point(1, 26);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(395, 378);
      this.panel2.TabIndex = 23;
      this.dtExpirationDate.BackColor = SystemColors.Window;
      this.dtExpirationDate.Location = new Point(191, 327);
      this.dtExpirationDate.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.dtExpirationDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dtExpirationDate.Name = "dtExpirationDate";
      this.dtExpirationDate.Size = new Size(104, 22);
      this.dtExpirationDate.TabIndex = 229;
      this.dtExpirationDate.ToolTip = "";
      this.dtExpirationDate.Value = new DateTime(0L);
      this.dtExpirationDate.ValueChanged += new EventHandler(this.onFieldValueChanged);
      this.lbtnTolerance.Location = new Point(169, 242);
      this.lbtnTolerance.LockedStateToolTip = "Use Default Value";
      this.lbtnTolerance.MaximumSize = new Size(16, 17);
      this.lbtnTolerance.MinimumSize = new Size(16, 17);
      this.lbtnTolerance.Name = "lbtnTolerance";
      this.lbtnTolerance.Size = new Size(16, 17);
      this.lbtnTolerance.TabIndex = 228;
      this.lbtnTolerance.UnlockedStateToolTip = "Enter Data Manually";
      this.lbtnTolerance.Click += new EventHandler(this.lbtnTolerance_Click);
      this.txtWABP.Location = new Point(191, 305);
      this.txtWABP.MaxLength = 18;
      this.txtWABP.Name = "txtWABP";
      this.txtWABP.Size = new Size(139, 20);
      this.txtWABP.TabIndex = 66;
      this.txtWABP.TextAlign = HorizontalAlignment.Right;
      this.txtWABP.TextChanged += new EventHandler(this.txtWABP_TextChanged);
      this.lbtnWABP.Location = new Point(169, 306);
      this.lbtnWABP.LockedStateToolTip = "Use Default Value";
      this.lbtnWABP.MaximumSize = new Size(16, 17);
      this.lbtnWABP.MinimumSize = new Size(16, 17);
      this.lbtnWABP.Name = "lbtnWABP";
      this.lbtnWABP.Size = new Size(16, 17);
      this.lbtnWABP.TabIndex = 227;
      this.lbtnWABP.UnlockedStateToolTip = "Enter Data Manually";
      this.lbtnWABP.Click += new EventHandler(this.lbtnWABP_Click);
      this.label19.AutoSize = true;
      this.label19.Location = new Point(9, 311);
      this.label19.Name = "label19";
      this.label19.Size = new Size(146, 14);
      this.label19.TabIndex = 65;
      this.label19.Text = "Weighted Average Bulk Price";
      this.label18.AutoSize = true;
      this.label18.Location = new Point(9, 60);
      this.label18.Name = "label18";
      this.label18.Size = new Size(92, 14);
      this.label18.TabIndex = 10;
      this.label18.Text = "Trade Description";
      this.cmbTradeDesc.FormattingEnabled = true;
      this.cmbTradeDesc.Location = new Point(191, 56);
      this.cmbTradeDesc.Name = "cmbTradeDesc";
      this.cmbTradeDesc.Size = new Size(139, 22);
      this.cmbTradeDesc.TabIndex = 14;
      this.cmbTradeDesc.TextChanged += new EventHandler(this.cmbTradeDesc_TextChanged);
      this.lbtnCommitmentNumber.Location = new Point(169, 11);
      this.lbtnCommitmentNumber.LockedStateToolTip = "Use Default Value";
      this.lbtnCommitmentNumber.MaximumSize = new Size(16, 17);
      this.lbtnCommitmentNumber.MinimumSize = new Size(16, 17);
      this.lbtnCommitmentNumber.Name = "lbtnCommitmentNumber";
      this.lbtnCommitmentNumber.Size = new Size(16, 17);
      this.lbtnCommitmentNumber.TabIndex = 226;
      this.lbtnCommitmentNumber.UnlockedStateToolTip = "Enter Data Manually";
      this.lbtnCommitmentNumber.Click += new EventHandler(this.lbtnCommitmentNumber_Click);
      this.lblDeliveryDays.AutoSize = true;
      this.lblDeliveryDays.Location = new Point(301, 335);
      this.lblDeliveryDays.Name = "lblDeliveryDays";
      this.lblDeliveryDays.Size = new Size(0, 14);
      this.lblDeliveryDays.TabIndex = 59;
      this.lblDeliveryDays.Visible = false;
      this.txtGainLossAmount.Location = new Point(191, 397);
      this.txtGainLossAmount.Name = "txtGainLossAmount";
      this.txtGainLossAmount.ReadOnly = true;
      this.txtGainLossAmount.Size = new Size(131, 20);
      this.txtGainLossAmount.TabIndex = 84;
      this.txtGainLossAmount.TextAlign = HorizontalAlignment.Right;
      this.label17.AutoSize = true;
      this.label17.Location = new Point(9, 405);
      this.label17.Name = "label17";
      this.label17.Size = new Size(120, 14);
      this.label17.TabIndex = 80;
      this.label17.Text = "Total Gain/Loss Amount";
      this.lblAuthorizedTrader.Location = new Point(9, 425);
      this.lblAuthorizedTrader.Name = "lblAuthorizedTrader";
      this.lblAuthorizedTrader.Size = new Size(95, 22);
      this.lblAuthorizedTrader.TabIndex = 81;
      this.lblAuthorizedTrader.Text = "Authorized Trader";
      this.cmbAuthorizedTrader.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbAuthorizedTrader.FormattingEnabled = true;
      this.cmbAuthorizedTrader.Location = new Point(191, 419);
      this.cmbAuthorizedTrader.Name = "cmbAuthorizedTrader";
      this.cmbAuthorizedTrader.Size = new Size(131, 22);
      this.cmbAuthorizedTrader.TabIndex = 84;
      this.cmbAuthorizedTrader.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.label5.AutoSize = true;
      this.label5.Location = new Point(9, 197);
      this.label5.Name = "label5";
      this.label5.Size = new Size(80, 14);
      this.label5.TabIndex = 40;
      this.label5.Text = "Organization ID";
      this.txtOrgId.BackColor = SystemColors.Control;
      this.txtOrgId.Enabled = false;
      this.txtOrgId.Location = new Point(191, 195);
      this.txtOrgId.MaxLength = 64;
      this.txtOrgId.Name = "txtOrgId";
      this.txtOrgId.Size = new Size(138, 20);
      this.txtOrgId.TabIndex = 44;
      this.label6.AutoSize = true;
      this.label6.Location = new Point(9, 175);
      this.label6.Name = "label6";
      this.label6.Size = new Size(39, 14);
      this.label6.TabIndex = 35;
      this.label6.Text = "TPO ID";
      this.txtTPOID.BackColor = SystemColors.Control;
      this.txtTPOID.Enabled = false;
      this.txtTPOID.Location = new Point(191, 173);
      this.txtTPOID.MaxLength = 64;
      this.txtTPOID.Name = "txtTPOID";
      this.txtTPOID.Size = new Size(138, 20);
      this.txtTPOID.TabIndex = 39;
      this.label7.AutoSize = true;
      this.label7.Location = new Point(9, 153);
      this.label7.Name = "label7";
      this.label7.Size = new Size(82, 14);
      this.label7.TabIndex = 30;
      this.label7.Text = "Company Name";
      this.txtName.BackColor = SystemColors.Control;
      this.txtName.Enabled = false;
      this.txtName.Location = new Point(191, 151);
      this.txtName.MaxLength = 64;
      this.txtName.Name = "txtName";
      this.txtName.Size = new Size(138, 20);
      this.txtName.TabIndex = 34;
      this.cmbDeliveryType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbDeliveryType.FormattingEnabled = true;
      this.cmbDeliveryType.Location = new Point(191, (int) sbyte.MaxValue);
      this.cmbDeliveryType.Name = "cmbDeliveryType";
      this.cmbDeliveryType.Size = new Size(139, 22);
      this.cmbDeliveryType.TabIndex = 29;
      this.cmbDeliveryType.SelectedIndexChanged += new EventHandler(this.cmbDeliveryType_SelectedIndexChanged);
      this.label14.AutoSize = true;
      this.label14.Location = new Point(9, 130);
      this.label14.Name = "label14";
      this.label14.Size = new Size(72, 14);
      this.label14.TabIndex = 25;
      this.label14.Text = "Delivery Type";
      this.txtCommitmentType.Enabled = false;
      this.txtCommitmentType.Location = new Point(191, 105);
      this.txtCommitmentType.MaxLength = 64;
      this.txtCommitmentType.Name = "txtCommitmentType";
      this.txtCommitmentType.ReadOnly = true;
      this.txtCommitmentType.Size = new Size(139, 20);
      this.txtCommitmentType.TabIndex = 24;
      this.txtPairOffAmt.Location = new Point(191, 375);
      this.txtPairOffAmt.Name = "txtPairOffAmt";
      this.txtPairOffAmt.ReadOnly = true;
      this.txtPairOffAmt.Size = new Size(131, 20);
      this.txtPairOffAmt.TabIndex = 79;
      this.txtPairOffAmt.TextAlign = HorizontalAlignment.Right;
      this.label21.AutoSize = true;
      this.label21.Location = new Point(9, 383);
      this.label21.Name = "label21";
      this.label21.Size = new Size(109, 14);
      this.label21.TabIndex = 75;
      this.label21.Text = "Total Pair-Off Amount";
      this.label16.AutoSize = true;
      this.label16.Location = new Point(9, 359);
      this.label16.Name = "label16";
      this.label16.Size = new Size(121, 14);
      this.label16.TabIndex = 70;
      this.label16.Text = "Delivery Expiration Date";
      this.label15.AutoSize = true;
      this.label15.Location = new Point(9, 335);
      this.label15.Name = "label15";
      this.label15.Size = new Size(79, 14);
      this.label15.TabIndex = 68;
      this.label15.Text = "Expiration Date";
      this.label1.AutoSize = true;
      this.label1.Location = new Point(9, 11);
      this.label1.Name = "label1";
      this.label1.Size = new Size(73, 14);
      this.label1.TabIndex = 1;
      this.label1.Text = "Commitment #";
      this.txtContract.Location = new Point(191, 81);
      this.txtContract.MaxLength = 64;
      this.txtContract.Name = "txtContract";
      this.txtContract.ReadOnly = true;
      this.txtContract.Size = new Size(138, 20);
      this.txtContract.TabIndex = 19;
      this.txtContract.Visible = false;
      this.label24.AutoSize = true;
      this.label24.Location = new Point(9, 108);
      this.label24.Name = "label24";
      this.label24.Size = new Size(90, 14);
      this.label24.TabIndex = 20;
      this.label24.Text = "Commitment Type";
      this.txtCommitmentNumber.Location = new Point(191, 8);
      this.txtCommitmentNumber.MaxLength = 18;
      this.txtCommitmentNumber.Name = "txtCommitmentNumber";
      this.txtCommitmentNumber.ReadOnly = true;
      this.txtCommitmentNumber.Size = new Size(139, 20);
      this.txtCommitmentNumber.TabIndex = 4;
      this.txtCommitmentNumber.TextChanged += new EventHandler(this.onCommitmentNumberChanged);
      this.label8.AutoSize = true;
      this.label8.Location = new Point(9, 83);
      this.label8.Name = "label8";
      this.label8.Size = new Size(109, 14);
      this.label8.TabIndex = 15;
      this.label8.Text = "Master Commitment #";
      this.label13.AutoSize = true;
      this.label13.Location = new Point(9, 34);
      this.label13.Name = "label13";
      this.label13.Size = new Size(89, 14);
      this.label13.TabIndex = 5;
      this.label13.Text = "Commitment Date";
      this.label13.DoubleClick += new EventHandler(this.label13_DoubleClick);
      this.txtMaxAmt.Location = new Point(191, 283);
      this.txtMaxAmt.MaxLength = 12;
      this.txtMaxAmt.Name = "txtMaxAmt";
      this.txtMaxAmt.ReadOnly = true;
      this.txtMaxAmt.Size = new Size(139, 20);
      this.txtMaxAmt.TabIndex = 64;
      this.txtMaxAmt.TextAlign = HorizontalAlignment.Right;
      this.dtDeliveryExpirationDate.BackColor = SystemColors.Window;
      this.dtDeliveryExpirationDate.Location = new Point(191, 351);
      this.dtDeliveryExpirationDate.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.dtDeliveryExpirationDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dtDeliveryExpirationDate.Name = "dtDeliveryExpirationDate";
      this.dtDeliveryExpirationDate.Size = new Size(104, 22);
      this.dtDeliveryExpirationDate.TabIndex = 74;
      this.dtDeliveryExpirationDate.ToolTip = "";
      this.dtDeliveryExpirationDate.Value = new DateTime(0L);
      this.dtDeliveryExpirationDate.ValueChanged += new EventHandler(this.onLoanUpdatableFieldValueChanged);
      this.cboMasters.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboMasters.FormattingEnabled = true;
      this.cboMasters.Location = new Point(191, 80);
      this.cboMasters.Name = "cboMasters";
      this.cboMasters.Size = new Size(139, 22);
      this.cboMasters.TabIndex = 19;
      this.cboMasters.SelectedIndexChanged += new EventHandler(this.cboMasters_SelectedIndexChanged);
      this.dtCommitment.BackColor = SystemColors.Window;
      this.dtCommitment.Location = new Point(191, 30);
      this.dtCommitment.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.dtCommitment.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dtCommitment.Name = "dtCommitment";
      this.dtCommitment.Size = new Size(104, 22);
      this.dtCommitment.TabIndex = 9;
      this.dtCommitment.ToolTip = "";
      this.dtCommitment.Value = new DateTime(0L);
      this.dtCommitment.ValueChanged += new EventHandler(this.onCommitmentDateChanged);
      this.label20.AutoSize = true;
      this.label20.Location = new Point(9, 287);
      this.label20.Name = "label20";
      this.label20.Size = new Size(90, 14);
      this.label20.TabIndex = 60;
      this.label20.Text = "Maximum Amount";
      this.txtMinAmt.Location = new Point(191, 260);
      this.txtMinAmt.MaxLength = 12;
      this.txtMinAmt.Name = "txtMinAmt";
      this.txtMinAmt.ReadOnly = true;
      this.txtMinAmt.Size = new Size(139, 20);
      this.txtMinAmt.TabIndex = 59;
      this.txtMinAmt.TextAlign = HorizontalAlignment.Right;
      this.label12.AutoSize = true;
      this.label12.Location = new Point(9, 264);
      this.label12.Name = "label12";
      this.label12.Size = new Size(86, 14);
      this.label12.TabIndex = 55;
      this.label12.Text = "Minimum Amount";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(9, 220);
      this.label4.Name = "label4";
      this.label4.Size = new Size(74, 14);
      this.label4.TabIndex = 45;
      this.label4.Text = "Trade Amount";
      this.txtAmount.Location = new Point(191, 217);
      this.txtAmount.MaxLength = 12;
      this.txtAmount.Name = "txtAmount";
      this.txtAmount.Size = new Size(139, 20);
      this.txtAmount.TabIndex = 49;
      this.txtAmount.TextAlign = HorizontalAlignment.Right;
      this.txtAmount.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.label3.AutoSize = true;
      this.label3.Location = new Point(9, 242);
      this.label3.Name = "label3";
      this.label3.Size = new Size(54, 14);
      this.label3.TabIndex = 50;
      this.label3.Text = "Tolerance";
      this.label10.AutoSize = true;
      this.label10.Location = new Point(305, 241);
      this.label10.Name = "label10";
      this.label10.Size = new Size(17, 14);
      this.label10.TabIndex = 10;
      this.label10.Text = "%";
      this.txtTolerance.Location = new Point(191, 238);
      this.txtTolerance.MaxLength = 13;
      this.txtTolerance.Name = "txtTolerance";
      this.txtTolerance.ShortcutsEnabled = false;
      this.txtTolerance.Size = new Size(104, 20);
      this.txtTolerance.TabIndex = 54;
      this.txtTolerance.TextAlign = HorizontalAlignment.Right;
      this.txtTolerance.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.txtTolerance.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.txtTolerance.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.collapsibleSplitter3.AnimationDelay = 20;
      this.collapsibleSplitter3.AnimationStep = 20;
      this.collapsibleSplitter3.BorderStyle3D = Border3DStyle.Flat;
      this.collapsibleSplitter3.ControlToHide = (Control) this.pnlAotInfo;
      this.collapsibleSplitter3.Dock = DockStyle.Bottom;
      this.collapsibleSplitter3.ExpandParentForm = false;
      this.collapsibleSplitter3.Location = new Point(0, 405);
      this.collapsibleSplitter3.Name = "collapsibleSplitter3";
      this.collapsibleSplitter3.TabIndex = 1;
      this.collapsibleSplitter3.TabStop = false;
      this.collapsibleSplitter3.UseAnimations = false;
      this.collapsibleSplitter3.VisualStyle = VisualStyles.Encompass;
      this.pnlAotInfo.Controls.Add((Control) this.grpAOTInformation);
      this.pnlAotInfo.Dock = DockStyle.Bottom;
      this.pnlAotInfo.Location = new Point(0, 412);
      this.pnlAotInfo.Name = "pnlAotInfo";
      this.pnlAotInfo.Size = new Size(397, 325);
      this.pnlAotInfo.TabIndex = 90;
      this.grpAOTInformation.Controls.Add((Control) this.panel4);
      this.grpAOTInformation.Dock = DockStyle.Fill;
      this.grpAOTInformation.HeaderForeColor = SystemColors.ControlText;
      this.grpAOTInformation.Location = new Point(0, 0);
      this.grpAOTInformation.Name = "grpAOTInformation";
      this.grpAOTInformation.Size = new Size(397, 325);
      this.grpAOTInformation.TabIndex = 1;
      this.grpAOTInformation.Text = "AOT Information";
      this.panel4.AutoScroll = true;
      this.panel4.Controls.Add((Control) this.tabControl1);
      this.panel4.Dock = DockStyle.Fill;
      this.panel4.Location = new Point(1, 26);
      this.panel4.Name = "panel4";
      this.panel4.Size = new Size(395, 298);
      this.panel4.TabIndex = 23;
      this.tabControl1.Controls.Add((Control) this.tabPage1);
      this.tabControl1.Dock = DockStyle.Fill;
      this.tabControl1.ItemSize = new Size(44, 20);
      this.tabControl1.Location = new Point(0, 0);
      this.tabControl1.Name = "tabControl1";
      this.tabControl1.Padding = new Point(11, 3);
      this.tabControl1.SelectedIndex = 0;
      this.tabControl1.Size = new Size(395, 298);
      this.tabControl1.TabIndex = 2;
      this.tabPage1.Controls.Add((Control) this.panel15);
      this.tabPage1.Controls.Add((Control) this.panel3);
      this.tabPage1.Location = new Point(4, 24);
      this.tabPage1.Name = "tabPage1";
      this.tabPage1.Padding = new Padding(0, 2, 2, 2);
      this.tabPage1.Size = new Size(387, 270);
      this.tabPage1.TabIndex = 0;
      this.tabPage1.Tag = (object) "Details";
      this.tabPage1.Text = "Details";
      this.tabPage1.UseVisualStyleBackColor = true;
      this.panel15.AutoScroll = true;
      this.panel15.BackColor = Color.WhiteSmoke;
      this.panel15.Controls.Add((Control) this.dtTrPartyExecutionDate);
      this.panel15.Controls.Add((Control) this.label9);
      this.panel15.Controls.Add((Control) this.label50);
      this.panel15.Controls.Add((Control) this.dtSettlementDate);
      this.panel15.Controls.Add((Control) this.txtSecurityPrice);
      this.panel15.Controls.Add((Control) this.label49);
      this.panel15.Controls.Add((Control) this.txtSecurityCoupon);
      this.panel15.Controls.Add((Control) this.label48);
      this.panel15.Controls.Add((Control) this.cboSecurityTerm);
      this.panel15.Controls.Add((Control) this.label47);
      this.panel15.Controls.Add((Control) this.cboSecurityType);
      this.panel15.Controls.Add((Control) this.label46);
      this.panel15.Controls.Add((Control) this.btnSelector);
      this.panel15.Controls.Add((Control) this.txtOriginalTradeDealer);
      this.panel15.Controls.Add((Control) this.label45);
      this.panel15.Controls.Add((Control) this.label44);
      this.panel15.Controls.Add((Control) this.dtOriginalTradeDate);
      this.panel15.Dock = DockStyle.Fill;
      this.panel15.Location = new Point(0, 2);
      this.panel15.Name = "panel15";
      this.panel15.Size = new Size(385, 266);
      this.panel15.TabIndex = 23;
      this.dtTrPartyExecutionDate.BackColor = SystemColors.Window;
      this.dtTrPartyExecutionDate.Location = new Point(187, 179);
      this.dtTrPartyExecutionDate.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.dtTrPartyExecutionDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dtTrPartyExecutionDate.Name = "dtTrPartyExecutionDate";
      this.dtTrPartyExecutionDate.Size = new Size(104, 22);
      this.dtTrPartyExecutionDate.TabIndex = 126;
      this.dtTrPartyExecutionDate.ToolTip = "";
      this.dtTrPartyExecutionDate.Value = new DateTime(0L);
      this.dtTrPartyExecutionDate.ValueChanged += new EventHandler(this.onFieldValueChanged);
      this.label9.AutoSize = true;
      this.label9.Location = new Point(5, 182);
      this.label9.Name = "label9";
      this.label9.Size = new Size(123, 14);
      this.label9.TabIndex = 125;
      this.label9.Text = "Tri-Party Execution Date";
      this.label50.AutoSize = true;
      this.label50.Location = new Point(5, 157);
      this.label50.Name = "label50";
      this.label50.Size = new Size(82, 14);
      this.label50.TabIndex = 120;
      this.label50.Text = "Settlement Date";
      this.dtSettlementDate.BackColor = SystemColors.Window;
      this.dtSettlementDate.Location = new Point(187, 151);
      this.dtSettlementDate.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.dtSettlementDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dtSettlementDate.Name = "dtSettlementDate";
      this.dtSettlementDate.Size = new Size(104, 22);
      this.dtSettlementDate.TabIndex = 124;
      this.dtSettlementDate.ToolTip = "";
      this.dtSettlementDate.Value = new DateTime(0L);
      this.dtSettlementDate.ValueChanged += new EventHandler(this.onFieldValueChanged);
      this.txtSecurityPrice.Location = new Point(187, 129);
      this.txtSecurityPrice.MaxLength = 12;
      this.txtSecurityPrice.Name = "txtSecurityPrice";
      this.txtSecurityPrice.Size = new Size(139, 20);
      this.txtSecurityPrice.TabIndex = 119;
      this.txtSecurityPrice.TextAlign = HorizontalAlignment.Right;
      this.txtSecurityPrice.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.label49.AutoSize = true;
      this.label49.Location = new Point(5, 133);
      this.label49.Name = "label49";
      this.label49.Size = new Size(74, 14);
      this.label49.TabIndex = 115;
      this.label49.Text = "Security Price";
      this.txtSecurityCoupon.Location = new Point(187, 107);
      this.txtSecurityCoupon.MaxLength = 12;
      this.txtSecurityCoupon.Name = "txtSecurityCoupon";
      this.txtSecurityCoupon.Size = new Size(139, 20);
      this.txtSecurityCoupon.TabIndex = 114;
      this.txtSecurityCoupon.TextAlign = HorizontalAlignment.Right;
      this.txtSecurityCoupon.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.label48.AutoSize = true;
      this.label48.Location = new Point(5, 110);
      this.label48.Name = "label48";
      this.label48.Size = new Size(87, 14);
      this.label48.TabIndex = 110;
      this.label48.Text = "Security Coupon";
      this.cboSecurityTerm.FormattingEnabled = true;
      this.cboSecurityTerm.Location = new Point(187, 83);
      this.cboSecurityTerm.Name = "cboSecurityTerm";
      this.cboSecurityTerm.Size = new Size(104, 22);
      this.cboSecurityTerm.TabIndex = 109;
      this.cboSecurityTerm.SelectedIndexChanged += new EventHandler(this.onFieldValueChanged);
      this.label47.AutoSize = true;
      this.label47.Location = new Point(5, 87);
      this.label47.Name = "label47";
      this.label47.Size = new Size(73, 14);
      this.label47.TabIndex = 105;
      this.label47.Text = "Security Term";
      this.cboSecurityType.FormattingEnabled = true;
      this.cboSecurityType.Location = new Point(187, 59);
      this.cboSecurityType.Name = "cboSecurityType";
      this.cboSecurityType.Size = new Size(104, 22);
      this.cboSecurityType.TabIndex = 104;
      this.cboSecurityType.SelectedIndexChanged += new EventHandler(this.onFieldValueChanged);
      this.label46.AutoSize = true;
      this.label46.Location = new Point(5, 64);
      this.label46.Name = "label46";
      this.label46.Size = new Size(73, 14);
      this.label46.TabIndex = 100;
      this.label46.Text = "Security Type";
      this.btnSelector.BackColor = Color.Transparent;
      this.btnSelector.Location = new Point(332, 38);
      this.btnSelector.MouseDownImage = (System.Drawing.Image) null;
      this.btnSelector.Name = "btnSelector";
      this.btnSelector.Size = new Size(16, 16);
      this.btnSelector.StandardButtonType = StandardIconButton.ButtonType.RolodexButton;
      this.btnSelector.TabIndex = 41;
      this.btnSelector.TabStop = false;
      this.btnSelector.Click += new EventHandler(this.btnSelector_Click);
      this.txtOriginalTradeDealer.Location = new Point(187, 37);
      this.txtOriginalTradeDealer.MaxLength = 12;
      this.txtOriginalTradeDealer.Name = "txtOriginalTradeDealer";
      this.txtOriginalTradeDealer.Size = new Size(139, 20);
      this.txtOriginalTradeDealer.TabIndex = 99;
      this.txtOriginalTradeDealer.TextAlign = HorizontalAlignment.Right;
      this.txtOriginalTradeDealer.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.label45.AutoSize = true;
      this.label45.Location = new Point(5, 41);
      this.label45.Name = "label45";
      this.label45.Size = new Size(108, 14);
      this.label45.TabIndex = 95;
      this.label45.Text = "Original Trade Dealer";
      this.label44.AutoSize = true;
      this.label44.Location = new Point(5, 18);
      this.label44.Name = "label44";
      this.label44.Size = new Size(99, 14);
      this.label44.TabIndex = 90;
      this.label44.Text = "Original Trade Date";
      this.dtOriginalTradeDate.BackColor = SystemColors.Window;
      this.dtOriginalTradeDate.Location = new Point(187, 12);
      this.dtOriginalTradeDate.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.dtOriginalTradeDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dtOriginalTradeDate.Name = "dtOriginalTradeDate";
      this.dtOriginalTradeDate.Size = new Size(104, 22);
      this.dtOriginalTradeDate.TabIndex = 94;
      this.dtOriginalTradeDate.ToolTip = "";
      this.dtOriginalTradeDate.Value = new DateTime(0L);
      this.dtOriginalTradeDate.ValueChanged += new EventHandler(this.onFieldValueChanged);
      this.panel3.AutoScroll = true;
      this.panel3.BackColor = Color.WhiteSmoke;
      this.panel3.Controls.Add((Control) this.cboxAgencyDeliveryType);
      this.panel3.Controls.Add((Control) this.cboxAgencyName);
      this.panel3.Controls.Add((Control) this.cboxRepWarrantType);
      this.panel3.Controls.Add((Control) this.cboxFundType);
      this.panel3.Controls.Add((Control) this.label2);
      this.panel3.Controls.Add((Control) this.label11);
      this.panel3.Controls.Add((Control) this.label22);
      this.panel3.Controls.Add((Control) this.label23);
      this.panel3.Controls.Add((Control) this.btnDocCustodian);
      this.panel3.Controls.Add((Control) this.txtDocCustodian);
      this.panel3.Controls.Add((Control) this.label25);
      this.panel3.Controls.Add((Control) this.label26);
      this.panel3.Dock = DockStyle.Fill;
      this.panel3.Location = new Point(0, 2);
      this.panel3.Name = "panel3";
      this.panel3.Size = new Size(385, 266);
      this.panel3.TabIndex = 24;
      this.cboxAgencyDeliveryType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboxAgencyDeliveryType.FormattingEnabled = true;
      this.cboxAgencyDeliveryType.Location = new Point(187, 83);
      this.cboxAgencyDeliveryType.Name = "cboxAgencyDeliveryType";
      this.cboxAgencyDeliveryType.Size = new Size(139, 22);
      this.cboxAgencyDeliveryType.TabIndex = 124;
      this.cboxAgencyDeliveryType.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.cboxAgencyName.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboxAgencyName.FormattingEnabled = true;
      this.cboxAgencyName.Location = new Point(187, 59);
      this.cboxAgencyName.Name = "cboxAgencyName";
      this.cboxAgencyName.Size = new Size(139, 22);
      this.cboxAgencyName.TabIndex = 123;
      this.cboxAgencyName.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.cboxRepWarrantType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboxRepWarrantType.FormattingEnabled = true;
      this.cboxRepWarrantType.Location = new Point(187, 36);
      this.cboxRepWarrantType.Name = "cboxRepWarrantType";
      this.cboxRepWarrantType.Size = new Size(139, 22);
      this.cboxRepWarrantType.TabIndex = 122;
      this.cboxRepWarrantType.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.cboxFundType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboxFundType.FormattingEnabled = true;
      this.cboxFundType.Location = new Point(187, 13);
      this.cboxFundType.Name = "cboxFundType";
      this.cboxFundType.Size = new Size(139, 22);
      this.cboxFundType.TabIndex = 121;
      this.cboxFundType.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(68, 188);
      this.label2.Name = "label2";
      this.label2.Size = new Size(0, 14);
      this.label2.TabIndex = 120;
      this.label11.AutoSize = true;
      this.label11.Location = new Point(5, 110);
      this.label11.Name = "label11";
      this.label11.Size = new Size(77, 14);
      this.label11.TabIndex = 110;
      this.label11.Text = "Doc Custodian";
      this.label22.AutoSize = true;
      this.label22.Location = new Point(5, 87);
      this.label22.Name = "label22";
      this.label22.Size = new Size(113, 14);
      this.label22.TabIndex = 105;
      this.label22.Text = "Agency Delivery Type";
      this.label23.AutoSize = true;
      this.label23.Location = new Point(5, 64);
      this.label23.Name = "label23";
      this.label23.Size = new Size(75, 14);
      this.label23.TabIndex = 100;
      this.label23.Text = "Agency Name";
      this.btnDocCustodian.BackColor = Color.Transparent;
      this.btnDocCustodian.Location = new Point(330, 109);
      this.btnDocCustodian.MouseDownImage = (System.Drawing.Image) null;
      this.btnDocCustodian.Name = "btnDocCustodian";
      this.btnDocCustodian.Size = new Size(16, 16);
      this.btnDocCustodian.StandardButtonType = StandardIconButton.ButtonType.RolodexButton;
      this.btnDocCustodian.TabIndex = 41;
      this.btnDocCustodian.TabStop = false;
      this.btnDocCustodian.Click += new EventHandler(this.btnDocCustodian_Click);
      this.txtDocCustodian.Location = new Point(186, 107);
      this.txtDocCustodian.MaxLength = 200;
      this.txtDocCustodian.Name = "txtDocCustodian";
      this.txtDocCustodian.Size = new Size(139, 20);
      this.txtDocCustodian.TabIndex = 99;
      this.txtDocCustodian.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.label25.AutoSize = true;
      this.label25.Location = new Point(5, 41);
      this.label25.Name = "label25";
      this.label25.Size = new Size(158, 14);
      this.label25.TabIndex = 95;
      this.label25.Text = "Origination Rep && Warrant Type";
      this.label26.AutoSize = true;
      this.label26.Location = new Point(5, 18);
      this.label26.Name = "label26";
      this.label26.Size = new Size(57, 14);
      this.label26.TabIndex = 90;
      this.label26.Text = "Fund Type";
      this.tpPricing.Controls.Add((Control) this.pnlPricing);
      this.tpPricing.Location = new Point(4, 24);
      this.tpPricing.Name = "tpPricing";
      this.tpPricing.Padding = new Padding(0, 2, 2, 2);
      this.tpPricing.Size = new Size(1234, 741);
      this.tpPricing.TabIndex = 1;
      this.tpPricing.Tag = (object) "Pricing";
      this.tpPricing.Text = "Pricing";
      this.tpPricing.UseVisualStyleBackColor = true;
      this.tpPricing.Resize += new EventHandler(this.tpPricing_Resize);
      this.pnlPricing.Controls.Add((Control) this.btnSRPTemplate);
      this.pnlPricing.Controls.Add((Control) this.btnAdjustmentTemplate);
      this.pnlPricing.Controls.Add((Control) this.tableLayoutPanel1);
      this.pnlPricing.Dock = DockStyle.Fill;
      this.pnlPricing.Location = new Point(0, 2);
      this.pnlPricing.Name = "pnlPricing";
      this.pnlPricing.Size = new Size(1232, 737);
      this.pnlPricing.TabIndex = 3;
      this.btnSRPTemplate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnSRPTemplate.BackColor = SystemColors.Control;
      this.btnSRPTemplate.Location = new Point(1103, 8);
      this.btnSRPTemplate.Margin = new Padding(0);
      this.btnSRPTemplate.Name = "btnSRPTemplate";
      this.btnSRPTemplate.Size = new Size(70, 22);
      this.btnSRPTemplate.TabIndex = 1;
      this.btnSRPTemplate.Text = "Template";
      this.btnSRPTemplate.UseVisualStyleBackColor = true;
      this.btnSRPTemplate.Click += new EventHandler(this.btnSRPTemplate_Click);
      this.btnAdjustmentTemplate.BackColor = SystemColors.Control;
      this.btnAdjustmentTemplate.Location = new Point(0, 717);
      this.btnAdjustmentTemplate.Margin = new Padding(0);
      this.btnAdjustmentTemplate.Name = "btnAdjustmentTemplate";
      this.btnAdjustmentTemplate.Size = new Size(70, 20);
      this.btnAdjustmentTemplate.TabIndex = 1;
      this.btnAdjustmentTemplate.Text = "Template";
      this.btnAdjustmentTemplate.UseVisualStyleBackColor = true;
      this.btnAdjustmentTemplate.Click += new EventHandler(this.btnAdjustmentTemplate_Click);
      this.tableLayoutPanel1.ColumnCount = 2;
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
      this.tableLayoutPanel1.Controls.Add((Control) this.ctlSRP, 1, 0);
      this.tableLayoutPanel1.Controls.Add((Control) this.tableLayoutPanel2, 0, 0);
      this.tableLayoutPanel1.Dock = DockStyle.Fill;
      this.tableLayoutPanel1.Location = new Point(0, 0);
      this.tableLayoutPanel1.Margin = new Padding(0);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.RowCount = 1;
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
      this.tableLayoutPanel1.Size = new Size(1232, 737);
      this.tableLayoutPanel1.TabIndex = 13;
      this.ctlSRP.Dock = DockStyle.Fill;
      this.ctlSRP.Location = new Point(617, 0);
      this.ctlSRP.Margin = new Padding(1, 0, 0, 0);
      this.ctlSRP.Name = "ctlSRP";
      this.ctlSRP.ReadOnly = false;
      this.ctlSRP.Size = new Size(615, 737);
      this.ctlSRP.SRPfromPPE = false;
      this.ctlSRP.SRPTable = (SRPTable) null;
      this.ctlSRP.TabIndex = 2;
      this.tableLayoutPanel2.ColumnCount = 1;
      this.tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
      this.tableLayoutPanel2.Controls.Add((Control) this.ctlAdjustments, 0, 1);
      this.tableLayoutPanel2.Controls.Add((Control) this.ctlPricing, 0, 0);
      this.tableLayoutPanel2.Dock = DockStyle.Fill;
      this.tableLayoutPanel2.Location = new Point(0, 0);
      this.tableLayoutPanel2.Margin = new Padding(0);
      this.tableLayoutPanel2.Name = "tableLayoutPanel2";
      this.tableLayoutPanel2.RowCount = 2;
      this.tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
      this.tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
      this.tableLayoutPanel2.Size = new Size(616, 737);
      this.tableLayoutPanel2.TabIndex = 3;
      this.ctlAdjustments.AdjustmentfromPPE = false;
      this.ctlAdjustments.Adjustments = (TradePriceAdjustments) null;
      this.ctlAdjustments.Dock = DockStyle.Fill;
      this.ctlAdjustments.Location = new Point(0, 369);
      this.ctlAdjustments.Margin = new Padding(0, 1, 1, 0);
      this.ctlAdjustments.Name = "ctlAdjustments";
      this.ctlAdjustments.ReadOnly = false;
      this.ctlAdjustments.Size = new Size(615, 368);
      this.ctlAdjustments.TabIndex = 2;
      this.ctlPricing.Dock = DockStyle.Fill;
      this.ctlPricing.Location = new Point(0, 0);
      this.ctlPricing.Margin = new Padding(0, 0, 1, 1);
      this.ctlPricing.Name = "ctlPricing";
      this.ctlPricing.PricingItems = (TradePricingItems) null;
      this.ctlPricing.ReadOnly = false;
      this.ctlPricing.Size = new Size(615, 367);
      this.ctlPricing.TabIndex = 1;
      this.tpLoans.Location = new Point(4, 24);
      this.tpLoans.Name = "tpLoans";
      this.tpLoans.Padding = new Padding(0, 2, 2, 2);
      this.tpLoans.Size = new Size(1234, 741);
      this.tpLoans.TabIndex = 2;
      this.tpLoans.Tag = (object) "Loans";
      this.tpLoans.Text = "Loans";
      this.tpLoans.UseVisualStyleBackColor = true;
      this.tpLoans.Resize += new EventHandler(this.tpLoans_Resize);
      this.tpHistory.Controls.Add((Control) this.pnlHistory);
      this.tpHistory.Location = new Point(4, 24);
      this.tpHistory.Name = "tpHistory";
      this.tpHistory.Padding = new Padding(0, 2, 2, 2);
      this.tpHistory.Size = new Size(1234, 741);
      this.tpHistory.TabIndex = 4;
      this.tpHistory.Tag = (object) "History";
      this.tpHistory.Text = "Notes/History";
      this.tpHistory.UseVisualStyleBackColor = true;
      this.tpHistory.Resize += new EventHandler(this.tpHistory_Resize);
      this.pnlHistory.Controls.Add((Control) this.grpHistory);
      this.pnlHistory.Controls.Add((Control) this.grpNotes);
      this.pnlHistory.Controls.Add((Control) this.grpHistoryDetails);
      this.pnlHistory.Dock = DockStyle.Fill;
      this.pnlHistory.Location = new Point(0, 2);
      this.pnlHistory.Name = "pnlHistory";
      this.pnlHistory.Size = new Size(1232, 737);
      this.pnlHistory.TabIndex = 4;
      this.grpHistory.Controls.Add((Control) this.btnExportHistory);
      this.grpHistory.Controls.Add((Control) this.gvHistory);
      this.grpHistory.HeaderForeColor = SystemColors.ControlText;
      this.grpHistory.Location = new Point(460, 0);
      this.grpHistory.Name = "grpHistory";
      this.grpHistory.Size = new Size(444, 388);
      this.grpHistory.TabIndex = 4;
      this.grpHistory.Text = "History";
      this.gvHistory.BorderStyle = System.Windows.Forms.BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Event Time";
      gvColumn1.Width = 155;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.SpringToFit = true;
      gvColumn2.Text = "Event";
      gvColumn2.Width = 162;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "By";
      gvColumn3.Width = 125;
      this.gvHistory.Columns.AddRange(new GVColumn[3]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3
      });
      this.gvHistory.Dock = DockStyle.Fill;
      this.gvHistory.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvHistory.Location = new Point(1, 26);
      this.gvHistory.Name = "gvHistory";
      this.gvHistory.Size = new Size(442, 361);
      this.gvHistory.TabIndex = 2;
      this.gvHistory.ItemClick += new GVItemEventHandler(this.gvHistory_ItemClick);
      this.grpNotes.Controls.Add((Control) this.btnAddNotes);
      this.grpNotes.Controls.Add((Control) this.btnDelete);
      this.grpNotes.Controls.Add((Control) this.btnEdit);
      this.grpNotes.Controls.Add((Control) this.gvNotes);
      this.grpNotes.HeaderForeColor = SystemColors.ControlText;
      this.grpNotes.Location = new Point(0, 0);
      this.grpNotes.Name = "grpNotes";
      this.grpNotes.Size = new Size(455, 388);
      this.grpNotes.TabIndex = 3;
      this.grpNotes.Text = "Notes";
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column1";
      gvColumn4.Text = "Date Time";
      gvColumn4.Width = 160;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column2";
      gvColumn5.SpringToFit = true;
      gvColumn5.Text = "Details";
      gvColumn5.Width = 308;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Column3";
      gvColumn6.Text = "User";
      gvColumn6.Width = 140;
      this.gvNotes.Columns.AddRange(new GVColumn[3]
      {
        gvColumn4,
        gvColumn5,
        gvColumn6
      });
      this.gvNotes.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvNotes.Location = new Point(0, 26);
      this.gvNotes.Name = "gvNotes";
      this.gvNotes.Size = new Size(610, 362);
      this.gvNotes.TabIndex = 2;
      this.gvNotes.SelectedIndexChanged += new EventHandler(this.gvNotes_SelectedIndexChanged);
      this.gvNotes.Click += new EventHandler(this.gvNotes_Click);
      this.gvNotes.DoubleClick += new EventHandler(this.gvNotes_DoubleClick);
      this.grpHistoryDetails.Controls.Add((Control) this.txtHistoryDetails);
      this.grpHistoryDetails.HeaderForeColor = SystemColors.ControlText;
      this.grpHistoryDetails.Location = new Point(900, 0);
      this.grpHistoryDetails.Name = "grpHistoryDetails";
      this.grpHistoryDetails.Size = new Size(444, 388);
      this.grpHistoryDetails.TabIndex = 4;
      this.grpHistoryDetails.Text = "History Details";
      this.grpHistoryDetails.Visible = false;
      this.txtHistoryDetails.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.txtHistoryDetails.Dock = DockStyle.Fill;
      this.txtHistoryDetails.Location = new Point(1, 26);
      this.txtHistoryDetails.Name = "txtHistoryDetails";
      this.txtHistoryDetails.Size = new Size(442, 361);
      this.txtHistoryDetails.TabIndex = 2;
      this.txtHistoryDetails.Text = "";
      this.gradientPanel1.BackColorGlassyStyle = true;
      this.gradientPanel1.Borders = AnchorStyles.Bottom;
      this.gradientPanel1.Controls.Add((Control) this.flowLayoutPanel2);
      this.gradientPanel1.Controls.Add((Control) this.flowLayoutPanel1);
      this.gradientPanel1.Dock = DockStyle.Top;
      this.gradientPanel1.GradientColor1 = Color.FromArgb(81, 123, 184);
      this.gradientPanel1.GradientColor2 = Color.FromArgb(167, 201, 239);
      this.gradientPanel1.Location = new Point(0, 0);
      this.gradientPanel1.Name = "gradientPanel1";
      this.gradientPanel1.Size = new Size(1244, 31);
      this.gradientPanel1.Style = GradientPanel.PanelStyle.PageHeader;
      this.gradientPanel1.TabIndex = 6;
      this.flowLayoutPanel2.BackColor = Color.Transparent;
      this.flowLayoutPanel2.Controls.Add((Control) this.lblTradeName);
      this.flowLayoutPanel2.Controls.Add((Control) this.pictPending);
      this.flowLayoutPanel2.Location = new Point(19, 7);
      this.flowLayoutPanel2.Name = "flowLayoutPanel2";
      this.flowLayoutPanel2.Size = new Size(301, 20);
      this.flowLayoutPanel2.TabIndex = 227;
      this.lblTradeName.AutoSize = true;
      this.lblTradeName.BackColor = Color.Transparent;
      this.lblTradeName.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblTradeName.Location = new Point(3, 0);
      this.lblTradeName.Name = "lblTradeName";
      this.lblTradeName.Padding = new Padding(0, 3, 0, 0);
      this.lblTradeName.Size = new Size(84, 17);
      this.lblTradeName.TabIndex = 6;
      this.lblTradeName.Text = "<Trade Name>";
      this.pictPending.Image = (System.Drawing.Image) componentResourceManager.GetObject("pictPending.Image");
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
      this.flowLayoutPanel1.Size = new Size(802, 22);
      this.flowLayoutPanel1.TabIndex = 5;
      this.datePicker1.BackColor = SystemColors.Window;
      this.datePicker1.Location = new Point(187, 151);
      this.datePicker1.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.datePicker1.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.datePicker1.Name = "datePicker1";
      this.datePicker1.Size = new Size(104, 21);
      this.datePicker1.TabIndex = 124;
      this.datePicker1.ToolTip = "";
      this.datePicker1.Value = new DateTime(0L);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.Controls.Add((Control) this.grpEditor);
      this.Controls.Add((Control) this.gradientPanel1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (CorrespondentTradeEditor);
      this.Size = new Size(1244, 802);
      ((ISupportInitialize) this.btnExportHistory).EndInit();
      ((ISupportInitialize) this.btnList).EndInit();
      ((ISupportInitialize) this.btnSave).EndInit();
      ((ISupportInitialize) this.btnAddNotes).EndInit();
      ((ISupportInitialize) this.btnDelete).EndInit();
      ((ISupportInitialize) this.btnEdit).EndInit();
      this.grpEditor.ResumeLayout(false);
      this.tabTrade.ResumeLayout(false);
      this.tpDetails.ResumeLayout(false);
      this.pnlDetails.ResumeLayout(false);
      this.pnlRight.ResumeLayout(false);
      this.pnlFilter.ResumeLayout(false);
      this.pnlRightBottom.ResumeLayout(false);
      this.pnlLeft.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.groupContainer1.ResumeLayout(false);
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      this.pnlAotInfo.ResumeLayout(false);
      this.grpAOTInformation.ResumeLayout(false);
      this.panel4.ResumeLayout(false);
      this.tabControl1.ResumeLayout(false);
      this.tabPage1.ResumeLayout(false);
      this.panel15.ResumeLayout(false);
      this.panel15.PerformLayout();
      ((ISupportInitialize) this.btnSelector).EndInit();
      this.panel3.ResumeLayout(false);
      this.panel3.PerformLayout();
      ((ISupportInitialize) this.btnDocCustodian).EndInit();
      this.tpPricing.ResumeLayout(false);
      this.pnlPricing.ResumeLayout(false);
      this.tableLayoutPanel1.ResumeLayout(false);
      this.tableLayoutPanel2.ResumeLayout(false);
      this.tpHistory.ResumeLayout(false);
      this.pnlHistory.ResumeLayout(false);
      this.grpHistory.ResumeLayout(false);
      this.grpNotes.ResumeLayout(false);
      this.grpHistoryDetails.ResumeLayout(false);
      this.gradientPanel1.ResumeLayout(false);
      this.flowLayoutPanel2.ResumeLayout(false);
      this.flowLayoutPanel2.PerformLayout();
      ((ISupportInitialize) this.pictPending).EndInit();
      this.flowLayoutPanel1.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
