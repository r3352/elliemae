// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.LoanActionAccessBpmManager
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
using System.Collections.Specialized;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  public class LoanActionAccessBpmManager : BpmManager
  {
    internal static LoanActionAccessBpmManager Instance
    {
      get => Session.DefaultInstance.LoanActionAccessBpmManager;
    }

    internal static void RefreshInstance()
    {
      if (Session.DefaultInstance == null)
        return;
      Session.DefaultInstance.ResetLoanActionAccessBpmManager();
    }

    internal LoanActionAccessBpmManager(Sessions.Session session)
      : base(session, BizRuleType.LoanActionAccess, ClientSessionCacheID.BpmLoanActionAccess)
    {
    }

    public Hashtable GetLoanActionAccessRight(
      Persona[] personas,
      LoanConditions loanConditions,
      LoanData loan)
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
      LoanActionAccessRights[][] actionAccessRightsArray = new LoanActionAccessRights[11][];
      actionAccessRightsArray[0] = this.getLoanActionAccessRights(BizRule.Condition.Null, channelValue, -1, (string) null, (string) null);
      actionAccessRightsArray[1] = this.getLoanActionAccessRights(BizRule.Condition.LoanPurpose, channelValue, (int) loanPurposeValue, (string) null, (string) null);
      actionAccessRightsArray[2] = this.getLoanActionAccessRights(BizRule.Condition.LoanType, channelValue, (int) loanTypeValue, (string) null, (string) null);
      actionAccessRightsArray[3] = this.getLoanActionAccessRights(BizRule.Condition.LoanStatus, channelValue, (int) loanStatusValue, (string) null, (string) null);
      actionAccessRightsArray[4] = (LoanActionAccessRights[]) null;
      for (int index = finishedMilestones.Length - 1; index >= 0; --index)
      {
        actionAccessRightsArray[4] = this.getLoanActionAccessRights(BizRule.Condition.CurrentLoanAssocMS, channelValue, -1, finishedMilestones[index], finishedRoleIds[index]);
        if (actionAccessRightsArray[4] != null && actionAccessRightsArray[4].Length != 0)
          break;
      }
      if (actionAccessRightsArray[4] == null)
        actionAccessRightsArray[4] = new LoanActionAccessRights[0];
      actionAccessRightsArray[5] = this.getLoanActionAccessRights(BizRule.Condition.RateLock, channelValue, (int) rateLockValue, (string) null, (string) null);
      actionAccessRightsArray[6] = (LoanActionAccessRights[]) null;
      int num = 2;
      if (loanConditions.DataFromPlinth)
        num = 1;
      if (milestonesToCheck.Length - num >= 0)
      {
        for (int index = milestonesToCheck.Length - num; index >= 0; --index)
        {
          actionAccessRightsArray[6] = this.getLoanActionAccessRights(BizRule.Condition.FinishedMilestone, channelValue, -1, milestonesToCheck[index], (string) null);
          if (actionAccessRightsArray[6] != null && actionAccessRightsArray[6].Length != 0)
            break;
        }
      }
      if (actionAccessRightsArray[6] == null)
        actionAccessRightsArray[6] = new LoanActionAccessRights[0];
      actionAccessRightsArray[7] = this.getLoanActionAccessRights(BizRule.Condition.PropertyState, channelValue, (int) loanConditions.StateCodeValue, (string) null, (string) null);
      actionAccessRightsArray[8] = this.getLoanActionAccessRights(BizRule.Condition.LoanDocType, channelValue, (int) loanConditions.DocTypeCodeValue, (string) null, (string) null);
      actionAccessRightsArray[9] = this.getAdvancedConditionLoanActionAccessRights(channelValue, loan);
      actionAccessRightsArray[10] = !(loanConditions.LoanProgramName != string.Empty) ? new LoanActionAccessRights[0] : this.getLoanActionAccessRights(BizRule.Condition.LoanProgram, channelValue, -1, loanConditions.LoanProgramName, (string) null);
      Hashtable insensitiveHashtable = CollectionsUtil.CreateCaseInsensitiveHashtable();
      for (int index = 0; index < actionAccessRightsArray.Length; ++index)
      {
        foreach (LoanActionAccessRights actionAccessRights in actionAccessRightsArray[index])
        {
          object obj = insensitiveHashtable[(object) actionAccessRights.LoanActionID];
          if (obj == null)
            insensitiveHashtable[(object) actionAccessRights.LoanActionID] = (object) actionAccessRights.Clone();
          else
            ((LoanActionAccessRights) obj).Combine(actionAccessRights.AccessRights);
        }
      }
      string[] strArray = new string[insensitiveHashtable.Keys.Count];
      insensitiveHashtable.Keys.CopyTo((Array) strArray, 0);
      foreach (string key in strArray)
      {
        LoanActionAccessRights actionAccessRights = (LoanActionAccessRights) insensitiveHashtable[(object) key];
        BizRule.LoanActionAccessRight actionAccessRight1 = BizRule.LoanActionAccessRight.Enable;
        for (int index = 0; index < personas.Length; ++index)
        {
          object accessRight = actionAccessRights.AccessRights[(object) personas[index].ID];
          if (accessRight != null)
          {
            BizRule.LoanActionAccessRight actionAccessRight2 = (BizRule.LoanActionAccessRight) accessRight;
            if (actionAccessRight2 != BizRule.LoanActionAccessRight.Enable && (actionAccessRight2 > actionAccessRight1 || actionAccessRight1 == BizRule.LoanActionAccessRight.Enable))
              actionAccessRight1 = actionAccessRight2;
          }
          else if (personas[index].ID == 1)
            actionAccessRight1 = BizRule.LoanActionAccessRight.Enable;
        }
        insensitiveHashtable[(object) key] = (object) actionAccessRight1;
      }
      return insensitiveHashtable;
    }

    private LoanActionAccessRights[] getAdvancedConditionLoanActionAccessRights(
      string channelValue,
      LoanData loan)
    {
      List<LoanActionAccessRights> actionAccessRightsList = new List<LoanActionAccessRights>();
      ExecutionContext context = new ExecutionContext(this.session.UserInfo, loan, (IServerDataProvider) new CustomCodeSessionDataProvider(this.session.SessionObjects));
      foreach (ConditionEvaluator conditionEvaluator in this.GetConditionEvaluators())
      {
        if (conditionEvaluator.Rule.Condition == BizRule.Condition.AdvancedCoding && conditionEvaluator.AppliesTo(context) && conditionEvaluator.Rule.Condition2.IndexOf(channelValue) > -1)
          actionAccessRightsList.AddRange((IEnumerable<LoanActionAccessRights>) ((LoanActionAccessRuleInfo) conditionEvaluator.Rule).LoanActionAccessRights);
      }
      return actionAccessRightsList.ToArray();
    }

    private LoanActionAccessRights[] getLoanActionAccessRights(
      BizRule.Condition condition,
      string condition2,
      int conditionState,
      string milestoneID,
      string conditionState2)
    {
      BizRuleInfo[] activeRule = this.GetActiveRule(condition, condition2, conditionState, milestoneID, conditionState2);
      if (activeRule == null)
        return new LoanActionAccessRights[0];
      LoanActionAccessRuleInfo[] actionAccessRuleInfoArray = new LoanActionAccessRuleInfo[activeRule.Length];
      for (int index = 0; index < activeRule.Length; ++index)
        actionAccessRuleInfoArray[index] = (LoanActionAccessRuleInfo) activeRule[index];
      List<LoanActionAccessRights> actionAccessRightsList = new List<LoanActionAccessRights>();
      for (int index = 0; index < actionAccessRuleInfoArray.Length; ++index)
        actionAccessRightsList.AddRange((IEnumerable<LoanActionAccessRights>) actionAccessRuleInfoArray[index].LoanActionAccessRights);
      return actionAccessRightsList.ToArray();
    }
  }
}
