// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.AsmResolver.AssemblyResolver
// Assembly: EllieMae.Encompass.AsmResolver, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF61C82F-0411-4587-80AB-293D90FB58E7
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\EllieMae.Encompass.AsmResolver.dll

using EllieMae.Encompass.AsmResolver.DeploymentHandlers;
using EllieMae.Encompass.AsmResolver.MOTD;
using EllieMae.Encompass.AsmResolver.Utils;
using EllieMae.Encompass.AsmResolver.XmlHelperClasses;
using Microsoft.Win32;
using mshtml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.Principal;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

#nullable disable
namespace EllieMae.Encompass.AsmResolver
{
  public class AssemblyResolver
  {
    public static bool SCPublisherQuietMode = false;
    private string[] defaultSmartClientAssemblies = new string[11]
    {
      "AppLauncherLib",
      "EllieMae.Encompass.SmartClient",
      "EncompassObjects",
      "ClickLoanImpl",
      "MainUI",
      "AdminToolsUI",
      "GenExpImpUI",
      "ContourMigrationUI",
      "ClickLoanProxyImpl",
      "FormEditorUI",
      "SDKConfigImpl"
    };
    private string firstSmartClientAssembly;
    private string firstMissingAssembly;
    public string[] SkipList;
    private EventLog eventLog;
    private string startupPath;
    private string executableName;
    private string executableExt;
    private string[] commandLineArgs;
    private string appSuiteCompanyName;
    private string appSuiteName;
    private string uacHashFolder;
    private string appDataHashFolder;
    private DeploymentHandler deployHandler;
    private AuthenticationResult authResult;
    private bool manifestsDownloaded;
    private static string authServerURL = (string) null;
    private static string scClientID = (string) null;
    private static bool isIexplore = false;
    private static AssemblyResolver instance = (AssemblyResolver) null;
    private int disableSDKAsmResolve;
    private bool asmResolverAndRuntimeVersionChecked;
    private static string[] configFilesToDelete = new string[8]
    {
      "AdminTools.exe.config",
      "ClickLoanProxy.exe.config",
      "CRMTool.exe.config",
      "EncAdminTools.exe.config",
      "Encompass.exe.config",
      "FormBuilder.exe.config",
      "SDKConfig.exe.config",
      "SettingsTool.exe.config"
    };
    private const string scRegistryPath = "Software\\Ellie Mae\\SmartClient";
    private const string regDataDirBase = "DataDirBase";

    public event EventHandler OnResolverInitDone;

    public event EventHandler BeforeAssemblyResolve;

    public event EventHandler AfterAssemblyDownload;

    public event EventHandler AfterAssemblyResolve;

    public static string AppStartupPath
    {
      get
      {
        return AssemblyResolver.Instance == null ? (string) null : AssemblyResolver.Instance.startupPath;
      }
    }

    public string ExecutableNameWOExt => this.executableName;

    public static string InstallationURL
    {
      get
      {
        return AssemblyResolver.Instance == null || AssemblyResolver.Instance.deployHandler == null ? (string) null : AssemblyResolver.Instance.deployHandler.InstallationURL;
      }
    }

    public AuthenticationResult AuthResultI => this.authResult;

    public static bool IsSmartClient
    {
      get => AssemblyResolver.instance != null && AssemblyResolver.instance.manifestsDownloaded;
    }

    public static string GetScAttribute(string attrName)
    {
      if (!AssemblyResolver.IsSmartClient)
        return (string) null;
      using (RegistryKey registryKey = BasicUtils.GetRegistryHive((string) null).OpenSubKey("Software\\Ellie Mae\\SmartClient\\" + Application.StartupPath.Replace("\\", "/") + "\\Attributes"))
        return registryKey == null ? (string) null : (string) registryKey.GetValue(attrName);
    }

    public static string AuthServerURL
    {
      get
      {
        if (AssemblyResolver.authServerURL == null)
        {
          if (AuthenticationForm.LastAuthentication != null)
            AssemblyResolver.authServerURL = AuthenticationForm.LastAuthentication.AuthServerURL;
          else if (AssemblyResolver.Instance != null)
            AssemblyResolver.authServerURL = AssemblyResolver.getCmdLineArg("-AuthServerURL");
        }
        return AssemblyResolver.authServerURL;
      }
    }

    public static string SCClientID
    {
      get
      {
        if (AssemblyResolver.scClientID == null)
        {
          if (AuthenticationForm.LastAuthentication != null)
            AssemblyResolver.scClientID = AuthenticationForm.LastAuthentication.Userid;
          else if (AssemblyResolver.Instance != null)
            AssemblyResolver.scClientID = AssemblyResolver.getCmdLineArg("-SCClientID");
        }
        return !((AssemblyResolver.scClientID ?? "").Trim() == "") ? AssemblyResolver.scClientID : throw new Exception(Path.GetFileName(Application.ExecutablePath) + ": AssemblyResolver.SCClientID: null or empty SmartClient ID.\r\n");
      }
    }

    internal static AssemblyResolver Instance => AssemblyResolver.instance;

    public static void Start(string[] args)
    {
      AssemblyResolver.Start(Application.ExecutablePath, args);
    }

    public static void Start(string executablePath, string[] args)
    {
      AssemblyResolver.Start(Path.GetDirectoryName(executablePath), Path.GetFileName(executablePath), args, (string) null, (string) null);
    }

    public static void Start(string startupPath, string executableNameWExt, string[] args)
    {
      AssemblyResolver.Start(startupPath, executableNameWExt, args, (string) null, (string) null);
    }

    public static void Start(
      string startupPath,
      string executableNameWExt,
      string[] args,
      string appSuiteCompanyName,
      string appSuiteName)
    {
      if (AssemblyResolver.instance != null)
        return;
      AssemblyResolver.instance = new AssemblyResolver(startupPath, executableNameWExt);
      AssemblyResolver.instance.commandLineArgs = args == null ? new string[0] : args;
      AssemblyResolver.instance.appSuiteCompanyName = appSuiteCompanyName;
      AssemblyResolver.instance.appSuiteName = appSuiteName;
    }

    static AssemblyResolver()
    {
      try
      {
        AssemblyResolver.isIexplore = string.Compare(Path.GetFileName(Application.ExecutablePath), "iexplore.exe", StringComparison.CurrentCultureIgnoreCase) == 0;
      }
      catch
      {
        AssemblyResolver.isIexplore = false;
      }
    }

    private AssemblyResolver(string startupPath, string executableNameWExt)
    {
      if (string.Compare(executableNameWExt, "AdminTools.exe", true) == 0)
        Win64.SyncEncompass32To64();
      this.eventLog = new EventLog();
      this.eventLog.BeginInit();
      this.eventLog.Log = "Application";
      this.eventLog.Source = nameof (AssemblyResolver);
      this.eventLog.EndInit();
      this.startupPath = startupPath;
      this.executableName = Path.GetFileNameWithoutExtension(executableNameWExt);
      this.executableExt = Path.GetExtension(executableNameWExt);
      AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(this.currentDomain_AssemblyResolve);
    }

    private Assembly currentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
    {
      string name = new AssemblyName(args.Name).Name;
      if (name.EndsWith(".XmlSerializers") || name.EndsWith(".resources") || name.Trim().ToLower() == "act.shared.licprovider" || name.Trim().ToLower() == "act.ui" || name == "Serilog" || name == "NLog" || string.Equals(name, "Unity_ILEmit_DynamicClasses", StringComparison.OrdinalIgnoreCase))
      {
        if (BasicUtils.RegistryDebugLevel >= 10)
          this.WriteToEventLog("Trying to resolve assembly " + name, EventLogEntryType.Information);
        return (Assembly) null;
      }
      if (BasicUtils.DisableSmartClient > 0 || BasicUtils.RegistryDebugLevel >= 10)
        this.WriteToEventLog("Trying to resolve assembly " + name, BasicUtils.DisableSmartClient > 0 ? EventLogEntryType.Error : EventLogEntryType.Information);
      if (this.SkipList != null)
      {
        foreach (string skip in this.SkipList)
        {
          if (name.Trim().ToLower() == (skip ?? "").Trim().ToLower())
            return (Assembly) null;
        }
      }
      try
      {
        if (this.firstMissingAssembly == null)
        {
          this.firstMissingAssembly = name;
          if (BasicUtils.RegistryDebugLevel >= 100)
            this.WriteToEventLog("First missing assembly: " + name, EventLogEntryType.Information);
        }
        if (!this.manifestsDownloaded)
        {
          if (BasicUtils.DisableSmartClient >= 0 && this.loadSDKAsmFromEncInstallDir)
          {
            if (BasicUtils.RegistryDebugLevel >= 100)
              this.WriteToEventLog("Try loading SDK assembly from Encompass InstallDir", EventLogEntryType.Information);
            return this.loadAssemblyFromEncompassInstallDir(name);
          }
          if (BasicUtils.DisableSmartClient > 0)
          {
            if (BasicUtils.RegistryDebugLevel >= 100)
              this.WriteToEventLog("Smart Client Disabled", EventLogEntryType.Information);
            return (Assembly) null;
          }
          if (BasicUtils.DisableSmartClient == 0)
          {
            List<string> stringList = new List<string>();
            if (this.firstSmartClientAssembly != null)
              stringList.Add(this.firstSmartClientAssembly.Trim().ToLower());
            if (this.defaultSmartClientAssemblies != null)
            {
              foreach (string smartClientAssembly in this.defaultSmartClientAssemblies)
                stringList.Add(smartClientAssembly.Trim().ToLower());
            }
            string lower = name.Trim().ToLower();
            bool flag = false;
            foreach (string str in stringList)
            {
              if (lower == str)
              {
                flag = true;
                break;
              }
            }
            if (!flag)
            {
              if (BasicUtils.RegistryDebugLevel >= 100)
                this.WriteToEventLog("Not recognized as Smart Client", EventLogEntryType.Information);
              return (Assembly) null;
            }
          }
          this.runOnce();
        }
      }
      catch (Exception ex)
      {
        int num = (int) AssemblyResolver.DisplayOrLogMessage(EventLogEntryType.Error, "An error occurred while initializing assembly resolver. Application will terminate.\r\nDetails: " + ex.Message + "\r\n" + ex.StackTrace, this.executableName);
        Process.GetCurrentProcess().Kill();
      }
      try
      {
        if (this.BeforeAssemblyResolve != null)
          this.BeforeAssemblyResolve((object) this, (EventArgs) null);
      }
      catch (Exception ex)
      {
        int num = (int) AssemblyResolver.DisplayOrLogMessage(EventLogEntryType.Error, "Error executing deployment module. Click OK to continue.\r\nDetails: " + ex.Message + "\r\n" + ex.StackTrace.ToString(), this.executableName);
      }
      string asmFilePath = (string) null;
      for (int index = 30; index >= 0; --index)
      {
        try
        {
          asmFilePath = this.deployHandler.GetAndCacheAssembly(name);
          if (asmFilePath == null)
          {
            if (BasicUtils.RegistryDebugLevel >= 1)
              this.WriteToEventLog("Getting null assembly path while trying to resolve assembly " + name, EventLogEntryType.Warning);
            return (Assembly) null;
          }
          break;
        }
        catch (Exception ex)
        {
          if (index == 0)
          {
            int num = (int) AssemblyResolver.DisplayOrLogMessage(EventLogEntryType.Error, "An error occurred while downloading assembly '" + name + "'. Application will terminate.\r\nDetails: " + ex.Message + "\r\n" + ex.StackTrace, this.executableName);
            Process.GetCurrentProcess().Kill();
          }
          Thread.Sleep(100);
        }
      }
      try
      {
        if (this.AfterAssemblyDownload != null)
          this.AfterAssemblyDownload((object) this, (EventArgs) null);
      }
      catch (Exception ex)
      {
        int num = (int) AssemblyResolver.DisplayOrLogMessage(EventLogEntryType.Error, "Error executing deployment module. Click OK to continue.\r\nDetails: " + ex.Message + "\r\n" + ex.StackTrace.ToString(), this.executableName);
      }
      Assembly assembly = this.loadAssemblyFile(asmFilePath);
      try
      {
        if (this.AfterAssemblyResolve != null)
          this.AfterAssemblyResolve((object) this, (EventArgs) null);
      }
      catch (Exception ex)
      {
        int num = (int) AssemblyResolver.DisplayOrLogMessage(EventLogEntryType.Error, "Error executing deployment module. Click OK to continue.\r\nDetails: " + ex.Message + "\r\n" + ex.StackTrace.ToString(), this.executableName);
      }
      return assembly;
    }

    private bool loadSDKAsmFromEncInstallDir
    {
      get
      {
        if (this.disableSDKAsmResolve == 0)
        {
          if ((this.firstMissingAssembly ?? "").ToLower() != "EncompassObjects".ToLower())
          {
            if (BasicUtils.RegistryDebugLevel >= 100)
              this.WriteToEventLog("First missing assembly is not EncompassObjects; disable SDK assembly resolve.", EventLogEntryType.Information);
            this.disableSDKAsmResolve = 1;
          }
          else
          {
            using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Ellie Mae\\Encompass"))
            {
              if (registryKey == null)
              {
                if (BasicUtils.RegistryDebugLevel >= 100)
                  this.WriteToEventLog("Encompass registry key not found; disable SDK assembly resolve.", EventLogEntryType.Information);
                this.disableSDKAsmResolve = 1;
              }
              else
              {
                string str = (string) registryKey.GetValue("InstallDir");
                this.disableSDKAsmResolve = ((string) registryKey.GetValue("DisableSDKAsmResolve") ?? "").Trim() == "1" || str == null ? 1 : -1;
              }
            }
          }
        }
        return this.disableSDKAsmResolve < 0;
      }
    }

    private Assembly loadAssemblyFile(string asmFilePath)
    {
      Assembly assembly = (Assembly) null;
      if (BasicUtils.RegistryDebugLevel >= 10)
        this.WriteToEventLog("Trying to load assembly from " + asmFilePath, EventLogEntryType.Information);
      for (int index = 50; index >= 0; --index)
      {
        try
        {
          assembly = Assembly.LoadFile(asmFilePath);
          break;
        }
        catch (Exception ex)
        {
          if (index == 0)
          {
            string message = "An error occurred while loading assembly file '" + asmFilePath + "'.\r\nDetails: " + ex.Message + "\r\n" + ex.StackTrace.ToString();
            this.WriteToEventLog(message, EventLogEntryType.Error);
            if (BasicUtils.RegistryDebugLevel >= 5)
            {
              int num = (int) AssemblyResolver.DisplayOrLogMessage(EventLogEntryType.Error, message, this.executableName);
            }
            return (Assembly) null;
          }
          Thread.Sleep(100);
        }
      }
      return assembly;
    }

    private void checkAsmResolverAndRuntimeVersion(string encInstallDir)
    {
      if (this.asmResolverAndRuntimeVersionChecked)
        return;
      this.asmResolverAndRuntimeVersionChecked = true;
      string str1 = Path.Combine(Application.StartupPath, ResolverConsts.AsmResolverDll);
      string str2 = Path.Combine(encInstallDir, ResolverConsts.AsmResolverDll);
      if (System.IO.File.Exists(str1) && System.IO.File.Exists(str2))
      {
        FileVersionInfo versionInfo1 = FileVersionInfo.GetVersionInfo(str1);
        FileVersionInfo versionInfo2 = FileVersionInfo.GetVersionInfo(str2);
        if (versionInfo1.FileVersion != versionInfo2.FileVersion)
          this.WriteToEventLog("The file version of AsmResolver.dll under Encompass install directory is different: " + versionInfo2.FileVersion, EventLogEntryType.Warning);
      }
      string path2 = "EllieMae.Encompass.Runtime.dll";
      string str3 = Path.Combine(Application.StartupPath, path2);
      string str4 = Path.Combine(encInstallDir, path2);
      if (!System.IO.File.Exists(str3) || !System.IO.File.Exists(str4))
        return;
      FileVersionInfo versionInfo3 = FileVersionInfo.GetVersionInfo(str3);
      FileVersionInfo versionInfo4 = FileVersionInfo.GetVersionInfo(str4);
      if (!(versionInfo3.FileVersion != versionInfo4.FileVersion))
        return;
      this.WriteToEventLog("The file version of Runtime.dll under Encompass install directory is different: " + versionInfo4.FileVersion, EventLogEntryType.Warning);
    }

    private Assembly loadAssemblyFromEncompassInstallDir(string asmName)
    {
      string str1 = (string) null;
      using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Ellie Mae\\Encompass"))
      {
        if (registryKey != null)
          str1 = (string) registryKey.GetValue("InstallDir");
      }
      if (str1 == null)
        return (Assembly) null;
      try
      {
        this.checkAsmResolverAndRuntimeVersion(str1);
      }
      catch (Exception ex)
      {
        this.WriteToEventLog("Error checking AsmResolver and Runtime version: " + ex.Message, EventLogEntryType.Warning);
      }
      string str2 = Path.Combine(str1, asmName + ".dll");
      if (!System.IO.File.Exists(str2))
      {
        str2 = Path.Combine(str1, asmName + ".exe");
        if (!System.IO.File.Exists(str2))
          return (Assembly) null;
      }
      return this.loadAssemblyFile(str2);
    }

    private string getCmdLineArgInstallationURL()
    {
      return AssemblyResolver.getCmdLineArg("-InstallationURL");
    }

    private static string getCmdLineArg(string argName)
    {
      return AssemblyResolver.getArg(argName, Environment.GetCommandLineArgs(), 1);
    }

    private static string getArg(string argName, string[] args, int startIdx = 0)
    {
      int argIndex = AssemblyResolver.getArgIndex(argName, false, args, startIdx);
      return argIndex >= startIdx && args.Length > argIndex ? args[argIndex + 1] : (string) null;
    }

    private static int getCmdLineArgIndex(string arg, bool ignoreCase)
    {
      return AssemblyResolver.getArgIndex(arg, ignoreCase, Environment.GetCommandLineArgs(), 1);
    }

    private static int getArgIndex(string arg, bool ignoreCase, string[] args, int startIdx = 0)
    {
      for (int argIndex = startIdx; argIndex < args.Length; ++argIndex)
      {
        if (string.Compare(args[argIndex], arg, ignoreCase) == 0)
          return argIndex;
      }
      return -1;
    }

    private bool skipAuthentication(string appCompanyName)
    {
      using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\" + appCompanyName + "\\SmartClient"))
        return registryKey != null && (string) registryKey.GetValue("SkipAuthentication") == "1" || AssemblyResolver.getCmdLineArgIndex("-SkipAuthentication", false) > 0;
    }

    private void updateFilePermissions(bool useAuthenticatedUsers, AppSuiteInfo appSuiteInfo)
    {
      try
      {
        if (appSuiteInfo == null || string.IsNullOrWhiteSpace(appSuiteInfo.CompanyName))
          return;
        string uacFolder = DeployUtils.GetUacFolder(appSuiteInfo.CompanyName);
        if (string.IsNullOrWhiteSpace(uacFolder))
          return;
        string fullName = Directory.GetParent(uacFolder).FullName;
        if (!Directory.Exists(fullName))
          return;
        if (useAuthenticatedUsers)
        {
          if (!FilePermission.ExistsSecurityGroup(fullName, WellKnownSidType.AuthenticatedUserSid, false))
            FilePermission.AddSecurityGroup(fullName, WellKnownSidType.AuthenticatedUserSid);
          if (!FilePermission.ExistsSecurityGroup(fullName, WellKnownSidType.WorldSid, true))
            return;
          FilePermission.RemoveSecurityGroup(fullName, WellKnownSidType.WorldSid);
        }
        else
        {
          if (FilePermission.ExistsSecurityGroup(fullName, WellKnownSidType.WorldSid, false))
            return;
          FilePermission.AddSecurityGroup(fullName, WellKnownSidType.WorldSid);
        }
      }
      catch (Exception ex)
      {
        this.WriteToEventLog("Unable to update permissions with Error" + ex.Message, EventLogEntryType.Error);
      }
    }

    private void runOnce()
    {
      if (this.manifestsDownloaded)
        return;
      AppSuiteInfo appSuiteInfo;
      if (this.appSuiteCompanyName != null && this.appSuiteName != null)
      {
        appSuiteInfo = new AppSuiteInfo(this.appSuiteCompanyName, this.appSuiteName);
      }
      else
      {
        appSuiteInfo = new AppSuiteInfo(Assembly.GetEntryAssembly());
        this.appSuiteCompanyName = appSuiteInfo.CompanyName;
        this.appSuiteName = appSuiteInfo.AppSuiteName;
      }
      HashUtil.ComputeHashB64FilePath(HashAlgorithm.SHA1, this.startupPath);
      this.uacHashFolder = DeployUtils.ConstructUacHashFolder(appSuiteInfo.CompanyName, this.startupPath);
      if (BasicUtils.RegistryDebugLevel >= 10)
        this.WriteToEventLog("UAC hash folder: " + this.uacHashFolder, EventLogEntryType.Information);
      this.appDataHashFolder = DeployUtils.ConstructAppDataHashFolder(appSuiteInfo.CompanyName, this.startupPath);
      if (BasicUtils.RegistryDebugLevel >= 10)
        this.WriteToEventLog("App data hash folder: " + this.appDataHashFolder, EventLogEntryType.Information);
      this.deployHandler = new DeploymentHandler(this.uacHashFolder, appSuiteInfo.AppSuiteName, this.executableName);
      bool skipAuth = false;
      bool continueInOfflineSelected = false;
      string str1 = this.getCmdLineArgInstallationURL();
      if (this.skipAuthentication(this.appSuiteCompanyName))
        skipAuth = true;
      else if (str1 != null)
      {
        string str2 = (string) null;
        using (RegistryKey registryKey = BasicUtils.GetRegistryHive((string) null).OpenSubKey("Software\\Ellie Mae\\SmartClient\\" + Application.StartupPath.Replace("\\", "/") + "\\Attributes"))
        {
          if (registryKey != null)
            str2 = (string) registryKey.GetValue("HTTPS");
        }
        switch (str2)
        {
          case "1":
            str1 = str1.Replace("http://", "https://");
            break;
          case "0":
            str1 = str1.Replace("https://", "http://");
            break;
        }
        this.deployHandler.SetInstallationURL(str1, false);
      }
      else
      {
        string scid = AssemblyResolver.getArg("-SCID", AssemblyResolver.instance.commandLineArgs);
        this.authResult = AuthenticationForm.Authenticate(appSuiteInfo.CompanyName, appSuiteInfo.AppSuiteName, this.executableName + this.executableExt, this.deployHandler.OfflineReady, out continueInOfflineSelected, this.uacHashFolder, scid);
        if (this.authResult != null)
        {
          if (this.authResult.ResultCode == AuthResultCode.Success || this.authResult.ResultCode == AuthResultCode.NullInstallationURL || this.authResult.ResultCode == AuthResultCode.OfflineMode)
          {
            this.deployHandler.InstallationURL = this.authResult.InstallationURL;
            BasicUtils.DisplayDebuggingInfo("InstallationURL", "InstallationURL: " + this.deployHandler.InstallationURL);
          }
          this.updateFilePermissions(this.authResult.Attributes == null || !(this.authResult.Attributes["UseAuthenticatedUsers"] == "0"), appSuiteInfo);
        }
      }
      this.downloadManifests(skipAuth, str1 != null, continueInOfflineSelected);
      this.manifestsDownloaded = true;
      try
      {
        this.checkAndUpdateExeAndResolver();
        this.loadDeploymentModules();
        if (this.OnResolverInitDone != null)
          this.OnResolverInitDone((object) this, (EventArgs) null);
        if (!this.deployHandler.AppManifest.DoBackgroundDownload)
          return;
        this.deployHandler.StartBackgroundDownload();
      }
      catch (Exception ex)
      {
        int num = (int) AssemblyResolver.DisplayOrLogMessage(EventLogEntryType.Error, "An error occurred while initializing resolver. Application will terminate.\r\nDetails: " + ex.Message + "\r\n" + ex.StackTrace, this.executableName);
        Process.GetCurrentProcess().Kill();
      }
    }

    private void downloadManifests(
      bool skipAuth,
      bool nonNullCmdLineInstallURL,
      bool continueInOfflineSelected)
    {
      if (skipAuth)
      {
        if (this.deployHandler.InstallationURL != null)
        {
          try
          {
            this.deployHandler.GetAndCacheManifestsFromSource();
            return;
          }
          catch
          {
          }
        }
      }
      else if (this.deployHandler.InstallationURL == null)
      {
        if (this.deployHandler.OfflineReady)
          return;
        switch (AssemblyResolver.DisplayOrLogMessage(EventLogEntryType.Error, "Error getting installation URL. Do you want to enter the installation URL?", this.executableName, MessageBoxButtons.YesNo, MessageBoxIcon.Question))
        {
          case DialogResult.None:
          case DialogResult.No:
            Process.GetCurrentProcess().Kill();
            break;
        }
      }
      else
      {
        try
        {
          this.deployHandler.GetAndCacheManifestsFromSource();
          return;
        }
        catch (Exception ex)
        {
          if (this.deployHandler.OfflineReady)
          {
            if (this.executableName == ResolverConsts.AppLauncher | continueInOfflineSelected)
              return;
            if (MessageBox.Show("Error encountered while downloading manifests. Since the application has been downloaded completely, do you want to continue using the application?", this.executableName, MessageBoxButtons.YesNo) == DialogResult.Yes)
              return;
          }
          else
          {
            if (!nonNullCmdLineInstallURL)
            {
              if (!nonNullCmdLineInstallURL)
              {
                if (continueInOfflineSelected)
                  goto label_19;
              }
              else
                goto label_19;
            }
            this.deployHandler.InstallationURL = (string) null;
            string message = "Unhandled exception thrown while downloading manifests:\r\n" + ex.Message + "\r\n\r\n" + ex.StackTrace;
            if (ex.InnerException != null)
              message = message + "\r\n" + ex.InnerException.Message;
            int num = (int) AssemblyResolver.DisplayOrLogMessage(EventLogEntryType.Error, message, this.executableName);
            Process.GetCurrentProcess().Kill();
          }
        }
      }
label_19:
      bool flag = false;
      while (!flag)
      {
        InstallationUrlDialog installationUrlDialog = new InstallationUrlDialog("");
        if (installationUrlDialog.ShowDialog() == DialogResult.Cancel)
          Process.GetCurrentProcess().Kill();
        try
        {
          this.deployHandler.GetAndCacheManifestsFromSource(installationUrlDialog.InstallationURL);
          this.deployHandler.InstallationURL = installationUrlDialog.InstallationURL;
          flag = true;
        }
        catch (Exception ex)
        {
          switch (ex)
          {
            case WebException _:
            case ArgumentException _:
            case DirectoryNotFoundException _:
              if (this.deployHandler.OfflineReady)
              {
                if (MessageBox.Show("Error encountered while connecting to server. Please check the installation URL. If the URL is correct, since the application has been downloaded completely, do you want to continue using the application?", this.executableName, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                  this.deployHandler.InstallationURL = installationUrlDialog.InstallationURL;
                  return;
                }
                continue;
              }
              if (MessageBox.Show("Error encountered while connecting to server. Do you want to re-enter the URL?", this.executableName, MessageBoxButtons.YesNo) == DialogResult.No)
              {
                Process.GetCurrentProcess().Kill();
                continue;
              }
              continue;
            default:
              int num = (int) MessageBox.Show("Unknown error encountered while connecting to server: " + ex.Message + ".\r\nApplication will be terminated.\r\n" + ex.StackTrace.ToString(), this.executableName);
              Process.GetCurrentProcess().Kill();
              continue;
          }
        }
      }
    }

    private void checkAndUpdateAppUpdater()
    {
      DeploymentHandler deploymentHandler = new DeploymentHandler(this.uacHashFolder, ResolverConsts.AppUpdater, ResolverConsts.AppUpdater);
      deploymentHandler.GetAndCacheManifestsFromSource();
      AsmFileInfo asmFileInfo1 = deploymentHandler.GetAsmFileInfo(ResolverConsts.AppUpdater);
      AsmFileInfo asmFileInfo2 = new AsmFileInfo(this.startupPath, ResolverConsts.AppUpdaterExe);
      if (!asmFileInfo1.IsSameVersion(asmFileInfo2))
      {
        bool flag = false;
        byte[] assemblyBytes = deploymentHandler.GetAssemblyBytes(asmFileInfo1.Name);
        if (this.waitForApplicationExit(ResolverConsts.AppUpdater, 1000))
        {
          try
          {
            System.IO.File.WriteAllBytes(Path.Combine(this.startupPath, ResolverConsts.AppUpdaterExe), assemblyBytes);
          }
          catch (Exception ex)
          {
            flag = !SystemUtils.RunRestoreAppLauncher(ResolverConsts.AppUpdaterExe);
            if (flag)
            {
              int num = (int) MessageBox.Show("Unable to update " + ResolverConsts.AppUpdaterExe + ": " + ex.Message, this.executableName);
              Process.GetCurrentProcess().Kill();
            }
          }
        }
        else
          flag = true;
        if (flag)
        {
          int num = (int) MessageBox.Show("Unable to update " + ResolverConsts.AppUpdater + "; please try again later.", this.executableName);
          Process.GetCurrentProcess().Kill();
        }
      }
      ResFileInfo resFileInfo1 = (ResFileInfo) null;
      ResFileInfo resFileInfo2 = (ResFileInfo) null;
      try
      {
        resFileInfo1 = deploymentHandler.GetResFileInfo(ResolverConsts.AppUpdaterExe + ".config");
      }
      catch (Exception ex)
      {
        this.WriteToEventLog("Unable to get information of resource file " + ResolverConsts.AppUpdaterExe + ".config from manifest: " + ex.Message, EventLogEntryType.Error);
      }
      try
      {
        if (System.IO.File.Exists(Path.Combine(this.startupPath, ResolverConsts.AppUpdaterExe + ".config")))
          resFileInfo2 = new ResFileInfo(this.startupPath, ResolverConsts.AppUpdaterExe + ".config", false);
      }
      catch (Exception ex)
      {
        this.WriteToEventLog("Unable to get information of resource file " + ResolverConsts.AppUpdaterExe + ".config from " + this.startupPath + ": " + ex.Message, EventLogEntryType.Error);
      }
      if (resFileInfo1 == null || resFileInfo2 != null && resFileInfo1.IsSameVersion(resFileInfo2, HashAlgorithm.SHA1))
        return;
      byte[] resourceFileBytes = deploymentHandler.GetResourceFileBytes(ResolverConsts.AppUpdaterExe + ".config");
      if (this.waitForApplicationExit(ResolverConsts.AppUpdater, 1000))
      {
        System.IO.File.WriteAllBytes(Path.Combine(this.startupPath, ResolverConsts.AppUpdaterExe + ".config"), resourceFileBytes);
      }
      else
      {
        int num = (int) MessageBox.Show("Unable to update " + ResolverConsts.AppUpdaterExe + ".config; please try again", this.executableName);
        Process.GetCurrentProcess().Kill();
      }
    }

    private void deleteDotNet40Files()
    {
      bool flag1 = false;
      string fileName = Path.GetFileName(Application.ExecutablePath);
      string[] strArray1 = new string[6]
      {
        "AppLauncher.exe",
        "Encompass.exe",
        "AdminTools.exe",
        "ClickLoanProxy.exe",
        "FormBuilder.exe",
        "CRMTool.exe"
      };
      foreach (string strA in strArray1)
      {
        if (string.Compare(strA, fileName, true) == 0)
        {
          flag1 = true;
          break;
        }
      }
      if (!flag1 || !(AssemblyResolver.Instance.deployHandler.GetAsmFileInfo(Path.GetFileNameWithoutExtension(ResolverConsts.AsmResolverDll)).FileVersion < new Version(4, 0, 0, 0)))
        return;
      string[] strArray2 = new string[4]
      {
        "AppLauncher.exe",
        "AppLauncher.exe.config",
        "EllieMae.Encompass.AsmResolver.dll",
        "AppUpdtr.exe"
      };
      string[] collection1 = new string[17]
      {
        "RemoveUAC.exe",
        "EMClickLoan.dll",
        "EllieMae.Encompass.Runtime.dll",
        "AdminTools.exe",
        "AdminTools.exe.config",
        "ClickLoanProxy.exe",
        "ClickLoanProxy.exe.config",
        "CRMTool.exe",
        "CRMTool.exe.config",
        "Encompass.exe",
        "Encompass.exe.config",
        "FormBuilder.exe",
        "FormBuilder.exe.config",
        "SDKConfig.exe",
        "SDKConfig.exe.config",
        "SettingsTool.exe",
        "SettingsTool.exe.config"
      };
      string[] collection2 = new string[0];
      if (Directory.Exists(Path.Combine(this.startupPath, "SDK")))
        collection2 = Directory.GetFiles(Path.Combine(this.startupPath, "SDK"), "*.*", SearchOption.TopDirectoryOnly);
      List<string> stringList = new List<string>();
      stringList.AddRange((IEnumerable<string>) collection1);
      stringList.AddRange((IEnumerable<string>) collection2);
      foreach (string path in stringList)
      {
        bool flag2 = true;
        foreach (string str in strArray2)
        {
          if (path.ToLower().EndsWith(str.ToLower()))
          {
            flag2 = false;
            break;
          }
        }
        if (flag2)
        {
          try
          {
            System.IO.File.Delete(path);
          }
          catch (Exception ex)
          {
            this.WriteToEventLog("Unable to delete .NET 4.0 file " + path + ": " + ex.Message, EventLogEntryType.Warning);
          }
        }
      }
    }

    private void deleteConfigFiles()
    {
      if (!(AssemblyResolver.Instance.deployHandler.GetAsmFileInfo(Path.GetFileNameWithoutExtension(ResolverConsts.AsmResolverDll)).FileVersion < new Version(4, 0, 2, 2)))
        return;
      foreach (string path2 in AssemblyResolver.configFilesToDelete)
      {
        try
        {
          System.IO.File.Delete(Path.Combine(this.startupPath, path2));
        }
        catch (Exception ex1)
        {
          this.WriteToEventLog("Unable to delete config file " + Path.Combine(this.startupPath, path2) + ": " + ex1.Message, EventLogEntryType.Warning);
          try
          {
            System.IO.File.Delete(Path.Combine(Application.StartupPath, path2));
          }
          catch (Exception ex2)
          {
            this.WriteToEventLog("Unable to delete config file " + Path.Combine(Application.StartupPath, path2) + ": " + ex2.Message, EventLogEntryType.Warning);
          }
        }
      }
    }

    private void checkAndUpdateExeAndResolver()
    {
      bool flag1 = false;
      if (this.executableName.ToLower() == Path.GetFileNameWithoutExtension(Application.ExecutablePath).ToLower())
        flag1 = !AssemblyResolver.IsAsmFileUpToDate(this.startupPath, this.executableName + ".exe");
      bool flag2 = !AssemblyResolver.IsAsmFileUpToDate(this.startupPath, ResolverConsts.AsmResolverDll);
      int num1 = 1;
      if (flag1)
      {
        ++num1;
        this.deployHandler.GetAssemblyBytes(this.executableName);
      }
      if (flag2)
      {
        this.deleteConfigFiles();
        this.deleteDotNet40Files();
        ++num1;
        this.deployHandler.GetAssemblyBytes(ResolverConsts.AsmResolver);
      }
      if (num1 <= 1)
        return;
      string[] strArray1 = new string[1 + this.commandLineArgs.Length + num1];
      strArray1[0] = Application.ExecutablePath;
      Array.Copy((Array) this.commandLineArgs, 0, (Array) strArray1, 1, this.commandLineArgs.Length);
      strArray1[1 + this.commandLineArgs.Length] = "-";
      int num2 = 2 + this.commandLineArgs.Length;
      if (flag1)
        strArray1[num2++] = Path.Combine(this.deployHandler.AppFolderUrl, this.executableName + this.executableExt);
      if (flag2)
      {
        string[] strArray2 = strArray1;
        int index = num2;
        int num3 = index + 1;
        string str = Path.Combine(this.deployHandler.AppFolderUrl, ResolverConsts.AsmResolverDll);
        strArray2[index] = str;
      }
      this.checkAndUpdateAppUpdater();
      SystemUtils.ExecSystemCmd(Path.Combine(this.startupPath, ResolverConsts.AppUpdaterExe), strArray1);
      Process.GetCurrentProcess().Kill();
    }

    private void loadDeploymentModules()
    {
      string resourceFileText = this.deployHandler.GetResourceFileText(ResolverConsts.DeployModuleDir + this.deployHandler.UrlSeparator + this.executableName + ResolverConsts.DeployModuleConfigFileExt);
      if (resourceFileText == null)
        return;
      XmlDocument xmldoc = new XmlDocument();
      xmldoc.LoadXml(resourceFileText);
      XDConfiguration xdConfiguration = new XDConfiguration(xmldoc);
      if (xdConfiguration.DeploymentModules == null)
        return;
      List<XEdeploymentModule> moduleList = xdConfiguration.DeploymentModules.ModuleList;
      if (moduleList == null || moduleList.Count == 0)
        return;
      for (int index = 0; index < moduleList.Count; ++index)
      {
        string typeName = moduleList[index].TypeName;
        string str = moduleList[index].AsmNameWOExt + ".dll";
        string path = AssemblyResolver.DownloadAndCacheResourceFile(ResolverConsts.DeployModuleDir + this.deployHandler.UrlSeparator + str, "");
        byte[] rawAssembly = System.IO.File.ReadAllBytes(path);
        Assembly assembly = !AssemblyResolver.isIexplore ? Assembly.Load(rawAssembly, (byte[]) null, Assembly.GetExecutingAssembly().Evidence) : Assembly.Load(rawAssembly, (byte[]) null);
        ((!(assembly == (Assembly) null) ? (IDeploymentModule) assembly.CreateInstance(typeName) : throw new Exception("Error loading deployment module " + path)) ?? throw new Exception("Error creating type " + typeName + " from deployment module " + path)).Init(this);
      }
    }

    private bool waitForApplicationExit(string appNameWithoutExt, int waitTimeInMsec)
    {
      try
      {
        foreach (Process process in Process.GetProcessesByName(appNameWithoutExt))
        {
          if (!process.WaitForExit(waitTimeInMsec))
          {
            while (!process.HasExited)
            {
              if (MessageBox.Show("Please close all " + appNameWithoutExt + " applications in order to allow the system to update itself.", this.executableName, MessageBoxButtons.RetryCancel, MessageBoxIcon.Exclamation) == DialogResult.Cancel)
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

    public void WriteToEventLog(string message, EventLogEntryType type)
    {
      try
      {
        this.eventLog.WriteEntry(message, type);
      }
      catch (Exception ex)
      {
        if (BasicUtils.RegistryDebugLevel < 101)
          return;
        int num = (int) MessageBox.Show("Error writing to event log.\r\n" + ex.Message + "\r\n" + ex.StackTrace.ToString(), this.executableName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
      }
    }

    private bool validatePath(string filePath)
    {
      if (!filePath.Contains(".."))
        return true;
      int num = (int) AssemblyResolver.DisplayOrLogMessage(EventLogEntryType.Error, filePath + ": file path cannot contain \"..\"", this.executableName);
      return false;
    }

    public static string FirstSmartClientAssembly
    {
      set
      {
        if (AssemblyResolver.Instance == null)
          return;
        AssemblyResolver.Instance.firstSmartClientAssembly = value;
      }
    }

    public static string[] SkipListS
    {
      get
      {
        return AssemblyResolver.Instance == null ? (string[]) null : AssemblyResolver.Instance.SkipList;
      }
      set
      {
        if (AssemblyResolver.Instance == null)
          return;
        AssemblyResolver.Instance.SkipList = value;
      }
    }

    public static void WriteToEventLogS(string message, EventLogEntryType type)
    {
      if (AssemblyResolver.Instance == null)
        return;
      AssemblyResolver.Instance.WriteToEventLog(message, type);
    }

    public static DialogResult DisplayOrLogMessage(
      EventLogEntryType type,
      string message,
      string caption = "SmartClient",
      MessageBoxButtons msgBoxBtns = MessageBoxButtons.OK,
      MessageBoxIcon msgBoxIcon = MessageBoxIcon.Hand,
      MessageBoxDefaultButton msgBoxDefaultBtn = MessageBoxDefaultButton.Button1)
    {
      DialogResult dialogResult = DialogResult.None;
      if (AssemblyResolver.Instance == null)
        return dialogResult;
      try
      {
        if (AssemblyResolver.SCPublisherQuietMode)
          AssemblyResolver.WriteToEventLogS(message, type);
        else
          dialogResult = MessageBox.Show(message, caption, msgBoxBtns, msgBoxIcon, msgBoxDefaultBtn);
      }
      catch (Exception ex)
      {
        if (BasicUtils.RegistryDebugLevel >= 101)
        {
          int num = (int) MessageBox.Show("Error writing to event log.\r\n" + ex.Message + "\r\n" + ex.StackTrace.ToString(), AssemblyResolver.Instance.executableName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
        }
      }
      return dialogResult;
    }

    public static AuthenticationResult AuthResult => AssemblyResolver.Instance.authResult;

    public static string GetResourceFileFullPath(string resFileRelPath)
    {
      return AssemblyResolver.GetResourceFileFullPath(resFileRelPath, (string) null, false);
    }

    public static string GetResourceFileFullPath(string resFileRelPath, string installRoot)
    {
      return AssemblyResolver.GetResourceFileFullPath(resFileRelPath, installRoot, false);
    }

    public static string GetResourceFileFullPath(string resFileRelPath, bool throwExceptionOnError)
    {
      return AssemblyResolver.GetResourceFileFullPath(resFileRelPath, (string) null, throwExceptionOnError);
    }

    public static string GetResourceFileFullPath(
      string resFileRelPath,
      string installRoot,
      bool throwExceptionOnError)
    {
      return AssemblyResolver.instance.GetResourceFileAndPath(resFileRelPath, installRoot, throwExceptionOnError);
    }

    public static string GetResourceFileFolderPath(string resFileFolder)
    {
      return AssemblyResolver.GetResourceFileFolderPath(resFileFolder, (string) null);
    }

    public static string GetResourceFileFolderPath(string resFileFolder, string installRoot)
    {
      if (installRoot == null)
        installRoot = AssemblyResolver.AppStartupPath;
      if (Path.IsPathRooted(resFileFolder))
        throw new IOException(resFileFolder + ": Resource file folder path should not be rooted");
      return AssemblyResolver.IsSmartClient ? Path.Combine(AssemblyResolver.Instance.deployHandler.AppFolderUrl, resFileFolder) : Path.Combine(installRoot, resFileFolder);
    }

    public static string UacHashFolder
    {
      get
      {
        return AssemblyResolver.Instance == null ? (string) null : AssemblyResolver.Instance.uacHashFolder;
      }
    }

    private static string _appDataHashFolder
    {
      get
      {
        return AssemblyResolver.Instance == null ? (string) null : AssemblyResolver.Instance.appDataHashFolder;
      }
    }

    public static string AppDataHashFolder
    {
      get
      {
        string path1 = (string) null;
        using (RegistryKey registryKey1 = Registry.CurrentUser.OpenSubKey("Software\\Ellie Mae\\SmartClient"))
        {
          if (registryKey1 != null)
            path1 = (string) registryKey1.GetValue("DataDirBase");
          if (path1 == null)
          {
            using (RegistryKey registryKey2 = Registry.LocalMachine.OpenSubKey("Software\\Ellie Mae\\SmartClient"))
            {
              if (registryKey2 != null)
                path1 = (string) registryKey2.GetValue("DataDirBase");
            }
          }
        }
        return path1 != null && AssemblyResolver.AppStartupPath != null ? Path.Combine(path1, HashUtil.ComputeHashB64FilePath(HashAlgorithm.SHA1, AssemblyResolver.AppStartupPath)) : AssemblyResolver._appDataHashFolder;
      }
    }

    public static string DataDirHashFolder => AssemblyResolver.AppDataHashFolder;

    public static bool IsAsmFileUpToDate(string localPath, string asmFileName)
    {
      AsmFileInfo asmFileInfo1 = new AsmFileInfo(localPath, asmFileName);
      AsmFileInfo asmFileInfo2 = AssemblyResolver.Instance.deployHandler.GetAsmFileInfo(Path.GetFileNameWithoutExtension(asmFileName));
      return asmFileInfo1.IsSameVersion(asmFileInfo2) && !asmFileInfo1.isCertExpired(localPath);
    }

    public static bool IsAsmFileUpToDate(string asmFileFullPath)
    {
      return AssemblyResolver.IsAsmFileUpToDate(Path.GetDirectoryName(asmFileFullPath), Path.GetFileName(asmFileFullPath));
    }

    public static bool IsResFileUpToDate(string basePath, string resFilePath)
    {
      return new ResFileInfo(basePath, resFilePath, HashAlgorithm.SHA1).IsSameVersion(AssemblyResolver.Instance.deployHandler.GetResFileInfo(resFilePath), HashAlgorithm.SHA1);
    }

    public static string DownloadAndCacheResourceFile(string filePath)
    {
      return AssemblyResolver.DownloadAndCacheResourceFile2(filePath).Key;
    }

    public static string DownloadAndCacheResourceFile(string filePath, string cachePath)
    {
      return AssemblyResolver.downloadAndCacheResourceFile(filePath, cachePath, false).Key;
    }

    public static KeyValuePair<string, bool> DownloadAndCacheResourceFile2(string filePath)
    {
      return AssemblyResolver.downloadAndCacheResourceFile(filePath, (string) null, false);
    }

    public static KeyValuePair<string, bool> DownloadAndCacheResourceFile2(
      string filePath,
      string cachePath)
    {
      return AssemblyResolver.downloadAndCacheResourceFile(filePath, cachePath, false);
    }

    private static KeyValuePair<string, bool> downloadAndCacheResourceFile(
      string filePath,
      bool throwExceptionOnError)
    {
      return AssemblyResolver.downloadAndCacheResourceFile(filePath, (string) null, throwExceptionOnError);
    }

    private static KeyValuePair<string, bool> downloadAndCacheResourceFile(
      string filePath,
      string cachePath,
      bool throwExceptionOnError)
    {
      if (!AssemblyResolver.IsSmartClient)
        return new KeyValuePair<string, bool>((string) null, false);
      KeyValuePair<string, bool> keyValuePair = new KeyValuePair<string, bool>((string) null, false);
      for (int index = 30; index >= 0; --index)
      {
        try
        {
          keyValuePair = AssemblyResolver.instance.deployHandler.GetAndCacheResourceFileFromSource(filePath, cachePath);
          break;
        }
        catch (Exception ex)
        {
          if (index == 0)
          {
            string message = "An error occurred while downloading resource file '" + filePath + "'.\r\nDetails: " + ex.Message + "\r\n" + ex.StackTrace.ToString();
            if (throwExceptionOnError)
              throw new Exception(message);
            int num = (int) AssemblyResolver.DisplayOrLogMessage(EventLogEntryType.Error, message, AssemblyResolver.instance.executableName);
          }
          else
            Thread.Sleep(100);
        }
      }
      return keyValuePair;
    }

    public static void DisplayMOTDMessage()
    {
      if (AuthenticationForm.LastAuthentication == null)
        return;
      try
      {
        HTMLDocumentClass htmlDocumentClass = new HTMLDocumentClass();
        MsgDisplayMgr.DisplayMessage(AssemblyResolver.AppStartupPath, AuthenticationForm.LastAuthentication.AuthServerURL, AuthenticationForm.LastAuthentication.Userid, AssemblyResolver.AuthResult.MotdSettings);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message, "SmartClient MOTD", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
      }
    }

    public string UacHashFolderI => AssemblyResolver.UacHashFolder;

    public string AppDataHashFolderI => AssemblyResolver.AppDataHashFolder;

    public string GetResourceFileAndPath(string resFileRelPath)
    {
      return this.GetResourceFileAndPath(resFileRelPath, (string) null, false);
    }

    public string GetResourceFileAndPath(string resFileRelPath, string installRoot)
    {
      return this.GetResourceFileAndPath(resFileRelPath, installRoot, false);
    }

    public string GetResourceFileAndPath(string resFileRelPath, bool throwExceptionOnError)
    {
      return this.GetResourceFileAndPath(resFileRelPath, (string) null, throwExceptionOnError);
    }

    public string GetResourceFileAndPath(
      string resFileRelPath,
      string installRoot,
      bool throwExceptionOnError)
    {
      if (installRoot == null)
        installRoot = AssemblyResolver.AppStartupPath;
      if (Path.IsPathRooted(resFileRelPath))
        throw new IOException(resFileRelPath + ": Resource file path should not be rooted");
      return AssemblyResolver.IsSmartClient ? AssemblyResolver.downloadAndCacheResourceFile(resFileRelPath, throwExceptionOnError).Key : Path.Combine(installRoot, resFileRelPath);
    }

    public void DownloadAndCacheResourceFileI(string filePath)
    {
      this.DownloadAndCacheResourceFileI(filePath, (string) null);
    }

    public void DownloadAndCacheResourceFileI(string filePath, string cachePath)
    {
      if (!AssemblyResolver.IsSmartClient)
        return;
      for (int index = 30; index >= 0; --index)
      {
        try
        {
          AssemblyResolver.instance.deployHandler.GetAndCacheResourceFileFromSource(filePath, cachePath);
          break;
        }
        catch (Exception ex)
        {
          if (index == 0)
          {
            int num = (int) AssemblyResolver.DisplayOrLogMessage(EventLogEntryType.Error, "An error occurred while downloading resource file '" + filePath + "'.\r\nDetails: " + ex.Message + "\r\n" + ex.StackTrace.ToString(), AssemblyResolver.instance.executableName);
          }
          else
            Thread.Sleep(100);
        }
      }
    }

    public void DownloadAndCacheResourceFiles(string searchPattern)
    {
      this.DownloadAndCacheResourceFiles(searchPattern, (string) null);
    }

    public void DownloadAndCacheResourceFiles(string searchPattern, string cachePath)
    {
      if (!AssemblyResolver.IsSmartClient)
        return;
      for (int index = 30; index >= 0; --index)
      {
        try
        {
          AssemblyResolver.instance.deployHandler.GetAndCacheResourceFilesFromSource(searchPattern, cachePath);
          break;
        }
        catch (Exception ex)
        {
          if (index == 0)
          {
            int num = (int) AssemblyResolver.DisplayOrLogMessage(EventLogEntryType.Error, "An error occurred while downloading resource files '" + searchPattern + "'.\r\nDetails: " + ex.Message + "\r\n" + ex.StackTrace.ToString(), AssemblyResolver.instance.executableName);
          }
          else
            Thread.Sleep(100);
        }
      }
    }

    public void DownloadAndCopyToAppStartupPath(string resFile, bool overwrite)
    {
      KeyValuePair<string, bool> keyValuePair = AssemblyResolver.DownloadAndCacheResourceFile2(resFile);
      string key = keyValuePair.Key;
      string str = Path.Combine(this.startupPath, resFile);
      if (!(!System.IO.File.Exists(str) | overwrite) && !keyValuePair.Value)
        return;
      if (!Directory.Exists(Path.GetDirectoryName(str)))
        Directory.CreateDirectory(Path.GetDirectoryName(str));
      System.IO.File.Copy(key, str, true);
    }

    public void DeleteCachedFile(string filePath)
    {
      if (!AssemblyResolver.IsSmartClient)
        return;
      if (!this.validatePath(filePath))
        return;
      try
      {
        string path = this.deployHandler.AppFolderUrl + this.deployHandler.UrlSeparator + filePath;
        if (!System.IO.File.Exists(path))
          return;
        System.IO.File.Delete(path);
      }
      catch (Exception ex)
      {
        int num = (int) AssemblyResolver.DisplayOrLogMessage(EventLogEntryType.Error, "An error occurred while deleting file " + filePath + ".\r\nDetails: " + ex.Message + "\r\n" + ex.StackTrace.ToString(), this.executableName);
      }
    }
  }
}
