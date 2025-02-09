// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.CalculationEngine.NewHUD2015Calculation
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.LoanUtils.CalculationServants;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.CalculationEngine
{
  internal class NewHUD2015Calculation : CalculationBase
  {
    private const string className = "NewHUD2015Calculation�";
    private static readonly string sw = Tracing.SwDataEngine;
    private SessionObjects sessionObjects;
    private ILoanConfigurationInfo configInfo;
    internal Routine CalcLoanTermTable_LoanAmount;
    internal Routine CalcLoanTermTable_InterestRate;
    internal Routine CalcLoanTermTable_MonthlyPrincipalAndInterest;
    internal Routine CalcLoanTermTable_Prepayment;
    internal Routine PopulateLoanEstimate_LoanPurpose;
    internal Routine CalcLoanTermTable_LoanTerm;
    internal Routine CalcClosingDisclosurePage5;
    internal Routine CalcLoanEstimate_ClosingCostExpDate;
    internal Routine PopulateLoanEstimate_LenderBrokerInfo;
    internal Routine CalcLoanEstimate_AIRMonthSuffix;
    internal Routine AggrEscrowAcc_EscrowedCosts;
    internal Routine CalcClosingCostSubTotal;
    internal Routine CalcLoanEstimateTotalLoanCosts;
    internal Routine CalcLoanEstimateTotalOtherCosts;
    internal Routine CalcLoanEstimateTotalClosingCosts;
    internal Routine CalcClosingDisclosureTotalLoanCosts;
    internal Routine CalcClosingDisclosureTotalOtherCosts;
    internal Routine CalcClosingDisclosureTotalClosingCosts;
    internal Routine CalcLoanEstimateDownPaymentAndFundsForBorrower;
    internal Routine CalculateProductDescription;
    internal Routine CalculateLoanEstimate_ComparisonsTable;
    internal Routine CalcClosingDisclosureTotalPayments;
    internal Routine CalcAPTable_InterestOnlyPayments;
    internal Routine CalcAPTable_FirstChangePayment;
    internal Routine CalcAPTable_SubsequentChanges;
    internal Routine CalcAPTable_MaximumPayment;
    internal Routine SetDisclosureTrackingLogData;
    internal Routine SetAIRTableIndexType;
    internal Routine CalcLoanEstimateEstimatedCashToCloseSV;
    internal Routine CalcLoanEstimateEstimatedCashToCloseAV;
    internal Routine CalcLoanEstimateTotalAdjustmentsCredit;
    internal Routine CalcLoanEstimateTaxesInsuranceInEscrowOrNot;
    internal Routine CalcLoanEstimateEstimatedTaxes;
    internal Routine CalcClosingDisclosureEstimatedTaxes;
    internal Routine CalcCDPage3TotalK;
    internal Routine CalcCDPage3TotalL;
    internal Routine CalcCDPage3TotalM;
    internal Routine CalcCDPage3TotalN;
    internal Routine CalcCDPage3TotalPurchasePayoffsIncluded;
    internal Routine CalcCDPage3CashToClose;
    internal Routine CalcCDPage3Cash;
    internal Routine CalcCDPage3ClosingCosts;
    internal Routine CalcCDPage3SellerCredit;
    internal Routine CalcPropertyCosts_OtherRow;
    internal Routine CalcEstimatedPropertyCosts;
    internal Routine CalcSection700;
    internal Routine CalcLineItem701;
    internal Routine CalcLineItem702;
    internal Routine CalcCdPage3Liabilities;
    internal Routine CalcWillHaveEscrowAccount;
    internal Routine PopulateCDPage4PartialPaymentIndicators;
    internal Routine Calc2015LineItem904;
    internal Routine Calc2015LineItem906;
    internal Routine Calc2015LineItem907;
    internal Routine Calc2015LineItem908;
    internal Routine Calc2015LineItem909;
    internal Routine Calc2015LineItem910;
    internal Routine Calc2015LineItem911;
    internal Routine Calc2015LineItem912;
    internal Routine CalcCDPage3AlternateCashToCloseFinal;
    internal Routine CalcCDPage3AlternateCashToCloseLoanEstimate;
    internal Routine CalcCDPage3StdCashToCloseFinal;
    internal Routine CalcCDPage3StdCashToCloseLoanEstimate;
    internal Routine CalcCDPage3StdDidChangeCol;
    internal Routine CalcCDPage3AltDidChangeCol;
    internal Routine CalcPropertyInsuranceTaxIndicators;
    internal Routine CalcLoanEstimate_EstimatedCashToClose;
    internal Routine CopyLoanNumToLoanIDNum;
    internal Routine CalcLERedisclosureFlag;
    internal Routine CalcCDRedisclosureFlag;
    internal Routine PopulateLoanTypeOtherField;
    internal Routine CalcLECashToCloseSellerCredit;
    internal Routine CalcCDPage3StdCashToCloseSellerCredit;
    internal Routine CalcLoanEstimateThirdPartyPaymentsNotOtherwiseDisclosed;
    internal Routine CalcLoanEstimate_IncludePayoffs;
    internal Routine CalcLoanEstimate_ClosingCostsFinanced;
    internal Routine RefreshFormList;
    internal Routine CalcLE3OtherConsiderationAssumptions;
    internal Routine CalcSellerNames;
    internal Routine SetCustomizedProjectedPaymentTable;
    internal Routine SetAppliedCureAmount;
    internal Routine Calc60Payments;
    internal Routine CalcSetExcludeBorrowerClosingCosts;
    internal Routine SetReasonForChangeCircumstances;
    internal Routine CalcMICReference;
    internal Routine SetDefaultValuesForClosingCostExpiration;
    internal Routine SetDefaultValuesForRateLockExpiration;
    internal Routine CalcAdjustmentsLineK04;
    internal Routine CalcAdjustmentsLineK05_06_07;
    internal Routine CalcAdjustmentsLineL4_L6_L7;
    internal Routine CalcAdjustmentsLineL8_L9_L10_L11;
    internal Routine CalcAdjustmentsLineL17;
    internal Routine CalcAdjustmentsLineM03;
    internal Routine CalcAdjustmentsLineM04;
    internal Routine CalcAdjustmentsLineM05;
    internal Routine CalcAdjustmentsLineM06;
    internal Routine CalcAdjustmentsLineM07;
    internal Routine CalcAdjustmentsLineM08;
    internal Routine CalcAdjustmentsLineM16;
    internal Routine CalcAdjustmentsLineN07;
    internal Routine CalcAdjustmentsLineN13;
    internal Routine CalcAdjustmentsLineN19;
    internal Routine CalcAdjustmentsLineKSubtotal;
    internal Routine CalcAdjustmentsLineLSubtotal;
    internal Routine CalcAdjustmentsUcdTotal;
    internal Routine CalcAdjustmentsNonUcdTotal;
    internal Routine CalcAdjustmentsDetailTotalMigration;
    internal Routine CalcConstructionPermPeriod;
    internal Routine CalcAsCompletedPurchasePrice;
    internal Routine CalcAsCompletedAppraisedValue;
    internal Routine CalcTotalAdjustmentAndOtherCredits;
    internal Routine CalcDisclosedSalesPrice;
    internal Routine CalcIntentToProceed;
    internal Routine CalcLECDDisplayFields;
    internal Routine CalcSubsequentlyPaidFinanceCharge;
    internal Routine ClearPostConsummationSection;
    internal Routine CalcToleranceCureAppliedCureAmount;
    internal Routine CalcGenrateCD3Liabilities;
    internal Routine CalcPrepaidMI;
    internal Routine CalcMIEscrowIndicator;
    internal Routine CalcLenderObligatedIndicator;
    internal Routine CalculateLotOwnedFreeAndClearIndicator;
    private readonly NewHud2015CalculationServant _newHud2015CalculationServant;
    private int closingCostDaysToExpire;
    private string rateLockExpirationTimeZoneSetting;
    private string closingCostExpirationTimeZoneSetting;
    private static Dictionary<string, string> LenderObligatedFeeMapping_borrAmt_indicator = new Dictionary<string, string>()
    {
      {
        "554",
        "NEWHUD2.X4780"
      },
      {
        "NEWHUD.X657",
        "NEWHUD2.X4781"
      },
      {
        "NEWHUD.X1604",
        "NEWHUD2.X4782"
      },
      {
        "NEWHUD.X1612",
        "NEWHUD2.X4783"
      },
      {
        "NEWHUD.X1620",
        "NEWHUD2.X4784"
      },
      {
        "NEWHUD.X1627",
        "NEWHUD2.X4785"
      }
    };
    private Dictionary<int, List<string>> totalVolLiability = new Dictionary<int, List<string>>();
    private Dictionary<int, List<string>> totalNonVolLiability = new Dictionary<int, List<string>>();
    private FieldDefinition fl0062Options = EncompassFields.GetField("FL0062");

    internal int ClosingCostDaysToExpire
    {
      get => this.closingCostDaysToExpire;
      set => this.closingCostDaysToExpire = value;
    }

    internal string RateLockExpirationTimeZoneSetting
    {
      get => this.rateLockExpirationTimeZoneSetting;
      set => this.rateLockExpirationTimeZoneSetting = value;
    }

    internal string ClosingCostExpirationTimeZoneSetting
    {
      get => this.closingCostExpirationTimeZoneSetting;
      set => this.closingCostExpirationTimeZoneSetting = value;
    }

    internal NewHUD2015Calculation(
      SessionObjects sessionObjects,
      ILoanConfigurationInfo configInfo,
      LoanData l,
      EllieMae.EMLite.CalculationEngine.CalculationObjects calObjs)
      : base(l, calObjs)
    {
      this.sessionObjects = sessionObjects;
      this.configInfo = configInfo;
      this.CalcLoanTermTable_LoanAmount = this.RoutineX(new Routine(this.calculateLoanTermTable_LoanAmount));
      this.CalcLoanTermTable_InterestRate = this.RoutineX(new Routine(this.calculateLoanTermTable_InterestRate));
      this.CalcLoanTermTable_MonthlyPrincipalAndInterest = this.RoutineX(new Routine(this.calculateLoanTermTable_MonthlyPrincipalAndInterest));
      this.CalcLoanTermTable_Prepayment = this.RoutineX(new Routine(this.calculateLoanTermTable_Prepayment));
      this.PopulateLoanEstimate_LoanPurpose = this.RoutineX(new Routine(this.populateLoanEstimate_LoanPurpose));
      this.PopulateLoanTypeOtherField = this.RoutineX(new Routine(this.populateLoanType_OtherField));
      this.CalcLoanTermTable_LoanTerm = this.RoutineX(new Routine(this.calculateLoanTermTable_LoanTerm));
      this.CalcClosingDisclosurePage5 = this.RoutineX(new Routine(this.calculateClosingDisclosurePage5));
      this.CalcLoanEstimate_ClosingCostExpDate = this.RoutineX(new Routine(this.calculateLoanEstimate_ClosingCostExpDate));
      this.PopulateLoanEstimate_LenderBrokerInfo = this.RoutineX(new Routine(this.populateLoanEstimate_LenderBrokerInfo));
      this.CalcLoanEstimate_AIRMonthSuffix = this.RoutineX(new Routine(this.calculateLoanEstimate_AIRMonthSuffix));
      this.AggrEscrowAcc_EscrowedCosts = this.RoutineX(new Routine(this.aggrEscrowAcc_EscrowedCosts));
      this.CalcClosingCostSubTotal = this.RoutineX(new Routine(this.calculateClosingCostSubTotal));
      this.CalcLoanEstimateTotalLoanCosts = this.RoutineX(new Routine(this.calculateLoanEstimate_TotalLoanCosts));
      this.CalcLoanEstimateTotalOtherCosts = this.RoutineX(new Routine(this.calculateLoanEstimate_TotalOtherCosts));
      this.CalcLoanEstimateTotalClosingCosts = this.RoutineX(new Routine(this.calculateLoanEstimate_TotalClosingCosts));
      this.CalcClosingDisclosureTotalLoanCosts = this.RoutineX(new Routine(this.calculateClosingDisclosure_TotalLoanCosts));
      this.CalcClosingDisclosureTotalOtherCosts = this.RoutineX(new Routine(this.calculateClosingDisclosure_TotalOtherCosts));
      this.CalcClosingDisclosureTotalClosingCosts = this.RoutineX(new Routine(this.calculateClosingDisclosure_TotalClosingCosts));
      this.CalculateProductDescription = this.RoutineX(new Routine(this.calculateProductDescription));
      this.CalculateLoanEstimate_ComparisonsTable = this.RoutineX(new Routine(this.calculateLoanEstimate_ComparisonsTable));
      this.CalcClosingDisclosureTotalPayments = this.RoutineX(new Routine(this.calculateLoan_TotalPayments));
      this.CalcAPTable_InterestOnlyPayments = this.RoutineX(new Routine(this.calculateAPTable_InterestOnlyPayments));
      this.CalcAPTable_FirstChangePayment = this.RoutineX(new Routine(this.calculateAPTable_FirstChangePayment));
      this.CalcAPTable_SubsequentChanges = this.RoutineX(new Routine(this.calculateAPTable_SubsequentChanges));
      this.CalcAPTable_MaximumPayment = this.RoutineX(new Routine(this.calculateAPTable_MaximumPayment));
      this.CalcLoanEstimateDownPaymentAndFundsForBorrower = this.RoutineX(new Routine(this.calculateLoanEstimate_DownPaymentAndFundsForBorrower));
      this.CalcLoanEstimateEstimatedCashToCloseSV = this.RoutineX(new Routine(this.calculateLoanEstimate_EstimatedCashToCloseSV));
      this.CalcLoanEstimateEstimatedCashToCloseAV = this.RoutineX(new Routine(this.calculateLoanEstimate_EstimatedCashToCloseAV));
      this.CalcLoanEstimateTotalAdjustmentsCredit = this.RoutineX(new Routine(this.calculateLoanEstimate_TotalAdjustmentsCredit));
      this.CalcLoanEstimateTaxesInsuranceInEscrowOrNot = this.RoutineX(new Routine(this.calculateLoanEstimate_TaxesInsuranceInEscrowOrNot));
      this.CalcLoanEstimateEstimatedTaxes = this.RoutineX(new Routine(this.calculateLoanEstimate_EstimatedTaxes));
      this.CalcClosingDisclosureEstimatedTaxes = this.RoutineX(new Routine(this.calculateClosingDisclosure_EstimatedTaxes));
      this.SetDisclosureTrackingLogData = this.RoutineX(new Routine(this.setDisclosureTrackingLogData));
      this.CalcCDPage3TotalK = this.RoutineX(new Routine(this.calculateCDPage3TotalK));
      this.CalcCDPage3TotalL = this.RoutineX(new Routine(this.calculateCDPage3TotalL));
      this.CalcCDPage3TotalPurchasePayoffsIncluded = this.RoutineX(new Routine(this.calculateCDPage3TotalPurchasePayoffsIncluded));
      this.CalcCDPage3TotalM = this.RoutineX(new Routine(this.calculateCDPage3TotalM));
      this.CalcCDPage3TotalN = this.RoutineX(new Routine(this.calculateCDPage3TotalN));
      this.CalcCDPage3CashToClose = this.RoutineX(new Routine(this.calculateCDPage3CashToClose));
      this.CalcCDPage3Cash = this.RoutineX(new Routine(this.calculateCDPage3Cash));
      this.CalcCDPage3ClosingCosts = this.RoutineX(new Routine(this.calculateCDPage3ClosingCosts));
      this.CalcCDPage3SellerCredit = this.RoutineX(new Routine(this.calculateCDPage3SellerCredit));
      this.CalcPropertyCosts_OtherRow = this.RoutineX(new Routine(this.calculatePropertyCosts_OtherRow));
      this.CalcEstimatedPropertyCosts = this.RoutineX(new Routine(this.calculateEstimatedPropertyCosts));
      this.CalcSection700 = this.RoutineX(new Routine(this.calculateSection700));
      this.CalcLineItem701 = this.RoutineX(new Routine(this.calculateLineItem701));
      this.CalcLineItem702 = this.RoutineX(new Routine(this.calculateLineItem702));
      this.Calc2015LineItem904 = this.RoutineX(new Routine(this.calculate2015LineItem904Fee));
      this.Calc2015LineItem906 = this.RoutineX(new Routine(this.calculate2015LineItem906Fee));
      this.Calc2015LineItem907 = this.RoutineX(new Routine(this.calculate2015LineItem907Fee));
      this.Calc2015LineItem908 = this.RoutineX(new Routine(this.calculate2015LineItem908Fee));
      this.Calc2015LineItem909 = this.RoutineX(new Routine(this.calculate2015LineItem909Fee));
      this.Calc2015LineItem910 = this.RoutineX(new Routine(this.calculate2015LineItem910Fee));
      this.Calc2015LineItem911 = this.RoutineX(new Routine(this.calculate2015LineItem911Fee));
      this.Calc2015LineItem912 = this.RoutineX(new Routine(this.calculate2015LineItem912Fee));
      this.CalcCdPage3Liabilities = this.RoutineX(new Routine(this.calculateCdPage3Liabilities));
      this.CalcCDPage3AlternateCashToCloseFinal = this.RoutineX(new Routine(this.CalculateCDPage3AlternateCashToCloseFinal));
      this.CalcCDPage3AlternateCashToCloseLoanEstimate = this.RoutineX(new Routine(this.CalculateCDPage3AlternateCashToCloseLoanEstimate));
      this.CalcCDPage3StdCashToCloseFinal = this.RoutineX(new Routine(this.CalculateCDPage3StdCashToCloseFinal));
      this.CalcCDPage3StdCashToCloseLoanEstimate = this.RoutineX(new Routine(this.CalculateCDPage3StdCashToCloseLoanEstimate));
      this.CalcCDPage3StdDidChangeCol = this.RoutineX(new Routine(this.CalculateCDPage3StdDidChangeCol));
      this.CalcCDPage3AltDidChangeCol = this.RoutineX(new Routine(this.CalculateCDPage3AltDidChangeCol));
      this.CalcWillHaveEscrowAccount = this.RoutineX(new Routine(this.calculateWillHaveEscrowAccount));
      this.PopulateCDPage4PartialPaymentIndicators = this.RoutineX(new Routine(this.populateCDPage4PartialPaymentIndicators));
      this.CalcLERedisclosureFlag = this.RoutineX(new Routine(this.calculateLERedisclosureFlag));
      this.CalcCDRedisclosureFlag = this.RoutineX(new Routine(this.calculateCDRedisclosureFlag));
      this.CalcLECashToCloseSellerCredit = this.RoutineX(new Routine(this.calculateLECashToCloseSellerCredit));
      this.CalcCDPage3StdCashToCloseSellerCredit = this.RoutineX(new Routine(this.calculateCDPage3StdCashToCloseSellerCredit));
      this.CalcPropertyInsuranceTaxIndicators = this.RoutineX(new Routine(this.calculatePropertyInsuranceTaxIndicators));
      this.CalcLoanEstimate_EstimatedCashToClose = this.RoutineX(new Routine(this.calculateLoanEstimate_EstimatedCashToClose));
      this.RefreshFormList = this.RoutineX(new Routine(this.refreshFormList));
      this.SetAIRTableIndexType = this.RoutineX(new Routine(this.setAIRTableIndexType));
      this.CalcSellerNames = this.RoutineX(new Routine(this.calculateSellerNames));
      this.SetCustomizedProjectedPaymentTable = this.RoutineX(new Routine(this.setCustomizedProjectedPaymentTable));
      this.SetAppliedCureAmount = this.RoutineX(new Routine(this.setAppliedCureAmount));
      this.CalcLE3OtherConsiderationAssumptions = this.RoutineX(new Routine(this.calculateLE3OtherConsiderationAssumptions));
      this.CopyLoanNumToLoanIDNum = this.RoutineX(new Routine(this.copyLoanNumToLoanIDNum));
      this.CalcSetExcludeBorrowerClosingCosts = this.RoutineX(new Routine(this.calculateSetExcludeBorrowerClosingCosts));
      this.addFieldHandlers();
      this._newHud2015CalculationServant = new NewHud2015CalculationServant((ILoanModelProvider) this, (ISettingsProvider) new SystemSettings(sessionObjects));
      this.Calc60Payments = this.RoutineX(new Routine(this.calculate60Payments));
      this.CalcLoanEstimateThirdPartyPaymentsNotOtherwiseDisclosed = this.RoutineX(new Routine(this.calculateLoanEstimate_ThirdPartyPaymentsNotOtherwiseDisclosed));
      this.CalcLoanEstimate_IncludePayoffs = this.RoutineX(new Routine(this.calculateLoanEstimate_IncludePayoffs));
      this.CalcLoanEstimate_ClosingCostsFinanced = this.RoutineX(new Routine(this.calculateLoanEstimate_ClosingCostsFinanced));
      this.SetReasonForChangeCircumstances = this.RoutineX(new Routine(this.setReasonForChangeCircumstances));
      this.CalcMICReference = this.RoutineX(new Routine(this.calculateMICReference));
      this.SetDefaultValuesForClosingCostExpiration = this.RoutineX(new Routine(this.setDefaultValuesForClosingCostExpiration));
      this.SetDefaultValuesForRateLockExpiration = this.RoutineX(new Routine(this.setDefaultValuesForRateLockExpiration));
      this.CalcAdjustmentsLineK04 = this.RoutineX(new Routine(this.calculateAdjustmentsLineK04));
      this.CalcAdjustmentsLineK05_06_07 = this.RoutineX(new Routine(this.calculateAdjustmentsLineK05_06_07));
      this.CalcAdjustmentsLineL4_L6_L7 = this.RoutineX(new Routine(this.calculateAdjustmentsLineL4_L6_L7));
      this.CalcAdjustmentsLineL8_L9_L10_L11 = this.RoutineX(new Routine(this.calculateAdjustmentsLineL8_L9_L10_L11));
      this.CalcAdjustmentsLineL17 = this.RoutineX(new Routine(this.calculateAdjustmentsLineL17));
      this.CalcAdjustmentsLineM03 = this.RoutineX(new Routine(this.calculateAdjustmentsLineM03));
      this.CalcAdjustmentsLineM04 = this.RoutineX(new Routine(this.calculateAdjustmentsLineM04));
      this.CalcAdjustmentsLineM05 = this.RoutineX(new Routine(this.calculateAdjustmentsLineM05));
      this.CalcAdjustmentsLineM06 = this.RoutineX(new Routine(this.calculateAdjustmentsLineM06));
      this.CalcAdjustmentsLineM07 = this.RoutineX(new Routine(this.calculateAdjustmentsLineM07));
      this.CalcAdjustmentsLineM08 = this.RoutineX(new Routine(this.calculateAdjustmentsLineM08));
      this.CalcAdjustmentsLineM16 = this.RoutineX(new Routine(this.calculateAdjustmentsLineM16));
      this.CalcAdjustmentsLineN07 = this.RoutineX(new Routine(this.calculateAdjustmentsLineN07));
      this.CalcAdjustmentsLineN13 = this.RoutineX(new Routine(this.calculateAdjustmentsLineN13));
      this.CalcAdjustmentsLineN19 = this.RoutineX(new Routine(this.calculateAdjustmentsLineN19));
      this.CalcAdjustmentsLineKSubtotal = this.RoutineX(new Routine(this.calculateAdjustmentsLineKSubtotal));
      this.CalcAdjustmentsLineLSubtotal = this.RoutineX(new Routine(this.calculateAdjustmentsLineLSubtotal));
      this.CalcAdjustmentsUcdTotal = this.RoutineX(new Routine(this.calculateUcdAdjustmentTotal));
      this.CalcAdjustmentsNonUcdTotal = this.RoutineX(new Routine(this.calculateNonUcdAdjustmentTotal));
      this.CalcAdjustmentsDetailTotalMigration = this.RoutineX(new Routine(this.calculateAdjustmentsDetialTotalMigration));
      this.CalcConstructionPermPeriod = this.RoutineX(new Routine(this.calculateConstructionPermPeriod));
      this.CalcAsCompletedPurchasePrice = this.RoutineX(new Routine(this.calculateAsCompletedPurchasePrice));
      this.CalcAsCompletedAppraisedValue = this.RoutineX(new Routine(this.calculateAsCompletedAppraisedValue));
      this.CalcTotalAdjustmentAndOtherCredits = this.RoutineX(new Routine(this.calculateTotalAdjustmentAndOtherCredits));
      this.CalcDisclosedSalesPrice = this.RoutineX(new Routine(this.calculateDisclosedSalesPrice));
      this.CalcIntentToProceed = this.RoutineX(new Routine(this.calculateIntentToProceed));
      this.CalcLECDDisplayFields = this.RoutineX(new Routine(this.calculateLECDDisplayFields));
      this.CalcSubsequentlyPaidFinanceCharge = this.RoutineX(new Routine(this.calculateSubsequentlyPaidFinanceCharge));
      this.ClearPostConsummationSection = this.RoutineX(new Routine(this.clearPostConsummationSection));
      this.CalcToleranceCureAppliedCureAmount = this.RoutineX(new Routine(this.calculateToleranceCureAppliedCureAmount));
      this.CalcGenrateCD3Liabilities = this.RoutineX(new Routine(this.genrateCD3Liabilities));
      this.CalcPrepaidMI = this.RoutineX(new Routine(this.calculatePrepaidMI));
      this.CalcMIEscrowIndicator = this.RoutineX(new Routine(this.calculateMIEscrowIndicator));
      this.CalcLenderObligatedIndicator = this.RoutineX(new Routine(this.calculateLenderObligatedIndicator));
      this.CalculateLotOwnedFreeAndClearIndicator = this.RoutineX(new Routine(this.calculateLotOwnedFreeAndClearIndicator));
    }

    private void addFieldHandlers()
    {
      this.AddFieldHandler("675", this.RoutineX(new Routine(this.calculateLoanTermTable_Prepayment)));
      Routine routine1 = this.RoutineX(new Routine(this.calculateLoanTermTable_MonthlyPrincipalAndInterest));
      this.AddFieldHandler("LE1.X13", routine1);
      this.AddFieldHandler("LE1.X14", routine1);
      this.AddFieldHandler("LE1.X15", routine1);
      this.AddFieldHandler("LE1.X16", routine1);
      this.AddFieldHandler("LE1.X19", routine1);
      this.AddFieldHandler("LE1.X20", routine1);
      this.AddFieldHandler("LE1.X21", routine1);
      this.AddFieldHandler("LE1.X22", routine1);
      Routine routine2 = this.RoutineX(new Routine(this.calculateLoanEstimate_ClosingCostExpDate));
      this.AddFieldHandler("LE1.X1", routine2);
      this.AddFieldHandler("LE1.X28", routine2);
      Routine routine3 = this.RoutineX(new Routine(this.populateLoanEstimate_LenderBrokerInfo));
      this.AddFieldHandler("3032", routine3);
      this.AddFieldHandler("95", routine3);
      this.AddFieldHandler("VEND.X300", routine3);
      this.AddFieldHandler("VEND.X305", routine3);
      this.AddFieldHandler("VEND.X303", routine3);
      this.AddFieldHandler("1612", routine3);
      this.AddFieldHandler("3238", routine3);
      this.AddFieldHandler("3968", routine3);
      this.AddFieldHandler("1823", routine3);
      this.AddFieldHandler("2306", routine3);
      this.AddFieldHandler("LE2.X99", this.RoutineX(new Routine(this.calculateLoanEstimate_AIRMonthSuffix)));
      Routine routine4 = this.RoutineX(new Routine(this.aggrEscrowAcc_EscrowedCosts));
      this.AddFieldHandler("HUD41", routine4);
      this.AddFieldHandler("HUD42", routine4);
      this.AddFieldHandler("HUD43", routine4);
      this.AddFieldHandler("HUD44", routine4);
      this.AddFieldHandler("HUD45", routine4);
      this.AddFieldHandler("HUD46", routine4);
      this.AddFieldHandler("HUD47", routine4);
      this.AddFieldHandler("HUD48", routine4);
      this.AddFieldHandler("HUD50", routine4);
      this.AddFieldHandler("HUD66", routine4);
      this.AddFieldHandler("HUD67", routine4);
      this.AddFieldHandler("CD4.X27", this.RoutineX(new Routine(this.calculateProductDescription)));
      Routine routine5 = this.RoutineX(new Routine(this.calculateLoanEstimate_TotalLoanCosts));
      this.AddFieldHandler("LE2.XSTA", routine5);
      this.AddFieldHandler("LE2.XSTB", routine5);
      this.AddFieldHandler("LE2.XSTC", routine5);
      Routine routine6 = this.RoutineX(new Routine(this.calculateLoanEstimate_TotalOtherCosts));
      this.AddFieldHandler("LE2.XSTE", routine6);
      this.AddFieldHandler("LE2.XSTF", routine6);
      this.AddFieldHandler("LE2.XSTG", routine6);
      this.AddFieldHandler("LE2.XSTH", routine6);
      this.AddFieldHandler("LE2.XLC", this.RoutineX(new Routine(this.calculateLoanEstimate_TotalClosingCosts)));
      Routine routine7 = this.RoutineX(new Routine(this.calculateLoanEstimate_ComparisonsTable));
      this.AddFieldHandler("912", routine7);
      this.AddFieldHandler("NEWHUD.X6", routine7);
      this.AddFieldHandler("CD4.X25", routine7);
      this.AddFieldHandler("CD4.X27", routine7);
      this.AddFieldHandler("CD4.X23", routine7);
      this.AddFieldHandler("CD5.X1", this.RoutineX(new Routine(this.calculateLoan_TotalPayments)));
      Routine routine8 = this.RoutineX(new Routine(this.calculateLoanEstimate_EstimatedCashToCloseSV));
      this.AddFieldHandler("LE2.X2", routine8);
      this.AddFieldHandler("LE2.X3", routine8);
      Routine routine9 = this.RoutineX(new Routine(this.calculateNonUcdAdjustmentTotal));
      this.AddFieldHandler("LE2.X6", routine9);
      this.AddFieldHandler("LE2.X8", routine9);
      this.AddFieldHandler("LE2.X10", routine9);
      this.AddFieldHandler("LE2.X12", routine9);
      this.AddFieldHandler("LE2.X14", routine9);
      this.AddFieldHandler("LE2.X16", routine9);
      this.AddFieldHandler("LE2.X18", routine9);
      this.AddFieldHandler("LE2.X20", routine9);
      this.AddFieldHandler("LE2.X22", routine9);
      this.AddFieldHandler("LE2.X24", routine9);
      this.AddFieldHandler("COMPLIANCEVERSION.CD3X1505", routine9);
      Routine routine10 = this.RoutineX(new Routine(this.calculateCDPage3TotalK));
      this.AddFieldHandler("CD3.X1", routine10);
      this.AddFieldHandler("CD3.X5", routine10);
      Routine routine11 = this.RoutineX(new Routine(this.calculateCDPage3TotalM));
      this.AddFieldHandler("CD3.X29", routine11);
      this.AddFieldHandler("CD3.X31", routine11);
      Routine routine12 = this.RoutineX(new Routine(this.calculateCDPage3TotalN));
      this.AddFieldHandler("CD3.X33", routine12);
      this.AddFieldHandler("CD3.X35", routine12);
      this.AddFieldHandler("CD3.X37", routine12);
      this.AddFieldHandler("CD3.X46", routine12);
      Routine routine13 = this.RoutineX(new Routine(this.calculateCDPage3TotalL));
      this.AddFieldHandler("CD3.X108", routine13);
      this.AddFieldHandler("CD3.X10", routine13);
      this.AddFieldHandler("CD3.X12", routine13);
      this.AddFieldHandler("CD3.X20", routine13);
      Routine routine14 = this.RoutineX(new Routine(this.calculateCDPage3CashToClose));
      this.AddFieldHandler("CD3.X41", routine14);
      this.AddFieldHandler("CD3.X42", routine14);
      Routine routine15 = this.RoutineX(new Routine(this.calculateCDPage3Cash));
      this.AddFieldHandler("CD3.X43", routine15);
      this.AddFieldHandler("CD3.X44", routine15);
      Routine routine16 = this.RoutineX(new Routine(this.calculateClosingDisclosure_TotalLoanCosts));
      this.AddFieldHandler("CD2.XLCAC", routine16);
      this.AddFieldHandler("CD2.XLCBC", routine16);
      Routine routine17 = this.RoutineX(new Routine(this.calculateClosingDisclosure_TotalOtherCosts));
      this.AddFieldHandler("CD2.XOCAC", routine17);
      this.AddFieldHandler("CD2.XOCBC", routine17);
      Routine routine18 = this.RoutineX(new Routine(this.calculateClosingDisclosure_TotalClosingCosts));
      this.AddFieldHandler("CD2.XBCCAC", routine18);
      this.AddFieldHandler("CD2.XSCCAC", routine18);
      this.AddFieldHandler("CD2.XSCCBC", routine18);
      this.AddFieldHandler("CD2.XCCBO", routine18);
      this.AddFieldHandler("NEWHUD2.X278", this.RoutineX(new Routine(this.calculateCDPage3SellerCredit)));
      Routine routine19 = this.RoutineX(new Routine(this.calculateWillHaveEscrowAccount));
      this.AddFieldHandler("DISCLOSURE.X913", routine19);
      this.AddFieldHandler("DISCLOSURE.X339", routine19);
      this.AddFieldHandler("CD4.X51", routine19);
      this.AddFieldHandler("CD4.X8", routine19);
      this.AddFieldHandler("CD4.X40", routine19);
      this.AddFieldHandler("CD4.X41", routine19);
      this.AddFieldHandler("CD4.X7", routine19);
      Routine routine20 = this.RoutineX(new Routine(this.CalculateCDPage3StdCashToCloseLoanEstimate));
      this.AddFieldHandler("CD3.X93", routine20);
      this.AddFieldHandler("CD3.X95", routine20);
      this.AddFieldHandler("CD3.X96", routine20);
      this.AddFieldHandler("CD3.X97", routine20);
      this.AddFieldHandler("CD3.X98", routine20);
      this.AddFieldHandler("CD3.X99", routine20);
      this.AddFieldHandler("CD3.X100", routine20);
      Routine routine21 = this.RoutineX(new Routine(this.CalculateCDPage3AlternateCashToCloseLoanEstimate));
      this.AddFieldHandler("CD3.X87", routine21);
      this.AddFieldHandler("CD3.X88", routine21);
      this.AddFieldHandler("CD3.X90", routine21);
      this.AddFieldHandler("364", this.RoutineX(new Routine(this.copyLoanNumToLoanIDNum)));
      this.AddFieldHandler("LE1.X5", this.RoutineX(new Routine(this.calculateProductDescription)));
      Routine routine22 = this.RoutineX(new Routine(this.calculateSellerNames));
      this.AddFieldHandler("638", routine22);
      this.AddFieldHandler("VEND.X412", routine22);
      Routine routine23 = this.RoutineX(new Routine(this.populateCDPage4PartialPaymentIndicators));
      this.AddFieldHandler("CD4.X3", routine23);
      this.AddFieldHandler("CD4.X42", routine23);
      this.AddFieldHandler("CD4.X43", routine23);
      Routine routine24 = this.RoutineX(new Routine(this.calculateAPTable_FirstChangePayment));
      this.AddFieldHandler("PAYMENTTABLE.CUSTOMIZE", routine24);
      this.AddFieldHandler("PAYMENTTABLE.USEACTUALPAYMENTCHANGE", routine24);
      this.AddFieldHandler("CD4.X31", routine24);
      this.AddFieldHandler("CD4.X10", routine24);
      Routine routine25 = this.RoutineX(new Routine(this.CalculateCDPage3StdCashToCloseFinal));
      this.AddFieldHandler("CD3.X103", routine25);
      this.AddFieldHandler("CD3.X105", routine25);
      this.AddFieldHandler("CD3.X106", routine25);
      this.AddFieldHandler("CD3.X107", routine25);
      Routine routine26 = this.RoutineX(new Routine(this.CalculateCDPage3AlternateCashToCloseFinal));
      this.AddFieldHandler("CD3.X81", routine26);
      this.AddFieldHandler("CD3.X82", routine26);
      this.AddFieldHandler("CD3.X83", routine26);
      Routine routine27 = this.RoutineX(new Routine(this.syncFileContactToCD5));
      this.AddFieldHandler("VEND.X993", routine27);
      this.AddFieldHandler("VEND.X994", routine27);
      this.AddFieldHandler("VEND.X654", routine27);
      this.AddFieldHandler("VEND.X135", routine27);
      this.AddFieldHandler("VEND.X136", routine27);
      this.AddFieldHandler("VEND.X137", routine27);
      this.AddFieldHandler("VEND.X146", routine27);
      this.AddFieldHandler("VEND.X147", routine27);
      this.AddFieldHandler("VEND.X148", routine27);
      this.AddFieldHandler("VEND.X657", routine27);
      this.AddFieldHandler("VEND.X658", routine27);
      this.AddFieldHandler("VEND.X659", routine27);
      Routine routine28 = this.RoutineX(new Routine(this.setReasonForChangeCircumstances));
      this.AddFieldHandler("3169", routine28);
      this.AddFieldHandler("CD1.X64", routine28);
      this.AddFieldHandler("CD1.X70", routine28);
      this.AddFieldHandler("LE1.X90", routine28);
      Routine routine29 = this.RoutineX(new Routine(this.calculateMICReference));
      this.AddFieldHandler("CD1.X71", routine29);
      this.AddFieldHandler("1040", routine29);
      this.AddFieldHandler("LE3.X12", this.RoutineX(new Routine(this.calculateLE3OtherConsiderationAssumptions)));
      this.AddFieldHandler("CD1.X3", this.RoutineX(new Routine(this.calculateClosingDisclosure_EstimatedTaxes)));
      this.AddFieldHandler("LE1.X29", this.RoutineX(new Routine(this.calculateLoanEstimate_EstimatedTaxes)));
      Routine routine30 = this.RoutineX(new Routine(this.calculateLECDDisplayFields));
      this.AddFieldHandler("KBYO.XD3", routine30);
      this.AddFieldHandler("NEWHUD.X555", routine30);
      this.AddFieldHandler("2625", routine30);
      this.AddFieldHandler("232", this.RoutineX(new Routine(this.calculateMIEscrowIndicator)));
      Routine routine31 = this.RoutineX(new Routine(this.calculateLenderObligatedIndicator));
      this.AddFieldHandler("554", routine31);
      this.AddFieldHandler("NEWHUD.X657", routine31);
      this.AddFieldHandler("NEWHUD.X1604", routine31);
      this.AddFieldHandler("NEWHUD.X1612", routine31);
      this.AddFieldHandler("NEWHUD.X1620", routine31);
      this.AddFieldHandler("NEWHUD.X1627", routine31);
    }

    private void calculateMIEscrowIndicator(string id, string val)
    {
      if (this.FltVal("232") > 0.0)
      {
        if (!(this.Val("NEWHUD2.X4769") == ""))
          return;
        this.SetVal("NEWHUD2.X4769", "Y");
      }
      else
        this.SetVal("NEWHUD2.X4769", "");
    }

    private void calculateLenderObligatedIndicator(string id, string val)
    {
      if (!NewHUD2015Calculation.LenderObligatedFeeMapping_borrAmt_indicator.ContainsKey(id))
        return;
      string id1 = NewHUD2015Calculation.LenderObligatedFeeMapping_borrAmt_indicator[id];
      if (Utils.ToDouble(val) != 0.0)
        this.SetVal(id1, "Y");
      else
        this.SetVal(id1, "");
    }

    private void calculateIntentToProceed(string id, string val)
    {
      if (this.Val("3164") != "Y")
      {
        this.SetVal("LE1.XD8", this.Val("LE1.X8"));
        this.SetVal("LE1.XD9", this.Val("LE1.X9"));
        this.SetVal("LE1.XD28", this.Val("LE1.X28"));
      }
      else
      {
        this.SetVal("LE1.XD8", "");
        this.SetVal("LE1.XD9", "");
        this.SetVal("LE1.XD28", "");
      }
    }

    private void calculateToleranceCureAppliedCureAmount(string id, string val)
    {
      this.loan.SetField("FV.X366", (this.FltVal("FV.X396") + this.FltVal("FV.X397")).ToString());
      if (id == "FV.X396")
        this.loan.SetField("CD2.X2", this.Val("FV.X396"));
      this.calObjs.FeeVarianceToolCal.ReCal_ItemsThatCannotDecrease();
    }

    private void calculateNonVol()
    {
      int num = 1;
      int numberOfNonVols = this.loan.GetNumberOfNonVols();
      string str1 = "";
      bool flag = true;
      string str2 = this.Val("FV.X397");
      for (; num <= numberOfNonVols; ++num)
      {
        str1 = "UNFL" + num.ToString("00");
        if (!string.IsNullOrEmpty(this.loan.GetField(str1 + "08")))
        {
          flag = false;
          break;
        }
      }
      if (flag)
      {
        if (string.IsNullOrEmpty(str1))
          num = numberOfNonVols + 1;
        this.loan.CreateNonVols(new Dictionary<int, List<string>>()
        {
          {
            num,
            new List<string>()
            {
              "Other",
              "$" + str2 + " Principal Reduction P.O.C. Lender",
              "PrincipalReductionCure",
              "",
              "N",
              "$ " + str2 + " Principal Reduction to Borrower (for exceeding legal limits P.O.C. Lender)"
            }
          }
        });
      }
      else if (string.IsNullOrEmpty(str2))
      {
        this.loan.RemoveNonVolAt(num - 1);
      }
      else
      {
        string field = this.loan.GetField(str1 + "07");
        this.SetVal(str1 + "02", "$" + str2 + " Principal Reduction P.O.C. " + field);
        this.SetVal(str1 + "08", "$ " + str2 + " Principal Reduction to Borrower (for exceeding legal limits P.O.C. " + field + ")");
      }
    }

    private void genrateCD3Liabilities(string id, string val)
    {
      this.calculateNonVol();
      this.calculateTotalLiabilities();
      int num1 = 0;
      Decimal num2 = 0.00M;
      if (this.totalVolLiability.Count <= 0 && this.totalNonVolLiability.Count <= 0)
      {
        for (int index = 0; index <= 20; ++index)
        {
          int num3 = index * 4;
          this.loan.SetField("CD3.X" + (object) (139 + num3), "");
          this.loan.SetField("CD3.X" + (object) (140 + num3), "");
          this.loan.SetField("CD3.X" + (object) (141 + num3), "");
          this.loan.SetField("CD3.X" + (object) (142 + num3), "");
        }
        for (int index = 0; index <= 15; ++index)
        {
          this.loan.SetField("CD3.X" + (object) (50 + index), "");
          this.loan.SetField("CD3.X" + (object) (65 + index), "");
        }
      }
      else
      {
        foreach (KeyValuePair<int, List<string>> keyValuePair in this.totalVolLiability)
        {
          if (keyValuePair.Key <= 20)
          {
            num1 = (keyValuePair.Key - 1) * 4;
            this.loan.SetField("CD3.X" + (object) (139 + num1), keyValuePair.Value[0]);
            this.loan.SetField("CD3.X" + (object) (140 + num1), keyValuePair.Value[1]);
            this.loan.SetField("CD3.X" + (object) (141 + num1), keyValuePair.Value[2]);
            this.loan.SetField("CD3.X" + (object) (142 + num1), this.mapLiabilityType(keyValuePair.Value[3]));
          }
          else
          {
            if (keyValuePair.Key == 21)
            {
              num1 = (keyValuePair.Key - 1) * 4;
              this.loan.SetField("CD3.X" + (object) (139 + num1), "Additional Liabilities");
              this.loan.SetField("CD3.X" + (object) (141 + num1), "Additional Liabilities");
              this.loan.SetField("CD3.X" + (object) (142 + num1), "Other");
            }
            num2 += Utils.ParseDecimal((object) keyValuePair.Value[1], 0.00M);
            this.loan.SetField("CD3.X" + (object) (140 + num1), num2.ToString());
          }
          if (keyValuePair.Key <= 15)
          {
            num1 = keyValuePair.Key - 1;
            this.loan.SetField("CD3.X" + (object) (50 + num1), keyValuePair.Value[0]);
            this.loan.SetField("CD3.X" + (object) (65 + num1), keyValuePair.Value[1]);
          }
        }
        foreach (KeyValuePair<int, List<string>> keyValuePair in this.totalNonVolLiability)
        {
          int num4 = keyValuePair.Key - 1;
          if (keyValuePair.Key <= 15)
          {
            this.loan.SetField("CD3.X" + (object) (50 + num4), keyValuePair.Value[0]);
            this.loan.SetField("CD3.X" + (object) (65 + num4), keyValuePair.Value[1]);
          }
          else
            break;
        }
        if (this.totalVolLiability.Count <= 21)
        {
          for (int count = this.totalVolLiability.Count; count <= 20; ++count)
          {
            int num5 = count * 4;
            this.loan.SetField("CD3.X" + (object) (139 + num5), "");
            this.loan.SetField("CD3.X" + (object) (140 + num5), "");
            this.loan.SetField("CD3.X" + (object) (141 + num5), "");
            this.loan.SetField("CD3.X" + (object) (142 + num5), "");
          }
        }
        if (this.totalVolLiability.Count + this.totalNonVolLiability.Count <= 15)
        {
          for (int index = this.totalVolLiability.Count + this.totalNonVolLiability.Count; index < 15; ++index)
          {
            this.loan.SetField("CD3.X" + (object) (50 + index), "");
            this.loan.SetField("CD3.X" + (object) (65 + index), "");
          }
        }
      }
      this.calculateAdjustmentsLineK04(id, val);
    }

    private void calculateTotalLiabilities()
    {
      int num1 = 1;
      this.totalVolLiability.Clear();
      int numberOfNonVols = this.loan.GetNumberOfNonVols();
      for (int index = 1; index <= numberOfNonVols; ++index)
      {
        string str = "UNFL" + index.ToString("00");
        if (this.Val(str + "02").Contains("exceeding legal limits P.O.C."))
        {
          double num2 = this.FltVal(str + "04");
          this.totalVolLiability.Add(num1++, new List<string>()
          {
            this.Val(str + "02"),
            num2.ToString(),
            this.Val(str + "09"),
            this.Val(str + "01")
          });
          break;
        }
      }
      BorrowerPair currentBorrowerPair = this.loan.CurrentBorrowerPair;
      foreach (BorrowerPair borrowerPair in this.GetBorrowerPairs())
      {
        this.loan.SetBorrowerPair(borrowerPair);
        int exlcudingAlimonyJobExp = this.loan.GetNumberOfLiabilitesExlcudingAlimonyJobExp();
        for (int index = 1; index <= exlcudingAlimonyJobExp; ++index)
        {
          string str = "FL" + index.ToString("00");
          if (this.Val(str + "63") == "Y")
          {
            double num3 = this.FltVal(str + "16");
            this.totalVolLiability.Add(num1++, new List<string>()
            {
              this.Val(str + "43"),
              num3.ToString(),
              this.Val(str + "02"),
              this.mapLiabilityType(this.Val(str + "62"))
            });
          }
        }
      }
      this.loan.SetBorrowerPair(currentBorrowerPair);
      this.totalNonVolLiability.Clear();
      for (int index = 1; index <= numberOfNonVols; ++index)
      {
        string str = "UNFL" + index.ToString("00");
        if (!(this.Val(str + "05") != "Y") && !this.Val(str + "02").Contains("exceeding legal limits P.O.C."))
        {
          double num4 = this.FltVal(str + "04");
          this.totalNonVolLiability.Add(num1++, new List<string>()
          {
            this.Val(str + "02"),
            num4.ToString(),
            this.Val(str + "09"),
            this.Val(str + "01")
          });
        }
      }
    }

    private string mapLiabilityType(string value)
    {
      if (this.fl0062Options == null || this.fl0062Options.Options == null)
        return value;
      for (int index = 0; index < this.fl0062Options.Options.Count; ++index)
      {
        if (value == this.fl0062Options.Options[index].Text)
          return this.fl0062Options.Options[index].Value;
      }
      return value;
    }

    private void aggrEscrowAcc_EscrowedCosts(string id, string val)
    {
      if (this.UseNewCompliance(18.3M) && this.Val("19") == "ConstructionToPermanent" && this.IntVal("1176") >= 12 && this.Val("HUD69") == "FirstAmortDate")
      {
        this.SetVal("HUD66", "0.00");
        this.SetVal("HUD67", "0.00");
      }
      else
      {
        double num1 = 0.0;
        if (this.Val("HUD41") != string.Empty)
          num1 = this.FltVal("HUD41");
        double num2 = 0.0;
        if (this.Val("HUD42") != string.Empty)
          num2 = this.FltVal("HUD42");
        double num3 = 0.0;
        if (this.Val("HUD43") != string.Empty)
          num3 = this.FltVal("HUD43");
        double num4 = 0.0;
        if (this.Val("HUD44") != string.Empty)
          num4 = this.FltVal("HUD44");
        double num5 = 0.0;
        if (this.Val("HUD45") != string.Empty)
          num5 = this.FltVal("HUD45");
        double num6 = 0.0;
        if (this.Val("HUD46") != string.Empty)
          num6 = this.FltVal("HUD46");
        double num7 = 0.0;
        if (this.Val("HUD47") != string.Empty)
          num7 = this.FltVal("HUD47");
        double num8 = 0.0;
        if (this.Val("HUD48") != string.Empty)
          num8 = this.FltVal("HUD48");
        double num9 = 0.0;
        if (this.Val("HUD50") != string.Empty)
          num9 = this.FltVal("HUD50");
        double num10 = 0.0;
        double num11 = this.FltVal("233") * 12.0;
        if (this.Val("NEWHUD.X337") == "Y")
          num10 += num1;
        else
          num11 += num1;
        if (this.Val("NEWHUD.X339") == "Y")
          num10 += num2;
        else
          num11 += num2;
        if (this.Val("NEWHUD.X338") == "Y")
          num10 += num4;
        else
          num11 += num4;
        if (this.Val("NEWHUD.X1726") == "Y")
          num10 += num5;
        else
          num11 += num5;
        if (this.Val("NEWHUD2.X124") == "Y" || this.Val("NEWHUD2.X125") == "Y" || this.Val("NEWHUD2.X126") == "Y")
        {
          if (this.Val("NEWHUD.X340") == "Y")
            num10 += num6;
          else
            num11 += num6;
        }
        if (this.Val("NEWHUD2.X127") == "Y" || this.Val("NEWHUD2.X128") == "Y" || this.Val("NEWHUD2.X129") == "Y")
        {
          if (this.Val("NEWHUD.X341") == "Y")
            num10 += num7;
          else
            num11 += num7;
        }
        if (this.Val("NEWHUD2.X130") == "Y" || this.Val("NEWHUD2.X131") == "Y" || this.Val("NEWHUD2.X132") == "Y")
        {
          if (this.Val("NEWHUD.X342") == "Y")
            num10 += num8;
          else
            num11 += num8;
        }
        if (this.UseNewCompliance(18.3M))
        {
          if (this.Val("NEWHUD.X1728") == "Y")
            num10 += num3;
          else
            num11 += num3;
          if (this.Val("NEWHUD.X343") == "Y")
            num10 += num9;
          else
            num11 += num9;
        }
        this.SetVal("HUD66", num10.ToString());
        this.SetVal("HUD67", num11.ToString());
      }
    }

    internal void CalcLoanTermTable()
    {
      this.calculateLoanTermTable_LoanAmount((string) null, (string) null);
      this.calculateLoanTermTable_InterestRate((string) null, (string) null);
      this.calculateLoanTermTable_LoanTerm((string) null, (string) null);
      this.calObjs.HMDACal.CalcLoanTerm((string) null, (string) null);
      this.calculateLoanTermTable_MonthlyPrincipalAndInterest((string) null, (string) null);
      this.calculateLoanTermTable_Prepayment((string) null, (string) null);
      this.populateLoanEstimate_LoanPurpose((string) null, (string) null);
    }

    internal void CalcEstimatedCashToCloseTable()
    {
      this.calculateLoanEstimate_TotalAdjustmentsCredit((string) null, (string) null);
      this.calculateLoanEstimate_ThirdPartyPaymentsNotOtherwiseDisclosed((string) null, (string) null);
      this.calculateLoanEstimate_ClosingCostsFinanced((string) null, (string) null);
      this.calculateLoanEstimate_DownPaymentAndFundsForBorrower((string) null, (string) null);
      this.calculateLoanEstimate_EstimatedCashToClose((string) null, (string) null);
    }

    internal void CalcEstimatedTaxesTable()
    {
      this.calculateLoanEstimate_EstimatedTaxes((string) null, (string) null);
      this.calculateLoanEstimate_TaxesInsuranceInEscrowOrNot((string) null, (string) null);
      this.calculateEstimatedPropertyCosts((string) null, (string) null);
      this.calculatePropertyCosts_OtherRow((string) null, (string) null);
      this.calculateWillHaveEscrowAccount((string) null, (string) null);
    }

    private void calculateLoanTermTable_LoanAmount(string id, string val)
    {
      if (this.Val("NEWHUD.X6") != "Y")
      {
        for (int index = 10; index <= 12; ++index)
          this.SetVal("LE1.X" + (object) index, "");
      }
      else
      {
        if (!(this.Val("LOANTERMTABLE.CUSTOMIZE") != "Y"))
          return;
        if (this.Val("LE1.X10") == string.Empty)
          this.SetVal("LE1.X10", "Can go");
        if (!(this.Val("LE1.X11") == string.Empty))
          return;
        this.SetVal("LE1.X11", "Can increase");
      }
    }

    private void calculateLoanTermTable_InterestRate(string id, string val)
    {
      bool flag1 = false;
      bool flag2 = this.Val("608") == "Fixed";
      int num1 = this.IntVal("1613");
      string str = this.Val("19");
      int num2 = this.IntVal("1176");
      int num3 = str == "ConstructionToPermanent" ? num2 + this.IntVal("696") : this.IntVal("696");
      bool flag3 = this.Val("CASASRN.X141") == "Borrower";
      bool flag4 = this.Val("COMPLIANCEVERSION.CASASRNX141") == "Y" || this.Val("COMPLIANCEVERSION.NEWBUYDOWNENABLED") != "Y";
      if (flag2 && this.Val("425") == "Y" && flag3 | flag4 && this.IntVal("1613") > 0 && this.FltVal("1269") > 0.0)
        flag1 = true;
      if (flag2 && !flag1 && str != "ConstructionToPermanent")
      {
        for (int index = 13; index <= 18; ++index)
          this.SetVal("LE1.X" + (object) index, "");
      }
      else
      {
        int num4 = flag1 ? num1 : (!flag2 ? this.IntVal("694") : 0);
        if (num4 > 0)
        {
          if (num4 < 24 && num4 != 12)
          {
            this.SetVal("LE1.X13", string.Concat((object) num4));
            this.SetVal("LE1.X14", "Months");
          }
          else if (num4 == 12)
          {
            this.SetVal("LE1.X13", "");
            this.SetVal("LE1.X14", "Year");
          }
          else if (num4 >= 24)
          {
            this.SetVal("LE1.X13", Utils.RemoveEndingZeros(string.Concat((object) Utils.ArithmeticRounding((double) num4 / 12.0, 2))));
            this.SetVal("LE1.X14", "Years");
          }
        }
        int num5 = flag1 ? num1 + 1 : (!flag2 ? num3 : 0);
        if (flag2 && str == "ConstructionToPermanent")
        {
          if (this.FltVal("1677") < this.FltVal("3"))
          {
            num5 = num2;
          }
          else
          {
            num5 = 0;
            this.SetVal("LE1.X15", "");
            this.SetVal("LE1.X16", "");
          }
        }
        if (num5 > 0)
        {
          if (num5 == 12 || num5 >= 24)
            this.SetVal("LE1.X15", "Year");
          else
            this.SetVal("LE1.X15", "Month");
          if (num5 == 12 || num5 >= 24)
          {
            if (num5 % 12 == 0)
              this.SetVal("LE1.X16", string.Concat((object) (num5 / 12 + 1)));
            else
              this.SetVal("LE1.X16", string.Concat((object) ((num5 + 12 - 1) / 12)));
          }
          else
            this.SetVal("LE1.X16", string.Concat((object) (num5 + 1)));
        }
        if (!(this.Val("LOANTERMTABLE.CUSTOMIZE") != "Y"))
          return;
        if (flag1)
        {
          if (!(this.Val("LE1.X23") == string.Empty))
            return;
          this.SetVal("LE1.X23", "Can go");
        }
        else if (this.Val("NEWHUD.X5") == "Y")
        {
          if (!(this.Val("LE1.X17") == string.Empty))
            return;
          this.SetVal("LE1.X17", "Can go");
        }
        else
          this.SetVal("LE1.X17", "");
      }
    }

    private void calculateLoan_TotalPayments(string id, string val)
    {
      string str1 = this.Val("19");
      string str2 = this.Val("1172");
      double num = this.FltVal("NEWHUD2.X2158");
      this.SetCurrentNum("CD5.X1", !this.UseNewCompliance(18.3M) ? (!(str1 == "ConstructionToPermanent") ? (!(str2 == "FarmersHomeAdministration") ? this.FltVal("4071") + this.FltVal("334") + this.FltVal("LE2.XSTD_DV") + this.FltVal("338") + this.FltVal("3971") + this.FltVal("NEWHUD2.X2082") - this.FltVal("NEWHUD2.X2111") + this.FltVal("NEWHUD2.X2115") - this.FltVal("NEWHUD2.X2144") + this.FltVal("NEWHUD2.X3567") - this.FltVal("NEWHUD2.X3596") + this.FltVal("NEWHUD2.X3600") - this.FltVal("NEWHUD2.X3629") : this.FltVal("4071") + this.FltVal("334") + this.FltVal("LE2.XSTD_DV") + this.FltVal("NEWHUD.X1708") + this.FltVal("NEWHUD2.X2082") - this.FltVal("NEWHUD2.X2111") + this.FltVal("NEWHUD2.X2115") - this.FltVal("NEWHUD2.X2144") + this.FltVal("NEWHUD2.X3567") - this.FltVal("NEWHUD2.X3596") + this.FltVal("NEWHUD2.X3600") - this.FltVal("NEWHUD2.X3629")) : this.FltVal("4071") + this.FltVal("334") + this.FltVal("CD2.XSTD") + this.FltVal("338") + this.FltVal("3971") + this.FltVal("NEWHUD.X1708")) + (this.Val("NEWHUD2.X2175") == "Y" ? this.FltVal("561") : 0.0) : (!(str2 == "FarmersHomeAdministration") ? (str1 == "ConstructionToPermanent" || str1 == "ConstructionOnly" ? this.FltVal("4071") + this.FltVal("CD2.XSTD") + num + this.FltVal("3971") + this.FltVal("NEWHUD2.X2587") + this.FltVal("NEWHUD2.X4670") + this.FltVal("NEWHUD2.X4693") + this.FltVal("NEWHUD2.X4716") + this.FltVal("NEWHUD2.X4739") : this.FltVal("4071") + this.FltVal("CD2.XSTD") + num + this.FltVal("3971") + this.FltVal("NEWHUD2.X2587")) : this.FltVal("4071") + this.FltVal("CD2.XSTD") + num + this.FltVal("NEWHUD2.X2818")), true);
    }

    private void calculatePrepaidMI(string id, string val)
    {
      double num = 0.0;
      switch (this.Val("1757"))
      {
        case "Loan Amount":
          num = this.FltVal("2");
          break;
        case "Purchase Price":
          num = this.FltVal("136");
          break;
        case "Appraisal Value":
          num = this.FltVal("356");
          break;
        case "Base Loan Amount":
          num = this.FltVal("1109");
          break;
      }
      this.SetCurrentNum("3971", Utils.ArithmeticRounding(Utils.ArithmeticRounding(this.FltVal("1199") / 100.0 * num / 12.0, 2) * (double) this.IntVal("1209"), 2));
    }

    internal void CalcLoanTotalPayments()
    {
      this.calculateLoan_TotalPayments((string) null, (string) null);
    }

    internal void CalcInterestOnlyPayments()
    {
      this.calculateAPTable_InterestOnlyPayments((string) null, (string) null);
    }

    private void calculateAPTable_InterestOnlyPayments(string id, string val)
    {
      string str = this.Val("19");
      int num1 = this.IntVal("4");
      int num2 = this.IntVal("325");
      int num3 = this.IntVal("1177");
      if (num3 <= 0)
      {
        switch (str)
        {
          case "ConstructionToPermanent":
            goto label_2;
          case "ConstructionOnly":
            if (this.Val("SYS.X6") == "A")
            {
              this.SetVal("CD4.X23", "Y");
              return;
            }
            break;
        }
        this.SetVal("CD4.X23", "N");
        return;
      }
label_2:
      if (!str.Contains("Construction") && num3 > 0 && num1 > num2 && (num3 == num2 || num3 == num2 - 1))
      {
        this.SetVal("CD4.X23", "");
        this.SetVal("CD4.X25", "");
        this.SetVal("CD4.X27", "");
      }
      else
      {
        if (this.Val("608") == "Fixed")
        {
          this.SetVal("CD4.X25", "N");
          this.SetVal("CD4.X27", "N");
        }
        this.SetVal("CD4.X23", "Y");
      }
    }

    private void calculateAPTable_FirstChangePayment(string id, string val)
    {
      this.calObjs.NewHudCal.CalculateProjectedPaymentTable(true);
    }

    private void calculateAPTable_MaximumPayment(string id, string val)
    {
      this.calObjs.NewHudCal.CalculateProjectedPaymentTable(true);
    }

    internal void CalcSubsequentChanges()
    {
      this.calculateAPTable_SubsequentChanges((string) null, (string) null);
    }

    private void calculateAPTable_SubsequentChanges(string id, string val)
    {
      bool flag = this.Val("CD4.X25") == "Y" || this.Val("CD4.X27") == "Y" || this.Val("NEWHUD.X6") == "Y" || this.Val("CD4.X25") != "Y" && this.Val("CD4.X27") != "Y" && this.Val("NEWHUD.X6") != "Y" && this.Val("CD4.X23") != "Y";
      int num1 = this.IntVal("4");
      int num2 = this.IntVal("325");
      int num3 = this.IntVal("1177");
      string str1 = this.Val("19");
      int month = this.IntVal("694");
      string str2 = this.Val("608");
      if (str1.Contains("Construction") && this.Val("SYS.X6") == "A")
        this.SetVal("CD4.X33", "Every Payment");
      else if (!str1.Contains("Construction") && num3 > 0 && num1 > num2 && (num3 == num2 || num3 == num2 - 1))
      {
        this.SetVal("CD4.X33", "");
      }
      else
      {
        if (flag)
          return;
        if (str2 == "Fixed")
        {
          if (this.Val("19") == "ConstructionToPermanent" || this.IntVal("1177") > 0)
            this.SetVal("CD4.X33", "No subsequent changes");
          else
            this.SetVal("CD4.X33", "");
        }
        else if (str2 == "AdjustableRate" && month == 12)
          this.SetVal("CD4.X33", "Every year");
        else if (str2 == "AdjustableRate" && month != 12 && month % 12 == 0 && month != 0)
          this.SetVal("CD4.X33", "Every " + (object) (month / 12) + " years");
        else if (str2 == "AdjustableRate" && month < 12 && month != 0)
          this.SetVal("CD4.X33", "Every " + (object) month + Utils.GetMonthSuffix(month) + " payment");
        else if (str2 == "AdjustableRate" && month > 12)
          this.SetVal("CD4.X33", "Every " + Utils.RemoveEndingZeros(((Decimal) month / 12M).ToString("#.##")) + " years");
        else
          this.SetVal("CD4.X33", "");
      }
    }

    private void calculateLoanTermTable_MonthlyPrincipalAndInterest(string id, string val)
    {
      if (this.Val("NEWHUD.X8") == "Y")
      {
        if (this.Val("LE1.X23") == string.Empty)
          this.SetVal("LE1.X23", "Can go");
        bool flag1 = this.Val("608") == "Fixed";
        int num1 = this.IntVal("696");
        bool flag2 = false;
        this.IntVal("1613");
        string str1 = this.Val("19");
        string str2 = this.Val("SYS.X6");
        int num2 = str1.StartsWith("Construction") ? (str1.Equals("ConstructionToPermanent") ? this.IntVal("1176") : this.IntVal("1176") + this.IntVal("1177")) : this.IntVal("1177");
        bool flag3 = this.Val("CASASRN.X141") == "Borrower";
        bool flag4 = this.Val("COMPLIANCEVERSION.CASASRNX141") == "Y" || this.Val("COMPLIANCEVERSION.NEWBUYDOWNENABLED") != "Y";
        if (flag1 && this.Val("425") == "Y" && flag3 | flag4 && this.IntVal("1613") > 0 && this.FltVal("1269") > 0.0)
          flag2 = true;
        if (str1.StartsWith("Construction") && str2 != "B")
        {
          this.SetVal("LE1.X19", "");
          this.SetVal("LE1.X20", "Month");
        }
        else if (flag2 || !flag1 && (num2 == 0 || num2 >= num1 || str1 == "ConstructionToPermanent"))
        {
          this.SetVal("LE1.X19", this.IsLocked("LE1.X13") ? this.ValOrg("LE1.X13") : this.Val("LE1.X13"));
          this.SetVal("LE1.X20", this.IsLocked("LE1.X14") ? this.ValOrg("LE1.X14") : this.Val("LE1.X14"));
        }
        else
        {
          this.SetVal("LE1.X19", "");
          this.SetVal("LE1.X20", "");
        }
        if (str1.StartsWith("Construction") && str2 != "B")
        {
          this.SetVal("LE1.X21", "Month");
          this.SetVal("LE1.X22", "1");
        }
        else if (flag1 && num2 > 0 || num2 > 0 && num1 > 0 && num2 < num1)
        {
          if (num2 == 12 || num2 >= 24)
            this.SetVal("LE1.X21", "Year");
          else
            this.SetVal("LE1.X21", "Month");
          if (num2 == 12 || num2 >= 24)
          {
            if (num2 % 12 == 0)
              this.SetVal("LE1.X22", string.Concat((object) (num2 / 12 + 1)));
            else
              this.SetVal("LE1.X22", string.Concat((object) ((num2 + 12 - 1) / 12)));
          }
          else
            this.SetVal("LE1.X22", string.Concat((object) (num2 + 1)));
        }
        else if (str1 == "ConstructionToPermanent" & flag1 && this.FltVal("1677") < this.FltVal("3"))
        {
          int val1 = this.IntVal("1176");
          int num3 = str1 == "ConstructionToPermanent" ? val1 + this.IntVal("696") : this.IntVal("696");
          if (val1 < 12 || val1 < 24 && val1 != 12)
          {
            this.SetVal("LE1.X21", "Month");
            this.SetVal("LE1.X22", string.Concat((object) (val1 + 1)));
          }
          else if (val1 >= 24)
          {
            this.SetVal("LE1.X21", "Year");
            this.SetVal("LE1.X22", string.Concat((object) Utils.ArithmeticRoundingUp(val1, 12)));
          }
          else if (val1 == 12)
          {
            this.SetVal("LE1.X21", "Year");
            this.SetVal("LE1.X22", "2");
          }
        }
        else if (flag2 || !flag1 && (num2 == 0 || num2 >= num1))
        {
          if (str1 == "ConstructionToPermanent")
          {
            int val2 = this.IntVal("1176");
            if (val2 < 12 || val2 < 24 && val2 != 12)
            {
              this.SetVal("LE1.X21", "Month");
              this.SetVal("LE1.X22", string.Concat((object) (val2 + 1)));
            }
            else if (val2 > 24 && val2 % 12 != 0)
            {
              this.SetVal("LE1.X21", "Year");
              this.SetVal("LE1.X22", string.Concat((object) Utils.ArithmeticRoundingUp(val2, 12)));
            }
            else if (val2 % 12 == 0)
            {
              this.SetVal("LE1.X21", "Year");
              this.SetVal("LE1.X22", string.Concat((object) (val2 / 12 + 1)));
            }
          }
          else
          {
            this.SetVal("LE1.X21", this.IsLocked("LE1.X15") ? this.ValOrg("LE1.X15") : this.Val("LE1.X15"));
            this.SetVal("LE1.X22", this.IsLocked("LE1.X16") ? this.ValOrg("LE1.X16") : this.Val("LE1.X16"));
          }
        }
        if (num2 == 0)
        {
          this.SetVal("LE1.X26", "");
          this.SetVal("LE1.X89", "");
        }
        else if (str1 == "ConstructionOnly" && this.Val("SYS.X6") != "B")
        {
          if (num2 < 24)
          {
            this.SetVal("LE1.X26", string.Concat((object) num2));
            this.SetVal("LE1.X89", "Month");
          }
          else
          {
            this.SetVal("LE1.X26", string.Concat((object) (num2 / 12)));
            this.SetVal("LE1.X89", "Year");
          }
        }
        else if (num2 > 0 && num2 < 24 && num2 != 12)
        {
          this.SetVal("LE1.X26", string.Concat((object) (num2 + 1)));
          this.SetVal("LE1.X89", "Month");
        }
        else if (num2 % 12 == 0)
        {
          this.SetVal("LE1.X26", string.Concat((object) (num2 / 12 + 1)));
          this.SetVal("LE1.X89", "Year");
        }
        else
        {
          this.SetVal("LE1.X26", string.Concat((object) ((num2 + 12 - 1) / 12)));
          this.SetVal("LE1.X89", "Year");
        }
      }
      else
      {
        for (int index = 19; index <= 26; ++index)
          this.SetVal("LE1.X" + (object) index, "");
        this.SetVal("LE1.X88", "");
        this.SetVal("LE1.X89", "");
      }
    }

    private void calculateLoanTermTable_Prepayment(string id, string val)
    {
      if (this.Val("675") != "Y")
      {
        this.SetVal("LE1.X27", "");
        this.SetVal("LE1.X91", "");
        this.SetVal("LE1.XD27", "");
      }
      else
      {
        int num = this.IntVal("RE88395.X316");
        switch (num)
        {
          case 0:
            this.SetVal("LE1.X27", "");
            this.SetVal("LE1.X91", "");
            this.SetVal("LE1.XD27", "");
            break;
          case 12:
            this.SetCurrentNum("LE1.X27", 1.0);
            this.SetVal("LE1.X91", "Year");
            this.SetVal("LE1.XD27", "First");
            break;
          default:
            if (num < 24 && num != 12)
            {
              this.SetCurrentNum("LE1.X27", (double) num);
              this.SetVal("LE1.X91", "Months");
              this.SetVal("LE1.XD27", string.Concat((object) num));
              break;
            }
            this.SetCurrentNum("LE1.X27", num == 0 ? 0.0 : (double) ((num + 12 - 1) / 12));
            this.SetVal("LE1.X91", "Years");
            this.SetVal("LE1.XD27", this.Val("LE1.X27"));
            break;
        }
      }
    }

    private void calculateLoanTermTable_LoanTerm(string id, string val)
    {
      int num1 = Utils.ParseInt((object) this.Val("4"));
      int num2 = Utils.ParseInt((object) this.Val("325"));
      if (num2 > 0 && num2 < num1)
        num1 = num2;
      if (this.Val("19") != "ConstructionToPermanent")
        this.SetVal("CONST.X1", "");
      else if (this.Val("CONST.X1") != "Y" && this.Val("19") == "ConstructionToPermanent")
        num1 += this.IntVal("1176");
      if (num1 <= 0 && num2 <= 0)
      {
        this.SetVal("LE1.X2", "");
        this.SetVal("LE1.X3", "");
      }
      else if (this.Val("423") == "Biweekly")
      {
        int num3 = this.IntVal("1701");
        int num4 = (num3 - num3 % 26) / 26;
        int num5 = num3 % 26 > 0 ? (int) ((Decimal) (num3 % 26) / 26M * 10M + 1M) : 0;
        this.SetVal("LE1.X2", num4 != 0 ? num4.ToString() : "");
        this.SetVal("LE1.X3", num5 != 0 ? num5.ToString() : "");
      }
      else if (num1 < 24)
      {
        if (num1 == 12)
        {
          this.SetVal("LE1.X2", "1");
          this.SetVal("LE1.X3", "");
        }
        else
        {
          this.SetVal("LE1.X2", "");
          this.SetVal("LE1.X3", num1.ToString());
        }
      }
      else
      {
        int num6 = num1 / 12;
        this.SetVal("LE1.X2", num6.ToString());
        if (num1 % 12 == 0)
        {
          this.SetVal("LE1.X3", "");
        }
        else
        {
          num6 = num1 % 12;
          this.SetVal("LE1.X3", num6.ToString());
        }
      }
    }

    internal void CalcLoanTypeOtherField()
    {
      this.populateLoanType_OtherField((string) null, (string) null);
    }

    private void populateLoanType_OtherField(string id, string val)
    {
      string str = this.Val("1172");
      if (str == "FarmersHomeAdministration")
      {
        this.SetVal("1063", "RHS");
      }
      else
      {
        if (!(str != "Other"))
          return;
        this.SetVal("1063", "");
      }
    }

    private void populateLoanEstimate_LoanPurpose(string id, string val)
    {
      string str1 = this.Val("19");
      int exlcudingAlimonyJobExp = this.loan.GetNumberOfLiabilitesExlcudingAlimonyJobExp();
      bool flag = false;
      for (int index = 1; index <= exlcudingAlimonyJobExp; ++index)
      {
        string str2 = index.ToString("00");
        if (this.Val("FL" + str2 + "18") == "Y" && this.Val("FL" + str2 + "61") == "Y")
        {
          flag = true;
          break;
        }
      }
      if (str1 == "Purchase")
        this.SetVal("LE1.X4", "Purchase");
      else if ((str1 == "Cash-Out Refinance" || str1 == "NoCash-Out Refinance") && this.FltVal("1092") == 0.0 && this.FltVal("26") == 0.0)
        this.SetVal("LE1.X4", "Home Equity Loan");
      else if (((!(str1 == "Cash-Out Refinance") || this.FltVal("1092") <= 0.0 ? 0 : (this.FltVal("26") == 0.0 ? 1 : 0)) & (flag ? 1 : 0)) != 0)
        this.SetVal("LE1.X4", "Home Equity Loan");
      else if (str1 == "Cash-Out Refinance" && this.Val("420") == "SecondLien" && this.FltVal("1092") < this.FltVal("26"))
        this.SetVal("LE1.X4", "Home Equity Loan");
      else if (str1 == "NoCash-Out Refinance" || str1 == "Cash-Out Refinance")
        this.SetVal("LE1.X4", "Refinance");
      else if ((str1 == "ConstructionOnly" || str1 == "ConstructionToPermanent") && this.Val("1964") == "Y")
        this.SetVal("LE1.X4", "Purchase");
      else if ((str1 == "ConstructionOnly" || str1 == "ConstructionToPermanent") && this.Val("1964") != "Y" && this.Val("Constr.Refi") == "Y" && this.Val("5015") != "Y")
        this.SetVal("LE1.X4", "Refinance");
      else if ((str1 == "ConstructionOnly" || str1 == "ConstructionToPermanent") && this.Val("1964") != "Y" && this.Val("Constr.Refi") != "Y" && this.Val("5015") == "Y")
        this.SetVal("LE1.X4", "Construction");
      else if (str1 == "ConstructionOnly" || str1 == "ConstructionToPermanent")
        this.SetVal("LE1.X4", "Construction");
      else if (str1 == "Other")
        this.SetVal("LE1.X4", "Home Equity Loan");
      else
        this.SetVal("LE1.X4", "");
    }

    private void calculateLotOwnedFreeAndClearIndicator(string id, string val)
    {
      if (!(this.Val("Constr.Refi") != "Y"))
        return;
      this.SetVal("5015", "");
    }

    private void calculateLoanEstimate_ClosingCostExpDate(string id, string val)
    {
      DateTime closingCostExpirationDate = DateTime.MinValue;
      string str = this.Val("3164");
      if (id == "LE1.X1" || id == "LE1.X28" || id == "3164")
      {
        if (id == "LE1.X1" || id == "3164")
        {
          DateTime date = id == "3164" ? Utils.ParseDate((object) this.Val("LE1.X1")) : Utils.ParseDate((object) val);
          if (date != DateTime.MinValue && this.closingCostDaysToExpire != 0)
          {
            DateTime dateTime = this.sessionObjects.ConfigurationManager.AddBusinessDays(CalendarType.Business, date, this.closingCostDaysToExpire, false);
            if (str != "Y")
            {
              this.SetVal("LE1.X28", dateTime.ToString("MM/dd/yyyy"));
              this.SetVal("LE1.XD28", this.Val("LE1.X28"));
            }
          }
          else if (str != "Y")
            this.SetVal("LE1.X28", "//");
          closingCostExpirationDate = Utils.ParseDate((object) this.Val("LE1.X28"));
        }
        else if (id == "LE1.X28")
          closingCostExpirationDate = Utils.ParseDate((object) val);
        this.SetVal("LE1.X9", this.GetClosingCostEstimateExpirationTimeZone(closingCostExpirationDate));
        if (str != "Y")
          this.SetVal("LE1.XD9", this.Val("LE1.X9"));
      }
      if (!(id == "762") && !(id == "761") && !(id == "432"))
        return;
      DateTime dateTime1 = id == "761" || id == "432" ? Utils.ParseDate((object) this.Val("762")) : Utils.ParseDate((object) val);
      this.SetVal("LE1.X7", Utils.TransformSettingTimezoneToStandardTimezone(this.rateLockExpirationTimeZoneSetting, dateTime1 != DateTime.MinValue && System.TimeZoneInfo.Local.IsDaylightSavingTime(dateTime1)));
    }

    private void calculateLoanEstimate_AIRMonthSuffix(string id, string val)
    {
      switch (id)
      {
        case "696":
          if (!string.IsNullOrEmpty(this.Val("696")))
          {
            if (this.Val("19") == "ConstructionToPermanent" && this.Val("608") == "AdjustableRate")
            {
              int num = string.IsNullOrEmpty(this.Val("1176")) ? 0 : Utils.ParseInt((object) this.Val("1176"));
              this.SetVal("LE2.X99", (Utils.ParseInt((object) this.Val("696")) + num + 1).ToString());
            }
            else
              this.SetVal("LE2.X99", (Utils.ParseInt((object) this.Val("696")) + 1).ToString());
            this.SetVal("LE2.X97", Utils.GetMonthSuffix(Utils.ParseInt((object) this.Val("LE2.X99"))));
            break;
          }
          this.SetVal("LE2.X99", "");
          this.SetVal("LE2.X97", "");
          break;
        case "694":
          if (!string.IsNullOrEmpty(this.Val("694")))
          {
            this.SetVal("LE2.X98", Utils.GetMonthSuffix(Utils.ParseInt((object) this.Val("694"))));
            break;
          }
          this.SetVal("LE2.X98", "");
          break;
        default:
          if (!string.IsNullOrEmpty(this.Val("LE2.X99")))
          {
            this.SetVal("LE2.X97", Utils.GetMonthSuffix(Utils.ParseInt((object) this.Val("LE2.X99"))));
            break;
          }
          this.SetVal("LE2.X97", "");
          break;
      }
    }

    private void populateLoanEstimate_LenderBrokerInfo(string id, string val)
    {
      if (this.Val("2626") == "Banked - Retail")
      {
        this.SetVal("LE3.X1", this.Val("3032"));
        this.SetVal("LE3.X2", this.Val("3968"));
        this.SetVal("LE3.X3", this.Val("1823"));
        this.SetVal("LE3.X4", this.Val("1612"));
        this.SetVal("LE3.X5", this.Val("3238"));
        this.SetVal("LE3.X24", this.Val("2306"));
        this.SetVal("LE3.X6", "");
        this.SetVal("LE3.X7", "");
        this.SetVal("LE3.X8", "");
        this.SetVal("LE3.X9", "");
        this.SetVal("LE3.X10", "");
        this.SetVal("LE3.X25", "");
      }
      else
      {
        if (!(this.Val("2626") == "Banked - Wholesale") && !(this.Val("2626") == "Brokered"))
          return;
        this.SetVal("LE3.X2", "");
        this.SetVal("LE3.X3", "");
        this.SetVal("LE3.X4", "");
        this.SetVal("LE3.X5", "");
        this.SetVal("LE3.X24", "");
        this.SetVal("LE3.X6", this.Val("VEND.X300"));
        this.SetVal("LE3.X7", this.Val("3968"));
        this.SetVal("LE3.X8", this.Val("1823"));
        this.SetVal("LE3.X9", this.Val("1612"));
        this.SetVal("LE3.X10", this.Val("3238"));
        this.SetVal("LE3.X25", this.Val("2306"));
      }
    }

    private void calculateClosingDisclosurePage5(string id, string val)
    {
      if (!(id == "1969") || !(this.Val("1969") == "Y"))
        return;
      this.Copy1033ToLender(true);
    }

    internal void CopyBusinessToFileContact(string contactType)
    {
      switch (contactType)
      {
        case "RealEstateBroker(B)":
          this.SetVal("VEND.X133", this.Val("CD5.X31"));
          this.SetVal("VEND.X134", this.Val("CD5.X32"));
          this.SetVal("VEND.X135", this.Val("CD5.X33"));
          this.SetVal("VEND.X136", this.Val("CD5.X34"));
          this.SetVal("VEND.X137", this.Val("CD5.X35"));
          this.SetVal("VEND.X936", this.Val("CD5.X37"));
          this.SetVal("VEND.X139", this.Val("CD5.X38"));
          this.SetVal("VEND.X739", this.Val("CD5.X40"));
          this.SetVal("VEND.X140", this.Val("CD5.X42"));
          this.SetVal("VEND.X141", this.Val("CD5.X41"));
          break;
        case "RealEstateBroker(S)":
          this.SetVal("VEND.X144", this.Val("CD5.X43"));
          this.SetVal("VEND.X145", this.Val("CD5.X44"));
          this.SetVal("VEND.X146", this.Val("CD5.X45"));
          this.SetVal("VEND.X147", this.Val("CD5.X46"));
          this.SetVal("VEND.X148", this.Val("CD5.X47"));
          this.SetVal("VEND.X937", this.Val("CD5.X49"));
          this.SetVal("VEND.X150", this.Val("CD5.X50"));
          this.SetVal("VEND.X928", this.Val("CD5.X52"));
          this.SetVal("VEND.X151", this.Val("CD5.X54"));
          this.SetVal("VEND.X152", this.Val("CD5.X53"));
          break;
        case "SettlementAgent":
          this.SetVal("VEND.X655", this.Val("CD5.X55"));
          this.SetVal("VEND.X656", this.Val("CD5.X56"));
          this.SetVal("VEND.X657", this.Val("CD5.X57"));
          this.SetVal("VEND.X658", this.Val("CD5.X58"));
          this.SetVal("VEND.X659", this.Val("CD5.X59"));
          this.SetVal("VEND.X662", this.Val("CD5.X60"));
          this.SetVal("VEND.X663", this.Val("CD5.X61"));
          this.SetVal("VEND.X668", this.Val("CD5.X62"));
          this.SetVal("VEND.X669", this.Val("CD5.X66"));
          this.SetVal("VEND.X670", this.Val("CD5.X65"));
          this.SetVal("VEND.X675", this.Val("CD5.X63"));
          this.SetVal("VEND.X676", this.Val("CD5.X64"));
          break;
      }
    }

    private void syncFileContactToCD5(string id, string val)
    {
      if ((id == "VEND.X993" || id == "VEND.X135" || id == "VEND.X136" || id == "VEND.X137") && this.Val("VEND.X993") == "Y")
      {
        this.SetVal("CD5.X31", this.Val("VEND.X133"));
        this.SetVal("CD5.X32", this.Val("VEND.X134"));
        this.SetVal("CD5.X33", this.Val("VEND.X135"));
        this.SetVal("CD5.X34", this.Val("VEND.X136"));
        this.SetVal("CD5.X35", this.Val("VEND.X137"));
        this.SetVal("CD5.X37", this.Val("VEND.X936"));
        this.SetVal("CD5.X38", this.Val("VEND.X139"));
        this.SetVal("CD5.X40", this.Val("VEND.X739"));
        this.SetVal("CD5.X41", this.Val("VEND.X141"));
        this.SetVal("CD5.X42", this.Val("VEND.X140"));
      }
      else if ((id == "VEND.X994" || id == "VEND.X146" || id == "VEND.X147" || id == "VEND.X148") && this.Val("VEND.X994") == "Y")
      {
        this.SetVal("CD5.X43", this.Val("VEND.X144"));
        this.SetVal("CD5.X44", this.Val("VEND.X145"));
        this.SetVal("CD5.X45", this.Val("VEND.X146"));
        this.SetVal("CD5.X46", this.Val("VEND.X147"));
        this.SetVal("CD5.X47", this.Val("VEND.X148"));
        this.SetVal("CD5.X49", this.Val("VEND.X937"));
        this.SetVal("CD5.X50", this.Val("VEND.X150"));
        this.SetVal("CD5.X52", this.Val("VEND.X928"));
        this.SetVal("CD5.X53", this.Val("VEND.X152"));
        this.SetVal("CD5.X54", this.Val("VEND.X151"));
      }
      else
      {
        if (!(id == "VEND.X654") && !(id == "VEND.X657") && !(id == "VEND.X658") && !(id == "VEND.X659") || !(this.Val("VEND.X654") == "Y"))
          return;
        this.SetVal("CD5.X55", this.Val("VEND.X655"));
        this.SetVal("CD5.X56", this.Val("VEND.X656"));
        this.SetVal("CD5.X57", this.Val("VEND.X657"));
        this.SetVal("CD5.X58", this.Val("VEND.X658"));
        this.SetVal("CD5.X59", this.Val("VEND.X659"));
        this.SetVal("CD5.X60", this.Val("VEND.X662"));
        this.SetVal("CD5.X61", this.Val("VEND.X663"));
        this.SetVal("CD5.X62", this.Val("VEND.X668"));
        this.SetVal("CD5.X63", this.Val("VEND.X675"));
        this.SetVal("CD5.X64", this.Val("VEND.X676"));
        this.SetVal("CD5.X65", this.Val("VEND.X670"));
        this.SetVal("CD5.X66", this.Val("VEND.X669"));
      }
    }

    internal void Copy1033ToLender(bool copyIfBlank)
    {
      bool flag = true;
      if (copyIfBlank)
      {
        if (this.Val("CD5.X7") != string.Empty)
          flag = false;
        else if (this.Val("CD5.X8") != string.Empty)
          flag = false;
        else if (this.Val("CD5.X9") != string.Empty)
          flag = false;
        else if (this.Val("CD5.X10") != string.Empty)
          flag = false;
        else if (this.Val("CD5.X11") != string.Empty)
          flag = false;
        else if (this.Val("CD5.X12") != string.Empty)
          flag = false;
        else if (this.Val("CD5.X13") != string.Empty)
          flag = false;
        else if (this.Val("CD5.X14") != string.Empty)
          flag = false;
        else if (this.Val("CD5.X15") != string.Empty)
          flag = false;
        else if (this.Val("CD5.X16") != string.Empty)
          flag = false;
        else if (this.Val("CD5.X17") != string.Empty)
          flag = false;
        else if (this.Val("CD5.X18") != string.Empty)
          flag = false;
      }
      if (copyIfBlank && !flag)
        return;
      this.SetVal("CD5.X7", this.Val("315"));
      this.SetVal("CD5.X8", this.Val("319"));
      this.SetVal("CD5.X9", this.Val("313"));
      this.SetVal("CD5.X10", this.Val("321"));
      this.SetVal("CD5.X11", this.Val("323"));
      this.SetVal("CD5.X12", this.Val("3237"));
      this.SetVal("CD5.X13", this.Val("3629"));
      this.SetVal("CD5.X14", this.Val("1612"));
      this.SetVal("CD5.X15", this.Val("3238"));
      this.SetVal("CD5.X16", this.Val("2306"));
      this.SetVal("CD5.X17", this.Val("3968"));
      this.SetVal("CD5.X18", this.Val("1823"));
    }

    private void calculateLineItem701(string id, string val)
    {
      if (!this.UseNew2015GFEHUD)
        return;
      this.SetCurrentNum("L211", this.FltVal("NEWHUD2.X1") + this.FltVal("NEWHUD2.X2"));
    }

    private void calculateLineItem702(string id, string val)
    {
      if (!this.UseNew2015GFEHUD)
        return;
      this.SetCurrentNum("L213", this.FltVal("NEWHUD2.X3") + this.FltVal("NEWHUD2.X4"));
    }

    private void calculateSection700(string id, string val)
    {
      this.SetCurrentNum("NEWHUD2.X5", this.FltVal("NEWHUD2.X1") + this.FltVal("NEWHUD2.X3"));
      this.SetCurrentNum("NEWHUD2.X6", this.FltVal("NEWHUD2.X2") + this.FltVal("NEWHUD2.X4"));
    }

    private void calculateClosingCostSubTotal(string id, string val)
    {
      double num1 = 0.0;
      double num2 = 0.0;
      double num3 = 0.0;
      string[] strArray = (string[]) null;
      for (int index = 0; index < HUDGFE2010Fields.WHOLEPOC_FIELDS.Count; ++index)
      {
        strArray = HUDGFE2010Fields.WHOLEPOC_FIELDS[index];
        if ((!(strArray[HUDGFE2010Fields.PTCPOCINDEX_LINENUMBER] == "1102") || !(strArray[HUDGFE2010Fields.PTCPOCINDEX_COMPONENTID] == "")) && (!(strArray[HUDGFE2010Fields.PTCPOCINDEX_LINENUMBER] == "0801") || !(strArray[HUDGFE2010Fields.PTCPOCINDEX_COMPONENTID] == "x")) && !(strArray[HUDGFE2010Fields.PTCPOCINDEX_BORPAID] == ""))
        {
          if (strArray[HUDGFE2010Fields.PTCPOCINDEX_BORPAID] == "NEWHUD2.X1" || strArray[HUDGFE2010Fields.PTCPOCINDEX_BORPAID] == "454" || strArray[HUDGFE2010Fields.PTCPOCINDEX_BORPAID] == "334" || strArray[HUDGFE2010Fields.PTCPOCINDEX_BORPAID] == "656" || strArray[HUDGFE2010Fields.PTCPOCINDEX_BORPAID] == "NEWHUD.X952" || strArray[HUDGFE2010Fields.PTCPOCINDEX_BORPAID] == "390" || strArray[HUDGFE2010Fields.PTCPOCINDEX_BORPAID] == "NEWHUD.X254" || strArray[HUDGFE2010Fields.PTCPOCINDEX_BORPAID] == "NEWHUD2.X4662")
          {
            if (strArray[HUDGFE2010Fields.PTCPOCINDEX_BORPAID] == "454")
            {
              this.SetCurrentNum("NEWHUD2.X24", num1);
              this.SetCurrentNum("NEWHUD2.X25", num2);
              this.SetCurrentNum("NEWHUD2.X26", num3);
            }
            else if (strArray[HUDGFE2010Fields.PTCPOCINDEX_BORPAID] == "334")
            {
              this.SetCurrentNum("NEWHUD2.X28", num1 - (this.FltVal("NEWHUD.X1142") + this.FltVal("NEWHUD.X1144") + this.FltVal("NEWHUD.X1146") + this.FltVal("NEWHUD.X1148")));
              this.SetCurrentNum("NEWHUD2.X29", num2);
              this.SetCurrentNum("NEWHUD2.X30", num3);
            }
            else if (strArray[HUDGFE2010Fields.PTCPOCINDEX_BORPAID] == "656")
            {
              this.SetCurrentNum("NEWHUD2.X32", num1);
              this.SetCurrentNum("NEWHUD2.X33", num2);
              this.SetCurrentNum("NEWHUD2.X34", num3);
            }
            else if (strArray[HUDGFE2010Fields.PTCPOCINDEX_BORPAID] == "NEWHUD.X952")
            {
              this.SetCurrentNum("NEWHUD2.X36", num1);
              this.SetCurrentNum("NEWHUD2.X37", num2);
              this.SetCurrentNum("NEWHUD2.X38", num3);
            }
            else if (strArray[HUDGFE2010Fields.PTCPOCINDEX_BORPAID] == "390")
            {
              this.SetCurrentNum("NEWHUD2.X40", num1);
              this.SetCurrentNum("NEWHUD2.X41", num2);
              this.SetCurrentNum("NEWHUD2.X42", num3);
            }
            else if (strArray[HUDGFE2010Fields.PTCPOCINDEX_BORPAID] == "NEWHUD.X254")
            {
              this.SetCurrentNum("NEWHUD2.X44", num1);
              this.SetCurrentNum("NEWHUD2.X45", num2);
              this.SetCurrentNum("NEWHUD2.X46", num3);
            }
            else if (strArray[HUDGFE2010Fields.PTCPOCINDEX_BORPAID] == "NEWHUD2.X4662")
            {
              this.SetCurrentNum("NEWHUD2.X48", num1);
              this.SetCurrentNum("NEWHUD2.X49", num2);
              this.SetCurrentNum("NEWHUD2.X50", num3);
            }
            double num4;
            num3 = num4 = 0.0;
            num2 = num4;
            num1 = num4;
          }
          if (!(strArray[HUDGFE2010Fields.PTCPOCINDEX_BORPAID] == "NEWHUD.X571") && !(strArray[HUDGFE2010Fields.PTCPOCINDEX_BORPAID] == "NEWHUD.X607") && !(strArray[HUDGFE2010Fields.PTCPOCINDEX_BORPAID] == "NEWHUD.X603") && !(strArray[HUDGFE2010Fields.PTCPOCINDEX_BORPAID] == "NEWHUD.X1149") && !(strArray[HUDGFE2010Fields.PTCPOCINDEX_BORPAID] == "NEWHUD.X1165") && !(strArray[HUDGFE2010Fields.PTCPOCINDEX_BORPAID] == "NEWHUD.X15"))
          {
            if ((strArray[HUDGFE2010Fields.PTCPOCINDEX_LINENUMBER] == "0701" || strArray[HUDGFE2010Fields.PTCPOCINDEX_LINENUMBER] == "0702") && this.IsLocked("NEWHUD2.X5"))
            {
              if (strArray[HUDGFE2010Fields.PTCPOCINDEX_LINENUMBER] == "0701")
                num1 += this.FltVal("NEWHUD2.X5");
            }
            else
              num1 += this.FltVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORAMTPAID]);
            if ((strArray[HUDGFE2010Fields.PTCPOCINDEX_LINENUMBER] == "0701" || strArray[HUDGFE2010Fields.PTCPOCINDEX_LINENUMBER] == "0702") && this.IsLocked("NEWHUD2.X6"))
            {
              if (strArray[HUDGFE2010Fields.PTCPOCINDEX_LINENUMBER] == "0701")
                num2 += this.FltVal("NEWHUD2.X6");
            }
            else
              num2 += this.FltVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015SELAMTPAID]);
            if (!(strArray[HUDGFE2010Fields.PTCPOCINDEX_LINENUMBER] == "0701") && !(strArray[HUDGFE2010Fields.PTCPOCINDEX_LINENUMBER] == "0702") || !this.IsLocked("NEWHUD2.X5"))
              num3 += this.FltVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015TOTALPAIDBY]);
          }
        }
      }
      if (strArray[HUDGFE2010Fields.PTCPOCINDEX_LINENUMBER] == "2004")
      {
        this.SetCurrentNum("NEWHUD2.X4760", num1);
        this.SetCurrentNum("NEWHUD2.X4761", num2);
        this.SetCurrentNum("NEWHUD2.X4762", num3);
      }
      this.SetCurrentNum("NEWHUD2.X4427", this.FltVal("NEWHUD2.X24") + this.FltVal("NEWHUD2.X28") + this.FltVal("NEWHUD2.X32") + this.FltVal("NEWHUD2.X36") + this.FltVal("NEWHUD2.X40") + this.FltVal("NEWHUD2.X44") + this.FltVal("NEWHUD2.X48"));
      this.SetCurrentNum("NEWHUD2.X4428", this.FltVal("NEWHUD2.X25") + this.FltVal("NEWHUD2.X29") + this.FltVal("NEWHUD2.X33") + this.FltVal("NEWHUD2.X37") + this.FltVal("NEWHUD2.X41") + this.FltVal("NEWHUD2.X45") + this.FltVal("NEWHUD2.X49"));
      this.SetCurrentNum("NEWHUD2.X52", this.FltVal("NEWHUD2.X26") + this.FltVal("NEWHUD2.X30") + this.FltVal("NEWHUD2.X34") + this.FltVal("NEWHUD2.X38") + this.FltVal("NEWHUD2.X42") + this.FltVal("NEWHUD2.X46") + this.FltVal("NEWHUD2.X50"));
      this.SetCurrentNum("NEWHUD2.X27", this.FltVal("NEWHUD2.X24") + this.FltVal("NEWHUD2.X25") + this.FltVal("NEWHUD2.X26"));
      this.SetCurrentNum("NEWHUD2.X31", this.FltVal("NEWHUD2.X28") + this.FltVal("NEWHUD2.X29") + this.FltVal("NEWHUD2.X30"));
      this.SetCurrentNum("NEWHUD2.X35", this.FltVal("NEWHUD2.X32") + this.FltVal("NEWHUD2.X33") + this.FltVal("NEWHUD2.X34"));
      this.SetCurrentNum("NEWHUD2.X39", this.FltVal("NEWHUD2.X36") + this.FltVal("NEWHUD2.X37") + this.FltVal("NEWHUD2.X38"));
      this.SetCurrentNum("NEWHUD2.X43", this.FltVal("NEWHUD2.X40") + this.FltVal("NEWHUD2.X41") + this.FltVal("NEWHUD2.X42"));
      this.SetCurrentNum("NEWHUD2.X47", this.FltVal("NEWHUD2.X44") + this.FltVal("NEWHUD2.X45") + this.FltVal("NEWHUD2.X46"));
      this.SetCurrentNum("NEWHUD2.X51", this.FltVal("NEWHUD2.X48") + this.FltVal("NEWHUD2.X49") + this.FltVal("NEWHUD2.X50"));
      this.SetCurrentNum("NEWHUD2.X53", this.FltVal("NEWHUD2.X4427") + this.FltVal("NEWHUD2.X4428") + this.FltVal("NEWHUD2.X52"));
      this.SetCurrentNum("NEWHUD2.X4763", this.FltVal("NEWHUD2.X4760") + this.FltVal("NEWHUD2.X4761") + this.FltVal("NEWHUD2.X4762"));
    }

    public string GetProductFeature()
    {
      string productFeature = "";
      string str1 = this.Val("19");
      if (str1 == "ConstructionOnly" || str1 == "ConstructionToPermanent")
      {
        int num1 = this.IntVal("1176");
        if (str1 == "ConstructionOnly")
          --num1;
        int num2 = num1;
        if (str1 == "ConstructionToPermanent" && this.Val("2982") == "Y")
          num2 += this.IntVal("1177");
        if (num2 > 0)
        {
          if (num2 == 12)
            productFeature += "1 Year";
          else if (num2 < 24)
            productFeature = productFeature + (object) num2 + " mo.";
          else if (num2 >= 24)
            productFeature = productFeature + (Convert.ToDouble(num2) / 12.0).ToString("#.##") + " Year";
          productFeature += " Interest Only";
        }
      }
      else if (this.Val("NEWHUD.X6") == "Y")
      {
        int result;
        int.TryParse(this.Val("1712"), out result);
        if (result == 12)
          productFeature += "1 Year";
        else if (result < 24)
          productFeature = productFeature + (object) result + " mo.";
        else if (result >= 24)
          productFeature = productFeature + (Convert.ToDouble(result) / 12.0).ToString("#.##") + " Year";
        productFeature += " Negative Amortization";
      }
      else if (this.Val("2982") == "Y")
      {
        int result;
        int.TryParse(this.Val("1177"), out result);
        switch (result)
        {
          case 0:
            goto label_40;
          case 12:
            productFeature += "1 Year";
            break;
          default:
            if (result < 24)
            {
              productFeature = productFeature + (object) result + " mo.";
              break;
            }
            if (result >= 24)
            {
              productFeature = productFeature + (Convert.ToDouble(result) / 12.0).ToString("#.##") + " Year";
              break;
            }
            break;
        }
        productFeature += " Interest Only";
      }
      else if (this.Val("608") == "GraduatedPaymentMortgage")
      {
        string str2 = this.Val("1266");
        productFeature = (str2 == "" ? "0" : str2) + " Year Step Payment";
      }
      else if (this.Val("1659") == "Y")
      {
        int result;
        int.TryParse(this.Val("325"), out result);
        if (result == 12)
          productFeature += "Year 1";
        else if (result < 24)
          productFeature = productFeature + "mo. " + (object) result;
        else if (result >= 24)
          productFeature = productFeature + "Year " + (Convert.ToDouble(result) / 12.0).ToString("#.##");
        productFeature += " Balloon Payment";
      }
      else
        productFeature = !(this.Val("CD4.X27") == "Y") ? "" : "Seasonal Payment";
label_40:
      return productFeature;
    }

    public string GetProductType()
    {
      string productType = "";
      string str1 = this.Val("19");
      int num1 = this.IntVal("1176");
      if (this.Val("608") == "Fixed")
      {
        productType = "Fixed Rate";
        if (str1 == "ConstructionToPermanent" && this.FltVal("1677") != this.FltVal("3") && num1 != 0)
        {
          string str2 = "";
          if (num1 == 12)
            str2 += "1/0";
          else if (num1 < 24)
            str2 = str2 + (object) num1 + " mo./0";
          else if (num1 >= 24)
            str2 = str2 + (Convert.ToDouble(num1) / 12.0).ToString("#.##") + "/0";
          productType = str2 + " Step Rate";
        }
      }
      else if (this.Val("608") == "AdjustableRate")
      {
        int num2 = Utils.ParseInt((object) this.Val("696"), 0);
        int num3 = Utils.ParseInt((object) this.Val("694"), 0);
        string str3;
        switch (str1)
        {
          case "ConstructionOnly":
            str3 = string.Format("{0} mo./{1} mo.", (object) num2, (object) num3);
            break;
          case "ConstructionToPermanent":
            int num4 = num2 + num1;
            if (num4 == 12)
              productType = "1";
            else if (num4 < 24)
              productType = num4.ToString() + " mo.";
            else if (num4 >= 24)
              productType = (Convert.ToDouble(num2 + num1) / 12.0).ToString("#.##");
            str3 = productType + "/";
            if (num3 == 12)
            {
              str3 += "1";
              break;
            }
            if (num3 < 24)
            {
              str3 = str3 + (object) num3 + " mo.";
              break;
            }
            if (num3 >= 24)
            {
              str3 += (Convert.ToDouble(num3) / 12.0).ToString("#.##");
              break;
            }
            break;
          default:
            switch (num2)
            {
              case 0:
                productType += "0";
                break;
              case 12:
                productType += "1";
                break;
              default:
                if (num2 < 24)
                {
                  productType = productType + (object) num2 + " mo.";
                  break;
                }
                if (num2 >= 24)
                {
                  productType += (Convert.ToDouble(num2) / 12.0).ToString("#.##");
                  break;
                }
                break;
            }
            str3 = productType + "/";
            if (num3 == 12)
            {
              str3 += "1";
              break;
            }
            if (num3 < 24)
            {
              str3 = str3 + (object) num3 + " mo.";
              break;
            }
            if (num3 >= 24)
            {
              str3 += (Convert.ToDouble(num3) / 12.0).ToString("#.##");
              break;
            }
            break;
        }
        productType = str3 + " Adjustable Rate";
      }
      else if (this.Val("608") == "GraduatedPaymentMortgage")
      {
        string str4 = this.Val("1266");
        string str5 = productType + (str4 == "" ? "0" : str4) + "/";
        int result;
        int.TryParse(this.Val("694"), out result);
        if (result == 12)
          str5 += "1";
        else if (result < 24)
          str5 = str5 + (object) result + " mo.";
        else if (result >= 24)
          str5 += (Convert.ToDouble(result) / 12.0).ToString("#.##");
        productType = str5 + " Step Rate";
      }
      else if (this.Val("608") == "OtherAmortizationType" || this.Val("608") == "")
        productType = "";
      return productType;
    }

    public string GetProductDescription()
    {
      string productDescription = "";
      string productFeature = this.GetProductFeature();
      string productType = this.GetProductType();
      if (productFeature != "")
        productDescription += productFeature;
      if (productFeature != "" && productType != "")
        productDescription += ", ";
      if (productType != "")
        productDescription += productType;
      return productDescription;
    }

    private void calculateProductDescription(string id, string val)
    {
      this.SetVal("LE1.X5", this.GetProductDescription());
    }

    private void calculateLoanEstimate_TotalLoanCosts(string id, string val)
    {
      this.SetCurrentNum("LE2.XSTD", this.FltVal("LE2.XSTA") + this.FltVal("LE2.XSTB") + this.FltVal("LE2.XSTC"), true);
      this.SetCurrentNum("LE2.XDI", this.FltVal("LE2.XSTD") + this.FltVal("LE2.XSTI"), true);
      this.calculateLoanEstimate_TotalClosingCosts((string) null, (string) null);
    }

    private void calculateLoanEstimate_TotalOtherCosts(string id, string val)
    {
      this.SetCurrentNum("LE2.XSTI", this.FltVal("LE2.XSTE") + this.FltVal("LE2.XSTF") + this.FltVal("LE2.XSTG") + this.FltVal("LE2.XSTH"), true);
      this.SetCurrentNum("LE2.XDI", this.FltVal("LE2.XSTD") + this.FltVal("LE2.XSTI"), true);
      this.calculateLoanEstimate_TotalClosingCosts((string) null, (string) null);
    }

    private void calculateLoanEstimate_TotalClosingCosts(string id, string val)
    {
      this.SetCurrentNum("LE2.XSTJ", this.FltVal("LE2.XDI") - this.FltVal("LE2.XLC"), true);
      this.calculateLoanEstimate_ComparisonsTable(id, val);
      this.calculateLoanEstimate_EstimatedCashToClose((string) null, (string) null);
    }

    private void calculateClosingDisclosure_TotalLoanCosts(string id, string val)
    {
      this.SetCurrentNum("CD2.XSTD", this.FltVal("CD2.XLCAC") + this.FltVal("CD2.XLCBC"), true);
      this.calculateClosingDisclosure_TotalClosingCostsAtClosing((string) null, (string) null);
      this.calculateClosingDisclosure_TotalClosingCostsBeforeClosing((string) null, (string) null);
    }

    private void calculateClosingDisclosure_TotalOtherCosts(string id, string val)
    {
      this.SetCurrentNum("CD2.XSTI", this.FltVal("CD2.XOCAC") + this.FltVal("CD2.XOCBC"), true);
      this.calculateClosingDisclosure_TotalClosingCostsAtClosing((string) null, (string) null);
      this.calculateClosingDisclosure_TotalClosingCostsBeforeClosing((string) null, (string) null);
    }

    private void calculateClosingDisclosure_TotalClosingCostsAtClosing(string id, string val)
    {
      this.SetCurrentNum("CD2.XBCCAC", this.FltVal("CD2.XLCAC") + this.FltVal("CD2.XOCAC"), true);
      this.calculateClosingDisclosure_TotalClosingCosts((string) null, (string) null);
    }

    private void calculateClosingDisclosure_TotalClosingCostsBeforeClosing(string id, string val)
    {
      this.SetCurrentNum("CD2.XBCCBC", this.FltVal("CD2.XLCBC") + this.FltVal("CD2.XOCBC"), true);
      this.calculateClosingDisclosure_TotalClosingCosts((string) null, (string) null);
      this.CalculateCDPage3StdCashToCloseFinal((string) null, (string) null);
      this.CalculateCDPage3AlternateCashToCloseFinal((string) null, (string) null);
    }

    private void calculateClosingDisclosure_TotalClosingCosts(string id, string val)
    {
      this.SetCurrentNum("CD2.XSTJ", this.FltVal("CD2.XBCCAC") + this.FltVal("CD2.XBCCBC") - this.FltVal("CD2.XSTLC"), true);
      this.calculateCDPage3ClosingCosts(id, val);
      this.calculateLoan_TotalPayments((string) null, (string) null);
      this.CalcCDPage3Transactions();
      this.CalculateCDPage3StdCashToCloseFinal((string) null, (string) null);
      this.calculateLoanEstimate_DownPaymentAndFundsForBorrower((string) null, (string) null);
      this.CalculateCDPage3AlternateCashToCloseFinal((string) null, (string) null);
    }

    private void calculateLoanEstimate_ComparisonsTable(string id, string val)
    {
      if (this.loan == null || this.loan.Calculator == null)
        return;
      this.loan.Calculator.CalculateProjectedPaymentTable();
    }

    private void calculateLoanEstimate_DownPaymentAndFundsForBorrower(string id, string val)
    {
      string str = this.Val("19");
      double num1 = 0.0;
      if (this.Val("LE2.X30") == "Y")
      {
        switch (str)
        {
          case "Purchase":
          case "ConstructionOnly":
          case "ConstructionToPermanent":
            this.SetCurrentNum("LE2.X2", this.FltVal("L726") - this.FltVal("140") - this.FltVal("1109"), true);
            this.SetVal("LE2.X3", "0");
            break;
          case "NoCash-Out Refinance":
          case "Cash-Out Refinance":
          case "Other":
            double num2 = Utils.ArithmeticRounding(this.FltVal("LE2.X31") - this.FltVal("1109"), 2);
            if (num2 >= 0.0)
            {
              this.SetCurrentNum("LE2.X2", num2, true);
              this.SetVal("LE2.X3", "0");
              break;
            }
            this.SetCurrentNum("LE2.X3", num2 * -1.0, true);
            this.SetVal("LE2.X2", "0");
            break;
          default:
            this.SetVal("LE2.X2", "0");
            this.SetVal("LE2.X3", "0");
            break;
        }
      }
      else
      {
        switch (str)
        {
          case "ConstructionOnly":
          case "ConstructionToPermanent":
          case "Purchase":
            num1 = Utils.ArithmeticRounding(this.FltVal("L726") + this.FltVal("L79") + this.FltVal("LE2.X29") - (this.FltVal("2") - this.FltVal("LE2.X1")), 2);
            break;
          case "Cash-Out Refinance":
          case "NoCash-Out Refinance":
            num1 = Utils.ArithmeticRounding(this.FltVal("LE2.X29") - (this.FltVal("2") - this.FltVal("LE2.X1")), 2);
            break;
        }
        this.SetCurrentNum("LE2.X2", num1 <= 0.0 ? 0.0 : num1, true);
        this.SetCurrentNum("LE2.X3", num1 < 0.0 ? num1 * -1.0 : 0.0, true);
      }
      this.calculateLoanEstimate_EstimatedCashToClose((string) null, (string) null);
    }

    private void calculateLoanEstimate_EstimatedCashToClose(string id, string val)
    {
      if (this.Val("LE2.X28") == "Y")
      {
        this.calculateLoanEstimate_EstimatedCashToCloseAV((string) null, (string) null);
        this.SetCurrentNum("LE1.X87", this.FltVal("LE2.X26"), true);
      }
      else
      {
        this.calculateLoanEstimate_EstimatedCashToCloseSV((string) null, (string) null);
        this.SetCurrentNum("LE1.X87", this.FltVal("LE2.X25"), true);
      }
    }

    private void calculateLoanEstimate_EstimatedCashToCloseSV(string id, string val)
    {
      this.SetCurrentNum("LE2.X25", Utils.ArithmeticRounding(this.FltVal("LE2.XSTJ") - Utils.ArithmeticRounding(this.FltVal("LE2.X1"), 0) + Utils.ArithmeticRounding(this.FltVal("LE2.X2"), 0) - Utils.ArithmeticRounding(this.FltVal("L128"), 0) - Utils.ArithmeticRounding(this.FltVal("LE2.X3"), 0) - this.FltVal("LE2.X100") - Utils.ArithmeticRounding(this.FltVal("LE2.X4"), 0), 0), true);
    }

    private void calculateLoanEstimate_EstimatedCashToCloseAV(string id, string val)
    {
      double num = this.FltVal("LE2.XSTJ") + Utils.ArithmeticRounding(this.FltVal("LE2.X31"), 0) - Utils.ArithmeticRounding(this.FltVal("2"), 2);
      this.SetCurrentNum("LE2.X26", num < 0.0 ? num * -1.0 : num, true);
      if (num <= 0.0)
        this.SetVal("LE2.X27", "To Borrower");
      else
        this.SetVal("LE2.X27", "From Borrower");
    }

    private void calculateLoanEstimate_TotalAdjustmentsCredit(string id, string val)
    {
      this.calculateLoanEstimate_EstimatedCashToClose((string) null, (string) null);
      this.CalculateCDPage3StdCashToCloseFinal((string) null, (string) null);
    }

    private void calculateLoanEstimate_TaxesInsuranceInEscrowOrNot(string id, string val)
    {
      this.calculateClosingDisclosure_TaxesInsuranceInEscrowOrNot((string) null, (string) null);
    }

    private void calculateLoanEstimate_EstimatedTaxes(string id, string val)
    {
      double num1;
      if (this.Val("423") == "Biweekly")
      {
        double num2 = 6.0 / 13.0;
        num1 = Utils.ArithmeticRounding(this.FltVal("231") * num2, 2) + Utils.ArithmeticRounding(this.FltVal("230") * num2, 2) + Utils.ArithmeticRounding(this.FltVal("235") * num2, 2) + Utils.ArithmeticRounding(this.FltVal("L268") * num2, 2) + Utils.ArithmeticRounding(this.FltVal("1630") * num2, 2) + Utils.ArithmeticRounding(this.FltVal("253") * num2, 2) + Utils.ArithmeticRounding(this.FltVal("254") * num2, 2) + (this.FltVal("233") > 0.0 ? Utils.ArithmeticRounding(this.FltVal("233") * num2, 2) : 0.0);
      }
      else
        num1 = this.FltVal("231") + this.FltVal("230") + this.FltVal("235") + this.FltVal("L268") + this.FltVal("1630") + this.FltVal("253") + this.FltVal("254") + (this.FltVal("233") > 0.0 ? this.FltVal("233") : 0.0);
      this.SetCurrentNum("LE1.X29", Utils.ParseDouble((object) Utils.RemoveEndingZeros(num1.ToString(), true)), true);
      this.SetVal("LE1.XD29", Utils.RemoveEndingZeros(num1.ToString(), true));
      this.calculateClosingDisclosure_EstimatedTaxes((string) null, (string) null);
    }

    private void calculateClosingDisclosure_TaxesInsuranceInEscrowOrNot(string id, string val)
    {
      if (this.FltVal("1630") != 0.0 && this.Val("NEWHUD2.X124") != "Y" && this.Val("NEWHUD2.X125") != "Y" && this.Val("NEWHUD2.X126") != "Y")
        this.SetVal("NEWHUD2.X126", "Y");
      if (this.FltVal("253") != 0.0 && this.Val("NEWHUD2.X127") != "Y" && this.Val("NEWHUD2.X128") != "Y" && this.Val("NEWHUD2.X129") != "Y")
        this.SetVal("NEWHUD2.X129", "Y");
      if (this.FltVal("254") != 0.0 && this.Val("NEWHUD2.X130") != "Y" && this.Val("NEWHUD2.X131") != "Y" && this.Val("NEWHUD2.X132") != "Y")
        this.SetVal("NEWHUD2.X132", "Y");
      string str1 = "" + (this.Val("231") == "" ? "" : (this.Val("NEWHUD2.X134") == "Y" ? "Y" : "N")) + (this.Val("L268") == "" ? "" : (this.Val("NEWHUD2.X135") == "Y" ? "Y" : "N")) + (this.Val("1630") == "" || this.Val("NEWHUD2.X124") != "Y" ? "" : (!(this.Val("NEWHUD2.X137") == "Y") || !(this.Val("NEWHUD2.X124") == "Y") ? "N" : "Y")) + (this.Val("253") == "" || this.Val("NEWHUD2.X127") != "Y" ? "" : (!(this.Val("NEWHUD2.X138") == "Y") || !(this.Val("NEWHUD2.X127") == "Y") ? "N" : "Y")) + (this.Val("254") == "" || this.Val("NEWHUD2.X130") != "Y" ? "" : (!(this.Val("NEWHUD2.X139") == "Y") || !(this.Val("NEWHUD2.X130") == "Y") ? "N" : "Y"));
      if (str1.IndexOf("Y") > -1 && str1.IndexOf("N") > -1)
        this.SetVal("CD1.X4", "Some");
      else if (str1.IndexOf("Y") > -1)
        this.SetVal("CD1.X4", "Yes");
      else
        this.SetVal("CD1.X4", "No");
      this.SetVal("LE1.X30", this.Val("CD1.X4"));
      string str2 = (this.Val("230") == "" ? "" : (this.Val("NEWHUD2.X133") == "Y" ? "Y" : "N")) + (this.Val("235") == "" ? "" : (this.Val("NEWHUD2.X136") == "Y" ? "Y" : "N")) + (this.Val("1630") == "" || this.Val("NEWHUD2.X125") != "Y" ? "" : (!(this.Val("NEWHUD2.X137") == "Y") || !(this.Val("NEWHUD2.X125") == "Y") ? "N" : "Y")) + (this.Val("253") == "" || this.Val("NEWHUD2.X128") != "Y" ? "" : (!(this.Val("NEWHUD2.X138") == "Y") || !(this.Val("NEWHUD2.X128") == "Y") ? "N" : "Y")) + (this.Val("254") == "" || this.Val("NEWHUD2.X131") != "Y" ? "" : (!(this.Val("NEWHUD2.X139") == "Y") || !(this.Val("NEWHUD2.X131") == "Y") ? "N" : "Y"));
      if (str2.IndexOf("Y") > -1 && str2.IndexOf("N") > -1)
        this.SetVal("CD1.X5", "Some");
      else if (str2.IndexOf("Y") > -1)
        this.SetVal("CD1.X5", "Yes");
      else
        this.SetVal("CD1.X5", "No");
      this.SetVal("LE1.X31", this.Val("CD1.X5"));
      string str3 = (this.FltVal("233") > 0.0 ? "N" : "") + (this.Val("1630") == "" || this.Val("NEWHUD2.X126") != "Y" ? "" : (!(this.Val("NEWHUD2.X137") == "Y") || !(this.Val("NEWHUD2.X126") == "Y") ? "N" : "Y")) + (this.Val("253") == "" || this.Val("NEWHUD2.X129") != "Y" ? "" : (!(this.Val("NEWHUD2.X138") == "Y") || !(this.Val("NEWHUD2.X129") == "Y") ? "N" : "Y")) + (this.Val("254") == "" || this.Val("NEWHUD2.X132") != "Y" ? "" : (!(this.Val("NEWHUD2.X139") == "Y") || !(this.Val("NEWHUD2.X132") == "Y") ? "N" : "Y"));
      if (str3.IndexOf("Y") > -1 && str3.IndexOf("N") > -1)
        this.SetVal("CD1.X6", "Some");
      else if (str3.IndexOf("Y") > -1)
        this.SetVal("CD1.X6", "Yes");
      else
        this.SetVal("CD1.X6", "No");
      this.SetVal("LE1.X32", this.Val("CD1.X6"));
    }

    private void calculateClosingDisclosure_EstimatedTaxes(string id, string val)
    {
      double num1;
      if (this.Val("423") == "Biweekly")
      {
        double num2 = 6.0 / 13.0;
        num1 = Utils.ArithmeticRounding(this.FltVal("231") * num2, 2) + Utils.ArithmeticRounding(this.FltVal("230") * num2, 2) + Utils.ArithmeticRounding(this.FltVal("235") * num2, 2) + Utils.ArithmeticRounding(this.FltVal("L268") * num2, 2) + Utils.ArithmeticRounding(this.FltVal("1630") * num2, 2) + Utils.ArithmeticRounding(this.FltVal("253") * num2, 2) + Utils.ArithmeticRounding(this.FltVal("254") * num2, 2) + (this.FltVal("233") > 0.0 ? Utils.ArithmeticRounding(this.FltVal("233") * num2, 2) : 0.0);
      }
      else
        num1 = this.FltVal("231") + this.FltVal("230") + this.FltVal("235") + this.FltVal("L268") + this.FltVal("1630") + this.FltVal("253") + this.FltVal("254") + (this.FltVal("233") > 0.0 ? this.FltVal("233") : 0.0);
      this.SetCurrentNum("CD1.X3", num1, true);
    }

    public void CalculateBorrowerIntentToProceedDate()
    {
      if (this.loan.GetField("3164") == "Y")
      {
        DateTime today = DateTime.Today;
        this.SetVal("3972", today.ToString("MM/dd/yyyy"));
        today = DateTime.Today;
        this.SetVal("3197", today.ToString("MM/dd/yyyy"));
      }
      else
      {
        this.SetVal("3972", "//");
        this.SetVal("3197", "//");
      }
    }

    internal void GetIntentToProceedDT2015Log(Dictionary<DisclosureTracking2015Log, bool> log)
    {
      if (log == null)
        return;
      DisclosureTracking2015Log disclosureTracking2015Log1 = log.Keys.First<DisclosureTracking2015Log>();
      bool flag = log.Values.First<bool>();
      this.SetVal("3972", disclosureTracking2015Log1.DisclosedDate.ToString("d"));
      if (flag)
      {
        this.SetVal("3197", DateTime.Now.ToString("d"));
        this.SetVal("3973", this.sessionObjects.UserInfo.FullName + " (" + this.sessionObjects.UserInfo.Userid + ")");
        this.SetVal("3974", "");
        this.SetVal("3975", "");
        this.SetVal("3976", "");
      }
      foreach (IDisclosureTracking2015Log disclosureTracking2015Log2 in this.loan.GetLogList().GetAllIDisclosureTracking2015Log(true))
      {
        if (disclosureTracking2015Log2 == disclosureTracking2015Log1)
        {
          disclosureTracking2015Log2.IntentToProceed = true;
          if (flag)
          {
            disclosureTracking2015Log2.IntentToProceedDate = DateTime.Now;
            disclosureTracking2015Log2.IntentToProceedReceivedBy = this.sessionObjects.UserInfo.FullName + "(" + this.sessionObjects.UserInfo.Userid + ")";
            disclosureTracking2015Log2.IntentToProceedReceivedMethod = DisclosureTrackingBase.DisclosedMethod.None;
            disclosureTracking2015Log2.IntentToProceedReceivedMethodOther = "";
            disclosureTracking2015Log2.IntentToProceedComments = "";
            break;
          }
          if (this.Val("3197") != "" && this.Val("3197") != "//")
            disclosureTracking2015Log2.IntentToProceedDate = Convert.ToDateTime(this.Val("3197"));
          disclosureTracking2015Log2.IsIntentReceivedByLocked = this.IsLocked("3973");
          if (disclosureTracking2015Log2.IsIntentReceivedByLocked)
            disclosureTracking2015Log2.LockedIntentReceivedByField = this.Val("3973");
          else
            disclosureTracking2015Log2.IntentToProceedReceivedBy = this.Val("3973");
          disclosureTracking2015Log2.IntentToProceedReceivedMethod = !(this.Val("3974") != "") ? DisclosureTrackingBase.DisclosedMethod.None : (DisclosureTrackingBase.DisclosedMethod) Enum.Parse(typeof (DisclosureTrackingBase.DisclosedMethod), this.Val("3974"));
          disclosureTracking2015Log2.IntentToProceedReceivedMethodOther = this.Val("3975");
          disclosureTracking2015Log2.IntentToProceedComments = this.Val("3976");
          break;
        }
      }
      foreach (IDisclosureTracking2015Log disclosureTracking2015Log3 in this.loan.GetLogList().GetAllIDisclosureTracking2015Log(true))
      {
        if (disclosureTracking2015Log3 != disclosureTracking2015Log1)
          disclosureTracking2015Log3.IntentToProceed = false;
      }
    }

    internal void GetIntentToProceedIDisclosureTracking2015Log(
      Dictionary<IDisclosureTracking2015Log, bool> log)
    {
      if (log == null)
        return;
      IDisclosureTracking2015Log disclosureTracking2015Log1 = log.Keys.First<IDisclosureTracking2015Log>();
      bool flag = log.Values.First<bool>();
      this.SetVal("3972", disclosureTracking2015Log1.DisclosedDate.ToString("d"));
      if (flag)
      {
        this.SetVal("3197", DateTime.Now.ToString("d"));
        this.SetVal("3973", this.sessionObjects.UserInfo.FullName + " (" + this.sessionObjects.UserInfo.Userid + ")");
        this.SetVal("3974", "");
        this.SetVal("3975", "");
        this.SetVal("3976", "");
      }
      foreach (IDisclosureTracking2015Log disclosureTracking2015Log2 in this.loan.GetLogList().GetAllIDisclosureTracking2015Log(true))
      {
        if (disclosureTracking2015Log2 == disclosureTracking2015Log1)
        {
          disclosureTracking2015Log2.IntentToProceed = true;
          if (flag)
          {
            disclosureTracking2015Log2.IntentToProceedDate = DateTime.Now;
            disclosureTracking2015Log2.IntentToProceedReceivedBy = this.sessionObjects.UserInfo.FullName + "(" + this.sessionObjects.UserInfo.Userid + ")";
            disclosureTracking2015Log2.IntentToProceedReceivedMethod = DisclosureTrackingBase.DisclosedMethod.None;
            disclosureTracking2015Log2.IntentToProceedReceivedMethodOther = "";
            disclosureTracking2015Log2.IntentToProceedComments = "";
            break;
          }
          if (this.Val("3197") != "" && this.Val("3197") != "//")
            disclosureTracking2015Log2.IntentToProceedDate = Convert.ToDateTime(this.Val("3197"));
          disclosureTracking2015Log2.IsIntentReceivedByLocked = this.IsLocked("3973");
          if (disclosureTracking2015Log2.IsIntentReceivedByLocked)
            disclosureTracking2015Log2.LockedIntentReceivedByField = this.Val("3973");
          else
            disclosureTracking2015Log2.IntentToProceedReceivedBy = this.Val("3973");
          disclosureTracking2015Log2.IntentToProceedReceivedMethod = !(this.Val("3974") != "") ? DisclosureTrackingBase.DisclosedMethod.None : (DisclosureTrackingBase.DisclosedMethod) Enum.Parse(typeof (DisclosureTrackingBase.DisclosedMethod), this.Val("3974"));
          disclosureTracking2015Log2.IntentToProceedReceivedMethodOther = this.Val("3975");
          disclosureTracking2015Log2.IntentToProceedComments = this.Val("3976");
          break;
        }
      }
      foreach (IDisclosureTracking2015Log disclosureTracking2015Log3 in this.loan.GetLogList().GetAllIDisclosureTracking2015Log(true))
      {
        if (disclosureTracking2015Log3 != disclosureTracking2015Log1)
          disclosureTracking2015Log3.IntentToProceed = false;
      }
    }

    private void setDisclosureTrackingLogData(string id, string val)
    {
      foreach (IDisclosureTracking2015Log disclosureTracking2015Log in this.loan.GetLogList().GetAllIDisclosureTracking2015Log(true))
      {
        if (disclosureTracking2015Log.DisclosedForLE && disclosureTracking2015Log.IntentToProceed)
        {
          if (id == "3973")
          {
            disclosureTracking2015Log.IsIntentReceivedByLocked = this.IsLocked("3973");
            if (disclosureTracking2015Log.IsIntentReceivedByLocked)
              disclosureTracking2015Log.LockedIntentReceivedByField = val;
            else
              disclosureTracking2015Log.IntentToProceedReceivedBy = val;
          }
          if (id == "3974")
            disclosureTracking2015Log.IntentToProceedReceivedMethod = (DisclosureTrackingBase.DisclosedMethod) Enum.Parse(typeof (DisclosureTrackingBase.DisclosedMethod), val);
          if (id == "3975")
            disclosureTracking2015Log.IntentToProceedReceivedMethodOther = val;
          if (id == "3976")
            disclosureTracking2015Log.IntentToProceedComments = val;
          if (!(id == "3197"))
            break;
          disclosureTracking2015Log.IntentToProceedDate = Convert.ToDateTime(val);
          break;
        }
      }
    }

    private void setAIRTableIndexType(string id, string val)
    {
      if (this.Val("19") == "ConstructionToPermanent" && this.Val("608") == "Fixed" && Utils.ToDouble(this.Val("1677")) < Utils.ToDouble(this.Val("3")))
      {
        this.SetCurrentNum("CD4.X47", 1.0);
        this.SetCurrentNum("CD4.X48", Utils.ToDouble(this.Val("3")) - Utils.ToDouble(this.Val("1677")));
        this.SetVal("LE2.X99", (Utils.ParseInt((object) this.Val("CD4.X46")) + 1).ToString());
        this.SetVal("LE2.X97", Utils.GetMonthSuffix(Utils.ParseInt((object) this.Val("CD4.X46")) + 1));
      }
      else
      {
        this.loan.SetField("LE2.X96", this.loan.GetField("1959"));
        if (!string.IsNullOrEmpty(this.Val("696")))
        {
          if (this.Val("19") == "ConstructionToPermanent" && this.Val("608") == "AdjustableRate")
          {
            int num = string.IsNullOrEmpty(this.Val("1176")) ? 0 : Utils.ParseInt((object) this.Val("1176"));
            this.SetVal("LE2.X99", (Utils.ParseInt((object) this.Val("696")) + num + 1).ToString());
          }
          else
            this.SetVal("LE2.X99", (Utils.ParseInt((object) this.Val("696")) + 1).ToString());
          this.SetVal("LE2.X97", Utils.GetMonthSuffix(Utils.ParseInt((object) this.Val("LE2.X99"))));
        }
        else
        {
          this.SetVal("LE2.X99", "");
          this.SetVal("LE2.X97", "");
        }
        if (!string.IsNullOrEmpty(this.Val("694")))
          this.SetVal("LE2.X98", Utils.GetMonthSuffix(Utils.ParseInt((object) this.Val("694"))));
        else
          this.SetVal("LE2.X98", "");
      }
    }

    public DateTime GetInitialBorrowerReceivedDate()
    {
      DateTime dateTime = new DateTime();
      foreach (IDisclosureTracking2015Log disclosureTracking2015Log in this.loan.GetLogList().GetAllIDisclosureTracking2015Log(true))
      {
        if (disclosureTracking2015Log.DisclosedForLE && disclosureTracking2015Log.IntentToProceed)
        {
          dateTime = disclosureTracking2015Log.BorrowerActualReceivedDate;
          if (disclosureTracking2015Log.IsBorrowerPresumedDateLocked)
          {
            if (disclosureTracking2015Log.LockedBorrowerPresumedReceivedDate != DateTime.MinValue && (disclosureTracking2015Log.LockedBorrowerPresumedReceivedDate < dateTime || dateTime == DateTime.MinValue))
              dateTime = disclosureTracking2015Log.LockedBorrowerPresumedReceivedDate;
          }
          else if (disclosureTracking2015Log.BorrowerPresumedReceivedDate != DateTime.MinValue && (disclosureTracking2015Log.BorrowerPresumedReceivedDate < dateTime || dateTime == DateTime.MinValue))
            dateTime = disclosureTracking2015Log.BorrowerPresumedReceivedDate;
          if (disclosureTracking2015Log.CoBorrowerActualReceivedDate != DateTime.MinValue && (disclosureTracking2015Log.CoBorrowerActualReceivedDate < dateTime || dateTime == DateTime.MinValue))
            dateTime = disclosureTracking2015Log.CoBorrowerActualReceivedDate;
          if (disclosureTracking2015Log.IsCoBorrowerPresumedDateLocked)
          {
            if (disclosureTracking2015Log.LockedCoBorrowerPresumedReceivedDate != DateTime.MinValue && (disclosureTracking2015Log.LockedCoBorrowerPresumedReceivedDate < dateTime || dateTime == DateTime.MinValue))
            {
              dateTime = disclosureTracking2015Log.LockedCoBorrowerPresumedReceivedDate;
              break;
            }
            break;
          }
          if (disclosureTracking2015Log.CoBorrowerPresumedReceivedDate != DateTime.MinValue && (disclosureTracking2015Log.CoBorrowerPresumedReceivedDate < dateTime || dateTime == DateTime.MinValue))
          {
            dateTime = disclosureTracking2015Log.CoBorrowerPresumedReceivedDate;
            break;
          }
          break;
        }
      }
      return dateTime.Date;
    }

    public DateTime GetEarliestFeeCollectionDate()
    {
      return Utils.ParseDate((object) this.loan.GetField("3153"));
    }

    private void calculateCDPage3Cash(string id, string val)
    {
      this.SetCurrentNum("CD3.X38", this.FltVal("CD3.X43"), true);
      this.SetCurrentNum("CD3.X39", this.FltVal("CD3.X44"), true);
      this.SetCurrentNum("CD3.X40", this.FltVal("CD3.X43") - this.FltVal("CD3.X44"), true);
      if (this.FltVal("CD3.X43") - this.FltVal("CD3.X44") < 0.0)
        this.SetVal("CD3.X49", "From Seller");
      else
        this.SetVal("CD3.X49", "To Seller");
    }

    private void calculateCDPage3CashToClose(string id, string val)
    {
      this.SetCurrentNum("CD3.X21", this.FltVal("CD3.X41"), true);
      this.SetCurrentNum("CD3.X22", this.FltVal("CD3.X42"), true);
      this.SetCurrentNum("CD3.X23", this.FltVal("CD3.X41") - this.FltVal("CD3.X42"), true);
      if (this.FltVal("CD3.X41") - this.FltVal("CD3.X42") >= 0.0)
        this.SetVal("CD3.X48", "From Borrower");
      else
        this.SetVal("CD3.X48", "To Borrower");
    }

    private void calculateCDPage3TotalK(string id, string val)
    {
      this.SetCurrentNum("CD3.X41", this.FltVal("L726") + this.FltVal("L79") + this.FltVal("CD3.X1") + this.FltVal("L85") + this.FltVal("L89") + this.FltVal("CD3.X3") + this.FltVal("CD3.X5") + this.FltVal("L94") + this.FltVal("L100") + this.FltVal("L106") + this.FltVal("L111") + this.FltVal("L115") + this.FltVal("L119") + this.FltVal("L123") + this.FltVal("CD3.X7"), true);
      this.calculateCDPage3CashToClose(id, val);
    }

    private void calculateCDPage3TotalN(string id, string val)
    {
      this.SetCurrentNum("CD3.X44", this.FltVal("L129") + this.FltVal("L133") + this.FltVal("L136") + this.FltVal("L139") + this.FltVal("L143") + this.FltVal("L147") + this.FltVal("L151") + this.FltVal("L155") + this.FltVal("CD3.X33") + this.FltVal("CD3.X35") + this.FltVal("CD3.X37") + this.FltVal("L161") + this.FltVal("L167") + this.FltVal("L173") + this.FltVal("L177") + this.FltVal("L181") + this.FltVal("L185") + this.FltVal("CD3.X46") + this.FltVal("CD3.X108"), true);
      this.calculateCDPage3Cash(id, val);
    }

    private void calculateCDPage3TotalM(string id, string val)
    {
      this.SetCurrentNum("CD3.X43", this.FltVal("L726") + this.FltVal("L80") + this.FltVal("L82") + this.FltVal("L87") + this.FltVal("L91") + this.FltVal("CD3.X25") + this.FltVal("CD3.X27") + this.FltVal("CD3.X29") + this.FltVal("L97") + this.FltVal("L103") + this.FltVal("L109") + this.FltVal("L113") + this.FltVal("L117") + this.FltVal("L121") + this.FltVal("L125") + this.FltVal("CD3.X31"), true);
      this.calculateCDPage3Cash(id, val);
    }

    private void calculateCDPage3TotalL(string id, string val)
    {
      this.SetCurrentNum("CD3.X42", this.FltVal("L128") + this.FltVal("2") + this.FltVal("L132") + this.FltVal("L135") + this.FltVal("CD3.X108") + this.FltVal("CD3.X10") + this.FltVal("CD3.X12") + this.FltVal("CD3.X14") + this.FltVal("CD3.X16") + this.FltVal("CD3.X18") + this.FltVal("CD3.X20") + this.FltVal("L158") + this.FltVal("L164") + this.FltVal("L170") + this.FltVal("L175") + this.FltVal("L179") + this.FltVal("L183"), true);
      this.calculateCDPage3CashToClose(id, val);
    }

    private void calculateCDPage3TotalPurchasePayoffsIncluded(string id, string val)
    {
      if (this.Val("LE2.X101") == "Y" && this.Val("19") == "Purchase")
        this.SetCurrentNum("CD3.X1543", this.FltVal("CD3.X84"), true);
      else
        this.SetVal("CD3.X1543", "");
    }

    private void calculateCDPage3ClosingCosts(string id, string val)
    {
      if (this.Val("CD2.XSTJ") != "")
      {
        if (this.Val("CD3.X137") == "Y")
          this.SetCurrentNum("CD3.X1", this.FltVal("CD2.XSTJ"), true);
        else
          this.SetCurrentNum("CD3.X1", this.FltVal("CD2.XSTJ") - this.FltVal("CD3.X103"));
      }
      this.SetCurrentNum("CD3.X46", this.FltVal("CD2.XSCCAC"), true);
    }

    private void calculateCDPage3SellerCredit(string id, string val)
    {
    }

    private void calculateEstimatedPropertyCosts(string id, string val)
    {
      double num1 = this.FltVal("230") + this.FltVal("231") + this.FltVal("L268") + this.FltVal("235");
      if (this.Val("NEWHUD2.X124") == "Y" || this.Val("NEWHUD2.X125") == "Y" || this.Val("NEWHUD2.X126") == "Y")
        num1 += this.FltVal("1630");
      if (this.Val("NEWHUD2.X127") == "Y" || this.Val("NEWHUD2.X128") == "Y" || this.Val("NEWHUD2.X129") == "Y")
        num1 += this.FltVal("253");
      if (this.Val("NEWHUD2.X130") == "Y" || this.Val("NEWHUD2.X131") == "Y" || this.Val("NEWHUD2.X132") == "Y")
        num1 += this.FltVal("254");
      double num2 = num1 * 12.0;
    }

    private void calculatePropertyCosts_OtherRow(string id, string val)
    {
      if ((this.Val("NEWHUD2.X124") == "Y" || this.Val("NEWHUD2.X125") == "Y" || this.Val("NEWHUD2.X126") == "Y") && this.Val("NEWHUD.X340") == "Y")
        this.SetVal("CD4.X4", "Y");
      else if (this.FltVal("1630") != 0.0)
        this.SetVal("CD4.X4", "N");
      else
        this.SetVal("CD4.X4", "");
      if ((this.Val("NEWHUD2.X127") == "Y" || this.Val("NEWHUD2.X128") == "Y" || this.Val("NEWHUD2.X129") == "Y") && this.Val("NEWHUD.X341") == "Y")
        this.SetVal("CD4.X5", "Y");
      else if (this.FltVal("253") != 0.0)
        this.SetVal("CD4.X5", "N");
      else
        this.SetVal("CD4.X5", "");
      if ((this.Val("NEWHUD2.X130") == "Y" || this.Val("NEWHUD2.X131") == "Y" || this.Val("NEWHUD2.X132") == "Y") && this.Val("NEWHUD.X342") == "Y")
        this.SetVal("CD4.X6", "Y");
      else if (this.FltVal("254") != 0.0)
        this.SetVal("CD4.X6", "N");
      else
        this.SetVal("CD4.X6", "");
      this.SetVal("CD4.X45", this.FltVal("233") > 0.0 ? "N" : "");
    }

    internal void CalcCDPage3Transactions()
    {
      this.calculateDisclosedSalesPrice((string) null, (string) null);
      this.calculateCDPage3TotalK((string) null, (string) null);
      this.calculateCDPage3TotalN((string) null, (string) null);
      this.calculateCDPage3TotalM((string) null, (string) null);
      this.calculateCDPage3TotalL((string) null, (string) null);
      this.calculateCDPage3ClosingCosts((string) null, (string) null);
    }

    private void calculate2015LineItem904Fee(string id, string val)
    {
      if (!this.UseNew2015GFEHUD)
        return;
      this.calculate2015LineItemFor900Fee("NEWHUD2.X4397", "231", "NEWHUD.X594", "NEWHUD.X591");
    }

    private void calculate2015LineItem906Fee(string id, string val)
    {
      if (!this.UseNew2015GFEHUD)
        return;
      this.calculate2015LineItemFor900Fee("NEWHUD2.X4399", "NEWHUD2.X4400", "579", "643");
    }

    private void calculate2015LineItem907Fee(string id, string val)
    {
      if (!this.UseNew2015GFEHUD)
        return;
      this.calculate2015LineItemFor900Fee("NEWHUD2.X4401", "NEWHUD2.X4402", "L261", "L260");
    }

    private void calculate2015LineItem908Fee(string id, string val)
    {
      if (!this.UseNew2015GFEHUD)
        return;
      this.calculate2015LineItemFor900Fee("NEWHUD2.X4403", "NEWHUD2.X4404", "1668", "1667");
    }

    private void calculate2015LineItem909Fee(string id, string val)
    {
      if (!this.UseNew2015GFEHUD)
        return;
      this.calculate2015LineItemFor900Fee("NEWHUD2.X4405", "NEWHUD2.X4406", "NEWHUD.X595", "NEWHUD.X592");
    }

    private void calculate2015LineItem910Fee(string id, string val)
    {
      if (!this.UseNew2015GFEHUD)
        return;
      this.calculate2015LineItemFor900Fee("NEWHUD2.X4407", "NEWHUD2.X4408", "NEWHUD.X596", "NEWHUD.X593");
    }

    private void calculate2015LineItem911Fee(string id, string val)
    {
      if (!this.UseNew2015GFEHUD)
        return;
      this.calculate2015LineItemFor900Fee("NEWHUD2.X4409", "NEWHUD2.X4410", "NEWHUD.X1589", "NEWHUD.X1588");
    }

    private void calculate2015LineItem912Fee(string id, string val)
    {
      if (!this.UseNew2015GFEHUD)
        return;
      this.calculate2015LineItemFor900Fee("NEWHUD2.X4411", "NEWHUD2.X4412", "NEWHUD.X1597", "NEWHUD.X1596");
    }

    private void calculate2015LineItemFor900Fee(
      string monthsFieldId,
      string paymentAmountFieldId,
      string sellerFeeFieldId,
      string borrowerAmountFieldId)
    {
      if (this.GetFieldFromCal(monthsFieldId) != string.Empty && this.GetFieldFromCal(paymentAmountFieldId) != string.Empty)
      {
        double num = this.FltVal(monthsFieldId) * this.FltVal(paymentAmountFieldId) - this.FltVal(sellerFeeFieldId);
        this.SetCurrentNum(borrowerAmountFieldId, num);
      }
      else
      {
        double num = -this.FltVal(sellerFeeFieldId);
        this.SetCurrentNum(borrowerAmountFieldId, num);
      }
    }

    private void calculateCdPage3Liabilities(string id, string val)
    {
      string str1 = "CD3.X";
      int num1 = 50;
      int num2 = 65;
      double num3 = 0.0;
      int num4 = 1;
      int numberOfNonVols = this.loan.GetNumberOfNonVols();
      for (int index = 1; index <= numberOfNonVols; ++index)
      {
        string str2 = "UNFL" + index.ToString("00");
        if (this.Val(str2 + "02").Contains("exceeding legal limits P.O.C."))
        {
          this.SetVal(str1 + (object) num1, this.Val(str2 + "02"));
          this.SetCurrentNum(str1 + (object) num2, this.FltVal(str2 + "04"));
          ++num1;
          ++num2;
          if (num4 == 15)
          {
            num1 = 1544;
            num2 = 1554;
          }
          ++num4;
          break;
        }
      }
      BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
      BorrowerPair currentBorrowerPair = this.loan.CurrentBorrowerPair;
      for (int index1 = 0; index1 < borrowerPairs.Length; ++index1)
      {
        this.loan.SetBorrowerPair(borrowerPairs[index1]);
        int exlcudingAlimonyJobExp = this.loan.GetNumberOfLiabilitesExlcudingAlimonyJobExp();
        for (int index2 = 1; index2 <= exlcudingAlimonyJobExp; ++index2)
        {
          string str3 = "FL" + index2.ToString("00");
          if (this.Val(str3 + "18") != "Y")
            this.SetVal(str3 + "63", "N");
          if (this.Val(str3 + "63") == "Y")
          {
            num3 += this.FltVal(str3 + "16");
            if (num4 <= 25)
            {
              this.SetVal(str1 + (object) num1, this.Val(str3 + "02") + " " + this.Val(str3 + "43"));
              this.SetCurrentNum(str1 + (object) num2, this.FltVal(str3 + "16"));
              ++num1;
              ++num2;
              if (num4 == 15)
              {
                num1 = 1544;
                num2 = 1554;
              }
              ++num4;
            }
          }
        }
      }
      for (int index = 1; index <= numberOfNonVols; ++index)
      {
        string str4 = "UNFL" + index.ToString("00");
        if (!(this.Val(str4 + "05") != "Y") && !this.Val(str4 + "02").Contains("exceeding legal limits P.O.C."))
        {
          num3 += this.FltVal(str4 + "04");
          if (num4 <= 25)
          {
            this.SetVal(str1 + (object) num1, this.Val(str4 + "02"));
            this.SetCurrentNum(str1 + (object) num2, this.FltVal(str4 + "04"));
            ++num1;
            ++num2;
            if (num4 == 15)
            {
              num1 = 1544;
              num2 = 1554;
            }
            ++num4;
          }
        }
      }
      this.SetCurrentNum("CD3.X80", num3, true);
      this.SetCurrentNum("LE2.X31", Utils.ArithmeticRounding(num3, 0), true);
      this.calObjs.VACal.CalcVACashOutRefinance((string) null, (string) null);
      int num5 = 50;
      int num6 = 65;
      int num7 = num4 - 1;
      for (int index = num7; index < 15; ++index)
      {
        this.SetVal(str1 + (object) (num5 + index), "");
        this.SetCurrentNum(str1 + (object) (num6 + index), 0.0);
        ++num7;
      }
      if (num7 >= 15)
      {
        num5 = 1544;
        num6 = 1554;
      }
      for (int index = num7 - 15; index < 10; ++index)
      {
        this.SetVal(str1 + (object) (num5 + index), "");
        this.SetCurrentNum(str1 + (object) (num6 + index), 0.0);
        ++num7;
      }
      if (currentBorrowerPair != null && (this.loan.CurrentBorrowerPair == null || currentBorrowerPair.Id != this.loan.CurrentBorrowerPair.Id))
        this.loan.SetBorrowerPair(currentBorrowerPair);
      this.CalculateCDPage3AlternateCashToCloseFinal("", "");
    }

    internal void MigrateCdPage3Liabilities()
    {
      BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
      BorrowerPair currentBorrowerPair = this.loan.CurrentBorrowerPair;
      for (int index1 = 0; index1 < borrowerPairs.Length; ++index1)
      {
        this.loan.SetBorrowerPair(borrowerPairs[index1]);
        int exlcudingAlimonyJobExp = this.loan.GetNumberOfLiabilitesExlcudingAlimonyJobExp();
        for (int index2 = 1; index2 <= exlcudingAlimonyJobExp; ++index2)
        {
          string str = "FL" + index2.ToString("00");
          if (this.Val(str + "18") == "Y")
            this.SetVal(str + "63", "Y");
        }
      }
      if (currentBorrowerPair == null || this.loan.CurrentBorrowerPair != null && !(currentBorrowerPair.Id != this.loan.CurrentBorrowerPair.Id))
        return;
      this.loan.SetBorrowerPair(currentBorrowerPair);
    }

    internal void MoveDataFrom2015to2010(string id, string val)
    {
      this.loan.Calculator.CalcOnDemand(CalcOnDemandEnum.Update2010ItemizationFrom2015Itemization);
      this.setCustomizedProjectedPaymentTable(id, val);
      this.populateLoanType_OtherField(id, val);
    }

    internal void MoveDataFrom2010to2015(string id, string val)
    {
      this.setCustomizedProjectedPaymentTable(id, val);
      if (this.Val("1172") == "FarmersHomeAdministration")
      {
        if (this.Val("3566") == "FinancingAll")
        {
          this.loan.SetField(this.IsLocked("NEWHUD.X1301") ? "NEWHUD2.X2187" : "NEWHUD2.X1593", this.Val("1045"));
          this.loan.SetField(this.IsLocked("NEWHUD.X1301") ? "NEWHUD2.X2188" : "NEWHUD2.X1594", this.Val("1760"));
        }
        else if (this.Val("3566") == "FinancingPortion")
        {
          double num = Utils.ArithmeticRounding(Utils.ArithmeticRounding(this.FltVal("3563") % 1.0, 2), 2);
          this.SetCurrentNum(this.IsLocked("NEWHUD.X1301") ? "NEWHUD2.X2187" : "NEWHUD2.X1593", this.FltVal("3563") - num);
          this.SetCurrentNum(this.IsLocked("NEWHUD.X1301") ? "NEWHUD2.X2188" : "NEWHUD2.X1594", this.FltVal("3564") - this.FltVal("3563") + num);
        }
      }
      for (int index = 0; index < HUDGFE2010Fields.WHOLEPOC_FIELDS.Count; ++index)
      {
        string[] strArray = HUDGFE2010Fields.WHOLEPOC_FIELDS[index];
        if (!(strArray[HUDGFE2010Fields.PTCPOCINDEX_LINENUMBER] == "2001"))
        {
          if (!(strArray[HUDGFE2010Fields.PTCPOCINDEX_POCPAIDBY] == ""))
          {
            string fieldFromCal1 = this.GetFieldFromCal(strArray[HUDGFE2010Fields.PTCPOCINDEX_POCPAIDBY]);
            string fieldFromCal2 = strArray[HUDGFE2010Fields.PTCPOCINDEX_PTCPAIDBY] != "" ? this.GetFieldFromCal(strArray[HUDGFE2010Fields.PTCPOCINDEX_PTCPAIDBY]) : "";
            string fieldFromCal3 = strArray[HUDGFE2010Fields.PTCPOCINDEX_POCAMT] != "" ? this.GetFieldFromCal(strArray[HUDGFE2010Fields.PTCPOCINDEX_POCAMT]) : "";
            string fieldFromCal4 = strArray[HUDGFE2010Fields.PTCPOCINDEX_PTCAMT] != "" ? this.GetFieldFromCal(strArray[HUDGFE2010Fields.PTCPOCINDEX_PTCAMT]) : "";
            string fieldFromCal5 = strArray[HUDGFE2010Fields.PTCPOCINDEX_PAIDTO] != "" ? this.GetFieldFromCal(strArray[HUDGFE2010Fields.PTCPOCINDEX_PAIDTO]) : "";
            switch (fieldFromCal1)
            {
              case "Lender":
                if (strArray[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPOCAMT] != "")
                {
                  this.loan.SetField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPOCAMT], fieldFromCal3);
                  break;
                }
                break;
              case "Broker":
                if (strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPOCAMT] != "")
                {
                  this.loan.SetField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPOCAMT], fieldFromCal3);
                  break;
                }
                break;
              case "Other":
                if (strArray[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPOCAMT] != "")
                {
                  this.loan.SetField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPOCAMT], fieldFromCal3);
                  break;
                }
                break;
              case "":
                if (strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORPOCAMT] != "")
                {
                  this.loan.SetField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORPOCAMT], fieldFromCal3);
                  break;
                }
                break;
            }
            switch (fieldFromCal2)
            {
              case "Lender":
                if (strArray[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPACAMT] != "")
                {
                  this.loan.SetField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPACAMT], fieldFromCal4);
                  break;
                }
                break;
              case "Broker":
                if (strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPACAMT] != "")
                {
                  this.loan.SetField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPACAMT], fieldFromCal4);
                  break;
                }
                break;
              case "Other":
                if (strArray[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPACAMT] != "")
                {
                  this.loan.SetField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPACAMT], fieldFromCal4);
                  break;
                }
                break;
            }
            if (strArray[HUDGFE2010Fields.PTCPOCINDEX_PAIDTONAME] != "" && this.GetFieldFromCal(strArray[HUDGFE2010Fields.PTCPOCINDEX_PAIDTONAME]) == "")
            {
              switch (fieldFromCal5)
              {
                case "Broker":
                  this.loan.SetField(strArray[HUDGFE2010Fields.PTCPOCINDEX_PAIDTONAME], this.GetFieldFromCal("VEND.X293"));
                  continue;
                case "Lender":
                  this.loan.SetField(strArray[HUDGFE2010Fields.PTCPOCINDEX_PAIDTONAME], this.GetFieldFromCal("1264"));
                  continue;
                case "Seller":
                  this.loan.SetField(strArray[HUDGFE2010Fields.PTCPOCINDEX_PAIDTONAME], this.GetFieldFromCal("638"));
                  continue;
                case "Investor":
                  this.loan.SetField(strArray[HUDGFE2010Fields.PTCPOCINDEX_PAIDTONAME], this.GetFieldFromCal("VEND.X263"));
                  continue;
                default:
                  continue;
              }
            }
          }
        }
        else
          break;
      }
      if (this.GetFieldFromCal("NEWHUD.X591") != string.Empty)
        this.loan.AddLock("NEWHUD.X591");
      if (this.GetFieldFromCal("643") != string.Empty)
        this.loan.AddLock("643");
      if (this.GetFieldFromCal("L260") != string.Empty)
        this.loan.AddLock("L260");
      if (this.GetFieldFromCal("1667") != string.Empty)
        this.loan.AddLock("1667");
      if (this.GetFieldFromCal("NEWHUD.X592") != string.Empty)
        this.loan.AddLock("NEWHUD.X592");
      if (this.GetFieldFromCal("NEWHUD.X593") != string.Empty)
        this.loan.AddLock("NEWHUD.X593");
      if (this.GetFieldFromCal("NEWHUD.X1588") != string.Empty)
        this.loan.AddLock("NEWHUD.X1588");
      if (this.GetFieldFromCal("NEWHUD.X1596") != string.Empty)
        this.loan.AddLock("NEWHUD.X1596");
      this.calculateLoanEstimate_AIRMonthSuffix("696", (string) null);
      this.calculateLoanEstimate_AIRMonthSuffix("694", (string) null);
      this.calculateLoanEstimate_AIRMonthSuffix((string) null, (string) null);
      this.populateLoanType_OtherField(id, val);
      this.loan.SetField("LE2.X96", this.loan.GetField("1959"));
      this.calObjs.Cal.CalculateAll();
    }

    public void CalculateCDPage3AlternateCashToCloseLoanEstimate(string id, string val)
    {
      IDisclosureTracking2015Log idisclosureTracking2015Log = this.loan.GetLogList().GetLatestIDisclosureTracking2015Log(DisclosureTracking2015Log.DisclosureTrackingType.LE);
      if (idisclosureTracking2015Log != null)
      {
        this.SetCurrentNum("CD3.X87", Utils.ArithmeticRounding(this.Flt(idisclosureTracking2015Log.GetDisclosedField("2")), 0), true);
        this.SetCurrentNum("CD3.X88", Utils.ArithmeticRounding(this.Flt(idisclosureTracking2015Log.GetDisclosedField("LE2.XSTJ")), 0), true);
        this.SetCurrentNum("CD3.X89", 0.0, true);
        this.SetCurrentNum("CD3.X90", Utils.ArithmeticRounding(this.Flt(idisclosureTracking2015Log.GetDisclosedField(idisclosureTracking2015Log.GetDisclosedField("LE2.X31") == "" ? "1092" : "LE2.X31")), 0), true);
        for (int index = 87; index <= 90; ++index)
        {
          switch (index)
          {
            case 89:
              continue;
            case 90:
              if (this.Flt(idisclosureTracking2015Log.GetDisclosedField("CD3.XH90")) == 0.0)
              {
                this.SetCurrentNum("CD3.XH90", this.FltVal("CD3.X90"));
                continue;
              }
              this.SetCurrentNum("CD3.XH" + (object) index, this.Flt(idisclosureTracking2015Log.GetDisclosedField("CD3.XH" + (object) index)), true);
              continue;
            default:
              this.SetCurrentNum("CD3.XH" + (object) index, this.Flt(idisclosureTracking2015Log.GetDisclosedField("CD3.XH" + (object) index)), true);
              continue;
          }
        }
        double num = this.FltVal("CD3.X87") - this.FltVal("CD3.X88") - this.FltVal("CD3.X90");
        this.SetCurrentNum("CD3.X91", num < 0.0 ? num * -1.0 : num, true);
        if (num > 0.0)
          this.SetVal("CD3.X92", "To Borrower");
        else
          this.SetVal("CD3.X92", "From Borrower");
      }
      else
      {
        this.SetCurrentNum("CD3.X87", 0.0, true);
        this.SetCurrentNum("CD3.X88", 0.0, true);
        this.SetCurrentNum("CD3.X89", 0.0, true);
        this.SetCurrentNum("CD3.X90", 0.0, true);
        for (int index = 87; index <= 90; ++index)
        {
          if (index != 89)
            this.SetCurrentNum("CD3.XH" + (object) index, 0.0, true);
        }
        double num = this.FltVal("CD3.X87") - this.FltVal("CD3.X88") - this.FltVal("CD3.X90");
        this.SetCurrentNum("CD3.X91", num < 0.0 ? num * -1.0 : num, true);
        if (num > 0.0)
          this.SetVal("CD3.X92", "To Borrower");
        else
          this.SetVal("CD3.X92", "From Borrower");
      }
      this.CalculateCDPage3AltDidChangeCol((string) null, (string) null);
    }

    private void CalculateCDPage1CashToClose(string id, string val)
    {
      if (this.Val("LE2.X28") == "Y")
        this.SetCurrentNum("CD1.X69", this.FltVal("CD3.X85"), true);
      else
        this.SetCurrentNum("CD1.X69", this.FltVal("CD3.X45"), true);
    }

    private void CalculateCDPage3AlternateCashToCloseFinal(string id, string val)
    {
      this.SetCurrentNum("CD3.X81", this.FltVal("2"), true);
      this.SetCurrentNum("CD3.X82", this.FltVal("CD2.XSTJ"), true);
      this.SetCurrentNum("CD3.X83", this.FltVal("CD2.XBCCBC"), true);
      this.SetCurrentNum("CD3.X84", this.FltVal("CD3.X80"), true);
      double num = (this.IsLocked("CD3.X81") ? this.FltVal("Cd3.X81") : this.FltVal("2")) - (this.IsLocked("CD3.X82") ? this.FltVal("Cd3.X82") : this.FltVal("CD2.XSTJ")) + (this.IsLocked("CD3.X83") ? this.FltVal("Cd3.X83") : this.FltVal("CD2.XBCCBC")) - (this.IsLocked("CD3.X84") ? this.FltVal("Cd3.X84") : this.FltVal("CD3.X80"));
      this.SetCurrentNum("CD3.X85", num < 0.0 ? num * -1.0 : num, true);
      if (num == 0.0)
        this.SetVal("CD3.X86", "From Borrower");
      else if (num >= 0.0)
        this.SetVal("CD3.X86", "To Borrower");
      else
        this.SetVal("CD3.X86", "From Borrower");
      this.CalculateCDPage1CashToClose((string) null, (string) null);
      this.CalculateCDPage3AltDidChangeCol((string) null, (string) null);
    }

    public void CalculateCDPage3StdCashToCloseLoanEstimate(string id, string val)
    {
      IDisclosureTracking2015Log idisclosureTracking2015Log = this.loan.GetLogList().GetLatestIDisclosureTracking2015Log(DisclosureTracking2015Log.DisclosureTrackingType.LE);
      if (idisclosureTracking2015Log != null)
      {
        this.SetCurrentNum("CD3.X93", Utils.ArithmeticRounding(this.Flt(idisclosureTracking2015Log.GetDisclosedField("LE2.XSTJ")), 0), true);
        this.SetCurrentNum("CD3.X94", 0.0, true);
        this.SetCurrentNum("CD3.X95", Utils.ArithmeticRounding(this.Flt(idisclosureTracking2015Log.GetDisclosedField("LE2.X1")), 0), true);
        this.SetCurrentNum("CD3.X96", Utils.ArithmeticRounding(this.Flt(idisclosureTracking2015Log.GetDisclosedField("LE2.X2")), 0), true);
        this.SetCurrentNum("CD3.X97", Utils.ArithmeticRounding(this.Flt(idisclosureTracking2015Log.GetDisclosedField("L128")), 0), true);
        this.SetCurrentNum("CD3.X98", Utils.ArithmeticRounding(this.Flt(idisclosureTracking2015Log.GetDisclosedField("LE2.X3")), 0), true);
        this.SetCurrentNum("CD3.X99", Utils.ArithmeticRounding(this.Flt(idisclosureTracking2015Log.GetDisclosedField("LE2.X100")), 0), true);
        this.SetCurrentNum("CD3.X100", Utils.ArithmeticRounding(this.Flt(idisclosureTracking2015Log.GetDisclosedField("LE2.X4")), 0), true);
        for (int index = 93; index <= 100; ++index)
        {
          if (index != 94)
            this.SetCurrentNum("CD3.XH" + (object) index, this.Flt(idisclosureTracking2015Log.GetDisclosedField("CD3.XH" + (object) index)), true);
        }
        this.SetCurrentNum("CD3.X101", this.FltVal("CD3.X93") - this.FltVal("CD3.X94") - this.FltVal("CD3.X95") + this.FltVal("CD3.X96") - this.FltVal("CD3.X97") - this.FltVal("CD3.X98") - this.FltVal("CD3.X99") - this.FltVal("CD3.X100"), true);
      }
      else
      {
        this.SetCurrentNum("CD3.X93", 0.0, true);
        this.SetCurrentNum("CD3.X94", 0.0, true);
        this.SetCurrentNum("CD3.X95", 0.0, true);
        this.SetCurrentNum("CD3.X96", 0.0, true);
        this.SetCurrentNum("CD3.X97", 0.0, true);
        this.SetCurrentNum("CD3.X98", 0.0, true);
        this.SetCurrentNum("CD3.X99", 0.0, true);
        this.SetCurrentNum("CD3.X100", 0.0, true);
        for (int index = 93; index <= 100; ++index)
        {
          if (index != 94)
            this.SetCurrentNum("CD3.XH" + (object) index, 0.0, true);
        }
        this.SetCurrentNum("CD3.X101", this.FltVal("CD3.X93") - this.FltVal("CD3.X94") - this.FltVal("CD3.X95") + this.FltVal("CD3.X96") - this.FltVal("CD3.X97") - this.FltVal("CD3.X98") - this.FltVal("CD3.X99") - this.FltVal("CD3.X100"), true);
      }
      this.CalculateCDPage3StdDidChangeCol((string) null, (string) null);
    }

    private void CalculateCDPage3StdCashToCloseFinal(string id, string val)
    {
      this.SetCurrentNum("CD3.X102", this.FltVal("CD2.XSTJ"), true);
      this.SetCurrentNum("CD3.X103", this.FltVal("CD2.XBCCBC"), true);
      string str = this.Val("19");
      double num1 = 0.0;
      double num2 = 0.0;
      if (this.Val("LE2.X30") == "Y")
      {
        this.SetCurrentNum("CD3.X104", this.FltVal("NEWHUD.X1585"), true);
        switch (str)
        {
          case "Purchase":
          case "ConstructionOnly":
          case "ConstructionToPermanent":
            this.SetCurrentNum("CD3.X105", this.FltVal("L726") - this.FltVal("140") - this.FltVal("1109"), true);
            this.SetVal("CD3.X107", "0");
            break;
          case "NoCash-Out Refinance":
          case "Cash-Out Refinance":
          case "Other":
            double num3 = Utils.ArithmeticRounding(this.FltVal("1092") - this.FltVal("1109"), 2);
            if (num3 >= 0.0)
            {
              this.SetCurrentNum("CD3.X105", num3, true);
              this.SetVal("CD3.X107", "0");
              break;
            }
            this.SetCurrentNum("CD3.X107", num3 * -1.0);
            this.SetVal("CD3.X105", "0");
            break;
          default:
            this.SetVal("CD3.X105", "0");
            this.SetVal("CD3.X107", "0");
            break;
        }
      }
      else if (this.Val("LE2.X28") != "Y")
      {
        if (str == "Purchase" || str == "Cash-Out Refinance" || str == "NoCash-Out Refinance" || str == "ConstructionOnly" || str == "ConstructionToPermanent")
          num1 = this.FltVal("2") - (this.FltVal("L726") + this.FltVal("L79") + this.FltVal("LE2.X29"));
        if (num1 <= 0.0)
          this.SetCurrentNum("CD3.X104", 0.0, true);
        else if (num1 <= this.FltVal("CD2.XSTJ"))
          this.SetCurrentNum("CD3.X104", num1, true);
        else
          this.SetCurrentNum("CD3.X104", this.FltVal("CD2.XSTJ"), true);
        switch (str)
        {
          case "ConstructionOnly":
          case "ConstructionToPermanent":
          case "Purchase":
            num2 = this.FltVal("L726") + this.FltVal("L79") + this.FltVal("LE2.X29") - (this.FltVal("2") - this.FltVal("CD3.X104"));
            break;
          case "Cash-Out Refinance":
          case "NoCash-Out Refinance":
            num2 = this.FltVal("LE2.X29") - (this.FltVal("2") - this.FltVal("CD3.X104"));
            break;
        }
        this.SetCurrentNum("CD3.X105", num2 <= 0.0 ? 0.0 : num2, true);
        this.SetCurrentNum("CD3.X107", num2 < 0.0 ? num2 * -1.0 : 0.0, true);
      }
      this.SetCurrentNum("CD3.X106", this.FltVal("L128"), true);
      this.SetCurrentNum("CD3.X109", this.FltVal("LE2.X4"), true);
      this.SetCurrentNum("CD3.X45", this.FltVal("CD3.X102") - this.FltVal("CD3.X103") - this.FltVal("CD3.X104") + this.FltVal("CD3.X105") - this.FltVal("CD3.X106") - this.FltVal("CD3.X107") - this.FltVal("CD3.X108") - this.FltVal("CD3.X109"), true);
      this.CalculateCDPage1CashToClose((string) null, (string) null);
      this.CalculateCDPage3StdDidChangeCol((string) null, (string) null);
      this.CalculateCDPage3AltDidChangeCol((string) null, (string) null);
    }

    private void CalculateCDPage3AltDidChangeCol(string id, string val)
    {
      if ((this.IsLocked("CD3.X87") ? (this.FltVal("CD3.X87") != this.FltVal("CD3.X81") ? 1 : 0) : (this.FltVal("CD3.XH87") != this.FltVal("CD3.X81") ? 1 : 0)) != 0)
      {
        this.SetVal("CD3.X118", "YES");
        if (this.FltVal("CD3.X81") > (this.IsLocked("CD3.X87") ? this.FltVal("CD3.X87") : this.FltVal("CD3.XH87")))
          this.SetVal("CD3.X130", "Increased");
        else
          this.SetVal("CD3.X130", "Decreased");
      }
      else
        this.SetVal("CD3.X118", "NO");
      if ((this.IsLocked("CD3.X88") ? (this.FltVal("CD3.X88") != this.FltVal("CD3.X82") ? 1 : 0) : (this.FltVal("CD3.XH88") != this.FltVal("CD3.X82") ? 1 : 0)) != 0 || this.FltVal("FV.X366") > 0.0)
      {
        this.SetVal("CD3.X119", "YES");
        if (this.IsLocked("CD3.X88"))
        {
          this.SetVal("CD3.X131", "");
        }
        else
        {
          IDisclosureTracking2015Log idisclosureTracking2015Log1 = this.loan.GetLogList().GetLatestIDisclosureTracking2015Log(DisclosureTracking2015Log.DisclosureTrackingType.LE);
          IDisclosureTracking2015Log idisclosureTracking2015Log2 = this.loan.GetLogList().GetLatestIDisclosureTracking2015Log(DisclosureTracking2015Log.DisclosureTrackingType.CD);
          if ((idisclosureTracking2015Log1 != null ? Utils.ParseDouble((object) idisclosureTracking2015Log1.GetDisclosedField("LE2.XSTD_DV")) : 0.0) != (idisclosureTracking2015Log2 != null ? Utils.ParseDouble((object) idisclosureTracking2015Log2.GetDisclosedField("CD2.XSTD")) : this.FltVal("CD2.XSTD")) && (idisclosureTracking2015Log1 != null ? Utils.ParseDouble((object) idisclosureTracking2015Log1.GetDisclosedField("LE2.XSTI_DV")) : 0.0) != (idisclosureTracking2015Log2 != null ? Utils.ParseDouble((object) idisclosureTracking2015Log2.GetDisclosedField("CD2.XSTI")) : this.FltVal("CD2.XSTI")))
            this.SetVal("CD3.X131", "Total Loan Costs (D) and Total Other Costs (I)");
          else if ((idisclosureTracking2015Log1 != null ? Utils.ParseDouble((object) idisclosureTracking2015Log1.GetDisclosedField("LE2.XSTD_DV")) : 0.0) != (idisclosureTracking2015Log2 != null ? Utils.ParseDouble((object) idisclosureTracking2015Log2.GetDisclosedField("CD2.XSTD")) : this.FltVal("CD2.XSTD")))
            this.SetVal("CD3.X131", "Total Loan Costs (D)");
          else if ((idisclosureTracking2015Log1 != null ? Utils.ParseDouble((object) idisclosureTracking2015Log1.GetDisclosedField("LE2.XSTI_DV")) : 0.0) != (idisclosureTracking2015Log2 != null ? Utils.ParseDouble((object) idisclosureTracking2015Log2.GetDisclosedField("CD2.XSTI")) : this.FltVal("CD2.XSTI")))
            this.SetVal("CD3.X131", "Total Other Costs (I)");
        }
      }
      else
        this.SetVal("CD3.X119", "NO");
      if (this.FltVal("CD3.X89") != this.FltVal("CD3.X83"))
        this.SetVal("CD3.X120", "YES");
      else
        this.SetVal("CD3.X120", "NO");
      if ((this.IsLocked("CD3.X90") ? (this.FltVal("CD3.X90") != this.FltVal("CD3.X84") ? 1 : 0) : (this.FltVal("CD3.XH90") != this.FltVal("CD3.X84") ? 1 : 0)) != 0)
        this.SetVal("CD3.X121", "YES");
      else
        this.SetVal("CD3.X121", "NO");
      if (this.FltVal("CD3.X91") != this.FltVal("CD3.X85"))
        this.SetVal("CD3.X122", "YES");
      else
        this.SetVal("CD3.X122", "NO");
      if (this.Val("LE2.X30") == "Y")
        this.SetCurrentNum("CD3.X133", this.FltVal("CD3.X104"));
      else if (this.Val("LE2.X28") == "Y")
      {
        double num1 = this.FltVal("2") - this.FltVal("CD3.X84");
        if (num1 <= 0.0)
        {
          this.SetCurrentNum("CD3.X133", 0.0, true);
        }
        else
        {
          double num2 = this.FltVal("CD2.XSTJ") - this.FltVal("CD2.XBCCBC");
          this.SetCurrentNum("CD3.X133", num2 <= 0.0 ? 0.0 : (num1 < num2 ? num1 : num2), true);
        }
      }
      else
        this.SetCurrentNum("CD3.X133", this.FltVal("CD3.X104"));
    }

    private void CalculateCDPage3StdDidChangeCol(string id, string val)
    {
      IDisclosureTracking2015Log disclosureTracking2015Log1 = (IDisclosureTracking2015Log) null;
      IDisclosureTracking2015Log disclosureTracking2015Log2 = (IDisclosureTracking2015Log) null;
      if ((this.IsLocked("CD3.X93") ? (this.FltVal("CD3.X93") != this.FltVal("CD3.X102") ? 1 : 0) : (this.FltVal("CD3.XH93") != this.FltVal("CD3.X102") ? 1 : 0)) != 0 || this.FltVal("FV.X366") > 0.0)
      {
        this.SetVal("CD3.X111", "YES");
        if (this.IsLocked("CD3.X93"))
        {
          this.SetVal("CD3.X123", "");
        }
        else
        {
          if (disclosureTracking2015Log1 == null)
            disclosureTracking2015Log1 = this.loan.GetLogList().GetLatestIDisclosureTracking2015Log(DisclosureTracking2015Log.DisclosureTrackingType.LE);
          if (disclosureTracking2015Log2 == null)
            disclosureTracking2015Log2 = this.loan.GetLogList().GetLatestIDisclosureTracking2015Log(DisclosureTracking2015Log.DisclosureTrackingType.CD);
          if (disclosureTracking2015Log1 != null && disclosureTracking2015Log1.DisclosureType == DisclosureTracking2015Log.DisclosureTypeEnum.Initial && disclosureTracking2015Log2 != null && disclosureTracking2015Log2.DisclosureType == DisclosureTracking2015Log.DisclosureTypeEnum.Initial && Utils.ParseDouble((object) disclosureTracking2015Log1.GetDisclosedField("LE2.XSTD_DV")) != this.FltVal("CD2.XSTD") && Utils.ParseDouble((object) disclosureTracking2015Log1.GetDisclosedField("LE2.XSTI_DV")) != this.FltVal("CD2.XSTI"))
            this.SetVal("CD3.X123", "Total Loan Costs (D) and Total Other Costs (I)");
          else if ((disclosureTracking2015Log1 != null ? Utils.ParseDouble((object) disclosureTracking2015Log1.GetDisclosedField("LE2.XSTD_DV")) : 0.0) != (disclosureTracking2015Log2 != null ? Utils.ParseDouble((object) disclosureTracking2015Log2.GetDisclosedField("CD2.XSTD")) : this.FltVal("CD2.XSTD")) && (disclosureTracking2015Log1 != null ? Utils.ParseDouble((object) disclosureTracking2015Log1.GetDisclosedField("LE2.XSTI_DV")) : 0.0) != (disclosureTracking2015Log2 != null ? Utils.ParseDouble((object) disclosureTracking2015Log2.GetDisclosedField("CD2.XSTI")) : this.FltVal("CD2.XSTI")))
            this.SetVal("CD3.X123", "Total Loan Costs (D) and Total Other Costs (I)");
          else if ((disclosureTracking2015Log1 != null ? Utils.ParseDouble((object) disclosureTracking2015Log1.GetDisclosedField("LE2.XSTD_DV")) : 0.0) != (disclosureTracking2015Log2 != null ? Utils.ParseDouble((object) disclosureTracking2015Log2.GetDisclosedField("CD2.XSTD")) : this.FltVal("CD2.XSTD")))
            this.SetVal("CD3.X123", "Total Loan Costs (D)");
          else if ((disclosureTracking2015Log1 != null ? Utils.ParseDouble((object) disclosureTracking2015Log1.GetDisclosedField("LE2.XSTI_DV")) : 0.0) != (disclosureTracking2015Log2 != null ? Utils.ParseDouble((object) disclosureTracking2015Log2.GetDisclosedField("CD2.XSTI")) : this.FltVal("CD2.XSTI")))
            this.SetVal("CD3.X123", "Total Other Costs (I)");
        }
      }
      else
        this.SetVal("CD3.X111", "NO");
      if (this.FltVal("CD3.X94") != this.FltVal("CD3.X103"))
        this.SetVal("CD3.X112", "YES");
      else
        this.SetVal("CD3.X112", "NO");
      if ((this.IsLocked("CD3.X95") ? (this.FltVal("CD3.X95") != this.FltVal("CD3.X104") ? 1 : 0) : (this.FltVal("CD3.XH95") != this.FltVal("CD3.X104") ? 1 : 0)) != 0)
        this.SetVal("CD3.X134", "YES");
      else
        this.SetVal("CD3.X134", "NO");
      if ((this.IsLocked("CD3.X96") ? (this.FltVal("CD3.X96") != this.FltVal("CD3.X105") ? 1 : 0) : (this.FltVal("CD3.XH96") != this.FltVal("CD3.X105") ? 1 : 0)) != 0)
      {
        this.SetVal("CD3.X113", "YES");
        double num1 = this.FltVal("CD3.X105") - (this.IsLocked("CD3.X96") ? this.FltVal("CD3.X96") : this.FltVal("CD3.XH96"));
        if (num1 > 0.0)
          this.SetVal("CD3.X124", "Increased");
        else if (num1 < 0.0)
          this.SetVal("CD3.X124", "Decreased");
        else
          this.SetVal("CD3.X124", "Decreased");
        if (disclosureTracking2015Log1 == null)
          disclosureTracking2015Log1 = this.loan.GetLogList().GetLatestIDisclosureTracking2015Log(DisclosureTracking2015Log.DisclosureTrackingType.LE);
        if (disclosureTracking2015Log2 == null)
          disclosureTracking2015Log2 = this.loan.GetLogList().GetLatestIDisclosureTracking2015Log(DisclosureTracking2015Log.DisclosureTrackingType.CD);
        double num2 = disclosureTracking2015Log1 != null ? Utils.ParseDouble((object) disclosureTracking2015Log1.GetDisclosedField("L726")) : 0.0;
        double num3 = disclosureTracking2015Log2 != null ? Utils.ParseDouble((object) disclosureTracking2015Log2.GetDisclosedField("L726")) : this.FltVal("L726");
        double num4 = disclosureTracking2015Log1 != null ? Utils.ParseDouble((object) disclosureTracking2015Log1.GetDisclosedField("2")) : 0.0;
        double num5 = disclosureTracking2015Log2 != null ? Utils.ParseDouble((object) disclosureTracking2015Log2.GetDisclosedField("2")) : this.FltVal("2");
        if (num3 != num2 && num5 != num4)
          this.SetVal("CD3.X125", "K and L");
        else if (num3 != num2)
          this.SetVal("CD3.X125", "K");
        else if (num5 != num4)
          this.SetVal("CD3.X125", "L");
        else
          this.SetVal("CD3.X125", "");
      }
      else
        this.SetVal("CD3.X113", "NO");
      if ((this.IsLocked("CD3.X97") ? (this.FltVal("CD3.X97") != this.FltVal("CD3.X106") ? 1 : 0) : (this.FltVal("CD3.XH97") != this.FltVal("CD3.X106") ? 1 : 0)) != 0)
      {
        this.SetVal("CD3.X114", "YES");
        if (this.FltVal("CD3.X106") > (this.IsLocked("CD3.X97") ? this.FltVal("CD3.X97") : this.FltVal("CD3.XH97")))
          this.SetVal("CD3.X126", "Increased");
        else
          this.SetVal("CD3.X126", "Decreased");
      }
      else
        this.SetVal("CD3.X114", "NO");
      if ((this.IsLocked("CD3.X98") ? (this.FltVal("CD3.X98") != this.FltVal("CD3.X107") ? 1 : 0) : (this.FltVal("CD3.XH98") != this.FltVal("CD3.X107") ? 1 : 0)) != 0)
      {
        this.SetVal("CD3.X115", "YES");
        if (this.FltVal("CD3.X107") > (this.IsLocked("CD3.X98") ? this.FltVal("CD3.X98") : this.FltVal("CD3.XH98")))
          this.SetVal("CD3.X127", "Increased");
        else
          this.SetVal("CD3.X127", "Decreased");
      }
      else
        this.SetVal("CD3.X115", "NO");
      if ((this.IsLocked("CD3.X99") ? (this.FltVal("CD3.X99") != this.FltVal("CD3.X108") ? 1 : 0) : (this.FltVal("CD3.XH99") != this.FltVal("CD3.X108") ? 1 : 0)) != 0)
      {
        this.SetVal("CD3.X116", "YES");
        this.SetVal("CD3.X1542", "Y");
        if (this.FltVal("CD3.X108") > (this.IsLocked("CD3.X99") ? this.FltVal("CD3.X99") : this.FltVal("CD3.XH99")))
          this.SetVal("CD3.X128", "Increased");
        else
          this.SetVal("CD3.X128", "Decreased");
      }
      else
      {
        this.SetVal("CD3.X116", "NO");
        this.SetVal("CD3.X1542", "N");
      }
      if ((this.IsLocked("CD3.X100") ? (this.FltVal("CD3.X100") != this.FltVal("CD3.X109") ? 1 : 0) : (this.FltVal("CD3.XH100") != this.FltVal("CD3.X109") ? 1 : 0)) != 0)
        this.SetVal("CD3.X117", "YES");
      else
        this.SetVal("CD3.X117", "NO");
    }

    private void calculateWillHaveEscrowAccount(string id, string val)
    {
      DateTime date1 = Utils.ParseDate((object) this.Val("HUD68"));
      double num1 = 0.0;
      double num2 = 0.0;
      string str1 = this.Val("CD4.X51");
      DateTime dateTime = !this.UseNewCompliance(18.3M) || str1 != "1stPaymentDate" ? Utils.ParseDate((object) this.Val("748")) : Utils.ParseDate((object) this.Val("682"));
      if (dateTime != DateTime.MinValue && date1 != DateTime.MinValue)
      {
        dateTime = dateTime.AddMonths(12);
        if (dateTime >= date1)
        {
          int num3;
          if (this.Val("19") == "ConstructionOnly")
          {
            int num4 = this.IntVal("1176");
            if (num4 < 12)
            {
              num3 = num4;
            }
            else
            {
              num3 = dateTime.Year * 12 + (dateTime.Month - 1) - (date1.Year * 12 + (date1.Month - 1)) + 1;
              if (dateTime.Day <= date1.Day)
                --num3;
            }
          }
          else
          {
            num3 = dateTime.Year * 12 + (dateTime.Month - 1) - (date1.Year * 12 + (date1.Month - 1)) + 1;
            if (dateTime.Day <= date1.Day)
              --num3;
          }
          if (this.Val("NEWHUD.X337") != "Y")
            num1 += this.FltVal("231");
          else
            num2 += this.FltVal("231");
          if (this.Val("NEWHUD.X339") != "Y")
            num1 += this.FltVal("230");
          else
            num2 += this.FltVal("230");
          if (this.Val("NEWHUD.X338") != "Y")
            num1 += this.FltVal("235");
          else
            num2 += this.FltVal("235");
          if (this.Val("NEWHUD.X1726") != "Y")
            num1 += this.FltVal("L268");
          else
            num2 += this.FltVal("L268");
          if (this.Val("NEWHUD.X340") != "Y")
            num1 += this.FltVal("1630");
          else
            num2 += this.FltVal("1630");
          if (this.Val("NEWHUD.X341") != "Y")
            num1 += this.FltVal("253");
          else
            num2 += this.FltVal("253");
          if (this.Val("NEWHUD.X342") != "Y")
            num1 += this.FltVal("254");
          else
            num2 += this.FltVal("254");
          double num5 = num1 + this.FltVal("233");
          if (this.UseNewCompliance(18.3M))
          {
            if (this.Val("NEWHUD.X1728") == "Y")
              num2 += this.FltVal("232");
            else
              num5 += this.FltVal("232");
            if (this.Val("NEWHUD.X343") == "Y")
              num2 += this.FltVal("NEWHUD.X1707");
          }
          num1 = num5 * (double) num3;
          num2 *= (double) num3;
        }
      }
      if (id == "DISCLOSURE.X913" && val == "Y")
        this.SetVal("DISCLOSURE.X339", "N");
      else if (id == "DISCLOSURE.X339" && val == "Y")
        this.SetVal("DISCLOSURE.X913", "N");
      if (this.UseNew2015GFEHUD)
      {
        string str2 = this.Val("19");
        DateTime date2 = Utils.ParseDate((object) this.Val("682"));
        if (this.Val("NEWHUD.X337") == "Y" || this.Val("NEWHUD.X338") == "Y" || this.Val("NEWHUD.X339") == "Y" || this.Val("NEWHUD.X1726") == "Y" || this.Val("CD4.X4") == "Y" || this.Val("CD4.X5") == "Y" || this.Val("CD4.X6") == "Y" || this.Val("NEWHUD.X1728") == "Y" || this.UseNewCompliance(18.3M) && this.Val("NEWHUD.X343") == "Y")
        {
          this.SetVal("CD4.X8", "");
          if (this.UseNewCompliance(18.3M))
            this.SetVal("CD4.X9", "Y");
          else if (this.FltVal("HUD66") == 0.0)
            this.SetVal("CD4.X9", "");
          else
            this.SetVal("CD4.X9", "Y");
          string str3 = this.Val("CD4.X51");
          if (str2 == "ConstructionToPermanent" && date1.Date != date2.Date && (str3 == "ConsummationDate" && this.IntVal("1176") >= 12 || str3 == "1stPaymentDate" && this.IntVal("1176") > 12))
            this.SetVal("CD4.X40", "");
          else
            this.SetCurrentNum("CD4.X40", num2);
          if (str2 == "ConstructionToPermanent" && date1.Date == DateTime.MinValue)
            this.SetVal("CD4.X41", "");
          else
            this.SetCurrentNum("CD4.X41", num1);
          this.SetVal("DISCLOSURE.X913", "N");
          this.SetVal("DISCLOSURE.X914", "");
          this.SetVal("CD4.X7", "N");
        }
        else if (this.Val("NEWHUD.X337") == "" && this.Val("NEWHUD.X338") == "" && this.Val("NEWHUD.X339") == "" && this.Val("NEWHUD.X1726") == "" && this.Val("CD4.X4") == "" && this.Val("CD4.X5") == "" && this.Val("CD4.X6") == "" && this.Val("NEWHUD.X1728") == "" && this.FltVal("233") == 0.0 && this.UseNewCompliance(18.3M) && this.Val("NEWHUD.X343") == "")
        {
          this.SetVal("CD4.X7", "");
          this.SetVal("CD4.X8", "");
          this.SetVal("CD4.X9", "");
          this.SetVal("CD4.X40", "");
          this.SetVal("CD4.X41", "");
          this.SetVal("DISCLOSURE.X913", "");
          this.SetVal("DISCLOSURE.X914", "");
        }
        else
        {
          this.SetVal("CD4.X9", "N");
          if (this.Val("DISCLOSURE.X913") == "Y")
            this.SetVal("DISCLOSURE.X339", "N");
          this.SetVal("CD4.X40", "");
          this.SetVal("CD4.X41", "");
          this.SetCurrentNum("CD4.X8", num1);
        }
        double num6 = this.FltVal("558") + (this.Val("NEWHUD2.X133") == "Y" ? this.FltVal("NEWHUD2.X2544") - this.FltVal("NEWHUD2.X2573") : 0.0) + (this.Val("NEWHUD2.X4769") == "Y" ? this.FltVal("NEWHUD2.X2577") - this.FltVal("NEWHUD2.X2606") : 0.0) + (this.Val("NEWHUD2.X134") == "Y" ? this.FltVal("NEWHUD2.X2610") - this.FltVal("NEWHUD2.X2639") : 0.0) + (this.Val("NEWHUD2.X135") == "Y" ? this.FltVal("NEWHUD2.X2643") - this.FltVal("NEWHUD2.X2672") : 0.0) + (this.Val("NEWHUD2.X136") == "Y" ? this.FltVal("NEWHUD2.X2676") - this.FltVal("NEWHUD2.X2705") : 0.0) + (this.Val("NEWHUD2.X137") == "Y" ? this.FltVal("NEWHUD2.X2709") - this.FltVal("NEWHUD2.X2738") : 0.0) + (this.Val("NEWHUD2.X138") == "Y" ? this.FltVal("NEWHUD2.X2742") - this.FltVal("NEWHUD2.X2771") : 0.0) + (this.Val("NEWHUD2.X139") == "Y" ? this.FltVal("NEWHUD2.X2775") - this.FltVal("NEWHUD2.X2804") : 0.0) + (this.Val("NEWHUD2.X140") == "Y" ? this.FltVal("NEWHUD2.X2808") - this.FltVal("NEWHUD2.X2837") : 0.0);
        if (num6 <= 0.0)
          this.SetVal("CD4.X44", "");
        else
          this.SetCurrentNum("CD4.X44", num6);
      }
      else
      {
        if (!(this.Val("DISCLOSURE.X913") != "Y"))
          return;
        this.SetVal("DISCLOSURE.X914", "");
      }
    }

    private void populateCDPage4PartialPaymentIndicators(string id, string val)
    {
      if (id == "CD4.X43" && val == "none" && (this.Val("CD4.X3") == "apply partial payment" || this.Val("CD4.X42") == "hold until complete amount"))
      {
        this.SetVal("CD4.X3", "");
        this.SetVal("CD4.X42", "");
      }
      else
      {
        if (!(this.Val("CD4.X43") == "none") || (!(id == "CD4.X3") || !(val == "apply partial payment")) && (!(id == "CD4.X42") || !(val == "hold until complete amount")))
          return;
        this.SetVal("CD4.X43", "");
      }
    }

    private void calculatePropertyInsuranceTaxIndicators(string id, string val)
    {
      if (!this.UseNew2015GFEHUD)
        return;
      if (this.Val("231") != "" || this.Val("L268") != "" || this.Val("NEWHUD2.X124") == "Y" || this.Val("NEWHUD2.X127") == "Y" || this.Val("NEWHUD2.X130") == "Y")
        this.SetVal("NEWHUD.X349", "Y");
      else
        this.SetVal("NEWHUD.X349", "N");
      if (this.Val("230") != "" || this.Val("235") != "" || this.Val("NEWHUD2.X125") == "Y" || this.Val("NEWHUD2.X128") == "Y" || this.Val("NEWHUD2.X131") == "Y")
        this.SetVal("NEWHUD.X350", "Y");
      else
        this.SetVal("NEWHUD.X350", "N");
      if (this.Val("NEWHUD2.X126") == "Y" || this.Val("NEWHUD2.X129") == "Y" || this.Val("NEWHUD2.X132") == "Y" || this.FltVal("233") > 0.0)
      {
        this.SetVal("NEWHUD.X351", "Y");
      }
      else
      {
        this.SetVal("NEWHUD.X351", "N");
        this.SetVal("NEWHUD.X78", "");
      }
    }

    private void calculateCDRedisclosureFlag(string id, string val)
    {
      if (!Utils.CheckIf2015RespaTila(this.loan.GetField("3969")) || !Utils.IsDate((object) this.loan.GetField("CD1.X1")) || Utils.ParseDate((object) this.loan.GetField("CD1.X1")) == DateTime.MinValue)
        return;
      DateTime date1 = Utils.ParseDate((object) this.loan.GetSimpleField("761"));
      DateTime date2 = Utils.ParseDate((object) this.loan.GetSimpleField("CD1.X47"));
      if (date1 == DateTime.MinValue)
        this.SetVal("3201", "N");
      else if (date2 == DateTime.MinValue || date2.Date > date1.Date)
        this.SetVal("3201", "N");
      else if (date2.Date < date1.Date)
      {
        this.SetVal("3201", "Y");
      }
      else
      {
        IDisclosureTracking2015Log idisclosureTracking2015Log = this.loan.GetLogList().GetLatestIDisclosureTracking2015Log(DisclosureTracking2015Log.DisclosureTrackingType.CD);
        DateTime date3 = Utils.ParseDate((object) this.loan.GetSimpleField("3200"));
        if (idisclosureTracking2015Log == null)
          this.SetVal("3201", "N");
        else if (date3 == DateTime.MinValue)
          this.SetVal("3201", "N");
        else if (date3 > Utils.ParseDate((object) Utils.DateTimeToUTCString(idisclosureTracking2015Log.DateAdded)))
          this.SetVal("3201", "Y");
        else
          this.SetVal("3201", "N");
      }
    }

    private void calculateLERedisclosureFlag(string id, string val)
    {
      if (!Utils.CheckIf2015RespaTila(this.loan.GetField("3969")) || Utils.IsDate((object) this.loan.GetField("CD1.X1")) || Utils.ParseDate((object) this.loan.GetField("CD1.X1")) != DateTime.MinValue)
        return;
      DateTime date1 = Utils.ParseDate((object) this.loan.GetSimpleField("761"));
      DateTime date2 = Utils.ParseDate((object) this.loan.GetSimpleField("LE1.X33"));
      if (date1 == DateTime.MinValue)
        this.SetVal("3201", "N");
      else if (date2 == DateTime.MinValue || date2.Date > date1.Date)
        this.SetVal("3201", "N");
      else if (date2.Date < date1.Date)
      {
        this.SetVal("3201", "Y");
      }
      else
      {
        IDisclosureTracking2015Log idisclosureTracking2015Log = this.loan.GetLogList().GetLatestIDisclosureTracking2015Log(DisclosureTracking2015Log.DisclosureTrackingType.LE);
        DateTime date3 = Utils.ParseDate((object) this.loan.GetSimpleField("3200"));
        if (idisclosureTracking2015Log == null)
          this.SetVal("3201", "N");
        else if (date3 == DateTime.MinValue)
          this.SetVal("3201", "N");
        else if (date3 > Utils.ParseDate((object) Utils.DateTimeToUTCString(idisclosureTracking2015Log.DateAdded)))
          this.SetVal("3201", "Y");
        else
          this.SetVal("3201", "N");
      }
    }

    private void calculateLECashToCloseSellerCredit(string id, string val)
    {
      double val1 = this.FltVal("143");
      if (this.Val("4796") == "Y")
      {
        val1 += this.FltVal("4795");
      }
      else
      {
        if (this.Val("202") == "SellerCredit")
          val1 += this.FltVal("141");
        if (this.Val("1091") == "SellerCredit")
          val1 += this.FltVal("1095");
        if (this.Val("1106") == "SellerCredit")
          val1 += this.FltVal("1115");
        if (this.Val("1646") == "SellerCredit")
          val1 += this.FltVal("1647");
      }
      this.SetCurrentNum("LE2.X100", Utils.ArithmeticRounding(val1, 0), true);
      this.SetCurrentNum("LE2.X100DV", this.IsLocked("LE2.X100") ? this.FltVal("LE2.X100") : val1, true);
    }

    private void calculateCDPage3StdCashToCloseSellerCredit(string id, string val)
    {
      double num = 0.0;
      if (this.Val("4796") == "Y")
      {
        num += this.FltVal("4795");
      }
      else
      {
        if (this.Val("202") == "SellerCredit")
          num += this.FltVal("141");
        if (this.Val("1091") == "SellerCredit")
          num += this.FltVal("1095");
        if (this.Val("1106") == "SellerCredit")
          num += this.FltVal("1115");
        if (this.Val("1646") == "SellerCredit")
          num += this.FltVal("1647");
      }
      this.SetCurrentNum("CD3.X108", num, true);
    }

    private void refreshFormList(string id, string val) => this.loan.ChangeFormVersion();

    private void calculateLE3OtherConsiderationAssumptions(string id, string val)
    {
      if (!this.UseNew2015GFEHUD)
        return;
      switch (id)
      {
        case "677":
          switch (this.Val("677"))
          {
            case "May_SubjectToConditions":
              this.SetVal("LE3.X12", "WillAllowAssumption");
              return;
            case "May_not":
              this.SetVal("LE3.X12", "WillNotAllowAssumption");
              return;
            default:
              this.SetVal("LE3.X12", "");
              return;
          }
        case "LE3.X12":
          switch (this.Val("LE3.X12"))
          {
            case "WillAllowAssumption":
              this.SetVal("677", "May_SubjectToConditions");
              return;
            case "WillNotAllowAssumption":
              this.SetVal("677", "May_not");
              return;
            default:
              this.SetVal("677", "");
              return;
          }
      }
    }

    private void copyLoanNumToLoanIDNum(string id, string val)
    {
      if (this.Val("2626") == "Banked - Retail" || this.Val("2626") == "Banked - Wholesale")
        this.SetVal("4063", this.Val("364"));
      else
        this.SetVal("4063", "");
    }

    private void calculateSellerNames(string id, string val)
    {
      string val1 = string.Empty;
      if ((this.Val("638") ?? "") != string.Empty)
        val1 = this.Val("638");
      if ((this.Val("VEND.X412") ?? "") != string.Empty)
        val1 = val1 + (val1 != string.Empty ? " and " : "") + this.Val("VEND.X412");
      this.SetVal("CD1.X2", val1);
    }

    private void setCustomizedProjectedPaymentTable(string id, string val)
    {
      if (this.UseNew2015GFEHUD)
      {
        if (!(this.Val("19") == "Other"))
          return;
        this.SetVal("PAYMENTTABLE.CUSTOMIZE", "Y");
        this.SetVal("LOANTERMTABLE.CUSTOMIZE", "Y");
      }
      else
      {
        this.SetVal("PAYMENTTABLE.CUSTOMIZE", "");
        this.SetVal("LOANTERMTABLE.CUSTOMIZE", "");
      }
    }

    private void setAppliedCureAmount(string id, string val)
    {
      this.SetVal("CD3.X129", this.loan.GetField("FV.X396"));
      this.calObjs.NewHudCal.CalcClosingCost(id, val);
      new UCDXmlExporter(this.loan).TriggerCalculation();
      this.CalculateCDPage3StdCashToCloseFinal(id, val);
    }

    private void calculate60Payments(string id, string val)
    {
      bool flag = this.UseNoPayment(0.0);
      if (this.calObjs.RegzCal.UseWorstCaseScenario || this.calObjs.RegzCal.UseBestCaseScenario || this.calObjs.RegzCal.TotalPrincipalIn5Years == 0.0 && this.calObjs.RegzCal.TotalInterestIn5Years == 0.0 && !flag)
        return;
      string str1 = this.Val("1172");
      string str2 = this.Val("19");
      double num1 = this.FltVal("334");
      double val1;
      if (this.UseNewCompliance(18.3M))
      {
        if (str1 == "FarmersHomeAdministration")
        {
          val1 = this.calObjs.RegzCal.TotalPrincipalIn5Years + this.calObjs.RegzCal.TotalInterestIn5Years + this.calObjs.RegzCal.TotalMIIn5Years + this.FltVal("LE2.XSTD_DV") + num1 + (this.FltVal("561") - this.FltVal("NEWHUD2.X2177")) + this.FltVal("338") + (this.FltVal("563") - this.FltVal("NEWHUD2.X2606")) + this.FltVal("NEWHUD.X1708") + (this.FltVal("NEWHUD.X1714") - this.FltVal("NEWHUD2.X2837"));
        }
        else
        {
          switch (str2)
          {
            case "ConstructionToPermanent":
              val1 = this.calObjs.RegzCal.TotalPrincipalIn5Years + this.calObjs.RegzCal.TotalInterestIn5Years + this.calObjs.RegzCal.TotalMIIn5Years + this.FltVal("4088") + this.FltVal("LE2.XSTD_DV") + num1 + (this.FltVal("561") - this.FltVal("NEWHUD2.X2177")) + this.FltVal("338") + (this.FltVal("563") - this.FltVal("NEWHUD2.X2606")) + this.FltVal("NEWHUD.X1708") + (this.FltVal("NEWHUD.X1714") - this.FltVal("NEWHUD2.X2837"));
              break;
            case "ConstructionOnly":
              val1 = this.calObjs.RegzCal.TotalPrincipalIn5Years + this.calObjs.RegzCal.TotalMIIn5Years + this.FltVal("4088") + this.FltVal("LE2.XSTD_DV") + num1 + (this.FltVal("561") - this.FltVal("NEWHUD2.X2177")) + this.FltVal("338") + (this.FltVal("563") - this.FltVal("NEWHUD2.X2606")) + this.FltVal("NEWHUD.X1708") + (this.FltVal("NEWHUD.X1714") - this.FltVal("NEWHUD2.X2837"));
              break;
            default:
              val1 = this.calObjs.RegzCal.TotalPrincipalIn5Years + this.calObjs.RegzCal.TotalInterestIn5Years + this.calObjs.RegzCal.TotalMIIn5Years + this.FltVal("LE2.XSTD_DV") + num1 + (this.FltVal("561") - this.FltVal("NEWHUD2.X2177")) + this.FltVal("338") + (this.FltVal("563") - this.FltVal("NEWHUD2.X2606")) + this.FltVal("NEWHUD.X1708") + (this.FltVal("NEWHUD.X1714") - this.FltVal("NEWHUD2.X2837"));
              break;
          }
        }
        if ((str2 == "ConstructionToPermanent" || str2 == "ConstructionOnly") && str1 != "FarmersHomeAdministration")
        {
          val1 += this.FltVal("NEWHUD2.X4662") + this.FltVal("NEWHUD2.X4685") + this.FltVal("NEWHUD2.X4708") + this.FltVal("NEWHUD2.X4731");
          if (this.Val("NEWHUD2.X4681") != "Y")
            val1 += this.FltVal("NEWHUD2.X4663");
          if (this.Val("NEWHUD2.X4704") != "Y")
            val1 += this.FltVal("NEWHUD2.X4686");
          if (this.Val("NEWHUD2.X4727") != "Y")
            val1 += this.FltVal("NEWHUD2.X4709");
          if (this.Val("NEWHUD2.X4750") != "Y")
            val1 += this.FltVal("NEWHUD2.X4732");
        }
        if (str1 != "VA")
          val1 += this.FltVal("3971");
      }
      else
      {
        double num2;
        switch (str2)
        {
          case "ConstructionToPermanent":
            num2 = this.calObjs.RegzCal.TotalPrincipalIn5Years + this.calObjs.RegzCal.TotalInterestIn5Years + this.FltVal("4088") + this.calObjs.RegzCal.TotalMIIn5Years + this.FltVal("LE2.XSTD_DV") + this.FltVal("334") + this.FltVal("338") + this.FltVal("NEWHUD.X1708") + this.FltVal("3971");
            break;
          case "ConstructionOnly":
            num2 = this.calObjs.RegzCal.TotalPrincipalIn5Years + this.FltVal("4088") + this.calObjs.RegzCal.TotalMIIn5Years + this.FltVal("LE2.XSTD_DV") + this.FltVal("334") + this.FltVal("338") + this.FltVal("NEWHUD.X1708") + this.FltVal("3971");
            break;
          default:
            num2 = this.calObjs.RegzCal.TotalPrincipalIn5Years + this.calObjs.RegzCal.TotalInterestIn5Years + this.calObjs.RegzCal.TotalMIIn5Years + this.FltVal("LE2.XSTD_DV") + this.FltVal("334") + this.FltVal("338") + this.FltVal("NEWHUD.X1708") + this.FltVal("3971");
            break;
        }
        val1 = num2 + (this.Val("NEWHUD2.X2175") == "Y" ? this.FltVal("561") : 0.0);
      }
      double num3;
      string val2;
      if (val1 != 0.0)
      {
        num3 = Utils.ArithmeticRounding(val1, 0);
        val2 = num3.ToString();
      }
      else
        val2 = flag ? "0" : "";
      this.SetVal("LE3.X17", val2);
      string val3;
      if (this.calObjs.RegzCal.TotalPrincipalIn5Years != 0.0)
      {
        num3 = Utils.ArithmeticRounding(this.calObjs.RegzCal.TotalPrincipalIn5Years, 0);
        val3 = num3.ToString();
      }
      else
        val3 = flag ? "0" : "";
      this.SetVal("LE3.X18", val3);
    }

    private void calculateSetExcludeBorrowerClosingCosts(string id, string val)
    {
      if (!(this.Val("CD3.X137") == ""))
        return;
      this.SetVal("CD3.X137", "Y");
    }

    private void calculateLoanEstimate_ThirdPartyPaymentsNotOtherwiseDisclosed(
      string id,
      string val)
    {
      if (this.Val("LE2.X101") == "Y")
        this.SetVal("LE2.X29", "");
      else
        this.SetCurrentNum("LE2.X29", this.FltVal("CD3.X80"), true);
    }

    private void calculateLoanEstimate_IncludePayoffs(string id, string val)
    {
      if (!(this.Val("LE2.X28") == "Y") && !(this.Val("19") != "Purchase"))
        return;
      this.SetVal("LE2.X101", "");
    }

    private void calculateLoanEstimate_ClosingCostsFinanced(string id, string val)
    {
      string str = this.Val("19");
      if (this.Val("LE2.X30") == "Y")
        this.SetCurrentNum("LE2.X1", this.FltVal("NEWHUD.X1585"), true);
      else if (this.Val("LE2.X28") != "Y")
      {
        if (str == "Purchase" || str == "Cash-Out Refinance" || str == "NoCash-Out Refinance" || str == "ConstructionOnly" || str == "ConstructionToPermanent")
        {
          double num = Utils.ArithmeticRounding(this.FltVal("2") - (this.FltVal("L726") + this.FltVal("L79") + this.FltVal("LE2.X29")), 2);
          if (num <= 0.0)
            this.SetCurrentNum("LE2.X1", 0.0, true);
          else
            this.SetCurrentNum("LE2.X1", num < this.FltVal("LE2.XSTJ") ? num : this.FltVal("LE2.XSTJ"), true);
        }
      }
      else
      {
        double num1 = this.FltVal("1092");
        if (num1 == 0.0)
          num1 = this.FltVal("LE2.X31");
        double num2 = Utils.ArithmeticRounding(this.FltVal("2") - num1, 2);
        if (num2 <= 0.0)
          this.SetCurrentNum("LE2.X1", 0.0, true);
        else
          this.SetCurrentNum("LE2.X1", num2 < this.FltVal("LE2.XSTJ") ? num2 : this.FltVal("LE2.XSTJ"), true);
      }
      this.calculateLoanEstimate_EstimatedCashToClose((string) null, (string) null);
    }

    private void setReasonForChangeCircumstances(string id, string val)
    {
      if (id != "3168" && id != "3169" && id != "LE1.X90" && id != "CD1.X61" && id != "CD1.X64" && id != "CD1.X70")
        return;
      bool flag = id == "3168" || id == "3169" || id == "LE1.X90";
      string empty = string.Empty;
      string val1;
      if (flag)
      {
        val1 = this.Val("LE1.X85");
        for (int index = 78; index <= 84; ++index)
        {
          if (this.IsLocked("LE1.X" + (object) index))
          {
            this.RemoveLock("LE1.X" + (object) index);
            this.SetVal("LE1.X" + (object) index, "");
            if (index == 84)
              this.SetVal("LE1.X85", "");
          }
        }
      }
      else
      {
        val1 = this.Val("CD1.X60");
        for (int index = 52; index <= 68; ++index)
        {
          if (index == 60)
            index = 66;
          if (this.IsLocked("CD1.X" + (object) index))
          {
            this.RemoveLock("CD1.X" + (object) index);
            this.SetVal("CD1.X" + (object) index, "");
            if (index == 59)
              this.SetVal("CD1.X60", "");
          }
        }
      }
      string str1 = this.Val("4461");
      foreach (string str2 in this.Val(flag ? "LE1.X90" : "CD1.X70").Split(','))
      {
        string id1 = "";
        if (!(str1 == "Y"))
        {
          switch (str2)
          {
            case "1":
              id1 = flag ? "LE1.X78" : "CD1.X55";
              break;
            case "10":
              if (!flag)
              {
                id1 = "CD1.X56";
                break;
              }
              break;
            case "11":
              if (!flag)
              {
                id1 = "CD1.X57";
                break;
              }
              break;
            case "12":
              if (!flag)
              {
                id1 = "CD1.X58";
                break;
              }
              break;
            case "13":
              id1 = flag ? "LE1.X84" : "CD1.X59";
              break;
            case "2":
              id1 = flag ? "LE1.X79" : "CD1.X68";
              break;
            case "3":
              id1 = flag ? "LE1.X80" : "CD1.X66";
              break;
            case "4":
              id1 = flag ? "LE1.X81" : "CD1.X67";
              break;
            case "5":
              if (flag)
              {
                id1 = "LE1.X82";
                break;
              }
              break;
            case "6":
              if (flag)
              {
                id1 = "LE1.X83";
                break;
              }
              break;
            case "7":
              if (!flag)
              {
                id1 = "CD1.X52";
                break;
              }
              break;
            case "8":
              if (!flag)
              {
                id1 = "CD1.X53";
                break;
              }
              break;
            case "9":
              if (!flag)
              {
                id1 = "CD1.X54";
                break;
              }
              break;
          }
          if (id1 != "")
          {
            if (this.Val(id1) != "Y")
              this.SetVal(id1, "Y");
            if (!this.IsLocked(id1))
              this.AddLock(id1);
          }
        }
      }
      this.SetVal(flag ? "LE1.X85" : "CD1.X60", val1);
    }

    private void calculateMICReference(string id, string val)
    {
      string str = this.Val("1172");
      if (!(str == "FHA") && !(str == "VA") && !(str == "FarmersHomeAdministration"))
        return;
      switch (id)
      {
        case "1172":
        case "CD1.X71":
          this.SetVal("1040", this.Val("CD1.X71"));
          break;
        case "1040":
          this.SetVal("CD1.X71", this.Val("1040"));
          break;
      }
    }

    private void setDefaultValuesForClosingCostExpiration(string id, string val)
    {
      string policySetting = (string) this.sessionObjects.StartupInfo.PolicySettings[(object) "Policies.CDExpirationTime"];
      if (policySetting != null)
      {
        this.SetVal("LE1.X8", policySetting);
        if (this.Val("3164") != "Y")
          this.SetVal("LE1.XD8", this.Val("LE1.X8"));
        else
          this.SetVal("LE1.XD8", "");
      }
      string expirationTimeZone = this.GetClosingCostEstimateExpirationTimeZone(Utils.ParseDate((object) this.Val("LE1.X28")));
      if (string.IsNullOrEmpty(expirationTimeZone))
        return;
      this.SetVal("LE1.X9", expirationTimeZone);
      if (this.Val("3164") != "Y")
        this.SetVal("LE1.XD9", this.Val("LE1.X9"));
      else
        this.SetVal("LE1.XD9", "");
    }

    private string GetClosingCostEstimateExpirationTimeZone(DateTime closingCostExpirationDate)
    {
      bool isDaylightSavingTime = !(closingCostExpirationDate == DateTime.MinValue) && System.TimeZoneInfo.Local.IsDaylightSavingTime(closingCostExpirationDate);
      string timezone = this.Val("LE1.XG9");
      return string.IsNullOrEmpty(timezone) ? Utils.TransformSettingTimezoneToStandardTimezone((string) this.sessionObjects.StartupInfo.PolicySettings[(object) "Policies.CDExpirationTimeZone"], isDaylightSavingTime) : Utils.TransformTimezoneToStandardTimezone(timezone, isDaylightSavingTime);
    }

    private void setDefaultValuesForRateLockExpiration(string id, string val)
    {
      string policySetting1 = (string) this.sessionObjects.StartupInfo.PolicySettings[(object) "Policies.RateLockExpirationTime"];
      if (policySetting1 != null)
        this.SetVal("LE1.X6", policySetting1);
      string policySetting2 = (string) this.sessionObjects.StartupInfo.PolicySettings[(object) "Policies.RateLockExpirationTimeZone"];
      string empty = string.Empty;
      DateTime date = Utils.ParseDate((object) this.Val("762"));
      string val1 = !(date != DateTime.MinValue) ? Utils.TransformSettingTimezoneToStandardTimezone(policySetting2, false) : Utils.TransformSettingTimezoneToStandardTimezone(policySetting2, System.TimeZoneInfo.Local.IsDaylightSavingTime(date));
      if (!(val1 != string.Empty))
        return;
      this.SetVal("LE1.X7", val1);
    }

    private void calculateAdjustmentsLineK04(string id, string val)
    {
      this.aggregateFeesForCD3UCD(id, val, "L84", "L85", 140, 220, 4, false, -1, -1);
    }

    private void calculateAdjustmentsLineK05_06_07(string id, string val)
    {
      this.twoWayCopyforCD3UCD(id, val, "L88", "L89", "CD3.X223", "CD3.X224", "CD3.X225", "CD3.X1507");
      this.twoWayCopyforCD3UCD(id, val, "CD3.X2", "CD3.X3", "CD3.X227", "CD3.X228", "CD3.X229", "CD3.X1508");
      this.aggregateFeesForCD3UCD(id, val, "CD3.X4", "CD3.X5", 232, 252, 4, true, 1509, 1514);
    }

    private void calculateAdjustmentsLineL4_L6_L7(string id, string val)
    {
      this.aggregateFeesForCD3UCD(id, val, "L134", "L135", 280, 283, 3, true, 1515, 1506);
      this.twoWayCopyforCD3UCD(id, val, "CD3.X9", "CD3.X10", "CD3.X285", "CD3.X286", "CD3.X288", "CD3.X1517");
      this.aggregateFeesForCD3UCD(id, val, "CD3.X11", "CD3.X12", 291, 306, 5, true, 1518, 1521);
    }

    private void calculateAdjustmentsLineL8_L9_L10_L11(string id, string val)
    {
      this.twoWayCopyforCD3UCD(id, val, "CD3.X13", "CD3.X14", "CD3.X310", "CD3.X311", "CD3.X312", "CD3.X1522");
      this.twoWayCopyforCD3UCD(id, val, "CD3.X15", "CD3.X16", "CD3.X314", "CD3.X315", "CD3.X316", "CD3.X1523");
      this.twoWayCopyforCD3UCD(id, val, "CD3.X17", "CD3.X18", "CD3.X318", "CD3.X319", "CD3.X320", "CD3.X1524");
      this.aggregateFeesForCD3UCD(id, val, "CD3.X19", "CD3.X20", 323, 335, 4, true, 1525, 1528);
    }

    private void calculateAdjustmentsLineL17(string id, string val)
    {
      this.aggregateFeesForCD3UCD(id, val, "L182", "L183", 354, 439, 5, false, -1, -1);
    }

    private void calculateAdjustmentsLineM03(string id, string val)
    {
      this.twoWayCopyforCD3UCD(id, val, "L81", "L82", "CD3.X443", "CD3.X444", "CD3.X445", "CD3.X1529");
    }

    private void calculateAdjustmentsLineM04(string id, string val)
    {
      this.twoWayCopyforCD3UCD(id, val, "L86", "L87", "CD3.X467", "CD3.X468", "CD3.X469", "CD3.X1530");
    }

    private void calculateAdjustmentsLineM05(string id, string val)
    {
      this.twoWayCopyforCD3UCD(id, val, "L90", "L91", "CD3.X491", "CD3.X492", "CD3.X493", "CD3.X1531");
    }

    private void calculateAdjustmentsLineM06(string id, string val)
    {
      this.twoWayCopyforCD3UCD(id, val, "CD3.X24", "CD3.X25", "CD3.X515", "CD3.X516", "CD3.X517", "CD3.X1532");
    }

    private void calculateAdjustmentsLineM07(string id, string val)
    {
      this.twoWayCopyforCD3UCD(id, val, "CD3.X26", "CD3.X27", "CD3.X539", "CD3.X540", "CD3.X541", "CD3.X1533");
    }

    private void calculateAdjustmentsLineM08(string id, string val)
    {
      this.aggregateFeesForCD3UCD(id, val, "CD3.X28", "CD3.X29", 564, 584, 4, true, 1534, 1539);
    }

    private void calculateAdjustmentsLineM16(string id, string val)
    {
      this.aggregateFeesForCD3UCD(id, val, "CD3.X30", "CD3.X31", 957, 1042, 5, false, -1, -1);
    }

    private void calculateAdjustmentsLineN07(string id, string val)
    {
      this.aggregateFeesForCD3UCD(id, val, "L146", "L147", 1068, 1086, 3, false, -1, -1);
    }

    private void calculateAdjustmentsLineN13(string id, string val)
    {
      this.aggregateFeesForCD3UCD(id, val, "CD3.X36", "CD3.X37", 1197, 1221, 3, false, -1, -1);
    }

    private void calculateAdjustmentsLineN19(string id, string val)
    {
      this.aggregateFeesForCD3UCD(id, val, "L184", "L185", 1413, 1498, 5, false, -1, -1);
    }

    private void aggregateFeesForCD3UCD(
      string id,
      string val,
      string feeNameID,
      string amtID,
      int amtIDStart,
      int amtIDEnd,
      int amtIDInterval,
      bool checkPOC,
      int paidByStart,
      int paidByEnd)
    {
      double num1 = 0.0;
      double num2 = 0.0;
      int num3 = 0;
      int num4 = 0;
      string val1 = "";
      string str1 = "";
      int num5 = 0;
      for (int index = amtIDStart; index <= amtIDEnd; index += amtIDInterval)
      {
        double num6 = this.FltVal("CD3.X" + (object) index);
        if (amtID == "L135")
        {
          if (index == 280 && this.Val("CD3.X1540") != "Y" || index == 283 && this.Val("CD3.X1541") != "Y")
            num1 += num6;
          else
            num2 += num6;
        }
        else if (!checkPOC || checkPOC && this.Val("CD3.X" + (object) (index + (feeNameID == "CD3.X11" ? 2 : 1))) != "Y")
          num1 += num6;
        else
          num2 += num6;
        string str2 = this.Val("CD3.X" + (object) (index - 1));
        if (str2 != "" && num3 < 2)
        {
          ++num3;
          if (val1 == "")
          {
            val1 = str2;
            if (paidByStart != -1 && paidByEnd != -1)
              str1 = this.Val("CD3.X" + (object) (paidByStart + num5));
          }
        }
        if (num6 > 0.0 || str2 != "")
          ++num4;
        ++num5;
      }
      this.SetCurrentNum(amtID, num1);
      if (feeNameID == "L84")
      {
        if (num4 > 1)
        {
          val1 = "See attached page for additional information";
        }
        else
        {
          string str3 = this.Val("CD3.X139");
          val1 = str3.Contains("exceeding legal limits P.O.C.") || str3.Contains("Principal Reduction") || str3.Contains("StandardOther") ? str3 : this.Val("CD3.X141");
        }
      }
      else if (num3 > 1)
        val1 = "See attached page for additional information";
      if (checkPOC && num2 > 0.0 && num3 <= 1 && num4 <= 1)
        val1 = val1 + " $" + num2.ToString("N2") + " P.O.C " + str1;
      this.SetVal(feeNameID, val1);
    }

    private void twoWayCopyforCD3UCD(
      string id,
      string val,
      string feeName1ID,
      string amt1ID,
      string feeName2ID,
      string amt2ID,
      string pocID,
      string paidByID)
    {
      if (this.Val(pocID) == "Y")
      {
        double num = this.FltVal(amt2ID);
        string str = paidByID != "" ? this.Val(paidByID) : "";
        this.SetVal(feeName1ID, this.Val(feeName2ID) + (num != 0.0 ? " $" + num.ToString("N2") + " P.O.C" : "") + (str != "" ? " " + str : ""));
        if (!(id == pocID))
          return;
        this.SetCurrentNum(amt1ID, 0.0);
      }
      else if (id == pocID)
      {
        this.SetVal(feeName1ID, this.Val(feeName2ID));
        this.SetVal(amt1ID, this.Val(amt2ID));
      }
      else if (id == feeName1ID)
        this.SetVal(feeName2ID, this.Val(feeName1ID));
      else if (id == feeName2ID)
        this.SetVal(feeName1ID, this.Val(feeName2ID));
      else if (id == amt1ID)
      {
        this.SetVal(amt2ID, this.Val(amt1ID));
      }
      else
      {
        if (!(id == amt2ID))
          return;
        this.SetVal(amt1ID, this.Val(amt2ID));
      }
    }

    private void calculateAdjustmentsLineKSubtotal(string id, string val)
    {
      this.calculateAdjustmentsLineKSubtotal();
      if (string.IsNullOrEmpty(this.Val("CD3.X1503")))
        this.calculateAdjustmentsLineLSubtotal();
      if (string.IsNullOrEmpty(this.Val("CD3.X1505")))
        this.calculateNonUcdAdjustmentTotal();
      this.calculateUcdAdjustmentTotal((string) null, (string) null);
      this.calculateTotalAdjustmentAndOtherCredits((string) null, (string) null);
      this.CalculateCDPage3StdCashToCloseFinal((string) null, (string) null);
    }

    private void calculateAdjustmentsLineKSubtotal()
    {
      this.SetCurrentNum("CD3.X1502", this.FltVal("L89") + this.FltVal("CD3.X3") + this.FltVal("CD3.X5") + this.FltVal("L94") + this.FltVal("L100") + this.FltVal("L106") + this.FltVal("L111") + this.FltVal("L115") + this.FltVal("L119") + this.FltVal("L123") + this.FltVal("CD3.X7"), true);
    }

    private void calculateAdjustmentsLineLSubtotal(string id, string val)
    {
      this.calculateAdjustmentsLineLSubtotal();
      if (string.IsNullOrEmpty(this.Val("CD3.X1502")))
        this.calculateAdjustmentsLineKSubtotal();
      if (string.IsNullOrEmpty(this.Val("CD3.X1505")))
        this.calculateNonUcdAdjustmentTotal();
      this.calculateUcdAdjustmentTotal((string) null, (string) null);
      this.calculateTotalAdjustmentAndOtherCredits((string) null, (string) null);
      this.CalculateCDPage3StdCashToCloseFinal((string) null, (string) null);
    }

    private void calculateAdjustmentsLineLSubtotal()
    {
      double num = this.FltVal("L135");
      for (int index = 10; index <= 20; index += 2)
        num += this.FltVal("CD3.X" + (object) index);
      for (int index = 158; index <= 170; index += 6)
        num += this.FltVal("L" + (object) index);
      for (int index = 175; index <= 183; index += 4)
        num += this.FltVal("L" + (object) index);
      this.SetCurrentNum("CD3.X1503", num, true);
    }

    private void calculateUcdAdjustmentTotal(string id, string val)
    {
      double num1 = this.FltVal("CD3.X1502");
      double num2 = this.FltVal("CD3.X1503");
      double num3 = this.FltVal("CD3.X1543");
      this.SetCurrentNum("CD3.X1504", num1 + num3 - num2, true);
    }

    private void calculateNonUcdAdjustmentTotal(string id, string val)
    {
      this.calculateNonUcdAdjustmentTotal();
      string str1 = this.Val("CD3.X1502");
      if (string.IsNullOrEmpty(str1))
        this.calculateAdjustmentsLineKSubtotal();
      string str2 = this.Val("CD3.X1503");
      if (string.IsNullOrEmpty(str2))
        this.calculateAdjustmentsLineLSubtotal();
      if (string.IsNullOrEmpty(str1) || string.IsNullOrEmpty(str2))
        this.calculateUcdAdjustmentTotal((string) null, (string) null);
      this.calculateTotalAdjustmentAndOtherCredits((string) null, (string) null);
      this.calculateLoanEstimate_EstimatedCashToClose((string) null, (string) null);
      this.CalculateCDPage3StdCashToCloseFinal((string) null, (string) null);
    }

    private void calculateNonUcdAdjustmentTotal()
    {
      this.SetCurrentNum("CD3.X1505", this.FltVal("LE2.X6") + this.FltVal("LE2.X8") + this.FltVal("LE2.X10") + this.FltVal("LE2.X12") + this.FltVal("LE2.X14") + this.FltVal("LE2.X16") + this.FltVal("LE2.X18") + this.FltVal("LE2.X20") + this.FltVal("LE2.X22") + this.FltVal("LE2.X24"), true);
    }

    private void calculateTotalAdjustmentAndOtherCredits(string id, string val)
    {
      double num1 = this.FltVal("CD3.X1504") + this.FltVal("CD3.X1505");
      this.SetCurrentNum("CD3.X1506", num1, true);
      double num2 = num1 * -1.0;
      this.SetCurrentNum("LE2.X4", num2, true);
      this.SetCurrentNum("CD3.X109", num2, true);
    }

    private void calculateAdjustmentsDetialTotalMigration(string id, string val)
    {
      this.calculateAdjustmentsLineKSubtotal();
      this.calculateAdjustmentsLineLSubtotal();
      this.calculateNonUcdAdjustmentTotal();
      this.calculateUcdAdjustmentTotal((string) null, (string) null);
      this.calculateTotalAdjustmentAndOtherCredits((string) null, (string) null);
      this.CalculateCDPage3StdCashToCloseFinal((string) null, (string) null);
    }

    private void calculateConstructionPermPeriod(string id, string val)
    {
      string str = this.Val("19");
      int num1 = this.IntVal("1176");
      int num2 = this.IntVal("4");
      int num3 = this.IntVal("325");
      int num4 = this.IntVal("1177");
      switch (str)
      {
        case "ConstructionToPermanent":
          this.SetVal("CD4.X46", string.Concat((object) (num1 + this.IntVal("1177"))));
          return;
        case "ConstructionOnly":
          if (this.Val("SYS.X6") == "A")
          {
            this.SetVal("CD4.X46", num1 > 0 ? string.Concat((object) (this.IntVal("1176") - 1)) : "");
            return;
          }
          break;
      }
      if (!str.Contains("Construction") && num4 > 0 && num2 > num3 && (num4 == num3 || num4 == num3 - 1))
        this.SetVal("CD4.X46", "");
      else
        this.SetVal("CD4.X46", this.Val("1177"));
    }

    private void calculateAsCompletedPurchasePrice(string id, string val)
    {
      this.SetCurrentNum("CONST.X58", this.FltVal("21") + this.FltVal("23"));
    }

    private void calculateAsCompletedAppraisedValue(string id, string val)
    {
      string str = this.Val("19");
      if (str == "ConstructionOnly" || str == "ConstructionToPermanent")
      {
        double num = this.FltVal("356");
        if (num != 0.0)
          this.SetCurrentNum("CONST.X59", num);
        else
          this.SetCurrentNum("CONST.X59", this.FltVal("22") + this.FltVal("23"));
      }
      else
        this.SetCurrentNum("CONST.X59", this.FltVal("22") + this.FltVal("23"));
    }

    private void calculateDisclosedSalesPrice(string id, string val)
    {
      string str = this.Val("19");
      if (str == "ConstructionOnly" || str == "ConstructionToPermanent")
      {
        if (this.Val("1964") == "Y")
          this.SetCurrentNum("L726", this.FltVal("21"));
        else
          this.SetVal("L726", "");
      }
      else
        this.SetCurrentNum("L726", this.FltVal("136"));
    }

    private void calculateLECDDisplayFields(string id, string val)
    {
      if (id == "3" || id == "IMPORT" || id == "KBYO.XD3")
        this.SetVal("KBYO.XD3", Utils.RemoveEndingZeros(this.FltVal("3").ToString()));
      if (id == "689" || id == "IMPORT" || id == "KBYO.XD3")
        this.SetVal("KBYO.XD689", Utils.RemoveEndingZeros(this.FltVal("689").ToString()));
      if (id == "697" || id == "IMPORT" || id == "KBYO.XD3")
        this.SetVal("KBYO.XD697", Utils.RemoveEndingZeros(this.FltVal("697").ToString()));
      if (id == "695" || id == "IMPORT" || id == "KBYO.XD3")
        this.SetVal("KBYO.XD695", Utils.RemoveEndingZeros(this.FltVal("695").ToString()));
      if (id == "799" || id == "IMPORT" || id == "KBYO.XD3")
        this.SetVal("KBYO.XD799", Utils.RemoveEndingZeros(this.FltVal("799").ToString()));
      if (id == "LE3.X16" || id == "IMPORT" || id == "KBYO.XD3")
        this.SetVal("KBYO.LE3XD16", Utils.RemoveEndingZeros(this.FltVal("LE3.X16").ToString()));
      if (id == "NEWHUD.X555" || id == "IMPORT" || id == "KBYO.XD3")
        this.SetVal("KBYO.NEWHUDXD555", Utils.RemoveEndingZeros(this.FltVal("NEWHUD.X555").ToString()));
      if (id == "2625" || id == "IMPORT" || id == "KBYO.XD3")
        this.SetVal("KBYO.XD2625", Utils.RemoveEndingZeros(this.FltVal("2625").ToString()));
      if (id == "4113" || id == "IMPORT" || id == "KBYO.XD3")
        this.SetVal("KBYO.XD4113", Utils.RemoveEndingZeros(this.FltVal("4113").ToString()));
      if (!(id == "1699") && !(id == "IMPORT") && !(id == "KBYO.XD3"))
        return;
      this.SetVal("KBYO.XD1699", Utils.RemoveEndingZeros(this.FltVal("1699").ToString()));
    }

    private void calculateSubsequentlyPaidFinanceCharge(string id, string val)
    {
      double num = 0.0;
      for (int index = 0; index < HUDGFE2010Fields.WHOLEPOC_FIELDS.Count; ++index)
      {
        string[] strArray = HUDGFE2010Fields.WHOLEPOC_FIELDS[index];
        switch (Utils.ParseInt((object) strArray[HUDGFE2010Fields.PTCPOCINDEX_LINENUMBER], 0))
        {
          case 2001:
          case 2002:
          case 2003:
          case 2004:
            num += this.FltVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORAMTPAID]) + this.FltVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BRKAMTPAID]) + this.FltVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015OTHAMTPAID]);
            break;
        }
      }
      this.SetCurrentNum("NEWHUD2.X4768", num);
    }

    internal bool UseNewCompliance(Decimal versionToRunNewLogic)
    {
      string complianceVersion = this.Val("COMPLIANCEVERSION.X1");
      return Mapping.UseNewCompliance(versionToRunNewLogic, complianceVersion);
    }

    private void clearPostConsummationSection(string id, string val)
    {
      string str = this.Val("19");
      if (!(str != "ConstructionOnly") || !(str != "ConstructionToPermanent"))
        return;
      for (int index = 4660; index <= 4767; ++index)
      {
        if ((index < 4756 || index > 4759) && index != 4668 && index != 4691 && index != 4714 && index != 4737 && index != 4676 && index != 4699 && index != 4722 && index != 4745)
        {
          if (index >= 4764 && index <= 4767)
            this.RemoveCurrentLock("NEWHUD2.X" + (object) index);
          this.SetVal("NEWHUD2.X" + (object) index, "");
        }
      }
    }
  }
}
