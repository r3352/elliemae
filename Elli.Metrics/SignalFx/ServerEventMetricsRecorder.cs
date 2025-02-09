// Decompiled with JetBrains decompiler
// Type: Elli.Metrics.SignalFx.ServerEventMetricsRecorder
// Assembly: Elli.Metrics, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D16DD1E4-F07E-4CB7-B2D0-A2DD51E6F671
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Metrics.dll

using Metrics;
using Metrics.Core;
using System;

#nullable disable
namespace Elli.Metrics.SignalFx
{
  public class ServerEventMetricsRecorder : IServerEventMetricsRecorder
  {
    private const string PublishErrorCounterName = "PublishErrorCounter";
    private const string PublishTimerName = "PublishTimer";
    private const string LogTimerName = "LogTimer";
    private const string LogErrorCounterName = "LogErrorCounter";
    private const string RepublishTimerName = "RepublishTimer";
    private const string RepublishErrorCounterName = "RepublishErrorCounter";

    private string GetEventDimension(ServerEventType eventType)
    {
      string str = "ServerEvent=";
      if (eventType == ServerEventType.Loan)
        return str + "Loan";
      throw new ArgumentOutOfRangeException(nameof (eventType), (object) eventType, "Unsupported ServerEventType");
    }

    private string GetErrorDimension(ServerEventErrorType errorType)
    {
      string str = "ServerEventError=";
      if (errorType == ServerEventErrorType.PublishFailure)
        return str + "PublishFailure";
      if (errorType == ServerEventErrorType.SoftTimeout)
        return str + "SoftTimeout";
      throw new ArgumentOutOfRangeException(nameof (errorType), (object) errorType, "Unsupported ServerEventErrorType");
    }

    public void IncrementPublishErrorCount(
      ServerEventType eventType,
      ServerEventErrorType errorType)
    {
      ((TaggedMetricsContext) Metric.Context("ENC_ServerEvent", (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName)))).IncrementalCounter("PublishErrorCounter", Unit.Errors, new MetricTags(new string[2]
      {
        this.GetEventDimension(eventType),
        this.GetErrorDimension(errorType)
      })).Increment(1L);
    }

    public IDisposable RecordPublishTime(ServerEventType eventType)
    {
      return (IDisposable) ((TaggedMetricsContext) Metric.Context("ENC_ServerEvent", (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName)))).IncrementalTimer("PublishTimer", Unit.Calls, tags: new MetricTags(this.GetEventDimension(eventType))).NewContext();
    }

    public IDisposable RecordLogTime(ServerEventType eventType)
    {
      return (IDisposable) ((TaggedMetricsContext) Metric.Context("ENC_ServerEvent", (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName)))).IncrementalTimer("LogTimer", Unit.Calls, tags: new MetricTags(this.GetEventDimension(eventType))).NewContext();
    }

    public void IncrementLogErrorCount(ServerEventType eventType)
    {
      ((TaggedMetricsContext) Metric.Context("ENC_ServerEvent", (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName)))).IncrementalCounter("LogErrorCounter", Unit.Errors, new MetricTags(this.GetEventDimension(eventType))).Increment(1L);
    }

    public IDisposable RecordRepublishTime(ServerEventType eventType)
    {
      return (IDisposable) ((TaggedMetricsContext) Metric.Context("ENC_ServerEvent", (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName)))).IncrementalTimer("RepublishTimer", Unit.Calls, tags: new MetricTags(this.GetEventDimension(eventType))).NewContext();
    }

    public void IncrementRepublishErrorCount(ServerEventType eventType)
    {
      ((TaggedMetricsContext) Metric.Context("ENC_ServerEvent", (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName)))).IncrementalCounter("RepublishErrorCounter", Unit.Errors, new MetricTags(this.GetEventDimension(eventType))).Increment(1L);
    }
  }
}
