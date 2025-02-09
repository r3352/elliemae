// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.BusinessRules.PrintFormSelector
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Compiler;
using System;

#nullable disable
namespace EllieMae.EMLite.BusinessRules
{
  [Serializable]
  public abstract class PrintFormSelector
  {
    private string selectorId;
    private string description;
    private string[] selectorFields;
    private RuleCondition condition;

    protected PrintFormSelector(
      string selectorId,
      string description,
      string[] selectorFields,
      RuleCondition condition)
    {
      this.selectorId = selectorId;
      this.description = description;
      this.selectorFields = selectorFields;
      this.condition = condition;
    }

    protected PrintFormSelector(
      string description,
      string[] selectorFields,
      RuleCondition condition)
      : this(Guid.NewGuid().ToString("N"), description, selectorFields, condition)
    {
    }

    public string SelectorId => this.selectorId;

    public string[] SelectorFields => this.selectorFields;

    public RuleCondition Condition => this.condition;

    public string Description => this.description;

    public abstract IFormSelectorImpl CreateImplementation(RuntimeContext context);
  }
}
