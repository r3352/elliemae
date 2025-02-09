// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Client.LicenseMonitorEventArgs
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
  /// Event argument class for the <see cref="E:EllieMae.Encompass.Client.ServerEvents.LicenseMonitor" /> event.
  /// </summary>
  public class LicenseMonitorEventArgs : EventArgs, ILicenseMonitorEventArgs
  {
    private LicenseEvent evnt;

    internal LicenseMonitorEventArgs(LicenseEvent evnt) => this.evnt = evnt;

    /// <summary>Gets the type of connection event that has occurred.</summary>
    public LicenseMonitorEventType EventType => (LicenseMonitorEventType) this.evnt.EventType;

    /// <summary>Gets the IP address of the client machine.</summary>
    public string UserID => this.evnt.UserID;
  }
}
