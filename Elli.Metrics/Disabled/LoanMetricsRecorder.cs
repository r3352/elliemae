// Decompiled with JetBrains decompiler
// Type: Elli.Metrics.Disabled.LoanMetricsRecorder
// Assembly: Elli.Metrics, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D16DD1E4-F07E-4CB7-B2D0-A2DD51E6F671
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Metrics.dll

using System;

#nullable disable
namespace Elli.Metrics.Disabled
{
  public class LoanMetricsRecorder : ILoanMetricsRecorder, IDisposable
  {
    public IDisposable RecordLoanRepositoryTime(string apiName) => (IDisposable) this;

    public IDisposable RecordLoanSerializationTime(string apiName) => (IDisposable) this;

    public IDisposable RecordArchiveRepositoryTime(string apiName) => (IDisposable) this;

    public IDisposable RecordLoanEventPublishTime() => (IDisposable) this;

    public double GetLoanEventPublishTimeMedian() => 0.0;

    public double GetLoanEventPublishTimeLast() => 0.0;

    public void RecordLoanDocumentLength(int documentLength)
    {
    }

    public void IncrementLoanOperationCount()
    {
    }

    public void IncrementErrorCount(string errorType)
    {
    }

    public void Dispose()
    {
    }

    public IDisposable RecordMortgageServiceTime(string apiName) => (IDisposable) this;

    public void IncrementFileSystemLoanRead()
    {
    }

    public IDisposable RecordLoanEventWebhookSendTime() => (IDisposable) this;

    public double GetLoanEventWebhookSendTimeMedian() => 0.0;

    public double GetLoanEventWebhookSendTimeLast() => 0.0;

    public IDisposable RecordLoanEventDataLakeSendTime() => (IDisposable) this;

    public double GetLoanEventDataLakeSendTimeMedian() => 0.0;

    public double GetLoanEventDataLakeSendTimeLast() => 0.0;

    public IDisposable RecordLoanDeferrableSnapshotTime() => (IDisposable) this;

    public double GetLoanDeferrableSnapshotTimeMedian() => 0.0;

    public double GetLoanDeferrableSnapshotTimeLast() => 0.0;
  }
}
