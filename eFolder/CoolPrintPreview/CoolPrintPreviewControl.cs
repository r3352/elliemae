// Decompiled with JetBrains decompiler
// Type: CoolPrintPreview.CoolPrintPreviewControl
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Windows.Forms;

#nullable disable
namespace CoolPrintPreview
{
  internal class CoolPrintPreviewControl : UserControl
  {
    private PrintDocument _doc;
    private ZoomMode _zoomMode;
    private double _zoom;
    private int _startPage;
    private Brush _backBrush;
    private Point _ptLast;
    private PointF _himm2pix = new PointF(-1f, -1f);
    private PageImageList _img = new PageImageList();
    private bool _cancel;
    private bool _rendering;
    private const int MARGIN = 4;

    public CoolPrintPreviewControl()
    {
      this.BackColor = SystemColors.AppWorkspace;
      this.ZoomMode = ZoomMode.FullPage;
      this.StartPage = 0;
      this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
    }

    public PrintDocument Document
    {
      get => this._doc;
      set
      {
        if (value == this._doc)
          return;
        this._doc = value;
        this.RefreshPreview();
      }
    }

    public void RefreshPreview()
    {
      if (this._doc != null)
      {
        this._img.Clear();
        PrintController printController = this._doc.PrintController;
        try
        {
          this._cancel = false;
          this._rendering = true;
          this._doc.PrintController = (PrintController) new PreviewPrintController();
          this._doc.PrintPage += new PrintPageEventHandler(this._doc_PrintPage);
          this._doc.EndPrint += new PrintEventHandler(this._doc_EndPrint);
          this._doc.Print();
        }
        finally
        {
          this._cancel = false;
          this._rendering = false;
          this._doc.PrintPage -= new PrintPageEventHandler(this._doc_PrintPage);
          this._doc.EndPrint -= new PrintEventHandler(this._doc_EndPrint);
          this._doc.PrintController = printController;
        }
      }
      this.OnPageCountChanged(EventArgs.Empty);
      this.UpdatePreview();
      this.UpdateScrollBars();
    }

    public void Cancel() => this._cancel = true;

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool IsRendering => this._rendering;

    [DefaultValue(ZoomMode.FullPage)]
    public ZoomMode ZoomMode
    {
      get => this._zoomMode;
      set
      {
        if (value == this._zoomMode)
          return;
        this._zoomMode = value;
        this.UpdateScrollBars();
        this.OnZoomModeChanged(EventArgs.Empty);
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public double Zoom
    {
      get => this._zoom;
      set
      {
        if (value == this._zoom && this.ZoomMode == ZoomMode.Custom)
          return;
        this.ZoomMode = ZoomMode.Custom;
        this._zoom = value;
        this.UpdateScrollBars();
        this.OnZoomModeChanged(EventArgs.Empty);
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public int StartPage
    {
      get => this._startPage;
      set
      {
        if (value > this.PageCount - 1)
          value = this.PageCount - 1;
        if (value < 0)
          value = 0;
        if (value == this._startPage)
          return;
        this._startPage = value;
        this.UpdateScrollBars();
        this.OnStartPageChanged(EventArgs.Empty);
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public int PageCount => this._img.Count;

    [DefaultValue(typeof (Color), "AppWorkspace")]
    public override Color BackColor
    {
      get => base.BackColor;
      set
      {
        base.BackColor = value;
        this._backBrush = (Brush) new SolidBrush(value);
      }
    }

    [Browsable(false)]
    public PageImageList PageImages => this._img;

    public void Print()
    {
      PrinterSettings printerSettings = this._doc.PrinterSettings;
      int first = printerSettings.MinimumPage - 1;
      int last = printerSettings.MaximumPage - 1;
      switch (printerSettings.PrintRange)
      {
        case PrintRange.AllPages:
          this.Document.Print();
          return;
        case PrintRange.Selection:
          first = last = this.StartPage;
          if (this.ZoomMode == ZoomMode.TwoPages)
          {
            last = Math.Min(first + 1, this.PageCount - 1);
            break;
          }
          break;
        case PrintRange.SomePages:
          first = printerSettings.FromPage - 1;
          last = printerSettings.ToPage - 1;
          break;
        case PrintRange.CurrentPage:
          first = last = this.StartPage;
          break;
      }
      new CoolPrintPreviewControl.DocumentPrinter(this, first, last).Print();
    }

    public event EventHandler StartPageChanged;

    protected void OnStartPageChanged(EventArgs e)
    {
      if (this.StartPageChanged == null)
        return;
      this.StartPageChanged((object) this, e);
    }

    public event EventHandler PageCountChanged;

    protected void OnPageCountChanged(EventArgs e)
    {
      if (this.PageCountChanged == null)
        return;
      this.PageCountChanged((object) this, e);
    }

    public event EventHandler ZoomModeChanged;

    protected void OnZoomModeChanged(EventArgs e)
    {
      if (this.ZoomModeChanged == null)
        return;
      this.ZoomModeChanged((object) this, e);
    }

    protected override void OnPaintBackground(PaintEventArgs e)
    {
    }

    protected override void OnPaint(PaintEventArgs e)
    {
      Image image1 = this.GetImage(this.StartPage);
      if (image1 != null)
      {
        Rectangle imageRectangle = this.GetImageRectangle(image1);
        if (imageRectangle.Width > 2 && imageRectangle.Height > 2)
        {
          imageRectangle.Offset(this.AutoScrollPosition);
          if (this._zoomMode != ZoomMode.TwoPages)
          {
            this.RenderPage(e.Graphics, image1, imageRectangle);
          }
          else
          {
            imageRectangle.Width = (imageRectangle.Width - 4) / 2;
            this.RenderPage(e.Graphics, image1, imageRectangle);
            Image image2 = this.GetImage(this.StartPage + 1);
            if (image2 != null)
            {
              imageRectangle = this.GetImageRectangle(image2);
              imageRectangle.Width = (imageRectangle.Width - 4) / 2;
              imageRectangle.Offset(imageRectangle.Width + 4, 0);
              this.RenderPage(e.Graphics, image2, imageRectangle);
            }
          }
        }
      }
      e.Graphics.FillRectangle(this._backBrush, this.ClientRectangle);
    }

    protected override void OnSizeChanged(EventArgs e)
    {
      this.UpdateScrollBars();
      base.OnSizeChanged(e);
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
      base.OnMouseDown(e);
      if (e.Button != MouseButtons.Left || !(this.AutoScrollMinSize != Size.Empty))
        return;
      this.Cursor = Cursors.NoMove2D;
      this._ptLast = new Point(e.X, e.Y);
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
      base.OnMouseUp(e);
      if (e.Button != MouseButtons.Left || !(this.Cursor == Cursors.NoMove2D))
        return;
      this.Cursor = Cursors.Default;
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
      base.OnMouseMove(e);
      if (!(this.Cursor == Cursors.NoMove2D))
        return;
      int num1 = e.X - this._ptLast.X;
      int num2 = e.Y - this._ptLast.Y;
      if (num1 == 0 && num2 == 0)
        return;
      Point autoScrollPosition = this.AutoScrollPosition;
      this.AutoScrollPosition = new Point(-(autoScrollPosition.X + num1), -(autoScrollPosition.Y + num2));
      this._ptLast = new Point(e.X, e.Y);
    }

    protected override bool IsInputKey(Keys keyData)
    {
      switch (keyData)
      {
        case Keys.Prior:
        case Keys.Next:
        case Keys.End:
        case Keys.Home:
        case Keys.Left:
        case Keys.Up:
        case Keys.Right:
        case Keys.Down:
          return true;
        default:
          return base.IsInputKey(keyData);
      }
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
      base.OnKeyDown(e);
      if (e.Handled)
        return;
      switch (e.KeyCode)
      {
        case Keys.Prior:
          --this.StartPage;
          break;
        case Keys.Next:
          ++this.StartPage;
          break;
        case Keys.End:
          this.AutoScrollPosition = Point.Empty;
          this.StartPage = this.PageCount - 1;
          break;
        case Keys.Home:
          this.AutoScrollPosition = Point.Empty;
          this.StartPage = 0;
          break;
        case Keys.Left:
        case Keys.Up:
        case Keys.Right:
        case Keys.Down:
          if (this.ZoomMode == ZoomMode.FullPage || this.ZoomMode == ZoomMode.TwoPages)
          {
            switch (e.KeyCode)
            {
              case Keys.Left:
              case Keys.Up:
                --this.StartPage;
                break;
              case Keys.Right:
              case Keys.Down:
                ++this.StartPage;
                break;
            }
          }
          else
          {
            Point autoScrollPosition = this.AutoScrollPosition;
            switch (e.KeyCode)
            {
              case Keys.Left:
                autoScrollPosition.X += 20;
                break;
              case Keys.Up:
                autoScrollPosition.Y += 20;
                break;
              case Keys.Right:
                autoScrollPosition.X -= 20;
                break;
              case Keys.Down:
                autoScrollPosition.Y -= 20;
                break;
            }
            this.AutoScrollPosition = new Point(-autoScrollPosition.X, -autoScrollPosition.Y);
            break;
          }
          break;
        default:
          return;
      }
      e.Handled = true;
    }

    private void _doc_PrintPage(object sender, PrintPageEventArgs e)
    {
      this.SyncPageImages(false);
      if (!this._cancel)
        return;
      e.Cancel = true;
    }

    private void _doc_EndPrint(object sender, PrintEventArgs e) => this.SyncPageImages(true);

    private void SyncPageImages(bool lastPageReady)
    {
      if (!(this._doc.PrintController is PreviewPrintController))
        return;
      PreviewPrintController printController = (PreviewPrintController) this._doc.PrintController;
      if (printController == null)
        return;
      PreviewPageInfo[] previewPageInfo = printController.GetPreviewPageInfo();
      int num = lastPageReady ? previewPageInfo.Length : previewPageInfo.Length - 1;
      for (int count = this._img.Count; count < num; ++count)
      {
        this._img.Add(previewPageInfo[count].Image);
        this.OnPageCountChanged(EventArgs.Empty);
        if (this.StartPage < 0)
          this.StartPage = 0;
        if (count == this.StartPage || count == this.StartPage + 1)
          this.Refresh();
        Application.DoEvents();
      }
    }

    private Image GetImage(int page)
    {
      return page <= -1 || page >= this.PageCount ? (Image) null : this._img[page];
    }

    private Rectangle GetImageRectangle(Image img)
    {
      Size imageSizeInPixels = this.GetImageSizeInPixels(img);
      Rectangle imageRectangle = new Rectangle(0, 0, imageSizeInPixels.Width, imageSizeInPixels.Height);
      Rectangle clientRectangle = this.ClientRectangle;
      switch (this._zoomMode)
      {
        case ZoomMode.ActualSize:
          this._zoom = 1.0;
          break;
        case ZoomMode.FullPage:
          this._zoom = Math.Min(imageRectangle.Width > 0 ? (double) clientRectangle.Width / (double) imageRectangle.Width : 0.0, imageRectangle.Height > 0 ? (double) clientRectangle.Height / (double) imageRectangle.Height : 0.0);
          break;
        case ZoomMode.PageWidth:
          this._zoom = imageRectangle.Width > 0 ? (double) clientRectangle.Width / (double) imageRectangle.Width : 0.0;
          break;
        case ZoomMode.TwoPages:
          imageRectangle.Width *= 2;
          goto case ZoomMode.FullPage;
      }
      imageRectangle.Width = (int) ((double) imageRectangle.Width * this._zoom);
      imageRectangle.Height = (int) ((double) imageRectangle.Height * this._zoom);
      int num1 = (clientRectangle.Width - imageRectangle.Width) / 2;
      if (num1 > 0)
        imageRectangle.X += num1;
      int num2 = (clientRectangle.Height - imageRectangle.Height) / 2;
      if (num2 > 0)
        imageRectangle.Y += num2;
      imageRectangle.Inflate(-4, -4);
      if (this._zoomMode == ZoomMode.TwoPages)
        imageRectangle.Inflate(-2, 0);
      return imageRectangle;
    }

    private Size GetImageSizeInPixels(Image img)
    {
      SizeF physicalDimension = img.PhysicalDimension;
      if (img is Metafile)
      {
        if ((double) this._himm2pix.X < 0.0)
        {
          using (Graphics graphics = this.CreateGraphics())
          {
            this._himm2pix.X = graphics.DpiX / 2540f;
            this._himm2pix.Y = graphics.DpiY / 2540f;
          }
        }
        physicalDimension.Width *= this._himm2pix.X;
        physicalDimension.Height *= this._himm2pix.Y;
      }
      return Size.Truncate(physicalDimension);
    }

    private void RenderPage(Graphics g, Image img, Rectangle rc)
    {
      rc.Offset(1, 1);
      g.DrawRectangle(Pens.Black, rc);
      rc.Offset(-1, -1);
      g.FillRectangle(Brushes.White, rc);
      g.DrawImage(img, rc);
      g.DrawRectangle(Pens.Black, rc);
      ++rc.Width;
      ++rc.Height;
      g.ExcludeClip(rc);
      rc.Offset(1, 1);
      g.ExcludeClip(rc);
    }

    private void UpdateScrollBars()
    {
      Rectangle rectangle = Rectangle.Empty;
      Image image = this.GetImage(this.StartPage);
      if (image != null)
        rectangle = this.GetImageRectangle(image);
      Size size = new Size(0, 0);
      switch (this._zoomMode)
      {
        case ZoomMode.ActualSize:
        case ZoomMode.Custom:
          size = new Size(rectangle.Width + 8, rectangle.Height + 8);
          break;
        case ZoomMode.PageWidth:
          size = new Size(0, rectangle.Height + 8);
          break;
      }
      if (size != this.AutoScrollMinSize)
        this.AutoScrollMinSize = size;
      this.UpdatePreview();
    }

    private void UpdatePreview()
    {
      if (this._startPage < 0)
        this._startPage = 0;
      if (this._startPage > this.PageCount - 1)
        this._startPage = this.PageCount - 1;
      this.Invalidate();
    }

    internal class DocumentPrinter : PrintDocument
    {
      private int _first;
      private int _last;
      private int _index;
      private PageImageList _imgList;

      public DocumentPrinter(CoolPrintPreviewControl preview, int first, int last)
      {
        this._first = first;
        this._last = last;
        this._imgList = preview.PageImages;
        this.DefaultPageSettings = preview.Document.DefaultPageSettings;
        this.PrinterSettings = preview.Document.PrinterSettings;
      }

      protected override void OnBeginPrint(PrintEventArgs e) => this._index = this._first;

      protected override void OnPrintPage(PrintPageEventArgs e)
      {
        e.Graphics.PageUnit = GraphicsUnit.Display;
        e.Graphics.DrawImage(this._imgList[this._index++], e.PageBounds);
        e.HasMorePages = this._index <= this._last;
      }
    }
  }
}
