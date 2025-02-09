// Decompiled with JetBrains decompiler
// Type: Elli.Web.Host.SkyDriveViewerControl
// Assembly: Elli.Web.Host, Version=19.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9006AF5-0CBA-41F3-A51F-BE05E37C7972
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Web.Host.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.SkyDrive;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Windows.Forms;

#nullable disable
namespace Elli.Web.Host
{
  public class SkyDriveViewerControl : UserControl
  {
    private const string className = "SkyDriveViewerControl";
    private static readonly string sw = Tracing.SwEFolder;
    private string[] attachmentIdList;
    private bool loadAttachments;
    private SkyDriveUrl skyDriveUrl;
    public WebHost webHost;
    private long startTicks;
    private static HttpClient _client = new HttpClient();
    private IContainer components;
    private Label lblMessage;
    private BackgroundWorker worker;

    public SkyDriveViewerControl()
    {
      this.InitializeComponent();
      this.lblMessage.Location = new Point(0, 0);
    }

    public void CloseFile()
    {
      Tracing.Log(SkyDriveViewerControl.sw, TraceLevel.Verbose, nameof (SkyDriveViewerControl), nameof (CloseFile));
      try
      {
        if (this.attachmentIdList == null)
          return;
        Tracing.Log(SkyDriveViewerControl.sw, TraceLevel.Verbose, nameof (SkyDriveViewerControl), "Clearing Attachment List");
        this.attachmentIdList = (string[]) null;
        Tracing.Log(SkyDriveViewerControl.sw, TraceLevel.Verbose, nameof (SkyDriveViewerControl), "Checking Worker");
        if (!this.worker.IsBusy)
          return;
        Tracing.Log(SkyDriveViewerControl.sw, TraceLevel.Verbose, nameof (SkyDriveViewerControl), "Cancelling Worker");
        this.worker.CancelAsync();
        Tracing.Log(SkyDriveViewerControl.sw, TraceLevel.Verbose, nameof (SkyDriveViewerControl), "Clearing Load Attachments Flag");
        this.loadAttachments = false;
      }
      catch (Exception ex)
      {
        Tracing.Log(SkyDriveViewerControl.sw, TraceLevel.Error, nameof (SkyDriveViewerControl), ex.ToString());
        this.showMessage("Something went wrong trying to trying to close the file. Please try again.\n\nIf the error persists, please contact Ellie Mae support.\n\n" + this.getCorrelationId(ex));
      }
    }

    public void LoadFiles(string[] attachmentIdList)
    {
      Tracing.Log(SkyDriveViewerControl.sw, TraceLevel.Verbose, nameof (SkyDriveViewerControl), nameof (LoadFiles));
      try
      {
        if (this.attachmentIdList != null)
        {
          bool flag = this.attachmentIdList.Length != attachmentIdList.Length;
          foreach (string attachmentId in this.attachmentIdList)
          {
            if (Array.IndexOf<string>(this.attachmentIdList, attachmentId) != Array.IndexOf<string>(attachmentIdList, attachmentId))
              flag = true;
          }
          if (!flag)
            return;
        }
        this.startTicks = DateTime.Now.Ticks;
        Tracing.Log(SkyDriveViewerControl.sw, TraceLevel.Verbose, nameof (SkyDriveViewerControl), "Showing Loading Message");
        this.showMessage("Preparing Document...");
        Tracing.Log(SkyDriveViewerControl.sw, TraceLevel.Verbose, nameof (SkyDriveViewerControl), "Setting Attachment List");
        this.attachmentIdList = attachmentIdList;
        Tracing.Log(SkyDriveViewerControl.sw, TraceLevel.Verbose, nameof (SkyDriveViewerControl), "Checking Worker");
        if (this.worker.IsBusy)
        {
          Tracing.Log(SkyDriveViewerControl.sw, TraceLevel.Verbose, nameof (SkyDriveViewerControl), "Cancelling Worker");
          this.worker.CancelAsync();
          Tracing.Log(SkyDriveViewerControl.sw, TraceLevel.Verbose, nameof (SkyDriveViewerControl), "Setting Load Attachments Flag");
          this.loadAttachments = true;
        }
        else
        {
          Tracing.Log(SkyDriveViewerControl.sw, TraceLevel.Verbose, nameof (SkyDriveViewerControl), "Starting Worker");
          this.worker.RunWorkerAsync((object) attachmentIdList);
          Tracing.Log(SkyDriveViewerControl.sw, TraceLevel.Verbose, nameof (SkyDriveViewerControl), "Clearing Load Attachments Flag");
          this.loadAttachments = false;
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(SkyDriveViewerControl.sw, TraceLevel.Error, nameof (SkyDriveViewerControl), ex.ToString());
        this.showMessage("Something went wrong trying to load the files. Please try again.\n\nIf the error persists, please contact Ellie Mae support.\n\n" + this.getCorrelationId(ex));
      }
    }

    private void showMessage(string message)
    {
      Tracing.Log(SkyDriveViewerControl.sw, TraceLevel.Verbose, nameof (SkyDriveViewerControl), "showMessage: " + message);
      this.lblMessage.Text = message;
      this.lblMessage.BringToFront();
    }

    private void worker_DoWork(object sender, DoWorkEventArgs e)
    {
      Tracing.Log(SkyDriveViewerControl.sw, TraceLevel.Verbose, nameof (SkyDriveViewerControl), "Worker Started");
      e.Cancel = true;
      Tracing.Log(SkyDriveViewerControl.sw, TraceLevel.Verbose, nameof (SkyDriveViewerControl), "Getting File List");
      string[] strArray = (string[]) e.Argument;
      try
      {
        if (strArray.Length == 1)
        {
          if (strArray[0] != null)
            this.skyDriveUrl = this.getAttachmentUrl(strArray[0]);
        }
      }
      catch (CanceledOperationException ex)
      {
        Tracing.Log(SkyDriveViewerControl.sw, TraceLevel.Verbose, nameof (SkyDriveViewerControl), "Worker Cancelled");
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
      Tracing.Log(SkyDriveViewerControl.sw, TraceLevel.Verbose, nameof (SkyDriveViewerControl), "Worker Complete");
      if (this.loadAttachments)
      {
        Tracing.Log(SkyDriveViewerControl.sw, TraceLevel.Verbose, nameof (SkyDriveViewerControl), "Starting Worker");
        this.worker.RunWorkerAsync((object) this.attachmentIdList);
        Tracing.Log(SkyDriveViewerControl.sw, TraceLevel.Verbose, nameof (SkyDriveViewerControl), "Clearing Load Files Flag");
        this.loadAttachments = false;
      }
      else if (e.Error != null)
      {
        Tracing.Log(SkyDriveViewerControl.sw, TraceLevel.Error, nameof (SkyDriveViewerControl), e.Error.ToString());
        this.showMessage("Something went wrong trying to prepare the document. Please try again.\n\nIf the error persists, please contact Ellie Mae support.\n\n" + this.getCorrelationId(e.Error));
      }
      else
      {
        if (e.Cancelled)
          return;
        this.showFile(this.skyDriveUrl);
      }
    }

    private void showFile(SkyDriveUrl skyDriveUrl)
    {
      Tracing.Log(SkyDriveViewerControl.sw, TraceLevel.Verbose, nameof (SkyDriveViewerControl), "showFile: " + skyDriveUrl.url);
      try
      {
        if (this.webHost == null)
        {
          Tracing.Log(SkyDriveViewerControl.sw, TraceLevel.Verbose, nameof (SkyDriveViewerControl), "Creating WebHost");
          this.webHost = new WebHost();
          this.webHost.FrameComplete += new WebHost.FrameCompleteEventHandler(this.WebHost_finishLoadingFrameEvent);
          this.Controls.Add((Control) this.webHost);
          this.webHost.SetBounds(0, 0, this.ClientSize.Width, this.ClientSize.Height);
        }
        Dictionary<string, object> dictionary = new Dictionary<string, object>();
        Guid guid = Guid.NewGuid();
        dictionary.Add("loanGuid", (object) guid.ToString());
        dictionary.Add("oapiBaseUrl", (object) Session.SessionObjects.StartupInfo.OAPIGatewayBaseUri);
        dictionary.Add("url", (object) skyDriveUrl.url);
        dictionary.Add("authorizationHeader", (object) skyDriveUrl.authorizationHeader);
        dictionary.Add("autoSaveUserSettingsDisabled", (object) true);
        dictionary.Add("fullscreenDisabled", (object) true);
        dictionary.Add("beforeUnloadDisabled", (object) true);
        dictionary.Add("userId", (object) ("Encompass\\" + Session.SessionObjects.StartupInfo.ServerInstanceName + "\\" + Session.DefaultInstance.UserInfo.Userid));
        dictionary.Add("forceMobileLayout", (object) true);
        if (this.attachmentIdList.Length == 1)
        {
          string attachmentId = this.attachmentIdList[0];
          dictionary.Add("attachmentId", (object) attachmentId);
          dictionary.Add("fileName", (object) "Attachment Title");
        }
        else
          dictionary.Add("fileName", (object) "MergedDocument.pdf");
        ModuleParameters parameters = new ModuleParameters()
        {
          User = this.getModuleUser(),
          Parameters = dictionary
        };
        Tracing.Log(SkyDriveViewerControl.sw, TraceLevel.Verbose, nameof (SkyDriveViewerControl), "Calling LoadModule");
        this.webHost.LoadModule(Session.SessionObjects.StartupInfo.ViewerUrl + "host.html", Session.SessionObjects.StartupInfo.ViewerUrl + "index.html", parameters, true);
      }
      catch (Exception ex)
      {
        Tracing.Log(SkyDriveViewerControl.sw, TraceLevel.Error, nameof (SkyDriveViewerControl), ex.ToString());
        this.showMessage("Something went wrong trying to show the document. Please try again.\n\nIf the error persists, please contact Ellie Mae support.\n\n" + this.getCorrelationId(ex));
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

    private void WebHost_finishLoadingFrameEvent(object sender, FinishedLoadingEventArgs e)
    {
      Tracing.Log(SkyDriveViewerControl.sw, TraceLevel.Verbose, nameof (SkyDriveViewerControl), "Finished Loading Frame: " + e.ValidatedURL);
      if (!e.IsMainFrame || this.loadAttachments)
        return;
      this.webHost.BringToFront();
      if (this.attachmentIdList == null || this.attachmentIdList.Length <= 1)
        return;
      RemoteLogger.Write(TraceLevel.Info, "Viewing CloudAttachments: " + (object) TimeSpan.FromTicks(DateTime.Now.Ticks - this.startTicks).TotalMilliseconds + " ms, " + (object) this.attachmentIdList.Length + " files");
    }

    private SkyDriveUrl getAttachmentUrl(string attachmentId)
    {
      SkyDriveUrl skyDriveUrl = Session.ConfigurationManager.GetSkyDriveUrl(attachmentId);
      Tracing.Log(SkyDriveViewerControl.sw, TraceLevel.Verbose, nameof (SkyDriveViewerControl), "Checking GetSkyDriveUrlForObject Response");
      return skyDriveUrl != null ? skyDriveUrl : throw new FileNotFoundException("Unable to generate pre-sign url for 'Attachment Title'", attachmentId);
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

    private void SkyDriveViewerControl_Resize(object sender, EventArgs e)
    {
      this.lblMessage.Size = this.ClientSize;
      if (this.webHost == null)
        return;
      this.webHost.Size = this.ClientSize;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        if (this.components != null)
          this.components.Dispose();
        this.UnsubscribeEvents();
      }
      base.Dispose(disposing);
    }

    private void UnsubscribeEvents()
    {
      if (this.webHost == null)
        return;
      this.webHost.FrameComplete -= new WebHost.FrameCompleteEventHandler(this.WebHost_finishLoadingFrameEvent);
    }

    private void InitializeComponent()
    {
      this.lblMessage = new Label();
      this.worker = new BackgroundWorker();
      this.SuspendLayout();
      this.lblMessage.BackColor = SystemColors.AppWorkspace;
      this.lblMessage.ForeColor = SystemColors.Window;
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
      this.Name = nameof (SkyDriveViewerControl);
      this.Size = new Size(809, 215);
      this.Resize += new EventHandler(this.SkyDriveViewerControl_Resize);
      this.ResumeLayout(false);
    }
  }
}
