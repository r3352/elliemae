// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.IIs.IIs
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;

#nullable disable
namespace EllieMae.EMLite.Common.IIs
{
  public class IIs
  {
    private IIs()
    {
    }

    public static IIsVersion GetVersion()
    {
      RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\InetStp");
      if (registryKey == null)
        return IIsVersion.IIsUnknown;
      using (registryKey)
      {
        switch (string.Concat(registryKey.GetValue("MajorVersion")))
        {
          case "5":
            return IIsVersion.IIs5;
          case "6":
            return IIsVersion.IIs6;
          case "7":
            return IIsVersion.IIs7;
          case "8":
            return IIsVersion.IIs8;
          case "10":
            return IIsVersion.IIs10;
          default:
            return IIsVersion.IIsUnknown;
        }
      }
    }

    public static IWebServer GetWebServer()
    {
      IIsVersion version = EllieMae.EMLite.Common.IIs.IIs.GetVersion();
      if (version == IIsVersion.IIsUnknown)
        return (IWebServer) null;
      if (version <= IIsVersion.IIs6)
        return (IWebServer) new IIs6();
      return version == IIsVersion.IIs7 ? (IWebServer) new IIs7() : (IWebServer) new IIs8();
    }

    public static void StartService()
    {
      try
      {
        SystemUtil.StartService("IISADMIN");
      }
      catch
      {
      }
      string[] strArray = new string[3]
      {
        "W3SVC",
        "SMTPSVC",
        "FTPSVC"
      };
      foreach (string name in strArray)
      {
        using (ServiceController service = new ServiceController(name))
        {
          try
          {
            SystemUtil.StartService(service);
          }
          catch
          {
          }
        }
      }
    }

    public static void StopService()
    {
      SystemUtil.StopService("W3SVC");
      try
      {
        SystemUtil.StopService("IISADMIN");
      }
      catch
      {
      }
    }

    public static bool IsInstalled()
    {
      return EllieMae.EMLite.Common.IIs.IIs.GetVersion() != IIsVersion.IIsUnknown && EllieMae.EMLite.Common.IIs.IIs.GetWebServer().IsInstalled();
    }

    public static bool IsRunning() => SystemUtil.IsServiceRunning("W3SVC");

    public static void InstallASPNET() => EllieMae.EMLite.Common.IIs.IIs.InstallASPNET(false);

    public static void InstallASPNET(bool runDism)
    {
      string aspnetInstallRoot = EllieMae.EMLite.Common.IIs.IIs.GetASPNETInstallRoot("v4.0.30319");
      try
      {
        string str1;
        string str2;
        if (runDism)
        {
          str1 = Path.Combine(Environment.SystemDirectory, "dism.exe");
          str2 = "/online /enable-feature /all /featurename:IIS-ASPNET45";
        }
        else
        {
          str1 = Path.Combine(aspnetInstallRoot, "aspnet_regiis.exe");
          str2 = "-ir" + (EllieMae.EMLite.Common.IIs.IIs.GetVersion() >= IIsVersion.IIs6 ? " -enable" : "");
        }
        Process process = new Process();
        process.StartInfo.FileName = str1;
        process.StartInfo.Arguments = str2;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        process.StartInfo.CreateNoWindow = true;
        process.Start();
        process.WaitForExit();
        if (process.ExitCode != 0)
          throw new ApplicationException("ASP.NET registration processs terminated with non-zero exit code");
      }
      catch (Exception ex)
      {
        throw new ApplicationException("An error occurred while registering the .NET extensions with IIS: " + ex.Message);
      }
    }

    public static string GetASPNETInstallRoot(string version)
    {
      string path1;
      using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\.NETFramework"))
        path1 = (string) registryKey.GetValue("InstallRoot") ?? "";
      return path1 != null ? Path.Combine(path1, version) : throw new ApplicationException("Missing or invalid .NET Framework installation root.");
    }

    public static void RunIisReset()
    {
      try
      {
        Process.Start(new ProcessStartInfo(Path.Combine(Environment.GetEnvironmentVariable("windir"), "system32\\iisreset.exe"))
        {
          WindowStyle = ProcessWindowStyle.Hidden,
          CreateNoWindow = true
        }).WaitForExit();
      }
      catch
      {
      }
    }
  }
}
