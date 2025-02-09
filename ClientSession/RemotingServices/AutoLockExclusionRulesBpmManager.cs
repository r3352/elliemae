// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.AutoLockExclusionRulesBpmManager
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using EllieMae.EMLite.BusinessRules;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Customization;
using EllieMae.EMLite.DataEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  public class AutoLockExclusionRulesBpmManager : BpmManager
  {
    internal static AutoLockExclusionRulesBpmManager Instance
    {
      get => Session.DefaultInstance.AutoLockExclusionRulesBpmManager;
    }

    internal static void RefreshInstance()
    {
      if (Session.DefaultInstance == null)
        return;
      Session.DefaultInstance.ResetFieldRulesBpmManager();
    }

    internal AutoLockExclusionRulesBpmManager(Sessions.Session session)
      : base(session, BizRuleType.AutoLockExclusionRules, ClientSessionCacheID.BpmFieldRules)
    {
    }

    public FieldRuleFieldIDAndType[] GetInconsistentFields(int ruleID)
    {
      return this.BPMManager.GetInconsistentRuleFields(ruleID);
    }

    public Hashtable GetAllRequiredFields(LoanConditions loanConditions, LoanData loanData)
    {
      BizRule.LoanPurpose loanPurposeValue = loanConditions.LoanPurposeValue;
      BizRule.LoanType loanTypeValue = loanConditions.LoanTypeValue;
      BizRule.LoanStatus loanStatusValue = loanConditions.LoanStatusValue;
      string currentMilestoneId = loanConditions.CurrentMilestoneID;
      BizRule.RateLock rateLockValue = loanConditions.RateLockValue;
      string currentRoleId = loanConditions.CurrentRoleID;
      string channelValue = loanConditions.ChannelValue;
      string milestoneID = ((IEnumerable<string>) loanConditions.FinishedMilestones).Last<string>();
      string conditionState2 = ((IEnumerable<string>) loanConditions.FinishedRoleIDs).Last<string>();
      List<Hashtable> hashtableList = new List<Hashtable>();
      hashtableList.Add(this.getRequiredFields(BizRule.Condition.Null, channelValue, -1, (string) null, (string) null));
      hashtableList.Add(this.getRequiredFields(BizRule.Condition.LoanPurpose, channelValue, (int) loanPurposeValue, (string) null, (string) null));
      hashtableList.Add(this.getRequiredFields(BizRule.Condition.LoanType, channelValue, (int) loanTypeValue, (string) null, (string) null));
      hashtableList.Add(this.getRequiredFields(BizRule.Condition.LoanStatus, channelValue, (int) loanStatusValue, (string) null, (string) null));
      hashtableList.Add(this.getRequiredFields(BizRule.Condition.CurrentLoanAssocMS, channelValue, -1, milestoneID, conditionState2));
      hashtableList.Add(this.getRequiredFields(BizRule.Condition.RateLock, channelValue, (int) rateLockValue, (string) null, (string) null));
      hashtableList.Add(this.getRequiredFields(BizRule.Condition.PropertyState, channelValue, (int) loanConditions.StateCodeValue, (string) null, (string) null));
      hashtableList.Add(this.getRequiredFields(BizRule.Condition.LoanDocType, channelValue, (int) loanConditions.DocTypeCodeValue, (string) null, (string) null));
      if (loanConditions.LoanProgramName != string.Empty)
        hashtableList.Add(this.getRequiredFields(BizRule.Condition.LoanProgram, channelValue, -1, loanConditions.LoanProgramName, (string) null));
      hashtableList.AddRange((IEnumerable<Hashtable>) this.getAdvancedConditionRequiredFields(channelValue, loanData));
      Hashtable allRequiredFields = new Hashtable();
      for (int index1 = 0; index1 < hashtableList.Count; ++index1)
      {
        if (hashtableList[index1] != null)
        {
          foreach (string key in (IEnumerable) hashtableList[index1].Keys)
          {
            if (!allRequiredFields.ContainsKey((object) key))
              allRequiredFields[(object) key] = (object) new ArrayList();
            ArrayList arrayList = (ArrayList) allRequiredFields[(object) key];
            string[] strArray = (string[]) hashtableList[index1][(object) key];
            for (int index2 = 0; index2 < strArray.Length; ++index2)
            {
              if (!arrayList.Contains((object) strArray[index2]))
                arrayList.Add((object) strArray[index2]);
            }
          }
        }
      }
      string[] strArray1 = new string[allRequiredFields.Keys.Count];
      int num = 0;
      foreach (string key in (IEnumerable) allRequiredFields.Keys)
        strArray1[num++] = key;
      foreach (string key in strArray1)
      {
        ArrayList arrayList = (ArrayList) allRequiredFields[(object) key];
        allRequiredFields[(object) key] = (object) (string[]) arrayList.ToArray(typeof (string));
      }
      return allRequiredFields;
    }

    public Hashtable GetAllFieldRules(LoanConditions loanConditions, LoanData loanData)
    {
      BizRule.LoanPurpose loanPurposeValue = loanConditions.LoanPurposeValue;
      BizRule.LoanType loanTypeValue = loanConditions.LoanTypeValue;
      BizRule.LoanStatus loanStatusValue = loanConditions.LoanStatusValue;
      string currentMilestoneId = loanConditions.CurrentMilestoneID;
      BizRule.RateLock rateLockValue = loanConditions.RateLockValue;
      string currentRoleId = loanConditions.CurrentRoleID;
      string channelValue = loanConditions.ChannelValue;
      string milestoneID = ((IEnumerable<string>) loanConditions.FinishedMilestones).Last<string>();
      string conditionState2 = ((IEnumerable<string>) loanConditions.FinishedRoleIDs).Last<string>();
      List<Hashtable> hashtableList = new List<Hashtable>();
      hashtableList.Add(this.getFieldRules(BizRule.Condition.Null, channelValue, -1, (string) null, (string) null));
      hashtableList.Add(this.getFieldRules(BizRule.Condition.LoanPurpose, channelValue, (int) loanPurposeValue, (string) null, (string) null));
      hashtableList.Add(this.getFieldRules(BizRule.Condition.LoanType, channelValue, (int) loanTypeValue, (string) null, (string) null));
      hashtableList.Add(this.getFieldRules(BizRule.Condition.LoanStatus, channelValue, (int) loanStatusValue, (string) null, (string) null));
      hashtableList.Add(this.getFieldRules(BizRule.Condition.CurrentLoanAssocMS, channelValue, -1, milestoneID, conditionState2));
      hashtableList.Add(this.getFieldRules(BizRule.Condition.RateLock, channelValue, (int) rateLockValue, (string) null, (string) null));
      hashtableList.Add(this.getFieldRules(BizRule.Condition.PropertyState, channelValue, (int) loanConditions.StateCodeValue, (string) null, (string) null));
      hashtableList.Add(this.getFieldRules(BizRule.Condition.LoanDocType, channelValue, (int) loanConditions.DocTypeCodeValue, (string) null, (string) null));
      hashtableList.Add(this.getFieldRules(BizRule.Condition.LoanProgram, channelValue, -1, loanConditions.LoanProgramName, (string) null));
      hashtableList.AddRange((IEnumerable<Hashtable>) this.getAdvancedConditionFieldRules(channelValue, loanData));
      Hashtable insensitiveHashtable = CollectionsUtil.CreateCaseInsensitiveHashtable();
      for (int index1 = 0; index1 < hashtableList.Count; ++index1)
      {
        if (hashtableList[index1] != null)
        {
          foreach (string key in (IEnumerable) hashtableList[index1].Keys)
          {
            if (!insensitiveHashtable.ContainsKey((object) key))
            {
              insensitiveHashtable[(object) key] = hashtableList[index1][(object) key];
            }
            else
            {
              object range = hashtableList[index1][(object) key];
              if (range is FRRange)
              {
                FRRange frRange = (FRRange) range;
                if (frRange.LowerBound == "" && frRange.UpperBound == "")
                  continue;
              }
              if (insensitiveHashtable[(object) key].GetType() != range.GetType())
                throw new Exception("Field Rule: mismatched rule types");
              switch (range)
              {
                case FRRange _:
                  ((FRRange) insensitiveHashtable[(object) key]).Combine((FRRange) range);
                  continue;
                case FRList _:
                  FRList frList1 = (FRList) insensitiveHashtable[(object) key];
                  FRList frList2 = (FRList) range;
                  if (frList1.IsLock != frList2.IsLock)
                    throw new Exception("Inconsistent field rule list type");
                  ArrayList arrayList1 = new ArrayList();
                  ArrayList arrayList2 = new ArrayList();
                  arrayList1.AddRange((ICollection) frList1.List);
                  arrayList2.AddRange((ICollection) frList2.List);
                  for (int index2 = 0; index2 < arrayList1.Count; ++index2)
                  {
                    if (!arrayList2.Contains(arrayList1[index2]))
                      arrayList1.RemoveAt(index2);
                  }
                  insensitiveHashtable[(object) key] = (object) new FRList((string[]) arrayList1.ToArray(typeof (string)), frList1.IsLock);
                  continue;
                case string _:
                  throw new Exception("Field Rule: multiple rules with code logic");
                default:
                  throw new Exception("Invalid Field Rule type");
              }
            }
          }
        }
      }
      return insensitiveHashtable;
    }

    private Hashtable getRequiredFields(
      BizRule.Condition condition,
      string condition2,
      int conditionState,
      string milestoneID,
      string conditionState2)
    {
      BizRuleInfo[] activeRule = this.GetActiveRule(condition, condition2, conditionState, milestoneID, conditionState2);
      if (activeRule == null)
        return new Hashtable();
      FieldRuleInfo[] fieldRuleInfoArray = new FieldRuleInfo[activeRule.Length];
      for (int index = 0; index < activeRule.Length; ++index)
        fieldRuleInfoArray[index] = (FieldRuleInfo) activeRule[index];
      Hashtable requiredFields = new Hashtable();
      for (int index = 0; index < fieldRuleInfoArray.Length; ++index)
      {
        foreach (DictionaryEntry requiredField in fieldRuleInfoArray[index].RequiredFields)
        {
          if (!requiredFields.ContainsKey(requiredField.Key))
            requiredFields.Add(requiredField.Key, requiredField.Value);
        }
      }
      return requiredFields;
    }

    private Hashtable getFieldRules(
      BizRule.Condition condition,
      string condition2,
      int conditionState,
      string milestoneID,
      string conditionState2)
    {
      BizRuleInfo[] activeRule = this.GetActiveRule(condition, condition2, conditionState, milestoneID, conditionState2);
      if (activeRule == null)
        return new Hashtable();
      FieldRuleInfo[] fieldRuleInfoArray = new FieldRuleInfo[activeRule.Length];
      for (int index = 0; index < activeRule.Length; ++index)
        fieldRuleInfoArray[index] = (FieldRuleInfo) activeRule[index];
      Hashtable fieldRules = new Hashtable();
      for (int index = 0; index < fieldRuleInfoArray.Length; ++index)
      {
        foreach (DictionaryEntry fieldRule in fieldRuleInfoArray[index].FieldRules)
        {
          if (!fieldRules.ContainsKey(fieldRule.Key))
            fieldRules.Add(fieldRule.Key, fieldRule.Value);
        }
      }
      return fieldRules;
    }

    private List<Hashtable> getAdvancedConditionFieldRules(string channelValue, LoanData loan)
    {
      List<Hashtable> conditionFieldRules = new List<Hashtable>();
      ExecutionContext context = new ExecutionContext(this.session.UserInfo, loan, (IServerDataProvider) new CustomCodeSessionDataProvider(this.session.SessionObjects));
      foreach (ConditionEvaluator conditionEvaluator in this.GetConditionEvaluators())
      {
        if (conditionEvaluator.Rule.Condition == BizRule.Condition.AdvancedCoding && conditionEvaluator.AppliesTo(context) && conditionEvaluator.Rule.Condition2.IndexOf(channelValue) > -1)
          conditionFieldRules.Add(((FieldRuleInfo) conditionEvaluator.Rule).FieldRules);
      }
      return conditionFieldRules;
    }

    private List<Hashtable> getAdvancedConditionRequiredFields(string channelValue, LoanData loan)
    {
      List<Hashtable> conditionRequiredFields = new List<Hashtable>();
      ExecutionContext context = new ExecutionContext(this.session.UserInfo, loan, (IServerDataProvider) new CustomCodeSessionDataProvider(this.session.SessionObjects));
      foreach (ConditionEvaluator conditionEvaluator in this.GetConditionEvaluators())
      {
        if (conditionEvaluator.Rule.Condition == BizRule.Condition.AdvancedCoding && conditionEvaluator.AppliesTo(context) && conditionEvaluator.Rule.Condition2.IndexOf(channelValue) > -1)
          conditionRequiredFields.Add(((FieldRuleInfo) conditionEvaluator.Rule).RequiredFields);
      }
      return conditionRequiredFields;
    }
  }
}
