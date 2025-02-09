// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.DisclosureDetailsDialog2015
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.CalculationEngine;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.InputEngine;
using EllieMae.EMLite.JedScriptEngine;
using EllieMae.EMLite.LoanUtils.EDelivery;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
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
  public class DisclosureDetailsDialog2015 : Form
  {
    private const string className = "DisclosureDetailsDialog2015";
    private static readonly string sw = Tracing.SwInputEngine;
    private const string AutomaticFullfillmentServiceName = "Encompass Fulfillment Service";
    private LoanData loanData;
    private bool suspendEvent;
    private DisclosureTracking2015Log currentLog;
    private string[] sentMethod = new string[6]
    {
      "In Person",
      "Phone",
      "Email",
      "Signature",
      "Other",
      "eFolder eDisclosures"
    };
    private string[] discloseMethod = new string[7]
    {
      "U.S. Mail",
      "In Person",
      "Email",
      "eFolder eDisclosures",
      "Fax",
      "Other",
      "Closing Docs Order"
    };
    private string[] borrowerType = new string[11]
    {
      "",
      "Individual",
      "Co-signer",
      "Title Only",
      "Non Title Spouse",
      "Trustee",
      "Title Only Trustee",
      "Settlor Trustee",
      "Settlor",
      "Title Only Settlor Trustee",
      "Officer"
    };
    private string[] nboType = new string[4]
    {
      "Title only",
      "Non Title Spouse",
      "Title Only Trustee",
      "Title Only Settlor Trustee"
    };
    private bool hasAccessRight = true;
    private bool canEditSentDateAndExternalField;
    private bool intermediateData;
    private bool hasManualFulfillmentPermission;
    private bool isFulfillmentServiceEnabled;
    private bool isReasonsChkBoxEnabled;
    private bool fulfillmentUpdated;
    private string eDisclosureManuallyFulfilledBy = "";
    private DateTime eDisclosureManualFulfillmentDate;
    private DateTime eDisclosurePresumedDate;
    private DateTime eDisclosureActualDate;
    private DisclosureTrackingBase.DisclosedMethod eDisclosureManualFulfillmentMethod;
    private string eDisclosureManualFulfillmentComment = "";
    private Dictionary<int, string> discRecipients = new Dictionary<int, string>();
    private int discRecipientSelectedIndex;
    private string nboInstance = "";
    private bool hasNBOs;
    private Dictionary<string, INonBorrowerOwnerItem> origNBOItems = new Dictionary<string, INonBorrowerOwnerItem>();
    private Dictionary<string, INonBorrowerOwnerItem> curNBOItems = new Dictionary<string, INonBorrowerOwnerItem>();
    private readonly string timezoneInfoStr;
    private DateTime borPre;
    private DateTime borAct;
    private DateTime coborPre;
    private DateTime coborAct;
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
    private TextBox txtCoBorrowerName;
    private TextBox txtPropertyAddress;
    private TextBox txtBorrowerName;
    private Label label37;
    private Label label36;
    private Label label35;
    private Label label34;
    private Label label33;
    private Label label32;
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
    private Button btnClose;
    private FieldLockButton lbtnDisclosedDailyInterest;
    private TextBox txtDisclosedDailyInterest;
    private Label label1;
    private GridView grdDisclosureTracking;
    private Label lblDateeDisclosureSent;
    private TableLayoutPanel tableLayoutPanel1;
    private Panel panel1;
    private ComboBox cmbBorrowerReceivedMethod;
    private Label label3;
    private ComboBox cmbDisclosureType;
    private TextBox txtDisclosedBy;
    private Label label2;
    private FieldLockButton lbtnDisclosedBy;
    private FieldLockButton lbtnSentDate;
    private TextBox txtDisclosedMethod;
    private Label lblDisclosurePresumedReceivedDate;
    private DateTimePicker dtDisclosedDate;
    private ComboBox cmbDisclosedMethod;
    private Label label17;
    private Label label16;
    private Label label15;
    private DatePicker dpBorrowerActualReceivedDate;
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
    private Label label11;
    private Label label12;
    private Label label13;
    private Label label21;
    private DatePicker dpCoBorrowerActualReceivedDate;
    private Label label22;
    private ComboBox cmbCoBorrowerReceivedMethod;
    private Label label23;
    private Label label24;
    private DatePicker dpCoBorrowerReceivedDate;
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
    private TextBox txtCoBorrowerReceivedMethod;
    private TextBox txtBorrowerReceivedMethod;
    private Button btnOK;
    private TextBox txtIntentToProceedMethod;
    private TextBox txtMethodOther;
    private TextBox txtIntentMethodOther;
    private TextBox txtCoBorrowerOther;
    private TextBox txtBorrowerOther;
    private GradientPanel gradientPanel5;
    private Label label26;
    private BorderPanel borderPanel1;
    private Panel pnlBottom;
    private FieldLockButton lBtnIntentReceivedBy;
    private TextBox txtBorrowerType;
    private TextBox txtCoBorrowerType;
    private TextBox txtChangedCircumstance;
    private FieldLockButton lBtnBorrowerPresumed;
    private FieldLockButton lBtnCoBorrowerPresumed;
    private Button btnItemSnapshot;
    private DatePicker dpActualFulfillmentDate;
    private Label label18;
    private Label label27;
    private DatePicker dpPresumedFulfillmentDate;
    private Label lbRevisedDueDate;
    private Label lbChangesRecievedDate;
    private DatePicker dpRevisedDueDate;
    private DatePicker dpChangesRecievedDate;
    private ComboBox cmbCoBorrowerType;
    private ComboBox cmbBorrowerType;
    private FieldLockButton lBtnCoBorrowerType;
    private FieldLockButton lBtnBorrowerType;
    private Label lblePackageId;
    private Label label45;
    private CheckBox changedCircumstancesChkBx;
    private CheckBox feeLevelIndicatorChkBx;
    private ComboBox detailsDisclosureRecipientDropDown;
    private Label detailsDisclosureRecipientLbl;
    private ComboBox dtDisclosureRecipientDropDown;
    private Label dtDisclosureRecipientLbl;
    private DatePicker dpBorrowerReceivedDate;
    private TextBox txtNBOOther;
    private TextBox txtNBOReceivedMethod;
    private FieldLockButton lBtnNBOType;
    private ComboBox cmbNBOType;
    private DatePicker dpNBOActualReceivedDate;
    private FieldLockButton lBtnNBOPresumed;
    private DatePicker dpNBOReceivedDate;
    private TextBox txtNBOType;
    private ComboBox cmbNBOReceivedMethod;
    private GridView grdNBODisclosureTracking;
    private Label label44;
    private Label label46;
    private Button btnEsign;
    private Label feeDetailsLabel;
    private GridView feeDetailsGV;
    private Panel detailsPanel;
    private Panel reasonsPanel;
    private CheckBox chkUseforUCDExport;
    private Label lblUseForUCDExport;
    private Button btnAuditTrail1;
    private Button btnAuditTrail2;
    private TextBox txtTrackingNumber;
    private Label label47;

    public bool IsLinkedLoan
    {
      get => this.loanData != null && this.loanData.LinkSyncType == LinkSyncType.ConstructionLinked;
    }

    public bool IsConstructionPrimaryLoan
    {
      get
      {
        return this.loanData != null && this.loanData.LinkSyncType == LinkSyncType.ConstructionPrimary;
      }
    }

    public DisclosureDetailsDialog2015(DisclosureTracking2015Log currentLog, bool hasAccessRight)
    {
      this.hasAccessRight = hasAccessRight;
      this.currentLog = currentLog;
      this.hasNBOs = currentLog.IsNboExist;
      if (this.hasNBOs)
        this.CopyNBOItems();
      this.InitializeComponent();
      this.applySecurity();
      this.loanData = Session.LoanData;
      string str = this.loanData.GetField("LE1.XG9") == "" ? this.loanData.GetField("LE1.X9") : this.loanData.GetField("LE1.XG9");
      this.timezoneInfoStr = this.currentLog.UseLE1X9ForTimeZone(this.loanData) ? str : "PST";
      this.suspendEvent = true;
      this.initControls();
      this.suspendEvent = false;
      this.AutoScroll = true;
      this.panel5.AutoScroll = true;
      this.refreshDisclosure();
      this.calculateDisclosedDate();
      this.DisableControlsForLinkedLoans();
    }

    private void CopyNBOItems()
    {
      foreach (KeyValuePair<string, INonBorrowerOwnerItem> allnboItem in this.currentLog.GetAllnboItems())
        this.curNBOItems.Add(allnboItem.Key, allnboItem.Value.CloneForDuplicate());
    }

    private string getTimeZoneDisplayForDate(DateTime dt)
    {
      if (!this.currentLog.UseLE1X9ForTimeZone(this.loanData))
        return "PST";
      string field = this.loanData.GetField("LE1.XG9");
      if (field == "")
        return this.loanData.GetField("LE1.X9");
      bool isDaylightSavingTime = false;
      if (dt != DateTime.MinValue)
        isDaylightSavingTime = System.TimeZoneInfo.Local.IsDaylightSavingTime(dt);
      return Utils.TransformTimezoneToStandardTimezone(field, isDaylightSavingTime);
    }

    private void PopulateDisclosureRecipients()
    {
      this.detailsDisclosureRecipientLbl.Visible = true;
      this.detailsDisclosureRecipientDropDown.Visible = true;
      this.dtDisclosureRecipientLbl.Visible = true;
      this.dtDisclosureRecipientDropDown.Visible = true;
      this.pnleDisclosureStatus.Location = new Point(1, 41);
      this.gradientPanel1.Dock = DockStyle.None;
      this.gradientPanel1.Location = new Point(1, 36);
      this.tableLayoutPanel1.Dock = DockStyle.None;
      this.tableLayoutPanel1.Location = new Point(1, 63);
      int key1 = 0;
      string str1 = this.currentLog.BorrowerName + " " + this.currentLog.CoBorrowerName;
      this.discRecipients.Add(key1, "Default");
      int key2 = key1 + 1;
      this.detailsDisclosureRecipientDropDown.Items.Add((object) str1);
      this.detailsDisclosureRecipientDropDown.SelectedIndexChanged -= new EventHandler(this.detailsDisclosureRecipientDropDown_SelectedIndexChanged);
      this.detailsDisclosureRecipientDropDown.SelectedItem = (object) str1;
      this.detailsDisclosureRecipientDropDown.SelectedIndexChanged += new EventHandler(this.detailsDisclosureRecipientDropDown_SelectedIndexChanged);
      this.dtDisclosureRecipientDropDown.Items.Add((object) str1);
      this.dtDisclosureRecipientDropDown.SelectedIndexChanged -= new EventHandler(this.dtDisclosureRecipientDropDown_SelectedIndexChanged);
      this.dtDisclosureRecipientDropDown.SelectedItem = (object) str1;
      this.dtDisclosureRecipientDropDown.SelectedIndexChanged += new EventHandler(this.dtDisclosureRecipientDropDown_SelectedIndexChanged);
      foreach (KeyValuePair<string, INonBorrowerOwnerItem> allnboItem in this.currentLog.GetAllnboItems())
      {
        string key3 = allnboItem.Key;
        INonBorrowerOwnerItem borrowerOwnerItem = allnboItem.Value;
        string str2 = string.IsNullOrWhiteSpace(allnboItem.Value.MidName) ? " " : " " + allnboItem.Value.MidName + " ";
        string str3 = allnboItem.Value.FirstName + str2 + allnboItem.Value.LastName;
        this.detailsDisclosureRecipientDropDown.Items.Add((object) str3);
        this.dtDisclosureRecipientDropDown.Items.Add((object) str3);
        this.discRecipients.Add(key2, allnboItem.Key);
        ++key2;
      }
    }

    private void showCoBorrower(bool show)
    {
      if (show)
      {
        this.label21.Visible = true;
        this.label23.Visible = true;
        this.cmbCoBorrowerReceivedMethod.Visible = true;
        this.txtCoBorrowerOther.Visible = true;
        this.label24.Visible = true;
        this.dpCoBorrowerReceivedDate.Visible = true;
        this.lBtnCoBorrowerPresumed.Visible = true;
        this.dpCoBorrowerActualReceivedDate.Visible = true;
        this.label22.Visible = true;
        this.label13.Visible = true;
        this.lBtnCoBorrowerType.Visible = true;
        if (this.currentLog.IsCoBorrowerTypeLocked)
        {
          this.cmbCoBorrowerType.Visible = true;
          this.txtCoBorrowerType.Visible = false;
        }
        else
        {
          this.cmbCoBorrowerType.Visible = false;
          this.txtCoBorrowerType.Visible = true;
        }
        if (this.currentLog.CoBorrowerDisclosedMethod != DisclosureTrackingBase.DisclosedMethod.eDisclosure)
          return;
        this.txtCoBorrowerReceivedMethod.Visible = true;
        this.cmbCoBorrowerReceivedMethod.Visible = false;
        this.txtCoBorrowerReceivedMethod.BringToFront();
        this.txtCoBorrowerOther.Text = this.currentLog.CoBorrowerFulfillmentMethodDescription;
        this.txtCoBorrowerOther.ReadOnly = true;
      }
      else
      {
        this.label21.Visible = false;
        this.label23.Visible = false;
        this.cmbCoBorrowerReceivedMethod.Visible = false;
        this.txtCoBorrowerReceivedMethod.Visible = false;
        this.txtCoBorrowerOther.Visible = false;
        this.label24.Visible = false;
        this.dpCoBorrowerReceivedDate.Visible = false;
        this.lBtnCoBorrowerPresumed.Visible = false;
        this.dpCoBorrowerActualReceivedDate.Visible = false;
        this.label22.Visible = false;
        this.dpCoBorrowerReceivedDate.Visible = false;
        this.cmbCoBorrowerType.Visible = false;
        this.label13.Visible = false;
        this.lBtnCoBorrowerType.Visible = false;
        this.txtCoBorrowerType.Visible = false;
      }
    }

    private void DisableControlsForLinkedLoans()
    {
      if (!this.IsLinkedLoan)
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

    private void applySecurity()
    {
      FeaturesAclManager aclManager = (FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features);
      this.canEditSentDateAndExternalField = aclManager.GetUserApplicationRight(AclFeature.ToolsTab_DT_ChangeSentDate);
      this.hasManualFulfillmentPermission = aclManager.GetUserApplicationRight(AclFeature.ToolsTab_DT_ManualFulfillment);
      this.isFulfillmentServiceEnabled = Session.ConfigurationManager.GetCompanySetting("Fulfillment", "ServiceEnabled") == "Y";
      this.isReasonsChkBoxEnabled = aclManager.GetUserApplicationRight(AclFeature.ToolsTab_DT_ChangeReasons);
      this.changedCircumstancesChkBx.Enabled = aclManager.GetUserApplicationRight(AclFeature.ToolsTab_DT_ChangeReasons);
    }

    private DateTime getReceivedDate(DisclosureTracking2015Log updatedLog)
    {
      DateTime dateTime = new DateTime();
      DateTime receivedDate = this.dpBorrowerActualReceivedDate.Value;
      if (this.dpBorrowerReceivedDate.Value != DateTime.MinValue && (this.dpBorrowerReceivedDate.Value < receivedDate || receivedDate == DateTime.MinValue))
        receivedDate = this.dpBorrowerReceivedDate.Value;
      if (this.dpCoBorrowerActualReceivedDate.Value != DateTime.MinValue && (this.dpCoBorrowerActualReceivedDate.Value < receivedDate || receivedDate == DateTime.MinValue))
        receivedDate = this.dpCoBorrowerActualReceivedDate.Value;
      if (this.dpCoBorrowerReceivedDate.Value != DateTime.MinValue && (this.dpCoBorrowerReceivedDate.Value < receivedDate || receivedDate == DateTime.MinValue))
        receivedDate = this.dpCoBorrowerReceivedDate.Value;
      return receivedDate;
    }

    private void viewAuditTrail()
    {
      EllieMae.EMLite.Log.AuditTrail.AuditTrail.ViewAuditTrail(Session.LoanDataMgr.LoanData.GUID, this.currentLog.eDisclosurePackageID);
    }

    private void refreshIntentToProceed(
      DateTime borPre,
      DateTime borAct,
      DateTime coborPre,
      DateTime coborAct)
    {
      this.lBtnIntentReceivedBy.Enabled = this.lBtnCoBorrowerPresumed.Enabled = this.lBtnCoBorrowerType.Enabled = this.canEditSentDateAndExternalField;
      this.chkIntent.Enabled = this.dpIntentDate.Enabled = this.txtIntentToProceedMethod.Enabled = this.cmbIntentReceivedMethod.Enabled = this.txtIntentMethodOther.Enabled = this.txtIntentComments.Enabled = this.canEditSentDateAndExternalField;
      DateTime receivedDate = this.getReceivedDate(this.currentLog);
      DateTime loanTimeZone = this.currentLog.ConvertToLoanTimeZone(DateTime.Now);
      if (this.chkIntent.Checked && receivedDate > loanTimeZone)
      {
        if (Utils.Dialog((IWin32Window) this, "Changing this date will nullify the current Intent to Proceed and Earliest Fee Collection dates. Do you still want to proceed?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
        {
          this.chkIntent.Checked = false;
        }
        else
        {
          this.dpBorrowerReceivedDate.Value = borPre;
          this.dpBorrowerActualReceivedDate.Value = borAct;
          this.dpCoBorrowerReceivedDate.Value = coborPre;
          this.dpCoBorrowerActualReceivedDate.Value = coborAct;
        }
      }
      if (receivedDate != DateTime.MinValue)
      {
        this.dpIntentDate.MinDate = receivedDate;
        this.chkIntent.Enabled = this.currentLog.IsDisclosed && this.canEditSentDateAndExternalField;
        if (receivedDate > loanTimeZone)
          this.chkIntent.Enabled = false;
      }
      else
        this.chkIntent.Checked = this.chkIntent.Enabled = false;
      foreach (IDisclosureTracking2015Log disclosureTracking2015Log in Session.LoanData.GetLogList().GetAllIDisclosureTracking2015Log(true))
      {
        if (disclosureTracking2015Log.DisclosedForLE && disclosureTracking2015Log.IntentToProceed && disclosureTracking2015Log != this.currentLog)
        {
          this.chkIntent.Enabled = false;
          break;
        }
      }
    }

    private void initControls()
    {
      this.dpIntentDate.Format = DateTimePickerFormat.Custom;
      this.dpIntentDate.CustomFormat = "MM/dd/yyyy";
      this.dtDisclosedDate.Format = DateTimePickerFormat.Custom;
      this.dtDisclosedDate.CustomFormat = "MM/dd/yyyy";
      this.dpBorrowerReceivedDate.MinValue = this.dpCoBorrowerReceivedDate.MinValue = this.currentLog.DisclosedDate;
      this.cmbDisclosedMethod.Items.Clear();
      this.cmbIntentReceivedMethod.Items.Clear();
      this.cmbBorrowerReceivedMethod.Items.Clear();
      this.cmbCoBorrowerReceivedMethod.Items.Clear();
      this.cmbNBOReceivedMethod.Items.Clear();
      this.cmbIntentReceivedMethod.Items.Add((object) "");
      for (int index = 0; index < this.discloseMethod.Length; ++index)
      {
        if (this.discloseMethod[index] != "eFolder eDisclosures" && this.discloseMethod[index] != "Closing Docs Order")
        {
          this.cmbDisclosedMethod.Items.Add((object) this.discloseMethod[index]);
          this.cmbBorrowerReceivedMethod.Items.Add((object) this.discloseMethod[index]);
          this.cmbCoBorrowerReceivedMethod.Items.Add((object) this.discloseMethod[index]);
          this.cmbNBOReceivedMethod.Items.Add((object) this.discloseMethod[index]);
        }
      }
      for (int index = 0; index < this.sentMethod.Length; ++index)
      {
        if (this.sentMethod[index] != "eFolder eDisclosures" && this.discloseMethod[index] != "Closing Docs Order")
          this.cmbIntentReceivedMethod.Items.Add((object) this.sentMethod[index]);
      }
      for (int index = 0; index < this.borrowerType.Length; ++index)
      {
        this.cmbBorrowerType.Items.Add((object) this.borrowerType[index]);
        this.cmbCoBorrowerType.Items.Add((object) this.borrowerType[index]);
      }
      for (int index = 0; index < this.nboType.Length; ++index)
        this.cmbNBOType.Items.Add((object) this.nboType[index]);
      if (this.currentLog.CoBorrowerName.Trim() == "")
      {
        this.cmbCoBorrowerReceivedMethod.Enabled = false;
        this.txtCoBorrowerReceivedMethod.ReadOnly = true;
        this.dpCoBorrowerActualReceivedDate.ReadOnly = true;
        this.dpCoBorrowerReceivedDate.ReadOnly = true;
        this.txtCoBorrowerOther.ReadOnly = true;
        this.txtCoBorrowerType.ReadOnly = true;
        this.lBtnCoBorrowerPresumed.Enabled = false;
        this.lBtnCoBorrowerType.Enabled = false;
      }
      bool flag = Session.LoanDataMgr.IsPlatformLoan();
      this.btnEsign.Visible = flag;
      if (flag && Session.UserInfo.Userid.Equals(this.currentLog.eDisclosureLOUserId) && this.currentLog.eDisclosureLOeSignedDate == DateTime.MinValue)
        this.btnEsign.Enabled = true;
      else
        this.btnEsign.Enabled = false;
      this.lblUseForUCDExport.Visible = this.chkUseforUCDExport.Visible = this.currentLog.DisclosedForCD;
      this.lblUseForUCDExport.Enabled = this.chkUseforUCDExport.Enabled = this.currentLog.IsDisclosed;
      this.btnAuditTrail1.Visible = flag && !string.IsNullOrEmpty(this.currentLog.eDisclosurePackageID);
      this.btnAuditTrail2.Visible = flag && !string.IsNullOrEmpty(this.currentLog.eDisclosurePackageID);
    }

    private void setButtons()
    {
      this.btnLESnapshot.Enabled = this.btnLESnapshot.Visible = this.currentLog.DisclosedForLE;
      this.btnCDSnapshot.Enabled = this.btnCDSnapshot.Visible = this.currentLog.DisclosedForCD;
      this.btnItemSnapshot.Enabled = this.btnItemSnapshot.Visible = this.currentLog.DisclosedForCD || this.currentLog.DisclosedForLE;
      this.btnSafeHarborSnapshot.Enabled = this.btnSafeHarborSnapshot.Visible = this.currentLog.DisclosedForSafeHarbor;
      this.btnProviderListSnapshot.Enabled = this.btnProviderListSnapshot.Visible = this.currentLog.ProviderListSent || this.currentLog.ProviderListNoFeeSent;
      this.btnProviderListSnapshot.Visible = this.currentLog.ProviderListSent || this.currentLog.ProviderListNoFeeSent;
    }

    private void setDetailsTab()
    {
      bool flag = this.currentLog.DisclosedForLE || this.currentLog.DisclosedForCD;
      this.label2.Visible = this.cmbDisclosureType.Visible = flag;
      if (!flag)
      {
        Label label15 = this.label15;
        Point location = this.label15.Location;
        int x1 = location.X;
        location = this.label15.Location;
        int y1 = location.Y - 25;
        Point point1 = new Point(x1, y1);
        label15.Location = point1;
        Label label16 = this.label16;
        location = this.label16.Location;
        int x2 = location.X;
        location = this.label16.Location;
        int y2 = location.Y - 25;
        Point point2 = new Point(x2, y2);
        label16.Location = point2;
        Label label17 = this.label17;
        location = this.label17.Location;
        int x3 = location.X;
        location = this.label17.Location;
        int y3 = location.Y - 25;
        Point point3 = new Point(x3, y3);
        label17.Location = point3;
        FieldLockButton lbtnSentDate = this.lbtnSentDate;
        location = this.lbtnSentDate.Location;
        int x4 = location.X;
        location = this.lbtnSentDate.Location;
        int y4 = location.Y - 25;
        Point point4 = new Point(x4, y4);
        lbtnSentDate.Location = point4;
        DateTimePicker dtDisclosedDate = this.dtDisclosedDate;
        location = this.dtDisclosedDate.Location;
        int x5 = location.X;
        location = this.dtDisclosedDate.Location;
        int y5 = location.Y - 25;
        Point point5 = new Point(x5, y5);
        dtDisclosedDate.Location = point5;
        FieldLockButton lbtnDisclosedBy = this.lbtnDisclosedBy;
        location = this.lbtnDisclosedBy.Location;
        int x6 = location.X;
        location = this.lbtnDisclosedBy.Location;
        int y6 = location.Y - 25;
        Point point6 = new Point(x6, y6);
        lbtnDisclosedBy.Location = point6;
        TextBox txtDisclosedBy = this.txtDisclosedBy;
        location = this.txtDisclosedBy.Location;
        int x7 = location.X;
        location = this.txtDisclosedBy.Location;
        int y7 = location.Y - 25;
        Point point7 = new Point(x7, y7);
        txtDisclosedBy.Location = point7;
        TextBox txtDisclosedMethod = this.txtDisclosedMethod;
        location = this.txtDisclosedMethod.Location;
        int x8 = location.X;
        location = this.txtDisclosedMethod.Location;
        int y8 = location.Y - 25;
        Point point8 = new Point(x8, y8);
        txtDisclosedMethod.Location = point8;
        ComboBox cmbDisclosedMethod = this.cmbDisclosedMethod;
        location = this.cmbDisclosedMethod.Location;
        int x9 = location.X;
        location = this.cmbDisclosedMethod.Location;
        int y9 = location.Y - 25;
        Point point9 = new Point(x9, y9);
        cmbDisclosedMethod.Location = point9;
        TextBox txtMethodOther = this.txtMethodOther;
        location = this.txtMethodOther.Location;
        int x10 = location.X;
        location = this.txtMethodOther.Location;
        int y10 = location.Y - 25;
        Point point10 = new Point(x10, y10);
        txtMethodOther.Location = point10;
      }
      this.chkLEDisclosedByBroker.Visible = this.currentLog.DisclosedForLE;
      this.chkIntent.Visible = this.pnlIntent.Visible = this.currentLog.DisclosedForLE;
      if (this.currentLog.DisclosedForCD)
        this.cmbDisclosureType.Items.Add((object) "Post Consummation");
      if (!this.currentLog.DisclosedForLE && !this.currentLog.DisclosedForCD)
        this.lBtnBorrowerPresumed.Enabled = this.lBtnCoBorrowerPresumed.Enabled = false;
      this.lBtnBorrowerType.Locked = this.currentLog.IsBorrowerTypeLocked;
      if (this.lBtnBorrowerType.Locked)
        this.cmbBorrowerType.Text = this.currentLog.LockedBorrowerType;
      if (!this.lBtnBorrowerType.Locked)
        this.cmbBorrowerType.Visible = false;
      this.lBtnCoBorrowerType.Locked = this.currentLog.IsCoBorrowerTypeLocked;
      if (this.lBtnCoBorrowerType.Locked)
        this.cmbCoBorrowerType.Text = this.currentLog.LockedCoBorrowerType;
      if (!this.lBtnCoBorrowerType.Locked)
        this.cmbCoBorrowerType.Visible = false;
      if (!this.hasNBOs)
      {
        this.btnAuditTrail1.Left = this.cmbDisclosureType.Left;
        this.btnAuditTrail1.Top = this.btnEsign.Top;
      }
      else
        this.PopulateDisclosureRecipients();
    }

    private void setReasonTab()
    {
      this.dpChangesRecievedDate.Value = this.currentLog.ChangesReceivedDate;
      this.dpRevisedDueDate.Value = this.currentLog.RevisedDueDate;
      this.lbChangesRecievedDate.Text = "Changes Received Date";
      if (this.currentLog.DisclosedForCD)
      {
        this.chkReason1.Text = "Change in APR";
        this.chkReason1.Checked = this.currentLog.CDReasonIsChangeInAPR;
        this.chkReason2.Text = "Change in Loan Product";
        this.chkReason2.Checked = this.currentLog.CDReasonIsChangeInLoanProduct;
        this.chkReason3.Text = "Prepayment Penalty Added";
        this.chkReason3.Checked = this.currentLog.CDReasonIsPrepaymentPenaltyAdded;
        this.chkReason4.Text = "Changed Circumstance - Settlement Charges";
        this.chkReason4.Checked = this.currentLog.CDReasonIsChangeInSettlementCharges;
        this.chkReason5.Text = "Changed Circumstance - Eligibility";
        this.chkReason5.Checked = this.currentLog.CDReasonIsChangedCircumstanceEligibility;
        this.chkReason6.Text = "Revisions requested by the Consumer";
        this.chkReason6.Checked = this.currentLog.CDReasonIsRevisionsRequestedByConsumer;
        this.chkReason7.Text = "Interest Rate dependent charges (Rate Lock)";
        this.chkReason7.Checked = this.currentLog.CDReasonIsInterestRateDependentCharges;
        this.chkReason8.Text = "24-hour Advanced Preview";
        this.chkReason8.Checked = this.currentLog.CDReasonIs24HourAdvancePreview;
        this.chkReason9.Text = "Tolerance Cure";
        this.chkReason9.Checked = this.currentLog.CDReasonIsToleranceCure;
        this.chkReason10.Text = "Clerical Error Correction";
        this.chkReason10.Checked = this.currentLog.CDReasonIsClericalErrorCorrection;
        this.lbRevisedDueDate.Text = "Revised CD Due Date";
        this.chkReasonOther.Checked = this.currentLog.CDReasonIsOther;
        if (this.chkReasonOther.Checked)
          this.txtReasonOther.Text = this.currentLog.CDReasonOther;
        this.addCOCFields();
      }
      else if (this.currentLog.DisclosedForLE || this.currentLog.DisclosedForSafeHarbor || this.currentLog.ProviderListSent || this.currentLog.ProviderListNoFeeSent)
      {
        this.chkReason1.Text = "Changed Circumstance - Settlement Charges";
        this.chkReason1.Checked = this.currentLog.LEReasonIsChangedCircumstanceSettlementCharges;
        this.chkReason2.Text = "Changed Circumstance - Eligibility";
        this.chkReason2.Checked = this.currentLog.LEReasonIsChangedCircumstanceEligibility;
        this.chkReason3.Text = "Revisions requested by the Consumer";
        this.chkReason3.Checked = this.currentLog.LEReasonIsRevisionsRequestedByConsumer;
        this.chkReason4.Text = "Interest Rate dependent charges (Rate Lock)";
        this.chkReason4.Checked = this.currentLog.LEReasonIsInterestRateDependentCharges;
        this.chkReason5.Text = "Expiration (Intent to Proceed received after 10 business days)";
        this.chkReason5.Checked = this.currentLog.LEReasonIsExpiration;
        this.chkReason6.Text = "Delayed Settlement on Construction Loans";
        this.chkReason6.Checked = this.currentLog.LEReasonIsDelayedSettlementOnConstructionLoans;
        this.chkReasonOther.Checked = this.currentLog.LEReasonIsOther;
        if (this.chkReasonOther.Checked)
          this.txtReasonOther.Text = this.currentLog.LEReasonOther;
        else
          this.txtReasonOther.Text = "";
        this.chkReason7.Visible = false;
        this.chkReason8.Visible = false;
        this.chkReason9.Visible = false;
        this.chkReason10.Visible = false;
        this.chkReasonOther.Location = this.chkReason7.Location;
        this.txtReasonOther.Location = new Point(this.txtReasonOther.Location.X, this.chkReason7.Location.Y);
        this.pnlReason.Height -= 80;
        this.lbRevisedDueDate.Text = "Revised LE Due Date";
        this.addCOCFields();
      }
      if (!this.currentLog.DisclosedForCD && !this.currentLog.DisclosedForLE && !this.currentLog.DisclosedForSafeHarbor && !this.currentLog.ProviderListSent && !this.currentLog.ProviderListNoFeeSent)
        this.tcDisclosure.TabPages.Remove(this.tabPageReasons);
      this.chkReasonOther_CheckedChanged((object) null, (EventArgs) null);
      this.chkReason1.Enabled = this.chkReason2.Enabled = this.chkReason3.Enabled = this.chkReason4.Enabled = this.chkReason5.Enabled = this.chkReason6.Enabled = this.chkReason7.Enabled = this.chkReason8.Enabled = this.chkReason9.Enabled = this.chkReason10.Enabled = this.chkReasonOther.Enabled = this.txtReasonOther.Enabled = this.isReasonsChkBoxEnabled;
      bool flag = this.loanData.GetField("4461") == "Y";
      this.feeLevelIndicatorChkBx.Enabled = false;
      if (this.currentLog.AttributeList.ContainsKey("XCOCFeeLevelIndicator"))
        flag = this.currentLog.AttributeList["XCOCFeeLevelIndicator"] == "Y";
      if (Session.StartupInfo.EnableCoC)
      {
        this.feeDetailsGV.Visible = this.feeDetailsLabel.Visible = flag;
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
        Panel pnlReason = this.pnlReason;
        Size size1 = this.pnlReason.Size;
        int width = size1.Width;
        size1 = this.pnlReason.Size;
        int height = size1.Height - 29;
        Size size2 = new Size(width, height);
        pnlReason.Size = size2;
        this.pnlBottom.Size = new Size(this.pnlBottom.Size.Width, 40);
      }
      this.feeLevelIndicatorChkBx.Checked = flag;
      this.changedCircumstancesChkBx.Checked = this.currentLog.AttributeList.ContainsKey("XCOCChangedCircumstances") && this.currentLog.AttributeList["XCOCChangedCircumstances"] == "Y";
      this.changedCircumstancesChkBx.Visible = this.feeLevelIndicatorChkBx.Visible = Session.StartupInfo.EnableCoC;
      if (!flag)
        return;
      this.disableControls();
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

    private void disableControls()
    {
      this.feeLevelIndicatorChkBx.Enabled = false;
      if (this.currentLog.DisclosedForLE)
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

    private void addCOCFields()
    {
      int num = Utils.ParseInt(this.currentLog.AttributeList.ContainsKey("XCOCcount") ? (object) this.currentLog.AttributeList["XCOCcount"] : (object) "0");
      if (num <= 0)
        return;
      for (int index = 1; index <= num; ++index)
      {
        List<string> stringList = new List<string>();
        string key1 = "XCOC" + index.ToString("00") + "01";
        string key2 = "XCOC" + index.ToString("00") + "_Description";
        string key3 = "XCOC" + index.ToString("00") + "05";
        string key4 = "XCOC" + index.ToString("00") + "06";
        string key5 = "XCOC" + index.ToString("00") + "_Amount";
        string key6 = "XCOC" + index.ToString("00") + "07";
        string key7 = "XCOC" + index.ToString("00") + "08";
        string key8 = "XCOC" + index.ToString("00") + "03";
        if (this.currentLog.AttributeList.ContainsKey(key1) && this.currentLog.AttributeList[key1] != "")
        {
          GVItem gvItem = new GVItem(this.currentLog.AttributeList.ContainsKey(key2) ? this.currentLog.AttributeList[key2] : "");
          gvItem.SubItems.Add(this.currentLog.AttributeList.ContainsKey(key5) ? (object) this.currentLog.AttributeList[key5] : (object) "");
          gvItem.SubItems.Add(this.currentLog.AttributeList.ContainsKey(key3) ? (object) this.currentLog.AttributeList[key3] : (object) "");
          gvItem.SubItems.Add(this.currentLog.AttributeList.ContainsKey(key4) ? (object) this.currentLog.AttributeList[key4] : (object) "");
          if (this.currentLog.AttributeList.ContainsKey(key6))
          {
            string str = this.currentLog.AttributeList[key6];
            if (string.Compare(str.ToLowerInvariant(), "other") == 0 && this.currentLog.AttributeList.ContainsKey(key7))
              str = str + " - " + this.currentLog.AttributeList[key7];
            gvItem.SubItems.Add((object) str);
            gvItem.SubItems.Add((object) this.currentLog.AttributeList[key8]);
          }
          else
          {
            gvItem.SubItems.Add((object) "");
            gvItem.SubItems.Add((object) "");
          }
          this.feeDetailsGV.Items.Add(gvItem);
        }
      }
    }

    private void refreshDisclosure()
    {
      string str1 = "";
      string str2 = "";
      if (this.currentLog.DisclosedForLE)
      {
        if (this.currentLog.AttributeList.ContainsKey("ChangesLEReceivedDate"))
          str1 = this.currentLog.AttributeList["ChangesLEReceivedDate"];
        if (this.currentLog.AttributeList.ContainsKey("RevisedLEDueDate"))
          str2 = this.currentLog.AttributeList["RevisedLEDueDate"];
      }
      if (this.currentLog.DisclosedForCD)
      {
        if (this.currentLog.AttributeList.ContainsKey("ChangesCDReceivedDate"))
          str1 = this.currentLog.AttributeList["ChangesCDReceivedDate"];
        if (this.currentLog.AttributeList.ContainsKey("RevisedCDDueDate"))
          str2 = this.currentLog.AttributeList["RevisedCDDueDate"];
        this.chkUseforUCDExport.Checked = this.currentLog.UseForUCDExport;
      }
      if (Utils.IsDate((object) str1))
        this.currentLog.ChangesReceivedDate = Convert.ToDateTime(str1);
      if (Utils.IsDate((object) str2))
        this.currentLog.RevisedDueDate = Convert.ToDateTime(str2);
      this.suspendEvent = true;
      this.cleanHistoryDetail();
      this.tcDisclosure.TabPages.Remove(this.tabPageeDisclosure);
      this.setDetailsTab();
      this.cmbDisclosedMethod.Items.Clear();
      int length = this.discloseMethod.Length;
      for (int index = 0; index < length; ++index)
      {
        if (this.discloseMethod[index] != "eFolder eDisclosures" && this.discloseMethod[index] != "Closing Docs Order")
          this.cmbDisclosedMethod.Items.Add((object) this.discloseMethod[index]);
      }
      if (this.currentLog.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.eDisclosure)
      {
        this.cmbDisclosedMethod.Enabled = false;
        this.cmbBorrowerReceivedMethod.Enabled = false;
        this.cmbCoBorrowerReceivedMethod.Enabled = false;
        this.cmbNBOReceivedMethod.Enabled = false;
        this.dpBorrowerReceivedDate.ReadOnly = !this.currentLog.IsWetSigned || !this.currentLog.IsDisclosedReceivedDateLocked;
        this.tcDisclosure.TabPages.Add(this.tabPageeDisclosure);
        bool flag = false;
        flag = System.TimeZoneInfo.Local.IsDaylightSavingTime(this.currentLog.eDisclosurePackageCreatedDate);
        this.lblDateeDisclosureSent.Text = this.currentLog.eDisclosurePackageCreatedDate != DateTime.MinValue ? this.currentLog.eDisclosurePackageCreatedDate.ToString("MM/dd/yyyy hh:mm tt ") + this.getTimeZoneDisplayForDate(this.currentLog.eDisclosurePackageCreatedDate) : "";
        this.lblePackageId.Text = string.IsNullOrWhiteSpace(this.currentLog.eDisclosurePackageID) ? "" : this.currentLog.eDisclosurePackageID;
        this.AddDisclosureGridItems();
        this.RefreshFulfillmentFields();
      }
      else if (this.currentLog.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder)
      {
        this.cmbDisclosedMethod.Enabled = false;
        this.cmbBorrowerReceivedMethod.Enabled = false;
        this.cmbNBOReceivedMethod.Enabled = false;
        this.cmbCoBorrowerReceivedMethod.Enabled = false;
        this.dpBorrowerReceivedDate.ReadOnly = true;
        this.dpNBOReceivedDate.ReadOnly = true;
        this.lBtnCoBorrowerPresumed.Enabled = this.lBtnBorrowerPresumed.Enabled = this.lBtnNBOPresumed.Enabled = false;
        this.dpCoBorrowerReceivedDate.ReadOnly = this.dpBorrowerReceivedDate.ReadOnly = this.dpNBOReceivedDate.ReadOnly = true;
        this.dpBorrowerActualReceivedDate.ReadOnly = this.dpCoBorrowerActualReceivedDate.ReadOnly = this.dpNBOActualReceivedDate.ReadOnly = true;
        this.dpCoBorrowerReceivedDate.Text = this.dpBorrowerReceivedDate.Text = this.dpNBOReceivedDate.Text = "";
      }
      if (this.canEditSentDateAndExternalField)
      {
        if (this.currentLog.DisclosureMethod != DisclosureTrackingBase.DisclosedMethod.eDisclosure || this.currentLog.DisclosureMethod != DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder)
          this.dpBorrowerReceivedDate.ReadOnly = false;
        this.lbtnSentDate.Enabled = true;
        this.lbtnDisclosedBy.Enabled = true;
        this.lbtnDisclosedAPR.Enabled = true;
        this.lbtnDisclosedDailyInterest.Enabled = true;
        this.lbtnFinanceCharge.Enabled = true;
      }
      bool flag1 = this.currentLog.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.ByMail || this.currentLog.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.InPerson || this.currentLog.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.Email || this.currentLog.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.Fax || this.currentLog.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.Other;
      if (this.currentLog != null && !flag1 && Session.LoanDataMgr.IsPlatformLoan() || this.currentLog.eDisclosurePackageViewableFile.Trim() != string.Empty)
        this.btnViewDisclosure.Visible = true;
      else
        this.btnViewDisclosure.Visible = false;
      switch (this.currentLog.DisclosureMethod)
      {
        case DisclosureTrackingBase.DisclosedMethod.ByMail:
          this.cmbDisclosedMethod.SelectedItem = (object) this.discloseMethod[0];
          break;
        case DisclosureTrackingBase.DisclosedMethod.eDisclosure:
          this.txtDisclosedMethod.Visible = true;
          this.cmbDisclosedMethod.Visible = false;
          this.txtDisclosedMethod.BringToFront();
          break;
        case DisclosureTrackingBase.DisclosedMethod.Fax:
          this.cmbDisclosedMethod.SelectedItem = (object) this.discloseMethod[4];
          break;
        case DisclosureTrackingBase.DisclosedMethod.InPerson:
          this.cmbDisclosedMethod.SelectedItem = (object) this.discloseMethod[1];
          break;
        case DisclosureTrackingBase.DisclosedMethod.Other:
          this.cmbDisclosedMethod.SelectedItem = (object) this.discloseMethod[5];
          this.txtMethodOther.Text = this.currentLog.DisclosedMethodOther;
          this.txtMethodOther.Enabled = true;
          break;
        case DisclosureTrackingBase.DisclosedMethod.Email:
          this.cmbDisclosedMethod.SelectedItem = (object) this.discloseMethod[2];
          break;
        case DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder:
          this.txtDisclosedMethod.Visible = true;
          this.txtDisclosedMethod.Text = "Closing Docs Order";
          this.cmbDisclosedMethod.Visible = false;
          this.txtDisclosedMethod.BringToFront();
          break;
        default:
          this.cmbDisclosedMethod.SelectedItem = (object) this.discloseMethod[0];
          break;
      }
      this.lbtnSentDate.Locked = this.currentLog.IsLocked;
      this.dtDisclosedDate.Value = this.disclosedDate;
      this.dtDisclosedDate.Enabled = this.currentLog.IsLocked;
      this.lbtnDisclosedAPR.Locked = this.currentLog.IsDisclosedAPRLocked;
      this.txtDisclosedAPR.Enabled = this.currentLog.IsDisclosedAPRLocked;
      this.lbtnDisclosedDailyInterest.Locked = this.currentLog.IsDisclosedDailyInterestLocked;
      this.txtDisclosedDailyInterest.Enabled = this.currentLog.IsDisclosedDailyInterestLocked;
      this.lbtnDisclosedBy.Locked = this.currentLog.IsDisclosedByLocked;
      this.txtDisclosedBy.Enabled = this.currentLog.IsDisclosedByLocked;
      this.lBtnIntentReceivedBy.Locked = this.currentLog.IsIntentReceivedByLocked;
      this.txtIntentReceived.Enabled = this.currentLog.IsIntentReceivedByLocked;
      this.lbtnFinanceCharge.Locked = this.currentLog.IsDisclosedFinanceChargeLocked;
      this.txtFinanceCharge.Enabled = this.currentLog.IsDisclosedFinanceChargeLocked;
      if (this.currentLog.IsDisclosedByLocked)
        this.txtDisclosedBy.Text = this.currentLog.LockedDisclosedByField;
      else
        this.txtDisclosedBy.Text = this.currentLog.DisclosedByFullName + "(" + this.currentLog.DisclosedBy + ")";
      this.txtBorrowerName.Text = this.currentLog.BorrowerName;
      this.txtCoBorrowerName.Text = this.currentLog.CoBorrowerName;
      this.txtPropertyAddress.Text = this.currentLog.PropertyAddress;
      this.txtPropertyCity.Text = this.currentLog.PropertyCity;
      this.txtPropertyState.Text = this.currentLog.PropertyState;
      this.txtPropertyZip.Text = this.currentLog.PropertyZip;
      this.txtDisclosedAPR.Text = this.currentLog.DisclosedAPR;
      this.txtDisclosedDailyInterest.Text = this.currentLog.DisclosedDailyInterest;
      this.txtLoanProgram.Text = this.currentLog.LoanProgram;
      this.txtLoanAmount.Text = this.currentLog.LoanAmount;
      this.txtFinanceCharge.Text = this.currentLog.FinanceCharge;
      this.txtApplicationDate.Text = this.currentLog.ApplicationDate == DateTime.MinValue ? "" : this.currentLog.ApplicationDate.ToString("MM/dd/yyyy");
      this.cmbDisclosedMethod.Enabled = true;
      this.chkLEDisclosedByBroker.Checked = this.currentLog.LEDisclosedByBroker;
      this.cmbDisclosureType.SelectedItem = (object) this.currentLog.DisclosureType.ToString();
      if (this.currentLog.DisclosureType == DisclosureTracking2015Log.DisclosureTypeEnum.PostConsummation)
        this.cmbDisclosureType.SelectedItem = (object) "Post Consummation";
      switch (this.currentLog.BorrowerDisclosedMethod)
      {
        case DisclosureTrackingBase.DisclosedMethod.ByMail:
          this.cmbBorrowerReceivedMethod.SelectedItem = (object) this.discloseMethod[0];
          if (this.currentLog.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.eDisclosure)
          {
            this.txtBorrowerOther.Text = this.currentLog.BorrowerFulfillmentMethodDescription;
            break;
          }
          break;
        case DisclosureTrackingBase.DisclosedMethod.eDisclosure:
          this.txtBorrowerReceivedMethod.Visible = true;
          this.cmbBorrowerReceivedMethod.Visible = false;
          this.txtBorrowerReceivedMethod.BringToFront();
          this.txtBorrowerOther.Text = this.currentLog.BorrowerFulfillmentMethodDescription;
          this.txtBorrowerOther.ReadOnly = true;
          break;
        case DisclosureTrackingBase.DisclosedMethod.Fax:
          this.cmbBorrowerReceivedMethod.SelectedItem = (object) this.discloseMethod[4];
          break;
        case DisclosureTrackingBase.DisclosedMethod.InPerson:
          this.cmbBorrowerReceivedMethod.SelectedItem = (object) this.discloseMethod[1];
          if (this.currentLog.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.eDisclosure)
          {
            this.txtBorrowerOther.Text = this.currentLog.BorrowerFulfillmentMethodDescription;
            break;
          }
          break;
        case DisclosureTrackingBase.DisclosedMethod.Other:
          this.cmbBorrowerReceivedMethod.SelectedItem = (object) this.discloseMethod[5];
          this.txtBorrowerOther.Text = this.currentLog.BorrowerDisclosedMethodOther;
          break;
        case DisclosureTrackingBase.DisclosedMethod.Email:
          this.cmbBorrowerReceivedMethod.SelectedItem = (object) this.discloseMethod[2];
          break;
        case DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder:
          this.cmbBorrowerReceivedMethod.SelectedItem = (object) -1;
          this.txtBorrowerOther.ReadOnly = true;
          break;
        default:
          this.cmbBorrowerReceivedMethod.SelectedItem = (object) this.discloseMethod[0];
          break;
      }
      if (this.currentLog.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder)
        this.cmbBorrowerReceivedMethod.SelectedIndex = -1;
      this.lBtnBorrowerPresumed.Locked = this.currentLog.IsBorrowerPresumedDateLocked;
      if (this.lBtnBorrowerPresumed.Locked)
        this.dpBorrowerReceivedDate.Value = this.currentLog.LockedBorrowerPresumedReceivedDate;
      else if (this.currentLog.DisclosureMethod != DisclosureTrackingBase.DisclosedMethod.InPerson && this.currentLog.DisclosureMethod != DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder)
        this.dpBorrowerReceivedDate.Value = this.currentLog.BorrowerPresumedReceivedDate;
      this.dpBorrowerReceivedDate.ReadOnly = !this.lBtnBorrowerPresumed.Locked;
      this.dpBorrowerReceivedDate.Enabled = this.lBtnBorrowerPresumed.Locked;
      this.dpBorrowerActualReceivedDate.Value = this.currentLog.BorrowerActualReceivedDate;
      this.txtBorrowerType.Text = this.currentLog.BorrowerType;
      if (this.currentLog.CoBorrowerName.Trim() != "")
      {
        this.cmbCoBorrowerReceivedMethod.SelectedItem = (object) this.currentLog.CoBorrowerDisclosedMethod;
        switch (this.currentLog.CoBorrowerDisclosedMethod)
        {
          case DisclosureTrackingBase.DisclosedMethod.ByMail:
            this.cmbCoBorrowerReceivedMethod.SelectedItem = (object) this.discloseMethod[0];
            if (this.currentLog.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.eDisclosure)
            {
              this.txtCoBorrowerOther.Text = this.currentLog.CoBorrowerFulfillmentMethodDescription;
              break;
            }
            break;
          case DisclosureTrackingBase.DisclosedMethod.eDisclosure:
            this.txtCoBorrowerReceivedMethod.Visible = true;
            this.cmbCoBorrowerReceivedMethod.Visible = false;
            this.txtCoBorrowerReceivedMethod.BringToFront();
            this.txtCoBorrowerOther.Text = this.currentLog.CoBorrowerFulfillmentMethodDescription;
            this.txtCoBorrowerOther.ReadOnly = true;
            break;
          case DisclosureTrackingBase.DisclosedMethod.Fax:
            this.cmbCoBorrowerReceivedMethod.SelectedItem = (object) this.discloseMethod[4];
            break;
          case DisclosureTrackingBase.DisclosedMethod.InPerson:
            this.cmbCoBorrowerReceivedMethod.SelectedItem = (object) this.discloseMethod[1];
            if (this.currentLog.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.eDisclosure)
            {
              this.txtCoBorrowerOther.Text = this.currentLog.CoBorrowerFulfillmentMethodDescription;
              break;
            }
            break;
          case DisclosureTrackingBase.DisclosedMethod.Other:
            this.cmbCoBorrowerReceivedMethod.SelectedItem = (object) this.discloseMethod[5];
            this.txtCoBorrowerOther.Text = this.currentLog.CoBorrowerDisclosedMethodOther;
            break;
          case DisclosureTrackingBase.DisclosedMethod.Email:
            this.cmbCoBorrowerReceivedMethod.SelectedItem = (object) this.discloseMethod[2];
            break;
          case DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder:
            this.cmbBorrowerReceivedMethod.SelectedItem = (object) -1;
            this.txtCoBorrowerOther.ReadOnly = true;
            break;
          default:
            this.cmbCoBorrowerReceivedMethod.SelectedItem = (object) this.discloseMethod[0];
            break;
        }
        if (this.currentLog.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder)
          this.cmbCoBorrowerReceivedMethod.SelectedIndex = -1;
        this.lBtnCoBorrowerPresumed.Locked = this.currentLog.IsCoBorrowerPresumedDateLocked;
        if (this.lBtnCoBorrowerPresumed.Locked)
          this.dpCoBorrowerReceivedDate.Value = this.currentLog.LockedCoBorrowerPresumedReceivedDate;
        else if (this.currentLog.DisclosureMethod != DisclosureTrackingBase.DisclosedMethod.InPerson && this.currentLog.DisclosureMethod != DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder)
          this.dpCoBorrowerReceivedDate.Value = this.currentLog.CoBorrowerPresumedReceivedDate;
        this.dpCoBorrowerReceivedDate.ReadOnly = !this.lBtnCoBorrowerPresumed.Locked;
        this.dpCoBorrowerReceivedDate.Enabled = this.lBtnCoBorrowerPresumed.Locked;
        this.dpCoBorrowerActualReceivedDate.Value = this.currentLog.CoBorrowerActualReceivedDate;
        this.txtCoBorrowerType.Text = this.currentLog.CoBorrowerType;
      }
      this.dtDisclosedDate_ValueChanged((object) null, (EventArgs) null);
      this.chkIntent.Checked = this.currentLog.IntentToProceed && DateTime.Today >= this.dpIntentDate.MinDate;
      this.chkIntent_CheckedChanged((object) null, (EventArgs) null);
      try
      {
        if (this.currentLog.IntentToProceedDate >= this.dpIntentDate.MinDate)
        {
          this.dpIntentDate.Value = this.currentLog.IntentToProceedDate;
          this.dpIntentDate.CustomFormat = "MM/dd/yyyy";
        }
      }
      catch
      {
        this.dpIntentDate.Text = "";
      }
      if (this.currentLog.IsIntentReceivedByLocked)
      {
        this.lBtnIntentReceivedBy.Locked = this.currentLog.IsIntentReceivedByLocked;
        this.txtIntentReceived.Text = this.currentLog.LockedIntentReceivedByField;
        this.txtIntentReceived.Enabled = this.lBtnIntentReceivedBy.Locked;
      }
      else if (this.currentLog.IntentToProceedReceivedBy != "")
        this.txtIntentReceived.Text = this.currentLog.IntentToProceedReceivedBy;
      else
        this.txtIntentReceived.Text = Session.UserInfo.FullName + "(" + Session.UserInfo.Userid + ")";
      switch (this.currentLog.IntentToProceedReceivedMethod)
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
          this.cmbIntentReceivedMethod.SelectedItem = (object) this.sentMethod[0];
          this.dpIntentDate.Enabled = true;
          break;
        case DisclosureTrackingBase.DisclosedMethod.Other:
          this.cmbIntentReceivedMethod.SelectedItem = (object) this.sentMethod[4];
          this.txtIntentMethodOther.Text = this.currentLog.IntentToProceedReceivedMethodOther;
          break;
        case DisclosureTrackingBase.DisclosedMethod.Email:
          this.cmbIntentReceivedMethod.SelectedItem = (object) this.sentMethod[2];
          this.dpIntentDate.Enabled = true;
          break;
        case DisclosureTrackingBase.DisclosedMethod.Phone:
          this.cmbIntentReceivedMethod.SelectedItem = (object) this.sentMethod[1];
          this.dpIntentDate.Enabled = true;
          break;
        case DisclosureTrackingBase.DisclosedMethod.Signature:
          this.cmbIntentReceivedMethod.SelectedItem = (object) this.sentMethod[3];
          this.dpIntentDate.Enabled = true;
          break;
        case DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder:
          this.txtIntentToProceedMethod.Visible = true;
          this.cmbIntentReceivedMethod.Visible = false;
          this.txtIntentToProceedMethod.BringToFront();
          break;
        default:
          this.cmbIntentReceivedMethod.SelectedItem = (object) this.sentMethod[0];
          break;
      }
      this.txtIntentComments.Text = this.currentLog.IntentToProceedComments;
      this.setReasonTab();
      if (this.currentLog.ChangeInCircumstance != null && this.currentLog.ChangeInCircumstance != "")
        this.txtChangedCircumstance.Text = this.currentLog.ChangeInCircumstance;
      this.txtCiCComment.Text = this.currentLog.ChangeInCircumstanceComments;
      this.setButtons();
      foreach (DisclosureTrackingFormItem disclosedForm in this.currentLog.DisclosedFormList)
        this.gvDocList.Items.Add(new GVItem(disclosedForm.FormName)
        {
          SubItems = {
            (object) disclosedForm.OutputFormTypeName
          }
        });
      this.gcDocList.Text = "Documents Sent (" + (object) this.gvDocList.Items.Count + ")";
      if (!this.hasAccessRight)
      {
        this.lbtnSentDate.Enabled = false;
        this.dtDisclosedDate.Enabled = false;
        this.cmbDisclosedMethod.Enabled = false;
        this.dpBorrowerReceivedDate.ReadOnly = true;
        this.cmbBorrowerType.Visible = false;
        this.lBtnBorrowerType.Visible = false;
        this.cmbCoBorrowerType.Visible = false;
        this.lBtnCoBorrowerType.Visible = false;
        this.lBtnNBOType.Visible = false;
        this.cmbNBOType.Visible = false;
        this.dpNBOReceivedDate.ReadOnly = true;
      }
      if (!this.canEditSentDateAndExternalField)
      {
        this.lbtnSentDate.Enabled = false;
        this.dtDisclosedDate.Enabled = false;
        this.lbtnDisclosedBy.Enabled = false;
        this.txtDisclosedBy.Enabled = false;
        this.lbtnDisclosedAPR.Enabled = false;
        this.txtDisclosedAPR.Enabled = false;
        this.lbtnDisclosedDailyInterest.Enabled = false;
        this.txtDisclosedDailyInterest.Enabled = false;
        this.lbtnFinanceCharge.Enabled = false;
        this.txtFinanceCharge.Enabled = false;
        this.cmbDisclosureType.Enabled = false;
        this.txtDisclosedMethod.Enabled = false;
        this.cmbBorrowerReceivedMethod.Enabled = false;
        this.cmbNBOReceivedMethod.Enabled = false;
        this.cmbDisclosedMethod.Enabled = false;
        this.dpBorrowerReceivedDate.Enabled = false;
        this.dpNBOReceivedDate.Enabled = false;
        this.dpBorrowerActualReceivedDate.Enabled = false;
        this.dpNBOActualReceivedDate.Enabled = false;
        this.lBtnBorrowerPresumed.Enabled = false;
        this.lBtnNBOPresumed.Enabled = false;
        this.chkLEDisclosedByBroker.Enabled = false;
        this.chkIntent.Enabled = false;
        this.cmbBorrowerType.Enabled = false;
        this.lBtnBorrowerType.Enabled = false;
        this.cmbCoBorrowerType.Enabled = false;
        this.lBtnCoBorrowerType.Enabled = false;
        this.cmbNBOType.Enabled = false;
        this.lBtnNBOType.Enabled = false;
        this.cmbCoBorrowerReceivedMethod.Enabled = this.txtCoBorrowerReceivedMethod.Enabled = false;
        this.dpCoBorrowerActualReceivedDate.Enabled = false;
        this.dpCoBorrowerReceivedDate.Enabled = false;
      }
      this.refreshIntentToProceed(this.dpBorrowerReceivedDate.Value, this.dpBorrowerActualReceivedDate.Value, this.dpCoBorrowerReceivedDate.Value, this.dpCoBorrowerActualReceivedDate.Value);
      this.suspendEvent = false;
      if (!this.hasNBOs)
        return;
      this.refreshNBO();
    }

    private void AddDisclosureGridItems()
    {
      this.grdDisclosureTracking.Items.Clear();
      this.addDisclosureGridItem("Name", this.currentLog.eDisclosureBorrowerName, this.currentLog.eDisclosureCoBorrowerName, this.currentLog.eDisclosureLOName);
      this.addDisclosureGridItem("Email Address", this.currentLog.eDisclosureBorrowerEmail, this.currentLog.eDisclosureCoBorrowerEmail, "");
      this.addDisclosureGridItem("Consent when eDisclosure was sent", this.currentLog.EDisclosureBorrowerLoanLevelConsent, this.currentLog.EDisclosureCoBorrowerLoanLevelConsent, "");
      this.addDisclosureGridItem("Message Viewed", this.currentLog.eDisclosureBorrowerViewMessageDate, this.currentLog.eDisclosureCoBorrowerViewMessageDate, this.currentLog.eDisclosureLOViewMessageDate);
      this.addDisclosureGridItem("Package Consent Form Accepted", this.currentLog.eDisclosureBorrowerAcceptConsentDate, this.currentLog.eDisclosureCoBorrowerAcceptConsentDate, DateTime.MinValue);
      this.addDisclosureGridItem("Package Consent Form Accepted from IP Address", this.currentLog.eDisclosureBorrowerAcceptConsentIP, this.currentLog.eDisclosureCoBorrowerAcceptConsentIP, "");
      this.addDisclosureGridItem("Package Consent Form Rejected", this.currentLog.eDisclosureBorrowerRejectConsentDate, this.currentLog.eDisclosureCoBorrowerRejectConsentDate, DateTime.MinValue);
      this.addDisclosureGridItem("Package Consent Form Rejected from IP Address", this.currentLog.eDisclosureBorrowerRejectConsentIP, this.currentLog.eDisclosureCoBorrowerRejectConsentIP, "");
      this.addDisclosureGridItem("Authenticated", this.currentLog.eDisclosureBorrowerAuthenticatedDate, this.currentLog.eDisclosureCoBorrowerAuthenticatedDate, DateTime.MinValue);
      this.addDisclosureGridItem("Authenticated from IP Address", this.currentLog.eDisclosureBorrowerAuthenticatedIP, this.currentLog.eDisclosureCoBorrowerAuthenticatedIP, "");
      this.addDisclosureGridItem("Document Viewed Date", this.currentLog.EDisclosureBorrowerDocumentViewedDate, this.currentLog.EDisclosureCoborrowerDocumentViewedDate, DateTime.MinValue);
      this.addDisclosureGridItem("eSigned Disclosures", this.currentLog.eDisclosureBorrowereSignedDate, this.currentLog.eDisclosureCoBorrowereSignedDate, this.currentLog.eDisclosureLOeSignedDate);
      this.addDisclosureGridItem("eSigned Disclosures from IP Address", this.currentLog.eDisclosureBorrowereSignedIP, this.currentLog.eDisclosureCoBorrowereSignedIP, this.currentLog.eDisclosureLOeSignedIP);
    }

    private void addDisclosureGridItem(
      string columnDesc,
      DateTime borrowerInfo,
      DateTime coBorrowerInfo,
      DateTime loanOfficerInfo)
    {
      GVItem gvItem = new GVItem();
      gvItem.SubItems.Add((object) columnDesc);
      if (borrowerInfo != DateTime.MinValue)
        gvItem.SubItems.Add((object) (borrowerInfo.ToString("MM/dd/yyyy hh:mm tt ") + this.getTimeZoneDisplayForDate(borrowerInfo)));
      else
        gvItem.SubItems.Add((object) "");
      if (coBorrowerInfo != DateTime.MinValue)
        gvItem.SubItems.Add((object) (coBorrowerInfo.ToString("MM/dd/yyyy hh:mm tt ") + this.getTimeZoneDisplayForDate(coBorrowerInfo)));
      else
        gvItem.SubItems.Add((object) "");
      if (loanOfficerInfo != DateTime.MinValue)
        gvItem.SubItems.Add((object) (loanOfficerInfo.ToString("MM/dd/yyyy hh:mm tt ") + this.getTimeZoneDisplayForDate(loanOfficerInfo)));
      else
        gvItem.SubItems.Add((object) "");
      this.grdDisclosureTracking.Items.Add(gvItem);
    }

    private void addNBODisclosureGridItem(string columnDesc, DateTime NBOinfo)
    {
      GVItem gvItem = new GVItem();
      gvItem.SubItems.Add((object) columnDesc);
      if (NBOinfo != DateTime.MinValue)
        gvItem.SubItems.Add((object) (NBOinfo.ToString("MM/dd/yyyy hh:mm tt ") + this.getTimeZoneDisplayForDate(NBOinfo)));
      else
        gvItem.SubItems.Add((object) "");
      this.grdNBODisclosureTracking.Items.Add(gvItem);
    }

    private void addDisclosureGridItem(
      string columnDesc,
      string borrowerInfo,
      string coBorrowerInfo,
      string loanOfficerInfo)
    {
      this.grdDisclosureTracking.Items.Add(new GVItem()
      {
        SubItems = {
          (object) columnDesc,
          (object) borrowerInfo,
          (object) coBorrowerInfo,
          (object) loanOfficerInfo
        }
      });
    }

    private void addNBODisclosureGridItem(string columnDesc, string NBOInfo)
    {
      this.grdNBODisclosureTracking.Items.Add(new GVItem()
      {
        SubItems = {
          (object) columnDesc,
          (object) NBOInfo
        }
      });
    }

    private void cleanHistoryDetail()
    {
      this.dpBorrowerReceivedDate.Text = "";
      this.txtDisclosedBy.Text = "";
      this.txtBorrowerName.Text = "";
      this.txtCoBorrowerName.Text = "";
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
      this.cmbDisclosedMethod.SelectedItem = (object) "";
      this.cmbDisclosedMethod.Enabled = false;
      this.dtDisclosedDate.Enabled = false;
      this.dpBorrowerReceivedDate.ReadOnly = true;
      this.btnLESnapshot.Enabled = false;
      this.btnCDSnapshot.Enabled = false;
      this.btnSafeHarborSnapshot.Enabled = false;
      this.txtDisclosedMethod.Visible = false;
      this.cmbDisclosedMethod.Visible = true;
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
      if (this.currentLog.eDisclosureManualFulfillmentDate == DateTime.MinValue)
      {
        this.txtFulfillmentOrderBy.Text = this.currentLog.FulfillmentOrderedBy;
        this.txtDateFulfillOrder.Text = this.currentLog.FullfillmentProcessedDate == DateTime.MinValue ? "" : this.currentLog.FullfillmentProcessedDate.ToString("MM/dd/yyyy hh:mm tt ") + Utils.GetTimezoneAbbrev(this.currentLog.TimeZoneInfo);
        if (this.currentLog.FullfillmentProcessedDate == DateTime.MinValue)
          this.txtFulfillmentMethod.Text = "";
        else if (this.currentLog.eDisclosureAutomatedFulfillmentMethod == DisclosureTrackingBase.DisclosedMethod.OvernightShipping)
          this.txtFulfillmentMethod.Text = "Overnight Shipping";
        else
          this.txtFulfillmentMethod.Text = "Encompass Fulfillment Service";
        this.txtTrackingNumber.Text = this.currentLog.FulfillmentTrackingNumber;
        if (this.currentLog.PresumedFulfillmentDate != DateTime.MinValue)
          this.dpPresumedFulfillmentDate.Value = this.currentLog.PresumedFulfillmentDate;
        this.dpActualFulfillmentDate.ReadOnly = true;
        if (this.currentLog.ActualFulfillmentDate != DateTime.MinValue)
          this.dpActualFulfillmentDate.Value = this.currentLog.ActualFulfillmentDate;
        this.txtFulfillmentComments.Text = "";
        this.btneDiscManualFulfill.Visible = !this.isFulfillmentServiceEnabled && this.hasManualFulfillmentPermission && this.currentLog.FullfillmentProcessedDate == DateTime.MinValue;
      }
      else
      {
        this.txtFulfillmentOrderBy.Text = this.currentLog.eDisclosureManuallyFulfilledBy;
        this.txtDateFulfillOrder.Text = this.currentLog.eDisclosureManualFulfillmentDate == DateTime.MinValue ? "" : this.currentLog.eDisclosureManualFulfillmentDate.ToString("MM/dd/yyyy hh:mm tt ") + Utils.GetTimezoneAbbrev(this.currentLog.TimeZoneInfo);
        switch (this.currentLog.eDisclosureManualFulfillmentMethod)
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
        this.dpPresumedFulfillmentDate.Value = this.currentLog.PresumedFulfillmentDate;
        this.dpActualFulfillmentDate.Value = this.currentLog.ActualFulfillmentDate;
        this.txtFulfillmentComments.Text = this.currentLog.eDisclosureManualFulfillmentComment;
        this.btneDiscManualFulfill.Visible = false;
      }
    }

    private void refreshUpdatedItem()
    {
      if (string.Concat(this.cmbDisclosedMethod.SelectedItem) == this.discloseMethod[1])
      {
        if (!this.lBtnBorrowerPresumed.Locked)
          this.dpBorrowerReceivedDate.Text = "";
        if (this.lBtnCoBorrowerPresumed.Locked)
          return;
        this.dpCoBorrowerReceivedDate.Text = "";
      }
      else if ((this.currentLog.DisclosedForLE || this.currentLog.DisclosedForCD) && this.currentLog.DisclosureMethod != DisclosureTrackingBase.DisclosedMethod.eDisclosure)
      {
        if (this.currentLog.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder)
          return;
        if (!this.lBtnBorrowerPresumed.Locked)
          this.dpBorrowerReceivedDate.Value = this.getReceivedDate();
        if (!(this.currentLog.CoBorrowerName.Trim() != "") || this.lBtnCoBorrowerPresumed.Locked)
          return;
        this.dpCoBorrowerReceivedDate.Value = this.getReceivedDate();
      }
      else if (this.currentLog.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.eDisclosure)
        this.dpActualFulfillmentDate_ValueChanged((object) null, (EventArgs) null);
      else
        this.dpCoBorrowerReceivedDate.Text = this.dpBorrowerReceivedDate.Text = "";
    }

    private void lbtnSentDate_Click(object sender, EventArgs e)
    {
      this.lbtnSentDate.Locked = !this.lbtnSentDate.Locked;
      if (!this.lbtnSentDate.Locked)
      {
        this.dtDisclosedDate.Value = this.disclosedDate;
        if (this.currentLog.DisclosedForLE || this.currentLog.DisclosedForCD)
        {
          if (this.currentLog.DisclosureMethod != DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder && Utils.IsDate((object) this.loanData.GetField("3983")))
          {
            if (!this.lBtnBorrowerPresumed.Locked)
              this.dpBorrowerReceivedDate.Value = this.getReceivedDate();
            if (this.currentLog.CoBorrowerName.Trim() != "" && !this.lBtnCoBorrowerPresumed.Locked)
              this.dpCoBorrowerReceivedDate.Value = this.getReceivedDate();
          }
        }
        else
          this.dpCoBorrowerReceivedDate.Text = this.dpBorrowerReceivedDate.Text = "";
        this.dtDisclosedDate.Enabled = false;
        this.refreshUpdatedItem();
      }
      else
      {
        if (this.disclosedDate != DateTime.MinValue)
          this.dtDisclosedDate.Value = this.disclosedDate;
        if (this.currentLog.DisclosedForLE || this.currentLog.DisclosedForCD)
        {
          if (this.currentLog.DisclosureMethod != DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder && Utils.IsDate((object) this.loanData.GetField("3983")))
          {
            if (!this.lBtnBorrowerPresumed.Locked)
              this.dpBorrowerReceivedDate.Value = this.getReceivedDate();
            if (this.currentLog.CoBorrowerName.Trim() != "" && !this.lBtnCoBorrowerPresumed.Locked)
              this.dpCoBorrowerReceivedDate.Value = this.getReceivedDate();
            if (this.hasNBOs)
            {
              this.getReceivedDateForNBOs();
              if (this.nboInstance != "")
                this.populateNBODetails();
            }
          }
        }
        else
          this.dpCoBorrowerReceivedDate.Text = this.dpBorrowerReceivedDate.Text = "";
        this.dtDisclosedDate.Enabled = true;
        this.refreshUpdatedItem();
        if (!this.hasNBOs || !(this.nboInstance != ""))
          return;
        this.refreshNBOUpdatedItem();
      }
    }

    private void getReceivedDateForNBOs()
    {
      foreach (KeyValuePair<string, INonBorrowerOwnerItem> allnboItem in this.currentLog.GetAllnboItems())
      {
        if (!allnboItem.Value.isPresumedDateLocked && allnboItem.Value.eDisclosureNBOLoanLevelConsent == "Accepted")
          this.currentLog.SetnboAttributeValue(allnboItem.Key, (object) this.getReceivedDate(), DisclosureTracking2015Log.NBOUpdatableFields.PresumedReceivedDate);
      }
    }

    private void setReceivedDateForNBOs()
    {
      Dictionary<string, object> disclosureNboReceivedDate = this.loanData.Calculator.CalculateNew2015DisclosureNBOReceivedDate((IDisclosureTracking2015Log) this.currentLog, this.dtDisclosedDate.Value);
      foreach (KeyValuePair<string, INonBorrowerOwnerItem> curNboItem in this.curNBOItems)
      {
        string key = curNboItem.Key + "_PresumedReceiveDate";
        if (Utils.ParseDate(disclosureNboReceivedDate[key]) != DateTime.MinValue)
          curNboItem.Value.PresumedReceivedDate = Utils.ParseDate(disclosureNboReceivedDate[key]);
      }
    }

    private void setActualReceivedDateForNBOs()
    {
      bool policySetting = (bool) Session.StartupInfo.PolicySettings[(object) "Policies.eSignReceivedDate"];
      foreach (KeyValuePair<string, INonBorrowerOwnerItem> curNboItem in this.curNBOItems)
      {
        DateTime dateTime1 = new DateTime();
        DateTime dateTime2 = !this.currentLog.IsWetSigned ? (!policySetting ? curNboItem.Value.eDisclosureNBODocumentViewedDate : curNboItem.Value.eDisclosureNBOSignedDate) : curNboItem.Value.eDisclosureNBODocumentViewedDate;
        if (curNboItem.Value.ActualReceivedDate == DateTime.MinValue || dateTime2 != DateTime.MinValue)
          curNboItem.Value.ActualReceivedDate = dateTime2;
      }
    }

    private void calculateDisclosedDate()
    {
      if (!Utils.IsDate((object) this.dtDisclosedDate.Text) || this.currentLog == null)
        return;
      this.validateDisclosedDate();
      DateTime receivedDate;
      if (this.currentLog.ReceivedDate != DateTime.MinValue)
      {
        receivedDate = this.dtDisclosedDate.Value;
        string str1 = receivedDate.ToString("MM/dd/yyyy");
        receivedDate = this.currentLog.ReceivedDate;
        string str2 = receivedDate.ToString("MM/dd/yyyy");
        if (str1 != str2 && this.dtDisclosedDate.Value > this.currentLog.ReceivedDate && this.currentLog.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.Other)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Borrower Received Date cannot be earlier than Sent Date");
        }
      }
      DateTime disclosedDate = this.disclosedDate;
      try
      {
        receivedDate = this.dtDisclosedDate.Value;
        this.disclosedDate = DateTime.Parse(receivedDate.ToString("MM/dd/yyyy  HH:mm:ss"));
      }
      catch (Exception ex)
      {
        this.dtDisclosedDate.Value = disclosedDate;
        this.disclosedDate = disclosedDate;
        throw ex;
      }
      this.refreshUpdatedItem();
    }

    private async void btnEsign_Click(object sender, EventArgs e)
    {
      DisclosureDetailsDialog2015 parentForm = this;
      try
      {
        parentForm.btnEsign.Enabled = false;
        int num = (int) await new ESignLO(parentForm.loanData.GUID).ShowESignDialog((IDisclosureTracking2015Log) parentForm.currentLog, (Form) parentForm);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
      finally
      {
        if (parentForm.currentLog.eDisclosureLOeSignedDate == DateTime.MinValue)
        {
          parentForm.btnEsign.Enabled = true;
        }
        else
        {
          parentForm.btnEsign.Enabled = false;
          parentForm.AddDisclosureGridItems();
        }
      }
    }

    private void lbtnDisclosedBy_Click(object sender, EventArgs e)
    {
      this.lbtnDisclosedBy.Locked = !this.lbtnDisclosedBy.Locked;
      if (!this.lbtnDisclosedBy.Locked)
      {
        this.txtDisclosedBy.Text = this.currentLog.DisclosedByFullName + "(" + this.currentLog.DisclosedBy + ")";
        this.txtDisclosedBy.Enabled = false;
        this.refreshUpdatedItem();
      }
      else
      {
        this.txtDisclosedBy.Text = this.currentLog.LockedDisclosedByField;
        this.txtDisclosedBy.Enabled = true;
      }
    }

    private void lbtnReceivedDate_Click(object sender, EventArgs e)
    {
      if (this.currentLog.IsDisclosedReceivedDateLocked)
        this.currentLog.IsDisclosedReceivedDateLocked = false;
      else
        this.currentLog.IsDisclosedReceivedDateLocked = true;
      if (!this.currentLog.IsDisclosedReceivedDateLocked)
      {
        if (this.currentLog.ReceivedDate == DateTime.MinValue)
          this.dpBorrowerReceivedDate.Text = "";
        else
          this.dpBorrowerReceivedDate.Value = this.currentLog.ReceivedDate;
        this.dpBorrowerReceivedDate.ReadOnly = true;
        this.refreshUpdatedItem();
      }
      else
        this.dpBorrowerReceivedDate.ReadOnly = false;
    }

    private void lblSnapshot_Click(object sender, EventArgs e)
    {
      int num = (int) new DisclosedLEDialog((IDisclosureTracking2015Log) this.currentLog).ShowDialog((IWin32Window) this);
    }

    private void btnSafeHarborSnapshot_Click(object sender, EventArgs e)
    {
      int num = (int) new DisclosedSafeHarborDialog((IDisclosureTrackingLog) this.currentLog).ShowDialog((IWin32Window) this);
    }

    private void lbtnDisclosedAPR_Click(object sender, EventArgs e)
    {
      this.lbtnDisclosedAPR.Locked = !this.lbtnDisclosedAPR.Locked;
      if (!this.lbtnDisclosedAPR.Locked)
      {
        this.txtDisclosedAPR.Text = this.currentLog.DisclosedAPRValue;
        this.txtDisclosedAPR.Enabled = false;
      }
      else
        this.txtDisclosedAPR.Enabled = true;
      this.txtDisclosedAPR_Leave((object) null, (EventArgs) null);
    }

    private void txtDisclosedAPR_Leave(object sender, EventArgs e)
    {
      if (this.suspendEvent || this.currentLog == null)
        return;
      if (this.intermediateData)
      {
        this.intermediateData = false;
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
        this.intermediateData = true;
        this.txtDisclosedAPR.Text = str;
        this.txtDisclosedAPR.SelectionStart = str.Length;
        this.intermediateData = false;
      }
    }

    private void lbtnFinanceCharge_Click(object sender, EventArgs e)
    {
      this.lbtnFinanceCharge.Locked = !this.lbtnFinanceCharge.Locked;
      if (!this.lbtnFinanceCharge.Locked)
      {
        this.txtFinanceCharge.Text = this.currentLog.FinanceChargeValue;
        this.txtFinanceCharge.Enabled = false;
      }
      else
        this.txtFinanceCharge.Enabled = true;
      this.txtFinanceCharge_Leave((object) null, (EventArgs) null);
    }

    private void txtFinanceCharge_Leave(object sender, EventArgs e)
    {
      if (this.suspendEvent || this.currentLog == null)
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
      this.intermediateData = true;
      this.txtFinanceCharge.Text = str;
      this.txtFinanceCharge.SelectionStart = str.Length;
      this.intermediateData = false;
    }

    private void btnViewDisclosure_Click(object sender, EventArgs e)
    {
      IEFolder service = Session.Application.GetService<IEFolder>();
      if (this.IsLinkedLoan)
        service.ViewDisclosures(Session.LoanDataMgr.LinkedLoan, this.currentLog.eDisclosurePackageViewableFile, this.currentLog.eDisclosurePackageID, this.currentLog.Guid);
      else
        service.ViewDisclosures(Session.LoanDataMgr, this.currentLog.eDisclosurePackageViewableFile, this.currentLog.eDisclosurePackageID, this.currentLog.Guid);
    }

    private void llViewDetails_Click(object sender, EventArgs e)
    {
      if (this.currentLog == null)
        return;
      int num = (int) new eDisclosureDetails((IDisclosureTrackingLog) this.currentLog).ShowDialog((IWin32Window) this);
    }

    private void llViewDetails_MouseEnter(object sender, EventArgs e) => this.Cursor = Cursors.Hand;

    private void llViewDetails_MouseLeave(object sender, EventArgs e)
    {
      this.Cursor = Cursors.Default;
    }

    private void lblViewForm_Click(object sender, EventArgs e)
    {
      if (this.currentLog == null)
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
      LoanDataMgr loanDataMgr = this.IsLinkedLoan ? Session.LoanDataMgr.LinkedLoan : Session.LoanDataMgr;
      try
      {
        BinaryObject binaryObject = (BinaryObject) null;
        if (loanDataMgr.IsPlatformLoan())
        {
          if (!string.IsNullOrEmpty(this.currentLog.eDisclosurePackageID))
            binaryObject = new EDeliveryRestClient(loanDataMgr).GetPackageConsentPdf(this.currentLog.eDisclosurePackageID).Result;
        }
        else if (!string.IsNullOrEmpty(this.currentLog.eDisclosureConsentPDF))
          binaryObject = loanDataMgr.GetSupportingData(this.currentLog.eDisclosureConsentPDF);
        if (binaryObject == null)
          throw new FileNotFoundException();
        string str = path + "eDisclosure_" + this.currentLog.Guid + ".pdf";
        binaryObject.Write(str);
        int num = (int) new PdfPreviewDialog(str, true, true, false).ShowDialog((IWin32Window) this);
      }
      catch (Exception ex)
      {
        Tracing.Log(DisclosureDetailsDialog2015.sw, TraceLevel.Error, nameof (DisclosureDetailsDialog2015), "View Consent Form. Error: " + (object) ex);
        int num = (int) Utils.Dialog((IWin32Window) this, "Consent form is not available at this time.");
      }
    }

    private void lblViewForm_MouseEnter(object sender, EventArgs e) => this.Cursor = Cursors.Hand;

    private void lblViewForm_MouseLeave(object sender, EventArgs e)
    {
      this.Cursor = Cursors.Default;
    }

    private void btnAuditTrail1_Click(object sender, EventArgs e) => this.viewAuditTrail();

    private void btnAuditTrail2_Click(object sender, EventArgs e) => this.viewAuditTrail();

    private void btneDiscManualFulfill_Click(object sender, EventArgs e)
    {
      if (this.currentLog.eDisclosurePackageViewableFile == string.Empty)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "There are no viewable documents associated with this disclosure tracking entry.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        Session.Application.GetService<IEFolder>();
        ManualFulfillmentDialog fulfillmentDialog = new ManualFulfillmentDialog((IDisclosureTrackingLog) this.currentLog);
        if (fulfillmentDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.fulfillmentUpdated = true;
        this.eDisclosureManuallyFulfilledBy = fulfillmentDialog.EDisclosureManuallyFulfilledBy;
        this.eDisclosureManualFulfillmentDate = fulfillmentDialog.EDisclosureManualFulfillmentDate;
        this.eDisclosureManualFulfillmentMethod = fulfillmentDialog.EDisclosureManualFulfillmentMethod;
        this.eDisclosureManualFulfillmentComment = fulfillmentDialog.EDisclosureManualFulfillmentComment;
        this.eDisclosurePresumedDate = fulfillmentDialog.EDisclosurePresumedDate;
        this.eDisclosureActualDate = fulfillmentDialog.EDisclosureActualDate;
        this.txtFulfillmentOrderBy.Text = this.eDisclosureManuallyFulfilledBy;
        this.txtDateFulfillOrder.Text = this.eDisclosureManualFulfillmentDate.ToString();
        if (this.eDisclosureManualFulfillmentMethod == DisclosureTrackingBase.DisclosedMethod.InPerson)
          this.txtFulfillmentMethod.Text = this.discloseMethod[1];
        else if (this.eDisclosureManualFulfillmentMethod == DisclosureTrackingBase.DisclosedMethod.ByMail)
          this.txtFulfillmentMethod.Text = this.discloseMethod[0];
        this.txtFulfillmentComments.Text = this.eDisclosureManualFulfillmentComment;
        this.dpPresumedFulfillmentDate.Value = this.eDisclosurePresumedDate;
        this.dpActualFulfillmentDate.ReadOnly = false;
        this.dpActualFulfillmentDate.Value = this.eDisclosureActualDate;
        Dictionary<string, object> disclosureReceivedDate = this.loanData.Calculator.CalculateNew2015DisclosureReceivedDate((IDisclosureTracking2015Log) this.currentLog, this.dtDisclosedDate.Value, this.eDisclosurePresumedDate, this.eDisclosureActualDate, this.eDisclosureManualFulfillmentMethod);
        if ((DisclosureTrackingBase.DisclosedMethod) disclosureReceivedDate["BorrowerDisclosedMethod"] == DisclosureTrackingBase.DisclosedMethod.eDisclosure)
        {
          this.txtBorrowerReceivedMethod.Visible = true;
          this.cmbBorrowerReceivedMethod.Visible = false;
          this.txtBorrowerReceivedMethod.BringToFront();
          this.txtBorrowerReceivedMethod.Text = this.discloseMethod[3];
        }
        else if ((DisclosureTrackingBase.DisclosedMethod) disclosureReceivedDate["BorrowerDisclosedMethod"] == DisclosureTrackingBase.DisclosedMethod.InPerson)
        {
          this.txtBorrowerReceivedMethod.Visible = false;
          this.cmbBorrowerReceivedMethod.Visible = true;
          this.cmbBorrowerReceivedMethod.SelectedItem = (object) this.discloseMethod[1];
        }
        else if ((DisclosureTrackingBase.DisclosedMethod) disclosureReceivedDate["BorrowerDisclosedMethod"] == DisclosureTrackingBase.DisclosedMethod.ByMail)
        {
          this.txtBorrowerReceivedMethod.Visible = false;
          this.cmbBorrowerReceivedMethod.Visible = true;
          this.cmbBorrowerReceivedMethod.SelectedItem = (object) this.discloseMethod[0];
        }
        this.txtBorrowerOther.Text = disclosureReceivedDate["BorrowerFulfillmentMethodDescription"].ToString();
        this.dpBorrowerActualReceivedDate.Value = Utils.ParseDate(disclosureReceivedDate["BorrowerActualReceivedDate"]);
        if (Utils.ParseDate(disclosureReceivedDate["BorrowerPresumedReceivedDate"]) != DateTime.MinValue)
          this.dpBorrowerReceivedDate.Value = Utils.ParseDate(disclosureReceivedDate["BorrowerPresumedReceivedDate"]);
        if (this.currentLog.CoBorrowerName.Trim() != "")
        {
          if ((DisclosureTrackingBase.DisclosedMethod) disclosureReceivedDate["CoBorrowerDisclosedMethod"] == DisclosureTrackingBase.DisclosedMethod.eDisclosure)
          {
            this.txtBorrowerReceivedMethod.Visible = true;
            this.cmbBorrowerReceivedMethod.Visible = false;
            this.txtBorrowerReceivedMethod.BringToFront();
            this.txtBorrowerReceivedMethod.Text = this.discloseMethod[3];
          }
          else if ((DisclosureTrackingBase.DisclosedMethod) disclosureReceivedDate["CoBorrowerDisclosedMethod"] == DisclosureTrackingBase.DisclosedMethod.InPerson)
          {
            this.txtCoBorrowerReceivedMethod.Visible = false;
            this.cmbCoBorrowerReceivedMethod.Visible = true;
            this.cmbCoBorrowerReceivedMethod.SelectedItem = (object) this.discloseMethod[1];
          }
          else if ((DisclosureTrackingBase.DisclosedMethod) disclosureReceivedDate["CoBorrowerDisclosedMethod"] == DisclosureTrackingBase.DisclosedMethod.ByMail)
          {
            this.txtCoBorrowerReceivedMethod.Visible = false;
            this.cmbCoBorrowerReceivedMethod.Visible = true;
            this.cmbCoBorrowerReceivedMethod.SelectedItem = (object) this.discloseMethod[0];
          }
          this.txtCoBorrowerOther.Text = disclosureReceivedDate["BorrowerFulfillmentMethodDescription"].ToString();
          this.dpCoBorrowerActualReceivedDate.Value = Utils.ParseDate(disclosureReceivedDate["CoBorrowerActualReceivedDate"]);
          if (Utils.ParseDate(disclosureReceivedDate["CoBorrowerPresumedReceivedDate"]) != DateTime.MinValue)
            this.dpCoBorrowerReceivedDate.Value = Utils.ParseDate(disclosureReceivedDate["CoBorrowerPresumedReceivedDate"]);
        }
        this.tcDisclosure.SelectedTab = this.tabPageeDisclosure;
      }
    }

    private bool validateDisclosedDate()
    {
      if (this.currentLog.DisclosureMethod != DisclosureTrackingBase.DisclosedMethod.ByMail || !this.currentLog.IsLocked)
        return true;
      DateTime closestBusinessDay = Session.ConfigurationManager.GetBusinessCalendar(CalendarType.Postal, this.dtDisclosedDate.Value, this.dtDisclosedDate.Value.AddMonths(1)).GetNextClosestBusinessDay(this.dtDisclosedDate.Value);
      if (!(closestBusinessDay != this.dtDisclosedDate.Value))
        return true;
      int num = (int) Utils.Dialog((IWin32Window) this, "U.S. Postal Service does not deliver disclosures on Sundays and legal holidays. The date of the next available business day will be used.");
      this.dtDisclosedDate.Value = closestBusinessDay;
      return false;
    }

    private void btnClose_Click(object sender, EventArgs e) => this.Close();

    private void cmbDisclosedMethod_Leave(object sender, EventArgs e)
    {
    }

    private void cmbDisclosedMethod_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.suspendEvent || this.currentLog == null)
        return;
      DisclosureTrackingBase.DisclosedMethod disclosedMethod = !(string.Concat(this.cmbDisclosedMethod.SelectedItem) == this.discloseMethod[0]) ? (!(string.Concat(this.cmbDisclosedMethod.SelectedItem) == this.discloseMethod[1]) ? (!(string.Concat(this.cmbDisclosedMethod.SelectedItem) == this.discloseMethod[3]) ? (!(string.Concat(this.cmbDisclosedMethod.SelectedItem) == this.discloseMethod[2]) ? (!(string.Concat(this.cmbDisclosedMethod.SelectedItem) == this.discloseMethod[4]) ? (!(string.Concat(this.cmbDisclosedMethod.SelectedItem) == this.discloseMethod[5]) ? DisclosureTrackingBase.DisclosedMethod.eDisclosure : DisclosureTrackingBase.DisclosedMethod.Other) : DisclosureTrackingBase.DisclosedMethod.Fax) : DisclosureTrackingBase.DisclosedMethod.Email) : DisclosureTrackingBase.DisclosedMethod.eDisclosure) : DisclosureTrackingBase.DisclosedMethod.InPerson) : DisclosureTrackingBase.DisclosedMethod.ByMail;
      if (disclosedMethod == DisclosureTrackingBase.DisclosedMethod.Other)
        this.dpBorrowerReceivedDate.Value = this.dpCoBorrowerReceivedDate.Value = DateTime.MinValue;
      else if (disclosedMethod == DisclosureTrackingBase.DisclosedMethod.eDisclosure || disclosedMethod == DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder)
        this.dpBorrowerReceivedDate.Value = this.dpCoBorrowerReceivedDate.Value = DateTime.MinValue;
      if (disclosedMethod == DisclosureTrackingBase.DisclosedMethod.Other)
      {
        this.txtMethodOther.Enabled = true;
        this.txtMethodOther.Text = this.currentLog.DisclosedMethodOther;
      }
      else
      {
        this.txtMethodOther.Enabled = false;
        this.txtMethodOther.Text = "";
      }
      if (disclosedMethod == DisclosureTrackingBase.DisclosedMethod.InPerson)
      {
        if (this.currentLog.DisclosureMethod != DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder)
        {
          this.dpBorrowerActualReceivedDate.Value = this.dtDisclosedDate.Value;
          this.cmbBorrowerReceivedMethod.SelectedItem = (object) this.discloseMethod[1];
          if (this.currentLog.CoBorrowerName.Trim() != "")
          {
            this.dpCoBorrowerActualReceivedDate.Value = this.dtDisclosedDate.Value;
            this.cmbCoBorrowerReceivedMethod.SelectedItem = (object) this.discloseMethod[1];
          }
          if (!this.lBtnBorrowerPresumed.Locked)
            this.dpBorrowerReceivedDate.Enabled = false;
          if (this.currentLog.CoBorrowerName.Trim() != "" && !this.lBtnCoBorrowerPresumed.Locked)
            this.dpCoBorrowerReceivedDate.Enabled = false;
        }
        this.dpReceivedDate_ValueChanged((object) null, (EventArgs) null);
      }
      this.dpBorrowerReceivedDate.Focus();
      this.refreshUpdatedItem();
    }

    private void txtDisclosedDailyInterest_Leave(object sender, EventArgs e)
    {
      if (this.suspendEvent || this.currentLog == null)
        return;
      if (this.intermediateData)
      {
        this.intermediateData = false;
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
        this.intermediateData = true;
        this.txtDisclosedDailyInterest.Text = str;
        this.txtDisclosedDailyInterest.SelectionStart = str.Length;
        this.intermediateData = false;
      }
    }

    private void lbtnDisclosedDailyInterest_Click(object sender, EventArgs e)
    {
      this.lbtnDisclosedDailyInterest.Locked = !this.lbtnDisclosedDailyInterest.Locked;
      if (!this.lbtnDisclosedDailyInterest.Locked)
      {
        this.txtDisclosedDailyInterest.Text = this.currentLog.DisclosedDailyInterestValue;
        this.txtDisclosedDailyInterest.Enabled = false;
      }
      else
        this.txtDisclosedDailyInterest.Enabled = true;
      this.txtDisclosedDailyInterest_Leave((object) null, (EventArgs) null);
    }

    private void btnSelect_Click(object sender, EventArgs e)
    {
      string _appliesTo = this.currentLog.DisclosedForLE ? "LE" : "CD";
      this.loanData.Calculator.UseNewCompliance(19.2M);
      using (ChangeCircumstanceSelector circumstanceSelector = new ChangeCircumstanceSelector(true, _appliesTo, this.loanData.GetField("4461") == "Y"))
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
      if (this.suspendEvent || this.currentLog == null)
        return;
      this.calculateDisclosedDate();
      this.updateNBO();
      this.saveNBOChanges();
      this.currentLog.LEDisclosedByBroker = this.chkLEDisclosedByBroker.Checked;
      if ((this.currentLog.DisclosedForLE || this.currentLog.DisclosedForCD) && this.cmbDisclosureType.SelectedItem != null)
        this.currentLog.DisclosureType = !(string.Concat(this.cmbDisclosureType.SelectedItem) == "Post Consummation") ? (DisclosureTracking2015Log.DisclosureTypeEnum) Enum.Parse(typeof (DisclosureTracking2015Log.DisclosureTypeEnum), this.cmbDisclosureType.SelectedItem.ToString()) : DisclosureTracking2015Log.DisclosureTypeEnum.PostConsummation;
      this.setDisclosedMethodValue(this.cmbDisclosedMethod);
      if (this.currentLog.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.Other)
        this.currentLog.DisclosedMethodOther = this.txtMethodOther.Text;
      this.currentLog.IsDisclosedByLocked = this.lbtnDisclosedBy.Locked;
      if (this.currentLog.IsDisclosedByLocked)
        this.currentLog.LockedDisclosedByField = this.txtDisclosedBy.Text;
      if (this.currentLog.IsLocked != this.lbtnSentDate.Locked)
      {
        DateTime date = this.currentLog.Date;
        this.currentLog.IsLocked = this.lbtnSentDate.Locked;
        this.currentLog.Date = date;
      }
      if (this.currentLog.DisclosedForCD)
        DisclosureTrackingLogUtils.SetUseForUCDExport((IDisclosureTracking2015Log) this.currentLog, this.chkUseforUCDExport.Checked);
      this.setDisclosedMethodValue(this.cmbBorrowerReceivedMethod);
      this.currentLog.IsBorrowerPresumedDateLocked = this.lBtnBorrowerPresumed.Locked;
      if (this.currentLog.IsBorrowerPresumedDateLocked)
        this.currentLog.LockedBorrowerPresumedReceivedDate = this.dpBorrowerReceivedDate.Value;
      else
        this.currentLog.BorrowerPresumedReceivedDate = this.dpBorrowerReceivedDate.Value;
      this.currentLog.IsBorrowerTypeLocked = this.lBtnBorrowerType.Locked;
      if (this.currentLog.IsBorrowerTypeLocked && this.cmbBorrowerType.SelectedIndex > 0)
        this.currentLog.LockedBorrowerType = this.borrowerType[this.cmbBorrowerType.SelectedIndex];
      this.currentLog.IsCoBorrowerTypeLocked = this.lBtnCoBorrowerType.Locked;
      if (this.currentLog.IsCoBorrowerTypeLocked && this.cmbCoBorrowerType.SelectedIndex > 0)
        this.currentLog.LockedCoBorrowerType = this.borrowerType[this.cmbCoBorrowerType.SelectedIndex];
      this.setDisclosedMethodValue(this.cmbCoBorrowerReceivedMethod);
      this.currentLog.IsCoBorrowerPresumedDateLocked = this.lBtnCoBorrowerPresumed.Locked;
      if (this.currentLog.IsCoBorrowerPresumedDateLocked)
        this.currentLog.LockedCoBorrowerPresumedReceivedDate = this.dpCoBorrowerReceivedDate.Value;
      else
        this.currentLog.CoBorrowerPresumedReceivedDate = this.dpCoBorrowerReceivedDate.Value;
      this.currentLog.CoBorrowerActualReceivedDate = this.dpCoBorrowerActualReceivedDate.Value;
      bool flag = this.currentLog.IntentToProceed != this.chkIntent.Checked;
      this.currentLog.IsIntentReceivedByLocked = this.lBtnIntentReceivedBy.Locked;
      this.currentLog.IntentToProceed = this.chkIntent.Checked;
      this.currentLog.BorrowerActualReceivedDate = this.dpBorrowerActualReceivedDate.Value;
      if (this.currentLog.IsIntentReceivedByLocked)
        this.currentLog.LockedIntentReceivedByField = this.txtIntentReceived.Text;
      if (this.currentLog.IntentToProceed)
      {
        this.currentLog.IntentToProceedDate = this.dpIntentDate.Value;
        if (this.currentLog.IntentToProceedReceivedBy == null || this.currentLog.IntentToProceedReceivedBy == "")
          this.currentLog.IntentToProceedReceivedBy = Session.UserInfo.FullName + "(" + Session.UserInfo.Userid + ")";
      }
      this.setSentMethodValue(this.cmbIntentReceivedMethod);
      this.currentLog.IntentToProceedComments = this.txtIntentComments.Text;
      DateTime dateTime;
      if (this.currentLog.IntentToProceed)
      {
        Dictionary<DisclosureTracking2015Log, bool> log = new Dictionary<DisclosureTracking2015Log, bool>();
        log.Add(this.currentLog, false);
        this.loanData.SetField("3164", this.chkIntent.Checked ? "Y" : "N");
        if (this.disclosedDate != DateTime.MinValue)
        {
          LoanData loanData = this.loanData;
          dateTime = this.disclosedDate;
          string val = dateTime.ToString("d");
          loanData.SetField("3972", val);
        }
        if (this.currentLog.IntentToProceedDate != DateTime.MinValue)
        {
          LoanData loanData = this.loanData;
          dateTime = this.currentLog.IntentToProceedDate;
          string val = dateTime.ToString("d");
          loanData.SetField("3197", val);
        }
        if (this.currentLog.IsIntentReceivedByLocked)
        {
          this.loanData.AddLock("3973");
          this.loanData.SetField("3973", this.currentLog.LockedIntentReceivedByField);
        }
        else
        {
          if (this.loanData.IsLocked("3973"))
            this.loanData.RemoveLock("3973");
          this.loanData.SetField("3973", this.currentLog.IntentToProceedReceivedBy);
        }
        this.loanData.SetField("3974", this.currentLog.IntentToProceedReceivedMethod.ToString());
        if (this.currentLog.IntentToProceedReceivedMethod == DisclosureTrackingBase.DisclosedMethod.Other)
          this.loanData.SetField("3975", this.currentLog.IntentToProceedReceivedMethodOther);
        else
          this.loanData.SetField("3975", "");
        this.loanData.SetField("3976", this.currentLog.IntentToProceedComments);
        this.loanData.Calculator.GetIntentToProceedDT2015Log(log);
      }
      else if (flag)
      {
        this.loanData.SetField("3164", "N");
        this.loanData.SetField("3972", "//");
        this.loanData.Calculator.FormCalculation("3164", (string) null, (string) null);
      }
      if (this.currentLog.DisclosedForCD)
      {
        this.currentLog.CDReasonIsChangeInAPR = this.chkReason1.Checked;
        this.currentLog.CDReasonIsChangeInLoanProduct = this.chkReason2.Checked;
        this.currentLog.CDReasonIsPrepaymentPenaltyAdded = this.chkReason3.Checked;
        this.currentLog.CDReasonIsChangeInSettlementCharges = this.chkReason4.Checked;
        this.currentLog.CDReasonIsChangedCircumstanceEligibility = this.chkReason5.Checked;
        this.currentLog.CDReasonIsRevisionsRequestedByConsumer = this.chkReason6.Checked;
        this.currentLog.CDReasonIsInterestRateDependentCharges = this.chkReason7.Checked;
        this.currentLog.CDReasonIsOther = this.chkReasonOther.Checked;
        this.currentLog.CDReasonIs24HourAdvancePreview = this.chkReason8.Checked;
        this.currentLog.CDReasonIsToleranceCure = this.chkReason9.Checked;
        this.currentLog.CDReasonIsClericalErrorCorrection = this.chkReason10.Checked;
        if (this.chkReasonOther.Checked)
          this.currentLog.CDReasonOther = this.txtReasonOther.Text;
        this.currentLog.AttributeList["ChangesCDReceivedDate"] = this.dpChangesRecievedDate.Text;
        Dictionary<string, string> attributeList = this.currentLog.AttributeList;
        dateTime = this.dpRevisedDueDate.Value;
        string str = dateTime.ToString();
        attributeList["RevisedCDDueDate"] = str;
      }
      else if (this.currentLog.DisclosedForLE || this.currentLog.DisclosedForSafeHarbor || this.currentLog.ProviderListSent || this.currentLog.ProviderListNoFeeSent)
      {
        this.currentLog.LEReasonIsChangedCircumstanceSettlementCharges = this.chkReason1.Checked;
        this.currentLog.LEReasonIsChangedCircumstanceEligibility = this.chkReason2.Checked;
        this.currentLog.LEReasonIsRevisionsRequestedByConsumer = this.chkReason3.Checked;
        this.currentLog.LEReasonIsInterestRateDependentCharges = this.chkReason4.Checked;
        this.currentLog.LEReasonIsExpiration = this.chkReason5.Checked;
        this.currentLog.LEReasonIsDelayedSettlementOnConstructionLoans = this.chkReason6.Checked;
        this.currentLog.LEReasonIsOther = this.chkReasonOther.Checked;
        if (this.chkReasonOther.Checked)
          this.currentLog.LEReasonOther = this.txtReasonOther.Text;
        Dictionary<string, string> attributeList1 = this.currentLog.AttributeList;
        dateTime = this.dpChangesRecievedDate.Value;
        string str1 = dateTime.ToString();
        attributeList1["ChangesLEReceivedDate"] = str1;
        Dictionary<string, string> attributeList2 = this.currentLog.AttributeList;
        dateTime = this.dpRevisedDueDate.Value;
        string str2 = dateTime.ToString();
        attributeList2["RevisedLEDueDate"] = str2;
      }
      this.currentLog.ChangeInCircumstance = this.txtChangedCircumstance.Text;
      this.currentLog.ChangeInCircumstanceComments = this.txtCiCComment.Text;
      this.currentLog.ChangesReceivedDate = this.dpChangesRecievedDate.Value;
      this.currentLog.RevisedDueDate = this.dpRevisedDueDate.Value;
      if (this.currentLog.AttributeList.ContainsKey("XCOCFeeLevelIndicator") && (this.currentLog.AttributeList["XCOCFeeLevelIndicator"] == "N" || this.currentLog.AttributeList["XCOCFeeLevelIndicator"] == ""))
        this.currentLog.AttributeList["XCOCFeeLevelIndicator"] = this.feeLevelIndicatorChkBx.Checked ? "Y" : "N";
      this.currentLog.AttributeList["XCOCChangedCircumstances"] = this.changedCircumstancesChkBx.Checked ? "Y" : "N";
      this.currentLog.IsDisclosedAPRLocked = this.lbtnDisclosedAPR.Locked;
      this.currentLog.DisclosedAPR = this.txtDisclosedAPR.Text;
      this.currentLog.IsDisclosedDailyInterestLocked = this.lbtnDisclosedDailyInterest.Locked;
      this.currentLog.DisclosedDailyInterest = this.txtDisclosedDailyInterest.Text;
      this.currentLog.IsDisclosedFinanceChargeLocked = this.lbtnFinanceCharge.Locked;
      this.currentLog.FinanceCharge = this.txtFinanceCharge.Text;
      if (this.currentLog.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.eDisclosure)
      {
        if (this.fulfillmentUpdated)
        {
          this.currentLog.eDisclosureManuallyFulfilledBy = this.eDisclosureManuallyFulfilledBy;
          this.currentLog.eDisclosureManualFulfillmentDate = this.eDisclosureManualFulfillmentDate;
          this.currentLog.eDisclosureManualFulfillmentMethod = this.eDisclosureManualFulfillmentMethod;
          this.currentLog.eDisclosureManualFulfillmentComment = this.eDisclosureManualFulfillmentComment;
          this.currentLog.PresumedFulfillmentDate = this.eDisclosurePresumedDate;
        }
        this.currentLog.ActualFulfillmentDate = this.dpActualFulfillmentDate.Value;
        if (this.eDisclosureManualFulfillmentMethod == DisclosureTrackingBase.DisclosedMethod.None)
        {
          if (this.txtFulfillmentMethod.Text == this.discloseMethod[1])
            this.eDisclosureManualFulfillmentMethod = DisclosureTrackingBase.DisclosedMethod.InPerson;
          else if (this.txtFulfillmentMethod.Text == this.discloseMethod[0])
            this.eDisclosureManualFulfillmentMethod = DisclosureTrackingBase.DisclosedMethod.ByMail;
        }
        Dictionary<string, object> disclosureReceivedDate = this.loanData.Calculator.CalculateNew2015DisclosureReceivedDate((IDisclosureTracking2015Log) this.currentLog, this.dtDisclosedDate.Value, this.dpPresumedFulfillmentDate.Value, this.dpActualFulfillmentDate.Value, this.eDisclosureManualFulfillmentMethod);
        this.currentLog.BorrowerFulfillmentMethodDescription = disclosureReceivedDate["BorrowerFulfillmentMethodDescription"].ToString();
        this.currentLog.CoBorrowerFulfillmentMethodDescription = disclosureReceivedDate["CoBorrowerFulfillmentMethodDescription"].ToString();
        if (this.dpBorrowerActualReceivedDate.Value == DateTime.MinValue || this.dpBorrowerActualReceivedDate.Value != DateTime.MinValue && Utils.ParseDate(disclosureReceivedDate["BorrowerActualReceivedDate"]) != DateTime.MinValue)
          this.currentLog.BorrowerActualReceivedDate = Utils.ParseDate(disclosureReceivedDate["BorrowerActualReceivedDate"]);
        if (this.dpCoBorrowerActualReceivedDate.Value == DateTime.MinValue || this.dpCoBorrowerActualReceivedDate.Value != DateTime.MinValue && Utils.ParseDate(disclosureReceivedDate["CoBorrowerActualReceivedDate"]) != DateTime.MinValue)
          this.currentLog.CoBorrowerActualReceivedDate = Utils.ParseDate(disclosureReceivedDate["CoBorrowerActualReceivedDate"]);
        this.currentLog.BorrowerDisclosedMethod = (DisclosureTrackingBase.DisclosedMethod) disclosureReceivedDate["BorrowerDisclosedMethod"];
        this.currentLog.CoBorrowerDisclosedMethod = (DisclosureTrackingBase.DisclosedMethod) disclosureReceivedDate["CoBorrowerDisclosedMethod"];
        this.currentLog.BorrowerPresumedReceivedDate = Utils.ParseDate(disclosureReceivedDate["BorrowerPresumedReceivedDate"]);
        this.currentLog.CoBorrowerPresumedReceivedDate = Utils.ParseDate(disclosureReceivedDate["CoBorrowerPresumedReceivedDate"]);
      }
      if (this.currentLog.DisclosureMethod != DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder)
        this.loanData.Calculator.CalculateLastDisclosedCDorLE((IDisclosureTracking2015Log) this.currentLog);
      double appliedCureAmount = 0.0;
      Hashtable triggerFields = (Hashtable) null;
      string cureLogComment = this.currentLog.GetCureLogComment();
      if (this.currentLog.DisclosureMethod != DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder && (this.currentLog.DisclosedForLE && this.currentLog.DisclosureType == DisclosureTracking2015Log.DisclosureTypeEnum.Revised || this.currentLog.DisclosedForCD) && cureLogComment != string.Empty && RegulationAlerts.GetGoodFaithFeeVarianceViolationAlert(this.loanData) != null)
      {
        appliedCureAmount = this.loanData.Calculator.GetRequiredVarianceCureAmount();
        triggerFields = this.loanData.Calculator.GetGFFVarianceAlertDetails();
      }
      if (this.currentLog.DisclosureMethod != DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder)
      {
        this.loanData.Calculator.UpdateLogs();
        this.currentLog.CreateCureLog(appliedCureAmount, cureLogComment, triggerFields, Session.UserID, Session.UserInfo.FullName, false);
      }
      if (this.IsConstructionPrimaryLoan)
        this.SyncToLinkedDTLog();
      this.DialogResult = DialogResult.OK;
    }

    private void SyncToLinkedDTLog()
    {
      DisclosureTracking2015Log disclosureTracking2015Log = new List<DisclosureTracking2015Log>((IEnumerable<DisclosureTracking2015Log>) this.loanData.LinkedData.GetLogList().GetAllDisclosureTracking2015Log(false)).FirstOrDefault<DisclosureTracking2015Log>((Func<DisclosureTracking2015Log, bool>) (x => x.LinkedGuid == this.currentLog.Guid));
      if (disclosureTracking2015Log == null)
        return;
      if (this.lbtnSentDate.Locked)
        disclosureTracking2015Log.LockedDisclosedDateField = this.currentLog.LockedDisclosedDateField;
      else
        disclosureTracking2015Log.OriginalDisclosedDate = this.currentLog.OriginalDisclosedDate;
      disclosureTracking2015Log.LEDisclosedByBroker = this.chkLEDisclosedByBroker.Checked;
      if ((this.currentLog.DisclosedForLE || this.currentLog.DisclosedForCD) && this.cmbDisclosureType.SelectedItem != null)
        disclosureTracking2015Log.DisclosureType = !(string.Concat(this.cmbDisclosureType.SelectedItem) == "Post Consummation") ? (DisclosureTracking2015Log.DisclosureTypeEnum) Enum.Parse(typeof (DisclosureTracking2015Log.DisclosureTypeEnum), this.cmbDisclosureType.SelectedItem.ToString()) : DisclosureTracking2015Log.DisclosureTypeEnum.PostConsummation;
      disclosureTracking2015Log.ReceivedDate = this.currentLog.ReceivedDate;
      disclosureTracking2015Log.LockedDisclosedByField = this.currentLog.LockedDisclosedByField;
      disclosureTracking2015Log.IsDisclosedByLocked = this.currentLog.IsDisclosedByLocked;
      disclosureTracking2015Log.IsLocked = this.currentLog.IsLocked;
      disclosureTracking2015Log.DisclosureMethod = this.currentLog.DisclosureMethod;
      disclosureTracking2015Log.DisclosedMethodOther = this.currentLog.DisclosedMethodOther;
      disclosureTracking2015Log.IntentToProceed = this.currentLog.IntentToProceed;
      disclosureTracking2015Log.IntentToProceedReceivedMethod = this.currentLog.IntentToProceedReceivedMethod;
      disclosureTracking2015Log.IntentToProceedDate = this.currentLog.IntentToProceedDate;
      disclosureTracking2015Log.IntentToProceedComments = this.currentLog.IntentToProceedComments;
      disclosureTracking2015Log.IntentToProceedReceivedMethodOther = this.currentLog.IntentToProceedReceivedMethodOther;
      disclosureTracking2015Log.BorrowerActualReceivedDate = this.currentLog.BorrowerActualReceivedDate;
      disclosureTracking2015Log.BorrowerDisclosedMethod = this.currentLog.BorrowerDisclosedMethod;
      disclosureTracking2015Log.BorrowerPresumedReceivedDate = this.currentLog.BorrowerPresumedReceivedDate;
      disclosureTracking2015Log.BorrowerType = this.currentLog.BorrowerType;
      disclosureTracking2015Log.IsBorrowerPresumedDateLocked = this.currentLog.IsBorrowerPresumedDateLocked;
      disclosureTracking2015Log.LockedBorrowerPresumedReceivedDate = this.currentLog.LockedBorrowerPresumedReceivedDate;
      disclosureTracking2015Log.BorrowerDisclosedMethodOther = this.currentLog.BorrowerDisclosedMethodOther;
      disclosureTracking2015Log.IsBorrowerTypeLocked = this.currentLog.IsBorrowerTypeLocked;
      disclosureTracking2015Log.LockedBorrowerType = this.currentLog.LockedBorrowerType;
      disclosureTracking2015Log.CoBorrowerActualReceivedDate = this.currentLog.CoBorrowerActualReceivedDate;
      disclosureTracking2015Log.CoBorrowerDisclosedMethod = this.currentLog.CoBorrowerDisclosedMethod;
      disclosureTracking2015Log.CoBorrowerPresumedReceivedDate = this.currentLog.CoBorrowerPresumedReceivedDate;
      disclosureTracking2015Log.CoBorrowerType = this.currentLog.CoBorrowerType;
      disclosureTracking2015Log.IsCoBorrowerPresumedDateLocked = this.currentLog.IsCoBorrowerPresumedDateLocked;
      disclosureTracking2015Log.LockedCoBorrowerPresumedReceivedDate = this.currentLog.LockedCoBorrowerPresumedReceivedDate;
      disclosureTracking2015Log.CoBorrowerDisclosedMethodOther = this.currentLog.CoBorrowerDisclosedMethodOther;
      disclosureTracking2015Log.IsCoBorrowerTypeLocked = this.currentLog.IsCoBorrowerTypeLocked;
      disclosureTracking2015Log.LockedCoBorrowerType = this.currentLog.LockedCoBorrowerType;
      disclosureTracking2015Log.eDisclosureManuallyFulfilledBy = this.currentLog.eDisclosureManuallyFulfilledBy;
      disclosureTracking2015Log.eDisclosureManualFulfillmentDate = this.currentLog.eDisclosureManualFulfillmentDate;
      disclosureTracking2015Log.eDisclosureManualFulfillmentMethod = this.currentLog.eDisclosureManualFulfillmentMethod;
      disclosureTracking2015Log.eDisclosureManualFulfillmentComment = this.currentLog.eDisclosureManualFulfillmentComment;
      disclosureTracking2015Log.PresumedFulfillmentDate = this.currentLog.PresumedFulfillmentDate;
      disclosureTracking2015Log.ActualFulfillmentDate = this.currentLog.ActualFulfillmentDate;
      disclosureTracking2015Log.DisclosureType = this.currentLog.DisclosureType;
      disclosureTracking2015Log.eDisclosureBorrowerName = this.currentLog.eDisclosureBorrowerName;
      disclosureTracking2015Log.eDisclosureBorrowerEmail = this.currentLog.eDisclosureBorrowerEmail;
      disclosureTracking2015Log.eDisclosureBorrowerAuthenticatedDate = this.currentLog.eDisclosureBorrowerAuthenticatedDate;
      disclosureTracking2015Log.eDisclosureBorrowerAuthenticatedIP = this.currentLog.eDisclosureBorrowerAuthenticatedIP;
      disclosureTracking2015Log.eDisclosureBorrowerViewMessageDate = this.currentLog.eDisclosureBorrowerViewMessageDate;
      disclosureTracking2015Log.eDisclosureBorrowerAcceptConsentDate = this.currentLog.eDisclosureBorrowerAcceptConsentDate;
      disclosureTracking2015Log.eDisclosureBorrowerRejectConsentDate = this.currentLog.eDisclosureBorrowerRejectConsentDate;
      disclosureTracking2015Log.eDisclosureBorrowerAcceptConsentIP = this.currentLog.eDisclosureBorrowerAcceptConsentIP;
      disclosureTracking2015Log.eDisclosureBorrowerRejectConsentIP = this.currentLog.eDisclosureBorrowerRejectConsentIP;
      disclosureTracking2015Log.eDisclosureBorrowereSignedDate = this.currentLog.eDisclosureBorrowereSignedDate;
      disclosureTracking2015Log.eDisclosureBorrowereSignedIP = this.currentLog.eDisclosureBorrowereSignedIP;
      disclosureTracking2015Log.eDisclosureCoBorrowerName = this.currentLog.eDisclosureCoBorrowerName;
      disclosureTracking2015Log.eDisclosureCoBorrowerEmail = this.currentLog.eDisclosureCoBorrowerEmail;
      disclosureTracking2015Log.eDisclosureCoBorrowerAuthenticatedDate = this.currentLog.eDisclosureCoBorrowerAuthenticatedDate;
      disclosureTracking2015Log.eDisclosureCoBorrowerAuthenticatedIP = this.currentLog.eDisclosureCoBorrowerAuthenticatedIP;
      disclosureTracking2015Log.eDisclosureCoBorrowerViewMessageDate = this.currentLog.eDisclosureCoBorrowerViewMessageDate;
      disclosureTracking2015Log.eDisclosureCoBorrowerAcceptConsentDate = this.currentLog.eDisclosureCoBorrowerAcceptConsentDate;
      disclosureTracking2015Log.eDisclosureCoBorrowerRejectConsentDate = this.currentLog.eDisclosureCoBorrowerRejectConsentDate;
      disclosureTracking2015Log.eDisclosureCoBorrowerAcceptConsentIP = this.currentLog.eDisclosureCoBorrowerAcceptConsentIP;
      disclosureTracking2015Log.eDisclosureCoBorrowerRejectConsentIP = this.currentLog.eDisclosureCoBorrowerRejectConsentIP;
      disclosureTracking2015Log.eDisclosureCoBorrowereSignedDate = this.currentLog.eDisclosureCoBorrowereSignedDate;
      disclosureTracking2015Log.eDisclosureCoBorrowereSignedIP = this.currentLog.eDisclosureCoBorrowereSignedIP;
      disclosureTracking2015Log.eDisclosureLOName = this.currentLog.eDisclosureLOName;
      disclosureTracking2015Log.eDisclosureLOViewMessageDate = this.currentLog.eDisclosureLOViewMessageDate;
      disclosureTracking2015Log.eDisclosureLOeSignedDate = this.currentLog.eDisclosureLOeSignedDate;
      disclosureTracking2015Log.eDisclosureLOeSignedIP = this.currentLog.eDisclosureLOeSignedIP;
      disclosureTracking2015Log.IsWetSigned = this.currentLog.IsWetSigned;
      disclosureTracking2015Log.FulfillmentOrderedBy = this.currentLog.FulfillmentOrderedBy;
      disclosureTracking2015Log.FullfillmentProcessedDate = this.currentLog.FullfillmentProcessedDate;
      disclosureTracking2015Log.PresumedFulfillmentDate = this.currentLog.PresumedFulfillmentDate;
      disclosureTracking2015Log.eDisclosurePackageCreatedDate = this.currentLog.eDisclosurePackageCreatedDate;
      disclosureTracking2015Log.eDisclosureConsentPDF = this.currentLog.eDisclosureConsentPDF;
      disclosureTracking2015Log.FulfillmentOrderedBy_CoBorrower = this.currentLog.FulfillmentOrderedBy_CoBorrower;
      disclosureTracking2015Log.FullfillmentProcessedDate_CoBorrower = this.currentLog.FullfillmentProcessedDate_CoBorrower;
      disclosureTracking2015Log.EDisclosureBorrowerDocumentViewedDate = this.currentLog.EDisclosureBorrowerDocumentViewedDate;
      disclosureTracking2015Log.EDisclosureCoborrowerDocumentViewedDate = this.currentLog.EDisclosureCoborrowerDocumentViewedDate;
      disclosureTracking2015Log.EDisclosureBorrowerLoanLevelConsent = this.currentLog.EDisclosureBorrowerLoanLevelConsent;
      disclosureTracking2015Log.EDisclosureCoBorrowerLoanLevelConsent = this.currentLog.EDisclosureCoBorrowerLoanLevelConsent;
    }

    private void setSentMethodValue(ComboBox cmbCtrl)
    {
      DisclosureTrackingBase.DisclosedMethod disclosedMethod = !(string.Concat(cmbCtrl.SelectedItem) == "") ? (!(string.Concat(cmbCtrl.SelectedItem) == this.sentMethod[0]) ? (!(string.Concat(cmbCtrl.SelectedItem) == this.sentMethod[1]) ? (!(string.Concat(cmbCtrl.SelectedItem) == this.sentMethod[2]) ? (!(string.Concat(cmbCtrl.SelectedItem) == this.sentMethod[3]) ? (!(string.Concat(cmbCtrl.SelectedItem) == this.sentMethod[4]) ? DisclosureTrackingBase.DisclosedMethod.eDisclosure : DisclosureTrackingBase.DisclosedMethod.Other) : DisclosureTrackingBase.DisclosedMethod.Signature) : DisclosureTrackingBase.DisclosedMethod.Email) : DisclosureTrackingBase.DisclosedMethod.Phone) : DisclosureTrackingBase.DisclosedMethod.InPerson) : DisclosureTrackingBase.DisclosedMethod.None;
      if (cmbCtrl.Name == "cmbIntentReceivedMethod")
      {
        this.currentLog.IntentToProceedReceivedMethod = disclosedMethod;
        if (disclosedMethod == DisclosureTrackingBase.DisclosedMethod.Other)
          this.currentLog.IntentToProceedReceivedMethodOther = this.txtIntentMethodOther.Text;
        if (disclosedMethod == DisclosureTrackingBase.DisclosedMethod.eDisclosure)
          this.currentLog.IntentToProceedDate = DateTime.MinValue;
        if (this.currentLog.IntentToProceedReceivedMethod == DisclosureTrackingBase.DisclosedMethod.ByMail && this.currentLog.IsLocked)
        {
          DateTime closestBusinessDay = Session.ConfigurationManager.GetBusinessCalendar(CalendarType.Postal, this.dpIntentDate.Value, this.dpIntentDate.Value.AddMonths(1)).GetNextClosestBusinessDay(this.dpIntentDate.Value);
          if (closestBusinessDay != this.dpIntentDate.Value)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "U.S. Postal Service does not deliver disclosures on Sundays and legal holidays. The date of the next available business day will be used.");
            this.dpIntentDate.Value = closestBusinessDay;
          }
        }
      }
      this.refreshUpdatedItem();
    }

    private void setDisclosedMethodValue(ComboBox cmbCtrl)
    {
      DisclosureTrackingBase.DisclosedMethod disclosedMethod = DisclosureTrackingBase.DisclosedMethod.ByMail;
      if (string.Concat(cmbCtrl.SelectedItem) == this.discloseMethod[0])
        disclosedMethod = DisclosureTrackingBase.DisclosedMethod.ByMail;
      else if (string.Concat(cmbCtrl.SelectedItem) == this.discloseMethod[1])
        disclosedMethod = DisclosureTrackingBase.DisclosedMethod.InPerson;
      else if (string.Concat(cmbCtrl.SelectedItem) == this.discloseMethod[2])
        disclosedMethod = DisclosureTrackingBase.DisclosedMethod.Email;
      else if (string.Concat(cmbCtrl.SelectedItem) == this.discloseMethod[3])
        disclosedMethod = DisclosureTrackingBase.DisclosedMethod.eDisclosure;
      else if (string.Concat(cmbCtrl.SelectedItem) == this.discloseMethod[4])
        disclosedMethod = DisclosureTrackingBase.DisclosedMethod.Fax;
      else if (string.Concat(cmbCtrl.SelectedItem) == this.discloseMethod[5])
        disclosedMethod = DisclosureTrackingBase.DisclosedMethod.Other;
      if (this.currentLog.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.eDisclosure)
        disclosedMethod = DisclosureTrackingBase.DisclosedMethod.eDisclosure;
      if (this.currentLog.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder)
        disclosedMethod = DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder;
      switch (cmbCtrl.Name)
      {
        case "cmbDisclosedMethod":
          this.currentLog.DisclosureMethod = disclosedMethod;
          if (disclosedMethod == DisclosureTrackingBase.DisclosedMethod.Other)
            this.currentLog.DisclosedMethodOther = this.txtMethodOther.Text;
          if (disclosedMethod == DisclosureTrackingBase.DisclosedMethod.Other || disclosedMethod == DisclosureTrackingBase.DisclosedMethod.eDisclosure || disclosedMethod == DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder)
            this.currentLog.ReceivedDate = DateTime.MinValue;
          if (this.currentLog.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.ByMail && this.currentLog.IsLocked)
          {
            DateTime closestBusinessDay = Session.ConfigurationManager.GetBusinessCalendar(CalendarType.Postal, this.dpIntentDate.Value, this.dpIntentDate.Value.AddMonths(1)).GetNextClosestBusinessDay(this.dpIntentDate.Value);
            if (closestBusinessDay != this.dpIntentDate.Value)
            {
              int num = (int) Utils.Dialog((IWin32Window) this, "U.S. Postal Service does not deliver disclosures on Sundays and legal holidays. The date of the next available business day will be used.");
              this.dpIntentDate.Value = closestBusinessDay;
              break;
            }
            break;
          }
          break;
        case "cmbIntentReceivedMethod":
          this.currentLog.IntentToProceedReceivedMethod = disclosedMethod;
          if (disclosedMethod == DisclosureTrackingBase.DisclosedMethod.Other)
            this.currentLog.IntentToProceedReceivedMethodOther = this.txtIntentMethodOther.Text;
          if (disclosedMethod == DisclosureTrackingBase.DisclosedMethod.Other || disclosedMethod == DisclosureTrackingBase.DisclosedMethod.eDisclosure || disclosedMethod == DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder)
            this.currentLog.IntentToProceedDate = DateTime.MinValue;
          if (this.currentLog.IntentToProceedReceivedMethod == DisclosureTrackingBase.DisclosedMethod.ByMail && this.currentLog.IsIntentReceivedByLocked)
          {
            DateTime closestBusinessDay = Session.ConfigurationManager.GetBusinessCalendar(CalendarType.Postal, this.dpIntentDate.Value, this.dpIntentDate.Value.AddMonths(1)).GetNextClosestBusinessDay(this.dpIntentDate.Value);
            if (closestBusinessDay != this.dpIntentDate.Value)
            {
              int num = (int) Utils.Dialog((IWin32Window) this, "U.S. Postal Service does not deliver disclosures on Sundays and legal holidays. The date of the next available business day will be used.");
              this.dpIntentDate.Value = closestBusinessDay;
              break;
            }
            break;
          }
          break;
        case "cmbBorrowerReceivedMethod":
          if (disclosedMethod != DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder)
          {
            this.currentLog.BorrowerDisclosedMethod = disclosedMethod;
            if (disclosedMethod == DisclosureTrackingBase.DisclosedMethod.Other)
              this.currentLog.BorrowerDisclosedMethodOther = this.txtBorrowerOther.Text;
            if (disclosedMethod == DisclosureTrackingBase.DisclosedMethod.Other)
              this.currentLog.BorrowerActualReceivedDate = DateTime.MinValue;
            if (this.dpBorrowerReceivedDate.Value != DateTime.MinValue)
            {
              DateTime closestBusinessDay = Session.ConfigurationManager.GetBusinessCalendar(CalendarType.Postal, this.dpBorrowerReceivedDate.Value, this.dpBorrowerReceivedDate.Value.AddMonths(1)).GetNextClosestBusinessDay(this.dpBorrowerReceivedDate.Value);
              if (closestBusinessDay != this.dpBorrowerReceivedDate.Value)
              {
                int num = (int) Utils.Dialog((IWin32Window) this, "U.S. Postal Service does not deliver disclosures on Sundays and legal holidays. The date of the next available business day will be used.");
                this.dpBorrowerReceivedDate.Value = closestBusinessDay;
                break;
              }
              break;
            }
            break;
          }
          break;
        case "cmbCoBorrowerReceivedMethod":
          if (disclosedMethod != DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder)
          {
            this.currentLog.CoBorrowerDisclosedMethod = disclosedMethod;
            if (disclosedMethod == DisclosureTrackingBase.DisclosedMethod.Other)
              this.currentLog.CoBorrowerDisclosedMethodOther = this.txtCoBorrowerOther.Text;
            if (disclosedMethod == DisclosureTrackingBase.DisclosedMethod.Other || disclosedMethod == DisclosureTrackingBase.DisclosedMethod.eDisclosure || disclosedMethod == DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder)
              this.currentLog.CoBorrowerActualReceivedDate = DateTime.MinValue;
            if (this.dpCoBorrowerReceivedDate.Value != DateTime.MinValue)
            {
              DateTime closestBusinessDay = Session.ConfigurationManager.GetBusinessCalendar(CalendarType.Postal, this.dpCoBorrowerReceivedDate.Value, this.dpCoBorrowerReceivedDate.Value.AddMonths(1)).GetNextClosestBusinessDay(this.dpCoBorrowerReceivedDate.Value);
              if (closestBusinessDay != this.dpCoBorrowerReceivedDate.Value)
              {
                int num = (int) Utils.Dialog((IWin32Window) this, "U.S. Postal Service does not deliver disclosures on Sundays and legal holidays. The date of the next available business day will be used.");
                this.dpCoBorrowerReceivedDate.Value = closestBusinessDay;
                break;
              }
              break;
            }
            break;
          }
          break;
        case "cmbNBOReceivedMethod":
          if (disclosedMethod != DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder)
          {
            this.curNBOItems[this.nboInstance].DisclosedMethod = (int) disclosedMethod;
            if (disclosedMethod == DisclosureTrackingBase.DisclosedMethod.Other)
              this.curNBOItems[this.nboInstance].DisclosedMethodOther = this.txtNBOOther.Text;
            if (disclosedMethod == DisclosureTrackingBase.DisclosedMethod.Other)
              this.curNBOItems[this.nboInstance].ActualReceivedDate = DateTime.MinValue;
            if (this.dpNBOReceivedDate.Value != DateTime.MinValue)
            {
              DateTime closestBusinessDay = Session.ConfigurationManager.GetBusinessCalendar(CalendarType.Postal, this.dpNBOReceivedDate.Value, this.dpNBOReceivedDate.Value.AddMonths(1)).GetNextClosestBusinessDay(this.dpNBOReceivedDate.Value);
              if (closestBusinessDay != this.dpNBOReceivedDate.Value)
              {
                int num = (int) Utils.Dialog((IWin32Window) this, "U.S. Postal Service does not deliver disclosures on Sundays and legal holidays. The date of the next available business day will be used.");
                this.dpNBOReceivedDate.Value = closestBusinessDay;
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
      DateTime loanTimeZone = this.currentLog.ConvertToLoanTimeZone(DateTime.Now);
      if (this.chkIntent.Checked)
      {
        this.dpIntentDate.Value = loanTimeZone;
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
      this.txtReasonOther.Enabled = this.chkReasonOther.Checked && this.isReasonsChkBoxEnabled;
      if (this.chkReasonOther.Checked)
      {
        if (this.currentLog.DisclosedForCD)
          this.txtReasonOther.Text = this.currentLog.CDReasonOther;
        else
          this.txtReasonOther.Text = this.currentLog.LEReasonOther;
      }
      else
        this.txtReasonOther.Text = "";
    }

    private void cmbBorrowerReceivedMethod_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (string.Concat(this.cmbBorrowerReceivedMethod.SelectedItem) == this.sentMethod[4])
      {
        this.txtBorrowerOther.ReadOnly = false;
        this.txtBorrowerOther.Text = this.currentLog.BorrowerDisclosedMethodOther;
      }
      else if (string.Concat(this.cmbBorrowerReceivedMethod.SelectedItem) == this.sentMethod[5])
      {
        this.txtBorrowerOther.ReadOnly = true;
        this.txtBorrowerOther.Text = this.currentLog.BorrowerFulfillmentMethodDescription;
      }
      else
      {
        this.txtBorrowerOther.ReadOnly = true;
        this.txtBorrowerOther.Text = "";
        if (!(string.Concat(this.cmbBorrowerReceivedMethod.SelectedItem) == this.sentMethod[0]))
          return;
        this.dpBorrowerActualReceivedDate.Value = this.dtDisclosedDate.Value;
        this.dpReceivedDate_ValueChanged((object) null, (EventArgs) null);
      }
    }

    private void cmbCoBorrowerReceivedMethod_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (string.Concat(this.cmbCoBorrowerReceivedMethod.SelectedItem) == this.sentMethod[4])
      {
        this.txtCoBorrowerOther.ReadOnly = false;
        this.txtCoBorrowerOther.Text = this.currentLog.CoBorrowerDisclosedMethodOther;
      }
      else if (string.Concat(this.cmbCoBorrowerReceivedMethod.SelectedItem) == this.sentMethod[5])
      {
        this.txtCoBorrowerOther.ReadOnly = true;
        this.txtCoBorrowerOther.Text = this.currentLog.CoBorrowerFulfillmentMethodDescription;
      }
      else
      {
        this.txtCoBorrowerOther.ReadOnly = true;
        this.txtCoBorrowerOther.Text = "";
        if (!(string.Concat(this.cmbCoBorrowerReceivedMethod.SelectedItem) == this.sentMethod[0]))
          return;
        this.dpCoBorrowerActualReceivedDate.Value = this.dtDisclosedDate.Value;
        this.dpReceivedDate_ValueChanged((object) null, (EventArgs) null);
      }
    }

    private void cmbIntentReceivedMethod_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (string.Concat(this.cmbIntentReceivedMethod.SelectedItem) == this.sentMethod[4])
      {
        this.txtIntentMethodOther.Enabled = true;
        this.txtIntentMethodOther.Text = this.currentLog.IntentToProceedReceivedMethodOther;
      }
      else
      {
        this.txtIntentMethodOther.Enabled = false;
        this.txtIntentMethodOther.Text = "";
      }
    }

    private void txt_KeyUp(object sender, KeyEventArgs e)
    {
      if (!e.Control || e.KeyCode != Keys.A || !(sender is TextBox))
        return;
      ((TextBoxBase) sender).SelectAll();
    }

    private void lBtnIntentReceivedBy_Click(object sender, EventArgs e)
    {
      this.lBtnIntentReceivedBy.Locked = !this.lBtnIntentReceivedBy.Locked;
      if (!this.lBtnIntentReceivedBy.Locked)
      {
        if (this.currentLog.IntentToProceedReceivedBy != null && this.currentLog.IntentToProceedReceivedBy != "")
          this.txtIntentReceived.Text = this.currentLog.IntentToProceedReceivedBy;
        else
          this.txtIntentReceived.Text = Session.UserInfo.FullName + "(" + Session.UserInfo.Userid + ")";
        this.txtIntentReceived.Enabled = false;
        this.refreshUpdatedItem();
      }
      else
      {
        this.txtIntentReceived.Text = this.currentLog.LockedIntentReceivedByField;
        this.txtIntentReceived.Enabled = true;
      }
    }

    private void dtDisclosedDate_ValueChanged(object sender, EventArgs e)
    {
      DateTime closestBusinessDay = Session.ConfigurationManager.GetBusinessCalendar(CalendarType.Postal, this.dtDisclosedDate.Value, this.dtDisclosedDate.Value.AddMonths(1)).GetNextClosestBusinessDay(this.dtDisclosedDate.Value);
      if (closestBusinessDay != this.dtDisclosedDate.Value && this.cmbDisclosedMethod.Text == this.discloseMethod[0])
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "U.S. Postal Service does not deliver disclosures on Sundays and legal holidays. The date of the next available business day will be used.");
        this.dtDisclosedDate.Value = closestBusinessDay;
      }
      if (this.cmbDisclosedMethod.Text == this.sentMethod[0] || this.cmbDisclosedMethod.Text == "" && this.txtDisclosedMethod.Text == this.sentMethod[5])
      {
        if (this.currentLog.DisclosureMethod != DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder)
        {
          if (!this.lBtnBorrowerPresumed.Locked)
            this.dpBorrowerReceivedDate.Enabled = false;
          if (this.currentLog.CoBorrowerName.Trim() != "" && !this.lBtnCoBorrowerPresumed.Locked)
            this.dpCoBorrowerReceivedDate.Enabled = false;
          if (this.hasNBOs && this.nboInstance != "")
            this.dpNBOReceivedDate.Enabled = false;
        }
        if (this.cmbDisclosedMethod.Text == "" && this.txtDisclosedMethod.Text == this.sentMethod[5])
          this.dpActualFulfillmentDate_ValueChanged((object) null, (EventArgs) null);
      }
      else if (this.currentLog.DisclosedForLE || this.currentLog.DisclosedForCD)
      {
        if (this.dtDisclosedDate.Value >= new DateTime(2029, 12, 29))
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Based on the Sent Date, the Presumed Received Date is not supported by your company's Compliance Calendar. Dates must occur within the date range provided by the Compliance Calendar." + Environment.NewLine + "For information about the Compliance Calendar contact your system administrator.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          this.dtDisclosedDate.Value = this.currentLog.DisclosedDate;
        }
        if (this.currentLog.DisclosureMethod != DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder)
        {
          if (!this.lBtnBorrowerPresumed.Locked)
            this.dpBorrowerReceivedDate.Value = this.getReceivedDate();
          if (this.currentLog.CoBorrowerName.Trim() != "" && !this.lBtnCoBorrowerPresumed.Locked)
            this.dpCoBorrowerReceivedDate.Value = this.getReceivedDate();
          if (this.hasNBOs)
          {
            this.getReceivedDateForNBOs();
            if (this.nboInstance != "")
              this.populateNBODetails();
          }
        }
      }
      this.dpBorrowerReceivedDate.MinValue = this.dpCoBorrowerReceivedDate.MinValue = this.dtDisclosedDate.Value;
      if (this.hasNBOs)
        this.setReceivedDateForNBOs();
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
          bool complianceSetting = (bool) this.loanData.Settings.ComplianceSettings[(object) "Compliance.CountDay0ForRediscloseDueDate"];
          this.dpRevisedDueDate.Value = Session.ConfigurationManager.AddBusinessDays(CalendarType.Business, date, complianceSetting ? 2 : 3, false);
        }
        catch (Exception ex)
        {
          if (ex.InnerException is ComplianceCalendarException)
          {
            this.dpRevisedDueDate.Value = DateTime.MinValue;
            throw new ComplianceCalendarException((ComplianceCalendarException) ex.InnerException, "3167");
          }
          throw ex;
        }
      }
    }

    private DateTime disclosedDate
    {
      get
      {
        if (!this.lbtnSentDate.Locked)
          return this.currentLog.OriginalDisclosedDate;
        return this.currentLog.LockedDisclosedDateField == DateTime.MinValue ? this.dtDisclosedDate.Value : this.currentLog.LockedDisclosedDateField;
      }
      set
      {
        if (!this.lbtnSentDate.Locked)
          return;
        this.currentLog.LockedDisclosedDateField = value.Date;
      }
    }

    private DateTime getReceivedDate()
    {
      DateTime date = this.dtDisclosedDate.Value;
      return Session.SessionObjects.GetBusinessCalendar(CalendarType.RegZ).AddBusinessDays(date, 3, true);
    }

    private void lBtnBorrowerPresumed_Click(object sender, EventArgs e)
    {
      this.lBtnBorrowerPresumed.Locked = !this.lBtnBorrowerPresumed.Locked;
      if (!this.lBtnBorrowerPresumed.Locked)
      {
        this.dpBorrowerReceivedDate.Value = this.currentLog.BorrowerPresumedReceivedDate;
        this.dpBorrowerReceivedDate.Enabled = false;
        this.dpBorrowerReceivedDate.ReadOnly = true;
        this.refreshUpdatedItem();
      }
      else
      {
        this.dpBorrowerReceivedDate.Value = this.currentLog.LockedBorrowerPresumedReceivedDate;
        this.dpBorrowerReceivedDate.Enabled = true;
        this.dpBorrowerReceivedDate.ReadOnly = false;
      }
      this.dpReceivedDate_ValueChanged((object) null, (EventArgs) null);
    }

    private void lBtnCoBorrowerPresumed_Click(object sender, EventArgs e)
    {
      this.lBtnCoBorrowerPresumed.Locked = !this.lBtnCoBorrowerPresumed.Locked;
      if (!this.lBtnCoBorrowerPresumed.Locked)
      {
        this.dpCoBorrowerReceivedDate.Value = this.currentLog.CoBorrowerPresumedReceivedDate;
        this.dpCoBorrowerReceivedDate.Enabled = false;
        this.dpCoBorrowerReceivedDate.ReadOnly = true;
        this.refreshUpdatedItem();
      }
      else
      {
        this.dpCoBorrowerReceivedDate.Value = this.currentLog.LockedCoBorrowerPresumedReceivedDate;
        this.dpCoBorrowerReceivedDate.Enabled = true;
        this.dpCoBorrowerReceivedDate.ReadOnly = false;
      }
      this.dpReceivedDate_ValueChanged((object) null, (EventArgs) null);
    }

    private void btnCDSnapshot_Click(object sender, EventArgs e)
    {
      int num = (int) new DisclosedCDDialog((IDisclosureTracking2015Log) this.currentLog).ShowDialog((IWin32Window) this);
    }

    private void btnItemSnapshot_Click(object sender, EventArgs e)
    {
      int num = (int) new DisclosedItemizationDialog((IDisclosureTracking2015Log) this.currentLog).ShowDialog((IWin32Window) this);
    }

    private void dpReceivedDate_ValueChanged(object sender, EventArgs e)
    {
      this.refreshIntentToProceed(this.borPre, this.borAct, this.coborPre, this.coborAct);
      this.borPre = this.dpBorrowerReceivedDate.Value;
      this.borAct = this.dpBorrowerActualReceivedDate.Value;
      this.coborPre = this.dpCoBorrowerReceivedDate.Value;
      this.coborAct = this.dpCoBorrowerActualReceivedDate.Value;
    }

    private void dtDisclosedDate_DropDown(object sender, EventArgs e)
    {
      this.dtDisclosedDate.ValueChanged -= new EventHandler(this.dtDisclosedDate_ValueChanged);
    }

    private void dtDisclosedDate_CloseUp(object sender, EventArgs e)
    {
      this.dtDisclosedDate.ValueChanged += new EventHandler(this.dtDisclosedDate_ValueChanged);
      this.dtDisclosedDate_ValueChanged((object) null, (EventArgs) null);
    }

    private void dpReceivedDate_KeyPress(object sender, KeyPressEventArgs e)
    {
      this.refreshIntentToProceed(this.borPre, this.borAct, this.coborPre, this.coborAct);
      this.borPre = this.dpBorrowerReceivedDate.Value;
      this.borAct = this.dpBorrowerActualReceivedDate.Value;
      this.coborPre = this.dpCoBorrowerReceivedDate.Value;
      this.coborAct = this.dpCoBorrowerActualReceivedDate.Value;
    }

    private void btnProviderListSnapshot_Click(object sender, EventArgs e)
    {
      int num = (int) new DisclosedSSPLDialog((IDisclosureTracking2015Log) this.currentLog).ShowDialog((IWin32Window) this);
    }

    private void cmbDisclosureType_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (!(this.cmbDisclosureType.Text == "Post Consummation") || this.currentLog.DisclosureType != DisclosureTracking2015Log.DisclosureTypeEnum.Initial)
        return;
      int num = (int) MessageBox.Show("Initial Disclosure cannot be set to Post Consummation.", "Encompass", MessageBoxButtons.OK);
      this.cmbDisclosureType.SelectedIndex = 0;
    }

    private void dpActualFulfillmentDate_ValueChanged(object sender, EventArgs e)
    {
      Dictionary<string, object> disclosureReceivedDate = this.loanData.Calculator.CalculateNew2015DisclosureReceivedDate((IDisclosureTracking2015Log) this.currentLog, this.dtDisclosedDate.Value, this.dpPresumedFulfillmentDate.Value, this.dpActualFulfillmentDate.Value, !((this.txtFulfillmentMethod.Text ?? "") == "") ? (!((this.txtFulfillmentMethod.Text ?? "") == this.discloseMethod[0]) ? (!((this.txtFulfillmentMethod.Text ?? "") == this.discloseMethod[1]) ? DisclosureTrackingBase.DisclosedMethod.eDisclosure : DisclosureTrackingBase.DisclosedMethod.InPerson) : DisclosureTrackingBase.DisclosedMethod.ByMail) : DisclosureTrackingBase.DisclosedMethod.None);
      if ((DisclosureTrackingBase.DisclosedMethod) disclosureReceivedDate["BorrowerDisclosedMethod"] == DisclosureTrackingBase.DisclosedMethod.eDisclosure)
      {
        this.txtBorrowerReceivedMethod.Visible = true;
        this.cmbBorrowerReceivedMethod.Visible = false;
        this.txtBorrowerReceivedMethod.BringToFront();
        this.txtBorrowerReceivedMethod.Text = this.discloseMethod[3];
      }
      else if ((DisclosureTrackingBase.DisclosedMethod) disclosureReceivedDate["BorrowerDisclosedMethod"] == DisclosureTrackingBase.DisclosedMethod.InPerson)
      {
        this.txtBorrowerReceivedMethod.Visible = false;
        this.cmbBorrowerReceivedMethod.Visible = true;
        this.cmbBorrowerReceivedMethod.SelectedItem = (object) this.discloseMethod[1];
      }
      else if ((DisclosureTrackingBase.DisclosedMethod) disclosureReceivedDate["BorrowerDisclosedMethod"] == DisclosureTrackingBase.DisclosedMethod.ByMail)
      {
        this.txtBorrowerReceivedMethod.Visible = false;
        this.cmbBorrowerReceivedMethod.Visible = true;
        this.cmbBorrowerReceivedMethod.SelectedItem = (object) this.discloseMethod[0];
      }
      this.txtBorrowerOther.Text = disclosureReceivedDate["BorrowerFulfillmentMethodDescription"].ToString();
      if (this.dpBorrowerActualReceivedDate.Value == DateTime.MinValue || this.dpBorrowerActualReceivedDate.Value != DateTime.MinValue && Utils.ParseDate(disclosureReceivedDate["BorrowerActualReceivedDate"]) != DateTime.MinValue)
        this.dpBorrowerActualReceivedDate.Value = Utils.ParseDate(disclosureReceivedDate["BorrowerActualReceivedDate"]);
      if (Utils.ParseDate(disclosureReceivedDate["BorrowerPresumedReceivedDate"]) != DateTime.MinValue)
        this.dpBorrowerReceivedDate.Value = Utils.ParseDate(disclosureReceivedDate["BorrowerPresumedReceivedDate"]);
      if (!(this.currentLog.CoBorrowerName.Trim() != ""))
        return;
      if ((DisclosureTrackingBase.DisclosedMethod) disclosureReceivedDate["CoBorrowerDisclosedMethod"] == DisclosureTrackingBase.DisclosedMethod.eDisclosure)
      {
        this.txtCoBorrowerReceivedMethod.Visible = true;
        this.cmbCoBorrowerReceivedMethod.Visible = false;
        this.txtCoBorrowerReceivedMethod.BringToFront();
        this.txtCoBorrowerReceivedMethod.Text = this.discloseMethod[3];
      }
      else if ((DisclosureTrackingBase.DisclosedMethod) disclosureReceivedDate["BorrowerDisclosedMethod"] == DisclosureTrackingBase.DisclosedMethod.InPerson)
      {
        this.txtCoBorrowerReceivedMethod.Visible = false;
        this.cmbCoBorrowerReceivedMethod.Visible = true;
        this.cmbCoBorrowerReceivedMethod.SelectedItem = (object) this.discloseMethod[1];
      }
      else if ((DisclosureTrackingBase.DisclosedMethod) disclosureReceivedDate["BorrowerDisclosedMethod"] == DisclosureTrackingBase.DisclosedMethod.ByMail)
      {
        this.txtCoBorrowerReceivedMethod.Visible = false;
        this.cmbCoBorrowerReceivedMethod.Visible = true;
        this.cmbCoBorrowerReceivedMethod.SelectedItem = (object) this.discloseMethod[0];
      }
      else if ((DisclosureTrackingBase.DisclosedMethod) disclosureReceivedDate["CoBorrowerDisclosedMethod"] == DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder)
      {
        this.txtCoBorrowerReceivedMethod.Visible = true;
        this.cmbCoBorrowerReceivedMethod.Visible = false;
        this.txtCoBorrowerReceivedMethod.BringToFront();
        this.txtCoBorrowerReceivedMethod.Text = "";
      }
      this.txtCoBorrowerOther.Text = disclosureReceivedDate["CoBorrowerFulfillmentMethodDescription"].ToString();
      if (this.dpCoBorrowerActualReceivedDate.Value == DateTime.MinValue || this.dpCoBorrowerActualReceivedDate.Value != DateTime.MinValue && Utils.ParseDate(disclosureReceivedDate["CoBorrowerActualReceivedDate"]) != DateTime.MinValue)
        this.dpCoBorrowerActualReceivedDate.Value = Utils.ParseDate(disclosureReceivedDate["CoBorrowerActualReceivedDate"]);
      if (!(Utils.ParseDate(disclosureReceivedDate["CoBorrowerPresumedReceivedDate"]) != DateTime.MinValue))
        return;
      this.dpCoBorrowerReceivedDate.Value = Utils.ParseDate(disclosureReceivedDate["CoBorrowerPresumedReceivedDate"]);
    }

    public void RefreshData() => this.btnOK_Click((object) null, (EventArgs) null);

    private void lBtnBorrowerType_Click(object sender, EventArgs e)
    {
      this.lBtnBorrowerType.Locked = !this.lBtnBorrowerType.Locked;
      if (this.lBtnBorrowerType.Locked)
      {
        this.cmbBorrowerType.Visible = true;
        this.txtBorrowerType.Visible = false;
        this.cmbBorrowerType.BringToFront();
        this.cmbBorrowerType.SelectedIndex = Array.IndexOf<string>(this.borrowerType, this.currentLog.LockedBorrowerType);
      }
      else
      {
        this.txtBorrowerType.Text = this.currentLog.BorrowerType;
        this.cmbBorrowerType.Visible = false;
        this.txtBorrowerType.Visible = true;
        this.txtBorrowerType.BringToFront();
      }
      this.cmbBorrowerType_SelectedIndexChanged((object) null, (EventArgs) null);
    }

    private void cmbBorrowerType_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (!this.lBtnBorrowerType.Locked)
        return;
      if (this.cmbBorrowerType.SelectedIndex < 0)
        this.cmbBorrowerType.SelectedIndex = 0;
      this.currentLog.BorrowerType = this.borrowerType[this.cmbBorrowerType.SelectedIndex];
    }

    private void lBtnCoBorrowerType_Click(object sender, EventArgs e)
    {
      this.lBtnCoBorrowerType.Locked = !this.lBtnCoBorrowerType.Locked;
      if (this.lBtnCoBorrowerType.Locked)
      {
        this.cmbCoBorrowerType.Visible = true;
        this.txtCoBorrowerType.Visible = false;
        this.cmbCoBorrowerType.SelectedIndex = Array.IndexOf<string>(this.borrowerType, this.currentLog.LockedCoBorrowerType);
      }
      else
      {
        this.txtCoBorrowerType.Text = this.currentLog.CoBorrowerType;
        this.cmbCoBorrowerType.Visible = false;
        this.txtCoBorrowerType.Visible = true;
      }
      this.cmbCoBorrowerType_SelectedIndexChanged((object) null, (EventArgs) null);
    }

    private void cmbCoBorrowerType_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (!this.lBtnCoBorrowerType.Locked)
        return;
      if (this.cmbCoBorrowerType.SelectedIndex < 0)
        this.cmbCoBorrowerType.SelectedIndex = 0;
      this.currentLog.CoBorrowerType = this.borrowerType[this.cmbCoBorrowerType.SelectedIndex];
    }

    private void detailsDisclosureRecipientDropDown_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (!this.hasNBOs)
        return;
      this.updateNBO();
      this.discRecipientSelectedIndex = this.detailsDisclosureRecipientDropDown.SelectedIndex;
      this.dtDisclosureRecipientDropDown.SelectedIndexChanged -= new EventHandler(this.dtDisclosureRecipientDropDown_SelectedIndexChanged);
      this.dtDisclosureRecipientDropDown.SelectedIndex = this.discRecipientSelectedIndex;
      this.dtDisclosureRecipientDropDown.SelectedIndexChanged += new EventHandler(this.dtDisclosureRecipientDropDown_SelectedIndexChanged);
      this.updateNBOdetails();
    }

    private void updateNBO()
    {
      if (this.nboInstance == "")
        return;
      this.curNBOItems[this.nboInstance].ActualReceivedDate = Utils.ParseDate((object) this.dpNBOActualReceivedDate.Text);
      if (this.lBtnNBOType.Locked)
        this.curNBOItems[this.nboInstance].LockedBorrowerType = this.cmbNBOType.SelectedItem == null ? "" : this.cmbNBOType.SelectedItem.ToString();
      this.curNBOItems[this.nboInstance].isBorrowerTypeLocked = this.lBtnNBOType.Locked;
      this.curNBOItems[this.nboInstance].isPresumedDateLocked = this.lBtnNBOPresumed.Locked;
      this.curNBOItems[this.nboInstance].DisclosedMethodOther = this.txtNBOOther.Text;
      this.setDisclosedMethodValue(this.cmbNBOReceivedMethod);
      this.curNBOItems[this.nboInstance].ActualReceivedDate = Utils.ParseDate((object) this.dpNBOActualReceivedDate.Text);
      if (this.curNBOItems[this.nboInstance].isPresumedDateLocked)
        this.curNBOItems[this.nboInstance].lockedPresumedReceivedDate = this.dpNBOReceivedDate.Value;
      else
        this.curNBOItems[this.nboInstance].PresumedReceivedDate = this.dpNBOReceivedDate.Value;
    }

    private void saveNBOChanges()
    {
      foreach (KeyValuePair<string, INonBorrowerOwnerItem> curNboItem in this.curNBOItems)
      {
        this.currentLog.SetnboAttributeValue(curNboItem.Key, (object) curNboItem.Value.LockedBorrowerType, DisclosureTracking2015Log.NBOUpdatableFields.LockedBorrowerType);
        this.currentLog.SetnboAttributeValue(curNboItem.Key, (object) curNboItem.Value.isBorrowerTypeLocked, DisclosureTracking2015Log.NBOUpdatableFields.isBorrowerTypeLocked);
        this.currentLog.SetnboAttributeValue(curNboItem.Key, (object) curNboItem.Value.BorrowerType, DisclosureTracking2015Log.NBOUpdatableFields.BorrowerType);
        this.currentLog.SetnboAttributeValue(curNboItem.Key, (object) curNboItem.Value.ActualReceivedDate, DisclosureTracking2015Log.NBOUpdatableFields.ActualReceivedDate);
        this.currentLog.SetnboAttributeValue(curNboItem.Key, (object) curNboItem.Value.PresumedReceivedDate, DisclosureTracking2015Log.NBOUpdatableFields.PresumedReceivedDate);
        this.currentLog.SetnboAttributeValue(curNboItem.Key, (object) curNboItem.Value.isPresumedDateLocked, DisclosureTracking2015Log.NBOUpdatableFields.isPresumedDateLocked);
        this.currentLog.SetnboAttributeValue(curNboItem.Key, (object) curNboItem.Value.lockedPresumedReceivedDate, DisclosureTracking2015Log.NBOUpdatableFields.lockedPresumedReceivedDate);
        this.currentLog.SetnboAttributeValue(curNboItem.Key, (object) curNboItem.Value.DisclosedMethod, DisclosureTracking2015Log.NBOUpdatableFields.DisclosedMethod);
        this.currentLog.SetnboAttributeValue(curNboItem.Key, (object) curNboItem.Value.DisclosedMethodOther, DisclosureTracking2015Log.NBOUpdatableFields.DisclosedMethodOther);
      }
    }

    private void refreshNBO()
    {
      if (this.currentLog.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.InPerson)
        return;
      if ((this.currentLog.DisclosedForLE || this.currentLog.DisclosedForCD) && this.currentLog.DisclosureMethod != DisclosureTrackingBase.DisclosedMethod.eDisclosure)
      {
        if (this.currentLog.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder)
          return;
        foreach (KeyValuePair<string, INonBorrowerOwnerItem> curNboItem in this.curNBOItems)
          curNboItem.Value.PresumedReceivedDate = this.getReceivedDate();
      }
      else
      {
        if (this.currentLog.DisclosureMethod != DisclosureTrackingBase.DisclosedMethod.eDisclosure)
          return;
        Dictionary<string, object> disclosureNboReceivedDate = this.loanData.Calculator.CalculateNew2015DisclosureNBOReceivedDate((IDisclosureTracking2015Log) this.currentLog, this.dtDisclosedDate.Value);
        foreach (KeyValuePair<string, INonBorrowerOwnerItem> curNboItem in this.curNBOItems)
        {
          string key1 = curNboItem.Key + "_PresumedReceiveDate";
          string key2 = curNboItem.Key + "_ActualReceivedDate";
          if (Utils.ParseDate(disclosureNboReceivedDate[key1]) != DateTime.MinValue)
            curNboItem.Value.PresumedReceivedDate = Utils.ParseDate(disclosureNboReceivedDate[key1]);
          if (curNboItem.Value.ActualReceivedDate == DateTime.MinValue || curNboItem.Value.ActualReceivedDate != DateTime.MinValue && Utils.ParseDate(disclosureNboReceivedDate[key2]) != DateTime.MinValue)
            curNboItem.Value.ActualReceivedDate = Utils.ParseDate(disclosureNboReceivedDate[key2]);
        }
      }
    }

    private void refreshNBOUpdatedItem()
    {
      if (string.Concat(this.cmbDisclosedMethod.SelectedItem) == this.discloseMethod[1])
      {
        if (this.lBtnNBOPresumed.Locked)
          return;
        this.dpNBOReceivedDate.Text = "";
      }
      else if ((this.currentLog.DisclosedForLE || this.currentLog.DisclosedForCD) && this.currentLog.DisclosureMethod != DisclosureTrackingBase.DisclosedMethod.eDisclosure)
      {
        if (this.currentLog.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder || this.lBtnNBOPresumed.Locked)
          return;
        this.dpNBOReceivedDate.Value = this.getReceivedDate();
      }
      else if (this.currentLog.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.eDisclosure)
      {
        Dictionary<string, object> disclosureNboReceivedDate = this.loanData.Calculator.CalculateNew2015DisclosureNBOReceivedDate((IDisclosureTracking2015Log) this.currentLog, this.dtDisclosedDate.Value);
        foreach (KeyValuePair<string, INonBorrowerOwnerItem> curNboItem in this.curNBOItems)
        {
          string key1 = curNboItem.Key + "_PresumedReceiveDate";
          string key2 = curNboItem.Key + "_ActualReceivedDate";
          if (Utils.ParseDate(disclosureNboReceivedDate[key1]) != DateTime.MinValue)
            curNboItem.Value.PresumedReceivedDate = Utils.ParseDate(disclosureNboReceivedDate[key1]);
          if (curNboItem.Value.ActualReceivedDate == DateTime.MinValue || curNboItem.Value.ActualReceivedDate != DateTime.MinValue && Utils.ParseDate(disclosureNboReceivedDate[key2]) != DateTime.MinValue)
            curNboItem.Value.ActualReceivedDate = Utils.ParseDate(disclosureNboReceivedDate[key2]);
        }
      }
      else
        this.dpNBOReceivedDate.Text = "";
    }

    private void showNBOFields(bool show)
    {
      if (show)
      {
        this.txtNBOType.Visible = true;
        this.cmbNBOType.Visible = true;
        this.lBtnNBOType.Visible = true;
        this.dpNBOActualReceivedDate.Visible = true;
        this.lBtnNBOPresumed.Visible = true;
        this.dpNBOReceivedDate.Visible = true;
        this.cmbNBOReceivedMethod.Visible = true;
        this.label11.Text = "Non-Borrowing Owner";
        this.grdNBODisclosureTracking.Visible = true;
        this.txtNBOOther.Visible = true;
      }
      else
      {
        this.txtNBOType.Visible = false;
        this.cmbNBOType.Visible = false;
        this.lBtnNBOType.Visible = false;
        this.dpNBOActualReceivedDate.Visible = false;
        this.lBtnNBOPresumed.Visible = false;
        this.dpNBOReceivedDate.Visible = false;
        this.cmbNBOReceivedMethod.Visible = false;
        this.txtNBOReceivedMethod.Visible = false;
        this.label11.Text = "Borrower";
        this.txtNBOOther.Visible = false;
        this.grdNBODisclosureTracking.Visible = false;
      }
    }

    private void showBorrowerFields(bool show)
    {
      if (show)
      {
        this.txtBorrowerType.Visible = true;
        this.cmbBorrowerType.Visible = true;
        this.lBtnBorrowerType.Visible = true;
        this.dpBorrowerActualReceivedDate.Visible = true;
        this.lBtnBorrowerPresumed.Visible = true;
        this.dpBorrowerReceivedDate.Visible = true;
        this.cmbBorrowerReceivedMethod.Visible = true;
        this.label11.Text = "Borrower";
        this.panel1.Enabled = true;
        this.txtBorrowerOther.Visible = true;
        this.grdDisclosureTracking.Visible = true;
      }
      else
      {
        this.txtBorrowerType.Visible = false;
        this.cmbBorrowerType.Visible = false;
        this.lBtnBorrowerType.Visible = false;
        this.dpBorrowerActualReceivedDate.Visible = false;
        this.lBtnBorrowerPresumed.Visible = false;
        this.dpBorrowerReceivedDate.Visible = false;
        this.cmbBorrowerReceivedMethod.Visible = false;
        this.txtBorrowerReceivedMethod.Visible = false;
        this.label11.Text = "Non-Borrowing Owner";
        this.panel1.Enabled = false;
        this.txtBorrowerOther.Visible = false;
        this.grdDisclosureTracking.Visible = false;
      }
    }

    private void populateNBODetails()
    {
      switch (this.curNBOItems[this.nboInstance].DisclosedMethod)
      {
        case 1:
          this.cmbNBOReceivedMethod.SelectedItem = (object) this.discloseMethod[0];
          this.cmbNBOReceivedMethod.BringToFront();
          if (this.curNBOItems[this.nboInstance].DisclosedMethod == 2)
          {
            this.txtNBOOther.Text = this.currentLog.BorrowerFulfillmentMethodDescription;
            break;
          }
          break;
        case 2:
          this.txtNBOReceivedMethod.Visible = true;
          this.cmbNBOReceivedMethod.Visible = false;
          this.txtNBOReceivedMethod.BringToFront();
          this.txtNBOOther.Text = this.currentLog.BorrowerFulfillmentMethodDescription;
          this.txtNBOOther.ReadOnly = true;
          break;
        case 3:
          this.cmbNBOReceivedMethod.SelectedItem = (object) this.discloseMethod[4];
          this.cmbNBOReceivedMethod.BringToFront();
          break;
        case 4:
          this.cmbNBOReceivedMethod.SelectedItem = (object) this.discloseMethod[1];
          this.cmbNBOReceivedMethod.BringToFront();
          if (this.curNBOItems[this.nboInstance].DisclosedMethod == 2)
          {
            this.txtNBOOther.Text = this.currentLog.BorrowerFulfillmentMethodDescription;
            break;
          }
          break;
        case 5:
          this.cmbNBOReceivedMethod.SelectedItem = (object) this.discloseMethod[5];
          this.cmbNBOReceivedMethod.BringToFront();
          this.txtNBOOther.Text = this.curNBOItems[this.nboInstance].DisclosedMethodOther;
          break;
        case 6:
          this.cmbNBOReceivedMethod.SelectedItem = (object) this.discloseMethod[2];
          this.cmbNBOReceivedMethod.BringToFront();
          break;
        case 9:
          this.cmbNBOReceivedMethod.SelectedItem = (object) -1;
          this.cmbNBOReceivedMethod.BringToFront();
          this.txtNBOOther.ReadOnly = true;
          break;
        default:
          this.cmbNBOReceivedMethod.SelectedItem = (object) this.discloseMethod[0];
          this.cmbNBOReceivedMethod.BringToFront();
          break;
      }
      if (this.curNBOItems[this.nboInstance].DisclosedMethod == 9)
        this.cmbBorrowerReceivedMethod.SelectedIndex = -1;
      this.lBtnNBOPresumed.Locked = this.curNBOItems[this.nboInstance].isPresumedDateLocked;
      this.lBtnNBOPresumed.BringToFront();
      if (this.lBtnNBOPresumed.Locked)
      {
        this.dpNBOReceivedDate.Value = this.curNBOItems[this.nboInstance].lockedPresumedReceivedDate;
        this.dpNBOReceivedDate.BringToFront();
      }
      else
      {
        if (this.curNBOItems[this.nboInstance].DisclosedMethod != 4 && this.curNBOItems[this.nboInstance].DisclosedMethod != 9)
          this.dpNBOReceivedDate.Value = this.curNBOItems[this.nboInstance].PresumedReceivedDate;
        this.dpNBOReceivedDate.BringToFront();
      }
      this.dpNBOReceivedDate.ReadOnly = !this.lBtnNBOPresumed.Locked;
      this.dpNBOReceivedDate.Enabled = this.lBtnNBOPresumed.Locked;
      this.dpNBOActualReceivedDate.Value = this.curNBOItems[this.nboInstance].ActualReceivedDate;
      this.txtNBOType.Text = this.curNBOItems[this.nboInstance].BorrowerType;
      this.txtNBOType.BringToFront();
      this.lBtnNBOType.Locked = this.curNBOItems[this.nboInstance].isBorrowerTypeLocked;
      if (this.lBtnNBOType.Locked)
      {
        this.cmbNBOType.Text = this.curNBOItems[this.nboInstance].LockedBorrowerType;
        this.cmbNBOType.BringToFront();
      }
      if (this.lBtnNBOType.Locked)
        return;
      this.cmbNBOType.Visible = false;
    }

    private void lBtnNBOPresumed_Click(object sender, EventArgs e)
    {
      if (!this.hasNBOs)
        return;
      this.lBtnNBOPresumed.Locked = !this.lBtnNBOPresumed.Locked;
      if (!this.lBtnNBOPresumed.Locked)
      {
        this.dpNBOReceivedDate.Value = this.curNBOItems[this.nboInstance].PresumedReceivedDate;
        this.dpNBOReceivedDate.Enabled = false;
        this.dpNBOReceivedDate.ReadOnly = true;
        this.refreshNBOUpdatedItem();
      }
      else
      {
        this.dpNBOReceivedDate.Value = this.curNBOItems[this.nboInstance].lockedPresumedReceivedDate;
        this.dpNBOReceivedDate.Enabled = true;
        this.dpNBOReceivedDate.ReadOnly = false;
      }
    }

    private void lBtnNBOType_Click(object sender, EventArgs e)
    {
      this.lBtnNBOType.Locked = !this.lBtnNBOType.Locked;
      if (this.lBtnNBOType.Locked)
      {
        this.cmbNBOType.Visible = true;
        switch (this.txtNBOType.Text.ToLower())
        {
          case "title only":
            this.cmbNBOType.SelectedIndex = 0;
            break;
          case "non title spouse":
            this.cmbNBOType.SelectedIndex = 1;
            break;
          case "title only trustee":
            this.cmbNBOType.SelectedIndex = 2;
            break;
          default:
            this.cmbNBOType.SelectedIndex = 3;
            break;
        }
        this.txtNBOType.Visible = false;
        this.cmbNBOType.BringToFront();
      }
      else
      {
        this.cmbNBOType.Visible = false;
        this.txtNBOType.Visible = true;
        this.txtNBOType.BringToFront();
      }
      this.cmbNBOType_SelectedIndexChanged((object) null, (EventArgs) null);
    }

    private void cmbNBOType_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (!this.hasNBOs || !this.lBtnNBOType.Locked || this.cmbNBOType.SelectedIndex >= 0)
        return;
      this.cmbNBOType.SelectedIndex = 0;
    }

    private void dpNBOReceivedDate_ValueChanged(object sender, EventArgs e)
    {
      this.refreshIntentToProceed(this.borPre, this.borAct, this.coborPre, this.coborAct);
      this.borPre = this.dpBorrowerReceivedDate.Value;
      this.borAct = this.dpBorrowerActualReceivedDate.Value;
      this.coborPre = this.dpCoBorrowerReceivedDate.Value;
      this.coborAct = this.dpCoBorrowerActualReceivedDate.Value;
    }

    private void cmbNBOReceivedMethod_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (string.Concat(this.cmbNBOReceivedMethod.SelectedItem) == this.sentMethod[4])
      {
        this.txtNBOOther.ReadOnly = false;
        this.txtNBOOther.Text = this.curNBOItems[this.nboInstance].DisclosedMethodOther;
        this.txtNBOOther.Visible = true;
      }
      else if (string.Concat(this.cmbNBOReceivedMethod.SelectedItem) == this.sentMethod[5])
      {
        this.txtNBOOther.ReadOnly = true;
        this.txtNBOOther.Text = this.currentLog.BorrowerFulfillmentMethodDescription;
      }
      else
      {
        this.txtNBOOther.ReadOnly = true;
        this.txtNBOOther.Text = "";
        if (!(string.Concat(this.cmbNBOReceivedMethod.SelectedItem) == this.sentMethod[0]))
          return;
        this.dpNBOActualReceivedDate.Value = this.dtDisclosedDate.Value;
      }
    }

    private void DisclosureDetailsDialog2015_FormClosing(object sender, FormClosingEventArgs e)
    {
    }

    private void PopulateNBODisclosureDetails()
    {
      this.grdNBODisclosureTracking.Items.Clear();
      this.addNBODisclosureGridItem("Name", this.curNBOItems[this.nboInstance].FirstName + (string.IsNullOrWhiteSpace(this.curNBOItems[this.nboInstance].MidName) ? " " : " " + this.curNBOItems[this.nboInstance].MidName + " ") + this.curNBOItems[this.nboInstance].LastName);
      this.addNBODisclosureGridItem("Email Address", this.curNBOItems[this.nboInstance].Email);
      this.addNBODisclosureGridItem("Consent when eDisclosure was sent", this.curNBOItems[this.nboInstance].eDisclosureNBOLoanLevelConsent);
      this.addNBODisclosureGridItem("Message Viewed", this.curNBOItems[this.nboInstance].eDisclosureNBOViewMessageDate);
      this.addNBODisclosureGridItem("Package Consent Form Accepted", this.curNBOItems[this.nboInstance].eDisclosureNBOAcceptConsentDate);
      this.addNBODisclosureGridItem("Package Consent Form Accepted from IP Address", this.curNBOItems[this.nboInstance].eDisclosureNBOAcceptConsentIP);
      this.addNBODisclosureGridItem("Package Consent Form Rejected", this.curNBOItems[this.nboInstance].eDisclosureNBORejectConsentDate);
      this.addNBODisclosureGridItem("Package Consent Form Rejected from IP Address", this.curNBOItems[this.nboInstance].eDisclosureNBORejectConsentIP);
      this.addNBODisclosureGridItem("Authenticated", this.curNBOItems[this.nboInstance].eDisclosureNBOAuthenticatedDate);
      this.addNBODisclosureGridItem("Authenticated from IP Address", this.curNBOItems[this.nboInstance].eDisclosureNBOAuthenticatedIP);
      this.addNBODisclosureGridItem("Document Viewed Date", this.curNBOItems[this.nboInstance].eDisclosureNBODocumentViewedDate);
      this.addNBODisclosureGridItem("eSigned Disclosures", this.curNBOItems[this.nboInstance].eDisclosureNBOSignedDate);
      this.addNBODisclosureGridItem("eSigned Disclosures from IP Address", this.curNBOItems[this.nboInstance].eDisclosureNBOeSignedIP);
    }

    private void dtDisclosureRecipientDropDown_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (!this.hasNBOs)
        return;
      this.updateNBO();
      this.discRecipientSelectedIndex = this.dtDisclosureRecipientDropDown.SelectedIndex;
      this.detailsDisclosureRecipientDropDown.SelectedIndexChanged -= new EventHandler(this.detailsDisclosureRecipientDropDown_SelectedIndexChanged);
      this.detailsDisclosureRecipientDropDown.SelectedIndex = this.discRecipientSelectedIndex;
      this.detailsDisclosureRecipientDropDown.SelectedIndexChanged += new EventHandler(this.detailsDisclosureRecipientDropDown_SelectedIndexChanged);
      this.updateNBOdetails();
    }

    private void updateNBOdetails()
    {
      this.nboInstance = "";
      if (this.discRecipients[this.discRecipientSelectedIndex].ToString() == "Default")
      {
        this.nboInstance = "";
        this.showBorrowerFields(true);
        this.showCoBorrower(true);
        this.lBtnBorrowerType.Locked = this.currentLog.IsBorrowerTypeLocked;
        if (this.lBtnBorrowerType.Locked)
        {
          this.cmbBorrowerType.Text = this.currentLog.LockedBorrowerType;
          this.cmbBorrowerType.BringToFront();
        }
        if (!this.lBtnBorrowerType.Locked)
        {
          this.cmbBorrowerType.Visible = false;
          this.txtBorrowerType.BringToFront();
        }
        this.showNBOFields(false);
      }
      else
      {
        this.nboInstance = this.discRecipients[this.discRecipientSelectedIndex].ToString();
        this.showNBOFields(true);
        this.PopulateNBODisclosureDetails();
        this.populateNBODetails();
        this.refreshNBOUpdatedItem();
        this.showBorrowerFields(false);
        this.showCoBorrower(false);
      }
    }

    private void dpBorrowerReceivedDate_Load(object sender, EventArgs e)
    {
    }

    private void grdNBODisclosureTracking_Click(object sender, EventArgs e)
    {
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
      GVColumn gvColumn11 = new GVColumn();
      GVColumn gvColumn12 = new GVColumn();
      GVColumn gvColumn13 = new GVColumn();
      GVColumn gvColumn14 = new GVColumn();
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
      this.txtCoBorrowerName = new TextBox();
      this.txtPropertyAddress = new TextBox();
      this.txtBorrowerName = new TextBox();
      this.label37 = new Label();
      this.label36 = new Label();
      this.label35 = new Label();
      this.label34 = new Label();
      this.label33 = new Label();
      this.label32 = new Label();
      this.gradientPanel3 = new GradientPanel();
      this.flowLayoutPanel2 = new FlowLayoutPanel();
      this.btnProviderListSnapshot = new Button();
      this.btnSafeHarborSnapshot = new Button();
      this.btnItemSnapshot = new Button();
      this.btnCDSnapshot = new Button();
      this.btnLESnapshot = new Button();
      this.label20 = new Label();
      this.pnlBasicInfo = new BorderPanel();
      this.detailsDisclosureRecipientDropDown = new ComboBox();
      this.btnAuditTrail1 = new Button();
      this.tableLayoutPanel1 = new TableLayoutPanel();
      this.panel1 = new Panel();
      this.chkUseforUCDExport = new CheckBox();
      this.lblUseForUCDExport = new Label();
      this.txtMethodOther = new TextBox();
      this.cmbDisclosureType = new ComboBox();
      this.pnlIntent = new Panel();
      this.lBtnIntentReceivedBy = new FieldLockButton();
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
      this.label2 = new Label();
      this.txtDisclosedBy = new TextBox();
      this.chkIntent = new CheckBox();
      this.lbtnDisclosedBy = new FieldLockButton();
      this.lbtnSentDate = new FieldLockButton();
      this.txtDisclosedMethod = new TextBox();
      this.dtDisclosedDate = new DateTimePicker();
      this.cmbDisclosedMethod = new ComboBox();
      this.label17 = new Label();
      this.label16 = new Label();
      this.label15 = new Label();
      this.panel2 = new Panel();
      this.txtNBOType = new TextBox();
      this.cmbNBOReceivedMethod = new ComboBox();
      this.lBtnNBOType = new FieldLockButton();
      this.dpBorrowerReceivedDate = new DatePicker();
      this.cmbNBOType = new ComboBox();
      this.txtBorrowerReceivedMethod = new TextBox();
      this.dpNBOActualReceivedDate = new DatePicker();
      this.txtNBOReceivedMethod = new TextBox();
      this.lBtnNBOPresumed = new FieldLockButton();
      this.dpNBOReceivedDate = new DatePicker();
      this.txtNBOOther = new TextBox();
      this.cmbCoBorrowerType = new ComboBox();
      this.cmbBorrowerType = new ComboBox();
      this.txtCoBorrowerReceivedMethod = new TextBox();
      this.dpBorrowerActualReceivedDate = new DatePicker();
      this.lBtnCoBorrowerType = new FieldLockButton();
      this.lBtnBorrowerType = new FieldLockButton();
      this.lBtnCoBorrowerPresumed = new FieldLockButton();
      this.lBtnBorrowerPresumed = new FieldLockButton();
      this.txtCoBorrowerType = new TextBox();
      this.txtBorrowerType = new TextBox();
      this.txtCoBorrowerOther = new TextBox();
      this.txtBorrowerOther = new TextBox();
      this.label13 = new Label();
      this.label21 = new Label();
      this.dpCoBorrowerActualReceivedDate = new DatePicker();
      this.label22 = new Label();
      this.cmbCoBorrowerReceivedMethod = new ComboBox();
      this.label23 = new Label();
      this.label24 = new Label();
      this.dpCoBorrowerReceivedDate = new DatePicker();
      this.label12 = new Label();
      this.label11 = new Label();
      this.label4 = new Label();
      this.cmbBorrowerReceivedMethod = new ComboBox();
      this.label3 = new Label();
      this.lblDisclosurePresumedReceivedDate = new Label();
      this.gradientPanel1 = new GradientPanel();
      this.btnEsign = new Button();
      this.label46 = new Label();
      this.label44 = new Label();
      this.chkLEDisclosedByBroker = new CheckBox();
      this.lblDisclosureInfo = new Label();
      this.detailsDisclosureRecipientLbl = new Label();
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
      this.btnAuditTrail2 = new Button();
      this.dtDisclosureRecipientDropDown = new ComboBox();
      this.dtDisclosureRecipientLbl = new Label();
      this.pnleDisclosureStatus = new BorderPanel();
      this.grdNBODisclosureTracking = new GridView();
      this.lblePackageId = new Label();
      this.label45 = new Label();
      this.lblDateeDisclosureSent = new Label();
      this.grdDisclosureTracking = new GridView();
      this.lblViewForm = new Label();
      this.llViewDetails = new Label();
      this.label19 = new Label();
      this.gradientPanel2 = new GradientPanel();
      this.label14 = new Label();
      this.pnlFulfillment = new BorderPanel();
      this.dpActualFulfillmentDate = new DatePicker();
      this.label18 = new Label();
      this.dpPresumedFulfillmentDate = new DatePicker();
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
      this.btnClose = new Button();
      this.btnOK = new Button();
      this.pnlBottom = new Panel();
      this.txtTrackingNumber = new TextBox();
      this.label47 = new Label();
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
      this.pnlBottom.SuspendLayout();
      this.SuspendLayout();
      this.tcDisclosure.Controls.Add((Control) this.tabPageDetail);
      this.tcDisclosure.Controls.Add((Control) this.tabPageReasons);
      this.tcDisclosure.Controls.Add((Control) this.tabPageeDisclosure);
      this.tcDisclosure.Dock = DockStyle.Fill;
      this.tcDisclosure.Location = new Point(3, 3);
      this.tcDisclosure.Name = "tcDisclosure";
      this.tcDisclosure.SelectedIndex = 0;
      this.tcDisclosure.Size = new Size(931, 623);
      this.tcDisclosure.TabIndex = 4;
      this.tabPageDetail.Controls.Add((Control) this.detailsPanel);
      this.tabPageDetail.Location = new Point(4, 22);
      this.tabPageDetail.Name = "tabPageDetail";
      this.tabPageDetail.Padding = new Padding(1, 3, 3, 3);
      this.tabPageDetail.Size = new Size(923, 597);
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
      this.detailsPanel.Margin = new Padding(2);
      this.detailsPanel.Name = "detailsPanel";
      this.detailsPanel.Size = new Size(919, 591);
      this.detailsPanel.TabIndex = 0;
      this.pnlLoanSnapshot.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
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
      this.pnlLoanSnapshot.Controls.Add((Control) this.txtCoBorrowerName);
      this.pnlLoanSnapshot.Controls.Add((Control) this.txtPropertyAddress);
      this.pnlLoanSnapshot.Controls.Add((Control) this.txtBorrowerName);
      this.pnlLoanSnapshot.Controls.Add((Control) this.label37);
      this.pnlLoanSnapshot.Controls.Add((Control) this.label36);
      this.pnlLoanSnapshot.Controls.Add((Control) this.label35);
      this.pnlLoanSnapshot.Controls.Add((Control) this.label34);
      this.pnlLoanSnapshot.Controls.Add((Control) this.label33);
      this.pnlLoanSnapshot.Controls.Add((Control) this.label32);
      this.pnlLoanSnapshot.Controls.Add((Control) this.gradientPanel3);
      this.pnlLoanSnapshot.Location = new Point(0, 393);
      this.pnlLoanSnapshot.Name = "pnlLoanSnapshot";
      this.pnlLoanSnapshot.Size = new Size(885, 759);
      this.pnlLoanSnapshot.TabIndex = 2;
      this.lbtnDisclosedDailyInterest.Location = new Point(588, 54);
      this.lbtnDisclosedDailyInterest.LockedStateToolTip = "Use Default Value";
      this.lbtnDisclosedDailyInterest.MaximumSize = new Size(16, 17);
      this.lbtnDisclosedDailyInterest.MinimumSize = new Size(16, 17);
      this.lbtnDisclosedDailyInterest.Name = "lbtnDisclosedDailyInterest";
      this.lbtnDisclosedDailyInterest.Size = new Size(16, 17);
      this.lbtnDisclosedDailyInterest.TabIndex = 48;
      this.lbtnDisclosedDailyInterest.UnlockedStateToolTip = "Enter Data Manually";
      this.lbtnDisclosedDailyInterest.Click += new EventHandler(this.lbtnDisclosedDailyInterest_Click);
      this.txtDisclosedDailyInterest.Location = new Point(609, 54);
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
      this.gcDocList.Size = new Size(777, 588);
      this.gcDocList.TabIndex = 25;
      this.gcDocList.Text = "Document Sent";
      this.btnViewDisclosure.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnViewDisclosure.Location = new Point(672, 2);
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
      gvColumn2.Width = 175;
      this.gvDocList.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.gvDocList.Dock = DockStyle.Fill;
      this.gvDocList.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvDocList.Location = new Point(1, 26);
      this.gvDocList.Name = "gvDocList";
      this.gvDocList.Size = new Size(775, 561);
      this.gvDocList.TabIndex = 24;
      this.txtApplicationDate.Location = new Point(609, 142);
      this.txtApplicationDate.Name = "txtApplicationDate";
      this.txtApplicationDate.ReadOnly = true;
      this.txtApplicationDate.Size = new Size(184, 20);
      this.txtApplicationDate.TabIndex = 23;
      this.label42.AutoSize = true;
      this.label42.Location = new Point(468, 144);
      this.label42.Name = "label42";
      this.label42.Size = new Size(85, 13);
      this.label42.TabIndex = 22;
      this.label42.Text = "Application Date";
      this.txtFinanceCharge.Location = new Point(609, 120);
      this.txtFinanceCharge.Name = "txtFinanceCharge";
      this.txtFinanceCharge.Size = new Size(184, 20);
      this.txtFinanceCharge.TabIndex = 22;
      this.txtFinanceCharge.Leave += new EventHandler(this.txtFinanceCharge_Leave);
      this.txtLoanProgram.Location = new Point(609, 76);
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
      this.txtPropertyZip.Location = new Point(207, 120);
      this.txtPropertyZip.Name = "txtPropertyZip";
      this.txtPropertyZip.ReadOnly = true;
      this.txtPropertyZip.Size = new Size(118, 20);
      this.txtPropertyZip.TabIndex = 13;
      this.txtPropertyState.Location = new Point(142, 120);
      this.txtPropertyState.Name = "txtPropertyState";
      this.txtPropertyState.ReadOnly = true;
      this.txtPropertyState.Size = new Size(31, 20);
      this.txtPropertyState.TabIndex = 12;
      this.txtPropertyCity.Location = new Point(142, 98);
      this.txtPropertyCity.Name = "txtPropertyCity";
      this.txtPropertyCity.ReadOnly = true;
      this.txtPropertyCity.Size = new Size(183, 20);
      this.txtPropertyCity.TabIndex = 11;
      this.txtCoBorrowerName.Location = new Point(142, 54);
      this.txtCoBorrowerName.Name = "txtCoBorrowerName";
      this.txtCoBorrowerName.ReadOnly = true;
      this.txtCoBorrowerName.Size = new Size(183, 20);
      this.txtCoBorrowerName.TabIndex = 9;
      this.txtPropertyAddress.Location = new Point(142, 76);
      this.txtPropertyAddress.Name = "txtPropertyAddress";
      this.txtPropertyAddress.ReadOnly = true;
      this.txtPropertyAddress.Size = new Size(183, 20);
      this.txtPropertyAddress.TabIndex = 10;
      this.txtBorrowerName.Location = new Point(142, 32);
      this.txtBorrowerName.Name = "txtBorrowerName";
      this.txtBorrowerName.ReadOnly = true;
      this.txtBorrowerName.Size = new Size(183, 20);
      this.txtBorrowerName.TabIndex = 8;
      this.label37.AutoSize = true;
      this.label37.Location = new Point(179, 122);
      this.label37.Name = "label37";
      this.label37.Size = new Size(22, 13);
      this.label37.TabIndex = 7;
      this.label37.Text = "Zip";
      this.label36.AutoSize = true;
      this.label36.Location = new Point(5, 78);
      this.label36.Name = "label36";
      this.label36.Size = new Size(87, 13);
      this.label36.TabIndex = 6;
      this.label36.Text = "Property Address";
      this.label35.AutoSize = true;
      this.label35.Location = new Point(5, 122);
      this.label35.Name = "label35";
      this.label35.Size = new Size(32, 13);
      this.label35.TabIndex = 5;
      this.label35.Text = "State";
      this.label34.AutoSize = true;
      this.label34.Location = new Point(5, 100);
      this.label34.Name = "label34";
      this.label34.Size = new Size(24, 13);
      this.label34.TabIndex = 4;
      this.label34.Text = "City";
      this.label33.AutoSize = true;
      this.label33.Location = new Point(5, 56);
      this.label33.Name = "label33";
      this.label33.Size = new Size(96, 13);
      this.label33.TabIndex = 3;
      this.label33.Text = "Co-Borrower Name";
      this.label32.AutoSize = true;
      this.label32.Location = new Point(5, 34);
      this.label32.Name = "label32";
      this.label32.Size = new Size(80, 13);
      this.label32.TabIndex = 2;
      this.label32.Text = "Borrower Name";
      this.gradientPanel3.Borders = AnchorStyles.Bottom;
      this.gradientPanel3.Controls.Add((Control) this.flowLayoutPanel2);
      this.gradientPanel3.Controls.Add((Control) this.label20);
      this.gradientPanel3.Dock = DockStyle.Top;
      this.gradientPanel3.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel3.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel3.Location = new Point(1, 0);
      this.gradientPanel3.Name = "gradientPanel3";
      this.gradientPanel3.Size = new Size(883, 25);
      this.gradientPanel3.Style = GradientPanel.PanelStyle.TableFooter;
      this.gradientPanel3.TabIndex = 1;
      this.flowLayoutPanel2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flowLayoutPanel2.Controls.Add((Control) this.btnProviderListSnapshot);
      this.flowLayoutPanel2.Controls.Add((Control) this.btnSafeHarborSnapshot);
      this.flowLayoutPanel2.Controls.Add((Control) this.btnItemSnapshot);
      this.flowLayoutPanel2.Controls.Add((Control) this.btnCDSnapshot);
      this.flowLayoutPanel2.Controls.Add((Control) this.btnLESnapshot);
      this.flowLayoutPanel2.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel2.Location = new Point(165, 0);
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
      this.pnlBasicInfo.Controls.Add((Control) this.detailsDisclosureRecipientDropDown);
      this.pnlBasicInfo.Controls.Add((Control) this.btnAuditTrail1);
      this.pnlBasicInfo.Controls.Add((Control) this.tableLayoutPanel1);
      this.pnlBasicInfo.Controls.Add((Control) this.gradientPanel1);
      this.pnlBasicInfo.Controls.Add((Control) this.detailsDisclosureRecipientLbl);
      this.pnlBasicInfo.Dock = DockStyle.Top;
      this.pnlBasicInfo.Location = new Point(0, 0);
      this.pnlBasicInfo.Name = "pnlBasicInfo";
      this.pnlBasicInfo.Size = new Size(902, 393);
      this.pnlBasicInfo.TabIndex = 0;
      this.detailsDisclosureRecipientDropDown.FormattingEnabled = true;
      this.detailsDisclosureRecipientDropDown.Location = new Point(148, 9);
      this.detailsDisclosureRecipientDropDown.Name = "detailsDisclosureRecipientDropDown";
      this.detailsDisclosureRecipientDropDown.Size = new Size(260, 21);
      this.detailsDisclosureRecipientDropDown.TabIndex = 12;
      this.detailsDisclosureRecipientDropDown.Visible = false;
      this.detailsDisclosureRecipientDropDown.SelectedIndexChanged += new EventHandler(this.detailsDisclosureRecipientDropDown_SelectedIndexChanged);
      this.btnAuditTrail1.Location = new Point(426, 9);
      this.btnAuditTrail1.Name = "btnAuditTrail1";
      this.btnAuditTrail1.Size = new Size(105, 23);
      this.btnAuditTrail1.TabIndex = 67;
      this.btnAuditTrail1.Text = "View Audit Trail";
      this.btnAuditTrail1.UseVisualStyleBackColor = true;
      this.btnAuditTrail1.Click += new EventHandler(this.btnAuditTrail1_Click);
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
      this.tableLayoutPanel1.Size = new Size(900, 367);
      this.tableLayoutPanel1.TabIndex = 10;
      this.panel1.Controls.Add((Control) this.chkUseforUCDExport);
      this.panel1.Controls.Add((Control) this.lblUseForUCDExport);
      this.panel1.Controls.Add((Control) this.txtMethodOther);
      this.panel1.Controls.Add((Control) this.cmbDisclosureType);
      this.panel1.Controls.Add((Control) this.pnlIntent);
      this.panel1.Controls.Add((Control) this.label2);
      this.panel1.Controls.Add((Control) this.txtDisclosedBy);
      this.panel1.Controls.Add((Control) this.chkIntent);
      this.panel1.Controls.Add((Control) this.lbtnDisclosedBy);
      this.panel1.Controls.Add((Control) this.lbtnSentDate);
      this.panel1.Controls.Add((Control) this.txtDisclosedMethod);
      this.panel1.Controls.Add((Control) this.dtDisclosedDate);
      this.panel1.Controls.Add((Control) this.cmbDisclosedMethod);
      this.panel1.Controls.Add((Control) this.label17);
      this.panel1.Controls.Add((Control) this.label16);
      this.panel1.Controls.Add((Control) this.label15);
      this.panel1.Dock = DockStyle.Fill;
      this.panel1.Location = new Point(4, 4);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(442, 359);
      this.panel1.TabIndex = 10;
      this.chkUseforUCDExport.AutoSize = true;
      this.chkUseforUCDExport.Location = new Point(158, 124);
      this.chkUseforUCDExport.Name = "chkUseforUCDExport";
      this.chkUseforUCDExport.Size = new Size(15, 14);
      this.chkUseforUCDExport.TabIndex = 62;
      this.chkUseforUCDExport.UseVisualStyleBackColor = true;
      this.lblUseForUCDExport.AutoSize = true;
      this.lblUseForUCDExport.Location = new Point(6, 125);
      this.lblUseForUCDExport.Name = "lblUseForUCDExport";
      this.lblUseForUCDExport.Size = new Size(100, 13);
      this.lblUseForUCDExport.TabIndex = 61;
      this.lblUseForUCDExport.Text = "Use for UCD Export";
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
      this.pnlIntent.BackColor = SystemColors.ControlLight;
      this.pnlIntent.Controls.Add((Control) this.lBtnIntentReceivedBy);
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
      this.pnlIntent.Location = new Point(7, 168);
      this.pnlIntent.Name = "pnlIntent";
      this.pnlIntent.Size = new Size(340, 176);
      this.pnlIntent.TabIndex = 10;
      this.lBtnIntentReceivedBy.Location = new Point(132, 25);
      this.lBtnIntentReceivedBy.LockedStateToolTip = "Use Default Value";
      this.lBtnIntentReceivedBy.MaximumSize = new Size(16, 17);
      this.lBtnIntentReceivedBy.MinimumSize = new Size(16, 17);
      this.lBtnIntentReceivedBy.Name = "lBtnIntentReceivedBy";
      this.lBtnIntentReceivedBy.Size = new Size(16, 17);
      this.lBtnIntentReceivedBy.TabIndex = 2;
      this.lBtnIntentReceivedBy.UnlockedStateToolTip = "Enter Data Manually";
      this.lBtnIntentReceivedBy.Click += new EventHandler(this.lBtnIntentReceivedBy_Click);
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
      this.label6.Location = new Point(3, 24);
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
      this.label2.AutoSize = true;
      this.label2.Location = new Point(6, 13);
      this.label2.Name = "label2";
      this.label2.Size = new Size(83, 13);
      this.label2.TabIndex = 60;
      this.label2.Text = "Disclosure Type";
      this.txtDisclosedBy.Location = new Point(158, 55);
      this.txtDisclosedBy.MaxLength = 100;
      this.txtDisclosedBy.Name = "txtDisclosedBy";
      this.txtDisclosedBy.Size = new Size(183, 20);
      this.txtDisclosedBy.TabIndex = 5;
      this.chkIntent.AutoSize = true;
      this.chkIntent.Location = new Point(10, 147);
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
      this.lbtnSentDate.Location = new Point(139, 37);
      this.lbtnSentDate.LockedStateToolTip = "Use Default Value";
      this.lbtnSentDate.MaximumSize = new Size(16, 17);
      this.lbtnSentDate.MinimumSize = new Size(16, 17);
      this.lbtnSentDate.Name = "lbtnSentDate";
      this.lbtnSentDate.Size = new Size(16, 17);
      this.lbtnSentDate.TabIndex = 2;
      this.lbtnSentDate.UnlockedStateToolTip = "Enter Data Manually";
      this.lbtnSentDate.Click += new EventHandler(this.lbtnSentDate_Click);
      this.txtDisclosedMethod.Location = new Point(158, 78);
      this.txtDisclosedMethod.Name = "txtDisclosedMethod";
      this.txtDisclosedMethod.ReadOnly = true;
      this.txtDisclosedMethod.Size = new Size(183, 20);
      this.txtDisclosedMethod.TabIndex = 7;
      this.txtDisclosedMethod.Text = "eFolder eDisclosures";
      this.txtDisclosedMethod.Visible = false;
      this.dtDisclosedDate.CalendarMonthBackground = Color.WhiteSmoke;
      this.dtDisclosedDate.CalendarTitleBackColor = Color.FromArgb(29, 110, 174);
      this.dtDisclosedDate.CustomFormat = "MM/dd/yyyy";
      this.dtDisclosedDate.Format = DateTimePickerFormat.Short;
      this.dtDisclosedDate.Location = new Point(158, 34);
      this.dtDisclosedDate.MaxDate = new DateTime(2029, 12, 31, 0, 0, 0, 0);
      this.dtDisclosedDate.MinDate = new DateTime(2000, 1, 1, 0, 0, 0, 0);
      this.dtDisclosedDate.Name = "dtDisclosedDate";
      this.dtDisclosedDate.Size = new Size(183, 20);
      this.dtDisclosedDate.TabIndex = 3;
      this.dtDisclosedDate.CloseUp += new EventHandler(this.dtDisclosedDate_CloseUp);
      this.dtDisclosedDate.ValueChanged += new EventHandler(this.dtDisclosedDate_ValueChanged);
      this.dtDisclosedDate.DropDown += new EventHandler(this.dtDisclosedDate_DropDown);
      this.cmbDisclosedMethod.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbDisclosedMethod.FormattingEnabled = true;
      this.cmbDisclosedMethod.Location = new Point(158, 78);
      this.cmbDisclosedMethod.Name = "cmbDisclosedMethod";
      this.cmbDisclosedMethod.Size = new Size(183, 21);
      this.cmbDisclosedMethod.TabIndex = 6;
      this.cmbDisclosedMethod.SelectedIndexChanged += new EventHandler(this.cmbDisclosedMethod_SelectedIndexChanged);
      this.label17.AutoSize = true;
      this.label17.Location = new Point(6, 59);
      this.label17.Name = "label17";
      this.label17.Size = new Size(19, 13);
      this.label17.TabIndex = 51;
      this.label17.Text = "By";
      this.label16.AutoSize = true;
      this.label16.Location = new Point(6, 82);
      this.label16.Name = "label16";
      this.label16.Size = new Size(68, 13);
      this.label16.TabIndex = 50;
      this.label16.Text = "Sent Method";
      this.label15.AutoSize = true;
      this.label15.Location = new Point(6, 38);
      this.label15.Name = "label15";
      this.label15.Size = new Size(55, 13);
      this.label15.TabIndex = 49;
      this.label15.Text = "Sent Date";
      this.panel2.Controls.Add((Control) this.txtNBOType);
      this.panel2.Controls.Add((Control) this.cmbNBOReceivedMethod);
      this.panel2.Controls.Add((Control) this.lBtnNBOType);
      this.panel2.Controls.Add((Control) this.dpBorrowerReceivedDate);
      this.panel2.Controls.Add((Control) this.cmbNBOType);
      this.panel2.Controls.Add((Control) this.txtBorrowerReceivedMethod);
      this.panel2.Controls.Add((Control) this.dpNBOActualReceivedDate);
      this.panel2.Controls.Add((Control) this.txtNBOReceivedMethod);
      this.panel2.Controls.Add((Control) this.lBtnNBOPresumed);
      this.panel2.Controls.Add((Control) this.dpNBOReceivedDate);
      this.panel2.Controls.Add((Control) this.txtNBOOther);
      this.panel2.Controls.Add((Control) this.cmbCoBorrowerType);
      this.panel2.Controls.Add((Control) this.cmbBorrowerType);
      this.panel2.Controls.Add((Control) this.txtCoBorrowerReceivedMethod);
      this.panel2.Controls.Add((Control) this.dpBorrowerActualReceivedDate);
      this.panel2.Controls.Add((Control) this.lBtnCoBorrowerType);
      this.panel2.Controls.Add((Control) this.lBtnBorrowerType);
      this.panel2.Controls.Add((Control) this.lBtnCoBorrowerPresumed);
      this.panel2.Controls.Add((Control) this.lBtnBorrowerPresumed);
      this.panel2.Controls.Add((Control) this.txtCoBorrowerType);
      this.panel2.Controls.Add((Control) this.txtBorrowerType);
      this.panel2.Controls.Add((Control) this.txtCoBorrowerOther);
      this.panel2.Controls.Add((Control) this.txtBorrowerOther);
      this.panel2.Controls.Add((Control) this.label13);
      this.panel2.Controls.Add((Control) this.label21);
      this.panel2.Controls.Add((Control) this.dpCoBorrowerActualReceivedDate);
      this.panel2.Controls.Add((Control) this.label22);
      this.panel2.Controls.Add((Control) this.cmbCoBorrowerReceivedMethod);
      this.panel2.Controls.Add((Control) this.label23);
      this.panel2.Controls.Add((Control) this.label24);
      this.panel2.Controls.Add((Control) this.dpCoBorrowerReceivedDate);
      this.panel2.Controls.Add((Control) this.label12);
      this.panel2.Controls.Add((Control) this.label11);
      this.panel2.Controls.Add((Control) this.label4);
      this.panel2.Controls.Add((Control) this.cmbBorrowerReceivedMethod);
      this.panel2.Controls.Add((Control) this.label3);
      this.panel2.Controls.Add((Control) this.lblDisclosurePresumedReceivedDate);
      this.panel2.Location = new Point(453, 4);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(346, 321);
      this.panel2.TabIndex = 11;
      this.txtNBOType.Location = new Point(155, 120);
      this.txtNBOType.Name = "txtNBOType";
      this.txtNBOType.ReadOnly = true;
      this.txtNBOType.Size = new Size(182, 20);
      this.txtNBOType.TabIndex = 91;
      this.txtNBOType.Visible = false;
      this.cmbNBOReceivedMethod.BackColor = SystemColors.Window;
      this.cmbNBOReceivedMethod.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbNBOReceivedMethod.FormattingEnabled = true;
      this.cmbNBOReceivedMethod.Location = new Point(155, 23);
      this.cmbNBOReceivedMethod.Name = "cmbNBOReceivedMethod";
      this.cmbNBOReceivedMethod.Size = new Size(183, 21);
      this.cmbNBOReceivedMethod.TabIndex = 90;
      this.cmbNBOReceivedMethod.Visible = false;
      this.cmbNBOReceivedMethod.SelectedIndexChanged += new EventHandler(this.cmbNBOReceivedMethod_SelectedIndexChanged);
      this.lBtnNBOType.Location = new Point(136, 124);
      this.lBtnNBOType.LockedStateToolTip = "Use Default Value";
      this.lBtnNBOType.MaximumSize = new Size(16, 16);
      this.lBtnNBOType.MinimumSize = new Size(16, 16);
      this.lBtnNBOType.Name = "lBtnNBOType";
      this.lBtnNBOType.Size = new Size(16, 16);
      this.lBtnNBOType.TabIndex = 89;
      this.lBtnNBOType.UnlockedStateToolTip = "Enter Data Manually";
      this.lBtnNBOType.Visible = false;
      this.lBtnNBOType.Click += new EventHandler(this.lBtnNBOType_Click);
      this.dpBorrowerReceivedDate.BackColor = SystemColors.Window;
      this.dpBorrowerReceivedDate.Location = new Point(155, 72);
      this.dpBorrowerReceivedDate.Margin = new Padding(4);
      this.dpBorrowerReceivedDate.MaxValue = new DateTime(2029, 12, 31, 0, 0, 0, 0);
      this.dpBorrowerReceivedDate.MinValue = new DateTime(2000, 1, 1, 0, 0, 0, 0);
      this.dpBorrowerReceivedDate.Name = "dpBorrowerReceivedDate";
      this.dpBorrowerReceivedDate.Size = new Size(183, 21);
      this.dpBorrowerReceivedDate.TabIndex = 14;
      this.dpBorrowerReceivedDate.Tag = (object) "763";
      this.dpBorrowerReceivedDate.ToolTip = "";
      this.dpBorrowerReceivedDate.Value = new DateTime(0L);
      this.dpBorrowerReceivedDate.ValueChanged += new EventHandler(this.dpReceivedDate_ValueChanged);
      this.dpBorrowerReceivedDate.Load += new EventHandler(this.dpBorrowerReceivedDate_Load);
      this.dpBorrowerReceivedDate.KeyPress += new KeyPressEventHandler(this.dpReceivedDate_KeyPress);
      this.cmbNBOType.BackColor = SystemColors.Window;
      this.cmbNBOType.FormattingEnabled = true;
      this.cmbNBOType.Location = new Point(155, 120);
      this.cmbNBOType.Name = "cmbNBOType";
      this.cmbNBOType.Size = new Size(183, 21);
      this.cmbNBOType.TabIndex = 88;
      this.cmbNBOType.Visible = false;
      this.cmbNBOType.SelectedIndexChanged += new EventHandler(this.cmbNBOType_SelectedIndexChanged);
      this.txtBorrowerReceivedMethod.BackColor = SystemColors.Control;
      this.txtBorrowerReceivedMethod.Location = new Point(155, 23);
      this.txtBorrowerReceivedMethod.Name = "txtBorrowerReceivedMethod";
      this.txtBorrowerReceivedMethod.ReadOnly = true;
      this.txtBorrowerReceivedMethod.Size = new Size(183, 20);
      this.txtBorrowerReceivedMethod.TabIndex = 12;
      this.txtBorrowerReceivedMethod.Text = "eFolder eDisclosures";
      this.txtBorrowerReceivedMethod.Visible = false;
      this.dpNBOActualReceivedDate.BackColor = SystemColors.Window;
      this.dpNBOActualReceivedDate.ForeColor = SystemColors.HotTrack;
      this.dpNBOActualReceivedDate.Location = new Point(155, 96);
      this.dpNBOActualReceivedDate.Margin = new Padding(4);
      this.dpNBOActualReceivedDate.MaxValue = new DateTime(2029, 12, 31, 0, 0, 0, 0);
      this.dpNBOActualReceivedDate.MinValue = new DateTime(2000, 1, 1, 0, 0, 0, 0);
      this.dpNBOActualReceivedDate.Name = "dpNBOActualReceivedDate";
      this.dpNBOActualReceivedDate.Size = new Size(183, 21);
      this.dpNBOActualReceivedDate.TabIndex = 87;
      this.dpNBOActualReceivedDate.Tag = (object) "763";
      this.dpNBOActualReceivedDate.ToolTip = "";
      this.dpNBOActualReceivedDate.Value = new DateTime(0L);
      this.dpNBOActualReceivedDate.Visible = false;
      this.txtNBOReceivedMethod.BackColor = SystemColors.Control;
      this.txtNBOReceivedMethod.Location = new Point(155, 23);
      this.txtNBOReceivedMethod.Name = "txtNBOReceivedMethod";
      this.txtNBOReceivedMethod.ReadOnly = true;
      this.txtNBOReceivedMethod.Size = new Size(183, 20);
      this.txtNBOReceivedMethod.TabIndex = 83;
      this.txtNBOReceivedMethod.Text = "eFolder eDisclosures";
      this.txtNBOReceivedMethod.Visible = false;
      this.lBtnNBOPresumed.Location = new Point(136, 72);
      this.lBtnNBOPresumed.LockedStateToolTip = "Use Default Value";
      this.lBtnNBOPresumed.MaximumSize = new Size(16, 17);
      this.lBtnNBOPresumed.MinimumSize = new Size(16, 17);
      this.lBtnNBOPresumed.Name = "lBtnNBOPresumed";
      this.lBtnNBOPresumed.Size = new Size(16, 17);
      this.lBtnNBOPresumed.TabIndex = 86;
      this.lBtnNBOPresumed.UnlockedStateToolTip = "Enter Data Manually";
      this.lBtnNBOPresumed.Visible = false;
      this.lBtnNBOPresumed.Click += new EventHandler(this.lBtnNBOPresumed_Click);
      this.dpNBOReceivedDate.BackColor = SystemColors.HotTrack;
      this.dpNBOReceivedDate.ForeColor = SystemColors.MenuHighlight;
      this.dpNBOReceivedDate.Location = new Point(155, 72);
      this.dpNBOReceivedDate.Margin = new Padding(4);
      this.dpNBOReceivedDate.MaxValue = new DateTime(2029, 12, 31, 0, 0, 0, 0);
      this.dpNBOReceivedDate.MinValue = new DateTime(2000, 1, 1, 0, 0, 0, 0);
      this.dpNBOReceivedDate.Name = "dpNBOReceivedDate";
      this.dpNBOReceivedDate.Size = new Size(183, 21);
      this.dpNBOReceivedDate.TabIndex = 85;
      this.dpNBOReceivedDate.Tag = (object) "763";
      this.dpNBOReceivedDate.ToolTip = "";
      this.dpNBOReceivedDate.Value = new DateTime(0L);
      this.dpNBOReceivedDate.Visible = false;
      this.dpNBOReceivedDate.ValueChanged += new EventHandler(this.dpNBOReceivedDate_ValueChanged);
      this.txtNBOOther.Location = new Point(155, 48);
      this.txtNBOOther.Name = "txtNBOOther";
      this.txtNBOOther.Size = new Size(183, 20);
      this.txtNBOOther.TabIndex = 84;
      this.txtNBOOther.Visible = false;
      this.cmbCoBorrowerType.FormattingEnabled = true;
      this.cmbCoBorrowerType.Location = new Point(155, 251);
      this.cmbCoBorrowerType.Name = "cmbCoBorrowerType";
      this.cmbCoBorrowerType.Size = new Size(183, 21);
      this.cmbCoBorrowerType.TabIndex = 82;
      this.cmbCoBorrowerType.SelectedIndexChanged += new EventHandler(this.cmbCoBorrowerType_SelectedIndexChanged);
      this.cmbBorrowerType.FormattingEnabled = true;
      this.cmbBorrowerType.Location = new Point(156, 120);
      this.cmbBorrowerType.Name = "cmbBorrowerType";
      this.cmbBorrowerType.Size = new Size(182, 21);
      this.cmbBorrowerType.TabIndex = 81;
      this.cmbBorrowerType.SelectedIndexChanged += new EventHandler(this.cmbBorrowerType_SelectedIndexChanged);
      this.txtCoBorrowerReceivedMethod.Location = new Point(155, 158);
      this.txtCoBorrowerReceivedMethod.Name = "txtCoBorrowerReceivedMethod";
      this.txtCoBorrowerReceivedMethod.ReadOnly = true;
      this.txtCoBorrowerReceivedMethod.Size = new Size(183, 20);
      this.txtCoBorrowerReceivedMethod.TabIndex = 18;
      this.txtCoBorrowerReceivedMethod.Text = "eFolder eDisclosures";
      this.txtCoBorrowerReceivedMethod.Visible = false;
      this.dpBorrowerActualReceivedDate.BackColor = SystemColors.Window;
      this.dpBorrowerActualReceivedDate.Location = new Point(155, 96);
      this.dpBorrowerActualReceivedDate.Margin = new Padding(4);
      this.dpBorrowerActualReceivedDate.MaxValue = new DateTime(2029, 12, 31, 0, 0, 0, 0);
      this.dpBorrowerActualReceivedDate.MinValue = new DateTime(2000, 1, 1, 0, 0, 0, 0);
      this.dpBorrowerActualReceivedDate.Name = "dpBorrowerActualReceivedDate";
      this.dpBorrowerActualReceivedDate.Size = new Size(183, 21);
      this.dpBorrowerActualReceivedDate.TabIndex = 15;
      this.dpBorrowerActualReceivedDate.Tag = (object) "763";
      this.dpBorrowerActualReceivedDate.ToolTip = "";
      this.dpBorrowerActualReceivedDate.Value = new DateTime(0L);
      this.dpBorrowerActualReceivedDate.ValueChanged += new EventHandler(this.dpReceivedDate_ValueChanged);
      this.dpBorrowerActualReceivedDate.KeyPress += new KeyPressEventHandler(this.dpReceivedDate_KeyPress);
      this.lBtnCoBorrowerType.Location = new Point(136, 252);
      this.lBtnCoBorrowerType.LockedStateToolTip = "Use Default Value";
      this.lBtnCoBorrowerType.MaximumSize = new Size(16, 16);
      this.lBtnCoBorrowerType.MinimumSize = new Size(16, 16);
      this.lBtnCoBorrowerType.Name = "lBtnCoBorrowerType";
      this.lBtnCoBorrowerType.Size = new Size(16, 16);
      this.lBtnCoBorrowerType.TabIndex = 80;
      this.lBtnCoBorrowerType.UnlockedStateToolTip = "Enter Data Manually";
      this.lBtnCoBorrowerType.Click += new EventHandler(this.lBtnCoBorrowerType_Click);
      this.lBtnBorrowerType.Location = new Point(136, 124);
      this.lBtnBorrowerType.LockedStateToolTip = "Use Default Value";
      this.lBtnBorrowerType.MaximumSize = new Size(16, 16);
      this.lBtnBorrowerType.MinimumSize = new Size(16, 16);
      this.lBtnBorrowerType.Name = "lBtnBorrowerType";
      this.lBtnBorrowerType.Size = new Size(16, 16);
      this.lBtnBorrowerType.TabIndex = 79;
      this.lBtnBorrowerType.UnlockedStateToolTip = "Enter Data Manually";
      this.lBtnBorrowerType.Click += new EventHandler(this.lBtnBorrowerType_Click);
      this.lBtnCoBorrowerPresumed.Location = new Point(136, 207);
      this.lBtnCoBorrowerPresumed.LockedStateToolTip = "Use Default Value";
      this.lBtnCoBorrowerPresumed.MaximumSize = new Size(16, 17);
      this.lBtnCoBorrowerPresumed.MinimumSize = new Size(16, 17);
      this.lBtnCoBorrowerPresumed.Name = "lBtnCoBorrowerPresumed";
      this.lBtnCoBorrowerPresumed.Size = new Size(16, 17);
      this.lBtnCoBorrowerPresumed.TabIndex = 78;
      this.lBtnCoBorrowerPresumed.UnlockedStateToolTip = "Enter Data Manually";
      this.lBtnCoBorrowerPresumed.Click += new EventHandler(this.lBtnCoBorrowerPresumed_Click);
      this.lBtnBorrowerPresumed.Location = new Point(136, 72);
      this.lBtnBorrowerPresumed.LockedStateToolTip = "Use Default Value";
      this.lBtnBorrowerPresumed.MaximumSize = new Size(16, 17);
      this.lBtnBorrowerPresumed.MinimumSize = new Size(16, 17);
      this.lBtnBorrowerPresumed.Name = "lBtnBorrowerPresumed";
      this.lBtnBorrowerPresumed.Size = new Size(16, 17);
      this.lBtnBorrowerPresumed.TabIndex = 77;
      this.lBtnBorrowerPresumed.UnlockedStateToolTip = "Enter Data Manually";
      this.lBtnBorrowerPresumed.Click += new EventHandler(this.lBtnBorrowerPresumed_Click);
      this.txtCoBorrowerType.Location = new Point(155, 251);
      this.txtCoBorrowerType.Name = "txtCoBorrowerType";
      this.txtCoBorrowerType.ReadOnly = true;
      this.txtCoBorrowerType.Size = new Size(183, 20);
      this.txtCoBorrowerType.TabIndex = 22;
      this.txtBorrowerType.Location = new Point(155, 120);
      this.txtBorrowerType.Name = "txtBorrowerType";
      this.txtBorrowerType.ReadOnly = true;
      this.txtBorrowerType.Size = new Size(182, 20);
      this.txtBorrowerType.TabIndex = 16;
      this.txtCoBorrowerOther.Location = new Point(155, 183);
      this.txtCoBorrowerOther.Name = "txtCoBorrowerOther";
      this.txtCoBorrowerOther.Size = new Size(183, 20);
      this.txtCoBorrowerOther.TabIndex = 19;
      this.txtBorrowerOther.Location = new Point(155, 48);
      this.txtBorrowerOther.Name = "txtBorrowerOther";
      this.txtBorrowerOther.Size = new Size(183, 20);
      this.txtBorrowerOther.TabIndex = 13;
      this.label13.AutoSize = true;
      this.label13.Location = new Point(11, (int) byte.MaxValue);
      this.label13.Name = "label13";
      this.label13.Size = new Size(76, 13);
      this.label13.TabIndex = 76;
      this.label13.Text = "Borrower Type";
      this.label21.AutoSize = true;
      this.label21.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label21.Location = new Point(11, 144);
      this.label21.Name = "label21";
      this.label21.Size = new Size(76, 13);
      this.label21.TabIndex = 75;
      this.label21.Text = "Co-Borrower";
      this.dpCoBorrowerActualReceivedDate.BackColor = SystemColors.Window;
      this.dpCoBorrowerActualReceivedDate.Location = new Point(155, 228);
      this.dpCoBorrowerActualReceivedDate.Margin = new Padding(4);
      this.dpCoBorrowerActualReceivedDate.MaxValue = new DateTime(2029, 12, 31, 0, 0, 0, 0);
      this.dpCoBorrowerActualReceivedDate.MinValue = new DateTime(2000, 1, 1, 0, 0, 0, 0);
      this.dpCoBorrowerActualReceivedDate.Name = "dpCoBorrowerActualReceivedDate";
      this.dpCoBorrowerActualReceivedDate.Size = new Size(183, 21);
      this.dpCoBorrowerActualReceivedDate.TabIndex = 21;
      this.dpCoBorrowerActualReceivedDate.Tag = (object) "763";
      this.dpCoBorrowerActualReceivedDate.ToolTip = "";
      this.dpCoBorrowerActualReceivedDate.Value = new DateTime(0L);
      this.dpCoBorrowerActualReceivedDate.ValueChanged += new EventHandler(this.dpReceivedDate_ValueChanged);
      this.dpCoBorrowerActualReceivedDate.KeyPress += new KeyPressEventHandler(this.dpReceivedDate_KeyPress);
      this.label22.AutoSize = true;
      this.label22.Location = new Point(11, 232);
      this.label22.Name = "label22";
      this.label22.Size = new Size(112, 13);
      this.label22.TabIndex = 73;
      this.label22.Text = "Actual Received Date";
      this.cmbCoBorrowerReceivedMethod.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbCoBorrowerReceivedMethod.FormattingEnabled = true;
      this.cmbCoBorrowerReceivedMethod.Location = new Point(154, 157);
      this.cmbCoBorrowerReceivedMethod.Name = "cmbCoBorrowerReceivedMethod";
      this.cmbCoBorrowerReceivedMethod.Size = new Size(183, 21);
      this.cmbCoBorrowerReceivedMethod.TabIndex = 17;
      this.cmbCoBorrowerReceivedMethod.SelectedIndexChanged += new EventHandler(this.cmbCoBorrowerReceivedMethod_SelectedIndexChanged);
      this.label23.AutoSize = true;
      this.label23.Location = new Point(11, 165);
      this.label23.Name = "label23";
      this.label23.Size = new Size(92, 13);
      this.label23.TabIndex = 71;
      this.label23.Text = "Received Method";
      this.label24.AutoSize = true;
      this.label24.Location = new Point(11, 209);
      this.label24.Name = "label24";
      this.label24.Size = new Size(129, 13);
      this.label24.TabIndex = 69;
      this.label24.Text = "Presumed Received Date";
      this.dpCoBorrowerReceivedDate.BackColor = SystemColors.Window;
      this.dpCoBorrowerReceivedDate.Location = new Point(155, 203);
      this.dpCoBorrowerReceivedDate.Margin = new Padding(4);
      this.dpCoBorrowerReceivedDate.MaxValue = new DateTime(2029, 12, 31, 0, 0, 0, 0);
      this.dpCoBorrowerReceivedDate.MinValue = new DateTime(2000, 1, 1, 0, 0, 0, 0);
      this.dpCoBorrowerReceivedDate.Name = "dpCoBorrowerReceivedDate";
      this.dpCoBorrowerReceivedDate.Size = new Size(183, 21);
      this.dpCoBorrowerReceivedDate.TabIndex = 20;
      this.dpCoBorrowerReceivedDate.Tag = (object) "763";
      this.dpCoBorrowerReceivedDate.ToolTip = "";
      this.dpCoBorrowerReceivedDate.Value = new DateTime(0L);
      this.dpCoBorrowerReceivedDate.KeyPress += new KeyPressEventHandler(this.dpReceivedDate_KeyPress);
      this.label12.AutoSize = true;
      this.label12.Location = new Point(11, 120);
      this.label12.Name = "label12";
      this.label12.Size = new Size(76, 13);
      this.label12.TabIndex = 67;
      this.label12.Text = "Borrower Type";
      this.label11.AutoSize = true;
      this.label11.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label11.Location = new Point(11, 9);
      this.label11.Name = "label11";
      this.label11.Size = new Size(57, 13);
      this.label11.TabIndex = 66;
      this.label11.Text = "Borrower";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(11, 97);
      this.label4.Name = "label4";
      this.label4.Size = new Size(112, 13);
      this.label4.TabIndex = 64;
      this.label4.Text = "Actual Received Date";
      this.cmbBorrowerReceivedMethod.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBorrowerReceivedMethod.FormattingEnabled = true;
      this.cmbBorrowerReceivedMethod.Location = new Point(155, 23);
      this.cmbBorrowerReceivedMethod.Name = "cmbBorrowerReceivedMethod";
      this.cmbBorrowerReceivedMethod.Size = new Size(183, 21);
      this.cmbBorrowerReceivedMethod.TabIndex = 11;
      this.cmbBorrowerReceivedMethod.SelectedIndexChanged += new EventHandler(this.cmbBorrowerReceivedMethod_SelectedIndexChanged);
      this.label3.AutoSize = true;
      this.label3.Location = new Point(11, 31);
      this.label3.Name = "label3";
      this.label3.Size = new Size(92, 13);
      this.label3.TabIndex = 62;
      this.label3.Text = "Received Method";
      this.lblDisclosurePresumedReceivedDate.AutoSize = true;
      this.lblDisclosurePresumedReceivedDate.Location = new Point(11, 74);
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
      this.gradientPanel1.Size = new Size(900, 25);
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
      this.lblDisclosureInfo.Location = new Point(3, -11);
      this.lblDisclosureInfo.Name = "lblDisclosureInfo";
      this.lblDisclosureInfo.Size = new Size(133, 14);
      this.lblDisclosureInfo.TabIndex = 1;
      this.lblDisclosureInfo.Text = "Disclosure Information";
      this.detailsDisclosureRecipientLbl.AutoSize = true;
      this.detailsDisclosureRecipientLbl.Location = new Point(7, 12);
      this.detailsDisclosureRecipientLbl.Name = "detailsDisclosureRecipientLbl";
      this.detailsDisclosureRecipientLbl.Size = new Size(137, 13);
      this.detailsDisclosureRecipientLbl.TabIndex = 11;
      this.detailsDisclosureRecipientLbl.Text = "Select Disclosure Recipient";
      this.detailsDisclosureRecipientLbl.Visible = false;
      this.tabPageReasons.Controls.Add((Control) this.reasonsPanel);
      this.tabPageReasons.Location = new Point(4, 22);
      this.tabPageReasons.Name = "tabPageReasons";
      this.tabPageReasons.Size = new Size(923, 597);
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
      this.reasonsPanel.Margin = new Padding(2);
      this.reasonsPanel.Name = "reasonsPanel";
      this.reasonsPanel.Size = new Size(923, 597);
      this.reasonsPanel.TabIndex = 0;
      this.borderPanel1.Controls.Add((Control) this.panel5);
      this.borderPanel1.Controls.Add((Control) this.pnlReason);
      this.borderPanel1.Dock = DockStyle.Fill;
      this.borderPanel1.Location = new Point(0, 25);
      this.borderPanel1.Name = "borderPanel1";
      this.borderPanel1.Size = new Size(906, 1127);
      this.borderPanel1.TabIndex = 17;
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
      this.panel5.Size = new Size(904, 795);
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
      gvColumn8.Width = 174;
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
      this.feeDetailsGV.Size = new Size(876, 624);
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
      this.pnlReason.Size = new Size(904, 330);
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
      this.txtReasonOther.Location = new Point(71, 284);
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
      this.chkReason5.Location = new Point(13, 145);
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
      this.chkReason1.Location = new Point(13, 50);
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
      this.gradientPanel5.Size = new Size(906, 25);
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
      this.tabPageeDisclosure.Controls.Add((Control) this.btnAuditTrail2);
      this.tabPageeDisclosure.Controls.Add((Control) this.dtDisclosureRecipientDropDown);
      this.tabPageeDisclosure.Controls.Add((Control) this.dtDisclosureRecipientLbl);
      this.tabPageeDisclosure.Controls.Add((Control) this.pnleDisclosureStatus);
      this.tabPageeDisclosure.Controls.Add((Control) this.pnlFulfillment);
      this.tabPageeDisclosure.Location = new Point(4, 22);
      this.tabPageeDisclosure.Margin = new Padding(1, 3, 3, 3);
      this.tabPageeDisclosure.Name = "tabPageeDisclosure";
      this.tabPageeDisclosure.Padding = new Padding(0, 3, 3, 3);
      this.tabPageeDisclosure.Size = new Size(923, 597);
      this.tabPageeDisclosure.TabIndex = 1;
      this.tabPageeDisclosure.Text = "eDisclosure Tracking";
      this.tabPageeDisclosure.UseVisualStyleBackColor = true;
      this.btnAuditTrail2.Location = new Point(426, 9);
      this.btnAuditTrail2.Name = "btnAuditTrail2";
      this.btnAuditTrail2.Size = new Size(105, 23);
      this.btnAuditTrail2.TabIndex = 67;
      this.btnAuditTrail2.Text = "View Audit Trail";
      this.btnAuditTrail2.UseVisualStyleBackColor = true;
      this.btnAuditTrail2.Click += new EventHandler(this.btnAuditTrail2_Click);
      this.dtDisclosureRecipientDropDown.FormattingEnabled = true;
      this.dtDisclosureRecipientDropDown.Location = new Point(146, 9);
      this.dtDisclosureRecipientDropDown.Name = "dtDisclosureRecipientDropDown";
      this.dtDisclosureRecipientDropDown.Size = new Size(261, 21);
      this.dtDisclosureRecipientDropDown.TabIndex = 13;
      this.dtDisclosureRecipientDropDown.Visible = false;
      this.dtDisclosureRecipientDropDown.SelectedIndexChanged += new EventHandler(this.dtDisclosureRecipientDropDown_SelectedIndexChanged);
      this.dtDisclosureRecipientLbl.AutoSize = true;
      this.dtDisclosureRecipientLbl.Location = new Point(3, 12);
      this.dtDisclosureRecipientLbl.Name = "dtDisclosureRecipientLbl";
      this.dtDisclosureRecipientLbl.Size = new Size(137, 13);
      this.dtDisclosureRecipientLbl.TabIndex = 12;
      this.dtDisclosureRecipientLbl.Text = "Select Disclosure Recipient";
      this.dtDisclosureRecipientLbl.Visible = false;
      this.pnleDisclosureStatus.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
      this.pnleDisclosureStatus.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.pnleDisclosureStatus.Controls.Add((Control) this.grdNBODisclosureTracking);
      this.pnleDisclosureStatus.Controls.Add((Control) this.lblePackageId);
      this.pnleDisclosureStatus.Controls.Add((Control) this.label45);
      this.pnleDisclosureStatus.Controls.Add((Control) this.lblDateeDisclosureSent);
      this.pnleDisclosureStatus.Controls.Add((Control) this.grdDisclosureTracking);
      this.pnleDisclosureStatus.Controls.Add((Control) this.lblViewForm);
      this.pnleDisclosureStatus.Controls.Add((Control) this.llViewDetails);
      this.pnleDisclosureStatus.Controls.Add((Control) this.label19);
      this.pnleDisclosureStatus.Controls.Add((Control) this.gradientPanel2);
      this.pnleDisclosureStatus.Location = new Point(1, 41);
      this.pnleDisclosureStatus.Name = "pnleDisclosureStatus";
      this.pnleDisclosureStatus.Size = new Size(735, 269);
      this.pnleDisclosureStatus.TabIndex = 0;
      this.grdNBODisclosureTracking.AllowMultiselect = false;
      this.grdNBODisclosureTracking.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
      this.grdNBODisclosureTracking.AutoHeight = true;
      this.grdNBODisclosureTracking.BorderColor = Color.LightGray;
      this.grdNBODisclosureTracking.BorderStyle = BorderStyle.FixedSingle;
      gvColumn9.ImageIndex = -1;
      gvColumn9.Name = "Column1";
      gvColumn9.Text = " ";
      gvColumn9.Width = 217;
      gvColumn10.ImageIndex = -1;
      gvColumn10.Name = "NBO";
      gvColumn10.SortMethod = GVSortMethod.None;
      gvColumn10.Text = "Non-Borrowing Owner";
      gvColumn10.Width = 155;
      this.grdNBODisclosureTracking.Columns.AddRange(new GVColumn[2]
      {
        gvColumn9,
        gvColumn10
      });
      this.grdNBODisclosureTracking.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.grdNBODisclosureTracking.Location = new Point(8, 75);
      this.grdNBODisclosureTracking.Name = "grdNBODisclosureTracking";
      this.grdNBODisclosureTracking.Size = new Size(663, 191);
      this.grdNBODisclosureTracking.TabIndex = 71;
      this.grdNBODisclosureTracking.Visible = false;
      this.grdNBODisclosureTracking.Click += new EventHandler(this.grdNBODisclosureTracking_Click);
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
      this.grdDisclosureTracking.AllowMultiselect = false;
      this.grdDisclosureTracking.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
      this.grdDisclosureTracking.AutoHeight = true;
      this.grdDisclosureTracking.BorderColor = Color.LightGray;
      this.grdDisclosureTracking.BorderStyle = BorderStyle.FixedSingle;
      gvColumn11.ImageIndex = -1;
      gvColumn11.Name = "Column1";
      gvColumn11.Text = " ";
      gvColumn11.Width = 217;
      gvColumn12.ImageIndex = -1;
      gvColumn12.Name = "Borrower";
      gvColumn12.SortMethod = GVSortMethod.None;
      gvColumn12.Text = "Borrower";
      gvColumn12.Width = 155;
      gvColumn13.ImageIndex = -1;
      gvColumn13.Name = "Co-Borrower";
      gvColumn13.Text = "Co-Borrower";
      gvColumn13.Width = 155;
      gvColumn14.ImageIndex = -1;
      gvColumn14.Name = "Loan Originator";
      gvColumn14.Text = "Loan Originator";
      gvColumn14.Width = 150;
      this.grdDisclosureTracking.Columns.AddRange(new GVColumn[4]
      {
        gvColumn11,
        gvColumn12,
        gvColumn13,
        gvColumn14
      });
      this.grdDisclosureTracking.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.grdDisclosureTracking.Location = new Point(8, 75);
      this.grdDisclosureTracking.Name = "grdDisclosureTracking";
      this.grdDisclosureTracking.Size = new Size(663, 191);
      this.grdDisclosureTracking.TabIndex = 67;
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
      this.pnlFulfillment.Controls.Add((Control) this.label47);
      this.pnlFulfillment.Controls.Add((Control) this.dpActualFulfillmentDate);
      this.pnlFulfillment.Controls.Add((Control) this.label18);
      this.pnlFulfillment.Controls.Add((Control) this.dpPresumedFulfillmentDate);
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
      this.pnlFulfillment.Location = new Point(2, 316);
      this.pnlFulfillment.Name = "pnlFulfillment";
      this.pnlFulfillment.Size = new Size(734, 265);
      this.pnlFulfillment.TabIndex = 1;
      this.dpActualFulfillmentDate.BackColor = SystemColors.Window;
      this.dpActualFulfillmentDate.Location = new Point(149, 143);
      this.dpActualFulfillmentDate.Margin = new Padding(4);
      this.dpActualFulfillmentDate.MaxValue = new DateTime(2029, 12, 31, 0, 0, 0, 0);
      this.dpActualFulfillmentDate.MinValue = new DateTime(2000, 1, 1, 0, 0, 0, 0);
      this.dpActualFulfillmentDate.Name = "dpActualFulfillmentDate";
      this.dpActualFulfillmentDate.Size = new Size(258, 21);
      this.dpActualFulfillmentDate.TabIndex = 70;
      this.dpActualFulfillmentDate.Tag = (object) "763";
      this.dpActualFulfillmentDate.ToolTip = "";
      this.dpActualFulfillmentDate.Value = new DateTime(0L);
      this.dpActualFulfillmentDate.ValueChanged += new EventHandler(this.dpActualFulfillmentDate_ValueChanged);
      this.label18.AutoSize = true;
      this.label18.Location = new Point(6, 147);
      this.label18.Name = "label18";
      this.label18.Size = new Size(112, 13);
      this.label18.TabIndex = 70;
      this.label18.Text = "Actual Received Date";
      this.dpPresumedFulfillmentDate.BackColor = SystemColors.Window;
      this.dpPresumedFulfillmentDate.Location = new Point(149, 121);
      this.dpPresumedFulfillmentDate.Margin = new Padding(4);
      this.dpPresumedFulfillmentDate.MaxValue = new DateTime(2029, 12, 31, 0, 0, 0, 0);
      this.dpPresumedFulfillmentDate.MinValue = new DateTime(2000, 1, 1, 0, 0, 0, 0);
      this.dpPresumedFulfillmentDate.Name = "dpPresumedFulfillmentDate";
      this.dpPresumedFulfillmentDate.ReadOnly = true;
      this.dpPresumedFulfillmentDate.Size = new Size(258, 21);
      this.dpPresumedFulfillmentDate.TabIndex = 68;
      this.dpPresumedFulfillmentDate.Tag = (object) "763";
      this.dpPresumedFulfillmentDate.ToolTip = "";
      this.dpPresumedFulfillmentDate.Value = new DateTime(0L);
      this.label27.AutoSize = true;
      this.label27.Location = new Point(6, 124);
      this.label27.Name = "label27";
      this.label27.Size = new Size(129, 13);
      this.label27.TabIndex = 69;
      this.label27.Text = "Presumed Received Date";
      this.txtFulfillmentComments.Location = new Point(149, 166);
      this.txtFulfillmentComments.Multiline = true;
      this.txtFulfillmentComments.Name = "txtFulfillmentComments";
      this.txtFulfillmentComments.ReadOnly = true;
      this.txtFulfillmentComments.Size = new Size(258, 76);
      this.txtFulfillmentComments.TabIndex = 72;
      this.label43.AutoSize = true;
      this.label43.Location = new Point(5, 169);
      this.label43.Name = "label43";
      this.label43.Size = new Size(56, 13);
      this.label43.TabIndex = 65;
      this.label43.Text = "Comments";
      this.txtFulfillmentMethod.Location = new Point(149, 76);
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
      this.txtDateFulfillOrder.Location = new Point(149, 54);
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
      this.btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnClose.Location = new Point(841, 7);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new Size(75, 23);
      this.btnClose.TabIndex = 6;
      this.btnClose.Text = "Cancel";
      this.btnClose.UseVisualStyleBackColor = true;
      this.btnClose.Click += new EventHandler(this.btnClose_Click);
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Location = new Point(763, 7);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 5;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.pnlBottom.Controls.Add((Control) this.btnOK);
      this.pnlBottom.Controls.Add((Control) this.btnClose);
      this.pnlBottom.Dock = DockStyle.Bottom;
      this.pnlBottom.Location = new Point(3, 626);
      this.pnlBottom.Name = "pnlBottom";
      this.pnlBottom.Size = new Size(931, 35);
      this.pnlBottom.TabIndex = 7;
      this.txtTrackingNumber.Enabled = false;
      this.txtTrackingNumber.Location = new Point(149, 98);
      this.txtTrackingNumber.Name = "txtTrackingNumber";
      this.txtTrackingNumber.ReadOnly = true;
      this.txtTrackingNumber.Size = new Size(258, 20);
      this.txtTrackingNumber.TabIndex = 66;
      this.label47.AutoSize = true;
      this.label47.Location = new Point(5, 101);
      this.label47.Name = "label47";
      this.label47.Size = new Size(89, 13);
      this.label47.TabIndex = 71;
      this.label47.Text = "Tracking Number";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(934, 661);
      this.Controls.Add((Control) this.tcDisclosure);
      this.Controls.Add((Control) this.pnlBottom);
      this.MinimizeBox = false;
      this.Name = nameof (DisclosureDetailsDialog2015);
      this.Padding = new Padding(3, 3, 0, 0);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Disclosure Details";
      this.FormClosing += new FormClosingEventHandler(this.DisclosureDetailsDialog2015_FormClosing);
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
      this.pnlBasicInfo.PerformLayout();
      this.tableLayoutPanel1.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.pnlIntent.ResumeLayout(false);
      this.pnlIntent.PerformLayout();
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
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
      this.tabPageeDisclosure.PerformLayout();
      this.pnleDisclosureStatus.ResumeLayout(false);
      this.pnleDisclosureStatus.PerformLayout();
      this.gradientPanel2.ResumeLayout(false);
      this.gradientPanel2.PerformLayout();
      this.pnlFulfillment.ResumeLayout(false);
      this.pnlFulfillment.PerformLayout();
      this.gradientPanel4.ResumeLayout(false);
      this.gradientPanel4.PerformLayout();
      this.pnlBottom.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
