// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.TradeUpdateLoansDialog
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Server;
using EllieMae.EMLite.UI;
using Newtonsoft.Json;
using RestApiProxy.WebhookService;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class TradeUpdateLoansDialog : Form
  {
    private const string className = "TradeUpdateLoansDialog";
    private static TradeUpdateLoansDialog _instance = (TradeUpdateLoansDialog) null;
    private static readonly string sw = Tracing.SwEFolder;
    private ISession session;
    private TradeLoanUpdateQueue<TradeLoanUpdateQueueItem> queue = new TradeLoanUpdateQueue<TradeLoanUpdateQueueItem>();
    private TradeLoanUpdateQueueItem currentQueueItem;
    private ITradeJob currentLoanJob;
    private bool _isCloseEncompass;
    private int loanCount;
    private bool cancelRequested;
    private List<TradeLoanUpdateError> errors = new List<TradeLoanUpdateError>();
    private int sequentialCompletedCount;
    private int parallelCompletedCount;
    private int completedSuccessfullyCount;
    private int completedWithErrorsCount;
    private IContainer components;
    private Button btnClose;
    private BackgroundWorker updateWorker;
    private Button btnRefresh;
    private GridView gvTrades;
    private RadioButton rbtnAllJobs;
    private RadioButton rbtnSessionJob;
    private RadioButton rbtnAdminJob;
    private Label label1;
    private DatePicker dtClear;
    private Button btnClear;

    public TradeUpdateLoansDialog() => this.InitializeComponent();

    private TradeUpdateLoansDialog(ISession session)
    {
      this.InitializeComponent();
      this.session = session;
      this.btnClear.Enabled = false;
      this.dtClear.MaxValue = DateTime.Today.AddDays(-1.0);
    }

    public static TradeUpdateLoansDialog Instance => TradeUpdateLoansDialog._instance;

    public static void CreateInstance(ISession session)
    {
      if (TradeUpdateLoansDialog._instance != null)
        return;
      Tracing.Log(TradeUpdateLoansDialog.sw, TraceLevel.Verbose, nameof (TradeUpdateLoansDialog), "Creating TradeUpdateLoansDialog");
      TradeUpdateLoansDialog._instance = new TradeUpdateLoansDialog(session);
      Tracing.Log(TradeUpdateLoansDialog.sw, TraceLevel.Verbose, nameof (TradeUpdateLoansDialog), "Checking IsHandleCreated");
      if (TradeUpdateLoansDialog._instance.IsHandleCreated)
        return;
      Tracing.Log(TradeUpdateLoansDialog.sw, TraceLevel.Verbose, nameof (TradeUpdateLoansDialog), "Calling CreateHandle");
      TradeUpdateLoansDialog._instance.CreateHandle();
    }

    public static void ShowInstance()
    {
      if (TradeUpdateLoansDialog._instance == null)
        return;
      if (TradeUpdateLoansDialog._instance.WindowState == FormWindowState.Minimized)
      {
        TradeUpdateLoansDialog._instance.WindowState = FormWindowState.Normal;
        TradeUpdateLoansDialog._instance.Activate();
      }
      else
      {
        TradeUpdateLoansDialog._instance.Show();
        TradeUpdateLoansDialog._instance.BringToFront();
        TradeUpdateLoansDialog._instance.rbtnAdminJob.Checked = true;
        TradeUpdateLoansDialog._instance.rbtnAdminJob.Text = "Show jobs submitted by " + Session.UserInfo.FullName;
      }
    }

    public static bool IsActive
    {
      get
      {
        bool isActive = false;
        if ((bool) Session.StartupInfo.UserAclFeatureRights[(object) AclFeature.TradeTab_Trades])
          isActive = true;
        return isActive;
      }
    }

    public static void Insert(
      MbsPoolProcesses.ActionType actionType,
      MbsPoolInfo mbsPoolInfo,
      bool flag,
      Decimal price,
      MbsPoolLoanAssignment[] assignments,
      List<string> skipFieldList,
      bool forceUpdateOfAllLoans)
    {
      if (TradeUpdateLoansDialog._instance == null)
        return;
      TradeLoanUpdateQueueItem queueItem = new TradeLoanUpdateQueueItem();
      TradeLoanUpdateJobInfo jobInfo = new TradeLoanUpdateJobInfo()
      {
        JobGuid = Guid.NewGuid().ToString(),
        TradeID = mbsPoolInfo.TradeID,
        TradeName = mbsPoolInfo.Name,
        TradeType = mbsPoolInfo.TradeType,
        CreatedBy = Session.UserInfo.FullName,
        SessionID = Session.SessionObjects.SessionID,
        CreatedDate = DateTime.Now
      };
      queueItem.LoanUpdateJobInfo = jobInfo;
      queueItem.TradePoolInfo = (TradeInfoObj) mbsPoolInfo;
      queueItem.AssignmentItems = new List<IAssignmentItem>();
      queueItem.SkipFieldList = skipFieldList;
      queueItem.ForceUpdateOfAllLoans = forceUpdateOfAllLoans;
      foreach (MbsPoolLoanAssignment assignment in assignments)
        queueItem.AssignmentItems.Add((IAssignmentItem) new MbsPoolLoanAssignmentItem(mbsPoolInfo, assignment, price));
      TradeUpdateLoansDialog.SetTradeUpdateStatus(jobInfo, (object) mbsPoolInfo, 1);
      TradeUpdateLoansDialog._instance.addQueueItem(queueItem, true);
    }

    public static void Insert(
      CorrespondentTradeProcesses.ActionType actionType,
      CorrespondentTradeInfo corrTradeInfo,
      bool flag,
      Decimal price,
      CorrespondentTradeLoanAssignment[] assignments,
      List<string> skipFieldList,
      bool forceUpdateOfAllLoans)
    {
      if (TradeUpdateLoansDialog._instance == null)
        return;
      TradeLoanUpdateQueueItem queueItem = new TradeLoanUpdateQueueItem();
      TradeLoanUpdateJobInfo jobInfo = new TradeLoanUpdateJobInfo()
      {
        JobGuid = Guid.NewGuid().ToString(),
        TradeID = corrTradeInfo.TradeID,
        TradeName = corrTradeInfo.Name,
        TradeType = corrTradeInfo.TradeType,
        CreatedBy = Session.UserInfo.FullName,
        SessionID = Session.SessionObjects.SessionID,
        CreatedDate = DateTime.Now,
        UpdateActionType = (TradeLoanUpdateJobInfo.ActionType) Enum.Parse(typeof (TradeLoanUpdateJobInfo.ActionType), actionType.ToString())
      };
      queueItem.LoanUpdateJobInfo = jobInfo;
      queueItem.TradePoolInfo = (TradeInfoObj) corrTradeInfo;
      queueItem.AssignmentItems = new List<IAssignmentItem>();
      queueItem.SkipFieldList = skipFieldList;
      queueItem.ForceUpdateOfAllLoans = forceUpdateOfAllLoans;
      foreach (CorrespondentTradeLoanAssignment assignment in assignments)
        queueItem.AssignmentItems.Add((IAssignmentItem) new CorrespondentTradeAssignmentItem(corrTradeInfo, assignment, price));
      corrTradeInfo.CopyOfStatus = corrTradeInfo.Status;
      TradeUpdateLoansDialog.SetTradeUpdateStatus(jobInfo, (object) corrTradeInfo, 1);
      TradeUpdateLoansDialog._instance.addQueueItem(queueItem, true);
    }

    public static void Insert(
      TradeProcesses2.ActionType actionType,
      LoanTradeInfo loanTradeInfo,
      bool flag,
      Decimal price,
      LoanTradeAssignment[] assignments,
      List<string> skipFieldList,
      bool forceUpdateOfAllLoans)
    {
      if (TradeUpdateLoansDialog._instance == null)
        return;
      TradeLoanUpdateQueueItem queueItem = new TradeLoanUpdateQueueItem();
      TradeLoanUpdateJobInfo jobInfo = new TradeLoanUpdateJobInfo()
      {
        JobGuid = Guid.NewGuid().ToString(),
        TradeID = loanTradeInfo.TradeID,
        TradeName = loanTradeInfo.Name,
        TradeType = loanTradeInfo.TradeType,
        CreatedBy = Session.UserInfo.FullName,
        SessionID = Session.SessionObjects.SessionID,
        CreatedDate = DateTime.Now
      };
      queueItem.LoanUpdateJobInfo = jobInfo;
      queueItem.TradePoolInfo = (TradeInfoObj) loanTradeInfo;
      queueItem.AssignmentItems = new List<IAssignmentItem>();
      queueItem.SkipFieldList = skipFieldList;
      queueItem.ForceUpdateOfAllLoans = forceUpdateOfAllLoans;
      foreach (LoanTradeAssignment assignment in assignments)
        queueItem.AssignmentItems.Add((IAssignmentItem) new AssignmentItem(loanTradeInfo, assignment, price));
      TradeUpdateLoansDialog.SetTradeUpdateStatus(jobInfo, (object) loanTradeInfo, 1);
      TradeUpdateLoansDialog._instance.addQueueItem(queueItem, true);
    }

    private void addQueueItem(TradeLoanUpdateQueueItem queueItem, bool processItem)
    {
      if (this.gvTrades.Items.GetItemByTag((object) queueItem) == null)
      {
        GVItem gvItem = new GVItem();
        gvItem.SubItems.Add((object) queueItem.LoanUpdateJobInfo.TradeName);
        gvItem.SubItems.Add((object) queueItem.LoanUpdateJobInfo.TradeType);
        gvItem.SubItems.Add((object) queueItem.LoanUpdateJobInfo.CreatedDate);
        gvItem.SubItems.Add((object) queueItem.LoanUpdateJobInfo.CreatedBy);
        gvItem.SubItems.Add((object) "");
        gvItem.SubItems.Add((object) "");
        gvItem.SubItems.Add((object) "");
        gvItem.Tag = (object) queueItem;
        this.gvTrades.Items.Add(gvItem);
        gvItem.SubItems["STATUS"].Value = (object) "Submitted";
        gvItem.SubItems["UPDATEPROGRESSSTATUS"].Value = (object) "Pending";
        gvItem.SubItems["ACTION"].Value = (object) this.BuildActionLabelLinks(TradeLoanUpdateQueueStatus.InQueue, queueItem);
        queueItem.LoanUpdateJobInfo.JobStatus = TradeLoanUpdateQueueStatus.InQueue;
        queueItem.LoanUpdateJobInfo.TotalLoans = queueItem.AssignmentItems.Count;
        queueItem.LoanUpdateJobInfo.LoansCompleted = 0;
        queueItem.LoanUpdateJobInfo.LastUpdateDate = DateTime.Now;
        queueItem.LoanUpdateJobInfo.JobEndDate = DateTime.MinValue;
        queueItem.LoanUpdateJobInfo.Results = (string) null;
        queueItem.LoanUpdateJobInfo.CancelledBy = (string) null;
        queueItem.LoanUpdateJobInfo.CancelledDate = DateTime.MinValue;
        queueItem.LoanUpdateJobInfo.DeletedBy = (string) null;
        queueItem.LoanUpdateJobInfo.DeletedDate = DateTime.MinValue;
        queueItem.LoanUpdateJobInfo.SessionLastUpdateDate = queueItem.LoanUpdateJobInfo.LastUpdateDate;
        Session.TradeLoanUpdateQueueManager.CreateQueue(queueItem.LoanUpdateJobInfo);
        this.applyFilter();
      }
      Tracing.Log(TradeUpdateLoansDialog.sw, TraceLevel.Verbose, nameof (TradeUpdateLoansDialog), "Lock Trade Loan Update Queue For Add: " + (object) queueItem.LoanUpdateJobInfo.TradeID);
      lock (this.queue)
      {
        Tracing.Log(TradeUpdateLoansDialog.sw, TraceLevel.Verbose, nameof (TradeUpdateLoansDialog), "Add To Trade Loan Update Queue: " + (object) queueItem.LoanUpdateJobInfo.TradeID);
        this.queue.Enqueue(queueItem);
      }
      if (!processItem)
        return;
      this.processQueue();
    }

    private void processQueue()
    {
      Tracing.Log(TradeUpdateLoansDialog.sw, TraceLevel.Verbose, nameof (TradeUpdateLoansDialog), "Checking Trade Loan Update Worker");
      if (this.updateWorker.IsBusy)
        return;
      Tracing.Log(TradeUpdateLoansDialog.sw, TraceLevel.Verbose, nameof (TradeUpdateLoansDialog), "Lock Trade Loan Update Queue For Processing");
      lock (this.queue)
      {
        Tracing.Log(TradeUpdateLoansDialog.sw, TraceLevel.Verbose, nameof (TradeUpdateLoansDialog), "Checking Queue Count: " + (object) this.queue.Count);
        if (this.queue.Count == 0)
          return;
        Tracing.Log(TradeUpdateLoansDialog.sw, TraceLevel.Verbose, nameof (TradeUpdateLoansDialog), "Getting Next Queue Item");
        this.currentQueueItem = this.queue.Dequeue();
        Tracing.Log(TradeUpdateLoansDialog.sw, TraceLevel.Verbose, nameof (TradeUpdateLoansDialog), "Starting Update Thread: " + (object) this.currentQueueItem.LoanUpdateJobInfo.TradeID);
        this.updateWorker.RunWorkerAsync((object) this.currentQueueItem);
      }
    }

    private void updateWorker_DoWork(object sender, DoWorkEventArgs e)
    {
      Tracing.Log(TradeUpdateLoansDialog.sw, TraceLevel.Verbose, nameof (TradeUpdateLoansDialog), "Trade Loan Update Worker Started");
      TradeLoanUpdateQueueItem queueItem = (TradeLoanUpdateQueueItem) e.Argument;
      GVItem itemByTag = this.gvTrades.Items.GetItemByTag((object) queueItem);
      if (itemByTag != null)
      {
        itemByTag.SubItems["STATUS"].Value = (object) "In Progress";
        itemByTag.SubItems["UPDATEPROGRESSSTATUS"].Value = (object) new ProgressElement(2M, true);
        itemByTag.SubItems["ACTION"].Value = (object) this.BuildActionLabelLinks(TradeLoanUpdateQueueStatus.InProgress, queueItem);
      }
      Dictionary<string, object> updateValue = new Dictionary<string, object>();
      updateValue.Add("JobStatus", (object) 2);
      updateValue.Add("LastUpdatedDate", (object) DateTime.Now);
      updateValue.Add("JobStartDate", (object) DateTime.Now);
      updateValue.Add("Results", (object) ("Processed: 0, Successful: 0, Errors: 0, Not Processed: " + (object) queueItem.AssignmentItems.Count));
      Session.TradeLoanUpdateQueueManager.UpdateQueue(updateValue, queueItem.LoanUpdateJobInfo.JobGuid);
      this.updateGvItemTag(updateValue, queueItem.LoanUpdateJobInfo.JobGuid);
      this.loanCount = queueItem.AssignmentItems.Count;
      this.parallelCompletedCount = 0;
      try
      {
        this.CreateCurrentLoanJob(queueItem);
        this.currentLoanJob.ForceUpdateOfAllLoans = queueItem.ForceUpdateOfAllLoans;
        this.currentLoanJob.SkipFieldList = queueItem.SkipFieldList;
        if (this.currentLoanJob is CorrespondentTradeParallelLoanUpdateJob && queueItem.LoanUpdateJobInfo.UpdateActionType == TradeLoanUpdateJobInfo.ActionType.ExtendLock)
          this.currentLoanJob.SkipFieldList = new List<string>()
          {
            "ExtendLock"
          };
        this.currentLoanJob.JobGuid = queueItem.LoanUpdateJobInfo.JobGuid;
        this.currentLoanJob.Completed += new TradeUpdateEventHandler(this.ParallelLoanJob_Completed);
        this.currentLoanJob.ProgressChanged += new ProgressChangedEventHandler(this.ParallelLoanJob_ProgressChanged);
        this.currentLoanJob.Cancelled += new TradeUpdateEventHandler(this.ParallelLoanJob_Cancelled);
        this.currentLoanJob.StartAsyncBackground();
      }
      catch (CanceledOperationException ex)
      {
        Tracing.Log(TradeUpdateLoansDialog.sw, TraceLevel.Verbose, nameof (TradeUpdateLoansDialog), "Worker Cancelled");
        this.updateWorker.ReportProgress(int.MinValue, (object) queueItem.LoanUpdateJobInfo.TradeID);
      }
      catch (Exception ex)
      {
        Tracing.Log(TradeUpdateLoansDialog.sw, TraceLevel.Verbose, nameof (TradeUpdateLoansDialog), ex.ToString());
        this.updateWorker.ReportProgress(int.MaxValue, (object) queueItem.LoanUpdateJobInfo.TradeID);
      }
    }

    private void updateWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      string tradeName = this.currentQueueItem.LoanUpdateJobInfo.TradeName;
      TradeInfoObj tradePoolInfo = this.currentQueueItem.TradePoolInfo;
      this.currentQueueItem = (TradeLoanUpdateQueueItem) null;
      this.currentLoanJob = (ITradeJob) null;
      if (!this._isCloseEncompass)
      {
        int num = (int) new TradeLoanUpdateNotificationDialog(tradeName, "completed").ShowDialog();
        this.RefreshTradeEditor(tradePoolInfo);
      }
      this._isCloseEncompass = false;
      Tracing.Log(TradeUpdateLoansDialog.sw, TraceLevel.Verbose, nameof (TradeUpdateLoansDialog), "Trade Loan Update Worker Complete");
      this.processQueue();
    }

    private void ParallelLoanJob_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
      this.parallelCompletedCount = this.currentLoanJob.LoansCompleted;
      GVItem gvItem = this.GetGvItem(e.UserState.ToString());
      if (gvItem != null && e.ProgressPercentage != 100)
        gvItem.SubItems["UPDATEPROGRESSSTATUS"].Value = (object) new ProgressElement((Decimal) e.ProgressPercentage, true);
      Dictionary<string, object> updateValue = new Dictionary<string, object>();
      updateValue.Add("LastUpdatedDate", (object) DateTime.Now);
      updateValue.Add("LoansCompleted", (object) this.parallelCompletedCount);
      string str = this.BuildResultsMessage(this.currentLoanJob.LoansCompleted, this.currentLoanJob.LoansSkipped, this.currentLoanJob.Errors.Count, this.currentQueueItem.AssignmentItems.Count);
      updateValue.Add("Results", (object) str);
      Session.TradeLoanUpdateQueueManager.UpdateQueue(updateValue, e.UserState.ToString());
      this.updateGvItemTag(updateValue, e.UserState.ToString());
    }

    private void ParallelLoanJob_Cancelled(object sender, TradeUpdateEventArgs eventArgs)
    {
      string str = this.BuildResultsMessage(this.currentLoanJob.LoansCompleted, this.currentLoanJob.LoansSkipped, this.currentLoanJob.Errors.Count, this.currentQueueItem.AssignmentItems.Count);
      string noteMesg = Session.UserInfo.FullName + " at " + (object) DateTime.Now + ": " + str;
      this.UpdateDababaseToCancelled(eventArgs.Info.ToString(), str);
      this.processCompleted(this.currentQueueItem, TradeLoanUpdateQueueStatus.Cancelled, noteMesg, str);
    }

    private void ParallelLoanJob_Completed(object sender, TradeUpdateEventArgs eventArgs)
    {
      Dictionary<string, object> updateValue = new Dictionary<string, object>();
      updateValue.Add("JobStatus", (object) 4);
      updateValue.Add("LastUpdatedDate", (object) DateTime.Now);
      updateValue.Add("JobEndDate", (object) DateTime.Now);
      string str = this.BuildResultsMessage(this.currentLoanJob.LoansCompleted, this.currentLoanJob.LoansSkipped, this.currentLoanJob.Errors.Count, this.currentQueueItem.AssignmentItems.Count);
      updateValue.Add("Results", (object) str);
      Session.TradeLoanUpdateQueueManager.UpdateQueue(updateValue, eventArgs.Info.ToString());
      this.updateGvItemTag(updateValue, eventArgs.Info.ToString());
      this.processCompleted(this.currentQueueItem, TradeLoanUpdateQueueStatus.Completed, str, str);
    }

    private void RefreshTradeEditor(TradeInfoObj obj)
    {
      if (TradeManagementConsole.Instance.CurrentScreen == TradeManagementScreen.CorrespondentTrades)
        (TradeManagementConsole.Instance.GetCurrentScreen() as CorrespondentTradeListScreen).RefreshList(true);
      ITradeEditor editor = TradeManagementConsole.Instance.GetActiveTradeEditor();
      if (editor == null || obj == null || obj.TradeID != editor.CurrentTradeInfo.TradeID)
        return;
      if (editor is TradeEditor && obj is LoanTradeInfo)
        ((Control) editor).Invoke((Delegate) (() => ((TradeEditor) editor).RefreshData((LoanTradeInfo) obj, obj.Status == TradeStatus.Archived ? 1 : 0)));
      if (editor is MbsPoolEditor && obj is MbsPoolInfo)
        ((Control) editor).Invoke((Delegate) (() => ((MbsPoolEditor) editor).RefreshData((MbsPoolInfo) obj)));
      if (editor is FannieMaePEPoolEditor && obj is MbsPoolInfo)
        ((Control) editor).Invoke((Delegate) (() => ((FannieMaePEPoolEditor) editor).RefreshData((MbsPoolInfo) obj)));
      if (!(editor is CorrespondentTradeEditor) || !(obj is CorrespondentTradeInfo))
        return;
      ((Control) editor).Invoke((Delegate) (() => ((CorrespondentTradeEditor) editor).RefreshData((CorrespondentTradeInfo) obj)));
    }

    private void processCompleted(
      TradeLoanUpdateQueueItem queueItem,
      TradeLoanUpdateQueueStatus status,
      string noteMesg,
      string gvItemMesg)
    {
      this.currentLoanJob.Completed -= new TradeUpdateEventHandler(this.ParallelLoanJob_Completed);
      this.currentLoanJob.ProgressChanged -= new ProgressChangedEventHandler(this.ParallelLoanJob_ProgressChanged);
      this.currentLoanJob.Cancelled -= new TradeUpdateEventHandler(this.ParallelLoanJob_Cancelled);
      this.parallelCompletedCount = this.currentLoanJob.LoansCompleted;
      this.errors = new List<TradeLoanUpdateError>();
      if (this.currentLoanJob.HadErrors)
      {
        this.errors.AddRange((IEnumerable<TradeLoanUpdateError>) this.currentLoanJob.Errors);
        this.completedWithErrorsCount += this.currentLoanJob.Errors.Count;
      }
      this.completedSuccessfullyCount += this.currentLoanJob.LoansCompleted - this.currentLoanJob.LoansSkipped - this.currentLoanJob.Errors.Count;
      TradeUpdateLoansDialog.SetTradeUpdateStatus(queueItem.LoanUpdateJobInfo, (object) queueItem.TradePoolInfo, 0, this.currentLoanJob.Errors.Count);
      TradeUpdateLoansDialog.CreateNotesHistory(status, queueItem.TradePoolInfo, this.currentLoanJob.Errors, noteMesg);
      this.UpdateGvItem(queueItem.LoanUpdateJobInfo.JobGuid, status, this.currentLoanJob.Errors.Count > 0, gvItemMesg);
      this.PublishKafkaEvent(status, queueItem.TradePoolInfo, this.currentLoanJob.Errors, noteMesg, queueItem.LoanUpdateJobInfo.UpdateActionType, queueItem.AssignmentItems);
    }

    private Control BuildActionLabelLinks(
      TradeLoanUpdateQueueStatus status,
      TradeLoanUpdateQueueItem queueItem)
    {
      Control control = (Control) null;
      string jobGuid = queueItem.LoanUpdateJobInfo.JobGuid;
      if (status == TradeLoanUpdateQueueStatus.Failed && this.hasClearActionPermission(queueItem))
        control = (Control) this.BuildClearLinkLabel(jobGuid);
      else if ((status == TradeLoanUpdateQueueStatus.InQueue || status == TradeLoanUpdateQueueStatus.InProgress) && this.hasCancelActionPermission(queueItem))
        control = (Control) this.BuildCancelLinkLabel(jobGuid);
      else if (status != TradeLoanUpdateQueueStatus.InQueue && status != TradeLoanUpdateQueueStatus.InProgress && this.hasClearActionPermission(queueItem))
        control = (Control) new TradeLoanUpdateResults(Session.DefaultInstance, jobGuid, queueItem.LoanUpdateJobInfo.TradeID, queueItem.LoanUpdateJobInfo.TradeType, this.gvTrades, this.GetGvItem(jobGuid));
      return control;
    }

    private bool hasCancelActionPermission(TradeLoanUpdateQueueItem queueItem)
    {
      return queueItem.LoanUpdateJobInfo.SessionID == Session.SessionObjects.SessionID;
    }

    private bool hasClearActionPermission(TradeLoanUpdateQueueItem queueItem)
    {
      return queueItem.LoanUpdateJobInfo.CreatedBy == Session.UserInfo.FullName || Session.UserInfo.IsAdministrator();
    }

    private System.Windows.Forms.LinkLabel BuildCancelLinkLabel(string jobGuid)
    {
      System.Windows.Forms.LinkLabel linkLabel = new System.Windows.Forms.LinkLabel();
      linkLabel.Text = "Cancel";
      linkLabel.Links[0].LinkData = (object) jobGuid;
      linkLabel.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkLabelCancel_LinkClicked);
      return linkLabel;
    }

    private void linkLabelCancel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      ((System.Windows.Forms.LinkLabel) sender).LinkVisited = true;
      GVItem gvItem = this.GetGvItem(e.Link.LinkData.ToString());
      if (this.gvTrades == null || ((TradeLoanUpdateQueueItem) gvItem.Tag).LoanUpdateJobInfo.JobStatus == TradeLoanUpdateQueueStatus.InProgress && Utils.Dialog((IWin32Window) this, "Encompass has started the update process. Cancelling may result in some loans not being updated. Are you sure you want to proceed?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
        return;
      this.CancelOneJob(e.Link.LinkData.ToString());
    }

    private System.Windows.Forms.LinkLabel BuildClearLinkLabel(string jobGuid)
    {
      System.Windows.Forms.LinkLabel linkLabel = new System.Windows.Forms.LinkLabel();
      linkLabel.Text = "Clear";
      linkLabel.Links[0].LinkData = (object) jobGuid;
      linkLabel.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkLabelClear_LinkClicked);
      return linkLabel;
    }

    private void linkLabelClear_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      ((System.Windows.Forms.LinkLabel) sender).LinkVisited = true;
      string jobGuid = e.Link.LinkData.ToString();
      string str = Session.UserInfo.FullName + " at " + (object) DateTime.Now;
      GVItem gvItem = this.GetGvItem(jobGuid);
      if (gvItem == null)
        return;
      Session.TradeLoanUpdateQueueManager.DeleteQueue(jobGuid);
      this.gvTrades.Items.Remove(gvItem);
    }

    private void btnClose_Click(object sender, EventArgs e) => this.Hide();

    private void TradeUpdateLoansDialog_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (e.CloseReason != CloseReason.UserClosing)
        return;
      e.Cancel = true;
      this.Hide();
    }

    private void btnRefresh_Click(object sender, EventArgs e) => this.applyFilter();

    private List<TradeLoanUpdateJobInfo> RefreshGridviewByDefaultOrder(
      TradeUpdateLoansDialog.filter filter)
    {
      this.gvTrades.ClearSort();
      TradeLoanUpdateJobInfo[] allQueues = Session.TradeLoanUpdateQueueManager.GetAllQueues();
      IEnumerable<TradeLoanUpdateJobInfo> source1 = ((IEnumerable<TradeLoanUpdateJobInfo>) allQueues).Where<TradeLoanUpdateJobInfo>((Func<TradeLoanUpdateJobInfo, bool>) (x => x.SessionID == Session.SessionObjects.SessionID));
      List<TradeLoanUpdateJobInfo> list = source1.Where<TradeLoanUpdateJobInfo>((Func<TradeLoanUpdateJobInfo, bool>) (x => x.JobStatus == TradeLoanUpdateQueueStatus.InQueue || x.JobStatus == TradeLoanUpdateQueueStatus.InProgress)).OrderBy<TradeLoanUpdateJobInfo, DateTime>((Func<TradeLoanUpdateJobInfo, DateTime>) (x => x.CreatedDate)).ToList<TradeLoanUpdateJobInfo>();
      list.AddRange((IEnumerable<TradeLoanUpdateJobInfo>) source1.Where<TradeLoanUpdateJobInfo>((Func<TradeLoanUpdateJobInfo, bool>) (x => x.JobStatus == TradeLoanUpdateQueueStatus.Cancelled || x.JobStatus == TradeLoanUpdateQueueStatus.Completed || x.JobStatus == TradeLoanUpdateQueueStatus.Failed)).OrderBy<TradeLoanUpdateJobInfo, DateTime>((Func<TradeLoanUpdateJobInfo, DateTime>) (x => x.CreatedDate)).ToList<TradeLoanUpdateJobInfo>());
      List<GVItem> gvItemList = new List<GVItem>();
      foreach (TradeLoanUpdateJobInfo loanUpdateJobInfo in list)
      {
        GVItem gvItem = this.GetGvItem(loanUpdateJobInfo.JobGuid);
        if (gvItem != null)
          gvItemList.Add(gvItem);
      }
      if (filter != TradeUpdateLoansDialog.filter.CurrentSession)
      {
        IEnumerable<TradeLoanUpdateJobInfo> source2 = ((IEnumerable<TradeLoanUpdateJobInfo>) allQueues).Where<TradeLoanUpdateJobInfo>((Func<TradeLoanUpdateJobInfo, bool>) (x => x.SessionID != Session.SessionObjects.SessionID));
        switch (filter)
        {
          case TradeUpdateLoansDialog.filter.AllJobs:
            if (Session.UserInfo.IsAdministrator())
              break;
            goto case TradeUpdateLoansDialog.filter.AdminUser;
          case TradeUpdateLoansDialog.filter.AdminUser:
            source2 = source2.Where<TradeLoanUpdateJobInfo>((Func<TradeLoanUpdateJobInfo, bool>) (x => x.CreatedBy == Session.UserInfo.FullName));
            break;
        }
        Dictionary<TradeType, List<TradeLoanUpdateJobInfo>> dictionary = new Dictionary<TradeType, List<TradeLoanUpdateJobInfo>>();
        foreach (TradeLoanUpdateJobInfo loanUpdateJobInfo in source2.Where<TradeLoanUpdateJobInfo>((Func<TradeLoanUpdateJobInfo, bool>) (x => x.JobStatus == TradeLoanUpdateQueueStatus.InProgress || x.JobStatus == TradeLoanUpdateQueueStatus.InQueue)))
        {
          DateTime sessionLastUpdateDate = loanUpdateJobInfo.SessionLastUpdateDate;
          if (loanUpdateJobInfo.SessionLastUpdateDate != DateTime.MinValue && loanUpdateJobInfo.SessionLastUpdateDate.AddMinutes(15.0) < DateTime.Now)
          {
            if (loanUpdateJobInfo.JobStatus == TradeLoanUpdateQueueStatus.InQueue)
              loanUpdateJobInfo.Results = "Encompass session was terminated";
            loanUpdateJobInfo.JobStatus = TradeLoanUpdateQueueStatus.Failed;
            if (!dictionary.ContainsKey(loanUpdateJobInfo.TradeType))
              dictionary.Add(loanUpdateJobInfo.TradeType, new List<TradeLoanUpdateJobInfo>()
              {
                loanUpdateJobInfo
              });
            else
              dictionary[loanUpdateJobInfo.TradeType].Add(loanUpdateJobInfo);
            Session.TradeLoanUpdateQueueManager.UpdateQueue(new Dictionary<string, object>()
            {
              {
                "JobStatus",
                (object) 5
              },
              {
                "LastUpdatedDate",
                (object) DateTime.Now
              },
              {
                "JobEndDate",
                (object) DateTime.Now
              },
              {
                "Results",
                (object) loanUpdateJobInfo.Results
              }
            }, loanUpdateJobInfo.JobGuid);
          }
        }
        if (dictionary.Count<KeyValuePair<TradeType, List<TradeLoanUpdateJobInfo>>>() > 0)
          TradeUpdateLoansDialog.UnlockTrades(dictionary);
        list = source2.Where<TradeLoanUpdateJobInfo>((Func<TradeLoanUpdateJobInfo, bool>) (x => x.JobStatus == TradeLoanUpdateQueueStatus.InProgress)).OrderBy<TradeLoanUpdateJobInfo, DateTime>((Func<TradeLoanUpdateJobInfo, DateTime>) (x => x.CreatedDate)).ToList<TradeLoanUpdateJobInfo>();
        list.AddRange((IEnumerable<TradeLoanUpdateJobInfo>) source2.Where<TradeLoanUpdateJobInfo>((Func<TradeLoanUpdateJobInfo, bool>) (x => x.JobStatus == TradeLoanUpdateQueueStatus.InQueue)).OrderBy<TradeLoanUpdateJobInfo, DateTime>((Func<TradeLoanUpdateJobInfo, DateTime>) (x => x.CreatedDate)).ToList<TradeLoanUpdateJobInfo>());
        if (Session.UserInfo.IsAdministrator())
        {
          IEnumerable<TradeLoanUpdateJobInfo> source3 = source2.Where<TradeLoanUpdateJobInfo>((Func<TradeLoanUpdateJobInfo, bool>) (x => x.CreatedBy == Session.UserInfo.FullName));
          IEnumerable<TradeLoanUpdateJobInfo> source4 = source2.Where<TradeLoanUpdateJobInfo>((Func<TradeLoanUpdateJobInfo, bool>) (x => x.CreatedBy != Session.UserInfo.FullName));
          list.AddRange((IEnumerable<TradeLoanUpdateJobInfo>) source3.Where<TradeLoanUpdateJobInfo>((Func<TradeLoanUpdateJobInfo, bool>) (x => x.JobStatus == TradeLoanUpdateQueueStatus.Cancelled || x.JobStatus == TradeLoanUpdateQueueStatus.Completed || x.JobStatus == TradeLoanUpdateQueueStatus.Failed)).OrderBy<TradeLoanUpdateJobInfo, DateTime>((Func<TradeLoanUpdateJobInfo, DateTime>) (x => x.CreatedDate)).ToList<TradeLoanUpdateJobInfo>());
          list.AddRange((IEnumerable<TradeLoanUpdateJobInfo>) source4.Where<TradeLoanUpdateJobInfo>((Func<TradeLoanUpdateJobInfo, bool>) (x => x.JobStatus == TradeLoanUpdateQueueStatus.Cancelled || x.JobStatus == TradeLoanUpdateQueueStatus.Completed || x.JobStatus == TradeLoanUpdateQueueStatus.Failed)).OrderBy<TradeLoanUpdateJobInfo, DateTime>((Func<TradeLoanUpdateJobInfo, DateTime>) (x => x.CreatedDate)).ToList<TradeLoanUpdateJobInfo>());
        }
        else if (filter == TradeUpdateLoansDialog.filter.AdminUser || filter == TradeUpdateLoansDialog.filter.AllJobs)
          list.AddRange((IEnumerable<TradeLoanUpdateJobInfo>) source2.Where<TradeLoanUpdateJobInfo>((Func<TradeLoanUpdateJobInfo, bool>) (x => x.JobStatus == TradeLoanUpdateQueueStatus.Cancelled || x.JobStatus == TradeLoanUpdateQueueStatus.Completed || x.JobStatus == TradeLoanUpdateQueueStatus.Failed)).OrderBy<TradeLoanUpdateJobInfo, DateTime>((Func<TradeLoanUpdateJobInfo, DateTime>) (x => x.CreatedDate)).ToList<TradeLoanUpdateJobInfo>());
        foreach (TradeLoanUpdateJobInfo info in list)
        {
          GVItem gvItem = this.BuildOneGvItem(info);
          gvItemList.Add(gvItem);
        }
      }
      this.gvTrades.Items.Clear();
      this.gvTrades.Items.AddRange(gvItemList.ToArray());
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvTrades.Items)
      {
        if (this.GetErrorCounts(((TradeLoanUpdateQueueItem) gvItem.Tag).LoanUpdateJobInfo.Results) > 0)
          gvItem.SubItems["UPDATEPROGRESSSTATUS"].ForeColor = Color.Red;
        gvItem.SubItems.Add((object) this.BuildActionLabelLinks(((TradeLoanUpdateQueueItem) gvItem.Tag).LoanUpdateJobInfo.JobStatus, (TradeLoanUpdateQueueItem) gvItem.Tag));
      }
      return list;
    }

    private GVItem BuildOneGvItem(TradeLoanUpdateJobInfo info)
    {
      GVItem gvItem = new GVItem();
      gvItem.Tag = (object) new TradeLoanUpdateQueueItem()
      {
        LoanUpdateJobInfo = info
      };
      gvItem.SubItems.Add((object) info.TradeName);
      gvItem.SubItems.Add((object) EnumUtil.GetEnumDescription((Enum) info.TradeType));
      gvItem.SubItems.Add((object) info.CreatedDate);
      gvItem.SubItems.Add((object) info.CreatedBy);
      gvItem.SubItems.Add((object) EnumUtil.GetEnumDescription((Enum) info.JobStatus));
      this.loadItemResults(info, gvItem.SubItems);
      return gvItem;
    }

    private void loadItemResults(TradeLoanUpdateJobInfo info, GVSubItemCollection collection)
    {
      if (info.JobStatus == TradeLoanUpdateQueueStatus.InQueue)
        collection.Add((object) "Pending");
      else if (info.JobStatus == TradeLoanUpdateQueueStatus.InProgress)
      {
        Decimal num = Decimal.Divide((Decimal) info.LoansCompleted, (Decimal) info.TotalLoans) * 100M;
        collection.Add((object) new ProgressElement((Decimal) (num > 100M ? 100 : (int) num), true));
      }
      else
        collection.Add((object) info.Results);
    }

    private void CreateCurrentLoanJob(TradeLoanUpdateQueueItem queueItem)
    {
      if (queueItem.LoanUpdateJobInfo.TradeType == TradeType.MbsPool)
      {
        MbsPoolParallelLoanUpdateJob parallelLoanUpdateJob = new MbsPoolParallelLoanUpdateJob();
        List<MbsPoolLoanAssignmentItem> loanAssignmentItemList = new List<MbsPoolLoanAssignmentItem>();
        foreach (MbsPoolLoanAssignmentItem assignmentItem in queueItem.AssignmentItems)
          loanAssignmentItemList.Add(assignmentItem);
        parallelLoanUpdateJob.AssignmentItems = loanAssignmentItemList;
        this.currentLoanJob = (ITradeJob) parallelLoanUpdateJob;
      }
      else if (queueItem.LoanUpdateJobInfo.TradeType == TradeType.LoanTrade)
      {
        ParallelLoanUpdateJob parallelLoanUpdateJob = new ParallelLoanUpdateJob();
        List<AssignmentItem> assignmentItemList = new List<AssignmentItem>();
        foreach (AssignmentItem assignmentItem in queueItem.AssignmentItems)
          assignmentItemList.Add(assignmentItem);
        parallelLoanUpdateJob.AssignmentItems = assignmentItemList;
        this.currentLoanJob = (ITradeJob) parallelLoanUpdateJob;
      }
      else
      {
        if (queueItem.LoanUpdateJobInfo.TradeType != TradeType.CorrespondentTrade)
          return;
        CorrespondentTradeParallelLoanUpdateJob parallelLoanUpdateJob = new CorrespondentTradeParallelLoanUpdateJob();
        List<CorrespondentTradeAssignmentItem> tradeAssignmentItemList = new List<CorrespondentTradeAssignmentItem>();
        foreach (CorrespondentTradeAssignmentItem assignmentItem in queueItem.AssignmentItems)
          tradeAssignmentItemList.Add(assignmentItem);
        parallelLoanUpdateJob.AssignmentItems = tradeAssignmentItemList;
        this.currentLoanJob = (ITradeJob) parallelLoanUpdateJob;
      }
    }

    private GVItem GetGvItem(string jobGuid)
    {
      return this.gvTrades != null && this.gvTrades.Items.Count > 0 && this.gvTrades.Items.Any<GVItem>((Func<GVItem, bool>) (i => ((TradeLoanUpdateQueueItem) i.Tag).LoanUpdateJobInfo.JobGuid == jobGuid)) ? this.gvTrades.Items.Where<GVItem>((Func<GVItem, bool>) (i => ((TradeLoanUpdateQueueItem) i.Tag).LoanUpdateJobInfo.JobGuid == jobGuid)).First<GVItem>() : (GVItem) null;
    }

    private void UpdateGvItem(
      string jobGuid,
      TradeLoanUpdateQueueStatus status,
      bool highlight,
      string message)
    {
      GVItem gvItem = this.GetGvItem(jobGuid);
      if (gvItem == null)
        return;
      TradeLoanUpdateQueueItem tag = (TradeLoanUpdateQueueItem) gvItem.Tag;
      gvItem.SubItems["ACTION"].Value = (object) this.BuildActionLabelLinks(status, tag);
      gvItem.SubItems["STATUS"].Value = (object) status;
      gvItem.SubItems["UPDATEPROGRESSSTATUS"].Value = (object) message;
      if (!highlight)
        return;
      gvItem.SubItems["UPDATEPROGRESSSTATUS"].ForeColor = Color.Red;
    }

    private static void UnlockTrades(
      Dictionary<TradeType, List<TradeLoanUpdateJobInfo>> failedJobs)
    {
      if (failedJobs.ContainsKey(TradeType.LoanTrade) && failedJobs[TradeType.LoanTrade] != null && failedJobs[TradeType.LoanTrade].Count<TradeLoanUpdateJobInfo>() > 0)
        Session.LoanTradeManager.SetTradeStatus(failedJobs[TradeType.LoanTrade].Select<TradeLoanUpdateJobInfo, int>((Func<TradeLoanUpdateJobInfo, int>) (j => j.TradeID)).ToArray<int>(), TradeStatus.Open, TradeHistoryAction.UnlockPendingTrade, false);
      if (failedJobs.ContainsKey(TradeType.MbsPool) && failedJobs[TradeType.MbsPool] != null && failedJobs[TradeType.MbsPool].Count > 0)
        Session.MbsPoolManager.SetTradeStatus(failedJobs[TradeType.MbsPool].Select<TradeLoanUpdateJobInfo, int>((Func<TradeLoanUpdateJobInfo, int>) (j => j.TradeID)).ToArray<int>(), TradeStatus.Open, TradeHistoryAction.UnlockPendingTrade, false);
      if (!failedJobs.ContainsKey(TradeType.CorrespondentTrade) || failedJobs[TradeType.CorrespondentTrade] == null || failedJobs[TradeType.CorrespondentTrade].Count <= 0)
        return;
      Session.CorrespondentTradeManager.SetTradeStatus(failedJobs[TradeType.CorrespondentTrade].Select<TradeLoanUpdateJobInfo, int>((Func<TradeLoanUpdateJobInfo, int>) (j => j.TradeID)).ToArray<int>(), TradeStatus.Committed, TradeHistoryAction.UnlockPendingTrade, false);
    }

    private static void SetTradeUpdateStatus(
      TradeLoanUpdateJobInfo jobInfo,
      object info,
      int pending,
      int errorCount = 0)
    {
      switch (info)
      {
        case LoanTradeInfo _:
          if (pending == 1)
          {
            ((TradeInfoObj) info).Status = TradeStatus.Pending;
            ((TradeInfoObj) info).PendingBy = Session.UserInfo.FullName;
            Session.LoanTradeManager.UpdateTrade((LoanTradeInfo) info, true);
            break;
          }
          ((TradeInfoObj) info).Status = TradeStatus.Open;
          Session.LoanTradeManager.UpdateTrade((LoanTradeInfo) info, false);
          break;
        case MbsPoolInfo _:
          if (pending == 1)
          {
            ((TradeInfoObj) info).Status = TradeStatus.Pending;
            ((TradeInfoObj) info).PendingBy = Session.UserInfo.FullName;
            Session.MbsPoolManager.UpdateTrade((MbsPoolInfo) info, false, true);
            break;
          }
          ((TradeInfoObj) info).Status = TradeStatus.Open;
          Session.MbsPoolManager.UpdateTrade((MbsPoolInfo) info, false, false);
          break;
        case CorrespondentTradeInfo _:
          CorrespondentTradeInfo correspondentTrade = (CorrespondentTradeInfo) info;
          if (pending == 1)
          {
            correspondentTrade.Status = TradeStatus.Pending;
            correspondentTrade.PendingBy = Session.UserInfo.FullName;
            Session.CorrespondentTradeManager.UpdateTrade(correspondentTrade, true);
            break;
          }
          if (jobInfo.JobStatus == TradeLoanUpdateQueueStatus.Completed && errorCount == 0 && jobInfo.UpdateActionType == TradeLoanUpdateJobInfo.ActionType.Void)
            correspondentTrade.Status = TradeStatus.Voided;
          else
            correspondentTrade.status = correspondentTrade.CopyOfStatus;
          Session.CorrespondentTradeManager.UpdateTrade(correspondentTrade, false);
          break;
      }
    }

    private string BuildResultsMessage(
      int loansCompleted,
      int loansSkipped,
      int errorsCount,
      int totalCount)
    {
      int num = loansCompleted - loansSkipped;
      return "Processed: " + (object) num + ", Successful: " + (object) (num - errorsCount) + ", Errors: " + (object) errorsCount + ", Not Processed: " + (object) (totalCount - num);
    }

    private static void PublishWebhookEvent(
      TradeLoanUpdateQueueStatus status,
      TradeInfoObj info,
      List<TradeLoanUpdateError> errors,
      string message,
      TradeLoanUpdateJobInfo.ActionType actionType,
      List<IAssignmentItem> assignedItems)
    {
      if (info == null)
        return;
      if (status == TradeLoanUpdateQueueStatus.Completed && info.TradeType == TradeType.CorrespondentTrade)
      {
        WebhookEventContract eventContract = new WebhookEventContract();
        eventContract.UserId = Session.UserInfo.Userid;
        eventContract.InstanceId = Session.DefaultInstance.ServerIdentity != null ? Session.DefaultInstance.ServerIdentity.InstanceName : "LOCALHOST";
        eventContract.EventType = "loanAssignmentComplete";
        eventContract.ResourceId = info.TradeID.ToString();
        if (errors != null && errors.Count > 0)
        {
          foreach (TradeLoanUpdateError error in errors)
          {
            eventContract.AddExpoObject("loanId", (object) error.LoanGuid);
            eventContract.AddExpoObject("failedReason", (object) error.Message);
          }
        }
        try
        {
          new WebhookEventHelper(eventContract.InstanceId, Session.SessionObjects.SessionID, WebhookResource.trade).Publish(eventContract);
        }
        catch (Exception ex)
        {
          Tracing.Log(TradeUpdateLoansDialog.sw, TraceLevel.Error, nameof (TradeUpdateLoansDialog), "Loan Assignment Complete Webhook publish failed:" + ex.Message);
        }
      }
      if (actionType == TradeLoanUpdateJobInfo.ActionType.ExtendLock && status == TradeLoanUpdateQueueStatus.Completed && info.TradeType == TradeType.CorrespondentTrade)
      {
        WebhookEventContract eventContract = new WebhookEventContract();
        eventContract.UserId = Session.UserInfo.Userid;
        eventContract.InstanceId = Session.DefaultInstance.ServerIdentity != null ? Session.DefaultInstance.ServerIdentity.InstanceName : "LOCALHOST";
        eventContract.EventType = "update";
        eventContract.ResourceId = info.TradeID.ToString();
        List<UpdatedLoan> propertyValue1 = new List<UpdatedLoan>();
        List<UpdatedLoan> propertyValue2 = new List<UpdatedLoan>();
        if (errors != null && errors.Count > 0)
          propertyValue1 = errors.Select<TradeLoanUpdateError, UpdatedLoan>((Func<TradeLoanUpdateError, UpdatedLoan>) (e => new UpdatedLoan()
          {
            LoanId = e.LoanGuid,
            Reason = e.Message
          })).ToList<UpdatedLoan>();
        if (assignedItems != null && assignedItems.Count > 0)
          propertyValue2 = errors == null || errors.Count <= 0 ? assignedItems.Select<IAssignmentItem, UpdatedLoan>((Func<IAssignmentItem, UpdatedLoan>) (a => new UpdatedLoan()
          {
            LoanId = (a as CorrespondentTradeAssignmentItem).Assignment.Guid
          })).ToList<UpdatedLoan>() : assignedItems.Where<IAssignmentItem>((Func<IAssignmentItem, bool>) (a => errors == null || !errors.Select<TradeLoanUpdateError, string>((Func<TradeLoanUpdateError, string>) (e => e.LoanGuid)).Contains<string>((a as CorrespondentTradeAssignmentItem).Assignment.Guid))).Select<IAssignmentItem, UpdatedLoan>((Func<IAssignmentItem, UpdatedLoan>) (a => new UpdatedLoan()
          {
            LoanId = (a as CorrespondentTradeAssignmentItem).Assignment.Guid
          })).ToList<UpdatedLoan>();
        eventContract.AddExpoObject("tradeExtensionStatus", (object) "Completed");
        eventContract.AddExpoObject("failedUpdatedLoans", (object) propertyValue1);
        eventContract.AddExpoObject("updatedLoans", (object) propertyValue2);
        try
        {
          new WebhookEventHelper(eventContract.InstanceId, Session.SessionObjects.SessionID, WebhookResource.trade).Publish(eventContract);
        }
        catch (Exception ex)
        {
          Tracing.Log(TradeUpdateLoansDialog.sw, TraceLevel.Error, nameof (TradeUpdateLoansDialog), "Trade Update Webhook publish failed:" + ex.Message);
        }
      }
      if (actionType != TradeLoanUpdateJobInfo.ActionType.Void || status != TradeLoanUpdateQueueStatus.Completed || info.TradeType != TradeType.CorrespondentTrade)
        return;
      string a1 = "update";
      string propertyValue = "Voided";
      WebhookEventContract eventContract1 = new WebhookEventContract();
      eventContract1.UserId = Session.UserInfo.Userid;
      eventContract1.InstanceId = Session.DefaultInstance.ServerIdentity != null ? Session.DefaultInstance.ServerIdentity.InstanceName : "LOCALHOST";
      eventContract1.EventType = "update";
      eventContract1.ResourceId = info.TradeID.ToString();
      if (string.Equals(a1, "publish", StringComparison.InvariantCultureIgnoreCase))
      {
        CorrespondentTradeInfo trade = Session.CorrespondentTradeManager.GetTrade(info.TradeID.ToString());
        JsonSerializerSettings settings = new JsonSerializerSettings()
        {
          DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffZ"
        };
        eventContract1.AddExpoObject("lastPublishedDateTime", (object) JsonConvert.SerializeObject((object) trade.LastPublishedDateTime, settings));
      }
      else if (string.Equals(a1, "update", StringComparison.InvariantCultureIgnoreCase))
        eventContract1.AddExpoObject("statusUpdatedTo", (object) propertyValue);
      try
      {
        new WebhookEventHelper(eventContract1.InstanceId, Session.SessionObjects.SessionID, WebhookResource.trade).Publish(eventContract1);
      }
      catch (Exception ex)
      {
        Tracing.Log(TradeUpdateLoansDialog.sw, TraceLevel.Error, nameof (TradeUpdateLoansDialog), "Webhook publish failed for " + a1 + " event: " + ex.Message);
      }
    }

    private static void CreateNotesHistory(
      TradeLoanUpdateQueueStatus status,
      TradeInfoObj info,
      List<TradeLoanUpdateError> errors,
      string message)
    {
      if (info == null)
        return;
      TradeType tradeType = info.TradeType;
      TradeHistoryAction action = TradeHistoryAction.None;
      switch (status)
      {
        case TradeLoanUpdateQueueStatus.InQueue:
          action = TradeHistoryAction.LoanUpdatePending;
          break;
        case TradeLoanUpdateQueueStatus.Cancelled:
          action = TradeHistoryAction.LoanUpdateCancelled;
          break;
        case TradeLoanUpdateQueueStatus.Completed:
          action = TradeHistoryAction.LoanUpdateCompleted;
          break;
        case TradeLoanUpdateQueueStatus.Cleared:
          action = TradeHistoryAction.LoanUpdateCleared;
          break;
      }
      switch (tradeType)
      {
        case TradeType.LoanTrade:
          Session.LoanTradeManager.AddTradeHistoryItem(new LoanTradeHistoryItem((LoanTradeInfo) info, action, message, Session.UserInfo));
          break;
        case TradeType.MbsPool:
          Session.MbsPoolManager.AddTradeHistoryItem(new MbsPoolHistoryItem((MbsPoolInfo) info, action, message, Session.UserInfo));
          break;
        case TradeType.CorrespondentTrade:
          Session.CorrespondentTradeManager.AddTradeHistoryItem(new CorrespondentTradeHistoryItem((CorrespondentTradeInfo) info, action, message, Session.UserInfo));
          break;
      }
      if (errors == null || errors.Count <= 0)
        return;
      foreach (TradeLoanUpdateError error in errors)
      {
        switch (tradeType)
        {
          case TradeType.LoanTrade:
            Session.LoanTradeManager.AddTradeHistoryItem(new LoanTradeHistoryItem((LoanTradeInfo) info, error.LoanInfo, TradeHistoryAction.LoanUpdateErrors, error.Message, Session.UserInfo));
            continue;
          case TradeType.MbsPool:
            Session.MbsPoolManager.AddTradeHistoryItem(new MbsPoolHistoryItem((MbsPoolInfo) info, error.LoanInfo, TradeHistoryAction.LoanUpdateErrors, error.Message, Session.UserInfo));
            continue;
          case TradeType.CorrespondentTrade:
            Session.CorrespondentTradeManager.AddTradeHistoryItem(new CorrespondentTradeHistoryItem((CorrespondentTradeInfo) info, error.LoanInfo, TradeHistoryAction.LoanUpdateErrors, error.Message, Session.UserInfo));
            continue;
          default:
            continue;
        }
      }
    }

    public void CancelOneJob(string jobGuid)
    {
      if (this.currentLoanJob != null && this.currentLoanJob.JobGuid == jobGuid)
      {
        this.currentLoanJob.CancelAsync();
      }
      else
      {
        GVItem gvItem = this.GetGvItem(jobGuid);
        if (gvItem == null)
          return;
        TradeLoanUpdateQueueItem tag = (TradeLoanUpdateQueueItem) gvItem.Tag;
        lock (this.queue)
        {
          if (this.queue.Contains(tag))
            this.queue.Remove(tag);
        }
        int num = 0;
        if (tag != null && tag.AssignmentItems != null)
          num = tag.AssignmentItems.Count<IAssignmentItem>();
        string message1 = "Processed: 0, Successful: 0, Errors: 0, Not Processed: " + (object) num;
        string message2 = Session.UserInfo.FullName + " at " + (object) DateTime.Now + ": " + message1;
        TradeInfoObj info = tag.TradePoolInfo ?? this.BuildTradeInfoObj(jobGuid);
        if (info == null)
          return;
        this.UpdateDababaseToCancelled(jobGuid, message1);
        TradeUpdateLoansDialog.SetTradeUpdateStatus(tag.LoanUpdateJobInfo, (object) info, 0);
        TradeUpdateLoansDialog.CreateNotesHistory(TradeLoanUpdateQueueStatus.Cancelled, info, (List<TradeLoanUpdateError>) null, message2);
        this.UpdateGvItem(jobGuid, TradeLoanUpdateQueueStatus.Cancelled, false, message1);
      }
    }

    public bool IsNoJobRunning()
    {
      if (this.currentLoanJob == null)
        return true;
      TradeLoanUpdateJobInfo queue = Session.TradeLoanUpdateQueueManager.GetQueue(this.currentLoanJob.JobGuid);
      if (this.queue.Count != 0)
        return false;
      return this.currentLoanJob == null || queue == null || queue.JobStatus != TradeLoanUpdateQueueStatus.InProgress;
    }

    public void CancelAllJobs()
    {
      this._isCloseEncompass = true;
      LinkedListNode<TradeLoanUpdateQueueItem> next;
      for (LinkedListNode<TradeLoanUpdateQueueItem> linkedListNode = this.queue.QueueList.First; linkedListNode != null; linkedListNode = next)
      {
        next = linkedListNode.Next;
        this.CancelOneJob(linkedListNode.Value.LoanUpdateJobInfo.JobGuid);
      }
      if (this.currentLoanJob == null)
        return;
      this.CancelOneJob(this.currentLoanJob.JobGuid);
    }

    private void UpdateDababaseToCancelled(string jobGuid, string message)
    {
      Dictionary<string, object> updateValue = new Dictionary<string, object>();
      updateValue.Add("JobStatus", (object) 3);
      updateValue.Add("LastUpdatedDate", (object) DateTime.Now);
      updateValue.Add("CancelledBy", (object) Session.UserInfo.FullName);
      updateValue.Add("CancelledDate", (object) DateTime.Now);
      updateValue.Add("JobEndDate", (object) DateTime.Now);
      updateValue.Add("Results", (object) message);
      Session.TradeLoanUpdateQueueManager.UpdateQueue(updateValue, jobGuid);
      this.updateGvItemTag(updateValue, jobGuid);
    }

    private void updateGvItemTag(Dictionary<string, object> updateValue, string jobGuid)
    {
      TradeLoanUpdateQueueItem tag = (TradeLoanUpdateQueueItem) this.GetGvItem(jobGuid).Tag;
      foreach (KeyValuePair<string, object> keyValuePair in updateValue)
      {
        switch (keyValuePair.Key)
        {
          case "CancelledBy":
            tag.LoanUpdateJobInfo.CancelledBy = Session.UserInfo.FullName;
            continue;
          case "CancelledDate":
            tag.LoanUpdateJobInfo.LastUpdateDate = DateTime.Now;
            continue;
          case "JobEndDate":
            tag.LoanUpdateJobInfo.JobEndDate = DateTime.Now;
            continue;
          case "JobStartDate":
            tag.LoanUpdateJobInfo.JobStartDate = DateTime.Now;
            continue;
          case "JobStatus":
            tag.LoanUpdateJobInfo.JobStatus = (TradeLoanUpdateQueueStatus) keyValuePair.Value;
            continue;
          case "LastUpdatedDate":
            tag.LoanUpdateJobInfo.LastUpdateDate = DateTime.Now;
            continue;
          case "LoansCompleted":
            tag.LoanUpdateJobInfo.LoansCompleted = (int) keyValuePair.Value;
            continue;
          case "Results":
            tag.LoanUpdateJobInfo.Results = (string) keyValuePair.Value;
            continue;
          default:
            continue;
        }
      }
    }

    private TradeInfoObj BuildTradeInfoObj(string jobGuid)
    {
      GVItem gvItem = this.GetGvItem(jobGuid);
      if (gvItem == null)
        return (TradeInfoObj) null;
      TradeLoanUpdateQueueItem tag = (TradeLoanUpdateQueueItem) gvItem.Tag;
      return tag == null ? (TradeInfoObj) null : this.BuildTradeInfoObj(tag.LoanUpdateJobInfo.TradeType, tag.LoanUpdateJobInfo.TradeID);
    }

    private TradeInfoObj BuildTradeInfoObj(TradeType tradeType, int tradeId)
    {
      switch (tradeType)
      {
        case TradeType.LoanTrade:
          return (TradeInfoObj) Session.LoanTradeManager.GetTrade(tradeId);
        case TradeType.MbsPool:
          return (TradeInfoObj) Session.MbsPoolManager.GetTrade(tradeId);
        case TradeType.CorrespondentTrade:
          return (TradeInfoObj) Session.CorrespondentTradeManager.GetTrade(tradeId);
        default:
          return (TradeInfoObj) null;
      }
    }

    private int GetErrorCounts(string results)
    {
      if (string.IsNullOrEmpty(results) || !results.Contains("Errors: "))
        return 0;
      int startIndex = results.IndexOf("Errors: ") + "Errors: ".Length;
      int num = results.LastIndexOf(",");
      return int.Parse(results.Substring(startIndex, num - startIndex));
    }

    public int LoanCount => this.loanCount;

    public bool CancelRequested => this.cancelRequested;

    public List<TradeLoanUpdateError> Errors => this.errors;

    public int TotalCompletedCount => this.sequentialCompletedCount + this.parallelCompletedCount;

    public int CompletedSuccessfullyCount => this.completedSuccessfullyCount;

    public int CompletedWithErrorsCount => this.completedWithErrorsCount;

    private void radioBtnCheckedChanged(object sender, EventArgs e)
    {
      if (!((RadioButton) sender).Checked)
        return;
      this.applyFilter();
    }

    private void applyFilter()
    {
      if (this.rbtnAllJobs.Checked)
        this.RefreshGridviewByDefaultOrder(TradeUpdateLoansDialog.filter.AllJobs);
      else if (this.rbtnAdminJob.Checked)
      {
        this.RefreshGridviewByDefaultOrder(TradeUpdateLoansDialog.filter.AdminUser);
      }
      else
      {
        if (!this.rbtnSessionJob.Checked)
          return;
        this.RefreshGridviewByDefaultOrder(TradeUpdateLoansDialog.filter.CurrentSession);
      }
    }

    private void btnClear_Click(object sender, EventArgs e)
    {
      if (!Utils.IsDate((object) this.dtClear.Value) || !(this.dtClear.Value != DateTime.MinValue))
        return;
      int num1 = Session.TradeLoanUpdateQueueManager.DeleteQueues(this.dtClear.Value);
      string str = num1 > 1 ? "entries have" : "entry has";
      int num2 = (int) Utils.Dialog((IWin32Window) this, num1.ToString() + " Trade Update Queue " + str + " been cleared.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      this.applyFilter();
      this.dtClear.Value = DateTime.MinValue;
      this.dtClear_ValueChanged((object) null, (EventArgs) null);
    }

    private void dtClear_ValueChanged(object sender, EventArgs e)
    {
      if (Utils.IsDate((object) this.dtClear.Value) && this.dtClear.Value != DateTime.MinValue && this.dtClear.Value < DateTime.Today)
        this.btnClear.Enabled = true;
      else
        this.btnClear.Enabled = false;
    }

    private void PublishKafkaEvent(
      TradeLoanUpdateQueueStatus status,
      TradeInfoObj info,
      List<TradeLoanUpdateError> errors,
      string message,
      TradeLoanUpdateJobInfo.ActionType actionType,
      List<IAssignmentItem> assignedItems)
    {
      int serverMode = (int) EncompassServer.ServerMode;
      WebhookEventConsts webhookEventConsts = new WebhookEventConsts(WebhookResource.trade);
      string clientId = Session.CompanyInfo.ClientID;
      if (info == null)
        return;
      Hashtable eventPayload = new Hashtable();
      if (status == TradeLoanUpdateQueueStatus.Completed && info.TradeType == TradeType.CorrespondentTrade)
      {
        if (errors != null && errors.Count > 0)
        {
          foreach (TradeLoanUpdateError error in errors)
          {
            if (!eventPayload.ContainsKey((object) error.LoanGuid))
              eventPayload.Add((object) error.LoanGuid, (object) error.Message);
          }
        }
        try
        {
          Session.CorrespondentTradeManager.PublishKafkaEvent("loanAssignmentComplete", info.TradeID, eventPayload);
        }
        catch (Exception ex)
        {
          Tracing.Log(TradeUpdateLoansDialog.sw, TraceLevel.Error, nameof (TradeUpdateLoansDialog), "Loan Assignment Complete Webhook publish failed:" + ex.Message);
        }
      }
      if (actionType == TradeLoanUpdateJobInfo.ActionType.ExtendLock && status == TradeLoanUpdateQueueStatus.Completed && info.TradeType == TradeType.CorrespondentTrade)
      {
        List<UpdatedLoan> updatedLoanList1 = new List<UpdatedLoan>();
        List<UpdatedLoan> updatedLoanList2 = new List<UpdatedLoan>();
        if (errors != null && errors.Count > 0)
          updatedLoanList1 = errors.Select<TradeLoanUpdateError, UpdatedLoan>((Func<TradeLoanUpdateError, UpdatedLoan>) (e => new UpdatedLoan()
          {
            LoanId = e.LoanGuid,
            Reason = e.Message
          })).ToList<UpdatedLoan>();
        if (assignedItems != null && assignedItems.Count > 0)
          updatedLoanList2 = errors == null || errors.Count <= 0 ? assignedItems.Select<IAssignmentItem, UpdatedLoan>((Func<IAssignmentItem, UpdatedLoan>) (a => new UpdatedLoan()
          {
            LoanId = (a as CorrespondentTradeAssignmentItem).Assignment.Guid
          })).ToList<UpdatedLoan>() : assignedItems.Where<IAssignmentItem>((Func<IAssignmentItem, bool>) (a => errors == null || !errors.Select<TradeLoanUpdateError, string>((Func<TradeLoanUpdateError, string>) (e => e.LoanGuid)).Contains<string>((a as CorrespondentTradeAssignmentItem).Assignment.Guid))).Select<IAssignmentItem, UpdatedLoan>((Func<IAssignmentItem, UpdatedLoan>) (a => new UpdatedLoan()
          {
            LoanId = (a as CorrespondentTradeAssignmentItem).Assignment.Guid
          })).ToList<UpdatedLoan>();
        eventPayload.Add((object) "tradeExtensionStatus", (object) "Completed");
        eventPayload.Add((object) "failedUpdatedLoans", (object) updatedLoanList1);
        eventPayload.Add((object) "updatedLoans", (object) updatedLoanList2);
        try
        {
          Session.CorrespondentTradeManager.PublishKafkaEvent("update", info.TradeID, eventPayload);
        }
        catch (Exception ex)
        {
          Tracing.Log(TradeUpdateLoansDialog.sw, TraceLevel.Error, nameof (TradeUpdateLoansDialog), "Trade Update Webhook publish failed:" + ex.Message);
        }
      }
      if (actionType != TradeLoanUpdateJobInfo.ActionType.Void || status != TradeLoanUpdateQueueStatus.Completed || info.TradeType != TradeType.CorrespondentTrade)
        return;
      eventPayload.Add((object) "statusUpdatedTo", (object) "Voided");
      try
      {
        Session.CorrespondentTradeManager.PublishKafkaEvent("update", info.TradeID, eventPayload);
      }
      catch (Exception ex)
      {
        Tracing.Log(TradeUpdateLoansDialog.sw, TraceLevel.Error, nameof (TradeUpdateLoansDialog), "Webhook publish failed for Void Trade: " + ex.Message);
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (TradeUpdateLoansDialog));
      this.btnClose = new Button();
      this.updateWorker = new BackgroundWorker();
      this.btnRefresh = new Button();
      this.rbtnAllJobs = new RadioButton();
      this.rbtnSessionJob = new RadioButton();
      this.rbtnAdminJob = new RadioButton();
      this.gvTrades = new GridView();
      this.label1 = new Label();
      this.dtClear = new DatePicker();
      this.btnClear = new Button();
      this.SuspendLayout();
      this.btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnClose.DialogResult = DialogResult.Cancel;
      this.btnClose.Location = new Point(1025, 282);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new Size(75, 23);
      this.btnClose.TabIndex = 2;
      this.btnClose.Text = "Close";
      this.btnClose.UseVisualStyleBackColor = true;
      this.btnClose.Click += new EventHandler(this.btnClose_Click);
      this.updateWorker.WorkerReportsProgress = true;
      this.updateWorker.WorkerSupportsCancellation = true;
      this.updateWorker.DoWork += new DoWorkEventHandler(this.updateWorker_DoWork);
      this.updateWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.updateWorker_RunWorkerCompleted);
      this.btnRefresh.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnRefresh.Location = new Point(932, 282);
      this.btnRefresh.Name = "btnRefresh";
      this.btnRefresh.Size = new Size(75, 23);
      this.btnRefresh.TabIndex = 3;
      this.btnRefresh.Text = "Refresh";
      this.btnRefresh.UseVisualStyleBackColor = true;
      this.btnRefresh.Click += new EventHandler(this.btnRefresh_Click);
      this.rbtnAllJobs.AutoSize = true;
      this.rbtnAllJobs.Location = new Point(9, 12);
      this.rbtnAllJobs.Name = "rbtnAllJobs";
      this.rbtnAllJobs.Size = new Size(61, 17);
      this.rbtnAllJobs.TabIndex = 4;
      this.rbtnAllJobs.Text = "All Jobs";
      this.rbtnAllJobs.UseVisualStyleBackColor = true;
      this.rbtnAllJobs.CheckedChanged += new EventHandler(this.radioBtnCheckedChanged);
      this.rbtnSessionJob.AutoSize = true;
      this.rbtnSessionJob.Location = new Point(101, 13);
      this.rbtnSessionJob.Name = "rbtnSessionJob";
      this.rbtnSessionJob.Size = new Size(269, 17);
      this.rbtnSessionJob.TabIndex = 5;
      this.rbtnSessionJob.Text = "Show jobs submitted during this Encompass session";
      this.rbtnSessionJob.UseVisualStyleBackColor = true;
      this.rbtnSessionJob.CheckedChanged += new EventHandler(this.radioBtnCheckedChanged);
      this.rbtnAdminJob.AutoSize = true;
      this.rbtnAdminJob.Location = new Point(401, 13);
      this.rbtnAdminJob.Name = "rbtnAdminJob";
      this.rbtnAdminJob.Size = new Size(139, 17);
      this.rbtnAdminJob.TabIndex = 6;
      this.rbtnAdminJob.Text = "Show jobs submitted by ";
      this.rbtnAdminJob.UseVisualStyleBackColor = true;
      this.rbtnAdminJob.CheckedChanged += new EventHandler(this.radioBtnCheckedChanged);
      this.gvTrades.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gvTrades.AutoHeight = true;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "TRADEID";
      gvColumn1.Text = "Trade/Pool ID";
      gvColumn1.Width = 150;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "TRADETYPE";
      gvColumn2.Text = "Trade Type";
      gvColumn2.Width = 100;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "REQUESTDATE";
      gvColumn3.Text = "Request Date";
      gvColumn3.Width = 160;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "REQEUSTEDBY";
      gvColumn4.Text = "Requested By";
      gvColumn4.Width = 120;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "STATUS";
      gvColumn5.Text = "Status";
      gvColumn5.Width = 100;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "UPDATEPROGRESSSTATUS";
      gvColumn6.Text = "Update Progress/Results";
      gvColumn6.Width = 330;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "ACTION";
      gvColumn7.Text = "Action";
      gvColumn7.Width = 130;
      this.gvTrades.Columns.AddRange(new GVColumn[7]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6,
        gvColumn7
      });
      this.gvTrades.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvTrades.ItemWordWrap = true;
      this.gvTrades.Location = new Point(8, 36);
      this.gvTrades.Name = "gvTrades";
      this.gvTrades.Size = new Size(1101, 235);
      this.gvTrades.TabIndex = 1;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(813, 17);
      this.label1.Name = "label1";
      this.label1.Size = new Size(96, 13);
      this.label1.TabIndex = 8;
      this.label1.Text = "Batch Clear Before";
      this.dtClear.BackColor = SystemColors.Window;
      this.dtClear.Location = new Point(915, 12);
      this.dtClear.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.dtClear.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dtClear.Name = "dtClear";
      this.dtClear.Size = new Size(104, 21);
      this.dtClear.TabIndex = 201;
      this.dtClear.ToolTip = "";
      this.dtClear.Value = new DateTime(0L);
      this.dtClear.ValueChanged += new EventHandler(this.dtClear_ValueChanged);
      this.btnClear.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnClear.Location = new Point(1025, 10);
      this.btnClear.Name = "btnClear";
      this.btnClear.Size = new Size(75, 23);
      this.btnClear.TabIndex = 202;
      this.btnClear.Text = "Clear";
      this.btnClear.UseVisualStyleBackColor = true;
      this.btnClear.Click += new EventHandler(this.btnClear_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnClose;
      this.ClientSize = new Size(1117, 312);
      this.Controls.Add((Control) this.btnClear);
      this.Controls.Add((Control) this.dtClear);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.rbtnAdminJob);
      this.Controls.Add((Control) this.rbtnSessionJob);
      this.Controls.Add((Control) this.rbtnAllJobs);
      this.Controls.Add((Control) this.gvTrades);
      this.Controls.Add((Control) this.btnRefresh);
      this.Controls.Add((Control) this.btnClose);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (TradeUpdateLoansDialog);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Trade Update Queue";
      this.FormClosing += new FormClosingEventHandler(this.TradeUpdateLoansDialog_FormClosing);
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private delegate void AddConversionQueueItem(
      TradeLoanUpdateQueueItem queueItem,
      bool processItem);

    private enum filter
    {
      AllJobs,
      CurrentSession,
      AdminUser,
    }
  }
}
