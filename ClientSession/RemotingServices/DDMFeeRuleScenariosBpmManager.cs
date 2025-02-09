// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.DDMFeeRuleScenariosBpmManager
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using EllieMae.EMLite.ClientServer;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  public class DDMFeeRuleScenariosBpmManager : BpmManager
  {
    internal static DDMFeeRuleScenariosBpmManager Instance
    {
      get => Session.DefaultInstance.DDMFeeRuleScenariosBpmManager;
    }

    internal static void RefreshInstance()
    {
      if (Session.DefaultInstance == null)
        return;
      Session.DefaultInstance.ResetDDMFeeRuleScenariosBpmManager();
    }

    internal DDMFeeRuleScenariosBpmManager(Sessions.Session session)
      : base(session, BizRuleType.DDMFeeScenarios, ClientSessionCacheID.BpmDDMFeeRulesScenarios)
    {
    }

    public List<DDMFeeRuleScenario> GetAllRulesFromEBS(bool includeDetails)
    {
      return (List<DDMFeeRuleScenario>) null;
    }

    public List<DDMFeeRuleScenario> GetAllRules(int feeRuleId, bool forceToPrimaryDb = false)
    {
      List<DDMFeeRuleScenario> allRules = new List<DDMFeeRuleScenario>();
      foreach (BizRuleInfo rule in this.BPMManager.GetRules(BizRuleType.DDMFeeScenarios, false, forceToPrimaryDb))
      {
        if (((DDMFeeRuleScenario) rule).FeeRuleID == feeRuleId)
          allRules.Add((DDMFeeRuleScenario) rule);
      }
      return allRules;
    }

    public void UpdateRules(
      List<DDMFeeRuleScenario> modifiedRules,
      bool isGlobalSetting = false,
      bool forceToPrimaryDb = false)
    {
      List<DDMFeeRuleScenario> list1 = modifiedRules.Where<DDMFeeRuleScenario>((Func<DDMFeeRuleScenario, bool>) (scenario => scenario.Deleted)).ToList<DDMFeeRuleScenario>();
      List<DDMFeeRuleScenario> list2 = modifiedRules.Where<DDMFeeRuleScenario>((Func<DDMFeeRuleScenario, bool>) (scenario => !scenario.Deleted)).ToList<DDMFeeRuleScenario>();
      list1.ForEach((Action<DDMFeeRuleScenario>) (rule =>
      {
        if (!rule.Deleted)
          return;
        this.DeleteRule(rule.RuleID, isGlobalSetting, forceToPrimaryDb);
      }));
      list2.ForEach((Action<DDMFeeRuleScenario>) (rule =>
      {
        if (rule.RuleID > 0 && !rule.Deleted)
          this.UpdateRule((BizRuleInfo) rule, isGlobalSetting);
        else
          this.CreateNewRule((BizRuleInfo) rule, isGlobalSetting);
      }));
    }
  }
}
