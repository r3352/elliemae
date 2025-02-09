// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Configuration.BooleanSettingDefinition
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

#nullable disable
namespace EllieMae.EMLite.ClientServer.Configuration
{
  public class BooleanSettingDefinition : SettingDefinition
  {
    private bool defaultValue;

    public BooleanSettingDefinition(
      string path,
      string displayName,
      string description,
      SettingTargetSystem appliesTo,
      bool defaultValue,
      bool requiresRestart,
      bool displayEnabled)
      : base(path, displayName, description, appliesTo, requiresRestart, displayEnabled)
    {
      this.defaultValue = defaultValue;
    }

    public override object DefaultValue => (object) this.defaultValue;

    public override object Parse(string value)
    {
      if (string.IsNullOrEmpty(value))
        return (object) this.defaultValue;
      try
      {
        return (object) bool.Parse(value);
      }
      catch
      {
        return (object) this.defaultValue;
      }
    }
  }
}
