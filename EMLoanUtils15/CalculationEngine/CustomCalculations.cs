// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.CalculationEngine.CustomCalculations
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Customization;
using EllieMae.EMLite.DataEngine;

#nullable disable
namespace EllieMae.EMLite.CalculationEngine
{
  internal class CustomCalculations : CalculationBase
  {
    private CustomCalculatorInvoker[] customCalcInvokers;

    public CustomCalculations(
      SessionObjects sessionObjects,
      ILoanConfigurationInfo configInfo,
      LoanData l,
      EllieMae.EMLite.CalculationEngine.CalculationObjects calcObjs)
      : base(l, calcObjs)
    {
      this.customCalcInvokers = CustomCalculationCache.GetFieldCalculators(sessionObjects, configInfo).CreateInvokers(new CustomCalculationContext(sessionObjects.UserInfo, l, (IServerDataProvider) new CustomCodeSessionDataProvider(sessionObjects)));
    }

    public void CalculateAll()
    {
      foreach (CustomCalculatorInvoker customCalcInvoker in this.customCalcInvokers)
        customCalcInvoker.Invoke();
    }
  }
}
