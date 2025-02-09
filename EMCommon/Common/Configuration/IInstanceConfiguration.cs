// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Configuration.IInstanceConfiguration
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

#nullable disable
namespace EllieMae.EMLite.Common.Configuration
{
  public interface IInstanceConfiguration
  {
    bool Disabled { get; set; }

    string EncompassDataDirectory { get; set; }

    string LogDirectory { get; set; }

    DbServerType DatabaseType { get; set; }

    string DatabaseServer { get; set; }

    string DatabaseAGListener { get; set; }

    string Port { get; set; }

    string DatabaseName { get; set; }

    string DatabaseUserID { get; set; }

    string DatabasePassword { get; set; }

    string MongoActiveHosts { get; set; }

    string MongoActiveDatabase { get; set; }

    string MongoActiveCredentials { get; set; }

    string MongoActiveOptions { get; set; }

    string MongoArchiveHosts { get; set; }

    string MongoArchiveDatabase { get; set; }

    string MongoArchiveCredentials { get; set; }

    string MongoArchiveOptions { get; set; }

    string TPOAdminSiteUrl { get; set; }

    string EncompassPlatformAPI { get; set; }

    string DesignatedVersion { get; set; }

    string PostgresDatabaseHost { get; set; }

    string PostgresDatabasePort { get; set; }

    string PostgresDatabaseUsername { get; set; }

    string PostgresDatabasePassword { get; set; }

    string PostgresDatabaseName { get; set; }

    string MongoActiveHosts2 { get; set; }

    string MongoActiveDatabase2 { get; set; }

    string MongoActiveCredentials2 { get; set; }

    string MongoActiveOptions2 { get; set; }

    string MongoArchiveHosts2 { get; set; }

    string MongoArchiveDatabase2 { get; set; }

    string MongoArchiveCredentials2 { get; set; }

    string MongoArchiveOptions2 { get; set; }

    bool DisabledAGListener { get; set; }
  }
}
