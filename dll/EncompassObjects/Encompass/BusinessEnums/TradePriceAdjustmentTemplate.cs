// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessEnums.TradePriceAdjustmentTemplate
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Trading;
using EllieMae.Encompass.Client;

#nullable disable
namespace EllieMae.Encompass.BusinessEnums
{
  public class TradePriceAdjustmentTemplate : EnumItem, ITradePriceAdjustmentTemplate
  {
    private PriceAdjustmentTemplate tradePriceTemplate;
    private FileSystemEntry entry;
    private IConfigurationManager configMngr;

    internal TradePriceAdjustmentTemplate(int id, FileSystemEntry entry, Session session)
      : base(id, entry.Name)
    {
      this.entry = entry;
      this.configMngr = (IConfigurationManager) session.GetObject("ConfigurationManager");
    }

    private void LoadTemplate()
    {
      if (this.tradePriceTemplate != null)
        return;
      this.tradePriceTemplate = PriceAdjustmentTemplate.op_Explicit(this.configMngr.GetTemplateSettings((TemplateSettingsType) 11, this.entry));
    }

    public new string Name
    {
      get
      {
        this.LoadTemplate();
        return this.tradePriceTemplate.TemplateName;
      }
    }

    public string Description
    {
      get
      {
        this.LoadTemplate();
        return this.tradePriceTemplate.Description;
      }
    }

    public string GUID
    {
      get
      {
        this.LoadTemplate();
        return this.tradePriceTemplate.Guid;
      }
    }

    internal PriceAdjustmentTemplate Unwrap()
    {
      this.LoadTemplate();
      return this.tradePriceTemplate;
    }
  }
}
