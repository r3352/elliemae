// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.SecurityContextOptions
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class SecurityContextOptions
  {
    private string contextName;
    private bool compression;

    public SecurityContextOptions(string contextName, bool compression)
    {
      this.contextName = contextName;
      this.compression = compression;
    }

    public string ContextName => this.contextName;

    public bool CompressionEnabled => this.compression;
  }
}
