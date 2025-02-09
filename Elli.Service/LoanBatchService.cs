// Decompiled with JetBrains decompiler
// Type: Elli.Service.LoanBatchService
// Assembly: Elli.Service, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 23E1ED92-4ABE-46FE-97AA-A4B97F8619DF
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Service.dll

using Elli.Domain;
using Elli.Domain.LoanBatch;
using Elli.Metrics;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Settings;
using System;

#nullable disable
namespace Elli.Service
{
  public class LoanBatchService : ILoanBatchService, IDisposable
  {
    private readonly ILoanBatchRepository _loanBatchRepository;
    private IStorageSettings _storageSettings;
    private StorageMode _storageMode;
    private IUnitOfWork _unitOfWork;
    private readonly IMetricsFactory _metricsFactory;

    public LoanBatchService(ILoanBatchRepository repository, IMetricsFactory metricsFactory)
    {
      this._loanBatchRepository = repository;
      this._metricsFactory = metricsFactory;
    }

    internal void ApplyConfiguration(IStorageSettings storageSettings, StorageMode storageMode)
    {
      this._storageSettings = storageSettings;
      this._storageMode = storageMode;
      this._unitOfWork = IocContainer.CreateUnitOfWork(this._storageSettings, this._storageMode);
    }

    public string CreateLoanBatchRequest(ILoanBatchRequest request)
    {
      return this._loanBatchRepository.CreateLoanBatchRequest(request);
    }

    public ILoanBatchRequest GetLoanBatchUpdateRequest(string requestId)
    {
      return this._loanBatchRepository.GetLoanBatchUpdateRequest(requestId);
    }

    public void UpdateLoanBatchRequest(string requestId, ILoanBatchRequest request)
    {
      this._loanBatchRepository.UpdateLoanBatchRequest(requestId, request);
    }

    public void Dispose()
    {
      GC.SuppressFinalize((object) this);
      this._unitOfWork.Dispose();
    }
  }
}
