// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.BuySellForm
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using Elli.ElliEnum.Triggers;
using EllieMae.EMLite.BusinessRules;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.EPC2;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.ProductPricing;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.Export;
using EllieMae.EMLite.InputEngine;
using EllieMae.EMLite.LoanUtils.RateLocks;
using EllieMae.EMLite.LoanUtils.Services;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using EllieMae.EMLite.Xml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class BuySellForm : Form
  {
    private const string className = "BuySellForm";
    private static readonly string sw = Tracing.SwInputEngine;
    private bool notAllowedPricingChangeSetting;
    private string origLockRequestGuid;
    private bool isNewLock;
    private static BuySellForm _instance = (BuySellForm) null;
    private LockForm lockForm;
    private LoanDataMgr loanMgr;
    private CurrentLockForm currentLockForm;
    private LockRequestLog lockLog;
    private LockRequestLog currentLog;
    private Routine fx;
    private LoanSnapshotForm snapshotForm;
    private RequestSnapshotForm requestSnapshotForm;
    private BusinessRuleCheck ruleChecker = new BusinessRuleCheck();
    private LRAdditionalFields lrFields;
    private bool isDirty;
    private bool isSummary;
    private Sessions.Session session;
    private StatusReport statusReport;
    private Hashtable dataTables;
    private bool lockConfirm;
    private bool lockDeny;
    private string denialComments = string.Empty;
    private bool lockCancel;
    private bool updateSellComparison;
    private string cancellationComments = string.Empty;
    private IContainer components;
    private Panel panel1;
    private Button btnCancel;
    private Button btnOK;
    private Button btnLockConfirm;
    private TabControl tabControlLock;
    private TabPage tabBuySell;
    private TabPage tabSnapshot;
    private Panel panelTop;
    private Button btnDeny;
    private Button btnBuySidePricing;
    private Button btnRevise;
    private TabPage tabPageRequest;
    private Button btnSellSidePricing;
    private FlowLayoutPanel flowLayoutPanel1;
    private Button btnDataCompare;
    private FlowLayoutPanel flowLayoutPanel2;
    private Button btnDenyLockExtension;
    private Button btnAcceptLockCancellation;
    private GradientPanel ViewPanel;
    private RadioButton radViewDetail;
    private RadioButton radViewSummary;
    private Label label3;
    private Button btnValidate;
    private Button btnSaveProgress;
    private Button btnSave;

    public event EventHandler ConfirmButtonClicked;

    public event EventHandler DenyButtonClicked;

    public event EventHandler UpdateSellComparisonButtonClicked;

    public event EventHandler SaveButtonClicked;

    public event EventHandler CancelButtonClicked;

    public event EventHandler ReviseButtonClicked;

    public event EventHandler ValidateButtonClicked;

    public event EventHandler GetPricingCompleted;

    public static BuySellForm Instance
    {
      get
      {
        if (BuySellForm._instance == null)
          BuySellForm._instance = new BuySellForm(Session.DefaultInstance);
        return BuySellForm._instance;
      }
    }

    public static bool IsFormDisplayed
    {
      get
      {
        if (BuySellForm._instance != null && BuySellForm._instance.Visible)
          return true;
        BuySellForm._instance = (BuySellForm) null;
        return false;
      }
    }

    public BuySellForm(Sessions.Session session)
    {
      this.session = session;
      this.InitializeComponent();
      this.fx = new Routine(this.populateOptimalblueResult);
      this.session.LoanData.RegisterFieldValueChangeEventHandler("OPTIMAL.RESPONSE", this.fx);
      this.enforceSecurity();
      this.radViewDetail.Checked = true;
      this.radViewDetail.TabStop = false;
      this.radViewSummary.Checked = false;
      this.notAllowedPricingChangeSetting = session.StartupInfo.PolicySettings.Contains((object) "Policies.NotAllowPricingChange") && (bool) session.StartupInfo.PolicySettings[(object) "Policies.NotAllowPricingChange"];
    }

    private void enforceSecurity()
    {
      try
      {
        if (this.session.StartupInfo.ProductPricingPartner == null)
        {
          this.btnSellSidePricing.Visible = false;
        }
        else
        {
          ProductPricingUtils.SynchronizeProductPricingSettingsWithServer();
          if (!this.session.StartupInfo.ProductPricingPartner.ShowSellSide)
          {
            this.btnSellSidePricing.Visible = false;
          }
          else
          {
            this.btnBuySidePricing.Text = "Get Buy Side Pricing";
            this.btnBuySidePricing.Size = new Size(this.btnSellSidePricing.Width, this.btnSellSidePricing.Height);
          }
        }
      }
      catch (Exception ex)
      {
        this.btnSellSidePricing.Visible = false;
      }
    }

    public void RefreshPage()
    {
      bool flag1 = false;
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      if (this.loanMgr == null || this.lockLog == null)
        return;
      if (this.loanMgr.LoanData.GetField("OPTIMAL.REQUEST") != "")
      {
        Dictionary<string, string> keyValuePair = new ValuePairXmlWriter(this.loanMgr.LoanData.GetField("OPTIMAL.REQUEST"), "FieldID", "FieldValue").GetKeyValuePair();
        if (!keyValuePair.ContainsKey("SnapShotGuid") || keyValuePair["SnapShotGuid"] != this.lockLog.Guid)
          return;
        if (keyValuePair.ContainsKey("IsHistoricalPricing") && keyValuePair["IsHistoricalPricing"] == "True")
          flag1 = true;
      }
      Hashtable hashtable1 = new Hashtable();
      Hashtable hashtable2 = this.dataTables == null || this.dataTables.Count <= 0 ? this.lockLog.GetLockRequestSnapshot() : this.dataTables;
      bool flag2 = false;
      ProductPricingParser productPricingParser = (ProductPricingParser) null;
      if (this.loanMgr.LoanData.GetField("OPTIMAL.RESPONSE") != "")
      {
        if (this.loanMgr.ValidationsEnabled)
        {
          flag2 = true;
          this.loanMgr.ValidationsEnabled = false;
        }
        productPricingParser = new ProductPricingParser(this.loanMgr.LoanData.GetField("OPTIMAL.RESPONSE"), hashtable2);
        Dictionary<string, string> formatedPartnerData = productPricingParser.FormatedPartnerData;
        if (formatedPartnerData.Count != 2 || !formatedPartnerData.ContainsKey("ExternalAction") || !(formatedPartnerData["ExternalAction"] == "LockAndConfirm") || !formatedPartnerData.ContainsKey("UncleanFields"))
          hashtable2 = productPricingParser.MergedData;
        if (flag2)
          this.loanMgr.ValidationsEnabled = !this.loanMgr.ValidationsEnabled;
      }
      if (flag1 && hashtable2.Contains((object) "2149") && this.loanMgr.LoanData.GetField("OPTIMAL.HISTORY") != "")
      {
        string fromEppsTxHistory = LockUtils.GetQualifiedAsOfDateFromEppsTxHistory(this.loanMgr.LoanData.GetField("OPTIMAL.HISTORY"));
        if (!string.IsNullOrEmpty(fromEppsTxHistory))
          hashtable2[(object) "2149"] = (object) fromEppsTxHistory;
      }
      if (hashtable2.Contains((object) "4059") && hashtable2[(object) "4059"].ToString() == "Y" && hashtable2.Contains((object) "4070") && hashtable2.Contains((object) "4058"))
        hashtable2[(object) "2149"] = (object) LockDeskHoursManager.GetLockDateForOnrp((IClientSession) this.session.SessionObjects.Session, this.session.SessionObjects, this.loanMgr, Utils.ParseDate((object) (hashtable2[(object) "4070"].ToString() + " " + hashtable2[(object) "4058"].ToString()))).ToShortDateString();
      this.lockForm.RefreshScreen(hashtable2, this.lockLog);
      this.currentLockForm.RefreshScreen(hashtable2, this.lockLog, this.lockForm.Height);
      this.requestSnapshotForm.RefreshSnapshotForm(hashtable2, this.loanMgr.LoanData);
      if (productPricingParser == null)
        return;
      switch (productPricingParser.ExternalActionRequested)
      {
        case ExternalAction.LockAndConfirm:
          this.btnLockConfirm_Click((object) this.btnSellSidePricing, (EventArgs) null);
          break;
        case ExternalAction.DenyLock:
          this.btnDeny_Click((object) null, (EventArgs) null);
          break;
      }
    }

    public void InitForm(LoanDataMgr loanMgr, LockRequestLog lockLog, LockRequestLog currentLog)
    {
      this.InitForm(loanMgr, lockLog, currentLog, false, false);
    }

    public void InitForm(
      LoanDataMgr loanMgr,
      LockRequestLog lockLog,
      LockRequestLog currentLog,
      bool keepSellData,
      bool isVoidLock,
      string lockRequestGuid,
      bool isNewLock,
      bool isUpdateSell = false)
    {
      this.origLockRequestGuid = lockRequestGuid;
      this.isNewLock = isNewLock;
      this.InitForm(loanMgr, lockLog, currentLog, keepSellData, isVoidLock, isUpdateSell);
    }

    public void InitForm(
      LoanDataMgr loanMgr,
      LockRequestLog lockLog,
      LockRequestLog currentLog,
      bool keepSellData,
      bool isVoidLock,
      bool isUpdateSell = false)
    {
      this.loanMgr = loanMgr;
      this.lockLog = lockLog;
      this.currentLog = currentLog;
      this.lockDeny = false;
      this.snapshotForm = new LoanSnapshotForm(this.session, lockLog, false, this.loanMgr.LoanData);
      this.tabSnapshot.Controls.Add((Control) this.snapshotForm);
      this.snapshotForm.BringToFront();
      this.requestSnapshotForm = new RequestSnapshotForm(this.session, lockLog, false, this.loanMgr.LoanData);
      this.tabPageRequest.Controls.Add((Control) this.requestSnapshotForm);
      this.requestSnapshotForm.BringToFront();
      LockRequestLog confirmedLockRequest = this.session.LoanDataMgr.LoanData.GetLogList().GetCurrentConfirmedLockRequest();
      if (currentLog != null)
      {
        this.dataTables = currentLog.GetLockRequestSnapshot();
        if (this.dataTables == null)
          this.dataTables = new Hashtable();
        else if (lockLog.RequestedStatus != RateLockEnum.GetRateLockStatusString(RateLockRequestStatus.Requested))
        {
          if (!keepSellData)
          {
            for (int index = 0; index < LockRequestLog.SellSideFields.Count; ++index)
              this.setFieldValue(LockRequestLog.SellSideFields[index], "");
          }
          if (this.snapshotForm.CurrentSnapshotData == null)
          {
            Hashtable loanSnapshot = this.loanMgr.LoanData.PrepareLockRequestData();
            string empty = string.Empty;
            foreach (DictionaryEntry dictionaryEntry in loanSnapshot)
            {
              string id = dictionaryEntry.Key.ToString();
              if (!(id == "2149") && !(id == "2151") && !(id == "2220") && !(id == "2222") && !(id == "3664") && !(id == "3666"))
                this.setFieldValue(id, dictionaryEntry.Value.ToString());
            }
            for (int index = 0; index < LockRequestLog.LoanInfoSnapshotFields.Count; ++index)
            {
              if (!loanSnapshot.Contains((object) LockRequestLog.LoanInfoSnapshotFields[index]) && this.dataTables.Contains((object) LockRequestLog.LoanInfoSnapshotFields[index]))
                this.dataTables.Remove((object) LockRequestLog.LoanInfoSnapshotFields[index]);
            }
            this.snapshotForm.RefreshSnapshotForm(loanSnapshot, this.loanMgr.LoanData);
            this.requestSnapshotForm.RefreshSnapshotForm(loanSnapshot, this.loanMgr.LoanData);
            for (int index = 0; index < LockRequestLog.LoanInfoSnapshotFields.Count; ++index)
            {
              if (loanSnapshot.ContainsKey((object) LockRequestLog.LoanInfoSnapshotFields[index]))
                this.setFieldValue(LockRequestLog.LoanInfoSnapshotFields[index], loanSnapshot[(object) LockRequestLog.LoanInfoSnapshotFields[index]].ToString());
            }
          }
        }
        else
        {
          Hashtable lockRequestSnapshot = this.lockLog.GetLockRequestSnapshot();
          for (int index = 0; index < LockRequestLog.LoanInfoSnapshotFields.Count; ++index)
            this.setFieldValue(LockRequestLog.LoanInfoSnapshotFields[index], this.getFieldValue(LockRequestLog.LoanInfoSnapshotFields[index], lockRequestSnapshot));
        }
      }
      else
      {
        this.dataTables = this.lockLog.GetLockRequestSnapshot();
        if (this.dataTables == null)
        {
          this.btnBuySidePricing.Enabled = true;
          this.btnSellSidePricing.Enabled = true;
          this.dataTables = new Hashtable();
        }
        else if (this.lockLog.RequestedStatus != "Old Request" && this.lockLog.RequestedStatus != "Requested")
        {
          this.btnBuySidePricing.Enabled = false;
          this.btnSellSidePricing.Enabled = false;
        }
        if (this.session.StartupInfo.ProductPricingPartner != null && this.session.StartupInfo.ProductPricingPartner.IsEPPS && confirmedLockRequest != null && confirmedLockRequest.Guid == this.lockLog.Guid && this.dataTables.Contains((object) "OPTIMAL.HISTORY") && this.dataTables[(object) "OPTIMAL.HISTORY"].ToString().Contains("<MPS_envelope") && ProductPricingUtils.IsHistoricalPricingEnabled)
        {
          if (this.dataTables.Contains((object) "3902") && this.dataTables[(object) "3902"].ToString().Trim() != string.Empty)
          {
            if (this.dataTables.Contains((object) "3911") && this.dataTables[(object) "3911"].ToString().Contains("Individual"))
              this.btnValidate.Visible = true;
            else
              this.btnValidate.Visible = false;
          }
          else if (!LockUtils.IfShipDark(this.session.SessionObjects, "EPPS_EPC2_SHIP_DARK_SR") && string.Compare(LockUtils.GetRequestLockStatus(this.loanMgr.LoanData), "Expired Lock", StringComparison.OrdinalIgnoreCase) == 0 && this.session.StartupInfo.ProductPricingPartner.VendorPlatform == VendorPlatform.EPC2)
          {
            this.btnValidate.Visible = true;
            this.btnValidate.Enabled = false;
          }
          else
            this.btnValidate.Visible = true;
        }
      }
      if (lockLog.IsFakeRequest)
      {
        if (currentLog != null && currentLog.IsLockExtension)
        {
          this.dataTables[(object) "2151"] = this.dataTables[(object) "3364"];
          this.dataTables[(object) "2150"] = (object) (Utils.ParseInt(this.dataTables[(object) "3431"]) + Utils.ParseInt(this.dataTables[(object) "2150"]));
        }
        if (this.dataTables.ContainsKey((object) "3431"))
          this.dataTables.Remove((object) "3431");
        if (this.dataTables.ContainsKey((object) "3358"))
          this.dataTables.Remove((object) "3358");
      }
      if (!lockLog.IsLockExtension && !lockLog.IsLockCancellation)
        this.setFieldValue("3364", "");
      if (lockLog.IsLockCancellation | isVoidLock)
      {
        this.lockForm = new LockForm(this.loanMgr, true, false, (LoanLockTool) null, false, this.isSummary);
        this.btnRevise.Visible = false;
        this.btnCancel.Text = "&Close";
      }
      else if (lockLog.RequestedStatus == RateLockEnum.GetRateLockStatusString(RateLockRequestStatus.NotLocked))
      {
        this.lockForm = new LockForm(this.loanMgr, true, false, (LoanLockTool) null, false, this.isSummary);
        this.btnRevise.Visible = false;
        this.btnCancel.Text = "&Close";
      }
      else if (lockLog.RequestedStatus == RateLockEnum.GetRateLockStatusString(RateLockRequestStatus.RateLocked) || lockLog.RequestedStatus == RateLockEnum.GetRateLockStatusString(RateLockRequestStatus.OldLock) || lockLog.RequestedStatus == RateLockEnum.GetRateLockStatusString(RateLockRequestStatus.RequestDenied))
      {
        this.lockForm = new LockForm(this.loanMgr, true, false, (LoanLockTool) null, false, this.isSummary, isUpdateSell);
        this.btnCancel.Text = "&Close";
      }
      else
      {
        this.lockForm = new LockForm(this.loanMgr, false, false, (LoanLockTool) null, false, this.isSummary, isUpdateSell);
        this.btnRevise.Visible = false;
      }
      this.lockForm.ZoomButtonClicked += new EventHandler(this.lockForm_ZoomButtonClicked);
      this.lockForm.Top = this.ViewPanel.Height;
      this.lockForm.Left = 332;
      this.lockForm.RefreshScreen(this.dataTables, this.lockLog);
      this.lockForm.IsSummary = this.isSummary = this.radViewSummary.Checked;
      this.lockForm.LockFormView();
      this.tabBuySell.Controls.Add((Control) this.lockForm);
      this.lockForm.BringToFront();
      DateTime dateTime = DateTime.MinValue;
      if (this.lockLog.IsLockExtension && this.dataTables.ContainsKey((object) "3369") && Utils.IsDate(this.dataTables[(object) "3369"]))
        dateTime = Utils.ParseDate(this.dataTables[(object) "3369"], DateTime.MinValue);
      if (isVoidLock)
      {
        this.btnDeny.Visible = false;
        this.btnDenyLockExtension.Visible = false;
        this.btnBuySidePricing.Visible = false;
        this.btnSellSidePricing.Visible = false;
        this.btnLockConfirm.Visible = false;
        this.btnOK.Visible = false;
        this.btnAcceptLockCancellation.Visible = false;
        this.btnDataCompare.Visible = false;
      }
      else if (lockLog.RequestedStatus == RateLockEnum.GetRateLockStatusString(RateLockRequestStatus.RequestDenied))
      {
        this.btnDeny.Visible = false;
        this.btnDenyLockExtension.Visible = false;
        this.btnLockConfirm.Visible = false;
        this.btnOK.Visible = false;
        this.btnRevise.Left = this.btnDeny.Left;
        this.btnCancel.Text = "&Close";
        this.btnAcceptLockCancellation.Visible = false;
        if (this.lockLog.IsLockExtension)
        {
          this.btnBuySidePricing.Visible = false;
          this.btnSellSidePricing.Visible = false;
          if (dateTime < DateTime.Today)
          {
            this.btnLockConfirm.Visible = false;
            this.btnOK.Visible = false;
            this.btnDenyLockExtension.Visible = false;
          }
        }
        else
        {
          this.btnBuySidePricing.Visible = true;
          this.btnSellSidePricing.Visible = true;
        }
      }
      else if (lockLog.RequestedStatus == RateLockEnum.GetRateLockStatusString(RateLockRequestStatus.RateLocked))
      {
        if (this.lockLog.IsLockExtension)
        {
          this.btnDeny.Visible = false;
          this.btnDenyLockExtension.Visible = true;
          this.btnBuySidePricing.Visible = false;
          this.btnSellSidePricing.Visible = false;
          this.btnAcceptLockCancellation.Visible = false;
          if (dateTime < DateTime.Today)
          {
            this.btnLockConfirm.Visible = false;
            this.btnOK.Visible = false;
            this.btnDenyLockExtension.Visible = false;
          }
        }
        else
        {
          this.btnDeny.Visible = true;
          this.btnDenyLockExtension.Visible = false;
          this.btnBuySidePricing.Visible = true;
          this.btnSellSidePricing.Visible = true;
          this.btnAcceptLockCancellation.Visible = false;
        }
        this.btnOK.Enabled = false;
      }
      else if (lockLog.IsLockCancellation && lockLog.RequestedStatus != RateLockEnum.GetRateLockStatusString(RateLockRequestStatus.Requested))
      {
        this.btnDeny.Visible = false;
        this.btnDenyLockExtension.Visible = false;
        this.btnBuySidePricing.Visible = false;
        this.btnSellSidePricing.Visible = false;
        this.btnLockConfirm.Visible = false;
        this.btnOK.Visible = false;
        this.btnAcceptLockCancellation.Visible = false;
        this.btnDataCompare.Visible = false;
      }
      else if (lockLog.IsLockExtension && lockLog.RequestedStatus == RateLockEnum.GetRateLockStatusString(RateLockRequestStatus.Requested))
      {
        this.btnDeny.Visible = false;
        this.btnDenyLockExtension.Visible = true;
        this.btnBuySidePricing.Visible = false;
        this.btnSellSidePricing.Visible = false;
        this.btnAcceptLockCancellation.Visible = false;
        this.btnLockConfirm.Visible = true;
        this.btnOK.Visible = true;
        this.btnDenyLockExtension.Visible = true;
      }
      else if (lockLog.RequestedStatus == RateLockEnum.GetRateLockStatusString(RateLockRequestStatus.NotLocked))
      {
        this.btnDeny.Visible = false;
        this.btnDenyLockExtension.Visible = false;
        this.btnBuySidePricing.Visible = false;
        this.btnSellSidePricing.Visible = false;
        this.btnLockConfirm.Visible = false;
        this.btnOK.Visible = false;
        this.btnAcceptLockCancellation.Visible = false;
        this.btnDataCompare.Visible = false;
      }
      else if (this.lockLog.IsLockExtension)
      {
        this.btnDeny.Visible = false;
        this.btnDenyLockExtension.Visible = true;
        this.btnBuySidePricing.Visible = false;
        this.btnSellSidePricing.Visible = false;
        this.btnAcceptLockCancellation.Visible = false;
        if (dateTime < DateTime.Today)
        {
          this.btnLockConfirm.Visible = false;
          this.btnOK.Visible = false;
          this.btnDenyLockExtension.Visible = false;
        }
      }
      else if (this.lockLog.IsLockCancellation)
      {
        this.btnDeny.Visible = false;
        this.btnDenyLockExtension.Visible = false;
        this.btnBuySidePricing.Visible = false;
        this.btnSellSidePricing.Visible = false;
        this.btnLockConfirm.Visible = false;
        this.btnOK.Visible = false;
        this.btnAcceptLockCancellation.Visible = true;
        this.btnDataCompare.Visible = false;
      }
      else
      {
        this.btnDeny.Visible = true;
        this.btnDenyLockExtension.Visible = false;
        this.btnBuySidePricing.Visible = true;
        this.btnSellSidePricing.Visible = true;
        this.btnAcceptLockCancellation.Visible = false;
      }
      if (isUpdateSell)
      {
        this.btnBuySidePricing.Visible = false;
        this.btnSellSidePricing.Visible = true;
        this.btnSellSidePricing.Enabled = true;
        this.btnValidate.Visible = false;
        this.btnSaveProgress.Visible = false;
        this.btnAcceptLockCancellation.Visible = false;
        this.btnRevise.Visible = false;
        this.btnDenyLockExtension.Visible = false;
        this.btnDeny.Visible = false;
        this.btnOK.Visible = false;
        this.btnLockConfirm.Visible = false;
        this.btnSave.Visible = true;
        this.btnCancel.Text = "&Cancel";
      }
      LockRequestLog[] allLockRequests = this.loanMgr.LoanData.GetLogList().GetAllLockRequests();
      LockRequestLog lockRequestLog = ((IEnumerable<LockRequestLog>) allLockRequests).LastOrDefault<LockRequestLog>((Func<LockRequestLog, bool>) (r => r.LockRequestStatus == RateLockRequestStatus.RateLocked)) ?? ((IEnumerable<LockRequestLog>) allLockRequests).LastOrDefault<LockRequestLog>((Func<LockRequestLog, bool>) (r => r.LockRequestStatus == RateLockRequestStatus.RequestDenied));
      if (lockRequestLog != null && lockRequestLog.Guid == lockLog.Guid)
      {
        this.btnOK.Visible = true;
        this.btnOK.Enabled = true;
      }
      else
        this.btnOK.Visible = false;
      if (lockLog.RequestedStatus == RateLockEnum.GetRateLockStatusString(RateLockRequestStatus.Requested))
        this.btnSaveProgress.Visible = true;
      this.currentLockForm = new CurrentLockForm(this.loanMgr);
      this.currentLockForm.RefreshScreen(this.dataTables, this.lockLog, this.lockForm.Height);
      this.currentLockForm.Top = this.ViewPanel.Height;
      this.currentLockForm.Left = 0;
      this.tabBuySell.Controls.Add((Control) this.currentLockForm);
      this.currentLockForm.BringToFront();
      bool hideBR = true;
      if (this.lockForm.IsBaseRateDisplayedAll || this.currentLockForm.IsBaseRateDisplayedAll)
        hideBR = false;
      bool hideBP = true;
      if (this.lockForm.IsBasePriceDisplayedAll || this.currentLockForm.IsBasePriceDisplayedAll)
        hideBP = false;
      bool hideBM = true;
      if (this.lockForm.IsBaseMarginDisplayedAll || this.currentLockForm.IsBaseMarginDisplayedAll)
        hideBM = false;
      bool hidePR = true;
      if (this.lockForm.IsProfitMarginDisplayedAll || this.currentLockForm.IsProfitMarginDisplayedAll)
        hidePR = false;
      bool hideEX = true;
      if (this.lockForm.IsExtensionDisplayedAll || this.currentLockForm.IsExtensionDisplayedAll)
        hideEX = false;
      bool hideRL = true;
      if (this.lockForm.IsRelockDisplayedAll || this.currentLockForm.IsRelockDisplayedAll)
        hideRL = false;
      bool hideCPA = true;
      if (this.lockForm.IsCPADisplayedAll || this.currentLockForm.IsCPADisplayedAll)
        hideCPA = false;
      bool hideCPC = true;
      if (this.lockForm.IsCPCDisplayedAll || this.currentLockForm.IsCPCDisplayedAll)
        hideCPC = false;
      bool hideBPC = true;
      if (this.lockForm.IsBPCDisplayedAll || this.currentLockForm.IsBPCDisplayedAll)
        hideBPC = false;
      this.currentLockForm.ZoomPanels(hideBR, hideBP, hideBM, hidePR, hideEX, hideRL, hideCPA, hideCPC, hideBPC);
      this.lockForm.ZoomPanels(hideBR, hideBP, hideBM, hidePR, hideEX, hideRL, hideCPA, hideCPC, hideBPC);
      this.currentLockForm.Height = this.lockForm.Height;
      try
      {
        PopupBusinessRules popupBusinessRules = new PopupBusinessRules(this.loanMgr.LoanData, (ResourceManager) null, (Image) null, (Image) null, this.session);
        popupBusinessRules.SetBusinessRules(this.btnRevise);
        popupBusinessRules.SetBusinessRules(this.btnBuySidePricing);
        popupBusinessRules.SetBusinessRules(this.btnLockConfirm);
        popupBusinessRules.SetBusinessRules(this.btnOK);
        popupBusinessRules.SetBusinessRules(this.btnDeny);
        popupBusinessRules.SetBusinessRules(this.btnSellSidePricing);
        popupBusinessRules.SetBusinessRules(this.btnValidate);
      }
      catch (Exception ex)
      {
        Tracing.Log(BuySellForm.sw, TraceLevel.Error, nameof (BuySellForm), "Cannot set Button access right. Error: " + ex.Message);
      }
      if (this.loanMgr.LoanData.IsULDDExporting)
        this.btnBuySidePricing.Enabled = this.btnSellSidePricing.Enabled = false;
      if (!lockLog.IsRelock || !string.Equals(this.lockLog.RequestedStatus, RateLockEnum.GetRateLockStatusString(RateLockRequestStatus.OldRequest)) && !string.Equals(this.lockLog.RequestedStatus, RateLockEnum.GetRateLockStatusString(RateLockRequestStatus.Requested)))
        return;
      if (this.lockLog.IsLockExtension)
      {
        this.btnBuySidePricing.Visible = true;
        this.btnSellSidePricing.Visible = true;
      }
      this.btnBuySidePricing.Enabled = false;
      if (this.session.StartupInfo.ProductPricingPartner == null || !this.session.StartupInfo.ProductPricingPartner.IsEPPS || !this.dataTables.Contains((object) "OPTIMAL.HISTORY") || !this.dataTables[(object) "OPTIMAL.HISTORY"].ToString().Contains("<MPS_envelope") || !ProductPricingUtils.IsHistoricalPricingEnabled || this.loanMgr.LoanData.IsULDDExporting)
        return;
      this.btnBuySidePricing.Enabled = true;
    }

    private void lockForm_ZoomButtonClicked(object sender, EventArgs e)
    {
      this.currentLockForm.ZoomPanels(!this.lockForm.IsBaseRateDisplayedAll, !this.lockForm.IsBasePriceDisplayedAll, !this.lockForm.IsBaseMarginDisplayedAll, !this.lockForm.IsProfitMarginDisplayedAll, !this.lockForm.IsExtensionDisplayedAll, !this.lockForm.IsRelockDisplayedAll, !this.lockForm.IsCPADisplayedAll, !this.lockForm.IsCPCDisplayedAll, !this.lockForm.IsBPCDisplayedAll);
      this.currentLockForm.Height = this.lockForm.Height;
    }

    public void RefreshCurrentLoanSnapshotData(bool getRequestLockStatus = false)
    {
      if (this.snapshotForm.CurrentSnapshotData == null)
      {
        Hashtable loanSnapshot = this.loanMgr.LoanData.PrepareLockRequestData();
        if (getRequestLockStatus)
        {
          loanSnapshot[(object) "4209"] = (object) LockUtils.GetRequestLockStatus(this.loanMgr.LoanData);
          this.currentLockForm.RefreshRequestLockStatus(loanSnapshot[(object) "4209"] as string);
        }
        this.snapshotForm.RefreshSnapshotForm(loanSnapshot, this.loanMgr.LoanData);
        this.requestSnapshotForm.RefreshSnapshotForm(loanSnapshot, this.loanMgr.LoanData);
        for (int index = 0; index < LockRequestLog.LoanInfoSnapshotFields.Count; ++index)
        {
          if (loanSnapshot.ContainsKey((object) LockRequestLog.LoanInfoSnapshotFields[index]))
            this.setFieldValue(LockRequestLog.LoanInfoSnapshotFields[index], loanSnapshot[(object) LockRequestLog.LoanInfoSnapshotFields[index]].ToString());
        }
        string[] fields1 = this.loanMgr.LoanData.SecondaryAdditionalFields.GetFields(true);
        if (fields1 != null)
        {
          foreach (string baseFieldId in fields1)
          {
            if (loanSnapshot.ContainsKey((object) LockRequestCustomField.GenerateCustomFieldID(baseFieldId)))
              this.setFieldValue(LockRequestCustomField.GenerateCustomFieldID(baseFieldId), loanSnapshot[(object) LockRequestCustomField.GenerateCustomFieldID(baseFieldId)].ToString());
          }
        }
        string[] fields2 = this.loanMgr.LoanData.SecondaryAdditionalFields.GetFields(false);
        if (fields2 != null)
        {
          foreach (string str in fields2)
          {
            if (loanSnapshot.ContainsKey((object) str))
              this.setFieldValue(str, loanSnapshot[(object) str].ToString());
          }
        }
      }
      this.isDirty = false;
    }

    public Hashtable DataTables => this.dataTables;

    public bool LockConfirm => this.lockConfirm;

    public bool LockDeny => this.lockDeny;

    public string DenialComments => this.denialComments;

    public bool LockCancel => this.lockCancel;

    public bool UpdateSellComparison => this.updateSellComparison;

    public string CancellationComments => this.cancellationComments;

    private void btnOK_Click(object sender, EventArgs e)
    {
      this.updateSellComparison = true;
      this.btnDataCompare.Enabled = false;
      if (!string.IsNullOrEmpty(this.loanMgr.LoanData.GetField("4120")) && this.loanMgr.LoanData.GetField("4120") != "//")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, string.Format("This loan was Withdrawn on {0} and may not be Updated for Sell/Comparison. This lock must be Denied.", (object) this.loanMgr.LoanData.GetField("4120")), MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (this.ruleChecker.HasPrerequiredFields(this.session.LoanData, "BUTTON_LOCK", true, this.lockForm.ReadFieldValues()))
          return;
        this.lockForm.SetFieldValues(this.dataTables);
        LockRequestLog sender1 = this.prepRevisedSnapshot(this.lockLog.GetLockRequestSnapshot(), RateLockAction.UpdateSell);
        this.resetDelayedCompiledTrigger(true);
        if (this.UpdateSellComparisonButtonClicked == null)
          return;
        this.UpdateSellComparisonButtonClicked((object) sender1, e);
        Application.DoEvents();
        Application.DoEvents();
      }
    }

    private void btnLockConfirm_Click(object sender, EventArgs e)
    {
      if (!string.IsNullOrEmpty(this.loanMgr.LoanData.GetField("4120")) && this.loanMgr.LoanData.GetField("4120") != "//")
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, string.Format("This loan was Withdrawn on {0} and may not be Locked and Confirmed. This lock must be Denied.", (object) this.loanMgr.LoanData.GetField("4120")), MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (this.lockLog.IsLockExtension && this.lockLog.RateLockAction != RateLockAction.TradeExtension && this.lockLog.ReviseAction != RateLockAction.TradeExtension && !this.lockForm.ValidateLockExpirationDays())
          return;
        if (this.loanMgr.LoanData.GetField("2626") == "Correspondent" && !string.IsNullOrEmpty(this.loanMgr.LoanData.GetField("4207")) && this.loanMgr.LoanData.GetField("4207") != "//")
          this.loanMgr.LoanData.SetField("4207", "");
        if (sender == this.btnLockConfirm)
        {
          if (this.loanMgr.LoanData.GetField("4791").Equals("pending", StringComparison.OrdinalIgnoreCase) || this.loanMgr.LoanData.GetField("4791").Equals("denied", StringComparison.OrdinalIgnoreCase))
          {
            int num2 = (int) Utils.Dialog((IWin32Window) this, "This loan has a pending or denied price concession outstanding that must be approved before it can be confirmed.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return;
          }
          ProductPricingUtils.SynchronizeProductPricingSettingsWithServer();
          if (this.session.StartupInfo.ProductPricingPartner != null && this.session.StartupInfo.ProductPricingPartner.PartnerLockConfirm && this.btnSellSidePricing.Visible)
          {
            this.btnSellSidePricing_Click((object) this.btnLockConfirm, e);
            return;
          }
          this.loanMgr.LoanData.SetField("OPTIMAL.REQUEST", "");
          this.loanMgr.LoanData.SetField("OPTIMAL.REQUEST", this.buildRequestData());
          this.lockAndConfirm(true, false);
        }
        else if (sender == this.btnSellSidePricing)
          this.lockAndConfirm(true, true);
        if (!(this.loanMgr.LoanData.GetField("2626") == "Correspondent"))
          return;
        this.session.Application.GetService<ILoanConsole>().SaveLoan();
      }
    }

    private void lockAndConfirm(bool triggerCompare, bool forceConfirm)
    {
      RegulationAlerts.DisableKeyPricingAlerts = true;
      try
      {
        if (this.dataTables[(object) "4463"] != null)
        {
          if (this.dataTables[(object) "4463"].ToString().ToLower() == "lender")
            this.dataTables[(object) "4463"] = (object) "Lender Paid";
          if (this.dataTables[(object) "4463"].ToString().ToLower() == "borrower")
            this.dataTables[(object) "4463"] = (object) "Borrower Paid";
        }
        if (triggerCompare)
        {
          using (LockSnapshotCompareForm snapshotCompareForm = new LockSnapshotCompareForm(this.loanMgr, this.dataTables, true, forceConfirm, this.isNewLock))
          {
            if (snapshotCompareForm.LoanValueIsChanged)
            {
              switch (snapshotCompareForm.ShowDialog((IWin32Window) this))
              {
                case DialogResult.OK:
                  this.isDirty = true;
                  this.requestSnapshotForm.RefreshSnapshotForm(this.dataTables, this.loanMgr.LoanData);
                  int num = (int) Utils.Dialog((IWin32Window) this, "The lock has been successfully updated.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                  if (!forceConfirm)
                    return;
                  break;
                case DialogResult.Yes:
                  break;
                default:
                  return;
              }
            }
          }
        }
        if (this.session.SessionObjects.AllowConcurrentEditing && !this.session.LoanDataMgr.LockLoanWithExclusiveA(true) || this.ruleChecker.HasPrerequiredFields(this.session.LoanData, "BUTTON_LOCK AND CONFIRM", true, this.lockForm.ReadFieldValues()))
          return;
        TriggerImplDef def = this.session.LoanDataMgr.ApplyLoanTemplateTrigger(TriggerConditionType.LockConfirmed);
        if (def != null)
        {
          this.showApplyLoanTemplateProgress();
          this.session.LoanDataMgr.ApplyLoanTemplate(def);
          this.closeProgress();
        }
        this.lockConfirm = true;
        this.lockDeny = false;
        this.lockForm.SetFieldValues(this.dataTables);
        string field1 = this.loanMgr.LoanData.GetField("3039");
        if (this.notAllowedPricingChangeSetting && field1 != "//" && Utils.IsDate((object) field1))
          this.loanMgr.LoanData.SetField("3039", "", false);
        string text = this.lockLog.IsLockCancellation ? "Lock has been cancelled successfully." : "The lock has been successfully confirmed.";
        if (this.ConfirmButtonClicked != null)
        {
          try
          {
            this.ConfirmButtonClicked((object) this.btnLockConfirm, (EventArgs) null);
            Application.DoEvents();
            Application.DoEvents();
            if (this.IsDisposed)
              return;
            int num = (int) Utils.Dialog((IWin32Window) this, text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            this.isDirty = this.lockForm.IsDirty = false;
          }
          catch (RateLockNotConfirmException ex)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return;
          }
          finally
          {
            this.Close();
            this.Dispose();
          }
        }
        else
        {
          if (this.IsDisposed)
            return;
          int num = (int) Utils.Dialog((IWin32Window) this, text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          this.isDirty = this.lockForm.IsDirty = false;
          this.DialogResult = DialogResult.OK;
        }
        AlertConfig alertConfig = this.loanMgr.LoanData.Settings.AlertSetupData.GetAlertConfig(43);
        if (alertConfig == null || alertConfig.TriggerFieldList == null)
          return;
        LockRequestLog lastConfirmedLock = this.loanMgr.LoanData.GetLogList().GetLastConfirmedLock();
        if (lastConfirmedLock == null)
          return;
        Hashtable lockRequestSnapshot = lastConfirmedLock.GetLockRequestSnapshot();
        foreach (string triggerField in alertConfig.TriggerFieldList)
        {
          string field2 = this.loanMgr.LoanData.GetField(triggerField);
          if (!string.IsNullOrWhiteSpace(field2))
          {
            string str = (string) lockRequestSnapshot[(object) triggerField] ?? string.Empty;
            if (field2 != str)
              this.loanMgr.LoanData.AddLockField(lastConfirmedLock.Guid, triggerField, field2);
          }
        }
      }
      finally
      {
        RegulationAlerts.DisableKeyPricingAlerts = false;
        if (this.loanMgr.LoanData.GetField("4062") == "N")
          this.loanMgr.LoanData.SetField("4062", "Y");
      }
    }

    private void setFieldValue(string id, string val)
    {
      if (this.dataTables.ContainsKey((object) id))
        this.dataTables[(object) id] = (object) val;
      else
        this.dataTables.Add((object) id, (object) val);
    }

    private string getFieldValue(string id, Hashtable t)
    {
      return t.ContainsKey((object) id) ? t[(object) id].ToString() : string.Empty;
    }

    private void btnDeny_Click(object sender, EventArgs e)
    {
      if (!this.ValidateAction(sender) || this.ruleChecker.HasPrerequiredFields(this.session.LoanData, "BUTTON_DENY LOCK", true, this.lockForm.ReadFieldValues()))
        return;
      using (DenialCommentsForm denialCommentsForm = new DenialCommentsForm())
      {
        if (denialCommentsForm.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.denialComments = denialCommentsForm.Comments;
      }
      TriggerImplDef def = this.session.LoanDataMgr.ApplyLoanTemplateTrigger(TriggerConditionType.LockDenied);
      if (def != null)
      {
        this.showApplyLoanTemplateProgress();
        this.session.LoanDataMgr.ApplyLoanTemplate(def);
        this.closeProgress();
      }
      this.lockConfirm = false;
      this.lockDeny = true;
      if (this.DenyButtonClicked != null)
      {
        this.DenyButtonClicked(sender, e);
        Application.DoEvents();
        Application.DoEvents();
        this.isDirty = this.lockForm.IsDirty = false;
        this.Close();
        this.Dispose();
      }
      else
      {
        this.isDirty = this.lockForm.IsDirty = false;
        this.DialogResult = DialogResult.OK;
      }
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      if (!this.validateCancelRequest())
        return;
      this.resetDelayedCompiledTrigger(false);
      if (this.CancelButtonClicked != null)
      {
        this.CancelButtonClicked(sender, e);
        Application.DoEvents();
        Application.DoEvents();
      }
      this.isDirty = false;
      if (this.lockForm != null)
        this.lockForm.IsDirty = false;
      this.Close();
      this.Dispose();
    }

    private void resetDelayedCompiledTrigger(bool isRevised)
    {
      foreach (DelayedTrigger activatedTrigger in this.session.LoanDataMgr.Triggers.GetDelayActivatedTriggers())
      {
        if (activatedTrigger is DelayedCompiledTrigger)
        {
          DelayedCompiledTrigger delayedCompiledTrigger = (DelayedCompiledTrigger) activatedTrigger;
          if (delayedCompiledTrigger.TriggerEvent.Action.ActionType == TriggerActionType.Email)
          {
            if (isRevised)
              delayedCompiledTrigger.ResetFieldValue("LOCKRATE.REQUESTSTATUS", "Revised");
            else
              delayedCompiledTrigger.Reset();
          }
        }
      }
    }

    private void BuySellForm_Load(object sender, EventArgs e)
    {
      Tracing.Log(BuySellForm.sw, TraceLevel.Verbose, nameof (BuySellForm), "Load");
      this.session.LoanDataMgr.LoanClosing += new EventHandler(this.btnCancel_Click);
    }

    private bool validateCancelRequest()
    {
      if (this.isDirty || this.lockForm != null && this.lockForm.IsDirty)
      {
        string text = "If you cancel, all the changes will be lost. Do you want to cancel?";
        if (this.btnCancel.Text == "&Close")
          text = "If you close, all the changes will be lost. Do you want to close?";
        if (Utils.Dialog((IWin32Window) this, text, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
          return false;
      }
      return true;
    }

    private void BuySellForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      Tracing.Log(BuySellForm.sw, TraceLevel.Verbose, nameof (BuySellForm), "Closing");
      if (!this.validateCancelRequest())
        e.Cancel = true;
      else
        this.ReleaseEventsAndInstance();
    }

    private void ReleaseEventsAndInstance()
    {
      if (this.session.LoanDataMgr != null)
        this.session.LoanDataMgr.LoanClosing -= new EventHandler(this.btnCancel_Click);
      if (this.session.LoanData != null)
        this.session.LoanData.UnregisterFieldValueChangeEventHandler("OPTIMAL.RESPONSE", this.fx);
      BuySellForm._instance = (BuySellForm) null;
    }

    private void btnSellSidePricing_Click(object sender, EventArgs e)
    {
      ServiceSetupEvaluatorResponse svcSetupResponse = (ServiceSetupEvaluatorResponse) null;
      bool flag = this.session.ConfigurationManager.GetCompanySetting("POLICIES", "AllowPPESelection") == "True";
      if (flag && sender != this.btnLockConfirm)
      {
        if (!this.SelectPricingProvider(ref svcSetupResponse))
          return;
      }
      else if (!this.GetProductPricingProvider(ref svcSetupResponse))
        return;
      ProductPricingSetting productPricingPartner = this.session.StartupInfo.ProductPricingPartner;
      this.triggerAllCalculations();
      this.WindowState = FormWindowState.Minimized;
      this.loanMgr.LoanData.SetField("OPTIMAL.REQUEST", "");
      this.loanMgr.LoanData.SetField("OPTIMAL.RESPONSE", "");
      this.loanMgr.LoanData.SetField("OPTIMAL.REQUEST", this.buildRequestData());
      IEPass service = Session.Application.GetService<IEPass>();
      if (sender == this.btnLockConfirm)
        service.ProcessURL("_EPASS_SIGNATURE;" + ProductPricingUtils.GetPartnerId(productPricingPartner) + ";;GetSellSidePricing_SEC;LockAndConfirm");
      else if (productPricingPartner.VendorPlatform == VendorPlatform.EPC2)
      {
        Epc2ServiceClient epc2ServiceClient = new Epc2ServiceClient();
        string source = "urn:elli:services:form:secondarylock:getsellsidepricing";
        if (!this.isNewLock)
          source = source + ":lockid:" + this.origLockRequestGuid;
        string url = epc2ServiceClient.ComposeEPassPayloadAndUrlForEPC2(source, productPricingPartner, svcSetupResponse);
        if (url == null)
          return;
        LockRequestLog recentLockRequest = this.loanMgr.LoanData.GetLogList()?.GetMostRecentLockRequest();
        service.ProcessURL(url);
        this.RefreshAfterGetPricing(recentLockRequest?.Guid);
      }
      else
        service.ProcessURL("_EPASS_SIGNATURE;" + ProductPricingUtils.GetPartnerId(productPricingPartner) + ";;GetSellSidePricing_SEC;SOURCE_FORM=GetSellSidePricing_SEC");
      if (!flag || sender == this.btnLockConfirm)
        return;
      this.session.StartupInfo.ProductPricingPartner = this.session.ConfigurationManager.GetActiveProductPricingPartner();
    }

    private void btnBuySidePricing_Click(object sender, EventArgs e)
    {
      ServiceSetupEvaluatorResponse svcSetupResponse = (ServiceSetupEvaluatorResponse) null;
      bool flag = this.session.ConfigurationManager.GetCompanySetting("POLICIES", "AllowPPESelection") == "True";
      if (flag)
      {
        if (!this.SelectPricingProvider(ref svcSetupResponse))
          return;
      }
      else if (!this.GetProductPricingProvider(ref svcSetupResponse))
        return;
      ProductPricingSetting productPricingPartner = this.session.StartupInfo.ProductPricingPartner;
      this.triggerAllCalculations();
      this.WindowState = FormWindowState.Minimized;
      this.loanMgr.LoanData.SetField("OPTIMAL.REQUEST", "");
      this.loanMgr.LoanData.SetField("OPTIMAL.RESPONSE", "");
      this.loanMgr.LoanData.SetField("OPTIMAL.REQUEST", this.buildRequestData(true));
      if (productPricingPartner.VendorPlatform == VendorPlatform.EPC2)
      {
        Epc2ServiceClient epc2ServiceClient = new Epc2ServiceClient();
        string source = "urn:elli:services:form:secondarylock:getbuysidepricing";
        if (!this.isNewLock)
          source = source + ":lockid:" + this.origLockRequestGuid;
        string url = epc2ServiceClient.ComposeEPassPayloadAndUrlForEPC2(source, productPricingPartner, svcSetupResponse);
        if (url == null)
          return;
        LockRequestLog recentLockRequest = this.loanMgr.LoanData.GetLogList()?.GetMostRecentLockRequest();
        Session.Application.GetService<IEPass>().ProcessURL(url);
        this.RefreshAfterGetPricing(recentLockRequest?.Guid);
      }
      else
        Session.Application.GetService<IEPass>().ProcessURL("_EPASS_SIGNATURE;" + ProductPricingUtils.GetPartnerId(productPricingPartner) + ";;GetPricing_SEC;SOURCE_FORM=GetBuySidePricing_SEC");
      if (!flag)
        return;
      this.session.StartupInfo.ProductPricingPartner = this.session.ConfigurationManager.GetActiveProductPricingPartner();
    }

    private void RefreshAfterGetPricing(string prevGuid)
    {
      LogList logList = this.loanMgr.LoanData.GetLogList();
      LockRequestLog recentLockRequest = logList?.GetMostRecentLockRequest();
      if (recentLockRequest != null)
      {
        string guid = recentLockRequest?.Guid;
        LockRequestLog sender = string.Compare(prevGuid, guid, true) != 0 || string.Compare(guid, this.origLockRequestGuid, true) == 0 ? recentLockRequest : logList.GetLockRequest(this.origLockRequestGuid);
        if (sender != null)
        {
          this.ReleaseEventsAndInstance();
          this.Close();
          this.GetPricingCompleted((object) sender, (EventArgs) null);
        }
      }
      this.WindowState = FormWindowState.Maximized;
    }

    private bool GetProductPricingProvider(ref ServiceSetupEvaluatorResponse svcSetupResponse)
    {
      bool productPricingProvider = false;
      if (this.session.StartupInfo.ProductPricingPartner == null)
      {
        if (this.session.UserInfo.IsTopLevelAdministrator())
        {
          ProductPricingLightAdmin pricingLightAdmin = new ProductPricingLightAdmin(this.session);
          if (pricingLightAdmin.ShowDialog((IWin32Window) this.session.Application) == DialogResult.OK)
          {
            this.session.StartupInfo.ProductPricingPartner = this.session.ConfigurationManager.GetActiveProductPricingPartner();
            if (this.session.StartupInfo.ProductPricingPartner != null)
            {
              svcSetupResponse = pricingLightAdmin.svcSetupResponse;
              productPricingProvider = true;
            }
          }
        }
      }
      else if (this.session.StartupInfo.ProductPricingPartner.VendorPlatform == VendorPlatform.EPC2)
      {
        string accessToken = new Bam((LoanData) null).GetAccessToken("sc");
        Task<ServiceSetupEvaluatorResponse> task = Task.Run<ServiceSetupEvaluatorResponse>((Func<Task<ServiceSetupEvaluatorResponse>>) (async () => await Epc2ServiceClient.GetServiceSetupEvaluatorResponse(this.session.SessionObjects, accessToken, this.session.LoanData.GUID, this.session.StartupInfo.ProductPricingPartner.ProviderID)));
        svcSetupResponse = task.Result;
        ServiceSetupEvaluatorResponse evaluatorResponse = svcSetupResponse;
        int num1;
        if (evaluatorResponse == null)
        {
          num1 = 0;
        }
        else
        {
          int? count = evaluatorResponse.MatchingResults?.Count;
          int num2 = 0;
          num1 = count.GetValueOrDefault() > num2 & count.HasValue ? 1 : 0;
        }
        if (num1 != 0)
        {
          productPricingProvider = true;
        }
        else
        {
          int num3 = (int) Utils.Dialog((IWin32Window) null, string.Format("Please contact your administrator for \"{0}\" access.", (object) this.session.StartupInfo.ProductPricingPartner.PartnerName), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return productPricingProvider;
        }
      }
      else
        productPricingProvider = true;
      if (!productPricingProvider)
      {
        int num = (int) Utils.Dialog((IWin32Window) null, "The Product and Pricing partner has not been selected by your administrator." + Environment.NewLine + "If no partner has been selected by your Encompass administrator, this feature will not be available.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      return productPricingProvider;
    }

    private bool SelectPricingProvider(ref ServiceSetupEvaluatorResponse svcSetupResponse)
    {
      bool flag = false;
      ProductPricingLightAdmin pricingLightAdmin = new ProductPricingLightAdmin(this.session, true);
      if (pricingLightAdmin.ShowDialog((IWin32Window) this.session.Application) == DialogResult.OK)
      {
        ProductPricingSetting productPricingSetting = pricingLightAdmin.settings.SingleOrDefault<ProductPricingSetting>((Func<ProductPricingSetting, bool>) (s => s.Active));
        if (productPricingSetting != null)
        {
          this.session.StartupInfo.ProductPricingPartner = productPricingSetting;
          svcSetupResponse = pricingLightAdmin.svcSetupResponse;
          flag = true;
        }
      }
      return flag;
    }

    private void populateOptimalblueResult(string id, string val)
    {
      if (BuySellForm.Instance == null || !(val != ""))
        return;
      BuySellForm.Instance.RefreshPage();
      BuySellForm.Instance.WindowState = FormWindowState.Normal;
      BuySellForm.Instance.BringToFront();
    }

    private string buildRequestData(bool needHistoricalPricingCheck = false)
    {
      ValuePairXmlWriter valuePairXmlWriter = new ValuePairXmlWriter("FieldID", "FieldValue");
      for (int index = 0; index < LockRequestLog.BuySideFields.Count; ++index)
      {
        string buySideField = LockRequestLog.BuySideFields[index];
        string fieldValue = this.getFieldValue(buySideField, this.dataTables);
        if (fieldValue != string.Empty && fieldValue != "//")
          valuePairXmlWriter.Write(buySideField, fieldValue);
      }
      for (int index = 0; index < LockRequestLog.SellSideFields.Count; ++index)
      {
        string sellSideField = LockRequestLog.SellSideFields[index];
        string fieldValue = this.getFieldValue(sellSideField, this.dataTables);
        if (fieldValue != string.Empty && fieldValue != "//")
          valuePairXmlWriter.Write(sellSideField, fieldValue);
      }
      for (int index = 0; index < LockRequestLog.RequestFields.Count; ++index)
      {
        string requestField = LockRequestLog.RequestFields[index];
        string fieldValue = this.getFieldValue(requestField, this.dataTables);
        if (fieldValue != string.Empty && fieldValue != "//")
          valuePairXmlWriter.Write(requestField, fieldValue);
      }
      if (!this.dataTables.Contains((object) "4070") || this.dataTables.Contains((object) "4070") && (this.getFieldValue("4070", this.dataTables) == string.Empty || this.getFieldValue("4070", this.dataTables) == "//"))
        valuePairXmlWriter.Write("4070", "");
      if (!this.dataTables.Contains((object) "4058") || this.dataTables.Contains((object) "4058") && (this.getFieldValue("4058", this.dataTables) == string.Empty || this.getFieldValue("4058", this.dataTables) == "//"))
        valuePairXmlWriter.Write("4058", "");
      valuePairXmlWriter.Write("SnapShotGuid", this.lockLog.Guid);
      valuePairXmlWriter.Write("ReturnPage", nameof (BuySellForm));
      string fieldValue1 = this.getFieldValue("OPTIMAL.HISTORY", this.dataTables);
      if (needHistoricalPricingCheck)
      {
        string val = "False";
        if (LockUtils.IsWorstCaseHistoricalPricingTrans(fieldValue1) || this.lockLog.IsRelock)
          val = "True";
        valuePairXmlWriter.Write("IsHistoricalPricing", val);
      }
      if (fieldValue1 != string.Empty)
      {
        valuePairXmlWriter.Write("History", fieldValue1);
        valuePairXmlWriter.Write("OPTIMAL.HISTORY", fieldValue1);
      }
      string xml = valuePairXmlWriter.ToXML();
      Tracing.Log(BuySellForm.sw, TraceLevel.Verbose, nameof (BuySellForm), "Optimal Blue Request String = " + xml);
      return xml;
    }

    private void btnRevise_Click(object sender, EventArgs e)
    {
      if (this.ruleChecker.HasPrerequiredFields(this.session.LoanData, "BUTTON_REVISE LOCK", true, this.lockForm.ReadFieldValues()))
        return;
      bool boolean1 = Utils.ParseBoolean(Session.SessionObjects.StartupInfo.PolicySettings[(object) "Policies.EnableZeroParPricingRetail"]);
      bool boolean2 = Utils.ParseBoolean(Session.SessionObjects.StartupInfo.PolicySettings[(object) "Policies.EnableZeroParPricingWholesale"]);
      LockConfirmLog confirmForCurrentLock = Session.LoanData.GetLogList().GetMostRecentConfirmForCurrentLock();
      if (confirmForCurrentLock != null)
      {
        bool parPricingRetail = confirmForCurrentLock.EnableZeroParPricingRetail;
        bool pricingWholesale = confirmForCurrentLock.EnableZeroParPricingWholesale;
        if (this.loanMgr.LoanData != null && (this.loanMgr.LoanData.GetField("2626") == "Banked - Retail" && boolean1 != parPricingRetail || this.loanMgr.LoanData.GetField("2626") == "Banked - Wholesale" && boolean2 != pricingWholesale))
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Please note that the current Zero Based Par Pricing setting has been updated and the Buy Side Lock and Pricing adjusted buy price and adjustments should be updated accordingly.");
        }
      }
      this.ProcessEmn(e);
      ProductPricingSetting productPricingPartner = this.session.StartupInfo.ProductPricingPartner;
      if (LockUtils.IfShipDark(this.session.SessionObjects, "EPPS_EPC2_SHIP_DARK_SR") || productPricingPartner == null || productPricingPartner.VendorPlatform != VendorPlatform.EPC2)
        return;
      this.lockForm.DisableBuySideLockandPricingFields();
    }

    private void ProcessEmn(EventArgs e)
    {
      Hashtable lockRequestSnapshot = this.lockLog.GetLockRequestSnapshot();
      string str = (string) lockRequestSnapshot[(object) "2151"];
      LockRequestLog sender = this.prepRevisedSnapshot(lockRequestSnapshot, RateLockAction.Revise);
      this.btnRevise.Visible = false;
      this.btnCancel.Text = "&Cancel";
      this.btnOK.Visible = true;
      this.btnOK.Enabled = false;
      this.btnLockConfirm.Visible = true;
      this.btnLockConfirm.Enabled = true;
      this.btnDeny.Visible = true;
      this.btnDeny.Enabled = false;
      this.btnBuySidePricing.Enabled = true;
      this.btnValidate.Visible = false;
      this.resetDelayedCompiledTrigger(true);
      if (this.ReviseButtonClicked != null)
      {
        this.ReviseButtonClicked((object) sender, e);
        Application.DoEvents();
        Application.DoEvents();
      }
      if (!(str != (string) this.dataTables[(object) "2151"]))
        return;
      int num = (int) Utils.Dialog((IWin32Window) this, "Please note that the Buy Side Lock and Pricing, Lock Expiration Date \"" + str + "\" has been updated based on the current Lock Expiration Date Settings. This new expiration date will only be effective after the lock request is Locked or Locked and Confirmed.");
    }

    private void BuySellForm_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar != '\u001B')
        return;
      this.Close();
    }

    private void btnDataCompare_Click(object sender, EventArgs e)
    {
      using (LockSnapshotCompareForm snapshotCompareForm = new LockSnapshotCompareForm(this.loanMgr, this.dataTables, false, false, this.isNewLock))
      {
        if (snapshotCompareForm.LoanValueIsChanged)
        {
          if (snapshotCompareForm.ShowDialog((IWin32Window) this) != DialogResult.OK)
            return;
          this.isDirty = true;
          this.requestSnapshotForm.RefreshSnapshotForm(this.dataTables, this.loanMgr.LoanData);
          this.snapshotForm.RefreshSnapshotForm(this.dataTables, this.loanMgr.LoanData);
          this.lockForm.RefreshScreen(this.dataTables, this.lockLog);
          this.currentLockForm.RefreshScreen(this.dataTables, this.lockLog, this.lockForm.Height);
          int num = (int) Utils.Dialog((IWin32Window) this, "The lock has been successfully updated.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        else
        {
          int num1 = (int) Utils.Dialog((IWin32Window) this, "There is no difference between the current loan data and the lock request data.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
      }
    }

    private void btnDenyLockExtension_Click(object sender, EventArgs e)
    {
      if (!this.ValidateAction(sender))
        return;
      this.triggerAllCalculations();
      if (this.ruleChecker.HasPrerequiredFields(this.session.LoanData, "BUTTON_DENY EXTENSION", true, this.lockForm.ReadFieldValues()))
        return;
      using (DenialCommentsForm denialCommentsForm = new DenialCommentsForm())
      {
        if (denialCommentsForm.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.denialComments = denialCommentsForm.Comments;
      }
      this.lockConfirm = false;
      this.lockDeny = true;
      if (this.DenyButtonClicked != null)
      {
        this.DenyButtonClicked(sender, e);
        Application.DoEvents();
        Application.DoEvents();
        this.isDirty = this.lockForm.IsDirty = false;
        this.Close();
        this.Dispose();
      }
      else
      {
        this.isDirty = this.lockForm.IsDirty = false;
        this.DialogResult = DialogResult.OK;
      }
    }

    private void btnAcceptLockCancellation_Click(object sender, EventArgs e)
    {
      this.lockCancel = true;
      this.lockAndConfirm(false, true);
    }

    private void radViewSummary_CheckedChanged(object sender, EventArgs e)
    {
      this.radViewDetail.Checked = !this.radViewSummary.Checked;
      if (this.isSummary != this.radViewSummary.Checked)
        this.isSummary = this.radViewSummary.Checked;
      this.radViewDetail.Checked = !this.radViewSummary.Checked;
      this.RefreshView();
    }

    private void RefreshView()
    {
      this.lockForm.IsSummary = this.currentLockForm.IsSummary = this.isSummary = this.radViewSummary.Checked;
      this.lockForm.Top = this.currentLockForm.Top;
      this.lockForm.Left = this.currentLockForm.Width - 1;
      this.lockForm.RefreshScreen(this.dataTables, this.lockLog);
      this.currentLockForm.RefreshScreen(this.dataTables, this.lockLog, this.lockForm.Height);
      this.tabBuySell.Controls.Clear();
      this.tabBuySell.Controls.Add((Control) this.ViewPanel);
      this.tabBuySell.Controls.Add((Control) this.currentLockForm);
      this.tabBuySell.Controls.Add((Control) this.lockForm);
      this.lockForm.BringToFront();
    }

    private LockRequestLog prepRevisedSnapshot(Hashtable currentData, RateLockAction action)
    {
      LockRequestLog lockRequestLog = new LockRequestLog(this.loanMgr.LoanData.GetLogList());
      lockRequestLog.Date = this.session.SessionObjects.Session.ServerTime;
      lockRequestLog.SetRequestingUser(this.session.UserInfo.Userid, this.session.UserInfo.FullName);
      lockRequestLog.IsLockExtension = this.lockLog.IsLockExtension;
      lockRequestLog.IsRelock = this.lockLog.IsRelock;
      lockRequestLog.ParentLockGuid = this.lockLog.Guid;
      lockRequestLog.ReLockSequenceNumberForInactiveLock = LockUtils.GetReLockSequenceNumberForInactiveLock(this.loanMgr.LoanData);
      lockRequestLog.RateLockAction = action;
      if (action == RateLockAction.UpdateSell)
        lockRequestLog.LockRequestStatus = this.lockLog.LockRequestStatus;
      if (this.lockLog != null)
      {
        if (this.lockLog.ReviseAction != RateLockAction.UnKnown)
          lockRequestLog.ReviseAction = this.lockLog.ReviseAction;
        else if (this.lockLog.RateLockAction != RateLockAction.UnKnown)
          lockRequestLog.ReviseAction = this.lockLog.RateLockAction;
      }
      this.tabBuySell.Controls.Clear();
      this.tabBuySell.Controls.Add((Control) this.ViewPanel);
      this.InitForm(this.loanMgr, lockRequestLog, (LockRequestLog) null, true, false, action == RateLockAction.UpdateSell);
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      this.currentLockForm.RefreshScreen(currentData, lockRequestLog, this.lockForm.Height);
      for (int index = 0; index < LockRequestLog.SnapshotFields.Count; ++index)
      {
        string snapshotField = LockRequestLog.SnapshotFields[index];
        if (currentData.ContainsKey((object) snapshotField))
          this.setFieldValue(snapshotField, currentData[(object) snapshotField].ToString());
      }
      if (this.lrFields == null)
        this.lrFields = this.session.ConfigurationManager.GetLRAdditionalFields();
      foreach (string field in this.lrFields.GetFields(true))
      {
        string customFieldId = LockRequestCustomField.GenerateCustomFieldID(field);
        if (currentData.ContainsKey((object) customFieldId))
          this.setFieldValue(customFieldId, currentData[(object) customFieldId].ToString());
      }
      foreach (string field in this.lrFields.GetFields(false))
      {
        if (currentData.ContainsKey((object) field))
          this.setFieldValue(field, currentData[(object) field].ToString());
      }
      this.snapshotForm.RefreshSnapshotForm(currentData, this.loanMgr.LoanData);
      this.requestSnapshotForm.RefreshSnapshotForm(currentData, this.loanMgr.LoanData);
      for (int index = 0; index < LockRequestLog.LoanInfoSnapshotFields.Count; ++index)
      {
        string infoSnapshotField = LockRequestLog.LoanInfoSnapshotFields[index];
        if (currentData.ContainsKey((object) infoSnapshotField))
          this.setFieldValue(infoSnapshotField, currentData[(object) infoSnapshotField].ToString());
      }
      this.setFieldValue("OPTIMAL.HISTORY", currentData.ContainsKey((object) "OPTIMAL.HISTORY") ? currentData[(object) "OPTIMAL.HISTORY"].ToString() : "");
      if (lockRequestLog.IsLockExtension)
      {
        for (int index = 0; index < LockRequestLog.LockExtensionFields.Count; ++index)
        {
          string lockExtensionField = LockRequestLog.LockExtensionFields[index];
          if (currentData.ContainsKey((object) lockExtensionField))
            this.setFieldValue(lockExtensionField, currentData[(object) lockExtensionField].ToString());
        }
        if (currentData.ContainsKey((object) "3431"))
          this.setFieldValue("3431", currentData[(object) "3431"].ToString());
      }
      this.lockForm.RefreshScreen(this.dataTables, lockRequestLog);
      return lockRequestLog;
    }

    private void btnValidate_Click(object sender, EventArgs e)
    {
      if (this.ruleChecker.HasPrerequiredFields(this.session.LoanData, "BUTTON_VALIDATE", true, this.lockForm.ReadFieldValues()))
        return;
      ProductPricingSetting productPricingPartner = this.session.StartupInfo.ProductPricingPartner;
      if (productPricingPartner == null || !productPricingPartner.IsEPPS)
        return;
      this.triggerAllCalculations();
      LockUtils.RefreshLockRequestMapFieldsFromLoan(this.loanMgr);
      LockUtils.SyncAdditionalLockFieldsToRequest(this.loanMgr);
      Hashtable lockRequestSnapshot = this.lockLog.GetLockRequestSnapshot();
      Hashtable currentData = this.loanMgr.LoanData.PrepareLockRequestData();
      if (lockRequestSnapshot.ContainsKey((object) "OPTIMAL.HISTORY"))
        currentData[(object) "OPTIMAL.HISTORY"] = lockRequestSnapshot[(object) "OPTIMAL.HISTORY"];
      if (lockRequestSnapshot.ContainsKey((object) "4060"))
        currentData[(object) "4060"] = lockRequestSnapshot[(object) "4060"];
      if (lockRequestSnapshot.ContainsKey((object) "4069"))
        currentData[(object) "4069"] = lockRequestSnapshot[(object) "4069"];
      if (lockRequestSnapshot.ContainsKey((object) "4061"))
        currentData[(object) "4061"] = lockRequestSnapshot[(object) "4061"];
      if (this.lockLog.IsLockExtension)
      {
        for (int index = 0; index < LockRequestLog.LockExtensionFields.Count; ++index)
        {
          string lockExtensionField = LockRequestLog.LockExtensionFields[index];
          if (lockRequestSnapshot.ContainsKey((object) lockExtensionField))
            currentData[(object) lockExtensionField] = lockRequestSnapshot[(object) lockExtensionField];
        }
        if (lockRequestSnapshot.ContainsKey((object) "3431"))
          currentData[(object) "3431"] = lockRequestSnapshot[(object) "3431"];
      }
      LockRequestLog sender1 = this.prepRevisedSnapshot(currentData, RateLockAction.Validate);
      this.btnRevise.Visible = false;
      this.btnCancel.Text = "&Cancel";
      this.btnLockConfirm.Visible = true;
      this.btnLockConfirm.Enabled = true;
      this.btnDeny.Visible = true;
      this.btnDeny.Enabled = false;
      this.btnBuySidePricing.Enabled = false;
      this.btnSellSidePricing.Enabled = false;
      this.btnValidate.Enabled = false;
      this.resetDelayedCompiledTrigger(true);
      if (this.ValidateButtonClicked != null)
      {
        this.ValidateButtonClicked((object) sender1, e);
        Application.DoEvents();
        Application.DoEvents();
      }
      this.WindowState = FormWindowState.Minimized;
      this.loanMgr.LoanData.SetField("OPTIMAL.REQUEST", "");
      this.loanMgr.LoanData.SetField("OPTIMAL.RESPONSE", "");
      this.loanMgr.LoanData.SetField("OPTIMAL.REQUEST", this.buildValidateRequestData(sender1.Guid));
      if (!LockUtils.IfShipDark(this.session.SessionObjects, "EPPS_EPC2_SHIP_DARK_SR") && productPricingPartner.VendorPlatform == VendorPlatform.EPC2)
      {
        ServiceSetupEvaluatorResponse svcSetupResponse = (ServiceSetupEvaluatorResponse) null;
        if (!this.GetProductPricingProvider(ref svcSetupResponse))
          return;
        string url = new Epc2ServiceClient().ComposeEPassPayloadAndUrlForEPC2("urn:elli:services:form:secondarylock:validatelock:lockid:" + this.lockLog.ParentLockGuid, productPricingPartner, svcSetupResponse);
        if (url == null)
          return;
        Session.Application.GetService<IEPass>().ProcessURL(url);
      }
      else
        Session.Application.GetService<IEPass>().ProcessURL("_EPASS_SIGNATURE;" + ProductPricingUtils.GetPartnerId(productPricingPartner) + ";;ValidateLock;SOURCE_FORM=ValidateLock_SEC");
    }

    private string buildValidateRequestData(string newLockGuid)
    {
      ValuePairXmlWriter valuePairXmlWriter = new ValuePairXmlWriter("FieldID", "FieldValue");
      valuePairXmlWriter.Write("SnapShotGuid", newLockGuid);
      valuePairXmlWriter.Write("ReturnPage", nameof (BuySellForm));
      string fieldValue = this.getFieldValue("OPTIMAL.HISTORY", this.dataTables);
      if (fieldValue != string.Empty)
      {
        valuePairXmlWriter.Write("History", fieldValue);
        valuePairXmlWriter.Write("OPTIMAL.HISTORY", fieldValue);
      }
      return valuePairXmlWriter.ToXML();
    }

    private bool ValidateAction(object sender)
    {
      string str = string.Empty;
      string field = this.loanMgr.LoanData.GetField("3907");
      if (sender is Button button)
      {
        switch (button.Name)
        {
          case "btnDenyLockExtension":
            str = "before the lock extension can be denied";
            break;
          case "btnDeny":
            str = "before the lock can be denied";
            break;
        }
      }
      if (string.IsNullOrEmpty(field))
        return true;
      int num = (int) Utils.Dialog((IWin32Window) this, "This loan must be removed from the Correspondent Trade \"" + field + "\" " + str + ".", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      return false;
    }

    private void showApplyLoanTemplateProgress()
    {
      new Thread(new ParameterizedThreadStart(this.threadStart))
      {
        IsBackground = true
      }.Start((object) "Please wait. Applying loan template is in progress.");
    }

    private void threadStart(object message)
    {
      this.statusReport = new StatusReport(string.Concat(message));
      this.statusReport.Text = "Applying loan template";
      Application.Run((Form) this.statusReport);
    }

    private void closeProgress()
    {
      if (this.statusReport == null)
        return;
      if (this.statusReport.InvokeRequired)
      {
        this.statusReport.Invoke((Delegate) new MethodInvoker(this.closeProgress));
      }
      else
      {
        try
        {
          this.statusReport.Close();
        }
        catch
        {
        }
      }
    }

    private void triggerAllCalculations()
    {
      if (this.loanMgr == null || this.loanMgr.LoanData == null)
        return;
      if (this.loanMgr.LoanData.Calculator == null)
        return;
      try
      {
        this.loanMgr.LoanData.Calculator.CalcOnDemand();
        bool skipLockRequestSync = this.loanMgr.LoanData.Calculator.SkipLockRequestSync;
        this.loanMgr.LoanData.Calculator.SkipLockRequestSync = true;
        this.loanMgr.LoanData.Calculator.CalculateAll(false);
        this.loanMgr.LoanData.Calculator.SkipLockRequestSync = skipLockRequestSync;
      }
      catch (Exception ex)
      {
      }
    }

    private void btnSaveProgress_Click(object sender, EventArgs e)
    {
      this.loanMgr.SaveProgressForPendingLockRequest(this.lockLog, this.dataTables, this.session.UserInfo);
      this.Close();
      this.Dispose();
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      if (this.SaveButtonClicked == null)
        return;
      this.SaveButtonClicked((object) this.btnSave, (EventArgs) null);
      Application.DoEvents();
      Application.DoEvents();
      this.Close();
      this.Dispose();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.panel1 = new Panel();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.btnBuySidePricing = new Button();
      this.btnSellSidePricing = new Button();
      this.btnValidate = new Button();
      this.flowLayoutPanel2 = new FlowLayoutPanel();
      this.btnLockConfirm = new Button();
      this.btnDeny = new Button();
      this.btnDenyLockExtension = new Button();
      this.btnOK = new Button();
      this.btnRevise = new Button();
      this.btnAcceptLockCancellation = new Button();
      this.btnSaveProgress = new Button();
      this.btnCancel = new Button();
      this.tabControlLock = new TabControl();
      this.tabBuySell = new TabPage();
      this.ViewPanel = new GradientPanel();
      this.radViewDetail = new RadioButton();
      this.radViewSummary = new RadioButton();
      this.label3 = new Label();
      this.tabSnapshot = new TabPage();
      this.tabPageRequest = new TabPage();
      this.panelTop = new Panel();
      this.btnDataCompare = new Button();
      this.btnSave = new Button();
      this.panel1.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      this.flowLayoutPanel2.SuspendLayout();
      this.tabControlLock.SuspendLayout();
      this.tabBuySell.SuspendLayout();
      this.ViewPanel.SuspendLayout();
      this.panelTop.SuspendLayout();
      this.SuspendLayout();
      this.panel1.Controls.Add((Control) this.flowLayoutPanel1);
      this.panel1.Controls.Add((Control) this.flowLayoutPanel2);
      this.panel1.Dock = DockStyle.Bottom;
      this.panel1.Location = new Point(0, 614);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(1024, 48);
      this.panel1.TabIndex = 0;
      this.flowLayoutPanel1.BackColor = Color.WhiteSmoke;
      this.flowLayoutPanel1.Controls.Add((Control) this.btnBuySidePricing);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnSellSidePricing);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnValidate);
      this.flowLayoutPanel1.Location = new Point(6, 10);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new Size(370, 30);
      this.flowLayoutPanel1.TabIndex = 8;
      this.btnBuySidePricing.BackColor = SystemColors.Control;
      this.btnBuySidePricing.Location = new Point(3, 3);
      this.btnBuySidePricing.Name = "btnBuySidePricing";
      this.btnBuySidePricing.Size = new Size(76, 23);
      this.btnBuySidePricing.TabIndex = 5;
      this.btnBuySidePricing.TabStop = false;
      this.btnBuySidePricing.Text = "Get Pricing";
      this.btnBuySidePricing.UseVisualStyleBackColor = true;
      this.btnBuySidePricing.Click += new EventHandler(this.btnBuySidePricing_Click);
      this.btnSellSidePricing.BackColor = SystemColors.Control;
      this.btnSellSidePricing.Location = new Point(85, 3);
      this.btnSellSidePricing.Name = "btnSellSidePricing";
      this.btnSellSidePricing.Size = new Size(117, 23);
      this.btnSellSidePricing.TabIndex = 7;
      this.btnSellSidePricing.TabStop = false;
      this.btnSellSidePricing.Text = "Get Sell Side Pricing";
      this.btnSellSidePricing.UseVisualStyleBackColor = true;
      this.btnSellSidePricing.Click += new EventHandler(this.btnSellSidePricing_Click);
      this.btnValidate.BackColor = SystemColors.Control;
      this.btnValidate.Location = new Point(208, 3);
      this.btnValidate.Name = "btnValidate";
      this.btnValidate.Size = new Size(117, 23);
      this.btnValidate.TabIndex = 8;
      this.btnValidate.TabStop = false;
      this.btnValidate.Text = "Validate";
      this.btnValidate.UseVisualStyleBackColor = true;
      this.btnValidate.Visible = false;
      this.btnValidate.Click += new EventHandler(this.btnValidate_Click);
      this.flowLayoutPanel2.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.flowLayoutPanel2.Controls.Add((Control) this.btnLockConfirm);
      this.flowLayoutPanel2.Controls.Add((Control) this.btnDeny);
      this.flowLayoutPanel2.Controls.Add((Control) this.btnDenyLockExtension);
      this.flowLayoutPanel2.Controls.Add((Control) this.btnOK);
      this.flowLayoutPanel2.Controls.Add((Control) this.btnRevise);
      this.flowLayoutPanel2.Controls.Add((Control) this.btnAcceptLockCancellation);
      this.flowLayoutPanel2.Controls.Add((Control) this.btnSaveProgress);
      this.flowLayoutPanel2.Controls.Add((Control) this.btnSave);
      this.flowLayoutPanel2.Controls.Add((Control) this.btnCancel);
      this.flowLayoutPanel2.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel2.Location = new Point(17, 10);
      this.flowLayoutPanel2.Name = "flowLayoutPanel2";
      this.flowLayoutPanel2.Size = new Size(1004, 30);
      this.flowLayoutPanel2.TabIndex = 9;
      this.btnLockConfirm.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnLockConfirm.BackColor = SystemColors.Control;
      this.btnLockConfirm.Location = new Point(885, 3);
      this.btnLockConfirm.Name = "btnLockConfirm";
      this.btnLockConfirm.Size = new Size(116, 23);
      this.btnLockConfirm.TabIndex = 2;
      this.btnLockConfirm.TabStop = false;
      this.btnLockConfirm.Text = "Lock and Confirm";
      this.btnLockConfirm.UseVisualStyleBackColor = true;
      this.btnLockConfirm.Click += new EventHandler(this.btnLockConfirm_Click);
      this.btnDeny.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnDeny.BackColor = SystemColors.Control;
      this.btnDeny.Location = new Point(804, 3);
      this.btnDeny.Name = "btnDeny";
      this.btnDeny.Size = new Size(75, 23);
      this.btnDeny.TabIndex = 3;
      this.btnDeny.TabStop = false;
      this.btnDeny.Text = "Deny Lock";
      this.btnDeny.UseVisualStyleBackColor = true;
      this.btnDeny.Click += new EventHandler(this.btnDeny_Click);
      this.btnDenyLockExtension.Location = new Point(707, 3);
      this.btnDenyLockExtension.Name = "btnDenyLockExtension";
      this.btnDenyLockExtension.Size = new Size(91, 23);
      this.btnDenyLockExtension.TabIndex = 7;
      this.btnDenyLockExtension.TabStop = false;
      this.btnDenyLockExtension.Text = "Deny Extension";
      this.btnDenyLockExtension.UseVisualStyleBackColor = true;
      this.btnDenyLockExtension.Click += new EventHandler(this.btnDenyLockExtension_Click);
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.BackColor = SystemColors.Control;
      this.btnOK.Location = new Point(556, 3);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(145, 23);
      this.btnOK.TabIndex = 1;
      this.btnOK.TabStop = false;
      this.btnOK.Text = "Update Sell / Comparison";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnRevise.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnRevise.BackColor = SystemColors.Control;
      this.btnRevise.Location = new Point(475, 3);
      this.btnRevise.Name = "btnRevise";
      this.btnRevise.Size = new Size(75, 23);
      this.btnRevise.TabIndex = 6;
      this.btnRevise.TabStop = false;
      this.btnRevise.Text = "Revise Lock";
      this.btnRevise.UseVisualStyleBackColor = true;
      this.btnRevise.Click += new EventHandler(this.btnRevise_Click);
      this.btnAcceptLockCancellation.Location = new Point(394, 3);
      this.btnAcceptLockCancellation.Name = "btnAcceptLockCancellation";
      this.btnAcceptLockCancellation.Size = new Size(75, 23);
      this.btnAcceptLockCancellation.TabIndex = 8;
      this.btnAcceptLockCancellation.TabStop = false;
      this.btnAcceptLockCancellation.Text = "Cancel Lock";
      this.btnAcceptLockCancellation.UseVisualStyleBackColor = true;
      this.btnAcceptLockCancellation.Click += new EventHandler(this.btnAcceptLockCancellation_Click);
      this.btnSaveProgress.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnSaveProgress.BackColor = SystemColors.Control;
      this.btnSaveProgress.DialogResult = DialogResult.Cancel;
      this.btnSaveProgress.Location = new Point(288, 3);
      this.btnSaveProgress.Name = "btnSaveProgress";
      this.btnSaveProgress.Size = new Size(100, 23);
      this.btnSaveProgress.TabIndex = 9;
      this.btnSaveProgress.TabStop = false;
      this.btnSaveProgress.Text = "Save Progress";
      this.btnSaveProgress.UseVisualStyleBackColor = true;
      this.btnSaveProgress.Visible = false;
      this.btnSaveProgress.Click += new EventHandler(this.btnSaveProgress_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.BackColor = SystemColors.Control;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(101, 3);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 0;
      this.btnCancel.TabStop = false;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.tabControlLock.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.tabControlLock.Controls.Add((Control) this.tabBuySell);
      this.tabControlLock.Controls.Add((Control) this.tabSnapshot);
      this.tabControlLock.Controls.Add((Control) this.tabPageRequest);
      this.tabControlLock.Location = new Point(3, 12);
      this.tabControlLock.Name = "tabControlLock";
      this.tabControlLock.Padding = new Point(10, 3);
      this.tabControlLock.SelectedIndex = 0;
      this.tabControlLock.Size = new Size(1017, 596);
      this.tabControlLock.TabIndex = 4;
      this.tabControlLock.TabStop = false;
      this.tabBuySell.AutoScroll = true;
      this.tabBuySell.Controls.Add((Control) this.ViewPanel);
      this.tabBuySell.Location = new Point(4, 22);
      this.tabBuySell.Name = "tabBuySell";
      this.tabBuySell.Padding = new Padding(2, 2, 2, 0);
      this.tabBuySell.Size = new Size(1009, 570);
      this.tabBuySell.TabIndex = 0;
      this.tabBuySell.Text = "Buy/Sell";
      this.tabBuySell.UseVisualStyleBackColor = true;
      this.ViewPanel.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.ViewPanel.Controls.Add((Control) this.radViewDetail);
      this.ViewPanel.Controls.Add((Control) this.radViewSummary);
      this.ViewPanel.Controls.Add((Control) this.label3);
      this.ViewPanel.Dock = DockStyle.Top;
      this.ViewPanel.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.ViewPanel.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.ViewPanel.Location = new Point(2, 2);
      this.ViewPanel.Name = "ViewPanel";
      this.ViewPanel.Size = new Size(1005, 31);
      this.ViewPanel.Style = GradientPanel.PanelStyle.PageSubHeader;
      this.ViewPanel.TabIndex = 2;
      this.radViewDetail.AutoSize = true;
      this.radViewDetail.BackColor = Color.Transparent;
      this.radViewDetail.Checked = true;
      this.radViewDetail.Location = new Point(123, 11);
      this.radViewDetail.Name = "radViewDetail";
      this.radViewDetail.Size = new Size(64, 17);
      this.radViewDetail.TabIndex = 6;
      this.radViewDetail.TabStop = true;
      this.radViewDetail.Text = "Detailed";
      this.radViewDetail.UseVisualStyleBackColor = true;
      this.radViewSummary.AutoSize = true;
      this.radViewSummary.BackColor = Color.Transparent;
      this.radViewSummary.Location = new Point(50, 11);
      this.radViewSummary.Name = "radViewSummary";
      this.radViewSummary.Size = new Size(68, 17);
      this.radViewSummary.TabIndex = 5;
      this.radViewSummary.Text = "Summary";
      this.radViewSummary.UseVisualStyleBackColor = false;
      this.radViewSummary.CheckedChanged += new EventHandler(this.radViewSummary_CheckedChanged);
      this.label3.AutoSize = true;
      this.label3.BackColor = Color.Transparent;
      this.label3.Location = new Point(11, 13);
      this.label3.Name = "label3";
      this.label3.Size = new Size(30, 13);
      this.label3.TabIndex = 4;
      this.label3.Text = "View";
      this.tabSnapshot.AutoScroll = true;
      this.tabSnapshot.Location = new Point(4, 22);
      this.tabSnapshot.Name = "tabSnapshot";
      this.tabSnapshot.Padding = new Padding(2, 2, 2, 0);
      this.tabSnapshot.Size = new Size(1009, 570);
      this.tabSnapshot.TabIndex = 1;
      this.tabSnapshot.Text = "Loan Snapshot";
      this.tabSnapshot.UseVisualStyleBackColor = true;
      this.tabPageRequest.AutoScroll = true;
      this.tabPageRequest.Location = new Point(4, 22);
      this.tabPageRequest.Name = "tabPageRequest";
      this.tabPageRequest.Padding = new Padding(2, 2, 2, 0);
      this.tabPageRequest.Size = new Size(1009, 570);
      this.tabPageRequest.TabIndex = 2;
      this.tabPageRequest.Text = "Lock Request Snapshot";
      this.tabPageRequest.UseVisualStyleBackColor = true;
      this.panelTop.Controls.Add((Control) this.btnDataCompare);
      this.panelTop.Controls.Add((Control) this.tabControlLock);
      this.panelTop.Dock = DockStyle.Fill;
      this.panelTop.Location = new Point(0, 0);
      this.panelTop.Name = "panelTop";
      this.panelTop.Size = new Size(1024, 614);
      this.panelTop.TabIndex = 27;
      this.btnDataCompare.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDataCompare.BackColor = SystemColors.Control;
      this.btnDataCompare.Location = new Point(840, 5);
      this.btnDataCompare.Name = "btnDataCompare";
      this.btnDataCompare.Size = new Size(178, 23);
      this.btnDataCompare.TabIndex = 9;
      this.btnDataCompare.TabStop = false;
      this.btnDataCompare.Text = "Compare with Current Loan Data";
      this.btnDataCompare.UseVisualStyleBackColor = true;
      this.btnDataCompare.Click += new EventHandler(this.btnDataCompare_Click);
      this.btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnSave.BackColor = SystemColors.Control;
      this.btnSave.DialogResult = DialogResult.Cancel;
      this.btnSave.Location = new Point(182, 3);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(100, 23);
      this.btnSave.TabIndex = 10;
      this.btnSave.TabStop = false;
      this.btnSave.Text = "Save";
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Visible = false;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.AutoScaleBaseSize = new Size(5, 13);
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(1024, 662);
      this.Controls.Add((Control) this.panelTop);
      this.Controls.Add((Control) this.panel1);
      this.KeyPreview = true;
      this.Name = nameof (BuySellForm);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Secondary Lock Tool";
      this.WindowState = FormWindowState.Maximized;
      this.FormClosing += new FormClosingEventHandler(this.BuySellForm_FormClosing);
      this.Load += new EventHandler(this.BuySellForm_Load);
      this.KeyPress += new KeyPressEventHandler(this.BuySellForm_KeyPress);
      this.panel1.ResumeLayout(false);
      this.flowLayoutPanel1.ResumeLayout(false);
      this.flowLayoutPanel2.ResumeLayout(false);
      this.tabControlLock.ResumeLayout(false);
      this.tabBuySell.ResumeLayout(false);
      this.ViewPanel.ResumeLayout(false);
      this.ViewPanel.PerformLayout();
      this.panelTop.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
