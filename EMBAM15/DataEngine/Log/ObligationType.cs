// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.ObligationType
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using System.ComponentModel;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public enum ObligationType
  {
    [Description("None")] None,
    [Description("Installment Loans")] InstallmentLoan,
    [Description("Real Estate Loans")] RealEstateLoan,
    [Description("Alimony Maintenance")] AlimonyOrMaintenance,
    [Description("Monthly Housing Expense [P & I]")] MonthlyHousingExpense,
    [Description("Revolving Charge Accounts")] RevolvingChargeAccount,
    [Description("Simultaneous Loans On Property")] SimultaneousLoansOnProperty,
    [Description("Child Support")] ChildSupport,
    [Description("Required Escrows (i.e. Taxes, Ins, MI, HOA, Dues)")] RequiredEscrow,
    [Description("Other")] OtherMonthlyObligation,
    [Description("No. And Age Of Credit Lines")] NoAndAgeOfCreditLine,
    [Description("Judgements")] Judgement,
    [Description("Backruptcies")] Backruptcy,
    [Description("Mortgage Lates")] MortgageLate,
    [Description("HELOC")] HELOC,
    [Description("Payment History")] PaymentHistory,
    [Description("Collections")] Collection,
    [Description("Rental Payment History")] RentalPaymentHistory,
    [Description("Debt Obligations Current")] DebtObligationCurrent,
    [Description("2nd Lien")] SecondLien,
    [Description("Other")] OtherCreditHistory,
  }
}
