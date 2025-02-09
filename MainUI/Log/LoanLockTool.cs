// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.LoanLockTool
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.ClientCommon;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.EPC2;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.ProductPricing;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.InputEngine;
using EllieMae.EMLite.LoanUtils.Services;
using EllieMae.EMLite.MainUI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class LoanLockTool : UserControl, IOnlineHelpTarget, IRefreshContents
  {
    private const string className = "LoanLockTool";
    private static readonly string sw = Tracing.SwInputEngine;
    private LockForm lockForm;
    private LoanDataMgr loanMgr;
    private TabPage tabPageRegistration;
    private LoanScreen loanBasicScreen;
    private LoanScreen registrationScreen;
    private FieldQuickEditor quickEditor;
    private Sessions.Session session;
    private bool sortedDescending = true;
    private readonly string channel;
    private LockRequestLog _instance_RequestLog;
    private bool _instance_IsNewLock = true;
    private IContainer components;
    private Panel headerPnl;
    private Label headerLbl;
    private Button newBtn;
    private Panel panelList;
    private Panel panelBottom;
    private Panel panelScreen;
    private TabControl tabControlCurrent;
    private TabPage tabPageLock;
    private TabPage tabPageInfo;
    private GridView gridSnapshot;
    private GroupContainer groupContainer1;
    private CollapsibleSplitter collapsibleSplitter1;
    private GradientPanel gradientPanelHeader;
    private BorderPanel borderPanel1;
    private ImageList imageList1;
    private Panel panelCurrentInfo;
    private Panel panelCurrentInfoTop;
    private GradientPanel gradientPanelAdd;
    private Label label1;
    private BorderPanel borderPanelBottom;
    private Panel panelCurrent;
    private Panel panelAdditional;
    private Panel panelForInfoForm;
    private Button btnExtendLock;
    private FlowLayoutPanel flowLayoutPanel1;
    private Button btnCancelLock;
    private Button btnVoidLock;

    public LoanLockTool(Sessions.Session session, LoanDataMgr loanMgr)
    {
      this.session = session;
      this.Dock = DockStyle.Fill;
      this.loanMgr = loanMgr;
      this.channel = this.loanMgr.LoanData.GetField("2626");
      this.InitializeComponent();
      this.initialPageValue();
      this.sortByRequestedDate();
      this.initForm();
    }

    private void initialPageValue()
    {
      if (this.lockForm != null)
      {
        this.tabPageLock.Controls.Remove((Control) this.lockForm);
        this.lockForm = (LockForm) null;
      }
      this.lockForm = new LockForm(this.loanMgr, true, true, this, true, false);
      this.tabPageLock.Controls.Add((Control) this.lockForm);
      this.lockForm.BringToFront();
      if (this.loanBasicScreen != null)
      {
        this.panelForInfoForm.Controls.Remove((Control) this.loanBasicScreen);
        this.loanBasicScreen = (LoanScreen) null;
      }
      this.loanBasicScreen = new LoanScreen(this.session);
      this.panelForInfoForm.Controls.Add((Control) this.loanBasicScreen);
      this.loanBasicScreen.RemoveTitle();
      this.loanBasicScreen.RemoveBorder();
      this.loanBasicScreen.LoadForm(new InputFormInfo("CurrentLoaninfo", "Current Loan Information"));
      this.panelCurrentInfoTop.Width = 2000;
      string[] requiredFields = (string[]) null;
      if (this.loanMgr.LoanData != null && this.loanMgr.LoanData.SecondaryAdditionalFields != null)
        requiredFields = this.loanMgr.LoanData.SecondaryAdditionalFields.GetFields(false);
      if (requiredFields != null && requiredFields.Length != 0)
      {
        if (this.quickEditor != null)
        {
          this.borderPanelBottom.Controls.Remove((Control) this.quickEditor);
          this.quickEditor = (FieldQuickEditor) null;
        }
        this.quickEditor = new FieldQuickEditor(this.session, this.loanMgr.LoanData, FieldQuickEditorMode.Other, false);
        this.quickEditor.RefreshFieldList(requiredFields, false);
        this.borderPanelBottom.Controls.Add((Control) this.quickEditor);
        this.borderPanelBottom.Height = this.quickEditor.MaxHeight;
        this.panelAdditional.Height = this.borderPanelBottom.Height + this.gradientPanelAdd.Height + 10;
      }
      else
      {
        this.gradientPanelAdd.Visible = false;
        this.borderPanelBottom.Visible = false;
      }
      if (this.registrationScreen != null)
      {
        this.tabPageRegistration.Controls.Remove((Control) this.registrationScreen);
        this.registrationScreen = (LoanScreen) null;
      }
      this.registrationScreen = new LoanScreen(this.session);
      this.tabPageRegistration.Controls.Add((Control) this.registrationScreen);
      this.registrationScreen.RemoveTitle();
      this.registrationScreen.RemoveBorder();
      this.registrationScreen.LoadForm(new InputFormInfo("RegistrationQuickForm", "Registration"));
      this.panelCurrentInfoTop.Height = this.panelForInfoForm.Height = 750;
      if (requiredFields != null && requiredFields.Length != 0)
        this.panelCurrent.Height = this.panelCurrentInfoTop.Height + this.panelAdditional.Height + 10;
      else
        this.panelCurrent.Height = this.panelCurrentInfoTop.Height + 10;
      if ((bool) this.session.ServerManager.GetServerSetting("Policies.EnableLockExtension"))
        this.btnExtendLock.Visible = true;
      else
        this.btnExtendLock.Visible = false;
      if ((bool) this.session.ServerManager.GetServerSetting("Policies.EnableLockCancellation"))
        this.btnCancelLock.Visible = true;
      else
        this.btnCancelLock.Visible = false;
      if (string.Equals(this.channel, "Correspondent", StringComparison.InvariantCultureIgnoreCase) && (bool) this.session.ServerManager.GetServerSetting("Policies.EnableLockVoid"))
        this.btnVoidLock.Visible = true;
      else if (string.Equals(this.channel, "Banked - Retail", StringComparison.InvariantCultureIgnoreCase) && (bool) this.session.ServerManager.GetServerSetting("Policies.EnableLockVoidRetail"))
        this.btnVoidLock.Visible = true;
      else if (string.Equals(this.channel, "Banked - Wholesale", StringComparison.InvariantCultureIgnoreCase) && (bool) this.session.ServerManager.GetServerSetting("Policies.EnableLockVoidWholesale"))
        this.btnVoidLock.Visible = true;
      else
        this.btnVoidLock.Visible = false;
      if (!LockUtils.IfShipDark(this.session.SessionObjects, "EPPS_EPC2_SHIP_DARK_SR") && this.session.StartupInfo.ProductPricingPartner != null && this.session.StartupInfo.ProductPricingPartner.IsEPPS && this.session.StartupInfo.ProductPricingPartner.VendorPlatform == VendorPlatform.EPC2)
        return;
      if (this.gridSnapshot.Columns.Where<GVColumn>((Func<GVColumn, bool>) (x => x.Name == "Column13")).Any<GVColumn>())
        this.gridSnapshot.Columns.Remove(this.gridSnapshot.Columns.Where<GVColumn>((Func<GVColumn, bool>) (x => x.Name == "Column13")).FirstOrDefault<GVColumn>());
      if (!this.gridSnapshot.Columns.Where<GVColumn>((Func<GVColumn, bool>) (x => x.Name == "Column14")).Any<GVColumn>())
        return;
      this.gridSnapshot.Columns.Remove(this.gridSnapshot.Columns.Where<GVColumn>((Func<GVColumn, bool>) (x => x.Name == "Column14")).FirstOrDefault<GVColumn>());
    }

    private void initForm()
    {
      this.gridSnapshot.Sort(this.gridSnapshot.Columns.GetSortOrder());
      this.gridSnapshot.Items.Clear();
      LockRequestLog[] allLockRequests = this.loanMgr.LoanData.GetLogList().GetAllLockRequests();
      this.gridSnapshot.BeginUpdate();
      for (int index = 0; index < allLockRequests.Length; ++index)
      {
        this.gridSnapshot.Items.Add(this.createListViewItem(allLockRequests[index], false));
        if (allLockRequests[index].RequestedStatus == RateLockEnum.GetRateLockStatusString(RateLockRequestStatus.RateLocked))
          this.lockForm.RefreshScreen(allLockRequests[index].GetLockRequestSnapshot(), allLockRequests[index]);
      }
      this.gridSnapshot.EndUpdate();
      this.gridSnapshot.ReSort();
      this.btnExtendLock.Enabled = LockUtils.IsLockExtendable(this.loanMgr);
      this.btnCancelLock.Enabled = false;
      if (this.loanMgr.LoanData != null && !this.loanMgr.LoanData.IsTemplate && LockUtils.IsLockCancellable(this.loanMgr))
        this.btnCancelLock.Enabled = true;
      this.btnVoidLock.Enabled = LockUtils.IsLockVoidable(this.loanMgr);
      this.RefreshBusinessRule();
    }

    public void RefreshBusinessRule()
    {
      try
      {
        if (this.loanMgr == null || this.loanMgr.LoanData == null)
          return;
        new PopupBusinessRules(this.loanMgr.LoanData, (ResourceManager) null, (Image) null, (Image) null, this.session).SetBusinessRules(this.newBtn);
      }
      catch (Exception ex)
      {
        Tracing.Log(LoanLockTool.sw, TraceLevel.Error, nameof (LoanLockTool), "Cannot set Button access right. Error: " + ex.Message);
      }
    }

    private GVItem createListViewItem(LockRequestLog lrl, bool selected)
    {
      GVItem listViewItem = new GVItem("");
      string lockStatus = LoanLockUtils.FindLockStatus(lrl);
      listViewItem.SubItems.Add((object) lockStatus);
      string str1 = lrl.RequestType;
      Hashtable lockRequestSnapshot = lrl.GetLockRequestSnapshot();
      string str2 = lockRequestSnapshot == null || !lockRequestSnapshot.ContainsKey((object) "4209") ? "" : lockRequestSnapshot[(object) "4209"].ToString();
      bool isCancelOrExpired = str2 == "Cancelled Lock" || str2 == "Expired Lock";
      if (str1 != "Trade Extension" && lrl.IsRelock | isCancelOrExpired)
        str1 = LockUtils.GetRelockRequestType(lrl.IsRelock, isCancelOrExpired);
      listViewItem.SubItems.Add((object) str1);
      if (!LockUtils.IfShipDark(this.session.SessionObjects, "EPPS_EPC2_SHIP_DARK_SR") && this.session.StartupInfo.ProductPricingPartner != null && this.session.StartupInfo.ProductPricingPartner.IsEPPS && this.session.StartupInfo.ProductPricingPartner.VendorPlatform == VendorPlatform.EPC2)
      {
        if (!string.IsNullOrEmpty(lrl.LockExtensionIndicator) && lrl.LockExtensionIndicator.ToUpper() == "Y")
          listViewItem.SubItems.Add(new GVSubItem()
          {
            ImageIndex = 7
          });
        else
          listViewItem.SubItems.Add((object) string.Empty);
        listViewItem.SubItems.Add(string.IsNullOrEmpty(lrl.PriceConcessionIndicator) || !(lrl.PriceConcessionIndicator.ToUpper() == "Y") || string.IsNullOrEmpty(lrl.PriceConcessionRequestStatus) ? (object) string.Empty : (object) lrl.PriceConcessionRequestStatus);
      }
      int num1 = 0;
      if (lrl.BuySideNumDayLocked > 0)
      {
        num1 = lrl.BuySideNumDayLocked;
      }
      else
      {
        lockRequestSnapshot = lrl.GetLockRequestSnapshot();
        if (lockRequestSnapshot != null && lockRequestSnapshot.ContainsKey((object) "2150"))
          num1 = Utils.ParseInt(lockRequestSnapshot[(object) "2150"]);
      }
      int num2 = 0;
      if (lrl.CumulatedDaystoExtend > 0)
      {
        num2 = lrl.CumulatedDaystoExtend;
      }
      else
      {
        if (lockRequestSnapshot == null)
          lockRequestSnapshot = lrl.GetLockRequestSnapshot();
        if (lockRequestSnapshot.ContainsKey((object) "3431"))
          num2 = Utils.ParseInt(lockRequestSnapshot[(object) "3431"]);
      }
      string str3 = string.Empty;
      if (!string.IsNullOrEmpty(lrl.InvestorName))
      {
        str3 = lrl.InvestorName;
      }
      else
      {
        if (lockRequestSnapshot == null)
          lockRequestSnapshot = lrl.GetLockRequestSnapshot();
        if (lockRequestSnapshot.ContainsKey((object) "2278"))
          str3 = string.Concat(lockRequestSnapshot[(object) "2278"]);
      }
      LockConfirmLog logForRequestLog1 = this.session.LoanDataMgr.LoanData.GetLogList().GetConfirmLockLogForRequestLog(lrl.Guid);
      LockCancellationLog logForRequestLog2 = this.session.LoanDataMgr.LoanData.GetLogList().GetCancellationLogForRequestLog(lrl.Guid);
      LockRemovedLog logForRequestLog3 = this.session.LoanDataMgr.LoanData.GetLogList().GetLockRemovedLogForRequestLog(lrl.Guid);
      LockVoidLog logForRequestLog4 = this.session.LoanDataMgr.LoanData.GetLogList().GetLockVoidLogForRequestLog(lrl.Guid);
      if (lrl.IsLockExtension)
      {
        switch (LoanLockUtils.TranslateRateLockRequestStatus(lrl))
        {
          case RateLockRequestStatus.OldLock:
          case RateLockRequestStatus.RateLocked:
          case RateLockRequestStatus.Voided:
          case RateLockRequestStatus.OldVoid:
            if (lrl.IsRelock)
            {
              listViewItem.SubItems.Add((object) (num1 + num2));
              break;
            }
            listViewItem.SubItems.Add((object) lrl.BuySideNumDayExtended);
            break;
          default:
            listViewItem.SubItems.Add((object) "");
            break;
        }
      }
      else if (lrl.IsLockCancellation)
        listViewItem.SubItems.Add((object) "");
      else if (num1 > 0)
        listViewItem.SubItems.Add((object) num1);
      else
        listViewItem.SubItems.Add((object) "");
      string str4 = "";
      if (lrl.IsLockExtension)
      {
        switch (LoanLockUtils.TranslateRateLockRequestStatus(lrl))
        {
          case RateLockRequestStatus.OldLock:
          case RateLockRequestStatus.RateLocked:
          case RateLockRequestStatus.Voided:
          case RateLockRequestStatus.OldVoid:
            if (lrl.BuySideNewLockExtensionDate != DateTime.MinValue)
            {
              str4 = lrl.BuySideNewLockExtensionDate.ToString("M/d/yyyy");
              break;
            }
            break;
        }
      }
      else if (lrl.BuySideExpirationDate != DateTime.MinValue)
        str4 = lrl.BuySideExpirationDate.ToString("M/d/yyyy");
      listViewItem.SubItems.Add((object) str4);
      GVSubItemCollection subItems1 = listViewItem.SubItems;
      DateTime dateTime = lrl.Date;
      string str5 = dateTime.ToString("M/d/yyyy") + " " + lrl.TimeRequested;
      subItems1.Add((object) str5);
      listViewItem.SubItems.Add((object) lrl.RequestedFullName);
      if (logForRequestLog1 != null)
      {
        if (lrl.RateLockAction == RateLockAction.UpdateSell)
        {
          listViewItem.SubItems.Add((object) "");
          listViewItem.SubItems.Add((object) "");
        }
        else
        {
          GVSubItemCollection subItems2 = listViewItem.SubItems;
          dateTime = logForRequestLog1.Date;
          string str6 = dateTime.ToString("M/d/yyyy hh:mm:ss tt");
          subItems2.Add((object) str6);
          listViewItem.SubItems.Add((object) logForRequestLog1.ConfirmedByFullName);
        }
      }
      else if (logForRequestLog2 != null)
      {
        GVSubItemCollection subItems3 = listViewItem.SubItems;
        dateTime = logForRequestLog2.Date;
        string str7 = dateTime.ToString("M/d/yyyy hh:mm:ss tt");
        subItems3.Add((object) str7);
        listViewItem.SubItems.Add((object) logForRequestLog2.CancelledByFullName);
      }
      else if (logForRequestLog3 != null)
      {
        GVSubItemCollection subItems4 = listViewItem.SubItems;
        dateTime = logForRequestLog3.Date;
        string str8 = dateTime.ToString("M/d/yyyy hh:mm:ss tt");
        subItems4.Add((object) str8);
        listViewItem.SubItems.Add((object) logForRequestLog3.RemovedByFullName);
      }
      else if (logForRequestLog4 != null)
      {
        GVSubItemCollection subItems5 = listViewItem.SubItems;
        dateTime = logForRequestLog4.Date;
        string str9 = dateTime.ToString("M/d/yyyy hh:mm:ss tt");
        subItems5.Add((object) str9);
        listViewItem.SubItems.Add((object) logForRequestLog4.VoidedByFullName);
      }
      else
      {
        listViewItem.SubItems.Add((object) "");
        listViewItem.SubItems.Add((object) "");
      }
      listViewItem.SubItems.Add((object) str3);
      if (lrl.SellSideDeliveryDate != DateTime.MinValue)
      {
        dateTime = lrl.SellSideDeliveryDate;
        GVSubItem gvSubItem = new GVSubItem((object) dateTime.ToString("M/d/yyyy"));
        if (DateTime.Compare(lrl.SellSideDeliveryDate, DateTime.Today) < 0)
        {
          gvSubItem.ImageIndex = 6;
          gvSubItem.ForeColor = Color.Red;
        }
        listViewItem.SubItems.Add(gvSubItem);
      }
      else
        listViewItem.SubItems.Add((object) "");
      GVSubItemCollection subItems6 = listViewItem.SubItems;
      dateTime = lrl.Date;
      string str10 = dateTime.ToString("yyyy-MM-dd HH:mm:ss.fff tt");
      subItems6.Add((object) str10);
      listViewItem.Tag = (object) lrl;
      listViewItem.Selected = selected;
      this.updateImageStatus(listViewItem);
      return listViewItem;
    }

    private void updateImageStatus(GVItem item)
    {
      LockRequestLog tag = (LockRequestLog) item.Tag;
      if (string.Compare(tag.RequestedStatus, RateLockEnum.GetRateLockStatusString(RateLockRequestStatus.Requested), true) == 0)
      {
        if (tag.IsRelock)
          item.ImageIndex = 0;
        else if (tag.IsLockExtension)
          item.ImageIndex = 3;
        else if (tag.IsLockCancellation)
          item.ImageIndex = 4;
        else
          item.ImageIndex = 0;
      }
      else if (string.Compare(tag.RequestedStatus, RateLockEnum.GetRateLockStatusString(RateLockRequestStatus.RateLocked), true) == 0)
      {
        DateTime today = DateTime.Today;
        DateTime t1 = !tag.IsLockExtension ? tag.BuySideExpirationDate : tag.BuySideNewLockExtensionDate;
        if (t1 != DateTime.MinValue && DateTime.Compare(t1, today) < 0)
          item.ImageIndex = 2;
        else
          item.ImageIndex = 1;
      }
      else if (string.Compare(tag.RequestedStatus, RateLockEnum.GetRateLockStatusString(RateLockRequestStatus.RateExpired), true) == 0)
        item.ImageIndex = 2;
      else if (string.Compare(tag.RequestedStatus, RateLockEnum.GetRateLockStatusString(RateLockRequestStatus.Cancelled), true) == 0)
        item.ImageIndex = 4;
      else if (string.Compare(tag.RequestedStatus, RateLockEnum.GetRateLockStatusString(RateLockRequestStatus.NotLocked), true) == 0)
        item.ImageIndex = 5;
      else
        item.ImageIndex = -1;
    }

    private void newBtn_Click(object sender, EventArgs e)
    {
      if (!this.ValidateAction(sender))
        return;
      this.gridSnapshot.SelectedItems.Clear();
      LockRequestLog requestLog = new LockRequestLog(this.loanMgr.LoanData.GetLogList());
      requestLog.Date = this.session.SessionObjects.Session.ServerTime;
      requestLog.SetRequestingUser(this.session.UserInfo.Userid, this.session.UserInfo.FullName);
      requestLog.IsFakeRequest = true;
      requestLog.ReLockSequenceNumberForInactiveLock = LockUtils.GetReLockSequenceNumberForInactiveLock(this.loanMgr.LoanData);
      LockRequestLog[] allLockRequests = this.loanMgr.LoanData.GetLogList().GetAllLockRequests();
      LockRequestLog currentLock = (LockRequestLog) null;
      for (int index = 0; index < allLockRequests.Length; ++index)
      {
        if (allLockRequests[index].RequestedStatus == RateLockEnum.GetRateLockStatusString(RateLockRequestStatus.RateLocked))
        {
          currentLock = allLockRequests[index];
          break;
        }
      }
      this.popupLockDetail(requestLog, true, currentLock, true, false);
      this.tabControlCurrent_SelectedIndexChanged((object) null, (EventArgs) null);
      BuySellForm.Instance.RefreshCurrentLoanSnapshotData(true);
    }

    private void listViewSnapshot_SubItemClick(object source, GVSubItemMouseEventArgs e)
    {
      if (e.SubItem.Item.Index < 0)
        return;
      LockRequestLog tag = (LockRequestLog) e.SubItem.Item.Tag;
      if (tag == null)
        return;
      LockRequestLog currentLock = (LockRequestLog) null;
      this.popupLockDetail(tag, false, currentLock, false, tag.LockRequestStatus == RateLockRequestStatus.Voided || tag.LockRequestStatus == RateLockRequestStatus.OldVoid);
    }

    private void popupLockDetail(
      LockRequestLog requestLog,
      bool isNewLock,
      LockRequestLog currentLock)
    {
      this.popupLockDetail(requestLog, isNewLock, currentLock, false, false);
    }

    private void popupLockDetail(
      LockRequestLog requestLog,
      bool isNewLock,
      LockRequestLog currentLock,
      bool keepSellData,
      bool isVoidLock)
    {
      this.gridSnapshot.Enabled = false;
      this.newBtn.Enabled = false;
      if (!BuySellForm.IsFormDisplayed)
      {
        BuySellForm.Instance.InitForm(this.loanMgr, requestLog, currentLock, keepSellData, isVoidLock, requestLog.Guid, isNewLock);
        this._instance_RequestLog = requestLog;
        this._instance_IsNewLock = isNewLock;
      }
      if (!BuySellForm.Instance.Visible)
      {
        BuySellForm.Instance.Owner = Session.MainForm;
        BuySellForm.Instance.UpdateSellComparisonButtonClicked += new EventHandler(this.buySellForm_UpdateSellComparisonClicked);
        BuySellForm.Instance.SaveButtonClicked += new EventHandler(this.buySellForm_EventClicked);
        BuySellForm.Instance.ConfirmButtonClicked += new EventHandler(this.buySellForm_EventClicked);
        BuySellForm.Instance.DenyButtonClicked += new EventHandler(this.buySellForm_EventClicked);
        BuySellForm.Instance.CancelButtonClicked += new EventHandler(this.buySellForm_CancelClicked);
        BuySellForm.Instance.ReviseButtonClicked += new EventHandler(this.buySellForm_ReviseClicked);
        BuySellForm.Instance.ValidateButtonClicked += new EventHandler(this.buySellForm_ValidateClicked);
        BuySellForm.Instance.FormClosing += new FormClosingEventHandler(this.buySellForm_FormClosing);
        BuySellForm.Instance.GetPricingCompleted += new EventHandler(this.buySellForm_GetPricingCompleted);
        BuySellForm.Instance.Show();
      }
      if (BuySellForm.Instance.WindowState == FormWindowState.Minimized)
        BuySellForm.Instance.WindowState = FormWindowState.Normal;
      BuySellForm.Instance.Activate();
    }

    private void buySellForm_GetPricingCompleted(object sender, EventArgs e)
    {
      LockRequestLog currentLock = (LockRequestLog) null;
      LockRequestLog requestLog = (LockRequestLog) sender;
      this.popupLockDetail(requestLog, false, currentLock, false, requestLog.LockRequestStatus == RateLockRequestStatus.Voided || requestLog.LockRequestStatus == RateLockRequestStatus.OldVoid);
    }

    private void buySellForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      this.gridSnapshot.Enabled = true;
      this.newBtn.Enabled = true;
    }

    private void buySellForm_EventClicked(object sender, EventArgs e)
    {
      Cursor.Current = Cursors.WaitCursor;
      if (BuySellForm.Instance.LockDeny)
      {
        bool flag = this._instance_RequestLog.LockRequestStatus == RateLockRequestStatus.Requested;
        this.loanMgr.DenyRateRequest(this._instance_RequestLog, BuySellForm.Instance.DataTables, this.session.UserInfo, BuySellForm.Instance.DenialComments);
        if (flag)
          MainScreen.Instance.RefreshLoanContents();
      }
      else if (BuySellForm.Instance.LockCancel)
        this.loanMgr.CancelRateLock(this._instance_RequestLog, BuySellForm.Instance.DataTables, this.session.UserInfo);
      else if (BuySellForm.Instance.UpdateSellComparison)
        this.loanMgr.SaveForUpdateSellComparison(this._instance_RequestLog, BuySellForm.Instance.DataTables, this.session.UserInfo);
      else
        this.loanMgr.LockRateRequest(this._instance_RequestLog, BuySellForm.Instance.DataTables, this.session.UserInfo, BuySellForm.Instance.LockConfirm);
      if (!BuySellForm.Instance.LockDeny)
        this.lockForm.RefreshScreen(BuySellForm.Instance.DataTables, this._instance_RequestLog);
      this.initForm();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gridSnapshot.Items)
      {
        if (gvItem.Tag == this._instance_RequestLog)
        {
          gvItem.Selected = true;
          break;
        }
      }
      Session.Application.GetService<ILoanEditor>().RefreshLogPanel();
      if (BuySellForm.Instance.LockCancel)
        this.RefreshLoanContents();
      if (sender != null && ((Control) sender).Text.ToLower() == "lock and confirm")
      {
        ProductPricingSetting productPricingPartner = this.session.StartupInfo.ProductPricingPartner;
        if (productPricingPartner != null && productPricingPartner.VendorPlatform != VendorPlatform.EPC2)
          Session.Application.GetService<IEPass>().ProcessURL("_EPASS_SIGNATURE;" + ProductPricingUtils.GetPartnerId(productPricingPartner) + ";;UpdateLoanFile");
      }
      Cursor.Current = Cursors.Default;
    }

    private void buySellForm_CancelClicked(object sender, EventArgs e)
    {
      if (!BuySellForm.Instance.LockDeny)
        return;
      this.gridSnapshot.SelectedItems[0].SubItems[2].Text = RateLockEnum.GetRateLockStatusString(RateLockRequestStatus.RequestDenied);
    }

    private void buySellForm_ReviseClicked(object sender, EventArgs e)
    {
      this._instance_RequestLog = (LockRequestLog) sender;
      this._instance_IsNewLock = true;
    }

    private void buySellForm_UpdateSellComparisonClicked(object sender, EventArgs e)
    {
      this._instance_RequestLog = (LockRequestLog) sender;
    }

    private void buySellForm_ValidateClicked(object sender, EventArgs e)
    {
      this._instance_RequestLog = (LockRequestLog) sender;
      this._instance_IsNewLock = true;
    }

    string IOnlineHelpTarget.GetHelpTargetName() => nameof (LoanLockTool);

    private void tabControlCurrent_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.tabControlCurrent.SelectedTab == this.tabPageInfo)
      {
        this.loanBasicScreen.RefreshContents();
      }
      else
      {
        if (this.tabControlCurrent.SelectedTab != this.tabPageRegistration)
          return;
        this.registrationScreen.RefreshContents();
      }
    }

    public void popUpCurrentLock()
    {
      int nItemIndex = -1;
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gridSnapshot.Items)
      {
        if (LoanLockUtils.FindLockStatus((LockRequestLog) gvItem.Tag) == "Locked")
        {
          nItemIndex = gvItem.Index;
          break;
        }
      }
      if (nItemIndex == -1)
      {
        LockRequestLog confirmedLockRequest = this.loanMgr.LoanData.GetLogList().GetCurrentConfirmedLockRequest();
        if (confirmedLockRequest == null)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Secondary Registration cannot find current lock. The current lock might be expired, removed, or cancelled.");
          return;
        }
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gridSnapshot.Items)
        {
          if (((LogRecordBase) gvItem.Tag).Guid == confirmedLockRequest.Guid)
          {
            nItemIndex = gvItem.Index;
            break;
          }
        }
      }
      this.gridSnapshot.Items[nItemIndex].Selected = true;
      LockRequestLog tag = (LockRequestLog) this.gridSnapshot.Items[nItemIndex].Tag;
      if (tag == null)
        return;
      this.popupLockDetail(tag, false, (LockRequestLog) null);
    }

    public void RefreshContents() => this.initForm();

    public void RefreshLoanContents()
    {
      this.initialPageValue();
      this.RefreshContents();
    }

    private void btnExtendLock_Click(object sender, EventArgs e)
    {
      string providerId = this.loanMgr.LoanData.GetProviderId();
      if (ProductPricingUtils.IsProviderICEPPE(providerId))
      {
        string source = "urn:elli:services:form:secondarylock:extendlock";
        string partnerName;
        string epC2EpassUrl = Epc2ServiceClient.GetEPC2EPassURL(this.session.SessionObjects, this.session.LoanData.GUID, providerId, source, out partnerName);
        if (string.IsNullOrWhiteSpace(epC2EpassUrl))
        {
          int num = (int) Utils.Dialog((IWin32Window) null, string.Format("Please contact your administrator for \"{0}\" access.", (object) partnerName), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else
          Session.Application.GetService<IEPass>().ProcessURL(epC2EpassUrl, true, true);
      }
      else
      {
        LockExtensionUtils settings = new LockExtensionUtils(this.session.SessionObjects, this.loanMgr.LoanData);
        int currentExtNumber = this.loanMgr.LoanData.GetLogList().GetCurrentExtNumber((Hashtable) null);
        if (settings.IsCompanyControlledOccur && settings.AdjOccurrence != null && currentExtNumber + 1 > settings.AdjOccurrence.Length || settings.IfExceedNumExtensionsLimit(currentExtNumber + 1))
        {
          int num1 = (int) Utils.Dialog((IWin32Window) this, "This request exceeds the max number of extensions allowed. Lock has not been extended.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else
        {
          int num2 = (int) new LockExtensionDialog(this.session, this.loanMgr.LoanData, settings, currentExtNumber + 1).ShowDialog((IWin32Window) this);
          this.tabControlCurrent_SelectedIndexChanged((object) null, (EventArgs) null);
        }
      }
    }

    private void btnCancelLock_Click(object sender, EventArgs e)
    {
      if (!this.ValidateAction(sender))
        return;
      int num = (int) new LockCancellationDialog(this.session, this.loanMgr.LoanData, LockCancellationDialog.CancellationActionType.Cancel).ShowDialog();
      this.tabControlCurrent_SelectedIndexChanged((object) null, (EventArgs) null);
      this.RefreshLoanContents();
    }

    private bool ValidateAction(object sender)
    {
      string str = string.Empty;
      string field = this.loanMgr.LoanData.GetField("3907");
      if (sender is Button button)
      {
        switch (button.Name)
        {
          case "newBtn":
            str = "before a new lock can be created ";
            break;
          case "btnCancelLock":
            str = "before the lock can be cancelled";
            break;
        }
      }
      if (string.IsNullOrEmpty(field))
        return true;
      int num = (int) Utils.Dialog((IWin32Window) this, "This loan must be removed from the Correspondent Trade \"" + field + "\" " + str + ".", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      return false;
    }

    private void btnVoidLock_Click(object sender, EventArgs e)
    {
      if (new LockVoidDialog(this.session, this.loanMgr.LoanData).ShowDialog() != DialogResult.OK)
        return;
      this.tabControlCurrent_SelectedIndexChanged((object) null, (EventArgs) null);
      this.RefreshLoanContents();
    }

    private void gridSnapshot_ColumnClick(object source, GVColumnClickEventArgs e)
    {
      bool flag = false;
      if (!LockUtils.IfShipDark(this.session.SessionObjects, "EPPS_EPC2_SHIP_DARK_SR") && this.session.StartupInfo.ProductPricingPartner != null && this.session.StartupInfo.ProductPricingPartner.IsEPPS && this.session.StartupInfo.ProductPricingPartner.VendorPlatform == VendorPlatform.EPC2)
        flag = true;
      if ((e.Column != 5 || flag) && !(e.Column == 7 & flag))
        return;
      this.sortByRequestedDate();
    }

    private void sortByRequestedDate()
    {
      bool flag = false;
      if (!LockUtils.IfShipDark(this.session.SessionObjects, "EPPS_EPC2_SHIP_DARK_SR") && this.session.StartupInfo.ProductPricingPartner != null && this.session.StartupInfo.ProductPricingPartner.IsEPPS && this.session.StartupInfo.ProductPricingPartner.VendorPlatform == VendorPlatform.EPC2)
        flag = true;
      SortOrder sortOrder = SortOrder.Descending;
      if (!this.sortedDescending)
      {
        sortOrder = SortOrder.Ascending;
        this.sortedDescending = true;
      }
      else
        this.sortedDescending = false;
      this.gridSnapshot.Sort(new List<GVColumnSort>()
      {
        new GVColumnSort(flag ? 7 : 5, sortOrder),
        new GVColumnSort(flag ? 13 : 11, sortOrder)
      }.ToArray());
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (LoanLockTool));
      this.headerPnl = new Panel();
      this.gradientPanelHeader = new GradientPanel();
      this.headerLbl = new Label();
      this.panelBottom = new Panel();
      this.panelScreen = new Panel();
      this.tabControlCurrent = new TabControl();
      this.tabPageLock = new TabPage();
      this.tabPageInfo = new TabPage();
      this.panelCurrentInfo = new Panel();
      this.panelCurrent = new Panel();
      this.panelAdditional = new Panel();
      this.gradientPanelAdd = new GradientPanel();
      this.label1 = new Label();
      this.borderPanelBottom = new BorderPanel();
      this.panelCurrentInfoTop = new Panel();
      this.panelForInfoForm = new Panel();
      this.tabPageRegistration = new TabPage();
      this.borderPanel1 = new BorderPanel();
      this.collapsibleSplitter1 = new CollapsibleSplitter();
      this.panelList = new Panel();
      this.groupContainer1 = new GroupContainer();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.btnVoidLock = new Button();
      this.btnCancelLock = new Button();
      this.btnExtendLock = new Button();
      this.newBtn = new Button();
      this.gridSnapshot = new GridView();
      this.imageList1 = new ImageList(this.components);
      this.headerPnl.SuspendLayout();
      this.gradientPanelHeader.SuspendLayout();
      this.panelBottom.SuspendLayout();
      this.panelScreen.SuspendLayout();
      this.tabControlCurrent.SuspendLayout();
      this.tabPageInfo.SuspendLayout();
      this.panelCurrentInfo.SuspendLayout();
      this.panelCurrent.SuspendLayout();
      this.panelAdditional.SuspendLayout();
      this.gradientPanelAdd.SuspendLayout();
      this.panelCurrentInfoTop.SuspendLayout();
      this.panelList.SuspendLayout();
      this.groupContainer1.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      this.SuspendLayout();
      this.headerPnl.Controls.Add((Control) this.gradientPanelHeader);
      this.headerPnl.Dock = DockStyle.Top;
      this.headerPnl.Location = new Point(0, 0);
      this.headerPnl.Name = "headerPnl";
      this.headerPnl.Size = new Size(972, 26);
      this.headerPnl.TabIndex = 2;
      this.gradientPanelHeader.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gradientPanelHeader.Controls.Add((Control) this.headerLbl);
      this.gradientPanelHeader.Dock = DockStyle.Fill;
      this.gradientPanelHeader.Location = new Point(0, 0);
      this.gradientPanelHeader.Name = "gradientPanelHeader";
      this.gradientPanelHeader.Size = new Size(972, 26);
      this.gradientPanelHeader.TabIndex = 0;
      this.headerLbl.AutoSize = true;
      this.headerLbl.BackColor = Color.Transparent;
      this.headerLbl.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.headerLbl.Location = new Point(8, 6);
      this.headerLbl.Name = "headerLbl";
      this.headerLbl.Size = new Size(220, 14);
      this.headerLbl.TabIndex = 1;
      this.headerLbl.Text = "Secondary Registration / Snapshot List";
      this.panelBottom.Controls.Add((Control) this.panelScreen);
      this.panelBottom.Controls.Add((Control) this.collapsibleSplitter1);
      this.panelBottom.Controls.Add((Control) this.panelList);
      this.panelBottom.Dock = DockStyle.Fill;
      this.panelBottom.Location = new Point(0, 26);
      this.panelBottom.Name = "panelBottom";
      this.panelBottom.Size = new Size(972, 530);
      this.panelBottom.TabIndex = 4;
      this.panelScreen.AutoScroll = true;
      this.panelScreen.Controls.Add((Control) this.tabControlCurrent);
      this.panelScreen.Controls.Add((Control) this.borderPanel1);
      this.panelScreen.Dock = DockStyle.Fill;
      this.panelScreen.Location = new Point(0, 259);
      this.panelScreen.Name = "panelScreen";
      this.panelScreen.Size = new Size(972, 271);
      this.panelScreen.TabIndex = 6;
      this.tabControlCurrent.Controls.Add((Control) this.tabPageLock);
      this.tabControlCurrent.Controls.Add((Control) this.tabPageInfo);
      this.tabControlCurrent.Controls.Add((Control) this.tabPageRegistration);
      this.tabControlCurrent.Dock = DockStyle.Fill;
      this.tabControlCurrent.Location = new Point(0, 2);
      this.tabControlCurrent.Margin = new Padding(0);
      this.tabControlCurrent.Name = "tabControlCurrent";
      this.tabControlCurrent.Padding = new Point(10, 3);
      this.tabControlCurrent.SelectedIndex = 0;
      this.tabControlCurrent.Size = new Size(972, 269);
      this.tabControlCurrent.TabIndex = 0;
      this.tabControlCurrent.SelectedIndexChanged += new EventHandler(this.tabControlCurrent_SelectedIndexChanged);
      this.tabPageLock.AutoScroll = true;
      this.tabPageLock.Location = new Point(4, 22);
      this.tabPageLock.Margin = new Padding(10);
      this.tabPageLock.Name = "tabPageLock";
      this.tabPageLock.Padding = new Padding(10);
      this.tabPageLock.Size = new Size(964, 243);
      this.tabPageLock.TabIndex = 0;
      this.tabPageLock.Text = "Current Lock";
      this.tabPageLock.UseVisualStyleBackColor = true;
      this.tabPageInfo.AutoScroll = true;
      this.tabPageInfo.Controls.Add((Control) this.panelCurrentInfo);
      this.tabPageInfo.Location = new Point(4, 22);
      this.tabPageInfo.Name = "tabPageInfo";
      this.tabPageInfo.Padding = new Padding(2, 2, 2, 0);
      this.tabPageInfo.Size = new Size(964, 243);
      this.tabPageInfo.TabIndex = 1;
      this.tabPageInfo.Text = "Current Loan Info";
      this.tabPageInfo.UseVisualStyleBackColor = true;
      this.panelCurrentInfo.AutoScroll = true;
      this.panelCurrentInfo.Controls.Add((Control) this.panelCurrent);
      this.panelCurrentInfo.Dock = DockStyle.Fill;
      this.panelCurrentInfo.Location = new Point(2, 2);
      this.panelCurrentInfo.Name = "panelCurrentInfo";
      this.panelCurrentInfo.Size = new Size(960, 241);
      this.panelCurrentInfo.TabIndex = 0;
      this.panelCurrent.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.panelCurrent.Controls.Add((Control) this.panelAdditional);
      this.panelCurrent.Controls.Add((Control) this.panelCurrentInfoTop);
      this.panelCurrent.Location = new Point(2, 2);
      this.panelCurrent.Name = "panelCurrent";
      this.panelCurrent.Size = new Size(958, 212);
      this.panelCurrent.TabIndex = 1;
      this.panelAdditional.Controls.Add((Control) this.gradientPanelAdd);
      this.panelAdditional.Controls.Add((Control) this.borderPanelBottom);
      this.panelAdditional.Dock = DockStyle.Top;
      this.panelAdditional.Location = new Point(0, 97);
      this.panelAdditional.Name = "panelAdditional";
      this.panelAdditional.Size = new Size(958, 100);
      this.panelAdditional.TabIndex = 2;
      this.gradientPanelAdd.Controls.Add((Control) this.label1);
      this.gradientPanelAdd.Location = new Point(1, 1);
      this.gradientPanelAdd.Name = "gradientPanelAdd";
      this.gradientPanelAdd.Size = new Size(601, 26);
      this.gradientPanelAdd.TabIndex = 0;
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(3, 7);
      this.label1.Name = "label1";
      this.label1.Size = new Size(98, 14);
      this.label1.TabIndex = 0;
      this.label1.Text = "Additional Fields";
      this.borderPanelBottom.Borders = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.borderPanelBottom.Location = new Point(1, 27);
      this.borderPanelBottom.Name = "borderPanelBottom";
      this.borderPanelBottom.Size = new Size(601, 70);
      this.borderPanelBottom.TabIndex = 1;
      this.panelCurrentInfoTop.Controls.Add((Control) this.panelForInfoForm);
      this.panelCurrentInfoTop.Dock = DockStyle.Top;
      this.panelCurrentInfoTop.Location = new Point(0, 0);
      this.panelCurrentInfoTop.Name = "panelCurrentInfoTop";
      this.panelCurrentInfoTop.Size = new Size(958, 97);
      this.panelCurrentInfoTop.TabIndex = 0;
      this.panelForInfoForm.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.panelForInfoForm.Location = new Point(1, 1);
      this.panelForInfoForm.Name = "panelForInfoForm";
      this.panelForInfoForm.Size = new Size(983, 100);
      this.panelForInfoForm.TabIndex = 0;
      this.tabPageRegistration.Location = new Point(4, 22);
      this.tabPageRegistration.Name = "tabPageRegistration";
      this.tabPageRegistration.Padding = new Padding(2, 2, 2, 0);
      this.tabPageRegistration.Size = new Size(964, 243);
      this.tabPageRegistration.TabIndex = 2;
      this.tabPageRegistration.Text = "Registration";
      this.tabPageRegistration.UseVisualStyleBackColor = true;
      this.borderPanel1.Borders = AnchorStyles.Top;
      this.borderPanel1.Dock = DockStyle.Top;
      this.borderPanel1.Location = new Point(0, 0);
      this.borderPanel1.Name = "borderPanel1";
      this.borderPanel1.Size = new Size(972, 2);
      this.borderPanel1.TabIndex = 1;
      this.collapsibleSplitter1.AnimationDelay = 20;
      this.collapsibleSplitter1.AnimationStep = 20;
      this.collapsibleSplitter1.BackColor = Color.WhiteSmoke;
      this.collapsibleSplitter1.BorderStyle3D = Border3DStyle.Flat;
      this.collapsibleSplitter1.ControlToHide = (Control) this.panelList;
      this.collapsibleSplitter1.Dock = DockStyle.Top;
      this.collapsibleSplitter1.ExpandParentForm = false;
      this.collapsibleSplitter1.Location = new Point(0, 252);
      this.collapsibleSplitter1.Name = "collapsibleSplitter1";
      this.collapsibleSplitter1.TabIndex = 7;
      this.collapsibleSplitter1.TabStop = false;
      this.collapsibleSplitter1.UseAnimations = false;
      this.collapsibleSplitter1.VisualStyle = VisualStyles.Encompass;
      this.panelList.Controls.Add((Control) this.groupContainer1);
      this.panelList.Dock = DockStyle.Top;
      this.panelList.Location = new Point(0, 0);
      this.panelList.Name = "panelList";
      this.panelList.Size = new Size(972, 252);
      this.panelList.TabIndex = 4;
      this.groupContainer1.Controls.Add((Control) this.flowLayoutPanel1);
      this.groupContainer1.Controls.Add((Control) this.gridSnapshot);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(972, 252);
      this.groupContainer1.TabIndex = 22;
      this.groupContainer1.Text = "Lock / Request Snapshot";
      this.flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flowLayoutPanel1.BackColor = Color.Transparent;
      this.flowLayoutPanel1.Controls.Add((Control) this.btnVoidLock);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnCancelLock);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnExtendLock);
      this.flowLayoutPanel1.Controls.Add((Control) this.newBtn);
      this.flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel1.Location = new Point(642, 1);
      this.flowLayoutPanel1.Margin = new Padding(3, 0, 3, 3);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new Size(328, 22);
      this.flowLayoutPanel1.TabIndex = 23;
      this.btnVoidLock.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnVoidLock.BackColor = SystemColors.Control;
      this.btnVoidLock.Location = new Point(250, 0);
      this.btnVoidLock.Margin = new Padding(3, 0, 3, 3);
      this.btnVoidLock.Name = "btnVoidLock";
      this.btnVoidLock.Size = new Size(75, 22);
      this.btnVoidLock.TabIndex = 24;
      this.btnVoidLock.Text = "Void Lock";
      this.btnVoidLock.UseVisualStyleBackColor = true;
      this.btnVoidLock.Click += new EventHandler(this.btnVoidLock_Click);
      this.btnCancelLock.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnCancelLock.BackColor = SystemColors.Control;
      this.btnCancelLock.Location = new Point(169, 0);
      this.btnCancelLock.Margin = new Padding(3, 0, 3, 3);
      this.btnCancelLock.Name = "btnCancelLock";
      this.btnCancelLock.Size = new Size(75, 22);
      this.btnCancelLock.TabIndex = 23;
      this.btnCancelLock.Text = "Cancel Lock";
      this.btnCancelLock.UseVisualStyleBackColor = true;
      this.btnCancelLock.Click += new EventHandler(this.btnCancelLock_Click);
      this.btnExtendLock.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnExtendLock.BackColor = SystemColors.Control;
      this.btnExtendLock.Location = new Point(88, 0);
      this.btnExtendLock.Margin = new Padding(3, 0, 3, 3);
      this.btnExtendLock.Name = "btnExtendLock";
      this.btnExtendLock.Size = new Size(75, 22);
      this.btnExtendLock.TabIndex = 22;
      this.btnExtendLock.Text = "Extend Lock";
      this.btnExtendLock.UseVisualStyleBackColor = true;
      this.btnExtendLock.Click += new EventHandler(this.btnExtendLock_Click);
      this.newBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.newBtn.BackColor = SystemColors.Control;
      this.newBtn.Location = new Point(7, 0);
      this.newBtn.Margin = new Padding(3, 0, 3, 3);
      this.newBtn.Name = "newBtn";
      this.newBtn.Size = new Size(75, 22);
      this.newBtn.TabIndex = 20;
      this.newBtn.Text = "New Lock";
      this.newBtn.UseVisualStyleBackColor = true;
      this.newBtn.Click += new EventHandler(this.newBtn_Click);
      this.gridSnapshot.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column6";
      gvColumn1.Text = "";
      gvColumn1.Width = 30;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Status";
      gvColumn2.Width = 78;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column9";
      gvColumn3.Text = "Req. Type";
      gvColumn3.Width = 100;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column13";
      gvColumn4.Text = "Extension";
      gvColumn4.Width = 75;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column14";
      gvColumn5.Text = "Concession";
      gvColumn5.Width = 100;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Column8";
      gvColumn6.SortMethod = GVSortMethod.Numeric;
      gvColumn6.Text = "Days";
      gvColumn6.Width = 50;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "Column10";
      gvColumn7.SortMethod = GVSortMethod.DateTime;
      gvColumn7.Text = "Lock Expiration Date";
      gvColumn7.Width = 120;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "Column1";
      gvColumn8.SortMethod = GVSortMethod.DateTime;
      gvColumn8.Text = "Requested On";
      gvColumn8.Width = 128;
      gvColumn9.ImageIndex = -1;
      gvColumn9.Name = "Column3";
      gvColumn9.Text = "Requested By";
      gvColumn9.Width = 126;
      gvColumn10.ImageIndex = -1;
      gvColumn10.Name = "Column5";
      gvColumn10.SortMethod = GVSortMethod.DateTime;
      gvColumn10.Text = "Fulfilled On";
      gvColumn10.Width = 128;
      gvColumn11.ImageIndex = -1;
      gvColumn11.Name = "Column11";
      gvColumn11.Text = "Fulfilled By";
      gvColumn11.Width = 100;
      gvColumn12.ImageIndex = -1;
      gvColumn12.Name = "Column4";
      gvColumn12.Text = "Investor";
      gvColumn12.Width = 100;
      gvColumn13.ImageIndex = -1;
      gvColumn13.Name = "Column7";
      gvColumn13.Text = "Investor Delivery";
      gvColumn13.Width = 100;
      gvColumn14.ImageIndex = -1;
      gvColumn14.Name = "Column12";
      gvColumn14.Text = "DateSort";
      gvColumn14.Width = 0;
      this.gridSnapshot.Columns.AddRange(new GVColumn[14]
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
        gvColumn14
      });
      this.gridSnapshot.Dock = DockStyle.Fill;
      this.gridSnapshot.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.gridSnapshot.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gridSnapshot.ImageList = this.imageList1;
      this.gridSnapshot.Location = new Point(1, 26);
      this.gridSnapshot.Name = "gridSnapshot";
      this.gridSnapshot.Size = new Size(970, 225);
      this.gridSnapshot.SortHistory = 5;
      this.gridSnapshot.TabIndex = 21;
      this.gridSnapshot.ColumnClick += new GVColumnClickEventHandler(this.gridSnapshot_ColumnClick);
      this.gridSnapshot.SubItemClick += new GVSubItemMouseEventHandler(this.listViewSnapshot_SubItemClick);
      this.imageList1.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imageList1.ImageStream");
      this.imageList1.TransparentColor = Color.Transparent;
      this.imageList1.Images.SetKeyName(0, "rate-lock-request.png");
      this.imageList1.Images.SetKeyName(1, "rate-locked.png");
      this.imageList1.Images.SetKeyName(2, "rate-expired.png");
      this.imageList1.Images.SetKeyName(3, "rate-locked-extension-indicator.png");
      this.imageList1.Images.SetKeyName(4, "cancel.png");
      this.imageList1.Images.SetKeyName(5, "rate-unlocked.png");
      this.imageList1.Images.SetKeyName(6, "red-warning-icon.png");
      this.imageList1.Images.SetKeyName(7, "checkmark-green-32x32.png");
      this.Controls.Add((Control) this.panelBottom);
      this.Controls.Add((Control) this.headerPnl);
      this.Name = nameof (LoanLockTool);
      this.Size = new Size(972, 556);
      this.headerPnl.ResumeLayout(false);
      this.gradientPanelHeader.ResumeLayout(false);
      this.gradientPanelHeader.PerformLayout();
      this.panelBottom.ResumeLayout(false);
      this.panelScreen.ResumeLayout(false);
      this.tabControlCurrent.ResumeLayout(false);
      this.tabPageInfo.ResumeLayout(false);
      this.panelCurrentInfo.ResumeLayout(false);
      this.panelCurrent.ResumeLayout(false);
      this.panelAdditional.ResumeLayout(false);
      this.gradientPanelAdd.ResumeLayout(false);
      this.gradientPanelAdd.PerformLayout();
      this.panelCurrentInfoTop.ResumeLayout(false);
      this.panelList.ResumeLayout(false);
      this.groupContainer1.ResumeLayout(false);
      this.flowLayoutPanel1.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
