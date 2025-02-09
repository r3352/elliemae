// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.SkyDrive.SkyDriveUrl
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.SkyDrive
{
  [Serializable]
  public class SkyDriveUrl
  {
    public string id { get; set; }

    public string url { get; set; }

    public string authorizationHeader { get; set; }

    public SkyDriveUrl(string id, string url, string authorization)
    {
      this.id = id;
      this.url = url;
      this.authorizationHeader = authorization;
    }
  }
}
