// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Printing.ImagePrintOptionsDialog
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using CoolPrintPreview;
using EllieMae.EMLite.Common;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.Printing
{
  public class ImagePrintOptionsDialog : Form
  {
    private const string className = "ImagePrintOptionsDialog";
    private static readonly string sw = Tracing.SwEFolder;
    private string[] filesToPrint;
    private Image currentImage;
    private double imageHeight;
    private double imageWidth;
    private double x;
    private double y;
    private ImagePrintOptions printOptions;
    private IContainer components;
    private ComboBox cboPrinter;
    private Label label1;
    private Button btnPrint;
    private Button btnCancel;
    private Button btnPrintSettings;
    private NumericUpDown nudCopies;
    private Label label2;
    private GroupBox groupBox1;
    private RadioButton rdoPageRange;
    private RadioButton rdoCurrentPage;
    private RadioButton rdoAllPages;
    private TextBox txtPageRange;
    private TrackBar tBar1;
    private Button btnPreviousPage;
    private Button btnNextPage;
    private Label lblPagePosition;
    private GroupBox groupBox2;
    private Label label3;
    private RadioButton rdoShrink;
    private RadioButton rdoActualSize;
    private RadioButton rdoFit;
    private Label label4;
    private ComboBox cboPaperSize;
    private CheckBox chkPaperSelect;
    private Label lblPaperDimensions;
    private PrintDocument printDocument1;
    private CheckBox chkDuplex;
    private OpenFileDialog openFileDialog1;
    private Button btnOpenFiles;
    private GroupBox groupBoxFlipOptions;
    private RadioButton rdoShortEdge;
    private RadioButton rdoLongEdge;
    private CoolPrintPreviewControl printPreviewLandscape;
    private CoolPrintPreviewControl printPreviewPortrait;

    [DllImport("winspool.Drv", EntryPoint = "DocumentPropertiesW", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
    private static extern int DocumentProperties(
      IntPtr hwnd,
      IntPtr hPrinter,
      [MarshalAs(UnmanagedType.LPWStr)] string pDeviceName,
      IntPtr pDevModeOutput,
      ref IntPtr pDevModeInput,
      int fMode);

    [DllImport("kernel32.dll")]
    private static extern IntPtr GlobalLock(IntPtr hMem);

    [DllImport("kernel32.dll")]
    private static extern bool GlobalUnlock(IntPtr hMem);

    [DllImport("kernel32.dll")]
    private static extern bool GlobalFree(IntPtr hMem);

    public ImagePrintOptionsDialog(string[] imageFiles)
    {
      this.InitializeComponent();
      Tracing.Log(ImagePrintOptionsDialog.sw, TraceLevel.Verbose, nameof (ImagePrintOptionsDialog), "Load Printers.");
      try
      {
        this.printDocument1.PrintController = (PrintController) new PreviewPrintController();
        string printerName = this.printDocument1.PrinterSettings.PrinterName;
        foreach (string installedPrinter in PrinterSettings.InstalledPrinters)
        {
          this.cboPrinter.Items.Add((object) installedPrinter);
          if (installedPrinter == printerName)
            this.cboPrinter.SelectedIndex = this.cboPrinter.Items.IndexOf((object) installedPrinter);
        }
        this.cboPaperSize.SelectedIndex = 0;
      }
      catch (Exception ex)
      {
        Tracing.Log(ImagePrintOptionsDialog.sw, TraceLevel.Verbose, nameof (ImagePrintOptionsDialog), ex.Message);
        int num = (int) Utils.Dialog((IWin32Window) this, "Error loading available printers: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.DialogResult = DialogResult.Cancel;
        return;
      }
      if (imageFiles.Length != 0)
      {
        this.filesToPrint = imageFiles;
        this.lblPagePosition.Text = "Page 1 of " + imageFiles.Length.ToString();
        this.tBar1.Maximum = imageFiles.Length;
        if (imageFiles.Length >= 5)
          this.tBar1.LargeChange = imageFiles.Length / 5;
        this.tBar1.Value = 1;
        this.RefreshImageView();
      }
      else
      {
        Tracing.Log(ImagePrintOptionsDialog.sw, TraceLevel.Verbose, nameof (ImagePrintOptionsDialog), "No images specified for preview/printing.");
        int num = (int) Utils.Dialog((IWin32Window) this, "No images specified for preview/printing.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.DialogResult = DialogResult.Cancel;
      }
    }

    public ImagePrintOptions PrintOptions => this.printOptions;

    private void cboPrinter_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.printDocument1.PrinterSettings.PrinterName = this.cboPrinter.SelectedItem.ToString();
      this.chkDuplex.Enabled = this.printDocument1.PrinterSettings.CanDuplex;
      if (!this.chkDuplex.Enabled)
        this.chkDuplex.Checked = false;
      this.groupBoxFlipOptions.Enabled = this.chkDuplex.Checked;
    }

    private void btnPrintSettings_Click(object sender, EventArgs e)
    {
      Tracing.Log(ImagePrintOptionsDialog.sw, TraceLevel.Verbose, nameof (ImagePrintOptionsDialog), "Open Printer Properties for: " + this.cboPrinter.SelectedItem.ToString());
      try
      {
        this.OpenPrinterPropertiesDialog(new PrinterSettings()
        {
          PrinterName = this.cboPrinter.SelectedItem.ToString()
        });
      }
      catch (Exception ex)
      {
        Tracing.Log(ImagePrintOptionsDialog.sw, TraceLevel.Verbose, nameof (ImagePrintOptionsDialog), ex.Message);
        int num = (int) Utils.Dialog((IWin32Window) this, "The following error occurred when trying to open Printer Properties for '" + this.cboPrinter.SelectedItem.ToString() + "': " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.DialogResult = DialogResult.Cancel;
      }
    }

    private void OpenPrinterPropertiesDialog(PrinterSettings printerSettings)
    {
      IntPtr zero = IntPtr.Zero;
      int cb = ImagePrintOptionsDialog.DocumentProperties(this.Handle, IntPtr.Zero, printerSettings.PrinterName, IntPtr.Zero, ref zero, 0);
      IntPtr hdevmode = printerSettings.GetHdevmode(printerSettings.DefaultPageSettings);
      IntPtr pDevModeInput = ImagePrintOptionsDialog.GlobalLock(hdevmode);
      IntPtr num = Marshal.AllocHGlobal(cb);
      ImagePrintOptionsDialog.DocumentProperties(this.Handle, IntPtr.Zero, printerSettings.PrinterName, num, ref pDevModeInput, 14);
      printerSettings.SetHdevmode(num);
      printerSettings.DefaultPageSettings.SetHdevmode(num);
      Marshal.FreeHGlobal(num);
      ImagePrintOptionsDialog.GlobalUnlock(hdevmode);
      ImagePrintOptionsDialog.GlobalFree(hdevmode);
    }

    private void rdoPages_CheckedChanged(object sender, EventArgs e)
    {
      this.txtPageRange.Enabled = this.rdoPageRange.Checked;
    }

    private void rdoSizeOption_CheckedChanged(object sender, EventArgs e)
    {
      this.RefreshImageView();
    }

    private void chkPaperSelect_CheckedChanged(object sender, EventArgs e)
    {
      this.cboPaperSize.Enabled = this.chkPaperSelect.Checked;
      if (this.cboPaperSize.Enabled)
        this.SetPaperSize(this.cboPaperSize.SelectedItem.ToString());
      this.RefreshImageView();
    }

    private void cboPaperSize_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.SetPaperSize(this.cboPaperSize.SelectedItem.ToString());
      this.RefreshImageView();
    }

    private void chkDuplex_CheckedChanged(object sender, EventArgs e)
    {
      this.groupBoxFlipOptions.Enabled = this.chkDuplex.Checked;
    }

    private void btnPreviousPage_Click(object sender, EventArgs e)
    {
      if (this.tBar1.Value > 1)
        --this.tBar1.Value;
      this.btnPreviousPage.Enabled = false;
      this.RefreshImageView();
      this.btnPreviousPage.Enabled = true;
    }

    private void btnNextPage_Click(object sender, EventArgs e)
    {
      if (this.tBar1.Value < this.filesToPrint.Length)
        ++this.tBar1.Value;
      this.btnNextPage.Enabled = false;
      this.RefreshImageView();
      this.btnNextPage.Enabled = true;
    }

    private void tBar1_Scroll(object sender, EventArgs e)
    {
      this.tBar1.Enabled = false;
      this.RefreshImageView();
      this.tBar1.Enabled = true;
    }

    private void RefreshImageView()
    {
      if (this.filesToPrint == null)
        return;
      Tracing.Log(ImagePrintOptionsDialog.sw, TraceLevel.Verbose, nameof (ImagePrintOptionsDialog), "Load: " + this.filesToPrint[this.tBar1.Value - 1]);
      try
      {
        this.currentImage = Image.FromFile(this.filesToPrint[this.tBar1.Value - 1]);
        if (this.currentImage.Width > this.currentImage.Height)
        {
          this.printDocument1.DefaultPageSettings.Landscape = true;
          this.printPreviewLandscape.Visible = true;
          this.printPreviewPortrait.Visible = false;
        }
        else
        {
          this.printDocument1.DefaultPageSettings.Landscape = false;
          this.printPreviewLandscape.Visible = false;
          this.printPreviewPortrait.Visible = true;
        }
        string str1;
        if (this.chkPaperSelect.Checked)
        {
          str1 = this.cboPaperSize.SelectedItem.ToString();
        }
        else
        {
          str1 = !this.printDocument1.DefaultPageSettings.Landscape ? (this.currentImage.Height > 2200 ? "Legal" : "Letter") : (this.currentImage.Width > 2200 ? "Legal" : "Letter");
          this.SetPaperSize(str1);
        }
        this.ScaleImage(str1);
        if (this.printDocument1.DefaultPageSettings.Landscape)
        {
          this.printPreviewLandscape.Document = this.printDocument1;
          this.printPreviewLandscape.RefreshPreview();
          this.printPreviewLandscape.Visible = true;
        }
        else
        {
          this.printPreviewPortrait.Document = this.printDocument1;
          this.printPreviewPortrait.RefreshPreview();
          this.printPreviewPortrait.Visible = true;
        }
        Label lblPagePosition = this.lblPagePosition;
        int length = this.tBar1.Value;
        string str2 = length.ToString();
        length = this.filesToPrint.Length;
        string str3 = length.ToString();
        string str4 = "Page " + str2 + " of " + str3;
        lblPagePosition.Text = str4;
      }
      catch (Exception ex)
      {
        Tracing.Log(ImagePrintOptionsDialog.sw, TraceLevel.Verbose, nameof (ImagePrintOptionsDialog), ex.Message);
        int num = (int) Utils.Dialog((IWin32Window) this, "The following error occurred when trying to preview image, '" + this.filesToPrint[this.tBar1.Value - 1] + "': " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.DialogResult = DialogResult.Cancel;
      }
    }

    private void SetPaperSize(string requestedSize)
    {
      for (int index = 0; index < this.printDocument1.PrinterSettings.PaperSizes.Count; ++index)
      {
        if (requestedSize == "Letter" && this.printDocument1.PrinterSettings.PaperSizes[index].Kind == PaperKind.Letter)
        {
          this.printDocument1.DefaultPageSettings.PaperSize = this.printDocument1.PrinterSettings.PaperSizes[index];
          this.lblPaperDimensions.Text = this.printDocument1.DefaultPageSettings.Landscape ? "11 x 8.5 Inches" : "8.5 x 11 Inches";
          break;
        }
        if (requestedSize == "Legal" && this.printDocument1.PrinterSettings.PaperSizes[index].Kind == PaperKind.Legal)
        {
          this.printDocument1.DefaultPageSettings.PaperSize = this.printDocument1.PrinterSettings.PaperSizes[index];
          this.lblPaperDimensions.Text = this.printDocument1.DefaultPageSettings.Landscape ? "14 x 8.5 Inches" : "8.5 x 14 Inches";
          break;
        }
      }
    }

    private void ScaleImage(string paperSize)
    {
      double num1 = 1700.0;
      double num2 = !(paperSize == "Legal") ? 2200.0 : 2800.0;
      double num3;
      double num4;
      if (this.printDocument1.DefaultPageSettings.Landscape)
      {
        num3 = (double) this.currentImage.Height / num1;
        num4 = (double) this.currentImage.Width / num2;
      }
      else
      {
        num3 = (double) this.currentImage.Width / num1;
        num4 = (double) this.currentImage.Height / num2;
      }
      this.imageHeight = (double) (this.currentImage.Height / 2);
      this.imageWidth = (double) (this.currentImage.Width / 2);
      if (this.rdoShrink.Checked && (num4 > 1.0 || num3 > 1.0) || this.rdoFit.Checked)
      {
        if (num3 > num4)
        {
          this.imageHeight = (double) (this.currentImage.Height / 2) / num3;
          this.imageWidth = (double) (this.currentImage.Width / 2) / num3;
        }
        else
        {
          this.imageHeight = (double) (this.currentImage.Height / 2) / num4;
          this.imageWidth = (double) (this.currentImage.Width / 2) / num4;
        }
      }
      this.x = 0.0;
      this.y = 0.0;
      if (!this.rdoActualSize.Checked && !this.rdoShrink.Checked || num4 >= 1.0 || num3 >= 1.0)
        return;
      if (this.printDocument1.DefaultPageSettings.Landscape)
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

    private void btnPrint_Click(object sender, EventArgs e)
    {
      this.printOptions = new ImagePrintOptions();
      this.printOptions.ImageFiles = this.filesToPrint;
      this.printOptions.Printer = this.cboPrinter.SelectedItem.ToString();
      this.printOptions.Copies = Convert.ToInt16(this.nudCopies.Value);
      this.printOptions.AllPages = this.rdoAllPages.Checked;
      if (this.rdoCurrentPage.Checked)
        this.printOptions.CurrentPage = this.tBar1.Value;
      if (this.rdoPageRange.Checked)
        this.printOptions.PageRange = this.txtPageRange.Text;
      if (this.chkPaperSelect.Checked)
        this.printOptions.PaperSize = this.cboPaperSize.SelectedItem.ToString();
      this.printOptions.PrintToFit = this.rdoFit.Checked;
      this.printOptions.ActualSize = this.rdoActualSize.Checked;
      this.printOptions.ShrinkOversized = this.rdoShrink.Checked;
      if (this.chkDuplex.Checked)
      {
        this.printOptions.DuplexVertical = this.rdoLongEdge.Checked;
        this.printOptions.DuplexHorizontal = this.rdoShortEdge.Checked;
      }
      using (ImagePrintDialog imagePrintDialog = new ImagePrintDialog())
      {
        if (!imagePrintDialog.PrintImages(this.printOptions))
          return;
      }
      this.DialogResult = DialogResult.OK;
    }

    private void btnCancel_Click(object sender, EventArgs e) => this.Close();

    private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
    {
      if (this.currentImage == null)
        return;
      e.Graphics.DrawImage(this.currentImage, (int) this.x, (int) this.y, (int) this.imageWidth, (int) this.imageHeight);
    }

    private void btnOpenFiles_Click(object sender, EventArgs e)
    {
      this.openFileDialog1.FileName = "*.*";
      if (this.openFileDialog1.ShowDialog() != DialogResult.OK)
        return;
      this.filesToPrint = this.openFileDialog1.FileNames;
      this.lblPagePosition.Text = "Page 1 of " + this.filesToPrint.Length.ToString();
      this.tBar1.Maximum = this.filesToPrint.Length;
      if (this.filesToPrint.Length >= 5)
        this.tBar1.LargeChange = this.filesToPrint.Length / 5;
      this.tBar1.Value = 1;
      this.cboPaperSize.SelectedIndex = 0;
      this.RefreshImageView();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.cboPrinter = new ComboBox();
      this.label1 = new Label();
      this.btnPrint = new Button();
      this.btnCancel = new Button();
      this.btnPrintSettings = new Button();
      this.nudCopies = new NumericUpDown();
      this.label2 = new Label();
      this.groupBox1 = new GroupBox();
      this.txtPageRange = new TextBox();
      this.rdoPageRange = new RadioButton();
      this.rdoCurrentPage = new RadioButton();
      this.rdoAllPages = new RadioButton();
      this.tBar1 = new TrackBar();
      this.btnPreviousPage = new Button();
      this.btnNextPage = new Button();
      this.lblPagePosition = new Label();
      this.groupBox2 = new GroupBox();
      this.label3 = new Label();
      this.rdoShrink = new RadioButton();
      this.rdoActualSize = new RadioButton();
      this.rdoFit = new RadioButton();
      this.label4 = new Label();
      this.cboPaperSize = new ComboBox();
      this.chkPaperSelect = new CheckBox();
      this.lblPaperDimensions = new Label();
      this.printDocument1 = new PrintDocument();
      this.chkDuplex = new CheckBox();
      this.openFileDialog1 = new OpenFileDialog();
      this.btnOpenFiles = new Button();
      this.groupBoxFlipOptions = new GroupBox();
      this.rdoShortEdge = new RadioButton();
      this.rdoLongEdge = new RadioButton();
      this.printPreviewLandscape = new CoolPrintPreviewControl();
      this.printPreviewPortrait = new CoolPrintPreviewControl();
      this.nudCopies.BeginInit();
      this.groupBox1.SuspendLayout();
      this.tBar1.BeginInit();
      this.groupBox2.SuspendLayout();
      this.groupBoxFlipOptions.SuspendLayout();
      this.SuspendLayout();
      this.cboPrinter.FormattingEnabled = true;
      this.cboPrinter.Location = new Point(58, 12);
      this.cboPrinter.Name = "cboPrinter";
      this.cboPrinter.Size = new Size(246, 21);
      this.cboPrinter.TabIndex = 0;
      this.cboPrinter.SelectedIndexChanged += new EventHandler(this.cboPrinter_SelectedIndexChanged);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(12, 15);
      this.label1.Name = "label1";
      this.label1.Size = new Size(40, 13);
      this.label1.TabIndex = 1;
      this.label1.Text = "Printer:";
      this.btnPrint.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnPrint.DialogResult = DialogResult.OK;
      this.btnPrint.Location = new Point(492, 540);
      this.btnPrint.Name = "btnPrint";
      this.btnPrint.Size = new Size(75, 24);
      this.btnPrint.TabIndex = 8;
      this.btnPrint.Text = "Print";
      this.btnPrint.UseVisualStyleBackColor = true;
      this.btnPrint.Click += new EventHandler(this.btnPrint_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.CausesValidation = false;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(583, 540);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 24);
      this.btnCancel.TabIndex = 9;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.btnPrintSettings.Location = new Point(310, 10);
      this.btnPrintSettings.Name = "btnPrintSettings";
      this.btnPrintSettings.Size = new Size(103, 23);
      this.btnPrintSettings.TabIndex = 10;
      this.btnPrintSettings.Text = "Properties";
      this.btnPrintSettings.UseVisualStyleBackColor = true;
      this.btnPrintSettings.Click += new EventHandler(this.btnPrintSettings_Click);
      this.nudCopies.Location = new Point(58, 40);
      this.nudCopies.Minimum = new Decimal(new int[4]
      {
        1,
        0,
        0,
        0
      });
      this.nudCopies.Name = "nudCopies";
      this.nudCopies.Size = new Size(42, 20);
      this.nudCopies.TabIndex = 11;
      this.nudCopies.Value = new Decimal(new int[4]
      {
        1,
        0,
        0,
        0
      });
      this.label2.AutoSize = true;
      this.label2.Location = new Point(12, 42);
      this.label2.Name = "label2";
      this.label2.Size = new Size(42, 13);
      this.label2.TabIndex = 12;
      this.label2.Text = "Copies:";
      this.groupBox1.Controls.Add((Control) this.txtPageRange);
      this.groupBox1.Controls.Add((Control) this.rdoPageRange);
      this.groupBox1.Controls.Add((Control) this.rdoCurrentPage);
      this.groupBox1.Controls.Add((Control) this.rdoAllPages);
      this.groupBox1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.groupBox1.Location = new Point(15, 84);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(304, 112);
      this.groupBox1.TabIndex = 13;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Pages To Print";
      this.txtPageRange.Enabled = false;
      this.txtPageRange.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.txtPageRange.Location = new Point(76, 73);
      this.txtPageRange.Name = "txtPageRange";
      this.txtPageRange.Size = new Size(213, 20);
      this.txtPageRange.TabIndex = 18;
      this.rdoPageRange.AutoSize = true;
      this.rdoPageRange.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.rdoPageRange.Location = new Point(15, 73);
      this.rdoPageRange.Name = "rdoPageRange";
      this.rdoPageRange.Size = new Size(55, 17);
      this.rdoPageRange.TabIndex = 17;
      this.rdoPageRange.Text = "Pages";
      this.rdoPageRange.UseVisualStyleBackColor = true;
      this.rdoPageRange.CheckedChanged += new EventHandler(this.rdoPages_CheckedChanged);
      this.rdoCurrentPage.AutoSize = true;
      this.rdoCurrentPage.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.rdoCurrentPage.Location = new Point(15, 50);
      this.rdoCurrentPage.Name = "rdoCurrentPage";
      this.rdoCurrentPage.Size = new Size(87, 17);
      this.rdoCurrentPage.TabIndex = 16;
      this.rdoCurrentPage.Text = "Current Page";
      this.rdoCurrentPage.UseVisualStyleBackColor = true;
      this.rdoCurrentPage.CheckedChanged += new EventHandler(this.rdoPages_CheckedChanged);
      this.rdoAllPages.AutoSize = true;
      this.rdoAllPages.Checked = true;
      this.rdoAllPages.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.rdoAllPages.Location = new Point(15, 27);
      this.rdoAllPages.Name = "rdoAllPages";
      this.rdoAllPages.Size = new Size(36, 17);
      this.rdoAllPages.TabIndex = 15;
      this.rdoAllPages.TabStop = true;
      this.rdoAllPages.Text = "All";
      this.rdoAllPages.UseVisualStyleBackColor = true;
      this.rdoAllPages.CheckedChanged += new EventHandler(this.rdoPages_CheckedChanged);
      this.tBar1.LargeChange = 1;
      this.tBar1.Location = new Point(409, 429);
      this.tBar1.Maximum = 1;
      this.tBar1.Minimum = 1;
      this.tBar1.Name = "tBar1";
      this.tBar1.Size = new Size(158, 45);
      this.tBar1.TabIndex = 15;
      this.tBar1.Value = 1;
      this.tBar1.Scroll += new EventHandler(this.tBar1_Scroll);
      this.btnPreviousPage.Location = new Point(366, 429);
      this.btnPreviousPage.Name = "btnPreviousPage";
      this.btnPreviousPage.Size = new Size(37, 23);
      this.btnPreviousPage.TabIndex = 16;
      this.btnPreviousPage.Text = "<";
      this.btnPreviousPage.UseVisualStyleBackColor = true;
      this.btnPreviousPage.Click += new EventHandler(this.btnPreviousPage_Click);
      this.btnNextPage.Location = new Point(573, 429);
      this.btnNextPage.Name = "btnNextPage";
      this.btnNextPage.Size = new Size(37, 23);
      this.btnNextPage.TabIndex = 17;
      this.btnNextPage.Text = ">";
      this.btnNextPage.UseVisualStyleBackColor = true;
      this.btnNextPage.Click += new EventHandler(this.btnNextPage_Click);
      this.lblPagePosition.AutoSize = true;
      this.lblPagePosition.Location = new Point(508, 461);
      this.lblPagePosition.Name = "lblPagePosition";
      this.lblPagePosition.Size = new Size(62, 13);
      this.lblPagePosition.TabIndex = 18;
      this.lblPagePosition.Text = "Page 1 of 1";
      this.groupBox2.Controls.Add((Control) this.label3);
      this.groupBox2.Controls.Add((Control) this.rdoShrink);
      this.groupBox2.Controls.Add((Control) this.rdoActualSize);
      this.groupBox2.Controls.Add((Control) this.rdoFit);
      this.groupBox2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.groupBox2.Location = new Point(15, 202);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new Size(296, 121);
      this.groupBox2.TabIndex = 19;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Page Sizing && Handling";
      this.label3.AutoSize = true;
      this.label3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label3.Location = new Point(12, 27);
      this.label3.Name = "label3";
      this.label3.Size = new Size(69, 13);
      this.label3.TabIndex = 18;
      this.label3.Text = "Size Options:";
      this.rdoShrink.AutoSize = true;
      this.rdoShrink.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.rdoShrink.Location = new Point(15, 89);
      this.rdoShrink.Name = "rdoShrink";
      this.rdoShrink.Size = new Size(135, 17);
      this.rdoShrink.TabIndex = 17;
      this.rdoShrink.Text = "Shrink oversized pages";
      this.rdoShrink.UseVisualStyleBackColor = true;
      this.rdoShrink.CheckedChanged += new EventHandler(this.rdoSizeOption_CheckedChanged);
      this.rdoActualSize.AutoSize = true;
      this.rdoActualSize.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.rdoActualSize.Location = new Point(15, 66);
      this.rdoActualSize.Name = "rdoActualSize";
      this.rdoActualSize.Size = new Size(78, 17);
      this.rdoActualSize.TabIndex = 16;
      this.rdoActualSize.Text = "Actual Size";
      this.rdoActualSize.UseVisualStyleBackColor = true;
      this.rdoActualSize.CheckedChanged += new EventHandler(this.rdoSizeOption_CheckedChanged);
      this.rdoFit.AutoSize = true;
      this.rdoFit.Checked = true;
      this.rdoFit.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.rdoFit.Location = new Point(15, 43);
      this.rdoFit.Name = "rdoFit";
      this.rdoFit.Size = new Size(36, 17);
      this.rdoFit.TabIndex = 15;
      this.rdoFit.TabStop = true;
      this.rdoFit.Text = "Fit";
      this.rdoFit.UseVisualStyleBackColor = true;
      this.rdoFit.CheckedChanged += new EventHandler(this.rdoSizeOption_CheckedChanged);
      this.label4.AutoSize = true;
      this.label4.Location = new Point(12, 365);
      this.label4.Name = "label4";
      this.label4.Size = new Size(61, 13);
      this.label4.TabIndex = 21;
      this.label4.Text = "Paper Size:";
      this.cboPaperSize.Enabled = false;
      this.cboPaperSize.FormattingEnabled = true;
      this.cboPaperSize.Items.AddRange(new object[2]
      {
        (object) "Letter",
        (object) "Legal"
      });
      this.cboPaperSize.Location = new Point(79, 362);
      this.cboPaperSize.Name = "cboPaperSize";
      this.cboPaperSize.Size = new Size(225, 21);
      this.cboPaperSize.TabIndex = 20;
      this.cboPaperSize.SelectedIndexChanged += new EventHandler(this.cboPaperSize_SelectedIndexChanged);
      this.chkPaperSelect.AutoSize = true;
      this.chkPaperSelect.Location = new Point(15, 339);
      this.chkPaperSelect.Name = "chkPaperSelect";
      this.chkPaperSelect.Size = new Size(110, 17);
      this.chkPaperSelect.TabIndex = 22;
      this.chkPaperSelect.Text = "Print all pages on:";
      this.chkPaperSelect.UseVisualStyleBackColor = true;
      this.chkPaperSelect.CheckedChanged += new EventHandler(this.chkPaperSelect_CheckedChanged);
      this.lblPaperDimensions.AutoSize = true;
      this.lblPaperDimensions.Location = new Point(449, 68);
      this.lblPaperDimensions.Name = "lblPaperDimensions";
      this.lblPaperDimensions.Size = new Size(80, 13);
      this.lblPaperDimensions.TabIndex = 23;
      this.lblPaperDimensions.Text = "8.5 x 11 Inches";
      this.printDocument1.PrintPage += new PrintPageEventHandler(this.printDocument1_PrintPage);
      this.chkDuplex.AutoSize = true;
      this.chkDuplex.Location = new Point(15, 406);
      this.chkDuplex.Name = "chkDuplex";
      this.chkDuplex.Size = new Size(155, 17);
      this.chkDuplex.TabIndex = 25;
      this.chkDuplex.Text = "Print on both sides of paper";
      this.chkDuplex.UseVisualStyleBackColor = true;
      this.chkDuplex.CheckedChanged += new EventHandler(this.chkDuplex_CheckedChanged);
      this.openFileDialog1.FileName = "*.*";
      this.openFileDialog1.Filter = "\"PNG files|*.png\"";
      this.openFileDialog1.Multiselect = true;
      this.btnOpenFiles.Location = new Point(583, 12);
      this.btnOpenFiles.Name = "btnOpenFiles";
      this.btnOpenFiles.Size = new Size(75, 23);
      this.btnOpenFiles.TabIndex = 27;
      this.btnOpenFiles.Text = "Open Files";
      this.btnOpenFiles.UseVisualStyleBackColor = true;
      this.btnOpenFiles.Visible = false;
      this.btnOpenFiles.Click += new EventHandler(this.btnOpenFiles_Click);
      this.groupBoxFlipOptions.Controls.Add((Control) this.rdoShortEdge);
      this.groupBoxFlipOptions.Controls.Add((Control) this.rdoLongEdge);
      this.groupBoxFlipOptions.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.groupBoxFlipOptions.Location = new Point(23, 420);
      this.groupBoxFlipOptions.Name = "groupBoxFlipOptions";
      this.groupBoxFlipOptions.RightToLeft = RightToLeft.No;
      this.groupBoxFlipOptions.Size = new Size((int) byte.MaxValue, 54);
      this.groupBoxFlipOptions.TabIndex = 29;
      this.groupBoxFlipOptions.TabStop = false;
      this.rdoShortEdge.AutoSize = true;
      this.rdoShortEdge.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.rdoShortEdge.Location = new Point((int) sbyte.MaxValue, 6);
      this.rdoShortEdge.Name = "rdoShortEdge";
      this.rdoShortEdge.Size = new Size(109, 17);
      this.rdoShortEdge.TabIndex = 16;
      this.rdoShortEdge.Text = "Flip on short edge";
      this.rdoShortEdge.UseVisualStyleBackColor = true;
      this.rdoLongEdge.AutoSize = true;
      this.rdoLongEdge.Checked = true;
      this.rdoLongEdge.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.rdoLongEdge.Location = new Point(15, 6);
      this.rdoLongEdge.Name = "rdoLongEdge";
      this.rdoLongEdge.Size = new Size(106, 17);
      this.rdoLongEdge.TabIndex = 15;
      this.rdoLongEdge.TabStop = true;
      this.rdoLongEdge.Text = "Flip on long edge";
      this.rdoLongEdge.UseVisualStyleBackColor = true;
      this.printPreviewLandscape.Document = (PrintDocument) null;
      this.printPreviewLandscape.Location = new Point(325, 146);
      this.printPreviewLandscape.Name = "printPreviewLandscape";
      this.printPreviewLandscape.Size = new Size(339, 210);
      this.printPreviewLandscape.TabIndex = 34;
      this.printPreviewPortrait.Document = (PrintDocument) null;
      this.printPreviewPortrait.Location = new Point(389, 84);
      this.printPreviewPortrait.Name = "printPreviewPortrait";
      this.printPreviewPortrait.Size = new Size(210, 339);
      this.printPreviewPortrait.TabIndex = 33;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(674, 576);
      this.Controls.Add((Control) this.printPreviewLandscape);
      this.Controls.Add((Control) this.printPreviewPortrait);
      this.Controls.Add((Control) this.groupBoxFlipOptions);
      this.Controls.Add((Control) this.btnOpenFiles);
      this.Controls.Add((Control) this.chkDuplex);
      this.Controls.Add((Control) this.lblPaperDimensions);
      this.Controls.Add((Control) this.chkPaperSelect);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.cboPaperSize);
      this.Controls.Add((Control) this.groupBox2);
      this.Controls.Add((Control) this.lblPagePosition);
      this.Controls.Add((Control) this.btnNextPage);
      this.Controls.Add((Control) this.btnPreviousPage);
      this.Controls.Add((Control) this.tBar1);
      this.Controls.Add((Control) this.groupBox1);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.nudCopies);
      this.Controls.Add((Control) this.btnPrintSettings);
      this.Controls.Add((Control) this.btnPrint);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.cboPrinter);
      this.Name = nameof (ImagePrintOptionsDialog);
      this.Text = "Print";
      this.nudCopies.EndInit();
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.tBar1.EndInit();
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.groupBoxFlipOptions.ResumeLayout(false);
      this.groupBoxFlipOptions.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
