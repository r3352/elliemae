// Decompiled with JetBrains decompiler
// Type: Encompass.Diagnostics.Logging.Targets.RollingFileLogTarget
// Assembly: Encompass.Diagnostics, Version=1.0.0.1, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E8A3B074-7BF0-4187-B0D2-083265232A16
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Encompass.Diagnostics.dll

using Encompass.Diagnostics.Events;
using Encompass.Diagnostics.Logging.Formatters;
using Encompass.Diagnostics.Logging.Schema;
using System;

#nullable disable
namespace Encompass.Diagnostics.Logging.Targets
{
  public class RollingFileLogTarget : FileLogTarget, ILogTarget, IDisposable
  {
    private DateTime _lastRolloverTime = DateTime.MinValue;
    private readonly FileLogRolloverFrequency _frequency;
    private readonly string _rootName;

    public RollingFileLogTarget(
      string logDir,
      string rootName,
      FileLogRolloverFrequency frequency,
      IApplicationEventHandler eventHandler,
      ILogFormatter logFormatter)
      : this(logDir, rootName, DateTime.Now, frequency, eventHandler, logFormatter)
    {
      this._frequency = frequency;
      this._lastRolloverTime = DateTime.Now;
      this._rootName = ArgumentChecks.IsNotNullOrEmpty(rootName, nameof (rootName));
    }

    private RollingFileLogTarget(
      string logDir,
      string rootName,
      DateTime now,
      FileLogRolloverFrequency frequency,
      IApplicationEventHandler eventHandler,
      ILogFormatter logFormatter)
      : base(logDir, RollingFileLogTarget.generateFileName(rootName, now, frequency), eventHandler, logFormatter)
    {
    }

    private static string generateFileName(
      string rootName,
      DateTime now,
      FileLogRolloverFrequency frequency)
    {
      string frequencyPathString = RollingFileLogTarget.getFrequencyPathString(frequency, now);
      return rootName + frequencyPathString;
    }

    private static string getFrequencyPathString(FileLogRolloverFrequency frequency, DateTime now)
    {
      if (frequency == FileLogRolloverFrequency.Hourly)
        return "." + now.ToString("yyyy-MM-dd-HH");
      return frequency == FileLogRolloverFrequency.Daily ? "." + now.ToString("yyyy-MM-dd") : "";
    }

    private bool isRolloverRequired()
    {
      if (this._frequency == FileLogRolloverFrequency.None)
        return false;
      DateTime now = DateTime.Now;
      return this._frequency == FileLogRolloverFrequency.Hourly ? now.Date != this._lastRolloverTime.Date || now.Hour != this._lastRolloverTime.Hour : now.Date != this._lastRolloverTime.Date;
    }

    protected override void WriteInternal(Log log)
    {
      lock (this)
      {
        if (this.isRolloverRequired())
        {
          this._lastRolloverTime = DateTime.Now;
          this._fileName = RollingFileLogTarget.generateFileName(this._rootName, this._lastRolloverTime, this._frequency);
          this.Reconnect();
        }
        base.WriteInternal(log);
      }
    }
  }
}
