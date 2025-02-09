// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.ListViewCurrencySort
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using System;
using System.Globalization;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class ListViewCurrencySort : ListViewTextSort
  {
    public ListViewCurrencySort(int sortColumn, bool ascending)
      : base(sortColumn, ascending, (ListViewSortManager) null)
    {
    }

    public ListViewCurrencySort(int sortColumn, bool ascending, ListViewSortManager sortMngr)
      : base(sortColumn, ascending, sortMngr)
    {
    }

    protected override int CompareText(string lhs, string rhs)
    {
      Decimal num1 = this.convertToDecimal(lhs);
      Decimal num2 = this.convertToDecimal(rhs);
      if (num1 > num2)
        return -1;
      return num1 < num2 ? 1 : 0;
    }

    private Decimal convertToDecimal(string text)
    {
      if (text == "")
        return Decimal.MinValue;
      try
      {
        return Decimal.Parse(this.ConvertToNumericString(text), NumberStyles.Number);
      }
      catch
      {
        return Decimal.MaxValue;
      }
    }
  }
}
