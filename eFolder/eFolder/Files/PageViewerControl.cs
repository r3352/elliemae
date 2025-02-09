// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Files.PageViewerControl
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DocumentConverter;
using EllieMae.EMLite.eFolder.Properties;
using EllieMae.EMLite.JedScriptEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.Files
{
  public class PageViewerControl : UserControl
  {
    private const string className = "PageViewerControl";
    private static readonly string sw = Tracing.SwEFolder;
    private string[] fileList;
    private bool loadFiles;
    private IntPtr handle;
    private string pdfFile;
    private IContainer components;
    private PictureBox pctEmpty;
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
    private Label lblMessage;
    private ToolTip tooltip;
    private BackgroundWorker worker;

    public PageViewerControl()
    {
      this.InitializeComponent();
      this.handle = this.Handle;
      this.pctEmpty.Location = new Point(0, 0);
      this.lblMessage.Location = new Point(0, 0);
      this.pnlViewer.Location = new Point(0, 0);
      this.cboZoom.SelectedItem = (object) "Fit Width";
      this.pctEmpty.BringToFront();
    }

    private void PageViewerControl_Resize(object sender, EventArgs e)
    {
      this.pctEmpty.Size = this.ClientSize;
      this.lblMessage.Size = this.ClientSize;
      this.pnlViewer.Size = this.ClientSize;
    }

    public string[] Files => this.fileList;

    public void CloseFiles()
    {
      Tracing.Log(PageViewerControl.sw, TraceLevel.Verbose, nameof (PageViewerControl), nameof (CloseFiles));
      try
      {
        if (this.fileList == null)
          return;
        Tracing.Log(PageViewerControl.sw, TraceLevel.Verbose, nameof (PageViewerControl), "Showing Empty Container");
        this.pctEmpty.BringToFront();
        Tracing.Log(PageViewerControl.sw, TraceLevel.Verbose, nameof (PageViewerControl), "Clearing File List");
        this.fileList = (string[]) null;
        Tracing.Log(PageViewerControl.sw, TraceLevel.Verbose, nameof (PageViewerControl), "Checking Worker");
        if (!this.worker.IsBusy)
          return;
        Tracing.Log(PageViewerControl.sw, TraceLevel.Verbose, nameof (PageViewerControl), "Cancelling Worker");
        this.worker.CancelAsync();
        Tracing.Log(PageViewerControl.sw, TraceLevel.Verbose, nameof (PageViewerControl), "Clearing Load Files Flag");
        this.loadFiles = false;
      }
      catch (Exception ex)
      {
        Tracing.Log(PageViewerControl.sw, TraceLevel.Error, nameof (PageViewerControl), ex.ToString());
        this.showMessage("The following error occurred when trying to close the file:\n\n" + ex.Message + "\n\n" + ex.StackTrace);
      }
    }

    public void LoadFiles(string[] fileList)
    {
      Tracing.Log(PageViewerControl.sw, TraceLevel.Verbose, nameof (PageViewerControl), nameof (LoadFiles));
      try
      {
        if (this.fileList != null)
        {
          bool flag = this.fileList.Length != fileList.Length;
          foreach (string file in this.fileList)
          {
            if (Array.IndexOf<string>(this.fileList, file) != Array.IndexOf<string>(fileList, file))
              flag = true;
          }
          if (!flag)
            return;
        }
        Tracing.Log(PageViewerControl.sw, TraceLevel.Verbose, nameof (PageViewerControl), "Showing Loading Message");
        this.showMessage("Preparing Document...");
        Tracing.Log(PageViewerControl.sw, TraceLevel.Verbose, nameof (PageViewerControl), "Setting File List");
        this.fileList = fileList;
        Tracing.Log(PageViewerControl.sw, TraceLevel.Verbose, nameof (PageViewerControl), "Checking Worker");
        if (this.worker.IsBusy)
        {
          Tracing.Log(PageViewerControl.sw, TraceLevel.Verbose, nameof (PageViewerControl), "Cancelling Worker");
          this.worker.CancelAsync();
          Tracing.Log(PageViewerControl.sw, TraceLevel.Verbose, nameof (PageViewerControl), "Setting Load Files Flag");
          this.loadFiles = true;
        }
        else
        {
          Tracing.Log(PageViewerControl.sw, TraceLevel.Verbose, nameof (PageViewerControl), "Starting Worker");
          this.worker.RunWorkerAsync((object) fileList);
          Tracing.Log(PageViewerControl.sw, TraceLevel.Verbose, nameof (PageViewerControl), "Clearing Load Files Flag");
          this.loadFiles = false;
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(PageViewerControl.sw, TraceLevel.Error, nameof (PageViewerControl), ex.ToString());
        this.showMessage("The following error occurred when trying to load the files:\n\n" + ex.Message + "\n\n" + ex.StackTrace);
      }
    }

    private void worker_DoWork(object sender, DoWorkEventArgs e)
    {
      Tracing.Log(PageViewerControl.sw, TraceLevel.Verbose, nameof (PageViewerControl), "Worker Started");
      e.Cancel = true;
      Tracing.Log(PageViewerControl.sw, TraceLevel.Verbose, nameof (PageViewerControl), "Getting File List");
      string[] fileList = (string[]) e.Argument;
      Tracing.Log(PageViewerControl.sw, TraceLevel.Verbose, nameof (PageViewerControl), "Creating PdfFileCreator");
      PdfCreator pdfCreator = new PdfCreator();
      pdfCreator.ProgressChanged += new ProgressEventHandler(this.creatorProgress);
      try
      {
        this.pdfFile = pdfCreator.MergeFiles(fileList);
      }
      catch (CanceledOperationException ex)
      {
        Tracing.Log(PageViewerControl.sw, TraceLevel.Verbose, nameof (PageViewerControl), "Merge cancelled");
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
      Tracing.Log(PageViewerControl.sw, TraceLevel.Verbose, nameof (PageViewerControl), "Worker Complete");
      if (this.loadFiles)
      {
        Tracing.Log(PageViewerControl.sw, TraceLevel.Verbose, nameof (PageViewerControl), "Starting Worker");
        this.worker.RunWorkerAsync((object) this.fileList);
        Tracing.Log(PageViewerControl.sw, TraceLevel.Verbose, nameof (PageViewerControl), "Clearing Load Files Flag");
        this.loadFiles = false;
      }
      else if (e.Error != null)
      {
        Tracing.Log(PageViewerControl.sw, TraceLevel.Error, nameof (PageViewerControl), e.Error.ToString());
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
      Tracing.Log(PageViewerControl.sw, TraceLevel.Verbose, nameof (PageViewerControl), "Handle Destroyed");
      base.OnHandleDestroyed(e);
      Tracing.Log(PageViewerControl.sw, TraceLevel.Verbose, nameof (PageViewerControl), "Checking Worker");
      if (!this.worker.IsBusy)
        return;
      Tracing.Log(PageViewerControl.sw, TraceLevel.Verbose, nameof (PageViewerControl), "Cancelling Worker");
      this.worker.CancelAsync();
      Tracing.Log(PageViewerControl.sw, TraceLevel.Verbose, nameof (PageViewerControl), "Clearing Load Files Flag");
      this.loadFiles = false;
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
      Tracing.Log(PageViewerControl.sw, TraceLevel.Verbose, nameof (PageViewerControl), "showFile: " + filepath);
      try
      {
        this.pdfViewer.LoadFile(filepath);
        this.applyZoom();
        this.pnlViewer.BringToFront();
      }
      catch (Exception ex)
      {
        Tracing.Log(PageViewerControl.sw, TraceLevel.Error, nameof (PageViewerControl), ex.ToString());
        this.showMessage("The following error occurred when trying to show the document:\n\n" + ex.Message + "\n\n" + ex.StackTrace);
      }
    }

    private void showMessage(string message)
    {
      Tracing.Log(PageViewerControl.sw, TraceLevel.Verbose, nameof (PageViewerControl), "showMessage: " + message);
      this.lblMessage.Text = message;
      this.lblMessage.BringToFront();
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

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (PageViewerControl));
      this.pnlViewer = new Panel();
      this.pdfViewer = new PdfViewerControl();
      this.pnlHeader = new GradientPanel();
      this.pnlToolbar = new FlowLayoutPanel();
      this.btnSave = new StandardIconButton();
      this.btnPrint = new StandardIconButton();
      this.separator1 = new VerticalSeparator();
      this.btnMoveFirst = new IconButton();
      this.btnMovePrevious = new StandardIconButton();
      this.btnMoveNext = new StandardIconButton();
      this.btnMoveLast = new IconButton();
      this.separator2 = new VerticalSeparator();
      this.btnZoomOut = new StandardIconButton();
      this.btnZoomIn = new StandardIconButton();
      this.cboZoom = new ComboBox();
      this.lblMessage = new Label();
      this.pctEmpty = new PictureBox();
      this.tooltip = new ToolTip(this.components);
      this.worker = new BackgroundWorker();
      this.pnlViewer.SuspendLayout();
      this.pnlHeader.SuspendLayout();
      this.pnlToolbar.SuspendLayout();
      ((ISupportInitialize) this.btnSave).BeginInit();
      ((ISupportInitialize) this.btnPrint).BeginInit();
      ((ISupportInitialize) this.btnMoveFirst).BeginInit();
      ((ISupportInitialize) this.btnMovePrevious).BeginInit();
      ((ISupportInitialize) this.btnMoveNext).BeginInit();
      ((ISupportInitialize) this.btnMoveLast).BeginInit();
      ((ISupportInitialize) this.btnZoomOut).BeginInit();
      ((ISupportInitialize) this.btnZoomIn).BeginInit();
      ((ISupportInitialize) this.pctEmpty).BeginInit();
      this.SuspendLayout();
      this.pnlViewer.Controls.Add((Control) this.pdfViewer);
      this.pnlViewer.Controls.Add((Control) this.pnlHeader);
      this.pnlViewer.Location = new Point(292, 12);
      this.pnlViewer.Name = "pnlViewer";
      this.pnlViewer.Size = new Size(312, 191);
      this.pnlViewer.TabIndex = 1;
      this.pdfViewer.BackColor = SystemColors.AppWorkspace;
      this.pdfViewer.BackgroundImageLayout = ImageLayout.Center;
      this.pdfViewer.Dock = DockStyle.Fill;
      this.pdfViewer.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.pdfViewer.Location = new Point(0, 31);
      this.pdfViewer.Name = "pdfViewer";
      this.pdfViewer.Size = new Size(312, 160);
      this.pdfViewer.TabIndex = 7;
      this.pdfViewer.TabStop = false;
      this.pnlHeader.Borders = AnchorStyles.Bottom;
      this.pnlHeader.Controls.Add((Control) this.pnlToolbar);
      this.pnlHeader.Dock = DockStyle.Top;
      this.pnlHeader.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.pnlHeader.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.pnlHeader.Location = new Point(0, 0);
      this.pnlHeader.Name = "pnlHeader";
      this.pnlHeader.Padding = new Padding(8, 0, 0, 0);
      this.pnlHeader.Size = new Size(312, 31);
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
      this.pnlToolbar.Location = new Point(8, 4);
      this.pnlToolbar.Name = "pnlToolbar";
      this.pnlToolbar.Size = new Size(296, 22);
      this.pnlToolbar.TabIndex = 3;
      this.btnSave.BackColor = Color.Transparent;
      this.btnSave.Location = new Point(0, 3);
      this.btnSave.Margin = new Padding(0, 3, 4, 0);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(16, 16);
      this.btnSave.StandardButtonType = StandardIconButton.ButtonType.SaveButton;
      this.btnSave.TabIndex = 28;
      this.btnSave.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnSave, "Save to Disk");
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.btnPrint.BackColor = Color.Transparent;
      this.btnPrint.Location = new Point(20, 3);
      this.btnPrint.Margin = new Padding(0, 3, 4, 0);
      this.btnPrint.Name = "btnPrint";
      this.btnPrint.Size = new Size(16, 16);
      this.btnPrint.StandardButtonType = StandardIconButton.ButtonType.PrintButton;
      this.btnPrint.TabIndex = 27;
      this.btnPrint.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnPrint, "Print Document");
      this.btnPrint.Click += new EventHandler(this.btnPrint_Click);
      this.separator1.Location = new Point(40, 3);
      this.separator1.Margin = new Padding(0, 3, 3, 3);
      this.separator1.MaximumSize = new Size(2, 16);
      this.separator1.MinimumSize = new Size(2, 16);
      this.separator1.Name = "separator1";
      this.separator1.Size = new Size(2, 16);
      this.separator1.TabIndex = 4;
      this.separator1.TabStop = false;
      this.btnMoveFirst.BackColor = Color.Transparent;
      this.btnMoveFirst.DisabledImage = (Image) componentResourceManager.GetObject("btnMoveFirst.DisabledImage");
      this.btnMoveFirst.Image = (Image) componentResourceManager.GetObject("btnMoveFirst.Image");
      this.btnMoveFirst.Location = new Point(45, 3);
      this.btnMoveFirst.Margin = new Padding(0, 3, 4, 0);
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
      this.btnMoveLast.MouseOverImage = (Image) componentResourceManager.GetObject("btnMoveLast.MouseOverImage");
      this.btnMoveLast.Name = "btnMoveLast";
      this.btnMoveLast.Size = new Size(16, 16);
      this.btnMoveLast.TabIndex = 15;
      this.btnMoveLast.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnMoveLast, "Last Page");
      this.btnMoveLast.Click += new EventHandler(this.btnMoveLast_Click);
      this.separator2.Location = new Point(125, 3);
      this.separator2.Margin = new Padding(0, 3, 3, 3);
      this.separator2.MaximumSize = new Size(2, 16);
      this.separator2.MinimumSize = new Size(2, 16);
      this.separator2.Name = "separator2";
      this.separator2.Size = new Size(2, 16);
      this.separator2.TabIndex = 5;
      this.separator2.TabStop = false;
      this.btnZoomOut.BackColor = Color.Transparent;
      this.btnZoomOut.Location = new Point(130, 3);
      this.btnZoomOut.Margin = new Padding(0, 3, 4, 0);
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
      this.btnZoomIn.Name = "btnZoomIn";
      this.btnZoomIn.Size = new Size(16, 16);
      this.btnZoomIn.StandardButtonType = StandardIconButton.ButtonType.ZoomInButton;
      this.btnZoomIn.TabIndex = 18;
      this.btnZoomIn.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnZoomIn, "Zoom In");
      this.btnZoomIn.Click += new EventHandler(this.btnZoomIn_Click);
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
      this.cboZoom.SelectionChangeCommitted += new EventHandler(this.cboZoom_SelectionChangeCommitted);
      this.cboZoom.SelectedIndexChanged += new EventHandler(this.cboZoom_SelectedIndexChanged);
      this.lblMessage.BackColor = SystemColors.AppWorkspace;
      this.lblMessage.ForeColor = SystemColors.Window;
      this.lblMessage.Location = new Point(152, 12);
      this.lblMessage.Name = "lblMessage";
      this.lblMessage.Size = new Size(128, 191);
      this.lblMessage.TabIndex = 0;
      this.lblMessage.Text = "Message";
      this.lblMessage.TextAlign = ContentAlignment.MiddleCenter;
      this.pctEmpty.BackColor = Color.WhiteSmoke;
      this.pctEmpty.Image = (Image) Resources.file_viewer;
      this.pctEmpty.Location = new Point(12, 12);
      this.pctEmpty.Name = "pctEmpty";
      this.pctEmpty.Size = new Size(128, 191);
      this.pctEmpty.SizeMode = PictureBoxSizeMode.CenterImage;
      this.pctEmpty.TabIndex = 9;
      this.pctEmpty.TabStop = false;
      this.worker.WorkerReportsProgress = true;
      this.worker.WorkerSupportsCancellation = true;
      this.worker.DoWork += new DoWorkEventHandler(this.worker_DoWork);
      this.worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.worker_RunWorkerCompleted);
      this.worker.ProgressChanged += new ProgressChangedEventHandler(this.worker_ProgressChanged);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.pctEmpty);
      this.Controls.Add((Control) this.pnlViewer);
      this.Controls.Add((Control) this.lblMessage);
      this.Name = nameof (PageViewerControl);
      this.Size = new Size(619, 221);
      this.Resize += new EventHandler(this.PageViewerControl_Resize);
      this.pnlViewer.ResumeLayout(false);
      this.pnlHeader.ResumeLayout(false);
      this.pnlToolbar.ResumeLayout(false);
      ((ISupportInitialize) this.btnSave).EndInit();
      ((ISupportInitialize) this.btnPrint).EndInit();
      ((ISupportInitialize) this.btnMoveFirst).EndInit();
      ((ISupportInitialize) this.btnMovePrevious).EndInit();
      ((ISupportInitialize) this.btnMoveNext).EndInit();
      ((ISupportInitialize) this.btnMoveLast).EndInit();
      ((ISupportInitialize) this.btnZoomOut).EndInit();
      ((ISupportInitialize) this.btnZoomIn).EndInit();
      ((ISupportInitialize) this.pctEmpty).EndInit();
      this.ResumeLayout(false);
    }
  }
}
