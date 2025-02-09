// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Diagnostics.ClassicLogTarget
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using Encompass.Diagnostics.Logging.Formatters;
using Encompass.Diagnostics.Logging.Schema;
using Encompass.Diagnostics.Logging.Targets;
using System;

#nullable disable
namespace EllieMae.EMLite.Common.Diagnostics
{
  public class ClassicLogTarget : ILogTarget, IDisposable
  {
    private readonly ILogTarget _innerTraceLogTarget;

    public static event ClassicLogTarget.WriteLogHandler WriteLog;

    public ClassicLogTarget()
    {
      this._innerTraceLogTarget = (ILogTarget) new TraceLogTarget((ILogFormatter) new PlainTextFormatter());
    }

    public void Dispose() => this._innerTraceLogTarget.Dispose();

    public void Flush() => this._innerTraceLogTarget.Flush();

    public void Write(Log log)
    {
      bool flag;
      try
      {
        ClassicLogTarget.WriteLogHandler writeLog = ClassicLogTarget.WriteLog;
        flag = writeLog == null || writeLog(log);
      }
      catch
      {
        flag = false;
      }
      if (!flag)
        return;
      this._innerTraceLogTarget.Write(log);
    }

    public delegate bool WriteLogHandler(Log log);
  }
}
