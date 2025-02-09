// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.Configuration.DirectoryInstanceConfiguration
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Elli.DirectoryServices.Contracts.Dto;
using Elli.DirectoryServices.Contracts.Services;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Configuration;
using EllieMae.EMLite.JedLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.Server.Configuration
{
  public class DirectoryInstanceConfiguration : IInstanceConfiguration
  {
    private static b _jed;
    private readonly IDirectoryService _directoryService;
    private string _databaseCredentials;
    private string _postgresDatabaseCredentials;

    static DirectoryInstanceConfiguration() => DirectoryInstanceConfiguration.ConfigJedLib();

    public DirectoryInstanceConfiguration(string instanceName, IDirectoryService directoryService)
    {
      this._directoryService = directoryService;
      try
      {
        IEnumerable<DirectoryEntryDto> entriesInInstance = this._directoryService.GetEntriesInInstance(instanceName);
        if (entriesInInstance != null)
        {
          this.Disabled = this.toString(entriesInInstance, nameof (Disabled), "") == "1";
          this.LogDirectory = this.toString(entriesInInstance, nameof (LogDirectory), "");
          this.EncompassAPI = entriesInInstance.FirstOrDefault<DirectoryEntryDto>((Func<DirectoryEntryDto, bool>) (x => x.Name.Equals(nameof (EncompassAPI), StringComparison.CurrentCultureIgnoreCase)))?.Value.ToString() ?? string.Empty;
          this.EncompassDataDirectory = entriesInInstance.FirstOrDefault<DirectoryEntryDto>((Func<DirectoryEntryDto, bool>) (x => x.Name.Equals(nameof (EncompassDataDirectory), StringComparison.CurrentCultureIgnoreCase)))?.Value.ToString() ?? string.Empty;
          this.DatabaseServer = entriesInInstance.FirstOrDefault<DirectoryEntryDto>((Func<DirectoryEntryDto, bool>) (x => x.Name.Equals(nameof (DatabaseServer), StringComparison.CurrentCultureIgnoreCase)))?.Value.ToString() ?? string.Empty;
          this.DatabaseAGListener = entriesInInstance.FirstOrDefault<DirectoryEntryDto>((Func<DirectoryEntryDto, bool>) (x => x.Name.Equals("AGListener", StringComparison.CurrentCultureIgnoreCase)))?.Value.ToString() ?? string.Empty;
          bool result;
          if (bool.TryParse(entriesInInstance.FirstOrDefault<DirectoryEntryDto>((Func<DirectoryEntryDto, bool>) (x => x.Name.Equals(nameof (DisabledAGListener), StringComparison.CurrentCultureIgnoreCase)))?.Value.ToString(), out result))
            this.DisabledAGListener = result;
          this.DatabaseName = entriesInInstance.FirstOrDefault<DirectoryEntryDto>((Func<DirectoryEntryDto, bool>) (x => x.Name.Equals(nameof (DatabaseName), StringComparison.CurrentCultureIgnoreCase)))?.Value.ToString() ?? string.Empty;
          this.DatabaseUserID = entriesInInstance.FirstOrDefault<DirectoryEntryDto>((Func<DirectoryEntryDto, bool>) (x => x.Name.Equals(nameof (DatabaseUserID), StringComparison.CurrentCultureIgnoreCase)))?.Value.ToString() ?? string.Empty;
          this._databaseCredentials = entriesInInstance.FirstOrDefault<DirectoryEntryDto>((Func<DirectoryEntryDto, bool>) (x => x.Name.Equals("DatabaseCredentials", StringComparison.CurrentCultureIgnoreCase)))?.Value.ToString() ?? string.Empty;
          try
          {
            string str = entriesInInstance.FirstOrDefault<DirectoryEntryDto>((Func<DirectoryEntryDto, bool>) (x => x.Name.Equals(nameof (DatabaseType), StringComparison.CurrentCultureIgnoreCase)))?.Value.ToString() ?? string.Empty;
            this.DatabaseType = !(str.ToLower() == "sqlserver") ? (!(str.ToLower() == "postgresql") ? DbServerType.SqlServer : DbServerType.Postgres) : DbServerType.SqlServer;
          }
          catch
          {
            this.DatabaseType = DbServerType.SqlServer;
          }
          this.Port = entriesInInstance.FirstOrDefault<DirectoryEntryDto>((Func<DirectoryEntryDto, bool>) (x => x.Name.Equals(nameof (Port), StringComparison.CurrentCultureIgnoreCase)))?.Value.ToString() ?? string.Empty;
          this.MongoActiveHosts = entriesInInstance.FirstOrDefault<DirectoryEntryDto>((Func<DirectoryEntryDto, bool>) (x => x.Name.Equals(nameof (MongoActiveHosts), StringComparison.CurrentCultureIgnoreCase)))?.Value.ToString() ?? string.Empty;
          this.MongoActiveDatabase = entriesInInstance.FirstOrDefault<DirectoryEntryDto>((Func<DirectoryEntryDto, bool>) (x => x.Name.Equals(nameof (MongoActiveDatabase), StringComparison.CurrentCultureIgnoreCase)))?.Value.ToString() ?? string.Empty;
          this.MongoActiveCredentials = entriesInInstance.FirstOrDefault<DirectoryEntryDto>((Func<DirectoryEntryDto, bool>) (x => x.Name.Equals(nameof (MongoActiveCredentials), StringComparison.CurrentCultureIgnoreCase)))?.Value.ToString() ?? string.Empty;
          this.MongoActiveOptions = entriesInInstance.FirstOrDefault<DirectoryEntryDto>((Func<DirectoryEntryDto, bool>) (x => x.Name.Equals(nameof (MongoActiveOptions), StringComparison.CurrentCultureIgnoreCase)))?.Value.ToString() ?? string.Empty;
          this.MongoArchiveHosts = entriesInInstance.FirstOrDefault<DirectoryEntryDto>((Func<DirectoryEntryDto, bool>) (x => x.Name.Equals(nameof (MongoArchiveHosts), StringComparison.CurrentCultureIgnoreCase)))?.Value.ToString() ?? string.Empty;
          this.MongoArchiveDatabase = entriesInInstance.FirstOrDefault<DirectoryEntryDto>((Func<DirectoryEntryDto, bool>) (x => x.Name.Equals(nameof (MongoArchiveDatabase), StringComparison.CurrentCultureIgnoreCase)))?.Value.ToString() ?? string.Empty;
          this.MongoArchiveCredentials = entriesInInstance.FirstOrDefault<DirectoryEntryDto>((Func<DirectoryEntryDto, bool>) (x => x.Name.Equals(nameof (MongoArchiveCredentials), StringComparison.CurrentCultureIgnoreCase)))?.Value.ToString() ?? string.Empty;
          this.MongoArchiveOptions = entriesInInstance.FirstOrDefault<DirectoryEntryDto>((Func<DirectoryEntryDto, bool>) (x => x.Name.Equals(nameof (MongoArchiveOptions), StringComparison.CurrentCultureIgnoreCase)))?.Value.ToString() ?? string.Empty;
          this.MongoActiveHosts2 = entriesInInstance.FirstOrDefault<DirectoryEntryDto>((Func<DirectoryEntryDto, bool>) (x => x.Name.Equals(nameof (MongoActiveHosts2), StringComparison.CurrentCultureIgnoreCase)))?.Value.ToString() ?? string.Empty;
          this.MongoActiveDatabase2 = entriesInInstance.FirstOrDefault<DirectoryEntryDto>((Func<DirectoryEntryDto, bool>) (x => x.Name.Equals(nameof (MongoActiveDatabase2), StringComparison.CurrentCultureIgnoreCase)))?.Value.ToString() ?? string.Empty;
          this.MongoActiveCredentials2 = entriesInInstance.FirstOrDefault<DirectoryEntryDto>((Func<DirectoryEntryDto, bool>) (x => x.Name.Equals(nameof (MongoActiveCredentials2), StringComparison.CurrentCultureIgnoreCase)))?.Value.ToString() ?? string.Empty;
          this.MongoActiveOptions2 = entriesInInstance.FirstOrDefault<DirectoryEntryDto>((Func<DirectoryEntryDto, bool>) (x => x.Name.Equals(nameof (MongoActiveOptions2), StringComparison.CurrentCultureIgnoreCase)))?.Value.ToString() ?? string.Empty;
          this.MongoArchiveHosts2 = entriesInInstance.FirstOrDefault<DirectoryEntryDto>((Func<DirectoryEntryDto, bool>) (x => x.Name.Equals(nameof (MongoArchiveHosts2), StringComparison.CurrentCultureIgnoreCase)))?.Value.ToString() ?? string.Empty;
          this.MongoArchiveDatabase2 = entriesInInstance.FirstOrDefault<DirectoryEntryDto>((Func<DirectoryEntryDto, bool>) (x => x.Name.Equals(nameof (MongoArchiveDatabase2), StringComparison.CurrentCultureIgnoreCase)))?.Value.ToString() ?? string.Empty;
          this.MongoArchiveCredentials2 = entriesInInstance.FirstOrDefault<DirectoryEntryDto>((Func<DirectoryEntryDto, bool>) (x => x.Name.Equals(nameof (MongoArchiveCredentials2), StringComparison.CurrentCultureIgnoreCase)))?.Value.ToString() ?? string.Empty;
          this.MongoArchiveOptions2 = entriesInInstance.FirstOrDefault<DirectoryEntryDto>((Func<DirectoryEntryDto, bool>) (x => x.Name.Equals(nameof (MongoArchiveOptions2), StringComparison.CurrentCultureIgnoreCase)))?.Value.ToString() ?? string.Empty;
          this.TPOAdminSiteUrl = entriesInInstance.FirstOrDefault<DirectoryEntryDto>((Func<DirectoryEntryDto, bool>) (x => x.Name.Equals(nameof (TPOAdminSiteUrl), StringComparison.CurrentCultureIgnoreCase)))?.Value.ToString() ?? string.Empty;
          this.EncompassPlatformAPI = entriesInInstance.FirstOrDefault<DirectoryEntryDto>((Func<DirectoryEntryDto, bool>) (x => x.Name.Equals(nameof (EncompassPlatformAPI), StringComparison.CurrentCultureIgnoreCase)))?.Value.ToString() ?? string.Empty;
          this.DesignatedVersion = entriesInInstance.FirstOrDefault<DirectoryEntryDto>((Func<DirectoryEntryDto, bool>) (x => x.Name.Equals(nameof (DesignatedVersion), StringComparison.CurrentCultureIgnoreCase)))?.Value.ToString() ?? string.Empty;
          this.PostgresDatabaseHost = entriesInInstance.FirstOrDefault<DirectoryEntryDto>((Func<DirectoryEntryDto, bool>) (x => x.Name.Equals(nameof (PostgresDatabaseHost), StringComparison.CurrentCultureIgnoreCase)))?.Value.ToString() ?? string.Empty;
          this.PostgresDatabaseName = entriesInInstance.FirstOrDefault<DirectoryEntryDto>((Func<DirectoryEntryDto, bool>) (x => x.Name.Equals(nameof (PostgresDatabaseName), StringComparison.CurrentCultureIgnoreCase)))?.Value.ToString() ?? string.Empty;
          this._postgresDatabaseCredentials = entriesInInstance.FirstOrDefault<DirectoryEntryDto>((Func<DirectoryEntryDto, bool>) (x => x.Name.Equals("PostgresDatabaseCredentials", StringComparison.CurrentCultureIgnoreCase)))?.Value.ToString() ?? string.Empty;
          this.PostgresDatabasePort = entriesInInstance.FirstOrDefault<DirectoryEntryDto>((Func<DirectoryEntryDto, bool>) (x => x.Name.Equals(nameof (PostgresDatabasePort), StringComparison.CurrentCultureIgnoreCase)))?.Value.ToString() ?? string.Empty;
          this.PostgresDatabaseUsername = entriesInInstance.FirstOrDefault<DirectoryEntryDto>((Func<DirectoryEntryDto, bool>) (x => x.Name.Equals(nameof (PostgresDatabaseUsername), StringComparison.CurrentCultureIgnoreCase)))?.Value.ToString() ?? string.Empty;
          this.IsValid = true;
        }
        else
          this.IsValid = false;
      }
      catch (Exception ex)
      {
        this.IsValid = false;
      }
    }

    private string toString(
      IEnumerable<DirectoryEntryDto> dirEntries,
      string key,
      string defaultValue)
    {
      DirectoryEntryDto directoryEntryDto = dirEntries.FirstOrDefault<DirectoryEntryDto>((Func<DirectoryEntryDto, bool>) (x => x.Name.Equals(key, StringComparison.CurrentCultureIgnoreCase)));
      return directoryEntryDto == null ? defaultValue : directoryEntryDto.Value.ToString();
    }

    public bool Disabled { get; set; }

    public string EncompassAPI { get; set; }

    public string EncompassDataDirectory { get; set; }

    public string LogDirectory { get; set; }

    public DbServerType DatabaseType { get; set; }

    public string DatabaseServer { get; set; }

    public string DatabaseAGListener { get; set; }

    public string Port { get; set; }

    public string DatabaseName { get; set; }

    public string DatabaseUserID { get; set; }

    public string DatabasePassword
    {
      get => DirectoryInstanceConfiguration.DecryptStringOrEmpty(this._databaseCredentials);
      set => this._databaseCredentials = DirectoryInstanceConfiguration.EncryptString(value);
    }

    public string TPOAdminSiteUrl { get; set; }

    public string MongoActiveHosts { get; set; }

    public string MongoActiveDatabase { get; set; }

    public string MongoActiveCredentials { get; set; }

    public string MongoActiveOptions { get; set; }

    public string MongoArchiveHosts { get; set; }

    public string MongoArchiveDatabase { get; set; }

    public string MongoArchiveCredentials { get; set; }

    public string MongoArchiveOptions { get; set; }

    public string DesignatedVersion { get; set; }

    public string PostgresDatabaseHost { get; set; }

    public string PostgresDatabasePort { get; set; }

    public string PostgresDatabaseUsername { get; set; }

    public string PostgresDatabasePassword
    {
      get => DirectoryInstanceConfiguration.DecryptStringOrEmpty(this._postgresDatabaseCredentials);
      set
      {
        this._postgresDatabaseCredentials = DirectoryInstanceConfiguration.EncryptString(value);
      }
    }

    public string PostgresDatabaseName { get; set; }

    public string PostgreSqlConnectionString { get; set; }

    public bool IsValid { get; set; }

    public string MongoActiveHosts2 { get; set; }

    public string MongoActiveDatabase2 { get; set; }

    public string MongoActiveCredentials2 { get; set; }

    public string MongoActiveOptions2 { get; set; }

    public string MongoArchiveHosts2 { get; set; }

    public string MongoArchiveDatabase2 { get; set; }

    public string MongoArchiveCredentials2 { get; set; }

    public string MongoArchiveOptions2 { get; set; }

    private static void ConfigJedLib()
    {
      a.a("z2r1xy8k5mp4ccpl");
      DirectoryInstanceConfiguration._jed = DirectoryInstanceConfiguration.CreateJed();
    }

    private static b CreateJed() => a.b("z5cty6u5dj3bd8");

    private static string DecryptStringOrEmpty(string cipherText)
    {
      try
      {
        return DirectoryInstanceConfiguration.DecryptString(cipherText + string.Empty);
      }
      catch
      {
        return string.Empty;
      }
    }

    private static string DecryptString(string cipherText)
    {
      lock (DirectoryInstanceConfiguration._jed)
      {
        DirectoryInstanceConfiguration._jed.b();
        return DirectoryInstanceConfiguration._jed.a((Stream) new MemoryStream(Convert.FromBase64String(cipherText)));
      }
    }

    private static string EncryptString(string plainText)
    {
      lock (DirectoryInstanceConfiguration._jed)
      {
        DirectoryInstanceConfiguration._jed.b();
        return Convert.ToBase64String(DirectoryInstanceConfiguration._jed.b(plainText));
      }
    }

    public string EncompassPlatformAPI { get; set; }

    public bool DisabledAGListener { get; set; }
  }
}
