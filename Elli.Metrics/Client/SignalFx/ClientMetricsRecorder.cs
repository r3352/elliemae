// Decompiled with JetBrains decompiler
// Type: Elli.Metrics.Client.SignalFx.ClientMetricsRecorder
// Assembly: Elli.Metrics, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D16DD1E4-F07E-4CB7-B2D0-A2DD51E6F671
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Metrics.dll

using Metrics;
using Metrics.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace Elli.Metrics.Client.SignalFx
{
  public class ClientMetricsRecorder : IClientMetricsRecorder
  {
    private readonly string _instance;
    private readonly string _smartClientVersion;
    private readonly string _userId;
    private static List<string> base_tags;

    private static MetricTags? _tags { get; set; }

    [CLSCompliant(false)]
    public static MetricTags Tags
    {
      get
      {
        if (!ClientMetricsRecorder._tags.HasValue)
          ClientMetricsRecorder._tags = new MetricTags?(new MetricTags((IEnumerable<string>) ClientMetricsRecorder.base_tags));
        return ClientMetricsRecorder._tags.Value;
      }
    }

    public ClientMetricsRecorder(string instance, string smartClientVersion, string userId)
    {
      this._instance = string.IsNullOrWhiteSpace(instance) ? "Default" : instance;
      this._smartClientVersion = smartClientVersion;
      this._userId = userId;
      ClientMetricsRecorder.base_tags = new List<string>((IEnumerable<string>) new string[3]
      {
        "Instance=" + this._instance,
        "SCVersion=" + this._smartClientVersion,
        "User=" + this._userId
      });
    }

    private TaggedMetricsContext GetContext()
    {
      return (TaggedMetricsContext) Metric.Context("Enc_Client", (Func<string, MetricsContext>) (ctxName => (MetricsContext) new TaggedMetricsContext(ctxName)));
    }

    public void AddTag(string tag, string value)
    {
      ClientMetricsRecorder.base_tags.Add(string.Format("{0}={1}", (object) tag, (object) value));
      ClientMetricsRecorder._tags = new MetricTags?();
    }

    public void AddTag(SFxTag tag)
    {
      ClientMetricsRecorder.base_tags.Add(tag.ToString());
      ClientMetricsRecorder._tags = new MetricTags?();
    }

    public void IncrementCounter(string name)
    {
      this.GetContext().IncrementalCounter(name, Unit.Items, ClientMetricsRecorder.Tags).Increment();
    }

    public void IncrementCounter(string name, params SFxTag[] tags)
    {
      List<string> tags1 = new List<string>((IEnumerable<string>) ClientMetricsRecorder.base_tags);
      foreach (SFxTag tag in tags)
        tags1.Add(tag.ToString());
      MetricTags tags2 = new MetricTags((IEnumerable<string>) tags1);
      this.GetContext().IncrementalCounter(name, Unit.Items, tags2).Increment();
    }

    public void IncrementCounter(string name, params string[] tags)
    {
      List<string> tags1 = new List<string>((IEnumerable<string>) ClientMetricsRecorder.base_tags);
      tags1.AddRange((IEnumerable<string>) tags);
      MetricTags tags2 = new MetricTags((IEnumerable<string>) tags1);
      this.GetContext().IncrementalCounter(name, Unit.Items, tags2).Increment();
    }

    public void IncrementErrorCounter(
      Exception ex,
      string description,
      [CallerFilePath] string sourceFilePath = "",
      [CallerMemberName] string member = "",
      [CallerLineNumber] int sourceLineNumber = 0)
    {
      List<string> tags1 = new List<string>((IEnumerable<string>) ClientMetricsRecorder.base_tags);
      if (!string.IsNullOrEmpty(description))
        tags1.Add(string.Format("ErrorType={0}", (object) description));
      else if (ex != null)
        tags1.Add(string.Format("ErrorType={0}", (object) ex.Message));
      string empty = string.Empty;
      if (!string.IsNullOrEmpty(sourceFilePath))
        empty += Path.GetFileNameWithoutExtension(sourceFilePath);
      if (!string.IsNullOrEmpty(member))
      {
        if (!string.IsNullOrEmpty(empty))
          empty += ".";
        empty += member;
      }
      if (sourceLineNumber >= 0)
      {
        if (!string.IsNullOrEmpty(empty))
          empty += ":";
        empty += sourceLineNumber.ToString();
      }
      if (!string.IsNullOrEmpty(empty))
        tags1.Add(string.Format("CodeLocation={0}", (object) empty));
      if (ex != null)
        tags1.Add(string.Format("ExceptionType={0}", (object) ex.GetType().ToString()));
      tags1.Add("ActionType=Internal");
      MetricTags tags2 = new MetricTags((IEnumerable<string>) tags1);
      this.GetContext().IncrementalCounter("LoanErrorIncCounter", Unit.Items, tags2).Increment();
    }

    public IDisposable IncrementalTimer(string name)
    {
      return (IDisposable) new ClientMetricsRecorder.NewTimer(name, (IClientMetricsRecorder) this);
    }

    public IDisposable IncrementalTimer(string name, params SFxTag[] tags)
    {
      return (IDisposable) new ClientMetricsRecorder.NewTimer(name, (IClientMetricsRecorder) this, tags);
    }

    public IDisposable OldIncrementalTimer(string name, params SFxTag[] tags)
    {
      List<string> tags1 = new List<string>((IEnumerable<string>) ClientMetricsRecorder.base_tags);
      foreach (SFxTag tag in tags)
        tags1.Add(tag.ToString());
      MetricTags tags2 = new MetricTags((IEnumerable<string>) tags1);
      return (IDisposable) this.GetContext().IncrementalTimer(name, Unit.Requests, SamplingType.SlidingWindow, tags: tags2).NewContext();
    }

    public IDisposable IncrementalTimer(string name, params string[] tags)
    {
      return (IDisposable) new ClientMetricsRecorder.NewTimer(name, (IClientMetricsRecorder) this, tags);
    }

    public void RecordIncrementalTimerSample(string name, long time)
    {
      TaggedMetricsContext context = this.GetContext();
      context.IncrementalTimer(name, Unit.Requests, SamplingType.SlidingWindow, tags: ClientMetricsRecorder.Tags).Record(time, TimeUnit.Milliseconds);
      context.IncrementalCounter(name + "_Arb", Unit.Items, ClientMetricsRecorder.Tags).Increment();
    }

    public void RecordIncrementalTimerSample(string name, long time, params SFxTag[] tags)
    {
      List<string> tags1 = new List<string>((IEnumerable<string>) ClientMetricsRecorder.base_tags);
      foreach (SFxTag tag in tags)
        tags1.Add(tag.ToString());
      MetricTags tags2 = new MetricTags((IEnumerable<string>) tags1);
      TaggedMetricsContext context = this.GetContext();
      context.IncrementalTimer(name, Unit.Requests, SamplingType.SlidingWindow, tags: tags2).Record(time, TimeUnit.Milliseconds);
      context.IncrementalCounter(name + "_Arb", Unit.Items, tags2).Increment();
    }

    public void RecordIncrementalTimerSample(string name, long time, params string[] tags)
    {
      List<string> tags1 = new List<string>((IEnumerable<string>) ClientMetricsRecorder.base_tags);
      if (tags != null && tags.Length != 0)
        tags1.AddRange((IEnumerable<string>) tags);
      MetricTags tags2 = new MetricTags((IEnumerable<string>) tags1);
      TaggedMetricsContext context = this.GetContext();
      context.IncrementalTimer(name, Unit.Requests, SamplingType.SlidingWindow, tags: tags2).Record(time, TimeUnit.Milliseconds);
      context.IncrementalCounter(name + "_Arb", Unit.Items, tags2).Increment();
    }

    public void RecordIncrementalTimerSample(string name, TimeSpan time)
    {
      this.RecordIncrementalTimerSample(name, (long) time.Milliseconds);
    }

    public void RecordIncrementalTimerSample(string name, TimeSpan time, params SFxTag[] tags)
    {
      this.RecordIncrementalTimerSample(name, (long) time.Milliseconds, tags);
    }

    public void RecordIncrementalTimerSample(string name, TimeSpan time, params string[] tags)
    {
      this.RecordIncrementalTimerSample(name, (long) time.Milliseconds, tags);
    }

    private class NewTimer : IDisposable
    {
      private string _name;
      private IClientMetricsRecorder _metricsRecorder;
      private long _startime;
      private List<string> _tags;

      public NewTimer(string name, IClientMetricsRecorder metricsRecorder, params SFxTag[] tags)
      {
        this._name = name;
        this._metricsRecorder = metricsRecorder;
        this._startime = DateTime.Now.Ticks;
        if (tags != null && tags.Length != 0)
        {
          this._tags = new List<string>();
          for (int index = 0; index < tags.Length; ++index)
            this._tags.Add(tags[index].ToString());
        }
        else
          this._tags = (List<string>) null;
      }

      public NewTimer(string name, IClientMetricsRecorder metricsRecorder, params string[] tags)
      {
        this._name = name;
        this._metricsRecorder = metricsRecorder;
        this._startime = DateTime.Now.Ticks;
        this._tags = new List<string>((IEnumerable<string>) tags);
      }

      public NewTimer(string name, IClientMetricsRecorder metricsRecorder)
      {
        this._name = name;
        this._metricsRecorder = metricsRecorder;
        this._startime = DateTime.Now.Ticks;
        this._tags = (List<string>) null;
      }

      public void Dispose()
      {
        long time = (DateTime.Now.Ticks - this._startime) / 10000L;
        if ((Marshal.GetExceptionPointers() != IntPtr.Zero ? 1 : (Marshal.GetExceptionCode() != 0 ? 1 : 0)) != 0)
          this._metricsRecorder.IncrementErrorCounter((Exception) null, this._name + " failed because of an exception");
        else
          this._metricsRecorder.RecordIncrementalTimerSample(this._name, time, this._tags.ToArray());
      }
    }
  }
}
