// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.eFolder.SkyDriveClassic.Pages
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer.eFolder.SkyDriveClassic
{
  [Serializable]
  public class Pages
  {
    public int Id { get; set; }

    public bool Page { get; set; }

    public bool Thumbnail { get; set; }

    public bool Overlay { get; set; }

    public int Rotation { get; set; }

    public List<int> Size { get; set; }

    public bool TextAvailable { get; set; }

    public long ContentLength { get; set; }
  }
}
