// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Events.DataExchangeEvent
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Events
{
  [Serializable]
  public class DataExchangeEvent : SessionManagementEvent
  {
    private SessionInfo source;
    private object data;

    public DataExchangeEvent(SessionInfo targetSession, object data, SessionInfo source)
      : base(targetSession)
    {
      this.source = source;
      this.data = data;
    }

    public object Data => this.data;

    public SessionInfo Source => this.source;

    public override string ToString()
    {
      return "[DataExchange to " + this.Context.UserID + "] " + this.data;
    }
  }
}
