// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.ListViewEnumSort`1
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class ListViewEnumSort<NameProvider> : ListViewTextSort
  {
    private IEnumNameProvider nameProvider;

    public ListViewEnumSort(int sortColumn, bool ascending)
      : base(sortColumn, ascending, (ListViewSortManager) null)
    {
      this.nameProvider = (IEnumNameProvider) typeof (NameProvider).GetConstructor(Type.EmptyTypes).Invoke(new object[0]);
    }

    public ListViewEnumSort(int sortColumn, bool ascending, ListViewSortManager sortMngr)
      : base(sortColumn, ascending, sortMngr)
    {
      this.nameProvider = (IEnumNameProvider) typeof (NameProvider).GetConstructor(Type.EmptyTypes).Invoke(new object[0]);
    }

    protected override int CompareText(string lhs, string rhs)
    {
      return Convert.ToInt32(this.nameProvider.GetValue(lhs)) - Convert.ToInt32(this.nameProvider.GetValue(rhs));
    }
  }
}
