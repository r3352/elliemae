// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.SimpleTemplateSelector
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.RemotingServices;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class SimpleTemplateSelector : TemplateSelectorForm
  {
    private EllieMae.EMLite.ClientServer.TemplateSettingsType templateType;

    public SimpleTemplateSelector() => this.templateType = EllieMae.EMLite.ClientServer.TemplateSettingsType.ClosingCost;

    public SimpleTemplateSelector(EllieMae.EMLite.ClientServer.TemplateSettingsType templateType)
    {
      this.templateType = templateType;
    }

    public SimpleTemplateSelector(EllieMae.EMLite.ClientServer.TemplateSettingsType templateType, bool allowAppend)
      : this(templateType)
    {
      this.DisplayAppendCheckbox = allowAppend;
    }

    protected override EllieMae.EMLite.ClientServer.TemplateSettingsType TemplateType
    {
      get => this.templateType;
    }

    public BinaryObject GetSelectedTemplate()
    {
      FileSystemEntry selectedItem = this.SelectedItem;
      return selectedItem == null ? (BinaryObject) null : Session.ConfigurationManager.GetTemplateSettings(this.templateType, selectedItem);
    }
  }
}
