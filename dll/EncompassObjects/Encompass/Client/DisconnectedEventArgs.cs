// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Client.DisconnectedEventArgs
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System;

#nullable disable
namespace EllieMae.Encompass.Client
{
  public class DisconnectedEventArgs : EventArgs, IDisconnectedEventArgs
  {
    private DisconnectReason reason;

    internal DisconnectedEventArgs(DisconnectReason reason) => this.reason = reason;

    public DisconnectReason Reason => this.reason;
  }
}
