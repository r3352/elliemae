// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Diagnostics.Log
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll

using Encompass.Diagnostics.Logging;
using Encompass.Diagnostics.Logging.Schema;
using Encompass.Diagnostics.PII;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.Diagnostics
{
  public class Log : ILog
  {
    internal Log(Log log)
    {
      bool maskSQL = MaskUtilities.RequiresMaskingForSQL(log);
      this.Message = MaskUtilities.MaskPII(log.GetLogMessage(), maskSQL);
      this.AdditionalFields = log.GetAdditionalFields();
      if (log.Error != null)
        this.Exception = (ILogErrorData) new LogErrorData(log.Error, maskSQL);
      this.ThreadId = new int?(log.ThreadId);
      this.TimeStamp = log.GetLogTime();
      this.Switch = Log.GetLogSwitch(log);
      this.LogLevel = Log.GetLogLevel(LogLevelExtensions.GetBaseType(log.Level));
      this.Source = log.Src;
    }

    public string Message { get; }

    public DateTime TimeStamp { get; }

    public Dictionary<string, object> AdditionalFields { get; }

    public ILogErrorData Exception { get; }

    public int? ThreadId { get; }

    public LogLevel LogLevel { get; }

    public string Source { get; }

    public string Switch { get; }

    internal static LogLevel GetLogLevel(Encompass.Diagnostics.Logging.LogLevel level)
    {
      if (level <= 4)
      {
        if (level == 2)
          return LogLevel.Error;
        if (level == 4)
          return LogLevel.Warning;
      }
      else
      {
        if (level == 8)
          return LogLevel.Information;
        if (level == 16)
          return LogLevel.Verbose;
      }
      return LogLevel.Off;
    }

    internal static string GetLogSwitch(Log log) => log.Logger;
  }
}
