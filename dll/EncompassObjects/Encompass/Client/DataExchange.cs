// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Client.DataExchange
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Events;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.BusinessObjects;
using System;

#nullable disable
namespace EllieMae.Encompass.Client
{
  public class DataExchange : SessionBoundObject, IDataExchange
  {
    private ScopedEventHandler<DataExchangeEventArgs> dataReceived;

    public event DataExchangeEventHandler DataReceived
    {
      add
      {
        if (value == null)
          return;
        this.dataReceived.Add(new ScopedEventHandler<DataExchangeEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
      remove
      {
        if (value == null)
          return;
        this.dataReceived.Remove(new ScopedEventHandler<DataExchangeEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
    }

    internal DataExchange(Session session)
      : base(session)
    {
      this.dataReceived = new ScopedEventHandler<DataExchangeEventArgs>(nameof (DataExchange), "DataReceived");
    }

    public int PostDataToUser(string userId, object data)
    {
      return ((IServerManager) this.Session.GetObject("ServerManager")).PostDataToUser(userId, data);
    }

    public int PostDataToAll(object data)
    {
      return ((IServerManager) this.Session.GetObject("ServerManager")).PostDataToAll(data);
    }

    public int PostDataToSession(string serverSessionId, object data)
    {
      return ((IServerManager) this.Session.GetObject("ServerManager")).PostDataToSession(serverSessionId, data);
    }

    public DataObject GetCustomDataObject(string fileName)
    {
      BinaryObject data = !((fileName ?? "") == "") ? ((IConfigurationManager) this.Session.GetObject("ConfigurationManager")).GetCustomDataObject(fileName) : throw new ArgumentException(nameof (fileName), "File name cannot be blank or null");
      return data == null ? (DataObject) null : new DataObject(data);
    }

    public void SaveCustomDataObject(string fileName, DataObject data)
    {
      if ((fileName ?? "") == "")
        throw new ArgumentException(nameof (fileName), "File name cannot be blank or null");
      if (data == null)
        throw new ArgumentNullException(nameof (data));
      ((IConfigurationManager) this.Session.GetObject("ConfigurationManager")).SaveCustomDataObject(fileName, data.Unwrap());
    }

    public void AppendToCustomDataObject(string fileName, DataObject data)
    {
      if ((fileName ?? "") == "")
        throw new ArgumentException(nameof (fileName), "File name cannot be blank or null");
      if (data == null)
        throw new ArgumentNullException(nameof (data));
      ((IConfigurationManager) this.Session.GetObject("ConfigurationManager")).AppendToCustomDataObject(fileName, data.Unwrap());
    }

    public void DeleteCustomDataObject(string fileName)
    {
      if ((fileName ?? "") == "")
        throw new ArgumentException(nameof (fileName), "File name cannot be blank or null");
      ((IConfigurationManager) this.Session.GetObject("ConfigurationManager")).SaveCustomDataObject(fileName, (BinaryObject) null);
    }

    internal void OnDataExchangeEvent(DataExchangeEvent e)
    {
      this.dataReceived((object) this, new DataExchangeEventArgs(e));
    }
  }
}
