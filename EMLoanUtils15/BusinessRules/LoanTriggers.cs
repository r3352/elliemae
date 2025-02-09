// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.BusinessRules.LoanTriggers
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Classes;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.Customization;
using EllieMae.EMLite.DataEngine;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.BusinessRules
{
  public class LoanTriggers : ILoanTriggers
  {
    private ExecutionContext context;
    private CompiledTriggers triggers;
    private List<TriggerInvoker> invokers = new List<TriggerInvoker>();
    private List<DelayedTrigger> trackers = new List<DelayedTrigger>();
    private bool enabled = true;

    public LoanTriggers(
      SessionObjects sessionObjects,
      ILoanConfigurationInfo configInfo,
      LoanData loanData)
    {
      if (loanData.Triggers != null)
        throw new InvalidOperationException("LoanData object already has a validator attached");
      PerformanceMeter current = PerformanceMeter.Current;
      this.context = new ExecutionContext(sessionObjects.UserInfo, loanData, (IServerDataProvider) new CustomCodeSessionDataProvider(sessionObjects), false);
      current.AddCheckpoint("Before TriggerCache.GetTriggers(sessionObjects, configInfo)", 36, ".ctor", "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\Triggers\\LoanTriggers.cs");
      sessionObjects.TriggersConfigInfo = new ConfigInfoForTriggers(configInfo.Triggers, configInfo.MilestonesList, configInfo.TriggersModificationTime);
      this.triggers = TriggerCache.GetTriggers(sessionObjects);
      current.AddCheckpoint("After TriggerCache.GetTriggers(sessionObjects, configInfo)", 40, ".ctor", "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\Triggers\\LoanTriggers.cs");
      foreach (CompiledTrigger trigger in this.triggers)
      {
        TriggerImplDef definition = (TriggerImplDef) trigger.Definition;
        if (definition.Event.Action.ActivationEvent == TriggerActivationEvent.FieldChanged)
          this.invokers.Add(new TriggerInvoker(trigger, this.context));
        else if (definition.Event.Action.ActivationEvent == TriggerActivationEvent.LoanSaved)
          this.trackers.Add((DelayedTrigger) new DelayedCompiledTrigger(trigger, this.context));
      }
      current.AddCheckpoint("Create the invokers for the triggers", 51, ".ctor", "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\Triggers\\LoanTriggers.cs");
      if ((BillingModel) sessionObjects.StartupInfo.LicenseSettings[(object) "License.BillingModel"] == BillingModel.ClosedLoan)
        this.trackers.Add((DelayedTrigger) new ClosedLoanBillingTrigger(sessionObjects, configInfo, this.context));
      else if (sessionObjects.GetCachedUserLicense().LicensedModules.Contains(11))
        this.trackers.Add((DelayedTrigger) new ClosedLoanBillingTrigger(sessionObjects, configInfo, this.context));
      current.AddCheckpoint("Add the Closed Loan Billing trigger", 64, ".ctor", "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\Triggers\\LoanTriggers.cs");
      loanData.AttachTriggers((ILoanTriggers) this);
      current.AddCheckpoint("loanData.AttachTriggers", 66, ".ctor", "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\Triggers\\LoanTriggers.cs");
    }

    public bool Enabled
    {
      get => this.enabled;
      set
      {
        this.enabled = value;
        foreach (TriggerInvoker invoker in this.invokers)
          invoker.Enabled = value;
      }
    }

    public DelayedTrigger[] GetDelayActivatedTriggers() => this.trackers.ToArray();

    public TriggerInvoker[] GetCompliedActivatedTriggers() => this.invokers.ToArray();

    public void Execute(string fieldId)
    {
      if (!this.enabled)
        return;
      foreach (TriggerInvoker invoker in this.invokers)
      {
        if (invoker.Trigger.Definition.ActivateOn(fieldId))
        {
          IExecutionContext context = (IExecutionContext) this.context;
          int num = (int) invoker.Trigger.Execute(this.context, fieldId, context.Fields[fieldId], context.Fields[fieldId]);
        }
      }
    }

    public void ResubscribeToFieldEvents()
    {
      foreach (TriggerInvoker invoker in this.invokers)
        invoker.ResubscribeToFieldEvents();
      foreach (DelayedTrigger tracker in this.trackers)
        tracker.ResubscribeToFieldEvents();
    }
  }
}
