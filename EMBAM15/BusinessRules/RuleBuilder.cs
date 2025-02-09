// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.BusinessRules.RuleBuilder
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Compiler;
using EllieMae.EMLite.Customization;
using System;
using System.IO;
using System.Reflection;

#nullable disable
namespace EllieMae.EMLite.BusinessRules
{
  public class RuleBuilder
  {
    private CodeWriter assemblyCode;
    private string assemblyId;

    public RuleBuilder() => this.Clear();

    public void Compile(RuntimeContext context)
    {
      context.CompileAssembly(this.assemblyId, this.assemblyCode.ToString(), CodeLanguage.VB, new string[1]
      {
        Path.GetFileName(Assembly.GetExecutingAssembly().CodeBase)
      });
    }

    public IValidatorImpl CreateValidatorImpl(CodedBusinessRule rule, RuntimeContext context)
    {
      this.Clear();
      TypeIdentifier typeId = this.AddRule(rule.RuleID, (ICodedObject) rule);
      this.Compile(context);
      return (IValidatorImpl) context.CreateInstance(typeId, typeof (IValidatorImpl));
    }

    public ITriggerImpl CreateTriggerImpl(CodedTrigger trigger, RuntimeContext context)
    {
      this.Clear();
      TypeIdentifier typeId = this.AddTrigger(trigger.TriggerID, (ICodedObject) trigger);
      this.Compile(context);
      return (ITriggerImpl) context.CreateInstance(typeId, typeof (ITriggerImpl));
    }

    public IFormSelectorImpl CreatePrintFormSelectorImpl(
      CodedFormSelector trigger,
      RuntimeContext context)
    {
      this.Clear();
      TypeIdentifier typeId = this.AddTrigger(trigger.SelectorId, (ICodedObject) trigger);
      this.Compile(context);
      return (IFormSelectorImpl) context.CreateInstance(typeId, typeof (IFormSelectorImpl));
    }

    public IConditionImpl CreateConditionImpl(CodedCondition condition, RuntimeContext context)
    {
      this.Clear();
      TypeIdentifier typeId = this.AddCondition((ICodedObject) condition);
      this.Compile(context);
      return (IConditionImpl) context.CreateInstance(typeId, typeof (IConditionImpl));
    }

    public void Clear()
    {
      this.assemblyId = Guid.NewGuid().ToString("N");
      this.assemblyCode = new CodeWriter(CodeLanguage.VB);
      this.assemblyCode.WriteLine("Imports System");
      this.assemblyCode.WriteLine("Imports Microsoft.VisualBasic");
      this.assemblyCode.WriteLine("Imports EllieMae.EMLite.BusinessRules");
    }

    public TypeIdentifier AddRule(string objectId, ICodedObject rule)
    {
      string className = "Rule_" + objectId;
      this.assemblyCode.StartBlock("Public Class " + className);
      this.assemblyCode.WriteLine("Inherits ValidatorImplBase");
      this.assemblyCode.StartBlock("Protected Overrides Sub ExecuteRule(Value as Object)");
      this.assemblyCode.StartRegion(objectId, 0);
      this.assemblyCode.WriteLine(rule.ToSourceCode());
      this.assemblyCode.EndRegion(objectId);
      this.assemblyCode.EndBlock("End Sub");
      this.assemblyCode.EndBlock("End Class");
      return new TypeIdentifier(this.assemblyId, className);
    }

    public TypeIdentifier AddTrigger(string objectId, ICodedObject rule)
    {
      string className = "Trigger_" + objectId;
      this.assemblyCode.StartBlock("Public Class " + className);
      this.assemblyCode.WriteLine("Inherits TriggerImplBase");
      this.assemblyCode.StartBlock("Protected Overrides Function ExecuteTrigger(FieldID as String, PriorValue as Object, NewValue as Object) as Boolean");
      this.assemblyCode.StartRegion(objectId, 0);
      this.assemblyCode.WriteLine(rule.ToSourceCode());
      this.assemblyCode.EndRegion(objectId);
      this.assemblyCode.EndBlock("End Function");
      this.assemblyCode.EndBlock("End Class");
      return new TypeIdentifier(this.assemblyId, className);
    }

    public TypeIdentifier AddPrintFormSelector(string objectId, ICodedObject rule)
    {
      string className = "PrintFormSelector_" + objectId;
      this.assemblyCode.StartBlock("Public Class " + className);
      this.assemblyCode.WriteLine("Inherits PrintFormSelectorImplBase");
      this.assemblyCode.StartBlock("Protected Overrides Function ExecutePrintFormSelector(FieldID as String, NewValue as Object) As Boolean");
      this.assemblyCode.StartRegion(objectId, 0);
      this.assemblyCode.WriteLine(rule.ToSourceCode());
      this.assemblyCode.EndRegion(objectId);
      this.assemblyCode.EndBlock("End Function");
      this.assemblyCode.EndBlock("End Class");
      return new TypeIdentifier(this.assemblyId, className);
    }

    public TypeIdentifier AddCondition(ICodedObject condition)
    {
      return this.AddCondition(Guid.NewGuid().ToString("N"), condition);
    }

    public TypeIdentifier AddCondition(string objectId, ICodedObject condition)
    {
      string className = "Condition_" + objectId;
      this.assemblyCode.StartBlock("Public Class " + className);
      this.assemblyCode.WriteLine("Inherits ConditionImplBase");
      this.assemblyCode.StartBlock("Protected Overrides Function Applies() as Boolean");
      this.assemblyCode.StartRegion(objectId, 7);
      this.assemblyCode.WriteLine("Return " + condition.ToSourceCode());
      this.assemblyCode.EndRegion(objectId);
      this.assemblyCode.EndBlock("End Function");
      this.assemblyCode.EndBlock("End Class");
      return new TypeIdentifier(this.assemblyId, className);
    }
  }
}
