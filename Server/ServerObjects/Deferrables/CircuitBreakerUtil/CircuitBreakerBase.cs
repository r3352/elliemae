// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Deferrables.CircuitBreakerUtil.CircuitBreakerBase
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Elli.Metrics;
using Polly.CircuitBreaker;
using System;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.Deferrables.CircuitBreakerUtil
{
  public abstract class CircuitBreakerBase
  {
    private object _breakerLock = new object();
    protected CircuitBreakerPolicy _breaker;
    private object _stopwatchLock = new object();
    private Stopwatch _stopwatch = new Stopwatch();
    protected string _className = nameof (CircuitBreakerBase);
    protected string _actionName = "act";
    protected double _latencyThreashold = 1000.0;
    protected long _latencyIntervalMs = 10000;

    protected abstract CircuitBreakerPolicy CreateBreaker();

    public virtual bool Execute(Action action, string logData)
    {
      bool bRC = false;
      try
      {
        this.GetBreaker();
        this._breaker.Execute((Action) (() =>
        {
          DateTime now = DateTime.Now;
          action();
          this.LatencyCheck((DateTime.Now - now).TotalMilliseconds, this._latencyThreashold, this._latencyIntervalMs);
          bRC = true;
        }));
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

    protected CircuitBreakerPolicy GetBreaker()
    {
      if (this._breaker != null)
        return this._breaker;
      lock (this._breakerLock)
      {
        if (this._breaker == null)
          this._breaker = this.CreateBreaker();
        return this._breaker;
      }
    }

    protected void LatencyCheck(
      double currentLatency,
      double latencyThreashold,
      long latencyIntervalMs)
    {
      if (currentLatency > latencyThreashold)
      {
        if (this._stopwatch.IsRunning)
        {
          if (this._stopwatch.ElapsedMilliseconds > latencyIntervalMs)
            throw new HighLatencyException(currentLatency, latencyThreashold, latencyIntervalMs);
        }
        else
        {
          lock (this._stopwatchLock)
            this._stopwatch.Restart();
        }
      }
      else
      {
        if (!this._stopwatch.IsRunning)
          return;
        lock (this._stopwatchLock)
          this._stopwatch.Reset();
      }
    }

    protected virtual void OnHighLatencyException(string logData, HighLatencyException exception)
    {
      TraceLog.WriteError(this._className, string.Format("Failed to {0} due to high latency. {1}, {2}", (object) this._actionName, (object) logData, (object) exception.Message));
    }

    protected virtual void OnBrokenCircuitException(
      string logData,
      BrokenCircuitException exception)
    {
      TraceLog.WriteError(this._className, string.Format("Failed to {0} - circuit is open. Will reset circuit when latency falls below threashold. {1}", (object) this._actionName, (object) logData));
    }

    protected virtual void OnException(string logData, Exception exception)
    {
      TraceLog.WriteError(this._className, string.Format("Failed to {0}. {1}, {2}", (object) this._actionName, (object) logData, (object) exception));
    }

    public CircuitState BreakerStatus
    {
      get => this._breaker != null ? this._breaker.CircuitState : CircuitState.Closed;
    }
  }
}
