// Decompiled with JetBrains decompiler
// Type: Elli.Metrics.SignalFx.MongoUtilityMetricsRecorder
// Assembly: Elli.Metrics, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D16DD1E4-F07E-4CB7-B2D0-A2DD51E6F671
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Metrics.dll

using Metrics;
using Metrics.Core;
using System;

#nullable disable
namespace Elli.Metrics.SignalFx
{
  public class MongoUtilityMetricsRecorder : IMongoUtilityMetricsRecorder
  {
    private readonly string _customer;
    private readonly string _instance;

    public MongoUtilityMetricsRecorder(string customer, string instanceId)
    {
      this._customer = customer;
      this._instance = instanceId;
    }

    public IDisposable RecordsMongoUtilityApiTime(string apiName)
    {
      return (IDisposable) Metric.Context(Constants.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName))).Timer("MongoApiCallTimer", Unit.Requests, SamplingType.SlidingWindow, tags: new MetricTags(new string[3]
      {
        Constants.CustomerDimension + this._customer,
        Constants.InstanceDimension + this._instance,
        Constants.ApiDimension + apiName
      })).NewContext();
    }

    public void IncrementLoanLoadCount()
    {
      Metric.Context(Constants.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName))).Counter("MongoLoadLoanCounter", Unit.Items).Increment(1L);
    }

    public void IncrementReadPipelineCount()
    {
      Metric.Context(Constants.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName))).Counter("MongoReadPipelineCounter", Unit.Items).Increment(1L);
    }

    public void IncrementLoanGetCount()
    {
      Metric.Context(Constants.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName))).Counter("MongoLoanGetCounter", Unit.Items).Increment(1L);
    }

    public void IncrementLoanUpdateCount()
    {
      Metric.Context(Constants.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName))).Counter("MongoLoanUpdateCounter", Unit.Items).Increment(1L);
    }
  }
}
