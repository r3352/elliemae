// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.ListViewTextCaseInsensitiveSort
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class ListViewTextCaseInsensitiveSort : ListViewTextSort
  {
    public ListViewTextCaseInsensitiveSort(int sortColumn, bool ascending)
      : base(sortColumn, ascending, (ListViewSortManager) null)
    {
    }

    public ListViewTextCaseInsensitiveSort(
      int sortColumn,
      bool ascending,
      ListViewSortManager sortMngr)
      : base(sortColumn, ascending, sortMngr)
    {
    }

    protected override int CompareText(string lhs, string rhs) => string.Compare(lhs, rhs, true);
  }
}
