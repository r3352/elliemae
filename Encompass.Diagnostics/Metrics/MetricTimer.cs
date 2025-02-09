// Decompiled with JetBrains decompiler
// Type: Encompass.Diagnostics.Metrics.MetricTimer
// Assembly: Encompass.Diagnostics, Version=1.0.0.1, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E8A3B074-7BF0-4187-B0D2-083265232A16
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Encompass.Diagnostics.dll

using System;
using System.Diagnostics;

#nullable disable
namespace Encompass.Diagnostics.Metrics
{
  public class MetricTimer : IDisposable
  {
    private readonly Stopwatch _watch = Stopwatch.StartNew();

    public int CallCount { get; private set; } = 0;

    public long TotalDurationMS => this._watch.ElapsedMilliseconds;

    public void Resume()
    {
      ++this.CallCount;
      this._watch.Start();
    }

    public void Dispose() => this._watch.Stop();
  }
}
