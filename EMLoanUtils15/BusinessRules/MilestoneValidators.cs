// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.BusinessRules.MilestoneValidators
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.Customization;

#nullable disable
namespace EllieMae.EMLite.BusinessRules
{
  public class MilestoneValidators(MilestoneRule[] rules) : RuleValidators((BusinessRule[]) rules)
  {
    public void ValidateMilestone(string milestoneId, ExecutionContext context)
    {
      foreach (RuleValidator ruleValidator in (RuleValidators) this)
      {
        if (((MilestoneRule) ruleValidator.Rule).AdvancedCodeMilestonePair.MilestoneID == milestoneId)
          ruleValidator.Validate(context, (object) true);
      }
    }
  }
}
