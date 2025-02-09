// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Configuration.LoanCompensation
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;
using System;

#nullable disable
namespace EllieMae.Encompass.Configuration
{
  public class LoanCompensation
  {
    internal LoanCompPlan _compPlan;

    internal LoanCompensation(LoanCompPlan compPlan) => this._compPlan = compPlan;

    public string PlanName => this._compPlan.Name;

    public Decimal BasePlanPercent => this._compPlan.PercentAmt;

    public Decimal BasePlanAmount => this._compPlan.DollarAmount;

    public Decimal MinAmount => this._compPlan.MinDollarAmount;

    public Decimal MaxAmount => this._compPlan.MaxDollarAmount;

    public LoanCompensation.PercentAmountBaseType PercentAmountBase
    {
      get
      {
        return this._compPlan.PercentAmtBase != 2 ? LoanCompensation.PercentAmountBaseType.TotalLoanAmount : LoanCompensation.PercentAmountBaseType.BaseLoanAmount;
      }
    }

    public LoanCompensation.RoundingMethodType RoundingMethod
    {
      get
      {
        return this._compPlan.RoundingMethod != 2 ? LoanCompensation.RoundingMethodType.ToNearestCent : LoanCompensation.RoundingMethodType.ToNearestDollar;
      }
    }

    public enum PercentAmountBaseType
    {
      TotalLoanAmount,
      BaseLoanAmount,
    }

    public enum RoundingMethodType
    {
      ToNearestCent,
      ToNearestDollar,
    }
  }
}
