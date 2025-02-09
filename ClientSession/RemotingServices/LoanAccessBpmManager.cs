// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.LoanAccessBpmManager
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using EllieMae.EMLite.BusinessRules;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Customization;
using EllieMae.EMLite.DataEngine;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  public class LoanAccessBpmManager : BpmManager
  {
    internal static LoanAccessBpmManager Instance => Session.DefaultInstance.LoanAccessBpmManager;

    internal static void RefreshInstance()
    {
      if (Session.DefaultInstance == null)
        return;
      Session.DefaultInstance.ResetLoanAccessBpmManager();
    }

    internal LoanAccessBpmManager(Sessions.Session session)
      : base(session, BizRuleType.LoanAccess, ClientSessionCacheID.BpmLoanAccess)
    {
    }

    public Hashtable GetLoanAccessFields(
      Persona[] personas,
      LoanConditions loanConditions,
      LoanData loanData)
    {
      ArrayList ruleArray = this.getRuleArray(personas, loanConditions, loanData);
      Hashtable loanAccessFields = new Hashtable();
      for (int index1 = 0; index1 < personas.Length; ++index1)
      {
        for (int index2 = 0; index2 < ruleArray.Count; ++index2)
        {
          LoanAccessRights[] loanAccessRightsArray = (LoanAccessRights[]) ruleArray[index2];
          for (int index3 = 0; index3 < loanAccessRightsArray.Length; ++index3)
          {
            if (loanAccessRightsArray[index3] != null)
            {
              string[] editableFields = loanAccessRightsArray[index3].GetEditableFields(personas[index1].ID);
              if (editableFields != null)
              {
                for (int index4 = 0; index4 < editableFields.Length; ++index4)
                {
                  if (!loanAccessFields.ContainsKey((object) editableFields[index4]))
                    loanAccessFields.Add((object) editableFields[index4], (object) null);
                }
              }
            }
          }
        }
      }
      return loanAccessFields;
    }

    public int GetLoanAccessRight(
      Persona[] personas,
      LoanConditions loanConditions,
      LoanData loanData)
    {
      ArrayList ruleArray = this.getRuleArray(personas, loanConditions, loanData);
      int num1 = 16384;
      for (int index1 = 0; index1 < personas.Length; ++index1)
      {
        if (personas[index1].Name == "Administrator")
          return 16777215;
        int num2 = 16384;
        for (int index2 = 0; index2 < ruleArray.Count && num2 != 1; ++index2)
        {
          LoanAccessRights[] loanAccessRightsArray = (LoanAccessRights[]) ruleArray[index2];
          for (int index3 = 0; index3 < loanAccessRightsArray.Length; ++index3)
          {
            int num3 = 16384;
            if (loanAccessRightsArray[index3] != null)
              num3 = loanAccessRightsArray[index3].GetAccessRight(personas[index1].ID);
            if (num3 != 16384 && num3 != num2)
            {
              if (num3 == 1)
              {
                num2 = num3;
                break;
              }
              if (num2 == 16384)
              {
                num2 = num3;
              }
              else
              {
                int num4 = 0;
                for (int index4 = 0; index4 <= 7; ++index4)
                {
                  int num5 = 0;
                  int num6 = 0;
                  int num7 = 0;
                  int num8 = 0;
                  switch (index4)
                  {
                    case 0:
                      num5 = 2;
                      num6 = 64;
                      num7 = 1073741824;
                      num8 = 1073610752;
                      break;
                    case 1:
                      num5 = 4;
                      num6 = 256;
                      break;
                    case 2:
                      num5 = 8;
                      num6 = 512;
                      break;
                    case 3:
                      num5 = 16;
                      num6 = 1024;
                      break;
                    case 4:
                      num5 = 32;
                      num6 = 128;
                      break;
                    case 5:
                      num5 = 2048;
                      num6 = 4096;
                      break;
                    case 6:
                      num5 = 32768;
                      num6 = 65536;
                      break;
                    case 7:
                      num5 = 8192;
                      break;
                  }
                  if ((num3 & num2 & num5) != 0)
                    num4 |= num5;
                  else if ((num3 & num5) != 0 && (num2 & num7) != 0)
                    num4 |= num7 | num2 & num8;
                  else if ((num3 & num5) != 0 && (num2 & num6) != 0)
                    num4 |= num6;
                  else if ((num3 & num7) != 0 && (num2 & num5) != 0)
                    num4 |= num7 | num3 & num8;
                  else if ((num3 & num2 & num7) != 0)
                    num4 |= num7 | num3 & num2 & num8;
                  else if ((num3 & num7) != 0 && (num2 & num6) != 0)
                    num4 |= num6;
                  else if ((num3 & num6) != 0 && (num2 & num5) != 0)
                    num4 |= num6;
                  else if ((num3 & num6) != 0 && (num2 & num7) != 0)
                    num4 |= num6;
                  else if ((num3 & num2 & num6) != 0)
                    num4 |= num6;
                }
                if ((num4 & 131072) != 0)
                {
                  if (((num3 | num2) & 1048576) != 0)
                    num4 |= 1048576;
                  else if (((num3 | num2) & 524288) != 0)
                    num4 |= 524288;
                  else
                    num4 |= 262144;
                }
                if ((num4 & 2097152) != 0)
                {
                  if (((num3 | num2) & 16777216) != 0)
                    num4 |= 16777216;
                  else if (((num3 | num2) & 8388608) != 0)
                    num4 |= 8388608;
                  else
                    num4 |= 4194304;
                }
                num2 = num4;
              }
            }
          }
        }
        switch (num2)
        {
          case 16384:
            continue;
          case 16777215:
            return 16777215;
          default:
            if (num1 == 16384)
            {
              num1 = num2;
              continue;
            }
            num1 |= num2;
            continue;
        }
      }
      return num1 == 16384 ? 16777215 : num1;
    }

    private ArrayList getRuleArray(
      Persona[] personas,
      LoanConditions loanConditions,
      LoanData loanData)
    {
      BizRule.LoanPurpose loanPurposeValue = loanConditions.LoanPurposeValue;
      BizRule.LoanType loanTypeValue = loanConditions.LoanTypeValue;
      BizRule.LoanStatus loanStatusValue = loanConditions.LoanStatusValue;
      BizRule.RateLock rateLockValue = loanConditions.RateLockValue;
      string[] milestonesToCheck = loanConditions.MilestonesToCheck;
      string[] finishedMilestones = loanConditions.FinishedMilestones;
      string[] finishedRoleIds = loanConditions.FinishedRoleIDs;
      string channelValue = loanConditions.ChannelValue;
      if (milestonesToCheck == null || milestonesToCheck.Length == 0)
        throw new Exception("Invalid milestones");
      ArrayList ruleArray = new ArrayList();
      ruleArray.Add((object) this.getLoanAccessRights(BizRule.Condition.Null, channelValue, -1, (string) null, (string) null));
      ruleArray.Add((object) this.getLoanAccessRights(BizRule.Condition.LoanPurpose, channelValue, (int) loanPurposeValue, (string) null, (string) null));
      ruleArray.Add((object) this.getLoanAccessRights(BizRule.Condition.LoanType, channelValue, (int) loanTypeValue, (string) null, (string) null));
      ruleArray.Add((object) this.getLoanAccessRights(BizRule.Condition.LoanStatus, channelValue, (int) loanStatusValue, (string) null, (string) null));
      LoanAccessRights[] loanAccessRightsArray = (LoanAccessRights[]) null;
      ArrayList arrayList1 = new ArrayList();
      for (int index1 = finishedMilestones.Length - 1; index1 >= 0; --index1)
      {
        LoanAccessRights[] loanAccessRights = this.getLoanAccessRights(BizRule.Condition.CurrentLoanAssocMS, channelValue, -1, finishedMilestones[index1], finishedRoleIds[index1]);
        if (loanAccessRights != null && loanAccessRights.Length != 0)
        {
          for (int index2 = 0; index2 < loanAccessRights.Length; ++index2)
          {
            if (loanAccessRights[index2].Count > 0)
              arrayList1.Add((object) loanAccessRights);
          }
        }
        if (arrayList1.Count > 0)
          break;
      }
      if (arrayList1.Count > 0)
      {
        for (int index = 0; index < arrayList1.Count; ++index)
          ruleArray.Add((object) (LoanAccessRights[]) arrayList1[index]);
      }
      ruleArray.Add((object) this.getLoanAccessRights(BizRule.Condition.RateLock, channelValue, (int) rateLockValue, (string) null, (string) null));
      loanAccessRightsArray = (LoanAccessRights[]) null;
      ArrayList arrayList2 = new ArrayList();
      for (int index3 = finishedMilestones.Length - 1; index3 >= 0; --index3)
      {
        LoanAccessRights[] loanAccessRights = this.getLoanAccessRights(BizRule.Condition.FinishedMilestone, channelValue, -1, finishedMilestones[index3], (string) null);
        if (loanAccessRights != null && loanAccessRights.Length != 0)
        {
          for (int index4 = 0; index4 < loanAccessRights.Length; ++index4)
          {
            if (loanAccessRights[index4].Count > 0)
              arrayList2.Add((object) loanAccessRights);
          }
        }
        if (arrayList2.Count > 0)
          break;
      }
      if (arrayList2.Count > 0)
      {
        for (int index = 0; index < arrayList2.Count; ++index)
          ruleArray.Add((object) (LoanAccessRights[]) arrayList2[index]);
      }
      ruleArray.Add((object) this.getLoanAccessRights(BizRule.Condition.PropertyState, channelValue, (int) loanConditions.StateCodeValue, (string) null, (string) null));
      ruleArray.Add((object) this.getLoanAccessRights(BizRule.Condition.LoanDocType, channelValue, (int) loanConditions.DocTypeCodeValue, (string) null, (string) null));
      if (loanData != null && this.HasAdvancedConditionRules())
        ruleArray.Add((object) this.getAdvancedConditionLoanAccessRights(channelValue, loanData));
      if (loanConditions.LoanProgramName != string.Empty)
        ruleArray.Add((object) this.getLoanAccessRights(BizRule.Condition.LoanProgram, channelValue, -1, loanConditions.LoanProgramName, (string) null));
      return ruleArray;
    }

    public bool HasAdvancedConditionRules()
    {
      foreach (BizRuleInfo activeRule in this.GetActiveRules())
      {
        if (activeRule.Condition == BizRule.Condition.AdvancedCoding)
          return true;
      }
      return false;
    }

    private LoanAccessRights[] getLoanAccessRights(
      BizRule.Condition condition,
      string condition2,
      int conditionState,
      string milestoneID,
      string conditionState2)
    {
      BizRuleInfo[] activeRule = this.GetActiveRule(condition, condition2, conditionState, milestoneID, conditionState2);
      if (activeRule == null)
        return new LoanAccessRights[1]
        {
          new LoanAccessRights(new PersonaLoanAccessRight[0])
        };
      LoanAccessRuleInfo[] loanAccessRuleInfoArray = new LoanAccessRuleInfo[activeRule.Length];
      for (int index = 0; index < activeRule.Length; ++index)
        loanAccessRuleInfoArray[index] = (LoanAccessRuleInfo) activeRule[index];
      LoanAccessRights[] loanAccessRights = new LoanAccessRights[loanAccessRuleInfoArray.Length];
      for (int index = 0; index < loanAccessRuleInfoArray.Length; ++index)
        loanAccessRights[index] = new LoanAccessRights(loanAccessRuleInfoArray[index].LoanAccessRights);
      return loanAccessRights;
    }

    private LoanAccessRights[] getAdvancedConditionLoanAccessRights(
      string channelValue,
      LoanData loan)
    {
      List<LoanAccessRights> loanAccessRightsList = new List<LoanAccessRights>();
      ExecutionContext context = new ExecutionContext(this.session.UserInfo, loan, (IServerDataProvider) new CustomCodeSessionDataProvider(this.session.SessionObjects));
      foreach (ConditionEvaluator conditionEvaluator in this.GetConditionEvaluators())
      {
        if (conditionEvaluator.Rule.Condition == BizRule.Condition.AdvancedCoding && conditionEvaluator.AppliesTo(context) && conditionEvaluator.Rule.Condition2.IndexOf(channelValue) > -1)
          loanAccessRightsList.Add(new LoanAccessRights(((LoanAccessRuleInfo) conditionEvaluator.Rule).LoanAccessRights));
      }
      return loanAccessRightsList.ToArray();
    }

    public LoanContentAccess GetLoanContentAccess(PipelineInfo pInfo)
    {
      return this.GetLoanContentAccess(pInfo, (LoanData) null);
    }

    public LoanContentAccess GetLoanContentAccess(LoanData loan)
    {
      return this.GetLoanContentAccess(loan.ToPipelineInfo(), loan);
    }

    public LoanContentAccess GetLoanContentAccess(PipelineInfo pInfo, LoanData loanData)
    {
      if (UserInfo.IsSuperAdministrator(this.session.UserID, this.session.UserInfo.UserPersonas))
        return LoanContentAccess.FullAccess;
      BizRule.LoanAccessRight loanAccessRight = (BizRule.LoanAccessRight) this.GetLoanAccessRight(this.session.UserInfo.UserPersonas, LoanBusinessRuleInfo.PipelineInfoForBusinessRule(pInfo), loanData);
      switch (loanAccessRight)
      {
        case BizRule.LoanAccessRight.ViewAllOnly:
          return LoanContentAccess.None;
        case BizRule.LoanAccessRight.EditAll:
          return LoanContentAccess.FullAccess;
        default:
          LoanContentAccess loanContentAccess = LoanContentAccess.None;
          if ((loanAccessRight & BizRule.LoanAccessRight.DocTracking) == BizRule.LoanAccessRight.DocTracking)
            loanContentAccess |= LoanContentAccess.DocumentTracking;
          else if ((loanAccessRight & BizRule.LoanAccessRight.DocTrackingPartial) == BizRule.LoanAccessRight.DocTrackingPartial)
          {
            loanContentAccess |= LoanContentAccess.DocTrackingPartial;
            if ((loanAccessRight & BizRule.LoanAccessRight.DocTrackingUnassignedFiles) == BizRule.LoanAccessRight.DocTrackingUnassignedFiles)
              loanContentAccess |= LoanContentAccess.DocTrackingUnassignedFiles;
            if ((loanAccessRight & BizRule.LoanAccessRight.DocTrackingUnprotectedDocs) == BizRule.LoanAccessRight.DocTrackingUnprotectedDocs)
              loanContentAccess |= LoanContentAccess.DocTrackingUnprotectedDocs;
            if ((loanAccessRight & BizRule.LoanAccessRight.DocTrackingProtectedDocs) == BizRule.LoanAccessRight.DocTrackingProtectedDocs)
              loanContentAccess |= LoanContentAccess.DocTrackingProtectedDocs;
            if ((loanAccessRight & BizRule.LoanAccessRight.DocTrackingCreateDocs) == BizRule.LoanAccessRight.DocTrackingCreateDocs)
              loanContentAccess |= LoanContentAccess.DocTrackingCreateDocs;
            if ((loanAccessRight & BizRule.LoanAccessRight.DocTrackingOrderDisclosures) == BizRule.LoanAccessRight.DocTrackingOrderDisclosures)
              loanContentAccess |= LoanContentAccess.DocTrackingOrderDisclosures;
            if ((loanAccessRight & BizRule.LoanAccessRight.DocTrackingRequestRetrieveBorrower) == BizRule.LoanAccessRight.DocTrackingRequestRetrieveBorrower)
            {
              loanContentAccess |= LoanContentAccess.DocTrackingRequestRetrieveBorrower;
              if ((loanAccessRight & BizRule.LoanAccessRight.DocTrackingRetrieveBorrowerCurrent) == BizRule.LoanAccessRight.DocTrackingRetrieveBorrowerCurrent)
                loanContentAccess |= LoanContentAccess.DocTrackingRetrieveBorrowerCurrent;
              else if ((loanAccessRight & BizRule.LoanAccessRight.DocTrackingRetrieveBorrowerNotCurrent) == BizRule.LoanAccessRight.DocTrackingRetrieveBorrowerNotCurrent)
                loanContentAccess |= LoanContentAccess.DocTrackingRetrieveBorrowerNotCurrent;
              else if ((loanAccessRight & BizRule.LoanAccessRight.DocTrackingRetrieveBorrowerUnassigned) == BizRule.LoanAccessRight.DocTrackingRetrieveBorrowerUnassigned)
                loanContentAccess |= LoanContentAccess.DocTrackingRetrieveBorrowerUnassigned;
            }
            if ((loanAccessRight & BizRule.LoanAccessRight.DocTrackingRequestRetrieveService) == BizRule.LoanAccessRight.DocTrackingRequestRetrieveService)
            {
              loanContentAccess |= LoanContentAccess.DocTrackingRequestRetrieveService;
              if ((loanAccessRight & BizRule.LoanAccessRight.DocTrackingRetrieveServiceCurrent) == BizRule.LoanAccessRight.DocTrackingRetrieveServiceCurrent)
                loanContentAccess |= LoanContentAccess.DocTrackingRetrieveServiceCurrent;
              else if ((loanAccessRight & BizRule.LoanAccessRight.DocTrackingRetrieveServiceNotCurrent) == BizRule.LoanAccessRight.DocTrackingRetrieveServiceNotCurrent)
                loanContentAccess |= LoanContentAccess.DocTrackingRetrieveServiceNotCurrent;
              else if ((loanAccessRight & BizRule.LoanAccessRight.DocTrackingRetrieveServiceUnassigned) == BizRule.LoanAccessRight.DocTrackingRetrieveServiceUnassigned)
                loanContentAccess |= LoanContentAccess.DocTrackingRetrieveServiceUnassigned;
            }
          }
          else if ((loanAccessRight & BizRule.LoanAccessRight.DocTrackingViewOnly) == BizRule.LoanAccessRight.DocTrackingViewOnly)
            loanContentAccess |= LoanContentAccess.DocTrackingViewOnly;
          if ((loanAccessRight & BizRule.LoanAccessRight.ConversationLog) == BizRule.LoanAccessRight.ConversationLog)
            loanContentAccess |= LoanContentAccess.ConversationLog;
          else if ((loanAccessRight & BizRule.LoanAccessRight.ConversationLogViewOnly) == BizRule.LoanAccessRight.ConversationLogViewOnly)
            loanContentAccess |= LoanContentAccess.ConversationLogViewOnly;
          if ((loanAccessRight & BizRule.LoanAccessRight.Task) == BizRule.LoanAccessRight.Task)
            loanContentAccess |= LoanContentAccess.Task;
          else if ((loanAccessRight & BizRule.LoanAccessRight.TaskViewOnly) == BizRule.LoanAccessRight.TaskViewOnly)
            loanContentAccess |= LoanContentAccess.TaskViewOnly;
          if ((loanAccessRight & BizRule.LoanAccessRight.ProfitMgmt) == BizRule.LoanAccessRight.ProfitMgmt)
            loanContentAccess |= LoanContentAccess.ProfitManagement;
          else if ((loanAccessRight & BizRule.LoanAccessRight.ProfitMgmtViewOnly) == BizRule.LoanAccessRight.ProfitMgmtViewOnly)
            loanContentAccess |= LoanContentAccess.ProfitMgmtViewOnly;
          if ((loanAccessRight & BizRule.LoanAccessRight.ConditionTracking) == BizRule.LoanAccessRight.ConditionTracking)
            loanContentAccess |= LoanContentAccess.ConditionTracking;
          else if ((loanAccessRight & BizRule.LoanAccessRight.ConditionTrackingViewOnly) == BizRule.LoanAccessRight.ConditionTrackingViewOnly)
            loanContentAccess |= LoanContentAccess.ConditionTrackingViewOnly;
          if ((loanAccessRight & BizRule.LoanAccessRight.LockRequest) == BizRule.LoanAccessRight.LockRequest)
            loanContentAccess |= LoanContentAccess.LockRequest;
          else if ((loanAccessRight & BizRule.LoanAccessRight.LockRequestViewOnly) == BizRule.LoanAccessRight.LockRequestViewOnly)
            loanContentAccess |= LoanContentAccess.LockRequestViewOnly;
          if ((loanAccessRight & BizRule.LoanAccessRight.FormFields) == BizRule.LoanAccessRight.FormFields)
            loanContentAccess |= LoanContentAccess.FormFields;
          if ((loanAccessRight & BizRule.LoanAccessRight.DisclosureTracking) == BizRule.LoanAccessRight.DisclosureTracking)
            loanContentAccess |= LoanContentAccess.DisclosureTracking;
          else if ((loanAccessRight & BizRule.LoanAccessRight.DisclosureTrackingViewOnly) == BizRule.LoanAccessRight.DisclosureTrackingViewOnly)
            loanContentAccess |= LoanContentAccess.DisclosureTrackingViewOnly;
          return loanContentAccess;
      }
    }

    public LoanInfo.Right GetEffectiveRightsForLoan(PipelineInfo pinfo)
    {
      return this.GetEffectiveRightsForLoans(new PipelineInfo[1]
      {
        pinfo
      })[0];
    }

    public LoanInfo.Right[] GetEffectiveRightsForLoans(PipelineInfo[] pinfos)
    {
      LoanInfo.Right[] effectiveRightsForLoans = new LoanInfo.Right[pinfos.Length];
      ArrayList arrayList1 = new ArrayList(pinfos.Length);
      ArrayList arrayList2 = new ArrayList(pinfos.Length);
      for (int index = 0; index < pinfos.Length; ++index)
      {
        if (pinfos[index] == null)
          effectiveRightsForLoans[index] = LoanInfo.Right.NoRight;
        else if (pinfos[index].Rights != null && pinfos[index].Rights.ContainsKey((object) this.session.UserID))
          effectiveRightsForLoans[index] = (LoanInfo.Right) pinfos[index].Rights[(object) this.session.UserID];
        else if (this.session.UserInfo.IsSuperAdministrator())
        {
          effectiveRightsForLoans[index] = LoanInfo.Right.FullRight;
        }
        else
        {
          arrayList1.Add((object) pinfos[index].GUID);
          arrayList2.Add((object) index);
        }
      }
      if (arrayList1.Count > 0)
      {
        LoanInfo.Right[] loanAccessRights = this.session.LoanManager.GetEffectiveLoanAccessRights((string[]) arrayList1.ToArray(typeof (string)));
        for (int index = 0; index < loanAccessRights.Length; ++index)
          effectiveRightsForLoans[(int) arrayList2[index]] = loanAccessRights[index];
      }
      return effectiveRightsForLoans;
    }
  }
}
