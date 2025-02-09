// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.AsmResolver.Utils.BasicUtils
// Assembly: RestoreAppLauncher, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DF703729-AA3A-440A-B03B-08F970F67A28
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\RestoreAppLauncher.exe

using Microsoft.Win32;
using RestoreAppLauncher.Properties;
using System;
using System.Diagnostics;
using System.Management;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Security.Principal;
using System.ServiceProcess;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.Encompass.AsmResolver.Utils
{
  public class BasicUtils
  {
    internal static readonly string Localization = "l10n";
    internal static readonly string Internationalization = "i18n";

    public static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
    {
      return string.Compare(new AssemblyName(args.Name).Name, "ICSharpCode.SharpZipLib", true) == 0 ? Assembly.Load(Resources.ICSharpCode_SharpZipLib) : (Assembly) null;
    }

    public static int RegistryDebugLevel
    {
      get
      {
        object registryValue = BasicUtils.GetRegistryValue("DebugLevel", "Ellie Mae");
        return registryValue == null ? 0 : Convert.ToInt32((string) registryValue);
      }
    }

    internal static string[] DebugCategories
    {
      get
      {
        if (!(BasicUtils.GetRegistryValue("Debug", (string) null) is string registryValue))
          return (string[]) null;
        char[] separator = new char[2]{ ',', ';' };
        string[] debugCategories = registryValue.Split(separator, StringSplitOptions.RemoveEmptyEntries);
        for (int index = 0; index < debugCategories.Length; ++index)
          debugCategories[index] = debugCategories[index].Trim();
        return debugCategories;
      }
    }

    internal static void DisplayDebuggingInfo(string category, string msg)
    {
      string[] debugCategories = BasicUtils.DebugCategories;
      if (debugCategories == null)
        return;
      foreach (string strA in debugCategories)
      {
        if (string.Compare(strA, category, true) == 0 || strA == "*")
        {
          int num = (int) MessageBox.Show(msg, Consts.EncompassSmartClient);
          break;
        }
      }
    }

    public static object GetRegistryValue(string valueName, string appCompanyName)
    {
      if (appCompanyName == null)
        appCompanyName = "Ellie Mae";
      object registryValue = (object) null;
      using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\" + appCompanyName + "\\SmartClient"))
      {
        if (registryKey != null)
          registryValue = registryKey.GetValue(valueName);
      }
      if (registryValue == null)
      {
        using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\" + appCompanyName + "\\SmartClient"))
        {
          if (registryKey == null)
            return (object) null;
          registryValue = registryKey.GetValue(valueName);
        }
      }
      return registryValue;
    }

    public static bool IsAeLookupSvcDisabled()
    {
      bool flag = false;
      try
      {
        new PermissionSet(PermissionState.Unrestricted).Demand();
        foreach (ManagementBaseObject managementBaseObject in new ManagementObjectSearcher("SELECT * FROM Win32_Service WHERE Name='AeLookupSvc'").Get())
        {
          if (managementBaseObject["StartMode"].ToString() == "Disabled")
          {
            flag = true;
            break;
          }
        }
      }
      catch (Exception ex)
      {
        if (BasicUtils.RegistryDebugLevel >= 2)
          EventLog.WriteEntry(Consts.EventLogSource, "Unable to get AeLookupSvc start mode: " + ex.Message, EventLogEntryType.Error);
      }
      return flag;
    }

    public static bool IsAdministrator()
    {
      return new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
    }

    public static void Elevate(string[] args)
    {
      Process.Start(new ProcessStartInfo()
      {
        UseShellExecute = true,
        WorkingDirectory = Environment.CurrentDirectory,
        FileName = Application.ExecutablePath,
        Arguments = "-DontElevate \"" + string.Join("\" \"", args) + "\"",
        Verb = "runas"
      });
      Process.GetCurrentProcess().Kill();
    }

    public static void StopService(string svcName)
    {
      using (ServiceController serviceController = new ServiceController(svcName))
      {
        try
        {
          if (serviceController.Status != ServiceControllerStatus.Running)
            return;
          serviceController.Stop();
        }
        catch
        {
        }
      }
    }

    public static void StartService(string svcName)
    {
      using (ServiceController serviceController = new ServiceController(svcName))
      {
        if (serviceController.Status != ServiceControllerStatus.Stopped)
          return;
        serviceController.Start();
      }
    }
  }
}
