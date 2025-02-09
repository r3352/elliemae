// Decompiled with JetBrains decompiler
// Type: Encompass.Diagnostics.Metrics.DisabledMetricRecorder
// Assembly: Encompass.Diagnostics, Version=1.0.0.1, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E8A3B074-7BF0-4187-B0D2-083265232A16
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Encompass.Diagnostics.dll

using System;

#nullable disable
namespace Encompass.Diagnostics.Metrics
{
  public class DisabledMetricRecorder : IMetricRecorder, IDisposable
  {
    public static DisabledMetricRecorder Instance = new DisabledMetricRecorder();

    private DisabledMetricRecorder()
    {
    }

    public void Dispose()
    {
    }

    public void IncrementCount(string counter)
    {
    }

    public void IncrementCount(string counter, int incrementBy)
    {
    }

    public void ResetCount(string counter)
    {
    }

    public void SetCount(string counter, int count)
    {
    }

    public void ResetTimer(string activity)
    {
    }

    public IDisposable StartTimer(string activity)
    {
      return (IDisposable) DisabledMetricRecorder.DisabledTimer.Instance;
    }

    public void Publish()
    {
    }

    private class DisabledTimer : IDisposable
    {
      public static DisabledMetricRecorder.DisabledTimer Instance = new DisabledMetricRecorder.DisabledTimer();

      private DisabledTimer()
      {
      }

      public void Dispose()
      {
      }
    }
  }
}
