// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.SessionObjects.ServerManager
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using Elli.Server.Remoting.Installer;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Calendar;
using EllieMae.EMLite.ClientServer.Events;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Server;
using EllieMae.EMLite.Server.ServerCommon;
using Encompass.Diagnostics;
using Encompass.Diagnostics.Logging.Targets;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

#nullable disable
namespace Elli.Server.Remoting.SessionObjects
{
  public class ServerManager : SessionBoundObject, IServerManager, IRemotingLogConsumer
  {
    private const string className = "ServerManager";
    private string sw = Tracing.SwVersionControl;
    private Dictionary<string, object> serverAttributes;
    private static string serverDllVersion;

    public ServerManager Initialize(ISession session)
    {
      this.InitializeInternal(session, nameof (ServerManager).ToLower());
      return this;
    }

    public virtual SessionInfo[] GetAllSessionInfo(bool fromDatabase)
    {
      this.onApiCalled(nameof (ServerManager), nameof (GetAllSessionInfo), Array.Empty<object>());
      try
      {
        return fromDatabase ? this.Session.Context.Sessions.GetAllSessionInfoFromDB() : this.Session.Context.Sessions.GetAllSessionInfo();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ServerManager), ex, this.Session.SessionInfo);
        return (SessionInfo[]) null;
      }
    }

    public virtual SessionInfo GetSessionInfo(string sessionId)
    {
      this.onApiCalled(nameof (ServerManager), nameof (GetSessionInfo), new object[1]
      {
        (object) sessionId
      });
      if ((sessionId ?? "") == "")
        Err.Raise(TraceLevel.Warning, nameof (ServerManager), (ServerException) new ServerArgumentException("Session ID cannot be blank or null", nameof (sessionId), this.Session.SessionInfo));
      try
      {
        return this.Session.Context.Sessions.GetSessionInfo(sessionId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ServerManager), ex, this.Session.SessionInfo);
        return (SessionInfo) null;
      }
    }

    public virtual KeyValuePair<string, string>[] GetAllUserSessionIds(string userId)
    {
      this.onApiCalled(nameof (ServerManager), nameof (GetAllUserSessionIds), Array.Empty<object>());
      try
      {
        return this.Session.Context.Sessions.GetSessionIDAndServerIDForUser(userId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ServerManager), ex, this.Session.SessionInfo);
        return (KeyValuePair<string, string>[]) null;
      }
    }

    public virtual bool LoginsEnabled
    {
      get
      {
        this.onApiCalled(nameof (ServerManager), "LoginsEnabled_get", Array.Empty<object>());
        try
        {
          return (bool) this.Session.Context.Settings.GetServerSetting("Login.Enabled");
        }
        catch (Exception ex)
        {
          Err.Reraise(nameof (ServerManager), ex, this.Session.SessionInfo);
          return false;
        }
      }
      set
      {
        this.onApiCalled(nameof (ServerManager), "LoginsEnabled_set", new object[1]
        {
          (object) value
        });
        try
        {
          this.Security.DemandRootAdministrator();
          this.Session.Context.Settings.SetServerSetting("Login.Enabled", (object) value);
        }
        catch (Exception ex)
        {
          Err.Reraise(nameof (ServerManager), ex, this.Session.SessionInfo);
        }
      }
    }

    public virtual bool LoginsEnabledWithForPersonna
    {
      get
      {
        this.onApiCalled(nameof (ServerManager), "LoginsEnabled_get", Array.Empty<object>());
        try
        {
          return (bool) this.Session.Context.Settings.GetServerSetting("Login.Enabled");
        }
        catch (Exception ex)
        {
          Err.Reraise(nameof (ServerManager), ex, this.Session.SessionInfo);
          return false;
        }
      }
      set
      {
        this.onApiCalled(nameof (ServerManager), "LoginsEnabled_set", new object[1]
        {
          (object) value
        });
        try
        {
          this.Security.DemandFeatureAccess(AclFeature.SettingsTab_Company_DisableLogins);
          this.Session.Context.Settings.SetServerSetting("Login.Enabled", (object) value);
        }
        catch (Exception ex)
        {
          Err.Reraise(nameof (ServerManager), ex, this.Session.SessionInfo);
        }
      }
    }

    public virtual TraceLevel TraceLevel
    {
      get
      {
        this.onApiCalled(nameof (ServerManager), "TraceLevel_get", Array.Empty<object>());
        try
        {
          return TraceLog.TraceLevel;
        }
        catch (Exception ex)
        {
          Err.Reraise(nameof (ServerManager), ex, this.Session.SessionInfo);
          return TraceLevel.Off;
        }
      }
      set
      {
        this.onApiCalled(nameof (ServerManager), "TraceLevel_set", new object[1]
        {
          (object) value
        });
        try
        {
          this.Security.DemandRootAdministrator();
          TraceLog.TraceLevel = value;
        }
        catch (Exception ex)
        {
          Err.Reraise(nameof (ServerManager), ex, this.Session.SessionInfo);
        }
      }
    }

    public virtual void TerminateSession(
      string sessionId,
      bool forceDisconnect,
      bool includeRemoteSession)
    {
      this.onApiCalled(nameof (ServerManager), nameof (TerminateSession), new object[2]
      {
        (object) sessionId,
        (object) forceDisconnect
      });
      if ((sessionId ?? "") == "")
        Err.Raise(TraceLevel.Warning, nameof (ServerManager), (ServerException) new ServerArgumentException("Session ID cannot be blank or null", nameof (sessionId), this.Session.SessionInfo));
      try
      {
        this.Session.Context.Sessions.TerminateSession(sessionId, forceDisconnect, includeRemoteSession);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ServerManager), ex, this.Session.SessionInfo);
      }
    }

    public void AddServiceSession(string sessionId, string appName)
    {
      this.onApiCalled(nameof (ServerManager), nameof (AddServiceSession), new object[2]
      {
        (object) sessionId,
        (object) new object[2]
        {
          (object) sessionId,
          (object) appName
        }
      });
      try
      {
        this.Session.Context.Sessions.AddServiceSession(sessionId, this.Session.UserID, appName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ServerManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void TerminateSessions(
      string[] sessionIds,
      bool forceDisconnect,
      bool includeRemoteSessions)
    {
      this.onApiCalled(nameof (ServerManager), nameof (TerminateSessions), new object[2]
      {
        (object) sessionIds,
        (object) forceDisconnect
      });
      if (sessionIds == null)
        Err.Raise(TraceLevel.Warning, nameof (ServerManager), (ServerException) new ServerArgumentException("Session ID list cannot be null", nameof (sessionIds), this.Session.SessionInfo));
      try
      {
        this.Session.Context.Sessions.TerminateSessions(sessionIds, forceDisconnect, includeRemoteSessions);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ServerManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void TerminateAllSessions(bool forceDisconnects, bool includeRemoteSessions)
    {
      this.onApiCalled(nameof (ServerManager), nameof (TerminateAllSessions), new object[1]
      {
        (object) forceDisconnects
      });
      try
      {
        this.Security.DemandAdministrator();
        this.Session.Context.Sessions.TerminateAllSessions(forceDisconnects ? DisconnectEventArgument.Force : DisconnectEventArgument.Nonforce, includeRemoteSessions);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ServerManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void SendMessage(Message message)
    {
      this.onApiCalled(nameof (ServerManager), nameof (SendMessage), new object[1]
      {
        (object) message
      });
      KeyValuePair<string, string>[] andServerIdForUser;
      switch (message)
      {
        case IMControlMessage _:
          IMControlMessage msg = (IMControlMessage) message;
          if (msg.MsgType == IMMessage.MessageType.AllowAddToList)
          {
            IMDBAccessor.DeleteAddToListRequest(msg.ToUser, msg.FromUser);
            return;
          }
          if (msg.MsgType == IMMessage.MessageType.DenyAck)
          {
            IMDBAccessor.DeleteDenyAddToList(msg.ToUser, msg.FromUser);
            return;
          }
          if (msg.MsgType == IMMessage.MessageType.RequestAddToList)
            IMDBAccessor.InsertToIMMessages(msg);
          else if (msg.MsgType == IMMessage.MessageType.DenyAddToList)
            IMDBAccessor.ProcessDenyAddToListRequest(msg);
          andServerIdForUser = this.Session.Context.Sessions.GetSessionIDAndServerIDForUser(msg.ToUser);
          break;
        case CSControlMessage _:
          CSControlMessage csControlMessage = (CSControlMessage) message;
          if (csControlMessage.MsgType == CSMessage.MessageType.RequestReadOnlyCalendarAccess)
            CSDBAccessor.SaveCSControlMessage(csControlMessage.FromUser, csControlMessage.ToUser, csControlMessage.MsgType, csControlMessage.Text);
          else if (csControlMessage.MsgType == CSMessage.MessageType.RequestPartialCalendarAccess)
            CSDBAccessor.SaveCSControlMessage(csControlMessage.FromUser, csControlMessage.ToUser, csControlMessage.MsgType, csControlMessage.Text);
          else if (csControlMessage.MsgType == CSMessage.MessageType.RequestFullCalendarAccess)
            CSDBAccessor.SaveCSControlMessage(csControlMessage.FromUser, csControlMessage.ToUser, csControlMessage.MsgType, csControlMessage.Text);
          else if (csControlMessage.MsgType == CSMessage.MessageType.AllowAddToList)
          {
            CSDBAccessor.DeleteCSMessage(csControlMessage.ToUser, csControlMessage.FromUser, new CSMessage.MessageType[3]
            {
              CSMessage.MessageType.RequestFullCalendarAccess,
              CSMessage.MessageType.RequestPartialCalendarAccess,
              CSMessage.MessageType.RequestReadOnlyCalendarAccess
            });
            CSDBAccessor.SaveCSControlMessage(csControlMessage.FromUser, csControlMessage.ToUser, csControlMessage.MsgType, csControlMessage.Text);
          }
          else if (csControlMessage.MsgType == CSMessage.MessageType.DenyAddToList)
          {
            CSDBAccessor.DeleteCSMessage(csControlMessage.ToUser, csControlMessage.FromUser, new CSMessage.MessageType[3]
            {
              CSMessage.MessageType.RequestFullCalendarAccess,
              CSMessage.MessageType.RequestPartialCalendarAccess,
              CSMessage.MessageType.RequestReadOnlyCalendarAccess
            });
            CSDBAccessor.SaveCSControlMessage(csControlMessage.FromUser, csControlMessage.ToUser, csControlMessage.MsgType, csControlMessage.Text);
          }
          else
          {
            if (csControlMessage.MsgType == CSMessage.MessageType.DenyAck)
            {
              CSDBAccessor.DeleteCSMessage(csControlMessage.ToUser, csControlMessage.FromUser, new CSMessage.MessageType[1]
              {
                CSMessage.MessageType.DenyAddToList
              });
              return;
            }
            if (csControlMessage.MsgType == CSMessage.MessageType.AcceptAck)
            {
              CSDBAccessor.DeleteCSMessage(csControlMessage.ToUser, csControlMessage.FromUser, new CSMessage.MessageType[1]
              {
                CSMessage.MessageType.AllowAddToList
              });
              return;
            }
            if (csControlMessage.MsgType == CSMessage.MessageType.ModifyAccess)
              CSDBAccessor.SaveCSControlMessage(csControlMessage.FromUser, csControlMessage.ToUser, csControlMessage.MsgType, csControlMessage.Text);
            else if (csControlMessage.MsgType == CSMessage.MessageType.DeleteAccess)
            {
              CSDBAccessor.SaveCSControlMessage(csControlMessage.FromUser, csControlMessage.ToUser, csControlMessage.MsgType, csControlMessage.Text);
            }
            else
            {
              if (csControlMessage.MsgType == CSMessage.MessageType.ModifyAck)
              {
                CSDBAccessor.DeleteCSMessage(csControlMessage.ToUser, csControlMessage.FromUser, new CSMessage.MessageType[3]
                {
                  CSMessage.MessageType.ModifyAccess,
                  CSMessage.MessageType.DeleteAccess,
                  CSMessage.MessageType.WithdrawAccess
                });
                return;
              }
              if (csControlMessage.MsgType == CSMessage.MessageType.WithdrawAccess)
                CSDBAccessor.SaveCSControlMessage(csControlMessage.FromUser, csControlMessage.ToUser, csControlMessage.MsgType, csControlMessage.Text);
            }
          }
          andServerIdForUser = this.Session.Context.Sessions.GetSessionIDAndServerIDForUser(csControlMessage.ToUser);
          break;
        default:
          return;
      }
      if (andServerIdForUser == null || andServerIdForUser.Length == 0)
        return;
      this.Session.Context.Sessions.SendMessage(message, andServerIdForUser[0].Key, true);
    }

    public virtual void SendMessage(Message message, string sessionId, bool includeRemoteSession)
    {
      if (message == null)
        Err.Raise(TraceLevel.Warning, nameof (ServerManager), (ServerException) new ServerArgumentException("Message cannot be null", nameof (message), this.Session.SessionInfo));
      if ((sessionId ?? "").Trim() == "")
        Err.Raise(TraceLevel.Warning, nameof (ServerManager), (ServerException) new ServerArgumentException("Session ID cannot be null or empty", nameof (sessionId), this.Session.SessionInfo));
      this.SendMessage(message, new string[1]{ sessionId }, (includeRemoteSession ? 1 : 0) != 0);
    }

    public virtual void SendMessage(
      Message message,
      string[] sessionIds,
      bool includeRemoteSessions)
    {
      this.onApiCalled(nameof (ServerManager), nameof (SendMessage), new object[2]
      {
        (object) message,
        (object) sessionIds
      });
      if (message == null)
        Err.Raise(TraceLevel.Warning, nameof (ServerManager), (ServerException) new ServerArgumentException("Message cannot be null", nameof (message), this.Session.SessionInfo));
      if (sessionIds == null)
        Err.Raise(TraceLevel.Warning, nameof (ServerManager), (ServerException) new ServerArgumentException("Session ID list cannot be null", nameof (sessionIds), this.Session.SessionInfo));
      try
      {
        Message message1 = message;
        if (!(message is IMControlMessage) && !(message is CSControlMessage) && message.Source == null)
          message1 = message.Clone(this.Session.SessionInfo);
        this.Session.Context.Sessions.SendMessage(message1, sessionIds, includeRemoteSessions);
      }
      catch (IMChatMessageException ex)
      {
        throw ex;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ServerManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void BroadcastMessage(Message message, bool includeRemoteSessions)
    {
      this.onApiCalled(nameof (ServerManager), nameof (BroadcastMessage), new object[1]
      {
        (object) message
      });
      if (message == null)
        Err.Raise(TraceLevel.Warning, nameof (ServerManager), (ServerException) new ServerArgumentException("Message cannot be null", nameof (message), this.Session.SessionInfo));
      try
      {
        this.Session.Context.Sessions.BroadcastMessage(message is AppointmentEventMessage || message.Source != null ? message : message.Clone(this.Session.SessionInfo), includeRemoteSessions);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ServerManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void SendNotification(UserNotification notification)
    {
      this.onApiCalled(nameof (ServerManager), nameof (SendNotification), new object[1]
      {
        (object) notification
      });
      try
      {
        UserNotifications.Send(notification);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ServerManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void SendNotifications(UserNotification[] notifications)
    {
      this.onApiCalled(nameof (ServerManager), nameof (SendNotifications), (object[]) notifications);
      try
      {
        foreach (UserNotification notification in notifications)
          UserNotifications.Send(notification);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ServerManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual string GetTPOAdminSiteUrlFromDS()
    {
      this.onApiCalled(nameof (ServerManager), "GetTPOAdminUrlFromDS", Array.Empty<object>());
      try
      {
        return this.Session.Context.Settings.TPOAdminSiteUrl;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ServerManager), ex, this.Session.SessionInfo);
        return (string) null;
      }
    }

    public virtual IDictionary GetServerSettings()
    {
      this.onApiCalled(nameof (ServerManager), nameof (GetServerSettings), Array.Empty<object>());
      try
      {
        return this.Session.Context.Settings.GetServerSettings();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ServerManager), ex, this.Session.SessionInfo);
        return (IDictionary) null;
      }
    }

    public virtual IDictionary GetServerSettings(string category)
    {
      return this.GetServerSettings(category, true);
    }

    public virtual IDictionary GetServerSettings(string category, bool checkDefinition)
    {
      this.onApiCalled(nameof (ServerManager), nameof (GetServerSettings), new object[1]
      {
        (object) category
      });
      if ((category ?? "") == null)
        Err.Raise(TraceLevel.Warning, nameof (ServerManager), (ServerException) new ServerArgumentException("Setting category cannot be null", nameof (category), this.Session.SessionInfo));
      try
      {
        return this.Session.Context.Settings.GetServerSettings(category, checkDefinition);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ServerManager), ex, this.Session.SessionInfo);
        return (IDictionary) null;
      }
    }

    public virtual object GetServerSetting(string path) => this.GetServerSetting(path, true);

    public virtual object GetServerSetting(string path, bool checkDefinition)
    {
      this.onApiCalled(nameof (ServerManager), nameof (GetServerSetting), new object[1]
      {
        (object) path
      });
      if ((path ?? "") == null)
        Err.Raise(TraceLevel.Warning, nameof (ServerManager), (ServerException) new ServerArgumentException("Setting path cannot be null", nameof (path), this.Session.SessionInfo));
      try
      {
        return this.Session.Context.Settings.GetServerSetting(path, checkDefinition);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ServerManager), ex, this.Session.SessionInfo);
        return (object) null;
      }
    }

    public virtual void UpdateServerSettings(IDictionary values, AclFeature feature)
    {
      this.UpdateServerSettings(values, true, feature, true);
    }

    public virtual void UpdateServerSettings(
      IDictionary values,
      bool checkDefinition,
      AclFeature feature,
      bool checkRootAdmin = true)
    {
      this.onApiCalled(nameof (ServerManager), nameof (UpdateServerSettings), new object[1]
      {
        (object) values
      });
      if (values == null)
        Err.Raise(TraceLevel.Warning, nameof (ServerManager), (ServerException) new ServerArgumentException("Value dictionary cannot be null", nameof (values), this.Session.SessionInfo));
      try
      {
        if (checkRootAdmin)
          this.Security.DemandFeatureAccess(feature);
        this.Session.Context.Settings.UpdateServerSettings(values, checkDefinition);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ServerManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void UpdateServerSettings(IDictionary values)
    {
      this.UpdateServerSettings(values, true, true);
    }

    public virtual void UpdateServerSettings(
      IDictionary values,
      bool checkDefinition,
      bool checkRootAdmin = true)
    {
      this.onApiCalled(nameof (ServerManager), nameof (UpdateServerSettings), new object[1]
      {
        (object) values
      });
      if (values == null)
        Err.Raise(TraceLevel.Warning, nameof (ServerManager), (ServerException) new ServerArgumentException("Value dictionary cannot be null", nameof (values), this.Session.SessionInfo));
      try
      {
        if (checkRootAdmin)
          this.Security.DemandRootAdministrator();
        this.Session.Context.Settings.UpdateServerSettings(values, checkDefinition);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ServerManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void UpdateServerSetting(string path, object value)
    {
      this.UpdateServerSetting(path, value, true);
    }

    public virtual void UpdateServerSetting(string path, object value, bool checkDefinition)
    {
      this.onApiCalled(nameof (ServerManager), nameof (UpdateServerSetting), new object[2]
      {
        (object) path,
        value
      });
      if ((path ?? "") == null)
        Err.Raise(TraceLevel.Warning, nameof (ServerManager), (ServerException) new ServerArgumentException("Setting path cannot be null", nameof (path), this.Session.SessionInfo));
      if (value == null)
        Err.Raise(TraceLevel.Warning, nameof (ServerManager), (ServerException) new ServerArgumentException("Setting value cannot be null", nameof (value), this.Session.SessionInfo));
      try
      {
        this.Session.Context.Settings.SetServerSetting(path, value, checkDefinition);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ServerManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void RefreshCache(bool async)
    {
      this.onApiCalled(nameof (ServerManager), nameof (RefreshCache), new object[1]
      {
        (object) async
      });
      try
      {
        this.Security.DemandRootAdministrator();
        this.Session.Context.RefreshCache(async);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ServerManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void UpdateWebLoginSetting(
      string keyName,
      string instanceName,
      string selectedValue,
      string userId)
    {
      this.onApiCalled(nameof (ServerManager), nameof (UpdateWebLoginSetting), new object[1]
      {
        (object) keyName
      });
      try
      {
        SmartClientUtils.UpsertAttribute(instanceName, "TokenLoginOnly", Convert.ToInt32(Utils.ParseBoolean((object) selectedValue)).ToString(), userId, ServerUtil.GetScHeaderToken());
        this.Session.Context.Cache.Remove(keyName);
      }
      catch (Exception ex)
      {
        Tracing.Log(true, "Error", nameof (ServerManager), string.Format("Error while updating weblogin setting for the key : {0} and exception {1} " + keyName, (object) ex.Message));
      }
    }

    public virtual RuntimeEnvironment GetRuntimeEnvironment()
    {
      this.onApiCalled(nameof (ServerManager), nameof (GetRuntimeEnvironment), Array.Empty<object>());
      try
      {
        return ServerGlobals.RuntimeEnvironment;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ServerManager), ex, this.Session.SessionInfo);
        return RuntimeEnvironment.Default;
      }
    }

    public virtual int PostDataToSession(string sessionId, object data)
    {
      this.onApiCalled(nameof (ServerManager), nameof (PostDataToSession), new object[2]
      {
        (object) sessionId,
        data
      });
      try
      {
        IClientSession session = this.Session.Context.Sessions.GetSession(sessionId);
        if (session == null)
          return 0;
        session.RaiseEvent((ServerEvent) new DataExchangeEvent(session.SessionInfo, data, this.Session.SessionInfo));
        return 1;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ServerManager), ex, this.Session.SessionInfo);
        return 0;
      }
    }

    public virtual int PostDataToUser(string userId, object data)
    {
      this.onApiCalled(nameof (ServerManager), nameof (PostDataToUser), new object[2]
      {
        (object) userId,
        data
      });
      try
      {
        IClientSession[] sessionsForUser = this.Session.Context.Sessions.GetSessionsForUser(userId);
        int user = 0;
        for (int index = 0; index < sessionsForUser.Length; ++index)
        {
          try
          {
            sessionsForUser[index].RaiseEvent((ServerEvent) new DataExchangeEvent(sessionsForUser[index].SessionInfo, data, this.Session.SessionInfo));
            ++user;
          }
          catch
          {
          }
        }
        return user;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ServerManager), ex, this.Session.SessionInfo);
        return 0;
      }
    }

    public virtual int PostDataToAll(object data)
    {
      this.onApiCalled(nameof (ServerManager), nameof (PostDataToAll), new object[1]
      {
        data
      });
      try
      {
        int all = 0;
        foreach (Elli.Server.Remoting.SessionObjects.Session session in this.Session.Context.Sessions)
        {
          try
          {
            session.RaiseEvent((ServerEvent) new DataExchangeEvent(session.SessionInfo, data, this.Session.SessionInfo));
            ++all;
          }
          catch
          {
          }
        }
        return all;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ServerManager), ex, this.Session.SessionInfo);
        return 0;
      }
    }

    public virtual void DefragmentDatabase(IServerProgressFeedback feedback)
    {
      this.onApiCalled(nameof (ServerManager), nameof (DefragmentDatabase), Array.Empty<object>());
      try
      {
        DbAccessManager.DefragmentDatabase(feedback);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ServerManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual int GetDbFragmentationLevel()
    {
      this.onApiCalled(nameof (ServerManager), "GetMaxDbFragmentationLevel", Array.Empty<object>());
      try
      {
        return DbAccessManager.GetAvgFragmentationLevel();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ServerManager), ex, this.Session.SessionInfo);
        return -1;
      }
    }

    public virtual int GetDbConsistencyErrorCount()
    {
      this.onApiCalled(nameof (ServerManager), nameof (GetDbConsistencyErrorCount), Array.Empty<object>());
      try
      {
        return DbAccessManager.GetConsistencyErrorCount();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ServerManager), ex, this.Session.SessionInfo);
        return -1;
      }
    }

    public virtual DbSize GetDbSize()
    {
      this.onApiCalled(nameof (ServerManager), nameof (GetDbSize), Array.Empty<object>());
      try
      {
        return DbAccessManager.GetDatabaseSize();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ServerManager), ex, this.Session.SessionInfo);
        return (DbSize) null;
      }
    }

    public virtual DbUsageInfo GetDbUsageInfo()
    {
      this.onApiCalled(nameof (ServerManager), nameof (GetDbUsageInfo), Array.Empty<object>());
      try
      {
        return DbAccessManager.GetUsageInfo();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ServerManager), ex, this.Session.SessionInfo);
        return (DbUsageInfo) null;
      }
    }

    public virtual PreauthorizedModule[] GetPreauthorizedModules()
    {
      this.onApiCalled(nameof (ServerManager), nameof (GetPreauthorizedModules), Array.Empty<object>());
      try
      {
        return PreauthorizedModuleAccessor.GetPreauthorizedModules();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ServerManager), ex, this.Session.SessionInfo);
        return (PreauthorizedModule[]) null;
      }
    }

    public virtual void AddPreauthorizedModule(PreauthorizedModule module)
    {
      this.onApiCalled(nameof (ServerManager), nameof (AddPreauthorizedModule), new object[1]
      {
        (object) module
      });
      try
      {
        PreauthorizedModuleAccessor.AddPreauthorizedModule(module);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ServerManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void RemovePreauthorizedModule(PreauthorizedModule module)
    {
      this.onApiCalled(nameof (ServerManager), nameof (RemovePreauthorizedModule), new object[1]
      {
        (object) module
      });
      try
      {
        PreauthorizedModuleAccessor.RemovePreauthorizedModule(module);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ServerManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual Dictionary<string, object> GetServerAttributes()
    {
      if (this.serverAttributes == null)
      {
        this.serverAttributes = new Dictionary<string, object>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
        string str = (string) null;
        using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Ellie Mae\\Encompass\\RMI"))
        {
          if (registryKey != null)
            str = (string) registryKey.GetValue("Credentials");
        }
        this.serverAttributes.Add("RMICredentials", (object) str);
      }
      return this.serverAttributes;
    }

    public virtual VersionManagementGroup[] GetVersionManagementGroups()
    {
      this.onApiCalled(nameof (ServerManager), nameof (GetVersionManagementGroups), Array.Empty<object>());
      try
      {
        return VersionManagementSettings.GetVersionManagementGroups();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ServerManager), ex, this.Session.SessionInfo);
        return (VersionManagementGroup[]) null;
      }
    }

    public virtual VersionManagementGroup GetVersionManagementGroup(int groupId)
    {
      this.onApiCalled(nameof (ServerManager), nameof (GetVersionManagementGroup), new object[1]
      {
        (object) groupId
      });
      try
      {
        return VersionManagementSettings.GetVersionManagementGroup(groupId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ServerManager), ex, this.Session.SessionInfo);
        return (VersionManagementGroup) null;
      }
    }

    public virtual VersionManagementGroup GetDefaultVersionManagementGroup()
    {
      this.onApiCalled(nameof (ServerManager), nameof (GetDefaultVersionManagementGroup), Array.Empty<object>());
      try
      {
        return VersionManagementSettings.GetDefaultVersionManagementGroup();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ServerManager), ex, this.Session.SessionInfo);
        return (VersionManagementGroup) null;
      }
    }

    public virtual void UpdateVersionManagementGroup(VersionManagementGroup group)
    {
      this.onApiCalled(nameof (ServerManager), nameof (UpdateVersionManagementGroup), new object[1]
      {
        (object) group
      });
      try
      {
        VersionManagementSettings.UpdateVersionManagementGroup(group);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ServerManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual int CreateVersionManagementGroup(VersionManagementGroup group)
    {
      this.onApiCalled(nameof (ServerManager), nameof (CreateVersionManagementGroup), new object[1]
      {
        (object) group
      });
      try
      {
        return VersionManagementSettings.CreateVersionManagementGroup(group);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ServerManager), ex, this.Session.SessionInfo);
        return -1;
      }
    }

    public virtual void DeleteVersionManagementGroup(int groupId)
    {
      this.onApiCalled(nameof (ServerManager), nameof (DeleteVersionManagementGroup), new object[1]
      {
        (object) groupId
      });
      try
      {
        VersionManagementSettings.DeleteVersionManagementGroup(groupId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ServerManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual UserInfoSummary[] GetVersionManagementGroupUsers(int groupId)
    {
      this.onApiCalled(nameof (ServerManager), nameof (GetVersionManagementGroupUsers), new object[1]
      {
        (object) groupId
      });
      try
      {
        return VersionManagementSettings.GetVersionManagementGroupUsers(groupId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ServerManager), ex, this.Session.SessionInfo);
        return (UserInfoSummary[]) null;
      }
    }

    public virtual Version GetEncompassFieldListVersion()
    {
      this.onApiCalled(nameof (ServerManager), nameof (GetEncompassFieldListVersion), Array.Empty<object>());
      try
      {
        return StandardFields.Instance.FileVersion;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ServerManager), ex, this.Session.SessionInfo);
        return (Version) null;
      }
    }

    public virtual void ReplaceEncompassFieldList(
      BinaryObject data,
      DateTime fileModificationTimeUtc,
      bool forceReload)
    {
      this.onApiCalled(nameof (ServerManager), nameof (ReplaceEncompassFieldList), new object[2]
      {
        (object) data,
        (object) forceReload
      });
      try
      {
        this.Security.DemandSuperAdministrator();
        data.Download();
        ServerGlobals.ReplaceEncompassFieldList(data, fileModificationTimeUtc);
        if (!forceReload)
          return;
        StandardFields.Instance.Reload(true);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ServerManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual Version ReloadEncompassFieldList()
    {
      this.onApiCalled(nameof (ServerManager), nameof (ReloadEncompassFieldList), Array.Empty<object>());
      try
      {
        StandardFields.Instance.Reload(true);
        return StandardFields.Instance.FileVersion;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ServerManager), ex, this.Session.SessionInfo);
        return (Version) null;
      }
    }

    public virtual string DownloadAndApplyServerMajorUpdates(
      string userid,
      string password,
      int tipMajorUpdateNumber,
      bool raiseOnError)
    {
      Tracing.Log(this.sw, TraceLevel.Info, "SMU", "DownloadAndApplyServerMajorUpdates in ServerManager");
      return this.downloadAndApplyServerUpdates(userid, password, tipMajorUpdateNumber, raiseOnError, false);
    }

    public virtual string DownloadAndApplyServerHotUpdates(
      string userid,
      string password,
      int tipHotUpdateNumber,
      bool raiseOnError)
    {
      Tracing.Log(this.sw, TraceLevel.Info, "SHU", "DownloadAndApplyServerHotUpdates in SeverManager");
      return this.downloadAndApplyServerUpdates(userid, password, tipHotUpdateNumber, raiseOnError, true);
    }

    private string downloadAndApplyServerUpdates(
      string userid,
      string password,
      int tipMajorUpdateNumber,
      bool raiseOnError,
      bool serverHotUpdate)
    {
      string className = serverHotUpdate ? "SHU" : "SMU";
      this.onApiCalled(nameof (ServerManager), nameof (downloadAndApplyServerUpdates), Array.Empty<object>());
      try
      {
        this.Security.DemandSuperAdministrator();
        string str = ServerUpdateInstaller.DownloadAndApplyServerHotUpdates(userid, password, tipMajorUpdateNumber, raiseOnError, serverHotUpdate);
        if (str == null)
          Tracing.Log(this.sw, TraceLevel.Info, className, "The server update is installed successfully");
        return str;
      }
      catch (Exception ex)
      {
        string msg = "Error applying server updates: " + ex.Message;
        Tracing.Log(true, "Error", nameof (ServerManager), msg);
        if (raiseOnError)
          Err.Reraise(nameof (ServerManager), ex, this.Session.SessionInfo);
        return msg;
      }
    }

    public virtual string GetDbSchemaVersion(string schemaName)
    {
      this.onApiCalled(nameof (ServerManager), nameof (GetDbSchemaVersion), new object[1]
      {
        (object) schemaName
      });
      try
      {
        return SchemaAccessor.GetDbSchemaVersion(schemaName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ServerManager), ex, this.Session.SessionInfo);
        return (string) null;
      }
    }

    public virtual BinaryObject GetDbSchemaScript(string schemaName)
    {
      this.onApiCalled(nameof (ServerManager), nameof (GetDbSchemaScript), new object[1]
      {
        (object) schemaName
      });
      try
      {
        return BinaryObject.Marshal(new BinaryObject(SchemaAccessor.GetDbSchemaScript(schemaName), Encoding.Default));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ServerManager), ex, this.Session.SessionInfo);
        return (BinaryObject) null;
      }
    }

    public virtual BinaryObject GetElliDataAssembly()
    {
      this.onApiCalled(nameof (ServerManager), nameof (GetElliDataAssembly), Array.Empty<object>());
      try
      {
        return BinaryObject.Marshal(SchemaAccessor.GetElliDataAssembly());
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ServerManager), ex, this.Session.SessionInfo);
        return (BinaryObject) null;
      }
    }

    public virtual string GetServerDllVersion()
    {
      try
      {
        if (string.IsNullOrWhiteSpace(ServerManager.serverDllVersion))
          ServerManager.serverDllVersion = EncompassServer.GetServerDllVersion();
      }
      catch (Exception ex)
      {
        Tracing.Log(true, "Error", nameof (ServerManager), "Error getting Server DLL version: " + ex.Message);
        ServerManager.serverDllVersion = "0.0.0.0";
      }
      return ServerManager.serverDllVersion;
    }

    public void WriteLogs(IEnumerable<Encompass.Diagnostics.Logging.Schema.Log> logs)
    {
      using (this.Session.Context.MakeCurrent())
        new RemotingLogConsumer(DiagUtility.DefaultLogger, this.Session.Context.ClientID, this.Session.UserID, this.Session.SessionID).WriteLogs(logs);
    }
  }
}
