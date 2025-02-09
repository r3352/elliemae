// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.BusinessRules.PredefinedTrigger
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Compiler;
using EllieMae.EMLite.Customization;
using System;

#nullable disable
namespace EllieMae.EMLite.BusinessRules
{
  [Serializable]
  public abstract class PredefinedTrigger : Trigger, ITriggerImpl
  {
    public PredefinedTrigger(string description, string[] triggerFields, RuleCondition condition)
      : base(description, triggerFields, condition)
    {
    }

    public PredefinedTrigger(
      string ruleId,
      string description,
      string[] triggerFields,
      RuleCondition condition)
      : base(ruleId, description, triggerFields, condition)
    {
    }

    public override ITriggerImpl CreateImplementation(RuntimeContext context)
    {
      return (ITriggerImpl) this;
    }

    public abstract bool Execute(
      IExecutionContext context,
      string AffectedFieldID,
      object PriorValue,
      object NewValue);
  }
}
