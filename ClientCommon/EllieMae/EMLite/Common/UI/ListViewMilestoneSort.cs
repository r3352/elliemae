// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.ListViewMilestoneSort
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using System.Collections;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class ListViewMilestoneSort : ListViewTextSort
  {
    private ArrayList milestoneArray = new ArrayList();

    public ListViewMilestoneSort(int sortColumn, bool ascending, string[] milestoneList)
      : base(sortColumn, ascending, (ListViewSortManager) null)
    {
      if (milestoneList == null)
        return;
      for (int index = 0; index < milestoneList.Length; ++index)
        this.milestoneArray.Add((object) milestoneList[index].ToString().ToUpper());
    }

    protected override int CompareText(string lhs, string rhs)
    {
      if (this.milestoneArray.Count == 0)
        return string.Compare(lhs, rhs, true);
      int num1 = -1;
      int num2 = -1;
      if (this.milestoneArray.Contains((object) lhs.ToUpper()))
        num1 = this.milestoneArray.IndexOf((object) lhs.ToUpper());
      if (this.milestoneArray.Contains((object) rhs.ToUpper()))
        num2 = this.milestoneArray.IndexOf((object) rhs.ToUpper());
      return num1 < 0 || num2 < 0 ? string.Compare(lhs, rhs, true) : num1 - num2;
    }
  }
}
