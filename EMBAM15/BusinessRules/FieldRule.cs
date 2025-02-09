// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.BusinessRules.FieldRule
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
  public abstract class FieldRule : CodedBusinessRule
  {
    private string fieldId;
    private string[] prereqFields;
    private FieldFormat fieldFormat;

    protected FieldRule(
      string ruleId,
      string description,
      string fieldId,
      RuleCondition condition,
      string[] prereqFields,
      FieldFormat fieldFormat)
      : base(ruleId, description, condition)
    {
      this.fieldId = fieldId;
      this.prereqFields = prereqFields;
      this.fieldFormat = fieldFormat;
    }

    protected FieldRule(
      string description,
      string fieldId,
      RuleCondition condition,
      string[] prereqFields,
      FieldFormat fieldFormat)
      : base(description, condition)
    {
      this.fieldId = fieldId;
      this.prereqFields = prereqFields;
      this.fieldFormat = fieldFormat;
    }

    public string FieldID => this.fieldId;

    public string[] PrerequisiteFields => this.prereqFields;

    public FieldFormat CurrentFieldFormat => this.fieldFormat;

    protected override string GetRuleDefinition()
    {
      StringBuilder stringBuilder = new StringBuilder();
      string empty = string.Empty;
      if (this.prereqFields != null)
      {
        foreach (string prereqField in this.prereqFields)
        {
          string str1 = "if Value <> \"\"";
          if (this.fieldFormat == FieldFormat.DATE)
            str1 += " And Value <> \"//\"";
          else if (this.fieldFormat == FieldFormat.MONTHDAY)
            str1 += " And Value <> \"/\"";
          string str2 = str1 + " And [-" + prereqField + "] = \"\" then" + Environment.NewLine + "    throw new MissingPrerequisiteException(\"" + prereqField + "\")" + Environment.NewLine + "end if" + Environment.NewLine;
          stringBuilder.Append(str2);
        }
      }
      stringBuilder.Append(this.GetFieldRuleDefinition());
      return stringBuilder.ToString();
    }

    public bool TargetFieldIsNumericFormat()
    {
      return this.fieldFormat == FieldFormat.DECIMAL || this.fieldFormat == FieldFormat.DECIMAL_1 || this.fieldFormat == FieldFormat.DECIMAL_2 || this.fieldFormat == FieldFormat.DECIMAL_3 || this.fieldFormat == FieldFormat.DECIMAL_4 || this.fieldFormat == FieldFormat.DECIMAL_5 || this.fieldFormat == FieldFormat.DECIMAL_6 || this.fieldFormat == FieldFormat.DECIMAL_7 || this.fieldFormat == FieldFormat.DECIMAL_10 || this.fieldFormat == FieldFormat.INTEGER;
    }

    protected abstract string GetFieldRuleDefinition();
  }
}
