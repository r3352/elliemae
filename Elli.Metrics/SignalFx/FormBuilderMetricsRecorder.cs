// Decompiled with JetBrains decompiler
// Type: Elli.Metrics.SignalFx.FormBuilderMetricsRecorder
// Assembly: Elli.Metrics, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D16DD1E4-F07E-4CB7-B2D0-A2DD51E6F671
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Metrics.dll

using Metrics;
using Metrics.Core;
using System;

#nullable disable
namespace Elli.Metrics.SignalFx
{
  public class FormBuilderMetricsRecorder : IFormBuilderMetricsRecorder
  {
    private readonly string _customer;
    private readonly string _instance;

    public FormBuilderMetricsRecorder(string customer, string instance)
    {
      this._customer = customer;
      this._instance = string.IsNullOrWhiteSpace(instance) ? Constants.DefaultInstance : instance;
    }

    public IDisposable RecordInputFormServiceTime(string apiName)
    {
      this.IncrementOpCount();
      return (IDisposable) Metric.Context(Constants.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName))).Timer("InputFormServiceTimer", Unit.Requests, tags: new MetricTags(new string[3]
      {
        Constants.CustomerDimension + this._customer,
        Constants.InstanceDimension + this._instance,
        Constants.ApiDimension + apiName
      })).NewContext();
    }

    public void IncrementOpCount()
    {
      Metric.Context(Constants.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName))).Counter("InputFormBuilderOpCounter", Unit.Items).Increment(1L);
    }
  }
}
