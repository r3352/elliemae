// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.EVerifyLoginInfoItem
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class EVerifyLoginInfoItem : IXmlSerializable
  {
    public bool bSave;
    public string vendorName = string.Empty;
    public string accountName = string.Empty;
    public string pwd = string.Empty;

    public EVerifyLoginInfoItem()
    {
    }

    public EVerifyLoginInfoItem(bool bSave, string vendorName, string accountName, string pwd)
    {
      this.bSave = bSave;
      this.vendorName = vendorName;
      this.accountName = accountName;
      this.pwd = pwd;
    }

    public EVerifyLoginInfoItem(XmlSerializationInfo info)
    {
      this.bSave = info.GetBoolean(nameof (bSave));
      this.vendorName = info.GetString(nameof (vendorName));
      this.accountName = info.GetString(nameof (accountName));
      this.pwd = info.GetString(nameof (pwd));
    }

    public void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("bSave", (object) this.bSave);
      info.AddValue("vendorName", (object) this.vendorName);
      info.AddValue("accountName", (object) this.accountName);
      info.AddValue("pwd", (object) this.pwd);
    }
  }
}
