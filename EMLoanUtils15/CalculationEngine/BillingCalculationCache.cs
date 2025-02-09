// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.CalculationEngine.BillingCalculationCache
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Services;
using EllieMae.EMLite.Compiler;
using System;
using System.Collections;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.CalculationEngine
{
  public static class BillingCalculationCache
  {
    private const string className = "BillingCalculationCache�";
    private static readonly string sw = Tracing.SwDataEngine;
    private static Hashtable calcs = new Hashtable();
    private static object syncRoot = new object();

    public static CompiledCalculation[] GetBillingCalculator(
      SessionObjects sessionObjects,
      ILoanConfigurationInfo configInfo)
    {
      string clientId = sessionObjects.CompanyInfo.ClientID;
      BillingCalculationCache.ClientBillingCalculations billingCalculations = (BillingCalculationCache.ClientBillingCalculations) null;
      lock (BillingCalculationCache.syncRoot)
      {
        if (!BillingCalculationCache.calcs.Contains((object) clientId))
          BillingCalculationCache.calcs[(object) clientId] = (object) new BillingCalculationCache.ClientBillingCalculations();
        billingCalculations = (BillingCalculationCache.ClientBillingCalculations) BillingCalculationCache.calcs[(object) clientId];
      }
      return billingCalculations.GetBillingCalculator(sessionObjects, configInfo);
    }

    private class ClientBillingCalculations
    {
      private const string className = "ClientBillingCalculations�";
      private static readonly string sw = Tracing.SwDataEngine;
      private CompiledCalculation[] billingCalc;
      private object syncRoot = new object();

      public CompiledCalculation[] GetBillingCalculator(
        SessionObjects sessionObjects,
        ILoanConfigurationInfo configInfo)
      {
        lock (this.syncRoot)
        {
          if (this.billingCalc == null)
            this.billingCalc = this.getBillingCalculations(sessionObjects.CompanyInfo.ClientID, sessionObjects?.StartupInfo?.ServiceUrls?.JedServicesUrl);
          return this.billingCalc;
        }
      }

      private CompiledCalculation[] getBillingCalculations(string clientId, string jedServicesUrl)
      {
        using (Tracing.StartTimer(BillingCalculationCache.ClientBillingCalculations.sw, nameof (ClientBillingCalculations), TraceLevel.Info, "Creating Closed Loan Billing Custom Date Calculation"))
        {
          ClosedLoanBillingInfo closedLoanBillingInfo = (ClosedLoanBillingInfo) null;
          CompiledCalculation[] billingCalculations = new CompiledCalculation[2]
          {
            BillingCalculationCache.ClientBillingCalculations.DefaultClosingDateCalculation.CreateInstance(),
            BillingCalculationCache.ClientBillingCalculations.DefaultBillingCategoryCalculation.CreateInstance()
          };
          try
          {
            Tracing.Log(BillingCalculationCache.ClientBillingCalculations.sw, nameof (ClientBillingCalculations), TraceLevel.Info, "Retrieving Closed Loan Billing Trigger data from license server");
            using (LicenseService licenseService = new LicenseService(jedServicesUrl))
              closedLoanBillingInfo = licenseService.GetClosedLoanBillingInfo(clientId);
            if (closedLoanBillingInfo != null)
            {
              if ((closedLoanBillingInfo.ClosingDateCalculation ?? "") == "")
              {
                if (!((closedLoanBillingInfo.BillingCategoryCalculation ?? "") == ""))
                  goto label_12;
              }
              else
                goto label_12;
            }
            Tracing.Log(BillingCalculationCache.ClientBillingCalculations.sw, nameof (ClientBillingCalculations), TraceLevel.Info, "No custom Closed Loan Billing Trigger data and billing category data for this client. Using default calculation.");
            return billingCalculations;
          }
          catch (Exception ex)
          {
            Tracing.Log(BillingCalculationCache.ClientBillingCalculations.sw, nameof (ClientBillingCalculations), TraceLevel.Error, "Failed to retrieve Closed Loan Billing Trigger data: " + (object) ex);
            return (CompiledCalculation[]) null;
          }
label_12:
          try
          {
            if ((closedLoanBillingInfo.ClosingDateCalculation ?? "") != "")
            {
              Tracing.Log(BillingCalculationCache.ClientBillingCalculations.sw, nameof (ClientBillingCalculations), TraceLevel.Info, "Preparing to compile Closed Loan Billing Date Trigger calculation: " + closedLoanBillingInfo.ClosingDateCalculation);
              CustomCalculation calc = new CustomCalculation(closedLoanBillingInfo.ClosingDateCalculation);
              ICustomCalculationImpl implementation = new CalculationBuilder().CreateImplementation(calc, RuntimeContext.Current);
              billingCalculations[0] = new CompiledCalculation(calc, implementation);
              Tracing.Log(BillingCalculationCache.ClientBillingCalculations.sw, nameof (ClientBillingCalculations), TraceLevel.Info, "Successfully compiled Closed Loan Billing Date Trigger calculation");
            }
            if ((closedLoanBillingInfo.BillingCategoryCalculation ?? "") != "")
            {
              Tracing.Log(BillingCalculationCache.ClientBillingCalculations.sw, nameof (ClientBillingCalculations), TraceLevel.Info, "Preparing to compile Closed Loan Billing Category Trigger calculation: " + closedLoanBillingInfo.BillingCategoryCalculation);
              CustomCalculation calc = new CustomCalculation(closedLoanBillingInfo.BillingCategoryCalculation);
              ICustomCalculationImpl implementation = new CalculationBuilder().CreateImplementation(calc, RuntimeContext.Current);
              billingCalculations[1] = new CompiledCalculation(calc, implementation);
              Tracing.Log(BillingCalculationCache.ClientBillingCalculations.sw, nameof (ClientBillingCalculations), TraceLevel.Info, "Successfully compiled Closed Loan Billing Category Trigger calculation");
            }
            return billingCalculations;
          }
          catch (Exception ex)
          {
            Tracing.Log(BillingCalculationCache.ClientBillingCalculations.sw, nameof (ClientBillingCalculations), TraceLevel.Error, "Failed to compile Closed Loan Billing Trigger calculation: " + (object) ex);
            return (CompiledCalculation[]) null;
          }
        }
      }

      private class DefaultClosingDateCalculation : ICustomCalculationImpl
      {
        public const string Expression = "[748]�";

        public object Calculate(ICalculationContext context) => context.Fields["748"];

        public static CompiledCalculation CreateInstance()
        {
          return new CompiledCalculation("[748]", (ICustomCalculationImpl) new BillingCalculationCache.ClientBillingCalculations.DefaultClosingDateCalculation());
        }
      }

      private class DefaultBillingCategoryCalculation : ICustomCalculationImpl
      {
        public object Calculate(ICalculationContext context) => (object) string.Empty;

        public static CompiledCalculation CreateInstance()
        {
          return new CompiledCalculation(string.Empty, (ICustomCalculationImpl) new BillingCalculationCache.ClientBillingCalculations.DefaultBillingCategoryCalculation());
        }
      }
    }
  }
}
