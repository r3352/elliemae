// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.Configuration.DirectoryServiceConfiguration
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Elli.DirectoryServices.Contracts.Dto;
using Elli.DirectoryServices.Contracts.Services;
using Elli.DirectoryServices.Proxies;
using EllieMae.EMLite.JedLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.Server.Configuration
{
  public class DirectoryServiceConfiguration
  {
    private static b _jed;
    private static IDirectoryService _directoryService;
    private const string className = "DirectoryServiceConfiguration�";
    private const string CacheName = "DirectorySvcData�";

    static DirectoryServiceConfiguration() => DirectoryServiceConfiguration.ConfigJedLib();

    public static string GetAGListenerDbConnectionString(
      string instanceName,
      string DatabaseConnectionStringFormat)
    {
      try
      {
        IEnumerable<DirectoryEntryDto> categoryEntries = DirectoryServiceConfiguration.GetCategoryEntries(instanceName, "DataStoreSettings");
        if (categoryEntries != null && categoryEntries.Any<DirectoryEntryDto>())
        {
          string str1 = categoryEntries.FirstOrDefault<DirectoryEntryDto>((Func<DirectoryEntryDto, bool>) (x => x.Name.Equals("AGListener", StringComparison.CurrentCultureIgnoreCase)))?.Value.ToString() ?? string.Empty;
          string str2 = categoryEntries.FirstOrDefault<DirectoryEntryDto>((Func<DirectoryEntryDto, bool>) (x => x.Name.Equals("DatabaseName", StringComparison.CurrentCultureIgnoreCase)))?.Value.ToString() ?? string.Empty;
          string str3 = categoryEntries.FirstOrDefault<DirectoryEntryDto>((Func<DirectoryEntryDto, bool>) (x => x.Name.Equals("DatabaseUserID", StringComparison.CurrentCultureIgnoreCase)))?.Value.ToString() ?? string.Empty;
          string cipherText = categoryEntries.FirstOrDefault<DirectoryEntryDto>((Func<DirectoryEntryDto, bool>) (x => x.Name.Equals("DatabaseCredentials", StringComparison.CurrentCultureIgnoreCase)))?.Value.ToString() ?? string.Empty;
          if (!string.IsNullOrEmpty(str1))
            return string.Format(DatabaseConnectionStringFormat, (object) str1, (object) str2, (object) str3, (object) DirectoryServiceConfiguration.DecryptString(cipherText));
        }
        return (string) null;
      }
      catch (Exception ex)
      {
        return (string) null;
      }
    }

    private static IEnumerable<DirectoryEntryDto> GetCategoryEntries(
      string instanceName,
      string category)
    {
      DirectoryServiceConfiguration._directoryService = (IDirectoryService) new DirectoryServiceClient();
      IEnumerable<DirectoryEntryDto> entriesInInstance = DirectoryServiceConfiguration._directoryService.GetEntriesInInstance(instanceName);
      return entriesInInstance == null ? (IEnumerable<DirectoryEntryDto>) null : entriesInInstance.Where<DirectoryEntryDto>((Func<DirectoryEntryDto, bool>) (entry => entry.CategoryName?.Trim().Equals(category?.Trim(), StringComparison.InvariantCultureIgnoreCase).Value));
    }

    private static string GetEntryValue(string instanceName, string category, string entryName)
    {
      DirectoryServiceConfiguration._directoryService = (IDirectoryService) new DirectoryServiceClient();
      return DirectoryServiceConfiguration._directoryService.GetEntry(instanceName, category, entryName)?.Value?.ToString();
    }

    private static string DecryptString(string cipherText)
    {
      lock (DirectoryServiceConfiguration._jed)
      {
        DirectoryServiceConfiguration._jed.b();
        return DirectoryServiceConfiguration._jed.a((Stream) new MemoryStream(Convert.FromBase64String(cipherText)));
      }
    }

    private static void ConfigJedLib()
    {
      a.a("z2r1xy8k5mp4ccpl");
      DirectoryServiceConfiguration._jed = DirectoryServiceConfiguration.CreateJed();
    }

    private static b CreateJed() => a.b("z5cty6u5dj3bd8");
  }
}
