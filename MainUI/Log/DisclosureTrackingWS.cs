// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.DisclosureTrackingWS
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
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using EllieMae.Encompass.Forms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class DisclosureTrackingWS : UserControl, IRefreshContents, IOnlineHelpTarget
  {
    private const string className = "DisclosureTrackingWS";
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
    private System.Windows.Forms.Control[] fieldControlList;
    private bool hasAccessRight = true;
    private BusinessRuleCheck ruleChecker = new BusinessRuleCheck();
    private PopupBusinessRules popupRules;
    private bool markInvalidButtonEnabledAllowedByRule = true;
    private bool markValidButtonEnabledAllowedByRule = true;
    private bool timeLineAccessFlag = true;
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
    private DatePicker dpAppDate;
    private DatePicker dpClosingDate;
    private DatePicker dpFeeCollectDate;
    private System.Windows.Forms.Button btnMarkInvalid;
    private FlowLayoutPanel flowLayoutPanel1;
    private VerticalSeparator verticalSeparator1;
    private StandardIconButton btnAdd;
    private System.Windows.Forms.Button btnMarkValid;
    private DatePicker dpActClosingDate;
    private DatePicker dpGFEActReReceivedDate;
    private DatePicker dpGFEActReProvidedDate;
    private DatePicker dpGFEActInitReceivedDate;
    private DatePicker dpGFEActInitProvidedDate;
    private DatePicker dpTILActReReceivedDate;
    private DatePicker dpTILActReProvidedDate;
    private DatePicker dpTILActInitReceivedDate;
    private DatePicker dpTILActInitProvidedDate;
    private DatePicker dpInitialDisclosureDueDate;
    private System.Windows.Forms.Label label2;
    private GroupContainer gcTIL;
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
    private DatePicker dpTILAppDate;
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
    private GroupContainer gcGFE;
    private GroupContainer groupContainer3;
    private DatePicker dpAppraisalProvided;
    private System.Windows.Forms.Label label18;
    private DatePicker dpAVM;
    private System.Windows.Forms.Label label20;
    private DatePicker dpSubAppraisal;
    private System.Windows.Forms.Label label19;
    private DatePicker dpHomeCounseling;
    private System.Windows.Forms.Label label21;
    private System.Windows.Forms.TextBox txtTimezone;
    private System.Windows.Forms.Label lblTimezone;

    public DisclosureTrackingWS(
      DisclosureTrackingLog selectedLog,
      bool clearNotification,
      Sessions.Session session)
    {
      this.InitializeComponent();
      this.loanData = Session.LoanData;
      if (Session.StartupInfo.UserAclFeatureRights.Contains((object) AclFeature.ToolsTab_DT_ExcludeIncludeRecords))
        this.timeLineAccessFlag = (bool) Session.StartupInfo.UserAclFeatureRights[(object) AclFeature.ToolsTab_DT_ExcludeIncludeRecords];
      this.btnMarkInvalid.Enabled = this.timeLineAccessFlag;
      try
      {
        this.popupRules = new PopupBusinessRules(this.loanData, (ResourceManager) null, (System.Drawing.Image) null, (System.Drawing.Image) null, session);
        if (this.timeLineAccessFlag)
        {
          this.popupRules.SetBusinessRules(this.btnMarkInvalid);
          this.markInvalidButtonEnabledAllowedByRule = this.btnMarkInvalid.Enabled;
        }
        else
          this.markInvalidButtonEnabledAllowedByRule = false;
        this.popupRules.SetBusinessRules(this.btnMarkValid);
        this.markValidButtonEnabledAllowedByRule = this.btnMarkInvalid.Enabled;
      }
      catch (Exception ex)
      {
        Tracing.Log(DisclosureTrackingWS.sw, TraceLevel.Error, nameof (DisclosureTrackingWS), "Cannot set Button access right. Error: " + ex.Message);
      }
      this.applySecurity();
      this.suspendEvent = true;
      this.initialControl();
      this.synchronizeeDisclosureTrackingInfo(clearNotification);
      this.gvHistory.Sort(0, SortOrder.Descending);
      if (selectedLog == null)
        selectedLog = this.loanData.GetLogList().GetLatestDisclosureTrackingLog(DisclosureTrackingLog.DisclosureTrackingType.All);
      this.loadDisclosureLog(selectedLog);
      this.reloadTimeline();
      this.suspendEvent = false;
      if (selectedLog == null && this.gvHistory.Items.Count > 0)
        this.gvHistory.Items[0].Selected = true;
      this.Dock = DockStyle.Fill;
    }

    private void restoreDisclosedByFullDate(DisclosureTrackingLog log)
    {
      if (!(log.DisclosedByFullName == "") || !(log.DisclosedBy != ""))
        return;
      if (log.IsDisclosedByLocked)
        return;
      try
      {
        UserInfo user = Session.OrganizationManager.GetUser(log.DisclosedBy);
        if (!(user != (UserInfo) null))
          return;
        log.DisclosedByFullName = user.FullName;
      }
      catch
      {
      }
    }

    public DisclosureTrackingWS(string packageID, bool clearNotification, Sessions.Session session)
      : this(DisclosureTrackingWS.getLogFromPackageID(packageID, Session.LoanData), clearNotification, session)
    {
    }

    private static DisclosureTrackingLog getLogFromPackageID(string packageID, LoanData loanData)
    {
      foreach (DisclosureTrackingLog logFromPackageId in loanData.GetLogList().GetAllDisclosureTrackingLog(false))
      {
        if (logFromPackageId.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.eDisclosure && logFromPackageId.eDisclosurePackageID == packageID)
          return logFromPackageId;
      }
      return (DisclosureTrackingLog) null;
    }

    public DisclosureTrackingWS(Sessions.Session session)
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
      if (Session.LoanData.GetField("3969") == "Old GFE and HUD-1" || Utils.CheckIf2015RespaTila(Session.LoanData.GetField("3969")))
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

    private void loadDisclosureLog(DisclosureTrackingLog selectedLog)
    {
      this.cleanHistoryDetail();
      this.gvHistory.Items.Clear();
      DisclosureTrackingLog[] disclosureTrackingLog = this.loanData.GetLogList().GetAllDisclosureTrackingLog(false);
      int nItemIndex = -1;
      foreach (DisclosureTrackingLog log in disclosureTrackingLog)
      {
        this.restoreDisclosedByFullDate(log);
        if (selectedLog != null && log.Guid == selectedLog.Guid)
          nItemIndex = this.gvHistory.Items.Count;
        GVItem gvItem = new GVItem(log.DisclosedDate.ToString("MM/dd/yyyy"));
        gvItem.SubItems[0].SortValue = (object) log;
        switch (log.DisclosureMethod)
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
          default:
            gvItem.SubItems.Add((object) this.discloseMethod[0]);
            break;
        }
        if (log.IsDisclosedByLocked)
          gvItem.SubItems.Add((object) log.DisclosedByFullName);
        else
          gvItem.SubItems.Add((object) (log.DisclosedByFullName + "(" + log.DisclosedBy + ")"));
        gvItem.SubItems.Add((object) log.NumOfDisclosedDocs);
        gvItem.SubItems.Add(log.DisclosedForGFE ? (object) "Yes" : (object) "No");
        gvItem.SubItems.Add(log.DisclosedForTIL ? (object) "Yes" : (object) "No");
        gvItem.SubItems.Add(log.DisclosedForSafeHarbor ? (object) "Yes" : (object) "No");
        if (log.CoBorrowerName.Trim() != "")
          gvItem.SubItems.Add((object) (log.BorrowerName + " and " + log.CoBorrowerName));
        else
          gvItem.SubItems.Add((object) log.BorrowerName);
        if (log.IsDisclosed)
          gvItem.SubItems.Add((object) "Yes");
        else
          gvItem.SubItems.Add((object) "No");
        gvItem.Tag = (object) log;
        if (!log.IsDisclosed)
          gvItem.BackColor = EncompassColors.Secondary2;
        this.gvHistory.Items.Add(gvItem);
      }
      if (nItemIndex > -1)
        this.gvHistory.Items[nItemIndex].Selected = true;
      this.gvHistory.ReSort();
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
      this.cleanHistoryDetail();
      if (this.gvHistory.SelectedItems.Count == 0)
        return;
      this.currentLog = (DisclosureTrackingLog) this.gvHistory.SelectedItems[0].Tag;
      if (this.currentLog.IsDisclosed)
      {
        if (this.popupRules.IsButtonVisible(this.btnMarkInvalid.Text))
          this.btnMarkInvalid.Visible = true;
        if (this.markInvalidButtonEnabledAllowedByRule)
          this.btnMarkInvalid.Enabled = true;
      }
      else
      {
        if (this.popupRules.IsButtonVisible(this.btnMarkValid.Text))
          this.btnMarkValid.Visible = true;
        this.btnMarkInvalid.Visible = false;
      }
      if (!this.hasAccessRight)
      {
        this.lBtnApplicationDate.Enabled = false;
        this.dpAppDate.ReadOnly = true;
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
            this.dpAppDate.Enabled = this.dpTILAppDate.Enabled = this.dpActClosingDate.Enabled = this.dpAffiliatedDate.Enabled = this.dpCHARMDate.Enabled = this.dpHUDSpecialDate.Enabled = this.dpHELOCBrochureDate.Enabled = this.dpAppraisalProvided.Enabled = this.dpSubAppraisal.Enabled = this.dpAVM.Enabled = this.dpHomeCounseling.Enabled = false;
        }
      }
      this.fieldControlList = new System.Windows.Forms.Control[23]
      {
        (System.Windows.Forms.Control) this.dpAppDate,
        (System.Windows.Forms.Control) this.dpInitialDisclosureDueDate,
        (System.Windows.Forms.Control) this.dpGFEActInitProvidedDate,
        (System.Windows.Forms.Control) this.dpTILActInitProvidedDate,
        (System.Windows.Forms.Control) this.dpGFEActInitReceivedDate,
        (System.Windows.Forms.Control) this.dpTILActInitReceivedDate,
        (System.Windows.Forms.Control) this.dpFeeCollectDate,
        (System.Windows.Forms.Control) this.dpGFEActReProvidedDate,
        (System.Windows.Forms.Control) this.dpTILActReProvidedDate,
        (System.Windows.Forms.Control) this.dpGFEActReReceivedDate,
        (System.Windows.Forms.Control) this.dpTILActReReceivedDate,
        (System.Windows.Forms.Control) this.dpClosingDate,
        (System.Windows.Forms.Control) this.dpActClosingDate,
        (System.Windows.Forms.Control) this.lBtnApplicationDate,
        (System.Windows.Forms.Control) this.dpTILAppDate,
        (System.Windows.Forms.Control) this.dpAffiliatedDate,
        (System.Windows.Forms.Control) this.dpCHARMDate,
        (System.Windows.Forms.Control) this.dpHUDSpecialDate,
        (System.Windows.Forms.Control) this.dpHELOCBrochureDate,
        (System.Windows.Forms.Control) this.dpAppraisalProvided,
        (System.Windows.Forms.Control) this.dpSubAppraisal,
        (System.Windows.Forms.Control) this.dpAVM,
        (System.Windows.Forms.Control) this.dpHomeCounseling
      };
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
      datePicker.ToolTip = FieldHelp.GetText(helpKey) == "" ? helpKey : helpKey + ": " + FieldHelp.GetText(helpKey);
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
                  datePicker.ReadOnly = str == "3142" && !this.loanData.IsLocked(str);
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
    }

    private void btnMarkInvalid_Click(object sender, EventArgs e)
    {
      if (this.ruleChecker.HasPrerequiredFields(this.loanData, "BUTTON_EXCLUDE FROM TIMELINE", true, (Hashtable) null) || this.loanData.GetLogList().CauseBorrowerIntentToCProceedViolation(this.currentLog.Guid, this.currentLog.DisclosedDate, false, this.currentLog.ReceivedDate) && Utils.Dialog((IWin32Window) this, "Exclude this disclosure tracking record might reset borrower intent to proceed field (3164).   Do you want to continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
        return;
      this.currentLog.IsDisclosed = false;
      this.gvHistory.SelectedItems[0].Tag = (object) this.currentLog;
      this.RefreshContents();
    }

    private void highlightSelectedLog()
    {
      if (this.currentLog == null)
        return;
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvHistory.Items)
      {
        if (this.currentLog == (DisclosureTrackingLog) gvItem.Tag)
        {
          gvItem.Selected = true;
          break;
        }
      }
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
      AdHocDisclosure adHocDisclosure = new AdHocDisclosure();
      if (DialogResult.Yes != adHocDisclosure.ShowDialog((IWin32Window) this))
        return;
      DisclosureTrackingLogUtils.WriteLog(new Dictionary<string, string>()
      {
        {
          "Location",
          "Manual"
        },
        {
          "ActualLog",
          DisclosureTrackingLogUtils.GetLog((IDisclosureTrackingLog) adHocDisclosure.Log)
        }
      }, Session.LoanDataMgr, Session.UserID, "", "", "", true, adHocDisclosure.Log.Guid);
      Session.LoanData.GetLogList().AddRecord((LogRecordBase) adHocDisclosure.Log);
      this.currentLog = adHocDisclosure.Log;
      this.RefreshContents();
    }

    private void refreshUpdatedItem()
    {
      if (this.gvHistory.SelectedItems.Count == 0 || this.suspendEvent)
        return;
      this.gvHistory.SelectedItems[0].SubItems[0].Value = (object) this.currentLog.DisclosedDate.ToString("MM/dd/yyyy");
      this.gvHistory.SelectedItems[0].SubItems[0].SortValue = (object) this.currentLog;
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
      if (this.ruleChecker.HasPrerequiredFields(this.loanData, "BUTTON_INCLUDE IN TIMELINE", true, (Hashtable) null) || this.loanData.GetLogList().CauseBorrowerIntentToCProceedViolation(this.currentLog.Guid, this.currentLog.DisclosedDate, true, this.currentLog.ReceivedDate) && Utils.Dialog((IWin32Window) this, "Include this disclosure tracking record might reset borrower intent to proceed field (3164).   Do you want to continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
        return;
      this.currentLog.IsDisclosed = true;
      this.gvHistory.SelectedItems[0].Tag = (object) this.currentLog;
      this.RefreshContents();
    }

    private void lblSnapshot_Click(object sender, EventArgs e)
    {
      int num = (int) new DisclosedHUDGFEDialog(this.currentLog).ShowDialog((IWin32Window) this);
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
      this.dpAppDate.ReadOnly = false;
      if (Utils.IsDate((object) field1))
        this.dpAppDate.Value = DateTime.Parse(field1);
      else
        this.dpAppDate.Text = "";
      if (this.loanData.IsLocked("3142"))
      {
        this.lBtnApplicationDate.Locked = true;
        this.dpAppDate.ReadOnly = false;
        if (Utils.IsDate((object) this.loanData.GetFieldFromCal("3142")))
          this.dpAppDate.MaxValue = Utils.ParseDate((object) this.loanData.GetFieldFromCal("3142"));
      }
      else
      {
        this.lBtnApplicationDate.Locked = false;
        this.dpAppDate.ReadOnly = true;
        this.dpAppDate.MaxValue = DateTime.MaxValue;
      }
      string field2 = this.loanData.GetField("3292");
      if (Utils.IsDate((object) field2))
        this.dpTILAppDate.Value = DateTime.Parse(field2);
      else
        this.dpTILAppDate.Text = "";
      string field3 = this.loanData.GetField("3143");
      if (Utils.IsDate((object) field3))
        this.dpInitialDisclosureDueDate.Value = DateTime.Parse(field3);
      else
        this.dpInitialDisclosureDueDate.Text = "";
      string field4 = this.loanData.GetField("3148");
      if (Utils.IsDate((object) field4))
        this.dpGFEActInitProvidedDate.Value = DateTime.Parse(field4);
      else
        this.dpGFEActInitProvidedDate.Text = "";
      string field5 = this.loanData.GetField("3152");
      if (Utils.IsDate((object) field5))
        this.dpTILActInitProvidedDate.Value = DateTime.Parse(field5);
      else
        this.dpTILActInitProvidedDate.Text = "";
      string field6 = this.loanData.GetField("3149");
      string field7 = this.loanData.GetField("3153");
      if (Utils.IsDate((object) field6))
        this.dpGFEActInitReceivedDate.Value = DateTime.Parse(field6);
      else
        this.dpGFEActInitReceivedDate.Text = "";
      if (Utils.IsDate((object) field7))
        this.dpTILActInitReceivedDate.Value = DateTime.Parse(field7);
      else
        this.dpTILActInitReceivedDate.Text = "";
      string field8 = this.loanData.GetField("3145");
      if (Utils.IsDate((object) field8))
        this.dpFeeCollectDate.Value = DateTime.Parse(field8);
      else
        this.dpFeeCollectDate.Text = "";
      string field9 = this.loanData.GetField("3150");
      string field10 = this.loanData.GetField("3154");
      if (Utils.IsDate((object) field9))
        this.dpGFEActReProvidedDate.Value = DateTime.Parse(field9);
      else
        this.dpGFEActReProvidedDate.Text = "";
      if (Utils.IsDate((object) field10))
        this.dpTILActReProvidedDate.Value = DateTime.Parse(field10);
      else
        this.dpTILActReProvidedDate.Text = "";
      string field11 = this.loanData.GetField("3151");
      string field12 = this.loanData.GetField("3155");
      if (Utils.IsDate((object) field11))
        this.dpGFEActReReceivedDate.Value = DateTime.Parse(field11);
      else
        this.dpGFEActReReceivedDate.Text = "";
      if (Utils.IsDate((object) field12))
        this.dpTILActReReceivedDate.Value = DateTime.Parse(field12);
      else
        this.dpTILActReReceivedDate.Text = "";
      string field13 = this.loanData.GetField("3147");
      if (Utils.IsDate((object) field13))
        this.dpClosingDate.Value = DateTime.Parse(field13);
      else
        this.dpClosingDate.Text = "";
      string field14 = this.loanData.GetField("763");
      if (Utils.IsDate((object) field14))
        this.dpActClosingDate.Value = DateTime.Parse(field14);
      else
        this.dpActClosingDate.Text = "";
      string field15 = this.loanData.GetField("3544");
      if (Utils.IsDate((object) field15))
        this.dpAffiliatedDate.Value = DateTime.Parse(field15);
      else
        this.dpAffiliatedDate.Text = "";
      string field16 = this.loanData.GetField("3545");
      if (Utils.IsDate((object) field16))
        this.dpCHARMDate.Value = DateTime.Parse(field16);
      else
        this.dpCHARMDate.Text = "";
      string field17 = this.loanData.GetField("3546");
      if (Utils.IsDate((object) field17))
        this.dpHUDSpecialDate.Value = DateTime.Parse(field17);
      else
        this.dpHUDSpecialDate.Text = "";
      string field18 = this.loanData.GetField("3547");
      if (Utils.IsDate((object) field18))
        this.dpHELOCBrochureDate.Value = DateTime.Parse(field18);
      else
        this.dpHELOCBrochureDate.Text = "";
      string field19 = this.loanData.GetField("3624");
      if (Utils.IsDate((object) field19))
        this.dpAppraisalProvided.Value = DateTime.Parse(field19);
      else
        this.dpAppraisalProvided.Text = "";
      string field20 = this.loanData.GetField("3857");
      if (Utils.IsDate((object) field20))
        this.dpSubAppraisal.Value = DateTime.Parse(field20);
      else
        this.dpSubAppraisal.Text = "";
      string field21 = this.loanData.GetField("3858");
      if (Utils.IsDate((object) field21))
        this.dpAVM.Value = DateTime.Parse(field21);
      else
        this.dpAVM.Text = "";
      string field22 = this.loanData.GetField("3859");
      if (Utils.IsDate((object) field22))
        this.dpHomeCounseling.Value = DateTime.Parse(field22);
      else
        this.dpHomeCounseling.Text = "";
      this.txtTimezone.Text = this.loanData.GetField("LE1.XG9") == "" ? this.loanData.GetField("LE1.X9") : this.loanData.GetField("LE1.XG9");
      this.applyFieldAccessRights();
      this.suspendEvent = false;
    }

    private void freeEntryDate_ValueChanged(object sender, EventArgs e)
    {
      if (this.suspendEvent)
        return;
      DatePicker datePicker = (DatePicker) sender;
      try
      {
        if (datePicker.Value != DateTime.MinValue)
          this.loanData.SetField(string.Concat(datePicker.Tag), datePicker.Value.ToString("MM/dd/yyyy"));
        else
          this.loanData.SetField(string.Concat(datePicker.Tag), "");
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, ex.InnerException.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.loanData.SetField(string.Concat(datePicker.Tag), "");
        datePicker.Text = "";
      }
      this.updateBusinessRule();
    }

    private void lBtnApplicationDate_Click(object sender, EventArgs e)
    {
      this.lBtnApplicationDate.Locked = !this.lBtnApplicationDate.Locked;
      this.dpAppDate.ReadOnly = !this.lBtnApplicationDate.Locked;
      if (!this.dpAppDate.ReadOnly)
      {
        this.loanData.AddLock("3142");
        if (Utils.IsDate((object) this.loanData.GetFieldFromCal("3142")))
          this.dpAppDate.MaxValue = Utils.ParseDate((object) this.loanData.GetFieldFromCal("3142"));
      }
      else
        this.loanData.RemoveLock("3142");
      this.reloadTimeline();
    }

    private void dpAppDate_ValueChanged(object sender, EventArgs e)
    {
      try
      {
        if (this.dpAppDate.Value != DateTime.MinValue)
          this.loanData.SetField("3142", this.dpAppDate.Value.ToString("MM/dd/yyyy"));
        else
          this.loanData.SetField("3142", "//");
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, ex.InnerException.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.loanData.SetField("3142", "//");
        this.dpAppDate.Text = "";
      }
      this.reloadTimeline();
      this.updateBusinessRule();
    }

    private void panel1_SizeChanged(object sender, EventArgs e)
    {
      GroupContainer gcCompliance = this.gcCompliance;
      GroupContainer gcTil = this.gcTIL;
      GroupContainer gcGfe = this.gcGFE;
      Size size1 = new Size(this.panel1.Width / 4, this.gcGFE.Height);
      Size size2 = size1;
      gcGfe.Size = size2;
      Size size3;
      Size size4 = size3 = size1;
      gcTil.Size = size3;
      Size size5 = size4;
      gcCompliance.Size = size5;
    }

    public string GetHelpTargetName() => "Disclosure Tracking";

    private void dpImportantDate_Click(object sender, EventArgs e)
    {
      if (!(sender is DatePicker))
        return;
      DatePicker datePicker = (DatePicker) sender;
      Session.Application.GetService<IStatusDisplay>().DisplayFieldID(string.Concat(datePicker.Tag));
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

    private void btnSafeHarborSnapshot_Click(object sender, EventArgs e)
    {
      int num = (int) new DisclosedSafeHarborDialog((IDisclosureTrackingLog) this.currentLog).ShowDialog((IWin32Window) this);
    }

    private void btnZoomIn_Click(object sender, EventArgs e) => this.viewDisclosureDetails();

    private void viewDisclosureDetails()
    {
      if (this.gvHistory.SelectedItems.Count == 0)
        return;
      DisclosureTrackingLog tag = (DisclosureTrackingLog) this.gvHistory.SelectedItems[0].Tag;
      int num = (int) new DisclosureDetailsDialog(this.currentLog, this.hasAccessRight).ShowDialog();
      this.RefreshContents();
    }

    private void gvHistory_DoubleClick(object sender, EventArgs e) => this.viewDisclosureDetails();

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
      this.toolTipField = new ToolTip(this.components);
      this.btnAdd = new StandardIconButton();
      this.groupContainer1 = new GroupContainer();
      this.panel2 = new System.Windows.Forms.Panel();
      this.gcHistory = new GroupContainer();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.btnMarkValid = new System.Windows.Forms.Button();
      this.btnMarkInvalid = new System.Windows.Forms.Button();
      this.verticalSeparator1 = new VerticalSeparator();
      this.btnZoomIn = new StandardIconButton();
      this.gvHistory = new GridView();
      this.splitter2 = new Splitter();
      this.panel1 = new System.Windows.Forms.Panel();
      this.groupContainer3 = new GroupContainer();
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
      this.gcGFE = new GroupContainer();
      this.label13 = new System.Windows.Forms.Label();
      this.label12 = new System.Windows.Forms.Label();
      this.label11 = new System.Windows.Forms.Label();
      this.label9 = new System.Windows.Forms.Label();
      this.dpGFEActInitProvidedDate = new DatePicker();
      this.dpGFEActInitReceivedDate = new DatePicker();
      this.dpGFEActReProvidedDate = new DatePicker();
      this.dpGFEActReReceivedDate = new DatePicker();
      this.emHelpLink3 = new EMHelpLink();
      this.splitter4 = new Splitter();
      this.gcTIL = new GroupContainer();
      this.emHelpLink2 = new EMHelpLink();
      this.label3 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.label6 = new System.Windows.Forms.Label();
      this.label10 = new System.Windows.Forms.Label();
      this.dpTILActReReceivedDate = new DatePicker();
      this.dpTILActInitProvidedDate = new DatePicker();
      this.dpTILActReProvidedDate = new DatePicker();
      this.dpTILActInitReceivedDate = new DatePicker();
      this.splitter3 = new Splitter();
      this.splitter1 = new Splitter();
      this.gcCompliance = new GroupContainer();
      this.dpTILAppDate = new DatePicker();
      this.label24 = new System.Windows.Forms.Label();
      this.emHelpLink1 = new EMHelpLink();
      this.lBtnApplicationDate = new FieldLockButton();
      this.dpInitialDisclosureDueDate = new DatePicker();
      this.label2 = new System.Windows.Forms.Label();
      this.dpClosingDate = new DatePicker();
      this.label7 = new System.Windows.Forms.Label();
      this.dpActClosingDate = new DatePicker();
      this.dpFeeCollectDate = new DatePicker();
      this.label5 = new System.Windows.Forms.Label();
      this.dpAppDate = new DatePicker();
      this.label1 = new System.Windows.Forms.Label();
      this.label8 = new System.Windows.Forms.Label();
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
      this.gcGFE.SuspendLayout();
      this.gcTIL.SuspendLayout();
      this.gcCompliance.SuspendLayout();
      this.SuspendLayout();
      this.btnAdd.BackColor = Color.Transparent;
      this.btnAdd.Location = new Point(30, 4);
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
      this.groupContainer1.Text = "Disclosure Tracking";
      this.panel2.Controls.Add((System.Windows.Forms.Control) this.gcHistory);
      this.panel2.Dock = DockStyle.Fill;
      this.panel2.Location = new Point(1, 275);
      this.panel2.Name = "panel2";
      this.panel2.Padding = new Padding(3, 0, 3, 0);
      this.panel2.Size = new Size(1074, 359);
      this.panel2.TabIndex = 2;
      this.gcHistory.Controls.Add((System.Windows.Forms.Control) this.flowLayoutPanel1);
      this.gcHistory.Controls.Add((System.Windows.Forms.Control) this.gvHistory);
      this.gcHistory.Dock = DockStyle.Fill;
      this.gcHistory.HeaderForeColor = SystemColors.ControlText;
      this.gcHistory.Location = new Point(3, 0);
      this.gcHistory.Name = "gcHistory";
      this.gcHistory.Size = new Size(1068, 359);
      this.gcHistory.TabIndex = 0;
      this.gcHistory.Text = "Disclosure History";
      this.flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flowLayoutPanel1.BackColor = Color.Transparent;
      this.flowLayoutPanel1.Controls.Add((System.Windows.Forms.Control) this.btnMarkValid);
      this.flowLayoutPanel1.Controls.Add((System.Windows.Forms.Control) this.btnMarkInvalid);
      this.flowLayoutPanel1.Controls.Add((System.Windows.Forms.Control) this.verticalSeparator1);
      this.flowLayoutPanel1.Controls.Add((System.Windows.Forms.Control) this.btnZoomIn);
      this.flowLayoutPanel1.Controls.Add((System.Windows.Forms.Control) this.btnAdd);
      this.flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel1.Location = new Point(741, 1);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Padding = new Padding(0, 0, 5, 0);
      this.flowLayoutPanel1.Size = new Size(327, 24);
      this.flowLayoutPanel1.TabIndex = 6;
      this.btnMarkValid.Location = new Point(212, 1);
      this.btnMarkValid.Margin = new Padding(0, 1, 0, 0);
      this.btnMarkValid.Name = "btnMarkValid";
      this.btnMarkValid.Size = new Size(110, 22);
      this.btnMarkValid.TabIndex = 7;
      this.btnMarkValid.Text = "Include in Timeline";
      this.btnMarkValid.UseVisualStyleBackColor = true;
      this.btnMarkValid.Click += new EventHandler(this.btnMarkValid_Click);
      this.btnMarkInvalid.Location = new Point(79, 1);
      this.btnMarkInvalid.Margin = new Padding(0, 1, 0, 0);
      this.btnMarkInvalid.Name = "btnMarkInvalid";
      this.btnMarkInvalid.Size = new Size(133, 22);
      this.btnMarkInvalid.TabIndex = 5;
      this.btnMarkInvalid.Text = "Exclude from Timeline";
      this.btnMarkInvalid.UseVisualStyleBackColor = true;
      this.btnMarkInvalid.Click += new EventHandler(this.btnMarkInvalid_Click);
      this.verticalSeparator1.Location = new Point(74, 4);
      this.verticalSeparator1.Margin = new Padding(3, 4, 3, 3);
      this.verticalSeparator1.MaximumSize = new Size(2, 16);
      this.verticalSeparator1.MinimumSize = new Size(2, 16);
      this.verticalSeparator1.Name = "verticalSeparator1";
      this.verticalSeparator1.Size = new Size(2, 16);
      this.verticalSeparator1.TabIndex = 6;
      this.verticalSeparator1.Text = "verticalSeparator1";
      this.btnZoomIn.BackColor = Color.Transparent;
      this.btnZoomIn.Location = new Point(52, 3);
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
      gvColumn1.SortMethod = GVSortMethod.Custom;
      gvColumn1.Text = "Sent Date";
      gvColumn1.Width = 100;
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
      gvColumn4.Text = "# of Disclosed Doc";
      gvColumn4.Width = 70;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column5";
      gvColumn5.Text = "GFE Sent?";
      gvColumn5.Width = 60;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Column6";
      gvColumn6.Text = "TIL Sent?";
      gvColumn6.Width = 60;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "Column9";
      gvColumn7.Text = "Safe Harbor Sent?";
      gvColumn7.Width = 104;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "Column7";
      gvColumn8.Text = "Borrower Pair";
      gvColumn8.Width = 100;
      gvColumn9.ImageIndex = -1;
      gvColumn9.Name = "Column8";
      gvColumn9.Text = "Included in Timeline";
      gvColumn9.Width = 109;
      this.gvHistory.Columns.AddRange(new GVColumn[9]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6,
        gvColumn7,
        gvColumn8,
        gvColumn9
      });
      this.gvHistory.Dock = DockStyle.Fill;
      this.gvHistory.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvHistory.Location = new Point(1, 26);
      this.gvHistory.Name = "gvHistory";
      this.gvHistory.Size = new Size(1066, 332);
      this.gvHistory.TabIndex = 0;
      this.gvHistory.SelectedIndexChanged += new EventHandler(this.gvHistory_SelectedIndexChanged);
      this.gvHistory.DoubleClick += new EventHandler(this.gvHistory_DoubleClick);
      this.splitter2.Dock = DockStyle.Top;
      this.splitter2.Location = new Point(1, 272);
      this.splitter2.Name = "splitter2";
      this.splitter2.Size = new Size(1074, 3);
      this.splitter2.TabIndex = 1;
      this.splitter2.TabStop = false;
      this.panel1.Controls.Add((System.Windows.Forms.Control) this.groupContainer3);
      this.panel1.Controls.Add((System.Windows.Forms.Control) this.splitter5);
      this.panel1.Controls.Add((System.Windows.Forms.Control) this.gcGFE);
      this.panel1.Controls.Add((System.Windows.Forms.Control) this.splitter4);
      this.panel1.Controls.Add((System.Windows.Forms.Control) this.gcTIL);
      this.panel1.Controls.Add((System.Windows.Forms.Control) this.splitter3);
      this.panel1.Controls.Add((System.Windows.Forms.Control) this.splitter1);
      this.panel1.Controls.Add((System.Windows.Forms.Control) this.gcCompliance);
      this.panel1.Dock = DockStyle.Top;
      this.panel1.Location = new Point(1, 26);
      this.panel1.Name = "panel1";
      this.panel1.Padding = new Padding(3, 3, 3, 0);
      this.panel1.Size = new Size(1074, 246);
      this.panel1.TabIndex = 0;
      this.panel1.SizeChanged += new EventHandler(this.panel1_SizeChanged);
      this.groupContainer3.AutoScroll = true;
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
      this.groupContainer3.Size = new Size(294, 243);
      this.groupContainer3.TabIndex = 8;
      this.groupContainer3.Text = "Other Tracking";
      this.dpHomeCounseling.BackColor = SystemColors.Window;
      this.dpHomeCounseling.Location = new Point(210, 201);
      this.dpHomeCounseling.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dpHomeCounseling.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpHomeCounseling.Name = "dpHomeCounseling";
      this.dpHomeCounseling.Size = new Size(85, 22);
      this.dpHomeCounseling.TabIndex = 15;
      this.dpHomeCounseling.Tag = (object) "3859";
      this.dpHomeCounseling.ToolTip = "";
      this.dpHomeCounseling.Value = new DateTime(0L);
      this.dpHomeCounseling.ValueChanged += new EventHandler(this.freeEntryDate_ValueChanged);
      this.dpHomeCounseling.Click += new EventHandler(this.dpImportantDate_Click);
      this.label21.AutoSize = true;
      this.label21.Location = new Point(7, 205);
      this.label21.Name = "label21";
      this.label21.Size = new Size(189, 14);
      this.label21.TabIndex = 14;
      this.label21.Text = "Home Counseling Disclosure Provided";
      this.dpAVM.BackColor = SystemColors.Window;
      this.dpAVM.Location = new Point(210, 177);
      this.dpAVM.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dpAVM.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpAVM.Name = "dpAVM";
      this.dpAVM.Size = new Size(85, 22);
      this.dpAVM.TabIndex = 13;
      this.dpAVM.Tag = (object) "3858";
      this.dpAVM.ToolTip = "";
      this.dpAVM.Value = new DateTime(0L);
      this.dpAVM.ValueChanged += new EventHandler(this.freeEntryDate_ValueChanged);
      this.dpAVM.Click += new EventHandler(this.dpImportantDate_Click);
      this.label20.AutoSize = true;
      this.label20.Location = new Point(7, 181);
      this.label20.Name = "label20";
      this.label20.Size = new Size(75, 14);
      this.label20.TabIndex = 12;
      this.label20.Text = "AVM Provided";
      this.dpSubAppraisal.BackColor = SystemColors.Window;
      this.dpSubAppraisal.Location = new Point(210, 153);
      this.dpSubAppraisal.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dpSubAppraisal.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpSubAppraisal.Name = "dpSubAppraisal";
      this.dpSubAppraisal.Size = new Size(85, 22);
      this.dpSubAppraisal.TabIndex = 11;
      this.dpSubAppraisal.Tag = (object) "3857";
      this.dpSubAppraisal.ToolTip = "";
      this.dpSubAppraisal.Value = new DateTime(0L);
      this.dpSubAppraisal.ValueChanged += new EventHandler(this.freeEntryDate_ValueChanged);
      this.dpSubAppraisal.Click += new EventHandler(this.dpImportantDate_Click);
      this.label19.AutoSize = true;
      this.label19.Location = new Point(7, 157);
      this.label19.Name = "label19";
      this.label19.Size = new Size(158, 14);
      this.label19.TabIndex = 10;
      this.label19.Text = "Subsequent Appraisal Provided";
      this.dpAppraisalProvided.BackColor = SystemColors.Window;
      this.dpAppraisalProvided.Location = new Point(210, 129);
      this.dpAppraisalProvided.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dpAppraisalProvided.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpAppraisalProvided.Name = "dpAppraisalProvided";
      this.dpAppraisalProvided.Size = new Size(85, 22);
      this.dpAppraisalProvided.TabIndex = 9;
      this.dpAppraisalProvided.Tag = (object) "3624";
      this.dpAppraisalProvided.ToolTip = "";
      this.dpAppraisalProvided.Value = new DateTime(0L);
      this.dpAppraisalProvided.ValueChanged += new EventHandler(this.freeEntryDate_ValueChanged);
      this.dpAppraisalProvided.Click += new EventHandler(this.dpImportantDate_Click);
      this.label18.AutoSize = true;
      this.label18.Location = new Point(7, 133);
      this.label18.Name = "label18";
      this.label18.Size = new Size(115, 14);
      this.label18.TabIndex = 8;
      this.label18.Text = "1st Appraisal Provided";
      this.dpAffiliatedDate.BackColor = SystemColors.Window;
      this.dpAffiliatedDate.Location = new Point(210, 33);
      this.dpAffiliatedDate.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dpAffiliatedDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpAffiliatedDate.Name = "dpAffiliatedDate";
      this.dpAffiliatedDate.Size = new Size(85, 22);
      this.dpAffiliatedDate.TabIndex = 1;
      this.dpAffiliatedDate.Tag = (object) "3544";
      this.dpAffiliatedDate.ToolTip = "";
      this.dpAffiliatedDate.Value = new DateTime(0L);
      this.dpAffiliatedDate.ValueChanged += new EventHandler(this.freeEntryDate_ValueChanged);
      this.dpAffiliatedDate.Click += new EventHandler(this.dpImportantDate_Click);
      this.label14.AutoSize = true;
      this.label14.Location = new Point(7, 37);
      this.label14.Name = "label14";
      this.label14.Size = new Size(197, 14);
      this.label14.TabIndex = 0;
      this.label14.Text = "Affiliated Business Disclosure Provided";
      this.dpHELOCBrochureDate.BackColor = SystemColors.Window;
      this.dpHELOCBrochureDate.Location = new Point(210, 105);
      this.dpHELOCBrochureDate.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dpHELOCBrochureDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpHELOCBrochureDate.Name = "dpHELOCBrochureDate";
      this.dpHELOCBrochureDate.Size = new Size(85, 22);
      this.dpHELOCBrochureDate.TabIndex = 7;
      this.dpHELOCBrochureDate.Tag = (object) "3547";
      this.dpHELOCBrochureDate.ToolTip = "";
      this.dpHELOCBrochureDate.Value = new DateTime(0L);
      this.dpHELOCBrochureDate.ValueChanged += new EventHandler(this.freeEntryDate_ValueChanged);
      this.dpHELOCBrochureDate.Click += new EventHandler(this.dpImportantDate_Click);
      this.label15.AutoSize = true;
      this.label15.Location = new Point(7, 61);
      this.label15.Name = "label15";
      this.label15.Size = new Size(130, 14);
      this.label15.TabIndex = 2;
      this.label15.Text = "CHARM Booklet Provided ";
      this.dpHUDSpecialDate.BackColor = SystemColors.Window;
      this.dpHUDSpecialDate.Location = new Point(210, 81);
      this.dpHUDSpecialDate.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dpHUDSpecialDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpHUDSpecialDate.Name = "dpHUDSpecialDate";
      this.dpHUDSpecialDate.Size = new Size(85, 22);
      this.dpHUDSpecialDate.TabIndex = 5;
      this.dpHUDSpecialDate.Tag = (object) "3546";
      this.dpHUDSpecialDate.ToolTip = "";
      this.dpHUDSpecialDate.Value = new DateTime(0L);
      this.dpHUDSpecialDate.ValueChanged += new EventHandler(this.freeEntryDate_ValueChanged);
      this.dpHUDSpecialDate.Click += new EventHandler(this.dpImportantDate_Click);
      this.label16.AutoSize = true;
      this.label16.Location = new Point(7, 85);
      this.label16.Name = "label16";
      this.label16.Size = new Size(149, 14);
      this.label16.TabIndex = 4;
      this.label16.Text = "HUD Special Booklet Provided";
      this.dpCHARMDate.BackColor = SystemColors.Window;
      this.dpCHARMDate.Location = new Point(210, 57);
      this.dpCHARMDate.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dpCHARMDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpCHARMDate.Name = "dpCHARMDate";
      this.dpCHARMDate.Size = new Size(85, 22);
      this.dpCHARMDate.TabIndex = 3;
      this.dpCHARMDate.Tag = (object) "3545";
      this.dpCHARMDate.ToolTip = "";
      this.dpCHARMDate.Value = new DateTime(0L);
      this.dpCHARMDate.ValueChanged += new EventHandler(this.freeEntryDate_ValueChanged);
      this.dpCHARMDate.Click += new EventHandler(this.dpImportantDate_Click);
      this.label17.AutoSize = true;
      this.label17.Location = new Point(7, 109);
      this.label17.Name = "label17";
      this.label17.Size = new Size(134, 14);
      this.label17.TabIndex = 6;
      this.label17.Text = "HELOC Brochure Provided";
      this.splitter5.Location = new Point(771, 3);
      this.splitter5.Name = "splitter5";
      this.splitter5.Size = new Size(3, 243);
      this.splitter5.TabIndex = 7;
      this.splitter5.TabStop = false;
      this.gcGFE.AutoScroll = true;
      this.gcGFE.Controls.Add((System.Windows.Forms.Control) this.label13);
      this.gcGFE.Controls.Add((System.Windows.Forms.Control) this.label12);
      this.gcGFE.Controls.Add((System.Windows.Forms.Control) this.label11);
      this.gcGFE.Controls.Add((System.Windows.Forms.Control) this.label9);
      this.gcGFE.Controls.Add((System.Windows.Forms.Control) this.dpGFEActInitProvidedDate);
      this.gcGFE.Controls.Add((System.Windows.Forms.Control) this.dpGFEActInitReceivedDate);
      this.gcGFE.Controls.Add((System.Windows.Forms.Control) this.dpGFEActReProvidedDate);
      this.gcGFE.Controls.Add((System.Windows.Forms.Control) this.dpGFEActReReceivedDate);
      this.gcGFE.Controls.Add((System.Windows.Forms.Control) this.emHelpLink3);
      this.gcGFE.Dock = DockStyle.Left;
      this.gcGFE.HeaderForeColor = SystemColors.ControlText;
      this.gcGFE.Location = new Point(520, 3);
      this.gcGFE.Name = "gcGFE";
      this.gcGFE.Size = new Size(251, 243);
      this.gcGFE.TabIndex = 6;
      this.gcGFE.Text = "GFE Tracking";
      this.label13.AutoSize = true;
      this.label13.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label13.Location = new Point(7, 37);
      this.label13.Name = "label13";
      this.label13.Size = new Size(103, 14);
      this.label13.TabIndex = 0;
      this.label13.Text = "Initial GFE Sent Date";
      this.label12.AutoSize = true;
      this.label12.Location = new Point(7, 61);
      this.label12.Name = "label12";
      this.label12.Size = new Size((int) sbyte.MaxValue, 14);
      this.label12.TabIndex = 2;
      this.label12.Text = "Borrower Received Date";
      this.label11.AutoSize = true;
      this.label11.Location = new Point(7, 85);
      this.label11.Name = "label11";
      this.label11.Size = new Size(139, 14);
      this.label11.TabIndex = 4;
      this.label11.Text = "Revised GFE Provided Date";
      this.label9.AutoSize = true;
      this.label9.Location = new Point(7, 109);
      this.label9.Name = "label9";
      this.label9.Size = new Size((int) sbyte.MaxValue, 14);
      this.label9.TabIndex = 6;
      this.label9.Text = "Borrower Received Date";
      this.dpGFEActInitProvidedDate.BackColor = SystemColors.Window;
      this.dpGFEActInitProvidedDate.Location = new Point(159, 33);
      this.dpGFEActInitProvidedDate.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.dpGFEActInitProvidedDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpGFEActInitProvidedDate.Name = "dpGFEActInitProvidedDate";
      this.dpGFEActInitProvidedDate.ReadOnly = true;
      this.dpGFEActInitProvidedDate.Size = new Size(75, 22);
      this.dpGFEActInitProvidedDate.TabIndex = 1;
      this.dpGFEActInitProvidedDate.Tag = (object) "3148";
      this.dpGFEActInitProvidedDate.ToolTip = "";
      this.dpGFEActInitProvidedDate.Value = new DateTime(0L);
      this.dpGFEActInitProvidedDate.Click += new EventHandler(this.dpImportantDate_Click);
      this.dpGFEActInitReceivedDate.BackColor = SystemColors.Window;
      this.dpGFEActInitReceivedDate.Location = new Point(159, 57);
      this.dpGFEActInitReceivedDate.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.dpGFEActInitReceivedDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpGFEActInitReceivedDate.Name = "dpGFEActInitReceivedDate";
      this.dpGFEActInitReceivedDate.ReadOnly = true;
      this.dpGFEActInitReceivedDate.Size = new Size(75, 22);
      this.dpGFEActInitReceivedDate.TabIndex = 3;
      this.dpGFEActInitReceivedDate.Tag = (object) "3149";
      this.dpGFEActInitReceivedDate.ToolTip = "";
      this.dpGFEActInitReceivedDate.Value = new DateTime(0L);
      this.dpGFEActInitReceivedDate.Click += new EventHandler(this.dpImportantDate_Click);
      this.dpGFEActReProvidedDate.BackColor = SystemColors.Window;
      this.dpGFEActReProvidedDate.Location = new Point(159, 81);
      this.dpGFEActReProvidedDate.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.dpGFEActReProvidedDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpGFEActReProvidedDate.Name = "dpGFEActReProvidedDate";
      this.dpGFEActReProvidedDate.ReadOnly = true;
      this.dpGFEActReProvidedDate.Size = new Size(75, 22);
      this.dpGFEActReProvidedDate.TabIndex = 5;
      this.dpGFEActReProvidedDate.Tag = (object) "3150";
      this.dpGFEActReProvidedDate.ToolTip = "";
      this.dpGFEActReProvidedDate.Value = new DateTime(0L);
      this.dpGFEActReProvidedDate.Click += new EventHandler(this.dpImportantDate_Click);
      this.dpGFEActReReceivedDate.BackColor = SystemColors.Window;
      this.dpGFEActReReceivedDate.Location = new Point(159, 105);
      this.dpGFEActReReceivedDate.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.dpGFEActReReceivedDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpGFEActReReceivedDate.Name = "dpGFEActReReceivedDate";
      this.dpGFEActReReceivedDate.ReadOnly = true;
      this.dpGFEActReReceivedDate.Size = new Size(75, 22);
      this.dpGFEActReReceivedDate.TabIndex = 7;
      this.dpGFEActReReceivedDate.Tag = (object) "3151";
      this.dpGFEActReReceivedDate.ToolTip = "";
      this.dpGFEActReReceivedDate.Value = new DateTime(0L);
      this.dpGFEActReReceivedDate.Click += new EventHandler(this.dpImportantDate_Click);
      this.emHelpLink3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.emHelpLink3.BackColor = Color.Transparent;
      this.emHelpLink3.Cursor = Cursors.Hand;
      this.emHelpLink3.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.emHelpLink3.HelpTag = "Disclosure Tracking GFE Tracking";
      this.emHelpLink3.Location = new Point(216, 5);
      this.emHelpLink3.Name = "emHelpLink3";
      this.emHelpLink3.Size = new Size(19, 26);
      this.emHelpLink3.TabIndex = 44;
      this.splitter4.Location = new Point(517, 3);
      this.splitter4.Name = "splitter4";
      this.splitter4.Size = new Size(3, 243);
      this.splitter4.TabIndex = 5;
      this.splitter4.TabStop = false;
      this.gcTIL.AutoScroll = true;
      this.gcTIL.Controls.Add((System.Windows.Forms.Control) this.emHelpLink2);
      this.gcTIL.Controls.Add((System.Windows.Forms.Control) this.label3);
      this.gcTIL.Controls.Add((System.Windows.Forms.Control) this.label4);
      this.gcTIL.Controls.Add((System.Windows.Forms.Control) this.label6);
      this.gcTIL.Controls.Add((System.Windows.Forms.Control) this.label10);
      this.gcTIL.Controls.Add((System.Windows.Forms.Control) this.dpTILActReReceivedDate);
      this.gcTIL.Controls.Add((System.Windows.Forms.Control) this.dpTILActInitProvidedDate);
      this.gcTIL.Controls.Add((System.Windows.Forms.Control) this.dpTILActReProvidedDate);
      this.gcTIL.Controls.Add((System.Windows.Forms.Control) this.dpTILActInitReceivedDate);
      this.gcTIL.Dock = DockStyle.Left;
      this.gcTIL.HeaderForeColor = SystemColors.ControlText;
      this.gcTIL.Location = new Point(266, 3);
      this.gcTIL.Name = "gcTIL";
      this.gcTIL.Size = new Size(251, 243);
      this.gcTIL.TabIndex = 3;
      this.gcTIL.Text = "TIL Tracking";
      this.emHelpLink2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.emHelpLink2.BackColor = Color.Transparent;
      this.emHelpLink2.Cursor = Cursors.Hand;
      this.emHelpLink2.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.emHelpLink2.HelpTag = "Disclosure Tracking TIL Tracking";
      this.emHelpLink2.Location = new Point(223, 5);
      this.emHelpLink2.Name = "emHelpLink2";
      this.emHelpLink2.Size = new Size(19, 26);
      this.emHelpLink2.TabIndex = 52;
      this.label3.Location = new Point(7, 109);
      this.label3.Name = "label3";
      this.label3.Size = new Size(138, 17);
      this.label3.TabIndex = 6;
      this.label3.Text = "Borrower Received Date";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(7, 85);
      this.label4.Name = "label4";
      this.label4.Size = new Size(133, 14);
      this.label4.TabIndex = 4;
      this.label4.Text = "Revised TIL Provided Date";
      this.label6.AutoSize = true;
      this.label6.Location = new Point(7, 61);
      this.label6.Name = "label6";
      this.label6.Size = new Size((int) sbyte.MaxValue, 14);
      this.label6.TabIndex = 2;
      this.label6.Text = "Borrower Received Date";
      this.label10.AutoSize = true;
      this.label10.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label10.Location = new Point(7, 37);
      this.label10.Name = "label10";
      this.label10.Size = new Size(97, 14);
      this.label10.TabIndex = 0;
      this.label10.Text = "Initial TIL Sent Date";
      this.dpTILActReReceivedDate.BackColor = SystemColors.Window;
      this.dpTILActReReceivedDate.Location = new Point(155, 105);
      this.dpTILActReReceivedDate.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.dpTILActReReceivedDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpTILActReReceivedDate.Name = "dpTILActReReceivedDate";
      this.dpTILActReReceivedDate.ReadOnly = true;
      this.dpTILActReReceivedDate.Size = new Size(75, 22);
      this.dpTILActReReceivedDate.TabIndex = 7;
      this.dpTILActReReceivedDate.Tag = (object) "3155";
      this.dpTILActReReceivedDate.ToolTip = "";
      this.dpTILActReReceivedDate.Value = new DateTime(0L);
      this.dpTILActReReceivedDate.Click += new EventHandler(this.dpImportantDate_Click);
      this.dpTILActInitProvidedDate.BackColor = SystemColors.Window;
      this.dpTILActInitProvidedDate.Location = new Point(155, 33);
      this.dpTILActInitProvidedDate.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.dpTILActInitProvidedDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpTILActInitProvidedDate.Name = "dpTILActInitProvidedDate";
      this.dpTILActInitProvidedDate.ReadOnly = true;
      this.dpTILActInitProvidedDate.Size = new Size(75, 22);
      this.dpTILActInitProvidedDate.TabIndex = 1;
      this.dpTILActInitProvidedDate.Tag = (object) "3152";
      this.dpTILActInitProvidedDate.ToolTip = "";
      this.dpTILActInitProvidedDate.Value = new DateTime(0L);
      this.dpTILActInitProvidedDate.Click += new EventHandler(this.dpImportantDate_Click);
      this.dpTILActReProvidedDate.BackColor = SystemColors.Window;
      this.dpTILActReProvidedDate.Location = new Point(155, 81);
      this.dpTILActReProvidedDate.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.dpTILActReProvidedDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpTILActReProvidedDate.Name = "dpTILActReProvidedDate";
      this.dpTILActReProvidedDate.ReadOnly = true;
      this.dpTILActReProvidedDate.Size = new Size(75, 22);
      this.dpTILActReProvidedDate.TabIndex = 5;
      this.dpTILActReProvidedDate.Tag = (object) "3154";
      this.dpTILActReProvidedDate.ToolTip = "";
      this.dpTILActReProvidedDate.Value = new DateTime(0L);
      this.dpTILActReProvidedDate.Click += new EventHandler(this.dpImportantDate_Click);
      this.dpTILActInitReceivedDate.BackColor = SystemColors.Window;
      this.dpTILActInitReceivedDate.Location = new Point(155, 57);
      this.dpTILActInitReceivedDate.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.dpTILActInitReceivedDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpTILActInitReceivedDate.Name = "dpTILActInitReceivedDate";
      this.dpTILActInitReceivedDate.ReadOnly = true;
      this.dpTILActInitReceivedDate.Size = new Size(75, 22);
      this.dpTILActInitReceivedDate.TabIndex = 3;
      this.dpTILActInitReceivedDate.Tag = (object) "3153";
      this.dpTILActInitReceivedDate.ToolTip = "";
      this.dpTILActInitReceivedDate.Value = new DateTime(0L);
      this.dpTILActInitReceivedDate.Click += new EventHandler(this.dpImportantDate_Click);
      this.splitter3.Dock = DockStyle.Right;
      this.splitter3.Location = new Point(1068, 3);
      this.splitter3.Name = "splitter3";
      this.splitter3.Size = new Size(3, 243);
      this.splitter3.TabIndex = 4;
      this.splitter3.TabStop = false;
      this.splitter1.Location = new Point(263, 3);
      this.splitter1.Name = "splitter1";
      this.splitter1.Size = new Size(3, 243);
      this.splitter1.TabIndex = 2;
      this.splitter1.TabStop = false;
      this.gcCompliance.AutoScroll = true;
      this.gcCompliance.Controls.Add((System.Windows.Forms.Control) this.txtTimezone);
      this.gcCompliance.Controls.Add((System.Windows.Forms.Control) this.lblTimezone);
      this.gcCompliance.Controls.Add((System.Windows.Forms.Control) this.dpTILAppDate);
      this.gcCompliance.Controls.Add((System.Windows.Forms.Control) this.label24);
      this.gcCompliance.Controls.Add((System.Windows.Forms.Control) this.emHelpLink1);
      this.gcCompliance.Controls.Add((System.Windows.Forms.Control) this.lBtnApplicationDate);
      this.gcCompliance.Controls.Add((System.Windows.Forms.Control) this.dpInitialDisclosureDueDate);
      this.gcCompliance.Controls.Add((System.Windows.Forms.Control) this.label2);
      this.gcCompliance.Controls.Add((System.Windows.Forms.Control) this.dpClosingDate);
      this.gcCompliance.Controls.Add((System.Windows.Forms.Control) this.label7);
      this.gcCompliance.Controls.Add((System.Windows.Forms.Control) this.dpActClosingDate);
      this.gcCompliance.Controls.Add((System.Windows.Forms.Control) this.dpFeeCollectDate);
      this.gcCompliance.Controls.Add((System.Windows.Forms.Control) this.label5);
      this.gcCompliance.Controls.Add((System.Windows.Forms.Control) this.dpAppDate);
      this.gcCompliance.Controls.Add((System.Windows.Forms.Control) this.label1);
      this.gcCompliance.Controls.Add((System.Windows.Forms.Control) this.label8);
      this.gcCompliance.Dock = DockStyle.Left;
      this.gcCompliance.HeaderForeColor = SystemColors.ControlText;
      this.gcCompliance.Location = new Point(3, 3);
      this.gcCompliance.Name = "gcCompliance";
      this.gcCompliance.Size = new Size(260, 243);
      this.gcCompliance.TabIndex = 0;
      this.gcCompliance.Text = "Compliance Timeline";
      this.dpTILAppDate.BackColor = SystemColors.Window;
      this.dpTILAppDate.Location = new Point(152, 57);
      this.dpTILAppDate.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dpTILAppDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpTILAppDate.Name = "dpTILAppDate";
      this.dpTILAppDate.Size = new Size(85, 22);
      this.dpTILAppDate.TabIndex = 8;
      this.dpTILAppDate.Tag = (object) "3292";
      this.dpTILAppDate.ToolTip = "";
      this.dpTILAppDate.Value = new DateTime(0L);
      this.dpTILAppDate.ValueChanged += new EventHandler(this.freeEntryDate_ValueChanged);
      this.dpTILAppDate.Click += new EventHandler(this.dpImportantDate_Click);
      this.label24.AutoSize = true;
      this.label24.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label24.Location = new Point(7, 61);
      this.label24.Name = "label24";
      this.label24.Size = new Size(104, 14);
      this.label24.TabIndex = 7;
      this.label24.Text = "TIL Application Date ";
      this.emHelpLink1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.emHelpLink1.HelpTag = "Disclosure Tracking Compliance Timeline";
      this.emHelpLink1.Location = new Point(233, 5);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(19, 24);
      this.emHelpLink1.TabIndex = 43;
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
      this.dpInitialDisclosureDueDate.BackColor = SystemColors.Window;
      this.dpInitialDisclosureDueDate.Location = new Point(152, 81);
      this.dpInitialDisclosureDueDate.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.dpInitialDisclosureDueDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpInitialDisclosureDueDate.Name = "dpInitialDisclosureDueDate";
      this.dpInitialDisclosureDueDate.ReadOnly = true;
      this.dpInitialDisclosureDueDate.Size = new Size(85, 22);
      this.dpInitialDisclosureDueDate.TabIndex = 10;
      this.dpInitialDisclosureDueDate.Tag = (object) "3143";
      this.dpInitialDisclosureDueDate.ToolTip = "";
      this.dpInitialDisclosureDueDate.Value = new DateTime(0L);
      this.dpInitialDisclosureDueDate.Click += new EventHandler(this.dpImportantDate_Click);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(7, 85);
      this.label2.Name = "label2";
      this.label2.Size = new Size(131, 14);
      this.label2.TabIndex = 9;
      this.label2.Text = "Initial Disclosure Due Date";
      this.dpClosingDate.BackColor = SystemColors.Window;
      this.dpClosingDate.Location = new Point(152, 129);
      this.dpClosingDate.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.dpClosingDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpClosingDate.Name = "dpClosingDate";
      this.dpClosingDate.ReadOnly = true;
      this.dpClosingDate.Size = new Size(85, 22);
      this.dpClosingDate.TabIndex = 14;
      this.dpClosingDate.Tag = (object) "3147";
      this.dpClosingDate.ToolTip = "";
      this.dpClosingDate.Value = new DateTime(0L);
      this.dpClosingDate.Click += new EventHandler(this.dpImportantDate_Click);
      this.label7.AutoSize = true;
      this.label7.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label7.Location = new Point(7, 133);
      this.label7.Name = "label7";
      this.label7.Size = new Size(105, 14);
      this.label7.TabIndex = 13;
      this.label7.Text = "Earliest Closing Date";
      this.dpActClosingDate.BackColor = SystemColors.Window;
      this.dpActClosingDate.Location = new Point(152, 153);
      this.dpActClosingDate.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dpActClosingDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpActClosingDate.Name = "dpActClosingDate";
      this.dpActClosingDate.Size = new Size(85, 22);
      this.dpActClosingDate.TabIndex = 16;
      this.dpActClosingDate.Tag = (object) "763";
      this.dpActClosingDate.ToolTip = "";
      this.dpActClosingDate.Value = new DateTime(0L);
      this.dpActClosingDate.ValueChanged += new EventHandler(this.freeEntryDate_ValueChanged);
      this.dpActClosingDate.Click += new EventHandler(this.dpImportantDate_Click);
      this.dpFeeCollectDate.BackColor = SystemColors.Window;
      this.dpFeeCollectDate.Location = new Point(152, 105);
      this.dpFeeCollectDate.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.dpFeeCollectDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpFeeCollectDate.Name = "dpFeeCollectDate";
      this.dpFeeCollectDate.ReadOnly = true;
      this.dpFeeCollectDate.Size = new Size(85, 22);
      this.dpFeeCollectDate.TabIndex = 12;
      this.dpFeeCollectDate.Tag = (object) "3145";
      this.dpFeeCollectDate.ToolTip = "";
      this.dpFeeCollectDate.Value = new DateTime(0L);
      this.dpFeeCollectDate.Click += new EventHandler(this.dpImportantDate_Click);
      this.label5.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label5.Location = new Point(7, 109);
      this.label5.Name = "label5";
      this.label5.Size = new Size(137, 13);
      this.label5.TabIndex = 11;
      this.label5.Text = "Earliest Fee Collection Date";
      this.dpAppDate.BackColor = SystemColors.Window;
      this.dpAppDate.Location = new Point(152, 33);
      this.dpAppDate.MaxValue = new DateTime(2029, 1, 1, 0, 0, 0, 0);
      this.dpAppDate.MinValue = new DateTime(2000, 1, 1, 0, 0, 0, 0);
      this.dpAppDate.Name = "dpAppDate";
      this.dpAppDate.ReadOnly = true;
      this.dpAppDate.Size = new Size(85, 22);
      this.dpAppDate.TabIndex = 6;
      this.dpAppDate.Tag = (object) "3142";
      this.dpAppDate.ToolTip = "";
      this.dpAppDate.Value = new DateTime(0L);
      this.dpAppDate.ValueChanged += new EventHandler(this.dpAppDate_ValueChanged);
      this.dpAppDate.Click += new EventHandler(this.dpImportantDate_Click);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(7, 37);
      this.label1.Name = "label1";
      this.label1.Size = new Size(85, 14);
      this.label1.TabIndex = 5;
      this.label1.Text = "Application Date";
      this.label8.AutoSize = true;
      this.label8.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label8.Location = new Point(7, 157);
      this.label8.Name = "label8";
      this.label8.Size = new Size(116, 14);
      this.label8.TabIndex = 1;
      this.label8.Text = "Estimated Closing Date";
      this.lblTimezone.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblTimezone.Location = new Point(7, 185);
      this.lblTimezone.Name = "lblTimezone";
      this.lblTimezone.Size = new Size(140, 40);
      this.lblTimezone.TabIndex = 44;
      this.lblTimezone.Text = "Disclosure Tracking Timezone";
      this.txtTimezone.BackColor = SystemColors.Menu;
      this.txtTimezone.Enabled = false;
      this.txtTimezone.Location = new Point(152, 185);
      this.txtTimezone.Name = "txtTimezone";
      this.txtTimezone.Size = new Size(85, 23);
      this.txtTimezone.TabIndex = 45;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((System.Windows.Forms.Control) this.groupContainer1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (DisclosureTrackingWS);
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
      this.gcGFE.ResumeLayout(false);
      this.gcGFE.PerformLayout();
      this.gcTIL.ResumeLayout(false);
      this.gcTIL.PerformLayout();
      this.gcCompliance.ResumeLayout(false);
      this.gcCompliance.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
