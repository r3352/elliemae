// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Interfaces.ISessionManager
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.Events;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Interfaces
{
  public interface ISessionManager : IDisposable, IEnumerable
  {
    new IEnumerator GetEnumerator();

    IEventManager Events { get; }

    ITracingManager Tracing { get; }

    int SessionCount { get; }

    int SessionObjectCount { get; }

    void AddSession(IClientSession session, IConnectionManager connectionMgr);

    void AddS2SSession(IClientSession session, IConnectionManager connectionMgr);

    void AddServiceSession(string sessionId, string userId, string appName);

    void RemoveServiceSession(string sessionId);

    void RemoveSession(IClientSession session, IConnectionManager connectionMgr);

    void RemoveS2SSession(IClientSession session);

    string GetServiceSessionUser(string sessionId, bool updateAccessTime);

    SessionInfo GetServiceSessionInfo(string sessionId, bool updateAccessTime);

    Dictionary<string, string[]> GetSessionObjectNames(string userid);

    IClientSession GetSession(string sessionId);

    IClientSession[] GetSessionsByUser(string userid);

    SessionInfo GetSessionInfo(string sessionId);

    SessionInfo[] GetAllSessionInfo();

    SessionInfo[] GetAllSessionInfo(bool includeCurrentLoanInfo);

    SessionInfo[] GetAllSessionInfoFromDB();

    SessionInfo[] GetAllSessionInfoFromDB(bool includeCurrentLoanInfo);

    void TerminateS2SClientSession(string s2sSessionID, string reason);

    IClientSession[] GetAllS2SClientSessions();

    IClientSession[] GetSessionsForUser(string userId);

    SessionInfo[] GetAllSessionsForUser(string userId, bool includeDbSessions);

    KeyValuePair<string, string>[] GetSessionIDAndServerIDForUser(string userID);

    void TerminateAllSessions(
      DisconnectEventArgument disconnectEventArg,
      bool includeRemoteSessions);

    void TerminateSession(string sessionId, bool forceDisconnect, bool includeRemoteSession);

    void TerminateSessions(string[] sessionIds, bool forceDisconnects, bool includeRemoteSessions);

    void SendMessage(Message message, string sessionId, bool includeRemoteSession);

    void SendMessage(Message message, string[] sessionIds, bool includeRemoteSessions);

    void BroadcastMessage(Message message, bool includeRemoteSessions);

    void RaiseServerEventOnRemoveServers(Message message);

    void ClearExpiredServiceSessionsAndLocks();

    void ClearExpiredServiceSessionLocksFromDatabase(Guid loanId);

    IClientContext RegisterContext(IClientContext clientCtx, bool overrideIfExists = false);

    void UpdateSession(IClientSession session, IConnectionManager connectionMgr);

    void DeleteSessionFromDBOnly(string sessionID);

    void ClearConcurrentUpdateNotificationCache();

    void ClearConcurrentUpdateNotificationCache(string sessionId);
  }
}
