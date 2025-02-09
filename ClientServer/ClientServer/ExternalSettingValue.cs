// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ExternalSettingValue
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class ExternalSettingValue
  {
    public int settingId { get; set; }

    public int settingTypeId { get; set; }

    public string settingValue { get; set; }

    public int sortId { get; set; }

    public string settingCode { get; set; }

    public ExternalSettingValue(
      int settingId,
      int settingTypeId,
      string settingCode,
      string settingValue,
      int sortId)
    {
      this.settingId = settingId;
      this.settingTypeId = settingTypeId;
      this.settingValue = settingValue;
      this.sortId = sortId;
      this.settingCode = settingCode;
    }

    public string CodeValueConcat
    {
      get
      {
        return this.settingCode == null || this.settingCode == string.Empty ? "" : string.Format("{0} - {1}", (object) this.settingCode, (object) this.settingValue);
      }
    }
  }
}
