// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ServiceInterface.ILoanService
// Assembly: ServiceInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF0C2B89-A027-4FA0-9669-4D2AA36A4D74
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ServiceInterface.dll

using Elli.BusinessRules;
using Elli.Domain.BusinessRule;
using Elli.Domain.Mortgage;
using Elli.ElliEnum;
using Elli.Service.Common;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.LoanUtils.Workflow;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.ServiceInterface.Results;
using EllieMae.EMLite.ServiceInterface.Services;
using EllieMae.EMLite.Workflow;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ServiceInterface
{
  public interface ILoanService : IContextBoundObject
  {
    Loan NewLoan(
      bool isLoanProcessTraceEnabled = false,
      bool forImport = false,
      bool generateLoanNumber = false,
      bool isTpoSession = false,
      string _loid = "",
      string loanFolder = "");

    Loan GetLoan(Guid encompassLoanId, bool isLoanProcessTraceEnabled = false);

    LoanData GetLoanDataWithLoanAccessRules(Loan loan);

    Loan GetLoanWithMissingEntities(Guid encompassLoanId, bool isLoanProcessTraceEnabled = false);

    Loan GetDraftLoanWithMissingEntities(
      UserInfo userInfo,
      Guid encompassLoanId,
      bool isLoanProcessTraceEnabled = false);

    LoanSaveResult CreateLoan(
      Loan loan,
      string loanFolder,
      string sessionId,
      LoanCalculateOption loanCalculateOption = LoanCalculateOption.Default,
      InternalFieldPopulationOption internalFieldPopulationOption = InternalFieldPopulationOption.None,
      bool hasEntities = false);

    LoanGetResult GetLoanResult(
      Guid loanId,
      LoanSecurityOptions options = LoanSecurityOptions.ApplyFieldPolicyRules,
      bool isLoanProcessTraceEnabled = false);

    LoanGetResult GetLoanResult(
      Guid loanId,
      out LoanData loanData,
      bool shouldHideFields,
      LoanSecurityOptions options = LoanSecurityOptions.ApplyFieldPolicyRules,
      bool isLoanProcessTraceEnabled = false);

    LoanGetResult GetLoanResultWithSnapshotFields(
      Guid encompassLoanId,
      LoanSecurityOptions options = LoanSecurityOptions.ApplyFieldPolicyRules,
      bool isLoanProcessTraceEnabled = false);

    List<FieldAccessRule> GetPersonaAccessToFieldRules(Loan loan, LoanSecurityOptions options = LoanSecurityOptions.ApplyFieldPolicyRules);

    DraftLoanGetResult GetDraftLoanResult(
      UserInfo userInfo,
      Guid loanId,
      LoanSecurityOptions options = LoanSecurityOptions.ApplyFieldPolicyRules,
      bool isLoanProcessTraceEnabled = false);

    DraftLoanGetResult GetDraftLoanAncillaryResult(Loan loan, LoanSecurityOptions options = LoanSecurityOptions.ApplyFieldPolicyRules);

    LoanGetResult GetLoanAncillaryResult(
      Loan loan,
      LoanSecurityOptions options = LoanSecurityOptions.ApplyFieldPolicyRules,
      LoanInfo.Right? userRights = null);

    LoanSaveResult SaveLoan(
      Loan loan,
      LoanProcessOption loanProcessOptions = LoanProcessOption.Default,
      LoanCalculateOption loanCalculateOption = LoanCalculateOption.Default,
      InternalFieldPopulationOption internalFieldPopulationOption = InternalFieldPopulationOption.None,
      bool hasEntities = false);

    Guid ImportLoan(string loanFile, string fileFormatType, string loanFolder, string sessionId);

    string LoanExport(Guid encompassGuid, string fileFormatType, string sessionId);

    Elli.Domain.Locking.LoanLock LoanLock(Guid loanId, string sessionId);

    bool CancelRateLockWithoutRequest(
      LoanData loanData,
      UserInfo cancellingUser,
      DateTime cancellationDate,
      string comment);

    Elli.Domain.Locking.LoanLock DraftLoanLock(Guid loanId, string sessionId);

    void ArchiveLoan(string fileData, string actionType);

    void LoanDeleteEncompassId(Guid encompassGuid);

    Tuple<bool, string, string> LoanUnlock(Guid loanId, string sessionId, bool force);

    void LoanCalculate(
      Loan loan,
      LoanProcessOption loanProcessOptions = LoanProcessOption.Default,
      LoanCalculateOption loanCalculateOption = LoanCalculateOption.Default,
      InternalFieldPopulationOption internalFieldPopulationOption = InternalFieldPopulationOption.None);

    string RateLockSubmit(Loan loan);

    MilestoneValidationError CompleteMilestone(
      Loan loan,
      NextMilestoneUserType userType,
      string nextMilestoneUserName,
      bool applyMatchingMilestoneTemplate = true,
      LoanPersistent loanPersistent = LoanPersistent.Permanent,
      string milestoneLogId = "",
      bool ignoreWorkflowTasks = false);

    ILoanSettings GetLoanSettings();

    ILoanConfigurationInfo GetLoanConfigurationInfo(string userId);

    LoanData CheckOutInLoanData(string loanId);

    bool ACL_IsAuthorizedForFeature(AclFeature feature);

    bool ACL_IsAuthorizedForFeature(AclFeature feature, int personaID);

    ILoanManager GetLoanManager();

    IBpmManager GetBpmManager();

    IConfigurationManager GetConfigurationManager();

    IList<LoanAccessRights> GetAdvancedConditionLoanAccessRights(string channelValue, LoanData loan);

    LoanProperties GetLoanProperties(IServicePrincipal principal, PipelineInfo pinfo);

    IList<EPassMessageInfo> GetLoanMessages(IServicePrincipal principal, string loanGuid);

    IList<LoanDuplicationTemplateAclInfo> GetAccessibleLoanDuplicationTemplates(
      IServicePrincipal principal);

    LoanContentAccess GetLoanContentAccess(PipelineInfo pInfo, LoanData loanData);

    void DumpLoanFields(Loan loan, string fileName);

    string CreateLoanCheckOut(
      LoanData loanData,
      string loanFolder,
      UserInfo userInfo,
      Loan loan,
      string sessionId,
      out Guid guid,
      string triggerLog = null,
      bool allowDeferrable = false,
      string auditUserId = null);

    void ApplyBestMatchingMilestoneTemplate(LoanData loanData, SessionObjects sessionObjects);

    MilestoneTemplate GetBestMatchingMilestoneTemplate(
      LoanData loanData,
      SessionObjects sessionObjects);

    string SaveLoanCheckOut(
      LoanData loanData,
      string sessionId,
      string triggerLog = null,
      bool allowDeferrable = false,
      string auditUserId = null);

    string SaveLoanCheckOutDraft(LoanData loanData, string sessionId, string triggerLog = null);

    bool HandleDeferredLoanProcessAsSubscriber(IMessage message, UserInfo userInfo);

    LoanIdentity GetLoanIdentity(string loanId);

    Loan GetDeferredLoan(string encompassLoanId, string loanPath, string loanFileName);

    IFieldAccessPolicy GetFieldAccessPolicy(Loan loan);

    BizRuleInfo[] GetBizRuleInfosByType(BizRuleType type);

    FieldAccessRuleInfo[] GetActiveFieldAccessRulesByPersona(int[] personaIds);

    string LoanAppSubmitCheckOut(
      LoanData loanData,
      string loanFolder,
      UserInfo userInfo,
      Loan loan,
      string sessionId,
      out Guid guid,
      string triggerLog = null);

    MilestoneTemplateEvaluator EvaluateMilestoneTemplateChanges(
      LoanData loanData,
      string existingMilestoneTemplateId,
      string newMilestoneTemplateId);

    IList<LockInfo> GetLoanLocks(string loanId);

    Elli.Domain.Locking.LoanLock LoanLock(
      Guid loanId,
      string sessionId,
      LockInfo.ExclusiveLock lockType);

    List<UserInfo> GetAssociatedUsers(Loan loan, int roleId);

    EllieMae.EMLite.ClientServer.LoanAssociateInfo[] GetLoanAssociatedUsers(
      string loanId,
      string userId);
  }
}
