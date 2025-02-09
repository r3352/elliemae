// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.PlatformServerHealth
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class PlatformServerHealth
  {
    private string _directoryServiceStatus;
    private string _hazelcastCacheStatus;
    private int _numberOfActiveClientContexts;
    private string _encompassVersion;

    public PlatformServerHealth(
      string directoryServiceStatus,
      string hazelcastCacheStatus,
      int numberOfActiveClientContexts,
      string encompassVersion)
    {
      this._directoryServiceStatus = directoryServiceStatus;
      this._hazelcastCacheStatus = hazelcastCacheStatus;
      this._numberOfActiveClientContexts = numberOfActiveClientContexts;
      this._encompassVersion = encompassVersion;
    }

    public string DirectoryServiceStatus => this._directoryServiceStatus;

    public string HazelcastCacheStatus => this._hazelcastCacheStatus;

    public int NumberOfActiveClientContexts => this._numberOfActiveClientContexts;

    public string EncomapssVersion => this._encompassVersion;
  }
}
