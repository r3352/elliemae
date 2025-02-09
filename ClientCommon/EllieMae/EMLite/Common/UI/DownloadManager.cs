// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.DownloadManager
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class DownloadManager : Form
  {
    private string sw = Tracing.SwVersionControl;
    private const string className = "DownloadManager";
    private Label lblTimeRemaining;
    private Button btnCancel;
    private Label label2;
    private ProgressBar pbDownload;
    private Label label3;
    private Label lblDownloadStatus;
    private Label Label1;
    private System.ComponentModel.Container components;
    private const int MAXBUFFERSIZE = 50000;
    private Uri sourceUri;
    private Stream sourceStream;
    private string targetPath;
    private long bytesToDownload;
    private DateTime startTime;
    private bool cancelDownload;
    private DownloadResult downloadResult;
    private DownloadManager.ProgressUpdateDelegate updateProgressDlgt;
    private DownloadManager.ProcessNotificationDelegate processNotificationDlgt;
    private object cancelSemaphore = new object();

    public DownloadManager()
      : this((string) null)
    {
    }

    public DownloadManager(string title)
    {
      this.InitializeComponent();
      if (title != null)
        this.Text = title;
      this.updateProgressDlgt = new DownloadManager.ProgressUpdateDelegate(this.updateProgress);
      this.processNotificationDlgt = new DownloadManager.ProcessNotificationDelegate(this.notifyProcessComplete);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.pbDownload = new ProgressBar();
      this.btnCancel = new Button();
      this.Label1 = new Label();
      this.lblTimeRemaining = new Label();
      this.label2 = new Label();
      this.label3 = new Label();
      this.lblDownloadStatus = new Label();
      this.SuspendLayout();
      this.pbDownload.Location = new Point(16, 40);
      this.pbDownload.Name = "pbDownload";
      this.pbDownload.Size = new Size(328, 16);
      this.pbDownload.TabIndex = 0;
      this.btnCancel.Location = new Point(272, 104);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 1;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.Label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.Label1.Location = new Point(16, 79);
      this.Label1.Name = "Label1";
      this.Label1.Size = new Size(96, 16);
      this.Label1.TabIndex = 2;
      this.Label1.Text = "Time Remaining:";
      this.lblTimeRemaining.Location = new Point(125, 79);
      this.lblTimeRemaining.Name = "lblTimeRemaining";
      this.lblTimeRemaining.Size = new Size(219, 16);
      this.lblTimeRemaining.TabIndex = 3;
      this.lblTimeRemaining.Text = "Calculating...";
      this.label2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(16, 16);
      this.label2.Name = "label2";
      this.label2.Size = new Size(232, 16);
      this.label2.TabIndex = 4;
      this.label2.Text = "Downloading update...";
      this.label3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label3.Location = new Point(16, 60);
      this.label3.Name = "label3";
      this.label3.Size = new Size(104, 16);
      this.label3.TabIndex = 5;
      this.label3.Text = "Download Status:";
      this.lblDownloadStatus.Location = new Point(125, 60);
      this.lblDownloadStatus.Name = "lblDownloadStatus";
      this.lblDownloadStatus.Size = new Size(219, 16);
      this.lblDownloadStatus.TabIndex = 6;
      this.lblDownloadStatus.Text = "Preparing for Download...";
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(360, 133);
      this.ControlBox = false;
      this.Controls.Add((Control) this.lblDownloadStatus);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.lblTimeRemaining);
      this.Controls.Add((Control) this.Label1);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.pbDownload);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Name = nameof (DownloadManager);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Encompass Download Manager";
      this.ResumeLayout(false);
    }

    public DownloadResult BeginDownload(Uri sourceUri, string targetPath)
    {
      this.Log(TraceLevel.Info, "Beginning download from from URI \"" + sourceUri.ToString() + "\"");
      this.sourceUri = sourceUri;
      this.sourceStream = (Stream) null;
      this.targetPath = targetPath;
      this.startTime = DateTime.MinValue;
      this.bytesToDownload = -1L;
      this.cancelDownload = false;
      this.downloadResult = new DownloadResult(DownloadStatus.InProgress);
      new Thread(new ThreadStart(this.executeDownload)).Start();
      return this.downloadResult;
    }

    public DownloadResult BeginDownload(Stream sourceStream, string targetPath)
    {
      this.Log(TraceLevel.Info, "Beginning download from from remote stream");
      this.sourceUri = (Uri) null;
      this.sourceStream = sourceStream;
      this.targetPath = targetPath;
      this.startTime = DateTime.MinValue;
      this.bytesToDownload = sourceStream.Length;
      this.cancelDownload = false;
      this.downloadResult = new DownloadResult(DownloadStatus.InProgress);
      new Thread(new ThreadStart(this.executeDownload)).Start();
      return this.downloadResult;
    }

    private void executeDownload()
    {
      DownloadStatus downloadStatus;
      try
      {
        downloadStatus = !(this.sourceUri != (Uri) null) ? (this.sourceStream == null ? DownloadStatus.Failed : this.retrieveRemoteStream(this.sourceStream)) : this.retrieveFileFromWeb();
      }
      catch (Exception ex)
      {
        downloadStatus = DownloadStatus.Failed;
        this.Log(TraceLevel.Error, "Unexpected error during download: " + (object) ex);
        ErrorDialog.Display("An unexpected error occurred while retrieving the update file from the Internet.\n\nPlease ensure you are connected to the Internet and then restart the update process.", ex);
      }
      this.BeginInvoke((Delegate) this.processNotificationDlgt, (object) downloadStatus);
    }

    private DownloadStatus retrieveFileFromWeb()
    {
      this.updateProgressSafe("Connecting to Server...", -1L);
      using (HttpWebResponse httpWebResponse = this.openWebConnection(this.sourceUri))
      {
        if (httpWebResponse == null)
          return DownloadStatus.Failed;
        this.bytesToDownload = httpWebResponse.ContentLength;
        using (Stream responseStream = httpWebResponse.GetResponseStream())
          return this.retrieveRemoteStream(responseStream);
      }
    }

    private DownloadStatus retrieveRemoteStream(Stream remoteStream)
    {
      long num = 0;
      this.Log(TraceLevel.Info, "Downloading " + (object) this.bytesToDownload + " bytes from remote location to \"" + this.targetPath + "\"");
      try
      {
        using (FileStream fileStream = new FileStream(this.targetPath, FileMode.Create, FileAccess.Write, FileShare.None))
        {
          this.startTime = DateTime.Now;
          this.updateProgressSafe("Receiving Data...", 0L);
          byte[] buffer = new byte[50000];
          int count;
          while ((count = remoteStream.Read(buffer, 0, 50000)) > 0)
          {
            fileStream.Write(buffer, 0, count);
            num += (long) count;
            lock (this)
            {
              if (this.cancelDownload)
              {
                this.Log(TraceLevel.Info, "Transfer cancelled by user");
                break;
              }
            }
            this.updateProgressSafe("Receiving Data... (" + this.formatByteCount(num) + " received)", num);
          }
        }
      }
      catch (WebException ex)
      {
        this.Log(TraceLevel.Error, "Connection error while transferring data from remote host: " + (object) ex);
        ErrorDialog.Display("An error has occurred while communicating with the remote server. Please verify your Internet connection and then restart this download.");
        System.IO.File.Delete(this.targetPath);
        return DownloadStatus.Failed;
      }
      lock (this)
      {
        if (this.cancelDownload)
        {
          this.Log(TraceLevel.Info, "Transfer cancelled by user");
          System.IO.File.Delete(this.targetPath);
          return DownloadStatus.Cancelled;
        }
      }
      if (num < this.bytesToDownload)
      {
        this.Log(TraceLevel.Error, "Number of bytes received does not match number expected. Download failed.");
        ErrorDialog.Display("The file transfer was interrupted before completion and must be restarted. Please verify your network connection and then restart the update process.");
        System.IO.File.Delete(this.targetPath);
        return DownloadStatus.Failed;
      }
      this.Log(TraceLevel.Info, "Downloaded complete. " + (object) num + " bytes received.");
      return DownloadStatus.Complete;
    }

    private HttpWebResponse openWebConnection(Uri url)
    {
label_0:
      try
      {
        this.Log(TraceLevel.Info, "Attempting to open HTTP connection to remote location \"" + (object) url + "\"");
        HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(url);
        httpWebRequest.KeepAlive = false;
        httpWebRequest.Method = "GET";
        httpWebRequest.Proxy = WebRequest.DefaultWebProxy;
        httpWebRequest.Proxy.Credentials = CredentialCache.DefaultCredentials;
        return (HttpWebResponse) httpWebRequest.GetResponse();
      }
      catch (WebException ex)
      {
        if (ex.Response == null)
        {
          this.Log(TraceLevel.Warning, "Connection failed to remote host: " + (object) ex);
          if (Utils.Dialog((IWin32Window) this, "Unable to connect to ICE Mortgage Technology web site. Please ensure you are connected to the Internet and try again.", MessageBoxButtons.RetryCancel, MessageBoxIcon.Asterisk) == DialogResult.Cancel)
          {
            this.Log(TraceLevel.Info, "User abort while attempting to connect to remote host.");
            return (HttpWebResponse) null;
          }
          goto label_0;
        }
        else
        {
          this.Log(TraceLevel.Error, "Response error from remote host: " + (object) ex);
          int num = (int) Utils.Dialog((IWin32Window) this, "The specified update could not be downloaded from the ICE Mortgage Technology servers. Please try again later.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          return (HttpWebResponse) null;
        }
      }
    }

    private void notifyProcessComplete(DownloadStatus status)
    {
      lock (this.cancelSemaphore)
      {
        if (this.cancelDownload)
          this.downloadResult.NotifyComplete(DownloadStatus.Cancelled);
        else
          this.downloadResult.NotifyComplete(status);
        this.Close();
      }
    }

    private void updateProgressSafe(string status, long bytesReceived)
    {
      try
      {
        this.BeginInvoke((Delegate) this.updateProgressDlgt, (object) status, (object) bytesReceived);
      }
      catch
      {
      }
    }

    private void updateProgress(string status, long bytesReceived)
    {
      this.lblDownloadStatus.Text = status;
      if (this.cancelDownload)
      {
        this.lblTimeRemaining.Text = "";
        this.pbDownload.Value = 0;
      }
      else if (this.startTime == DateTime.MinValue || this.bytesToDownload < 0L)
      {
        this.lblTimeRemaining.Text = "Calculating...";
        this.pbDownload.Value = 0;
      }
      else
      {
        if (this.bytesToDownload <= 0L)
          return;
        int num1 = (int) (bytesReceived * 100L / this.bytesToDownload);
        long totalMilliseconds = (long) (DateTime.Now - this.startTime).TotalMilliseconds;
        long num2 = (long) ((double) (this.bytesToDownload - bytesReceived) * ((double) totalMilliseconds / (double) bytesReceived));
        if (totalMilliseconds <= 0L || num2 >= (long) int.MaxValue)
          return;
        TimeSpan timeSpan = new TimeSpan(0, 0, (int) ((double) num2 / 1000.0) + 1);
        if (timeSpan.TotalMinutes > 60.0)
          this.lblTimeRemaining.Text = this.formatQuantity((int) Math.Ceiling(timeSpan.TotalMinutes / 60.0), "hour");
        else if (timeSpan.TotalSeconds > 60.0)
          this.lblTimeRemaining.Text = this.formatQuantity((int) Math.Ceiling(timeSpan.TotalSeconds / 60.0), "minute");
        else
          this.lblTimeRemaining.Text = this.formatQuantity(timeSpan.Seconds, "second");
        this.pbDownload.Value = num1;
      }
    }

    private string formatQuantity(int quantity, string caption)
    {
      if (quantity == 1)
        return quantity.ToString() + " " + caption;
      return quantity.ToString() + " " + caption + "s";
    }

    private string formatByteCount(long byteCount)
    {
      long num1 = 1000;
      long num2 = 1000000;
      if (byteCount < 10L * num1)
        return byteCount.ToString() + " Bytes";
      return byteCount < num2 ? byteCount.ToString("#0,.0") + " KB" : byteCount.ToString("#0,,.0") + " MB";
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      new Thread(new ThreadStart(this.showCancelDialog)).Start();
    }

    private void showCancelDialog()
    {
      lock (this.cancelSemaphore)
      {
        if (Utils.Dialog((IWin32Window) this, "Are you sure you want to cancel the Encompass update setup?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) != DialogResult.Yes)
          return;
        lock (this)
        {
          this.cancelDownload = true;
          this.updateProgressSafe("Stopping download...", -1L);
        }
      }
    }

    private void Log(TraceLevel l, string msg)
    {
      Tracing.Log(this.sw, l, nameof (DownloadManager), msg);
    }

    private delegate void ProgressUpdateDelegate(string statusText, long bytesReceived);

    private delegate void ProcessNotificationDelegate(DownloadStatus status);
  }
}
