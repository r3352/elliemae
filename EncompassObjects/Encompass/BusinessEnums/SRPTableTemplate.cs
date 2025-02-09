// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessEnums.SRPTableTemplate
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
  /// The SRPTableTemplate class represents an SRP Table template as configured in Encompass' Secondary Setup Settings
  /// </summary>
  public class SRPTableTemplate : EnumItem, ISRPTableTemplate
  {
    private EllieMae.EMLite.Trading.SRPTableTemplate sRPTableTemplate;
    private FileSystemEntry entry;
    private IConfigurationManager configMngr;

    internal SRPTableTemplate(int id, FileSystemEntry entry, Session session)
      : base(id, entry.Name)
    {
      this.entry = entry;
      this.configMngr = (IConfigurationManager) session.GetObject("ConfigurationManager");
    }

    private void LoadTemplate()
    {
      if (this.sRPTableTemplate != null)
        return;
      this.sRPTableTemplate = (EllieMae.EMLite.Trading.SRPTableTemplate) this.configMngr.GetTemplateSettings(TemplateSettingsType.SRPTable, this.entry);
    }

    /// <summary>Gets Name of the Template.</summary>
    public new string Name
    {
      get
      {
        this.LoadTemplate();
        return this.sRPTableTemplate.TemplateName;
      }
    }

    /// <summary>Gets the Description of Template.</summary>
    public string Description
    {
      get
      {
        this.LoadTemplate();
        return this.sRPTableTemplate.Description;
      }
    }

    /// <summary>Gets the GUID of Template</summary>
    public string GUID
    {
      get
      {
        this.LoadTemplate();
        return this.sRPTableTemplate.Guid;
      }
    }

    internal EllieMae.EMLite.Trading.SRPTableTemplate Unwrap()
    {
      this.LoadTemplate();
      return this.sRPTableTemplate;
    }
  }
}
