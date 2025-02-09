// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ServiceInterface.IConditionService
// Assembly: ServiceInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF0C2B89-A027-4FA0-9669-4D2AA36A4D74
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ServiceInterface.dll

using Elli.EncompassPlatform.Common.DataContracts;
using Elli.EncompassPlatform.Common.SoapMessages;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.ServiceInterface
{
  public interface IConditionService : IContextBoundObject
  {
    ConditionDocumentsAssignResponse AssignConditionDocuments(
      ConditionDocumentsAssignRequest request);

    PreliminaryConditionsCreateResponse CreatePreliminaryConditions(
      PreliminaryConditionsCreateRequest request);

    PreliminaryConditionsGetResponse GetPreliminaryConditions(
      PreliminaryConditionsGetRequest request);

    UnderwritingConditionsCreateResponse CreateUnderwritingConditions(
      UnderwritingConditionsCreateRequest request);

    UnderwritingConditionsGetResponse GetUnderwritingConditions(
      UnderwritingConditionsGetRequest request);

    PreliminaryConditionsSaveResponse SavePreliminaryConditions(
      PreliminaryConditionsSaveRequest request);

    UnderwritingConditionsSaveResponse SaveUnderwritingConditions(
      UnderwritingConditionsSaveRequest request);

    ConditionCommentsCreateResponse CreateComments(ConditionCommentsCreateRequest request);

    PreliminaryConditionLog CreatePreliminaryConditionLog(
      LoanData loanData,
      PreliminaryConditionCreateContract condContract,
      Hashtable userAclFeatureRights);

    UnderwritingConditionLog CreateUnderwritingConditionLog(
      LoanData loanData,
      UnderwritingConditionCreateContract condContract,
      Hashtable userAclFeatureRights);
  }
}
