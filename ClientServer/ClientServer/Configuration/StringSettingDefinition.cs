// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Configuration.StringSettingDefinition
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

#nullable disable
namespace EllieMae.EMLite.ClientServer.Configuration
{
  public class StringSettingDefinition : SettingDefinition
  {
    private string defaultValue;
    private int maxLength;

    public StringSettingDefinition(
      string path,
      string displayName,
      string description,
      SettingTargetSystem appliesTo,
      int maxLength,
      string defaultValue,
      bool requiresRestart,
      bool displayEnabled)
      : base(path, displayName, description, appliesTo, requiresRestart, displayEnabled)
    {
      this.defaultValue = defaultValue;
      this.maxLength = maxLength;
    }

    public override object DefaultValue => (object) this.defaultValue;

    public int MaxLength => this.maxLength;

    public override object Parse(string value)
    {
      return value == null ? (object) this.defaultValue : (object) value;
    }
  }
}
