// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Deferrables.CircuitBreakerUtil.MetricsCircuitBreakerBase
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Elli.Metrics;
using Polly.CircuitBreaker;
using System;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.Deferrables.CircuitBreakerUtil
{
  public abstract class MetricsCircuitBreakerBase : CircuitBreakerBase
  {
    public override bool Execute(Action action, string logData)
    {
      bool bRC = false;
      new MetricsFactory().CreateServerEventMetricsRecorder();
      try
      {
        ILoanMetricsRecorder recorder = this.CreateMetricsRecorder();
        using (this.GetMetricsTimer(recorder))
        {
          this.GetBreaker();
          this._breaker.Execute((Action) (() =>
          {
            action();
            this.LatencyCheck(this._breaker.CircuitState == CircuitState.Closed ? this.GetMetricsMedianDuration(recorder) : this.GetMetricsLastDuration(recorder), this._latencyThreashold, this._latencyIntervalMs);
            bRC = true;
          }));
        }
      }
      catch (HighLatencyException ex)
      {
        this.OnHighLatencyException(logData, ex);
      }
      catch (BrokenCircuitException ex)
      {
        this.OnBrokenCircuitException(logData, ex);
      }
      catch (Exception ex)
      {
        this.OnException(logData, ex);
      }
      return bRC;
    }

    protected virtual ILoanMetricsRecorder CreateMetricsRecorder()
    {
      ClientContext current = ClientContext.GetCurrent();
      return new MetricsFactory().CreateLoanMetricsRecorder(current.ClientID, current.InstanceName);
    }

    protected abstract IDisposable GetMetricsTimer(ILoanMetricsRecorder recorder);

    protected abstract double GetMetricsMedianDuration(ILoanMetricsRecorder recorder);

    protected abstract double GetMetricsLastDuration(ILoanMetricsRecorder recorder);
  }
}
