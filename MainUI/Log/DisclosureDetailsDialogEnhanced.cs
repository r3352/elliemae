// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.DisclosureDetailsDialogEnhanced
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.CalculationEngine;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.TimeZones;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.InputEngine;
using EllieMae.EMLite.JedScriptEngine;
using EllieMae.EMLite.LoanUtils.EDelivery;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class DisclosureDetailsDialogEnhanced : Form
  {
    private const string className = "DisclosureDetailsDialogEnhanced";
    private static readonly string sw = Tracing.SwInputEngine;
    private readonly IDisclosureManager disclosureManager;
    private bool suspendEvent;
    private EnhancedDisclosureTracking2015Log currentLog;
    private IContainer components;
    private TabControl tcDisclosure;
    private TabPage tabPageDetail;
    private BorderPanel pnlLoanSnapshot;
    private FieldLockButton lbtnFinanceCharge;
    private FieldLockButton lbtnDisclosedAPR;
    private GroupContainer gcDocList;
    private Button btnViewDisclosure;
    private GridView gvDocList;
    private TextBox txtApplicationDate;
    private Label label42;
    private TextBox txtFinanceCharge;
    private TextBox txtLoanProgram;
    private TextBox txtLoanAmount;
    private TextBox txtDisclosedAPR;
    private Label label38;
    private Label label39;
    private Label label40;
    private Label label41;
    private TextBox txtPropertyZip;
    private TextBox txtPropertyState;
    private TextBox txtPropertyCity;
    private TextBox txtPropertyAddress;
    private Label label37;
    private Label label36;
    private Label label35;
    private Label label34;
    private GradientPanel gradientPanel3;
    private FlowLayoutPanel flowLayoutPanel2;
    private Button btnSafeHarborSnapshot;
    private Button btnLESnapshot;
    private Label label20;
    private BorderPanel pnlBasicInfo;
    private GradientPanel gradientPanel1;
    private Label lblDisclosureInfo;
    private TabPage tabPageeDisclosure;
    private BorderPanel pnlFulfillment;
    private TextBox txtFulfillmentComments;
    private Label label43;
    private TextBox txtFulfillmentMethod;
    private Label label31;
    private TextBox txtDateFulfillOrder;
    private TextBox txtFulfillmentOrderBy;
    private Label label30;
    private Label label29;
    private GradientPanel gradientPanel4;
    private Button btneDiscManualFulfill;
    private Label label28;
    private BorderPanel pnleDisclosureStatus;
    private Label lblViewForm;
    private Label llViewDetails;
    private Label label19;
    private GradientPanel gradientPanel2;
    private Label label14;
    private FieldLockButton lbtnDisclosedDailyInterest;
    private TextBox txtDisclosedDailyInterest;
    private Label label1;
    private Label lblDateeDisclosureSent;
    private TableLayoutPanel tableLayoutPanel1;
    private Panel panel1;
    private ComboBox cmbRecipientReceivedMethod;
    private Label label3;
    private ComboBox cmbDisclosureType;
    private TextBox txtDisclosedBy;
    private Label lblDisclosureType;
    private FieldLockButton lbtnDisclosedBy;
    private FieldLockButton lbtnSentDate;
    private TextBox txtSentMethod;
    private Label lblDisclosurePresumedReceivedDate;
    private DateTimePicker dtSentDate;
    private ComboBox cmbSentMethod;
    private Label lblBy;
    private Label bblSentMethod;
    private Label lblSentDate;
    private DatePicker dpRecipientActualReceivedDate;
    private Label label4;
    private Panel panel2;
    private CheckBox chkIntent;
    private Panel pnlIntent;
    private ComboBox cmbIntentReceivedMethod;
    private Label label5;
    private TextBox txtIntentReceived;
    private DateTimePicker dpIntentDate;
    private Label label6;
    private Label label7;
    private TextBox txtIntentComments;
    private Label label8;
    private Label lblSigner;
    private Label label12;
    private Label lblUserID;
    private TextBox txtUserID;
    private Button btnProviderListSnapshot;
    private TabPage tabPageReasons;
    private Panel panel5;
    private StandardIconButton btnSelect;
    private Label label10;
    private Panel pnlReason;
    private TextBox txtReasonOther;
    private CheckBox chkReasonOther;
    private CheckBox chkReason6;
    private CheckBox chkReason5;
    private CheckBox chkReason4;
    private CheckBox chkReason3;
    private CheckBox chkReason2;
    private CheckBox chkReason1;
    private CheckBox chkReason10;
    private CheckBox chkReason9;
    private CheckBox chkReason8;
    private Label label9;
    private TextBox txtCiCComment;
    private Label label25;
    private CheckBox chkLEDisclosedByBroker;
    private Button btnCDSnapshot;
    private CheckBox chkReason7;
    private TextBox txtRecipientReceivedMethod;
    private Button btnOK;
    private TextBox txtIntentToProceedMethod;
    private TextBox txtMethodOther;
    private TextBox txtIntentMethodOther;
    private TextBox txtRecipientOther;
    private GradientPanel gradientPanel5;
    private Label label26;
    private BorderPanel borderPanel1;
    private FieldLockButton lBtnIntentReceivedBy;
    private TextBox txtRecipientType;
    private TextBox txtChangedCircumstance;
    private Button btnItemSnapshot;
    private DatePicker dpActualFulfillmentDate;
    private Label label18;
    private Label label27;
    private DatePicker dpPresumedFulfillmentDate;
    private Label lbRevisedDueDate;
    private Label lbChangesRecievedDate;
    private DatePicker dpRevisedDueDate;
    private DatePicker dpChangesRecievedDate;
    private ComboBox cmbRecipientType;
    private Label lblePackageId;
    private Label label45;
    private CheckBox changedCircumstancesChkBx;
    private CheckBox feeLevelIndicatorChkBx;
    private ComboBox ddRecipient;
    private Label detailsDisclosureRecipientLbl;
    private DatePicker dpRecipientPresumedReceivedDate;
    private FieldLockButton lBtnRecipientType;
    private FieldLockButton lBtnRecipientPresumed;
    private GridView grdDisclosureTracking;
    private Label label44;
    private Label label46;
    private Button btnEsign;
    private Label feeDetailsLabel;
    private GridView feeDetailsGV;
    private Panel detailsPanel;
    private Panel reasonsPanel;
    private TextBox txtSource;
    private Label lblProvider;
    private TableLayoutPanel tableLayoutPanel2;
    private Button btnClose;
    private Panel pnlBottom;
    private Panel panel3;
    private Label lblViewSummary;
    private Panel pnlUserId;
    private TextBox txtProviderDescription;
    private CheckBox chkUseforUCDExport;
    private Label lblUseforUCDExport;
    private Button btnAuditTrail;
    private TextBox txtTrackingNumber;
    private Label label2;

    private string SelectedRecipientId { get; set; }

    public DisclosureDetailsDialogEnhanced(IDisclosureManager disclosureManager)
    {
      this.disclosureManager = disclosureManager;
      this.currentLog = (EnhancedDisclosureTracking2015Log) disclosureManager.DisclosureTrackingLog;
      this.InitializeComponent();
      this.changedCircumstancesChkBx.Enabled = disclosureManager.IsReasonsEnabled;
      this.suspendEvent = true;
      this.initControls();
      this.suspendEvent = false;
      this.populateRecipientsDropDown();
      if (this.currentLog.DisclosureRecipients.Count <= 0)
        return;
      this.ddRecipient.SelectedIndex = 0;
      this.RefreshDisclosure();
      this.calculateDisclosedDate();
      this.DisableControlsForLinkedLoans();
    }

    private void populateRecipientsDropDown()
    {
      this.ddRecipient.SelectedIndexChanged -= new EventHandler(this.ddRecipient_SelectedIndexChanged);
      this.ddRecipient.DataSource = (object) this.currentLog.DisclosureRecipients.Where<EnhancedDisclosureTracking2015Log.DisclosureRecipient>((Func<EnhancedDisclosureTracking2015Log.DisclosureRecipient, bool>) (x => !string.IsNullOrEmpty(x.Name))).ToList<EnhancedDisclosureTracking2015Log.DisclosureRecipient>();
      this.ddRecipient.DisplayMember = "Name";
      this.ddRecipient.SelectedIndexChanged += new EventHandler(this.ddRecipient_SelectedIndexChanged);
    }

    private void DisableControlsForLinkedLoans()
    {
      if (!this.disclosureManager.IsLinkedLoan)
        return;
      this.gradientPanel1.Enabled = false;
      this.panel1.Enabled = false;
      this.panel2.Enabled = false;
      this.lbtnDisclosedAPR.Enabled = false;
      this.txtDisclosedAPR.Enabled = false;
      this.lbtnDisclosedDailyInterest.Enabled = false;
      this.txtDisclosedDailyInterest.Enabled = false;
      this.lbtnFinanceCharge.Enabled = false;
      this.txtFinanceCharge.Enabled = false;
      this.pnlFulfillment.Enabled = false;
    }

    private DateTime getReceivedDate()
    {
      DateTime receivedDate = DateTime.MinValue;
      if (this.disclosureManager.RecipientItems[this.SelectedRecipientId].Role == EnhancedDisclosureTracking2015Log.DisclosureRecipientType.Borrower || this.disclosureManager.RecipientItems[this.SelectedRecipientId].Role == EnhancedDisclosureTracking2015Log.DisclosureRecipientType.CoBorrower)
      {
        receivedDate = this.dpRecipientActualReceivedDate.Value;
        if (this.dpRecipientPresumedReceivedDate.Value != DateTime.MinValue && (this.dpRecipientPresumedReceivedDate.Value < receivedDate || receivedDate == DateTime.MinValue))
          receivedDate = this.dpRecipientPresumedReceivedDate.Value;
      }
      foreach (KeyValuePair<string, EnhancedDisclosureTracking2015Log.DisclosureRecipient> recipientItem in this.disclosureManager.RecipientItems)
      {
        if (recipientItem.Value.Id != this.SelectedRecipientId && (recipientItem.Value.Role == EnhancedDisclosureTracking2015Log.DisclosureRecipientType.Borrower || recipientItem.Value.Role == EnhancedDisclosureTracking2015Log.DisclosureRecipientType.CoBorrower))
        {
          if (recipientItem.Value.ActualReceivedDate != DateTime.MinValue && (recipientItem.Value.ActualReceivedDate < receivedDate || receivedDate == DateTime.MinValue))
            receivedDate = recipientItem.Value.ActualReceivedDate;
          if (recipientItem.Value.PresumedReceivedDate.Value != DateTime.MinValue && (recipientItem.Value.PresumedReceivedDate.Value < receivedDate || receivedDate == DateTime.MinValue))
            receivedDate = recipientItem.Value.PresumedReceivedDate.Value;
        }
      }
      return receivedDate;
    }

    private void refreshIntentToProceed(DateTime borPre, DateTime borAct)
    {
      this.lBtnIntentReceivedBy.Enabled = this.disclosureManager.CanEditSentDateAndExternalField;
      this.chkIntent.Enabled = this.dpIntentDate.Enabled = this.txtIntentToProceedMethod.Enabled = this.cmbIntentReceivedMethod.Enabled = this.txtIntentMethodOther.Enabled = this.txtIntentComments.Enabled = this.disclosureManager.CanEditSentDateAndExternalField;
      DateTime receivedDate = this.getReceivedDate();
      DateTimeWithZone dateTimeWithZone = ((EnhancedDisclosureTracking2015Log) this.disclosureManager.DisclosureTrackingLog).ConvertToDateTimeWithZone(DateTime.Now);
      if (this.chkIntent.Checked && receivedDate > dateTimeWithZone.DateTime)
      {
        if (Utils.Dialog((IWin32Window) this, "Changing this date will nullify the current Intent to Proceed and Earliest Fee Collection dates. Do you still want to proceed?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
        {
          this.chkIntent.Checked = false;
        }
        else
        {
          this.dpRecipientPresumedReceivedDate.Value = borPre;
          this.dpRecipientActualReceivedDate.Value = borAct;
        }
      }
      if (receivedDate != DateTime.MinValue)
      {
        this.dpIntentDate.MinDate = receivedDate;
        this.chkIntent.Enabled = this.disclosureManager.DisclosureTrackingLog.IsDisclosed;
        if (receivedDate > dateTimeWithZone.DateTime)
          this.chkIntent.Enabled = false;
      }
      else
        this.chkIntent.Checked = this.chkIntent.Enabled = false;
      foreach (IDisclosureTracking2015Log disclosureTracking2015Log in Session.LoanData.GetLogList().GetAllIDisclosureTracking2015Log(true))
      {
        if (disclosureTracking2015Log.DisclosedForLE && disclosureTracking2015Log.IntentToProceed && disclosureTracking2015Log != this.disclosureManager.DisclosureTrackingLog)
        {
          this.chkIntent.Enabled = false;
          break;
        }
      }
    }

    private void initControls()
    {
      if (this.currentLog.Status == DisclosureTracking2015Log.TrackingLogStatus.Pending)
      {
        this.panel1.Enabled = false;
        this.panel2.Enabled = false;
        this.pnlLoanSnapshot.Enabled = false;
        this.pnlReason.Enabled = false;
        this.panel5.Enabled = false;
        this.pnlFulfillment.Enabled = false;
      }
      this.dpIntentDate.Format = DateTimePickerFormat.Custom;
      this.dpIntentDate.CustomFormat = "MM/dd/yyyy";
      this.dtSentDate.Format = DateTimePickerFormat.Custom;
      this.dtSentDate.CustomFormat = "MM/dd/yyyy";
      this.dpRecipientPresumedReceivedDate.MinValue = this.disclosureManager.DisclosureTrackingLog.DisclosedDate;
      this.cmbSentMethod.Items.Clear();
      this.cmbIntentReceivedMethod.Items.Clear();
      this.cmbRecipientReceivedMethod.Items.Clear();
      this.cmbIntentReceivedMethod.Items.Add((object) "");
      this.cmbRecipientReceivedMethod.DataSource = (object) this.disclosureManager.eClosingDisclosedMethod;
      this.cmbSentMethod.DataSource = (object) this.disclosureManager.eClosingDisclosedMethod;
      for (int index = 0; index < this.disclosureManager.SentMethod.Length; ++index)
      {
        if (this.disclosureManager.SentMethod[index] != "eFolder eDisclosures")
          this.cmbIntentReceivedMethod.Items.Add((object) this.disclosureManager.SentMethod[index]);
      }
      this.btnEsign.Visible = this.disclosureManager.IsPlatformLoan;
      this.btnEsign.Enabled = this.disclosureManager.IsEsignEnabled;
      this.btnAuditTrail.Visible = this.disclosureManager.IsPlatformLoan;
      if (string.IsNullOrEmpty(this.currentLog.Tracking.PackageId))
      {
        Tracing.Log(DisclosureDetailsDialogEnhanced.sw, TraceLevel.Warning, nameof (DisclosureDetailsDialogEnhanced), "No packageId for this disclosure tracking log.");
        this.btnAuditTrail.Visible = false;
      }
      this.lblUseforUCDExport.Visible = this.chkUseforUCDExport.Visible = this.disclosureManager.DisclosureTrackingLog.DisclosedForCD;
      this.lblUseforUCDExport.Enabled = this.chkUseforUCDExport.Enabled = this.disclosureManager.DisclosureTrackingLog.IsDisclosed;
    }

    private void SetButtons()
    {
      this.btnLESnapshot.Enabled = this.btnLESnapshot.Visible = this.disclosureManager.DisclosureTrackingLog.DisclosedForLE;
      this.btnCDSnapshot.Enabled = this.btnCDSnapshot.Visible = this.disclosureManager.DisclosureTrackingLog.DisclosedForCD;
      this.btnItemSnapshot.Enabled = this.btnItemSnapshot.Visible = this.disclosureManager.DisclosureTrackingLog.DisclosedForCD || this.disclosureManager.DisclosureTrackingLog.DisclosedForLE;
      this.btnSafeHarborSnapshot.Enabled = this.btnSafeHarborSnapshot.Visible = this.disclosureManager.DisclosureTrackingLog.DisclosedForSafeHarbor;
      this.btnProviderListSnapshot.Enabled = this.btnProviderListSnapshot.Visible = this.disclosureManager.DisclosureTrackingLog.ProviderListSent || this.disclosureManager.DisclosureTrackingLog.ProviderListNoFeeSent;
      this.btnProviderListSnapshot.Visible = this.disclosureManager.DisclosureTrackingLog.ProviderListSent || this.disclosureManager.DisclosureTrackingLog.ProviderListNoFeeSent;
    }

    private void SetDetailsTab()
    {
      bool flag = this.disclosureManager.DisclosureTrackingLog.DisclosedForLE || this.disclosureManager.DisclosureTrackingLog.DisclosedForCD;
      this.lblDisclosureType.Visible = this.cmbDisclosureType.Visible = flag;
      if (!flag)
        this.MoveControlsUp();
      this.chkIntent.Visible = this.pnlIntent.Visible = this.chkLEDisclosedByBroker.Visible = this.disclosureManager.DisclosureTrackingLog.DisclosedForLE;
      if (!this.disclosureManager.DisclosureTrackingLog.DisclosedForCD)
        return;
      this.cmbDisclosureType.Items.Add((object) "Post Consummation");
      this.chkUseforUCDExport.Checked = this.disclosureManager.DisclosureTrackingLog.UseForUCDExport;
    }

    private void MoveControlsUp()
    {
      Label lblSentDate = this.lblSentDate;
      Point location1 = this.lblSentDate.Location;
      int x1 = location1.X;
      location1 = this.lblSentDate.Location;
      int y1 = location1.Y - 25;
      Point point1 = new Point(x1, y1);
      lblSentDate.Location = point1;
      Label bblSentMethod = this.bblSentMethod;
      Point location2 = this.bblSentMethod.Location;
      int x2 = location2.X;
      location2 = this.bblSentMethod.Location;
      int y2 = location2.Y - 25;
      Point point2 = new Point(x2, y2);
      bblSentMethod.Location = point2;
      Label lblBy = this.lblBy;
      Point location3 = this.lblBy.Location;
      int x3 = location3.X;
      location3 = this.lblBy.Location;
      int y3 = location3.Y - 25;
      Point point3 = new Point(x3, y3);
      lblBy.Location = point3;
      FieldLockButton lbtnSentDate = this.lbtnSentDate;
      Point location4 = this.lbtnSentDate.Location;
      int x4 = location4.X;
      location4 = this.lbtnSentDate.Location;
      int y4 = location4.Y - 25;
      Point point4 = new Point(x4, y4);
      lbtnSentDate.Location = point4;
      DateTimePicker dtSentDate = this.dtSentDate;
      Point location5 = this.dtSentDate.Location;
      int x5 = location5.X;
      location5 = this.dtSentDate.Location;
      int y5 = location5.Y - 25;
      Point point5 = new Point(x5, y5);
      dtSentDate.Location = point5;
      FieldLockButton lbtnDisclosedBy = this.lbtnDisclosedBy;
      Point location6 = this.lbtnDisclosedBy.Location;
      int x6 = location6.X;
      location6 = this.lbtnDisclosedBy.Location;
      int y6 = location6.Y - 25;
      Point point6 = new Point(x6, y6);
      lbtnDisclosedBy.Location = point6;
      TextBox txtDisclosedBy = this.txtDisclosedBy;
      Point location7 = this.txtDisclosedBy.Location;
      int x7 = location7.X;
      location7 = this.txtDisclosedBy.Location;
      int y7 = location7.Y - 25;
      Point point7 = new Point(x7, y7);
      txtDisclosedBy.Location = point7;
      TextBox txtSentMethod = this.txtSentMethod;
      Point location8 = this.txtSentMethod.Location;
      int x8 = location8.X;
      location8 = this.txtSentMethod.Location;
      int y8 = location8.Y - 25;
      Point point8 = new Point(x8, y8);
      txtSentMethod.Location = point8;
      ComboBox cmbSentMethod = this.cmbSentMethod;
      Point location9 = this.cmbSentMethod.Location;
      int x9 = location9.X;
      location9 = this.cmbSentMethod.Location;
      int y9 = location9.Y - 25;
      Point point9 = new Point(x9, y9);
      cmbSentMethod.Location = point9;
      TextBox txtMethodOther = this.txtMethodOther;
      Point location10 = this.txtMethodOther.Location;
      int x10 = location10.X;
      location10 = this.txtMethodOther.Location;
      int y10 = location10.Y - 25;
      Point point10 = new Point(x10, y10);
      txtMethodOther.Location = point10;
      Label lblProvider = this.lblProvider;
      Point location11 = this.lblProvider.Location;
      int x11 = location11.X;
      location11 = this.lblProvider.Location;
      int y11 = location11.Y - 25;
      Point point11 = new Point(x11, y11);
      lblProvider.Location = point11;
      TextBox txtSource = this.txtSource;
      Point location12 = this.txtSource.Location;
      int x12 = location12.X;
      location12 = this.txtSource.Location;
      int y12 = location12.Y - 25;
      Point point12 = new Point(x12, y12);
      txtSource.Location = point12;
    }

    private void setReasonTab()
    {
      this.lbChangesRecievedDate.Text = "Changes Received Date";
      if (this.disclosureManager.DisclosureTrackingLog.DisclosedForCD)
      {
        this.dpChangesRecievedDate.Value = this.currentLog.ClosingDisclosure.ChangesReceivedDate;
        this.dpRevisedDueDate.Value = this.currentLog.ClosingDisclosure.RevisedDueDate;
        this.chkReason1.Text = "Change in APR";
        this.chkReason1.Checked = this.currentLog.ClosingDisclosure.IsChangeInAPR;
        this.chkReason2.Text = "Change in Loan Product";
        this.chkReason2.Checked = this.currentLog.ClosingDisclosure.IsChangeInLoanProduct;
        this.chkReason3.Text = "Prepayment Penalty Added";
        this.chkReason3.Checked = this.currentLog.ClosingDisclosure.IsPrepaymentPenaltyAdded;
        this.chkReason4.Text = "Changed Circumstance - Settlement Charges";
        this.chkReason4.Checked = this.currentLog.ClosingDisclosure.IsChangeInSettlementCharges;
        this.chkReason5.Text = "Changed Circumstance - Eligibility";
        this.chkReason5.Checked = this.currentLog.ClosingDisclosure.IsChangedCircumstanceEligibility;
        this.chkReason6.Text = "Revisions requested by the Consumer";
        this.chkReason6.Checked = this.currentLog.ClosingDisclosure.IsRevisionsRequestedByConsumer;
        this.chkReason7.Text = "Interest Rate dependent charges (Rate Lock)";
        this.chkReason7.Checked = this.currentLog.ClosingDisclosure.IsInterestRateDependentCharges;
        this.chkReason8.Text = "24-hour Advanced Preview";
        this.chkReason8.Checked = this.currentLog.ClosingDisclosure.Is24HourAdvancePreview;
        this.chkReason9.Text = "Tolerance Cure";
        this.chkReason9.Checked = this.currentLog.ClosingDisclosure.IsToleranceCure;
        this.chkReason10.Text = "Clerical Error Correction";
        this.chkReason10.Checked = this.currentLog.ClosingDisclosure.IsClericalErrorCorrection;
        this.lbRevisedDueDate.Text = "Revised CD Due Date";
        this.chkReasonOther.Checked = this.currentLog.ClosingDisclosure.IsOther;
        if (this.chkReasonOther.Checked)
          this.txtReasonOther.Text = this.currentLog.ClosingDisclosure.OtherDescription;
        this.AddCOCFieldsToGrid();
      }
      else if (this.disclosureManager.DisclosureTrackingLog.DisclosedForLE || this.disclosureManager.DisclosureTrackingLog.DisclosedForSafeHarbor || this.disclosureManager.DisclosureTrackingLog.ProviderListSent || this.disclosureManager.DisclosureTrackingLog.ProviderListNoFeeSent)
      {
        this.dpChangesRecievedDate.Value = this.currentLog.LoanEstimate.ChangesReceivedDate;
        this.dpRevisedDueDate.Value = this.currentLog.LoanEstimate.RevisedDueDate;
        this.chkReason1.Text = "Changed Circumstance - Settlement Charges";
        this.chkReason1.Checked = this.currentLog.LoanEstimate.IsChangedCircumstanceSettlementCharges;
        this.chkReason2.Text = "Changed Circumstance - Eligibility";
        this.chkReason2.Checked = this.currentLog.LoanEstimate.IsChangedCircumstanceEligibility;
        this.chkReason3.Text = "Revisions requested by the Consumer";
        this.chkReason3.Checked = this.currentLog.LoanEstimate.IsRevisionsRequestedByConsumer;
        this.chkReason4.Text = "Interest Rate dependent charges (Rate Lock)";
        this.chkReason4.Checked = this.currentLog.LoanEstimate.IsInterestRateDependentCharges;
        this.chkReason5.Text = "Expiration (Intent to Proceed received after 10 business days)";
        this.chkReason5.Checked = this.currentLog.LoanEstimate.IsExpiration;
        this.chkReason6.Text = "Delayed Settlement on Construction Loans";
        this.chkReason6.Checked = this.currentLog.LoanEstimate.IsDelayedSettlementOnConstructionLoans;
        this.chkReasonOther.Checked = this.currentLog.LoanEstimate.IsOther;
        this.txtReasonOther.Text = this.chkReasonOther.Checked ? this.currentLog.LoanEstimate.OtherDescription : "";
        this.chkReason7.Visible = false;
        this.chkReason8.Visible = false;
        this.chkReason9.Visible = false;
        this.chkReason10.Visible = false;
        this.chkReasonOther.Location = this.chkReason7.Location;
        this.txtReasonOther.Location = new Point(this.txtReasonOther.Location.X, this.chkReason7.Location.Y);
        this.pnlReason.Height -= 80;
        this.lbRevisedDueDate.Text = "Revised LE Due Date";
        this.AddCOCFieldsToGrid();
      }
      if (!this.disclosureManager.DisclosureTrackingLog.DisclosedForCD && !this.disclosureManager.DisclosureTrackingLog.DisclosedForLE && !this.disclosureManager.DisclosureTrackingLog.DisclosedForSafeHarbor && !this.disclosureManager.DisclosureTrackingLog.ProviderListSent && !this.disclosureManager.DisclosureTrackingLog.ProviderListNoFeeSent)
        this.tcDisclosure.TabPages.Remove(this.tabPageReasons);
      this.chkReasonOther_CheckedChanged((object) null, (EventArgs) null);
      this.chkReason1.Enabled = this.chkReason2.Enabled = this.chkReason3.Enabled = this.chkReason4.Enabled = this.chkReason5.Enabled = this.chkReason6.Enabled = this.chkReason7.Enabled = this.chkReason8.Enabled = this.chkReason9.Enabled = this.chkReason10.Enabled = this.chkReasonOther.Enabled = this.txtReasonOther.Enabled = this.disclosureManager.IsReasonsEnabled;
      this.feeLevelIndicatorChkBx.Enabled = false;
      if (Session.StartupInfo.EnableCoC)
      {
        this.feeDetailsGV.Visible = this.feeDetailsLabel.Visible = this.disclosureManager.FeeLevelIndicator;
      }
      else
      {
        this.feeDetailsGV.Visible = this.feeDetailsLabel.Visible = false;
        this.positionReasonsControls((Control) this.label9);
        this.positionReasonsControls((Control) this.chkReason1);
        this.positionReasonsControls((Control) this.chkReason2);
        this.positionReasonsControls((Control) this.chkReason3);
        this.positionReasonsControls((Control) this.chkReason4);
        this.positionReasonsControls((Control) this.chkReason5);
        this.positionReasonsControls((Control) this.chkReason6);
        this.positionReasonsControls((Control) this.chkReason7);
        this.positionReasonsControls((Control) this.chkReason8);
        this.positionReasonsControls((Control) this.chkReason9);
        this.positionReasonsControls((Control) this.chkReason10);
        this.positionReasonsControls((Control) this.chkReasonOther);
        this.positionReasonsControls((Control) this.txtReasonOther);
        this.positionReasonsControls((Control) this.lbChangesRecievedDate);
        this.positionReasonsControls((Control) this.lbRevisedDueDate);
        this.positionReasonsControls((Control) this.dpChangesRecievedDate);
        this.positionReasonsControls((Control) this.dpRevisedDueDate);
        this.pnlReason.Size = new Size(this.pnlReason.Size.Width, this.pnlReason.Size.Height - 29);
        this.pnlBottom.Size = new Size(this.pnlBottom.Size.Width, 40);
      }
      this.feeLevelIndicatorChkBx.Checked = this.disclosureManager.FeeLevelIndicator;
      this.changedCircumstancesChkBx.Checked = this.currentLog.Attributes.ContainsKey("XCOCChangedCircumstances") && this.currentLog.Attributes["XCOCChangedCircumstances"] == "Y";
      this.changedCircumstancesChkBx.Visible = this.feeLevelIndicatorChkBx.Visible = Session.StartupInfo.EnableCoC;
      if (!this.disclosureManager.FeeLevelIndicator)
        return;
      this.DisableReasonsTabControls();
    }

    private void positionReasonsControls(Control ctrl)
    {
      Control control = ctrl;
      Point location = ctrl.Location;
      int x = location.X;
      location = ctrl.Location;
      int y = location.Y - 29;
      Point point = new Point(x, y);
      control.Location = point;
    }

    private void DisableReasonsTabControls()
    {
      this.feeLevelIndicatorChkBx.Enabled = false;
      if (this.disclosureManager.DisclosureTrackingLog.DisclosedForLE)
      {
        this.chkReason1.Enabled = false;
        this.chkReason2.Enabled = false;
        this.chkReason3.Enabled = false;
      }
      this.chkReason4.Enabled = false;
      this.chkReason5.Enabled = false;
      this.chkReason6.Enabled = false;
      this.chkReason7.Enabled = false;
      this.dpRevisedDueDate.Enabled = false;
      if (this.chkReasonOther.Checked)
        return;
      this.txtReasonOther.Enabled = false;
    }

    private void AddCOCFieldsToGrid()
    {
      List<Tuple<string, string, string, string, string, string>> xcocFields = this.disclosureManager.GetXcocFields();
      if (xcocFields == null)
        return;
      foreach (Tuple<string, string, string, string, string, string> tuple in xcocFields)
        this.feeDetailsGV.Items.Add(new GVItem(tuple.Item1)
        {
          SubItems = {
            (object) tuple.Item2,
            (object) tuple.Item3,
            (object) tuple.Item4,
            (object) tuple.Item5,
            (object) tuple.Item6
          }
        });
    }

    private void initDisclosureMethodDependendentControls()
    {
      DisclosureTrackingBase.DisclosedMethod disclosureMethod = this.disclosureManager.DisclosureTrackingLog.DisclosureMethod;
      switch (disclosureMethod)
      {
        case DisclosureTrackingBase.DisclosedMethod.eDisclosure:
        case DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder:
        case DisclosureTrackingBase.DisclosedMethod.eClose:
          this.cmbSentMethod.Enabled = false;
          this.cmbRecipientReceivedMethod.Enabled = false;
          this.dpRecipientPresumedReceivedDate.ReadOnly = !this.disclosureManager.DisclosureTrackingLog.IsWetSigned || !this.currentLog.DisclosureRecipients.FirstOrDefault<EnhancedDisclosureTracking2015Log.DisclosureRecipient>((Func<EnhancedDisclosureTracking2015Log.DisclosureRecipient, bool>) (b => b.Role == EnhancedDisclosureTracking2015Log.DisclosureRecipientType.Borrower)).PresumedReceivedDate.UseUserValue;
          if (disclosureMethod != DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder)
            this.tcDisclosure.TabPages.Add(this.tabPageeDisclosure);
          if (this.disclosureManager.DisclosureTrackingLog.eDisclosurePackageCreatedDate != DateTime.MinValue)
            this.lblDateeDisclosureSent.Text = this.disclosureManager.DisclosureTrackingLog.eDisclosurePackageCreatedDate.ToString("MM/dd/yyyy hh:mm tt ") + this.disclosureManager.GetTimeZoneString(this.disclosureManager.DisclosureTrackingLog.eDisclosurePackageCreatedDate);
          else
            this.lblDateeDisclosureSent.Text = "";
          this.lblePackageId.Text = string.IsNullOrWhiteSpace(this.currentLog.Tracking.PackageId) ? "" : this.currentLog.Tracking.PackageId;
          this.txtProviderDescription.Visible = false;
          break;
        default:
          this.txtProviderDescription.Visible = true;
          this.txtProviderDescription.Text = this.currentLog.ProviderDescription;
          break;
      }
      if (disclosureMethod == DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder || disclosureMethod == DisclosureTrackingBase.DisclosedMethod.eClose)
      {
        this.cmbSentMethod.Enabled = false;
        this.cmbRecipientReceivedMethod.Enabled = false;
        this.dpRecipientPresumedReceivedDate.ReadOnly = true;
        this.lBtnRecipientPresumed.Enabled = false;
        this.dpRecipientPresumedReceivedDate.ReadOnly = true;
        this.dpRecipientActualReceivedDate.ReadOnly = true;
        this.dpRecipientPresumedReceivedDate.Text = "";
        this.dpRecipientActualReceivedDate.Text = "";
      }
      if (this.disclosureManager.CanEditSentDateAndExternalField)
      {
        if (disclosureMethod != DisclosureTrackingBase.DisclosedMethod.eDisclosure || disclosureMethod != DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder || disclosureMethod != DisclosureTrackingBase.DisclosedMethod.eClose)
          this.dpRecipientPresumedReceivedDate.ReadOnly = false;
        this.lbtnSentDate.Enabled = true;
        this.lbtnDisclosedBy.Enabled = true;
        this.lbtnDisclosedAPR.Enabled = true;
        this.lbtnDisclosedDailyInterest.Enabled = true;
        this.lbtnFinanceCharge.Enabled = true;
      }
      bool flag = this.currentLog.DisclosedMethod == DisclosureTrackingBase.DisclosedMethod.ByMail || this.currentLog.DisclosedMethod == DisclosureTrackingBase.DisclosedMethod.InPerson || this.currentLog.DisclosedMethod == DisclosureTrackingBase.DisclosedMethod.Email || this.currentLog.DisclosedMethod == DisclosureTrackingBase.DisclosedMethod.Fax || this.currentLog.DisclosedMethod == DisclosureTrackingBase.DisclosedMethod.Other;
      if (this.disclosureManager.DisclosureTrackingLog != null && !flag && this.disclosureManager.IsPlatformLoan || this.currentLog.Documents != null && !string.IsNullOrWhiteSpace(this.currentLog.Documents?.ViewableFormsFile))
        this.btnViewDisclosure.Visible = true;
      else
        this.btnViewDisclosure.Visible = false;
      switch (disclosureMethod)
      {
        case DisclosureTrackingBase.DisclosedMethod.eDisclosure:
          this.txtSentMethod.Visible = true;
          this.cmbSentMethod.Visible = false;
          this.cmbSentMethod.Text = "";
          this.txtSentMethod.BringToFront();
          break;
        case DisclosureTrackingBase.DisclosedMethod.Other:
          this.cmbSentMethod.SelectedIndex = this.cmbSentMethod.Items.IndexOf((object) this.disclosureManager.DiscloseMethod[5]);
          this.txtMethodOther.Text = this.disclosureManager.DisclosureTrackingLog.DisclosedMethodOther;
          this.txtMethodOther.Enabled = true;
          this.cmbSentMethod.Text = "";
          break;
        case DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder:
          this.txtSentMethod.Visible = true;
          this.txtSentMethod.Text = "Closing Docs Order";
          this.cmbSentMethod.Visible = false;
          this.cmbSentMethod.Text = "";
          this.txtSentMethod.BringToFront();
          break;
        case DisclosureTrackingBase.DisclosedMethod.eClose:
          this.txtSentMethod.Visible = true;
          this.txtSentMethod.Text = "eClose";
          this.cmbSentMethod.Visible = false;
          this.cmbSentMethod.Text = "";
          this.txtSentMethod.BringToFront();
          break;
        default:
          this.setSelectedDisclosedMethod(this.cmbSentMethod, disclosureMethod);
          break;
      }
      this.lbtnSentDate.Locked = this.disclosureManager.DisclosureTrackingLog.IsLocked;
      this.dtSentDate.Value = this.disclosureManager.DisclosedDate;
      this.dtSentDate.Enabled = this.disclosureManager.DisclosureTrackingLog.IsLocked;
      this.txtSource.Text = this.currentLog.Provider;
    }

    private void ClearActualReceivedDate()
    {
      switch (this.disclosureManager.DisclosureTrackingLog.DisclosureMethod)
      {
        case DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder:
        case DisclosureTrackingBase.DisclosedMethod.eClose:
          this.cmbSentMethod.Enabled = false;
          this.cmbRecipientReceivedMethod.Enabled = false;
          this.dpRecipientPresumedReceivedDate.ReadOnly = true;
          this.lBtnRecipientPresumed.Enabled = false;
          this.dpRecipientPresumedReceivedDate.ReadOnly = true;
          this.dpRecipientActualReceivedDate.ReadOnly = true;
          this.dpRecipientPresumedReceivedDate.Text = "";
          this.dpRecipientActualReceivedDate.Text = "";
          break;
      }
    }

    private void initLoanSnapshotPannelControls()
    {
      this.lbtnDisclosedAPR.Locked = this.disclosureManager.DisclosureTrackingLog.IsDisclosedAPRLocked;
      this.txtDisclosedAPR.Enabled = this.disclosureManager.DisclosureTrackingLog.IsDisclosedAPRLocked;
      this.lbtnDisclosedDailyInterest.Locked = this.currentLog.DisclosedDailyInterest.UseUserValue;
      this.txtDisclosedDailyInterest.Enabled = this.currentLog.DisclosedDailyInterest.UseUserValue;
      this.lbtnDisclosedBy.Locked = this.disclosureManager.DisclosureTrackingLog.IsDisclosedByLocked;
      this.txtDisclosedBy.Enabled = this.disclosureManager.DisclosureTrackingLog.IsDisclosedByLocked;
      this.lBtnIntentReceivedBy.Locked = this.currentLog.IntentToProceed.ReceivedBy.UseUserValue;
      this.txtIntentReceived.Enabled = this.currentLog.IntentToProceed.ReceivedBy.UseUserValue;
      this.lbtnFinanceCharge.Locked = this.currentLog.DisclosedFinanceCharge.UseUserValue;
      this.txtFinanceCharge.Enabled = this.currentLog.DisclosedFinanceCharge.UseUserValue;
      if (this.disclosureManager.DisclosureTrackingLog.IsDisclosedByLocked)
        this.txtDisclosedBy.Text = this.disclosureManager.DisclosureTrackingLog.LockedDisclosedByField;
      else
        this.txtDisclosedBy.Text = this.disclosureManager.DisclosureTrackingLog.DisclosedByFullName + "(" + this.disclosureManager.DisclosureTrackingLog.DisclosedBy + ")";
      this.txtPropertyAddress.Text = this.currentLog.PropertyAddress.AddressFullName;
      this.txtPropertyCity.Text = this.currentLog.PropertyAddress?.City;
      this.txtPropertyState.Text = this.currentLog.PropertyAddress.State;
      this.txtPropertyZip.Text = this.currentLog.PropertyAddress.Zip;
      this.txtDisclosedAPR.Text = this.disclosureManager.DisclosureTrackingLog.DisclosedAPR;
      this.txtDisclosedDailyInterest.Text = this.disclosureManager.DisclosureTrackingLog.DisclosedDailyInterest;
      this.txtLoanProgram.Text = this.currentLog.LoanProgram;
      this.txtLoanAmount.Text = this.currentLog.LoanAmount.ToString();
      this.txtFinanceCharge.Text = this.disclosureManager.DisclosureTrackingLog.FinanceCharge;
      this.txtApplicationDate.Text = this.currentLog.ApplicationDate == DateTime.MinValue ? "" : this.currentLog.ApplicationDate.ToString("MM/dd/yyyy");
      this.cmbSentMethod.Enabled = true;
      this.chkLEDisclosedByBroker.Checked = this.disclosureManager.DisclosureTrackingLog.LEDisclosedByBroker;
      this.cmbDisclosureType.SelectedItem = (object) this.disclosureManager.DisclosureTrackingLog.DisclosureType.ToString();
      if (this.disclosureManager.DisclosureTrackingLog.DisclosureType != DisclosureTracking2015Log.DisclosureTypeEnum.PostConsummation)
        return;
      this.cmbDisclosureType.SelectedItem = (object) "Post Consummation";
    }

    private void setIntendToProceedControls()
    {
      this.chkIntent.Checked = this.disclosureManager.DisclosureTrackingLog.IntentToProceed && DateTime.Today >= this.dpIntentDate.MinDate;
      this.chkIntent_CheckedChanged((object) null, (EventArgs) null);
      try
      {
        this.dpIntentDate.Value = this.currentLog.IntentToProceed.Date;
        this.dpIntentDate.CustomFormat = "MM/dd/yyyy";
      }
      catch
      {
        this.dpIntentDate.Text = "";
      }
      if (this.currentLog.IntentToProceed.ReceivedBy.UseUserValue)
      {
        this.lBtnIntentReceivedBy.Locked = this.currentLog.IntentToProceed.ReceivedBy.UseUserValue;
        this.txtIntentReceived.Text = this.currentLog.IntentToProceed.ReceivedBy.UserValue;
        this.txtIntentReceived.Enabled = this.lBtnIntentReceivedBy.Locked;
      }
      else
        this.txtIntentReceived.Text = this.currentLog.IntentToProceed.ReceivedBy.ToString() != "" ? this.currentLog.IntentToProceed.ReceivedBy.ComputedValue : Session.UserInfo.FullName + "(" + Session.UserInfo.Userid + ")";
      switch (this.currentLog.IntentToProceed.ReceivedMethod)
      {
        case DisclosureTrackingBase.DisclosedMethod.None:
          this.cmbIntentReceivedMethod.SelectedItem = (object) "";
          this.dpIntentDate.Enabled = true;
          break;
        case DisclosureTrackingBase.DisclosedMethod.eDisclosure:
          this.txtIntentToProceedMethod.Visible = true;
          this.cmbIntentReceivedMethod.Visible = false;
          this.txtIntentToProceedMethod.BringToFront();
          break;
        case DisclosureTrackingBase.DisclosedMethod.InPerson:
          this.cmbIntentReceivedMethod.SelectedItem = (object) this.disclosureManager.SentMethod[0];
          this.dpIntentDate.Enabled = true;
          break;
        case DisclosureTrackingBase.DisclosedMethod.Other:
          this.cmbIntentReceivedMethod.SelectedItem = (object) this.disclosureManager.SentMethod[4];
          this.txtIntentMethodOther.Text = this.currentLog.IntentToProceed.ReceivedMethodOther;
          break;
        case DisclosureTrackingBase.DisclosedMethod.Email:
          this.cmbIntentReceivedMethod.SelectedItem = (object) this.disclosureManager.SentMethod[2];
          this.dpIntentDate.Enabled = true;
          break;
        case DisclosureTrackingBase.DisclosedMethod.Phone:
          this.cmbIntentReceivedMethod.SelectedItem = (object) this.disclosureManager.SentMethod[1];
          this.dpIntentDate.Enabled = true;
          break;
        case DisclosureTrackingBase.DisclosedMethod.Signature:
          this.cmbIntentReceivedMethod.SelectedItem = (object) this.disclosureManager.SentMethod[3];
          this.dpIntentDate.Enabled = true;
          break;
        case DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder:
        case DisclosureTrackingBase.DisclosedMethod.eClose:
          this.txtIntentToProceedMethod.Visible = true;
          this.cmbIntentReceivedMethod.Visible = false;
          this.txtIntentToProceedMethod.BringToFront();
          break;
      }
      this.txtIntentComments.Text = this.currentLog.IntentToProceed.Comments;
    }

    private void setDocListControls()
    {
      foreach (DisclosureTrackingFormItem form in (IEnumerable<DisclosureTrackingFormItem>) this.currentLog.Documents.Forms)
        this.gvDocList.Items.Add(new GVItem(form.FormName)
        {
          SubItems = {
            (object) form.OutputFormTypeName
          }
        });
      this.gcDocList.Text = "Documents Sent (" + (object) this.gvDocList.Items.Count + ")";
    }

    private void disableControlsBasedonAccess()
    {
      if (!this.disclosureManager.HasAccessRight)
      {
        this.lbtnSentDate.Enabled = false;
        this.dtSentDate.Enabled = false;
        this.cmbSentMethod.Enabled = false;
        this.dpRecipientPresumedReceivedDate.ReadOnly = true;
        this.cmbRecipientType.Visible = false;
        this.lBtnRecipientType.Visible = false;
      }
      if (this.disclosureManager.CanEditSentDateAndExternalField)
        return;
      this.lbtnSentDate.Enabled = false;
      this.dtSentDate.Enabled = false;
      this.lbtnDisclosedBy.Enabled = false;
      this.txtDisclosedBy.Enabled = false;
      this.lbtnDisclosedAPR.Enabled = false;
      this.txtDisclosedAPR.Enabled = false;
      this.lbtnDisclosedDailyInterest.Enabled = false;
      this.txtDisclosedDailyInterest.Enabled = false;
      this.lbtnFinanceCharge.Enabled = false;
      this.txtFinanceCharge.Enabled = false;
      this.cmbDisclosureType.Enabled = false;
      this.txtSentMethod.Enabled = false;
      this.cmbRecipientReceivedMethod.Enabled = false;
      this.cmbSentMethod.Enabled = false;
      this.dpRecipientPresumedReceivedDate.Enabled = false;
      this.dpRecipientActualReceivedDate.Enabled = false;
      this.lBtnRecipientPresumed.Enabled = false;
      this.chkLEDisclosedByBroker.Enabled = false;
      this.chkIntent.Enabled = false;
      this.cmbRecipientType.Enabled = false;
      this.lBtnRecipientType.Enabled = false;
    }

    private void RefreshDisclosure()
    {
      this.suspendEvent = true;
      this.cleanHistoryDetail();
      this.tcDisclosure.TabPages.Remove(this.tabPageeDisclosure);
      this.ddRecipient_SelectedIndexChanged((object) null, (EventArgs) null);
      this.SetDetailsTab();
      this.initDisclosureMethodDependendentControls();
      this.initLoanSnapshotPannelControls();
      this.dtDisclosedDate_ValueChanged((object) null, (EventArgs) null);
      this.setIntendToProceedControls();
      this.setReasonTab();
      if (!string.IsNullOrEmpty(this.currentLog.ChangeInCircumstance))
        this.txtChangedCircumstance.Text = this.currentLog.ChangeInCircumstance;
      this.txtCiCComment.Text = this.disclosureManager.DisclosureTrackingLog.ChangeInCircumstanceComments;
      this.SetButtons();
      this.setDocListControls();
      this.disableControlsBasedonAccess();
      this.refreshIntentToProceed(this.disclosureManager.RecipientItems[this.SelectedRecipientId].PresumedReceivedDate.Value, this.disclosureManager.RecipientItems[this.SelectedRecipientId].ActualReceivedDate);
      this.suspendEvent = false;
      if (!this.disclosureManager.DisclosureTrackingLog.IsNboExist)
        return;
      this.disclosureManager.UpdateNboRecievedDates(this.dtSentDate.Value);
    }

    private void addDisclosureGridItem(string columnDesc, DateTime dateInfo)
    {
      GVItem gvItem = new GVItem();
      gvItem.SubItems.Add((object) columnDesc);
      if (dateInfo != DateTime.MinValue)
        gvItem.SubItems.Add((object) (dateInfo.ToString("MM/dd/yyyy hh:mm tt ") + this.disclosureManager.GetTimeZoneString(dateInfo)));
      else
        gvItem.SubItems.Add((object) "");
      this.grdDisclosureTracking.Items.Add(gvItem);
    }

    private void addDisclosureGridItem(string columnDesc, string info)
    {
      this.grdDisclosureTracking.Items.Add(new GVItem()
      {
        SubItems = {
          (object) columnDesc,
          (object) info
        }
      });
    }

    private void cleanHistoryDetail()
    {
      this.dpRecipientPresumedReceivedDate.Text = "";
      this.txtDisclosedBy.Text = "";
      this.txtPropertyAddress.Text = "";
      this.txtPropertyCity.Text = "";
      this.txtPropertyState.Text = "";
      this.txtPropertyZip.Text = "";
      this.txtDisclosedAPR.Text = "";
      this.txtDisclosedDailyInterest.Text = "";
      this.txtLoanProgram.Text = "";
      this.txtLoanAmount.Text = "";
      this.txtFinanceCharge.Text = "";
      this.txtApplicationDate.Text = "";
      this.gvDocList.Items.Clear();
      this.cmbSentMethod.SelectedItem = (object) "";
      this.cmbSentMethod.Enabled = false;
      this.dtSentDate.Enabled = false;
      this.dpRecipientPresumedReceivedDate.ReadOnly = true;
      this.btnLESnapshot.Enabled = false;
      this.btnCDSnapshot.Enabled = false;
      this.btnSafeHarborSnapshot.Enabled = false;
      this.txtSentMethod.Visible = false;
      this.cmbSentMethod.Visible = true;
      this.lbtnSentDate.Locked = false;
      this.lbtnSentDate.Enabled = false;
      this.lbtnDisclosedBy.Enabled = false;
      this.txtDisclosedBy.Enabled = false;
      this.lbtnDisclosedAPR.Enabled = false;
      this.txtDisclosedAPR.Enabled = false;
      this.lbtnDisclosedDailyInterest.Enabled = false;
      this.txtDisclosedDailyInterest.Enabled = false;
      this.lbtnFinanceCharge.Enabled = false;
      this.txtFinanceCharge.Enabled = false;
      this.lbtnDisclosedBy.Locked = false;
      this.lbtnDisclosedAPR.Locked = false;
      this.lbtnFinanceCharge.Locked = false;
      this.gcDocList.Text = "Documents Sent (" + (object) this.gvDocList.Items.Count + ")";
    }

    private void RefreshFulfillmentFields()
    {
      if (this.currentLog.Fulfillments.Count == 0)
        return;
      IList<EnhancedDisclosureTracking2015Log.FulfillmentFields> fulfillments = this.currentLog.Fulfillments;
      EnhancedDisclosureTracking2015Log.FulfillmentFields fulfillmentFields = fulfillments != null ? fulfillments.FirstOrDefault<EnhancedDisclosureTracking2015Log.FulfillmentFields>() : (EnhancedDisclosureTracking2015Log.FulfillmentFields) null;
      if (fulfillmentFields == null || !Enum.IsDefined(typeof (EnhancedDisclosureTracking2015Log.FulfillmentRecipientType), (object) this.disclosureManager.RecipientItems[this.SelectedRecipientId].Role.ToString()))
      {
        this.txtFulfillmentMethod.Text = this.txtFulfillmentOrderBy.Text = this.txtDateFulfillOrder.Text = this.txtFulfillmentComments.Text = this.txtTrackingNumber.Text = string.Empty;
        this.dpPresumedFulfillmentDate.Value = this.dpActualFulfillmentDate.Value = DateTime.MinValue;
      }
      else
      {
        if (!fulfillmentFields.IsManual)
        {
          if (this.disclosureManager.DisclosureTrackingLog.FullfillmentProcessedDate == DateTime.MinValue)
            this.txtFulfillmentMethod.Text = "";
          else if (fulfillmentFields.DisclosedMethod == DisclosureTrackingBase.DisclosedMethod.OvernightShipping)
            this.txtFulfillmentMethod.Text = "Overnight Shipping";
          else
            this.txtFulfillmentMethod.Text = this.disclosureManager.AutomaticFullfillmentServiceName;
          this.txtTrackingNumber.Text = fulfillmentFields.TrackingNumber;
          this.btneDiscManualFulfill.Visible = !this.disclosureManager.IsFulfillmentServiceEnabled && this.disclosureManager.HasManualFulfillmentPermission && fulfillmentFields.ProcessedDate.DateTime == DateTime.MinValue && this.disclosureManager.DisclosureTrackingLog.DisclosureMethod != DisclosureTrackingBase.DisclosedMethod.eClose;
        }
        else
        {
          switch (fulfillmentFields.DisclosedMethod)
          {
            case DisclosureTrackingBase.DisclosedMethod.ByMail:
              this.txtFulfillmentMethod.Text = "U.S. Mail";
              break;
            case DisclosureTrackingBase.DisclosedMethod.InPerson:
              this.txtFulfillmentMethod.Text = "In Person";
              break;
            default:
              this.txtFulfillmentMethod.Text = "";
              break;
          }
          this.btneDiscManualFulfill.Visible = false;
        }
        this.txtFulfillmentOrderBy.Text = fulfillmentFields.OrderedBy;
        this.dpActualFulfillmentDate.ReadOnly = !fulfillmentFields.IsManual;
        TextBox dateFulfillOrder = this.txtDateFulfillOrder;
        DateTimeWithZone dateTimeWithZone = fulfillmentFields.ProcessedDate;
        string str1;
        if (!(dateTimeWithZone.DateTime == DateTime.MinValue))
        {
          dateTimeWithZone = fulfillmentFields.ProcessedDate;
          string str2 = dateTimeWithZone.DateTime.ToString("MM/dd/yyyy hh:mm tt ");
          IDisclosureManager disclosureManager = this.disclosureManager;
          dateTimeWithZone = fulfillmentFields.ProcessedDate;
          DateTime dateTime = dateTimeWithZone.DateTime;
          string timeZoneString = disclosureManager.GetTimeZoneString(dateTime);
          str1 = str2 + timeZoneString;
        }
        else
          str1 = "";
        dateFulfillOrder.Text = str1;
        EnhancedDisclosureTracking2015Log.FulfillmentRecipient fulfillmentRecipient = fulfillmentFields.Recipients.FirstOrDefault<EnhancedDisclosureTracking2015Log.FulfillmentRecipient>((Func<EnhancedDisclosureTracking2015Log.FulfillmentRecipient, bool>) (fr => fr.Id == this.SelectedRecipientId));
        dateTimeWithZone = fulfillmentRecipient.PresumedDate;
        if (dateTimeWithZone.DateTime != DateTime.MinValue)
        {
          DatePicker presumedFulfillmentDate = this.dpPresumedFulfillmentDate;
          dateTimeWithZone = fulfillmentRecipient.PresumedDate;
          DateTime dateTime = dateTimeWithZone.DateTime;
          presumedFulfillmentDate.Value = dateTime;
        }
        dateTimeWithZone = fulfillmentRecipient.ActualDate;
        if (dateTimeWithZone.DateTime != DateTime.MinValue)
        {
          DatePicker actualFulfillmentDate = this.dpActualFulfillmentDate;
          dateTimeWithZone = fulfillmentRecipient.ActualDate;
          DateTime dateTime = dateTimeWithZone.DateTime;
          actualFulfillmentDate.Value = dateTime;
        }
        this.txtFulfillmentComments.Text = fulfillmentRecipient.Comments;
      }
    }

    private void refreshUpdatedItem()
    {
      if (string.Concat(this.cmbSentMethod.SelectedItem) == this.disclosureManager.DiscloseMethod[1])
      {
        if (this.lBtnRecipientPresumed.Locked)
          return;
        this.dpRecipientPresumedReceivedDate.Text = "";
      }
      else if ((this.disclosureManager.DisclosureTrackingLog.DisclosedForLE || this.disclosureManager.DisclosureTrackingLog.DisclosedForCD) && this.disclosureManager.DisclosureTrackingLog.DisclosureMethod != DisclosureTrackingBase.DisclosedMethod.eDisclosure)
      {
        if (this.disclosureManager.DisclosureTrackingLog.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder || this.disclosureManager.DisclosureTrackingLog.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.eClose || this.lBtnRecipientPresumed.Locked)
          return;
        this.dpRecipientPresumedReceivedDate.Value = this.disclosureManager.GetReceivedDateByAdding3BusinessDays(this.dtSentDate.Value);
      }
      else
      {
        if (this.disclosureManager.DisclosureTrackingLog.DisclosureMethod != DisclosureTrackingBase.DisclosedMethod.eDisclosure)
          return;
        this.calcToPopulateControls();
      }
    }

    private void lbtnSentDate_Click(object sender, EventArgs e)
    {
      this.lbtnSentDate.Locked = !this.lbtnSentDate.Locked;
      if (!this.lbtnSentDate.Locked)
      {
        this.dtSentDate.Value = this.currentLog.DisclosedDate.ComputedValue.DateTime;
        if (this.disclosureManager.DisclosureTrackingLog.DisclosedForLE || this.disclosureManager.DisclosureTrackingLog.DisclosedForCD)
        {
          if ((this.disclosureManager.DisclosureTrackingLog.DisclosureMethod != DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder || this.disclosureManager.DisclosureTrackingLog.DisclosureMethod != DisclosureTrackingBase.DisclosedMethod.eClose) && Utils.IsDate((object) this.disclosureManager.LoanData.GetField("3983")) && !this.lBtnRecipientPresumed.Locked)
            this.dpRecipientPresumedReceivedDate.Value = this.disclosureManager.GetReceivedDateByAdding3BusinessDays(this.dtSentDate.Value);
        }
        else
          this.dpRecipientPresumedReceivedDate.Text = "";
        this.dtSentDate.Enabled = false;
        this.refreshUpdatedItem();
      }
      else
      {
        if (this.currentLog.DisclosedDate.UserValue != DateTime.MinValue)
          this.dtSentDate.Value = this.currentLog.DisclosedDate.UserValue;
        if (this.disclosureManager.DisclosureTrackingLog.DisclosedForLE || this.disclosureManager.DisclosureTrackingLog.DisclosedForCD)
        {
          if ((this.disclosureManager.DisclosureTrackingLog.DisclosureMethod != DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder || this.disclosureManager.DisclosureTrackingLog.DisclosureMethod != DisclosureTrackingBase.DisclosedMethod.eClose) && Utils.IsDate((object) this.disclosureManager.LoanData.GetField("3983")) && !this.lBtnRecipientPresumed.Locked)
            this.dpRecipientPresumedReceivedDate.Value = this.disclosureManager.GetReceivedDateByAdding3BusinessDays(this.dtSentDate.Value);
        }
        else
          this.dpRecipientPresumedReceivedDate.Text = "";
        this.dtSentDate.Enabled = true;
        this.refreshUpdatedItem();
        if (!this.disclosureManager.DisclosureTrackingLog.IsNboExist)
          return;
        this.disclosureManager.UpdateNboRecievedDates(this.dtSentDate.Value);
      }
    }

    private void calculateDisclosedDate()
    {
      if (!Utils.IsDate((object) this.dtSentDate.Text) || this.disclosureManager.DisclosureTrackingLog == null)
        return;
      if (this.disclosureManager.DisclosureTrackingLog.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.ByMail && this.disclosureManager.DisclosureTrackingLog.IsLocked)
      {
        DateTime closestBusinessDay = this.disclosureManager.GetNextClosestBusinessDay(this.dtSentDate.Value);
        if (closestBusinessDay != this.dtSentDate.Value)
        {
          this.ShowDeliverMessageDialog();
          this.dtSentDate.Value = closestBusinessDay;
        }
      }
      DateTime receivedDate;
      if (this.disclosureManager.DisclosureTrackingLog.ReceivedDate != DateTime.MinValue)
      {
        receivedDate = this.dtSentDate.Value;
        string str1 = receivedDate.ToString("MM/dd/yyyy");
        receivedDate = this.disclosureManager.DisclosureTrackingLog.ReceivedDate;
        string str2 = receivedDate.ToString("MM/dd/yyyy");
        if (str1 != str2 && this.dtSentDate.Value > this.disclosureManager.DisclosureTrackingLog.ReceivedDate && this.disclosureManager.DisclosureTrackingLog.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.Other)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Borrower Received Date cannot be earlier than Sent Date");
        }
      }
      DateTime disclosedDate1 = this.disclosureManager.DisclosedDate;
      try
      {
        if (this.lbtnSentDate.Locked)
        {
          EnhancedDisclosureTracking2015Log.DisclosedDateField disclosedDate2 = this.currentLog.DisclosedDate;
          receivedDate = this.dtSentDate.Value;
          DateTime dateTime = DateTime.Parse(receivedDate.ToString("MM/dd/yyyy  HH:mm:ss"));
          disclosedDate2.UserValue = dateTime;
        }
        this.disclosureManager.DisclosureTrackingLog.IsLocked = this.lbtnSentDate.Locked;
      }
      catch (Exception ex)
      {
        this.dtSentDate.Value = disclosedDate1;
        this.disclosureManager.DisclosedDate = disclosedDate1;
        throw ex;
      }
      this.refreshUpdatedItem();
    }

    private async void btnEsign_Click(object sender, EventArgs e)
    {
      DisclosureDetailsDialogEnhanced parentForm = this;
      try
      {
        parentForm.btnEsign.Enabled = false;
        int num = (int) await new ESignLO(parentForm.disclosureManager.LoanData.GUID).ShowESignDialog(parentForm.disclosureManager.DisclosureTrackingLog, (Form) parentForm, Session.UserInfo.Userid);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
      finally
      {
        parentForm.btnEsign.Enabled = parentForm.disclosureManager.DisclosureTrackingLog.eDisclosureLOeSignedDate == DateTime.MinValue;
      }
    }

    private void lbtnDisclosedBy_Click(object sender, EventArgs e)
    {
      this.lbtnDisclosedBy.Locked = !this.lbtnDisclosedBy.Locked;
      if (!this.lbtnDisclosedBy.Locked)
      {
        this.txtDisclosedBy.Text = this.disclosureManager.DisclosureTrackingLog.DisclosedByFullName + "(" + this.disclosureManager.DisclosureTrackingLog.DisclosedBy + ")";
        this.txtDisclosedBy.Enabled = false;
        this.refreshUpdatedItem();
      }
      else
      {
        this.txtDisclosedBy.Text = this.disclosureManager.DisclosureTrackingLog.LockedDisclosedByField;
        this.txtDisclosedBy.Enabled = true;
      }
    }

    private void lblSnapshot_Click(object sender, EventArgs e)
    {
      int num = (int) new DisclosedLEDialog(this.disclosureManager.DisclosureTrackingLog).ShowDialog((IWin32Window) this);
    }

    private void btnSafeHarborSnapshot_Click(object sender, EventArgs e)
    {
      int num = (int) new DisclosedSafeHarborDialog((IDisclosureTrackingLog) this.disclosureManager.DisclosureTrackingLog).ShowDialog((IWin32Window) this);
    }

    private void lbtnDisclosedAPR_Click(object sender, EventArgs e)
    {
      this.lbtnDisclosedAPR.Locked = !this.lbtnDisclosedAPR.Locked;
      if (!this.lbtnDisclosedAPR.Locked)
      {
        this.txtDisclosedAPR.Text = this.currentLog.DisclosedApr.ComputedValue;
        this.txtDisclosedAPR.Enabled = false;
      }
      else
        this.txtDisclosedAPR.Enabled = true;
      this.txtDisclosedAPR_Leave((object) null, (EventArgs) null);
    }

    private void txtDisclosedAPR_Leave(object sender, EventArgs e)
    {
      if (this.suspendEvent || this.disclosureManager.DisclosureTrackingLog == null)
        return;
      if (this.disclosureManager.IntermediateData)
      {
        this.disclosureManager.IntermediateData = false;
      }
      else
      {
        bool needsUpdate = false;
        string s = Utils.FormatInput(this.txtDisclosedAPR.Text, FieldFormat.DECIMAL_3, ref needsUpdate);
        string str;
        switch (s)
        {
          case ".":
            needsUpdate = true;
            str = "0.000";
            break;
          case "":
            needsUpdate = true;
            str = "";
            break;
          default:
            str = Decimal.Parse(s).ToString("0.000");
            if (this.txtDisclosedAPR.Text != str)
            {
              needsUpdate = true;
              break;
            }
            break;
        }
        if (!needsUpdate)
          return;
        this.disclosureManager.IntermediateData = true;
        this.txtDisclosedAPR.Text = str;
        this.txtDisclosedAPR.SelectionStart = str.Length;
        this.disclosureManager.IntermediateData = false;
      }
    }

    private void lbtnFinanceCharge_Click(object sender, EventArgs e)
    {
      this.lbtnFinanceCharge.Locked = !this.lbtnFinanceCharge.Locked;
      if (!this.lbtnFinanceCharge.Locked)
      {
        this.txtFinanceCharge.Text = this.currentLog.DisclosedFinanceCharge.ComputedValue;
        this.txtFinanceCharge.Enabled = false;
      }
      else
        this.txtFinanceCharge.Enabled = true;
      this.txtFinanceCharge_Leave((object) null, (EventArgs) null);
    }

    private void txtFinanceCharge_Leave(object sender, EventArgs e)
    {
      if (this.suspendEvent || this.disclosureManager.DisclosureTrackingLog == null)
        return;
      bool needsUpdate = false;
      string s = Utils.FormatInput(this.txtFinanceCharge.Text, FieldFormat.DECIMAL_2, ref needsUpdate);
      string str;
      switch (s)
      {
        case ".":
          needsUpdate = true;
          str = "0.00";
          break;
        case "":
          needsUpdate = true;
          str = "";
          break;
        default:
          str = Decimal.Parse(s).ToString("C").Replace("$", "");
          if (this.txtDisclosedAPR.Text != str)
          {
            needsUpdate = true;
            break;
          }
          break;
      }
      if (!needsUpdate)
        return;
      this.disclosureManager.IntermediateData = true;
      this.txtFinanceCharge.Text = str;
      this.txtFinanceCharge.SelectionStart = str.Length;
      this.disclosureManager.IntermediateData = false;
    }

    private void btnViewDisclosure_Click(object sender, EventArgs e)
    {
      IEFolder service = Session.Application.GetService<IEFolder>();
      if (this.disclosureManager.IsLinkedLoan)
        service.ViewDisclosures(Session.LoanDataMgr.LinkedLoan, this.currentLog.Documents.ViewableFormsFile, this.disclosureManager.DisclosureTrackingLog.eDisclosurePackageID, this.currentLog.Guid);
      else
        service.ViewDisclosures(Session.LoanDataMgr, this.currentLog.Documents.ViewableFormsFile, this.disclosureManager.DisclosureTrackingLog.eDisclosurePackageID, this.currentLog.Guid);
    }

    private void llViewDetails_Click(object sender, EventArgs e)
    {
      if (this.disclosureManager.DisclosureTrackingLog == null)
        return;
      int num = (int) new eDisclosureDetails((IDisclosureTrackingLog) this.disclosureManager.DisclosureTrackingLog).ShowDialog((IWin32Window) this);
    }

    private void llViewDetails_MouseEnter(object sender, EventArgs e) => this.Cursor = Cursors.Hand;

    private void llViewDetails_MouseLeave(object sender, EventArgs e)
    {
      this.Cursor = Cursors.Default;
    }

    private void lblViewForm_Click(object sender, EventArgs e)
    {
      if (this.disclosureManager.DisclosureTrackingLog == null)
        return;
      string path = SystemSettings.TempFolderRoot + "DisclosureTracking\\";
      try
      {
        if (!Directory.Exists(path))
          Directory.CreateDirectory(path);
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Failed to create temporary location at " + path + ".");
        return;
      }
      LoanDataMgr loanDataMgr = this.disclosureManager.IsLinkedLoan ? Session.LoanDataMgr.LinkedLoan : Session.LoanDataMgr;
      try
      {
        BinaryObject binaryObject = (BinaryObject) null;
        if (loanDataMgr.IsPlatformLoan())
        {
          if (!string.IsNullOrEmpty(this.disclosureManager.DisclosureTrackingLog.eDisclosurePackageID))
            binaryObject = new EDeliveryRestClient(loanDataMgr).GetPackageConsentPdf(this.disclosureManager.DisclosureTrackingLog.eDisclosurePackageID).Result;
        }
        else if (!string.IsNullOrEmpty(this.disclosureManager.DisclosureTrackingLog.eDisclosureConsentPDF))
          binaryObject = loanDataMgr.GetSupportingData(this.disclosureManager.DisclosureTrackingLog.eDisclosureConsentPDF);
        if (binaryObject == null)
          throw new FileNotFoundException();
        string str = path + "eDisclosure_" + this.disclosureManager.DisclosureTrackingLog.Guid + ".pdf";
        binaryObject.Write(str);
        int num = (int) new PdfPreviewDialog(str, true, true, false).ShowDialog((IWin32Window) this);
      }
      catch (Exception ex)
      {
        Tracing.Log(DisclosureDetailsDialogEnhanced.sw, TraceLevel.Error, nameof (DisclosureDetailsDialogEnhanced), "View Consent Form. Error: " + (object) ex);
        int num = (int) Utils.Dialog((IWin32Window) this, "Consent form is not available at this time.");
      }
    }

    private void lblViewForm_MouseEnter(object sender, EventArgs e) => this.Cursor = Cursors.Hand;

    private void lblViewForm_MouseLeave(object sender, EventArgs e)
    {
      this.Cursor = Cursors.Default;
    }

    private void btneDiscManualFulfill_Click(object sender, EventArgs e)
    {
      if (this.currentLog.Documents.ViewableFormsFile == string.Empty)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "There are no viewable documents associated with this disclosure tracking entry.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        ManualFulfillmentDialog fulfillmentDialog = new ManualFulfillmentDialog((IDisclosureTrackingLog) this.disclosureManager.DisclosureTrackingLog);
        if (fulfillmentDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.disclosureManager.FulfillmentUpdated = true;
        this.disclosureManager.eDisclosureManuallyFulfilledBy = fulfillmentDialog.EDisclosureManuallyFulfilledBy;
        this.disclosureManager.eDisclosureManualFulfillmentDate = fulfillmentDialog.EDisclosureManualFulfillmentDate;
        this.disclosureManager.eDisclosureManualFulfillmentMethod = fulfillmentDialog.EDisclosureManualFulfillmentMethod;
        this.disclosureManager.eDisclosureManualFulfillmentComment = fulfillmentDialog.EDisclosureManualFulfillmentComment;
        this.disclosureManager.eDisclosurePresumedDate = fulfillmentDialog.EDisclosurePresumedDate;
        this.disclosureManager.eDisclosureActualDate = fulfillmentDialog.EDisclosureActualDate;
        this.txtFulfillmentOrderBy.Text = this.disclosureManager.eDisclosureManuallyFulfilledBy;
        this.txtDateFulfillOrder.Text = this.disclosureManager.eDisclosureManualFulfillmentDate.ToString();
        switch (this.disclosureManager.eDisclosureManualFulfillmentMethod)
        {
          case DisclosureTrackingBase.DisclosedMethod.ByMail:
            this.txtFulfillmentMethod.Text = this.disclosureManager.DiscloseMethod[0];
            break;
          case DisclosureTrackingBase.DisclosedMethod.InPerson:
            this.txtFulfillmentMethod.Text = this.disclosureManager.DiscloseMethod[1];
            break;
        }
        this.txtFulfillmentComments.Text = this.disclosureManager.eDisclosureManualFulfillmentComment;
        this.dpPresumedFulfillmentDate.Value = this.disclosureManager.eDisclosurePresumedDate;
        this.dpActualFulfillmentDate.ReadOnly = false;
        this.dpActualFulfillmentDate.Value = this.disclosureManager.eDisclosureActualDate;
        Dictionary<string, object> disclosureReceivedDate = this.disclosureManager.CalculateNew2015DisclosureReceivedDate(this.dtSentDate.Value, this.disclosureManager.eDisclosurePresumedDate, this.disclosureManager.eDisclosureActualDate, this.disclosureManager.eDisclosureManualFulfillmentMethod);
        this.ShowControlsBasedonDisclosedMethod((DisclosureTrackingBase.DisclosedMethod) disclosureReceivedDate["BorrowerDisclosedMethod"]);
        this.SetRecipientControls((IReadOnlyDictionary<string, object>) disclosureReceivedDate);
        if (this.disclosureManager.DisclosureTrackingLog.CoBorrowerName.Trim() != "" && (DisclosureTrackingBase.DisclosedMethod) disclosureReceivedDate["CoBorrowerDisclosedMethod"] == DisclosureTrackingBase.DisclosedMethod.eDisclosure)
        {
          this.txtRecipientReceivedMethod.Visible = true;
          this.cmbRecipientReceivedMethod.Visible = false;
          this.txtRecipientReceivedMethod.BringToFront();
          this.txtRecipientReceivedMethod.Text = this.disclosureManager.DiscloseMethod[3];
        }
        this.disclosureManager.UpdateFulfillmentFields(this.SelectedRecipientId, this.txtFulfillmentOrderBy.Text, this.txtDateFulfillOrder.Text, this.dpPresumedFulfillmentDate.Value, this.dpActualFulfillmentDate.Value, this.txtFulfillmentComments.Text, this.txtFulfillmentMethod.Text);
        this.tcDisclosure.SelectedTab = this.tabPageeDisclosure;
      }
    }

    private void SetRecipientControls(IReadOnlyDictionary<string, object> result)
    {
      if (this.disclosureManager.RecipientItems[this.SelectedRecipientId].Role == EnhancedDisclosureTracking2015Log.DisclosureRecipientType.Borrower)
      {
        this.txtRecipientOther.Text = result["BorrowerFulfillmentMethodDescription"].ToString();
        if (this.dpRecipientActualReceivedDate.Value == DateTime.MinValue || this.dpRecipientActualReceivedDate.Value != DateTime.MinValue && Utils.ParseDate(result["BorrowerActualReceivedDate"]) != DateTime.MinValue)
          this.dpRecipientActualReceivedDate.Value = Utils.ParseDate(result["BorrowerActualReceivedDate"]);
        if (!(Utils.ParseDate(result["BorrowerPresumedReceivedDate"]) != DateTime.MinValue))
          return;
        this.dpRecipientPresumedReceivedDate.Value = Utils.ParseDate(result["BorrowerPresumedReceivedDate"]);
      }
      else
      {
        if (this.disclosureManager.RecipientItems[this.SelectedRecipientId].Role != EnhancedDisclosureTracking2015Log.DisclosureRecipientType.CoBorrower)
          return;
        this.txtRecipientOther.Text = result["CoBorrowerFulfillmentMethodDescription"].ToString();
        if (this.dpRecipientActualReceivedDate.Value == DateTime.MinValue || this.dpRecipientActualReceivedDate.Value != DateTime.MinValue && Utils.ParseDate(result["CoBorrowerActualReceivedDate"]) != DateTime.MinValue)
          this.dpRecipientActualReceivedDate.Value = Utils.ParseDate(result["CoBorrowerActualReceivedDate"]);
        if (!(Utils.ParseDate(result["CoBorrowerPresumedReceivedDate"]) != DateTime.MinValue))
          return;
        this.dpRecipientPresumedReceivedDate.Value = Utils.ParseDate(result["CoBorrowerPresumedReceivedDate"]);
      }
    }

    private void ShowControlsBasedonDisclosedMethod(
      DisclosureTrackingBase.DisclosedMethod disclosedMethod)
    {
      switch (disclosedMethod)
      {
        case DisclosureTrackingBase.DisclosedMethod.ByMail:
          this.txtRecipientReceivedMethod.Visible = false;
          this.cmbRecipientReceivedMethod.Visible = true;
          this.cmbRecipientReceivedMethod.SelectedItem = (object) this.disclosureManager.DiscloseMethod[0];
          break;
        case DisclosureTrackingBase.DisclosedMethod.eDisclosure:
          this.txtRecipientReceivedMethod.Visible = true;
          this.cmbRecipientReceivedMethod.Visible = false;
          this.txtRecipientReceivedMethod.BringToFront();
          this.txtRecipientReceivedMethod.Text = this.disclosureManager.DiscloseMethod[3];
          break;
        case DisclosureTrackingBase.DisclosedMethod.InPerson:
          this.txtRecipientReceivedMethod.Visible = false;
          this.cmbRecipientReceivedMethod.Visible = true;
          this.cmbRecipientReceivedMethod.SelectedItem = (object) this.disclosureManager.DiscloseMethod[1];
          break;
      }
    }

    private void btnClose_Click(object sender, EventArgs e) => this.Close();

    private void cmbDisclosedMethod_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.suspendEvent || this.disclosureManager.DisclosureTrackingLog == null)
        return;
      DisclosureTrackingBase.DisclosedMethod disclosedMethod = this.disclosureManager.GetDisclosedMethod(string.Concat(this.cmbSentMethod.SelectedItem));
      if (disclosedMethod == DisclosureTrackingBase.DisclosedMethod.eDisclosure || disclosedMethod == DisclosureTrackingBase.DisclosedMethod.Other || (uint) (disclosedMethod - 9) <= 1U)
        this.dpRecipientPresumedReceivedDate.Value = DateTime.MinValue;
      if (disclosedMethod == DisclosureTrackingBase.DisclosedMethod.Other)
      {
        this.txtMethodOther.Enabled = true;
        this.txtMethodOther.Text = this.disclosureManager.DisclosureTrackingLog.DisclosedMethodOther;
      }
      else
      {
        this.txtMethodOther.Enabled = false;
        this.txtMethodOther.Text = "";
      }
      if (disclosedMethod == DisclosureTrackingBase.DisclosedMethod.InPerson)
      {
        if (this.disclosureManager.DisclosureTrackingLog.DisclosureMethod != DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder || this.disclosureManager.DisclosureTrackingLog.DisclosureMethod != DisclosureTrackingBase.DisclosedMethod.eClose)
        {
          this.dpRecipientActualReceivedDate.Value = this.dtSentDate.Value;
          this.cmbRecipientReceivedMethod.SelectedItem = (object) this.disclosureManager.DiscloseMethod[1];
          foreach (KeyValuePair<string, EnhancedDisclosureTracking2015Log.DisclosureRecipient> recipientItem in this.disclosureManager.RecipientItems)
          {
            if (recipientItem.Value.Role == EnhancedDisclosureTracking2015Log.DisclosureRecipientType.Borrower || recipientItem.Value.Role == EnhancedDisclosureTracking2015Log.DisclosureRecipientType.CoBorrower)
            {
              recipientItem.Value.ActualReceivedDate = this.dtSentDate.Value;
              recipientItem.Value.DisclosedMethod = DisclosureTrackingBase.DisclosedMethod.InPerson;
            }
          }
          if (!this.lBtnRecipientPresumed.Locked)
            this.dpRecipientPresumedReceivedDate.Enabled = false;
        }
        this.dpReceivedDate_ValueChanged((object) null, (EventArgs) null);
      }
      this.dpRecipientPresumedReceivedDate.Focus();
      this.refreshUpdatedItem();
    }

    private void txtDisclosedDailyInterest_Leave(object sender, EventArgs e)
    {
      if (this.suspendEvent || this.disclosureManager.DisclosureTrackingLog == null)
        return;
      if (this.disclosureManager.IntermediateData)
      {
        this.disclosureManager.IntermediateData = false;
      }
      else
      {
        bool needsUpdate = false;
        string s = Utils.FormatInput(this.txtDisclosedDailyInterest.Text, FieldFormat.DECIMAL_2, ref needsUpdate);
        string str;
        switch (s)
        {
          case ".":
            needsUpdate = true;
            str = "0.00";
            break;
          case "":
            needsUpdate = true;
            str = "";
            break;
          default:
            str = Decimal.Parse(s).ToString("0.00");
            if (this.txtDisclosedDailyInterest.Text != str)
            {
              needsUpdate = true;
              break;
            }
            break;
        }
        if (!needsUpdate)
          return;
        this.disclosureManager.IntermediateData = true;
        this.txtDisclosedDailyInterest.Text = str;
        this.txtDisclosedDailyInterest.SelectionStart = str.Length;
        this.disclosureManager.IntermediateData = false;
      }
    }

    private void lbtnDisclosedDailyInterest_Click(object sender, EventArgs e)
    {
      this.lbtnDisclosedDailyInterest.Locked = !this.lbtnDisclosedDailyInterest.Locked;
      if (!this.lbtnDisclosedDailyInterest.Locked)
      {
        this.txtDisclosedDailyInterest.Text = this.currentLog.DisclosedDailyInterest.ComputedValue;
        this.txtDisclosedDailyInterest.Enabled = false;
      }
      else
        this.txtDisclosedDailyInterest.Enabled = true;
      this.txtDisclosedDailyInterest_Leave((object) null, (EventArgs) null);
    }

    private void btnSelect_Click(object sender, EventArgs e)
    {
      using (ChangeCircumstanceSelector circumstanceSelector = new ChangeCircumstanceSelector(true, this.disclosureManager.DisclosureTrackingLog.DisclosedForLE ? "LE" : "CD", this.disclosureManager.LoanData.GetField("4461") == "Y"))
      {
        if (circumstanceSelector.ShowDialog() != DialogResult.OK)
          return;
        List<string[]> allOptions = circumstanceSelector.AllOptions;
        string str1 = "";
        string str2 = "";
        foreach (string[] strArray in allOptions)
        {
          str1 = str1 + strArray[1] + Environment.NewLine;
          if (!str2.Contains(Environment.NewLine + strArray[2] + Environment.NewLine) && !str2.StartsWith(strArray[2]))
            str2 = str2 + strArray[2] + Environment.NewLine;
        }
        this.txtChangedCircumstance.Text = str1;
        this.txtCiCComment.Text = str2;
      }
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (this.suspendEvent || this.disclosureManager.DisclosureTrackingLog == null)
        return;
      this.calculateDisclosedDate();
      this.SetRecipientInMemory(this.SelectedRecipientId);
      this.disclosureManager.UpdateRecipients(this.disclosureManager.RecipientItems);
      this.currentLog.LoanEstimate.IsDisclosedByBroker = this.chkLEDisclosedByBroker.Checked;
      this.currentLog.ProviderDescription = Convert.ToString(this.txtProviderDescription.Text);
      if ((this.disclosureManager.DisclosureTrackingLog.DisclosedForLE || this.disclosureManager.DisclosureTrackingLog.DisclosedForCD) && this.cmbDisclosureType.SelectedItem != null)
        this.disclosureManager.DisclosureTrackingLog.DisclosureType = !(string.Concat(this.cmbDisclosureType.SelectedItem) == "Post Consummation") ? (DisclosureTracking2015Log.DisclosureTypeEnum) Enum.Parse(typeof (DisclosureTracking2015Log.DisclosureTypeEnum), this.cmbDisclosureType.SelectedItem.ToString()) : DisclosureTracking2015Log.DisclosureTypeEnum.PostConsummation;
      this.setDisclosedMethodValue(this.cmbSentMethod);
      if (this.disclosureManager.DisclosureTrackingLog.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.Other)
        this.disclosureManager.DisclosureTrackingLog.DisclosedMethodOther = this.txtMethodOther.Text;
      this.disclosureManager.DisclosureTrackingLog.IsDisclosedByLocked = this.lbtnDisclosedBy.Locked;
      if (this.disclosureManager.DisclosureTrackingLog.IsDisclosedByLocked)
        this.disclosureManager.DisclosureTrackingLog.LockedDisclosedByField = this.txtDisclosedBy.Text;
      if (this.disclosureManager.DisclosureTrackingLog.DisclosedForCD)
        DisclosureTrackingLogUtils.SetUseForUCDExport(this.disclosureManager.DisclosureTrackingLog, this.chkUseforUCDExport.Checked);
      this.setDisclosedMethodValue(this.cmbRecipientReceivedMethod);
      this.currentLog.IntentToProceed.ReceivedBy.UseUserValue = this.lBtnIntentReceivedBy.Locked;
      this.disclosureManager.DisclosureTrackingLog.IntentToProceed = this.chkIntent.Checked;
      if (this.currentLog.IntentToProceed.ReceivedBy.UseUserValue)
        this.currentLog.IntentToProceed.ReceivedBy.UserValue = this.txtIntentReceived.Text;
      if (this.disclosureManager.DisclosureTrackingLog.IntentToProceed)
      {
        this.currentLog.IntentToProceed.Date = this.dpIntentDate.Value;
        if (string.IsNullOrEmpty(this.currentLog.IntentToProceed.ReceivedBy.UserValue))
          this.currentLog.IntentToProceed.ReceivedBy.UserValue = Session.UserInfo.FullName + "(" + Session.UserInfo.Userid + ")";
      }
      this.setSentMethodValue(this.cmbIntentReceivedMethod);
      this.currentLog.IntentToProceed.Comments = this.txtIntentComments.Text;
      this.disclosureManager.UpdateLePage1IntendToProceed(this.chkIntent.Checked);
      if (this.disclosureManager.DisclosureTrackingLog.DisclosedForCD)
      {
        this.disclosureManager.DisclosureTrackingLog.CDReasonIsChangeInAPR = this.chkReason1.Checked;
        this.disclosureManager.DisclosureTrackingLog.CDReasonIsChangeInLoanProduct = this.chkReason2.Checked;
        this.disclosureManager.DisclosureTrackingLog.CDReasonIsPrepaymentPenaltyAdded = this.chkReason3.Checked;
        this.disclosureManager.DisclosureTrackingLog.CDReasonIsChangeInSettlementCharges = this.chkReason4.Checked;
        this.disclosureManager.DisclosureTrackingLog.CDReasonIsChangedCircumstanceEligibility = this.chkReason5.Checked;
        this.disclosureManager.DisclosureTrackingLog.CDReasonIsRevisionsRequestedByConsumer = this.chkReason6.Checked;
        this.disclosureManager.DisclosureTrackingLog.CDReasonIsInterestRateDependentCharges = this.chkReason7.Checked;
        this.disclosureManager.DisclosureTrackingLog.CDReasonIsOther = this.chkReasonOther.Checked;
        this.disclosureManager.DisclosureTrackingLog.CDReasonIs24HourAdvancePreview = this.chkReason8.Checked;
        this.disclosureManager.DisclosureTrackingLog.CDReasonIsToleranceCure = this.chkReason9.Checked;
        this.disclosureManager.DisclosureTrackingLog.CDReasonIsClericalErrorCorrection = this.chkReason10.Checked;
        if (this.chkReasonOther.Checked)
          this.currentLog.ClosingDisclosure.OtherDescription = this.txtReasonOther.Text;
        this.currentLog.ClosingDisclosure.ChangesReceivedDate = this.dpChangesRecievedDate.Value;
        this.currentLog.ClosingDisclosure.RevisedDueDate = this.dpRevisedDueDate.Value;
      }
      else if (this.disclosureManager.DisclosureTrackingLog.DisclosedForLE || this.disclosureManager.DisclosureTrackingLog.DisclosedForSafeHarbor || this.disclosureManager.DisclosureTrackingLog.ProviderListSent || this.disclosureManager.DisclosureTrackingLog.ProviderListNoFeeSent)
      {
        this.disclosureManager.DisclosureTrackingLog.LEReasonIsChangedCircumstanceSettlementCharges = this.chkReason1.Checked;
        this.disclosureManager.DisclosureTrackingLog.LEReasonIsChangedCircumstanceEligibility = this.chkReason2.Checked;
        this.disclosureManager.DisclosureTrackingLog.LEReasonIsRevisionsRequestedByConsumer = this.chkReason3.Checked;
        this.disclosureManager.DisclosureTrackingLog.LEReasonIsInterestRateDependentCharges = this.chkReason4.Checked;
        this.disclosureManager.DisclosureTrackingLog.LEReasonIsExpiration = this.chkReason5.Checked;
        this.disclosureManager.DisclosureTrackingLog.LEReasonIsDelayedSettlementOnConstructionLoans = this.chkReason6.Checked;
        this.disclosureManager.DisclosureTrackingLog.LEReasonIsOther = this.chkReasonOther.Checked;
        if (this.chkReasonOther.Checked)
          this.currentLog.LoanEstimate.OtherDescription = this.txtReasonOther.Text;
        this.currentLog.LoanEstimate.ChangesReceivedDate = this.dpChangesRecievedDate.Value;
        this.currentLog.LoanEstimate.RevisedDueDate = this.dpRevisedDueDate.Value;
      }
      this.currentLog.ChangeInCircumstance = this.txtChangedCircumstance.Text;
      this.disclosureManager.DisclosureTrackingLog.ChangeInCircumstanceComments = this.txtCiCComment.Text;
      this.disclosureManager.UpdateCocFields(this.feeLevelIndicatorChkBx.Checked, this.changedCircumstancesChkBx.Checked);
      this.disclosureManager.DisclosureTrackingLog.IsDisclosedAPRLocked = this.lbtnDisclosedAPR.Locked;
      this.disclosureManager.DisclosureTrackingLog.DisclosedAPR = this.txtDisclosedAPR.Text;
      this.currentLog.DisclosedDailyInterest.UseUserValue = this.lbtnDisclosedDailyInterest.Locked;
      this.disclosureManager.DisclosureTrackingLog.DisclosedDailyInterest = this.txtDisclosedDailyInterest.Text;
      this.currentLog.DisclosedFinanceCharge.UseUserValue = this.lbtnFinanceCharge.Locked;
      this.disclosureManager.DisclosureTrackingLog.FinanceCharge = this.txtFinanceCharge.Text;
      this.disclosureManager.UpdateFulfillment(this.txtFulfillmentMethod.Text, this.dpActualFulfillmentDate.Value, this.dtSentDate.Value, this.dpPresumedFulfillmentDate.Value, this.dpRecipientActualReceivedDate.Value);
      int num = this.disclosureManager.IsConstructionPrimaryLoan ? 1 : 0;
      this.DialogResult = DialogResult.OK;
    }

    private void setSentMethodValue(ComboBox cmbCtrl)
    {
      DisclosureTrackingBase.DisclosedMethod disclosedMethod = !(string.Concat(cmbCtrl.SelectedItem) == "") ? (!(string.Concat(cmbCtrl.SelectedItem) == this.disclosureManager.SentMethod[0]) ? (!(string.Concat(cmbCtrl.SelectedItem) == this.disclosureManager.SentMethod[1]) ? (!(string.Concat(cmbCtrl.SelectedItem) == this.disclosureManager.SentMethod[2]) ? (!(string.Concat(cmbCtrl.SelectedItem) == this.disclosureManager.SentMethod[3]) ? (!(string.Concat(cmbCtrl.SelectedItem) == this.disclosureManager.SentMethod[4]) ? DisclosureTrackingBase.DisclosedMethod.eDisclosure : DisclosureTrackingBase.DisclosedMethod.Other) : DisclosureTrackingBase.DisclosedMethod.Signature) : DisclosureTrackingBase.DisclosedMethod.Email) : DisclosureTrackingBase.DisclosedMethod.Phone) : DisclosureTrackingBase.DisclosedMethod.InPerson) : DisclosureTrackingBase.DisclosedMethod.None;
      if (cmbCtrl.Name == "cmbIntentReceivedMethod")
      {
        this.currentLog.IntentToProceed.ReceivedMethod = disclosedMethod;
        switch (disclosedMethod)
        {
          case DisclosureTrackingBase.DisclosedMethod.eDisclosure:
            this.currentLog.IntentToProceed.Date = DateTime.MinValue;
            break;
          case DisclosureTrackingBase.DisclosedMethod.Other:
            this.currentLog.IntentToProceed.ReceivedMethodOther = this.txtIntentMethodOther.Text;
            break;
        }
        if (this.currentLog.IntentToProceed.ReceivedMethod == DisclosureTrackingBase.DisclosedMethod.ByMail && this.disclosureManager.DisclosureTrackingLog.IsLocked)
        {
          DateTime closestBusinessDay = this.disclosureManager.GetNextClosestBusinessDay(this.dpIntentDate.Value);
          if (closestBusinessDay != this.dpIntentDate.Value)
          {
            this.ShowDeliverMessageDialog();
            this.dpIntentDate.Value = closestBusinessDay;
          }
        }
      }
      this.refreshUpdatedItem();
    }

    private void setDisclosedMethodValue(ComboBox cmbCtrl, string recipientId = "")
    {
      DisclosureTrackingBase.DisclosedMethod disclosedMethod = this.disclosureManager.GetDisclosedMethod(string.Concat(cmbCtrl.SelectedItem));
      switch (this.disclosureManager.DisclosureTrackingLog.DisclosureMethod)
      {
        case DisclosureTrackingBase.DisclosedMethod.eDisclosure:
          disclosedMethod = DisclosureTrackingBase.DisclosedMethod.eDisclosure;
          break;
        case DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder:
          disclosedMethod = DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder;
          break;
        case DisclosureTrackingBase.DisclosedMethod.eClose:
          disclosedMethod = DisclosureTrackingBase.DisclosedMethod.eClose;
          break;
      }
      switch (cmbCtrl.Name)
      {
        case "cmbSentMethod":
          this.disclosureManager.DisclosureTrackingLog.DisclosureMethod = disclosedMethod;
          if (disclosedMethod == DisclosureTrackingBase.DisclosedMethod.Other)
            this.disclosureManager.DisclosureTrackingLog.DisclosedMethodOther = this.txtMethodOther.Text;
          if (disclosedMethod == DisclosureTrackingBase.DisclosedMethod.Other || disclosedMethod == DisclosureTrackingBase.DisclosedMethod.eDisclosure || disclosedMethod == DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder || disclosedMethod == DisclosureTrackingBase.DisclosedMethod.eClose)
            this.disclosureManager.DisclosureTrackingLog.ReceivedDate = DateTime.MinValue;
          if (this.disclosureManager.DisclosureTrackingLog.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.ByMail && this.disclosureManager.DisclosureTrackingLog.IsLocked)
          {
            DateTime closestBusinessDay = this.disclosureManager.GetNextClosestBusinessDay(this.dpIntentDate.Value);
            if (closestBusinessDay != this.dpIntentDate.Value)
            {
              this.ShowDeliverMessageDialog();
              this.dpIntentDate.Value = closestBusinessDay;
              break;
            }
            break;
          }
          break;
        case "cmbIntentReceivedMethod":
          this.currentLog.IntentToProceed.ReceivedMethod = disclosedMethod;
          if (disclosedMethod == DisclosureTrackingBase.DisclosedMethod.Other)
            this.currentLog.IntentToProceed.ReceivedMethodOther = this.txtIntentMethodOther.Text;
          if (disclosedMethod == DisclosureTrackingBase.DisclosedMethod.Other || disclosedMethod == DisclosureTrackingBase.DisclosedMethod.eDisclosure || disclosedMethod == DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder || disclosedMethod == DisclosureTrackingBase.DisclosedMethod.eClose)
            this.currentLog.IntentToProceed.Date = DateTime.MinValue;
          if (this.currentLog.IntentToProceed.ReceivedMethod == DisclosureTrackingBase.DisclosedMethod.ByMail && this.currentLog.IntentToProceed.ReceivedBy.UseUserValue)
          {
            DateTime closestBusinessDay = this.disclosureManager.GetNextClosestBusinessDay(this.dpIntentDate.Value);
            if (closestBusinessDay != this.dpIntentDate.Value)
            {
              this.ShowDeliverMessageDialog();
              this.dpIntentDate.Value = closestBusinessDay;
              break;
            }
            break;
          }
          break;
        case "cmbRecipientReceivedMethod":
          if (disclosedMethod != DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder && disclosedMethod != DisclosureTrackingBase.DisclosedMethod.eClose && !string.IsNullOrEmpty(recipientId))
          {
            this.disclosureManager.RecipientItems[recipientId].DisclosedMethod = disclosedMethod;
            if (disclosedMethod == DisclosureTrackingBase.DisclosedMethod.Other)
            {
              this.disclosureManager.RecipientItems[recipientId].DisclosedMethodDescription = this.txtRecipientOther.Text;
              this.disclosureManager.RecipientItems[recipientId].ActualReceivedDate = DateTime.MinValue;
            }
            if (this.dpRecipientPresumedReceivedDate.Value != DateTime.MinValue)
            {
              DateTime closestBusinessDay = this.disclosureManager.GetNextClosestBusinessDay(this.dpRecipientPresumedReceivedDate.Value);
              if (closestBusinessDay != this.dpRecipientPresumedReceivedDate.Value)
              {
                int num = (int) Utils.Dialog((IWin32Window) this, "U.S. Postal Service does not deliver disclosures on Sundays and legal holidays. The date of the next available business day will be used.");
                this.dpRecipientPresumedReceivedDate.Value = closestBusinessDay;
                break;
              }
              break;
            }
            break;
          }
          break;
      }
      this.refreshUpdatedItem();
    }

    private void chkIntent_CheckedChanged(object sender, EventArgs e)
    {
      this.pnlIntent.Enabled = this.chkIntent.Checked;
      DateTimeWithZone dateTimeWithZone = ((EnhancedDisclosureTracking2015Log) this.disclosureManager.DisclosureTrackingLog).ConvertToDateTimeWithZone(DateTime.Now);
      if (this.chkIntent.Checked)
      {
        this.dpIntentDate.Value = dateTimeWithZone.DateTime;
        this.dpIntentDate.CustomFormat = "MM/dd/yyyy";
        this.lBtnIntentReceivedBy.Locked = false;
        this.txtIntentReceived.Text = Session.UserInfo.FullName + " (" + Session.UserInfo.Userid + ")";
        this.txtIntentReceived.Enabled = false;
        this.cmbIntentReceivedMethod.SelectedIndex = 0;
        this.txtIntentComments.Text = "";
      }
      else
      {
        if (!(this.dpIntentDate.Value == this.dpIntentDate.MinDate))
          return;
        this.dpIntentDate.CustomFormat = " ";
      }
    }

    private void chkReasonOther_CheckedChanged(object sender, EventArgs e)
    {
      this.txtReasonOther.Enabled = this.chkReasonOther.Checked && this.disclosureManager.IsReasonsEnabled;
      this.txtReasonOther.Text = this.chkReasonOther.Checked ? (this.disclosureManager.DisclosureTrackingLog.DisclosedForCD ? this.currentLog.ClosingDisclosure.OtherDescription : this.currentLog.LoanEstimate.OtherDescription) : "";
    }

    private void cmbBorrowerReceivedMethod_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (string.Concat(this.cmbRecipientReceivedMethod.SelectedItem) == this.disclosureManager.SentMethod[4])
      {
        this.txtRecipientOther.ReadOnly = false;
        this.txtRecipientOther.Text = ((EnhancedDisclosureTracking2015Log.DisclosureRecipient) this.ddRecipient.SelectedItem).DisclosedMethodDescription;
      }
      else if (string.Concat(this.cmbRecipientReceivedMethod.SelectedItem) == this.disclosureManager.SentMethod[5])
      {
        this.txtRecipientOther.ReadOnly = true;
        this.txtRecipientOther.Text = this.disclosureManager.DisclosureTrackingLog.BorrowerFulfillmentMethodDescription;
      }
      else
      {
        this.txtRecipientOther.ReadOnly = true;
        this.txtRecipientOther.Text = "";
        if (string.Concat(this.cmbRecipientReceivedMethod.SelectedItem) != this.disclosureManager.SentMethod[0])
          return;
        this.dpRecipientActualReceivedDate.Value = this.dtSentDate.Value;
        this.dpReceivedDate_ValueChanged((object) null, (EventArgs) null);
      }
    }

    private void cmbIntentReceivedMethod_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (string.Concat(this.cmbIntentReceivedMethod.SelectedItem) == this.disclosureManager.SentMethod[4])
      {
        this.txtIntentMethodOther.Enabled = true;
        this.txtIntentMethodOther.Text = this.currentLog.IntentToProceed.ReceivedMethodOther;
      }
      else
      {
        this.txtIntentMethodOther.Enabled = false;
        this.txtIntentMethodOther.Text = "";
      }
    }

    private void txt_KeyUp(object sender, KeyEventArgs e)
    {
      if (!e.Control || e.KeyCode != Keys.A || !(sender is TextBox textBox))
        return;
      textBox.SelectAll();
    }

    private void lBtnIntentReceivedBy_Click(object sender, EventArgs e)
    {
      this.lBtnIntentReceivedBy.Locked = !this.lBtnIntentReceivedBy.Locked;
      if (!this.lBtnIntentReceivedBy.Locked)
      {
        if (!string.IsNullOrEmpty(this.currentLog.IntentToProceed.ReceivedBy.UserValue))
          this.txtIntentReceived.Text = this.currentLog.IntentToProceed.ReceivedBy.UserValue;
        else
          this.txtIntentReceived.Text = Session.UserInfo.FullName + "(" + Session.UserInfo.Userid + ")";
        this.txtIntentReceived.Enabled = false;
        this.refreshUpdatedItem();
      }
      else
      {
        this.txtIntentReceived.Text = this.currentLog.IntentToProceed.ReceivedBy.ComputedValue;
        this.txtIntentReceived.Enabled = true;
      }
    }

    private void ShowDeliverMessageDialog()
    {
      int num = (int) Utils.Dialog((IWin32Window) this, "U.S. Postal Service does not deliver disclosures on Sundays and legal holidays. The date of the next available business day will be used.");
    }

    private void dtDisclosedDate_ValueChanged(object sender, EventArgs e)
    {
      DateTime closestBusinessDay = this.disclosureManager.GetNextClosestBusinessDay(this.dtSentDate.Value);
      if (closestBusinessDay != this.dtSentDate.Value && this.cmbSentMethod.Text == this.disclosureManager.DiscloseMethod[0])
      {
        this.ShowDeliverMessageDialog();
        this.dtSentDate.Value = closestBusinessDay;
      }
      if (this.cmbSentMethod.Text == this.disclosureManager.SentMethod[0] || this.cmbSentMethod.Text == "" && this.txtSentMethod.Text == this.disclosureManager.SentMethod[5])
      {
        if (this.disclosureManager.DisclosureTrackingLog.DisclosureMethod != DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder && this.disclosureManager.DisclosureTrackingLog.DisclosureMethod != DisclosureTrackingBase.DisclosedMethod.eClose && !this.lBtnRecipientPresumed.Locked)
          this.dpRecipientPresumedReceivedDate.Enabled = false;
        if (this.cmbSentMethod.Text == "" && this.txtSentMethod.Text == this.disclosureManager.SentMethod[5])
        {
          Dictionary<string, object> disclosureReceivedDate = this.disclosureManager.CalculateNew2015DisclosureReceivedDate(this.dtSentDate.Value, this.dpPresumedFulfillmentDate.Value, this.dpActualFulfillmentDate.Value, this.disclosureManager.GetDisclosedMethod(this.txtFulfillmentMethod.Text ?? ""));
          if (this.disclosureManager.RecipientItems[this.SelectedRecipientId].Role == EnhancedDisclosureTracking2015Log.DisclosureRecipientType.Borrower)
            this.ShowControlsBasedonDisclosedMethod((DisclosureTrackingBase.DisclosedMethod) disclosureReceivedDate["BorrowerDisclosedMethod"]);
          else if (this.disclosureManager.RecipientItems[this.SelectedRecipientId].Role == EnhancedDisclosureTracking2015Log.DisclosureRecipientType.CoBorrower)
            this.ShowControlsBasedonDisclosedMethod((DisclosureTrackingBase.DisclosedMethod) disclosureReceivedDate["CoBorrowerDisclosedMethod"]);
        }
      }
      else if (this.disclosureManager.DisclosureTrackingLog.DisclosedForLE || this.disclosureManager.DisclosureTrackingLog.DisclosedForCD)
      {
        if (this.dtSentDate.Value >= new DateTime(2029, 12, 29))
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Based on the Sent Date, the Presumed Received Date is not supported by your company's Compliance Calendar. Dates must occur within the date range provided by the Compliance Calendar." + Environment.NewLine + "For information about the Compliance Calendar contact your system administrator.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          this.dtSentDate.Value = this.disclosureManager.DisclosureTrackingLog.DisclosedDate;
        }
        if (this.disclosureManager.DisclosureTrackingLog.DisclosureMethod != DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder && this.disclosureManager.DisclosureTrackingLog.DisclosureMethod != DisclosureTrackingBase.DisclosedMethod.eClose && !this.lBtnRecipientPresumed.Locked)
        {
          this.dpRecipientPresumedReceivedDate.Value = this.disclosureManager.GetReceivedDateByAdding3BusinessDays(this.dtSentDate.Value);
          foreach (KeyValuePair<string, EnhancedDisclosureTracking2015Log.DisclosureRecipient> recipientItem in this.disclosureManager.RecipientItems)
            recipientItem.Value.PresumedReceivedDate.ComputedValue = this.disclosureManager.GetReceivedDateByAdding3BusinessDays(this.dtSentDate.Value);
        }
      }
      this.dpRecipientPresumedReceivedDate.MinValue = this.dtSentDate.Value;
      if (this.disclosureManager.RecipientItems[this.SelectedRecipientId].Role != EnhancedDisclosureTracking2015Log.DisclosureRecipientType.Borrower)
        return;
      this.dpReceivedDate_ValueChanged(sender, e);
    }

    private void dpChangesRecievedDate_ValueChanged(object sender, EventArgs e)
    {
      DateTime date = this.dpChangesRecievedDate.Value;
      if (date == DateTime.MinValue)
      {
        this.dpRevisedDueDate.Value = DateTime.MinValue;
      }
      else
      {
        try
        {
          bool complianceSetting = (bool) this.disclosureManager.LoanData.Settings.ComplianceSettings[(object) "Compliance.CountDay0ForRediscloseDueDate"];
          this.dpRevisedDueDate.Value = Session.ConfigurationManager.AddBusinessDays(CalendarType.Business, date, complianceSetting ? 2 : 3, false);
        }
        catch (Exception ex)
        {
          if (!(ex.InnerException is ComplianceCalendarException))
            throw ex;
          this.dpRevisedDueDate.Value = DateTime.MinValue;
          throw new ComplianceCalendarException((ComplianceCalendarException) ex.InnerException, "3167");
        }
      }
    }

    private void btnCDSnapshot_Click(object sender, EventArgs e)
    {
      int num = (int) new DisclosedCDDialog(this.disclosureManager.DisclosureTrackingLog).ShowDialog((IWin32Window) this);
    }

    private void btnItemSnapshot_Click(object sender, EventArgs e)
    {
      int num = (int) new DisclosedItemizationDialog(this.disclosureManager.DisclosureTrackingLog).ShowDialog((IWin32Window) this);
    }

    private void dpReceivedDate_ValueChanged(object sender, EventArgs e)
    {
      this.refreshIntentToProceed(this.disclosureManager.RecipientItems[this.SelectedRecipientId].PresumedReceivedDate.Value, this.disclosureManager.RecipientItems[this.SelectedRecipientId].ActualReceivedDate);
      this.disclosureManager.BorrowerReceivedDate = this.dpRecipientPresumedReceivedDate.Value;
      this.disclosureManager.BorrowerActualReceivedDate = this.dpRecipientActualReceivedDate.Value;
    }

    private void dtDisclosedDate_DropDown(object sender, EventArgs e)
    {
      this.dtSentDate.ValueChanged -= new EventHandler(this.dtDisclosedDate_ValueChanged);
    }

    private void dtDisclosedDate_CloseUp(object sender, EventArgs e)
    {
      this.dtSentDate.ValueChanged += new EventHandler(this.dtDisclosedDate_ValueChanged);
      this.dtDisclosedDate_ValueChanged((object) null, (EventArgs) null);
    }

    private void dpBorrowerReceivedDate_KeyPress(object sender, KeyPressEventArgs e)
    {
      this.refreshIntentToProceed(this.disclosureManager.RecipientItems[this.SelectedRecipientId].PresumedReceivedDate.Value, this.disclosureManager.RecipientItems[this.SelectedRecipientId].ActualReceivedDate);
      this.disclosureManager.BorrowerReceivedDate = this.dpRecipientPresumedReceivedDate.Value;
      this.disclosureManager.BorrowerActualReceivedDate = this.dpRecipientActualReceivedDate.Value;
    }

    private void btnProviderListSnapshot_Click(object sender, EventArgs e)
    {
      int num = (int) new DisclosedSSPLDialog(this.disclosureManager.DisclosureTrackingLog).ShowDialog((IWin32Window) this);
    }

    private void cmbDisclosureType_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.cmbDisclosureType.Text != "Post Consummation" || this.disclosureManager.DisclosureTrackingLog.DisclosureType != DisclosureTracking2015Log.DisclosureTypeEnum.Initial)
        return;
      int num = (int) MessageBox.Show("Initial Disclosure cannot be set to Post Consummation.", "Encompass", MessageBoxButtons.OK);
      this.cmbDisclosureType.SelectedIndex = 0;
    }

    private void calcToPopulateControls()
    {
      Dictionary<string, object> disclosureReceivedDate = this.disclosureManager.CalculateNew2015DisclosureReceivedDate(this.dtSentDate.Value, this.dpPresumedFulfillmentDate.Value, this.dpActualFulfillmentDate.Value, this.disclosureManager.GetDisclosedMethod(this.txtFulfillmentMethod.Text ?? ""));
      if (this.disclosureManager.RecipientItems[this.SelectedRecipientId].Role == EnhancedDisclosureTracking2015Log.DisclosureRecipientType.Borrower)
        this.ShowControlsBasedonDisclosedMethod((DisclosureTrackingBase.DisclosedMethod) disclosureReceivedDate["BorrowerDisclosedMethod"]);
      else if (this.disclosureManager.RecipientItems[this.SelectedRecipientId].Role == EnhancedDisclosureTracking2015Log.DisclosureRecipientType.CoBorrower)
        this.ShowControlsBasedonDisclosedMethod((DisclosureTrackingBase.DisclosedMethod) disclosureReceivedDate["CoBorrowerDisclosedMethod"]);
      this.SetRecipientControls((IReadOnlyDictionary<string, object>) disclosureReceivedDate);
    }

    private void ddRecipient_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.SelectedRecipientId != null)
        this.SetRecipientInMemory(this.SelectedRecipientId);
      this.SelectedRecipientId = ((EnhancedDisclosureTracking2015Log.DisclosureRecipient) this.ddRecipient.SelectedItem).Id;
      this.PopulateSelectedRecipientDetailsUi();
      this.ClearActualReceivedDate();
      this.RefreshFulfillmentFields();
    }

    private void SetRecipientInMemory(string recipientId)
    {
      this.disclosureManager.RecipientItems[recipientId].BorrowerType.UseUserValue = this.lBtnRecipientType.Locked;
      if (this.lBtnRecipientType.Locked)
        this.disclosureManager.RecipientItems[recipientId].BorrowerType.UserValue = this.cmbRecipientType.SelectedItem == null ? "" : this.cmbRecipientType.SelectedItem.ToString();
      this.setDisclosedMethodValue(this.cmbRecipientReceivedMethod, recipientId);
      this.disclosureManager.RecipientItems[recipientId].ActualReceivedDate = Utils.ParseDate((object) this.dpRecipientActualReceivedDate.Text);
      this.disclosureManager.RecipientItems[recipientId].PresumedReceivedDate.UseUserValue = this.lBtnRecipientPresumed.Locked;
      if (this.disclosureManager.RecipientItems[recipientId].PresumedReceivedDate.UseUserValue)
        this.disclosureManager.RecipientItems[recipientId].PresumedReceivedDate.UserValue = this.dpRecipientPresumedReceivedDate.Value;
      else
        this.disclosureManager.RecipientItems[recipientId].PresumedReceivedDate.ComputedValue = this.dpRecipientPresumedReceivedDate.Value;
    }

    private void populateRecipientDetails()
    {
      this.setSelectedDisclosedMethod(this.cmbRecipientReceivedMethod, this.disclosureManager.RecipientItems[this.SelectedRecipientId].DisclosedMethod);
      this.cmbRecipientReceivedMethod.BringToFront();
      string str = string.Empty;
      if (this.disclosureManager.RecipientItems[this.SelectedRecipientId].Role == EnhancedDisclosureTracking2015Log.DisclosureRecipientType.Borrower)
        str = this.disclosureManager.DisclosureTrackingLog.BorrowerFulfillmentMethodDescription;
      else if (this.disclosureManager.RecipientItems[this.SelectedRecipientId].Role == EnhancedDisclosureTracking2015Log.DisclosureRecipientType.CoBorrower)
        str = this.disclosureManager.DisclosureTrackingLog.CoBorrowerFulfillmentMethodDescription;
      switch (this.disclosureManager.RecipientItems[this.SelectedRecipientId].DisclosedMethod)
      {
        case DisclosureTrackingBase.DisclosedMethod.ByMail:
        case DisclosureTrackingBase.DisclosedMethod.InPerson:
          if (this.disclosureManager.RecipientItems[this.SelectedRecipientId].DisclosedMethod == DisclosureTrackingBase.DisclosedMethod.eDisclosure)
          {
            this.txtRecipientOther.Text = str;
            break;
          }
          break;
        case DisclosureTrackingBase.DisclosedMethod.eDisclosure:
          this.txtRecipientReceivedMethod.Visible = true;
          this.cmbRecipientReceivedMethod.Visible = false;
          this.txtRecipientReceivedMethod.BringToFront();
          this.txtRecipientReceivedMethod.Text = this.disclosureManager.DiscloseMethod[3];
          this.txtRecipientOther.Text = str;
          this.txtRecipientOther.ReadOnly = true;
          break;
        case DisclosureTrackingBase.DisclosedMethod.Other:
          this.txtRecipientOther.Text = this.disclosureManager.RecipientItems[this.SelectedRecipientId].DisclosedMethodDescription;
          break;
        case DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder:
        case DisclosureTrackingBase.DisclosedMethod.eClose:
          this.cmbRecipientReceivedMethod.SelectedIndex = -1;
          this.txtRecipientOther.ReadOnly = true;
          break;
      }
      if (this.disclosureManager.RecipientItems[this.SelectedRecipientId].DisclosedMethod == DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder || this.disclosureManager.RecipientItems[this.SelectedRecipientId].DisclosedMethod == DisclosureTrackingBase.DisclosedMethod.eClose)
        this.cmbRecipientReceivedMethod.SelectedIndex = -1;
      this.lBtnRecipientPresumed.Locked = this.disclosureManager.RecipientItems[this.SelectedRecipientId].PresumedReceivedDate.UseUserValue;
      this.lBtnRecipientPresumed.BringToFront();
      switch (this.disclosureManager.RecipientItems[this.SelectedRecipientId].Role.ToString())
      {
        case "CoBorrower":
          this.lblSigner.Text = "Co-Borrower";
          break;
        case "NonBorrowingOwner":
          this.lblSigner.Text = "Non-Borrowing Owner";
          break;
        default:
          this.lblSigner.Text = this.disclosureManager.RecipientItems[this.SelectedRecipientId].Role.ToString();
          break;
      }
      if (this.lBtnRecipientPresumed.Locked)
      {
        this.dpRecipientPresumedReceivedDate.Value = this.disclosureManager.RecipientItems[this.SelectedRecipientId].PresumedReceivedDate.UserValue;
        this.dpRecipientPresumedReceivedDate.BringToFront();
      }
      else
      {
        if (this.disclosureManager.RecipientItems[this.SelectedRecipientId].DisclosedMethod != DisclosureTrackingBase.DisclosedMethod.InPerson && this.disclosureManager.RecipientItems[this.SelectedRecipientId].DisclosedMethod != DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder && this.disclosureManager.RecipientItems[this.SelectedRecipientId].DisclosedMethod != DisclosureTrackingBase.DisclosedMethod.eClose)
          this.dpRecipientPresumedReceivedDate.Value = this.disclosureManager.RecipientItems[this.SelectedRecipientId].PresumedReceivedDate.ComputedValue;
        this.dpRecipientPresumedReceivedDate.BringToFront();
      }
      this.dpRecipientPresumedReceivedDate.ReadOnly = !this.lBtnRecipientPresumed.Locked;
      this.dpRecipientPresumedReceivedDate.Enabled = this.lBtnRecipientPresumed.Locked;
      this.dpRecipientActualReceivedDate.Value = this.disclosureManager.RecipientItems[this.SelectedRecipientId].ActualReceivedDate;
      this.txtRecipientType.Text = this.disclosureManager.RecipientItems[this.SelectedRecipientId].BorrowerType.ComputedValue;
      this.txtRecipientType.BringToFront();
      this.populateRecipientTypes(this.disclosureManager.RecipientItems[this.SelectedRecipientId].Role);
      this.pnlUserId.Visible = this.lBtnRecipientType.Visible = false;
      if (this.disclosureManager.RecipientItems[this.SelectedRecipientId].Role == EnhancedDisclosureTracking2015Log.DisclosureRecipientType.Other)
        this.txtRecipientType.Text = this.disclosureManager.RecipientItems[this.SelectedRecipientId].RoleDescription;
      else if (this.disclosureManager.RecipientItems[this.SelectedRecipientId].Role == EnhancedDisclosureTracking2015Log.DisclosureRecipientType.LoanAssociate)
      {
        this.pnlUserId.Visible = true;
        this.txtRecipientType.Text = this.disclosureManager.RecipientItems[this.SelectedRecipientId].RoleDescription;
        this.txtUserID.Text = this.disclosureManager.RecipientItems[this.SelectedRecipientId].UserId;
      }
      else
      {
        this.lBtnRecipientType.Visible = true;
        this.cmbRecipientType.Visible = this.disclosureManager.RecipientItems[this.SelectedRecipientId].BorrowerType.UseUserValue;
        this.txtRecipientType.Visible = !this.disclosureManager.RecipientItems[this.SelectedRecipientId].BorrowerType.UseUserValue;
        this.lBtnRecipientType.Locked = this.disclosureManager.RecipientItems[this.SelectedRecipientId].BorrowerType.UseUserValue;
        if (this.lBtnRecipientType.Locked)
          this.cmbRecipientType.Text = this.disclosureManager.RecipientItems[this.SelectedRecipientId].BorrowerType.UserValue;
        else
          this.txtRecipientType.Text = this.disclosureManager.RecipientItems[this.SelectedRecipientId].BorrowerType.ComputedValue;
      }
    }

    private void lBtnRecipientPresumed_Click(object sender, EventArgs e)
    {
      this.lBtnRecipientPresumed.Locked = !this.lBtnRecipientPresumed.Locked;
      if (!this.lBtnRecipientPresumed.Locked)
      {
        this.dpRecipientPresumedReceivedDate.Value = this.disclosureManager.RecipientItems[this.SelectedRecipientId].PresumedReceivedDate.ComputedValue;
        this.dpRecipientPresumedReceivedDate.Enabled = false;
        this.dpRecipientPresumedReceivedDate.ReadOnly = true;
        this.refreshUpdatedItem();
        if (this.disclosureManager.RecipientItems[this.SelectedRecipientId].Role == EnhancedDisclosureTracking2015Log.DisclosureRecipientType.NonBorrowingOwner)
          this.disclosureManager.UpdateNboRecievedDates(this.dtSentDate.Value);
      }
      else
      {
        this.dpRecipientPresumedReceivedDate.Value = this.disclosureManager.RecipientItems[this.SelectedRecipientId].PresumedReceivedDate.UserValue;
        this.dpRecipientPresumedReceivedDate.Enabled = true;
        this.dpRecipientPresumedReceivedDate.ReadOnly = false;
      }
      if (this.disclosureManager.RecipientItems[this.SelectedRecipientId].Role != EnhancedDisclosureTracking2015Log.DisclosureRecipientType.Borrower)
        return;
      this.dpReceivedDate_ValueChanged((object) null, (EventArgs) null);
    }

    private void lBtnRecipientType_Click(object sender, EventArgs e)
    {
      this.lBtnRecipientType.Locked = !this.lBtnRecipientType.Locked;
      EnhancedDisclosureTracking2015Log.DisclosureRecipient recipientItem = this.disclosureManager.RecipientItems[this.SelectedRecipientId];
      if (this.lBtnRecipientType.Locked)
      {
        if (recipientItem.BorrowerType.UserValue != null)
        {
          this.cmbRecipientType.SelectedIndex = this.cmbRecipientType.Items.IndexOf((object) recipientItem.BorrowerType.UserValue);
        }
        else
        {
          this.cmbRecipientType.SelectedIndex = 0;
          if (recipientItem.Role == EnhancedDisclosureTracking2015Log.DisclosureRecipientType.NonBorrowingOwner)
          {
            string lower = this.txtRecipientType.Text.ToLower();
            if (lower == "title only" || lower == "non title spouse" || lower == "title only trustee")
              this.cmbRecipientType.SelectedItem = (object) this.txtRecipientType.Text;
            else
              this.cmbRecipientType.SelectedIndex = 3;
          }
        }
        this.cmbRecipientType.Visible = true;
        this.txtRecipientType.Visible = false;
        this.cmbRecipientType.BringToFront();
      }
      else
      {
        this.txtRecipientType.Text = recipientItem.BorrowerType.ComputedValue != null ? this.disclosureManager.RecipientItems[this.SelectedRecipientId].BorrowerType.ComputedValue : string.Empty;
        this.cmbRecipientType.Visible = false;
        this.txtRecipientType.Visible = true;
        this.txtRecipientType.BringToFront();
      }
    }

    private void populateTrackingGridDetails()
    {
      this.grdDisclosureTracking.Items.Clear();
      this.grdDisclosureTracking.Columns[1].Text = this.disclosureManager.RecipientItems[this.SelectedRecipientId].Role.ToString();
      this.addDisclosureGridItem("Recipient", this.disclosureManager.RecipientItems[this.SelectedRecipientId].Name);
      this.addDisclosureGridItem("Email Address", this.disclosureManager.RecipientItems[this.SelectedRecipientId].Email);
      this.addDisclosureGridItem("Consent when eDisclosure was sent", this.disclosureManager.RecipientItems[this.SelectedRecipientId].Tracking.LoanLevelConsent);
      this.addDisclosureGridItem("Message Viewed", this.disclosureManager.RecipientItems[this.SelectedRecipientId].Tracking.ViewMessageDate.DateTime);
      this.addDisclosureGridItem("Package Consent Form Accepted", this.disclosureManager.RecipientItems[this.SelectedRecipientId].Tracking.AcceptConsentDate.DateTime);
      this.addDisclosureGridItem("Package Consent Form Accepted from IP Address", this.disclosureManager.RecipientItems[this.SelectedRecipientId].Tracking.AcceptConsentIP);
      this.addDisclosureGridItem("Package Consent Form Rejected", this.disclosureManager.RecipientItems[this.SelectedRecipientId].Tracking.RejectConsentDate.DateTime);
      this.addDisclosureGridItem("Package Consent Form Rejected from IP Address", this.disclosureManager.RecipientItems[this.SelectedRecipientId].Tracking.RejectConsentIP);
      this.addDisclosureGridItem("Authenticated", this.disclosureManager.RecipientItems[this.SelectedRecipientId].Tracking.AuthenticatedDate.DateTime);
      this.addDisclosureGridItem("Authenticated from IP Address", this.disclosureManager.RecipientItems[this.SelectedRecipientId].Tracking.AuthenticatedIP);
      this.addDisclosureGridItem("eSigned Viewed Date", this.disclosureManager.RecipientItems[this.SelectedRecipientId].Tracking.ViewESignedDate.DateTime);
      this.addDisclosureGridItem("WetSigned Viewed Date", this.disclosureManager.RecipientItems[this.SelectedRecipientId].Tracking.ViewWetSignedDate.DateTime);
      this.addDisclosureGridItem("eSigned Disclosures", this.disclosureManager.RecipientItems[this.SelectedRecipientId].Tracking.ESignedDate.DateTime);
      this.addDisclosureGridItem("eSigned Disclosures from IP Address", this.disclosureManager.RecipientItems[this.SelectedRecipientId].Tracking.ESignedIP);
      this.addDisclosureGridItem("Informational Viewed Date", this.disclosureManager.RecipientItems[this.SelectedRecipientId].Tracking.InformationalViewedDate.DateTime);
      this.addDisclosureGridItem("Informational Viewed from IP Address", this.disclosureManager.RecipientItems[this.SelectedRecipientId].Tracking.InformationalViewedIP);
      this.addDisclosureGridItem("Informational Completed Date", this.disclosureManager.RecipientItems[this.SelectedRecipientId].Tracking.InformationalCompletedDate.DateTime);
      this.addDisclosureGridItem("Informational Completed from IP Address", this.disclosureManager.RecipientItems[this.SelectedRecipientId].Tracking.InformationalCompletedIP);
    }

    private void PopulateSelectedRecipientDetailsUi()
    {
      this.populateTrackingGridDetails();
      this.populateRecipientDetails();
      this.panel1.Enabled = this.disclosureManager.RecipientItems[this.SelectedRecipientId].Role != EnhancedDisclosureTracking2015Log.DisclosureRecipientType.NonBorrowingOwner;
    }

    private void populateRecipientTypes(
      EnhancedDisclosureTracking2015Log.DisclosureRecipientType selectedRecipientRole)
    {
      this.cmbRecipientType.Visible = this.txtRecipientType.Enabled = this.lBtnRecipientType.Enabled = true;
      switch (selectedRecipientRole)
      {
        case EnhancedDisclosureTracking2015Log.DisclosureRecipientType.Borrower:
        case EnhancedDisclosureTracking2015Log.DisclosureRecipientType.CoBorrower:
          this.cmbRecipientType.DataSource = (object) this.disclosureManager.BorrowerType;
          break;
        case EnhancedDisclosureTracking2015Log.DisclosureRecipientType.NonBorrowingOwner:
          this.cmbRecipientType.DataSource = (object) this.disclosureManager.NboType;
          break;
        default:
          this.cmbRecipientType.DataSource = (object) null;
          this.cmbRecipientType.Items.Clear();
          this.txtRecipientType.Visible = true;
          this.cmbRecipientType.Visible = this.txtRecipientType.Enabled = this.lBtnRecipientType.Enabled = false;
          this.txtRecipientType.Text = string.Empty;
          break;
      }
    }

    private void lblViewSummary_Click(object sender, EventArgs e)
    {
      if (this.disclosureManager.DisclosureTrackingLog == null)
        return;
      int num = (int) new LogSummary(this.disclosureManager).ShowDialog((IWin32Window) this);
    }

    private void lblViewSummary_MouseEnter(object sender, EventArgs e)
    {
      this.Cursor = Cursors.Hand;
    }

    private void lblViewSummary_MouseLeave(object sender, EventArgs e)
    {
      this.Cursor = Cursors.Default;
    }

    private void btnAuditTrail_Click(object sender, EventArgs e)
    {
      EllieMae.EMLite.Log.AuditTrail.AuditTrail.ViewAuditTrail(Session.LoanDataMgr.LoanData.GUID, this.currentLog.Tracking.PackageId);
    }

    private void setSelectedDisclosedMethod(
      ComboBox cmbCtrl,
      DisclosureTrackingBase.DisclosedMethod method)
    {
      cmbCtrl.SelectedIndex = -1;
      switch (method)
      {
        case DisclosureTrackingBase.DisclosedMethod.None:
          cmbCtrl.SelectedIndex = 0;
          break;
        case DisclosureTrackingBase.DisclosedMethod.ByMail:
          cmbCtrl.SelectedIndex = cmbCtrl.Items.IndexOf((object) this.disclosureManager.DiscloseMethod[0]);
          break;
        case DisclosureTrackingBase.DisclosedMethod.eDisclosure:
          cmbCtrl.SelectedIndex = cmbCtrl.Items.IndexOf((object) this.disclosureManager.DiscloseMethod[3]);
          break;
        case DisclosureTrackingBase.DisclosedMethod.Fax:
          cmbCtrl.SelectedIndex = cmbCtrl.Items.IndexOf((object) this.disclosureManager.DiscloseMethod[4]);
          break;
        case DisclosureTrackingBase.DisclosedMethod.InPerson:
          cmbCtrl.SelectedIndex = cmbCtrl.Items.IndexOf((object) this.disclosureManager.DiscloseMethod[1]);
          break;
        case DisclosureTrackingBase.DisclosedMethod.Other:
          cmbCtrl.SelectedIndex = cmbCtrl.Items.IndexOf((object) this.disclosureManager.DiscloseMethod[5]);
          break;
        case DisclosureTrackingBase.DisclosedMethod.Email:
          cmbCtrl.SelectedIndex = cmbCtrl.Items.IndexOf((object) this.disclosureManager.DiscloseMethod[2]);
          break;
        case DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder:
          cmbCtrl.SelectedIndex = cmbCtrl.Items.IndexOf((object) this.disclosureManager.DiscloseMethod[6]);
          break;
        case DisclosureTrackingBase.DisclosedMethod.eClose:
          cmbCtrl.SelectedIndex = cmbCtrl.Items.IndexOf((object) this.disclosureManager.DiscloseMethod[7]);
          break;
      }
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
      GVColumn gvColumn7 = new GVColumn();
      GVColumn gvColumn8 = new GVColumn();
      GVColumn gvColumn9 = new GVColumn();
      GVColumn gvColumn10 = new GVColumn();
      this.tcDisclosure = new TabControl();
      this.tabPageDetail = new TabPage();
      this.detailsPanel = new Panel();
      this.pnlLoanSnapshot = new BorderPanel();
      this.lbtnDisclosedDailyInterest = new FieldLockButton();
      this.txtDisclosedDailyInterest = new TextBox();
      this.label1 = new Label();
      this.lbtnFinanceCharge = new FieldLockButton();
      this.lbtnDisclosedAPR = new FieldLockButton();
      this.gcDocList = new GroupContainer();
      this.btnViewDisclosure = new Button();
      this.gvDocList = new GridView();
      this.txtApplicationDate = new TextBox();
      this.label42 = new Label();
      this.txtFinanceCharge = new TextBox();
      this.txtLoanProgram = new TextBox();
      this.txtLoanAmount = new TextBox();
      this.txtDisclosedAPR = new TextBox();
      this.label38 = new Label();
      this.label39 = new Label();
      this.label40 = new Label();
      this.label41 = new Label();
      this.txtPropertyZip = new TextBox();
      this.txtPropertyState = new TextBox();
      this.txtPropertyCity = new TextBox();
      this.txtPropertyAddress = new TextBox();
      this.label37 = new Label();
      this.label36 = new Label();
      this.label35 = new Label();
      this.label34 = new Label();
      this.gradientPanel3 = new GradientPanel();
      this.flowLayoutPanel2 = new FlowLayoutPanel();
      this.btnProviderListSnapshot = new Button();
      this.btnSafeHarborSnapshot = new Button();
      this.btnItemSnapshot = new Button();
      this.btnCDSnapshot = new Button();
      this.btnLESnapshot = new Button();
      this.label20 = new Label();
      this.pnlBasicInfo = new BorderPanel();
      this.tableLayoutPanel1 = new TableLayoutPanel();
      this.panel1 = new Panel();
      this.chkUseforUCDExport = new CheckBox();
      this.lblUseforUCDExport = new Label();
      this.txtSource = new TextBox();
      this.txtProviderDescription = new TextBox();
      this.lblProvider = new Label();
      this.lbtnSentDate = new FieldLockButton();
      this.txtMethodOther = new TextBox();
      this.cmbDisclosureType = new ComboBox();
      this.lblDisclosureType = new Label();
      this.txtDisclosedBy = new TextBox();
      this.chkIntent = new CheckBox();
      this.lbtnDisclosedBy = new FieldLockButton();
      this.txtSentMethod = new TextBox();
      this.dtSentDate = new DateTimePicker();
      this.cmbSentMethod = new ComboBox();
      this.lblBy = new Label();
      this.bblSentMethod = new Label();
      this.lblSentDate = new Label();
      this.pnlIntent = new Panel();
      this.txtIntentMethodOther = new TextBox();
      this.txtIntentToProceedMethod = new TextBox();
      this.txtIntentComments = new TextBox();
      this.label8 = new Label();
      this.cmbIntentReceivedMethod = new ComboBox();
      this.label5 = new Label();
      this.txtIntentReceived = new TextBox();
      this.dpIntentDate = new DateTimePicker();
      this.label6 = new Label();
      this.label7 = new Label();
      this.lBtnIntentReceivedBy = new FieldLockButton();
      this.panel2 = new Panel();
      this.pnlUserId = new Panel();
      this.lblUserID = new Label();
      this.txtUserID = new TextBox();
      this.lBtnRecipientType = new FieldLockButton();
      this.dpRecipientPresumedReceivedDate = new DatePicker();
      this.txtRecipientReceivedMethod = new TextBox();
      this.lBtnRecipientPresumed = new FieldLockButton();
      this.cmbRecipientType = new ComboBox();
      this.dpRecipientActualReceivedDate = new DatePicker();
      this.txtRecipientType = new TextBox();
      this.txtRecipientOther = new TextBox();
      this.label12 = new Label();
      this.lblSigner = new Label();
      this.label4 = new Label();
      this.cmbRecipientReceivedMethod = new ComboBox();
      this.label3 = new Label();
      this.lblDisclosurePresumedReceivedDate = new Label();
      this.gradientPanel1 = new GradientPanel();
      this.btnEsign = new Button();
      this.label46 = new Label();
      this.label44 = new Label();
      this.chkLEDisclosedByBroker = new CheckBox();
      this.lblDisclosureInfo = new Label();
      this.tabPageReasons = new TabPage();
      this.reasonsPanel = new Panel();
      this.borderPanel1 = new BorderPanel();
      this.panel5 = new Panel();
      this.feeDetailsLabel = new Label();
      this.feeDetailsGV = new GridView();
      this.txtChangedCircumstance = new TextBox();
      this.txtCiCComment = new TextBox();
      this.label25 = new Label();
      this.btnSelect = new StandardIconButton();
      this.label10 = new Label();
      this.pnlReason = new Panel();
      this.changedCircumstancesChkBx = new CheckBox();
      this.feeLevelIndicatorChkBx = new CheckBox();
      this.lbRevisedDueDate = new Label();
      this.lbChangesRecievedDate = new Label();
      this.dpRevisedDueDate = new DatePicker();
      this.dpChangesRecievedDate = new DatePicker();
      this.chkReason10 = new CheckBox();
      this.chkReason9 = new CheckBox();
      this.chkReason8 = new CheckBox();
      this.chkReason7 = new CheckBox();
      this.txtReasonOther = new TextBox();
      this.chkReasonOther = new CheckBox();
      this.chkReason6 = new CheckBox();
      this.chkReason5 = new CheckBox();
      this.chkReason4 = new CheckBox();
      this.chkReason3 = new CheckBox();
      this.chkReason2 = new CheckBox();
      this.chkReason1 = new CheckBox();
      this.label9 = new Label();
      this.gradientPanel5 = new GradientPanel();
      this.label26 = new Label();
      this.tabPageeDisclosure = new TabPage();
      this.pnleDisclosureStatus = new BorderPanel();
      this.grdDisclosureTracking = new GridView();
      this.lblePackageId = new Label();
      this.label45 = new Label();
      this.lblDateeDisclosureSent = new Label();
      this.lblViewForm = new Label();
      this.llViewDetails = new Label();
      this.label19 = new Label();
      this.gradientPanel2 = new GradientPanel();
      this.label14 = new Label();
      this.pnlFulfillment = new BorderPanel();
      this.dpActualFulfillmentDate = new DatePicker();
      this.dpPresumedFulfillmentDate = new DatePicker();
      this.label18 = new Label();
      this.label27 = new Label();
      this.txtFulfillmentComments = new TextBox();
      this.label43 = new Label();
      this.txtFulfillmentMethod = new TextBox();
      this.label31 = new Label();
      this.txtDateFulfillOrder = new TextBox();
      this.txtFulfillmentOrderBy = new TextBox();
      this.label30 = new Label();
      this.label29 = new Label();
      this.gradientPanel4 = new GradientPanel();
      this.btneDiscManualFulfill = new Button();
      this.label28 = new Label();
      this.ddRecipient = new ComboBox();
      this.detailsDisclosureRecipientLbl = new Label();
      this.btnOK = new Button();
      this.tableLayoutPanel2 = new TableLayoutPanel();
      this.panel3 = new Panel();
      this.btnAuditTrail = new Button();
      this.lblViewSummary = new Label();
      this.btnClose = new Button();
      this.pnlBottom = new Panel();
      this.txtTrackingNumber = new TextBox();
      this.label2 = new Label();
      this.tcDisclosure.SuspendLayout();
      this.tabPageDetail.SuspendLayout();
      this.detailsPanel.SuspendLayout();
      this.pnlLoanSnapshot.SuspendLayout();
      this.gcDocList.SuspendLayout();
      this.gradientPanel3.SuspendLayout();
      this.flowLayoutPanel2.SuspendLayout();
      this.pnlBasicInfo.SuspendLayout();
      this.tableLayoutPanel1.SuspendLayout();
      this.panel1.SuspendLayout();
      this.pnlIntent.SuspendLayout();
      this.panel2.SuspendLayout();
      this.pnlUserId.SuspendLayout();
      this.gradientPanel1.SuspendLayout();
      this.tabPageReasons.SuspendLayout();
      this.reasonsPanel.SuspendLayout();
      this.borderPanel1.SuspendLayout();
      this.panel5.SuspendLayout();
      ((ISupportInitialize) this.btnSelect).BeginInit();
      this.pnlReason.SuspendLayout();
      this.gradientPanel5.SuspendLayout();
      this.tabPageeDisclosure.SuspendLayout();
      this.pnleDisclosureStatus.SuspendLayout();
      this.gradientPanel2.SuspendLayout();
      this.pnlFulfillment.SuspendLayout();
      this.gradientPanel4.SuspendLayout();
      this.tableLayoutPanel2.SuspendLayout();
      this.panel3.SuspendLayout();
      this.pnlBottom.SuspendLayout();
      this.SuspendLayout();
      this.tableLayoutPanel2.SetColumnSpan((Control) this.tcDisclosure, 2);
      this.tcDisclosure.Controls.Add((Control) this.tabPageDetail);
      this.tcDisclosure.Controls.Add((Control) this.tabPageReasons);
      this.tcDisclosure.Controls.Add((Control) this.tabPageeDisclosure);
      this.tcDisclosure.Dock = DockStyle.Fill;
      this.tcDisclosure.Location = new Point(3, 32);
      this.tcDisclosure.Name = "tcDisclosure";
      this.tcDisclosure.SelectedIndex = 0;
      this.tcDisclosure.Size = new Size(925, 588);
      this.tcDisclosure.TabIndex = 4;
      this.tabPageDetail.Controls.Add((Control) this.detailsPanel);
      this.tabPageDetail.Location = new Point(4, 22);
      this.tabPageDetail.Name = "tabPageDetail";
      this.tabPageDetail.Padding = new Padding(1, 3, 3, 3);
      this.tabPageDetail.Size = new Size(917, 562);
      this.tabPageDetail.TabIndex = 0;
      this.tabPageDetail.Text = "Details";
      this.tabPageDetail.UseVisualStyleBackColor = true;
      this.detailsPanel.AutoScroll = true;
      this.detailsPanel.AutoScrollMinSize = new Size(900, 1152);
      this.detailsPanel.BackColor = Color.Transparent;
      this.detailsPanel.Controls.Add((Control) this.pnlLoanSnapshot);
      this.detailsPanel.Controls.Add((Control) this.pnlBasicInfo);
      this.detailsPanel.Dock = DockStyle.Fill;
      this.detailsPanel.Location = new Point(1, 3);
      this.detailsPanel.Margin = new Padding(2, 1, 2, 1);
      this.detailsPanel.Name = "detailsPanel";
      this.detailsPanel.Size = new Size(913, 556);
      this.detailsPanel.TabIndex = 0;
      this.pnlLoanSnapshot.BorderColor = Color.DarkGray;
      this.pnlLoanSnapshot.BorderColorStyle = BorderColorStyle.UserDefined;
      this.pnlLoanSnapshot.Borders = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlLoanSnapshot.Controls.Add((Control) this.lbtnDisclosedDailyInterest);
      this.pnlLoanSnapshot.Controls.Add((Control) this.txtDisclosedDailyInterest);
      this.pnlLoanSnapshot.Controls.Add((Control) this.label1);
      this.pnlLoanSnapshot.Controls.Add((Control) this.lbtnFinanceCharge);
      this.pnlLoanSnapshot.Controls.Add((Control) this.lbtnDisclosedAPR);
      this.pnlLoanSnapshot.Controls.Add((Control) this.gcDocList);
      this.pnlLoanSnapshot.Controls.Add((Control) this.txtApplicationDate);
      this.pnlLoanSnapshot.Controls.Add((Control) this.label42);
      this.pnlLoanSnapshot.Controls.Add((Control) this.txtFinanceCharge);
      this.pnlLoanSnapshot.Controls.Add((Control) this.txtLoanProgram);
      this.pnlLoanSnapshot.Controls.Add((Control) this.txtLoanAmount);
      this.pnlLoanSnapshot.Controls.Add((Control) this.txtDisclosedAPR);
      this.pnlLoanSnapshot.Controls.Add((Control) this.label38);
      this.pnlLoanSnapshot.Controls.Add((Control) this.label39);
      this.pnlLoanSnapshot.Controls.Add((Control) this.label40);
      this.pnlLoanSnapshot.Controls.Add((Control) this.label41);
      this.pnlLoanSnapshot.Controls.Add((Control) this.txtPropertyZip);
      this.pnlLoanSnapshot.Controls.Add((Control) this.txtPropertyState);
      this.pnlLoanSnapshot.Controls.Add((Control) this.txtPropertyCity);
      this.pnlLoanSnapshot.Controls.Add((Control) this.txtPropertyAddress);
      this.pnlLoanSnapshot.Controls.Add((Control) this.label37);
      this.pnlLoanSnapshot.Controls.Add((Control) this.label36);
      this.pnlLoanSnapshot.Controls.Add((Control) this.label35);
      this.pnlLoanSnapshot.Controls.Add((Control) this.label34);
      this.pnlLoanSnapshot.Controls.Add((Control) this.gradientPanel3);
      this.pnlLoanSnapshot.Dock = DockStyle.Fill;
      this.pnlLoanSnapshot.Location = new Point(0, 435);
      this.pnlLoanSnapshot.Name = "pnlLoanSnapshot";
      this.pnlLoanSnapshot.Size = new Size(900, 717);
      this.pnlLoanSnapshot.TabIndex = 2;
      this.lbtnDisclosedDailyInterest.Location = new Point(588, 53);
      this.lbtnDisclosedDailyInterest.LockedStateToolTip = "Use Default Value";
      this.lbtnDisclosedDailyInterest.MaximumSize = new Size(16, 17);
      this.lbtnDisclosedDailyInterest.MinimumSize = new Size(16, 17);
      this.lbtnDisclosedDailyInterest.Name = "lbtnDisclosedDailyInterest";
      this.lbtnDisclosedDailyInterest.Size = new Size(16, 17);
      this.lbtnDisclosedDailyInterest.TabIndex = 48;
      this.lbtnDisclosedDailyInterest.UnlockedStateToolTip = "Enter Data Manually";
      this.lbtnDisclosedDailyInterest.Click += new EventHandler(this.lbtnDisclosedDailyInterest_Click);
      this.txtDisclosedDailyInterest.Location = new Point(609, 53);
      this.txtDisclosedDailyInterest.Name = "txtDisclosedDailyInterest";
      this.txtDisclosedDailyInterest.Size = new Size(184, 20);
      this.txtDisclosedDailyInterest.TabIndex = 19;
      this.txtDisclosedDailyInterest.Leave += new EventHandler(this.txtDisclosedDailyInterest_Leave);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(468, 56);
      this.label1.Name = "label1";
      this.label1.Size = new Size(117, 13);
      this.label1.TabIndex = 46;
      this.label1.Text = "Disclosed Daily Interest";
      this.lbtnFinanceCharge.Location = new Point(588, 120);
      this.lbtnFinanceCharge.LockedStateToolTip = "Use Default Value";
      this.lbtnFinanceCharge.MaximumSize = new Size(16, 17);
      this.lbtnFinanceCharge.MinimumSize = new Size(16, 17);
      this.lbtnFinanceCharge.Name = "lbtnFinanceCharge";
      this.lbtnFinanceCharge.Size = new Size(16, 17);
      this.lbtnFinanceCharge.TabIndex = 45;
      this.lbtnFinanceCharge.UnlockedStateToolTip = "Enter Data Manually";
      this.lbtnFinanceCharge.Click += new EventHandler(this.lbtnFinanceCharge_Click);
      this.lbtnDisclosedAPR.Location = new Point(588, 32);
      this.lbtnDisclosedAPR.LockedStateToolTip = "Use Default Value";
      this.lbtnDisclosedAPR.MaximumSize = new Size(16, 17);
      this.lbtnDisclosedAPR.MinimumSize = new Size(16, 17);
      this.lbtnDisclosedAPR.Name = "lbtnDisclosedAPR";
      this.lbtnDisclosedAPR.Size = new Size(16, 17);
      this.lbtnDisclosedAPR.TabIndex = 44;
      this.lbtnDisclosedAPR.UnlockedStateToolTip = "Enter Data Manually";
      this.lbtnDisclosedAPR.Click += new EventHandler(this.lbtnDisclosedAPR_Click);
      this.gcDocList.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gcDocList.Controls.Add((Control) this.btnViewDisclosure);
      this.gcDocList.Controls.Add((Control) this.gvDocList);
      this.gcDocList.HeaderForeColor = SystemColors.ControlText;
      this.gcDocList.Location = new Point(7, 167);
      this.gcDocList.Name = "gcDocList";
      this.gcDocList.Size = new Size(786, 546);
      this.gcDocList.TabIndex = 25;
      this.gcDocList.Text = "Document Sent";
      this.btnViewDisclosure.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnViewDisclosure.Location = new Point(680, 1);
      this.btnViewDisclosure.Name = "btnViewDisclosure";
      this.btnViewDisclosure.Size = new Size(96, 22);
      this.btnViewDisclosure.TabIndex = 28;
      this.btnViewDisclosure.Text = "View Document";
      this.btnViewDisclosure.UseVisualStyleBackColor = true;
      this.btnViewDisclosure.Click += new EventHandler(this.btnViewDisclosure_Click);
      this.gvDocList.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Name";
      gvColumn1.Width = 600;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.SpringToFit = true;
      gvColumn2.Text = "Type";
      gvColumn2.Width = 184;
      this.gvDocList.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.gvDocList.Dock = DockStyle.Fill;
      this.gvDocList.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvDocList.Location = new Point(1, 26);
      this.gvDocList.Name = "gvDocList";
      this.gvDocList.Size = new Size(784, 519);
      this.gvDocList.TabIndex = 24;
      this.txtApplicationDate.Location = new Point(141, 96);
      this.txtApplicationDate.Name = "txtApplicationDate";
      this.txtApplicationDate.ReadOnly = true;
      this.txtApplicationDate.Size = new Size(184, 20);
      this.txtApplicationDate.TabIndex = 23;
      this.label42.AutoSize = true;
      this.label42.Location = new Point(4, 98);
      this.label42.Name = "label42";
      this.label42.Size = new Size(85, 13);
      this.label42.TabIndex = 22;
      this.label42.Text = "Application Date";
      this.txtFinanceCharge.Location = new Point(609, 120);
      this.txtFinanceCharge.Name = "txtFinanceCharge";
      this.txtFinanceCharge.Size = new Size(184, 20);
      this.txtFinanceCharge.TabIndex = 22;
      this.txtFinanceCharge.Leave += new EventHandler(this.txtFinanceCharge_Leave);
      this.txtLoanProgram.Location = new Point(609, 77);
      this.txtLoanProgram.Name = "txtLoanProgram";
      this.txtLoanProgram.ReadOnly = true;
      this.txtLoanProgram.Size = new Size(184, 20);
      this.txtLoanProgram.TabIndex = 20;
      this.txtLoanAmount.Location = new Point(609, 98);
      this.txtLoanAmount.Name = "txtLoanAmount";
      this.txtLoanAmount.ReadOnly = true;
      this.txtLoanAmount.Size = new Size(184, 20);
      this.txtLoanAmount.TabIndex = 21;
      this.txtDisclosedAPR.Location = new Point(609, 32);
      this.txtDisclosedAPR.Name = "txtDisclosedAPR";
      this.txtDisclosedAPR.Size = new Size(184, 20);
      this.txtDisclosedAPR.TabIndex = 18;
      this.txtDisclosedAPR.Leave += new EventHandler(this.txtDisclosedAPR_Leave);
      this.label38.AutoSize = true;
      this.label38.Location = new Point(468, 100);
      this.label38.Name = "label38";
      this.label38.Size = new Size(70, 13);
      this.label38.TabIndex = 17;
      this.label38.Text = "Loan Amount";
      this.label39.AutoSize = true;
      this.label39.Location = new Point(468, 122);
      this.label39.Name = "label39";
      this.label39.Size = new Size(82, 13);
      this.label39.TabIndex = 16;
      this.label39.Text = "Finance Charge";
      this.label40.AutoSize = true;
      this.label40.Location = new Point(468, 78);
      this.label40.Name = "label40";
      this.label40.Size = new Size(73, 13);
      this.label40.TabIndex = 15;
      this.label40.Text = "Loan Program";
      this.label41.AutoSize = true;
      this.label41.Location = new Point(468, 34);
      this.label41.Name = "label41";
      this.label41.Size = new Size(78, 13);
      this.label41.TabIndex = 14;
      this.label41.Text = "Disclosed APR";
      this.txtPropertyZip.Location = new Point(206, 74);
      this.txtPropertyZip.Name = "txtPropertyZip";
      this.txtPropertyZip.ReadOnly = true;
      this.txtPropertyZip.Size = new Size(118, 20);
      this.txtPropertyZip.TabIndex = 13;
      this.txtPropertyState.Location = new Point(141, 74);
      this.txtPropertyState.Name = "txtPropertyState";
      this.txtPropertyState.ReadOnly = true;
      this.txtPropertyState.Size = new Size(31, 20);
      this.txtPropertyState.TabIndex = 12;
      this.txtPropertyCity.Location = new Point(141, 52);
      this.txtPropertyCity.Name = "txtPropertyCity";
      this.txtPropertyCity.ReadOnly = true;
      this.txtPropertyCity.Size = new Size(183, 20);
      this.txtPropertyCity.TabIndex = 11;
      this.txtPropertyAddress.Location = new Point(141, 31);
      this.txtPropertyAddress.Name = "txtPropertyAddress";
      this.txtPropertyAddress.ReadOnly = true;
      this.txtPropertyAddress.Size = new Size(183, 20);
      this.txtPropertyAddress.TabIndex = 10;
      this.label37.AutoSize = true;
      this.label37.Location = new Point(178, 76);
      this.label37.Name = "label37";
      this.label37.Size = new Size(22, 13);
      this.label37.TabIndex = 7;
      this.label37.Text = "Zip";
      this.label36.AutoSize = true;
      this.label36.Location = new Point(4, 32);
      this.label36.Name = "label36";
      this.label36.Size = new Size(87, 13);
      this.label36.TabIndex = 6;
      this.label36.Text = "Property Address";
      this.label35.AutoSize = true;
      this.label35.Location = new Point(4, 76);
      this.label35.Name = "label35";
      this.label35.Size = new Size(32, 13);
      this.label35.TabIndex = 5;
      this.label35.Text = "State";
      this.label34.AutoSize = true;
      this.label34.Location = new Point(4, 54);
      this.label34.Name = "label34";
      this.label34.Size = new Size(24, 13);
      this.label34.TabIndex = 4;
      this.label34.Text = "City";
      this.gradientPanel3.Borders = AnchorStyles.Bottom;
      this.gradientPanel3.Controls.Add((Control) this.flowLayoutPanel2);
      this.gradientPanel3.Controls.Add((Control) this.label20);
      this.gradientPanel3.Dock = DockStyle.Top;
      this.gradientPanel3.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel3.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel3.Location = new Point(1, 0);
      this.gradientPanel3.Name = "gradientPanel3";
      this.gradientPanel3.Size = new Size(898, 25);
      this.gradientPanel3.Style = GradientPanel.PanelStyle.TableFooter;
      this.gradientPanel3.TabIndex = 1;
      this.flowLayoutPanel2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flowLayoutPanel2.Controls.Add((Control) this.btnProviderListSnapshot);
      this.flowLayoutPanel2.Controls.Add((Control) this.btnSafeHarborSnapshot);
      this.flowLayoutPanel2.Controls.Add((Control) this.btnItemSnapshot);
      this.flowLayoutPanel2.Controls.Add((Control) this.btnCDSnapshot);
      this.flowLayoutPanel2.Controls.Add((Control) this.btnLESnapshot);
      this.flowLayoutPanel2.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel2.Location = new Point(179, 0);
      this.flowLayoutPanel2.Name = "flowLayoutPanel2";
      this.flowLayoutPanel2.Padding = new Padding(0, 1, 0, 0);
      this.flowLayoutPanel2.Size = new Size(611, 26);
      this.flowLayoutPanel2.TabIndex = 23;
      this.btnProviderListSnapshot.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnProviderListSnapshot.Enabled = false;
      this.btnProviderListSnapshot.Location = new Point(521, 1);
      this.btnProviderListSnapshot.Margin = new Padding(0);
      this.btnProviderListSnapshot.Name = "btnProviderListSnapshot";
      this.btnProviderListSnapshot.Size = new Size(90, 22);
      this.btnProviderListSnapshot.TabIndex = 27;
      this.btnProviderListSnapshot.Text = "SSPL Snapshot";
      this.btnProviderListSnapshot.UseVisualStyleBackColor = true;
      this.btnProviderListSnapshot.Click += new EventHandler(this.btnProviderListSnapshot_Click);
      this.btnSafeHarborSnapshot.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnSafeHarborSnapshot.Enabled = false;
      this.btnSafeHarborSnapshot.Location = new Point(389, 1);
      this.btnSafeHarborSnapshot.Margin = new Padding(0);
      this.btnSafeHarborSnapshot.Name = "btnSafeHarborSnapshot";
      this.btnSafeHarborSnapshot.Size = new Size(132, 22);
      this.btnSafeHarborSnapshot.TabIndex = 26;
      this.btnSafeHarborSnapshot.Text = "Safe Harbor Snapshot";
      this.btnSafeHarborSnapshot.UseVisualStyleBackColor = true;
      this.btnSafeHarborSnapshot.Click += new EventHandler(this.btnSafeHarborSnapshot_Click);
      this.btnItemSnapshot.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnItemSnapshot.Location = new Point(269, 1);
      this.btnItemSnapshot.Margin = new Padding(0);
      this.btnItemSnapshot.Name = "btnItemSnapshot";
      this.btnItemSnapshot.Size = new Size(120, 22);
      this.btnItemSnapshot.TabIndex = 79;
      this.btnItemSnapshot.Text = "Itemization Snapshot";
      this.btnItemSnapshot.UseVisualStyleBackColor = true;
      this.btnItemSnapshot.Click += new EventHandler(this.btnItemSnapshot_Click);
      this.btnCDSnapshot.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnCDSnapshot.Location = new Point(179, 1);
      this.btnCDSnapshot.Margin = new Padding(0);
      this.btnCDSnapshot.Name = "btnCDSnapshot";
      this.btnCDSnapshot.Size = new Size(90, 22);
      this.btnCDSnapshot.TabIndex = 25;
      this.btnCDSnapshot.Text = "CD Snapshot";
      this.btnCDSnapshot.UseVisualStyleBackColor = true;
      this.btnCDSnapshot.Click += new EventHandler(this.btnCDSnapshot_Click);
      this.btnLESnapshot.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnLESnapshot.Location = new Point(89, 1);
      this.btnLESnapshot.Margin = new Padding(0);
      this.btnLESnapshot.Name = "btnLESnapshot";
      this.btnLESnapshot.Size = new Size(90, 22);
      this.btnLESnapshot.TabIndex = 24;
      this.btnLESnapshot.Text = "LE Snapshot";
      this.btnLESnapshot.UseVisualStyleBackColor = true;
      this.btnLESnapshot.Click += new EventHandler(this.lblSnapshot_Click);
      this.label20.AutoSize = true;
      this.label20.BackColor = Color.Transparent;
      this.label20.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label20.Location = new Point(5, 5);
      this.label20.Name = "label20";
      this.label20.Size = new Size(89, 14);
      this.label20.TabIndex = 2;
      this.label20.Text = "Loan Snapshot";
      this.pnlBasicInfo.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlBasicInfo.Controls.Add((Control) this.tableLayoutPanel1);
      this.pnlBasicInfo.Controls.Add((Control) this.gradientPanel1);
      this.pnlBasicInfo.Dock = DockStyle.Top;
      this.pnlBasicInfo.Location = new Point(0, 0);
      this.pnlBasicInfo.Name = "pnlBasicInfo";
      this.pnlBasicInfo.Size = new Size(900, 435);
      this.pnlBasicInfo.TabIndex = 0;
      this.tableLayoutPanel1.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
      this.tableLayoutPanel1.ColumnCount = 2;
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
      this.tableLayoutPanel1.Controls.Add((Control) this.panel1, 0, 0);
      this.tableLayoutPanel1.Controls.Add((Control) this.panel2, 1, 0);
      this.tableLayoutPanel1.Dock = DockStyle.Fill;
      this.tableLayoutPanel1.Location = new Point(1, 26);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.RowCount = 1;
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 56.34096f));
      this.tableLayoutPanel1.Size = new Size(898, 409);
      this.tableLayoutPanel1.TabIndex = 10;
      this.panel1.Controls.Add((Control) this.chkUseforUCDExport);
      this.panel1.Controls.Add((Control) this.lblUseforUCDExport);
      this.panel1.Controls.Add((Control) this.txtSource);
      this.panel1.Controls.Add((Control) this.txtProviderDescription);
      this.panel1.Controls.Add((Control) this.lblProvider);
      this.panel1.Controls.Add((Control) this.lbtnSentDate);
      this.panel1.Controls.Add((Control) this.txtMethodOther);
      this.panel1.Controls.Add((Control) this.cmbDisclosureType);
      this.panel1.Controls.Add((Control) this.lblDisclosureType);
      this.panel1.Controls.Add((Control) this.txtDisclosedBy);
      this.panel1.Controls.Add((Control) this.chkIntent);
      this.panel1.Controls.Add((Control) this.lbtnDisclosedBy);
      this.panel1.Controls.Add((Control) this.txtSentMethod);
      this.panel1.Controls.Add((Control) this.dtSentDate);
      this.panel1.Controls.Add((Control) this.cmbSentMethod);
      this.panel1.Controls.Add((Control) this.lblBy);
      this.panel1.Controls.Add((Control) this.bblSentMethod);
      this.panel1.Controls.Add((Control) this.lblSentDate);
      this.panel1.Controls.Add((Control) this.pnlIntent);
      this.panel1.Dock = DockStyle.Fill;
      this.panel1.Location = new Point(4, 4);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(441, 401);
      this.panel1.TabIndex = 10;
      this.chkUseforUCDExport.AutoSize = true;
      this.chkUseforUCDExport.Location = new Point(158, 174);
      this.chkUseforUCDExport.Name = "chkUseforUCDExport";
      this.chkUseforUCDExport.Size = new Size(15, 14);
      this.chkUseforUCDExport.TabIndex = 65;
      this.chkUseforUCDExport.UseVisualStyleBackColor = true;
      this.lblUseforUCDExport.AutoSize = true;
      this.lblUseforUCDExport.Location = new Point(8, 175);
      this.lblUseforUCDExport.Name = "lblUseforUCDExport";
      this.lblUseforUCDExport.Size = new Size(100, 13);
      this.lblUseforUCDExport.TabIndex = 64;
      this.lblUseforUCDExport.Text = "Use for UCD Export";
      this.txtSource.Enabled = false;
      this.txtSource.Location = new Point(158, 124);
      this.txtSource.Name = "txtSource";
      this.txtSource.Size = new Size(183, 20);
      this.txtSource.TabIndex = 62;
      this.txtProviderDescription.Location = new Point(158, 148);
      this.txtProviderDescription.Name = "txtProviderDescription";
      this.txtProviderDescription.Size = new Size(183, 20);
      this.txtProviderDescription.TabIndex = 63;
      this.lblProvider.AutoSize = true;
      this.lblProvider.Location = new Point(8, 123);
      this.lblProvider.Name = "lblProvider";
      this.lblProvider.Size = new Size(46, 13);
      this.lblProvider.TabIndex = 61;
      this.lblProvider.Text = "Provider";
      this.lbtnSentDate.Location = new Point(139, 37);
      this.lbtnSentDate.LockedStateToolTip = "Use Default Value";
      this.lbtnSentDate.MaximumSize = new Size(16, 17);
      this.lbtnSentDate.MinimumSize = new Size(16, 17);
      this.lbtnSentDate.Name = "lbtnSentDate";
      this.lbtnSentDate.Size = new Size(16, 17);
      this.lbtnSentDate.TabIndex = 2;
      this.lbtnSentDate.UnlockedStateToolTip = "Enter Data Manually";
      this.lbtnSentDate.Click += new EventHandler(this.lbtnSentDate_Click);
      this.txtMethodOther.Enabled = false;
      this.txtMethodOther.Location = new Point(158, 100);
      this.txtMethodOther.Name = "txtMethodOther";
      this.txtMethodOther.Size = new Size(183, 20);
      this.txtMethodOther.TabIndex = 8;
      this.cmbDisclosureType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbDisclosureType.FormattingEnabled = true;
      this.cmbDisclosureType.Items.AddRange(new object[3]
      {
        (object) "Initial",
        (object) "Revised",
        (object) "Final"
      });
      this.cmbDisclosureType.Location = new Point(158, 7);
      this.cmbDisclosureType.Name = "cmbDisclosureType";
      this.cmbDisclosureType.Size = new Size(183, 21);
      this.cmbDisclosureType.TabIndex = 1;
      this.cmbDisclosureType.SelectedIndexChanged += new EventHandler(this.cmbDisclosureType_SelectedIndexChanged);
      this.lblDisclosureType.AutoSize = true;
      this.lblDisclosureType.Location = new Point(6, 13);
      this.lblDisclosureType.Name = "lblDisclosureType";
      this.lblDisclosureType.Size = new Size(83, 13);
      this.lblDisclosureType.TabIndex = 60;
      this.lblDisclosureType.Text = "Disclosure Type";
      this.txtDisclosedBy.Location = new Point(158, 55);
      this.txtDisclosedBy.MaxLength = 100;
      this.txtDisclosedBy.Name = "txtDisclosedBy";
      this.txtDisclosedBy.Size = new Size(183, 20);
      this.txtDisclosedBy.TabIndex = 5;
      this.chkIntent.AutoSize = true;
      this.chkIntent.Location = new Point(10, 194);
      this.chkIntent.Name = "chkIntent";
      this.chkIntent.Size = new Size(108, 17);
      this.chkIntent.TabIndex = 9;
      this.chkIntent.Text = "Intent to Proceed";
      this.chkIntent.UseVisualStyleBackColor = true;
      this.chkIntent.CheckedChanged += new EventHandler(this.chkIntent_CheckedChanged);
      this.lbtnDisclosedBy.Location = new Point(139, 58);
      this.lbtnDisclosedBy.LockedStateToolTip = "Use Default Value";
      this.lbtnDisclosedBy.MaximumSize = new Size(16, 17);
      this.lbtnDisclosedBy.MinimumSize = new Size(16, 17);
      this.lbtnDisclosedBy.Name = "lbtnDisclosedBy";
      this.lbtnDisclosedBy.Size = new Size(16, 17);
      this.lbtnDisclosedBy.TabIndex = 4;
      this.lbtnDisclosedBy.UnlockedStateToolTip = "Enter Data Manually";
      this.lbtnDisclosedBy.Click += new EventHandler(this.lbtnDisclosedBy_Click);
      this.txtSentMethod.Location = new Point(158, 78);
      this.txtSentMethod.Name = "txtSentMethod";
      this.txtSentMethod.ReadOnly = true;
      this.txtSentMethod.Size = new Size(183, 20);
      this.txtSentMethod.TabIndex = 7;
      this.txtSentMethod.Text = "eFolder eDisclosures";
      this.txtSentMethod.Visible = false;
      this.dtSentDate.CalendarMonthBackground = Color.WhiteSmoke;
      this.dtSentDate.CalendarTitleBackColor = Color.FromArgb(29, 110, 174);
      this.dtSentDate.CustomFormat = "MM/dd/yyyy";
      this.dtSentDate.Format = DateTimePickerFormat.Short;
      this.dtSentDate.Location = new Point(158, 34);
      this.dtSentDate.MaxDate = new DateTime(2029, 12, 31, 0, 0, 0, 0);
      this.dtSentDate.MinDate = new DateTime(2000, 1, 1, 0, 0, 0, 0);
      this.dtSentDate.Name = "dtSentDate";
      this.dtSentDate.Size = new Size(183, 20);
      this.dtSentDate.TabIndex = 3;
      this.dtSentDate.CloseUp += new EventHandler(this.dtDisclosedDate_CloseUp);
      this.dtSentDate.ValueChanged += new EventHandler(this.dtDisclosedDate_ValueChanged);
      this.dtSentDate.DropDown += new EventHandler(this.dtDisclosedDate_DropDown);
      this.cmbSentMethod.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbSentMethod.FormattingEnabled = true;
      this.cmbSentMethod.Location = new Point(158, 78);
      this.cmbSentMethod.Name = "cmbSentMethod";
      this.cmbSentMethod.Size = new Size(183, 21);
      this.cmbSentMethod.TabIndex = 6;
      this.cmbSentMethod.SelectedIndexChanged += new EventHandler(this.cmbDisclosedMethod_SelectedIndexChanged);
      this.lblBy.AutoSize = true;
      this.lblBy.Location = new Point(6, 59);
      this.lblBy.Name = "lblBy";
      this.lblBy.Size = new Size(19, 13);
      this.lblBy.TabIndex = 51;
      this.lblBy.Text = "By";
      this.bblSentMethod.AutoSize = true;
      this.bblSentMethod.Location = new Point(6, 82);
      this.bblSentMethod.Name = "bblSentMethod";
      this.bblSentMethod.Size = new Size(68, 13);
      this.bblSentMethod.TabIndex = 50;
      this.bblSentMethod.Text = "Sent Method";
      this.lblSentDate.AutoSize = true;
      this.lblSentDate.Location = new Point(6, 38);
      this.lblSentDate.Name = "lblSentDate";
      this.lblSentDate.Size = new Size(55, 13);
      this.lblSentDate.TabIndex = 49;
      this.lblSentDate.Text = "Sent Date";
      this.pnlIntent.BackColor = SystemColors.ControlLight;
      this.pnlIntent.Controls.Add((Control) this.txtIntentMethodOther);
      this.pnlIntent.Controls.Add((Control) this.txtIntentToProceedMethod);
      this.pnlIntent.Controls.Add((Control) this.txtIntentComments);
      this.pnlIntent.Controls.Add((Control) this.label8);
      this.pnlIntent.Controls.Add((Control) this.cmbIntentReceivedMethod);
      this.pnlIntent.Controls.Add((Control) this.label5);
      this.pnlIntent.Controls.Add((Control) this.txtIntentReceived);
      this.pnlIntent.Controls.Add((Control) this.dpIntentDate);
      this.pnlIntent.Controls.Add((Control) this.label6);
      this.pnlIntent.Controls.Add((Control) this.label7);
      this.pnlIntent.Controls.Add((Control) this.lBtnIntentReceivedBy);
      this.pnlIntent.Location = new Point(7, 216);
      this.pnlIntent.Name = "pnlIntent";
      this.pnlIntent.Size = new Size(340, 176);
      this.pnlIntent.TabIndex = 10;
      this.txtIntentMethodOther.Location = new Point(151, 69);
      this.txtIntentMethodOther.Name = "txtIntentMethodOther";
      this.txtIntentMethodOther.Size = new Size(183, 20);
      this.txtIntentMethodOther.TabIndex = 6;
      this.txtIntentToProceedMethod.Location = new Point(151, 48);
      this.txtIntentToProceedMethod.Name = "txtIntentToProceedMethod";
      this.txtIntentToProceedMethod.ReadOnly = true;
      this.txtIntentToProceedMethod.Size = new Size(183, 20);
      this.txtIntentToProceedMethod.TabIndex = 5;
      this.txtIntentToProceedMethod.Text = "eFolder eDisclosures";
      this.txtIntentToProceedMethod.Visible = false;
      this.txtIntentComments.Location = new Point(6, 103);
      this.txtIntentComments.Multiline = true;
      this.txtIntentComments.Name = "txtIntentComments";
      this.txtIntentComments.ScrollBars = ScrollBars.Both;
      this.txtIntentComments.Size = new Size(328, 66);
      this.txtIntentComments.TabIndex = 7;
      this.txtIntentComments.KeyUp += new KeyEventHandler(this.txt_KeyUp);
      this.label8.AutoSize = true;
      this.label8.Location = new Point(3, 87);
      this.label8.Name = "label8";
      this.label8.Size = new Size(56, 13);
      this.label8.TabIndex = 70;
      this.label8.Text = "Comments";
      this.cmbIntentReceivedMethod.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbIntentReceivedMethod.FormattingEnabled = true;
      this.cmbIntentReceivedMethod.Location = new Point(151, 47);
      this.cmbIntentReceivedMethod.Name = "cmbIntentReceivedMethod";
      this.cmbIntentReceivedMethod.Size = new Size(183, 21);
      this.cmbIntentReceivedMethod.TabIndex = 4;
      this.cmbIntentReceivedMethod.SelectedIndexChanged += new EventHandler(this.cmbIntentReceivedMethod_SelectedIndexChanged);
      this.label5.AutoSize = true;
      this.label5.Location = new Point(3, 47);
      this.label5.Name = "label5";
      this.label5.Size = new Size(92, 13);
      this.label5.TabIndex = 68;
      this.label5.Text = "Received Method";
      this.txtIntentReceived.Location = new Point(151, 25);
      this.txtIntentReceived.MaxLength = 100;
      this.txtIntentReceived.Name = "txtIntentReceived";
      this.txtIntentReceived.Size = new Size(183, 20);
      this.txtIntentReceived.TabIndex = 3;
      this.dpIntentDate.CalendarMonthBackground = Color.WhiteSmoke;
      this.dpIntentDate.CalendarTitleBackColor = Color.FromArgb(29, 110, 174);
      this.dpIntentDate.CustomFormat = "MM/dd/yyyy";
      this.dpIntentDate.Format = DateTimePickerFormat.Short;
      this.dpIntentDate.Location = new Point(151, 3);
      this.dpIntentDate.MaxDate = new DateTime(2029, 12, 31, 0, 0, 0, 0);
      this.dpIntentDate.MinDate = new DateTime(2000, 1, 3, 0, 0, 0, 0);
      this.dpIntentDate.Name = "dpIntentDate";
      this.dpIntentDate.Size = new Size(183, 20);
      this.dpIntentDate.TabIndex = 1;
      this.dpIntentDate.Value = new DateTime(2000, 1, 3, 0, 0, 0, 0);
      this.label6.AutoSize = true;
      this.label6.Location = new Point(3, 25);
      this.label6.Name = "label6";
      this.label6.Size = new Size(68, 13);
      this.label6.TabIndex = 66;
      this.label6.Text = "Received By";
      this.label7.AutoSize = true;
      this.label7.Location = new Point(3, 3);
      this.label7.Name = "label7";
      this.label7.Size = new Size(30, 13);
      this.label7.TabIndex = 65;
      this.label7.Text = "Date";
      this.lBtnIntentReceivedBy.Location = new Point(132, 25);
      this.lBtnIntentReceivedBy.LockedStateToolTip = "Use Default Value";
      this.lBtnIntentReceivedBy.MaximumSize = new Size(16, 17);
      this.lBtnIntentReceivedBy.MinimumSize = new Size(16, 17);
      this.lBtnIntentReceivedBy.Name = "lBtnIntentReceivedBy";
      this.lBtnIntentReceivedBy.Size = new Size(16, 17);
      this.lBtnIntentReceivedBy.TabIndex = 2;
      this.lBtnIntentReceivedBy.UnlockedStateToolTip = "Enter Data Manually";
      this.lBtnIntentReceivedBy.Click += new EventHandler(this.lBtnIntentReceivedBy_Click);
      this.panel2.Controls.Add((Control) this.pnlUserId);
      this.panel2.Controls.Add((Control) this.lBtnRecipientType);
      this.panel2.Controls.Add((Control) this.dpRecipientPresumedReceivedDate);
      this.panel2.Controls.Add((Control) this.txtRecipientReceivedMethod);
      this.panel2.Controls.Add((Control) this.lBtnRecipientPresumed);
      this.panel2.Controls.Add((Control) this.cmbRecipientType);
      this.panel2.Controls.Add((Control) this.dpRecipientActualReceivedDate);
      this.panel2.Controls.Add((Control) this.txtRecipientType);
      this.panel2.Controls.Add((Control) this.txtRecipientOther);
      this.panel2.Controls.Add((Control) this.label12);
      this.panel2.Controls.Add((Control) this.lblSigner);
      this.panel2.Controls.Add((Control) this.label4);
      this.panel2.Controls.Add((Control) this.cmbRecipientReceivedMethod);
      this.panel2.Controls.Add((Control) this.label3);
      this.panel2.Controls.Add((Control) this.lblDisclosurePresumedReceivedDate);
      this.panel2.Location = new Point(452, 4);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(346, 321);
      this.panel2.TabIndex = 11;
      this.pnlUserId.Controls.Add((Control) this.lblUserID);
      this.pnlUserId.Controls.Add((Control) this.txtUserID);
      this.pnlUserId.Location = new Point(3, 156);
      this.pnlUserId.Name = "pnlUserId";
      this.pnlUserId.Size = new Size(342, 32);
      this.pnlUserId.TabIndex = 90;
      this.lblUserID.AutoSize = true;
      this.lblUserID.Location = new Point(9, 6);
      this.lblUserID.Name = "lblUserID";
      this.lblUserID.Size = new Size(43, 13);
      this.lblUserID.TabIndex = 67;
      this.lblUserID.Text = "User ID";
      this.txtUserID.Location = new Point(152, 3);
      this.txtUserID.Name = "txtUserID";
      this.txtUserID.Size = new Size(183, 20);
      this.txtUserID.TabIndex = 84;
      this.lBtnRecipientType.Location = new Point(136, 133);
      this.lBtnRecipientType.LockedStateToolTip = "Use Default Value";
      this.lBtnRecipientType.MaximumSize = new Size(16, 16);
      this.lBtnRecipientType.MinimumSize = new Size(16, 16);
      this.lBtnRecipientType.Name = "lBtnRecipientType";
      this.lBtnRecipientType.Size = new Size(16, 16);
      this.lBtnRecipientType.TabIndex = 89;
      this.lBtnRecipientType.UnlockedStateToolTip = "Enter Data Manually";
      this.lBtnRecipientType.Visible = false;
      this.lBtnRecipientType.Click += new EventHandler(this.lBtnRecipientType_Click);
      this.dpRecipientPresumedReceivedDate.BackColor = SystemColors.Window;
      this.dpRecipientPresumedReceivedDate.Location = new Point(155, 79);
      this.dpRecipientPresumedReceivedDate.Margin = new Padding(4);
      this.dpRecipientPresumedReceivedDate.MaxValue = new DateTime(2029, 12, 31, 0, 0, 0, 0);
      this.dpRecipientPresumedReceivedDate.MinValue = new DateTime(2000, 1, 1, 0, 0, 0, 0);
      this.dpRecipientPresumedReceivedDate.Name = "dpRecipientPresumedReceivedDate";
      this.dpRecipientPresumedReceivedDate.Size = new Size(183, 21);
      this.dpRecipientPresumedReceivedDate.TabIndex = 14;
      this.dpRecipientPresumedReceivedDate.Tag = (object) "763";
      this.dpRecipientPresumedReceivedDate.ToolTip = "";
      this.dpRecipientPresumedReceivedDate.Value = new DateTime(0L);
      this.dpRecipientPresumedReceivedDate.ValueChanged += new EventHandler(this.dpReceivedDate_ValueChanged);
      this.dpRecipientPresumedReceivedDate.KeyPress += new KeyPressEventHandler(this.dpBorrowerReceivedDate_KeyPress);
      this.txtRecipientReceivedMethod.BackColor = SystemColors.Control;
      this.txtRecipientReceivedMethod.Location = new Point(155, 31);
      this.txtRecipientReceivedMethod.Name = "txtRecipientReceivedMethod";
      this.txtRecipientReceivedMethod.ReadOnly = true;
      this.txtRecipientReceivedMethod.Size = new Size(183, 20);
      this.txtRecipientReceivedMethod.TabIndex = 12;
      this.txtRecipientReceivedMethod.Text = "eFolder eDisclosures";
      this.txtRecipientReceivedMethod.Visible = false;
      this.lBtnRecipientPresumed.Location = new Point(136, 81);
      this.lBtnRecipientPresumed.LockedStateToolTip = "Use Default Value";
      this.lBtnRecipientPresumed.MaximumSize = new Size(16, 17);
      this.lBtnRecipientPresumed.MinimumSize = new Size(16, 17);
      this.lBtnRecipientPresumed.Name = "lBtnRecipientPresumed";
      this.lBtnRecipientPresumed.Size = new Size(16, 17);
      this.lBtnRecipientPresumed.TabIndex = 86;
      this.lBtnRecipientPresumed.UnlockedStateToolTip = "Enter Data Manually";
      this.lBtnRecipientPresumed.Click += new EventHandler(this.lBtnRecipientPresumed_Click);
      this.cmbRecipientType.FormattingEnabled = true;
      this.cmbRecipientType.Location = new Point(156, 129);
      this.cmbRecipientType.Name = "cmbRecipientType";
      this.cmbRecipientType.Size = new Size(182, 21);
      this.cmbRecipientType.TabIndex = 81;
      this.dpRecipientActualReceivedDate.BackColor = SystemColors.Window;
      this.dpRecipientActualReceivedDate.Location = new Point(155, 104);
      this.dpRecipientActualReceivedDate.Margin = new Padding(4);
      this.dpRecipientActualReceivedDate.MaxValue = new DateTime(2029, 12, 31, 0, 0, 0, 0);
      this.dpRecipientActualReceivedDate.MinValue = new DateTime(2000, 1, 1, 0, 0, 0, 0);
      this.dpRecipientActualReceivedDate.Name = "dpRecipientActualReceivedDate";
      this.dpRecipientActualReceivedDate.Size = new Size(183, 21);
      this.dpRecipientActualReceivedDate.TabIndex = 15;
      this.dpRecipientActualReceivedDate.Tag = (object) "763";
      this.dpRecipientActualReceivedDate.ToolTip = "";
      this.dpRecipientActualReceivedDate.Value = new DateTime(0L);
      this.dpRecipientActualReceivedDate.ValueChanged += new EventHandler(this.dpReceivedDate_ValueChanged);
      this.dpRecipientActualReceivedDate.KeyPress += new KeyPressEventHandler(this.dpBorrowerReceivedDate_KeyPress);
      this.txtRecipientType.Location = new Point(155, 129);
      this.txtRecipientType.Name = "txtRecipientType";
      this.txtRecipientType.ReadOnly = true;
      this.txtRecipientType.Size = new Size(182, 20);
      this.txtRecipientType.TabIndex = 16;
      this.txtRecipientOther.Location = new Point(155, 56);
      this.txtRecipientOther.Name = "txtRecipientOther";
      this.txtRecipientOther.Size = new Size(183, 20);
      this.txtRecipientOther.TabIndex = 13;
      this.label12.AutoSize = true;
      this.label12.Location = new Point(11, 129);
      this.label12.Name = "label12";
      this.label12.Size = new Size(79, 13);
      this.label12.TabIndex = 67;
      this.label12.Text = "Recipient Type";
      this.lblSigner.AutoSize = true;
      this.lblSigner.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblSigner.Location = new Point(11, 10);
      this.lblSigner.Name = "lblSigner";
      this.lblSigner.Size = new Size(61, 13);
      this.lblSigner.TabIndex = 66;
      this.lblSigner.Text = "Recipient";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(11, 105);
      this.label4.Name = "label4";
      this.label4.Size = new Size(112, 13);
      this.label4.TabIndex = 64;
      this.label4.Text = "Actual Received Date";
      this.cmbRecipientReceivedMethod.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbRecipientReceivedMethod.FormattingEnabled = true;
      this.cmbRecipientReceivedMethod.Location = new Point(155, 31);
      this.cmbRecipientReceivedMethod.Name = "cmbRecipientReceivedMethod";
      this.cmbRecipientReceivedMethod.Size = new Size(183, 21);
      this.cmbRecipientReceivedMethod.TabIndex = 11;
      this.cmbRecipientReceivedMethod.SelectedIndexChanged += new EventHandler(this.cmbBorrowerReceivedMethod_SelectedIndexChanged);
      this.label3.AutoSize = true;
      this.label3.Location = new Point(12, 34);
      this.label3.Name = "label3";
      this.label3.Size = new Size(92, 13);
      this.label3.TabIndex = 62;
      this.label3.Text = "Received Method";
      this.lblDisclosurePresumedReceivedDate.AutoSize = true;
      this.lblDisclosurePresumedReceivedDate.Location = new Point(11, 82);
      this.lblDisclosurePresumedReceivedDate.Name = "lblDisclosurePresumedReceivedDate";
      this.lblDisclosurePresumedReceivedDate.Size = new Size(129, 13);
      this.lblDisclosurePresumedReceivedDate.TabIndex = 54;
      this.lblDisclosurePresumedReceivedDate.Text = "Presumed Received Date";
      this.gradientPanel1.Borders = AnchorStyles.Bottom;
      this.gradientPanel1.Controls.Add((Control) this.btnEsign);
      this.gradientPanel1.Controls.Add((Control) this.label46);
      this.gradientPanel1.Controls.Add((Control) this.label44);
      this.gradientPanel1.Controls.Add((Control) this.chkLEDisclosedByBroker);
      this.gradientPanel1.Controls.Add((Control) this.lblDisclosureInfo);
      this.gradientPanel1.Dock = DockStyle.Top;
      this.gradientPanel1.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel1.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel1.Location = new Point(1, 1);
      this.gradientPanel1.Name = "gradientPanel1";
      this.gradientPanel1.Size = new Size(898, 25);
      this.gradientPanel1.Style = GradientPanel.PanelStyle.TableFooter;
      this.gradientPanel1.TabIndex = 0;
      this.btnEsign.Location = new Point(692, 1);
      this.btnEsign.Name = "btnEsign";
      this.btnEsign.Size = new Size(105, 23);
      this.btnEsign.TabIndex = 17;
      this.btnEsign.Text = "eSign Documents";
      this.btnEsign.UseVisualStyleBackColor = true;
      this.btnEsign.Click += new EventHandler(this.btnEsign_Click);
      this.label46.AutoSize = true;
      this.label46.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label46.Location = new Point(11, 7);
      this.label46.Name = "label46";
      this.label46.Size = new Size(106, 14);
      this.label46.TabIndex = 16;
      this.label46.Text = "Disclosure Details";
      this.label44.AutoSize = true;
      this.label44.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label44.Location = new Point(5, 8);
      this.label44.Name = "label44";
      this.label44.Size = new Size(0, 13);
      this.label44.TabIndex = 15;
      this.chkLEDisclosedByBroker.AutoSize = true;
      this.chkLEDisclosedByBroker.Location = new Point(470, 5);
      this.chkLEDisclosedByBroker.Name = "chkLEDisclosedByBroker";
      this.chkLEDisclosedByBroker.Size = new Size(190, 17);
      this.chkLEDisclosedByBroker.TabIndex = 2;
      this.chkLEDisclosedByBroker.Text = "Loan Estimate Disclosed by Broker";
      this.chkLEDisclosedByBroker.UseVisualStyleBackColor = true;
      this.lblDisclosureInfo.AutoSize = true;
      this.lblDisclosureInfo.BackColor = Color.Transparent;
      this.lblDisclosureInfo.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblDisclosureInfo.Location = new Point(3, -12);
      this.lblDisclosureInfo.Name = "lblDisclosureInfo";
      this.lblDisclosureInfo.Size = new Size(133, 14);
      this.lblDisclosureInfo.TabIndex = 1;
      this.lblDisclosureInfo.Text = "Disclosure Information";
      this.tabPageReasons.Controls.Add((Control) this.reasonsPanel);
      this.tabPageReasons.Location = new Point(4, 22);
      this.tabPageReasons.Name = "tabPageReasons";
      this.tabPageReasons.Size = new Size(917, 562);
      this.tabPageReasons.TabIndex = 2;
      this.tabPageReasons.Text = "Reasons";
      this.tabPageReasons.UseVisualStyleBackColor = true;
      this.reasonsPanel.AutoScroll = true;
      this.reasonsPanel.AutoScrollMinSize = new Size(900, 1152);
      this.reasonsPanel.BackColor = Color.Transparent;
      this.reasonsPanel.Controls.Add((Control) this.borderPanel1);
      this.reasonsPanel.Controls.Add((Control) this.gradientPanel5);
      this.reasonsPanel.Dock = DockStyle.Fill;
      this.reasonsPanel.Location = new Point(0, 0);
      this.reasonsPanel.Margin = new Padding(2, 1, 2, 1);
      this.reasonsPanel.Name = "reasonsPanel";
      this.reasonsPanel.Size = new Size(917, 562);
      this.reasonsPanel.TabIndex = 0;
      this.borderPanel1.Controls.Add((Control) this.panel5);
      this.borderPanel1.Controls.Add((Control) this.pnlReason);
      this.borderPanel1.Dock = DockStyle.Fill;
      this.borderPanel1.Location = new Point(0, 25);
      this.borderPanel1.Name = "borderPanel1";
      this.borderPanel1.Size = new Size(900, 1127);
      this.borderPanel1.TabIndex = 17;
      this.panel5.AutoScroll = true;
      this.panel5.Controls.Add((Control) this.feeDetailsLabel);
      this.panel5.Controls.Add((Control) this.feeDetailsGV);
      this.panel5.Controls.Add((Control) this.txtChangedCircumstance);
      this.panel5.Controls.Add((Control) this.txtCiCComment);
      this.panel5.Controls.Add((Control) this.label25);
      this.panel5.Controls.Add((Control) this.btnSelect);
      this.panel5.Controls.Add((Control) this.label10);
      this.panel5.Dock = DockStyle.Fill;
      this.panel5.Location = new Point(1, 331);
      this.panel5.Name = "panel5";
      this.panel5.Size = new Size(898, 795);
      this.panel5.TabIndex = 14;
      this.feeDetailsLabel.AutoSize = true;
      this.feeDetailsLabel.Location = new Point(10, 144);
      this.feeDetailsLabel.Name = "feeDetailsLabel";
      this.feeDetailsLabel.Size = new Size(70, 13);
      this.feeDetailsLabel.TabIndex = 49;
      this.feeDetailsLabel.Text = "Fee Changes";
      this.feeDetailsGV.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "FeeDesc";
      gvColumn3.Text = "Fee Description";
      gvColumn3.Width = 200;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "NewAmt";
      gvColumn4.Text = "New Amount $";
      gvColumn4.Width = 100;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Description";
      gvColumn5.Text = "Description";
      gvColumn5.Width = 125;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Comments";
      gvColumn6.Text = "Comments";
      gvColumn6.Width = 125;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "Reason";
      gvColumn7.Text = "Reason";
      gvColumn7.Width = 150;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "changesReceivedDate";
      gvColumn8.SpringToFit = true;
      gvColumn8.Text = "Changes Received Date";
      gvColumn8.Width = 471;
      this.feeDetailsGV.Columns.AddRange(new GVColumn[6]
      {
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6,
        gvColumn7,
        gvColumn8
      });
      this.feeDetailsGV.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.feeDetailsGV.Location = new Point(13, 159);
      this.feeDetailsGV.Name = "feeDetailsGV";
      this.feeDetailsGV.Size = new Size(872, 625);
      this.feeDetailsGV.TabIndex = 48;
      this.txtChangedCircumstance.Location = new Point(13, 34);
      this.txtChangedCircumstance.Multiline = true;
      this.txtChangedCircumstance.Name = "txtChangedCircumstance";
      this.txtChangedCircumstance.ScrollBars = ScrollBars.Both;
      this.txtChangedCircumstance.Size = new Size(307, 95);
      this.txtChangedCircumstance.TabIndex = 40;
      this.txtChangedCircumstance.KeyUp += new KeyEventHandler(this.txt_KeyUp);
      this.txtCiCComment.Location = new Point(388, 34);
      this.txtCiCComment.Multiline = true;
      this.txtCiCComment.Name = "txtCiCComment";
      this.txtCiCComment.ScrollBars = ScrollBars.Both;
      this.txtCiCComment.Size = new Size(501, 95);
      this.txtCiCComment.TabIndex = 42;
      this.txtCiCComment.KeyUp += new KeyEventHandler(this.txt_KeyUp);
      this.label25.AutoSize = true;
      this.label25.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label25.Location = new Point(388, 18);
      this.label25.Name = "label25";
      this.label25.Size = new Size(64, 13);
      this.label25.TabIndex = 38;
      this.label25.Text = "Comments";
      this.btnSelect.BackColor = Color.Transparent;
      this.btnSelect.Location = new Point(326, 34);
      this.btnSelect.MouseDownImage = (Image) null;
      this.btnSelect.Name = "btnSelect";
      this.btnSelect.Size = new Size(16, 16);
      this.btnSelect.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnSelect.TabIndex = 37;
      this.btnSelect.TabStop = false;
      this.btnSelect.Click += new EventHandler(this.btnSelect_Click);
      this.label10.AutoSize = true;
      this.label10.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label10.Location = new Point(13, 18);
      this.label10.Name = "label10";
      this.label10.Size = new Size(137, 13);
      this.label10.TabIndex = 0;
      this.label10.Text = "Changed Circumstance";
      this.pnlReason.Controls.Add((Control) this.changedCircumstancesChkBx);
      this.pnlReason.Controls.Add((Control) this.feeLevelIndicatorChkBx);
      this.pnlReason.Controls.Add((Control) this.lbRevisedDueDate);
      this.pnlReason.Controls.Add((Control) this.lbChangesRecievedDate);
      this.pnlReason.Controls.Add((Control) this.dpRevisedDueDate);
      this.pnlReason.Controls.Add((Control) this.dpChangesRecievedDate);
      this.pnlReason.Controls.Add((Control) this.chkReason10);
      this.pnlReason.Controls.Add((Control) this.chkReason9);
      this.pnlReason.Controls.Add((Control) this.chkReason8);
      this.pnlReason.Controls.Add((Control) this.chkReason7);
      this.pnlReason.Controls.Add((Control) this.txtReasonOther);
      this.pnlReason.Controls.Add((Control) this.chkReasonOther);
      this.pnlReason.Controls.Add((Control) this.chkReason6);
      this.pnlReason.Controls.Add((Control) this.chkReason5);
      this.pnlReason.Controls.Add((Control) this.chkReason4);
      this.pnlReason.Controls.Add((Control) this.chkReason3);
      this.pnlReason.Controls.Add((Control) this.chkReason2);
      this.pnlReason.Controls.Add((Control) this.chkReason1);
      this.pnlReason.Controls.Add((Control) this.label9);
      this.pnlReason.Dock = DockStyle.Top;
      this.pnlReason.Location = new Point(1, 1);
      this.pnlReason.Name = "pnlReason";
      this.pnlReason.Size = new Size(898, 330);
      this.pnlReason.TabIndex = 13;
      this.changedCircumstancesChkBx.AutoSize = true;
      this.changedCircumstancesChkBx.Location = new Point(373, 5);
      this.changedCircumstancesChkBx.Name = "changedCircumstancesChkBx";
      this.changedCircumstancesChkBx.Size = new Size(136, 17);
      this.changedCircumstancesChkBx.TabIndex = 29;
      this.changedCircumstancesChkBx.Text = "Changed Circumstance";
      this.changedCircumstancesChkBx.UseVisualStyleBackColor = true;
      this.feeLevelIndicatorChkBx.AutoSize = true;
      this.feeLevelIndicatorChkBx.Location = new Point(13, 5);
      this.feeLevelIndicatorChkBx.Name = "feeLevelIndicatorChkBx";
      this.feeLevelIndicatorChkBx.Size = new Size(130, 17);
      this.feeLevelIndicatorChkBx.TabIndex = 26;
      this.feeLevelIndicatorChkBx.Text = "Fee Level Disclosures";
      this.feeLevelIndicatorChkBx.UseVisualStyleBackColor = true;
      this.lbRevisedDueDate.AutoSize = true;
      this.lbRevisedDueDate.Location = new Point(370, 70);
      this.lbRevisedDueDate.Name = "lbRevisedDueDate";
      this.lbRevisedDueDate.Size = new Size(41, 13);
      this.lbRevisedDueDate.TabIndex = 25;
      this.lbRevisedDueDate.Text = "label44";
      this.lbChangesRecievedDate.AutoSize = true;
      this.lbChangesRecievedDate.Location = new Point(370, 46);
      this.lbChangesRecievedDate.Name = "lbChangesRecievedDate";
      this.lbChangesRecievedDate.Size = new Size(41, 13);
      this.lbChangesRecievedDate.TabIndex = 24;
      this.lbChangesRecievedDate.Text = "label44";
      this.dpRevisedDueDate.BackColor = SystemColors.Window;
      this.dpRevisedDueDate.Location = new Point(512, 70);
      this.dpRevisedDueDate.Margin = new Padding(4);
      this.dpRevisedDueDate.MaxValue = new DateTime(2029, 12, 31, 0, 0, 0, 0);
      this.dpRevisedDueDate.MinValue = new DateTime(2000, 1, 1, 0, 0, 0, 0);
      this.dpRevisedDueDate.Name = "dpRevisedDueDate";
      this.dpRevisedDueDate.ReadOnly = true;
      this.dpRevisedDueDate.Size = new Size(183, 21);
      this.dpRevisedDueDate.TabIndex = 23;
      this.dpRevisedDueDate.Tag = (object) "763";
      this.dpRevisedDueDate.ToolTip = "";
      this.dpRevisedDueDate.Value = new DateTime(0L);
      this.dpChangesRecievedDate.BackColor = SystemColors.Window;
      this.dpChangesRecievedDate.Location = new Point(512, 46);
      this.dpChangesRecievedDate.Margin = new Padding(4);
      this.dpChangesRecievedDate.MaxValue = new DateTime(2029, 12, 31, 0, 0, 0, 0);
      this.dpChangesRecievedDate.MinValue = new DateTime(2000, 1, 1, 0, 0, 0, 0);
      this.dpChangesRecievedDate.Name = "dpChangesRecievedDate";
      this.dpChangesRecievedDate.Size = new Size(183, 21);
      this.dpChangesRecievedDate.TabIndex = 22;
      this.dpChangesRecievedDate.Tag = (object) "763";
      this.dpChangesRecievedDate.ToolTip = "";
      this.dpChangesRecievedDate.Value = new DateTime(0L);
      this.dpChangesRecievedDate.ValueChanged += new EventHandler(this.dpChangesRecievedDate_ValueChanged);
      this.chkReason10.AutoSize = true;
      this.chkReason10.Location = new Point(13, 263);
      this.chkReason10.Name = "chkReason10";
      this.chkReason10.Size = new Size(86, 17);
      this.chkReason10.TabIndex = 21;
      this.chkReason10.Text = "checkBox10";
      this.chkReason10.UseVisualStyleBackColor = true;
      this.chkReason9.AutoSize = true;
      this.chkReason9.Location = new Point(13, 239);
      this.chkReason9.Name = "chkReason9";
      this.chkReason9.Size = new Size(80, 17);
      this.chkReason9.TabIndex = 20;
      this.chkReason9.Text = "checkBox9";
      this.chkReason9.UseVisualStyleBackColor = true;
      this.chkReason8.AutoSize = true;
      this.chkReason8.Location = new Point(13, 215);
      this.chkReason8.Name = "chkReason8";
      this.chkReason8.Size = new Size(80, 17);
      this.chkReason8.TabIndex = 19;
      this.chkReason8.Text = "checkBox8";
      this.chkReason8.UseVisualStyleBackColor = true;
      this.chkReason7.AutoSize = true;
      this.chkReason7.Location = new Point(13, 192);
      this.chkReason7.Name = "chkReason7";
      this.chkReason7.Size = new Size(80, 17);
      this.chkReason7.TabIndex = 16;
      this.chkReason7.Text = "checkBox7";
      this.chkReason7.UseVisualStyleBackColor = true;
      this.txtReasonOther.Location = new Point(71, 285);
      this.txtReasonOther.Name = "txtReasonOther";
      this.txtReasonOther.Size = new Size(184, 20);
      this.txtReasonOther.TabIndex = 18;
      this.chkReasonOther.AutoSize = true;
      this.chkReasonOther.Location = new Point(13, 286);
      this.chkReasonOther.Name = "chkReasonOther";
      this.chkReasonOther.Size = new Size(52, 17);
      this.chkReasonOther.TabIndex = 17;
      this.chkReasonOther.Text = "Other";
      this.chkReasonOther.UseVisualStyleBackColor = true;
      this.chkReasonOther.CheckedChanged += new EventHandler(this.chkReasonOther_CheckedChanged);
      this.chkReason6.AutoSize = true;
      this.chkReason6.Location = new Point(13, 169);
      this.chkReason6.Name = "chkReason6";
      this.chkReason6.Size = new Size(80, 17);
      this.chkReason6.TabIndex = 13;
      this.chkReason6.Text = "checkBox6";
      this.chkReason6.UseVisualStyleBackColor = true;
      this.chkReason5.AutoSize = true;
      this.chkReason5.Location = new Point(13, 144);
      this.chkReason5.Name = "chkReason5";
      this.chkReason5.Size = new Size(80, 17);
      this.chkReason5.TabIndex = 12;
      this.chkReason5.Text = "checkBox5";
      this.chkReason5.UseVisualStyleBackColor = true;
      this.chkReason4.AutoSize = true;
      this.chkReason4.Location = new Point(13, 122);
      this.chkReason4.Name = "chkReason4";
      this.chkReason4.Size = new Size(80, 17);
      this.chkReason4.TabIndex = 11;
      this.chkReason4.Text = "checkBox4";
      this.chkReason4.UseVisualStyleBackColor = true;
      this.chkReason3.AutoSize = true;
      this.chkReason3.Location = new Point(13, 98);
      this.chkReason3.Name = "chkReason3";
      this.chkReason3.Size = new Size(80, 17);
      this.chkReason3.TabIndex = 10;
      this.chkReason3.Text = "checkBox3";
      this.chkReason3.UseVisualStyleBackColor = true;
      this.chkReason2.AutoSize = true;
      this.chkReason2.Location = new Point(13, 74);
      this.chkReason2.Name = "chkReason2";
      this.chkReason2.Size = new Size(80, 17);
      this.chkReason2.TabIndex = 9;
      this.chkReason2.Text = "checkBox2";
      this.chkReason2.UseVisualStyleBackColor = true;
      this.chkReason1.AutoSize = true;
      this.chkReason1.Location = new Point(13, 51);
      this.chkReason1.Name = "chkReason1";
      this.chkReason1.Size = new Size(80, 17);
      this.chkReason1.TabIndex = 8;
      this.chkReason1.Text = "checkBox1";
      this.chkReason1.UseVisualStyleBackColor = true;
      this.label9.AutoSize = true;
      this.label9.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label9.Location = new Point(10, 34);
      this.label9.Name = "label9";
      this.label9.Size = new Size(50, 13);
      this.label9.TabIndex = 0;
      this.label9.Text = "Reason";
      this.gradientPanel5.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gradientPanel5.Controls.Add((Control) this.label26);
      this.gradientPanel5.Dock = DockStyle.Top;
      this.gradientPanel5.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel5.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel5.Location = new Point(0, 0);
      this.gradientPanel5.Name = "gradientPanel5";
      this.gradientPanel5.Size = new Size(900, 25);
      this.gradientPanel5.Style = GradientPanel.PanelStyle.TableFooter;
      this.gradientPanel5.TabIndex = 16;
      this.label26.AutoSize = true;
      this.label26.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label26.Location = new Point(4, 6);
      this.label26.Name = "label26";
      this.label26.Size = new Size(138, 14);
      this.label26.TabIndex = 0;
      this.label26.Text = "Reasons For Disclosure";
      this.tabPageeDisclosure.AutoScroll = true;
      this.tabPageeDisclosure.Controls.Add((Control) this.pnleDisclosureStatus);
      this.tabPageeDisclosure.Controls.Add((Control) this.pnlFulfillment);
      this.tabPageeDisclosure.Location = new Point(4, 22);
      this.tabPageeDisclosure.Margin = new Padding(1, 3, 3, 3);
      this.tabPageeDisclosure.Name = "tabPageeDisclosure";
      this.tabPageeDisclosure.Padding = new Padding(0, 3, 3, 3);
      this.tabPageeDisclosure.Size = new Size(917, 562);
      this.tabPageeDisclosure.TabIndex = 1;
      this.tabPageeDisclosure.Text = "eDisclosure Tracking";
      this.tabPageeDisclosure.UseVisualStyleBackColor = true;
      this.pnleDisclosureStatus.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
      this.pnleDisclosureStatus.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.pnleDisclosureStatus.Controls.Add((Control) this.grdDisclosureTracking);
      this.pnleDisclosureStatus.Controls.Add((Control) this.lblePackageId);
      this.pnleDisclosureStatus.Controls.Add((Control) this.label45);
      this.pnleDisclosureStatus.Controls.Add((Control) this.lblDateeDisclosureSent);
      this.pnleDisclosureStatus.Controls.Add((Control) this.lblViewForm);
      this.pnleDisclosureStatus.Controls.Add((Control) this.llViewDetails);
      this.pnleDisclosureStatus.Controls.Add((Control) this.label19);
      this.pnleDisclosureStatus.Controls.Add((Control) this.gradientPanel2);
      this.pnleDisclosureStatus.Location = new Point(3, 6);
      this.pnleDisclosureStatus.Name = "pnleDisclosureStatus";
      this.pnleDisclosureStatus.Size = new Size(735, 239);
      this.pnleDisclosureStatus.TabIndex = 0;
      this.grdDisclosureTracking.AllowMultiselect = false;
      this.grdDisclosureTracking.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
      this.grdDisclosureTracking.AutoHeight = true;
      this.grdDisclosureTracking.BorderColor = Color.LightGray;
      this.grdDisclosureTracking.BorderStyle = BorderStyle.FixedSingle;
      gvColumn9.ImageIndex = -1;
      gvColumn9.Name = "Column1";
      gvColumn9.Text = " ";
      gvColumn9.Width = 217;
      gvColumn10.ImageIndex = -1;
      gvColumn10.Name = "Recipients";
      gvColumn10.SortMethod = GVSortMethod.None;
      gvColumn10.Text = "";
      gvColumn10.Width = 250;
      this.grdDisclosureTracking.Columns.AddRange(new GVColumn[2]
      {
        gvColumn9,
        gvColumn10
      });
      this.grdDisclosureTracking.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.grdDisclosureTracking.Location = new Point(10, 74);
      this.grdDisclosureTracking.Name = "grdDisclosureTracking";
      this.grdDisclosureTracking.Size = new Size(663, 161);
      this.grdDisclosureTracking.TabIndex = 71;
      this.lblePackageId.AutoSize = true;
      this.lblePackageId.Location = new Point(190, 58);
      this.lblePackageId.Name = "lblePackageId";
      this.lblePackageId.Size = new Size(67, 13);
      this.lblePackageId.TabIndex = 70;
      this.lblePackageId.Text = "ePackageID";
      this.label45.AutoSize = true;
      this.label45.Location = new Point(6, 58);
      this.label45.Name = "label45";
      this.label45.Size = new Size(70, 13);
      this.label45.TabIndex = 69;
      this.label45.Text = "ePackage ID";
      this.lblDateeDisclosureSent.AutoSize = true;
      this.lblDateeDisclosureSent.Location = new Point(190, 36);
      this.lblDateeDisclosureSent.Name = "lblDateeDisclosureSent";
      this.lblDateeDisclosureSent.Size = new Size(107, 13);
      this.lblDateeDisclosureSent.TabIndex = 68;
      this.lblDateeDisclosureSent.Text = "eDisclosureSentDate";
      this.lblViewForm.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.lblViewForm.AutoSize = true;
      this.lblViewForm.ForeColor = SystemColors.Highlight;
      this.lblViewForm.Location = new Point(677, 143);
      this.lblViewForm.Name = "lblViewForm";
      this.lblViewForm.Size = new Size(56, 13);
      this.lblViewForm.TabIndex = 65;
      this.lblViewForm.Text = "View Form";
      this.lblViewForm.Click += new EventHandler(this.lblViewForm_Click);
      this.lblViewForm.MouseEnter += new EventHandler(this.lblViewForm_MouseEnter);
      this.lblViewForm.MouseLeave += new EventHandler(this.lblViewForm_MouseLeave);
      this.llViewDetails.AutoSize = true;
      this.llViewDetails.ForeColor = SystemColors.Highlight;
      this.llViewDetails.Location = new Point(411, 36);
      this.llViewDetails.Name = "llViewDetails";
      this.llViewDetails.Size = new Size(65, 13);
      this.llViewDetails.TabIndex = 64;
      this.llViewDetails.Text = "View Details";
      this.llViewDetails.Click += new EventHandler(this.llViewDetails_Click);
      this.llViewDetails.MouseEnter += new EventHandler(this.llViewDetails_MouseEnter);
      this.llViewDetails.MouseLeave += new EventHandler(this.llViewDetails_MouseLeave);
      this.label19.AutoSize = true;
      this.label19.Location = new Point(6, 36);
      this.label19.Name = "label19";
      this.label19.Size = new Size(92, 13);
      this.label19.TabIndex = 2;
      this.label19.Text = "eDisclosures Sent";
      this.gradientPanel2.Borders = AnchorStyles.Bottom;
      this.gradientPanel2.Controls.Add((Control) this.label14);
      this.gradientPanel2.Dock = DockStyle.Top;
      this.gradientPanel2.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel2.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel2.Location = new Point(1, 1);
      this.gradientPanel2.Name = "gradientPanel2";
      this.gradientPanel2.Size = new Size(733, 25);
      this.gradientPanel2.Style = GradientPanel.PanelStyle.TableFooter;
      this.gradientPanel2.TabIndex = 1;
      this.label14.AutoSize = true;
      this.label14.BackColor = Color.Transparent;
      this.label14.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label14.Location = new Point(5, 5);
      this.label14.Name = "label14";
      this.label14.Size = new Size(42, 14);
      this.label14.TabIndex = 1;
      this.label14.Text = "Status";
      this.pnlFulfillment.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.pnlFulfillment.Controls.Add((Control) this.txtTrackingNumber);
      this.pnlFulfillment.Controls.Add((Control) this.label2);
      this.pnlFulfillment.Controls.Add((Control) this.dpActualFulfillmentDate);
      this.pnlFulfillment.Controls.Add((Control) this.dpPresumedFulfillmentDate);
      this.pnlFulfillment.Controls.Add((Control) this.label18);
      this.pnlFulfillment.Controls.Add((Control) this.label27);
      this.pnlFulfillment.Controls.Add((Control) this.txtFulfillmentComments);
      this.pnlFulfillment.Controls.Add((Control) this.label43);
      this.pnlFulfillment.Controls.Add((Control) this.txtFulfillmentMethod);
      this.pnlFulfillment.Controls.Add((Control) this.label31);
      this.pnlFulfillment.Controls.Add((Control) this.txtDateFulfillOrder);
      this.pnlFulfillment.Controls.Add((Control) this.txtFulfillmentOrderBy);
      this.pnlFulfillment.Controls.Add((Control) this.label30);
      this.pnlFulfillment.Controls.Add((Control) this.label29);
      this.pnlFulfillment.Controls.Add((Control) this.gradientPanel4);
      this.pnlFulfillment.Location = new Point(4, 281);
      this.pnlFulfillment.Name = "pnlFulfillment";
      this.pnlFulfillment.Size = new Size(734, 266);
      this.pnlFulfillment.TabIndex = 1;
      this.dpActualFulfillmentDate.BackColor = SystemColors.Window;
      this.dpActualFulfillmentDate.Enabled = false;
      this.dpActualFulfillmentDate.Location = new Point(149, 143);
      this.dpActualFulfillmentDate.Margin = new Padding(4);
      this.dpActualFulfillmentDate.MaxValue = new DateTime(2029, 12, 31, 0, 0, 0, 0);
      this.dpActualFulfillmentDate.MinValue = new DateTime(2000, 1, 1, 0, 0, 0, 0);
      this.dpActualFulfillmentDate.Name = "dpActualFulfillmentDate";
      this.dpActualFulfillmentDate.Size = new Size(258, 21);
      this.dpActualFulfillmentDate.TabIndex = 68;
      this.dpActualFulfillmentDate.Tag = (object) "763";
      this.dpActualFulfillmentDate.ToolTip = "";
      this.dpActualFulfillmentDate.Value = new DateTime(0L);
      this.dpPresumedFulfillmentDate.BackColor = SystemColors.Window;
      this.dpPresumedFulfillmentDate.Location = new Point(149, 120);
      this.dpPresumedFulfillmentDate.Margin = new Padding(4);
      this.dpPresumedFulfillmentDate.MaxValue = new DateTime(2029, 12, 31, 0, 0, 0, 0);
      this.dpPresumedFulfillmentDate.MinValue = new DateTime(2000, 1, 1, 0, 0, 0, 0);
      this.dpPresumedFulfillmentDate.Name = "dpPresumedFulfillmentDate";
      this.dpPresumedFulfillmentDate.ReadOnly = true;
      this.dpPresumedFulfillmentDate.Size = new Size(258, 21);
      this.dpPresumedFulfillmentDate.TabIndex = 67;
      this.dpPresumedFulfillmentDate.Tag = (object) "763";
      this.dpPresumedFulfillmentDate.ToolTip = "";
      this.dpPresumedFulfillmentDate.Value = new DateTime(0L);
      this.label18.AutoSize = true;
      this.label18.Location = new Point(6, 147);
      this.label18.Name = "label18";
      this.label18.Size = new Size(112, 13);
      this.label18.TabIndex = 70;
      this.label18.Text = "Actual Received Date";
      this.label27.AutoSize = true;
      this.label27.Location = new Point(6, 125);
      this.label27.Name = "label27";
      this.label27.Size = new Size(129, 13);
      this.label27.TabIndex = 69;
      this.label27.Text = "Presumed Received Date";
      this.txtFulfillmentComments.Location = new Point(149, 166);
      this.txtFulfillmentComments.Multiline = true;
      this.txtFulfillmentComments.Name = "txtFulfillmentComments";
      this.txtFulfillmentComments.ReadOnly = true;
      this.txtFulfillmentComments.Size = new Size(258, 76);
      this.txtFulfillmentComments.TabIndex = 66;
      this.label43.AutoSize = true;
      this.label43.Location = new Point(5, 169);
      this.label43.Name = "label43";
      this.label43.Size = new Size(56, 13);
      this.label43.TabIndex = 65;
      this.label43.Text = "Comments";
      this.txtFulfillmentMethod.Location = new Point(149, 77);
      this.txtFulfillmentMethod.Name = "txtFulfillmentMethod";
      this.txtFulfillmentMethod.ReadOnly = true;
      this.txtFulfillmentMethod.Size = new Size(258, 20);
      this.txtFulfillmentMethod.TabIndex = 64;
      this.label31.AutoSize = true;
      this.label31.Location = new Point(5, 79);
      this.label31.Name = "label31";
      this.label31.Size = new Size(92, 13);
      this.label31.TabIndex = 63;
      this.label31.Text = "Fulfillment Method";
      this.txtDateFulfillOrder.Location = new Point(149, 53);
      this.txtDateFulfillOrder.Name = "txtDateFulfillOrder";
      this.txtDateFulfillOrder.ReadOnly = true;
      this.txtDateFulfillOrder.Size = new Size(258, 20);
      this.txtDateFulfillOrder.TabIndex = 62;
      this.txtFulfillmentOrderBy.Location = new Point(149, 32);
      this.txtFulfillmentOrderBy.Name = "txtFulfillmentOrderBy";
      this.txtFulfillmentOrderBy.ReadOnly = true;
      this.txtFulfillmentOrderBy.Size = new Size(258, 20);
      this.txtFulfillmentOrderBy.TabIndex = 23;
      this.label30.AutoSize = true;
      this.label30.Location = new Point(5, 57);
      this.label30.Name = "label30";
      this.label30.Size = new Size(96, 13);
      this.label30.TabIndex = 4;
      this.label30.Text = "Date/Time Fulfilled";
      this.label29.AutoSize = true;
      this.label29.Location = new Point(5, 35);
      this.label29.Name = "label29";
      this.label29.Size = new Size(56, 13);
      this.label29.TabIndex = 3;
      this.label29.Text = "Fulfilled by";
      this.gradientPanel4.Borders = AnchorStyles.Bottom;
      this.gradientPanel4.Controls.Add((Control) this.btneDiscManualFulfill);
      this.gradientPanel4.Controls.Add((Control) this.label28);
      this.gradientPanel4.Dock = DockStyle.Top;
      this.gradientPanel4.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel4.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel4.Location = new Point(1, 1);
      this.gradientPanel4.Name = "gradientPanel4";
      this.gradientPanel4.Size = new Size(732, 25);
      this.gradientPanel4.Style = GradientPanel.PanelStyle.TableFooter;
      this.gradientPanel4.TabIndex = 2;
      this.btneDiscManualFulfill.Location = new Point(531, 1);
      this.btneDiscManualFulfill.Name = "btneDiscManualFulfill";
      this.btneDiscManualFulfill.Size = new Size(195, 22);
      this.btneDiscManualFulfill.TabIndex = 2;
      this.btneDiscManualFulfill.Text = "Print Documents and Manually Fulfill";
      this.btneDiscManualFulfill.UseVisualStyleBackColor = true;
      this.btneDiscManualFulfill.Visible = false;
      this.btneDiscManualFulfill.Click += new EventHandler(this.btneDiscManualFulfill_Click);
      this.label28.AutoSize = true;
      this.label28.BackColor = Color.Transparent;
      this.label28.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label28.Location = new Point(5, 5);
      this.label28.Name = "label28";
      this.label28.Size = new Size(65, 14);
      this.label28.TabIndex = 1;
      this.label28.Text = "Fulfillment";
      this.ddRecipient.FormattingEnabled = true;
      this.ddRecipient.Location = new Point(3, 0);
      this.ddRecipient.Margin = new Padding(3, 6, 3, 3);
      this.ddRecipient.Name = "ddRecipient";
      this.ddRecipient.Size = new Size(254, 21);
      this.ddRecipient.TabIndex = 12;
      this.ddRecipient.SelectedIndexChanged += new EventHandler(this.ddRecipient_SelectedIndexChanged);
      this.detailsDisclosureRecipientLbl.AutoSize = true;
      this.detailsDisclosureRecipientLbl.Location = new Point(3, 6);
      this.detailsDisclosureRecipientLbl.Margin = new Padding(3, 6, 3, 0);
      this.detailsDisclosureRecipientLbl.Name = "detailsDisclosureRecipientLbl";
      this.detailsDisclosureRecipientLbl.Size = new Size(104, 13);
      this.detailsDisclosureRecipientLbl.TabIndex = 11;
      this.detailsDisclosureRecipientLbl.Text = "Disclosure Recipient";
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Location = new Point(763, 7);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 5;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.tableLayoutPanel2.ColumnCount = 2;
      this.tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 11.84529f));
      this.tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 88.15472f));
      this.tableLayoutPanel2.Controls.Add((Control) this.panel3, 1, 0);
      this.tableLayoutPanel2.Controls.Add((Control) this.tcDisclosure, 0, 1);
      this.tableLayoutPanel2.Controls.Add((Control) this.detailsDisclosureRecipientLbl, 0, 0);
      this.tableLayoutPanel2.Dock = DockStyle.Fill;
      this.tableLayoutPanel2.Location = new Point(3, 3);
      this.tableLayoutPanel2.Margin = new Padding(2);
      this.tableLayoutPanel2.Name = "tableLayoutPanel2";
      this.tableLayoutPanel2.RowCount = 2;
      this.tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 4.693612f));
      this.tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 95.30639f));
      this.tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 16f));
      this.tableLayoutPanel2.Size = new Size(931, 623);
      this.tableLayoutPanel2.TabIndex = 1;
      this.panel3.Controls.Add((Control) this.btnAuditTrail);
      this.panel3.Controls.Add((Control) this.lblViewSummary);
      this.panel3.Controls.Add((Control) this.ddRecipient);
      this.panel3.Dock = DockStyle.Fill;
      this.panel3.Location = new Point(113, 3);
      this.panel3.Name = "panel3";
      this.panel3.Size = new Size(815, 23);
      this.panel3.TabIndex = 63;
      this.btnAuditTrail.Location = new Point(355, 0);
      this.btnAuditTrail.Name = "btnAuditTrail";
      this.btnAuditTrail.Size = new Size(105, 23);
      this.btnAuditTrail.TabIndex = 66;
      this.btnAuditTrail.Text = "View Audit Trail";
      this.btnAuditTrail.UseVisualStyleBackColor = true;
      this.btnAuditTrail.Click += new EventHandler(this.btnAuditTrail_Click);
      this.lblViewSummary.AutoSize = true;
      this.lblViewSummary.ForeColor = SystemColors.Highlight;
      this.lblViewSummary.Location = new Point(263, 3);
      this.lblViewSummary.Name = "lblViewSummary";
      this.lblViewSummary.Size = new Size(76, 13);
      this.lblViewSummary.TabIndex = 65;
      this.lblViewSummary.Text = "View Summary";
      this.lblViewSummary.Click += new EventHandler(this.lblViewSummary_Click);
      this.lblViewSummary.MouseEnter += new EventHandler(this.lblViewSummary_MouseEnter);
      this.lblViewSummary.MouseLeave += new EventHandler(this.lblViewSummary_MouseLeave);
      this.btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnClose.Location = new Point(841, 7);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new Size(75, 23);
      this.btnClose.TabIndex = 6;
      this.btnClose.Text = "Cancel";
      this.btnClose.UseVisualStyleBackColor = true;
      this.btnClose.Click += new EventHandler(this.btnClose_Click);
      this.pnlBottom.Controls.Add((Control) this.btnOK);
      this.pnlBottom.Controls.Add((Control) this.btnClose);
      this.pnlBottom.Dock = DockStyle.Bottom;
      this.pnlBottom.Location = new Point(3, 626);
      this.pnlBottom.Name = "pnlBottom";
      this.pnlBottom.Size = new Size(931, 35);
      this.pnlBottom.TabIndex = 7;
      this.txtTrackingNumber.Location = new Point(149, 99);
      this.txtTrackingNumber.Name = "txtTrackingNumber";
      this.txtTrackingNumber.ReadOnly = true;
      this.txtTrackingNumber.Size = new Size(258, 20);
      this.txtTrackingNumber.TabIndex = 72;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(5, 101);
      this.label2.Name = "label2";
      this.label2.Size = new Size(89, 13);
      this.label2.TabIndex = 71;
      this.label2.Text = "Tracking Number";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.AutoScroll = true;
      this.ClientSize = new Size(934, 661);
      this.Controls.Add((Control) this.tableLayoutPanel2);
      this.Controls.Add((Control) this.pnlBottom);
      this.MinimizeBox = false;
      this.Name = nameof (DisclosureDetailsDialogEnhanced);
      this.Padding = new Padding(3, 3, 0, 0);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Disclosure Details";
      this.tcDisclosure.ResumeLayout(false);
      this.tabPageDetail.ResumeLayout(false);
      this.detailsPanel.ResumeLayout(false);
      this.pnlLoanSnapshot.ResumeLayout(false);
      this.pnlLoanSnapshot.PerformLayout();
      this.gcDocList.ResumeLayout(false);
      this.gradientPanel3.ResumeLayout(false);
      this.gradientPanel3.PerformLayout();
      this.flowLayoutPanel2.ResumeLayout(false);
      this.pnlBasicInfo.ResumeLayout(false);
      this.tableLayoutPanel1.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.pnlIntent.ResumeLayout(false);
      this.pnlIntent.PerformLayout();
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      this.pnlUserId.ResumeLayout(false);
      this.pnlUserId.PerformLayout();
      this.gradientPanel1.ResumeLayout(false);
      this.gradientPanel1.PerformLayout();
      this.tabPageReasons.ResumeLayout(false);
      this.reasonsPanel.ResumeLayout(false);
      this.borderPanel1.ResumeLayout(false);
      this.panel5.ResumeLayout(false);
      this.panel5.PerformLayout();
      ((ISupportInitialize) this.btnSelect).EndInit();
      this.pnlReason.ResumeLayout(false);
      this.pnlReason.PerformLayout();
      this.gradientPanel5.ResumeLayout(false);
      this.gradientPanel5.PerformLayout();
      this.tabPageeDisclosure.ResumeLayout(false);
      this.pnleDisclosureStatus.ResumeLayout(false);
      this.pnleDisclosureStatus.PerformLayout();
      this.gradientPanel2.ResumeLayout(false);
      this.gradientPanel2.PerformLayout();
      this.pnlFulfillment.ResumeLayout(false);
      this.pnlFulfillment.PerformLayout();
      this.gradientPanel4.ResumeLayout(false);
      this.gradientPanel4.PerformLayout();
      this.tableLayoutPanel2.ResumeLayout(false);
      this.tableLayoutPanel2.PerformLayout();
      this.panel3.ResumeLayout(false);
      this.panel3.PerformLayout();
      this.pnlBottom.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
