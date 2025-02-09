// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.TemplateCondition
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>
  /// This represents the conditions for a <see cref="T:EllieMae.Encompass.BusinessEnums.MilestoneTemplate">MilestoneTemplate</see>.
  /// </summary>
  public class TemplateCondition
  {
    private TemplateConditionType conditionType;
    private string condition = string.Empty;
    private string advancedCondition = string.Empty;

    internal TemplateCondition(FieldRuleInfo rule)
    {
      switch (rule.Condition)
      {
        case BizRule.Condition.LoanPurpose:
          this.conditionType = TemplateConditionType.LoanPurpose;
          this.condition = Enum.Parse(typeof (BizRule.LoanPurpose), rule.ConditionState).ToString();
          break;
        case BizRule.Condition.LoanType:
          this.conditionType = TemplateConditionType.LoanType;
          this.condition = Enum.Parse(typeof (BizRule.LoanType), rule.ConditionState).ToString();
          break;
        case BizRule.Condition.AdvancedCoding:
          this.conditionType = TemplateConditionType.AdvancedCondition;
          this.advancedCondition = rule.ConditionState;
          break;
      }
      rule.Condition2.Split(new string[1]{ "," }, StringSplitOptions.RemoveEmptyEntries);
    }

    /// <summary>Gets the type of condition.</summary>
    public TemplateConditionType ConditionType => this.conditionType;

    /// <summary>
    /// Gets the value of the condition when <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.TemplateCondition.ConditionType">ConditionType</see> is LoanType or LoanPurpose.
    /// <remarks>This value is empty for <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.TemplateCondition.ConditionType">ConditionType</see> None and AdvancedCondition.</remarks>
    /// </summary>
    public string Condition => this.condition;

    /// <summary>
    /// Gets the value of the condition when <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.TemplateCondition.ConditionType" /> is AdvancedCondition.
    /// </summary>
    public string AdvancedCondition => this.advancedCondition;
  }
}
