// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.Configuration.DSWriterInstanceConfiguration
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
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.Server.Configuration
{
  public class DSWriterInstanceConfiguration : IInstanceConfiguration
  {
    private static b _jed;
    private readonly IDirectoryService _client;
    private Dictionary<string, DirectoryEntryDto> _instanceEntries;
    private Dictionary<string, DirectoryCategoryDto> _categories;
    private int? _instanceId;
    private string _instanceName;

    static DSWriterInstanceConfiguration()
    {
      a.a("z2r1xy8k5mp4ccpl");
      DSWriterInstanceConfiguration._jed = a.b("z5cty6u5dj3bd8");
    }

    public DSWriterInstanceConfiguration(string instanceName, IDirectoryService directoryService)
    {
      this._client = directoryService;
      this._instanceName = instanceName;
    }

    private static string EncryptString(string plainText)
    {
      lock (DSWriterInstanceConfiguration._jed)
      {
        DSWriterInstanceConfiguration._jed.b();
        return Convert.ToBase64String(DSWriterInstanceConfiguration._jed.b(plainText));
      }
    }

    private void UpdateEntry(DSWriterInstanceConfiguration.DSEntry key, string value)
    {
      if (key == null || value == null)
        return;
      if (this._categories == null)
        this._categories = this._client.GetCategories().OrderBy<DirectoryCategoryDto, string>((Func<DirectoryCategoryDto, string>) (cat => cat.Name)).GroupBy<DirectoryCategoryDto, string>((Func<DirectoryCategoryDto, string>) (cat => cat.Name)).ToDictionary<IGrouping<string, DirectoryCategoryDto>, string, DirectoryCategoryDto>((Func<IGrouping<string, DirectoryCategoryDto>, string>) (grp => grp.Key), (Func<IGrouping<string, DirectoryCategoryDto>, DirectoryCategoryDto>) (grp => grp.FirstOrDefault<DirectoryCategoryDto>()));
      if (this._instanceEntries == null)
        this._instanceEntries = this._client.GetEntriesInInstance(this._instanceName).GroupBy<DirectoryEntryDto, string>((Func<DirectoryEntryDto, string>) (ent => ent.Name)).ToDictionary<IGrouping<string, DirectoryEntryDto>, string, DirectoryEntryDto>((Func<IGrouping<string, DirectoryEntryDto>, string>) (grp => grp.Key), (Func<IGrouping<string, DirectoryEntryDto>, DirectoryEntryDto>) (grp => grp.FirstOrDefault<DirectoryEntryDto>()));
      if (!this._instanceId.HasValue)
        this._instanceId = this._instanceEntries.Count <= 0 ? new int?(this._client.AddInstance(this._instanceName).Id) : new int?(this._instanceEntries.Values.FirstOrDefault<DirectoryEntryDto>().InstanceId);
      if (this._instanceEntries.ContainsKey(key.Name))
      {
        DirectoryEntryDto instanceEntry = this._instanceEntries[key.Name];
        this._client.UpdateEntry(instanceEntry.Id, instanceEntry.CategoryId, instanceEntry.Name, DirectoryEntryValueTypeDto.String, (object) value);
      }
      else
      {
        int instanceId = this._instanceId ?? -1;
        string category = key.Category;
        int categoryId = !this._categories.ContainsKey(category) ? this._client.AddCategory(category).Id : this._categories[category].Id;
        this._client.AddEntry(instanceId, categoryId, key.Name, DirectoryEntryValueTypeDto.String, (object) value);
      }
    }

    public bool Disabled
    {
      get => throw new NotSupportedException();
      set => this.UpdateEntry(DSWriterInstanceConfiguration.DSEntries.Disabled, value ? "1" : "");
    }

    public string EncompassDataDirectory
    {
      get => throw new NotSupportedException();
      set
      {
        this.UpdateEntry(DSWriterInstanceConfiguration.DSEntries.EncompassDataDirectory, value);
      }
    }

    public string LogDirectory
    {
      get => throw new NotSupportedException();
      set => this.UpdateEntry(DSWriterInstanceConfiguration.DSEntries.LogDirectory, value);
    }

    public DbServerType DatabaseType
    {
      get => throw new NotSupportedException();
      set
      {
        string str;
        switch (value)
        {
          case DbServerType.SqlServer:
            str = "sqlserver";
            break;
          case DbServerType.Postgres:
            str = "postgresql";
            break;
          default:
            str = (string) null;
            break;
        }
        this.UpdateEntry(DSWriterInstanceConfiguration.DSEntries.DatabaseType, str);
      }
    }

    public string DatabaseServer
    {
      get => throw new NotSupportedException();
      set => throw new NotSupportedException();
    }

    public string DatabaseAGListener
    {
      get => throw new NotSupportedException();
      set => throw new NotSupportedException();
    }

    public string Port
    {
      get => throw new NotSupportedException();
      set => this.UpdateEntry(DSWriterInstanceConfiguration.DSEntries.Port, value);
    }

    public string DatabaseName
    {
      get => throw new NotSupportedException();
      set => this.UpdateEntry(DSWriterInstanceConfiguration.DSEntries.DatabaseName, value);
    }

    public string DatabaseUserID
    {
      get => throw new NotSupportedException();
      set => this.UpdateEntry(DSWriterInstanceConfiguration.DSEntries.DatabaseUserID, value);
    }

    public string DatabasePassword
    {
      get => throw new NotSupportedException();
      set
      {
        this.UpdateEntry(DSWriterInstanceConfiguration.DSEntries.DatabaseCredentials, DSWriterInstanceConfiguration.EncryptString(value));
      }
    }

    public string MongoActiveHosts
    {
      get => throw new NotSupportedException();
      set => this.UpdateEntry(DSWriterInstanceConfiguration.DSEntries.MongoActiveHosts, value);
    }

    public string MongoActiveDatabase
    {
      get => throw new NotSupportedException();
      set => this.UpdateEntry(DSWriterInstanceConfiguration.DSEntries.MongoActiveDatabase, value);
    }

    public string MongoActiveCredentials
    {
      get => throw new NotSupportedException();
      set
      {
        this.UpdateEntry(DSWriterInstanceConfiguration.DSEntries.MongoActiveCredentials, value);
      }
    }

    public string MongoActiveOptions
    {
      get => throw new NotSupportedException();
      set => this.UpdateEntry(DSWriterInstanceConfiguration.DSEntries.MongoActiveOptions, value);
    }

    public string MongoArchiveHosts
    {
      get => throw new NotSupportedException();
      set => this.UpdateEntry(DSWriterInstanceConfiguration.DSEntries.MongoArchiveHosts, value);
    }

    public string MongoArchiveDatabase
    {
      get => throw new NotSupportedException();
      set => this.UpdateEntry(DSWriterInstanceConfiguration.DSEntries.MongoArchiveDatabase, value);
    }

    public string MongoArchiveCredentials
    {
      get => throw new NotSupportedException();
      set
      {
        this.UpdateEntry(DSWriterInstanceConfiguration.DSEntries.MongoArchiveCredentials, value);
      }
    }

    public string MongoArchiveOptions
    {
      get => throw new NotSupportedException();
      set => this.UpdateEntry(DSWriterInstanceConfiguration.DSEntries.MongoArchiveOptions, value);
    }

    public string TPOAdminSiteUrl
    {
      get => throw new NotSupportedException();
      set => this.UpdateEntry(DSWriterInstanceConfiguration.DSEntries.TPOAdminSiteUrl, value);
    }

    public string EncompassPlatformAPI
    {
      get => throw new NotSupportedException();
      set => this.UpdateEntry(DSWriterInstanceConfiguration.DSEntries.EncompassPlatformAPI, value);
    }

    public string DesignatedVersion
    {
      get => throw new NotSupportedException();
      set => this.UpdateEntry(DSWriterInstanceConfiguration.DSEntries.DesignatedVersion, value);
    }

    public string PostgresDatabaseHost
    {
      get => throw new NotSupportedException();
      set => this.UpdateEntry(DSWriterInstanceConfiguration.DSEntries.PostgresDatabaseHost, value);
    }

    public string PostgresDatabasePort
    {
      get => throw new NotSupportedException();
      set => this.UpdateEntry(DSWriterInstanceConfiguration.DSEntries.PostgresDatabasePort, value);
    }

    public string PostgresDatabaseUsername
    {
      get => throw new NotSupportedException();
      set
      {
        this.UpdateEntry(DSWriterInstanceConfiguration.DSEntries.PostgresDatabaseUsername, value);
      }
    }

    public string PostgresDatabasePassword
    {
      get => throw new NotSupportedException();
      set
      {
        this.UpdateEntry(DSWriterInstanceConfiguration.DSEntries.PostgresDatabaseCredentials, DSWriterInstanceConfiguration.EncryptString(value));
      }
    }

    public string PostgresDatabaseName
    {
      get => throw new NotSupportedException();
      set => this.UpdateEntry(DSWriterInstanceConfiguration.DSEntries.PostgresDatabaseName, value);
    }

    public string MongoActiveHosts2
    {
      get => throw new NotSupportedException();
      set => this.UpdateEntry(DSWriterInstanceConfiguration.DSEntries.MongoActiveHosts2, value);
    }

    public string MongoActiveDatabase2
    {
      get => throw new NotSupportedException();
      set => this.UpdateEntry(DSWriterInstanceConfiguration.DSEntries.MongoActiveDatabase2, value);
    }

    public string MongoActiveCredentials2
    {
      get => throw new NotSupportedException();
      set
      {
        this.UpdateEntry(DSWriterInstanceConfiguration.DSEntries.MongoActiveCredentials2, value);
      }
    }

    public string MongoActiveOptions2
    {
      get => throw new NotSupportedException();
      set => this.UpdateEntry(DSWriterInstanceConfiguration.DSEntries.MongoActiveOptions2, value);
    }

    public string MongoArchiveHosts2
    {
      get => throw new NotSupportedException();
      set => this.UpdateEntry(DSWriterInstanceConfiguration.DSEntries.MongoArchiveHosts2, value);
    }

    public string MongoArchiveDatabase2
    {
      get => throw new NotSupportedException();
      set => this.UpdateEntry(DSWriterInstanceConfiguration.DSEntries.MongoArchiveDatabase2, value);
    }

    public string MongoArchiveCredentials2
    {
      get => throw new NotSupportedException();
      set
      {
        this.UpdateEntry(DSWriterInstanceConfiguration.DSEntries.MongoArchiveCredentials2, value);
      }
    }

    public string MongoArchiveOptions2
    {
      get => throw new NotSupportedException();
      set => this.UpdateEntry(DSWriterInstanceConfiguration.DSEntries.MongoArchiveOptions2, value);
    }

    public bool DisabledAGListener
    {
      get => throw new NotImplementedException();
      set => throw new NotImplementedException();
    }

    private class DSEntry
    {
      public DSEntry(string name, string category)
      {
        this.Name = name;
        this.Category = category;
      }

      public string Name { get; private set; }

      public string Category { get; private set; }
    }

    private class DSEntries
    {
      public static DSWriterInstanceConfiguration.DSEntry DatabaseCredentials = new DSWriterInstanceConfiguration.DSEntry(nameof (DatabaseCredentials), "DataStoreSettings");
      public static DSWriterInstanceConfiguration.DSEntry DatabaseType = new DSWriterInstanceConfiguration.DSEntry(nameof (DatabaseType), "DataStoreSettings");
      public static DSWriterInstanceConfiguration.DSEntry DatabaseName = new DSWriterInstanceConfiguration.DSEntry(nameof (DatabaseName), "DataStoreSettings");
      public static DSWriterInstanceConfiguration.DSEntry DatabaseServer = new DSWriterInstanceConfiguration.DSEntry(nameof (DatabaseServer), "DataStoreSettings");
      public static DSWriterInstanceConfiguration.DSEntry Port = new DSWriterInstanceConfiguration.DSEntry(nameof (Port), "DataStoreSettings");
      public static DSWriterInstanceConfiguration.DSEntry DatabaseUserID = new DSWriterInstanceConfiguration.DSEntry(nameof (DatabaseUserID), "DataStoreSettings");
      public static DSWriterInstanceConfiguration.DSEntry DatabaseCredentials2 = new DSWriterInstanceConfiguration.DSEntry(nameof (DatabaseCredentials2), "DataStoreSettings");
      public static DSWriterInstanceConfiguration.DSEntry DatabaseName2 = new DSWriterInstanceConfiguration.DSEntry(nameof (DatabaseName2), "DataStoreSettings");
      public static DSWriterInstanceConfiguration.DSEntry DatabaseServer2 = new DSWriterInstanceConfiguration.DSEntry(nameof (DatabaseServer2), "DataStoreSettings");
      public static DSWriterInstanceConfiguration.DSEntry DatabaseUserID2 = new DSWriterInstanceConfiguration.DSEntry(nameof (DatabaseUserID2), "DataStoreSettings");
      public static DSWriterInstanceConfiguration.DSEntry EncompassDataDirectory = new DSWriterInstanceConfiguration.DSEntry(nameof (EncompassDataDirectory), "DataStoreSettings");
      public static DSWriterInstanceConfiguration.DSEntry LogDirectory = new DSWriterInstanceConfiguration.DSEntry(nameof (LogDirectory), "DataStoreSettings");
      public static DSWriterInstanceConfiguration.DSEntry Disabled = new DSWriterInstanceConfiguration.DSEntry(nameof (Disabled), "Host");
      public static DSWriterInstanceConfiguration.DSEntry EncompassPlatformAPI = new DSWriterInstanceConfiguration.DSEntry(nameof (EncompassPlatformAPI), "Host");
      public static DSWriterInstanceConfiguration.DSEntry TPOAdminSiteUrl = new DSWriterInstanceConfiguration.DSEntry(nameof (TPOAdminSiteUrl), "Host");
      public static DSWriterInstanceConfiguration.DSEntry MongoActiveCredentials = new DSWriterInstanceConfiguration.DSEntry(nameof (MongoActiveCredentials), "Mongo");
      public static DSWriterInstanceConfiguration.DSEntry MongoActiveDatabase = new DSWriterInstanceConfiguration.DSEntry(nameof (MongoActiveDatabase), "Mongo");
      public static DSWriterInstanceConfiguration.DSEntry MongoActiveHosts = new DSWriterInstanceConfiguration.DSEntry(nameof (MongoActiveHosts), "Mongo");
      public static DSWriterInstanceConfiguration.DSEntry MongoActiveOptions = new DSWriterInstanceConfiguration.DSEntry(nameof (MongoActiveOptions), "Mongo");
      public static DSWriterInstanceConfiguration.DSEntry MongoArchiveCredentials = new DSWriterInstanceConfiguration.DSEntry(nameof (MongoArchiveCredentials), "Mongo");
      public static DSWriterInstanceConfiguration.DSEntry MongoArchiveDatabase = new DSWriterInstanceConfiguration.DSEntry(nameof (MongoArchiveDatabase), "Mongo");
      public static DSWriterInstanceConfiguration.DSEntry MongoArchiveHosts = new DSWriterInstanceConfiguration.DSEntry(nameof (MongoArchiveHosts), "Mongo");
      public static DSWriterInstanceConfiguration.DSEntry MongoArchiveOptions = new DSWriterInstanceConfiguration.DSEntry(nameof (MongoArchiveOptions), "Mongo");
      public static DSWriterInstanceConfiguration.DSEntry DesignatedVersion = new DSWriterInstanceConfiguration.DSEntry(nameof (DesignatedVersion), "Host");
      public static DSWriterInstanceConfiguration.DSEntry PostgresDatabaseHost = new DSWriterInstanceConfiguration.DSEntry(nameof (PostgresDatabaseHost), "DataStoreSettings");
      public static DSWriterInstanceConfiguration.DSEntry PostgresDatabasePort = new DSWriterInstanceConfiguration.DSEntry(nameof (PostgresDatabasePort), "DataStoreSettings");
      public static DSWriterInstanceConfiguration.DSEntry PostgresDatabaseUsername = new DSWriterInstanceConfiguration.DSEntry(nameof (PostgresDatabaseUsername), "DataStoreSettings");
      public static DSWriterInstanceConfiguration.DSEntry PostgresDatabaseCredentials = new DSWriterInstanceConfiguration.DSEntry(nameof (PostgresDatabaseCredentials), "DataStoreSettings");
      public static DSWriterInstanceConfiguration.DSEntry PostgresDatabaseName = new DSWriterInstanceConfiguration.DSEntry(nameof (PostgresDatabaseName), "DataStoreSettings");
      public static DSWriterInstanceConfiguration.DSEntry MongoActiveCredentials2 = new DSWriterInstanceConfiguration.DSEntry(nameof (MongoActiveCredentials2), "Mongo");
      public static DSWriterInstanceConfiguration.DSEntry MongoActiveDatabase2 = new DSWriterInstanceConfiguration.DSEntry(nameof (MongoActiveDatabase2), "Mongo");
      public static DSWriterInstanceConfiguration.DSEntry MongoActiveHosts2 = new DSWriterInstanceConfiguration.DSEntry(nameof (MongoActiveHosts2), "Mongo");
      public static DSWriterInstanceConfiguration.DSEntry MongoActiveOptions2 = new DSWriterInstanceConfiguration.DSEntry(nameof (MongoActiveOptions2), "Mongo");
      public static DSWriterInstanceConfiguration.DSEntry MongoArchiveCredentials2 = new DSWriterInstanceConfiguration.DSEntry(nameof (MongoArchiveCredentials2), "Mongo");
      public static DSWriterInstanceConfiguration.DSEntry MongoArchiveDatabase2 = new DSWriterInstanceConfiguration.DSEntry(nameof (MongoArchiveDatabase2), "Mongo");
      public static DSWriterInstanceConfiguration.DSEntry MongoArchiveHosts2 = new DSWriterInstanceConfiguration.DSEntry(nameof (MongoArchiveHosts2), "Mongo");
      public static DSWriterInstanceConfiguration.DSEntry MongoArchiveOptions2 = new DSWriterInstanceConfiguration.DSEntry(nameof (MongoArchiveOptions2), "Mongo");
    }
  }
}
