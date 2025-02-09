// Decompiled with JetBrains decompiler
// Type: RestoreAppLauncher.SCUtil
// Assembly: RestoreAppLauncher, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DF703729-AA3A-440A-B03B-08F970F67A28
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\RestoreAppLauncher.exe

using EllieMae.Encompass.AsmResolver;
using EllieMae.Encompass.AsmResolver.Utils;
using Microsoft.Win32;
using System;
using System.IO;
using System.Management;
using System.Text;

#nullable disable
namespace RestoreAppLauncher
{
  public class SCUtil
  {
    private static string _installationURL;
    private static string _downloadFolderUrlEncompass;
    private static string _downloadFolderUrlAppUpdtr;
    private static string _scAppMgrFolder;

    public static string GetInstallDirFromUacFolder()
    {
      return Path.Combine(Path.GetDirectoryName(UACUtil.GetUacFolder()), "Ellie Mae", "Encompass");
    }

    public static string InstallationURL
    {
      get
      {
        if (SCUtil._installationURL == null)
          SCUtil._installationURL = AuthenticationForm.Authenticate("Encompass.exe").InstallationURL;
        return SCUtil._installationURL;
      }
    }

    public static string GetDownloadFolderUrl(string fileToDownload)
    {
      bool flag = false;
      string str1 = "Encompass360.man";
      string str2 = "Encompass360_";
      if (fileToDownload.StartsWith("AppUpdtr.exe", StringComparison.OrdinalIgnoreCase))
      {
        flag = true;
        str1 = "AppUpdtr.man";
        str2 = "AppUpdtr_";
      }
      if (!flag && SCUtil._downloadFolderUrlEncompass == null || flag && SCUtil._downloadFolderUrlAppUpdtr == null)
      {
        string str3 = Encoding.ASCII.GetString(WebUtil.GetFile(SCUtil.InstallationURL + "/" + str1, 0L, 0L, "Downloading deployment manifest. Please wait..."));
        int startIndex = str3.IndexOf(str2);
        int num = str3.IndexOf("\\", startIndex);
        if (!flag)
          SCUtil._downloadFolderUrlEncompass = SCUtil.InstallationURL + "/" + str3.Substring(startIndex, num - startIndex);
        else
          SCUtil._downloadFolderUrlAppUpdtr = SCUtil.InstallationURL + "/" + str3.Substring(startIndex, num - startIndex);
      }
      return !flag ? SCUtil._downloadFolderUrlEncompass : SCUtil._downloadFolderUrlAppUpdtr;
    }

    public static string SCAppMgrFolder
    {
      get
      {
        if (SCUtil._scAppMgrFolder == null)
        {
          try
          {
            using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Services\\SCAppMgr"))
            {
              if (registryKey != null)
              {
                string str = (string) registryKey.GetValue("ImagePath");
                if (str != null)
                  SCUtil._scAppMgrFolder = Path.GetDirectoryName(str.Trim('"'));
              }
            }
          }
          catch
          {
          }
          if (SCUtil._scAppMgrFolder == null)
          {
            using (ManagementObject managementObject = new ManagementObject("Win32_Service.Name='" + "SCAppMgr" + "'"))
            {
              managementObject.Get();
              string str = managementObject["PathName"].ToString();
              if (str != null)
                SCUtil._scAppMgrFolder = Path.GetDirectoryName(str.Trim('"'));
            }
          }
        }
        return SCUtil._scAppMgrFolder;
      }
    }
  }
}
