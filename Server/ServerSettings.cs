// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerSettings
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Elli.Common.Data;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.Common.Settings;
using EllieMae.Encompass.AsmResolver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public sealed class ServerSettings : IServerSettings
  {
    private const string className = "ServerSettings�";
    private static readonly string sw = Tracing.SwOutsideLoan;
    public readonly int MaxRecentLoans = 5;
    private object configLock = new object();
    private ClientContext context;
    private EnGlobalSettings staticSettings;
    private object sqlReadLock = new object();
    private bool _sqlReadNext = true;

    public string AlertQueryMethod { get; set; }

    public string PipelineQueryMethod { get; set; }

    public ServerSettings(ClientContext context, EnGlobalSettings globalSettings)
    {
      this.context = context;
      this.staticSettings = globalSettings;
      this.Refresh();
    }

    private ServerSettings()
    {
    }

    public PwdRuleValidator GetPasswordValidator()
    {
      int serverSetting1 = (int) this.GetServerSetting("Password.MinLength");
      int serverSetting2 = (int) this.GetServerSetting("Password.NumUpperCase");
      int serverSetting3 = (int) this.GetServerSetting("Password.NumLowerCase");
      int serverSetting4 = (int) this.GetServerSetting("Password.NumDigits");
      int serverSetting5 = (int) this.GetServerSetting("Password.NumSpecial");
      return new PwdRuleValidator(serverSetting1, serverSetting3, serverSetting2, serverSetting4, serverSetting5);
    }

    public bool Disabled => this.GetInstanceSettings().Disabled;

    public bool TaskSchedulerDisabled => this.staticSettings.TaskSchedulerDisabled || this.Disabled;

    public TimeSpan TaskHistoryExpirationPeriod
    {
      get => TimeSpan.FromDays((double) (int) this.GetServerSetting("Scheduler.NumDaysOfHistory"));
    }

    public string ApplicationDir => this.staticSettings.EncompassProgramDirectory;

    public string EncompassDataDir => this.GetInstanceSettings().EncompassDataDirectory;

    public string TPOAdminSiteUrl => this.staticSettings.TPOAdminSiteUrl;

    public string AppDataDir => this.GetInstanceSettings().AppDataDirectory;

    public string LoansDir => this.GetInstanceSettings().AppLoanDirectory;

    public string DraftLoansDir => this.GetInstanceSettings().AppDraftLoansDirectory;

    public string SettingsDir => this.GetInstanceSettings().AppSettingsDirectory;

    public string LogDir => this.GetInstanceSettings().AppLogDirectory;

    public string EncompassPlatformAPI => this.GetInstanceSettings().EncompassPlatformAPI;

    public string GetSqlDbID()
    {
      EnGlobalSettings instanceSettings = this.GetInstanceSettings();
      string str = instanceSettings.DatabaseServer;
      try
      {
        if ((str ?? "").ToLower().StartsWith("(local)"))
          str = str.Insert(7, "[" + Dns.GetHostName() + "]");
      }
      catch (Exception ex)
      {
        Tracing.Log(ServerSettings.sw, nameof (ServerSettings), TraceLevel.Warning, "Error getting real database server: " + ex.Message);
      }
      return XT.ESB64(str + "|" + instanceSettings.DatabaseName + "|" + instanceSettings.DatabaseUserID, KB.SC64);
    }

    public string GetDataFolderPath(string relativePath, bool createIfMissing = true)
    {
      return this.getFolderPath(this.AppDataDir, relativePath, createIfMissing);
    }

    public string GetDataFilePath(string relativePath, bool createFolderIfMissing = true)
    {
      return this.getFilePath(this.AppDataDir, relativePath, createFolderIfMissing);
    }

    public string GetUserDataFolderPath(string userId, bool createIfMissing = true)
    {
      return this.getFolderPath(this.AppDataDir, "Users\\" + (User.IsVirtualUser(userId) ? "admin" : userId), createIfMissing);
    }

    public string GetUserDataFolderPath(string userId, string relativePath, bool createIfMissing = true)
    {
      return this.getFolderPath(this.AppDataDir, "Users\\" + (User.IsVirtualUser(userId) ? "admin" : userId) + "\\" + relativePath, createIfMissing);
    }

    public string GetUserDataFilePath(
      string userId,
      string relativePath,
      bool createFolderIfMissing = true)
    {
      return this.getFilePath(this.AppDataDir, "Users\\" + (User.IsVirtualUser(userId) ? "admin" : userId) + "\\" + relativePath, createFolderIfMissing);
    }

    public string GetLoansFolderPath(string relativePath, bool createIfMissing = true)
    {
      return this.getFolderPath(this.LoansDir, relativePath, createIfMissing);
    }

    public string GetLoansFilePath(string relativePath, bool createFolderIfMissing = true)
    {
      return this.getFilePath(this.LoansDir, relativePath, createFolderIfMissing);
    }

    public string GetLoanFolderPath(string loanFolder, string loanName, bool createIfMissing = true)
    {
      return this.getFolderPath(this.LoansDir, loanFolder + "\\" + loanName, createIfMissing);
    }

    public string GetLoanFilePath(
      string loanFolder,
      string loanName,
      string relativePath,
      bool createFolderIfMissing = true)
    {
      return this.getFilePath(this.LoansDir, loanFolder + "\\" + loanName + "\\" + relativePath, (createFolderIfMissing ? 1 : 0) != 0);
    }

    public string GetSettingsFolderPath(string relativePath, bool createIfMissing = true)
    {
      return this.getFolderPath(this.SettingsDir, relativePath, createIfMissing);
    }

    public string GetSettingsFilePath(string relativePath, bool createFolderIfMissing = true)
    {
      return this.getFilePath(this.SettingsDir, relativePath, createFolderIfMissing);
    }

    public string GetLogFolderPath(string relativePath, bool createIfMissing = true)
    {
      return this.getFolderPath(this.LogDir, relativePath, createIfMissing);
    }

    public string GetLogFilePath(string relativePath, bool createFolderIfMissing = true)
    {
      return this.getFilePath(this.LogDir, relativePath, createFolderIfMissing);
    }

    public DbServerType DbServerType => this.GetInstanceSettings().DatabaseType;

    public string ConnectionString => this.GetInstanceSettings().DatabaseConnectionString;

    public string ReadReplicaConnectionString
    {
      get => this.GetInstanceSettings().ReadReplicaConnectionString;
    }

    public string GetSqlConnectionString(bool useReadReplica)
    {
      return !useReadReplica ? this.GetInstanceSettings().DatabaseConnectionString : this.GetInstanceSettings().ReadReplicaConnectionString;
    }

    public bool IsReadReplicaUseAGListener
    {
      get
      {
        return !string.IsNullOrWhiteSpace(this.GetInstanceSettings().DatabaseAGListener) && !this.GetInstanceSettings().DisabledAGListener;
      }
    }

    private int sqlReadNext
    {
      get
      {
        int sqlReadNext;
        lock (this.sqlReadLock)
        {
          sqlReadNext = this._sqlReadNext ? 1 : 0;
          this._sqlReadNext = !this._sqlReadNext;
        }
        return sqlReadNext;
      }
    }

    public string GetConnectionString(int sqlRead) => this.ConnectionString;

    public int GetSqlRead() => (int) this.GetServerSetting("SQL.SqlRead");

    public void SetSqlRead(int sqlRead) => this.SetServerSetting("SQL.SqlRead", (object) sqlRead);

    public CacheSetting CacheSetting => this.context.Cache.CacheSetting;

    public ServerMutexSetting MutexSetting
    {
      get
      {
        if (this.context.Cache.CacheStoreSource == CacheStoreSource.HazelCast || EncompassServer.IsRunningInProcess)
          return ServerMutexSetting.MultiServer;
        return EncompassServer.ServerMode == EncompassServerMode.Service || ServerGlobals.RuntimeEnvironment == RuntimeEnvironment.Hosted ? ServerMutexSetting.SingleServer : ServerMutexSetting.Default;
      }
    }

    public object GetServerSetting(string path) => this.GetServerSetting(path, true);

    private void parseServerSettingPath(string path, out string category, out string name)
    {
      int length = path.IndexOf('.');
      if (length < 0)
        Err.Raise(TraceLevel.Warning, nameof (ServerSettings), (ServerException) new ServerArgumentException("Invalid server setting path '" + path + "'", nameof (path)));
      category = path.Substring(0, length);
      name = path.Substring(length + 1);
    }

    public object GetServerSetting(string path, bool checkDefinition)
    {
      ClientContext context = this.context;
      if (path.ToUpper() == "COMPONENTS.USEERDB")
        return (object) false;
      SettingDefinition settingDefinition = (SettingDefinition) null;
      string category = (string) null;
      string name = (string) null;
      if (checkDefinition)
      {
        settingDefinition = SettingDefinitions.GetSettingDefinition(path);
        if (settingDefinition == null)
          Err.Raise(TraceLevel.Warning, nameof (ServerSettings), (ServerException) new ServerArgumentException("Invalid server setting path '" + path + "'", nameof (path)));
        category = settingDefinition.Category;
        name = settingDefinition.Name;
      }
      else
        this.parseServerSettingPath(path, out category, out name);
      if (path.ToUpper() == "COMPONENTS.ENABLEFASTLOANLOAD")
        category = "Feature";
      Hashtable companySettings = Company.GetCompanySettings(category.ToUpper());
      object serverSetting = companySettings == null || !companySettings.ContainsKey((object) name.ToUpper()) ? (object) null : companySettings[(object) name.ToUpper()];
      if (path.ToUpper() == "COMPONENTS.ENABLEFASTLOANLOAD")
        serverSetting = !((string) serverSetting == "True") ? (object) "DISABLED" : (object) "ENABLED";
      if (checkDefinition)
        serverSetting = settingDefinition.Parse((string) serverSetting);
      return serverSetting;
    }

    public void SetServerSetting(string path, object value)
    {
      this.SetServerSetting(path, value, true);
    }

    public void UpdateServerSettings(IDictionary settings, bool checkDefinition)
    {
      Dictionary<string, Dictionary<string, string>> companySettings = new Dictionary<string, Dictionary<string, string>>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      foreach (DictionaryEntry setting in settings)
        this.PopulateCompanySettings(setting.Key.ToString(), setting.Value, checkDefinition, companySettings);
      foreach (KeyValuePair<string, Dictionary<string, string>> keyValuePair in companySettings)
      {
        string key = keyValuePair.Key;
        Company.SetCompanySettings(keyValuePair.Key, keyValuePair.Value);
        if (string.Equals(key, "POLICIES", StringComparison.OrdinalIgnoreCase) && keyValuePair.Value.ContainsKey("DiscloseManually"))
          this.UpdatePoliciesDiscloseManually(keyValuePair.Value["DiscloseManually"]);
      }
    }

    private void SetServerSetting(
      string path,
      object value,
      bool checkDefinition,
      bool storeAttributeInUpperCase)
    {
      SettingDefinition settingDefinition = (SettingDefinition) null;
      string category = (string) null;
      string name = (string) null;
      if (checkDefinition)
      {
        settingDefinition = SettingDefinitions.GetSettingDefinition(path);
        if (settingDefinition == null)
          Err.Raise(TraceLevel.Warning, nameof (ServerSettings), (ServerException) new ServerArgumentException("Invalid server setting path '" + path + "'", nameof (path)));
        category = settingDefinition.Category;
        name = settingDefinition.Name;
      }
      else
        this.parseServerSettingPath(path, out category, out name);
      string str = checkDefinition ? settingDefinition.ToString(value) : Convert.ToString(value);
      Company.SetCompanySetting(category.ToUpper(), storeAttributeInUpperCase ? name.ToUpper() : name, str);
      if (string.Compare(path, "Policies.DiscloseManually", true) != 0)
        return;
      this.UpdatePoliciesDiscloseManually(str);
    }

    public void SetServerSetting(string path, object value, bool checkDefinition)
    {
      this.SetServerSetting(path, value, checkDefinition, true);
    }

    public void PopulateCompanySettings(
      string path,
      object value,
      bool checkDefinition,
      Dictionary<string, Dictionary<string, string>> companySettings)
    {
      SettingDefinition settingDefinition = (SettingDefinition) null;
      string category;
      string name;
      if (checkDefinition)
      {
        settingDefinition = SettingDefinitions.GetSettingDefinition(path);
        if (settingDefinition == null)
        {
          this.parseServerSettingPath(path, out category, out name);
        }
        else
        {
          category = settingDefinition.Category;
          name = settingDefinition.Name;
        }
      }
      else
        this.parseServerSettingPath(path, out category, out name);
      if (!companySettings.ContainsKey(category.ToUpper()))
        companySettings[category.ToUpper()] = new Dictionary<string, string>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      companySettings[category.ToUpper()][name.ToUpper()] = settingDefinition != null ? settingDefinition.ToString(value) : Convert.ToString(value);
    }

    private void UpdatePoliciesDiscloseManually(string value)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder((IClientContext) this.context);
      dbQueryBuilder.Append("delete [Acl_Features_User] where featureID in (369)");
      dbQueryBuilder.Append("delete [Acl_Features] where featureID in (369)");
      if (string.Compare(value, "TRUE", true) == 0)
        dbQueryBuilder.Append("insert into acl_features select 369, PersonaID, 1 from Personas where PersonaID <> 0");
      dbQueryBuilder.ExecuteNonQuery();
    }

    public IDictionary GetServerSettings()
    {
      Hashtable insensitiveHashtable = CollectionsUtil.CreateCaseInsensitiveHashtable();
      foreach (string category in SettingDefinitions.Categories)
      {
        foreach (DictionaryEntry serverSetting in this.GetServerSettings(category, true))
          insensitiveHashtable[serverSetting.Key] = serverSetting.Value;
      }
      return (IDictionary) insensitiveHashtable;
    }

    public IDictionary GetServerSettings(string category) => this.GetServerSettings(category, true);

    public IDictionary GetServerSettings(string[] categories)
    {
      return this.GetServerSettings(categories, true);
    }

    public IDictionary GetServerSettings(string[] lstOfCategories, bool checkDefinition)
    {
      Hashtable insensitiveHashtable1 = CollectionsUtil.CreateCaseInsensitiveHashtable();
      Hashtable insensitiveHashtable2 = CollectionsUtil.CreateCaseInsensitiveHashtable();
      Dictionary<string, Hashtable> companySettings = Company.GetCompanySettings(lstOfCategories);
      foreach (string lstOfCategory in lstOfCategories)
      {
        if (!insensitiveHashtable1.ContainsKey((object) lstOfCategory))
        {
          Hashtable insensitiveHashtable3 = CollectionsUtil.CreateCaseInsensitiveHashtable();
          insensitiveHashtable1.Add((object) lstOfCategory, (object) insensitiveHashtable3);
          if (companySettings.ContainsKey(lstOfCategory))
          {
            Hashtable hashtable = companySettings[lstOfCategory];
            foreach (object key in (IEnumerable) hashtable.Keys)
              insensitiveHashtable3.Add(key, hashtable[key]);
          }
        }
      }
      if (checkDefinition)
      {
        foreach (DictionaryEntry dictionaryEntry in insensitiveHashtable1)
        {
          Hashtable hashtable = (Hashtable) insensitiveHashtable1[dictionaryEntry.Key];
          Hashtable insensitiveHashtable4 = CollectionsUtil.CreateCaseInsensitiveHashtable();
          foreach (SettingDefinition settingDefinition in (IEnumerable) SettingDefinitions.GetSettingDefinitions(dictionaryEntry.Key.ToString()).Values)
          {
            insensitiveHashtable4[(object) settingDefinition.Path] = settingDefinition.Parse((string) hashtable[(object) settingDefinition.Name]);
            if (settingDefinition.Path.ToUpper() == "COMPONENTS.USEERDB" && Company.GetEdition(this.context) != EncompassEdition.Banker)
              insensitiveHashtable4[(object) settingDefinition.Path] = (object) false;
          }
          insensitiveHashtable2[(object) dictionaryEntry.Key.ToString()] = (object) insensitiveHashtable4;
        }
      }
      else
      {
        foreach (DictionaryEntry dictionaryEntry in insensitiveHashtable1)
        {
          Hashtable hashtable = (Hashtable) insensitiveHashtable1[dictionaryEntry.Key];
          Hashtable insensitiveHashtable5 = CollectionsUtil.CreateCaseInsensitiveHashtable();
          foreach (string key1 in (IEnumerable) hashtable.Keys)
          {
            string key2 = dictionaryEntry.Key.ToString() + "." + key1;
            insensitiveHashtable5[(object) key2] = hashtable[(object) key1];
            if (dictionaryEntry.Key.ToString().ToUpper() == "COMPONENTS" && key1.ToUpper() == "USEERDB" && Company.GetEdition(this.context) != EncompassEdition.Banker)
              insensitiveHashtable5[(object) key2] = (object) "False";
          }
          insensitiveHashtable2[(object) dictionaryEntry.Key.ToString()] = (object) insensitiveHashtable5;
        }
      }
      return (IDictionary) insensitiveHashtable2;
    }

    public IDictionary GetServerSettings(string category, bool checkDefinition)
    {
      Hashtable insensitiveHashtable = CollectionsUtil.CreateCaseInsensitiveHashtable();
      Hashtable companySettings = Company.GetCompanySettings((IClientContext) this.context, category);
      if (checkDefinition)
      {
        foreach (SettingDefinition settingDefinition in (IEnumerable) SettingDefinitions.GetSettingDefinitions(category).Values)
        {
          insensitiveHashtable[(object) settingDefinition.Path] = settingDefinition.Parse((string) companySettings[(object) settingDefinition.Name]);
          if (settingDefinition.Path.ToUpper() == "COMPONENTS.USEERDB" && Company.GetEdition(this.context) != EncompassEdition.Banker)
            insensitiveHashtable[(object) settingDefinition.Path] = (object) false;
        }
      }
      else
      {
        foreach (string key1 in (IEnumerable) companySettings.Keys)
        {
          string key2 = category + "." + key1;
          insensitiveHashtable[(object) key2] = companySettings[(object) key1];
          if (category.ToUpper() == "COMPONENTS" && key1.ToUpper() == "USEERDB" && Company.GetEdition(this.context) != EncompassEdition.Banker)
            insensitiveHashtable[(object) key2] = (object) "False";
        }
      }
      return (IDictionary) insensitiveHashtable;
    }

    public IDictionary GetServerSettings(IDictionary settingDefs)
    {
      Hashtable insensitiveHashtable = CollectionsUtil.CreateCaseInsensitiveHashtable();
      try
      {
        foreach (SettingDefinition settingDefinition in (IEnumerable) settingDefs.Values)
          insensitiveHashtable.Add((object) settingDefinition.Path, this.GetServerSetting(settingDefinition.Path));
      }
      catch (InvalidCastException ex)
      {
        Err.Raise(TraceLevel.Warning, nameof (ServerSettings), (ServerException) new ServerArgumentException("Setting definition dictionary contains invalid types", nameof (settingDefs)));
      }
      return (IDictionary) insensitiveHashtable;
    }

    public void SetServerSettings(IDictionary settings) => this.SetServerSettings(settings, true);

    public void SetServerSettings(IDictionary settings, bool checkDefinition)
    {
      foreach (DictionaryEntry setting in settings)
        this.SetServerSetting(setting.Key.ToString(), setting.Value, checkDefinition);
    }

    public void Refresh()
    {
      if (AssemblyResolver.IsSmartClient)
        return;
      lock (this.configLock)
      {
        this.AlertQueryMethod = string.Concat(this.staticSettings["AlertQueryMethod"]).ToLower();
        this.PipelineQueryMethod = string.Concat(this.staticSettings["PipelineQueryMethod"]).ToLower();
      }
    }

    public object GetStorageSetting(string name)
    {
      return this.context.Cache.Get<IDictionary>("DataStore.StorageSettings", (Func<IDictionary>) (() => this.readStorageSettings()))[(object) name];
    }

    public object GetPipelineSearchSetting(string name)
    {
      return this.context.Cache.Get<IDictionary>("Policies.PipelineSearchSettings", (Func<IDictionary>) (() => this.readPipelineSearchSettings()))[(object) name];
    }

    public IStorageSettings GetStorageModeSettings()
    {
      return EncompassServer.ServerMode == EncompassServerMode.Service ? (IStorageSettings) this.context.Cache.Get<StorageSettings>("DataStore.StorageModeSettings.EBS", (Func<StorageSettings>) (() => this.readStorageModeSettingsFromConfig())) : (IStorageSettings) this.context.Cache.Get<StorageSettings>("DataStore.StorageModeSettings.Core", (Func<StorageSettings>) (() => this.readStorageModeSettingsFromConfig()));
    }

    public IStorageSettings GetStorageModeSettingsForCC()
    {
      return EncompassServer.ServerMode == EncompassServerMode.Service ? (IStorageSettings) this.context.Cache.Get<StorageSettings>("DataStore.StorageModeSettingsForCC.EBS", (Func<StorageSettings>) (() => this.readStorageModeSettingsForCCFromConfig())) : (IStorageSettings) this.context.Cache.Get<StorageSettings>("DataStore.StorageModeSettingsForCC.Core", (Func<StorageSettings>) (() => this.readStorageModeSettingsForCCFromConfig()));
    }

    public void EnableRegistryMonitoring()
    {
      if (!(InstanceConfiguration.Provider is RegistryConfigurationProvider))
        return;
      this.staticSettings.SettingsChanged += new EventHandler(this.onSettingsChanged);
      this.staticSettings.EnableMonitoring();
    }

    private StorageSettings readStorageModeSettingsFromConfig()
    {
      EnGlobalSettings instanceSettings = this.GetInstanceSettings();
      return new StorageSettings()
      {
        SqlConnectionString = instanceSettings.DatabaseConnectionString,
        ClientId = this.context.ClientID,
        InstanceId = this.context.InstanceName,
        MongoActiveCredentials = instanceSettings.MongoActiveCredentials,
        MongoActiveDatabase = instanceSettings.MongoActiveDatabase,
        MongoActiveHosts = instanceSettings.MongoActiveHosts,
        MongoActiveOptions = instanceSettings.MongoActiveOptions,
        MongoArchiveCredentials = instanceSettings.MongoArchiveCredentials,
        MongoArchiveDatabase = instanceSettings.MongoArchiveDatabase,
        MongoArchiveHosts = instanceSettings.MongoArchiveHosts,
        MongoArchiveOptions = instanceSettings.MongoArchiveOptions,
        PostgreSqlConnectionString = instanceSettings.PostgreSqlConnectionString
      };
    }

    private StorageSettings readStorageModeSettingsForCCFromConfig()
    {
      EnGlobalSettings instanceSettings = this.GetInstanceSettings();
      return new StorageSettings()
      {
        SqlConnectionString = instanceSettings.DatabaseConnectionString,
        ClientId = this.context.ClientID,
        InstanceId = this.context.InstanceName,
        MongoActiveCredentials = instanceSettings.MongoActiveCredentials2,
        MongoActiveDatabase = instanceSettings.MongoActiveDatabase2,
        MongoActiveHosts = instanceSettings.MongoActiveHosts2,
        MongoActiveOptions = instanceSettings.MongoActiveOptions2,
        MongoArchiveCredentials = instanceSettings.MongoArchiveCredentials2,
        MongoArchiveDatabase = instanceSettings.MongoArchiveDatabase2,
        MongoArchiveHosts = instanceSettings.MongoArchiveHosts2,
        MongoArchiveOptions = instanceSettings.MongoArchiveOptions2
      };
    }

    private IDictionary readStorageSettings()
    {
      IDictionary serverSettings = this.GetServerSettings("DataStore");
      if (this.staticSettings.ServerStorageMode != StorageMode.Default)
        serverSettings[(object) "DataStore.StorageMode"] = (object) this.staticSettings.ServerStorageMode;
      if ((StorageMode) serverSettings[(object) "DataStore.StorageMode"] == StorageMode.Default)
        serverSettings[(object) "DataStore.StorageMode"] = (object) StorageMode.FileSystemOnly;
      return serverSettings;
    }

    private IDictionary readPipelineSearchSettings()
    {
      IDictionary serverSettings = this.GetServerSettings("Policies");
      if (this.staticSettings.ServerPipelineSearchMode != PipelineSearchMode.Default)
        serverSettings[(object) "Policies.PipelineSearchMode"] = (object) this.staticSettings.ServerStorageMode;
      if ((PipelineSearchMode) serverSettings[(object) "Policies.PipelineSearchMode"] == PipelineSearchMode.Default)
        serverSettings[(object) "Policies.PipelineSearchMode"] = (object) PipelineSearchMode.MSSQL;
      return serverSettings;
    }

    private DirectoryInfo createFolder(string folderName)
    {
      DirectoryInfo folder = new DirectoryInfo(folderName);
      if (!folder.Exists)
        folder.Create();
      return folder;
    }

    private void onSettingsChanged(object sender, EventArgs e) => this.Refresh();

    private string getFolderPath(string rootDir, string relativePath, bool createIfMissing = true)
    {
      string path = !Path.IsPathRooted(relativePath) ? Path.Combine(rootDir, relativePath) : throw new ArgumentException("The specified path must be a relative path", nameof (relativePath));
      if (createIfMissing && !Directory.Exists(path))
        Directory.CreateDirectory(path);
      return path;
    }

    private string getFilePath(string rootDir, string relativePath, bool createFolderIfMissing = true)
    {
      string path = !Path.IsPathRooted(relativePath) ? Path.Combine(rootDir, relativePath) : throw new ArgumentException("The specified path must be a relative path", nameof (relativePath));
      if (createFolderIfMissing && !Directory.Exists(Path.GetDirectoryName(path)))
        Directory.CreateDirectory(Path.GetDirectoryName(path));
      return path;
    }

    private EnGlobalSettings GetInstanceSettings() => this.context.GetEnGlobalSettings();

    public void DeleteVendorSetting(string category, string setting)
    {
      List<string> source = new List<string>()
      {
        "CLIENT",
        "eDisclosures",
        "LOCompDefault",
        "DefaultEmailTemplates",
        "REPORTING",
        "SERVICING",
        "TEMPLATE"
      };
      List<string> list = ((IEnumerable<string>) SettingDefinitions.Categories).ToList<string>();
      if (source.Contains<string>(category, (IEqualityComparer<string>) StringComparer.InvariantCultureIgnoreCase) || list.Contains<string>(category, (IEqualityComparer<string>) StringComparer.InvariantCultureIgnoreCase))
        Err.Raise(TraceLevel.Warning, nameof (ServerSettings), (ServerException) new ServerArgumentException("You can not delete a reserved system category", nameof (category)));
      if (!string.IsNullOrEmpty(setting))
        Company.DeleteCompanySetting(category.ToUpper(), setting.ToUpper());
      else
        Company.DeleteCompanySettings(category.ToUpper());
    }

    public void SetVendorSetting(string path, object value, bool checkDefinition)
    {
      this.SetServerSetting(path, value, checkDefinition, false);
    }
  }
}
