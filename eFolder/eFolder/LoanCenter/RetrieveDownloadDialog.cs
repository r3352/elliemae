// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.LoanCenter.RetrieveDownloadDialog
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.ClientServer.Authentication;
using EllieMae.EMLite.ClientServer.eFolder;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.eFolder.Files;
using EllieMae.EMLite.ePass;
using EllieMae.EMLite.ePass.Messaging;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.WebServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web.Services.Protocols;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.LoanCenter
{
  public class RetrieveDownloadDialog : Form
  {
    private const string className = "RetrieveDownloadDialog";
    private static readonly string sw = Tracing.SwEFolder;
    private const string GETURL = "https://loancenter.elliemae.com/eFolder/GetFiles.aspx";
    private const string DOWNLOADURL = "https://loancenter.elliemae.com/eFolder/DownloadFile.aspx";
    private const string scope = "sc";
    private LoanDataMgr loanDataMgr;
    private DownloadLog download;
    private PageIdentity[] identityList;
    private string downloadFile;
    private IContainer components;
    private Button btnCancel;
    private ProgressBar pbRetrieve;
    private Label lblRetrieve;
    private BackgroundWorker worker;

    private ReauthenticateOnUnauthorised ReauthenticateOnUnauthorised { get; set; }

    public RetrieveDownloadDialog(LoanDataMgr loanDataMgr, RetrySettings settings = null)
    {
      this.InitializeComponent();
      this.loanDataMgr = loanDataMgr;
      if (settings == null)
        settings = new RetrySettings(loanDataMgr.SessionObjects);
      this.ReauthenticateOnUnauthorised = new ReauthenticateOnUnauthorised(loanDataMgr.SessionObjects.StartupInfo.ServerInstanceName, loanDataMgr.SessionObjects.StartupInfo.SessionID, loanDataMgr.SessionObjects.StartupInfo.OAPIGatewayBaseUri, settings);
    }

    public static DownloadLog[] GetAvailableDownloads(
      LoanDataMgr loanDataMgr,
      RetrySettings settings = null)
    {
      if (settings == null)
        settings = new RetrySettings(loanDataMgr.SessionObjects);
      ReauthenticateOnUnauthorised reauthenticateOnUnauthorised = new ReauthenticateOnUnauthorised(loanDataMgr.SessionObjects.StartupInfo.ServerInstanceName, loanDataMgr.SessionObjects.StartupInfo.SessionID, loanDataMgr.SessionObjects.StartupInfo.OAPIGatewayBaseUri, settings);
      List<DownloadLog> downloadList = new List<DownloadLog>();
      try
      {
        string tokenType = string.Empty;
        string accessToken = string.Empty;
        reauthenticateOnUnauthorised.Execute("sc", (Action<AccessToken>) (AccessToken =>
        {
          if (loanDataMgr.IsPlatformLoan() && AccessToken != null)
          {
            tokenType = AccessToken.Type;
            accessToken = AccessToken.Token;
          }
          using (InboxServiceWse inboxServiceWse = new InboxServiceWse(tokenType, accessToken, Session.SessionObjects?.StartupInfo?.ServiceUrls?.InboxServiceUrl))
          {
            foreach (InboxFile file in inboxServiceWse.GetFiles(Session.CompanyInfo.ClientID, loanDataMgr.LoanData.GUID.Replace("{", string.Empty).Replace("}", string.Empty)))
              downloadList.Add(new DownloadLog()
              {
                FileSource = file.FileSource,
                FileID = file.FileID,
                FileType = file.FileType,
                Title = file.Title,
                Sender = file.Sender,
                DocumentID = file.DocumentID,
                BarcodePage = file.BarcodePage,
                Date = file.ReceivedDate.ToLocalTime()
              });
          }
        }));
      }
      catch (WebException ex)
      {
        Tracing.Log(RetrieveDownloadDialog.sw, TraceLevel.Error, nameof (RetrieveDownloadDialog), ex.ToString());
        throw;
      }
      catch (SoapException ex)
      {
        Tracing.Log(RetrieveDownloadDialog.sw, TraceLevel.Error, nameof (RetrieveDownloadDialog), ex.ToString());
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "There was an issue processing your request. Please try again shortly. Contact your administrator if the issue continues.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      catch (TaskCanceledException ex)
      {
        Tracing.Log(RetrieveDownloadDialog.sw, TraceLevel.Error, nameof (RetrieveDownloadDialog), ex.ToString());
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "There was an issue processing your request. Please try again shortly. Contact your administrator if the issue continues.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      catch (Exception ex)
      {
        Tracing.Log(RetrieveDownloadDialog.sw, TraceLevel.Error, nameof (RetrieveDownloadDialog), ex.ToString());
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "The following error occurred when trying to get the list of downloads:\n\n" + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      return downloadList.ToArray();
    }

    public FileAttachment[] Retrieve(DownloadLog download)
    {
      if (!Modules.IsModuleAvailableForUser(EncompassModule.EDM, true))
        return (FileAttachment[]) null;
      if (!new eFolderAccessRights(this.loanDataMgr).CanRetrieveDocuments)
      {
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "You do not have rights to retrieve documents from borrower.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return (FileAttachment[]) null;
      }
      Tracing.Log(RetrieveDownloadDialog.sw, TraceLevel.Verbose, nameof (RetrieveDownloadDialog), "Retrieve: " + download.Title);
      this.download = download;
      this.identityList = (PageIdentity[]) null;
      this.downloadFile = (string) null;
      Tracing.Log(RetrieveDownloadDialog.sw, TraceLevel.Verbose, nameof (RetrieveDownloadDialog), "Starting Thread");
      this.worker.RunWorkerAsync();
      Tracing.Log(RetrieveDownloadDialog.sw, TraceLevel.Verbose, nameof (RetrieveDownloadDialog), "Showing Dialog");
      if (this.ShowDialog((IWin32Window) Form.ActiveForm) != DialogResult.OK)
        return (FileAttachment[]) null;
      Tracing.Log(RetrieveDownloadDialog.sw, TraceLevel.Verbose, nameof (RetrieveDownloadDialog), "Logging Transaction");
      if (!Transaction.Log(this.loanDataMgr, "Download"))
        return (FileAttachment[]) null;
      LogList logList = this.loanDataMgr.LoanData.GetLogList();
      download.MarkAsReceived(DateTime.Now, Session.UserID);
      logList.AddRecord((LogRecordBase) download);
      DocumentLog doc = (DocumentLog) null;
      if (!string.IsNullOrEmpty(download.DocumentID))
      {
        LogRecordBase recordById = logList.GetRecordByID(download.DocumentID, false);
        if (recordById is DocumentLog)
          doc = (DocumentLog) recordById;
      }
      AddReasonType reason = AddReasonType.Retrieve;
      switch (download.FileSource.ToLower())
      {
        case "edelivery":
        case "esign":
          reason = AddReasonType.Esign;
          break;
        case "fax":
          reason = AddReasonType.Fax;
          break;
        case "scandoc":
          reason = AddReasonType.ScanDoc;
          break;
        case "upload":
          reason = AddReasonType.Upload;
          break;
      }
      Tracing.Log(RetrieveDownloadDialog.sw, TraceLevel.Verbose, nameof (RetrieveDownloadDialog), "Attaching File");
      using (AttachFilesDialog attachFilesDialog = new AttachFilesDialog(this.loanDataMgr))
        return attachFilesDialog.AttachFile(reason, this.downloadFile, download.Title, this.identityList, doc) ?? (FileAttachment[]) null;
    }

    private string removeCoversheet(string imageFile, int barcodePage)
    {
      Tracing.Log(RetrieveDownloadDialog.sw, TraceLevel.Verbose, nameof (RetrieveDownloadDialog), "Loading Image: " + imageFile);
      using (Image image = Image.FromFile(imageFile))
      {
        int frameCount = image.GetFrameCount(FrameDimension.Page);
        Tracing.Log(RetrieveDownloadDialog.sw, TraceLevel.Verbose, nameof (RetrieveDownloadDialog), "GetFrameCount: " + (object) frameCount);
        if (frameCount <= barcodePage)
          return imageFile;
        Bitmap bitmap = (Bitmap) null;
        try
        {
          string filename = (string) null;
          for (int frameIndex = 0; frameIndex < frameCount; ++frameIndex)
          {
            if (frameIndex != barcodePage)
            {
              Tracing.Log(RetrieveDownloadDialog.sw, TraceLevel.Verbose, nameof (RetrieveDownloadDialog), "Selecting Page: " + (object) frameIndex);
              image.SelectActiveFrame(FrameDimension.Page, frameIndex);
              EncoderParameters encoderParams = new EncoderParameters(1);
              if (bitmap == null)
              {
                bitmap = new Bitmap(image);
                bitmap.SetResolution(image.HorizontalResolution, image.VerticalResolution);
                ImageCodecInfo encoder = (ImageCodecInfo) null;
                foreach (ImageCodecInfo imageEncoder in ImageCodecInfo.GetImageEncoders())
                {
                  if (imageEncoder.MimeType == "image/tiff")
                    encoder = imageEncoder;
                }
                filename = SystemSettings.GetTempFileNameWithExtension("tif");
                Tracing.Log(RetrieveDownloadDialog.sw, TraceLevel.Verbose, nameof (RetrieveDownloadDialog), "Calling Bitmap.Save: " + filename);
                encoderParams.Param[0] = new EncoderParameter(Encoder.SaveFlag, 18L);
                bitmap.Save(filename, encoder, encoderParams);
              }
              else
              {
                Tracing.Log(RetrieveDownloadDialog.sw, TraceLevel.Verbose, nameof (RetrieveDownloadDialog), "Calling Bitmap.SaveAdd: " + filename);
                encoderParams.Param[0] = new EncoderParameter(Encoder.SaveFlag, 23L);
                bitmap.SaveAdd(image, encoderParams);
              }
            }
          }
          if (this.identityList != null)
          {
            List<PageIdentity> pageIdentityList = new List<PageIdentity>();
            foreach (PageIdentity identity in this.identityList)
            {
              if (identity.PageIndex != barcodePage)
              {
                if (identity.PageIndex > barcodePage)
                  --identity.PageIndex;
                pageIdentityList.Add(identity);
              }
            }
            this.identityList = pageIdentityList.ToArray();
          }
          return filename;
        }
        finally
        {
          bitmap?.Dispose();
        }
      }
    }

    private void rebuildIdentities(string imageFile)
    {
      if (this.identityList == null)
        return;
      List<PageIdentity> pageIdentityList = new List<PageIdentity>();
      try
      {
        using (Bitmap bitmap = new Bitmap(imageFile))
        {
          int frameCount = bitmap.GetFrameCount(FrameDimension.Page);
          for (int index = 0; index < frameCount; ++index)
          {
            PageIdentity pageIdentity = (PageIdentity) null;
            foreach (PageIdentity identity in this.identityList)
            {
              if (identity.PageIndex == index)
                pageIdentity = identity;
            }
            if (pageIdentity == null)
            {
              pageIdentity = new PageIdentity();
              pageIdentity.PageIndex = index;
              pageIdentity.DocumentType = "Unknown";
              pageIdentity.DocumentSource = "Unknown";
              pageIdentity.Title = "Unknown";
            }
            pageIdentityList.Add(pageIdentity);
          }
        }
        this.identityList = pageIdentityList.ToArray();
      }
      catch (Exception ex)
      {
        Tracing.Log(RetrieveDownloadDialog.sw, TraceLevel.Error, nameof (RetrieveDownloadDialog), ex.ToString());
        this.identityList = (PageIdentity[]) null;
      }
    }

    private void worker_DoWork(object sender, DoWorkEventArgs e)
    {
      Tracing.Log(RetrieveDownloadDialog.sw, TraceLevel.Verbose, nameof (RetrieveDownloadDialog), "Thread Started");
      try
      {
        string tokenType = string.Empty;
        string accessToken = string.Empty;
        this.ReauthenticateOnUnauthorised.Execute("sc", (Action<AccessToken>) (AccessToken =>
        {
          if (this.loanDataMgr.IsPlatformLoan() && AccessToken != null)
          {
            tokenType = AccessToken.Type;
            accessToken = AccessToken.Token;
          }
          using (InboxServiceWse inboxServiceWse = new InboxServiceWse(tokenType, accessToken, Session.SessionObjects?.StartupInfo?.ServiceUrls?.InboxServiceUrl))
          {
            inboxServiceWse.InboxCredentialsValue = new InboxCredentials();
            inboxServiceWse.InboxCredentialsValue.ClientID = Session.CompanyInfo.ClientID;
            inboxServiceWse.InboxCredentialsValue.UserID = Session.UserID;
            inboxServiceWse.InboxCredentialsValue.Password = EpassLogin.LoginPassword;
            inboxServiceWse.ChunkReceived += new ChunkHandler(this.chunkReceived);
            try
            {
              InboxFile file = new InboxFile();
              file.ClientID = Session.CompanyInfo.ClientID;
              file.LoanID = this.loanDataMgr.LoanData.GUID.Replace("{", string.Empty).Replace("}", string.Empty);
              file.FileSource = this.download.FileSource;
              file.FileID = this.download.FileID;
              file.FileType = this.download.FileType;
              file.Title = this.download.Title;
              file.Sender = this.download.Sender;
              file.DocumentID = this.download.DocumentID;
              file.BarcodePage = this.download.BarcodePage;
              file.ReceivedDate = this.download.Date.ToUniversalTime();
              this.identityList = inboxServiceWse.DownloadFile(file);
              string nameWithExtension = SystemSettings.GetTempFileNameWithExtension(file.FileType);
              using (Stream stream = inboxServiceWse.ResponseSoapContext.Attachments[0].Stream)
              {
                using (BinaryReader binaryReader = new BinaryReader(stream))
                {
                  int int32 = Convert.ToInt32(stream.Length);
                  byte[] bytes = binaryReader.ReadBytes(int32);
                  binaryReader.Close();
                  System.IO.File.WriteAllBytes(nameWithExtension, bytes);
                }
              }
              this.downloadFile = this.download.BarcodePage < 0 ? nameWithExtension : this.removeCoversheet(nameWithExtension, this.download.BarcodePage);
              this.rebuildIdentities(this.downloadFile);
              Tracing.Log(RetrieveDownloadDialog.sw, TraceLevel.Verbose, nameof (RetrieveDownloadDialog), "Synchronizing Messages");
              EPassMessages.SyncReadMessages(true);
            }
            catch (WebException ex)
            {
              if (ex.Status == WebExceptionStatus.RequestCanceled)
                Tracing.Log(RetrieveDownloadDialog.sw, TraceLevel.Verbose, nameof (RetrieveDownloadDialog), "Transfer Cancelled");
              else
                throw;
            }
            finally
            {
              inboxServiceWse.ChunkReceived -= new ChunkHandler(this.chunkReceived);
            }
          }
        }));
      }
      catch (Exception ex)
      {
        Tracing.Log(RetrieveDownloadDialog.sw, TraceLevel.Error, nameof (RetrieveDownloadDialog), ex.Message);
      }
    }

    private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
      this.pbRetrieve.Value = e.ProgressPercentage;
    }

    private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      if (e.Error != null)
      {
        Tracing.Log(RetrieveDownloadDialog.sw, TraceLevel.Error, nameof (RetrieveDownloadDialog), e.Error.ToString());
        int num = (int) Utils.Dialog((IWin32Window) this, "The following error occurred when trying to download the file:\n\n" + e.Error.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      Tracing.Log(RetrieveDownloadDialog.sw, TraceLevel.Verbose, nameof (RetrieveDownloadDialog), "Closing Window");
      if (this.downloadFile != null)
        this.DialogResult = DialogResult.OK;
      else
        this.DialogResult = DialogResult.Cancel;
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      Tracing.Log(RetrieveDownloadDialog.sw, TraceLevel.Verbose, nameof (RetrieveDownloadDialog), "Cancelling");
      this.worker.CancelAsync();
    }

    private void chunkReceived(object sender, ChunkHandlerEventArgs e)
    {
      if (this.worker.CancellationPending)
        e.Abort = true;
      this.worker.ReportProgress(e.Percent);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.btnCancel = new Button();
      this.pbRetrieve = new ProgressBar();
      this.lblRetrieve = new Label();
      this.worker = new BackgroundWorker();
      this.SuspendLayout();
      this.btnCancel.Location = new Point(282, 58);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 2;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.pbRetrieve.Location = new Point(12, 32);
      this.pbRetrieve.Name = "pbRetrieve";
      this.pbRetrieve.Size = new Size(345, 16);
      this.pbRetrieve.TabIndex = 1;
      this.lblRetrieve.AutoSize = true;
      this.lblRetrieve.Location = new Point(12, 12);
      this.lblRetrieve.Name = "lblRetrieve";
      this.lblRetrieve.Size = new Size(120, 14);
      this.lblRetrieve.TabIndex = 0;
      this.lblRetrieve.Text = "Retrieving documents...";
      this.worker.WorkerReportsProgress = true;
      this.worker.WorkerSupportsCancellation = true;
      this.worker.DoWork += new DoWorkEventHandler(this.worker_DoWork);
      this.worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.worker_RunWorkerCompleted);
      this.worker.ProgressChanged += new ProgressChangedEventHandler(this.worker_ProgressChanged);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(367, 90);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.pbRetrieve);
      this.Controls.Add((Control) this.lblRetrieve);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (RetrieveDownloadDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Loan Center";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
