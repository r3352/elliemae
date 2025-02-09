// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.MainUI.EncompassLoginRoutines
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using Elli.Server.Remoting;
using EllieMae.EMLite.Client;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.Common.Login;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.Version;
using EllieMae.EMLite.Diagnostics;
using EllieMae.EMLite.ePass;
using EllieMae.EMLite.ePass.PartnerAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.VersionInterface15;
using EllieMae.Encompass.AsmResolver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.MainUI
{
  public static class EncompassLoginRoutines
  {
    private static DateTime loginStartTime;
    private static string serverForCRMTool;
    private static string pwdForCRMTool;
    private static readonly string className = nameof (EncompassLoginRoutines);
    private static readonly string sw = Tracing.SwOutsideLoan;

    internal static bool LoginEncompassWithAccessToken(LoginContext context)
    {
      EncompassLoginRoutines.loginStartTime = DateTime.Now;
      PerformanceMeter performanceMeter = PerformanceMeter.StartNew("Encompass.Login", "Login process from login screen to UI display", true, 68, nameof (LoginEncompassWithAccessToken), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\LoginHelper.cs");
      try
      {
        Cursor.Current = Cursors.WaitCursor;
        if (!EncompassLoginRoutines.performLoginEncompass(context.UserId, context.Password, context.Server, context.AuthCode))
          return false;
        PerformanceMeter.Current.AddCheckpoint("Perform Login Logistic", 79, nameof (LoginEncompassWithAccessToken), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\LoginHelper.cs");
        if (AssemblyResolver.IsSmartClient)
        {
          Tracing.Log(true, "Info", EncompassLoginRoutines.className, "SCID = " + AssemblyResolver.SCClientID);
          if (SmartClientUtils.IsUpdateClientInfo2Required())
            SmartClientUtils.UpdateClientInfo(LoginUtil.getSmartClientSessionInfo(Session.DefaultInstance, AssemblyResolver.SCClientID, context.Server), AssemblyResolver.SCClientID, VersionInformation.CurrentVersion.Version.FullVersion, context.Server, Session.UserInfo.IsSuperAdministrator());
          if (AssemblyResolver.SCClientID.EndsWith(SmartClientUtils.SCTestCIDSuffix, StringComparison.CurrentCultureIgnoreCase))
          {
            List<UserInfoSummary> userInfoSummaryList = new List<UserInfoSummary>();
            VersionManagementGroup[] managementGroups = Session.ServerManager.GetVersionManagementGroups();
            if (managementGroups != null)
            {
              foreach (VersionManagementGroup versionManagementGroup in managementGroups)
              {
                if (!versionManagementGroup.IsDefaultGroup)
                  userInfoSummaryList.AddRange((IEnumerable<UserInfoSummary>) Session.ServerManager.GetVersionManagementGroupUsers(versionManagementGroup.GroupID));
              }
            }
            bool flag = true;
            if (userInfoSummaryList.Count > 0)
            {
              flag = false;
              foreach (UserInfoSummary userInfoSummary in userInfoSummaryList)
              {
                if (string.Compare(Session.UserID, userInfoSummary.UserID, true) == 0)
                {
                  flag = true;
                  break;
                }
              }
            }
            if (!flag)
              Application.Exit();
          }
        }
        return true;
      }
      catch (LoginException ex)
      {
        EncompassLoginRoutines.handleLoginException(ex);
      }
      catch (ServerDataException ex)
      {
      }
      catch (VersionMismatchException ex)
      {
        if (AssemblyResolver.IsSmartClient)
        {
          JedVersion jedVersion = ex.ClientVersion;
          string fullVersion1 = jedVersion.FullVersion;
          jedVersion = ex.ServerVersionControl.Version;
          string fullVersion2 = jedVersion.FullVersion;
          SmartClientUtils.HandleVersionMismatch(fullVersion1, fullVersion2);
        }
        if (EllieMae.EMLite.Common.VersionControl.QueryInstallVersionUpdate(ex.ServerVersionControl))
          Application.Exit();
      }
      catch (LicenseException ex)
      {
        if (ex.Cause == LicenseExceptionType.TrialExpired)
        {
          int num1 = (int) new TrialExpiredForm(ex.License).ShowDialog((IWin32Window) MainForm.Instance);
        }
        else
        {
          int num2 = (int) Utils.Dialog((IWin32Window) MainForm.Instance, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        Application.Exit();
      }
      catch (ServerConnectionException ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) MainForm.Instance, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      catch (FormatException ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) MainForm.Instance, "The Server name format you have entered is invalid.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      catch (Exception ex)
      {
        if (ex.InnerException != null && ex.InnerException is LoginException)
        {
          EncompassLoginRoutines.handleLoginException((LoginException) ex.InnerException);
          return false;
        }
        if (EnConfigurationSettings.GlobalSettings.Debug)
        {
          int num3 = (int) Utils.Dialog((IWin32Window) MainForm.Instance, "Unknown login error: " + (object) ex, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          int num4 = (int) Utils.Dialog((IWin32Window) MainForm.Instance, "Unknown login error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        return false;
      }
      finally
      {
        if (!Session.IsConnected)
          performanceMeter.Abort();
        else
          PerformanceMeter.Current.AddCheckpoint("Complete Login", 190, nameof (LoginEncompassWithAccessToken), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\LoginHelper.cs");
        Cursor.Current = Cursors.Default;
      }
      return true;
    }

    private static void handleLoginException(LoginException ex)
    {
      if (ex.LoginReturnCode == LoginReturnCode.USER_DISABLED)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) MainForm.Instance, "This user account has been disabled. Your System Administrator must enable your account in order for you to log in.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (ex.LoginReturnCode == LoginReturnCode.LOGIN_DISABLED)
      {
        int num2 = (int) Utils.Dialog((IWin32Window) MainForm.Instance, "All Encompass logins have been disabled by your System Administrator. Only the \"admin\" user can currently log into the system.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (ex.LoginReturnCode == LoginReturnCode.USER_LOCKED)
      {
        string str = string.Empty;
        if (!string.IsNullOrWhiteSpace(ex.Message))
        {
          int num3 = ex.Message.IndexOf("USER_LOCKED");
          if (num3 > 0 && ex.Message.Length >= num3 + 12)
            str = ex.Message.Substring(ex.Message.IndexOf("USER_LOCKED") + 12);
        }
        if (string.IsNullOrEmpty(str))
        {
          int num4 = (int) Utils.Dialog((IWin32Window) MainForm.Instance, "Your user account has been locked. Your System Administrator must unlock your account in order for you to log in.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          int num5 = (int) Utils.Dialog((IWin32Window) MainForm.Instance, "Your account has been locked due to multiple failed login attempts. Please wait " + str + " to try again.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
      }
      else if (ex.LoginReturnCode == LoginReturnCode.PERSONA_NOT_FOUND)
      {
        int num6 = (int) Utils.Dialog((IWin32Window) MainForm.Instance, "This user account does not have a persona assigned to it. Contact your System Administrator to have this corrected.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (ex.LoginReturnCode == LoginReturnCode.Concurrent_Editing_Offline_Not_Allowed)
      {
        int num7 = (int) Utils.Dialog((IWin32Window) MainForm.Instance, "Multi-User Editing is enabled on this system. Please login to Encompass server using \"Networked\" connection mode.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (ex.LoginReturnCode == LoginReturnCode.IP_Blocked)
      {
        int num8 = (int) Utils.Dialog((IWin32Window) MainForm.Instance, "Remote access from IP address " + (object) ex.ClientIPAddress + " is not allowed.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (ex.LoginReturnCode == LoginReturnCode.TPO_LOGIN_RESTRICTED)
      {
        int num9 = (int) Utils.Dialog((IWin32Window) MainForm.Instance, "You are not authorized to log into Encompass.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (ex.LoginReturnCode == LoginReturnCode.SERVER_BUSY)
      {
        int num10 = (int) Utils.Dialog((IWin32Window) MainForm.Instance, "Server is currently busy. Please try again later.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else if (ex.LoginReturnCode == LoginReturnCode.API_USER_RESTRICTED)
      {
        int num11 = (int) Utils.Dialog((IWin32Window) MainForm.Instance, "You do not have sufficient privileges to log into Encompass, please contact your administrator.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (ex.LoginReturnCode == LoginReturnCode.SSO_USER_PASSWORD_NOT_ALLOWED)
      {
        int num12 = (int) Utils.Dialog((IWin32Window) MainForm.Instance, "Your login method has been changed to use your company credentials, please contact your administrator.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        int num13 = (int) Utils.Dialog((IWin32Window) MainForm.Instance, "Unknown login error.");
      }
    }

    private static void startAsyncEpassLoginTask()
    {
      EpassLogin.silentEpassLoginTask = Task.Run<bool>((Func<bool>) (() => EpassLogin.LoginUser(false)));
    }

    private static bool performLoginEncompass(
      string loginName,
      string password,
      string serverInstance,
      string authCode)
    {
      PerformanceMeter performanceMeter = PerformanceMeter.Get("Encompass.Login");
      bool flag = (serverInstance ?? "") == "";
      string displayVersionString = VersionInformation.CurrentVersion.DisplayVersionString;
      ThreadPool.SetMinThreads(15, 10);
      SystemSettings.LastLoginName = loginName;
      if ((serverInstance ?? "") == "")
      {
        SystemSettings.DefaultApplicationMode = ApplicationMode.Local;
        SystemSettings.ApplicationMode = ApplicationMode.Local;
        Session.Start(loginName, password, "Encompass");
        EncompassLoginRoutines.startAsyncEpassLoginTask();
      }
      else
      {
        SystemSettings.DefaultApplicationMode = ApplicationMode.Server;
        SystemSettings.ApplicationMode = ApplicationMode.Server;
        Session.Start(serverInstance, loginName, password, "Encompass", true, (string) null, authCode);
        EncompassLoginRoutines.startAsyncEpassLoginTask();
        performanceMeter.AddCheckpoint("Perform Login - Start Session", 293, nameof (performLoginEncompass), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\LoginHelper.cs");
        EncompassLoginRoutines.updateServerSettings(serverInstance);
        performanceMeter.AddCheckpoint("Perform Login - Update Server Settings", 298, nameof (performLoginEncompass), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\LoginHelper.cs");
        if (EnConfigurationSettings.GlobalSettings.InstallationMode == InstallationMode.Local)
          EncompassLoginRoutines.syncCompanySettings();
        performanceMeter.AddCheckpoint("Perform Login - Sync Company Settings", 304, nameof (performLoginEncompass), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\LoginHelper.cs");
      }
      DiagnosticSession.InitializeSessionVariables();
      if (!EncompassLoginRoutines.performLicenseChecks())
        return false;
      if (EnConfigurationSettings.GlobalSettings.Debug)
        Tracing.Log(true, "Init", EncompassLoginRoutines.className, "Server time is " + (object) Session.ServerTime);
      MainForm instance1 = MainForm.Instance;
      instance1.Text = instance1.Text + " - Build " + displayVersionString;
      if ((serverInstance ?? "") == "")
      {
        MainForm.Instance.Text += " - (Local)";
      }
      else
      {
        MainForm instance2 = MainForm.Instance;
        instance2.Text = instance2.Text + " - " + serverInstance;
      }
      if (!EncompassLoginRoutines.validatePassword())
        return false;
      EncompassLoginRoutines.checkCRMStatus(flag ? "(Local)" : serverInstance, password);
      MainForm instance3 = MainForm.Instance;
      instance3.Text = instance3.Text + " - " + Session.StartupInfo.UserInfo.Userid + " - " + Session.CompanyInfo.ClientID;
      if (PapiSettings.Debug)
        MainForm.Instance.Text += " - Partner API Debug Mode Enabled";
      try
      {
        new Thread(new ThreadStart(EncompassLoginRoutines.WriteDTToServer)).Start();
      }
      catch
      {
      }
      return true;
    }

    private static void checkCRMStatus(string server, string password)
    {
      if (Session.UserID != "admin")
        return;
      string companySetting = Session.ConfigurationManager.GetCompanySetting("Migration", "CRMTool");
      if (!(companySetting == "0") && !(companySetting == ""))
        return;
      CRMToolPrompt crmToolPrompt = new CRMToolPrompt();
      int num = (int) crmToolPrompt.ShowDialog((IWin32Window) MainForm.Instance);
      if (crmToolPrompt.SelectedOption == CRMToolPrompt.CRMOption.DonotShow)
        Session.ConfigurationManager.SetCompanySetting("Migration", "CRMTool", "2");
      else if (crmToolPrompt.SelectedOption == CRMToolPrompt.CRMOption.Later)
      {
        Session.ConfigurationManager.SetCompanySetting("Migration", "CRMTool", "0");
      }
      else
      {
        EncompassLoginRoutines.serverForCRMTool = server;
        EncompassLoginRoutines.pwdForCRMTool = password;
        new Thread(new ThreadStart(EncompassLoginRoutines.LaunchCRMTool))
        {
          Priority = ThreadPriority.Normal,
          IsBackground = false
        }.Start();
      }
    }

    private static void LaunchCRMTool()
    {
      Process process = new Process();
      process.StartInfo.CreateNoWindow = true;
      process.StartInfo.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
      string str = (EncompassLoginRoutines.serverForCRMTool == null ? "" : " -s " + EncompassLoginRoutines.serverForCRMTool) + (EncompassLoginRoutines.pwdForCRMTool == null ? "" : " -p " + EncompassLoginRoutines.pwdForCRMTool);
      if (AssemblyResolver.IsSmartClient)
      {
        process.StartInfo.FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AppLauncher.exe");
        process.StartInfo.Arguments = "CRMTool.exe" + str;
      }
      else
      {
        process.StartInfo.FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CRMTool.exe");
        process.StartInfo.Arguments = str.TrimStart();
      }
      process.StartInfo.UseShellExecute = false;
      process.Start();
    }

    private static bool validatePassword()
    {
      if (!Session.StartupInfo.IsPasswordChangeRequired)
        return true;
      using (ChangePasswordDialog changePasswordDialog = new ChangePasswordDialog())
        return changePasswordDialog.ShowDialog((IWin32Window) MainForm.Instance) == DialogResult.OK;
    }

    private static bool performLicenseChecks()
    {
      LicenseInfo serverLicense = Session.ServerLicense;
      if (serverLicense.IsTrialVersion && !EncompassLoginRoutines.showTrialLicensePrompt(serverLicense))
        return false;
      if (Session.UserInfo.IsAdministrator())
        EncompassLoginRoutines.performUserLimitCheck(serverLicense);
      return true;
    }

    private static bool showTrialLicensePrompt(LicenseInfo license)
    {
      if (new TrialUpgradeForm(license).ShowDialog((IWin32Window) MainForm.Instance) != DialogResult.OK)
        return true;
      Application.Exit();
      return false;
    }

    private static void performUserLimitCheck(LicenseInfo license)
    {
      int enabledUserCount = Session.OrganizationManager.GetEnabledUserCount();
      if (license.UserLimit <= 0 || enabledUserCount <= license.UserLimit)
        return;
      int num = (int) Utils.Dialog((IWin32Window) MainForm.Instance, "Your Encompass system is in violation of the Encompass licensing agreement." + Environment.NewLine + Environment.NewLine + "Your license permits " + (object) license.UserLimit + " enabled user(s), and you currently have " + (object) enabledUserCount + " enabled users. You will be unable to create or enable user accounts until you purchase additional user licenses or delete/disable existing users.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
    }

    private static void updateServerSettings(string serverName)
    {
      string[] servers = SystemSettings.Servers;
      string[] strArray;
      if (((IEnumerable<string>) servers).Where<string>((Func<string, bool>) (server => server.ToLower() == serverName.ToLower())).Count<string>() == 0)
      {
        strArray = new string[servers.Length + 1];
        strArray[0] = serverName;
        servers.CopyTo((Array) strArray, 1);
      }
      else
      {
        int index = 0;
        while (servers[index].ToLower() != serverName.ToLower())
          ++index;
        string str = servers[index];
        servers[index] = servers[0];
        servers[0] = str;
        strArray = servers;
      }
      if (strArray.Length == 0)
        return;
      SystemSettings.Servers = strArray;
    }

    private static void syncCompanySettings()
    {
      try
      {
        using (InProcConnection inProcConnection = new InProcConnection())
        {
          inProcConnection.OpenTrusted();
          IConfigurationManager configurationManager = (IConfigurationManager) inProcConnection.Session.GetObject("ConfigurationManager");
          CompanyInfo companyInfo = configurationManager.GetCompanyInfo();
          if (companyInfo.ClientID == Session.CompanyInfo.ClientID && companyInfo.Password != Session.CompanyInfo.Password && Session.CompanyInfo.Password != "")
            configurationManager.SetCompanySetting("CLIENT", "CLIENTPASSWORD", Session.CompanyInfo.Password);
          if (!(companyInfo.ClientID == Session.CompanyInfo.ClientID))
            return;
          configurationManager.UpdateCompanyDataServicesOpt(Session.ConfigurationManager.GetEncompassSystemInfo().DataServicesOptKey);
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(EncompassLoginRoutines.sw, EncompassLoginRoutines.className, TraceLevel.Warning, "Company password sync failed due to error: " + (object) ex);
      }
    }

    private static void downloadCompanyPassword()
    {
      try
      {
        Session.RecacheCompanyInfo();
      }
      catch (Exception ex)
      {
        Tracing.Log(EncompassLoginRoutines.sw, TraceLevel.Warning, EncompassLoginRoutines.className, "Unable to set local password from server: " + ex.Message);
      }
    }

    private static void WriteDTToServer()
    {
      try
      {
        string message = DTErrorLogger.ReadLog();
        if (!(message != ""))
          return;
        RemoteLogger.Write(TraceLevel.Warning, message);
        DTErrorLogger.Rename();
      }
      catch
      {
      }
    }
  }
}
