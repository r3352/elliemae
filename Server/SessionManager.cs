// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.SessionManager
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using com.elliemae.services.eventbus.models;
using EllieMae.EMLite.Cache;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Events;
using EllieMae.EMLite.ClientServer.Interfaces;
using EllieMae.EMLite.ClientServer.MessageServices.Kafka.Event;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Version;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.Server.ServerObjects;
using EllieMae.EMLite.Server.ServiceObjects.KafkaEvent;
using EllieMae.EMLite.Server.ServiceObjects.Repositories;
using EllieMae.EMLite.ServiceInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Net;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public sealed class SessionManager : ISessionManager, IDisposable, IEnumerable
  {
    private const string className = "SessionManager�";
    private const int DefaultSessionTimeoutMinutes = 15;
    private static readonly int sessionTimeoutMinutes = 15;
    private IClientContext context;
    private Hashtable sessions = new Hashtable();
    private Hashtable s2sSessions = new Hashtable();
    private EventManager events = new EventManager();
    private TracingManager tracing;
    public static Hashtable S2SSessions = CollectionsUtil.CreateCaseInsensitiveHashtable();
    private static string serverName = (string) null;
    private const int noTries = 3;
    private object localServerIDLock = new object();
    private string localServerID;

    static SessionManager()
    {
      SessionManager.serverName = Dns.GetHostName();
      int result;
      if (!int.TryParse(ConfigurationManager.AppSettings["Session.TimeoutMinutes"], out result) || result <= 0)
        return;
      SessionManager.sessionTimeoutMinutes = result;
    }

    public SessionManager(IClientContext context)
    {
      this.context = context;
      this.tracing = new TracingManager(context);
      EncompassServer.ServerEvent += new ServerEventHandler(this.onServerEvent);
    }

    public IClientContext RegisterContext(IClientContext clientCtx, bool overrideIfExists = false)
    {
      if (this.context != null && !overrideIfExists)
        return clientCtx;
      this.context = clientCtx;
      this.tracing = new TracingManager(clientCtx);
      return clientCtx;
    }

    public IEnumerator GetEnumerator()
    {
      lock (this.sessions)
        return new ArrayList(this.sessions.Values).GetEnumerator();
    }

    public void Dispose()
    {
      try
      {
        EncompassServer.ServerEvent -= new ServerEventHandler(this.onServerEvent);
      }
      catch
      {
      }
    }

    public IEventManager Events => (IEventManager) this.events;

    public ITracingManager Tracing => (ITracingManager) this.tracing;

    public int SessionCount
    {
      get
      {
        lock (this.sessions)
          return this.sessions.Count;
      }
    }

    public int SessionObjectCount
    {
      get
      {
        lock (this.sessions)
        {
          int sessionObjectCount = 0;
          foreach (object obj in (IEnumerable) this.sessions.Values)
          {
            if (obj is ISession session)
              sessionObjectCount += session.RemoteObjectCount;
          }
          return sessionObjectCount;
        }
      }
    }

    public void AddSession(IClientSession session, IConnectionManager connectionMgr)
    {
      lock (this.sessions)
      {
        if (session is ISession)
          this.addSessionToDatabase(session as ISession);
        this.sessions.Add((object) session.SessionID, (object) session);
        connectionMgr.RegisterSession(session);
      }
    }

    public void UpdateSession(IClientSession session, IConnectionManager connectionMgr)
    {
      lock (this.sessions)
      {
        if (!(session is ISession))
          return;
        this.updateSessionInDatabase(session as ISession);
        this.sessions.Add((object) session.SessionID, (object) session);
        connectionMgr.RegisterSession(session);
      }
    }

    public void AddS2SSession(IClientSession session, IConnectionManager connectionMgr)
    {
      lock (this.s2sSessions)
      {
        this.s2sSessions.Add((object) session.SessionID, (object) session);
        connectionMgr.RegisterSession(session);
      }
    }

    public void AddServiceSession(string sessionId, string userId, string appName)
    {
      this.addServiceSessionToDatabase(sessionId, userId, appName);
    }

    public void RemoveServiceSession(string sessionId)
    {
      sessionId = SessionIdentity.Parse(sessionId).SessionID;
      using (this.context.MakeCurrent())
        new SessionCacheRepository((IDataCache) null).CleanSessionCache(sessionId);
      this.deleteLocksFromDatabase(sessionId);
      this.deleteSessionFromDatabase(sessionId);
    }

    public void ClearConcurrentUpdateNotificationCache()
    {
      lock (this.sessions)
      {
        foreach (object key in (IEnumerable) this.sessions.Keys)
          ((IClientSession) this.sessions[key]).ConcurrentUpdatesNotificationCache.Clear();
      }
    }

    public void ClearConcurrentUpdateNotificationCache(string sessionId)
    {
      this.GetSession(sessionId)?.ConcurrentUpdatesNotificationCache.Clear();
    }

    public void RemoveSession(IClientSession session, IConnectionManager connectionMgr)
    {
      lock (this.sessions)
      {
        if (this.sessions.ContainsKey((object) session.SessionID))
          this.sessions.Remove((object) session.SessionID);
        if (session is ISession)
        {
          this.deleteLocksFromDatabase(session.SessionID);
          this.deleteSessionFromDatabase(session.SessionID);
        }
        this.events.UnregisterListenerFromAll(session);
        this.tracing.UnregisterListener(session);
        connectionMgr.UnregisterSession(session);
      }
    }

    public void DeleteSessionFromDBOnly(string sessionID)
    {
      this.deleteSessionFromDatabase(sessionID);
    }

    public void RemoveS2SSession(IClientSession session)
    {
      lock (this.s2sSessions)
      {
        if (!this.s2sSessions.ContainsKey((object) session.SessionID))
          return;
        this.s2sSessions.Remove((object) session.SessionID);
      }
    }

    private string getHostname(string serverUri)
    {
      string hostname = serverUri;
      int num = hostname.IndexOf("://");
      if (num >= 0)
        hostname = hostname.Substring(num + 3);
      int length1 = hostname.LastIndexOf("$");
      if (length1 >= 0)
        hostname = hostname.Substring(0, length1);
      int length2 = hostname.LastIndexOf("/");
      if (length2 >= 0)
        hostname = hostname.Substring(0, length2);
      return hostname;
    }

    internal IClientSession GetS2SClientSession(string serverUri)
    {
      string hostname = this.getHostname(serverUri);
      TraceLog.WriteInfo(nameof (SessionManager), "Server uri: " + serverUri + "; host name: " + hostname);
      lock (this.s2sSessions)
      {
        foreach (string key in (IEnumerable) this.s2sSessions.Keys)
        {
          IClientSession s2sSession = (IClientSession) this.s2sSessions[(object) key];
          TraceLog.WriteInfo(nameof (SessionManager), "ClientSession hostname: " + s2sSession.Hostname);
          if (string.Compare(s2sSession.Hostname, hostname, StringComparison.OrdinalIgnoreCase) == 0)
            return s2sSession;
        }
        return (IClientSession) null;
      }
    }

    public string GetServiceSessionUser(string sessionId, bool updateAccessTime)
    {
      return this.getSessionFromDatabase(sessionId, updateAccessTime)?.Userid;
    }

    public SessionInfo GetServiceSessionInfo(string sessionId, bool updateAccessTime)
    {
      SessionManager.DbSessionInfo sessionFromDatabase = this.getSessionFromDatabase(sessionId, updateAccessTime);
      IPAddress hostAddr;
      try
      {
        hostAddr = IPAddress.Parse(sessionFromDatabase.IPAddress);
      }
      catch
      {
        hostAddr = (IPAddress) null;
      }
      if (sessionFromDatabase != null)
        return new SessionInfo(sessionFromDatabase.ServerInfo, sessionFromDatabase.SessionID, sessionFromDatabase.SessionType, sessionFromDatabase.Userid, sessionFromDatabase.Hostname, hostAddr, sessionFromDatabase.DisplayVersion, sessionFromDatabase.LoginTime, sessionFromDatabase.ServerHostname);
      return (SessionInfo) null;
    }

    public Dictionary<string, string[]> GetSessionObjectNames(string userid)
    {
      Dictionary<string, string[]> sessionObjectNames = new Dictionary<string, string[]>();
      lock (this.sessions)
      {
        foreach (object obj in (IEnumerable) this.sessions.Values)
        {
          if (obj is ISession session && string.Compare(session.UserID, userid, true) == 0)
            sessionObjectNames.Add(session.SessionID, session.SessionObjectNames);
        }
      }
      return sessionObjectNames;
    }

    public IClientSession GetSession(string sessionId)
    {
      lock (this.sessions)
        return (IClientSession) this.sessions[(object) sessionId];
    }

    public IClientSession[] GetSessionsByUser(string userid)
    {
      List<IClientSession> clientSessionList = new List<IClientSession>();
      lock (this.sessions)
      {
        foreach (IClientSession session in this.sessions)
        {
          if (session.UserID == userid)
            clientSessionList.Add(session);
        }
      }
      return clientSessionList.ToArray();
    }

    public SessionInfo GetSessionInfo(string sessionId) => this.GetSession(sessionId)?.SessionInfo;

    public SessionInfo[] GetAllSessionInfo() => this.GetAllSessionInfo(true);

    public SessionInfo[] GetAllSessionInfo(bool includeCurrentLoanInfo)
    {
      ArrayList arrayList = new ArrayList();
      lock (this.sessions)
      {
        foreach (IClientSession clientSession in (IEnumerable) this.sessions.Values)
        {
          if (clientSession is ISession session)
            clientSession.SessionInfo.SessionObjectCount = session.RemoteObjectCount;
          arrayList.Add((object) clientSession.SessionInfo);
        }
      }
      SessionInfo[] array = (SessionInfo[]) arrayList.ToArray(typeof (SessionInfo));
      if (includeCurrentLoanInfo)
        this.populateCurrentLoanInfo(array);
      return array;
    }

    public SessionInfo[] GetAllSessionInfoFromDB() => this.GetAllSessionInfoFromDB(true);

    public SessionInfo[] GetAllSessionInfoFromDB(bool includeCurrentLoanInfo)
    {
      SessionManager.DbSessionInfo[] sessionsFromDatabase = this.getAllSessionsFromDatabase();
      SessionInfo[] sessions = new SessionInfo[sessionsFromDatabase.Length];
      for (int index = 0; index < sessions.Length; ++index)
      {
        IPAddress hostAddr;
        try
        {
          hostAddr = IPAddress.Parse(sessionsFromDatabase[index].IPAddress);
        }
        catch
        {
          hostAddr = (IPAddress) null;
        }
        sessions[index] = new SessionInfo(sessionsFromDatabase[index].ServerInfo, sessionsFromDatabase[index].SessionID, sessionsFromDatabase[index].SessionType, sessionsFromDatabase[index].Userid, sessionsFromDatabase[index].Hostname, hostAddr, sessionsFromDatabase[index].DisplayVersion, sessionsFromDatabase[index].LoginTime, sessionsFromDatabase[index].ServerHostname);
      }
      if (includeCurrentLoanInfo)
        this.populateCurrentLoanInfo(sessions);
      return sessions;
    }

    public void TerminateS2SClientSession(string s2sSessionID, string reason)
    {
      try
      {
        lock (this.s2sSessions)
        {
          if (!this.s2sSessions.ContainsKey((object) s2sSessionID))
            return;
          ((IClientSession) this.s2sSessions[(object) s2sSessionID]).Terminate(DisconnectEventArgument.Force, reason);
          this.s2sSessions.Remove((object) s2sSessionID);
        }
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (SessionManager), "Error terminating S2S ClientSession for session ID " + s2sSessionID + ": " + ex.Message);
      }
    }

    public static void EndS2SSession(string serverUri)
    {
      try
      {
        serverUri = (serverUri ?? "").Trim();
        lock (SessionManager.S2SSessions)
        {
          if (!SessionManager.S2SSessions.ContainsKey((object) serverUri))
            return;
          S2SSession s2Ssession = (S2SSession) SessionManager.S2SSessions[(object) serverUri];
          try
          {
            s2Ssession.End();
          }
          catch
          {
          }
          if (s2Ssession.IsConnected)
            return;
          SessionManager.S2SSessions.Remove((object) serverUri);
        }
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (SessionManager), "Error terminating S2SSession for server Uri " + serverUri + ": " + ex.Message);
      }
    }

    public S2SSession[] GetAllS2SSessions()
    {
      lock (SessionManager.S2SSessions)
      {
        S2SSession[] allS2Ssessions = new S2SSession[SessionManager.S2SSessions.Count];
        SessionManager.S2SSessions.Values.CopyTo((Array) allS2Ssessions, 0);
        return allS2Ssessions;
      }
    }

    public IClientSession[] GetAllS2SClientSessions()
    {
      lock (this.s2sSessions)
      {
        object[] s2SclientSessions = new object[this.s2sSessions.Count];
        this.s2sSessions.Values.CopyTo((Array) s2SclientSessions, 0);
        return (IClientSession[]) s2SclientSessions;
      }
    }

    private void populateCurrentLoanInfo(SessionInfo[] sessions)
    {
      if (sessions == null || sessions.Length == 0)
        return;
      Dictionary<string, SessionInfo.BasicLoanInfo> sessionLockInfo = this.getSessionLockInfo(false);
      foreach (SessionInfo session in sessions)
        session.CurrentLoanInfo = !sessionLockInfo.ContainsKey(session.SessionID) ? (SessionInfo.BasicLoanInfo) null : sessionLockInfo[session.SessionID];
    }

    public IClientSession[] GetSessionsForUser(string userId)
    {
      Hashtable hashtable = this.cloneSessions();
      ArrayList arrayList = new ArrayList();
      foreach (IClientSession clientSession in (IEnumerable) hashtable.Values)
      {
        if (clientSession.UserID == userId)
          arrayList.Add((object) clientSession);
      }
      return (IClientSession[]) arrayList.ToArray(typeof (IClientSession));
    }

    public SessionInfo[] GetAllSessionsForUser(string userId, bool includeDbSessions)
    {
      List<SessionInfo> sessionInfoList = new List<SessionInfo>();
      IClientSession[] sessionsForUser = this.GetSessionsForUser(userId);
      if (sessionsForUser != null)
      {
        foreach (IClientSession session in sessionsForUser)
          sessionInfoList.Add(new SessionInfo(session));
      }
      if (includeDbSessions)
      {
        foreach (SessionInfo sessionInfo1 in this.GetAllSessionInfoFromDB())
        {
          if (sessionInfo1.UserID == userId)
          {
            bool flag = false;
            foreach (SessionInfo sessionInfo2 in sessionInfoList)
            {
              if (sessionInfo1.SessionID == sessionInfo2.SessionID)
              {
                flag = true;
                break;
              }
            }
            if (!flag)
              sessionInfoList.Add(sessionInfo1);
          }
        }
      }
      return sessionInfoList.ToArray();
    }

    public KeyValuePair<string, string>[] GetSessionIDAndServerIDForUser(string userID)
    {
      using (this.context.MakeCurrent())
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine("select [SessionID], [ServerID] from [Sessions] where [UserID] = " + SQL.Encode((object) userID));
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        if (dataRowCollection.Count == 0)
          return new KeyValuePair<string, string>[0];
        KeyValuePair<string, string>[] andServerIdForUser = new KeyValuePair<string, string>[dataRowCollection.Count];
        for (int index = 0; index < dataRowCollection.Count; ++index)
          andServerIdForUser[index] = new KeyValuePair<string, string>(dataRowCollection[index]["SessionID"] as string, dataRowCollection[index]["ServerID"] as string);
        return andServerIdForUser;
      }
    }

    private void terminateRemoteSessions(
      string serverUri,
      string[] sessionIds,
      bool forceDisconnect)
    {
      if ((serverUri ?? "").Trim() == "")
        return;
      for (int index = 0; index < 3; ++index)
      {
        try
        {
          S2SSession s2Ssession = this.GetS2SSession(serverUri, index > 0);
          if (s2Ssession != null)
          {
            s2Ssession.ServerManager.TerminateSessions(sessionIds, forceDisconnect, false);
            break;
          }
        }
        catch (Exception ex)
        {
          if (index == 2)
            TraceLog.WriteError(nameof (SessionManager), "Error terminating sessions on server " + serverUri + ": " + ex.Message);
        }
      }
    }

    private void terminateAllRemoteSessions(string serverUri, bool forceDisconnect)
    {
      if ((serverUri ?? "").Trim() == "")
        return;
      for (int index = 0; index < 3; ++index)
      {
        try
        {
          S2SSession s2Ssession = this.GetS2SSession(serverUri, index > 0);
          if (s2Ssession != null)
          {
            s2Ssession.ServerManager.TerminateAllSessions(forceDisconnect, false);
            break;
          }
        }
        catch (Exception ex)
        {
          if (index == 2)
            TraceLog.WriteError(nameof (SessionManager), "Error terminating all sessions on server " + serverUri + ": " + ex.Message);
        }
      }
    }

    public void TerminateAllSessions(
      DisconnectEventArgument disconnectEventArg,
      bool includeRemoteSessions)
    {
      Hashtable hashtable = this.cloneSessions();
      if (includeRemoteSessions)
      {
        ArrayList arrayList = new ArrayList();
        foreach (SessionInfo sessionInfo in this.GetAllSessionInfoFromDB())
        {
          if (!hashtable.ContainsKey((object) sessionInfo.SessionID) && !arrayList.Contains((object) sessionInfo.Server.ToString()))
            arrayList.Add((object) sessionInfo.Server.ToString());
        }
        foreach (string serverUri in arrayList)
          this.terminateAllRemoteSessions(serverUri, disconnectEventArg != 0);
      }
      foreach (IClientSession clientSession in (IEnumerable) hashtable.Values)
        clientSession.Terminate(disconnectEventArg, "Forced by Admin");
    }

    public void TerminateSession(string sessionId, bool forceDisconnect, bool includeRemoteSession)
    {
      this.TerminateSessions(new string[1]{ sessionId }, forceDisconnect, includeRemoteSession);
    }

    public void ResetConcurrentUpdateNotificationCache()
    {
    }

    public void TerminateSessions(
      string[] sessionIds,
      bool forceDisconnects,
      bool includeRemoteSessions)
    {
      Hashtable insensitiveHashtable = CollectionsUtil.CreateCaseInsensitiveHashtable();
      foreach (string sessionId in sessionIds)
      {
        IClientSession session = this.GetSession(sessionId);
        if (session != null)
          session.Terminate(forceDisconnects ? DisconnectEventArgument.Force : DisconnectEventArgument.Nonforce, "Forced by Admin");
        else if (includeRemoteSessions)
        {
          SessionManager.DbSessionInfo sessionFromDatabase = this.getSessionFromDatabase(sessionId);
          if (sessionFromDatabase != null && sessionFromDatabase.ServerInfo != null)
          {
            string key = sessionFromDatabase.ServerInfo.ToString();
            if (insensitiveHashtable.ContainsKey((object) key))
              ((List<string>) insensitiveHashtable[(object) key]).Add(sessionId);
            else
              insensitiveHashtable[(object) key] = (object) new List<string>()
              {
                sessionId
              };
          }
        }
      }
      foreach (string key in (IEnumerable) insensitiveHashtable.Keys)
      {
        List<string> stringList = (List<string>) insensitiveHashtable[(object) key];
        this.terminateRemoteSessions(key, stringList.ToArray(), forceDisconnects);
      }
      if (!(includeRemoteSessions & forceDisconnects))
        return;
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder(this.context);
        dbQueryBuilder.Append("delete from [Sessions] where [SessionID] = ");
        bool flag = true;
        foreach (string sessionId in sessionIds)
        {
          if (flag)
            flag = false;
          else
            dbQueryBuilder.Append(" or [SessionID] = ");
          dbQueryBuilder.Append(SQL.Encode((object) sessionId));
        }
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (SessionManager), "Error deleting database sessions: " + ex.Message);
      }
    }

    public S2SSession GetS2SSession(string serverUri, bool reconnect)
    {
      serverUri = (serverUri ?? "").Trim();
      if (serverUri.EndsWith("/"))
        serverUri = serverUri.Substring(0, serverUri.Length - 1);
      S2SSession s2Ssession = (S2SSession) null;
      for (int index = 0; index < 3; ++index)
      {
        try
        {
          lock (SessionManager.S2SSessions)
          {
            if (SessionManager.S2SSessions.ContainsKey((object) serverUri) & reconnect)
            {
              ((S2SSession) SessionManager.S2SSessions[(object) serverUri]).End();
              SessionManager.S2SSessions.Remove((object) serverUri);
            }
            if (!SessionManager.S2SSessions.ContainsKey((object) serverUri))
            {
              s2Ssession = new S2SSession(this);
              try
              {
                s2Ssession.Start(serverUri, "admin", ClientContext.GetCurrent().Settings.ConnectionString, "trustedEnc");
              }
              catch (Exception ex)
              {
                TraceLog.WriteError(nameof (SessionManager), "Error starting server-to-server session (" + serverUri + "): " + ex.Message);
                s2Ssession = (S2SSession) null;
                throw ex;
              }
              SessionManager.S2SSessions.Add((object) serverUri, (object) s2Ssession);
              break;
            }
            s2Ssession = (S2SSession) SessionManager.S2SSessions[(object) serverUri];
            s2Ssession.Reconnect();
            break;
          }
        }
        catch (Exception ex)
        {
          if (index == 2)
            TraceLog.WriteError(nameof (SessionManager), "Error getting server-to-server session (" + serverUri + "): " + ex.Message);
        }
      }
      return s2Ssession;
    }

    private void sendMessageToRemoteSessions(
      string serverUri,
      Message message,
      string[] sessionIds)
    {
      if ((serverUri ?? "").Trim() == "")
        return;
      for (int index = 0; index < 3; ++index)
      {
        try
        {
          S2SSession s2Ssession = this.GetS2SSession(serverUri, index > 0);
          if (s2Ssession != null)
          {
            if (sessionIds == null)
            {
              s2Ssession.ServerManager.BroadcastMessage(message, false);
              break;
            }
            s2Ssession.ServerManager.SendMessage(message, sessionIds, false);
            break;
          }
        }
        catch (Exception ex)
        {
          if (index == 2)
            TraceLog.WriteError(nameof (SessionManager), "Error " + (sessionIds == null ? "broadcasting" : "sending") + " message " + (sessionIds == null ? "on" : "to") + " remote server " + serverUri + ": " + ex.Message);
        }
      }
    }

    private void broadcastMessageOnRemoteServer(string serverUri, Message message)
    {
      this.sendMessageToRemoteSessions(serverUri, message, (string[]) null);
    }

    public void SendMessage(Message message, string sessionId, bool includeRemoteSession)
    {
      this.SendMessage(message, new string[1]{ sessionId }, (includeRemoteSession ? 1 : 0) != 0);
    }

    public void SendMessage(Message message, string[] sessionIds, bool includeRemoteSessions)
    {
      Hashtable insensitiveHashtable = CollectionsUtil.CreateCaseInsensitiveHashtable();
      foreach (string sessionId in sessionIds)
      {
        IClientSession session = this.GetSession(sessionId);
        bool flag = false;
        if (session != null)
        {
          session.SendMessage(message);
          flag = true;
        }
        else if (includeRemoteSessions)
        {
          SessionManager.DbSessionInfo sessionFromDatabase = this.getSessionFromDatabase(sessionId);
          if (sessionFromDatabase != null && sessionFromDatabase.ServerInfo != null)
          {
            flag = true;
            string key = sessionFromDatabase.ServerInfo.ToString();
            if (insensitiveHashtable.ContainsKey((object) key))
              ((List<string>) insensitiveHashtable[(object) key]).Add(sessionId);
            else
              insensitiveHashtable[(object) key] = (object) new List<string>()
              {
                sessionId
              };
          }
        }
        if (!flag && message is IMChatMessage)
          throw new IMChatMessageException(IMChatMessageExceptionCause.NullSession, (string) null, (string) null, "Null session");
      }
      foreach (string key in (IEnumerable) insensitiveHashtable.Keys)
      {
        List<string> stringList = (List<string>) insensitiveHashtable[(object) key];
        this.sendMessageToRemoteSessions(key, message, stringList.ToArray());
      }
    }

    public void BroadcastMessage(Message message, bool includeRemoteSessions)
    {
      Hashtable hashtable = this.cloneSessions();
      if (message is ServerEventMessage)
      {
        if (message is SessionEventMessage)
        {
          if (message is SessionEventMessage sessionEventMessage)
            EncompassServer.RaiseEvent((ServerEvent) new SessionEvent(sessionEventMessage.EventType, sessionEventMessage.Source));
        }
        else if (message is AppointmentEventMessage && message is AppointmentEventMessage appointmentEventMessage)
        {
          AppointmentEvent appointmentEvent = appointmentEventMessage.AppointmentEvent;
          foreach (string contactUserId in appointmentEventMessage.ContactUserIDs)
          {
            foreach (ISession session in this.GetSessionsForUser(contactUserId))
            {
              try
              {
                session.RaiseEvent((ServerEvent) new AppointmentEvent(session.SessionInfo, appointmentEvent.DataKey, appointmentEvent.Action, appointmentEvent.OwnerID));
              }
              catch
              {
              }
            }
          }
        }
      }
      else
      {
        foreach (IClientSession clientSession in (IEnumerable) hashtable.Values)
          clientSession.SendMessage(message);
      }
      if (!includeRemoteSessions)
        return;
      ArrayList arrayList = new ArrayList();
      foreach (SessionInfo sessionInfo in this.GetAllSessionInfoFromDB())
      {
        if (!hashtable.ContainsKey((object) sessionInfo.SessionID) && !arrayList.Contains((object) sessionInfo.Server.ToString()))
          arrayList.Add((object) sessionInfo.Server.ToString());
      }
      foreach (string serverUri in arrayList)
        this.broadcastMessageOnRemoteServer(serverUri, message);
    }

    public void RaiseServerEventOnRemoveServers(Message message)
    {
      lock (this.localServerIDLock)
      {
        string localServerId = this.getLocalServerID();
        if (!string.IsNullOrWhiteSpace(localServerId))
          this.localServerID = localServerId;
      }
      foreach (string serverUrI in this.getServerURIs(this.localServerID))
        this.broadcastMessageOnRemoteServer(serverUrI, message);
    }

    private string getLocalServerID()
    {
      int num = 3;
      string localServerId = (string) null;
      do
      {
        try
        {
          foreach (string key in (IEnumerable) this.cloneSessions().Keys)
          {
            localServerId = this.getServerID(key);
            if (!string.IsNullOrWhiteSpace(localServerId))
              break;
          }
        }
        catch
        {
        }
        --num;
      }
      while (string.IsNullOrWhiteSpace(localServerId) && num > 0);
      return localServerId;
    }

    private string getServerID(string sessionID)
    {
      sessionID = (sessionID ?? "").Trim();
      using (this.context.MakeCurrent())
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine("select [ServerID] from [Sessions] where [SessionID] = " + SQL.Encode((object) sessionID));
        return dbQueryBuilder.ExecuteScalar() as string;
      }
    }

    private string[] getServerURIs(string excludeServerID)
    {
      string str1 = "";
      if (!string.IsNullOrWhiteSpace(excludeServerID))
        str1 = " where [ServerID] != " + SQL.Encode((object) excludeServerID);
      using (this.context.MakeCurrent())
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine("select [ServerID] from [ServerStatus]" + str1);
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        List<string> stringList = new List<string>();
        for (int index = 0; index < dataRowCollection.Count; ++index)
        {
          string str2 = dataRowCollection[index]["ServerID"] as string;
          dbQueryBuilder.Reset();
          dbQueryBuilder.AppendLine("select top 1 [ServerUri] from [Sessions] where [ServerID] = " + SQL.Encode((object) str2));
          if (dbQueryBuilder.ExecuteScalar() is string str3)
            stringList.Add(str3);
        }
        return stringList.ToArray();
      }
    }

    private void onServerEvent(ClientContext context, ServerEvent e)
    {
      if (context != null && this.context != null && context != this.context)
        return;
      try
      {
        IClientSession[] listeners = this.events.GetListeners(e.GetType());
        if (EventQueueCounters.EventQueue.Value + listeners.Length > ServerGlobals.MaxServerEvents)
        {
          try
          {
            this.context.TraceLog.WriteWarning(nameof (SessionManager), "Max number of server events (" + (object) ServerGlobals.MaxServerEvents + ") reached. Drop event (" + e.GetType().Name + "): " + e.ToString());
          }
          catch (Exception ex)
          {
            this.context.TraceLog.WriteWarning(nameof (SessionManager), "Max number of server events (" + (object) ServerGlobals.MaxServerEvents + ") reached. Drop event (" + e.GetType().Name + ") without event info: " + ex.ToString());
          }
        }
        else
        {
          for (int index = 0; index < listeners.Length; ++index)
          {
            try
            {
              listeners[index].RaiseEvent(e);
            }
            catch
            {
            }
          }
        }
      }
      catch (Exception ex)
      {
        try
        {
          this.context.TraceLog.WriteWarning(nameof (SessionManager), "onServerEvent Exception Handling: " + ex.ToString());
        }
        catch
        {
        }
      }
    }

    private Hashtable cloneSessions()
    {
      lock (this.sessions)
        return (Hashtable) this.sessions.Clone();
    }

    [PgReady]
    private void addSessionToDatabase(ISession session)
    {
      if (this.context.Settings.DbServerType == DbServerType.Postgres)
      {
        using (this.context.MakeCurrent())
        {
          PgDbQueryBuilder pgDbQueryBuilder = new PgDbQueryBuilder(this.context);
          DbTableInfo table = DbAccessManager.GetTable("Sessions");
          DbValueList valueListForSession = this.createValueListForSession(session);
          pgDbQueryBuilder.InsertInto(table, valueListForSession, true, false);
          pgDbQueryBuilder.ExecuteNonQuery();
        }
      }
      else
      {
        using (this.context.MakeCurrent())
        {
          DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
          DbTableInfo table = DbAccessManager.GetTable("Sessions");
          DbValueList valueListForSession = this.createValueListForSession(session);
          dbQueryBuilder.InsertInto(table, valueListForSession, true, false);
          dbQueryBuilder.ExecuteNonQuery();
        }
      }
    }

    private void updateSessionInDatabase(ISession session)
    {
      using (this.context.MakeCurrent())
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        DbTableInfo table = DbAccessManager.GetTable("Sessions");
        DbValueList forSessionUpdate = this.createValueListForSessionUpdate(session);
        DbValue key = new DbValue("SessionID", (object) session.SessionID);
        dbQueryBuilder.Update(table, forSessionUpdate, key);
        dbQueryBuilder.ExecuteNonQuery();
      }
    }

    private DbValueList createValueListForSessionUpdate(ISession session)
    {
      DbValueList forSessionUpdate = new DbValueList();
      forSessionUpdate.Add("SessionType", (object) (int) session.SessionInfo.SessionType);
      forSessionUpdate.Add("ServerID", (object) EncompassServer.ServerID);
      forSessionUpdate.Add("Version", (object) session.DisplayVersion);
      forSessionUpdate.Add("AppName", (object) session.LoginParams.AppName);
      forSessionUpdate.Add("Hostname", (object) session.LoginParams.Hostname);
      forSessionUpdate.Add("UserID", (object) session.LoginParams.UserID);
      Uri uri = this.replaceHost(session.LoginParams.Server.Uri, SessionManager.serverName);
      ServerIdentity serverIdentity = !(uri == (Uri) null) ? new ServerIdentity(uri, session.LoginParams.Server.InstanceName) : new ServerIdentity(session.LoginParams.Server.InstanceName);
      forSessionUpdate.Add("ServerUri", (object) serverIdentity.ToString());
      forSessionUpdate.Add("IPAddress", session.HostAddress != null ? (object) session.HostAddress.ToString() : (object) (string) null);
      return forSessionUpdate;
    }

    private void addServiceSessionToDatabase(string sessionId, string userId, string appName)
    {
      using (this.context.MakeCurrent())
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        DbTableInfo table = DbAccessManager.GetTable("Sessions");
        DbValueList valueListForService = this.createValueListForService(sessionId, userId, appName);
        dbQueryBuilder.InsertInto(table, valueListForService, true, false);
        dbQueryBuilder.ExecuteNonQuery();
      }
    }

    [PgReady]
    private void deleteSessionFromDatabase(string sessionId)
    {
      if (this.context.Settings.DbServerType == DbServerType.Postgres)
      {
        using (this.context.MakeCurrent())
        {
          PgDbQueryBuilder pgDbQueryBuilder = new PgDbQueryBuilder();
          DbTableInfo table = DbAccessManager.GetTable("Sessions");
          DbValue key = new DbValue("SessionID", (object) sessionId);
          pgDbQueryBuilder.DeleteFrom(table, key);
          pgDbQueryBuilder.ExecuteNonQuery();
        }
      }
      else
      {
        using (this.context.MakeCurrent())
        {
          DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
          DbTableInfo table = DbAccessManager.GetTable("Sessions");
          DbValue key = new DbValue("SessionID", (object) sessionId);
          dbQueryBuilder.DeleteFrom(table, key);
          dbQueryBuilder.ExecuteNonQuery();
        }
      }
    }

    private SessionManager.DbSessionInfo dataRowToDbSessionInfo(DataRow row)
    {
      string sessionID = row["SessionID"].ToString();
      SessionType sessionType = SQL.DecodeEnum<SessionType>(row["SessionType"], SessionType.Remoting);
      string userid = row["UserID"].ToString();
      DateTime loginTime = (DateTime) row["LoginTime"];
      string serverID = (string) SQL.Decode(row["ServerID"], (object) null);
      string serverUri = (string) SQL.Decode(row["ServerUri"], (object) null);
      string appName = row["AppName"].ToString();
      string hostname = row["Hostname"].ToString();
      string data = SQL.DecodeString(row["Data"], (string) null);
      string displayVersion = SQL.DecodeString(row["Version"]);
      string ipAddress = SQL.DecodeString(row["IPAddress"]);
      string serverHostname = "";
      DateTime lastAccessed = DateTime.MinValue;
      try
      {
        serverHostname = SQL.DecodeString(row["ServerHostname"]);
        lastAccessed = SQL.DecodeDateTime(row["LastAccessed"], DateTime.MinValue);
      }
      catch
      {
      }
      return new SessionManager.DbSessionInfo(sessionID, sessionType, userid, loginTime, serverID, serverUri, appName, hostname, displayVersion, data, lastAccessed, serverHostname, ipAddress);
    }

    [PgReady]
    private SessionManager.DbSessionInfo getSessionFromDatabase(string sessionId)
    {
      if (this.context.Settings.DbServerType == DbServerType.Postgres)
      {
        using (this.context.MakeCurrent())
        {
          PgDbQueryBuilder pgDbQueryBuilder = new PgDbQueryBuilder();
          pgDbQueryBuilder.AppendLine("select * from Sessions where SessionID = " + SQL.Encode((object) sessionId));
          DataRowCollection dataRowCollection = pgDbQueryBuilder.Execute();
          return dataRowCollection.Count == 0 ? (SessionManager.DbSessionInfo) null : this.dataRowToDbSessionInfo(dataRowCollection[0]);
        }
      }
      else
      {
        using (this.context.MakeCurrent())
        {
          DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
          dbQueryBuilder.AppendLine("select * from Sessions where SessionID = " + SQL.Encode((object) sessionId));
          DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
          return dataRowCollection.Count == 0 ? (SessionManager.DbSessionInfo) null : this.dataRowToDbSessionInfo(dataRowCollection[0]);
        }
      }
    }

    [PgReady]
    private SessionManager.DbSessionInfo[] getAllSessionsFromDatabase()
    {
      if (this.context.Settings.DbServerType == DbServerType.Postgres)
      {
        using (this.context.MakeCurrent())
        {
          PgDbQueryBuilder pgDbQueryBuilder = new PgDbQueryBuilder();
          pgDbQueryBuilder.AppendLine("select SN.*, SS.[Hostname] as [ServerHostname] from [Sessions] as SN left outer join [ServerStatus] as SS on SN.[ServerID] = SS.[ServerID]");
          DataRowCollection dataRowCollection;
          try
          {
            dataRowCollection = pgDbQueryBuilder.Execute();
          }
          catch
          {
            pgDbQueryBuilder.Reset();
            pgDbQueryBuilder.AppendLine("select * from Sessions");
            dataRowCollection = pgDbQueryBuilder.Execute();
          }
          if (dataRowCollection.Count == 0)
            return new SessionManager.DbSessionInfo[0];
          SessionManager.DbSessionInfo[] sessionsFromDatabase = new SessionManager.DbSessionInfo[dataRowCollection.Count];
          for (int index = 0; index < dataRowCollection.Count; ++index)
            sessionsFromDatabase[index] = this.dataRowToDbSessionInfo(dataRowCollection[index]);
          return sessionsFromDatabase;
        }
      }
      else
      {
        using (this.context.MakeCurrent())
        {
          DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
          dbQueryBuilder.AppendLine("select SN.*, SS.[Hostname] as [ServerHostname] from [Sessions] as SN left outer join [ServerStatus] as SS on SN.[ServerID] = SS.[ServerID] where SN.SessionType = 1");
          DataRowCollection dataRowCollection;
          try
          {
            dataRowCollection = dbQueryBuilder.Execute();
          }
          catch
          {
            dbQueryBuilder.Reset();
            dbQueryBuilder.AppendLine("select * from Sessions");
            dataRowCollection = dbQueryBuilder.Execute();
          }
          if (dataRowCollection.Count == 0)
            return new SessionManager.DbSessionInfo[0];
          SessionManager.DbSessionInfo[] sessionsFromDatabase = new SessionManager.DbSessionInfo[dataRowCollection.Count];
          for (int index = 0; index < dataRowCollection.Count; ++index)
            sessionsFromDatabase[index] = this.dataRowToDbSessionInfo(dataRowCollection[index]);
          return sessionsFromDatabase;
        }
      }
    }

    [PgReady]
    private SessionManager.DbSessionInfo getSessionFromDatabase(
      string sessionId,
      bool updateActivityTime)
    {
      if (this.context.Settings.DbServerType == DbServerType.Postgres)
      {
        using (this.context.MakeCurrent())
        {
          string text = string.Format("where (([SessionID] = {0}) AND ((SessionType = 2 AND DateDiff(minute, LastAccessed, GetDate()) <= {1}) OR (SessionType = 1)))", (object) SQL.Encode((object) sessionId), (object) SessionManager.sessionTimeoutMinutes);
          PgDbQueryBuilder pgDbQueryBuilder = new PgDbQueryBuilder();
          pgDbQueryBuilder.AppendLine("select *");
          pgDbQueryBuilder.AppendLine("from Sessions");
          pgDbQueryBuilder.AppendLine(text);
          pgDbQueryBuilder.AppendLine(";");
          if (updateActivityTime)
          {
            pgDbQueryBuilder.AppendLine("update Sessions");
            pgDbQueryBuilder.AppendLine("set [LastAccessed] = GetDate()");
            pgDbQueryBuilder.AppendLine(text);
            pgDbQueryBuilder.AppendLine(";");
          }
          DataRowCollection dataRowCollection = pgDbQueryBuilder.Execute();
          return dataRowCollection.Count == 0 ? (SessionManager.DbSessionInfo) null : this.dataRowToDbSessionInfo(dataRowCollection[0]);
        }
      }
      else
      {
        string text = string.Format("where (([SessionID] = {0}) AND ((SessionType = 2 AND DateDiff(minute, LastAccessed, GetDate()) <= {1}) OR (SessionType = 1)))", (object) SQL.Encode((object) sessionId), (object) SessionManager.sessionTimeoutMinutes);
        SessionManager.DbSessionInfo sessionFromDatabase = (SessionManager.DbSessionInfo) null;
        using (this.context.MakeCurrent())
        {
          DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
          dbQueryBuilder.AppendLine("select *");
          dbQueryBuilder.AppendLine("from Sessions");
          dbQueryBuilder.AppendLine(text);
          dbQueryBuilder.AppendLine(";");
          DataRowCollection dataRowCollection = dbQueryBuilder.Execute(DbTransactionType.None);
          if (dataRowCollection == null || dataRowCollection.Count <= 0)
            return (SessionManager.DbSessionInfo) null;
          sessionFromDatabase = this.dataRowToDbSessionInfo(dataRowCollection[0]);
        }
        if (updateActivityTime && sessionFromDatabase != null && (sessionFromDatabase.LastAccessed == DateTime.MinValue || sessionFromDatabase.LastAccessed < DateTime.Now.AddSeconds(-1.0)))
          BackgroundActionRunner.EnqueueAction("UpdateSessionActivityTime-" + sessionId, this.context, true, (Action<IClientContext>) (ctx =>
          {
            DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
            dbQueryBuilder.AppendLine("SET NOCOUNT ON");
            dbQueryBuilder.AppendLine("BEGIN TRY");
            dbQueryBuilder.AppendLine("UPDATE Sessions WITH (NOWAIT)");
            dbQueryBuilder.AppendLine("SET [LastAccessed] = GETDATE()");
            dbQueryBuilder.AppendLine(string.Format("WHERE [SessionID] = {0}", (object) SQL.Encode((object) sessionId)));
            dbQueryBuilder.AppendLine("AND [LastAccessed] < DATEADD(ss, -1, GETDATE())");
            dbQueryBuilder.AppendLine("END TRY");
            dbQueryBuilder.AppendLine("BEGIN CATCH");
            dbQueryBuilder.AppendLine("IF ERROR_MESSAGE() <> 'Lock request time out period exceeded.'");
            dbQueryBuilder.AppendLine("BEGIN");
            dbQueryBuilder.AppendLine("DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT");
            dbQueryBuilder.AppendLine("SELECT @ErrorMessage = ERROR_MESSAGE(), @ErrorSeverity = ERROR_SEVERITY(), @ErrorState = ERROR_STATE()");
            dbQueryBuilder.AppendLine("RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState)");
            dbQueryBuilder.AppendLine("END");
            dbQueryBuilder.AppendLine("END CATCH");
            dbQueryBuilder.ExecuteNonQuery();
          }));
        return sessionFromDatabase;
      }
    }

    private DbValueList createValueListForSession(ISession session)
    {
      DbValueList valueListForSession = new DbValueList();
      valueListForSession.Add("SessionID", (object) session.SessionID);
      valueListForSession.Add("SessionType", (object) (int) session.SessionInfo.SessionType);
      valueListForSession.Add("UserID", (object) session.UserID);
      valueListForSession.Add("LoginTime", (object) session.LoginTime);
      valueListForSession.Add("ServerID", (object) EncompassServer.ServerID);
      valueListForSession.Add("Version", (object) session.DisplayVersion);
      Uri uri = this.replaceHost(session.LoginParams.Server.Uri, SessionManager.serverName);
      ServerIdentity serverIdentity = !(uri == (Uri) null) ? new ServerIdentity(uri, session.LoginParams.Server.InstanceName) : new ServerIdentity(session.LoginParams.Server.InstanceName);
      valueListForSession.Add("ServerUri", (object) serverIdentity.ToString());
      valueListForSession.Add("AppName", (object) session.LoginParams.AppName);
      valueListForSession.Add("Hostname", (object) session.LoginParams.Hostname);
      valueListForSession.Add("IPAddress", session.HostAddress == null ? (object) (string) null : (object) session.HostAddress.ToString());
      return valueListForSession;
    }

    private DbValueList createValueListForService(string sessionId, string userId, string appName)
    {
      return new DbValueList()
      {
        {
          "SessionID",
          (object) sessionId
        },
        {
          "SessionType",
          (object) SessionType.Service
        },
        {
          "UserID",
          (object) userId
        },
        {
          "LoginTime",
          (object) DateTime.Now
        },
        {
          "ServerID",
          (object) null
        },
        {
          "ServerUri",
          (object) null
        },
        {
          "Version",
          (object) VersionInformation.CurrentVersion.DisplayVersion
        },
        {
          "AppName",
          (object) appName
        },
        {
          "Hostname",
          (object) ""
        },
        {
          "LastAccessed",
          (object) DateTime.Now
        }
      };
    }

    private Uri replaceHost(Uri uri, string host)
    {
      if (uri == (Uri) null)
        return (Uri) null;
      string uriString = uri.Scheme + "://" + host;
      if (uriString.ToLower().StartsWith("tcp://") || !uri.IsDefaultPort)
        uriString = uriString + ":" + (object) uri.Port;
      if (uri.AbsolutePath.ToLower() != "/encompass" && uri.AbsolutePath.ToLower() != "/encompass/")
        uriString += uri.AbsolutePath;
      Uri uri1 = new Uri(uriString);
      if (!uri1.AbsoluteUri.EndsWith("/"))
        uri1 = new Uri(uri1.AbsoluteUri + "/");
      return uri1;
    }

    [PgReady]
    private Dictionary<string, SessionInfo.BasicLoanInfo> getSessionLockInfo(bool sameServerOnly)
    {
      if (this.context.Settings.DbServerType == DbServerType.Postgres)
      {
        using (this.context.MakeCurrent())
        {
          Dictionary<string, SessionInfo.BasicLoanInfo> sessionLockInfo = new Dictionary<string, SessionInfo.BasicLoanInfo>();
          if (this.context.AllowConcurrentEditing)
            return sessionLockInfo;
          PgDbQueryBuilder pgDbQueryBuilder = new PgDbQueryBuilder();
          pgDbQueryBuilder.AppendLine("select Loan.Guid, Loan.LoanNumber, Loan.BorrowerLastName, Loan.BorrowerFirstName, Sessions.SessionID");
          pgDbQueryBuilder.AppendLine("from LoanSummary as Loan");
          pgDbQueryBuilder.AppendLine("   inner join LoanLock on Loan.Guid = LoanLock.guid");
          pgDbQueryBuilder.AppendLine("   inner join Sessions on LoanLock.loginSessionID = Sessions.SessionID");
          pgDbQueryBuilder.AppendLine("where LoanLock.lockedfor = " + (object) 1);
          if (sameServerOnly)
            pgDbQueryBuilder.AppendLine("   and Sessions.ServerID = " + SQL.Encode((object) EncompassServer.ServerID));
          pgDbQueryBuilder.AppendLine("order by LoanLock.locktime");
          foreach (DataRow dataRow in (InternalDataCollectionBase) pgDbQueryBuilder.Execute())
          {
            string key = SQL.DecodeString(dataRow["SessionID"]);
            if (!sessionLockInfo.ContainsKey(key))
            {
              SessionInfo.BasicLoanInfo basicLoanInfo = new SessionInfo.BasicLoanInfo(dataRow["Guid"].ToString(), string.Concat(dataRow["BorrowerLastName"]), string.Concat(dataRow["BorrowerFirstName"]), string.Concat(dataRow["LoanNumber"]));
              sessionLockInfo.Add(key, basicLoanInfo);
            }
          }
          return sessionLockInfo;
        }
      }
      else
      {
        using (this.context.MakeCurrent())
        {
          Dictionary<string, SessionInfo.BasicLoanInfo> sessionLockInfo = new Dictionary<string, SessionInfo.BasicLoanInfo>();
          if (this.context.AllowConcurrentEditing)
            return sessionLockInfo;
          DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
          dbQueryBuilder.AppendLine("select Loan.Guid, Loan.LoanNumber, Loan.BorrowerLastName, Loan.BorrowerFirstName, Sessions.SessionID");
          dbQueryBuilder.AppendLine("from LoanSummary as Loan");
          dbQueryBuilder.AppendLine("   inner join LoanLock on Loan.Guid = LoanLock.guid");
          dbQueryBuilder.AppendLine("   inner join Sessions on LoanLock.loginSessionID = Sessions.SessionID");
          dbQueryBuilder.AppendLine("where LoanLock.lockedfor = " + (object) 1);
          if (sameServerOnly)
            dbQueryBuilder.AppendLine("   and Sessions.ServerID = " + SQL.Encode((object) EncompassServer.ServerID));
          dbQueryBuilder.AppendLine("order by LoanLock.locktime");
          foreach (DataRow dataRow in (InternalDataCollectionBase) dbQueryBuilder.Execute())
          {
            string key = SQL.DecodeString(dataRow["SessionID"]);
            if (!sessionLockInfo.ContainsKey(key))
            {
              SessionInfo.BasicLoanInfo basicLoanInfo = new SessionInfo.BasicLoanInfo(dataRow["Guid"].ToString(), string.Concat(dataRow["BorrowerLastName"]), string.Concat(dataRow["BorrowerFirstName"]), string.Concat(dataRow["LoanNumber"]));
              sessionLockInfo.Add(key, basicLoanInfo);
            }
          }
          return sessionLockInfo;
        }
      }
    }

    [PgReady]
    public void ClearExpiredServiceSessionLocksFromDatabase(Guid loanId)
    {
      if (this.context.Settings.DbServerType == DbServerType.Postgres)
      {
        using (this.context.MakeCurrent())
        {
          PgDbQueryBuilder pgDbQueryBuilder = new PgDbQueryBuilder();
          pgDbQueryBuilder.AppendLineFormat("DELETE FROM LoanLock WHERE guid = {0} AND loginSessionID IN (SELECT SessionId FROM Sessions WHERE DATEDIFF(mi,LastAccessed,GetDate()) >= {1} AND SessionType = {2});", (object) SQL.Encode((object) LoanData.GuidToString(loanId)), (object) 16, (object) 2);
          pgDbQueryBuilder.AppendLineFormat("DELETE FROM LoanLock WHERE guid = {0} AND IsSessionLess = '1' AND DATEDIFF(mi,lockTime,GetDate()) >= {1};", (object) SQL.Encode((object) LoanData.GuidToString(loanId)), (object) 16);
          pgDbQueryBuilder.ExecuteNonQuery();
        }
      }
      else
      {
        using (this.context.MakeCurrent())
        {
          DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
          dbQueryBuilder.AppendLineFormat("DELETE FROM LoanLock WHERE guid = {0} AND loginSessionID IN (SELECT SessionId FROM Sessions WHERE DATEDIFF(mi,LastAccessed,GetDate()) >= {1} AND SessionType = {2});", (object) SQL.Encode((object) LoanData.GuidToString(loanId)), (object) 16, (object) 2);
          dbQueryBuilder.AppendLineFormat("DELETE FROM LoanLock WHERE guid = {0} AND IsSessionLess = '1' AND DATEDIFF(mi,lockTime,GetDate()) >= {1};", (object) SQL.Encode((object) LoanData.GuidToString(loanId)), (object) 16);
          dbQueryBuilder.ExecuteNonQuery();
        }
      }
    }

    [PgReady]
    private void deleteLocksFromDatabase(string sessionId)
    {
      if (this.context.Settings.DbServerType == DbServerType.Postgres)
      {
        using (this.context.MakeCurrent())
        {
          PgDbQueryBuilder pgDbQueryBuilder = new PgDbQueryBuilder();
          DbTableInfo table = DbAccessManager.GetTable("LoanLock");
          DbValue key = new DbValue("loginSessionID", (object) sessionId);
          pgDbQueryBuilder.DeleteFrom(table, key);
          pgDbQueryBuilder.ExecuteNonQuery();
        }
      }
      else
      {
        using (this.context.MakeCurrent())
        {
          DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
          DbTableInfo table = DbAccessManager.GetTable("LoanLock");
          DbValue key = new DbValue("loginSessionID", (object) sessionId);
          dbQueryBuilder.DeleteFrom(table, key);
          dbQueryBuilder.ExecuteNonQuery();
        }
      }
    }

    public void ClearExpiredServiceSessionsAndLocks()
    {
      using (this.context.MakeCurrent())
      {
        DbQueryBuilder sql = new DbQueryBuilder();
        try
        {
          sql.Reset();
          sql.AppendLine("IF(datepart(mi, GETDATE()) = 5) and(datepart(HH, GETDATE()) = 20)");
          sql.AppendLine("BEGIN");
          sql.AppendLine("DELETE lck");
          sql.AppendLine("FROM LoanLock lck");
          sql.AppendLine("WHERE lockedfor = 0 AND loginSessionID = '' AND InUse = 'N'");
          sql.AppendLine("END");
          sql.AppendLine("IF(datepart(mi, GETDATE()) = 30)");
          sql.AppendLine("BEGIN");
          sql.AppendLine("DELETE lck");
          sql.AppendLine("FROM LoanLock lck");
          sql.AppendLine("LEFT OUTER JOIN Sessions ssn ON ssn.SessionID = lck.loginSessionID");
          sql.AppendLine("WHERE lck.loginSessionID <> '' and COALESCE(ssn.SessionID, '') = ''");
          sql.AppendLine("AND locktime < DATEADD(mi, -5, GETDATE())");
          sql.AppendLine("END");
          sql.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
          TraceLog.WriteError(nameof (SessionManager), "ClearExpiredServiceSessionsAndLocks : exception : " + ex.StackTrace);
        }
        sql.Reset();
        sql.AppendLine("DECLARE @expirationTime DATETIME");
        sql.AppendLineFormat("SET @expirationTime = DATEADD(mi, -{0}, GETDATE())", (object) 15);
        sql.AppendLineFormat("SELECT SessionId FROM Sessions WHERE LastAccessed < @expirationTime AND SessionType = {0};", (object) 2);
        DataRowCollection dataRowCollection = sql.Execute(DbTransactionType.None);
        sql.Reset();
        int num = 0;
        foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
        {
          ++num;
          string str = dataRow["SessionId"].ToString();
          if (num == 1)
            sql.AppendLine("DECLARE @LoanIdsForLoanLocks TABLE (LoanId varchar(38));");
          sql.AppendLineFormat("DELETE FROM SessionCaches WHERE SessionID = {0};", (object) SQL.Encode((object) str));
          sql.AppendLineFormat("DELETE FROM LoanLock output deleted.guid into @LoanIdsForLoanLocks WHERE loginSessionID = {0};", (object) SQL.Encode((object) str));
          sql.AppendLineFormat("DELETE FROM Sessions WHERE SessionID = {0};", (object) SQL.Encode((object) str));
          if (num == 10)
          {
            SessionManager.ExecuteAndSendKafka(sql);
            sql.Reset();
            num = 0;
          }
        }
        if (sql.Length > 0)
        {
          SessionManager.ExecuteAndSendKafka(sql);
          sql.ExecuteNonQuery();
        }
        sql.Reset();
        for (int index = 0; index < 10; ++index)
        {
          sql.AppendLineFormat("DECLARE @expirationTime DATETIME = DATEADD(mi, -{0}, GETDATE())", (object) 1);
          sql.AppendLine("DECLARE @rowsAffected INT = 1");
          sql.AppendLine("DELETE TOP (50) sll from LoanLock sll WHERE IsSessionLess = '1' AND lockTime < @expirationTime;");
          sql.AppendLine("SELECT @@ROWCOUNT RowsAffected");
          sql.AppendLine("");
          DataRow dataRow = sql.ExecuteRowQuery();
          if (dataRow != null && Convert.ToInt32(dataRow["RowsAffected"]) <= 0)
            break;
          sql.Reset();
        }
      }
    }

    private static void ExecuteAndSendKafka(DbQueryBuilder sql)
    {
      sql.AppendLine("select distinct LoanId from  @LoanIdsForLoanLocks r left join loanLock on LoanLock.guid = r.LoanId and IsSessionLess = 0 where LoanLock.guid is null");
      try
      {
        DataSet dataSet = sql.ExecuteSetQuery();
        if (dataSet == null || dataSet.Tables.Count <= 0 || dataSet.Tables[0].Rows.Count <= 0)
          return;
        foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[0].Rows)
          SessionManager.SendKafkaEventForWebhook(row["LoanId"].ToString());
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (SessionManager), string.Format("Error in executing the loan lock scheduler. Stack={0}", (object) ex));
      }
    }

    private static void SendKafkaEventForWebhook(string loanId)
    {
      try
      {
        ClientContext current = ClientContext.GetCurrent();
        UnlockAllEvent queueEvent = new UnlockAllEvent("serviceId", current.InstanceName, (string) null, loanId, "<system>", EncompassServer.ServerMode != EncompassServerMode.Service ? Enums.Source.URN_ELLI_SERVICE_ENCOMPASS : Enums.Source.URN_ELLI_SERVICE_EBS, DateTime.UtcNow);
        queueEvent.AddLockUnlockKafkaMessage(loanId, ClientContext.CurrentRequest.CorrelationId, "<system>", current.ClientID, current.InstanceName, EncompassServer.ServerMode != EncompassServerMode.Service, "unlockall");
        if (queueEvent.QueueMessages.Count <= 0)
          return;
        IMessageQueueEventService queueEventService = (IMessageQueueEventService) new MessageQueueEventService();
        IMessageQueueProcessor processor = (IMessageQueueProcessor) new KafkaProcessor();
        queueEventService.MessageQueueProducer((MessageQueueEvent) queueEvent, processor);
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (SessionManager), string.Format("Failed to publish unlockAll event. Loan GUID={0} Stack={1}", (object) loanId, (object) ex));
      }
    }

    internal class DbSessionInfo
    {
      internal readonly string SessionID;
      internal readonly SessionType SessionType;
      internal readonly string Userid;
      internal readonly DateTime LoginTime = DateTime.MaxValue;
      internal readonly string ServerID;
      internal readonly ServerIdentity ServerInfo;
      internal readonly string AppName;
      internal readonly string Hostname;
      internal readonly string DisplayVersion;
      internal readonly string Data;
      internal readonly string ServerHostname = "";
      internal readonly string IPAddress = "";
      internal readonly DateTime LastAccessed;

      internal DbSessionInfo(
        string sessionID,
        SessionType sessionType,
        string userid,
        DateTime loginTime,
        string serverID,
        string serverUri,
        string appName,
        string hostname,
        string displayVersion,
        string data,
        DateTime lastAccessed,
        string serverHostname = "�",
        string ipAddress = "�")
      {
        this.SessionID = sessionID;
        this.SessionType = sessionType;
        this.Userid = userid;
        this.LoginTime = loginTime;
        this.ServerID = serverID;
        this.DisplayVersion = displayVersion;
        if ((serverUri ?? "").Trim() != "")
        {
          try
          {
            this.ServerInfo = ServerIdentity.Parse(serverUri);
          }
          catch
          {
          }
        }
        this.AppName = appName;
        this.Hostname = hostname;
        this.Data = data;
        this.ServerHostname = serverHostname;
        this.IPAddress = ipAddress;
        this.LastAccessed = lastAccessed;
      }
    }
  }
}
