// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.eFolder.ImageIdentity
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.eFolder
{
  [Serializable]
  public class ImageIdentity
  {
    private string zipKey;
    private string imageKey;

    public ImageIdentity(string zipKey, string imageKey)
    {
      this.zipKey = zipKey;
      this.imageKey = imageKey;
    }

    public string ZipKey => this.zipKey;

    public string ImageKey => this.imageKey;
  }
}
