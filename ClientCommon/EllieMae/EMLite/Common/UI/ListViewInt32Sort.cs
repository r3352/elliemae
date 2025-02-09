// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.ListViewInt32Sort
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using System.Globalization;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class ListViewInt32Sort : ListViewTextSort
  {
    public ListViewInt32Sort(int sortColumn, bool ascending)
      : base(sortColumn, ascending, (ListViewSortManager) null)
    {
    }

    public ListViewInt32Sort(int sortColumn, bool ascending, ListViewSortManager sortMngr)
      : base(sortColumn, ascending, sortMngr)
    {
    }

    protected override int CompareText(string lhs, string rhs)
    {
      int num1 = this.convertToInt(lhs);
      int num2 = this.convertToInt(rhs);
      if (num1 > num2)
        return -1;
      return num1 < num2 ? 1 : 0;
    }

    private int convertToInt(string text)
    {
      if (text == "")
        return int.MinValue;
      try
      {
        return int.Parse(this.ConvertToNumericString(text), NumberStyles.Number);
      }
      catch
      {
        return int.MaxValue;
      }
    }
  }
}
