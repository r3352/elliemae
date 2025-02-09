// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Client.SessionMonitorEventArgs
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer.Events;
using System;

#nullable disable
namespace EllieMae.Encompass.Client
{
  /// <summary>
  /// Event argument class for the <see cref="E:EllieMae.Encompass.Client.ServerEvents.SessionMonitor" /> event.
  /// </summary>
  public class SessionMonitorEventArgs : EventArgs, ISessionMonitorEventArgs
  {
    private SessionEvent evnt;
    private SessionInformation sessionInfo;

    internal SessionMonitorEventArgs(SessionEvent evnt)
    {
      this.evnt = evnt;
      this.sessionInfo = new SessionInformation(this.evnt.Session);
    }

    /// <summary>Gets the type of connection event that has occurred.</summary>
    public SessionMonitorEventType EventType => (SessionMonitorEventType) this.evnt.EventType;

    /// <summary>Gets the IP address of the client machine.</summary>
    public SessionInformation SessionInformation => this.sessionInfo;
  }
}
