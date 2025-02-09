// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.AsmResolver.Utils.DeployUtils
// Assembly: EllieMae.Encompass.AsmResolver, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF61C82F-0411-4587-80AB-293D90FB58E7
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\EllieMae.Encompass.AsmResolver.dll

using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

#nullable disable
namespace EllieMae.Encompass.AsmResolver.Utils
{
  public class DeployUtils
  {
    public static string[] ValidPriorityStrings = new string[4]
    {
      "NoBackgroundDownload",
      "Lowest",
      "BelowNormal",
      "Normal"
    };

    public static string GetUacFolder(string appCompanyName)
    {
      return DeployUtils.GetUacFolder(appCompanyName, false);
    }

    public static string GetUacFolder(string appCompanyName, bool skipHKLMRegistry)
    {
      string uacFolder = (string) null;
      using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\" + appCompanyName + "\\SmartClient", false))
      {
        if (registryKey != null)
          uacFolder = (string) registryKey.GetValue("UACFolder");
      }
      if (BasicUtils.IsAllUsersInstall(appCompanyName) && !skipHKLMRegistry && BasicUtils.IsNullOrEmpty(uacFolder))
      {
        using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\" + appCompanyName + "\\SmartClient", false))
        {
          if (registryKey != null)
            uacFolder = (string) registryKey.GetValue("UACFolder");
          if (uacFolder == null)
            throw new Exception("Registry entry UACFolder not set");
        }
      }
      if (BasicUtils.IsNullOrEmpty(uacFolder))
      {
        string str = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        if (str.ToLower().EndsWith("application data"))
          str = Path.GetDirectoryName(str);
        else if (str.ToLower().EndsWith("appdata\\local"))
          str += "Low";
        uacFolder = Path.Combine(str, "Apps\\UAC");
      }
      while (uacFolder.EndsWith("\\"))
        uacFolder = uacFolder.Substring(0, uacFolder.Length - 1);
      if (!uacFolder.ToUpper().EndsWith("UAC"))
        uacFolder = Path.Combine(uacFolder, "UAC");
      if (BasicUtils.RegistryDebugLevel >= 7 && !skipHKLMRegistry && AssemblyResolver.Instance != null)
        AssemblyResolver.Instance.WriteToEventLog("UAC folder: " + uacFolder, EventLogEntryType.Information);
      return uacFolder;
    }

    public static string ConstructUacHashFolder(string appCompanyName, string appStartupPath)
    {
      return Path.Combine(DeployUtils.GetUacFolder(appCompanyName), appCompanyName + "\\" + HashUtil.ComputeHashB64FilePath(HashAlgorithm.SHA1, appStartupPath));
    }

    public static string ConstructAppDataHashFolder(string appCompanyName, string appStartupPath)
    {
      return Path.Combine(Path.GetDirectoryName(DeployUtils.GetUacFolder(appCompanyName, true)), appCompanyName + "\\" + HashUtil.ComputeHashB64FilePath(HashAlgorithm.SHA1, appStartupPath));
    }

    public static string RemoveDeployExtension(string filePath)
    {
      if (filePath == null)
        return (string) null;
      filePath = filePath.Trim();
      for (int index = 0; index < ResolverConsts.DeployExts.Length; ++index)
      {
        if (filePath.ToLower().EndsWith(ResolverConsts.DeployExts[index]))
        {
          filePath = filePath.Substring(0, filePath.Length - ResolverConsts.DeployExts[index].Length);
          break;
        }
      }
      return filePath;
    }

    public static string RemoveDeployFileExtension(string filePath)
    {
      if (filePath == null)
        return (string) null;
      filePath = filePath.Trim();
      if (filePath.EndsWith(ResolverConsts.DeployFileExt, StringComparison.CurrentCultureIgnoreCase))
        filePath = filePath.Substring(0, filePath.Length - ResolverConsts.DeployFileExt.Length);
      return filePath;
    }

    public static string RemoveDeployZipExtension(string filePath)
    {
      if (filePath == null)
        return (string) null;
      filePath = filePath.Trim();
      if (filePath.ToLower().EndsWith(ResolverConsts.DeployZipExt))
        filePath = filePath.Substring(0, filePath.Length - ResolverConsts.DeployZipExt.Length);
      return filePath;
    }

    public static string RemoveB64Extension(string filePath)
    {
      if (filePath == null)
        return (string) null;
      filePath = filePath.Trim();
      if (filePath.EndsWith(ResolverConsts.B64Ext, StringComparison.CurrentCultureIgnoreCase))
        filePath = filePath.Substring(0, filePath.Length - ResolverConsts.B64Ext.Length);
      return filePath;
    }

    public static string AddDeployFileExtension(string filePath)
    {
      if (filePath == null)
        return (string) null;
      filePath = filePath.Trim();
      if (!filePath.ToLower().EndsWith(ResolverConsts.DeployZipExt))
        filePath += ResolverConsts.DeployFileExt;
      return filePath;
    }

    public static string AddDeployZipExtension(string filePath)
    {
      if (filePath == null)
        return (string) null;
      filePath = filePath.Trim();
      if (filePath.ToLower().EndsWith(ResolverConsts.DeployZipExt))
        return filePath;
      filePath = !filePath.ToLower().EndsWith(ResolverConsts.DeployFileExt) ? filePath + ResolverConsts.DeployZipExt : filePath + ResolverConsts.ZipExt;
      return filePath;
    }

    public static bool ValidateBgDownloadThreadPriorityString(string priority)
    {
      if (priority == null)
        return true;
      foreach (string validPriorityString in DeployUtils.ValidPriorityStrings)
      {
        if (priority.ToLower() == validPriorityString.ToLower())
          return true;
      }
      return false;
    }

    public static bool DoBackgroundDownload(string priority)
    {
      if (!DeployUtils.ValidateBgDownloadThreadPriorityString(priority))
        throw new Exception(priority + ": invalid background download thread priority string");
      return priority != null && !(priority.ToLower() == DeployUtils.ValidPriorityStrings[0].ToLower());
    }

    public static string AddVersionSuffix(string name, Version version)
    {
      return string.Format("{0}_{1}_{2}_{3}_{4}", (object) name, (object) version.Major, (object) version.Minor, (object) version.Build, (object) version.Revision);
    }

    public static string RemoveVersionSuffix(string name)
    {
      Match match = new Regex("_[0-9]*_[0-9]*_[0-9]*_[0-9]*$").Match(name);
      if (match.Success)
        name = name.Substring(0, match.Index);
      return name;
    }
  }
}
