// Decompiled with JetBrains decompiler
// Type: Encompass.Diagnostics.Metrics.SampledLogMetricRecorder
// Assembly: Encompass.Diagnostics, Version=1.0.0.1, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E8A3B074-7BF0-4187-B0D2-083265232A16
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Encompass.Diagnostics.dll

using Encompass.Diagnostics.Logging.Schema;
using System;
using System.Diagnostics;

#nullable disable
namespace Encompass.Diagnostics.Metrics
{
  public class SampledLogMetricRecorder : LogMetricRecorder, IMetricRecorder, IDisposable
  {
    private readonly bool _sample;
    private readonly long _forceSampleDurationMS;
    private readonly Stopwatch _watch = Stopwatch.StartNew();

    public SampledLogMetricRecorder(
      IMetricSampler sampler,
      int samplePercentage,
      long forceSampleMS,
      LogFields logFields)
      : base(logFields)
    {
      this._sample = sampler.ShouldSample(samplePercentage);
      this._forceSampleDurationMS = forceSampleMS;
    }

    public override void Publish()
    {
      if (this._watch.ElapsedMilliseconds >= this._forceSampleDurationMS || this._sample)
        base.Publish();
      else
        this.Clear();
    }
  }
}
