// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Deferrables.CircuitBreakerUtil.LoanFileSnapshotCircuitBreaker
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
  internal class LoanFileSnapshotCircuitBreaker : MetricsCircuitBreakerBase
  {
    private static LoanFileSnapshotCircuitBreaker _instance = new LoanFileSnapshotCircuitBreaker();

    private LoanFileSnapshotCircuitBreaker()
    {
      this._className = nameof (LoanFileSnapshotCircuitBreaker);
      this._actionName = "take deferrable snapshot";
    }

    public static LoanFileSnapshotCircuitBreaker Instance
    {
      get => LoanFileSnapshotCircuitBreaker._instance;
    }

    protected override CircuitBreakerPolicy CreateBreaker()
    {
      return Policy.Handle<HighLatencyException>().CircuitBreaker(1, TimeSpan.FromSeconds(10.0), new Action<Exception, TimeSpan>(this.OnBreak), new Action(this.OnReset));
    }

    protected override IDisposable GetMetricsTimer(ILoanMetricsRecorder recorder)
    {
      return recorder.RecordLoanDeferrableSnapshotTime();
    }

    protected override double GetMetricsMedianDuration(ILoanMetricsRecorder recorder)
    {
      return recorder.GetLoanDeferrableSnapshotTimeMedian();
    }

    protected override double GetMetricsLastDuration(ILoanMetricsRecorder recorder)
    {
      return recorder.GetLoanDeferrableSnapshotTimeLast();
    }

    private void OnBreak(Exception exception, TimeSpan timespan)
    {
      TraceLog.WriteError(this._className, string.Format("Failed to copy a file.  Circuite breaker is open and not allowing calls. {0}", (object) exception));
    }

    private void OnReset()
    {
      TraceLog.WriteInfo(this._className, "Take snapshot circuit breaker is reset. The circuit breaker is in a closed state and allowing calls.");
    }
  }
}
