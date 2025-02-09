// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.CS.ListViewFileSystemEntrySort
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using EllieMae.EMLite.Common;
using System.Collections;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.RemotingServices.CS
{
  internal class ListViewFileSystemEntrySort : IComparer
  {
    private int m_column;
    private bool m_ascending;

    public ListViewFileSystemEntrySort(int sortColumn, bool ascending)
    {
      this.m_column = sortColumn;
      this.m_ascending = ascending;
    }

    public ListViewFileSystemEntrySort(
      int sortColumn,
      bool ascending,
      ListViewSortManager sortMngr)
    {
      this.m_column = sortColumn;
      this.m_ascending = ascending;
    }

    public int Compare(object lhs, object rhs)
    {
      ListViewItem listViewItem1 = lhs as ListViewItem;
      ListViewItem listViewItem2 = rhs as ListViewItem;
      if (listViewItem1.SubItems[0].Text == "..")
        return this.m_ascending ? 0 : -1;
      if (listViewItem2.SubItems[0].Text == "..")
        return this.m_ascending ? -1 : 0;
      if (listViewItem1 == null || listViewItem2 == null)
        return 0;
      FileSystemEntry tag1 = listViewItem1.Tag as FileSystemEntry;
      FileSystemEntry tag2 = listViewItem2.Tag as FileSystemEntry;
      int num;
      if (tag1 == null || tag2 == null)
        num = string.Compare(listViewItem1.SubItems[this.m_column].Text, listViewItem2.SubItems[this.m_column].Text, true);
      else if (tag1.Type == tag2.Type)
      {
        string strA = "";
        if (this.m_column < listViewItem1.SubItems.Count)
          strA = listViewItem1.SubItems[this.m_column].Text;
        string strB = "";
        if (this.m_column < listViewItem2.SubItems.Count)
          strB = listViewItem2.SubItems[this.m_column].Text;
        num = string.Compare(strA, strB, true);
      }
      else
        num = tag1.Type != FileSystemEntry.Types.Folder ? 1 : -1;
      return (this.m_ascending ? 1 : -1) * num;
    }
  }
}
