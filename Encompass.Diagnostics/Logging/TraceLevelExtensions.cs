// Decompiled with JetBrains decompiler
// Type: Encompass.Diagnostics.Logging.TraceLevelExtensions
// Assembly: Encompass.Diagnostics, Version=1.0.0.1, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E8A3B074-7BF0-4187-B0D2-083265232A16
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Encompass.Diagnostics.dll

using System.Diagnostics;

#nullable disable
namespace Encompass.Diagnostics.Logging
{
  public static class TraceLevelExtensions
  {
    public static LogLevel ToLogLevel(this TraceLevel level)
    {
      switch (level)
      {
        case TraceLevel.Off:
          return LogLevel.None;
        case TraceLevel.Error:
          return LogLevel.ERROR;
        case TraceLevel.Warning:
          return LogLevel.WARN;
        case TraceLevel.Info:
          return LogLevel.INFO;
        case TraceLevel.Verbose:
          return LogLevel.DEBUG;
        default:
          return LogLevel.ERROR;
      }
    }

    public static LogLevelFilter ToLogLevelFilter(this TraceLevel level)
    {
      switch (level)
      {
        case TraceLevel.Off:
          return LogLevelFilter.Off;
        case TraceLevel.Error:
          return LogLevelFilter.Error;
        case TraceLevel.Warning:
          return LogLevelFilter.Warning;
        case TraceLevel.Info:
          return LogLevelFilter.Information;
        case TraceLevel.Verbose:
          return LogLevelFilter.Verbose;
        default:
          return LogLevelFilter.Error;
      }
    }
  }
}
