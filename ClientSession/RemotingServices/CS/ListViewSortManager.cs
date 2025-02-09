// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.CS.ListViewSortManager
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using System;
using System.Collections;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.RemotingServices.CS
{
  internal class ListViewSortManager
  {
    private string[] milestones;
    private bool m_multiSort;
    private int m_column;
    private SortOrder m_sortOrder;
    private ListView m_list;
    private System.Type[] m_comparers;
    private ImageList m_imgList;
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
      this.m_imgList = new ImageList();
      this.m_imgList.ImageSize = new Size(8, 8);
      this.m_imgList.TransparentColor = Color.Magenta;
      this.m_imgList.Images.Add((Image) this.GetArrowBitmap(ListViewSortManager.ArrowType.Ascending));
      this.m_imgList.Images.Add((Image) this.GetArrowBitmap(ListViewSortManager.ArrowType.Descending));
      this.SetHeaderImageList(this.m_list, this.m_imgList);
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

    private void ColumnClick(object sender, ColumnClickEventArgs e) => this.Sort(e.Column);

    private Bitmap GetArrowBitmap(ListViewSortManager.ArrowType type)
    {
      Bitmap arrowBitmap = new Bitmap(8, 8);
      Graphics graphics = Graphics.FromImage((Image) arrowBitmap);
      Pen controlLightLight = SystemPens.ControlLightLight;
      Pen controlDark = SystemPens.ControlDark;
      graphics.FillRectangle(Brushes.Magenta, 0, 0, 8, 8);
      switch (type)
      {
        case ListViewSortManager.ArrowType.Ascending:
          graphics.DrawLine(controlLightLight, 0, 7, 7, 7);
          graphics.DrawLine(controlLightLight, 7, 7, 4, 0);
          graphics.DrawLine(controlDark, 3, 0, 0, 7);
          break;
        case ListViewSortManager.ArrowType.Descending:
          graphics.DrawLine(controlLightLight, 4, 7, 7, 0);
          graphics.DrawLine(controlDark, 3, 7, 0, 0);
          graphics.DrawLine(controlDark, 0, 0, 7, 0);
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
      if (sortOrder != SortOrder.None)
        lParam.fmt |= 2048;
      lParam.iImage = (int) (sortOrder - 1);
      ListViewSortManager.SendMessage2(Handle, 4620, new IntPtr(columnIndex), ref lParam);
    }

    private void SetHeaderImageList(ListView list, ImageList imgList)
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
