// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.CalculationEngine.CustomFieldCalculators
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Compiler;
using EllieMae.EMLite.DataEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.CalculationEngine
{
  public class CustomFieldCalculators : IEnumerable, IDisposable
  {
    private static string className = nameof (CustomFieldCalculators);
    private static readonly string sw = Tracing.SwDataEngine;
    private RuntimeContext context;
    private CustomFieldCalculator[] calcs;

    public CustomFieldCalculators(CustomFieldsInfo customFields)
      : this(customFields, true)
    {
    }

    public CustomFieldCalculators(CustomFieldsInfo customFields, bool loadInNewAppDomain)
    {
      using (Tracing.StartTimer(CustomFieldCalculators.sw, CustomFieldCalculators.className, TraceLevel.Info, "Building CustomFieldCalculators from custom field definitions"))
      {
        CalculationBuilder calculationBuilder = new CalculationBuilder();
        Dictionary<string, TypeIdentifier> dictionary1 = new Dictionary<string, TypeIdentifier>();
        Dictionary<string, CustomCalculation> dictionary2 = new Dictionary<string, CustomCalculation>();
        int num = 0;
        Tracing.Log(CustomFieldCalculators.sw, CustomFieldCalculators.className, TraceLevel.Info, "Generating source code for calculated custom fields...");
        foreach (CustomFieldInfo customField in customFields)
        {
          try
          {
            if (customField.Calculation != null)
            {
              if (customField.Calculation.Trim() != "")
              {
                CustomCalculation calc = new CustomCalculation(customField.Calculation.Trim());
                dictionary2[customField.FieldID] = calc;
                dictionary1[customField.FieldID] = calculationBuilder.Add(calc);
                ++num;
              }
            }
          }
          catch (Exception ex)
          {
            Tracing.Log(CustomFieldCalculators.sw, CustomFieldCalculators.className, TraceLevel.Error, "Error building calculation for field '" + customField.FieldID + "': " + (object) ex);
          }
        }
        if (num == 0)
        {
          Tracing.Log(CustomFieldCalculators.sw, CustomFieldCalculators.className, TraceLevel.Info, "No custom calculations found to compile.");
          this.calcs = new CustomFieldCalculator[0];
        }
        else
        {
          this.context = loadInNewAppDomain ? RuntimeContext.Create() : RuntimeContext.Current;
          try
          {
            Tracing.Log(CustomFieldCalculators.sw, CustomFieldCalculators.className, TraceLevel.Info, "Compiling " + (object) num + " calculated custom fields...");
            calculationBuilder.Compile(this.context);
            Tracing.Log(CustomFieldCalculators.sw, CustomFieldCalculators.className, TraceLevel.Info, "Assembling compiled calculations...");
            List<CustomFieldCalculator> customFieldCalculatorList = new List<CustomFieldCalculator>();
            foreach (KeyValuePair<string, TypeIdentifier> keyValuePair in dictionary1)
            {
              CustomFieldCalculator customFieldCalculator = new CustomFieldCalculator(customFields.GetField(keyValuePair.Key), dictionary2[keyValuePair.Key], keyValuePair.Value, this.context);
              customFieldCalculatorList.Add(customFieldCalculator);
            }
            this.calcs = customFieldCalculatorList.ToArray();
            Tracing.Log(CustomFieldCalculators.sw, CustomFieldCalculators.className, TraceLevel.Info, "Successfully compiled " + (object) dictionary1.Count + " custom calculations.");
          }
          catch (Exception ex)
          {
            Tracing.Log(CustomFieldCalculators.sw, CustomFieldCalculators.className, TraceLevel.Error, "Error compiling custom calculations: " + (object) ex);
            this.calcs = new CustomFieldCalculator[0];
          }
        }
      }
    }

    public int Count => this.calcs.Length;

    public CustomFieldCalculator this[int index] => this.calcs[index];

    public IEnumerator GetEnumerator() => this.calcs.GetEnumerator();

    public CustomCalculatorInvoker[] CreateInvokers(CustomCalculationContext context)
    {
      List<CustomCalculatorInvoker> calculatorInvokerList = new List<CustomCalculatorInvoker>();
      foreach (CustomFieldCalculator calc in this.calcs)
        calculatorInvokerList.Add(new CustomCalculatorInvoker(calc, context));
      return calculatorInvokerList.ToArray();
    }

    public void InvokeAll(CustomCalculationContext context)
    {
      foreach (CustomCalculatorInvoker invoker in this.CreateInvokers(context))
        invoker.Invoke();
    }

    public void InvokeCalculations(CustomCalculationContext context, string[] fieldIds)
    {
      Dictionary<string, bool> dictionary = new Dictionary<string, bool>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
      foreach (string fieldId in fieldIds)
        dictionary[fieldId] = true;
      foreach (CustomCalculatorInvoker invoker in this.CreateInvokers(context))
      {
        if (dictionary.ContainsKey(invoker.Calculator.Field.FieldID))
          invoker.Invoke();
      }
    }

    public void Dispose()
    {
      if (this.context == null)
        return;
      this.context.Dispose();
      this.context = (RuntimeContext) null;
    }
  }
}
