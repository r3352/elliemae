// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Client.DisconnectedEventArgs
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System;

#nullable disable
namespace EllieMae.Encompass.Client
{
  /// <summary>
  /// Defines the arguments passed into the Disconnected event on the <see cref="T:EllieMae.Encompass.Client.Session">Session</see>.
  /// </summary>
  public class DisconnectedEventArgs : EventArgs, IDisconnectedEventArgs
  {
    private DisconnectReason reason;

    internal DisconnectedEventArgs(DisconnectReason reason) => this.reason = reason;

    /// <summary>Gets the reason why the session was disconnected.</summary>
    public DisconnectReason Reason => this.reason;
  }
}
