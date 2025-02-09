// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.BPM
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using EllieMae.EMLite.ClientServer;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  public class BPM
  {
    private Sessions.Session session;
    public static List<string> DEAD_FIELDS_IN_NEW_HUD = new List<string>((IEnumerable<string>) new string[231]
    {
      "SYS.X251",
      "SYS.X253",
      "SYS.X255",
      "SYS.X257",
      "SYS.X259",
      "SYS.X261",
      "SYS.X263",
      "SYS.X265",
      "SYS.X267",
      "SYS.X269",
      "SYS.X271",
      "SYS.X273",
      "SYS.X275",
      "SYS.X277",
      "SYS.X279",
      "SYS.X281",
      "SYS.X283",
      "SYS.X285",
      "SYS.X287",
      "SYS.X287",
      "SYS.X291",
      "SYS.X296",
      "SYS.X301",
      "SYS.X136",
      "SYS.X137",
      "SYS.X231",
      "SYS.X232",
      "SYS.X138",
      "SYS.X139",
      "SYS.X140",
      "SYS.X147",
      "SYS.X141",
      "SYS.X233",
      "SYS.X142",
      "SYS.X234",
      "SYS.X149",
      "SYS.X150",
      "SYS.X151",
      "SYS.X152",
      "SYS.X153",
      "SYS.X154",
      "SYS.X155",
      "SYS.X156",
      "SYS.X243",
      "SYS.X293",
      "SYS.X298",
      "SYS.X65",
      "SYS.X66",
      "SYS.X67",
      "SYS.X68",
      "SYS.X69",
      "SYS.X126",
      "SYS.X127",
      "SYS.X76",
      "SYS.X70",
      "SYS.X202",
      "SYS.X71",
      "SYS.X204",
      "SYS.X78",
      "SYS.X79",
      "SYS.X80",
      "SYS.X81",
      "SYS.X82",
      "SYS.X83",
      "SYS.X84",
      "SYS.X85",
      "SYS.X216",
      "SYS.X295",
      "SYS.X300",
      "SYS.X252",
      "SYS.X254",
      "SYS.X260",
      "SYS.X262",
      "SYS.X264",
      "SYS.X266",
      "SYS.X270",
      "SYS.X272",
      "SYS.X274",
      "SYS.X290",
      "SYS.X292",
      "SYS.X297",
      "SYS.X302",
      "SYS.X303",
      "SYS.X305",
      "SYS.X307",
      "SYS.X391",
      "SYS.X311",
      "SYS.X312",
      "SYS.X313",
      "SYS.X315",
      "SYS.X157",
      "SYS.X158",
      "SYS.X388",
      "SYS.X86",
      "SYS.X87",
      "SYS.X88",
      "SYS.X390",
      "SYS.X77",
      "SYS.X89",
      "SYS.X128",
      "SYS.X206",
      "SYS.X392",
      "SYS.X317",
      "SYS.X319",
      "SYS.X321",
      "SYS.X323",
      "SYS.X325",
      "SYS.X327",
      "SYS.X329",
      "SYS.X331",
      "SYS.X162",
      "SYS.X163",
      "SYS.X164",
      "SYS.X165",
      "SYS.X167",
      "SYS.X239",
      "SYS.X168",
      "SYS.X169",
      "SYS.X91",
      "SYS.X92",
      "SYS.X129",
      "SYS.X93",
      "SYS.X94",
      "SYS.X208",
      "SYS.X95",
      "SYS.X96",
      "SYS.X333",
      "SYS.X403",
      "SYS.X405",
      "SYS.X407",
      "SYS.X335",
      "SYS.X337",
      "SYS.X339",
      "SYS.X341",
      "SYS.X343",
      "SYS.X345",
      "SYS.X347",
      "SYS.X349",
      "SYS.X351",
      "SYS.X353",
      "SYS.X170",
      "SYS.X171",
      "SYS.X172",
      "SYS.X173",
      "SYS.X174",
      "SYS.X175",
      "SYS.X176",
      "SYS.X177",
      "SYS.X181",
      "SYS.X240",
      "SYS.X244",
      "SYS.X245",
      "SYS.X246",
      "SYS.X247",
      "SYS.X97",
      "SYS.X131",
      "SYS.X132",
      "SYS.X133",
      "SYS.X98",
      "SYS.X99",
      "SYS.X100",
      "SYS.X101",
      "SYS.X103",
      "SYS.X210",
      "SYS.X218",
      "SYS.X220",
      "SYS.X222",
      "SYS.X224",
      "SYS.X334",
      "SYS.X404",
      "SYS.X406",
      "SYS.X408",
      "SYS.X336",
      "SYS.X338",
      "SYS.X340",
      "SYS.X342",
      "SYS.X344",
      "SYS.X346",
      "SYS.X355",
      "SYS.X357",
      "SYS.X359",
      "SYS.X361",
      "SYS.X363",
      "SYS.X365",
      "SYS.X182",
      "SYS.X183",
      "SYS.X184",
      "SYS.X185",
      "SYS.X241",
      "SYS.X242",
      "SYS.X104",
      "SYS.X105",
      "SYS.X106",
      "SYS.X107",
      "SYS.X212",
      "SYS.X214",
      "SYS.X356",
      "SYS.X358",
      "SYS.X360",
      "SYS.X362",
      "SYS.X364",
      "SYS.X366",
      "SYS.X370",
      "SYS.X372",
      "SYS.X374",
      "SYS.X376",
      "SYS.X378",
      "SYS.X380",
      "SYS.X382",
      "SYS.X384",
      "SYS.X386",
      "SYS.X186",
      "SYS.X187",
      "SYS.X190",
      "SYS.X191",
      "SYS.X192",
      "SYS.X193",
      "SYS.X248",
      "SYS.X249",
      "SYS.X250",
      "SYS.X108",
      "SYS.X109",
      "SYS.X112",
      "SYS.X113",
      "SYS.X114",
      "SYS.X115",
      "SYS.X226",
      "SYS.X228",
      "SYS.X230",
      "SYS.X371",
      "SYS.X373"
    });

    internal BPM(Sessions.Session session) => this.session = session;

    public object GetBpmManager(BpmCategory category)
    {
      switch (category)
      {
        case BpmCategory.Workflow:
          return (object) this.session.WorkflowManager;
        case BpmCategory.MilestoneRules:
          return (object) this.session.MilestoneRulesBpmManager;
        case BpmCategory.LoanAccess:
          return (object) this.session.LoanAccessBpmManager;
        case BpmCategory.FieldAccess:
          return (object) this.session.FieldAccessBpmManager;
        case BpmCategory.FieldRules:
          return (object) this.session.FieldRulesBpmManager;
        case BpmCategory.InputForms:
          return (object) this.session.InputFormsBpmManager;
        case BpmCategory.LoanFolder:
          return (object) this.session.LoanFolderRuleManager;
        case BpmCategory.Document:
          return (object) this.session.DocumentAccessRuleManager;
        case BpmCategory.Triggers:
          return (object) this.session.TriggersBpmManager;
        case BpmCategory.PrintForms:
          return (object) this.session.PrintFormsBpmManager;
        case BpmCategory.PrintSelection:
          return (object) this.session.PrintSelectionBpmManager;
        case BpmCategory.AutomatedConditions:
          return (object) this.session.AutomatedConditionsBpmManager;
        case BpmCategory.Milestones:
          return (object) this.session.MilestoneTemplatesBpmManager;
        case BpmCategory.LoanActionAccess:
          return (object) this.session.LoanActionAccessBpmManager;
        case BpmCategory.LoanActionCompletionRules:
          return (object) this.session.LoanActionCompletionRulesBpmManager;
        case BpmCategory.AutoLockExclusionRules:
          return (object) this.session.AutoLockExclusionRulesBpmManager;
        case BpmCategory.DDMFeeScanarioRules:
          return (object) this.session.DDMFeeRuleScenariosBpmManager;
        case BpmCategory.DDMFeeRules:
          return (object) this.session.DDMFeeRulesBpmManager;
        case BpmCategory.DDMDataPopTiming:
          return (object) this.session.DDMDataPopTimingBpmManager;
        case BpmCategory.DDMFieldRules:
          return (object) this.session.DDMFieldRulesBpmManager;
        case BpmCategory.DDMFieldScenarioRules:
          return (object) this.session.DDMFieldRuleScenariosBpmManager;
        case BpmCategory.DDMDataTables:
          return (object) this.session.DDMDataTableBpmManager;
        case BpmCategory.DDMDataTableFields:
          return (object) this.session.DDMDataTableFieldBpmManager;
        case BpmCategory.AutomatedPurchaseConditions:
          return (object) this.session.AutomatedPurchaseConditionsBpmManager;
        case BpmCategory.AutomatedEnhancedConditions:
          return (object) this.session.AutomatedEnhancedConditionsBpmManager;
        default:
          throw new Exception("Unknown category " + category.ToString());
      }
    }

    public BpmManager GetBpmManager(BizRuleType ruleType)
    {
      BpmCategory category;
      switch (ruleType)
      {
        case BizRuleType.MilestoneRules:
          category = BpmCategory.MilestoneRules;
          break;
        case BizRuleType.LoanAccess:
          category = BpmCategory.LoanAccess;
          break;
        case BizRuleType.FieldAccess:
          category = BpmCategory.FieldAccess;
          break;
        case BizRuleType.FieldRules:
          category = BpmCategory.FieldRules;
          break;
        case BizRuleType.InputForms:
          category = BpmCategory.InputForms;
          break;
        case BizRuleType.Triggers:
          category = BpmCategory.Triggers;
          break;
        case BizRuleType.PrintForms:
          category = BpmCategory.PrintForms;
          break;
        case BizRuleType.PrintSelection:
          category = BpmCategory.PrintSelection;
          break;
        case BizRuleType.AutomatedConditions:
          category = BpmCategory.AutomatedConditions;
          break;
        case BizRuleType.MilestoneTemplateConditions:
          category = BpmCategory.Milestones;
          break;
        case BizRuleType.LoanActionAccess:
          category = BpmCategory.LoanActionAccess;
          break;
        case BizRuleType.LoanActionCompletionRules:
          category = BpmCategory.LoanActionCompletionRules;
          break;
        case BizRuleType.AutoLockExclusionRules:
          category = BpmCategory.AutoLockExclusionRules;
          break;
        case BizRuleType.DDMFeeScenarios:
          category = BpmCategory.DDMFeeScanarioRules;
          break;
        case BizRuleType.DDMDataPopTiming:
          category = BpmCategory.DDMDataPopTiming;
          break;
        case BizRuleType.DDMFieldScenarios:
          category = BpmCategory.DDMFieldScenarioRules;
          break;
        case BizRuleType.AutomatedEnhancedConditions:
          category = BpmCategory.AutomatedEnhancedConditions;
          break;
        default:
          throw new Exception("Unknown rule type " + ruleType.ToString());
      }
      return (BpmManager) this.GetBpmManager(category);
    }

    public void PreloadRules(BizRuleInfo[] activeRules)
    {
      Dictionary<BizRuleType, List<BizRuleInfo>> dictionary = new Dictionary<BizRuleType, List<BizRuleInfo>>();
      foreach (BizRuleType bizRuleType in BizRule.BizRuleTypes)
        dictionary[bizRuleType] = new List<BizRuleInfo>();
      foreach (BizRuleInfo activeRule in activeRules)
        dictionary[activeRule.RuleType].Add(activeRule);
      foreach (BizRuleType bizRuleType in BizRule.BizRuleTypes)
        this.GetBpmManager(bizRuleType).InitializeCache(dictionary[bizRuleType].ToArray());
    }

    public void PreloadMilestoneTemplateConditions(List<FieldRuleInfo> conditionList)
    {
      this.GetBpmManager(BizRuleType.MilestoneTemplateConditions).InitializeCacheMT((BizRuleInfo[]) conditionList.ToArray());
    }

    public void RefreshBpmManagerInstances()
    {
      this.session.ResetWorkflowManager();
      this.session.ResetLoanFolderRuleManager();
      this.session.ResetMilestoneRulesBpmManager();
      this.session.ResetLoanActionCompletionRulesBpmManager();
      this.session.ResetFieldRulesBpmManager();
      this.session.ResetLoanAccessBpmManager();
      this.session.ResetFieldAccessBpmManager();
      this.session.ResetInputFormsBpmManager();
      this.session.ResetDocumentAccessRuleManager();
      this.session.ResetTriggersBpmManager();
      this.session.ResetPrintFormsBpmManager();
      this.session.ResetPrintSelectionBpmManager();
      this.session.ResetAutomatedConditionsBpmManager();
      this.session.ResetLoanActionAccessBpmManager();
      this.session.ResetAutoLockExclusionRulesBpmManager();
      this.session.ResetDDMFeeRuleScenariosBpmManager();
      this.session.ResetDDMFieldRuleScenariosBpmManager();
      this.session.ResetAutomatedEnhancedConditionsBpmManager();
    }
  }
}
