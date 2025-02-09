// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Events.LoanEvent
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.DataEngine;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Events
{
  [Serializable]
  public class LoanEvent : SessionMonitorEvent
  {
    private LoanIdentity loanId;
    private LoanEventType eventType;

    public LoanEvent(LoanEventType eventType, LoanIdentity loanId, SessionInfo info)
      : base(info)
    {
      this.eventType = eventType;
      this.loanId = loanId;
    }

    public LoanEventType EventType => this.eventType;

    public LoanIdentity LoanIdentity => this.loanId;

    public override string ToString()
    {
      if (this.eventType == LoanEventType.Completed)
        return "[Session " + this.Session.SessionID + "] User " + this.Session.UserID + " completed loan " + this.loanId.ToString();
      if (this.eventType == LoanEventType.Created)
        return "[Session " + this.Session.SessionID + "] User " + this.Session.UserID + " create loan " + this.loanId.ToString();
      if (this.eventType == LoanEventType.Deleted)
        return "[Session " + this.Session.SessionID + "] User " + this.Session.UserID + " deleted loan " + this.loanId.ToString();
      if (this.eventType == LoanEventType.Exported)
        return "[Session " + this.Session.SessionID + "] User " + this.Session.UserID + " downloaded loan " + this.loanId.ToString();
      if (this.eventType == LoanEventType.Imported)
        return "[Session " + this.Session.SessionID + "] User " + this.Session.UserID + " uploaded loan " + this.loanId.ToString();
      if (this.eventType == LoanEventType.Locked)
        return "[Session " + this.Session.SessionID + "] User " + this.Session.UserID + " locked loan " + this.loanId.ToString();
      if (this.eventType == LoanEventType.Unlocked)
        return "[Session " + this.Session.SessionID + "] User " + this.Session.UserID + " unlocked loan " + this.loanId.ToString();
      if (this.eventType == LoanEventType.Opened)
        return "[Session " + this.Session.SessionID + "] User " + this.Session.UserID + " opened loan " + this.loanId.ToString();
      if (this.eventType == LoanEventType.PermissionsChanged)
        return "[Session " + this.Session.SessionID + "] User " + this.Session.UserID + " modified the persmissions on loan " + this.loanId.ToString();
      if (this.eventType == LoanEventType.Saved)
        return "[Session " + this.Session.SessionID + "] User " + this.Session.UserID + " saved loan " + this.loanId.ToString();
      if (this.eventType != LoanEventType.Moved)
        return "[Session " + this.Session.SessionID + "] " + this.eventType.ToString();
      return "[Session " + this.Session.SessionID + "] User " + this.Session.UserID + " moved loan " + this.loanId.ToString();
    }
  }
}
