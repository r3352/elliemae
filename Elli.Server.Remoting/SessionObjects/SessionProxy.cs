// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.SessionObjects.SessionProxy
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Cache;
using EllieMae.EMLite.ClientServer.Events;
using EllieMae.EMLite.ClientServer.Interfaces;
using EllieMae.EMLite.ClientServer.SystemAuditTrail;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net;
using System.Runtime.Remoting;

#nullable disable
namespace Elli.Server.Remoting.SessionObjects
{
  public class SessionProxy : 
    RemotedObject,
    ISession,
    IClientSession,
    ISessionBoundObject,
    IDisposable,
    IClientCacheSettings
  {
    private Elli.Server.Remoting.SessionObjects.Session _innerSession;

    public SessionProxy Initialize(Elli.Server.Remoting.SessionObjects.Session innerSession)
    {
      this._innerSession = innerSession;
      new ConnectionManagerWrapper().RegisterSession((IClientSession) this);
      System.Runtime.Remoting.RemotingServices.SetObjectUriForMarshal((MarshalByRefObject) this, innerSession.SessionID + "/" + this.ObjectKey + ".rem");
      return this;
    }

    public virtual string ServerID => this._innerSession.ServerID;

    public virtual string SystemID => this._innerSession.SystemID;

    public virtual string SqlDbID => this._innerSession.SqlDbID;

    public virtual bool LoanImportInProgress
    {
      get => this._innerSession.LoanImportInProgress;
      set => this._innerSession.LoanImportInProgress = value;
    }

    public virtual SessionDiagnostics Diagnostics => this._innerSession.Diagnostics;

    public IClientContext Context => this._innerSession.Context;

    public virtual ISecurityManager SecurityManager => this._innerSession.SecurityManager;

    public virtual bool IMEnabled => this._innerSession.IMEnabled;

    public virtual bool CSEnabled => this._innerSession.CSEnabled;

    public virtual int RemoteObjectCount => this._innerSession.RemoteObjectCount;

    public virtual string[] SessionObjectNames => this._innerSession.SessionObjectNames;

    public virtual string SessionID => this._innerSession.SessionID;

    public virtual string UserID => this._innerSession.UserID;

    public virtual string Hostname => this._innerSession.Hostname;

    public virtual IPAddress HostAddress => this._innerSession.HostAddress;

    public virtual DateTime LoginTime => this._innerSession.LoginTime;

    public virtual LoginParameters LoginParams => this._innerSession.LoginParams;

    public virtual DateTime ServerTime => this._innerSession.ServerTime;

    public virtual string ServerTimeZone => this._innerSession.ServerTimeZone;

    public virtual string DisplayVersion => this._innerSession.DisplayVersion;

    public virtual SessionInfo SessionInfo => this._innerSession.SessionInfo;

    public virtual EllieMae.EMLite.ClientServer.ServerIdentity Server => this._innerSession.Server;

    public virtual ConcurrentDictionary<string, string> ConcurrentUpdatesNotificationCache
    {
      get => this._innerSession.ConcurrentUpdatesNotificationCache;
    }

    public ISession Session => (ISession) this._innerSession;

    public string ObjectKey => nameof (SessionProxy);

    public bool PersistentClientCacheEnabled => this._innerSession.PersistentClientCacheEnabled;

    public virtual void Abandon()
    {
      this._innerSession.Abandon();
      new ConnectionManagerWrapper().UnregisterSession((IClientSession) this);
      base.Disconnect();
    }

    public virtual object GetAclManager(AclCategory category)
    {
      return this._innerSession.GetAclManager(category);
    }

    public virtual SystemAuditRecord[] GetAuditRecord(
      string userID,
      ActionType actionType,
      DateTime startDTTM,
      DateTime endDTTM,
      string objectID,
      string objectName)
    {
      return this._innerSession.GetAuditRecord(userID, actionType, startDTTM, endDTTM, objectID, objectName);
    }

    public virtual object GetObject(string objectName) => this._innerSession.GetObject(objectName);

    public virtual ISessionStartupInfo GetSessionStartupInfo()
    {
      return this._innerSession.GetSessionStartupInfo();
    }

    public virtual ICurrentUser GetUser() => this._innerSession.GetUser();

    public virtual UserInfo GetUserInfo() => this._innerSession.GetUserInfo();

    public virtual void ImpersonateUser(string userId)
    {
      this._innerSession.ImpersonateUser(userId);
    }

    public virtual void InitDataSyncManager() => this._innerSession.InitDataSyncManager();

    public virtual void InsertAuditRecord(SystemAuditRecord record)
    {
      this._innerSession.InsertAuditRecord(record);
    }

    public virtual bool Ping(TimeSpan timeout) => this._innerSession.Ping(timeout);

    public virtual IAsyncResult RaiseEvent(ServerEvent e) => this._innerSession.RaiseEvent(e);

    public virtual void RegisterForEvents(Type eventType)
    {
      this._innerSession.RegisterForEvents(eventType);
    }

    public virtual void RegisterForTracing(TraceLevel traceLevel)
    {
      this._innerSession.RegisterForTracing(traceLevel);
    }

    public virtual void RegisterSessionObject(ISessionBoundObject o)
    {
      this._innerSession.RegisterSessionObject(o);
    }

    public virtual void ReleaseSessionObject(ISessionBoundObject o)
    {
      this._innerSession.ReleaseSessionObject(o);
    }

    public virtual void RestoreIdentity() => this._innerSession.RestoreIdentity();

    public virtual IAsyncResult SendMessage(Message message)
    {
      return this._innerSession.SendMessage(message);
    }

    public virtual void Terminate(DisconnectEventArgument disconnectEventArg, string reason)
    {
      this._innerSession.Terminate(disconnectEventArg, reason);
      new ConnectionManagerWrapper().UnregisterSession((IClientSession) this);
      base.Disconnect();
    }

    public virtual void UnregisterForEvents(Type eventType)
    {
      this._innerSession.UnregisterForEvents(eventType);
    }

    public virtual void UnregisterForTracing() => this._innerSession.UnregisterForTracing();

    public override sealed object InitializeLifetimeService() => base.InitializeLifetimeService();

    public override sealed ObjRef CreateObjRef(Type requestedType)
    {
      return base.CreateObjRef(requestedType);
    }

    public override sealed void Disconnect()
    {
      this._innerSession.Disconnect();
      new ConnectionManagerWrapper().UnregisterSession((IClientSession) this);
      base.Disconnect();
    }

    public override sealed string ToString() => this._innerSession.ToString();

    public void onApiCalled(string className, string apiName, params object[] parms)
    {
    }

    public void Dispose()
    {
    }
  }
}
