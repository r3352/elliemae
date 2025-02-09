// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.BusinessRules.LoanConditionGroupMonitor
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Customization;
using EllieMae.EMLite.DataEngine;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.BusinessRules
{
  public class LoanConditionGroupMonitor : IDisposable
  {
    private Dictionary<string, List<LoanConditionMonitor>> fieldMonitors = new Dictionary<string, List<LoanConditionMonitor>>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
    private ExecutionContext context;

    public event EventHandler ActiveStateChanged;

    public LoanConditionGroupMonitor(ExecutionContext context) => this.context = context;

    public void Clear()
    {
      foreach (string key in this.fieldMonitors.Keys)
      {
        foreach (LoanConditionMonitor conditionMonitor in this.fieldMonitors[key])
          conditionMonitor.Dispose();
        this.context.Loan.UnregisterCustomFieldValueChangeEventHandler(key, new Routine(this.onLoanFieldChanged));
      }
      this.fieldMonitors.Clear();
    }

    public void Add(ConditionEvaluator evaluator)
    {
      if (!(BizRuleTranslator.GetRuleCondition(evaluator.Rule) is IFieldComposition ruleCondition))
        throw new ArgumentException("Evaluator does not provide field-composition condition");
      string[] dependentFields = ruleCondition.GetDependentFields();
      LoanConditionMonitor conditionMonitor = new LoanConditionMonitor(evaluator, this.context);
      foreach (string str in dependentFields)
      {
        if (!this.fieldMonitors.ContainsKey(str))
        {
          this.fieldMonitors.Add(str, new List<LoanConditionMonitor>());
          this.context.Loan.RegisterCustomFieldValueChangeEventHandler(str, new Routine(this.onLoanFieldChanged));
        }
        this.fieldMonitors[str].Add(conditionMonitor);
      }
    }

    public void AddRange(IEnumerable<ConditionEvaluator> evaluators)
    {
      foreach (ConditionEvaluator evaluator in evaluators)
        this.Add(evaluator);
    }

    private void onLoanFieldChanged(string fieldId, string value)
    {
      bool flag1 = false;
      bool flag2 = false;
      if (this.fieldMonitors.ContainsKey(fieldId))
        flag2 = true;
      else if (fieldId.IndexOf("#") > -1)
      {
        fieldId = fieldId.Substring(0, fieldId.IndexOf("#"));
        flag2 = this.fieldMonitors.ContainsKey(fieldId);
      }
      if (!flag2)
        return;
      foreach (LoanConditionMonitor conditionMonitor in this.fieldMonitors[fieldId])
        flag1 |= conditionMonitor.Reevaluate();
      if (!flag1)
        return;
      this.OnActiveStateChanged();
    }

    protected void OnActiveStateChanged()
    {
      if (this.ActiveStateChanged == null)
        return;
      this.ActiveStateChanged((object) this, EventArgs.Empty);
    }

    public void Dispose()
    {
      try
      {
        this.Clear();
        this.context = (ExecutionContext) null;
      }
      catch
      {
      }
    }
  }
}
