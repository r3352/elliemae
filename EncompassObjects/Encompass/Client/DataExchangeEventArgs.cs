// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Client.DataExchangeEventArgs
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
  /// Represents the event arguments for the <see cref="P:EllieMae.Encompass.Client.Session.DataExchange" /> event.
  /// </summary>
  public class DataExchangeEventArgs : EventArgs, IDataExchangeEventArgs
  {
    private DataExchangeEvent e;
    private SessionInformation source;

    internal DataExchangeEventArgs(DataExchangeEvent e)
    {
      this.e = e;
      this.source = new SessionInformation(e.Source);
    }

    /// <summary>Gets the data portion of the exchange.</summary>
    public object Data => this.e.Data;

    /// <summary>
    /// Gets the information identifying the session from which the exchange originated.
    /// </summary>
    public SessionInformation Source => this.source;
  }
}
