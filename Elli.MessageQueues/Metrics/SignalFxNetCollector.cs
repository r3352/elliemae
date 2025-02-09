// Decompiled with JetBrains decompiler
// Type: Elli.MessageQueues.Metrics.SignalFxNetCollector
// Assembly: Elli.MessageQueues, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 24211DB4-B81B-430D-BE95-0449CADCF25D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.MessageQueues.dll

using Metrics;
using Metrics.Core;
using System;
using System.Diagnostics.CodeAnalysis;

#nullable disable
namespace Elli.MessageQueues.Metrics
{
  [ExcludeFromCodeCoverage]
  public class SignalFxNetCollector : IMetricsCollector, IDisposable
  {
    public static string MessageTypeName = "MessageType";
    public static string QueueName = "Queue";

    public SignalFxNetCollector(string contextName) => this.ContextName = contextName;

    public string ContextName { get; private set; }

    public void IncrementConnectionCount()
    {
      if (!Global.SignalFxPolicy.MetricsCollectionEnabled)
        return;
      ((TaggedMetricsContext) Metric.Context(this.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName)))).IncrementalCounter("ConnectionIncCounter", Unit.Calls).Increment(1L);
    }

    public void IncrementChannelCount()
    {
      if (!Global.SignalFxPolicy.MetricsCollectionEnabled)
        return;
      ((TaggedMetricsContext) Metric.Context(this.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName)))).IncrementalCounter("ChannelIncCounter", Unit.Calls).Increment(1L);
    }

    public IDisposable RecordPublishTime(string messageType)
    {
      return !Global.SignalFxPolicy.MetricsCollectionEnabled ? (IDisposable) this : (IDisposable) ((TaggedMetricsContext) Metric.Context(this.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName)))).IncrementalTimer("PublishIncTimer", Unit.Requests, tags: new MetricTags(string.Format("{0}={1}", (object) SignalFxNetCollector.MessageTypeName, (object) messageType))).NewContext();
    }

    public void IncrementSubscriptionCount()
    {
      if (!Global.SignalFxPolicy.MetricsCollectionEnabled)
        return;
      ((TaggedMetricsContext) Metric.Context(this.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName)))).IncrementalCounter("SubscriptionIncCounter", Unit.Calls).Increment(1L);
    }

    public IDisposable RecordHandleMessageDeliveryTime(string messageType)
    {
      return !Global.SignalFxPolicy.MetricsCollectionEnabled ? (IDisposable) this : (IDisposable) ((TaggedMetricsContext) Metric.Context(this.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName)))).IncrementalTimer("HandleMessageDeliveryIncTimer", Unit.Events, tags: new MetricTags(string.Format("{0}={1}", (object) SignalFxNetCollector.MessageTypeName, (object) messageType))).NewContext();
    }

    public void IncrementErrorCount()
    {
      if (!Global.SignalFxPolicy.MetricsCollectionEnabled)
        return;
      ((TaggedMetricsContext) Metric.Context(this.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName)))).IncrementalCounter("ErrorIncCounter", Unit.Errors).Increment();
    }

    public IDisposable RecordMessageAckTime()
    {
      return !Global.SignalFxPolicy.MetricsCollectionEnabled ? (IDisposable) this : (IDisposable) ((TaggedMetricsContext) Metric.Context(this.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName)))).IncrementalTimer("AckIncTimer", Unit.Requests).NewContext();
    }

    public void RecordMessageWaitInQueueTime(string queueName, TimeSpan waitTimeSpan)
    {
      if (!Global.SignalFxPolicy.MetricsCollectionEnabled)
        return;
      double totalMilliseconds = waitTimeSpan.TotalMilliseconds;
      if (totalMilliseconds < 1.0)
        return;
      ((TaggedMetricsContext) Metric.Context(this.ContextName, (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName)))).Timer("MessageWaitInQueueIncTimer", Unit.Requests, SamplingType.FavourRecent, TimeUnit.Seconds, TimeUnit.Milliseconds, new MetricTags(string.Format("{0}={1}", (object) SignalFxNetCollector.QueueName, (object) queueName))).Record(Convert.ToInt64(totalMilliseconds), TimeUnit.Milliseconds);
    }

    public void Dispose()
    {
    }
  }
}
