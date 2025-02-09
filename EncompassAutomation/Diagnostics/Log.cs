// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Diagnostics.Log
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

using Encompass.Diagnostics.Logging;
using Encompass.Diagnostics.PII;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.Diagnostics
{
  /// <summary>Represents single log written by application</summary>
  public class Log : ILog
  {
    internal Log(Encompass.Diagnostics.Logging.Schema.Log log)
    {
      bool flag = log.RequiresMaskingForSQL();
      this.Message = MaskUtilities.MaskPII(log.GetLogMessage(), flag);
      this.AdditionalFields = log.GetAdditionalFields();
      if (log.Error != null)
        this.Exception = (ILogErrorData) new LogErrorData(log.Error, flag);
      this.ThreadId = new int?(log.ThreadId);
      this.TimeStamp = log.GetLogTime();
      this.Switch = Log.GetLogSwitch(log);
      this.LogLevel = Log.GetLogLevel(log.Level.GetBaseType());
      this.Source = log.Src;
    }

    /// <summary>Describes the event being logged.</summary>
    public string Message { get; }

    /// <summary>Date-timestamp at which the log was generated</summary>
    public DateTime TimeStamp { get; }

    /// <summary>Additional log fields.</summary>
    public Dictionary<string, object> AdditionalFields { get; }

    /// <summary>
    /// If this logs is for an Exception this field will be populated.
    /// </summary>
    public ILogErrorData Exception { get; }

    /// <summary>
    /// Managed Thread Id of the thread that generated the log
    /// </summary>
    public int? ThreadId { get; }

    /// <summary>LogLevel for the log</summary>
    public LogLevel LogLevel { get; }

    /// <summary>
    /// Source of the log. Generally the class or method name where the log is originated.
    /// </summary>
    public string Source { get; }

    /// <summary>
    /// Logger switch using wich log was written. A logger switch corresponds to a functional area for which granularity of logs can be configured.
    /// </summary>
    public string Switch { get; }

    internal static LogLevel GetLogLevel(Encompass.Diagnostics.Logging.LogLevel level)
    {
      switch (level)
      {
        case Encompass.Diagnostics.Logging.LogLevel.ERROR:
          return LogLevel.Error;
        case Encompass.Diagnostics.Logging.LogLevel.WARN:
          return LogLevel.Warning;
        case Encompass.Diagnostics.Logging.LogLevel.INFO:
          return LogLevel.Information;
        case Encompass.Diagnostics.Logging.LogLevel.DEBUG:
          return LogLevel.Verbose;
        default:
          return LogLevel.Off;
      }
    }

    internal static string GetLogSwitch(Encompass.Diagnostics.Logging.Schema.Log log) => log.Logger;
  }
}
