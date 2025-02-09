// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Configuration.IntegerSettingDefinition
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

#nullable disable
namespace EllieMae.EMLite.ClientServer.Configuration
{
  public class IntegerSettingDefinition : SettingDefinition
  {
    private int defaultValue;
    private int minValue;
    private int maxValue;

    public IntegerSettingDefinition(
      string path,
      string displayName,
      string description,
      SettingTargetSystem appliesTo,
      int minValue,
      int maxValue,
      int defaultValue,
      bool requiresRestart,
      bool displayEnabled)
      : base(path, displayName, description, appliesTo, requiresRestart, displayEnabled)
    {
      this.defaultValue = defaultValue;
      this.minValue = minValue;
      this.maxValue = maxValue;
    }

    public override object DefaultValue => (object) this.defaultValue;

    public int MinValue => this.minValue;

    public int MaxValue => this.maxValue;

    public override object Parse(string value)
    {
      if (string.IsNullOrEmpty(value))
        return (object) this.defaultValue;
      try
      {
        return (object) int.Parse(value);
      }
      catch
      {
        return (object) this.defaultValue;
      }
    }
  }
}
