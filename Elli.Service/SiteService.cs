// Decompiled with JetBrains decompiler
// Type: Elli.Service.SiteService
// Assembly: Elli.Service, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 23E1ED92-4ABE-46FE-97AA-A4B97F8619DF
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Service.dll

using Elli.Common.Data;
using Elli.Domain;
using Elli.Domain.Site;
using Elli.Metrics;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Settings;
using System;
using System.Collections.Generic;

#nullable disable
namespace Elli.Service
{
  public class SiteService : ISiteService, IDisposable
  {
    private readonly ISiteRepository _siteRepository;
    private StorageSettings _settings;
    private StorageMode _storageMode;
    private IUnitOfWork _unitOfWork;
    private readonly IMetricsFactory _metricsFactory;

    public SiteService(ISiteRepository repository, IMetricsFactory metricsFactory)
    {
      this._siteRepository = repository;
      this._metricsFactory = metricsFactory;
    }

    internal void ApplyConfiguration(StorageSettings storageSettings, StorageMode storageMode)
    {
      this._settings = storageSettings;
      this._storageMode = storageMode;
      this._unitOfWork = IocContainer.CreateUnitOfWork((IStorageSettings) this._settings, this._storageMode);
    }

    public bool CreateAdminRule(string siteId, Guid ruleId, string AdminRuleData)
    {
      return this._siteRepository.CreateAdminRule(siteId, ruleId, AdminRuleData);
    }

    public bool UpdateAdminRule(string siteId, Guid ruleId, string AdminRuleData)
    {
      return this._siteRepository.UpdateAdminRule(siteId, ruleId, AdminRuleData);
    }

    public string GetAdminRule(string siteId, Guid ruleId)
    {
      return this._siteRepository.GetAdminRule(siteId, ruleId);
    }

    public List<string> GetAdminRules(string siteId) => this._siteRepository.GetAdminRules(siteId);

    public bool DeleteAdminRule(string siteId, Guid ruleId)
    {
      return this._siteRepository.DeleteAdminRule(siteId, ruleId);
    }

    public bool UpdateAdminRuleOrder(string siteId, List<Guid> AdminRuleOrderIds)
    {
      return this._siteRepository.UpdateAdminRuleOrder(siteId, AdminRuleOrderIds);
    }

    public List<string> GetAdminRuleOrder(string siteId)
    {
      return this._siteRepository.GetAdminRuleOrder(siteId);
    }

    public void Dispose()
    {
      GC.SuppressFinalize((object) this);
      this._unitOfWork.Dispose();
    }
  }
}
