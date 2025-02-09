// Decompiled with JetBrains decompiler
// Type: Elli.Metrics.SignalFx.ContactsPaginationMetricsRecorder
// Assembly: Elli.Metrics, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D16DD1E4-F07E-4CB7-B2D0-A2DD51E6F671
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Metrics.dll

using Metrics;
using Metrics.Core;
using System;

#nullable disable
namespace Elli.Metrics.SignalFx
{
  public class ContactsPaginationMetricsRecorder : IContactsPaginationMetricsRecorder
  {
    private readonly string _customer;
    private readonly string _instance;
    private const string CreateCursorTimerName = "ContactsPaginationCreateCursorTimer";
    private const string CreateCursorMetadataTimerName = "ContactsPaginationCreateCursorMetadataTimer";
    private const string StoreCursorDataTimer = "ContactsPaginationStoreCursorDataTimer";
    private const string GetCursorMetadataTimerName = "ContactsPaginationGetCursorMetadataTimer";
    private const string RetrieveCursorDataTimerName = "ContactsPaginationRetrieveCursorDataTimer";
    private const string ExecuteFilterQueryTimerName = "ContactsPaginationExecuteFilterQueryTimer";
    private const string RetrieveLoanDetailsTimerName = "ContactsPaginationRetrieveContactsSummaryTimer";

    public ContactsPaginationMetricsRecorder(string customer, string instance)
    {
      this._customer = customer;
      this._instance = string.IsNullOrWhiteSpace(instance) ? Constants.DefaultInstance : instance;
    }

    public IDisposable RecordCreateCursorTime()
    {
      return (IDisposable) ((TaggedMetricsContext) Metric.Context(Constants.ServiceContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName)))).IncrementalTimer("ContactsPaginationCreateCursorTimer", Unit.Requests).NewContext();
    }

    public IDisposable RecordCreateCursorMetadataTime()
    {
      return (IDisposable) ((TaggedMetricsContext) Metric.Context(Constants.ServiceContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName)))).IncrementalTimer("ContactsPaginationCreateCursorMetadataTimer", Unit.Requests).NewContext();
    }

    public IDisposable RecordStoreCursorDataTime()
    {
      return (IDisposable) ((TaggedMetricsContext) Metric.Context(Constants.ServiceContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName)))).IncrementalTimer("ContactsPaginationStoreCursorDataTimer", Unit.Requests).NewContext();
    }

    public IDisposable RecordGetCursorMetadataTime()
    {
      return (IDisposable) ((TaggedMetricsContext) Metric.Context(Constants.ServiceContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName)))).IncrementalTimer("ContactsPaginationGetCursorMetadataTimer", Unit.Requests).NewContext();
    }

    public IDisposable RecordRetrieveCursorDataTime()
    {
      return (IDisposable) ((TaggedMetricsContext) Metric.Context(Constants.ServiceContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName)))).IncrementalTimer("ContactsPaginationRetrieveCursorDataTimer", Unit.Requests).NewContext();
    }

    public IDisposable RecordExecuteFilterQueryTime()
    {
      return (IDisposable) ((TaggedMetricsContext) Metric.Context(Constants.ServiceContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName)))).IncrementalTimer("ContactsPaginationExecuteFilterQueryTimer", Unit.Requests).NewContext();
    }

    public IDisposable RecordRetrieveContactsSummaryTime()
    {
      return (IDisposable) ((TaggedMetricsContext) Metric.Context(Constants.ServiceContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName)))).IncrementalTimer("ContactsPaginationRetrieveContactsSummaryTimer", Unit.Requests).NewContext();
    }

    public void IncrementErrorCount(string errorType)
    {
      ((TaggedMetricsContext) Metric.Context(Constants.ServiceContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName)))).IncrementalCounter("ContactsPaginationErrorCounter", Unit.Items, new MetricTags(Constants.ErrorType + errorType)).Increment(1L);
    }
  }
}
