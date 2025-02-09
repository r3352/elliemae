// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessEnums.MilestoneTemplates
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.Workflow;
using EllieMae.Encompass.Client;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.Encompass.BusinessEnums
{
  public class MilestoneTemplates : EnumBase, IMilestoneTemplates
  {
    private Session session;
    private Dictionary<string, int> sortOrderMapping;

    internal MilestoneTemplates(Session session)
    {
      this.session = session;
      List<MilestoneTemplate> list = this.session.SessionObjects.BpmManager.GetMilestoneTemplates(false).ToList<MilestoneTemplate>();
      this.sortOrderMapping = new Dictionary<string, int>(list.Count<MilestoneTemplate>());
      for (int index = 0; index < list.Count<MilestoneTemplate>(); ++index)
      {
        MilestoneTemplate milestoneTemplate = list[index];
        this.AddItem((EnumItem) new MilestoneTemplate(milestoneTemplate, session));
        this.sortOrderMapping.Add(milestoneTemplate.TemplateID, index + 1);
      }
    }

    public MilestoneTemplate GetItemByName(string itemName)
    {
      return (MilestoneTemplate) base.GetItemByName(itemName);
    }

    public MilestoneTemplate GetItemByID(int itemId)
    {
      return (MilestoneTemplate) base.GetItemByID(itemId);
    }

    public MilestoneTemplate this[int index] => (MilestoneTemplate) this.GetItem(index);

    public MilestoneTemplate DefaultTemplate => this.GetItemByName("Default Template");

    internal int GetOrderByIndex(string id) => this.sortOrderMapping[id];
  }
}
