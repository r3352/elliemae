// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Configuration.EnumSettingDefinition
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Configuration
{
  public class EnumSettingDefinition : SettingDefinition
  {
    private object defaultValue;
    private Type enumType;
    private IEnumNameProvider nameProvider;

    public EnumSettingDefinition(
      string path,
      string displayName,
      string description,
      SettingTargetSystem appliesTo,
      Type enumType,
      object defaultValue,
      bool requiresRestart,
      bool displayEnabled)
      : base(path, displayName, description, appliesTo, requiresRestart, displayEnabled)
    {
      this.defaultValue = defaultValue;
      this.enumType = enumType;
      this.nameProvider = (IEnumNameProvider) new DefaultEnumNameProvider(enumType);
    }

    public EnumSettingDefinition(
      string path,
      string displayName,
      string description,
      SettingTargetSystem appliesTo,
      Type enumType,
      IEnumNameProvider nameProvider,
      object defaultValue,
      bool requiresRestart,
      bool displayEnabled)
      : base(path, displayName, description, appliesTo, requiresRestart, displayEnabled)
    {
      this.defaultValue = defaultValue;
      this.enumType = enumType;
      this.nameProvider = nameProvider;
    }

    public override object DefaultValue => this.defaultValue;

    public Type EnumType => this.enumType;

    public IEnumNameProvider NameProvider => this.nameProvider;

    public override object Parse(string value)
    {
      if (string.IsNullOrEmpty(value))
        return this.defaultValue;
      try
      {
        return Enum.Parse(this.enumType, value, true);
      }
      catch
      {
        return this.defaultValue;
      }
    }
  }
}
