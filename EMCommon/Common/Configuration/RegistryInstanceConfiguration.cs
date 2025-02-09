// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Configuration.RegistryInstanceConfiguration
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using EllieMae.EMLite.JedLib;
using Microsoft.Win32;
using System;
using System.IO;

#nullable disable
namespace EllieMae.EMLite.Common.Configuration
{
  public class RegistryInstanceConfiguration : IInstanceConfiguration
  {
    private static b jed = (b) null;
    private RegistryAccessor registryAccessor;
    private static string tpoAdminSiteurl = "";

    static RegistryInstanceConfiguration()
    {
      a.a("z2r1xy8k5mp4ccpl");
      RegistryInstanceConfiguration.jed = RegistryInstanceConfiguration.CreateJed();
    }

    public RegistryInstanceConfiguration(string instanceName)
      : this(RegistryInstanceConfiguration.CreateInstanceRegistryAccessor(instanceName))
    {
    }

    public RegistryInstanceConfiguration(RegistryAccessor regAccessor)
    {
      this.registryAccessor = regAccessor;
    }

    public RegistryAccessor RegistryHive => this.registryAccessor;

    public string EncompassDataDirectory
    {
      get => string.Concat(this.registryAccessor.ReadValue("DataDir"));
      set => this.registryAccessor.WriteValue("DataDir", (object) value);
    }

    public string LogDirectory
    {
      get => string.Concat(this.registryAccessor.ReadValue("LogDir"));
      set => this.registryAccessor.WriteValue("LogDir", (object) value);
    }

    public bool Disabled
    {
      get => string.Concat(this.registryAccessor.ReadValue(nameof (Disabled))) == "1";
      set
      {
        this.registryAccessor.WriteValue(nameof (Disabled), value ? (object) "1" : (object) "0");
      }
    }

    public DbServerType DatabaseType
    {
      get
      {
        string str = string.Concat(this.registryAccessor.ReadValue("MSDE\\DatabaseType", (object) "sqlserver"));
        return str.ToLower() == "sqlserver" || !(str.ToLower() == "postgresql") ? DbServerType.SqlServer : DbServerType.Postgres;
      }
      set
      {
        if (value == DbServerType.SqlServer)
          this.registryAccessor.WriteValue("MSDE\\DatabaseType", (object) "sqlserver");
        else if (value == DbServerType.Postgres)
          this.registryAccessor.WriteValue("MSDE\\DatabaseType", (object) "postgresql");
        else
          this.registryAccessor.WriteValue("MSDE\\DatabaseType", (object) "sqlserver");
      }
    }

    public string DatabaseServer
    {
      get
      {
        return string.Concat(this.registryAccessor.ReadValue("MSDE\\Server", (object) "(local)\\EMMSDE"));
      }
      set => this.registryAccessor.WriteValue("MSDE\\Server", (object) value);
    }

    public string DatabaseAGListener
    {
      get
      {
        return string.Concat(this.registryAccessor.ReadValue("MSDE\\AvailabilityGroup\\ListenerName"));
      }
      set
      {
        this.registryAccessor.WriteValue("MSDE\\AvailabilityGroup\\ListenerName", (object) value);
      }
    }

    public bool DisabledAGListener
    {
      get
      {
        return this.registryAccessor.ReadValue("MSDE\\AvailabilityGroup\\Disabled", (object) "(local)\\EMMSDE")?.ToString() == "1";
      }
      set => this.registryAccessor.WriteValue("MSDE\\AvailabilityGroup\\Disabled", (object) value);
    }

    public string Port
    {
      get => string.Concat(this.registryAccessor.ReadValue("MSDE\\Port"));
      set => this.registryAccessor.WriteValue("MSDE\\Port", (object) value);
    }

    public string DatabaseName
    {
      get => string.Concat(this.registryAccessor.ReadValue("MSDE\\Database", (object) "emdb"));
      set => this.registryAccessor.WriteValue("MSDE\\Database", (object) value);
    }

    public string DatabaseUserID
    {
      get => string.Concat(this.registryAccessor.ReadValue("MSDE\\UserID", (object) "emdbuser"));
      set => this.registryAccessor.WriteValue("MSDE\\UserID", (object) value);
    }

    public string DatabasePassword
    {
      get
      {
        try
        {
          return RegistryInstanceConfiguration.decryptString(string.Concat(this.registryAccessor.ReadValue("MSDE\\Credentials")));
        }
        catch
        {
          return "";
        }
      }
      set
      {
        this.registryAccessor.WriteValue("MSDE\\Credentials", (object) RegistryInstanceConfiguration.encryptString(value));
      }
    }

    public string MongoActiveHosts
    {
      get
      {
        return string.Concat(this.registryAccessor.ReadValue("MongoDB\\ActiveHosts", (object) "localhost"));
      }
      set => this.registryAccessor.WriteValue("MongoDB\\ActiveHosts", (object) value);
    }

    public string MongoActiveDatabase
    {
      get
      {
        return string.Concat(this.registryAccessor.ReadValue("MongoDB\\ActiveDatabase", (object) "testinstance-active"));
      }
      set => this.registryAccessor.WriteValue("MongoDB\\ActiveDatabase", (object) value);
    }

    public string MongoActiveCredentials
    {
      get
      {
        return string.Concat(this.registryAccessor.ReadValue("MongoDB\\ActiveCredentials", (object) ""));
      }
      set => this.registryAccessor.WriteValue("MongoDB\\ActiveCredentials", (object) value);
    }

    public string MongoActiveOptions
    {
      get => string.Concat(this.registryAccessor.ReadValue("MongoDB\\ActiveOptions", (object) ""));
      set => this.registryAccessor.WriteValue("MongoDB\\ActiveOptions", (object) value);
    }

    public string MongoArchiveHosts
    {
      get
      {
        return string.Concat(this.registryAccessor.ReadValue("MongoDB\\ArchiveHosts", (object) "localhost"));
      }
      set => this.registryAccessor.WriteValue("MongoDB\\ArchiveHosts", (object) value);
    }

    public string MongoArchiveDatabase
    {
      get
      {
        return string.Concat(this.registryAccessor.ReadValue("MongoDB\\ArchiveDatabase", (object) "testinstance-archive"));
      }
      set => this.registryAccessor.WriteValue("MongoDB\\ArchiveDatabase", (object) value);
    }

    public string MongoArchiveCredentials
    {
      get
      {
        return string.Concat(this.registryAccessor.ReadValue("MongoDB\\ArchiveCredentials", (object) ""));
      }
      set => this.registryAccessor.WriteValue("MongoDB\\ArchiveCredentials", (object) value);
    }

    public string DesignatedVersion
    {
      get
      {
        return string.Concat(this.registryAccessor.ReadValue(nameof (DesignatedVersion), (object) ""));
      }
      set => this.registryAccessor.WriteValue(nameof (DesignatedVersion), (object) value);
    }

    public string TPOAdminSiteUrl
    {
      get => RegistryInstanceConfiguration.tpoAdminSiteurl;
      set => RegistryInstanceConfiguration.tpoAdminSiteurl = value;
    }

    public string MongoArchiveOptions
    {
      get => string.Concat(this.registryAccessor.ReadValue("MongoDB\\ArchiveOptions", (object) ""));
      set => this.registryAccessor.WriteValue("MongoDB\\ArchiveOptions", (object) value);
    }

    public string PostgresDatabaseHost
    {
      get => string.Concat(this.registryAccessor.ReadValue("Postgres\\Host", (object) ""));
      set => this.registryAccessor.WriteValue("Postgres\\Host", (object) value);
    }

    public string PostgresDatabasePort
    {
      get => string.Concat(this.registryAccessor.ReadValue("Postgres\\Port", (object) ""));
      set => this.registryAccessor.WriteValue("Postgres\\Port", (object) value);
    }

    public string PostgresDatabaseUsername
    {
      get => string.Concat(this.registryAccessor.ReadValue("Postgres\\Username", (object) ""));
      set => this.registryAccessor.WriteValue("Postgres\\Username", (object) value);
    }

    public string PostgresDatabasePassword
    {
      get
      {
        try
        {
          return RegistryInstanceConfiguration.decryptString(string.Concat(this.registryAccessor.ReadValue("Postgres\\Password")));
        }
        catch
        {
          return "";
        }
      }
      set
      {
        this.registryAccessor.WriteValue("Postgres\\Password", (object) RegistryInstanceConfiguration.encryptString(value));
      }
    }

    public string PostgresDatabaseName
    {
      get => string.Concat(this.registryAccessor.ReadValue("Postgres\\Database", (object) ""));
      set => this.registryAccessor.WriteValue("Postgres\\Database", (object) value);
    }

    public string MongoActiveHosts2
    {
      get
      {
        return string.Concat(this.registryAccessor.ReadValue("MongoDB\\ActiveHosts2", (object) "localhost"));
      }
      set => this.registryAccessor.WriteValue("MongoDB\\ActiveHosts2", (object) value);
    }

    public string MongoActiveDatabase2
    {
      get
      {
        return string.Concat(this.registryAccessor.ReadValue("MongoDB\\ActiveDatabase2", (object) "testinstance-active2"));
      }
      set => this.registryAccessor.WriteValue("MongoDB\\ActiveDatabase2", (object) value);
    }

    public string MongoActiveCredentials2
    {
      get
      {
        return string.Concat(this.registryAccessor.ReadValue("MongoDB\\ActiveCredentials2", (object) ""));
      }
      set => this.registryAccessor.WriteValue("MongoDB\\ActiveCredentials2", (object) value);
    }

    public string MongoActiveOptions2
    {
      get => string.Concat(this.registryAccessor.ReadValue("MongoDB\\ActiveOptions2", (object) ""));
      set => this.registryAccessor.WriteValue("MongoDB\\ActiveOptions2", (object) value);
    }

    public string MongoArchiveHosts2
    {
      get
      {
        return string.Concat(this.registryAccessor.ReadValue("MongoDB\\ArchiveHosts2", (object) "localhost"));
      }
      set => this.registryAccessor.WriteValue("MongoDB\\ArchiveHosts2", (object) value);
    }

    public string MongoArchiveDatabase2
    {
      get
      {
        return string.Concat(this.registryAccessor.ReadValue("MongoDB\\ArchiveDatabase2", (object) "testinstance-archive2"));
      }
      set => this.registryAccessor.WriteValue("MongoDB\\ArchiveDatabase2", (object) value);
    }

    public string MongoArchiveCredentials2
    {
      get
      {
        return string.Concat(this.registryAccessor.ReadValue("MongoDB\\ArchiveCredentials2", (object) ""));
      }
      set => this.registryAccessor.WriteValue("MongoDB\\ArchiveCredentials2", (object) value);
    }

    public string MongoArchiveOptions2
    {
      get
      {
        return string.Concat(this.registryAccessor.ReadValue("MongoDB\\ArchiveOptions2", (object) ""));
      }
      set => this.registryAccessor.WriteValue("MongoDB\\ArchiveOptions2", (object) value);
    }

    private static string decryptString(string cipherText)
    {
      lock (RegistryInstanceConfiguration.jed)
      {
        RegistryInstanceConfiguration.jed.b();
        return RegistryInstanceConfiguration.jed.a((Stream) new MemoryStream(Convert.FromBase64String(cipherText)));
      }
    }

    private static string encryptString(string plainText)
    {
      lock (RegistryInstanceConfiguration.jed)
      {
        RegistryInstanceConfiguration.jed.b();
        return Convert.ToBase64String(RegistryInstanceConfiguration.jed.b(plainText));
      }
    }

    private static b CreateJed() => a.b("z5cty6u5dj3bd8");

    public static RegistryAccessor CreateInstanceRegistryAccessor(string instanceName)
    {
      return new RegistryAccessor(Registry.LocalMachine, EnGlobalSettings.GetInstanceRootKeyPath(instanceName));
    }

    public string EncompassPlatformAPI
    {
      get => string.Concat(this.registryAccessor.ReadValue(nameof (EncompassPlatformAPI)));
      set => this.registryAccessor.WriteValue(nameof (EncompassPlatformAPI), (object) value);
    }
  }
}
