// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.MilestoneRulesBpmManager
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using EllieMae.EMLite.BusinessRules;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Customization;
using EllieMae.EMLite.DataEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  public class MilestoneRulesBpmManager : BpmManager
  {
    internal static MilestoneRulesBpmManager Instance
    {
      get => Session.DefaultInstance.MilestoneRulesBpmManager;
    }

    internal static void RefreshInstance()
    {
      if (Session.DefaultInstance == null)
        return;
      Session.DefaultInstance.ResetMilestoneRulesBpmManager();
    }

    internal MilestoneRulesBpmManager(Sessions.Session session)
      : base(session, BizRuleType.MilestoneRules, ClientSessionCacheID.BpmMilestoneRules)
    {
    }

    public DocMilestonePair[] GetAllRequiredDocs(LoanConditions loanConditions, LoanData loanData)
    {
      ArrayList arrayList = new ArrayList();
      DocMilestonePair[] requiredDocs1 = this.getRequiredDocs(loanConditions, loanData);
      for (int index = 0; index < requiredDocs1.Length; ++index)
      {
        if (loanConditions.ContainMilestone(requiredDocs1[index].MilestoneID))
          arrayList.Add((object) requiredDocs1[index]);
      }
      if (!loanConditions.CheckRoleOnly)
      {
        DocMilestonePair[] requiredDocs2 = this.getRequiredDocs(BizRule.Condition.Null, loanConditions.ChannelValue, -1, (string) null, (string) null);
        for (int index = 0; index < requiredDocs2.Length; ++index)
        {
          if (loanConditions.ContainMilestone(requiredDocs2[index].MilestoneID))
          {
            bool flag = false;
            foreach (DocMilestonePair docMilestonePair in requiredDocs1)
            {
              if (requiredDocs2[index].DocGuid == docMilestonePair.DocGuid && docMilestonePair.MilestoneID == requiredDocs2[index].MilestoneID)
              {
                flag = true;
                break;
              }
            }
            if (!flag)
              arrayList.Add((object) requiredDocs2[index]);
          }
        }
      }
      return (DocMilestonePair[]) arrayList.ToArray(typeof (DocMilestonePair));
    }

    public TaskMilestonePair[] GetAllRequiredTasks(LoanConditions loanConditions, LoanData loanData)
    {
      ArrayList arrayList = new ArrayList();
      TaskMilestonePair[] requiredTasks1 = this.getRequiredTasks(loanConditions, loanData);
      for (int index = 0; index < requiredTasks1.Length; ++index)
      {
        if (loanConditions.ContainMilestone(requiredTasks1[index].MilestoneID))
          arrayList.Add((object) requiredTasks1[index]);
      }
      if (!loanConditions.CheckRoleOnly)
      {
        TaskMilestonePair[] requiredTasks2 = this.getRequiredTasks(BizRule.Condition.Null, loanConditions.ChannelValue, -1, (string) null, (string) null);
        for (int index = 0; index < requiredTasks2.Length; ++index)
        {
          if (loanConditions.ContainMilestone(requiredTasks2[index].MilestoneID))
          {
            bool flag = false;
            foreach (TaskMilestonePair taskMilestonePair in requiredTasks1)
            {
              if (requiredTasks2[index].TaskGuid == taskMilestonePair.TaskGuid && taskMilestonePair.MilestoneID == requiredTasks2[index].MilestoneID)
              {
                flag = true;
                break;
              }
            }
            if (!flag)
              arrayList.Add((object) requiredTasks2[index]);
          }
        }
      }
      return (TaskMilestonePair[]) arrayList.ToArray(typeof (TaskMilestonePair));
    }

    public FieldMilestonePair[] GetAllRequiredFields(
      LoanConditions loanConditions,
      LoanData loanData)
    {
      ArrayList arrayList = new ArrayList();
      FieldMilestonePair[] requiredFields1 = this.getRequiredFields(loanConditions, loanData);
      for (int index = 0; index < requiredFields1.Length; ++index)
      {
        if (loanConditions.ContainMilestone(requiredFields1[index].MilestoneID) || string.Compare(loanConditions.CurrentMilestoneID, requiredFields1[index].MilestoneID, true) == 0)
          arrayList.Add((object) requiredFields1[index]);
      }
      if (!loanConditions.CheckRoleOnly)
      {
        bool flag1 = loanData.GetField("NEWHUD.X354") == "Y" || Utils.CheckIf2015RespaTila(loanData.GetField("3969"));
        FieldMilestonePair[] requiredFields2 = this.getRequiredFields(BizRule.Condition.Null, loanConditions.ChannelValue, -1, (string) null, (string) null);
        for (int index = 0; index < requiredFields2.Length; ++index)
        {
          if (loanConditions.ContainMilestone(requiredFields2[index].MilestoneID) || string.Compare(loanConditions.CurrentMilestoneID, requiredFields2[index].MilestoneID, true) == 0)
          {
            bool flag2 = false;
            foreach (FieldMilestonePair fieldMilestonePair in requiredFields1)
            {
              if (requiredFields2[index].FieldID == fieldMilestonePair.FieldID)
              {
                flag2 = true;
                break;
              }
            }
            if (!flag2 && (flag1 || !requiredFields2[index].FieldID.ToUpper().StartsWith("NEWHUD.X")) && (!flag1 || !BPM.DEAD_FIELDS_IN_NEW_HUD.Contains(requiredFields2[index].FieldID.ToUpper())))
            {
              if (requiredFields2[index].FieldID.ToUpper().StartsWith("FD") && requiredFields2[index].FieldID.Length == 6)
                requiredFields2[index] = new FieldMilestonePair(requiredFields2[index].FieldID.Replace("FD", "DD"), requiredFields2[index].MilestoneID);
              arrayList.Add((object) requiredFields2[index]);
            }
          }
        }
      }
      return (FieldMilestonePair[]) arrayList.ToArray(typeof (FieldMilestonePair));
    }

    private DocMilestonePair[] getRequiredDocs(LoanConditions loanConditions, LoanData loanData)
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
      DocMilestonePair[][] docMilestonePairArray = new DocMilestonePair[9][];
      if (loanConditions.CheckRoleOnly)
      {
        docMilestonePairArray[3] = this.getRequiredDocs(BizRule.Condition.CurrentLoanAssocMS, channelValue, -1, milestoneID, conditionState2);
      }
      else
      {
        docMilestonePairArray[0] = this.getRequiredDocs(BizRule.Condition.LoanPurpose, channelValue, (int) loanPurposeValue, (string) null, (string) null);
        docMilestonePairArray[1] = this.getRequiredDocs(BizRule.Condition.LoanType, channelValue, (int) loanTypeValue, (string) null, (string) null);
        docMilestonePairArray[2] = this.getRequiredDocs(BizRule.Condition.LoanStatus, channelValue, (int) loanStatusValue, (string) null, (string) null);
        docMilestonePairArray[3] = this.getRequiredDocs(BizRule.Condition.CurrentLoanAssocMS, channelValue, -1, milestoneID, conditionState2);
        docMilestonePairArray[4] = this.getRequiredDocs(BizRule.Condition.RateLock, channelValue, (int) rateLockValue, (string) null, (string) null);
        docMilestonePairArray[5] = this.getRequiredDocs(BizRule.Condition.PropertyState, channelValue, (int) loanConditions.StateCodeValue, (string) null, (string) null);
        docMilestonePairArray[6] = this.getRequiredDocs(BizRule.Condition.LoanDocType, channelValue, (int) loanConditions.DocTypeCodeValue, (string) null, (string) null);
        docMilestonePairArray[7] = this.getRequiredDocs(BizRule.Condition.LoanProgram, channelValue, -1, loanConditions.LoanProgramName, (string) null);
        docMilestonePairArray[8] = this.getAdvancedConditionRequiredDocs(channelValue, loanData);
      }
      ArrayList arrayList = new ArrayList();
      for (int index = 0; index < docMilestonePairArray.Length; ++index)
      {
        if (docMilestonePairArray[index] != null)
        {
          foreach (DocMilestonePair docMilestonePair1 in docMilestonePairArray[index])
          {
            bool flag = false;
            foreach (DocMilestonePair docMilestonePair2 in arrayList)
            {
              if (docMilestonePair1.DocGuid == docMilestonePair2.DocGuid && docMilestonePair1.MilestoneID == docMilestonePair2.MilestoneID)
              {
                flag = true;
                break;
              }
            }
            if (!flag)
              arrayList.Add((object) docMilestonePair1);
          }
        }
      }
      return (DocMilestonePair[]) arrayList.ToArray(typeof (DocMilestonePair));
    }

    private TaskMilestonePair[] getRequiredTasks(LoanConditions loanConditions, LoanData loanData)
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
      TaskMilestonePair[][] taskMilestonePairArray = new TaskMilestonePair[9][];
      if (loanConditions.CheckRoleOnly)
      {
        taskMilestonePairArray[3] = this.getRequiredTasks(BizRule.Condition.CurrentLoanAssocMS, channelValue, -1, milestoneID, conditionState2);
      }
      else
      {
        taskMilestonePairArray[0] = this.getRequiredTasks(BizRule.Condition.LoanPurpose, channelValue, (int) loanPurposeValue, (string) null, (string) null);
        taskMilestonePairArray[1] = this.getRequiredTasks(BizRule.Condition.LoanType, channelValue, (int) loanTypeValue, (string) null, (string) null);
        taskMilestonePairArray[2] = this.getRequiredTasks(BizRule.Condition.LoanStatus, channelValue, (int) loanStatusValue, (string) null, (string) null);
        taskMilestonePairArray[3] = this.getRequiredTasks(BizRule.Condition.CurrentLoanAssocMS, channelValue, -1, milestoneID, conditionState2);
        taskMilestonePairArray[4] = this.getRequiredTasks(BizRule.Condition.RateLock, channelValue, (int) rateLockValue, (string) null, (string) null);
        taskMilestonePairArray[5] = this.getRequiredTasks(BizRule.Condition.PropertyState, channelValue, (int) loanConditions.StateCodeValue, (string) null, (string) null);
        taskMilestonePairArray[6] = this.getRequiredTasks(BizRule.Condition.LoanDocType, channelValue, (int) loanConditions.DocTypeCodeValue, (string) null, (string) null);
        taskMilestonePairArray[7] = this.getRequiredTasks(BizRule.Condition.LoanProgram, channelValue, -1, loanConditions.LoanProgramName, (string) null);
        taskMilestonePairArray[8] = this.getAdvancedConditionRequiredTasks(channelValue, loanData);
      }
      ArrayList arrayList = new ArrayList();
      for (int index = 0; index < taskMilestonePairArray.Length; ++index)
      {
        if (taskMilestonePairArray[index] != null)
        {
          foreach (TaskMilestonePair taskMilestonePair1 in taskMilestonePairArray[index])
          {
            bool flag = false;
            foreach (TaskMilestonePair taskMilestonePair2 in arrayList)
            {
              if (taskMilestonePair1.TaskGuid == taskMilestonePair2.TaskGuid && taskMilestonePair1.MilestoneID == taskMilestonePair2.MilestoneID)
              {
                flag = true;
                break;
              }
            }
            if (!flag)
              arrayList.Add((object) taskMilestonePair1);
          }
        }
      }
      return (TaskMilestonePair[]) arrayList.ToArray(typeof (TaskMilestonePair));
    }

    private FieldMilestonePair[] getRequiredFields(LoanConditions loanConditions, LoanData loanData)
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
      FieldMilestonePair[][] fieldMilestonePairArray = new FieldMilestonePair[9][];
      if (loanConditions.CheckRoleOnly)
      {
        fieldMilestonePairArray[3] = this.getRequiredFields(BizRule.Condition.CurrentLoanAssocMS, channelValue, -1, milestoneID, conditionState2);
      }
      else
      {
        fieldMilestonePairArray[0] = this.getRequiredFields(BizRule.Condition.LoanPurpose, channelValue, (int) loanPurposeValue, (string) null, (string) null);
        fieldMilestonePairArray[1] = this.getRequiredFields(BizRule.Condition.LoanType, channelValue, (int) loanTypeValue, (string) null, (string) null);
        fieldMilestonePairArray[2] = this.getRequiredFields(BizRule.Condition.LoanStatus, channelValue, (int) loanStatusValue, (string) null, (string) null);
        fieldMilestonePairArray[3] = this.getRequiredFields(BizRule.Condition.CurrentLoanAssocMS, channelValue, -1, milestoneID, conditionState2);
        fieldMilestonePairArray[4] = this.getRequiredFields(BizRule.Condition.RateLock, channelValue, (int) rateLockValue, (string) null, (string) null);
        fieldMilestonePairArray[5] = this.getRequiredFields(BizRule.Condition.PropertyState, channelValue, (int) loanConditions.StateCodeValue, (string) null, (string) null);
        fieldMilestonePairArray[6] = this.getRequiredFields(BizRule.Condition.LoanDocType, channelValue, (int) loanConditions.DocTypeCodeValue, (string) null, (string) null);
        fieldMilestonePairArray[7] = this.getRequiredFields(BizRule.Condition.LoanProgram, channelValue, -1, loanConditions.LoanProgramName, (string) null);
        fieldMilestonePairArray[8] = this.getAdvancedConditionRequiredFields(loanData);
      }
      bool flag1 = loanData.GetField("NEWHUD.X354") == "Y" || Utils.CheckIf2015RespaTila(loanData.GetField("3969"));
      ArrayList arrayList = new ArrayList();
      for (int index = 0; index < fieldMilestonePairArray.Length; ++index)
      {
        if (fieldMilestonePairArray[index] != null)
        {
          foreach (FieldMilestonePair fieldMilestonePair1 in fieldMilestonePairArray[index])
          {
            bool flag2 = false;
            foreach (FieldMilestonePair fieldMilestonePair2 in arrayList)
            {
              if (fieldMilestonePair1.FieldID == fieldMilestonePair2.FieldID)
              {
                flag2 = true;
                break;
              }
            }
            if (!flag2 && (flag1 || !fieldMilestonePair1.FieldID.ToUpper().StartsWith("NEWHUD.X")) && (!flag1 || !BPM.DEAD_FIELDS_IN_NEW_HUD.Contains(fieldMilestonePair1.FieldID.ToUpper())))
              arrayList.Add((object) fieldMilestonePair1);
          }
        }
      }
      return (FieldMilestonePair[]) arrayList.ToArray(typeof (FieldMilestonePair));
    }

    private DocMilestonePair[] getRequiredDocs(
      BizRule.Condition condition,
      string condition2,
      int conditionState,
      string milestoneID,
      string conditionState2)
    {
      BizRuleInfo[] activeRule = this.GetActiveRule(condition, condition2, conditionState, milestoneID, conditionState2);
      if (activeRule == null)
        return new DocMilestonePair[0];
      MilestoneRuleInfo[] milestoneRuleInfoArray = new MilestoneRuleInfo[activeRule.Length];
      for (int index = 0; index < activeRule.Length; ++index)
        milestoneRuleInfoArray[index] = (MilestoneRuleInfo) activeRule[index];
      ArrayList arrayList = new ArrayList();
      for (int index = 0; index < milestoneRuleInfoArray.Length; ++index)
        arrayList.AddRange((ICollection) milestoneRuleInfoArray[index].Docs);
      return (DocMilestonePair[]) arrayList.ToArray(typeof (DocMilestonePair));
    }

    private TaskMilestonePair[] getRequiredTasks(
      BizRule.Condition condition,
      string condition2,
      int conditionState,
      string milestoneID,
      string conditionState2)
    {
      BizRuleInfo[] activeRule = this.GetActiveRule(condition, condition2, conditionState, milestoneID, conditionState2);
      if (activeRule == null)
        return new TaskMilestonePair[0];
      MilestoneRuleInfo[] milestoneRuleInfoArray = new MilestoneRuleInfo[activeRule.Length];
      for (int index = 0; index < activeRule.Length; ++index)
        milestoneRuleInfoArray[index] = (MilestoneRuleInfo) activeRule[index];
      ArrayList arrayList = new ArrayList();
      for (int index = 0; index < milestoneRuleInfoArray.Length; ++index)
        arrayList.AddRange((ICollection) milestoneRuleInfoArray[index].Tasks);
      return (TaskMilestonePair[]) arrayList.ToArray(typeof (TaskMilestonePair));
    }

    private FieldMilestonePair[] getRequiredFields(
      BizRule.Condition condition,
      string condition2,
      int conditionState,
      string milestoneID,
      string conditionState2)
    {
      BizRuleInfo[] activeRule = this.GetActiveRule(condition, condition2, conditionState, milestoneID, conditionState2);
      if (activeRule == null)
        return new FieldMilestonePair[0];
      MilestoneRuleInfo[] milestoneRuleInfoArray = new MilestoneRuleInfo[activeRule.Length];
      for (int index = 0; index < activeRule.Length; ++index)
        milestoneRuleInfoArray[index] = (MilestoneRuleInfo) activeRule[index];
      ArrayList arrayList = new ArrayList();
      for (int index = 0; index < milestoneRuleInfoArray.Length; ++index)
        arrayList.AddRange((ICollection) milestoneRuleInfoArray[index].Fields);
      return (FieldMilestonePair[]) arrayList.ToArray(typeof (FieldMilestonePair));
    }

    private FieldMilestonePair[] getAdvancedConditionRequiredFields(LoanData loan)
    {
      ArrayList arrayList = new ArrayList();
      ExecutionContext context = new ExecutionContext(this.session.UserInfo, loan, (IServerDataProvider) new CustomCodeSessionDataProvider(this.session.SessionObjects));
      foreach (ConditionEvaluator conditionEvaluator in this.GetConditionEvaluators())
      {
        if (conditionEvaluator.Rule.Condition == BizRule.Condition.AdvancedCoding && conditionEvaluator.AppliesTo(context))
          arrayList.AddRange((ICollection) ((MilestoneRuleInfo) conditionEvaluator.Rule).Fields);
      }
      return (FieldMilestonePair[]) arrayList.ToArray(typeof (FieldMilestonePair));
    }

    private TaskMilestonePair[] getAdvancedConditionRequiredTasks(
      string channelValue,
      LoanData loan)
    {
      ArrayList arrayList = new ArrayList();
      ExecutionContext context = new ExecutionContext(this.session.UserInfo, loan, (IServerDataProvider) new CustomCodeSessionDataProvider(this.session.SessionObjects));
      foreach (ConditionEvaluator conditionEvaluator in this.GetConditionEvaluators())
      {
        if (conditionEvaluator.Rule.Condition == BizRule.Condition.AdvancedCoding && conditionEvaluator.AppliesTo(context) && conditionEvaluator.Rule.Condition2.IndexOf(channelValue) > -1)
          arrayList.AddRange((ICollection) ((MilestoneRuleInfo) conditionEvaluator.Rule).Tasks);
      }
      return (TaskMilestonePair[]) arrayList.ToArray(typeof (TaskMilestonePair));
    }

    private DocMilestonePair[] getAdvancedConditionRequiredDocs(string channelValue, LoanData loan)
    {
      ArrayList arrayList = new ArrayList();
      ExecutionContext context = new ExecutionContext(this.session.UserInfo, loan, (IServerDataProvider) new CustomCodeSessionDataProvider(this.session.SessionObjects));
      foreach (ConditionEvaluator conditionEvaluator in this.GetConditionEvaluators())
      {
        if (conditionEvaluator.Rule.Condition == BizRule.Condition.AdvancedCoding && conditionEvaluator.AppliesTo(context) && conditionEvaluator.Rule.Condition2.IndexOf(channelValue) > -1)
          arrayList.AddRange((ICollection) ((MilestoneRuleInfo) conditionEvaluator.Rule).Docs);
      }
      return (DocMilestonePair[]) arrayList.ToArray(typeof (DocMilestonePair));
    }
  }
}
