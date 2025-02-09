// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Configuration.StaticInstanceConfiguration
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Common.Configuration
{
  [Serializable]
  public class StaticInstanceConfiguration : IInstanceConfiguration
  {
    public StaticInstanceConfiguration(IInstanceConfiguration baseConfig)
    {
      this.ObjectID = Guid.NewGuid();
      this.Disabled = baseConfig.Disabled;
      this.EncompassDataDirectory = baseConfig.EncompassDataDirectory;
      this.LogDirectory = baseConfig.LogDirectory;
      this.DatabaseType = baseConfig.DatabaseType;
      this.DatabaseServer = baseConfig.DatabaseServer;
      this.DatabaseAGListener = baseConfig.DatabaseAGListener;
      this.DisabledAGListener = baseConfig.DisabledAGListener;
      this.Port = baseConfig.Port;
      this.DatabaseName = baseConfig.DatabaseName;
      this.DatabaseUserID = baseConfig.DatabaseUserID;
      this.DatabasePassword = baseConfig.DatabasePassword;
      this.MongoActiveHosts = baseConfig.MongoActiveHosts;
      this.TPOAdminSiteUrl = baseConfig.TPOAdminSiteUrl;
      this.MongoActiveDatabase = baseConfig.MongoActiveDatabase;
      this.MongoActiveCredentials = baseConfig.MongoActiveCredentials;
      this.MongoActiveOptions = baseConfig.MongoActiveOptions;
      this.MongoArchiveHosts = baseConfig.MongoArchiveHosts;
      this.MongoArchiveDatabase = baseConfig.MongoArchiveDatabase;
      this.MongoArchiveCredentials = baseConfig.MongoArchiveCredentials;
      this.MongoArchiveOptions = baseConfig.MongoArchiveOptions;
      this.EncompassPlatformAPI = baseConfig.EncompassPlatformAPI;
      this.DesignatedVersion = baseConfig.DesignatedVersion;
      this.PostgresDatabaseHost = baseConfig.PostgresDatabaseHost;
      this.PostgresDatabaseName = baseConfig.PostgresDatabaseName;
      this.PostgresDatabasePassword = baseConfig.PostgresDatabasePassword;
      this.PostgresDatabasePort = baseConfig.PostgresDatabasePort;
      this.PostgresDatabaseUsername = baseConfig.PostgresDatabaseUsername;
      this.MongoActiveHosts2 = baseConfig.MongoActiveHosts2;
      this.MongoActiveDatabase2 = baseConfig.MongoActiveDatabase2;
      this.MongoActiveCredentials2 = baseConfig.MongoActiveCredentials2;
      this.MongoActiveOptions2 = baseConfig.MongoActiveOptions2;
      this.MongoArchiveHosts2 = baseConfig.MongoArchiveHosts2;
      this.MongoArchiveDatabase2 = baseConfig.MongoArchiveDatabase2;
      this.MongoArchiveCredentials2 = baseConfig.MongoArchiveCredentials2;
      this.MongoArchiveOptions2 = baseConfig.MongoArchiveOptions2;
    }

    public Guid ObjectID { get; private set; }

    public bool Disabled { get; set; }

    public string EncompassDataDirectory { get; set; }

    public string LogDirectory { get; set; }

    public DbServerType DatabaseType { get; set; }

    public string DatabaseServer { get; set; }

    public string DatabaseAGListener { get; set; }

    public string Port { get; set; }

    public string DatabaseName { get; set; }

    public string DatabaseUserID { get; set; }

    public string DatabasePassword { get; set; }

    public string MongoActiveHosts { get; set; }

    public string MongoActiveDatabase { get; set; }

    public string MongoActiveCredentials { get; set; }

    public string MongoActiveOptions { get; set; }

    public string MongoArchiveHosts { get; set; }

    public string MongoArchiveDatabase { get; set; }

    public string MongoArchiveCredentials { get; set; }

    public string MongoArchiveOptions { get; set; }

    public string TPOAdminSiteUrl { get; set; }

    public string EncompassPlatformAPI { get; set; }

    public string DesignatedVersion { get; set; }

    public string PostgresDatabaseHost { get; set; }

    public string PostgresDatabasePort { get; set; }

    public string PostgresDatabaseUsername { get; set; }

    public string PostgresDatabasePassword { get; set; }

    public string PostgresDatabaseName { get; set; }

    public string MongoActiveHosts2 { get; set; }

    public string MongoActiveDatabase2 { get; set; }

    public string MongoActiveCredentials2 { get; set; }

    public string MongoActiveOptions2 { get; set; }

    public string MongoArchiveHosts2 { get; set; }

    public string MongoArchiveDatabase2 { get; set; }

    public string MongoArchiveCredentials2 { get; set; }

    public string MongoArchiveOptions2 { get; set; }

    public bool DisabledAGListener { get; set; }
  }
}
