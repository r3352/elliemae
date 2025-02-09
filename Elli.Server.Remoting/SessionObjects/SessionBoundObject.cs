// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.SessionObjects.SessionBoundObject
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Interfaces;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Server;
using System;
using System.Runtime.Remoting;

#nullable disable
namespace Elli.Server.Remoting.SessionObjects
{
  public abstract class SessionBoundObject : RemotedObject, ISessionBoundObject, IDisposable
  {
    private const string className = "SessionBoundObject";
    private ISession session;
    private string objectKey = "";

    protected void InitializeInternal(ISession session)
    {
      this.InitializeInternal(session, Guid.NewGuid().ToString());
    }

    protected void InitializeInternal(ISession session, string objectKey)
    {
      this.session = session;
      this.objectKey = objectKey;
      session.RegisterSessionObject((ISessionBoundObject) this);
      RemotingServices.SetObjectUriForMarshal((MarshalByRefObject) this, session.SessionID + "/" + objectKey + ".rem");
    }

    public ISession Session => this.session;

    public string ObjectKey => this.objectKey;

    protected ISecurityManager Security => this.session.SecurityManager;

    public IClientContext Context => this.Session?.Context;

    public virtual void Dispose()
    {
      try
      {
        this.session.ReleaseSessionObject((ISessionBoundObject) this);
        this.Disconnect();
      }
      catch
      {
      }
    }

    public override sealed object InitializeLifetimeService() => base.InitializeLifetimeService();

    public override sealed void Disconnect() => base.Disconnect();

    public override sealed ObjRef CreateObjRef(Type requestedType)
    {
      return base.CreateObjRef(requestedType);
    }

    protected string formatMsg(string msg)
    {
      return "[" + this.Session.UserID + " / " + this.Session.SessionID + "] " + msg;
    }

    public string dbReadReplicaLog(
      IClientContext context,
      DBReadReplicaFeature readReplicaAccessorFeature,
      bool forceToPrimaryDb = false)
    {
      bool replicaUseAgListener = context.Settings.IsReadReplicaUseAGListener;
      return !replicaUseAgListener || !(!Utils.ParseBoolean((object) Company.GetCompanySetting(context, "DisableReadReplica", readReplicaAccessorFeature.ToString())) & replicaUseAgListener) || forceToPrimaryDb ? "<DBReadReplicaFeature:" + readReplicaAccessorFeature.ToString() + "><DB:Primary>" : "<DBReadReplicaFeature:" + readReplicaAccessorFeature.ToString() + "><DB:ReadReplica>";
    }

    public void onApiCalled(string className, string apiName, params object[] parms)
    {
      try
      {
        this.Session.Diagnostics.RecordAPICall(className + "." + apiName);
        if (ClientContext.GetCurrent(false) == null)
          return;
        ClientContext.GetCurrent().RecordLogflag();
        ClientContext.GetCurrent().RecordClassName(className);
        ClientContext.GetCurrent().RecordApiName(apiName);
        ClientContext.GetCurrent().RecordParms(parms);
        ClientContext.GetCurrent().RecordSession(this.session);
      }
      catch
      {
      }
    }
  }
}
