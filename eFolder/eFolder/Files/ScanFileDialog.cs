// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Files.ScanFileDialog
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using DTI.ImageMan.Twain;
using EllieMae.EMLite.ClientServer.eFolder;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.DocumentConverter;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.Files
{
  public class ScanFileDialog : Form
  {
    private const string className = "ScanFileDialog";
    private static readonly string sw = Tracing.SwEFolder;
    private LoanDataMgr loanDataMgr;
    private DocumentLog doc;
    private List<string> pageFileList;
    private List<FileAttachment> attachmentList;
    private DateTime progressStartTime;
    private int filesToUpload;
    private int filesUploaded;
    private int uploadProgressOffset;
    private IContainer components;
    private Label scannerLbl;
    private ComboBox scannerCbo;
    private Button scanBtn;
    private Button cancelBtn;
    private TwainControl dtiTwain;
    private Label lblTimeRemaining;
    private BackgroundWorker worker;

    public ScanFileDialog(LoanDataMgr loanDataMgr, DocumentLog doc)
    {
      this.InitializeComponent();
      this.loanDataMgr = loanDataMgr;
      this.doc = doc;
      this.attachmentList = new List<FileAttachment>();
      this.loadScannerList();
    }

    public FileAttachment[] Files => this.attachmentList.ToArray();

    private void loadScannerList()
    {
      foreach (Identity source in this.dtiTwain.Sources)
        this.scannerCbo.Items.Add((object) source.ProductName);
      if (this.scannerCbo.Items.Count <= 0)
        return;
      this.scannerCbo.SelectedIndex = 0;
    }

    private void scanBtn_Click(object sender, EventArgs e)
    {
      if (Session.SessionObjects.RuntimeEnvironment == RuntimeEnvironment.Hosted && !Modules.IsModuleAvailableForUser(EncompassModule.EDM, true))
        return;
      string text = this.scannerCbo.Text;
      if (text == string.Empty)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You must select a scanner.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        this.pageFileList = new List<string>();
        foreach (Identity source in this.dtiTwain.Sources)
        {
          if (text == source.ProductName)
            this.dtiTwain.CurrentSource = source;
        }
        Tracing.Log(ScanFileDialog.sw, TraceLevel.Verbose, nameof (ScanFileDialog), "Setting TwainControl.AppName");
        this.dtiTwain.AppName = "Encompass";
        Tracing.Log(ScanFileDialog.sw, TraceLevel.Verbose, nameof (ScanFileDialog), "Setting TwainControl.DisplayHandle");
        this.dtiTwain.DisplayHandle = this.Handle;
        Tracing.Log(ScanFileDialog.sw, TraceLevel.Verbose, nameof (ScanFileDialog), "Setting TwainControl.UserInterface");
        this.dtiTwain.UserInterface = UserInterfaces.ModalUserInterface;
        Tracing.Log(ScanFileDialog.sw, TraceLevel.Verbose, nameof (ScanFileDialog), "Setting TwainControl.PixelType");
        this.dtiTwain.PixelType = PixelTypes.BlackWhite;
        Tracing.Log(ScanFileDialog.sw, TraceLevel.Verbose, nameof (ScanFileDialog), "Setting TwainControl.MaxPages");
        this.dtiTwain.MaxPages = -1;
        this.scanBtn.Enabled = false;
        Tracing.Log(ScanFileDialog.sw, TraceLevel.Verbose, nameof (ScanFileDialog), "Calling TwainControl.ScanAll");
        bool flag = this.dtiTwain.ScanAll();
        Tracing.Log(ScanFileDialog.sw, TraceLevel.Verbose, nameof (ScanFileDialog), "Checking Scan Result: " + flag.ToString());
        if (flag)
          return;
        Tracing.Log(ScanFileDialog.sw, TraceLevel.Error, nameof (ScanFileDialog), this.dtiTwain.LastError.Details);
        int num2 = (int) Utils.Dialog((IWin32Window) this, "The following error occurred when trying to scan the document:\n\n" + this.dtiTwain.LastError.Details, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.scanBtn.Enabled = true;
      }
    }

    private void cancelBtn_Click(object sender, EventArgs e)
    {
      Tracing.Log(ScanFileDialog.sw, TraceLevel.Verbose, nameof (ScanFileDialog), "Checking Worker.IsBusy");
      if (this.worker.IsBusy)
      {
        Tracing.Log(ScanFileDialog.sw, TraceLevel.Verbose, nameof (ScanFileDialog), "Calling Worker.CancelAsync");
        this.worker.CancelAsync();
      }
      else
      {
        Tracing.Log(ScanFileDialog.sw, TraceLevel.Verbose, nameof (ScanFileDialog), "Closing Dialog");
        this.DialogResult = DialogResult.Cancel;
      }
    }

    private void dtiTwain_PageScanned(object sender, PageScannedEventArgs e)
    {
      string nameWithExtension;
      if (e.ScanImage.Palette.Entries.Length == 2)
      {
        nameWithExtension = SystemSettings.GetTempFileNameWithExtension("tif");
        e.ScanImage.Save(nameWithExtension, ImageFormat.Tiff);
      }
      else
      {
        nameWithExtension = SystemSettings.GetTempFileNameWithExtension("jpg");
        e.ScanImage.Save(nameWithExtension, ImageFormat.Jpeg);
      }
      Tracing.Log(ScanFileDialog.sw, TraceLevel.Verbose, nameof (ScanFileDialog), "TwainControl.PageScanned: " + nameWithExtension);
      this.pageFileList.Add(nameWithExtension);
    }

    private void dtiTwain_ScanCancelled(object sender, EventArgs e)
    {
      Tracing.Log(ScanFileDialog.sw, TraceLevel.Verbose, nameof (ScanFileDialog), "TwainControl.ScanCancelled");
      this.Activate();
      if (this.pageFileList.Count == 0)
      {
        this.DialogResult = DialogResult.Cancel;
      }
      else
      {
        this.progressStartTime = DateTime.Now;
        Tracing.Log(ScanFileDialog.sw, TraceLevel.Verbose, nameof (ScanFileDialog), "Starting Worker");
        this.worker.RunWorkerAsync((object) this.pageFileList.ToArray());
      }
    }

    private void dtiTwain_ScanComplete(object sender, EventArgs e)
    {
      Tracing.Log(ScanFileDialog.sw, TraceLevel.Verbose, nameof (ScanFileDialog), "TwainControl.ScanComplete: " + (object) this.pageFileList.Count);
      this.Activate();
      if (this.pageFileList.Count == 0)
      {
        this.DialogResult = DialogResult.Cancel;
      }
      else
      {
        this.progressStartTime = DateTime.Now;
        Tracing.Log(ScanFileDialog.sw, TraceLevel.Verbose, nameof (ScanFileDialog), "Starting Worker");
        this.worker.RunWorkerAsync((object) this.pageFileList.ToArray());
      }
    }

    private void worker_DoWork(object sender, DoWorkEventArgs e)
    {
      Tracing.Log(ScanFileDialog.sw, TraceLevel.Verbose, nameof (ScanFileDialog), "Worker Started");
      string[] fileList = (string[]) e.Argument;
      this.loanDataMgr.FileAttachments.FileAttachmentUploadProgress += new TransferProgressEventHandler(this.uploadProgress);
      try
      {
        if (this.loanDataMgr.UseSkyDrive)
        {
          this.filesToUpload = 1;
          this.filesUploaded = 0;
          this.uploadProgressOffset = 0;
          Tracing.Log(ScanFileDialog.sw, TraceLevel.Verbose, nameof (ScanFileDialog), "Creating PdfCreator");
          PdfCreator pdfCreator = new PdfCreator();
          Tracing.Log(ScanFileDialog.sw, TraceLevel.Verbose, nameof (ScanFileDialog), "Calling PdfCreator.MergeImages");
          string filepath = pdfCreator.MergeImages(fileList, new PdfTextAnnotation[0]);
          string title = string.Format("{0} Page Scan", (object) fileList.Length);
          Tracing.Log(ScanFileDialog.sw, TraceLevel.Verbose, nameof (ScanFileDialog), "Creating CloudAttachment");
          this.attachmentList.Add(this.loanDataMgr.FileAttachments.AddAttachment(AddReasonType.Scan, filepath, title, this.doc));
          ++this.filesUploaded;
        }
        else if (this.loanDataMgr.SystemConfiguration.ImageAttachmentSettings.UseImageAttachments)
        {
          this.filesToUpload = 1;
          this.filesUploaded = 0;
          this.uploadProgressOffset = 50;
          string title = string.Format("{0} Page Scan", (object) fileList.Length);
          Tracing.Log(ScanFileDialog.sw, TraceLevel.Verbose, nameof (ScanFileDialog), "Creating ImageAttachment");
          this.attachmentList.Add(this.loanDataMgr.FileAttachments.AddAttachment(AddReasonType.Scan, fileList, title, this.doc));
          ++this.filesUploaded;
        }
        else
        {
          this.filesToUpload = fileList.Length;
          this.filesUploaded = 0;
          this.uploadProgressOffset = 0;
          for (int index = 0; index < fileList.Length; ++index)
          {
            string title = string.Format("{0} Page Scan - Page {1}", (object) fileList.Length, (object) (index + 1));
            Tracing.Log(ScanFileDialog.sw, TraceLevel.Verbose, nameof (ScanFileDialog), "Creating NativeAttachment");
            this.attachmentList.Add(this.loanDataMgr.FileAttachments.AddAttachment(AddReasonType.Scan, fileList[index], title, this.doc));
            ++this.filesUploaded;
          }
        }
      }
      catch (Exception ex)
      {
        if (ex.InnerException is CanceledOperationException)
          Tracing.Log(ScanFileDialog.sw, TraceLevel.Verbose, nameof (ScanFileDialog), "Transfer Cancelled");
        else
          throw;
      }
      finally
      {
        this.loanDataMgr.FileAttachments.FileAttachmentUploadProgress -= new TransferProgressEventHandler(this.uploadProgress);
      }
    }

    private void uploadProgress(object source, TransferProgressEventArgs e)
    {
      if (this.worker.CancellationPending)
        e.Cancel = true;
      int num1 = 100 - this.uploadProgressOffset;
      int num2 = num1 * this.filesToUpload;
      this.worker.ReportProgress(100 * (num1 * this.filesUploaded + (e.PercentCompleted - this.uploadProgressOffset)) / num2);
    }

    private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
      if (e.ProgressPercentage <= 0)
        return;
      double num = TimeSpan.FromTicks(DateTime.Now.Ticks - this.progressStartTime.Ticks).TotalMilliseconds / System.Convert.ToDouble(e.ProgressPercentage);
      TimeSpan timeSpan = TimeSpan.FromMilliseconds(System.Convert.ToDouble(100 - e.ProgressPercentage) * num);
      if (timeSpan.TotalSeconds >= 120.0)
        this.lblTimeRemaining.Text = "About " + System.Convert.ToInt32(timeSpan.TotalMinutes).ToString() + " minutes remaining";
      else if (timeSpan.TotalSeconds > 60.0)
        this.lblTimeRemaining.Text = "About 1 minute and " + System.Convert.ToInt32(timeSpan.TotalSeconds - 60.0).ToString() + " seconds remaining";
      else
        this.lblTimeRemaining.Text = "About " + timeSpan.Seconds.ToString() + " seconds remaining";
      this.lblTimeRemaining.Visible = true;
    }

    private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      if (e.Error != null)
      {
        Tracing.Log(ScanFileDialog.sw, TraceLevel.Error, nameof (ScanFileDialog), e.Error.ToString());
        int num = (int) Utils.Dialog((IWin32Window) this, "The following error occurred when trying to upload the scanned document:\n\n" + e.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      if (this.attachmentList.Count > 0)
      {
        if (Session.SessionObjects.RuntimeEnvironment == RuntimeEnvironment.Hosted)
          Transaction.Log(this.loanDataMgr, "Scan");
        this.DialogResult = DialogResult.OK;
      }
      else
        this.DialogResult = DialogResult.Cancel;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.scannerLbl = new Label();
      this.scannerCbo = new ComboBox();
      this.scanBtn = new Button();
      this.cancelBtn = new Button();
      this.dtiTwain = new TwainControl();
      this.lblTimeRemaining = new Label();
      this.worker = new BackgroundWorker();
      this.SuspendLayout();
      this.scannerLbl.AutoSize = true;
      this.scannerLbl.Location = new Point(12, 16);
      this.scannerLbl.Name = "scannerLbl";
      this.scannerLbl.Size = new Size(48, 14);
      this.scannerLbl.TabIndex = 0;
      this.scannerLbl.Text = "Scanner";
      this.scannerCbo.DropDownStyle = ComboBoxStyle.DropDownList;
      this.scannerCbo.Location = new Point(68, 12);
      this.scannerCbo.Name = "scannerCbo";
      this.scannerCbo.Size = new Size(323, 22);
      this.scannerCbo.TabIndex = 1;
      this.scanBtn.Location = new Point(236, 44);
      this.scanBtn.Name = "scanBtn";
      this.scanBtn.Size = new Size(75, 22);
      this.scanBtn.TabIndex = 2;
      this.scanBtn.Text = "Scan";
      this.scanBtn.Click += new EventHandler(this.scanBtn_Click);
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(316, 44);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 22);
      this.cancelBtn.TabIndex = 3;
      this.cancelBtn.Text = "Cancel";
      this.cancelBtn.Click += new EventHandler(this.cancelBtn_Click);
      this.dtiTwain.PageScanned += new TwainControl.PageScannedHandler(this.dtiTwain_PageScanned);
      this.dtiTwain.ScanComplete += new TwainControl.ScanCompleteHandler(this.dtiTwain_ScanComplete);
      this.dtiTwain.ScanCancelled += new TwainControl.ScanCancelledHandler(this.dtiTwain_ScanCancelled);
      this.lblTimeRemaining.AutoSize = true;
      this.lblTimeRemaining.Location = new Point(12, 48);
      this.lblTimeRemaining.Name = "lblTimeRemaining";
      this.lblTimeRemaining.Size = new Size(149, 14);
      this.lblTimeRemaining.TabIndex = 7;
      this.lblTimeRemaining.Text = "About N seconds remaining...";
      this.lblTimeRemaining.Visible = false;
      this.worker.WorkerReportsProgress = true;
      this.worker.WorkerSupportsCancellation = true;
      this.worker.DoWork += new DoWorkEventHandler(this.worker_DoWork);
      this.worker.ProgressChanged += new ProgressChangedEventHandler(this.worker_ProgressChanged);
      this.worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.worker_RunWorkerCompleted);
      this.AcceptButton = (IButtonControl) this.scanBtn;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(401, 76);
      this.ControlBox = false;
      this.Controls.Add((Control) this.lblTimeRemaining);
      this.Controls.Add((Control) this.scannerLbl);
      this.Controls.Add((Control) this.scannerCbo);
      this.Controls.Add((Control) this.scanBtn);
      this.Controls.Add((Control) this.cancelBtn);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ScanFileDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Scan and Attach";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
