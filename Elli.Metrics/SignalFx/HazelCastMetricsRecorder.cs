// Decompiled with JetBrains decompiler
// Type: Elli.Metrics.SignalFx.HazelCastMetricsRecorder
// Assembly: Elli.Metrics, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D16DD1E4-F07E-4CB7-B2D0-A2DD51E6F671
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Metrics.dll

using Metrics;
using Metrics.Core;
using Metrics.MetricData;
using System;
using System.Linq;

#nullable disable
namespace Elli.Metrics.SignalFx
{
  public class HazelCastMetricsRecorder : IHazelCastMetricsRecorder
  {
    private const string HazelCastConnectionTimeName = "HazelCastConnectionCallIncTimer";
    private const string HazelCastGetTimeName = "HazelCastGetCallIncTimer";
    private const string HazelCastRemoveTimeName = "HazelCastRemoveCallIncTimer";
    private const string HazelCastKeyTimeName = "HazelCastKeyCallIncTimer";
    private const string HazelCastUnlockTimeName = "HazelCastUnlockCallIncTimer";
    private const string HazelCastGetEntryViewTimeName = "HazelCastGetEntryViewCallIncTimer";
    private const string HazelCastPutTimeName = "HazelCastPutCallIncTimer";
    private const string HazelCastLockTimeName = "HazelCastLockCallIncTimer";

    public void IncrementErrorCount(string errorType, string instance, string version)
    {
      Metric.Context(Constants.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName))).Counter("HazelCastErrorCounter", Unit.Items, new MetricTags(new string[3]
      {
        Constants.ErrorType + errorType,
        Constants.InstanceDimension + instance,
        Constants.PodVersionDimension + version
      })).Increment(1L);
    }

    public void IncrementCircuitStateChangeCount(string state, string instance, string version)
    {
      Metric.Context(Constants.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName))).Counter("HazelCastCircuit" + state, Unit.Items, new MetricTags(new string[2]
      {
        Constants.InstanceDimension + instance,
        Constants.PodVersionDimension + version
      })).Increment(1L);
    }

    public void IncrementHazelCastOperationCount(string operation, string instance, string version)
    {
      Metric.Context(Constants.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName))).Counter("HazelCast" + operation, Unit.Items, new MetricTags(new string[2]
      {
        Constants.InstanceDimension + instance,
        Constants.PodVersionDimension + version
      })).Increment(1L);
    }

    public IDisposable RecordHazelCastConnectionTime(string instance, string version)
    {
      return (IDisposable) ((TaggedMetricsContext) Metric.Context(Constants.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName)))).Timer("HazelCastConnectionCallIncTimer", Unit.Requests, SamplingType.FavourRecent, TimeUnit.Seconds, TimeUnit.Milliseconds, new MetricTags(new string[2]
      {
        Constants.InstanceDimension + instance,
        Constants.PodVersionDimension + version
      })).NewContext();
    }

    public IDisposable RecordHazelCastGetTime(string instance, string version)
    {
      return (IDisposable) ((TaggedMetricsContext) Metric.Context(Constants.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName)))).Timer("HazelCastGetCallIncTimer", Unit.Requests, SamplingType.FavourRecent, TimeUnit.Seconds, TimeUnit.Milliseconds, new MetricTags(new string[2]
      {
        Constants.InstanceDimension + instance,
        Constants.PodVersionDimension + version
      })).NewContext();
    }

    public IDisposable RecordHazelCastRemoveTime(string instance, string version)
    {
      return (IDisposable) ((TaggedMetricsContext) Metric.Context(Constants.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName)))).Timer("HazelCastRemoveCallIncTimer", Unit.Requests, SamplingType.FavourRecent, TimeUnit.Seconds, TimeUnit.Milliseconds, new MetricTags(new string[2]
      {
        Constants.InstanceDimension + instance,
        Constants.PodVersionDimension + version
      })).NewContext();
    }

    public IDisposable RecordHazelCastKeyTime(string instance, string version)
    {
      return (IDisposable) ((TaggedMetricsContext) Metric.Context(Constants.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName)))).Timer("HazelCastKeyCallIncTimer", Unit.Requests, SamplingType.FavourRecent, TimeUnit.Seconds, TimeUnit.Milliseconds, new MetricTags(new string[2]
      {
        Constants.InstanceDimension + instance,
        Constants.PodVersionDimension + version
      })).NewContext();
    }

    public IDisposable RecordHazelCastUnlockTime(string instance, string version)
    {
      return (IDisposable) ((TaggedMetricsContext) Metric.Context(Constants.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName)))).Timer("HazelCastUnlockCallIncTimer", Unit.Requests, SamplingType.FavourRecent, TimeUnit.Seconds, TimeUnit.Milliseconds, new MetricTags(new string[2]
      {
        Constants.InstanceDimension + instance,
        Constants.PodVersionDimension + version
      })).NewContext();
    }

    public IDisposable RecordHazelCastGetEntryViewTime(string instance, string version)
    {
      return (IDisposable) ((TaggedMetricsContext) Metric.Context(Constants.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName)))).Timer("HazelCastGetEntryViewCallIncTimer", Unit.Requests, SamplingType.FavourRecent, TimeUnit.Seconds, TimeUnit.Milliseconds, new MetricTags(new string[2]
      {
        Constants.InstanceDimension + instance,
        Constants.PodVersionDimension + version
      })).NewContext();
    }

    public IDisposable RecordHazelCastPutTime(string instance, string version)
    {
      return (IDisposable) ((TaggedMetricsContext) Metric.Context(Constants.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName)))).Timer("HazelCastPutCallIncTimer", Unit.Requests, SamplingType.FavourRecent, TimeUnit.Seconds, TimeUnit.Milliseconds, new MetricTags(new string[2]
      {
        Constants.InstanceDimension + instance,
        Constants.PodVersionDimension + version
      })).NewContext();
    }

    public IDisposable RecordHazelCastLockTime(string instance, string version)
    {
      return (IDisposable) ((TaggedMetricsContext) Metric.Context(Constants.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName)))).Timer("HazelCastLockCallIncTimer", Unit.Requests, SamplingType.FavourRecent, TimeUnit.Seconds, TimeUnit.Milliseconds, new MetricTags(new string[2]
      {
        Constants.InstanceDimension + instance,
        Constants.PodVersionDimension + version
      })).NewContext();
    }

    public double GetHazelCastConnectionTimeMedian()
    {
      double connectionTimeMedian = 0.0;
      TimerValueSource timerValueSource = Metric.Context(Constants.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName))).DataProvider.CurrentMetricsData.Timers.FirstOrDefault<TimerValueSource>((Func<TimerValueSource, bool>) (t => t.Name == "HazelCastConnectionCallIncTimer"));
      if (timerValueSource != null)
        connectionTimeMedian = timerValueSource.Value.Histogram.Median;
      return connectionTimeMedian;
    }

    public double GetHazelCastGetTimeMedian()
    {
      double castGetTimeMedian = 0.0;
      TimerValueSource timerValueSource = Metric.Context(Constants.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName))).DataProvider.CurrentMetricsData.Timers.FirstOrDefault<TimerValueSource>((Func<TimerValueSource, bool>) (t => t.Name == "HazelCastGetCallIncTimer"));
      if (timerValueSource != null)
        castGetTimeMedian = timerValueSource.Value.Histogram.Median;
      return castGetTimeMedian;
    }

    public double GetHazelCastRemoveTimeMedian()
    {
      double removeTimeMedian = 0.0;
      TimerValueSource timerValueSource = Metric.Context(Constants.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName))).DataProvider.CurrentMetricsData.Timers.FirstOrDefault<TimerValueSource>((Func<TimerValueSource, bool>) (t => t.Name == "HazelCastRemoveCallIncTimer"));
      if (timerValueSource != null)
        removeTimeMedian = timerValueSource.Value.Histogram.Median;
      return removeTimeMedian;
    }

    public double GetHazelCastKeyTimeMedian()
    {
      double castKeyTimeMedian = 0.0;
      TimerValueSource timerValueSource = Metric.Context(Constants.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName))).DataProvider.CurrentMetricsData.Timers.FirstOrDefault<TimerValueSource>((Func<TimerValueSource, bool>) (t => t.Name == "HazelCastKeyCallIncTimer"));
      if (timerValueSource != null)
        castKeyTimeMedian = timerValueSource.Value.Histogram.Median;
      return castKeyTimeMedian;
    }

    public double GetHazelCastUnlockTimeMedian()
    {
      double unlockTimeMedian = 0.0;
      TimerValueSource timerValueSource = Metric.Context(Constants.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName))).DataProvider.CurrentMetricsData.Timers.FirstOrDefault<TimerValueSource>((Func<TimerValueSource, bool>) (t => t.Name == "HazelCastUnlockCallIncTimer"));
      if (timerValueSource != null)
        unlockTimeMedian = timerValueSource.Value.Histogram.Median;
      return unlockTimeMedian;
    }

    public double GetHazelCastGetEntryViewTimeMedian()
    {
      double entryViewTimeMedian = 0.0;
      TimerValueSource timerValueSource = Metric.Context(Constants.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName))).DataProvider.CurrentMetricsData.Timers.FirstOrDefault<TimerValueSource>((Func<TimerValueSource, bool>) (t => t.Name == "HazelCastGetEntryViewCallIncTimer"));
      if (timerValueSource != null)
        entryViewTimeMedian = timerValueSource.Value.Histogram.Median;
      return entryViewTimeMedian;
    }

    public double GetHazelCastPutTimeMedian()
    {
      double castPutTimeMedian = 0.0;
      TimerValueSource timerValueSource = Metric.Context(Constants.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName))).DataProvider.CurrentMetricsData.Timers.FirstOrDefault<TimerValueSource>((Func<TimerValueSource, bool>) (t => t.Name == "HazelCastPutCallIncTimer"));
      if (timerValueSource != null)
        castPutTimeMedian = timerValueSource.Value.Histogram.Median;
      return castPutTimeMedian;
    }

    public double GetHazelCastLockTimeMedian()
    {
      double castLockTimeMedian = 0.0;
      TimerValueSource timerValueSource = Metric.Context(Constants.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName))).DataProvider.CurrentMetricsData.Timers.FirstOrDefault<TimerValueSource>((Func<TimerValueSource, bool>) (t => t.Name == "HazelCastLockCallIncTimer"));
      if (timerValueSource != null)
        castLockTimeMedian = timerValueSource.Value.Histogram.Median;
      return castLockTimeMedian;
    }

    public void ResetHazelCastConnectionTimer()
    {
      ((TaggedMetricsContext) Metric.Context(Constants.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName)))).Timer("HazelCastConnectionCallIncTimer", Unit.Requests, SamplingType.FavourRecent, TimeUnit.Seconds, TimeUnit.Milliseconds, new MetricTags()).Reset();
    }

    public void ResetHazelCastGetTimer()
    {
      ((TaggedMetricsContext) Metric.Context(Constants.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName)))).Timer("HazelCastGetCallIncTimer", Unit.Requests, SamplingType.FavourRecent, TimeUnit.Seconds, TimeUnit.Milliseconds, new MetricTags()).Reset();
    }

    public void ResetHazelCastRemoveTimer()
    {
      ((TaggedMetricsContext) Metric.Context(Constants.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName)))).Timer("HazelCastRemoveCallIncTimer", Unit.Requests, SamplingType.FavourRecent, TimeUnit.Seconds, TimeUnit.Milliseconds, new MetricTags()).Reset();
    }

    public void ResetHazelCastKeyTimer()
    {
      ((TaggedMetricsContext) Metric.Context(Constants.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName)))).Timer("HazelCastKeyCallIncTimer", Unit.Requests, SamplingType.FavourRecent, TimeUnit.Seconds, TimeUnit.Milliseconds, new MetricTags()).Reset();
    }

    public void ResetHazelCastUnlockTimer()
    {
      ((TaggedMetricsContext) Metric.Context(Constants.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName)))).Timer("HazelCastUnlockCallIncTimer", Unit.Requests, SamplingType.FavourRecent, TimeUnit.Seconds, TimeUnit.Milliseconds, new MetricTags()).Reset();
    }

    public void ResetHazelCastGetEntryViewTimer()
    {
      ((TaggedMetricsContext) Metric.Context(Constants.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName)))).Timer("HazelCastGetEntryViewCallIncTimer", Unit.Requests, SamplingType.FavourRecent, TimeUnit.Seconds, TimeUnit.Milliseconds, new MetricTags()).Reset();
    }

    public void ResetHazelCastPutTimer()
    {
      ((TaggedMetricsContext) Metric.Context(Constants.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName)))).Timer("HazelCastPutCallIncTimer", Unit.Requests, SamplingType.FavourRecent, TimeUnit.Seconds, TimeUnit.Milliseconds, new MetricTags()).Reset();
    }

    public void ResetHazelCastLockTimer()
    {
      ((TaggedMetricsContext) Metric.Context(Constants.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName)))).Timer("HazelCastLockCallIncTimer", Unit.Requests, SamplingType.FavourRecent, TimeUnit.Seconds, TimeUnit.Milliseconds, new MetricTags()).Reset();
    }

    public double GetHazelCastLastConnectionTime()
    {
      double lastConnectionTime = 0.0;
      TimerValueSource timerValueSource = Metric.Context(Constants.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName))).DataProvider.CurrentMetricsData.Timers.FirstOrDefault<TimerValueSource>((Func<TimerValueSource, bool>) (t => t.Name == "HazelCastConnectionCallIncTimer"));
      if (timerValueSource != null)
        lastConnectionTime = timerValueSource.Value.Histogram.LastValue;
      return lastConnectionTime;
    }

    public double GetHazelCastLastGetTime()
    {
      double hazelCastLastGetTime = 0.0;
      TimerValueSource timerValueSource = Metric.Context(Constants.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName))).DataProvider.CurrentMetricsData.Timers.FirstOrDefault<TimerValueSource>((Func<TimerValueSource, bool>) (t => t.Name == "HazelCastGetCallIncTimer"));
      if (timerValueSource != null)
        hazelCastLastGetTime = timerValueSource.Value.Histogram.LastValue;
      return hazelCastLastGetTime;
    }

    public double GetHazelCastLastRemoveTime()
    {
      double castLastRemoveTime = 0.0;
      TimerValueSource timerValueSource = Metric.Context(Constants.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName))).DataProvider.CurrentMetricsData.Timers.FirstOrDefault<TimerValueSource>((Func<TimerValueSource, bool>) (t => t.Name == "HazelCastRemoveCallIncTimer"));
      if (timerValueSource != null)
        castLastRemoveTime = timerValueSource.Value.Histogram.LastValue;
      return castLastRemoveTime;
    }

    public double GetHazelCastLastKeyTime()
    {
      double hazelCastLastKeyTime = 0.0;
      TimerValueSource timerValueSource = Metric.Context(Constants.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName))).DataProvider.CurrentMetricsData.Timers.FirstOrDefault<TimerValueSource>((Func<TimerValueSource, bool>) (t => t.Name == "HazelCastKeyCallIncTimer"));
      if (timerValueSource != null)
        hazelCastLastKeyTime = timerValueSource.Value.Histogram.LastValue;
      return hazelCastLastKeyTime;
    }

    public double GetHazelCastLastUnlockTime()
    {
      double castLastUnlockTime = 0.0;
      TimerValueSource timerValueSource = Metric.Context(Constants.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName))).DataProvider.CurrentMetricsData.Timers.FirstOrDefault<TimerValueSource>((Func<TimerValueSource, bool>) (t => t.Name == "HazelCastUnlockCallIncTimer"));
      if (timerValueSource != null)
        castLastUnlockTime = timerValueSource.Value.Histogram.LastValue;
      return castLastUnlockTime;
    }

    public double GetHazelCastLastGetEntryViewTime()
    {
      double getEntryViewTime = 0.0;
      TimerValueSource timerValueSource = Metric.Context(Constants.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName))).DataProvider.CurrentMetricsData.Timers.FirstOrDefault<TimerValueSource>((Func<TimerValueSource, bool>) (t => t.Name == "HazelCastGetEntryViewCallIncTimer"));
      if (timerValueSource != null)
        getEntryViewTime = timerValueSource.Value.Histogram.LastValue;
      return getEntryViewTime;
    }

    public double GetHazelCastLastPutTime()
    {
      double hazelCastLastPutTime = 0.0;
      TimerValueSource timerValueSource = Metric.Context(Constants.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName))).DataProvider.CurrentMetricsData.Timers.FirstOrDefault<TimerValueSource>((Func<TimerValueSource, bool>) (t => t.Name == "HazelCastPutCallIncTimer"));
      if (timerValueSource != null)
        hazelCastLastPutTime = timerValueSource.Value.Histogram.LastValue;
      return hazelCastLastPutTime;
    }

    public double GetHazelCastLastLockTime()
    {
      double castLastLockTime = 0.0;
      TimerValueSource timerValueSource = Metric.Context(Constants.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName))).DataProvider.CurrentMetricsData.Timers.FirstOrDefault<TimerValueSource>((Func<TimerValueSource, bool>) (t => t.Name == "HazelCastLockCallIncTimer"));
      if (timerValueSource != null)
        castLastLockTime = timerValueSource.Value.Histogram.LastValue;
      return castLastLockTime;
    }
  }
}
