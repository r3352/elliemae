// Decompiled with JetBrains decompiler
// Type: Elli.Service.ILoanBatchService
// Assembly: Elli.Service, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 23E1ED92-4ABE-46FE-97AA-A4B97F8619DF
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Service.dll

using Elli.Domain.LoanBatch;

#nullable disable
namespace Elli.Service
{
  public interface ILoanBatchService
  {
    string CreateLoanBatchRequest(ILoanBatchRequest request);

    ILoanBatchRequest GetLoanBatchUpdateRequest(string requestId);

    void UpdateLoanBatchRequest(string requestId, ILoanBatchRequest request);
  }
}
