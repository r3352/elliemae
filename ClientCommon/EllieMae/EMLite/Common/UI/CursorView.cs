// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.CursorView
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class CursorView : ListViewEx
  {
    private ICursor dataCursor;
    private BitArray stateFlags;
    private int cursorSize = -1;
    private int chunkSize = 100;
    private int chunkCount = -1;
    private int lastChunkSize = -1;
    private string[] emptyData;
    private bool rowBufferingEnabled;
    private int rowBufferSize = 2500;
    private ListView baseView;
    private int lastBufferSize = -1;
    private CursorView.CursorItems items;
    private CursorView.CursorCheckedItems checkedItems;
    private CursorView.CursorCheckedIndices checkedIndices;
    private CursorView.CursorSelectedItems selectedItems;
    private CursorView.CursorSelectedIndices selectedIndices;
    private bool checkAll;
    private bool selectAll;
    private int sortColumn;
    private SortOrder sortOrder;
    private ImageList sortImages;
    private int sortImageIndexAsc = -1;
    private int sortImageIndexDesc = -1;
    private bool sorted = true;
    private bool forwardEvents = true;
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

    [Category("Data")]
    public event PopulateItemEventHandler PopulateItem;

    [Category("Data")]
    public event SortItemsEventHandler SortItems;

    [Category("Behavior")]
    public new event ItemCheckEventHandler ItemCheck;

    [Category("Behavior")]
    public new event EventHandler SelectedIndexChanged;

    public CursorView()
    {
      this.baseView = (ListView) this;
      this.items = new CursorView.CursorItems(this);
      this.checkedItems = new CursorView.CursorCheckedItems(this);
      this.selectedItems = new CursorView.CursorSelectedItems(this);
      base.ItemCheck += new ItemCheckEventHandler(this.onBaseViewItemCheck);
      base.SelectedIndexChanged += new EventHandler(this.onBaseViewSelectedIndexChanged);
    }

    [Browsable(false)]
    public ICursor DataSource
    {
      get => this.dataCursor;
      set
      {
        this.dataCursor = value;
        this.rebuildList();
      }
    }

    [Browsable(false)]
    public CursorView.CursorItems Items => this.items;

    [Browsable(false)]
    public CursorView.CursorCheckedItems CheckedItems => this.checkedItems;

    [Browsable(false)]
    public CursorView.CursorCheckedIndices CheckedIndices => this.checkedIndices;

    [Browsable(false)]
    public CursorView.CursorSelectedItems SelectedItems => this.selectedItems;

    [Browsable(false)]
    public CursorView.CursorSelectedIndices SelectedIndices => this.selectedIndices;

    [Category("Behavior")]
    [DefaultValue(100)]
    [Description("The number of items retrieved from the cursor in a single chunk when data is required")]
    public int DelayLoadChunkSize
    {
      get => this.chunkSize;
      set => this.chunkSize = value;
    }

    [Category("Behavior")]
    [DefaultValue(false)]
    [Description("Improves initial load performance by buffering rows and only loading them into the list when needed")]
    public bool RowBuffering
    {
      get => this.rowBufferingEnabled;
      set => this.rowBufferingEnabled = value;
    }

    [Category("Behavior")]
    [DefaultValue(2500)]
    [Description("Determines the number of rows that are loaded into the listview at once when RowBuffering is enabled.")]
    public int RowBufferSize
    {
      get => this.rowBufferSize;
      set => this.rowBufferSize = value;
    }

    public int SortColumn
    {
      get => this.sortColumn;
      set => this.Sort(value, this.sortOrder);
    }

    public SortOrder SortOrder
    {
      get => this.sortOrder;
      set => this.Sort(this.sortColumn, value);
    }

    public void Sort(int columnIndex, SortOrder sortOrder)
    {
      SortItemsEventArgs e = new SortItemsEventArgs(columnIndex, sortOrder);
      this.OnSortItems(e);
      if (e.Cancel)
        return;
      this.DisplaySort(columnIndex, sortOrder);
    }

    public void DisplaySort(int columnIndex, SortOrder sortOrder)
    {
      if (this.sortImageIndexAsc == -1)
        this.initSortImages();
      if (columnIndex != this.sortColumn && this.sortColumn >= 0)
        this.showHeaderIcon((ListView) this, this.sortColumn, SortOrder.None);
      this.showHeaderIcon((ListView) this, columnIndex, sortOrder);
      this.sortColumn = columnIndex;
      this.sortOrder = sortOrder;
    }

    protected override void WndProc(ref System.Windows.Forms.Message m)
    {
      if (m.Msg == 15 && this.dataCursor != null && this.cursorSize > 0)
      {
        ListViewItem topItem = this.TopItem;
        if (topItem != null)
          this.populateVisibleItems(topItem);
      }
      base.WndProc(ref m);
    }

    protected override void OnColumnClick(ColumnClickEventArgs e)
    {
      if (this.sorted)
      {
        if (e.Column == this.sortColumn)
          this.Sort(e.Column, this.sortOrder == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending);
        else
          this.Sort(e.Column, SortOrder.Ascending);
      }
      else
        base.OnColumnClick(e);
    }

    protected virtual void OnPopulateItem(PopulateItemEventArgs e)
    {
      if (this.PopulateItem == null)
        return;
      this.PopulateItem((object) this, e);
    }

    protected virtual void OnSortItems(SortItemsEventArgs e)
    {
      if (this.SortItems == null)
        return;
      this.SortItems((object) this, e);
    }

    protected new virtual void OnItemCheck(ItemCheckEventArgs e)
    {
      if (this.ItemCheck == null)
        return;
      this.ItemCheck((object) this, e);
    }

    protected new virtual void OnSelectedIndexChanged(EventArgs e)
    {
      if (this.SelectedIndexChanged == null)
        return;
      this.SelectedIndexChanged((object) this, e);
    }

    private void onBaseViewItemCheck(object sender, ItemCheckEventArgs e)
    {
      if (!this.forwardEvents)
        return;
      this.OnItemCheck(e);
    }

    private void onBaseViewSelectedIndexChanged(object sender, EventArgs e)
    {
      if (!this.forwardEvents)
        return;
      if (this.baseView.SelectedItems.Count != this.baseView.Items.Count)
        this.selectAll = false;
      this.OnSelectedIndexChanged(e);
    }

    private void rebuildList()
    {
      this.baseView.Items.Clear();
      this.OnSelectedIndexChanged(EventArgs.Empty);
      this.stateFlags = (BitArray) null;
      this.cursorSize = -1;
      this.checkAll = false;
      this.selectAll = false;
      this.lastBufferSize = this.rowBufferSize;
      this.emptyData = new string[this.Columns.Count];
      for (int index = 0; index < this.emptyData.Length; ++index)
        this.emptyData[index] = "";
      if (this.dataCursor == null)
        return;
      this.cursorSize = this.dataCursor.GetItemCount();
      if (this.rowBufferingEnabled)
        this.ensureRowsCreated(Math.Min(this.cursorSize - 1, this.RowBufferSize));
      else
        this.ensureRowsCreated(this.cursorSize - 1);
      this.lastChunkSize = this.cursorSize % this.chunkSize;
      if (this.lastChunkSize == 0)
      {
        this.chunkCount = this.cursorSize / this.chunkSize;
        this.lastChunkSize = this.chunkSize;
      }
      else
        this.chunkCount = this.cursorSize / this.chunkSize + 1;
      this.stateFlags = new BitArray(this.chunkCount);
    }

    private bool isItemPopulated(int index) => this.stateFlags[this.getChunkIndexForItem(index)];

    private bool isItemPopulated(ListViewItem item) => this.isItemPopulated(item.Index);

    private void ensureItemPopulated(int index)
    {
      this.ensureChunkLoaded(this.getChunkIndexForItem(index));
    }

    private void ensureItemPopulated(ListViewItem item) => this.ensureItemPopulated(item.Index);

    private void populateVisibleItems(ListViewItem topItem)
    {
      int index1 = Math.Min(topItem.Index, this.cursorSize - 1);
      int height = topItem.GetBounds(ItemBoundsPortion.Entire).Height;
      if (height == 0)
        return;
      int num = this.ClientSize.Height / height + 1;
      int index2 = Math.Min(index1 + num, this.cursorSize - 1);
      int chunkIndexForItem1 = this.getChunkIndexForItem(index1);
      int chunkIndexForItem2 = this.getChunkIndexForItem(index2);
      for (int chunkIndex = chunkIndexForItem1; chunkIndex <= chunkIndexForItem2; ++chunkIndex)
        this.ensureChunkLoaded(chunkIndex);
    }

    private int getChunkIndexForItem(int index)
    {
      return Math.Min(index / this.chunkSize, this.chunkCount - 1);
    }

    private int getChunkStartIndex(int chunkIndex) => chunkIndex * this.chunkSize;

    private int getChunkSize(int chunkIndex)
    {
      return chunkIndex < this.chunkCount - 1 ? this.chunkSize : this.lastChunkSize;
    }

    private void ensureChunkLoaded(int chunkIndex)
    {
      if (chunkIndex < 0 || this.stateFlags[chunkIndex])
        return;
      this.stateFlags[chunkIndex] = true;
      int chunkStartIndex = this.getChunkStartIndex(chunkIndex);
      object[] items = this.dataCursor.GetItems(chunkStartIndex, this.getChunkSize(chunkIndex), false);
      for (int index = 0; index < items.Length; ++index)
      {
        int num = chunkStartIndex + index;
        this.ensureRowsCreated(num);
        ListViewItem listViewItem = this.baseView.Items[num];
        if (listViewItem.Index != num)
          throw new InvalidOperationException("Items cannot be populated at this time");
        this.OnPopulateItem(new PopulateItemEventArgs(listViewItem, items[index]));
      }
      if (chunkIndex >= this.chunkCount - 1 || this.baseView.Items.Count >= this.cursorSize || this.baseView.Items.Count >= chunkStartIndex + this.rowBufferSize)
        return;
      this.ensureRowsCreated(Math.Min(this.baseView.Items.Count + this.rowBufferSize, this.cursorSize - 1));
    }

    private int getBufferedRowCount() => Math.Max(this.cursorSize - this.baseView.Items.Count, 0);

    private void ensureRowsCreated(int maxIndex)
    {
      this.forwardEvents = false;
      try
      {
        for (int count = this.baseView.Items.Count; count <= maxIndex; ++count)
          this.baseView.Items.Add(new ListViewItem(this.emptyData)
          {
            Checked = this.checkAll,
            Selected = this.selectAll
          });
      }
      finally
      {
        this.forwardEvents = true;
      }
    }

    private void ensureAllRowsCreated() => this.ensureRowsCreated(this.cursorSize - 1);

    private void initSortImages()
    {
      Color magenta = Color.Magenta;
      this.sortImages = this.CheckBoxes ? (ImageList) null : this.SmallImageList;
      if (this.sortImages == null)
      {
        this.sortImages = new ImageList();
        this.sortImages.ImageSize = new Size(8, 8);
      }
      Size imageSize = this.sortImages.ImageSize;
      this.sortImageIndexAsc = this.sortImages.Images.Add((Image) CursorView.getArrowBitmap(CursorView.ArrowType.Ascending, imageSize, magenta), magenta);
      this.sortImageIndexDesc = this.sortImages.Images.Add((Image) CursorView.getArrowBitmap(CursorView.ArrowType.Descending, imageSize, magenta), magenta);
      if (this.CheckBoxes)
      {
        this.setHeaderImageList((ListView) this, this.sortImages);
      }
      else
      {
        if (this.SmallImageList != null)
          return;
        this.SmallImageList = this.sortImages;
      }
    }

    private static Bitmap getArrowBitmap(
      CursorView.ArrowType type,
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
        case CursorView.ArrowType.Ascending:
          graphics.DrawLine(controlLightLight, point.X, point.Y + 7, point.X + 7, point.Y + 7);
          graphics.DrawLine(controlLightLight, point.X + 7, point.Y + 7, point.X + 4, point.Y);
          graphics.DrawLine(controlDark, point.X + 3, point.Y, point.X, point.Y + 7);
          break;
        case CursorView.ArrowType.Descending:
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
      ref CursorView.HDITEM lParam);

    private void showHeaderIcon(ListView list, int columnIndex, SortOrder sortOrder)
    {
      if (columnIndex < 0 || columnIndex >= list.Columns.Count)
        return;
      IntPtr Handle = CursorView.SendMessage(list.Handle, 4127, IntPtr.Zero, IntPtr.Zero);
      ColumnHeader column = list.Columns[columnIndex];
      CursorView.HDITEM lParam = new CursorView.HDITEM();
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
          lParam.iImage = this.sortImageIndexAsc;
          break;
        case SortOrder.Descending:
          lParam.fmt |= 2048;
          lParam.iImage = this.sortImageIndexDesc;
          break;
        default:
          lParam.iImage = -1;
          break;
      }
      CursorView.SendMessage2(Handle, 4620, new IntPtr(columnIndex), ref lParam);
    }

    private void setHeaderImageList(ListView list, ImageList imgList)
    {
      CursorView.SendMessage(CursorView.SendMessage(list.Handle, 4127, IntPtr.Zero, IntPtr.Zero), 4616, IntPtr.Zero, imgList.Handle);
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

    public interface IListItemEnumerable : IEnumerable
    {
      int Count { get; }

      ListViewItem this[int index] { get; }
    }

    public interface IListIndexEnumerable : IEnumerable
    {
      int Count { get; }

      int this[int index] { get; }
    }

    public class CursorItems : CursorView.IListItemEnumerable, IEnumerable
    {
      private CursorView view;

      internal CursorItems(CursorView view) => this.view = view;

      public int Count => Math.Max(this.view.cursorSize, this.view.baseView.Items.Count);

      public ListViewItem this[int index]
      {
        get
        {
          this.view.ensureItemPopulated(index);
          return this.view.baseView.Items[index];
        }
      }

      public ListViewItem Add(ListViewItem item)
      {
        this.view.ensureAllRowsCreated();
        return this.view.baseView.Items.Add(item);
      }

      public IEnumerator GetEnumerator()
      {
        return (IEnumerator) new CursorView.CursorItemEnumerator((CursorView.IListItemEnumerable) this);
      }
    }

    public class CursorCheckedItems : CursorView.IListItemEnumerable, IEnumerable
    {
      private CursorView view;

      internal CursorCheckedItems(CursorView view) => this.view = view;

      public int Count
      {
        get
        {
          return this.view.checkAll ? this.view.baseView.CheckedItems.Count + this.view.getBufferedRowCount() : this.view.baseView.CheckedItems.Count;
        }
      }

      public ListViewItem this[int index]
      {
        get
        {
          if (index < this.view.baseView.CheckedIndices.Count)
            return this.view.Items[this.view.baseView.CheckedIndices[index]];
          if (index < this.Count)
            return this.view.Items[this.view.baseView.Items.Count + index - this.view.baseView.CheckedIndices.Count];
          throw new ArgumentOutOfRangeException();
        }
      }

      public void Clear()
      {
        this.view.checkAll = false;
        foreach (ListViewItem listViewItem in this.view.baseView.Items)
          listViewItem.Checked = false;
      }

      public void CheckAll()
      {
        this.view.checkAll = true;
        foreach (ListViewItem listViewItem in this.view.baseView.Items)
          listViewItem.Checked = true;
      }

      public IEnumerator GetEnumerator()
      {
        return (IEnumerator) new CursorView.CursorFastItemEnumerator(this.view, (ICollection) this.view.baseView.CheckedIndices, this.view.checkAll);
      }
    }

    public class CursorCheckedIndices : CursorView.IListIndexEnumerable, IEnumerable
    {
      private CursorView.CursorCheckedItems items;

      internal CursorCheckedIndices(CursorView view)
      {
        this.items = new CursorView.CursorCheckedItems(view);
      }

      public int Count => this.items.Count;

      public int this[int index] => this.items[index].Index;

      public void Clear() => this.items.Clear();

      public IEnumerator GetEnumerator()
      {
        return (IEnumerator) new CursorView.CursorIndexEnumerator((CursorView.IListIndexEnumerable) this);
      }
    }

    public class CursorSelectedItems : CursorView.IListItemEnumerable, IEnumerable
    {
      private CursorView view;

      internal CursorSelectedItems(CursorView view) => this.view = view;

      public int Count
      {
        get
        {
          return this.view.selectAll ? this.view.baseView.SelectedItems.Count + this.view.getBufferedRowCount() : this.view.baseView.SelectedItems.Count;
        }
      }

      public ListViewItem this[int index]
      {
        get
        {
          if (index < this.view.baseView.SelectedIndices.Count)
            return this.view.Items[this.view.baseView.SelectedIndices[index]];
          if (index < this.Count)
            return this.view.Items[this.view.baseView.Items.Count + index - this.view.baseView.SelectedIndices.Count];
          throw new ArgumentOutOfRangeException();
        }
      }

      public void Clear()
      {
        this.view.selectAll = false;
        this.view.baseView.SelectedItems.Clear();
      }

      public void SelectAll()
      {
        this.view.forwardEvents = false;
        foreach (ListViewItem listViewItem in this.view.baseView.Items)
          listViewItem.Selected = true;
        this.view.selectAll = true;
        this.view.forwardEvents = true;
        this.view.onBaseViewSelectedIndexChanged((object) null, EventArgs.Empty);
      }

      public IEnumerator GetEnumerator()
      {
        return (IEnumerator) new CursorView.CursorFastItemEnumerator(this.view, (ICollection) this.view.baseView.SelectedIndices, this.view.selectAll);
      }
    }

    public class CursorSelectedIndices : CursorView.IListIndexEnumerable, IEnumerable
    {
      private CursorView.CursorSelectedItems items;

      internal CursorSelectedIndices(CursorView view)
      {
        this.items = new CursorView.CursorSelectedItems(view);
      }

      public int Count => this.items.Count;

      public int this[int index] => this.items[index].Index;

      public void Clear() => this.items.Clear();

      public IEnumerator GetEnumerator()
      {
        return (IEnumerator) new CursorView.CursorIndexEnumerator((CursorView.IListIndexEnumerable) this);
      }
    }

    private class CursorItemEnumerator : IEnumerator
    {
      private int index = -1;
      private CursorView.IListItemEnumerable enumObject;

      internal CursorItemEnumerator(CursorView.IListItemEnumerable enumObject)
      {
        this.enumObject = enumObject;
      }

      public void Reset() => this.index = -1;

      public object Current => (object) this.enumObject[this.index];

      public bool MoveNext() => ++this.index < this.enumObject.Count;
    }

    private class CursorIndexEnumerator : IEnumerator
    {
      private int index = -1;
      private CursorView.IListIndexEnumerable enumObject;

      internal CursorIndexEnumerator(CursorView.IListIndexEnumerable enumObject)
      {
        this.enumObject = enumObject;
      }

      public void Reset() => this.index = -1;

      public object Current => (object) this.enumObject[this.index];

      public bool MoveNext() => ++this.index < this.enumObject.Count;
    }

    private class CursorFastItemEnumerator : IEnumerator
    {
      private int index = -1;
      private CursorView view;
      private int[] baseIndexArray;
      private int baseViewSize;
      private int count;

      internal CursorFastItemEnumerator(
        CursorView view,
        ICollection indexCollection,
        bool enumerateBuffered)
      {
        this.view = view;
        this.baseViewSize = view.baseView.Items.Count;
        this.baseIndexArray = new int[indexCollection.Count];
        indexCollection.CopyTo((Array) this.baseIndexArray, 0);
        this.count = this.baseIndexArray.Length;
        if (!enumerateBuffered)
          return;
        this.count += view.getBufferedRowCount();
      }

      public void Reset() => this.index = -1;

      public object Current
      {
        get
        {
          return this.index < this.baseIndexArray.Length ? (object) this.view.Items[this.baseIndexArray[this.index]] : (object) this.view.Items[this.baseViewSize + this.index - this.baseIndexArray.Length];
        }
      }

      public bool MoveNext() => ++this.index < this.count;
    }
  }
}
