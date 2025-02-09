// Decompiled with JetBrains decompiler
// Type: Elli.Metrics.SignalFx.LoanMetricsRecorder
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
  public class LoanMetricsRecorder : ILoanMetricsRecorder
  {
    private readonly string _customer;
    private readonly string _instance;
    private const string LoanEventPublishTimerName = "LoanEventPublishTimer";
    private const string LoanEventWebhookSendTimeName = "LoanEventWebhookCallIncTimer";
    private const string LoanEventDataLakeSendTimeName = "LoanEventDataLakeCallIncTimer";
    private const string LoanDeferrableSnapshotTimerName = "LoanDeferrableSnapshotTimer";

    public LoanMetricsRecorder(string customer, string instance)
    {
      this._customer = customer;
      this._instance = string.IsNullOrWhiteSpace(instance) ? Constants.DefaultInstance : instance;
    }

    public IDisposable RecordLoanRepositoryTime(string apiName)
    {
      return (IDisposable) Metric.Context(Constants.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName))).Timer("LoanRepositoryTimer", Unit.Requests, tags: new MetricTags(Constants.ApiDimension + apiName)).NewContext();
    }

    public IDisposable RecordArchiveRepositoryTime(string apiName)
    {
      return (IDisposable) Metric.Context(Constants.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName))).Timer("ArchiveRepositoryTimer", Unit.Requests, tags: new MetricTags(Constants.ApiDimension + apiName)).NewContext();
    }

    public IDisposable RecordLoanEventPublishTime()
    {
      return (IDisposable) Metric.Context(Constants.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName))).Timer("LoanEventPublishTimer", Unit.Requests).NewContext();
    }

    public double GetLoanEventPublishTimeMedian()
    {
      double publishTimeMedian = 0.0;
      TimerValueSource timerValueSource = Metric.Context(Constants.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName))).DataProvider.CurrentMetricsData.Timers.FirstOrDefault<TimerValueSource>((Func<TimerValueSource, bool>) (t => t.Name == "LoanEventPublishTimer"));
      if (timerValueSource != null)
        publishTimeMedian = timerValueSource.Value.Histogram.Median;
      return publishTimeMedian;
    }

    public double GetLoanEventPublishTimeLast()
    {
      double eventPublishTimeLast = 0.0;
      TimerValueSource timerValueSource = Metric.Context(Constants.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName))).DataProvider.CurrentMetricsData.Timers.FirstOrDefault<TimerValueSource>((Func<TimerValueSource, bool>) (t => t.Name == "LoanEventPublishTimer"));
      if (timerValueSource != null)
        eventPublishTimeLast = timerValueSource.Value.Histogram.LastValue;
      return eventPublishTimeLast;
    }

    public void RecordLoanDocumentLength(int documentLength)
    {
      Metric.Context(Constants.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName))).Histogram("LoanDocLengthHistogram", Unit.Custom("Chars"), tags: new MetricTags(new string[2]
      {
        Constants.CustomerDimension + this._customer,
        Constants.InstanceDimension + this._instance
      })).Update((long) documentLength);
    }

    public void IncrementLoanOperationCount()
    {
      Metric.Context(Constants.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName))).Counter("LoanOpCounter", Unit.Items).Increment(1L);
    }

    public void IncrementErrorCount(string errorType)
    {
      Metric.Context(Constants.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName))).Counter("LoanErrorCounter", Unit.Items, new MetricTags(Constants.ErrorType + errorType)).Increment(1L);
    }

    public void IncrementFileSystemLoanRead()
    {
      Metric.Context(Constants.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName))).Counter("FileSystemLoanReadCounter", Unit.Items).Increment(1L);
    }

    public IDisposable RecordMortgageServiceTime(string apiName)
    {
      return (IDisposable) Metric.Context(Constants.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName))).Timer("MortgageServiceTimer", Unit.Requests, tags: new MetricTags(new string[3]
      {
        Constants.CustomerDimension + this._customer,
        Constants.InstanceDimension + this._instance,
        Constants.ApiDimension + apiName
      })).NewContext();
    }

    public IDisposable RecordLoanEventWebhookSendTime()
    {
      return (IDisposable) ((TaggedMetricsContext) Metric.Context(Constants.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName)))).Timer("LoanEventWebhookCallIncTimer", Unit.Requests, SamplingType.FavourRecent, TimeUnit.Seconds, TimeUnit.Milliseconds, new MetricTags()).NewContext();
    }

    public double GetLoanEventWebhookSendTimeMedian()
    {
      double webhookSendTimeMedian = 0.0;
      TimerValueSource timerValueSource = Metric.Context(Constants.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName))).DataProvider.CurrentMetricsData.Timers.FirstOrDefault<TimerValueSource>((Func<TimerValueSource, bool>) (t => t.Name == "LoanEventWebhookCallIncTimer"));
      if (timerValueSource != null)
        webhookSendTimeMedian = timerValueSource.Value.Histogram.Median;
      return webhookSendTimeMedian;
    }

    public double GetLoanEventWebhookSendTimeLast()
    {
      double webhookSendTimeLast = 0.0;
      TimerValueSource timerValueSource = Metric.Context(Constants.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName))).DataProvider.CurrentMetricsData.Timers.FirstOrDefault<TimerValueSource>((Func<TimerValueSource, bool>) (t => t.Name == "LoanEventWebhookCallIncTimer"));
      if (timerValueSource != null)
        webhookSendTimeLast = timerValueSource.Value.Histogram.LastValue;
      return webhookSendTimeLast;
    }

    public IDisposable RecordLoanEventDataLakeSendTime()
    {
      return (IDisposable) ((TaggedMetricsContext) Metric.Context(Constants.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName)))).Timer("LoanEventDataLakeCallIncTimer", Unit.Requests, SamplingType.FavourRecent, TimeUnit.Seconds, TimeUnit.Milliseconds, new MetricTags()).NewContext();
    }

    public double GetLoanEventDataLakeSendTimeMedian()
    {
      double lakeSendTimeMedian = 0.0;
      TimerValueSource timerValueSource = Metric.Context(Constants.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName))).DataProvider.CurrentMetricsData.Timers.FirstOrDefault<TimerValueSource>((Func<TimerValueSource, bool>) (t => t.Name == "LoanEventDataLakeCallIncTimer"));
      if (timerValueSource != null)
        lakeSendTimeMedian = timerValueSource.Value.Histogram.Median;
      return lakeSendTimeMedian;
    }

    public double GetLoanEventDataLakeSendTimeLast()
    {
      double lakeSendTimeLast = 0.0;
      TimerValueSource timerValueSource = Metric.Context(Constants.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName))).DataProvider.CurrentMetricsData.Timers.FirstOrDefault<TimerValueSource>((Func<TimerValueSource, bool>) (t => t.Name == "LoanEventDataLakeCallIncTimer"));
      if (timerValueSource != null)
        lakeSendTimeLast = timerValueSource.Value.Histogram.LastValue;
      return lakeSendTimeLast;
    }

    public IDisposable RecordLoanDeferrableSnapshotTime()
    {
      return (IDisposable) Metric.Context(Constants.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName))).Timer("LoanDeferrableSnapshotTimer", Unit.Requests).NewContext();
    }

    public double GetLoanDeferrableSnapshotTimeMedian()
    {
      double snapshotTimeMedian = 0.0;
      TimerValueSource timerValueSource = Metric.Context(Constants.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName))).DataProvider.CurrentMetricsData.Timers.FirstOrDefault<TimerValueSource>((Func<TimerValueSource, bool>) (t => t.Name == "LoanDeferrableSnapshotTimer"));
      if (timerValueSource != null)
        snapshotTimeMedian = timerValueSource.Value.Histogram.Median;
      return snapshotTimeMedian;
    }

    public double GetLoanDeferrableSnapshotTimeLast()
    {
      double snapshotTimeLast = 0.0;
      TimerValueSource timerValueSource = Metric.Context(Constants.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName))).DataProvider.CurrentMetricsData.Timers.FirstOrDefault<TimerValueSource>((Func<TimerValueSource, bool>) (t => t.Name == "LoanDeferrableSnapshotTimer"));
      if (timerValueSource != null)
        snapshotTimeLast = timerValueSource.Value.Histogram.LastValue;
      return snapshotTimeLast;
    }
  }
}
