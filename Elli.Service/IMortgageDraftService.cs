// Decompiled with JetBrains decompiler
// Type: Elli.Service.IMortgageDraftService
// Assembly: Elli.Service, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 23E1ED92-4ABE-46FE-97AA-A4B97F8619DF
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Service.dll

using EllieMae.EMLite.RemotingServices;
using MongoDB.Bson;
using System;
using System.Collections.Generic;

#nullable disable
namespace Elli.Service
{
  public interface IMortgageDraftService
  {
    bool CreateDraftLoan(string loanApplicationContract);

    object GetDraftLoan(Guid applicationId, bool blnIsCCAdmin);

    object UpdateDraftLoan(Guid applicationId, string loanApplicationContract);

    List<string> GetDraftLoanSummary(
      UserInfo currentUser,
      int start,
      int limit,
      string sort,
      string filter,
      string status,
      bool blnIsCCAdmin,
      out long xTotalCount);

    BsonDocument GetDraftLoanForSubscriber(Guid loanGuid);

    bool UpdateDraftLoanStatusForPublisher(
      Guid applicationId,
      out string userId,
      bool blnIsCCAdmin);

    bool UpdateDraftLoanStatusForSubscriber(
      Guid loanGuid,
      string status,
      DateTime submDateTime,
      string errorMessage,
      bool isReSubmitAllowed,
      bool isFailed,
      bool isBorrowerActionRequired,
      bool isLenderActionRequired,
      bool isEncompassLevelActionRequired,
      string errorDetails);

    bool CheckDraftLoanExist(Guid applicationId);
  }
}
