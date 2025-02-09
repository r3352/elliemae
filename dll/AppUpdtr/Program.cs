// Decompiled with JetBrains decompiler
// Type: AppUpdtr.Program
// Assembly: AppUpdtr, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3ADBA3EC-518B-4BBF-94A2-A2027DADA3FC
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\AppUpdtr.exe

using AppUpdtr.Properties;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace AppUpdtr
{
  internal static class Program
  {
    private const string updaterNameWOExt = "AppUpdtr";
    private const string tmpDirBase = "EncompassSC";
    private const string asmResolverDll = "EllieMae.Encompass.AsmResolver.dll";
    private static string[] appSuiteApps = new string[11]
    {
      "AppLauncher",
      "Encompass",
      "AdminTools",
      "EncAdminTools",
      "ClickLoanProxy",
      "ContourMigration",
      "Gen2EncImp",
      "FormBuilder",
      "SettingsTool",
      "CRMTool",
      "SDKConfig"
    };

    [STAThread]
    private static int Main(string[] args)
    {
      ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
      AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(Program.currentDomain_AssemblyResolve);
      return Program.run(args);
    }

    private static int run(string[] args)
    {
      try
      {
        string environmentVariable = Environment.GetEnvironmentVariable("TMP");
        if ((environmentVariable ?? "").Trim() == "")
          environmentVariable = Environment.GetEnvironmentVariable("TEMP");
        string str = Path.Combine(environmentVariable, "EncompassSC");
        if (!Directory.Exists(str))
          Directory.CreateDirectory(str);
        Tracing.Init(Path.Combine(str, "AppUpdtr"), false);
        Tracing.Log("", "Start", "AppUpdtr.exe starts");
        Program.ParsedArgs args1 = new Program.ParsedArgs(args);
        List<string> stringList = new List<string>();
        stringList.AddRange((IEnumerable<string>) Program.appSuiteApps);
        string appSetting = ConfigurationManager.AppSettings["AppSuiteApplications"];
        if (appSetting != null)
        {
          string[] strArray = appSetting.Split(new char[1]
          {
            ','
          }, StringSplitOptions.RemoveEmptyEntries);
          if (strArray != null)
          {
            for (int index = 0; index < strArray.Length; ++index)
            {
              if (strArray[index].Trim() != "")
                stringList.Add(strArray[index].Trim());
            }
          }
        }
        string withoutExtension1 = Path.GetFileNameWithoutExtension(args1.RestartExePath);
        if (!stringList.Contains(withoutExtension1))
          stringList.Add(withoutExtension1);
        foreach (string path in args1.FilesToUpdate)
        {
          if (!path.ToLower().EndsWith(".dll"))
          {
            string withoutExtension2 = Path.GetFileNameWithoutExtension(path);
            if (!stringList.Contains(withoutExtension2))
              stringList.Add(withoutExtension2);
          }
        }
        Program.appSuiteApps = stringList.ToArray();
        Tracing.Log("VERBOSE", "AppUpdtr", "App suite apps: " + string.Join(":", Program.appSuiteApps));
        bool flag = false;
        Mutex mutex = (Mutex) null;
        try
        {
          bool createdNew;
          mutex = new Mutex(true, "AppUpdtr", out createdNew);
          if (!createdNew)
            mutex.WaitOne();
          flag = Program.setProcessRegistryKey(0);
          return (int) Program.installFiles(args1);
        }
        catch (Exception ex)
        {
          Tracing.Log("ERROR", "AppUpdtr", "Unexpected error: " + (object) ex);
          return 3;
        }
        finally
        {
          if (flag)
            Program.setProcessRegistryKey(1);
          mutex?.ReleaseMutex();
        }
      }
      finally
      {
        try
        {
          Tracing.Close();
        }
        catch
        {
        }
      }
    }

    private static void updateEncompassExeConfig()
    {
      try
      {
        System.IO.File.WriteAllText(Path.Combine(Application.StartupPath, "Encompass.exe.config"), Resources.Encompass_exe_config);
      }
      catch (Exception ex)
      {
        Tracing.Log("ERROR", "AppUpdtr", "Unable to update Encompass.exe.config: " + (object) ex);
      }
    }

    private static Assembly currentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
    {
      return string.Compare(new AssemblyName(args.Name).Name, "RestoreAppLauncher", true) == 0 ? Assembly.Load(Resources.RestoreAppLauncher) : (Assembly) null;
    }

    private static void forceDelete(string cmdArgs)
    {
      string str = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "cmd.exe");
      Process process = new Process();
      process.StartInfo.FileName = str;
      process.StartInfo.Arguments = cmdArgs;
      process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
      process.StartInfo.CreateNoWindow = true;
      process.StartInfo.UseShellExecute = true;
      process.Start();
      process.WaitForExit();
      int exitCode = process.ExitCode;
      if (exitCode != 0)
        throw new Exception("cmd.exe " + cmdArgs + ": return code = " + (object) exitCode);
    }

    private static ExecutionResult installFiles(Program.ParsedArgs args)
    {
      Tracing.Log("INFO", "AppUpdtr", "Installing files...");
      string withoutExtension = Path.GetFileNameWithoutExtension(args.RestartExePath);
      if (!Program.waitForApplicationExit(withoutExtension))
        return ExecutionResult.UserAbort;
      bool flag = false;
      for (int index = 1; index < args.FilesToUpdate.Length; ++index)
      {
        if (args.FilesToUpdate[index].ToLower().LastIndexOf("EllieMae.Encompass.AsmResolver.dll".ToLower()) >= 0)
        {
          flag = true;
          break;
        }
      }
      if (!Program.closeApps(withoutExtension, flag ? Program.appSuiteApps : (string[]) null))
        return ExecutionResult.UserAbort;
      string startupPath = Application.StartupPath;
      for (int index1 = 0; index1 < args.FilesToUpdate.Length; ++index1)
      {
        Mutex mutex = (Mutex) null;
        try
        {
          string fileName = Path.GetFileName(args.FilesToUpdate[index1]);
          bool createdNew;
          mutex = new Mutex(true, fileName, out createdNew);
          if (!createdNew)
            mutex.WaitOne();
          string str = Path.Combine(startupPath, fileName);
          int num1 = 50;
          int millisecondsTimeout = 100;
          for (int index2 = num1; index2 >= 0; --index2)
          {
            try
            {
              Program.forceDelete("/C del /F /Q \"" + str + "\"");
              break;
            }
            catch (Exception ex)
            {
              if (index2 == 0)
              {
                if (MessageBox.Show("Cannot delete file '" + str + "'? Try again?\r\n" + ex.Message, "Application Installer", MessageBoxButtons.YesNo) != DialogResult.No)
                  index2 = num1;
                else
                  break;
              }
              else
                Thread.Sleep(millisecondsTimeout);
            }
          }
          try
          {
            System.IO.File.Copy(args.FilesToUpdate[index1], str, true);
            Tracing.Log("VERBOSE", "AppUpdtr", "Update local file; copy from " + args.FilesToUpdate[index1]);
          }
          catch (Exception ex)
          {
            if (string.Compare(Path.GetExtension(str), ".exe", true) != 0)
              throw ex;
            int num2;
            for (num2 = 4; num2 > 0; --num2)
            {
              Thread.Sleep(250);
              try
              {
                System.IO.File.Copy(args.FilesToUpdate[index1], str, true);
                Tracing.Log("VERBOSE", "AppUpdtr", "Update local file; copy from " + args.FilesToUpdate[index1]);
                break;
              }
              catch
              {
              }
            }
            if (num2 <= 0)
            {
              if (!Program.runRestoreAppLauncher())
                throw ex;
            }
          }
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show("Error trying to update application: " + ex.Message + "\r\nPlease try executing the application again later.");
          return ExecutionResult.UnexpectedError;
        }
        finally
        {
          mutex?.ReleaseMutex();
        }
      }
      Program.startApplication(args.RestartExePath, args.RestartExeArgsAsString);
      Tracing.Log("INFO", "AppUpdtr", "Files installation complete.");
      return ExecutionResult.Success;
    }

    public static bool runRestoreAppLauncher()
    {
      string path = Path.Combine(Application.StartupPath, "RestoreAppLauncher.exe");
      try
      {
        Program._runRestoreAppLauncher();
      }
      catch (Exception ex)
      {
        if (!System.IO.File.Exists(path))
          throw ex;
      }
      Process process = new Process();
      process.StartInfo.FileName = path;
      process.StartInfo.Arguments = "-ExcludeUpdtr";
      process.Start();
      process.WaitForExit();
      return process.ExitCode == 0;
    }

    public static bool _runRestoreAppLauncher()
    {
      return RestoreAppLauncher.Program.Main(new string[1]
      {
        "-ExcludeUpdtr"
      }) == 0;
    }

    private static bool waitForApplicationExit(string applicationName)
    {
      try
      {
        foreach (Process process in Process.GetProcessesByName(applicationName))
        {
          if (!process.WaitForExit(10000))
          {
            while (!process.HasExited)
            {
              if (MessageBox.Show("Please close all " + applicationName + " applications in order to allow the system to update itself.", "Update", MessageBoxButtons.RetryCancel, MessageBoxIcon.Exclamation) == DialogResult.Cancel)
                return false;
            }
          }
        }
      }
      catch
      {
      }
      return true;
    }

    private static bool ensureClientStopped(string clientName)
    {
      foreach (Process process in Process.GetProcessesByName(clientName))
      {
        DialogResult dialogResult;
        do
        {
          dialogResult = MessageBox.Show("Please exit the " + clientName + " application in order to allow the system to update itself.", "Update", MessageBoxButtons.RetryCancel, MessageBoxIcon.Exclamation);
        }
        while (dialogResult == DialogResult.Retry && !process.HasExited);
        if (dialogResult == DialogResult.Cancel)
          return false;
      }
      return true;
    }

    private static void startApplication(string exeName, string arguments)
    {
      Tracing.Log("VERBOSE", "AppUpdtr", "Re-launching application " + exeName);
      new Process()
      {
        StartInfo = {
          FileName = exeName,
          Arguments = arguments,
          WorkingDirectory = Path.GetDirectoryName(Application.ExecutablePath)
        }
      }.Start();
    }

    private static bool closeApps(string clientName, string[] appNames)
    {
      try
      {
        if (!Program.ensureClientStopped(clientName))
          return false;
        if (appNames != null)
        {
          foreach (string appName in appNames)
            Program.killProcess(appName);
        }
      }
      catch
      {
      }
      return true;
    }

    private static void killProcess(string processName)
    {
      foreach (Process proc in Process.GetProcessesByName(processName))
        Program.killWait(proc);
    }

    private static void killWait(Process proc)
    {
      proc.Kill();
      proc.WaitForExit(10000);
    }

    private static bool setProcessRegistryKey(int value)
    {
      try
      {
        RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("System\\CurrentControlSet\\Services\\PerfProc\\Performance", true);
        if (registryKey == null)
          return false;
        try
        {
          if ((int) registryKey.GetValue("Disable Performance Counters", (object) 0) != value)
          {
            registryKey.SetValue("Disable Performance Counters", (object) value);
            return true;
          }
        }
        finally
        {
          registryKey.Close();
        }
      }
      catch
      {
      }
      return false;
    }

    private class ParsedArgs
    {
      public readonly string RestartExePath;
      public readonly string[] RestartExeArgs;
      public readonly string[] FilesToUpdate;
      private List<string> restartExeArgs;
      private List<string> filesToUpdate;

      public string RestartExeArgsAsString
      {
        get
        {
          if (this.RestartExeArgs == null)
            return (string) null;
          string[] strArray = new string[this.RestartExeArgs.Length];
          for (int index = 0; index < this.RestartExeArgs.Length; ++index)
            strArray[index] = this.RestartExeArgs[index].IndexOf(' ') >= 0 || this.RestartExeArgs[index].IndexOf('\t') >= 0 || this.RestartExeArgs[index] == "" ? "\"" + this.RestartExeArgs[index] + "\"" : this.RestartExeArgs[index];
          return string.Join(" ", strArray);
        }
      }

      public string EncodeCommand(string commandPath)
      {
        return commandPath.Replace("|", "").Replace("&", "");
      }

      public ParsedArgs(string[] args)
      {
        if (args.Length == 0)
        {
          Tracing.Log("WARNING", "AppUpdtr", "No args passed in");
        }
        else
        {
          this.RestartExePath = this.EncodeCommand(args[0]);
          if (args.Length == 1)
          {
            Tracing.Log("WARNING", "AppUpdtr", "No files to update passed in");
          }
          else
          {
            int index1;
            for (index1 = 1; index1 < args.Length && !(args[index1] == "-"); ++index1)
            {
              if (this.restartExeArgs == null)
                this.restartExeArgs = new List<string>();
              this.restartExeArgs.Add(args[index1]);
            }
            if (this.restartExeArgs != null)
              this.RestartExeArgs = this.restartExeArgs.ToArray();
            if (index1 == args.Length)
            {
              Tracing.Log("WARNING", "AppUpdtr", "No files to update passed in");
            }
            else
            {
              for (int index2 = index1 + 1; index2 < args.Length; ++index2)
              {
                if (this.filesToUpdate == null)
                  this.filesToUpdate = new List<string>();
                this.filesToUpdate.Add(args[index2]);
              }
              if (this.filesToUpdate == null)
                Tracing.Log("WARNING", "AppUpdtr", "No files to update passed in");
              else
                this.FilesToUpdate = this.filesToUpdate.ToArray();
              Tracing.Log("VERBOSE", "AppUpdtr", "Restart exe path: " + this.RestartExePath);
              Tracing.Log("VERBOSE", "AppUpdtr", "Restart exe args: " + this.RestartExeArgsAsString);
              string str = (string) null;
              if (this.FilesToUpdate != null)
                str = string.Join("\r\n", this.FilesToUpdate);
              Tracing.Log("VERBOSE", "AppUpdtr", "Files to update:\r\n" + str);
            }
          }
        }
      }
    }
  }
}
