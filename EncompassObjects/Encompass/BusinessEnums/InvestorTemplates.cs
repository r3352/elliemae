// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessEnums.InvestorTemplates
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.Encompass.Client;

#nullable disable
namespace EllieMae.Encompass.BusinessEnums
{
  /// <summary>
  /// The InvestorTemplates class represents the set of all Investor Templates defined in Encompass settings.
  /// </summary>
  public class InvestorTemplates : EnumBase, IInvestorTemplates
  {
    private Session session;
    private IConfigurationManager configMngr;

    internal InvestorTemplates(Session session)
    {
      this.session = session;
      this.configMngr = (IConfigurationManager) session.GetObject("ConfigurationManager");
      FileSystemEntry[] settingsFileEntries = this.configMngr.GetAllPublicTemplateSettingsFileEntries(TemplateSettingsType.Investor, true);
      if (settingsFileEntries == null)
        return;
      int id = 0;
      foreach (FileSystemEntry entry in settingsFileEntries)
      {
        this.AddItem((EnumItem) new InvestorTemplate(id, entry, session));
        ++id;
      }
    }

    /// <summary>
    /// Gets the <see cref="T:EllieMae.Encompass.BusinessEnums.InvestorTemplate">InvestorTemplate</see> by its name.
    /// </summary>
    /// <param name="name">The name of the <see cref="T:EllieMae.Encompass.BusinessEnums.InvestorTemplate">InvestorTemplate</see> in the list.</param>
    /// <returns>The selected <see cref="T:EllieMae.Encompass.BusinessEnums.InvestorTemplate">InvestorTemplate</see></returns>
    public InvestorTemplate this[string name] => (InvestorTemplate) this.GetItemByName(name);

    /// <summary>
    /// Gets the <see cref="T:EllieMae.Encompass.BusinessEnums.InvestorTemplate">InvestorTemplate</see> by its index.
    /// </summary>
    /// <param name="index">The index of the <see cref="T:EllieMae.Encompass.BusinessEnums.InvestorTemplate">InvestorTemplate</see> in the list.</param>
    /// <returns>The selected <see cref="T:EllieMae.Encompass.BusinessEnums.InvestorTemplate">InvestorTemplate</see></returns>
    public InvestorTemplate this[int index] => (InvestorTemplate) this.GetItem(index);
  }
}
