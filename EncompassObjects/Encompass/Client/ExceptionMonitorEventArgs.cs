// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Client.ExceptionMonitorEventArgs
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
  /// Event argument class for the <see cref="E:EllieMae.Encompass.Client.ServerEvents.ExceptionMonitor" /> event.
  /// </summary>
  public class ExceptionMonitorEventArgs : EventArgs, IExceptionMonitorEventArgs
  {
    private ExceptionEvent evnt;
    private Exception ex;

    internal ExceptionMonitorEventArgs(ExceptionEvent evnt)
    {
      this.evnt = evnt;
      this.ex = evnt.Exception;
      if (!(this.ex is EllieMae.EMLite.ClientServer.Exceptions.LoginException))
        return;
      this.ex = (Exception) new LoginException(this.ex as EllieMae.EMLite.ClientServer.Exceptions.LoginException);
    }

    /// <summary>Gets the exception that triggered this event.</summary>
    public Exception Exception => this.ex;
  }
}
