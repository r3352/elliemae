// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.CS.ListViewCurrencySort
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using System.Globalization;

#nullable disable
namespace EllieMae.EMLite.RemotingServices.CS
{
  internal class ListViewCurrencySort : ListViewTextSort
  {
    public ListViewCurrencySort(int sortColumn, bool ascending)
      : base(sortColumn, ascending, (ListViewSortManager) null)
    {
    }

    public ListViewCurrencySort(int sortColumn, bool ascending, ListViewSortManager sortMngr)
      : base(sortColumn, ascending, sortMngr)
    {
    }

    protected override int OnCompare(string lhs, string rhs)
    {
      double num = double.Parse(lhs, NumberStyles.Currency) - double.Parse(rhs, NumberStyles.Currency);
      if (num > 0.0)
        return 1;
      return num < 0.0 ? -1 : 0;
    }
  }
}
