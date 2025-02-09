// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.CS.ListViewDateSort
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using System;

#nullable disable
namespace EllieMae.EMLite.RemotingServices.CS
{
  internal class ListViewDateSort : ListViewTextSort
  {
    public ListViewDateSort(int sortColumn, bool ascending)
      : base(sortColumn, ascending, (ListViewSortManager) null)
    {
    }

    public ListViewDateSort(int sortColumn, bool ascending, ListViewSortManager sortMngr)
      : base(sortColumn, ascending, sortMngr)
    {
    }

    protected override int OnCompare(string lhs, string rhs)
    {
      DateTime dateTime1;
      try
      {
        dateTime1 = DateTime.Parse(lhs);
      }
      catch
      {
        return 1;
      }
      DateTime dateTime2;
      try
      {
        dateTime2 = DateTime.Parse(rhs);
      }
      catch
      {
        return -1;
      }
      return dateTime1.CompareTo(dateTime2);
    }
  }
}
