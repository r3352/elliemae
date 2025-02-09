// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessEnums.TemplateMilestones
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.Workflow;
using EllieMae.Encompass.Client;
using System;
using System.Linq;

#nullable disable
namespace EllieMae.Encompass.BusinessEnums
{
  public class TemplateMilestones : EnumBase, ITemplateMilestones
  {
    private Session session;
    private MilestoneTemplate templateMilestone;

    internal TemplateMilestones(MilestoneTemplate templateMilestone, Session session)
    {
      this.session = session;
      this.templateMilestone = templateMilestone;
      for (int index = 0; index < this.templateMilestone.SequentialMilestones.Count(); ++index)
      {
        TemplateMilestone tm = this.templateMilestone.SequentialMilestones[index];
        Milestone milestone = this.session.SessionObjects.StartupInfo.Milestones.FirstOrDefault<Milestone>((Func<Milestone, bool>) (m => m.MilestoneID == tm.MilestoneID));
        int id = this.session.SessionObjects.StartupInfo.Milestones.IndexOf(milestone) + 1;
        this.AddItem((EnumItem) new TemplateMilestone(tm, id, milestone.Name, milestone.MilestoneID));
      }
      TemplateMilestone prev = (TemplateMilestone) null;
      for (int index = 0; index < this.Count; ++index)
      {
        TemplateMilestone next = this[index];
        if ((EnumItem) prev != (EnumItem) null)
        {
          next.SetPreviousTemplateMilestone(prev);
          prev.SetNextTemplateMilestone(next);
        }
        prev = next;
      }
    }

    public TemplateMilestone this[int index] => (TemplateMilestone) this.GetItem(index);

    public TemplateMilestone GetItemByID(int itemId)
    {
      return (TemplateMilestone) base.GetItemByID(itemId);
    }

    public TemplateMilestone GetItemByName(string itemName)
    {
      return (TemplateMilestone) base.GetItemByName(itemName);
    }

    public TemplateMilestone First => this[0];
  }
}
