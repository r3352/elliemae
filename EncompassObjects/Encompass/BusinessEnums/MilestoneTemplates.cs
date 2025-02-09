// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessEnums.MilestoneTemplates
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.Encompass.Client;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.Encompass.BusinessEnums
{
  /// <summary>
  /// Represents the collection of <see cref="T:EllieMae.Encompass.BusinessEnums.MilestoneTemplate">MilestoneTemplate</see>s as configured for the Encompass system.
  /// This includes active as well as non active <see cref="T:EllieMae.Encompass.BusinessEnums.MilestoneTemplate">MilestoneTemplate</see>s.
  /// </summary>
  public class MilestoneTemplates : EnumBase, IMilestoneTemplates
  {
    private Session session;
    private Dictionary<string, int> sortOrderMapping;

    internal MilestoneTemplates(Session session)
    {
      this.session = session;
      List<EllieMae.EMLite.Workflow.MilestoneTemplate> list = this.session.SessionObjects.BpmManager.GetMilestoneTemplates(false).ToList<EllieMae.EMLite.Workflow.MilestoneTemplate>();
      this.sortOrderMapping = new Dictionary<string, int>(list.Count<EllieMae.EMLite.Workflow.MilestoneTemplate>());
      for (int index = 0; index < list.Count<EllieMae.EMLite.Workflow.MilestoneTemplate>(); ++index)
      {
        EllieMae.EMLite.Workflow.MilestoneTemplate milestoneTemplate = list[index];
        this.AddItem((EnumItem) new MilestoneTemplate(milestoneTemplate, session));
        this.sortOrderMapping.Add(milestoneTemplate.TemplateID, index + 1);
      }
    }

    /// <summary>
    /// Gets a <see cref="T:EllieMae.Encompass.BusinessEnums.MilestoneTemplate">MilestoneTemplate</see> by name.
    /// </summary>
    /// <param name="itemName">The name of the <see cref="T:EllieMae.Encompass.BusinessEnums.MilestoneTemplate">MilestoneTemplate</see></param>
    /// <returns>The selected <see cref="T:EllieMae.Encompass.BusinessEnums.MilestoneTemplate">MilestoneTemplate</see></returns>
    public MilestoneTemplate GetItemByName(string itemName)
    {
      return (MilestoneTemplate) base.GetItemByName(itemName);
    }

    /// <summary>
    /// Gets a <see cref="T:EllieMae.Encompass.BusinessEnums.MilestoneTemplate">MilestoneTemplate</see> by it's ID.
    /// </summary>
    /// <param name="itemId">The ID of the <see cref="T:EllieMae.Encompass.BusinessEnums.MilestoneTemplate">MilestoneTemplate</see></param>
    /// <returns>The selected <see cref="T:EllieMae.Encompass.BusinessEnums.MilestoneTemplate">MilestoneTemplate</see></returns>
    public MilestoneTemplate GetItemByID(int itemId)
    {
      return (MilestoneTemplate) base.GetItemByID(itemId);
    }

    /// <summary>
    /// Gets the <see cref="T:EllieMae.Encompass.BusinessEnums.MilestoneTemplate">MilestoneTemplate</see> by it's index.
    /// </summary>
    /// <param name="index">The index of the <see cref="T:EllieMae.Encompass.BusinessEnums.MilestoneTemplate">MilestoneTemplate</see> in the list.</param>
    /// <returns>The selected <see cref="T:EllieMae.Encompass.BusinessEnums.MilestoneTemplate">MilestoneTemplate</see></returns>
    public MilestoneTemplate this[int index] => (MilestoneTemplate) this.GetItem(index);

    /// <summary>Direct access to the Default MilestoneTemplate</summary>
    public MilestoneTemplate DefaultTemplate => this.GetItemByName("Default Template");

    internal int GetOrderByIndex(string id) => this.sortOrderMapping[id];
  }
}
