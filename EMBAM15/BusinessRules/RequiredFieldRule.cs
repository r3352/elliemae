// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.BusinessRules.RequiredFieldRule
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using System;

#nullable disable
namespace EllieMae.EMLite.BusinessRules
{
  public class RequiredFieldRule : FieldRule
  {
    public RequiredFieldRule(
      string description,
      string fieldId,
      RuleCondition condition,
      string[] prereqFields,
      FieldFormat fieldFormat)
      : base(description, fieldId, condition, prereqFields, fieldFormat)
    {
    }

    public RequiredFieldRule(
      string ruleId,
      string description,
      string fieldId,
      RuleCondition condition,
      string[] prereqFields,
      FieldFormat fieldFormat)
      : base(ruleId, description, fieldId, condition, prereqFields, fieldFormat)
    {
    }

    protected override string GetFieldRuleDefinition()
    {
      string str = "if Value = \"\"";
      if (this.CurrentFieldFormat == FieldFormat.DATE)
        str = "if Value = \"//\"";
      if (this.CurrentFieldFormat == FieldFormat.MONTHDAY)
        str = "if Value = \"/\"";
      return str + " then" + Environment.NewLine + "   Fail(\"Field '" + this.FieldID + "' is required.\")" + Environment.NewLine + "end if" + Environment.NewLine;
    }
  }
}
