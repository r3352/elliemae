// Decompiled with JetBrains decompiler
// Type: Elli.Data.MongoDB.Mapping.DisclosureTracking2015LogMap
// Assembly: Elli.Data.MongoDB, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: F1D8D155-58C1-404A-A2A9-D942D1AE4E32
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Data.MongoDB.dll

using Elli.Domain.Mortgage;
using MongoDB.Bson.Serialization;
using System;
using System.Linq.Expressions;

#nullable disable
namespace Elli.Data.MongoDB.Mapping
{
  public class DisclosureTracking2015LogMap
  {
    public static void Register()
    {
      if (BsonClassMap.IsClassMapRegistered(typeof (DisclosureTracking2015Log)))
        return;
      BsonClassMap.RegisterClassMap<DisclosureTracking2015Log>((Action<BsonClassMap<DisclosureTracking2015Log>>) (cm =>
      {
        cm.AutoMap();
        cm.MapProperty<DateTime?>((Expression<Func<DisclosureTracking2015Log, DateTime?>>) (c => c.ApplicationDate));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.BorrowerName));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.CoBorrowerName));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.DisclosedApr));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.FinanceCharge));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.LoanProgram));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.LoanAmount));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.PropertyAddress));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.PropertyCity));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.PropertyState));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.PropertyZip));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.LenderTotalPaidOriginatorAmount));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.LoanPurchaseCreditType1));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.LoanPurchaseCreditAmount1));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.LoanPurchaseCreditType2));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.LoanPurchaseCreditAmount2));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.LoanPurchaseCreditType3));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.LoanPurchaseCreditAmount3));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.LoanPurchaseCreditType4));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.LoanPurchaseCreditAmount4));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.NonSpecificLenderCredit));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.NonSpecificSellerCredit));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.UseItemizedCredits));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.LoanClosingCostLenderCredits));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.LoanClosingCostTotalFeeAmount2015));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.LoanClosingCost3StdLegalLimit));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.LoanClosingCost2TotalLoanCost));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.LoanClosingCost2TotalLoanCost));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.LoanClosingCost2TotalOtherCost));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.LoanClosingCost2LenderCredits));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.LoanClosingCostGfe800BorPaidAmount));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.LoanFeesCityTaxBorPaidAmount));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.LoanFeesStateTaxBorPaidAmount));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.LoanClosingCostGfe1200BorPaidAmount));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.LoanClosingCostSection1000BorrowerTotalPaidAmount));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.LoanGfeAgregateAdjustment));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.LoanGfeGovermentRecordingCharges));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.LoanSection1000SellerPaidTotalAmount));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.LoanAdjustmentsOtherCredits));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.LoanDownPayment));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.LoanFundsForBorrower));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.LoanLineItemAmount));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.LoanRefinanceIncludingDebtsToBePaidOffAmount));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.LoanSellerCreditAmount));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.LoanTotalClosingCosts));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.LoanClosingCostsFinanced));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.PrepaymentPenaltyIndicator));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.LoanEstimateLoanProduct));
        cm.MapField("forms").SetElementName("Forms");
        cm.MapField("snapshotFields").SetElementName("SnapshotFields");
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.Line907PropertyIndicator2015));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.Line908PropertyIndicator2015));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.Line909PropertyIndicator2015));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.Line910PropertyIndicator2015));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.Line911PropertyIndicator2015));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.Line912PropertyIndicator2015));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.LoanClosingCost2BorrowerClosingCostAtClosing));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.LoanEstimate2TotalLoanCosts));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.LoanEstimate2TotalOtherCosts));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.LoanEstimate2TotalLoanAndOtherCosts));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.PurchasePriceAmount));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.LoanEstimate2UnroundedTotalLoanCosts));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.LoanEstimate2UnroundedTotalOtherCosts));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.Line907InsuranceIndicator2015));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.Line908InsuranceIndicator2015));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.Line909InsuranceIndicator2015));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.Line910InsuranceIndicator2015));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.Line911InsuranceIndicator2015));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.Line912InsuranceIndicator2015));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.Line907TaxesIndicator2015));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.Line908TaxesIndicator2015));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.Line909TaxesIndicator2015));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.Line910TaxesIndicator2015));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.Line911TaxesIndicator2015));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.Line912TaxesIndicator2015));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.AppliedCureAmount));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.LenderCompensationCreditAmount2));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.Line802LOCompAdditionalAmount1));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.Line802LOCompAdditionalAmount2));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.ChargesThatCannotDecreaseItemization9));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.ChargesThatCannotIncreaseItemization13));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.ChargesCannotIncrease10Itemization34));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.ChargesThatCannotDecreaseLE7));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.ChargesThatCannotIncreaseLE11));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.ChargesCannotIncrease10LE32));
        cm.MapProperty<string>((Expression<Func<DisclosureTracking2015Log, string>>) (c => c.EstimatedTotalPayoffsAndPaymentsAmount));
      }));
      DisclosureFormMap.Register();
      LogAlertMap.Register();
      LogCommentMap.Register();
      LogSnapshotFieldMap.Register();
    }
  }
}
