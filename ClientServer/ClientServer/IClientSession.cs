// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.IClientSession
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.Events;
using System;
using System.Collections.Concurrent;
using System.Net;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public interface IClientSession
  {
    string SessionID { get; }

    string UserID { get; }

    string Hostname { get; }

    IPAddress HostAddress { get; }

    DateTime LoginTime { get; }

    LoginParameters LoginParams { get; }

    DateTime ServerTime { get; }

    string ServerTimeZone { get; }

    string DisplayVersion { get; }

    void Abandon();

    void Terminate(DisconnectEventArgument disconnectEventArg, string reason);

    IAsyncResult RaiseEvent(ServerEvent e);

    SessionInfo SessionInfo { get; }

    ServerIdentity Server { get; }

    IAsyncResult SendMessage(Message message);

    bool Ping(TimeSpan timeout);

    ConcurrentDictionary<string, string> ConcurrentUpdatesNotificationCache { get; }
  }
}
