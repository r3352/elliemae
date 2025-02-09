// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Logging.RollingFileSystemLog
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using Encompass.Diagnostics.Logging.Targets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

#nullable disable
namespace EllieMae.EMLite.Logging
{
  public class RollingFileSystemLog : FileSystemLog
  {
    private FileLogRolloverFrequency rolloverFrequency;
    private DateTime lastRolloverTime = DateTime.MinValue;
    private static Timer rolloverTimer = (Timer) null;
    private static List<RollingFileSystemLog> activeLogs = new List<RollingFileSystemLog>();

    static RollingFileSystemLog()
    {
      RollingFileSystemLog.rolloverTimer = new Timer(new TimerCallback(RollingFileSystemLog.evaluateRollover), (object) null, TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0));
    }

    private RollingFileSystemLog(
      string logDir,
      string rootName,
      DateTime now,
      FileLogRolloverFrequency frequency)
      : base(RollingFileSystemLog.generateLogPath(logDir, rootName, now, frequency))
    {
      this.lastRolloverTime = now;
      this.LogDir = logDir;
      this.RootName = rootName;
      this.rolloverFrequency = frequency;
    }

    public string LogDir { get; private set; }

    public string RootName { get; private set; }

    public FileLogRolloverFrequency RolloverFrequency
    {
      get
      {
        lock (this)
          return this.rolloverFrequency;
      }
      set
      {
        lock (this)
        {
          if (this.rolloverFrequency == value)
            return;
          this.rolloverFrequency = value;
          this.lastRolloverTime = DateTime.MinValue;
        }
      }
    }

    private void Rollover()
    {
      lock (this)
      {
        this.lastRolloverTime = DateTime.Now;
        this.ChangePath(RollingFileSystemLog.generateLogPath(this.LogDir, this.RootName, this.lastRolloverTime, this.RolloverFrequency));
      }
    }

    public override void Close()
    {
      lock (RollingFileSystemLog.activeLogs)
        RollingFileSystemLog.activeLogs.Remove(this);
      base.Close();
    }

    private static string generateLogPath(
      string logDir,
      string rootName,
      DateTime now,
      FileLogRolloverFrequency frequency)
    {
      string frequencyPathString = RollingFileSystemLog.getFrequencyPathString(frequency, now);
      return Path.Combine(logDir, rootName + frequencyPathString + ".log");
    }

    private static string getFrequencyPathString(FileLogRolloverFrequency frequency, DateTime now)
    {
      if (frequency == FileLogRolloverFrequency.Hourly)
        return "." + now.ToString("yyyy-MM-dd-HH");
      return frequency == FileLogRolloverFrequency.Daily ? "." + now.ToString("yyyy-MM-dd") : "";
    }

    public static RollingFileSystemLog Create(
      string logDir,
      string rootName,
      FileLogRolloverFrequency frequency)
    {
      RollingFileSystemLog rollingFileSystemLog = new RollingFileSystemLog(logDir, rootName, DateTime.Now, frequency);
      lock (RollingFileSystemLog.activeLogs)
        RollingFileSystemLog.activeLogs.Add(rollingFileSystemLog);
      return rollingFileSystemLog;
    }

    public static void ApplyRolloverFrequency(FileLogRolloverFrequency frequency)
    {
      lock (RollingFileSystemLog.activeLogs)
      {
        foreach (RollingFileSystemLog activeLog in RollingFileSystemLog.activeLogs)
          activeLog.RolloverFrequency = frequency;
      }
    }

    private static void evaluateRollover(object notUsed)
    {
      DateTime now = DateTime.Now;
      List<RollingFileSystemLog> rollingFileSystemLogList;
      lock (RollingFileSystemLog.activeLogs)
        rollingFileSystemLogList = new List<RollingFileSystemLog>((IEnumerable<RollingFileSystemLog>) RollingFileSystemLog.activeLogs);
      foreach (RollingFileSystemLog rollingFileSystemLog in rollingFileSystemLogList)
      {
        if (RollingFileSystemLog.isRolloverRequired(rollingFileSystemLog.lastRolloverTime, now, rollingFileSystemLog.RolloverFrequency))
          rollingFileSystemLog.Rollover();
      }
    }

    private static bool isRolloverRequired(
      DateTime lastRolloverTime,
      DateTime now,
      FileLogRolloverFrequency frequency)
    {
      switch (frequency)
      {
        case FileLogRolloverFrequency.Hourly:
          return now.Date != lastRolloverTime.Date || now.Hour != lastRolloverTime.Hour;
        case FileLogRolloverFrequency.Daily:
          return now.Date != lastRolloverTime.Date;
        default:
          return false;
      }
    }
  }
}
