// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Import.ListViewColumnSorter
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using System.Collections;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Import
{
  public class ListViewColumnSorter : IComparer
  {
    private int ColumnToSort;
    private SortOrder OrderOfSort;
    private CaseInsensitiveComparer ObjectCompare;
    private int TypeToSort;

    public ListViewColumnSorter()
    {
      this.ColumnToSort = 0;
      this.OrderOfSort = SortOrder.None;
      this.ObjectCompare = new CaseInsensitiveComparer();
    }

    public int Compare(object x, object y)
    {
      ListViewItem listViewItem1 = (ListViewItem) x;
      ListViewItem listViewItem2 = (ListViewItem) y;
      if (this.ColumnToSort >= listViewItem1.SubItems.Count || this.ColumnToSort >= listViewItem2.SubItems.Count)
        return 0;
      int num = this.TypeToSort != 1 ? this.ObjectCompare.Compare((object) listViewItem1.SubItems[this.ColumnToSort].Text, (object) listViewItem2.SubItems[this.ColumnToSort].Text) : this.ObjectCompare.Compare((object) this.DoubleValue(listViewItem1.SubItems[this.ColumnToSort].Text), (object) this.DoubleValue(listViewItem2.SubItems[this.ColumnToSort].Text));
      if (this.OrderOfSort == SortOrder.Ascending)
        return num;
      return this.OrderOfSort == SortOrder.Descending ? -num : 0;
    }

    private double DoubleValue(string strValue)
    {
      return strValue == string.Empty || strValue == null ? 0.0 : double.Parse(strValue.Replace(",", string.Empty));
    }

    public int SortColumn
    {
      set => this.ColumnToSort = value;
      get => this.ColumnToSort;
    }

    public int SortType
    {
      set => this.TypeToSort = value;
      get => this.TypeToSort;
    }

    public SortOrder Order
    {
      set => this.OrderOfSort = value;
      get => this.OrderOfSort;
    }
  }
}
