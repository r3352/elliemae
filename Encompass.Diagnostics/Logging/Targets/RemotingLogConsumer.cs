// Decompiled with JetBrains decompiler
// Type: Encompass.Diagnostics.Logging.Targets.RemotingLogConsumer
// Assembly: Encompass.Diagnostics, Version=1.0.0.1, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E8A3B074-7BF0-4187-B0D2-083265232A16
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Encompass.Diagnostics.dll

using Encompass.Diagnostics.Logging.Schema;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Encompass.Diagnostics.Logging.Targets
{
  public class RemotingLogConsumer : IRemotingLogConsumer
  {
    private readonly ILogger _logger;
    private readonly string _clientId;
    private readonly string _userId;
    private readonly string _sessionId;

    public RemotingLogConsumer(ILogger logger, string clientId, string userId, string sessionId)
    {
      this._logger = logger;
      this._clientId = clientId;
      this._userId = userId;
      this._sessionId = sessionId;
    }

    public void WriteLogs(IEnumerable<Log> logs)
    {
      foreach (RemotingClientLog log in logs.Select<Log, RemotingClientLog>((Func<Log, RemotingClientLog>) (log => new RemotingClientLog(log))))
      {
        try
        {
          log.CustomerId = this._clientId;
          log.UserId = this._userId;
          log.SessionId = this._sessionId;
          this._logger.Write<RemotingClientLog>(log);
        }
        catch (Exception ex)
        {
          this._logger.Write(Encompass.Diagnostics.Logging.LogLevel.INFO, nameof (RemotingLogConsumer), "Error writing client message: " + ex.GetFullStackTrace());
        }
      }
    }
  }
}
