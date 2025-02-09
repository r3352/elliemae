// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.Installer.ServerUpdateInstaller
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.ClientServer.Events;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Version;
using EllieMae.EMLite.Server;
using EllieMae.Encompass.EncAppMgr;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Timers;

#nullable disable
namespace Elli.Server.Remoting.Installer
{
  public class ServerUpdateInstaller
  {
    private const string SHUInstallerclassName = "SHU";
    private const string SMUInstallerclassName = "SMU";
    private const string encAppMgrIpcUrl = "ipc://EncAppMgr/EncAppMgrRO.rem";
    private static System.Timers.Timer timerHotUpdate = new System.Timers.Timer();
    private static string _clientID_ = (string) null;
    private const string versionControlExe = "VersionControl.exe";

    private static string checkEncAppMgr(bool isSHU)
    {
      try
      {
        ((IEncAppMgrRO) Activator.GetObject(typeof (IEncAppMgrRO), "ipc://EncAppMgr/EncAppMgrRO.rem")).IsAlive();
        return (string) null;
      }
      catch (Exception ex)
      {
        string msg = "Unable to connect to EncAppMgr: " + ex.Message;
        Tracing.Log(true, "Error", isSHU ? "SHU" : "SMU", msg);
        return msg;
      }
    }

    public static string DownloadAndApplyServerHotUpdates(
      string userid,
      string password,
      int tipUpdateNumber,
      bool raiseOnErr,
      bool isSHU)
    {
      Tracing.Log(true, "Info", "SMU", "DownloadAndApplyServerHotUpdates in ServerUpdateInstaller");
      string str;
      try
      {
        str = ServerUpdateInstaller.DownloadServerUpdates(userid, password, tipUpdateNumber, raiseOnErr, isSHU);
        if (str == null)
          ServerUpdateInstaller.prepareUpdates(userid, password, VersionInformation.CurrentVersion.Version.FullVersion, tipUpdateNumber, isSHU);
      }
      catch (Exception ex)
      {
        str = "Error applying server updates: " + ex.Message;
        string className = isSHU ? "SHU" : "SMU";
        if (raiseOnErr)
          Err.Raise(TraceLevel.Error, className, new ServerException(str));
        else
          Tracing.Log(true, "Error", className, str);
      }
      return str;
    }

    public static string DownloadServerUpdates(
      string userid,
      string password,
      int tipUpdateNumber,
      bool raiseOnErr,
      bool isSHU)
    {
      string str1 = ServerUpdateInstaller.checkEncAppMgr(isSHU);
      if (str1 != null)
        return str1;
      string className = isSHU ? "SHU" : "SMU";
      Tracing.Log(true, "Info", className, nameof (DownloadServerUpdates));
      string str2;
      try
      {
        IEncAppMgrRO encAppMgrRo = (IEncAppMgrRO) Activator.GetObject(typeof (IEncAppMgrRO), "ipc://EncAppMgr/EncAppMgrRO.rem");
        str2 = !isSHU ? encAppMgrRo.DownloadServerMajorUpdates(userid, password, ServerUpdateInstaller._clientID(false), VersionInformation.CurrentVersion.Version.FullVersion, tipUpdateNumber) : encAppMgrRo.DownloadServerHotUpdates(userid, password, ServerUpdateInstaller._clientID(true), VersionInformation.CurrentVersion.Version.FullVersion, tipUpdateNumber);
        if (str2 != null)
          Tracing.Log(true, "Error", className, "Error downloading server updates: " + str2);
      }
      catch (Exception ex)
      {
        str2 = "Error downloading server hot updates: " + ex.Message;
        if (raiseOnErr)
          Err.Raise(TraceLevel.Error, className, new ServerException(str2));
        else
          Tracing.Log(true, "Error", className, str2);
      }
      return str2;
    }

    internal static void StartServerHotUpdateTimer()
    {
      if (EllieMae.EMLite.Server.ServerGlobals.RuntimeEnvironment == RuntimeEnvironment.Hosted || !EncompassSettings.Default.ApplyServerHotUpdates)
        return;
      ServerUpdateInstaller.timerHotUpdate.Interval = 100.0;
      ServerUpdateInstaller.timerHotUpdate.Elapsed += new ElapsedEventHandler(ServerUpdateInstaller.timerHotUpdate_Elapsed);
      ServerUpdateInstaller.timerHotUpdate.Start();
    }

    private static void timerHotUpdate_Elapsed(object sender, ElapsedEventArgs e)
    {
      ServerUpdateInstaller.timerHotUpdate.Stop();
      try
      {
        ShuSetting suSetting1 = (ShuSetting) ServerUpdateInstaller.getSuSetting("Setting", true);
        if (suSetting1 != ShuSetting.Manual)
        {
          DateTime dateTime = DateTime.Now;
          if (dateTime.Minute < 10 || dateTime.Minute > 50)
          {
            if (dateTime.Minute < 10)
            {
              dateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, 0, 0);
            }
            else
            {
              dateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 0);
              dateTime = dateTime.AddMinutes((double) (60 - dateTime.Minute));
            }
            ShuSettingDay suSetting2 = (ShuSettingDay) ServerUpdateInstaller.getSuSetting("SettingDay", true);
            ShuSettingTime suSetting3 = (ShuSettingTime) ServerUpdateInstaller.getSuSetting("SettingTime", true);
            if ((suSetting2 == ShuSettingDay.EveryDay || suSetting2.ToString().IndexOf(dateTime.DayOfWeek.ToString()) >= 0) && suSetting3 == (ShuSettingTime) dateTime.Hour)
            {
              string userid = "(rmi)";
              string password = EnConfigurationSettings.GlobalSettings.RMIPassword;
              if ((password ?? "").Trim() == "")
              {
                userid = "(trusted)";
                password = EnConfigurationSettings.GlobalSettings.DatabasePassword;
              }
              switch (suSetting1)
              {
                case ShuSetting.AutoInstall:
                  ServerUpdateInstaller.DownloadAndApplyServerHotUpdates(userid, password, 0, false, true);
                  break;
                case ShuSetting.AutoDownload:
                  ServerUpdateInstaller.DownloadServerUpdates(userid, password, 0, false, true);
                  break;
              }
            }
          }
        }
        ServerUpdateInstaller.timerHotUpdate.Interval = ServerUpdateInstaller.getTimerInterval();
      }
      catch (Exception ex)
      {
        EllieMae.EMLite.Server.ServerGlobals.TraceLog.WriteError("SMU", "Error checking/applying hot update: " + ex.Message);
      }
      finally
      {
        ServerUpdateInstaller.timerHotUpdate.Start();
      }
    }

    private static double getTimerInterval()
    {
      DateTime now = DateTime.Now;
      DateTime dateTime = now.AddHours(1.0);
      return ((now.Minute > 30 ? dateTime.AddMinutes((double) (60 - now.Minute)) : dateTime.AddMinutes((double) (-1 * now.Minute))) - now).TotalMilliseconds;
    }

    private static object getSuSetting(string attribute, bool isSHU)
    {
      string str1 = (string) null;
      string str2 = !isSHU ? "SMU" : "SHU";
      using (EllieMae.EMLite.DataAccess.DbAccessManager dbAccessManager = new EllieMae.EMLite.DataAccess.DbAccessManager(EnConfigurationSettings.GlobalSettings.DatabaseConnectionString, EnConfigurationSettings.GlobalSettings.DatabaseType))
        str1 = (string) dbAccessManager.ExecuteScalar("select [value] from [company_settings] where [category] = '" + str2 + "' and [attribute] = '" + attribute + "'");
      return SettingDefinitions.GetSettingDefinition(str2 + "." + attribute).Parse(str1);
    }

    private static string _clientID(bool isSHU)
    {
      string className = isSHU ? "SHU" : "SMU";
      if (ServerUpdateInstaller._clientID_ == null)
      {
        try
        {
          if (EllieMae.EMLite.Server.ServerGlobals.RuntimeEnvironment == RuntimeEnvironment.Hosted)
            ServerUpdateInstaller._clientID_ = "(hosted)";
          using (EllieMae.EMLite.DataAccess.DbAccessManager dbAccessManager = new EllieMae.EMLite.DataAccess.DbAccessManager(EnConfigurationSettings.GlobalSettings.DatabaseConnectionString, EnConfigurationSettings.GlobalSettings.DatabaseType))
            ServerUpdateInstaller._clientID_ = (string) dbAccessManager.ExecuteScalar("select [value] from [company_settings] where [category] = 'CLIENT' and [attribute] = 'CLIENTID'");
        }
        catch (Exception ex)
        {
          Tracing.Log(true, "Error", className, "Error getting client ID: " + ex.Message);
        }
      }
      return ServerUpdateInstaller._clientID_ ?? "";
    }

    private static void prepareUpdates(
      string userid,
      string password,
      string encMajorVersion,
      int tipUpdateNumber,
      bool isSHU)
    {
      string className = isSHU ? "SHU" : "SMU";
      Tracing.Log(true, "Info", className, "Prepare updates for " + className.ToString());
      bool stopServer = false;
      int num = isSHU ? VersionInformation.CurrentVersion.GetMaxInstalledServerHotUpdateNumber() : VersionInformation.CurrentVersion.GetMaxInstalledServerMajorUpdateNumber();
      bool serverSetting = (bool) ClientContext.GetCurrent().Settings.GetServerSetting("Login.Enabled");
      string[] downloadedSuFiles = VersionInformation.CurrentVersion.GetDownloadedSuFiles(new System.Version(encMajorVersion + ".0"), isSHU);
      if (downloadedSuFiles == null || downloadedSuFiles.Length == 0)
        return;
      List<ServerHotUpdateInfo> serverHotUpdateInfoList = new List<ServerHotUpdateInfo>();
      foreach (string filename in downloadedSuFiles)
      {
        ServerHotUpdateInfo serverHotUpdateInfo = new ServerHotUpdateInfo(filename);
        if ((tipUpdateNumber <= 0 || serverHotUpdateInfo.Version.UpdateNumber <= tipUpdateNumber) && serverHotUpdateInfo.Version.UpdateNumber > num)
        {
          if (!filename.ToLower().EndsWith(".cemzip"))
            stopServer = true;
          serverHotUpdateInfoList.Add(serverHotUpdateInfo);
        }
      }
      if (serverHotUpdateInfoList.Count == 0)
        return;
      serverHotUpdateInfoList.Sort();
      List<string> stringList1 = new List<string>();
      List<string> stringList2 = new List<string>();
      foreach (ServerHotUpdateInfo serverHotUpdateInfo in serverHotUpdateInfoList)
      {
        stringList1.Add(serverHotUpdateInfo.Filename);
        stringList2.Add(serverHotUpdateInfo.Version.DisplayVersionString);
      }
      IEncAppMgrRO encAppMgrRo = (IEncAppMgrRO) Activator.GetObject(typeof (IEncAppMgrRO), "ipc://EncAppMgr/EncAppMgrRO.rem");
      string str = !isSHU ? encAppMgrRo.PrepareServerMajorUpdates(userid, password, encMajorVersion, num, stringList1.ToArray()) : encAppMgrRo.PrepareServerHotUpdates(userid, password, encMajorVersion, num, stringList1.ToArray());
      if (str != null)
        throw new Exception("Error preparing updates: " + str);
      if (stopServer)
      {
        Tracing.Log(true, "Info", className, "Need to shut down the server later");
        int suSetting = (int) ServerUpdateInstaller.getSuSetting("SettingNotificationTime", isSHU);
        if (suSetting > 0)
        {
          Message message = !isSHU ? (Message) new SmuMessage((string) ServerUpdateInstaller.getSuSetting("SettingNotificationMessage", isSHU), suSetting) : (Message) new ShuMessage(suSetting);
          Tracing.Log(true, "Info", className, "The Broadcast begins");
          foreach (ClientContext clientContext in ClientContext.GetAll())
            clientContext.Sessions.BroadcastMessage(message, false);
          Tracing.Log(true, "Info", className, "The Broadcast finishes");
          Tracing.Log(true, "Info", className, "The system will beign to sleep");
          Thread.Sleep(suSetting * 1000);
          Tracing.Log(true, "Info", className, "The system finish sleeping");
        }
        if (isSHU)
        {
          RemotableEncompassServer.Stop(DisconnectEventArgument.ShuReconnect);
          EllieMae.EMLite.Server.ServerGlobals.TraceLog.WriteInfo("SHU", "EncompassServer stopped (with reconnect) to apply hot updates");
        }
        else
        {
          if (serverSetting)
            ClientContext.GetCurrent().Settings.SetServerSetting("Login.Enabled", (object) true);
          RemotableEncompassServer.Stop(DisconnectEventArgument.Force);
          EllieMae.EMLite.Server.ServerGlobals.TraceLog.WriteInfo("SMU", "EncompassServer stopped to apply major updates");
        }
      }
      if (isSHU)
        encAppMgrRo.InstallServerHotUpdates(userid, password, encMajorVersion, num, stringList2.ToArray(), stopServer);
      else
        encAppMgrRo.InstallServerMajorUpdates(userid, password, encMajorVersion, num, stringList2.ToArray(), stopServer);
      if (stopServer)
      {
        if (EncompassServer.ServerMode == EncompassServerMode.HTTP)
        {
          try
          {
            encAppMgrRo.StopIIsEncompassServer(userid, password);
            EllieMae.EMLite.Server.ServerGlobals.TraceLog.WriteInfo(className, "Encompass server (IIS) stopped to apply hot updates");
          }
          catch (Exception ex)
          {
            Tracing.Log(true, "Warning", className, "Error stopping EncompassServer (IIS): " + ex.Message);
          }
        }
        Environment.Exit(0);
      }
      VersionInformation.WaitForVersionControlToExit();
      VersionInformation.ReloadVersionInformation();
    }

    private static void backupFiles(string shuDir, string backupDir, string currSubDir)
    {
      string path1 = Path.Combine(EnConfigurationSettings.GlobalSettings.EncompassProgramDirectory, currSubDir);
      string path = Path.Combine(shuDir, currSubDir);
      string str1 = Path.Combine(backupDir, currSubDir);
      if (!Directory.Exists(str1))
        Directory.CreateDirectory(str1);
      foreach (string file in Directory.GetFiles(path))
      {
        string fileName = Path.GetFileName(file);
        string str2 = Path.Combine(path1, fileName);
        string destFileName = Path.Combine(str1, fileName);
        if (File.Exists(str2))
          File.Copy(str2, destFileName, true);
      }
      foreach (string directory in Directory.GetDirectories(path))
        ServerUpdateInstaller.backupFiles(shuDir, backupDir, Path.GetFileName(directory));
    }

    private static void killProcess(string processName)
    {
      foreach (Process proc in Process.GetProcessesByName(processName))
        ServerUpdateInstaller.killWait(proc);
    }

    private static void killWait(Process proc)
    {
      proc.Kill();
      proc.WaitForExit(10000);
    }
  }
}
