// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.Configuration.DirectoryConfigurationProvider
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Elli.DirectoryServices.Contracts.Services;
using Elli.DirectoryServices.Proxies;
using EllieMae.EMLite.Common.Configuration;
using System;

#nullable disable
namespace EllieMae.EMLite.Server.Configuration
{
  public class DirectoryConfigurationProvider : IInstanceConfigurationProvider
  {
    private readonly IDirectoryService _directoryService;

    public DirectoryConfigurationProvider()
      : this((IDirectoryService) new DirectoryServiceClient())
    {
    }

    public DirectoryConfigurationProvider(IDirectoryService directoryService)
    {
      this._directoryService = directoryService;
    }

    public IInstanceConfiguration GetConfiguration(string instanceName)
    {
      DirectoryInstanceConfiguration instanceConfiguration = new DirectoryInstanceConfiguration(instanceName, this._directoryService);
      if (instanceConfiguration == null)
        throw new InvalidOperationException("The specified Encompass instance '" + instanceName + "' is invalid.");
      return instanceConfiguration.IsValid ? (IInstanceConfiguration) instanceConfiguration : throw new InvalidOperationException("The specified Encompass instance '" + instanceName + "' configuration is invalid.");
    }

    public override string ToString() => "[Directory Services Provider]";
  }
}
