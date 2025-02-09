// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.DDMViewModelConverter
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class DDMViewModelConverter
  {
    private static Dictionary<string, Dictionary<string, string>> SetFieldConditionWrapperMap;
    private static Dictionary<string, string> MipSettingsFields = new Dictionary<string, string>()
    {
      {
        "1766",
        (string) null
      },
      {
        "1760",
        (string) null
      },
      {
        "1107",
        (string) null
      },
      {
        "1199",
        (string) null
      },
      {
        "1198",
        (string) null
      },
      {
        "1205",
        (string) null
      },
      {
        "1200",
        (string) null
      },
      {
        "1201",
        (string) null
      },
      {
        "1765",
        (string) null
      },
      {
        "3531",
        (string) null
      },
      {
        "3532",
        (string) null
      },
      {
        "3533",
        (string) null
      },
      {
        "3625",
        (string) null
      },
      {
        "3262",
        (string) null
      },
      {
        "SYS.X11",
        (string) null
      },
      {
        "1209",
        (string) null
      },
      {
        "2978",
        (string) null
      },
      {
        "NEWHUD.X1707",
        (string) null
      },
      {
        "NEWHUD.X1714",
        (string) null
      },
      {
        "563",
        (string) null
      },
      {
        "1109",
        (string) null
      }
    };

    static DDMViewModelConverter()
    {
      for (int index = 3560; index <= 3566; ++index)
        DDMViewModelConverter.MipSettingsFields.Add(string.Concat((object) index), (string) null);
      DDMViewModelConverter.SetFieldConditionWrapperMap = new Dictionary<string, Dictionary<string, string>>()
      {
        {
          "1003",
          DDMViewModelConverter.MipSettingsFields
        },
        {
          "1010",
          DDMViewModelConverter.MipSettingsFields
        },
        {
          "902",
          DDMViewModelConverter.MipSettingsFields
        }
      };
    }

    public static DDMVmFieldRule ConvertDDMFieldRule(DDMFieldRule ddmFieldRule)
    {
      DDMVmFieldRule ddmVmFieldRule = new DDMVmFieldRule(ddmFieldRule.RuleName);
      if (ddmFieldRule.InitLESent)
        ddmVmFieldRule.EnableByPassCondition(ByPassCondition.LEInitialized);
      if (ddmFieldRule.Condition)
      {
        ddmVmFieldRule.EnableByPassCondition(ByPassCondition.UseAdvancedCode);
        string str = DDMViewModelConverter.RemoveNewlineCharacters(ddmFieldRule.ConditionState);
        ddmVmFieldRule.ByPassAdvancedCode = str;
      }
      int ordinalId = 0;
      if (ddmFieldRule.Scenarios != null)
      {
        foreach (DDMFieldRuleScenario scenario in ddmFieldRule.Scenarios)
        {
          DDMVmCondition ddmVmCondition = new DDMVmCondition(++ordinalId);
          DDMVmAction ddmVmAction = new DDMVmAction(ordinalId);
          ddmVmAction.ContextRuleType = "FIELD";
          ddmVmAction.ContextRuleName = ddmFieldRule.RuleName;
          ddmVmAction.ContextScenarioName = scenario.RuleName;
          string loanChannelAdvCode = DDMViewModelConverter.getLoanChannelAdvCode(scenario.Condition2);
          ddmVmCondition.Add(new DDMVmFieldPair()
          {
            FieldId = "DDM:ADVANCEDCODE",
            Value = loanChannelAdvCode
          });
          DDMVmFieldPair condition = DDMViewModelConverter.generateCondition(scenario.Condition, scenario.ConditionState, scenario.MilestoneID, scenario.RoleID);
          if (condition != null)
          {
            condition.ProcessCondition();
            ddmVmCondition.Add(condition);
          }
          ddmVmCondition.ProcessEffectiveDate(scenario.EffectiveDateInfo);
          if (scenario.FieldRuleValues != null)
          {
            List<DDMFeeRuleValue> list1 = scenario.FieldRuleValues.Where<DDMFeeRuleValue>((Func<DDMFeeRuleValue, bool>) (f => f.Field_Value_Type != fieldValueType.Table && f.Field_Value_Type != fieldValueType.ValueNotSet)).ToList<DDMFeeRuleValue>();
            List<DDMFeeRuleValue> list2 = scenario.FieldRuleValues.Where<DDMFeeRuleValue>((Func<DDMFeeRuleValue, bool>) (f => f.Field_Value_Type == fieldValueType.Table && f.Field_Value_Type != fieldValueType.ValueNotSet)).ToList<DDMFeeRuleValue>();
            foreach (DDMFeeRuleValue ruleValue in list1)
            {
              DDMVmFieldPair action = DDMViewModelConverter.generateAction(ruleValue);
              ddmVmAction.Add(action);
            }
            foreach (DDMFeeRuleValue ruleValue in list2)
            {
              DDMVmFieldPair action = DDMViewModelConverter.generateAction(ruleValue);
              ddmVmAction.Add(action);
            }
          }
          ddmVmFieldRule.Conditions.Add(ddmVmCondition);
          ddmVmFieldRule.Actions.Add(ddmVmAction);
        }
      }
      return ddmVmFieldRule;
    }

    public static DDMVmFeeRule ConvertDDMFeeRule(DDMFeeRule ddmFeeRule)
    {
      DDMVmFeeRule ddmVmFeeRule = new DDMVmFeeRule(ddmFeeRule.FeeLine, ddmFeeRule.RuleName);
      if (ddmFeeRule.InitLESent)
        ddmVmFeeRule.EnableByPassCondition(ByPassCondition.LEInitialized);
      if (ddmFeeRule.Condition)
      {
        ddmVmFeeRule.EnableByPassCondition(ByPassCondition.UseAdvancedCode);
        string str = DDMViewModelConverter.RemoveNewlineCharacters(ddmFeeRule.ConditionState);
        ddmVmFeeRule.ByPassAdvancedCode = str;
      }
      int ordinalId = 0;
      if (ddmFeeRule.Scenarios != null)
      {
        foreach (DDMFeeRuleScenario scenario in ddmFeeRule.Scenarios)
        {
          DDMVmCondition ddmVmCondition = new DDMVmCondition(++ordinalId);
          DDMVmAction ddmVmAction = new DDMVmAction(ordinalId);
          ddmVmAction.ContextRuleType = "FEE";
          ddmVmAction.ContextRuleName = ddmFeeRule.RuleName;
          ddmVmAction.ContextScenarioName = scenario.RuleName;
          string loanChannelAdvCode = DDMViewModelConverter.getLoanChannelAdvCode(scenario.Condition2);
          ddmVmCondition.Add(new DDMVmFieldPair()
          {
            FieldId = "DDM:ADVANCEDCODE",
            Value = loanChannelAdvCode
          });
          DDMVmFieldPair condition = DDMViewModelConverter.generateCondition(scenario.Condition, scenario.ConditionState, scenario.MilestoneID, scenario.RoleID);
          if (condition != null)
          {
            condition.ProcessCondition();
            ddmVmCondition.Add(condition);
          }
          ddmVmCondition.ProcessEffectiveDate(scenario.EffectiveDateInfo);
          if (scenario.FeeRuleValues != null)
          {
            List<DDMFeeRuleValue> list1 = scenario.FeeRuleValues.Where<DDMFeeRuleValue>((Func<DDMFeeRuleValue, bool>) (f => f.Field_Value_Type != fieldValueType.Table && f.Field_Value_Type != fieldValueType.ValueNotSet)).ToList<DDMFeeRuleValue>();
            List<DDMFeeRuleValue> list2 = scenario.FeeRuleValues.Where<DDMFeeRuleValue>((Func<DDMFeeRuleValue, bool>) (f => f.Field_Value_Type == fieldValueType.Table && f.Field_Value_Type != fieldValueType.ValueNotSet)).ToList<DDMFeeRuleValue>();
            foreach (DDMFeeRuleValue ruleValue in list1)
            {
              DDMVmFieldPair action = DDMViewModelConverter.generateAction(ddmFeeRule, ruleValue);
              ddmVmAction.Add(action);
            }
            foreach (DDMFeeRuleValue ruleValue in list2)
            {
              DDMVmFieldPair action = DDMViewModelConverter.generateAction(ddmFeeRule, ruleValue);
              ddmVmAction.Add(action);
            }
          }
          ddmVmFeeRule.Conditions.Add(ddmVmCondition);
          ddmVmFeeRule.Actions.Add(ddmVmAction);
        }
      }
      return ddmVmFeeRule;
    }

    public static DDMVmDataTable ConvertDDMDataTable(
      DDMDataTable ddmFeeRule,
      SessionObjects sessionObjects)
    {
      DDMVmDataTable ddmVmDataTable = new DDMVmDataTable(ddmFeeRule.Name);
      foreach (KeyValuePair<int, List<DDMDataTableFieldValue>> fieldValue1 in ddmFeeRule.FieldValues)
      {
        int key = fieldValue1.Key;
        DDMVmDataTableCondition dataTableCondition = new DDMVmDataTableCondition(key);
        DDMVmDataTableAction vmDataTableAction = new DDMVmDataTableAction(key);
        List<DDMDataTableFieldValue> fieldValue2 = ddmFeeRule.FieldValues[key];
        foreach (DDMDataTableFieldValue dataTableFieldValue in fieldValue2.Where<DDMDataTableFieldValue>((Func<DDMDataTableFieldValue, bool>) (fd => !fd.IsOutput)).OrderBy<DDMDataTableFieldValue, int>((Func<DDMDataTableFieldValue, int>) (fd => fd.ColumnId)).ToList<DDMDataTableFieldValue>())
        {
          if (dataTableFieldValue.Criteria != -1 && dataTableFieldValue.Criteria != 24)
          {
            DDMVmDataTableFieldPair dataTableFieldPair = new DDMVmDataTableFieldPair();
            dataTableFieldPair.FieldId = dataTableFieldValue.FieldId;
            dataTableFieldPair.Criteria = dataTableFieldValue.Criteria;
            dataTableFieldPair.Values = dataTableFieldValue.Values.Split('|');
            dataTableFieldPair.SessionObjects = sessionObjects;
            dataTableFieldPair.Process();
            dataTableCondition.Add(dataTableFieldPair);
          }
        }
        foreach (DDMDataTableFieldValue dataTableFieldValue in fieldValue2.Where<DDMDataTableFieldValue>((Func<DDMDataTableFieldValue, bool>) (fd => fd.IsOutput)).OrderBy<DDMDataTableFieldValue, int>((Func<DDMDataTableFieldValue, int>) (fd => fd.ColumnId)).ToList<DDMDataTableFieldValue>())
        {
          DDMVmDataTableOutput vmDataTableOutput = new DDMVmDataTableOutput();
          vmDataTableOutput.Criteria = dataTableFieldValue.Criteria;
          vmDataTableOutput.Value = dataTableFieldValue.Values;
          vmDataTableOutput.Process();
          vmDataTableAction.Add(vmDataTableOutput);
        }
        ddmVmDataTable.Conditions.Add(dataTableCondition);
        ddmVmDataTable.Actions.Add(vmDataTableAction);
      }
      return ddmVmDataTable;
    }

    private static string normalizeDbValueString(string advCode)
    {
      string pattern = "\\[([^0-9]*(420|2958|AUS\\.X56|663|1393|HMDA\\.X95)[^0-9]*)\\]\\s*=\\s*\"([^\"]*)\"";
      foreach (Match match in Regex.Matches(advCode, pattern))
      {
        string oldValue = match.Groups[0].ToString();
        string str1 = match.Groups[2].ToString();
        string str2 = match.Groups[3].ToString();
        if ((str1.Equals("420") || str1.Equals("2958") || str1.Equals("AUS.X56")) && (str2.Equals("First Lien") || str2.Equals("Second Lien")))
        {
          string newValue = oldValue.Replace("First Lien", "FirstLien").Replace("Second Lien", "SecondLien");
          advCode = advCode.Replace(oldValue, newValue);
        }
        else if (str1.Equals("663") && (str2.Equals("does") || oldValue.Contains("does not")))
        {
          string newValue = oldValue.Replace("\"does\"", "\"Y\"").Replace("\"does not\"", "\"N\"");
          advCode = advCode.Replace(oldValue, newValue);
        }
        else if ((str1.Equals("1393") || str1.Equals("HMDA.X95")) && str2.Equals("Active Loan"))
        {
          string newValue = oldValue.Replace("\"Active Loan\"", "\"\"");
          advCode = advCode.Replace(oldValue, newValue);
        }
      }
      return advCode;
    }

    public static string RemoveNewlineCharacters(string advCode)
    {
      return string.IsNullOrEmpty(advCode) ? advCode : Regex.Replace(advCode, "[\\u000A\\u000B\\u000C\\u000D\\u2028\\u2029\\u0085]+", " ");
    }

    private static DDMVmFieldPair generateCondition(
      BizRule.Condition conditionType,
      string conditionState,
      string milestonId = null,
      int roleId = 0)
    {
      DDMVmFieldPair condition = (DDMVmFieldPair) null;
      switch (conditionType)
      {
        case BizRule.Condition.Null:
          condition = new DDMVmFieldPair()
          {
            RunAlways = true
          };
          break;
        case BizRule.Condition.LoanPurpose:
          string loanPuposeByCombId = DDMViewModelConverter.getLoanPuposeByCombId(conditionState);
          condition = new DDMVmFieldPair()
          {
            FieldId = "19",
            Value = loanPuposeByCombId
          };
          break;
        case BizRule.Condition.LoanType:
          string loanTypeByComboId = DDMViewModelConverter.getLoanTypeByComboId(conditionState);
          condition = new DDMVmFieldPair()
          {
            FieldId = "1172",
            Value = loanTypeByComboId
          };
          break;
        case BizRule.Condition.LoanStatus:
          condition = new DDMVmFieldPair()
          {
            FieldId = "1393",
            Value = conditionState
          };
          break;
        case BizRule.Condition.CurrentLoanAssocMS:
          condition = new DDMVmFieldPair()
          {
            FieldId = "DDM:MILESTONELOG:" + milestonId + ":" + (object) roleId,
            Value = "Y"
          };
          break;
        case BizRule.Condition.RateLock:
          condition = new DDMVmFieldPair()
          {
            FieldId = "DDM:FIELDSTATE:2400:" + conditionState,
            Value = conditionState.Equals("0") ? "N" : "Y"
          };
          break;
        case BizRule.Condition.PropertyState:
          string propertyStateByComboId = DDMViewModelConverter.getPropertyStateByComboId(conditionState);
          condition = new DDMVmFieldPair()
          {
            FieldId = "14",
            Value = propertyStateByComboId
          };
          break;
        case BizRule.Condition.LoanDocType:
          condition = new DDMVmFieldPair()
          {
            FieldId = "MORNET.X67",
            Value = DDMViewModelConverter.getDocType(conditionState)
          };
          break;
        case BizRule.Condition.FinishedMilestone:
          condition = new DDMVmFieldPair()
          {
            FieldId = "DDM:FINISHEDMILESTONE:" + milestonId,
            Value = "Y"
          };
          break;
        case BizRule.Condition.AdvancedCoding:
          conditionState = DDMViewModelConverter.normalizeDbValueString(conditionState);
          conditionState = DDMViewModelConverter.RemoveNewlineCharacters(conditionState);
          condition = new DDMVmFieldPair()
          {
            FieldId = "DDM:ADVANCEDCODE",
            Value = conditionState
          };
          break;
        case BizRule.Condition.LoanProgram:
          condition = new DDMVmFieldPair()
          {
            FieldId = "1401",
            Value = conditionState
          };
          break;
        case BizRule.Condition.PropertyType:
          string propertyTypeByComboId = DDMViewModelConverter.getPropertyTypeByComboId(conditionState);
          condition = new DDMVmFieldPair()
          {
            FieldId = "1553",
            Value = propertyTypeByComboId
          };
          break;
        case BizRule.Condition.Occupancy:
          string occupancyByComboId = DDMViewModelConverter.getOccupancyByComboId(conditionState);
          condition = new DDMVmFieldPair()
          {
            FieldId = "1811",
            Value = occupancyByComboId
          };
          break;
      }
      return condition;
    }

    private static string getLoanTypeByComboId(string id)
    {
      switch (id)
      {
        case "1":
          return "Other";
        case "2":
          return "Conventional";
        case "3":
          return "FHA";
        case "4":
          return "VA";
        case "5":
          return "FarmersHomeAdministration";
        case "6":
          return "HELOC";
        default:
          return "";
      }
    }

    private static string getOccupancyByComboId(string id)
    {
      string occupancyByComboId = "";
      int result;
      if (int.TryParse(id, out result) && result >= 0 && (BizRule.PropertyOccupancy) result <= Enum.GetValues(typeof (BizRule.PropertyOccupancy)).Cast<BizRule.PropertyOccupancy>().Max<BizRule.PropertyOccupancy>())
        occupancyByComboId = Enum.GetName(typeof (BizRule.PropertyOccupancy), (object) result);
      return occupancyByComboId;
    }

    private static string getPropertyStateByComboId(string id)
    {
      string propertyStateByComboId = "";
      int result;
      if (int.TryParse(id, out result) && result > 0 && (USPS.StateCode) result <= Enum.GetValues(typeof (USPS.StateCode)).Cast<USPS.StateCode>().Max<USPS.StateCode>())
        propertyStateByComboId = Enum.GetName(typeof (USPS.StateCode), (object) result);
      return propertyStateByComboId;
    }

    private static string getPropertyTypeByComboId(string id)
    {
      string propertyTypeByComboId = "";
      int result;
      if (int.TryParse(id, out result) && result >= 0 && result < BizRule.PropertyTypeStrings.Length && !BizRule.PropertyTypeStrings[result].Equals("Unknown", StringComparison.InvariantCultureIgnoreCase))
        propertyTypeByComboId = BizRule.PropertyTypeStrings[result];
      return propertyTypeByComboId;
    }

    private static string getLoanPuposeByCombId(string id)
    {
      string loanPuposeByCombId = "Invalid_Loan_Puporse";
      int result;
      if (int.TryParse(id, out result) && result >= 0 && (BizRule.LoanPurpose) result <= Enum.GetValues(typeof (BizRule.LoanPurpose)).Cast<BizRule.LoanPurpose>().Max<BizRule.LoanPurpose>())
      {
        switch (result)
        {
          case 0:
            return "";
          case 1:
            return "Other";
          case 2:
            return "Purchase";
          case 3:
            return "Cash-Out Refinance";
          case 4:
            return "NoCash-Out Refinance";
          case 5:
            return "ConstructionOnly";
          case 6:
            return "ConstructionToPermanent";
        }
      }
      return loanPuposeByCombId;
    }

    private static DDMVmFieldPair generateAction(DDMFeeRule feeRule, DDMFeeRuleValue ruleValue)
    {
      Func<DDMFeeRule, DDMFeeRuleValue, DDMVmFieldPair> baseActionFunc = DDMViewModelConverter.generateBaseActionFunc();
      return DDMViewModelConverter.injectMIPSettingToActionFunc()(baseActionFunc(feeRule, ruleValue));
    }

    private static DDMVmFieldPair generateAction(DDMFeeRuleValue ruleValue)
    {
      return DDMViewModelConverter.generateAction((DDMFeeRule) null, ruleValue);
    }

    private static Func<DDMFeeRule, DDMFeeRuleValue, DDMVmFieldPair> generateBaseActionFunc()
    {
      return (Func<DDMFeeRule, DDMFeeRuleValue, DDMVmFieldPair>) ((feeRule, ruleValue) =>
      {
        DDMVmFieldPair baseActionFunc = new DDMVmFieldPair();
        switch (ruleValue.Field_Value_Type)
        {
          case fieldValueType.SpecificValue:
            baseActionFunc.FieldId = ruleValue.FieldID;
            baseActionFunc.Value = Utils.EscapeDoubleQuotesForVB(ruleValue.Field_Value);
            break;
          case fieldValueType.Table:
            baseActionFunc.FieldId = ruleValue.FieldID;
            baseActionFunc.Value = "DDM[DDM:USERTABLE:" + ruleValue.Field_Value + "]";
            break;
          case fieldValueType.Calculation:
            baseActionFunc.FieldId = ruleValue.FieldID;
            string str = DDMViewModelConverter.RemoveNewlineCharacters(ruleValue.Field_Value);
            baseActionFunc.Value = "DDM(" + str + ")";
            break;
          case fieldValueType.ClearValueInLoanFile:
            baseActionFunc.FieldId = ruleValue.FieldID;
            baseActionFunc.Value = "";
            break;
          case fieldValueType.SystemTable:
            baseActionFunc.FieldId = "DDM:SYSTEMTABLE:" + ruleValue.FieldID;
            baseActionFunc.Value = ruleValue.Field_Value;
            break;
          case fieldValueType.FeeManagement:
            baseActionFunc.FieldId = "DDM:FEEMANAGEMENT:" + ruleValue.FieldID;
            baseActionFunc.Value = ruleValue.Field_Value;
            break;
          case fieldValueType.UseCalculatedValue:
            baseActionFunc.FieldId = "DDM:USECALCVALUE:" + ruleValue.FieldID;
            baseActionFunc.Value = "";
            break;
          default:
            throw new NotSupportedException("Invalid Field-Value-Type in DDM ViewObject Conversion");
        }
        if (feeRule != null)
          baseActionFunc.FeeRuleLine = feeRule.FeeLine;
        return baseActionFunc;
      });
    }

    private static Func<DDMVmFieldPair, DDMVmFieldPair> injectMIPSettingToActionFunc()
    {
      return (Func<DDMVmFieldPair, DDMVmFieldPair>) (action =>
      {
        if (action != null && !string.IsNullOrEmpty(action.FeeRuleLine) && !action.FieldId.ToLower().StartsWith("ddm:") && DDMViewModelConverter.SetFieldConditionWrapperMap.ContainsKey(action.FeeRuleLine) && DDMViewModelConverter.SetFieldConditionWrapperMap[action.FeeRuleLine].ContainsKey(action.FieldId))
        {
          action.NeedConditionWrapper = true;
          switch (action.FeeRuleLine)
          {
            case "1003":
              action.ConditionWrapper = "[1172] <> \"FarmersHomeAdministration\"";
              break;
            case "1010":
              action.ConditionWrapper = "[1172] = \"FarmersHomeAdministration\"";
              break;
            default:
              action.NeedConditionWrapper = false;
              break;
          }
        }
        return action;
      });
    }

    private static string getDocType(string fieldValue)
    {
      string docType = string.Empty;
      switch (fieldValue)
      {
        case "0":
          docType = string.Empty;
          break;
        case "1":
          docType = "Alternative";
          break;
        case "10":
          docType = "NoIncomeOn1003";
          break;
        case "11":
          docType = "NoVerificationOfStatedIncomeEmploymentOrAssets";
          break;
        case "12":
          docType = "NoVerificationOfStatedIncomeOrAssets";
          break;
        case "13":
          docType = "NoVerificationOfStatedAssets";
          break;
        case "14":
          docType = "NoVerificationOfStatedIncomeOrEmployment";
          break;
        case "15":
          docType = "NoVerificationOfStatedIncome";
          break;
        case "16":
          docType = "VerbalVerificationOfEmployment";
          break;
        case "17":
          docType = "OnePaystub";
          break;
        case "18":
          docType = "Reduced";
          break;
        case "19":
          docType = "OnePaystubAndVerbalVerificationOfEmployment";
          break;
        case "2":
          docType = "StreamlineRefinance";
          break;
        case "20":
          docType = "OnePaystubAndOneW2AndVerbalVerificationOfEmploymentOrOneYear1040";
          break;
        case "21":
          docType = "NoIncomeNoEmploymentNoAssetsOn1003";
          break;
        case "3":
          docType = "NoDocumentation";
          break;
        case "4":
          docType = "NoRatio";
          break;
        case "5":
          docType = "LimitedDocumentation";
          break;
        case "6":
          docType = "FullDocumentation";
          break;
        case "7":
          docType = "NoDepositVerificationEmploymentVerificationOrIncomeVerification";
          break;
        case "8":
          docType = "NoDepositVerification";
          break;
        case "9":
          docType = "NoEmploymentVerificationOrIncomeVerification";
          break;
      }
      return docType;
    }

    private static string getLoanChannelAdvCode(string channelOptions)
    {
      List<string> stringList = new List<string>();
      for (int index = 0; index < channelOptions.Length; ++index)
      {
        switch (channelOptions[index])
        {
          case '0':
            stringList.Add("");
            break;
          case '1':
            stringList.Add("Banked - Retail");
            break;
          case '2':
            stringList.Add("Banked - Wholesale");
            break;
          case '3':
            stringList.Add("Brokered");
            break;
          case '4':
            stringList.Add("Correspondent");
            break;
        }
      }
      if (stringList.Count == 0)
        return "False";
      return stringList.Count == 5 ? "True" : "Match([2626], \"" + string.Join("\", \"", stringList.ToArray()) + "\") >= 0";
    }
  }
}
