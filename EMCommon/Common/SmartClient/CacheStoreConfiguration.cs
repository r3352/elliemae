// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.SmartClient.CacheStoreConfiguration
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Configuration;

#nullable disable
namespace EllieMae.EMLite.Common.SmartClient
{
  public class CacheStoreConfiguration : ConfigurationSection
  {
    static CacheStoreConfiguration()
    {
      try
      {
        CacheStoreConfiguration.currentConfiguration = (CacheStoreConfiguration) ConfigurationManager.GetSection("CacheStoreSettings");
      }
      catch (Exception ex)
      {
        CacheStoreConfiguration.currentConfiguration = (CacheStoreConfiguration) null;
      }
    }

    private static CacheStoreConfiguration currentConfiguration { get; set; }

    public static CacheStoreSource CacheStoreType
    {
      get
      {
        return CacheStoreConfiguration.currentConfiguration == null ? CacheStoreSource.InProcess : CacheStoreConfiguration.currentConfiguration.type;
      }
    }

    [ConfigurationProperty("Type", DefaultValue = "InProcess", IsRequired = false)]
    private CacheStoreSource type
    {
      get
      {
        return !(this["Type"].ToString().ToUpper() == "HAZELCAST") ? CacheStoreSource.InProcess : CacheStoreSource.HazelCast;
      }
    }
  }
}
