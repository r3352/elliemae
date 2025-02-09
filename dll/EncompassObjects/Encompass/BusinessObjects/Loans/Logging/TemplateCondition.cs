// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.TemplateCondition
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  public class TemplateCondition
  {
    private TemplateConditionType conditionType;
    private string condition = string.Empty;
    private string advancedCondition = string.Empty;

    internal TemplateCondition(FieldRuleInfo rule)
    {
      BizRule.Condition condition = ((BizRuleInfo) rule).Condition;
      if (condition != 1)
      {
        if (condition != 2)
        {
          if (condition == 9)
          {
            this.conditionType = TemplateConditionType.AdvancedCondition;
            this.advancedCondition = ((BizRuleInfo) rule).ConditionState;
          }
        }
        else
        {
          this.conditionType = TemplateConditionType.LoanType;
          this.condition = Enum.Parse(typeof (BizRule.LoanType), ((BizRuleInfo) rule).ConditionState).ToString();
        }
      }
      else
      {
        this.conditionType = TemplateConditionType.LoanPurpose;
        this.condition = Enum.Parse(typeof (BizRule.LoanPurpose), ((BizRuleInfo) rule).ConditionState).ToString();
      }
      ((BizRuleInfo) rule).Condition2.Split(new string[1]
      {
        ","
      }, StringSplitOptions.RemoveEmptyEntries);
    }

    public TemplateConditionType ConditionType => this.conditionType;

    public string Condition => this.condition;

    public string AdvancedCondition => this.advancedCondition;
  }
}
