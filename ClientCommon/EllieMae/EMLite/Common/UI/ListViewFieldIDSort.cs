// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.ListViewFieldIDSort
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class ListViewFieldIDSort : ListViewTextCaseInsensitiveSort
  {
    public ListViewFieldIDSort(int sortColumn, bool ascending)
      : base(sortColumn, ascending, (ListViewSortManager) null)
    {
    }

    public ListViewFieldIDSort(int sortColumn, bool ascending, ListViewSortManager sortMngr)
      : base(sortColumn, ascending, sortMngr)
    {
    }

    protected override int CompareText(string lhs, string rhs)
    {
      int num1 = -1;
      int num2 = -1;
      try
      {
        if (char.IsDigit(lhs[0]))
          num1 = int.Parse(lhs);
      }
      catch
      {
      }
      try
      {
        if (char.IsDigit(rhs[0]))
          num2 = int.Parse(rhs);
      }
      catch
      {
      }
      if (num1 > 0 && num2 > 0)
        return num1 - num2;
      if (num1 > 0)
        return -1;
      return num2 > 0 ? 1 : base.CompareText(lhs, rhs);
    }
  }
}
