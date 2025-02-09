// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.EnGlobalSettings
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using EllieMae.EMLite.Common.Configuration;
using EllieMae.EMLite.JedLib;
using EllieMae.EMLite.Logging;
using EllieMae.Encompass.AsmResolver;
using Encompass.Diagnostics.Logging.Targets;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public class EnGlobalSettings
  {
    private const string registryRoot = "Software\\Ellie Mae";
    private const string serverServiceName = "EncompassServer";
    private const string defaultVirtualRootName = "Encompass";
    private static readonly TimeSpan defaultSqlTimeout = TimeSpan.FromSeconds(60.0);
    private static readonly int defaultMaxServerEvents = 1000;
    private static readonly int defaultLockTimeoutDuringGetCache = 600;
    private static readonly int defaultCacheRegetFromCache = 1;
    private static readonly int defaultCacheRegetFromDB = 1;
    private static readonly TimeSpan defaultSettingsSyncSqlTimeout = TimeSpan.FromSeconds(600.0);
    private static readonly TimeSpan defaultAclGroupSqlTimeout = TimeSpan.FromMinutes(10.0);
    private static readonly TimeSpan defaultConnectionTimeout = TimeSpan.FromMinutes(5.0);
    private const int defaultServerPort = 11091;
    private static b jed = (b) null;
    public static string ApplicationName = "Encompass";
    public static readonly string EncompassVersionFileName = "EncompassVersion.xml";
    private string installDir;
    private string dataDir;
    private string instanceName;
    private int messageQueueConsumerDisposeTime = -1;
    private IInstanceConfiguration instanceConfig;
    private RegistryAccessor registry;
    private RegistryMonitor monitor;
    private string _appTempDirectory;
    private bool? _allowCalcDiag;

    public static VersionDesignator VersionDesignator
    {
      get => VersionDesignator.GetInstance("Software\\Ellie Mae");
    }

    public event EventHandler SettingsChanged;

    static EnGlobalSettings()
    {
      a.a("z2r1xy8k5mp4ccpl");
      EnGlobalSettings.jed = EnGlobalSettings.CreateJed();
    }

    public EnGlobalSettings(string instanceName)
      : this(instanceName, (IInstanceConfiguration) null)
    {
    }

    public EnGlobalSettings(string instanceName, IInstanceConfiguration instanceConfig)
    {
      this.instanceName = instanceName ?? "";
      string rootPath = instanceConfig == null || !(instanceConfig.GetType() != typeof (RegistryInstanceConfiguration)) ? EnGlobalSettings.GetInstanceRootKeyPath(instanceName) : EnGlobalSettings.GetInstanceRootKeyPath("");
      try
      {
        if (Application.ExecutablePath.ToLower().EndsWith("encerdbserver.exe"))
          rootPath = "Software\\Ellie Mae\\EncompassERDB";
      }
      catch
      {
      }
      this.registry = new RegistryAccessor(Registry.LocalMachine, rootPath);
      this.instanceConfig = instanceConfig != null ? instanceConfig : (IInstanceConfiguration) new StaticInstanceConfiguration((IInstanceConfiguration) new RegistryInstanceConfiguration(this.registry));
      if (this.HttpClientCustomHeaders != null)
        return;
      this.HttpClientCustomHeaders = new Dictionary<string, string>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
    }

    public static string GetInstanceRootKeyPath(string instanceName)
    {
      string str = "Software\\Ellie Mae\\";
      if (Application.ExecutablePath.ToLower().EndsWith("versionmigration.exe") && Registry.LocalMachine.OpenSubKey("Software\\Wow6432Node\\Ellie Mae") != null)
        str = "Software\\Wow6432Node\\Ellie Mae\\";
      string instanceRootKeyPath = str + EnGlobalSettings.ApplicationName;
      VersionDesignator versionDesignator = EnGlobalSettings.VersionDesignator;
      if (!versionDesignator.IsDesignatedVersionNil)
        instanceRootKeyPath = str + versionDesignator.DesignatedVersion + "\\" + EnGlobalSettings.ApplicationName;
      if ((instanceName ?? "") != "")
        instanceRootKeyPath = instanceRootKeyPath + "$" + instanceName;
      return instanceRootKeyPath;
    }

    public string RegistryPath => this.registry.RootPath;

    public string EncompassProgramDirectory
    {
      get
      {
        if (this.installDir != null)
          return this.installDir;
        if (AssemblyResolver.IsSmartClient)
        {
          this.installDir = Application.StartupPath;
          return this.installDir;
        }
        try
        {
          this.installDir = !EnConfigurationSettings.ApplicationArgumentExists("-installdir") ? this["InstallDir", true].ToString() : Path.GetDirectoryName(EnConfigurationSettings.GetApplicationPath());
        }
        catch (Exception ex)
        {
          if (!Application.ExecutablePath.ToLower().EndsWith("encerdbserver.exe"))
            throw ex;
          this.installDir = Application.StartupPath;
        }
        return this.installDir;
      }
    }

    public string EncompassDataDirectory
    {
      get
      {
        if (this.dataDir != null)
          return this.dataDir;
        if (AssemblyResolver.IsSmartClient)
        {
          this.dataDir = Path.Combine(AssemblyResolver.AppDataHashFolder, "EncompassData");
        }
        else
        {
          this.dataDir = this.instanceConfig.EncompassDataDirectory;
          if (string.IsNullOrEmpty(this.dataDir))
          {
            if (Application.ExecutablePath.ToLower().EndsWith("encerdbserver.exe"))
            {
              this.dataDir = Path.Combine(Application.StartupPath, "ERDBData");
            }
            else
            {
              string path1 = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) ?? "";
              if (path1 == "")
                path1 = this["TempDir", (object) Path.GetTempPath()].ToString();
              this.dataDir = Path.Combine(path1, "Encompass");
            }
          }
        }
        return this.dataDir;
      }
      set
      {
        this["DataDir"] = (object) value;
        this.dataDir = value;
      }
    }

    public string AppLoanDirectory => Path.Combine(this.EncompassDataDirectory, "Loans");

    public string AppDraftLoansDirectory
    {
      get => Path.Combine(this.EncompassDataDirectory, "Applications");
    }

    public string AppDataDirectory => Path.Combine(this.EncompassDataDirectory, "Data");

    public string AppTempResourcesDirectory => Path.Combine(this.EncompassDataDirectory, "Temp");

    public string AppSettingsDirectory => Path.Combine(this.EncompassDataDirectory, "Settings");

    public string AppSettingsReportDownloadsDirectory
    {
      get
      {
        string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads\\Encompass");
        if (!Directory.Exists(path))
          Directory.CreateDirectory(path);
        return path;
      }
    }

    public string KafkaCertFolderLocation
    {
      get
      {
        string path = Path.Combine(this.installDir, "Kafka\\Certificates");
        if (!Directory.Exists(path))
          Directory.CreateDirectory(path);
        return path;
      }
    }

    public string ServerUpdatesDirectory
    {
      get
      {
        string path = Path.Combine(this.AppSettingsDirectory, "Updates\\ServerUpdates");
        if (!Directory.Exists(path))
          Directory.CreateDirectory(path);
        return path;
      }
    }

    public string GetShusDirectory(Version encMajorVersion)
    {
      string path = Path.Combine(this.ServerUpdatesDirectory, "v" + (object) encMajorVersion.Major + "." + (object) encMajorVersion.Minor + "." + (object) encMajorVersion.Build);
      if (!Directory.Exists(path))
        Directory.CreateDirectory(path);
      return path;
    }

    public string AppTempDirectory
    {
      get
      {
        if (this._appTempDirectory == null)
        {
          string path1 = this["TempDir", (object) Path.GetTempPath()].ToString();
          if (!path1.EndsWith("\\"))
            path1 += "\\";
          if (path1.IndexOf("\\EncompassSC\\") < 0)
            path1 = Path.Combine(path1, "Encompass\\Temp");
          this._appTempDirectory = path1;
        }
        return this._appTempDirectory;
      }
    }

    public string AppSharedSettingsDirectory
    {
      get
      {
        return this["SettingsDir", (object) Path.Combine(this.AppSettingsDirectory, "LocalSettings")].ToString();
      }
    }

    public string AppBaseLogDirectory
    {
      get
      {
        string baseLogDirectory = this.instanceConfig.LogDirectory;
        if (string.IsNullOrEmpty(baseLogDirectory))
          baseLogDirectory = Path.Combine(this.AppSettingsDirectory, "Logs");
        return baseLogDirectory;
      }
    }

    public string AppLogDirectory
    {
      get
      {
        return EnConfigurationSettings.ApplicationArgumentExists("-logsid") ? this.GetUniqueLogDirectoryForSession(EnConfigurationSettings.ApplicationSessionID) : this.AppBaseLogDirectory;
      }
    }

    public string GetUniqueLogDirectoryForSession(string sessionId)
    {
      return Path.Combine(this.AppBaseLogDirectory, sessionId);
    }

    public string AppLoanAutosaveDirectory
    {
      get
      {
        string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Encompass\\Autosave");
        if (!Directory.Exists(path))
          Directory.CreateDirectory(path);
        return path;
      }
    }

    public string AppBackgroundConversionDirectory
    {
      get
      {
        string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Encompass\\BackgroundConversion");
        if (!Directory.Exists(path))
          Directory.CreateDirectory(path);
        return path;
      }
    }

    public string AppBackgroundUploadDirectory
    {
      get
      {
        string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Encompass\\BackgroundUpload");
        if (!Directory.Exists(path))
          Directory.CreateDirectory(path);
        return path;
      }
    }

    public string VersionInformationFile
    {
      get => Path.Combine(this.EncompassProgramDirectory, "EncompassVersion.xml");
    }

    public RuntimeEnvironment RuntimeEnvironment
    {
      get => this.GetKeyAsEnum<RuntimeEnvironment>("Environment", RuntimeEnvironment.Default);
    }

    public InstallationMode InstallationMode
    {
      get
      {
        try
        {
          if (AssemblyResolver.IsSmartClient)
            return InstallationMode.Client;
          string str = this["InstallMode"].ToString();
          return str == "Personal" ? InstallationMode.Local : (InstallationMode) Enum.Parse(typeof (InstallationMode), str, true);
        }
        catch
        {
          return InstallationMode.Local;
        }
      }
      set => this["InstallMode"] = (object) value.ToString();
    }

    public ServerMode ServerMode
    {
      get => this.GetKeyAsEnum<ServerMode>("Server\\Mode", ServerMode.None);
      set
      {
        if (value == ServerMode.None)
          this["Server\\Mode"] = (object) "";
        else
          this["Server\\Mode"] = (object) value.ToString();
      }
    }

    public string IIsVirtualRootName
    {
      get
      {
        this.migrateLegacyIIsSettings();
        return this["Server\\HTTP\\IIsVirtualRoot", (object) ""].ToString();
      }
      set => this["Server\\HTTP\\IIsVirtualRoot"] = (object) value;
    }

    public string IIsWebSiteID
    {
      get
      {
        this.migrateLegacyIIsSettings();
        return this["Server\\HTTP\\IIsWebSite", (object) ""].ToString();
      }
      set => this["Server\\HTTP\\IIsWebSite"] = (object) value;
    }

    public string IIsDefaultVirtualRootName
    {
      get => this.instanceName == "" ? "Encompass" : "Encompass_" + this.instanceName;
    }

    public int DefaultServerPortNumber => 11091;

    public string ServerServiceName
    {
      get => this.instanceName == "" ? "EncompassServer" : "EncompassServer$" + this.instanceName;
    }

    public int ServerPortNumber
    {
      get => this.GetKeyAsInt("Server\\TCP\\Port", this.DefaultServerPortNumber);
      set => this["Server\\TCP\\Port"] = (object) value.ToString();
    }

    public bool ServerEncryption
    {
      get => this.GetKeyAsInt("Server\\Encryption", 0) == 1;
      set => this["Server\\Encryption"] = value ? (object) "1" : (object) "0";
    }

    public bool ServerCompression
    {
      get => this.GetKeyAsInt("Server\\Compression", 0) == 1;
      set => this["Server\\Compression"] = value ? (object) "1" : (object) "0";
    }

    public bool ServerDiagnosticsInterfaceEnabled
    {
      get => this.GetKeyAsInt("Server\\Diagnostics", 1) == 1;
      set => this["Server\\Diagnostics"] = value ? (object) "1" : (object) "0";
    }

    public TimeSpan ServerConnectionTimeout
    {
      get
      {
        return TimeSpan.FromSeconds((double) this.GetKeyAsInt("Server\\SessionTimeout", (int) EnGlobalSettings.defaultConnectionTimeout.TotalSeconds));
      }
      set => this["Server\\SessionTimeout"] = (object) string.Concat((object) value);
    }

    public StorageMode ServerStorageMode
    {
      get => this.GetKeyAsEnum<StorageMode>("Server\\StorageMode", StorageMode.Default);
      set => this["Server\\StorageMode"] = (object) value.ToString();
    }

    public PipelineSearchMode ServerPipelineSearchMode
    {
      get
      {
        return this.GetKeyAsEnum<PipelineSearchMode>("Server\\PipelineSearchMode", PipelineSearchMode.Default);
      }
      set => this["Server\\PipelineSearchMode"] = (object) value.ToString();
    }

    public IDictionary GetDefaultTCPClientChannelProperties()
    {
      return (IDictionary) new Hashtable()
      {
        [(object) "name"] = (object) "gtcp-client",
        [(object) "NoSizeChecking"] = (object) "true",
        [(object) "MaxQueuedItems"] = (object) "5000",
        [(object) "ConnectTimeout"] = (object) "20000",
        [(object) "SleepBetweenReconnections"] = (object) "2000",
        [(object) "MaxTimeSpanToReconnect"] = (object) "120000",
        [(object) "InvocationTimeout"] = (object) "120000000",
        [(object) "Compression"] = (object) "false"
      };
    }

    public IDictionary GetTCPClientChannelProperties()
    {
      string propString = this["Client\\TCP\\ChannelProperties", (object) ""].ToString();
      return propString == "" ? this.GetDefaultTCPClientChannelProperties() : EnGlobalSettings.ParseChannelProperties(propString);
    }

    public IDictionary GetDefaultHTTPClientChannelProperties()
    {
      return (IDictionary) new Hashtable()
      {
        [(object) "name"] = (object) "ghttp-client",
        [(object) "NoSizeChecking"] = (object) "true",
        [(object) "MaxQueuedItems"] = (object) "5000",
        [(object) "ConnectTimeout"] = (object) "40000",
        [(object) "SleepBetweenReconnections"] = (object) "2000",
        [(object) "MaxTimespanToReconnect"] = (object) "120000",
        [(object) "InvocationTimeout"] = (object) "120000000",
        [(object) "Compression"] = (object) "false",
        [(object) "HttpUseGlobalProxy"] = (object) "true",
        [(object) "HttpUseDefaultCredentials"] = (object) "true"
      };
    }

    public IDictionary GetHTTPClientChannelProperties()
    {
      string propString = this["Client\\HTTP\\ChannelProperties", (object) ""].ToString();
      return propString == "" ? this.GetDefaultHTTPClientChannelProperties() : EnGlobalSettings.ParseChannelProperties(propString);
    }

    public Dictionary<string, string> HttpClientCustomHeaders { get; private set; }

    public IDictionary GetDefaultTCPServerChannelProperties()
    {
      return (IDictionary) new Hashtable()
      {
        [(object) "name"] = (object) "gtcp",
        [(object) "NoSizeChecking"] = (object) "true",
        [(object) "MaxQueuedItems"] = (object) "5000",
        [(object) "MaxTimespanToReconnect"] = (object) "120000",
        [(object) "Compression"] = (object) "false"
      };
    }

    public IDictionary GetTCPServerChannelProperties()
    {
      string propString = this["Server\\TCP\\ChannelProperties", (object) ""].ToString();
      return propString == "" ? this.GetDefaultTCPServerChannelProperties() : EnGlobalSettings.ParseChannelProperties(propString);
    }

    public IDictionary GetDefaultHTTPServerChannelProperties()
    {
      return (IDictionary) new Hashtable()
      {
        [(object) "name"] = (object) "ghttp",
        [(object) "NoSizeChecking"] = (object) "true",
        [(object) "MaxQueuedItems"] = (object) "5000",
        [(object) "MaxTimespanToReconnect"] = (object) "120000",
        [(object) "Compression"] = (object) "false"
      };
    }

    public IDictionary GetHTTPServerChannelProperties()
    {
      string propString = this["Server\\HTTP\\ChannelProperties", (object) ""].ToString();
      return propString == "" ? this.GetDefaultHTTPServerChannelProperties() : EnGlobalSettings.ParseChannelProperties(propString);
    }

    public IPAddressRange[] ServerIPAddressRestrictions
    {
      get
      {
        string addrString = this["Server\\TCP\\IPRestrictions", (object) ""].ToString();
        return addrString == "" ? new IPAddressRange[0] : EnGlobalSettings.parseIpAddressRanges(addrString);
      }
      set
      {
        this["Server\\TCP\\IPRestrictions"] = (object) EnGlobalSettings.joinIpAddressRanges(value);
      }
    }

    public string EncompassPlatformAPI => this.instanceConfig.EncompassPlatformAPI;

    public string MongoActiveHosts => this.instanceConfig.MongoActiveHosts;

    public string MongoActiveDatabase => this.instanceConfig.MongoActiveDatabase;

    public string MongoActiveCredentials => this.instanceConfig.MongoActiveCredentials;

    public string MongoActiveOptions => this.instanceConfig.MongoActiveOptions;

    public string MongoArchiveHosts => this.instanceConfig.MongoArchiveHosts;

    public string MongoArchiveDatabase => this.instanceConfig.MongoArchiveDatabase;

    public string MongoArchiveCredentials => this.instanceConfig.MongoArchiveCredentials;

    public string MongoArchiveOptions => this.instanceConfig.MongoArchiveOptions;

    public string PostgresDatabaseHost
    {
      get => this.instanceConfig.PostgresDatabaseHost;
      set => EnGlobalSettings.toRegistryConfig(this.instanceConfig).PostgresDatabaseHost = value;
    }

    public string PostgresDatabaseName
    {
      get => this.instanceConfig.PostgresDatabaseName;
      set => EnGlobalSettings.toRegistryConfig(this.instanceConfig).PostgresDatabaseName = value;
    }

    public string PostgresDatabasePassword
    {
      get => this.instanceConfig.PostgresDatabasePassword;
      set
      {
        EnGlobalSettings.toRegistryConfig(this.instanceConfig).PostgresDatabasePassword = value;
      }
    }

    public string PostgresDatabasePort
    {
      get => this.instanceConfig.PostgresDatabasePort;
      set => EnGlobalSettings.toRegistryConfig(this.instanceConfig).PostgresDatabasePort = value;
    }

    public string PostgresDatabaseUsername
    {
      get => this.instanceConfig.PostgresDatabaseUsername;
      set
      {
        EnGlobalSettings.toRegistryConfig(this.instanceConfig).PostgresDatabaseUsername = value;
      }
    }

    public string PostgresDatabaseConnectionStringFormat
    {
      get
      {
        return this["Postgres\\ConnectionString", (object) "Host={0};Port={1};Username={2};Password={3};Database={4}"].ToString();
      }
      set => this["Postgres\\ConnectionString"] = (object) value;
    }

    public string PostgreSqlConnectionString
    {
      get
      {
        try
        {
          return string.Format(this.PostgresDatabaseConnectionStringFormat, (object) this.PostgresDatabaseHost, (object) this.PostgresDatabasePort, (object) this.PostgresDatabaseUsername, (object) this.PostgresDatabasePassword, (object) this.PostgresDatabaseName);
        }
        catch
        {
          return "";
        }
      }
    }

    public string MongoActiveHosts2 => this.instanceConfig.MongoActiveHosts2;

    public string MongoActiveDatabase2 => this.instanceConfig.MongoActiveDatabase2;

    public string MongoActiveCredentials2 => this.instanceConfig.MongoActiveCredentials2;

    public string MongoActiveOptions2 => this.instanceConfig.MongoActiveOptions2;

    public string MongoArchiveHosts2 => this.instanceConfig.MongoArchiveHosts2;

    public string MongoArchiveDatabase2 => this.instanceConfig.MongoArchiveDatabase2;

    public string MongoArchiveCredentials2 => this.instanceConfig.MongoArchiveCredentials2;

    public string MongoArchiveOptions2 => this.instanceConfig.MongoArchiveOptions2;

    public string TPOAdminSiteUrl => this.instanceConfig.TPOAdminSiteUrl;

    public DbServerType DatabaseType
    {
      get => this.instanceConfig.DatabaseType;
      set
      {
        this.instanceConfig.DatabaseType = value;
        EnGlobalSettings.toRegistryConfig(this.instanceConfig).DatabaseType = value;
      }
    }

    public string DatabaseServer
    {
      get => this.instanceConfig.DatabaseServer;
      set => EnGlobalSettings.toRegistryConfig(this.instanceConfig).DatabaseServer = value;
    }

    public string DatabaseAGListener
    {
      get => this.instanceConfig.DatabaseAGListener;
      set => EnGlobalSettings.toRegistryConfig(this.instanceConfig).DatabaseAGListener = value;
    }

    public bool DisabledAGListener
    {
      get => this.instanceConfig.DisabledAGListener;
      set => EnGlobalSettings.toRegistryConfig(this.instanceConfig).DisabledAGListener = value;
    }

    public string Port
    {
      get => this.instanceConfig.Port;
      set => EnGlobalSettings.toRegistryConfig(this.instanceConfig).Port = value;
    }

    public string DatabaseName
    {
      get => this.instanceConfig.DatabaseName;
      set => EnGlobalSettings.toRegistryConfig(this.instanceConfig).DatabaseName = value;
    }

    public string DatabaseUserID
    {
      get => this.instanceConfig.DatabaseUserID;
      set => EnGlobalSettings.toRegistryConfig(this.instanceConfig).DatabaseUserID = value;
    }

    public string DatabasePassword
    {
      get => this.instanceConfig.DatabasePassword;
      set => EnGlobalSettings.toRegistryConfig(this.instanceConfig).DatabasePassword = value;
    }

    public string DatabaseConnectionStringFormat
    {
      get
      {
        return this["MSDE\\ConnectionString", (object) "Data Source={0};Initial Catalog={1};User ID={2};Password={3};Pooling=true;Min Pool Size=1;Max Pool Size=250;Connect Timeout=15;"].ToString();
      }
      set => this["MSDE\\ConnectionString"] = (object) value;
    }

    public string DatabaseConnectionString
    {
      get
      {
        try
        {
          return new SqlConnectionStringBuilder(string.Format(this.DatabaseConnectionStringFormat, (object) this.DatabaseServer, (object) this.DatabaseName, (object) this.DatabaseUserID, (object) this.DatabasePassword)).ConnectionString;
        }
        catch (Exception ex)
        {
          return (string) null;
        }
      }
    }

    public string ReadReplicaConnectionString
    {
      get
      {
        try
        {
          bool flag = !string.IsNullOrWhiteSpace(this.DatabaseAGListener) && !this.DisabledAGListener;
          SqlConnectionStringBuilder connectionStringBuilder = new SqlConnectionStringBuilder(string.Format(this.DatabaseConnectionStringFormat, flag ? (object) this.DatabaseAGListener : (object) this.DatabaseServer, (object) this.DatabaseName, (object) this.DatabaseUserID, (object) this.DatabasePassword));
          if (flag)
            connectionStringBuilder.ApplicationIntent = ApplicationIntent.ReadOnly;
          return connectionStringBuilder.ConnectionString;
        }
        catch (Exception ex)
        {
          return (string) null;
        }
      }
    }

    public int MaxServerEvents
    {
      get => this.GetKeyAsInt(nameof (MaxServerEvents), EnGlobalSettings.defaultMaxServerEvents);
    }

    public int LockTimeoutDuringGetCache
    {
      get
      {
        return this.GetKeyAsInt(nameof (LockTimeoutDuringGetCache), EnGlobalSettings.defaultLockTimeoutDuringGetCache);
      }
    }

    public int CacheRegetFromCache
    {
      get
      {
        return this.GetKeyAsInt(nameof (CacheRegetFromCache), EnGlobalSettings.defaultCacheRegetFromCache);
      }
    }

    public int CacheRegetFromDB
    {
      get => this.GetKeyAsInt(nameof (CacheRegetFromDB), EnGlobalSettings.defaultCacheRegetFromDB);
    }

    public TimeSpan SQLTimeout
    {
      get
      {
        return TimeSpan.FromSeconds((double) this.GetKeyAsInt("MSDE\\Timeout", (int) EnGlobalSettings.defaultSqlTimeout.TotalSeconds));
      }
      set => this["MSDE\\Timeout"] = (object) string.Concat((object) value);
    }

    public TimeSpan SettingsSyncSQLTimeout
    {
      get
      {
        return TimeSpan.FromSeconds((double) this.GetKeyAsInt("MSDE\\SettingsSyncTimeout", (int) EnGlobalSettings.defaultSettingsSyncSqlTimeout.TotalSeconds));
      }
      set => this["MSDE\\SettingsSycTimeout"] = (object) string.Concat((object) value);
    }

    public TimeSpan ReportSQLTimeout
    {
      get
      {
        return TimeSpan.FromSeconds((double) this.GetKeyAsInt("MSDE\\ReportTimeout", 2 * (int) EnGlobalSettings.defaultSqlTimeout.TotalSeconds));
      }
      set => this["MSDE\\ReportTimeout"] = (object) string.Concat((object) value);
    }

    public TimeSpan AclGroupSQLTimeout
    {
      get
      {
        return TimeSpan.FromSeconds((double) this.GetKeyAsInt("MSDE\\AclGroupTimeout", (int) EnGlobalSettings.defaultAclGroupSqlTimeout.TotalSeconds));
      }
      set => this["MSDE\\AclGroupTimeout"] = (object) string.Concat((object) value);
    }

    public byte[] SDKLicense
    {
      get => (byte[]) this["SDK\\License"];
      set => this["SDK\\License"] = (object) value;
    }

    public string PersonalEditionUserID
    {
      get => this["PEUID", (object) "admin"].ToString();
      set => this["PEUID"] = (object) value;
    }

    public bool AllowRecreateRDB
    {
      get => string.Concat(this[nameof (AllowRecreateRDB)]) == "1";
      set => this[nameof (AllowRecreateRDB)] = value ? (object) "1" : (object) "0";
    }

    public bool AllowCalculationDiagnostic
    {
      get
      {
        if (!this._allowCalcDiag.HasValue)
          this._allowCalcDiag = new bool?(string.Concat(this[nameof (AllowCalculationDiagnostic)]) == "1");
        return this._allowCalcDiag.Value;
      }
      set
      {
        this[nameof (AllowCalculationDiagnostic)] = value ? (object) "1" : (object) "0";
        this._allowCalcDiag = new bool?(value);
      }
    }

    public bool Debug
    {
      get
      {
        return string.Concat(this[nameof (Debug)]) == "1" || EnConfigurationSettings.ApplicationArgumentExists("-debug");
      }
      set => this[nameof (Debug)] = value ? (object) "1" : (object) "0";
    }

    public bool DebugDoNotAskIndicator
    {
      get => string.Concat(this[nameof (DebugDoNotAskIndicator)]) == "1";
      set => this[nameof (DebugDoNotAskIndicator)] = value ? (object) "1" : (object) "0";
    }

    public bool APITrace
    {
      get => string.Concat(this[nameof (APITrace)]) != "0";
      set => this[nameof (APITrace)] = value ? (object) "1" : (object) "0";
    }

    public bool SQLTrace
    {
      get => string.Concat(this[nameof (SQLTrace)]) == "1";
      set => this[nameof (SQLTrace)] = value ? (object) "1" : (object) "0";
    }

    public bool SQLTrace_IncludeAccessorName
    {
      get => string.Concat(this[nameof (SQLTrace_IncludeAccessorName)]) == "1";
      set => this[nameof (SQLTrace_IncludeAccessorName)] = value ? (object) "1" : (object) "0";
    }

    public bool ServiceAPITrace
    {
      get => string.Concat(this[nameof (ServiceAPITrace), (object) "1"]) == "1";
      set => this[nameof (ServiceAPITrace)] = value ? (object) "1" : (object) "0";
    }

    public bool GCTrace
    {
      get
      {
        return string.Concat(this[nameof (GCTrace)]) == "1" || EnConfigurationSettings.ApplicationArgumentExists("-gctrace");
      }
      set => this[nameof (GCTrace)] = value ? (object) "1" : (object) "0";
    }

    public bool ShowEnhancedMetrics
    {
      get
      {
        return !(string.Concat(this[nameof (ShowEnhancedMetrics)]) == "") && string.Concat(this[nameof (ShowEnhancedMetrics)]) == "1";
      }
      set => this[nameof (ShowEnhancedMetrics)] = value ? (object) "1" : (object) "0";
    }

    public bool PGRLSEnabled
    {
      get => string.Concat(this[nameof (PGRLSEnabled), (object) "1"]) == "1";
      set => this[nameof (PGRLSEnabled)] = value ? (object) "1" : (object) "0";
    }

    public bool EnableMetrics => false;

    public bool UseEnhnacedConditions
    {
      get => ConfigurationManager.AppSettings["EnableEnchancedConditions"] == "Y";
    }

    public int MessageQueueConsumerDisposeTimeout
    {
      get
      {
        if (this.messageQueueConsumerDisposeTime > 0)
          return this.messageQueueConsumerDisposeTime;
        this.messageQueueConsumerDisposeTime = this.GetKeyAsInt("MessageQueueConsumerDisposeTime", 300);
        return this.messageQueueConsumerDisposeTime;
      }
    }

    public bool AutoSyncFieldList
    {
      get => string.Concat(this["AutoSyncFields"]) == "1";
      set => this["AutoSyncFields"] = value ? (object) "1" : (object) "0";
    }

    public FileLogRolloverFrequency ServerLogRolloverFrequency
    {
      get
      {
        return this.GetKeyAsEnum<FileLogRolloverFrequency>("Server\\LogRollover", FileLogRolloverFrequency.None);
      }
      set => this["Server\\LogRollover"] = (object) value.ToString();
    }

    public ServerLogMode ServerLogMode
    {
      get => this.GetKeyAsEnum<ServerLogMode>("Server\\LogMode", ServerLogMode.LogPerInstance);
      set => this["Server\\LogMode"] = (object) value.ToString();
    }

    public TimeSpan APILatency
    {
      get => TimeSpan.FromMilliseconds((double) this.GetKeyAsInt(nameof (APILatency), 0));
      set => this[nameof (APILatency)] = (object) ((int) value.TotalMilliseconds).ToString();
    }

    public TimeSpan SQLLatency
    {
      get => TimeSpan.FromMilliseconds((double) this.GetKeyAsInt(nameof (SQLLatency), 0));
      set => this[nameof (SQLLatency)] = (object) ((int) value.TotalMilliseconds).ToString();
    }

    public bool Disabled => this.instanceConfig.Disabled;

    public HotfixOverrideOption HotfixOverrideOption
    {
      get
      {
        if (AssemblyResolver.IsSmartClient || string.Concat(this["ApplyHotfixes"]) == "0")
          return HotfixOverrideOption.NeverApply;
        return string.Concat(this["ApplyHotfixes"]) == "1" ? HotfixOverrideOption.AlwaysApply : HotfixOverrideOption.None;
      }
    }

    public string RMIPassword
    {
      get
      {
        try
        {
          object obj = this["RMI\\Credentials"];
          return obj != null ? EnGlobalSettings.decryptString(obj.ToString()) : string.Empty;
        }
        catch
        {
          return string.Empty;
        }
      }
      set => this["RMI\\Credentials"] = (object) EnGlobalSettings.encryptString(value);
    }

    public int TaskProcessingThreadCount
    {
      get => this.GetKeyAsInt("Server\\TaskProcessingThreads", 2);
      set => this["Server\\TaskProcessingThreads"] = (object) string.Concat((object) value);
    }

    public TimeSpan ServerActivityTimeout
    {
      get => TimeSpan.FromSeconds((double) this.GetKeyAsInt("Server\\ActivityTimeout", 3600));
      set => this["Server\\ActivityTimeout"] = (object) value.TotalSeconds.ToString("#");
    }

    public TimeSpan ServerActivityTimerInterval
    {
      get => TimeSpan.FromSeconds((double) this.GetKeyAsInt("Server\\ActivityTimerInterval", 600));
      set => this["Server\\ActivityTimerInterval"] = (object) value.TotalSeconds.ToString("#");
    }

    public TimeSpan InstanceConfigRefreshInterval
    {
      get
      {
        return TimeSpan.FromSeconds((double) this.GetKeyAsInt(nameof (InstanceConfigRefreshInterval), 900));
      }
      set
      {
        this[nameof (InstanceConfigRefreshInterval)] = (object) value.TotalSeconds.ToString("#");
      }
    }

    public bool TaskSchedulerDisabled
    {
      get => string.Concat(this["Scheduler\\Disabled"]) == "1";
      set => this["Scheduler\\Disabled"] = value ? (object) "1" : (object) "0";
    }

    public TimeSpan TaskSchedulerInterval
    {
      get => TimeSpan.FromSeconds((double) this.GetKeyAsInt("Scheduler\\Interval", 60));
      set => this["Scheduler\\Interval"] = (object) value.TotalSeconds.ToString("#");
    }

    internal bool DisableCrossAppDomainRemoting
    {
      get => string.Concat(this[nameof (DisableCrossAppDomainRemoting)]) == "1";
    }

    public int MaxHazelCastClients
    {
      get => this.GetKeyAsInt("Server\\MaxHazelCastClients", 1);
      set => this["Server\\MaxHazelCastClients"] = (object) string.Concat((object) value);
    }

    public int MaxHazelCastRequestsPerConnection
    {
      get => this.GetKeyAsInt("Server\\MaxHazelCastRequestsPerConnection", 5);
      set
      {
        this["Server\\MaxHazelCastRequestsPerConnection"] = (object) string.Concat((object) value);
      }
    }

    public int HazelcastUnusedConnectionShutdownIdleTimeInSeconds
    {
      get => this.GetKeyAsInt("Server\\HazelcastUnusedConnectionShutdownIdleTimeInSeconds", 120);
      set
      {
        this["Server\\HazelcastUnusedConnectionShutdownIdleTimeInSeconds"] = (object) string.Concat((object) value);
      }
    }

    public int TraceLocks => this.GetKeyAsInt(nameof (TraceLocks), 0);

    public object this[string key]
    {
      get => this.registry.ReadValue(key);
      set => this.registry.WriteValue(key, value);
    }

    public object this[string key, object defaultValue]
    {
      get => this.registry.ReadValue(key, defaultValue);
    }

    private object this[string key, bool required]
    {
      get
      {
        object obj = this[key];
        return !required || obj != null ? obj : throw new Exception("Missing required registry value \"" + key + "\".");
      }
    }

    public void EnableMonitoring()
    {
      if (this.monitor != null)
        return;
      this.monitor = new RegistryMonitor("HKLM\\" + this.registry.RootPath);
      this.monitor.RegChangeNotifyFilter = RegChangeNotifyFilter.Key | RegChangeNotifyFilter.Value;
      this.monitor.RegChanged += new EventHandler(this.onRegistryChangeDetected);
      this.monitor.Start();
    }

    public void DisableMonitoring()
    {
      if (this.monitor == null)
        return;
      this.monitor.Stop();
      this.monitor.Dispose();
      this.monitor = (RegistryMonitor) null;
    }

    public bool Exists()
    {
      try
      {
        return string.Concat(this["DataDir", (object) ""]) != "";
      }
      catch
      {
        return false;
      }
    }

    public static string[] GetAllInstanceNames() => EnGlobalSettings.GetAllInstanceNames(false);

    public static string[] GetAllInstanceNames(bool enabledOnly)
    {
      ArrayList arrayList = new ArrayList();
      using (RegistryKey registryKey1 = Registry.LocalMachine.OpenSubKey("Software\\Ellie Mae"))
      {
        if (registryKey1 != null)
        {
          foreach (string subKeyName in registryKey1.GetSubKeyNames())
          {
            string str = (string) null;
            if (subKeyName.ToLower() == "encompass")
              str = "";
            else if (subKeyName.ToLower().StartsWith("encompass$") && subKeyName.Length > 10)
              str = subKeyName.Substring(10);
            if (str != null)
            {
              bool flag = true;
              using (RegistryKey registryKey2 = registryKey1.OpenSubKey(subKeyName))
              {
                if (enabledOnly)
                  flag = string.Concat(registryKey2.GetValue("Disabled")) != "1";
                flag = flag && string.Concat(registryKey2.GetValue("DataDir")) != "";
              }
              if (flag)
                arrayList.Add((object) str);
            }
          }
        }
      }
      arrayList.Sort();
      return (string[]) arrayList.ToArray(typeof (string));
    }

    public static EnGlobalSettings CreateInstance(string instanceName)
    {
      EnGlobalSettings enGlobalSettings = new EnGlobalSettings("");
      using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Ellie Mae", true))
      {
        string str = "Encompass$" + instanceName;
        if (registryKey.OpenSubKey(str) == null)
          registryKey.CreateSubKey(str);
      }
      return new EnGlobalSettings(instanceName)
      {
        ["InstallDir"] = enGlobalSettings["InstallDir"],
        ["Version"] = enGlobalSettings["Version"],
        ["InstallMode"] = enGlobalSettings["InstallMode"]
      };
    }

    public static void DeleteInstance(string instanceName)
    {
      using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Ellie Mae", true))
      {
        string subkey = "Encompass$" + instanceName;
        try
        {
          registryKey.DeleteSubKeyTree(subkey);
        }
        catch
        {
        }
      }
    }

    public static string GenerateCredentials(string password)
    {
      return EnGlobalSettings.encryptString(password);
    }

    public static IDictionary ParseChannelProperties(string propString)
    {
      Hashtable channelProperties = new Hashtable();
      string str1 = propString;
      char[] chArray1 = new char[1]{ ';' };
      foreach (string str2 in str1.Split(chArray1))
      {
        char[] chArray2 = new char[1]{ '=' };
        string[] strArray = str2.Split(chArray2);
        if (strArray.Length == 2)
          channelProperties.Add((object) strArray[0], (object) strArray[1]);
      }
      return (IDictionary) channelProperties;
    }

    private int GetKeyAsInt(string key, int defaultValue)
    {
      int keyAsInt = defaultValue;
      int result;
      if (int.TryParse(string.Concat(this[key, (object) defaultValue]), out result))
        keyAsInt = result;
      return keyAsInt;
    }

    private TEnum GetKeyAsEnum<TEnum>(string key, TEnum defaultValue) where TEnum : struct
    {
      TEnum keyAsEnum = defaultValue;
      TEnum result;
      if (Enum.TryParse<TEnum>(string.Concat(this[key, (object) ""]), true, out result))
        keyAsEnum = result;
      return keyAsEnum;
    }

    private static string joinChannelProperties(IDictionary props)
    {
      string str = "";
      foreach (DictionaryEntry prop in props)
      {
        if (str.Length > 0)
          str += ";";
        str = str + prop.Key.ToString() + "=" + prop.Value.ToString();
      }
      return str;
    }

    private static IPAddressRange[] parseIpAddressRanges(string addrString)
    {
      ArrayList arrayList = new ArrayList();
      string str1 = addrString;
      char[] chArray = new char[1]{ ';' };
      foreach (string str2 in str1.Split(chArray))
      {
        try
        {
          arrayList.Add((object) IPAddressRange.Parse(str2));
        }
        catch
        {
        }
      }
      return (IPAddressRange[]) arrayList.ToArray(typeof (IPAddressRange));
    }

    private static string joinIpAddressRanges(IPAddressRange[] ranges)
    {
      if (ranges == null)
        return "";
      string str = "";
      for (int index = 0; index < ranges.Length; ++index)
        str = str + (index != 0 ? ";" : "") + ranges[index].ToString();
      return str;
    }

    public static string encryptString(string plainText)
    {
      lock (EnGlobalSettings.jed)
      {
        EnGlobalSettings.jed.b();
        return Convert.ToBase64String(EnGlobalSettings.jed.b(plainText));
      }
    }

    public static string decryptString(string cipherText)
    {
      lock (EnGlobalSettings.jed)
      {
        EnGlobalSettings.jed.b();
        return EnGlobalSettings.jed.a((Stream) new MemoryStream(Convert.FromBase64String(cipherText)));
      }
    }

    private void onRegistryChangeDetected(object sender, EventArgs e)
    {
      lock (this)
      {
        if (this.SettingsChanged == null)
          return;
        this.SettingsChanged((object) this, EventArgs.Empty);
      }
    }

    private static b CreateJed() => a.b("z5cty6u5dj3bd8");

    private void migrateLegacyIIsSettings()
    {
      string input = this["Server\\HTTP\\IIsVirtualDirPath", (object) ""].ToString();
      if (input == "")
        return;
      Match match = new Regex(".*/W3SVC/(?<site>[0-9]+)/Root/(?<vroot>.*)", RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Singleline).Match(input);
      if (match.Success)
      {
        this.IIsVirtualRootName = match.Groups["vroot"].Value;
        this.IIsWebSiteID = match.Groups["site"].Value;
      }
      this["Server\\HTTP\\IIsVirtualDirPath"] = (object) "";
    }

    private static RegistryInstanceConfiguration toRegistryConfig(IInstanceConfiguration config)
    {
      return config is RegistryInstanceConfiguration instanceConfiguration ? instanceConfiguration : throw new Exception("The current instance configuration does not support this operation");
    }
  }
}
