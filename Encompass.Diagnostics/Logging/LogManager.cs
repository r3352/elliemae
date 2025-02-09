// Decompiled with JetBrains decompiler
// Type: Encompass.Diagnostics.Logging.LogManager
// Assembly: Encompass.Diagnostics, Version=1.0.0.1, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E8A3B074-7BF0-4187-B0D2-083265232A16
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Encompass.Diagnostics.dll

using Encompass.Diagnostics.Logging.Schema;
using System;
using System.Diagnostics;
using System.Net;
using System.Threading;

#nullable disable
namespace Encompass.Diagnostics.Logging
{
  public class LogManager : ILogManager
  {
    private ILoggerScopeProvider _loggerScopeProvider;

    public LogManager(string appName, string version, string envName)
    {
      this.AppName = ArgumentChecks.IsNotNullOrEmpty(appName, nameof (appName));
      this.Version = ArgumentChecks.IsNotNullOrEmpty(version, nameof (version));
      this.EnvironmentName = envName;
      this.ComputerName = Dns.GetHostEntry("").HostName;
    }

    public string AppName { get; }

    public string Version { get; }

    public string EnvironmentName { get; }

    public string ComputerName { get; }

    public void SetLoggerScopeProvider(ILoggerScopeProvider loggerScopeProvider)
    {
      this._loggerScopeProvider = ArgumentChecks.IsNotNull<ILoggerScopeProvider>(loggerScopeProvider, nameof (loggerScopeProvider));
    }

    public void WriteLog(Encompass.Diagnostics.Logging.Schema.Log log, bool useGlobalScope)
    {
      if (this._loggerScopeProvider == null)
        return;
      ILoggerScope scope = useGlobalScope ? this._loggerScopeProvider.GetGlobal() : this._loggerScopeProvider.GetCurrent();
      if (scope == null)
        return;
      this.PrepareLog(log, scope);
      scope.WriteToLogTargets(log);
    }

    private void PrepareLog(Encompass.Diagnostics.Logging.Schema.Log log, ILoggerScope scope)
    {
      if (log.SkipPreparationBeforeWrite)
        return;
      log.AppName = this.AppName;
      if (!string.IsNullOrEmpty(this.EnvironmentName))
        log.Environment = this.EnvironmentName;
      log.Version = this.Version;
      log.Host = this.ComputerName;
      if (scope != null)
      {
        log.InstanceId = scope.Instance;
        log.CorrelationId = scope.CorrelationId;
        log.TransactionId = (TransactionId) scope.TransactionId;
      }
      if (string.IsNullOrEmpty(log.CorrelationId))
        log.CorrelationId = Guid.NewGuid().ToString();
      if (log.TransactionId == null || Guid.Empty.Equals(log.TransactionId.Id))
        log.TransactionId = (TransactionId) Guid.NewGuid();
      log.Pid = Process.GetCurrentProcess().Id;
      log.ThreadId = Thread.CurrentThread.ManagedThreadId;
      log.Timestamp = DateTime.UtcNow;
    }

    public bool HasTargetsEnabledFor(string logger, LogLevel logLevel, bool useGlobalScope)
    {
      if (this._loggerScopeProvider == null)
        return false;
      ILoggerScope loggerScope = useGlobalScope ? this._loggerScopeProvider.GetGlobal() : this._loggerScopeProvider.GetCurrent();
      if (loggerScope == null)
        return false;
      return loggerScope.IsActiveFor(new Encompass.Diagnostics.Logging.Schema.Log()
      {
        Level = logLevel,
        Logger = logger
      });
    }

    public ILogger GetLogger() => this.GetLogger((string) null);

    public ILogger GetLogger(string logger) => this.GetLogger(logger, false);

    public ILogger GetLogger(string logger, bool forGlobalScope)
    {
      return (ILogger) new Logger((ILogManager) this, logger, forGlobalScope);
    }
  }
}
