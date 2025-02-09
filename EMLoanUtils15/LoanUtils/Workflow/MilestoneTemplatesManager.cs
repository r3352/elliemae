// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.Workflow.MilestoneTemplatesManager
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.BusinessRules;
using EllieMae.EMLite.CalculationEngine;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Customization;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Workflow;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.LoanUtils.Workflow
{
  public class MilestoneTemplatesManager
  {
    private List<string> mt;
    private bool advancedCodeErr;

    public void ResetMT() => this.mt = new List<string>();

    public List<string> MT() => this.mt;

    public List<string> GetSatisfiedMilestoneTemplate(
      LoanConditions loanConditions,
      LoanData loan,
      UserInfo userInfo,
      SessionObjects sessionObj)
    {
      this.mt = new List<string>();
      this.GetMilestoneTemplateConditions(loanConditions, loan, userInfo, sessionObj);
      return this.mt;
    }

    public FieldRuleInfo[][] GetMilestoneTemplateConditions(
      LoanConditions loanConditions,
      LoanData loan,
      UserInfo userInfo,
      SessionObjects sessionObj)
    {
      this.mt = new List<string>();
      BizRule.LoanPurpose loanPurposeValue = loanConditions.LoanPurposeValue;
      BizRule.LoanType loanTypeValue = loanConditions.LoanTypeValue;
      string channelValue = loanConditions.ChannelValue;
      return new FieldRuleInfo[4][]
      {
        this.getMilestoneTemplateConditions(sessionObj, BizRule.Condition.Null, channelValue, -1, (string) null, (string) null),
        this.getMilestoneTemplateConditions(sessionObj, BizRule.Condition.LoanPurpose, channelValue, (int) loanPurposeValue, (string) null, (string) null),
        this.getMilestoneTemplateConditions(sessionObj, BizRule.Condition.LoanType, channelValue, (int) loanTypeValue, (string) null, (string) null),
        this.getAdvancedConditionMilestoneTemplate(channelValue, loan, userInfo, sessionObj)
      };
    }

    public BizRuleInfo[] GetActiveMilestoneTemplateRule(
      SessionObjects sessionObject,
      BizRule.Condition condition,
      string condition2,
      int conditionState,
      string milestoneID,
      string conditionState2)
    {
      string conditionState1 = BizRule.ConditionStateAsString(condition, conditionState, milestoneID);
      return this.GetActiveMilestoneTemplateRule(sessionObject, condition, condition2, conditionState1, conditionState2);
    }

    public BizRuleInfo[] GetActiveMilestoneTemplateRule(
      SessionObjects sessionObject,
      BizRule.Condition condition,
      string condition2,
      string conditionState,
      string conditionState2)
    {
      List<BizRuleInfo> bizRuleInfoList = new List<BizRuleInfo>();
      foreach (BizRuleInfo milestoneTemplateRule in this.getActiveMilestoneTemplateRules(sessionObject))
      {
        if (milestoneTemplateRule.Condition == condition && milestoneTemplateRule.Condition2.IndexOf(condition2) > -1 && milestoneTemplateRule.ConditionState == conditionState && (milestoneTemplateRule.ConditionState2 ?? "") == (conditionState2 ?? ""))
          bizRuleInfoList.Add(milestoneTemplateRule);
      }
      return bizRuleInfoList.Count > 0 ? bizRuleInfoList.ToArray() : (BizRuleInfo[]) null;
    }

    private List<string> sortTemplateByOrder(
      List<string> satisfiedTemplates,
      IEnumerable<MilestoneTemplate> templates)
    {
      Dictionary<int, string> source = new Dictionary<int, string>();
      foreach (string satisfiedTemplate in satisfiedTemplates)
      {
        string satisfiedTemplateName = satisfiedTemplate;
        MilestoneTemplate milestoneTemplate = templates.FirstOrDefault<MilestoneTemplate>((Func<MilestoneTemplate, bool>) (x => x.Name == satisfiedTemplateName));
        if (milestoneTemplate != null)
          source.Add(milestoneTemplate.SortIndex, milestoneTemplate.Name);
      }
      return source.OrderByDescending<KeyValuePair<int, string>, int>((Func<KeyValuePair<int, string>, int>) (x => x.Key)).ToDictionary<KeyValuePair<int, string>, int, string>((Func<KeyValuePair<int, string>, int>) (x => x.Key), (Func<KeyValuePair<int, string>, string>) (x => x.Value)).Values.ToList<string>();
    }

    public bool? ApplyMilestoneTemplate(
      SessionObjects sessionObjects,
      LoanData loan,
      ILoanMilestoneTemplateOrchestrator orchestrator,
      MilestoneTemplate selected,
      string changeReason)
    {
      if (orchestrator == null || orchestrator.LoanConditions == null)
        return new bool?();
      MilestoneTemplatesSetting policySetting = (MilestoneTemplatesSetting) sessionObjects.StartupInfo.PolicySettings[(object) "Policies.MilestoneTemplateSettings"];
      if (!orchestrator.IsManualApply() && (policySetting == MilestoneTemplatesSetting.Manual || policySetting == MilestoneTemplatesSetting.None || loan.GetLogList().MSLock))
        return new bool?(true);
      LogList logList = loan.GetLogList();
      MilestoneTemplate milestoneTemplate1 = logList.MilestoneTemplate;
      bool msLock = logList.MSLock;
      RoleInfo[] allRoleFunctions = sessionObjects.GetCachedAllRoleFunctions();
      List<LogRecordBase> logRecords = ((IEnumerable<LogRecordBase>) logList.GetAllDatedRecords()).ToList<LogRecordBase>();
      List<string> confirmedEmailList = new List<string>();
      List<LogRecordBase> logRecordBaseList = new List<LogRecordBase>();
      LoanConditions loanConditions = orchestrator.LoanConditions;
      foreach (LogRecordBase logRecordBase in logRecords)
      {
        if (!(logRecordBase is MilestoneLog))
          logRecordBaseList.Add(logRecordBase);
      }
      logRecordBaseList.ForEach((Action<LogRecordBase>) (item => logRecords.Remove(item)));
      List<string> milestoneTemplate2 = this.GetSatisfiedMilestoneTemplate(loanConditions, loan, sessionObjects.UserInfo, sessionObjects);
      IEnumerable<MilestoneTemplate> milestoneTemplates = sessionObjects.BpmManager.GetMilestoneTemplates(true);
      List<string> sortedSatisfiedTemplates = this.sortTemplateByOrder(milestoneTemplate2, milestoneTemplates);
      MilestoneTemplate selectedTemplate = (MilestoneTemplate) null;
      if (selected != null)
        selectedTemplate = selected;
      else if (orchestrator.IsManualApply())
      {
        bool? nullable1 = orchestrator.SelectMilestoneTemplate(milestoneTemplates, sortedSatisfiedTemplates, logList.MilestoneTemplate.Name, out selectedTemplate);
        bool? nullable2 = nullable1;
        bool flag = true;
        if (!(nullable2.GetValueOrDefault() == flag & nullable2.HasValue))
          return nullable1;
      }
      else
        selectedTemplate = milestoneTemplates.FirstOrDefault<MilestoneTemplate>((Func<MilestoneTemplate, bool>) (item => item.Name == sortedSatisfiedTemplates[0]));
      if (!orchestrator.IsManualApply() && logList.MilestoneTemplate.Equals(selectedTemplate))
        return new bool?(true);
      if (this.advancedCodeErr)
      {
        RemoteLogger.Write(TraceLevel.Info, "Errors occurred from building and compiling advanced code. Templates will not get reapplied on this loan save.");
        return new bool?(true);
      }
      if (selectedTemplate.IsDefaultTemplate)
      {
        string.Join(",", (IEnumerable<string>) milestoneTemplate2);
        string str1 = string.Join(",", (IEnumerable<string>) sortedSatisfiedTemplates);
        string str2 = string.Empty;
        foreach (MilestoneTemplate milestoneTemplate3 in milestoneTemplates)
          str2 = str2 + milestoneTemplate3.Name + ",";
        str2.Trim(',');
        RemoteLogger.Write(TraceLevel.Info, "Milestone templates reset to default.  List of Active Milestone Templates:" + str2 + " List of Condition Satisfied Templates: " + (object) milestoneTemplate2 + " List of sortedTemplates:" + str1);
      }
      int num1 = 0;
      MilestoneLog[] allMilestones = logList.GetAllMilestones();
      int num2 = Math.Min(allMilestones.Length, selectedTemplate.SequentialMilestones.Count());
      for (int index = 0; index < num2 && allMilestones[index].MilestoneID.Equals(selectedTemplate.SequentialMilestones[index].MilestoneID); ++index)
        ++num1;
      List<LogRecordBase> logRecordDifference = new List<LogRecordBase>();
      foreach (MilestoneLog allMilestone in logList.GetAllMilestones())
      {
        if (selectedTemplate.SequentialMilestones.GetMilestone(allMilestone.MilestoneID) == null)
          logRecordDifference.Add((LogRecordBase) allMilestone);
      }
      Dictionary<UserInfo, List<string>> sendEmail;
      Dictionary<UserInfo, List<string>> dontSendEmail;
      this.findAllRolesLosingAccess(sessionObjects, num1, logList, allRoleFunctions, selectedTemplate, out sendEmail, out dontSendEmail);
      if (!orchestrator.MilestoneLogChangeConfirmation(selectedTemplate, logList, num1, sendEmail, dontSendEmail, logRecordDifference, new List<LogRecordBase>((IEnumerable<LogRecordBase>) logList.GetAllMilestones()), out confirmedEmailList, orchestrator.IsManualApply()))
        return new bool?(false);
      return !this.replaceTemplate(sessionObjects, orchestrator, milestoneTemplate1, logList, selectedTemplate, num1, allRoleFunctions, logRecords, changeReason, loan) ? new bool?(false) : new bool?(orchestrator.OnCompletion(confirmedEmailList));
    }

    public MilestoneTemplate CheckifMatchingMilestoneTemplate(
      SessionObjects sessionObjects,
      LoanData loan,
      ILoanMilestoneTemplateOrchestrator orchestrator)
    {
      if (orchestrator == null || orchestrator.LoanConditions == null)
        return (MilestoneTemplate) null;
      MilestoneTemplatesSetting policySetting = (MilestoneTemplatesSetting) sessionObjects.StartupInfo.PolicySettings[(object) "Policies.MilestoneTemplateSettings"];
      if (!orchestrator.IsManualApply() && (policySetting == MilestoneTemplatesSetting.Manual || policySetting == MilestoneTemplatesSetting.None || loan.GetLogList().MSLock))
        return (MilestoneTemplate) null;
      LoanConditions loanConditions = orchestrator.LoanConditions;
      MilestoneTemplate milestoneTemplate1 = loan.GetLogList().MilestoneTemplate;
      MilestoneTemplate templateTwo = (MilestoneTemplate) null;
      List<string> milestoneTemplate2 = this.GetSatisfiedMilestoneTemplate(loanConditions, loan, sessionObjects.UserInfo, sessionObjects);
      IEnumerable<MilestoneTemplate> milestoneTemplates = sessionObjects.BpmManager.GetMilestoneTemplates(true);
      if (milestoneTemplate2.Count > 0)
      {
        List<string> sortedSatisfiedTemplates = this.sortTemplateByOrder(milestoneTemplate2, milestoneTemplates);
        templateTwo = milestoneTemplates.FirstOrDefault<MilestoneTemplate>((Func<MilestoneTemplate, bool>) (item => item.Name == sortedSatisfiedTemplates[0]));
      }
      return !milestoneTemplate1.Equals(templateTwo) ? templateTwo : (MilestoneTemplate) null;
    }

    public ConditionEvaluators GetMilestoneTemplateConditionEvaluators(SessionObjects sessionObj)
    {
      BizRuleInfo[] milestoneTemplateRules = this.getActiveMilestoneTemplateRules(sessionObj);
      PerformanceMeter.Current.AddCheckpoint("Starting to compile the milestone template rules", 257, nameof (GetMilestoneTemplateConditionEvaluators), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\Workflow\\MilestoneTemplatesManager.cs");
      ConditionEvaluators conditionEvaluators = new ConditionEvaluators(milestoneTemplateRules, false);
      PerformanceMeter.Current.AddCheckpoint("Finished compiling the milestone template rules", 259, nameof (GetMilestoneTemplateConditionEvaluators), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\Workflow\\MilestoneTemplatesManager.cs");
      return conditionEvaluators;
    }

    public MilestoneTemplateEvaluator EvaluateMilestoneTemplateChanges(
      LoanData loanData,
      SessionObjects sessionObjects,
      string currentMilestoneTemplateId,
      string newMilestoneTemplateId)
    {
      try
      {
        if (string.IsNullOrEmpty(currentMilestoneTemplateId))
          throw new MilestoneTemplateInvalidIdException(string.Format("Invalid Milestone Template Id. currentMilestoneTemplateId: {0}.", (object) newMilestoneTemplateId));
        if (string.IsNullOrEmpty(newMilestoneTemplateId))
          throw new MilestoneTemplateInvalidIdException(string.Format("Invalid Milestone Template Id. NewMilestoneTemplateId: {0}.", (object) newMilestoneTemplateId));
        if (sessionObjects == null)
          throw new MilestoneTemplateException("Milestone Template Evaluation couldn't be completed.  SessionObjects parameter is null.");
        MilestoneTemplate milestoneTemplate;
        if (loanData == null)
        {
          milestoneTemplate = sessionObjects.BpmManager.GetMilestoneTemplate(currentMilestoneTemplateId);
          loanData = LoanDataMgr.NewLoanFromPlatformService(sessionObjects, false, loanFolder: "").LoanData;
        }
        else
        {
          milestoneTemplate = sessionObjects.BpmManager.GetMilestoneTemplate(currentMilestoneTemplateId);
          if (milestoneTemplate == null)
            throw new MilestoneTemplateInvalidIdException(string.Format("Invalid Milestone Template Id. ExistingMilestoneTemplateId: {0}.", (object) currentMilestoneTemplateId));
        }
        LogList logList = loanData.GetLogList();
        if (!milestoneTemplate.Equals(logList.MilestoneTemplate))
          throw new MilestoneTemplateException(string.Format("Current Milestone Template: {0} does not match with the Milestone Template of Loan Id: {1}.", (object) currentMilestoneTemplateId, (object) loanData.GUID));
        MilestoneLog[] allMilestones = logList.GetAllMilestones();
        MilestoneTemplate newMilestoneTemplate = sessionObjects.BpmManager.GetMilestoneTemplate(newMilestoneTemplateId);
        if (newMilestoneTemplate == null)
          throw new MilestoneTemplateInvalidIdException(string.Format("Invalid Milestone Template Id. NewMilestoneTemplateId: {0}.", (object) newMilestoneTemplateId));
        int differentPoint = 0;
        ((IEnumerable<MilestoneLog>) allMilestones).TakeWhile<MilestoneLog>((Func<MilestoneLog, int, bool>) ((item, index) => item.MilestoneID.Equals(newMilestoneTemplate.SequentialMilestones[index].MilestoneID))).ToList<MilestoneLog>().ForEach((Action<MilestoneLog>) (item => ++differentPoint));
        RoleInfo[] allRoleFunctions = sessionObjects.BpmManager.GetAllRoleFunctions();
        MilestoneTemplateEvaluator milestoneTemplateChanges = new MilestoneTemplateEvaluator(sessionObjects, milestoneTemplate, newMilestoneTemplate, allMilestones, differentPoint, (IEnumerable<RoleInfo>) allRoleFunctions);
        Dictionary<UserInfo, List<string>> sendEmail;
        Dictionary<UserInfo, List<string>> dontSendEmail;
        this.findAllRolesLosingAccess(sessionObjects, differentPoint, logList, allRoleFunctions, newMilestoneTemplate, out sendEmail, out dontSendEmail);
        milestoneTemplateChanges.RemovedUsers = milestoneTemplateChanges.GetUsersInfoToNotify(sendEmail, dontSendEmail);
        return milestoneTemplateChanges;
      }
      catch (MilestoneTemplateInvalidIdException ex)
      {
        throw;
      }
      catch (MilestoneTemplateException ex)
      {
        throw;
      }
      catch (Exception ex)
      {
        throw new MilestoneTemplateException("Milestone Template Evaluation couldn't be completed.", ex);
      }
    }

    private FieldRuleInfo[] getAdvancedConditionMilestoneTemplate(
      string channelValue,
      LoanData loan,
      UserInfo userInfo,
      SessionObjects sessionObj)
    {
      if (string.Compare(sessionObj.LoanManager.GetType().Name, "ServiceLoanManager", true) == 0)
      {
        FieldRuleInfo[] milestoneTemplates = sessionObj.LoanManager.GetAdvancedConditionMilestoneTemplates(loan, userInfo, sessionObj);
        foreach (BizRuleInfo bizRuleInfo in milestoneTemplates)
          this.mt.Add(bizRuleInfo.RuleName);
        return ((IEnumerable<FieldRuleInfo>) milestoneTemplates).ToArray<FieldRuleInfo>();
      }
      List<FieldRuleInfo> fieldRuleInfoList = new List<FieldRuleInfo>();
      ExecutionContext context = new ExecutionContext(userInfo, loan, (IServerDataProvider) new CustomCodeSessionDataProvider(sessionObj));
      PerformanceMeter.Current.AddCheckpoint("Starting to evaluate milestone template rules", 377, nameof (getAdvancedConditionMilestoneTemplate), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\Workflow\\MilestoneTemplatesManager.cs");
      ConditionEvaluators conditionEvaluators = this.GetMilestoneTemplateConditionEvaluators(sessionObj);
      if (conditionEvaluators.exceptionErr)
        this.advancedCodeErr = true;
      foreach (ConditionEvaluator conditionEvaluator in conditionEvaluators)
      {
        if (conditionEvaluator.Rule.Condition == BizRule.Condition.AdvancedCoding && conditionEvaluator.AppliesTo(context))
        {
          fieldRuleInfoList.Add((FieldRuleInfo) conditionEvaluator.Rule);
          this.mt.Add(conditionEvaluator.Rule.RuleName);
        }
      }
      PerformanceMeter.Current.AddCheckpoint("Finished evaluating milestone template rules", 391, nameof (getAdvancedConditionMilestoneTemplate), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\Workflow\\MilestoneTemplatesManager.cs");
      return fieldRuleInfoList.ToArray();
    }

    private FieldRuleInfo[] getMilestoneTemplateConditions(
      SessionObjects sessionObject,
      BizRule.Condition condition,
      string condition2,
      int conditionState,
      string milestoneID,
      string conditionState2)
    {
      BizRuleInfo[] milestoneTemplateRule = this.GetActiveMilestoneTemplateRule(sessionObject, condition, condition2, conditionState, milestoneID, conditionState2);
      if (milestoneTemplateRule == null)
        return new FieldRuleInfo[0];
      FieldRuleInfo[] templateConditions = new FieldRuleInfo[milestoneTemplateRule.Length];
      for (int index = 0; index < milestoneTemplateRule.Length; ++index)
      {
        templateConditions[index] = (FieldRuleInfo) milestoneTemplateRule[index];
        this.mt.Add(templateConditions[index].RuleName);
      }
      return templateConditions;
    }

    private BizRuleInfo[] getActiveMilestoneTemplateRules(SessionObjects sessionObject)
    {
      return (BizRuleInfo[]) sessionObject.StartupInfo.MilestoneTemplate.ToArray();
    }

    private bool replaceTemplate(
      SessionObjects sessionObjects,
      ILoanMilestoneTemplateOrchestrator orchestrator,
      MilestoneTemplate currentTemplate,
      LogList logList,
      MilestoneTemplate selectedTemplate,
      int differentPoint,
      RoleInfo[] roles,
      List<LogRecordBase> logRecords,
      string preChangeReason,
      LoanData loan)
    {
      try
      {
        MilestoneHistoryLog rec1 = (MilestoneHistoryLog) null;
        UserInfo userInfo = sessionObjects.UserInfo;
        bool createHistory = orchestrator.NeedToCreateHistory();
        bool flag = orchestrator.IsManualApply();
        IEnumerable<EllieMae.EMLite.Workflow.Milestone> milestones = sessionObjects.BpmManager.GetMilestones(false);
        AutoDayCountSetting policySetting = (AutoDayCountSetting) sessionObjects.StartupInfo.PolicySettings[(object) "Policies.MilestoneExpDayCount"];
        string changeReason = "";
        if (createHistory)
        {
          if (currentTemplate != null)
            changeReason = !(preChangeReason != "") ? (!flag ? "Changed Automatically" : "Changed Manually") : preChangeReason;
          rec1 = this.createMilestoneHistoryLog(logList, changeReason, userInfo.Userid);
        }
        logList.MilestoneTemplate = selectedTemplate;
        MilestoneTemplate.TemplateMilestones newMSList = selectedTemplate.SequentialMilestones;
        DateTime now = DateTime.Now;
        logList.RemoveMilestone(differentPoint);
        DateTime date = logList.GetMilestone("Started").Date;
        foreach (MilestoneLog allMilestone in logList.GetAllMilestones())
        {
          MilestoneLog log = allMilestone;
          if (!log.Done)
          {
            log.Date = this.AddDays(date, log.Days, policySetting, sessionObjects);
            EllieMae.EMLite.Workflow.Milestone milestone = milestones.FirstOrDefault<EllieMae.EMLite.Workflow.Milestone>((Func<EllieMae.EMLite.Workflow.Milestone, bool>) (x => x.MilestoneID == log.MilestoneID));
            log.TPOConnectStatus = milestone.TPOConnectStatus;
            log.ConsumerStatus = milestone.ConsumerStatus;
          }
          date = log.Date;
        }
        for (int i = differentPoint; i < newMSList.Count(); i++)
        {
          EllieMae.EMLite.Workflow.Milestone milestone = milestones.FirstOrDefault<EllieMae.EMLite.Workflow.Milestone>((Func<EllieMae.EMLite.Workflow.Milestone, bool>) (x => x.MilestoneID == newMSList[i].MilestoneID));
          MilestoneLog milestoneLog = logList.AddMilestone(milestone.Name, newMSList[i].DaysToComplete, newMSList[i].MilestoneID, newMSList[i].SortIndex, milestone.TPOConnectStatus, milestone.ConsumerStatus);
          milestoneLog.RoleID = milestone.RoleID;
          milestoneLog.RoleName = this.getRoleName(milestoneLog.RoleID, roles);
          milestoneLog.RoleRequired = milestone.RoleRequired ? "Y" : "N";
          date = this.AddDays(date, newMSList[i].DaysToComplete, policySetting, sessionObjects);
          milestoneLog.Date = date;
        }
        if (differentPoint > 1)
        {
          MilestoneLog logRecord = (MilestoneLog) logRecords[differentPoint - 1];
          EllieMae.EMLite.Workflow.Milestone milestone = milestones.FirstOrDefault<EllieMae.EMLite.Workflow.Milestone>((Func<EllieMae.EMLite.Workflow.Milestone, bool>) (x => x.MilestoneID == newMSList[differentPoint - 1].MilestoneID));
          if (logRecord.Done)
          {
            logRecord.Done = false;
            logRecord.Date = now;
          }
          if (milestone.RoleID != logRecord.RoleID)
          {
            logRecord.ClearLoanAssociate();
            logRecord.RoleID = milestone.RoleID;
            logRecord.RoleName = this.getRoleName(milestone.RoleID, roles);
            logRecord.RoleRequired = milestone.RoleRequired ? "Y" : "N";
          }
        }
        foreach (MilestoneFreeRoleLog milestoneFreeRole in logList.GetAllMilestoneFreeRoles())
        {
          MilestoneFreeRoleLog existingLog = milestoneFreeRole;
          if (selectedTemplate.FreeRoles.FirstOrDefault<TemplateFreeRole>((Func<TemplateFreeRole, bool>) (x => x.RoleID == existingLog.RoleID)) == null)
            logList.RemoveRecord((LogRecordBase) existingLog);
        }
        MilestoneFreeRoleLog[] milestoneFreeRoles = logList.GetAllMilestoneFreeRoles();
        foreach (TemplateFreeRole freeRole1 in selectedTemplate.FreeRoles)
        {
          TemplateFreeRole freeRole = freeRole1;
          if (((IEnumerable<MilestoneFreeRoleLog>) milestoneFreeRoles).FirstOrDefault<MilestoneFreeRoleLog>((Func<MilestoneFreeRoleLog, bool>) (x => x.RoleID == freeRole.RoleID)) == null)
          {
            RoleInfo roleInfo = ((IEnumerable<RoleInfo>) roles).FirstOrDefault<RoleInfo>((Func<RoleInfo, bool>) (y => y.RoleID == freeRole.RoleID));
            if (roleInfo != null)
            {
              MilestoneFreeRoleLog rec2 = new MilestoneFreeRoleLog();
              rec2.RoleID = roleInfo.RoleID;
              rec2.RoleName = roleInfo.RoleName;
              rec2.MarkAsClean();
              logList.AddRecord((LogRecordBase) rec2, true);
            }
          }
        }
        logList.MSLock = orchestrator.IsManualApply();
        if (rec1 != null)
          logList.AddRecord((LogRecordBase) rec1);
        logList.ReAssignCustomMileStones();
        logList.ReAssignTasksMilestones();
        if (loan.Calculator == null)
        {
          ILoanConfigurationInfo configurationInfo = sessionObjects.LoanManager.GetLoanConfigurationInfo();
          LoanCalculator calculator = new LoanCalculator(sessionObjects, configurationInfo, loan);
          loan.AttachCalculator((ILoanCalculator) calculator);
        }
        loan.Calculator.UpdateLenderRepresentative((LoanAssociateLog) logList.GetCurrentMilestone(), "all");
      }
      catch
      {
        return false;
      }
      return true;
    }

    private DateTime AddDays(
      DateTime date,
      int dayCount,
      AutoDayCountSetting calenderType,
      SessionObjects sessionObjects)
    {
      DateTime date1 = date;
      switch (calenderType)
      {
        case AutoDayCountSetting.CalendarDays:
          date1 = dayCount == 0 ? date1.AddMinutes(1.0) : date1.AddDays((double) dayCount);
          break;
        case AutoDayCountSetting.CompanyDays:
          try
          {
            date1 = sessionObjects.GetBusinessCalendar(CalendarType.Business).AddBusinessDays(date1, dayCount, false);
            break;
          }
          catch (ArgumentOutOfRangeException ex)
          {
            break;
          }
        default:
          int num = dayCount;
          if (num == 0)
            date1 = date1.AddMinutes(1.0);
          while (num != 0)
          {
            date1 = date1.AddDays(1.0);
            if (date1.DayOfWeek < DayOfWeek.Saturday && date1.DayOfWeek > DayOfWeek.Sunday)
              --num;
          }
          break;
      }
      return date1;
    }

    private MilestoneHistoryLog createMilestoneHistoryLog(
      LogList logList,
      string changeReason,
      string userID)
    {
      List<LogRecordBase> historyLogs = new List<LogRecordBase>();
      historyLogs.AddRange((IEnumerable<LogRecordBase>) logList.GetAllMilestones());
      historyLogs.AddRange((IEnumerable<LogRecordBase>) logList.GetAllMilestoneTaskLogs());
      historyLogs.AddRange((IEnumerable<LogRecordBase>) logList.GetAllDocuments());
      return new MilestoneHistoryLog(logList, historyLogs, userID, changeReason, logList.MilestoneTemplate, logList.MSLock, logList.MSDateLock);
    }

    private void findAllRolesLosingAccess(
      SessionObjects sessionObjects,
      int differentPoint,
      LogList list,
      RoleInfo[] roleList,
      MilestoneTemplate selectedTemplate,
      out Dictionary<UserInfo, List<string>> sendEmail,
      out Dictionary<UserInfo, List<string>> dontSendEmail)
    {
      bool flag = true;
      dontSendEmail = new Dictionary<UserInfo, List<string>>();
      sendEmail = new Dictionary<UserInfo, List<string>>();
      MilestoneLog[] allMilestones = list.GetAllMilestones();
      List<EllieMae.EMLite.Workflow.Milestone> source = (List<EllieMae.EMLite.Workflow.Milestone>) null;
      for (int index = differentPoint - 1; index < allMilestones.Length; ++index)
      {
        MilestoneLog log = ((IEnumerable<MilestoneLog>) allMilestones).ElementAt<MilestoneLog>(index);
        if (log.MilestoneID == "1")
          flag = false;
        else if (log.LoanAssociateID == "")
        {
          flag = false;
        }
        else
        {
          if (flag)
          {
            flag = false;
            if (source == null)
              source = sessionObjects.BpmManager.GetMilestones(false).ToList<EllieMae.EMLite.Workflow.Milestone>();
            if (source.FirstOrDefault<EllieMae.EMLite.Workflow.Milestone>((Func<EllieMae.EMLite.Workflow.Milestone, bool>) (x => x.MilestoneID == log.MilestoneID)).RoleID == log.RoleID)
              continue;
          }
          UserInfo user1 = sessionObjects.OrganizationManager.GetUser(log.LoanAssociateID);
          if (user1 != (UserInfo) null)
          {
            if (dontSendEmail.ContainsKey(user1))
              dontSendEmail.Remove(user1);
            if (!user1.IsAdministrator())
            {
              if (!sendEmail.ContainsKey(user1))
              {
                sendEmail.Add(user1, new List<string>()
                {
                  this.getRoleName(log.RoleID, roleList)
                });
              }
              else
              {
                List<string> stringList = sendEmail[user1];
                stringList.Add(this.getRoleName(log.RoleID, roleList));
                sendEmail[user1] = stringList;
              }
            }
          }
          else
          {
            int result;
            if (int.TryParse(log.LoanAssociateID, out result))
            {
              foreach (string userId in sessionObjects.AclGroupManager.GetUsersInGroup(result, true))
              {
                UserInfo user2 = sessionObjects.OrganizationManager.GetUser(userId);
                if (!user2.IsAdministrator())
                {
                  if (!sendEmail.ContainsKey(user2) && !dontSendEmail.ContainsKey(user2))
                    dontSendEmail.Add(user2, new List<string>()
                    {
                      this.getRoleName(log.RoleID, roleList)
                    });
                  else if (dontSendEmail.ContainsKey(user2))
                  {
                    List<string> stringList = dontSendEmail[user2];
                    stringList.Add(this.getRoleName(log.RoleID, roleList));
                    dontSendEmail[user2] = stringList;
                  }
                }
              }
            }
          }
        }
      }
      foreach (MilestoneFreeRoleLog milestoneFreeRole in list.GetAllMilestoneFreeRoles())
      {
        MilestoneFreeRoleLog freeRole = milestoneFreeRole;
        if (!(freeRole.LoanAssociateID == string.Empty) && selectedTemplate.FreeRoles.FirstOrDefault<TemplateFreeRole>((Func<TemplateFreeRole, bool>) (x => x.RoleID == freeRole.RoleID)) == null)
        {
          UserInfo user = sessionObjects.OrganizationManager.GetUser(freeRole.LoanAssociateID);
          if (user != (UserInfo) null)
          {
            if (dontSendEmail.ContainsKey(user))
              dontSendEmail.Remove(user);
            if (!user.IsAdministrator())
            {
              if (!sendEmail.ContainsKey(user))
              {
                sendEmail.Add(user, new List<string>()
                {
                  freeRole.RoleName
                });
              }
              else
              {
                List<string> stringList = sendEmail[user];
                stringList.Add(freeRole.RoleName);
                sendEmail[user] = stringList;
              }
            }
          }
        }
      }
    }

    private string getRoleName(int roleId, RoleInfo[] roles)
    {
      RoleInfo roleInfo = ((IEnumerable<RoleInfo>) roles).FirstOrDefault<RoleInfo>((Func<RoleInfo, bool>) (x => x.RoleID == roleId));
      return roleInfo != null ? roleInfo.RoleName : "";
    }
  }
}
