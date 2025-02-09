// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessEnums.InvestorTemplates
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.Encompass.Client;

#nullable disable
namespace EllieMae.Encompass.BusinessEnums
{
  public class InvestorTemplates : EnumBase, IInvestorTemplates
  {
    private Session session;
    private IConfigurationManager configMngr;

    internal InvestorTemplates(Session session)
    {
      this.session = session;
      this.configMngr = (IConfigurationManager) session.GetObject("ConfigurationManager");
      FileSystemEntry[] settingsFileEntries = this.configMngr.GetAllPublicTemplateSettingsFileEntries((TemplateSettingsType) 14, true);
      if (settingsFileEntries == null)
        return;
      int id = 0;
      foreach (FileSystemEntry entry in settingsFileEntries)
      {
        this.AddItem((EnumItem) new InvestorTemplate(id, entry, session));
        ++id;
      }
    }

    public InvestorTemplate this[string name] => (InvestorTemplate) this.GetItemByName(name);

    public InvestorTemplate this[int index] => (InvestorTemplate) this.GetItem(index);
  }
}
