// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Client.ExceptionMonitorEventArgs
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer.Events;
using EllieMae.EMLite.ClientServer.Exceptions;
using System;

#nullable disable
namespace EllieMae.Encompass.Client
{
  public class ExceptionMonitorEventArgs : EventArgs, IExceptionMonitorEventArgs
  {
    private ExceptionEvent evnt;
    private Exception ex;

    internal ExceptionMonitorEventArgs(ExceptionEvent evnt)
    {
      this.evnt = evnt;
      this.ex = evnt.Exception;
      if (!(this.ex is LoginException))
        return;
      this.ex = (Exception) new LoginException(this.ex as LoginException);
    }

    public Exception Exception => this.ex;
  }
}
