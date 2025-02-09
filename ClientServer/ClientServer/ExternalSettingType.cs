// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ExternalSettingType
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class ExternalSettingType
  {
    public int settingTypeId { get; set; }

    public string settingTypeKey { get; set; }

    public string settingTypeText { get; set; }

    public ExternalSettingType(int settingTypeId, string settingTypeKey, string settingTypeText)
    {
      this.settingTypeId = settingTypeId;
      this.settingTypeKey = settingTypeKey;
      this.settingTypeText = settingTypeText;
    }
  }
}
