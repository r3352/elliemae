// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.AutomatedConditionBpmManager
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using EllieMae.EMLite.BusinessRules;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Customization;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  public class AutomatedConditionBpmManager : BpmManager
  {
    internal static AutomatedConditionBpmManager Instance
    {
      get => Session.DefaultInstance.AutomatedConditionsBpmManager;
    }

    internal static void RefreshInstance()
    {
      if (Session.DefaultInstance == null)
        return;
      Session.DefaultInstance.ResetTriggersBpmManager();
    }

    internal AutomatedConditionBpmManager(Sessions.Session session)
      : base(session, BizRuleType.AutomatedConditions, ClientSessionCacheID.BpmAutomatedConditions)
    {
    }

    public string[] GetConditions(
      LoanConditions loanConditions,
      LoanData loanData,
      ConditionType conditionType)
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
      string str = loanData.GetField("1553").Replace(" ", "");
      BizRule.PropertyType conditionState1 = BizRule.PropertyType.Unknow;
      switch (str)
      {
        case "":
          AutomatedCondition[][] automatedConditionArray = new AutomatedCondition[12][]
          {
            this.getConditions(BizRule.Condition.Null, channelValue, -1, (string) null, (string) null),
            this.getConditions(BizRule.Condition.LoanPurpose, channelValue, (int) loanPurposeValue, (string) null, (string) null),
            this.getConditions(BizRule.Condition.LoanType, channelValue, (int) loanTypeValue, (string) null, (string) null),
            this.getConditions(BizRule.Condition.LoanStatus, channelValue, (int) loanStatusValue, (string) null, (string) null),
            this.getConditions(BizRule.Condition.CurrentLoanAssocMS, channelValue, -1, milestoneID, conditionState2),
            this.getConditions(BizRule.Condition.RateLock, channelValue, (int) rateLockValue, (string) null, (string) null),
            this.getConditions(BizRule.Condition.PropertyState, channelValue, (int) loanConditions.StateCodeValue, (string) null, (string) null),
            this.getConditions(BizRule.Condition.LoanDocType, channelValue, (int) loanConditions.DocTypeCodeValue, (string) null, (string) null),
            this.getConditions(BizRule.Condition.LoanProgram, channelValue, -1, loanConditions.LoanProgramName, (string) null),
            null,
            null,
            null
          };
          if (loanData.GetField("1811") != string.Empty)
          {
            BizRule.PropertyOccupancy conditionState3 = (BizRule.PropertyOccupancy) Enum.Parse(typeof (BizRule.PropertyOccupancy), loanData.GetField("1811"), true);
            automatedConditionArray[9] = this.getConditions(BizRule.Condition.Occupancy, channelValue, (int) conditionState3, (string) null, (string) null);
          }
          else
            automatedConditionArray[9] = (AutomatedCondition[]) null;
          automatedConditionArray[10] = this.getConditions(BizRule.Condition.Occupancy, channelValue, (int) conditionState1, (string) null, (string) null);
          automatedConditionArray[11] = this.getAdvancedAutomatedConditions(channelValue, loanData);
          List<string> stringList = new List<string>();
          for (int index = 0; index < automatedConditionArray.Length; ++index)
          {
            if (automatedConditionArray[index] != null && automatedConditionArray[index].Length != 0)
            {
              foreach (AutomatedCondition automatedCondition in automatedConditionArray[index])
              {
                if (automatedCondition.ConditionType == conditionType && !stringList.Contains(automatedCondition.ConditionName))
                  stringList.Add(automatedCondition.ConditionName);
              }
            }
          }
          return stringList.ToArray();
        case "1Unit":
          conditionState1 = BizRule.PropertyType.Unit_1;
          goto case "";
        case "2-4Units":
          conditionState1 = BizRule.PropertyType.Units_24;
          goto case "";
        default:
          conditionState1 = (BizRule.PropertyType) Enum.Parse(typeof (BizRule.PropertyType), str, true);
          goto case "";
      }
    }

    private AutomatedCondition[] getConditions(
      BizRule.Condition condition,
      string condition2,
      int conditionState,
      string milestoneID,
      string conditionState2)
    {
      BizRuleInfo[] activeRule = this.GetActiveRule(condition, condition2, conditionState, milestoneID, conditionState2);
      if (activeRule == null)
        return new AutomatedCondition[0];
      AutomatedConditionRuleInfo[] conditionRuleInfoArray = new AutomatedConditionRuleInfo[activeRule.Length];
      for (int index = 0; index < activeRule.Length; ++index)
        conditionRuleInfoArray[index] = (AutomatedConditionRuleInfo) activeRule[index];
      List<AutomatedCondition> automatedConditionList = new List<AutomatedCondition>();
      for (int index = 0; index < conditionRuleInfoArray.Length; ++index)
        automatedConditionList.AddRange((IEnumerable<AutomatedCondition>) conditionRuleInfoArray[index].Conditions);
      return automatedConditionList.ToArray();
    }

    private AutomatedCondition[] getAdvancedAutomatedConditions(string channelValue, LoanData loan)
    {
      List<AutomatedCondition> automatedConditionList = new List<AutomatedCondition>();
      ExecutionContext context = new ExecutionContext(this.session.UserInfo, loan, (IServerDataProvider) new CustomCodeSessionDataProvider(this.session.SessionObjects));
      foreach (ConditionEvaluator conditionEvaluator in this.GetConditionEvaluators())
      {
        if (conditionEvaluator.Rule.Condition == BizRule.Condition.AdvancedCoding && conditionEvaluator.AppliesTo(context) && conditionEvaluator.Rule.Condition2.IndexOf(channelValue) > -1)
          automatedConditionList.AddRange((IEnumerable<AutomatedCondition>) ((AutomatedConditionRuleInfo) conditionEvaluator.Rule).Conditions);
      }
      return automatedConditionList.ToArray();
    }
  }
}
