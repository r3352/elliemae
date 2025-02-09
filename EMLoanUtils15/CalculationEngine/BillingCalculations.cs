// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.CalculationEngine.BillingCalculations
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.Customization;
using EllieMae.EMLite.DataEngine;
using System;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.CalculationEngine
{
  internal class BillingCalculations : CalculationBase
  {
    private const string className = "BillingCalculations�";
    private static readonly string sw = Tracing.SwDataEngine;
    private SessionObjects sessionObjects;
    private CompiledCalculation[] closedLoanBillingCalculation;

    public BillingCalculations(
      SessionObjects sessionObjects,
      ILoanConfigurationInfo configInfo,
      LoanData l,
      EllieMae.EMLite.CalculationEngine.CalculationObjects calcObjs)
      : base(l, calcObjs)
    {
      this.sessionObjects = sessionObjects;
      if ((BillingModel) sessionObjects.StartupInfo.LicenseSettings[(object) "License.BillingModel"] != BillingModel.ClosedLoan)
        return;
      this.closedLoanBillingCalculation = BillingCalculationCache.GetBillingCalculator(sessionObjects, configInfo);
      if (this.closedLoanBillingCalculation == null)
        return;
      if (this.closedLoanBillingCalculation[0] != null)
      {
        foreach (string dependentField in this.closedLoanBillingCalculation[0].Calculation.DependentFields)
          l.RegisterCustomFieldValueChangeEventHandler(dependentField, new Routine(this.onClosedLoanBillingTriggerFieldChanged));
      }
      if (this.closedLoanBillingCalculation[1] == null)
        return;
      foreach (string dependentField in this.closedLoanBillingCalculation[1].Calculation.DependentFields)
        l.RegisterCustomFieldValueChangeEventHandler(dependentField, new Routine(this.onClosedLoanBillingCategoryFieldChanged));
    }

    private void onClosedLoanBillingTriggerFieldChanged(string fieldId, string value)
    {
      Tracing.Log(BillingCalculations.sw, nameof (BillingCalculations), TraceLevel.Verbose, "Evaluating Closed Loan Billing Date calculation");
      using (CustomCalculationContext context = new CustomCalculationContext(this.sessionObjects.UserInfo, this.loan, (IServerDataProvider) new CustomCodeSessionDataProvider(this.sessionObjects)))
      {
        string val = "";
        try
        {
          object obj = this.closedLoanBillingCalculation[0].Calculate((ICalculationContext) context);
          if (obj is DateTime)
            val = Convert.ToDateTime(obj).ToString("MM/dd/yyyy");
        }
        catch (Exception ex)
        {
          Tracing.Log(BillingCalculations.sw, nameof (BillingCalculations), TraceLevel.Verbose, "Closed Loan Billing Date calculation failed due to error: " + ex.Message + ". Field will be set to blank value.");
        }
        if (this.loan != null && this.loan.LinkedData != null && this.loan.LinkSyncType == LinkSyncType.ConstructionPrimary)
          this.SetVal("3260", "");
        else
          this.SetVal("3260", val);
        Tracing.Log(BillingCalculations.sw, nameof (BillingCalculations), TraceLevel.Verbose, "Closed Loan Billing Date set to '" + val + "'.");
      }
    }

    private void onClosedLoanBillingCategoryFieldChanged(string fieldId, string value)
    {
      Tracing.Log(BillingCalculations.sw, nameof (BillingCalculations), TraceLevel.Verbose, "Evaluating Closed Loan Billing Category calculation");
      using (CustomCalculationContext context = new CustomCalculationContext(this.sessionObjects.UserInfo, this.loan, (IServerDataProvider) new CustomCodeSessionDataProvider(this.sessionObjects)))
      {
        string val = "";
        try
        {
          object obj = this.closedLoanBillingCalculation[1].Calculate((ICalculationContext) context);
          if (obj is string)
            val = Convert.ToString(obj);
        }
        catch (Exception ex)
        {
          Tracing.Log(BillingCalculations.sw, nameof (BillingCalculations), TraceLevel.Verbose, "Closed Loan Billing Category calculation failed due to error: " + ex.Message + ". Field will be set to blank value.");
        }
        this.SetVal("BILLINGCATEGORY", val);
        Tracing.Log(BillingCalculations.sw, nameof (BillingCalculations), TraceLevel.Verbose, "Closed Loan Billing Category set to '" + val + "'.");
      }
    }

    public void CalculateAll()
    {
      if (this.closedLoanBillingCalculation == null)
        return;
      if (this.closedLoanBillingCalculation[0] != null)
        this.onClosedLoanBillingTriggerFieldChanged((string) null, (string) null);
      if (this.closedLoanBillingCalculation[1] == null)
        return;
      this.onClosedLoanBillingCategoryFieldChanged((string) null, (string) null);
    }
  }
}
