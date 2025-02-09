// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.DDMFeeRuleValue
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class DDMFeeRuleValue
  {
    public const string FEE_RULE_VALUE_ID = "ruleValueID�";
    public const string FEE_RULE_SCENARIO_ID = "feeRuleScenarioID�";
    public const string FIELD_TYPE = "fieldType�";
    public const string FIELD_ID = "fieldID�";
    public const string FIELD_NAME = "fieldName�";
    public const string FIELD_VALUE = "fieldValue�";
    public const string FIELD_VALUE_TYPE = "fieldValueType�";
    public static Dictionary<string, fieldValueType> valueToTypeTable = new Dictionary<string, fieldValueType>()
    {
      {
        "No Value Set",
        fieldValueType.ValueNotSet
      },
      {
        "Specific Value",
        fieldValueType.SpecificValue
      },
      {
        "Table",
        fieldValueType.Table
      },
      {
        "Calculation",
        fieldValueType.Calculation
      },
      {
        "Clear value in loan file",
        fieldValueType.ClearValueInLoanFile
      },
      {
        "Use Calculated Value",
        fieldValueType.UseCalculatedValue
      }
    };
    public static Dictionary<fieldValueType, string> typeToValueTable = new Dictionary<fieldValueType, string>()
    {
      {
        fieldValueType.ValueNotSet,
        "No Value Set"
      },
      {
        fieldValueType.SpecificValue,
        "Specific Value"
      },
      {
        fieldValueType.Table,
        "Table"
      },
      {
        fieldValueType.Calculation,
        "Calculation"
      },
      {
        fieldValueType.ClearValueInLoanFile,
        "Clear value in loan file"
      },
      {
        fieldValueType.FeeManagement,
        "Specific Value"
      },
      {
        fieldValueType.SystemTable,
        "Table"
      },
      {
        fieldValueType.UseCalculatedValue,
        "Use Calculated Value"
      }
    };

    public int RuleID { get; set; }

    public int RuleScenarioID { get; set; }

    public string FieldID { get; set; }

    [CLSCompliant(false)]
    public FieldFormat Field_Type { get; set; }

    [CLSCompliant(false)]
    public string Field_Name { get; set; }

    [CLSCompliant(false)]
    public string Field_Value { get; set; }

    [CLSCompliant(false)]
    public fieldValueType Field_Value_Type { get; set; }

    public DDMFeeRuleValue()
    {
    }

    public DDMFeeRuleValue(DataRow r, bool isFeeRule = true)
    {
      this.RuleID = Convert.ToInt32(r["ruleValueID"]);
      this.RuleScenarioID = !isFeeRule ? Convert.ToInt32(r["fieldRuleScenarioID"]) : Convert.ToInt32(r["feeRuleScenarioID"]);
      this.Field_Type = (FieldFormat) Convert.ToInt32(r["fieldType"]);
      this.Field_Value = Convert.ToString(r["fieldValue"]);
      this.FieldID = Convert.ToString(r["fieldID"]);
      this.Field_Name = Convert.ToString(r["fieldName"]);
      this.Field_Value_Type = (fieldValueType) Convert.ToInt32(r["fieldValueType"]);
    }
  }
}
