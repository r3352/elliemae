// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ContextTraceLog
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Encompass.Diagnostics;
using Encompass.Diagnostics.Logging;
using System;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public sealed class ContextTraceLog : EllieMae.EMLite.ClientServer.IContextTraceLog, IDisposable
  {
    private ClientContext context;
    private readonly ILogger _logger;

    public ContextTraceLog(ClientContext context)
      : this(context, DiagUtility.LogManager.GetLogger((string) null, context == null))
    {
    }

    public ContextTraceLog(ClientContext context, ILogger logger)
    {
      this.context = context;
      this._logger = logger;
    }

    public void Dispose()
    {
    }

    public void Write(TraceLevel level, string className, string message)
    {
      this._logger.Write(level.ToLogLevel(), className, message);
    }

    public void Write(Encompass.Diagnostics.Logging.LogLevel level, string className, string message)
    {
      this._logger.Write(level, className, message);
    }

    public void WriteException(TraceLevel level, string className, Exception ex)
    {
      this._logger.Write(level.ToLogLevel(), className, ex);
    }

    public void WriteException(string className, Exception ex)
    {
      this.WriteException(TraceLevel.Error, className, ex);
    }

    public void WriteInfo(string className, string message)
    {
      this.Write(TraceLevel.Info, className, message);
    }

    public void WriteVerbose(string className, string message)
    {
      this.Write(TraceLevel.Verbose, className, message);
    }

    public void WriteWarning(string className, string message)
    {
      this.Write(TraceLevel.Warning, className, message);
    }

    public void WriteError(string className, string message)
    {
      this.Write(TraceLevel.Error, className, message);
    }

    public void WriteDebug(string className, string message)
    {
      if (!EllieMae.EMLite.DataAccess.ServerGlobals.Debug)
        return;
      this.Write(TraceLevel.Verbose, className, message);
    }

    public int GetErrorCount() => 0;

    public string[] GetErrors() => new string[0];

    public void ClearErrors()
    {
    }

    private void cacheError(string text)
    {
    }
  }
}
