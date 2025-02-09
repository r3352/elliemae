// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.BusinessRules.ValueRangeFieldRule
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using System;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.BusinessRules
{
  [Serializable]
  public class ValueRangeFieldRule : FieldRule
  {
    private object minValue;
    private object maxValue;

    public ValueRangeFieldRule(
      string description,
      string fieldId,
      RuleCondition condition,
      object minValue,
      object maxValue,
      string[] prereqFields,
      FieldFormat fieldFormat)
      : base(description, fieldId, condition, prereqFields, fieldFormat)
    {
      this.minValue = minValue;
      this.maxValue = maxValue;
    }

    public ValueRangeFieldRule(
      string ruleId,
      string description,
      string fieldId,
      RuleCondition condition,
      object minValue,
      object maxValue,
      string[] prereqFields,
      FieldFormat fieldFormat)
      : base(ruleId, description, fieldId, condition, prereqFields, fieldFormat)
    {
      this.minValue = minValue;
      this.maxValue = maxValue;
    }

    public object MinValue => this.minValue;

    public object MaxValue => this.maxValue;

    protected override string GetFieldRuleDefinition()
    {
      string minValue = (string) this.minValue;
      string maxValue = (string) this.maxValue;
      if (minValue == "" && maxValue == "")
        return string.Empty;
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("if Value = \"\" then Exit Sub" + Environment.NewLine);
      if (this.CurrentFieldFormat == FieldFormat.DATE)
        stringBuilder.Append("if Value = \"//\" then Exit Sub" + Environment.NewLine);
      if (this.CurrentFieldFormat == FieldFormat.MONTHDAY)
        stringBuilder.Append("if Value = \"/\" then Exit Sub" + Environment.NewLine);
      if (minValue.IndexOf("/") > -1 && maxValue.IndexOf("/") > -1)
      {
        if (this.minValue == null || this.maxValue == null)
          return string.Empty;
        stringBuilder.Append("if Value = \"//\" then Exit Sub" + Environment.NewLine);
        stringBuilder.Append("Dim lower As Date" + Environment.NewLine);
        stringBuilder.Append("Dim upper As Date" + Environment.NewLine);
        stringBuilder.Append("Dim current As Date" + Environment.NewLine);
        stringBuilder.Append("current = Value" + Environment.NewLine);
        stringBuilder.Append("lower = \"" + this.minValue + "\"" + Environment.NewLine);
        stringBuilder.Append("upper = \"" + this.maxValue + "\"" + Environment.NewLine);
        stringBuilder.Append("if current < lower or current > upper then" + Environment.NewLine + "    Fail(\"The value for field '" + this.FieldID + "' must be in the range " + this.minValue + " to " + this.maxValue + ".\")" + Environment.NewLine + "end if" + Environment.NewLine);
      }
      else if (string.Concat(this.minValue) != "" && string.Concat(this.maxValue) != "")
        stringBuilder.Append("if Value < " + this.minValue + " or Value > " + this.maxValue + " then" + Environment.NewLine + "    Fail(\"The value for field '" + this.FieldID + "' must be in the range " + this.minValue + " to " + this.maxValue + ".\")" + Environment.NewLine + "end if" + Environment.NewLine);
      else if (string.Concat(this.minValue) != "")
        stringBuilder.Append("if Value < " + this.minValue + " then" + Environment.NewLine + "    Fail(\"The value for field '" + this.FieldID + "' must be greater or equal to than " + this.minValue + ".\")" + Environment.NewLine + "end if" + Environment.NewLine);
      else if (string.Concat(this.maxValue) != "")
        stringBuilder.Append("if Value > " + this.maxValue + " then" + Environment.NewLine + "    Fail(\"The value for field '" + this.FieldID + "' must be less or equal to than " + this.maxValue + ".\")" + Environment.NewLine + "end if" + Environment.NewLine);
      return stringBuilder.ToString();
    }
  }
}
