// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.ListViewSortManager
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using System;
using System.Collections;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class ListViewSortManager
  {
    private string[] milestones;
    private bool m_multiSort;
    private ImageList m_images;
    private int m_column;
    private SortOrder m_sortOrder;
    private ListView m_list;
    private System.Type[] m_comparers;
    private int m_imgIndexAsc;
    private int m_imgIndexDesc;
    private const int HDI_WIDTH = 1;
    private const int HDI_HEIGHT = 1;
    private const int HDI_TEXT = 2;
    private const int HDI_FORMAT = 4;
    private const int HDI_LPARAM = 8;
    private const int HDI_BITMAP = 16;
    private const int HDI_IMAGE = 32;
    private const int HDI_DI_SETITEM = 64;
    private const int HDI_ORDER = 128;
    private const int HDI_FILTER = 256;
    private const int HDF_LEFT = 0;
    private const int HDF_RIGHT = 1;
    private const int HDF_CENTER = 2;
    private const int HDF_JUSTIFYMASK = 3;
    private const int HDF_RTLREADING = 4;
    private const int HDF_OWNERDRAW = 32768;
    private const int HDF_STRING = 16384;
    private const int HDF_BITMAP = 8192;
    private const int HDF_BITMAP_ON_RIGHT = 4096;
    private const int HDF_IMAGE = 2048;
    private const int HDF_SORTUP = 1024;
    private const int HDF_SORTDOWN = 512;
    private const int LVM_FIRST = 4096;
    private const int LVM_GETHEADER = 4127;
    private const int HDM_FIRST = 4608;
    private const int HDM_SETIMAGELIST = 4616;
    private const int HDM_GETIMAGELIST = 4617;
    private const int HDM_GETITEM = 4619;
    private const int HDM_SETITEM = 4620;

    public event EventHandler ColumnSorted;

    public ListViewSortManager(ListView list, System.Type[] comparers, string[] milestones)
      : this(list, comparers)
    {
      this.milestones = milestones;
    }

    public ListViewSortManager(ListView list, System.Type[] comparers)
      : this(list, comparers, false)
    {
    }

    public ListViewSortManager(ListView list, System.Type[] comparers, bool multiSort)
    {
      this.m_column = -1;
      this.m_sortOrder = SortOrder.None;
      this.m_multiSort = multiSort;
      this.m_list = list;
      this.m_comparers = comparers;
      this.m_images = list.CheckBoxes ? (ImageList) null : list.SmallImageList;
      if (this.m_images == null)
      {
        this.m_images = new ImageList();
        this.m_images.ImageSize = new Size(8, 8);
      }
      Color magenta = Color.Magenta;
      Size imageSize = this.m_images.ImageSize;
      this.m_imgIndexAsc = this.m_images.Images.Add((Image) this.GetArrowBitmap(ListViewSortManager.ArrowType.Ascending, imageSize, magenta), magenta);
      this.m_imgIndexDesc = this.m_images.Images.Add((Image) this.GetArrowBitmap(ListViewSortManager.ArrowType.Descending, imageSize, magenta), magenta);
      if (list.CheckBoxes)
        this.setHeaderImageList(list, this.m_images);
      else if (list.SmallImageList == null)
        list.SmallImageList = this.m_images;
      list.ColumnClick += new ColumnClickEventHandler(this.ColumnClick);
    }

    public int Column => this.m_column;

    public SortOrder SortOrder => this.m_sortOrder;

    public System.Type GetColumnComparerType(int column) => this.m_comparers[column];

    public void SetColumnComparerType(int column, System.Type comparerType)
    {
      this.m_comparers[column] = comparerType;
    }

    public void SetComparerTypes(System.Type[] comparers) => this.m_comparers = comparers;

    public void Sort(int column)
    {
      if (this.m_comparers[column] == typeof (ListViewShortCircuitSort))
        return;
      SortOrder order = SortOrder.Ascending;
      if (column == this.m_column)
        order = this.m_sortOrder == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
      this.Sort(column, order);
    }

    public void Sort(int column, SortOrder order)
    {
      if (column < 0 || column >= this.m_comparers.Length)
        throw new IndexOutOfRangeException();
      if (column != this.m_column)
      {
        this.ShowHeaderIcon(this.m_list, this.m_column, SortOrder.None);
        this.m_column = column;
      }
      this.ShowHeaderIcon(this.m_list, this.m_column, order);
      this.m_sortOrder = order;
      if (order == SortOrder.None)
        return;
      this.m_list.ListViewItemSorter = this.instantiateComparer(column, order);
    }

    public IComparer GetComparerForColumn(int column)
    {
      return this.instantiateComparer(column, this.m_sortOrder);
    }

    private IComparer instantiateComparer(int column, SortOrder order)
    {
      if (column < 0 || column >= this.m_comparers.Length)
        return (IComparer) null;
      ListViewSortManager listViewSortManager = (ListViewSortManager) null;
      if (this.m_multiSort)
        listViewSortManager = this;
      IComparer instance;
      if (this.m_comparers[column] == typeof (ListViewMilestoneSort))
        instance = (IComparer) Activator.CreateInstance(this.m_comparers[column], (object) column, (object) (this.m_sortOrder == SortOrder.Ascending), (object) this.milestones);
      else
        instance = (IComparer) Activator.CreateInstance(this.m_comparers[column], (object) column, (object) (this.m_sortOrder == SortOrder.Ascending), (object) listViewSortManager);
      return instance;
    }

    public void Disable() => this.m_list.ListViewItemSorter = (IComparer) null;

    public void Enable()
    {
      if (this.m_column < 0 || this.m_column >= this.m_comparers.Length)
        return;
      this.Sort(this.m_column, this.m_sortOrder);
    }

    private void ColumnClick(object sender, ColumnClickEventArgs e)
    {
      this.Sort(e.Column);
      if (this.ColumnSorted == null)
        return;
      this.ColumnSorted(sender, new EventArgs());
    }

    private Bitmap GetArrowBitmap(
      ListViewSortManager.ArrowType type,
      Size imageSize,
      Color transparentColor)
    {
      Bitmap arrowBitmap = new Bitmap(imageSize.Width, imageSize.Height);
      Graphics graphics = Graphics.FromImage((Image) arrowBitmap);
      Pen controlLightLight = SystemPens.ControlLightLight;
      Pen controlDark = SystemPens.ControlDark;
      using (Brush brush = (Brush) new SolidBrush(transparentColor))
        graphics.FillRectangle(brush, 0, 0, imageSize.Width, imageSize.Height);
      Point point = new Point((imageSize.Width - 8) / 2, (imageSize.Height - 8) / 2);
      switch (type)
      {
        case ListViewSortManager.ArrowType.Ascending:
          graphics.DrawLine(controlLightLight, point.X, point.Y + 7, point.X + 7, point.Y + 7);
          graphics.DrawLine(controlLightLight, point.X + 7, point.Y + 7, point.X + 4, point.Y);
          graphics.DrawLine(controlDark, point.X + 3, point.Y, point.X, point.Y + 7);
          break;
        case ListViewSortManager.ArrowType.Descending:
          graphics.DrawLine(controlLightLight, point.X + 4, point.Y + 7, point.X + 7, point.Y);
          graphics.DrawLine(controlDark, point.X + 3, point.Y + 7, point.X, point.Y);
          graphics.DrawLine(controlDark, point.X, point.Y, point.X + 7, point.Y);
          break;
      }
      graphics.Dispose();
      return arrowBitmap;
    }

    [DllImport("user32")]
    private static extern IntPtr SendMessage(IntPtr Handle, int msg, IntPtr wParam, IntPtr lParam);

    [DllImport("user32", EntryPoint = "SendMessage")]
    private static extern IntPtr SendMessage2(
      IntPtr Handle,
      int msg,
      IntPtr wParam,
      ref ListViewSortManager.HDITEM lParam);

    private void ShowHeaderIcon(ListView list, int columnIndex, SortOrder sortOrder)
    {
      if (columnIndex < 0 || columnIndex >= list.Columns.Count)
        return;
      IntPtr Handle = ListViewSortManager.SendMessage(list.Handle, 4127, IntPtr.Zero, IntPtr.Zero);
      ColumnHeader column = list.Columns[columnIndex];
      ListViewSortManager.HDITEM lParam = new ListViewSortManager.HDITEM();
      lParam.mask = 36;
      switch (column.TextAlign)
      {
        case HorizontalAlignment.Left:
          lParam.fmt = 20480;
          break;
        case HorizontalAlignment.Center:
          lParam.fmt = 20482;
          break;
        default:
          lParam.fmt = 16385;
          break;
      }
      switch (sortOrder)
      {
        case SortOrder.Ascending:
          lParam.fmt |= 2048;
          lParam.iImage = this.m_imgIndexAsc;
          break;
        case SortOrder.Descending:
          lParam.fmt |= 2048;
          lParam.iImage = this.m_imgIndexDesc;
          break;
        default:
          lParam.iImage = -1;
          break;
      }
      ListViewSortManager.SendMessage2(Handle, 4620, new IntPtr(columnIndex), ref lParam);
    }

    private void setHeaderImageList(ListView list, ImageList imgList)
    {
      ListViewSortManager.SendMessage(ListViewSortManager.SendMessage(list.Handle, 4127, IntPtr.Zero, IntPtr.Zero), 4616, IntPtr.Zero, imgList.Handle);
    }

    private enum ArrowType
    {
      Ascending,
      Descending,
    }

    private struct HDITEM
    {
      public int mask;
      public int cxy;
      [MarshalAs(UnmanagedType.LPTStr)]
      public string pszText;
      public IntPtr hbm;
      public int cchTextMax;
      public int fmt;
      public int lParam;
      public int iImage;
      public int iOrder;
    }
  }
}
