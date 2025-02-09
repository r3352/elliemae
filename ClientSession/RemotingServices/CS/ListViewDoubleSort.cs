// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.CS.ListViewDoubleSort
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

#nullable disable
namespace EllieMae.EMLite.RemotingServices.CS
{
  internal class ListViewDoubleSort : ListViewTextSort
  {
    public ListViewDoubleSort(int sortColumn, bool ascending)
      : base(sortColumn, ascending, (ListViewSortManager) null)
    {
    }

    public ListViewDoubleSort(int sortColumn, bool ascending, ListViewSortManager sortMngr)
      : base(sortColumn, ascending, sortMngr)
    {
    }

    protected override int OnCompare(string lhs, string rhs)
    {
      double num = double.Parse(lhs) - double.Parse(rhs);
      if (num > 0.0)
        return 1;
      return num < 0.0 ? -1 : 0;
    }
  }
}
