// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Client.DisconnectReason
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Client
{
  /// <summary>
  /// Defines the possible disconnection causes for a <see cref="T:EllieMae.Encompass.Client.Session">Session</see>.
  /// </summary>
  [Guid("E5BD291F-55D2-3C35-8E3E-867A93F88C9A")]
  public enum DisconnectReason
  {
    /// <summary>The session was terminated by calling the End() method.</summary>
    SessionDisposed,
    /// <summary>The session was terminated due to a connection failure with the server.</summary>
    ConnectionError,
    /// <summary>The session was terminated by the server.</summary>
    TerminatedByServer,
  }
}
