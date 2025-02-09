// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.CS.ListViewInt64Sort
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using System.Globalization;

#nullable disable
namespace EllieMae.EMLite.RemotingServices.CS
{
  internal class ListViewInt64Sort : ListViewTextSort
  {
    public ListViewInt64Sort(int sortColumn, bool ascending)
      : base(sortColumn, ascending, (ListViewSortManager) null)
    {
    }

    public ListViewInt64Sort(int sortColumn, bool ascending, ListViewSortManager sortMngr)
      : base(sortColumn, ascending, sortMngr)
    {
    }

    protected override int OnCompare(string lhs, string rhs)
    {
      return (int) (long.Parse(lhs, NumberStyles.Number) - long.Parse(rhs, NumberStyles.Number));
    }
  }
}
