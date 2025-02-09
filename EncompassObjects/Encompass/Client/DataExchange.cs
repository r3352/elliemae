// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Client.DataExchange
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Events;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.BusinessObjects;
using System;

#nullable disable
namespace EllieMae.Encompass.Client
{
  /// <summary>
  /// Provides methods for exchanging data with other Encompass clients.
  /// </summary>
  public class DataExchange : SessionBoundObject, IDataExchange
  {
    private ScopedEventHandler<DataExchangeEventArgs> dataReceived;

    /// <summary>
    /// An event used for data exchange between Encompass clients.
    /// </summary>
    /// <remarks>The DataReceived event can be used to take advantage of Encompass's internal
    /// messaging system to send custom data between Encompass sessions. A message is sent
    /// using one of the methods <see cref="M:EllieMae.Encompass.Client.DataExchange.PostDataToUser(System.String,System.Object)" />, <see cref="M:EllieMae.Encompass.Client.DataExchange.PostDataToSession(System.String,System.Object)" />
    /// or <see cref="M:EllieMae.Encompass.Client.DataExchange.PostDataToAll(System.Object)" />. If you have subscribed to this event, you can then
    /// receive this message and interpret the contents in whatever way makes sense to you.
    /// The Encompass application does not use this event so it is made available solely
    /// for use by custom application developers who need to transmit data between clients.
    /// </remarks>
    public event DataExchangeEventHandler DataReceived
    {
      add
      {
        if (value == null)
          return;
        this.dataReceived.Add(new ScopedEventHandler<DataExchangeEventArgs>.EventHandlerT(value.Invoke));
      }
      remove
      {
        if (value == null)
          return;
        this.dataReceived.Remove(new ScopedEventHandler<DataExchangeEventArgs>.EventHandlerT(value.Invoke));
      }
    }

    internal DataExchange(Session session)
      : base(session)
    {
      this.dataReceived = new ScopedEventHandler<DataExchangeEventArgs>(nameof (DataExchange), "DataReceived");
    }

    /// <summary>Performs a data exchange with another user.</summary>
    /// <param name="userId">The ID of the user to receive the data.</param>
    /// <param name="data">The data to be posted.</param>
    /// <returns>Returns the number of sessions to which the data was sent.</returns>
    /// <remarks>The data being posted must be a simple data type (string, number, etc.)
    /// or can be any serializable .NET class that the recipient is able to deserialize.
    /// </remarks>
    public int PostDataToUser(string userId, object data)
    {
      return ((IServerManager) this.Session.GetObject("ServerManager")).PostDataToUser(userId, data);
    }

    /// <summary>
    /// Performs a data exchange with all users connected to the Encompass server.
    /// </summary>
    /// <param name="data">The data to be posted.</param>
    /// <returns>Returns the number of sessions to which the data was sent.</returns>
    /// <remarks>The data being posted must be a simple data type (string, number, etc.)
    /// or can be any serializable .NET class that the recipient is able to deserialize.
    /// </remarks>
    public int PostDataToAll(object data)
    {
      return ((IServerManager) this.Session.GetObject("ServerManager")).PostDataToAll(data);
    }

    /// <summary>
    /// Performs a data exchange with all users connected to the Encompass server.
    /// </summary>
    /// <param name="serverSessionId">The ServerSessionID of the session to receive the data.</param>
    /// <param name="data">The data to be posted.</param>
    /// <returns>Returns the number of sessions to which the data was sent.</returns>
    /// <remarks>The data being posted must be a simple data type (string, number, etc.)
    /// or can be any serializable .NET class that the recipient is able to deserialize.
    /// This method requries the ServerSessionID from the Session and not the ID.
    /// </remarks>
    public int PostDataToSession(string serverSessionId, object data)
    {
      return ((IServerManager) this.Session.GetObject("ServerManager")).PostDataToSession(serverSessionId, data);
    }

    /// <summary>Gets a custom data file from the Encompass Server.</summary>
    /// <param name="fileName">The name of the file to retrieve.</param>
    /// <returns>Returns a byte array containing the data from the remote file.</returns>
    public DataObject GetCustomDataObject(string fileName)
    {
      BinaryObject data = !((fileName ?? "") == "") ? ((IConfigurationManager) this.Session.GetObject("ConfigurationManager")).GetCustomDataObject(fileName) : throw new ArgumentException(nameof (fileName), "File name cannot be blank or null");
      return data == null ? (DataObject) null : new DataObject(data);
    }

    /// <summary>Saves a custom data file to the Encompass Server.</summary>
    /// <param name="fileName">The name of the file to save.</param>
    /// <param name="data">A byte array containing the data to be saved.</param>
    public void SaveCustomDataObject(string fileName, DataObject data)
    {
      if ((fileName ?? "") == "")
        throw new ArgumentException(nameof (fileName), "File name cannot be blank or null");
      if (data == null)
        throw new ArgumentNullException(nameof (data));
      ((IConfigurationManager) this.Session.GetObject("ConfigurationManager")).SaveCustomDataObject(fileName, data.Unwrap());
    }

    /// <summary>
    /// Appends data to a previously-created custom data file.
    /// </summary>
    /// <param name="fileName">The name of the file to which the data will be appended.</param>
    /// <param name="data">The DataObject containing the data to be saved.</param>
    /// <remarks>If the specified custom data object does not already exist, it will be created and
    /// the provided data will be added to it.</remarks>
    public void AppendToCustomDataObject(string fileName, DataObject data)
    {
      if ((fileName ?? "") == "")
        throw new ArgumentException(nameof (fileName), "File name cannot be blank or null");
      if (data == null)
        throw new ArgumentNullException(nameof (data));
      ((IConfigurationManager) this.Session.GetObject("ConfigurationManager")).AppendToCustomDataObject(fileName, data.Unwrap());
    }

    /// <summary>Saves a custom data file to the Encompass Server.</summary>
    /// <param name="fileName">The name of the file to delete.</param>
    public void DeleteCustomDataObject(string fileName)
    {
      if ((fileName ?? "") == "")
        throw new ArgumentException(nameof (fileName), "File name cannot be blank or null");
      ((IConfigurationManager) this.Session.GetObject("ConfigurationManager")).SaveCustomDataObject(fileName, (BinaryObject) null);
    }

    internal void OnDataExchangeEvent(DataExchangeEvent e)
    {
      this.dataReceived.Invoke((object) this, new DataExchangeEventArgs(e));
    }
  }
}
