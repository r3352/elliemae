// Decompiled with JetBrains decompiler
// Type: Elli.Metrics.Client.IClientMetricsRecorder
// Assembly: Elli.Metrics, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D16DD1E4-F07E-4CB7-B2D0-A2DD51E6F671
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Metrics.dll

using System;

#nullable disable
namespace Elli.Metrics.Client
{
  public interface IClientMetricsRecorder
  {
    void AddTag(string tag, string value);

    void IncrementCounter(string name);

    void IncrementCounter(string name, params SFxTag[] tags);

    void IncrementCounter(string name, params string[] tags);

    void IncrementErrorCounter(
      Exception ex,
      string description,
      string sourceFilePath = "",
      string member = "",
      int sourceLineNumber = 0);

    IDisposable IncrementalTimer(string name);

    IDisposable IncrementalTimer(string name, params SFxTag[] tags);

    IDisposable OldIncrementalTimer(string name, params SFxTag[] tags);

    IDisposable IncrementalTimer(string name, params string[] tags);

    void RecordIncrementalTimerSample(string name, long time);

    void RecordIncrementalTimerSample(string name, long time, params SFxTag[] tags);

    void RecordIncrementalTimerSample(string name, long time, params string[] tags);

    void RecordIncrementalTimerSample(string name, TimeSpan time);

    void RecordIncrementalTimerSample(string name, TimeSpan time, params SFxTag[] tags);

    void RecordIncrementalTimerSample(string name, TimeSpan time, params string[] tags);
  }
}
