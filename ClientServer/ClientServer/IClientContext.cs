// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.IClientContext
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Cache;
using EllieMae.EMLite.ClientServer.Interfaces;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public interface IClientContext
  {
    void ClearIsTPOClient();

    string InstanceName { get; }

    DateTime InstanceStartTime { get; }

    bool AllowConcurrentEditing { get; }

    bool EnableLoanSoftArchival { get; }

    bool ExclusiveLockCurrLoginsOnly { get; }

    bool UseERDB { get; }

    IServerSettings Settings { get; }

    IClientCache Cache { get; }

    IContextTraceLog TraceLog { get; }

    ISessionManager Sessions { get; }

    string ERDBServer { get; }

    int ERDBServerPort { get; }

    string ClientID { get; }

    string EncompassSystemID { get; }

    bool IPRestrictionSetting { get; }

    void ResetTraceLog();

    bool IsDefault();

    IRequestContext MakeCurrent(
      IDataCache requestCache = null,
      string correlationId = null,
      Guid? transactionId = null,
      bool? forcePrimaryDB = null);

    IDataCache CurrentRequestCache { get; }

    void RecordClassName(string c);

    void RecordApiName(string a);

    void RecordParms(object[] p);

    void AddParm(object p);

    void RecordSession(ISession s);

    void Release();

    void RefreshCache(bool async);

    void ResetCacheStore(bool force);

    void ClearERDBCache();

    void ResetRemotingInterfaces();

    bool IPRestrictionSettingAllApplications { get; }

    bool IsConcurrentUpdateNotificationEnabled { get; }

    IConcurrentUpdateNotificationHandler ConcurrentUpdateNotificationHandler { get; }

    void ResetConcurrentUpdateNotificationSubscription();

    ITradeLoanUpdateNotificationHandler TradeLoanUpdateNotificationHandler { get; }

    void ResetTradeLoanUpdateNotificationSubscription();

    ILockComparisonFieldsNotificationHandler LockComparisonFieldsNotificationHandler { get; }

    void ResetLockComparisonFieldsNotificationSubscription();
  }
}
