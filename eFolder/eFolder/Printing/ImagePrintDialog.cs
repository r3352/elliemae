// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Printing.ImagePrintDialog
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.Printing
{
  public class ImagePrintDialog : Form
  {
    private const string className = "ImagePrintDialog";
    private static readonly string sw = Tracing.SwEFolder;
    private string currentFile = string.Empty;
    private Image currentImage;
    private double imageHeight;
    private double imageWidth;
    private double x;
    private double y;
    private PrintDocument printDocument;
    private ImagePrintOptions printOptions;
    private List<int> pagesToPrint;
    private int fileCount;
    private int fileIndex;
    private IContainer components;
    private Label lblProgress;
    private ProgressBar pbImagePrint;
    private Button btnCancel;
    private BackgroundWorker worker;

    public ImagePrintDialog() => this.InitializeComponent();

    public bool PrintImages(ImagePrintOptions options)
    {
      this.printOptions = options;
      Tracing.Log(ImagePrintDialog.sw, TraceLevel.Verbose, nameof (ImagePrintDialog), "Starting Thread");
      this.worker.RunWorkerAsync();
      Tracing.Log(ImagePrintDialog.sw, TraceLevel.Verbose, nameof (ImagePrintDialog), "Showing Dialog");
      return this.ShowDialog((IWin32Window) Form.ActiveForm) == DialogResult.OK;
    }

    private void worker_DoWork(object sender, DoWorkEventArgs e)
    {
      try
      {
        Tracing.Log(ImagePrintDialog.sw, TraceLevel.Verbose, nameof (ImagePrintDialog), "Creating PtinterSettings from ImagePrintOptions.");
        this.printDocument = new PrintDocument();
        this.printDocument.PrintController = (PrintController) new StandardPrintController();
        this.printDocument.PrinterSettings.PrinterName = this.printOptions.Printer;
        this.printDocument.PrinterSettings.Copies = this.printOptions.Copies;
        if (this.printOptions.DuplexVertical)
        {
          this.printDocument.DefaultPageSettings.PrinterSettings.Duplex = Duplex.Vertical;
          this.printDocument.PrinterSettings.Duplex = Duplex.Vertical;
        }
        if (this.printOptions.DuplexHorizontal)
        {
          this.printDocument.DefaultPageSettings.PrinterSettings.Duplex = Duplex.Horizontal;
          this.printDocument.PrinterSettings.Duplex = Duplex.Horizontal;
        }
        this.printDocument.QueryPageSettings += new QueryPageSettingsEventHandler(this.printDocument_QueryPageSettings);
        this.printDocument.PrintPage += new PrintPageEventHandler(this.printDocument_PrintPage);
        Tracing.Log(ImagePrintDialog.sw, TraceLevel.Verbose, nameof (ImagePrintDialog), "Determining pages to print.");
        if (this.printOptions.CurrentPage != 0)
        {
          Tracing.Log(ImagePrintDialog.sw, TraceLevel.Verbose, nameof (ImagePrintDialog), "Current Page only is specified: Page '" + (object) this.printOptions.CurrentPage + "'.");
          this.fileCount = 1;
          this.fileIndex = this.printOptions.CurrentPage - 1;
        }
        if (!string.IsNullOrEmpty(this.printOptions.PageRange))
        {
          Tracing.Log(ImagePrintDialog.sw, TraceLevel.Verbose, nameof (ImagePrintDialog), "Printing specified range of pages: '" + this.printOptions.PageRange + "'.");
          this.pagesToPrint = this.parsePageNumbers(this.printOptions.PageRange);
          this.fileCount = this.pagesToPrint.Count;
          this.fileIndex = 0;
        }
        if (this.printOptions.AllPages)
        {
          this.fileCount = this.printOptions.ImageFiles.Length;
          this.fileIndex = 0;
          Tracing.Log(ImagePrintDialog.sw, TraceLevel.Verbose, nameof (ImagePrintDialog), "Printing all pages. (" + this.fileCount.ToString() + ")");
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(ImagePrintDialog.sw, TraceLevel.Verbose, nameof (ImagePrintDialog), ex.Message);
        int num = (int) Utils.Dialog((IWin32Window) this, "An error occurred when setting up PrintDocument and file counters: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.DialogResult = DialogResult.Cancel;
        return;
      }
      this.printDocument.Print();
    }

    private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
      ((ProgressBar) e.UserState).Value = e.ProgressPercentage;
    }

    private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      if (e.Error != null)
      {
        Tracing.Log(ImagePrintDialog.sw, TraceLevel.Error, nameof (ImagePrintDialog), e.Error.ToString());
        int num = (int) Utils.Dialog((IWin32Window) this, "The following error occurred when trying to print images:\n\n" + e.Error.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      Tracing.Log(ImagePrintDialog.sw, TraceLevel.Verbose, nameof (ImagePrintDialog), "Closing Window");
      this.DialogResult = DialogResult.Cancel;
    }

    private void printDocument_QueryPageSettings(object sender, QueryPageSettingsEventArgs e)
    {
      this.currentFile = string.Empty;
      try
      {
        if (string.IsNullOrEmpty(this.printOptions.PageRange))
        {
          this.currentFile = this.printOptions.ImageFiles[this.fileIndex];
        }
        else
        {
          if (this.pagesToPrint[this.fileIndex] > this.printOptions.ImageFiles.Length)
            return;
          this.currentFile = this.printOptions.ImageFiles[this.pagesToPrint[this.fileIndex] - 1];
        }
        Tracing.Log(ImagePrintDialog.sw, TraceLevel.Verbose, nameof (ImagePrintDialog), "Creating image from file : " + this.currentFile);
        this.currentImage = Image.FromFile(this.currentFile);
        e.PageSettings.Landscape = this.currentImage.Width > this.currentImage.Height;
        string str = !(this.printOptions.PaperSize != string.Empty) ? (!e.PageSettings.Landscape ? (this.currentImage.Height > 2200 ? "Legal" : "Letter") : (this.currentImage.Width > 2200 ? "Legal" : "Letter")) : this.printOptions.PaperSize;
        PaperSize paperSize = this.GetPaperSize(str);
        if (paperSize != null)
        {
          Tracing.Log(ImagePrintDialog.sw, TraceLevel.Verbose, nameof (ImagePrintDialog), "Setting paper size to: " + str);
          e.PageSettings.PaperSize = paperSize;
        }
        this.ScaleImage(str, e.PageSettings.Landscape);
        this.createPrintLog(this.currentFile);
      }
      catch (Exception ex)
      {
        Tracing.Log(ImagePrintDialog.sw, TraceLevel.Verbose, nameof (ImagePrintDialog), ex.Message);
        int num = (int) Utils.Dialog((IWin32Window) this, "An error occurred when preparing file '" + this.currentFile + "' to print: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.DialogResult = DialogResult.Cancel;
      }
    }

    private void printDocument_PrintPage(object o, PrintPageEventArgs e)
    {
      try
      {
        if (this.currentFile == string.Empty)
        {
          e.HasMorePages = false;
        }
        else
        {
          double num = e.PageSettings.Landscape ? (double) e.PageSettings.PrintableArea.Height / (double) e.PageBounds.Width : (double) e.PageSettings.PrintableArea.Height / (double) e.PageBounds.Height;
          this.imageHeight *= num;
          this.imageWidth *= num;
          this.x *= num;
          this.y *= num;
          Tracing.Log(ImagePrintDialog.sw, TraceLevel.Verbose, nameof (ImagePrintDialog), "Printing image.");
          e.Graphics.DrawImage(this.currentImage, (int) this.x, (int) this.y, (int) this.imageWidth, (int) this.imageHeight);
          if (this.worker.CancellationPending)
            e.HasMorePages = false;
          else if (this.printOptions.CurrentPage == 0)
          {
            e.HasMorePages = this.fileIndex < this.fileCount - 1;
            this.worker.ReportProgress((this.fileIndex + 1) * 100 / this.fileCount, (object) this.pbImagePrint);
            ++this.fileIndex;
          }
          else
            e.HasMorePages = false;
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(ImagePrintDialog.sw, TraceLevel.Verbose, nameof (ImagePrintDialog), ex.Message);
        int num = (int) Utils.Dialog((IWin32Window) this, "A printing error occurred: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.DialogResult = DialogResult.Cancel;
      }
    }

    private List<int> parsePageNumbers(string input)
    {
      string[] strArray = !string.IsNullOrEmpty(input) ? input.Split(',') : throw new InvalidOperationException("Input string is empty.");
      List<int> pageNumbers = new List<int>();
      foreach (string str in strArray)
      {
        if (str.Contains("-"))
          this.parsePageRange(pageNumbers, str);
        else
          pageNumbers.Add(this.parsePageNumber(str));
      }
      pageNumbers.Sort();
      return pageNumbers;
    }

    private int parsePageNumber(string pageString)
    {
      int result;
      if (!int.TryParse(pageString, out result))
        throw new InvalidOperationException(string.Format("Page number '{0}' is not valid.", (object) pageString));
      return result;
    }

    private void parsePageRange(List<int> pageNumbers, string pageNo)
    {
      string[] strArray = pageNo.Split('-');
      int num = strArray.Length == 2 ? this.parsePageNumber(strArray[0]) : throw new InvalidOperationException(string.Format("Page range '{0}' is not valid.", (object) pageNo));
      int pageNumber = this.parsePageNumber(strArray[1]);
      if (num > pageNumber)
        throw new InvalidOperationException(string.Format("Page number {0} is greater than page number {1} in page range '{2}'", (object) num, (object) pageNumber, (object) pageNo));
      for (int index = num; index < pageNumber + 1; ++index)
        pageNumbers.Add(index);
    }

    private PaperSize GetPaperSize(string requestedSize)
    {
      PaperSize paperSize = (PaperSize) null;
      for (int index = 0; index < this.printDocument.PrinterSettings.PaperSizes.Count; ++index)
      {
        if (requestedSize == "Letter")
        {
          paperSize = this.printDocument.PrinterSettings.PaperSizes[index];
          if (paperSize.Kind == PaperKind.Letter)
            break;
        }
        if (requestedSize == "Legal")
        {
          paperSize = this.printDocument.PrinterSettings.PaperSizes[index];
          if (paperSize.Kind == PaperKind.Legal)
            break;
        }
      }
      return paperSize;
    }

    private void ScaleImage(string paperSize, bool landscapeOrientation)
    {
      double num1 = 1700.0;
      double num2 = !(paperSize == "Legal") ? 2200.0 : 2800.0;
      double num3;
      double num4;
      if (landscapeOrientation)
      {
        num3 = (double) this.currentImage.Height / num1;
        num4 = (double) this.currentImage.Width / num2;
      }
      else
      {
        num3 = (double) this.currentImage.Height / num2;
        num4 = (double) this.currentImage.Width / num1;
      }
      this.imageHeight = (double) (this.currentImage.Height / 2);
      this.imageWidth = (double) (this.currentImage.Width / 2);
      if (this.printOptions.ShrinkOversized && (num3 > 1.0 || num4 > 1.0) || this.printOptions.PrintToFit)
      {
        if (num4 > num3)
        {
          this.imageHeight = (double) (this.currentImage.Height / 2) / num4;
          this.imageWidth = (double) (this.currentImage.Width / 2) / num4;
        }
        else
        {
          this.imageHeight = (double) (this.currentImage.Height / 2) / num3;
          this.imageWidth = (double) (this.currentImage.Width / 2) / num3;
        }
      }
      this.x = 0.0;
      this.y = 0.0;
      if (!this.printOptions.ActualSize && !this.printOptions.ShrinkOversized || num3 >= 1.0 || num4 >= 1.0)
        return;
      if (landscapeOrientation)
      {
        this.x = num2 / 4.0 - this.imageWidth / 2.0;
        this.y = num1 / 4.0 - this.imageHeight / 2.0;
      }
      else
      {
        this.x = num1 / 4.0 - this.imageWidth / 2.0;
        this.y = num2 / 4.0 - this.imageHeight / 2.0;
      }
      this.x = this.x < 0.0 ? 0.0 : this.x;
      this.y = this.y < 0.0 ? 0.0 : this.y;
    }

    private void createPrintLog(string filename)
    {
      PrintLog printLog = new PrintLog(DateTime.Now);
      printLog.PrintedBy = Session.UserInfo.Userid;
      printLog.PrintedByFullName = Session.UserInfo.FullName;
      printLog.Action = PrintLog.PrintAction.Print;
      printLog.ItemList.Add((object) filename);
      Session.LoanDataMgr.LoanData.GetLogList().AddRecord((LogRecordBase) printLog);
      LoanEventLogList loanEventLog = new LoanEventLogList();
      loanEventLog.InsertNonSystemLog((LogRecordBase) printLog);
      Session.LoanDataMgr.AddLoanEventLogs(loanEventLog);
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      Tracing.Log(ImagePrintDialog.sw, TraceLevel.Verbose, nameof (ImagePrintDialog), "Cancelling");
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
      this.pbImagePrint = new ProgressBar();
      this.btnCancel = new Button();
      this.worker = new BackgroundWorker();
      this.SuspendLayout();
      this.lblProgress.AutoSize = true;
      this.lblProgress.Location = new Point(12, 15);
      this.lblProgress.Name = "lblProgress";
      this.lblProgress.Size = new Size(57, 13);
      this.lblProgress.TabIndex = 5;
      this.lblProgress.Text = "Progress...";
      this.pbImagePrint.Location = new Point(12, 31);
      this.pbImagePrint.Name = "pbImagePrint";
      this.pbImagePrint.Size = new Size(374, 16);
      this.pbImagePrint.Style = ProgressBarStyle.Continuous;
      this.pbImagePrint.TabIndex = 6;
      this.btnCancel.Location = new Point(312, 55);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 7;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.worker.WorkerReportsProgress = true;
      this.worker.WorkerSupportsCancellation = true;
      this.worker.DoWork += new DoWorkEventHandler(this.worker_DoWork);
      this.worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.worker_RunWorkerCompleted);
      this.worker.ProgressChanged += new ProgressChangedEventHandler(this.worker_ProgressChanged);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(404, 99);
      this.Controls.Add((Control) this.lblProgress);
      this.Controls.Add((Control) this.pbImagePrint);
      this.Controls.Add((Control) this.btnCancel);
      this.Name = nameof (ImagePrintDialog);
      this.Text = "Printing Document";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
