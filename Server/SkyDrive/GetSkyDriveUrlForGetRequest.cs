// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.SkyDrive.GetSkyDriveUrlForGetRequest
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

#nullable disable
namespace EllieMae.EMLite.Server.SkyDrive
{
  public class GetSkyDriveUrlForGetRequest
  {
    public int expires { get; set; }

    public string ipAddress { get; set; }

    public string sub { get; set; }

    public string[] objectIds { get; set; }

    public SkyDriveFile[] files { get; set; }
  }
}
