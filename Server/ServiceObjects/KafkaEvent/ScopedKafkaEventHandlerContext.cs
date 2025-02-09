// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServiceObjects.KafkaEvent.ScopedKafkaEventHandlerContext
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using Encompass.Diagnostics;
using Encompass.Diagnostics.Logging;
using Encompass.Diagnostics.Logging.Schema;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Server.ServiceObjects.KafkaEvent
{
  public class ScopedKafkaEventHandlerContext : IKafkaEventHandlerContext, IDisposable
  {
    internal readonly ScopedKafkaEventHandlerContext _previous;
    private readonly List<KafkaEventLog> _logs = new List<KafkaEventLog>();
    private readonly ILogger _logger;
    private readonly IClientContext _clientContext;
    private readonly string _correlationId;
    private readonly Guid? _transactionId;
    private readonly object _syncLock = new object();
    private int _handlersAdded;
    private int _handlersCompleted;
    private bool _disposed;
    private bool _logWritten;

    public ScopedKafkaEventHandlerContext(
      ScopedKafkaEventHandlerContext previous,
      IClientContext clientContext,
      string correlationId,
      Guid? transactionId)
    {
      this._previous = previous;
      this._clientContext = clientContext;
      this._correlationId = correlationId;
      this._transactionId = transactionId;
      this._logger = DiagUtility.LogManager.GetLogger("Kafka.Messages");
    }

    public void Add(KafkaEventResponseHandler handler)
    {
      lock (this._syncLock)
      {
        if (this._disposed)
          throw new Exception("Context is already disposed. Handlers cannot be added at this point.");
        ++this._handlersAdded;
      }
    }

    public void Complete(KafkaEventResponseHandler handler, KafkaEventLog log)
    {
      lock (this._syncLock)
      {
        ++this._handlersCompleted;
        if (log != null)
          this._logs.Add(log);
        if (!this._disposed || this._handlersAdded != this._handlersCompleted)
          return;
        this.WriteAggregatedLog();
      }
    }

    private void WriteAggregatedLog()
    {
      if (this._logWritten)
        return;
      this._logWritten = true;
      KafkaEventsAggregatedLog.EventData[] events = new KafkaEventsAggregatedLog.EventData[this._logs.Count];
      string str1 = (string) null;
      string str2 = (string) null;
      string str3 = (string) null;
      bool flag = false;
      for (int index = 0; index < this._logs.Count; ++index)
      {
        KafkaEventLog log = this._logs[index];
        if (log != null)
        {
          string tvalue1;
          if (string.IsNullOrEmpty(str1) && log.TryGet<string>(Log.CommonFields.LoanId, out tvalue1))
            str1 = tvalue1;
          string tvalue2;
          if (string.IsNullOrEmpty(str2) && log.TryGet<string>(Log.CommonFields.LoanFilePath, out tvalue2))
            str2 = tvalue2;
          string tvalue3;
          if (string.IsNullOrEmpty(str3) && log.TryGet<string>(KafkaEventLog.Fields.Source, out tvalue3))
            str3 = tvalue3;
          KafkaEventsAggregatedLog.EventData eventData = new KafkaEventsAggregatedLog.EventData(log);
          if (!eventData.Success)
            flag = true;
          events[index] = eventData;
        }
      }
      KafkaEventsAggregatedLog log1 = new KafkaEventsAggregatedLog(events);
      if (!string.IsNullOrEmpty(str1))
        log1.Set<string>(Log.CommonFields.LoanId, str1);
      if (!string.IsNullOrEmpty(str2))
        log1.Set<string>(Log.CommonFields.LoanFilePath, str2);
      if (!string.IsNullOrEmpty(str3))
        log1.Set<string>(KafkaEventLog.Fields.Source, str3);
      log1.Level = flag ? Encompass.Diagnostics.Logging.LogLevel.WARN : Encompass.Diagnostics.Logging.LogLevel.INFO;
      log1.Src = "KafkaEventResponseHandler";
      using (this._clientContext.MakeCurrent(correlationId: this._correlationId, transactionId: this._transactionId))
        this._logger.Write<KafkaEventsAggregatedLog>(log1);
    }

    public bool IsInfoLogEnabled() => this._logger.IsEnabled(Encompass.Diagnostics.Logging.LogLevel.INFO);

    public void Dispose()
    {
      lock (this._syncLock)
      {
        KafkaEventHandlerContextFactory.Instance.ExitScope(this);
        this._disposed = true;
        if (this._handlersAdded != this._handlersCompleted)
          return;
        this.WriteAggregatedLog();
      }
    }
  }
}
