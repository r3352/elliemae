// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Configuration.LoanCompensation
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer;
using System;

#nullable disable
namespace EllieMae.Encompass.Configuration
{
  /// <summary>Provides detials of the Loan Compensation Plan.</summary>
  public class LoanCompensation
  {
    internal LoanCompPlan _compPlan;

    internal LoanCompensation(LoanCompPlan compPlan) => this._compPlan = compPlan;

    /// <summary>Gets the name of the assigned plan.</summary>
    public string PlanName => this._compPlan.Name;

    /// <summary>Gets the compensation percent.</summary>
    public Decimal BasePlanPercent => this._compPlan.PercentAmt;

    /// <summary>Gets the compensation amount.</summary>
    public Decimal BasePlanAmount => this._compPlan.DollarAmount;

    /// <summary>Gets the minimum compensation.</summary>
    public Decimal MinAmount => this._compPlan.MinDollarAmount;

    /// <summary>Gets the maximum compensation.</summary>
    public Decimal MaxAmount => this._compPlan.MaxDollarAmount;

    /// <summary>
    /// Gets the loan amount type for compensation (0: blank, 1: total loan amount, 2: base loan amount).
    /// If field value = 2, we will use base loan amount. Otherwiae, we will use total loan amount.
    /// </summary>
    public LoanCompensation.PercentAmountBaseType PercentAmountBase
    {
      get
      {
        return this._compPlan.PercentAmtBase != 2 ? LoanCompensation.PercentAmountBaseType.TotalLoanAmount : LoanCompensation.PercentAmountBaseType.BaseLoanAmount;
      }
    }

    /// <summary>
    /// Gets the rounding type for compensation (0: blank, 1: To Nearest Cent, 2: To Nearest Dollar).
    /// If field value = 2, we will round value to integer. Otherwiae, we will round it to 2 decimals.
    /// </summary>
    public LoanCompensation.RoundingMethodType RoundingMethod
    {
      get
      {
        return this._compPlan.RoundingMethod != 2 ? LoanCompensation.RoundingMethodType.ToNearestCent : LoanCompensation.RoundingMethodType.ToNearestDollar;
      }
    }

    /// <summary>Enum for Percent Amount Base Type</summary>
    public enum PercentAmountBaseType
    {
      /// <summary>TotalLoanAmount</summary>
      TotalLoanAmount,
      /// <summary>BaseLoanAmount</summary>
      BaseLoanAmount,
    }

    /// <summary>Enum form Rounding Method Type</summary>
    public enum RoundingMethodType
    {
      /// <summary>ToNearestCent</summary>
      ToNearestCent,
      /// <summary>ToNearestDollar</summary>
      ToNearestDollar,
    }
  }
}
