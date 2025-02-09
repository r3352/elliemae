// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Events.ConnectionEvent
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Net;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Events
{
  [Serializable]
  public class ConnectionEvent : ServerMonitorEvent
  {
    private ConnectionEventType eventType;
    private IPAddress clientIp;
    private string clientUri;

    public ConnectionEvent(ConnectionEventType eventType, string clientUri)
      : this(eventType, clientUri, (IPAddress) null)
    {
    }

    public ConnectionEvent(ConnectionEventType eventType, string clientUri, IPAddress clientIp)
    {
      this.eventType = eventType;
      this.clientUri = clientUri;
      this.clientIp = clientIp;
    }

    public ConnectionEventType EventType => this.eventType;

    public string ClientUri => this.clientUri;

    public IPAddress ClientIPAddress => this.clientIp;

    public override string ToString()
    {
      string str = this.clientUri + (this.clientIp != null ? " (" + this.clientIp.ToString() + ")" : "");
      if (this.eventType == ConnectionEventType.Accepted)
        return "[Connection] Connection accepted from client " + str;
      if (this.eventType == ConnectionEventType.Rejected)
        return "[Connection] Connection rejected from client " + str;
      return this.eventType == ConnectionEventType.Closed ? "[Connection] Connection closed from client " + str : "[Connection] Unknown connection event from client " + str;
    }
  }
}
