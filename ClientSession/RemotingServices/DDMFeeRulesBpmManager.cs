// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.DDMFeeRulesBpmManager
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using EllieMae.EMLite.ClientServer;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  public class DDMFeeRulesBpmManager : ManagerBase
  {
    private IBpmManager ddmFeeRuleMgr;

    internal DDMFeeRulesBpmManager(Sessions.Session session)
      : base(session, ClientSessionCacheID.BpmDDMFeeRules)
    {
      this.ddmFeeRuleMgr = this.session.SessionObjects.BpmManager;
    }

    internal static DDMFeeRulesBpmManager Instance => Session.DefaultInstance.DDMFeeRulesBpmManager;

    internal static void RefreshInstance()
    {
      if (Session.DefaultInstance == null)
        return;
      Session.DefaultInstance.ResetDDMFeeRulesBpmManager();
    }

    public int CreateFeeRule(DDMFeeRule feeRule, bool forceToPrimaryDb = false)
    {
      int feeRule1 = 0;
      if (feeRule != null)
        feeRule1 = this.ddmFeeRuleMgr.CreateDDMFeeRule(feeRule, forceToPrimaryDb);
      return feeRule1;
    }

    public DDMFeeRule[] GetDDMFeeRuleByDataTable(string dataTableName, bool forceToPrimaryDb = false)
    {
      return this.ddmFeeRuleMgr.GetDDMFeeRuleByDataTable(dataTableName, forceToPrimaryDb);
    }

    public DDMFeeRule[] GetAllDDMFeeRules(bool forceToPrimaryDb = false)
    {
      return this.ddmFeeRuleMgr.GetAllDDMFeeRules(forceToPrimaryDb);
    }

    public DDMFeeRule[] GetAllDDMFeeRulesAndScenarios(bool activeOnly, bool forceToPrimaryDb = false)
    {
      return this.ddmFeeRuleMgr.GetAllDDMFeeRulesAndScenarios(activeOnly, forceToPrimaryDb);
    }

    public DDMFeeRule GetDDMFeeRuleAndScenarioByID(int ruleID, bool forceToPrimaryDb = false)
    {
      return this.ddmFeeRuleMgr.GetDDMFeeRuleAndScenarioByID(ruleID, forceToPrimaryDb);
    }

    public void UpdateDDMFeeRuleByID(
      int ruleID,
      DDMFeeRule feeRule,
      bool statusUpdate = false,
      bool isGlobalSetting = false,
      int activeDeActiveScenarioId = 0)
    {
      this.ddmFeeRuleMgr.UpdateDDMFeeRuleByID(ruleID, feeRule, statusUpdate, isGlobalSetting, activeDeActiveScenarioId);
    }

    public bool DDMFeeRuleExist(string ruleName, bool forceToPrimaryDb = false)
    {
      return this.ddmFeeRuleMgr.DDMFeeRuleExist(ruleName, forceToPrimaryDb);
    }

    public bool DDMFeeLineExist(string feeLine, bool forceToPrimaryDb = false)
    {
      return this.ddmFeeRuleMgr.DDMFeeLineExist(feeLine, forceToPrimaryDb);
    }

    public void DeleteDDMFeeRuleByID(int ruleID, bool forceToPrimaryDb = false)
    {
      this.ddmFeeRuleMgr.DeleteDDMFeeRuleByID(ruleID, forceToPrimaryDb);
    }
  }
}
