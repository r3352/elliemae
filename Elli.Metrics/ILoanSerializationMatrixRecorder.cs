// Decompiled with JetBrains decompiler
// Type: Elli.Metrics.ILoanSerializationMatrixRecorder
// Assembly: Elli.Metrics, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D16DD1E4-F07E-4CB7-B2D0-A2DD51E6F671
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Metrics.dll

using System;

#nullable disable
namespace Elli.Metrics
{
  [CLSCompliant(true)]
  public interface ILoanSerializationMatrixRecorder
  {
    IDisposable RecordsLoanSerializationTime(
      LoanSeriazationOperationType serializationOperation);

    IDisposable RecordsLoanDeserializationTime(
      LoanSeriazationOperationType serializationOperation);

    void IncrementLoanSerializationCount();

    void IncrementLoanDeserializationCount();
  }
}
