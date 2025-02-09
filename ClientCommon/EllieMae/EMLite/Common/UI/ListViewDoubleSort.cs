// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.ListViewDoubleSort
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using System.Globalization;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class ListViewDoubleSort : ListViewTextSort
  {
    public ListViewDoubleSort(int sortColumn, bool ascending)
      : base(sortColumn, ascending, (ListViewSortManager) null)
    {
    }

    public ListViewDoubleSort(int sortColumn, bool ascending, ListViewSortManager sortMngr)
      : base(sortColumn, ascending, sortMngr)
    {
    }

    protected override int CompareText(string lhs, string rhs)
    {
      double num1 = this.convertToDouble(lhs);
      double num2 = this.convertToDouble(rhs);
      if (num1 > num2)
        return -1;
      return num1 < num2 ? 1 : 0;
    }

    private double convertToDouble(string text)
    {
      if (text == "")
        return double.MinValue;
      try
      {
        return double.Parse(this.ConvertToNumericString(text), NumberStyles.Number);
      }
      catch
      {
        return double.MaxValue;
      }
    }
  }
}
