// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.Configuration.ContainerInstanceConfiguration
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Configuration;

#nullable disable
namespace EllieMae.EMLite.Server.Configuration
{
  public class ContainerInstanceConfiguration : IInstanceConfiguration
  {
    public bool Disabled { get; set; }

    public string EncompassDataDirectory { get; set; }

    public string LogDirectory { get; set; }

    public string DatabaseServer { get; set; }

    public string DatabaseAGListener { get; set; }

    public string DatabaseName { get; set; }

    public string DatabaseUserID { get; set; }

    public string DatabasePassword { get; set; }

    public DbServerType DatabaseType { get; set; }

    public string Port { get; set; }

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
