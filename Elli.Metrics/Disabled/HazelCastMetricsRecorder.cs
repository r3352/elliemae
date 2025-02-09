// Decompiled with JetBrains decompiler
// Type: Elli.Metrics.Disabled.HazelCastMetricsRecorder
// Assembly: Elli.Metrics, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D16DD1E4-F07E-4CB7-B2D0-A2DD51E6F671
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Metrics.dll

using System;

#nullable disable
namespace Elli.Metrics.Disabled
{
  public class HazelCastMetricsRecorder : IHazelCastMetricsRecorder, IDisposable
  {
    public void IncrementErrorCount(string errorType, string instance, string version)
    {
    }

    public void IncrementCircuitStateChangeCount(string state, string instance, string version)
    {
    }

    public void IncrementHazelCastOperationCount(string operation, string instance, string version)
    {
    }

    public double GetHazelCastConnectionTimeMedian() => 0.0;

    public double GetHazelCastGetTimeMedian() => 0.0;

    public double GetHazelCastRemoveTimeMedian() => 0.0;

    public double GetHazelCastKeyTimeMedian() => 0.0;

    public double GetHazelCastUnlockTimeMedian() => 0.0;

    public double GetHazelCastGetEntryViewTimeMedian() => 0.0;

    public double GetHazelCastPutTimeMedian() => 0.0;

    public double GetHazelCastLockTimeMedian() => 0.0;

    public IDisposable RecordHazelCastConnectionTime(string instance, string version)
    {
      return (IDisposable) this;
    }

    public IDisposable RecordHazelCastGetTime(string instance, string version)
    {
      return (IDisposable) this;
    }

    public IDisposable RecordHazelCastRemoveTime(string instance, string version)
    {
      return (IDisposable) this;
    }

    public IDisposable RecordHazelCastKeyTime(string instance, string version)
    {
      return (IDisposable) this;
    }

    public IDisposable RecordHazelCastUnlockTime(string instance, string version)
    {
      return (IDisposable) this;
    }

    public IDisposable RecordHazelCastGetEntryViewTime(string instance, string version)
    {
      return (IDisposable) this;
    }

    public IDisposable RecordHazelCastPutTime(string instance, string version)
    {
      return (IDisposable) this;
    }

    public IDisposable RecordHazelCastLockTime(string instance, string version)
    {
      return (IDisposable) this;
    }

    public void ResetHazelCastConnectionTimer()
    {
    }

    public void ResetHazelCastGetTimer()
    {
    }

    public void ResetHazelCastRemoveTimer()
    {
    }

    public void ResetHazelCastKeyTimer()
    {
    }

    public void ResetHazelCastUnlockTimer()
    {
    }

    public void ResetHazelCastGetEntryViewTimer()
    {
    }

    public void ResetHazelCastPutTimer()
    {
    }

    public void ResetHazelCastLockTimer()
    {
    }

    public double GetHazelCastLastConnectionTime() => 0.0;

    public double GetHazelCastLastGetTime() => 0.0;

    public double GetHazelCastLastRemoveTime() => 0.0;

    public double GetHazelCastLastKeyTime() => 0.0;

    public double GetHazelCastLastUnlockTime() => 0.0;

    public double GetHazelCastLastGetEntryViewTime() => 0.0;

    public double GetHazelCastLastPutTime() => 0.0;

    public double GetHazelCastLastLockTime() => 0.0;

    public void Dispose()
    {
    }
  }
}
