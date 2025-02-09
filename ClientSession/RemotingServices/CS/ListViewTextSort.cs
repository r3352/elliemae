// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.CS.ListViewTextSort
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using System.Collections;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.RemotingServices.CS
{
  internal class ListViewTextSort : IComparer
  {
    private ListViewSortManager m_sortMngr;
    private int m_column;
    private bool m_ascending;

    public ListViewTextSort(int sortColumn, bool ascending, ListViewSortManager sortMngr)
    {
      this.m_column = sortColumn;
      this.m_ascending = ascending;
      this.m_sortMngr = sortMngr;
    }

    public int Compare(object lhs, object rhs)
    {
      ListViewItem listViewItem1 = lhs as ListViewItem;
      ListViewItem listViewItem2 = rhs as ListViewItem;
      if (listViewItem1 == null || listViewItem2 == null)
        return 0;
      ListViewItem.ListViewSubItemCollection subItems1 = listViewItem1.SubItems;
      ListViewItem.ListViewSubItemCollection subItems2 = listViewItem2.SubItems;
      string lhs1 = subItems1.Count > this.m_column ? subItems1[this.m_column].Text : string.Empty;
      string str = subItems2.Count > this.m_column ? subItems2[this.m_column].Text : string.Empty;
      int num = lhs1.Length == 0 || str.Length == 0 ? lhs1.CompareTo(str) : this.OnCompare(lhs1, str);
      if (num == 0)
        num = this.OnNextCompare(lhs, rhs);
      if (!this.m_ascending)
        num = -num;
      return num;
    }

    protected virtual int OnCompare(string lhs, string rhs) => string.Compare(lhs, rhs, false);

    protected int OnNextCompare(object lhs, object rhs)
    {
      if (this.m_sortMngr == null)
        return 0;
      IComparer comparerForColumn = this.m_sortMngr.GetComparerForColumn(this.m_column + 1);
      if (comparerForColumn == null)
        return 0;
      int num = comparerForColumn.Compare(lhs, rhs);
      if (!this.m_ascending)
        num = -num;
      return num;
    }
  }
}
