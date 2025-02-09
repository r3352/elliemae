// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.AutomatedEnhancedConditionBpmManager
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using EllieMae.EMLite.BusinessRules;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Customization;
using EllieMae.EMLite.DataEngine;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  public class AutomatedEnhancedConditionBpmManager : BpmManager
  {
    internal static AutomatedEnhancedConditionBpmManager Instance
    {
      get => Session.DefaultInstance.AutomatedEnhancedConditionsBpmManager;
    }

    internal static void RefreshInstance()
    {
      if (Session.DefaultInstance == null)
        return;
      Session.DefaultInstance.ResetAutomatedEnhancedConditionsBpmManager();
    }

    internal AutomatedEnhancedConditionBpmManager(Sessions.Session session)
      : base(session, BizRuleType.AutomatedEnhancedConditions, ClientSessionCacheID.BpmAutomatedEnhancedConditions)
    {
    }

    public string[] GetConditions(LoanConditions loanConditions, LoanData loanData)
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
          AutomatedEnhancedCondition[][] enhancedConditionArray = new AutomatedEnhancedCondition[12][]
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
            enhancedConditionArray[9] = this.getConditions(BizRule.Condition.Occupancy, channelValue, (int) conditionState3, (string) null, (string) null);
          }
          else
            enhancedConditionArray[9] = (AutomatedEnhancedCondition[]) null;
          enhancedConditionArray[10] = this.getConditions(BizRule.Condition.Occupancy, channelValue, (int) conditionState1, (string) null, (string) null);
          enhancedConditionArray[11] = this.getAdvancedAutomatedConditions(channelValue, loanData);
          List<string> stringList = new List<string>();
          for (int index = 0; index < enhancedConditionArray.Length; ++index)
          {
            if (enhancedConditionArray[index] != null && enhancedConditionArray[index].Length != 0)
            {
              foreach (AutomatedEnhancedCondition enhancedCondition in enhancedConditionArray[index])
              {
                if (!stringList.Contains(enhancedCondition.UniqueKey))
                  stringList.Add(enhancedCondition.UniqueKey);
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

    private AutomatedEnhancedCondition[] getConditions(
      BizRule.Condition condition,
      string condition2,
      int conditionState,
      string milestoneID,
      string conditionState2)
    {
      BizRuleInfo[] activeRule = this.GetActiveRule(condition, condition2, conditionState, milestoneID, conditionState2);
      if (activeRule == null)
        return new AutomatedEnhancedCondition[0];
      AutomatedEnhancedConditionRuleInfo[] conditionRuleInfoArray = new AutomatedEnhancedConditionRuleInfo[activeRule.Length];
      for (int index = 0; index < activeRule.Length; ++index)
        conditionRuleInfoArray[index] = (AutomatedEnhancedConditionRuleInfo) activeRule[index];
      List<AutomatedEnhancedCondition> enhancedConditionList = new List<AutomatedEnhancedCondition>();
      for (int index = 0; index < conditionRuleInfoArray.Length; ++index)
        enhancedConditionList.AddRange((IEnumerable<AutomatedEnhancedCondition>) conditionRuleInfoArray[index].Conditions);
      return enhancedConditionList.ToArray();
    }

    private AutomatedEnhancedCondition[] getAdvancedAutomatedConditions(
      string channelValue,
      LoanData loan)
    {
      List<AutomatedEnhancedCondition> enhancedConditionList = new List<AutomatedEnhancedCondition>();
      ExecutionContext context = new ExecutionContext(this.session.UserInfo, loan, (IServerDataProvider) new CustomCodeSessionDataProvider(this.session.SessionObjects));
      foreach (ConditionEvaluator conditionEvaluator in this.GetConditionEvaluators())
      {
        if (conditionEvaluator.Rule.Condition == BizRule.Condition.AdvancedCoding && conditionEvaluator.AppliesTo(context) && conditionEvaluator.Rule.Condition2.IndexOf(channelValue) > -1)
          enhancedConditionList.AddRange((IEnumerable<AutomatedEnhancedCondition>) ((AutomatedEnhancedConditionRuleInfo) conditionEvaluator.Rule).Conditions);
      }
      return enhancedConditionList.ToArray();
    }
  }
}
