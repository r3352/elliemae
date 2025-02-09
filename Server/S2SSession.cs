// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.S2SSession
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.Client;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Events;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class S2SSession
  {
    private const string className = "S2SSession�";
    private static readonly string sw = Tracing.SwRemoting;
    private IConnection conn;
    private string userPassword;
    private string remoteServer;
    private ISessionStartupInfo startupInfo;
    private SessionObjects sessionObjects;
    private SessionManager sessionMgr;
    private string serverUri;
    private string userId;
    private string password;
    private string appName;

    public event EventHandler Started;

    public event EventHandler Ended;

    internal S2SSession(SessionManager sessionMgr) => this.sessionMgr = sessionMgr;

    public ISession ISession => this.conn.Session;

    public void Start(string serverUri, string userId, string password, string appName)
    {
      this.serverUri = serverUri;
      this.userId = userId;
      this.password = password;
      this.appName = appName;
      this.End();
      EllieMae.EMLite.Client.Connection newConn = new EllieMae.EMLite.Client.Connection();
      newConn.Open(serverUri, userId, password, appName, false);
      newConn.ConnectionError += new ConnectionErrorEventHandler(this.handleConnectionError);
      this.initializeSession((IConnection) newConn, password);
    }

    internal void Reconnect(bool onlyIfNotConnected = true)
    {
      if (onlyIfNotConnected && this.IsConnected)
        return;
      this.Start(this.serverUri, this.userId, this.password, this.appName);
    }

    private void initializeSession(IConnection newConn, string password)
    {
      this.conn = newConn;
      this.userPassword = password;
      newConn.ServerEvent += new EllieMae.EMLite.Client.ServerEventHandler(this.handleServerEvent);
      if (newConn.Server != null)
        this.remoteServer = newConn.Server.ToString();
      this.startupInfo = this.conn.Session.GetSessionStartupInfo();
      this.sessionObjects = new SessionObjects(this.conn.Session, password, this.startupInfo);
      if (this.Started == null)
        return;
      this.Started((object) null, EventArgs.Empty);
    }

    public string EncompassSystemID => this.sessionObjects.SystemID;

    public void SetConnection(IConnection newConnection, string userPassword)
    {
      this.End();
      this.conn = newConnection;
      this.startupInfo = this.conn.Session.GetSessionStartupInfo();
      this.sessionObjects = new SessionObjects(this.conn.Session, userPassword, this.startupInfo);
    }

    public void End()
    {
      if (this.conn == null)
        return;
      if (this.conn is EllieMae.EMLite.Server.Connection)
        ((ConnectionBase) this.conn).ConnectionError -= new ConnectionErrorEventHandler(this.handleConnectionError);
      this.conn.Close();
      this.conn = (IConnection) null;
      this.userPassword = (string) null;
      this.remoteServer = (string) null;
      this.startupInfo = (ISessionStartupInfo) null;
      this.sessionObjects = (SessionObjects) null;
      if (this.Ended == null)
        return;
      this.Ended((object) null, EventArgs.Empty);
    }

    public bool IsConnected => this.conn != null;

    public string Password
    {
      get => this.userPassword;
      set => this.userPassword = value;
    }

    public IConnection Connection => this.conn;

    public DateTime ServerTime => this.conn.Session.ServerTime;

    public string UserID => this.sessionObjects.UserID;

    public UserInfo UserInfo => this.sessionObjects.UserInfo;

    public UserInfo RecacheUserInfo()
    {
      this.sessionObjects.InvalidateUserInfo();
      this.startupInfo.UserInfo = this.sessionObjects.UserInfo;
      return this.startupInfo.UserInfo;
    }

    public string RemoteServer => this.remoteServer;

    public ICurrentUser User => this.sessionObjects.CurrentUser;

    public IServerManager ServerManager => this.sessionObjects.ServerManager;

    public IMessengerListManager MessengerListManager => this.sessionObjects.MessengerListManager;

    private void handleServerEvent(IConnection conn, ServerEvent e)
    {
      switch (e)
      {
        case MessageEvent _:
          if (!(((MessageEvent) e).Message is S2SMessage message))
          {
            TraceLog.WriteError(nameof (S2SSession), "handleServerEvent: Unknown server-to-server message type.");
            break;
          }
          if (message.SessionIDs != null)
          {
            this.sessionMgr.SendMessage(message.OriginalMessage, message.SessionIDs, false);
            break;
          }
          this.sessionMgr.BroadcastMessage(message.OriginalMessage, false);
          break;
        case S2SDisconnectEvent _:
          S2SDisconnectEvent sdisconnectEvent = e as S2SDisconnectEvent;
          if (sdisconnectEvent.SessionIDs == null)
          {
            this.sessionMgr.TerminateAllSessions(sdisconnectEvent.EventArgument, false);
            break;
          }
          this.sessionMgr.TerminateSessions(sdisconnectEvent.SessionIDs, sdisconnectEvent.EventArgument != 0, false);
          break;
        case DisconnectEvent _:
          try
          {
            DisconnectEvent disconnectEvent = e as DisconnectEvent;
            if (this.conn == null || !(this.sessionObjects.SessionID == disconnectEvent.Context.SessionID))
              break;
            this.End();
            SessionManager.EndS2SSession(this.serverUri);
            break;
          }
          catch
          {
            break;
          }
      }
    }

    private void handleConnectionError(IConnection conn, ConnectionErrorType errType)
    {
      try
      {
        Tracing.Log(S2SSession.sw, nameof (S2SSession), TraceLevel.Error, "Error connecting to " + conn.Server.ToString() + "; error type: " + errType.ToString());
      }
      catch
      {
      }
      conn = (IConnection) null;
      this.End();
    }
  }
}
