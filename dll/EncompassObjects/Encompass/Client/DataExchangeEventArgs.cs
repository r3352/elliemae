// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Client.DataExchangeEventArgs
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer.Events;
using System;

#nullable disable
namespace EllieMae.Encompass.Client
{
  public class DataExchangeEventArgs : EventArgs, IDataExchangeEventArgs
  {
    private DataExchangeEvent e;
    private SessionInformation source;

    internal DataExchangeEventArgs(DataExchangeEvent e)
    {
      this.e = e;
      this.source = new SessionInformation(e.Source);
    }

    public object Data => this.e.Data;

    public SessionInformation Source => this.source;
  }
}
