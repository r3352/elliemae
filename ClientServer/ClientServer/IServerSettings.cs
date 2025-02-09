// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.IServerSettings
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Settings;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public interface IServerSettings
  {
    PwdRuleValidator GetPasswordValidator();

    bool Disabled { get; }

    bool TaskSchedulerDisabled { get; }

    TimeSpan TaskHistoryExpirationPeriod { get; }

    string ApplicationDir { get; }

    string EncompassDataDir { get; }

    string TPOAdminSiteUrl { get; }

    string AppDataDir { get; }

    string LoansDir { get; }

    string DraftLoansDir { get; }

    string SettingsDir { get; }

    string LogDir { get; }

    string EncompassPlatformAPI { get; }

    DbServerType DbServerType { get; }

    string ConnectionString { get; }

    string ReadReplicaConnectionString { get; }

    bool IsReadReplicaUseAGListener { get; }

    CacheSetting CacheSetting { get; }

    ServerMutexSetting MutexSetting { get; }

    string AlertQueryMethod { get; set; }

    string PipelineQueryMethod { get; set; }

    string GetSqlDbID();

    string GetDataFolderPath(string relativePath, bool createIfMissing = true);

    string GetDataFilePath(string relativePath, bool createFolderIfMissing = true);

    string GetUserDataFolderPath(string userId, bool createIfMissing = true);

    string GetUserDataFolderPath(string userId, string relativePath, bool createIfMissing = true);

    string GetUserDataFilePath(string userId, string relativePath, bool createFolderIfMissing = true);

    string GetLoansFolderPath(string relativePath, bool createIfMissing = true);

    string GetLoansFilePath(string relativePath, bool createFolderIfMissing = true);

    string GetLoanFolderPath(string loanFolder, string loanName, bool createIfMissing = true);

    string GetLoanFilePath(
      string loanFolder,
      string loanName,
      string relativePath,
      bool createFolderIfMissing = true);

    string GetSettingsFolderPath(string relativePath, bool createIfMissing = true);

    string GetSettingsFilePath(string relativePath, bool createFolderIfMissing = true);

    string GetLogFolderPath(string relativePath, bool createIfMissing = true);

    string GetLogFilePath(string relativePath, bool createFolderIfMissing = true);

    string GetConnectionString(int sqlRead);

    int GetSqlRead();

    void SetSqlRead(int sqlRead);

    object GetServerSetting(string path);

    object GetServerSetting(string path, bool checkDefinition);

    void SetServerSetting(string path, object value);

    void UpdateServerSettings(IDictionary settings, bool checkDefinition);

    void SetServerSetting(string path, object value, bool checkDefinition);

    IDictionary GetServerSettings();

    IDictionary GetServerSettings(string category);

    IDictionary GetServerSettings(string[] categories);

    IDictionary GetServerSettings(string[] lstOfCategories, bool checkDefinition);

    IDictionary GetServerSettings(string category, bool checkDefinition);

    IDictionary GetServerSettings(IDictionary settingDefs);

    void SetServerSettings(IDictionary settings);

    void SetServerSettings(IDictionary settings, bool checkDefinition);

    void DeleteVendorSetting(string category, string setting);

    void SetVendorSetting(string path, object value, bool checkDefinition = true);

    void Refresh();

    object GetStorageSetting(string name);

    object GetPipelineSearchSetting(string name);

    IStorageSettings GetStorageModeSettings();

    void EnableRegistryMonitoring();

    IStorageSettings GetStorageModeSettingsForCC();

    string GetSqlConnectionString(bool useReadReplica);
  }
}
