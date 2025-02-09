// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.EVerifyLoginInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class EVerifyLoginInfo : IXmlSerializable
  {
    public EVerifyLoginInfoItem[] Items = new EVerifyLoginInfoItem[0];

    public EVerifyLoginInfo()
    {
    }

    public EVerifyLoginInfo(XmlSerializationInfo info)
    {
      this.Items = (EVerifyLoginInfoItem[]) info.GetValue(nameof (Items), typeof (EVerifyLoginInfoItem[]));
    }

    public void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("Items", (object) this.Items);
    }
  }
}
