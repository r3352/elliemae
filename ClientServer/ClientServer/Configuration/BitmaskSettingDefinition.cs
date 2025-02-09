// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Configuration.BitmaskSettingDefinition
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Configuration
{
  public class BitmaskSettingDefinition : EnumSettingDefinition
  {
    public BitmaskSettingDefinition(
      string path,
      string displayName,
      string description,
      SettingTargetSystem appliesTo,
      Type enumType,
      object defaultValue,
      bool requiresRestart,
      bool displayEnabled)
      : base(path, displayName, description, appliesTo, enumType, (IEnumNameProvider) new BitmaskEnumNameProvider(enumType), defaultValue, requiresRestart, displayEnabled)
    {
      if (enumType.GetCustomAttributes(typeof (FlagsAttribute), true).Length == 0)
        throw new ArgumentException("The specified enumeration type must have the Flags attribute");
    }

    public BitmaskSettingDefinition(
      string path,
      string displayName,
      string description,
      SettingTargetSystem appliesTo,
      Type enumType,
      IEnumNameProvider nameProvider,
      object defaultValue,
      bool requiresRestart,
      bool displayEnabled)
      : base(path, displayName, description, appliesTo, enumType, nameProvider, defaultValue, requiresRestart, displayEnabled)
    {
      if (enumType.GetCustomAttributes(typeof (FlagsAttribute), true).Length == 0)
        throw new ArgumentException("The specified enumeration type must have the Flags attribute");
    }

    public override string ToString(object value)
    {
      string str = value.ToString();
      return str.Length > 200 ? ((int) value).ToString() : str;
    }
  }
}
