// Decompiled with JetBrains decompiler
// Type: Encompass.Diagnostics.Logging.Targets.TraceLogTarget
// Assembly: Encompass.Diagnostics, Version=1.0.0.1, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E8A3B074-7BF0-4187-B0D2-083265232A16
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Encompass.Diagnostics.dll

using Encompass.Diagnostics.Logging.Formatters;
using System;
using System.Diagnostics;

#nullable disable
namespace Encompass.Diagnostics.Logging.Targets
{
  public class TraceLogTarget : ILogTarget, IDisposable
  {
    private readonly ILogFormatter _logFormatter;

    public TraceLogTarget(ILogFormatter logFormatter)
    {
      this._logFormatter = ArgumentChecks.IsNotNull<ILogFormatter>(logFormatter, nameof (logFormatter));
      Trace.AutoFlush = true;
    }

    public void Dispose()
    {
    }

    public void Flush() => Trace.Flush();

    public void Write(Encompass.Diagnostics.Logging.Schema.Log log)
    {
      if (!log.SupportsFormatter(this._logFormatter.GetFormat()))
        return;
      Trace.WriteLine(this._logFormatter.FormatLog(log));
    }
  }
}
