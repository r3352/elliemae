// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Bpm.FieldAccessRuleEvaluator
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Bpm
{
  public class FieldAccessRuleEvaluator(FieldAccessRuleInfo[] rules) : BpmRuleEvaluator((BizRuleInfo[]) rules)
  {
    protected override object EvaluateConditions(Persona[] personas, LoanConditions loanConditions)
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
      FieldAccessRights[][] fieldAccessRightsArray = new FieldAccessRights[10][]
      {
        this.getFieldAccessRights(BizRule.Condition.Null, channelValue, -1, (string) null, (string) null),
        this.getFieldAccessRights(BizRule.Condition.LoanPurpose, channelValue, (int) loanPurposeValue, (string) null, (string) null),
        this.getFieldAccessRights(BizRule.Condition.LoanType, channelValue, (int) loanTypeValue, (string) null, (string) null),
        this.getFieldAccessRights(BizRule.Condition.LoanStatus, channelValue, (int) loanStatusValue, (string) null, (string) null),
        (FieldAccessRights[]) null,
        null,
        null,
        null,
        null,
        null
      };
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
      fieldAccessRightsArray[9] = !(loanConditions.LoanProgramName != string.Empty) ? new FieldAccessRights[0] : this.getFieldAccessRights(BizRule.Condition.LoanProgram, channelValue, -1, loanConditions.LoanProgramName, (string) null);
      Hashtable conditions = new Hashtable();
      for (int index = 0; index < fieldAccessRightsArray.Length; ++index)
      {
        foreach (FieldAccessRights fieldAccessRights in fieldAccessRightsArray[index])
        {
          object obj = conditions[(object) fieldAccessRights.FieldID];
          if (obj == null)
            conditions[(object) fieldAccessRights.FieldID] = (object) fieldAccessRights.Clone();
          else
            ((FieldAccessRights) obj).Combine(fieldAccessRights.AccessRights);
        }
      }
      string[] strArray = new string[conditions.Keys.Count];
      conditions.Keys.CopyTo((Array) strArray, 0);
      foreach (string key in strArray)
      {
        FieldAccessRights fieldAccessRights = (FieldAccessRights) conditions[(object) key];
        BizRule.FieldAccessRight fieldAccessRight1 = BizRule.FieldAccessRight.DoesNotApply;
        for (int index = 0; index < personas.Length; ++index)
        {
          object accessRight = fieldAccessRights.AccessRights[(object) personas[index].ID];
          if (accessRight != null)
          {
            BizRule.FieldAccessRight fieldAccessRight2 = (BizRule.FieldAccessRight) accessRight;
            if (fieldAccessRight2 != BizRule.FieldAccessRight.DoesNotApply)
            {
              if (fieldAccessRight1 == BizRule.FieldAccessRight.DoesNotApply)
                fieldAccessRight1 = fieldAccessRight2;
              if (fieldAccessRight2 > fieldAccessRight1)
                fieldAccessRight1 = fieldAccessRight2;
            }
          }
          else if (personas[index].ID == 1)
            fieldAccessRight1 = BizRule.FieldAccessRight.Edit;
        }
        conditions[(object) key] = (object) fieldAccessRight1;
      }
      return (object) conditions;
    }

    private FieldAccessRights[] getFieldAccessRights(
      BizRule.Condition condition,
      string condition2,
      int conditionState,
      string milestoneID,
      string conditionState2)
    {
      BizRuleInfo[] rule = this.GetRule(condition, condition2, conditionState, milestoneID, conditionState2);
      if (rule == null)
        return new FieldAccessRights[0];
      FieldAccessRuleInfo[] fieldAccessRuleInfoArray = new FieldAccessRuleInfo[rule.Length];
      for (int index = 0; index < rule.Length; ++index)
        fieldAccessRuleInfoArray[index] = (FieldAccessRuleInfo) rule[index];
      List<FieldAccessRights> fieldAccessRightsList = new List<FieldAccessRights>();
      for (int index = 0; index < fieldAccessRuleInfoArray.Length; ++index)
        fieldAccessRightsList.AddRange((IEnumerable<FieldAccessRights>) fieldAccessRuleInfoArray[index].FieldAccessRights);
      return fieldAccessRightsList.ToArray();
    }

    protected override object EvaluateNullConditions(Persona[] personas)
    {
      FieldAccessRights[] fieldAccessRights1 = this.getFieldAccessRights(BizRule.Condition.Null, "0", -1, (string) null, (string) null);
      Hashtable nullConditions = new Hashtable();
      foreach (FieldAccessRights fieldAccessRights2 in fieldAccessRights1)
      {
        object obj = nullConditions[(object) fieldAccessRights2.FieldID];
        if (obj == null)
          nullConditions[(object) fieldAccessRights2.FieldID] = (object) fieldAccessRights2.Clone();
        else
          ((FieldAccessRights) obj).Combine(fieldAccessRights2.AccessRights);
      }
      string[] strArray = new string[nullConditions.Keys.Count];
      nullConditions.Keys.CopyTo((Array) strArray, 0);
      foreach (string key in strArray)
      {
        FieldAccessRights fieldAccessRights3 = (FieldAccessRights) nullConditions[(object) key];
        BizRule.FieldAccessRight fieldAccessRight1 = BizRule.FieldAccessRight.DoesNotApply;
        for (int index = 0; index < personas.Length; ++index)
        {
          object accessRight = fieldAccessRights3.AccessRights[(object) personas[index].ID];
          if (accessRight != null)
          {
            BizRule.FieldAccessRight fieldAccessRight2 = (BizRule.FieldAccessRight) accessRight;
            if (fieldAccessRight2 != BizRule.FieldAccessRight.DoesNotApply)
            {
              if (fieldAccessRight1 == BizRule.FieldAccessRight.DoesNotApply)
                fieldAccessRight1 = fieldAccessRight2;
              else if (fieldAccessRight2 > fieldAccessRight1)
                fieldAccessRight1 = fieldAccessRight2;
            }
          }
          else if (personas[index].ID == 1)
            fieldAccessRight1 = BizRule.FieldAccessRight.Edit;
        }
        nullConditions[(object) key] = (object) fieldAccessRight1;
      }
      return (object) nullConditions;
    }
  }
}
