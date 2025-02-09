// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Files.AttachFilesDialog
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.ClientServer.eFolder;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.DocumentConverter;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.WebServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.Files
{
  public class AttachFilesDialog : Form
  {
    private const string className = "AttachFilesDialog";
    private static readonly string sw = Tracing.SwEFolder;
    private LoanDataMgr loanDataMgr;
    private AddReasonType reason;
    private string[] fileList;
    private string title;
    private FileAttachment[] sourceList;
    private PageIdentity[] identityList;
    private DocumentLog[] docList;
    private List<FileAttachment> attachedList;
    private DateTime transferStartTime;
    private Dictionary<string, List<int>> pageIndexTable;
    private string[] fileListForSDC;
    private IContainer components;
    private Label lblTransfer;
    private ProgressBar pbTransfer;
    private Button btnCancel;
    private Label lblOverall;
    private ProgressBar pbOverall;
    private BackgroundWorker worker;
    private Label lblPageCount;
    private Label lblTimeRemaining;

    public AttachFilesDialog(LoanDataMgr loanDataMgr)
    {
      this.InitializeComponent();
      this.loanDataMgr = loanDataMgr;
    }

    public FileAttachment[] AttachFiles(DocumentLog doc)
    {
      if (Session.SessionObjects.RuntimeEnvironment == RuntimeEnvironment.Hosted && !Modules.IsModuleAvailableForUser(EncompassModule.EDM, true))
        return (FileAttachment[]) null;
      FileAttachment[] fileAttachmentArray = (FileAttachment[]) null;
      using (OpenFileDialog openFileDialog = new OpenFileDialog())
      {
        openFileDialog.Title = "Browse and Attach";
        openFileDialog.CheckFileExists = true;
        openFileDialog.Multiselect = true;
        openFileDialog.ShowReadOnly = false;
        openFileDialog.Filter = "All Supported Formats|*.pdf;*.doc;*.docx;*.txt;*.tif;*.jpg;*.jpeg;*.jpe;*.emf;*.xps|Adobe PDF Documents (*.pdf)|*.pdf|Microsoft Word Documents (*.doc,*.docx)|*.doc;*.docx|Text Documents (*.txt)|*.txt|TIFF Images (*.tif)|*.tif|JPEG Images (*.jpg, *.jpeg, *.jpe)|*.jpg;*.jpeg;*.jpe|Windows Enhanced Metafile (*.emf)|*.emf|XPS Files (*.xps)|*.xps";
        if (openFileDialog.ShowDialog((IWin32Window) Form.ActiveForm) == DialogResult.OK)
          fileAttachmentArray = this.AttachFiles(openFileDialog.FileNames, doc);
      }
      if (Session.SessionObjects.RuntimeEnvironment == RuntimeEnvironment.Hosted && fileAttachmentArray != null)
        Transaction.Log(this.loanDataMgr, "Browse");
      return fileAttachmentArray;
    }

    public FileAttachment[] AttachFile(
      AddReasonType reason,
      string filepath,
      string title,
      PageIdentity[] identityList,
      DocumentLog doc)
    {
      string[] fileList = new string[1]{ filepath };
      return this.attachFiles(reason, fileList, title, (FileAttachment[]) null, identityList, doc);
    }

    public FileAttachment[] AttachFiles(string[] fileList, DocumentLog doc)
    {
      return this.attachFiles(AddReasonType.Browse, fileList, (string) null, (FileAttachment[]) null, (PageIdentity[]) null, doc);
    }

    public FileAttachment ReplaceFile(
      AddReasonType reason,
      NativeAttachment attachment,
      string filepath,
      DocumentLog doc)
    {
      string[] fileList = new string[1]{ filepath };
      FileAttachment[] sourceList = new FileAttachment[1]
      {
        (FileAttachment) attachment
      };
      FileAttachment[] fileAttachmentArray = this.attachFiles(reason, fileList, attachment.Title, sourceList, (PageIdentity[]) null, doc);
      return fileAttachmentArray != null && fileAttachmentArray.Length != 0 ? fileAttachmentArray[0] : (FileAttachment) null;
    }

    public FileAttachment[] SplitFile(
      NativeAttachment attachment,
      string filepath1,
      string filepath2,
      DocumentLog doc,
      Dictionary<string, List<int>> pageIndexTable = null,
      string[] fileListForSDC = null)
    {
      string[] fileList = new string[2]
      {
        filepath1,
        filepath2
      };
      DocumentLog[] docList = new DocumentLog[2]{ doc, doc };
      return this.SplitFile(attachment, fileList, docList, pageIndexTable, fileListForSDC);
    }

    public FileAttachment[] SplitFile(
      NativeAttachment attachment,
      string[] fileList,
      DocumentLog[] docList,
      Dictionary<string, List<int>> pageIndexTable = null,
      string[] fileListForSDC = null)
    {
      if (this.loanDataMgr.UseSkyDriveClassic)
      {
        this.pageIndexTable = pageIndexTable;
        this.fileListForSDC = fileListForSDC;
      }
      FileAttachment[] sourceList = new FileAttachment[1]
      {
        (FileAttachment) attachment
      };
      return this.attachFiles(AddReasonType.Split, fileList, attachment.Title, sourceList, (PageIdentity[]) null, docList);
    }

    public FileAttachment MergeFiles(
      FileAttachment[] attachmentList,
      string filepath,
      DocumentLog doc)
    {
      FileAttachment[] fileAttachmentArray = this.attachFiles(AddReasonType.Merge, new string[1]
      {
        filepath
      }, "Merged File", attachmentList, (PageIdentity[]) null, doc);
      return fileAttachmentArray != null && fileAttachmentArray.Length != 0 ? fileAttachmentArray[0] : (FileAttachment) null;
    }

    private FileAttachment[] attachFiles(
      AddReasonType reason,
      string[] fileList,
      string title,
      FileAttachment[] sourceList,
      PageIdentity[] identityList,
      DocumentLog doc)
    {
      DocumentLog[] docList = new DocumentLog[1]{ doc };
      return this.attachFiles(reason, fileList, title, sourceList, identityList, docList);
    }

    private FileAttachment[] attachFiles(
      AddReasonType reason,
      string[] fileList,
      string title,
      FileAttachment[] sourceList,
      PageIdentity[] identityList,
      DocumentLog[] docList)
    {
      string[] array = new string[12]
      {
        ".pdf",
        ".doc",
        ".docx",
        ".txt",
        ".tif",
        ".jpg",
        ".jpeg",
        ".jpe",
        ".emf",
        ".htm",
        ".html",
        ".xps"
      };
      for (int index = 0; index < fileList.Length; ++index)
      {
        string file = fileList[index];
        string str = Path.GetExtension(file).ToLower().Trim();
        if (Array.IndexOf<string>(array, str) < 0)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "The '" + str + "' file type is not supported. The allowed file types are '.pdf', '.doc', '.docx', '.txt', '.tif', '.jpg', '.jpeg', '.jpe', '.emf', and '.xps'.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return (FileAttachment[]) null;
        }
        if (PasswordManager.IsPasswordProtected(file))
        {
          fileList[index] = this.removePasswordProtection(file);
          if (fileList[index] == null)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "The file '" + file + " is password protected and cannot be opened.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return (FileAttachment[]) null;
          }
        }
      }
      int fileCount = this.getFileCount(fileList);
      if (fileCount == 0)
        return (FileAttachment[]) null;
      this.attachedList = new List<FileAttachment>();
      this.reason = reason;
      this.fileList = fileList;
      this.docList = docList;
      this.title = title;
      this.sourceList = sourceList;
      this.identityList = identityList;
      this.pbOverall.Maximum = fileCount;
      Tracing.Log(AttachFilesDialog.sw, TraceLevel.Verbose, nameof (AttachFilesDialog), "Starting Thread");
      this.worker.RunWorkerAsync();
      Tracing.Log(AttachFilesDialog.sw, TraceLevel.Verbose, nameof (AttachFilesDialog), "Showing Dialog");
      return this.ShowDialog((IWin32Window) Form.ActiveForm) != DialogResult.OK ? (FileAttachment[]) null : this.attachedList.ToArray();
    }

    private int getFileCount(string[] fileList)
    {
      if (this.loanDataMgr.SystemConfiguration.ImageAttachmentSettings.UseImageAttachments || this.loanDataMgr.UseSkyDrive)
        return fileList.Length;
      int fileCount = 0;
      foreach (string file in fileList)
      {
        string lower = new FileInfo(file).Extension.ToLower();
        if (lower == ".tif" || lower == ".jpg" || lower == ".jpeg" || lower == ".jpe")
        {
          using (Image image = Image.FromFile(file))
            fileCount += image.GetFrameCount(FrameDimension.Page);
        }
        else
          ++fileCount;
      }
      return fileCount;
    }

    private string removePasswordProtection(string filePath)
    {
      using (PasswordDialog passwordDialog = new PasswordDialog(filePath))
      {
        int num = (int) passwordDialog.ShowDialog((IWin32Window) Form.ActiveForm);
        return passwordDialog.DialogResult == DialogResult.OK ? passwordDialog.UnProtectedFilePath : (string) null;
      }
    }

    private void addFile(
      string filepath,
      string title,
      PageIdentity[] pageIdentityList,
      DocumentLog doc)
    {
      DocumentIdentity[] identityList = (DocumentIdentity[]) null;
      if (pageIdentityList != null)
      {
        identityList = new DocumentIdentity[pageIdentityList.Length];
        for (int index = 0; index < pageIdentityList.Length; ++index)
        {
          PageIdentity pageIdentity = pageIdentityList[index];
          identityList[index] = new DocumentIdentity(pageIdentity.PageIndex, pageIdentity.DocumentType, pageIdentity.DocumentSource);
        }
      }
      this.transferStartTime = DateTime.Now;
      Tracing.Log(AttachFilesDialog.sw, TraceLevel.Verbose, nameof (AttachFilesDialog), "Adding File: " + filepath);
      this.attachedList.Add(this.loanDataMgr.FileAttachments.AddAttachment(this.reason, filepath, title, doc, identityList));
      this.worker.ReportProgress(100, (object) this.pbTransfer);
      this.worker.ReportProgress(this.attachedList.Count, (object) this.pbOverall);
    }

    private void addImage(string imageFile, string title, DocumentLog doc)
    {
      if (this.loanDataMgr.SystemConfiguration.ImageAttachmentSettings.UseImageAttachments || this.loanDataMgr.UseSkyDrive)
      {
        this.addFile(imageFile, title, this.identityList, doc);
      }
      else
      {
        DocumentLog[] allDocuments = this.loanDataMgr.LoanData.GetLogList().GetAllDocuments();
        Tracing.Log(AttachFilesDialog.sw, TraceLevel.Verbose, nameof (AttachFilesDialog), "Loading Image: " + imageFile);
        using (Image image = Image.FromFile(imageFile))
        {
          int frameCount = image.GetFrameCount(FrameDimension.Page);
          Tracing.Log(AttachFilesDialog.sw, TraceLevel.Verbose, nameof (AttachFilesDialog), "GetFrameCount: " + (object) frameCount);
          int num = 0;
          for (int frameIndex = 0; frameIndex < frameCount && !this.worker.CancellationPending; ++frameIndex)
          {
            Tracing.Log(AttachFilesDialog.sw, TraceLevel.Verbose, nameof (AttachFilesDialog), "Selecting Page: " + (object) frameIndex);
            image.SelectActiveFrame(FrameDimension.Page, frameIndex);
            string nameWithExtension;
            if (image.Palette.Entries.Length == 2)
            {
              nameWithExtension = SystemSettings.GetTempFileNameWithExtension("tif");
              Tracing.Log(AttachFilesDialog.sw, TraceLevel.Verbose, nameof (AttachFilesDialog), "Saving Image: " + nameWithExtension);
              image.Save(nameWithExtension, ImageFormat.Tiff);
            }
            else
            {
              nameWithExtension = SystemSettings.GetTempFileNameWithExtension("jpg");
              Tracing.Log(AttachFilesDialog.sw, TraceLevel.Verbose, nameof (AttachFilesDialog), "Saving Image: " + nameWithExtension);
              image.Save(nameWithExtension, ImageFormat.Jpeg);
            }
            string title1 = title;
            if (frameCount > 1)
            {
              ++num;
              title1 = title1 + " - Page " + num.ToString();
            }
            PageIdentity[] pageIdentityList1 = (PageIdentity[]) null;
            if (this.identityList != null)
            {
              List<PageIdentity> pageIdentityList2 = new List<PageIdentity>();
              foreach (PageIdentity identity in this.identityList)
              {
                if (identity.PageIndex == frameIndex)
                {
                  PageIdentity pageIdentity = new PageIdentity();
                  pageIdentity.PageIndex = 0;
                  pageIdentity.DocumentType = identity.DocumentType;
                  pageIdentity.DocumentSource = identity.DocumentSource;
                  pageIdentity.Title = identity.Title;
                  pageIdentityList2.Add(pageIdentity);
                  if (pageIdentity.DocumentType == "Document")
                  {
                    DocumentLog barcodedDocument = PdfEditor.GetBarcodedDocument(pageIdentity.DocumentSource, allDocuments);
                    if (barcodedDocument != null)
                      title1 = barcodedDocument.Title;
                  }
                }
              }
              pageIdentityList1 = pageIdentityList2.ToArray();
            }
            this.addFile(nameWithExtension, title1, pageIdentityList1, doc);
          }
        }
      }
    }

    private void uploadProgress(object source, TransferProgressEventArgs e)
    {
      if (this.worker.CancellationPending)
        e.Cancel = true;
      this.worker.ReportProgress(e.PercentCompleted, (object) this.pbTransfer);
    }

    private void worker_DoWork(object sender, DoWorkEventArgs e)
    {
      Tracing.Log(AttachFilesDialog.sw, TraceLevel.Verbose, nameof (AttachFilesDialog), "Thread Started");
      NativeAttachment attachment = (NativeAttachment) null;
      if (this.sourceList != null && this.sourceList.Length != 0)
        attachment = this.sourceList[0] as NativeAttachment;
      DocumentLog doc = (DocumentLog) null;
      if (this.docList != null && this.docList.Length != 0)
        doc = this.docList[0];
      this.transferStartTime = DateTime.Now;
      this.loanDataMgr.FileAttachments.FileAttachmentUploadProgress += new TransferProgressEventHandler(this.uploadProgress);
      try
      {
        switch (this.reason)
        {
          case AddReasonType.Edit:
          case AddReasonType.Annotate:
          case AddReasonType.Conversion:
          case AddReasonType.ConversionForMerge:
            FileAttachment fileAttachment1 = this.loanDataMgr.FileAttachments.ReplaceAttachment(this.reason, attachment, this.fileList[0], doc);
            if (fileAttachment1 == null)
              break;
            this.attachedList.Add(fileAttachment1);
            break;
          case AddReasonType.Split:
            FileAttachment[] collection = !this.loanDataMgr.UseSkyDriveClassic ? this.loanDataMgr.FileAttachments.SplitAttachment(attachment, this.fileList, this.docList) : this.loanDataMgr.FileAttachments.SplitAttachmentForSDC(attachment, this.fileList, this.docList, this.pageIndexTable, this.fileListForSDC);
            if (collection == null)
              break;
            this.attachedList.AddRange((IEnumerable<FileAttachment>) collection);
            break;
          case AddReasonType.Merge:
            FileAttachment fileAttachment2 = this.loanDataMgr.FileAttachments.MergeAttachments(this.sourceList, this.fileList[0], doc);
            if (fileAttachment2 == null)
              break;
            this.attachedList.Add(fileAttachment2);
            break;
          default:
            foreach (string file in this.fileList)
            {
              FileInfo fileInfo = new FileInfo(file);
              string title = this.title ?? fileInfo.Name;
              switch (fileInfo.Extension.ToLower())
              {
                case ".tif":
                  this.addImage(file, title, doc);
                  break;
                case ".jpg":
                  this.addImage(file, title, doc);
                  break;
                case ".jpeg":
                  this.addImage(file, title, doc);
                  break;
                case ".jpe":
                  this.addImage(file, title, doc);
                  break;
                default:
                  this.addFile(file, title, this.identityList, doc);
                  break;
              }
            }
            break;
        }
      }
      catch (Exception ex)
      {
        if (ex.InnerException is CanceledOperationException)
          Tracing.Log(AttachFilesDialog.sw, TraceLevel.Verbose, nameof (AttachFilesDialog), "Transfer Cancelled");
        else
          throw;
      }
      finally
      {
        this.loanDataMgr.FileAttachments.FileAttachmentUploadProgress -= new TransferProgressEventHandler(this.uploadProgress);
      }
    }

    private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
      if (!(e.UserState is ProgressBar userState))
        return;
      userState.Value = e.ProgressPercentage;
      if (userState != this.pbTransfer || e.ProgressPercentage <= 0)
        return;
      double num = TimeSpan.FromTicks(DateTime.Now.Ticks - this.transferStartTime.Ticks).TotalMilliseconds / Convert.ToDouble(e.ProgressPercentage);
      TimeSpan timeSpan = TimeSpan.FromMilliseconds(Convert.ToDouble(this.pbTransfer.Maximum - e.ProgressPercentage) * num);
      if (timeSpan.TotalSeconds >= 120.0)
        this.lblTimeRemaining.Text = "About " + Convert.ToInt32(timeSpan.TotalMinutes).ToString() + " minutes remaining";
      else if (timeSpan.TotalSeconds > 60.0)
        this.lblTimeRemaining.Text = "About 1 minute and " + Convert.ToInt32(timeSpan.TotalSeconds - 60.0).ToString() + " seconds remaining";
      else
        this.lblTimeRemaining.Text = "About " + timeSpan.Seconds.ToString() + " seconds remaining";
      this.lblTimeRemaining.Visible = true;
    }

    private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      if (e.Error != null)
      {
        Tracing.Log(AttachFilesDialog.sw, TraceLevel.Error, nameof (AttachFilesDialog), e.Error.ToString());
        string str = string.Empty;
        if (e.Error.InnerException != null && e.Error.InnerException.Message.LastIndexOf("Correlation") >= 0)
          str = e.Error.InnerException.Message.Substring(e.Error.InnerException.Message.LastIndexOf("Correlation"));
        int num = (int) Utils.Dialog((IWin32Window) this, "Something went wrong trying to upload the file. Please try again.\n\nIf the error persists, please contact ICE Mortgage Technology support.\n\n" + str, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      Tracing.Log(AttachFilesDialog.sw, TraceLevel.Verbose, nameof (AttachFilesDialog), "Closing Window");
      if (this.attachedList.Count > 0)
        this.DialogResult = DialogResult.OK;
      else
        this.DialogResult = DialogResult.Cancel;
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      Tracing.Log(AttachFilesDialog.sw, TraceLevel.Verbose, nameof (AttachFilesDialog), "Cancelling");
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
      this.lblOverall = new Label();
      this.pbOverall = new ProgressBar();
      this.worker = new BackgroundWorker();
      this.lblPageCount = new Label();
      this.lblTimeRemaining = new Label();
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
      this.btnCancel.TabIndex = 5;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.lblOverall.AutoSize = true;
      this.lblOverall.Location = new Point(12, 12);
      this.lblOverall.Name = "lblOverall";
      this.lblOverall.Size = new Size(97, 14);
      this.lblOverall.TabIndex = 0;
      this.lblOverall.Text = "Overall Progress...";
      this.pbOverall.Location = new Point(12, 28);
      this.pbOverall.Name = "pbOverall";
      this.pbOverall.Size = new Size(374, 16);
      this.pbOverall.TabIndex = 1;
      this.worker.WorkerReportsProgress = true;
      this.worker.WorkerSupportsCancellation = true;
      this.worker.DoWork += new DoWorkEventHandler(this.worker_DoWork);
      this.worker.ProgressChanged += new ProgressChangedEventHandler(this.worker_ProgressChanged);
      this.worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.worker_RunWorkerCompleted);
      this.lblPageCount.AutoSize = true;
      this.lblPageCount.Location = new Point(12, 65);
      this.lblPageCount.Name = "lblPageCount";
      this.lblPageCount.Size = new Size(84, 14);
      this.lblPageCount.TabIndex = 4;
      this.lblPageCount.Text = "Pages Complete";
      this.lblPageCount.Visible = false;
      this.lblTimeRemaining.AutoSize = true;
      this.lblTimeRemaining.Location = new Point(12, 92);
      this.lblTimeRemaining.Name = "lblTimeRemaining";
      this.lblTimeRemaining.Size = new Size(149, 14);
      this.lblTimeRemaining.TabIndex = 6;
      this.lblTimeRemaining.Text = "About N seconds remaining...";
      this.lblTimeRemaining.Visible = false;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(398, 121);
      this.ControlBox = false;
      this.Controls.Add((Control) this.lblTimeRemaining);
      this.Controls.Add((Control) this.lblPageCount);
      this.Controls.Add((Control) this.lblTransfer);
      this.Controls.Add((Control) this.pbTransfer);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.lblOverall);
      this.Controls.Add((Control) this.pbOverall);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AttachFilesDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Attaching Document";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
