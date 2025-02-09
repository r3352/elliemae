// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Client.IServerEventsInterface
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Client
{
  [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
  public interface IServerEventsInterface
  {
    [DispId(1)]
    void ConnectionMonitor(object sender, ConnectionMonitorEventArgs e);

    [DispId(2)]
    void ExceptionMonitor(object sender, ExceptionMonitorEventArgs e);

    [DispId(3)]
    void LicenseMonitor(object sender, LicenseMonitorEventArgs e);

    [DispId(4)]
    void LoanMonitor(object sender, LoanMonitorEventArgs e);

    [DispId(5)]
    void SessionMonitor(object sender, SessionMonitorEventArgs e);
  }
}
