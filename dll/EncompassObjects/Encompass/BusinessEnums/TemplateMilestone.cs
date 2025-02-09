// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessEnums.TemplateMilestone
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.Workflow;

#nullable disable
namespace EllieMae.Encompass.BusinessEnums
{
  public class TemplateMilestone : EnumItem, ITemplateMilestone
  {
    private TemplateMilestone next;
    private TemplateMilestone prev;
    private string milestoneID = "";
    private TemplateMilestone templateMilestone;

    internal TemplateMilestone(
      TemplateMilestone templateMilestone,
      int id,
      string name,
      string milestoneID)
      : base(id, name)
    {
      this.templateMilestone = templateMilestone;
      this.milestoneID = milestoneID;
    }

    public string MilestoneID => this.milestoneID;

    public int DaysToComplete => this.templateMilestone.DaysToComplete;

    public TemplateMilestone Previous => this.prev;

    public TemplateMilestone Next => this.next;

    public static bool operator >(TemplateMilestone tm1, TemplateMilestone tm2)
    {
      if ((EnumItem) tm1 == (EnumItem) null || (EnumItem) tm2 == (EnumItem) null)
        return false;
      for (TemplateMilestone previous = tm1.Previous; (EnumItem) previous != (EnumItem) null; previous = previous.Previous)
      {
        if ((EnumItem) previous == (EnumItem) tm2)
          return true;
      }
      return false;
    }

    public static bool operator >=(TemplateMilestone tm1, TemplateMilestone tm2)
    {
      return (EnumItem) tm1 == (EnumItem) tm2 || tm1 > tm2;
    }

    public static bool operator <(TemplateMilestone tm1, TemplateMilestone tm2) => !(tm1 >= tm2);

    public static bool operator <=(TemplateMilestone tm1, TemplateMilestone tm2) => !(tm1 > tm2);

    internal void SetNextTemplateMilestone(TemplateMilestone next) => this.next = next;

    internal void SetPreviousTemplateMilestone(TemplateMilestone prev) => this.prev = prev;
  }
}
