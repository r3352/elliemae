// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Configuration.SettingTargetSystem
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Configuration
{
  [Flags]
  public enum SettingTargetSystem
  {
    None = 0,
    BrokerOnly = 4,
    BankerOnly = 8,
    OfflineOnly = 16, // 0x00000010
    ServerOnly = 32, // 0x00000020
    HostedOnly = 64, // 0x00000040
    SelfHostedOnly = 128, // 0x00000080
    OfflineBanker = SelfHostedOnly | HostedOnly | OfflineOnly | BankerOnly, // 0x000000D8
    Offline = OfflineBanker | BrokerOnly, // 0x000000DC
    ServerBanker = SelfHostedOnly | HostedOnly | ServerOnly | BankerOnly, // 0x000000E8
    Server = ServerBanker | BrokerOnly, // 0x000000EC
    HostedServer = HostedOnly | ServerOnly | BankerOnly | BrokerOnly, // 0x0000006C
    SelfHostedServer = SelfHostedOnly | ServerOnly | BankerOnly | BrokerOnly, // 0x000000AC
    Broker = SelfHostedOnly | HostedOnly | ServerOnly | OfflineOnly | BrokerOnly, // 0x000000F4
    Banker = ServerBanker | OfflineOnly, // 0x000000F8
    Hosted = HostedServer | OfflineOnly, // 0x0000007C
    SelfHosted = SelfHostedServer | OfflineOnly, // 0x000000BC
    All = SelfHosted | HostedOnly, // 0x000000FC
  }
}
