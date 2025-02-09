// Decompiled with JetBrains decompiler
// Type: Elli.Metrics.SignalFx.LoanPipelineMetricsRecorder
// Assembly: Elli.Metrics, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D16DD1E4-F07E-4CB7-B2D0-A2DD51E6F671
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Metrics.dll

using Metrics;
using Metrics.Core;
using System;

#nullable disable
namespace Elli.Metrics.SignalFx
{
  public class LoanPipelineMetricsRecorder : ILoanPipelineMetricsRecorder
  {
    private readonly string _customer;
    private readonly string _instance;
    private const string CreateCursorTimerName = "LoanPipelineCreateCursorTimer";
    private const string CreateCursorMetadataTimerName = "LoanPipelineCreateCursorMetadataTimer";
    private const string StoreCursorDataTimer = "LoanPipelineStoreCursorDataTimer";
    private const string GetCursorMetadataTimerName = "LoanPipelineGetCursorMetadataTimer";
    private const string RetrieveCursorDataTimerName = "LoanPipelineRetrieveCursorDataTimer";
    private const string ExecuteFilterQueryTimerName = "LoanPipelineExecuteFilterQueryTimer";
    private const string RetrieveLoanDetailsTimerName = "LoanPipelineRetrieveLoanDetailsTimer";

    public LoanPipelineMetricsRecorder(string customer, string instance)
    {
      this._customer = customer;
      this._instance = string.IsNullOrWhiteSpace(instance) ? Constants.DefaultInstance : instance;
    }

    public IDisposable RecordCreateCursorTime()
    {
      return (IDisposable) ((TaggedMetricsContext) Metric.Context(Constants.ServiceContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName)))).IncrementalTimer("LoanPipelineCreateCursorTimer", Unit.Requests).NewContext();
    }

    public IDisposable RecordCreateCursorMetadataTime()
    {
      return (IDisposable) ((TaggedMetricsContext) Metric.Context(Constants.ServiceContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName)))).IncrementalTimer("LoanPipelineCreateCursorMetadataTimer", Unit.Requests).NewContext();
    }

    public IDisposable RecordStoreCursorDataTime()
    {
      return (IDisposable) ((TaggedMetricsContext) Metric.Context(Constants.ServiceContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName)))).IncrementalTimer("LoanPipelineStoreCursorDataTimer", Unit.Requests).NewContext();
    }

    public IDisposable RecordGetCursorMetadataTime()
    {
      return (IDisposable) ((TaggedMetricsContext) Metric.Context(Constants.ServiceContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName)))).IncrementalTimer("LoanPipelineGetCursorMetadataTimer", Unit.Requests).NewContext();
    }

    public IDisposable RecordRetrieveCursorDataTime()
    {
      return (IDisposable) ((TaggedMetricsContext) Metric.Context(Constants.ServiceContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName)))).IncrementalTimer("LoanPipelineRetrieveCursorDataTimer", Unit.Requests).NewContext();
    }

    public IDisposable RecordExecuteFilterQueryTime()
    {
      return (IDisposable) ((TaggedMetricsContext) Metric.Context(Constants.ServiceContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName)))).IncrementalTimer("LoanPipelineExecuteFilterQueryTimer", Unit.Requests).NewContext();
    }

    public IDisposable RecordRetrieveLoanDetailsTime()
    {
      return (IDisposable) ((TaggedMetricsContext) Metric.Context(Constants.ServiceContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName)))).IncrementalTimer("LoanPipelineRetrieveLoanDetailsTimer", Unit.Requests).NewContext();
    }

    public void IncrementErrorCount(string errorType)
    {
      ((TaggedMetricsContext) Metric.Context(Constants.ServiceContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName)))).IncrementalCounter("LoanPipelineErrorCounter", Unit.Items, new MetricTags(Constants.ErrorType + errorType)).Increment(1L);
    }
  }
}
