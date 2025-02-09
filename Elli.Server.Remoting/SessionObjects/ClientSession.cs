// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.SessionObjects.ClientSession
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Events;
using System;
using System.Collections.Concurrent;
using System.Net;

#nullable disable
namespace Elli.Server.Remoting.SessionObjects
{
  public abstract class ClientSession : RemotedObject, IClientSession
  {
    private LoginParameters loginParams;
    private DateTime loginTime = DateTime.Now;
    private string hostname;
    private IPAddress hostAddr;
    private IServerCallback callback;
    private string sessionId;
    private SessionInfo sessionInfo;
    private ConcurrentDictionary<string, string> externalUpdatesNotificationCache;

    protected ClientSession(
      LoginParameters loginParams,
      string sessionId,
      IServerCallback callback)
    {
      this.sessionId = sessionId;
      this.loginParams = loginParams;
      this.hostname = loginParams.Hostname;
      this.hostAddr = (IPAddress) null;
      if (!loginParams.OfflineMode)
        this.hostAddr = ConnectionManager.GetCurrentClientIPAddress();
      this.callback = callback;
      this.sessionInfo = new SessionInfo((IClientSession) this);
      this.externalUpdatesNotificationCache = new ConcurrentDictionary<string, string>();
    }

    public virtual ConcurrentDictionary<string, string> ConcurrentUpdatesNotificationCache
    {
      get => this.externalUpdatesNotificationCache;
    }

    public virtual LoginParameters LoginParams => this.loginParams;

    protected virtual IServerCallback ClientCallback => this.callback;

    public virtual SessionInfo SessionInfo => this.sessionInfo;

    public virtual void Terminate(DisconnectEventArgument disconnectEventArg, string reason)
    {
      lock (this)
      {
        if (this.ClientCallback == null)
          return;
        AsyncEventResult asyncEventResult = (AsyncEventResult) null;
        try
        {
          asyncEventResult = (AsyncEventResult) this.RaiseEvent((ServerEvent) new DisconnectEvent(this.SessionInfo, disconnectEventArg));
        }
        catch
        {
        }
        if (disconnectEventArg == DisconnectEventArgument.Nonforce)
          return;
        asyncEventResult?.WaitOne(new TimeSpan(0, 0, 5), true);
        this.Disconnect();
      }
    }

    internal virtual void S2STerminate(string[] sessionIDs, bool forceDisconnect)
    {
      lock (this)
      {
        if (this.ClientCallback == null)
          return;
        try
        {
          this.RaiseEvent((ServerEvent) new S2SDisconnectEvent(sessionIDs, this.SessionInfo, forceDisconnect ? DisconnectEventArgument.Force : DisconnectEventArgument.Nonforce));
        }
        catch
        {
        }
      }
    }

    internal virtual void S2STerminateAll(bool forceDisconnect)
    {
      this.S2STerminate((string[]) null, forceDisconnect);
    }

    public IAsyncResult RaiseEvent(ServerEvent e)
    {
      if (this.ClientCallback == null)
        return (IAsyncResult) null;
      return (IAsyncResult) EventQueue.Enqueue(new DelegateInvoker((Delegate) new SessionEventHandler(this.ClientCallback.OnServerEvent), new object[1]
      {
        (object) e
      }));
    }

    public bool Ping(TimeSpan timeout)
    {
      try
      {
        AsyncEventResult asyncEventResult = (AsyncEventResult) this.RaiseEvent((ServerEvent) new PingEvent(this.SessionInfo));
        return asyncEventResult.WaitOne(timeout, false) && asyncEventResult.EventStatus == AsyncEventStatus.Success;
      }
      catch
      {
      }
      return false;
    }

    public virtual IAsyncResult SendMessage(Message message)
    {
      AsyncEventResult asyncEventResult = (AsyncEventResult) null;
      try
      {
        asyncEventResult = (AsyncEventResult) this.RaiseEvent((ServerEvent) new MessageEvent(this.sessionInfo, message));
      }
      catch
      {
      }
      return (IAsyncResult) asyncEventResult;
    }

    public virtual AsyncEventResult SendS2SMessage(string[] sessionIDs, Message message)
    {
      AsyncEventResult asyncEventResult = (AsyncEventResult) null;
      try
      {
        asyncEventResult = (AsyncEventResult) this.RaiseEvent((ServerEvent) new MessageEvent(this.sessionInfo, (Message) new S2SMessage(sessionIDs, message)));
      }
      catch
      {
      }
      return asyncEventResult;
    }

    internal virtual AsyncEventResult BroadcastS2SMessage(Message message)
    {
      return this.SendS2SMessage((string[]) null, message);
    }

    public string SessionID => this.sessionId;

    public virtual string UserID => this.loginParams.UserID;

    public ServerIdentity Server => this.loginParams.Server;

    public DateTime LoginTime => this.loginTime;

    public string Hostname => this.hostname;

    public IPAddress HostAddress => this.hostAddr;

    public DateTime ServerTime => DateTime.Now;

    public string ServerTimeZone => TimeZoneInfo.Local.Id;

    public string DisplayVersion => this.loginParams.ClientDisplayVersion;

    public virtual void Abandon() => this.Disconnect();

    public override void Disconnect()
    {
      this.callback = (IServerCallback) null;
      base.Disconnect();
    }
  }
}
