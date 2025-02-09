// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.DisclosureTracking2015WS
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.ePass.Messaging;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.InputEngine;
using EllieMae.EMLite.LoanUtils.Workflow;
using EllieMae.EMLite.Log.DisclosureTracking2015;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using EllieMae.Encompass.Forms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class DisclosureTracking2015WS : UserControl, IRefreshContents, IOnlineHelpTarget
  {
    private const string className = "DisclosureTracking2015WS";
    private static readonly string sw = Tracing.SwInputEngine;
    private const string AutomaticFullfillmentServiceName = "Encompass Fulfillment Service";
    private LoanData loanData;
    private bool suspendEvent;
    private IDisclosureTracking2015Log currentLog;
    private string[] discloseMethod = new string[8]
    {
      "U.S. Mail",
      "In Person",
      "Fax",
      "Other",
      "eFolder eDisclosures",
      "Email",
      "Closing Docs Order",
      "eClose"
    };
    private System.Windows.Forms.Control[] fieldControlList;
    private bool hasAccessRight = true;
    private BusinessRuleCheck ruleChecker = new BusinessRuleCheck();
    private PopupBusinessRules popupRules;
    private bool markInvalidButtonEnabledAllowedByRule = true;
    private bool markValidButtonEnabledAllowedByRule = true;
    private bool timeLineAccessFlag = true;
    private bool forLinkSync;
    private IContainer components;
    private GroupContainer groupContainer1;
    private System.Windows.Forms.Panel panel1;
    private GroupContainer gcCompliance;
    private Splitter splitter1;
    private Splitter splitter2;
    private System.Windows.Forms.Panel panel2;
    private GroupContainer gcHistory;
    private GridView gvHistory;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.Label label9;
    private System.Windows.Forms.Label label11;
    private System.Windows.Forms.Label label12;
    private System.Windows.Forms.Label label13;
    private DatePicker dpLEApp;
    private DatePicker dpClosingDate;
    private DatePicker dpFeeCollectDate;
    private System.Windows.Forms.Button btnMarkInvalid;
    private FlowLayoutPanel flowLayoutPanel1;
    private VerticalSeparator verticalSeparator1;
    private StandardIconButton btnAdd;
    private System.Windows.Forms.Button btnMarkValid;
    private DatePicker dpActClosingDate;
    private DatePicker dpCDRevisedReceived;
    private DatePicker dpCDRevisedSent;
    private DatePicker dpCDReceived;
    private DatePicker dpCDSent;
    private DatePicker dpLERevisedReceived;
    private DatePicker dpLERevisedSent;
    private DatePicker dpLEReceived;
    private DatePicker dpLESent;
    private DatePicker dpLEDue;
    private System.Windows.Forms.Label label2;
    private GroupContainer gcLE;
    private Splitter splitter3;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.Label label10;
    private FieldLockButton lBtnApplicationDate;
    private ToolTip toolTipField;
    private EMHelpLink emHelpLink1;
    private EMHelpLink emHelpLink2;
    private EMHelpLink emHelpLink3;
    private DatePicker dpIntentToProceed;
    private System.Windows.Forms.Label label24;
    private StandardIconButton btnZoomIn;
    private System.Windows.Forms.Label label17;
    private System.Windows.Forms.Label label16;
    private System.Windows.Forms.Label label15;
    private System.Windows.Forms.Label label14;
    private DatePicker dpHELOCBrochureDate;
    private DatePicker dpHUDSpecialDate;
    private DatePicker dpCHARMDate;
    private DatePicker dpAffiliatedDate;
    private Splitter splitter4;
    private Splitter splitter5;
    private GroupContainer gcCD;
    private GroupContainer groupContainer3;
    private DatePicker dpAppraisalProvided;
    private System.Windows.Forms.Label label18;
    private DatePicker dpAVM;
    private System.Windows.Forms.Label label20;
    private DatePicker dpSubAppraisal;
    private System.Windows.Forms.Label label19;
    private DatePicker dpHomeCounseling;
    private System.Windows.Forms.Label label21;
    private System.Windows.Forms.Label label22;
    private System.Windows.Forms.Label label23;
    private DatePicker dpPostConDisSent;
    private DatePicker dpPostConDisReceived;
    private DatePicker dpeSign;
    private System.Windows.Forms.Label label25;
    private StandardIconButton btnSelecteSign;
    private System.Windows.Forms.Label label26;
    private System.Windows.Forms.Label label27;
    private DatePicker dpSHSent;
    private DatePicker dpSSPLSent;
    private System.Windows.Forms.Label label28;
    private DatePicker dpHighCostDisclosure;
    private System.Windows.Forms.Button btnEsign;
    private System.Windows.Forms.TextBox txtTimezone;
    private System.Windows.Forms.Label lblTimezone;

    public bool IsLinkedLoan
    {
      get => this.loanData != null && this.loanData.LinkSyncType == LinkSyncType.ConstructionLinked;
    }

    public bool IsLinknSyncPrimaryLoan
    {
      get
      {
        return this.loanData != null && this.loanData.LinkSyncType == LinkSyncType.ConstructionPrimary;
      }
    }

    public DisclosureTracking2015WS(
      IDisclosureTracking2015Log selectedLog,
      bool clearNotification,
      Sessions.Session session)
    {
      this.InitializeComponent();
      if (Session.LoanData.LinkedData != null && Session.LoanData.LinkSyncType == LinkSyncType.ConstructionLinked)
        this.forLinkSync = true;
      this.loanData = Session.LoanDataMgr.LoanData;
      if (Session.StartupInfo.UserAclFeatureRights.Contains((object) AclFeature.ToolsTab_DT_ExcludeIncludeRecords))
        this.timeLineAccessFlag = (bool) Session.StartupInfo.UserAclFeatureRights[(object) AclFeature.ToolsTab_DT_ExcludeIncludeRecords];
      this.btnMarkInvalid.Enabled = this.btnMarkValid.Enabled = this.timeLineAccessFlag;
      this.btnEsign.Visible = Session.LoanDataMgr.IsPlatformLoan();
      try
      {
        this.popupRules = new PopupBusinessRules(this.loanData, (ResourceManager) null, (System.Drawing.Image) null, (System.Drawing.Image) null, session);
        if (this.timeLineAccessFlag)
        {
          this.popupRules.SetBusinessRules(this.btnMarkInvalid);
          this.markInvalidButtonEnabledAllowedByRule = this.btnMarkInvalid.Enabled;
          this.popupRules.SetBusinessRules(this.btnMarkValid);
          this.markValidButtonEnabledAllowedByRule = this.btnMarkValid.Enabled;
        }
        else
        {
          this.markInvalidButtonEnabledAllowedByRule = false;
          this.markValidButtonEnabledAllowedByRule = false;
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(DisclosureTracking2015WS.sw, TraceLevel.Error, nameof (DisclosureTracking2015WS), "Cannot set Button access right. Error: " + ex.Message);
      }
      this.applySecurity();
      this.setClosingCostExpirationFromDisclosureSettings();
      this.suspendEvent = true;
      this.initialControl();
      this.synchronizeeDisclosureTrackingInfo(clearNotification);
      this.gvHistory.Sort(0, SortOrder.Descending);
      if (selectedLog == null)
        selectedLog = this.loanData.GetLogList().GetLatestIDisclosureTracking2015Log(DisclosureTracking2015Log.DisclosureTrackingType.All);
      this.loadDisclosureLog(selectedLog);
      this.reloadTimeline();
      this.suspendEvent = false;
      if (selectedLog == null && this.gvHistory.Items.Count > 0)
        this.gvHistory.Items[0].Selected = true;
      if (this.loanData.GetBorrowerPairs().Length == 0)
        this.btnSelecteSign.Enabled = false;
      this.Dock = DockStyle.Fill;
      this.DisableControlsForLinkedLoans();
    }

    private void setClosingCostExpirationFromDisclosureSettings()
    {
      if (this.loanData.IsLocked("LE1.X9") || ((IEnumerable<IDisclosureTracking2015Log>) this.loanData.GetLogList().GetAllIDisclosureTracking2015Log(false)).Any<IDisclosureTracking2015Log>())
        return;
      Session.LoanDataMgr.SetDefaultValuesForClosingCostExpiration();
    }

    private void DisableControlsForLinkedLoans()
    {
      if (!this.IsLinkedLoan)
        return;
      this.btnAdd.Enabled = false;
      this.btnMarkInvalid.Enabled = false;
      this.btnMarkValid.Enabled = false;
      this.lBtnApplicationDate.Enabled = false;
      this.dpLEApp.Enabled = false;
      foreach (System.Windows.Forms.Control control in (ArrangedElementCollection) this.groupContainer3.Controls)
      {
        switch (control)
        {
          case ScrollBar _:
          case System.Windows.Forms.Label _:
            continue;
          default:
            control.Enabled = false;
            continue;
        }
      }
      this.dpActClosingDate.Enabled = false;
    }

    public DisclosureTracking2015WS(
      string packageID,
      bool clearNotification,
      Sessions.Session session)
      : this(DisclosureTracking2015WS.getLogFromPackageID(packageID, Session.LoanData), clearNotification, session)
    {
    }

    private static IDisclosureTracking2015Log getLogFromPackageID(
      string packageID,
      LoanData loanData)
    {
      foreach (IDisclosureTracking2015Log logFromPackageId in loanData.GetLogList().GetAllIDisclosureTracking2015Log(false))
      {
        if (logFromPackageId.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.eDisclosure && logFromPackageId.eDisclosurePackageID == packageID)
          return logFromPackageId;
      }
      return (IDisclosureTracking2015Log) null;
    }

    public DisclosureTracking2015WS(Sessions.Session session)
      : this("-99999", true, session)
    {
    }

    private void synchronizeeDisclosureTrackingInfo(bool clearNotification)
    {
      if (!(Session.LoanDataMgr.SyncAllEDisclosurePackageStatuses(clearNotification) & clearNotification))
        return;
      EPassMessages.SyncReadMessages(true);
    }

    private void applySecurity()
    {
      if (Session.LoanData.GetField("3969") == "Old GFE and HUD-1" || Session.LoanData.GetField("3969") == "RESPA 2010 GFE and HUD-1")
      {
        this.hasAccessRight = false;
      }
      else
      {
        if (Session.LoanDataMgr.GetEffectiveRight(Session.UserID) == LoanInfo.Right.NoRight)
          this.hasAccessRight = false;
        this.hasAccessRight = (Session.LoanData.ContentAccess & LoanContentAccess.DisclosureTracking) == LoanContentAccess.DisclosureTracking;
      }
      if (!this.hasAccessRight)
        this.flowLayoutPanel1.Visible = false;
      else
        this.flowLayoutPanel1.Visible = true;
    }

    private void loadDisclosureLog(IDisclosureTracking2015Log selectedLog)
    {
      this.cleanHistoryDetail();
      this.gvHistory.Items.Clear();
      IDisclosureTracking2015Log[] idisclosureTracking2015Log = this.loanData.GetLogList().GetAllIDisclosureTracking2015Log(false);
      int nItemIndex = -1;
      foreach (IDisclosureTracking2015Log disclosureTracking2015Log in idisclosureTracking2015Log)
      {
        if (selectedLog != null && disclosureTracking2015Log.Guid == selectedLog.Guid)
          nItemIndex = this.gvHistory.Items.Count;
        GVItem gvItem = new GVItem();
        DateTime dateTime;
        if (disclosureTracking2015Log.IsLocked)
        {
          GVSubItemCollection subItems = gvItem.SubItems;
          dateTime = disclosureTracking2015Log.LockedDisclosedDateField;
          string str = dateTime.ToString("MM/dd/yyyy");
          subItems.Add((object) str);
        }
        else
        {
          GVSubItemCollection subItems = gvItem.SubItems;
          dateTime = disclosureTracking2015Log.OriginalDisclosedDate;
          string str = dateTime.ToString("MM/dd/yyyy hh:mm:ss tt");
          subItems.Add((object) str);
        }
        switch (disclosureTracking2015Log.DisclosureMethod)
        {
          case DisclosureTrackingBase.DisclosedMethod.ByMail:
            gvItem.SubItems.Add((object) this.discloseMethod[0]);
            break;
          case DisclosureTrackingBase.DisclosedMethod.eDisclosure:
            gvItem.SubItems.Add((object) this.discloseMethod[4]);
            break;
          case DisclosureTrackingBase.DisclosedMethod.Fax:
            gvItem.SubItems.Add((object) this.discloseMethod[2]);
            break;
          case DisclosureTrackingBase.DisclosedMethod.InPerson:
            gvItem.SubItems.Add((object) this.discloseMethod[1]);
            break;
          case DisclosureTrackingBase.DisclosedMethod.Other:
            gvItem.SubItems.Add((object) this.discloseMethod[3]);
            break;
          case DisclosureTrackingBase.DisclosedMethod.Email:
            gvItem.SubItems.Add((object) this.discloseMethod[5]);
            break;
          case DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder:
            gvItem.SubItems.Add((object) this.discloseMethod[6]);
            break;
          case DisclosureTrackingBase.DisclosedMethod.eClose:
            gvItem.SubItems.Add((object) this.discloseMethod[7]);
            break;
          default:
            gvItem.SubItems.Add((object) this.discloseMethod[0]);
            break;
        }
        if (disclosureTracking2015Log.IsDisclosedByLocked)
          gvItem.SubItems.Add((object) disclosureTracking2015Log.LockedDisclosedByField);
        else
          gvItem.SubItems.Add((object) (disclosureTracking2015Log.DisclosedByFullName + "(" + disclosureTracking2015Log.DisclosedBy + ")"));
        gvItem.SubItems.Add((object) disclosureTracking2015Log.NumOfDisclosedDocs);
        gvItem.SubItems.Add(disclosureTracking2015Log.DisclosedForLE ? (object) "Yes" : (object) "No");
        gvItem.SubItems.Add(disclosureTracking2015Log.DisclosedForCD ? (object) "Yes" : (object) "No");
        gvItem.SubItems.Add(disclosureTracking2015Log.DisclosedForSafeHarbor ? (object) "Yes" : (object) "No");
        gvItem.SubItems.Add(disclosureTracking2015Log.ProviderListSent || disclosureTracking2015Log.ProviderListNoFeeSent ? (object) "Yes" : (object) "No");
        if (disclosureTracking2015Log.CoBorrowerName.Trim() != "")
          gvItem.SubItems.Add((object) (disclosureTracking2015Log.BorrowerName + " and " + disclosureTracking2015Log.CoBorrowerName));
        else
          gvItem.SubItems.Add((object) disclosureTracking2015Log.BorrowerName);
        if (disclosureTracking2015Log.IsDisclosed)
          gvItem.SubItems.Add((object) "Yes");
        else
          gvItem.SubItems.Add((object) "No");
        if (disclosureTracking2015Log.DisclosureType == DisclosureTracking2015Log.DisclosureTypeEnum.PostConsummation)
          gvItem.SubItems.Add((object) "Post Consummation");
        else
          gvItem.SubItems.Add((object) disclosureTracking2015Log.DisclosureType.ToString());
        if (disclosureTracking2015Log.DisclosedForLE)
          gvItem.SubItems.Add(disclosureTracking2015Log.IntentToProceed ? (object) "Yes" : (object) "No");
        else
          gvItem.SubItems.Add((object) "");
        if (disclosureTracking2015Log.DisclosedForLE)
          gvItem.SubItems.Add(disclosureTracking2015Log.LEDisclosedByBroker ? (object) "Yes" : (object) "No");
        else
          gvItem.SubItems.Add((object) "");
        string str1 = "Active";
        if (disclosureTracking2015Log is EnhancedDisclosureTracking2015Log)
          str1 = ((EnhancedDisclosureTracking2015Log) disclosureTracking2015Log).Status == DisclosureTracking2015Log.TrackingLogStatus.Active ? "Active" : "Pending";
        gvItem.SubItems.Add((object) str1);
        gvItem.SubItems.Add(disclosureTracking2015Log.UseForUCDExport ? (object) "Y" : (object) string.Empty);
        gvItem.Tag = (object) disclosureTracking2015Log;
        if (!disclosureTracking2015Log.IsDisclosed)
          gvItem.BackColor = EncompassColors.Secondary2;
        this.gvHistory.Items.Add(gvItem);
      }
      if (nItemIndex > -1)
        this.gvHistory.Items[nItemIndex].Selected = true;
      this.gvHistory.Sort(0, SortOrder.Descending);
      this.gcHistory.Text = "Disclosure History (" + (object) this.gvHistory.Items.Count + ")";
    }

    public void RefreshContents()
    {
      this.suspendEvent = true;
      this.loanData = Session.LoanData;
      this.applySecurity();
      this.loadDisclosureLog(this.currentLog);
      this.reloadTimeline();
      this.suspendEvent = false;
    }

    public void RefreshLoanContents() => this.RefreshContents();

    private void gvHistory_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.suspendEvent = true;
      this.btnMarkValid.Enabled = false;
      this.cleanHistoryDetail();
      if (this.gvHistory.SelectedItems.Count == 0)
        return;
      this.currentLog = !(this.gvHistory.SelectedItems[0].Tag is DisclosureTracking2015Log) ? (IDisclosureTracking2015Log) this.gvHistory.SelectedItems[0].Tag : (IDisclosureTracking2015Log) this.gvHistory.SelectedItems[0].Tag;
      if (this.currentLog.IsDisclosed)
      {
        if (this.popupRules.IsButtonVisible(this.btnMarkInvalid.Text))
          this.btnMarkInvalid.Visible = true;
        if (this.markInvalidButtonEnabledAllowedByRule && !this.IsLinkedLoan)
          this.btnMarkInvalid.Enabled = true;
      }
      else
      {
        if (this.popupRules.IsButtonVisible(this.btnMarkValid.Text))
          this.btnMarkValid.Visible = true;
        if (this.markValidButtonEnabledAllowedByRule && !this.IsLinkedLoan)
          this.btnMarkValid.Enabled = true;
        if (this.currentLog is EnhancedDisclosureTracking2015Log && ((EnhancedDisclosureTracking2015Log) this.currentLog).Status == DisclosureTracking2015Log.TrackingLogStatus.Pending)
          this.btnMarkValid.Enabled = false;
        this.btnMarkInvalid.Visible = false;
      }
      if (!this.hasAccessRight)
      {
        this.lBtnApplicationDate.Enabled = false;
        this.dpLEApp.ReadOnly = true;
      }
      if (this.gvHistory.SelectedItems[0].Tag is DisclosureTracking2015Log)
      {
        if (Session.UserInfo.Userid.Equals(this.currentLog.eDisclosureLOUserId) && this.currentLog.eDisclosureLOeSignedDate == DateTime.MinValue)
          this.btnEsign.Enabled = true;
        else
          this.btnEsign.Enabled = false;
      }
      else if (this.gvHistory.SelectedItems[0].Tag is EnhancedDisclosureTracking2015Log)
      {
        EnhancedDisclosureTracking2015Log.DisclosureRecipient disclosureRecipient = ((EnhancedDisclosureTracking2015Log) this.currentLog).DisclosureRecipients.Where<EnhancedDisclosureTracking2015Log.DisclosureRecipient>((Func<EnhancedDisclosureTracking2015Log.DisclosureRecipient, bool>) (r => r.Role == EnhancedDisclosureTracking2015Log.DisclosureRecipientType.LoanAssociate && r.UserId == Session.UserInfo.Userid)).FirstOrDefault<EnhancedDisclosureTracking2015Log.DisclosureRecipient>();
        if (disclosureRecipient != null)
          this.btnEsign.Enabled = Session.LoanDataMgr.IsPlatformLoan() && disclosureRecipient.Tracking.ESignedDate.DateTime == DateTime.MinValue;
        else
          this.btnEsign.Enabled = false;
      }
      this.suspendEvent = false;
    }

    private void initialControl()
    {
      if (Session.User == null || !Session.User.GetUserInfo().IsAdministrator())
      {
        if (!(bool) Session.StartupInfo.PolicySettings[(object) "Policies.DiscloseManually"])
        {
          this.btnAdd.Visible = false;
          this.verticalSeparator1.Visible = false;
        }
        else
        {
          FeaturesAclManager aclManager = (FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features);
          if (!aclManager.GetUserApplicationRight(AclFeature.ToolsTab_DT_CreateManualEntry))
          {
            this.btnAdd.Visible = false;
            this.verticalSeparator1.Visible = false;
          }
          if (!aclManager.GetUserApplicationRight(AclFeature.ToolsTab_DT_ChangeSentDate))
            this.dpLEApp.Enabled = this.dpActClosingDate.Enabled = this.dpAffiliatedDate.Enabled = this.dpCHARMDate.Enabled = this.dpHUDSpecialDate.Enabled = this.dpHELOCBrochureDate.Enabled = this.dpAppraisalProvided.Enabled = this.dpSubAppraisal.Enabled = this.dpAVM.Enabled = this.dpHomeCounseling.Enabled = this.dpHighCostDisclosure.Enabled = false;
        }
        this.dpLEApp.MaxValue = new DateTime(2029, 1, 1);
      }
      this.fieldControlList = new System.Windows.Forms.Control[29]
      {
        (System.Windows.Forms.Control) this.dpLEApp,
        (System.Windows.Forms.Control) this.dpLEDue,
        (System.Windows.Forms.Control) this.dpCDSent,
        (System.Windows.Forms.Control) this.dpLESent,
        (System.Windows.Forms.Control) this.dpCDReceived,
        (System.Windows.Forms.Control) this.dpLEReceived,
        (System.Windows.Forms.Control) this.dpFeeCollectDate,
        (System.Windows.Forms.Control) this.dpCDRevisedSent,
        (System.Windows.Forms.Control) this.dpLERevisedSent,
        (System.Windows.Forms.Control) this.dpCDRevisedReceived,
        (System.Windows.Forms.Control) this.dpLERevisedReceived,
        (System.Windows.Forms.Control) this.dpClosingDate,
        (System.Windows.Forms.Control) this.dpActClosingDate,
        (System.Windows.Forms.Control) this.lBtnApplicationDate,
        (System.Windows.Forms.Control) this.dpeSign,
        (System.Windows.Forms.Control) this.dpIntentToProceed,
        (System.Windows.Forms.Control) this.dpAffiliatedDate,
        (System.Windows.Forms.Control) this.dpCHARMDate,
        (System.Windows.Forms.Control) this.dpHUDSpecialDate,
        (System.Windows.Forms.Control) this.dpHELOCBrochureDate,
        (System.Windows.Forms.Control) this.dpAppraisalProvided,
        (System.Windows.Forms.Control) this.dpSubAppraisal,
        (System.Windows.Forms.Control) this.dpAVM,
        (System.Windows.Forms.Control) this.dpHomeCounseling,
        (System.Windows.Forms.Control) this.dpHighCostDisclosure,
        (System.Windows.Forms.Control) this.dpSSPLSent,
        (System.Windows.Forms.Control) this.dpSHSent,
        (System.Windows.Forms.Control) this.dpPostConDisSent,
        (System.Windows.Forms.Control) this.dpPostConDisReceived
      };
      if (this.hideTimeZone())
      {
        this.txtTimezone.Visible = false;
        this.lblTimezone.Visible = false;
      }
      foreach (System.Windows.Forms.Control fieldControl in this.fieldControlList)
        this.setToolTip(fieldControl);
    }

    private void setToolTip(System.Windows.Forms.Control ctl)
    {
      if (!(ctl is DatePicker))
        return;
      DatePicker datePicker = (DatePicker) ctl;
      if (string.Concat(datePicker.Tag) == "")
        return;
      string helpKey = string.Concat(datePicker.Tag);
      string str;
      if (datePicker.Tag.ToString().Length > 4)
        str = datePicker.Tag.ToString().Split('_')[0] ?? "";
      else
        str = string.Concat(datePicker.Tag);
      datePicker.ToolTip = FieldHelp.GetText(helpKey) == "" ? str : str + ": " + FieldHelp.GetText(helpKey);
    }

    private void applyFieldAccessRights()
    {
      if (Session.UserInfo.IsSuperAdministrator())
        return;
      Hashtable fieldAccessList = Session.LoanDataMgr.GetFieldAccessList();
      string empty = string.Empty;
      foreach (System.Windows.Forms.Control fieldControl in this.fieldControlList)
      {
        FieldAccessMode fieldAccessMode = FieldAccessMode.NoRestrictions;
        bool flag = false;
        string str = "";
        DatePicker datePicker = (DatePicker) null;
        FieldLockButton fieldLockButton = (FieldLockButton) null;
        if (fieldControl is DatePicker)
        {
          datePicker = (DatePicker) fieldControl;
          if (datePicker.Tag.ToString().Length > 4)
            str = datePicker.Tag.ToString().Split('_')[0] ?? "";
          else
            str = string.Concat(datePicker.Tag);
        }
        else if (fieldControl is FieldLockButton)
        {
          fieldLockButton = (FieldLockButton) fieldControl;
          str = string.Concat(fieldLockButton.Tag);
        }
        if (!(str == ""))
        {
          FieldDefinition field = EncompassFields.GetField(str);
          if (field != null)
            flag = field.AllowEdit;
          if (fieldAccessList != null && fieldAccessList.ContainsKey((object) str))
          {
            switch (Session.LoanDataMgr.GetFieldAccessRights(str))
            {
              case BizRule.FieldAccessRight.Hide:
                fieldAccessMode = FieldAccessMode.Hidden;
                break;
              case BizRule.FieldAccessRight.ViewOnly:
                fieldAccessMode = FieldAccessMode.ReadOnly;
                break;
              case BizRule.FieldAccessRight.Edit:
                fieldAccessMode = FieldAccessMode.NoRestrictions;
                break;
            }
          }
          if (field != null && !flag)
          {
            if (datePicker != null)
            {
              datePicker.ReadOnly = true;
              if (fieldAccessMode == FieldAccessMode.Hidden)
                datePicker.Text = "//";
            }
            else
              fieldLockButton.Enabled = false;
          }
          else
          {
            switch (fieldAccessMode)
            {
              case FieldAccessMode.NoRestrictions:
                if (datePicker != null)
                {
                  if (str == "3142")
                  {
                    datePicker.ReadOnly = !this.loanData.IsLocked(str);
                    continue;
                  }
                  datePicker.ReadOnly = false;
                  if (str == "3983")
                  {
                    datePicker.ReadOnly = !this.loanData.IsLocked(str);
                    continue;
                  }
                  datePicker.ReadOnly = false;
                  if (str == "3197")
                  {
                    datePicker.ReadOnly = true;
                    continue;
                  }
                  continue;
                }
                fieldLockButton.Enabled = true;
                continue;
              case FieldAccessMode.ReadOnly:
                if (datePicker != null)
                {
                  datePicker.ReadOnly = true;
                  continue;
                }
                fieldLockButton.Enabled = false;
                continue;
              case FieldAccessMode.Hidden:
                if (datePicker != null)
                {
                  datePicker.Text = "//";
                  datePicker.ReadOnly = true;
                  continue;
                }
                fieldLockButton.Enabled = false;
                continue;
              default:
                continue;
            }
          }
        }
      }
    }

    private void cleanHistoryDetail()
    {
      if (this.popupRules.IsButtonVisible(this.btnMarkInvalid.Text))
        this.btnMarkInvalid.Visible = true;
      this.btnMarkInvalid.Enabled = false;
      this.btnMarkValid.Visible = false;
      this.btnEsign.Enabled = false;
    }

    private void btnMarkInvalid_Click(object sender, EventArgs e)
    {
      if (this.ruleChecker.HasPrerequiredFields(this.loanData, "BUTTON_EXCLUDE FROM TIMELINE", true, (Hashtable) null))
        return;
      if (this.currentLog.IntentToProceed)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "This disclosure cannot be excluded because it's indicated as \"Intent to Proceed\". In order to exclude this disclosure you must first indicate another LE disclosure as \"Intent to Proceed\" (select the check box in the Disclosure Details window). Only LE disclosure history entries without the Intent to Proceed check box selected may be excluded from the timeline.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        IDisclosureTracking2015Log tracking2015LogByType1 = this.loanData.GetLogList().GetIDisclosureTracking2015LogByType(DisclosureTracking2015Log.DisclosureTrackingType.CD, DisclosureTracking2015Log.DisclosureTypeEnum.PostConsummation);
        IDisclosureTracking2015Log tracking2015LogByType2 = this.loanData.GetLogList().GetIDisclosureTracking2015LogByType(DisclosureTracking2015Log.DisclosureTrackingType.CD, DisclosureTracking2015Log.DisclosureTypeEnum.Revised);
        IDisclosureTracking2015Log tracking2015LogByType3 = this.loanData.GetLogList().GetIDisclosureTracking2015LogByType(DisclosureTracking2015Log.DisclosureTrackingType.CD, DisclosureTracking2015Log.DisclosureTypeEnum.Initial);
        if (this.currentLog.DisclosedForCD && tracking2015LogByType3.Guid == this.currentLog.Guid && tracking2015LogByType2 == null && tracking2015LogByType1 != null)
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "Cannot exclude Initial disclosure.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else
        {
          this.currentLog.IsDisclosed = false;
          this.gvHistory.SelectedItems[0].Tag = (object) this.currentLog;
          this.RefreshContents();
          this.UpdateLatestDisclosedValue(this.loanData.GetLogList().GetLatestIDisclosureTracking2015Log(DisclosureTracking2015Log.DisclosureTrackingType.CD), this.loanData.GetLogList().GetLatestIDisclosureTracking2015Log(DisclosureTracking2015Log.DisclosureTrackingType.LE));
          if (this.IsLinknSyncPrimaryLoan)
          {
            IDisclosureTracking2015Log idisclosureTracking2015Log1 = this.loanData.LinkedData.GetLogList().GetLatestIDisclosureTracking2015Log(DisclosureTracking2015Log.DisclosureTrackingType.CD);
            IDisclosureTracking2015Log idisclosureTracking2015Log2 = this.loanData.LinkedData.GetLogList().GetLatestIDisclosureTracking2015Log(DisclosureTracking2015Log.DisclosureTrackingType.LE);
            this.UpdateLinkedLoansDisclosedLogValue(this.currentLog.Guid, this.currentLog.IsDisclosed);
            this.UpdateLatestDisclosedValueToLinkedLoans(idisclosureTracking2015Log1, idisclosureTracking2015Log2);
          }
          Session.Application.GetService<ILoanEditor>().RefreshContents();
        }
      }
    }

    private void highlightSelectedLog()
    {
      if (this.currentLog == null)
        return;
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvHistory.Items)
      {
        if (this.currentLog == (DisclosureTracking2015Log) gvItem.Tag)
        {
          gvItem.Selected = true;
          break;
        }
      }
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
      if (Session.LoanDataMgr.IsNew())
      {
        if (Utils.Dialog((IWin32Window) this, "You must save the loan before you can add a Disclosure Tracking entry. Would you like to save the loan now?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) != DialogResult.Yes)
          return;
        Cursor.Current = Cursors.WaitCursor;
        if (!Session.LoanDataMgr.SaveLoan(true, (ILoanMilestoneTemplateOrchestrator) null, false))
          return;
        Cursor.Current = Cursors.Default;
      }
      else
      {
        using (PerformanceMeter performanceMeter = PerformanceMeter.StartNew("LoanDataMgr.DDMTriggerExecute", "DDM Execution", true, 680, nameof (btnAdd_Click), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\Log\\DisclosureTracking2015\\DisclosureTracking2015WS.cs"))
        {
          Session.LoanDataMgr.DDMTriggerExecute(DDMStartPopulationTrigger.LoanSave, false);
          performanceMeter.Stop();
        }
      }
      AdHocDisclosure2015 hocDisclosure2015 = new AdHocDisclosure2015();
      if (DialogResult.Yes != hocDisclosure2015.ShowDialog((IWin32Window) this))
        return;
      this.loanData.LoanDisclosedClearGFE = true;
      Session.LoanDataMgr.AddDisclosureTracking2015toLoanLog(hocDisclosure2015.Log);
      if (Session.LoanDataMgr.LinkedLoan != null && hocDisclosure2015.Log_ls != null)
      {
        if (Session.LoanDataMgr.LoanData.Disclosure2015CreatedEventHandler != null)
        {
          Session.LoanDataMgr.LinkedLoan.LoanData.Disclosure2015Created -= Session.LoanDataMgr.LoanData.Disclosure2015CreatedEventHandler;
          Session.LoanDataMgr.LinkedLoan.LoanData.Disclosure2015Created += Session.LoanDataMgr.LoanData.Disclosure2015CreatedEventHandler;
        }
        Session.LoanDataMgr.LinkedLoan.AddDisclosureTracking2015toLoanLog(hocDisclosure2015.Log_ls);
      }
      this.currentLog = (IDisclosureTracking2015Log) hocDisclosure2015.Log;
      this.RefreshContents();
    }

    private void refreshUpdatedItem()
    {
      if (this.gvHistory.SelectedItems.Count == 0 || this.suspendEvent)
        return;
      this.gvHistory.SelectedItems[0].SubItems[0].Value = !this.currentLog.IsLocked ? (object) this.currentLog.DisclosedDate.ToString("MM/dd/yyyy") : (object) this.currentLog.LockedDisclosedDateField.ToString("MM/dd/yyyy");
      switch (this.currentLog.DisclosureMethod)
      {
        case DisclosureTrackingBase.DisclosedMethod.ByMail:
          this.gvHistory.SelectedItems[0].SubItems[1].Value = (object) this.discloseMethod[0];
          break;
        case DisclosureTrackingBase.DisclosedMethod.Fax:
          this.gvHistory.SelectedItems[0].SubItems[1].Value = (object) this.discloseMethod[2];
          break;
        case DisclosureTrackingBase.DisclosedMethod.InPerson:
          this.gvHistory.SelectedItems[0].SubItems[1].Value = (object) this.discloseMethod[1];
          break;
        case DisclosureTrackingBase.DisclosedMethod.Other:
          this.gvHistory.SelectedItems[0].SubItems[1].Value = (object) this.discloseMethod[3];
          break;
        case DisclosureTrackingBase.DisclosedMethod.Email:
          this.gvHistory.SelectedItems[0].SubItems[1].Value = (object) this.discloseMethod[5];
          break;
        default:
          this.gvHistory.SelectedItems[0].SubItems[1].Value = (object) this.discloseMethod[4];
          break;
      }
      if (this.currentLog.IsDisclosedByLocked)
        this.gvHistory.SelectedItems[0].SubItems[2].Value = (object) this.currentLog.DisclosedByFullName;
      else
        this.gvHistory.SelectedItems[0].SubItems[2].Value = (object) (this.currentLog.DisclosedByFullName + "(" + this.currentLog.DisclosedBy + ")");
    }

    private void btnMarkValid_Click(object sender, EventArgs e)
    {
      if (this.ruleChecker.HasPrerequiredFields(this.loanData, "BUTTON_INCLUDE IN TIMELINE", true, (Hashtable) null))
        return;
      string disclosedField = this.currentLog.GetDisclosedField("4461");
      if (this.gvHistory.Items.Where<GVItem>((Func<GVItem, bool>) (t => (t.Tag as IDisclosureTracking2015Log).IsDisclosed)).Select<GVItem, object>((Func<GVItem, object>) (t => t.Tag)).ToList<object>().Count == 0)
      {
        if (!disclosedField.Equals("Y") && Session.LoanDataMgr.ConfigInfo.RequireCoCPriorDisclosure)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "The disclosure level you are trying to include (Disclosure Level) is different than your Disclosure Tracking Settings for Changed Circumstances (Fee Level) and cannot be included.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return;
        }
        this.loanData.SetCurrentField("4461", disclosedField);
      }
      else
      {
        string str1 = disclosedField.Equals("Y") ? "Fee Level" : "Disclosure Level";
        string str2 = this.loanData.GetField("4461").Equals("Y") ? "Fee Level" : "Disclosure Level";
        if (!str1.Equals(str2))
        {
          int num = (int) Utils.Dialog((IWin32Window) this, string.Format("The disclosure level you are trying to include ({0}) is different than the current disclosure(s) ({1}) and cannot be included.", (object) str1, (object) str2), MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return;
        }
      }
      this.currentLog.IsDisclosed = true;
      this.gvHistory.SelectedItems[0].Tag = (object) this.currentLog;
      this.RefreshContents();
      this.UpdateLatestDisclosedValue(this.loanData.GetLogList().GetLatestIDisclosureTracking2015Log(DisclosureTracking2015Log.DisclosureTrackingType.CD), this.loanData.GetLogList().GetLatestIDisclosureTracking2015Log(DisclosureTracking2015Log.DisclosureTrackingType.LE));
      if (this.IsLinknSyncPrimaryLoan)
      {
        IDisclosureTracking2015Log idisclosureTracking2015Log1 = this.loanData.LinkedData.GetLogList().GetLatestIDisclosureTracking2015Log(DisclosureTracking2015Log.DisclosureTrackingType.CD);
        IDisclosureTracking2015Log idisclosureTracking2015Log2 = this.loanData.LinkedData.GetLogList().GetLatestIDisclosureTracking2015Log(DisclosureTracking2015Log.DisclosureTrackingType.LE);
        this.UpdateLinkedLoansDisclosedLogValue(this.currentLog.Guid, this.currentLog.IsDisclosed);
        this.UpdateLatestDisclosedValueToLinkedLoans(idisclosureTracking2015Log1, idisclosureTracking2015Log2);
      }
      Session.Application.GetService<ILoanEditor>().RefreshContents();
    }

    private void UpdateLinkedLoansDisclosedLogValue(string pLogGuid, bool isDisclosed)
    {
      IDisclosureTracking2015Log disclosureTracking2015Log = new List<IDisclosureTracking2015Log>((IEnumerable<IDisclosureTracking2015Log>) this.loanData.LinkedData.GetLogList().GetAllIDisclosureTracking2015Log(false)).FirstOrDefault<IDisclosureTracking2015Log>((Func<IDisclosureTracking2015Log, bool>) (x => x.LinkedGuid == pLogGuid));
      if (disclosureTracking2015Log == null)
        return;
      disclosureTracking2015Log.IsDisclosed = isDisclosed;
    }

    public void UpdateLatestDisclosedValue(
      IDisclosureTracking2015Log cdLatestLog,
      IDisclosureTracking2015Log leLatestLog)
    {
      IDisclosureTracking2015Log disclosureTracking2015Log = cdLatestLog;
      if (cdLatestLog == null)
      {
        disclosureTracking2015Log = leLatestLog;
        this.loanData.SetField("CD1.X64", "");
        this.loanData.SetField("CD1.X70", "");
        this.loanData.SetField("CD1.X65", "");
        this.loanData.SetField("CD1.X52", "N");
        this.loanData.SetField("CD1.X53", "N");
        this.loanData.SetField("CD1.X54", "N");
        this.loanData.SetField("CD1.X55", "N");
        this.loanData.SetField("CD1.X56", "N");
        this.loanData.SetField("CD1.X57", "N");
        this.loanData.SetField("CD1.X58", "N");
        this.loanData.SetField("CD1.X59", "N");
        this.loanData.SetField("CD1.X66", "N");
        this.loanData.SetField("CD1.X67", "N");
        this.loanData.SetField("CD1.X68", "N");
        this.loanData.SetField("CD1.X60", "");
        this.loanData.SetField("CD1.X62", "//");
        this.loanData.SetField("CD1.X63", "//");
        this.loanData.SetField("CD1.X61", "N");
      }
      if (disclosureTracking2015Log != null)
      {
        this.loanData.SetCurrentField("3121", disclosureTracking2015Log.GetDisclosedField("799"));
        this.loanData.SetCurrentField("4017", disclosureTracking2015Log.GetDisclosedField("LE1.X5"));
        this.loanData.SetCurrentField("4018", disclosureTracking2015Log.GetDisclosedField("675"));
      }
      if (leLatestLog == null)
      {
        this.loanData.SetField("3169", "");
        this.loanData.SetField("LE1.X90", "");
        this.loanData.SetField("LE1.X86", "");
        this.loanData.SetField("LE1.X78", "N");
        this.loanData.SetField("LE1.X79", "N");
        this.loanData.SetField("LE1.X80", "N");
        this.loanData.SetField("LE1.X81", "N");
        this.loanData.SetField("LE1.X82", "N");
        this.loanData.SetField("LE1.X83", "N");
        this.loanData.SetField("LE1.X84", "N");
        this.loanData.SetField("LE1.X85", "");
        this.loanData.SetField("3165", "//");
        this.loanData.SetField("3167", "//");
        this.loanData.SetField("3168", "N");
      }
      this.updateBusinessRule();
    }

    public void UpdateLatestDisclosedValueToLinkedLoans(
      IDisclosureTracking2015Log cdLatestLog,
      IDisclosureTracking2015Log leLatestLog)
    {
      IDisclosureTracking2015Log disclosureTracking2015Log = cdLatestLog;
      if (cdLatestLog == null)
      {
        disclosureTracking2015Log = leLatestLog;
        this.loanData.LinkedData.SetField("CD1.X64", "");
        this.loanData.LinkedData.SetField("CD1.X70", "");
        this.loanData.LinkedData.SetField("CD1.X65", "");
        this.loanData.LinkedData.SetField("CD1.X52", "N");
        this.loanData.LinkedData.SetField("CD1.X53", "N");
        this.loanData.LinkedData.SetField("CD1.X54", "N");
        this.loanData.LinkedData.SetField("CD1.X55", "N");
        this.loanData.LinkedData.SetField("CD1.X56", "N");
        this.loanData.LinkedData.SetField("CD1.X57", "N");
        this.loanData.LinkedData.SetField("CD1.X58", "N");
        this.loanData.LinkedData.SetField("CD1.X59", "N");
        this.loanData.LinkedData.SetField("CD1.X66", "N");
        this.loanData.LinkedData.SetField("CD1.X67", "N");
        this.loanData.LinkedData.SetField("CD1.X68", "N");
        this.loanData.LinkedData.SetField("CD1.X60", "");
        this.loanData.LinkedData.SetField("CD1.X62", "//");
        this.loanData.LinkedData.SetField("CD1.X63", "//");
        this.loanData.LinkedData.SetField("CD1.X61", "N");
      }
      if (disclosureTracking2015Log != null)
      {
        this.loanData.LinkedData.SetCurrentField("3121", disclosureTracking2015Log.GetDisclosedField("799"));
        this.loanData.LinkedData.SetCurrentField("4017", disclosureTracking2015Log.GetDisclosedField("LE1.X5"));
        this.loanData.LinkedData.SetCurrentField("4018", disclosureTracking2015Log.GetDisclosedField("675"));
      }
      if (leLatestLog != null)
        return;
      this.loanData.LinkedData.SetField("3169", "");
      this.loanData.LinkedData.SetField("LE1.X90", "");
      this.loanData.LinkedData.SetField("LE1.X86", "");
      this.loanData.LinkedData.SetField("LE1.X78", "N");
      this.loanData.LinkedData.SetField("LE1.X79", "N");
      this.loanData.LinkedData.SetField("LE1.X80", "N");
      this.loanData.LinkedData.SetField("LE1.X81", "N");
      this.loanData.LinkedData.SetField("LE1.X82", "N");
      this.loanData.LinkedData.SetField("LE1.X83", "N");
      this.loanData.LinkedData.SetField("LE1.X84", "N");
      this.loanData.LinkedData.SetField("LE1.X85", "");
      this.loanData.LinkedData.SetField("3165", "//");
      this.loanData.LinkedData.SetField("3167", "//");
      this.loanData.LinkedData.SetField("3168", "N");
    }

    private void lblSnapshot_Click(object sender, EventArgs e)
    {
    }

    private void lblSnapshot_MouseEnter(object sender, EventArgs e) => this.Cursor = Cursors.Hand;

    private void lblSnapshot_MouseLeave(object sender, EventArgs e)
    {
      this.Cursor = Cursors.Default;
    }

    private void reloadTimeline()
    {
      this.suspendEvent = true;
      string field1 = this.loanData.GetField("3142");
      this.loanData.GetField("3977");
      this.dpLEApp.ReadOnly = false;
      if (Utils.IsDate((object) field1))
        this.dpLEApp.Value = DateTime.Parse(field1);
      else
        this.dpLEApp.Text = "";
      if (this.loanData.IsLocked("3142"))
      {
        this.lBtnApplicationDate.Locked = true;
        this.dpLEApp.ReadOnly = false;
        this.dpLEApp.MaxValue = Session.User == null || !Session.User.GetUserInfo().IsAdministrator() ? DateTime.Now : new DateTime(2029, 1, 1);
      }
      else
      {
        this.lBtnApplicationDate.Locked = false;
        this.dpLEApp.ReadOnly = true;
      }
      string field2 = this.loanData.GetField("3197");
      if (Utils.IsDate((object) field2))
        this.dpIntentToProceed.Value = DateTime.Parse(field2);
      else
        this.dpIntentToProceed.Text = "";
      string field3 = this.loanData.GetField("3143");
      if (Utils.IsDate((object) field3))
        this.dpLEDue.Value = DateTime.Parse(field3);
      else
        this.dpLEDue.Text = "";
      string s = !this.forLinkSync ? this.loanData.GetField("3983") : this.loanData.LinkedData.GetField("3983");
      if (Utils.IsDate((object) s))
        this.dpeSign.Value = DateTime.Parse(s);
      else
        this.dpeSign.Text = "";
      this.dpeSign.ReadOnly = true;
      string field4 = this.loanData.GetField("3152");
      if (Utils.IsDate((object) field4))
        this.dpLESent.Value = DateTime.Parse(field4);
      else
        this.dpLESent.Text = "";
      string field5 = this.loanData.GetField("3153");
      if (Utils.IsDate((object) field5))
        this.dpLEReceived.Value = DateTime.Parse(field5);
      else
        this.dpLEReceived.Text = "";
      string field6 = this.loanData.GetField("3154");
      if (Utils.IsDate((object) field6))
        this.dpLERevisedSent.Value = DateTime.Parse(field6);
      else
        this.dpLERevisedSent.Text = "";
      string field7 = this.loanData.GetField("3155");
      if (Utils.IsDate((object) field7))
        this.dpLERevisedReceived.Value = DateTime.Parse(field7);
      else
        this.dpLERevisedReceived.Text = "";
      string field8 = this.loanData.GetField("4014");
      if (Utils.IsDate((object) field8))
        this.dpSSPLSent.Value = DateTime.Parse(field8);
      else
        this.dpSSPLSent.Text = "";
      string field9 = this.loanData.GetField("4015");
      if (Utils.IsDate((object) field9))
        this.dpSHSent.Value = DateTime.Parse(field9);
      else
        this.dpSHSent.Text = "";
      string field10 = this.loanData.GetField("3977");
      if (Utils.IsDate((object) field10))
        this.dpCDSent.Value = DateTime.Parse(field10);
      else
        this.dpCDSent.Text = "";
      string field11 = this.loanData.GetField("3978");
      if (Utils.IsDate((object) field11))
        this.dpCDReceived.Value = DateTime.Parse(field11);
      else
        this.dpCDReceived.Text = "";
      string field12 = this.loanData.GetField("3979");
      if (Utils.IsDate((object) field12))
        this.dpCDRevisedSent.Value = DateTime.Parse(field12);
      else
        this.dpCDRevisedSent.Text = "";
      string field13 = this.loanData.GetField("3980");
      if (Utils.IsDate((object) field13))
        this.dpCDRevisedReceived.Value = DateTime.Parse(field13);
      else
        this.dpCDRevisedReceived.Text = "";
      string field14 = this.loanData.GetField("3981");
      if (Utils.IsDate((object) field14))
        this.dpPostConDisSent.Value = DateTime.Parse(field14);
      else
        this.dpPostConDisSent.Text = "";
      string field15 = this.loanData.GetField("3982");
      if (Utils.IsDate((object) field15))
        this.dpPostConDisReceived.Value = DateTime.Parse(field15);
      else
        this.dpPostConDisReceived.Text = "";
      string field16 = this.loanData.GetField("3145");
      if (Utils.IsDate((object) field16))
        this.dpFeeCollectDate.Value = DateTime.Parse(field16);
      else
        this.dpFeeCollectDate.Text = "";
      string field17 = this.loanData.GetField("3147");
      if (Utils.IsDate((object) field17))
        this.dpClosingDate.Value = DateTime.Parse(field17);
      else
        this.dpClosingDate.Text = "";
      string field18 = this.loanData.GetField("763");
      if (Utils.IsDate((object) field18))
        this.dpActClosingDate.Value = DateTime.Parse(field18);
      else
        this.dpActClosingDate.Text = "";
      string field19 = this.loanData.GetField("3544");
      if (Utils.IsDate((object) field19))
        this.dpAffiliatedDate.Value = DateTime.Parse(field19);
      else
        this.dpAffiliatedDate.Text = "";
      string field20 = this.loanData.GetField("3545");
      if (Utils.IsDate((object) field20))
        this.dpCHARMDate.Value = DateTime.Parse(field20);
      else
        this.dpCHARMDate.Text = "";
      string field21 = this.loanData.GetField("3546");
      if (Utils.IsDate((object) field21))
        this.dpHUDSpecialDate.Value = DateTime.Parse(field21);
      else
        this.dpHUDSpecialDate.Text = "";
      string field22 = this.loanData.GetField("3547");
      if (Utils.IsDate((object) field22))
        this.dpHELOCBrochureDate.Value = DateTime.Parse(field22);
      else
        this.dpHELOCBrochureDate.Text = "";
      string field23 = this.loanData.GetField("3624");
      if (Utils.IsDate((object) field23))
        this.dpAppraisalProvided.Value = DateTime.Parse(field23);
      else
        this.dpAppraisalProvided.Text = "";
      string field24 = this.loanData.GetField("3857");
      if (Utils.IsDate((object) field24))
        this.dpSubAppraisal.Value = DateTime.Parse(field24);
      else
        this.dpSubAppraisal.Text = "";
      string field25 = this.loanData.GetField("3858");
      if (Utils.IsDate((object) field25))
        this.dpAVM.Value = DateTime.Parse(field25);
      else
        this.dpAVM.Text = "";
      string field26 = this.loanData.GetField("3859");
      if (Utils.IsDate((object) field26))
        this.dpHomeCounseling.Value = DateTime.Parse(field26);
      else
        this.dpHomeCounseling.Text = "";
      string field27 = this.loanData.GetField("4022");
      if (Utils.IsDate((object) field27))
        this.dpHighCostDisclosure.Value = DateTime.Parse(field27);
      else
        this.dpHighCostDisclosure.Text = "";
      this.txtTimezone.Text = this.loanData.GetField("LE1.XG9") == "" ? this.loanData.GetField("LE1.X9") : this.loanData.GetField("LE1.XG9");
      this.applyFieldAccessRights();
      this.suspendEvent = false;
    }

    private bool hideTimeZone()
    {
      return !Mapping.UseNewCompliance(21.104M, this.loanData.GetField("COMPLIANCEVERSION.X1"));
    }

    private void freeEntryDate_ValueChanged(object sender, EventArgs e)
    {
      if (this.suspendEvent)
        return;
      DatePicker datePicker = (DatePicker) sender;
      try
      {
        if (datePicker.Value != DateTime.MinValue)
        {
          if (datePicker.ToString().Length > 4)
          {
            LoanData loanData = this.loanData;
            string id1 = datePicker.Tag.ToString().Split('_')[0] ?? "";
            DateTime dateTime = datePicker.Value;
            string val1 = dateTime.ToString("MM/dd/yyyy");
            loanData.SetField(id1, val1);
            if (this.IsLinknSyncPrimaryLoan)
            {
              LoanData linkedData = this.loanData.LinkedData;
              string id2 = datePicker.Tag.ToString().Split('_')[0] ?? "";
              dateTime = datePicker.Value;
              string val2 = dateTime.ToString("MM/dd/yyyy");
              linkedData.SetField(id2, val2);
            }
          }
          else
            this.loanData.SetField(string.Concat(datePicker.Tag), datePicker.Value.ToString("MM/dd/yyyy"));
        }
        else if (datePicker.ToString().Length > 4)
        {
          this.loanData.SetField(datePicker.Tag.ToString().Split('_')[0] ?? "", "");
          if (this.IsLinknSyncPrimaryLoan)
            this.loanData.LinkedData.SetField(datePicker.Tag.ToString().Split('_')[0] ?? "", datePicker.Value.ToString("MM/dd/yyyy"));
        }
        else
          this.loanData.SetField(string.Concat(datePicker.Tag), "");
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, ex.InnerException.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        if (datePicker.ToString().Length > 4)
          this.loanData.SetField(datePicker.Tag.ToString().Split('_')[0] ?? "", "");
        else
          this.loanData.SetField(string.Concat(datePicker.Tag), "");
        datePicker.Text = "";
      }
      this.updateBusinessRule();
    }

    private void lBtnApplicationDate_Click(object sender, EventArgs e)
    {
      this.lBtnApplicationDate.Locked = !this.lBtnApplicationDate.Locked;
      this.dpLEApp.ReadOnly = !this.lBtnApplicationDate.Locked;
      if (!this.dpLEApp.ReadOnly)
      {
        this.loanData.AddLock("3142");
        if (this.IsLinknSyncPrimaryLoan)
          this.loanData.LinkedData.AddLock("3142");
        if (Utils.IsDate((object) this.loanData.GetFieldFromCal("3142")))
          this.dpLEApp.MaxValue = Utils.ParseDate((object) this.loanData.GetFieldFromCal("3142"));
      }
      else
      {
        this.loanData.RemoveLock("3142");
        if (this.IsLinknSyncPrimaryLoan)
          this.loanData.LinkedData.RemoveLock("3142");
      }
      this.reloadTimeline();
    }

    private void dpAppDate_ValueChanged(object sender, EventArgs e)
    {
      try
      {
        if (this.dpLEApp.Value != DateTime.MinValue)
        {
          LoanData loanData = this.loanData;
          DateTime dateTime = this.dpLEApp.Value;
          string val1 = dateTime.ToString("MM/dd/yyyy");
          loanData.SetField("3142", val1);
          if (this.IsLinknSyncPrimaryLoan)
          {
            LoanData linkedData = this.loanData.LinkedData;
            dateTime = this.dpLEApp.Value;
            string val2 = dateTime.ToString("MM/dd/yyyy");
            linkedData.SetField("3142", val2);
          }
        }
        else
        {
          this.loanData.SetField("3142", "//");
          if (this.IsLinknSyncPrimaryLoan)
            this.loanData.LinkedData.SetField("3142", "//");
        }
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, ex.InnerException.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.loanData.SetField("3142", "//");
        if (this.IsLinknSyncPrimaryLoan)
          this.loanData.LinkedData.SetField("3142", "//");
        this.dpLEApp.Text = "";
      }
      this.updateBusinessRule();
      this.reloadTimeline();
    }

    private void panel1_SizeChanged(object sender, EventArgs e)
    {
      GroupContainer gcCompliance = this.gcCompliance;
      GroupContainer gcLe = this.gcLE;
      GroupContainer gcCd = this.gcCD;
      Size size1 = new Size(this.panel1.Width / 4, this.gcCD.Height);
      Size size2 = size1;
      gcCd.Size = size2;
      Size size3;
      Size size4 = size3 = size1;
      gcLe.Size = size3;
      Size size5 = size4;
      gcCompliance.Size = size5;
    }

    public string GetHelpTargetName() => "Disclosure Tracking 2015";

    private void dpImportantDate_Click(object sender, EventArgs e)
    {
      if (!(sender is DatePicker))
        return;
      DatePicker datePicker = (DatePicker) sender;
      IStatusDisplay service = Session.Application.GetService<IStatusDisplay>();
      if (datePicker.Tag.ToString().Length > 4)
        service.DisplayFieldID(((string) datePicker.Tag).Split('_')[0]);
      else
        service.DisplayFieldID(string.Concat(datePicker.Tag));
    }

    private void btnViewDisclosure_Click(object sender, EventArgs e)
    {
      Session.Application.GetService<IEFolder>().ViewDisclosures(Session.LoanDataMgr, this.currentLog is DisclosureTracking2015Log ? ((DisclosureTrackingBase) this.currentLog).eDisclosurePackageViewableFile : ((EnhancedDisclosureTracking2015Log) this.currentLog).Documents.ViewableFormsFile.Trim());
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

    private void btnSafeHarborSnapshot_Click(object sender, EventArgs e)
    {
    }

    private void btnZoomIn_Click(object sender, EventArgs e) => this.showDisclosureDialog();

    private void showDisclosureDialog()
    {
      if (this.gvHistory.SelectedItems.Count == 0)
        return;
      if (this.gvHistory.SelectedItems[0].Tag is EnhancedDisclosureTracking2015Log)
      {
        int num1 = (int) new DisclosureDetailsDialogEnhanced((IDisclosureManager) new DisclosureManager((IDisclosureTracking2015Log) this.gvHistory.SelectedItems[0].Tag, this.hasAccessRight)).ShowDialog();
      }
      else
      {
        int num2 = (int) new DisclosureDetailsDialog2015((DisclosureTracking2015Log) this.gvHistory.SelectedItems[0].Tag, this.hasAccessRight).ShowDialog();
      }
      this.RefreshContents();
    }

    private void gvHistory_DoubleClick(object sender, EventArgs e) => this.showDisclosureDialog();

    private void btnSelect_Click(object sender, EventArgs e)
    {
      int num = (int) new eSignConsent().ShowDialog();
      this.RefreshContents();
    }

    private async void btnEsign_Click(object sender, EventArgs e)
    {
      DisclosureTracking2015WS disclosureTracking2015Ws = this;
      try
      {
        disclosureTracking2015Ws.btnEsign.Enabled = false;
        ESignLO esignLo = new ESignLO(disclosureTracking2015Ws.loanData.GUID);
        if (disclosureTracking2015Ws.gvHistory.SelectedItems[0].Tag is DisclosureTracking2015Log)
        {
          int num1 = (int) await esignLo.ShowESignDialog(disclosureTracking2015Ws.currentLog, disclosureTracking2015Ws.ParentForm);
        }
        else
        {
          if (!(disclosureTracking2015Ws.gvHistory.SelectedItems[0].Tag is EnhancedDisclosureTracking2015Log))
            return;
          int num2 = (int) await esignLo.ShowESignDialog(disclosureTracking2015Ws.currentLog, disclosureTracking2015Ws.ParentForm, Session.UserInfo.Userid);
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
      finally
      {
        if (disclosureTracking2015Ws.currentLog.eDisclosureLOeSignedDate == DateTime.MinValue)
          disclosureTracking2015Ws.btnEsign.Enabled = true;
        else
          disclosureTracking2015Ws.btnEsign.Enabled = false;
      }
    }

    private void dpeSign_ValueChanged(object sender, EventArgs e)
    {
      try
      {
        if (this.dpeSign.Value != DateTime.MinValue)
          this.loanData.SetField("3983", this.dpeSign.Value.ToString("MM/dd/yyyy"));
        else
          this.loanData.SetField("3983", "//");
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, ex.InnerException.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.loanData.SetField("3983", "//");
        this.dpeSign.Text = "";
      }
      this.updateBusinessRule();
      this.reloadTimeline();
    }

    private void updateBusinessRule()
    {
      try
      {
        if (this.loanData == null || this.loanData.IsTemplate)
          return;
        Session.Application.GetService<ILoanEditor>().ApplyOnDemandBusinessRules();
      }
      catch (Exception ex)
      {
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
      this.components = (IContainer) new System.ComponentModel.Container();
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
      GVColumn gvColumn15 = new GVColumn();
      this.toolTipField = new ToolTip(this.components);
      this.btnAdd = new StandardIconButton();
      this.groupContainer1 = new GroupContainer();
      this.panel2 = new System.Windows.Forms.Panel();
      this.gcHistory = new GroupContainer();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.btnMarkValid = new System.Windows.Forms.Button();
      this.btnMarkInvalid = new System.Windows.Forms.Button();
      this.btnEsign = new System.Windows.Forms.Button();
      this.verticalSeparator1 = new VerticalSeparator();
      this.btnZoomIn = new StandardIconButton();
      this.gvHistory = new GridView();
      this.splitter2 = new Splitter();
      this.panel1 = new System.Windows.Forms.Panel();
      this.groupContainer3 = new GroupContainer();
      this.dpHighCostDisclosure = new DatePicker();
      this.label28 = new System.Windows.Forms.Label();
      this.dpHomeCounseling = new DatePicker();
      this.label21 = new System.Windows.Forms.Label();
      this.dpAVM = new DatePicker();
      this.label20 = new System.Windows.Forms.Label();
      this.dpSubAppraisal = new DatePicker();
      this.label19 = new System.Windows.Forms.Label();
      this.dpAppraisalProvided = new DatePicker();
      this.label18 = new System.Windows.Forms.Label();
      this.dpAffiliatedDate = new DatePicker();
      this.label14 = new System.Windows.Forms.Label();
      this.dpHELOCBrochureDate = new DatePicker();
      this.label15 = new System.Windows.Forms.Label();
      this.dpHUDSpecialDate = new DatePicker();
      this.label16 = new System.Windows.Forms.Label();
      this.dpCHARMDate = new DatePicker();
      this.label17 = new System.Windows.Forms.Label();
      this.splitter5 = new Splitter();
      this.gcCD = new GroupContainer();
      this.label22 = new System.Windows.Forms.Label();
      this.label23 = new System.Windows.Forms.Label();
      this.dpPostConDisSent = new DatePicker();
      this.dpPostConDisReceived = new DatePicker();
      this.label13 = new System.Windows.Forms.Label();
      this.label12 = new System.Windows.Forms.Label();
      this.label11 = new System.Windows.Forms.Label();
      this.label9 = new System.Windows.Forms.Label();
      this.dpCDSent = new DatePicker();
      this.dpCDReceived = new DatePicker();
      this.dpCDRevisedSent = new DatePicker();
      this.dpCDRevisedReceived = new DatePicker();
      this.emHelpLink3 = new EMHelpLink();
      this.splitter4 = new Splitter();
      this.gcLE = new GroupContainer();
      this.label26 = new System.Windows.Forms.Label();
      this.label27 = new System.Windows.Forms.Label();
      this.dpSHSent = new DatePicker();
      this.dpSSPLSent = new DatePicker();
      this.label3 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.label6 = new System.Windows.Forms.Label();
      this.label10 = new System.Windows.Forms.Label();
      this.dpLERevisedReceived = new DatePicker();
      this.dpLESent = new DatePicker();
      this.dpLERevisedSent = new DatePicker();
      this.dpLEReceived = new DatePicker();
      this.emHelpLink2 = new EMHelpLink();
      this.splitter3 = new Splitter();
      this.splitter1 = new Splitter();
      this.gcCompliance = new GroupContainer();
      this.btnSelecteSign = new StandardIconButton();
      this.dpeSign = new DatePicker();
      this.label25 = new System.Windows.Forms.Label();
      this.dpIntentToProceed = new DatePicker();
      this.label24 = new System.Windows.Forms.Label();
      this.lBtnApplicationDate = new FieldLockButton();
      this.dpLEDue = new DatePicker();
      this.label2 = new System.Windows.Forms.Label();
      this.dpClosingDate = new DatePicker();
      this.label7 = new System.Windows.Forms.Label();
      this.dpActClosingDate = new DatePicker();
      this.dpFeeCollectDate = new DatePicker();
      this.label5 = new System.Windows.Forms.Label();
      this.dpLEApp = new DatePicker();
      this.label1 = new System.Windows.Forms.Label();
      this.label8 = new System.Windows.Forms.Label();
      this.emHelpLink1 = new EMHelpLink();
      this.lblTimezone = new System.Windows.Forms.Label();
      this.txtTimezone = new System.Windows.Forms.TextBox();
      ((ISupportInitialize) this.btnAdd).BeginInit();
      this.groupContainer1.SuspendLayout();
      this.panel2.SuspendLayout();
      this.gcHistory.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      ((ISupportInitialize) this.btnZoomIn).BeginInit();
      this.panel1.SuspendLayout();
      this.groupContainer3.SuspendLayout();
      this.gcCD.SuspendLayout();
      this.gcLE.SuspendLayout();
      this.gcCompliance.SuspendLayout();
      ((ISupportInitialize) this.btnSelecteSign).BeginInit();
      this.SuspendLayout();
      this.btnAdd.BackColor = Color.Transparent;
      this.btnAdd.Location = new Point(23, 4);
      this.btnAdd.Margin = new Padding(3, 4, 3, 3);
      this.btnAdd.MouseDownImage = (System.Drawing.Image) null;
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new Size(16, 17);
      this.btnAdd.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAdd.TabIndex = 7;
      this.btnAdd.TabStop = false;
      this.toolTipField.SetToolTip((System.Windows.Forms.Control) this.btnAdd, "Add a Disclosure Record");
      this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
      this.groupContainer1.Controls.Add((System.Windows.Forms.Control) this.panel2);
      this.groupContainer1.Controls.Add((System.Windows.Forms.Control) this.splitter2);
      this.groupContainer1.Controls.Add((System.Windows.Forms.Control) this.panel1);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Padding = new Padding(0, 0, 0, 3);
      this.groupContainer1.Size = new Size(1076, 638);
      this.groupContainer1.TabIndex = 0;
      this.groupContainer1.Text = "Disclosure Tracking Tool";
      this.panel2.Controls.Add((System.Windows.Forms.Control) this.gcHistory);
      this.panel2.Dock = DockStyle.Fill;
      this.panel2.Location = new Point(1, 315);
      this.panel2.Name = "panel2";
      this.panel2.Padding = new Padding(3, 0, 3, 0);
      this.panel2.Size = new Size(1074, 319);
      this.panel2.TabIndex = 2;
      this.gcHistory.Controls.Add((System.Windows.Forms.Control) this.flowLayoutPanel1);
      this.gcHistory.Controls.Add((System.Windows.Forms.Control) this.gvHistory);
      this.gcHistory.Dock = DockStyle.Fill;
      this.gcHistory.HeaderForeColor = SystemColors.ControlText;
      this.gcHistory.Location = new Point(3, 0);
      this.gcHistory.Name = "gcHistory";
      this.gcHistory.Size = new Size(1068, 319);
      this.gcHistory.TabIndex = 0;
      this.gcHistory.Text = "Disclosure History";
      this.flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flowLayoutPanel1.BackColor = Color.Transparent;
      this.flowLayoutPanel1.Controls.Add((System.Windows.Forms.Control) this.btnMarkValid);
      this.flowLayoutPanel1.Controls.Add((System.Windows.Forms.Control) this.btnMarkInvalid);
      this.flowLayoutPanel1.Controls.Add((System.Windows.Forms.Control) this.btnEsign);
      this.flowLayoutPanel1.Controls.Add((System.Windows.Forms.Control) this.verticalSeparator1);
      this.flowLayoutPanel1.Controls.Add((System.Windows.Forms.Control) this.btnZoomIn);
      this.flowLayoutPanel1.Controls.Add((System.Windows.Forms.Control) this.btnAdd);
      this.flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel1.Location = new Point(638, 1);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Padding = new Padding(0, 0, 5, 0);
      this.flowLayoutPanel1.Size = new Size(430, 24);
      this.flowLayoutPanel1.TabIndex = 6;
      this.btnMarkValid.Location = new Point(315, 1);
      this.btnMarkValid.Margin = new Padding(0, 1, 0, 0);
      this.btnMarkValid.Name = "btnMarkValid";
      this.btnMarkValid.Size = new Size(110, 22);
      this.btnMarkValid.TabIndex = 7;
      this.btnMarkValid.Text = "Include in Timeline";
      this.btnMarkValid.UseVisualStyleBackColor = true;
      this.btnMarkValid.Click += new EventHandler(this.btnMarkValid_Click);
      this.btnMarkInvalid.Location = new Point(182, 1);
      this.btnMarkInvalid.Margin = new Padding(0, 1, 0, 0);
      this.btnMarkInvalid.Name = "btnMarkInvalid";
      this.btnMarkInvalid.Size = new Size(133, 22);
      this.btnMarkInvalid.TabIndex = 5;
      this.btnMarkInvalid.Text = "Exclude from Timeline";
      this.btnMarkInvalid.UseVisualStyleBackColor = true;
      this.btnMarkInvalid.Click += new EventHandler(this.btnMarkInvalid_Click);
      this.btnEsign.Enabled = false;
      this.btnEsign.Location = new Point(72, 1);
      this.btnEsign.Margin = new Padding(0, 1, 0, 0);
      this.btnEsign.Name = "btnEsign";
      this.btnEsign.Size = new Size(110, 22);
      this.btnEsign.TabIndex = 9;
      this.btnEsign.Text = "eSign Documents";
      this.btnEsign.UseVisualStyleBackColor = true;
      this.btnEsign.Click += new EventHandler(this.btnEsign_Click);
      this.verticalSeparator1.Location = new Point(67, 4);
      this.verticalSeparator1.Margin = new Padding(3, 4, 3, 3);
      this.verticalSeparator1.MaximumSize = new Size(2, 16);
      this.verticalSeparator1.MinimumSize = new Size(2, 16);
      this.verticalSeparator1.Name = "verticalSeparator1";
      this.verticalSeparator1.Size = new Size(2, 16);
      this.verticalSeparator1.TabIndex = 6;
      this.verticalSeparator1.Text = "verticalSeparator1";
      this.btnZoomIn.BackColor = Color.Transparent;
      this.btnZoomIn.Location = new Point(45, 3);
      this.btnZoomIn.MouseDownImage = (System.Drawing.Image) null;
      this.btnZoomIn.Name = "btnZoomIn";
      this.btnZoomIn.Size = new Size(16, 16);
      this.btnZoomIn.StandardButtonType = StandardIconButton.ButtonType.ZoomInButton;
      this.btnZoomIn.TabIndex = 8;
      this.btnZoomIn.TabStop = false;
      this.btnZoomIn.Click += new EventHandler(this.btnZoomIn_Click);
      this.gvHistory.AllowMultiselect = false;
      this.gvHistory.BorderStyle = System.Windows.Forms.BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.SortMethod = GVSortMethod.DateTime;
      gvColumn1.Text = "Sent Date";
      gvColumn1.Width = 140;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Method";
      gvColumn2.Width = 100;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column4";
      gvColumn3.Text = "By";
      gvColumn3.Width = 150;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column3";
      gvColumn4.Text = "# of Disclosed Docs";
      gvColumn4.Width = 70;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column5";
      gvColumn5.Text = "LE Sent?";
      gvColumn5.Width = 60;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Column6";
      gvColumn6.Text = "CD Sent?";
      gvColumn6.Width = 60;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "Column9";
      gvColumn7.Text = "Safe Harbor Sent?";
      gvColumn7.Width = 104;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "Column10";
      gvColumn8.Text = "Provider List Sent?";
      gvColumn8.Width = 110;
      gvColumn9.ImageIndex = -1;
      gvColumn9.Name = "Column7";
      gvColumn9.Text = "Borrower Pair";
      gvColumn9.Width = 100;
      gvColumn10.ImageIndex = -1;
      gvColumn10.Name = "Column8";
      gvColumn10.Text = "Included in Timeline";
      gvColumn10.Width = 109;
      gvColumn11.ImageIndex = -1;
      gvColumn11.Name = "Column11";
      gvColumn11.Text = "Disclosure Type";
      gvColumn11.Width = 100;
      gvColumn12.ImageIndex = -1;
      gvColumn12.Name = "Column12";
      gvColumn12.Text = "Intent";
      gvColumn12.Width = 100;
      gvColumn13.ImageIndex = -1;
      gvColumn13.Name = "Column13";
      gvColumn13.Text = "Broker Disclosed?";
      gvColumn13.Width = 100;
      gvColumn14.ImageIndex = -1;
      gvColumn14.Name = "Column14";
      gvColumn14.Text = "Status";
      gvColumn14.Width = 100;
      gvColumn15.ImageIndex = -1;
      gvColumn15.Name = "Column15";
      gvColumn15.Text = "Use for UCD Export";
      gvColumn15.Width = 140;
      this.gvHistory.Columns.AddRange(new GVColumn[15]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6,
        gvColumn7,
        gvColumn8,
        gvColumn9,
        gvColumn10,
        gvColumn11,
        gvColumn12,
        gvColumn13,
        gvColumn14,
        gvColumn15
      });
      this.gvHistory.Dock = DockStyle.Fill;
      this.gvHistory.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvHistory.Location = new Point(1, 26);
      this.gvHistory.Name = "gvHistory";
      this.gvHistory.Size = new Size(1066, 292);
      this.gvHistory.TabIndex = 0;
      this.gvHistory.SelectedIndexChanged += new EventHandler(this.gvHistory_SelectedIndexChanged);
      this.gvHistory.DoubleClick += new EventHandler(this.gvHistory_DoubleClick);
      this.splitter2.Dock = DockStyle.Top;
      this.splitter2.Location = new Point(1, 312);
      this.splitter2.Name = "splitter2";
      this.splitter2.Size = new Size(1074, 3);
      this.splitter2.TabIndex = 1;
      this.splitter2.TabStop = false;
      this.panel1.Controls.Add((System.Windows.Forms.Control) this.groupContainer3);
      this.panel1.Controls.Add((System.Windows.Forms.Control) this.splitter5);
      this.panel1.Controls.Add((System.Windows.Forms.Control) this.gcCD);
      this.panel1.Controls.Add((System.Windows.Forms.Control) this.splitter4);
      this.panel1.Controls.Add((System.Windows.Forms.Control) this.gcLE);
      this.panel1.Controls.Add((System.Windows.Forms.Control) this.splitter3);
      this.panel1.Controls.Add((System.Windows.Forms.Control) this.splitter1);
      this.panel1.Controls.Add((System.Windows.Forms.Control) this.gcCompliance);
      this.panel1.Dock = DockStyle.Top;
      this.panel1.Location = new Point(1, 26);
      this.panel1.Name = "panel1";
      this.panel1.Padding = new Padding(3, 3, 3, 0);
      this.panel1.Size = new Size(1074, 286);
      this.panel1.TabIndex = 0;
      this.panel1.SizeChanged += new EventHandler(this.panel1_SizeChanged);
      this.groupContainer3.AutoScroll = true;
      this.groupContainer3.Controls.Add((System.Windows.Forms.Control) this.dpHighCostDisclosure);
      this.groupContainer3.Controls.Add((System.Windows.Forms.Control) this.label28);
      this.groupContainer3.Controls.Add((System.Windows.Forms.Control) this.dpHomeCounseling);
      this.groupContainer3.Controls.Add((System.Windows.Forms.Control) this.label21);
      this.groupContainer3.Controls.Add((System.Windows.Forms.Control) this.dpAVM);
      this.groupContainer3.Controls.Add((System.Windows.Forms.Control) this.label20);
      this.groupContainer3.Controls.Add((System.Windows.Forms.Control) this.dpSubAppraisal);
      this.groupContainer3.Controls.Add((System.Windows.Forms.Control) this.label19);
      this.groupContainer3.Controls.Add((System.Windows.Forms.Control) this.dpAppraisalProvided);
      this.groupContainer3.Controls.Add((System.Windows.Forms.Control) this.label18);
      this.groupContainer3.Controls.Add((System.Windows.Forms.Control) this.dpAffiliatedDate);
      this.groupContainer3.Controls.Add((System.Windows.Forms.Control) this.label14);
      this.groupContainer3.Controls.Add((System.Windows.Forms.Control) this.dpHELOCBrochureDate);
      this.groupContainer3.Controls.Add((System.Windows.Forms.Control) this.label15);
      this.groupContainer3.Controls.Add((System.Windows.Forms.Control) this.dpHUDSpecialDate);
      this.groupContainer3.Controls.Add((System.Windows.Forms.Control) this.label16);
      this.groupContainer3.Controls.Add((System.Windows.Forms.Control) this.dpCHARMDate);
      this.groupContainer3.Controls.Add((System.Windows.Forms.Control) this.label17);
      this.groupContainer3.Dock = DockStyle.Fill;
      this.groupContainer3.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer3.Location = new Point(774, 3);
      this.groupContainer3.Name = "groupContainer3";
      this.groupContainer3.Size = new Size(294, 283);
      this.groupContainer3.TabIndex = 8;
      this.groupContainer3.Text = "Other Tracking";
      this.dpHighCostDisclosure.BackColor = SystemColors.Window;
      this.dpHighCostDisclosure.Location = new Point(210, 225);
      this.dpHighCostDisclosure.Margin = new Padding(4);
      this.dpHighCostDisclosure.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dpHighCostDisclosure.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpHighCostDisclosure.Name = "dpHighCostDisclosure";
      this.dpHighCostDisclosure.Size = new Size(85, 27);
      this.dpHighCostDisclosure.TabIndex = 17;
      this.dpHighCostDisclosure.Tag = (object) "4022";
      this.dpHighCostDisclosure.ToolTip = "";
      this.dpHighCostDisclosure.Value = new DateTime(0L);
      this.dpHighCostDisclosure.ValueChanged += new EventHandler(this.freeEntryDate_ValueChanged);
      this.dpHighCostDisclosure.Click += new EventHandler(this.dpImportantDate_Click);
      this.label28.AutoSize = true;
      this.label28.Location = new Point(7, 230);
      this.label28.Name = "label28";
      this.label28.Size = new Size(162, 19);
      this.label28.TabIndex = 16;
      this.label28.Text = "High Cost Disclosure";
      this.dpHomeCounseling.BackColor = SystemColors.Window;
      this.dpHomeCounseling.Location = new Point(210, 201);
      this.dpHomeCounseling.Margin = new Padding(4);
      this.dpHomeCounseling.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dpHomeCounseling.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpHomeCounseling.Name = "dpHomeCounseling";
      this.dpHomeCounseling.Size = new Size(85, 27);
      this.dpHomeCounseling.TabIndex = 15;
      this.dpHomeCounseling.Tag = (object) "3859";
      this.dpHomeCounseling.ToolTip = "";
      this.dpHomeCounseling.Value = new DateTime(0L);
      this.dpHomeCounseling.ValueChanged += new EventHandler(this.freeEntryDate_ValueChanged);
      this.dpHomeCounseling.Click += new EventHandler(this.dpImportantDate_Click);
      this.label21.AutoSize = true;
      this.label21.Location = new Point(7, 205);
      this.label21.Name = "label21";
      this.label21.Size = new Size(288, 19);
      this.label21.TabIndex = 14;
      this.label21.Text = "Home Counseling Disclosure Provided";
      this.dpAVM.BackColor = SystemColors.Window;
      this.dpAVM.Location = new Point(210, 177);
      this.dpAVM.Margin = new Padding(4);
      this.dpAVM.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dpAVM.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpAVM.Name = "dpAVM";
      this.dpAVM.Size = new Size(85, 27);
      this.dpAVM.TabIndex = 13;
      this.dpAVM.Tag = (object) "3858";
      this.dpAVM.ToolTip = "";
      this.dpAVM.Value = new DateTime(0L);
      this.dpAVM.ValueChanged += new EventHandler(this.freeEntryDate_ValueChanged);
      this.dpAVM.Click += new EventHandler(this.dpImportantDate_Click);
      this.label20.AutoSize = true;
      this.label20.Location = new Point(7, 181);
      this.label20.Name = "label20";
      this.label20.Size = new Size(112, 19);
      this.label20.TabIndex = 12;
      this.label20.Text = "AVM Provided";
      this.dpSubAppraisal.BackColor = SystemColors.Window;
      this.dpSubAppraisal.Location = new Point(210, 153);
      this.dpSubAppraisal.Margin = new Padding(4);
      this.dpSubAppraisal.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dpSubAppraisal.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpSubAppraisal.Name = "dpSubAppraisal";
      this.dpSubAppraisal.Size = new Size(85, 27);
      this.dpSubAppraisal.TabIndex = 11;
      this.dpSubAppraisal.Tag = (object) "3857";
      this.dpSubAppraisal.ToolTip = "";
      this.dpSubAppraisal.Value = new DateTime(0L);
      this.dpSubAppraisal.ValueChanged += new EventHandler(this.freeEntryDate_ValueChanged);
      this.dpSubAppraisal.Click += new EventHandler(this.dpImportantDate_Click);
      this.label19.AutoSize = true;
      this.label19.Location = new Point(7, 157);
      this.label19.Name = "label19";
      this.label19.Size = new Size(236, 19);
      this.label19.TabIndex = 10;
      this.label19.Text = "Subsequent Appraisal Provided";
      this.dpAppraisalProvided.BackColor = SystemColors.Window;
      this.dpAppraisalProvided.Location = new Point(210, 129);
      this.dpAppraisalProvided.Margin = new Padding(4);
      this.dpAppraisalProvided.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dpAppraisalProvided.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpAppraisalProvided.Name = "dpAppraisalProvided";
      this.dpAppraisalProvided.Size = new Size(85, 27);
      this.dpAppraisalProvided.TabIndex = 9;
      this.dpAppraisalProvided.Tag = (object) "3624";
      this.dpAppraisalProvided.ToolTip = "";
      this.dpAppraisalProvided.Value = new DateTime(0L);
      this.dpAppraisalProvided.ValueChanged += new EventHandler(this.freeEntryDate_ValueChanged);
      this.dpAppraisalProvided.Click += new EventHandler(this.dpImportantDate_Click);
      this.label18.AutoSize = true;
      this.label18.Location = new Point(7, 133);
      this.label18.Name = "label18";
      this.label18.Size = new Size(171, 19);
      this.label18.TabIndex = 8;
      this.label18.Text = "1st Appraisal Provided";
      this.dpAffiliatedDate.BackColor = SystemColors.Window;
      this.dpAffiliatedDate.Location = new Point(210, 33);
      this.dpAffiliatedDate.Margin = new Padding(4);
      this.dpAffiliatedDate.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dpAffiliatedDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpAffiliatedDate.Name = "dpAffiliatedDate";
      this.dpAffiliatedDate.Size = new Size(85, 27);
      this.dpAffiliatedDate.TabIndex = 1;
      this.dpAffiliatedDate.Tag = (object) "3544";
      this.dpAffiliatedDate.ToolTip = "";
      this.dpAffiliatedDate.Value = new DateTime(0L);
      this.dpAffiliatedDate.ValueChanged += new EventHandler(this.freeEntryDate_ValueChanged);
      this.dpAffiliatedDate.Click += new EventHandler(this.dpImportantDate_Click);
      this.label14.AutoSize = true;
      this.label14.Location = new Point(7, 37);
      this.label14.Name = "label14";
      this.label14.Size = new Size(294, 19);
      this.label14.TabIndex = 0;
      this.label14.Text = "Affiliated Business Disclosure Provided";
      this.dpHELOCBrochureDate.BackColor = SystemColors.Window;
      this.dpHELOCBrochureDate.Location = new Point(210, 105);
      this.dpHELOCBrochureDate.Margin = new Padding(4);
      this.dpHELOCBrochureDate.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dpHELOCBrochureDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpHELOCBrochureDate.Name = "dpHELOCBrochureDate";
      this.dpHELOCBrochureDate.Size = new Size(85, 27);
      this.dpHELOCBrochureDate.TabIndex = 7;
      this.dpHELOCBrochureDate.Tag = (object) "3547";
      this.dpHELOCBrochureDate.ToolTip = "";
      this.dpHELOCBrochureDate.Value = new DateTime(0L);
      this.dpHELOCBrochureDate.ValueChanged += new EventHandler(this.freeEntryDate_ValueChanged);
      this.dpHELOCBrochureDate.Click += new EventHandler(this.dpImportantDate_Click);
      this.label15.AutoSize = true;
      this.label15.Location = new Point(7, 61);
      this.label15.Name = "label15";
      this.label15.Size = new Size(199, 19);
      this.label15.TabIndex = 2;
      this.label15.Text = "CHARM Booklet Provided ";
      this.dpHUDSpecialDate.BackColor = SystemColors.Window;
      this.dpHUDSpecialDate.Location = new Point(210, 81);
      this.dpHUDSpecialDate.Margin = new Padding(4);
      this.dpHUDSpecialDate.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dpHUDSpecialDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpHUDSpecialDate.Name = "dpHUDSpecialDate";
      this.dpHUDSpecialDate.Size = new Size(85, 27);
      this.dpHUDSpecialDate.TabIndex = 5;
      this.dpHUDSpecialDate.Tag = (object) "3546";
      this.dpHUDSpecialDate.ToolTip = "";
      this.dpHUDSpecialDate.Value = new DateTime(0L);
      this.dpHUDSpecialDate.ValueChanged += new EventHandler(this.freeEntryDate_ValueChanged);
      this.dpHUDSpecialDate.Click += new EventHandler(this.dpImportantDate_Click);
      this.label16.AutoSize = true;
      this.label16.Location = new Point(7, 85);
      this.label16.Name = "label16";
      this.label16.Size = new Size(223, 19);
      this.label16.TabIndex = 4;
      this.label16.Text = "Special Info Booklet Provided";
      this.dpCHARMDate.BackColor = SystemColors.Window;
      this.dpCHARMDate.Location = new Point(210, 57);
      this.dpCHARMDate.Margin = new Padding(4);
      this.dpCHARMDate.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dpCHARMDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpCHARMDate.Name = "dpCHARMDate";
      this.dpCHARMDate.Size = new Size(85, 27);
      this.dpCHARMDate.TabIndex = 3;
      this.dpCHARMDate.Tag = (object) "3545";
      this.dpCHARMDate.ToolTip = "";
      this.dpCHARMDate.Value = new DateTime(0L);
      this.dpCHARMDate.ValueChanged += new EventHandler(this.freeEntryDate_ValueChanged);
      this.dpCHARMDate.Click += new EventHandler(this.dpImportantDate_Click);
      this.label17.AutoSize = true;
      this.label17.Location = new Point(7, 109);
      this.label17.Name = "label17";
      this.label17.Size = new Size(206, 19);
      this.label17.TabIndex = 6;
      this.label17.Text = "HELOC Brochure Provided";
      this.splitter5.Location = new Point(771, 3);
      this.splitter5.Name = "splitter5";
      this.splitter5.Size = new Size(3, 283);
      this.splitter5.TabIndex = 7;
      this.splitter5.TabStop = false;
      this.gcCD.AutoScroll = true;
      this.gcCD.Controls.Add((System.Windows.Forms.Control) this.label22);
      this.gcCD.Controls.Add((System.Windows.Forms.Control) this.label23);
      this.gcCD.Controls.Add((System.Windows.Forms.Control) this.dpPostConDisSent);
      this.gcCD.Controls.Add((System.Windows.Forms.Control) this.dpPostConDisReceived);
      this.gcCD.Controls.Add((System.Windows.Forms.Control) this.label13);
      this.gcCD.Controls.Add((System.Windows.Forms.Control) this.label12);
      this.gcCD.Controls.Add((System.Windows.Forms.Control) this.label11);
      this.gcCD.Controls.Add((System.Windows.Forms.Control) this.label9);
      this.gcCD.Controls.Add((System.Windows.Forms.Control) this.dpCDSent);
      this.gcCD.Controls.Add((System.Windows.Forms.Control) this.dpCDReceived);
      this.gcCD.Controls.Add((System.Windows.Forms.Control) this.dpCDRevisedSent);
      this.gcCD.Controls.Add((System.Windows.Forms.Control) this.dpCDRevisedReceived);
      this.gcCD.Controls.Add((System.Windows.Forms.Control) this.emHelpLink3);
      this.gcCD.Dock = DockStyle.Left;
      this.gcCD.HeaderForeColor = SystemColors.ControlText;
      this.gcCD.Location = new Point(520, 3);
      this.gcCD.Name = "gcCD";
      this.gcCD.Size = new Size(251, 283);
      this.gcCD.TabIndex = 6;
      this.gcCD.Text = "CD Tracking";
      this.label22.AutoSize = true;
      this.label22.Location = new Point(7, 131);
      this.label22.MaximumSize = new Size(150, 0);
      this.label22.Name = "label22";
      this.label22.Size = new Size(124, 57);
      this.label22.TabIndex = 8;
      this.label22.Text = "Post Consummation Disclosure Sent";
      this.label23.AutoSize = true;
      this.label23.Location = new Point(7, 164);
      this.label23.MaximumSize = new Size(150, 0);
      this.label23.Name = "label23";
      this.label23.Size = new Size(122, 76);
      this.label23.TabIndex = 10;
      this.label23.Text = "Post Consummation Disclosure Received";
      this.dpPostConDisSent.BackColor = SystemColors.Window;
      this.dpPostConDisSent.Location = new Point(159, 129);
      this.dpPostConDisSent.Margin = new Padding(4);
      this.dpPostConDisSent.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.dpPostConDisSent.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpPostConDisSent.Name = "dpPostConDisSent";
      this.dpPostConDisSent.ReadOnly = true;
      this.dpPostConDisSent.Size = new Size(75, 27);
      this.dpPostConDisSent.TabIndex = 9;
      this.dpPostConDisSent.Tag = (object) "3981_2015";
      this.dpPostConDisSent.ToolTip = "";
      this.dpPostConDisSent.Value = new DateTime(0L);
      this.dpPostConDisSent.Click += new EventHandler(this.dpImportantDate_Click);
      this.dpPostConDisReceived.BackColor = SystemColors.Window;
      this.dpPostConDisReceived.Location = new Point(159, 170);
      this.dpPostConDisReceived.Margin = new Padding(4);
      this.dpPostConDisReceived.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.dpPostConDisReceived.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpPostConDisReceived.Name = "dpPostConDisReceived";
      this.dpPostConDisReceived.ReadOnly = true;
      this.dpPostConDisReceived.Size = new Size(75, 27);
      this.dpPostConDisReceived.TabIndex = 11;
      this.dpPostConDisReceived.Tag = (object) "3982_2015";
      this.dpPostConDisReceived.ToolTip = "";
      this.dpPostConDisReceived.Value = new DateTime(0L);
      this.dpPostConDisReceived.Click += new EventHandler(this.dpImportantDate_Click);
      this.label13.AutoSize = true;
      this.label13.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label13.Location = new Point(7, 37);
      this.label13.Name = "label13";
      this.label13.Size = new Size(71, 19);
      this.label13.TabIndex = 0;
      this.label13.Text = "CD Sent";
      this.label12.AutoSize = true;
      this.label12.Location = new Point(7, 61);
      this.label12.Name = "label12";
      this.label12.Size = new Size(105, 19);
      this.label12.TabIndex = 2;
      this.label12.Text = "CD Received";
      this.label11.AutoSize = true;
      this.label11.Location = new Point(7, 85);
      this.label11.Name = "label11";
      this.label11.Size = new Size(133, 19);
      this.label11.TabIndex = 4;
      this.label11.Text = "Revised CD Sent";
      this.label9.AutoSize = true;
      this.label9.Location = new Point(7, 109);
      this.label9.Name = "label9";
      this.label9.Size = new Size(167, 19);
      this.label9.TabIndex = 6;
      this.label9.Text = "Revised CD Received";
      this.dpCDSent.BackColor = SystemColors.Window;
      this.dpCDSent.Location = new Point(159, 33);
      this.dpCDSent.Margin = new Padding(4);
      this.dpCDSent.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.dpCDSent.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpCDSent.Name = "dpCDSent";
      this.dpCDSent.ReadOnly = true;
      this.dpCDSent.Size = new Size(75, 27);
      this.dpCDSent.TabIndex = 1;
      this.dpCDSent.Tag = (object) "3977_2015";
      this.dpCDSent.ToolTip = "";
      this.dpCDSent.Value = new DateTime(0L);
      this.dpCDSent.Click += new EventHandler(this.dpImportantDate_Click);
      this.dpCDReceived.BackColor = SystemColors.Window;
      this.dpCDReceived.Location = new Point(159, 57);
      this.dpCDReceived.Margin = new Padding(4);
      this.dpCDReceived.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.dpCDReceived.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpCDReceived.Name = "dpCDReceived";
      this.dpCDReceived.ReadOnly = true;
      this.dpCDReceived.Size = new Size(75, 27);
      this.dpCDReceived.TabIndex = 3;
      this.dpCDReceived.Tag = (object) "3978_2015";
      this.dpCDReceived.ToolTip = "";
      this.dpCDReceived.Value = new DateTime(0L);
      this.dpCDReceived.Click += new EventHandler(this.dpImportantDate_Click);
      this.dpCDRevisedSent.BackColor = SystemColors.Window;
      this.dpCDRevisedSent.Location = new Point(159, 81);
      this.dpCDRevisedSent.Margin = new Padding(4);
      this.dpCDRevisedSent.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.dpCDRevisedSent.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpCDRevisedSent.Name = "dpCDRevisedSent";
      this.dpCDRevisedSent.ReadOnly = true;
      this.dpCDRevisedSent.Size = new Size(75, 27);
      this.dpCDRevisedSent.TabIndex = 5;
      this.dpCDRevisedSent.Tag = (object) "3979_2015";
      this.dpCDRevisedSent.ToolTip = "";
      this.dpCDRevisedSent.Value = new DateTime(0L);
      this.dpCDRevisedSent.Click += new EventHandler(this.dpImportantDate_Click);
      this.dpCDRevisedReceived.BackColor = SystemColors.Window;
      this.dpCDRevisedReceived.Location = new Point(159, 105);
      this.dpCDRevisedReceived.Margin = new Padding(4);
      this.dpCDRevisedReceived.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.dpCDRevisedReceived.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpCDRevisedReceived.Name = "dpCDRevisedReceived";
      this.dpCDRevisedReceived.ReadOnly = true;
      this.dpCDRevisedReceived.Size = new Size(75, 27);
      this.dpCDRevisedReceived.TabIndex = 7;
      this.dpCDRevisedReceived.Tag = (object) "3980_2015";
      this.dpCDRevisedReceived.ToolTip = "";
      this.dpCDRevisedReceived.Value = new DateTime(0L);
      this.dpCDRevisedReceived.Click += new EventHandler(this.dpImportantDate_Click);
      this.emHelpLink3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.emHelpLink3.BackColor = Color.Transparent;
      this.emHelpLink3.Cursor = Cursors.Hand;
      this.emHelpLink3.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.emHelpLink3.HelpTag = "Disclosure Tracking 2015 Fields";
      this.emHelpLink3.Location = new Point(224, 5);
      this.emHelpLink3.Name = "emHelpLink3";
      this.emHelpLink3.Size = new Size(19, 26);
      this.emHelpLink3.TabIndex = 44;
      this.splitter4.Location = new Point(517, 3);
      this.splitter4.Name = "splitter4";
      this.splitter4.Size = new Size(3, 283);
      this.splitter4.TabIndex = 5;
      this.splitter4.TabStop = false;
      this.gcLE.AutoScroll = true;
      this.gcLE.Controls.Add((System.Windows.Forms.Control) this.label26);
      this.gcLE.Controls.Add((System.Windows.Forms.Control) this.label27);
      this.gcLE.Controls.Add((System.Windows.Forms.Control) this.dpSHSent);
      this.gcLE.Controls.Add((System.Windows.Forms.Control) this.dpSSPLSent);
      this.gcLE.Controls.Add((System.Windows.Forms.Control) this.label3);
      this.gcLE.Controls.Add((System.Windows.Forms.Control) this.label4);
      this.gcLE.Controls.Add((System.Windows.Forms.Control) this.label6);
      this.gcLE.Controls.Add((System.Windows.Forms.Control) this.label10);
      this.gcLE.Controls.Add((System.Windows.Forms.Control) this.dpLERevisedReceived);
      this.gcLE.Controls.Add((System.Windows.Forms.Control) this.dpLESent);
      this.gcLE.Controls.Add((System.Windows.Forms.Control) this.dpLERevisedSent);
      this.gcLE.Controls.Add((System.Windows.Forms.Control) this.dpLEReceived);
      this.gcLE.Controls.Add((System.Windows.Forms.Control) this.emHelpLink2);
      this.gcLE.Dock = DockStyle.Left;
      this.gcLE.HeaderForeColor = SystemColors.ControlText;
      this.gcLE.Location = new Point(266, 3);
      this.gcLE.Name = "gcLE";
      this.gcLE.Size = new Size(251, 283);
      this.gcLE.TabIndex = 4;
      this.gcLE.Text = "LE Tracking";
      this.label26.Location = new Point(7, 157);
      this.label26.Name = "label26";
      this.label26.Size = new Size(138, 17);
      this.label26.TabIndex = 10;
      this.label26.Text = "Safe Harbor Sent";
      this.label27.AutoSize = true;
      this.label27.Location = new Point(7, 133);
      this.label27.Name = "label27";
      this.label27.Size = new Size(88, 19);
      this.label27.TabIndex = 8;
      this.label27.Text = "SSPL Sent";
      this.dpSHSent.BackColor = SystemColors.Window;
      this.dpSHSent.Location = new Point(155, 153);
      this.dpSHSent.Margin = new Padding(4);
      this.dpSHSent.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.dpSHSent.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpSHSent.Name = "dpSHSent";
      this.dpSHSent.ReadOnly = true;
      this.dpSHSent.Size = new Size(75, 27);
      this.dpSHSent.TabIndex = 11;
      this.dpSHSent.Tag = (object) "4015_2015";
      this.dpSHSent.ToolTip = "";
      this.dpSHSent.Value = new DateTime(0L);
      this.dpSHSent.Click += new EventHandler(this.dpImportantDate_Click);
      this.dpSSPLSent.BackColor = SystemColors.Window;
      this.dpSSPLSent.Location = new Point(155, 129);
      this.dpSSPLSent.Margin = new Padding(4);
      this.dpSSPLSent.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.dpSSPLSent.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpSSPLSent.Name = "dpSSPLSent";
      this.dpSSPLSent.ReadOnly = true;
      this.dpSSPLSent.Size = new Size(75, 27);
      this.dpSSPLSent.TabIndex = 9;
      this.dpSSPLSent.Tag = (object) "4014_2015";
      this.dpSSPLSent.ToolTip = "";
      this.dpSSPLSent.Value = new DateTime(0L);
      this.dpSSPLSent.Click += new EventHandler(this.dpImportantDate_Click);
      this.label3.Location = new Point(7, 109);
      this.label3.Name = "label3";
      this.label3.Size = new Size(138, 17);
      this.label3.TabIndex = 6;
      this.label3.Text = "Revised LE Received";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(7, 85);
      this.label4.Name = "label4";
      this.label4.Size = new Size(129, 19);
      this.label4.TabIndex = 4;
      this.label4.Text = "Revised LE Sent";
      this.label6.AutoSize = true;
      this.label6.Location = new Point(7, 61);
      this.label6.Name = "label6";
      this.label6.Size = new Size(101, 19);
      this.label6.TabIndex = 2;
      this.label6.Text = "LE Received";
      this.label10.AutoSize = true;
      this.label10.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label10.Location = new Point(7, 37);
      this.label10.Name = "label10";
      this.label10.Size = new Size(67, 19);
      this.label10.TabIndex = 0;
      this.label10.Text = "LE Sent";
      this.dpLERevisedReceived.BackColor = SystemColors.Window;
      this.dpLERevisedReceived.Location = new Point(155, 105);
      this.dpLERevisedReceived.Margin = new Padding(4);
      this.dpLERevisedReceived.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.dpLERevisedReceived.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpLERevisedReceived.Name = "dpLERevisedReceived";
      this.dpLERevisedReceived.ReadOnly = true;
      this.dpLERevisedReceived.Size = new Size(75, 27);
      this.dpLERevisedReceived.TabIndex = 7;
      this.dpLERevisedReceived.Tag = (object) "3155_2015";
      this.dpLERevisedReceived.ToolTip = "";
      this.dpLERevisedReceived.Value = new DateTime(0L);
      this.dpLERevisedReceived.Click += new EventHandler(this.dpImportantDate_Click);
      this.dpLESent.BackColor = SystemColors.Window;
      this.dpLESent.Location = new Point(155, 33);
      this.dpLESent.Margin = new Padding(4);
      this.dpLESent.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.dpLESent.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpLESent.Name = "dpLESent";
      this.dpLESent.ReadOnly = true;
      this.dpLESent.Size = new Size(75, 27);
      this.dpLESent.TabIndex = 1;
      this.dpLESent.Tag = (object) "3152_2015";
      this.dpLESent.ToolTip = "";
      this.dpLESent.Value = new DateTime(0L);
      this.dpLESent.Click += new EventHandler(this.dpImportantDate_Click);
      this.dpLERevisedSent.BackColor = SystemColors.Window;
      this.dpLERevisedSent.Location = new Point(155, 81);
      this.dpLERevisedSent.Margin = new Padding(4);
      this.dpLERevisedSent.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.dpLERevisedSent.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpLERevisedSent.Name = "dpLERevisedSent";
      this.dpLERevisedSent.ReadOnly = true;
      this.dpLERevisedSent.Size = new Size(75, 27);
      this.dpLERevisedSent.TabIndex = 5;
      this.dpLERevisedSent.Tag = (object) "3154_2015";
      this.dpLERevisedSent.ToolTip = "";
      this.dpLERevisedSent.Value = new DateTime(0L);
      this.dpLERevisedSent.Click += new EventHandler(this.dpImportantDate_Click);
      this.dpLEReceived.BackColor = SystemColors.Window;
      this.dpLEReceived.Location = new Point(155, 57);
      this.dpLEReceived.Margin = new Padding(4);
      this.dpLEReceived.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.dpLEReceived.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpLEReceived.Name = "dpLEReceived";
      this.dpLEReceived.ReadOnly = true;
      this.dpLEReceived.Size = new Size(75, 27);
      this.dpLEReceived.TabIndex = 3;
      this.dpLEReceived.Tag = (object) "3153_2015";
      this.dpLEReceived.ToolTip = "";
      this.dpLEReceived.Value = new DateTime(0L);
      this.dpLEReceived.Click += new EventHandler(this.dpImportantDate_Click);
      this.emHelpLink2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.emHelpLink2.BackColor = Color.Transparent;
      this.emHelpLink2.Cursor = Cursors.Hand;
      this.emHelpLink2.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.emHelpLink2.HelpTag = "Disclosure Tracking 2015 Fields";
      this.emHelpLink2.Location = new Point(223, 5);
      this.emHelpLink2.Name = "emHelpLink2";
      this.emHelpLink2.Size = new Size(19, 26);
      this.emHelpLink2.TabIndex = 52;
      this.splitter3.Dock = DockStyle.Right;
      this.splitter3.Location = new Point(1068, 3);
      this.splitter3.Name = "splitter3";
      this.splitter3.Size = new Size(3, 283);
      this.splitter3.TabIndex = 4;
      this.splitter3.TabStop = false;
      this.splitter1.Location = new Point(263, 3);
      this.splitter1.Name = "splitter1";
      this.splitter1.Size = new Size(3, 283);
      this.splitter1.TabIndex = 2;
      this.splitter1.TabStop = false;
      this.gcCompliance.AutoScroll = true;
      this.gcCompliance.Controls.Add((System.Windows.Forms.Control) this.txtTimezone);
      this.gcCompliance.Controls.Add((System.Windows.Forms.Control) this.lblTimezone);
      this.gcCompliance.Controls.Add((System.Windows.Forms.Control) this.btnSelecteSign);
      this.gcCompliance.Controls.Add((System.Windows.Forms.Control) this.dpeSign);
      this.gcCompliance.Controls.Add((System.Windows.Forms.Control) this.label25);
      this.gcCompliance.Controls.Add((System.Windows.Forms.Control) this.dpIntentToProceed);
      this.gcCompliance.Controls.Add((System.Windows.Forms.Control) this.label24);
      this.gcCompliance.Controls.Add((System.Windows.Forms.Control) this.lBtnApplicationDate);
      this.gcCompliance.Controls.Add((System.Windows.Forms.Control) this.dpLEDue);
      this.gcCompliance.Controls.Add((System.Windows.Forms.Control) this.label2);
      this.gcCompliance.Controls.Add((System.Windows.Forms.Control) this.dpClosingDate);
      this.gcCompliance.Controls.Add((System.Windows.Forms.Control) this.label7);
      this.gcCompliance.Controls.Add((System.Windows.Forms.Control) this.dpActClosingDate);
      this.gcCompliance.Controls.Add((System.Windows.Forms.Control) this.dpFeeCollectDate);
      this.gcCompliance.Controls.Add((System.Windows.Forms.Control) this.label5);
      this.gcCompliance.Controls.Add((System.Windows.Forms.Control) this.dpLEApp);
      this.gcCompliance.Controls.Add((System.Windows.Forms.Control) this.label1);
      this.gcCompliance.Controls.Add((System.Windows.Forms.Control) this.label8);
      this.gcCompliance.Controls.Add((System.Windows.Forms.Control) this.emHelpLink1);
      this.gcCompliance.Dock = DockStyle.Left;
      this.gcCompliance.HeaderForeColor = SystemColors.ControlText;
      this.gcCompliance.Location = new Point(3, 3);
      this.gcCompliance.Name = "gcCompliance";
      this.gcCompliance.Size = new Size(260, 283);
      this.gcCompliance.TabIndex = 2;
      this.gcCompliance.Text = "Compliance Timeline";
      this.btnSelecteSign.BackColor = Color.Transparent;
      this.btnSelecteSign.Location = new Point(239, 84);
      this.btnSelecteSign.MouseDownImage = (System.Drawing.Image) null;
      this.btnSelecteSign.Name = "btnSelecteSign";
      this.btnSelecteSign.Size = new Size(16, 16);
      this.btnSelecteSign.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnSelecteSign.TabIndex = 49;
      this.btnSelecteSign.TabStop = false;
      this.btnSelecteSign.Click += new EventHandler(this.btnSelect_Click);
      this.dpeSign.BackColor = SystemColors.Window;
      this.dpeSign.Location = new Point(152, 82);
      this.dpeSign.Margin = new Padding(4);
      this.dpeSign.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dpeSign.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpeSign.Name = "dpeSign";
      this.dpeSign.ReadOnly = true;
      this.dpeSign.Size = new Size(85, 27);
      this.dpeSign.TabIndex = 5;
      this.dpeSign.Tag = (object) "3983_2015";
      this.dpeSign.ToolTip = "";
      this.dpeSign.Value = new DateTime(0L);
      this.dpeSign.ValueChanged += new EventHandler(this.dpeSign_ValueChanged);
      this.dpeSign.Click += new EventHandler(this.dpImportantDate_Click);
      this.label25.AutoSize = true;
      this.label25.Location = new Point(8, 85);
      this.label25.Name = "label25";
      this.label25.Size = new Size(78, 19);
      this.label25.TabIndex = 4;
      this.label25.Text = "eConsent";
      this.dpIntentToProceed.BackColor = SystemColors.Window;
      this.dpIntentToProceed.Location = new Point(152, 106);
      this.dpIntentToProceed.Margin = new Padding(4);
      this.dpIntentToProceed.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dpIntentToProceed.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpIntentToProceed.Name = "dpIntentToProceed";
      this.dpIntentToProceed.ReadOnly = true;
      this.dpIntentToProceed.Size = new Size(85, 27);
      this.dpIntentToProceed.TabIndex = 7;
      this.dpIntentToProceed.Tag = (object) "3197_2015";
      this.dpIntentToProceed.ToolTip = "";
      this.dpIntentToProceed.Value = new DateTime(0L);
      this.dpIntentToProceed.ValueChanged += new EventHandler(this.freeEntryDate_ValueChanged);
      this.dpIntentToProceed.Click += new EventHandler(this.dpImportantDate_Click);
      this.label24.AutoSize = true;
      this.label24.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label24.Location = new Point(7, 61);
      this.label24.Name = "label24";
      this.label24.Size = new Size(64, 19);
      this.label24.TabIndex = 2;
      this.label24.Text = "LE Due";
      this.lBtnApplicationDate.Location = new Point(133, 36);
      this.lBtnApplicationDate.LockedStateToolTip = "Use Default Value";
      this.lBtnApplicationDate.MaximumSize = new Size(16, 17);
      this.lBtnApplicationDate.MinimumSize = new Size(16, 17);
      this.lBtnApplicationDate.Name = "lBtnApplicationDate";
      this.lBtnApplicationDate.Size = new Size(16, 17);
      this.lBtnApplicationDate.TabIndex = 42;
      this.lBtnApplicationDate.Tag = (object) "LOCKBUTTON_3142";
      this.lBtnApplicationDate.UnlockedStateToolTip = "Enter Data Manually";
      this.lBtnApplicationDate.Click += new EventHandler(this.lBtnApplicationDate_Click);
      this.dpLEDue.BackColor = SystemColors.Window;
      this.dpLEDue.Location = new Point(152, 57);
      this.dpLEDue.Margin = new Padding(4);
      this.dpLEDue.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.dpLEDue.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpLEDue.Name = "dpLEDue";
      this.dpLEDue.ReadOnly = true;
      this.dpLEDue.Size = new Size(85, 27);
      this.dpLEDue.TabIndex = 3;
      this.dpLEDue.Tag = (object) "3143_2015";
      this.dpLEDue.ToolTip = "";
      this.dpLEDue.Value = new DateTime(0L);
      this.dpLEDue.Click += new EventHandler(this.dpImportantDate_Click);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(7, 110);
      this.label2.Name = "label2";
      this.label2.Size = new Size(134, 19);
      this.label2.TabIndex = 6;
      this.label2.Text = "Intent to Proceed";
      this.dpClosingDate.BackColor = SystemColors.Window;
      this.dpClosingDate.Location = new Point(152, 154);
      this.dpClosingDate.Margin = new Padding(4);
      this.dpClosingDate.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.dpClosingDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpClosingDate.Name = "dpClosingDate";
      this.dpClosingDate.ReadOnly = true;
      this.dpClosingDate.Size = new Size(85, 27);
      this.dpClosingDate.TabIndex = 11;
      this.dpClosingDate.Tag = (object) "3147_2015";
      this.dpClosingDate.ToolTip = "";
      this.dpClosingDate.Value = new DateTime(0L);
      this.dpClosingDate.Click += new EventHandler(this.dpImportantDate_Click);
      this.label7.AutoSize = true;
      this.label7.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label7.Location = new Point(7, 158);
      this.label7.Name = "label7";
      this.label7.Size = new Size(122, 19);
      this.label7.TabIndex = 10;
      this.label7.Text = "Earliest Closing";
      this.dpActClosingDate.BackColor = SystemColors.Window;
      this.dpActClosingDate.Location = new Point(152, 178);
      this.dpActClosingDate.Margin = new Padding(4);
      this.dpActClosingDate.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dpActClosingDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpActClosingDate.Name = "dpActClosingDate";
      this.dpActClosingDate.Size = new Size(85, 27);
      this.dpActClosingDate.TabIndex = 13;
      this.dpActClosingDate.Tag = (object) "763_2015";
      this.dpActClosingDate.ToolTip = "";
      this.dpActClosingDate.Value = new DateTime(0L);
      this.dpActClosingDate.ValueChanged += new EventHandler(this.freeEntryDate_ValueChanged);
      this.dpActClosingDate.Click += new EventHandler(this.dpImportantDate_Click);
      this.dpFeeCollectDate.BackColor = SystemColors.Window;
      this.dpFeeCollectDate.Location = new Point(152, 130);
      this.dpFeeCollectDate.Margin = new Padding(4);
      this.dpFeeCollectDate.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.dpFeeCollectDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpFeeCollectDate.Name = "dpFeeCollectDate";
      this.dpFeeCollectDate.ReadOnly = true;
      this.dpFeeCollectDate.Size = new Size(85, 27);
      this.dpFeeCollectDate.TabIndex = 9;
      this.dpFeeCollectDate.Tag = (object) "3145_2015";
      this.dpFeeCollectDate.ToolTip = "";
      this.dpFeeCollectDate.Value = new DateTime(0L);
      this.dpFeeCollectDate.Click += new EventHandler(this.dpImportantDate_Click);
      this.label5.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label5.Location = new Point(7, 134);
      this.label5.Name = "label5";
      this.label5.Size = new Size(137, 13);
      this.label5.TabIndex = 8;
      this.label5.Text = "Earliest Fee Collection";
      this.dpLEApp.BackColor = SystemColors.Window;
      this.dpLEApp.Location = new Point(152, 33);
      this.dpLEApp.Margin = new Padding(4);
      this.dpLEApp.MaxValue = new DateTime(2029, 1, 1, 0, 0, 0, 0);
      this.dpLEApp.MinValue = new DateTime(2000, 1, 1, 0, 0, 0, 0);
      this.dpLEApp.Name = "dpLEApp";
      this.dpLEApp.ReadOnly = true;
      this.dpLEApp.Size = new Size(85, 27);
      this.dpLEApp.TabIndex = 1;
      this.dpLEApp.Tag = (object) "3142_2015";
      this.dpLEApp.ToolTip = "";
      this.dpLEApp.Value = new DateTime(0L);
      this.dpLEApp.ValueChanged += new EventHandler(this.dpAppDate_ValueChanged);
      this.dpLEApp.Click += new EventHandler(this.dpImportantDate_Click);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(7, 37);
      this.label1.Name = "label1";
      this.label1.Size = new Size(128, 19);
      this.label1.TabIndex = 0;
      this.label1.Text = "Application Date";
      this.label8.AutoSize = true;
      this.label8.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label8.Location = new Point(7, 182);
      this.label8.Name = "label8";
      this.label8.Size = new Size(139, 19);
      this.label8.TabIndex = 12;
      this.label8.Text = "Estimated Closing";
      this.emHelpLink1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.emHelpLink1.HelpTag = "Disclosure Tracking 2015 Fields";
      this.emHelpLink1.Location = new Point(233, 5);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(19, 24);
      this.emHelpLink1.TabIndex = 43;
      this.lblTimezone.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblTimezone.Location = new Point(7, 206);
      this.lblTimezone.Name = "lblTimezone";
      this.lblTimezone.Size = new Size(140, 40);
      this.lblTimezone.TabIndex = 50;
      this.lblTimezone.Text = "Disclosure Tracking Timezone";
      this.txtTimezone.BackColor = SystemColors.Menu;
      this.txtTimezone.Enabled = false;
      this.txtTimezone.Location = new Point(152, 202);
      this.txtTimezone.Name = "txtTimezone";
      this.txtTimezone.Size = new Size(85, 23);
      this.txtTimezone.TabIndex = 51;
      this.AutoScaleDimensions = new SizeF(9f, 19f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((System.Windows.Forms.Control) this.groupContainer1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (DisclosureTracking2015WS);
      this.Size = new Size(1076, 638);
      ((ISupportInitialize) this.btnAdd).EndInit();
      this.groupContainer1.ResumeLayout(false);
      this.panel2.ResumeLayout(false);
      this.gcHistory.ResumeLayout(false);
      this.flowLayoutPanel1.ResumeLayout(false);
      ((ISupportInitialize) this.btnZoomIn).EndInit();
      this.panel1.ResumeLayout(false);
      this.groupContainer3.ResumeLayout(false);
      this.groupContainer3.PerformLayout();
      this.gcCD.ResumeLayout(false);
      this.gcCD.PerformLayout();
      this.gcLE.ResumeLayout(false);
      this.gcLE.PerformLayout();
      this.gcCompliance.ResumeLayout(false);
      this.gcCompliance.PerformLayout();
      ((ISupportInitialize) this.btnSelecteSign).EndInit();
      this.ResumeLayout(false);
    }
  }
}
