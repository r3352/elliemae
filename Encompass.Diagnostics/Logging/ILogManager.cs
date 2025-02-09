// Decompiled with JetBrains decompiler
// Type: Encompass.Diagnostics.Logging.ILogManager
// Assembly: Encompass.Diagnostics, Version=1.0.0.1, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E8A3B074-7BF0-4187-B0D2-083265232A16
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Encompass.Diagnostics.dll

using Encompass.Diagnostics.Logging.Schema;

#nullable disable
namespace Encompass.Diagnostics.Logging
{
  public interface ILogManager
  {
    string AppName { get; }

    string EnvironmentName { get; }

    string Version { get; }

    string ComputerName { get; }

    void SetLoggerScopeProvider(ILoggerScopeProvider loggerScopeProvider);

    void WriteLog(Log log, bool useGlobalScope);

    bool HasTargetsEnabledFor(string logger, LogLevel eventType, bool useGlobalScope);

    ILogger GetLogger();

    ILogger GetLogger(string logger);

    ILogger GetLogger(string logger, bool forGlobalScope);
  }
}
