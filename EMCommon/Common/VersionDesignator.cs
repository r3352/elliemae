// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.VersionDesignator
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using Microsoft.Win32;
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public class VersionDesignator
  {
    private static readonly string nilVersion = "";
    private static readonly object creationLock = new object();
    private static string versionRegistryBasePath = "VersionProxy\\Versions";
    private static string[] designatedVersionTypes = new string[2]
    {
      "IIS",
      "WindowService"
    };
    private static VersionDesignator designator = (VersionDesignator) null;
    private string designatedVersion = VersionDesignator.nilVersion;
    private string designatedVersionType;
    private int? designatedVersionPort;
    private RegistryAccessor registry;

    private VersionDesignator(string registryRoot)
    {
      RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(registryRoot);
      this.registry = new RegistryAccessor(registryKey);
      if (!this.DesignatedVersionEnabled)
        return;
      string assemblyFullPath = this.GetAssemblyFullPath();
      this.designatedVersion = this.GetDesignatedVersion(assemblyFullPath);
      this.designatedVersionType = this.GetDesignatedVersionType(assemblyFullPath);
      this.designatedVersionPort = this.GetDesignatedVersionPort(registryKey);
    }

    private string GetDesignatedVersion(string path)
    {
      if (!this.DesignatedVersionSource.Equals("FILE", StringComparison.CurrentCultureIgnoreCase))
        return VersionDesignator.GetVersionFromPath(path);
      string str1 = Path.Combine(Path.GetDirectoryName(path), this.DesignatedVersionAssembly);
      if (!File.Exists(str1))
        return VersionDesignator.GetVersionFromPath(path);
      string empty = string.Empty;
      string str2 = !string.Equals(Path.GetExtension(str1), ".txt", StringComparison.CurrentCultureIgnoreCase) ? FileVersionInfo.GetVersionInfo(str1).FileVersion : File.ReadAllText(str1);
      return string.IsNullOrWhiteSpace(str2) ? VersionDesignator.nilVersion : str2;
    }

    public static string GetVersionFromPath(string path)
    {
      string str = "^\\d+.\\d+.\\d+";
      if (!Path.IsPathRooted(path))
        return VersionDesignator.GetVersionFromPathString(path, str);
      DirectoryInfo directoryInfo;
      string name;
      try
      {
        directoryInfo = new FileInfo(path).Directory;
        name = directoryInfo.Name;
      }
      catch (NullReferenceException ex)
      {
        return "";
      }
      for (; !Regex.IsMatch(name, str) && directoryInfo.Parent != null; name = directoryInfo.Name)
        directoryInfo = directoryInfo.Parent;
      return Path.IsPathRooted(name) ? "" : name;
    }

    public static string GetVersionFromPathString(string path, string regExPattern)
    {
      path = path.TrimEnd('\\', '/');
      string versionFromPathString = string.Empty;
      string str = path;
      char[] separator = new char[2]{ '\\', '/' };
      foreach (string input in str.Split(separator, StringSplitOptions.RemoveEmptyEntries))
      {
        if (Regex.IsMatch(input, regExPattern))
        {
          versionFromPathString = input;
          break;
        }
      }
      return versionFromPathString;
    }

    private int? GetDesignatedVersionPort(RegistryKey registryRootKey)
    {
      using (RegistryKey registryKey = registryRootKey.OpenSubKey(Path.Combine(VersionDesignator.versionRegistryBasePath, this.DesignatedVersionType)))
      {
        try
        {
          object obj = registryKey.GetValue(this.DesignatedVersion);
          return obj != null ? new int?(Convert.ToInt32(obj)) : new int?();
        }
        catch
        {
          return new int?();
        }
      }
    }

    public string DesignatedVersion => this.designatedVersion;

    public int? DesignatedVersionPort => this.designatedVersionPort;

    public string DesignatedVersionType => this.designatedVersionType;

    public bool IsDesignatedVersionNil => this.DesignatedVersion == VersionDesignator.nilVersion;

    private bool DesignatedVersionEnabled
    {
      get
      {
        bool result;
        return bool.TryParse(string.Concat(this.registry.ReadValue(nameof (DesignatedVersionEnabled))), out result) && result;
      }
      set
      {
        if (!value)
          this.designatedVersion = VersionDesignator.nilVersion;
        this.registry.WriteValue(nameof (DesignatedVersionEnabled), (object) value);
      }
    }

    private string DesignatedVersionAssembly
    {
      get => string.Concat(this.registry.ReadValue(nameof (DesignatedVersionAssembly)));
      set => this.registry.WriteValue(nameof (DesignatedVersionAssembly), (object) value);
    }

    private string DesignatedVersionSource
    {
      get
      {
        return string.Concat(this.registry.ReadValue(nameof (DesignatedVersionSource), (object) "PATH"));
      }
      set => this.registry.WriteValue(nameof (DesignatedVersionSource), (object) value);
    }

    public static VersionDesignator GetInstance(string registryRoot)
    {
      if (VersionDesignator.designator == null)
      {
        lock (VersionDesignator.creationLock)
          VersionDesignator.designator = new VersionDesignator(registryRoot);
      }
      return VersionDesignator.designator;
    }

    private string GetAssemblyFullPath()
    {
      string assemblyFullPath = Assembly.GetExecutingAssembly().CodeBase;
      if (assemblyFullPath.ToLower().StartsWith("file:///"))
        assemblyFullPath = assemblyFullPath.Substring("file:///".Length);
      return assemblyFullPath;
    }

    private string GetDesignatedVersionType(string assemblyCodeBase)
    {
      string str = ConfigurationManager.AppSettings["DesignatedVersionType"] ?? "exe";
      switch (str.ToLower())
      {
        case "web":
          return VersionDesignator.designatedVersionTypes[0];
        case "exe":
          return VersionDesignator.designatedVersionTypes[1];
        default:
          throw new Exception("Bad .config file setting: '" + str + "'. DesignatedVersionType key must be set to 'web' or 'exe'");
      }
    }
  }
}
