// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.LoanCenter.DocumentationClassificationClient
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.ClientServer.eFolder;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.DocumentConverter;
using EllieMae.EMLite.eFolder.DocClassificationControllerServiceReference;
using EllieMae.EMLite.eFolder.Files;
using EllieMae.EMLite.eFolder.WcfExtensions;
using EllieMae.EMLite.ePass;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.LoanCenter
{
  public class DocumentationClassificationClient : Form
  {
    private const string className = "DocumentationClassificationClient";
    private static readonly string sw = Tracing.SwEFolder;
    private Sessions.Session session;
    private DocClassificationControllerClient svcClient;
    private LoanDataMgr loanDataMgr;
    private FileAttachment[] fileList;
    private DocumentLog docLog;
    private IContainer components;
    private Button btnCancel;
    private ProgressBar pbTransfer;
    private Label lblTransfer;
    private ProgressBar pbOverall;
    private Label lblOverall;
    private BackgroundWorker worker;

    public DocumentationClassificationClient(Sessions.Session session)
    {
      this.InitializeComponent();
      this.session = session;
      this.loanDataMgr = session.LoanDataMgr;
      this.svcClient = DocumentationClassificationClient.InitializeServiceClient();
    }

    public static DocClassificationControllerClient InitializeServiceClient()
    {
      Tracing.Log(DocumentationClassificationClient.sw, TraceLevel.Verbose, nameof (DocumentationClassificationClient), "Initializing Service Client and Endpoint");
      string str = Session.SessionObjects?.StartupInfo?.ServiceUrls?.DocumentClassificationUrl;
      if (string.IsNullOrWhiteSpace(str) || !Uri.IsWellFormedUriString(str, UriKind.Absolute))
        str = "https://loancenter.elliemae.com/DocumentClassification/DocClassificationController.svc";
      EndpointAddress remoteAddress = new EndpointAddress(str);
      WSHttpBinding wsHttpBinding = new WSHttpBinding();
      wsHttpBinding.Security.Mode = SecurityMode.Transport;
      wsHttpBinding.SendTimeout = TimeSpan.FromMinutes(10.0);
      wsHttpBinding.ReceiveTimeout = TimeSpan.FromMinutes(10.0);
      wsHttpBinding.MessageEncoding = WSMessageEncoding.Mtom;
      wsHttpBinding.MaxReceivedMessageSize = (long) int.MaxValue;
      wsHttpBinding.MaxBufferPoolSize = (long) int.MaxValue;
      wsHttpBinding.ReaderQuotas.MaxStringContentLength = int.MaxValue;
      wsHttpBinding.ReaderQuotas.MaxArrayLength = int.MaxValue;
      wsHttpBinding.ReaderQuotas.MaxBytesPerRead = int.MaxValue;
      wsHttpBinding.ReaderQuotas.MaxNameTableCharCount = int.MaxValue;
      DocClassificationControllerClient controllerClient = new DocClassificationControllerClient((System.ServiceModel.Channels.Binding) wsHttpBinding, remoteAddress);
      controllerClient.ChannelFactory.Endpoint.Behaviors.Add((IEndpointBehavior) new SsoTokenEndpointBehavior());
      return controllerClient;
    }

    public DialogResult SuggestTraining(DocumentLog docLog, FileAttachment[] attachmentList)
    {
      this.docLog = docLog;
      this.fileList = attachmentList;
      Tracing.Log(DocumentationClassificationClient.sw, TraceLevel.Verbose, nameof (DocumentationClassificationClient), "Starting Thread");
      this.worker.RunWorkerAsync();
      Tracing.Log(DocumentationClassificationClient.sw, TraceLevel.Verbose, nameof (DocumentationClassificationClient), "Showing Dialog");
      return this.ShowDialog((IWin32Window) Form.ActiveForm);
    }

    private void worker_DoWork(object sender, DoWorkEventArgs e)
    {
      Tracing.Log(DocumentationClassificationClient.sw, TraceLevel.Verbose, nameof (DocumentationClassificationClient), "Thread Started");
      List<Tuple<string, string>> tupleList = new List<Tuple<string, string>>();
      int length = this.fileList.Length;
      for (int index1 = 0; index1 < length; ++index1)
      {
        if (this.fileList[index1] is NativeAttachment)
        {
          NativeAttachment file1 = (NativeAttachment) this.fileList[index1];
          Tracing.Log(DocumentationClassificationClient.sw, TraceLevel.Verbose, nameof (DocumentationClassificationClient), "Training: " + file1.ID);
          this.loanDataMgr.FileAttachments.FileAttachmentDownloadProgress += new EllieMae.EMLite.eFolder.Files.TransferProgressEventHandler(this.attachment_downloadProgress);
          try
          {
            string file2 = new PdfFileCreator(this.loanDataMgr).CreateFile(file1);
            if (file2 == null)
              throw new Exception("Unable to convert the '" + file1.Title + "' file.");
            using (PdfEditor pdfEditor = new PdfEditor(file2))
            {
              string str = pdfEditor.ConvertPage(1, 300f, 300f, true);
              tupleList.Add(new Tuple<string, string>(file1.ID, str));
            }
            this.worker.ReportProgress(100, (object) this.pbTransfer);
          }
          catch (CanceledOperationException ex)
          {
            Tracing.Log(DocumentationClassificationClient.sw, TraceLevel.Verbose, nameof (DocumentationClassificationClient), "Download Cancelled");
            return;
          }
          finally
          {
            this.loanDataMgr.FileAttachments.FileAttachmentDownloadProgress -= new EllieMae.EMLite.eFolder.Files.TransferProgressEventHandler(this.attachment_downloadProgress);
          }
        }
        else if (this.fileList[index1] is ImageAttachment)
        {
          ImageAttachment file = this.fileList[index1] as ImageAttachment;
          Tracing.Log(DocumentationClassificationClient.sw, TraceLevel.Verbose, nameof (DocumentationClassificationClient), "Training: " + file.ID);
          this.loanDataMgr.FileAttachments.PageDownloaded += new ExtractProgressEventHandler(this.attachment_PageDownloaded);
          try
          {
            this.loanDataMgr.FileAttachments.DownloadAttachment(file);
            for (int index2 = 0; index2 < file.Pages.Count; ++index2)
            {
              string imageFile = this.loanDataMgr.FileAttachments.GetImageFile(file.Pages[index2]);
              tupleList.Add(new Tuple<string, string>(file.Pages[index2].ImageKey, imageFile));
            }
          }
          catch (CanceledOperationException ex)
          {
            Tracing.Log(DocumentationClassificationClient.sw, TraceLevel.Verbose, nameof (DocumentationClassificationClient), "Page Download Cancelled");
            return;
          }
          finally
          {
            this.loanDataMgr.FileAttachments.PageDownloaded -= new ExtractProgressEventHandler(this.attachment_PageDownloaded);
          }
        }
        else if (this.fileList[index1] is CloudAttachment)
        {
          CloudAttachment file = (CloudAttachment) this.fileList[index1];
          Tracing.Log(DocumentationClassificationClient.sw, TraceLevel.Verbose, nameof (DocumentationClassificationClient), "Training: " + file.ID);
          this.loanDataMgr.FileAttachments.FileAttachmentDownloadProgress += new EllieMae.EMLite.eFolder.Files.TransferProgressEventHandler(this.attachment_downloadProgress);
          try
          {
            string pdf = this.loanDataMgr.FileAttachments.CreatePdf(new CloudAttachment[1]
            {
              file
            }, AnnotationExportType.None);
            if (pdf == null)
              throw new Exception("Unable to convert the '" + file.Title + "' file.");
            using (PdfEditor pdfEditor = new PdfEditor(pdf))
            {
              int pageCount = pdfEditor.PageCount;
              for (int index3 = 0; index3 < pageCount; ++index3)
              {
                string str = pdfEditor.ConvertPage(index3 + 1, 300f, 300f, true);
                tupleList.Add(new Tuple<string, string>(file.ID + "-" + (object) index3, str));
                if (this.worker.CancellationPending)
                  return;
                this.worker.ReportProgress(50 + 50 * index3 / pageCount, (object) this.pbTransfer);
              }
            }
          }
          catch (CanceledOperationException ex)
          {
            Tracing.Log(DocumentationClassificationClient.sw, TraceLevel.Verbose, nameof (DocumentationClassificationClient), "Download Cancelled");
            return;
          }
          finally
          {
            this.loanDataMgr.FileAttachments.FileAttachmentDownloadProgress -= new EllieMae.EMLite.eFolder.Files.TransferProgressEventHandler(this.attachment_downloadProgress);
          }
        }
        this.worker.ReportProgress((index1 + 1) * 50 / length, (object) this.pbOverall);
      }
      int count = tupleList.Count;
      for (int index = 0; index < count; ++index)
      {
        if (this.worker.CancellationPending)
          return;
        Tuple<string, string> tuple = tupleList[count - (index + 1)];
        this.worker.ReportProgress(0, (object) this.pbTransfer);
        Tracing.Log(DocumentationClassificationClient.sw, TraceLevel.Verbose, nameof (DocumentationClassificationClient), "Calling File.ReadAllBytes: " + tuple.Item2);
        byte[] content = File.ReadAllBytes(tuple.Item2);
        this.worker.ReportProgress(100, (object) this.pbTransfer);
        Tracing.Log(DocumentationClassificationClient.sw, TraceLevel.Verbose, nameof (DocumentationClassificationClient), "Calling SuggestTraining: " + tuple.Item1);
        this.svcClient.SuggestTraining(Session.CompanyInfo.ClientID, Session.UserID, EpassLogin.LoginPassword, tuple.Item1, this.docLog.Title, Session.UserInfo.FullName, content);
        e.Result = (object) "Some";
        this.worker.ReportProgress(50 + (index + 1) * 50 / count, (object) this.pbOverall);
      }
      e.Result = (object) "All";
    }

    private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
      ((ProgressBar) e.UserState).Value = e.ProgressPercentage;
    }

    private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      if (e.Error != null)
      {
        Tracing.Log(DocumentationClassificationClient.sw, TraceLevel.Error, nameof (DocumentationClassificationClient), e.Error.ToString());
        int num = (int) Utils.Dialog((IWin32Window) this, "The following error occurred when trying to process the request:\n\n" + e.Error.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      if ((string) e.Result == "All")
        this.DialogResult = DialogResult.OK;
      else if ((string) e.Result == "Some")
        this.DialogResult = DialogResult.Abort;
      else
        this.DialogResult = DialogResult.Cancel;
    }

    private void attachment_downloadProgress(object source, TransferProgressEventArgs e)
    {
      if (this.worker.CancellationPending)
        e.Cancel = true;
      this.worker.ReportProgress(Convert.ToInt32(e.PercentCompleted / 2), (object) this.pbTransfer);
    }

    private void attachment_PageDownloaded(object source, ExtractProgressEventArgs e)
    {
      if (this.worker.CancellationPending)
        e.Cancel = true;
      this.worker.ReportProgress(e.PercentCompleted * 50, (object) this.pbTransfer);
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      Tracing.Log(DocumentationClassificationClient.sw, TraceLevel.Verbose, nameof (DocumentationClassificationClient), "Cancelling");
      this.worker.CancelAsync();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        if (this.svcClient != null)
          this.svcClient.Close();
        if (this.components != null)
          this.components.Dispose();
      }
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.btnCancel = new Button();
      this.pbTransfer = new ProgressBar();
      this.lblTransfer = new Label();
      this.pbOverall = new ProgressBar();
      this.lblOverall = new Label();
      this.worker = new BackgroundWorker();
      this.SuspendLayout();
      this.btnCancel.Location = new Point(312, 90);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 0;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.pbTransfer.Location = new Point(12, 64);
      this.pbTransfer.Name = "pbTransfer";
      this.pbTransfer.Size = new Size(374, 16);
      this.pbTransfer.TabIndex = 3;
      this.lblTransfer.AutoSize = true;
      this.lblTransfer.Location = new Point(12, 48);
      this.lblTransfer.Name = "lblTransfer";
      this.lblTransfer.Size = new Size(105, 14);
      this.lblTransfer.TabIndex = 2;
      this.lblTransfer.Text = "Transfer Progress...";
      this.pbOverall.Location = new Point(12, 28);
      this.pbOverall.Name = "pbOverall";
      this.pbOverall.Size = new Size(374, 16);
      this.pbOverall.TabIndex = 1;
      this.lblOverall.AutoSize = true;
      this.lblOverall.Location = new Point(12, 12);
      this.lblOverall.Name = "lblOverall";
      this.lblOverall.Size = new Size(97, 14);
      this.lblOverall.TabIndex = 0;
      this.lblOverall.Text = "Overall Progress...";
      this.worker.WorkerReportsProgress = true;
      this.worker.WorkerSupportsCancellation = true;
      this.worker.DoWork += new DoWorkEventHandler(this.worker_DoWork);
      this.worker.ProgressChanged += new ProgressChangedEventHandler(this.worker_ProgressChanged);
      this.worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.worker_RunWorkerCompleted);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(398, 121);
      this.ControlBox = false;
      this.Controls.Add((Control) this.pbOverall);
      this.Controls.Add((Control) this.lblOverall);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.pbTransfer);
      this.Controls.Add((Control) this.lblTransfer);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (DocumentationClassificationClient);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Suggest For Training";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
