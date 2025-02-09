// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Events.SessionEvent
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Events
{
  [Serializable]
  public class SessionEvent : SessionMonitorEvent
  {
    private SessionEventType eventType;

    public SessionEvent(SessionEventType eventType, SessionInfo info)
      : base(info)
    {
      this.eventType = eventType;
    }

    public SessionEventType EventType => this.eventType;

    public override string ToString()
    {
      if (this.eventType == SessionEventType.Login)
        return "[Session " + this.Session.SessionID + "] User " + this.Session.UserID + " logged in";
      if (this.eventType == SessionEventType.Logout)
        return "[Session " + this.Session.SessionID + "] User " + this.Session.UserID + " logged out";
      if (this.eventType == SessionEventType.Terminated)
        return "[Session " + this.Session.SessionID + "] Session terminated for user " + this.Session.UserID;
      return this.eventType == SessionEventType.Logout ? "[Session " + this.Session.SessionID + "] Session dropped for user " + this.Session.UserID : "[Session " + this.Session.SessionID + "] " + this.eventType.ToString();
    }
  }
}
