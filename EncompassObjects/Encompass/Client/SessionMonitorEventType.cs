// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Client.SessionMonitorEventType
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Client
{
  /// <summary>
  /// Enumeration of session event types supported by the <see cref="T:EllieMae.Encompass.Client.SessionMonitorEventArgs" />.
  /// </summary>
  [Guid("54FCB942-A6E5-4123-823D-04A5B5935545")]
  public enum SessionMonitorEventType
  {
    /// <summary>Session has been started</summary>
    Login = 1,
    /// <summary>Session has been closed by user.</summary>
    Logout = 2,
    /// <summary>Session has been closed due to disconnect or forceful termination.</summary>
    Terminated = 4,
  }
}
