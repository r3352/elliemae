// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.IServerManager
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.RemotingServices;
using Encompass.Diagnostics.Logging.Targets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public interface IServerManager : IRemotingLogConsumer
  {
    SessionInfo[] GetAllSessionInfo(bool fromDatabase);

    SessionInfo GetSessionInfo(string sessionId);

    void AddServiceSession(string sessionId, string appName);

    KeyValuePair<string, string>[] GetAllUserSessionIds(string userId);

    bool LoginsEnabled { get; set; }

    bool LoginsEnabledWithForPersonna { get; set; }

    TraceLevel TraceLevel { get; set; }

    void TerminateSession(string sessionId, bool forceDisconnect, bool includeRemoteSession);

    void TerminateSessions(string[] sessionId, bool forceDisconnect, bool includeRemoteSessions);

    void TerminateAllSessions(bool forceDisconnect, bool includeRemoteSessions);

    void SendMessage(Message message);

    void SendMessage(Message message, string sessionId, bool includeRemoteSession);

    void SendMessage(Message message, string[] sessionIds, bool includeRemoteSessions);

    void BroadcastMessage(Message message, bool includeRemoteSessions);

    void SendNotification(UserNotification notification);

    void SendNotifications(UserNotification[] notifications);

    IDictionary GetServerSettings();

    IDictionary GetServerSettings(string category);

    IDictionary GetServerSettings(string category, bool checkDefinition);

    object GetServerSetting(string path);

    object GetServerSetting(string path, bool checkDefinition);

    void UpdateServerSettings(IDictionary settings, AclFeature feature);

    void UpdateServerSettings(
      IDictionary settings,
      bool checkDefinition,
      AclFeature feature,
      bool checkRootAdmin = true);

    void UpdateServerSettings(IDictionary settings);

    void UpdateServerSettings(IDictionary settings, bool checkDefinition, bool checkRootAdmin = true);

    void UpdateServerSetting(string path, object value);

    void UpdateServerSetting(string path, object value, bool checkDefinition);

    string GetTPOAdminSiteUrlFromDS();

    void RefreshCache(bool async);

    void UpdateWebLoginSetting(
      string keyName,
      string instanceName,
      string selectedValue,
      string userId);

    RuntimeEnvironment GetRuntimeEnvironment();

    int PostDataToSession(string sessionId, object data);

    int PostDataToUser(string userId, object data);

    int PostDataToAll(object data);

    void DefragmentDatabase(IServerProgressFeedback feedback);

    int GetDbFragmentationLevel();

    int GetDbConsistencyErrorCount();

    DbSize GetDbSize();

    DbUsageInfo GetDbUsageInfo();

    PreauthorizedModule[] GetPreauthorizedModules();

    void AddPreauthorizedModule(PreauthorizedModule module);

    void RemovePreauthorizedModule(PreauthorizedModule module);

    Dictionary<string, object> GetServerAttributes();

    VersionManagementGroup[] GetVersionManagementGroups();

    VersionManagementGroup GetVersionManagementGroup(int groupId);

    VersionManagementGroup GetDefaultVersionManagementGroup();

    void UpdateVersionManagementGroup(VersionManagementGroup group);

    int CreateVersionManagementGroup(VersionManagementGroup group);

    void DeleteVersionManagementGroup(int groupId);

    UserInfoSummary[] GetVersionManagementGroupUsers(int groupId);

    Version GetEncompassFieldListVersion();

    void ReplaceEncompassFieldList(
      BinaryObject data,
      DateTime fileModificationTimeUtc,
      bool forceReload);

    Version ReloadEncompassFieldList();

    string DownloadAndApplyServerHotUpdates(
      string userid,
      string password,
      int tipHotUpdateNumber,
      bool raiseOnError);

    string DownloadAndApplyServerMajorUpdates(
      string userid,
      string password,
      int tipMajorUpdateNumber,
      bool raiseOnError);

    string GetDbSchemaVersion(string schemaName);

    BinaryObject GetDbSchemaScript(string schemaName);

    BinaryObject GetElliDataAssembly();

    string GetServerDllVersion();
  }
}
