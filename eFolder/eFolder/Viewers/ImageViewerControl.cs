// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Viewers.ImageViewerControl
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DocumentConverter;
using EllieMae.EMLite.RemotingServices;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.Viewers
{
  public class ImageViewerControl : UserControl
  {
    private const string className = "ImageViewerControl";
    private static readonly string sw = Tracing.SwEFolder;
    private Cursor imageCursor;
    private Cursor originalCursor;
    private float dpiX;
    private float dpiY;
    private Image image;
    private string workerFile;
    private int workerPage;
    private string workerImage;
    private string queueFile;
    private int queuePage;
    private const float DPIFORANNOTATION = 150f;
    private IContainer components;
    private BackgroundWorker worker;
    private Label lblMessage;

    public ImageViewerControl()
    {
      this.InitializeComponent();
      this.SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
      this.imageCursor = Cursors.Default;
      using (Graphics graphics = this.CreateGraphics())
      {
        if (Session.LoanDataMgr.UseSkyDriveClassic)
        {
          this.dpiX = 150f;
          this.dpiY = 150f;
        }
        else
        {
          this.dpiX = graphics.DpiX;
          this.dpiY = graphics.DpiY;
        }
      }
    }

    [Description("Returns the images that are currently loaded")]
    [Category("Appearance")]
    [Browsable(true)]
    [DefaultValue(typeof (Cursor), "Default")]
    public Cursor ImageCursor
    {
      get => this.imageCursor;
      set => this.imageCursor = value;
    }

    public void LoadImage(string imageFile)
    {
      Tracing.Log(ImageViewerControl.sw, TraceLevel.Verbose, nameof (ImageViewerControl), nameof (LoadImage));
      try
      {
        Tracing.Log(ImageViewerControl.sw, TraceLevel.Verbose, nameof (ImageViewerControl), "Reading Image: " + imageFile);
        byte[] buffer = File.ReadAllBytes(imageFile);
        Tracing.Log(ImageViewerControl.sw, TraceLevel.Verbose, nameof (ImageViewerControl), "Loading Image");
        using (MemoryStream memoryStream = new MemoryStream(buffer, false))
        {
          Image image = Image.FromStream((Stream) memoryStream);
          if (this.image != null)
            this.image.Dispose();
          this.image = image;
        }
        Tracing.Log(ImageViewerControl.sw, TraceLevel.Verbose, nameof (ImageViewerControl), "Hiding Message");
        this.lblMessage.Visible = false;
        Tracing.Log(ImageViewerControl.sw, TraceLevel.Verbose, nameof (ImageViewerControl), "Setting AutoScrollMinSize");
        this.AutoScrollMinSize = this.image.Size;
        Tracing.Log(ImageViewerControl.sw, TraceLevel.Verbose, nameof (ImageViewerControl), "Invalidating");
        this.Invalidate();
      }
      catch (Exception ex)
      {
        Tracing.Log(ImageViewerControl.sw, TraceLevel.Error, nameof (ImageViewerControl), ex.ToString());
        this.showMessage("The following error occurred when trying to load the image:\n\n" + ex.Message + "\n\n" + ex.StackTrace);
      }
    }

    public void LoadPage(string filepath, int pageIndex)
    {
      Tracing.Log(ImageViewerControl.sw, TraceLevel.Verbose, nameof (ImageViewerControl), nameof (LoadPage));
      try
      {
        Tracing.Log(ImageViewerControl.sw, TraceLevel.Verbose, nameof (ImageViewerControl), "Showing Preparing Message");
        this.showMessage("Preparing Page " + pageIndex.ToString() + "...");
        Tracing.Log(ImageViewerControl.sw, TraceLevel.Verbose, nameof (ImageViewerControl), "Checking Worker");
        if (this.worker.IsBusy)
        {
          Tracing.Log(ImageViewerControl.sw, TraceLevel.Verbose, nameof (ImageViewerControl), "Cancelling Worker");
          this.worker.CancelAsync();
          Tracing.Log(ImageViewerControl.sw, TraceLevel.Verbose, nameof (ImageViewerControl), "Setting Queue File");
          this.queueFile = filepath;
          Tracing.Log(ImageViewerControl.sw, TraceLevel.Verbose, nameof (ImageViewerControl), "Setting Queue Page");
          this.queuePage = pageIndex;
        }
        else
        {
          Tracing.Log(ImageViewerControl.sw, TraceLevel.Verbose, nameof (ImageViewerControl), "Setting Worker File");
          this.workerFile = filepath;
          Tracing.Log(ImageViewerControl.sw, TraceLevel.Verbose, nameof (ImageViewerControl), "Setting Worker Page");
          this.workerPage = pageIndex;
          Tracing.Log(ImageViewerControl.sw, TraceLevel.Verbose, nameof (ImageViewerControl), "Starting Worker");
          this.worker.RunWorkerAsync();
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(ImageViewerControl.sw, TraceLevel.Error, nameof (ImageViewerControl), ex.ToString());
        this.showMessage("The following error occurred when trying to load the page:\n\n" + ex.Message + "\n\n" + ex.StackTrace);
      }
    }

    private void showMessage(string message)
    {
      Tracing.Log(ImageViewerControl.sw, TraceLevel.Verbose, nameof (ImageViewerControl), "showMessage: " + message);
      this.AutoScrollMinSize = new Size(0, 0);
      this.lblMessage.Text = message;
      this.lblMessage.Visible = true;
    }

    private void worker_DoWork(object sender, DoWorkEventArgs e)
    {
      Tracing.Log(ImageViewerControl.sw, TraceLevel.Verbose, nameof (ImageViewerControl), "Worker Started");
      this.workerImage = (string) null;
      string str;
      using (PdfEditor pdfEditor = new PdfEditor(this.workerFile))
        str = pdfEditor.ConvertPage(this.workerPage, this.dpiX, this.dpiY, false);
      if (this.worker.CancellationPending)
        return;
      this.workerImage = str;
    }

    private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      Tracing.Log(ImageViewerControl.sw, TraceLevel.Verbose, nameof (ImageViewerControl), "Worker Complete");
      if (this.queueFile != null)
      {
        Tracing.Log(ImageViewerControl.sw, TraceLevel.Verbose, nameof (ImageViewerControl), "Setting Worker File");
        this.workerFile = this.queueFile;
        Tracing.Log(ImageViewerControl.sw, TraceLevel.Verbose, nameof (ImageViewerControl), "Setting Worker Page");
        this.workerPage = this.queuePage;
        Tracing.Log(ImageViewerControl.sw, TraceLevel.Verbose, nameof (ImageViewerControl), "Clearing Queue File");
        this.queueFile = (string) null;
        Tracing.Log(ImageViewerControl.sw, TraceLevel.Verbose, nameof (ImageViewerControl), "Clearing Queue Page");
        this.queuePage = 0;
        Tracing.Log(ImageViewerControl.sw, TraceLevel.Verbose, nameof (ImageViewerControl), "Starting Worker");
        this.worker.RunWorkerAsync();
      }
      else if (e.Error != null)
      {
        Tracing.Log(ImageViewerControl.sw, TraceLevel.Error, nameof (ImageViewerControl), e.Error.ToString());
        this.showMessage("The following error occurred when trying to prepare the page:\n\n" + e.Error.Message + "\n\n" + e.Error.StackTrace);
      }
      else
      {
        if (this.workerImage == null)
          return;
        Tracing.Log(ImageViewerControl.sw, TraceLevel.Verbose, nameof (ImageViewerControl), "Loading Image");
        this.LoadImage(this.workerImage);
      }
    }

    public event ImageMouseEventHandler ImageClick;

    protected override void OnMouseClick(MouseEventArgs e)
    {
      if (this.image != null && this.ImageClick != null)
      {
        RectangleF rectangleF = (RectangleF) new Rectangle(0, 0, this.image.Width, this.image.Height);
        Point point;
        ref Point local = ref point;
        int x1 = e.X;
        Point autoScrollPosition = this.AutoScrollPosition;
        int x2 = autoScrollPosition.X;
        int x3 = x1 - x2;
        int y1 = e.Y;
        autoScrollPosition = this.AutoScrollPosition;
        int y2 = autoScrollPosition.Y;
        int y3 = y1 - y2;
        local = new Point(x3, y3);
        if (rectangleF.Contains((float) point.X, (float) point.Y))
        {
          float num1 = (float) this.image.Width / this.image.HorizontalResolution;
          float num2 = (float) this.image.Height / this.image.VerticalResolution;
          float num3 = num1 * ((float) point.X / rectangleF.Width);
          float num4 = num2 * ((float) point.Y / rectangleF.Height);
          int int32_1 = Convert.ToInt32(num3 * 72f);
          int int32_2 = Convert.ToInt32((float) (((double) num2 - (double) num4) * 72.0));
          this.ImageClick(new ImageMouseEventArgs(e, int32_1, int32_2));
        }
      }
      base.OnMouseClick(e);
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
      if (this.image != null)
      {
        RectangleF rectangleF = (RectangleF) new Rectangle(0, 0, this.image.Width, this.image.Height);
        Point point = new Point(e.X - this.AutoScrollPosition.X, e.Y - this.AutoScrollPosition.Y);
        if (rectangleF.Contains((float) point.X, (float) point.Y))
        {
          if (this.originalCursor == (Cursor) null)
          {
            this.originalCursor = this.Cursor;
            this.Cursor = this.imageCursor;
          }
        }
        else if (this.originalCursor != (Cursor) null)
        {
          this.Cursor = this.originalCursor;
          this.originalCursor = (Cursor) null;
        }
      }
      base.OnMouseMove(e);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
      if (this.image != null)
      {
        using (Matrix matrix1 = new Matrix())
        {
          Matrix matrix2 = matrix1;
          Point autoScrollPosition = this.AutoScrollPosition;
          double x = (double) autoScrollPosition.X;
          autoScrollPosition = this.AutoScrollPosition;
          double y = (double) autoScrollPosition.Y;
          matrix2.Translate((float) x, (float) y);
          e.Graphics.Transform = matrix1;
          e.Graphics.InterpolationMode = InterpolationMode.High;
          e.Graphics.DrawImage(this.image, new Rectangle(0, 0, this.image.Width, this.image.Height), 0, 0, this.image.Width, this.image.Height, GraphicsUnit.Pixel);
        }
      }
      base.OnPaint(e);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.worker = new BackgroundWorker();
      this.lblMessage = new Label();
      this.SuspendLayout();
      this.worker.WorkerSupportsCancellation = true;
      this.worker.DoWork += new DoWorkEventHandler(this.worker_DoWork);
      this.worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.worker_RunWorkerCompleted);
      this.lblMessage.BackColor = SystemColors.AppWorkspace;
      this.lblMessage.Dock = DockStyle.Fill;
      this.lblMessage.ForeColor = SystemColors.Window;
      this.lblMessage.Location = new Point(0, 0);
      this.lblMessage.Name = "lblMessage";
      this.lblMessage.Size = new Size(781, 223);
      this.lblMessage.TabIndex = 0;
      this.lblMessage.Text = "Message";
      this.lblMessage.TextAlign = ContentAlignment.MiddleCenter;
      this.lblMessage.Visible = false;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.AutoScroll = true;
      this.BackColor = SystemColors.AppWorkspace;
      this.Controls.Add((Control) this.lblMessage);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = "ImageViewerControl2";
      this.Size = new Size(781, 223);
      this.ResumeLayout(false);
    }
  }
}
