// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.CalculationEngine.ToolCalculation
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.InterimServicing;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.LoanUtils.RateLocks;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;
using EllieMae.EMLite.Xml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

#nullable disable
namespace EllieMae.EMLite.CalculationEngine
{
  internal class ToolCalculation : CalculationBase
  {
    private const string className = "ToolCalculation�";
    private static readonly string sw = Tracing.SwDataEngine;
    internal Routine CalcProfit;
    internal Routine UpdateSysIdGuid;
    internal Routine CalcRateLock;
    internal Routine CalcInvestorStatus;
    internal Routine PopulateCountyLimit;
    internal Routine CalcSafeHarbor;
    internal Routine UpdateSellSideSRPANDPriceBalance;
    internal Routine CalcNetTangibleBenefit;
    internal Routine CalcIRS1098;
    internal Routine CalcLOCompensation;
    internal Routine CalcTPOLoanInfo;
    internal Routine CalcCorrespondentPurchaseAdvice;
    internal Routine CalcCorrespondentLateFee;
    internal Routine CalcTpoFeeAmount;
    internal Routine CalcStatementOfDenial;
    internal Routine CalcValueForIntrestRateConditions;
    internal Routine UpdateDisclosureDeterminedField;
    internal Routine UpdateDisclosureExplanationFields;
    internal Routine UpdateCorrespondentPrincipal;
    internal Routine CopyFromCorrespondentPrincipal;
    internal Routine CopyFromInterimServicing;
    internal Routine ClearAmtOfInitialAdvance;
    internal Routine LoadPurchaseAdvicePrincipal;
    internal Routine SyncConstructionLinkSyncDefaultData;
    internal Routine CopyFromCorrespondentPurchaseAdvice;
    internal Routine UpdatePercentageOwnershipInterest;
    internal Routine CalcHMDAReportingYear;
    internal Routine CalcFundingWorksheetULDD;
    internal Routine CalcNBOtoVesting;
    internal Routine CalcMICancelCondTypeAndRMLA;
    internal Routine UpdateConfirmAdviceDateAndName;
    internal Routine CalcPrepaymentBalance;
    internal Routine CalcFirstInvestorPaymentDate;
    internal Routine CalcAmortization;
    internal Routine CalcFloodInsuranceAgentName;
    internal Routine CalcConsumerHIOrderEligible;
    private bool policyEnableGEICOIntegration;
    internal Routine CalcCPAEscrowDetails;
    internal Routine CalcCPAEscrowDetails_FCD_AmountFields;
    internal Routine CalcCPAEscrowDetails_AddlEscrow_AmountFields;
    public EventHandler ExceededCountyLimitEvent;
    internal bool CreditorOverride;
    private bool internalInvoke;
    private bool skipCountyLimitCheck;
    private SessionObjects sessionObjects;
    private ILoanConfigurationInfo configInfo;
    private BusinessCalendar businessCalendar;
    private PaymentScheduleSnapshot paySnapshot;
    private static string[] tpoInfoFields = new string[71]
    {
      "1",
      "3",
      "4",
      "5",
      "6",
      "8",
      "9",
      "10",
      "11",
      "12",
      "92",
      "14",
      "15",
      "16",
      "17",
      "18",
      "19",
      "20",
      "21",
      "22",
      "23",
      "24",
      "27",
      "28",
      "29",
      "30",
      "31",
      "38",
      "39",
      "40",
      "41",
      "42",
      "43",
      "44",
      "45",
      "46",
      "47",
      "48",
      "51",
      "52",
      "53",
      "54",
      "55",
      "61",
      "62",
      "63",
      "64",
      "65",
      "66",
      "67",
      "68",
      "69",
      "70",
      "71",
      "72",
      "56",
      "57",
      "74",
      "75",
      "76",
      "77",
      "78",
      "79",
      "80",
      "81",
      "82",
      "83",
      "84",
      "85",
      "58",
      "59"
    };
    private static string[] closingVendorLenderFields = new string[22]
    {
      "VEND.X231",
      "VEND.X232",
      "1256",
      "Lender.CntctTitle",
      "1262",
      "95",
      "1263",
      "305",
      "1261",
      "1264",
      "1257",
      "1258",
      "1259",
      "1260",
      "Lender.OrgState",
      "Lender.OrgType",
      "3244",
      "3895",
      "3896",
      "3897",
      "3032",
      "3898"
    };
    private static string[] closingVendorInvestorFields = new string[18]
    {
      "VEND.X268",
      "VEND.X269",
      "VEND.X270",
      "VEND.X271",
      "Investor.CntctTitle",
      "VEND.X272",
      "VEND.X273",
      "VEND.X274",
      "VEND.X275",
      "VEND.X276",
      "VEND.X277",
      "VEND.X263",
      "VEND.X264",
      "VEND.X265",
      "VEND.X266",
      "VEND.X267",
      "VEND.X649",
      "VEND.X650"
    };
    private static string[] closingVendorBrokerFields = new string[23]
    {
      "VEND.X298",
      "VEND.X301",
      "VEND.X302",
      "Broker.CntctTitle",
      "VEND.X303",
      "VEND.X304",
      "VEND.X305",
      "VEND.X306",
      "VEND.X308",
      "VEND.X309",
      "VEND.X293",
      "VEND.X294",
      "VEND.X295",
      "VEND.X296",
      "VEND.X297",
      "VEND.X299",
      "VEND.X307",
      "VEND.X528",
      "VEND.X527",
      "VEND.X651",
      "VEND.X652",
      "VEND.X300",
      "VEND.X653"
    };
    private object printLOLicenseSetting;

    internal ToolCalculation(
      SessionObjects sessionObjects,
      ILoanConfigurationInfo configInfo,
      LoanData l,
      EllieMae.EMLite.CalculationEngine.CalculationObjects calObjs)
      : base(l, calObjs)
    {
      this.sessionObjects = sessionObjects;
      this.configInfo = configInfo;
      this.CalcProfit = this.RoutineX(new Routine(this.calculateProfit));
      this.CalcRateLock = this.RoutineX(new Routine(this.calculateRateLock));
      this.CalcInvestorStatus = this.RoutineX(new Routine(this.refreshInvestorStatus));
      this.PopulateCountyLimit = this.RoutineX(new Routine(this.validateCountyLimit));
      this.CalcSafeHarbor = this.RoutineX(new Routine(this.calculateSafeHarbor));
      this.UpdateSellSideSRPANDPriceBalance = this.RoutineX(new Routine(this.updateSellSideSRPANDPriceBalance));
      this.CalcNetTangibleBenefit = this.RoutineX(new Routine(this.calculateNetTangibleBenefit));
      this.CalcIRS1098 = this.RoutineX(new Routine(this.calculateIRS1098));
      this.CalcLOCompensation = this.RoutineX(new Routine(this.calculateLOCompensation));
      this.CalcTPOLoanInfo = this.RoutineX(new Routine(this.calculateTPOLoanInfo));
      this.CalcCorrespondentPurchaseAdvice = this.RoutineX(new Routine(this.calculateCorrespondentPurchaseAdvice));
      this.CalcCorrespondentLateFee = this.RoutineX(new Routine(this.calculateCorrespondentLateFee));
      this.CalcTpoFeeAmount = this.RoutineX(new Routine(this.calculateTpoFeeAmount));
      this.CalcStatementOfDenial = this.RoutineX(new Routine(this.calculateStatementOfDenial));
      this.CalcValueForIntrestRateConditions = this.RoutineX(new Routine(this.calculateValueForIntrestRateConditions));
      this.UpdateDisclosureDeterminedField = this.RoutineX(new Routine(this.updateDisclosureDeterminedField));
      this.UpdateDisclosureExplanationFields = this.RoutineX(new Routine(this.updateDisclosureExplanationFields));
      this.UpdateSysIdGuid = this.RoutineX(new Routine(this.updateSystemIdGuid));
      this.UpdateCorrespondentPrincipal = this.RoutineX(new Routine(this.updateCorrespondentLoanStatusBalance));
      this.CopyFromCorrespondentPrincipal = this.RoutineX(new Routine(this.copyFromCorrespondentLoanStatusBalance));
      this.CopyFromInterimServicing = this.RoutineX(new Routine(this.copyFromInterimServicing));
      this.ClearAmtOfInitialAdvance = this.RoutineX(new Routine(this.ClearAmountOfInitialAdvance));
      this.LoadPurchaseAdvicePrincipal = this.RoutineX(new Routine(this.loadPurchaseAdvicePrincipal));
      this.SyncConstructionLinkSyncDefaultData = this.RoutineX(new Routine(this.syncConstLinkSyncDefaultData));
      this.CopyFromCorrespondentPurchaseAdvice = this.RoutineX(new Routine(this.copyFromCorrespondentPurchaseAdvice));
      this.UpdatePercentageOwnershipInterest = this.RoutineX(new Routine(this.updatePercentageOwnershipInterest));
      this.CalcHMDAReportingYear = this.RoutineX(new Routine(this.calculateHmdaReportingYear));
      this.CalcFundingWorksheetULDD = this.RoutineX(new Routine(this.calculatefundingWorksheetULDD));
      this.CalcNBOtoVesting = this.RoutineX(new Routine(this.calculateNBOtoVesting));
      this.CalcMICancelCondTypeAndRMLA = this.RoutineX(new Routine(this.calculateMICancelCondTypeAndRMLA));
      this.UpdateConfirmAdviceDateAndName = this.RoutineX(new Routine(this.updateConfirmAdviceDateAndName));
      this.CalcPrepaymentBalance = this.RoutineX(new Routine(this.calculatePrepaymentBalance));
      this.CalcFirstInvestorPaymentDate = this.RoutineX(new Routine(this.calculatefirstInvestorPaymentDate));
      this.CalcAmortization = this.RoutineX(new Routine(this.calculateAmortization));
      this.CalcConsumerHIOrderEligible = this.RoutineX(new Routine(this.calculateConsumerHIOrderEligible));
      this.CalcCPAEscrowDetails = this.RoutineX(new Routine(this.calculateCPAEscrowDetails_DescFields)) + this.RoutineX(new Routine(this.calculateCPAEscrowDetails_FCD_AmountFields)) + this.RoutineX(new Routine(this.calculateCPAEscrowDetails_AddlEscrow_AmountFields)) + this.RoutineX(new Routine(this.calculateCPA_ADDLESCROW_MIMIP));
      this.CalcCPAEscrowDetails_FCD_AmountFields = this.RoutineX(new Routine(this.calculateCPAEscrowDetails_FCD_AmountFields));
      this.CalcCPAEscrowDetails_AddlEscrow_AmountFields = this.RoutineX(new Routine(this.calculateCPAEscrowDetails_AddlEscrow_AmountFields));
      this.CalcFloodInsuranceAgentName = this.RoutineX(new Routine(this.calculateFloodInsuranceAgentName));
      this.addFieldHandlers();
      if (!string.Equals(this.sessionObjects.GetCompanySettingFromCache("Policies", "ENABLEGEICOINTEGRATION"), "True", StringComparison.CurrentCultureIgnoreCase))
        return;
      this.policyEnableGEICOIntegration = true;
    }

    private void addFieldHandlers()
    {
      Routine routine1 = this.RoutineX(new Routine(this.calculateServicingTotalDue));
      this.AddFieldHandler("SERVICE.X13", routine1);
      this.AddFieldHandler("SERVICE.X14", routine1);
      this.AddFieldHandler("SERVICE.X15", routine1);
      this.AddFieldHandler("SERVICE.X16", routine1);
      this.AddFieldHandler("SERVICE.X17", routine1);
      this.AddFieldHandler("SERVICE.X18", routine1);
      this.AddFieldHandler("SERVICE.X19", routine1);
      this.AddFieldHandler("SERVICE.X20", routine1);
      this.AddFieldHandler("SERVICE.X21", routine1);
      this.AddFieldHandler("SERVICE.X22", routine1);
      this.AddFieldHandler("SERVICE.X23", routine1);
      this.AddFieldHandler("SERVICE.X25", routine1);
      this.AddFieldHandler("SERVICE.X105", routine1);
      this.AddFieldHandler("SERVICE.X106", routine1);
      for (int index = 58; index <= 73; ++index)
        this.AddFieldHandler("SERVICE.X" + (object) index, routine1);
      this.AttachIntermServicingFieldHandlers();
      Routine routine2 = this.RoutineX(new Routine(this.calculateProfit));
      this.AddFieldHandler("PM01", routine2);
      this.AddFieldHandler("PM03", routine2);
      this.AddFieldHandler("PM05", routine2);
      this.AddFieldHandler("PM07", routine2);
      this.AddFieldHandler("PM09", routine2);
      this.AddFieldHandler("PM11", routine2);
      this.AddFieldHandler("PM12", routine2);
      this.AddFieldHandler("PM13", routine2);
      this.AddFieldHandler("PM15", routine2);
      this.AddFieldHandler("PM16", routine2);
      this.AddFieldHandler("PM17", routine2);
      this.AddFieldHandler("PM19", routine2);
      this.AddFieldHandler("PM20", routine2);
      this.AddFieldHandler("PM21", routine2);
      this.AddFieldHandler("PM23", routine2);
      this.AddFieldHandler("PM24", routine2);
      this.AddFieldHandler("PM25", routine2);
      Routine routine3 = this.RoutineX(new Routine(this.calculateTrust));
      this.AddFieldHandler("TA00PA", routine3);
      this.AddFieldHandler("TA00RA", routine3);
      Routine routine4 = this.RoutineX(new Routine(this.calculateRateLock));
      for (int index = 2101; index <= 2141; index += 2)
        this.AddFieldHandler(index.ToString(), routine4);
      for (int index = 2593; index <= 2607; ++index)
        this.AddFieldHandler(index.ToString(), routine4);
      for (int index = 2092; index <= 2098; index += 2)
        this.AddFieldHandler(index.ToString(), routine4);
      for (int index = 2415; index <= 2447; index += 2)
        this.AddFieldHandler(index.ToString(), routine4);
      for (int index = 2373; index <= 2395; index += 2)
        this.AddFieldHandler(index.ToString(), routine4);
      for (int index = 2649; index <= 2687; index += 2)
        this.AddFieldHandler(index.ToString(), routine4);
      for (int index = 2834; index <= 2837; ++index)
        this.AddFieldHandler(index.ToString(), routine4);
      for (int index = 3455; index <= 3473; ++index)
        this.AddFieldHandler(index.ToString(), routine4);
      for (int index = 4257; index <= 4275; index += 2)
        this.AddFieldHandler(index.ToString(), routine4);
      for (int index = 4337; index <= 4355; index += 2)
        this.AddFieldHandler(index.ToString(), routine4);
      this.AddFieldHandler("2647", routine4);
      this.AddFieldHandler("2211", routine4);
      this.AddFieldHandler("2212", routine4);
      this.AddFieldHandler("2213", routine4);
      this.AddFieldHandler("2632", routine4);
      this.AddFieldHandler("3130", routine4);
      this.AddFieldHandler("3131", routine4);
      Routine routine5 = this.RoutineX(new Routine(this.UpdatePurchaseAdviceBalance));
      this.AddFieldHandler("2207", routine5);
      this.AddFieldHandler("2209", routine5);
      Routine routine6 = this.RoutineX(new Routine(this.refreshInvestorStatus));
      this.AddFieldHandler("2819", routine6);
      this.AddFieldHandler("2014", routine6);
      this.AddFieldHandler("2370", routine6);
      Routine routine7 = this.RoutineX(new Routine(this.validateCountyLimit));
      this.AddFieldHandler("16", routine7);
      this.AddFieldHandler("MORNET.X40", routine7);
      this.AddFieldHandler("OPTIMAL.RESPONSE", this.RoutineX(new Routine(this.populateOptimalblueResult)));
      Routine routine8 = this.RoutineX(new Routine(this.calculateSafeHarbor));
      for (int index = 688; index <= 866; ++index)
        this.AddFieldHandler("DISCLOSURE.X" + (object) index, routine8);
      this.AddFieldHandler("DISCLOSURE.X977", routine8);
      this.AddFieldHandler("DISCLOSURE.X981", routine8);
      this.AddFieldHandler("DISCLOSURE.X985", routine8);
      this.AddFieldHandler("DISCLOSURE.X989", routine8);
      Routine routine9 = this.RoutineX(new Routine(this.calculateSellSideSRPAndPrice));
      for (int index = 2207; index <= 2210; ++index)
        this.AddFieldHandler(index.ToString(), routine9);
      Routine routine10 = this.RoutineX(new Routine(this.updateSellSideSRPANDPriceBalance));
      for (int index = 3422; index <= 3428; index += 2)
        this.AddFieldHandler(index.ToString(), routine10);
      this.AddFieldHandler("2211", routine10);
      Routine routine11 = this.RoutineX(new Routine(this.calculateCorrespondentPurchaseAdvice));
      this.AddFieldHandler("2203", routine11);
      this.AddFieldHandler("3567", routine11);
      this.AddFieldHandler("3573", routine11);
      this.AddFieldHandler("3575", routine11);
      this.AddFieldHandler("3577", routine11);
      this.AddFieldHandler("3578", routine11);
      this.AddFieldHandler("CORRESPONDENT.X405", routine11);
      for (int index = 3579; index <= 3585; ++index)
        this.AddFieldHandler(index.ToString(), routine11);
      for (int index = 3588; index <= 3610; index += 2)
        this.AddFieldHandler(index.ToString(), routine11);
      this.AddFieldHandler("3970", routine11);
      this.AddFieldHandler("3938", routine11);
      this.AddFieldHandler("3939", routine11);
      this.AddFieldHandler("CORRESPONDENT.X64", routine11);
      this.AddFieldHandler("CORRESPONDENT.X66", routine11);
      this.loan.FieldChanged += new FieldChangedEventHandler(this.loan_FieldChanged);
      Routine routine12 = this.RoutineX(new Routine(this.calculateLOCompensation));
      for (int index = 2; index <= 5; ++index)
        this.AddFieldHandler("LCP.X" + index.ToString(), routine12);
      for (int index = 13; index <= 17; ++index)
        this.AddFieldHandler("LCP.X" + index.ToString(), routine12);
      for (int index = 25; index <= 34; ++index)
        this.AddFieldHandler("LCP.X" + index.ToString(), routine12);
      Routine routine13 = this.RoutineX(new Routine(this.calculateTPOLoanInfo));
      this.AddFieldHandler("TPO.X5", routine13);
      this.AddFieldHandler("TPO.X93", routine13);
      Routine routine14 = this.RoutineX(new Routine(this.calculateCorrespondentLateFee));
      this.AddFieldHandler("3571", routine14);
      this.AddFieldHandler("3926", routine14);
      this.AddFieldHandler("3917", routine14);
      this.AddFieldHandler("3918", routine14);
      this.AddFieldHandler("3919", routine14);
      this.AddFieldHandler("3920", routine14);
      this.AddFieldHandler("3921", routine14);
      this.AddFieldHandler("3927", routine14);
      this.AddFieldHandler("3928", routine14);
      this.AddFieldHandler("3929", routine14);
      this.AddFieldHandler("3930", routine14);
      this.AddFieldHandler("3931", routine14);
      this.AddFieldHandler("3932", routine14);
      this.AddFieldHandler("4110", routine14);
      this.AddFieldHandler("TPO.X15", routine14);
      this.AddFieldHandler("761", this.RoutineX(new Routine(this.calculateTpoFeeAmount)));
      this.AddFieldHandler("TPO.X88", this.RoutineX(new Routine(this.calculateStatementOfDenial)));
      Routine routine15 = this.RoutineX(new Routine(this.calculatePMICancellationConditions));
      this.AddFieldHandler("DISCLOSURE.X1147", routine15);
      this.AddFieldHandler("DISCLOSURE.X1148", routine15);
      this.AddFieldHandler("ULDD.X191", this.RoutineX(new Routine(this.calculatefundingWorksheetULDD)));
      Routine routine16 = this.RoutineX(new Routine(this.setRequiredDataForEDS));
      this.AddFieldHandler("EDS.X5", routine16);
      this.AddFieldHandler("LE1.XD8", routine16);
      this.AddFieldHandlerToKeyPricingFields();
      this.AddFieldHandler("VEND.X263", this.RoutineX(new Routine(this.clearInvestorInfo)));
      Routine routine17 = this.RoutineX(new Routine(this.updateConfirmAdviceDateAndName));
      this.AddFieldHandler("4666", routine17);
      this.AddFieldHandler("3612", routine17);
      this.AddFieldHandler("3613", routine17);
      Routine routine18 = this.RoutineX(new Routine(this.calculateCPAEscrowDetails_DescFields));
      this.AddFieldHandler("1628", routine18);
      this.AddFieldHandler("660", routine18);
      this.AddFieldHandler("661", routine18);
      Routine routine19 = this.RoutineX(new Routine(this.calculateCPAEscrowDetails_OptionDescFields));
      this.AddFieldHandler("CPA.FCD.Option1Desc", routine19);
      this.AddFieldHandler("CPA.FCD.Option2Desc", routine19);
      Routine routine20 = this.RoutineX(new Routine(this.calculateCPAEscrowDetails_FCD_AmountFields));
      this.AddFieldHandler("1631", routine20);
      this.AddFieldHandler("1632", routine20);
      this.AddFieldHandler("658", routine20);
      this.AddFieldHandler("598", routine20);
      this.AddFieldHandler("659", routine20);
      this.AddFieldHandler("599", routine20);
      this.AddFieldHandler("656", routine20);
      this.AddFieldHandler("596", routine20);
      this.AddFieldHandler("338", routine20);
      this.AddFieldHandler("563", routine20);
      this.AddFieldHandler("655", routine20);
      this.AddFieldHandler("595", routine20);
      this.AddFieldHandler("L269", routine20);
      this.AddFieldHandler("L270", routine20);
      this.AddFieldHandler("657", routine20);
      this.AddFieldHandler("597", routine20);
      this.AddFieldHandler("NEWHUD.X1708", routine20);
      this.AddFieldHandler("NEWHUD.X1714", routine20);
      this.AddFieldHandler("558", routine20);
      this.AddFieldHandler("CPA.RetainUserInputs", routine20);
      Routine routine21 = this.RoutineX(new Routine(this.calculateCPAEscrowDetails_FCD_TotalFields));
      this.AddFieldHandler("CPA.FCD.Option1Amount", routine21);
      this.AddFieldHandler("CPA.FCD.Option2Amount", routine21);
      this.AddFieldHandler("CPA.FCD.1007Amount", routine21);
      this.AddFieldHandler("CPA.FCD.1008Amount", routine21);
      this.AddFieldHandler("CPA.FCD.1009Amount", routine21);
      this.AddFieldHandler("CPA.FCD.HomeInsurance", routine21);
      this.AddFieldHandler("CPA.FCD.MortgageInsurance", routine21);
      this.AddFieldHandler("CPA.FCD.PropertyTax", routine21);
      this.AddFieldHandler("CPA.FCD.CityPropertyTax", routine21);
      this.AddFieldHandler("CPA.FCD.FloodInsurance", routine21);
      this.AddFieldHandler("CPA.FCD.USDAAnnualFee", routine21);
      this.AddFieldHandler("CPA.FCD.AggAdjAmount", routine21);
      Routine routine22 = this.RoutineX(new Routine(this.calculateCPAEscrowDetails_AddlEscrow_AmountFields));
      this.AddFieldHandler("1630", routine22);
      this.AddFieldHandler("253", routine22);
      this.AddFieldHandler("254", routine22);
      this.AddFieldHandler("230", routine22);
      this.AddFieldHandler("231", routine22);
      this.AddFieldHandler("L268", routine22);
      this.AddFieldHandler("235", routine22);
      this.AddFieldHandler("NEWHUD.X1707", routine22);
      this.AddFieldHandler("CPA.RetainUserInputs", routine22);
      this.AddFieldHandler("NEWHUD2.X133", routine22);
      this.AddFieldHandler("NEWHUD2.X134", routine22);
      this.AddFieldHandler("NEWHUD2.X135", routine22);
      this.AddFieldHandler("NEWHUD2.X136", routine22);
      this.AddFieldHandler("NEWHUD2.X137", routine22);
      this.AddFieldHandler("NEWHUD2.X138", routine22);
      this.AddFieldHandler("NEWHUD2.X139", routine22);
      this.AddFieldHandler("NEWHUD2.X140", routine22);
      Routine routine23 = this.RoutineX(new Routine(this.calculateCPA_ADDLESCROW_MIMIP));
      this.AddFieldHandler("232", routine23);
      this.AddFieldHandler("CPA.PaymentHistory.AnticipatedPurchaseDate", new Routine(this.calculatefirstInvestorPaymentDate) + routine23);
      this.AddFieldHandler("CPA.PaymentHistory.FirstBorrowerPaymentDueDate", new Routine(this.calculatefirstInvestorPaymentDate) + routine23);
      this.AddFieldHandler("CPA.RetainUserInputs", routine23);
      this.AddFieldHandler("CPA.PaymentHistory.FirstInvestorPaymentDate", routine23);
      Routine routine24 = this.RoutineX(new Routine(this.calculateCPAEscrowDetails_AddlEscrow_TotalFields));
      this.AddFieldHandler("CPA.ADDLESCROW.Option1Amount", routine24);
      this.AddFieldHandler("CPA.ADDLESCROW.Option2Amount", routine24);
      this.AddFieldHandler("CPA.ADDLESCROW.1007Amount", routine24);
      this.AddFieldHandler("CPA.ADDLESCROW.1008Amount", routine24);
      this.AddFieldHandler("CPA.ADDLESCROW.1009Amount", routine24);
      this.AddFieldHandler("CPA.ADDLESCROW.HomeInsurance", routine24);
      this.AddFieldHandler("CPA.ADDLESCROW.PropertyTax", routine24);
      this.AddFieldHandler("CPA.ADDLESCROW.CityPropertyTax", routine24);
      this.AddFieldHandler("CPA.ADDLESCROW.FloodInsurance", routine24);
      this.AddFieldHandler("CPA.ADDLESCROW.USDAAnnualFee", routine24);
      this.AddFieldHandler("CPA.ADDLESCROW.MIMIP", routine24);
      Routine routine25 = this.RoutineX(new Routine(this.calculateCPAEscrowDetails_EscrowDisbursements_TotalFields));
      this.AddFieldHandler("CPA.ESCROWDISBURSE.HomeInsurance", routine25);
      this.AddFieldHandler("CPA.ESCROWDISBURSE.MortgageInsurance", routine25);
      this.AddFieldHandler("CPA.ESCROWDISBURSE.PropertyTax", routine25);
      this.AddFieldHandler("CPA.ESCROWDISBURSE.CityPropertyTax", routine25);
      this.AddFieldHandler("CPA.ESCROWDISBURSE.FloodInsurance", routine25);
      this.AddFieldHandler("CPA.ESCROWDISBURSE.1007Amount", routine25);
      this.AddFieldHandler("CPA.ESCROWDISBURSE.1008Amount", routine25);
      this.AddFieldHandler("CPA.ESCROWDISBURSE.1009Amount", routine25);
      this.AddFieldHandler("CPA.ESCROWDISBURSE.USDAAnnualFee", routine25);
      this.AddFieldHandler("CPA.ESCROWDISBURSE.Option1Amount", routine25);
      this.AddFieldHandler("CPA.ESCROWDISBURSE.Option2Amount", routine25);
      this.AddFieldHandler("3925", this.RoutineX(new Routine(this.calculatePrepaymentBalance)));
      Routine routine26 = this.RoutineX(new Routine(this.calculatefirstInvestorPaymentDate));
      this.AddFieldHandler("CPA.PaymentHistory.NoteDate", routine26 + new Routine(this.calculateAmortization));
      this.AddFieldHandler("CPA.PaymentHistory.FirstBorrowerPaymentDueDate", routine26 + new Routine(this.calculateAmortization));
      this.AddFieldHandler("CPA.PaymentHistory.AnticipatedPurchaseDate", routine26 + new Routine(this.calculateAmortization));
      for (int index = 1; index <= 11; ++index)
      {
        this.AddFieldHandler("CPA.PaymentHistory.ExtraPayment.Date." + index.ToString("00"), new Routine(this.calculateAmortization));
        this.AddFieldHandler("CPA.PaymentHistory.ExtraPayment.Amount." + index.ToString("00"), new Routine(this.calculateAmortization));
      }
      Routine routine27 = this.RoutineX(new Routine(this.calculateAmortization));
      this.AddFieldHandler("CPA.PaymentHistory.PricipalReduction", routine27);
      this.AddFieldHandler("CPA.PaymentHistory.FirstInvestorPaymentDate", routine27);
      this.AddFieldHandler("4533", this.RoutineX(new Routine(this.calculateConsumerHIOrderEligible)));
      Routine routine28 = this.RoutineX(new Routine(this.calculateFloodInsuranceAgentName));
      this.AddFieldHandler("VEND.X13", routine28);
      this.AddFieldHandler("DISCLOSURE.X1219", routine28);
    }

    internal void AddFieldHandlerBasedOnExtLateFeeSettings()
    {
      Routine routine = this.RoutineX(new Routine(this.calculateCorrespondentLateFee));
      ExternalLateFeeSettings externalLateFeeSettings = this.calObjs.ExternalLateFeeSettings ?? this.sessionObjects.ConfigurationManager.GetExternalOrgLateFeeSettings(this.Val("TPO.X15"), true);
      if (externalLateFeeSettings != null && !string.IsNullOrEmpty(externalLateFeeSettings.OtherDate) && externalLateFeeSettings.OtherDate.StartsWith("Fields."))
      {
        string fieldId = externalLateFeeSettings.OtherDate.Substring(7);
        if (fieldId != string.Empty)
          this.AddFieldHandler(fieldId, routine);
      }
      if (externalLateFeeSettings != null && !string.IsNullOrEmpty(externalLateFeeSettings.DayClearedOtherDate) && externalLateFeeSettings.DayClearedOtherDate.StartsWith("Fields."))
      {
        string fieldId = externalLateFeeSettings.DayClearedOtherDate.Substring(7);
        if (fieldId != string.Empty)
          this.AddFieldHandler(fieldId, routine);
      }
      this.SetLateDaysEndDefaultValue(externalLateFeeSettings);
    }

    internal void AttachIntermServicingFieldHandlers()
    {
      Routine routine = this.RoutineX(new Routine(this.calculateServicingTotalDue));
      if (!(this.Val("SERVICE.X8") != string.Empty))
        return;
      if (this.Val("2626") == "Correspondent")
        this.AddFieldHandler("3579", routine);
      else
        this.AddFieldHandler("2", routine);
      this.AddFieldHandler("2370", routine);
      this.AddFieldHandler("3514", routine);
      this.AddFieldHandler("2211", routine);
    }

    public void SetLateDaysEndDefaultValueFromImport(ExternalLateFeeSettings externalLateFeeSettings)
    {
      this.SetLateDaysEndDefaultValue(externalLateFeeSettings);
    }

    private void SetLateDaysEndDefaultValue(ExternalLateFeeSettings externalLateFeeSettings)
    {
      if (externalLateFeeSettings == null)
        return;
      if (string.IsNullOrEmpty(this.Val("3927")))
        this.SetVal("3927", externalLateFeeSettings.GracePeriodDays.ToString());
      if (externalLateFeeSettings.DayCleared == 2)
        this.SetVal("4112", "Cleared for Purchase Date");
      if (externalLateFeeSettings.DayCleared == 4)
      {
        try
        {
          this.SetVal("4112", this.sessionObjects.LoanManager.CalculateDescription(this.calObjs.ExternalLateFeeSettings.DayClearedOtherDate));
        }
        catch
        {
        }
      }
      if (externalLateFeeSettings.DayCleared == 1)
        this.SetVal("4112", "Purchase Approval Date");
      if (!string.IsNullOrEmpty(this.Val("3567")))
        return;
      this.SetVal("3931", externalLateFeeSettings.LateFee.ToString());
      this.SetVal("3932", externalLateFeeSettings.Amount.ToString());
    }

    private void calculatefundingWorksheetULDD(string id, string val)
    {
      if (this.Val("ULDD.X191") != string.Empty)
        this.SetVal("ULDD.X193", "Y");
      else
        this.SetVal("ULDD.X193", "N");
    }

    private void loan_FieldChanged(object source, FieldChangedEventArgs e)
    {
      if (this.calObjs.SkipFieldChangeEvent)
        return;
      if (e.FieldID == "1172")
      {
        if (e.NewValue == "FHA" && e.PriorValue == "Conventional")
        {
          this.internalInvoke = true;
          this.validateCountyLimit("1172", "FHA");
          this.internalInvoke = false;
        }
        if (e.NewValue != e.PriorValue)
        {
          if (e.PriorValue == "FarmersHomeAdministration" && this.Val("NEWHUD.X1299") == "Guarantee Fee")
            this.SetVal("NEWHUD.X1305", "");
          this.calObjs.USDACal.UpdateField234("1172", e.NewValue);
          this.calObjs.USDACal.ClearMIPFields("1172", e.NewValue);
          this.calObjs.USDACal.ClearLine819("1172", e.NewValue);
        }
      }
      else if (e.FieldID == "NEWHUD.X1068" || e.FieldID == "VEND.X293")
        this.calObjs.NewHudCal.UsingTableFunded = this.Val("NEWHUD.X1068") == "Y" && this.Val("VEND.X293") != string.Empty;
      else if (e.FieldID == "3969")
      {
        this.calObjs.NewHud2015FeeDetailCal.ClearTaxStampIndicatorFor1204and1205(e.FieldID, e.NewValue);
        if (Utils.CheckIf2015RespaTila(e.NewValue))
          this.calObjs.NewHud2015FeeDetailCal.Calc_2015AllFeeDetails(e.FieldID, e.NewValue);
        this.calObjs.NewHudCal.FormCal(e.FieldID, e.NewValue);
      }
      if (this.sessionObjects.StartupInfo.ServerLicense.Edition != EncompassEdition.Banker)
        return;
      if (e.FieldID != string.Empty && this.Val("LCP.X19") == string.Empty && (e.FieldID == "1172" || e.FieldID == "1811" || this.configInfo != null && this.configInfo.LoanCompDefaultPlan != null && string.Compare(this.configInfo.LoanCompDefaultPlan.TriggerField, e.FieldID, true) == 0))
      {
        try
        {
          this.UpdateLOCompensation(e.FieldID, e.NewValue);
        }
        catch (Exception ex)
        {
        }
      }
      if (e.FieldID == "2626" || e.FieldID == "TPO.X15" || e.FieldID == "TPO.X62" || e.FieldID == "LOID" || e.FieldID == "14" || e.FieldID == "TPO.X39")
      {
        try
        {
          this.UpdateClosingVendorInformation(e.FieldID, e.NewValue);
        }
        catch (Exception ex)
        {
        }
      }
      if (!(e.FieldID == "TPO.X15") || this.sessionObjects == null || this.sessionObjects.ConfigurationManager == null)
        return;
      this.calObjs.ExternalLateFeeSettings = this.sessionObjects.ConfigurationManager.GetExternalOrgLateFeeSettings(e.NewValue, true);
    }

    private void refreshInvestorStatus(string id, string val)
    {
      Tracing.Log(ToolCalculation.sw, TraceLevel.Info, nameof (ToolCalculation), "routine: refreshInvestorStatus");
      string str1 = this.Val("2819");
      string val1 = this.Val("2014");
      string val2 = this.Val("2370");
      string str2 = this.Val("2031");
      string str3 = "";
      LockRequestLog currentLockRequest = this.loan.GetLogList().GetCurrentLockRequest();
      if (currentLockRequest != null)
        str3 = string.Concat(currentLockRequest.GetLockRequestSnapshot()[(object) "2286"]);
      if (id != null && Utils.IsEmptyValue(this.Val(id), this.loan.GetFormat(id)) && !(str2 != "Rejected"))
        return;
      if (Utils.IsDate((object) val2))
      {
        this.SetVal("2031", "Purchased");
        this.SetVal("2033", val2);
      }
      else if (Utils.IsDate((object) val1))
      {
        this.SetVal("2031", "Shipped");
        this.SetVal("2033", val1);
      }
      else if ((str1 ?? "") != "")
      {
        if (!(str2 != "AssignedBulk"))
          return;
        this.SetVal("2031", "AssignedBulk");
        this.SetVal("2033", DateTime.Now.ToString("MM/dd/yyyy"));
      }
      else if ((str3 ?? "") != "")
      {
        if (!(str2 != "AssignedFlow"))
          return;
        this.SetVal("2031", "AssignedFlow");
        this.SetVal("2033", DateTime.Now.ToString("MM/dd/yyyy"));
      }
      else
      {
        if (!(str2 != ""))
          return;
        this.SetVal("2031", "");
        this.SetVal("2033", DateTime.Now.ToString("MM/dd/yyyy"));
      }
    }

    private void updateSystemIdGuid(string id, string val)
    {
      if (string.Compare(this.Val("SYS.X610"), this.loan.SystemID, true) == 0)
        return;
      this.SetVal("SYS.X610", this.loan.SystemID);
    }

    private void calculateProfit(string id, string val)
    {
      if (Tracing.IsSwitchActive(ToolCalculation.sw, TraceLevel.Info))
        Tracing.Log(ToolCalculation.sw, TraceLevel.Info, nameof (ToolCalculation), "routine: calculateProfit");
      double num1 = this.FltVal("PM01") - (this.FltVal("PM03") + this.FltVal("PM05") + this.FltVal("PM07") + this.FltVal("PM09"));
      this.SetCurrentNum("PM10", num1);
      double profit = num1;
      double loanAmt = this.FltVal("1109");
      double num2 = this.getCommission(this.Val("PM12"), this.FltVal("PM11"), profit, loanAmt) + this.FltVal("PM13");
      this.SetCurrentNum("PM14", num2);
      double num3 = num1 - num2;
      double num4 = this.getCommission(this.Val("PM16"), this.FltVal("PM15"), profit, loanAmt) + this.FltVal("PM17");
      this.SetCurrentNum("PM18", num4);
      double num5 = num3 - num4;
      double num6 = this.getCommission(this.Val("PM20"), this.FltVal("PM19"), profit, loanAmt) + this.FltVal("PM21");
      this.SetCurrentNum("PM22", num6);
      double num7 = num5 - num6;
      double num8 = this.getCommission(this.Val("PM24"), this.FltVal("PM23"), profit, loanAmt) + this.FltVal("PM25");
      this.SetCurrentNum("PM26", num8);
      this.SetCurrentNum("PM28", num7 - num8);
    }

    private double getCommission(string type, double percent, double profit, double loanAmt)
    {
      switch (type)
      {
        case "Profit":
          return percent / 100.0 * profit;
        case "Loan":
          return percent / 100.0 * loanAmt;
        default:
          return 0.0;
      }
    }

    private void calculateTrust(string id, string val)
    {
      if (Tracing.IsSwitchActive(ToolCalculation.sw, TraceLevel.Info))
        Tracing.Log(ToolCalculation.sw, TraceLevel.Info, nameof (ToolCalculation), "routine: calculateTrust");
      double num1 = 0.0;
      double num2 = 0.0;
      for (int index = 1; index <= 16; ++index)
      {
        string id1 = "TA" + index.ToString("00") + "PA";
        num1 += this.FltVal(id1);
        string id2 = "TA" + index.ToString("00") + "RA";
        num2 += this.FltVal(id2);
      }
      this.SetCurrentNum("TATOTAL1", num1);
      this.SetCurrentNum("TATOTAL2", num2);
      this.SetCurrentNum("TABALANCE", num2 - num1);
    }

    internal void UpdatePurchaseAdviceBalance(string id, string val)
    {
      string str = this.Val("2371");
      DateTime date1 = Utils.ParseDate((object) this.Val("2370"));
      double num1 = this.FltVal("2207");
      if (num1 != 0.0)
        this.SetCurrentNum("2208", this.FltVal("2") * ((num1 - 100.0) / 100.0));
      else
        this.SetCurrentNum("2208", 0.0);
      this.SetCurrentNum("2210", this.FltVal("2") * (this.FltVal("2209") / 100.0));
      if (this.Val("SERVICE.X8") != string.Empty)
      {
        PaymentScheduleSnapshot scheduleSnapshot = this.loan.GetPaymentScheduleSnapshot();
        if (scheduleSnapshot != null)
        {
          this.SetCurrentNum("2593", this.getExpectedPurchaseAdviseFormPrinciple(scheduleSnapshot.MonthlyPayments));
          this.SetCurrentNum("2837", this.getExpectedEscrowBalanceToInvestor(scheduleSnapshot.EscrowPayments));
        }
        if (str == "Lender")
        {
          double num2 = this.FltVal("2593");
          this.SetCurrentNum("2594", num2 * (this.FltVal("2207") - 100.0) / 100.0);
          this.SetCurrentNum("2595", num2 * this.FltVal("2209") / 100.0);
        }
        else
        {
          this.SetCurrentNum("2594", (this.FltVal("SERVICE.X57") + this.FltVal("SERVICE.X139")) * (this.FltVal("2207") - 100.0) / 100.0);
          this.SetCurrentNum("2595", (this.FltVal("SERVICE.X57") + this.FltVal("SERVICE.X139")) * this.FltVal("2209") / 100.0);
        }
      }
      else
      {
        if (str == "Lender")
        {
          this.SetCurrentNum("2594", this.FltVal("2208"));
        }
        else
        {
          this.SetCurrentNum("2594", this.FltVal("2208"));
          this.SetCurrentNum("2595", this.FltVal("2210"));
        }
        PaymentScheduleSnapshot paymentSchedule = this.loan.Calculator.GetPaymentSchedule(false);
        if (paymentSchedule != null)
          this.SetCurrentNum("2593", this.getExpectedPurchaseAdviseFormPrinciple(paymentSchedule.MonthlyPayments));
        this.SetCurrentNum("2837", this.getExpectedEscrowBalanceToInvestor());
      }
      DateTime date2 = Utils.ParseDate((object) this.Val("3514"));
      double val1;
      if (date1 == DateTime.MinValue || date2 == DateTime.MinValue || date2.Day != 1)
      {
        val1 = 0.0;
      }
      else
      {
        DateTime date3 = Utils.ParseDate((object) this.Val("748"));
        DateTime dateTime1 = date2.AddMonths(-1);
        int num3;
        if (date1.Date == date2.Date && date2.Date == date3.Date)
          num3 = 0;
        else if (date1.Date == date2.Date)
        {
          DateTime dateTime2 = date1.Date;
          dateTime2 = dateTime2.AddMonths(-1);
          int year = dateTime2.Year;
          dateTime2 = date1.Date;
          dateTime2 = dateTime2.AddMonths(-1);
          int month = dateTime2.Month;
          num3 = DateTime.DaysInMonth(year, month);
        }
        else if (date1.Year == dateTime1.Year && date1.Month == dateTime1.Month)
        {
          num3 = date3.Year != dateTime1.Year || date3.Month != dateTime1.Month ? date1.Day - 1 : date1.Day - date3.Day;
        }
        else
        {
          DateTime dateTime3 = dateTime1.AddDays(-1.0);
          num3 = (Utils.GetTotalTimeSpanDays(date1.Date, dateTime3.Date) + 1) * -1;
        }
        double num4 = !(this.Val("3549") == "") ? this.FltVal("3549") : 360.0;
        switch (this.Val("3550"))
        {
          case "2 Decimals":
            val1 = Utils.ArithmeticRounding(this.FltVal("2593") * (this.FltVal("3") / num4 / 100.0), 2) * (double) num3;
            break;
          case "4 Decimals":
            val1 = Utils.ArithmeticRounding(this.FltVal("2593") * (this.FltVal("3") / num4 / 100.0), 4) * (double) num3;
            break;
          default:
            val1 = this.FltVal("2593") * (this.FltVal("3") / num4 / 100.0) * (double) num3;
            break;
        }
      }
      this.SetCurrentNum("2836", Utils.ArithmeticRounding(val1, 2));
      this.calculateRateLock((string) null, (string) null);
      this.calculateSellSideSRPAndPrice((string) null, (string) null);
    }

    private void calculateCorrespondentPurchaseAdvice(string id, string val)
    {
      double num1 = this.FltVal("3579");
      double num2 = this.FltVal("2203") + this.FltVal("3573") + this.FltVal("3575") + this.FltVal("3577") + this.FltVal("3938");
      this.SetCurrentNum("3578", num2);
      this.SetCurrentNum("3583", num1 * (num2 - 100.0) / 100.0 * -1.0);
      if (!this.sessionObjects.GetPolicySetting("EnablePaymentHistoryAndCalc"))
      {
        DateTime date1 = Utils.ParseDate((object) this.Val("3567"));
        DateTime date2 = Utils.ParseDate((object) this.Val("3570"));
        if (date2 != DateTime.MinValue && date1 != DateTime.MinValue)
          this.SetCurrentNum("3581", (double) ((int) (date2 - date1).TotalDays - Utils.ParseDate((object) (date2.Month.ToString("00") + "/01/" + (object) date2.Year)).AddDays(-1.0).Day));
      }
      int num3 = this.IntVal("3549");
      if (num3 != 0)
      {
        string str = this.Val("3550");
        double val1 = num1 * this.FltVal("3") / 100.0 / (double) num3;
        switch (str)
        {
          case "2 Decimals":
            val1 = Utils.ArithmeticRounding(val1, 2);
            break;
          case "4 Decimals":
            val1 = Utils.ArithmeticRounding(val1, 4);
            break;
        }
        this.SetCurrentNum("3580", val1 * (double) this.IntVal("3581"));
      }
      this.SetCurrentNum("3584", Utils.ArithmeticRounding(-1.0 * (num1 * this.FltVal("2205") / 100.0), 2));
      double num4 = num1 - this.FltVal("3580") - this.FltVal("3582") - this.FltVal("3583") - this.FltVal("3584") - this.FltVal("3585") - this.FltVal("CORRESPONDENT.X405");
      for (int index = 3588; index <= 3610; index += 2)
        num4 -= this.FltVal(string.Concat((object) index));
      this.SetCurrentNum("3611", num4 - (this.FltVal("CORRESPONDENT.X64") + this.FltVal("CORRESPONDENT.X66") + this.FltVal("3970") + this.FltVal("3939")));
      this.SetCurrentNum("4192", this.FltVal("3939") + this.FltVal("3588") + this.FltVal("3590") + this.FltVal("3592") + this.FltVal("3594") + this.FltVal("3596") + this.FltVal("3598") + this.FltVal("3600") + this.FltVal("3602") + this.FltVal("3604") + this.FltVal("3606") + this.FltVal("3608") + this.FltVal("3610") + this.FltVal("CORRESPONDENT.X64") + this.FltVal("CORRESPONDENT.X66") + this.FltVal("3970"));
      this.SetCurrentNum("4191", this.FltVal("3583") + this.FltVal("3584"));
    }

    internal void UpdatePurchasedPrincipal()
    {
      int num1 = this.IntVal("1177");
      DateTime date1 = Utils.ParseDate((object) this.Val("682"));
      DateTime dateTime = date1.AddMonths(num1);
      DateTime date2 = Utils.ParseDate((object) this.Val("3569"));
      DateTime date3 = Utils.ParseDate((object) this.Val("3570"));
      if (date2 != DateTime.MinValue && date3 != DateTime.MinValue && DateTime.Compare(date3.Date, date2.Date) >= 0)
      {
        bool flag = true;
        int num2 = 0;
        if (date2.Year == date3.Year && date3.Month - date2.Month > 1)
        {
          flag = false;
          num2 = date3.Month - date2.Month;
        }
        else if (date3.Year > date2.Year && date3.Month + 12 - date2.Month > 1)
        {
          flag = false;
          num2 = date3.Month + 12 - date2.Month;
        }
        if (flag || date1 != DateTime.MinValue && num1 > 0 && DateTime.Compare(date3.Date, date1.Date) >= 0 && DateTime.Compare(date3.Date, dateTime.Date) <= 0)
        {
          this.SetCurrentNum("3579", this.FltVal("3571"));
        }
        else
        {
          if (flag)
            return;
          double num3 = this.FltVal("3571");
          double num4 = this.FltVal("3") / 100.0 / 12.0;
          double monthlyPayment = RegzCalculation.CalculateMonthlyPayment(this.IntVal("4"), num1, this.FltVal("2"), this.FltVal("3"));
          for (int index = 1; index < num2; ++index)
            num3 -= monthlyPayment - num4 * num3;
          this.SetCurrentNum("3579", num3);
        }
      }
      else
        this.SetCurrentNum("3579", this.FltVal("3571"));
    }

    internal void UpdateCorrespondentPurchaseAdvice(string id, string val)
    {
      int months = this.IntVal("1177");
      Utils.ParseDate((object) this.Val("682")).AddMonths(months);
      DateTime date1 = Utils.ParseDate((object) this.Val("3569"));
      DateTime date2 = Utils.ParseDate((object) this.Val("3570"));
      this.UpdatePurchasedPrincipal();
      if (date1 != DateTime.MinValue && date2 != DateTime.MinValue && DateTime.Compare(date2.Date, date1.Date) >= 0)
      {
        if (date1.Year == date2.Year && date2.Month - date1.Month > 1 || date2.Year > date1.Year && date2.Month + 12 - date1.Month > 1)
        {
          string empty = string.Empty;
          string str1 = date1.Month.ToString("00");
          int num1 = date1.Year;
          string str2 = num1.ToString().Substring(2);
          string str3 = str1 + "/" + str2;
          num1 = date2.Month;
          string str4 = num1.ToString("00");
          num1 = date2.Year;
          string str5 = num1.ToString().Substring(2);
          string str6 = str4 + "/" + str5;
          double val1 = this.FltVal("3568");
          double num2 = this.FltVal("HUD26");
          for (int index1 = 1; index1 <= 24; ++index1)
          {
            string str7 = this.Val("HUD" + index1.ToString("00") + "01");
            if (str3 == str7)
            {
              for (int index2 = index1 + 1; index2 <= 24; ++index2)
              {
                string str8 = this.Val("HUD" + index2.ToString("00") + "01");
                if (str6 == str8)
                {
                  index1 = 25;
                  break;
                }
                val1 = val1 + num2 - this.FltVal("HUD" + index2.ToString("00") + "10");
              }
            }
          }
          this.SetCurrentNum("3582", Utils.ArithmeticRounding(val1, 2));
        }
        else
          this.SetCurrentNum("3582", this.FltVal("3568"));
      }
      else
        this.SetCurrentNum("3582", this.FltVal("3568"));
      if (this.sessionObjects.GetPolicySetting("EnablePaymentHistoryAndCalc") && this.loan.GetField("CPA.PaymentHistory.AnticipatedPurchaseDate") != "//" && !string.IsNullOrEmpty(this.loan.GetField("CPA.PaymentHistory.AnticipatedPurchaseDate")))
      {
        this.loan.SetField("3549", this.sessionObjects.ConfigurationManager.GetCompanySetting("Policies", "PerDiemInterestDaysPerYear"));
        this.loan.SetField("3550", this.sessionObjects.ConfigurationManager.GetCompanySetting("Policies", "PerDiemInterestRounding"));
        int num3 = Utils.ParseInt((object) this.sessionObjects.ConfigurationManager.GetCompanySetting("Policies", "CutoffCalendarDay"));
        DateTime date3 = Utils.ParseDate((object) this.loan.GetField("CPA.PaymentHistory.AnticipatedPurchaseDate"));
        DateTime dateTime = Utils.ParseDate((object) this.loan.GetField("CPA.PaymentHistory.FirstBorrowerPaymentDueDate")).AddMonths(-1);
        int num4;
        if (date3 < dateTime)
        {
          num4 = (dateTime - date3).Days;
        }
        else
        {
          int num5 = date3.Day - 1;
          if (date3.Day >= num3)
          {
            int num6 = DateTime.DaysInMonth(date3.Year, date3.Month);
            num5 -= num6;
          }
          num4 = num5 * -1;
        }
        this.SetCurrentNum("3581", (double) num4);
      }
      this.calculateCorrespondentPurchaseAdvice(id, val);
      this.calculateCPAEscrowDetails_AddlEscrow_TotalFields(id, val);
    }

    private void copyFromCorrespondentPurchaseAdvice(string id, string value)
    {
      if (!(id == "3567") || !(this.Val("2626") == "Correspondent"))
        return;
      this.SetVal("3922", this.Val("3567"));
    }

    private void updatePercentageOwnershipInterest(string id, string value)
    {
      if (!(id == "AFF.X3") || !string.IsNullOrEmpty(value))
        return;
      this.SetVal("AFF.X32", "");
    }

    private double getExpectedPurchaseAdviseFormPrinciple(PaymentSchedule[] paySchedule)
    {
      DateTime date1 = Utils.ParseDate((object) this.Val("3514"), DateTime.MinValue);
      DateTime date2 = Utils.ParseDate((object) this.Val("682"), DateTime.MinValue);
      if (date1 == DateTime.MinValue || date2 == DateTime.MinValue || date1 < date2)
        return this.FltVal("2");
      int num1 = paySchedule.Length - 1;
      if (this.Val("SERVICE.X8") != string.Empty)
      {
        double adviseFormPrinciple = Utils.ParseDouble((object) this.Val("SERVICE.X57"), 0.0) + Utils.ParseDouble((object) this.Val("SERVICE.X139"), 0.0);
        DateTime date3 = Utils.ParseDate((object) this.Val("SERVICE.X14"), DateTime.MinValue);
        if (this.IsLocked("SERVICE.X14"))
          date3 = Utils.ParseDate((object) this.GetFieldFromCal("SERVICE.X14"), DateTime.MinValue);
        if (date3 == DateTime.MinValue)
          return 0.0;
        for (int index = 0; index < num1; ++index)
        {
          PaymentSchedule paymentSchedule = paySchedule[index];
          if (paymentSchedule != null)
          {
            DateTime date4 = Utils.ParseDate((object) paymentSchedule.PayDate, DateTime.MinValue);
            if (!(date4 < date3))
            {
              if (!(date4 >= date1))
                adviseFormPrinciple -= paymentSchedule.Principal;
              else
                break;
            }
          }
          else
            break;
        }
        return adviseFormPrinciple;
      }
      double num2 = 0.0;
      for (int index = 0; index < num1; ++index)
      {
        DateTime date5 = Utils.ParseDate((object) paySchedule[index].PayDate, DateTime.MinValue);
        if (!(date5 < date2))
        {
          if (!(date5 >= date1))
            num2 += paySchedule[index].Principal;
          else
            break;
        }
      }
      return Utils.ParseDouble((object) this.Val("2"), 0.0) - num2;
    }

    private double getExpectedEscrowBalanceToInvestor()
    {
      double balanceToInvestor = this.FltVal("656") + this.FltVal("338") + this.FltVal("L269") + this.FltVal("655") + this.FltVal("657") + this.FltVal("1631") + this.FltVal("658") + this.FltVal("659") + this.FltVal("NEWHUD.X1708") + this.FltVal("596") + this.FltVal("563") + this.FltVal("L270") + this.FltVal("595") + this.FltVal("597") + this.FltVal("1632") + this.FltVal("598") + this.FltVal("599") + this.FltVal("558");
      DateTime date1 = Utils.ParseDate((object) this.Val("3514"), DateTime.MinValue);
      DateTime date2 = Utils.ParseDate((object) this.Val("682"), DateTime.MinValue);
      if (date1 == DateTime.MinValue || date2 == DateTime.MinValue || date1 < date2)
        return balanceToInvestor;
      if (date1.Month == date2.Month && date1.Year == date2.Year)
        return balanceToInvestor * -1.0;
      int num1 = 1;
      DateTime dateTime = date2;
      while (dateTime.AddMonths(1) < date1)
      {
        dateTime = dateTime.AddMonths(1);
        ++num1;
      }
      double num2 = balanceToInvestor + (double) num1 * Utils.ParseDouble((object) this.Val("HUD24"));
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      bool flag = this.Val("423") == "Biweekly";
      string empty3 = string.Empty;
      int num3 = flag ? 49 : 25;
      for (int index = 1; index < num3; ++index)
      {
        string id1 = index < 10 ? "HUD0" + (object) index + "01" : "HUD" + (object) index + "01";
        string id2 = index < 10 ? "HUD0" + (object) index + "10" : "HUD" + (object) index + "10";
        string str = this.Val(id1);
        if (!string.IsNullOrEmpty(str))
        {
          if (!flag)
            str = str.Substring(0, str.IndexOf('/')) + "/" + (object) date2.Day + "/" + str.Substring(str.IndexOf('/') + 1);
          if (Utils.ParseDate((object) str, DateTime.MinValue) < date1)
            num2 -= Utils.ParseDouble((object) this.Val(id2), 0.0);
          else
            break;
        }
      }
      return num2 * -1.0;
    }

    private double getExpectedEscrowBalanceToInvestor(EscrowSchedule[] escrowSchedule)
    {
      DateTime date1 = Utils.ParseDate((object) this.Val("3514"), DateTime.MinValue);
      DateTime date2 = Utils.ParseDate((object) this.Val("682"), DateTime.MinValue);
      if (date1 == DateTime.MinValue || date2 == DateTime.MinValue || date1 < date2)
        return this.FltVal("Service.X81");
      int num1 = escrowSchedule.Length - 1;
      double num2 = Utils.ParseDouble((object) this.Val("Service.X81"), 0.0);
      DateTime date3 = Utils.ParseDate((object) this.Val("SERVICE.X14"), DateTime.MinValue);
      if (this.IsLocked("SERVICE.X14"))
        date3 = Utils.ParseDate((object) this.GetFieldFromCal("SERVICE.X14"), DateTime.MinValue);
      if (date3 == DateTime.MinValue)
        return 0.0;
      int num3 = 0;
      if (date3 < date1)
      {
        num3 = 1;
        DateTime dateTime = date3;
        while (dateTime.AddMonths(1) < date1)
        {
          dateTime = dateTime.AddMonths(1);
          ++num3;
        }
      }
      double num4 = num2 + (double) num3 * Utils.ParseDouble((object) this.Val("SERVICE.X20"));
      for (int index = 0; index < num1; ++index)
      {
        EscrowSchedule escrowSchedule1 = escrowSchedule[index];
        if (escrowSchedule1 != null)
        {
          DateTime date4 = Utils.ParseDate((object) escrowSchedule1.PayDate, DateTime.MinValue);
          if (!(date4 < date3))
          {
            if (!(date4 >= date1))
              num4 -= escrowSchedule1.TotalPayment;
            else
              break;
          }
        }
        else
          break;
      }
      return num4 * -1.0;
    }

    private void calculateSellSideSRPAndPrice(string id, string val)
    {
      int num1 = 2207;
      int num2 = 3423;
      for (int index = 3422; index <= 3428; index += 2)
      {
        double num3 = this.FltVal(index.ToString()) - this.FltVal(num1.ToString());
        this.SetCurrentNum(num2.ToString(), num3);
        ++num1;
        num2 += 2;
      }
    }

    private void updateSellSideSRPANDPriceBalance(string id, string val)
    {
      if (id == "2211" && val == "" || id == null && this.FltVal("2211") == 0.0)
      {
        this.SetCurrentNum("3422", 0.0);
        this.updateSellSideSRPANDPriceBalance("3422", "0");
        this.SetCurrentNum("3424", 0.0);
        this.SetCurrentNum("3426", 0.0);
        this.updateSellSideSRPANDPriceBalance("3426", "0");
        this.SetCurrentNum("3428", 0.0);
      }
      else if (id == "2211" && val != "")
      {
        if (this.Val("3430") == "Y")
        {
          this.updateSellSideSRPANDPriceBalance("3422", string.Concat((object) this.FltVal("3422")));
          this.updateSellSideSRPANDPriceBalance("3426", string.Concat((object) this.FltVal("3426")));
        }
        else
        {
          this.updateSellSideSRPANDPriceBalance("3424", string.Concat((object) this.FltVal("3424")));
          this.updateSellSideSRPANDPriceBalance("3428", string.Concat((object) this.FltVal("3428")));
        }
      }
      else
      {
        this.SetCurrentNum("2212", this.FltVal("3424"));
        this.SetCurrentNum("2213", this.FltVal("3428"));
        switch (id)
        {
          case "3422":
            double num1 = this.FltVal(id);
            if (num1 != 0.0)
            {
              double num2 = this.FltVal("2211") * ((num1 - 100.0) / 100.0);
              this.SetCurrentNum("3424", num2);
              this.SetCurrentNum("2212", num2);
              break;
            }
            this.SetCurrentNum("3424", 0.0);
            this.SetCurrentNum("2212", 0.0);
            break;
          case "3424":
            double num3 = this.FltVal(id);
            this.SetCurrentNum("2212", num3);
            if (num3 != 0.0)
            {
              this.SetCurrentNum("3422", num3 / this.FltVal("2211") * 100.0 + 100.0);
              break;
            }
            this.SetCurrentNum("3422", 0.0);
            break;
          case "3426":
            double num4 = this.FltVal("2211") * (this.FltVal("3426") / 100.0);
            this.SetCurrentNum("3428", num4);
            this.SetCurrentNum("2213", num4);
            break;
          case "3428":
            double num5 = this.FltVal(id);
            this.SetCurrentNum("2213", num5);
            this.SetCurrentNum("2611", this.FltVal("2213") - this.FltVal("2595"));
            if (num5 != 0.0)
            {
              this.SetCurrentNum("3426", num5 / this.FltVal("2211") * 100.0);
              break;
            }
            this.SetCurrentNum("3426", 0.0);
            break;
        }
        this.calculateSellSideSRPAndPrice((string) null, (string) null);
      }
    }

    private void calculateRateLock(string id, string val)
    {
      double num1 = this.FltVal("2094") + this.FltVal("2096") + this.FltVal("2098");
      for (int index = 2415; index <= 2447; index += 2)
        num1 += this.FltVal(index.ToString());
      this.SetCurrentNum("2099", num1);
      this.SetCurrentNum("2100", num1 + this.FltVal("2092"));
      double num2 = 0.0;
      for (int index = 2103; index <= 2141; index += 2)
        num2 += this.FltVal(index.ToString());
      for (int index = 3455; index <= 3473; index += 2)
        num2 += this.FltVal(index.ToString());
      for (int index = 4257; index <= 4275; index += 2)
        num2 += this.FltVal(index.ToString());
      for (int index = 4337; index <= 4355; index += 2)
        num2 += this.FltVal(index.ToString());
      double num3 = num2 + this.FltVal("4787");
      this.SetCurrentNum("2142", num3);
      this.SetCurrentNum("2143", num3 + this.FltVal("2101"));
      double num4 = 0.0;
      for (int index = 2649; index <= 2687; index += 2)
        num4 += this.FltVal(index.ToString());
      this.SetCurrentNum("2688", num4);
      this.SetCurrentNum("2689", num4 + this.FltVal("2647"));
      double num5 = 0.0;
      double num6 = 0.0;
      int num7 = 2596;
      int num8 = 2612;
      for (int index = 2373; index <= 2395; index += 2)
      {
        num5 += this.FltVal(index.ToString());
        num6 += this.FltVal(num7.ToString());
        double num9 = this.FltVal(index.ToString()) - this.FltVal(num7.ToString());
        this.SetCurrentNum(num8.ToString(), num9);
        ++num7;
        ++num8;
      }
      int num10 = 2593;
      num8 = 2609;
      for (int index = 2211; index <= 2213; ++index)
      {
        num5 += this.FltVal(index.ToString());
        num6 += this.FltVal(num10.ToString());
        double num11 = this.FltVal(index.ToString()) - this.FltVal(num10.ToString());
        this.SetCurrentNum(num8.ToString(), num11);
        ++num10;
        ++num8;
      }
      int num12 = 2836;
      num8 = 2838;
      for (int index = 2834; index <= 2835; ++index)
      {
        num5 += this.FltVal(index.ToString());
        num6 += this.FltVal(num12.ToString());
        double num13 = this.FltVal(index.ToString()) - this.FltVal(num12.ToString());
        this.SetCurrentNum(num8.ToString(), num13);
        ++num12;
        ++num8;
      }
      double num14 = num5 + this.FltVal("3131");
      double num15 = num6 + this.FltVal("3130");
      this.SetCurrentNum("3132", this.FltVal("3131") - this.FltVal("3130"));
      this.SetCurrentNum("2214", num14);
      this.SetCurrentNum("2608", num15);
      double num16 = num14 - num15;
      this.SetCurrentNum("2624", num16);
      if (num16 < 0.0)
      {
        num16 = Math.Abs(num16);
        this.SetVal("2627", "Lender");
        this.SetVal("2628", "Lender");
      }
      else if (num16 > 0.0)
      {
        this.SetVal("2627", "Investor");
        this.SetVal("2628", "Investor");
      }
      else
      {
        this.SetVal("2627", "");
        this.SetVal("2628", "");
      }
      this.SetCurrentNum("2631", num16);
      if (this.sessionObjects.ServerLicense.IsBankerEdition)
        this.SetCurrentNum("2629", num16 - this.FltVal("2632"));
      else
        this.SetVal("2629", "");
    }

    internal void CalcInterimServicing(bool current)
    {
      if (this.loan == null)
        return;
      DateTime dateTime1 = DateTime.Today;
      int year = dateTime1.Year;
      int num1 = 0;
      int num2 = 0;
      int num3 = 0;
      double num4 = 0.0;
      double num5 = 0.0;
      double num6 = 0.0;
      double num7 = 0.0;
      double num8 = 0.0;
      double num9 = 0.0;
      double num10 = 0.0;
      double num11 = 0.0;
      double num12 = 0.0;
      double num13 = 0.0;
      double num14 = 0.0;
      double num15 = 0.0;
      double num16 = 0.0;
      double num17 = 0.0;
      double num18 = 0.0;
      double num19 = 0.0;
      double num20 = 0.0;
      double num21 = 0.0;
      int num22 = 0;
      double num23 = 0.0;
      double num24 = 0.0;
      double num25 = 0.0;
      double num26 = 0.0;
      double num27 = 0.0;
      double payOff1 = 0.0;
      double payOff2 = 0.0;
      double payOff3 = 0.0;
      double num28 = 0.0;
      PaymentTransactionLog lastPayTransLog = (PaymentTransactionLog) null;
      Hashtable hashtable1 = (Hashtable) null;
      ServicingTransactionBase[] servicingTransactions = this.loan.GetServicingTransactions();
      if (this.paySnapshot == null || servicingTransactions == null || servicingTransactions.Length == 0)
        this.loadPaymentSnapshot();
      if (this.Val("2626") == "Correspondent")
        this.SetCurrentNum("SERVICE.X144", Utils.ParseDouble((object) this.Val("3579")));
      else
        this.SetCurrentNum("SERVICE.X144", Utils.ParseDouble((object) this.Val("2")));
      double num29 = this.FltVal("SERVICE.X144");
      double unpaidBalance = num29;
      int nextPaymentIndex = 0;
      Hashtable schedulePays1 = this.loadRealPaymentSchedule(ref nextPaymentIndex, servicingTransactions);
      double[] escrow = new double[9]
      {
        this.FltVal("231"),
        this.FltVal("230"),
        this.FltVal("232"),
        this.FltVal("235"),
        this.FltVal("L268"),
        this.FltVal("1630"),
        this.FltVal("253"),
        this.FltVal("254"),
        this.FltVal("NEWHUD.X1707")
      };
      string field1 = this.loan.GetField("HUD0141");
      if (string.IsNullOrWhiteSpace(field1) || field1 == "//")
        escrow[0] = 0.0;
      string field2 = this.loan.GetField("HUD0142");
      if (string.IsNullOrWhiteSpace(field2) || field2 == "//")
        escrow[1] = 0.0;
      string field3 = this.loan.GetField("HUD0143");
      if (string.IsNullOrWhiteSpace(field3) || field3 == "//")
        escrow[2] = 0.0;
      string field4 = this.loan.GetField("HUD0144");
      if (string.IsNullOrWhiteSpace(field4) || field4 == "//")
        escrow[3] = 0.0;
      string field5 = this.loan.GetField("HUD0145");
      if (string.IsNullOrWhiteSpace(field5) || field5 == "//")
        escrow[4] = 0.0;
      string field6 = this.loan.GetField("HUD0146");
      if (string.IsNullOrWhiteSpace(field6) || field6 == "//")
        escrow[5] = 0.0;
      string field7 = this.loan.GetField("HUD0147");
      if (string.IsNullOrWhiteSpace(field7) || field7 == "//")
        escrow[6] = 0.0;
      string field8 = this.loan.GetField("HUD0148");
      if (string.IsNullOrWhiteSpace(field8) || field8 == "//")
        escrow[7] = 0.0;
      string field9 = this.loan.GetField("HUD0149");
      if (string.IsNullOrWhiteSpace(field9) || field9 == "//")
        escrow[8] = 0.0;
      if (servicingTransactions != null)
      {
        hashtable1 = new Hashtable();
        for (int index = 0; index < servicingTransactions.Length; ++index)
        {
          if (servicingTransactions[index] is PaymentReversalLog)
          {
            PaymentReversalLog paymentReversalLog = (PaymentReversalLog) servicingTransactions[index];
            if (!hashtable1.ContainsKey((object) paymentReversalLog.PaymentGUID))
              hashtable1.Add((object) paymentReversalLog.PaymentGUID, (object) paymentReversalLog);
          }
        }
        for (int index = 0; index < servicingTransactions.Length; ++index)
        {
          if (!hashtable1.ContainsKey((object) servicingTransactions[index].TransactionGUID))
          {
            if (servicingTransactions[index] is EscrowDisbursementLog)
            {
              EscrowDisbursementLog escrowDisbursementLog = (EscrowDisbursementLog) servicingTransactions[index];
              ++num22;
              switch (escrowDisbursementLog.DisbursementType)
              {
                case ServicingDisbursementTypes.Taxes:
                  num27 += escrowDisbursementLog.TransactionAmount;
                  continue;
                case ServicingDisbursementTypes.HazardInsurance:
                  num25 += escrowDisbursementLog.TransactionAmount;
                  continue;
                case ServicingDisbursementTypes.MortgageInsurance:
                  num26 += escrowDisbursementLog.TransactionAmount;
                  continue;
                case ServicingDisbursementTypes.FloodInsurance:
                  num24 += escrowDisbursementLog.TransactionAmount;
                  continue;
                case ServicingDisbursementTypes.CityPropertyTax:
                  num23 += escrowDisbursementLog.TransactionAmount;
                  continue;
                case ServicingDisbursementTypes.Other1:
                  payOff1 += escrowDisbursementLog.TransactionAmount;
                  continue;
                case ServicingDisbursementTypes.Other2:
                  payOff2 += escrowDisbursementLog.TransactionAmount;
                  continue;
                case ServicingDisbursementTypes.Other3:
                  payOff3 += escrowDisbursementLog.TransactionAmount;
                  continue;
                case ServicingDisbursementTypes.USDAMonthlyPremium:
                  num28 += escrowDisbursementLog.TransactionAmount;
                  continue;
                default:
                  continue;
              }
            }
            else if (servicingTransactions[index] is EscrowInterestLog)
              num9 += servicingTransactions[index].TransactionAmount;
            else if (!(servicingTransactions[index] is SchedulePaymentLog))
            {
              if (servicingTransactions[index] is PaymentTransactionLog)
              {
                PaymentTransactionLog paymentTransactionLog = (PaymentTransactionLog) servicingTransactions[index];
                ++num1;
                num4 += paymentTransactionLog.Principal;
                num6 += paymentTransactionLog.Interest;
                num8 += paymentTransactionLog.Escrow;
                num13 += paymentTransactionLog.LateFee;
                num15 += paymentTransactionLog.MiscFee;
                num11 += paymentTransactionLog.BuydownSubsidyAmount;
                num17 += paymentTransactionLog.AdditionalPrincipal;
                num19 += paymentTransactionLog.AdditionalEscrow;
                num29 -= paymentTransactionLog.AdditionalPrincipal + paymentTransactionLog.Principal;
                dateTime1 = paymentTransactionLog.PaymentReceivedDate;
                if (dateTime1.Year == year)
                {
                  num5 += paymentTransactionLog.Principal;
                  num7 += paymentTransactionLog.Interest;
                  num10 += paymentTransactionLog.Escrow;
                  num12 += paymentTransactionLog.BuydownSubsidyAmount;
                  num14 += paymentTransactionLog.LateFee;
                  num16 += paymentTransactionLog.MiscFee;
                  num18 += paymentTransactionLog.AdditionalPrincipal;
                  num20 += paymentTransactionLog.AdditionalEscrow;
                }
                if (paymentTransactionLog.PaymentNo > num3)
                  lastPayTransLog = paymentTransactionLog;
                if (schedulePays1.ContainsKey((object) paymentTransactionLog.PaymentIndexDate))
                {
                  SchedulePaymentLog schedulePaymentLog = (SchedulePaymentLog) schedulePays1[(object) paymentTransactionLog.PaymentIndexDate];
                  schedulePaymentLog.PaymentReceivedDate = paymentTransactionLog.PaymentReceivedDate;
                  schedulePaymentLog.Principal += paymentTransactionLog.Principal;
                  schedulePaymentLog.Interest += paymentTransactionLog.Interest;
                  schedulePaymentLog.Escrow += paymentTransactionLog.Escrow;
                  schedulePaymentLog.BuydownSubsidyAmount += paymentTransactionLog.BuydownSubsidyAmount;
                  schedulePaymentLog.MiscFee += paymentTransactionLog.MiscFee;
                  schedulePaymentLog.LateFee += paymentTransactionLog.LateFee;
                  schedulePaymentLog.AdditionalPrincipal += paymentTransactionLog.AdditionalPrincipal;
                  schedulePaymentLog.AdditionalEscrow += paymentTransactionLog.AdditionalEscrow;
                  schedulePaymentLog.Taxes += paymentTransactionLog.EscowTaxes;
                  schedulePaymentLog.HazardInsurance += paymentTransactionLog.HazardInsurance;
                  schedulePaymentLog.MortgageInsurance += paymentTransactionLog.MortgageInsurance;
                  schedulePaymentLog.FloodInsurance += paymentTransactionLog.FloodInsurance;
                  schedulePaymentLog.CityPropertytax += paymentTransactionLog.CityPropertyTax;
                  schedulePaymentLog.Other1Escrow += paymentTransactionLog.Other1Escrow;
                  schedulePaymentLog.Other2Escrow += paymentTransactionLog.Other2Escrow;
                  schedulePaymentLog.Other3Escrow += paymentTransactionLog.Other3Escrow;
                  schedulePaymentLog.USDAMonthlyPremium += paymentTransactionLog.USDAMonthlyPremium;
                }
              }
              else if (servicingTransactions[index] is LoanPurchaseLog)
              {
                LoanPurchaseLog loanPurchaseLog = (LoanPurchaseLog) servicingTransactions[index];
                num4 += loanPurchaseLog.PurchaseAmount;
                Hashtable hashtable2 = schedulePays1;
                Hashtable schedulePays2 = schedulePays1;
                dateTime1 = loanPurchaseLog.PurchaseAdviceDate;
                string purchaseAdviceDate = dateTime1.ToString("MM/dd/yyyy");
                // ISSUE: variable of a boxed type
                __Boxed<DateTime> date = (ValueType) Utils.ToDate(this.getSchedulePaymentLogforPurchase(schedulePays2, purchaseAdviceDate));
                SchedulePaymentLog schedulePaymentLog = (SchedulePaymentLog) hashtable2[(object) date];
                if (schedulePaymentLog != null)
                  schedulePaymentLog.Principal += loanPurchaseLog.PurchaseAmount;
                num21 = loanPurchaseLog.PurchaseAmount;
              }
              else if (servicingTransactions[index] is PrincipalDisbursementLog)
              {
                PrincipalDisbursementLog principalDisbursementLog = (PrincipalDisbursementLog) servicingTransactions[index];
                num4 -= principalDisbursementLog.TransactionAmount;
                num5 -= principalDisbursementLog.TransactionAmount;
                num29 += principalDisbursementLog.TransactionAmount;
              }
            }
          }
        }
      }
      this.SetCurrentNum("SERVICE.X39", (double) num1);
      this.SetCurrentNum("SERVICE.X41", num4 + num17 - num21);
      this.SetCurrentNum("SERVICE.X42", num5 + num18);
      this.SetCurrentNum("SERVICE.X43", num6);
      this.SetCurrentNum("SERVICE.X44", num7);
      this.SetCurrentNum("SERVICE.X45", num4 + num6 + num17 - num21);
      this.SetCurrentNum("SERVICE.X46", num5 + num7 + num18);
      this.SetCurrentNum("SERVICE.X47", num8 + num19);
      this.SetCurrentNum("SERVICE.X48", num10 + num20);
      this.SetCurrentNum("SERVICE.X102", num11);
      this.SetCurrentNum("SERVICE.X103", num12);
      this.SetCurrentNum("SERVICE.X87", num15);
      this.SetCurrentNum("SERVICE.X88", num16);
      this.SetCurrentNum("SERVICE.X49", num13);
      this.SetCurrentNum("SERVICE.X50", num14);
      this.SetCurrentNum("SERVICE.X55", num4 + num6 + num8 + num13 + num15 + num17 + num19 + num11 - num21);
      this.SetCurrentNum("SERVICE.X56", num5 + num7 + num10 + num14 + num16 + num18 + num20 + num12);
      this.SetCurrentNum("SERVICE.X57", num29);
      if (current)
        this.copyFromInterimServicing("SERVICE.X57", this.Val("SERVICE.X57"));
      this.calObjs.ULDDExpCal.calculateFannieMaeExportFields("SERVICE.X57", this.Val("SERVICE.X57"));
      this.SetCurrentNum("SERVICE.X74", (double) num22);
      this.SetCurrentNum("SERVICE.X75", num27);
      this.SetCurrentNum("SERVICE.X76", num25);
      this.SetCurrentNum("SERVICE.X77", num26);
      this.SetCurrentNum("SERVICE.X89", num24);
      this.SetCurrentNum("SERVICE.X90", num23);
      this.SetCurrentNum("SERVICE.X79", payOff1 + payOff2 + payOff3);
      this.SetCurrentNum("SERVICE.X107", num28);
      this.SetCurrentNum("SERVICE.X80", payOff1 + payOff2 + payOff3 + num27 + num25 + num26 + num24 + num23 + num28);
      this.SetCurrentNum("SERVICE.X81", this.paySnapshot.GetNumField("ESCROWBEGINBALANCE") + num8 + num19 + num9 - this.FltVal("SERVICE.X80"));
      double num30 = Utils.ParseDouble((object) this.paySnapshot.GetField("3119"));
      if (num30 > 0.0)
      {
        if (servicingTransactions != null)
        {
          double[] numArray1 = new double[6];
          int[] numArray2 = new int[6];
          double[] numArray3 = new double[6];
          for (int index = 0; index < numArray1.Length; ++index)
          {
            numArray1[index] = Utils.ParseDouble((object) this.paySnapshot.GetField(string.Concat((object) (3097 + index * 4))), 0.0);
            numArray2[index] = Utils.ParseInt((object) this.paySnapshot.GetField(string.Concat((object) (index + 3124))), 0);
            numArray3[index] = Utils.ParseDouble((object) this.paySnapshot.GetField(string.Concat((object) (3096 + index * 4))), 0.0);
          }
          for (int index1 = 0; index1 < servicingTransactions.Length; ++index1)
          {
            if (!hashtable1.ContainsKey((object) servicingTransactions[index1].TransactionGUID) && servicingTransactions[index1] is PaymentTransactionLog)
            {
              double num31 = ((PaymentTransactionLog) servicingTransactions[index1]).BuydownSubsidyAmount;
              if (num31 != 0.0)
              {
                for (int index2 = 0; index2 < numArray1.Length; ++index2)
                {
                  if (numArray1[index2] - num31 >= 0.0)
                  {
                    numArray1[index2] -= num31;
                    numArray2[index2] -= Utils.ParseInt((object) (num31 / numArray3[index2]));
                    break;
                  }
                  double val = num31 - numArray1[index2];
                  numArray1[index2] = 0.0;
                  num31 = Utils.ArithmeticRounding(val, 2);
                  numArray2[index2] = 0;
                }
              }
            }
          }
          for (int index = 0; index < numArray1.Length; ++index)
          {
            this.SetCurrentNum(string.Concat((object) (3098 + index * 4)), numArray1[index]);
            this.SetCurrentNum(string.Concat((object) (index + 3124)), (double) numArray2[index]);
          }
        }
        double num32 = num30 - num11;
        this.SetCurrentNum("3120", num32);
        this.SetCurrentNum("3130", -num32);
        this.calculateRateLock((string) null, (string) null);
      }
      else
      {
        this.SetCurrentNum("3120", 0.0);
        this.SetCurrentNum("3130", 0.0);
        this.calculateRateLock((string) null, (string) null);
      }
      this.populateLastPayment(lastPayTransLog);
      double val1 = 0.0;
      double val2 = 0.0;
      double val3 = 0.0;
      double num33 = 0.0;
      double val4 = 0.0;
      double num34 = 0.0;
      double num35 = 0.0;
      double num36 = 0.0;
      double num37 = 0.0;
      double num38 = 0.0;
      double num39 = 0.0;
      double num40 = 0.0;
      double num41 = 0.0;
      double num42 = 0.0;
      double num43 = 0.0;
      double num44 = 0.0;
      double num45 = 0.0;
      double num46 = 0.0;
      double num47 = 0.0;
      double num48 = 0.0;
      double num49 = 0.0;
      double num50 = 0.0;
      double num51 = 0.0;
      double num52 = 0.0;
      double num53 = 0.0;
      double num54 = 0.0;
      double num55 = 0.0;
      DateTime dateToStop = DateTime.Today;
      if (this.Val("2626") == "Correspondent" && Utils.ParseDate((object) this.Val("3570")) != DateTime.MinValue)
        dateToStop = Utils.ParseDate((object) this.Val("3570"));
      double num56 = 0.0;
      int index3 = 0;
      DateTime dateTime2 = DateTime.MaxValue;
      if (current)
      {
        if (lastPayTransLog != null)
        {
          if (lastPayTransLog.PaymentDueDate == Utils.ToDate(this.loan.GetField("SERVICE.X14")) && lastPayTransLog.PaymentReceivedDate > Utils.ToDate(this.loan.GetField("SERVICE.X15")))
            dateTime2 = DateTime.MaxValue;
          else if (lastPayTransLog.StatementDate < Utils.ToDate(this.loan.GetField("SERVICE.X13")))
            this.updateRecentSchedulePaymentLog((SchedulePaymentLog) schedulePays1[(object) Utils.ToDate(this.loan.GetField("SERVICE.X14"))]);
        }
        else
        {
          dateTime2 = Utils.ParseDate((object) this.loan.GetField("SERVICE.X14"));
          if (servicingTransactions != null)
            this.updatePerviousSchedule(servicingTransactions);
        }
      }
      for (int index4 = 0; index4 < this.paySnapshot.MonthlyPayments.Length && this.paySnapshot.MonthlyPayments[index4] != null; ++index4)
      {
        SchedulePaymentLog schedulePaymentLog = (SchedulePaymentLog) null;
        DateTime date1 = Utils.ParseDate((object) this.paySnapshot.MonthlyPayments[index4].PayDate);
        if (date1 < dateTime2)
        {
          if (schedulePays1.ContainsKey((object) date1))
          {
            schedulePaymentLog = (SchedulePaymentLog) schedulePays1[(object) date1];
            if (schedulePaymentLog.LatePaymentDate < DateTime.Now && schedulePaymentLog.TotalPastDue > 0.0)
              ++num2;
            if (this.isScheduledPayLate(schedulePaymentLog, date1, dateToStop))
            {
              num56 += this.accumulateLateCharge(schedulePaymentLog);
              val1 += schedulePaymentLog.PrincipalDue - schedulePaymentLog.Principal;
              val2 += schedulePaymentLog.InterestDue - schedulePaymentLog.Interest;
              val3 += schedulePaymentLog.EscrowDue - schedulePaymentLog.Escrow;
              num33 += schedulePaymentLog.MiscFeeDue - schedulePaymentLog.MiscFee;
              val4 += schedulePaymentLog.BuydownSubsidyAmountDue - schedulePaymentLog.BuydownSubsidyAmount;
              num38 += schedulePaymentLog.EscrowCityPropertyTaxDue - schedulePaymentLog.CityPropertytax;
              num34 += schedulePaymentLog.EscrowTaxDue - schedulePaymentLog.Taxes;
              num37 += schedulePaymentLog.EscrowFloodInsuranceDue - schedulePaymentLog.FloodInsurance;
              num35 += schedulePaymentLog.EscrowHazardInsuranceDue - schedulePaymentLog.HazardInsurance;
              num36 += schedulePaymentLog.EscrowMortgageInsuranceDue - schedulePaymentLog.MortgageInsurance;
              num39 += schedulePaymentLog.EscrowOther1Due - schedulePaymentLog.Other1Escrow;
              num40 += schedulePaymentLog.EscrowOther2Due - schedulePaymentLog.Other2Escrow;
              num41 += schedulePaymentLog.EscrowOther3Due - schedulePaymentLog.Other3Escrow;
              num42 += schedulePaymentLog.EscrowUSDAMonthlyPremiumDue - schedulePaymentLog.USDAMonthlyPremium;
              num43 += schedulePaymentLog.PrincipalDue - schedulePaymentLog.Principal;
              num44 += schedulePaymentLog.InterestDue - schedulePaymentLog.Interest;
              num55 += schedulePaymentLog.BuydownSubsidyAmountDue - schedulePaymentLog.BuydownSubsidyAmount;
              num45 += schedulePaymentLog.EscrowDue - schedulePaymentLog.Escrow;
              num46 += schedulePaymentLog.EscrowTaxDue - schedulePaymentLog.Escrow;
              num49 += schedulePaymentLog.EscrowFloodInsuranceDue - schedulePaymentLog.FloodInsurance;
              num48 += schedulePaymentLog.EscrowHazardInsuranceDue - schedulePaymentLog.HazardInsurance;
              num47 += schedulePaymentLog.EscrowMortgageInsuranceDue - schedulePaymentLog.MortgageInsurance;
              num50 += schedulePaymentLog.EscrowCityPropertyTaxDue - schedulePaymentLog.CityPropertytax;
              num51 += schedulePaymentLog.EscrowOther1Due - schedulePaymentLog.Other1Escrow;
              num52 += schedulePaymentLog.EscrowOther2Due - schedulePaymentLog.Other2Escrow;
              num53 += schedulePaymentLog.EscrowOther3Due - schedulePaymentLog.Other3Escrow;
              num54 += schedulePaymentLog.EscrowUSDAMonthlyPremiumDue - schedulePaymentLog.USDAMonthlyPremium;
              schedulePaymentLog.TotalPastDue = Utils.ArithmeticRounding(val1 + val2 + val3 + val4 + num33, 2);
              schedulePaymentLog.UnpaidLateFeeDue = Utils.ArithmeticRounding(num56, 2);
              this.loan.AddServicingTransaction((ServicingTransactionBase) schedulePaymentLog, false);
            }
            else if (schedulePaymentLog.PaymentReceivedDate != DateTime.MinValue && schedulePaymentLog.PaymentReceivedDate <= schedulePaymentLog.LatePaymentDate)
            {
              if (schedulePaymentLog.Principal >= 0.0)
              {
                val1 -= schedulePaymentLog.Principal;
                num43 += schedulePaymentLog.PrincipalDue - schedulePaymentLog.Principal;
              }
              if (schedulePaymentLog.Interest >= 0.0)
              {
                num44 += schedulePaymentLog.InterestDue - schedulePaymentLog.Interest;
                val2 -= schedulePaymentLog.Interest;
              }
              if (schedulePaymentLog.Escrow >= 0.0)
              {
                val3 -= schedulePaymentLog.Escrow;
                num45 += schedulePaymentLog.EscrowDue - schedulePaymentLog.Escrow;
              }
              if (schedulePaymentLog.BuydownSubsidyAmount >= 0.0)
              {
                val4 -= schedulePaymentLog.BuydownSubsidyAmount;
                num55 += schedulePaymentLog.BuydownSubsidyAmountDue - schedulePaymentLog.BuydownSubsidyAmount;
              }
              if (schedulePaymentLog.Taxes >= 0.0)
              {
                num34 -= schedulePaymentLog.Taxes;
                num46 += schedulePaymentLog.EscrowTaxDue - schedulePaymentLog.Taxes;
              }
              if (schedulePaymentLog.MortgageInsurance >= 0.0)
              {
                num36 -= schedulePaymentLog.MortgageInsurance;
                num47 += schedulePaymentLog.EscrowMortgageInsuranceDue - schedulePaymentLog.MortgageInsurance;
              }
              if (schedulePaymentLog.HazardInsurance >= 0.0)
              {
                num35 -= schedulePaymentLog.HazardInsurance;
                num48 += schedulePaymentLog.EscrowHazardInsuranceDue - schedulePaymentLog.HazardInsurance;
              }
              if (schedulePaymentLog.FloodInsurance >= 0.0)
              {
                num37 -= schedulePaymentLog.FloodInsurance;
                num49 += schedulePaymentLog.EscrowFloodInsuranceDue - schedulePaymentLog.FloodInsurance;
              }
              if (schedulePaymentLog.CityPropertytax >= 0.0)
              {
                num38 -= schedulePaymentLog.CityPropertytax;
                num50 += schedulePaymentLog.EscrowCityPropertyTaxDue - schedulePaymentLog.CityPropertytax;
              }
              if (schedulePaymentLog.Other1Escrow >= 0.0)
              {
                num39 -= schedulePaymentLog.Other1Escrow;
                num51 += schedulePaymentLog.EscrowOther1Due - schedulePaymentLog.Other1Escrow;
              }
              if (schedulePaymentLog.Other2Escrow >= 0.0)
              {
                num40 -= schedulePaymentLog.Other2Escrow;
                num52 += schedulePaymentLog.EscrowOther2Due - schedulePaymentLog.Other2Escrow;
              }
              if (schedulePaymentLog.Other3Escrow >= 0.0)
              {
                num41 -= schedulePaymentLog.Other3Escrow;
                num53 += schedulePaymentLog.EscrowOther3Due - schedulePaymentLog.Other3Escrow;
              }
              if (schedulePaymentLog.USDAMonthlyPremium >= 0.0)
              {
                num42 -= schedulePaymentLog.USDAMonthlyPremium;
                num54 += schedulePaymentLog.EscrowUSDAMonthlyPremiumDue - schedulePaymentLog.USDAMonthlyPremium;
              }
            }
            num56 -= schedulePaymentLog.LateFee;
          }
          DateTime date2 = dateToStop.Date;
          dateTime1 = date1.Date;
          DateTime dateTime3 = dateTime1.AddDays((double) this.paySnapshot.PaymentDueDays);
          if (!(date2 <= dateTime3) || schedulePaymentLog != null && Math.Round(schedulePaymentLog.PrincipalDue, 2) <= Math.Round(schedulePaymentLog.Principal, 2) && Math.Round(schedulePaymentLog.InterestDue, 2) <= Math.Round(schedulePaymentLog.Interest, 2) && Math.Round(schedulePaymentLog.EscrowDue, 2) <= Math.Round(schedulePaymentLog.Escrow, 2) && Math.Round(schedulePaymentLog.BuydownSubsidyAmountDue, 2) <= Math.Round(schedulePaymentLog.BuydownSubsidyAmount, 2) && Math.Round(schedulePaymentLog.MiscFeeDue, 2) <= Math.Round(schedulePaymentLog.MiscFee, 2) && Math.Round(Utils.ArithmeticRounding(num43, 2), 2) <= 0.0 && Math.Round(Utils.ArithmeticRounding(num44, 2), 2) <= 0.0 && Math.Round(Utils.ArithmeticRounding(num45, 2), 2) <= 0.0 && Math.Round(Utils.ArithmeticRounding(num33, 2), 2) <= 0.0)
          {
            if (schedulePaymentLog != null)
              unpaidBalance -= schedulePaymentLog.PrincipalDue;
            ++index3;
          }
          else
            break;
        }
      }
      if (Utils.ArithmeticRounding(val1, 2) < 0.0)
        val1 = 0.0;
      if (Utils.ArithmeticRounding(val2, 2) < 0.0)
        val2 = 0.0;
      if (Utils.ArithmeticRounding(val3, 2) < 0.0)
        val3 = 0.0;
      if (Utils.ArithmeticRounding(val4, 2) < 0.0)
        val4 = 0.0;
      if (Utils.ArithmeticRounding(num33, 2) < 0.0)
        num33 = 0.0;
      if (Utils.ArithmeticRounding(num56, 2) < 0.0)
        num56 = 0.0;
      if (num45 <= 0.0)
      {
        double num57;
        num49 = num57 = 0.0;
        num54 = num57;
        num53 = num57;
        num52 = num57;
        num47 = num57;
        num51 = num57;
        num50 = num57;
        num48 = num57;
        num46 = num57;
        num45 = num57;
      }
      if (this.loan.IsLocked("SERVICE.X21"))
      {
        this.SetCurrentNum("SERVICE.X91", this.FltVal("SERVICE.X21"));
        this.SetCurrentNum("SERVICE.X92", 0.0);
        this.SetCurrentNum("SERVICE.X93", 0.0);
        this.SetCurrentNum("SERVICE.X94", 0.0);
        this.SetCurrentNum("SERVICE.X104", 0.0);
        this.SetCurrentNum("SERVICE.X130", 0.0);
        this.SetCurrentNum("SERVICE.X131", 0.0);
        this.SetCurrentNum("SERVICE.X132", 0.0);
        this.SetCurrentNum("SERVICE.X133", 0.0);
        this.SetCurrentNum("SERVICE.X134", 0.0);
        this.SetCurrentNum("SERVICE.X135", 0.0);
        this.SetCurrentNum("SERVICE.X136", 0.0);
        this.SetCurrentNum("SERVICE.X137", 0.0);
        this.SetCurrentNum("SERVICE.X138", 0.0);
      }
      else
      {
        this.SetCurrentNum("SERVICE.X91", num43);
        this.SetCurrentNum("SERVICE.X92", num44);
        this.SetCurrentNum("SERVICE.X93", num45);
        this.SetCurrentNum("SERVICE.X94", num33);
        this.SetCurrentNum("SERVICE.X104", num55);
        this.SetCurrentNum("SERVICE.X130", num46);
        this.SetCurrentNum("SERVICE.X131", num47);
        this.SetCurrentNum("SERVICE.X132", num48);
        this.SetCurrentNum("SERVICE.X133", num49);
        this.SetCurrentNum("SERVICE.X134", num50);
        this.SetCurrentNum("SERVICE.X135", num51);
        this.SetCurrentNum("SERVICE.X136", num52);
        this.SetCurrentNum("SERVICE.X137", num53);
        this.SetCurrentNum("SERVICE.X138", num54);
      }
      this.SetCurrentNum("SERVICE.X40", (double) num2);
      this.SetCurrentNum("SERVICE.X95", num56);
      this.updateInterimAfterPurchase(servicingTransactions, current);
      double val5 = Utils.ArithmeticRounding(val1 + val2 + val3 + val4 + num33, 2);
      bool flag = false;
      DateTime key = DateTime.MinValue;
      if (index3 < this.paySnapshot.MonthlyPayments.Length && this.paySnapshot.MonthlyPayments[index3] != null)
        key = Utils.ParseDate((object) this.paySnapshot.MonthlyPayments[index3].PayDate);
      if (key != Utils.ParseDate((object) this.Val("SERVICE.X99")))
        flag = true;
      if (current && dateTime2 != DateTime.MaxValue)
        flag = false;
      for (int index5 = 13; index5 <= 26; ++index5)
      {
        if (index5 < 21 || index5 > 23)
          this.SetVal("SERVICE.X" + index5.ToString(), "");
        if (flag)
          this.loan.RemoveCurrentLock("SERVICE.X" + index5.ToString());
      }
      if (flag)
        this.SetVal("SERVICE.X23", "");
      this.SetVal("SERVICE.X25", "");
      this.SetVal("SERVICE.X82", "");
      if (key == DateTime.MinValue)
        return;
      if (this.paySnapshot.EscrowPayments != null)
      {
        this.updateEscrowDisbursement(1, num27);
        this.updateEscrowDisbursement(4, num24);
        this.updateEscrowDisbursement(2, num25);
        this.updateEscrowDisbursement(3, num26);
        this.updateEscrowDisbursement(5, num23);
        this.updateEscrowDisbursement(6, payOff1);
        this.updateEscrowDisbursement(7, payOff2);
        this.updateEscrowDisbursement(8, payOff3);
        this.updateEscrowDisbursement(9, num28);
        if (index3 < this.paySnapshot.EscrowPayments.Length && this.paySnapshot.EscrowPayments[index3] != null && num29 > 0.0)
          this.SetCurrentNum("SERVICE.X20", this.paySnapshot.GetNumField("HUD24"));
      }
      if (num29 > 0.0 || val5 > 0.0)
      {
        this.SetVal("SERVICE.X14", key.ToString("MM/dd/yyyy"));
        this.calObjs.ULDDExpCal.calculateFannieMaeExportFields("SERVICE.X14", this.Val("SERVICE.X14"));
      }
      DateTime dateTime4 = key;
      key = key.AddDays((double) this.paySnapshot.PaymentDueDays);
      if (num29 > 0.0 || val5 > 0.0)
      {
        dateTime1 = key.AddDays(1.0);
        this.SetVal("SERVICE.X15", dateTime1.ToString("MM/dd/yyyy"));
      }
      DateTime dateTime5 = DateTime.Today;
      if (this.Val("2626") == "Correspondent" && Utils.ParseDate((object) this.Val("3570")) != DateTime.MinValue && dateTime4 <= dateTime5.AddMonths(-1))
        dateTime5 = Utils.ParseDate((object) this.Val("3570"));
      if (val5 > 0.0 || key < dateTime5)
        this.SetVal("SERVICE.X8", "Past Due");
      else
        this.SetVal("SERVICE.X8", "Current");
      key = key.AddDays((double) ((this.paySnapshot.PaymentDueDays + this.paySnapshot.StatementPrintDueDay) * -1));
      if (num29 > 0.0 || val5 > 0.0)
      {
        this.SetVal("SERVICE.X13", key.ToString("MM/dd/yyyy"));
        this.calObjs.ULDDExpCal.calculateFannieMaeExportFields("SERVICE.X13", this.Val("SERVICE.X13"));
      }
      key = Utils.ParseDate((object) this.paySnapshot.MonthlyPayments[index3].PayDate);
      SchedulePaymentLog lastSchedulePayLog = (SchedulePaymentLog) null;
      SchedulePaymentLog transactionLog = (SchedulePaymentLog) null;
      if (schedulePays1.ContainsKey((object) key.AddMonths(-1)))
        lastSchedulePayLog = (SchedulePaymentLog) schedulePays1[(object) key.AddMonths(-1)];
      if (schedulePays1.ContainsKey((object) key) && lastPayTransLog != null)
      {
        transactionLog = (SchedulePaymentLog) schedulePays1[(object) this.CalculateNextPaymentDueDate(schedulePays1, lastPayTransLog.PaymentIndexDate)];
        if (transactionLog != null)
        {
          transactionLog.TotalPastDue = val5 > 0.0 ? this.EMRounding(val5, 2) : 0.0;
          transactionLog.UnpaidLateFeeDue = num56 > 0.0 ? this.EMRounding(num56, 2) : 0.0;
          this.loan.AddServicingTransaction((ServicingTransactionBase) transactionLog, false);
        }
      }
      if (schedulePays1.ContainsKey((object) key))
        transactionLog = (SchedulePaymentLog) schedulePays1[(object) key];
      if (transactionLog != null && !flag)
      {
        this.SetCurrentNum("SERVICE.X23", transactionLog.MiscFeeDue > 0.0 ? transactionLog.MiscFeeDue : 0.0);
        this.SetCurrentNum("SERVICE.X94", transactionLog.MiscFeeDue - transactionLog.MiscFee > 0.0 ? transactionLog.MiscFeeDue - transactionLog.MiscFee : 0.0);
      }
      this.SetCurrentNum("SERVICE.X21", val5 > 0.0 ? this.EMRounding(val5, 2) : 0.0);
      this.SetCurrentNum("SERVICE.X22", num56 > 0.0 ? this.EMRounding(num56, 2) : 0.0);
      this.SetVal("SERVICE.X99", key.ToString("MM/dd/yyyy"));
      double nextInterestRate = this.findNextInterestRate(lastSchedulePayLog, index3, schedulePays1);
      double num58 = this.paySnapshot.MonthlyPayments[index3].Payment - this.paySnapshot.MonthlyPayments[index3].MortgageInsurance;
      double interest = 0.0;
      if (lastSchedulePayLog != null)
      {
        if (nextInterestRate == lastSchedulePayLog.InterestRate)
          num58 = lastSchedulePayLog.PrincipalDue + lastSchedulePayLog.InterestDue;
        else if (!this.paySnapshot.IsBuydownLoan)
          num58 = RegzCalculation.CalcRawMonthlyPayment(this.paySnapshot.LoanTerm - index3, unpaidBalance, nextInterestRate, this.paySnapshot.IsBiweekly, this.paySnapshot.RateFactorBiweekly(nextInterestRate), this.paySnapshot.NumberPayPerYear, this.paySnapshot.LoanTerm, index3, this.paySnapshot.LoanAmount, this.paySnapshot.ZeroPercentPaymentOption, this.checkIfSimpleInterest(), this.findFirstPaymentDate(), this.findConstInterestType(), this.findSimpleInterestUse366ForLeapYear());
        num58 = Utils.ArithmeticRounding(num58, 2);
      }
      if (this.paySnapshot.GetField("19") != "ConstructionOnly")
        interest = !this.paySnapshot.IsBuydownLoan ? RegzCalculation.CalcInterestPayment(num29, nextInterestRate, this.paySnapshot.IsBiweekly, this.paySnapshot.DaysPerYear) : this.paySnapshot.MonthlyPayments[index3].Interest;
      if (index3 + 1 >= this.paySnapshot.MonthlyPayments.Length)
        num58 = num29 + interest;
      if (num29 == 0.0 && val5 == 0.0)
        this.SetCurrentNum("SERVICE.X17", 0.0);
      if (this.Val("2626") == "Correspondent")
        this.SetCurrentNum("SERVICE.X18", this.paySnapshot.MonthlyPayments[index3].Principal, this.UseNoPayment(this.paySnapshot.MonthlyPayments[index3].Principal));
      else if (interest > num58)
      {
        this.SetCurrentNum("SERVICE.X18", 0.0, this.UseNoPayment(0.0));
      }
      else
      {
        double num59 = num29 > 0.0 ? num58 - interest : 0.0;
        this.SetCurrentNum("SERVICE.X18", num59, this.UseNoPayment(num59));
      }
      if (this.Val("2626") == "Correspondent")
        this.SetCurrentNum("SERVICE.X19", this.paySnapshot.MonthlyPayments[index3].Interest);
      else
        this.SetCurrentNum("SERVICE.X19", num29 > 0.0 ? interest : 0.0);
      this.SetCurrentNum("SERVICE.X100", this.paySnapshot.MonthlyPayments[index3].BuydownSubsidyAmount);
      this.SetCurrentNum("SERVICE.X24", this.FltVal("SERVICE.X18") + this.FltVal("SERVICE.X19") + this.FltVal("SERVICE.X20") + this.FltVal("SERVICE.X21") + this.FltVal("SERVICE.X22") + this.FltVal("SERVICE.X23") + this.FltVal("SERVICE.X100"));
      this.SetCurrentNum("SERVICE.X25", this.CalcLateCharge(num58 + this.FltVal("SERVICE.X20"), num58 + this.FltVal("SERVICE.X20"), interest, num58, this.paySnapshot.GetNumField("674"), this.paySnapshot.GetField("1719"), this.paySnapshot.MinLateCharge, this.paySnapshot.MaxLateCharge));
      this.SetCurrentNum("SERVICE.X26", this.FltVal("SERVICE.X24") + this.FltVal("SERVICE.X25"));
      this.SetCurrentNum("SERVICE.X82", this.FltVal("SERVICE.X18") + this.FltVal("SERVICE.X19"));
      if (this.loan.GetField("HUD24") != "")
        this.setPaymentEscrow(escrow);
      this.resetMicFee(servicingTransactions);
    }

    private DateTime CalculateNextPaymentDueDate(Hashtable schedulePays, DateTime indexDate)
    {
      return schedulePays.Keys.Cast<DateTime>().OrderBy<DateTime, DateTime>((Func<DateTime, DateTime>) (c => c)).ToList<DateTime>().Where<DateTime>((Func<DateTime, bool>) (x => x.Date > indexDate)).FirstOrDefault<DateTime>();
    }

    private bool isScheduledPayLate(
      SchedulePaymentLog schedulePayLog,
      DateTime payDate,
      DateTime dateToStop)
    {
      bool flag1 = false;
      DateTime dateTime = dateToStop > DateTime.Today.AddMonths(-1) ? dateToStop : DateTime.Today.AddMonths(-1);
      bool flag2 = schedulePayLog.PaymentReceivedDate == DateTime.MinValue && dateToStop > schedulePayLog.LatePaymentDate || schedulePayLog.PaymentReceivedDate != DateTime.MinValue && schedulePayLog.PaymentReceivedDate > schedulePayLog.LatePaymentDate || schedulePayLog.PaymentReceivedDate != DateTime.MinValue && dateToStop > schedulePayLog.LatePaymentDate && schedulePayLog.PaymentDifference > 0.0;
      if (this.Val("2626") != "Correspondent" & flag2)
        flag1 = true;
      else if (this.Val("2626") == "Correspondent" & flag2 && payDate > dateTime)
        flag1 = true;
      return flag1;
    }

    public DateTime GetISWCutoffDate()
    {
      DateTime iswCutoffDate = DateTime.Today;
      DateTime date = Utils.ParseDate((object) this.Val("3570"));
      DateTime dateTime = !(date != DateTime.MinValue) || !(date > DateTime.Today.AddMonths(-1)) ? DateTime.Today.AddMonths(-1) : date;
      if (this.Val("2626") == "Correspondent" && date != DateTime.MinValue && Utils.ParseDate((object) this.Val("SERVICE.X14")) <= dateTime)
        iswCutoffDate = date;
      return iswCutoffDate;
    }

    private void resetMicFee(ServicingTransactionBase[] transactions)
    {
      if (transactions == null)
        return;
      int[] num1 = new int[3];
      this.getLatestTypeLog(transactions, num1);
      if (num1[1] <= 2 || !(transactions[num1[0]] is PaymentTransactionLog))
        return;
      PaymentTransactionLog transaction1 = (PaymentTransactionLog) transactions[num1[0]];
      PaymentReversalLog transaction2 = (PaymentReversalLog) transactions[num1[1]];
      SchedulePaymentLog transaction3 = (SchedulePaymentLog) transactions[num1[2]];
      int num2 = this.getpreviouspayment(transactions, num1[0]);
      if (num2 != -1)
      {
        PaymentTransactionLog transaction4 = (PaymentTransactionLog) transactions[num2];
        DateTime dateTime1;
        if (transaction2.PaymentGUID == transaction1.TransactionGUID)
        {
          DateTime paymentIndexDate = transaction1.PaymentIndexDate;
          dateTime1 = transaction3.TransactionDate;
          DateTime date = dateTime1.Date;
          if (paymentIndexDate == date)
          {
            if (!(transaction4.PaymentIndexDate == transaction1.PaymentIndexDate))
              return;
            this.SetCurrentNum("Service.X23", transaction4.SchedulePayLogMiscFee);
            this.getpreviouspayment(transactions, num2);
            return;
          }
        }
        if (!(transaction2.PaymentGUID == transaction1.TransactionGUID))
          return;
        DateTime paymentIndexDate1 = transaction1.PaymentIndexDate;
        dateTime1 = transaction3.TransactionDate;
        dateTime1 = dateTime1.Date;
        DateTime dateTime2 = dateTime1.AddMonths(-1);
        if (!(paymentIndexDate1 == dateTime2) || !(transaction4.PaymentIndexDate != transaction1.PaymentIndexDate))
          return;
        this.SetCurrentNum("Service.X23", transaction1.SchedulePayLogMiscFee);
      }
      else
        this.SetCurrentNum("Service.X23", transaction1.SchedulePayLogMiscFee);
    }

    public string getSchedulePaymentLogforPurchase(
      Hashtable schedulePays,
      string purchaseAdviceDate)
    {
      DateTime dateTime1 = DateTime.MinValue;
      foreach (DateTime key in (IEnumerable) schedulePays.Keys)
      {
        SchedulePaymentLog schedulePay = (SchedulePaymentLog) schedulePays[(object) key];
        DateTime dateTime2 = schedulePay.TransactionDate;
        if (dateTime2.Date < Utils.ToDate(purchaseAdviceDate))
        {
          DateTime date1 = Utils.ToDate(purchaseAdviceDate);
          dateTime2 = schedulePay.LatePaymentDate;
          DateTime date2 = dateTime2.Date;
          if (date1 < date2)
            dateTime1 = key;
        }
      }
      return dateTime1.ToString("MM/dd/yyyy");
    }

    internal void setPaymentEscrow(double[] escrow)
    {
      this.SetCurrentNum("SERVICE.X112", escrow[0]);
      this.SetCurrentNum("SERVICE.X113", escrow[1]);
      this.SetCurrentNum("SERVICE.X114", escrow[2]);
      this.SetCurrentNum("SERVICE.X115", escrow[3]);
      this.SetCurrentNum("SERVICE.X116", escrow[4]);
      this.SetCurrentNum("SERVICE.X117", escrow[5]);
      this.SetCurrentNum("SERVICE.X119", escrow[6]);
      this.SetCurrentNum("SERVICE.X120", escrow[7]);
      this.SetCurrentNum("SERVICE.X118", escrow[8]);
    }

    internal int[] getLatestTypeLog(ServicingTransactionBase[] trans, int[] num)
    {
      for (int index = 0; index < trans.Length; ++index)
      {
        if (trans[index] is PaymentTransactionLog)
          num[0] = index;
        else if (trans[index] is PaymentReversalLog)
          num[1] = index;
        else if (trans[index] is SchedulePaymentLog)
          num[2] = index;
      }
      return num;
    }

    internal int getpreviouspayment(ServicingTransactionBase[] trans, int num)
    {
      int num1 = -1;
      for (int index = num - 1; index > 0; --index)
      {
        if (trans[index] is PaymentTransactionLog)
        {
          num1 = index;
          break;
        }
      }
      return num1;
    }

    internal ServicingSummaryViewModel GetInterimServicingAnnualSummary(int year)
    {
      return ServicingSummaryViewModel.GenerateAnnualSummary(this.loan.GetServicingTransactions(), year);
    }

    private double payoffAmountFromCurrentMonth(
      DateTime paymentIndexDate,
      ServicingTransactionBase[] transactions,
      Hashtable reversalGUIDs)
    {
      if (transactions == null)
        return 0.0;
      double num = 0.0;
      for (int index = 0; index < transactions.Length; ++index)
      {
        if (!reversalGUIDs.ContainsKey((object) transactions[index].TransactionGUID) && transactions[index] is PaymentTransactionLog)
        {
          PaymentTransactionLog transaction = (PaymentTransactionLog) transactions[index];
          if (paymentIndexDate == transaction.PaymentIndexDate)
            num += transaction.Principal + transaction.AdditionalPrincipal;
        }
      }
      return num;
    }

    private double findNextInterestRate(
      SchedulePaymentLog lastSchedulePayLog,
      int nextPaymentIndex,
      Hashtable schedulePays)
    {
      bool flag = false;
      for (int index = 0; index < nextPaymentIndex; ++index)
      {
        DateTime date = Utils.ParseDate((object) this.paySnapshot.MonthlyPayments[index].PayDate);
        if (schedulePays.ContainsKey((object) date) && ((SchedulePaymentLog) schedulePays[(object) date]).InterestRate != this.paySnapshot.MonthlyPayments[index].CurrentRate)
        {
          flag = true;
          break;
        }
      }
      if (this.paySnapshot.IsARMLoan)
      {
        if (!this.IsLocked("SERVICE.X16"))
        {
          if (this.paySnapshot.IndexRate == 0.0)
          {
            if (lastSchedulePayLog == null || !flag)
              this.SetCurrentNum("SERVICE.X16", this.paySnapshot.MonthlyPayments[nextPaymentIndex].CurrentRate - this.paySnapshot.MarginRate);
            else
              this.SetCurrentNum("SERVICE.X16", lastSchedulePayLog.InterestRate - this.paySnapshot.MarginRate);
          }
          else
            this.SetCurrentNum("SERVICE.X16", this.paySnapshot.IndexRate);
        }
        if (!this.IsLocked("SERVICE.X17"))
        {
          if (lastSchedulePayLog == null || !flag)
            this.SetCurrentNum("SERVICE.X17", this.paySnapshot.MonthlyPayments[nextPaymentIndex].CurrentRate - this.paySnapshot.MarginRate + this.paySnapshot.MarginRate);
          else
            this.SetCurrentNum("SERVICE.X17", lastSchedulePayLog.InterestRate - this.paySnapshot.MarginRate + this.paySnapshot.MarginRate);
        }
      }
      else
      {
        if (!this.IsLocked("SERVICE.X17"))
        {
          if (lastSchedulePayLog == null || !flag)
            this.SetCurrentNum("SERVICE.X17", this.paySnapshot.MonthlyPayments[nextPaymentIndex].CurrentRate);
          else
            this.SetCurrentNum("SERVICE.X17", lastSchedulePayLog.InterestRate);
        }
        this.SetCurrentNum("SERVICE.X16", 0.0);
      }
      return this.FltVal("SERVICE.X17");
    }

    private void calculateServicingTotalDue(string id, string val)
    {
      this.CalcInterimServicing(true);
    }

    private void populateLastPayment(PaymentTransactionLog lastPayTransLog)
    {
      if (lastPayTransLog != null)
      {
        this.SetCurrentNum("SERVICE.X30", (double) lastPayTransLog.PaymentNo);
        if (lastPayTransLog.StatementDate != DateTime.MinValue)
          this.SetVal("SERVICE.X31", lastPayTransLog.StatementDate.ToString("MM/dd/yyyy"));
        if (lastPayTransLog.PaymentReceivedDate != DateTime.MinValue)
        {
          this.SetVal("SERVICE.X32", lastPayTransLog.PaymentReceivedDate.ToString("MM/dd/yyyy"));
          this.calObjs.ULDDExpCal.calculateFannieMaeExportFields("SERVICE.X32", this.Val("SERVICE.X32"));
        }
        this.SetCurrentNum("SERVICE.X33", lastPayTransLog.TotalAmountReceived);
        this.SetCurrentNum("SERVICE.X34", lastPayTransLog.Principal);
        this.SetCurrentNum("SERVICE.X35", lastPayTransLog.Interest);
        this.SetCurrentNum("SERVICE.X83", lastPayTransLog.Principal + lastPayTransLog.Interest);
        this.SetCurrentNum("SERVICE.X36", lastPayTransLog.Escrow);
        this.SetCurrentNum("SERVICE.X101", lastPayTransLog.BuydownSubsidyAmount);
        this.SetCurrentNum("SERVICE.X37", lastPayTransLog.LateFee);
        this.SetCurrentNum("SERVICE.X85", lastPayTransLog.MiscFee);
        this.SetCurrentNum("SERVICE.X38", lastPayTransLog.AdditionalPrincipal);
        this.SetCurrentNum("SERVICE.X86", lastPayTransLog.AdditionalEscrow);
        this.SetCurrentNum("SERVICE.X121", lastPayTransLog.EscowTaxes);
        this.SetCurrentNum("SERVICE.X122", lastPayTransLog.HazardInsurance);
        this.SetCurrentNum("SERVICE.X123", lastPayTransLog.MortgageInsurance);
        this.SetCurrentNum("SERVICE.X124", lastPayTransLog.FloodInsurance);
        this.SetCurrentNum("SERVICE.X125", lastPayTransLog.CityPropertyTax);
        this.SetCurrentNum("SERVICE.X126", lastPayTransLog.Other1Escrow);
        this.SetCurrentNum("SERVICE.X127", lastPayTransLog.Other2Escrow);
        this.SetCurrentNum("SERVICE.X128", lastPayTransLog.Other3Escrow);
        this.SetCurrentNum("SERVICE.X129", lastPayTransLog.USDAMonthlyPremium);
        this.SetVal("SERVICE.LASTGUID", lastPayTransLog.TransactionGUID);
      }
      else
      {
        this.SetVal("SERVICE.LASTGUID", "");
        for (int index = 30; index <= 38; ++index)
          this.SetVal("SERVICE.X" + index.ToString(), "");
        for (int index = 83; index <= 86; ++index)
        {
          if (index != 84)
            this.SetVal("SERVICE.X" + index.ToString(), "");
        }
        this.calObjs.ULDDExpCal.calculateFannieMaeExportFields("SERVICE.X32", this.Val("SERVICE.X32"));
        this.SetVal("SERVICE.X101", "");
      }
      for (int index = 58; index <= 73; ++index)
        this.SetVal("SERVICE.X" + index.ToString(), "");
      this.SetVal("SERVICE.X105", "");
      this.SetVal("SERVICE.X106", "");
      this.calObjs.RegzCal.CalcIRS1098((string) null, (string) null);
    }

    private void loadPaymentSnapshot()
    {
      this.paySnapshot = this.loan.GetPaymentScheduleSnapshot();
      if (this.paySnapshot == null)
        return;
      this.paySnapshot.StatementPrintDueDay = Utils.ParseInt((object) this.sessionObjects.ConfigurationManager.GetCompanySetting("SERVICING", "DaysToPrint"));
      if (this.paySnapshot.StatementPrintDueDay <= 0)
        this.paySnapshot.StatementPrintDueDay = 14;
      this.paySnapshot.MinLateCharge = this.paySnapshot.GetNumField("2831");
      this.paySnapshot.MaxLateCharge = this.paySnapshot.GetNumField("2832");
    }

    private Hashtable loadRealPaymentSchedule(
      ref int nextPaymentIndex,
      ServicingTransactionBase[] transactions)
    {
      Hashtable hashtable = new Hashtable();
      if (transactions != null && transactions.Length != 0)
      {
        for (int index = 0; index < transactions.Length; ++index)
        {
          if (transactions[index] is SchedulePaymentLog && !hashtable.ContainsKey((object) transactions[index].TransactionDate))
          {
            ((SchedulePaymentLog) transactions[index]).ClearReceivedAmount();
            hashtable.Add((object) transactions[index].TransactionDate, (object) transactions[index]);
          }
        }
      }
      SchedulePaymentLog schedulePayLog1 = (SchedulePaymentLog) null;
      DateTime date1 = Utils.ParseDate((object) this.Val("SERVICE.X99"));
      DateTime date2 = Utils.ParseDate((object) this.Val("SERVICE.X14"));
      if (date1 != DateTime.MinValue && !hashtable.ContainsKey((object) date1))
        schedulePayLog1 = new SchedulePaymentLog();
      else if (hashtable.ContainsKey((object) date1))
        schedulePayLog1 = (SchedulePaymentLog) hashtable[(object) date1];
      if (schedulePayLog1 != null)
      {
        schedulePayLog1.TransactionDate = date1;
        this.setSchedulePayLog(schedulePayLog1);
        if (date1 != DateTime.MinValue && !hashtable.ContainsKey((object) date1))
          hashtable.Add((object) schedulePayLog1.TransactionDate, (object) schedulePayLog1);
      }
      if (date2 != DateTime.MinValue && !hashtable.ContainsKey((object) date2))
      {
        SchedulePaymentLog schedulePayLog2 = new SchedulePaymentLog();
        schedulePayLog2.TransactionDate = date2;
        this.setSchedulePayLog(schedulePayLog2);
        hashtable.Add((object) schedulePayLog2.TransactionDate, (object) schedulePayLog2);
      }
      DateTime dateTime = DateTime.Today;
      if (this.Val("2626") == "Correspondent" && Utils.ParseDate((object) this.Val("3570")) != DateTime.MinValue)
        dateTime = Utils.ParseDate((object) this.Val("3570"));
      for (int index = 0; index < this.paySnapshot.MonthlyPayments.Length; ++index)
      {
        DateTime date3 = Utils.ParseDate((object) this.paySnapshot.MonthlyPayments[index].PayDate);
        if (!(dateTime < date3))
        {
          ++nextPaymentIndex;
          if (!hashtable.ContainsKey((object) date3))
          {
            SchedulePaymentLog transactionLog;
            if (date1 == date3)
            {
              transactionLog = new SchedulePaymentLog();
              transactionLog.TransactionDate = date1;
              transactionLog.LatePaymentDate = Utils.ParseDate((object) this.Val("SERVICE.X15"));
              transactionLog.IndexRate = Utils.ParseDouble((object) this.FltVal("SERVICE.X16"));
              transactionLog.InterestRate = Utils.ParseDouble((object) this.FltVal("SERVICE.X17"));
              transactionLog.PrincipalDue = Utils.ParseDouble((object) this.FltVal("SERVICE.X18"));
              transactionLog.InterestDue = Utils.ParseDouble((object) this.FltVal("SERVICE.X19"));
              transactionLog.EscrowDue = Utils.ParseDouble((object) this.FltVal("SERVICE.X20"));
              transactionLog.BuydownSubsidyAmount = Utils.ParseDouble((object) this.FltVal("SERVICE.X100"));
              transactionLog.UnpaidLateFeeDue = Utils.ParseDouble((object) this.FltVal("SERVICE.X22"));
              transactionLog.MiscFeeDue = Utils.ParseDouble((object) this.FltVal("SERVICE.X23"));
              transactionLog.TotalPastDue = Utils.ParseDouble((object) this.FltVal("SERVICE.X21"));
              transactionLog.EscrowTaxDue = Utils.ParseDouble((object) this.FltVal("SERVICE.X112"));
              transactionLog.EscrowMortgageInsuranceDue = Utils.ParseDouble((object) this.FltVal("SERVICE.X114"));
              transactionLog.EscrowHazardInsuranceDue = Utils.ParseDouble((object) this.FltVal("SERVICE.X113"));
              transactionLog.EscrowFloodInsuranceDue = Utils.ParseDouble((object) this.FltVal("SERVICE.X115"));
              transactionLog.EscrowCityPropertyTaxDue = Utils.ParseDouble((object) this.FltVal("SERVICE.X116"));
              transactionLog.EscrowOther1Due = Utils.ParseDouble((object) this.FltVal("SERVICE.X117"));
              transactionLog.EscrowOther2Due = Utils.ParseDouble((object) this.FltVal("SERVICE.X119"));
              transactionLog.EscrowOther3Due = Utils.ParseDouble((object) this.FltVal("SERVICE.X120"));
              transactionLog.EscrowUSDAMonthlyPremiumDue = Utils.ParseDouble((object) this.FltVal("SERVICE.X118"));
            }
            else
            {
              transactionLog = new SchedulePaymentLog();
              transactionLog.TransactionDate = Utils.ParseDate((object) this.paySnapshot.MonthlyPayments[index].PayDate);
              transactionLog.LatePaymentDate = transactionLog.TransactionDate.AddDays((double) this.paySnapshot.PaymentDueDays);
              transactionLog.IndexRate = this.paySnapshot.MonthlyPayments[index].CurrentRate != this.paySnapshot.NoteRate ? this.paySnapshot.MonthlyPayments[index].CurrentRate - this.paySnapshot.MarginRate : 0.0;
              transactionLog.InterestRate = this.paySnapshot.MonthlyPayments[index].CurrentRate;
              transactionLog.PrincipalDue = this.paySnapshot.MonthlyPayments[index].Principal;
              transactionLog.InterestDue = this.paySnapshot.MonthlyPayments[index].Interest;
              transactionLog.EscrowDue = Utils.ParseDouble((object) this.paySnapshot.GetField("HUD24"));
              transactionLog.EscrowTaxDue = Utils.ParseDouble((object) this.FltVal("231"));
              transactionLog.EscrowHazardInsuranceDue = Utils.ParseDouble((object) this.FltVal("230"));
              transactionLog.EscrowMortgageInsuranceDue = Utils.ParseDouble((object) this.FltVal("232"));
              transactionLog.EscrowFloodInsuranceDue = Utils.ParseDouble((object) this.FltVal("235"));
              transactionLog.EscrowCityPropertyTaxDue = Utils.ParseDouble((object) this.FltVal("L268"));
              transactionLog.EscrowOther1Due = Utils.ParseDouble((object) this.FltVal("1630"));
              transactionLog.EscrowOther2Due = Utils.ParseDouble((object) this.FltVal("253"));
              transactionLog.EscrowOther3Due = Utils.ParseDouble((object) this.FltVal("254"));
              transactionLog.EscrowUSDAMonthlyPremiumDue = Utils.ParseDouble((object) this.FltVal("NEWHUD.X1707"));
              transactionLog.BuydownSubsidyAmountDue = this.paySnapshot.MonthlyPayments[index].BuydownSubsidyAmount;
            }
            hashtable.Add((object) transactionLog.TransactionDate, (object) transactionLog);
            this.loan.AddServicingTransaction((ServicingTransactionBase) transactionLog, false);
          }
        }
        else
          break;
      }
      return hashtable;
    }

    private void setSchedulePayLog(SchedulePaymentLog schedulePayLog)
    {
      schedulePayLog.LatePaymentDate = Utils.ParseDate((object) this.Val("SERVICE.X15"));
      schedulePayLog.IndexRate = Utils.ParseDouble((object) this.FltVal("SERVICE.X16"));
      schedulePayLog.InterestRate = Utils.ParseDouble((object) this.FltVal("SERVICE.X17"));
      schedulePayLog.PrincipalDue = Utils.ParseDouble((object) this.FltVal("SERVICE.X18"));
      schedulePayLog.InterestDue = Utils.ParseDouble((object) this.FltVal("SERVICE.X19"));
      schedulePayLog.EscrowDue = Utils.ParseDouble((object) this.FltVal("SERVICE.X20"));
      if (this.loan.GetField("HUD24") == "0")
        schedulePayLog.EscrowDue = 0.0;
      schedulePayLog.BuydownSubsidyAmount = Utils.ParseDouble((object) this.FltVal("SERVICE.X100"));
      schedulePayLog.UnpaidLateFeeDue = Utils.ParseDouble((object) this.FltVal("SERVICE.X22"));
      schedulePayLog.MiscFeeDue = Utils.ParseDouble((object) this.FltVal("SERVICE.X23"));
      schedulePayLog.TotalPastDue = Utils.ParseDouble((object) this.FltVal("SERVICE.X21"));
      schedulePayLog.EscrowTaxDue = Utils.ParseDouble((object) this.FltVal("SERVICE.X112"));
      schedulePayLog.EscrowMortgageInsuranceDue = Utils.ParseDouble((object) this.FltVal("SERVICE.X114"));
      schedulePayLog.EscrowHazardInsuranceDue = Utils.ParseDouble((object) this.FltVal("SERVICE.X113"));
      schedulePayLog.EscrowFloodInsuranceDue = Utils.ParseDouble((object) this.FltVal("SERVICE.X115"));
      schedulePayLog.EscrowCityPropertyTaxDue = Utils.ParseDouble((object) this.FltVal("SERVICE.X116"));
      schedulePayLog.EscrowOther1Due = Utils.ParseDouble((object) this.FltVal("SERVICE.X117"));
      schedulePayLog.EscrowOther2Due = Utils.ParseDouble((object) this.FltVal("SERVICE.X119"));
      schedulePayLog.EscrowOther3Due = Utils.ParseDouble((object) this.FltVal("SERVICE.X120"));
      schedulePayLog.EscrowUSDAMonthlyPremiumDue = Utils.ParseDouble((object) this.FltVal("SERVICE.X118"));
      this.loan.AddServicingTransaction((ServicingTransactionBase) schedulePayLog, false);
    }

    private void updateEscrowDisbursement(int escrowType, double payOff)
    {
      double num1 = 0.0;
      double num2 = 0.0;
      DateTime dateTime = DateTime.MinValue;
      for (int index = 0; index < this.paySnapshot.EscrowPayments.Length; ++index)
      {
        num2 = 0.0;
        dateTime = DateTime.MinValue;
        if (this.paySnapshot.EscrowPayments[index] != null)
        {
          switch (escrowType)
          {
            case 1:
              num2 = this.paySnapshot.EscrowPayments[index].Tax;
              dateTime = this.paySnapshot.EscrowPayments[index].PayDate_Tax;
              break;
            case 2:
              num2 = this.paySnapshot.EscrowPayments[index].HazardInsurance;
              dateTime = this.paySnapshot.EscrowPayments[index].PayDate_Hazard;
              break;
            case 3:
              num2 = this.paySnapshot.EscrowPayments[index].MortgageInsurance;
              dateTime = this.paySnapshot.EscrowPayments[index].PayDate_Mortgage;
              break;
            case 4:
              num2 = this.paySnapshot.EscrowPayments[index].FloodInsurance;
              dateTime = this.paySnapshot.EscrowPayments[index].PayDate_Flood;
              break;
            case 5:
              num2 = this.paySnapshot.EscrowPayments[index].CityTax;
              dateTime = this.paySnapshot.EscrowPayments[index].PayDate_City;
              break;
            case 6:
              num2 = this.paySnapshot.EscrowPayments[index].UserTax1;
              dateTime = this.paySnapshot.EscrowPayments[index].PayDate_User1;
              break;
            case 7:
              num2 = this.paySnapshot.EscrowPayments[index].UserTax2;
              dateTime = this.paySnapshot.EscrowPayments[index].PayDate_User2;
              break;
            case 8:
              num2 = this.paySnapshot.EscrowPayments[index].UserTax3;
              dateTime = this.paySnapshot.EscrowPayments[index].PayDate_User3;
              break;
            case 9:
              num2 = this.paySnapshot.EscrowPayments[index].USDAPremium;
              dateTime = this.paySnapshot.EscrowPayments[index].PayDate_USDAPremium;
              break;
          }
          if (num2 != 0.0)
          {
            num1 += num2;
            if (num1 > payOff)
              break;
          }
        }
      }
      int num3 = escrowType == 9 ? 105 : escrowType * 2 + 56;
      int num4 = num3 + 1;
      if (num2 > 0.0)
      {
        this.SetCurrentNum("SERVICE.X" + num3.ToString(), num2);
        if (dateTime != DateTime.MinValue)
          this.SetVal("SERVICE.X" + num4.ToString(), dateTime.ToString("MM/dd/yyyy"));
        else
          this.SetVal("SERVICE.X" + num4.ToString(), "");
      }
      else
      {
        this.SetCurrentNum("SERVICE.X" + num3.ToString(), 0.0);
        this.SetVal("SERVICE.X" + num4.ToString(), "");
      }
    }

    private double accumulateLateCharge(SchedulePaymentLog schedulePay)
    {
      double ofThePayment = schedulePay.PrincipalDue + schedulePay.InterestDue + schedulePay.EscrowDue + schedulePay.MiscFeeDue;
      double overduePayment = schedulePay.PrincipalDue - schedulePay.Principal + schedulePay.InterestDue - schedulePay.Interest + schedulePay.EscrowDue - schedulePay.Escrow + schedulePay.MiscFeeDue - schedulePay.MiscFee;
      double principalAndInterest = schedulePay.PrincipalDue - schedulePay.Principal + schedulePay.InterestDue - schedulePay.Interest;
      return this.CalcLateCharge(ofThePayment, overduePayment, schedulePay.InterestDue, principalAndInterest, this.paySnapshot.GetNumField("674"), this.paySnapshot.GetField("1719"), this.paySnapshot.MinLateCharge, this.paySnapshot.MaxLateCharge);
    }

    internal double CalcLateCharge(
      double ofThePayment,
      double overduePayment,
      double interest,
      double principalAndInterest,
      double lateChargePercent,
      string lateChargeType,
      double minLateCharge,
      double maxLateCharge)
    {
      lateChargePercent /= 100.0;
      double val = 0.0;
      switch (lateChargeType.ToLower())
      {
        case "of the payment":
        case "of any installment":
          val = lateChargePercent * ofThePayment;
          break;
        case "of the overdue payment":
          val = lateChargePercent * overduePayment;
          break;
        case "of the interest payment due":
          val = lateChargePercent * interest;
          break;
        case "of the principal and interest overdue":
          val = lateChargePercent * principalAndInterest;
          break;
      }
      if (val < 0.0)
        return 0.0;
      if (val < minLateCharge)
        val = minLateCharge;
      if (maxLateCharge > 0.0 && val > maxLateCharge)
        val = maxLateCharge;
      return this.Truncate(val, 2);
    }

    private void updateRecentSchedulePaymentLog(SchedulePaymentLog schedulePayLog)
    {
      double[] escrow = new double[9]
      {
        this.FltVal("231"),
        this.FltVal("230"),
        this.FltVal("232"),
        this.FltVal("235"),
        this.FltVal("L268"),
        this.FltVal("1630"),
        this.FltVal("253"),
        this.FltVal("254"),
        this.FltVal("NEWHUD.X1707")
      };
      string field1 = this.loan.GetField("HUD0141");
      if (string.IsNullOrWhiteSpace(field1) || field1 == "//")
        escrow[0] = 0.0;
      string field2 = this.loan.GetField("HUD0142");
      if (string.IsNullOrWhiteSpace(field2) || field2 == "//")
        escrow[1] = 0.0;
      string field3 = this.loan.GetField("HUD0143");
      if (string.IsNullOrWhiteSpace(field3) || field3 == "//")
        escrow[2] = 0.0;
      string field4 = this.loan.GetField("HUD0144");
      if (string.IsNullOrWhiteSpace(field4) || field4 == "//")
        escrow[3] = 0.0;
      string field5 = this.loan.GetField("HUD0145");
      if (string.IsNullOrWhiteSpace(field5) || field5 == "//")
        escrow[4] = 0.0;
      string field6 = this.loan.GetField("HUD0146");
      if (string.IsNullOrWhiteSpace(field6) || field6 == "//")
        escrow[5] = 0.0;
      string field7 = this.loan.GetField("HUD0147");
      if (string.IsNullOrWhiteSpace(field7) || field7 == "//")
        escrow[6] = 0.0;
      string field8 = this.loan.GetField("HUD0148");
      if (string.IsNullOrWhiteSpace(field8) || field8 == "//")
        escrow[7] = 0.0;
      string field9 = this.loan.GetField("HUD0149");
      if (string.IsNullOrWhiteSpace(field9) || field9 == "//")
        escrow[8] = 0.0;
      this.setPaymentEscrow(escrow);
      this.loan.SetField("SERVICE.X20", this.loan.GetField("HUD24"));
      schedulePayLog.EscrowDue = Utils.ToDouble(this.loan.GetField("SERVICE.X20"));
      schedulePayLog.EscrowTaxDue = Utils.ToDouble(this.loan.GetField("SERVICE.X112"));
      schedulePayLog.EscrowHazardInsuranceDue = Utils.ToDouble(this.loan.GetField("SERVICE.X113"));
      schedulePayLog.EscrowMortgageInsuranceDue = Utils.ToDouble(this.loan.GetField("SERVICE.X114"));
      schedulePayLog.EscrowFloodInsuranceDue = Utils.ToDouble(this.loan.GetField("SERVICE.X115"));
      schedulePayLog.EscrowCityPropertyTaxDue = Utils.ToDouble(this.loan.GetField("SERVICE.X116"));
      schedulePayLog.EscrowOther1Due = Utils.ToDouble(this.loan.GetField("SERVICE.X117"));
      schedulePayLog.EscrowOther2Due = Utils.ToDouble(this.loan.GetField("SERVICE.X119"));
      schedulePayLog.EscrowOther3Due = Utils.ToDouble(this.loan.GetField("SERVICE.X120"));
      schedulePayLog.EscrowUSDAMonthlyPremiumDue = Utils.ToDouble(this.loan.GetField("SERVICE.X118"));
    }

    private void updatePerviousSchedule(ServicingTransactionBase[] trans)
    {
      for (int index = 0; index < trans.Length; ++index)
      {
        if (trans[index] is SchedulePaymentLog)
        {
          SchedulePaymentLog tran = (SchedulePaymentLog) trans[index];
          tran.EscrowTaxDue = Utils.ParseDouble((object) this.FltVal("231"));
          tran.EscrowHazardInsuranceDue = Utils.ParseDouble((object) this.FltVal("230"));
          tran.EscrowMortgageInsuranceDue = Utils.ParseDouble((object) this.FltVal("232"));
          tran.EscrowFloodInsuranceDue = Utils.ParseDouble((object) this.FltVal("235"));
          tran.EscrowCityPropertyTaxDue = Utils.ParseDouble((object) this.FltVal("L268"));
          tran.EscrowOther1Due = Utils.ParseDouble((object) this.FltVal("1630"));
          tran.EscrowOther2Due = Utils.ParseDouble((object) this.FltVal("253"));
          tran.EscrowOther3Due = Utils.ParseDouble((object) this.FltVal("254"));
          tran.EscrowUSDAMonthlyPremiumDue = Utils.ParseDouble((object) this.FltVal("NEWHUD.X1707"));
        }
      }
    }

    private void copyFromInterimServicing(string id, string value)
    {
      if (!(id == "SERVICE.X57") || !(this.Val("2626") == "Correspondent"))
        return;
      this.SetVal("4106", this.Val("SERVICE.X57"));
    }

    private void loadPurchaseAdvicePrincipal(string id, string value)
    {
      if (!(this.Val("SERVICE.X8") == "Current") && !(this.Val("SERVICE.X8") == "Past Due") || this.Val("2626") == "Correspondent")
        return;
      if (this.Val("2370") != "//" && this.Val("2370") != "" && this.Val("3514") != "//" && this.Val("3514") != "" && this.Val("2211") != "")
      {
        this.SetVal("SERVICE.X139", this.Val("2211"));
        this.SetVal("SERVICE.X57", (this.FltVal("SERVICE.X57") - this.FltVal("2211")).ToString());
      }
      else
      {
        double num = this.FltVal("SERVICE.X57") + this.FltVal("2211");
        this.SetVal("SERVICE.X139", "");
        this.SetVal("SERVICE.X57", num.ToString());
      }
      this.CalcInterimServicing(true);
    }

    private void updateInterimAfterPurchase(ServicingTransactionBase[] transactions, bool current)
    {
      if (this.Val("2370") != "//" && this.Val("2370") != "" && this.Val("3514") != "//" && this.Val("3514") != "" && this.Val("2211") != "")
      {
        this.SetCurrentNum("SERVICE.X139", Utils.ToDouble(this.Val("2211")));
        this.SetCurrentNum("SERVICE.X57", Utils.ToDouble(this.Val("SERVICE.X57")) - Utils.ToDouble(this.Val("2211")));
        LoanPurchaseLog transactionLog;
        if (transactions != null)
        {
          int index1 = 0;
          for (int index2 = 0; index2 < transactions.Length; ++index2)
          {
            if (transactions[index2] is LoanPurchaseLog)
              index1 = index2;
          }
          if (index1 == 0 && !(transactions[0] is LoanPurchaseLog))
          {
            transactionLog = new LoanPurchaseLog();
            transactionLog.CreatedTime = DateTime.Now;
          }
          else
            transactionLog = (LoanPurchaseLog) transactions[index1];
        }
        else
        {
          transactionLog = new LoanPurchaseLog();
          transactionLog.CreatedTime = DateTime.Now;
        }
        transactionLog.TransactionType = ServicingTransactionTypes.PurchaseAdvice;
        transactionLog.PurchaseAdviceDate = Utils.ToDate(this.Val("2370"));
        transactionLog.Investor = this.Val("VEND.X263");
        transactionLog.InvestorLoanNumber = this.Val("352");
        transactionLog.FirstPaymenttoInvestor = Utils.ToDate(this.Val("3514"));
        transactionLog.PurchaseAmount = Utils.ToDouble(this.Val("SERVICE.X139"));
        this.loan.AddServicingTransaction((ServicingTransactionBase) transactionLog, false);
      }
      else
      {
        int index3 = 0;
        this.SetVal("SERVICE.X139", "");
        if (transactions == null)
          return;
        for (int index4 = 0; index4 < transactions.Length; ++index4)
        {
          if (transactions[index4] is LoanPurchaseLog)
            index3 = index4;
        }
        if (index3 == 0 && !(transactions[0] is LoanPurchaseLog))
          return;
        this.loan.RemoveServicingTransaction(transactions[index3]);
      }
    }

    private void ClearAmountOfInitialAdvance(string id, string value)
    {
      if (!(this.Val("1964") != "Y"))
        return;
      this.SetVal("DISCLOSURE.X1153", "");
    }

    private void validateCountyLimit(string id, string value)
    {
      this.ValidateCountyLimit(id, value, false);
    }

    internal void ValidateCountyLimit(string id, string value, bool validationOnly)
    {
      if (this.loan.SkipCountyLimitCalculation || this.skipCountyLimitCheck)
        return;
      bool importInProgress = this.sessionObjects.Session.LoanImportInProgress;
      bool policySetting = (bool) this.sessionObjects.StartupInfo.PolicySettings[(object) "Policies.EnforceCountyLimit"];
      if (!validationOnly)
        this.SetVal("MCAWPUR.X27", "");
      if (this.Val("1172") != "FHA" && !validationOnly || id == "13" && value == "" && !importInProgress || id == "14" && value == "" && !importInProgress || this.Val("13") == "" && !importInProgress || this.Val("16") == "" && !importInProgress || this.Val("14") == "" && !importInProgress || this.Val("MORNET.X40") == "StreamlineWithoutAppraisal")
        return;
      if (id == "16" && (this.IntVal("16") > 4 || this.IntVal("16") < 1) && !importInProgress)
      {
        if (this.internalInvoke & policySetting && this.Val("3894") == "Y" && !validationOnly)
          this.SetVal("1172", "Conventional");
        this.internalInvoke = false;
        throw new CountyLimitException("No county limit record can be found.  Number of units must be greater than 0 and less than 5.");
      }
      string str1 = this.Val("13");
      string countyName = str1;
      string str2 = this.Val("14");
      int noUnit = this.IntVal("16");
      int countyLimit = this.getCountyLimit(str2, countyName, noUnit);
      if (countyLimit == -999)
      {
        string countyLimitCounty = CountyNameMappingUtils.GetCountyLimitCounty(str2, str1);
        if (string.Compare(countyLimitCounty, str1, true) != 0)
          countyLimit = this.getCountyLimit(str2, countyLimitCounty, noUnit);
        if (countyLimit == -999)
        {
          string fhaCountyLimitName = ZipCodeUtils.ConvertToFHACountyLimitName(str2, str1);
          if (string.Compare(fhaCountyLimitName, str1, true) != 0)
            countyLimit = this.getCountyLimit(str2, fhaCountyLimitName, noUnit);
          if (countyLimit == -999)
          {
            string str3 = str1.Trim().Replace(" ", "");
            if (string.Compare(str3, str1, true) != 0)
              countyLimit = this.getCountyLimit(str2, str3, noUnit);
          }
        }
      }
      if (countyLimit == -999 && !importInProgress)
      {
        if (this.internalInvoke & policySetting && this.Val("3894") == "Y" && !validationOnly)
          this.SetVal("1172", "Conventional");
        this.internalInvoke = false;
        throw new CountyLimitException("No valid county limit can be found based on the provided county name and number of units information.");
      }
      if (!importInProgress)
      {
        double num = this.FltVal("1109");
        if (id == "1109" && Utils.IsInt((object) value))
          num = double.Parse(value);
        if ((double) countyLimit < num && this.Val("3894") == "Y")
        {
          bool flag = true;
          if (this.internalInvoke & policySetting)
          {
            if (!validationOnly)
              this.SetVal("1172", "Conventional");
            flag = false;
          }
          else if (policySetting && !validationOnly)
          {
            this.SetVal("1109", "");
            this.calObjs.D1003Cal.CalcField1109("1109", "");
          }
          try
          {
            if (this.ExceededCountyLimitEvent != null)
              this.ExceededCountyLimitEvent((object) null, (EventArgs) null);
          }
          catch
          {
          }
          this.internalInvoke = false;
          if (flag && countyLimit > -999 && !validationOnly)
            this.SetCurrentNum("MCAWPUR.X27", (double) countyLimit);
          if (!policySetting)
          {
            this.skipCountyLimitCheck = true;
            if (!validationOnly)
            {
              this.SetVal("1109", string.Concat((object) num));
              this.calObjs.D1003Cal.CalcField1109("1109", string.Concat((object) num));
            }
            this.skipCountyLimitCheck = false;
          }
          throw new CountyLimitException("Loan amt (field:1109) exceeds county limit.");
        }
      }
      if (countyLimit > -999 && !validationOnly)
        this.SetCurrentNum("MCAWPUR.X27", (double) countyLimit);
      this.internalInvoke = false;
    }

    private int getCountyLimit(string stateName, string countyName, int noUnit)
    {
      string fips = ZipCodeUtils.GetFIPS(stateName, countyName);
      if (!string.IsNullOrEmpty(fips))
      {
        int countyLimitByFips = this.sessionObjects.ConfigurationManager.GetCountyLimitByFips(stateName, fips, noUnit);
        if (countyLimitByFips != -999)
          return countyLimitByFips;
      }
      return this.sessionObjects.ConfigurationManager.GetCountyLimit(stateName, countyName, noUnit);
    }

    private void populateOptimalblueResult(string id, string val)
    {
      if (!(val != "") || !(this.Val("OPTIMAL.RESPONSE") != ""))
        return;
      bool flag = false;
      if (this.loan.Validator != null && this.loan.Validator.Enabled)
      {
        flag = true;
        this.loan.Validator.Enabled = false;
      }
      Dictionary<string, string> keyValuePair1 = new ValuePairXmlWriter(this.Val("OPTIMAL.RESPONSE"), "FieldID", "FieldValue").GetKeyValuePair();
      Dictionary<string, string> keyValuePair2 = new ValuePairXmlWriter(this.Val("OPTIMAL.REQUEST"), "FieldID", "FieldValue").GetKeyValuePair();
      if (keyValuePair2.ContainsKey("SnapShotGuid") || keyValuePair2.ContainsKey("WorstCasePricing"))
        return;
      for (int index = 2088; index < 2144; ++index)
        this.SetVal(string.Concat((object) index), "");
      for (int index = 2414; index < 2448; ++index)
        this.SetVal(string.Concat((object) index), "");
      for (int index = 2647; index < 2689; ++index)
        this.SetVal(string.Concat((object) index), "");
      this.SetVal("2848", "");
      this.SetVal("2089", DateTime.Now.ToString("d"));
      if (keyValuePair2.ContainsKey("ValidateRelock") && keyValuePair1.ContainsKey("OPTIMAL.HISTORY"))
      {
        string fromEppsTxHistory = LockUtils.GetQualifiedAsOfDateFromEppsTxHistory(keyValuePair1["OPTIMAL.HISTORY"]);
        if (fromEppsTxHistory != "")
          this.SetVal("2089", fromEppsTxHistory);
      }
      if (this.sessionObjects.StartupInfo != null && this.sessionObjects.StartupInfo.ProductPricingPartner != null && this.sessionObjects.StartupInfo.ProductPricingPartner.PartnerID == "MPS" && !string.IsNullOrEmpty(this.Val("4069")) && Utils.ParseDate((object) this.Val("4069"), DateTime.MinValue) != DateTime.MinValue && !string.IsNullOrEmpty(this.Val("4060")))
        this.SetVal("2089", LockDeskHoursManager.GetLockDateForOnrp((IClientSession) this.sessionObjects.Session, this.sessionObjects, (LoanDataMgr) this.loan.DataMgr, Utils.ParseDate((object) (this.Val("4069") + " " + this.Val("4060")))).ToShortDateString());
      foreach (string key in keyValuePair1.Keys)
      {
        try
        {
          if (key == "2144")
            this.SetVal("4456", keyValuePair1[key]);
          else
            this.SetVal(key, keyValuePair1[key]);
        }
        catch
        {
        }
      }
      this.SetVal("3039", this.sessionObjects.Session.ServerTime.ToString("MM/dd/yyyy hh:mm:ss tt"));
      if (string.Compare(this.Val("2626"), "Correspondent", true) == 0 && keyValuePair1.ContainsKey("OPTIMAL.HISTORY"))
      {
        Decimal fromEppsTxHistory = LockUtils.GetSRPFromEppsTxHistory(keyValuePair1["OPTIMAL.HISTORY"], true);
        if (fromEppsTxHistory != 0M)
        {
          this.SetVal("4201", fromEppsTxHistory.ToString());
          this.SetVal("2101", (Utils.ParseDecimal((object) this.Val("2101")) - fromEppsTxHistory).ToString());
        }
      }
      this.loan.TriggerCalculation("2092", this.Val("2092"));
      this.loan.TriggerCalculation("2089", this.Val("2089"));
      this.SetVal("OPTIMAL.REQUEST", "");
      this.SetVal("OPTIMAL.RESPONSE", "");
      if (!flag)
        return;
      this.loan.Validator.Enabled = true;
    }

    public double CalcTitleEscrowRate(TableFeeListBase.FeeTable t)
    {
      if (t.RateList == string.Empty)
        return 0.0;
      double num1 = 0.0;
      switch (t.CalcBasedOn)
      {
        case "Sales Price":
          num1 = this.FltVal("136");
          break;
        case "Loan Amount":
          num1 = this.FltVal("2");
          break;
        case "Appraisal Value":
          num1 = this.FltVal("356");
          break;
        case "Base Loan Amount":
          num1 = this.FltVal("1109");
          break;
      }
      string empty = string.Empty;
      string[] strArray1 = t.RateList.Split('|');
      int length = strArray1.Length;
      double num2 = 0.0;
      double num3 = 0.0;
      for (int index = 0; index < length; ++index)
      {
        string[] strArray2 = strArray1[index].Split(':');
        if (strArray2.Length > 2)
        {
          double num4 = Utils.ParseDouble((object) strArray2[0]);
          if (num1 <= num4)
          {
            num2 = Utils.ParseDouble((object) strArray2[1]);
            num3 = Utils.ParseDouble((object) strArray2[2]) / 100.0;
            break;
          }
        }
      }
      double num5 = Utils.ParseDouble((object) t.Nearest);
      double num6 = Utils.ParseDouble((object) t.Offset);
      if (num5 != 0.0)
      {
        double num7 = num1 % num5;
        if (num7 != 0.0)
        {
          if (t.Rounding == "Up")
            num1 += num5 - num7;
          else
            num1 -= num7;
        }
        num1 -= num6;
      }
      return Utils.ArithmeticRounding(num1 * num3 + num2, 0);
    }

    public List<FundingFee> GetFundingFees(bool hideZero)
    {
      Dictionary<string, string> dictionary = (Dictionary<string, string>) null;
      if (this.UseNew2015GFEHUD)
      {
        try
        {
          dictionary = new UCDXmlParser(this.loan.Calculator.GetUCDXmlDocument(false, true)).ParseXml(false);
        }
        catch (Exception ex)
        {
        }
      }
      List<FundingFee> fundingFees = new List<FundingFee>();
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      double num1 = 0.0;
      string empty3 = string.Empty;
      string empty4 = string.Empty;
      bool flag1 = this.Val("NEWHUD.X1139") == "Y";
      bool flag2 = this.Val("LE2.X28") == "Y";
      string str1 = (string) null;
      string str2 = (string) null;
      string str3 = (string) null;
      bool flag3 = this.Val("NEWHUD.X750") == "Y";
      bool flag4 = this.Val("NEWHUD.X1017") == "Y";
      if (this.UseNew2015GFEHUD && (!flag3 || !flag4))
      {
        if (!flag3 && dictionary.ContainsValue("Origination Fee"))
        {
          foreach (KeyValuePair<string, string> keyValuePair in dictionary)
          {
            if (keyValuePair.Value == "Origination Fee")
            {
              str1 = keyValuePair.Key.Substring(18);
              str1 = "A." + Utils.ParseInt((object) str1.Substring(0, str1.Length - 2)).ToString("00");
              break;
            }
          }
        }
        if (!flag4 && dictionary != null && dictionary.ContainsValue("Title - Title Insurance Services"))
        {
          foreach (KeyValuePair<string, string> keyValuePair in dictionary)
          {
            if (keyValuePair.Value == "Title - Title Insurance Services")
            {
              if (keyValuePair.Key.IndexOf("DidNotShop") > -1)
              {
                str2 = keyValuePair.Key.Substring(29);
                str2 = "B." + Utils.ParseInt((object) str2.Substring(0, str2.Length - 2)).ToString("00");
              }
              else if (keyValuePair.Key.IndexOf("DidShop") > -1)
              {
                str3 = keyValuePair.Key.Substring(26);
                str3 = "C." + Utils.ParseInt((object) str3.Substring(0, str3.Length - 2)).ToString("00");
              }
              if (str2 != null)
              {
                if (str3 != null)
                  break;
              }
            }
          }
        }
      }
      string[] strArray = (string[]) null;
      List<GFEItem> gfeItemList = this.UseNewGFEHUD || this.UseNew2015GFEHUD ? GFEItemCollection.GFEItems2010 : GFEItemCollection.GFEItems;
      for (int index1 = 0; index1 < gfeItemList.Count; ++index1)
      {
        GFEItem gfeItem = gfeItemList[index1];
        if (gfeItem.LineNumber < 2001 && (!(gfeItem.For2015 == "Y") && gfeItem.LineNumber >= 100 || this.UseNew2015GFEHUD) && (!this.UseNew2015GFEHUD || !GFEItemCollection.Excluded2015GFEItem.Contains(gfeItem.LineNumber)) && (!(this.UseNew2015GFEHUD & flag2) || gfeItem.LineNumber >= 520 || gfeItem.LineNumber >= 21 && gfeItem.LineNumber <= 45) && (!this.UseNew2015GFEHUD || flag2 || gfeItem.LineNumber < 21 || gfeItem.LineNumber > 45) && (!this.UseNew2015GFEHUD || (gfeItem.LineNumber != 1101 || !(gfeItem.ComponentID == "x")) && (gfeItem.LineNumber != 1102 || !(gfeItem.ComponentID == "")) && (gfeItem.LineNumber != 1002 || !(this.Val("NEWHUD2.X133") != "Y")) && (gfeItem.LineNumber != 1003 || !(this.Val("NEWHUD2.X4769") != "Y")) && (gfeItem.LineNumber != 1004 || !(this.Val("NEWHUD2.X134") != "Y")) && (gfeItem.LineNumber != 1005 || !(this.Val("NEWHUD2.X135") != "Y")) && (gfeItem.LineNumber != 1006 || !(this.Val("NEWHUD2.X136") != "Y")) && (gfeItem.LineNumber != 1007 || !(this.Val("NEWHUD2.X137") != "Y")) && (gfeItem.LineNumber != 1008 || !(this.Val("NEWHUD2.X138") != "Y")) && (gfeItem.LineNumber != 1009 || !(this.Val("NEWHUD2.X139") != "Y")) && (gfeItem.LineNumber != 1010 || !(this.Val("NEWHUD2.X140") != "Y"))) && (!flag1 || gfeItem.LineNumber != 802 || !(gfeItem.ComponentID == string.Empty)) && (flag1 || gfeItem.LineNumber != 802 || !(gfeItem.ComponentID != "")))
        {
          if (gfeItem.LineNumber == 802 && (this.UseNewGFEHUD || this.UseNew2015GFEHUD) && this.Val("NEWHUD.X713") == "Origination Charge")
          {
            if (flag1)
            {
              if (gfeItem.LineNumber == 802 && gfeItem.ComponentID != string.Empty)
              {
                gfeItem = gfeItem.Clone();
                gfeItem.LineNumber = 801;
                if (gfeItem.ComponentID == "a")
                  gfeItem.ComponentID = "s";
                if (gfeItem.ComponentID == "b")
                  gfeItem.ComponentID = "t";
                if (gfeItem.ComponentID == "c")
                  gfeItem.ComponentID = "u";
                if (gfeItem.ComponentID == "d")
                  gfeItem.ComponentID = "v";
                if (gfeItem.ComponentID == "e")
                  gfeItem.ComponentID = "w";
                if (gfeItem.ComponentID == "f")
                  gfeItem.ComponentID = "x";
                if (gfeItem.ComponentID == "g")
                  gfeItem.ComponentID = "y";
                if (gfeItem.ComponentID == "h")
                  gfeItem.ComponentID = "z";
              }
            }
            else if (this.Val("NEWHUD.X715") == "Include Origination Credit")
              gfeItem = new GFEItem(801, "s", "", "1663", "", "NEWHUD.X749", "", "NEWHUD.X627", "NEWHUD.X398", "", "Origination Credit");
            else if (this.Val("NEWHUD.X715") == "Include Origination Points")
              gfeItem = new GFEItem(801, "s", "", "NEWHUD.X15", "NEWHUD.X788", "NEWHUD.X749", "", "NEWHUD.X627", "NEWHUD.X398", "NEWHUD.X790", "Origination Points");
          }
          if (this.UseNewGFEHUD || this.UseNew2015GFEHUD || !(gfeItem.POCFieldID != string.Empty) || !(this.Val(gfeItem.POCFieldID) == "Y"))
          {
            double num2;
            double num3 = num2 = 0.0;
            double num4 = num2;
            double num5 = num2;
            double num6 = num2;
            double num7 = num2;
            double num8 = num2;
            num1 = num2;
            double num9 = num2;
            for (int index2 = 1; index2 <= 2 && (this.UseNewGFEHUD || this.UseNew2015GFEHUD || gfeItem.LineNumber != 824 && gfeItem.LineNumber != 825 || index2 < 2); ++index2)
            {
              string str4 = !(gfeItem.PaidByFieldID != "") ? (this.UseNewGFEHUD || this.UseNew2015GFEHUD || gfeItem.LineNumber != 824 && gfeItem.LineNumber != 825 ? string.Empty : "Lender") : this.Val(gfeItem.PaidByFieldID);
              if ((gfeItem.LineNumber >= 520 || index2 != 2 && (!(str4 != "Credit") || !(str4 != "Debt"))) && (!(gfeItem.BorrowerFieldID == "558") && gfeItem.LineNumber != 1011 || index2 != 2) && ((gfeItem.LineNumber != 801 || !(gfeItem.ComponentID == "f") && !(gfeItem.ComponentID == "x")) && !(gfeItem.ComponentID == "y") && !(gfeItem.ComponentID == "z") && (gfeItem.LineNumber != 802 || this.UseNew2015GFEHUD || !(gfeItem.ComponentID == "f") && !(gfeItem.ComponentID == "g") && !(gfeItem.ComponentID == "h")) || index2 != 2))
              {
                if ((this.UseNewGFEHUD || this.UseNew2015GFEHUD) && index2 == 1)
                {
                  strArray = HUDGFE2010Fields.PAIDBYPOPTFIELDS.ContainsKey((object) gfeItem.PaidByFieldID) ? (string[]) HUDGFE2010Fields.PAIDBYPOPTFIELDS[(object) gfeItem.PaidByFieldID] : (string[]) null;
                  if (strArray == null && gfeItem.LineNumber >= 520 && gfeItem.LineNumber != 802 && gfeItem.LineNumber != 1011)
                    continue;
                }
                FundingFee fundingFee = new FundingFee();
                fundingFee.LineID = !this.UseNew2015GFEHUD || gfeItem.LineNumber != 803 || !(gfeItem.ComponentID == "x") ? gfeItem.LineNumber.ToString() + gfeItem.ComponentID + "." : "803.";
                if (this.UseNew2015GFEHUD)
                {
                  if (fundingFee.LineID == "802e.")
                    fundingFee.CDLineID = "A.01";
                  else if (fundingFee.LineID == "802a." || fundingFee.LineID == "802b." || fundingFee.LineID == "802c." || fundingFee.LineID == "802d.")
                    fundingFee.CDLineID = "J.02";
                  else if (fundingFee.LineID == "803." && dictionary != null && dictionary.ContainsKey("LINE:803x"))
                    fundingFee.CDLineID = dictionary["LINE:803x"];
                  else if (fundingFee.LineID == "1204." || fundingFee.LineID == "1205.")
                    fundingFee.CDLineID = "E.02";
                  else if (fundingFee.LineID.StartsWith("801") && fundingFee.LineID != "801f." && fundingFee.LineID != "801e." && str1 != null)
                    fundingFee.CDLineID = str1;
                  else if (fundingFee.LineID.StartsWith("1101") & flag4 && dictionary != null && dictionary.ContainsKey("LINE:" + fundingFee.LineID.Replace(".", "")))
                    fundingFee.CDLineID = dictionary["LINE:" + fundingFee.LineID.Replace(".", "")];
                  else if (fundingFee.LineID.StartsWith("1101") && strArray != null && strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORDIDSHOP] != "")
                  {
                    if (this.Val(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORDIDSHOP]) == "Y" && str3 != null)
                      fundingFee.CDLineID = str3;
                    else if (str2 != null)
                      fundingFee.CDLineID = str2;
                  }
                  else
                    fundingFee.CDLineID = dictionary == null || !dictionary.ContainsKey("LINE:" + fundingFee.LineID.Replace(".", "")) ? "" : dictionary["LINE:" + fundingFee.LineID.Replace(".", "")];
                }
                if (gfeItem.LineNumber == 802 && gfeItem.ComponentID == "e" && this.UseNew2015GFEHUD)
                  fundingFee.FeeDescription = this.Val("NEWHUD2.X928") + " % of Loan Amount (Points)";
                else if (gfeItem.Description.Length <= 4 || gfeItem.Description.StartsWith("NEWHUD.X") || gfeItem.Description.StartsWith("NEWHUD2.X") || gfeItem.Description.StartsWith("CD3.X"))
                  fundingFee.FeeDescription = this.Val(gfeItem.Description);
                else if (gfeItem.LineNumber == 1204 || gfeItem.LineNumber == 1205)
                {
                  string taxStampIndicator = gfeItem.LineNumber == 1204 ? this.Val("4855") : this.Val("4856");
                  fundingFee.FeeDescription = UCDXmlExporterBase.GetNewFeeDescription(gfeItem.LineNumber, gfeItem.Description, taxStampIndicator);
                }
                else
                  fundingFee.FeeDescription = gfeItem.Description;
                fundingFee.Payee = gfeItem.PayeeFieldID.Length <= 0 ? string.Empty : this.Val(gfeItem.PayeeFieldID);
                double num10 = 0.0;
                double num11 = 0.0;
                string str5 = string.Empty;
                double num12 = 0.0;
                string str6 = string.Empty;
                double num13;
                double num14 = num13 = 0.0;
                if (index2 == 1)
                {
                  if (gfeItem.BorrowerFieldID != string.Empty)
                  {
                    num9 = num10 = Utils.ParseDouble((object) this.Val(gfeItem.BorrowerFieldID));
                    if (flag1 && (gfeItem.LineNumber == 802 && (gfeItem.ComponentID == "a" || gfeItem.ComponentID == "b" || gfeItem.ComponentID == "c" || gfeItem.ComponentID == "d") || gfeItem.LineNumber == 801 && (gfeItem.ComponentID == "s" || gfeItem.ComponentID == "t" || gfeItem.ComponentID == "u" || gfeItem.ComponentID == "v")))
                      num10 *= -1.0;
                    if (this.UseNew2015GFEHUD)
                    {
                      if (strArray != null)
                      {
                        num14 = Utils.ParseDouble((object) this.Val(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORPOCAMT]));
                        if (num10 != 0.0)
                        {
                          num8 = Utils.ParseDouble((object) this.Val(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPOCAMT]));
                          num7 = Utils.ParseDouble((object) this.Val(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPOCAMT]));
                          num6 = Utils.ParseDouble((object) this.Val(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPOCAMT]));
                          num5 = Utils.ParseDouble((object) this.Val(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPACAMT]));
                          num4 = Utils.ParseDouble((object) this.Val(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPACAMT]));
                          num3 = Utils.ParseDouble((object) this.Val(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPACAMT]));
                          if (gfeItem.LineNumber == 801 && gfeItem.ComponentID == "f")
                            num10 -= num7;
                          else if (Utils.ParseDouble((object) this.Val(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORAMTPAID])) == 0.0 && Utils.ParseDouble((object) this.Val(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015SELAMTPAID])) == 0.0)
                            num10 = num5 + num4 + num3;
                          else
                            num10 -= num14 + num7 + num6 + num8;
                        }
                      }
                    }
                    else if (this.UseNewGFEHUD && strArray != null)
                    {
                      num11 = Utils.ParseDouble((object) this.Val(strArray[HUDGFE2010Fields.PTCPOCINDEX_POCAMT]));
                      str5 = this.Val(strArray[HUDGFE2010Fields.PTCPOCINDEX_POCPAIDBY]);
                      num12 = Utils.ParseDouble((object) this.Val(strArray[HUDGFE2010Fields.PTCPOCINDEX_PTCAMT]));
                      str6 = this.Val(strArray[HUDGFE2010Fields.PTCPOCINDEX_PTCPAIDBY]);
                      num10 -= num11;
                    }
                  }
                }
                else if (gfeItem.SellerFieldID != string.Empty)
                {
                  double num15 = num10 = Utils.ParseDouble((object) this.Val(gfeItem.SellerFieldID));
                  if (this.UseNew2015GFEHUD && strArray != null)
                  {
                    num13 = Utils.ParseDouble((object) this.Val(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015SELPOCAMT]));
                    num10 = Utils.ParseDouble((object) this.Val(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015SELPACAMT]));
                    if (num15 > 0.0 && num9 == 0.0)
                    {
                      num8 = Utils.ParseDouble((object) this.Val(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPOCAMT]));
                      num7 = Utils.ParseDouble((object) this.Val(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPOCAMT]));
                      num6 = Utils.ParseDouble((object) this.Val(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPOCAMT]));
                      num5 = Utils.ParseDouble((object) this.Val(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPACAMT]));
                      num4 = Utils.ParseDouble((object) this.Val(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPACAMT]));
                      num3 = Utils.ParseDouble((object) this.Val(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPACAMT]));
                    }
                    else
                    {
                      double num16;
                      num3 = num16 = 0.0;
                      num4 = num16;
                      num5 = num16;
                      num6 = num16;
                      num7 = num16;
                      num8 = num16;
                    }
                  }
                }
                else
                  continue;
                if (!(num10 == 0.0 & hideZero) && ((num10 != 0.0 ? 0 : (index2 == 2 ? 1 : 0)) & (hideZero ? 1 : 0)) == 0)
                {
                  if (index2 == 1)
                  {
                    fundingFee.PaidBy = !(str4 != "") || gfeItem.LineNumber <= 520 ? (!(gfeItem.BorrowerFieldID == "558") ? "Borrower" : "") : (!(str4 == "Seller") || num10 != 0.0 || hideZero ? str4 : "Borrower");
                    if (gfeItem.CheckBorrowerFieldID != "" && this.Val(gfeItem.CheckBorrowerFieldID) == "Y")
                      fundingFee.BalanceChecked = true;
                  }
                  else
                  {
                    fundingFee.PaidBy = "Seller";
                    if (gfeItem.CheckSellerFieldID != "" && this.Val(gfeItem.CheckSellerFieldID) == "Y")
                      fundingFee.BalanceChecked = true;
                  }
                  string str7 = !(gfeItem.PTBFieldID != "") ? (this.UseNewGFEHUD || this.UseNew2015GFEHUD || gfeItem.LineNumber != 824 && gfeItem.LineNumber != 825 ? string.Empty : "Broker") : this.Val(gfeItem.PTBFieldID);
                  fundingFee.PaidTo = !(str7 == "Broker") ? (!(gfeItem.BorrowerFieldID == "558") ? "Lender/Other" : "") : "Broker";
                  if (gfeItem.LineNumber < 520 && gfeItem.PaidByFieldID != string.Empty && this.Val(gfeItem.PaidByFieldID) == "Credit")
                    num10 *= -1.0;
                  if (gfeItem.LineNumber == 801 && gfeItem.ComponentID == "s" && gfeItem.BorrowerFieldID == "1663")
                    num10 *= -1.0;
                  if (this.UseNew2015GFEHUD && gfeItem.LineNumber == 801 && gfeItem.ComponentID == "f")
                  {
                    fundingFee.PACLender2015 = num10;
                    fundingFee.Amount = num10;
                  }
                  else
                    fundingFee.Amount = num10;
                  if (this.UseNew2015GFEHUD)
                  {
                    if (index2 == 1)
                      fundingFee.POCBorrower2015 = num14;
                    else
                      fundingFee.POCSeller2015 = num13;
                    fundingFee.POCBroker2015 = num8;
                    fundingFee.POCLender2015 = num7;
                    fundingFee.POCOther2015 = num6;
                    fundingFee.PACBroker2015 = num5;
                    fundingFee.PACLender2015 = num4;
                    fundingFee.PACOther2015 = num3;
                  }
                  else
                  {
                    fundingFee.POCAmount = num11;
                    fundingFee.POCPaidBy = num11 != 0.0 ? (str5 == string.Empty ? "Borrower" : str5) : "";
                    fundingFee.PTCAmount = num12;
                    fundingFee.PTCPaidBy = num12 != 0.0 ? (str6 == string.Empty ? "Borrower" : str6) : "";
                  }
                  fundingFee.Tag = (object) gfeItem;
                  fundingFees.Add(fundingFee);
                }
              }
            }
          }
        }
      }
      string val = "";
      PropertyInfo[] properties = typeof (FundingFee).GetProperties();
      for (int index = 0; index < fundingFees.Count; ++index)
      {
        string str8 = (string) null;
        foreach (PropertyInfo propertyInfo in properties)
        {
          if (!(propertyInfo.Name == "Tag") && !(propertyInfo.GetSetMethod() == (MethodInfo) null))
          {
            object obj = propertyInfo.GetValue((object) fundingFees[index]);
            str8 = str8 + (str8 != null ? "|" : "") + (obj != null ? obj.ToString() : "");
          }
        }
        val = val + (val != "" ? "^" : "") + str8;
      }
      this.SetVal("FUNDINGFEES", val);
      if (this.loan != null && this.loan.Calculator != null)
        this.loan.Calculator.EditCalcOnDemandEnum(CalcOnDemandEnum.FundingFees, false);
      return fundingFees;
    }

    public Dictionary<string, object> GetFundingBalancingWorksheet()
    {
      Dictionary<string, object> balancingWorksheet = new Dictionary<string, object>();
      List<string[]> strArrayList1 = new List<string[]>();
      double num1 = 0.0;
      if (this.UseNew2015GFEHUD)
      {
        double num2 = this.FltVal("4083");
        if (num2 > 0.0)
        {
          strArrayList1.Add(new string[2]
          {
            "Lender Credits",
            num2.ToString("N2")
          });
          num1 += num2;
        }
      }
      double num3 = this.FltVal("2");
      strArrayList1.Add(new string[2]
      {
        "Total Loan Amount",
        num3.ToString("N2")
      });
      double amount = num1 + num3;
      balancingWorksheet.Add("Debits", (object) strArrayList1);
      balancingWorksheet.Add("Total Debits", (object) amount.ToString("N2"));
      List<string[]> strArrayList2 = new List<string[]>();
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      string empty = string.Empty;
      double num4 = 0.0;
      bool flag1 = this.Val("NEWHUD.X1139") == "Y";
      bool flag2 = this.Val("LE2.X28") == "Y";
      List<GFEItem> gfeItemList = this.UseNewGFEHUD || this.UseNew2015GFEHUD ? GFEItemCollection.GFEItems2010 : GFEItemCollection.GFEItems;
      for (int index = 0; index < gfeItemList.Count; ++index)
      {
        GFEItem gfeItemObject = gfeItemList[index];
        string key = gfeItemObject.LineNumber.ToString() + gfeItemObject.ComponentID;
        if ((this.UseNew2015GFEHUD || gfeItemObject.LineNumber >= 100 && gfeItemObject.LineNumber != 701 && gfeItemObject.LineNumber != 702 && gfeItemObject.LineNumber != 703 && gfeItemObject.LineNumber != 704) && (!this.UseNew2015GFEHUD || !GFEItemCollection.Excluded2015GFEItem.Contains(gfeItemObject.LineNumber)) && (!(this.UseNew2015GFEHUD & flag2) || gfeItemObject.LineNumber >= 520 || gfeItemObject.LineNumber >= 21 && gfeItemObject.LineNumber <= 45) && (!this.UseNew2015GFEHUD || flag2 || gfeItemObject.LineNumber < 21 || gfeItemObject.LineNumber > 45) && this.CalcFundingBalancingWorksheet((object) gfeItemObject, ref empty, ref amount))
        {
          if (!dictionary.ContainsKey(key))
            dictionary.Add(key, amount.ToString());
          List<string> stringList = new List<string>();
          if (gfeItemObject.LineNumber == 802 && (this.UseNewGFEHUD || this.UseNew2015GFEHUD) && this.Val("NEWHUD.X713") == "Origination Charge")
          {
            if (flag1)
            {
              GFEItem gfeItem = gfeItemObject.Clone();
              gfeItem.LineNumber = 801;
              if (gfeItem.ComponentID == "a")
                gfeItem.ComponentID = "s";
              if (gfeItem.ComponentID == "b")
                gfeItem.ComponentID = "t";
              if (gfeItem.ComponentID == "c")
                gfeItem.ComponentID = "u";
              if (gfeItem.ComponentID == "d")
                gfeItem.ComponentID = "v";
              if (gfeItem.ComponentID == "e")
                gfeItem.ComponentID = "w";
              if (gfeItem.ComponentID == "f")
                gfeItem.ComponentID = "x";
              if (gfeItem.ComponentID == "g")
                gfeItem.ComponentID = "y";
              if (gfeItem.ComponentID == "h")
                gfeItem.ComponentID = "z";
              stringList.Add(gfeItem.LineNumber.ToString() + gfeItem.ComponentID + ".");
              if (gfeItem.LineNumber == 802 && gfeItem.ComponentID == "e" && this.UseNew2015GFEHUD)
                stringList.Add(this.loan.GetField("NEWHUD2.X928") + " % of Loan Amount (Points)");
              else if (gfeItem.Description.Length <= 4 || gfeItem.Description.StartsWith("NEWHUD.X") || gfeItem.Description.StartsWith("CD3.X"))
                stringList.Add(this.Val(gfeItem.Description));
              else
                stringList.Add(gfeItem.Description);
            }
            else
            {
              stringList.Add("801s.");
              if (this.loan.GetField("NEWHUD.X715") == "Include Origination Credit")
                stringList.Add("Origination Credit");
              else
                stringList.Add("Origination Points");
            }
          }
          else
          {
            if (this.UseNew2015GFEHUD && gfeItemObject.LineNumber < 520)
              stringList.Add(FundingFee.GetCDLineID(gfeItemObject.LineNumber));
            else
              stringList.Add(gfeItemObject.LineNumber.ToString() + gfeItemObject.ComponentID + ".");
            if (gfeItemObject.LineNumber == 802 && gfeItemObject.ComponentID == "e" && this.UseNew2015GFEHUD)
              stringList.Add(this.loan.GetField("NEWHUD2.X928") + " % of Loan Amount (Points)");
            else if (gfeItemObject.Description.Length <= 4 || gfeItemObject.Description.StartsWith("NEWHUD.X") || gfeItemObject.Description.StartsWith("NEWHUD2.X") || gfeItemObject.Description.StartsWith("CD3.X"))
              stringList.Add(this.Val(gfeItemObject.Description));
            else if (this.loan.Use2015RESPA)
            {
              string taxStampIndicator = "";
              if (gfeItemObject.LineNumber == 1204 || gfeItemObject.LineNumber == 1205)
                taxStampIndicator = gfeItemObject.LineNumber == 1204 ? this.Val("4855") : this.Val("4856");
              string newFeeDescription = UCDXmlExporterBase.GetNewFeeDescription(gfeItemObject.LineNumber, gfeItemObject.Description, taxStampIndicator);
              stringList.Add(FundingFee.GetFeeDescription2015(gfeItemObject.LineNumber.ToString() + gfeItemObject.ComponentID + ".", newFeeDescription));
            }
            else
              stringList.Add(gfeItemObject.Description);
          }
          stringList.Add(amount.ToString("N2"));
          strArrayList2.Add(stringList.ToArray());
          num4 += amount;
        }
      }
      amount = this.FltVal("1990");
      if (amount != 0.0)
      {
        strArrayList2.Add(new string[3]
        {
          "",
          "Wire Transfer Amount",
          amount.ToString("N2")
        });
        num4 += amount;
      }
      balancingWorksheet.Add("Credits", (object) strArrayList2);
      balancingWorksheet.Add("Total Credits", (object) num4.ToString("N2"));
      if (dictionary != null && dictionary.Count > 0)
        balancingWorksheet.Add("Printable Amounts", (object) dictionary);
      return balancingWorksheet;
    }

    public bool CalcFundingBalancingWorksheet(
      object gfeItemObject,
      ref string paidBy,
      ref double amount)
    {
      GFEItem gfeItem = (GFEItem) null;
      if (gfeItemObject is GFEItem)
        gfeItem = (GFEItem) gfeItemObject;
      if (gfeItem == null || this.Val("NEWHUD.X1139") == "Y" && gfeItem.LineNumber == 802 && gfeItem.ComponentID == string.Empty || this.Val("NEWHUD.X1139") != "Y" && gfeItem.LineNumber == 802 && gfeItem.ComponentID != string.Empty)
        return false;
      paidBy = string.Empty;
      amount = 0.0;
      if (gfeItem.LineNumber == 802 && (this.UseNewGFEHUD || this.UseNew2015GFEHUD) && this.Val("NEWHUD.X713") == "Origination Charge")
      {
        if (this.Val("NEWHUD.X1139") == "Y")
        {
          gfeItem = gfeItem.Clone();
          gfeItem.LineNumber = 801;
          if (gfeItem.ComponentID == "a")
            gfeItem.ComponentID = "s";
          if (gfeItem.ComponentID == "b")
            gfeItem.ComponentID = "t";
          if (gfeItem.ComponentID == "c")
            gfeItem.ComponentID = "u";
          if (gfeItem.ComponentID == "d")
            gfeItem.ComponentID = "v";
          if (gfeItem.ComponentID == "e")
            gfeItem.ComponentID = "w";
          if (gfeItem.ComponentID == "f")
            gfeItem.ComponentID = "x";
          if (gfeItem.ComponentID == "g")
            gfeItem.ComponentID = "y";
          if (gfeItem.ComponentID == "h")
            gfeItem.ComponentID = "z";
        }
        else if (this.Val("NEWHUD.X715") == "Include Origination Credit")
          gfeItem = new GFEItem(801, "s", "", "1663", "", "NEWHUD.X749", "", "NEWHUD.X627", "NEWHUD.X398", "", "Origination Credit");
        else if (this.Val("NEWHUD.X715") == "Include Origination Points")
          gfeItem = new GFEItem(801, "s", "", "NEWHUD.X15", "NEWHUD.X788", "NEWHUD.X749", "", "NEWHUD.X627", "NEWHUD.X398", "NEWHUD.X790", "Origination Points");
      }
      paidBy = !(gfeItem.PaidByFieldID != "") ? (this.loan.Use2010RESPA || this.loan.Use2015RESPA || gfeItem.LineNumber != 824 && gfeItem.LineNumber != 825 ? string.Empty : "Lender") : this.Val(gfeItem.PaidByFieldID);
      if (gfeItem.LineNumber < 520)
      {
        if (paidBy != "Credit" && paidBy != "Debt")
          return false;
      }
      else if (gfeItem.LineNumber != 824 && gfeItem.LineNumber != 825 && !this.UseNewGFEHUD && !this.UseNew2015GFEHUD && gfeItem.POCFieldID != string.Empty && this.Val(gfeItem.POCFieldID) == "Y")
        return false;
      if (gfeItem.CheckBorrowerFieldID != string.Empty && this.Val(gfeItem.CheckBorrowerFieldID) == "Y")
      {
        amount = this.FltVal(gfeItem.BorrowerFieldID);
        if (this.Val("NEWHUD.X1139") == "Y" && (gfeItem.LineNumber == 802 && (gfeItem.ComponentID == "a" || gfeItem.ComponentID == "b" || gfeItem.ComponentID == "c" || gfeItem.ComponentID == "d") || gfeItem.LineNumber == 801 && (gfeItem.ComponentID == "s" || gfeItem.ComponentID == "t" || gfeItem.ComponentID == "u" || gfeItem.ComponentID == "v")))
          amount *= -1.0;
        else if ((this.UseNewGFEHUD || this.UseNew2015GFEHUD) && gfeItem.LineNumber > 800 && gfeItem.LineNumber != 1011)
        {
          if (gfeItem.PaidByFieldID == "" || !HUDGFE2010Fields.PAIDBYPOPTFIELDS.ContainsKey((object) gfeItem.PaidByFieldID))
            return false;
          string[] strArray = (string[]) HUDGFE2010Fields.PAIDBYPOPTFIELDS[(object) gfeItem.PaidByFieldID];
          double num = this.FltVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_POCAMT]);
          this.FltVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_PTCAMT]);
          amount -= num;
        }
      }
      if (gfeItem.LineNumber < 520 && gfeItem.PaidByFieldID != string.Empty && this.Val(gfeItem.PaidByFieldID) == "Credit")
        amount *= -1.0;
      if (gfeItem.LineNumber == 801 && gfeItem.ComponentID == "s" && gfeItem.BorrowerFieldID == "1663")
        amount *= -1.0;
      if (gfeItem.CheckSellerFieldID != string.Empty && this.Val(gfeItem.CheckSellerFieldID) == "Y")
        amount += Utils.ParseDouble((object) this.Val(gfeItem.SellerFieldID));
      return amount != 0.0;
    }

    private void calculateSafeHarbor(string id, string val)
    {
      if (this.Val("DISCLOSURE.X732") != "Other Option")
        this.SetVal("DISCLOSURE.X733", "");
      if (this.Val("DISCLOSURE.X689") == "Fixed" || this.Val("DISCLOSURE.X689") == "")
      {
        this.SetVal("DISCLOSURE.X692", "");
        this.SetVal("DISCLOSURE.X704", "");
        this.SetVal("DISCLOSURE.X711", "");
        this.SetVal("DISCLOSURE.X723", "");
      }
      double loanAmount1 = this.FltVal("2");
      double loanAmount2 = loanAmount1;
      string str = this.Val("1172");
      if (str != "FHA")
      {
        if (str == "VA" && this.Val("958") == "IRRRL" && this.Val("19").IndexOf("Refinance") > -1)
          loanAmount1 = this.FltVal("VARRRWS.X2");
      }
      else
        loanAmount1 = this.FltVal("1109");
      this.calculateLoanOption_Fees(loanAmount1, "DISCLOSURE.X735", (string) null, "DISCLOSURE.X737", "DISCLOSURE.X736", id, val);
      this.calculateLoanOption_Fees(loanAmount2, "DISCLOSURE.X744", "DISCLOSURE.X745", "DISCLOSURE.X747", "DISCLOSURE.X746", (string) null, (string) null);
      this.calculateLoanOption_Fees(loanAmount2, "DISCLOSURE.X748", "DISCLOSURE.X749", (string) null, "DISCLOSURE.X750", (string) null, (string) null);
      this.calculateLoanOption_Fees(loanAmount2, "DISCLOSURE.X695", "DISCLOSURE.X977", "DISCLOSURE.X767", "DISCLOSURE.X766", (string) null, (string) null);
      double num1 = this.FltVal("DISCLOSURE.X736") + this.FltVal("DISCLOSURE.X737") + this.FltVal("DISCLOSURE.X738") + this.FltVal("DISCLOSURE.X739") + this.FltVal("DISCLOSURE.X740") + this.FltVal("DISCLOSURE.X741") + this.FltVal("DISCLOSURE.X742") + this.FltVal("DISCLOSURE.X743") + this.FltVal("DISCLOSURE.X746") + this.FltVal("DISCLOSURE.X747") + this.FltVal("DISCLOSURE.X750") + this.FltVal("DISCLOSURE.X752") + this.FltVal("DISCLOSURE.X753") + this.FltVal("DISCLOSURE.X755") + this.FltVal("DISCLOSURE.X756") + this.FltVal("DISCLOSURE.X758") + this.FltVal("DISCLOSURE.X759") + this.FltVal("DISCLOSURE.X761") + this.FltVal("DISCLOSURE.X762") + this.FltVal("DISCLOSURE.X764") + this.FltVal("DISCLOSURE.X765");
      this.SetCurrentNum("DISCLOSURE.X694", num1);
      if (loanAmount1 != 0.0)
        this.SetCurrentNum("DISCLOSURE.X693", Utils.ArithmeticRounding(num1 / loanAmount1 * 100.0, 3));
      else
        this.SetVal("DISCLOSURE.X693", "");
      this.SetCurrentNum("DISCLOSURE.X979", Utils.ArithmeticRounding(this.FltVal("DISCLOSURE.X695") / 100.0 * loanAmount2 + this.FltVal("DISCLOSURE.X977"), 2));
      this.SetCurrentNum("DISCLOSURE.X978", loanAmount2 == 0.0 ? 0.0 : Utils.ArithmeticRounding(this.FltVal("DISCLOSURE.X979") / loanAmount2 * 100.0, 3));
      this.SetCurrentNum("DISCLOSURE.X980", this.FltVal("DISCLOSURE.X694") + this.FltVal("DISCLOSURE.X979"));
      this.calculateLoanOption_Fees(loanAmount1, "DISCLOSURE.X768", (string) null, "DISCLOSURE.X770", "DISCLOSURE.X769", id, val);
      this.calculateLoanOption_Fees(loanAmount2, "DISCLOSURE.X777", "DISCLOSURE.X778", "DISCLOSURE.X780", "DISCLOSURE.X779", (string) null, (string) null);
      this.calculateLoanOption_Fees(loanAmount2, "DISCLOSURE.X781", "DISCLOSURE.X782", (string) null, "DISCLOSURE.X783", (string) null, (string) null);
      this.calculateLoanOption_Fees(loanAmount2, "DISCLOSURE.X707", "DISCLOSURE.X981", "DISCLOSURE.X800", "DISCLOSURE.X799", (string) null, (string) null);
      double num2 = this.FltVal("DISCLOSURE.X769") + this.FltVal("DISCLOSURE.X770") + this.FltVal("DISCLOSURE.X771") + this.FltVal("DISCLOSURE.X772") + this.FltVal("DISCLOSURE.X773") + this.FltVal("DISCLOSURE.X774") + this.FltVal("DISCLOSURE.X775") + this.FltVal("DISCLOSURE.X776") + this.FltVal("DISCLOSURE.X779") + this.FltVal("DISCLOSURE.X780") + this.FltVal("DISCLOSURE.X783") + this.FltVal("DISCLOSURE.X785") + this.FltVal("DISCLOSURE.X786") + this.FltVal("DISCLOSURE.X788") + this.FltVal("DISCLOSURE.X789") + this.FltVal("DISCLOSURE.X791") + this.FltVal("DISCLOSURE.X792") + this.FltVal("DISCLOSURE.X794") + this.FltVal("DISCLOSURE.X795") + this.FltVal("DISCLOSURE.X797") + this.FltVal("DISCLOSURE.X798");
      this.SetCurrentNum("DISCLOSURE.X706", num2);
      if (loanAmount1 != 0.0)
        this.SetCurrentNum("DISCLOSURE.X705", Utils.ArithmeticRounding(num2 / loanAmount1 * 100.0, 3));
      else
        this.SetVal("DISCLOSURE.X705", "");
      this.SetCurrentNum("DISCLOSURE.X983", Utils.ArithmeticRounding(this.FltVal("DISCLOSURE.X707") / 100.0 * loanAmount2 + this.FltVal("DISCLOSURE.X981"), 2));
      this.SetCurrentNum("DISCLOSURE.X982", loanAmount2 == 0.0 ? 0.0 : Utils.ArithmeticRounding(this.FltVal("DISCLOSURE.X983") / loanAmount2 * 100.0, 3));
      this.SetCurrentNum("DISCLOSURE.X984", this.FltVal("DISCLOSURE.X706") + this.FltVal("DISCLOSURE.X983"));
      this.calculateLoanOption_Fees(loanAmount1, "DISCLOSURE.X801", (string) null, "DISCLOSURE.X803", "DISCLOSURE.X802", id, val);
      this.calculateLoanOption_Fees(loanAmount2, "DISCLOSURE.X810", "DISCLOSURE.X811", "DISCLOSURE.X813", "DISCLOSURE.X812", (string) null, (string) null);
      this.calculateLoanOption_Fees(loanAmount2, "DISCLOSURE.X814", "DISCLOSURE.X815", (string) null, "DISCLOSURE.X816", (string) null, (string) null);
      this.calculateLoanOption_Fees(loanAmount2, "DISCLOSURE.X714", "DISCLOSURE.X985", "DISCLOSURE.X833", "DISCLOSURE.X832", (string) null, (string) null);
      double num3 = this.FltVal("DISCLOSURE.X802") + this.FltVal("DISCLOSURE.X803") + this.FltVal("DISCLOSURE.X804") + this.FltVal("DISCLOSURE.X805") + this.FltVal("DISCLOSURE.X806") + this.FltVal("DISCLOSURE.X807") + this.FltVal("DISCLOSURE.X808") + this.FltVal("DISCLOSURE.X809") + this.FltVal("DISCLOSURE.X812") + this.FltVal("DISCLOSURE.X813") + this.FltVal("DISCLOSURE.X816") + this.FltVal("DISCLOSURE.X818") + this.FltVal("DISCLOSURE.X819") + this.FltVal("DISCLOSURE.X821") + this.FltVal("DISCLOSURE.X822") + this.FltVal("DISCLOSURE.X824") + this.FltVal("DISCLOSURE.X825") + this.FltVal("DISCLOSURE.X827") + this.FltVal("DISCLOSURE.X828") + this.FltVal("DISCLOSURE.X830") + this.FltVal("DISCLOSURE.X831");
      this.SetCurrentNum("DISCLOSURE.X713", num3);
      if (loanAmount1 != 0.0)
        this.SetCurrentNum("DISCLOSURE.X712", Utils.ArithmeticRounding(num3 / loanAmount1 * 100.0, 3));
      else
        this.SetVal("DISCLOSURE.X712", "");
      this.SetCurrentNum("DISCLOSURE.X987", Utils.ArithmeticRounding(this.FltVal("DISCLOSURE.X714") / 100.0 * loanAmount2 + this.FltVal("DISCLOSURE.X985"), 2));
      this.SetCurrentNum("DISCLOSURE.X986", loanAmount2 == 0.0 ? 0.0 : Utils.ArithmeticRounding(this.FltVal("DISCLOSURE.X987") / loanAmount2 * 100.0, 3));
      this.SetCurrentNum("DISCLOSURE.X988", this.FltVal("DISCLOSURE.X713") + this.FltVal("DISCLOSURE.X987"));
      this.calculateLoanOption_Fees(loanAmount1, "DISCLOSURE.X834", (string) null, "DISCLOSURE.X836", "DISCLOSURE.X835", id, val);
      this.calculateLoanOption_Fees(loanAmount2, "DISCLOSURE.X843", "DISCLOSURE.X844", "DISCLOSURE.X846", "DISCLOSURE.X845", (string) null, (string) null);
      this.calculateLoanOption_Fees(loanAmount2, "DISCLOSURE.X847", "DISCLOSURE.X848", (string) null, "DISCLOSURE.X849", (string) null, (string) null);
      this.calculateLoanOption_Fees(loanAmount2, "DISCLOSURE.X726", "DISCLOSURE.X989", "DISCLOSURE.X866", "DISCLOSURE.X865", (string) null, (string) null);
      double num4 = this.FltVal("DISCLOSURE.X835") + this.FltVal("DISCLOSURE.X836") + this.FltVal("DISCLOSURE.X837") + this.FltVal("DISCLOSURE.X838") + this.FltVal("DISCLOSURE.X839") + this.FltVal("DISCLOSURE.X840") + this.FltVal("DISCLOSURE.X841") + this.FltVal("DISCLOSURE.X842") + this.FltVal("DISCLOSURE.X845") + this.FltVal("DISCLOSURE.X846") + this.FltVal("DISCLOSURE.X849") + this.FltVal("DISCLOSURE.X851") + this.FltVal("DISCLOSURE.X852") + this.FltVal("DISCLOSURE.X854") + this.FltVal("DISCLOSURE.X855") + this.FltVal("DISCLOSURE.X857") + this.FltVal("DISCLOSURE.X858") + this.FltVal("DISCLOSURE.X860") + this.FltVal("DISCLOSURE.X861") + this.FltVal("DISCLOSURE.X863") + this.FltVal("DISCLOSURE.X864");
      this.SetCurrentNum("DISCLOSURE.X725", num4);
      if (loanAmount1 != 0.0)
        this.SetCurrentNum("DISCLOSURE.X724", Utils.ArithmeticRounding(num4 / loanAmount1 * 100.0, 3));
      else
        this.SetVal("DISCLOSURE.X724", "");
      this.SetCurrentNum("DISCLOSURE.X991", Utils.ArithmeticRounding(this.FltVal("DISCLOSURE.X726") / 100.0 * loanAmount2 + this.FltVal("DISCLOSURE.X989"), 2));
      this.SetCurrentNum("DISCLOSURE.X990", loanAmount2 == 0.0 ? 0.0 : Utils.ArithmeticRounding(this.FltVal("DISCLOSURE.X991") / loanAmount2 * 100.0, 3));
      this.SetCurrentNum("DISCLOSURE.X992", this.FltVal("DISCLOSURE.X725") + this.FltVal("DISCLOSURE.X991"));
    }

    private void calculateLoanOption_Fees(
      double loanAmount,
      string percentID,
      string additionalID,
      string sellerID,
      string borID,
      string id,
      string val)
    {
      if (id == borID && this.IsLocked(borID))
      {
        double num = Utils.ArithmeticRounding((this.FltVal(borID) + (additionalID != null ? this.FltVal(additionalID) : 0.0) + (sellerID != null ? this.FltVal(sellerID) : 0.0)) / loanAmount * 100.0, 2);
        this.SetCurrentNum(percentID, num);
      }
      else
      {
        double num = Utils.ArithmeticRounding(loanAmount * this.FltVal(percentID) / 100.0, 2) + (additionalID != null ? this.FltVal(additionalID) : 0.0) - (sellerID != null ? this.FltVal(sellerID) : 0.0);
        this.SetCurrentNum(borID, num);
      }
    }

    internal void CopySafeHarborToLoan()
    {
      string str = this.Val("DISCLOSURE.X732");
      this.SetVal("608", this.Val("DISCLOSURE.X689"));
      switch (str)
      {
        case "Loan Option 1":
          this.SetCurrentNum("4", this.FltVal("DISCLOSURE.X690"));
          this.SetCurrentNum("3", this.FltVal("DISCLOSURE.X691"));
          this.SetVal("KBYO.XD3", Utils.RemoveEndingZeros(this.FltVal("3").ToString()));
          if (this.Val("19") == "ConstructionOnly")
            this.SetCurrentNum("1677", this.FltVal("DISCLOSURE.X691"));
          this.SetCurrentNum("696", this.FltVal("DISCLOSURE.X692"));
          this.SetCurrentNum("388", this.FltVal("DISCLOSURE.X735"));
          this.SetCurrentNum("454", this.FltVal("DISCLOSURE.X736"));
          if (this.IsLocked("DISCLOSURE.X735") && !this.IsLocked("454"))
            this.loan.AddLock("454");
          this.SetCurrentNum("559", this.FltVal("DISCLOSURE.X737"));
          this.SetCurrentNum("L228", this.FltVal("DISCLOSURE.X738"));
          this.SetCurrentNum("L229", this.FltVal("DISCLOSURE.X739"));
          this.SetCurrentNum("1621", this.FltVal("DISCLOSURE.X740"));
          this.SetCurrentNum("1622", this.FltVal("DISCLOSURE.X741"));
          this.SetCurrentNum("367", this.FltVal("DISCLOSURE.X742"));
          this.SetCurrentNum("569", this.FltVal("DISCLOSURE.X743"));
          this.SetCurrentNum("389", this.FltVal("DISCLOSURE.X744"));
          this.SetCurrentNum("1620", this.FltVal("DISCLOSURE.X745"));
          this.SetCurrentNum("439", this.FltVal("DISCLOSURE.X746"));
          this.SetCurrentNum("572", this.FltVal("DISCLOSURE.X747"));
          this.SetCurrentNum("NEWHUD.X223", this.FltVal("DISCLOSURE.X748"));
          this.SetCurrentNum("NEWHUD.X224", this.FltVal("DISCLOSURE.X749"));
          this.SetCurrentNum("NEWHUD.X225", this.FltVal("DISCLOSURE.X750"));
          this.SetVal("154", this.Val("DISCLOSURE.X751"));
          this.SetCurrentNum("155", this.FltVal("DISCLOSURE.X752"));
          this.SetCurrentNum("200", this.FltVal("DISCLOSURE.X753"));
          this.SetVal("1627", this.Val("DISCLOSURE.X754"));
          this.SetCurrentNum("1625", this.FltVal("DISCLOSURE.X755"));
          this.SetCurrentNum("1626", this.FltVal("DISCLOSURE.X756"));
          this.SetVal("1838", this.Val("DISCLOSURE.X757"));
          this.SetCurrentNum("1839", this.FltVal("DISCLOSURE.X758"));
          this.SetCurrentNum("1840", this.FltVal("DISCLOSURE.X759"));
          this.SetVal("1841", this.Val("DISCLOSURE.X760"));
          this.SetCurrentNum("1842", this.FltVal("DISCLOSURE.X761"));
          this.SetCurrentNum("1843", this.FltVal("DISCLOSURE.X762"));
          this.SetVal("NEWHUD.X732", this.Val("DISCLOSURE.X763"));
          this.SetCurrentNum("NEWHUD.X733", this.FltVal("DISCLOSURE.X764"));
          this.SetCurrentNum("NEWHUD.X779", this.FltVal("DISCLOSURE.X765"));
          if (this.FltVal("DISCLOSURE.X695") != 0.0 || this.FltVal("DISCLOSURE.X977") != 0.0)
          {
            this.SetVal("NEWHUD.X715", "Include Origination Points");
            if (this.Val("NEWHUD.X1139") == "Y")
            {
              this.SetCurrentNum("NEWHUD.X1150", this.FltVal("DISCLOSURE.X695"));
              this.SetCurrentNum("NEWHUD.X1227", this.FltVal("DISCLOSURE.X977"));
              this.SetCurrentNum("NEWHUD.X1152", this.FltVal("DISCLOSURE.X767"));
            }
            else
            {
              this.SetCurrentNum("1061", this.FltVal("DISCLOSURE.X695"));
              this.SetCurrentNum("436", this.FltVal("DISCLOSURE.X977"));
              this.SetCurrentNum("NEWHUD.X788", this.FltVal("DISCLOSURE.X767"));
            }
          }
          else if (this.Val("1061") != string.Empty || this.Val("NEWHUD.X1139") == "Y" && this.Val("NEWHUD.X1150") != string.Empty)
          {
            this.SetVal("NEWHUD.X715", "");
            if (this.Val("NEWHUD.X1139") == "Y")
            {
              this.SetCurrentNum("NEWHUD.X1150", 0.0);
              this.SetCurrentNum("NEWHUD.X1152", 0.0);
              this.SetCurrentNum("NEWHUD.X1227", 0.0);
              this.SetCurrentNum("NEWHUD.X1178", 0.0);
            }
            else
            {
              this.SetCurrentNum("1061", 0.0);
              this.SetCurrentNum("436", 0.0);
              this.SetCurrentNum("NEWHUD.X788", 0.0);
            }
          }
          this.calObjs.VACal.CalcVARRRWS((string) null, (string) null);
          break;
        case "Loan Option 2":
          this.SetCurrentNum("4", this.FltVal("DISCLOSURE.X702"));
          this.SetCurrentNum("3", this.FltVal("DISCLOSURE.X703"));
          this.SetVal("KBYO.XD3", Utils.RemoveEndingZeros(this.FltVal("3").ToString()));
          if (this.Val("19") == "ConstructionOnly")
            this.SetCurrentNum("1677", this.FltVal("DISCLOSURE.X703"));
          this.SetCurrentNum("696", this.FltVal("DISCLOSURE.X704"));
          this.SetCurrentNum("388", this.FltVal("DISCLOSURE.X768"));
          this.SetCurrentNum("454", this.FltVal("DISCLOSURE.X769"));
          if (this.IsLocked("DISCLOSURE.X769") && !this.IsLocked("454"))
            this.loan.AddLock("454");
          this.SetCurrentNum("559", this.FltVal("DISCLOSURE.X770"));
          this.SetCurrentNum("L228", this.FltVal("DISCLOSURE.X771"));
          this.SetCurrentNum("L229", this.FltVal("DISCLOSURE.X772"));
          this.SetCurrentNum("1621", this.FltVal("DISCLOSURE.X773"));
          this.SetCurrentNum("1622", this.FltVal("DISCLOSURE.X774"));
          this.SetCurrentNum("367", this.FltVal("DISCLOSURE.X775"));
          this.SetCurrentNum("569", this.FltVal("DISCLOSURE.X776"));
          this.SetCurrentNum("389", this.FltVal("DISCLOSURE.X777"));
          this.SetCurrentNum("1620", this.FltVal("DISCLOSURE.X778"));
          this.SetCurrentNum("439", this.FltVal("DISCLOSURE.X779"));
          this.SetCurrentNum("572", this.FltVal("DISCLOSURE.X780"));
          this.SetCurrentNum("NEWHUD.X223", this.FltVal("DISCLOSURE.X781"));
          this.SetCurrentNum("NEWHUD.X224", this.FltVal("DISCLOSURE.X782"));
          this.SetCurrentNum("NEWHUD.X225", this.FltVal("DISCLOSURE.X783"));
          this.SetVal("154", this.Val("DISCLOSURE.X784"));
          this.SetCurrentNum("155", this.FltVal("DISCLOSURE.X785"));
          this.SetCurrentNum("200", this.FltVal("DISCLOSURE.X786"));
          this.SetVal("1627", this.Val("DISCLOSURE.X787"));
          this.SetCurrentNum("1625", this.FltVal("DISCLOSURE.X788"));
          this.SetCurrentNum("1626", this.FltVal("DISCLOSURE.X789"));
          this.SetVal("1838", this.Val("DISCLOSURE.X790"));
          this.SetCurrentNum("1839", this.FltVal("DISCLOSURE.X791"));
          this.SetCurrentNum("1840", this.FltVal("DISCLOSURE.X792"));
          this.SetVal("1841", this.Val("DISCLOSURE.X793"));
          this.SetCurrentNum("1842", this.FltVal("DISCLOSURE.X794"));
          this.SetCurrentNum("1843", this.FltVal("DISCLOSURE.X795"));
          this.SetVal("NEWHUD.X732", this.Val("DISCLOSURE.X796"));
          this.SetCurrentNum("NEWHUD.X733", this.FltVal("DISCLOSURE.X797"));
          this.SetCurrentNum("NEWHUD.X779", this.FltVal("DISCLOSURE.X798"));
          if (this.FltVal("DISCLOSURE.X707") != 0.0 || this.FltVal("DISCLOSURE.X981") != 0.0)
          {
            this.SetVal("NEWHUD.X715", "Include Origination Points");
            if (this.Val("NEWHUD.X1139") == "Y")
            {
              this.SetCurrentNum("NEWHUD.X1150", this.FltVal("DISCLOSURE.X707"));
              this.SetCurrentNum("NEWHUD.X1227", this.FltVal("DISCLOSURE.X981"));
              this.SetCurrentNum("NEWHUD.X1152", this.FltVal("DISCLOSURE.X800"));
            }
            else
            {
              this.SetCurrentNum("1061", this.FltVal("DISCLOSURE.X707"));
              this.SetCurrentNum("436", this.FltVal("DISCLOSURE.X981"));
              this.SetCurrentNum("NEWHUD.X788", this.FltVal("DISCLOSURE.X800"));
            }
          }
          else if (this.Val("1061") != string.Empty || this.Val("NEWHUD.X1139") == "Y" && this.Val("NEWHUD.X1150") != string.Empty)
          {
            this.SetVal("NEWHUD.X715", "");
            if (this.Val("NEWHUD.X1139") == "Y")
            {
              this.SetCurrentNum("NEWHUD.X1150", 0.0);
              this.SetCurrentNum("NEWHUD.X1152", 0.0);
              this.SetCurrentNum("NEWHUD.X1227", 0.0);
              this.SetCurrentNum("NEWHUD.X1178", 0.0);
            }
            else
            {
              this.SetCurrentNum("1061", 0.0);
              this.SetCurrentNum("436", 0.0);
              this.SetCurrentNum("NEWHUD.X788", 0.0);
            }
          }
          this.calObjs.VACal.CalcVARRRWS((string) null, (string) null);
          break;
        case "Loan Option 3":
          this.SetCurrentNum("4", this.FltVal("DISCLOSURE.X709"));
          this.SetCurrentNum("3", this.FltVal("DISCLOSURE.X710"));
          this.SetVal("KBYO.XD3", Utils.RemoveEndingZeros(this.FltVal("3").ToString()));
          if (this.Val("19") == "ConstructionOnly")
            this.SetCurrentNum("1677", this.FltVal("DISCLOSURE.X710"));
          this.SetCurrentNum("696", this.FltVal("DISCLOSURE.X711"));
          this.SetCurrentNum("388", this.FltVal("DISCLOSURE.X801"));
          this.SetCurrentNum("454", this.FltVal("DISCLOSURE.X802"));
          if (this.IsLocked("DISCLOSURE.X802") && !this.IsLocked("454"))
            this.loan.AddLock("454");
          this.SetCurrentNum("559", this.FltVal("DISCLOSURE.X803"));
          this.SetCurrentNum("L228", this.FltVal("DISCLOSURE.X804"));
          this.SetCurrentNum("L229", this.FltVal("DISCLOSURE.X805"));
          this.SetCurrentNum("1621", this.FltVal("DISCLOSURE.X806"));
          this.SetCurrentNum("1622", this.FltVal("DISCLOSURE.X807"));
          this.SetCurrentNum("367", this.FltVal("DISCLOSURE.X808"));
          this.SetCurrentNum("569", this.FltVal("DISCLOSURE.X809"));
          this.SetCurrentNum("389", this.FltVal("DISCLOSURE.X810"));
          this.SetCurrentNum("1620", this.FltVal("DISCLOSURE.X811"));
          this.SetCurrentNum("439", this.FltVal("DISCLOSURE.X812"));
          this.SetCurrentNum("572", this.FltVal("DISCLOSURE.X813"));
          this.SetCurrentNum("NEWHUD.X223", this.FltVal("DISCLOSURE.X814"));
          this.SetCurrentNum("NEWHUD.X224", this.FltVal("DISCLOSURE.X815"));
          this.SetCurrentNum("NEWHUD.X225", this.FltVal("DISCLOSURE.X816"));
          this.SetVal("154", this.Val("DISCLOSURE.X817"));
          this.SetCurrentNum("155", this.FltVal("DISCLOSURE.X818"));
          this.SetCurrentNum("200", this.FltVal("DISCLOSURE.X819"));
          this.SetVal("1627", this.Val("DISCLOSURE.X820"));
          this.SetCurrentNum("1625", this.FltVal("DISCLOSURE.X821"));
          this.SetCurrentNum("1626", this.FltVal("DISCLOSURE.X822"));
          this.SetVal("1838", this.Val("DISCLOSURE.X823"));
          this.SetCurrentNum("1839", this.FltVal("DISCLOSURE.X824"));
          this.SetCurrentNum("1840", this.FltVal("DISCLOSURE.X825"));
          this.SetVal("1841", this.Val("DISCLOSURE.X826"));
          this.SetCurrentNum("1842", this.FltVal("DISCLOSURE.X827"));
          this.SetCurrentNum("1843", this.FltVal("DISCLOSURE.X828"));
          this.SetVal("NEWHUD.X732", this.Val("DISCLOSURE.X829"));
          this.SetCurrentNum("NEWHUD.X733", this.FltVal("DISCLOSURE.X830"));
          this.SetCurrentNum("NEWHUD.X779", this.FltVal("DISCLOSURE.X831"));
          if (this.FltVal("DISCLOSURE.X714") != 0.0 || this.FltVal("DISCLOSURE.X985") != 0.0)
          {
            this.SetVal("NEWHUD.X715", "Include Origination Points");
            if (this.Val("NEWHUD.X1139") == "Y")
            {
              this.SetCurrentNum("NEWHUD.X1150", this.FltVal("DISCLOSURE.X714"));
              this.SetCurrentNum("NEWHUD.X1227", this.FltVal("DISCLOSURE.X985"));
              this.SetCurrentNum("NEWHUD.X1152", this.FltVal("DISCLOSURE.X833"));
            }
            else
            {
              this.SetCurrentNum("1061", this.FltVal("DISCLOSURE.X714"));
              this.SetCurrentNum("436", this.FltVal("DISCLOSURE.X985"));
              this.SetCurrentNum("NEWHUD.X788", this.FltVal("DISCLOSURE.X833"));
            }
          }
          else if (this.Val("1061") != string.Empty || this.Val("NEWHUD.X1139") == "Y" && this.Val("NEWHUD.X1150") != string.Empty)
          {
            this.SetVal("NEWHUD.X715", "");
            if (this.Val("NEWHUD.X1139") == "Y")
            {
              this.SetCurrentNum("NEWHUD.X1150", 0.0);
              this.SetCurrentNum("NEWHUD.X1152", 0.0);
              this.SetCurrentNum("NEWHUD.X1227", 0.0);
              this.SetCurrentNum("NEWHUD.X1178", 0.0);
            }
            else
            {
              this.SetCurrentNum("1061", 0.0);
              this.SetCurrentNum("436", 0.0);
              this.SetCurrentNum("NEWHUD.X788", 0.0);
            }
          }
          this.calObjs.VACal.CalcVARRRWS((string) null, (string) null);
          break;
        case "Loan Option 4":
          this.SetCurrentNum("4", this.FltVal("DISCLOSURE.X721"));
          this.SetCurrentNum("3", this.FltVal("DISCLOSURE.X722"));
          this.SetVal("KBYO.XD3", Utils.RemoveEndingZeros(this.FltVal("3").ToString()));
          if (this.Val("19") == "ConstructionOnly")
            this.SetCurrentNum("1677", this.FltVal("DISCLOSURE.X722"));
          this.SetCurrentNum("696", this.FltVal("DISCLOSURE.X723"));
          this.SetCurrentNum("388", this.FltVal("DISCLOSURE.X834"));
          this.SetCurrentNum("454", this.FltVal("DISCLOSURE.X835"));
          if (this.IsLocked("DISCLOSURE.X835") && !this.IsLocked("454"))
            this.loan.AddLock("454");
          this.SetCurrentNum("559", this.FltVal("DISCLOSURE.X836"));
          this.SetCurrentNum("L228", this.FltVal("DISCLOSURE.X837"));
          this.SetCurrentNum("L229", this.FltVal("DISCLOSURE.X838"));
          this.SetCurrentNum("1621", this.FltVal("DISCLOSURE.X839"));
          this.SetCurrentNum("1622", this.FltVal("DISCLOSURE.X840"));
          this.SetCurrentNum("367", this.FltVal("DISCLOSURE.X841"));
          this.SetCurrentNum("569", this.FltVal("DISCLOSURE.X842"));
          this.SetCurrentNum("389", this.FltVal("DISCLOSURE.X843"));
          this.SetCurrentNum("1620", this.FltVal("DISCLOSURE.X844"));
          this.SetCurrentNum("439", this.FltVal("DISCLOSURE.X845"));
          this.SetCurrentNum("572", this.FltVal("DISCLOSURE.X846"));
          this.SetCurrentNum("NEWHUD.X223", this.FltVal("DISCLOSURE.X847"));
          this.SetCurrentNum("NEWHUD.X224", this.FltVal("DISCLOSURE.X848"));
          this.SetCurrentNum("NEWHUD.X225", this.FltVal("DISCLOSURE.X849"));
          this.SetVal("154", this.Val("DISCLOSURE.X850"));
          this.SetCurrentNum("155", this.FltVal("DISCLOSURE.X851"));
          this.SetCurrentNum("200", this.FltVal("DISCLOSURE.X852"));
          this.SetVal("1627", this.Val("DISCLOSURE.X853"));
          this.SetCurrentNum("1625", this.FltVal("DISCLOSURE.X854"));
          this.SetCurrentNum("1626", this.FltVal("DISCLOSURE.X855"));
          this.SetVal("1838", this.Val("DISCLOSURE.X856"));
          this.SetCurrentNum("1839", this.FltVal("DISCLOSURE.X857"));
          this.SetCurrentNum("1840", this.FltVal("DISCLOSURE.X858"));
          this.SetVal("1841", this.Val("DISCLOSURE.X859"));
          this.SetCurrentNum("1842", this.FltVal("DISCLOSURE.X860"));
          this.SetCurrentNum("1843", this.FltVal("DISCLOSURE.X861"));
          this.SetVal("NEWHUD.X732", this.Val("DISCLOSURE.X862"));
          this.SetCurrentNum("NEWHUD.X733", this.FltVal("DISCLOSURE.X863"));
          this.SetCurrentNum("NEWHUD.X779", this.FltVal("DISCLOSURE.X864"));
          if (this.FltVal("DISCLOSURE.X726") != 0.0 || this.FltVal("DISCLOSURE.X989") != 0.0)
          {
            this.SetVal("NEWHUD.X715", "Include Origination Points");
            if (this.Val("NEWHUD.X1139") == "Y")
            {
              this.SetCurrentNum("NEWHUD.X1150", this.FltVal("DISCLOSURE.X726"));
              this.SetCurrentNum("NEWHUD.X1227", this.FltVal("DISCLOSURE.X989"));
              this.SetCurrentNum("NEWHUD.X1152", this.FltVal("DISCLOSURE.X866"));
            }
            else
            {
              this.SetCurrentNum("1061", this.FltVal("DISCLOSURE.X726"));
              this.SetCurrentNum("436", this.FltVal("DISCLOSURE.X989"));
              this.SetCurrentNum("NEWHUD.X788", this.FltVal("DISCLOSURE.X866"));
            }
          }
          else if (this.Val("1061") != string.Empty || this.Val("NEWHUD.X1139") == "Y" && this.Val("NEWHUD.X1150") != string.Empty)
          {
            this.SetVal("NEWHUD.X715", "");
            if (this.Val("NEWHUD.X1139") == "Y")
            {
              this.SetCurrentNum("NEWHUD.X1150", 0.0);
              this.SetCurrentNum("NEWHUD.X1152", 0.0);
              this.SetCurrentNum("NEWHUD.X1227", 0.0);
              this.SetCurrentNum("NEWHUD.X1178", 0.0);
            }
            else
            {
              this.SetCurrentNum("1061", 0.0);
              this.SetCurrentNum("436", 0.0);
              this.SetCurrentNum("NEWHUD.X788", 0.0);
            }
          }
          this.calObjs.VACal.CalcVARRRWS((string) null, (string) null);
          break;
      }
      if (!(str != string.Empty) || !(str != "Other Option"))
        return;
      if (this.FltVal("3119") != 0.0 && this.FltVal("1061") != 0.0)
      {
        for (int index = 1269; index <= 1274; ++index)
        {
          if (this.IsLocked(string.Concat((object) index)))
            this.loan.RemoveCurrentLock(string.Concat((object) index));
          if (this.IsLocked(string.Concat((object) (index + 344))))
            this.loan.RemoveCurrentLock(string.Concat((object) (index + 344)));
          if (this.Val(string.Concat((object) index)) != string.Empty)
            this.SetVal(string.Concat((object) index), "");
          if (this.Val(string.Concat((object) (index + 344))) != string.Empty)
            this.SetVal(string.Concat((object) (index + 344)), "");
        }
      }
      if (this.Val("3119") != string.Empty)
        this.SetCurrentNum("3119", 0.0);
      this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem801a((string) null, (string) null);
      this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem801e((string) null, (string) null);
      this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem801f((string) null, (string) null);
      this.calObjs.NewHudCal.CopyHUD2010ToGFE2010("388", (string) null, false);
      if (this.IsLocked("454"))
        this.calObjs.NewHudCal.CopyHUD2010ToGFE2010("454", (string) null, false);
      this.calObjs.NewHudCal.CopyHUD2010ToGFE2010("559", (string) null, false);
      this.calObjs.NewHudCal.CopyHUD2010ToGFE2010("L228", (string) null, false);
      this.calObjs.NewHudCal.CopyHUD2010ToGFE2010("L229", (string) null, false);
      this.calObjs.NewHudCal.CopyHUD2010ToGFE2010("1621", (string) null, false);
      this.calObjs.NewHudCal.CopyHUD2010ToGFE2010("1622", (string) null, false);
      this.calObjs.NewHudCal.CopyHUD2010ToGFE2010("367", (string) null, false);
      this.calObjs.NewHudCal.CopyHUD2010ToGFE2010("569", (string) null, false);
      this.calObjs.NewHudCal.CopyHUD2010ToGFE2010("389", (string) null, false);
      this.calObjs.NewHudCal.CopyHUD2010ToGFE2010("1620", (string) null, false);
      this.calObjs.NewHudCal.CopyHUD2010ToGFE2010("439", (string) null, false);
      this.calObjs.NewHudCal.CopyHUD2010ToGFE2010("572", (string) null, false);
      this.calObjs.NewHudCal.CopyHUD2010ToGFE2010("NEWHUD.X223", (string) null, false);
      this.calObjs.NewHudCal.CopyHUD2010ToGFE2010("NEWHUD.X224", (string) null, false);
      this.calObjs.NewHudCal.CopyHUD2010ToGFE2010("NEWHUD.X225", (string) null, false);
      this.calObjs.NewHudCal.CopyHUD2010ToGFE2010("154", (string) null, false);
      this.calObjs.NewHudCal.CopyHUD2010ToGFE2010("155", (string) null, false);
      this.calObjs.NewHudCal.CopyHUD2010ToGFE2010("200", (string) null, false);
      this.calObjs.NewHudCal.CopyHUD2010ToGFE2010("1627", (string) null, false);
      this.calObjs.NewHudCal.CopyHUD2010ToGFE2010("1625", (string) null, false);
      this.calObjs.NewHudCal.CopyHUD2010ToGFE2010("1626", (string) null, false);
      this.calObjs.NewHudCal.CopyHUD2010ToGFE2010("1838", (string) null, false);
      this.calObjs.NewHudCal.CopyHUD2010ToGFE2010("1839", (string) null, false);
      this.calObjs.NewHudCal.CopyHUD2010ToGFE2010("1840", (string) null, false);
      this.calObjs.NewHudCal.CopyHUD2010ToGFE2010("1841", (string) null, false);
      this.calObjs.NewHudCal.CopyHUD2010ToGFE2010("1842", (string) null, false);
      this.calObjs.NewHudCal.CopyHUD2010ToGFE2010("1843", (string) null, false);
      this.calObjs.NewHudCal.CopyHUD2010ToGFE2010("NEWHUD.X732", (string) null, false);
      this.calObjs.NewHudCal.CopyHUD2010ToGFE2010("NEWHUD.X733", (string) null, false);
      this.calObjs.NewHudCal.CopyHUD2010ToGFE2010("NEWHUD.X779", (string) null, false);
      this.calObjs.NewHudCal.CopyHUD2010ToGFE2010("NEWHUD.X713", (string) null, false);
      this.calObjs.NewHudCal.CopyHUD2010ToGFE2010("NEWHUD.X715", (string) null, false);
      this.calObjs.NewHudCal.CopyHUD2010ToGFE2010("1061", (string) null, false);
      this.calObjs.NewHudCal.CopyHUD2010ToGFE2010("436", (string) null, false);
      if (this.Val("NEWHUD.X1139") == "Y")
      {
        this.calObjs.NewHudCal.CopyHUD2010ToGFE2010("NEWHUD.X1150", "NEWHUD.X1150", false);
        this.calObjs.NewHudCal.CopyHUD2010ToGFE2010("NEWHUD.X1227", "NEWHUD.X1227", false);
      }
      else
      {
        this.calObjs.NewHudCal.CopyHUD2010ToGFE2010("1061", "1061", false);
        this.calObjs.NewHudCal.CopyHUD2010ToGFE2010("436", "436", false);
      }
      this.calObjs.NewHudCal.FormCal((string) null, (string) null);
      this.calObjs.LoansubCal.CalcLoansub((string) null, (string) null);
    }

    internal void CopyLoanToSafeHarbor(int optionNumber)
    {
      this.SetVal("DISCLOSURE.X689", this.Val("608"));
      switch (optionNumber)
      {
        case 1:
          this.SetCurrentNum("DISCLOSURE.X690", this.FltVal("4"));
          this.SetCurrentNum("DISCLOSURE.X691", this.FltVal("3"));
          this.SetCurrentNum("DISCLOSURE.X692", this.FltVal("696"));
          this.SetCurrentNum("DISCLOSURE.X735", this.FltVal("388"));
          this.SetCurrentNum("DISCLOSURE.X736", this.FltVal("454"));
          if (this.IsLocked("454") && !this.IsLocked("DISCLOSURE.X736"))
            this.loan.AddLock("DISCLOSURE.X736");
          this.SetCurrentNum("DISCLOSURE.X737", this.FltVal("559"));
          this.SetCurrentNum("DISCLOSURE.X738", this.FltVal("L228"));
          this.SetCurrentNum("DISCLOSURE.X739", this.FltVal("L229"));
          this.SetCurrentNum("DISCLOSURE.X740", this.FltVal("1621"));
          this.SetCurrentNum("DISCLOSURE.X741", this.FltVal("1622"));
          this.SetCurrentNum("DISCLOSURE.X742", this.FltVal("367"));
          this.SetCurrentNum("DISCLOSURE.X743", this.FltVal("569"));
          this.SetCurrentNum("DISCLOSURE.X744", this.FltVal("389"));
          this.SetCurrentNum("DISCLOSURE.X745", this.FltVal("1620"));
          this.SetCurrentNum("DISCLOSURE.X746", this.FltVal("439"));
          this.SetCurrentNum("DISCLOSURE.X747", this.FltVal("572"));
          this.SetCurrentNum("DISCLOSURE.X748", this.FltVal("NEWHUD.X223"));
          this.SetCurrentNum("DISCLOSURE.X749", this.FltVal("NEWHUD.X224"));
          this.SetCurrentNum("DISCLOSURE.X750", this.FltVal("NEWHUD.X225"));
          this.SetCurrentNum("DISCLOSURE.X751", this.FltVal("154"));
          this.SetCurrentNum("DISCLOSURE.X752", this.FltVal("155"));
          this.SetCurrentNum("DISCLOSURE.X753", this.FltVal("200"));
          this.SetCurrentNum("DISCLOSURE.X754", this.FltVal("1627"));
          this.SetCurrentNum("DISCLOSURE.X755", this.FltVal("1625"));
          this.SetCurrentNum("DISCLOSURE.X756", this.FltVal("1626"));
          this.SetCurrentNum("DISCLOSURE.X757", this.FltVal("1838"));
          this.SetCurrentNum("DISCLOSURE.X758", this.FltVal("1839"));
          this.SetCurrentNum("DISCLOSURE.X759", this.FltVal("1840"));
          this.SetCurrentNum("DISCLOSURE.X760", this.FltVal("1841"));
          this.SetCurrentNum("DISCLOSURE.X761", this.FltVal("1842"));
          this.SetCurrentNum("DISCLOSURE.X762", this.FltVal("1843"));
          this.SetCurrentNum("DISCLOSURE.X763", this.FltVal("NEWHUD.X732"));
          this.SetCurrentNum("DISCLOSURE.X764", this.FltVal("NEWHUD.X733"));
          this.SetCurrentNum("DISCLOSURE.X765", this.FltVal("NEWHUD.X779"));
          if (this.Val("NEWHUD.X1139") == "Y")
          {
            this.SetCurrentNum("DISCLOSURE.X695", this.FltVal("NEWHUD.X1150"));
            this.SetCurrentNum("DISCLOSURE.X977", this.FltVal("NEWHUD.X1227"));
            this.SetCurrentNum("DISCLOSURE.X767", this.FltVal("NEWHUD.X1152"));
            break;
          }
          this.SetCurrentNum("DISCLOSURE.X695", this.FltVal("1061"));
          this.SetCurrentNum("DISCLOSURE.X977", this.FltVal("436"));
          this.SetCurrentNum("DISCLOSURE.X767", this.FltVal("NEWHUD.X788"));
          break;
        case 2:
          this.SetCurrentNum("DISCLOSURE.X702", this.FltVal("4"));
          this.SetCurrentNum("DISCLOSURE.X703", this.FltVal("3"));
          this.SetCurrentNum("DISCLOSURE.X704", this.FltVal("696"));
          this.SetCurrentNum("DISCLOSURE.X768", this.FltVal("388"));
          this.SetCurrentNum("DISCLOSURE.X769", this.FltVal("454"));
          if (this.IsLocked("454") && !this.IsLocked("DISCLOSURE.X769"))
            this.loan.AddLock("DISCLOSURE.X769");
          this.SetCurrentNum("DISCLOSURE.X770", this.FltVal("559"));
          this.SetCurrentNum("DISCLOSURE.X771", this.FltVal("L228"));
          this.SetCurrentNum("DISCLOSURE.X772", this.FltVal("L229"));
          this.SetCurrentNum("DISCLOSURE.X773", this.FltVal("1621"));
          this.SetCurrentNum("DISCLOSURE.X774", this.FltVal("1622"));
          this.SetCurrentNum("DISCLOSURE.X775", this.FltVal("367"));
          this.SetCurrentNum("DISCLOSURE.X776", this.FltVal("569"));
          this.SetCurrentNum("DISCLOSURE.X777", this.FltVal("389"));
          this.SetCurrentNum("DISCLOSURE.X778", this.FltVal("1620"));
          this.SetCurrentNum("DISCLOSURE.X779", this.FltVal("439"));
          this.SetCurrentNum("DISCLOSURE.X780", this.FltVal("572"));
          this.SetCurrentNum("DISCLOSURE.X781", this.FltVal("NEWHUD.X223"));
          this.SetCurrentNum("DISCLOSURE.X782", this.FltVal("NEWHUD.X224"));
          this.SetCurrentNum("DISCLOSURE.X783", this.FltVal("NEWHUD.X225"));
          this.SetCurrentNum("DISCLOSURE.X784", this.FltVal("154"));
          this.SetCurrentNum("DISCLOSURE.X785", this.FltVal("155"));
          this.SetCurrentNum("DISCLOSURE.X786", this.FltVal("200"));
          this.SetCurrentNum("DISCLOSURE.X787", this.FltVal("1627"));
          this.SetCurrentNum("DISCLOSURE.X788", this.FltVal("1625"));
          this.SetCurrentNum("DISCLOSURE.X789", this.FltVal("1626"));
          this.SetCurrentNum("DISCLOSURE.X790", this.FltVal("1838"));
          this.SetCurrentNum("DISCLOSURE.X791", this.FltVal("1839"));
          this.SetCurrentNum("DISCLOSURE.X792", this.FltVal("1840"));
          this.SetCurrentNum("DISCLOSURE.X793", this.FltVal("1841"));
          this.SetCurrentNum("DISCLOSURE.X794", this.FltVal("1842"));
          this.SetCurrentNum("DISCLOSURE.X795", this.FltVal("1843"));
          this.SetCurrentNum("DISCLOSURE.X796", this.FltVal("NEWHUD.X732"));
          this.SetCurrentNum("DISCLOSURE.X797", this.FltVal("NEWHUD.X733"));
          this.SetCurrentNum("DISCLOSURE.X798", this.FltVal("NEWHUD.X779"));
          this.SetCurrentNum("DISCLOSURE.X707", this.FltVal("1061"));
          this.SetCurrentNum("DISCLOSURE.X978", this.FltVal("1061") / 100.0 * this.FltVal("2"));
          if (this.Val("NEWHUD.X1139") == "Y")
          {
            this.SetCurrentNum("DISCLOSURE.X707", this.FltVal("NEWHUD.X1150"));
            this.SetCurrentNum("DISCLOSURE.X981", this.FltVal("NEWHUD.X1227"));
            this.SetCurrentNum("DISCLOSURE.X800", this.FltVal("NEWHUD.X1152"));
            break;
          }
          this.SetCurrentNum("DISCLOSURE.X707", this.FltVal("1061"));
          this.SetCurrentNum("DISCLOSURE.X981", this.FltVal("436"));
          this.SetCurrentNum("DISCLOSURE.X800", this.FltVal("NEWHUD.X788"));
          break;
        case 3:
          this.SetCurrentNum("DISCLOSURE.X709", this.FltVal("4"));
          this.SetCurrentNum("DISCLOSURE.X710", this.FltVal("3"));
          this.SetCurrentNum("DISCLOSURE.X711", this.FltVal("696"));
          this.SetCurrentNum("DISCLOSURE.X801", this.FltVal("388"));
          this.SetCurrentNum("DISCLOSURE.X802", this.FltVal("454"));
          if (this.IsLocked("454") && !this.IsLocked("DISCLOSURE.X802"))
            this.loan.AddLock("DISCLOSURE.X802");
          this.SetCurrentNum("DISCLOSURE.X803", this.FltVal("559"));
          this.SetCurrentNum("DISCLOSURE.X804", this.FltVal("L228"));
          this.SetCurrentNum("DISCLOSURE.X805", this.FltVal("L229"));
          this.SetCurrentNum("DISCLOSURE.X806", this.FltVal("1621"));
          this.SetCurrentNum("DISCLOSURE.X807", this.FltVal("1622"));
          this.SetCurrentNum("DISCLOSURE.X808", this.FltVal("367"));
          this.SetCurrentNum("DISCLOSURE.X809", this.FltVal("569"));
          this.SetCurrentNum("DISCLOSURE.X810", this.FltVal("389"));
          this.SetCurrentNum("DISCLOSURE.X811", this.FltVal("1620"));
          this.SetCurrentNum("DISCLOSURE.X812", this.FltVal("439"));
          this.SetCurrentNum("DISCLOSURE.X813", this.FltVal("572"));
          this.SetCurrentNum("DISCLOSURE.X814", this.FltVal("NEWHUD.X223"));
          this.SetCurrentNum("DISCLOSURE.X815", this.FltVal("NEWHUD.X224"));
          this.SetCurrentNum("DISCLOSURE.X816", this.FltVal("NEWHUD.X225"));
          this.SetCurrentNum("DISCLOSURE.X817", this.FltVal("154"));
          this.SetCurrentNum("DISCLOSURE.X818", this.FltVal("155"));
          this.SetCurrentNum("DISCLOSURE.X819", this.FltVal("200"));
          this.SetCurrentNum("DISCLOSURE.X820", this.FltVal("1627"));
          this.SetCurrentNum("DISCLOSURE.X821", this.FltVal("1625"));
          this.SetCurrentNum("DISCLOSURE.X822", this.FltVal("1626"));
          this.SetCurrentNum("DISCLOSURE.X823", this.FltVal("1838"));
          this.SetCurrentNum("DISCLOSURE.X824", this.FltVal("1839"));
          this.SetCurrentNum("DISCLOSURE.X825", this.FltVal("1840"));
          this.SetCurrentNum("DISCLOSURE.X826", this.FltVal("1841"));
          this.SetCurrentNum("DISCLOSURE.X827", this.FltVal("1842"));
          this.SetCurrentNum("DISCLOSURE.X828", this.FltVal("1843"));
          this.SetCurrentNum("DISCLOSURE.X829", this.FltVal("NEWHUD.X732"));
          this.SetCurrentNum("DISCLOSURE.X830", this.FltVal("NEWHUD.X733"));
          this.SetCurrentNum("DISCLOSURE.X831", this.FltVal("NEWHUD.X779"));
          if (this.Val("NEWHUD.X1139") == "Y")
          {
            this.SetCurrentNum("DISCLOSURE.X714", this.FltVal("NEWHUD.X1150"));
            this.SetCurrentNum("DISCLOSURE.X985", this.FltVal("NEWHUD.X1227"));
            this.SetCurrentNum("DISCLOSURE.X833", this.FltVal("NEWHUD.X1152"));
            break;
          }
          this.SetCurrentNum("DISCLOSURE.X714", this.FltVal("1061"));
          this.SetCurrentNum("DISCLOSURE.X985", this.FltVal("436"));
          this.SetCurrentNum("DISCLOSURE.X833", this.FltVal("NEWHUD.X788"));
          break;
        case 4:
          this.SetCurrentNum("DISCLOSURE.X721", this.FltVal("4"));
          this.SetCurrentNum("DISCLOSURE.X722", this.FltVal("3"));
          this.SetCurrentNum("DISCLOSURE.X723", this.FltVal("696"));
          this.SetCurrentNum("DISCLOSURE.X834", this.FltVal("388"));
          this.SetCurrentNum("DISCLOSURE.X835", this.FltVal("454"));
          if (this.IsLocked("454") && !this.IsLocked("DISCLOSURE.X835"))
            this.loan.AddLock("DISCLOSURE.X835");
          this.SetCurrentNum("DISCLOSURE.X836", this.FltVal("559"));
          this.SetCurrentNum("DISCLOSURE.X837", this.FltVal("L228"));
          this.SetCurrentNum("DISCLOSURE.X838", this.FltVal("L229"));
          this.SetCurrentNum("DISCLOSURE.X839", this.FltVal("1621"));
          this.SetCurrentNum("DISCLOSURE.X840", this.FltVal("1622"));
          this.SetCurrentNum("DISCLOSURE.X841", this.FltVal("367"));
          this.SetCurrentNum("DISCLOSURE.X842", this.FltVal("569"));
          this.SetCurrentNum("DISCLOSURE.X843", this.FltVal("389"));
          this.SetCurrentNum("DISCLOSURE.X844", this.FltVal("1620"));
          this.SetCurrentNum("DISCLOSURE.X845", this.FltVal("439"));
          this.SetCurrentNum("DISCLOSURE.X846", this.FltVal("572"));
          this.SetCurrentNum("DISCLOSURE.X847", this.FltVal("NEWHUD.X223"));
          this.SetCurrentNum("DISCLOSURE.X848", this.FltVal("NEWHUD.X224"));
          this.SetCurrentNum("DISCLOSURE.X849", this.FltVal("NEWHUD.X225"));
          this.SetCurrentNum("DISCLOSURE.X850", this.FltVal("154"));
          this.SetCurrentNum("DISCLOSURE.X851", this.FltVal("155"));
          this.SetCurrentNum("DISCLOSURE.X852", this.FltVal("200"));
          this.SetCurrentNum("DISCLOSURE.X853", this.FltVal("1627"));
          this.SetCurrentNum("DISCLOSURE.X854", this.FltVal("1625"));
          this.SetCurrentNum("DISCLOSURE.X855", this.FltVal("1626"));
          this.SetCurrentNum("DISCLOSURE.X856", this.FltVal("1838"));
          this.SetCurrentNum("DISCLOSURE.X857", this.FltVal("1839"));
          this.SetCurrentNum("DISCLOSURE.X858", this.FltVal("1840"));
          this.SetCurrentNum("DISCLOSURE.X859", this.FltVal("1841"));
          this.SetCurrentNum("DISCLOSURE.X860", this.FltVal("1842"));
          this.SetCurrentNum("DISCLOSURE.X861", this.FltVal("1843"));
          this.SetCurrentNum("DISCLOSURE.X862", this.FltVal("NEWHUD.X732"));
          this.SetCurrentNum("DISCLOSURE.X863", this.FltVal("NEWHUD.X733"));
          this.SetCurrentNum("DISCLOSURE.X864", this.FltVal("NEWHUD.X779"));
          if (this.Val("NEWHUD.X1139") == "Y")
          {
            this.SetCurrentNum("DISCLOSURE.X726", this.FltVal("NEWHUD.X1150"));
            this.SetCurrentNum("DISCLOSURE.X989", this.FltVal("NEWHUD.X1227"));
            this.SetCurrentNum("DISCLOSURE.X866", this.FltVal("NEWHUD.X1152"));
            break;
          }
          this.SetCurrentNum("DISCLOSURE.X726", this.FltVal("1061"));
          this.SetCurrentNum("DISCLOSURE.X989", this.FltVal("436"));
          this.SetCurrentNum("DISCLOSURE.X866", this.FltVal("NEWHUD.X788"));
          break;
      }
      this.calculateSafeHarbor((string) null, (string) null);
    }

    private void calculateNetTangibleBenefit(string id, string val)
    {
      this.SetCurrentNum("NTB.X29", this.FltVal("NTB.X20") - this.FltVal("5"));
      this.SetCurrentNum("NTB.X27", this.FltVal("NTB.X22") - this.FltVal("NEWHUD.X217"));
      this.SetCurrentNum("NTB.X28", this.FltVal("NTB.X21") - this.FltVal("912"));
      if (id == "NTB.X13" && val != "Y")
      {
        this.SetVal("NTB.X6", "");
        this.SetVal("NTB.X14", "");
        this.SetVal("NTB.X26", "");
      }
      else if (id == "675" && val != "Y")
      {
        this.SetVal("RE88395.X316", "");
        this.SetVal("1948", "");
        this.SetVal("2830", "");
      }
      else if (id == "NTB.X30" && val != "Y")
      {
        this.SetVal("NTB.X31", "");
      }
      else
      {
        if (!(id == "NTB.X35") || !(val != "Y"))
          return;
        this.SetVal("NTB.X36", "");
      }
    }

    private void calculateIRS1098(string id, string val)
    {
      ServicingSummaryViewModel summaryViewModel = new ServicingSummaryViewModel(this.loan.GetServicingTransactions());
      DateTime date = Utils.ParseDate((object) this.Val("748"));
      int num = 0;
      if (date != DateTime.MinValue)
        num = date.Year;
      if (num > 0)
      {
        summaryViewModel.GenerateAnnualSummary(num);
        this.SetVal("3614", num.ToString());
        this.SetCurrentNum("3615", Utils.ParseDouble((object) this.Val("334")) + summaryViewModel.Interest);
        this.SetCurrentNum("3842", summaryViewModel.Escrow.MortgageInsurance);
      }
      int maxPaymentYear = summaryViewModel.GetMaxPaymentYear(num, DateTime.Now.Year);
      if (num < maxPaymentYear && maxPaymentYear > 0)
      {
        summaryViewModel.GenerateAnnualSummary(maxPaymentYear);
        this.SetVal("3616", maxPaymentYear.ToString());
        this.SetCurrentNum("3617", summaryViewModel.Interest);
        this.SetCurrentNum("3843", summaryViewModel.Escrow.MortgageInsurance);
      }
      else
      {
        this.SetVal("3616", "");
        this.SetVal("3617", "");
        this.SetVal("3843", "");
      }
      if (string.IsNullOrEmpty(this.Val("3616")) || !string.IsNullOrEmpty(this.Val("3618")))
        return;
      this.SetVal("3618", "0");
    }

    internal void UpdateLOCompensation(string id, string val)
    {
      if (this.sessionObjects.StartupInfo.ServerLicense.Edition != EncompassEdition.Banker)
        return;
      string strA = this.Val("2626");
      bool flag1 = false;
      bool flag2 = false;
      if (string.Compare(strA, "Banked - Retail", true) == 0)
        flag1 = true;
      else if (string.Compare(strA, "Brokered", true) == 0)
      {
        flag2 = true;
      }
      else
      {
        if (string.Compare(strA, "Correspondent", true) == 0)
          throw new Exception("Since this loan is assigned to the Correspondent loan channel (field ID 2626), LO compensation plans cannot be applied.");
        if (string.Compare(strA, "Banked - Wholesale", true) != 0)
          throw new Exception("You must assign this loan to a loan Channel (field ID 2626) before you can apply an LO compensation plan.");
      }
      LoanCompDefaultPlan loanCompDefaultPlan = this.configInfo == null || this.configInfo.LoanCompDefaultPlan == null ? this.sessionObjects.ConfigurationManager.GetDefaultLoanCompPlan() : this.configInfo.LoanCompDefaultPlan;
      string str1 = this.Val("LCP.X1");
      if (str1 == string.Empty)
      {
        if (loanCompDefaultPlan == null || loanCompDefaultPlan.PaidByString == string.Empty)
          this.SetVal("LCP.X1", "Lender Paid");
        else
          this.SetVal("LCP.X1", loanCompDefaultPlan.PaidByString);
        str1 = this.Val("LCP.X1");
        this.SetVal("4463", this.Val("LCP.X1"));
      }
      switch (str1)
      {
        case "Borrower Paid":
          throw new Exception("Since loan originator compensation is being paid by the borrower (field ID LCP.X1), an LO compensation plan will not be applied to this loan.");
        case "Exempt":
          throw new Exception("Since loan originator compensation is exempt (field ID LCP.X1) for this loan, an LO compensation plan will not be applied.");
        default:
          string str2 = this.Val("1172");
          if (loanCompDefaultPlan == null || loanCompDefaultPlan.TriggerField == string.Empty)
            throw new Exception("Your system administrator did not specify a Trigger Field (field ID LCP.X4) when creating this plan. The plan cannot be applied without a Trigger Field.");
          DateTime date1 = Utils.ParseDate((object) this.Val(loanCompDefaultPlan.TriggerField));
          if (date1.Date == DateTime.MinValue.Date)
            throw new Exception("A valid Trigger Field Date (field ID LCP.X5) for this plan must be provided. Locate the field (" + loanCompDefaultPlan.TriggerField + ") in Encompass and enter a valid date.");
          if (loanCompDefaultPlan != null)
          {
            if ((loanCompDefaultPlan.LoansExempt & 1) == 1 && str2 == "HELOC")
            {
              if (!((id ?? "") == "") && !(id == "LCP.X1") && !this.LOCompensationIsApplied(true, true, true))
                break;
              throw new Exception("Due to the settings configured by your system administrator, LO compensation plans cannot be applied to this loan.  The administrator has specified that plans may not be applied to HELOC (field ID 1172) loans.");
            }
            if ((loanCompDefaultPlan.LoansExempt & 4) == 4 && this.Val("1811") == "Investor")
            {
              if (!((id ?? "") == "") && !(id == "LCP.X1") && !this.LOCompensationIsApplied(true, true, true))
                break;
              throw new Exception("Due to the settings configured by your system administrator, LO compensation plans cannot be applied to this loan.  The administrator has specified that plans may not be applied to Investment Property (field ID 1811) loans.");
            }
          }
          LoanCompPlan loanCompPlan1 = (LoanCompPlan) null;
          LoanCompPlan loanCompPlan2 = (LoanCompPlan) null;
          if (flag2)
          {
            loanCompPlan1 = this.getLoanCompPlanByBrokerID(true, date1);
            if (loanCompPlan1 == null && strA != "Brokered")
              throw new Exception("An LO compensation Plan could not be found for " + this.Val("LCP.X2") + ". In order to update the LO compensation here, your system administrator must first assign an LO compensation plan to " + this.Val("LCP.X2") + ".");
          }
          string userID = this.Val("LOID");
          if (flag1 | flag2 && userID == string.Empty)
          {
            string str3 = this.Val("317");
            if (str3 == string.Empty)
              throw new Exception("You must assign a loan officer to this loan (field ID 317) before you can apply an LO compensation plan.");
            throw new Exception("An LO compensation plan could not be found for " + str3 + ". In order to update the LO compensation here, your system administrator must first assign an LO compensation plan to " + str3 + ".");
          }
          if (flag1 | flag2)
          {
            loanCompPlan2 = this.sessionObjects.ConfigurationManager.GetCurrentComPlanforUser(userID, date1.Date);
            this.SetVal("LCP.X21", this.Val("317"));
            if (loanCompPlan1 == null && loanCompPlan2 == null && strA == "Brokered")
              throw new Exception("An LO compensation Plan could not be found for " + this.Val("LCP.X2") + ". In order to update the LO compensation here, your system administrator must first assign an LO compensation plan to " + this.Val("LCP.X2") + ".");
          }
          else
          {
            loanCompPlan1 = this.getLoanCompPlanByBrokerID(false, date1);
            if (loanCompPlan1 == null)
              throw new Exception("An LO compensation Plan could not be found for " + this.Val("LCP.X2") + ". In order to update the LO compensation here, your system administrator must first assign an LO compensation plan to " + this.Val("LCP.X2") + ".");
          }
          if (!flag1 | flag2 && loanCompPlan1 == null && strA != "Brokered")
            throw new Exception("Based on the Trigger Field Date (field ID LCP.X5) provided, no LO compensation plans can be applied for broker " + this.Val("LCP.X2") + ". The Trigger Field Date must occur during a valid LO compensation plan term configured by your system administrator.");
          if (flag1 && loanCompPlan2 == null)
            throw new Exception("Based on the Trigger Field Date (field ID LCP.X5) provided, no LO compensation plans can be applied for loan officer " + this.Val("317") + ". The Trigger Field Date must occur during a valid LO compensation plan term configured by your system administrator.");
          this.SetVal("LCP.X4", loanCompDefaultPlan.TriggerField);
          DateTime date2 = date1.Date;
          DateTime dateTime = DateTime.MinValue;
          DateTime date3 = dateTime.Date;
          this.SetVal("LCP.X5", date2 == date3 ? "" : date1.ToString("MM/dd/yyyy"));
          if (loanCompPlan1 != null)
          {
            for (int index = 6; index <= 10; ++index)
            {
              if (this.IsLocked("LCP.X" + (object) index))
                this.RemoveCurrentLock("LCP.X" + (object) index);
            }
            this.SetVal("LCP.X3", loanCompPlan1.Name);
            this.SetCurrentNum("LCP.X6", Utils.ParseDouble((object) loanCompPlan1.PercentAmt));
            this.SetCurrentNum("LCP.X7", Utils.ParseDouble((object) loanCompPlan1.DollarAmount));
            this.SetCurrentNum("LCP.X9", Utils.ParseDouble((object) loanCompPlan1.MinDollarAmount));
            this.SetCurrentNum("LCP.X10", Utils.ParseDouble((object) loanCompPlan1.MaxDollarAmount));
            this.SetVal("LCP.X16", loanCompPlan1.RoundingMethod == 2 ? "To Nearest Dollar" : "");
            this.SetVal("LCP.X17", loanCompPlan1.PercentAmtBase == 1 ? "Total Loan" : (loanCompPlan1.PercentAmtBase == 2 ? "Base Loan" : ""));
            this.SetVal("LCP.X11", "");
            this.SetVal("LCP.X12", "");
          }
          if (loanCompPlan2 != null)
          {
            for (int index = 25; index <= 29; ++index)
            {
              if (index != 27 && this.IsLocked("LCP.X" + (object) index))
                this.RemoveCurrentLock("LCP.X" + (object) index);
            }
            this.SetVal("LCP.X22", loanCompPlan2.Name);
            this.SetVal("LCP.X23", loanCompPlan2.RoundingMethod == 2 ? "To Nearest Dollar" : "");
            this.SetVal("LCP.X24", loanCompPlan2.PercentAmtBase == 1 ? "Total Loan" : (loanCompPlan2.PercentAmtBase == 2 ? "Base Loan" : ""));
            this.SetCurrentNum("LCP.X25", Utils.ParseDouble((object) loanCompPlan2.PercentAmt));
            this.SetCurrentNum("LCP.X26", Utils.ParseDouble((object) loanCompPlan2.DollarAmount));
            this.SetCurrentNum("LCP.X28", Utils.ParseDouble((object) loanCompPlan2.MinDollarAmount));
            this.SetCurrentNum("LCP.X29", Utils.ParseDouble((object) loanCompPlan2.MaxDollarAmount));
            for (int index = 30; index <= 36; ++index)
              this.SetVal("LCP.X" + (object) index, "");
          }
          dateTime = DateTime.Now;
          this.SetVal("LCP.X19", dateTime.ToString("MM/dd/yyyy h:mm:ss tt"));
          dateTime = DateTime.Now;
          dateTime = dateTime.Date;
          this.SetVal("LCP.X20", dateTime.ToString("MM/dd/yyyy"));
          this.calculateLOCompensation((string) null, (string) null);
          if (strA == "Brokered")
          {
            if (loanCompPlan1 == null)
              throw new Exception("An LO compensation Plan could not be found for broker " + this.Val("LCP.X2") + ". In order to update the LO compensation here, your system administrator must first assign an LO compensation plan to broker " + this.Val("LCP.X2") + ".");
            if (loanCompPlan2 == null)
              throw new Exception("An LO compensation plan could not be found for loan officer " + this.Val("LCP.X21") + ". In order to update the LO compensation here, your system administrator must first assign an LO compensation plan to loan officer " + this.Val("LCP.X21") + ".");
          }
          if (!flag2 || loanCompPlan2 != null || !this.LOCompensationIsApplied(false, true, false))
            break;
          throw new Exception("Based on the Trigger Field Date (field ID LCP.X5) provided, no LO compensation plans can be applied for loan officer " + this.Val("317") + ". The Trigger Field Date must occur during a valid LO compensation plan term configured by your system administrator.");
      }
    }

    private LoanCompPlan getLoanCompPlanByBrokerID(bool forLender, DateTime triggerDate)
    {
      LoanCompPlan compPlanByBrokerId = (LoanCompPlan) null;
      string brokerID = this.Val("LCP.X18");
      string legalName = this.Val("LCP.X2");
      if (brokerID != string.Empty)
      {
        compPlanByBrokerId = this.sessionObjects.ConfigurationManager.GetCurrentComPlanforBrokerByID(forLender, brokerID, triggerDate.Date);
        if (compPlanByBrokerId == null && this.sessionObjects.ConfigurationManager.GetByoid(forLender, Utils.ParseInt((object) brokerID)) != null)
          return (LoanCompPlan) null;
      }
      if (brokerID == string.Empty || compPlanByBrokerId == null)
      {
        if (legalName == string.Empty)
          throw new Exception("You must select a broker in this loan (field ID LCP.X2) before you can apply an LO compensation plan.");
        List<ExternalOriginatorManagementData> externalOrganizations = this.sessionObjects.ConfigurationManager.GetExternalOrganizations(forLender, ExternalOriginatorEntityType.None, legalName, (string) null, (string) null, (string) null, true, (string) null, false, (string) null);
        if (externalOrganizations == null || externalOrganizations.Count == 0)
          return (LoanCompPlan) null;
        this.SetVal("LCP.X18", string.Concat((object) externalOrganizations[0].oid));
        compPlanByBrokerId = this.sessionObjects.ConfigurationManager.GetCurrentComPlanforBrokerByID(forLender, string.Concat((object) externalOrganizations[0].oid), triggerDate.Date);
      }
      return compPlanByBrokerId;
    }

    internal void ClearLOCompensation(
      bool clearBrokerFields,
      bool clearLOFields,
      bool clearLastAppliedFields,
      bool includeNameField)
    {
      if (this.sessionObjects.StartupInfo.ServerLicense.Edition != EncompassEdition.Banker)
        return;
      if (clearBrokerFields)
      {
        for (int index = 6; index <= 10; ++index)
        {
          if (this.IsLocked("LCP.X" + (object) index))
            this.RemoveCurrentLock("LCP.X" + (object) index);
        }
        for (int index = 3; index <= 17; ++index)
        {
          if (index != 4 && index != 5 && index != 15)
            this.SetVal("LCP.X" + (object) index, "");
        }
        if (includeNameField)
        {
          this.SetVal("LCP.X2", "");
          this.SetVal("LCP.X18", "");
        }
      }
      if (clearLOFields)
      {
        for (int index = 25; index <= 29; ++index)
        {
          if (index != 27 && this.IsLocked("LCP.X" + (object) index))
            this.RemoveCurrentLock("LCP.X" + (object) index);
        }
        for (int index = 22; index <= 36; ++index)
          this.SetVal("LCP.X" + (object) index, "");
        if (includeNameField)
        {
          this.SetVal("LCP.X21", "");
          this.SetVal("LCP.X38", "");
        }
      }
      if (clearLastAppliedFields)
      {
        this.SetVal("LCP.X19", "");
        this.SetVal("LCP.X20", "");
      }
      this.calculateLOCompensation((string) null, (string) null);
      if (!(this.Val("NEWHUD.X1718") == "Y"))
        return;
      this.Clear801LOCompensationFields(true);
    }

    internal bool LOCompensationIsApplied(
      bool checkBroker,
      bool checkLoanOfficer,
      bool checkNameField)
    {
      string empty = string.Empty;
      if (checkBroker)
      {
        for (int index = 3; index <= 17; ++index)
        {
          if (index != 4 && index != 5 && index != 15)
          {
            string str = this.Val("LCP.X" + (object) index);
            if (str != string.Empty && str != "//")
              return true;
          }
        }
        if (checkNameField && this.Val("LCP.X2") != string.Empty)
          return true;
      }
      if (checkLoanOfficer)
      {
        for (int index = 22; index <= 36; ++index)
        {
          string str = this.Val("LCP.X" + (object) index);
          if (str != string.Empty && str != "//")
            return true;
        }
        if (checkNameField && this.Val("LCP.X21") != string.Empty)
          return true;
      }
      return false;
    }

    private void calculateLOCompensation(string id, string val)
    {
      if (this.sessionObjects.StartupInfo.ServerLicense.Edition != EncompassEdition.Banker)
        return;
      bool flag1 = this.Val("LCP.X16") == "To Nearest Dollar";
      double num1 = this.Val("LCP.X17") == "Base Loan" ? this.FltVal("1109") : this.FltVal("2");
      double num2 = Utils.ArithmeticRounding(Utils.ArithmeticRounding(this.FltVal("LCP.X6") / 100.0 * num1 + this.FltVal("LCP.X7"), 2), flag1 ? 0 : 2);
      double num3 = this.FltVal("LCP.X9");
      if (num3 > 0.0 && num2 < num3)
        num2 = num3;
      double num4 = this.FltVal("LCP.X10");
      if (num4 > 0.0 && num2 > num4)
        num2 = num4;
      this.SetCurrentNum("LCP.X8", num2);
      this.SetCurrentNum("LCP.X13", Utils.ArithmeticRounding(Utils.ArithmeticRounding(this.FltVal("LCP.X11") / 100.0 * num1 + this.FltVal("LCP.X12"), 2), flag1 ? 0 : 2));
      double num5 = Utils.ArithmeticRounding(this.FltVal("LCP.X8") - this.FltVal("LCP.X13"), flag1 ? 0 : 2);
      if (num5 != 0.0)
        this.SetCurrentNum("LCP.X14", num5);
      else if (this.Val("LCP.X8") != string.Empty || this.Val("LCP.X13") != string.Empty)
        this.SetVal("LCP.X14", "0.00");
      else
        this.SetVal("LCP.X14", "");
      bool flag2 = this.Val("LCP.X23") == "To Nearest Dollar";
      double num6 = this.Val("LCP.X24") == "Base Loan" ? this.FltVal("1109") : this.FltVal("2");
      double num7 = Utils.ArithmeticRounding(Utils.ArithmeticRounding(this.FltVal("LCP.X25") / 100.0 * num6 + this.FltVal("LCP.X26"), 2), flag2 ? 0 : 2);
      double num8 = this.FltVal("LCP.X28");
      if (num8 > 0.0 && num7 < num8)
        num7 = num8;
      double num9 = this.FltVal("LCP.X29");
      if (num9 > 0.0 && num7 > num9)
        num7 = num9;
      this.SetCurrentNum("LCP.X27", num7);
      this.SetCurrentNum("LCP.X32", Utils.ArithmeticRounding(Utils.ArithmeticRounding(this.FltVal("LCP.X30") / 100.0 * num6 + this.FltVal("LCP.X31"), 2), flag2 ? 0 : 2));
      this.SetCurrentNum("LCP.X35", Utils.ArithmeticRounding(Utils.ArithmeticRounding(this.FltVal("LCP.X33") / 100.0 * num6 + this.FltVal("LCP.X34"), 2), flag2 ? 0 : 2));
      double num10 = Utils.ArithmeticRounding(this.FltVal("LCP.X27") + this.FltVal("LCP.X32") - this.FltVal("LCP.X35"), flag2 ? 0 : 2);
      if (num10 != 0.0)
        this.SetCurrentNum("LCP.X36", num10);
      else if (this.Val("LCP.X27") != string.Empty || this.Val("LCP.X35") != string.Empty)
        this.SetVal("LCP.X36", "0.00");
      else
        this.SetVal("LCP.X36", "");
      if (!(this.Val("NEWHUD.X1718") == "Y"))
        return;
      this.CopyLOCompTo2010Itemization(false);
    }

    internal void Clear801LOCompensationFields(bool callNewHudFormCalculation)
    {
      if (this.sessionObjects.StartupInfo.ServerLicense.Edition != EncompassEdition.Banker)
        return;
      this.SetVal("NEWHUD.X223", "");
      this.SetVal("NEWHUD.X224", "");
      this.SetVal("NEWHUD.X225", "");
      this.SetVal("NEWHUD.X229", "");
      this.SetVal("NEWHUD.X230", "");
      this.SetVal("NEWHUD.X227", "");
      this.SetVal("NEWHUD.X231", "");
      this.SetVal("NEWHUD.X232", "");
      this.SetVal("PTC.X6", "");
      this.SetVal("PTC.X84", "");
      this.SetVal("PTC.X162", "");
      this.SetVal("POPT.X6", "");
      this.SetVal("POPT.X84", "");
      this.SetVal("POPT.X162", "");
      this.SetVal("NEWHUD.X1141", "");
      this.SetVal("NEWHUD.X1225", "");
      this.SetVal("NEWHUD.X1142", "");
      this.SetVal("NEWHUD.X1167", "");
      this.SetVal("NEWHUD.X1168", "");
      this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem801f((string) null, (string) null);
      if (!callNewHudFormCalculation)
        return;
      this.calObjs.NewHudCal.FormCal((string) null, (string) null);
    }

    internal void CopyLOCompTo2010Itemization(bool callNewHudFormCalculation)
    {
      if (this.sessionObjects.StartupInfo.ServerLicense.Edition != EncompassEdition.Banker)
        return;
      if (this.Val("NEWHUD.X1139") != "Y")
        this.SetVal("NEWHUD.X1139", "Y");
      bool flag = this.Val("LCP.X17") != "Base Loan";
      double num1 = this.FltVal("2");
      double num2 = this.FltVal("LCP.X14");
      double num3 = Utils.ArithmeticRounding(this.FltVal("LCP.X6"), 3);
      if (num1 != 0.0 && Utils.ArithmeticRounding(Utils.ArithmeticRounding(this.FltVal("LCP.X6"), 3) / 100.0 * num1 + this.FltVal("LCP.X7"), 2) != num2)
        num3 = num1 == 0.0 || num2 == 0.0 ? 0.0 : Utils.ArithmeticRounding(num2 / num1 * 100.0 - 0.0005, 3);
      this.SetCurrentNum("NEWHUD.X223", num3);
      this.SetCurrentNum("NEWHUD.X1141", num3);
      double num4 = Utils.ArithmeticRounding(num1 * num3 / 100.0, 2);
      double num5 = num2 - num4;
      if (num5 == 0.0)
      {
        this.SetVal("NEWHUD.X224", "");
        this.SetVal("NEWHUD.X1225", "");
      }
      else
      {
        this.SetCurrentNum("NEWHUD.X224", num5);
        this.SetCurrentNum("NEWHUD.X1225", num5);
      }
      this.SetCurrentNum("NEWHUD.X225", num2);
      this.SetCurrentNum("NEWHUD.X1142", num2);
      this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem801f((string) null, (string) null);
      if (num3 != 0.0 || num5 != 0.0)
      {
        this.SetVal("NEWHUD.X227", "Lender");
        this.SetVal("NEWHUD.X230", "Broker");
        this.SetVal("NEWHUD.X1167", "Lender");
        this.SetVal("NEWHUD.X1168", "Broker");
        this.SetVal("NEWHUD2.X109", this.Val("VEND.X293"));
      }
      else
      {
        this.SetVal("NEWHUD.X227", "");
        this.SetVal("NEWHUD.X230", "");
        this.SetVal("NEWHUD.X1167", "");
        this.SetVal("NEWHUD.X1168", "");
      }
      if ((!this.calObjs.StopSyncItemization || !this.IsLoanDisclosed()) && this.calObjs.CurrentFormID != "HUD1PG2_2010")
      {
        this.SetCurrentNum("NEWHUD.X246", num3);
        this.SetCurrentNum("NEWHUD.X247", num5);
        this.SetCurrentNum("NEWHUD.X250", num2);
        this.SetCurrentNum("NEWHUD.X1200", num3);
        this.SetCurrentNum("NEWHUD.X1228", num5);
        this.SetCurrentNum("NEWHUD.X1201", num2);
      }
      if (callNewHudFormCalculation)
        this.calObjs.NewHudCal.FormCal((string) null, (string) null);
      else
        this.calObjs.NewHud2015Cal.CalcClosingCostSubTotal((string) null, (string) null);
    }

    private void calculateTPOLoanInfo(string id, string val)
    {
      switch (id)
      {
        case "TPO.X5":
          this.SetVal("TPO.X10", this.Val("TPO.X5") == "Y" ? DateTime.Today.ToString("MM/dd/yyyy") : "");
          break;
        case "TPO.X93":
          this.SetVal("TPO.X94", this.Val("TPO.X93") == "Y" ? DateTime.Today.ToString("MM/dd/yyyy") : "");
          break;
      }
    }

    internal void ClearTPOInformationTool()
    {
      for (int index = 0; index < ToolCalculation.tpoInfoFields.Length; ++index)
        this.SetVal("TPO.X" + ToolCalculation.tpoInfoFields[index], "");
    }

    internal bool IsTPOInformationEmpty()
    {
      for (int index = 0; index < ToolCalculation.tpoInfoFields.Length; ++index)
      {
        if (this.Val("TPO.X" + ToolCalculation.tpoInfoFields[index]) != string.Empty)
          return false;
      }
      return true;
    }

    internal bool IsClosingVendorInformationEmpty(
      bool checkLender,
      bool checkInvestor,
      bool checkBroker)
    {
      if (checkLender)
      {
        for (int index = 0; index < ToolCalculation.closingVendorLenderFields.Length; ++index)
        {
          if (this.Val(ToolCalculation.closingVendorLenderFields[index]) != string.Empty)
            return false;
        }
      }
      if (checkInvestor)
      {
        for (int index = 0; index < ToolCalculation.closingVendorInvestorFields.Length; ++index)
        {
          if (this.Val(ToolCalculation.closingVendorInvestorFields[index]) != string.Empty)
            return false;
        }
      }
      if (checkBroker)
      {
        for (int index = 0; index < ToolCalculation.closingVendorBrokerFields.Length; ++index)
        {
          if (this.Val(ToolCalculation.closingVendorBrokerFields[index]) != string.Empty)
            return false;
        }
      }
      return true;
    }

    internal void ClearClosingVendorInformation(
      bool clearLender,
      bool clearInvestor,
      bool clearBroker)
    {
      if (clearLender)
      {
        for (int index = 0; index < ToolCalculation.closingVendorLenderFields.Length; ++index)
          this.SetVal(ToolCalculation.closingVendorLenderFields[index], "");
      }
      if (clearInvestor)
      {
        for (int index = 0; index < ToolCalculation.closingVendorInvestorFields.Length; ++index)
          this.SetVal(ToolCalculation.closingVendorInvestorFields[index], "");
        this.clearInvestorInfo((string) null, (string) null);
      }
      if (!clearBroker)
        return;
      for (int index = 0; index < ToolCalculation.closingVendorBrokerFields.Length; ++index)
        this.SetVal(ToolCalculation.closingVendorBrokerFields[index], "");
    }

    internal void UpdateClosingVendorInformation(string id, string val)
    {
      string strA = this.Val("2626");
      string userId = this.Val("LOID");
      string stateName = this.Val("14");
      string tpoOrgID = this.Val("TPO.X39");
      if (tpoOrgID == string.Empty)
        tpoOrgID = this.Val("TPO.X15");
      string tpoLOID = this.Val("TPO.X62");
      if ((string.Compare(strA, "Banked - Retail", true) == 0 || string.Compare(strA, "Brokered", true) == 0) && strA != string.Empty && userId != string.Empty && stateName != string.Empty)
      {
        UserInfo user = this.sessionObjects.OrganizationManager.GetUser(userId);
        OrgInfo vendorInformation = user != (UserInfo) null ? this.sessionObjects.OrganizationManager.GetOrganizationForClosingVendorInformation(user.OrgId) : (OrgInfo) null;
        StateLicenseExtType stateLicense = vendorInformation?.OrgBranchLicensing.IsExists(stateName);
        if (string.Compare(strA, "Banked - Retail", true) == 0)
          this.SetInternalLenderLicense(vendorInformation != null ? vendorInformation.CompanyName : "", vendorInformation != null ? vendorInformation.CompanyAddress.Street1 + " " + vendorInformation.CompanyAddress.Street2 : "", vendorInformation != null ? vendorInformation.CompanyAddress.City : "", vendorInformation != null ? vendorInformation.CompanyAddress.State : "", vendorInformation != null ? vendorInformation.CompanyAddress.Zip : "", vendorInformation != null ? vendorInformation.NMLSCode : "", vendorInformation?.OrgBranchLicensing, stateLicense);
        else
          this.SetInternalBrokerLicense(vendorInformation != null ? vendorInformation.CompanyName : "", vendorInformation != null ? vendorInformation.CompanyAddress.Street1 + " " + vendorInformation.CompanyAddress.Street2 : "", vendorInformation != null ? vendorInformation.CompanyAddress.City : "", vendorInformation != null ? vendorInformation.CompanyAddress.State : "", vendorInformation != null ? vendorInformation.CompanyAddress.Zip : "", vendorInformation != null ? vendorInformation.NMLSCode : "", vendorInformation?.OrgBranchLicensing, stateLicense);
      }
      else
      {
        if (string.Compare(strA, "Banked - Wholesale", true) != 0 && string.Compare(strA, "Correspondent", true) != 0 || !(strA != string.Empty) || !(stateName != string.Empty) || !(tpoOrgID != string.Empty))
          return;
        ExternalOriginatorManagementData originatorManagementData = (ExternalOriginatorManagementData) null;
        BranchExtLicensing branchExtLicensing = (BranchExtLicensing) null;
        OrgInfo orgInfo = (OrgInfo) null;
        List<object> vendorInformation = this.sessionObjects.ConfigurationManager.GetTPOForClosingVendorInformation(tpoOrgID, tpoLOID);
        if (vendorInformation != null && vendorInformation.Count > 0)
        {
          foreach (object obj in vendorInformation)
          {
            switch (obj)
            {
              case ExternalOriginatorManagementData _:
                originatorManagementData = (ExternalOriginatorManagementData) obj;
                continue;
              case BranchExtLicensing _:
                branchExtLicensing = (BranchExtLicensing) obj;
                continue;
              case OrgInfo _:
                orgInfo = (OrgInfo) obj;
                continue;
              default:
                continue;
            }
          }
        }
        StateLicenseExtType stateLicenseExtType = branchExtLicensing?.IsExists(stateName);
        string tpoEntityType = originatorManagementData == null || originatorManagementData.TypeOfEntity <= -1 || originatorManagementData.TypeOfEntity >= Utils.TPOEntityTypes.Count ? "" : Utils.TPOEntityTypes[originatorManagementData.TypeOfEntity];
        if (string.Compare(strA, "Banked - Wholesale", true) == 0)
          this.SetExternalBrokerLicense(originatorManagementData != null ? (originatorManagementData.CompanyDBAName == string.Empty ? originatorManagementData.CompanyLegalName : originatorManagementData.CompanyDBAName) : "", originatorManagementData != null ? originatorManagementData.Address : "", originatorManagementData != null ? originatorManagementData.City : "", originatorManagementData != null ? originatorManagementData.State : "", originatorManagementData != null ? originatorManagementData.Zip : "", originatorManagementData != null ? originatorManagementData.StateIncorp : "", tpoEntityType, originatorManagementData != null ? originatorManagementData.TaxID : "", originatorManagementData != null ? originatorManagementData.NmlsId : "", branchExtLicensing, stateLicenseExtType);
        else
          this.SetExternalLenderLicense(originatorManagementData != null ? (originatorManagementData.CompanyDBAName == string.Empty ? originatorManagementData.CompanyLegalName : originatorManagementData.CompanyDBAName) : "", originatorManagementData != null ? originatorManagementData.Address : "", originatorManagementData != null ? originatorManagementData.City : "", originatorManagementData != null ? originatorManagementData.State : "", originatorManagementData != null ? originatorManagementData.Zip : "", originatorManagementData != null ? originatorManagementData.StateIncorp : "", tpoEntityType, originatorManagementData != null ? originatorManagementData.NmlsId : "", branchExtLicensing, stateLicenseExtType);
        StateLicenseExtType stateLicense = orgInfo == null || orgInfo.OrgBranchLicensing == null ? (StateLicenseExtType) null : orgInfo.OrgBranchLicensing.IsExists(stateName);
        if (string.Compare(strA, "Banked - Wholesale", true) == 0)
          this.SetInternalLenderLicense(orgInfo != null ? orgInfo.CompanyName : "", orgInfo != null ? orgInfo.CompanyAddress.Street1 + " " + orgInfo.CompanyAddress.Street2 : "", orgInfo != null ? orgInfo.CompanyAddress.City : "", orgInfo != null ? orgInfo.CompanyAddress.State : "", orgInfo != null ? orgInfo.CompanyAddress.Zip : "", orgInfo != null ? orgInfo.NMLSCode : "", orgInfo?.OrgBranchLicensing, stateLicense);
        else
          this.SetInternalInvestorLicense(orgInfo != null ? orgInfo.CompanyName : "", orgInfo != null ? orgInfo.CompanyAddress.Street1 + " " + orgInfo.CompanyAddress.Street2 : "", orgInfo != null ? orgInfo.CompanyAddress.City : "", orgInfo != null ? orgInfo.CompanyAddress.State : "", orgInfo != null ? orgInfo.CompanyAddress.Zip : "", stateLicense);
      }
    }

    internal void SetInternalLenderLicense(
      string lenderName,
      string lenderAddress,
      string lenderCity,
      string lenderState,
      string lenderZip,
      string lenderNMLS,
      BranchExtLicensing lenderLicense,
      StateLicenseExtType stateLicense)
    {
      this.SetVal("1264", lenderName);
      this.SetVal("1257", lenderAddress);
      this.SetVal("1258", lenderCity);
      this.SetVal("1259", lenderState);
      this.SetVal("1260", lenderZip);
      this.SetVal("Lender.OrgState", "");
      this.SetVal("Lender.OrgType", "");
      this.SetVal("3244", lenderNMLS);
      this.SetVal("3895", lenderLicense != null ? lenderLicense.LenderType : "");
      this.SetVal("3896", lenderLicense != null ? lenderLicense.HomeState : "");
      this.SetVal("3897", stateLicense != null ? MaventLicenseTypesUtils.GetLicenseName(stateLicense.LicenseType) : "");
      this.SetVal("3032", stateLicense != null ? stateLicense.LicenseNo : "");
      this.SetVal("3898", stateLicense != null ? (stateLicense.Exempt ? "Y" : "N") : "");
    }

    internal void SetInternalBrokerLicense(
      string brokerName,
      string brokerAddress,
      string brokerCity,
      string brokerState,
      string brokerZip,
      string brokerNMLS,
      BranchExtLicensing brokerLicense,
      StateLicenseExtType stateLicense)
    {
      this.SetVal("VEND.X293", brokerName);
      this.SetVal("VEND.X294", brokerAddress);
      this.SetVal("VEND.X295", brokerCity);
      this.SetVal("VEND.X296", brokerState);
      this.SetVal("VEND.X297", brokerZip);
      this.SetVal("VEND.X299", "");
      this.SetVal("VEND.X307", "");
      this.SetVal("VEND.X528", "");
      this.SetVal("VEND.X527", brokerNMLS);
      this.SetVal("VEND.X651", brokerLicense != null ? brokerLicense.LenderType : "");
      this.SetVal("VEND.X652", stateLicense != null ? MaventLicenseTypesUtils.GetLicenseName(stateLicense.LicenseType) : "");
      this.SetVal("VEND.X300", stateLicense != null ? stateLicense.LicenseNo : "");
      this.SetVal("VEND.X653", stateLicense != null ? (stateLicense.Exempt ? "Y" : "N") : "");
    }

    internal void SetInternalInvestorLicense(
      string investorName,
      string investorAddress,
      string investorCity,
      string investorState,
      string investorZip,
      StateLicenseExtType stateLicense)
    {
      this.SetVal("VEND.X263", investorName);
      if (investorName == "")
      {
        this.clearInvestorInfo((string) null, (string) null);
      }
      else
      {
        this.SetVal("VEND.X264", investorAddress);
        this.SetVal("VEND.X265", investorCity);
        this.SetVal("VEND.X266", investorState);
        this.SetVal("VEND.X267", investorZip);
      }
      this.SetVal("VEND.X649", stateLicense != null ? MaventLicenseTypesUtils.GetLicenseName(stateLicense.LicenseType) : "");
      this.SetVal("VEND.X650", stateLicense != null ? stateLicense.LicenseNo : "");
    }

    internal void SetExternalLenderLicense(
      string lenderName,
      string lenderAddress,
      string lenderCity,
      string lenderState,
      string lenderZip,
      string orgState,
      string orgType,
      string lenderNMLS,
      BranchExtLicensing license,
      StateLicenseExtType stateLic)
    {
      this.SetVal("1264", lenderName);
      this.SetVal("1257", lenderAddress);
      this.SetVal("1258", lenderCity);
      this.SetVal("1259", lenderState);
      this.SetVal("1260", lenderZip);
      this.SetVal("Lender.OrgState", orgState);
      this.SetVal("Lender.OrgType", orgType);
      this.SetVal("3244", lenderNMLS);
      this.SetVal("3895", license != null ? license.LenderType : "");
      this.SetVal("3896", license != null ? license.HomeState : "");
      this.SetVal("3897", stateLic != null ? stateLic.LicenseType : "");
      this.SetVal("3032", stateLic != null ? stateLic.LicenseNo : "");
      this.SetVal("3898", stateLic != null ? (stateLic.Exempt ? "Y" : "N") : "");
    }

    internal void SetExternalBrokerLicense(
      string brokerName,
      string brokerAddress,
      string brokerCity,
      string brokerState,
      string brokerZip,
      string brokerOrgState,
      string brokerOrgType,
      string brokerTaxID,
      string brokerNMLS,
      BranchExtLicensing brokerLicense,
      StateLicenseExtType stateLicense)
    {
      this.SetVal("VEND.X293", brokerName);
      this.SetVal("VEND.X294", brokerAddress);
      this.SetVal("VEND.X295", brokerCity);
      this.SetVal("VEND.X296", brokerState);
      this.SetVal("VEND.X297", brokerZip);
      this.SetVal("VEND.X299", brokerOrgState);
      this.SetVal("VEND.X307", brokerOrgType);
      this.SetVal("VEND.X528", brokerTaxID);
      this.SetVal("VEND.X527", brokerNMLS);
      this.SetVal("VEND.X651", brokerLicense != null ? brokerLicense.LenderType : "");
      this.SetVal("VEND.X652", stateLicense != null ? stateLicense.LicenseType : "");
      this.SetVal("VEND.X300", stateLicense != null ? stateLicense.LicenseNo : "");
      this.SetVal("VEND.X653", stateLicense != null ? (stateLicense.Exempt ? "Y" : "N") : "");
    }

    private void clearClosingVendorLicenseFields(
      bool clearLender,
      bool clearInvestor,
      bool clearBroker)
    {
      if (clearLender)
      {
        this.SetVal("1264", "");
        this.SetVal("1257", "");
        this.SetVal("1258", "");
        this.SetVal("1259", "");
        this.SetVal("1260", "");
        this.SetVal("Lender.OrgState", "");
        this.SetVal("Lender.OrgType", "");
        this.SetVal("3244", "");
        this.SetVal("3895", "");
        this.SetVal("3896", "");
        this.SetVal("3897", "");
        this.SetVal("3032", "");
        this.SetVal("3898", "");
      }
      if (clearInvestor)
      {
        this.SetVal("VEND.X263", "");
        this.clearInvestorInfo((string) null, (string) null);
        this.SetVal("VEND.X649", "");
        this.SetVal("VEND.X650", "");
      }
      if (!clearBroker)
        return;
      this.SetVal("VEND.X293", "");
      this.SetVal("VEND.X294", "");
      this.SetVal("VEND.X295", "");
      this.SetVal("VEND.X296", "");
      this.SetVal("VEND.X297", "");
      this.SetVal("VEND.X299", "");
      this.SetVal("VEND.X307", "");
      this.SetVal("VEND.X528", "");
      this.SetVal("VEND.X527", "");
      this.SetVal("VEND.X651", "");
      this.SetVal("VEND.X652", "");
      this.SetVal("VEND.X300", "");
      this.SetVal("VEND.X653", "");
    }

    private void clearInvestorInfo(string id, string val)
    {
      if (this.Val("VEND.X263") != "")
        return;
      for (int index = 264; index <= 267; ++index)
        this.SetVal("VEND.X" + (object) index, "");
      for (int index = 271; index <= 274; ++index)
        this.SetVal("VEND.X" + (object) index, "");
      for (int index = 369; index <= 395; ++index)
        this.SetVal("VEND.X" + (object) index, "");
      for (int index = 529; index <= 648; ++index)
        this.SetVal("VEND.X" + (object) index, "");
    }

    internal void CopyTPOCustomFieldsToLoanFields(int oid)
    {
      ContactCustomFieldInfoCollection customFieldInfo = this.sessionObjects.ConfigurationManager.GetCustomFieldInfo();
      if (oid != 0)
      {
        foreach (ContactCustomField customFieldValue in this.sessionObjects.ConfigurationManager.GetCustomFieldValues(oid))
        {
          foreach (ContactCustomFieldInfo contactCustomFieldInfo in customFieldInfo.Items)
          {
            if (contactCustomFieldInfo.LabelID == customFieldValue.FieldID && contactCustomFieldInfo.LoanFieldId != "" && customFieldValue.FieldValue != "")
              this.SetVal(contactCustomFieldInfo.LoanFieldId, customFieldValue.FieldValue);
          }
        }
      }
      else
      {
        string tpoId1 = this.Val("TPO.X15");
        if (tpoId1 == "")
          return;
        List<ExternalOriginatorManagementData> organizationByTpoid1 = this.sessionObjects.ConfigurationManager.GetExternalOrganizationByTPOID(tpoId1);
        ExternalOriginatorManagementData originatorManagementData1 = (ExternalOriginatorManagementData) null;
        foreach (ExternalOriginatorManagementData originatorManagementData2 in organizationByTpoid1)
        {
          if (originatorManagementData2.OrganizationType == ExternalOriginatorOrgType.Company)
          {
            originatorManagementData1 = originatorManagementData2;
            break;
          }
        }
        if (originatorManagementData1 == null)
          return;
        foreach (ContactCustomField customFieldValue in this.sessionObjects.ConfigurationManager.GetCustomFieldValues(originatorManagementData1.oid))
        {
          foreach (ContactCustomFieldInfo contactCustomFieldInfo in customFieldInfo.Items)
          {
            if (contactCustomFieldInfo.LabelID == customFieldValue.FieldID && contactCustomFieldInfo.LoanFieldId != "" && customFieldValue.FieldValue != "")
              this.SetVal(contactCustomFieldInfo.LoanFieldId, customFieldValue.FieldValue);
          }
        }
        string tpoId2 = this.Val("TPO.X39");
        if (tpoId2 == "")
          return;
        List<ExternalOriginatorManagementData> organizationByTpoid2 = this.sessionObjects.ConfigurationManager.GetExternalOrganizationByTPOID(tpoId2);
        ExternalOriginatorManagementData originatorManagementData3 = (ExternalOriginatorManagementData) null;
        foreach (ExternalOriginatorManagementData originatorManagementData4 in organizationByTpoid2)
        {
          if (originatorManagementData4.OrganizationType == ExternalOriginatorOrgType.Branch)
          {
            originatorManagementData3 = originatorManagementData4;
            break;
          }
        }
        if (originatorManagementData3 == null)
          return;
        foreach (ContactCustomField customFieldValue in this.sessionObjects.ConfigurationManager.GetCustomFieldValues(originatorManagementData3.oid))
        {
          foreach (ContactCustomFieldInfo contactCustomFieldInfo in customFieldInfo.Items)
          {
            if (contactCustomFieldInfo.LabelID == customFieldValue.FieldID && contactCustomFieldInfo.LoanFieldId != "" && customFieldValue.FieldValue != "")
              this.SetVal(contactCustomFieldInfo.LoanFieldId, customFieldValue.FieldValue);
          }
        }
      }
    }

    internal ContactCustomField[] FindCompanyTPOCustomFields()
    {
      string tpoId = this.Val("TPO.X15");
      if (tpoId == "")
        return (ContactCustomField[]) null;
      List<ExternalOriginatorManagementData> organizationByTpoid = this.sessionObjects.ConfigurationManager.GetExternalOrganizationByTPOID(tpoId);
      ExternalOriginatorManagementData originatorManagementData1 = (ExternalOriginatorManagementData) null;
      foreach (ExternalOriginatorManagementData originatorManagementData2 in organizationByTpoid)
      {
        if (originatorManagementData2.OrganizationType == ExternalOriginatorOrgType.Company)
        {
          originatorManagementData1 = originatorManagementData2;
          break;
        }
      }
      return originatorManagementData1 == null ? (ContactCustomField[]) null : this.sessionObjects.ConfigurationManager.GetCustomFieldValues(originatorManagementData1.oid);
    }

    internal ContactCustomField[] FindBranchTPOCustomFields()
    {
      string tpoId = this.Val("TPO.X39");
      if (tpoId == "")
        return (ContactCustomField[]) null;
      List<ExternalOriginatorManagementData> organizationByTpoid = this.sessionObjects.ConfigurationManager.GetExternalOrganizationByTPOID(tpoId);
      ExternalOriginatorManagementData originatorManagementData1 = (ExternalOriginatorManagementData) null;
      foreach (ExternalOriginatorManagementData originatorManagementData2 in organizationByTpoid)
      {
        if (originatorManagementData2.OrganizationType == ExternalOriginatorOrgType.Branch)
        {
          originatorManagementData1 = originatorManagementData2;
          break;
        }
      }
      return originatorManagementData1 == null ? (ContactCustomField[]) null : this.sessionObjects.ConfigurationManager.GetCustomFieldValues(originatorManagementData1.oid);
    }

    internal void CopyTPOCustomFieldsToLoanFields(ContactCustomField[] customFields)
    {
      ContactCustomFieldInfoCollection customFieldInfo = this.sessionObjects.ConfigurationManager.GetCustomFieldInfo();
      foreach (ContactCustomField customField in customFields)
      {
        foreach (ContactCustomFieldInfo contactCustomFieldInfo in customFieldInfo.Items)
        {
          if (contactCustomFieldInfo.LabelID == customField.FieldID && contactCustomFieldInfo.LoanFieldId != "" && customField.FieldValue != "" && this.loan.IsFieldEditable(contactCustomFieldInfo.LoanFieldId))
            this.SetVal(contactCustomFieldInfo.LoanFieldId, customField.FieldValue);
        }
      }
    }

    private void calculateCorrespondentLateFee(string id, string val)
    {
      DateTime feeLatestBeginDate = this.GetCorrespondentLateFeeLatestBeginDate(false);
      if (this.calObjs.ExternalLateFeeSettings != null)
        this.SetVal("3928", feeLatestBeginDate != DateTime.MinValue ? feeLatestBeginDate.ToString("MM/dd/yyyy") : "");
      if (this.calObjs.ExternalLateFeeSettings != null)
        this.SetVal("4111", this.GetGracePeriodStartTriggerDate());
      if (this.calObjs.ExternalLateFeeSettings != null)
        this.SetVal("3929", this.GetLateDaysEnd());
      int num1 = Utils.GetTotalTimeSpanDays(this.Val("3928"), this.Val("3929"), true);
      if (num1 < 0)
        this.SetVal("3929", "");
      if (this.calObjs.ExternalLateFeeSettings != null && this.calObjs.ExternalLateFeeSettings.MaxLateDays > 0 && num1 > this.calObjs.ExternalLateFeeSettings.MaxLateDays)
        num1 = this.calObjs.ExternalLateFeeSettings.MaxLateDays;
      this.SetVal("3930", num1 > 0 ? num1.ToString() : "");
      Decimal num2 = this.CalcCorrespondentLateFeeCharge(Utils.ParseDecimal((object) this.Val("3931")), Utils.ParseDecimal((object) this.Val("3932")), Utils.ParseInt((object) this.Val("3930")));
      if (this.calObjs.ExternalLateFeeSettings != null && this.calObjs.ExternalLateFeeSettings.FeeHandledAs == 1)
      {
        this.SetVal("3934", num2 > 0M ? num2.ToString("N5") : "");
        this.SetVal("3937", "");
      }
      else
      {
        this.SetVal("3934", "");
        this.SetVal("3937", num2 > 0M ? num2.ToString("N2") : "");
      }
      if (this.calObjs.ExternalLateFeeSettings != null && this.calObjs.ExternalLateFeeSettings.FeeHandledAs == 1)
      {
        if (num2 > 0M)
        {
          num2 *= -1M;
          this.SetVal("3938", num2 != 0M ? num2.ToString("N5") : "");
        }
        else
          this.SetVal("3938", "");
        this.SetVal("3939", "");
      }
      else
      {
        this.SetVal("3939", num2 > 0M ? num2.ToString("N2") : "");
        this.SetVal("3938", "");
      }
      this.calculateCorrespondentPurchaseAdvice(id, val);
    }

    internal DateTime GetCorrespondentLateFeeLatestBeginDate(bool throwException)
    {
      if (this.calObjs.ExternalLateFeeSettings == null)
        this.calObjs.ExternalLateFeeSettings = this.sessionObjects.ConfigurationManager.GetExternalOrgLateFeeSettings(this.Val("TPO.X15"), true);
      if (this.calObjs.ExternalLateFeeSettings == null)
        return DateTime.MinValue;
      List<DateTime> datesToCheck = new List<DateTime>();
      if ((this.calObjs.ExternalLateFeeSettings.GracePeriodLaterOf & 1) == 1)
        datesToCheck.Add(Utils.ParseDate((object) this.Val("3918")));
      if ((this.calObjs.ExternalLateFeeSettings.GracePeriodLaterOf & 8) == 8)
        datesToCheck.Add(Utils.ParseDate((object) this.Val("3919")));
      if ((this.calObjs.ExternalLateFeeSettings.GracePeriodLaterOf & 2) == 2)
        datesToCheck.Add(Utils.ParseDate((object) this.Val("3920")));
      if ((this.calObjs.ExternalLateFeeSettings.GracePeriodLaterOf & 4) == 4)
        datesToCheck.Add(Utils.ParseDate((object) this.Val("3926")));
      if ((this.calObjs.ExternalLateFeeSettings.GracePeriodLaterOf & 16) == 16 && this.calObjs.ExternalLateFeeSettings.OtherDate.StartsWith("Fields."))
      {
        string id = this.calObjs.ExternalLateFeeSettings.OtherDate.Substring(7);
        if (id != string.Empty)
          datesToCheck.Add(Utils.ParseDate((object) this.Val(id)));
      }
      return this.GetCorrespondentLateFeeLatestBeginDate(datesToCheck, this.IntVal("3927"), throwException);
    }

    internal string GetGracePeriodStartTriggerDate()
    {
      DateTime date = DateTime.MinValue;
      string startTriggerDate = string.Empty;
      if (this.calObjs.ExternalLateFeeSettings != null)
      {
        List<DateTime> dateTimeList = new List<DateTime>();
        if ((this.calObjs.ExternalLateFeeSettings.GracePeriodLaterOf & 1) == 1 && Utils.ParseDate((object) this.Val("3918")) > date)
        {
          date = Utils.ParseDate((object) this.Val("3918"));
          startTriggerDate = "Purchase Suspense Date";
        }
        if ((this.calObjs.ExternalLateFeeSettings.GracePeriodLaterOf & 8) == 8 && Utils.ParseDate((object) this.Val("3919")) > date)
        {
          date = Utils.ParseDate((object) this.Val("3919"));
          startTriggerDate = "Latest Cond's Issue Date";
        }
        if ((this.calObjs.ExternalLateFeeSettings.GracePeriodLaterOf & 2) == 2 && Utils.ParseDate((object) this.Val("3920")) > date)
        {
          date = Utils.ParseDate((object) this.Val("3920"));
          startTriggerDate = "Purchase Approval Date";
        }
        if ((this.calObjs.ExternalLateFeeSettings.GracePeriodLaterOf & 4) == 4 && Utils.ParseDate((object) this.Val("3926")) > date)
        {
          date = Utils.ParseDate((object) this.Val("3926"));
          startTriggerDate = "Delivery Expiration Date";
        }
        if ((this.calObjs.ExternalLateFeeSettings.GracePeriodLaterOf & 16) == 16 && this.calObjs.ExternalLateFeeSettings.OtherDate.StartsWith("Fields."))
        {
          string id = this.calObjs.ExternalLateFeeSettings.OtherDate.Substring(7);
          if (id != string.Empty && Utils.ParseDate((object) this.Val(id)) > date)
          {
            this.Val(id);
            date = Utils.ParseDate((object) this.Val(id));
            startTriggerDate = id;
          }
        }
      }
      if (date > DateTime.MinValue)
        this.SetVal("4110", this.getBusinessDate(date, this.calObjs.ExternalLateFeeSettings.GracePeriodStarts).ToString("MM/dd/yyyy"));
      else
        this.SetVal("4110", "");
      return startTriggerDate;
    }

    private DateTime getBusinessDate(DateTime date, int addDays)
    {
      if (date == DateTime.MinValue)
        return DateTime.MinValue;
      DateTime date1 = date;
      while (!this.isBusinessDay(date1))
        date1 = date1.AddDays(1.0);
      if (addDays != 0)
      {
        int num = 0;
        while (num < addDays)
        {
          date1 = date1.AddDays(1.0);
          if (this.isBusinessDay(date1))
            ++num;
        }
      }
      return date1;
    }

    private bool isBusinessDay(DateTime date)
    {
      bool flag = true;
      this.getTpoSettingAndBusinessCalendar();
      if (this.calObjs.ExternalLateFeeSettings.GracePeriodCalendar == 0)
      {
        if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
          flag = false;
      }
      else if (!this.businessCalendar.IsBusinessDay(date))
        flag = false;
      return flag;
    }

    private void getTpoSettingAndBusinessCalendar()
    {
      if (this.calObjs.ExternalLateFeeSettings == null)
        this.calObjs.ExternalLateFeeSettings = this.sessionObjects.ConfigurationManager.GetExternalOrgLateFeeSettings(this.Val("TPO.X15"), true);
      if (this.calObjs.ExternalLateFeeSettings == null || this.calObjs.ExternalLateFeeSettings.GracePeriodCalendar <= 0 || this.businessCalendar != null && (this.calObjs.ExternalLateFeeSettings.GracePeriodCalendar != 2 || this.businessCalendar.CalendarType == CalendarType.Business) && (this.calObjs.ExternalLateFeeSettings.GracePeriodCalendar != 1 || this.businessCalendar.CalendarType == CalendarType.Postal))
        return;
      this.businessCalendar = this.sessionObjects.GetBusinessCalendar(this.calObjs.ExternalLateFeeSettings.GracePeriodCalendar == 2 ? CalendarType.Business : CalendarType.Postal);
    }

    internal string GetLateDaysEnd()
    {
      string s = string.Empty;
      if (this.calObjs.ExternalLateFeeSettings.DayCleared == 2)
        s = this.Val("3921");
      else if (this.calObjs.ExternalLateFeeSettings.DayCleared == 4)
      {
        if (this.calObjs.ExternalLateFeeSettings.DayClearedOtherDate.StartsWith("Fields."))
        {
          string id = this.calObjs.ExternalLateFeeSettings.DayClearedOtherDate.Substring(7);
          if (id != string.Empty)
            s = this.Val(id);
        }
      }
      else
        s = this.Val("3920");
      if (string.IsNullOrEmpty(s))
        return s;
      DateTime dateTime = DateTime.MinValue;
      try
      {
        dateTime = DateTime.Parse(s);
      }
      catch (Exception ex)
      {
        Tracing.Log(ToolCalculation.sw, TraceLevel.Error, nameof (ToolCalculation), ex.Message);
      }
      if (dateTime != DateTime.MinValue && this.calObjs.ExternalLateFeeSettings != null && this.calObjs.ExternalLateFeeSettings.IncludeDay == 1)
        dateTime = dateTime.AddDays(-1.0);
      return !(dateTime != DateTime.MinValue) ? "" : dateTime.ToString("MM/dd/yyyy");
    }

    internal Decimal CalcCorrespondentLateFeeCharge(
      Decimal lateFeePercent,
      Decimal lateFeeAdditional,
      int lateFeeDays)
    {
      if (lateFeePercent < 0M)
        return 0M;
      Decimal num1 = 0M;
      if (lateFeeDays > 0 && (this.calObjs.ExternalLateFeeSettings == null || this.calObjs.ExternalLateFeeSettings.CalculateAs != 1))
        lateFeeDays = 1;
      if (this.calObjs.ExternalLateFeeSettings != null && this.calObjs.ExternalLateFeeSettings.FeeHandledAs == 1)
      {
        if (lateFeeDays > 0)
          num1 = lateFeePercent * (Decimal) lateFeeDays;
        return num1;
      }
      Decimal num2 = string.IsNullOrEmpty(this.Val("3571")) ? Utils.ParseDecimal((object) this.Val("2")) : Utils.ParseDecimal((object) this.Val("3571"));
      return (lateFeePercent / 100M * num2 + lateFeeAdditional) * (Decimal) lateFeeDays;
    }

    internal DateTime GetCorrespondentLateFeeLatestBeginDate(
      List<DateTime> datesToCheck,
      int graceDays,
      bool throwException)
    {
      DateTime minValue = DateTime.MinValue;
      if (datesToCheck != null && datesToCheck.Count > 0)
      {
        for (int index = 0; index < datesToCheck.Count; ++index)
        {
          if (DateTime.Compare(datesToCheck[index], minValue) > 0)
            minValue = datesToCheck[index];
        }
      }
      DateTime date = this.getBusinessDate(minValue, 0);
      if (date != DateTime.MinValue)
      {
        this.getTpoSettingAndBusinessCalendar();
        if (this.calObjs.ExternalLateFeeSettings != null && this.calObjs.ExternalLateFeeSettings.GracePeriodStarts == 1)
          date = this.getBusinessDate(date, 1);
        date = this.getBusinessDate(date, graceDays);
        try
        {
          if (this.calObjs.ExternalLateFeeSettings != null)
          {
            if (this.calObjs.ExternalLateFeeSettings.StartOnWeekend == 1)
            {
              if (this.businessCalendar != null)
                date = this.businessCalendar.GetNextClosestBusinessDay(date);
              if (date.DayOfWeek == DayOfWeek.Saturday)
                date = date.AddDays(2.0);
              else if (date.DayOfWeek == DayOfWeek.Sunday)
                date = date.AddDays(1.0);
            }
          }
        }
        catch (Exception ex)
        {
          if (throwException)
            throw new Exception(ex.Message);
          return DateTime.MinValue;
        }
      }
      return date;
    }

    internal DateTime CalcCorrespondentLateFeeEndDate(DateTime conditionReceivedDate)
    {
      if (conditionReceivedDate != DateTime.MinValue && this.calObjs.ExternalLateFeeSettings != null && this.calObjs.ExternalLateFeeSettings.IncludeDay == 1)
        conditionReceivedDate = conditionReceivedDate.AddDays(-1.0);
      return conditionReceivedDate;
    }

    internal void UpdateExternalLateFeeSettingFields()
    {
      if (this.calObjs.ExternalLateFeeSettings == null)
      {
        for (int index = 1; index <= 17; ++index)
          this.SetVal("LATEFEESETTING.X" + (object) index, "");
      }
      else
      {
        this.SetVal("LATEFEESETTING.X1", string.Concat((object) this.calObjs.ExternalLateFeeSettings.CalculateAs));
        this.SetVal("LATEFEESETTING.X2", string.Concat((object) this.calObjs.ExternalLateFeeSettings.FeeHandledAs));
        this.SetVal("LATEFEESETTING.X3", string.Concat((object) this.calObjs.ExternalLateFeeSettings.GracePeriodCalendar));
        this.SetVal("LATEFEESETTING.X4", string.Concat((object) this.calObjs.ExternalLateFeeSettings.GracePeriodLaterOf));
        this.SetVal("LATEFEESETTING.X5", string.Concat((object) this.calObjs.ExternalLateFeeSettings.GracePeriodStarts));
        this.SetVal("LATEFEESETTING.X6", string.Concat((object) this.calObjs.ExternalLateFeeSettings.IncludeDay));
        this.SetVal("LATEFEESETTING.X7", string.Concat((object) this.calObjs.ExternalLateFeeSettings.LateFeeBasedOn));
        this.SetVal("LATEFEESETTING.X8", string.Concat((object) this.calObjs.ExternalLateFeeSettings.MaxLateDays));
        this.SetVal("LATEFEESETTING.X9", this.calObjs.ExternalLateFeeSettings.OtherDate);
        this.SetVal("LATEFEESETTING.X10", string.Concat((object) this.calObjs.ExternalLateFeeSettings.StartOnWeekend));
        this.SetVal("LATEFEESETTING.X11", string.Concat((object) this.calObjs.ExternalLateFeeSettings.GracePeriodDays));
        this.SetVal("LATEFEESETTING.X12", string.Concat((object) this.calObjs.ExternalLateFeeSettings.DayCleared));
        this.SetVal("LATEFEESETTING.X13", this.calObjs.ExternalLateFeeSettings.DayClearedOtherDate);
        try
        {
          this.SetVal("LATEFEESETTING.X18", this.sessionObjects.LoanManager.CalculateDescription(this.calObjs.ExternalLateFeeSettings.DayClearedOtherDate));
        }
        catch
        {
        }
        string id1 = this.calObjs.ExternalLateFeeSettings.OtherDate;
        if (!string.IsNullOrEmpty(id1) && id1.StartsWith("Fields."))
          id1 = id1.Substring(7);
        string id2 = this.calObjs.ExternalLateFeeSettings.DayClearedOtherDate;
        if (!string.IsNullOrEmpty(id2) && id2.StartsWith("Fields."))
          id2 = id2.Substring(7);
        this.SetVal("LATEFEESETTING.X14", this.Val(id1));
        this.SetVal("LATEFEESETTING.X15", this.Val(id2));
        this.SetVal("LATEFEESETTING.X16", string.Concat((object) this.calObjs.ExternalLateFeeSettings.LateFee));
        this.SetVal("LATEFEESETTING.X17", string.Concat((object) this.calObjs.ExternalLateFeeSettings.Amount));
      }
    }

    private void updateCorrespondentLoanStatusBalance(string id, string val)
    {
      if (this.Val("2626") != "Correspondent")
        return;
      if (this.Val("3567") != string.Empty && Utils.ToDate(this.Val("3567")) != DateTime.MinValue && this.Val("2") != string.Empty)
        this.SetVal("4107", this.Val("2"));
      if (this.IsLocked("4106"))
        return;
      if (this.Val("3567") != string.Empty && Utils.ToDate(this.Val("3567")) != DateTime.MinValue && (this.Val("2370") == string.Empty || Utils.ToDate(this.Val("2370")) == DateTime.MinValue))
      {
        if (this.Val("Service.X57") != string.Empty && this.FltVal("Service.X57") > 0.0)
          this.SetVal("4106", this.Val("Service.X57"));
        else if (this.Val("3579") != string.Empty && this.FltVal("3579") > 0.0)
          this.SetVal("4106", this.Val("3579"));
      }
      if (!(this.Val("2370") != string.Empty) || !(Utils.ToDate(this.Val("2370")) != DateTime.MinValue) || !(this.Val("2211") != string.Empty) || this.FltVal("2211") <= 0.0)
        return;
      this.SetVal("4106", this.Val("2211"));
    }

    private void copyFromCorrespondentLoanStatusBalance(string id, string val)
    {
      if (this.Val("2626") != LoanChannel.Correspondent.ToString() || !(id == "4106"))
        return;
      this.SetVal("ULDD.X1", this.Val("4106"));
    }

    internal void CalcDefaultTpoBankContact()
    {
      ExternalOriginatorManagementData originatorManagementData1 = (ExternalOriginatorManagementData) null;
      string tpoId = this.Val("TPO.X15");
      string str = this.Val("3964");
      if (tpoId == "")
        return;
      List<ExternalOriginatorManagementData> organizationByTpoid = this.sessionObjects.ConfigurationManager.GetExternalOrganizationByTPOID(tpoId);
      if (organizationByTpoid == null)
        return;
      foreach (ExternalOriginatorManagementData originatorManagementData2 in organizationByTpoid)
      {
        if (originatorManagementData2.OrganizationType == ExternalOriginatorOrgType.Company)
        {
          originatorManagementData1 = originatorManagementData2;
          break;
        }
      }
      if (originatorManagementData1 == null)
        return;
      List<ExternalOrgWarehouse> externalOrgWarehouses = this.sessionObjects.ConfigurationManager.GetExternalOrgWarehouses(originatorManagementData1.oid);
      if (externalOrgWarehouses == null)
        return;
      foreach (ExternalOrgWarehouse externalOrgWarehouse in externalOrgWarehouses)
      {
        if (externalOrgWarehouse.BankID.ToString() == str)
        {
          if (externalOrgWarehouse.UseBankContact)
          {
            this.SetVal("3958", externalOrgWarehouse.BankContactName);
            this.SetVal("3959", externalOrgWarehouse.BankContactEmail);
            this.SetVal("3960", externalOrgWarehouse.BankContactPhone);
            this.SetVal("3961", externalOrgWarehouse.BankContactFax);
          }
          else
          {
            this.SetVal("3958", externalOrgWarehouse.ContactName);
            this.SetVal("3959", externalOrgWarehouse.ContactEmail);
            this.SetVal("3960", externalOrgWarehouse.ContactPhone);
            this.SetVal("3961", externalOrgWarehouse.ContactFax);
          }
          this.SetVal("3963", "Y");
          return;
        }
      }
      this.SetVal("3958", "");
      this.SetVal("3959", "");
      this.SetVal("3960", "");
      this.SetVal("3961", "");
    }

    internal void calculateTpoFeeAmount(string id, string val)
    {
      if (this.Val("2626") != LoanChannel.Correspondent.ToString())
        return;
      string tpoId = this.Val("TPO.X15");
      if (tpoId == "")
        return;
      DateTime date;
      try
      {
        date = Utils.ParseDate((object) this.Val("761"));
      }
      catch
      {
        return;
      }
      ExternalOriginatorManagementData originatorManagementData1 = (ExternalOriginatorManagementData) null;
      List<ExternalOriginatorManagementData> organizationByTpoid = this.sessionObjects.ConfigurationManager.GetExternalOrganizationByTPOID(tpoId);
      if (organizationByTpoid == null)
        return;
      foreach (ExternalOriginatorManagementData originatorManagementData2 in organizationByTpoid)
      {
        if (originatorManagementData2.OrganizationType == ExternalOriginatorOrgType.Company)
        {
          originatorManagementData1 = originatorManagementData2;
          break;
        }
      }
      if (originatorManagementData1 == null)
        return;
      List<ExternalFeeManagement> feeManagement = this.sessionObjects.ConfigurationManager.GetFeeManagement(originatorManagementData1.oid);
      if (feeManagement == null)
        return;
      for (int index = 3587; index <= 3610; ++index)
      {
        if (this.Val(index.ToString()) != string.Empty)
          return;
      }
      this.UpdatePurchasedPrincipal();
      int num1 = 3587;
      Decimal num2 = 0M;
      bool flag1 = false;
      bool flag2 = false;
      foreach (ExternalFeeManagement fee in feeManagement)
      {
        if ((fee.Channel == ExternalOriginatorEntityType.Both || fee.Channel == ExternalOriginatorEntityType.Correspondent) && this.ValidateLockDate(date, fee) && (fee.AdvancedCodeXml == string.Empty || fee.AdvancedCodeXml != string.Empty && this.ValidateFeeConditions(fee)))
        {
          if (num1 == 3670 && !flag1)
          {
            this.SetVal("CORRESPONDENT.X63", fee.Code + " - " + fee.FeeName);
            this.SetVal("CORRESPONDENT.X64", this.CalculateFeeAmount(fee).ToString());
            flag1 = true;
          }
          else if (num1 == 3670 && !flag2)
          {
            this.SetVal("CORRESPONDENT.X65", fee.Code + " - " + fee.FeeName);
            this.SetVal("CORRESPONDENT.X66", this.CalculateFeeAmount(fee).ToString());
            flag2 = true;
          }
          else if (num1 == 3670)
          {
            num2 += this.CalculateFeeAmount(fee);
          }
          else
          {
            this.SetVal(num1.ToString(), fee.Code + " - " + fee.FeeName);
            int num3 = num1 + 1;
            this.SetVal(num3.ToString(), this.CalculateFeeAmount(fee).ToString());
            num1 = num3 + 1;
            if (num1 == 3611)
              num1 = 3670;
          }
        }
      }
      if (num2 > 0M)
        this.SetVal("3970", num2.ToString());
      else
        this.SetVal("3970", "");
      this.UpdateCorrespondentPurchaseAdvice(id, val);
    }

    private bool ValidateLockDate(DateTime lockDate, ExternalFeeManagement fee)
    {
      return lockDate == DateTime.MinValue ? (!(fee.EndDate != DateTime.MinValue) || !(fee.EndDate < DateTime.Today)) && (!(fee.StartDate != DateTime.MinValue) || !(fee.StartDate > DateTime.Today)) : (fee.StartDate == DateTime.MinValue && fee.EndDate != DateTime.MinValue ? lockDate <= fee.EndDate : (fee.StartDate != DateTime.MinValue && fee.EndDate == DateTime.MinValue ? lockDate >= fee.StartDate : !(fee.StartDate != DateTime.MinValue) || !(fee.EndDate != DateTime.MinValue) || lockDate >= fee.StartDate && lockDate <= fee.EndDate));
    }

    private Decimal CalculateFeeAmount(ExternalFeeManagement fee)
    {
      Decimal num1 = 0M;
      if (fee.FeePercent != 0.0)
      {
        Decimal num2 = Utils.ParseDecimal((object) this.Val("3579"), 0M);
        num1 += (Decimal) fee.FeePercent * num2 / 100M;
      }
      return num1 + (Decimal) fee.FeeAmount;
    }

    private bool ValidateFeeConditions(ExternalFeeManagement fee)
    {
      if (fee.AdvancedCodeXml == string.Empty)
        return true;
      try
      {
        foreach (FieldFilter fieldFilter in (List<FieldFilter>) new XmlSerializer().Deserialize(fee.AdvancedCodeXml, typeof (FieldFilterList)))
        {
          if (!fieldFilter.Evaluate((object) this.Val(fieldFilter.FieldID)))
            return false;
        }
        return true;
      }
      catch
      {
        return false;
      }
    }

    internal void UpdateCorrespondentFees(string id, string val)
    {
      this.ClearCorrespondentFees(false);
      this.calculateTpoFeeAmount(id, val);
    }

    private void ClearCorrespondentFees(bool needUpdateBalance)
    {
      bool flag = false;
      List<string> stringList = new List<string>();
      for (int index = 3587; index <= 3610; ++index)
        stringList.Add(index.ToString());
      stringList.Add("CORRESPONDENT.X63");
      stringList.Add("CORRESPONDENT.X64");
      stringList.Add("CORRESPONDENT.X65");
      stringList.Add("CORRESPONDENT.X66");
      foreach (string id in stringList)
      {
        if (this.Val(id) != string.Empty)
        {
          flag = true;
          break;
        }
      }
      if (!flag)
        return;
      foreach (object obj in stringList)
        this.SetVal(obj.ToString(), string.Empty);
      if (!needUpdateBalance)
        return;
      this.UpdateCorrespondentPurchaseAdvice((string) null, (string) null);
    }

    internal void calculateStatementOfDenial(string id, string val)
    {
      if (this.Val("2626") == "Correspondent" && this.Val("TPO.X88") != "Y" && this.CreditorOverride)
      {
        CompanyInfo companyInfo = this.sessionObjects.ConfigurationManager.GetCompanyInfo();
        this.SetVal("DENIAL.X91", companyInfo.Name);
        this.SetVal("DENIAL.X92", companyInfo.Address);
        this.SetVal("DENIAL.X93", companyInfo.City);
        this.SetVal("DENIAL.X94", companyInfo.State);
        this.SetVal("DENIAL.X95", companyInfo.Zip);
        this.SetVal("DENIAL.X96", companyInfo.Phone);
      }
      else
      {
        this.SetVal("DENIAL.X91", this.Val("1264"));
        this.SetVal("DENIAL.X92", this.Val("1257"));
        this.SetVal("DENIAL.X93", this.Val("1258"));
        this.SetVal("DENIAL.X94", this.Val("1259"));
        this.SetVal("DENIAL.X95", this.Val("1260"));
        this.SetVal("DENIAL.X96", this.Val("1262"));
      }
    }

    private void calculatePMICancellationConditions(string id, string val)
    {
      if (this.Val("DISCLOSURE.X1147") != "Y")
        this.SetVal("DISCLOSURE.X1149", "");
      if (!(this.Val("DISCLOSURE.X1148") != "Y"))
        return;
      this.SetVal("DISCLOSURE.X1150", "");
    }

    private void calculateMICancelCondTypeAndRMLA(string id, string val)
    {
      string str1 = this.Val("14");
      string str2 = this.Val("DISCLOSURE.X1209");
      if (this.Val("DISCLOSURE.X1208") == "" && str1 == "OH")
        this.SetVal("DISCLOSURE.X1208", "Partially");
      if (str2 == "" && str1 == "CA")
        this.SetVal("DISCLOSURE.X1209", "PrincipalBalance");
      if (!(str2 != "Other") || !(str1 == "CA"))
        return;
      this.SetVal("DISCLOSURE.X1210", "");
    }

    private void calculateValueForIntrestRateConditions(string id, string val)
    {
    }

    private void updateDisclosureDeterminedField(string id, string val)
    {
      if (!(this.Val("14") == "IL") || !(this.Val("DISCLOSURE.X478") != "Y"))
        return;
      this.SetVal("DISCLOSURE.X479", "");
    }

    private void updateDisclosureExplanationFields(string id, string val)
    {
      if (!(this.Val("14") == "IL") || !(this.Val("DISCLOSURE.X483") != "Y"))
        return;
      this.SetVal("DISCLOSURE.X484", "");
    }

    private void syncConstLinkSyncDefaultData(string id, string val)
    {
      if (!this.calObjs.ForceDefaultLinkSync)
        return;
      if (string.IsNullOrEmpty(id))
      {
        if (this.loan.LinkSyncType != LinkSyncType.ConstructionPrimary && this.loan.LinkSyncType != LinkSyncType.ConstructionLinked)
          return;
        BorrowerPair[] borrowerPairs1 = this.GetBorrowerPairs();
        BorrowerPair[] borrowerPairs2 = this.loan.LinkedData.GetBorrowerPairs();
        if (borrowerPairs1.Length > borrowerPairs2.Length)
        {
          for (; borrowerPairs1.Length > borrowerPairs2.Length; borrowerPairs2 = this.loan.LinkedData.GetBorrowerPairs())
            this.loan.LinkedData.CreateBorrowerPair();
        }
        else if (borrowerPairs2.Length > borrowerPairs1.Length)
        {
          for (; borrowerPairs2.Length > borrowerPairs1.Length; borrowerPairs2 = this.loan.LinkedData.GetBorrowerPairs())
            this.loan.LinkedData.RemoveBorrowerPair(borrowerPairs2[borrowerPairs2.Length - 1]);
        }
        for (int index = 0; index < borrowerPairs1.Length; ++index)
        {
          this.loan.LinkedData.SetCurrentField("4008", this.Val("4008", borrowerPairs1[index]), borrowerPairs2[index]);
          this.loan.LinkedData.SetCurrentField("4000", this.Val("4000", borrowerPairs1[index]), borrowerPairs2[index]);
          this.loan.LinkedData.SetCurrentField("4001", this.Val("4001", borrowerPairs1[index]), borrowerPairs2[index]);
          this.loan.LinkedData.SetCurrentField("4002", this.Val("4002", borrowerPairs1[index]), borrowerPairs2[index]);
          this.loan.LinkedData.SetCurrentField("4003", this.Val("4003", borrowerPairs1[index]), borrowerPairs2[index]);
          this.loan.LinkedData.SetCurrentField("1240", this.Val("1240", borrowerPairs1[index]), borrowerPairs2[index]);
          this.loan.LinkedData.SetCurrentField("4009", this.Val("4009", borrowerPairs1[index]), borrowerPairs2[index]);
          this.loan.LinkedData.SetCurrentField("4004", this.Val("4004", borrowerPairs1[index]), borrowerPairs2[index]);
          this.loan.LinkedData.SetCurrentField("4005", this.Val("4005", borrowerPairs1[index]), borrowerPairs2[index]);
          this.loan.LinkedData.SetCurrentField("4006", this.Val("4006", borrowerPairs1[index]), borrowerPairs2[index]);
          this.loan.LinkedData.SetCurrentField("4007", this.Val("4007", borrowerPairs1[index]), borrowerPairs2[index]);
          this.loan.LinkedData.SetCurrentField("4009", this.Val("4009", borrowerPairs1[index]), borrowerPairs2[index]);
          this.loan.LinkedData.SetCurrentField("1268", this.Val("1268", borrowerPairs1[index]), borrowerPairs2[index]);
          this.loan.LinkedData.SetCurrentField("36", this.Val("36", borrowerPairs1[index]), borrowerPairs2[index]);
          this.loan.LinkedData.SetCurrentField("37", this.Val("37", borrowerPairs1[index]), borrowerPairs2[index]);
          this.loan.LinkedData.SetCurrentField("68", this.Val("68", borrowerPairs1[index]), borrowerPairs2[index]);
          this.loan.LinkedData.SetCurrentField("69", this.Val("69", borrowerPairs1[index]), borrowerPairs2[index]);
          this.calObjs.Cal.UpdateAccountName("4000", (string) null);
          this.loan.LinkedData.SetCurrentField("1868", this.Val("1868", borrowerPairs1[index]), borrowerPairs2[index]);
          this.loan.LinkedData.SetCurrentField("1873", this.Val("1873", borrowerPairs1[index]), borrowerPairs2[index]);
        }
        this.loan.LinkedData.SetField("LE1.X1", this.Val("LE1.X1"));
        string val1 = this.Val("LE1.X28");
        this.loan.LinkedData.SetField("LE1.X28", val1);
        string val2 = this.Val("LE1.X8");
        this.loan.LinkedData.SetField("LE1.X8", val2);
        string val3 = this.Val("LE1.X9");
        this.loan.LinkedData.SetField("LE1.X9", val3);
        if (this.Val("3164") != "Y")
        {
          this.loan.LinkedData.SetField("LE1.XD8", val2);
          this.loan.LinkedData.SetField("LE1.XD9", val3);
          this.loan.LinkedData.SetField("LE1.XD28", val1);
        }
        this.loan.LinkedData.SetField("763", this.Val("763"));
        this.loan.LinkedData.SetField("CD1.X1", this.Val("CD1.X1"));
        this.loan.LinkedData.SetField("748", this.Val("748"));
        this.loan.LinkedData.SetField("3164", this.Val("3164"));
        this.loan.LinkedData.SetField("3972", this.Val("3972"));
        this.loan.LinkedData.SetField("3197", this.Val("3197"));
        this.loan.LinkedData.SetField("3973", this.Val("3973"));
        this.loan.LinkedData.SetField("3974", this.Val("3974"));
        this.loan.LinkedData.SetField("3975", this.Val("3975"));
        this.loan.LinkedData.SetField("3976", this.Val("3976"));
        this.loan.LinkedData.SetField("3942", this.Val("3942"));
        this.loan.LinkedData.SetField("CONST.X2", this.Val("CONST.X2"));
      }
      else
      {
        if (this.loan.LinkSyncType != LinkSyncType.ConstructionPrimary && this.loan.LinkSyncType != LinkSyncType.ConstructionLinked)
          return;
        this.loan.LinkedData.SetField(id, val);
      }
    }

    private void calculateHmdaReportingYear(string id, string val)
    {
      if (this.IsLocked("HMDA.X27"))
        return;
      this.SetVal("HMDA.X27", ToolCalculation.GetHmdaReportingYear(Utils.ParseDate((object) this.Val("749")), this.Val("1393")).ToString());
    }

    public static int GetHmdaReportingYear(DateTime actionDate, string actionTaken)
    {
      if (actionDate == DateTime.MinValue)
        actionDate = DateTime.Now;
      int year = actionDate.Year;
      return string.IsNullOrEmpty(actionTaken) ? DateTime.Now.Year : actionDate.Year;
    }

    private void calculateCPA_ADDLESCROW_MIMIP(string id, string val)
    {
      if (this.Val("CPA.RetainUserInputs") == "Y")
        return;
      string s1 = !(id == "232") ? this.loan.GetField("232") : val;
      string s2 = !(id == "CPA.PaymentHistory.AnticipatedPurchaseDate") ? this.loan.GetField("CPA.PaymentHistory.AnticipatedPurchaseDate") : val;
      string val1 = string.Empty;
      if (!string.IsNullOrEmpty(s1) && !string.IsNullOrEmpty(s2) && s2 != "//")
      {
        double result1 = -1.0;
        DateTime result2 = DateTime.MinValue;
        if (double.TryParse(s1, out result1) && result1 > 0.0 && DateTime.TryParse(s2, out result2))
        {
          string field1 = this.loan.GetField("CPA.PaymentHistory.FirstBorrowerPaymentDueDate");
          DateTime result3 = DateTime.MinValue;
          DateTime.TryParse(field1, out result3);
          string field2 = this.loan.GetField("CPA.PaymentHistory.FirstInvestorPaymentDate");
          DateTime result4 = DateTime.MinValue;
          DateTime.TryParse(field2, out result4);
          int num = Utils.ParseInt((object) this.sessionObjects.ConfigurationManager.GetCompanySetting("Policies", "CutoffCalendarDay"), 14);
          val1 = "0";
          if (result4.Date > result3.Date && (result2.Day == 1 || result2.Day >= num))
            val1 = s1;
        }
      }
      this.SetVal("CPA.ADDLESCROW.MIMIP", val1);
      this.calculateCPAEscrowDetails_AddlEscrow_TotalFields(id, val);
    }

    private void calculateCPAEscrowDetails_OptionDescFields(string id, string val)
    {
      switch (id)
      {
        case "CPA.FCD.Option1Desc":
          this.SetVal("CPA.ADDLESCROW.Option1Desc", val);
          this.SetVal("CPA.ESCROWDISBURSE.Option1Desc", val);
          break;
        case "CPA.FCD.Option2Desc":
          this.SetVal("CPA.ADDLESCROW.Option2Desc", val);
          this.SetVal("CPA.ESCROWDISBURSE.Option2Desc", val);
          break;
      }
    }

    private void calculateCPAEscrowDetails_DescFields(string id, string val)
    {
      string val1 = this.Val("1628");
      this.SetVal("CPA.FCD.1007Description", val1);
      this.SetVal("CPA.ADDLESCROW.1007Description", val1);
      this.SetVal("CPA.ESCROWDISBURSE.1007Description", val1);
      string val2 = this.Val("660");
      this.SetVal("CPA.FCD.1008Description", val2);
      this.SetVal("CPA.ADDLESCROW.1008Description", val2);
      this.SetVal("CPA.ESCROWDISBURSE.1008Description", val2);
      string val3 = this.Val("661");
      this.SetVal("CPA.FCD.1009Description", val3);
      this.SetVal("CPA.ADDLESCROW.1009Description", val3);
      this.SetVal("CPA.ESCROWDISBURSE.1009Description", val3);
    }

    private void calculateCPAEscrowDetails_FCD_AmountFields(string id, string val)
    {
      if (this.Val("CPA.RetainUserInputs") == "Y")
        return;
      this.SetCurrentNum("CPA.FCD.1007Amount", this.FltVal("1631") + this.FltVal("1632"));
      this.SetCurrentNum("CPA.FCD.1008Amount", this.FltVal("658") + this.FltVal("598"));
      this.SetCurrentNum("CPA.FCD.1009Amount", this.FltVal("659") + this.FltVal("599"));
      this.SetCurrentNum("CPA.FCD.HomeInsurance", this.FltVal("656") + this.FltVal("596"));
      this.SetCurrentNum("CPA.FCD.MortgageInsurance", this.FltVal("338") + this.FltVal("563"));
      this.SetCurrentNum("CPA.FCD.PropertyTax", this.FltVal("655") + this.FltVal("595"));
      this.SetCurrentNum("CPA.FCD.CityPropertyTax", this.FltVal("L269") + this.FltVal("L270"));
      this.SetCurrentNum("CPA.FCD.FloodInsurance", this.FltVal("657") + this.FltVal("597"));
      this.SetCurrentNum("CPA.FCD.USDAAnnualFee", this.FltVal("NEWHUD.X1708") + this.FltVal("NEWHUD.X1714"));
      this.SetCurrentNum("CPA.FCD.AggAdjAmount", this.FltVal("558"));
      this.calculateCPAEscrowDetails_FCD_TotalFields(id, val);
    }

    private void calculateCPAEscrowDetails_AddlEscrow_AmountFields(string id, string val)
    {
      if (this.Val("CPA.RetainUserInputs") == "Y")
        return;
      double num1 = this.BoolVal("NEWHUD2.X137") ? this.FltVal("1630") : 0.0;
      this.SetCurrentNum("CPA.ADDLESCROW.1007Amount", num1 > 0.0 ? num1 : 0.0);
      double num2 = this.BoolVal("NEWHUD2.X138") ? this.FltVal("253") : 0.0;
      this.SetCurrentNum("CPA.ADDLESCROW.1008Amount", num2 > 0.0 ? num2 : 0.0);
      double num3 = this.BoolVal("NEWHUD2.X139") ? this.FltVal("254") : 0.0;
      this.SetCurrentNum("CPA.ADDLESCROW.1009Amount", num3 > 0.0 ? num3 : 0.0);
      double num4 = this.BoolVal("NEWHUD2.X133") ? this.FltVal("230") : 0.0;
      this.SetCurrentNum("CPA.ADDLESCROW.HomeInsurance", num4 > 0.0 ? num4 : 0.0);
      double num5 = this.BoolVal("NEWHUD2.X134") ? this.FltVal("231") : 0.0;
      this.SetCurrentNum("CPA.ADDLESCROW.PropertyTax", num5 > 0.0 ? num5 : 0.0);
      double num6 = this.BoolVal("NEWHUD2.X135") ? this.FltVal("L268") : 0.0;
      this.SetCurrentNum("CPA.ADDLESCROW.CityPropertyTax", num6 > 0.0 ? num6 : 0.0);
      double num7 = this.BoolVal("NEWHUD2.X136") ? this.FltVal("235") : 0.0;
      this.SetCurrentNum("CPA.ADDLESCROW.FloodInsurance", num7 > 0.0 ? num7 : 0.0);
      double num8 = this.BoolVal("NEWHUD2.X140") ? this.FltVal("NEWHUD.X1707") : 0.0;
      this.SetCurrentNum("CPA.ADDLESCROW.USDAAnnualFee", num8 > 0.0 ? num8 : 0.0);
      this.calculateCPAEscrowDetails_AddlEscrow_TotalFields(id, val);
    }

    private void calculateCPAEscrowDetails_FCD_TotalFields(string id, string val)
    {
      string[] strArray = new string[12]
      {
        "cpa.fcd.homeinsurance",
        "cpa.fcd.mortgageinsurance",
        "cpa.fcd.propertytax",
        "cpa.fcd.citypropertytax",
        "cpa.fcd.floodinsurance",
        "cpa.fcd.1007amount",
        "cpa.fcd.1008amount",
        "cpa.fcd.1009amount",
        "cpa.fcd.usdaannualfee",
        "cpa.fcd.option1amount",
        "cpa.fcd.option2amount",
        "cpa.fcd.aggadjamount"
      };
      Decimal d = 0M;
      foreach (string id1 in strArray)
        d += Utils.ParseDecimal((object) this.loan.GetField(id1));
      this.SetVal("CPA.FCD.ReservesCollectedAtClosing", this.BlankOrVal(d));
      this.calculateCPAEscrowDetails_FinalTotal(id, val);
    }

    private void calculateCPAEscrowDetails_AddlEscrow_TotalFields(string id, string val)
    {
      int d1 = 0;
      string[] strArray = new string[10]
      {
        "cpa.addlescrow.homeinsurance",
        "cpa.addlescrow.propertytax",
        "cpa.addlescrow.citypropertytax",
        "cpa.addlescrow.floodinsurance",
        "cpa.addlescrow.1007amount",
        "cpa.addlescrow.1008amount",
        "cpa.addlescrow.1009amount",
        "cpa.addlescrow.usdaannualfee",
        "cpa.addlescrow.option1amount",
        "cpa.addlescrow.option2amount"
      };
      Decimal num1 = 0M;
      foreach (string id1 in strArray)
        num1 += Utils.ParseDecimal((object) this.loan.GetField(id1));
      DateTime date1 = Utils.ParseDate((object) this.loan.GetField("CPA.PaymentHistory.FirstBorrowerPaymentDueDate"), DateTime.MinValue);
      DateTime date2 = Utils.ParseDate((object) this.loan.GetField("CPA.PaymentHistory.FirstInvestorPaymentDate"), DateTime.MinValue);
      if (date1 != DateTime.MinValue && date2 != DateTime.MinValue)
      {
        d1 = date2.Month - date1.Month;
        if (date2.Year > date1.Year)
          d1 += 12 * (date2.Year - date1.Year);
      }
      this.loan.SetField("CPA.ADDLESCROW.NumOfPayments", this.BlankOrVal((Decimal) d1));
      Decimal d2 = num1 * (Decimal) d1;
      this.loan.SetField("CPA.ADDLESCROW.SumOfPayments", this.BlankOrVal(d2));
      Decimal num2 = Utils.ParseDecimal((object) this.loan.GetField("CPA.ADDLESCROW.MIMIP"), 0M);
      this.loan.SetField("CPA.ADDLESCROW.AdditionalEscrow", this.BlankOrVal(d2 + num2));
      this.calculateCPAEscrowDetails_FinalTotal(id, val);
    }

    private void calculateCPAEscrowDetails_EscrowDisbursements_TotalFields(string id, string val)
    {
      string[] strArray = new string[11]
      {
        "cpa.escrowdisburse.homeinsurance",
        "cpa.escrowdisburse.mortgageinsurance",
        "cpa.escrowdisburse.propertytax",
        "cpa.escrowdisburse.citypropertytax",
        "cpa.escrowdisburse.floodinsurance",
        "cpa.escrowdisburse.1007amount",
        "cpa.escrowdisburse.1008amount",
        "cpa.escrowdisburse.1009amount",
        "cpa.escrowdisburse.usdaannualfee",
        "cpa.escrowdisburse.option1amount",
        "cpa.escrowdisburse.option2amount"
      };
      Decimal d = 0M;
      foreach (string id1 in strArray)
        d += Utils.ParseDecimal((object) this.loan.GetField(id1));
      this.loan.SetField("CPA.ESCROWDISBURSE.EscrowsToBePaidBySeller", this.BlankOrVal(d));
      this.calculateCPAEscrowDetails_FinalTotal(id, val);
    }

    private void calculateCPAEscrowDetails_FinalTotal(string id, string val)
    {
      this.loan.SetField("CPA.ESCROWDISBURSE.EsrowFundedByInvestor", this.BlankOrVal(Utils.ParseDecimal((object) this.Val("CPA.FCD.ReservesCollectedAtClosing"), 0M) + Utils.ParseDecimal((object) this.loan.GetField("CPA.ADDLESCROW.AdditionalEscrow"), 0M) - Utils.ParseDecimal((object) this.loan.GetField("CPA.ESCROWDISBURSE.EscrowsToBePaidBySeller"), 0M)));
    }

    private string BlankOrVal(Decimal d) => !(d == 0M) ? d.ToString() : "";

    private void calculateNBOtoVesting(string id, string val)
    {
      if (!id.StartsWith("NBOC") && !id.StartsWith("TR"))
        return;
      if (id.StartsWith("NBOC"))
      {
        string str = "NBOC" + this.getRepetableIDIndex(id, 4).ToString("00");
        string vestingGUID = this.Val(str + "99");
        if (vestingGUID == "")
        {
          if (this.Val(str + "09") == "" || this.Val(str + "01") == "" || this.Val(str + "03") == "")
            return;
          int num1 = this.loan.NewAdditionalVestingParty();
          if (num1 == -1)
            return;
          int num2 = num1 + 1;
          this.SetVal(str + "99", this.Val("TR" + num2.ToString("00") + "10"));
          this.SetVal("TR" + num2.ToString("00") + "99", this.Val(str + "98"));
          string val1 = Utils.FormatBorrowerNames(this.Val(str + "01"), this.Val(str + "02"), this.Val(str + "03"), this.Val(str + "04"));
          this.SetVal("TR" + num2.ToString("00") + "01", val1);
          this.SetVal("TR" + num2.ToString("00") + "04", this.Val(str + "09"));
          this.SetVal("TR" + num2.ToString("00") + "12", this.Val(str + "16"));
          BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
          if (borrowerPairs != null && borrowerPairs.Length != 0)
            this.SetVal("TR" + num2.ToString("00") + "05", borrowerPairs[0].Borrower.Id);
          this.SetVal("NewVestingNboAlert", "Y");
        }
        else
        {
          int vestingLinkedNbo = this.loan.GetVestingLinkedNBO(vestingGUID);
          if (vestingLinkedNbo <= 0)
            return;
          if (id.EndsWith("01") || id.EndsWith("02") || id.EndsWith("03") || id.EndsWith("04"))
          {
            string val2 = Utils.FormatBorrowerNames(this.Val(str + "01"), this.Val(str + "02"), this.Val(str + "03"), this.Val(str + "04"));
            this.SetVal("TR" + vestingLinkedNbo.ToString("00") + "01", val2);
          }
          else if (id.EndsWith("09"))
          {
            this.SetVal("TR" + vestingLinkedNbo.ToString("00") + "04", this.Val(str + "09"));
          }
          else
          {
            if (!id.EndsWith("16"))
              return;
            this.SetVal("TR" + vestingLinkedNbo.ToString("00") + "12", this.Val(str + "16"));
          }
        }
      }
      else
      {
        if (!id.StartsWith("TR") || id.Length < 6)
          return;
        string str = "TR" + this.getRepetableIDIndex(id, 2).ToString("00");
        string nboGUID = this.Val(str + "99");
        if (nboGUID == "")
          return;
        int nboLinkedVesting = this.loan.GetNBOLinkedVesting(nboGUID);
        if (nboLinkedVesting == 0)
          return;
        if (id.EndsWith("04"))
        {
          this.SetVal("NBOC" + nboLinkedVesting.ToString("00") + "09", this.Val(str + "04"));
        }
        else
        {
          if (!id.EndsWith("12"))
            return;
          this.SetVal("NBOC" + nboLinkedVesting.ToString("00") + "16", this.Val(str + "12"));
        }
      }
    }

    private int getRepetableIDIndex(string id, int prefixLength)
    {
      int repetableIdIndex = int.Parse(id.Substring(prefixLength, 2));
      if (prefixLength == 4 && id.Length > 8 || prefixLength == 2 && id.Length > 6)
        repetableIdIndex = int.Parse(id.Substring(prefixLength, 3));
      return repetableIdIndex;
    }

    private void setRequiredDataForEDS(string id, string val)
    {
      if (id == "EDS.X5")
      {
        if (this.printLOLicenseSetting == null)
          this.printLOLicenseSetting = this.sessionObjects.ServerManager.GetServerSetting("Printing.PrintLOLicense");
        this.SetVal("EDS.X5", (EnableDisableSetting) this.printLOLicenseSetting == EnableDisableSetting.Enabled ? "Y" : "N");
      }
      if (!(id == "LE1.XD8"))
        return;
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

    private void AddFieldHandlerToKeyPricingFields()
    {
      Routine routine = this.RoutineX(new Routine(this.turnOnKeyPricingAlert));
      AlertConfig alertConfig = this.loan.Settings.AlertSetupData.GetAlertConfig(43);
      if (alertConfig == null)
        return;
      if (alertConfig.TriggerFieldList != null)
      {
        foreach (string triggerField in alertConfig.TriggerFieldList)
          this.AddFieldHandler(triggerField, routine);
      }
      RegulationAlert definition = (RegulationAlert) alertConfig.Definition;
      if (definition == null)
        return;
      foreach (AlertTriggerField triggerField in (List<AlertTriggerField>) definition.TriggerFields)
        this.AddFieldHandler(triggerField.FieldID, routine);
    }

    private void turnOnKeyPricingAlert(string id, string val)
    {
      if (!(this.Val("4062") == "Y"))
        return;
      this.SetVal("4062", "N");
    }

    private void updateConfirmAdviceDateAndName(string id, string val)
    {
      if (!(this.Val("4666") == "Y") || this.sessionObjects.CurrentUser == null)
        return;
      this.SetVal("3612", this.sessionObjects.CurrentUser.GetUserInfo().FullName);
      this.SetVal("3613", DateTime.Today.ToString("MM/dd/yyyy"));
    }

    private void calculatePrepaymentBalance(string id, string val)
    {
      if (id == "3925")
      {
        string str = this.Val("CPA.PaymentHistory.NoteDate");
        if (str == "" || str == "//")
          this.SetVal("CPA.PaymentHistory.NoteDate", this.Val("3925"));
      }
      if (!(id == "682"))
        return;
      this.SetVal("CPA.PaymentHistory.FirstBorrowerPaymentDueDate", this.Val("682"));
    }

    private void calculatefirstInvestorPaymentDate(string id, string val)
    {
      if (this.validateAnticipatedPurchaseDate())
      {
        if (!(id == "CPA.PaymentHistory.FirstBorrowerPaymentDueDate") && !(id == "CPA.PaymentHistory.AnticipatedPurchaseDate"))
          return;
        object policySetting = this.sessionObjects.StartupInfo.PolicySettings[(object) "Policies.CutoffCalendarDay"];
        int num = policySetting == null ? 14 : Utils.ParseInt(policySetting, 14);
        DateTime date1 = Utils.ParseDate((object) this.Val("CPA.PaymentHistory.AnticipatedPurchaseDate"));
        DateTime dateTime1 = date1.AddMonths(date1.Day < num ? 1 : 2);
        DateTime dateTime2 = new DateTime(dateTime1.Year, dateTime1.Month, 1);
        DateTime date2 = Utils.ParseDate((object) this.Val("CPA.PaymentHistory.FirstBorrowerPaymentDueDate"));
        if (dateTime2 < date2)
          dateTime2 = date2;
        this.SetVal("CPA.PaymentHistory.FirstInvestorPaymentDate", dateTime2.ToString());
      }
      else
        this.SetVal("CPA.PaymentHistory.AnticipatedPurchaseDate", "");
    }

    private bool validateAnticipatedPurchaseDate()
    {
      bool flag = true;
      DateTime date1 = Utils.ParseDate((object) this.Val("CPA.PaymentHistory.NoteDate"));
      DateTime date2 = Utils.ParseDate((object) this.Val("CPA.PaymentHistory.FirstBorrowerPaymentDueDate"));
      DateTime date3 = Utils.ParseDate((object) this.Val("CPA.PaymentHistory.AnticipatedPurchaseDate"));
      if ((date1 == DateTime.MinValue || date2 == DateTime.MinValue) && date3 != DateTime.MinValue)
        flag = false;
      if (date3 != DateTime.MinValue)
      {
        if (date3 < date1)
          flag = false;
        if (date3 > DateTime.Today.AddMonths(1))
          flag = false;
      }
      else
        flag = false;
      if (flag)
        return true;
      this.SetVal("CPA.PaymentHistory.AnticipatedPurchaseDate", "");
      this.SetVal("CPA.PaymentHistory.FirstInvestorPaymentDate", "");
      this.SetVal("CPA.PaymentHistory.CalculatedPurchasedPrincipal", "");
      return false;
    }

    private void calculateAmortization(string id, string val) => this.calculateAmortization();

    internal List<string[]> getPaymentSchedule()
    {
      ToolCalculation.PaymentHistorySchedule[] amortization = this.calculateAmortization();
      if (amortization == null || amortization.Length == 0)
        return (List<string[]>) null;
      List<string[]> paymentSchedule = new List<string[]>();
      for (int index = 0; index < amortization.Length && amortization[index] != null; ++index)
        paymentSchedule.Add(new List<string>()
        {
          amortization[index].PaymentNumber.ToString(),
          amortization[index].PaymentDate.ToString("MM/dd/yyyy"),
          amortization[index].BegBalance.ToString("N2"),
          amortization[index].SchedPayment.ToString("N2"),
          amortization[index].ExtraPrincipal.ToString("N2"),
          amortization[index].TotalPayment.ToString("N2"),
          amortization[index].Principal.ToString("N2"),
          amortization[index].Interest.ToString("N2"),
          amortization[index].UPDBalance.ToString("N2")
        }.ToArray());
      return paymentSchedule;
    }

    private ToolCalculation.PaymentHistorySchedule[] calculateAmortization()
    {
      DateTime minValue = DateTime.MinValue;
      ToolCalculation.PaymentHistorySchedule[] amortization = new ToolCalculation.PaymentHistorySchedule[24];
      double Principal = this.FltVal("2");
      double num1 = this.FltVal("3");
      int NumPayments = this.IntVal("4");
      DateTime date1 = Utils.ParseDate((object) this.Val("CPA.PaymentHistory.FirstBorrowerPaymentDueDate"));
      double num2 = this.FltVal("CPA.PaymentHistory.PricipalReduction");
      DateTime date2 = Utils.ParseDate((object) this.Val("CPA.PaymentHistory.FirstInvestorPaymentDate"));
      if (Principal == 0.0 || num1 == 0.0 || NumPayments == 0)
        return (ToolCalculation.PaymentHistorySchedule[]) null;
      Decimal monthlyInterst = (Decimal) num1 / 1200M;
      object policySetting = this.sessionObjects.StartupInfo.PolicySettings[(object) "Policies.NumberOfMonths"];
      int num3 = policySetting == null ? 24 : Utils.ParseInt(policySetting, 24);
      this.SetVal("CPA.PaymentHistory.CalculatedPurchasedPrincipal", string.Empty);
      if (date1 == DateTime.MinValue)
        return (ToolCalculation.PaymentHistorySchedule[]) null;
      DateTime date3 = date1.Date;
      if (date3.Day != 1)
        return (ToolCalculation.PaymentHistorySchedule[]) null;
      if (date2 == DateTime.MinValue)
        return (ToolCalculation.PaymentHistorySchedule[]) null;
      Decimal monthlyPayment = this.CalculateMonthlyPayment((Decimal) Principal, monthlyInterst, NumPayments);
      double num4 = Principal - num2;
      try
      {
        Decimal num5 = (Decimal) num4;
        DateTime start = date3;
        int index1 = 0;
        for (int index2 = 1; index2 <= num3; ++index2)
        {
          DateTime end = start.AddMonths(1);
          double paymentsForPeriod = this.getExtraPaymentsForPeriod(start, end);
          Decimal num6 = Math.Round(monthlyInterst * num5, 2);
          Decimal num7 = monthlyPayment + (Decimal) paymentsForPeriod - num6;
          Decimal num8 = num5 - num7;
          amortization[index1] = new ToolCalculation.PaymentHistorySchedule();
          amortization[index1].PaymentNumber = index2;
          amortization[index1].PaymentDate = start;
          amortization[index1].BegBalance = num5;
          amortization[index1].SchedPayment = monthlyPayment;
          amortization[index1].ExtraPrincipal = paymentsForPeriod;
          amortization[index1].TotalPayment = monthlyPayment + (Decimal) paymentsForPeriod;
          amortization[index1].Principal = num7;
          amortization[index1].Interest = num6;
          amortization[index1].UPDBalance = num8;
          if (date2 == start)
            this.SetVal("CPA.PaymentHistory.CalculatedPurchasedPrincipal", num5.ToString("C2").TrimStart('$'));
          start = end;
          num5 = num8;
          ++index1;
        }
      }
      catch
      {
      }
      return amortization;
    }

    private Decimal CalculateMonthlyPayment(
      Decimal Principal,
      Decimal monthlyInterst,
      int NumPayments)
    {
      Decimal num = (Decimal) Math.Pow((double) (1.0M + monthlyInterst), (double) NumPayments);
      return Math.Round(monthlyInterst * Principal * num / (num - 1M), 2);
    }

    private double getExtraPaymentsForPeriod(DateTime start, DateTime end)
    {
      double paymentsForPeriod = 0.0;
      for (int index = 1; index <= 11; ++index)
      {
        DateTime date = Utils.ParseDate((object) this.Val("CPA.PaymentHistory.ExtraPayment.Date." + index.ToString("00")));
        double num = this.FltVal("CPA.PaymentHistory.ExtraPayment.Amount." + index.ToString("00"));
        if (date >= start && date < end)
          paymentsForPeriod += num;
      }
      return paymentsForPeriod;
    }

    private void calculateConsumerHIOrderEligible(string id, string val)
    {
      if (!this.policyEnableGEICOIntegration || !string.IsNullOrEmpty(this.Val("ConsumerHIOrderEligible")))
        return;
      bool flag1 = this.Val("3164") == "Y";
      string str1 = this.Val("11").Trim();
      bool flag2 = !string.IsNullOrWhiteSpace(str1);
      if (flag2 && str1.ToLower().Replace(" ", string.Empty).IndexOf("tbd") > -1)
        flag2 = false;
      bool flag3 = !string.IsNullOrWhiteSpace(this.Val("ConsumerConnectSiteID"));
      BorrowerPair[] borrowerPairs = this.loan.GetBorrowerPairs();
      bool flag4 = !string.IsNullOrWhiteSpace(this.Val("4000", borrowerPairs[0]));
      bool flag5 = !string.IsNullOrWhiteSpace(this.Val("4002", borrowerPairs[0]));
      bool flag6 = !string.IsNullOrWhiteSpace(this.Val("1240", borrowerPairs[0]));
      bool flag7 = !string.IsNullOrWhiteSpace(this.Val("66", borrowerPairs[0])) || !string.IsNullOrWhiteSpace(this.Val("1490", borrowerPairs[0])) || !string.IsNullOrWhiteSpace(this.Val("4533", borrowerPairs[0]));
      string str2 = this.Val("1402", borrowerPairs[0]);
      bool flag8 = !string.IsNullOrWhiteSpace(str2) && "//" != str2;
      if (!(flag1 & flag2 & flag3 & flag4 & flag5 & flag6 & flag7 & flag8))
        return;
      this.SetVal("ConsumerHIOrderEligible", "Y");
    }

    private void calculateFloodInsuranceAgentName(string id, string val)
    {
      switch (id)
      {
        case "VEND.X13":
          this.SetVal("DISCLOSURE.X1219", val);
          break;
        case "DISCLOSURE.X1219":
          this.SetVal("VEND.X13", val);
          break;
      }
    }

    public class PaymentHistorySchedule
    {
      public PaymentHistorySchedule()
      {
        this.PaymentNumber = 0;
        this.PaymentDate = DateTime.MinValue;
        this.BegBalance = 0M;
        this.SchedPayment = 0M;
        this.ExtraPrincipal = 0.0;
        this.TotalPayment = 0M;
        this.Principal = 0M;
        this.Interest = 0M;
        this.UPDBalance = 0M;
      }

      public int PaymentNumber { set; get; }

      public DateTime PaymentDate { set; get; }

      public Decimal BegBalance { set; get; }

      public Decimal SchedPayment { set; get; }

      public double ExtraPrincipal { set; get; }

      public Decimal TotalPayment { set; get; }

      public Decimal Principal { set; get; }

      public Decimal Interest { set; get; }

      public Decimal UPDBalance { set; get; }
    }
  }
}
