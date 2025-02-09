// Decompiled with JetBrains decompiler
// Type: Encompass.Diagnostics.Logging.Logger
// Assembly: Encompass.Diagnostics, Version=1.0.0.1, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E8A3B074-7BF0-4187-B0D2-083265232A16
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Encompass.Diagnostics.dll

using Encompass.Diagnostics.Logging.Schema;
using System;

#nullable disable
namespace Encompass.Diagnostics.Logging
{
  public class Logger : ILogger
  {
    private readonly ILogManager _logManager;
    private readonly string _name;
    private readonly bool _globalScope;

    public Logger(ILogManager logManager, string loggerName, bool global)
    {
      this._logManager = ArgumentChecks.IsNotNull<ILogManager>(logManager, nameof (logManager));
      this._name = loggerName;
      this._globalScope = global;
    }

    public bool IsEnabled(LogLevel level)
    {
      return this._logManager.HasTargetsEnabledFor(this._name, level, this._globalScope);
    }

    public void Write(LogLevel level, string src, string message, Exception ex, LogFields info)
    {
      Log log = new Log();
      log.Level = level;
      log.Src = src;
      log.Message = message;
      if (ex != null)
        log.Error = new LogErrorData(ex);
      info?.MapTo((LogFields) log);
      this.Write<Log>(log);
    }

    public void Write<TExtendedLog>(TExtendedLog log) where TExtendedLog : Log
    {
      log.Logger = this._name;
      this._logManager.WriteLog((Log) log, this._globalScope);
    }

    public void Write(LogLevel level, string src, string message)
    {
      this.Write(level, src, message, (Exception) null, (LogFields) null);
    }

    public void Write(LogLevel level, string src, Exception ex)
    {
      this.Write(level, src, (string) null, ex, (LogFields) null);
    }

    public void Write(LogLevel level, string src, string message, Exception ex)
    {
      this.Write(level, src, message, ex, (LogFields) null);
    }

    public void Write(LogLevel level, string src, string message, LogFields info)
    {
      this.Write(level, src, message, (Exception) null, info);
    }

    public void When(LogLevel level, Action action)
    {
      if (!this.IsEnabled(level) || action == null)
        return;
      action();
    }
  }
}
