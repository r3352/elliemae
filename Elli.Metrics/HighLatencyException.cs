// Decompiled with JetBrains decompiler
// Type: Elli.Metrics.HighLatencyException
// Assembly: Elli.Metrics, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D16DD1E4-F07E-4CB7-B2D0-A2DD51E6F671
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Metrics.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace Elli.Metrics
{
  [Serializable]
  public class HighLatencyException : Exception
  {
    private double latencyMs;
    private double latencyThreasholdMs;
    private long latencyAlertPeriodMs;

    protected HighLatencyException()
    {
    }

    public HighLatencyException(
      double latencyMs,
      double latencyThreasholdMs,
      long latencyAlertPeriodMs)
      : base(string.Format("Latency of {0} ms stayed above {1} ms for longer than {2} ms.", (object) latencyMs, (object) latencyThreasholdMs, (object) latencyAlertPeriodMs))
    {
      this.latencyMs = latencyMs;
      this.latencyThreasholdMs = latencyThreasholdMs;
      this.latencyAlertPeriodMs = latencyAlertPeriodMs;
    }

    protected HighLatencyException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }

    public double LatencyMs => this.latencyMs;

    public double LatencyThreasholdMs => this.latencyThreasholdMs;

    public long LatencyAlertPeriodMs => this.latencyAlertPeriodMs;
  }
}
