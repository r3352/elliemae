// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.DDMFieldRuleScenariosBpmManager
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
  public class DDMFieldRuleScenariosBpmManager : BpmManager
  {
    internal static DDMFieldRuleScenariosBpmManager Instance
    {
      get => Session.DefaultInstance.DDMFieldRuleScenariosBpmManager;
    }

    internal static void RefreshInstance()
    {
      if (Session.DefaultInstance == null)
        return;
      Session.DefaultInstance.ResetDDMFeeRuleScenariosBpmManager();
    }

    internal DDMFieldRuleScenariosBpmManager(Sessions.Session session)
      : base(session, BizRuleType.DDMFieldScenarios, ClientSessionCacheID.BpmDDMFieldRulesScenarios)
    {
    }

    public List<DDMFieldRuleScenario> GetAllRulesFromEBS(bool includeDetails)
    {
      return (List<DDMFieldRuleScenario>) null;
    }

    public List<DDMFieldRuleScenario> GetAllRules(int fieldRuleId, bool forceToPrimaryDb = false)
    {
      List<DDMFieldRuleScenario> allRules = new List<DDMFieldRuleScenario>();
      foreach (BizRuleInfo rule in this.BPMManager.GetRules(BizRuleType.DDMFieldScenarios, false, forceToPrimaryDb))
      {
        if (((DDMFieldRuleScenario) rule).FieldRuleID == fieldRuleId)
          allRules.Add((DDMFieldRuleScenario) rule);
      }
      return allRules;
    }

    public void UpdateRules(
      List<DDMFieldRuleScenario> modifiedRules,
      bool isGlobalSetting = false,
      bool forceToPrimaryDb = false)
    {
      List<DDMFieldRuleScenario> list1 = modifiedRules.Where<DDMFieldRuleScenario>((Func<DDMFieldRuleScenario, bool>) (scenario => scenario.Deleted)).ToList<DDMFieldRuleScenario>();
      List<DDMFieldRuleScenario> list2 = modifiedRules.Where<DDMFieldRuleScenario>((Func<DDMFieldRuleScenario, bool>) (scenario => !scenario.Deleted)).ToList<DDMFieldRuleScenario>();
      list1.ForEach((Action<DDMFieldRuleScenario>) (rule =>
      {
        if (!rule.Deleted)
          return;
        this.DeleteRule(rule.RuleID, isGlobalSetting, forceToPrimaryDb);
      }));
      list2.ForEach((Action<DDMFieldRuleScenario>) (rule =>
      {
        if (rule.RuleID > 0 && !rule.Deleted)
          this.UpdateRule((BizRuleInfo) rule, isGlobalSetting);
        else
          this.CreateNewRule((BizRuleInfo) rule, isGlobalSetting);
      }));
    }
  }
}
