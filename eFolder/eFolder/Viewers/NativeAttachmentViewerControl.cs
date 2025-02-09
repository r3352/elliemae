// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Viewers.NativeAttachmentViewerControl
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.ClientServer.eFolder;
using EllieMae.EMLite.ClientServer.eFolder.SkyDriveClassic;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.DocumentConverter;
using EllieMae.EMLite.eFolder.Files;
using EllieMae.EMLite.eFolder.Properties;
using EllieMae.EMLite.JedScriptEngine;
using EllieMae.EMLite.LoanUtils.eFolder.SkyDriveClassic;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.Viewers
{
  public class NativeAttachmentViewerControl : UserControl
  {
    private const string className = "NativeAttachmentViewerControl";
    private static readonly string sw = Tracing.SwEFolder;
    private static bool? autoConversionSetting;
    private FileAttachment[] attachmentList;
    private FileAttachment[] newAttachmentList;
    private bool loadAttachments;
    private string pdfFile;
    private bool autoConversionEnabled;
    private long startTicks;
    private IContainer components;
    private ToolTip tooltip;
    private BackgroundWorker worker;
    private Panel pnlViewer;
    private PdfViewerControl pdfViewer;
    private GradientPanel pnlHeader;
    private FlowLayoutPanel pnlToolbar;
    private StandardIconButton btnSave;
    private StandardIconButton btnPrint;
    private VerticalSeparator separator1;
    private IconButton btnMoveFirst;
    private StandardIconButton btnMovePrevious;
    private StandardIconButton btnMoveNext;
    private IconButton btnMoveLast;
    private VerticalSeparator separator2;
    private StandardIconButton btnZoomOut;
    private StandardIconButton btnZoomIn;
    private ComboBox cboZoom;
    private VerticalSeparator separator3;
    private IconButton btnRotate;
    private VerticalSeparator separator4;
    private Button btnOpenNative;
    private Label lblMessage;
    private IconButton btnEditFile;
    private IconButton btnSplitFile;
    private IconButton btnAnnotateFile;
    private Button btnConvert;

    public NativeAttachmentViewerControl()
    {
      this.InitializeComponent();
      this.lblMessage.Location = new Point(0, 0);
      this.pnlViewer.Location = new Point(0, 0);
      this.cboZoom.SelectedItem = (object) "Fit Width";
      this.pnlViewer.BringToFront();
    }

    public FileAttachment[] NewAttachments => this.newAttachmentList;

    private void applySecurity()
    {
      eFolderAccessRights folderAccessRights = new eFolderAccessRights(Session.LoanDataMgr, (LogRecordBase) Session.LoanDataMgr.FileAttachments.GetLinkedDocument(this.attachmentList[0].ID));
      this.btnEditFile.Visible = folderAccessRights.CanEditFile;
      this.btnSplitFile.Visible = folderAccessRights.CanSplitFiles;
      this.btnAnnotateFile.Visible = folderAccessRights.CanAnnotateFiles;
      this.btnEditFile.Enabled = this.btnSplitFile.Enabled = this.btnAnnotateFile.Enabled = this.attachmentList.Length == 1;
      this.btnConvert.Visible = folderAccessRights.CanEditFile && Session.LoanDataMgr.SystemConfiguration.ImageAttachmentSettings.UseImageAttachments;
      lock (this)
      {
        if (!NativeAttachmentViewerControl.autoConversionSetting.HasValue)
        {
          string companySetting = Session.ConfigurationManager.GetCompanySetting("POLICIES", "AUTOCONVERSION");
          NativeAttachmentViewerControl.autoConversionSetting = string.IsNullOrEmpty(companySetting) || !(companySetting.ToLower() == "false") ? new bool?(true) : new bool?(false);
        }
        this.autoConversionEnabled = NativeAttachmentViewerControl.autoConversionSetting.Value && folderAccessRights.CanEditFile && Session.LoanDataMgr.SystemConfiguration.ImageAttachmentSettings.UseImageAttachments;
      }
    }

    private void NativeAttachmentViewerControl_Resize(object sender, EventArgs e)
    {
      this.lblMessage.Size = this.ClientSize;
      this.pnlViewer.Size = this.ClientSize;
    }

    public void CloseFile()
    {
      Tracing.Log(NativeAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (NativeAttachmentViewerControl), nameof (CloseFile));
      try
      {
        if (this.attachmentList == null)
          return;
        Tracing.Log(NativeAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (NativeAttachmentViewerControl), "Clearing Attachment List");
        this.attachmentList = (FileAttachment[]) null;
        Tracing.Log(NativeAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (NativeAttachmentViewerControl), "Checking Worker");
        if (!this.worker.IsBusy)
          return;
        Tracing.Log(NativeAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (NativeAttachmentViewerControl), "Cancelling Worker");
        this.worker.CancelAsync();
        Tracing.Log(NativeAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (NativeAttachmentViewerControl), "Clearing Load Attachments Flag");
        this.loadAttachments = false;
      }
      catch (Exception ex)
      {
        Tracing.Log(NativeAttachmentViewerControl.sw, TraceLevel.Error, nameof (NativeAttachmentViewerControl), ex.ToString());
        this.showMessage("The following error occurred when trying to close the file:\n\n" + ex.Message + "\n\n" + ex.StackTrace);
      }
    }

    public void LoadFiles(FileAttachment[] attachmentList)
    {
      Tracing.Log(NativeAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (NativeAttachmentViewerControl), nameof (LoadFiles));
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
        using (PerformanceMeter.StartNew(nameof (LoadFiles), "NativeAttachmentViewerControl.LoadFiles", 190, nameof (LoadFiles), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\Viewers\\NativeAttachmentViewerControl.cs"))
        {
          this.startTicks = DateTime.Now.Ticks;
          PerformanceMeter.Current.AddCheckpoint("LoadFiles BEGIN TICKS: " + (object) this.startTicks, 195, nameof (LoadFiles), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\Viewers\\NativeAttachmentViewerControl.cs");
          Tracing.Log(NativeAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (NativeAttachmentViewerControl), "Showing Loading Message");
          this.showMessage("Preparing Document...");
          Tracing.Log(NativeAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (NativeAttachmentViewerControl), "Setting Attachment List");
          this.attachmentList = attachmentList;
          this.applySecurity();
          Tracing.Log(NativeAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (NativeAttachmentViewerControl), "Checking Worker");
          if (this.worker.IsBusy)
          {
            Tracing.Log(NativeAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (NativeAttachmentViewerControl), "Cancelling Worker");
            this.worker.CancelAsync();
            Tracing.Log(NativeAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (NativeAttachmentViewerControl), "Setting Load Attachments Flag");
            this.loadAttachments = true;
          }
          else
          {
            Tracing.Log(NativeAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (NativeAttachmentViewerControl), "Starting Worker");
            this.worker.RunWorkerAsync((object) attachmentList);
            Tracing.Log(NativeAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (NativeAttachmentViewerControl), "Clearing Load Attachments Flag");
            this.loadAttachments = false;
          }
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(NativeAttachmentViewerControl.sw, TraceLevel.Error, nameof (NativeAttachmentViewerControl), ex.ToString());
        this.showMessage("The following error occurred when trying to load the files:\n\n" + ex.Message + "\n\n" + ex.StackTrace);
      }
    }

    private void worker_DoWork(object sender, DoWorkEventArgs e)
    {
      Tracing.Log(NativeAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (NativeAttachmentViewerControl), "Worker Started");
      e.Cancel = true;
      Tracing.Log(NativeAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (NativeAttachmentViewerControl), "Getting File List");
      FileAttachment[] fileAttachmentArray = (FileAttachment[]) e.Argument;
      List<string> stringList = new List<string>();
      for (int index = 0; index < fileAttachmentArray.Length; ++index)
      {
        FileAttachment fileAttachment = fileAttachmentArray[index];
        if (this.worker.CancellationPending)
          return;
        this.worker.ReportProgress(0, (object) ("Preparing " + fileAttachment.Title + "..."));
        switch (fileAttachment)
        {
          case NativeAttachment _:
            NativeAttachment attachment = fileAttachment as NativeAttachment;
            if (this.autoConversionEnabled)
            {
              FileAttachment image = this.convertNativeToImage(attachment);
              if (image == null)
                return;
              fileAttachmentArray[index] = image;
              this.newAttachmentList = fileAttachmentArray;
              this.OnLoadAttachments(EventArgs.Empty);
              this.OnFileUpdated(EventArgs.Empty);
              return;
            }
            Session.LoanDataMgr.FileAttachments.FileAttachmentDownloadProgress += new EllieMae.EMLite.eFolder.Files.TransferProgressEventHandler(this.attachment_DownloadProgress);
            try
            {
              string empty = string.Empty;
              stringList.Add((!Session.LoanDataMgr.UseSkyDriveClassic ? new PdfFileCreator(Session.LoanDataMgr).CreateFile(attachment) : new SDCHelper(Session.LoanDataMgr).GetConvertedVersionFile((FileAttachment) attachment)) ?? throw new Exception("Unable to convert the '" + attachment.Title + "' file."));
              break;
            }
            catch (CanceledOperationException ex)
            {
              Tracing.Log(NativeAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (NativeAttachmentViewerControl), "Worker Cancelled");
              return;
            }
            finally
            {
              Session.LoanDataMgr.FileAttachments.FileAttachmentDownloadProgress -= new EllieMae.EMLite.eFolder.Files.TransferProgressEventHandler(this.attachment_DownloadProgress);
            }
          case ImageAttachment _:
            ImageAttachment imageAttachment = fileAttachment as ImageAttachment;
            Session.LoanDataMgr.FileAttachments.PageDownloaded += new ExtractProgressEventHandler(this.attachment_PageDownloaded);
            try
            {
              stringList.Add(Session.LoanDataMgr.FileAttachments.CreatePdf(imageAttachment) ?? throw new Exception("Unable to convert the '" + imageAttachment.Title + "' file."));
              break;
            }
            catch (CanceledOperationException ex)
            {
              Tracing.Log(NativeAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (NativeAttachmentViewerControl), "Worker Cancelled");
              return;
            }
            finally
            {
              Session.LoanDataMgr.FileAttachments.PageDownloaded -= new ExtractProgressEventHandler(this.attachment_PageDownloaded);
            }
        }
      }
      if (this.worker.CancellationPending)
        return;
      PdfCreator pdfCreator = new PdfCreator();
      pdfCreator.ProgressChanged += new ProgressEventHandler(this.creatorProgress);
      try
      {
        this.pdfFile = pdfCreator.MergeFiles(stringList.ToArray());
      }
      catch (CanceledOperationException ex)
      {
        Tracing.Log(NativeAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (NativeAttachmentViewerControl), "Merge cancelled");
        return;
      }
      finally
      {
        pdfCreator.ProgressChanged -= new ProgressEventHandler(this.creatorProgress);
      }
      this.worker.ReportProgress(0, (object) "Showing Document...");
      Thread.Sleep(1000);
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
      Tracing.Log(NativeAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (NativeAttachmentViewerControl), "Worker Complete");
      if (this.loadAttachments)
      {
        Tracing.Log(NativeAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (NativeAttachmentViewerControl), "Starting Worker");
        this.worker.RunWorkerAsync((object) this.attachmentList);
        Tracing.Log(NativeAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (NativeAttachmentViewerControl), "Clearing Load Files Flag");
        this.loadAttachments = false;
      }
      else if (e.Error != null)
      {
        Tracing.Log(NativeAttachmentViewerControl.sw, TraceLevel.Error, nameof (NativeAttachmentViewerControl), e.Error.ToString());
        this.showMessage("The following error occurred when trying to prepare the document:\n\n" + e.Error.Message + "\n\n" + e.Error.StackTrace);
      }
      else
      {
        if (e.Cancelled)
          return;
        this.showFile(this.pdfFile);
      }
    }

    protected override void OnHandleDestroyed(EventArgs e)
    {
      Tracing.Log(NativeAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (NativeAttachmentViewerControl), "Handle Destroyed");
      base.OnHandleDestroyed(e);
      Tracing.Log(NativeAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (NativeAttachmentViewerControl), "Checking Worker");
      if (!this.worker.IsBusy)
        return;
      Tracing.Log(NativeAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (NativeAttachmentViewerControl), "Cancelling Worker");
      this.worker.CancelAsync();
      Tracing.Log(NativeAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (NativeAttachmentViewerControl), "Clearing Load Files Flag");
      this.loadAttachments = false;
    }

    private void attachment_DownloadProgress(object source, TransferProgressEventArgs e)
    {
      if (this.worker.CancellationPending)
        e.Cancel = true;
      string userState = "Preparing Document...";
      if (e.PercentCompleted != 100)
        userState = userState + "\n\nDownloading " + (object) e.PercentCompleted + "% complete";
      this.worker.ReportProgress(0, (object) userState);
    }

    private void attachment_PageDownloaded(object source, ExtractProgressEventArgs e)
    {
      if (this.worker.CancellationPending)
        e.Cancel = true;
      string userState = "Preparing Document...";
      if (e.PercentCompleted < 100)
        userState = userState + "\n\nDownloading " + (object) e.PercentCompleted + "% complete";
      this.worker.ReportProgress(0, (object) userState);
    }

    private void creatorProgress(object source, ProgressEventArgs e)
    {
      if (this.worker.CancellationPending)
        e.Abort = true;
      string userState = "Preparing Document...";
      if (e.CurrentIndex != e.TotalCount)
        userState = userState + "\n\nMerging " + (object) e.PercentCompleted + "% complete";
      this.worker.ReportProgress(0, (object) userState);
    }

    private void showFile(string filepath)
    {
      using (PerformanceMeter.StartNew(nameof (showFile), "NativeAttachmentViewerControl.showFile", 521, nameof (showFile), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\Viewers\\NativeAttachmentViewerControl.cs"))
      {
        Tracing.Log(NativeAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (NativeAttachmentViewerControl), "showFile: " + filepath);
        try
        {
          this.pdfViewer.LoadFile(filepath);
          this.applyZoom();
          this.pnlViewer.BringToFront();
          if (this.attachmentList == null || this.attachmentList.Length <= 1)
            return;
          string sw = NativeAttachmentViewerControl.sw;
          object[] objArray1 = new object[5]
          {
            (object) "Viewing NativeAttachments: ",
            null,
            null,
            null,
            null
          };
          DateTime now = DateTime.Now;
          objArray1[1] = (object) TimeSpan.FromTicks(now.Ticks - this.startTicks).TotalMilliseconds;
          objArray1[2] = (object) " ms, ";
          objArray1[3] = (object) this.attachmentList.Length;
          objArray1[4] = (object) " files";
          string msg = string.Concat(objArray1);
          Tracing.Log(sw, TraceLevel.Info, nameof (NativeAttachmentViewerControl), msg);
          PerformanceMeter current = PerformanceMeter.Current;
          object[] objArray2 = new object[5]
          {
            (object) "Viewing NativeAttachments END TICKS: ",
            null,
            null,
            null,
            null
          };
          now = DateTime.Now;
          objArray2[1] = (object) TimeSpan.FromTicks(now.Ticks - this.startTicks).TotalMilliseconds;
          objArray2[2] = (object) " ms, ";
          objArray2[3] = (object) this.attachmentList.Length;
          objArray2[4] = (object) " files";
          string description = string.Concat(objArray2);
          current.AddCheckpoint(description, 538, nameof (showFile), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\Viewers\\NativeAttachmentViewerControl.cs");
        }
        catch (Exception ex)
        {
          Tracing.Log(NativeAttachmentViewerControl.sw, TraceLevel.Error, nameof (NativeAttachmentViewerControl), ex.ToString());
          this.showMessage("The following error occurred when trying to show the document:\n\n" + ex.Message + "\n\n" + ex.StackTrace);
        }
      }
    }

    private void showMessage(string message)
    {
      Tracing.Log(NativeAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (NativeAttachmentViewerControl), "showMessage: " + message);
      this.lblMessage.Text = message;
      this.lblMessage.BringToFront();
    }

    private FileAttachment convertNativeToImage(NativeAttachment attachment)
    {
      Session.LoanDataMgr.FileAttachments.FileAttachmentDownloadProgress += new EllieMae.EMLite.eFolder.Files.TransferProgressEventHandler(this.attachment_DownloadProgress);
      Session.LoanDataMgr.FileAttachments.FileAttachmentUploadProgress += new EllieMae.EMLite.eFolder.Files.TransferProgressEventHandler(this.attachment_UploadProgress);
      try
      {
        string filepath = Session.LoanDataMgr.FileAttachments.DownloadAttachment(attachment);
        if (this.worker.CancellationPending)
          return (FileAttachment) null;
        DocumentLog linkedDocument = Session.LoanDataMgr.FileAttachments.GetLinkedDocument(attachment.ID, false);
        return Session.LoanDataMgr.FileAttachments.ReplaceAttachment(AddReasonType.Conversion, attachment, filepath, linkedDocument, false) ?? (FileAttachment) null;
      }
      catch (CanceledOperationException ex)
      {
        Tracing.Log(NativeAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (NativeAttachmentViewerControl), "Worker Cancelled");
        return (FileAttachment) null;
      }
      finally
      {
        Session.LoanDataMgr.FileAttachments.FileAttachmentDownloadProgress -= new EllieMae.EMLite.eFolder.Files.TransferProgressEventHandler(this.attachment_DownloadProgress);
        Session.LoanDataMgr.FileAttachments.FileAttachmentUploadProgress -= new EllieMae.EMLite.eFolder.Files.TransferProgressEventHandler(this.attachment_UploadProgress);
      }
    }

    private void attachment_UploadProgress(object source, TransferProgressEventArgs e)
    {
      if (this.worker.CancellationPending)
        e.Cancel = true;
      string userState = "Preparing Document...";
      if (e.PercentCompleted != 100)
        userState = userState + "\n\nConversion " + (object) e.PercentCompleted + "% complete";
      this.worker.ReportProgress(0, (object) userState);
    }

    private void btnSave_Click(object sender, EventArgs e) => this.pdfViewer.SaveDocument();

    private void btnPrint_Click(object sender, EventArgs e) => this.pdfViewer.PrintDocument();

    private void btnMoveFirst_Click(object sender, EventArgs e) => this.pdfViewer.GotoFirstPage();

    private void btnMovePrevious_Click(object sender, EventArgs e)
    {
      this.pdfViewer.GotoPreviousPage();
    }

    private void btnMoveNext_Click(object sender, EventArgs e) => this.pdfViewer.GotoNextPage();

    private void btnMoveLast_Click(object sender, EventArgs e) => this.pdfViewer.GotoLastPage();

    private void btnZoomIn_Click(object sender, EventArgs e)
    {
      if (this.cboZoom.Text.StartsWith("Fit"))
        this.cboZoom.SelectedItem = (object) "150%";
      else
        ++this.cboZoom.SelectedIndex;
      this.applyZoom();
    }

    private void btnZoomOut_Click(object sender, EventArgs e)
    {
      if (this.cboZoom.Text.StartsWith("Fit"))
        this.cboZoom.SelectedItem = (object) "50%";
      else
        --this.cboZoom.SelectedIndex;
      this.applyZoom();
    }

    private void cboZoom_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnZoomIn.Enabled = !this.cboZoom.Text.Equals("1600%");
      this.btnZoomOut.Enabled = !this.cboZoom.Text.Equals("10%");
    }

    private void cboZoom_SelectionChangeCommitted(object sender, EventArgs e) => this.applyZoom();

    private void applyZoom()
    {
      string text = this.cboZoom.Text;
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(text))
      {
        case 258596000:
          if (!(text == "Fit Width"))
            break;
          this.pdfViewer.SetView("FitH");
          break;
        case 450565215:
          if (!(text == "25%"))
            break;
          this.pdfViewer.SetZoom(25f);
          break;
        case 546869599:
          if (!(text == "50%"))
            break;
          this.pdfViewer.SetZoom(50f);
          break;
        case 798482235:
          if (!(text == "100%"))
            break;
          this.pdfViewer.SetZoom(100f);
          break;
        case 954427916:
          if (!(text == "150%"))
            break;
          this.pdfViewer.SetZoom(150f);
          break;
        case 1412675251:
          if (!(text == "10%"))
            break;
          this.pdfViewer.SetZoom(10f);
          break;
        case 2193486807:
          if (!(text == "Fit Height"))
            break;
          this.pdfViewer.SetView("FitV");
          break;
        case 2847927124:
          if (!(text == "75%"))
            break;
          this.pdfViewer.SetZoom(75f);
          break;
        case 3631597265:
          if (!(text == "1600%"))
            break;
          this.pdfViewer.SetZoom(1600f);
          break;
        case 3685534510:
          if (!(text == "800%"))
            break;
          this.pdfViewer.SetZoom(800f);
          break;
        case 3710824936:
          if (!(text == "200%"))
            break;
          this.pdfViewer.SetZoom(200f);
          break;
        case 3769160063:
          if (!(text == "Fit Page"))
            break;
          this.pdfViewer.SetView("Fit");
          break;
        case 3885901786:
          if (!(text == "400%"))
            break;
          this.pdfViewer.SetZoom(400f);
          break;
        case 3906145400:
          if (!(text == "125%"))
            break;
          this.pdfViewer.SetZoom(125f);
          break;
      }
    }

    private void btnOpenNative_Click(object sender, EventArgs e)
    {
      Tracing.Log(NativeAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (NativeAttachmentViewerControl), "View in Original Format");
      try
      {
        List<string> stringList = new List<string>();
        foreach (FileAttachment attachment in this.attachmentList)
        {
          switch (attachment)
          {
            case NativeAttachment _:
              if ((eFolderViewInOriginalFormat) Enum.Parse(typeof (eFolderViewInOriginalFormat), string.Concat(Session.ServerManager.GetServerSetting("eFolder.ViewInOriginalFormat")), true) == eFolderViewInOriginalFormat.ViewModifiedAttachmentAsPdf)
              {
                stringList.Add(Session.LoanDataMgr.FileAttachments.DownloadAttachment((NativeAttachment) attachment));
                break;
              }
              foreach (string originalAttachment in Session.LoanDataMgr.FileAttachments.DownloadOriginalAttachments((NativeAttachment) attachment))
              {
                if (originalAttachment != null)
                  stringList.Add(originalAttachment);
              }
              break;
            case ImageAttachment _:
              IEnumerator enumerator = ((ImageAttachment) attachment).Pages.GetEnumerator();
              try
              {
                while (enumerator.MoveNext())
                {
                  string str = Session.LoanDataMgr.FileAttachments.DownloadNative((PageImage) enumerator.Current);
                  if (str != null && !stringList.Contains(str))
                    stringList.Add(str);
                }
                break;
              }
              finally
              {
                if (enumerator is IDisposable disposable)
                  disposable.Dispose();
              }
          }
        }
        if (stringList.Count == 0)
        {
          int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "There are no documents to display in original format.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        else
        {
          foreach (string str in stringList)
          {
            string tempFileName = SystemSettings.GetTempFileName(str);
            File.Copy(str, tempFileName, true);
            string lower = Path.GetExtension(tempFileName).ToLower();
            if (string.Equals(lower, ".html") || string.Equals(lower, ".htm"))
              Process.Start("notepad.exe", tempFileName);
            else
              SystemUtil.ShellExecute(tempFileName);
          }
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(NativeAttachmentViewerControl.sw, TraceLevel.Error, nameof (NativeAttachmentViewerControl), ex.ToString());
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "The following error occurred when trying to view the document in the original format:\n\n" + ex.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void btnRotate_Click(object sender, EventArgs e)
    {
      long ticks = DateTime.Now.Ticks;
      foreach (FileAttachment attachment in this.attachmentList)
      {
        switch (attachment)
        {
          case NativeAttachment _:
            NativeAttachment native = (NativeAttachment) attachment;
            if (Session.LoanDataMgr.UseSkyDriveClassic)
            {
              List<string> stringList = new List<string>();
              string convertedFile = native.GetConvertedFile();
              string filepath = (string) null;
              SDCDocument sdcDocumentCopy = native.CurrentSDCDocument == null ? Utils.DeepClone<SDCDocument>(native.OriginalSDCDocument) : Utils.DeepClone<SDCDocument>(native.CurrentSDCDocument);
              try
              {
                using (PdfEditor pdfEditor = new PdfEditor(convertedFile))
                {
                  foreach (Pages page in native.OriginalSDCDocument.Pages)
                  {
                    Tracing.Log(NativeAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (NativeAttachmentViewerControl), string.Format("Calling PageRotationMapper for pageID-{0}", (object) page.Id));
                    new SDCMapper().PageRotationMapper(sdcDocumentCopy, page.Id, 90);
                  }
                  filepath = pdfEditor.Rotate(90, convertedFile);
                }
                native.SetConvertedFile(filepath);
                native.CurrentSDCDocument = sdcDocumentCopy;
                if (Session.LoanDataMgr.IsAutosaveEnabled)
                {
                  Tracing.Log(NativeAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (NativeAttachmentViewerControl), "Calling SavePartnerFilesToSkyDrive");
                  SDCHelper sdcHelper = new SDCHelper(Session.LoanDataMgr);
                  Task.WaitAll(Task.Run((Func<Task>) (() => sdcHelper.SavePartnerFilesToSkyDrive(native))));
                  break;
                }
                break;
              }
              catch (Exception ex)
              {
                Tracing.Log(NativeAttachmentViewerControl.sw, TraceLevel.Error, nameof (NativeAttachmentViewerControl), string.Format("SkyDriveClassic: Error in rotating the file for skyDriveObjectId-{0}. Ex: {1}", (object) native.ObjectId, (object) ex));
                throw;
              }
            }
            else
            {
              native.Rotation += 90;
              break;
            }
          case ImageAttachment _:
            IEnumerator enumerator = ((ImageAttachment) attachment).Pages.GetEnumerator();
            try
            {
              while (enumerator.MoveNext())
                ((PageImage) enumerator.Current).Rotate();
              break;
            }
            finally
            {
              if (enumerator is IDisposable disposable)
                disposable.Dispose();
            }
        }
      }
      this.pdfViewer.RotateDocument(90);
      Tracing.Log(NativeAttachmentViewerControl.sw, TraceLevel.Info, nameof (NativeAttachmentViewerControl), "Rotated NativeAttachment: " + (object) TimeSpan.FromTicks(DateTime.Now.Ticks - this.startTicks).TotalMilliseconds + " ms");
      this.OnFileUpdated(EventArgs.Empty);
    }

    private void btnEditFile_Click(object sender, EventArgs e)
    {
      NativeAttachment attachment = this.attachmentList[0] as NativeAttachment;
      using (PdfFileBuilder pdfFileBuilder = new PdfFileBuilder(Session.LoanDataMgr))
      {
        string empty = string.Empty;
        string filepath = !Session.LoanDataMgr.UseSkyDriveClassic ? pdfFileBuilder.CreateFile((FileAttachment) attachment) : attachment.GetConvertedFile();
        if (filepath == null)
          return;
        using (PdfPageBuilder pdfPageBuilder = new PdfPageBuilder())
        {
          string[] pdfPageList = pdfPageBuilder.SplitFile(filepath);
          if (pdfPageList == null)
            return;
          using (EditFileDialog editFileDialog = new EditFileDialog(Session.LoanDataMgr, attachment, pdfPageList))
          {
            if (editFileDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
              return;
            this.newAttachmentList = new FileAttachment[1]
            {
              editFileDialog.File
            };
            this.OnLoadAttachments(EventArgs.Empty);
            this.OnFileUpdated(EventArgs.Empty);
          }
        }
      }
    }

    private void btnSplitFile_Click(object sender, EventArgs e)
    {
      NativeAttachment attachment = this.attachmentList[0] as NativeAttachment;
      using (PdfFileBuilder pdfFileBuilder = new PdfFileBuilder(Session.LoanDataMgr))
      {
        string empty = string.Empty;
        string filepath = !Session.LoanDataMgr.UseSkyDriveClassic ? pdfFileBuilder.CreateFile((FileAttachment) attachment) : attachment.GetConvertedFile();
        if (filepath == null)
          return;
        using (PdfPageBuilder pdfPageBuilder = new PdfPageBuilder())
        {
          string[] pdfPageList = pdfPageBuilder.SplitFile(filepath);
          if (pdfPageList == null)
            return;
          if (pdfPageList.Length <= 1)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "You cannot split a file that only has 1 page.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          }
          else if (Session.LoanDataMgr.FileAttachments.GetLinkedDocument(attachment.ID) == null)
          {
            using (SplitUnassignedFileDialog unassignedFileDialog = new SplitUnassignedFileDialog(Session.LoanDataMgr, attachment, pdfPageList))
            {
              if (unassignedFileDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
                return;
              this.newAttachmentList = unassignedFileDialog.Files;
              this.OnLoadAttachments(EventArgs.Empty);
              this.OnFileUpdated(EventArgs.Empty);
            }
          }
          else
          {
            using (SplitAssignedFileDialog assignedFileDialog = new SplitAssignedFileDialog(Session.LoanDataMgr, attachment, pdfPageList))
            {
              if (assignedFileDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
                return;
              this.newAttachmentList = assignedFileDialog.Files;
              this.OnLoadAttachments(EventArgs.Empty);
              this.OnFileUpdated(EventArgs.Empty);
            }
          }
        }
      }
    }

    private void btnAnnotateFile_Click(object sender, EventArgs e)
    {
      NativeAttachment attachment = this.attachmentList[0] as NativeAttachment;
      using (PdfFileBuilder pdfFileBuilder = new PdfFileBuilder(Session.LoanDataMgr))
      {
        string empty = string.Empty;
        string pdfFile = !Session.LoanDataMgr.UseSkyDriveClassic ? pdfFileBuilder.CreateFile((FileAttachment) attachment) : attachment.GetConvertedFile();
        if (pdfFile == null)
          return;
        using (AnnotateFileDialog annotateFileDialog = new AnnotateFileDialog(Session.LoanDataMgr, attachment, pdfFile))
        {
          if (annotateFileDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
            return;
          this.newAttachmentList = new FileAttachment[1]
          {
            annotateFileDialog.File
          };
          this.OnLoadAttachments(EventArgs.Empty);
          this.OnFileUpdated(EventArgs.Empty);
        }
      }
    }

    protected virtual void OnLoadAttachments(EventArgs e)
    {
      if (this.LoadAttachments == null)
        return;
      if (this.InvokeRequired)
        this.Invoke((Delegate) (() => this.LoadAttachments((object) this, e)));
      else
        this.LoadAttachments((object) this, e);
    }

    public event EventHandler LoadAttachments;

    protected virtual void OnFileUpdated(EventArgs e)
    {
      if (this.FileUpdated == null)
        return;
      if (this.InvokeRequired)
        this.Invoke((Delegate) (() => this.FileUpdated((object) this, e)));
      else
        this.FileUpdated((object) this, e);
    }

    public event EventHandler FileUpdated;

    private void btnConvert_Click(object sender, EventArgs e)
    {
      List<FileAttachment> fileAttachmentList = new List<FileAttachment>();
      foreach (FileAttachment attachment1 in this.attachmentList)
      {
        if (attachment1 is NativeAttachment)
        {
          NativeAttachment attachment2 = (NativeAttachment) attachment1;
          string filepath = Session.LoanDataMgr.FileAttachments.DownloadAttachment(attachment2);
          if (filepath == null)
            return;
          DocumentLog linkedDocument = Session.LoanDataMgr.FileAttachments.GetLinkedDocument(attachment2.ID);
          using (AttachFilesDialog attachFilesDialog = new AttachFilesDialog(Session.LoanDataMgr))
          {
            FileAttachment fileAttachment = attachFilesDialog.ReplaceFile(AddReasonType.Conversion, attachment2, filepath, linkedDocument);
            if (fileAttachment == null)
              return;
            fileAttachmentList.Add(fileAttachment);
          }
        }
        else
          fileAttachmentList.Add(attachment1);
      }
      this.newAttachmentList = fileAttachmentList.ToArray();
      this.OnLoadAttachments(EventArgs.Empty);
      this.OnFileUpdated(EventArgs.Empty);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (NativeAttachmentViewerControl));
      this.tooltip = new ToolTip(this.components);
      this.btnSave = new StandardIconButton();
      this.btnPrint = new StandardIconButton();
      this.btnMoveFirst = new IconButton();
      this.btnMovePrevious = new StandardIconButton();
      this.btnMoveNext = new StandardIconButton();
      this.btnMoveLast = new IconButton();
      this.btnZoomOut = new StandardIconButton();
      this.btnZoomIn = new StandardIconButton();
      this.btnRotate = new IconButton();
      this.btnSplitFile = new IconButton();
      this.btnEditFile = new IconButton();
      this.btnAnnotateFile = new IconButton();
      this.worker = new BackgroundWorker();
      this.pnlViewer = new Panel();
      this.pdfViewer = new PdfViewerControl();
      this.pnlHeader = new GradientPanel();
      this.pnlToolbar = new FlowLayoutPanel();
      this.separator1 = new VerticalSeparator();
      this.separator2 = new VerticalSeparator();
      this.cboZoom = new ComboBox();
      this.separator3 = new VerticalSeparator();
      this.separator4 = new VerticalSeparator();
      this.btnOpenNative = new Button();
      this.lblMessage = new Label();
      this.btnConvert = new Button();
      ((ISupportInitialize) this.btnSave).BeginInit();
      ((ISupportInitialize) this.btnPrint).BeginInit();
      ((ISupportInitialize) this.btnMoveFirst).BeginInit();
      ((ISupportInitialize) this.btnMovePrevious).BeginInit();
      ((ISupportInitialize) this.btnMoveNext).BeginInit();
      ((ISupportInitialize) this.btnMoveLast).BeginInit();
      ((ISupportInitialize) this.btnZoomOut).BeginInit();
      ((ISupportInitialize) this.btnZoomIn).BeginInit();
      ((ISupportInitialize) this.btnRotate).BeginInit();
      ((ISupportInitialize) this.btnSplitFile).BeginInit();
      ((ISupportInitialize) this.btnEditFile).BeginInit();
      ((ISupportInitialize) this.btnAnnotateFile).BeginInit();
      this.pnlViewer.SuspendLayout();
      this.pnlHeader.SuspendLayout();
      this.pnlToolbar.SuspendLayout();
      this.SuspendLayout();
      this.btnSave.BackColor = Color.Transparent;
      this.btnSave.Location = new Point(0, 3);
      this.btnSave.Margin = new Padding(0, 3, 4, 0);
      this.btnSave.MouseDownImage = (Image) null;
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(16, 16);
      this.btnSave.StandardButtonType = StandardIconButton.ButtonType.SaveButton;
      this.btnSave.TabIndex = 28;
      this.btnSave.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnSave, "Save To Disk");
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.btnPrint.BackColor = Color.Transparent;
      this.btnPrint.Location = new Point(20, 3);
      this.btnPrint.Margin = new Padding(0, 3, 4, 0);
      this.btnPrint.MouseDownImage = (Image) null;
      this.btnPrint.Name = "btnPrint";
      this.btnPrint.Size = new Size(16, 16);
      this.btnPrint.StandardButtonType = StandardIconButton.ButtonType.PrintButton;
      this.btnPrint.TabIndex = 27;
      this.btnPrint.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnPrint, "Print Document");
      this.btnPrint.Click += new EventHandler(this.btnPrint_Click);
      this.btnMoveFirst.BackColor = Color.Transparent;
      this.btnMoveFirst.DisabledImage = (Image) componentResourceManager.GetObject("btnMoveFirst.DisabledImage");
      this.btnMoveFirst.Image = (Image) componentResourceManager.GetObject("btnMoveFirst.Image");
      this.btnMoveFirst.Location = new Point(45, 3);
      this.btnMoveFirst.Margin = new Padding(0, 3, 4, 0);
      this.btnMoveFirst.MouseDownImage = (Image) null;
      this.btnMoveFirst.MouseOverImage = (Image) componentResourceManager.GetObject("btnMoveFirst.MouseOverImage");
      this.btnMoveFirst.Name = "btnMoveFirst";
      this.btnMoveFirst.Size = new Size(16, 16);
      this.btnMoveFirst.TabIndex = 12;
      this.btnMoveFirst.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnMoveFirst, "First Page");
      this.btnMoveFirst.Click += new EventHandler(this.btnMoveFirst_Click);
      this.btnMovePrevious.BackColor = Color.Transparent;
      this.btnMovePrevious.Location = new Point(65, 3);
      this.btnMovePrevious.Margin = new Padding(0, 3, 4, 0);
      this.btnMovePrevious.MouseDownImage = (Image) null;
      this.btnMovePrevious.Name = "btnMovePrevious";
      this.btnMovePrevious.Size = new Size(16, 16);
      this.btnMovePrevious.StandardButtonType = StandardIconButton.ButtonType.MovePreviousButton;
      this.btnMovePrevious.TabIndex = 25;
      this.btnMovePrevious.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnMovePrevious, "Previous Page");
      this.btnMovePrevious.Click += new EventHandler(this.btnMovePrevious_Click);
      this.btnMoveNext.BackColor = Color.Transparent;
      this.btnMoveNext.Location = new Point(85, 3);
      this.btnMoveNext.Margin = new Padding(0, 3, 4, 0);
      this.btnMoveNext.MouseDownImage = (Image) null;
      this.btnMoveNext.Name = "btnMoveNext";
      this.btnMoveNext.Size = new Size(16, 16);
      this.btnMoveNext.StandardButtonType = StandardIconButton.ButtonType.MoveNextButton;
      this.btnMoveNext.TabIndex = 26;
      this.btnMoveNext.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnMoveNext, "Next Page");
      this.btnMoveNext.Click += new EventHandler(this.btnMoveNext_Click);
      this.btnMoveLast.BackColor = Color.Transparent;
      this.btnMoveLast.DisabledImage = (Image) componentResourceManager.GetObject("btnMoveLast.DisabledImage");
      this.btnMoveLast.Image = (Image) componentResourceManager.GetObject("btnMoveLast.Image");
      this.btnMoveLast.Location = new Point(105, 3);
      this.btnMoveLast.Margin = new Padding(0, 3, 4, 0);
      this.btnMoveLast.MouseDownImage = (Image) null;
      this.btnMoveLast.MouseOverImage = (Image) componentResourceManager.GetObject("btnMoveLast.MouseOverImage");
      this.btnMoveLast.Name = "btnMoveLast";
      this.btnMoveLast.Size = new Size(16, 16);
      this.btnMoveLast.TabIndex = 15;
      this.btnMoveLast.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnMoveLast, "Last Page");
      this.btnMoveLast.Click += new EventHandler(this.btnMoveLast_Click);
      this.btnZoomOut.BackColor = Color.Transparent;
      this.btnZoomOut.Location = new Point(130, 3);
      this.btnZoomOut.Margin = new Padding(0, 3, 4, 0);
      this.btnZoomOut.MouseDownImage = (Image) null;
      this.btnZoomOut.Name = "btnZoomOut";
      this.btnZoomOut.Size = new Size(16, 16);
      this.btnZoomOut.StandardButtonType = StandardIconButton.ButtonType.ZoomOutButton;
      this.btnZoomOut.TabIndex = 17;
      this.btnZoomOut.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnZoomOut, "Zoom Out");
      this.btnZoomOut.Click += new EventHandler(this.btnZoomOut_Click);
      this.btnZoomIn.BackColor = Color.Transparent;
      this.btnZoomIn.Location = new Point(150, 3);
      this.btnZoomIn.Margin = new Padding(0, 3, 4, 0);
      this.btnZoomIn.MouseDownImage = (Image) null;
      this.btnZoomIn.Name = "btnZoomIn";
      this.btnZoomIn.Size = new Size(16, 16);
      this.btnZoomIn.StandardButtonType = StandardIconButton.ButtonType.ZoomInButton;
      this.btnZoomIn.TabIndex = 18;
      this.btnZoomIn.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnZoomIn, "Zoom In");
      this.btnZoomIn.Click += new EventHandler(this.btnZoomIn_Click);
      this.btnRotate.BackColor = Color.Transparent;
      this.btnRotate.DisabledImage = (Image) Resources.rotate_disabled;
      this.btnRotate.Image = (Image) Resources.rotate;
      this.btnRotate.Location = new Point(315, 3);
      this.btnRotate.Margin = new Padding(0, 3, 4, 0);
      this.btnRotate.MouseDownImage = (Image) null;
      this.btnRotate.MouseOverImage = (Image) Resources.rotate_over;
      this.btnRotate.Name = "btnRotate";
      this.btnRotate.Size = new Size(16, 16);
      this.btnRotate.TabIndex = 31;
      this.btnRotate.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnRotate, "Rotate Document");
      this.btnRotate.Click += new EventHandler(this.btnRotate_Click);
      this.btnSplitFile.BackColor = Color.Transparent;
      this.btnSplitFile.DisabledImage = (Image) Resources.split_disabled;
      this.btnSplitFile.Enabled = false;
      this.btnSplitFile.Image = (Image) Resources.split;
      this.btnSplitFile.Location = new Point(275, 3);
      this.btnSplitFile.Margin = new Padding(0, 3, 4, 0);
      this.btnSplitFile.MouseDownImage = (Image) null;
      this.btnSplitFile.MouseOverImage = (Image) Resources.split_over;
      this.btnSplitFile.Name = "btnSplitFile";
      this.btnSplitFile.Size = new Size(16, 16);
      this.btnSplitFile.TabIndex = 53;
      this.btnSplitFile.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnSplitFile, "Split File");
      this.btnSplitFile.Click += new EventHandler(this.btnSplitFile_Click);
      this.btnEditFile.BackColor = Color.Transparent;
      this.btnEditFile.DisabledImage = (Image) Resources.edit_attachment_disabled;
      this.btnEditFile.Enabled = false;
      this.btnEditFile.Image = (Image) Resources.edit_attachment;
      this.btnEditFile.Location = new Point((int) byte.MaxValue, 3);
      this.btnEditFile.Margin = new Padding(0, 3, 4, 0);
      this.btnEditFile.MouseDownImage = (Image) null;
      this.btnEditFile.MouseOverImage = (Image) Resources.edit_attachment_over;
      this.btnEditFile.Name = "btnEditFile";
      this.btnEditFile.Size = new Size(16, 16);
      this.btnEditFile.TabIndex = 52;
      this.btnEditFile.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnEditFile, "Edit File");
      this.btnEditFile.Click += new EventHandler(this.btnEditFile_Click);
      this.btnAnnotateFile.BackColor = Color.Transparent;
      this.btnAnnotateFile.DisabledImage = (Image) Resources.notes_efolder_disabled;
      this.btnAnnotateFile.Enabled = false;
      this.btnAnnotateFile.Image = (Image) Resources.notes_efolder;
      this.btnAnnotateFile.Location = new Point(295, 3);
      this.btnAnnotateFile.Margin = new Padding(0, 3, 4, 0);
      this.btnAnnotateFile.MouseDownImage = (Image) null;
      this.btnAnnotateFile.MouseOverImage = (Image) Resources.notes_efolder_over;
      this.btnAnnotateFile.Name = "btnAnnotateFile";
      this.btnAnnotateFile.Size = new Size(16, 16);
      this.btnAnnotateFile.TabIndex = 54;
      this.btnAnnotateFile.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnAnnotateFile, "Add Notes");
      this.btnAnnotateFile.Click += new EventHandler(this.btnAnnotateFile_Click);
      this.worker.WorkerReportsProgress = true;
      this.worker.WorkerSupportsCancellation = true;
      this.worker.DoWork += new DoWorkEventHandler(this.worker_DoWork);
      this.worker.ProgressChanged += new ProgressChangedEventHandler(this.worker_ProgressChanged);
      this.worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.worker_RunWorkerCompleted);
      this.pnlViewer.Controls.Add((Control) this.pdfViewer);
      this.pnlViewer.Controls.Add((Control) this.pnlHeader);
      this.pnlViewer.Location = new Point(152, 8);
      this.pnlViewer.Name = "pnlViewer";
      this.pnlViewer.Size = new Size(630, 191);
      this.pnlViewer.TabIndex = 3;
      this.pdfViewer.BackColor = SystemColors.AppWorkspace;
      this.pdfViewer.BackgroundImageLayout = ImageLayout.Center;
      this.pdfViewer.Dock = DockStyle.Fill;
      this.pdfViewer.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.pdfViewer.Location = new Point(0, 31);
      this.pdfViewer.Name = "pdfViewer";
      this.pdfViewer.Size = new Size(630, 160);
      this.pdfViewer.TabIndex = 10;
      this.pdfViewer.TabStop = false;
      this.pnlHeader.Borders = AnchorStyles.Bottom;
      this.pnlHeader.Controls.Add((Control) this.pnlToolbar);
      this.pnlHeader.Dock = DockStyle.Top;
      this.pnlHeader.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.pnlHeader.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.pnlHeader.Location = new Point(0, 0);
      this.pnlHeader.Name = "pnlHeader";
      this.pnlHeader.Padding = new Padding(8, 0, 0, 0);
      this.pnlHeader.Size = new Size(630, 31);
      this.pnlHeader.Style = GradientPanel.PanelStyle.PageSubHeader;
      this.pnlHeader.TabIndex = 2;
      this.pnlToolbar.BackColor = Color.Transparent;
      this.pnlToolbar.Controls.Add((Control) this.btnSave);
      this.pnlToolbar.Controls.Add((Control) this.btnPrint);
      this.pnlToolbar.Controls.Add((Control) this.separator1);
      this.pnlToolbar.Controls.Add((Control) this.btnMoveFirst);
      this.pnlToolbar.Controls.Add((Control) this.btnMovePrevious);
      this.pnlToolbar.Controls.Add((Control) this.btnMoveNext);
      this.pnlToolbar.Controls.Add((Control) this.btnMoveLast);
      this.pnlToolbar.Controls.Add((Control) this.separator2);
      this.pnlToolbar.Controls.Add((Control) this.btnZoomOut);
      this.pnlToolbar.Controls.Add((Control) this.btnZoomIn);
      this.pnlToolbar.Controls.Add((Control) this.cboZoom);
      this.pnlToolbar.Controls.Add((Control) this.separator3);
      this.pnlToolbar.Controls.Add((Control) this.btnEditFile);
      this.pnlToolbar.Controls.Add((Control) this.btnSplitFile);
      this.pnlToolbar.Controls.Add((Control) this.btnAnnotateFile);
      this.pnlToolbar.Controls.Add((Control) this.btnRotate);
      this.pnlToolbar.Controls.Add((Control) this.separator4);
      this.pnlToolbar.Controls.Add((Control) this.btnOpenNative);
      this.pnlToolbar.Controls.Add((Control) this.btnConvert);
      this.pnlToolbar.Location = new Point(8, 4);
      this.pnlToolbar.Name = "pnlToolbar";
      this.pnlToolbar.Size = new Size(607, 22);
      this.pnlToolbar.TabIndex = 3;
      this.separator1.Location = new Point(40, 3);
      this.separator1.Margin = new Padding(0, 3, 3, 3);
      this.separator1.MaximumSize = new Size(2, 16);
      this.separator1.MinimumSize = new Size(2, 16);
      this.separator1.Name = "separator1";
      this.separator1.Size = new Size(2, 16);
      this.separator1.TabIndex = 4;
      this.separator1.TabStop = false;
      this.separator2.Location = new Point(125, 3);
      this.separator2.Margin = new Padding(0, 3, 3, 3);
      this.separator2.MaximumSize = new Size(2, 16);
      this.separator2.MinimumSize = new Size(2, 16);
      this.separator2.Name = "separator2";
      this.separator2.Size = new Size(2, 16);
      this.separator2.TabIndex = 5;
      this.separator2.TabStop = false;
      this.cboZoom.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboZoom.FormattingEnabled = true;
      this.cboZoom.Items.AddRange(new object[14]
      {
        (object) "Fit Page",
        (object) "Fit Width",
        (object) "Fit Height",
        (object) "10%",
        (object) "25%",
        (object) "50%",
        (object) "75%",
        (object) "100%",
        (object) "125%",
        (object) "150%",
        (object) "200%",
        (object) "400%",
        (object) "800%",
        (object) "1600%"
      });
      this.cboZoom.Location = new Point(170, 0);
      this.cboZoom.Margin = new Padding(0, 0, 4, 0);
      this.cboZoom.Name = "cboZoom";
      this.cboZoom.Size = new Size(76, 21);
      this.cboZoom.TabIndex = 6;
      this.cboZoom.TabStop = false;
      this.cboZoom.SelectedIndexChanged += new EventHandler(this.cboZoom_SelectedIndexChanged);
      this.cboZoom.SelectionChangeCommitted += new EventHandler(this.cboZoom_SelectionChangeCommitted);
      this.separator3.Location = new Point(250, 3);
      this.separator3.Margin = new Padding(0, 3, 3, 3);
      this.separator3.MaximumSize = new Size(2, 16);
      this.separator3.MinimumSize = new Size(2, 16);
      this.separator3.Name = "separator3";
      this.separator3.Size = new Size(2, 16);
      this.separator3.TabIndex = 7;
      this.separator3.TabStop = false;
      this.separator4.Location = new Point(335, 3);
      this.separator4.Margin = new Padding(0, 3, 3, 3);
      this.separator4.MaximumSize = new Size(2, 16);
      this.separator4.MinimumSize = new Size(2, 16);
      this.separator4.Name = "separator4";
      this.separator4.Size = new Size(2, 16);
      this.separator4.TabIndex = 8;
      this.separator4.TabStop = false;
      this.btnOpenNative.BackColor = SystemColors.Control;
      this.btnOpenNative.Location = new Point(340, 0);
      this.btnOpenNative.Margin = new Padding(0, 0, 4, 0);
      this.btnOpenNative.Name = "btnOpenNative";
      this.btnOpenNative.Size = new Size(132, 22);
      this.btnOpenNative.TabIndex = 9;
      this.btnOpenNative.TabStop = false;
      this.btnOpenNative.Text = "View in Original Format";
      this.btnOpenNative.UseVisualStyleBackColor = true;
      this.btnOpenNative.Click += new EventHandler(this.btnOpenNative_Click);
      this.lblMessage.BackColor = SystemColors.AppWorkspace;
      this.lblMessage.ForeColor = SystemColors.Window;
      this.lblMessage.Location = new Point(8, 8);
      this.lblMessage.Name = "lblMessage";
      this.lblMessage.Size = new Size(128, 191);
      this.lblMessage.TabIndex = 2;
      this.lblMessage.Text = "Message";
      this.lblMessage.TextAlign = ContentAlignment.MiddleCenter;
      this.btnConvert.BackColor = SystemColors.Control;
      this.btnConvert.Location = new Point(476, 0);
      this.btnConvert.Margin = new Padding(0, 0, 4, 0);
      this.btnConvert.Name = "btnConvert";
      this.btnConvert.Size = new Size(60, 22);
      this.btnConvert.TabIndex = 10;
      this.btnConvert.TabStop = false;
      this.btnConvert.Text = "Convert";
      this.btnConvert.UseVisualStyleBackColor = true;
      this.btnConvert.Click += new EventHandler(this.btnConvert_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.pnlViewer);
      this.Controls.Add((Control) this.lblMessage);
      this.Name = nameof (NativeAttachmentViewerControl);
      this.Size = new Size(809, 215);
      this.Resize += new EventHandler(this.NativeAttachmentViewerControl_Resize);
      ((ISupportInitialize) this.btnSave).EndInit();
      ((ISupportInitialize) this.btnPrint).EndInit();
      ((ISupportInitialize) this.btnMoveFirst).EndInit();
      ((ISupportInitialize) this.btnMovePrevious).EndInit();
      ((ISupportInitialize) this.btnMoveNext).EndInit();
      ((ISupportInitialize) this.btnMoveLast).EndInit();
      ((ISupportInitialize) this.btnZoomOut).EndInit();
      ((ISupportInitialize) this.btnZoomIn).EndInit();
      ((ISupportInitialize) this.btnRotate).EndInit();
      ((ISupportInitialize) this.btnSplitFile).EndInit();
      ((ISupportInitialize) this.btnEditFile).EndInit();
      ((ISupportInitialize) this.btnAnnotateFile).EndInit();
      this.pnlViewer.ResumeLayout(false);
      this.pnlHeader.ResumeLayout(false);
      this.pnlToolbar.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
