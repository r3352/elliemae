// Decompiled with JetBrains decompiler
// Type: Encompass.Diagnostics.Metrics.LogMetricRecorder
// Assembly: Encompass.Diagnostics, Version=1.0.0.1, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E8A3B074-7BF0-4187-B0D2-083265232A16
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Encompass.Diagnostics.dll

using Encompass.Diagnostics.Logging.Schema;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Encompass.Diagnostics.Metrics
{
  public class LogMetricRecorder : IMetricRecorder, IDisposable
  {
    private readonly Dictionary<string, MetricTimer> _timers = new Dictionary<string, MetricTimer>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, int> _counters = new Dictionary<string, int>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
    private readonly LogFields _logFields;

    public LogMetricRecorder(LogFields logFields) => this._logFields = logFields;

    public IDisposable StartTimer(string activity)
    {
      MetricTimer metricTimer;
      if (!this._timers.TryGetValue(activity, out metricTimer))
        metricTimer = this._timers[activity] = new MetricTimer();
      metricTimer.Resume();
      return (IDisposable) metricTimer;
    }

    public void ResetTimer(string activity)
    {
      MetricTimer metricTimer;
      if (!this._timers.TryGetValue(activity, out metricTimer))
        return;
      metricTimer.Dispose();
      this._timers.Remove(activity);
    }

    public void IncrementCount(string counter) => this.IncrementCount(counter, 1);

    public void IncrementCount(string counter, int incrementBy)
    {
      if (!this._counters.ContainsKey(counter))
        this._counters[counter] = incrementBy;
      else
        this._counters[counter] += incrementBy;
    }

    public void SetCount(string counter, int count) => this._counters[counter] = count;

    public void ResetCount(string counter) => this._timers.Remove(counter);

    private RecorderState GetState()
    {
      RecorderState state = (RecorderState) null;
      if (this._counters.Any<KeyValuePair<string, int>>())
      {
        state = new RecorderState();
        state.Counters = new Dictionary<string, int>((IDictionary<string, int>) this._counters);
      }
      if (this._timers.Any<KeyValuePair<string, MetricTimer>>())
      {
        state = state ?? new RecorderState();
        state.Timers = this._timers.ToDictionary<KeyValuePair<string, MetricTimer>, string, TimerState>((Func<KeyValuePair<string, MetricTimer>, string>) (entry => entry.Key), (Func<KeyValuePair<string, MetricTimer>, TimerState>) (entry =>
        {
          MetricTimer metricTimer = entry.Value;
          metricTimer.Dispose();
          return new TimerState()
          {
            CallCount = metricTimer.CallCount,
            TotalDurationMS = metricTimer.TotalDurationMS
          };
        }));
      }
      return state;
    }

    protected void Clear()
    {
      this._timers.Clear();
      this._counters.Clear();
    }

    public virtual void Publish()
    {
      RecorderState state = this.GetState();
      if (state == null)
        return;
      this._logFields.Set<RecorderState>(LogMetricRecorder.LogFieldNames.Metrics, state);
      this.Clear();
    }

    public void Dispose()
    {
    }

    private static class LogFieldNames
    {
      public static readonly LogFieldName<RecorderState> Metrics = LogFields.Field<RecorderState>("metrics");
    }
  }
}
