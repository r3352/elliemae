// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Deferrables.CircuitBreakerUtil.LoanEventPublisherCircuitBreaker
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Elli.Metrics;
using Polly;
using Polly.CircuitBreaker;
using System;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.Deferrables.CircuitBreakerUtil
{
  internal class LoanEventPublisherCircuitBreaker : MetricsCircuitBreakerBase
  {
    private static LoanEventPublisherCircuitBreaker _instance = new LoanEventPublisherCircuitBreaker();

    private LoanEventPublisherCircuitBreaker()
    {
      this._className = nameof (LoanEventPublisherCircuitBreaker);
      this._actionName = "publish loan event";
    }

    public static LoanEventPublisherCircuitBreaker Instance
    {
      get => LoanEventPublisherCircuitBreaker._instance;
    }

    protected override CircuitBreakerPolicy CreateBreaker()
    {
      int serverSetting = (int) ClientContext.GetCurrent().Settings.GetServerSetting("Policies.LoanEventPublishCircuitResetMs");
      return Policy.Handle<HighLatencyException>().CircuitBreaker(1, TimeSpan.FromMilliseconds((double) serverSetting), new Action<Exception, TimeSpan>(this.OnBreak), new Action(this.OnReset));
    }

    protected override IDisposable GetMetricsTimer(ILoanMetricsRecorder recorder)
    {
      return recorder.RecordLoanEventPublishTime();
    }

    protected override double GetMetricsMedianDuration(ILoanMetricsRecorder recorder)
    {
      return recorder.GetLoanEventPublishTimeMedian();
    }

    protected override double GetMetricsLastDuration(ILoanMetricsRecorder recorder)
    {
      return recorder.GetLoanEventPublishTimeLast();
    }

    private void OnBreak(Exception exception, TimeSpan timespan)
    {
      TraceLog.WriteError(this._className, string.Format("Failed to publish loan event.  Circuite breaker is open and not allowing calls.  {0}", (object) exception.Message));
    }

    private void OnReset()
    {
      TraceLog.WriteInfo(this._className, "Publish loan event circuit breaker is reset.  The circuit breaker is in a closed state and allowing calls.");
    }
  }
}
