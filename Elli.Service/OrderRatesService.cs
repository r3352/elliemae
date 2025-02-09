// Decompiled with JetBrains decompiler
// Type: Elli.Service.OrderRatesService
// Assembly: Elli.Service, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 23E1ED92-4ABE-46FE-97AA-A4B97F8619DF
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Service.dll

using Elli.Common.Data;
using Elli.Domain;
using Elli.Domain.APR;
using Elli.Metrics;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Settings;
using System;

#nullable disable
namespace Elli.Service
{
  public class OrderRatesService : IOrderRatesService, IDisposable
  {
    private readonly IOrderRatesRepository _orderRatesRepository;
    private readonly IMetricsFactory _metricsFactory;
    private StorageSettings _settings;
    private StorageMode _storageMode;
    private IUnitOfWork _unitOfWork;

    public OrderRatesService(IOrderRatesRepository repository, IMetricsFactory metricsFactory)
    {
      this._orderRatesRepository = repository;
      this._metricsFactory = metricsFactory;
    }

    public bool SaveEPPSRates(string orderRates, string ratesRequest, string loanId)
    {
      return this._orderRatesRepository.SaveEPPSRates(orderRates, ratesRequest, loanId);
    }

    public object GetEppsRatesRequest(string eppsLoanId)
    {
      return this._orderRatesRepository.GetEppsRatesRequest(eppsLoanId);
    }

    internal void ApplyConfiguration(StorageSettings storageSettings, StorageMode storageMode)
    {
      this._settings = storageSettings;
      this._storageMode = storageMode;
      this._unitOfWork = IocContainer.CreateUnitOfWork((IStorageSettings) this._settings, this._storageMode);
    }

    public void Dispose()
    {
      GC.SuppressFinalize((object) this);
      this._unitOfWork.Dispose();
    }
  }
}
