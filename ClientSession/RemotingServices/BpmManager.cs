// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.BpmManager
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using EllieMae.EMLite.BusinessRules;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  public abstract class BpmManager : ManagerBase
  {
    private const string ruleCacheKey = "rules";
    private const string conditionEvaluatorsCacheKey = "adv_cond_evals";
    private BizRuleType ruleType;
    private IBpmManager bpmManager;

    protected BpmManager(
      Sessions.Session session,
      BizRuleType ruleType,
      ClientSessionCacheID cacheId)
      : base(session, cacheId)
    {
      this.ruleType = ruleType;
      if (this.session == null)
        return;
      this.bpmManager = this.session.SessionObjects.BpmManager;
    }

    public IBpmManager BPMManager => this.bpmManager;

    public BizRuleInfo GetRule(int ruleId) => this.bpmManager.GetRule(this.ruleType, ruleId);

    public BizRuleInfo[] GetAllRules(bool forceToPrimaryDb = false)
    {
      return this.bpmManager.GetRules(this.ruleType, false, forceToPrimaryDb);
    }

    public BizRuleInfo[] GetAllActiveRules(bool forceToPrimaryDb = false)
    {
      return this.bpmManager.GetRules(this.ruleType, true, forceToPrimaryDb);
    }

    public BizRuleInfo[] GetRules(bool generalRules, bool forceToPrimaryDb = false)
    {
      return this.bpmManager.GetRules(this.ruleType, generalRules ? BizRule.GeneralConditions : BizRule.SpecializedConditions, false, forceToPrimaryDb);
    }

    public BizRuleInfo[] GetAllRulesFromDatabase()
    {
      this.ClearCache();
      this.bpmManager.InvalidateCache(this.ruleType);
      return this.bpmManager.GetRules(this.ruleType, false, true);
    }

    public BizRuleInfo[] GetRulesFromDatabase(bool generalRules)
    {
      this.ClearCache();
      this.bpmManager.InvalidateCache(this.ruleType);
      return this.bpmManager.GetRules(this.ruleType, generalRules ? BizRule.GeneralConditions : BizRule.SpecializedConditions, false, true);
    }

    public BizRuleInfo CreateNewRule(BizRuleInfo rule, bool isGlobalSetting = false)
    {
      this.ClearCache();
      return this.bpmManager.CreateNewRule(rule, isGlobalSetting);
    }

    public void UpdateRule(BizRuleInfo rule, bool isGlobalSetting = false)
    {
      this.ClearCache();
      this.bpmManager.UpdateRule(rule, isGlobalSetting);
    }

    public void SyncBrokerRules()
    {
      if (Session.EncompassEdition != EncompassEdition.Broker)
        return;
      ((IEnumerable<BizRuleInfo>) this.GetAllRules()).ToList<BizRuleInfo>().ForEach((Action<BizRuleInfo>) (x => this.UpdateRule((BizRuleInfo) this.UpdateBrokerRuleMilestoneSetting((LoanAccessRuleInfo) x))));
    }

    public LoanAccessRuleInfo UpdateBrokerRuleMilestoneSetting(LoanAccessRuleInfo rule)
    {
      if (Session.EncompassEdition != EncompassEdition.Broker)
        return rule;
      IEnumerable<EllieMae.EMLite.Workflow.Milestone> source = (IEnumerable<EllieMae.EMLite.Workflow.Milestone>) this.GetAllMilestones().TakeWhile<EllieMae.EMLite.Workflow.Milestone>((Func<EllieMae.EMLite.Workflow.Milestone, bool>) (y => !y.Archived)).OrderBy<EllieMae.EMLite.Workflow.Milestone, int>((Func<EllieMae.EMLite.Workflow.Milestone, int>) (x => x.SortIndex));
      EllieMae.EMLite.Workflow.Milestone milestone = source.FirstOrDefault<EllieMae.EMLite.Workflow.Milestone>((Func<EllieMae.EMLite.Workflow.Milestone, bool>) (x => x.RoleID == Utils.ParseInt((object) rule.ConditionState2, -2)));
      if (milestone != null)
      {
        rule.ConditionState = milestone.MilestoneID;
        for (int index = milestone.SortIndex - 1; index < source.Count<EllieMae.EMLite.Workflow.Milestone>() && (source.ElementAt<EllieMae.EMLite.Workflow.Milestone>(index).RoleID == -1 || string.Concat((object) source.ElementAt<EllieMae.EMLite.Workflow.Milestone>(index).RoleID) == rule.ConditionState2); ++index)
          rule.ConditionState = source.ElementAt<EllieMae.EMLite.Workflow.Milestone>(index).MilestoneID;
      }
      return rule;
    }

    public void DeleteRule(int ruleID, bool isGlobalSetting = false, bool forceToPrimaryDb = false)
    {
      this.ClearCache();
      this.bpmManager.DeleteRule(this.ruleType, ruleID, isGlobalSetting, forceToPrimaryDb);
    }

    public BizRule.ActivationReturnCode ActivateRule(
      int ruleID,
      bool isGlobalSetting = false,
      bool forceToPrimaryDb = false)
    {
      this.ClearCache();
      return this.bpmManager.ActivateRule(this.ruleType, ruleID, isGlobalSetting, forceToPrimaryDb);
    }

    public BizRule.ActivationReturnCode DeactivateRule(
      int ruleID,
      bool isGlobalSetting = false,
      bool forceToPrimaryDb = false)
    {
      this.ClearCache();
      return this.bpmManager.DeactivateRule(this.ruleType, ruleID, isGlobalSetting, forceToPrimaryDb);
    }

    internal void InitializeCache(BizRuleInfo[] activeRules)
    {
      this.SetSubjectCache("rules", (object) activeRules);
    }

    public IEnumerable<EllieMae.EMLite.Workflow.Milestone> GetAllMilestones()
    {
      IEnumerable<EllieMae.EMLite.Workflow.Milestone> subject = (IEnumerable<EllieMae.EMLite.Workflow.Milestone>) this.GetSubjectFromCache("MilestonesList");
      if (subject == null)
      {
        subject = this.bpmManager.GetMilestones(false);
        this.SetSubjectCache("MilestonesList", (object) subject);
      }
      return subject;
    }

    public void ResetMilestonesCache()
    {
      this.SetSubjectCache("MilestonesList", (object) this.bpmManager.GetMilestones(false));
    }

    public void ClearMilestoneCache()
    {
      this.bpmManager.UpdateMilestoneCache();
      this.RemoveSubjectCache("MilestonesList");
    }

    internal void InitializeCacheMT(BizRuleInfo[] activeRules)
    {
      this.SetSubjectCache("MilestoneTemplateConditions", (object) activeRules);
    }

    protected BizRuleInfo[] GetActiveMilestoneTemplateRules()
    {
      BizRuleInfo[] subject = (BizRuleInfo[]) this.GetSubjectFromCache("MilestoneTemplateConditions");
      if (subject == null)
      {
        subject = this.bpmManager.GetRules(this.ruleType, true);
        this.SetSubjectCache("rules", (object) subject);
      }
      return subject;
    }

    protected BizRuleInfo[] GetActiveRules()
    {
      BizRuleInfo[] subject = (BizRuleInfo[]) this.GetSubjectFromCache("rules");
      if (subject == null)
      {
        subject = this.bpmManager.GetRules(this.ruleType, true);
        this.SetSubjectCache("rules", (object) subject);
      }
      return subject;
    }

    public BizRuleInfo[] GetActiveRules(BizRule.Condition condition)
    {
      List<BizRuleInfo> bizRuleInfoList = new List<BizRuleInfo>();
      foreach (BizRuleInfo activeRule in this.GetActiveRules())
      {
        if (activeRule.Condition == condition)
          bizRuleInfoList.Add(activeRule);
      }
      return bizRuleInfoList.ToArray();
    }

    public BizRuleInfo[] GetActiveRule(
      BizRule.Condition condition,
      string condition2,
      string conditionState,
      string conditionState2)
    {
      ArrayList arrayList = new ArrayList();
      foreach (BizRuleInfo activeRule in this.GetActiveRules())
      {
        if (activeRule.Condition == condition && activeRule.Condition2.IndexOf(condition2) > -1 && condition == BizRule.Condition.Null)
          arrayList.Add((object) activeRule);
        else if (activeRule.Condition == condition && activeRule.Condition2.IndexOf(condition2) > -1 && activeRule.ConditionState == conditionState && (activeRule.ConditionState2 ?? "") == (conditionState2 ?? ""))
          arrayList.Add((object) activeRule);
      }
      return arrayList.Count > 0 ? (BizRuleInfo[]) arrayList.ToArray(typeof (BizRuleInfo)) : (BizRuleInfo[]) null;
    }

    public BizRuleInfo[] GetActiveMilestoneTemplateRule(
      BizRule.Condition condition,
      string condition2,
      string conditionState,
      string conditionState2)
    {
      ArrayList arrayList = new ArrayList();
      foreach (BizRuleInfo milestoneTemplateRule in this.GetActiveMilestoneTemplateRules())
      {
        if (milestoneTemplateRule.Condition == condition && milestoneTemplateRule.Condition2.IndexOf(condition2) > -1 && milestoneTemplateRule.ConditionState == conditionState && (milestoneTemplateRule.ConditionState2 ?? "") == (conditionState2 ?? ""))
          arrayList.Add((object) milestoneTemplateRule);
      }
      return arrayList.Count > 0 ? (BizRuleInfo[]) arrayList.ToArray(typeof (BizRuleInfo)) : (BizRuleInfo[]) null;
    }

    public BizRuleInfo[] GetActiveRule(
      BizRule.Condition condition,
      string condition2,
      int conditionState,
      string milestoneID,
      string conditionState2)
    {
      string conditionState1 = BizRule.ConditionStateAsString(condition, conditionState, milestoneID);
      return this.GetActiveRule(condition, condition2, conditionState1, conditionState2);
    }

    public BizRuleInfo[] GetActiveMilestoneTemplateRule(
      BizRule.Condition condition,
      string condition2,
      int conditionState,
      string milestoneID,
      string conditionState2)
    {
      string conditionState1 = BizRule.ConditionStateAsString(condition, conditionState, milestoneID);
      return this.GetActiveMilestoneTemplateRule(condition, condition2, conditionState1, conditionState2);
    }

    public ConditionEvaluators GetMilestoneTemplateConditionEvaluators()
    {
      return new ConditionEvaluators(this.GetActiveMilestoneTemplateRules(), false);
    }

    public ConditionEvaluators GetConditionEvaluators()
    {
      ConditionEvaluators subject = (ConditionEvaluators) this.GetSubjectFromCache("adv_cond_evals");
      if (subject == null)
      {
        subject = new ConditionEvaluators(this.GetActiveRules(), false);
        this.SetSubjectCache("adv_cond_evals", (object) subject);
      }
      return subject;
    }

    public string[] GetMilestoneNamesByRuleIds(int[] ruleIDs)
    {
      return this.bpmManager.GetMilestoneNamesByRuleIds(ruleIDs);
    }
  }
}
