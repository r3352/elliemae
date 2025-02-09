// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.IManagementSession
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.ServerTasks;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.RemotingServices;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public interface IManagementSession : IClientSession
  {
    ServerProcessInfo GetServerProcessInfo();

    string[] GetAllInstanceNames(bool enabledOnly);

    int GetSessionCount(string instanceName);

    SessionInfo[] GetAllSessionInfo(string instanceName);

    SessionInfo[] GetAllSessionInfoFromDB(string instanceName);

    SessionDiagnostics GetSessionDiagnostics(string instanceName, string sessionId);

    SessionDiagnostics[] GetAllSessionDiagnostics(string instanceName);

    string GetDefaultInstanceName();

    void RefreshCache(string instanceName, bool async);

    Version GetEncompassFieldListVersion();

    Version ReloadEncompassFieldList();

    void GarbageCollect();

    void CreateInstance(string instanceName, DataSourceInfo dataSource);

    void DeleteInstance(string instanceName);

    DataSourceInfo GetDataSource(string instanceName);

    bool PingSession(string instanceName, string sessionId, TimeSpan timeout);

    void TerminateSession(string instanceName, string sessionId);

    void TerminateAllSessions(string instanceName);

    void Disable(string instanceName, bool terminateSessions);

    void Enable(string instanceName);

    ProcessStatus GetStatus(string instanceName);

    void VerifyConfiguration(string instanceName);

    string Echo(string text);

    void SendMessage(string instanceName, string sessionId, Message message);

    void BroadcastMessage(string instanceName, Message message);

    CompanyInfo GetCompanyInfo(string instanceName);

    LicenseInfo GetServerLicense(string instanceName);

    string GetCompanySetting(string instanceName, string section, string key);

    int GetEnabledUserCount(string instanceName);

    void SetDebug(bool enabled);

    bool IsDebugEnabled();

    void SetGlobalTraceLevel(TraceLevel level);

    int GetErrorCount(string instanceName);

    string[] GetErrors(string instanceName);

    void ClearErrors(string instanceName);

    bool isPipelineMigrationRequired(string instanceName);

    bool isPipelineMigrationRunning(string instanceName);

    void updatePipelineMigrationRunningFlag(string instanceName, bool updateMigrationTimeFlag);

    bool MigratePipelineView(string instanceName);

    void UpdatePipelineViewExtOrgId(string instanceName);

    int GetSystemAlertCount();

    SystemAlert[] GetSystemAlerts();

    void ClearSystemAlerts();

    void RegisterForEvents(Type eventType);

    void UnregisterForEvents(Type eventType);

    void RegisterForTracing(string instanceName, TraceLevel traceLevel);

    void UnregisterForTracing(string instanceName);

    UserInfo[] GetAllUsers(string instanceName);

    void EnableUser(string instanceName, string userId);

    void DisableUser(string instanceName, string userId);

    void ResetEPassMessages(string instanceName);

    void DefragmentDatabase(string instanceName, IServerProgressFeedback feedback);

    int GetDbFragmentationLevel(string instanceName);

    int GetDbConsistencyErrorCount(string instanceName);

    DbSize GetDbSize(string instanceName);

    DbUsageInfo GetDbUsageInfo(string instanceName);

    ServerTaskProcessorStatus[] GetTaskProcessorStatuses();

    string StartTaskProcessor();

    void ResetTaskProcessors();

    object GetServerSetting(string instanceName, string settingPath);

    IDictionary GetServerSettings(string instanceName, string categoryName);

    void UpdateServerSetting(string instanceName, string settingName, object value);

    ServerInternalsInfo GetLoanXDBStoreCacheInfo(string instanceName, string fieldID = null);

    string[] GetServerCacheNames(string instanceName);

    ServerInternalsInfo GetServerCacheInfo(string instanceName, string cacheName);

    ServerInternalsInfo GetAllS2SSessionInfo(string instanceName);

    ServerInternalsInfo GetAllS2SClientSessionInfo(string instanceName);

    void EndS2SSession(string serverUri);

    void TerminateS2SClientSession(string instanceName, string s2sSessionID);

    void ResetClientContextTraceLog(string instanceName);

    string GetEncInstallDir(string instanceName);

    string GetEncDataDir(string instanceName);

    string GetEncLogDir(string instanceName);

    string GetEncompassDbFullVersion(string instanceName);

    string GetDbConnectionString(string instanceName);

    string[] GetCachedDataSource(string instanceName);

    int GetSqlRead(string instanceName);

    void SetSqlRead(string instanceName, int sqlRead);

    int GetNumConcurrentLogins(string instanceName);

    int GetMaxConcurrentLogins(string instanceName);

    void SetMaxConcurrentLogins(string instanceName, int val, bool updateDatabase = false);

    string GetUseReaderWriterLockSlim(string instanceName);

    void ResetUseReaderWriterLockSlim(string instanceName);

    string[] GetMacAddresses();

    int GetRebuildReportingDbThreadCount(string instanceName);

    void SetRebuildReportingDbThreadCount(string instanceName, int val);

    string GetCacheStoreSource(string instanceName);

    string ResetCacheStore(string instanceName);

    string[] GetCacheKeys(string instanceName);

    object GetCachedValue(string key, string instanceName);

    void RemoveKey(string key, string instanceName);

    Dictionary<string, string[]> GetSessionObjectNames(string instanceName, string userid);

    RegistryKey OpenEncompassRegistryKey(string name, bool wow6432Node);

    int CacheLockTimeout(string instanceName);

    bool? CacheRegetFromCache(string instanceName);

    bool? CacheRegetFromDB(string instanceName);

    object ServerGlobal(string instanceName, string varname);

    int GetMaxRecentLoans(string instanceName);

    void ResetMaxRecentLoans(string instanceName);

    Tuple<int, int> GetServerActivityTimeoutTimerInterval();

    Tuple<int, int> SetServerActivityTimeoutTimerInterval(int timeout, int timerInterval);

    string GetCacheStats(string instanceName);

    Dictionary<string, bool> CompareRegistryAndDSConfigurations(
      string instanceName,
      string dsUrl,
      string[] ignores);

    void CopyRegistryConfigsToDS(string instanceName, string dsUrl, string[] ignores);

    void SendReconnectMessageAndStopServer();

    void RenewServerLic(string instanceName);

    void RefreshBillingCalc(string instanceName);

    string SendConcurrentUpdateNotification(string instanceName, string sessionId, string loanGuid);

    void ClearConcurrentUpdateNotificationCache(string instanceName);

    void ResetConcurrentUpdateNotificationSubscription(string instanceName);

    string GetNotificationSubscriptionStatus(string instanceName);

    List<Tuple<string, string, string>> VerifyXDBFieldsNotInSyncWithLoan(
      string instanceName,
      string loanGuid);

    bool MigrateEfolderViewsFromFilesToDB(
      string instanceName,
      TemplateSettingsType templateSettingsType);

    void ResetTradeLoanUpdateNotificationSubscription(string instanceName);

    string SendTradeLoanUpdateNotification(
      string instanceName,
      string sessionId,
      string tradeId,
      string tradeStatus);

    string GetTradeLoanUpdateNotificationSubscriptionStatus(string instanceName);

    void ResetLockComparisonFieldsNotificationSubscription(string instanceName);

    string GetLockComparisonFieldsNotificationSubscriptionStatus(string instanceName);
  }
}
