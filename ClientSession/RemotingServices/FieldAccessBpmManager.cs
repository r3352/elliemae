// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.FieldAccessBpmManager
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
  public class FieldAccessBpmManager : BpmManager
  {
    internal static FieldAccessBpmManager Instance => Session.DefaultInstance.FieldAccessBpmManager;

    internal static void RefreshInstance()
    {
      if (Session.DefaultInstance == null)
        return;
      Session.DefaultInstance.ResetFieldAccessBpmManager();
    }

    internal FieldAccessBpmManager(Sessions.Session session)
      : base(session, BizRuleType.FieldAccess, ClientSessionCacheID.BpmFieldAccess)
    {
    }

    public Hashtable GetFieldAccessRight(
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
      FieldAccessRights[][] fieldAccessRightsArray = new FieldAccessRights[11][];
      fieldAccessRightsArray[0] = this.getFieldAccessRights(BizRule.Condition.Null, channelValue, -1, (string) null, (string) null);
      fieldAccessRightsArray[1] = this.getFieldAccessRights(BizRule.Condition.LoanPurpose, channelValue, (int) loanPurposeValue, (string) null, (string) null);
      fieldAccessRightsArray[2] = this.getFieldAccessRights(BizRule.Condition.LoanType, channelValue, (int) loanTypeValue, (string) null, (string) null);
      fieldAccessRightsArray[3] = this.getFieldAccessRights(BizRule.Condition.LoanStatus, channelValue, (int) loanStatusValue, (string) null, (string) null);
      fieldAccessRightsArray[4] = (FieldAccessRights[]) null;
      for (int index = finishedMilestones.Length - 1; index >= 0; --index)
      {
        fieldAccessRightsArray[4] = this.getFieldAccessRights(BizRule.Condition.CurrentLoanAssocMS, channelValue, -1, finishedMilestones[index], finishedRoleIds[index]);
        if (fieldAccessRightsArray[4] != null && fieldAccessRightsArray[4].Length != 0)
          break;
      }
      if (fieldAccessRightsArray[4] == null)
        fieldAccessRightsArray[4] = new FieldAccessRights[0];
      fieldAccessRightsArray[5] = this.getFieldAccessRights(BizRule.Condition.RateLock, channelValue, (int) rateLockValue, (string) null, (string) null);
      fieldAccessRightsArray[6] = (FieldAccessRights[]) null;
      int num = 2;
      if (loanConditions.DataFromPlinth)
        num = 1;
      if (milestonesToCheck.Length - num >= 0)
      {
        for (int index = milestonesToCheck.Length - num; index >= 0; --index)
        {
          fieldAccessRightsArray[6] = this.getFieldAccessRights(BizRule.Condition.FinishedMilestone, channelValue, -1, milestonesToCheck[index], (string) null);
          if (fieldAccessRightsArray[6] != null && fieldAccessRightsArray[6].Length != 0)
            break;
        }
      }
      if (fieldAccessRightsArray[6] == null)
        fieldAccessRightsArray[6] = new FieldAccessRights[0];
      fieldAccessRightsArray[7] = this.getFieldAccessRights(BizRule.Condition.PropertyState, channelValue, (int) loanConditions.StateCodeValue, (string) null, (string) null);
      fieldAccessRightsArray[8] = this.getFieldAccessRights(BizRule.Condition.LoanDocType, channelValue, (int) loanConditions.DocTypeCodeValue, (string) null, (string) null);
      fieldAccessRightsArray[9] = this.getAdvancedConditionFieldAccessRights(channelValue, loan);
      fieldAccessRightsArray[10] = !(loanConditions.LoanProgramName != string.Empty) ? new FieldAccessRights[0] : this.getFieldAccessRights(BizRule.Condition.LoanProgram, channelValue, -1, loanConditions.LoanProgramName, (string) null);
      Hashtable insensitiveHashtable = CollectionsUtil.CreateCaseInsensitiveHashtable();
      for (int index = 0; index < fieldAccessRightsArray.Length; ++index)
      {
        foreach (FieldAccessRights fieldAccessRights in fieldAccessRightsArray[index])
        {
          object obj = insensitiveHashtable[(object) fieldAccessRights.FieldID];
          if (obj == null)
            insensitiveHashtable[(object) fieldAccessRights.FieldID] = (object) fieldAccessRights.Clone();
          else
            ((FieldAccessRights) obj).Combine(fieldAccessRights.AccessRights);
        }
      }
      string[] strArray = new string[insensitiveHashtable.Keys.Count];
      insensitiveHashtable.Keys.CopyTo((Array) strArray, 0);
      foreach (string key in strArray)
      {
        FieldAccessRights fieldAccessRights = (FieldAccessRights) insensitiveHashtable[(object) key];
        BizRule.FieldAccessRight fieldAccessRight1 = BizRule.FieldAccessRight.DoesNotApply;
        for (int index = 0; index < personas.Length; ++index)
        {
          object accessRight = fieldAccessRights.AccessRights[(object) personas[index].ID];
          if (accessRight != null)
          {
            BizRule.FieldAccessRight fieldAccessRight2 = (BizRule.FieldAccessRight) accessRight;
            if (fieldAccessRight2 != BizRule.FieldAccessRight.DoesNotApply && (fieldAccessRight2 > fieldAccessRight1 || fieldAccessRight1 == BizRule.FieldAccessRight.DoesNotApply))
              fieldAccessRight1 = fieldAccessRight2;
          }
          else if (personas[index].ID == 1)
            fieldAccessRight1 = BizRule.FieldAccessRight.Edit;
        }
        insensitiveHashtable[(object) key] = (object) fieldAccessRight1;
      }
      return insensitiveHashtable;
    }

    private FieldAccessRights[] getAdvancedConditionFieldAccessRights(
      string channelValue,
      LoanData loan)
    {
      List<FieldAccessRights> fieldAccessRightsList = new List<FieldAccessRights>();
      ExecutionContext context = new ExecutionContext(this.session.UserInfo, loan, (IServerDataProvider) new CustomCodeSessionDataProvider(this.session.SessionObjects));
      foreach (ConditionEvaluator conditionEvaluator in this.GetConditionEvaluators())
      {
        if (conditionEvaluator.Rule.Condition == BizRule.Condition.AdvancedCoding && conditionEvaluator.AppliesTo(context) && conditionEvaluator.Rule.Condition2.IndexOf(channelValue) > -1)
          fieldAccessRightsList.AddRange((IEnumerable<FieldAccessRights>) ((FieldAccessRuleInfo) conditionEvaluator.Rule).FieldAccessRights);
      }
      return fieldAccessRightsList.ToArray();
    }

    private FieldAccessRights[] getFieldAccessRights(
      BizRule.Condition condition,
      string condition2,
      int conditionState,
      string milestoneID,
      string conditionState2)
    {
      BizRuleInfo[] activeRule = this.GetActiveRule(condition, condition2, conditionState, milestoneID, conditionState2);
      if (activeRule == null)
        return new FieldAccessRights[0];
      FieldAccessRuleInfo[] fieldAccessRuleInfoArray = new FieldAccessRuleInfo[activeRule.Length];
      for (int index = 0; index < activeRule.Length; ++index)
        fieldAccessRuleInfoArray[index] = (FieldAccessRuleInfo) activeRule[index];
      List<FieldAccessRights> fieldAccessRightsList = new List<FieldAccessRights>();
      for (int index = 0; index < fieldAccessRuleInfoArray.Length; ++index)
        fieldAccessRightsList.AddRange((IEnumerable<FieldAccessRights>) fieldAccessRuleInfoArray[index].FieldAccessRights);
      return fieldAccessRightsList.ToArray();
    }
  }
}
