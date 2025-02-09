// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.eFolder.SkyDriveClassic.PdfEmbeddedBoomark
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer.eFolder.SkyDriveClassic
{
  public class PdfEmbeddedBoomark
  {
    public string Title { get; set; }

    public string Color { get; set; }

    public string Page { get; set; }

    public string Action { get; set; }

    public List<PdfEmbeddedBoomark> Kids { get; set; }
  }
}
