// Decompiled with JetBrains decompiler
// Type: RestoreAppLauncher.UACUtil
// Assembly: RestoreAppLauncher, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DF703729-AA3A-440A-B03B-08F970F67A28
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\RestoreAppLauncher.exe

using EllieMae.Encompass.AsmResolver.Utils;
using Microsoft.Win32;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

#nullable disable
namespace RestoreAppLauncher
{
  public class UACUtil
  {
    private static SHA1CryptoServiceProvider _sha1;

    private static SHA1CryptoServiceProvider sha1
    {
      get
      {
        if (UACUtil._sha1 == null)
          UACUtil._sha1 = new SHA1CryptoServiceProvider();
        return UACUtil._sha1;
      }
    }

    public static string ConstructUacHashFolder(string appStartupPath)
    {
      return Path.Combine(UACUtil.GetUacFolder(), Consts.AppCompanyName + "\\" + UACUtil.computeHashB64FilePath(appStartupPath));
    }

    public static string GetUacFolder()
    {
      string path1 = (string) null;
      using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\" + Consts.AppCompanyName + "\\SmartClient", false))
      {
        if (registryKey != null)
          path1 = (string) registryKey.GetValue("UACFolder");
      }
      if ((path1 ?? "").Trim() == "")
      {
        using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\" + Consts.AppCompanyName + "\\SmartClient", false))
        {
          if (registryKey != null)
            path1 = (string) registryKey.GetValue("UACFolder");
          if (path1 == null)
            throw new Exception("Registry entry UACFolder not set");
        }
      }
      if ((path1 ?? "").Trim() == "")
      {
        string str = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        if (str.ToLower().EndsWith("application data"))
          str = Path.GetDirectoryName(str);
        else if (str.ToLower().EndsWith("appdata\\local"))
          str += "Low";
        path1 = Path.Combine(str, "Apps\\UAC");
      }
      while (path1.EndsWith("\\"))
        path1 = path1.Substring(0, path1.Length - 1);
      if (!path1.ToUpper().EndsWith("UAC"))
        path1 = Path.Combine(path1, "UAC");
      return path1;
    }

    private static string computeHashB64FilePath(string pathString)
    {
      return UACUtil.computeHashB64(Encoding.Default.GetBytes(pathString.Trim().ToLower())).Replace("/", "#");
    }

    private static string computeHashB64(byte[] buffer)
    {
      return Convert.ToBase64String(UACUtil.sha1.ComputeHash(buffer));
    }
  }
}
