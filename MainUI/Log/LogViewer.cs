// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.LogViewer
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.DocEngine;
using EllieMae.EMLite.eFolder;
using EllieMae.EMLite.ePass.Messaging;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.MainUI;
using EllieMae.EMLite.Properties;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class LogViewer : UserControl, IRefreshContents
  {
    private static readonly TimeSpan AsyncRefreshDelay = TimeSpan.FromMilliseconds(500.0);
    private static readonly string className = nameof (LogViewer);
    private static readonly string concurrentUpdateTraceSwitch = Tracing.SwConcurrentUpdates;
    private GVItem concurrentUpdateNotificationGVItem;
    private static readonly string[] fieldsToMonitor = new string[39]
    {
      "2400",
      "761",
      "762",
      "1393",
      "2626",
      "2629",
      "2012",
      "2014",
      "SERVICE.X13",
      "SERVICE.X15",
      "SERVICE.X59",
      "SERVICE.X61",
      "SERVICE.X63",
      "SERVICE.X65",
      "SERVICE.X67",
      "SERVICE.X69",
      "SERVICE.X71",
      "SERVICE.X73",
      "NEWHUD.X354",
      "3136",
      "3137",
      "3140",
      "3143",
      "3144",
      "3148",
      "3152",
      "3201",
      "799",
      "3121",
      "608",
      "3160",
      "763",
      "3147",
      "3171",
      "3197",
      "3156",
      "3167",
      "3168",
      "AUSF.X19"
    };
    private EPassMessageInfo[] messages;
    private bool suspendEvents;
    private bool displayPrintLog = true;
    private bool displayExportLog = true;
    private string lastCompletedMilestoneID;
    private LoanData currentLoan;
    private DateTime nextAsyncRefresh = DateTime.MinValue;
    private object refreshLock = new object();
    private Sessions.Session session;
    private object lastSelectedObject;
    private DateTime lastSelectedObjectTime = DateTime.MinValue;
    private DateTime previousDate = DateTime.Now;
    private IContainer components;
    private TabControlEx tabsLog;
    private TabPageEx tpAlerts;
    private TabPageEx tpLog;
    private GridView gvAlerts;
    private GridView gvLog;
    private StandardIconButton btnNewLog;
    private ToolTip toolTip1;
    private ContextMenuStrip mnuContext;
    private ToolStripMenuItem mnuExpandAll;
    private ToolStripMenuItem mnuCollapseAll;
    private TableLayoutPanel tableLayoutPanel1;
    private Label label1;
    private Panel panel1;
    private Panel panel2;

    public LogViewer()
    {
      this.InitializeComponent();
      this.subscibedEventHandlers();
    }

    protected override void Dispose(bool disposing)
    {
      this.unSubscribeEventHandlers();
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void subscibedEventHandlers()
    {
      ConcurrentUpdateNotificationClientListener.ConcurrentUpdateNotificationActivity += new ConcurrentUpdateNotificationEventHandler(this.onConcurrentUpdateNotification);
      EPassMessages.MessageActivity += new EPassMessageEventHandler(this.onEPassMessageActivity);
      MainScreen.RefreshAlertLogAsync += new SyncAllDisclosurePackageEventHandler(this.MainScreen_RefreshAlertLogAsync);
    }

    private void unSubscribeEventHandlers()
    {
      ConcurrentUpdateNotificationClientListener.ConcurrentUpdateNotificationActivity -= new ConcurrentUpdateNotificationEventHandler(this.onConcurrentUpdateNotification);
      EPassMessages.MessageActivity -= new EPassMessageEventHandler(this.onEPassMessageActivity);
      MainScreen.RefreshAlertLogAsync -= new SyncAllDisclosurePackageEventHandler(this.MainScreen_RefreshAlertLogAsync);
    }

    private void onConcurrentUpdateNotification(object sender, ConcurrentUpdateArgs eventArgs)
    {
      if (eventArgs.LoanGuid != Session.LoanData.GUID)
      {
        Tracing.Log(LogViewer.concurrentUpdateTraceSwitch, LogViewer.className, TraceLevel.Info, string.Format("Suppresed notification as the loanId does not match. Current LoanId: {0}, Received LoanId: {1}, CorrelationId: {2}, Date: {3}", (object) Session.LoanData.GUID, (object) eventArgs.LoanGuid, (object) eventArgs.CorrelationId, (object) eventArgs.Timestamp.ToString()));
      }
      else
      {
        this.gvAlerts.Items.Add(this.createGVItemForConcurrentUpdateNotification(eventArgs));
        Tracing.Log(LogViewer.concurrentUpdateTraceSwitch, LogViewer.className, TraceLevel.Verbose, string.Format("Received notification. LoanId: {0}, CorrelationId: {1}, Date: {2}", (object) eventArgs.LoanGuid, (object) eventArgs.CorrelationId, (object) eventArgs.Timestamp.ToString()));
        this.RefreshContents();
      }
    }

    public LogViewer(Sessions.Session session)
      : this()
    {
      this.session = session;
    }

    public void RefreshContents(bool refreshEpassMessagess = true)
    {
      this.refreshContents(true, refreshEpassMessagess);
    }

    public void RefreshContents() => this.refreshContents(true);

    public void RefreshLoanContents() => this.RefreshContents();

    private void refreshContents(bool preserveState, bool refreshEpassMessagess = true)
    {
      PipelineInfo pipelineInfo = Session.LoanData.ToPipelineInfo();
      pipelineInfo.UpdateAlerts(Session.UserInfo, Session.LoanDataMgr.SystemConfiguration.UserAclGroups, Session.LoanDataMgr.SystemConfiguration.AlertSetupData);
      this.refreshAlertsAndMessages(pipelineInfo, refreshEpassMessagess);
      this.refreshLog(pipelineInfo, preserveState);
    }

    public void Initialize()
    {
      if (this.currentLoan != Session.LoanData)
        this.attachLoanDataEvents();
      this.refreshContents(false);
      if (this.gvAlerts.Items.Count > 0)
        this.tabsLog.SelectedPage = this.tpAlerts;
      else
        this.tabsLog.SelectedPage = this.tpLog;
    }

    private void attachLoanDataEvents()
    {
      this.ReleaseLoan();
      this.currentLoan = Session.LoanData;
      if (this.currentLoan == null)
        return;
      this.currentLoan.LogRecordAdded += new LogRecordEventHandler(this.onLogRecordModified);
      this.currentLoan.LogRecordChanged += new LogRecordEventHandler(this.onLogRecordModified);
      this.currentLoan.LogRecordRemoved += new LogRecordEventHandler(this.onLogRecordModified);
      foreach (string id in LogViewer.fieldsToMonitor)
        this.currentLoan.RegisterCustomFieldValueChangeEventHandler(id, new Routine(this.onLoanFieldModified));
      ((LoanAlertMonitor) this.currentLoan.AlertMonitor).AlertActivation += new EventHandler(this.onLoanAlertActivation);
    }

    public void ReleaseLoan()
    {
      this.concurrentUpdateNotificationGVItem = (GVItem) null;
      this.gvAlerts.Items.Clear();
      this.gvLog.Items.Clear();
      this.messages = (EPassMessageInfo[]) null;
      if (this.currentLoan == null)
        return;
      this.currentLoan.LogRecordAdded -= new LogRecordEventHandler(this.onLogRecordModified);
      this.currentLoan.LogRecordChanged -= new LogRecordEventHandler(this.onLogRecordModified);
      this.currentLoan.LogRecordRemoved -= new LogRecordEventHandler(this.onLogRecordModified);
      foreach (string id in LogViewer.fieldsToMonitor)
        this.currentLoan.UnregisterCustomFieldValueChangeEventHandler(id, new Routine(this.onLoanFieldModified));
      ((LoanAlertMonitor) this.currentLoan.AlertMonitor).AlertActivation -= new EventHandler(this.onLoanAlertActivation);
      this.currentLoan = (LoanData) null;
    }

    public void PopulateLockLabels()
    {
      bool flag = this.session.EncompassEdition != EncompassEdition.Broker && this.session.LoanData.GetLogList().MSLock;
      bool msDateLock = this.session.LoanData.GetLogList().MSDateLock;
      if (!flag && !msDateLock)
      {
        this.label1.Visible = false;
        this.tableLayoutPanel1.RowStyles[0].Height = 0.0f;
        this.tableLayoutPanel1.RowStyles[1].Height = 100f;
      }
      else
      {
        this.label1.Visible = true;
        this.tableLayoutPanel1.RowStyles[0].SizeType = SizeType.Absolute;
        this.tableLayoutPanel1.RowStyles[0].Height = 40f;
        if (flag & msDateLock)
        {
          this.label1.Text = "        Milestone List is in manual mode.\r\n         Milestone Date is in manual mode.\r\n";
          this.label1.Location = new Point(3, 1);
          this.panel2.Location = new Point(4, 5);
        }
        else
        {
          this.label1.Location = new Point(3, 5);
          this.panel2.Location = new Point(4, 7);
          if (flag)
          {
            this.label1.Text = "        Milestone List is in manual mode.";
          }
          else
          {
            if (!msDateLock)
              return;
            this.label1.Text = "        Milestone Date is in manual mode.";
          }
        }
      }
    }

    public bool IsLockEnabled() => this.session.LoanData.GetLogList().MSLock;

    public bool IsDateLockEnabled() => this.session.LoanData.GetLogList().MSDateLock;

    private void onLoanFieldModified(string fieldId, string val) => this.queueAsyncRefresh(false);

    private void onLogRecordModified(object source, LogRecordEventArgs e)
    {
      this.queueAsyncRefresh();
    }

    private void onLoanAlertActivation(object sender, EventArgs e) => this.queueAsyncRefresh();

    private void queueAsyncRefresh(bool refreshEpassMessagess = true)
    {
      lock (this.refreshLock)
      {
        bool flag = this.nextAsyncRefresh == DateTime.MinValue;
        this.nextAsyncRefresh = DateTime.Now + LogViewer.AsyncRefreshDelay;
        if (!flag)
          return;
        ThreadPool.QueueUserWorkItem((WaitCallback) (state => this.invokeAsyncRefresh((object) null, refreshEpassMessagess)));
      }
    }

    private void invokeAsyncRefresh(object notUsed, bool refreshEpassMessagess = true)
    {
      while (true)
      {
        DateTime dateTime = DateTime.MinValue;
        lock (this.refreshLock)
          dateTime = this.nextAsyncRefresh;
        if (!(dateTime == DateTime.MinValue))
        {
          TimeSpan timeout = dateTime - DateTime.Now;
          if (!(timeout < TimeSpan.Zero))
            Thread.Sleep(timeout);
          else
            goto label_9;
        }
        else
          break;
      }
      return;
label_9:
      this.Invoke((Delegate) (() => this.invokeQueuedRefresh(refreshEpassMessagess)));
    }

    private void invokeQueuedRefresh(bool refreshEpassMessagess = true)
    {
      lock (this.refreshLock)
      {
        try
        {
          this.RefreshContents(refreshEpassMessagess);
          this.nextAsyncRefresh = DateTime.MinValue;
        }
        catch
        {
          this.queueAsyncRefresh(refreshEpassMessagess);
        }
      }
    }

    private void refreshAlertsAndMessages(PipelineInfo pinfo, bool refreshEpassMessagess = true)
    {
      this.gvAlerts.Items.Clear();
      List<PipelineInfo.Alert> alertList = new List<PipelineInfo.Alert>((IEnumerable<PipelineInfo.Alert>) pinfo.Alerts);
      alertList.Sort(new Comparison<PipelineInfo.Alert>(this.compareAlertsByDate));
      for (int index = alertList.Count - 1; index >= 0; --index)
      {
        if ((alertList[index].AlertMessage ?? "") == "")
          alertList.RemoveAt(index);
      }
      foreach (PipelineInfo.Alert alert in alertList)
        this.gvAlerts.Items.Add(this.createGVItemForAlert(alert));
      if (refreshEpassMessagess)
        this.messages = Session.SessionObjects.GetEpassMessagesForCurrentLoan(Session.LoanData.GUID, Session.UserID);
      if (this.messages != null)
      {
        foreach (EPassMessageInfo message in this.messages)
          this.gvAlerts.Items.Add(this.createGVItemForMessage(message));
      }
      int length = this.messages.Length;
      if (this.concurrentUpdateNotificationGVItem != null)
      {
        this.gvAlerts.Items.Add(this.concurrentUpdateNotificationGVItem);
        ++length;
      }
      this.createAlertTabHeader(alertList.Count, length);
    }

    private int compareAlertsByDate(PipelineInfo.Alert alert1, PipelineInfo.Alert alert2)
    {
      return alert1.Date == alert2.Date ? string.Compare(alert1.AlertMessage, alert2.AlertMessage) : alert1.Date.CompareTo(alert2.Date);
    }

    private void createAlertTabHeader(int alertCount, int msgCount)
    {
      Element objB = (Element) new FormattedText("Alerts & Messages");
      int spacing = 4;
      if (msgCount > 0)
      {
        objB = (Element) new MultiValueElement((Element) new AlertMessageLabel(AlertMessageLabel.AlertMessageStyle.Message, msgCount.ToString()), objB, spacing, LayoutDirection.Horizontal);
        spacing = 0;
      }
      if (alertCount > 0)
        objB = (Element) new MultiValueElement((Element) new AlertMessageLabel(AlertMessageLabel.AlertMessageStyle.Alert, alertCount.ToString()), objB, spacing, LayoutDirection.Horizontal);
      this.tpAlerts.Value = (object) objB;
    }

    private GVItem createGVItemForConcurrentUpdateNotification(ConcurrentUpdateArgs args)
    {
      GVItem updateNotification = new GVItem();
      updateNotification.SubItems[0].Value = (object) new ObjectWithImage("New updates to the loan are available", (Image) Resources.new_message_sm);
      DateTime localTime = args.Timestamp.ToLocalTime();
      updateNotification.SubItems[1].Text = LogViewer.formatDate(localTime);
      updateNotification.ForeColor = EncompassColors.Alert1;
      updateNotification.Tag = (object) args;
      this.concurrentUpdateNotificationGVItem = updateNotification;
      return updateNotification;
    }

    private GVItem createGVItemForAlert(PipelineInfo.Alert alert)
    {
      Image image = (Image) Resources.alert_sm;
      if (Alerts.IsRegulationAlert(alert.AlertID))
        image = alert.AlertID == 72 && !Utils.IsEmptyDate((object) Session.LoanData.GetField("5001")) || alert.AlertID == 73 && !Utils.IsEmptyDate((object) Session.LoanData.GetField("5005")) || alert.AlertID == 47 && !Utils.IsEmptyDate((object) Session.LoanData.GetField("5003")) || alert.AlertID == 46 && !Utils.IsEmptyDate((object) Session.LoanData.GetField("5007")) ? (Image) Resources.flag_compliance_green_10x10 : (Image) Resources.flag_compliance_10x10;
      else if (alert.AlertID == 28)
        image = (Image) Resources.flag_compliance_10x10;
      else if (alert.AlertID == 52)
        image = (Image) Resources.flag_compliance_10x10;
      else if (alert.AlertID == 53)
        image = (Image) Resources.flag_compliance_10x10;
      else if (alert.AlertID == 56)
        image = (Image) Resources.flag_compliance_10x10;
      else if (alert.AlertID == 57)
        image = (Image) Resources.flag_compliance_10x10;
      else if (alert.AlertID == 59)
        image = (Image) Resources.flag_compliance_10x10;
      else if (alert.AlertID == 60)
        image = (Image) Resources.flag_compliance_10x10;
      else if (alert.AlertID == 61)
        image = (Image) Resources.flag_compliance_10x10;
      else if (alert.AlertID == 30)
        image = (Image) Resources.alert_efolder_10x10;
      return new GVItem()
      {
        SubItems = {
          [0] = {
            Value = (object) new ObjectWithImage(this.trimAlertMessage(alert.AlertMessage.Trim()), image)
          },
          [1] = {
            Text = LogViewer.formatDate(alert.Date)
          }
        },
        ForeColor = EncompassColors.Alert2,
        Tag = (object) alert
      };
    }

    private string trimAlertMessage(string msg)
    {
      int length = msg.Replace('\r', '\n').IndexOf('\n');
      if (length < 0)
        return msg;
      return length == 0 ? "" : msg.Substring(0, length);
    }

    private GVItem createGVItemForMessage(EPassMessageInfo msg)
    {
      return new GVItem()
      {
        SubItems = {
          [0] = {
            Value = (object) new ObjectWithImage(msg.Description, (Image) Resources.new_message_sm)
          },
          [1] = {
            Text = LogViewer.formatDate(EPassUtils.EPassTimeToLocalTime(msg.Timestamp))
          }
        },
        ForeColor = EncompassColors.Alert1,
        Tag = (object) msg
      };
    }

    private void refreshLog(PipelineInfo pinfo, bool preserveState)
    {
      Dictionary<string, GVItem> dictionary = new Dictionary<string, GVItem>();
      int index = 0;
      if (preserveState)
      {
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvLog.Items)
        {
          if (gvItem.Tag is LogRecordBase tag)
            dictionary[tag.Guid] = gvItem;
        }
        index = this.gvLog.TopItemIndex;
      }
      this.gvLog.Items.Clear();
      this.populateLogRecords(pinfo);
      this.suspendEvents = true;
      GVItem gvItem1 = (GVItem) null;
      MilestoneLog completedMilestone = Session.LoanData.GetLogList().GetLastCompletedMilestone();
      this.lastCompletedMilestoneID = completedMilestone.MilestoneID;
      foreach (GVItem gvItem2 in (IEnumerable<GVItem>) this.gvLog.Items)
      {
        if (gvItem2.Tag is LogRecordBase tag && dictionary.ContainsKey(tag.Guid))
        {
          gvItem2.Selected = dictionary[tag.Guid].Selected;
          gvItem2.State = dictionary[tag.Guid].State;
        }
        else
          gvItem2.State = gvItem2.GroupItems.Count <= 0 || !this.containsAlert(gvItem2) ? (tag != completedMilestone ? GVItemState.Collapsed : GVItemState.Normal) : GVItemState.Normal;
        if (gvItem1 == null)
        {
          if (this.isAlert(gvItem2) || this.containsAlert(gvItem2))
            gvItem1 = gvItem2;
          else if (tag == completedMilestone)
            gvItem1 = gvItem2;
        }
      }
      if (!preserveState && gvItem1 != null)
        this.gvLog.MoveToTop(gvItem1.Index);
      else if (preserveState)
        this.gvLog.MoveToTop(index);
      else
        this.gvLog.EnsureVisible(this.gvLog.VisibleItems.Count - 1);
      this.suspendEvents = false;
      this.tabsLog.Invalidate();
      this.PopulateLockLabels();
    }

    private void populateLogRecords(PipelineInfo pinfo)
    {
      GVItem gvItem1 = (GVItem) null;
      LogRecordBase[] allDatedRecords = Session.LoanData.GetLogList().GetAllDatedRecords();
      Session.LoanData.GetLogList().GetCurrentMilestone();
      Dictionary<string, PipelineInfo.Alert> dictionary = new Dictionary<string, PipelineInfo.Alert>();
      foreach (PipelineInfo.Alert alert in pinfo.Alerts)
      {
        if ((alert.LogRecordID ?? "") != "")
          dictionary[alert.LogRecordID] = alert;
      }
      foreach (LogRecordBase log1 in allDatedRecords)
      {
        if (log1.DisplayInLog)
        {
          GVItem gvItem2 = (GVItem) null;
          if (log1 is MilestoneHistoryLog)
          {
            this.gvLog.Items.Add(this.createGVItemForLogRecord(log1));
          }
          else
          {
            if (log1 is MilestoneLog)
            {
              MilestoneLog log2 = (MilestoneLog) log1;
              if (!(log2.MilestoneID == string.Empty))
              {
                gvItem2 = gvItem1 = this.createGVItemForMilestone(log2, Session.LoanData.GetLogList().ShowDatesInLog);
                this.gvLog.Items.Add(gvItem1);
              }
              else
                continue;
            }
            else if (gvItem1 != null)
            {
              gvItem2 = this.createGVItemForLogRecord(log1);
              if (gvItem2 != null)
                gvItem1.GroupItems.Add(gvItem2);
            }
            if (gvItem2 != null && dictionary.ContainsKey(((LogRecordBase) gvItem2.Tag).Guid))
              LogViewer.makeAlert(gvItem2);
          }
        }
      }
    }

    private bool isAlert(GVItem item) => item.ForeColor == EncompassColors.Alert2;

    private static void makeAlert(GVItem item) => item.ForeColor = EncompassColors.Alert2;

    private static object createLogElement(string text)
    {
      return (object) new PaddedElement(text, new Padding(16, 0, 0, 0));
    }

    private bool containsAlert(GVItem item)
    {
      foreach (GVItem groupItem in (IEnumerable<GVItem>) item.GroupItems)
      {
        if (this.isAlert(groupItem))
          return true;
      }
      return false;
    }

    private GVItem createGVItemForMilestone(MilestoneLog log, bool showDates)
    {
      string str = "";
      EllieMae.EMLite.Workflow.Milestone milestoneById = ((MilestoneTemplatesBpmManager) this.session.BPM.GetBpmManager(BpmCategory.Milestones)).GetMilestoneByID(log.MilestoneID, log.Stage, false, log.Days, log.DoneText, log.ExpText, log.RoleRequired == "Y", log.RoleID);
      MilestoneLabel milestoneLabel = new MilestoneLabel(milestoneById);
      if (milestoneById.DescTextAfter == "")
        milestoneById.DescTextAfter = milestoneById.Name.Trim() + " finished";
      if (milestoneById.DescTextBefore == "")
        milestoneById.DescTextBefore = milestoneById.Name.Trim() + " expected";
      milestoneLabel.DisplayName = str = log.Done ? milestoneById.DescTextAfter : milestoneById.DescTextBefore;
      GVItem itemForMilestone = new GVItem((object) milestoneLabel);
      if (log.Done)
        milestoneLabel.Font = new Font(this.Font, FontStyle.Bold);
      if (showDates)
      {
        DateTime dateTime = log.Date;
        DateTime date1 = dateTime.Date;
        dateTime = DateTime.MinValue;
        DateTime date2 = dateTime.Date;
        if (date1 != date2)
        {
          dateTime = log.Date;
          DateTime date3 = dateTime.Date;
          dateTime = DateTime.MaxValue;
          DateTime date4 = dateTime.Date;
          if (date3 != date4)
          {
            if (log.Done)
            {
              itemForMilestone.SubItems[1].Value = (object) new TextElement(LogViewer.formatDate(log.Date), new Font(this.Font, FontStyle.Bold), Color.Empty);
              this.previousDate = log.Date;
              goto label_15;
            }
            else
            {
              this.previousDate = log.Date;
              itemForMilestone.SubItems[1].Value = (object) LogViewer.formatDate(this.previousDate);
              goto label_15;
            }
          }
        }
        this.previousDate = Session.Application.GetService<ILoanEditor>().AddDays(this.previousDate, log.Days);
        itemForMilestone.SubItems[1].Value = (object) LogViewer.formatDate(this.previousDate);
      }
      else if (!showDates && milestoneById.MilestoneID == "1")
        itemForMilestone.SubItems[1].Value = (object) new TextElement(LogViewer.formatDate(log.Date), new Font(this.Font, FontStyle.Bold), Color.Empty);
label_15:
      itemForMilestone.Tag = (object) log;
      return itemForMilestone;
    }

    private GVItem createGVItemForLogRecord(LogRecordBase log)
    {
      GVItem itemForLogRecord = new GVItem();
      if (!this.populateGVItemForLogRecord(itemForLogRecord, log))
        return (GVItem) null;
      itemForLogRecord.Tag = (object) log;
      return itemForLogRecord;
    }

    private bool populateGVItemForLogRecord(GVItem item, LogRecordBase log)
    {
      switch (log)
      {
        case ConversationLog _:
          return this.populateGVItemForConversationLog(item, log as ConversationLog);
        case DocumentLog _:
          return this.populateGVItemForDocumentLog(item, log as DocumentLog);
        case LockRequestLog _:
          return this.populateGVItemForLockRequestLog(item, log as LockRequestLog);
        case LockConfirmLog _:
          return this.populateGVItemForLockConfirmationLog(item, log as LockConfirmLog);
        case LockDenialLog _:
          return this.populateGVItemForLockDenialLog(item, log as LockDenialLog);
        case LockCancellationLog _:
          return this.populateGVItemForLockCancellationLog(item, log as LockCancellationLog);
        case LockVoidLog _:
          return this.populateGVItemForLockVoidLog(item, log as LockVoidLog);
        case LockRemovedLog _:
          return this.populateGVItemForLockRemovedLog(item, log as LockRemovedLog);
        case RegistrationLog _:
          return this.populateGVItemForRegistrationLog(item, log as RegistrationLog);
        case PrintLog _ when this.displayPrintLog:
          return this.populateGVItemForPrintLog(item, log as PrintLog);
        case ExportLog _ when this.displayExportLog:
          return this.populateGVItemForExportLog(item, log as ExportLog);
        case ServicingPrintLog _:
          return this.populateGVItemForServicingPrintLog(item, log as ServicingPrintLog);
        case DownloadLog _:
          return this.populateGVItemForDownloadLog(item, log as DownloadLog);
        case SystemLog _:
          return this.populateGVItemForSystemLog(item, log as SystemLog);
        case MilestoneTaskLog _:
          return this.populateGVItemForMilestoneTaskLog(item, log as MilestoneTaskLog);
        case EmailTriggerLog _:
          return this.populateGVItemForEmailTriggerLog(item, log as EmailTriggerLog);
        case EDMLog _:
          return this.populateGVItemForEDMLog(item, log as EDMLog);
        case HtmlEmailLog _:
          return this.populateGVItemForHtmlEmailLog(item, log as HtmlEmailLog);
        case DisclosureTrackingLog _:
          return this.populateGVItemForDisclosureTrackingLog(item, log as DisclosureTrackingLog);
        case DisclosureTracking2015Log _:
          return this.populateGVItemForDisclosureTracking2015Log(item, log as DisclosureTracking2015Log);
        case DataTracLog _:
          return this.populateGVItemForDataTracLog(item, log as DataTracLog);
        case DocumentOrderLog _:
          return this.populateGVItemForDocumentOrderLog(item, log as DocumentOrderLog);
        case ECloseLog _:
          return this.populateGVItemForECloseLog(item, log as ECloseLog);
        case GetIndexLog _:
          return this.populateGVItemForGetIndexLog(item, log as GetIndexLog);
        case MilestoneHistoryLog _:
          return this.populateGVItemForMilestoneHistoryLog(item, log as MilestoneHistoryLog);
        case GoodFaithFeeVarianceCureLog _:
          return this.populateGVItemForGoodFaithFeeVarianceCureLog(item, log as GoodFaithFeeVarianceCureLog);
        default:
          return false;
      }
    }

    private static string formatDate(DateTime date)
    {
      return date.Date == DateTime.MinValue.Date || date.Date == DateTime.MaxValue.Date ? "" : date.ToString("MM/dd/yy");
    }

    private bool populateGVItemForConversationLog(GVItem item, ConversationLog log)
    {
      LogAlert mostCriticalAlert = log.AlertList.GetMostCriticalAlert();
      if (mostCriticalAlert == null)
      {
        item.SubItems[0].Value = !log.IsEmail ? LogViewer.createLogElement("Called " + log.Name) : LogViewer.createLogElement("Emailed " + log.Name);
        item.SubItems[1].Text = LogViewer.formatDate(log.Date);
      }
      else if (mostCriticalAlert.FollowedUpDate == DateTime.MinValue)
      {
        item.SubItems[0].Value = LogViewer.createLogElement("Follow up (" + log.Name + ")");
        item.SubItems[1].Text = LogViewer.formatDate(mostCriticalAlert.DueDate);
      }
      else
      {
        item.SubItems[0].Value = LogViewer.createLogElement("Follow up (" + log.Name + ")");
        item.SubItems[1].Text = LogViewer.formatDate(mostCriticalAlert.FollowedUpDate);
      }
      return true;
    }

    private bool populateGVItemForDocumentLog(GVItem item, DocumentLog log)
    {
      DateTime date = log.Date;
      string str1 = log.Title;
      if (log is VerifLog)
        str1 = str1 + "-" + log.RequestedFrom;
      string str2 = string.Empty;
      switch (log.Status)
      {
        case "expected":
          str2 = " expected";
          break;
        case "expected!":
          str2 = " expected";
          break;
        case "expired!":
          str2 = " expired";
          break;
        case "needed":
          str2 = " needed";
          break;
        case "ordered":
          str2 = " requested";
          break;
        case "ready for UW":
          str2 = " ready for UW";
          break;
        case "ready to ship":
          str2 = " ready to ship";
          break;
        case "received":
          str2 = " received";
          break;
        case "reordered":
          str2 = " re-requested";
          break;
        case "reviewed":
          str2 = " reviewed";
          break;
      }
      if (!log.IsExpired && log.Expires && this.isAlertDate(log.DateExpires, 17))
      {
        str2 = " expires";
        date = log.DateExpires;
      }
      item.SubItems[0].Value = LogViewer.createLogElement(str1 + str2);
      item.SubItems[1].Text = LogViewer.formatDate(date);
      return true;
    }

    private bool populateGVItemForDisclosureTrackingLog(GVItem item, DisclosureTrackingLog log)
    {
      string str;
      switch (log.DisclosureMethod)
      {
        case DisclosureTrackingBase.DisclosedMethod.ByMail:
          str = "mailed";
          break;
        case DisclosureTrackingBase.DisclosedMethod.eDisclosure:
          str = "sent electronically";
          break;
        case DisclosureTrackingBase.DisclosedMethod.Fax:
          str = "faxed";
          break;
        case DisclosureTrackingBase.DisclosedMethod.InPerson:
          str = "hand delivered";
          break;
        case DisclosureTrackingBase.DisclosedMethod.Email:
          str = "emailed";
          break;
        case DisclosureTrackingBase.DisclosedMethod.Phone:
          str = "called";
          break;
        case DisclosureTrackingBase.DisclosedMethod.Signature:
          str = "signed";
          break;
        default:
          str = "provided";
          break;
      }
      item.SubItems[0].Value = LogViewer.createLogElement("Disclosures " + str);
      item.SubItems[1].Text = LogViewer.formatDate(log.Date);
      return log.IsDisclosed;
    }

    private bool populateGVItemForDisclosureTracking2015Log(
      GVItem item,
      DisclosureTracking2015Log log)
    {
      string str;
      switch (log.DisclosureMethod)
      {
        case DisclosureTrackingBase.DisclosedMethod.ByMail:
          str = "mailed";
          break;
        case DisclosureTrackingBase.DisclosedMethod.eDisclosure:
        case DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder:
          str = "sent electronically";
          break;
        case DisclosureTrackingBase.DisclosedMethod.Fax:
          str = "faxed";
          break;
        case DisclosureTrackingBase.DisclosedMethod.InPerson:
          str = "hand delivered";
          break;
        case DisclosureTrackingBase.DisclosedMethod.Email:
          str = "emailed";
          break;
        case DisclosureTrackingBase.DisclosedMethod.Phone:
          str = "called";
          break;
        case DisclosureTrackingBase.DisclosedMethod.Signature:
          str = "signed";
          break;
        default:
          str = "provided";
          break;
      }
      item.SubItems[0].Value = log.DisclosureMethod != DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder ? LogViewer.createLogElement("Disclosures " + str) : LogViewer.createLogElement("Closing Disclosures " + str);
      item.SubItems[1].Text = LogViewer.formatDate(log.Date);
      return log.IsDisclosed;
    }

    private bool populateGVItemForMilestoneTaskLog(GVItem item, MilestoneTaskLog log)
    {
      LogAlert mostCriticalAlert = log.AlertList.GetMostCriticalAlert();
      if (mostCriticalAlert == null)
      {
        if (log.CompletedDate != DateTime.MinValue)
        {
          item.SubItems[0].Value = LogViewer.createLogElement(log.TaskName + " completed");
          item.SubItems[1].Text = LogViewer.formatDate(log.CompletedDate);
        }
        else
        {
          if (!(log.CompletedDate == DateTime.MinValue) || log.DaysToComplete <= 0)
            return false;
          item.SubItems[0].Value = LogViewer.createLogElement(log.TaskName + " expected");
          item.SubItems[1].Text = LogViewer.formatDate(log.Date);
        }
      }
      else if (mostCriticalAlert.FollowedUpDate == DateTime.MinValue)
      {
        item.SubItems[0].Value = LogViewer.createLogElement("Follow up (" + log.TaskName + ")");
        item.SubItems[1].Text = LogViewer.formatDate(mostCriticalAlert.DueDate);
      }
      else
      {
        if (!(log.CompletedDate == DateTime.MinValue) || log.DaysToComplete <= 0)
          return false;
        item.SubItems[0].Value = LogViewer.createLogElement(log.TaskName + " expected");
        item.SubItems[1].Text = LogViewer.formatDate(log.ExpectedDate);
      }
      return true;
    }

    private bool populateGVItemForLockRequestLog(GVItem item, LockRequestLog log)
    {
      item.SubItems[0].Value = !log.IsLockCancellation ? LogViewer.createLogElement("Lock requested by " + log.RequestedFullName) : LogViewer.createLogElement("Lock cancellation requested by " + log.RequestedFullName);
      item.SubItems[1].Text = LogViewer.formatDate(log.Date);
      return true;
    }

    private bool populateGVItemForLockVoidLog(GVItem item, LockVoidLog log)
    {
      item.SubItems[0].Value = LogViewer.createLogElement("Lock voided by " + log.VoidedByFullName);
      item.SubItems[1].Text = LogViewer.formatDate(log.Date);
      return true;
    }

    private bool populateGVItemForLockConfirmationLog(GVItem item, LockConfirmLog log)
    {
      item.SubItems[0].Value = LogViewer.createLogElement("Lock confirmed by " + log.ConfirmedByFullName);
      item.SubItems[1].Text = LogViewer.formatDate(log.Date);
      return true;
    }

    private bool populateGVItemForLockDenialLog(GVItem item, LockDenialLog log)
    {
      item.SubItems[0].Value = LogViewer.createLogElement("Lock denied by " + log.DeniedByFullName);
      item.SubItems[1].Text = LogViewer.formatDate(log.Date);
      return true;
    }

    private bool populateGVItemForLockCancellationLog(GVItem item, LockCancellationLog log)
    {
      item.SubItems[0].Value = LogViewer.createLogElement("Lock cancelled by " + log.CancelledByFullName);
      item.SubItems[1].Text = LogViewer.formatDate(log.Date);
      return true;
    }

    private bool populateGVItemForLockRemovedLog(GVItem item, LockRemovedLog log)
    {
      item.SubItems[0].Value = LogViewer.createLogElement("Lock removed from Correspondent Trade by " + log.RemovedByFullName);
      item.SubItems[1].Text = LogViewer.formatDate(log.Date);
      return true;
    }

    private bool populateGVItemForRegistrationLog(GVItem item, RegistrationLog log)
    {
      string text = "Loan registered";
      if (log.InvestorName != "")
        text = text + " with " + log.InvestorName;
      DateTime date = log.Date;
      if (log.IsCurrent)
      {
        if (log.IsExpired)
        {
          text = "Registration expired";
          date = log.ExpiredDate;
        }
        else if (log.Expires && this.isAlertDate(log.ExpiredDate, 14))
        {
          text = "Registration expires";
          date = log.ExpiredDate;
        }
      }
      item.SubItems[0].Value = LogViewer.createLogElement(text);
      item.SubItems[1].Text = LogViewer.formatDate(date);
      return true;
    }

    private bool populateGVItemForPrintLog(GVItem item, PrintLog log)
    {
      string text;
      if (log.Action == PrintLog.PrintAction.PrintToFile)
        text = "Forms printed to file";
      else if (log.Action == PrintLog.PrintAction.Preview)
      {
        text = "Forms previewed";
      }
      else
      {
        if (log.Action != PrintLog.PrintAction.Print)
          return false;
        text = "Forms printed";
      }
      if (log.PrintedByFullName != "")
        text = text + " by " + log.PrintedByFullName;
      item.SubItems[0].Value = LogViewer.createLogElement(text);
      item.SubItems[1].Text = LogViewer.formatDate(log.Date);
      return true;
    }

    private bool populateGVItemForExportLog(GVItem item, ExportLog log)
    {
      string text;
      if (log.Category == ExportLog.ExportCategory.GSEServices)
      {
        text = "GSE Services exported";
      }
      else
      {
        if (log.Category != ExportLog.ExportCategory.ComplianceServices)
          return false;
        text = "Compliance Services exported";
      }
      if (log.ExportedByFullName != "")
        text = text + " by " + log.ExportedByFullName;
      item.SubItems[0].Value = LogViewer.createLogElement(text);
      item.SubItems[1].Text = LogViewer.formatDate(log.Date);
      return true;
    }

    private bool populateGVItemForDocumentOrderLog(GVItem item, DocumentOrderLog log)
    {
      item.SubItems[0].Value = log.OrderType != DocumentOrderType.Closing ? LogViewer.createLogElement("eDisclosures Ordered") : LogViewer.createLogElement("Closing Documents Ordered");
      item.SubItems[1].Text = LogViewer.formatDate(log.Date);
      return true;
    }

    private bool populateGVItemForECloseLog(GVItem item, ECloseLog log)
    {
      item.SubItems[0].Value = LogViewer.createLogElement("eClose Ordered");
      item.SubItems[1].Text = LogViewer.formatDate(log.Date);
      return true;
    }

    private bool populateGVItemForEmailTriggerLog(GVItem item, EmailTriggerLog log)
    {
      item.SubItems[0].Value = LogViewer.createLogElement("Automated Email Sent");
      item.SubItems[1].Text = LogViewer.formatDate(log.Date);
      return true;
    }

    private bool populateGVItemForServicingPrintLog(GVItem item, ServicingPrintLog log)
    {
      item.SubItems[0].Value = LogViewer.createLogElement("Statement Printed by " + log.PrintedBy);
      item.SubItems[1].Text = LogViewer.formatDate(log.Date);
      return true;
    }

    private bool populateGVItemForDownloadLog(GVItem item, DownloadLog log)
    {
      item.SubItems[0].Value = LogViewer.createLogElement(log.Title + " received");
      item.SubItems[1].Text = LogViewer.formatDate(log.Date);
      return true;
    }

    private bool populateGVItemForSystemLog(GVItem item, SystemLog log)
    {
      item.SubItems[0].Value = LogViewer.createLogElement(log.Description);
      item.SubItems[1].Text = LogViewer.formatDate(log.Date);
      return true;
    }

    private bool populateGVItemForEDMLog(GVItem item, EDMLog log)
    {
      item.SubItems[0].Value = LogViewer.createLogElement(log.Description);
      item.SubItems[1].Text = LogViewer.formatDate(log.Date);
      return true;
    }

    private bool populateGVItemForHtmlEmailLog(GVItem item, HtmlEmailLog log)
    {
      item.SubItems[0].Value = LogViewer.createLogElement(log.Description);
      item.SubItems[1].Text = LogViewer.formatDate(log.Date);
      return true;
    }

    private bool populateGVItemForDataTracLog(GVItem item, DataTracLog log)
    {
      item.SubItems[0].Value = LogViewer.createLogElement("Submitted to DataTrac by " + log.CreatedBy);
      item.SubItems[1].Text = LogViewer.formatDate(log.Date);
      return true;
    }

    private bool populateGVItemForGetIndexLog(GVItem item, GetIndexLog log)
    {
      item.SubItems[0].Value = LogViewer.createLogElement("Index requested by " + log.UserId);
      item.SubItems[1].Text = LogViewer.formatDate(log.Date);
      return true;
    }

    private bool populateGVItemForMilestoneHistoryLog(GVItem item, MilestoneHistoryLog log)
    {
      item.SubItems[0].Value = LogViewer.createLogElement("Milestone Template Change");
      item.SubItems[1].Text = LogViewer.formatDate(log.Date);
      item.ForeColor = Color.Red;
      return true;
    }

    private bool populateGVItemForGoodFaithFeeVarianceCureLog(
      GVItem item,
      GoodFaithFeeVarianceCureLog log)
    {
      item.SubItems[0].Value = LogViewer.createLogElement("Variance Cured");
      item.SubItems[1].Text = LogViewer.formatDate(log.Date);
      return true;
    }

    private bool allowAddNewLog()
    {
      if (Session.UserInfo.IsSuperAdministrator())
        return true;
      eFolderAccessRights folderAccessRights = new eFolderAccessRights(Session.LoanDataMgr);
      LoanData loanData = Session.LoanData;
      return (loanData.ContentAccess & LoanContentAccess.ConversationLog) == LoanContentAccess.ConversationLog || folderAccessRights.CanAddDocuments || (loanData.ContentAccess & LoanContentAccess.Task) == LoanContentAccess.Task || loanData.ContentAccess == LoanContentAccess.FullAccess;
    }

    private void gvLog_SubItemClick(object source, GVSubItemMouseEventArgs e)
    {
      if (this.suspendEvents || e.Button != MouseButtons.Left)
        return;
      LogRecordBase tag = e.SubItem.Item.Tag as LogRecordBase;
      if (this.isDoubleClick((object) tag))
        return;
      Session.Application.GetService<ILoanEditor>().OpenLogRecord(tag);
    }

    private void btnNewLog_Click(object sender, EventArgs e)
    {
      Session.Application.GetService<ILoanEditor>().PromptCreateNewLogRecord();
    }

    private void gvAlerts_SubItemClick(object source, GVSubItemMouseEventArgs e)
    {
      if (this.suspendEvents)
        return;
      object tag = e.SubItem.Item.Tag;
      if (this.isDoubleClick(tag))
        return;
      switch (tag)
      {
        case EPassMessageInfo _:
          this.processEPassMessage(tag as EPassMessageInfo);
          break;
        case ConcurrentUpdateArgs stagedDocumentUpdateEventArgs:
          this.processConcurrentUpdateNotification(stagedDocumentUpdateEventArgs, e.SubItem.Item);
          break;
        default:
          this.processAlert(tag as PipelineInfo.Alert);
          break;
      }
    }

    private void processConcurrentUpdateNotification(
      ConcurrentUpdateArgs stagedDocumentUpdateEventArgs,
      GVItem item)
    {
      if (MessageBox.Show((IWin32Window) this, "New updates to the loan are available. The loan file will save your changes and refresh with the new updates.", "Save & Refresh", MessageBoxButtons.OK, MessageBoxIcon.Asterisk) == DialogResult.Cancel)
        return;
      MainScreen.Instance.OpenLoanPage.SaveAndRefresh();
      this.gvAlerts.Items.Remove(item);
      this.concurrentUpdateNotificationGVItem = (GVItem) null;
      this.RefreshContents();
    }

    private bool isDoubleClick(object selectedObject)
    {
      bool flag = false;
      if (this.lastSelectedObject != selectedObject)
        flag = false;
      else if (this.lastSelectedObjectTime == DateTime.MinValue)
        flag = false;
      else if ((DateTime.Now - this.lastSelectedObjectTime).TotalSeconds < 1.0)
        flag = true;
      this.lastSelectedObject = selectedObject;
      this.lastSelectedObjectTime = DateTime.Now;
      return flag;
    }

    private void processEPassMessage(EPassMessageInfo msg)
    {
      try
      {
        Application.DoEvents();
        EPassMessageAction action = msg.GetAction();
        if (action != null)
          Notifications.PerformAction(action, msg.Description);
        if (msg.Description.Contains("eConsent Accepted") || msg.Description.Contains("eConsent Rejected") || msg.Description.Contains("Documents esigned") || msg.MessageType == "TITLECENTER_DOC")
          this.session.ConfigurationManager.DeleteEPassMessages(new string[1]
          {
            msg.MessageID
          });
        if (action.ActionType == EPassMessageActionType.Command)
        {
          EPassMessageCommandAction messageCommandAction = (EPassMessageCommandAction) action;
          if (messageCommandAction.Command.ToLower() == "eclose" || messageCommandAction.Command.ToLower() == "enote")
            this.session.ConfigurationManager.DeleteEPassMessages(new string[1]
            {
              msg.MessageID
            });
        }
        this.checkAutoImportMessage(msg, action);
      }
      catch (NotSupportedException ex)
      {
      }
    }

    private void processAlert(PipelineInfo.Alert alert)
    {
      switch ((StandardAlertID) alert.AlertID)
      {
        case StandardAlertID.EscrowDisbursementDue:
          this.openTool("Interim Servicing Worksheet");
          break;
        case StandardAlertID.PaymentPastDue:
          this.openTool("Interim Servicing Worksheet");
          break;
        case StandardAlertID.PrintMailPaymentStatement:
          this.openTool("Interim Servicing Worksheet");
          break;
        case StandardAlertID.PurchaseAdviceForm:
          if (!((FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.ToolsTab_PurchaseAdviceForm))
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "You do not have access to Purchase Advice Input Form.");
            break;
          }
          this.openTool("Purchase Advice Form");
          break;
        case StandardAlertID.RateLockExpired:
          this.openForm("QALO", "QALP");
          break;
        case StandardAlertID.ShippingExpected:
          if (!((FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.ToolsTab_ShippingDetail))
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "You do not have access to Shipping Detail tool.");
            break;
          }
          this.openTool("Shipping Detail");
          break;
        case StandardAlertID.ComplianceReview:
          this.openTool("Compliance Review");
          break;
        case StandardAlertID.FannieServiceDu:
          this.openTool("Fannie Service DU");
          break;
        case StandardAlertID.FannieServiceEc:
          this.openTool("Fannie Service EC");
          break;
        case StandardAlertID.FreddieServiceLpa:
          this.openTool("Freddie Service LPA");
          break;
        case StandardAlertID.FreddieServiceLqa:
          this.openTool("Freddie Service LQA");
          break;
        case StandardAlertID.MIServiceArch:
          this.openTool("MI Service Arch");
          break;
        case StandardAlertID.MIServiceRadian:
          this.openTool("MI Service Radian");
          break;
        case StandardAlertID.MIServiceMgic:
          this.openTool("MI Service MGIC");
          break;
        default:
          Control alertPanel = AlertPanels.GetAlertPanel(alert);
          if (alertPanel != null)
          {
            this.openControl(alertPanel);
            break;
          }
          if ((alert.LogRecordID ?? "") != "")
          {
            this.openLogRecord(alert.LogRecordID);
            break;
          }
          if (!Alerts.IsCustomAlert(alert.AlertID))
            break;
          this.openControl((Control) new AlertPanel(alert));
          break;
      }
    }

    private void openControl(Control ctl)
    {
      Session.Application.GetService<ILoanEditor>().AddToWorkArea(ctl);
    }

    private void openTool(string toolName)
    {
      if (Session.Application.GetService<ILoanEditor>().OpenForm(toolName))
        return;
      int num = (int) Utils.Dialog((IWin32Window) this, "You do not have access to the tool associated with this alert.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
    }

    private void openForm(params string[] formIDs)
    {
      InputFormsAclManager aclManager = (InputFormsAclManager) Session.ACL.GetAclManager(AclCategory.InputForms);
      ILoanEditor service = Session.Application.GetService<ILoanEditor>();
      foreach (string formId in formIDs)
      {
        if (aclManager.CheckPermission(formId) && service.OpenFormByID(formId))
          return;
      }
      int num = (int) Utils.Dialog((IWin32Window) this, "You do not have access to the form associated with this alert.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
    }

    private void openLogRecord(string logGuid)
    {
      LogRecordBase recordById = Session.LoanData.GetLogList().GetRecordByID(logGuid);
      if (recordById == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The specified record could not be found in the loan.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        this.RefreshContents();
      }
      else
        Session.Application.GetService<ILoanEditor>().OpenLogRecord(recordById);
    }

    private void mnuCollapseAll_Click(object sender, EventArgs e)
    {
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvLog.Items)
        gvItem.State = GVItemState.Collapsed;
    }

    private void mnuExpandAll_Click(object sender, EventArgs e)
    {
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvLog.Items)
        gvItem.State = GVItemState.Normal;
    }

    private void LogViewer_Load(object sender, EventArgs e)
    {
      if (this.DesignMode)
        return;
      this.displayPrintLog = (bool) Session.StartupInfo.PolicySettings[(object) "Policies.ShowPrintingLog"];
    }

    private static AlertConfig getAlertConfig(int alertId)
    {
      foreach (AlertConfig alertConfig in Session.StartupInfo.AlertConfigs)
      {
        if (alertConfig.AlertID == alertId)
          return alertConfig;
      }
      return (AlertConfig) null;
    }

    private bool isAlertDate(DateTime date, int alertId)
    {
      AlertConfig alertConfig = LogViewer.getAlertConfig(alertId);
      return alertConfig != null && alertConfig.AlertEnabled && alertConfig.MilestoneGuidList.Contains(this.lastCompletedMilestoneID) && (date.Date - DateTime.Today).Days <= alertConfig.DaysBefore;
    }

    private void onEPassMessageActivity(object sender, EPassMessageEventArgs eventArgs)
    {
      if (this.currentLoan == null)
        return;
      if (eventArgs.EventType == EPassMessageEventType.MessagesSynced)
      {
        this.queueAsyncRefresh();
      }
      else
      {
        if (eventArgs.EventType != EPassMessageEventType.MessageArrived || !(eventArgs.Message.LoanGuid == this.currentLoan.GUID))
          return;
        this.queueAsyncRefresh();
      }
    }

    private void tableLayoutPanel1_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
    {
      if (e.Row != 0)
        return;
      e.Graphics.FillRectangle(Brushes.LemonChiffon, e.CellBounds);
    }

    private void panel2_Click(object sender, EventArgs e)
    {
      JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, "LogLockMode");
    }

    private void MainScreen_RefreshAlertLogAsync(object sender, EventArgs eventArgs)
    {
      this.queueAsyncRefresh(false);
    }

    private void checkAutoImportMessage(EPassMessageInfo msgInfo, EPassMessageAction action)
    {
      bool flag = false;
      if (action.ActionType == EPassMessageActionType.Command && ((EPassMessageCommandAction) action).Command != null && ((EPassMessageCommandAction) action).Command.ToLower().Contains("investorconnect_investordocuments") && !msgInfo.Description.ToLower().Contains("error"))
      {
        if (this.session.UserInfo.IsAdministrator() || this.session.StartupInfo.UserAclFeatureRights.Contains((object) AclFeature.eFolder_Other_NewDuplicateDoc) && (bool) this.session.StartupInfo.UserAclFeatureRights[(object) AclFeature.eFolder_Other_NewDuplicateDoc] && this.session.StartupInfo.UserAclFeatureRights.Contains((object) AclFeature.eFolder_UD_ED_BrowseAndAttach) && (bool) this.session.StartupInfo.UserAclFeatureRights[(object) AclFeature.eFolder_UD_ED_BrowseAndAttach])
          flag = true;
      }
      else if (action.ActionType == EPassMessageActionType.Command && (action.Description.ToLower().Contains("open efolder") || action.Description.ToLower().Contains("retrieve document") || action.Description.ToLower().Contains("retrieve package")))
      {
        eFolderAccessRights folderAccessRights = new eFolderAccessRights(this.session.LoanDataMgr);
        switch (action.Description.ToLower())
        {
          case "open efolder":
            if (this.session.UserInfo.IsAdministrator() || folderAccessRights.CanAccessDocumentTab)
            {
              flag = true;
              break;
            }
            break;
          case "retrieve document":
            if (this.session.UserInfo.IsAdministrator() || folderAccessRights.CanAccessDocumentTab && folderAccessRights.CanRetrieveDocuments)
            {
              flag = true;
              break;
            }
            break;
          case "retrieve package":
            if ((this.session.UserInfo.IsAdministrator() || (bool) this.session.LoanDataMgr.SessionObjects.StartupInfo.UserAclFeatureRights[(object) AclFeature.ToolsTab_DisclosureTracking]) && (msgInfo.MessageType.Equals("EDISCLOSURE_DOC_FULFILLMENT", StringComparison.OrdinalIgnoreCase) || msgInfo.MessageType.Equals("EDISCLOSURE_DOC_DOCNOTVIEWED", StringComparison.OrdinalIgnoreCase)))
            {
              flag = true;
              break;
            }
            break;
        }
      }
      else
      {
        switch (msgInfo.Description)
        {
          case "Shipping Details Auto Imported":
            if (this.session.UserInfo.IsAdministrator() || this.session.StartupInfo.UserAclFeatureRights.Contains((object) AclFeature.ToolsTab_ImportShippingDetails) && (bool) this.session.StartupInfo.UserAclFeatureRights[(object) AclFeature.ToolsTab_ImportShippingDetails])
            {
              flag = true;
              break;
            }
            break;
          case "Funding Details Auto Imported":
            if (this.session.UserInfo.IsAdministrator() || this.session.StartupInfo.UserAclFeatureRights.Contains((object) AclFeature.ToolsTab_FundingImportWS) && (bool) this.session.StartupInfo.UserAclFeatureRights[(object) AclFeature.ToolsTab_FundingImportWS])
            {
              flag = true;
              break;
            }
            break;
          case "Conditions Details Auto Imported":
            eFolderAccessRights folderAccessRights1 = new eFolderAccessRights(Session.LoanDataMgr);
            if (folderAccessRights1.CanImportAllEnhancedConditions() && folderAccessRights1.CanReviewAndImportEnhancedConditions())
            {
              flag = true;
              break;
            }
            break;
        }
      }
      if (!flag)
        return;
      this.session.ConfigurationManager.DeleteEPassMessages(new string[1]
      {
        msgInfo.MessageID
      });
      this.queueAsyncRefresh();
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      this.toolTip1 = new ToolTip(this.components);
      this.btnNewLog = new StandardIconButton();
      this.mnuContext = new ContextMenuStrip(this.components);
      this.mnuExpandAll = new ToolStripMenuItem();
      this.mnuCollapseAll = new ToolStripMenuItem();
      this.tabsLog = new TabControlEx();
      this.tpAlerts = new TabPageEx();
      this.gvAlerts = new GridView();
      this.tpLog = new TabPageEx();
      this.tableLayoutPanel1 = new TableLayoutPanel();
      this.gvLog = new GridView();
      this.panel1 = new Panel();
      this.panel2 = new Panel();
      this.label1 = new Label();
      ((ISupportInitialize) this.btnNewLog).BeginInit();
      this.mnuContext.SuspendLayout();
      this.tabsLog.SuspendLayout();
      this.tpAlerts.SuspendLayout();
      this.tpLog.SuspendLayout();
      this.tableLayoutPanel1.SuspendLayout();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      this.btnNewLog.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnNewLog.BackColor = Color.Transparent;
      this.btnNewLog.Location = new Point(240, 3);
      this.btnNewLog.MouseDownImage = (Image) null;
      this.btnNewLog.Name = "btnNewLog";
      this.btnNewLog.Size = new Size(16, 16);
      this.btnNewLog.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnNewLog.TabIndex = 1;
      this.btnNewLog.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnNewLog, "Add Log Entry");
      this.btnNewLog.Click += new EventHandler(this.btnNewLog_Click);
      this.mnuContext.Items.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.mnuExpandAll,
        (ToolStripItem) this.mnuCollapseAll
      });
      this.mnuContext.Name = "mnuContext";
      this.mnuContext.ShowImageMargin = false;
      this.mnuContext.Size = new Size(112, 48);
      this.mnuExpandAll.Name = "mnuExpandAll";
      this.mnuExpandAll.Size = new Size(111, 22);
      this.mnuExpandAll.Text = "&Expand All";
      this.mnuExpandAll.Click += new EventHandler(this.mnuExpandAll_Click);
      this.mnuCollapseAll.Name = "mnuCollapseAll";
      this.mnuCollapseAll.Size = new Size(111, 22);
      this.mnuCollapseAll.Text = "&Collapse All";
      this.mnuCollapseAll.Click += new EventHandler(this.mnuCollapseAll_Click);
      this.tabsLog.BackColor = Color.Transparent;
      this.tabsLog.Dock = DockStyle.Fill;
      this.tabsLog.Location = new Point(0, 0);
      this.tabsLog.Name = "tabsLog";
      this.tabsLog.Size = new Size(259, 285);
      this.tabsLog.TabHeight = 20;
      this.tabsLog.TabIndex = 0;
      this.tabsLog.TabPages.Add(this.tpAlerts);
      this.tabsLog.TabPages.Add(this.tpLog);
      this.tpAlerts.BackColor = Color.Transparent;
      this.tpAlerts.Controls.Add((Control) this.gvAlerts);
      this.tpAlerts.Location = new Point(1, 23);
      this.tpAlerts.Name = "tpAlerts";
      this.tpAlerts.TabIndex = 0;
      this.tpAlerts.TabWidth = 150;
      this.tpAlerts.Text = "Alerts & Messages";
      this.tpAlerts.Value = (object) "Alerts & Messages";
      this.gvAlerts.AllowMultiselect = false;
      this.gvAlerts.AlternatingColors = false;
      this.gvAlerts.BorderColor = SystemColors.Control;
      this.gvAlerts.BorderStyle = BorderStyle.FixedSingle;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.SpringToFit = true;
      gvColumn1.Text = "Text";
      gvColumn1.Width = 188;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Date";
      gvColumn2.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn2.Width = 65;
      this.gvAlerts.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.gvAlerts.Dock = DockStyle.Fill;
      this.gvAlerts.HeaderHeight = 0;
      this.gvAlerts.HeaderVisible = false;
      this.gvAlerts.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvAlerts.Location = new Point(0, 0);
      this.gvAlerts.Name = "gvAlerts";
      this.gvAlerts.Selectable = false;
      this.gvAlerts.Size = new Size((int) byte.MaxValue, 259);
      this.gvAlerts.TabIndex = 0;
      this.gvAlerts.SubItemClick += new GVSubItemMouseEventHandler(this.gvAlerts_SubItemClick);
      this.tpLog.BackColor = Color.Transparent;
      this.tpLog.Controls.Add((Control) this.tableLayoutPanel1);
      this.tpLog.Location = new Point(1, 23);
      this.tpLog.Name = "tpLog";
      this.tpLog.TabIndex = 0;
      this.tpLog.TabWidth = 42;
      this.tpLog.Text = "Log";
      this.tpLog.Value = (object) "Log";
      this.tableLayoutPanel1.BackColor = Color.Transparent;
      this.tableLayoutPanel1.ColumnCount = 1;
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
      this.tableLayoutPanel1.Controls.Add((Control) this.gvLog, 0, 1);
      this.tableLayoutPanel1.Controls.Add((Control) this.panel1, 0, 0);
      this.tableLayoutPanel1.Dock = DockStyle.Fill;
      this.tableLayoutPanel1.Location = new Point(0, 0);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.RowCount = 2;
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 10f));
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 90f));
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20f));
      this.tableLayoutPanel1.Size = new Size(200, 100);
      this.tableLayoutPanel1.TabIndex = 2;
      this.tableLayoutPanel1.CellPaint += new TableLayoutCellPaintEventHandler(this.tableLayoutPanel1_CellPaint);
      this.gvLog.AllowMultiselect = false;
      this.gvLog.AlternatingColors = false;
      this.gvLog.BorderColor = SystemColors.Control;
      this.gvLog.BorderStyle = BorderStyle.FixedSingle;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column1";
      gvColumn3.SpringToFit = true;
      gvColumn3.Text = "Text";
      gvColumn3.Width = 182;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column2";
      gvColumn4.Text = "Date";
      gvColumn4.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn4.Width = 65;
      this.gvLog.Columns.AddRange(new GVColumn[2]
      {
        gvColumn3,
        gvColumn4
      });
      this.gvLog.ContextMenuStrip = this.mnuContext;
      this.gvLog.Dock = DockStyle.Fill;
      this.gvLog.HeaderHeight = 0;
      this.gvLog.HeaderVisible = false;
      this.gvLog.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvLog.ItemGrouping = true;
      this.gvLog.ItemGroupingIndentWidth = 0;
      this.gvLog.Location = new Point(3, 13);
      this.gvLog.Name = "gvLog";
      this.gvLog.Selectable = false;
      this.gvLog.Size = new Size(194, 84);
      this.gvLog.TabIndex = 1;
      this.gvLog.SubItemClick += new GVSubItemMouseEventHandler(this.gvLog_SubItemClick);
      this.panel1.Controls.Add((Control) this.panel2);
      this.panel1.Controls.Add((Control) this.label1);
      this.panel1.Dock = DockStyle.Fill;
      this.panel1.Location = new Point(3, 3);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(194, 4);
      this.panel1.TabIndex = 2;
      this.panel2.BackgroundImage = (Image) Resources.info;
      this.panel2.BackgroundImageLayout = ImageLayout.Center;
      this.panel2.Cursor = Cursors.Help;
      this.panel2.Location = new Point(3, 1);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(18, 18);
      this.panel2.TabIndex = 2;
      this.panel2.Click += new EventHandler(this.panel2_Click);
      this.label1.ImageAlign = ContentAlignment.MiddleLeft;
      this.label1.Location = new Point(3, -3);
      this.label1.Name = "label1";
      this.label1.Size = new Size(198, 25);
      this.label1.TabIndex = 1;
      this.label1.Text = "        Milestone List is in manual mode.\r\n          Milestone Date is in manual mode.\r\n";
      this.label1.TextAlign = ContentAlignment.MiddleCenter;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.Transparent;
      this.Controls.Add((Control) this.btnNewLog);
      this.Controls.Add((Control) this.tabsLog);
      this.Name = nameof (LogViewer);
      this.Size = new Size(259, 285);
      this.Load += new EventHandler(this.LogViewer_Load);
      ((ISupportInitialize) this.btnNewLog).EndInit();
      this.mnuContext.ResumeLayout(false);
      this.tabsLog.ResumeLayout(false);
      this.tpAlerts.ResumeLayout(false);
      this.tpLog.ResumeLayout(false);
      this.tableLayoutPanel1.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
