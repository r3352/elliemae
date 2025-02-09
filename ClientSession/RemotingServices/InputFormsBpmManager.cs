// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.InputFormsBpmManager
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using EllieMae.EMLite.BusinessRules;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Customization;
using EllieMae.EMLite.DataEngine;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  public class InputFormsBpmManager : BpmManager
  {
    internal static InputFormsBpmManager Instance => Session.DefaultInstance.InputFormsBpmManager;

    internal static void RefreshInstance()
    {
      if (Session.DefaultInstance == null)
        return;
      Session.DefaultInstance.ResetInputFormsBpmManager();
    }

    internal InputFormsBpmManager(Sessions.Session session)
      : base(session, BizRuleType.InputForms, ClientSessionCacheID.BpmInputForms)
    {
    }

    public InputFormInfo[] GetForms(InputFormRuleInfo rule)
    {
      return this.session.SessionObjects.FormManager.GetFormInfos(rule.Forms);
    }

    public string[] GetForms(LoanConditions loanConditions, LoanData loanData)
    {
      BizRule.LoanPurpose loanPurposeValue = loanConditions.LoanPurposeValue;
      BizRule.LoanType loanTypeValue = loanConditions.LoanTypeValue;
      BizRule.LoanStatus loanStatusValue = loanConditions.LoanStatusValue;
      string currentMilestoneId = loanConditions.CurrentMilestoneID;
      string currentRoleId = loanConditions.CurrentRoleID;
      BizRule.RateLock rateLockValue = loanConditions.RateLockValue;
      string channelValue = loanConditions.ChannelValue;
      string milestoneID = ((IEnumerable<string>) loanConditions.FinishedMilestones).Last<string>();
      string conditionState2 = ((IEnumerable<string>) loanConditions.FinishedRoleIDs).Last<string>();
      string[][] strArray = new string[9][]
      {
        this.getForms(BizRule.Condition.LoanPurpose, channelValue, (int) loanPurposeValue, (string) null, (string) null),
        this.getForms(BizRule.Condition.LoanType, channelValue, (int) loanTypeValue, (string) null, (string) null),
        this.getForms(BizRule.Condition.LoanStatus, channelValue, (int) loanStatusValue, (string) null, (string) null),
        this.getForms(BizRule.Condition.CurrentLoanAssocMS, channelValue, -1, milestoneID, conditionState2),
        this.getForms(BizRule.Condition.RateLock, channelValue, (int) rateLockValue, (string) null, (string) null),
        this.getForms(BizRule.Condition.PropertyState, channelValue, (int) loanConditions.StateCodeValue, (string) null, (string) null),
        this.getForms(BizRule.Condition.LoanDocType, channelValue, (int) loanConditions.DocTypeCodeValue, (string) null, (string) null),
        this.getForms(BizRule.Condition.LoanProgram, channelValue, -1, loanConditions.LoanProgramName, (string) null),
        this.getAdvancedConditionInputForms(channelValue, loanData)
      };
      List<string> stringList = new List<string>();
      for (int index = 0; index < strArray.Length; ++index)
      {
        if (strArray[index] != null)
        {
          foreach (string str in strArray[index])
          {
            if (!stringList.Contains(str))
              stringList.Add(str);
          }
        }
      }
      return stringList.ToArray();
    }

    private string[] getForms(
      BizRule.Condition condition,
      string condition2,
      int conditionState,
      string milestoneID,
      string conditionState2)
    {
      BizRuleInfo[] activeRule = this.GetActiveRule(condition, condition2, conditionState, milestoneID, conditionState2);
      if (activeRule == null)
        return new string[0];
      InputFormRuleInfo[] inputFormRuleInfoArray = new InputFormRuleInfo[activeRule.Length];
      for (int index = 0; index < activeRule.Length; ++index)
        inputFormRuleInfoArray[index] = (InputFormRuleInfo) activeRule[index];
      List<string> stringList = new List<string>();
      for (int index = 0; index < inputFormRuleInfoArray.Length; ++index)
        stringList.AddRange((IEnumerable<string>) inputFormRuleInfoArray[index].Forms);
      return stringList.ToArray();
    }

    private string[] getAdvancedConditionInputForms(string channelValue, LoanData loan)
    {
      List<string> stringList = new List<string>();
      ExecutionContext context = new ExecutionContext(this.session.UserInfo, loan, (IServerDataProvider) new CustomCodeSessionDataProvider(this.session.SessionObjects));
      foreach (ConditionEvaluator conditionEvaluator in this.GetConditionEvaluators())
      {
        if (conditionEvaluator.Rule.Condition == BizRule.Condition.AdvancedCoding && conditionEvaluator.AppliesTo(context) && conditionEvaluator.Rule.Condition2.IndexOf(channelValue) > -1)
          stringList.AddRange((IEnumerable<string>) ((InputFormRuleInfo) conditionEvaluator.Rule).Forms);
      }
      return stringList.ToArray();
    }
  }
}
