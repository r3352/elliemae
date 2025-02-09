// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerGlobals
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.Logging;
using EllieMae.EMLite.RemotingServices;
using Encompass.Diagnostics.Config;
using System;
using System.Configuration;
using System.IO;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class ServerGlobals : EllieMae.EMLite.DataAccess.ServerGlobals
  {
    private const string className = "ServerGlobals�";
    public static readonly EllieMae.EMLite.ClientServer.IContextTraceLog TraceLog = (EllieMae.EMLite.ClientServer.IContextTraceLog) new ContextTraceLog((ClientContext) null);
    public static readonly DirectoryInfo UpdatesFolder;
    public static readonly RuntimeEnvironment RuntimeEnvironment = EnConfigurationSettings.GlobalSettings.RuntimeEnvironment;
    private static bool apiTraceEnabled = EnConfigurationSettings.GlobalSettings.APITrace;
    private static TimeSpan apiLatency = EnConfigurationSettings.GlobalSettings.APILatency;
    private static TimeSpan serverActivityTimeout = EnConfigurationSettings.GlobalSettings.ServerActivityTimeout;
    private static TimeSpan serverActivityTimerInterval = EnConfigurationSettings.GlobalSettings.ServerActivityTimerInterval;
    private static TimeSpan instanceConfigRefreshInterval = EnConfigurationSettings.GlobalSettings.InstanceConfigRefreshInterval;
    private static TimeSpan connectionTimeout = TimeSpan.FromMinutes(5.0);
    private static string rmiPassword = EnConfigurationSettings.GlobalSettings.RMIPassword;
    private static int taskProcessingThreadCount = EnConfigurationSettings.GlobalSettings.TaskProcessingThreadCount;
    private static TimeSpan taskSchedulerInterval = EnConfigurationSettings.GlobalSettings.TaskSchedulerInterval;
    private static bool taskSchedulerDisabled = EnConfigurationSettings.GlobalSettings.TaskSchedulerDisabled;
    private static int maxServerEvents = EnConfigurationSettings.GlobalSettings.MaxServerEvents;
    private static int lockTimeoutDuringGetCache = EnConfigurationSettings.GlobalSettings.LockTimeoutDuringGetCache;
    private static bool cacheRegetFromCache = EnConfigurationSettings.GlobalSettings.CacheRegetFromCache > 0;
    private static bool cacheRegetFromDB = EnConfigurationSettings.GlobalSettings.CacheRegetFromDB > 0;
    private static int maxHazelCastClients = EnConfigurationSettings.GlobalSettings.MaxHazelCastClients;
    private static int maxHazelCastRequestsPerConnection = EnConfigurationSettings.GlobalSettings.MaxHazelCastRequestsPerConnection;
    private static int hazelcastUnusedConnectionShutdownIdleTimeInSeconds = EnConfigurationSettings.GlobalSettings.HazelcastUnusedConnectionShutdownIdleTimeInSeconds;

    private ServerGlobals()
    {
    }

    static ServerGlobals()
    {
      ServerGlobals.UpdatesFolder = new DirectoryInfo(Path.Combine(EnConfigurationSettings.GlobalSettings.AppSettingsDirectory, "Updates"));
      Directory.CreateDirectory(ServerGlobals.UpdatesFolder.FullName);
      EnConfigurationSettings.GlobalSettings.SettingsChanged += new EventHandler(ServerGlobals.onGlobalSettingsChanged);
      EnConfigurationSettings.GlobalSettings.EnableMonitoring();
      FileSystemWatcher fileSystemWatcher = new FileSystemWatcher(Path.GetDirectoryName(StandardFields.MapFilePath), Path.GetFileName(StandardFields.MapFilePath));
      fileSystemWatcher.Changed += new FileSystemEventHandler(ServerGlobals.onStandardFieldListChanged);
      fileSystemWatcher.EnableRaisingEvents = true;
      ServerGlobals.onGlobalSettingsChanged((object) null, (EventArgs) null);
    }

    public static bool APITrace => ServerGlobals.apiTraceEnabled;

    public static TimeSpan APILatency => ServerGlobals.apiLatency;

    public static TimeSpan ConnectionTimeout => ServerGlobals.connectionTimeout;

    public static string RMIPassword => ServerGlobals.rmiPassword;

    public static int TaskProcessingThreadCount => ServerGlobals.taskProcessingThreadCount;

    public static TimeSpan TaskSchedulerInterval => ServerGlobals.taskSchedulerInterval;

    public static bool TaskSchedulerDisabled => ServerGlobals.taskSchedulerDisabled;

    public static TimeSpan ServerActivityTimeout => ServerGlobals.serverActivityTimeout;

    public static TimeSpan ServerActivityTimerInterval => ServerGlobals.serverActivityTimerInterval;

    public static TimeSpan InstanceConfigRefreshInterval
    {
      get => ServerGlobals.instanceConfigRefreshInterval;
    }

    public static bool IsRunningAsPlatform
    {
      get
      {
        string appSetting = ConfigurationManager.AppSettings[nameof (IsRunningAsPlatform)];
        bool result;
        return !string.IsNullOrEmpty(appSetting) && bool.TryParse(appSetting, out result) && result;
      }
    }

    public static int MaxServerEvents => ServerGlobals.maxServerEvents;

    public static int LockTimeoutDuringGetCache => ServerGlobals.lockTimeoutDuringGetCache;

    public static bool CacheRegetFromCache => ServerGlobals.cacheRegetFromCache;

    public static bool CacheRegetFromDB => ServerGlobals.cacheRegetFromDB;

    public static int MaxHazelCastClients => ServerGlobals.maxHazelCastClients;

    public static int MaxHazelCastRequestsPerConnection
    {
      get => ServerGlobals.maxHazelCastRequestsPerConnection;
    }

    public static int HazelcastUnusedConnectionShutdownIdleTimeInSeconds
    {
      get => ServerGlobals.hazelcastUnusedConnectionShutdownIdleTimeInSeconds;
    }

    private static void onGlobalSettingsChanged(object sender, EventArgs e)
    {
      ServerGlobals.apiTraceEnabled = EnConfigurationSettings.GlobalSettings.APITrace;
      ServerGlobals.apiLatency = EnConfigurationSettings.GlobalSettings.APILatency;
      ServerGlobals.connectionTimeout = EnConfigurationSettings.GlobalSettings.ServerConnectionTimeout;
      ServerGlobals.rmiPassword = EnConfigurationSettings.GlobalSettings.RMIPassword;
      ServerGlobals.taskProcessingThreadCount = EnConfigurationSettings.GlobalSettings.TaskProcessingThreadCount;
      ServerGlobals.taskSchedulerInterval = EnConfigurationSettings.GlobalSettings.TaskSchedulerInterval;
      ServerGlobals.taskSchedulerDisabled = EnConfigurationSettings.GlobalSettings.TaskSchedulerDisabled;
      ServerGlobals.maxServerEvents = EnConfigurationSettings.GlobalSettings.MaxServerEvents;
      ServerGlobals.lockTimeoutDuringGetCache = EnConfigurationSettings.GlobalSettings.LockTimeoutDuringGetCache;
      ServerGlobals.cacheRegetFromCache = EnConfigurationSettings.GlobalSettings.CacheRegetFromCache > 0;
      ServerGlobals.cacheRegetFromDB = EnConfigurationSettings.GlobalSettings.CacheRegetFromDB > 0;
      ServerGlobals.maxHazelCastClients = EnConfigurationSettings.GlobalSettings.MaxHazelCastClients;
      ServerGlobals.maxHazelCastRequestsPerConnection = EnConfigurationSettings.GlobalSettings.MaxHazelCastRequestsPerConnection;
      ServerGlobals.hazelcastUnusedConnectionShutdownIdleTimeInSeconds = EnConfigurationSettings.GlobalSettings.HazelcastUnusedConnectionShutdownIdleTimeInSeconds;
      ServerGlobals.serverActivityTimeout = EnConfigurationSettings.GlobalSettings.ServerActivityTimeout;
      ServerGlobals.serverActivityTimerInterval = EnConfigurationSettings.GlobalSettings.ServerActivityTimerInterval;
      ServerStatusMonitor.ChangeTimerInterval(ServerGlobals.ServerActivityTimerInterval);
      ServerGlobals.instanceConfigRefreshInterval = EnConfigurationSettings.GlobalSettings.InstanceConfigRefreshInterval;
      ServerInstanceConfigMonitor.ChangeTimerInterval(ServerGlobals.InstanceConfigRefreshInterval);
      RollingFileSystemLog.ApplyRolloverFrequency(EnConfigurationSettings.GlobalSettings.ServerLogRolloverFrequency);
      DiagConfig<ServerDiagConfigData>.Instance.ReloadConfig();
    }

    public static void ReplaceEncompassFieldList(
      BinaryObject data,
      DateTime fileModificationTimeUtc)
    {
      try
      {
        StandardFields.AcquireFileLock();
      }
      catch (Exception ex)
      {
        throw new Exception("Failed to acquire EncompassFields file lock", ex);
      }
      try
      {
        string mapFilePath = StandardFields.MapFilePath;
        string withoutExtension = Path.GetFileNameWithoutExtension(mapFilePath);
        string extension = Path.GetExtension(mapFilePath);
        string directoryName = Path.GetDirectoryName(mapFilePath);
        int num = 1;
        string str;
        while (true)
        {
          str = Path.Combine(directoryName, withoutExtension + "." + (object) num + extension);
          if (File.Exists(str))
            ++num;
          else
            break;
        }
        File.Move(mapFilePath, str);
        try
        {
          data.Write(mapFilePath);
          new FileInfo(mapFilePath).LastWriteTimeUtc = fileModificationTimeUtc;
        }
        catch
        {
          if (File.Exists(mapFilePath))
            File.Delete(mapFilePath);
          File.Move(str, mapFilePath);
          throw;
        }
        ServerGlobals.TraceLog.WriteInfo(nameof (ServerGlobals), "Successfully replaced field definition file '" + mapFilePath + "'");
      }
      catch (Exception ex)
      {
        throw new Exception("Failed to replace field definition file", ex);
      }
      finally
      {
        StandardFields.ReleaseFileLock();
      }
    }

    private static void onStandardFieldListChanged(object sender, FileSystemEventArgs e)
    {
      if (!EnConfigurationSettings.GlobalSettings.AutoSyncFieldList)
        return;
      try
      {
        ServerGlobals.TraceLog.WriteInfo(nameof (ServerGlobals), "Change detected in EncompassFields.dat file. Auto-reload is enabled.");
        if (!new FileInfo(StandardFields.MapFilePath).Exists)
        {
          ServerGlobals.TraceLog.WriteInfo(nameof (ServerGlobals), "Encompass field list file '" + StandardFields.MapFilePath + "' not found and will not be auto-reloaded.");
        }
        else
        {
          StandardFields.Instance.Reload(true);
          ServerGlobals.TraceLog.WriteInfo(nameof (ServerGlobals), "Encompass standard fields auto-reloaded successfully.");
        }
      }
      catch (IOException ex)
      {
        ServerGlobals.TraceLog.WriteWarning(nameof (ServerGlobals), "Encompass standard fields failed to auto-reload due to I/O error (" + ex.Message + "). This is often normal and should be followed by a successful reload.");
      }
      catch (Exception ex)
      {
        ServerGlobals.TraceLog.WriteError(nameof (ServerGlobals), "Encompass standard fields failed to auto-reload: " + (object) ex);
      }
    }
  }
}
