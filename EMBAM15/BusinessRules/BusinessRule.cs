// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.BusinessRules.BusinessRule
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Compiler;
using System;

#nullable disable
namespace EllieMae.EMLite.BusinessRules
{
  [Serializable]
  public abstract class BusinessRule
  {
    private string ruleId;
    private string description;
    private RuleCondition condition;

    protected BusinessRule(string ruleId, string description, RuleCondition condition)
    {
      this.ruleId = ruleId;
      this.description = description;
      this.condition = condition;
    }

    protected BusinessRule(string description, RuleCondition condition)
      : this(Guid.NewGuid().ToString("N"), description, condition)
    {
    }

    public string RuleID => this.ruleId;

    public string Description => this.description;

    public RuleCondition Condition => this.condition;

    public abstract IValidatorImpl CreateImplementation(RuntimeContext context);
  }
}
