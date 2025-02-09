// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Configuration.DecimalSettingDefinition
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Configuration
{
  public class DecimalSettingDefinition : SettingDefinition
  {
    private Decimal defaultValue;
    private int roundingDecimalPlaces;

    public DecimalSettingDefinition(
      string path,
      string displayName,
      string description,
      SettingTargetSystem appliesTo,
      int roundingDecimalPlaces,
      Decimal defaultValue,
      bool requiresRestart,
      bool displayEnabled)
      : base(path, displayName, description, appliesTo, requiresRestart, displayEnabled)
    {
      this.defaultValue = defaultValue;
      this.roundingDecimalPlaces = roundingDecimalPlaces;
    }

    public override object DefaultValue => (object) this.defaultValue;

    public int RoundingDecimalPlaces => this.roundingDecimalPlaces;

    public override object Parse(string value)
    {
      if (string.IsNullOrEmpty(value))
        return (object) this.defaultValue;
      try
      {
        return (object) Utils.ParseDecimal((object) value, this.defaultValue, 3);
      }
      catch
      {
        return (object) this.defaultValue;
      }
    }
  }
}
