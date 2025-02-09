// Decompiled with JetBrains decompiler
// Type: Elli.Metrics.Client.Disabled.ClientMetricsRecorder
// Assembly: Elli.Metrics, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D16DD1E4-F07E-4CB7-B2D0-A2DD51E6F671
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Metrics.dll

using System;

#nullable disable
namespace Elli.Metrics.Client.Disabled
{
  public class ClientMetricsRecorder : MarshalByRefObject, IClientMetricsRecorder, IDisposable
  {
    public void AddTag(string tag, string value)
    {
    }

    public void IncrementCounter(string name)
    {
    }

    public IDisposable IncrementalTimer(string name) => (IDisposable) this;

    public void RecordIncrementalTimerSample(string name, long time)
    {
    }

    public void RecordIncrementalTimerSample(string name, TimeSpan time)
    {
    }

    public void IncrementCounter(string name, params SFxTag[] tags)
    {
    }

    public IDisposable IncrementalTimer(string name, params SFxTag[] tags) => (IDisposable) this;

    public IDisposable OldIncrementalTimer(string name, params SFxTag[] tags) => (IDisposable) this;

    public void RecordIncrementalTimerSample(string name, long time, params SFxTag[] tags)
    {
    }

    public void RecordIncrementalTimerSample(string name, TimeSpan time, params SFxTag[] tags)
    {
    }

    public void IncrementCounter(string name, params string[] tags)
    {
    }

    public IDisposable IncrementalTimer(string name, params string[] tags) => (IDisposable) this;

    public void RecordIncrementalTimerSample(string name, long time, params string[] tags)
    {
    }

    public void RecordIncrementalTimerSample(string name, TimeSpan time, params string[] tags)
    {
    }

    public void IncrementErrorCounter(
      Exception ex,
      string description,
      string sourceFilePath = "",
      string member = "",
      int sourceLineNumber = 0)
    {
    }

    public void Dispose()
    {
    }
  }
}
