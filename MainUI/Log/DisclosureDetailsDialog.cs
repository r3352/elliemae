// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.DisclosureDetailsDialog
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

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
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class DisclosureDetailsDialog : Form
  {
    private const string className = "DisclosureDetailsDialog";
    private static readonly string sw = Tracing.SwInputEngine;
    private const string AutomaticFullfillmentServiceName = "Encompass Fulfillment Service";
    private LoanData loanData;
    private bool suspendEvent;
    private DisclosureTrackingLog currentLog;
    private string[] discloseMethod = new string[5]
    {
      "U.S. Mail",
      "In Person",
      "Fax",
      "Other",
      "eFolder eDisclosures"
    };
    private bool hasAccessRight = true;
    private bool canEditSentDateAndExternalField;
    private bool intermediateData;
    private bool hasManualFulfillmentPermission;
    private bool isFulfillmentServiceEnabled;
    private readonly string timezoneInfoStr;
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
    private Button btnGFESnapshot;
    private Label label20;
    private BorderPanel pnlBasicInfo;
    private FieldLockButton lbtnReceivedDate;
    private FieldLockButton lbtnDisclosedBy;
    private DatePicker dtReceivedDate;
    private FieldLockButton lbtnSentDate;
    private TextBox txtDisclosedMethod;
    private Label lblDisclosureReceivedDate;
    private DateTimePicker dtDisclosedDate;
    private TextBox txtDisclosedComment;
    private Label label18;
    private ComboBox cmbDisclosedMethod;
    private TextBox txtDisclosedBy;
    private Label label17;
    private Label label16;
    private Label label15;
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
    private Label label2;
    private Label lblePackageId;

    public DisclosureDetailsDialog(DisclosureTrackingLog currentLog, bool hasAccessRight)
    {
      this.hasAccessRight = hasAccessRight;
      this.currentLog = currentLog;
      this.InitializeComponent();
      this.applySecurity();
      this.loanData = Session.LoanData;
      string str = this.loanData.GetField("LE1.XG9") == "" ? this.loanData.GetField("LE1.X9") : this.loanData.GetField("LE1.XG9");
      this.timezoneInfoStr = this.currentLog.UseLE1X9ForTimeZone(this.loanData) ? str : "PST";
      this.suspendEvent = true;
      this.initControls();
      this.suspendEvent = false;
      this.refreshDisclosure();
    }

    private void applySecurity()
    {
      FeaturesAclManager aclManager = (FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features);
      this.canEditSentDateAndExternalField = aclManager.GetUserApplicationRight(AclFeature.ToolsTab_DT_ChangeSentDate);
      this.hasManualFulfillmentPermission = aclManager.GetUserApplicationRight(AclFeature.ToolsTab_DT_ManualFulfillment);
      this.isFulfillmentServiceEnabled = Session.ConfigurationManager.GetCompanySetting("Fulfillment", "ServiceEnabled") == "Y";
    }

    private void initControls()
    {
      this.cmbDisclosedMethod.Items.Clear();
      for (int index = 0; index < this.discloseMethod.Length - 1; ++index)
        this.cmbDisclosedMethod.Items.Add((object) this.discloseMethod[index]);
    }

    private void refreshDisclosure()
    {
      this.suspendEvent = true;
      this.cleanHistoryDetail();
      this.tcDisclosure.TabPages.Remove(this.tabPageeDisclosure);
      this.cmbDisclosedMethod.Items.Clear();
      int num = this.discloseMethod.Length - 1;
      for (int index = 0; index < num; ++index)
        this.cmbDisclosedMethod.Items.Add((object) this.discloseMethod[index]);
      if (this.currentLog.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.eDisclosure)
      {
        this.cmbDisclosedMethod.Enabled = false;
        this.lbtnReceivedDate.Locked = this.currentLog.IsDisclosedReceivedDateLocked;
        if (!this.currentLog.IsWetSigned)
        {
          this.dtReceivedDate.ReadOnly = true;
          this.lbtnReceivedDate.Visible = false;
        }
        else
        {
          this.dtReceivedDate.ReadOnly = !this.currentLog.IsDisclosedReceivedDateLocked;
          this.lbtnReceivedDate.Visible = true;
        }
        this.tcDisclosure.TabPages.Add(this.tabPageeDisclosure);
        if (this.currentLog.eDisclosurePackageCreatedDate != DateTime.MinValue)
          this.lblDateeDisclosureSent.Text = this.currentLog.eDisclosurePackageCreatedDate.ToString("MM/dd/yyyy hh:mm tt ") + this.timezoneInfoStr;
        else
          this.lblDateeDisclosureSent.Text = "";
        this.lblePackageId.Text = string.IsNullOrWhiteSpace(this.currentLog.eDisclosurePackageID) ? "" : this.currentLog.eDisclosurePackageID;
        this.addDisclosureGridItem("Name", this.currentLog.eDisclosureBorrowerName, this.currentLog.eDisclosureCoBorrowerName, this.currentLog.eDisclosureLOName);
        this.addDisclosureGridItem("Email Address", this.currentLog.eDisclosureBorrowerEmail, this.currentLog.eDisclosureCoBorrowerEmail, "");
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
        this.RefreshFulfillmentFields();
      }
      if (this.canEditSentDateAndExternalField)
      {
        if (this.currentLog.DisclosureMethod != DisclosureTrackingBase.DisclosedMethod.eDisclosure)
          this.dtReceivedDate.ReadOnly = false;
        this.lbtnSentDate.Enabled = true;
        this.lbtnDisclosedBy.Enabled = true;
        this.lbtnDisclosedAPR.Enabled = true;
        this.lbtnDisclosedDailyInterest.Enabled = true;
        this.lbtnFinanceCharge.Enabled = true;
      }
      if (this.currentLog != null && this.currentLog.eDisclosurePackageViewableFile.Trim() != string.Empty)
        this.btnViewDisclosure.Visible = true;
      else
        this.btnViewDisclosure.Visible = false;
      this.dtDisclosedDate.Value = this.currentLog.DisclosedDate;
      this.lbtnSentDate.Locked = this.currentLog.IsLocked;
      this.dtDisclosedDate.Enabled = this.currentLog.IsLocked;
      this.lbtnDisclosedAPR.Locked = this.currentLog.IsDisclosedAPRLocked;
      this.txtDisclosedAPR.Enabled = this.currentLog.IsDisclosedAPRLocked;
      this.lbtnDisclosedDailyInterest.Locked = this.currentLog.IsDisclosedDailyInterestLocked;
      this.txtDisclosedDailyInterest.Enabled = this.currentLog.IsDisclosedDailyInterestLocked;
      this.lbtnDisclosedBy.Locked = this.currentLog.IsDisclosedByLocked;
      this.txtDisclosedBy.Enabled = this.currentLog.IsDisclosedByLocked;
      this.lbtnFinanceCharge.Locked = this.currentLog.IsDisclosedFinanceChargeLocked;
      this.txtFinanceCharge.Enabled = this.currentLog.IsDisclosedFinanceChargeLocked;
      if (this.currentLog.ReceivedDate == DateTime.MinValue)
        this.dtReceivedDate.Text = "";
      else
        this.dtReceivedDate.Value = this.currentLog.ReceivedDate;
      if (this.currentLog.IsDisclosedByLocked)
        this.txtDisclosedBy.Text = this.currentLog.DisclosedByFullName;
      else
        this.txtDisclosedBy.Text = this.currentLog.DisclosedByFullName + "(" + this.currentLog.DisclosedBy + ")";
      this.txtDisclosedComment.ReadOnly = false;
      this.txtDisclosedComment.Text = this.currentLog.Comments;
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
      this.txtApplicationDate.Text = this.currentLog.ApplicationDate.ToString("MM/dd/yyyy");
      this.cmbDisclosedMethod.Enabled = true;
      if (this.currentLog.DisclosedForGFE)
        this.btnGFESnapshot.Enabled = true;
      else
        this.btnGFESnapshot.Enabled = false;
      this.btnSafeHarborSnapshot.Enabled = this.currentLog.DisclosedForSafeHarbor;
      switch (this.currentLog.DisclosureMethod)
      {
        case DisclosureTrackingBase.DisclosedMethod.ByMail:
          this.cmbDisclosedMethod.SelectedItem = (object) this.discloseMethod[0];
          this.dtReceivedDate.ReadOnly = true;
          break;
        case DisclosureTrackingBase.DisclosedMethod.eDisclosure:
          this.txtDisclosedMethod.Visible = true;
          this.cmbDisclosedMethod.Visible = false;
          this.txtDisclosedMethod.BringToFront();
          break;
        case DisclosureTrackingBase.DisclosedMethod.Fax:
          this.cmbDisclosedMethod.SelectedItem = (object) this.discloseMethod[2];
          this.dtReceivedDate.ReadOnly = true;
          break;
        case DisclosureTrackingBase.DisclosedMethod.InPerson:
          this.cmbDisclosedMethod.SelectedItem = (object) this.discloseMethod[1];
          this.dtReceivedDate.ReadOnly = true;
          break;
        case DisclosureTrackingBase.DisclosedMethod.Other:
          this.cmbDisclosedMethod.SelectedItem = (object) this.discloseMethod[3];
          break;
        default:
          this.cmbDisclosedMethod.SelectedItem = (object) this.discloseMethod[0];
          break;
      }
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
        this.dtReceivedDate.ReadOnly = true;
        this.txtDisclosedComment.ReadOnly = true;
      }
      if (!this.canEditSentDateAndExternalField)
      {
        this.dtReceivedDate.Enabled = false;
        this.lbtnReceivedDate.Enabled = false;
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
      }
      this.suspendEvent = false;
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
        gvItem.SubItems.Add((object) (borrowerInfo.ToString("MM/dd/yyyy hh:mm tt ") + this.timezoneInfoStr));
      else
        gvItem.SubItems.Add((object) "");
      if (coBorrowerInfo != DateTime.MinValue)
        gvItem.SubItems.Add((object) (coBorrowerInfo.ToString("MM/dd/yyyy hh:mm tt ") + this.timezoneInfoStr));
      else
        gvItem.SubItems.Add((object) "");
      if (loanOfficerInfo != DateTime.MinValue)
        gvItem.SubItems.Add((object) (loanOfficerInfo.ToString("MM/dd/yyyy hh:mm tt ") + this.timezoneInfoStr));
      else
        gvItem.SubItems.Add((object) "");
      this.grdDisclosureTracking.Items.Add(gvItem);
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

    private void cleanHistoryDetail()
    {
      this.dtReceivedDate.Text = "";
      this.txtDisclosedBy.Text = "";
      this.txtDisclosedComment.ReadOnly = true;
      this.txtDisclosedComment.Text = "";
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
      this.dtReceivedDate.ReadOnly = true;
      this.btnGFESnapshot.Enabled = false;
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
      this.lbtnReceivedDate.Visible = false;
      this.lbtnReceivedDate.Locked = false;
      this.gcDocList.Text = "Documents Sent (" + (object) this.gvDocList.Items.Count + ")";
    }

    private void RefreshFulfillmentFields()
    {
      if (this.currentLog.eDisclosureManualFulfillmentDate == DateTime.MinValue)
      {
        this.txtFulfillmentOrderBy.Text = this.currentLog.FulfillmentOrderedBy;
        this.txtDateFulfillOrder.Text = this.currentLog.FullfillmentProcessedDate == DateTime.MinValue ? "" : this.currentLog.FullfillmentProcessedDate.ToString("MM/dd/yyyy hh:mm tt ") + this.timezoneInfoStr;
        this.txtFulfillmentMethod.Text = this.currentLog.FullfillmentProcessedDate == DateTime.MinValue ? "" : "Encompass Fulfillment Service";
        this.txtFulfillmentComments.Text = "";
        this.btneDiscManualFulfill.Visible = !this.isFulfillmentServiceEnabled && this.hasManualFulfillmentPermission && !this.currentLog.Received && this.currentLog.FullfillmentProcessedDate == DateTime.MinValue;
      }
      else
      {
        this.txtFulfillmentOrderBy.Text = this.currentLog.eDisclosureManuallyFulfilledBy;
        this.txtDateFulfillOrder.Text = this.currentLog.eDisclosureManualFulfillmentDate == DateTime.MinValue ? "" : this.currentLog.eDisclosureManualFulfillmentDate.ToString("MM/dd/yyyy hh:mm tt ") + this.timezoneInfoStr;
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
        this.txtFulfillmentComments.Text = this.currentLog.eDisclosureManualFulfillmentComment;
        this.btneDiscManualFulfill.Visible = false;
      }
    }

    private void refreshUpdatedItem()
    {
      this.dtReceivedDate.ReadOnly = true;
      switch (this.currentLog.DisclosureMethod)
      {
        case DisclosureTrackingBase.DisclosedMethod.ByMail:
        case DisclosureTrackingBase.DisclosedMethod.Fax:
        case DisclosureTrackingBase.DisclosedMethod.InPerson:
          if (this.currentLog.ReceivedDate == DateTime.MinValue)
          {
            this.dtReceivedDate.Text = "";
            break;
          }
          this.dtReceivedDate.Value = this.currentLog.ReceivedDate;
          break;
        case DisclosureTrackingBase.DisclosedMethod.Other:
          this.dtReceivedDate.ReadOnly = false;
          goto case DisclosureTrackingBase.DisclosedMethod.ByMail;
        default:
          if (this.currentLog.IsDisclosedReceivedDateLocked)
          {
            this.dtReceivedDate.ReadOnly = false;
            goto case DisclosureTrackingBase.DisclosedMethod.ByMail;
          }
          else
            goto case DisclosureTrackingBase.DisclosedMethod.ByMail;
      }
    }

    private void lbtnSentDate_Click(object sender, EventArgs e)
    {
      if (this.currentLog.IsDisclosed && this.loanData.GetLogList().CauseBorrowerIntentToCProceedViolation(this.currentLog.Guid, this.currentLog.IsLocked ? Utils.ParseDate((object) this.currentLog.DisclosureCreatedDTTM.ToString("MM/dd/yyyy  HH:mm:ss")) : this.currentLog.DisclosureCreatedDTTM, true, this.currentLog.ReceivedDate) && Utils.Dialog((IWin32Window) this, "This change might reset borrower intent to proceed field (3164).   Do you want to continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
        return;
      if (this.currentLog.IsLocked)
        this.currentLog.IsLocked = false;
      else
        this.currentLog.IsLocked = true;
      this.lbtnSentDate.Locked = this.currentLog.IsLocked;
      if (!this.currentLog.IsLocked)
      {
        this.dtDisclosedDate.Value = this.currentLog.DisclosedDate;
        if (this.currentLog.ReceivedDate == DateTime.MinValue)
          this.dtReceivedDate.Text = "";
        else
          this.dtReceivedDate.Value = this.currentLog.ReceivedDate;
        this.dtDisclosedDate.Enabled = false;
        this.refreshUpdatedItem();
      }
      else
        this.dtDisclosedDate.Enabled = true;
    }

    private void dtDisclosedDate_Leave(object sender, EventArgs e)
    {
      if (this.suspendEvent || !Utils.IsDate((object) this.dtDisclosedDate.Text) || this.currentLog == null)
        return;
      this.validateDisclosedDate();
      DateTime receivedDate1;
      if (this.currentLog.ReceivedDate != DateTime.MinValue)
      {
        string str1 = this.dtDisclosedDate.Value.ToString("MM/dd/yyyy");
        receivedDate1 = this.currentLog.ReceivedDate;
        string str2 = receivedDate1.ToString("MM/dd/yyyy");
        if (str1 != str2 && this.dtDisclosedDate.Value > this.currentLog.ReceivedDate && this.currentLog.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.Other)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Borrower Received Date cannot be earlier than Sent Date");
        }
      }
      DateTime disclosedDate = this.currentLog.DisclosedDate;
      try
      {
        if (this.currentLog.IsDisclosed)
        {
          LogList logList = this.loanData.GetLogList();
          string guid = this.currentLog.Guid;
          receivedDate1 = this.dtDisclosedDate.Value;
          DateTime newDisclosureSentTime = DateTime.Parse(receivedDate1.ToString("MM/dd/yyyy"));
          DateTime receivedDate2 = this.currentLog.ReceivedDate;
          if (logList.CauseBorrowerIntentToCProceedViolation(guid, newDisclosureSentTime, true, receivedDate2) && Utils.Dialog((IWin32Window) this, "This change might reset borrower intent to proceed field (3164).   Do you want to continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
            return;
        }
        DisclosureTrackingLog currentLog = this.currentLog;
        receivedDate1 = this.dtDisclosedDate.Value;
        DateTime dateTime = DateTime.Parse(receivedDate1.ToString("MM/dd/yyyy"));
        currentLog.DisclosedDate = dateTime;
      }
      catch (Exception ex)
      {
        this.dtDisclosedDate.Value = disclosedDate;
        this.currentLog.DisclosedDate = disclosedDate;
        throw ex;
      }
      if (this.currentLog.ReceivedDate == DateTime.MinValue)
        this.dtReceivedDate.Text = "";
      else
        this.dtReceivedDate.Value = this.currentLog.ReceivedDate;
      this.refreshUpdatedItem();
    }

    private void dtDisclosedDate_CloseUp(object sender, EventArgs e)
    {
      this.dtDisclosedDate_Leave((object) null, (EventArgs) null);
    }

    private void lbtnDisclosedBy_Click(object sender, EventArgs e)
    {
      if (this.currentLog.IsDisclosedByLocked)
        this.currentLog.IsDisclosedByLocked = false;
      else
        this.currentLog.IsDisclosedByLocked = true;
      this.lbtnDisclosedBy.Locked = this.currentLog.IsDisclosedByLocked;
      if (!this.currentLog.IsDisclosedByLocked)
      {
        this.txtDisclosedBy.Text = this.currentLog.DisclosedByFullName + "(" + this.currentLog.DisclosedBy + ")";
        this.txtDisclosedBy.Enabled = false;
        this.refreshUpdatedItem();
      }
      else
        this.txtDisclosedBy.Enabled = true;
      this.txtDisclosedBy_Leave((object) null, (EventArgs) null);
    }

    private void txtDisclosedBy_Leave(object sender, EventArgs e)
    {
      if (this.suspendEvent || this.currentLog == null)
        return;
      if (this.currentLog.IsDisclosedByLocked)
        this.currentLog.DisclosedByFullName = this.txtDisclosedBy.Text;
      this.refreshUpdatedItem();
    }

    private void lbtnReceivedDate_Click(object sender, EventArgs e)
    {
      if (this.currentLog.IsDisclosedReceivedDateLocked)
        this.currentLog.IsDisclosedReceivedDateLocked = false;
      else
        this.currentLog.IsDisclosedReceivedDateLocked = true;
      this.lbtnReceivedDate.Locked = this.currentLog.IsDisclosedReceivedDateLocked;
      if (!this.currentLog.IsDisclosedReceivedDateLocked)
      {
        if (this.currentLog.ReceivedDate == DateTime.MinValue)
          this.dtReceivedDate.Text = "";
        else
          this.dtReceivedDate.Value = this.currentLog.ReceivedDate;
        this.dtReceivedDate.ReadOnly = true;
        this.refreshUpdatedItem();
      }
      else
        this.dtReceivedDate.ReadOnly = false;
    }

    private void dtReceivedDate_ValueChanged(object sender, EventArgs e)
    {
      if (this.suspendEvent || this.currentLog == null)
        return;
      if (this.dtReceivedDate.Text != "" && this.dtReceivedDate.Value.ToString("MM/dd/yyyy") != this.currentLog.DisclosedDate.ToString("MM/dd/yyyy") && this.dtReceivedDate.Value < this.currentLog.DisclosedDate)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Borrower Received Date cannot be earlier than Sent Date");
        this.dtReceivedDate.Text = "";
      }
      DateTime receivedDate = this.currentLog.ReceivedDate;
      try
      {
        if (this.currentLog.IsDisclosed && this.loanData.GetLogList().CauseBorrowerIntentToCProceedViolation(this.currentLog.Guid, this.currentLog.DisclosedDate, true, this.dtReceivedDate.Value) && Utils.Dialog((IWin32Window) this, "This change might reset borrower intent to proceed field (3164).   Do you want to continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
          return;
        this.currentLog.ReceivedDate = this.dtReceivedDate.Value;
      }
      catch (Exception ex)
      {
        this.dtReceivedDate.Value = receivedDate;
        this.currentLog.ReceivedDate = receivedDate;
        throw ex;
      }
      this.refreshUpdatedItem();
    }

    private void txtDisclosedComment_Leave(object sender, EventArgs e)
    {
      if (this.suspendEvent || this.currentLog == null)
        return;
      this.currentLog.Comments = this.txtDisclosedComment.Text.Trim();
      this.refreshUpdatedItem();
    }

    private void lblSnapshot_Click(object sender, EventArgs e)
    {
      int num = (int) new DisclosedHUDGFEDialog(this.currentLog).ShowDialog((IWin32Window) this);
    }

    private void btnSafeHarborSnapshot_Click(object sender, EventArgs e)
    {
      int num = (int) new DisclosedSafeHarborDialog((IDisclosureTrackingLog) this.currentLog).ShowDialog((IWin32Window) this);
    }

    private void lbtnDisclosedAPR_Click(object sender, EventArgs e)
    {
      this.currentLog.IsDisclosedAPRLocked = !this.currentLog.IsDisclosedAPRLocked;
      this.lbtnDisclosedAPR.Locked = this.currentLog.IsDisclosedAPRLocked;
      if (!this.currentLog.IsDisclosedAPRLocked)
      {
        this.txtDisclosedAPR.Text = this.currentLog.DisclosedAPR;
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
        if (needsUpdate)
        {
          this.intermediateData = true;
          this.txtDisclosedAPR.Text = str;
          this.txtDisclosedAPR.SelectionStart = str.Length;
          this.intermediateData = false;
        }
        this.currentLog.DisclosedAPR = this.txtDisclosedAPR.Text;
      }
    }

    private void lbtnFinanceCharge_Click(object sender, EventArgs e)
    {
      this.currentLog.IsDisclosedFinanceChargeLocked = !this.currentLog.IsDisclosedFinanceChargeLocked;
      this.lbtnFinanceCharge.Locked = this.currentLog.IsDisclosedFinanceChargeLocked;
      if (!this.currentLog.IsDisclosedFinanceChargeLocked)
      {
        this.txtFinanceCharge.Text = this.currentLog.FinanceCharge;
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
      if (needsUpdate)
      {
        this.intermediateData = true;
        this.txtFinanceCharge.Text = str;
        this.txtFinanceCharge.SelectionStart = str.Length;
        this.intermediateData = false;
      }
      this.currentLog.FinanceCharge = this.txtFinanceCharge.Text;
    }

    private void btnViewDisclosure_Click(object sender, EventArgs e)
    {
      Session.Application.GetService<IEFolder>().ViewDisclosures(Session.LoanDataMgr, this.currentLog.eDisclosurePackageViewableFile);
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
      try
      {
        BinaryObject binaryObject = (BinaryObject) null;
        if (Session.LoanDataMgr.IsPlatformLoan())
        {
          if (!string.IsNullOrEmpty(this.currentLog.eDisclosurePackageID))
            binaryObject = new EDeliveryRestClient(Session.LoanDataMgr).GetPackageConsentPdf(this.currentLog.eDisclosurePackageID).Result;
        }
        else if (!string.IsNullOrEmpty(this.currentLog.eDisclosureConsentPDF))
          binaryObject = Session.LoanDataMgr.GetSupportingData(this.currentLog.eDisclosureConsentPDF);
        if (binaryObject == null)
          throw new FileNotFoundException();
        string str = path + "eDisclosure_" + this.currentLog.Guid + ".pdf";
        binaryObject.Write(str);
        int num = (int) new PdfPreviewDialog(str, true, true, false).ShowDialog((IWin32Window) this);
      }
      catch (Exception ex)
      {
        Tracing.Log(DisclosureDetailsDialog.sw, TraceLevel.Error, nameof (DisclosureDetailsDialog), "View Consent Form. Error: " + (object) ex);
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
      if (this.currentLog.eDisclosurePackageViewableFile == string.Empty)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "There are no viewable documents associated with this disclosure tracking entry.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        Session.Application.GetService<IEFolder>().ViewDisclosures(Session.LoanDataMgr, this.currentLog.eDisclosurePackageViewableFile);
        if (new ManualFulfillmentDialog((IDisclosureTrackingLog) this.currentLog).ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.currentLog.ReceivedDate = this.loanData.Calculator.CalculateNewDisclosureReceivedDate(this.currentLog.eDisclosureManualFulfillmentMethod, this.currentLog.eDisclosureManualFulfillmentDate, this.currentLog.ReceivedDate);
        this.refreshDisclosure();
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
      int num = (int) Utils.Dialog((IWin32Window) this, "U.S. Postal Service does not deliver disclosures on Sundays and legal holidays.");
      this.dtDisclosedDate.Value = closestBusinessDay;
      return false;
    }

    private void btnClose_Click(object sender, EventArgs e) => this.Close();

    private void cmbDisclosedMethod_Leave(object sender, EventArgs e)
    {
      if (this.suspendEvent || this.currentLog == null || this.currentLog.IsDisclosed && this.loanData.GetLogList().CauseBorrowerIntentToCProceedViolation(this.currentLog.Guid, this.currentLog.DisclosedDate, true, this.currentLog.ReceivedDate) && Utils.Dialog((IWin32Window) this, "This change might reset borrower intent to proceed field (3164).   Do you want to continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
        return;
      if (string.Concat(this.cmbDisclosedMethod.SelectedItem) == this.discloseMethod[0])
        this.currentLog.DisclosureMethod = DisclosureTrackingBase.DisclosedMethod.ByMail;
      else if (string.Concat(this.cmbDisclosedMethod.SelectedItem) == this.discloseMethod[1])
        this.currentLog.DisclosureMethod = DisclosureTrackingBase.DisclosedMethod.InPerson;
      else if (string.Concat(this.cmbDisclosedMethod.SelectedItem) == this.discloseMethod[3])
      {
        this.currentLog.DisclosureMethod = DisclosureTrackingBase.DisclosedMethod.Other;
        this.currentLog.ReceivedDate = DateTime.MinValue;
      }
      else if (string.Concat(this.cmbDisclosedMethod.SelectedItem) == this.discloseMethod[2])
      {
        this.currentLog.DisclosureMethod = DisclosureTrackingBase.DisclosedMethod.Fax;
      }
      else
      {
        this.currentLog.DisclosureMethod = DisclosureTrackingBase.DisclosedMethod.eDisclosure;
        this.currentLog.ReceivedDate = DateTime.MinValue;
      }
      if (this.currentLog.IsLocked && !this.validateDisclosedDate())
        this.currentLog.DisclosedDate = this.dtDisclosedDate.Value;
      this.refreshUpdatedItem();
    }

    private void cmbDisclosedMethod_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.suspendEvent || this.currentLog == null)
        return;
      DisclosureTrackingBase.DisclosedMethod method = !(string.Concat(this.cmbDisclosedMethod.SelectedItem) == this.discloseMethod[0]) ? (!(string.Concat(this.cmbDisclosedMethod.SelectedItem) == this.discloseMethod[1]) ? (!(string.Concat(this.cmbDisclosedMethod.SelectedItem) == this.discloseMethod[3]) ? (!(string.Concat(this.cmbDisclosedMethod.SelectedItem) == this.discloseMethod[2]) ? DisclosureTrackingBase.DisclosedMethod.eDisclosure : DisclosureTrackingBase.DisclosedMethod.Fax) : DisclosureTrackingBase.DisclosedMethod.Other) : DisclosureTrackingBase.DisclosedMethod.InPerson) : DisclosureTrackingBase.DisclosedMethod.ByMail;
      if (this.currentLog.IsDisclosed && this.loanData.GetLogList().CauseBorrowerIntentToCProceedViolation(this.currentLog.Guid, this.currentLog.DisclosedDate, true, this.loanData.Calculator.CalculateNewDisclosureReceivedDate(method, this.currentLog.DisclosedDate, method == DisclosureTrackingBase.DisclosedMethod.Other ? DateTime.MinValue : this.currentLog.ReceivedDate)) && Utils.Dialog((IWin32Window) this, "This change might reset borrower intent to proceed field (3164).   Do you want to continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
      {
        this.suspendEvent = true;
        switch (this.currentLog.DisclosureMethod)
        {
          case DisclosureTrackingBase.DisclosedMethod.ByMail:
            this.cmbDisclosedMethod.SelectedItem = (object) this.discloseMethod[0];
            break;
          case DisclosureTrackingBase.DisclosedMethod.Fax:
            this.cmbDisclosedMethod.SelectedItem = (object) this.discloseMethod[2];
            break;
          case DisclosureTrackingBase.DisclosedMethod.InPerson:
            this.cmbDisclosedMethod.SelectedItem = (object) this.discloseMethod[1];
            break;
          case DisclosureTrackingBase.DisclosedMethod.Other:
            this.cmbDisclosedMethod.SelectedItem = (object) this.discloseMethod[3];
            break;
        }
        this.suspendEvent = false;
      }
      else
      {
        this.currentLog.DisclosureMethod = method;
        switch (method)
        {
          case DisclosureTrackingBase.DisclosedMethod.eDisclosure:
            this.currentLog.ReceivedDate = DateTime.MinValue;
            break;
          case DisclosureTrackingBase.DisclosedMethod.Other:
            this.currentLog.ReceivedDate = DateTime.MinValue;
            break;
        }
        if (this.currentLog.IsLocked && !this.validateDisclosedDate())
          this.currentLog.DisclosedDate = this.dtDisclosedDate.Value;
        this.dtReceivedDate.Focus();
        this.refreshUpdatedItem();
      }
    }

    private void txtDisclosedBy_TextChanged(object sender, EventArgs e)
    {
      if (this.suspendEvent || this.currentLog == null)
        return;
      if (this.currentLog.IsDisclosedByLocked)
        this.currentLog.DisclosedByFullName = this.txtDisclosedBy.Text;
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
        if (needsUpdate)
        {
          this.intermediateData = true;
          this.txtDisclosedDailyInterest.Text = str;
          this.txtDisclosedDailyInterest.SelectionStart = str.Length;
          this.intermediateData = false;
        }
        this.currentLog.DisclosedDailyInterest = this.txtDisclosedDailyInterest.Text;
      }
    }

    private void lbtnDisclosedDailyInterest_Click(object sender, EventArgs e)
    {
      this.currentLog.IsDisclosedDailyInterestLocked = !this.currentLog.IsDisclosedDailyInterestLocked;
      this.lbtnDisclosedDailyInterest.Locked = this.currentLog.IsDisclosedDailyInterestLocked;
      if (!this.currentLog.IsDisclosedDailyInterestLocked)
      {
        this.txtDisclosedDailyInterest.Text = this.currentLog.DisclosedDailyInterest;
        this.txtDisclosedDailyInterest.Enabled = false;
      }
      else
        this.txtDisclosedDailyInterest.Enabled = true;
      this.txtDisclosedDailyInterest_Leave((object) null, (EventArgs) null);
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
      this.tcDisclosure = new TabControl();
      this.tabPageDetail = new TabPage();
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
      this.btnSafeHarborSnapshot = new Button();
      this.btnGFESnapshot = new Button();
      this.label20 = new Label();
      this.pnlBasicInfo = new BorderPanel();
      this.lbtnReceivedDate = new FieldLockButton();
      this.lbtnDisclosedBy = new FieldLockButton();
      this.dtReceivedDate = new DatePicker();
      this.lbtnSentDate = new FieldLockButton();
      this.txtDisclosedMethod = new TextBox();
      this.lblDisclosureReceivedDate = new Label();
      this.dtDisclosedDate = new DateTimePicker();
      this.txtDisclosedComment = new TextBox();
      this.label18 = new Label();
      this.cmbDisclosedMethod = new ComboBox();
      this.txtDisclosedBy = new TextBox();
      this.label17 = new Label();
      this.label16 = new Label();
      this.label15 = new Label();
      this.gradientPanel1 = new GradientPanel();
      this.lblDisclosureInfo = new Label();
      this.tabPageeDisclosure = new TabPage();
      this.pnlFulfillment = new BorderPanel();
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
      this.pnleDisclosureStatus = new BorderPanel();
      this.lblePackageId = new Label();
      this.label2 = new Label();
      this.lblDateeDisclosureSent = new Label();
      this.grdDisclosureTracking = new GridView();
      this.lblViewForm = new Label();
      this.llViewDetails = new Label();
      this.label19 = new Label();
      this.gradientPanel2 = new GradientPanel();
      this.label14 = new Label();
      this.btnClose = new Button();
      this.tcDisclosure.SuspendLayout();
      this.tabPageDetail.SuspendLayout();
      this.pnlLoanSnapshot.SuspendLayout();
      this.gcDocList.SuspendLayout();
      this.gradientPanel3.SuspendLayout();
      this.flowLayoutPanel2.SuspendLayout();
      this.pnlBasicInfo.SuspendLayout();
      this.gradientPanel1.SuspendLayout();
      this.tabPageeDisclosure.SuspendLayout();
      this.pnlFulfillment.SuspendLayout();
      this.gradientPanel4.SuspendLayout();
      this.pnleDisclosureStatus.SuspendLayout();
      this.gradientPanel2.SuspendLayout();
      this.SuspendLayout();
      this.tcDisclosure.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.tcDisclosure.Controls.Add((Control) this.tabPageDetail);
      this.tcDisclosure.Controls.Add((Control) this.tabPageeDisclosure);
      this.tcDisclosure.Location = new Point(5, 5);
      this.tcDisclosure.Name = "tcDisclosure";
      this.tcDisclosure.SelectedIndex = 0;
      this.tcDisclosure.Size = new Size(783, 592);
      this.tcDisclosure.TabIndex = 4;
      this.tabPageDetail.Controls.Add((Control) this.pnlLoanSnapshot);
      this.tabPageDetail.Controls.Add((Control) this.pnlBasicInfo);
      this.tabPageDetail.Location = new Point(4, 22);
      this.tabPageDetail.Name = "tabPageDetail";
      this.tabPageDetail.Padding = new Padding(1, 3, 3, 3);
      this.tabPageDetail.Size = new Size(775, 566);
      this.tabPageDetail.TabIndex = 0;
      this.tabPageDetail.Text = "Details";
      this.tabPageDetail.UseVisualStyleBackColor = true;
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
      this.pnlLoanSnapshot.Dock = DockStyle.Top;
      this.pnlLoanSnapshot.Location = new Point(1, 137);
      this.pnlLoanSnapshot.Name = "pnlLoanSnapshot";
      this.pnlLoanSnapshot.Size = new Size(771, 393);
      this.pnlLoanSnapshot.TabIndex = 2;
      this.lbtnDisclosedDailyInterest.Location = new Point(455, 54);
      this.lbtnDisclosedDailyInterest.LockedStateToolTip = "Use Default Value";
      this.lbtnDisclosedDailyInterest.MaximumSize = new Size(16, 17);
      this.lbtnDisclosedDailyInterest.MinimumSize = new Size(16, 17);
      this.lbtnDisclosedDailyInterest.Name = "lbtnDisclosedDailyInterest";
      this.lbtnDisclosedDailyInterest.Size = new Size(16, 17);
      this.lbtnDisclosedDailyInterest.TabIndex = 48;
      this.lbtnDisclosedDailyInterest.UnlockedStateToolTip = "Enter Data Manually";
      this.lbtnDisclosedDailyInterest.Click += new EventHandler(this.lbtnDisclosedDailyInterest_Click);
      this.txtDisclosedDailyInterest.Location = new Point(477, 54);
      this.txtDisclosedDailyInterest.Name = "txtDisclosedDailyInterest";
      this.txtDisclosedDailyInterest.Size = new Size(184, 20);
      this.txtDisclosedDailyInterest.TabIndex = 19;
      this.txtDisclosedDailyInterest.Leave += new EventHandler(this.txtDisclosedDailyInterest_Leave);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(335, 57);
      this.label1.Name = "label1";
      this.label1.Size = new Size(117, 13);
      this.label1.TabIndex = 46;
      this.label1.Text = "Disclosed Daily Interest";
      this.lbtnFinanceCharge.Location = new Point(455, 120);
      this.lbtnFinanceCharge.LockedStateToolTip = "Use Default Value";
      this.lbtnFinanceCharge.MaximumSize = new Size(16, 17);
      this.lbtnFinanceCharge.MinimumSize = new Size(16, 17);
      this.lbtnFinanceCharge.Name = "lbtnFinanceCharge";
      this.lbtnFinanceCharge.Size = new Size(16, 17);
      this.lbtnFinanceCharge.TabIndex = 45;
      this.lbtnFinanceCharge.UnlockedStateToolTip = "Enter Data Manually";
      this.lbtnFinanceCharge.Click += new EventHandler(this.lbtnFinanceCharge_Click);
      this.lbtnDisclosedAPR.Location = new Point(455, 32);
      this.lbtnDisclosedAPR.LockedStateToolTip = "Use Default Value";
      this.lbtnDisclosedAPR.MaximumSize = new Size(16, 17);
      this.lbtnDisclosedAPR.MinimumSize = new Size(16, 17);
      this.lbtnDisclosedAPR.Name = "lbtnDisclosedAPR";
      this.lbtnDisclosedAPR.Size = new Size(16, 17);
      this.lbtnDisclosedAPR.TabIndex = 44;
      this.lbtnDisclosedAPR.UnlockedStateToolTip = "Enter Data Manually";
      this.lbtnDisclosedAPR.Click += new EventHandler(this.lbtnDisclosedAPR_Click);
      this.gcDocList.Controls.Add((Control) this.btnViewDisclosure);
      this.gcDocList.Controls.Add((Control) this.gvDocList);
      this.gcDocList.HeaderForeColor = SystemColors.ControlText;
      this.gcDocList.Location = new Point(7, 168);
      this.gcDocList.Name = "gcDocList";
      this.gcDocList.Size = new Size(654, 221);
      this.gcDocList.TabIndex = 25;
      this.gcDocList.Text = "Document Sent";
      this.btnViewDisclosure.Location = new Point(545, 2);
      this.btnViewDisclosure.Name = "btnViewDisclosure";
      this.btnViewDisclosure.Size = new Size(96, 22);
      this.btnViewDisclosure.TabIndex = 25;
      this.btnViewDisclosure.Text = "View Document";
      this.btnViewDisclosure.UseVisualStyleBackColor = true;
      this.btnViewDisclosure.Click += new EventHandler(this.btnViewDisclosure_Click);
      this.gvDocList.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Name";
      gvColumn1.Width = 480;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Type";
      gvColumn2.Width = 120;
      this.gvDocList.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.gvDocList.Dock = DockStyle.Fill;
      this.gvDocList.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvDocList.Location = new Point(1, 26);
      this.gvDocList.Name = "gvDocList";
      this.gvDocList.Size = new Size(652, 194);
      this.gvDocList.TabIndex = 24;
      this.txtApplicationDate.Location = new Point(477, 142);
      this.txtApplicationDate.Name = "txtApplicationDate";
      this.txtApplicationDate.ReadOnly = true;
      this.txtApplicationDate.Size = new Size(184, 20);
      this.txtApplicationDate.TabIndex = 23;
      this.label42.AutoSize = true;
      this.label42.Location = new Point(335, 145);
      this.label42.Name = "label42";
      this.label42.Size = new Size(85, 13);
      this.label42.TabIndex = 22;
      this.label42.Text = "Application Date";
      this.txtFinanceCharge.Location = new Point(477, 120);
      this.txtFinanceCharge.Name = "txtFinanceCharge";
      this.txtFinanceCharge.Size = new Size(184, 20);
      this.txtFinanceCharge.TabIndex = 20;
      this.txtFinanceCharge.Leave += new EventHandler(this.txtFinanceCharge_Leave);
      this.txtLoanProgram.Location = new Point(477, 76);
      this.txtLoanProgram.Name = "txtLoanProgram";
      this.txtLoanProgram.ReadOnly = true;
      this.txtLoanProgram.Size = new Size(184, 20);
      this.txtLoanProgram.TabIndex = 20;
      this.txtLoanAmount.Location = new Point(477, 98);
      this.txtLoanAmount.Name = "txtLoanAmount";
      this.txtLoanAmount.ReadOnly = true;
      this.txtLoanAmount.Size = new Size(184, 20);
      this.txtLoanAmount.TabIndex = 19;
      this.txtDisclosedAPR.Location = new Point(477, 32);
      this.txtDisclosedAPR.Name = "txtDisclosedAPR";
      this.txtDisclosedAPR.Size = new Size(184, 20);
      this.txtDisclosedAPR.TabIndex = 18;
      this.txtDisclosedAPR.Leave += new EventHandler(this.txtDisclosedAPR_Leave);
      this.label38.AutoSize = true;
      this.label38.Location = new Point(335, 101);
      this.label38.Name = "label38";
      this.label38.Size = new Size(70, 13);
      this.label38.TabIndex = 17;
      this.label38.Text = "Loan Amount";
      this.label39.AutoSize = true;
      this.label39.Location = new Point(335, 123);
      this.label39.Name = "label39";
      this.label39.Size = new Size(82, 13);
      this.label39.TabIndex = 16;
      this.label39.Text = "Finance Charge";
      this.label40.AutoSize = true;
      this.label40.Location = new Point(335, 79);
      this.label40.Name = "label40";
      this.label40.Size = new Size(73, 13);
      this.label40.TabIndex = 15;
      this.label40.Text = "Loan Program";
      this.label41.AutoSize = true;
      this.label41.Location = new Point(335, 35);
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
      this.txtCoBorrowerName.TabIndex = 10;
      this.txtPropertyAddress.Location = new Point(142, 76);
      this.txtPropertyAddress.Name = "txtPropertyAddress";
      this.txtPropertyAddress.ReadOnly = true;
      this.txtPropertyAddress.Size = new Size(183, 20);
      this.txtPropertyAddress.TabIndex = 9;
      this.txtBorrowerName.Location = new Point(142, 32);
      this.txtBorrowerName.Name = "txtBorrowerName";
      this.txtBorrowerName.ReadOnly = true;
      this.txtBorrowerName.Size = new Size(183, 20);
      this.txtBorrowerName.TabIndex = 8;
      this.label37.AutoSize = true;
      this.label37.Location = new Point(179, 123);
      this.label37.Name = "label37";
      this.label37.Size = new Size(22, 13);
      this.label37.TabIndex = 7;
      this.label37.Text = "Zip";
      this.label36.AutoSize = true;
      this.label36.Location = new Point(5, 79);
      this.label36.Name = "label36";
      this.label36.Size = new Size(87, 13);
      this.label36.TabIndex = 6;
      this.label36.Text = "Property Address";
      this.label35.AutoSize = true;
      this.label35.Location = new Point(5, 123);
      this.label35.Name = "label35";
      this.label35.Size = new Size(32, 13);
      this.label35.TabIndex = 5;
      this.label35.Text = "State";
      this.label34.AutoSize = true;
      this.label34.Location = new Point(5, 101);
      this.label34.Name = "label34";
      this.label34.Size = new Size(24, 13);
      this.label34.TabIndex = 4;
      this.label34.Text = "City";
      this.label33.AutoSize = true;
      this.label33.Location = new Point(5, 57);
      this.label33.Name = "label33";
      this.label33.Size = new Size(96, 13);
      this.label33.TabIndex = 3;
      this.label33.Text = "Co-Borrower Name";
      this.label32.AutoSize = true;
      this.label32.Location = new Point(5, 35);
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
      this.gradientPanel3.Location = new Point(1, 1);
      this.gradientPanel3.Name = "gradientPanel3";
      this.gradientPanel3.Size = new Size(769, 25);
      this.gradientPanel3.Style = GradientPanel.PanelStyle.TableFooter;
      this.gradientPanel3.TabIndex = 1;
      this.flowLayoutPanel2.Controls.Add((Control) this.btnSafeHarborSnapshot);
      this.flowLayoutPanel2.Controls.Add((Control) this.btnGFESnapshot);
      this.flowLayoutPanel2.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel2.Location = new Point(413, 0);
      this.flowLayoutPanel2.Name = "flowLayoutPanel2";
      this.flowLayoutPanel2.Padding = new Padding(0, 1, 0, 0);
      this.flowLayoutPanel2.Size = new Size(280, 26);
      this.flowLayoutPanel2.TabIndex = 9;
      this.btnSafeHarborSnapshot.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnSafeHarborSnapshot.Enabled = false;
      this.btnSafeHarborSnapshot.Location = new Point(148, 1);
      this.btnSafeHarborSnapshot.Margin = new Padding(0);
      this.btnSafeHarborSnapshot.Name = "btnSafeHarborSnapshot";
      this.btnSafeHarborSnapshot.Size = new Size(132, 22);
      this.btnSafeHarborSnapshot.TabIndex = 9;
      this.btnSafeHarborSnapshot.Text = "Safe Harbor Snapshot";
      this.btnSafeHarborSnapshot.UseVisualStyleBackColor = true;
      this.btnSafeHarborSnapshot.Click += new EventHandler(this.btnSafeHarborSnapshot_Click);
      this.btnGFESnapshot.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnGFESnapshot.Location = new Point(58, 1);
      this.btnGFESnapshot.Margin = new Padding(0);
      this.btnGFESnapshot.Name = "btnGFESnapshot";
      this.btnGFESnapshot.Size = new Size(90, 22);
      this.btnGFESnapshot.TabIndex = 8;
      this.btnGFESnapshot.Text = "GFE Snapshot";
      this.btnGFESnapshot.UseVisualStyleBackColor = true;
      this.btnGFESnapshot.Click += new EventHandler(this.lblSnapshot_Click);
      this.label20.AutoSize = true;
      this.label20.BackColor = Color.Transparent;
      this.label20.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label20.Location = new Point(5, 5);
      this.label20.Name = "label20";
      this.label20.Size = new Size(89, 14);
      this.label20.TabIndex = 2;
      this.label20.Text = "Loan Snapshot";
      this.pnlBasicInfo.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlBasicInfo.Controls.Add((Control) this.lbtnReceivedDate);
      this.pnlBasicInfo.Controls.Add((Control) this.lbtnDisclosedBy);
      this.pnlBasicInfo.Controls.Add((Control) this.dtReceivedDate);
      this.pnlBasicInfo.Controls.Add((Control) this.lbtnSentDate);
      this.pnlBasicInfo.Controls.Add((Control) this.txtDisclosedMethod);
      this.pnlBasicInfo.Controls.Add((Control) this.lblDisclosureReceivedDate);
      this.pnlBasicInfo.Controls.Add((Control) this.dtDisclosedDate);
      this.pnlBasicInfo.Controls.Add((Control) this.txtDisclosedComment);
      this.pnlBasicInfo.Controls.Add((Control) this.label18);
      this.pnlBasicInfo.Controls.Add((Control) this.cmbDisclosedMethod);
      this.pnlBasicInfo.Controls.Add((Control) this.txtDisclosedBy);
      this.pnlBasicInfo.Controls.Add((Control) this.label17);
      this.pnlBasicInfo.Controls.Add((Control) this.label16);
      this.pnlBasicInfo.Controls.Add((Control) this.label15);
      this.pnlBasicInfo.Controls.Add((Control) this.gradientPanel1);
      this.pnlBasicInfo.Dock = DockStyle.Top;
      this.pnlBasicInfo.Location = new Point(1, 3);
      this.pnlBasicInfo.Name = "pnlBasicInfo";
      this.pnlBasicInfo.Size = new Size(771, 134);
      this.pnlBasicInfo.TabIndex = 0;
      this.lbtnReceivedDate.Location = new Point(138, 103);
      this.lbtnReceivedDate.MaximumSize = new Size(16, 16);
      this.lbtnReceivedDate.MinimumSize = new Size(16, 16);
      this.lbtnReceivedDate.Name = "lbtnReceivedDate";
      this.lbtnReceivedDate.Size = new Size(16, 16);
      this.lbtnReceivedDate.TabIndex = 44;
      this.lbtnReceivedDate.Click += new EventHandler(this.lbtnReceivedDate_Click);
      this.lbtnDisclosedBy.Location = new Point(138, 57);
      this.lbtnDisclosedBy.LockedStateToolTip = "Use Default Value";
      this.lbtnDisclosedBy.MaximumSize = new Size(16, 17);
      this.lbtnDisclosedBy.MinimumSize = new Size(16, 17);
      this.lbtnDisclosedBy.Name = "lbtnDisclosedBy";
      this.lbtnDisclosedBy.Size = new Size(16, 17);
      this.lbtnDisclosedBy.TabIndex = 43;
      this.lbtnDisclosedBy.UnlockedStateToolTip = "Enter Data Manually";
      this.lbtnDisclosedBy.Click += new EventHandler(this.lbtnDisclosedBy_Click);
      this.dtReceivedDate.BackColor = SystemColors.Window;
      this.dtReceivedDate.Location = new Point(157, 101);
      this.dtReceivedDate.MaxValue = new DateTime(2029, 12, 31, 0, 0, 0, 0);
      this.dtReceivedDate.MinValue = new DateTime(2000, 1, 1, 0, 0, 0, 0);
      this.dtReceivedDate.Name = "dtReceivedDate";
      this.dtReceivedDate.Size = new Size(183, 21);
      this.dtReceivedDate.TabIndex = 42;
      this.dtReceivedDate.Tag = (object) "763";
      this.dtReceivedDate.ToolTip = "";
      this.dtReceivedDate.Value = new DateTime(0L);
      this.dtReceivedDate.ValueChanged += new EventHandler(this.dtReceivedDate_ValueChanged);
      this.lbtnSentDate.Location = new Point(138, 36);
      this.lbtnSentDate.LockedStateToolTip = "Use Default Value";
      this.lbtnSentDate.MaximumSize = new Size(16, 17);
      this.lbtnSentDate.MinimumSize = new Size(16, 17);
      this.lbtnSentDate.Name = "lbtnSentDate";
      this.lbtnSentDate.Size = new Size(16, 17);
      this.lbtnSentDate.TabIndex = 15;
      this.lbtnSentDate.UnlockedStateToolTip = "Enter Data Manually";
      this.lbtnSentDate.Click += new EventHandler(this.lbtnSentDate_Click);
      this.txtDisclosedMethod.Location = new Point(157, 77);
      this.txtDisclosedMethod.Name = "txtDisclosedMethod";
      this.txtDisclosedMethod.ReadOnly = true;
      this.txtDisclosedMethod.Size = new Size(183, 20);
      this.txtDisclosedMethod.TabIndex = 13;
      this.txtDisclosedMethod.Text = "eFolder eDisclosures";
      this.txtDisclosedMethod.Visible = false;
      this.lblDisclosureReceivedDate.AutoSize = true;
      this.lblDisclosureReceivedDate.Location = new Point(5, 105);
      this.lblDisclosureReceivedDate.Name = "lblDisclosureReceivedDate";
      this.lblDisclosureReceivedDate.Size = new Size(124, 13);
      this.lblDisclosureReceivedDate.TabIndex = 12;
      this.lblDisclosureReceivedDate.Text = "Borrower Received Date";
      this.dtDisclosedDate.CalendarMonthBackground = Color.WhiteSmoke;
      this.dtDisclosedDate.CalendarTitleBackColor = Color.FromArgb(29, 110, 174);
      this.dtDisclosedDate.CustomFormat = "MM/dd/yyyy";
      this.dtDisclosedDate.Format = DateTimePickerFormat.Short;
      this.dtDisclosedDate.Location = new Point(157, 33);
      this.dtDisclosedDate.MaxDate = new DateTime(2029, 12, 31, 0, 0, 0, 0);
      this.dtDisclosedDate.MinDate = new DateTime(2000, 1, 1, 0, 0, 0, 0);
      this.dtDisclosedDate.Name = "dtDisclosedDate";
      this.dtDisclosedDate.Size = new Size(183, 20);
      this.dtDisclosedDate.TabIndex = 10;
      this.dtDisclosedDate.CloseUp += new EventHandler(this.dtDisclosedDate_CloseUp);
      this.dtDisclosedDate.Leave += new EventHandler(this.dtDisclosedDate_Leave);
      this.txtDisclosedComment.Location = new Point(350, 55);
      this.txtDisclosedComment.Multiline = true;
      this.txtDisclosedComment.Name = "txtDisclosedComment";
      this.txtDisclosedComment.ScrollBars = ScrollBars.Both;
      this.txtDisclosedComment.Size = new Size(318, 68);
      this.txtDisclosedComment.TabIndex = 8;
      this.txtDisclosedComment.Leave += new EventHandler(this.txtDisclosedComment_Leave);
      this.label18.AutoSize = true;
      this.label18.Location = new Point(347, 39);
      this.label18.Name = "label18";
      this.label18.Size = new Size(56, 13);
      this.label18.TabIndex = 7;
      this.label18.Text = "Comments";
      this.cmbDisclosedMethod.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbDisclosedMethod.FormattingEnabled = true;
      this.cmbDisclosedMethod.Location = new Point(157, 77);
      this.cmbDisclosedMethod.Name = "cmbDisclosedMethod";
      this.cmbDisclosedMethod.Size = new Size(183, 21);
      this.cmbDisclosedMethod.TabIndex = 6;
      this.cmbDisclosedMethod.SelectedIndexChanged += new EventHandler(this.cmbDisclosedMethod_SelectedIndexChanged);
      this.cmbDisclosedMethod.Leave += new EventHandler(this.cmbDisclosedMethod_Leave);
      this.txtDisclosedBy.Location = new Point(157, 55);
      this.txtDisclosedBy.Name = "txtDisclosedBy";
      this.txtDisclosedBy.Size = new Size(183, 20);
      this.txtDisclosedBy.TabIndex = 5;
      this.txtDisclosedBy.TextChanged += new EventHandler(this.txtDisclosedBy_TextChanged);
      this.txtDisclosedBy.Leave += new EventHandler(this.txtDisclosedBy_Leave);
      this.label17.AutoSize = true;
      this.label17.Location = new Point(5, 58);
      this.label17.Name = "label17";
      this.label17.Size = new Size(19, 13);
      this.label17.TabIndex = 3;
      this.label17.Text = "By";
      this.label16.AutoSize = true;
      this.label16.Location = new Point(5, 81);
      this.label16.Name = "label16";
      this.label16.Size = new Size(43, 13);
      this.label16.TabIndex = 2;
      this.label16.Text = "Method";
      this.label15.AutoSize = true;
      this.label15.Location = new Point(5, 37);
      this.label15.Name = "label15";
      this.label15.Size = new Size(55, 13);
      this.label15.TabIndex = 1;
      this.label15.Text = "Sent Date";
      this.gradientPanel1.Borders = AnchorStyles.Bottom;
      this.gradientPanel1.Controls.Add((Control) this.lblDisclosureInfo);
      this.gradientPanel1.Dock = DockStyle.Top;
      this.gradientPanel1.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel1.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel1.Location = new Point(1, 1);
      this.gradientPanel1.Name = "gradientPanel1";
      this.gradientPanel1.Size = new Size(769, 25);
      this.gradientPanel1.Style = GradientPanel.PanelStyle.TableFooter;
      this.gradientPanel1.TabIndex = 0;
      this.lblDisclosureInfo.AutoSize = true;
      this.lblDisclosureInfo.BackColor = Color.Transparent;
      this.lblDisclosureInfo.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblDisclosureInfo.Location = new Point(5, 5);
      this.lblDisclosureInfo.Name = "lblDisclosureInfo";
      this.lblDisclosureInfo.Size = new Size(133, 14);
      this.lblDisclosureInfo.TabIndex = 1;
      this.lblDisclosureInfo.Text = "Disclosure Information";
      this.tabPageeDisclosure.AutoScroll = true;
      this.tabPageeDisclosure.Controls.Add((Control) this.pnlFulfillment);
      this.tabPageeDisclosure.Controls.Add((Control) this.pnleDisclosureStatus);
      this.tabPageeDisclosure.Location = new Point(4, 22);
      this.tabPageeDisclosure.Margin = new Padding(1, 3, 3, 3);
      this.tabPageeDisclosure.Name = "tabPageeDisclosure";
      this.tabPageeDisclosure.Padding = new Padding(0, 3, 3, 3);
      this.tabPageeDisclosure.Size = new Size(775, 566);
      this.tabPageeDisclosure.TabIndex = 1;
      this.tabPageeDisclosure.Text = "eDisclosure Tracking";
      this.tabPageeDisclosure.UseVisualStyleBackColor = true;
      this.pnlFulfillment.Controls.Add((Control) this.txtFulfillmentComments);
      this.pnlFulfillment.Controls.Add((Control) this.label43);
      this.pnlFulfillment.Controls.Add((Control) this.txtFulfillmentMethod);
      this.pnlFulfillment.Controls.Add((Control) this.label31);
      this.pnlFulfillment.Controls.Add((Control) this.txtDateFulfillOrder);
      this.pnlFulfillment.Controls.Add((Control) this.txtFulfillmentOrderBy);
      this.pnlFulfillment.Controls.Add((Control) this.label30);
      this.pnlFulfillment.Controls.Add((Control) this.label29);
      this.pnlFulfillment.Controls.Add((Control) this.gradientPanel4);
      this.pnlFulfillment.Dock = DockStyle.Top;
      this.pnlFulfillment.Location = new Point(0, 325);
      this.pnlFulfillment.Name = "pnlFulfillment";
      this.pnlFulfillment.Size = new Size(772, 200);
      this.pnlFulfillment.TabIndex = 1;
      this.txtFulfillmentComments.Location = new Point(125, 117);
      this.txtFulfillmentComments.Multiline = true;
      this.txtFulfillmentComments.Name = "txtFulfillmentComments";
      this.txtFulfillmentComments.ReadOnly = true;
      this.txtFulfillmentComments.Size = new Size(258, 76);
      this.txtFulfillmentComments.TabIndex = 66;
      this.label43.AutoSize = true;
      this.label43.Location = new Point(5, 120);
      this.label43.Name = "label43";
      this.label43.Size = new Size(56, 13);
      this.label43.TabIndex = 65;
      this.label43.Text = "Comments";
      this.txtFulfillmentMethod.Location = new Point(125, 95);
      this.txtFulfillmentMethod.Name = "txtFulfillmentMethod";
      this.txtFulfillmentMethod.ReadOnly = true;
      this.txtFulfillmentMethod.Size = new Size(258, 20);
      this.txtFulfillmentMethod.TabIndex = 64;
      this.label31.AutoSize = true;
      this.label31.Location = new Point(5, 98);
      this.label31.Name = "label31";
      this.label31.Size = new Size(92, 13);
      this.label31.TabIndex = 63;
      this.label31.Text = "Fulfillment Method";
      this.txtDateFulfillOrder.Location = new Point(125, 73);
      this.txtDateFulfillOrder.Name = "txtDateFulfillOrder";
      this.txtDateFulfillOrder.ReadOnly = true;
      this.txtDateFulfillOrder.Size = new Size(258, 20);
      this.txtDateFulfillOrder.TabIndex = 62;
      this.txtFulfillmentOrderBy.Location = new Point(125, 51);
      this.txtFulfillmentOrderBy.Name = "txtFulfillmentOrderBy";
      this.txtFulfillmentOrderBy.ReadOnly = true;
      this.txtFulfillmentOrderBy.Size = new Size(258, 20);
      this.txtFulfillmentOrderBy.TabIndex = 23;
      this.label30.AutoSize = true;
      this.label30.Location = new Point(5, 76);
      this.label30.Name = "label30";
      this.label30.Size = new Size(96, 13);
      this.label30.TabIndex = 4;
      this.label30.Text = "Date/Time Fulfilled";
      this.label29.AutoSize = true;
      this.label29.Location = new Point(5, 54);
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
      this.gradientPanel4.Size = new Size(770, 25);
      this.gradientPanel4.Style = GradientPanel.PanelStyle.TableFooter;
      this.gradientPanel4.TabIndex = 2;
      this.btneDiscManualFulfill.Location = new Point(499, 1);
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
      this.pnleDisclosureStatus.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.pnleDisclosureStatus.Controls.Add((Control) this.lblePackageId);
      this.pnleDisclosureStatus.Controls.Add((Control) this.label2);
      this.pnleDisclosureStatus.Controls.Add((Control) this.lblDateeDisclosureSent);
      this.pnleDisclosureStatus.Controls.Add((Control) this.grdDisclosureTracking);
      this.pnleDisclosureStatus.Controls.Add((Control) this.lblViewForm);
      this.pnleDisclosureStatus.Controls.Add((Control) this.llViewDetails);
      this.pnleDisclosureStatus.Controls.Add((Control) this.label19);
      this.pnleDisclosureStatus.Controls.Add((Control) this.gradientPanel2);
      this.pnleDisclosureStatus.Dock = DockStyle.Top;
      this.pnleDisclosureStatus.Location = new Point(0, 3);
      this.pnleDisclosureStatus.Name = "pnleDisclosureStatus";
      this.pnleDisclosureStatus.Size = new Size(772, 322);
      this.pnleDisclosureStatus.TabIndex = 0;
      this.lblePackageId.AutoSize = true;
      this.lblePackageId.Location = new Point(193, 59);
      this.lblePackageId.Name = "lblePackageId";
      this.lblePackageId.Size = new Size(67, 13);
      this.lblePackageId.TabIndex = 70;
      this.lblePackageId.Text = "ePackageID";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(9, 59);
      this.label2.Name = "label2";
      this.label2.Size = new Size(70, 13);
      this.label2.TabIndex = 69;
      this.label2.Text = "ePackage ID";
      this.lblDateeDisclosureSent.AutoSize = true;
      this.lblDateeDisclosureSent.Location = new Point(190, 36);
      this.lblDateeDisclosureSent.Name = "lblDateeDisclosureSent";
      this.lblDateeDisclosureSent.Size = new Size(107, 13);
      this.lblDateeDisclosureSent.TabIndex = 68;
      this.lblDateeDisclosureSent.Text = "eDisclosureSentDate";
      this.grdDisclosureTracking.AllowMultiselect = false;
      this.grdDisclosureTracking.Anchor = AnchorStyles.None;
      this.grdDisclosureTracking.AutoHeight = true;
      this.grdDisclosureTracking.BorderColor = Color.LightGray;
      this.grdDisclosureTracking.BorderStyle = BorderStyle.FixedSingle;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column1";
      gvColumn3.Text = " ";
      gvColumn3.Width = 217;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Borrower";
      gvColumn4.SortMethod = GVSortMethod.None;
      gvColumn4.Text = "Borrower";
      gvColumn4.Width = 155;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Co-Borrower";
      gvColumn5.Text = "Co-Borrower";
      gvColumn5.Width = 155;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Loan Originator";
      gvColumn6.Text = "Loan Originator";
      gvColumn6.Width = 150;
      this.grdDisclosureTracking.Columns.AddRange(new GVColumn[4]
      {
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6
      });
      this.grdDisclosureTracking.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.grdDisclosureTracking.Location = new Point(9, 86);
      this.grdDisclosureTracking.Name = "grdDisclosureTracking";
      this.grdDisclosureTracking.Size = new Size(686, 219);
      this.grdDisclosureTracking.TabIndex = 67;
      this.lblViewForm.AutoSize = true;
      this.lblViewForm.ForeColor = SystemColors.Highlight;
      this.lblViewForm.Location = new Point(702, 142);
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
      this.gradientPanel2.Size = new Size(770, 25);
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
      this.btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnClose.Location = new Point(713, 603);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new Size(75, 23);
      this.btnClose.TabIndex = 5;
      this.btnClose.Text = "Close";
      this.btnClose.UseVisualStyleBackColor = true;
      this.btnClose.Click += new EventHandler(this.btnClose_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(791, 630);
      this.Controls.Add((Control) this.btnClose);
      this.Controls.Add((Control) this.tcDisclosure);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (DisclosureDetailsDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Disclosure Details";
      this.tcDisclosure.ResumeLayout(false);
      this.tabPageDetail.ResumeLayout(false);
      this.pnlLoanSnapshot.ResumeLayout(false);
      this.pnlLoanSnapshot.PerformLayout();
      this.gcDocList.ResumeLayout(false);
      this.gradientPanel3.ResumeLayout(false);
      this.gradientPanel3.PerformLayout();
      this.flowLayoutPanel2.ResumeLayout(false);
      this.pnlBasicInfo.ResumeLayout(false);
      this.pnlBasicInfo.PerformLayout();
      this.gradientPanel1.ResumeLayout(false);
      this.gradientPanel1.PerformLayout();
      this.tabPageeDisclosure.ResumeLayout(false);
      this.pnlFulfillment.ResumeLayout(false);
      this.pnlFulfillment.PerformLayout();
      this.gradientPanel4.ResumeLayout(false);
      this.gradientPanel4.PerformLayout();
      this.pnleDisclosureStatus.ResumeLayout(false);
      this.pnleDisclosureStatus.PerformLayout();
      this.gradientPanel2.ResumeLayout(false);
      this.gradientPanel2.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
