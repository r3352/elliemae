// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Viewers.PageThumbnailListControl
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.eFolder;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.eFolder.Properties;
using EllieMae.EMLite.eFolder.UI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.Viewers
{
  public class PageThumbnailListControl : UserControl
  {
    private const string className = "PageThumbnailListControl";
    private static readonly string sw = Tracing.SwEFolder;
    private const int WM_LBUTTONDOWN = 513;
    private const int WM_LBUTTONDBLCLK = 515;
    private const int WM_RBUTTONDOWN = 516;
    private const int WM_RBUTTONDBLCLK = 518;
    private const int LVM_FIRST = 4096;
    private const int LVM_SETICONSPACING = 4149;
    private const int WM_VSCROLL = 277;
    private ImageAttachment[] attachmentList;
    private PageThumbnail[] thumbnailList;
    private Dictionary<string, int> thumbnailTable;
    private MsgHook lvwPagesHook;
    private Point dragPoint;
    private DateTime lastDragScrollDate;
    private int dropItemIndex = -1;
    private string highlightPosition = string.Empty;
    private bool loadThumbnails;
    private IContainer components;
    private GroupContainer gcPages;
    private ListView lvwPages;
    private FlowLayoutPanel pnlToolbar;
    private StandardIconButton btnDelete;
    private IconButton btnRotateRight;
    private IconButton btnRotateLeft;
    private ImageList imageList;
    private ToolTip toolTip;
    private BackgroundWorker worker;

    public PageThumbnailListControl()
    {
      this.InitializeComponent();
      this.lvwPagesHook = new MsgHook((Control) this.lvwPages);
      this.lvwPagesHook.CaptureMsg(513);
      this.lvwPagesHook.CaptureMsg(515);
      this.lvwPagesHook.CaptureMsg(516);
      this.lvwPagesHook.CaptureMsg(518);
      this.lvwPagesHook.OnMessage += new CallBackHandler(this.lvwPagesHook_OnMessage);
    }

    public ListView.SelectedIndexCollection SelectedIndices => this.lvwPages.SelectedIndices;

    public void LoadThumbnails(ImageAttachment[] attachmentList)
    {
      this.attachmentList = attachmentList;
      this.applySecurity();
      List<PageThumbnail> pageThumbnailList = new List<PageThumbnail>();
      foreach (ImageAttachment attachment in attachmentList)
      {
        foreach (PageImage page in attachment.Pages)
          pageThumbnailList.Add(page.Thumbnail);
      }
      this.thumbnailList = pageThumbnailList.ToArray();
      this.lvwPages.VirtualListSize = 0;
      this.thumbnailTable = new Dictionary<string, int>();
      this.imageList.Images.Clear();
      int num = 1;
      foreach (PageThumbnail thumbnail in this.thumbnailList)
      {
        if (thumbnail.Width > num)
          num = thumbnail.Width;
        if (thumbnail.Height > num)
          num = thumbnail.Height;
      }
      Size size = Size.Empty;
      using (Graphics graphics = Graphics.FromHwnd(this.lvwPages.Handle))
        size = Size.Ceiling(graphics.MeasureString("1", this.lvwPages.Font));
      int lParam = (int) (ushort) (num + 12) | num + 4 + size.Height + 6 << 16;
      EllieMae.EMLite.Common.Win32.SendMessage(this.lvwPages.Handle, 4149, IntPtr.Zero, (IntPtr) lParam);
      this.imageList.ImageSize = new Size(num, num);
      this.lvwPages.VirtualListSize = this.thumbnailList.Length;
      Tracing.Log(PageThumbnailListControl.sw, TraceLevel.Verbose, nameof (PageThumbnailListControl), "Checking Worker");
      if (this.worker.IsBusy)
      {
        Tracing.Log(PageThumbnailListControl.sw, TraceLevel.Verbose, nameof (PageThumbnailListControl), "Cancelling Worker");
        this.worker.CancelAsync();
        Tracing.Log(PageThumbnailListControl.sw, TraceLevel.Verbose, nameof (PageThumbnailListControl), "Setting LoadThumbnails Flag");
        this.loadThumbnails = true;
      }
      else
      {
        Tracing.Log(PageThumbnailListControl.sw, TraceLevel.Verbose, nameof (PageThumbnailListControl), "Starting Worker");
        this.worker.RunWorkerAsync((object) attachmentList);
        Tracing.Log(PageThumbnailListControl.sw, TraceLevel.Verbose, nameof (PageThumbnailListControl), "Clearing LoadThumbnails Flag");
        this.loadThumbnails = false;
      }
    }

    private void applySecurity()
    {
      bool flag = false;
      foreach (FileAttachment attachment in this.attachmentList)
      {
        flag = new eFolderAccessRights(Session.LoanDataMgr, (LogRecordBase) Session.LoanDataMgr.FileAttachments.GetLinkedDocument(attachment.ID)).CanDeletePageFromFile;
        if (!flag)
          break;
      }
      this.btnDelete.Visible = flag;
    }

    public void BeginUpdate() => this.lvwPages.BeginUpdate();

    public void EndUpdate() => this.lvwPages.EndUpdate();

    public void EnsureVisible(int index) => this.lvwPages.EnsureVisible(index);

    private void lvwPages_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
    {
      PageThumbnail thumbnail = this.thumbnailList[e.ItemIndex];
      if (!this.thumbnailTable.ContainsKey(thumbnail.ImageKey))
      {
        using (Bitmap bitmap = new Bitmap(this.imageList.ImageSize.Width, this.imageList.ImageSize.Height, PixelFormat.Format32bppArgb))
        {
          Rectangle rectangle = new Rectangle((bitmap.Width - thumbnail.Width) / 2, bitmap.Height - thumbnail.Height, thumbnail.Width, thumbnail.Height);
          using (Graphics graphic = Graphics.FromImage((Image) bitmap))
          {
            graphic.Clear(Color.Transparent);
            using (Image image = Session.LoanDataMgr.FileAttachments.GetImage(thumbnail))
            {
              if (image != null)
                graphic.DrawImage(image, rectangle);
              else
                graphic.FillRectangle(Brushes.White, rectangle);
            }
            ControlDraw.DrawBorder(rectangle, graphic, true, true, true, true);
          }
          this.imageList.Images.Add((Image) bitmap);
          this.thumbnailTable.Add(thumbnail.ImageKey, this.imageList.Images.Count - 1);
        }
      }
      int imageIndex = this.thumbnailTable[thumbnail.ImageKey];
      e.Item = new ListViewItem(Convert.ToString(e.ItemIndex + 1), imageIndex);
    }

    private void refreshImage(string imageKey)
    {
      if (this.lvwPages.IsDisposed)
        return;
      if (this.thumbnailTable.ContainsKey(imageKey))
        this.thumbnailTable.Remove(imageKey);
      for (int index = 0; index < this.thumbnailList.Length; ++index)
      {
        if (this.thumbnailList[index].ImageKey == imageKey)
          this.lvwPages.Invalidate(this.lvwPages.GetItemRect(index));
      }
    }

    private void lvwPages_MouseDown(object sender, MouseEventArgs e)
    {
      if (this.lvwPages.GetItemAt(e.X, e.Y) == null)
        return;
      this.dragPoint = e.Location;
    }

    private void lvwPages_MouseMove(object sender, MouseEventArgs e)
    {
      if (!(this.dragPoint != Point.Empty) || this.dragPoint.Equals((object) e.Location))
        return;
      List<PageImage> pageImageList = new List<PageImage>();
      foreach (int selectedIndex in this.lvwPages.SelectedIndices)
        pageImageList.Add(this.thumbnailList[selectedIndex].Page);
      int num = (int) this.lvwPages.DoDragDrop((object) pageImageList.ToArray(), DragDropEffects.Move);
      this.dragPoint = Point.Empty;
    }

    private void lvwPages_MouseUp(object sender, MouseEventArgs e) => this.dragPoint = Point.Empty;

    private void lvwPages_GiveFeedback(object sender, GiveFeedbackEventArgs e)
    {
      e.UseDefaultCursors = e.Effect != DragDropEffects.Move;
    }

    private void lvwPages_DragOver(object sender, DragEventArgs e)
    {
      Point client = this.lvwPages.PointToClient(new Point(e.X, e.Y));
      if (this.lastDragScrollDate <= DateTime.Now.AddSeconds(-0.15))
      {
        if (client.Y < this.lvwPages.Font.Height)
        {
          EllieMae.EMLite.Common.Win32.SendMessage(this.lvwPages.Handle, 277, (IntPtr) 0, IntPtr.Zero);
          this.lastDragScrollDate = DateTime.Now;
        }
        else if (client.Y > this.lvwPages.ClientSize.Height - this.lvwPages.Font.Height)
        {
          EllieMae.EMLite.Common.Win32.SendMessage(this.lvwPages.Handle, 277, (IntPtr) 1, IntPtr.Zero);
          this.lastDragScrollDate = DateTime.Now;
        }
      }
      if (e.Data.GetDataPresent(typeof (PageImage[])))
      {
        ListViewItem itemAt = this.lvwPages.GetItemAt(client.X, client.Y);
        if (itemAt == null)
        {
          e.Effect = DragDropEffects.None;
          this.highlightDropItem((ListViewItem) null);
        }
        else if (itemAt.Selected)
        {
          e.Effect = DragDropEffects.None;
          this.highlightDropItem((ListViewItem) null);
        }
        else
        {
          e.Effect = DragDropEffects.Move;
          eFolderUIHelper.SetPageThumbnailCursor((PageImage[]) e.Data.GetData(typeof (PageImage[])));
          Rectangle itemRect = this.lvwPages.GetItemRect(itemAt.Index);
          if (client.Y < itemRect.Top + itemRect.Height / 2)
            this.highlightDropItem(itemAt, "TOP");
          else
            this.highlightDropItem(itemAt, "BOTTOM");
        }
      }
      else
        e.Effect = DragDropEffects.None;
    }

    private void highlightDropItem(ListViewItem item) => this.highlightDropItem(item, string.Empty);

    private void highlightDropItem(ListViewItem item, string position)
    {
      int index = -1;
      if (item != null)
        index = item.Index;
      if (this.dropItemIndex == index && this.highlightPosition == position)
        return;
      if (this.dropItemIndex >= 0)
      {
        Rectangle itemRect = this.lvwPages.GetItemRect(this.dropItemIndex);
        this.lvwPages.Invalidate(!(this.highlightPosition == "TOP") ? new Rectangle(itemRect.Left + 15, itemRect.Bottom + 1, itemRect.Width - 15, 2) : new Rectangle(itemRect.Left + 15, itemRect.Top - 1, itemRect.Width - 15, 2));
        this.dropItemIndex = -1;
        this.highlightPosition = string.Empty;
      }
      if (index < 0)
        return;
      Rectangle itemRect1 = this.lvwPages.GetItemRect(index);
      Rectangle rect = !(position == "TOP") ? new Rectangle(itemRect1.Left + 15, itemRect1.Bottom + 1, itemRect1.Width - 15, 2) : new Rectangle(itemRect1.Left + 15, itemRect1.Top - 1, itemRect1.Width - 15, 2);
      using (Graphics graphics = Graphics.FromHwnd(this.lvwPages.Handle))
        graphics.FillRectangle(Brushes.Orange, rect);
      this.dropItemIndex = index;
      this.highlightPosition = position;
    }

    private void lvwPages_DragDrop(object sender, DragEventArgs e)
    {
      long ticks = DateTime.Now.Ticks;
      PageImage page1 = this.thumbnailList[this.dropItemIndex].Page;
      string str = this.highlightPosition;
      this.highlightDropItem((ListViewItem) null);
      DocumentLog linkedDocument = Session.LoanDataMgr.FileAttachments.GetLinkedDocument(page1.Attachment.ID);
      DocumentLog logEntry = (DocumentLog) null;
      bool flag1 = false;
      bool flag2 = false;
      bool flag3 = false;
      PageImage[] data = (PageImage[]) e.Data.GetData(typeof (PageImage[]));
      foreach (PageImage pageImage in data)
      {
        if (pageImage != page1)
        {
          if (pageImage.Attachment == page1.Attachment)
          {
            if (!new eFolderAccessRights(Session.LoanDataMgr, (LogRecordBase) linkedDocument).CanEditFile)
            {
              int num = (int) Utils.Dialog((IWin32Window) this, "You do not have rights to edit the '" + page1.Attachment.Title + "' file.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              return;
            }
            flag1 = true;
          }
          else
          {
            logEntry = Session.LoanDataMgr.FileAttachments.GetLinkedDocument(pageImage.Attachment.ID);
            eFolderAccessRights folderAccessRights = new eFolderAccessRights(Session.LoanDataMgr, (LogRecordBase) logEntry);
            if (!folderAccessRights.CanSplitFiles)
            {
              int num = (int) Utils.Dialog((IWin32Window) this, "You do not have rights to split the '" + pageImage.Attachment.Title + "' file.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              return;
            }
            flag2 = true;
            if (logEntry != linkedDocument)
            {
              if (!folderAccessRights.CanRemoveDocumentFiles)
              {
                int num = (int) Utils.Dialog((IWin32Window) this, "You do not have rights to remove the pages from the '" + pageImage.Attachment.Title + "' file.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
              }
              if (linkedDocument != null && !new eFolderAccessRights(Session.LoanDataMgr, (LogRecordBase) linkedDocument).CanAttachUnassignedFiles)
              {
                int num = (int) Utils.Dialog((IWin32Window) this, "You do not have rights to add pages from the '" + pageImage.Attachment.Title + "' file to the '" + linkedDocument.Title + "' document.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
              }
            }
            else
            {
              if (!new eFolderAccessRights(Session.LoanDataMgr, (LogRecordBase) linkedDocument).CanMergeFiles)
              {
                int num = (int) Utils.Dialog((IWin32Window) this, "You do not have rights to add pages from the '" + pageImage.Attachment.Title + "' file to the '" + page1.Attachment.Title + "' file.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
              }
              flag3 = true;
            }
          }
        }
      }
      foreach (PageImage page2 in data)
      {
        int index = page1.Attachment.Pages.IndexOf(page1) + 1;
        if (str == "TOP")
        {
          str = (string) null;
          --index;
        }
        page1.Attachment.Pages.Insert(index, page2);
        page1 = page2;
      }
      logEntry?.MarkLastUpdated();
      linkedDocument?.MarkLastUpdated();
      DateTime now;
      if (flag1)
      {
        now = DateTime.Now;
        RemoteLogger.Write(TraceLevel.Info, "Edited ImageAttachment: " + (object) TimeSpan.FromTicks(now.Ticks - ticks).TotalMilliseconds + " ms");
      }
      if (flag2)
      {
        now = DateTime.Now;
        RemoteLogger.Write(TraceLevel.Info, "Split ImageAttachment: " + (object) TimeSpan.FromTicks(now.Ticks - ticks).TotalMilliseconds + " ms");
      }
      if (flag3)
      {
        now = DateTime.Now;
        RemoteLogger.Write(TraceLevel.Info, "Merged ImageAttachment: " + (object) TimeSpan.FromTicks(now.Ticks - ticks).TotalMilliseconds + " ms");
      }
      this.LoadThumbnails(this.attachmentList);
      this.OnPagesChanged(EventArgs.Empty);
    }

    private void lvwPages_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.OnSelectedIndexChanged(EventArgs.Empty);
    }

    private void lvwPagesHook_OnMessage(ref System.Windows.Forms.Message m)
    {
      Point point = new Point(m.LParam.ToInt32());
      if (this.lvwPages.GetItemAt(point.X, point.Y) == null)
        return;
      this.lvwPagesHook.DefWndProc(ref m);
    }

    private void btnRotateLeft_Click(object sender, EventArgs e)
    {
      long ticks = DateTime.Now.Ticks;
      foreach (int selectedIndex in this.lvwPages.SelectedIndices)
      {
        for (int index = 0; index < 3; ++index)
          this.thumbnailList[selectedIndex].Page.Rotate();
        this.refreshImage(this.thumbnailList[selectedIndex].ImageKey);
      }
      RemoteLogger.Write(TraceLevel.Info, "Rotated ImageAttachment: " + (object) TimeSpan.FromTicks(DateTime.Now.Ticks - ticks).TotalMilliseconds + " ms");
      this.OnPagesChanged(EventArgs.Empty);
    }

    private void btnRotateRight_Click(object sender, EventArgs e)
    {
      long ticks = DateTime.Now.Ticks;
      foreach (int selectedIndex in this.lvwPages.SelectedIndices)
      {
        this.thumbnailList[selectedIndex].Page.Rotate();
        this.refreshImage(this.thumbnailList[selectedIndex].ImageKey);
      }
      RemoteLogger.Write(TraceLevel.Info, "Rotated ImageAttachment: " + (object) TimeSpan.FromTicks(DateTime.Now.Ticks - ticks).TotalMilliseconds + " ms");
      this.OnPagesChanged(EventArgs.Empty);
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
      if (Utils.Dialog((IWin32Window) Form.ActiveForm, "Are you sure that you want to delete the selected page(s)?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
        return;
      long ticks = DateTime.Now.Ticks;
      List<PageImage> pageImageList = new List<PageImage>();
      foreach (int selectedIndex in this.lvwPages.SelectedIndices)
        pageImageList.Add(this.thumbnailList[selectedIndex].Page);
      Dictionary<ImageAttachment, List<PageImage>> dictionary = new Dictionary<ImageAttachment, List<PageImage>>();
      foreach (PageImage pageImage in pageImageList)
      {
        if (pageImage.Attachment != null)
        {
          if (!dictionary.ContainsKey(pageImage.Attachment))
            dictionary.Add(pageImage.Attachment, new List<PageImage>()
            {
              pageImage
            });
          else
            dictionary[pageImage.Attachment].Add(pageImage);
        }
      }
      foreach (KeyValuePair<ImageAttachment, List<PageImage>> keyValuePair in dictionary)
        keyValuePair.Key.Pages.RemoveRange(keyValuePair.Value.ToArray());
      RemoteLogger.Write(TraceLevel.Info, "Edited ImageAttachment: " + (object) TimeSpan.FromTicks(DateTime.Now.Ticks - ticks).TotalMilliseconds + " ms");
      this.LoadThumbnails(this.attachmentList);
      this.OnPagesChanged(EventArgs.Empty);
    }

    protected virtual void OnPagesChanged(EventArgs e)
    {
      if (this.PagesChanged == null)
        return;
      this.PagesChanged((object) this, e);
    }

    public event EventHandler PagesChanged;

    protected virtual void OnSelectedIndexChanged(EventArgs e)
    {
      if (this.SelectedIndexChanged == null)
        return;
      this.SelectedIndexChanged((object) this, e);
    }

    public event EventHandler SelectedIndexChanged;

    public void CancelWorker()
    {
      Tracing.Log(PageThumbnailListControl.sw, TraceLevel.Verbose, nameof (PageThumbnailListControl), "Checking Worker");
      if (!this.worker.IsBusy)
        return;
      Tracing.Log(PageThumbnailListControl.sw, TraceLevel.Verbose, nameof (PageThumbnailListControl), "Cancelling Worker");
      this.worker.CancelAsync();
      Tracing.Log(PageThumbnailListControl.sw, TraceLevel.Verbose, nameof (PageThumbnailListControl), "Clearing LoadThumbnails Flag");
      this.loadThumbnails = false;
    }

    private void worker_DoWork(object sender, DoWorkEventArgs e)
    {
      Tracing.Log(PageThumbnailListControl.sw, TraceLevel.Verbose, nameof (PageThumbnailListControl), "Worker Started");
      foreach (ImageAttachment imageAttachment in (ImageAttachment[]) e.Argument)
      {
        Session.LoanDataMgr.FileAttachments.ThumbnailDownloaded += new ExtractProgressEventHandler(this.worker_ThumbnailDownloaded);
        try
        {
          Session.LoanDataMgr.FileAttachments.DownloadThumbnails(imageAttachment.Pages.ToArray());
        }
        catch (CanceledOperationException ex)
        {
          Tracing.Log(PageThumbnailListControl.sw, TraceLevel.Verbose, nameof (PageThumbnailListControl), "Worker Cancelled");
          break;
        }
        catch (Exception ex)
        {
          Tracing.Log(PageThumbnailListControl.sw, TraceLevel.Error, nameof (PageThumbnailListControl), ex.ToString());
          throw;
        }
        finally
        {
          Session.LoanDataMgr.FileAttachments.ThumbnailDownloaded -= new ExtractProgressEventHandler(this.worker_ThumbnailDownloaded);
        }
      }
    }

    private void worker_ThumbnailDownloaded(object source, ExtractProgressEventArgs e)
    {
      if (this.worker.CancellationPending)
        e.Cancel = true;
      this.worker.ReportProgress(e.PercentCompleted, (object) e.EntryName);
    }

    private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
      this.refreshImage((string) e.UserState);
    }

    private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      Tracing.Log(PageThumbnailListControl.sw, TraceLevel.Verbose, nameof (PageThumbnailListControl), "Worker Complete");
      if (!this.loadThumbnails)
        return;
      Tracing.Log(PageThumbnailListControl.sw, TraceLevel.Verbose, nameof (PageThumbnailListControl), "Starting Worker");
      this.worker.RunWorkerAsync((object) this.attachmentList);
      Tracing.Log(PageThumbnailListControl.sw, TraceLevel.Verbose, nameof (PageThumbnailListControl), "Clearing LoadThumbnails Flag");
      this.loadThumbnails = false;
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
      this.gcPages = new GroupContainer();
      this.pnlToolbar = new FlowLayoutPanel();
      this.btnDelete = new StandardIconButton();
      this.btnRotateRight = new IconButton();
      this.btnRotateLeft = new IconButton();
      this.lvwPages = new ListView();
      this.imageList = new ImageList(this.components);
      this.toolTip = new ToolTip(this.components);
      this.worker = new BackgroundWorker();
      this.gcPages.SuspendLayout();
      this.pnlToolbar.SuspendLayout();
      ((ISupportInitialize) this.btnDelete).BeginInit();
      ((ISupportInitialize) this.btnRotateRight).BeginInit();
      ((ISupportInitialize) this.btnRotateLeft).BeginInit();
      this.SuspendLayout();
      this.gcPages.Controls.Add((Control) this.pnlToolbar);
      this.gcPages.Controls.Add((Control) this.lvwPages);
      this.gcPages.Dock = DockStyle.Fill;
      this.gcPages.HeaderForeColor = SystemColors.ControlText;
      this.gcPages.Location = new Point(0, 0);
      this.gcPages.Name = "gcPages";
      this.gcPages.Size = new Size(360, 244);
      this.gcPages.TabIndex = 5;
      this.gcPages.Text = "Pages";
      this.pnlToolbar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.pnlToolbar.BackColor = Color.Transparent;
      this.pnlToolbar.Controls.Add((Control) this.btnDelete);
      this.pnlToolbar.Controls.Add((Control) this.btnRotateRight);
      this.pnlToolbar.Controls.Add((Control) this.btnRotateLeft);
      this.pnlToolbar.FlowDirection = FlowDirection.RightToLeft;
      this.pnlToolbar.Location = new Point(272, 2);
      this.pnlToolbar.Name = "pnlToolbar";
      this.pnlToolbar.Size = new Size(84, 22);
      this.pnlToolbar.TabIndex = 5;
      this.btnDelete.BackColor = Color.Transparent;
      this.btnDelete.Location = new Point(68, 3);
      this.btnDelete.Margin = new Padding(4, 3, 0, 3);
      this.btnDelete.MouseDownImage = (Image) null;
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new Size(16, 16);
      this.btnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDelete.TabIndex = 28;
      this.btnDelete.TabStop = false;
      this.toolTip.SetToolTip((Control) this.btnDelete, "Delete Page");
      this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
      this.btnRotateRight.BackColor = Color.Transparent;
      this.btnRotateRight.DisabledImage = (Image) Resources.rotate_disabled;
      this.btnRotateRight.Image = (Image) Resources.rotate;
      this.btnRotateRight.Location = new Point(48, 3);
      this.btnRotateRight.Margin = new Padding(4, 3, 0, 3);
      this.btnRotateRight.MouseDownImage = (Image) null;
      this.btnRotateRight.MouseOverImage = (Image) Resources.rotate_over;
      this.btnRotateRight.Name = "btnRotateRight";
      this.btnRotateRight.Size = new Size(16, 16);
      this.btnRotateRight.TabIndex = 33;
      this.btnRotateRight.TabStop = false;
      this.toolTip.SetToolTip((Control) this.btnRotateRight, "Rotate Right");
      this.btnRotateRight.Click += new EventHandler(this.btnRotateRight_Click);
      this.btnRotateLeft.BackColor = Color.Transparent;
      this.btnRotateLeft.DisabledImage = (Image) Resources.rotatecc_disabled;
      this.btnRotateLeft.Image = (Image) Resources.rotatecc;
      this.btnRotateLeft.Location = new Point(28, 3);
      this.btnRotateLeft.Margin = new Padding(4, 3, 0, 3);
      this.btnRotateLeft.MouseDownImage = (Image) null;
      this.btnRotateLeft.MouseOverImage = (Image) Resources.rotatecc_over;
      this.btnRotateLeft.Name = "btnRotateLeft";
      this.btnRotateLeft.Size = new Size(16, 16);
      this.btnRotateLeft.TabIndex = 34;
      this.btnRotateLeft.TabStop = false;
      this.toolTip.SetToolTip((Control) this.btnRotateLeft, "Rotate Left");
      this.btnRotateLeft.Click += new EventHandler(this.btnRotateLeft_Click);
      this.lvwPages.AllowDrop = true;
      this.lvwPages.BorderStyle = BorderStyle.None;
      this.lvwPages.Dock = DockStyle.Fill;
      this.lvwPages.HideSelection = false;
      this.lvwPages.LargeImageList = this.imageList;
      this.lvwPages.Location = new Point(1, 26);
      this.lvwPages.Name = "lvwPages";
      this.lvwPages.ShowGroups = false;
      this.lvwPages.Size = new Size(358, 217);
      this.lvwPages.TabIndex = 4;
      this.lvwPages.UseCompatibleStateImageBehavior = false;
      this.lvwPages.VirtualMode = true;
      this.lvwPages.SelectedIndexChanged += new EventHandler(this.lvwPages_SelectedIndexChanged);
      this.lvwPages.MouseUp += new MouseEventHandler(this.lvwPages_MouseUp);
      this.lvwPages.DragDrop += new DragEventHandler(this.lvwPages_DragDrop);
      this.lvwPages.MouseMove += new MouseEventHandler(this.lvwPages_MouseMove);
      this.lvwPages.RetrieveVirtualItem += new RetrieveVirtualItemEventHandler(this.lvwPages_RetrieveVirtualItem);
      this.lvwPages.MouseDown += new MouseEventHandler(this.lvwPages_MouseDown);
      this.lvwPages.GiveFeedback += new GiveFeedbackEventHandler(this.lvwPages_GiveFeedback);
      this.lvwPages.DragOver += new DragEventHandler(this.lvwPages_DragOver);
      this.imageList.ColorDepth = ColorDepth.Depth32Bit;
      this.imageList.ImageSize = new Size(16, 16);
      this.imageList.TransparentColor = Color.Transparent;
      this.worker.WorkerReportsProgress = true;
      this.worker.WorkerSupportsCancellation = true;
      this.worker.DoWork += new DoWorkEventHandler(this.worker_DoWork);
      this.worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.worker_RunWorkerCompleted);
      this.worker.ProgressChanged += new ProgressChangedEventHandler(this.worker_ProgressChanged);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.gcPages);
      this.Name = nameof (PageThumbnailListControl);
      this.Size = new Size(360, 244);
      this.gcPages.ResumeLayout(false);
      this.pnlToolbar.ResumeLayout(false);
      ((ISupportInitialize) this.btnDelete).EndInit();
      ((ISupportInitialize) this.btnRotateRight).EndInit();
      ((ISupportInitialize) this.btnRotateLeft).EndInit();
      this.ResumeLayout(false);
    }
  }
}
