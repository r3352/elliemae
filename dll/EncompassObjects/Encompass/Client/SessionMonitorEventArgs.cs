// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Client.SessionMonitorEventArgs
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer.Events;
using System;

#nullable disable
namespace EllieMae.Encompass.Client
{
  public class SessionMonitorEventArgs : EventArgs, ISessionMonitorEventArgs
  {
    private SessionEvent evnt;
    private SessionInformation sessionInfo;

    internal SessionMonitorEventArgs(SessionEvent evnt)
    {
      this.evnt = evnt;
      this.sessionInfo = new SessionInformation(((SessionMonitorEvent) this.evnt).Session);
    }

    public SessionMonitorEventType EventType => (SessionMonitorEventType) this.evnt.EventType;

    public SessionInformation SessionInformation => this.sessionInfo;
  }
}
