// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.DDMFieldRulesBpmManager
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using EllieMae.EMLite.ClientServer;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  public class DDMFieldRulesBpmManager : ManagerBase
  {
    private IBpmManager ddmFieldRuleMgr;

    internal DDMFieldRulesBpmManager(Sessions.Session session)
      : base(session, ClientSessionCacheID.BpmDDMFieldRules)
    {
      this.ddmFieldRuleMgr = this.session.SessionObjects.BpmManager;
    }

    internal static DDMFieldRulesBpmManager Instance
    {
      get => Session.DefaultInstance.DDMFieldRulesBpmManager;
    }

    internal static void RefreshInstance()
    {
      if (Session.DefaultInstance == null)
        return;
      Session.DefaultInstance.ResetDDMFieldRulesBpmManager();
    }

    public int CreateFieldRule(DDMFieldRule fieldRule)
    {
      int fieldRule1 = 0;
      if (fieldRule != null)
        fieldRule1 = this.ddmFieldRuleMgr.CreateDDMFieldRule(fieldRule);
      return fieldRule1;
    }

    public int UpdateFieldRule(
      DDMFieldRule fieldRule,
      bool statusUpdate = false,
      bool isGlobalSetting = false,
      int activeDeActiveScenarioId = 0)
    {
      int num = 0;
      if (fieldRule != null)
        num = this.ddmFieldRuleMgr.UpdateDDMFieldRule(fieldRule, statusUpdate, isGlobalSetting, activeDeActiveScenarioId);
      return num;
    }

    public DDMFieldRule[] GetAllDDMFieldRules(bool forceToPrimaryDb = false)
    {
      return this.ddmFieldRuleMgr.GetAllDDMFieldRules(forceToPrimaryDb);
    }

    public DDMFieldRule[] GetAllDDMFieldRulesAndScenarios(bool forceToPrimaryDb = false)
    {
      return this.ddmFieldRuleMgr.GetAllDDMFieldRulesAndScenarios(true, forceToPrimaryDb: forceToPrimaryDb);
    }

    public DDMFieldRule[] GetDDMFieldRuleByDataTable(string dataTableName, bool forceToPrimaryDb = false)
    {
      return this.ddmFieldRuleMgr.GetDDMFieldRuleByDataTable(dataTableName, forceToPrimaryDb);
    }

    public bool DDMFieldRuleExist(string ruleName, bool forceToPrimaryDb = false)
    {
      return this.ddmFieldRuleMgr.DDMFieldRuleExist(ruleName, forceToPrimaryDb);
    }

    public bool DDMFieldsExistInFieldRules(string fields, bool forceToPrimaryDb = false)
    {
      return this.ddmFieldRuleMgr.DDMFieldsExistInFieldRules(fields, forceToPrimaryDb);
    }

    public void DeleteDDMFieldRuleByID(int ruleID, bool forceToPrimaryDb = false)
    {
      this.ddmFieldRuleMgr.DeleteDDMFieldRuleByID(ruleID, forceToPrimaryDb);
    }
  }
}
