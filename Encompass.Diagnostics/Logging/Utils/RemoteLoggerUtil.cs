// Decompiled with JetBrains decompiler
// Type: Encompass.Diagnostics.Logging.Utils.RemoteLoggerUtil
// Assembly: Encompass.Diagnostics, Version=1.0.0.1, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E8A3B074-7BF0-4187-B0D2-083265232A16
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Encompass.Diagnostics.dll

using System;
using System.Diagnostics;

#nullable disable
namespace Encompass.Diagnostics.Logging.Utils
{
  public class RemoteLoggerUtil
  {
    public const string RemoteLoggerName = "LegacyRemoteLogger";
    private readonly ILoggerScopeProvider _loggerScopeProvider;
    private readonly ILogManager _logManager;

    public RemoteLoggerUtil(ILoggerScopeProvider loggerScopeProvider, ILogManager logManager)
    {
      this._loggerScopeProvider = loggerScopeProvider;
      this._logManager = logManager;
    }

    public void WriteLog(TraceLevel level, string message)
    {
      using (this._loggerScopeProvider.EnterNew((Action<ILoggerScopeBuilder>) (scope => scope.SetCorrelationId(Guid.NewGuid().ToString()))))
        this._logManager.GetLogger("LegacyRemoteLogger").Write(level.ToLogLevel().Force(), "RemoteLogger", message);
    }

    public void WriteLog(TraceLevel level, string src, string message, Exception ex)
    {
      using (this._loggerScopeProvider.EnterNew((Action<ILoggerScopeBuilder>) (scope => scope.SetCorrelationId(Guid.NewGuid().ToString()))))
        this._logManager.GetLogger("LegacyRemoteLogger").Write(level.ToLogLevel().Force(), src, message, ex);
    }

    public void WriteLog(Exception ex)
    {
      using (this._loggerScopeProvider.EnterNew((Action<ILoggerScopeBuilder>) (scope => scope.SetCorrelationId(Guid.NewGuid().ToString()))))
        this._logManager.GetLogger("LegacyRemoteLogger").Write(Encompass.Diagnostics.Logging.LogLevel.ERROR.Force(), "", ex);
    }
  }
}
