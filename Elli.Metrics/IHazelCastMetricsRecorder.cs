// Decompiled with JetBrains decompiler
// Type: Elli.Metrics.IHazelCastMetricsRecorder
// Assembly: Elli.Metrics, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D16DD1E4-F07E-4CB7-B2D0-A2DD51E6F671
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Metrics.dll

using System;

#nullable disable
namespace Elli.Metrics
{
  public interface IHazelCastMetricsRecorder
  {
    void IncrementErrorCount(string errorType, string instance, string version);

    void IncrementCircuitStateChangeCount(string state, string instance, string version);

    void IncrementHazelCastOperationCount(string operation, string instance, string version);

    double GetHazelCastConnectionTimeMedian();

    double GetHazelCastGetTimeMedian();

    double GetHazelCastRemoveTimeMedian();

    double GetHazelCastKeyTimeMedian();

    double GetHazelCastUnlockTimeMedian();

    double GetHazelCastGetEntryViewTimeMedian();

    double GetHazelCastPutTimeMedian();

    double GetHazelCastLockTimeMedian();

    IDisposable RecordHazelCastConnectionTime(string instance, string version);

    IDisposable RecordHazelCastGetTime(string instance, string version);

    IDisposable RecordHazelCastRemoveTime(string instance, string version);

    IDisposable RecordHazelCastKeyTime(string instance, string version);

    IDisposable RecordHazelCastUnlockTime(string instance, string version);

    IDisposable RecordHazelCastGetEntryViewTime(string instance, string version);

    IDisposable RecordHazelCastPutTime(string instance, string version);

    IDisposable RecordHazelCastLockTime(string instance, string version);

    void ResetHazelCastConnectionTimer();

    void ResetHazelCastGetTimer();

    void ResetHazelCastRemoveTimer();

    void ResetHazelCastKeyTimer();

    void ResetHazelCastUnlockTimer();

    void ResetHazelCastGetEntryViewTimer();

    void ResetHazelCastPutTimer();

    void ResetHazelCastLockTimer();

    double GetHazelCastLastConnectionTime();

    double GetHazelCastLastGetTime();

    double GetHazelCastLastRemoveTime();

    double GetHazelCastLastKeyTime();

    double GetHazelCastLastUnlockTime();

    double GetHazelCastLastGetEntryViewTime();

    double GetHazelCastLastPutTime();

    double GetHazelCastLastLockTime();
  }
}
