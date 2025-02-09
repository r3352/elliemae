// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Viewers.PageImageListControl
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.eFolder;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.eFolder.Properties;
using EllieMae.EMLite.eFolder.UI;
using EllieMae.EMLite.JedScriptEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.Viewers
{
  public class PageImageListControl : UserControl
  {
    private const string className = "PageImageListControl";
    private static readonly string sw = Tracing.SwEFolder;
    private const int WM_VSCROLL = 277;
    private const int SB_VERT = 1;
    private const uint SIF_TRACKPOS = 16;
    private ImageAttachment[] attachmentList;
    private VirtualPageImage[] virtualPageList;
    private List<PageImage> downloadPageList;
    private MsgHook pnlPagesHook;
    private Cursor annotationCursor;
    private Point dragPoint;
    private VirtualPageImage currentPage;
    private VirtualPageAnnotation selectedAnnotation;
    private IContainer components;
    private BorderPanel pnlBorders;
    private Panel pnlScrollThumbnail;
    private PictureBox pctScrollThumbnail;
    private Label lblScrollThumbnail;
    private GradientPanel pnlHeader;
    private FlowLayoutPanel pnlToolbar;
    private StandardIconButton btnSave;
    private StandardIconButton btnPrint;
    private VerticalSeparator separator1;
    private StandardIconButton btnPrevious;
    private StandardIconButton btnNext;
    private VerticalSeparator separator2;
    private StandardIconButton btnZoomOut;
    private StandardIconButton btnZoomIn;
    private ComboBox cboZoom;
    private VerticalSeparator separator3;
    private IconCheckBox chkAnnotate;
    private VerticalSeparator separator4;
    private Button btnViewOriginal;
    private DoubleBufferedPanel pnlPages;
    private ToolTip toolTip;
    private ContextMenuStrip contextMenu;
    private ToolStripMenuItem mnuDelete;
    private BackgroundWorker worker;
    private ToolStripSeparator toolStripMenuItem1;
    private ToolStripMenuItem mnuVisibility;
    private ToolStripMenuItem personalToolStripMenuItem;
    private ToolStripMenuItem internalToolStripMenuItem;
    private ToolStripMenuItem publicToolStripMenuItem;

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetScrollInfo(
      IntPtr hwnd,
      int fnBar,
      ref PageImageListControl.SCROLLINFO lpsi);

    public PageImageListControl()
    {
      this.InitializeComponent();
      this.cboZoom.SelectedItem = (object) "Fit Width";
      this.pnlPages.HorizontalScroll.SmallChange = 15;
      this.pnlPages.VerticalScroll.SmallChange = 15;
      this.pnlPagesHook = new MsgHook((Control) this.pnlPages);
      this.pnlPagesHook.CaptureMsg(277);
      this.pnlPagesHook.OnMessage += new CallBackHandler(this.pnlPagesHook_OnMessage);
      this.pnlPages.MouseWheel += new MouseEventHandler(this.pnlPages_MouseWheel);
      this.annotationCursor = Cursors.Cross;
      this.downloadPageList = new List<PageImage>();
    }

    public void LoadPages(ImageAttachment[] attachmentList)
    {
      if (this.attachmentList == null && !string.IsNullOrEmpty(Session.GetPrivateProfileString(Environment.MachineName, "ZoomLevel")))
        this.cboZoom.SelectedItem = (object) Session.GetPrivateProfileString(Environment.MachineName, "ZoomLevel");
      this.attachmentList = attachmentList;
      this.applySecurity();
      List<VirtualPageImage> virtualPageImageList = new List<VirtualPageImage>();
      foreach (ImageAttachment attachment in attachmentList)
      {
        foreach (PageImage page in attachment.Pages)
          virtualPageImageList.Add(new VirtualPageImage(page));
      }
      this.virtualPageList = virtualPageImageList.ToArray();
      this.calculateVirtualRectangles();
    }

    private void applySecurity()
    {
      eFolderAccessRights folderAccessRights = new eFolderAccessRights(Session.LoanDataMgr, (LogRecordBase) Session.LoanDataMgr.FileAttachments.GetLinkedDocument(this.attachmentList[0].ID));
      this.separator3.Visible = folderAccessRights.CanAnnotateFiles;
      this.chkAnnotate.Visible = folderAccessRights.CanAnnotateFiles;
      this.contextMenu.Enabled = folderAccessRights.CanDeleteAnnotations;
    }

    public int GetCurrentPage()
    {
      Rectangle displayRect = this.getDisplayRect();
      for (int currentPage = 0; currentPage < this.virtualPageList.Length; ++currentPage)
      {
        if (displayRect.IntersectsWith(this.virtualPageList[currentPage].Bounds))
          return currentPage;
      }
      return -1;
    }

    public void SetCurrentPage(int pageIndex)
    {
      VirtualPageImage virtualPage = this.virtualPageList[pageIndex];
      this.pnlPages.AutoScrollPosition = new Point(virtualPage.Left - 4, virtualPage.Top - 4);
      this.OnScrollComplete(EventArgs.Empty);
    }

    public void SetCurrentPage(PageAnnotation annotation)
    {
      foreach (VirtualPageImage virtualPage in this.virtualPageList)
      {
        foreach (VirtualPageAnnotation annotation1 in virtualPage.Annotations)
        {
          if (annotation1.Annotation == annotation)
          {
            this.pnlPages.AutoScrollPosition = new Point(annotation1.Left - 4, annotation1.Top - 4);
            this.OnScrollComplete(EventArgs.Empty);
            return;
          }
        }
      }
    }

    private VirtualPageImage getPageAtPoint(int x, int y)
    {
      foreach (VirtualPageImage virtualPage in this.virtualPageList)
      {
        if (virtualPage.Bounds.Contains(x, y))
          return virtualPage;
      }
      return (VirtualPageImage) null;
    }

    private Rectangle getDisplayRect()
    {
      Point autoScrollPosition = this.pnlPages.AutoScrollPosition;
      int x = Math.Abs(autoScrollPosition.X);
      autoScrollPosition = this.pnlPages.AutoScrollPosition;
      int y = Math.Abs(autoScrollPosition.Y);
      Size clientSize = this.pnlPages.ClientSize;
      int width = clientSize.Width;
      clientSize = this.pnlPages.ClientSize;
      int height = clientSize.Height;
      return new Rectangle(x, y, width, height);
    }

    private void calculateVirtualRectangles()
    {
      if (this.virtualPageList == null)
        return;
      int width1 = this.pnlPages.ClientSize.Width;
      int height1 = this.pnlPages.ClientSize.Height;
      float num1 = 0.0f;
      float num2 = 0.0f;
      using (Graphics graphics = Graphics.FromHwnd(this.pnlPages.Handle))
      {
        num1 = graphics.DpiX;
        num2 = graphics.DpiY;
      }
      float num3 = 0.0f;
      switch (this.cboZoom.Text)
      {
        case "10%":
          num3 = 0.1f;
          break;
        case "100%":
          num3 = 1f;
          break;
        case "125%":
          num3 = 1.25f;
          break;
        case "150%":
          num3 = 1.5f;
          break;
        case "200%":
          num3 = 2f;
          break;
        case "25%":
          num3 = 0.25f;
          break;
        case "400%":
          num3 = 4f;
          break;
        case "50%":
          num3 = 0.5f;
          break;
        case "75%":
          num3 = 0.75f;
          break;
        case "Fit Height":
          num3 = -3f;
          break;
        case "Fit Page":
          num3 = -1f;
          break;
        case "Fit Width":
          num3 = -2f;
          break;
      }
      int width2 = width1;
      foreach (VirtualPageImage virtualPage in this.virtualPageList)
      {
        int int32_1 = Convert.ToInt32((float) virtualPage.Page.Width / virtualPage.Page.HorizontalResolution * num1);
        int int32_2 = Convert.ToInt32((float) virtualPage.Page.Height / virtualPage.Page.VerticalResolution * num2);
        int width3;
        int height2;
        if ((double) num3 == -1.0)
        {
          if ((double) (((float) width1 - 8f) / (float) int32_1) <= (double) (((float) height1 - 8f) / (float) int32_2))
          {
            width3 = width1 - 8;
            height2 = width3 * int32_2 / int32_1;
          }
          else
          {
            height2 = height1 - 8;
            width3 = height2 * int32_1 / int32_2;
          }
        }
        else if ((double) num3 == -2.0)
        {
          width3 = width1 - 8;
          height2 = width3 * int32_2 / int32_1;
        }
        else if ((double) num3 == -3.0)
        {
          height2 = height1 - 8;
          width3 = height2 * int32_1 / int32_2;
        }
        else
        {
          width3 = Convert.ToInt32((float) int32_1 * num3);
          height2 = Convert.ToInt32((float) int32_2 * num3);
        }
        if (width3 + 8 > width2)
          width2 = width3 + 8;
        virtualPage.Size = new Size(width3, height2);
      }
      int height3 = 0;
      foreach (VirtualPageImage virtualPage in this.virtualPageList)
      {
        int y = height3 + 4;
        int x = (width2 - virtualPage.Width) / 2;
        virtualPage.Location = new Point(x, y);
        height3 = y + (virtualPage.Height + 4);
      }
      if (width1 >= width2)
        width2 = 0;
      if (height1 >= height3)
        height3 = 0;
      this.pnlPages.AutoScrollMinSize = new Size(width2, height3);
      this.pnlPages.Invalidate();
      this.OnScrollComplete(EventArgs.Empty);
    }

    private void pnlPagesHook_OnMessage(ref System.Windows.Forms.Message m)
    {
      int num = m.WParam.ToInt32() & (int) ushort.MaxValue;
      if (num == 5)
      {
        PageImageListControl.SCROLLINFO lpsi = new PageImageListControl.SCROLLINFO();
        lpsi.cbSize = Marshal.SizeOf<PageImageListControl.SCROLLINFO>(lpsi);
        lpsi.fMask = 16U;
        if (!PageImageListControl.GetScrollInfo(m.HWnd, 1, ref lpsi))
          return;
        this.showScrollThumbnail(lpsi.nTrackPos);
      }
      else
      {
        if (num == 4)
          this.pnlScrollThumbnail.Visible = false;
        if (num == 8)
          this.OnScrollComplete(EventArgs.Empty);
        this.pnlPagesHook.DefWndProc(ref m);
      }
    }

    private void showScrollThumbnail(int nTrackPos)
    {
      int num1 = nTrackPos;
      int num2 = num1 + this.ClientSize.Height;
      VirtualPageImage virtualPageImage = (VirtualPageImage) null;
      foreach (VirtualPageImage virtualPage in this.virtualPageList)
      {
        int top = virtualPage.Top;
        int num3 = top + virtualPage.Height;
        if (top < num2 && num1 < num3)
        {
          virtualPageImage = virtualPage;
          break;
        }
      }
      if (virtualPageImage == null)
        return;
      this.lblScrollThumbnail.Text = "Page " + Convert.ToString(Array.IndexOf<VirtualPageImage>(this.virtualPageList, virtualPageImage) + 1);
      Size size = new Size(this.pnlScrollThumbnail.Padding.Size.Width + virtualPageImage.Page.Thumbnail.Width, this.pnlScrollThumbnail.Padding.Size.Height + virtualPageImage.Page.Thumbnail.Height + this.lblScrollThumbnail.Height);
      Point point = new Point(this.pnlPages.ClientSize.Width - size.Width - 10, (this.pnlPages.ClientSize.Height - size.Height) / 2);
      this.pnlScrollThumbnail.SetBounds(point.X, point.Y, size.Width, size.Height);
      if (this.pctScrollThumbnail.Image != null)
      {
        this.pctScrollThumbnail.Image.Dispose();
        this.pctScrollThumbnail.Image = (Image) null;
      }
      this.pctScrollThumbnail.Image = Session.LoanDataMgr.FileAttachments.GetImage(virtualPageImage.Page.Thumbnail);
      this.pnlScrollThumbnail.Visible = true;
    }

    private void pnlPages_ClientSizeChanged(object sender, EventArgs e)
    {
      this.calculateVirtualRectangles();
    }

    private void pnlPages_Paint(object sender, PaintEventArgs e)
    {
      if (this.virtualPageList == null)
        return;
      Rectangle displayRect = this.getDisplayRect();
      Rectangle a;
      ref Rectangle local = ref a;
      Rectangle clipRectangle = e.ClipRectangle;
      int x = clipRectangle.Left - this.pnlPages.AutoScrollPosition.X;
      clipRectangle = e.ClipRectangle;
      int y = clipRectangle.Top - this.pnlPages.AutoScrollPosition.Y;
      clipRectangle = e.ClipRectangle;
      int width = clipRectangle.Width;
      clipRectangle = e.ClipRectangle;
      int height = clipRectangle.Height;
      local = new Rectangle(x, y, width, height);
      List<string> stringList = new List<string>();
      foreach (VirtualPageImage virtualPage1 in this.virtualPageList)
      {
        if (displayRect.IntersectsWith(virtualPage1.Bounds) && !stringList.Contains(virtualPage1.Page.ZipKey))
          stringList.Add(virtualPage1.Page.ZipKey);
        if (a.IntersectsWith(virtualPage1.Bounds))
        {
          Rectangle rectangle = Rectangle.Intersect(a, virtualPage1.Bounds);
          float num1 = Convert.ToSingle(virtualPage1.Page.Width) / (float) virtualPage1.Width;
          float num2 = Convert.ToSingle(virtualPage1.Page.Height) / (float) virtualPage1.Height;
          int int32_1 = Convert.ToInt32((float) (rectangle.Left - virtualPage1.Left) * num1);
          int int32_2 = Convert.ToInt32((float) (rectangle.Top - virtualPage1.Top) * num2);
          int int32_3 = Convert.ToInt32((float) rectangle.Width * num1);
          int int32_4 = Convert.ToInt32((float) rectangle.Height * num2);
          rectangle.Offset(this.pnlPages.AutoScrollPosition);
          using (Image image = Session.LoanDataMgr.FileAttachments.GetImage(virtualPage1.Page))
          {
            if (image != null)
            {
              e.Graphics.DrawImage(image, rectangle, int32_1, int32_2, int32_3, int32_4, GraphicsUnit.Pixel);
            }
            else
            {
              e.Graphics.FillRectangle(Brushes.White, rectangle);
              if (!this.downloadPageList.Contains(virtualPage1.Page))
                this.downloadPageList.Add(virtualPage1.Page);
              foreach (VirtualPageImage virtualPage2 in this.virtualPageList)
              {
                if (virtualPage2.Page.ZipKey == virtualPage1.Page.ZipKey && !this.downloadPageList.Contains(virtualPage2.Page))
                  this.downloadPageList.Add(virtualPage2.Page);
              }
            }
          }
          foreach (VirtualPageAnnotation annotation in virtualPage1.Annotations)
          {
            if (a.IntersectsWith(annotation.Bounds) && eFolderUIHelper.IsAnnotationVisible(annotation.Annotation))
            {
              rectangle = annotation.Bounds;
              rectangle.Offset(this.pnlPages.AutoScrollPosition);
              if (annotation.Annotation.Visibility == PageAnnotationVisibilityType.Personal)
                e.Graphics.DrawImage((Image) Resources.Annotation_Private, rectangle);
              if (annotation.Annotation.Visibility == PageAnnotationVisibilityType.Internal)
                e.Graphics.DrawImage((Image) Resources.Annotation_Internal, rectangle);
              if (annotation.Annotation.Visibility == PageAnnotationVisibilityType.Public)
                e.Graphics.DrawImage((Image) Resources.Annotation_Public, rectangle);
            }
          }
        }
      }
      List<PageImage> pageImageList = new List<PageImage>();
      foreach (PageImage downloadPage in this.downloadPageList)
      {
        if (!stringList.Contains(downloadPage.ZipKey))
          pageImageList.Add(downloadPage);
      }
      foreach (PageImage pageImage in pageImageList)
        this.downloadPageList.Remove(pageImage);
      if (this.downloadPageList.Count <= 0 || this.worker.IsBusy)
        return;
      Tracing.Log(PageImageListControl.sw, TraceLevel.Verbose, nameof (PageImageListControl), "Starting Worker");
      this.worker.RunWorkerAsync((object) this.downloadPageList.ToArray());
      Tracing.Log(PageImageListControl.sw, TraceLevel.Verbose, nameof (PageImageListControl), "Clearing Page Stack");
      this.downloadPageList.Clear();
    }

    private void pnlPages_MouseDown(object sender, MouseEventArgs e)
    {
      this.currentPage = (VirtualPageImage) null;
      this.selectedAnnotation = (VirtualPageAnnotation) null;
      if (!this.contextMenu.Enabled)
        return;
      int x = e.X - this.pnlPages.AutoScrollPosition.X;
      int y = e.Y - this.pnlPages.AutoScrollPosition.Y;
      VirtualPageImage pageAtPoint = this.getPageAtPoint(x, y);
      if (pageAtPoint == null)
        return;
      this.currentPage = pageAtPoint;
      VirtualPageAnnotation annotationAtPoint = pageAtPoint.GetAnnotationAtPoint(x, y);
      if (annotationAtPoint == null)
        return;
      this.selectedAnnotation = annotationAtPoint;
      this.dragPoint = e.Location;
    }

    private void pnlPages_MouseMove(object sender, MouseEventArgs e)
    {
      if (this.contextMenu.Enabled && this.dragPoint != Point.Empty && !this.dragPoint.Equals((object) e.Location))
      {
        int num = (int) this.pnlPages.DoDragDrop((object) this.selectedAnnotation, DragDropEffects.Move);
        this.dragPoint = Point.Empty;
      }
      Cursor annotationCursor = Cursors.Default;
      string caption = string.Empty;
      int x = e.X - this.pnlPages.AutoScrollPosition.X;
      int y = e.Y - this.pnlPages.AutoScrollPosition.Y;
      VirtualPageImage pageAtPoint = this.getPageAtPoint(x, y);
      if (pageAtPoint != null)
      {
        VirtualPageAnnotation annotationAtPoint = pageAtPoint.GetAnnotationAtPoint(x, y);
        if (annotationAtPoint != null)
          caption = annotationAtPoint.ToolTip;
        else if (this.chkAnnotate.Checked)
          annotationCursor = this.annotationCursor;
      }
      if (this.pnlPages.Cursor != annotationCursor)
        this.pnlPages.Cursor = annotationCursor;
      if (!(this.toolTip.GetToolTip((Control) this.pnlPages) != caption))
        return;
      this.toolTip.SetToolTip((Control) this.pnlPages, caption);
    }

    private void pnlPages_MouseClick(object sender, MouseEventArgs e)
    {
      int x = e.X - this.pnlPages.AutoScrollPosition.X;
      int y = e.Y - this.pnlPages.AutoScrollPosition.Y;
      VirtualPageImage pageAtPoint = this.getPageAtPoint(x, y);
      if (pageAtPoint == null)
        return;
      VirtualPageAnnotation annotationAtPoint = pageAtPoint.GetAnnotationAtPoint(x, y);
      if (annotationAtPoint != null)
      {
        if (e.Button != MouseButtons.Right)
          return;
        this.contextMenu.Tag = (object) annotationAtPoint;
        this.internalToolStripMenuItem.Checked = annotationAtPoint.Annotation.Visibility == PageAnnotationVisibilityType.Internal;
        this.personalToolStripMenuItem.Checked = annotationAtPoint.Annotation.Visibility == PageAnnotationVisibilityType.Personal;
        this.publicToolStripMenuItem.Checked = annotationAtPoint.Annotation.Visibility == PageAnnotationVisibilityType.Public;
        this.contextMenu.Show((Control) this.pnlPages, e.Location);
      }
      else
      {
        if (!this.chkAnnotate.Checked || e.Button != MouseButtons.Left)
          return;
        float num1 = Convert.ToSingle(pageAtPoint.Page.Width) / (float) pageAtPoint.Width;
        float num2 = Convert.ToSingle(pageAtPoint.Page.Height) / (float) pageAtPoint.Height;
        int int32_1 = Convert.ToInt32((float) (x - pageAtPoint.Left) * num1);
        int int32_2 = Convert.ToInt32((float) (y - pageAtPoint.Top) * num2);
        int int32_3 = Convert.ToInt32(0.25f * pageAtPoint.Page.HorizontalResolution);
        int int32_4 = Convert.ToInt32(0.25f * pageAtPoint.Page.VerticalResolution);
        this.createAnnotation(pageAtPoint, int32_1, int32_2, int32_3, int32_4);
      }
    }

    private void pnlPages_MouseUp(object sender, MouseEventArgs e)
    {
      this.dragPoint = Point.Empty;
      this.selectedAnnotation = (VirtualPageAnnotation) null;
    }

    private void pnlPages_GiveFeedback(object sender, GiveFeedbackEventArgs e)
    {
      e.UseDefaultCursors = e.Effect != DragDropEffects.Move;
    }

    private void pnlPages_DragOver(object sender, DragEventArgs e)
    {
      if (!this.contextMenu.Enabled)
        return;
      Point client = this.pnlPages.PointToClient(new Point(e.X - this.pnlPages.AutoScrollPosition.X, e.Y - this.pnlPages.AutoScrollPosition.Y));
      VirtualPageImage pageAtPoint = this.getPageAtPoint(client.X, client.Y);
      if (pageAtPoint == null)
        e.Effect = DragDropEffects.None;
      else if (pageAtPoint.Page.ImageKey != this.currentPage.Page.ImageKey)
      {
        e.Effect = DragDropEffects.None;
      }
      else
      {
        e.Effect = DragDropEffects.Move;
        if (this.selectedAnnotation.Annotation.Visibility == PageAnnotationVisibilityType.Personal)
          Cursor.Current = new Cursor(Resources.Annotation_Private_border.Handle);
        if (this.selectedAnnotation.Annotation.Visibility == PageAnnotationVisibilityType.Internal)
          Cursor.Current = new Cursor(Resources.Annotation_Internal_border.Handle);
        if (this.selectedAnnotation.Annotation.Visibility != PageAnnotationVisibilityType.Public)
          return;
        Cursor.Current = new Cursor(Resources.Annotation_Public_border.Handle);
      }
    }

    private void pnlPages_DragDrop(object sender, DragEventArgs e)
    {
      if (!this.contextMenu.Enabled)
        return;
      Point client = this.pnlPages.PointToClient(new Point(e.X - this.pnlPages.AutoScrollPosition.X, e.Y - this.pnlPages.AutoScrollPosition.Y));
      float num1 = Convert.ToSingle(this.currentPage.Page.Width) / (float) this.currentPage.Width;
      float num2 = Convert.ToSingle(this.currentPage.Page.Height) / (float) this.currentPage.Height;
      this.selectedAnnotation.Annotation.Left = Convert.ToInt32((float) (client.X - this.currentPage.Left) * num1);
      this.selectedAnnotation.Annotation.Top = Convert.ToInt32((float) (client.Y - this.currentPage.Top) * num2);
      this.LoadPages(this.attachmentList);
    }

    private void visibilityToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ((VirtualPageAnnotation) this.contextMenu.Tag).Annotation.Visibility = (PageAnnotationVisibilityType) Enum.Parse(typeof (PageAnnotationVisibilityType), (sender as ToolStripMenuItem).Text);
      this.LoadPages(this.attachmentList);
      this.OnAnnotationsChanged(EventArgs.Empty);
    }

    private void createAnnotation(
      VirtualPageImage virtualPage,
      int left,
      int top,
      int width,
      int height)
    {
      this.chkAnnotate.Checked = false;
      using (PageAnnotationDialog annotationDialog = new PageAnnotationDialog(left, top, width, height))
      {
        if (annotationDialog.ShowDialog((IWin32Window) this.ParentForm) != DialogResult.OK)
          return;
        long ticks = DateTime.Now.Ticks;
        virtualPage.Page.Annotations.Add(annotationDialog.Annotation);
        RemoteLogger.Write(TraceLevel.Info, "Annotated ImageAttachment: " + (object) TimeSpan.FromTicks(DateTime.Now.Ticks - ticks).TotalMilliseconds + " ms");
        this.LoadPages(this.attachmentList);
        this.OnAnnotationsChanged(EventArgs.Empty);
      }
    }

    private void pnlPages_MouseWheel(object sender, MouseEventArgs e)
    {
      this.OnScrollComplete(EventArgs.Empty);
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      string sourceFileName = (string) null;
      using (PdfFileBuilder pdfFileBuilder = new PdfFileBuilder(Session.LoanDataMgr))
      {
        sourceFileName = pdfFileBuilder.CreateFile((FileAttachment[]) this.attachmentList);
        if (sourceFileName == null)
          return;
      }
      using (SaveFileDialog saveFileDialog = new SaveFileDialog())
      {
        saveFileDialog.Filter = "Adobe PDF Documents (*.pdf)|*.pdf|All Files (*.*)|*.*";
        saveFileDialog.OverwritePrompt = true;
        if (saveFileDialog.ShowDialog((IWin32Window) Form.ActiveForm) != DialogResult.OK)
          return;
        File.Copy(sourceFileName, saveFileDialog.FileName, true);
      }
    }

    private void btnPrint_Click(object sender, EventArgs e)
    {
      using (PdfFileBuilder pdfFileBuilder = new PdfFileBuilder(Session.LoanDataMgr))
      {
        string file = pdfFileBuilder.CreateFile((FileAttachment[]) this.attachmentList);
        if (file == null)
          return;
        int currentPage = this.GetCurrentPage() + 1;
        using (PdfPrintDialog pdfPrintDialog = new PdfPrintDialog(file, currentPage))
        {
          int num = (int) pdfPrintDialog.ShowDialog((IWin32Window) Form.ActiveForm);
        }
      }
    }

    private void btnPrevious_Click(object sender, EventArgs e)
    {
      int pageIndex = this.GetCurrentPage() - 1;
      if (pageIndex < 0)
        return;
      this.SetCurrentPage(pageIndex);
    }

    private void btnNext_Click(object sender, EventArgs e)
    {
      int pageIndex = this.GetCurrentPage() + 1;
      if (pageIndex >= this.virtualPageList.Length)
        return;
      this.SetCurrentPage(pageIndex);
    }

    private void btnZoomIn_Click(object sender, EventArgs e)
    {
      if (this.cboZoom.Text.StartsWith("Fit"))
        this.cboZoom.SelectedItem = (object) "150%";
      else
        ++this.cboZoom.SelectedIndex;
    }

    private void btnZoomOut_Click(object sender, EventArgs e)
    {
      if (this.cboZoom.Text.StartsWith("Fit"))
        this.cboZoom.SelectedItem = (object) "50%";
      else
        --this.cboZoom.SelectedIndex;
    }

    private void cboZoom_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnZoomIn.Enabled = !this.cboZoom.Text.Equals("400%");
      this.btnZoomOut.Enabled = !this.cboZoom.Text.Equals("10%");
      this.calculateVirtualRectangles();
    }

    private void cboZoom_SelectionChangeCommitted(object sender, EventArgs e)
    {
      Session.WritePrivateProfileString(Environment.MachineName, "ZoomLevel", this.cboZoom.SelectedItem.ToString());
    }

    private void btnViewOriginal_Click(object sender, EventArgs e)
    {
      Tracing.Log(PageImageListControl.sw, TraceLevel.Verbose, nameof (PageImageListControl), "View in Original Format");
      try
      {
        List<string> stringList = new List<string>();
        foreach (ImageAttachment attachment in this.attachmentList)
        {
          foreach (PageImage page in attachment.Pages)
          {
            string str = Session.LoanDataMgr.FileAttachments.DownloadNative(page);
            if (str != null && !stringList.Contains(str))
              stringList.Add(str);
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
        Tracing.Log(PageImageListControl.sw, TraceLevel.Error, nameof (PageImageListControl), ex.ToString());
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "The following error occurred when trying to view the document in the original format:\n\n" + ex.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void mnuDelete_Click(object sender, EventArgs e)
    {
      if (!(this.contextMenu.Tag is VirtualPageAnnotation tag) || Utils.Dialog((IWin32Window) Form.ActiveForm, "Are you sure that you want to delete this annotation?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
        return;
      long ticks = DateTime.Now.Ticks;
      tag.Annotation.Page.Annotations.Remove(tag.Annotation);
      RemoteLogger.Write(TraceLevel.Info, "Annotated ImageAttachment: " + (object) TimeSpan.FromTicks(DateTime.Now.Ticks - ticks).TotalMilliseconds + " ms");
      this.LoadPages(this.attachmentList);
      this.OnAnnotationsChanged(EventArgs.Empty);
    }

    protected virtual void OnAnnotationsChanged(EventArgs e)
    {
      if (this.AnnotationsChanged == null)
        return;
      this.AnnotationsChanged((object) this, e);
    }

    public event EventHandler AnnotationsChanged;

    protected virtual void OnScrollComplete(EventArgs e)
    {
      if (this.ScrollComplete == null)
        return;
      this.ScrollComplete((object) this, e);
    }

    public event EventHandler ScrollComplete;

    public void CancelWorker()
    {
      Tracing.Log(PageImageListControl.sw, TraceLevel.Verbose, nameof (PageImageListControl), "Checking Worker");
      if (!this.worker.IsBusy)
        return;
      Tracing.Log(PageImageListControl.sw, TraceLevel.Verbose, nameof (PageImageListControl), "Cancelling Worker");
      this.worker.CancelAsync();
      Tracing.Log(PageImageListControl.sw, TraceLevel.Verbose, nameof (PageImageListControl), "Clearing DownloadPage List");
      this.downloadPageList.Clear();
    }

    private void worker_DoWork(object sender, DoWorkEventArgs e)
    {
      Tracing.Log(PageImageListControl.sw, TraceLevel.Verbose, nameof (PageImageListControl), "Worker Started");
      PageImage[] pageList = (PageImage[]) e.Argument;
      foreach (PageImage pageImage in pageList)
      {
        Session.LoanDataMgr.FileAttachments.PageDownloaded += new ExtractProgressEventHandler(this.worker_PageDownloaded);
        try
        {
          Session.LoanDataMgr.FileAttachments.DownloadPages(pageList);
        }
        catch (CanceledOperationException ex)
        {
          Tracing.Log(PageImageListControl.sw, TraceLevel.Verbose, nameof (PageImageListControl), "Worker Cancelled");
          break;
        }
        catch (Exception ex)
        {
          Tracing.Log(PageImageListControl.sw, TraceLevel.Error, nameof (PageImageListControl), ex.ToString());
          throw;
        }
        finally
        {
          Session.LoanDataMgr.FileAttachments.PageDownloaded -= new ExtractProgressEventHandler(this.worker_PageDownloaded);
        }
      }
    }

    private void worker_PageDownloaded(object source, ExtractProgressEventArgs e)
    {
      if (this.worker.CancellationPending)
        e.Cancel = true;
      this.worker.ReportProgress(e.PercentCompleted, (object) e.EntryName);
    }

    private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
      if (this.pnlPages.IsDisposed || this.virtualPageList == null)
        return;
      string userState = (string) e.UserState;
      Rectangle displayRect = this.getDisplayRect();
      foreach (VirtualPageImage virtualPage in this.virtualPageList)
      {
        if (!(virtualPage.Page.ImageKey != userState) && displayRect.IntersectsWith(virtualPage.Bounds))
        {
          Rectangle rc = Rectangle.Intersect(displayRect, virtualPage.Bounds);
          rc.Offset(this.pnlPages.AutoScrollPosition);
          this.pnlPages.Invalidate(rc);
        }
      }
    }

    private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      Tracing.Log(PageImageListControl.sw, TraceLevel.Verbose, nameof (PageImageListControl), "Worker Complete");
      if (this.downloadPageList.Count <= 0)
        return;
      Tracing.Log(PageImageListControl.sw, TraceLevel.Verbose, nameof (PageImageListControl), "Starting Worker");
      this.worker.RunWorkerAsync((object) this.downloadPageList.ToArray());
      Tracing.Log(PageImageListControl.sw, TraceLevel.Verbose, nameof (PageImageListControl), "Clearing Page Stack");
      this.downloadPageList.Clear();
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
      this.pnlBorders = new BorderPanel();
      this.pnlPages = new DoubleBufferedPanel();
      this.pnlScrollThumbnail = new Panel();
      this.pctScrollThumbnail = new PictureBox();
      this.lblScrollThumbnail = new Label();
      this.pnlHeader = new GradientPanel();
      this.pnlToolbar = new FlowLayoutPanel();
      this.btnSave = new StandardIconButton();
      this.btnPrint = new StandardIconButton();
      this.separator1 = new VerticalSeparator();
      this.btnPrevious = new StandardIconButton();
      this.btnNext = new StandardIconButton();
      this.separator2 = new VerticalSeparator();
      this.btnZoomOut = new StandardIconButton();
      this.btnZoomIn = new StandardIconButton();
      this.cboZoom = new ComboBox();
      this.separator3 = new VerticalSeparator();
      this.chkAnnotate = new IconCheckBox();
      this.separator4 = new VerticalSeparator();
      this.btnViewOriginal = new Button();
      this.toolTip = new ToolTip(this.components);
      this.contextMenu = new ContextMenuStrip(this.components);
      this.mnuDelete = new ToolStripMenuItem();
      this.toolStripMenuItem1 = new ToolStripSeparator();
      this.mnuVisibility = new ToolStripMenuItem();
      this.personalToolStripMenuItem = new ToolStripMenuItem();
      this.internalToolStripMenuItem = new ToolStripMenuItem();
      this.publicToolStripMenuItem = new ToolStripMenuItem();
      this.worker = new BackgroundWorker();
      this.pnlBorders.SuspendLayout();
      this.pnlPages.SuspendLayout();
      this.pnlScrollThumbnail.SuspendLayout();
      ((ISupportInitialize) this.pctScrollThumbnail).BeginInit();
      this.pnlHeader.SuspendLayout();
      this.pnlToolbar.SuspendLayout();
      ((ISupportInitialize) this.btnSave).BeginInit();
      ((ISupportInitialize) this.btnPrint).BeginInit();
      ((ISupportInitialize) this.btnPrevious).BeginInit();
      ((ISupportInitialize) this.btnNext).BeginInit();
      ((ISupportInitialize) this.btnZoomOut).BeginInit();
      ((ISupportInitialize) this.btnZoomIn).BeginInit();
      ((ISupportInitialize) this.chkAnnotate).BeginInit();
      this.contextMenu.SuspendLayout();
      this.SuspendLayout();
      this.pnlBorders.Borders = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlBorders.Controls.Add((Control) this.pnlPages);
      this.pnlBorders.Dock = DockStyle.Fill;
      this.pnlBorders.Location = new Point(0, 32);
      this.pnlBorders.Name = "pnlBorders";
      this.pnlBorders.Size = new Size(672, 217);
      this.pnlBorders.TabIndex = 4;
      this.pnlPages.AllowDrop = true;
      this.pnlPages.AutoScroll = true;
      this.pnlPages.BackColor = SystemColors.AppWorkspace;
      this.pnlPages.Controls.Add((Control) this.pnlScrollThumbnail);
      this.pnlPages.Dock = DockStyle.Fill;
      this.pnlPages.Location = new Point(1, 0);
      this.pnlPages.Name = "pnlPages";
      this.pnlPages.Size = new Size(670, 216);
      this.pnlPages.TabIndex = 5;
      this.pnlPages.ClientSizeChanged += new EventHandler(this.pnlPages_ClientSizeChanged);
      this.pnlPages.DragDrop += new DragEventHandler(this.pnlPages_DragDrop);
      this.pnlPages.DragOver += new DragEventHandler(this.pnlPages_DragOver);
      this.pnlPages.GiveFeedback += new GiveFeedbackEventHandler(this.pnlPages_GiveFeedback);
      this.pnlPages.Paint += new PaintEventHandler(this.pnlPages_Paint);
      this.pnlPages.MouseClick += new MouseEventHandler(this.pnlPages_MouseClick);
      this.pnlPages.MouseDown += new MouseEventHandler(this.pnlPages_MouseDown);
      this.pnlPages.MouseMove += new MouseEventHandler(this.pnlPages_MouseMove);
      this.pnlPages.MouseUp += new MouseEventHandler(this.pnlPages_MouseUp);
      this.pnlScrollThumbnail.BackColor = Color.Black;
      this.pnlScrollThumbnail.Controls.Add((Control) this.pctScrollThumbnail);
      this.pnlScrollThumbnail.Controls.Add((Control) this.lblScrollThumbnail);
      this.pnlScrollThumbnail.Location = new Point(515, 40);
      this.pnlScrollThumbnail.Name = "pnlScrollThumbnail";
      this.pnlScrollThumbnail.Padding = new Padding(8);
      this.pnlScrollThumbnail.Size = new Size(124, 116);
      this.pnlScrollThumbnail.TabIndex = 3;
      this.pnlScrollThumbnail.Visible = false;
      this.pctScrollThumbnail.BackColor = Color.White;
      this.pctScrollThumbnail.Dock = DockStyle.Fill;
      this.pctScrollThumbnail.Location = new Point(8, 8);
      this.pctScrollThumbnail.Name = "pctScrollThumbnail";
      this.pctScrollThumbnail.Size = new Size(108, 84);
      this.pctScrollThumbnail.TabIndex = 2;
      this.pctScrollThumbnail.TabStop = false;
      this.lblScrollThumbnail.Dock = DockStyle.Bottom;
      this.lblScrollThumbnail.ForeColor = Color.White;
      this.lblScrollThumbnail.Location = new Point(8, 92);
      this.lblScrollThumbnail.Name = "lblScrollThumbnail";
      this.lblScrollThumbnail.Size = new Size(108, 16);
      this.lblScrollThumbnail.TabIndex = 1;
      this.lblScrollThumbnail.Text = "Page 1";
      this.lblScrollThumbnail.TextAlign = ContentAlignment.BottomCenter;
      this.pnlHeader.Controls.Add((Control) this.pnlToolbar);
      this.pnlHeader.Dock = DockStyle.Top;
      this.pnlHeader.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.pnlHeader.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.pnlHeader.Location = new Point(0, 0);
      this.pnlHeader.Name = "pnlHeader";
      this.pnlHeader.Padding = new Padding(8, 0, 0, 0);
      this.pnlHeader.Size = new Size(672, 32);
      this.pnlHeader.Style = GradientPanel.PanelStyle.PageSubHeader;
      this.pnlHeader.TabIndex = 6;
      this.pnlToolbar.BackColor = Color.Transparent;
      this.pnlToolbar.Controls.Add((Control) this.btnSave);
      this.pnlToolbar.Controls.Add((Control) this.btnPrint);
      this.pnlToolbar.Controls.Add((Control) this.separator1);
      this.pnlToolbar.Controls.Add((Control) this.btnPrevious);
      this.pnlToolbar.Controls.Add((Control) this.btnNext);
      this.pnlToolbar.Controls.Add((Control) this.separator2);
      this.pnlToolbar.Controls.Add((Control) this.btnZoomOut);
      this.pnlToolbar.Controls.Add((Control) this.btnZoomIn);
      this.pnlToolbar.Controls.Add((Control) this.cboZoom);
      this.pnlToolbar.Controls.Add((Control) this.separator3);
      this.pnlToolbar.Controls.Add((Control) this.chkAnnotate);
      this.pnlToolbar.Controls.Add((Control) this.separator4);
      this.pnlToolbar.Controls.Add((Control) this.btnViewOriginal);
      this.pnlToolbar.Location = new Point(8, 5);
      this.pnlToolbar.Name = "pnlToolbar";
      this.pnlToolbar.Size = new Size(440, 22);
      this.pnlToolbar.TabIndex = 3;
      this.btnSave.BackColor = Color.Transparent;
      this.btnSave.Location = new Point(0, 3);
      this.btnSave.Margin = new Padding(0, 3, 4, 0);
      this.btnSave.MouseDownImage = (Image) null;
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(16, 16);
      this.btnSave.StandardButtonType = StandardIconButton.ButtonType.SaveButton;
      this.btnSave.TabIndex = 28;
      this.btnSave.TabStop = false;
      this.toolTip.SetToolTip((Control) this.btnSave, "Save To Disk");
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
      this.toolTip.SetToolTip((Control) this.btnPrint, "Print Document");
      this.btnPrint.Click += new EventHandler(this.btnPrint_Click);
      this.separator1.Location = new Point(40, 3);
      this.separator1.Margin = new Padding(0, 3, 3, 3);
      this.separator1.MaximumSize = new Size(2, 16);
      this.separator1.MinimumSize = new Size(2, 16);
      this.separator1.Name = "separator1";
      this.separator1.Size = new Size(2, 16);
      this.separator1.TabIndex = 4;
      this.separator1.TabStop = false;
      this.btnPrevious.BackColor = Color.Transparent;
      this.btnPrevious.Location = new Point(45, 3);
      this.btnPrevious.Margin = new Padding(0, 3, 4, 0);
      this.btnPrevious.MouseDownImage = (Image) null;
      this.btnPrevious.Name = "btnPrevious";
      this.btnPrevious.Size = new Size(16, 16);
      this.btnPrevious.StandardButtonType = StandardIconButton.ButtonType.UpArrowButton;
      this.btnPrevious.TabIndex = 25;
      this.btnPrevious.TabStop = false;
      this.toolTip.SetToolTip((Control) this.btnPrevious, "Previous Page");
      this.btnPrevious.Click += new EventHandler(this.btnPrevious_Click);
      this.btnNext.BackColor = Color.Transparent;
      this.btnNext.Location = new Point(65, 3);
      this.btnNext.Margin = new Padding(0, 3, 4, 0);
      this.btnNext.MouseDownImage = (Image) null;
      this.btnNext.Name = "btnNext";
      this.btnNext.Size = new Size(16, 16);
      this.btnNext.StandardButtonType = StandardIconButton.ButtonType.DownArrowButton;
      this.btnNext.TabIndex = 26;
      this.btnNext.TabStop = false;
      this.toolTip.SetToolTip((Control) this.btnNext, "Next Page");
      this.btnNext.Click += new EventHandler(this.btnNext_Click);
      this.separator2.Location = new Point(85, 3);
      this.separator2.Margin = new Padding(0, 3, 3, 3);
      this.separator2.MaximumSize = new Size(2, 16);
      this.separator2.MinimumSize = new Size(2, 16);
      this.separator2.Name = "separator2";
      this.separator2.Size = new Size(2, 16);
      this.separator2.TabIndex = 5;
      this.separator2.TabStop = false;
      this.btnZoomOut.BackColor = Color.Transparent;
      this.btnZoomOut.Location = new Point(90, 3);
      this.btnZoomOut.Margin = new Padding(0, 3, 4, 0);
      this.btnZoomOut.MouseDownImage = (Image) null;
      this.btnZoomOut.Name = "btnZoomOut";
      this.btnZoomOut.Size = new Size(16, 16);
      this.btnZoomOut.StandardButtonType = StandardIconButton.ButtonType.ZoomOutButton;
      this.btnZoomOut.TabIndex = 17;
      this.btnZoomOut.TabStop = false;
      this.toolTip.SetToolTip((Control) this.btnZoomOut, "Zoom Out");
      this.btnZoomOut.Click += new EventHandler(this.btnZoomOut_Click);
      this.btnZoomIn.BackColor = Color.Transparent;
      this.btnZoomIn.Location = new Point(110, 3);
      this.btnZoomIn.Margin = new Padding(0, 3, 4, 0);
      this.btnZoomIn.MouseDownImage = (Image) null;
      this.btnZoomIn.Name = "btnZoomIn";
      this.btnZoomIn.Size = new Size(16, 16);
      this.btnZoomIn.StandardButtonType = StandardIconButton.ButtonType.ZoomInButton;
      this.btnZoomIn.TabIndex = 18;
      this.btnZoomIn.TabStop = false;
      this.toolTip.SetToolTip((Control) this.btnZoomIn, "Zoom In");
      this.btnZoomIn.Click += new EventHandler(this.btnZoomIn_Click);
      this.cboZoom.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboZoom.FormattingEnabled = true;
      this.cboZoom.Items.AddRange(new object[12]
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
        (object) "400%"
      });
      this.cboZoom.Location = new Point(130, 0);
      this.cboZoom.Margin = new Padding(0, 0, 4, 0);
      this.cboZoom.Name = "cboZoom";
      this.cboZoom.Size = new Size(76, 21);
      this.cboZoom.TabIndex = 6;
      this.cboZoom.TabStop = false;
      this.cboZoom.SelectedIndexChanged += new EventHandler(this.cboZoom_SelectedIndexChanged);
      this.cboZoom.SelectionChangeCommitted += new EventHandler(this.cboZoom_SelectionChangeCommitted);
      this.separator3.Location = new Point(210, 3);
      this.separator3.Margin = new Padding(0, 3, 3, 3);
      this.separator3.MaximumSize = new Size(2, 16);
      this.separator3.MinimumSize = new Size(2, 16);
      this.separator3.Name = "separator3";
      this.separator3.Size = new Size(2, 16);
      this.separator3.TabIndex = 7;
      this.separator3.TabStop = false;
      this.chkAnnotate.BackColor = Color.Transparent;
      this.chkAnnotate.Checked = false;
      this.chkAnnotate.CheckedImage = (Image) Resources.notes_efolder_over;
      this.chkAnnotate.CheckedMouseOverImage = (Image) Resources.notes_efolder_over;
      this.chkAnnotate.DisabledImage = (Image) Resources.notes_efolder_disabled;
      this.chkAnnotate.Image = (Image) Resources.notes_efolder;
      this.chkAnnotate.Location = new Point(215, 3);
      this.chkAnnotate.Margin = new Padding(0, 3, 4, 0);
      this.chkAnnotate.Name = "chkAnnotate";
      this.chkAnnotate.Size = new Size(16, 16);
      this.chkAnnotate.TabIndex = 54;
      this.chkAnnotate.TabStop = false;
      this.toolTip.SetToolTip((Control) this.chkAnnotate, "Add Note");
      this.chkAnnotate.UncheckedImage = (Image) Resources.notes_efolder;
      this.chkAnnotate.UncheckedMouseOverImage = (Image) Resources.notes_efolder_over;
      this.separator4.Location = new Point(235, 3);
      this.separator4.Margin = new Padding(0, 3, 3, 3);
      this.separator4.MaximumSize = new Size(2, 16);
      this.separator4.MinimumSize = new Size(2, 16);
      this.separator4.Name = "separator4";
      this.separator4.Size = new Size(2, 16);
      this.separator4.TabIndex = 33;
      this.separator4.TabStop = false;
      this.btnViewOriginal.BackColor = Color.Transparent;
      this.btnViewOriginal.Location = new Point(240, 0);
      this.btnViewOriginal.Margin = new Padding(0, 0, 4, 0);
      this.btnViewOriginal.Name = "btnViewOriginal";
      this.btnViewOriginal.Size = new Size(135, 21);
      this.btnViewOriginal.TabIndex = 34;
      this.btnViewOriginal.Text = "View in Original Format";
      this.btnViewOriginal.UseVisualStyleBackColor = false;
      this.btnViewOriginal.Click += new EventHandler(this.btnViewOriginal_Click);
      this.contextMenu.Items.AddRange(new ToolStripItem[3]
      {
        (ToolStripItem) this.mnuDelete,
        (ToolStripItem) this.toolStripMenuItem1,
        (ToolStripItem) this.mnuVisibility
      });
      this.contextMenu.Name = "contextMenu";
      this.contextMenu.Size = new Size(119, 54);
      this.mnuDelete.Name = "mnuDelete";
      this.mnuDelete.Size = new Size(118, 22);
      this.mnuDelete.Text = "Delete";
      this.mnuDelete.Click += new EventHandler(this.mnuDelete_Click);
      this.toolStripMenuItem1.Name = "toolStripMenuItem1";
      this.toolStripMenuItem1.Size = new Size(115, 6);
      this.mnuVisibility.DropDownItems.AddRange(new ToolStripItem[3]
      {
        (ToolStripItem) this.personalToolStripMenuItem,
        (ToolStripItem) this.internalToolStripMenuItem,
        (ToolStripItem) this.publicToolStripMenuItem
      });
      this.mnuVisibility.Name = "mnuVisibility";
      this.mnuVisibility.Size = new Size(118, 22);
      this.mnuVisibility.Text = "Visibility";
      this.personalToolStripMenuItem.Name = "personalToolStripMenuItem";
      this.personalToolStripMenuItem.Size = new Size(119, 22);
      this.personalToolStripMenuItem.Text = "Personal";
      this.personalToolStripMenuItem.Click += new EventHandler(this.visibilityToolStripMenuItem_Click);
      this.internalToolStripMenuItem.Name = "internalToolStripMenuItem";
      this.internalToolStripMenuItem.Size = new Size(119, 22);
      this.internalToolStripMenuItem.Text = "Internal";
      this.internalToolStripMenuItem.Click += new EventHandler(this.visibilityToolStripMenuItem_Click);
      this.publicToolStripMenuItem.Name = "publicToolStripMenuItem";
      this.publicToolStripMenuItem.Size = new Size(119, 22);
      this.publicToolStripMenuItem.Text = "Public";
      this.publicToolStripMenuItem.Click += new EventHandler(this.visibilityToolStripMenuItem_Click);
      this.worker.WorkerReportsProgress = true;
      this.worker.WorkerSupportsCancellation = true;
      this.worker.DoWork += new DoWorkEventHandler(this.worker_DoWork);
      this.worker.ProgressChanged += new ProgressChangedEventHandler(this.worker_ProgressChanged);
      this.worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.worker_RunWorkerCompleted);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.pnlBorders);
      this.Controls.Add((Control) this.pnlHeader);
      this.Name = nameof (PageImageListControl);
      this.Size = new Size(672, 249);
      this.pnlBorders.ResumeLayout(false);
      this.pnlPages.ResumeLayout(false);
      this.pnlScrollThumbnail.ResumeLayout(false);
      ((ISupportInitialize) this.pctScrollThumbnail).EndInit();
      this.pnlHeader.ResumeLayout(false);
      this.pnlToolbar.ResumeLayout(false);
      ((ISupportInitialize) this.btnSave).EndInit();
      ((ISupportInitialize) this.btnPrint).EndInit();
      ((ISupportInitialize) this.btnPrevious).EndInit();
      ((ISupportInitialize) this.btnNext).EndInit();
      ((ISupportInitialize) this.btnZoomOut).EndInit();
      ((ISupportInitialize) this.btnZoomIn).EndInit();
      ((ISupportInitialize) this.chkAnnotate).EndInit();
      this.contextMenu.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private struct SCROLLINFO
    {
      public int cbSize;
      public uint fMask;
      public int nMin;
      public int nMax;
      public uint nPage;
      public int nPos;
      public int nTrackPos;
    }
  }
}
