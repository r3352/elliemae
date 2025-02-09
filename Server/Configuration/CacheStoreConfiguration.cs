// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.Configuration.CacheStoreConfiguration
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using System;
using System.Configuration;

#nullable disable
namespace EllieMae.EMLite.Server.Configuration
{
  public class CacheStoreConfiguration : ConfigurationSection
  {
    static CacheStoreConfiguration()
    {
      try
      {
        CacheStoreConfiguration.CurrentConfiguration = (CacheStoreConfiguration) ConfigurationManager.GetSection("CacheStoreSettings");
      }
      catch (Exception ex)
      {
        CacheStoreConfiguration.CurrentConfiguration = (CacheStoreConfiguration) null;
      }
    }

    public static CacheStoreConfiguration CurrentConfiguration { get; private set; }

    [ConfigurationProperty("InstanceSwitch", DefaultValue = "", IsRequired = false)]
    public string InstanceSwitch
    {
      get
      {
        string str = this[nameof (InstanceSwitch)].ToString();
        return !string.IsNullOrEmpty(str) ? str : "";
      }
    }

    [ConfigurationProperty("ServerAddresses", IsRequired = false)]
    public string ServerAddresses => this[nameof (ServerAddresses)].ToString();

    public string[] ServerAddress => this.ServerAddresses.Replace(" ", "").Split(';');

    [ConfigurationProperty("ClusterName", IsRequired = false)]
    public string ClusterName => this[nameof (ClusterName)].ToString();

    [ConfigurationProperty("ClusterUsername", IsRequired = false)]
    public string ClusterUsername => this[nameof (ClusterUsername)].ToString();

    [ConfigurationProperty("ClusterPassword", IsRequired = false)]
    public string ClusterPassword => this[nameof (ClusterPassword)].ToString();

    [ConfigurationProperty("MapSuffix", IsRequired = false, DefaultValue = "")]
    public string MapSuffix => this[nameof (MapSuffix)].ToString();

    [ConfigurationProperty("LicenseKey", IsRequired = false, DefaultValue = "")]
    public string LicenseKey => this[nameof (LicenseKey)].ToString();

    [ConfigurationProperty("ConnectionAttemptLimit", IsRequired = false, DefaultValue = 2147483647)]
    public int ConnectionAttemptLimit => Convert.ToInt32(this[nameof (ConnectionAttemptLimit)]);

    [ConfigurationProperty("ConnectionAttemptPeriod", IsRequired = false, DefaultValue = 3000)]
    public int ConnectionAttemptPeriod => Convert.ToInt32(this[nameof (ConnectionAttemptPeriod)]);

    [ConfigurationProperty("LeaseTime", IsRequired = false, DefaultValue = 300)]
    public int LeaseTime => Convert.ToInt32(this[nameof (LeaseTime)]);

    [ConfigurationProperty("LargeObjectSize", IsRequired = false, DefaultValue = 100000)]
    public int LargeObjectSize => Convert.ToInt32(this[nameof (LargeObjectSize)]);

    [ConfigurationProperty("NearCacheObjectSize", IsRequired = false, DefaultValue = -1)]
    public int NearCacheObjectSize => Convert.ToInt32(this[nameof (NearCacheObjectSize)]);

    [ConfigurationProperty("UseHzcForLoanLock", IsRequired = false, DefaultValue = false)]
    public bool UseHzcForLoanLock => Convert.ToBoolean(this[nameof (UseHzcForLoanLock)]);

    [ConfigurationProperty("DisableSSL", IsRequired = false, DefaultValue = true)]
    public bool DisableSSL => Convert.ToBoolean(this[nameof (DisableSSL)]);
  }
}
