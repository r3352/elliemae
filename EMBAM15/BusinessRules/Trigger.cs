// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.BusinessRules.Trigger
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Compiler;
using System;

#nullable disable
namespace EllieMae.EMLite.BusinessRules
{
  [Serializable]
  public abstract class Trigger
  {
    private string triggerId;
    private string description;
    private string[] triggerFields;
    private RuleCondition condition;

    protected Trigger(
      string triggerId,
      string description,
      string[] triggerFields,
      RuleCondition condition)
    {
      this.triggerId = triggerId;
      this.description = description;
      this.triggerFields = triggerFields;
      this.condition = condition;
    }

    protected Trigger(string description, string[] triggerFields, RuleCondition condition)
      : this(Guid.NewGuid().ToString("N"), description, triggerFields, condition)
    {
    }

    public string TriggerID => this.triggerId;

    public string[] TriggerFields => this.triggerFields;

    public RuleCondition Condition => this.condition;

    public string Description => this.description;

    public bool ActivateOn(string fieldId)
    {
      for (int index = 0; index < this.triggerFields.Length; ++index)
      {
        if (string.Compare(fieldId, this.triggerFields[index], true) == 0)
          return true;
      }
      return false;
    }

    public abstract ITriggerImpl CreateImplementation(RuntimeContext context);
  }
}
