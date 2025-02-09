// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.Services.EOSClientDialog
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer.eFolder;
using EllieMae.EMLite.ClientServer.SkyDrive;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.eFolder.Files;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.LoanUtils.Services
{
  public class EOSClientDialog : Form
  {
    private const string className = "EOSClientDialog�";
    private static readonly string sw = Tracing.SwEFolder;
    private LoanDataMgr loanDataMgr;
    private EOSClient eosClient;
    private string jobId;
    private CloudAttachment[] attachmentList;
    private CloudAttachment attachment;
    private DocumentLog doc;
    private SplitFile[] splitFileList;
    private string[] attachmentIdList;
    private GetClassificationJobResponse classificationResponse;
    private IContainer components;
    private BackgroundWorker mergeWorker;
    private BackgroundWorker splitWorker;
    private Button btnCancel;
    private Label lblOverall;
    private ProgressBar pbOverall;
    private BackgroundWorker classificationWorker;
    private BackgroundWorker ngSplitWorker;

    public bool IsClassificationFailed { get; set; }

    public EOSClientDialog(LoanDataMgr loanDataMgr)
    {
      this.InitializeComponent();
      this.loanDataMgr = loanDataMgr;
      this.eosClient = new EOSClient(this.loanDataMgr);
    }

    public CloudAttachment MergeAttachments(
      string jobId,
      CloudAttachment[] attachmentList,
      DocumentLog doc)
    {
      this.Text = "Merging Documents";
      this.jobId = jobId;
      this.attachmentList = attachmentList;
      this.doc = doc;
      Tracing.Log(EOSClientDialog.sw, TraceLevel.Verbose, nameof (EOSClientDialog), "CompleteMerge: " + (object) attachmentList.Length);
      this.attachment = (CloudAttachment) null;
      Tracing.Log(EOSClientDialog.sw, TraceLevel.Verbose, nameof (EOSClientDialog), "Starting Worker");
      this.mergeWorker.RunWorkerAsync();
      Tracing.Log(EOSClientDialog.sw, TraceLevel.Verbose, nameof (EOSClientDialog), "Showing Dialog");
      return this.ShowDialog((IWin32Window) Form.ActiveForm) != DialogResult.OK ? (CloudAttachment) null : this.attachment;
    }

    private void mergeWorker_DoWork(object sender, DoWorkEventArgs e)
    {
      Tracing.Log(EOSClientDialog.sw, TraceLevel.Verbose, nameof (EOSClientDialog), "Merge Worker Started");
      if (this.mergeWorker.CancellationPending)
        return;
      if (string.IsNullOrEmpty(this.jobId))
        this.jobId = this.createMergeJob(this.attachmentList);
      if (!this.waitMergeJobComplete(this.jobId))
        return;
      this.attachment = this.commitMergeJob(this.jobId, this.doc);
    }

    private string createMergeJob(CloudAttachment[] attachmentList)
    {
      Tracing.Log(EOSClientDialog.sw, TraceLevel.Verbose, nameof (EOSClientDialog), "Calling CreateMergeJob");
      Task<CreateMergeJobResponse> mergeJob = this.eosClient.CreateMergeJob((FileAttachment[]) attachmentList);
      Tracing.Log(EOSClientDialog.sw, TraceLevel.Verbose, nameof (EOSClientDialog), "Waiting for CreateMergeJob Task");
      Task.WaitAll((Task) mergeJob);
      return mergeJob.Result.jobId;
    }

    private bool waitMergeJobComplete(string jobId)
    {
      int num = 25;
      Task<SkyDriveUrl> task;
      for (SkyDriveUrl skyDriveUrl = (SkyDriveUrl) null; skyDriveUrl == null; skyDriveUrl = task.Result)
      {
        if (num <= 75)
          this.mergeWorker.ReportProgress(num++);
        Thread.Sleep(1000);
        if (this.mergeWorker.CancellationPending)
          return false;
        Tracing.Log(EOSClientDialog.sw, TraceLevel.Verbose, nameof (EOSClientDialog), "Calling CheckMergeJobStatus");
        task = this.eosClient.CheckMergeJobStatus(jobId);
        Tracing.Log(EOSClientDialog.sw, TraceLevel.Verbose, nameof (EOSClientDialog), "Waiting for CheckMergeJobStatus Task");
        Task.WaitAll((Task) task);
      }
      this.mergeWorker.ReportProgress(75);
      return true;
    }

    private CloudAttachment commitMergeJob(string jobId, DocumentLog doc)
    {
      if (this.mergeWorker.CancellationPending)
        return (CloudAttachment) null;
      this.Invoke((Delegate) (() => this.btnCancel.Enabled = false));
      Tracing.Log(EOSClientDialog.sw, TraceLevel.Verbose, nameof (EOSClientDialog), "Calling CommitMergeJob");
      Task<string> task = this.eosClient.CommitMergeJob(jobId);
      Tracing.Log(EOSClientDialog.sw, TraceLevel.Verbose, nameof (EOSClientDialog), "Waiting for CommitMergeJob Task");
      Task.WaitAll((Task) task);
      Tracing.Log(EOSClientDialog.sw, TraceLevel.Verbose, nameof (EOSClientDialog), "Calling Resync: " + task.Result);
      FileAttachment[] fileAttachmentArray = this.loanDataMgr.FileAttachments.Resync(new string[1]
      {
        task.Result
      });
      CloudAttachment cloudAttachment = (CloudAttachment) null;
      if (fileAttachmentArray != null)
        cloudAttachment = fileAttachmentArray[0] as CloudAttachment;
      if (cloudAttachment == null)
      {
        PerformanceMeter.Current.AddCheckpoint("EXIT THROW", 201, nameof (commitMergeJob), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\Services\\EOSClientDialog.cs");
        throw new Exception("Merged Attachment was not created: " + task.Result);
      }
      if (doc != null)
      {
        int index = doc.Files.IndexOf(this.attachmentList[0].ID);
        if (index < 0)
          index = doc.Files.Count;
        doc.Files.Insert(index, cloudAttachment.ID, this.loanDataMgr.SessionObjects.UserID, true);
      }
      foreach (FileAttachment attachment in this.attachmentList)
        this.loanDataMgr.FileAttachments.Remove(RemoveReasonType.Merge, attachment.ID);
      return cloudAttachment;
    }

    private void mergeWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
      this.pbOverall.Value = e.ProgressPercentage;
    }

    private void mergeWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      PerformanceMeter.Current.AddCheckpoint("END", 240, nameof (mergeWorker_RunWorkerCompleted), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\Services\\EOSClientDialog.cs");
      if (e.Error != null)
      {
        Tracing.Log(EOSClientDialog.sw, TraceLevel.Error, nameof (EOSClientDialog), e.Error.ToString());
        string str = string.Empty;
        if (e.Error.InnerException != null && e.Error.InnerException.Message.LastIndexOf("Correlation") >= 0)
          str = e.Error.InnerException.Message.Substring(e.Error.InnerException.Message.LastIndexOf("Correlation"));
        int num = (int) Utils.Dialog((IWin32Window) this, "Something went wrong trying to merge the document. Please try again. If the error persists, please contact ICE Mortgage Technology support.\n\n" + str, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      Tracing.Log(EOSClientDialog.sw, TraceLevel.Verbose, nameof (EOSClientDialog), "Closing Window");
      if (this.attachment == null)
        this.DialogResult = DialogResult.Cancel;
      else
        this.DialogResult = DialogResult.OK;
    }

    public CloudAttachment[] SplitAttachment(CloudAttachment attachment, SplitFile[] splitFileList)
    {
      this.Text = "Splitting Document";
      this.attachment = attachment;
      this.splitFileList = splitFileList;
      Tracing.Log(EOSClientDialog.sw, TraceLevel.Verbose, nameof (EOSClientDialog), "SplitAttachment: " + attachment.ID + " Parts: " + (object) splitFileList.Length);
      this.attachmentList = (CloudAttachment[]) null;
      Tracing.Log(EOSClientDialog.sw, TraceLevel.Verbose, nameof (EOSClientDialog), "Starting Worker");
      this.splitWorker.RunWorkerAsync();
      Tracing.Log(EOSClientDialog.sw, TraceLevel.Verbose, nameof (EOSClientDialog), "Showing Dialog");
      return this.ShowDialog((IWin32Window) Form.ActiveForm) != DialogResult.OK ? (CloudAttachment[]) null : this.attachmentList;
    }

    private void splitWorker_DoWork(object sender, DoWorkEventArgs e)
    {
      Tracing.Log(EOSClientDialog.sw, TraceLevel.Verbose, nameof (EOSClientDialog), "Split Worker Started");
      if (this.mergeWorker.CancellationPending)
        return;
      if (string.IsNullOrEmpty(this.jobId))
        this.jobId = this.createSplitJob(this.attachment, this.splitFileList);
      if (!this.waitSplitJobComplete(this.jobId))
        return;
      this.attachmentList = this.commitSplitJob(this.jobId, this.attachment, this.splitFileList);
    }

    private string createSplitJob(CloudAttachment attachment, SplitFile[] splitFileList)
    {
      SplitFile[] files = new SplitFile[splitFileList.Length];
      for (int index = 0; index < splitFileList.Length; ++index)
      {
        List<int> intList = new List<int>();
        foreach (int page in splitFileList[index].pages)
          intList.Add(page + 1);
        files[index] = new SplitFile();
        files[index].pages = intList.ToArray();
      }
      Tracing.Log(EOSClientDialog.sw, TraceLevel.Verbose, nameof (EOSClientDialog), "Calling CreateSplitJob");
      Task<string> splitJob = this.eosClient.CreateSplitJob(attachment.ID, files);
      Tracing.Log(EOSClientDialog.sw, TraceLevel.Verbose, nameof (EOSClientDialog), "Waiting for CreateSplitJob Task");
      Task.WaitAll((Task) splitJob);
      return splitJob.Result;
    }

    private bool waitSplitJobComplete(string jobId)
    {
      int num = 25;
      Task<SplitJobStatusResponse> task;
      for (SplitJobStatusResponse jobStatusResponse = (SplitJobStatusResponse) null; jobStatusResponse == null; jobStatusResponse = task.Result)
      {
        if (num <= 75)
          this.mergeWorker.ReportProgress(num++);
        Thread.Sleep(1000);
        if (this.mergeWorker.CancellationPending)
          return false;
        Tracing.Log(EOSClientDialog.sw, TraceLevel.Verbose, nameof (EOSClientDialog), "Calling CheckSplitJobStatus");
        task = this.eosClient.CheckSplitJobStatus(jobId);
        Tracing.Log(EOSClientDialog.sw, TraceLevel.Verbose, nameof (EOSClientDialog), "Waiting for CheckMergeJobStatus Task");
        Task.WaitAll((Task) task);
      }
      this.mergeWorker.ReportProgress(75);
      return true;
    }

    private CloudAttachment[] commitSplitJob(
      string jobId,
      CloudAttachment attachment,
      SplitFile[] splitFileList)
    {
      if (this.mergeWorker.CancellationPending)
        return (CloudAttachment[]) null;
      this.Invoke((Delegate) (() => this.btnCancel.Enabled = false));
      CommitAttachmentRequest[] commitList = new CommitAttachmentRequest[splitFileList.Length];
      for (int index = 0; index < splitFileList.Length; ++index)
      {
        commitList[index] = new CommitAttachmentRequest();
        commitList[index].title = attachment.Title + " (" + Convert.ToString(index + 1) + ")";
      }
      Tracing.Log(EOSClientDialog.sw, TraceLevel.Verbose, nameof (EOSClientDialog), "Calling CommitSplitJob");
      Task<CommitSplitJobResponse> task = this.eosClient.CommitSplitJob(jobId, commitList);
      Tracing.Log(EOSClientDialog.sw, TraceLevel.Verbose, nameof (EOSClientDialog), "Waiting for CommitSplitJob Task");
      Task.WaitAll((Task) task);
      List<string> stringList = new List<string>();
      foreach (CommitAttachmentResponse attachment1 in task.Result.attachments)
      {
        if (string.Equals(attachment1.status, "Success", StringComparison.InvariantCultureIgnoreCase))
          stringList.Add(attachment1.attachmentId);
      }
      List<CloudAttachment> cloudAttachmentList = new List<CloudAttachment>();
      if (stringList.Count > 0)
      {
        Tracing.Log(EOSClientDialog.sw, TraceLevel.Verbose, nameof (EOSClientDialog), "Calling Resync: " + (object) stringList.Count);
        FileAttachment[] source = this.loanDataMgr.FileAttachments.Resync(stringList.ToArray());
        if (source != null)
        {
          for (int index = 0; index < task.Result.attachments.Length; ++index)
          {
            string attachmentId = task.Result.attachments[index].attachmentId;
            if (!string.IsNullOrEmpty(attachmentId) && ((IEnumerable<FileAttachment>) source).FirstOrDefault<FileAttachment>((Func<FileAttachment, bool>) (x => x.ID == attachmentId)) is CloudAttachment cloudAttachment)
            {
              bool flag = true;
              for (int pageIndex = 0; pageIndex < splitFileList[index].pages.Length; ++pageIndex)
              {
                DocumentIdentity documentIdentity = attachment.Identities.Get(splitFileList[index].pages[pageIndex]);
                if (documentIdentity != null)
                  cloudAttachment.Identities.Add(new DocumentIdentity(pageIndex, documentIdentity.DocumentType, documentIdentity.DocumentSource));
                else
                  flag = false;
              }
              cloudAttachment.Identities.Complete = flag;
              cloudAttachmentList.Add(cloudAttachment);
            }
          }
        }
      }
      if (cloudAttachmentList.Count == splitFileList.Length)
        this.loanDataMgr.FileAttachments.Remove(RemoveReasonType.Split, attachment.ID);
      return cloudAttachmentList.ToArray();
    }

    private void splitWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
      this.pbOverall.Value = e.ProgressPercentage;
    }

    private void splitWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      PerformanceMeter.Current.AddCheckpoint("END", 508, nameof (splitWorker_RunWorkerCompleted), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\Services\\EOSClientDialog.cs");
      if (e.Error != null)
      {
        Tracing.Log(EOSClientDialog.sw, TraceLevel.Error, nameof (EOSClientDialog), e.Error.ToString());
        string str = string.Empty;
        if (e.Error.InnerException != null && e.Error.InnerException.Message.LastIndexOf("Correlation") >= 0)
          str = e.Error.InnerException.Message.Substring(e.Error.InnerException.Message.LastIndexOf("Correlation"));
        int num = (int) Utils.Dialog((IWin32Window) this, "Something went wrong trying to split the document. Please try again. If the error persists, please contact ICE Mortgage Technology support.\n\n" + str, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      if (this.attachmentList != null)
      {
        if (this.attachmentList.Length != this.splitFileList.Length)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "A problem occurred when splitting the " + this.attachment.Title + " document. Only " + (object) this.attachmentList.Length + " of the " + (object) this.splitFileList.Length + " splits were successful. The original file will not be removed from the loan so that you can try splitting it again.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        this.DialogResult = DialogResult.OK;
      }
      else
        this.DialogResult = DialogResult.Cancel;
    }

    public GetClassificationJobResponse IdentifyFiles(string[] attachmentIdLst)
    {
      this.Text = "Identifying Files";
      this.attachmentIdList = attachmentIdLst;
      Tracing.Log(EOSClientDialog.sw, TraceLevel.Verbose, nameof (EOSClientDialog), "classifyingAttachment having files: " + (object) this.attachmentIdList.Length);
      Tracing.Log(EOSClientDialog.sw, TraceLevel.Verbose, nameof (EOSClientDialog), "Starting Worker");
      this.classificationWorker.RunWorkerAsync();
      Tracing.Log(EOSClientDialog.sw, TraceLevel.Verbose, nameof (EOSClientDialog), "Showing Dialog");
      return this.ShowDialog((IWin32Window) Form.ActiveForm) != DialogResult.OK ? (GetClassificationJobResponse) null : this.classificationResponse;
    }

    private void classificationWorker_DoWork(object sender, DoWorkEventArgs e)
    {
      Tracing.Log(EOSClientDialog.sw, TraceLevel.Verbose, nameof (EOSClientDialog), "Classification Worker Started");
      if (this.classificationWorker.CancellationPending)
        return;
      if (string.IsNullOrEmpty(this.jobId))
        this.jobId = this.createClassificationJob();
      if (string.IsNullOrEmpty(this.jobId))
        return;
      this.waitClassificationJobComplete();
    }

    private string createClassificationJob()
    {
      Tracing.Log(EOSClientDialog.sw, TraceLevel.Verbose, nameof (EOSClientDialog), "CreateClassificationJob :: Calling CreateClassificationJob");
      Task<string> classificationJob = this.eosClient.CreateDocClassificationJob(this.attachmentIdList);
      Tracing.Log(EOSClientDialog.sw, TraceLevel.Verbose, nameof (EOSClientDialog), "CreateClassificationJob :: Waiting for CreateclassificationJob Task");
      Task.WaitAll((Task) classificationJob);
      return classificationJob.Result;
    }

    private void waitClassificationJobComplete()
    {
      GetClassificationJobResponse classificationJobResponse = new GetClassificationJobResponse();
      int num = 25;
      Task<GetClassificationJobResponse> task;
      for (; classificationJobResponse.progress < 60; classificationJobResponse = task.Result)
      {
        if (this.classificationWorker.CancellationPending)
        {
          this.CancelClassificationJob();
          return;
        }
        if (num <= 75)
          this.ngSplitWorker.ReportProgress(num++);
        Thread.Sleep(1000);
        Tracing.Log(EOSClientDialog.sw, TraceLevel.Verbose, nameof (EOSClientDialog), "waitClassificationJobComplete :: Calling CheckClassificationJobStatus");
        task = this.eosClient.CheckDocClassificationJobStatus(this.jobId);
        Tracing.Log(EOSClientDialog.sw, TraceLevel.Verbose, nameof (EOSClientDialog), "waitClassificationJobComplete :: Waiting for CheckClassificationJobStatus Task");
        Task.WaitAll((Task) task);
      }
      this.classificationResponse = classificationJobResponse;
      this.classificationWorker.ReportProgress(100);
    }

    private void classificationWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
      this.pbOverall.Value = e.ProgressPercentage;
    }

    private void classificationWorker_RunWorkerCompleted(
      object sender,
      RunWorkerCompletedEventArgs e)
    {
      PerformanceMeter.Current.AddCheckpoint("END", 682, nameof (classificationWorker_RunWorkerCompleted), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\Services\\EOSClientDialog.cs");
      if (e.Error != null)
      {
        this.IsClassificationFailed = this.eosClient.IsClassificationFailed;
        Tracing.Log(EOSClientDialog.sw, TraceLevel.Error, nameof (EOSClientDialog), e.Error.ToString());
        string str = string.Empty;
        if (e.Error.InnerException != null && e.Error.InnerException.Message.LastIndexOf("Correlation") >= 0)
          str = e.Error.InnerException.Message.Substring(e.Error.InnerException.Message.LastIndexOf("Correlation"));
        int num = (int) Utils.Dialog((IWin32Window) this, "Something went wrong trying to auto assign the document. Please try again. If the error persists, please contact ICE Mortgage Technology support.\n\n" + str, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      Tracing.Log(EOSClientDialog.sw, TraceLevel.Verbose, nameof (EOSClientDialog), "Closing Window");
      if (this.classificationResponse == null)
        this.DialogResult = DialogResult.Cancel;
      else
        this.DialogResult = DialogResult.OK;
    }

    public GetClassificationJobResponse SplitAndAssignAttachment(string classificationJobId)
    {
      this.Text = "Splitting Document";
      this.btnCancel.Visible = false;
      this.jobId = classificationJobId;
      Tracing.Log(EOSClientDialog.sw, TraceLevel.Verbose, nameof (EOSClientDialog), "Starting Worker");
      this.ngSplitWorker.RunWorkerAsync();
      Tracing.Log(EOSClientDialog.sw, TraceLevel.Verbose, nameof (EOSClientDialog), "Showing Dialog");
      return this.ShowDialog((IWin32Window) Form.ActiveForm) != DialogResult.OK ? (GetClassificationJobResponse) null : this.classificationResponse;
    }

    private void ngSplitWorker_DoWork(object sender, DoWorkEventArgs e)
    {
      Tracing.Log(EOSClientDialog.sw, TraceLevel.Verbose, nameof (EOSClientDialog), "Split Worker Started");
      if (this.ngSplitWorker.CancellationPending)
        return;
      this.waitSplitAssignmentJobComplete();
    }

    private void waitSplitAssignmentJobComplete()
    {
      GetClassificationJobResponse classificationJobResponse = new GetClassificationJobResponse();
      int num = 25;
      Task<GetClassificationJobResponse> task;
      for (; classificationJobResponse.status != "Success"; classificationJobResponse = task.Result)
      {
        if (num <= 75)
          this.ngSplitWorker.ReportProgress(num++);
        Thread.Sleep(1000);
        if (this.ngSplitWorker.CancellationPending)
          return;
        Tracing.Log(EOSClientDialog.sw, TraceLevel.Verbose, nameof (EOSClientDialog), "waitSplitAssignmentJobComplete :: Calling CheckClassificationJobStatus");
        task = this.eosClient.CheckDocClassificationJobStatus(this.jobId);
        Tracing.Log(EOSClientDialog.sw, TraceLevel.Verbose, nameof (EOSClientDialog), "waitSplitAssignmentJobComplete :: Waiting for CheckClassificationJobStatus Task");
        Task.WaitAll((Task) task);
      }
      this.classificationResponse = classificationJobResponse;
      this.ngSplitWorker.ReportProgress(100);
    }

    private void ngSplitWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
      this.pbOverall.Value = e.ProgressPercentage;
    }

    private void ngSplitWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      PerformanceMeter.Current.AddCheckpoint("END", 804, nameof (ngSplitWorker_RunWorkerCompleted), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\Services\\EOSClientDialog.cs");
      if (e.Error != null)
      {
        this.IsClassificationFailed = this.eosClient.IsClassificationFailed;
        Tracing.Log(EOSClientDialog.sw, TraceLevel.Error, nameof (EOSClientDialog), e.Error.ToString());
        string str = string.Empty;
        if (e.Error.InnerException != null && e.Error.InnerException.Message.LastIndexOf("Correlation") >= 0)
          str = e.Error.InnerException.Message.Substring(e.Error.InnerException.Message.LastIndexOf("Correlation"));
        int num = (int) Utils.Dialog((IWin32Window) this, "Something went wrong trying to auto assign the document. Please try again. If the error persists, please contact ICE Mortgage Technology support.\n\n" + str, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      Tracing.Log(EOSClientDialog.sw, TraceLevel.Verbose, nameof (EOSClientDialog), "Closing Window");
      if (this.classificationResponse == null)
        this.DialogResult = DialogResult.Cancel;
      else
        this.DialogResult = DialogResult.OK;
    }

    private void CancelClassificationJob()
    {
      Tracing.Log(EOSClientDialog.sw, TraceLevel.Verbose, nameof (EOSClientDialog), "CancelClassificationJob :: Calling CheckClassificationJobStatus");
      Task.WaitAll(this.eosClient.CancelDocClassificationJob(this.jobId));
      Tracing.Log(EOSClientDialog.sw, TraceLevel.Verbose, nameof (EOSClientDialog), "CancelClassificationJob :: Calling CheckClassificationJobStatus");
      Task<GetClassificationJobResponse> task = this.eosClient.CheckDocClassificationJobStatus(this.jobId);
      Tracing.Log(EOSClientDialog.sw, TraceLevel.Verbose, nameof (EOSClientDialog), "CancelClassificationJob :: Waiting for CheckClassificationJobStatus Task");
      Task.WaitAll((Task) task);
      if (!string.Equals(task.Result.status, "Cancelled", StringComparison.InvariantCultureIgnoreCase))
        return;
      this.classificationResponse = task.Result;
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      Tracing.Log(EOSClientDialog.sw, TraceLevel.Verbose, nameof (EOSClientDialog), "Cancelling");
      this.mergeWorker.CancelAsync();
      if (!this.classificationWorker.IsBusy)
        return;
      this.classificationWorker.CancelAsync();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.mergeWorker = new BackgroundWorker();
      this.splitWorker = new BackgroundWorker();
      this.btnCancel = new Button();
      this.lblOverall = new Label();
      this.pbOverall = new ProgressBar();
      this.classificationWorker = new BackgroundWorker();
      this.ngSplitWorker = new BackgroundWorker();
      this.SuspendLayout();
      this.mergeWorker.WorkerReportsProgress = true;
      this.mergeWorker.WorkerSupportsCancellation = true;
      this.mergeWorker.DoWork += new DoWorkEventHandler(this.mergeWorker_DoWork);
      this.mergeWorker.ProgressChanged += new ProgressChangedEventHandler(this.mergeWorker_ProgressChanged);
      this.mergeWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.mergeWorker_RunWorkerCompleted);
      this.splitWorker.WorkerReportsProgress = true;
      this.splitWorker.WorkerSupportsCancellation = true;
      this.splitWorker.DoWork += new DoWorkEventHandler(this.splitWorker_DoWork);
      this.splitWorker.ProgressChanged += new ProgressChangedEventHandler(this.splitWorker_ProgressChanged);
      this.splitWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.splitWorker_RunWorkerCompleted);
      this.btnCancel.Location = new Point(312, 52);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 9;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.lblOverall.AutoSize = true;
      this.lblOverall.Location = new Point(12, 12);
      this.lblOverall.Name = "lblOverall";
      this.lblOverall.Size = new Size(97, 14);
      this.lblOverall.TabIndex = 5;
      this.lblOverall.Text = "Overall Progress...";
      this.pbOverall.Location = new Point(12, 28);
      this.pbOverall.Name = "pbOverall";
      this.pbOverall.Size = new Size(374, 16);
      this.pbOverall.Style = ProgressBarStyle.Continuous;
      this.pbOverall.TabIndex = 6;
      this.classificationWorker.WorkerReportsProgress = true;
      this.classificationWorker.WorkerSupportsCancellation = true;
      this.classificationWorker.DoWork += new DoWorkEventHandler(this.classificationWorker_DoWork);
      this.classificationWorker.ProgressChanged += new ProgressChangedEventHandler(this.classificationWorker_ProgressChanged);
      this.classificationWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.classificationWorker_RunWorkerCompleted);
      this.ngSplitWorker.WorkerReportsProgress = true;
      this.ngSplitWorker.WorkerSupportsCancellation = true;
      this.ngSplitWorker.DoWork += new DoWorkEventHandler(this.ngSplitWorker_DoWork);
      this.ngSplitWorker.ProgressChanged += new ProgressChangedEventHandler(this.ngSplitWorker_ProgressChanged);
      this.ngSplitWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.ngSplitWorker_RunWorkerCompleted);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(398, 85);
      this.ControlBox = false;
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.lblOverall);
      this.Controls.Add((Control) this.pbOverall);
      this.Font = new Font("Arial", 8.25f);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (EOSClientDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Preparing Document";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
