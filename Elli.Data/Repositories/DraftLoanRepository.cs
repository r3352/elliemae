// Decompiled with JetBrains decompiler
// Type: Elli.Data.Repositories.DraftLoanRepository
// Assembly: Elli.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 5199CF45-D8E1-4436-8A49-245565D9CA6B
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Data.dll

using Elli.Domain.Mortgage;
using EllieMae.EMLite.RemotingServices;
using MongoDB.Bson;
using System;
using System.Collections.Generic;

#nullable disable
namespace Elli.Data.Repositories
{
  public class DraftLoanRepository : IDraftLoanRepository
  {
    public bool CreateDraftLoan(string loanApplicationContract) => true;

    public object GetDraftLoan(Guid applicationId, bool blnIsCCAdmin) => (object) null;

    public object UpdateDraftLoan(Guid applicationId, string loanApplicationContract)
    {
      return (object) null;
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
      xTotalCount = 0L;
      return (List<string>) null;
    }

    public List<string> GetSubmitLoanGuid(
      UserInfo currentUser,
      int start,
      int limit,
      string sort,
      string filter)
    {
      return (List<string>) null;
    }

    public bool UpdateDraftLoanStatusForPublisher(
      Guid loanGuid,
      out string userId,
      bool blnIsCCAdmin)
    {
      userId = (string) null;
      return false;
    }

    public BsonDocument GetDraftLoanForSubscriber(Guid loanGuid) => (BsonDocument) null;

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
      return false;
    }

    public bool CheckDraftLoanExist(Guid applicationId) => false;
  }
}
