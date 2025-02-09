// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.BusinessRules.CompiledTriggers
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Compiler;
using EllieMae.EMLite.Customization;
using System;
using System.Collections;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.BusinessRules
{
  public class CompiledTriggers : IEnumerable, IDisposable
  {
    private static string className = "TriggerKeepers";
    private static readonly string sw = Tracing.SwDataEngine;
    private RuntimeContext context;
    private ArrayList triggers;

    public CompiledTriggers(Trigger[] triggers)
      : this(triggers, true)
    {
    }

    public CompiledTriggers(Trigger[] triggers, bool loadInNewAppDomain)
    {
      using (Tracing.StartTimer(CompiledTriggers.sw, CompiledTriggers.className, TraceLevel.Info, "Building CompiledTriggers from " + (object) triggers.Length + " trigger definitions"))
      {
        RuleBuilder ruleBuilder = new RuleBuilder();
        Hashtable hashtable1 = new Hashtable();
        Hashtable hashtable2 = new Hashtable();
        Hashtable hashtable3 = new Hashtable();
        Tracing.Log(CompiledTriggers.sw, CompiledTriggers.className, TraceLevel.Info, "Generating source code for coded triggers and conditions...");
        foreach (Trigger trigger in triggers)
        {
          try
          {
            if (trigger is ICodedObject)
              hashtable1[(object) trigger.TriggerID] = (object) ruleBuilder.AddTrigger(trigger.TriggerID, trigger as ICodedObject);
            if (trigger.Condition is ICodedObject)
              hashtable3[(object) trigger.TriggerID] = (object) ruleBuilder.AddCondition(trigger.TriggerID, trigger.Condition as ICodedObject);
            hashtable2[(object) trigger.TriggerID] = (object) trigger;
          }
          catch (Exception ex)
          {
            Tracing.Log(CompiledTriggers.sw, CompiledTriggers.className, TraceLevel.Error, "Error building validation function for trigger '" + (object) trigger + "': " + (object) ex);
          }
        }
        if (hashtable2.Count == 0)
        {
          Tracing.Log(CompiledTriggers.sw, CompiledTriggers.className, TraceLevel.Info, "No valid triggers found. Aborting trigger compilation process.");
          this.triggers = new ArrayList();
        }
        else
        {
          if (hashtable1.Count + hashtable3.Count > 0)
            this.context = loadInNewAppDomain ? RuntimeContext.Create() : RuntimeContext.Current;
          try
          {
            if (this.context != null)
            {
              Tracing.Log(CompiledTriggers.sw, CompiledTriggers.className, TraceLevel.Info, "Compiling coded triggers...");
              ruleBuilder.Compile(this.context);
            }
            this.triggers = new ArrayList();
            Tracing.Log(CompiledTriggers.sw, CompiledTriggers.className, TraceLevel.Info, "Assembling trigger implementations and conditions...");
            foreach (DictionaryEntry dictionaryEntry in hashtable2)
            {
              Trigger def = (Trigger) dictionaryEntry.Value;
              ITriggerImpl triggerImpl = def as ITriggerImpl;
              IConditionImpl conditionImpl = def.Condition as IConditionImpl;
              if (triggerImpl == null)
                triggerImpl = (ITriggerImpl) this.context.CreateInstance((TypeIdentifier) hashtable1[dictionaryEntry.Key], typeof (ITriggerImpl));
              if (def.Condition != null && conditionImpl == null)
                conditionImpl = (IConditionImpl) this.context.CreateInstance((TypeIdentifier) hashtable3[dictionaryEntry.Key], typeof (IConditionImpl));
              this.triggers.Add((object) new CompiledTrigger(def, triggerImpl, conditionImpl, this.context));
            }
          }
          catch (Exception ex)
          {
            Tracing.Log(CompiledTriggers.sw, CompiledTriggers.className, TraceLevel.Error, "Error compiling field triggers: " + (object) ex);
            this.triggers = new ArrayList();
          }
        }
      }
    }

    public int Count => this.triggers.Count;

    public CompiledTrigger this[int index] => (CompiledTrigger) this.triggers[index];

    public IEnumerator GetEnumerator() => this.triggers.GetEnumerator();

    public void Dispose()
    {
      if (this.context == null)
        return;
      this.context.Dispose();
      this.context = (RuntimeContext) null;
    }
  }
}
