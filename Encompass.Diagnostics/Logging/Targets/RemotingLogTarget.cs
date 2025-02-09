// Decompiled with JetBrains decompiler
// Type: Encompass.Diagnostics.Logging.Targets.RemotingLogTarget
// Assembly: Encompass.Diagnostics, Version=1.0.0.1, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E8A3B074-7BF0-4187-B0D2-083265232A16
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Encompass.Diagnostics.dll

using Encompass.Diagnostics.Logging.Schema;
using System;
using System.Collections.Generic;

#nullable disable
namespace Encompass.Diagnostics.Logging.Targets
{
  public class RemotingLogTarget : ILogTarget, IDisposable
  {
    private IRemotingLogConsumer _remotingLogConsumer;
    private readonly List<Log> _logs;

    public RemotingLogTarget(IRemotingLogConsumer remotingLogConsumer)
    {
      this._remotingLogConsumer = remotingLogConsumer;
      this._logs = new List<Log>();
    }

    public void Write(Log log)
    {
      lock (this)
        this._logs.Add(log);
    }

    public void Flush()
    {
      List<Log> clientLogs;
      lock (this)
      {
        clientLogs = new List<Log>((IEnumerable<Log>) this._logs);
        this._logs.Clear();
      }
      this._remotingLogConsumer.WriteLogs((IEnumerable<Log>) clientLogs);
    }

    public void Dispose()
    {
    }
  }
}
