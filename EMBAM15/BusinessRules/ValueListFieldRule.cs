// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.BusinessRules.ValueListFieldRule
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
  public class ValueListFieldRule : FieldRule
  {
    private object[] validValues;

    public ValueListFieldRule(
      string description,
      string fieldId,
      RuleCondition condition,
      object[] validValues,
      string[] prereqFields,
      FieldFormat fieldFormat)
      : base(description, fieldId, condition, prereqFields, fieldFormat)
    {
      this.validValues = validValues;
    }

    public ValueListFieldRule(
      string ruleId,
      string description,
      string fieldId,
      RuleCondition condition,
      object[] validValues,
      string[] prereqFields,
      FieldFormat fieldFormat)
      : base(ruleId, description, fieldId, condition, prereqFields, fieldFormat)
    {
      this.validValues = validValues;
    }

    public object[] ValidValues => this.validValues;

    protected override string GetFieldRuleDefinition()
    {
      if (this.validValues == null)
        return string.Empty;
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("if Value = \"\" then Exit Sub" + Environment.NewLine);
      if (this.CurrentFieldFormat == FieldFormat.DATE)
        stringBuilder.Append("if Value = \"//\" then Exit Sub" + Environment.NewLine);
      if (this.CurrentFieldFormat == FieldFormat.MONTHDAY)
        stringBuilder.Append("if Value = \"/\" then Exit Sub" + Environment.NewLine);
      for (int index = 0; index < this.validValues.Length; ++index)
      {
        if (this.TargetFieldIsNumericFormat())
        {
          stringBuilder.Append("Value = Replace(Value, \",\", \"\")" + Environment.NewLine);
          stringBuilder.Append("If Val(Value) = Val(" + this.encode(this.validValues[index]) + ") Then Exit Sub" + Environment.NewLine);
        }
        else
          stringBuilder.Append("If Value = " + this.encode(this.validValues[index]) + " Then Exit Sub" + Environment.NewLine);
      }
      stringBuilder.Append("Fail(\"The value '\" + Value + \"' is not allowed for field '" + this.FieldID + "'.\")\n");
      return stringBuilder.ToString();
    }

    private string encode(object o) => o is string ? "\"" + o + "\"" : string.Concat(o);
  }
}
