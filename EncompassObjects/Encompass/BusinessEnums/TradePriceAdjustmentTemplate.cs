// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessEnums.TradePriceAdjustmentTemplate
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Trading;
using EllieMae.Encompass.Client;

#nullable disable
namespace EllieMae.Encompass.BusinessEnums
{
  /// <summary>
  /// The TradePriceAdjustmentTemplate class represents a Trade Price adjustment template as configured in Encompass' Secondary Setup Settings
  /// </summary>
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
      this.tradePriceTemplate = (PriceAdjustmentTemplate) this.configMngr.GetTemplateSettings(TemplateSettingsType.TradePriceAdjustment, this.entry);
    }

    /// <summary>Gets Name of the Template.</summary>
    public new string Name
    {
      get
      {
        this.LoadTemplate();
        return this.tradePriceTemplate.TemplateName;
      }
    }

    /// <summary>Gets the Description of Template.</summary>
    public string Description
    {
      get
      {
        this.LoadTemplate();
        return this.tradePriceTemplate.Description;
      }
    }

    /// <summary>Gets the GUID of Template</summary>
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
