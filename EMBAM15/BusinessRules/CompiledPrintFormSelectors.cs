// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.BusinessRules.CompiledPrintFormSelectors
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
  public class CompiledPrintFormSelectors : IEnumerable, IDisposable
  {
    private static string className = "CompiledPrintFormSelector";
    private static readonly string sw = Tracing.SwDataEngine;
    private RuntimeContext context;
    private ArrayList formSelectors;

    public CompiledPrintFormSelectors(PrintFormSelector[] formSelectors)
    {
      RuleBuilder ruleBuilder = new RuleBuilder();
      Hashtable hashtable1 = new Hashtable();
      Hashtable hashtable2 = new Hashtable();
      Hashtable hashtable3 = new Hashtable();
      foreach (PrintFormSelector formSelector in formSelectors)
      {
        try
        {
          if (formSelector is ICodedObject)
            hashtable1[(object) formSelector.SelectorId] = (object) ruleBuilder.AddPrintFormSelector(formSelector.SelectorId, formSelector as ICodedObject);
          if (formSelector.Condition is ICodedObject)
            hashtable3[(object) formSelector.SelectorId] = (object) ruleBuilder.AddCondition(formSelector.SelectorId, formSelector.Condition as ICodedObject);
          hashtable2[(object) formSelector.SelectorId] = (object) formSelector;
        }
        catch (Exception ex)
        {
          Tracing.Log(CompiledPrintFormSelectors.sw, CompiledPrintFormSelectors.className, TraceLevel.Error, "CompiledPrintFormSelectors: Error building validation function for form selector '" + (object) formSelector + "': " + (object) ex);
        }
      }
      if (hashtable2.Count == 0)
      {
        this.formSelectors = new ArrayList();
      }
      else
      {
        if (hashtable1.Count + hashtable3.Count > 0)
          this.context = RuntimeContext.Create();
        try
        {
          if (this.context != null)
            ruleBuilder.Compile(this.context);
          this.formSelectors = new ArrayList();
          foreach (DictionaryEntry dictionaryEntry in hashtable2)
          {
            PrintFormSelector def = (PrintFormSelector) dictionaryEntry.Value;
            IFormSelectorImpl printFormImpl = def as IFormSelectorImpl;
            IConditionImpl conditionImpl = def.Condition as IConditionImpl;
            if (printFormImpl == null)
              printFormImpl = (IFormSelectorImpl) this.context.CreateInstance((TypeIdentifier) hashtable1[dictionaryEntry.Key], typeof (IFormSelectorImpl));
            if (def.Condition != null && conditionImpl == null)
              conditionImpl = (IConditionImpl) this.context.CreateInstance((TypeIdentifier) hashtable3[dictionaryEntry.Key], typeof (IConditionImpl));
            this.formSelectors.Add((object) new CompiledPrintFormSelector(def, printFormImpl, conditionImpl, this.context));
          }
        }
        catch (Exception ex)
        {
          Tracing.Log(CompiledPrintFormSelectors.sw, CompiledPrintFormSelectors.className, TraceLevel.Error, "CompiledPrintFormSelectors: Error compiling field formSelectors: " + (object) ex);
          this.formSelectors = new ArrayList();
        }
      }
    }

    public int Count => this.formSelectors.Count;

    public CompiledPrintFormSelector this[int index]
    {
      get => (CompiledPrintFormSelector) this.formSelectors[index];
    }

    public IEnumerator GetEnumerator() => this.formSelectors.GetEnumerator();

    public void Dispose()
    {
      if (this.context == null)
        return;
      this.context.Dispose();
      this.context = (RuntimeContext) null;
    }
  }
}
