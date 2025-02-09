// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.PrintFormsBpmManager
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using EllieMae.EMLite.BusinessRules;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Customization;
using EllieMae.EMLite.DataEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  public class PrintFormsBpmManager : BpmManager
  {
    internal static PrintFormsBpmManager Instance => Session.DefaultInstance.PrintFormsBpmManager;

    internal static void RefreshInstance()
    {
      if (Session.DefaultInstance == null)
        return;
      Session.DefaultInstance.ResetPrintFormsBpmManager();
    }

    internal PrintFormsBpmManager(Sessions.Session session)
      : base(session, BizRuleType.PrintForms, ClientSessionCacheID.BpmPrintForms)
    {
    }

    public ActivatedPrintFormRule GetAllRequiredFields(
      LoanConditions loanConditions,
      LoanData loanData)
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
      hashtableList.Add(this.getAdvancedConditionRequiredFields(channelValue, loanData));
      ActivatedPrintFormRule allRequiredFields = new ActivatedPrintFormRule();
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      for (int index = 0; index < hashtableList.Count; ++index)
      {
        if (hashtableList[index] != null)
        {
          foreach (DictionaryEntry dictionaryEntry in hashtableList[index])
          {
            string formID = dictionaryEntry.Key.ToString();
            allRequiredFields.AddFormRequiredFields(formID, (Hashtable) dictionaryEntry.Value);
          }
        }
      }
      return allRequiredFields;
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
      Hashtable insensitiveHashtable = CollectionsUtil.CreateCaseInsensitiveHashtable();
      for (int index = 0; index < activeRule.Length; ++index)
      {
        foreach (PrintRequiredFieldsInfo formRule in ((PrintFormRuleInfo) activeRule[index]).FormRules)
          this.collectRequiredFields(formRule, insensitiveHashtable);
      }
      return insensitiveHashtable;
    }

    private void collectRequiredFields(PrintRequiredFieldsInfo r, Hashtable allRules)
    {
      Hashtable hashtable;
      if (!allRules.ContainsKey((object) r.FormID))
      {
        allRules.Add((object) r.FormID, (object) null);
        hashtable = (Hashtable) null;
      }
      else
        hashtable = (Hashtable) allRules[(object) r.FormID];
      if (hashtable == null)
        hashtable = CollectionsUtil.CreateCaseInsensitiveHashtable();
      if (r.FieldIDs != null)
      {
        for (int index = 0; index < r.FieldIDs.Length; ++index)
        {
          if (!hashtable.ContainsKey((object) r.FieldIDs[index]))
            hashtable.Add((object) r.FieldIDs[index], (object) "");
        }
      }
      if (r.AdvancedCoding != string.Empty)
      {
        if (!hashtable.ContainsKey((object) PrintRequiredFieldsInfo.ADVANCEDCODINGID))
          hashtable.Add((object) PrintRequiredFieldsInfo.ADVANCEDCODINGID, (object) new ArrayList());
        ((ArrayList) hashtable[(object) PrintRequiredFieldsInfo.ADVANCEDCODINGID]).Add((object) r.AdvancedCoding);
      }
      allRules[(object) r.FormID] = (object) hashtable;
    }

    private Hashtable getAdvancedConditionRequiredFields(string channelValue, LoanData loan)
    {
      ExecutionContext context = new ExecutionContext(this.session.UserInfo, loan, (IServerDataProvider) new CustomCodeSessionDataProvider(this.session.SessionObjects));
      Hashtable insensitiveHashtable = CollectionsUtil.CreateCaseInsensitiveHashtable();
      foreach (ConditionEvaluator conditionEvaluator in this.GetConditionEvaluators())
      {
        if (conditionEvaluator.Rule.Condition == BizRule.Condition.AdvancedCoding && conditionEvaluator.AppliesTo(context) && conditionEvaluator.Rule.Condition2.IndexOf(channelValue) > -1)
        {
          PrintFormRuleInfo rule = (PrintFormRuleInfo) conditionEvaluator.Rule;
          if (rule != null)
          {
            foreach (PrintRequiredFieldsInfo formRule in rule.FormRules)
              this.collectRequiredFields(formRule, insensitiveHashtable);
          }
        }
      }
      return insensitiveHashtable;
    }
  }
}
