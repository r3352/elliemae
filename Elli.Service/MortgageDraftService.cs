// Decompiled with JetBrains decompiler
// Type: Elli.Service.MortgageDraftService
// Assembly: Elli.Service, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 23E1ED92-4ABE-46FE-97AA-A4B97F8619DF
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Service.dll

using Elli.Domain;
using Elli.Domain.Mortgage;
using Elli.Metrics;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Settings;
using EllieMae.EMLite.RemotingServices;
using MongoDB.Bson;
using System;
using System.Collections.Generic;

#nullable disable
namespace Elli.Service
{
  public class MortgageDraftService : IMortgageDraftService, IDisposable
  {
    private readonly IDraftLoanRepository _draftLoanRepository;
    private IStorageSettings _storgaeSettings;
    private StorageMode _storageMode;
    private IUnitOfWork _unitOfWork;
    private readonly IMetricsFactory _metricsFactory;
    private ILoanMetricsRecorder _metricsRecorder;

    public MortgageDraftService(IDraftLoanRepository repository, IMetricsFactory metricsFactory)
    {
      this._draftLoanRepository = repository;
      this._metricsFactory = metricsFactory;
    }

    internal void ApplyConfiguration(IStorageSettings storageSettings, StorageMode storageMode)
    {
      this._storgaeSettings = storageSettings;
      this._storageMode = storageMode;
      this._unitOfWork = IocContainer.CreateUnitOfWork(this._storgaeSettings, this._storageMode);
    }

    private ILoanMetricsRecorder MetricsRecorder
    {
      get
      {
        return this._metricsRecorder != null ? this._metricsRecorder : (this._metricsRecorder = this._metricsFactory.CreateLoanMetricsRecorder(this._storgaeSettings.ClientId ?? string.Empty, this._storgaeSettings.InstanceId ?? string.Empty));
      }
    }

    public bool CreateDraftLoan(string loanApplicationContract)
    {
      return this._draftLoanRepository.CreateDraftLoan(loanApplicationContract);
    }

    public object GetDraftLoan(Guid applicationId, bool blnIsCCAdmin)
    {
      return this._draftLoanRepository.GetDraftLoan(applicationId, blnIsCCAdmin);
    }

    public object UpdateDraftLoan(Guid applicationId, string loanApplicationContract)
    {
      return this._draftLoanRepository.UpdateDraftLoan(applicationId, loanApplicationContract);
    }

    public List<string> GetDraftLoanSummary(
      UserInfo currentUser,
      int start,
      int limit,
      string sort,
      string filter,
      string status,
      bool blnIsCCAdmin,
      out long xTotalCount)
    {
      return this._draftLoanRepository.GetDraftLoanSummary(currentUser, start, limit, sort, filter, status, blnIsCCAdmin, out xTotalCount);
    }

    public BsonDocument GetDraftLoanForSubscriber(Guid loanGuid)
    {
      return this._draftLoanRepository.GetDraftLoanForSubscriber(loanGuid);
    }

    public bool UpdateDraftLoanStatusForPublisher(
      Guid applicationId,
      out string userId,
      bool blnIsCCAdmin)
    {
      return this._draftLoanRepository.UpdateDraftLoanStatusForPublisher(applicationId, out userId, blnIsCCAdmin);
    }

    public bool UpdateDraftLoanStatusForSubscriber(
      Guid loanGuid,
      string status,
      DateTime submDateTime,
      string errorMessage,
      bool isReSubmitAllowed,
      bool isFailed,
      bool isBorrowerActionRequired,
      bool isLenderActionRequired,
      bool isEncompassLevelActionRequired,
      string errorDetails)
    {
      return this._draftLoanRepository.UpdateDraftLoanStatusForSubscriber(loanGuid, status, submDateTime, errorMessage, isReSubmitAllowed, isFailed, isBorrowerActionRequired, isLenderActionRequired, isEncompassLevelActionRequired, errorDetails);
    }

    public bool CheckDraftLoanExist(Guid applicationId)
    {
      return this._draftLoanRepository.CheckDraftLoanExist(applicationId);
    }

    public void Dispose()
    {
      GC.SuppressFinalize((object) this);
      this._unitOfWork.Dispose();
    }
  }
}
