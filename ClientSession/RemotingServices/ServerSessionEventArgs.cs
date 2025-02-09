// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.ServerSessionEventArgs
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using EllieMae.EMLite.ClientServer.Events;
using System;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  public class ServerSessionEventArgs : EventArgs
  {
    private SessionEvent sessionEvent;

    public ServerSessionEventArgs(SessionEvent sessionEvent) => this.sessionEvent = sessionEvent;

    public SessionEvent SessionEvent => this.sessionEvent;
  }
}
