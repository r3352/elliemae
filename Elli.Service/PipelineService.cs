// Decompiled with JetBrains decompiler
// Type: Elli.Service.PipelineService
// Assembly: Elli.Service, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 23E1ED92-4ABE-46FE-97AA-A4B97F8619DF
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Service.dll

using Elli.Common.Data;
using Elli.Domain;
using Elli.Domain.Mortgage;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Settings;
using System;
using System.Collections.Generic;

#nullable disable
namespace Elli.Service
{
  public class PipelineService : IPipelineService, IDisposable
  {
    private IUnitOfWork _unitOfWork;
    private StorageSettings _storageSettings;
    private StorageMode _storageMode;
    private IPipelineRepository _pipelineRepository;

    public IList<PipelineItem> PipelineGetItems(
      IList<PipelineField> fields,
      string loanFolder,
      IList<PipelineSortField> sortFields,
      IList<FilterCriterion> filter,
      int maxResults,
      int pageIndex,
      int pageSize)
    {
      return (IList<PipelineItem>) null;
    }

    public PipelineService(IPipelineRepository pipelineRepository)
    {
      this._pipelineRepository = pipelineRepository;
    }

    internal void ApplyConfiguration(StorageSettings storageSettings, StorageMode storageMode)
    {
      this._storageSettings = storageSettings;
      this._storageMode = storageMode;
      this._unitOfWork = IocContainer.CreateUnitOfWork((IStorageSettings) this._storageSettings, this._storageMode);
    }

    public void Dispose()
    {
      GC.SuppressFinalize((object) this);
      this._unitOfWork.Dispose();
    }
  }
}
