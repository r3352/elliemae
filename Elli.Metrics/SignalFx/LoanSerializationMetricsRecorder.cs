// Decompiled with JetBrains decompiler
// Type: Elli.Metrics.SignalFx.LoanSerializationMetricsRecorder
// Assembly: Elli.Metrics, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D16DD1E4-F07E-4CB7-B2D0-A2DD51E6F671
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Metrics.dll

using Metrics;
using Metrics.Core;
using System;

#nullable disable
namespace Elli.Metrics.SignalFx
{
  public class LoanSerializationMetricsRecorder : ILoanSerializationMatrixRecorder
  {
    private readonly string _customer;
    private readonly string _instance;
    private const string LoanSerializationTimerName = "LoanSerializationTime";
    private const string LoanDeserializationTimerName = "LoanDeserializationTime";

    public LoanSerializationMetricsRecorder(string customer, string instance)
    {
      this._customer = customer;
      this._instance = string.IsNullOrWhiteSpace(instance) ? Constants.DefaultInstance : instance;
    }

    public IDisposable RecordsLoanDeserializationTime(
      LoanSeriazationOperationType serializationOperation)
    {
      return (IDisposable) Metric.Context(Constants.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName))).Timer("LoanDeserializationTime", Unit.Requests, tags: new MetricTags(new string[3]
      {
        Constants.CustomerDimension + this._customer,
        Constants.InstanceDimension + this._instance,
        Constants.SerializationOperation + (object) serializationOperation
      })).NewContext();
    }

    public IDisposable RecordsLoanSerializationTime(
      LoanSeriazationOperationType serializationOperation)
    {
      return (IDisposable) Metric.Context(Constants.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName))).Timer("LoanSerializationTime", Unit.Requests, tags: new MetricTags(new string[3]
      {
        Constants.CustomerDimension + this._customer,
        Constants.InstanceDimension + this._instance,
        Constants.SerializationOperation + (object) serializationOperation
      })).NewContext();
    }

    public void IncrementLoanSerializationCount()
    {
      Metric.Context(Constants.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName))).Counter("LoanSerializationCount", Unit.Items).Increment(1L);
    }

    public void IncrementLoanDeserializationCount()
    {
      Metric.Context(Constants.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName))).Counter("LoanDeserializationCount", Unit.Items).Increment(1L);
    }
  }
}
