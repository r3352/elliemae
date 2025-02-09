// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessEnums.TradePriceAdjustmentTemplates
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.Encompass.Client;

#nullable disable
namespace EllieMae.Encompass.BusinessEnums
{
  public class TradePriceAdjustmentTemplates : EnumBase, ITradePriceAdjustmentTemplates
  {
    private Session session;
    private IConfigurationManager configMngr;

    internal TradePriceAdjustmentTemplates(Session session)
    {
      this.session = session;
      this.configMngr = (IConfigurationManager) session.GetObject("ConfigurationManager");
      FileSystemEntry[] settingsFileEntries = this.configMngr.GetAllPublicTemplateSettingsFileEntries((TemplateSettingsType) 11, true);
      if (settingsFileEntries == null)
        return;
      int id = 0;
      foreach (FileSystemEntry entry in settingsFileEntries)
      {
        this.AddItem((EnumItem) new TradePriceAdjustmentTemplate(id, entry, session));
        ++id;
      }
    }

    public TradePriceAdjustmentTemplate this[string name]
    {
      get => (TradePriceAdjustmentTemplate) this.GetItemByName(name);
    }

    public TradePriceAdjustmentTemplate this[int index]
    {
      get => (TradePriceAdjustmentTemplate) this.GetItem(index);
    }
  }
}
