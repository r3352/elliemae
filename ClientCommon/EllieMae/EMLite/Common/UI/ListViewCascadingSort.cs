// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.ListViewCascadingSort
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using System.Collections;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public abstract class ListViewCascadingSort : IComparer
  {
    private ListViewSortManager m_sortMngr;
    private int m_column;
    private bool m_ascending;
    private bool m_cascade = true;

    public ListViewCascadingSort(int sortColumn, bool ascending, ListViewSortManager sortMngr)
    {
      this.m_column = sortColumn;
      this.m_ascending = ascending;
      this.m_sortMngr = sortMngr;
    }

    public int Column => this.m_column;

    public bool Ascending => this.m_ascending;

    public ListViewSortManager SortManager => this.m_sortMngr;

    public bool Cascade
    {
      get => this.m_cascade;
      set => this.m_cascade = value;
    }

    public virtual int Compare(object lhs, object rhs)
    {
      ListViewItem lhs1 = lhs as ListViewItem;
      ListViewItem rhs1 = rhs as ListViewItem;
      if (lhs1 == null || rhs1 == null)
        return 0;
      int num = this.CompareItems(lhs1, rhs1);
      return num != 0 || !this.m_cascade ? num : this.CompareNext(lhs1, rhs1);
    }

    protected abstract int CompareItems(ListViewItem lhs, ListViewItem rhs);

    protected int CompareNext(ListViewItem lhs, ListViewItem rhs)
    {
      if (this.m_sortMngr == null)
        return 0;
      IComparer comparerForColumn = this.m_sortMngr.GetComparerForColumn(this.m_column + 1);
      return comparerForColumn == null ? 0 : comparerForColumn.Compare((object) lhs, (object) rhs);
    }
  }
}
