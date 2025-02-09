// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Files.IdentifyFilesDialog
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.ClientServer.eFolder;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.DocumentConverter;
using EllieMae.EMLite.ePass;
using EllieMae.EMLite.LoanUtils.eFolder.SkyDriveClassic;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.WebServices;
using Microsoft.Web.Services2.Attachments;
using Microsoft.Web.Services2.Dime;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.Files
{
  public class IdentifyFilesDialog : Form
  {
    private const string className = "IdentifyFilesDialog";
    private static readonly string sw = Tracing.SwEFolder;
    private LoanDataMgr loanDataMgr;
    private List<FileAttachment> identifyFileList;
    private bool logTransaction;
    private IContainer components;
    private Label lblTransfer;
    private ProgressBar pbTransfer;
    private Button btnCancel;
    private BackgroundWorker worker;
    private ProgressBar pbOverall;
    private Label lblOverall;

    public IdentifyFilesDialog(LoanDataMgr loanDataMgr)
    {
      this.InitializeComponent();
      this.loanDataMgr = loanDataMgr;
    }

    public void IdentifyFiles(FileAttachment[] fileList)
    {
      if (!Modules.IsModuleAvailableForUser(EncompassModule.EDM, true))
        return;
      this.identifyFileList = new List<FileAttachment>();
      foreach (FileAttachment file in fileList)
      {
        if (!(file is BackgroundAttachment))
        {
          if (this.loanDataMgr.UseSkyDriveClassic && file is NativeAttachment && file.Identities != null)
            file.Identities.Remove(0);
          if (!file.Identities.Complete)
            this.identifyFileList.Add(file);
        }
      }
      if (this.identifyFileList.Count <= 0)
        return;
      Tracing.Log(IdentifyFilesDialog.sw, TraceLevel.Verbose, nameof (IdentifyFilesDialog), "Reset LogTransaction Flag");
      this.logTransaction = false;
      Tracing.Log(IdentifyFilesDialog.sw, TraceLevel.Verbose, nameof (IdentifyFilesDialog), "Starting Thread");
      this.worker.RunWorkerAsync();
      Tracing.Log(IdentifyFilesDialog.sw, TraceLevel.Verbose, nameof (IdentifyFilesDialog), "Showing Dialog");
      int num = (int) this.ShowDialog((IWin32Window) Form.ActiveForm);
      Tracing.Log(IdentifyFilesDialog.sw, TraceLevel.Verbose, nameof (IdentifyFilesDialog), "Checking LogTransaction Flag");
      if (!this.logTransaction)
        return;
      Tracing.Log(IdentifyFilesDialog.sw, TraceLevel.Verbose, nameof (IdentifyFilesDialog), "Logging Transaction");
      Transaction.Log(this.loanDataMgr, "IDR");
    }

    private void worker_DoWork(object sender, DoWorkEventArgs e)
    {
      Tracing.Log(IdentifyFilesDialog.sw, TraceLevel.Verbose, nameof (IdentifyFilesDialog), "Thread Started");
      List<Tuple<FileAttachment, int, string>> tupleList = new List<Tuple<FileAttachment, int, string>>();
      int count1 = this.identifyFileList.Count;
      using (PerformanceMeter.StartNew("IdentifyFilesDialog.worker_DoWork", count1.ToString() + " files", 115, nameof (worker_DoWork), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\Files\\IdentifyFilesDialog.cs"))
      {
        for (int index1 = 0; index1 < count1; ++index1)
        {
          if (this.identifyFileList[index1] is NativeAttachment)
          {
            NativeAttachment identifyFile = (NativeAttachment) this.identifyFileList[index1];
            Tracing.Log(IdentifyFilesDialog.sw, TraceLevel.Verbose, nameof (IdentifyFilesDialog), "Identifying: " + identifyFile.ID);
            this.loanDataMgr.FileAttachments.FileAttachmentDownloadProgress += new TransferProgressEventHandler(this.attachment_downloadProgress);
            try
            {
              string empty = string.Empty;
              string filepath = !Session.LoanDataMgr.UseSkyDriveClassic ? new PdfFileCreator(Session.LoanDataMgr).CreateFile(identifyFile) : new SDCHelper(Session.LoanDataMgr).GetConvertedVersionFile((FileAttachment) identifyFile);
              if (filepath == null)
                throw new Exception("Unable to convert the '" + identifyFile.Title + "' file.");
              using (PdfEditor pdfEditor = new PdfEditor(filepath))
              {
                string str = pdfEditor.ConvertPage(1, 300f, 300f, true);
                tupleList.Add(new Tuple<FileAttachment, int, string>((FileAttachment) identifyFile, 0, str));
              }
              this.worker.ReportProgress(100, (object) this.pbTransfer);
            }
            catch (CanceledOperationException ex)
            {
              Tracing.Log(IdentifyFilesDialog.sw, TraceLevel.Verbose, nameof (IdentifyFilesDialog), "Download Cancelled");
              return;
            }
            finally
            {
              this.loanDataMgr.FileAttachments.FileAttachmentDownloadProgress -= new TransferProgressEventHandler(this.attachment_downloadProgress);
            }
          }
          else if (this.identifyFileList[index1] is ImageAttachment)
          {
            ImageAttachment identifyFile = this.identifyFileList[index1] as ImageAttachment;
            this.loanDataMgr.FileAttachments.PageDownloaded += new ExtractProgressEventHandler(this.attachment_PageDownloaded);
            try
            {
              this.loanDataMgr.FileAttachments.DownloadAttachment(identifyFile);
              for (int index2 = 0; index2 < identifyFile.Pages.Count; ++index2)
              {
                if (identifyFile.Identities.Get(index2) == null)
                {
                  string imageFile = this.loanDataMgr.FileAttachments.GetImageFile(identifyFile.Pages[index2]);
                  tupleList.Add(new Tuple<FileAttachment, int, string>((FileAttachment) identifyFile, index2, imageFile));
                }
              }
            }
            catch (CanceledOperationException ex)
            {
              Tracing.Log(IdentifyFilesDialog.sw, TraceLevel.Verbose, nameof (IdentifyFilesDialog), "Page Download Cancelled");
              return;
            }
            finally
            {
              this.loanDataMgr.FileAttachments.PageDownloaded -= new ExtractProgressEventHandler(this.attachment_PageDownloaded);
            }
          }
          else if (this.identifyFileList[index1] is CloudAttachment)
          {
            CloudAttachment identifyFile = (CloudAttachment) this.identifyFileList[index1];
            Tracing.Log(IdentifyFilesDialog.sw, TraceLevel.Verbose, nameof (IdentifyFilesDialog), "Identifying: " + identifyFile.ID);
            this.loanDataMgr.FileAttachments.FileAttachmentDownloadProgress += new TransferProgressEventHandler(this.attachment_downloadProgress);
            try
            {
              string pdf = this.loanDataMgr.FileAttachments.CreatePdf(new CloudAttachment[1]
              {
                identifyFile
              }, AnnotationExportType.None);
              if (pdf == null)
                throw new Exception("Unable to convert the '" + identifyFile.Title + "' file.");
              using (PdfEditor pdfEditor = new PdfEditor(pdf))
              {
                int pageCount = pdfEditor.PageCount;
                for (int pageIndex = 0; pageIndex < pageCount; ++pageIndex)
                {
                  if (identifyFile.Identities.Get(pageIndex) == null)
                  {
                    string str = pdfEditor.ConvertPage(pageIndex + 1, 300f, 300f, true);
                    tupleList.Add(new Tuple<FileAttachment, int, string>((FileAttachment) identifyFile, pageIndex, str));
                    if (this.worker.CancellationPending)
                      return;
                    this.worker.ReportProgress(50 + 50 * pageIndex / pageCount, (object) this.pbTransfer);
                  }
                }
              }
            }
            catch (CanceledOperationException ex)
            {
              Tracing.Log(IdentifyFilesDialog.sw, TraceLevel.Verbose, nameof (IdentifyFilesDialog), "Download Cancelled");
              return;
            }
            finally
            {
              this.loanDataMgr.FileAttachments.FileAttachmentDownloadProgress -= new TransferProgressEventHandler(this.attachment_downloadProgress);
            }
          }
          this.worker.ReportProgress((index1 + 1) * 50 / count1, (object) this.pbOverall);
        }
      }
      using (IDRServiceWse idrServiceWse = new IDRServiceWse(Session.SessionObjects?.StartupInfo?.ServiceUrls?.IDRServiceUrl))
      {
        idrServiceWse.ChunkSent += new ChunkHandler(this.chunkSent);
        try
        {
          int count2 = tupleList.Count;
          for (int index = 0; index < count2; ++index)
          {
            idrServiceWse.IDRCredentialsValue = new IDRCredentials();
            idrServiceWse.IDRCredentialsValue.ClientID = Session.CompanyInfo.ClientID;
            idrServiceWse.IDRCredentialsValue.UserID = Session.UserID;
            idrServiceWse.IDRCredentialsValue.Password = EpassLogin.LoginPassword;
            Tuple<FileAttachment, int, string> tuple = tupleList[index];
            Tracing.Log(IdentifyFilesDialog.sw, TraceLevel.Verbose, nameof (IdentifyFilesDialog), "Creating DimeAttachment");
            DimeAttachment dimeAttachment = new DimeAttachment(tuple.Item1.ID, "image/tiff", TypeFormat.Unchanged, tuple.Item3);
            Tracing.Log(IdentifyFilesDialog.sw, TraceLevel.Verbose, nameof (IdentifyFilesDialog), "Adding DimeAttachment");
            idrServiceWse.RequestSoapContext.Attachments.Add((Attachment) dimeAttachment);
            Tracing.Log(IdentifyFilesDialog.sw, TraceLevel.Verbose, nameof (IdentifyFilesDialog), "Calling IdentifyForm");
            FormIdentity formIdentity = idrServiceWse.IdentifyForm();
            Tracing.Log(IdentifyFilesDialog.sw, TraceLevel.Verbose, nameof (IdentifyFilesDialog), "Checking FormIdentity");
            if (formIdentity != null)
            {
              Tracing.Log(IdentifyFilesDialog.sw, TraceLevel.Verbose, nameof (IdentifyFilesDialog), "FormIdentity: " + formIdentity.Title + "--Confidence Score: " + (object) formIdentity.Confidence + "--Document Source:" + formIdentity.DocumentSource);
              tuple.Item1.Identities.Add(new DocumentIdentity(tuple.Item2, formIdentity.DocumentType, formIdentity.DocumentSource));
              Tracing.Log(IdentifyFilesDialog.sw, TraceLevel.Verbose, nameof (IdentifyFilesDialog), "Set LogTransaction Flag");
              this.logTransaction = true;
            }
            else
            {
              Tracing.Log(IdentifyFilesDialog.sw, TraceLevel.Verbose, nameof (IdentifyFilesDialog), "Unknown Identity");
              tuple.Item1.Identities.Add(new DocumentIdentity(tuple.Item2, "Unknown", "Unknown"));
            }
            FileAttachment fileAttachment = (FileAttachment) null;
            if (index + 1 < count2)
              fileAttachment = tupleList[index + 1].Item1;
            if (tuple.Item1 != fileAttachment)
              tuple.Item1.Identities.Complete = true;
            this.worker.ReportProgress(50 + (index + 1) * 50 / count2, (object) this.pbOverall);
          }
        }
        catch (WebException ex)
        {
          if (ex.Status == WebExceptionStatus.RequestCanceled)
            Tracing.Log(IdentifyFilesDialog.sw, TraceLevel.Verbose, nameof (IdentifyFilesDialog), "Transfer Cancelled");
          else
            throw;
        }
        finally
        {
          idrServiceWse.ChunkSent -= new ChunkHandler(this.chunkSent);
        }
      }
    }

    private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
      ProgressBar userState = (ProgressBar) e.UserState;
      if (e.ProgressPercentage > 100)
        return;
      userState.Value = e.ProgressPercentage;
    }

    private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      if (e.Error != null)
      {
        Tracing.Log(IdentifyFilesDialog.sw, TraceLevel.Error, nameof (IdentifyFilesDialog), e.Error.ToString());
        int num = (int) Utils.Dialog((IWin32Window) this, "The following error occurred when trying to identify the files:\n\n" + e.Error.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      Tracing.Log(IdentifyFilesDialog.sw, TraceLevel.Verbose, nameof (IdentifyFilesDialog), "Closing Window");
      this.Close();
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

    private void chunkSent(object sender, ChunkHandlerEventArgs e)
    {
      if (this.worker.CancellationPending)
        e.Abort = true;
      this.worker.ReportProgress(e.Percent, (object) this.pbTransfer);
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      Tracing.Log(IdentifyFilesDialog.sw, TraceLevel.Verbose, nameof (IdentifyFilesDialog), "Cancelling");
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
      this.lblTransfer = new Label();
      this.pbTransfer = new ProgressBar();
      this.btnCancel = new Button();
      this.worker = new BackgroundWorker();
      this.pbOverall = new ProgressBar();
      this.lblOverall = new Label();
      this.SuspendLayout();
      this.lblTransfer.AutoSize = true;
      this.lblTransfer.Location = new Point(12, 48);
      this.lblTransfer.Name = "lblTransfer";
      this.lblTransfer.Size = new Size(105, 14);
      this.lblTransfer.TabIndex = 2;
      this.lblTransfer.Text = "Transfer Progress...";
      this.pbTransfer.Location = new Point(12, 64);
      this.pbTransfer.Name = "pbTransfer";
      this.pbTransfer.Size = new Size(374, 16);
      this.pbTransfer.TabIndex = 3;
      this.btnCancel.Location = new Point(312, 88);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 4;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.worker.WorkerReportsProgress = true;
      this.worker.WorkerSupportsCancellation = true;
      this.worker.DoWork += new DoWorkEventHandler(this.worker_DoWork);
      this.worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.worker_RunWorkerCompleted);
      this.worker.ProgressChanged += new ProgressChangedEventHandler(this.worker_ProgressChanged);
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
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(398, 121);
      this.ControlBox = false;
      this.Controls.Add((Control) this.pbOverall);
      this.Controls.Add((Control) this.lblOverall);
      this.Controls.Add((Control) this.lblTransfer);
      this.Controls.Add((Control) this.pbTransfer);
      this.Controls.Add((Control) this.btnCancel);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (IdentifyFilesDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Identifying Files";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
