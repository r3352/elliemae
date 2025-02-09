// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Viewers.CloudAttachmentViewerControl
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using Elli.Web.Host;
using Elli.Web.Host.EventObjects;
using Elli.Web.Host.Interface;
using EllieMae.EMLite.ClientServer.eFolder;
using EllieMae.EMLite.ClientServer.SkyDrive;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.eFolder.Files;
using EllieMae.EMLite.LoanUtils.Services;
using EllieMae.EMLite.LoanUtils.SkyDrive;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Caching;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.Viewers
{
  public class CloudAttachmentViewerControl : UserControl
  {
    private const string className = "CloudAttachmentViewerControl";
    private static readonly string sw = Tracing.SwEFolder;
    private FileAttachment[] attachmentList;
    private bool loadAttachments;
    private AttachmentView[] attachmentViewList;
    public WebHost webHost;
    private long startTicks;
    private bool browserLoaded;
    private static Cache skyDriveUrlCache = new Cache();
    private IContainer components;
    private Label lblMessage;
    private BackgroundWorker worker;

    public CloudAttachmentViewerControl()
    {
      this.InitializeComponent();
      this.lblMessage.Location = new Point(0, 0);
    }

    private void CloudAttachmentViewerControl_Resize(object sender, EventArgs e)
    {
      this.lblMessage.Size = this.ClientSize;
      if (this.webHost == null)
        return;
      this.webHost.Size = this.ClientSize;
    }

    public void CloseFile()
    {
      Tracing.Log(CloudAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (CloudAttachmentViewerControl), nameof (CloseFile));
      try
      {
        if (this.attachmentList == null)
          return;
        Tracing.Log(CloudAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (CloudAttachmentViewerControl), "Clearing Attachment List");
        this.attachmentList = (FileAttachment[]) null;
        Tracing.Log(CloudAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (CloudAttachmentViewerControl), "Checking Worker");
        if (!this.worker.IsBusy)
          return;
        Tracing.Log(CloudAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (CloudAttachmentViewerControl), "Cancelling Worker");
        this.worker.CancelAsync();
        Tracing.Log(CloudAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (CloudAttachmentViewerControl), "Clearing Load Attachments Flag");
        this.loadAttachments = false;
      }
      catch (Exception ex)
      {
        Tracing.Log(CloudAttachmentViewerControl.sw, TraceLevel.Error, nameof (CloudAttachmentViewerControl), ex.ToString());
        this.showMessage("Something went wrong trying to trying to close the file. Please try again.\n\nIf the error persists, please contact ICE Mortgage Technology support.\n\n" + this.getCorrelationId(ex));
      }
    }

    public void LoadFiles(FileAttachment[] attachmentList)
    {
      Tracing.Log(CloudAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (CloudAttachmentViewerControl), nameof (LoadFiles));
      try
      {
        if (this.attachmentList != null)
        {
          bool flag = this.attachmentList.Length != attachmentList.Length;
          foreach (FileAttachment attachment in this.attachmentList)
          {
            if (Array.IndexOf<FileAttachment>(this.attachmentList, attachment) != Array.IndexOf<FileAttachment>(attachmentList, attachment))
              flag = true;
          }
          if (!flag)
            return;
        }
        using (PerformanceMeter.StartNew(nameof (LoadFiles), "CloudAttachmentViewerControl.LoadFiles", 130, nameof (LoadFiles), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\Viewers\\CloudAttachmentViewerControl.cs"))
        {
          this.startTicks = DateTime.Now.Ticks;
          PerformanceMeter.Current.AddCheckpoint("LoadFiles BEGIN TICKS: " + (object) this.startTicks, 135, nameof (LoadFiles), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\Viewers\\CloudAttachmentViewerControl.cs");
          this.MergeJobId = string.Empty;
          Tracing.Log(CloudAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (CloudAttachmentViewerControl), "Showing Loading Message");
          this.showMessage("Preparing Document...");
          Tracing.Log(CloudAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (CloudAttachmentViewerControl), "Setting Attachment List");
          this.attachmentList = attachmentList;
          Tracing.Log(CloudAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (CloudAttachmentViewerControl), "Checking Worker");
          if (this.worker.IsBusy)
          {
            Tracing.Log(CloudAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (CloudAttachmentViewerControl), "Cancelling Worker");
            this.worker.CancelAsync();
            Tracing.Log(CloudAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (CloudAttachmentViewerControl), "Setting Load Attachments Flag");
            this.loadAttachments = true;
          }
          else
          {
            Tracing.Log(CloudAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (CloudAttachmentViewerControl), "Starting Worker");
            this.worker.RunWorkerAsync((object) attachmentList);
            Tracing.Log(CloudAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (CloudAttachmentViewerControl), "Clearing Load Attachments Flag");
            this.loadAttachments = false;
          }
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(CloudAttachmentViewerControl.sw, TraceLevel.Error, nameof (CloudAttachmentViewerControl), ex.ToString());
        this.showMessage("Something went wrong trying to load the files. Please try again.\n\nIf the error persists, please contact ICE Mortgage Technology support.\n\n" + this.getCorrelationId(ex));
      }
    }

    public string MergeJobId { get; set; }

    public bool CanCloseViewer()
    {
      try
      {
        if (this.webHost == null)
          return true;
        ViewerMessage2 eventParams = new ViewerMessage2()
        {
          type = "CloseFile"
        };
        Tracing.Log(CloudAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (CloudAttachmentViewerControl), "Calling WebHost.RaiseEvent: raiseMessage for " + eventParams.type);
        ViewerResponse viewerResponse = this.webHost.RaiseEvent<ViewerResponse>("raiseMessage", (object) eventParams, 20000);
        Tracing.Log(CloudAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (CloudAttachmentViewerControl), "Checking WebHost.RaiseEvent Result");
        if (viewerResponse == null)
        {
          Tracing.Log(CloudAttachmentViewerControl.sw, TraceLevel.Warning, nameof (CloudAttachmentViewerControl), "RaiseEvent Response: Null");
          return true;
        }
        if (!string.IsNullOrEmpty(viewerResponse.error))
        {
          Tracing.Log(CloudAttachmentViewerControl.sw, TraceLevel.Warning, nameof (CloudAttachmentViewerControl), "RaiseEvent Response: Error=" + viewerResponse.error);
          throw new Exception(viewerResponse.error);
        }
        if (!viewerResponse.isCloseFile)
          return Utils.Dialog((IWin32Window) Form.ActiveForm, "There are pending changes to the current file.\r\n\r\nClick OK to continue without saving.\r\nClick Cancel to continue working on current file.", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1).Equals((object) DialogResult.OK);
        Tracing.Log(CloudAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (CloudAttachmentViewerControl), "RaiseEvent Response: True");
        return true;
      }
      catch (Exception ex)
      {
        Tracing.Log(CloudAttachmentViewerControl.sw, TraceLevel.Error, nameof (CloudAttachmentViewerControl), ex.ToString());
        return true;
      }
    }

    private void showMessage(string message)
    {
      Tracing.Log(CloudAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (CloudAttachmentViewerControl), "showMessage: " + message);
      this.lblMessage.Text = message;
      this.lblMessage.BringToFront();
    }

    private void worker_DoWork(object sender, DoWorkEventArgs e)
    {
      Tracing.Log(CloudAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (CloudAttachmentViewerControl), "Worker Started");
      e.Cancel = true;
      Tracing.Log(CloudAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (CloudAttachmentViewerControl), "Getting File List");
      FileAttachment[] attachments = (FileAttachment[]) e.Argument;
      try
      {
        this.attachmentViewList = this.getAttachmentViewList(attachments);
      }
      catch (CanceledOperationException ex)
      {
        Tracing.Log(CloudAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (CloudAttachmentViewerControl), "Worker Cancelled");
        return;
      }
      e.Cancel = this.worker.CancellationPending;
    }

    private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
      if (this.worker.CancellationPending)
        return;
      this.showMessage(e.UserState.ToString());
    }

    private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      Tracing.Log(CloudAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (CloudAttachmentViewerControl), "Worker Complete");
      if (this.loadAttachments)
      {
        Tracing.Log(CloudAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (CloudAttachmentViewerControl), "Starting Worker");
        this.worker.RunWorkerAsync((object) this.attachmentList);
        Tracing.Log(CloudAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (CloudAttachmentViewerControl), "Clearing Load Files Flag");
        this.loadAttachments = false;
      }
      else if (e.Error != null)
      {
        Tracing.Log(CloudAttachmentViewerControl.sw, TraceLevel.Error, nameof (CloudAttachmentViewerControl), e.Error.ToString());
        this.showMessage("Something went wrong trying to prepare the document. Please try again.\n\nIf the error persists, please contact ICE Mortgage Technology support.\n\n" + this.getCorrelationId(e.Error));
      }
      else
      {
        if (e.Cancelled)
          return;
        if (((IEnumerable<AttachmentView>) this.attachmentViewList).Any<AttachmentView>((Func<AttachmentView, bool>) (x => x.dragDropJobStatus == "Failed")))
          this.showFailedJob();
        else
          this.showFile(this.attachmentViewList);
      }
    }

    private AttachmentView[] getAttachmentViewList(FileAttachment[] attachments)
    {
      using (PerformanceMeter.StartNew("getAttachmentUrl", "CloudAttachmentViewerControl.getAttachmentUrl", 342, nameof (getAttachmentViewList), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\Viewers\\CloudAttachmentViewerControl.cs"))
      {
        List<AttachmentView> source1 = new List<AttachmentView>();
        foreach (CloudAttachment attachment in attachments)
        {
          AttachmentView attachmentView = new AttachmentView()
          {
            attachmentId = attachment.ID,
            url = (string) null,
            token = (string) null,
            fileName = attachment.Title,
            dragDropJobStatus = (string) null,
            isDragDropJobRunning = false,
            objectID = attachment.ObjectId
          };
          source1.Add(attachmentView);
        }
        EOSClient eosClient = new EOSClient(Session.LoanDataMgr);
        List<AttachmentView> source2 = source1;
        ParallelOptions parallelOptions = new ParallelOptions();
        parallelOptions.MaxDegreeOfParallelism = 4;
        Action<AttachmentView, ParallelLoopState> body = (Action<AttachmentView, ParallelLoopState>) ((attachmentView, state) =>
        {
          string jobId = DragDropDetailCollection.GetJobId(Session.LoanDataMgr.LoanData.GUID, attachmentView.attachmentId);
          if (string.IsNullOrEmpty(jobId))
            return;
          Tracing.Log(CloudAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (CloudAttachmentViewerControl), "Calling CheckDragDropJobStatus: " + jobId);
          Task<DragDropJobStatusResponse> task = eosClient.CheckDragDropJobStatus(jobId);
          Tracing.Log(CloudAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (CloudAttachmentViewerControl), "Waiting for CheckDragDropJobStatus Task: " + jobId);
          Task.WaitAll((Task) task);
          PerformanceMeter.Current.AddCheckpoint("CheckDragDropJobStatus completed: " + jobId, 391, nameof (getAttachmentViewList), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\Viewers\\CloudAttachmentViewerControl.cs");
          Tracing.Log(CloudAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (CloudAttachmentViewerControl), "Getting CheckDragDropJobStatus Response: " + jobId);
          DragDropJobStatusResponse result = task.Result;
          Tracing.Log(CloudAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (CloudAttachmentViewerControl), string.Format("DragDropJobStatus: JobId={0}, Status={1}, AttachmentId={2}", (object) jobId, (object) result.status, (object) attachmentView.attachmentId));
          if (result.status == "Running")
          {
            attachmentView.url = result.drop.entityUri;
            attachmentView.token = result.drop.authorizationHeader;
            attachmentView.dragDropJobStatus = "Running";
            attachmentView.isDragDropJobRunning = true;
          }
          else if (result.status == "Success")
          {
            CloudAttachment cloudAttachment = ((IEnumerable<FileAttachment>) attachments).FirstOrDefault<FileAttachment>((Func<FileAttachment, bool>) (x => x.ID == attachmentView.attachmentId)) as CloudAttachment;
            Tracing.Log(CloudAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (CloudAttachmentViewerControl), "Calling Resync: " + cloudAttachment.ID);
            Session.LoanDataMgr.FileAttachments.Resync(cloudAttachment);
            Tracing.Log(CloudAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (CloudAttachmentViewerControl), "Getting Hidden Attachments: " + jobId);
            foreach (string hiddenAttachmentId in DragDropDetailCollection.GetHiddenAttachmentIds(jobId))
            {
              FileAttachment hiddenAttachment = Session.LoanDataMgr.FileAttachments.GetHiddenAttachment(hiddenAttachmentId);
              if (hiddenAttachment != null)
              {
                Tracing.Log(CloudAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (CloudAttachmentViewerControl), "Removing Hidden Attachment: " + hiddenAttachmentId);
                Session.LoanDataMgr.FileAttachments.Remove(RemoveReasonType.Merge, hiddenAttachment);
              }
            }
            Tracing.Log(CloudAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (CloudAttachmentViewerControl), "Removing DragDropJob: " + jobId);
            DragDropDetailCollection.RemoveJobId(jobId);
            attachmentView.url = result.drop.entityUri;
            attachmentView.token = result.drop.authorizationHeader;
            attachmentView.dragDropJobStatus = "Success";
            attachmentView.isDragDropJobRunning = false;
          }
          else
          {
            if (!(result.status == "Failed"))
              return;
            Tracing.Log(CloudAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (CloudAttachmentViewerControl), "Removing DragDropJob: " + jobId);
            DragDropDetailCollection.RemoveJobId(jobId);
            attachmentView.dragDropJobStatus = "Failed";
            attachmentView.isDragDropJobRunning = false;
          }
        });
        Parallel.ForEach<AttachmentView>((IEnumerable<AttachmentView>) source2, parallelOptions, body);
        AttachmentView[] array = source1.Where<AttachmentView>((Func<AttachmentView, bool>) (x => string.IsNullOrEmpty(x.url))).ToArray<AttachmentView>();
        if (array.Length != 0)
        {
          List<SkyDriveUrl> skyDriveUrls = this.getSkyDriveUrls(((IEnumerable<AttachmentView>) array).Select<AttachmentView, string>((Func<AttachmentView, string>) (x => x.objectID)).ToArray<string>());
          Tracing.Log(CloudAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (CloudAttachmentViewerControl), "Assigning Pre-Signed URLs");
          foreach (AttachmentView attachmentView1 in array)
          {
            AttachmentView attachmentView = attachmentView1;
            SkyDriveUrl skyDriveUrl = skyDriveUrls.FirstOrDefault<SkyDriveUrl>((Func<SkyDriveUrl, bool>) (x => x.id == attachmentView.objectID));
            attachmentView.url = skyDriveUrl != null ? skyDriveUrl.url : throw new FileNotFoundException("Failed to generate Pre-Signed URL", attachmentView.objectID);
            attachmentView.token = skyDriveUrl.authorizationHeader;
          }
        }
        return source1.ToArray();
      }
    }

    private List<SkyDriveUrl> getSkyDriveUrls(string[] objectIds)
    {
      List<SkyDriveUrl> skyDriveUrls = new List<SkyDriveUrl>();
      List<string> stringList = new List<string>();
      foreach (string objectId in objectIds)
      {
        if (CloudAttachmentViewerControl.skyDriveUrlCache.Get(objectId) == null)
          stringList.Add(objectId);
        else
          skyDriveUrls.Add((SkyDriveUrl) CloudAttachmentViewerControl.skyDriveUrlCache.Get(objectId));
      }
      if (stringList.Count > 0)
      {
        Tracing.Log(CloudAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (CloudAttachmentViewerControl), "Calling GetSkyDriveUrlForObjects: " + (object) stringList.ToArray().Length);
        List<SkyDriveUrl> driveUrlForObjects = Session.LoanDataMgr.LoanObject.GetSkyDriveUrlForObjects(stringList.ToArray());
        Tracing.Log(CloudAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (CloudAttachmentViewerControl), "Checking GetSkyDriveUrlForObjects Result");
        if (driveUrlForObjects == null)
          throw new FileNotFoundException("Failed to generate Pre-Signed URLs");
        foreach (string str in stringList)
        {
          string objectId = str;
          SkyDriveUrl skyDriveUrl = driveUrlForObjects.FirstOrDefault<SkyDriveUrl>((Func<SkyDriveUrl, bool>) (x => x.id == objectId));
          if (skyDriveUrl == null)
            throw new FileNotFoundException("Failed to generate Pre-Signed URL", objectId);
          CloudAttachmentViewerControl.skyDriveUrlCache.Add(objectId, (object) skyDriveUrl, (CacheDependency) null, DateTime.UtcNow.AddMinutes(10.0), Cache.NoSlidingExpiration, CacheItemPriority.Normal, (CacheItemRemovedCallback) null);
          skyDriveUrls.Add(skyDriveUrl);
        }
      }
      return skyDriveUrls;
    }

    private void showFailedJob()
    {
      int num = (int) Utils.Dialog((IWin32Window) this, "An action failed for this file. Please try again.\n\nIf the error persists, please contact ICE Mortgage Technology support.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      FileAttachment file = (FileAttachment) null;
      if (this.attachmentList.Length == 1)
        file = Session.LoanDataMgr.FileAttachments[this.attachmentList[0].ID];
      this.OnViewerFileChanged(file);
    }

    private void showFile(AttachmentView[] attachmentViews)
    {
      using (PerformanceMeter.StartNew(nameof (showFile), "CloudAttachmentViewerControl.showFile", 569, nameof (showFile), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\Viewers\\CloudAttachmentViewerControl.cs"))
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 571, nameof (showFile), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\Viewers\\CloudAttachmentViewerControl.cs");
        try
        {
          if (this.webHost == null)
          {
            Tracing.Log(CloudAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (CloudAttachmentViewerControl), "Creating WebHost");
            this.webHost = new WebHost("sc", new Elli.Web.Host.PostMessageHandler(this.PostMessageHandler));
            this.webHost.PageComplete += new PageCompleteEventHandler(this.webhost_PageComplete);
            this.webHost.BrowserTitleChanged += new EventHandler<TitleChangeEventArgs>(this.webhost_TitleChanged);
            this.Controls.Add((Control) this.webHost);
            this.webHost.SetBounds(0, 0, this.ClientSize.Width, this.ClientSize.Height);
            PerformanceMeter.Current.AddCheckpoint("WebHost created.", 589, nameof (showFile), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\Viewers\\CloudAttachmentViewerControl.cs");
          }
          Dictionary<string, object> parameters1 = new Dictionary<string, object>();
          parameters1.Add("loanGuid", (object) Guid.Parse(Session.LoanDataMgr.LoanData.GUID).ToString());
          parameters1.Add("oapiBaseUrl", (object) Session.LoanDataMgr.SessionObjects.StartupInfo.OAPIGatewayBaseUri);
          parameters1.Add("responsiveBreakpoint", (object) 0);
          parameters1.Add("fullscreenDisabled", (object) true);
          parameters1.Add("beforeUnloadDisabled", (object) true);
          parameters1.Add("userId", (object) string.Format("Encompass\\{0}\\{1}", (object) Session.LoanDataMgr.SessionObjects.StartupInfo.ServerInstanceName, (object) Session.DefaultInstance.UserInfo.Userid));
          if (attachmentViews.Length == 1 && this.attachmentList.Length == 1)
          {
            FileAttachment attachment = this.attachmentList[0];
            parameters1.Add("attachmentId", (object) attachment.ID);
            parameters1.Add("fileName", (object) attachment.Title);
            parameters1.Add("viewOriginalDisabled", (object) false);
            parameters1.Add("permissions", (object) this.getPermissions(attachment));
            parameters1.Add("url", (object) attachmentViews[0].url);
            parameters1.Add("authorizationHeader", (object) attachmentViews[0].token);
            parameters1.Add("isDragDropJobRunning", (object) attachmentViews[0].isDragDropJobRunning);
            if (!string.IsNullOrEmpty(attachmentViews[0].dragDropJobStatus))
              parameters1.Add("dragDropJobStatus", (object) attachmentViews[0].dragDropJobStatus);
          }
          else
          {
            parameters1.Add("fileName", (object) "MergedDocument.pdf");
            parameters1.Add("viewOriginalDisabled", (object) true);
            parameters1.Add("permissions", (object) this.getPermissions((FileAttachment) null));
            parameters1.Add("mergeView", (object) JsonConvert.SerializeObject((object) attachmentViews));
          }
          if (this.browserLoaded)
          {
            Tracing.Log(CloudAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (CloudAttachmentViewerControl), "Calling refreshViewer.");
            this.refreshViewer(parameters1);
            PerformanceMeter.Current.AddCheckpoint("refreshViewer called.", 635, nameof (showFile), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\Viewers\\CloudAttachmentViewerControl.cs");
          }
          else
          {
            ModuleParameters parameters2 = new ModuleParameters()
            {
              User = this.getModuleUser(),
              Parameters = parameters1
            };
            PerformanceMeter.Current.AddCheckpoint("ModuleParameters created.", 646, nameof (showFile), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\Viewers\\CloudAttachmentViewerControl.cs");
            string hostUrl = Session.LoanDataMgr.SessionObjects.StartupInfo.ViewerUrl + "host.html?r=" + Guid.NewGuid().ToString("D").ToLower();
            string guestUrl = Session.LoanDataMgr.SessionObjects.StartupInfo.ViewerUrl + "index.html";
            Tracing.Log(CloudAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (CloudAttachmentViewerControl), "Calling LoadModule: " + hostUrl);
            this.webHost.LoadModule(hostUrl, guestUrl, parameters2, true);
            PerformanceMeter.Current.AddCheckpoint("LoadModule called.", 659, nameof (showFile), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\Viewers\\CloudAttachmentViewerControl.cs");
          }
        }
        catch (Exception ex)
        {
          Tracing.Log(CloudAttachmentViewerControl.sw, TraceLevel.Error, nameof (CloudAttachmentViewerControl), ex.ToString());
          this.showMessage("Something went wrong trying to show the document. Please try again.\n\nIf the error persists, please contact ICE Mortgage Technology support.\n\n" + this.getCorrelationId(ex));
        }
        PerformanceMeter.Current.AddCheckpoint("END", 670, nameof (showFile), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\Viewers\\CloudAttachmentViewerControl.cs");
      }
    }

    private string getCorrelationId(Exception ex)
    {
      for (; ex != null; ex = ex.InnerException)
      {
        int startIndex = ex.Message.LastIndexOf("Correlation");
        if (startIndex >= 0)
          return ex.Message.Substring(startIndex);
      }
      return string.Empty;
    }

    private ModuleUser getModuleUser()
    {
      UserInfo userInfo = Session.UserInfo;
      return new ModuleUser()
      {
        ID = userInfo.Userid,
        LastName = userInfo.LastName,
        FirstName = userInfo.FirstName,
        Email = userInfo.Email
      };
    }

    private string[] getPermissions(FileAttachment attachment)
    {
      List<string> stringList = new List<string>();
      DocumentLog logEntry = (DocumentLog) null;
      if (attachment != null)
        logEntry = Session.LoanDataMgr.FileAttachments.GetLinkedDocument(attachment.ID);
      eFolderAccessRights folderAccessRights = new eFolderAccessRights(Session.LoanDataMgr, (LogRecordBase) logEntry);
      if (folderAccessRights.CanViewAllAnnotations)
        stringList.Add("CanViewAllAnnotations");
      if (attachment != null)
      {
        if (folderAccessRights.CanEditFile)
        {
          stringList.Add("CanRotatePages");
          stringList.Add("CanRearrangePages");
        }
        if (folderAccessRights.CanDeletePageFromFile)
          stringList.Add("CanDeletePages");
        if (folderAccessRights.CanAnnotateFiles)
        {
          stringList.Add("CanAddAnnotations");
          stringList.Add("CanEditAnnotations");
        }
        if (folderAccessRights.CanDeleteAnnotations)
          stringList.Add("CanDeleteAnnotations");
        if (folderAccessRights.CanSplitFiles)
          stringList.Add("CanSplitFiles");
        if (folderAccessRights.CanMergeFiles)
          stringList.Add("CanMergeFiles");
        if (folderAccessRights.CanDeleteFiles && Session.LoanDataMgr.Writable)
          stringList.Add("CanDeleteFiles");
      }
      return stringList.ToArray();
    }

    private void refreshViewer(Dictionary<string, object> parameters)
    {
      try
      {
        ViewerMessage2 eventParams = new ViewerMessage2()
        {
          type = "RefreshParameters",
          payload = new ViewerMessagePayload()
          {
            parameters = parameters
          }
        };
        Tracing.Log(CloudAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (CloudAttachmentViewerControl), "Calling WebHost.RaiseEvent: raiseMessage for " + eventParams.type);
        ViewerResponse viewerResponse = this.webHost.RaiseEvent<ViewerResponse>("raiseMessage", (object) eventParams, 20000);
        Tracing.Log(CloudAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (CloudAttachmentViewerControl), "Checking WebHost.RaiseEvent Result");
        if (viewerResponse == null)
        {
          Tracing.Log(CloudAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (CloudAttachmentViewerControl), "RaiseEvent Response: Null");
        }
        else
        {
          if (!string.IsNullOrEmpty(viewerResponse.error))
          {
            Tracing.Log(CloudAttachmentViewerControl.sw, TraceLevel.Warning, nameof (CloudAttachmentViewerControl), "RaiseEvent Response: Error=" + viewerResponse.error);
            throw new Exception(viewerResponse.error);
          }
          if (((IEnumerable<FileAttachment>) this.attachmentList).Count<FileAttachment>() > 1)
          {
            FileAttachment attachment = this.attachmentList[0];
            if (attachment != null)
              this.OnViewerMergeFileSelected(attachment);
          }
          this.webHost.BringToFront();
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(CloudAttachmentViewerControl.sw, TraceLevel.Error, nameof (CloudAttachmentViewerControl), ex.ToString());
      }
    }

    private void webhost_PageComplete(object sender, FinishedLoadingEventArgs e)
    {
      using (PerformanceMeter.StartNew("Page Complete", "CloudAttachmentViewerControl.webhost_FrameComplete", 839, nameof (webhost_PageComplete), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\Viewers\\CloudAttachmentViewerControl.cs"))
      {
        Tracing.Log(CloudAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (CloudAttachmentViewerControl), "Page complete: " + e.ValidatedURL);
        if (this.loadAttachments)
          return;
        this.webHost.BringToFront();
        if (this.attachmentList != null && this.attachmentList.Length > 1)
        {
          string sw = CloudAttachmentViewerControl.sw;
          object[] objArray1 = new object[5]
          {
            (object) "WebHost finished loading: ",
            null,
            null,
            null,
            null
          };
          TimeSpan timeSpan = TimeSpan.FromTicks(DateTime.Now.Ticks - this.startTicks);
          objArray1[1] = (object) timeSpan.TotalMilliseconds;
          objArray1[2] = (object) " ms, ";
          objArray1[3] = (object) this.attachmentList.Length;
          objArray1[4] = (object) " files";
          string msg = string.Concat(objArray1);
          Tracing.Log(sw, TraceLevel.Info, nameof (CloudAttachmentViewerControl), msg);
          PerformanceMeter current = PerformanceMeter.Current;
          object[] objArray2 = new object[5]
          {
            (object) "WebHost finished loading END TICKS: ",
            null,
            null,
            null,
            null
          };
          timeSpan = TimeSpan.FromTicks(DateTime.Now.Ticks - this.startTicks);
          objArray2[1] = (object) timeSpan.TotalMilliseconds;
          objArray2[2] = (object) " ms, ";
          objArray2[3] = (object) this.attachmentList.Length;
          objArray2[4] = (object) " files";
          string description = string.Concat(objArray2);
          current.AddCheckpoint(description, 852, nameof (webhost_PageComplete), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\Viewers\\CloudAttachmentViewerControl.cs");
        }
        this.browserLoaded = true;
      }
    }

    private void webhost_TitleChanged(object sender, TitleChangeEventArgs e)
    {
      Tracing.Log(CloudAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (CloudAttachmentViewerControl), "Browser title changed: " + e.Title);
      this.PostMessageHandler(e.Title);
    }

    public event FileAttachmentEventHandler ViewerFileChanged;

    protected virtual void OnViewerFileChanged(FileAttachment file)
    {
      if (this.ViewerFileChanged == null)
        return;
      Tracing.Log(CloudAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (CloudAttachmentViewerControl), "Raising ViewerFileChanged Event");
      if (this.InvokeRequired)
        this.Invoke((Delegate) (() => this.ViewerFileChanged((object) this, new FileAttachmentEventArgs(file))));
      else
        this.ViewerFileChanged((object) this, new FileAttachmentEventArgs(file));
    }

    public event FileAttachmentEventHandler ViewerMergeFileSelected;

    protected virtual void OnViewerMergeFileSelected(FileAttachment file)
    {
      if (this.ViewerMergeFileSelected == null)
        return;
      Tracing.Log(CloudAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (CloudAttachmentViewerControl), "Raising ViewerMergeFileSelected Event");
      if (this.InvokeRequired)
        this.Invoke((Delegate) (() => this.ViewerMergeFileSelected((object) this, new FileAttachmentEventArgs(file))));
      else
        this.ViewerMergeFileSelected((object) this, new FileAttachmentEventArgs(file));
    }

    internal void BrowserDragDrop(ViewerMessage2 viewerMessage)
    {
      try
      {
        using (CursorActivator.Wait())
        {
          if (this.webHost == null)
            return;
          Tracing.Log(CloudAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (CloudAttachmentViewerControl), "Calling WebHost.RaiseEvent: raiseMessage for " + viewerMessage.type);
          ViewerResponse viewerResponse = this.webHost.RaiseEvent<ViewerResponse>("raiseMessage", (object) viewerMessage, 20000);
          Tracing.Log(CloudAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (CloudAttachmentViewerControl), "Checking WebHost.RaiseEvent Result");
          if (viewerResponse == null)
          {
            Tracing.Log(CloudAttachmentViewerControl.sw, TraceLevel.Warning, nameof (CloudAttachmentViewerControl), "RaiseEvent Response: Null");
          }
          else
          {
            if (!string.IsNullOrEmpty(viewerResponse.error))
            {
              Tracing.Log(CloudAttachmentViewerControl.sw, TraceLevel.Warning, nameof (CloudAttachmentViewerControl), "RaiseEvent Response: Error=" + viewerResponse.error);
              throw new Exception(viewerResponse.error);
            }
            if (!string.IsNullOrEmpty(viewerResponse.dragDropJobId))
            {
              DragDropDetailCollection.AddDragDropDetail(new DragDropDetail()
              {
                attachmentId = viewerResponse.attachmentId,
                jobId = viewerResponse.dragDropJobId,
                loanId = Session.LoanDataMgr.LoanData.GUID
              });
              if (!string.IsNullOrEmpty(viewerResponse.hideAttachmentId))
              {
                Session.LoanDataMgr.FileAttachments.GetLinkedDocument(viewerResponse.hideAttachmentId)?.Files.Remove(viewerResponse.hideAttachmentId);
                DragDropDetailCollection.AddDragDropDetail(new DragDropDetail()
                {
                  attachmentId = viewerResponse.hideAttachmentId,
                  jobId = viewerResponse.dragDropJobId,
                  hideAttachment = true,
                  loanId = Session.LoanDataMgr.LoanData.GUID
                });
                this.OnViewerFileChanged((FileAttachment) null);
                return;
              }
            }
            if (!(viewerMessage.type == "CreateFile"))
              return;
            if (!(Session.LoanDataMgr.FileAttachments[viewerResponse.attachmentId] is CloudAttachment fileAttachment))
            {
              Session.LoanDataMgr.FileAttachments.Resync(new string[1]
              {
                viewerResponse.attachmentId
              });
            }
            else
            {
              Session.LoanDataMgr.FileAttachments.Resync(fileAttachment);
              DocumentLog linkedDocument = Session.LoanDataMgr.FileAttachments.GetLinkedDocument(fileAttachment.ID);
              if (linkedDocument != null && linkedDocument.Guid != viewerResponse.documentId)
                linkedDocument.Files.Remove(fileAttachment.ID);
            }
            if (string.IsNullOrEmpty(viewerResponse.documentId) || !(Session.LoanData.GetLogList().GetRecordByID(viewerResponse.documentId) is DocumentLog recordById))
              return;
            recordById.Files.Add(viewerResponse.attachmentId, Session.UserID);
          }
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(CloudAttachmentViewerControl.sw, TraceLevel.Error, nameof (CloudAttachmentViewerControl), ex.ToString());
        this.showMessage("Something went wrong with this file operation. Please try again.\n\nIf the error persists, please contact ICE Mortgage Technology support.\n\n" + ex.Message);
      }
    }

    public string PostMessageHandler(string payload)
    {
      Tracing.Log(CloudAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (CloudAttachmentViewerControl), "PostMessage: " + payload);
      try
      {
        ViewerMessage message = JsonConvert.DeserializeObject<ViewerMessage>(payload);
        switch (message.action)
        {
          case "CreateAnnotation":
          case "DeleteAnnotation":
          case "UpdateAnnotation":
          case "UpdateDocument":
            return this.processFileChangedMessage(message);
          case "CurrentAttachment":
            return this.processCurrentAttachmentMessage(message);
          case "DeleteFile":
            return this.processDeleteFileMessage(message);
          case "DragDropFail":
            return this.processDragDropFailMessage(message);
          case "Print":
            return this.processPrintMessage(message);
          case "SplitDocument":
            return this.processSplitMessage(message);
          case "View":
            return this.processViewMessage(message);
          default:
            throw new Exception("Unsupported action:" + message.action + ".");
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(CloudAttachmentViewerControl.sw, nameof (CloudAttachmentViewerControl), TraceLevel.Error, "PostMessage failed: " + ex.ToString());
        return "Something went wrong with this operation. Please try again.\n\nIf the error persists, please contact ICE Mortgage Technology support. Details:\n\n" + ex.ToString();
      }
    }

    private string processCurrentAttachmentMessage(ViewerMessage message)
    {
      FileAttachment fileAttachment = Session.LoanDataMgr.FileAttachments[message.attachments[0]];
      if (fileAttachment != null)
        this.OnViewerMergeFileSelected(fileAttachment);
      return string.Empty;
    }

    private string processDeleteFileMessage(ViewerMessage message)
    {
      if (!Session.LoanDataMgr.LockLoanWithExclusiveA())
        return "Error Deleting File: Unable to lock loan";
      foreach (string attachment in message.attachments)
      {
        FileAttachment fileAttachment = Session.LoanDataMgr.FileAttachments[attachment];
        if (fileAttachment != null)
          Session.LoanDataMgr.FileAttachments.Remove(RemoveReasonType.User, fileAttachment);
      }
      this.OnViewerFileChanged((FileAttachment) null);
      return string.Empty;
    }

    private string processDragDropFailMessage(ViewerMessage message)
    {
      int num = (int) Utils.Dialog((IWin32Window) this, "An action failed while processing this file. Please try again.\n\nIf the error persists, please contact your Encompass Administrator.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      DragDropDetailCollection.RemoveJobId(message.dragDropJobId);
      FileAttachment file = (FileAttachment) null;
      if (this.attachmentList != null && this.attachmentList.Length == 1)
        file = Session.LoanDataMgr.FileAttachments[this.attachmentList[0].ID];
      this.OnViewerFileChanged(file);
      return string.Empty;
    }

    private string processFileChangedMessage(ViewerMessage message)
    {
      foreach (string attachment in message.attachments)
      {
        FileAttachment fileAttachment = Session.LoanDataMgr.FileAttachments[attachment];
        if (fileAttachment != null)
          Session.LoanDataMgr.FileAttachments.OnFileAttachmentChanged(fileAttachment);
      }
      return string.Empty;
    }

    private string processPrintMessage(ViewerMessage message)
    {
      try
      {
        eFolderManager eFolderManager = new eFolderManager();
        foreach (url url in message.urls)
        {
          Tracing.Log(CloudAttachmentViewerControl.sw, nameof (CloudAttachmentViewerControl), TraceLevel.Verbose, "Calling eFolderManager.Print: " + url.URL);
          eFolderManager.Print(Session.LoanDataMgr, url.URL, url.authorizationHeader, url.currentPageNumber);
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(CloudAttachmentViewerControl.sw, nameof (CloudAttachmentViewerControl), TraceLevel.Error, "Print failed: " + ex.ToString());
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "The following error occurred when trying to print the file:\n\n" + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      return string.Empty;
    }

    private string processSplitMessage(ViewerMessage message)
    {
      Tracing.Log(CloudAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (CloudAttachmentViewerControl), "Calling FileAttachments.Resync");
      Session.LoanDataMgr.FileAttachments.Resync(message.attachments);
      Tracing.Log(CloudAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (CloudAttachmentViewerControl), "Assigning Attachments");
      for (int index = 0; index < message.attachments.Length; ++index)
      {
        string attachment = message.attachments[index];
        if (message.documents != null && message.documents.Length > index)
        {
          string document = message.documents[index];
          if (!string.IsNullOrEmpty(document) && Session.LoanData.GetLogList().GetRecordByID(document) is DocumentLog recordById)
            recordById.Files.Add(attachment, Session.UserID);
        }
      }
      return string.Empty;
    }

    private string processViewMessage(ViewerMessage message)
    {
      ViewerMessage2 eventParams = new ViewerMessage2()
      {
        type = "DownloadComplete"
      };
      try
      {
        Tracing.Log(CloudAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (CloudAttachmentViewerControl), "Creating SkyDriveStreamingClient");
        SkyDriveStreamingClient driveStreamingClient = new SkyDriveStreamingClient(Session.LoanDataMgr);
        foreach (url url1 in message.urls)
        {
          SkyDriveUrl url2 = new SkyDriveUrl((string) null, url1.URL, url1.authorizationHeader);
          string fileKey = "Attachment.pdf";
          Task<string> task = driveStreamingClient.DownloadFile(url2, fileKey, true);
          Task.WaitAll((Task) task);
          string lower = Path.GetExtension(task.Result).ToLower();
          if (string.Equals(lower, ".html") || string.Equals(lower, ".htm"))
            Process.Start("notepad.exe", task.Result);
          else
            SystemUtil.ShellExecute(task.Result);
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(CloudAttachmentViewerControl.sw, nameof (CloudAttachmentViewerControl), TraceLevel.Error, "View failed: " + ex.ToString());
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "The following error occurred when trying to view the file:\n\n" + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        eventParams.payload = new ViewerMessagePayload()
        {
          error = new ViewerErrorMessage()
          {
            message = ex.Message,
            correlationId = string.Empty,
            httpCode = string.Empty
          }
        };
      }
      Tracing.Log(CloudAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (CloudAttachmentViewerControl), "Calling WebHost.RaiseEvent: raiseMessage for " + eventParams.type);
      ViewerResponse viewerResponse = this.webHost.RaiseEvent<ViewerResponse>("raiseMessage", (object) eventParams, 20000);
      Tracing.Log(CloudAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (CloudAttachmentViewerControl), "Checking WebHost.RaiseEvent Result");
      if (viewerResponse == null)
        Tracing.Log(CloudAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (CloudAttachmentViewerControl), "RaiseEvent Response: Null");
      else if (!string.IsNullOrEmpty(viewerResponse.error))
      {
        Tracing.Log(CloudAttachmentViewerControl.sw, TraceLevel.Warning, nameof (CloudAttachmentViewerControl), "RaiseEvent Response: Error=" + viewerResponse.error);
        throw new Exception(viewerResponse.error);
      }
      return string.Empty;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.lblMessage = new Label();
      this.worker = new BackgroundWorker();
      this.SuspendLayout();
      this.lblMessage.BackColor = SystemColors.Window;
      this.lblMessage.ForeColor = SystemColors.WindowText;
      this.lblMessage.Location = new Point(340, 12);
      this.lblMessage.Name = "lblMessage";
      this.lblMessage.Size = new Size(128, 191);
      this.lblMessage.TabIndex = 3;
      this.lblMessage.Text = "Message";
      this.lblMessage.TextAlign = ContentAlignment.MiddleCenter;
      this.worker.WorkerReportsProgress = true;
      this.worker.WorkerSupportsCancellation = true;
      this.worker.DoWork += new DoWorkEventHandler(this.worker_DoWork);
      this.worker.ProgressChanged += new ProgressChangedEventHandler(this.worker_ProgressChanged);
      this.worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.worker_RunWorkerCompleted);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.lblMessage);
      this.Name = nameof (CloudAttachmentViewerControl);
      this.Size = new Size(809, 215);
      this.Resize += new EventHandler(this.CloudAttachmentViewerControl_Resize);
      this.ResumeLayout(false);
    }
  }
}
