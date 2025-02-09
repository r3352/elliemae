// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Helper
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.DataEngine.Log;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public static class Helper
  {
    public static LoanBorrowerType GetLoanBorrowerType(string value)
    {
      switch (value.ToLowerInvariant())
      {
        case "borrower":
          return LoanBorrowerType.Borrower;
        case "coborrower":
          return LoanBorrowerType.Coborrower;
        case "both":
          return LoanBorrowerType.Both;
        default:
          return LoanBorrowerType.None;
      }
    }

    public static VerificationTimelineType GetVerificationTimelineType(string value)
    {
      switch (value.ToLowerInvariant())
      {
        case "asset":
          return VerificationTimelineType.Asset;
        case "employment":
          return VerificationTimelineType.Employment;
        case "income":
          return VerificationTimelineType.Income;
        case "obligation":
          return VerificationTimelineType.Obligation;
        default:
          return VerificationTimelineType.None;
      }
    }

    public static AssetType GetAssetType(string value)
    {
      switch (value.ToLowerInvariant())
      {
        case "bankstatement":
          return AssetType.BankStatement;
        case "mutualfund":
          return AssetType.MutualFund;
        case "rentalpropertyincome":
          return AssetType.RentalPropertyIncome;
        case "other":
          return AssetType.Other;
        default:
          return AssetType.None;
      }
    }

    public static EmploymentType GetEmploymentType(string value)
    {
      switch (value.ToLowerInvariant())
      {
        case "fulltime":
          return EmploymentType.FullTime;
        case "irregular":
          return EmploymentType.Irregular;
        case "military":
          return EmploymentType.Military;
        case "parttime":
          return EmploymentType.PartTime;
        case "retired":
          return EmploymentType.Retired;
        case "seasonal":
          return EmploymentType.Seasonal;
        case "selfemployed":
          return EmploymentType.SelfEmployed;
        default:
          return EmploymentType.None;
      }
    }

    public static IncomeType GetIncomeType(string value)
    {
      switch (value.ToLowerInvariant())
      {
        case "alimonyormaintenance":
          return IncomeType.AlimonyOrMaintenance;
        case "childsupport":
          return IncomeType.ChildSupport;
        case "four01k":
          return IncomeType.Four01K;
        case "military":
          return IncomeType.Military;
        case "otheremployment":
          return IncomeType.OtherEmployment;
        case "othernonemployment":
          return IncomeType.OtherNonEmployment;
        case "paystub":
          return IncomeType.Paystub;
        case "pension":
          return IncomeType.Pension;
        case "rentalincome":
          return IncomeType.RentalIncome;
        case "socialsecurity":
          return IncomeType.SocialSecurity;
        case "taxreturn":
          return IncomeType.TaxReturn;
        case "ten99":
          return IncomeType.Ten99;
        case "w2":
          return IncomeType.W2;
        default:
          return IncomeType.None;
      }
    }

    public static ObligationType GetObligationType(string value)
    {
      switch (value.ToLowerInvariant())
      {
        case "alimonyormaintenance":
          return ObligationType.AlimonyOrMaintenance;
        case "backruptcy":
          return ObligationType.Backruptcy;
        case "childsupport":
          return ObligationType.ChildSupport;
        case "collection":
          return ObligationType.Collection;
        case "debtobligationcurrent":
          return ObligationType.DebtObligationCurrent;
        case "heloc":
          return ObligationType.HELOC;
        case "installmentLoan":
          return ObligationType.InstallmentLoan;
        case "judgement":
          return ObligationType.Judgement;
        case "monthlyhousingexpense":
          return ObligationType.MonthlyHousingExpense;
        case "mortgagelate":
          return ObligationType.MortgageLate;
        case "noAndageofcreditline":
          return ObligationType.NoAndAgeOfCreditLine;
        case "othercredithistory":
          return ObligationType.OtherCreditHistory;
        case "othermonthlyobligation":
          return ObligationType.OtherMonthlyObligation;
        case "paymenthistory":
          return ObligationType.PaymentHistory;
        case "realestateloan":
          return ObligationType.RealEstateLoan;
        case "rentalpaymenthistory":
          return ObligationType.RentalPaymentHistory;
        case "requiredescrow":
          return ObligationType.RequiredEscrow;
        case "revolvingchargeaccount":
          return ObligationType.RevolvingChargeAccount;
        case "secondlien":
          return ObligationType.SecondLien;
        case "simultaneousloansonproperty":
          return ObligationType.SimultaneousLoansOnProperty;
        default:
          return ObligationType.None;
      }
    }
  }
}
