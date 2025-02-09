// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.RegistryConfigurationProvider
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Elli.DirectoryServices.Contracts.Dto;
using Elli.DirectoryServices.Proxies;
using EllieMae.EMLite.Common.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class RegistryConfigurationProvider : IInstanceConfigurationProvider
  {
    public IInstanceConfiguration GetConfiguration(string instanceName)
    {
      RegistryInstanceConfiguration instanceConfiguration = new RegistryInstanceConfiguration(instanceName);
      if (instanceName != "")
      {
        try
        {
          IEnumerable<DirectoryEntryDto> entriesInInstance = new DirectoryServiceClient().GetEntriesInInstance(instanceName);
          if (entriesInInstance != null)
          {
            if (entriesInInstance.Any<DirectoryEntryDto>((Func<DirectoryEntryDto, bool>) (x => x.Name.Equals("TPOAdminSiteUrl", StringComparison.CurrentCultureIgnoreCase))))
              instanceConfiguration.TPOAdminSiteUrl = entriesInInstance.Single<DirectoryEntryDto>((Func<DirectoryEntryDto, bool>) (x => x.Name.Equals("TPOAdminSiteUrl", StringComparison.CurrentCultureIgnoreCase))).Value.ToString();
          }
        }
        catch
        {
        }
      }
      return instanceConfiguration.RegistryHive.Exists() ? (IInstanceConfiguration) instanceConfiguration : throw new InvalidOperationException("The specified Encompass instance '" + instanceName + "' is invalid.");
    }

    public override string ToString() => "[Registry Provider]";
  }
}
