// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ISession
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.Cache;
using EllieMae.EMLite.ClientServer.Interfaces;
using EllieMae.EMLite.ClientServer.SystemAuditTrail;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public interface ISession : IClientSession
  {
    string ServerID { get; }

    string SystemID { get; }

    string SqlDbID { get; }

    ICurrentUser GetUser();

    object GetObject(string objectName);

    [PersistentClientCacheable(typeof (SessionStartupInfoETagsProvider))]
    ISessionStartupInfo GetSessionStartupInfo();

    object GetAclManager(AclCategory category);

    void ImpersonateUser(string userId);

    void RestoreIdentity();

    void RegisterForEvents(Type eventType);

    void UnregisterForEvents(Type eventType);

    void RegisterForTracing(TraceLevel traceLevel);

    void UnregisterForTracing();

    void RegisterSessionObject(ISessionBoundObject o);

    void ReleaseSessionObject(ISessionBoundObject o);

    bool LoanImportInProgress { get; set; }

    UserInfo GetUserInfo();

    SessionDiagnostics Diagnostics { get; }

    void InitDataSyncManager();

    void InsertAuditRecord(SystemAuditRecord record);

    SystemAuditRecord[] GetAuditRecord(
      string userID,
      ActionType actionType,
      DateTime startDTTM,
      DateTime endDTTM,
      string objectID,
      string objectName);

    IClientContext Context { get; }

    ISecurityManager SecurityManager { get; }

    bool IMEnabled { get; }

    bool CSEnabled { get; }

    int RemoteObjectCount { get; }

    string[] SessionObjectNames { get; }
  }
}
