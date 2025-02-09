// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.InstanceConfiguration
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.Server.Configuration;
using System;
using System.Configuration;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class InstanceConfiguration : ConfigurationSection
  {
    static InstanceConfiguration()
    {
      InstanceConfiguration instanceConfiguration = (InstanceConfiguration) null;
      try
      {
        instanceConfiguration = (InstanceConfiguration) ConfigurationManager.GetSection("instanceConfig");
      }
      catch
      {
      }
      if (instanceConfiguration != null && !string.IsNullOrWhiteSpace(instanceConfiguration.ProviderType))
        InstanceConfiguration.Provider = InstanceConfiguration.CreateInstanceConfigurationProvider(instanceConfiguration.ProviderType, instanceConfiguration.ProviderSource);
      else
        InstanceConfiguration.Provider = (IInstanceConfigurationProvider) new RegistryConfigurationProvider();
    }

    internal static IInstanceConfigurationProvider Provider { get; private set; }

    [ConfigurationProperty("provider", IsRequired = true)]
    public string ProviderType
    {
      get => string.Concat(this["provider"]);
      set => this["provider"] = (object) value;
    }

    [ConfigurationProperty("source", DefaultValue = "", IsRequired = false)]
    public string ProviderSource
    {
      get => string.Concat(this["source"]);
      set => this["source"] = (object) value;
    }

    internal static IInstanceConfigurationProvider CreateInstanceConfigurationProvider(
      string provider,
      string source)
    {
      switch (provider)
      {
        case "registry":
          return (IInstanceConfigurationProvider) new RegistryConfigurationProvider();
        case "directory":
          return (IInstanceConfigurationProvider) new DirectoryConfigurationProvider();
        default:
          throw new ArgumentException("The instance configuration provider '" + provider + "' is not a valid value");
      }
    }
  }
}
