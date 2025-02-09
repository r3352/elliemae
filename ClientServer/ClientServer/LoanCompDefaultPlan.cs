// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.LoanCompDefaultPlan
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class LoanCompDefaultPlan
  {
    private LoanCompPlanType planType;
    private string triggerField;
    private int minTermDays;
    private int roundingMethod;
    private int loansExempt;
    private LoanCompPaidBy paidBy;

    public LoanCompDefaultPlan()
    {
    }

    public LoanCompDefaultPlan(
      LoanCompPlanType planType,
      string triggerField,
      int minTermDays,
      int roundingMethod,
      int loansExempt,
      LoanCompPaidBy paidBy)
    {
      this.planType = planType;
      this.triggerField = triggerField;
      this.minTermDays = minTermDays;
      this.roundingMethod = roundingMethod;
      this.loansExempt = loansExempt;
      this.paidBy = paidBy;
    }

    public LoanCompPlanType PlanType
    {
      get => this.planType;
      set => this.planType = value;
    }

    public string TriggerField
    {
      get => this.triggerField;
      set => this.triggerField = value;
    }

    public int MinTermDays
    {
      get => this.minTermDays;
      set => this.minTermDays = value;
    }

    public int RoundingMethod
    {
      get => this.roundingMethod;
      set => this.roundingMethod = value;
    }

    public int LoansExempt
    {
      get => this.loansExempt;
      set => this.loansExempt = value;
    }

    public LoanCompPaidBy PaidBy
    {
      get => this.paidBy;
      set => this.paidBy = value;
    }

    public string PaidByString
    {
      get
      {
        if (this.paidBy == LoanCompPaidBy.Borrower)
          return "Borrower Paid";
        if (this.paidBy == LoanCompPaidBy.Lender)
          return "Lender Paid";
        return this.paidBy == LoanCompPaidBy.Exempt ? "Exempt" : string.Empty;
      }
    }
  }
}
