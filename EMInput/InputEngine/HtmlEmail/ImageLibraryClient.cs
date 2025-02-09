// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.HtmlEmail.ImageLibraryClient
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.WebServices;
using Microsoft.Web.Services2.Attachments;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Web.Services.Protocols;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine.HtmlEmail
{
  public class ImageLibraryClient : Form
  {
    private const string className = "ImageLibraryClient";
    private static readonly string sw = Tracing.SwEFolder;
    private ImageLibraryMessage uploadMessage;
    private ImageInfo uploadedImageInfo;
    private string uploadFilename = string.Empty;
    private string uploadFilepath = string.Empty;
    private ResourceAccessType uploadAccessType;
    private IContainer components;
    private Label lblProgress;
    private ProgressBar pbTransfer;
    private Button btnCancel;
    private BackgroundWorker worker;

    public ImageLibraryClient() => this.InitializeComponent();

    public bool GetAllImageList(
      List<ImageInfo> companyImageInfo,
      List<ImageInfo> userImageInfo,
      Sessions.Session session)
    {
      try
      {
        ImageLibraryMessage imageLibraryMessage = new ImageLibraryMessage(ResourceAccessType.All, ResourceActionType.GetList, string.Empty, session);
        string xml = imageLibraryMessage.ToXml();
        using (ImageLibraryWse imageLibraryWse = new ImageLibraryWse(session?.SessionObjects?.StartupInfo?.ServiceUrls?.ImageLibraryServiceUrl))
        {
          string imageList = imageLibraryWse.GetImageList(xml);
          imageLibraryMessage.FromXml(imageList, companyImageInfo, userImageInfo);
        }
        return true;
      }
      catch (SoapException ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The following error occurred when trying to get the image list:\n\n" + ex.Detail.InnerText, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The following error occurred when trying to get the image list:\n\n" + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      return false;
    }

    public bool DeleteImage(
      ResourceAccessType accessType,
      string filename,
      Sessions.Session session)
    {
      try
      {
        string xml = new ImageLibraryMessage(accessType, ResourceActionType.Delete, filename, session).ToXml();
        using (ImageLibraryWse imageLibraryWse = new ImageLibraryWse(session?.SessionObjects?.StartupInfo?.ServiceUrls?.ImageLibraryServiceUrl))
          imageLibraryWse.DeleteImage(xml);
        return true;
      }
      catch (SoapException ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The following error occurred when trying to delete the image:\n\n" + ex.Detail.InnerText, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The following error occurred when trying to delete the image:\n\n" + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      return false;
    }

    public ImageInfo UploadImage(
      ResourceAccessType accessType,
      string[] names,
      Sessions.Session session)
    {
      using (OpenFileDialog openFileDialog = new OpenFileDialog())
      {
        openFileDialog.Title = "Browse and Attach Images";
        openFileDialog.CheckFileExists = true;
        openFileDialog.Multiselect = false;
        openFileDialog.ShowReadOnly = false;
        openFileDialog.Filter = "All Supported Formats|*.tif;*.jpg;*.jpeg;*.jpe;*.gif;|TIFF Images (*.tif)|*.tif|JPEG Images (*.jpg, *.jpeg, *.jpe)|*.jpg;*.jpeg;*.jpe|GIF Files (*.gif)|*.gif";
        if (openFileDialog.ShowDialog((IWin32Window) Form.ActiveForm) == DialogResult.OK)
          return this.upload(accessType, openFileDialog.SafeFileName, openFileDialog.FileName, names, session);
      }
      return (ImageInfo) null;
    }

    private ImageInfo upload(
      ResourceAccessType accessType,
      string filename,
      string filepath,
      string[] names,
      Sessions.Session session)
    {
      string[] array = new string[5]
      {
        ".tif",
        ".jpg",
        ".jpeg",
        ".jpe",
        ".gif"
      };
      string str = Path.GetExtension(filepath).ToLower().Trim();
      if (Array.IndexOf<string>(array, str) < 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The '" + str + "' file type is not supported.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return (ImageInfo) null;
      }
      if (Array.IndexOf<string>(names, filename.ToLower().Trim()) >= 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, filename.ToLower().Trim() + " already exists in the " + accessType.ToString() + " image list", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return (ImageInfo) null;
      }
      Tracing.Log(ImageLibraryClient.sw, TraceLevel.Verbose, nameof (ImageLibraryClient), "Initializing Request");
      this.uploadFilename = filename;
      this.uploadFilepath = filepath;
      this.uploadAccessType = accessType;
      this.uploadMessage = new ImageLibraryMessage(this.uploadAccessType, ResourceActionType.Upload, filename, session);
      this.uploadedImageInfo = (ImageInfo) null;
      Tracing.Log(ImageLibraryClient.sw, TraceLevel.Verbose, nameof (ImageLibraryClient), "Starting Thread");
      this.worker.RunWorkerAsync();
      Tracing.Log(ImageLibraryClient.sw, TraceLevel.Verbose, nameof (ImageLibraryClient), "Showing Dialog");
      return this.ShowDialog((IWin32Window) Form.ActiveForm) != DialogResult.OK ? (ImageInfo) null : this.uploadedImageInfo;
    }

    private void worker_DoWork(object sender, DoWorkEventArgs e)
    {
      Tracing.Log(ImageLibraryClient.sw, TraceLevel.Verbose, nameof (ImageLibraryClient), "Thread Started");
      try
      {
        string xml = this.uploadMessage.ToXml();
        using (ImageLibraryWse imageLibraryWse = new ImageLibraryWse(Session.SessionObjects?.StartupInfo?.ServiceUrls?.ImageLibraryServiceUrl))
        {
          imageLibraryWse.ChunkSent += new ChunkHandler(this.chunkSent);
          try
          {
            if (this.worker.CancellationPending)
              return;
            Tracing.Log(ImageLibraryClient.sw, TraceLevel.Verbose, nameof (ImageLibraryClient), "Clearing Attachments");
            imageLibraryWse.RequestSoapContext.Attachments.Clear();
            Tracing.Log(ImageLibraryClient.sw, TraceLevel.Verbose, nameof (ImageLibraryClient), "Adding Attachment");
            ImageLibraryAttachment libraryAttachment = new ImageLibraryAttachment(this.uploadFilepath, this.uploadFilename);
            imageLibraryWse.RequestSoapContext.Attachments.Add((Attachment) libraryAttachment.DimeAttachment);
            if (this.worker.CancellationPending)
              return;
            Tracing.Log(ImageLibraryClient.sw, TraceLevel.Verbose, nameof (ImageLibraryClient), "UploadImage");
            string xmlResponse = imageLibraryWse.UploadImage(xml);
            Tracing.Log(ImageLibraryClient.sw, TraceLevel.Verbose, nameof (ImageLibraryClient), "Clearing Attachments");
            imageLibraryWse.RequestSoapContext.Attachments.Clear();
            List<ImageInfo> imageInfoList = new List<ImageInfo>();
            if (this.uploadAccessType == ResourceAccessType.Company)
              this.uploadMessage.FromXml(xmlResponse, imageInfoList, (List<ImageInfo>) null);
            else if (this.uploadAccessType == ResourceAccessType.User)
              this.uploadMessage.FromXml(xmlResponse, (List<ImageInfo>) null, imageInfoList);
            this.uploadedImageInfo = imageInfoList[0];
            if (this.uploadedImageInfo == null)
              throw new Exception("No upload response");
          }
          finally
          {
            imageLibraryWse.ChunkSent -= new ChunkHandler(this.chunkSent);
          }
        }
      }
      catch (WebException ex)
      {
        if (ex.Status == WebExceptionStatus.RequestCanceled)
          Tracing.Log(ImageLibraryClient.sw, TraceLevel.Verbose, nameof (ImageLibraryClient), "Transfer Cancelled");
        else
          throw;
      }
    }

    private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
      ((ProgressBar) e.UserState).Value = e.ProgressPercentage;
    }

    private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      if (e.Error != null)
      {
        Tracing.Log(ImageLibraryClient.sw, TraceLevel.Error, nameof (ImageLibraryClient), e.Error.ToString());
        int num = (int) Utils.Dialog((IWin32Window) this, "The following error occurred when trying to upload the image:\n\n" + e.Error.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      Tracing.Log(ImageLibraryClient.sw, TraceLevel.Verbose, nameof (ImageLibraryClient), "Closing Window");
      if (this.uploadedImageInfo != null)
        this.DialogResult = DialogResult.OK;
      else
        this.DialogResult = DialogResult.Cancel;
    }

    private void chunkSent(object sender, ChunkHandlerEventArgs e)
    {
      if (this.worker.CancellationPending)
        e.Abort = true;
      this.worker.ReportProgress(e.Percent, (object) this.pbTransfer);
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      Tracing.Log(ImageLibraryClient.sw, TraceLevel.Verbose, nameof (ImageLibraryClient), "Cancelling");
      this.worker.CancelAsync();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.lblProgress = new Label();
      this.pbTransfer = new ProgressBar();
      this.btnCancel = new Button();
      this.worker = new BackgroundWorker();
      this.SuspendLayout();
      this.lblProgress.AutoSize = true;
      this.lblProgress.Location = new Point(11, 9);
      this.lblProgress.Name = "lblProgress";
      this.lblProgress.Size = new Size(105, 14);
      this.lblProgress.TabIndex = 0;
      this.lblProgress.Text = "Transfer Progress...";
      this.pbTransfer.Location = new Point(12, 26);
      this.pbTransfer.Name = "pbTransfer";
      this.pbTransfer.Size = new Size(418, 23);
      this.pbTransfer.TabIndex = 1;
      this.btnCancel.Location = new Point(355, 58);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 2;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.worker.WorkerReportsProgress = true;
      this.worker.WorkerSupportsCancellation = true;
      this.worker.DoWork += new DoWorkEventHandler(this.worker_DoWork);
      this.worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.worker_RunWorkerCompleted);
      this.worker.ProgressChanged += new ProgressChangedEventHandler(this.worker_ProgressChanged);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(442, 93);
      this.ControlBox = false;
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.pbTransfer);
      this.Controls.Add((Control) this.lblProgress);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ImageLibraryClient);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Image Library";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
