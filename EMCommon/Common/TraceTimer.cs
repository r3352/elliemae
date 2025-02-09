// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.TraceTimer
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using Encompass.Diagnostics.Logging;
using Encompass.Diagnostics.Logging.Schema;
using System;
using System.Runtime.Serialization;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public class TraceTimer : IDisposable
  {
    private readonly ILogger logger;
    private readonly string className;
    private readonly string description;
    private readonly DateTime start;
    private readonly Encompass.Diagnostics.Logging.LogLevel level;

    internal TraceTimer(ILogger logger, string className, string description, Encompass.Diagnostics.Logging.LogLevel level)
    {
      this.logger = logger;
      this.className = className;
      this.description = description;
      this.start = DateTime.UtcNow;
      this.level = level;
      TraceTimer.TimerLog log = new TraceTimer.TimerLog();
      log.Level = level;
      log.Src = className;
      log.Message = description;
      log.Set<DateTime>(Log.CommonFields.StartTime, this.start);
      log.Set<LogEventType>(Log.CommonFields.EventType, LogEventType.Start);
      logger.Write<TraceTimer.TimerLog>(log);
    }

    void IDisposable.Dispose() => this.Stop();

    public void Stop()
    {
      DateTime utcNow = DateTime.UtcNow;
      TraceTimer.TimerLog log = new TraceTimer.TimerLog();
      log.Level = this.level;
      log.Src = this.className;
      log.Message = this.description;
      log.Set<LogEventType>(Log.CommonFields.EventType, LogEventType.End);
      log.Set<DateTime>(Log.CommonFields.StartTime, this.start);
      log.Set<DateTime>(Log.CommonFields.EndTime, utcNow);
      log.Set<double>(Log.CommonFields.DurationMS, (utcNow - this.start).TotalMilliseconds);
      this.logger.Write<TraceTimer.TimerLog>(log);
    }

    public class TimerLog : Log
    {
      public TimerLog()
      {
      }

      protected TimerLog(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
      }

      public override string GetLogMessage()
      {
        double tvalue;
        return this.TryGet<double>(Log.CommonFields.DurationMS, out tvalue) ? this.Message + ". Elapsed Time = " + tvalue.ToString("0") + "ms" : this.Message;
      }

      public override string GetLogLevel() => "TIMER_" + base.GetLogLevel();
    }
  }
}
