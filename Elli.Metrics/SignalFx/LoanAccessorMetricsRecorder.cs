// Decompiled with JetBrains decompiler
// Type: Elli.Metrics.SignalFx.LoanAccessorMetricsRecorder
// Assembly: Elli.Metrics, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D16DD1E4-F07E-4CB7-B2D0-A2DD51E6F671
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Metrics.dll

using Metrics;
using Metrics.Core;
using System;

#nullable disable
namespace Elli.Metrics.SignalFx
{
  public class LoanAccessorMetricsRecorder : ILoanAccessorMetricsRecorder
  {
    private readonly string _customer;
    private readonly string _instance;

    public LoanAccessorMetricsRecorder(string customer, string instance)
    {
      this._customer = customer;
      this._instance = string.IsNullOrWhiteSpace(instance) ? Constants.DefaultInstance : instance;
    }

    public IDisposable RecordLoanAccessorTime(LoanAccessorOperationType loanOp)
    {
      return (IDisposable) Metric.Context(Constants.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName))).Timer("LoanAccessorTimer", Unit.Requests, tags: new MetricTags(new string[3]
      {
        Constants.CustomerDimension + this._customer,
        Constants.InstanceDimension + this._instance,
        Constants.LoanOperation + (object) loanOp
      })).NewContext();
    }

    public void IncrementLoanSaveCount()
    {
      Metric.Context(Constants.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName))).Counter("LoanAccessor_LoanSaveCounter", Unit.Items).Increment(1L);
    }

    public void IncrementLoanGetCount()
    {
      Metric.Context(Constants.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName))).Counter("LoanAccessor_LoanGetCounter", Unit.Items).Increment(1L);
    }
  }
}
