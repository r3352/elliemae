// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.CalculationEngine.Calculation
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.LoanUtils.CalculationServants;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.CalculationEngine
{
  internal class Calculation : CalculationBase
  {
    private const string className = "Calculation�";
    private static readonly string sw = Tracing.SwDataEngine;
    internal Routine CalcLTV;
    internal Routine CalcOthers;
    internal Routine CalcMortgageInsurance;
    internal Routine CalcLifeInsurance;
    internal Routine CalcVAAccount;
    internal Routine CalcIsLSSecondaryFile;
    internal Routine CalcLoanLinkSyncType;
    internal Routine CalcUnlinkedRemote;
    internal Routine PopulateSubjectPropertyAddress;
    internal Routine UpdateBorrowerVestingName;
    internal Routine UpdateCoborrowerVestingName;
    internal Routine CalcHelocDrawAmount;
    internal Routine CalcHelocLoanAmount;
    internal Routine CalcValidDrawAmount;
    internal Routine CalcVerifAccountName;
    internal Routine CalcLoanSatatusLastUpdatedDateTime;
    internal Routine CalcCorrespondentPublishedDate;
    internal Routine CalcPropertyEstateType;
    internal Routine CalcLomaOrLomrIndicator;
    internal Routine Calc4506CPrintVersion;
    internal Routine UpdateIndexRatePrecision;
    internal Routine CalcAMILimit;
    internal Routine CalcHomeCounseling;
    internal Routine CalcDisclosureReadyDate;
    internal Routine PopulateAMILimits;
    internal Routine ClearAMILimits;
    internal Routine ClearMFILimits;
    internal Routine CalcAtAppDisclosureDate;
    internal Routine CalcAtLockDisclosureDate;
    internal Routine CalcChangeCircumstanceRequirementsDate;
    internal Routine CalcChangesReceivedDate;
    internal Routine PopulateMFILimits;
    internal Routine CalcLockinExtensionDays;
    internal Validation LE1X9Validation;
    private SessionObjects sessionObjects;
    private GfeCalculationServant _gfeCalculationServant;
    private List<KeyValuePair<string, string>> serviceList;
    private List<KeyValuePair<string, string>> languageList;

    internal Calculation(SessionObjects sessionObjects, LoanData l, EllieMae.EMLite.CalculationEngine.CalculationObjects calObjs)
      : base(l, calObjs)
    {
      this.sessionObjects = sessionObjects;
      l.BorrowerPairChanged += new LoanDataEventHandler(this.onBorrowerPairChanged);
      l.LienPositionChanged += new LoanDataEventHandler(this.onLienPositionChanged);
      l.VestingChanged += new LoanDataEventHandler(this.onVestingChanged);
      this._gfeCalculationServant = new GfeCalculationServant((ILoanModelProvider) this.calObjs.GFECal, (ISettingsProvider) new SystemSettings(sessionObjects));
      this.addFieldHandlers(l);
      this.addFieldValidations();
    }

    private void addFieldHandlers(LoanData l)
    {
      this.CalcLTV = this.RoutineX(new Routine(this.calculateLTV));
      this.CalcLifeInsurance = this.RoutineX(new Routine(this.calculateLifeInsurance));
      this.CalcMortgageInsurance = this.RoutineX(new Routine(this.calculateMortgageInsurance));
      this.CalcVAAccount = this.RoutineX(new Routine(this.updateVAAccount));
      this.CalcIsLSSecondaryFile = this.RoutineX(new Routine(this.calculateIsLSSecondaryFile));
      this.CalcLoanLinkSyncType = this.RoutineX(new Routine(this.calculateLoanLinkSyncType));
      this.CalcUnlinkedRemote = this.RoutineX(new Routine(this.calculateUnlinkedRemote));
      this.PopulateSubjectPropertyAddress = this.RoutineX(new Routine(this.populateSubjectPropertyAddress));
      this.UpdateBorrowerVestingName = this.RoutineX(new Routine(this.updateBorrowerVestingName));
      this.UpdateCoborrowerVestingName = this.RoutineX(new Routine(this.updateCoborrowerVestingName));
      this.CalcHelocDrawAmount = this.RoutineX(new Routine(this.calculateHelocDrawAmount));
      this.CalcHelocLoanAmount = this.RoutineX(new Routine(this.calculateHelocLoanAmount));
      this.CalcValidDrawAmount = this.RoutineX(new Routine(this.calculateValidDrawAmount));
      this.CalcVerifAccountName = this.RoutineX(new Routine(this.calculateLiabilityAccountName));
      this.CalcLoanSatatusLastUpdatedDateTime = this.RoutineX(new Routine(this.calculateLoanStatusLastUpdatedDateTime));
      this.CalcCorrespondentPublishedDate = this.RoutineX(new Routine(this.calculateCorrespondentPublishedDate));
      this.CalcPropertyEstateType = this.RoutineX(new Routine(this.calculatePropertyEstateType));
      this.CalcLomaOrLomrIndicator = this.RoutineX(new Routine(this.calculateLomaOrLomrIndicator));
      this.Calc4506CPrintVersion = this.RoutineX(new Routine(this.calculate4506CPrintVersion));
      this.UpdateIndexRatePrecision = this.RoutineX(new Routine(this.updateIndexRatePrecision));
      this.CalcAMILimit = this.RoutineX(new Routine(this.calculateAMILimit));
      this.CalcHomeCounseling = this.RoutineX(new Routine(this.calculateHomeCounseling));
      this.CalcDisclosureReadyDate = this.RoutineX(new Routine(this.calculateDisclosureReadyDate));
      this.PopulateAMILimits = this.RoutineX(new Routine(this.populateAMILimits));
      this.ClearAMILimits = this.RoutineX(new Routine(this.clearAMILimits));
      this.ClearMFILimits = this.RoutineX(new Routine(this.clearMFILimits));
      this.CalcAtAppDisclosureDate = this.RoutineX(new Routine(this.calculateAtAppDisclosureDate));
      this.CalcAtLockDisclosureDate = this.RoutineX(new Routine(this.calculateAtLockDisclosureDate));
      this.CalcChangeCircumstanceRequirementsDate = this.RoutineX(new Routine(this.calculatChangeCircumstanceRequirementsDate));
      this.CalcChangesReceivedDate = this.RoutineX(new Routine(this.calculateChangesReceivedDate));
      this.PopulateMFILimits = this.RoutineX(new Routine(this.populateMFILimits));
      this.CalcLockinExtensionDays = this.RoutineX(new Routine(this.calculateLockinExtensionDays));
      Routine routine1 = this.RoutineX(new Routine(this.calculateLockDate));
      this.AddFieldHandler("761", routine1 + this.calObjs.NewHudCal.CalcHUDGFEDisclosureInfo + this.calObjs.NewHudCal.CalcREGZGFEHud + this.calObjs.NewHud2015Cal.CalcLoanEstimate_ClosingCostExpDate);
      this.AddFieldHandler("762", routine1 + this.calObjs.NewHudCal.CalcHUDGFEDisclosureInfo + this.calObjs.NewHudCal.CalcREGZGFEHud + this.calObjs.NewHud2015Cal.CalcLoanEstimate_ClosingCostExpDate);
      this.AddFieldHandler("432", routine1 + this.calObjs.NewHudCal.CalcHUDGFEDisclosureInfo + this.calObjs.NewHudCal.CalcREGZGFEHud + this.calObjs.NewHud2015Cal.CalcLoanEstimate_ClosingCostExpDate);
      this.AddFieldHandler("2089", routine1);
      this.AddFieldHandler("2090", routine1);
      this.AddFieldHandler("2091", routine1);
      this.AddFieldHandler("4493", this.RoutineX(new Routine(this.calculatePiggyBackFields) + this.calObjs.D1003URLA2020Cal.UpdateLinkedVoal + this.calObjs.VERIFCal.CalcHelocLineTotal));
      this.AddFieldHandler("CORRESPONDENT.X53", this.RoutineX(this.CalcLoanSatatusLastUpdatedDateTime));
      this.AddFieldHandler("CORRESPONDENT.X55", this.RoutineX(this.CalcCorrespondentPublishedDate));
      Routine routine2 = this.RoutineX(this.CalcPropertyEstateType);
      this.AddFieldHandler("1825", routine2);
      this.AddFieldHandler("33", routine2);
      this.AddFieldHandler("URLA.X138", routine2);
      this.AddFieldHandler("1066", this.RoutineX(this.CalcPropertyEstateType + this.CalcLTV));
      Routine routine3 = this.RoutineX(new Routine(this.calculateOthers));
      this.CalcOthers = routine3;
      this.AddFieldHandler("IRS4506.X52", routine3);
      this.AddFieldHandler("AR0052", routine3);
      this.AddFieldHandler("IRS4506.X31", routine3);
      this.AddFieldHandler("IRS4506.X32", routine3);
      this.AddFieldHandler("AR0031", routine3);
      this.AddFieldHandler("AR0032", routine3);
      this.AddFieldHandler("IRS4506.X1", routine3);
      this.AddFieldHandler("IR0001", routine3);
      this.AddFieldHandler("AR0001", routine3);
      this.AddFieldHandler("MORNET.X4", routine3);
      this.AddFieldHandler("CASASRN.X13", routine3);
      this.AddFieldHandler("IR0093", routine3);
      this.AddFieldHandler("IRS4506.X57", routine3);
      this.AddFieldHandler("IR0057", routine3);
      this.AddFieldHandler("AR0057", routine3);
      this.AddFieldHandler("IRS4506.X58", routine3);
      this.AddFieldHandler("IR0058", routine3);
      this.AddFieldHandler("AR0058", routine3);
      this.AddFieldHandler("FL0015", this.RoutineX(this.calObjs.VERIFCal.CalcVOMOwnedBy + this.CalcVerifAccountName));
      this.AddFieldHandler("DD0024", this.RoutineX(this.calObjs.D1003URLA2020Cal.CalcVODTotalDeposits + this.CalcVerifAccountName));
      Routine routine4 = this.RoutineX(new Routine(this.updateNamesInRequest));
      this.AddFieldHandler("REQUEST.X25", routine4);
      this.AddFieldHandler("66", routine4 + this.calObjs.ToolCal.CalcConsumerHIOrderEligible);
      this.AddFieldHandler("FE0117", routine4);
      this.AddFieldHandler("1490", routine4 + this.calObjs.ToolCal.CalcConsumerHIOrderEligible);
      this.AddFieldHandler("98", routine4);
      this.AddFieldHandler("FE0217", routine4);
      this.AddFieldHandler("1480", routine4);
      this.AddFieldHandler("1268", routine4);
      Routine routine5 = this.RoutineX(this.calObjs.ToolCal.CalcConsumerHIOrderEligible);
      this.AddFieldHandler("1240", routine5);
      this.AddFieldHandler("ConsumerConnectSiteID", routine5);
      this.AddFieldHandler("TQL.X108", this.RoutineX(new Routine(this.calculateLomaOrLomrIndicator)));
      this.AddFieldHandler("2", this.calObjs.GFECal.CalcGFEFees + new Routine(this.calObjs.ToolCal.UpdatePurchaseAdviceBalance) + this.calObjs.GFECal.CalcPrepaid + this.calObjs.NewHud2015FeeDetailCal.Calc_2015AllFeeDetails + this.calObjs.USDACal.CalcLoanFundsUsage + this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.RegzCal.CalculateHELOCPayment + this.calObjs.RegzCal.CalcHELOCInitialPayment + this.calObjs.GFECal.CalcTotalCosts + this.calObjs.D1003URLA2020Cal.CalcOtherCredits + this.calObjs.PrequalCal.CalcRentOwn + this.calObjs.FHACal.CalcMACAWP + this.calObjs.VACal.CalcVALA + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.LoansubCal.CalcLoansub + this.CalcLTV + this.calObjs.ATRQMCal.CalcDiscountPointPercent + this.calObjs.ATRQMCal.CalcDiscountPoints + this.calObjs.VACal.CalcVARecoupment + this.calObjs.NewHud2015Cal.CalcLoanEstimateEstimatedCashToCloseAV + this.calObjs.NewHud2015Cal.CalcLoanEstimate_ClosingCostsFinanced + this.calObjs.NewHud2015Cal.CalcLoanEstimateDownPaymentAndFundsForBorrower + this.calObjs.NewHud2015Cal.CalcCDPage3TotalL + this.calObjs.NewHud2015Cal.CalcCDPage3AlternateCashToCloseFinal + this.calObjs.NewHud2015Cal.CalcCDPage3StdCashToCloseFinal + this.calObjs.ToolCal.UpdateCorrespondentPrincipal + this.calObjs.NewHud2015Cal.CalcCDPage3StdCashToCloseFinal + this.calObjs.ATRQMCal.CalcPointsAndFees + this.calObjs.HMDACal.CalcLoanAmount + this.calObjs.VACal.CalcVACashOutRefinance + this.calObjs.VACal.CalcVAStatutoryRecoupment + this.calObjs.D1003URLA2020Cal.CalcTotalNewMortgageLoans + this.calObjs.D1003URLA2020Cal.UpdateLinkedVoal + this.calObjs.VERIFCal.CalcHelocLineTotal + this.calObjs.ToolCal.CalcAmortization);
      this.AddFieldHandler("3", this.calObjs.RegzCal.CalcZeroPercentPaymentOption + this.calObjs.RegzCal.SychConstructionRateNoteRate + this.calObjs.FHACal.CalcEEMWorksheet + this.calObjs.RegzCal.CalcFloorRate + this.calObjs.GFECal.CalcGFEFees + this.calObjs.GFECal.CalcPrepaid + this.calObjs.RegzCal.CalcAPR + this.calObjs.RegzCal.CalculateHELOCPayment + this.calObjs.RegzCal.CalcHELOCInitialPayment + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.GFECal.CalcTotalCosts + this.calObjs.D1003URLA2020Cal.CalcOtherCredits + this.calObjs.PrequalCal.CalcRentOwn + this.calObjs.FHACal.CalcMACAWP + this.calObjs.VACal.CalcVALA + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.LoansubCal.CalcLoansub + this.calObjs.PrequalCal.CalcMinMax + new Routine(this.calculateUCD) + this.CalcLTV + this.calObjs.RegzCal.CalcFirstAdjustmentMinimum + this.calObjs.ATRQMCal.CalcDiscountPoints + this.calObjs.GFECal.CalcSection32 + this.calObjs.ToolCal.CalcValueForIntrestRateConditions + this.calObjs.VACal.CalcVARecoupment + this.calObjs.NewHud2015Cal.CalculateProductDescription + this.calObjs.NewHud2015Cal.CalcLoanTermTable_InterestRate + this.calObjs.NewHud2015Cal.SetAIRTableIndexType + this.calObjs.RegzCal.CalcInitialInterestRate + this.calObjs.HMDACal.CalcInterestRate + this.calObjs.NewHud2015Cal.CalcLECDDisplayFields + this.calObjs.HMDACal.CalcIntroRatePeriod + this.calObjs.VACal.CalcVACashOutRefinance + this.calObjs.VACal.CalcVAStatutoryRecoupment + this.calObjs.HelocCal.CalcHELOCPeriodicRates + this.calObjs.ToolCal.CalcAmortization);
      this.AddFieldHandler("DISCLOSURE.X478", this.calObjs.ToolCal.UpdateDisclosureDeterminedField);
      this.AddFieldHandler("DISCLOSURE.X483", this.calObjs.ToolCal.UpdateDisclosureExplanationFields);
      this.AddFieldHandler("4", this.calObjs.FHACal.CalcEEMWorksheet + this.calObjs.D1003Cal.PopulateOtherFields + this.calObjs.GFECal.CalcGFEFees + this.calObjs.GFECal.CalcPrepaid + this.calObjs.RegzCal.CalcAPR + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.GFECal.CalcTotalCosts + this.calObjs.D1003URLA2020Cal.CalcOtherCredits + this.calObjs.PrequalCal.CalcRentOwn + this.calObjs.FHACal.CalcMACAWP + this.calObjs.VACal.CalcVALA + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.LoansubCal.CalcLoansub + this.calObjs.PrequalCal.CalcMinMax + new Routine(this.calculateUCD) + this.CalcLTV + this.calObjs.RegzCal.CalcPMIMidpointCancelationDate + this.calObjs.ATRQMCal.CalcEligibility + this.calObjs.VACal.CalcVARecoupment + this.calObjs.NewHud2015Cal.CalculateProductDescription + this.calObjs.NewHud2015Cal.CalcLoanTermTable_LoanTerm + this.calObjs.HMDACal.CalcLoanTerm + this.calObjs.NewHud2015Cal.Calc60Payments + this.calObjs.VACal.CalcVACashOutRefinance + this.calObjs.VACal.CalcVAStatutoryRecoupment + this.calObjs.ToolCal.CalcAmortization + this.calObjs.D1003URLA2020Cal.CalcEstimatedNetMonthlyRent + this.calObjs.NewHud2015Cal.CalcAPTable_InterestOnlyPayments + this.calObjs.NewHud2015Cal.CalcConstructionPermPeriod);
      this.AddFieldHandler("5", this.calObjs.RegzCal.CalcAPR + this.calObjs.VACal.CalcVALA + this.CalcLifeInsurance + this.calObjs.RegzCal.CalcREGZSummary + this.calObjs.ToolCal.CalcNetTangibleBenefit + this.calObjs.HMDACal.CalcPrepaymentPenaltyPeriod + this.calObjs.ATRQMCal.CalcMax5YearsPandI + this.calObjs.D1003URLA2020Cal.UpdateLinkedVoal + this.calObjs.VERIFCal.CalcHelocLineTotal + this.calObjs.VACal.CalcVARecoupment + this.calObjs.ATRQMCal.CalcEligibility);
      this.AddFieldHandler("13", this.calObjs.D1003Cal.PopulateOtherFields + this.calObjs.ToolCal.PopulateCountyLimit + this.calObjs.D1003Cal.CopyCountyToJurisdiction + this.calObjs.HMDACal.CalcCountyCode + this.calObjs.HMDACal.CalcHMDACountyCensusTrackCode + this.PopulateAMILimits + this.calObjs.HMDACal.CalculateMSACode + this.PopulateMFILimits);
      this.AddFieldHandler("14", this.calObjs.FHACal.CalcMACAWP + this.calObjs.MLDSCal.CalcMLDSScenarios + this.CalcOthers + this.calObjs.ToolCal.PopulateCountyLimit + this.calObjs.GFECal.CalcOthers + this.calObjs.ToolCal.CalcNetTangibleBenefit + this.calObjs.HMDACal.CalcPrepaymentPenaltyPeriod + this.calObjs.D1003Cal.CopyCountyToJurisdiction + this.calObjs.D1003Cal.CalcCompanyStateLicense + this.calObjs.HMDACal.CalcCountyCode + this.calObjs.HMDACal.CalcHMDACountyCensusTrackCode + this.calObjs.ToolCal.CalcMICancelCondTypeAndRMLA + this.calObjs.NewHudCal.EnforceGFEAppDateRule + this.calObjs.NewHudCal.CalcInitialDisclosureDueDate + this.CalcHomeCounseling + this.PopulateAMILimits + this.calObjs.HMDACal.CalculateMSACode + this.PopulateMFILimits);
      this.AddFieldHandler("15", this.calObjs.FHACal.CalcMACAWP + this.calObjs.MLDSCal.CalcMLDSScenarios + this.calObjs.HMDACal.CalcPrepaymentPenaltyPeriod + this.CalcOthers + this.calObjs.NewHudCal.EnforceGFEAppDateRule + this.calObjs.NewHudCal.CalcInitialDisclosureDueDate + this.calObjs.HMDACal.CalcCountyCode + this.calObjs.HMDACal.CalcHMDACountyCensusTrackCode + this.CalcHomeCounseling + this.PopulateAMILimits + this.calObjs.HMDACal.CalculateMSACode + this.PopulateMFILimits);
      Routine routine6 = new Routine((this.calObjs.RegzCal.SychConstructionRateNoteRate + this.calObjs.RegzCal.CalcLotLandStatus + this.calObjs.Hud1Cal.CalcEscrowFirstPaymentDate + this.calObjs.NewHud2015Cal.CalcConstructionPermPeriod + this.calObjs.D1003Cal.PopulateOtherFields + this.calObjs.PrequalCal.ClearLockRequestFreeAndClear + this.calObjs.USDACal.CalcRefinanceIndicator + this.calObjs.PreCal.UpdateF19 + this.calObjs.NewHud2015Cal.CalcWillHaveEscrowAccount + this.calObjs.NewHud2015Cal.CalcAsCompletedPurchasePrice + this.calObjs.NewHud2015Cal.CalcAsCompletedAppraisedValue + this.calObjs.NewHud2015Cal.SetCustomizedProjectedPaymentTable + this.calObjs.FHACal.CalcExisting23KDebt + this.calObjs.FHACal.CalcMAX23K + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.GFECal.CalcGFEFees + this.calObjs.ToolCal.CalcSafeHarbor + this.calObjs.D1003Cal.AdjConstTotal + this.calObjs.GFECal.CalcSection32 + this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.Hud1Cal.CalcHUD1ES + this.calObjs.RegzCal.CalcAPR + this.CalcLTV + this.calObjs.PrequalCal.CalcCopyConstructionFields + this.calObjs.PrequalCal.CalcLockRequestLoan + new Routine(this.calObjs.GFECal.UpdateValueExistingJuniorLien) + this.calObjs.NewHudCal.CalcGFEApplicationDate + this.calObjs.RegzCal.CalcLateExemptRightRescissionFlag + this.calObjs.NewHud2015Cal.PopulateLoanEstimate_LoanPurpose + this.calObjs.NewHud2015Cal.CalcLoanEstimateThirdPartyPaymentsNotOtherwiseDisclosed + this.calObjs.NewHud2015Cal.CalcLoanEstimate_ClosingCostsFinanced + this.calObjs.NewHud2015Cal.CalcCDPage3TotalPurchasePayoffsIncluded + this.calObjs.NewHud2015Cal.CalcCdPage3Liabilities + this.calObjs.NewHud2015Cal.CalcLoanEstimateDownPaymentAndFundsForBorrower + this.calObjs.NewHud2015Cal.CalcAdjustmentsUcdTotal + this.calObjs.NewHud2015Cal.CalcCDPage3StdCashToCloseFinal + this.calObjs.NewHud2015Cal.CalcAPTable_FirstChangePayment + this.calObjs.NewHud2015Cal.CalcAPTable_MaximumPayment + this.calObjs.HMDACal.CalcLoanTerm + this.calObjs.RegzCal.CalcConstructionPhaseDisclosedSeparately + this.calObjs.NewHud2015Cal.CalculateProductDescription + this.calObjs.RegzCal.CalcInitialInterestRate + this.calObjs.NewHud2015Cal.CalcLoanTermTable_InterestRate + this.calObjs.NewHud2015Cal.SetAIRTableIndexType + this.calObjs.NewHud2015Cal.CalcClosingDisclosureTotalPayments + this.calObjs.NewHud2015Cal.CalcAsCompletedAppraisedValue + this.CalcIsLSSecondaryFile + this.CalcLoanLinkSyncType + this.calObjs.HMDACal.CalcHMDAReporting + this.calObjs.HMDACal.CalcIntroRatePeriod + this.calObjs.HMDACal.CalcHmdaInterestOnlyIndicator + this.calObjs.HMDACal.CalcNewHmdaInterestOnlyIndicator + this.calObjs.NewHud2015Cal.ClearPostConsummationSection + this.calObjs.VERIFCal.CalcHelocLineTotal + this.calObjs.HMDACal.CalcLoanAmount + this.calObjs.HMDACal.CalcLoanTerm + this.calObjs.RegzCal.CalcPMIMidpointCancelationDate + this.calObjs.D1003URLA2020Cal.CalcURLALoanPurposeMapping + this.calObjs.D1003URLA2020Cal.CalcConstructionLoanIndicator + this.calObjs.D1003URLA2020Cal.ClearConstructionLoanTypes + this.calObjs.NewHudCal.EnforceGFEAppDateRule + this.calObjs.NewHudCal.CalcInitialDisclosureDueDate + this.calObjs.NewHud2015Cal.CalcLoanEstimate_IncludePayoffs + this.calObjs.D1003URLA2020Cal.CalcEstimatedNetMonthlyRent).Invoke);
      this.AddFieldHandler("19", this.calObjs.RegzCal.CalcZeroPercentPaymentOption + routine6);
      this.AddFieldHandler("4746", routine6);
      this.AddFieldHandler("21", this.calObjs.NewHud2015Cal.CalcDisclosedSalesPrice + this.calObjs.NewHud2015Cal.CalcAsCompletedPurchasePrice + this.calObjs.GFECal.CalcPrepaid + this.calObjs.NewHud2015Cal.Calc2015LineItem904 + this.CalcLTV + this.calObjs.PrequalCal.CalcCopyConstructionFields + this.calObjs.PrequalCal.CalcLockRequestLoan);
      this.AddFieldHandler("22", this.calObjs.D1003Cal.AdjConstTotal + this.calObjs.NewHud2015Cal.CalcAsCompletedAppraisedValue + this.calObjs.GFECal.CalcPrepaid + this.calObjs.NewHud2015Cal.Calc2015LineItem904 + this.CalcLTV + this.calObjs.PrequalCal.CalcCopyConstructionFields + this.calObjs.PrequalCal.CalcLockRequestLoan);
      this.AddFieldHandler("23", this.calObjs.D1003Cal.AdjConstTotal + this.calObjs.NewHud2015Cal.CalcAsCompletedPurchasePrice + this.calObjs.NewHud2015Cal.CalcAsCompletedAppraisedValue + this.calObjs.GFECal.CalcPrepaid + this.calObjs.NewHud2015Cal.Calc2015LineItem904 + this.CalcLTV + this.calObjs.PrequalCal.CalcCopyConstructionFields + this.calObjs.PrequalCal.CalcLockRequestLoan);
      this.AddFieldHandler("26", this.calObjs.D1003URLA2020Cal.CalcBackEndRatio + this.calObjs.VERIFCal.CalcLiabilities + this.calObjs.VACal.CalcVARRRWS + this.calObjs.GFECal.CalcGFEFees + new Routine(((CalculationBase) this.calObjs.NewHudCal).FormCal) + this.calObjs.ToolCal.CalcSafeHarbor + this.calObjs.FHACal.CalcExisting23KDebt + this.calObjs.FHACal.CalcMAX23K + this.calObjs.FHACal.CalcCAWRefi + this.calObjs.FHACal.CalcFredMac + this.calObjs.NewHud2015Cal.PopulateLoanEstimate_LoanPurpose);
      this.AddFieldHandler("29", this.calObjs.FHACal.CalcExisting23KDebt + this.calObjs.FHACal.CalcMAX23K + this.calObjs.FHACal.CalcCAWRefi);
      this.AddFieldHandler("60", this.calObjs.PrequalCal.CalcMiddleFICO + this.calObjs.PrequalCal.CalcLoanComparison + this.calObjs.D1003Cal.CopyInfoToLockRequest);
      this.AddFieldHandler("67", this.calObjs.PrequalCal.CalcMiddleFICO + this.calObjs.PrequalCal.CalcLoanComparison + this.calObjs.D1003Cal.CopyInfoToLockRequest);
      this.AddFieldHandler("65", this.CalcVAAccount + this.calObjs.D1003Cal.CopyInfoToLockRequest + this.calObjs.NewHudCal.EnforceGFEAppDateRule + this.calObjs.NewHudCal.CalcInitialDisclosureDueDate);
      this.AddFieldHandler("97", this.CalcVAAccount + this.calObjs.D1003Cal.CopyInfoToLockRequest);
      this.AddFieldHandler("101", this.calObjs.D1003Cal.CalcBorrowerIncome + this.calObjs.FHACal.CalcEEMWorksheet + this.calObjs.USDACal.CalcIncomeWorksheet + this.calObjs.VACal.CalcVALA + this.calObjs.FHACal.CalcMACAWP);
      this.AddFieldHandler("102", this.calObjs.D1003Cal.CalcBorrowerIncome + this.calObjs.FHACal.CalcEEMWorksheet + this.calObjs.USDACal.CalcIncomeWorksheet + this.calObjs.VACal.CalcVALA + this.calObjs.FHACal.CalcMACAWP);
      this.AddFieldHandler("103", this.calObjs.D1003Cal.CalcBorrowerIncome + this.calObjs.FHACal.CalcEEMWorksheet + this.calObjs.USDACal.CalcIncomeWorksheet + this.calObjs.VACal.CalcVALA + this.calObjs.FHACal.CalcMACAWP);
      this.AddFieldHandler("104", this.calObjs.D1003Cal.CalcBorrowerIncome + this.calObjs.FHACal.CalcEEMWorksheet + this.calObjs.USDACal.CalcIncomeWorksheet + this.calObjs.VACal.CalcVALA + this.calObjs.FHACal.CalcMACAWP);
      this.AddFieldHandler("105", this.calObjs.D1003Cal.CalcBorrowerIncome + this.calObjs.FHACal.CalcEEMWorksheet + this.calObjs.USDACal.CalcIncomeWorksheet + this.calObjs.VACal.CalcVALA + this.calObjs.FHACal.CalcMACAWP);
      this.AddFieldHandler("106", this.calObjs.D1003Cal.CalcBorrowerIncome + this.calObjs.FHACal.CalcEEMWorksheet + this.calObjs.USDACal.CalcIncomeWorksheet + this.calObjs.FHACal.CalcMACAWP);
      this.AddFieldHandler("107", this.calObjs.D1003Cal.CalcBorrowerIncome + this.calObjs.FHACal.CalcEEMWorksheet + this.calObjs.USDACal.CalcIncomeWorksheet + this.calObjs.VACal.CalcVALA + this.calObjs.FHACal.CalcMACAWP);
      this.AddFieldHandler("108", this.calObjs.D1003Cal.CalcBorrowerIncome + this.calObjs.FHACal.CalcEEMWorksheet + this.calObjs.USDACal.CalcIncomeWorksheet + this.calObjs.VACal.CalcVALA + this.calObjs.FHACal.CalcMACAWP);
      this.AddFieldHandler("110", this.calObjs.D1003Cal.CalcBorrowerIncome + this.calObjs.FHACal.CalcEEMWorksheet + this.calObjs.USDACal.CalcIncomeWorksheet + this.calObjs.VACal.CalcVALA + this.calObjs.FHACal.CalcMACAWP);
      this.AddFieldHandler("111", this.calObjs.D1003Cal.CalcBorrowerIncome + this.calObjs.FHACal.CalcEEMWorksheet + this.calObjs.USDACal.CalcIncomeWorksheet + this.calObjs.VACal.CalcVALA + this.calObjs.FHACal.CalcMACAWP);
      this.AddFieldHandler("112", this.calObjs.D1003Cal.CalcBorrowerIncome + this.calObjs.FHACal.CalcEEMWorksheet + this.calObjs.USDACal.CalcIncomeWorksheet + this.calObjs.VACal.CalcVALA + this.calObjs.FHACal.CalcMACAWP);
      this.AddFieldHandler("113", this.calObjs.D1003Cal.CalcBorrowerIncome + this.calObjs.FHACal.CalcEEMWorksheet + this.calObjs.USDACal.CalcIncomeWorksheet + this.calObjs.VACal.CalcVALA + this.calObjs.FHACal.CalcMACAWP);
      this.AddFieldHandler("114", this.calObjs.D1003Cal.CalcBorrowerIncome + this.calObjs.FHACal.CalcEEMWorksheet + this.calObjs.USDACal.CalcIncomeWorksheet + this.calObjs.VACal.CalcVALA + this.calObjs.FHACal.CalcMACAWP);
      this.AddFieldHandler("115", this.calObjs.D1003Cal.CalcBorrowerIncome + this.calObjs.FHACal.CalcEEMWorksheet + this.calObjs.USDACal.CalcIncomeWorksheet + this.calObjs.VACal.CalcVALA + this.calObjs.FHACal.CalcMACAWP);
      this.AddFieldHandler("116", this.calObjs.D1003Cal.CalcBorrowerIncome + this.calObjs.FHACal.CalcEEMWorksheet + this.calObjs.USDACal.CalcIncomeWorksheet + this.calObjs.VACal.CalcVALA + this.calObjs.FHACal.CalcMACAWP);
      this.AddFieldHandler("117", this.calObjs.D1003Cal.CalcBorrowerIncome + this.calObjs.FHACal.CalcEEMWorksheet + this.calObjs.USDACal.CalcIncomeWorksheet + this.calObjs.VACal.CalcVALA + this.calObjs.FHACal.CalcMACAWP);
      this.AddFieldHandler("119", this.calObjs.D1003URLA2020Cal.CalcPresentHousingExpenseRent + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.PrequalCal.CalcRentOwn);
      this.AddFieldHandler("120", this.calObjs.RegzCal.CalculateHELOCPayment + this.calObjs.D1003URLA2020Cal.CalcTotalMortgageAndTaxes + this.calObjs.D1003Cal.CalcHousingExp);
      this.AddFieldHandler("121", this.calObjs.RegzCal.CalculateHELOCPayment + this.calObjs.D1003URLA2020Cal.CalcTotalMortgageAndTaxes + this.calObjs.D1003Cal.CalcHousingExp);
      this.AddFieldHandler("123", this.calObjs.D1003URLA2020Cal.CalcTotalMortgageAndTaxes + this.calObjs.D1003Cal.CalcHousingExp);
      this.AddFieldHandler("136", this.calObjs.D1003Cal.PopulateOtherFields + this.calObjs.PrequalCal.CalcMIP + this.calObjs.PrequalCal.CalcDownPay + this.calObjs.ToolCal.CalcLOCompensation + new Routine(this.calObjs.ToolCal.UpdatePurchaseAdviceBalance) + this.calObjs.PrequalCal.CalcMIP + this.calObjs.NewHud2015Cal.CalcPrepaidMI + this.calObjs.PrequalCal.CalcDownPay + this.calObjs.RegzCal.CalculateHELOCPayment + this.calObjs.NewHud2015Cal.CalcDisclosedSalesPrice + this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.PrequalCal.CalcHomeMaintenance + this.calObjs.FHACal.CalcFredMac + this.calObjs.FHACal.CalcExisting23KDebt + this.calObjs.FHACal.CalcMAX23K + this.calObjs.VACal.CalcMaximumSellerContribution + this.calObjs.FHACal.CalcMACAWP + this.calObjs.GFECal.CalcPrepaid + this.calObjs.GFECal.CalcGFEFees + this.calObjs.GFECal.CalcOthers + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003URLA2020Cal.CalcTotalMortgageAndTaxes + this.calObjs.MLDSCal.UpdateInsurance + this.calObjs.GFECal.CalcTotalCosts + this.calObjs.D1003URLA2020Cal.CalcOtherCredits + this.calObjs.NewHudCal.CopySpecialHUD2010ToGFE2010 + new Routine(((CalculationBase) this.calObjs.NewHudCal).FormCal) + this.calObjs.ToolCal.CalcProfit + this.calObjs.PrequalCal.CalcRentOwn + this.calObjs.PrequalCal.CalcMinMax + this.calObjs.MLDSCal.CalcMLDSScenarios + this.calObjs.HMDACal.CalcPrepaymentPenaltyPeriod + this.CalcLTV + this.calObjs.PrequalCal.CalcLockRequestLoan + this.calObjs.GFECal.CalcSecondAppraisalRequired + this.calObjs.ATRQMCal.CalcPrepaymentPenaltyPercent + this.calObjs.NewHud2015Cal.CalcCDPage3StdDidChangeCol + this.calObjs.RegzCal.CalcPointsInInitialAdjustedRate);
      this.AddFieldHandler("968", this.CalcLTV);
      this.AddFieldHandler("3894", this.calObjs.ToolCal.PopulateCountyLimit);
      this.AddFieldHandler("137", this.calObjs.GFECal.CalcTotalCosts + this.calObjs.D1003URLA2020Cal.CalcOtherCredits + this.calObjs.ATRQMCal.CalcEligibility + this.calObjs.FHACal.CalcFredMac + this.calObjs.FHACal.CalcExisting23KDebt + this.calObjs.FHACal.CalcMAX23K + this.calObjs.D1003URLA2020Cal.CalcEstimatedClosingCostsAmount);
      this.AddFieldHandler("138", this.calObjs.GFECal.CalcGFEFees + this.calObjs.GFECal.CalcPrepaid + this.calObjs.GFECal.CalcTotalCosts + this.calObjs.D1003URLA2020Cal.CalcOtherCredits + this.calObjs.NewHudCal.CalcREGZGFE + this.calObjs.NewHudCal.CalcHUD1PG2 + this.calObjs.FHACal.CalcExisting23KDebt + this.calObjs.FHACal.CalcMAX23K + this.calObjs.D1003URLA2020Cal.CalcEstimatedClosingCostsAmount);
      this.AddFieldHandler("140", this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + this.calObjs.NewHud2015Cal.CalcLoanEstimateDownPaymentAndFundsForBorrower + this.calObjs.VERIFCal.CalSubFin + this.calObjs.NewHud2015Cal.CalcCDPage3StdCashToCloseFinal);
      this.AddFieldHandler("141", this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + new Routine(this.calculateUCD) + this.calObjs.NewHud2015Cal.CalcLECashToCloseSellerCredit + this.calObjs.NewHud2015Cal.CalcCDPage3StdCashToCloseSellerCredit + this.calObjs.NewHud2015Cal.CalcClosingDisclosureTotalClosingCosts + new Routine(this.calObjs.GFECal.CalculateFunder) + this.calObjs.D1003URLA2020Cal.CalcOtherCredits);
      this.AddFieldHandler("202", this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + new Routine(this.calculateUCD) + this.calObjs.NewHud2015Cal.CalcLECashToCloseSellerCredit + this.calObjs.NewHud2015Cal.CalcCDPage3StdCashToCloseSellerCredit + this.calObjs.NewHud2015Cal.CalcClosingDisclosureTotalClosingCosts + new Routine(this.calObjs.GFECal.CalculateFunder));
      this.AddFieldHandler("1041", this.calObjs.D1003Cal.PopulateOtherFields + this.calObjs.ATRQMCal.CalcEligibility);
      this.AddFieldHandler("1091", this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + new Routine(this.calculateUCD) + this.calObjs.NewHud2015Cal.CalcLECashToCloseSellerCredit + this.calObjs.NewHud2015Cal.CalcCDPage3StdCashToCloseSellerCredit + this.calObjs.NewHud2015Cal.CalcClosingDisclosureTotalClosingCosts + new Routine(this.calObjs.GFECal.CalculateFunder));
      this.AddFieldHandler("1106", this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + new Routine(this.calculateUCD) + this.calObjs.NewHud2015Cal.CalcLECashToCloseSellerCredit + this.calObjs.NewHud2015Cal.CalcCDPage3StdCashToCloseSellerCredit + this.calObjs.NewHud2015Cal.CalcClosingDisclosureTotalClosingCosts + new Routine(this.calObjs.GFECal.CalculateFunder));
      this.AddFieldHandler("1646", this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + new Routine(this.calculateUCD) + this.calObjs.NewHud2015Cal.CalcLECashToCloseSellerCredit + this.calObjs.NewHud2015Cal.CalcCDPage3StdCashToCloseSellerCredit + this.calObjs.NewHud2015Cal.CalcClosingDisclosureTotalClosingCosts + new Routine(this.calObjs.GFECal.CalculateFunder));
      this.AddFieldHandler("1093", this.calObjs.NewHudCal.CalcClosingCost + this.calObjs.GFECal.CalcTotalCosts + this.calObjs.D1003URLA2020Cal.CalcOtherCredits + this.calObjs.ATRQMCal.CalcEligibility + this.calObjs.FHACal.CalcFredMac + this.calObjs.FHACal.CalcExisting23KDebt + this.calObjs.FHACal.CalcMAX23K + this.calObjs.HMDACal.CalcDiscountPoints);
      this.AddFieldHandler("1073", this.calObjs.GFECal.CalcTotalCosts + this.calObjs.D1003URLA2020Cal.CalcOtherCredits + this.calObjs.ATRQMCal.CalcEligibility + this.calObjs.FHACal.CalcFredMac + this.calObjs.FHACal.CalcExisting23KDebt + this.calObjs.FHACal.CalcMAX23K + this.calObjs.HMDACal.CalcDiscountPoints);
      this.AddFieldHandler("1095", this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + new Routine(this.calculateUCD) + this.calObjs.NewHud2015Cal.CalcLECashToCloseSellerCredit + this.calObjs.NewHud2015Cal.CalcCDPage3StdCashToCloseSellerCredit + this.calObjs.NewHud2015Cal.CalcClosingDisclosureTotalClosingCosts + new Routine(this.calObjs.GFECal.CalculateFunder) + this.calObjs.D1003URLA2020Cal.CalcOtherCredits);
      this.AddFieldHandler("1115", this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + new Routine(this.calculateUCD) + this.calObjs.NewHud2015Cal.CalcLECashToCloseSellerCredit + this.calObjs.NewHud2015Cal.CalcCDPage3StdCashToCloseSellerCredit + this.calObjs.NewHud2015Cal.CalcClosingDisclosureTotalClosingCosts + new Routine(this.calObjs.GFECal.CalculateFunder) + this.calObjs.D1003URLA2020Cal.CalcOtherCredits);
      this.AddFieldHandler("1393", new Routine(this.calculateOthers) + this.calObjs.ToolCal.CalcHMDAReportingYear + this.calObjs.HMDACal.CalcLoanAmount + this.calObjs.HMDACal.CalcIncome + this.calObjs.HMDACal.CalcAllLoanCostsByActionTaken + new Routine(this.calculateLTV) + this.calObjs.HMDACal.CalcHmdaAge + this.calObjs.HMDACal.CalcHmdaNmlsLoanOriginatorId + this.calObjs.HMDACal.CalcRiskAssessment + this.calObjs.HMDACal.CalcTotalPointsAndFees + this.calObjs.HMDACal.CalcHmdaLoanPurpose + this.calObjs.HMDACal.CalcCreditScoreForDecisionMaking + this.calObjs.HMDACal.CalcCreditScoringModel + this.calObjs.HMDACal.CalcLenderCredits + this.calObjs.HMDACal.CalcImportEthnicitySortingWithPairs + this.calObjs.HMDACal.SortEthnicityCategoryWithPairs + this.calObjs.HMDACal.CalcRaceImportSortingWithPairs + this.calObjs.HMDACal.SortRaceCategoryWithPairs + this.calObjs.HMDACal.CalcHMDARace);
      this.AddFieldHandler("1647", this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + new Routine(this.calculateUCD) + this.calObjs.NewHud2015Cal.CalcLECashToCloseSellerCredit + this.calObjs.NewHud2015Cal.CalcCDPage3StdCashToCloseSellerCredit + this.calObjs.NewHud2015Cal.CalcClosingDisclosureTotalClosingCosts + new Routine(this.calObjs.GFECal.CalculateFunder) + this.calObjs.D1003URLA2020Cal.CalcOtherCredits);
      this.AddFieldHandler("4794", this.calObjs.D1003URLA2020Cal.CalcTotalOtherAssets + this.calObjs.D1003Cal.CalcTotalOtherAssets + this.calObjs.D1003URLA2020Cal.CalcTotalCredits + this.calObjs.GFECal.CalcClosingCosts + this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + new Routine(this.calculateUCD) + this.calObjs.NewHud2015Cal.CalcLECashToCloseSellerCredit + this.calObjs.NewHud2015Cal.CalcCDPage3StdCashToCloseSellerCredit + this.calObjs.NewHud2015Cal.CalcClosingDisclosureTotalClosingCosts + new Routine(this.calObjs.GFECal.CalculateFunder) + this.calObjs.D1003URLA2020Cal.CalcOtherCredits);
      this.AddFieldHandler("4795", this.calObjs.D1003URLA2020Cal.CalcTotalOtherAssets + this.calObjs.D1003Cal.CalcTotalOtherAssets + this.calObjs.D1003URLA2020Cal.CalcTotalCredits + this.calObjs.GFECal.CalcClosingCosts + this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + new Routine(this.calculateUCD) + this.calObjs.NewHud2015Cal.CalcLECashToCloseSellerCredit + this.calObjs.NewHud2015Cal.CalcCDPage3StdCashToCloseSellerCredit + this.calObjs.NewHud2015Cal.CalcClosingDisclosureTotalClosingCosts + new Routine(this.calObjs.GFECal.CalculateFunder) + this.calObjs.D1003URLA2020Cal.CalcOtherCredits);
      this.AddFieldHandler("4796", this.calObjs.D1003URLA2020Cal.CalcTotalOtherAssets + this.calObjs.D1003Cal.CalcTotalOtherAssets + this.calObjs.D1003URLA2020Cal.CalcTotalCredits + this.calObjs.GFECal.CalcClosingCosts + this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + new Routine(this.calculateUCD) + this.calObjs.NewHud2015Cal.CalcLECashToCloseSellerCredit + this.calObjs.NewHud2015Cal.CalcCDPage3StdCashToCloseSellerCredit + this.calObjs.NewHud2015Cal.CalcClosingDisclosureTotalClosingCosts + new Routine(this.calObjs.GFECal.CalculateFunder) + this.calObjs.D1003URLA2020Cal.CalcOtherCredits);
      this.AddFieldHandler("967", this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR);
      this.AddFieldHandler("968", this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR);
      this.AddFieldHandler("1825", this.calObjs.D1003Cal.Calc2018DI + new Routine(this.calObjs.D1003URLA2020Cal.SwitchToURLA2020) + this.calObjs.VERIFCal.CalcAllMonthlyIncome + this.calObjs.D1003URLA2020Cal.CalcBorrowerCount + this.calObjs.D1003URLA2020Cal.CalcCreditType + this.calObjs.D1003URLA2020Cal.CalcEstimatedNetMonthlyRent);
      this.AddFieldHandler("1851", this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR);
      this.AddFieldHandler("142", this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR);
      this.AddFieldHandler("143", this.calObjs.GFECal.CalcClosingCosts + this.CalcLTV + this.calObjs.NewHud2015Cal.CalcLECashToCloseSellerCredit + this.calObjs.D1003URLA2020Cal.CalcTotalCredits);
      this.AddFieldHandler("183", this.calObjs.D1003Cal.CalcLiquidAssets + this.calObjs.GFECal.CalcGFEFees + this.calObjs.GFECal.CalcPrepaid + this.calObjs.GFECal.CalcTotalCosts + this.calObjs.D1003URLA2020Cal.CalcOtherCredits + this.calObjs.D1003URLA2020Cal.UpdateLinkedVoal + this.calObjs.VERIFCal.CalcHelocLineTotal);
      this.AddFieldHandler("228", this.calObjs.NewHud2015Cal.CalculateLoanEstimate_ComparisonsTable + this.calObjs.D1003URLA2020Cal.CalcEstimatedNetMonthlyRent + this.calObjs.D1003URLA2020Cal.UpdateLinkedVoal + this.calObjs.VERIFCal.CalcHelocLineTotal);
      this.AddFieldHandler("229", this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.FHACal.CalcMACAWP + this.calObjs.FHACal.CalcFredMac + this.calObjs.GFECal.CalcPrepaid + this.calObjs.VACal.CalcVALA + this.calObjs.NewHud2015Cal.CalculateLoanEstimate_ComparisonsTable + this.calObjs.D1003URLA2020Cal.CalcEstimatedNetMonthlyRent);
      this.AddFieldHandler("230", this.calObjs.D1003URLA2020Cal.CalcProposedSupplementalPropertyInsurance + this.calObjs.D1003URLA2020Cal.CalcSupplementalInsuranceAmount + this.calObjs.VERIFCal.CalcMaintenanceExpenseAmount + new Routine(this.calObjs.VERIFCal.CalculateMultipleVOMs) + this.calObjs.NewHud2015Cal.CalcPropertyInsuranceTaxIndicators + this.calObjs.FHACal.CalcEEMWorksheet + this.calObjs.VACal.CalcVALA + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003URLA2020Cal.CalcTotalMortgageAndTaxes + this.calObjs.Hud1Cal.CalcHUD1ES + this.calObjs.NewHudCal.CalcREGZGFEHud + this.calObjs.PrequalCal.CalcRentOwn + this.calObjs.FHACal.CalcMACAWP + this.calObjs.FHACal.CalcFredMac + this.calObjs.GFECal.CalcPrepaid + new Routine(this.calculateUCD) + this.calObjs.Hud1Cal.CalcBiWeeklyEscrowPayment + this.calObjs.NewHud2015Cal.CalculateLoanEstimate_ComparisonsTable + this.calObjs.D1003URLA2020Cal.CalcEstimatedNetMonthlyRent + this.calObjs.NewHud2015Cal.CalcEstimatedPropertyCosts + this.calObjs.ToolCal.CalcCPAEscrowDetails_FCD_AmountFields);
      this.AddFieldHandler("231", this.calObjs.D1003URLA2020Cal.CalcProposedSupplementalPropertyInsurance + this.calObjs.D1003URLA2020Cal.CalcSupplementalInsuranceAmount + this.calObjs.NewHud2015Cal.CalcPropertyInsuranceTaxIndicators + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.VERIFCal.CalcMaintenanceExpenseAmount + new Routine(this.calObjs.VERIFCal.CalculateMultipleVOMs) + this.calObjs.D1003URLA2020Cal.CalcTotalMortgageAndTaxes + this.calObjs.VERIFCal.CalcMaintenanceExpenseAmount + this.calObjs.Hud1Cal.CalcHUD1ES + this.calObjs.D1003URLA2020Cal.CalcTotalMortgageAndTaxes + this.calObjs.NewHudCal.CalcREGZGFEHud + this.calObjs.GFECal.CalcPrepaid + new Routine(this.calculateUCD) + this.calObjs.Hud1Cal.CalcBiWeeklyEscrowPayment + this.calObjs.NewHud2015Cal.CalcEstimatedPropertyCosts + this.calObjs.NewHud2015Cal.Calc2015LineItem904 + this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem904 + this.calObjs.NewHud2015Cal.CalcClosingCostSubTotal + this.calObjs.ToolCal.CalcCPAEscrowDetails_FCD_AmountFields);
      this.AddFieldHandler("232", this.calObjs.VERIFCal.CalcMaintenanceExpenseAmount + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.VERIFCal.CalcMaintenanceExpenseAmount + new Routine(this.calObjs.VERIFCal.CalculateMultipleVOMs) + this.calObjs.D1003URLA2020Cal.CalcTotalMortgageAndTaxes + this.calObjs.VERIFCal.CalcMaintenanceExpenseAmount + this.calObjs.Hud1Cal.CalcHUD1ES + this.calObjs.D1003URLA2020Cal.CalcTotalMortgageAndTaxes + this.calObjs.NewHudCal.CalcREGZGFEHud + this.calObjs.FHACal.CalcMACAWP + this.calObjs.GFECal.CalcPrepaid + this.calObjs.NewHud2015Cal.CalculateLoanEstimate_ComparisonsTable + this.calObjs.D1003URLA2020Cal.CalcEstimatedNetMonthlyRent + this.calObjs.NewHud2015Cal.CalcLoanEstimateEstimatedTaxes + this.calObjs.ToolCal.CalcCPAEscrowDetails_FCD_AmountFields);
      this.AddFieldHandler("233", this.calObjs.VERIFCal.CalcMaintenanceExpenseAmount + this.calObjs.NewHud2015Cal.CalcPropertyInsuranceTaxIndicators + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003URLA2020Cal.CalcTotalMortgageAndTaxes + this.calObjs.Hud1Cal.CalcHUD1ES + this.calObjs.VERIFCal.CalcMaintenanceExpenseAmount + new Routine(this.calObjs.VERIFCal.CalculateMultipleVOMs) + this.calObjs.FHACal.CalcMACAWP + this.calObjs.GFECal.CalcPrepaid + this.calObjs.VACal.CalcVALA + this.calObjs.Hud1Cal.CalcBiWeeklyEscrowPayment + this.calObjs.NewHud2015Cal.CalculateLoanEstimate_ComparisonsTable + this.calObjs.D1003URLA2020Cal.CalcEstimatedNetMonthlyRent + this.calObjs.NewHud2015Cal.CalcLoanEstimateEstimatedTaxes);
      this.AddFieldHandler("234", this.calObjs.VERIFCal.CalcMaintenanceExpenseAmount + this.calObjs.D1003URLA2020Cal.CalcProposedSupplementalPropertyInsurance + this.calObjs.D1003URLA2020Cal.CalcSupplementalInsuranceAmount + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.VERIFCal.CalcMaintenanceExpenseAmount + new Routine(this.calObjs.VERIFCal.CalculateMultipleVOMs) + this.calObjs.VERIFCal.CalcMaintenanceExpenseAmount + new Routine(this.calObjs.VERIFCal.CalculateMultipleVOMs) + this.calObjs.D1003URLA2020Cal.CalcTotalMortgageAndTaxes + this.calObjs.FHACal.CalcMACAWP + this.calObjs.GFECal.CalcPrepaid + this.calObjs.VACal.CalcVALA + this.calObjs.NewHud2015Cal.CalculateLoanEstimate_ComparisonsTable + this.calObjs.D1003URLA2020Cal.CalcEstimatedNetMonthlyRent + this.calObjs.D1003URLA2020Cal.UpdateLinkedVoal + this.calObjs.VERIFCal.CalcHelocLineTotal);
      this.AddFieldHandler("URLA.X144", this.calObjs.D1003URLA2020Cal.CalcProposedSupplementalPropertyInsurance + this.calObjs.NewHud2015Cal.CalcPropertyInsuranceTaxIndicators + this.calObjs.VERIFCal.CalcMaintenanceExpenseAmount + this.calObjs.D1003URLA2020Cal.CalcProposedSupplementalPropertyInsurance + this.calObjs.D1003URLA2020Cal.CalcSupplementalInsuranceAmount + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.VERIFCal.CalcMaintenanceExpenseAmount + new Routine(this.calObjs.VERIFCal.CalculateMultipleVOMs) + this.calObjs.D1003URLA2020Cal.CalcTotalMortgageAndTaxes + this.calObjs.FHACal.CalcMACAWP + this.calObjs.GFECal.CalcPrepaid + this.calObjs.VACal.CalcVALA + this.calObjs.NewHud2015Cal.CalculateLoanEstimate_ComparisonsTable + this.calObjs.D1003URLA2020Cal.CalcEstimatedNetMonthlyRent + this.calObjs.D1003URLA2020Cal.UpdateLinkedVoal + this.calObjs.VERIFCal.CalcHelocLineTotal);
      this.AddFieldHandler("912", this.calObjs.VERIFCal.CalcMaintenanceExpenseAmount + this.calObjs.D1003URLA2020Cal.CalcProposedSupplementalPropertyInsurance + this.calObjs.D1003URLA2020Cal.CalcSupplementalInsuranceAmount + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003URLA2020Cal.CalcTotalMortgageAndTaxes + this.calObjs.FHACal.CalcMACAWP + this.calObjs.GFECal.CalcPrepaid + this.calObjs.VACal.CalcVALA + this.calObjs.NewHud2015Cal.CalculateLoanEstimate_ComparisonsTable + this.calObjs.D1003URLA2020Cal.CalcEstimatedNetMonthlyRent + this.calObjs.D1003URLA2020Cal.UpdateLinkedVoal + this.calObjs.VERIFCal.CalcHelocLineTotal);
      this.AddFieldHandler("URLA.X146", this.calObjs.GFECal.CalcTotalPrepaidFees + this.calObjs.NewHudCal.CalcClosingCost + this.calObjs.GFECal.CalcTotalCosts + this.calObjs.D1003URLA2020Cal.CalcOtherCredits + this.calObjs.D1003URLA2020Cal.CalcTotalCredits);
      this.AddFieldHandler("235", this.calObjs.D1003URLA2020Cal.CalcProposedSupplementalPropertyInsurance + this.calObjs.D1003URLA2020Cal.CalcSupplementalInsuranceAmount + this.calObjs.NewHud2015Cal.CalcPropertyInsuranceTaxIndicators + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003URLA2020Cal.CalcTotalMortgageAndTaxes + this.calObjs.VERIFCal.CalcMaintenanceExpenseAmount + this.calObjs.Hud1Cal.CalcHUD1ES + this.calObjs.D1003URLA2020Cal.CalcTotalMortgageAndTaxes + this.calObjs.NewHudCal.CalcREGZGFEHud + this.calObjs.FHACal.CalcMACAWP + this.calObjs.FHACal.CalcFredMac + this.calObjs.GFECal.CalcPrepaid + new Routine(this.calculateUCD) + this.calObjs.Hud1Cal.CalcBiWeeklyEscrowPayment + this.calObjs.VACal.CalcVARecoupment + this.calObjs.NewHud2015Cal.CalcEstimatedPropertyCosts + this.calObjs.ToolCal.CalcCPAEscrowDetails_FCD_AmountFields);
      this.AddFieldHandler("247", this.calObjs.RegzCal.CalcFloorRate + this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + this.calObjs.NewHud2015Cal.CalcAPTable_MaximumPayment + this.calObjs.HelocCal.CalcHELOCPeriodicRates);
      this.AddFieldHandler("253", this.calObjs.D1003URLA2020Cal.CalcProposedSupplementalPropertyInsurance + this.calObjs.D1003URLA2020Cal.CalcSupplementalInsuranceAmount + this.calObjs.NewHud2015Cal.CalcPropertyInsuranceTaxIndicators + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.VERIFCal.CalcMaintenanceExpenseAmount + this.calObjs.NewHud2015Cal.CalcLoanEstimateTaxesInsuranceInEscrowOrNot + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003URLA2020Cal.CalcTotalMortgageAndTaxes + this.calObjs.VERIFCal.CalcMaintenanceExpenseAmount + this.calObjs.Hud1Cal.CalcHUD1ES + this.calObjs.D1003URLA2020Cal.CalcTotalMortgageAndTaxes + this.calObjs.FHACal.CalcMACAWP + this.calObjs.FHACal.CalcFredMac + this.calObjs.GFECal.CalcPrepaid + new Routine(this.calculateUCD) + this.calObjs.Hud1Cal.CalcBiWeeklyEscrowPayment + this.calObjs.VACal.CalcVARecoupment + this.calObjs.NewHud2015Cal.CalcEstimatedPropertyCosts + this.calObjs.ToolCal.CalcCPAEscrowDetails_FCD_AmountFields);
      this.AddFieldHandler("254", this.calObjs.D1003URLA2020Cal.CalcProposedSupplementalPropertyInsurance + this.calObjs.D1003URLA2020Cal.CalcSupplementalInsuranceAmount + this.calObjs.NewHud2015Cal.CalcPropertyInsuranceTaxIndicators + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.VERIFCal.CalcMaintenanceExpenseAmount + this.calObjs.NewHud2015Cal.CalcLoanEstimateTaxesInsuranceInEscrowOrNot + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003URLA2020Cal.CalcTotalMortgageAndTaxes + this.calObjs.VERIFCal.CalcMaintenanceExpenseAmount + this.calObjs.Hud1Cal.CalcHUD1ES + this.calObjs.D1003URLA2020Cal.CalcTotalMortgageAndTaxes + this.calObjs.FHACal.CalcMACAWP + this.calObjs.FHACal.CalcFredMac + this.calObjs.GFECal.CalcPrepaid + new Routine(this.calculateUCD) + this.calObjs.Hud1Cal.CalcBiWeeklyEscrowPayment + this.calObjs.VACal.CalcVARecoupment + this.calObjs.NewHud2015Cal.CalcEstimatedPropertyCosts + this.calObjs.ToolCal.CalcCPAEscrowDetails_FCD_AmountFields);
      this.AddFieldHandler("299", this.calObjs.GFECal.CalcSection32 + this.calObjs.D1003Cal.CalcNMLSRefiPurpose + this.calObjs.D1003URLA2020Cal.CalculateRefinanceType);
      this.AddFieldHandler("312", this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR);
      this.AddFieldHandler("315", this.calObjs.D1003Cal.CopyBrokerInfoToLender + this.calObjs.NewHudCal.CalcLoanOriginatorName);
      this.AddFieldHandler("325", this.calObjs.D1003Cal.PopulateOtherFields + this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + new Routine(this.calculateUCD) + this.calObjs.HMDACal.CalcLoanTerm + this.calObjs.RegzCal.CalcPMIMidpointCancelationDate + this.calObjs.NewHud2015Cal.CalculateProductDescription + this.calObjs.NewHud2015Cal.Calc60Payments + this.calObjs.HMDACal.CalcLoanTerm + this.calObjs.NewHud2015Cal.CalcAPTable_InterestOnlyPayments + this.calObjs.NewHud2015Cal.CalcConstructionPermPeriod);
      this.AddFieldHandler("332", this.calObjs.GFECal.CalcPrepaid + new Routine(this.calculateUCD) + this.calObjs.NewHud2015Cal.CalcLoanEstimateTotalAdjustmentsCredit + this.calObjs.NewHud2015Cal.CalcLoanEstimateThirdPartyPaymentsNotOtherwiseDisclosed + this.calObjs.NewHud2015Cal.CalcLoanEstimate_ClosingCostsFinanced + this.calObjs.NewHud2015Cal.CalcLoanEstimateDownPaymentAndFundsForBorrower + this.calObjs.NewHud2015Cal.CalcLoanEstimate_EstimatedCashToClose);
      this.AddFieldHandler("334", this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + this.calObjs.RegzCal.CalcIRS1098);
      this.AddFieldHandler("356", this.calObjs.FHACal.CalcEEMWorksheet + new Routine(this.UpdateAccountName) + this.calObjs.D1003Cal.PopulateOtherFields + this.calObjs.PrequalCal.CalcMIP + this.calObjs.NewHud2015Cal.CalcPrepaidMI + this.calObjs.PrequalCal.CalcDownPay + this.calObjs.GFECal.CalcGFEFees + this.calObjs.GFECal.CalcPrepaid + this.calObjs.MLDSCal.UpdateInsurance + this.calObjs.VACal.CalcMaximumSellerContribution + this.calObjs.FHACal.CalcMACAWP + this.CalcLTV + this.calObjs.RegzCal.CalcAPR + this.calObjs.PrequalCal.CalcLockRequestLoan + this.calObjs.NewHud2015Cal.Calc2015LineItem904 + this.calObjs.NewHud2015Cal.CalcAsCompletedAppraisedValue + this.calObjs.VACal.CalcVACashOutRefinance);
      this.AddFieldHandler("HMDA.X108", new Routine(this.calculateLTV));
      this.AddFieldHandler("372", this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR);
      this.AddFieldHandler("4749", new Routine(this.calculateUCD) + this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + new Routine(this.calculateUCD) + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.FHACal.CalcEEMWorksheet + this.calObjs.NewHud2015Cal.CalcAPTable_MaximumPayment);
      this.AddFieldHandler("4748", this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + this.calObjs.FHACal.CalcEEMWorksheet + this.calObjs.NewHud2015Cal.CalcAPTable_MaximumPayment);
      this.AddFieldHandler("388", this.calObjs.GFECal.CalcGFEFees + new Routine(((CalculationBase) this.calObjs.NewHudCal).FormCal) + this.calObjs.LoansubCal.CalcLoansub + this.calObjs.VACal.CalcVARRRWS);
      this.AddFieldHandler("389", this.calObjs.GFECal.CalcGFEFees + this.calObjs.LoansubCal.CalcLoansub);
      this.AddFieldHandler("420", this.calObjs.D1003Cal.PopulateOtherFields + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.VERIFCal.CalcHelocLineTotal + new Routine(((CalculationBase) this.calObjs.NewHudCal).FormCal) + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.FHACal.CalcFredMac + this.calObjs.GFECal.CalcHighPrice + this.calObjs.GFECal.CalcSecondAppraisalRequired + this.calObjs.ATRQMCal.CalcHigherPricedCheck + this.CalcLTV + this.calObjs.ATRQMCal.CalcEligibility + this.calObjs.ATRQMCal.CalcMaxTotalPayments + new Routine(this.calObjs.GFECal.UpdateValueExistingJuniorLien) + this.calObjs.NewHud2015Cal.PopulateLoanEstimate_LoanPurpose + this.CalcIsLSSecondaryFile + this.CalcLoanLinkSyncType + this.calObjs.RegzCal.CalculateHELOCPayment + this.calObjs.D1003URLA2020Cal.CalcEstimatedNetMonthlyRent + this.calObjs.D1003Cal.CalcLienPosition);
      this.AddFieldHandler("423", this.calObjs.GFECal.CalcGFEFees + this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.Hud1Cal.CalcHUD1ES + this.calObjs.RegzCal.CalcREGZSummary + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + this.calObjs.NewHud2015Cal.CalcAPTable_MaximumPayment + this.calObjs.HMDACal.CalcLoanTerm + this.calObjs.RegzCal.CalcPMIMidpointCancelationDate + this.calObjs.ToolCal.CalcCPAEscrowDetails_FCD_AmountFields);
      this.AddFieldHandler("425", this.calObjs.RegzCal.CalcBuydownIndicator + this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR);
      this.AddFieldHandler("427", this.calObjs.D1003Cal.PopulateOtherFields + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.VERIFCal.CalcHelocLineTotal + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.CalcLTV + this.calObjs.PrequalCal.CalcLockRequestLoan);
      this.AddFieldHandler("428", this.calObjs.D1003Cal.PopulateOtherFields + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.VERIFCal.CalcHelocLineTotal + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.CalcLTV + this.calObjs.PrequalCal.CalcLockRequestLoan);
      this.AddFieldHandler("436", this.calObjs.GFECal.CalcGFEFees + new Routine(((CalculationBase) this.calObjs.NewHudCal).FormCal) + this.calObjs.LoansubCal.CalcLoansub + this.calObjs.ATRQMCal.CalcDiscountPoints);
      this.AddFieldHandler("471", this.calObjs.HMDACal.CalcGender + this.CalcVAAccount);
      this.AddFieldHandler("478", this.calObjs.D1003Cal.CalcEthnicityRaceSex + this.calObjs.HMDACal.CalcNocoapplicant + this.calObjs.HMDACal.CalcGender + this.CalcVAAccount);
      this.AddFieldHandler("4193", this.calObjs.HMDACal.CalcGender + this.CalcVAAccount);
      this.AddFieldHandler("4194", this.calObjs.HMDACal.CalcGender + this.CalcVAAccount);
      this.AddFieldHandler("4196", this.calObjs.HMDACal.CalcGender + this.CalcVAAccount);
      this.AddFieldHandler("4245", this.calObjs.HMDACal.CalcGender + this.CalcVAAccount);
      this.AddFieldHandler("4197", this.calObjs.HMDACal.CalcGender + this.CalcVAAccount);
      this.AddFieldHandler("4198", this.calObjs.HMDACal.CalcGender + this.CalcVAAccount);
      this.AddFieldHandler("4200", this.calObjs.HMDACal.CalcGender + this.CalcVAAccount);
      this.AddFieldHandler("4248", this.calObjs.HMDACal.CalcGender + this.CalcVAAccount);
      this.AddFieldHandler("4189", this.calObjs.HMDACal.CalcNocoapplicant + this.CalcVAAccount);
      this.AddFieldHandler("559", this.calObjs.GFECal.CalcGFEFees + this.calObjs.LoansubCal.CalcLoansub);
      this.AddFieldHandler("562", this.calObjs.FHACal.CalcMACAWP + this.calObjs.GFECal.CalcGFEFees + this.calObjs.GFECal.CalcPrepaid + this.calObjs.PrequalCal.CalcMIP + this.calObjs.D1003Cal.CalculatePMIIndicator);
      this.AddFieldHandler("608", this.calObjs.RegzCal.CalcZeroPercentPaymentOption + this.calObjs.D1003URLA2020Cal.CalcArmProg + this.calObjs.NewHud2015Cal.CalcConstructionPermPeriod + this.calObjs.NewHud2015Cal.CalcAPTable_InterestOnlyPayments + this.calObjs.D1003Cal.PopulateOtherFields + this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + this.calObjs.ATRQMCal.CalcMax5YearsPandI + this.calObjs.D1003URLA2020Cal.UpdateLinkedVoal + this.calObjs.VERIFCal.CalcHelocLineTotal + this.calObjs.ATRQMCal.CalcEligibility + this.calObjs.NewHud2015Cal.CalcAPTable_SubsequentChanges + this.calObjs.NewHud2015Cal.CalcAPTable_MaximumPayment + this.calObjs.ATRQMCal.CalcEligibility + this.calObjs.NewHud2015Cal.CalculateProductDescription + this.calObjs.NewHud2015Cal.SetAIRTableIndexType + this.calObjs.HMDACal.CalcIntroRatePeriod + this.calObjs.HelocCal.CalcHELOCPeriodicRates + this.calObjs.RegzCal.CalcMaturityDate);
      this.AddFieldHandler("647", new Routine(this._gfeCalculationServant.UpdateCityStateUserFees) + this.calObjs.NewHudCal.CalcREGZGFE + this.calObjs.NewHudCal.CalcClosingCost);
      this.AddFieldHandler("660", this.calObjs.D1003URLA2020Cal.CalcProposedSupplementalPropertyInsurance + this.calObjs.D1003URLA2020Cal.CalcSupplementalInsuranceAmount + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.Hud1Cal.CalcHUD1ES + this.calObjs.FHACal.CalcMACAWP + this.calObjs.FHACal.CalcFredMac + this.calObjs.GFECal.CalcPrepaid + this.calObjs.VACal.CalcVARecoupment);
      this.AddFieldHandler("661", this.calObjs.D1003URLA2020Cal.CalcProposedSupplementalPropertyInsurance + this.calObjs.D1003URLA2020Cal.CalcSupplementalInsuranceAmount + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.Hud1Cal.CalcHUD1ES + this.calObjs.FHACal.CalcMACAWP + this.calObjs.FHACal.CalcFredMac + this.calObjs.GFECal.CalcPrepaid + this.calObjs.VACal.CalcVARecoupment);
      this.AddFieldHandler("682", this.calObjs.Hud1Cal.CalcEscrowFirstPaymentDate + this.calObjs.NewHud2015Cal.CalcWillHaveEscrowAccount + this.calObjs.GFECal.CalcGFEFees + new Routine(this.calculateUCD) + this.calObjs.Hud1Cal.CalcShiftPaymentFrequence + new Routine(((CalculationBase) this.calObjs.NewHudCal).FormCal) + this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.RegzCal.CalculateHELOCPayment + this.calObjs.RegzCal.CalcHELOCInitialPayment + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.Hud1Cal.CalcHUD1ES + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + this.calObjs.RegzCal.CalcPMIMidpointCancelationDate + this.calObjs.HMDACal.CalcIntroRatePeriod + this.calObjs.RegzCal.CalcMaturityDate + this.calObjs.ToolCal.CalcPrepaymentBalance + this.calObjs.FHACal.CalcEEMWorksheet + this.calObjs.FHACal.CalcEEMX88AndEEMX89 + this.calObjs.D1003URLA2020Cal.CalcEstimatedNetMonthlyRent);
      this.AddFieldHandler("688", this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.RegzCal.CalculateHELOCPayment + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + this.calObjs.NewHud2015Cal.CalcAPTable_MaximumPayment + this.calObjs.PrequalCal.CalcHELOCLockRequest + this.calObjs.HelocCal.CalcHELOCPeriodicRates + this.calObjs.D1003Cal.ClearIndexDate);
      this.AddFieldHandler("689", this.calObjs.RegzCal.CalcFloorRate + this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + this.calObjs.NewHud2015Cal.CalcAPTable_MaximumPayment + this.calObjs.RegzCal.CalcFirstAdjustmentMinimum + this.calObjs.NewHud2015Cal.CalcLECDDisplayFields + this.calObjs.HelocCal.CalcHELOCPeriodicRates);
      this.AddFieldHandler("690", this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR);
      this.AddFieldHandler("691", this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR);
      this.AddFieldHandler("694", this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + this.calObjs.NewHud2015Cal.CalcLoanEstimate_AIRMonthSuffix + this.calObjs.NewHud2015Cal.CalcAPTable_SubsequentChanges + this.calObjs.NewHud2015Cal.CalcLoanEstimate_AIRMonthSuffix + this.calObjs.NewHud2015Cal.CalcAPTable_MaximumPayment + this.calObjs.NewHud2015Cal.CalculateProductDescription);
      this.AddFieldHandler("695", this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + this.calObjs.NewHud2015Cal.CalcAPTable_MaximumPayment + this.calObjs.NewHud2015Cal.CalculateProductDescription + this.calObjs.NewHud2015Cal.CalcLECDDisplayFields);
      this.AddFieldHandler("696", this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + this.calObjs.NewHud2015Cal.CalcLoanEstimate_AIRMonthSuffix + this.calObjs.NewHud2015Cal.CalcAPTable_MaximumPayment + this.calObjs.NewHud2015Cal.CalculateProductDescription + this.calObjs.HMDACal.CalcIntroRatePeriod);
      this.AddFieldHandler("697", this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + this.calObjs.NewHud2015Cal.CalcAPTable_MaximumPayment + this.calObjs.NewHud2015Cal.CalculateProductDescription + this.calObjs.RegzCal.CalcFirstAdjustmentMinimum + this.calObjs.NewHud2015Cal.CalcLECDDisplayFields);
      this.AddFieldHandler("698", this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + this.calObjs.NewHud2015Cal.CalcAPTable_MaximumPayment + this.calObjs.RegzCal.CalcFirstAdjustmentMinimum);
      this.AddFieldHandler("733", this.calObjs.D1003Cal.CalcNetWorth + this.calObjs.FHACal.CalcMACAWP);
      this.AddFieldHandler("745", this.calObjs.ATRQMCal.CalcQMX383 + this.calObjs.NewHudCal.CalcREGZGFEHud + this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.GFECal.CalcSection32 + this.calObjs.ATRQMCal.CalcPointsAndFees + this.calObjs.ATRQMCal.CalcAgencyGSEQMAvailability + this.calObjs.ATRQMCal.CalcEligibility + this.calObjs.FHACal.CalcCAWRefi + this.calObjs.HMDACal.CalcApplicationDate + this.calObjs.D1003Cal.CalcConformingLimit);
      this.AddFieldHandler("748", this.calObjs.RegzCal.CalcIRS1098 + this.calObjs.Hud1Cal.CalcHUD1ES + this.calObjs.D1003Cal.CalcHousingExp + new Routine(this.calObjs.VERIFCal.CalculateMultipleVOMs) + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.VACal.CalcVARecoupment + this.calObjs.ToolCal.SyncConstructionLinkSyncDefaultData + new Routine(((CalculationBase) this.calObjs.FeeVarianceToolCal).FormCal) + this.calObjs.RegzCal.CalcHELOCInitialPayment + this.calObjs.VACal.CalcVAStatutoryRecoupment + this.calObjs.GFECal.CalcSection32);
      this.AddFieldHandler("749", this.calObjs.ToolCal.CalcHMDAReportingYear);
      this.AddFieldHandler("763", this.calObjs.D1003Cal.PopulateOtherFields + this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.ATRQMCal.CalcPointsAndFees + this.calObjs.ATRQMCal.CalcEligibility + this.calObjs.RegzCal.CalcAPR + this.calObjs.ToolCal.SyncConstructionLinkSyncDefaultData + this.calObjs.VACal.CalcVARecoupment + this.calObjs.VACal.CalcVAStatutoryRecoupment + this.calObjs.GFECal.CalcSection32);
      this.AddFieldHandler("934", this.calObjs.D1003URLA2020Cal.CalcFirstTimeHomeBuyer + this.calObjs.ULDDExpCal.CalcFannieMaeExportFields);
      this.AddFieldHandler("949", this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + this.calObjs.ATRQMCal.CalcPointsAndFees);
      this.AddFieldHandler("958", this.calObjs.GFECal.CalcGFEFees + this.calObjs.GFECal.CalcSection32 + this.calObjs.ToolCal.CalcSafeHarbor + this.calObjs.ATRQMCal.CalcEligibility + this.calObjs.VACal.CalcVARecoupment + this.CalcLTV + this.calObjs.PrequalCal.CalcLockRequestLoan + this.calObjs.VACal.CalcVAStatutoryRecoupment);
      this.AddFieldHandler("967", this.calObjs.GFECal.CalcTotalCosts + this.calObjs.D1003URLA2020Cal.CalcOtherCredits + this.CalcLTV);
      this.AddFieldHandler("969", this.calObjs.GFECal.CalcGFEFees + this.calObjs.GFECal.CalcPrepaid + this.calObjs.NewHudCal.CalcREGZGFE + this.calObjs.NewHudCal.CalcHUD1PG2 + this.calObjs.VACal.CalcVARecoupment + this.calObjs.FHACal.CalcFredMac + this.calObjs.ATRQMCal.CalcEligibility + this.calObjs.D1003URLA2020Cal.CalcEstimatedClosingCostsAmount + this.calObjs.D1003Cal.CalculatePMIIndicator);
      this.AddFieldHandler("FL0008", this.calObjs.D1003URLA2020Cal.CalcBackEndRatio + this.calObjs.VERIFCal.CalcLiabilities + this.calObjs.D1003URLA2020Cal.CalcNonSubjectPropertyDebtsToBePaidOffAmount + this.calObjs.VACal.CalcVARRRWS + this.calObjs.GFECal.CalcGFEFees + new Routine(((CalculationBase) this.calObjs.NewHudCal).FormCal) + this.calObjs.ToolCal.CalcSafeHarbor + this.calObjs.FHACal.CalcExisting23KDebt + this.calObjs.FHACal.CalcMAX23K + this.calObjs.FHACal.CalcCAWRefi + this.calObjs.FHACal.CalcFredMac + this.calObjs.NewHud2015Cal.PopulateLoanEstimate_LoanPurpose + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.VERIFCal.CalcHelocLineTotal + this.calObjs.HMDACal.CalcLoanAmount + this.CalcVerifAccountName);
      this.AddFieldHandler("FL0014", this.calObjs.D1003URLA2020Cal.CalcBackEndRatio + this.calObjs.VERIFCal.CalcLiabilities + this.calObjs.D1003Cal.CalcLoansubAndTSUM);
      this.AddFieldHandler("FL0013", this.calObjs.D1003URLA2020Cal.CalcBackEndRatio + this.calObjs.VERIFCal.CalcLiabilities + this.calObjs.D1003URLA2020Cal.CalcNonSubjectPropertyDebtsToBePaidOffAmount + this.calObjs.NewHud2015Cal.CalcCdPage3Liabilities + this.calObjs.NewHud2015Cal.CalcLoanEstimateThirdPartyPaymentsNotOtherwiseDisclosed + this.calObjs.NewHud2015Cal.CalcCDPage3TotalPurchasePayoffsIncluded + this.calObjs.NewHud2015Cal.CalcAdjustmentsUcdTotal + this.calObjs.NewHud2015Cal.CalcTotalAdjustmentAndOtherCredits + this.calObjs.NewHud2015Cal.CalcLoanEstimateDownPaymentAndFundsForBorrower + this.calObjs.NewHud2015Cal.PopulateLoanEstimate_LoanPurpose + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.VERIFCal.CalcHelocLineTotal + this.calObjs.FHACal.CalcMAX23K + this.calObjs.VERIFCal.CalcPaceLoanAmounts);
      this.AddFieldHandler("FL0016", this.calObjs.D1003URLA2020Cal.CalcBackEndRatio + this.calObjs.VERIFCal.CalcLiabilities + this.calObjs.D1003URLA2020Cal.CalcNonSubjectPropertyDebtsToBePaidOffAmount + this.calObjs.VACal.CalcVARRRWS + this.calObjs.GFECal.CalcGFEFees + new Routine(((CalculationBase) this.calObjs.NewHudCal).FormCal) + this.calObjs.ToolCal.CalcSafeHarbor + this.calObjs.FHACal.CalcExisting23KDebt + this.calObjs.FHACal.CalcMAX23K + this.calObjs.FHACal.CalcCAWRefi + this.calObjs.FHACal.CalcFredMac + this.calObjs.NewHud2015Cal.CalcCdPage3Liabilities + this.calObjs.NewHud2015Cal.CalcLoanEstimateThirdPartyPaymentsNotOtherwiseDisclosed + this.calObjs.NewHud2015Cal.PopulateLoanEstimate_LoanPurpose);
      this.AddFieldHandler("FL0017", this.calObjs.D1003URLA2020Cal.CalcBackEndRatio + this.calObjs.VERIFCal.CalcLiabilities + this.calObjs.D1003URLA2020Cal.CalcNonSubjectPropertyDebtsToBePaidOffAmount + this.calObjs.VACal.CalcVARRRWS + this.calObjs.GFECal.CalcGFEFees + new Routine(((CalculationBase) this.calObjs.NewHudCal).FormCal) + this.calObjs.ToolCal.CalcSafeHarbor + this.calObjs.FHACal.CalcExisting23KDebt + this.calObjs.FHACal.CalcMAX23K + this.calObjs.FHACal.CalcCAWRefi + this.calObjs.FHACal.CalcFredMac + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.VERIFCal.CalcHelocLineTotal + this.calObjs.HMDACal.CalcLoanAmount + this.calObjs.FHACal.CalcMAX23K);
      this.AddFieldHandler("FL0018", this.calObjs.RegzCal.CalculateHELOCPayment + this.calObjs.D1003URLA2020Cal.CalcBackEndRatio + this.calObjs.VERIFCal.CalcLiabilities + this.calObjs.D1003URLA2020Cal.CalcNonSubjectPropertyDebtsToBePaidOffAmount + this.calObjs.VACal.CalcVARRRWS + this.calObjs.GFECal.CalcGFEFees + new Routine(((CalculationBase) this.calObjs.NewHudCal).FormCal) + this.calObjs.ToolCal.CalcSafeHarbor + this.calObjs.FHACal.CalcExisting23KDebt + this.calObjs.FHACal.CalcMAX23K + this.calObjs.FHACal.CalcCAWRefi + this.calObjs.FHACal.CalcFredMac + this.calObjs.NewHud2015Cal.CalcCdPage3Liabilities + this.calObjs.NewHud2015Cal.CalcLoanEstimateThirdPartyPaymentsNotOtherwiseDisclosed + this.calObjs.NewHud2015Cal.CalcCDPage3TotalPurchasePayoffsIncluded + this.calObjs.NewHud2015Cal.CalcAdjustmentsUcdTotal + this.calObjs.NewHud2015Cal.CalcTotalAdjustmentAndOtherCredits + this.calObjs.NewHud2015Cal.CalcLoanEstimateDownPaymentAndFundsForBorrower + this.calObjs.NewHud2015Cal.PopulateLoanEstimate_LoanPurpose + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.VERIFCal.CalcHelocLineTotal + this.calObjs.HMDACal.CalcLoanAmount + this.calObjs.VERIFCal.CalcPaceLoanAmounts + this.calObjs.D1003URLA2020Cal.CalcEstimatedNetMonthlyRent);
      this.AddFieldHandler("FL0026", this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.VERIFCal.CalcHelocLineTotal + this.calObjs.FHACal.CalcMAX23K);
      this.AddFieldHandler("FL0027", this.calObjs.RegzCal.CalculateHELOCPayment + this.calObjs.D1003URLA2020Cal.CalcBackEndRatio + this.calObjs.VERIFCal.CalcLiabilities + this.calObjs.D1003URLA2020Cal.CalcNonSubjectPropertyDebtsToBePaidOffAmount + this.calObjs.VACal.CalcVARRRWS + this.calObjs.GFECal.CalcGFEFees + new Routine(((CalculationBase) this.calObjs.NewHudCal).FormCal) + this.calObjs.ToolCal.CalcSafeHarbor + this.calObjs.FHACal.CalcExisting23KDebt + this.calObjs.FHACal.CalcMAX23K + this.calObjs.FHACal.CalcCAWRefi + this.calObjs.FHACal.CalcFredMac + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.VERIFCal.CalcHelocLineTotal + this.calObjs.HMDACal.CalcLoanAmount + this.calObjs.FHACal.CalcMAX23K + this.calObjs.D1003URLA2020Cal.CalcEstimatedNetMonthlyRent);
      this.AddFieldHandler("FL0028", this.calObjs.GFECal.CalcTotalCosts + this.calObjs.FHACal.CalcFredMac);
      this.AddFieldHandler("FL0029", this.calObjs.RegzCal.CalculateHELOCPayment + this.calObjs.VERIFCal.CalcLiabilities + new Routine(this.calObjs.VERIFCal.CalcRealEstate) + this.calObjs.D1003URLA2020Cal.CalcTotalMortgageAndTaxes + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.VERIFCal.CalcLiabilities + this.calObjs.D1003URLA2020Cal.CalcTotalMortgageAndTaxes + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.VERIFCal.CalcHelocLineTotal + this.calObjs.FHACal.CalcMAX23K + this.calObjs.D1003URLA2020Cal.CalcEstimatedNetMonthlyRent);
      this.AddFieldHandler("FL0031", this.calObjs.VERIFCal.CalcHelocLineTotal + this.calObjs.HMDACal.CalcLoanAmount + this.calObjs.FHACal.CalcMAX23K);
      this.AddFieldHandler("FL0063", this.calObjs.D1003URLA2020Cal.CalcBackEndRatio + this.calObjs.VERIFCal.CalcLiabilities + this.calObjs.VACal.CalcVARRRWS + this.calObjs.GFECal.CalcGFEFees + new Routine(((CalculationBase) this.calObjs.NewHudCal).FormCal) + this.calObjs.ToolCal.CalcSafeHarbor + this.calObjs.FHACal.CalcExisting23KDebt + this.calObjs.FHACal.CalcMAX23K + this.calObjs.FHACal.CalcCAWRefi + this.calObjs.FHACal.CalcFredMac + this.calObjs.NewHud2015Cal.CalcCdPage3Liabilities + this.calObjs.NewHud2015Cal.CalcLoanEstimateThirdPartyPaymentsNotOtherwiseDisclosed + this.calObjs.NewHud2015Cal.PopulateLoanEstimate_LoanPurpose);
      this.AddFieldHandler("FL0061", this.calObjs.NewHud2015Cal.PopulateLoanEstimate_LoanPurpose);
      this.AddFieldHandler("1014", this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003URLA2020Cal.CalcEstimatedNetMonthlyRent + this.calObjs.LoansubCal.CalcLoansub);
      this.AddFieldHandler("SYS.X11", this.calObjs.D1003Cal.PopulateOtherFields + this.calObjs.PrequalCal.CalcMIP + this.calObjs.PrequalCal.CalcDownPay + new Routine(this.calObjs.ToolCal.UpdatePurchaseAdviceBalance) + this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.PrequalCal.CalcRentOwn + this.calObjs.GFECal.CalcGFEFees + this.calObjs.LoansubCal.CalcLoansub + this.calObjs.FHACal.CalcFredMac + this.CalcLTV + this.calObjs.FHACal.CalcExisting23KDebt + this.calObjs.FHACal.CalcMAX23K + this.calObjs.VACal.CalcVARRRWS);
      this.AddFieldHandler("1045", this.calObjs.PrequalCal.CalcMIP + this.calObjs.PrequalCal.CalcDownPay + this.calObjs.USDACal.CalcLoanFundsUsage + new Routine(this.calObjs.ToolCal.UpdatePurchaseAdviceBalance) + this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.PrequalCal.CalcRentOwn + this.calObjs.GFECal.CalcGFEFees + this.calObjs.LoansubCal.CalcLoansub + this.calObjs.FHACal.CalcFredMac + this.CalcLTV + this.calObjs.FHACal.CalcExisting23KDebt + this.calObjs.FHACal.CalcMAX23K + this.calObjs.VACal.CalcVARRRWS + this.calObjs.D1003Cal.CalculatePMIIndicator);
      this.AddFieldHandler("1046", new Routine(this.calObjs.NewHudCal.CalcLOCompensationTool) + this.calObjs.FHACal.CalcMACAWP + this.calObjs.FHACal.CalcDraw203K);
      this.AddFieldHandler("1061", this.calObjs.GFECal.CalcGFEFees + new Routine(((CalculationBase) this.calObjs.NewHudCal).FormCal) + this.calObjs.LoansubCal.CalcLoansub + this.calObjs.VACal.CalcVARRRWS + this.calObjs.ATRQMCal.CalcDiscountPoints);
      this.AddFieldHandler("1086", this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.LoansubCal.CalcLoansub);
      this.AddFieldHandler("1092", this.calObjs.GFECal.CalcGFEFees + this.calObjs.GFECal.CalcPrepaid + this.calObjs.NewHudCal.CalcREGZGFE + this.calObjs.NewHudCal.CalcHUD1PG2 + this.calObjs.NewHud2015Cal.PopulateLoanEstimate_LoanPurpose + this.calObjs.NewHud2015Cal.CalcLoanEstimateEstimatedCashToCloseAV + this.calObjs.NewHud2015Cal.CalcLoanEstimateThirdPartyPaymentsNotOtherwiseDisclosed + this.calObjs.NewHud2015Cal.PopulateLoanEstimate_LoanPurpose + this.calObjs.NewHud2015Cal.CalcLoanEstimate_ClosingCostsFinanced + this.calObjs.NewHud2015Cal.CalcLoanEstimateDownPaymentAndFundsForBorrower + this.calObjs.NewHud2015Cal.CalcCDPage3StdCashToCloseFinal);
      this.AddFieldHandler("1094", this.calObjs.FHACal.CalcMACAWP + this.calObjs.FHACal.CalcFredMac);
      this.AddFieldHandler("1107", this.calObjs.PrequalCal.CalcMIP + this.calObjs.PrequalCal.CalcDownPay + this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.FHACal.CalcMACAWP + this.calObjs.GFECal.CalcPrepaid + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.PrequalCal.CalcRentOwn + this.calObjs.GFECal.CalcGFEFees + this.calObjs.LoansubCal.CalcLoansub + this.CalcLTV + this.calObjs.ToolCal.CalcProfit + this.calObjs.VACal.CalcVARRRWS + this.calObjs.FHACal.CalcEEMWorksheet + this.calObjs.D1003URLA2020Cal.UpdateLinkedVoal + this.calObjs.VERIFCal.CalcHelocLineTotal + this.calObjs.D1003Cal.CalculatePMIIndicator + this.calObjs.RegzCal.CalcPointsInInitialAdjustedRate);
      this.AddFieldHandler("1109", this.calObjs.D1003Cal.CalcField1109 + this.calObjs.NewHud2015Cal.CalcLoanEstimateDownPaymentAndFundsForBorrower + this.calObjs.NewHud2015Cal.CalcCDPage3StdCashToCloseFinal + this.calObjs.NewHud2015Cal.Calc2015LineItem904 + this.calObjs.HMDACal.CalcLoanAmount + this.calObjs.NewHud2015Cal.CalcPrepaidMI + this.calObjs.D1003URLA2020Cal.UpdateLinkedVoal + this.calObjs.VERIFCal.CalcHelocLineTotal + this.calObjs.NewHudCal.EnforceGFEAppDateRule + this.calObjs.NewHudCal.CalcInitialDisclosureDueDate + this.calObjs.HelocCal.CalcHELOCPeriodicRates + this.calObjs.VACal.CalcVAStatutoryRecoupment + this.calObjs.D1003Cal.CalculatePMIIndicator + this.calObjs.RegzCal.CalcPointsInInitialAdjustedRate + this.calObjs.D1003URLA2020Cal.CalcEstimatedNetMonthlyRent);
      this.AddFieldHandler("ULDD.FNM.MORNET.X75", this.CalcLTV + this.calObjs.ULDDExpCal.CalcFannieMaeExportFields);
      this.AddFieldHandler("ULDD.FNM.MORNET.X76", this.CalcLTV);
      this.AddFieldHandler("ULDD.FNM.MORNET.X77", this.CalcLTV);
      this.AddFieldHandler("ULDD.FRD.CASASRN.X217", this.CalcLTV);
      this.AddFieldHandler("ULDD.FRD.CASASRN.X218", this.CalcLTV);
      this.AddFieldHandler("ULDD.FRD.CASASRN.X219", this.CalcLTV);
      this.AddFieldHandler("1130", this.calObjs.D1003Cal.PopulateOtherFields);
      this.AddFieldHandler("1132", this.calObjs.FHACal.CalcExisting23KDebt + this.calObjs.FHACal.CalcMAX23K + this.calObjs.NewHudCal.CalcClosingCost + this.calObjs.FHACal.CalcMACAWP);
      this.AddFieldHandler("1134", this.calObjs.FHACal.CalcExisting23KDebt + this.calObjs.FHACal.CalcMAX23K + this.calObjs.FHACal.CalcCAWRefi + this.calObjs.NewHudCal.CalcClosingCost + this.calObjs.GFECal.CalcTotalCosts + this.calObjs.D1003URLA2020Cal.CalcOtherCredits + this.calObjs.D1003URLA2020Cal.CalcTotalCredits + this.CalcLTV + this.calObjs.PrequalCal.CalcLockRequestLoan + this.calObjs.GFECal.CalcClosingCosts);
      this.AddFieldHandler("1161", this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.VACal.CalcVALA);
      this.AddFieldHandler("1163", this.calObjs.VERIFCal.CalcLiabilities);
      this.AddFieldHandler("1172", this.calObjs.D1003URLA2020Cal.CalcArmProg + this.calObjs.RegzCal.CalculateHELOCPayment + this.calObjs.HelocCal.CalcHELOCSharedCalculations + this.calObjs.D1003Cal.PopulateOtherFields + this.calObjs.PrequalCal.CalcDownPay + this.calObjs.D1003Cal.AdjConstTotal + this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.ATRQMCal.CalcPointsAndFees + this.calObjs.GFECal.CalcOthers + this.calObjs.GFECal.CalcGFEFees + this.calObjs.NewHud2015FeeDetailCal.Calc_2015AllFeeDetails + new Routine(((CalculationBase) this.calObjs.NewHudCal).FormCal) + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.LoansubCal.CalcLoansub + this.calObjs.ToolCal.PopulateCountyLimit + this.calObjs.MLDSCal.CalcMLDSScenarios + this.calObjs.HMDACal.CalcPrepaymentPenaltyPeriod + this.calObjs.CloserCal.CalOther + this.CalcLTV + this.calObjs.PrequalCal.CalcFHASecondaryResidence + this.calObjs.PrequalCal.CalcLockRequestLoan + this.calObjs.ATRQMCal.CalcAgencyGSEQMAvailability + this.calObjs.ATRQMCal.CalcEligibility + this.calObjs.NewHudCal.CalcGFEApplicationDate + this.calObjs.NewHud2015Cal.PopulateLoanEstimate_LoanPurpose + this.calObjs.NewHud2015Cal.PopulateLoanTypeOtherField + this.calObjs.NewHud2015Cal.CalcAPTable_FirstChangePayment + this.calObjs.NewHud2015Cal.CalcMICReference + this.calObjs.VACal.CalcMaximumSellerContribution + this.calObjs.HMDACal.CalcLoanType + this.calObjs.USDACal.CalcFirstTimeHomeBuyer + this.calObjs.VERIFCal.CalcHelocLineTotal + this.calObjs.HMDACal.CalcLoanAmount + this.calObjs.HMDACal.CalcIntroRatePeriod + this.calObjs.D1003URLA2020Cal.UpdateLinkedVoal + this.calObjs.VERIFCal.CalcHelocLineTotal + this.calObjs.HelocCal.CalcHELOCPeriodicRates + this.calObjs.RegzCal.CalcMaturityDate + this.calObjs.VACal.CalcVAStatutoryRecoupment + this.calObjs.NewHudCal.EnforceGFEAppDateRule + this.calObjs.NewHudCal.CalcInitialDisclosureDueDate + this.calObjs.HMDACal.CalcInterestRate + new Routine(this.calObjs.GFECal.CalculateFunder));
      this.AddFieldHandler("337", this.calObjs.GFECal.CalcPrepaid + this.calObjs.NewHud2015Cal.CalcClosingDisclosureTotalPayments);
      this.AddFieldHandler("1176", this.calObjs.NewHud2015Cal.CalcConstructionPermPeriod + this.calObjs.NewHud2015Cal.SetAIRTableIndexType + this.calObjs.RegzCal.CalcConstructionDates + this.calObjs.Hud1Cal.CalcEscrowFirstPaymentDate + this.calObjs.Hud1Cal.CalcHUD1ES + this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.NewHud2015Cal.CalcWillHaveEscrowAccount + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + this.calObjs.NewHud2015Cal.CalculateProductDescription + this.calObjs.NewHud2015Cal.CalcAPTable_FirstChangePayment + this.calObjs.HMDACal.CalcLoanTerm + this.calObjs.NewHud2015Cal.CalcLoanTermTable_InterestRate + this.calObjs.HMDACal.CalcIntroRatePeriod + this.calObjs.HMDACal.CalcHmdaInterestOnlyIndicator + this.calObjs.HMDACal.CalcNewHmdaInterestOnlyIndicator + this.calObjs.HMDACal.CalcLoanTerm + this.calObjs.RegzCal.CalcPMIMidpointCancelationDate);
      this.AddFieldHandler("1177", this.calObjs.D1003Cal.CalcIntrOnly + this.calObjs.NewHud2015Cal.CalcConstructionPermPeriod + this.calObjs.NewHud2015Cal.SetAIRTableIndexType + this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + this.calObjs.NewHud2015Cal.CalcLoanTermTable_MonthlyPrincipalAndInterest + this.calObjs.NewHud2015Cal.CalcAPTable_InterestOnlyPayments + this.calObjs.NewHud2015Cal.CalcAPTable_FirstChangePayment + this.calObjs.NewHud2015Cal.CalcLoanTermTable_MonthlyPrincipalAndInterest + this.calObjs.NewHud2015Cal.CalculateProductDescription + this.calObjs.HMDACal.CalcHmdaInterestOnlyIndicator + this.calObjs.HMDACal.CalcNewHmdaInterestOnlyIndicator);
      this.AddFieldHandler("2852", this.calObjs.GFECal.CalcGFEFees + new Routine(this.calculateUCD) + this.calObjs.NewHud2015Cal.CalcLoanEstimateTotalAdjustmentsCredit + this.calObjs.NewHud2015Cal.CalcLoanEstimateThirdPartyPaymentsNotOtherwiseDisclosed + this.calObjs.NewHud2015Cal.CalcLoanEstimate_ClosingCostsFinanced + this.calObjs.NewHud2015Cal.CalcLoanEstimateDownPaymentAndFundsForBorrower + this.calObjs.NewHud2015Cal.CalcLoanEstimate_EstimatedCashToClose + this.calObjs.VACal.CalcVAStatutoryRecoupment);
      this.AddFieldHandler("2982", this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + this.calObjs.NewHud2015Cal.CalculateProductDescription);
      this.AddFieldHandler("1198", this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + this.calObjs.NewHud2015Cal.CalcAPTable_MaximumPayment);
      this.AddFieldHandler("1199", this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.FHACal.CalcEEMWorksheet + this.calObjs.NewHud2015Cal.CalcPrepaidMI + this.calObjs.RegzCal.CalcAPR + this.calObjs.ATRQMCal.CalcEligibility);
      this.AddFieldHandler("1200", this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + this.calObjs.NewHud2015Cal.CalcAPTable_MaximumPayment);
      this.AddFieldHandler("1205", this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + this.calObjs.NewHud2015Cal.CalcAPTable_MaximumPayment);
      this.AddFieldHandler("1757", this.calObjs.NewHud2015Cal.CalcPrepaidMI + this.CalcMortgageInsurance);
      this.AddFieldHandler("1209", this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.PrequalCal.CalcRentOwn + this.calObjs.VACal.CalcVALA + this.calObjs.Hud1Cal.CalcHUD1ES + this.calObjs.GFECal.CalcGFEFees + new Routine(this.calculateUCD) + this.calObjs.GFECal.CalcTotalCosts + this.calObjs.D1003URLA2020Cal.CalcOtherCredits + this.calObjs.FHACal.CalcMACAWP + this.calObjs.NewHud2015Cal.CalcPrepaidMI + this.calObjs.NewHud2015Cal.CalcAPTable_MaximumPayment);
      this.AddFieldHandler("1257", this.calObjs.D1003Cal.CopyBrokerInfoToLender + this.calObjs.ToolCal.CalcStatementOfDenial);
      this.AddFieldHandler("1258", this.calObjs.D1003Cal.CopyBrokerInfoToLender + this.calObjs.ToolCal.CalcStatementOfDenial);
      this.AddFieldHandler("1259", this.calObjs.D1003Cal.CopyBrokerInfoToLender + this.calObjs.ToolCal.CalcStatementOfDenial);
      this.AddFieldHandler("1260", this.calObjs.D1003Cal.CopyBrokerInfoToLender + this.calObjs.ToolCal.CalcStatementOfDenial);
      this.AddFieldHandler("1262", this.calObjs.D1003Cal.CopyBrokerInfoToLender + this.calObjs.ToolCal.CalcStatementOfDenial + this.calObjs.NewHud2015Cal.PopulateLoanEstimate_LenderBrokerInfo);
      this.AddFieldHandler("1264", this.calObjs.D1003Cal.CopyBrokerInfoToLender + this.calObjs.D1003URLA2020Cal.UpdateLinkedVoal + this.calObjs.ToolCal.CalcStatementOfDenial);
      this.AddFieldHandler("1265", this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR);
      this.AddFieldHandler("1266", this.calObjs.D1003Cal.PopulateOtherFields + this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + this.calObjs.NewHud2015Cal.CalcAPTable_MaximumPayment + this.calObjs.NewHud2015Cal.CalculateProductDescription);
      this.AddFieldHandler("1267", this.calObjs.D1003Cal.PopulateOtherFields + this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + this.calObjs.NewHud2015Cal.CalcAPTable_MaximumPayment);
      this.AddFieldHandler("1269", this.calObjs.RegzCal.BuildPaySchedule + new Routine(((CalculationBase) this.calObjs.NewHudCal).FormCal) + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + this.calObjs.NewHud2015Cal.CalcAPTable_MaximumPayment);
      this.AddFieldHandler("1270", this.calObjs.RegzCal.BuildPaySchedule + new Routine(((CalculationBase) this.calObjs.NewHudCal).FormCal) + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + this.calObjs.NewHud2015Cal.CalcAPTable_MaximumPayment + this.calObjs.D1003Cal.CopyBuyDownToLockRequest);
      this.AddFieldHandler("1271", this.calObjs.RegzCal.BuildPaySchedule + new Routine(((CalculationBase) this.calObjs.NewHudCal).FormCal) + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + this.calObjs.NewHud2015Cal.CalcAPTable_MaximumPayment + this.calObjs.D1003Cal.CopyBuyDownToLockRequest);
      this.AddFieldHandler("1272", this.calObjs.RegzCal.BuildPaySchedule + new Routine(((CalculationBase) this.calObjs.NewHudCal).FormCal) + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + this.calObjs.NewHud2015Cal.CalcAPTable_MaximumPayment + this.calObjs.D1003Cal.CopyBuyDownToLockRequest);
      this.AddFieldHandler("1273", this.calObjs.RegzCal.BuildPaySchedule + new Routine(((CalculationBase) this.calObjs.NewHudCal).FormCal) + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + this.calObjs.NewHud2015Cal.CalcAPTable_MaximumPayment + this.calObjs.D1003Cal.CopyBuyDownToLockRequest);
      this.AddFieldHandler("1274", this.calObjs.RegzCal.BuildPaySchedule + new Routine(((CalculationBase) this.calObjs.NewHudCal).FormCal) + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + this.calObjs.NewHud2015Cal.CalcAPTable_MaximumPayment + this.calObjs.D1003Cal.CopyBuyDownToLockRequest);
      this.AddFieldHandler("4535", this.calObjs.RegzCal.CalcBuydownIndicator + this.calObjs.RegzCal.BuildPaySchedule + new Routine(((CalculationBase) this.calObjs.NewHudCal).FormCal) + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + this.calObjs.NewHud2015Cal.CalcAPTable_MaximumPayment + this.calObjs.D1003Cal.CopyBuyDownToLockRequest);
      this.AddFieldHandler("4536", this.calObjs.RegzCal.BuildPaySchedule + new Routine(((CalculationBase) this.calObjs.NewHudCal).FormCal) + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + this.calObjs.NewHud2015Cal.CalcAPTable_MaximumPayment + this.calObjs.D1003Cal.CopyBuyDownToLockRequest);
      this.AddFieldHandler("4537", this.calObjs.RegzCal.BuildPaySchedule + new Routine(((CalculationBase) this.calObjs.NewHudCal).FormCal) + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + this.calObjs.NewHud2015Cal.CalcAPTable_MaximumPayment + this.calObjs.D1003Cal.CopyBuyDownToLockRequest);
      this.AddFieldHandler("4538", this.calObjs.RegzCal.BuildPaySchedule + new Routine(((CalculationBase) this.calObjs.NewHudCal).FormCal) + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + this.calObjs.NewHud2015Cal.CalcAPTable_MaximumPayment + this.calObjs.D1003Cal.CopyBuyDownToLockRequest);
      this.AddFieldHandler("4539", this.calObjs.RegzCal.BuildPaySchedule + new Routine(((CalculationBase) this.calObjs.NewHudCal).FormCal) + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + this.calObjs.NewHud2015Cal.CalcAPTable_MaximumPayment + this.calObjs.D1003Cal.CopyBuyDownToLockRequest);
      this.AddFieldHandler("4540", this.calObjs.RegzCal.BuildPaySchedule + new Routine(((CalculationBase) this.calObjs.NewHudCal).FormCal) + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + this.calObjs.NewHud2015Cal.CalcAPTable_MaximumPayment + this.calObjs.D1003Cal.CopyBuyDownToLockRequest);
      this.AddFieldHandler("4645", this.calObjs.RegzCal.BuildPaySchedule + new Routine(((CalculationBase) this.calObjs.NewHudCal).FormCal) + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + this.calObjs.NewHud2015Cal.CalcAPTable_MaximumPayment + this.calObjs.D1003Cal.CopyBuyDownToLockRequest);
      this.AddFieldHandler("1335", this.calObjs.PrequalCal.CalcDownPay + this.calObjs.RegzCal.CalculateHELOCPayment + this.calObjs.ToolCal.CalcLOCompensation + new Routine(this.calObjs.ToolCal.UpdatePurchaseAdviceBalance) + this.calObjs.VERIFCal.CalcHelocLineTotal + this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003URLA2020Cal.CalcTotalMortgageAndTaxes + this.calObjs.PrequalCal.CalcRentOwn + this.calObjs.FHACal.CalcExisting23KDebt + this.calObjs.FHACal.CalcMAX23K + this.calObjs.FHACal.CalcMACAWP + this.calObjs.GFECal.CalcPrepaid + this.calObjs.GFECal.CalcGFEFees + this.calObjs.GFECal.CalcOthers + this.calObjs.GFECal.CalcTotalCosts + this.calObjs.D1003URLA2020Cal.CalcOtherCredits + this.calObjs.NewHudCal.CopySpecialHUD2010ToGFE2010 + new Routine(this.calculateUCD) + new Routine(((CalculationBase) this.calObjs.NewHudCal).FormCal) + this.CalcLTV + this.calObjs.ToolCal.CalcProfit + this.calObjs.MLDSCal.CalcMLDSScenarios + this.calObjs.HMDACal.CalcPrepaymentPenaltyPeriod + this.calObjs.PrequalCal.CalcLockRequestLoan + this.calObjs.ATRQMCal.CalcPrepaymentPenaltyPercent + this.calObjs.ATRQMCal.CalcDiscountPoints + this.calObjs.NewHud2015Cal.CalcLoanEstimate_ClosingCostsFinanced + this.calObjs.NewHud2015Cal.CalcLoanEstimateDownPaymentAndFundsForBorrower + this.calObjs.NewHud2015Cal.CalcCDPage3StdCashToCloseFinal + this.calObjs.NewHud2015Cal.Calc2015LineItem904 + this.calObjs.RegzCal.CalcPointsInInitialAdjustedRate);
      this.AddFieldHandler("1402", this.calObjs.D1003Cal.CopyCitizenshipAndAge + this.CalcVAAccount + this.calObjs.HMDACal.CalcHmdaAge + this.calObjs.ToolCal.CalcConsumerHIOrderEligible);
      this.AddFieldHandler("1403", this.calObjs.D1003Cal.CopyCitizenshipAndAge + this.CalcVAAccount + this.calObjs.HMDACal.CalcHmdaAge);
      this.AddFieldHandler("745", this.calObjs.D1003Cal.CopyCitizenshipAndAge + this.CalcVAAccount + this.calObjs.HMDACal.CalcHmdaAge);
      this.AddFieldHandler("1405", this.calObjs.VERIFCal.CalcMaintenanceExpenseAmount + this.calObjs.FHACal.CalcEEMWorksheet + this.calObjs.D1003Cal.CalcHousingExp + new Routine(this.calObjs.VERIFCal.CalculateMultipleVOMs) + this.calObjs.D1003URLA2020Cal.CalcTotalMortgageAndTaxes + this.calObjs.PrequalCal.CalcRentOwn + this.calObjs.VACal.CalcVALA + this.calObjs.GFECal.CalcGFEFees + this.calObjs.GFECal.CalcPrepaid + this.calObjs.Hud1Cal.CalcHUD1ES + this.calObjs.NewHudCal.CalcREGZGFE + this.calObjs.NewHudCal.CalcHUD1PG2 + this.calObjs.NewHudCal.CalcHUD1PG3 + this.calObjs.NewHudCal.CalcREGZGFEHud + this.calObjs.GFECal.CalcTotalCosts + this.calObjs.D1003URLA2020Cal.CalcOtherCredits + this.calObjs.FHACal.CalcMACAWP + this.calObjs.NewHud2015Cal.CalculateLoanEstimate_ComparisonsTable + this.calObjs.D1003URLA2020Cal.CalcEstimatedNetMonthlyRent + this.calObjs.NewHud2015Cal.Calc2015LineItem904 + this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem904);
      this.AddFieldHandler("1413", this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR);
      this.AddFieldHandler("1414", this.calObjs.PrequalCal.CalcMiddleFICO + this.calObjs.PrequalCal.CalcLoanComparison + this.calObjs.D1003Cal.CopyInfoToLockRequest);
      this.AddFieldHandler("1415", this.calObjs.PrequalCal.CalcMiddleFICO + this.calObjs.PrequalCal.CalcLoanComparison + this.calObjs.D1003Cal.CopyInfoToLockRequest);
      this.AddFieldHandler("1450", this.calObjs.PrequalCal.CalcMiddleFICO + this.calObjs.PrequalCal.CalcLoanComparison + this.calObjs.D1003Cal.CopyInfoToLockRequest);
      this.AddFieldHandler("1452", this.calObjs.PrequalCal.CalcMiddleFICO + this.calObjs.PrequalCal.CalcLoanComparison + this.calObjs.D1003Cal.CopyInfoToLockRequest);
      this.AddFieldHandler("1483", this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.RegzCal.CalculateHELOCPayment + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + this.calObjs.RegzCal.CalcHELOCInitialPayment + this.calObjs.D1003URLA2020Cal.CalcEstimatedNetMonthlyRent);
      this.AddFieldHandler("4494", this.calObjs.D1003Cal.CalcLienPosition + this.calObjs.D1003URLA2020Cal.UpdateLinkedVoal + this.calObjs.VERIFCal.CalcHelocLineTotal);
      this.AddFieldHandler("1482", this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + this.calObjs.PrequalCal.CalcHELOCLockRequest + this.calObjs.HelocCal.CalcHELOCPeriodicRates + this.calObjs.HMDACal.CalcInterestRate);
      this.AddFieldHandler("1888", this.calObjs.RegzCal.CalculateHELOCPayment + this.calObjs.RegzCal.CalcHELOCInitialPayment + this.calObjs.D1003URLA2020Cal.CalcTotalNewMortgageLoans + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003URLA2020Cal.UpdateLinkedVoal + this.calObjs.D1003URLA2020Cal.CalcTotalNewMortgageLoans + this.calObjs.VERIFCal.CalcHelocLineTotal + new Routine(this.calculateLTV) + this.calObjs.PrequalCal.CalcHELOCLockRequest + this.calObjs.PrequalCal.CalcLockRequestLoan + new Routine(this.calObjs.GFECal.CalculateFunder));
      this.AddFieldHandler("1613", this.calObjs.RegzCal.BuildPaySchedule + new Routine(((CalculationBase) this.calObjs.NewHudCal).FormCal) + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + this.calObjs.NewHud2015Cal.CalcAPTable_MaximumPayment + this.calObjs.D1003Cal.CopyBuyDownToLockRequest);
      this.AddFieldHandler("1614", this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.RegzCal.CalcHELOCInitialPayment + new Routine(((CalculationBase) this.calObjs.NewHudCal).FormCal) + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + this.calObjs.NewHud2015Cal.CalcAPTable_MaximumPayment + this.calObjs.D1003Cal.CopyBuyDownToLockRequest);
      this.AddFieldHandler("1615", this.calObjs.RegzCal.BuildPaySchedule + new Routine(((CalculationBase) this.calObjs.NewHudCal).FormCal) + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + this.calObjs.NewHud2015Cal.CalcAPTable_MaximumPayment + this.calObjs.D1003Cal.CopyBuyDownToLockRequest);
      this.AddFieldHandler("1616", this.calObjs.RegzCal.BuildPaySchedule + new Routine(((CalculationBase) this.calObjs.NewHudCal).FormCal) + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + this.calObjs.NewHud2015Cal.CalcAPTable_MaximumPayment + this.calObjs.D1003Cal.CopyBuyDownToLockRequest);
      this.AddFieldHandler("1617", this.calObjs.RegzCal.BuildPaySchedule + new Routine(((CalculationBase) this.calObjs.NewHudCal).FormCal) + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + this.calObjs.NewHud2015Cal.CalcAPTable_MaximumPayment + this.calObjs.D1003Cal.CopyBuyDownToLockRequest);
      this.AddFieldHandler("1618", this.calObjs.RegzCal.BuildPaySchedule + new Routine(((CalculationBase) this.calObjs.NewHudCal).FormCal) + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + this.calObjs.NewHud2015Cal.CalcAPTable_MaximumPayment + this.calObjs.D1003Cal.CopyBuyDownToLockRequest);
      this.AddFieldHandler("4541", this.calObjs.RegzCal.BuildPaySchedule + new Routine(((CalculationBase) this.calObjs.NewHudCal).FormCal) + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + this.calObjs.NewHud2015Cal.CalcAPTable_MaximumPayment + this.calObjs.D1003Cal.CopyBuyDownToLockRequest);
      this.AddFieldHandler("4542", this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.RegzCal.CalcHELOCInitialPayment + new Routine(((CalculationBase) this.calObjs.NewHudCal).FormCal) + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + this.calObjs.NewHud2015Cal.CalcAPTable_MaximumPayment + this.calObjs.D1003Cal.CopyBuyDownToLockRequest);
      this.AddFieldHandler("4543", this.calObjs.RegzCal.BuildPaySchedule + new Routine(((CalculationBase) this.calObjs.NewHudCal).FormCal) + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + this.calObjs.NewHud2015Cal.CalcAPTable_MaximumPayment + this.calObjs.D1003Cal.CopyBuyDownToLockRequest);
      this.AddFieldHandler("4544", this.calObjs.RegzCal.BuildPaySchedule + new Routine(((CalculationBase) this.calObjs.NewHudCal).FormCal) + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + this.calObjs.NewHud2015Cal.CalcAPTable_MaximumPayment + this.calObjs.D1003Cal.CopyBuyDownToLockRequest);
      this.AddFieldHandler("4545", this.calObjs.RegzCal.BuildPaySchedule + new Routine(((CalculationBase) this.calObjs.NewHudCal).FormCal) + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + this.calObjs.NewHud2015Cal.CalcAPTable_MaximumPayment + this.calObjs.D1003Cal.CopyBuyDownToLockRequest);
      this.AddFieldHandler("4546", this.calObjs.RegzCal.BuildPaySchedule + new Routine(((CalculationBase) this.calObjs.NewHudCal).FormCal) + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + this.calObjs.NewHud2015Cal.CalcAPTable_MaximumPayment + this.calObjs.D1003Cal.CopyBuyDownToLockRequest);
      this.AddFieldHandler("1619", this.calObjs.GFECal.CalcGFEFees + this.calObjs.LoansubCal.CalcLoansub + this.calObjs.VACal.CalcVARRRWS);
      this.AddFieldHandler("1620", this.calObjs.GFECal.CalcGFEFees + this.calObjs.LoansubCal.CalcLoansub);
      this.AddFieldHandler("1628", this.calObjs.D1003URLA2020Cal.CalcProposedSupplementalPropertyInsurance + this.calObjs.D1003URLA2020Cal.CalcSupplementalInsuranceAmount + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.Hud1Cal.CalcHUD1ES + this.calObjs.FHACal.CalcMACAWP + this.calObjs.FHACal.CalcFredMac + this.calObjs.GFECal.CalcPrepaid + this.calObjs.VACal.CalcVARecoupment);
      this.AddFieldHandler("340", this.calObjs.GFECal.CalcPrepaid + this.calObjs.ToolCal.CalcCPAEscrowDetails_FCD_AmountFields);
      this.AddFieldHandler("341", this.calObjs.GFECal.CalcPrepaid + this.calObjs.ToolCal.CalcCPAEscrowDetails_FCD_AmountFields);
      this.AddFieldHandler("1296", this.calObjs.GFECal.CalcPrepaid + this.calObjs.ToolCal.CalcCPAEscrowDetails_FCD_AmountFields);
      this.AddFieldHandler("1386", this.calObjs.GFECal.CalcPrepaid + this.calObjs.ToolCal.CalcCPAEscrowDetails_FCD_AmountFields);
      this.AddFieldHandler("1387", this.calObjs.GFECal.CalcPrepaid + this.calObjs.ToolCal.CalcCPAEscrowDetails_FCD_AmountFields);
      this.AddFieldHandler("1388", this.calObjs.GFECal.CalcPrepaid + this.calObjs.ToolCal.CalcCPAEscrowDetails_FCD_AmountFields);
      this.AddFieldHandler("1629", this.calObjs.GFECal.CalcPrepaid + this.calObjs.ToolCal.CalcCPAEscrowDetails_FCD_AmountFields);
      this.AddFieldHandler("L267", this.calObjs.GFECal.CalcPrepaid + this.calObjs.ToolCal.CalcCPAEscrowDetails_FCD_AmountFields);
      this.AddFieldHandler("NEWHUD.X1706", this.calObjs.GFECal.CalcPrepaid + this.calObjs.ToolCal.CalcCPAEscrowDetails_FCD_AmountFields);
      this.AddFieldHandler("1630", this.calObjs.D1003URLA2020Cal.CalcProposedSupplementalPropertyInsurance + this.calObjs.D1003URLA2020Cal.CalcSupplementalInsuranceAmount + this.calObjs.NewHud2015Cal.CalcPropertyInsuranceTaxIndicators + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003URLA2020Cal.CalcTotalMortgageAndTaxes + this.calObjs.VERIFCal.CalcMaintenanceExpenseAmount + this.calObjs.NewHud2015Cal.CalcLoanEstimateTaxesInsuranceInEscrowOrNot + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003URLA2020Cal.CalcTotalMortgageAndTaxes + this.calObjs.Hud1Cal.CalcHUD1ES + this.calObjs.FHACal.CalcMACAWP + this.calObjs.FHACal.CalcFredMac + this.calObjs.GFECal.CalcPrepaid + new Routine(this.calculateUCD) + this.calObjs.Hud1Cal.CalcBiWeeklyEscrowPayment + this.calObjs.VACal.CalcVARecoupment + this.calObjs.NewHud2015Cal.CalcEstimatedPropertyCosts + this.calObjs.ToolCal.CalcCPAEscrowDetails_FCD_AmountFields);
      this.AddFieldHandler("CASASRN.X78", this.calObjs.FHACal.CalcFredMac + this.calObjs.GFECal.CalcPrepaid + new Routine(this.calculateUCD) + this.calObjs.Hud1Cal.CalcBiWeeklyEscrowPayment + this.calObjs.VACal.CalcVARecoupment + this.calObjs.NewHud2015Cal.CalcEstimatedPropertyCosts + this.calObjs.ToolCal.CalcCPAEscrowDetails_FCD_AmountFields);
      this.AddFieldHandler("1636", new Routine(this._gfeCalculationServant.UpdateCityStateUserFees) + this.calObjs.NewHudCal.CalcREGZGFE + this.calObjs.NewHudCal.CalcClosingCost);
      this.AddFieldHandler("1638", new Routine(this._gfeCalculationServant.UpdateCityStateUserFees) + this.calObjs.NewHudCal.CalcREGZGFE + this.calObjs.NewHudCal.CalcClosingCost);
      this.AddFieldHandler("1659", this.calObjs.RegzCal.CalcAPR + this.calObjs.NewHud2015Cal.CalcAPTable_MaximumPayment + this.calObjs.NewHud2015Cal.CalculateProductDescription);
      this.AddFieldHandler("1660", this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.LoansubCal.CalcLoansub);
      this.AddFieldHandler("1677", this.calObjs.RegzCal.SychConstructionRateNoteRate + new Routine(this.calculateUCD) + this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + this.calObjs.NewHud2015Cal.CalculateProductDescription + this.calObjs.RegzCal.CalcInitialInterestRate + this.calObjs.NewHud2015Cal.CalcLoanTermTable_InterestRate + this.calObjs.NewHud2015Cal.SetAIRTableIndexType + this.calObjs.HMDACal.CalcIntroRatePeriod);
      this.AddFieldHandler("1678", this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR);
      this.AddFieldHandler("1699", this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + this.calObjs.NewHud2015Cal.CalcAPTable_MaximumPayment + this.calObjs.RegzCal.CalcFirstAdjustmentMinimum + this.calObjs.NewHud2015Cal.CalcLECDDisplayFields);
      this.AddFieldHandler("1700", this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + this.calObjs.NewHud2015Cal.CalcAPTable_MaximumPayment);
      this.AddFieldHandler("1701", this.calObjs.RegzCal.CalcPMIMidpointCancelationDate);
      this.AddFieldHandler("1712", this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + this.calObjs.NewHud2015Cal.CalcAPTable_MaximumPayment + this.calObjs.NewHud2015Cal.CalculateProductDescription);
      this.AddFieldHandler("1713", this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + this.calObjs.NewHud2015Cal.CalcAPTable_MaximumPayment + this.calObjs.NewHud2015Cal.CalculateProductDescription);
      this.AddFieldHandler("1716", this.calObjs.D1003Cal.CalcLiquidAssets + this.calObjs.GFECal.CalcGFEFees + this.calObjs.GFECal.CalcPrepaid + this.calObjs.GFECal.CalcTotalCosts + this.calObjs.D1003URLA2020Cal.CalcOtherCredits);
      this.AddFieldHandler("1732", this.calObjs.D1003Cal.PopulateOtherFields + this.calObjs.D1003Cal.CalcHousingExp + this.CalcLTV + this.calObjs.PrequalCal.CalcLockRequestLoan + this.calObjs.VERIFCal.CalcHelocLineTotal);
      this.AddFieldHandler("1753", this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR);
      this.AddFieldHandler("1760", this.calObjs.FHACal.CalcCAWRefi + this.calObjs.D1003Cal.CalculatePMIIndicator + this.calObjs.RegzCal.CalcPointsInInitialAdjustedRate);
      this.AddFieldHandler("1766", this.calObjs.PrequalCal.CalcMIP + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.PrequalCal.CalcRentOwn + this.calObjs.VACal.CalcVALA + this.calObjs.Hud1Cal.CalcHUD1ES + this.calObjs.GFECal.CalcTotalCosts + this.calObjs.D1003URLA2020Cal.CalcOtherCredits + this.calObjs.FHACal.CalcMACAWP + this.calObjs.RegzCal.CalcIRS1098 + this.calObjs.NewHud2015Cal.CalcAPTable_MaximumPayment);
      this.AddFieldHandler("1770", this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + this.calObjs.NewHud2015Cal.CalcAPTable_MaximumPayment);
      this.AddFieldHandler("1771", this.calObjs.PrequalCal.CalcDownPay + this.calObjs.RegzCal.CalculateHELOCPayment + this.calObjs.ToolCal.CalcLOCompensation + new Routine(this.calObjs.ToolCal.UpdatePurchaseAdviceBalance) + this.calObjs.VERIFCal.CalcHelocLineTotal + this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003URLA2020Cal.CalcTotalMortgageAndTaxes + this.calObjs.PrequalCal.CalcRentOwn + this.calObjs.FHACal.CalcExisting23KDebt + this.calObjs.FHACal.CalcMAX23K + this.calObjs.FHACal.CalcMACAWP + this.calObjs.GFECal.CalcPrepaid + this.calObjs.GFECal.CalcGFEFees + this.calObjs.GFECal.CalcOthers + new Routine(this.calculateUCD) + this.calObjs.NewHudCal.CopySpecialHUD2010ToGFE2010 + new Routine(((CalculationBase) this.calObjs.NewHudCal).FormCal) + this.calObjs.GFECal.CalcTotalCosts + this.calObjs.D1003URLA2020Cal.CalcOtherCredits + this.CalcLTV + this.calObjs.PrequalCal.CalcMinMax + this.calObjs.ToolCal.CalcProfit + this.calObjs.MLDSCal.CalcMLDSScenarios + this.calObjs.HMDACal.CalcPrepaymentPenaltyPeriod + this.calObjs.PrequalCal.CalcLockRequestLoan + this.calObjs.ATRQMCal.CalcPrepaymentPenaltyPercent + this.calObjs.ATRQMCal.CalcDiscountPoints + this.calObjs.NewHud2015Cal.CalcLoanEstimateDownPaymentAndFundsForBorrower + this.calObjs.NewHud2015Cal.Calc2015LineItem904 + this.calObjs.RegzCal.CalcPointsInInitialAdjustedRate);
      this.AddFieldHandler("1775", this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR);
      this.AddFieldHandler("1799", this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.VACal.CalcVARecoupment);
      this.AddFieldHandler("1801", this.calObjs.D1003URLA2020Cal.CalcTotalMortgageAndTaxes + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.VACal.CalcVARecoupment);
      this.AddFieldHandler("1811", this.calObjs.D1003Cal.PopulateOtherFields + this.calObjs.D1003URLA2020Cal.CalcNonSubjectPropertyDebtsToBePaidOffAmount + this.calObjs.GFECal.CalcGFEFees + this.calObjs.RegzCal.CalculateHELOCPayment + this.calObjs.VERIFCal.CalcLiabilities + new Routine(this.calObjs.VERIFCal.CalcRealEstate) + this.calObjs.D1003URLA2020Cal.CalcNonSubjectPropertyDebtsToBePaidOffAmount + this.calObjs.D1003URLA2020Cal.CalcSupplementalInsuranceAmount + this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + new Routine(this.calObjs.VERIFCal.CalculateMultipleVOMs) + this.calObjs.VERIFCal.CalcLiabilities + this.calObjs.D1003URLA2020Cal.CalcTotalMortgageAndTaxes + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.GFECal.CalcHighPrice + this.calObjs.GFECal.CalcSecondAppraisalRequired + this.calObjs.FHACal.CalcFHA203K + this.calObjs.RegzCal.CalDisclosureLog2015 + this.calObjs.HMDACal.CalcHOEPAStatus + this.calObjs.D1003URLA2020Cal.CalcEstimatedNetMonthlyRent + this.calObjs.PrequalCal.CalcFHASecondaryResidence);
      this.AddFieldHandler("1821", this.calObjs.D1003Cal.PopulateOtherFields + this.CalcLTV + this.calObjs.PrequalCal.CalcLockRequestLoan + this.calObjs.NewHudCal.EnforceGFEAppDateRule + this.calObjs.NewHudCal.CalcInitialDisclosureDueDate);
      this.AddFieldHandler("1826", this.calObjs.D1003Cal.CalculatePMIIndicator + this.calObjs.RegzCal.CalcPointsInInitialAdjustedRate);
      this.AddFieldHandler("1853", this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR);
      this.AddFieldHandler("1887", this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.ATRQMCal.CalcPointsAndFees + this.calObjs.ATRQMCal.CalcEligibility + this.calObjs.VACal.CalcVAStatutoryRecoupment + this.calObjs.GFECal.CalcSection32);
      this.AddFieldHandler("1961", this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR);
      this.AddFieldHandler("1962", new Routine(this.calculateUCD) + this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.RegzCal.CalcHELOCInitialPayment + this.calObjs.D1003Cal.CalcHousingExp + new Routine(this.calculateUCD) + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.HelocCal.CalcHELOCPeriodicRates + this.calObjs.FHACal.CalcEEMWorksheet + this.calObjs.FHACal.CalcEEMX88AndEEMX89);
      this.AddFieldHandler("1963", this.calObjs.Hud1Cal.CalcEscrowFirstPaymentDate + this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + this.calObjs.HMDACal.CalcIntroRatePeriod);
      this.AddFieldHandler("1964", this.calObjs.RegzCal.CalcLotLandStatus + this.calObjs.NewHud2015Cal.CalculateLotOwnedFreeAndClearIndicator + this.calObjs.NewHud2015Cal.CalcDisclosedSalesPrice + this.calObjs.NewHud2015Cal.PopulateLoanEstimate_LoanPurpose + this.calObjs.ToolCal.ClearAmtOfInitialAdvance + this.calObjs.GFECal.CalcPrepaid + this.CalcLTV + this.calObjs.PrequalCal.CalcCopyConstructionFields + this.calObjs.PrequalCal.CalcLockRequestLoan + this.calObjs.D1003URLA2020Cal.CalcURLALoanPurposeMapping);
      this.AddFieldHandler("Constr.Refi", this.calObjs.RegzCal.CalcLotLandStatus + this.calObjs.NewHud2015Cal.CalculateLotOwnedFreeAndClearIndicator + this.calObjs.NewHud2015Cal.PopulateLoanEstimate_LoanPurpose + this.CalcLTV + this.calObjs.PrequalCal.CalcCopyConstructionFields + this.calObjs.PrequalCal.CalcLockRequestLoan + this.calObjs.D1003URLA2020Cal.CalcURLALoanPurposeMapping + this.calObjs.PrequalCal.ClearLockRequestFreeAndClear);
      this.AddFieldHandler("1969", this.calObjs.D1003Cal.CopyBrokerInfoToLender + this.calObjs.NewHud2015Cal.CalcClosingDisclosurePage5 + this.calObjs.NewHud2015Cal.PopulateLoanEstimate_LenderBrokerInfo);
      this.AddFieldHandler("3629", this.calObjs.D1003Cal.CopyBrokerInfoToLender + this.calObjs.NewHud2015Cal.PopulateLoanEstimate_LenderBrokerInfo);
      this.AddFieldHandler("2307", this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR);
      this.AddFieldHandler("2400", new Routine(((CalculationBase) this.calObjs.NewHudCal).FormCal) + this.calObjs.NewHudCal.CalcHUDGFEDisclosureInfo);
      this.AddFieldHandler("2551", this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + this.calObjs.NewHud2015Cal.CalcAPTable_MaximumPayment);
      this.AddFieldHandler("2552", this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR);
      this.AddFieldHandler("2553", this.calObjs.D1003Cal.PopulateOtherFields + this.calObjs.GFECal.CalcGFEFees + new Routine(this.calculateUCD) + new Routine(((CalculationBase) this.calObjs.NewHudCal).FormCal) + this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + new Routine(this.calObjs.VERIFCal.CalculateMultipleVOMs) + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + this.calObjs.HMDACal.CalcIntroRatePeriod + this.calObjs.HelocCal.CalcHELOCPeriodicRates + this.calObjs.VACal.CalcVAStatutoryRecoupment);
      this.AddFieldHandler("2626", this.calObjs.D1003Cal.CalcLenderChannel + this.calObjs.D1003Cal.CalcUli + this.calObjs.D1003Cal.CopyBrokerInfoToLender + this.calObjs.ToolCal.CalcLOCompensation + this.calObjs.NewHud2015Cal.PopulateLoanEstimate_LenderBrokerInfo + this.calObjs.NewHud2015Cal.CopyLoanNumToLoanIDNum + this.calObjs.ToolCal.CalcStatementOfDenial + this.calObjs.ToolCal.UpdateCorrespondentPrincipal);
      this.AddFieldHandler("2978", this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.PrequalCal.CalcRentOwn + this.calObjs.VACal.CalcVALA + this.calObjs.Hud1Cal.CalcHUD1ES + this.calObjs.GFECal.CalcGFEFees + this.calObjs.GFECal.CalcTotalCosts + this.calObjs.D1003URLA2020Cal.CalcOtherCredits + this.calObjs.FHACal.CalcMACAWP + this.calObjs.RegzCal.CalcAPR);
      this.AddFieldHandler("3134", this.calObjs.GFECal.CalcHighPrice + this.calObjs.GFECal.CalcSecondAppraisalRequired + this.calObjs.GFECal.CalcSection32 + this.calObjs.ATRQMCal.CalcHigherPricedCheck + this.calObjs.ATRQMCal.CalcEligibility);
      this.AddFieldHandler("3142", this.calObjs.NewHudCal.CalcInitialDisclosureDueDate + this.calObjs.NewHudCal.EnforceGFEAppDateRule + this.calObjs.D1003Cal.PopulateLoanAmountToNmlsApplicationAmounts + this.calObjs.PrequalCal.CalcMIP);
      this.AddFieldHandler("3164", this.calObjs.NewHudCal.CalcHUDGFEDisclosureInfo + this.calObjs.NewHud2015Cal.CalcLoanEstimate_ClosingCostExpDate + this.calObjs.ToolCal.SyncConstructionLinkSyncDefaultData + this.calObjs.NewHud2015Cal.CalcIntentToProceed + this.calObjs.ToolCal.CalcConsumerHIOrderEligible);
      this.AddFieldHandler("3331", this.calObjs.GFECal.CalcHighPrice + this.calObjs.D1003Cal.CalcNMLSResidentialMortgageType + this.calObjs.GFECal.CalcSecondAppraisalRequired);
      this.AddFieldHandler("3560", this.calObjs.D1003Cal.CalculatePMIIndicator);
      this.AddFieldHandler("3561", this.calObjs.D1003Cal.CalculatePMIIndicator);
      this.AddFieldHandler("3563", this.calObjs.D1003Cal.CalculatePMIIndicator);
      this.AddFieldHandler("4084", this.calObjs.NewHud2015Cal.SetCustomizedProjectedPaymentTable + this.calObjs.RegzCal.CalcAPR + this.calObjs.NewHud2015Cal.CalcAPTable_FirstChangePayment + this.calObjs.NewHud2015Cal.CalculateProductDescription + this.calObjs.NewHud2015Cal.PopulateLoanEstimate_LoanPurpose + this.CalcIsLSSecondaryFile + this.CalcLoanLinkSyncType + this.calObjs.HMDACal.CalcHMDAReporting);
      this.AddFieldHandler("LINKGUID", this.calObjs.HMDACal.CalcHMDAReporting + this.calObjs.VERIFCal.CalSubFin);
      this.AddFieldHandler("4086", this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.RegzCal.CalcAPR + this.calObjs.NewHud2015Cal.Calc60Payments);
      this.AddFieldHandler("4087", this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.RegzCal.CalcAPR + this.calObjs.NewHud2015Cal.Calc60Payments);
      this.AddFieldHandler("3292", this.calObjs.RegzCal.BuildPaySchedule);
      this.AddFieldHandler("CALCREQUIRED", this.calObjs.RegzCal.CalcOnDemand);
      this.AddFieldHandler("3136", this.calObjs.NewHudCal.CalcLoanOriginatorName);
      this.AddFieldHandler("NEWHUD.X807", this.calObjs.NewHudCal.CalcLoanOriginatorName);
      this.AddFieldHandler("NEWHUD.X808", this.calObjs.NewHudCal.CalcREGZGFEHud);
      this.AddFieldHandler("MAX23K.X5", this.calObjs.FHACal.CalcExisting23KDebt + this.calObjs.FHACal.CalcMAX23K + this.calObjs.RegzCal.BuildPaySchedule + new Routine(this.calculateLTV) + this.calObjs.PrequalCal.CalcLockRequestLoan);
      this.AddFieldHandler("MAX23K.X6", this.calObjs.FHACal.CalcExisting23KDebt + this.calObjs.FHACal.CalcMAX23K + this.calObjs.FHACal.CalcEEMWorksheet + this.calObjs.RegzCal.BuildPaySchedule + new Routine(this.calculateLTV) + this.calObjs.PrequalCal.CalcLockRequestLoan);
      this.AddFieldHandler("3000", this.calObjs.FHACal.CalcExisting23KDebt + this.calObjs.FHACal.CalcMAX23K + this.calObjs.FHACal.CalcEEMWorksheet + this.calObjs.RegzCal.BuildPaySchedule + new Routine(this.calculateLTV) + this.calObjs.PrequalCal.CalcLockRequestLoan);
      this.AddFieldHandler("MAX23K.X8", this.calObjs.FHACal.CalcExisting23KDebt + this.calObjs.FHACal.CalcMAX23K + new Routine(this.calculateLTV) + this.calObjs.PrequalCal.CalcLockRequestLoan);
      this.AddFieldHandler("MAX23K.X9", this.calObjs.FHACal.CalcExisting23KDebt + this.calObjs.FHACal.CalcMAX23K + this.calObjs.RegzCal.BuildPaySchedule + new Routine(this.calculateLTV) + this.calObjs.PrequalCal.CalcLockRequestLoan);
      this.AddFieldHandler("MAX23K.X70", this.calObjs.FHACal.CalcExisting23KDebt + this.calObjs.FHACal.CalcMAX23K + this.calObjs.RegzCal.BuildPaySchedule + new Routine(this.calculateLTV) + this.calObjs.PrequalCal.CalcLockRequestLoan);
      this.AddFieldHandler("MAX23K.X11", this.calObjs.FHACal.CalcExisting23KDebt + this.calObjs.FHACal.CalcMAX23K + this.calObjs.RegzCal.BuildPaySchedule);
      this.AddFieldHandler("MAX23K.X12", this.calObjs.FHACal.CalcExisting23KDebt + this.calObjs.FHACal.CalcMAX23K + this.calObjs.RegzCal.BuildPaySchedule);
      this.AddFieldHandler("MAX23K.X13", this.calObjs.FHACal.CalcExisting23KDebt + this.calObjs.FHACal.CalcMAX23K + this.calObjs.RegzCal.BuildPaySchedule);
      this.AddFieldHandler("MAX23K.X14", this.calObjs.FHACal.CalcExisting23KDebt + this.calObjs.FHACal.CalcMAX23K + this.calObjs.RegzCal.BuildPaySchedule);
      this.AddFieldHandler("MAX23K.X16", this.calObjs.FHACal.CalcExisting23KDebt + this.calObjs.FHACal.CalcMAX23K + this.calObjs.RegzCal.BuildPaySchedule);
      this.AddFieldHandler("MAX23K.X63", this.calObjs.FHACal.CalcExisting23KDebt + this.calObjs.FHACal.CalcMAX23K + this.calObjs.RegzCal.BuildPaySchedule);
      this.AddFieldHandler("MAX23K.X19", this.calObjs.FHACal.CalcExisting23KDebt + this.calObjs.FHACal.CalcMAX23K + this.calObjs.RegzCal.BuildPaySchedule + new Routine(this.calculateLTV) + this.calObjs.PrequalCal.CalcLockRequestLoan);
      this.AddFieldHandler("MAX23K.X77", this.calObjs.FHACal.CalcExisting23KDebt + this.calObjs.FHACal.CalcMAX23K + this.calObjs.RegzCal.BuildPaySchedule + new Routine(this.calculateLTV) + this.calObjs.PrequalCal.CalcLockRequestLoan);
      this.AddFieldHandler("MAX23K.X22", this.calObjs.FHACal.CalcExisting23KDebt + this.calObjs.FHACal.CalcMAX23K + this.calObjs.RegzCal.BuildPaySchedule + new Routine(this.calculateLTV) + this.calObjs.PrequalCal.CalcLockRequestLoan);
      this.AddFieldHandler("MAX23K.X23", this.calObjs.FHACal.CalcExisting23KDebt + this.calObjs.FHACal.CalcMAX23K + this.calObjs.RegzCal.BuildPaySchedule + new Routine(this.calculateLTV) + this.calObjs.PrequalCal.CalcLockRequestLoan);
      this.AddFieldHandler("MAX23K.X21", this.calObjs.FHACal.CalcExisting23KDebt + this.calObjs.FHACal.CalcMAX23K + this.calObjs.RegzCal.BuildPaySchedule + new Routine(this.calculateLTV) + this.calObjs.PrequalCal.CalcLockRequestLoan);
      this.AddFieldHandler("MAX23K.X24", this.calObjs.FHACal.CalcExisting23KDebt + this.calObjs.FHACal.CalcMAX23K + this.calObjs.RegzCal.BuildPaySchedule + new Routine(this.calculateLTV) + this.calObjs.PrequalCal.CalcLockRequestLoan);
      this.AddFieldHandler("MAX23K.X26", this.calObjs.FHACal.CalcExisting23KDebt + this.calObjs.FHACal.CalcMAX23K + this.calObjs.RegzCal.BuildPaySchedule + new Routine(this.calculateLTV) + this.calObjs.PrequalCal.CalcLockRequestLoan);
      this.AddFieldHandler("MAX23K.X44", this.calObjs.FHACal.CalcExisting23KDebt + this.calObjs.FHACal.CalcMAX23K + this.calObjs.RegzCal.BuildPaySchedule + new Routine(this.calculateLTV) + this.calObjs.PrequalCal.CalcLockRequestLoan);
      this.AddFieldHandler("MAX23K.X101", this.calObjs.FHACal.CalcExisting23KDebt + this.calObjs.FHACal.CalcFHA203K + new Routine(this.calculateLTV));
      this.AddFieldHandler("SYS.X1", this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + this.calObjs.NewHud2015Cal.CalcAPTable_MaximumPayment);
      this.AddFieldHandler("SYS.X2", new Routine(this.calculateUCD) + this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.GFECal.CalcPrepaid + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.NewHudCal.CalcREGZGFEHud + new Routine(this.calculateUCD) + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + this.calObjs.NewHud2015Cal.CalcAPTable_MaximumPayment);
      this.AddFieldHandler("SYS.X4", this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR);
      this.AddFieldHandler("SYS.X6", new Routine(this.calculateUCD) + this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + this.calObjs.NewHud2015Cal.CalcAPTable_MaximumPayment);
      this.AddFieldHandler("VEND.X293", this.calObjs.D1003Cal.CopyBrokerInfoToLender + new Routine(((CalculationBase) this.calObjs.NewHudCal).FormCal) + this.calObjs.NewHudCal.CalcLoanOriginatorName);
      this.AddFieldHandler("FL0011", this.calObjs.VERIFCal.CalcLiabilities + new Routine(this.calObjs.VERIFCal.CalcRealEstate) + this.calObjs.D1003URLA2020Cal.CalcTotalMortgageAndTaxes + this.calObjs.RegzCal.CalculateHELOCPayment + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.VERIFCal.CalcLiabilities + this.calObjs.D1003URLA2020Cal.CalcTotalMortgageAndTaxes + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.D1003URLA2020Cal.CalcEstimatedNetMonthlyRent);
      this.AddFieldHandler("FL0012", this.calObjs.D1003URLA2020Cal.CalcBackEndRatio + this.calObjs.VERIFCal.CalcLiabilities + this.calObjs.D1003Cal.CalcLoansubAndTSUM);
      this.AddFieldHandler("FM0028", this.calObjs.D1003URLA2020Cal.ClearForeignAddressFields + this.calObjs.VERIFCal.CalcMaintenanceExpenseAmount + this.calObjs.D1003URLA2020Cal.CalcNonSubjectPropertyDebtsToBePaidOffAmount + this.calObjs.D1003URLA2020Cal.CalcBackEndRatio + this.calObjs.VERIFCal.CalcLiabilities + new Routine(this.calObjs.VERIFCal.CalcRealEstate) + this.calObjs.RegzCal.CalculateHELOCPayment + this.calObjs.D1003URLA2020Cal.CalcNonSubjectPropertyDebtsToBePaidOffAmount + this.calObjs.D1003Cal.CalcHousingExp + new Routine(this.calObjs.VERIFCal.CalculateVOM) + this.calObjs.VERIFCal.CalcLiabilities + this.calObjs.D1003URLA2020Cal.CalcTotalMortgageAndTaxes + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.VERIFCal.CalcHelocLineTotal);
      this.AddFieldHandler("L244", this.calObjs.GFECal.CalcGFEFees + new Routine(this.calculateUCD) + this.calObjs.NewHud2015Cal.CalcLoanEstimateTotalAdjustmentsCredit + this.calObjs.NewHud2015Cal.CalcLoanEstimateThirdPartyPaymentsNotOtherwiseDisclosed + this.calObjs.NewHud2015Cal.CalcLoanEstimate_ClosingCostsFinanced + this.calObjs.NewHud2015Cal.CalcLoanEstimateDownPaymentAndFundsForBorrower + this.calObjs.NewHud2015Cal.CalcLoanEstimate_EstimatedCashToClose + this.calObjs.VACal.CalcVAStatutoryRecoupment + this.calObjs.HelocCal.CalcHELOCPeriodicRates);
      this.AddFieldHandler("L245", this.calObjs.GFECal.CalcGFEFees + new Routine(this.calculateUCD) + this.calObjs.NewHud2015Cal.CalcLoanEstimateTotalAdjustmentsCredit + this.calObjs.NewHud2015Cal.CalcLoanEstimateThirdPartyPaymentsNotOtherwiseDisclosed + this.calObjs.NewHud2015Cal.CalcLoanEstimate_ClosingCostsFinanced + this.calObjs.NewHud2015Cal.CalcLoanEstimateDownPaymentAndFundsForBorrower + this.calObjs.NewHud2015Cal.CalcLoanEstimate_EstimatedCashToClose);
      this.AddFieldHandler("L268", this.calObjs.D1003URLA2020Cal.CalcProposedSupplementalPropertyInsurance + this.calObjs.D1003URLA2020Cal.CalcSupplementalInsuranceAmount + this.calObjs.NewHud2015Cal.CalcPropertyInsuranceTaxIndicators + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.VERIFCal.CalcMaintenanceExpenseAmount + new Routine(this.calObjs.VERIFCal.CalculateMultipleVOMs) + this.calObjs.D1003URLA2020Cal.CalcTotalMortgageAndTaxes + this.calObjs.VERIFCal.CalcMaintenanceExpenseAmount + this.calObjs.Hud1Cal.CalcHUD1ES + this.calObjs.D1003URLA2020Cal.CalcTotalMortgageAndTaxes + this.calObjs.VERIFCal.CalcMaintenanceExpenseAmount + this.calObjs.Hud1Cal.CalcHUD1ES + this.calObjs.D1003URLA2020Cal.CalcTotalMortgageAndTaxes + this.calObjs.NewHudCal.CalcREGZGFEHud + this.calObjs.FHACal.CalcMACAWP + this.calObjs.FHACal.CalcFredMac + this.calObjs.GFECal.CalcPrepaid + new Routine(this.calculateUCD) + this.calObjs.Hud1Cal.CalcBiWeeklyEscrowPayment + this.calObjs.NewHud2015Cal.CalcEstimatedPropertyCosts + this.calObjs.ToolCal.CalcCPAEscrowDetails_FCD_AmountFields);
      this.AddFieldHandler("NEWHUD.X354", this.calObjs.GFECal.RecalcForm + this.calObjs.NewHudCal.RecalcForm + this.calObjs.RegzCal.BuildPaySchedule + this.CalcLTV + this.calObjs.RegzCal.CalcAPR);
      this.AddFieldHandler("VARRRWS.X1", this.calObjs.GFECal.CalcGFEFees + new Routine(((CalculationBase) this.calObjs.NewHudCal).FormCal) + this.calObjs.ToolCal.CalcSafeHarbor);
      this.AddFieldHandler("NEWHUD.X1302", this.calObjs.PrequalCal.CalcMIP + this.calObjs.D1003Cal.CalculatePMIIndicator);
      this.AddFieldHandler("NEWHUD.X1707", this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.NewHudCal.CalcREGZGFEHud + this.calObjs.GFECal.CalcPrepaid + this.calObjs.NewHud2015Cal.Calc60Payments + this.calObjs.Hud1Cal.CalcBiWeeklyEscrowPayment + this.calObjs.NewHud2015Cal.CalcEstimatedPropertyCosts + this.calObjs.ToolCal.CalcCPAEscrowDetails_FCD_AmountFields);
      this.AddFieldHandler("NEWHUD.X6", this.calObjs.ATRQMCal.CalcEligibility + this.calObjs.NewHud2015Cal.CalculateProductDescription);
      this.AddFieldHandler("NEWHUD.X8", this.calObjs.NewHud2015Cal.CalcLoanTermTable_MonthlyPrincipalAndInterest + this.calObjs.NewHud2015Cal.CalcAPTable_MaximumPayment);
      this.AddFieldHandler("675", this.calObjs.D1003Cal.PopulateOtherFields + this.calObjs.MLDSCal.CalcMLDSScenarios + this.calObjs.ToolCal.CalcNetTangibleBenefit + this.calObjs.HMDACal.CalcPrepaymentPenaltyPeriod + this.calObjs.GFECal.CalcSection32 + this.calObjs.NewHud2015Cal.CalcLoanTermTable_Prepayment);
      this.AddFieldHandler("1946", this.calObjs.D1003Cal.PopulateOtherFields + this.calObjs.MLDSCal.CalcMLDSScenarios + this.calObjs.HMDACal.CalcPrepaymentPenaltyPeriod);
      this.AddFieldHandler("1947", this.calObjs.D1003Cal.PopulateOtherFields + this.calObjs.MLDSCal.CalcMLDSScenarios + this.calObjs.HMDACal.CalcPrepaymentPenaltyPeriod + this.calObjs.GFECal.CalcSection32 + this.calObjs.ATRQMCal.CalcEligibility + this.calObjs.NewHud2015Cal.CalcLoanTermTable_Prepayment);
      this.AddFieldHandler("1948", this.calObjs.D1003Cal.PopulateOtherFields + this.calObjs.MLDSCal.CalcMLDSScenarios + this.calObjs.HMDACal.CalcPrepaymentPenaltyPeriod + this.calObjs.GFECal.CalcSection32 + this.calObjs.ATRQMCal.CalcPrepaymentPenaltyPercent + this.calObjs.ATRQMCal.CalcEligibility);
      this.AddFieldHandler("1971", this.calObjs.D1003Cal.PopulateOtherFields + this.calObjs.MLDSCal.CalcMLDSScenarios + this.calObjs.HMDACal.CalcPrepaymentPenaltyPeriod);
      this.AddFieldHandler("1972", this.calObjs.D1003Cal.PopulateOtherFields + this.calObjs.MLDSCal.CalcMLDSScenarios + this.calObjs.HMDACal.CalcPrepaymentPenaltyPeriod + this.calObjs.GFECal.CalcSection32 + this.calObjs.ATRQMCal.CalcEligibility + this.calObjs.NewHud2015Cal.CalcLoanTermTable_Prepayment);
      this.AddFieldHandler("1973", this.calObjs.D1003Cal.PopulateOtherFields + this.calObjs.MLDSCal.CalcMLDSScenarios + this.calObjs.HMDACal.CalcPrepaymentPenaltyPeriod + this.calObjs.GFECal.CalcSection32 + this.calObjs.ATRQMCal.CalcPrepaymentPenaltyPercent + this.calObjs.ATRQMCal.CalcEligibility);
      this.AddFieldHandler("1974", this.calObjs.D1003Cal.PopulateOtherFields + this.calObjs.MLDSCal.CalcMLDSScenarios + this.calObjs.HMDACal.CalcPrepaymentPenaltyPeriod);
      this.AddFieldHandler("1975", this.calObjs.D1003Cal.PopulateOtherFields + this.calObjs.MLDSCal.CalcMLDSScenarios + this.calObjs.HMDACal.CalcPrepaymentPenaltyPeriod + this.calObjs.GFECal.CalcSection32 + this.calObjs.ATRQMCal.CalcEligibility + this.calObjs.NewHud2015Cal.CalcLoanTermTable_Prepayment);
      this.AddFieldHandler("1976", this.calObjs.D1003Cal.PopulateOtherFields + this.calObjs.MLDSCal.CalcMLDSScenarios + this.calObjs.HMDACal.CalcPrepaymentPenaltyPeriod + this.calObjs.GFECal.CalcSection32 + this.calObjs.ATRQMCal.CalcPrepaymentPenaltyPercent + this.calObjs.ATRQMCal.CalcEligibility);
      this.AddFieldHandler("1977", this.calObjs.D1003Cal.PopulateOtherFields + this.calObjs.MLDSCal.CalcMLDSScenarios + this.calObjs.HMDACal.CalcPrepaymentPenaltyPeriod);
      this.AddFieldHandler("1978", this.calObjs.D1003Cal.PopulateOtherFields + this.calObjs.MLDSCal.CalcMLDSScenarios + this.calObjs.HMDACal.CalcPrepaymentPenaltyPeriod + this.calObjs.GFECal.CalcSection32 + this.calObjs.ATRQMCal.CalcEligibility + this.calObjs.NewHud2015Cal.CalcLoanTermTable_Prepayment);
      this.AddFieldHandler("1979", this.calObjs.D1003Cal.PopulateOtherFields + this.calObjs.MLDSCal.CalcMLDSScenarios + this.calObjs.HMDACal.CalcPrepaymentPenaltyPeriod + this.calObjs.GFECal.CalcSection32 + this.calObjs.ATRQMCal.CalcPrepaymentPenaltyPercent + this.calObjs.ATRQMCal.CalcEligibility);
      this.AddFieldHandler("1980", this.calObjs.D1003Cal.PopulateOtherFields + this.calObjs.MLDSCal.CalcMLDSScenarios + this.calObjs.HMDACal.CalcPrepaymentPenaltyPeriod);
      this.AddFieldHandler("1981", this.calObjs.D1003Cal.PopulateOtherFields + this.calObjs.MLDSCal.CalcMLDSScenarios + this.calObjs.HMDACal.CalcPrepaymentPenaltyPeriod + this.calObjs.GFECal.CalcSection32 + this.calObjs.ATRQMCal.CalcEligibility + this.calObjs.NewHud2015Cal.CalcLoanTermTable_Prepayment);
      this.AddFieldHandler("1982", this.calObjs.D1003Cal.PopulateOtherFields + this.calObjs.MLDSCal.CalcMLDSScenarios + this.calObjs.HMDACal.CalcPrepaymentPenaltyPeriod + this.calObjs.GFECal.CalcSection32 + this.calObjs.ATRQMCal.CalcPrepaymentPenaltyPercent + this.calObjs.ATRQMCal.CalcEligibility);
      this.AddFieldHandler("2830", this.calObjs.D1003Cal.PopulateOtherFields + this.calObjs.MLDSCal.CalcMLDSScenarios + this.calObjs.HMDACal.CalcPrepaymentPenaltyPeriod + this.calObjs.GFECal.CalcSection32);
      this.AddFieldHandler("2829", this.calObjs.D1003Cal.PopulateOtherFields + this.calObjs.MLDSCal.CalcMLDSScenarios + this.calObjs.HMDACal.CalcPrepaymentPenaltyPeriod);
      this.AddFieldHandler("RE88395.X322", this.calObjs.D1003Cal.PopulateOtherFields + this.calObjs.MLDSCal.CalcMLDSScenarios + this.calObjs.HMDACal.CalcPrepaymentPenaltyPeriod);
      this.AddFieldHandler("RE88395.X191", this.calObjs.D1003Cal.PopulateOtherFields + this.calObjs.MLDSCal.CalcMLDSScenarios + this.calObjs.HMDACal.CalcPrepaymentPenaltyPeriod);
      this.AddFieldHandler("RE88395.X123", this.calObjs.D1003Cal.PopulateOtherFields + this.calObjs.MLDSCal.CalcMLDSScenarios + this.calObjs.HMDACal.CalcPrepaymentPenaltyPeriod);
      this.AddFieldHandler("RE88395.X124", this.calObjs.D1003Cal.PopulateOtherFields + this.calObjs.MLDSCal.CalcMLDSScenarios + this.calObjs.HMDACal.CalcPrepaymentPenaltyPeriod);
      this.AddFieldHandler("RE88395.X315", this.calObjs.MLDSCal.CalcMLDSScenarios + this.calObjs.HMDACal.CalcPrepaymentPenaltyPeriod + this.calObjs.GFECal.CalcSection32 + this.calObjs.ATRQMCal.CalcPrepaymentPenaltyPercent + this.calObjs.ATRQMCal.CalcEligibility + this.calObjs.NewHud2015Cal.CalcLoanTermTable_Prepayment);
      this.AddFieldHandler("DISCLOSURE.X649", this.calObjs.GFECal.CalcOthers + this.calObjs.NewHudCal.CalcClosingCost);
      this.AddFieldHandler("DISCLOSURE.X650", this.calObjs.GFECal.CalcOthers + this.calObjs.NewHudCal.CalcClosingCost);
      this.AddFieldHandler("FR0104", this.calObjs.D1003URLA2020Cal.ConcatenateURLAAddresses + this.calObjs.D1003Cal.CopySameMailingAddress + this.CalcVAAccount + this.calObjs.D1003URLA2020Cal.CalcCopyCurrentToMailingAddress);
      this.AddFieldHandler("FR0106", this.calObjs.D1003Cal.CopySameMailingAddress + this.CalcVAAccount + this.calObjs.D1003URLA2020Cal.CalcCopyCurrentToMailingAddress + this.CalcHomeCounseling);
      this.AddFieldHandler("FR0107", this.calObjs.D1003Cal.CopySameMailingAddress + this.CalcVAAccount + this.calObjs.D1003URLA2020Cal.CalcCopyCurrentToMailingAddress + this.CalcHomeCounseling);
      this.AddFieldHandler("FR0108", this.calObjs.D1003Cal.CopySameMailingAddress + this.CalcVAAccount + this.calObjs.D1003URLA2020Cal.CalcCopyCurrentToMailingAddress + this.CalcHomeCounseling);
      this.AddFieldHandler("FR0130", this.calObjs.D1003Cal.CopySameMailingAddress + this.CalcVAAccount + this.calObjs.D1003URLA2020Cal.CalcCopyCurrentToMailingAddress);
      this.AddFieldHandler("FR0204", this.calObjs.D1003URLA2020Cal.ConcatenateURLAAddresses + this.calObjs.D1003Cal.CopySameMailingAddress + this.CalcVAAccount + this.calObjs.D1003URLA2020Cal.CalcCopyCurrentToMailingAddress);
      this.AddFieldHandler("FR0206", this.calObjs.D1003Cal.CopySameMailingAddress + this.CalcVAAccount + this.calObjs.D1003URLA2020Cal.CalcCopyCurrentToMailingAddress);
      this.AddFieldHandler("FR0207", this.calObjs.D1003Cal.CopySameMailingAddress + this.CalcVAAccount + this.calObjs.D1003URLA2020Cal.CalcCopyCurrentToMailingAddress);
      this.AddFieldHandler("FR0208", this.calObjs.D1003Cal.CopySameMailingAddress + this.CalcVAAccount + this.calObjs.D1003URLA2020Cal.CalcCopyCurrentToMailingAddress);
      this.AddFieldHandler("FR0230", this.calObjs.D1003Cal.CopySameMailingAddress + this.CalcVAAccount + this.calObjs.D1003URLA2020Cal.CalcCopyCurrentToMailingAddress);
      this.AddFieldHandler("FR0125", this.calObjs.D1003URLA2020Cal.ConcatenateURLAAddresses + this.calObjs.D1003URLA2020Cal.CalcCopyCurrentToMailingAddress + this.calObjs.D1003Cal.CopySameMailingAddress);
      this.AddFieldHandler("FR0126", this.calObjs.D1003URLA2020Cal.ConcatenateURLAAddresses + this.calObjs.D1003URLA2020Cal.CalcCopyCurrentToMailingAddress);
      this.AddFieldHandler("FR0127", this.calObjs.D1003URLA2020Cal.ConcatenateURLAAddresses + this.calObjs.D1003URLA2020Cal.CalcCopyCurrentToMailingAddress + this.calObjs.D1003Cal.CopySameMailingAddress);
      this.AddFieldHandler("FR0225", this.calObjs.D1003URLA2020Cal.ConcatenateURLAAddresses + this.calObjs.D1003URLA2020Cal.CalcCopyCurrentToMailingAddress + this.calObjs.D1003Cal.CopySameMailingAddress);
      this.AddFieldHandler("FR0226", this.calObjs.D1003URLA2020Cal.ConcatenateURLAAddresses + this.calObjs.D1003URLA2020Cal.CalcCopyCurrentToMailingAddress);
      this.AddFieldHandler("FR0227", this.calObjs.D1003URLA2020Cal.ConcatenateURLAAddresses + this.calObjs.D1003URLA2020Cal.CalcCopyCurrentToMailingAddress + this.calObjs.D1003Cal.CopySameMailingAddress);
      this.AddFieldHandler("FR0304", this.calObjs.D1003URLA2020Cal.ConcatenateURLAAddresses);
      this.AddFieldHandler("FR0325", this.calObjs.D1003URLA2020Cal.ConcatenateURLAAddresses);
      this.AddFieldHandler("FR0326", this.calObjs.D1003URLA2020Cal.ConcatenateURLAAddresses);
      this.AddFieldHandler("FR0327", this.calObjs.D1003URLA2020Cal.ConcatenateURLAAddresses);
      this.AddFieldHandler("FR0404", this.calObjs.D1003URLA2020Cal.ConcatenateURLAAddresses);
      this.AddFieldHandler("FR0425", this.calObjs.D1003URLA2020Cal.ConcatenateURLAAddresses);
      this.AddFieldHandler("FR0426", this.calObjs.D1003URLA2020Cal.ConcatenateURLAAddresses);
      this.AddFieldHandler("FR0427", this.calObjs.D1003URLA2020Cal.ConcatenateURLAAddresses);
      this.AddFieldHandler("FR0115", this.calObjs.D1003URLA2020Cal.CalcRent + this.calObjs.D1003URLA2020Cal.CalcPresentHousingExpenseRent + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.PrequalCal.CalcRentOwn);
      this.AddFieldHandler("FR0116", this.calObjs.D1003URLA2020Cal.CalcPresentHousingExpenseRent + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.PrequalCal.CalcRentOwn);
      this.AddFieldHandler("FR0215", this.calObjs.D1003URLA2020Cal.CalcRent + this.calObjs.D1003URLA2020Cal.CalcPresentHousingExpenseRent + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.PrequalCal.CalcRentOwn);
      this.AddFieldHandler("FR0216", this.calObjs.D1003URLA2020Cal.CalcPresentHousingExpenseRent + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.PrequalCal.CalcRentOwn);
      this.AddFieldHandler("FR0315", this.calObjs.D1003URLA2020Cal.CalcRent + this.calObjs.D1003URLA2020Cal.CalcPresentHousingExpenseRent + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.PrequalCal.CalcRentOwn);
      this.AddFieldHandler("FR0316", this.calObjs.D1003URLA2020Cal.CalcPresentHousingExpenseRent + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.PrequalCal.CalcRentOwn);
      this.AddFieldHandler("FR0415", this.calObjs.D1003URLA2020Cal.CalcRent + this.calObjs.D1003URLA2020Cal.CalcPresentHousingExpenseRent + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.PrequalCal.CalcRentOwn);
      this.AddFieldHandler("FR0416", this.calObjs.D1003URLA2020Cal.CalcPresentHousingExpenseRent + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.PrequalCal.CalcRentOwn);
      this.AddFieldHandler("BR0004", this.calObjs.D1003URLA2020Cal.ConcatenateURLAAddresses);
      this.AddFieldHandler("BR0025", this.calObjs.D1003URLA2020Cal.ConcatenateURLAAddresses);
      this.AddFieldHandler("BR0026", this.calObjs.D1003URLA2020Cal.ConcatenateURLAAddresses);
      this.AddFieldHandler("BR0027", this.calObjs.D1003URLA2020Cal.ConcatenateURLAAddresses);
      this.AddFieldHandler("CR0004", this.calObjs.D1003URLA2020Cal.ConcatenateURLAAddresses);
      this.AddFieldHandler("CR0025", this.calObjs.D1003URLA2020Cal.ConcatenateURLAAddresses);
      this.AddFieldHandler("CR0026", this.calObjs.D1003URLA2020Cal.ConcatenateURLAAddresses);
      this.AddFieldHandler("CR0027", this.calObjs.D1003URLA2020Cal.ConcatenateURLAAddresses);
      Routine routine7 = this.RoutineX(this.calObjs.D1003URLA2020Cal.CalcPresentHousingExpenseRent + this.calObjs.D1003Cal.CalcHousingExp);
      this.AddFieldHandler("BR0015", routine7);
      this.AddFieldHandler("BR0016", routine7);
      this.AddFieldHandler("BR0023", routine7);
      this.AddFieldHandler("CR0015", routine7);
      this.AddFieldHandler("CR0016", routine7);
      this.AddFieldHandler("CR0023", routine7);
      this.AddFieldHandler("FE0104", this.calObjs.D1003URLA2020Cal.ConcatenateURLAAddresses);
      this.AddFieldHandler("FE0158", this.calObjs.D1003URLA2020Cal.ConcatenateURLAAddresses);
      this.AddFieldHandler("FE0159", this.calObjs.D1003URLA2020Cal.ConcatenateURLAAddresses);
      this.AddFieldHandler("FE0160", this.calObjs.D1003URLA2020Cal.ConcatenateURLAAddresses);
      this.AddFieldHandler("FE0204", this.calObjs.D1003URLA2020Cal.ConcatenateURLAAddresses);
      this.AddFieldHandler("FE0258", this.calObjs.D1003URLA2020Cal.ConcatenateURLAAddresses);
      this.AddFieldHandler("FE0259", this.calObjs.D1003URLA2020Cal.ConcatenateURLAAddresses);
      this.AddFieldHandler("FE0260", this.calObjs.D1003URLA2020Cal.ConcatenateURLAAddresses);
      this.AddFieldHandler("FE0304", this.calObjs.D1003URLA2020Cal.ConcatenateURLAAddresses);
      this.AddFieldHandler("FE0358", this.calObjs.D1003URLA2020Cal.ConcatenateURLAAddresses);
      this.AddFieldHandler("FE0359", this.calObjs.D1003URLA2020Cal.ConcatenateURLAAddresses);
      this.AddFieldHandler("FE0360", this.calObjs.D1003URLA2020Cal.ConcatenateURLAAddresses);
      this.AddFieldHandler("FE0404", this.calObjs.D1003URLA2020Cal.ConcatenateURLAAddresses);
      this.AddFieldHandler("FE0458", this.calObjs.D1003URLA2020Cal.ConcatenateURLAAddresses);
      this.AddFieldHandler("FE0459", this.calObjs.D1003URLA2020Cal.ConcatenateURLAAddresses);
      this.AddFieldHandler("FE0460", this.calObjs.D1003URLA2020Cal.ConcatenateURLAAddresses);
      this.AddFieldHandler("FE0504", this.calObjs.D1003URLA2020Cal.ConcatenateURLAAddresses);
      this.AddFieldHandler("FE0558", this.calObjs.D1003URLA2020Cal.ConcatenateURLAAddresses);
      this.AddFieldHandler("FE0559", this.calObjs.D1003URLA2020Cal.ConcatenateURLAAddresses);
      this.AddFieldHandler("FE0560", this.calObjs.D1003URLA2020Cal.ConcatenateURLAAddresses);
      this.AddFieldHandler("FE0604", this.calObjs.D1003URLA2020Cal.ConcatenateURLAAddresses);
      this.AddFieldHandler("FE0658", this.calObjs.D1003URLA2020Cal.ConcatenateURLAAddresses);
      this.AddFieldHandler("FE0659", this.calObjs.D1003URLA2020Cal.ConcatenateURLAAddresses);
      this.AddFieldHandler("FE0660", this.calObjs.D1003URLA2020Cal.ConcatenateURLAAddresses);
      this.AddFieldHandler("BE0004", this.calObjs.D1003URLA2020Cal.ConcatenateURLAAddresses);
      this.AddFieldHandler("BE0058", this.calObjs.D1003URLA2020Cal.ConcatenateURLAAddresses);
      this.AddFieldHandler("BE0059", this.calObjs.D1003URLA2020Cal.ConcatenateURLAAddresses);
      this.AddFieldHandler("BE0060", this.calObjs.D1003URLA2020Cal.ConcatenateURLAAddresses);
      this.AddFieldHandler("CE0004", this.calObjs.D1003URLA2020Cal.ConcatenateURLAAddresses);
      this.AddFieldHandler("CE0058", this.calObjs.D1003URLA2020Cal.ConcatenateURLAAddresses);
      this.AddFieldHandler("CE0059", this.calObjs.D1003URLA2020Cal.ConcatenateURLAAddresses);
      this.AddFieldHandler("CE0060", this.calObjs.D1003URLA2020Cal.ConcatenateURLAAddresses);
      this.AddFieldHandler("FE0112", this.calObjs.VERIFCal.CalcMonthlyIncome + this.calObjs.VERIFCal.CalcPreviousGrossIncome);
      this.AddFieldHandler("FE0212", this.calObjs.VERIFCal.CalcMonthlyIncome + this.calObjs.VERIFCal.CalcPreviousGrossIncome);
      this.AddFieldHandler("FE0312", this.calObjs.VERIFCal.CalcMonthlyIncome + this.calObjs.VERIFCal.CalcPreviousGrossIncome);
      this.AddFieldHandler("FE0412", this.calObjs.VERIFCal.CalcMonthlyIncome + this.calObjs.VERIFCal.CalcPreviousGrossIncome);
      this.AddFieldHandler("FE0515", this.calObjs.VERIFCal.CalcPreviousGrossIncome);
      this.AddFieldHandler("FE0615", this.calObjs.VERIFCal.CalcPreviousGrossIncome);
      this.AddFieldHandler("BE0012", this.calObjs.VERIFCal.CalcMonthlyIncome + this.calObjs.VERIFCal.CalcPreviousGrossIncome);
      this.AddFieldHandler("CE0012", this.calObjs.VERIFCal.CalcMonthlyIncome + this.calObjs.VERIFCal.CalcPreviousGrossIncome);
      Routine routine8 = this.RoutineX(this.calObjs.D1003URLA2020Cal.CopySelfEmployedToBaseIncome + this.calObjs.VERIFCal.CalcMonthlyIncome + this.calObjs.D1003URLA2020Cal.CalcAggregateIncome + this.calObjs.VERIFCal.CalcPreviousGrossIncome);
      for (int index = 1; index <= 4; ++index)
      {
        this.AddFieldHandler("FE" + index.ToString("00") + "56", routine8);
        this.AddFieldHandler("FE" + index.ToString("00") + "15", routine8);
      }
      this.AddFieldHandler("BE0056", routine8);
      this.AddFieldHandler("CE0056", routine8);
      this.AddFieldHandler("BE0015", routine8);
      this.AddFieldHandler("CE0015", routine8);
      this.AddFieldHandler("BE0009", this.calObjs.D1003URLA2020Cal.CalcAggregateIncome + this.calObjs.D1003URLA2020Cal.CalcVOEDoesNotApply + this.calObjs.VERIFCal.CalcPreviousGrossIncome);
      this.AddFieldHandler("CE0009", this.calObjs.D1003URLA2020Cal.CalcAggregateIncome + this.calObjs.D1003URLA2020Cal.CalcVOEDoesNotApply + this.calObjs.VERIFCal.CalcPreviousGrossIncome);
      this.AddFieldHandler("1416", this.calObjs.D1003URLA2020Cal.ConcatenateURLAAddresses);
      this.AddFieldHandler("URLA.X7", this.calObjs.D1003URLA2020Cal.ConcatenateURLAAddresses);
      this.AddFieldHandler("URLA.X8", this.calObjs.D1003URLA2020Cal.ConcatenateURLAAddresses);
      this.AddFieldHandler("URLA.X197", this.calObjs.D1003URLA2020Cal.ConcatenateURLAAddresses);
      this.AddFieldHandler("1519", this.calObjs.D1003URLA2020Cal.ConcatenateURLAAddresses);
      this.AddFieldHandler("URLA.X9", this.calObjs.D1003URLA2020Cal.ConcatenateURLAAddresses);
      this.AddFieldHandler("URLA.X10", this.calObjs.D1003URLA2020Cal.ConcatenateURLAAddresses);
      this.AddFieldHandler("URLA.X198", this.calObjs.D1003URLA2020Cal.ConcatenateURLAAddresses);
      this.AddFieldHandler("FM0004", this.calObjs.D1003URLA2020Cal.ConcatenateURLAAddresses);
      this.AddFieldHandler("FM0047", this.calObjs.D1003URLA2020Cal.ConcatenateURLAAddresses);
      this.AddFieldHandler("FM0048", this.calObjs.D1003URLA2020Cal.ConcatenateURLAAddresses);
      this.AddFieldHandler("FM0050", this.calObjs.D1003URLA2020Cal.ConcatenateURLAAddresses);
      this.AddFieldHandler("FM0059", new Routine(this.calObjs.VERIFCal.CalculateVOM) + this.calObjs.D1003URLA2020Cal.CalcTotalMortgageAndTaxes + this.calObjs.RegzCal.CalculateHELOCPayment + this.calObjs.D1003Cal.CalcHousingExp);
      this.AddFieldHandler("FM0041", new Routine(this.calObjs.VERIFCal.CalculateVOM) + this.calObjs.D1003URLA2020Cal.CalcTotalMortgageAndTaxes + this.calObjs.RegzCal.CalculateHELOCPayment + this.calObjs.D1003Cal.CalcHousingExp);
      this.AddFieldHandler("FM0046", new Routine(this.calObjs.VERIFCal.CalculateVOM) + this.calObjs.D1003URLA2020Cal.CalcTotalMortgageAndTaxes + this.calObjs.D1003Cal.CalcHousingExp);
      this.AddFieldHandler("FM0016", new Routine(this.calObjs.VERIFCal.CalculateVOM) + this.calObjs.D1003URLA2020Cal.CalcTotalMortgageAndTaxes + this.calObjs.D1003Cal.CalcHousingExp);
      this.AddFieldHandler("FM0021", this.calObjs.VERIFCal.CalcMaintenanceExpenseAmount + new Routine(this.calObjs.VERIFCal.CalculateVOM) + this.calObjs.D1003URLA2020Cal.CalcTotalMortgageAndTaxes + this.calObjs.D1003Cal.CalcHousingExp);
      this.AddFieldHandler("USEGFETAX", this.calObjs.VERIFCal.CalcMaintenanceExpenseAmount + this.calObjs.D1003Cal.CalcHousingExp + new Routine(this.calObjs.VERIFCal.CalculateMultipleVOMs) + this.calObjs.D1003Cal.CalcLoansubAndTSUM);
      this.AddFieldHandler("11", this.calObjs.D1003URLA2020Cal.ConcatenateURLAAddresses + this.calObjs.D1003URLA2020Cal.CalcLockRequestPropertyAddress + this.calObjs.NewHudCal.EnforceGFEAppDateRule + this.calObjs.NewHudCal.CalcInitialDisclosureDueDate + this.calObjs.ToolCal.CalcConsumerHIOrderEligible);
      this.AddFieldHandler("URLA.X73", this.calObjs.D1003Cal.PopulateOtherFields + this.calObjs.D1003URLA2020Cal.ConcatenateURLAAddresses + this.calObjs.D1003URLA2020Cal.CalcLockRequestPropertyAddress + this.calObjs.NewHudCal.CalcGFEApplicationDate + this.calObjs.NewHudCal.EnforceGFEAppDateRule + this.calObjs.NewHudCal.CalcInitialDisclosureDueDate + new Routine(this.populateSubjectPropertyAddress) + this.calObjs.HMDACal.CalcHmdaSyncaddressfields + this.calObjs.ToolCal.CalcConsumerHIOrderEligible);
      this.AddFieldHandler("URLA.X74", this.calObjs.D1003Cal.PopulateOtherFields + this.calObjs.D1003URLA2020Cal.ConcatenateURLAAddresses + this.calObjs.D1003URLA2020Cal.CalcLockRequestPropertyAddress + this.calObjs.NewHudCal.EnforceGFEAppDateRule + this.calObjs.NewHudCal.CalcInitialDisclosureDueDate + new Routine(this.populateSubjectPropertyAddress) + this.calObjs.HMDACal.CalcHmdaSyncaddressfields);
      this.AddFieldHandler("URLA.X75", this.calObjs.D1003Cal.PopulateOtherFields + this.calObjs.D1003URLA2020Cal.ConcatenateURLAAddresses + this.calObjs.D1003URLA2020Cal.CalcLockRequestPropertyAddress + this.calObjs.NewHudCal.EnforceGFEAppDateRule + this.calObjs.NewHudCal.CalcInitialDisclosureDueDate + new Routine(this.populateSubjectPropertyAddress) + this.calObjs.HMDACal.CalcHmdaSyncaddressfields);
      this.AddFieldHandler("HMDA.X113", this.calObjs.D1003Cal.CalcUli + this.calObjs.D1003URLA2020Cal.CalcURLALoanIdentifier + this.calObjs.D1003URLA2020Cal.UpdateOtherRelationship);
      this.AddFieldHandler("URLA.X119", this.calObjs.D1003URLA2020Cal.CalcURLALoanIdentifier);
      this.AddFieldHandler("URLA.X120", this.calObjs.D1003URLA2020Cal.CalcURLALoanIdentifier);
      this.AddFieldHandler("HMDA.X28", this.calObjs.D1003URLA2020Cal.CalcURLALoanIdentifier);
      this.AddFieldHandler("2573", this.calObjs.D1003URLA2020Cal.CalcURLALoanIdentifier);
      this.AddFieldHandler("352", this.calObjs.D1003URLA2020Cal.CalcURLALoanIdentifier);
      this.AddFieldHandler("364", this.calObjs.D1003URLA2020Cal.CalcURLALoanIdentifier);
      this.AddFieldHandler("URLA.X1", this.calObjs.D1003URLA2020Cal.CalcCopyCitizenship);
      this.AddFieldHandler("URLA.X2", this.calObjs.D1003URLA2020Cal.CalcCopyCitizenship);
      this.AddFieldHandler("965", this.calObjs.D1003URLA2020Cal.CalcCopyCitizenship);
      this.AddFieldHandler("985", this.calObjs.D1003URLA2020Cal.CalcCopyCitizenship);
      this.AddFieldHandler("466", this.calObjs.D1003URLA2020Cal.CalcCopyCitizenship);
      this.AddFieldHandler("467", this.calObjs.D1003URLA2020Cal.CalcCopyCitizenship);
      this.AddFieldHandler("1819", this.calObjs.D1003URLA2020Cal.CalcCopyCurrentToMailingAddress);
      this.AddFieldHandler("1820", this.calObjs.D1003URLA2020Cal.CalcCopyCurrentToMailingAddress);
      this.AddFieldHandler("ARM.FlrBasis", this.calObjs.RegzCal.CalcFloorRate + this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + this.calObjs.RegzCal.CalcFirstAdjustmentMinimum);
      this.AddFieldHandler("ARM.ApplyLfCpLow", this.calObjs.RegzCal.CalcFloorRate + this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + this.calObjs.RegzCal.CalcFirstAdjustmentMinimum);
      this.AddFieldHandler("3551", this.calObjs.D1003Cal.CalcHousingExp);
      this.AddFieldHandler("LCP.X6", this.calObjs.ToolCal.CalcLOCompensation + new Routine(((CalculationBase) this.calObjs.NewHudCal).FormCal));
      this.AddFieldHandler("LCP.X7", this.calObjs.ToolCal.CalcLOCompensation + new Routine(((CalculationBase) this.calObjs.NewHudCal).FormCal));
      this.AddFieldHandler("LCP.X9", this.calObjs.ToolCal.CalcLOCompensation + new Routine(((CalculationBase) this.calObjs.NewHudCal).FormCal));
      this.AddFieldHandler("LCP.X10", this.calObjs.ToolCal.CalcLOCompensation + new Routine(((CalculationBase) this.calObjs.NewHudCal).FormCal));
      this.AddFieldHandler("LCP.X11", this.calObjs.ToolCal.CalcLOCompensation + new Routine(((CalculationBase) this.calObjs.NewHudCal).FormCal));
      this.AddFieldHandler("LCP.X12", this.calObjs.ToolCal.CalcLOCompensation + new Routine(((CalculationBase) this.calObjs.NewHudCal).FormCal));
      this.AddFieldHandler("VALA.X30", this.calObjs.D1003URLA2020Cal.CalcBackEndRatio + this.calObjs.VERIFCal.CalcLiabilities + this.calObjs.VACal.CalcVALA);
      this.AddFieldHandler("QM.X371", this.calObjs.GFECal.CalcSection32);
      this.AddFieldHandler("QM.X372", this.calObjs.GFECal.CalcSection32 + this.calObjs.ATRQMCal.CalcDiscountPointPercent);
      this.AddFieldHandler("3293", this.calObjs.GFECal.CalcSection32);
      this.AddFieldHandler("QM.X365", this.calObjs.GFECal.CalcSection32);
      this.AddFieldHandler("QM.X366", this.calObjs.GFECal.CalcSection32);
      this.AddFieldHandler("NEWHUD.X715", this.calObjs.GFECal.CalcSection32);
      this.AddFieldHandler("NEWHUD.X1139", this.calObjs.GFECal.CalcSection32);
      this.AddFieldHandler("NEWHUD.X1150", this.calObjs.GFECal.CalcSection32 + this.calObjs.LoansubCal.CalcLoansub + this.calObjs.VACal.CalcVARRRWS);
      this.AddFieldHandler("NEWHUD.X1151", this.calObjs.GFECal.CalcSection32);
      this.AddFieldHandler("NEWHUD.X1152", this.calObjs.NewHud2015FeeDetailCal.CalcOnlyOneFeeDetail + this.calObjs.GFECal.CalcSection32 + this.calObjs.LoansubCal.CalcLoansub + this.calObjs.VACal.CalcVARRRWS);
      this.AddFieldHandler("NEWHUD.X1067", this.calObjs.GFECal.CalcSection32);
      this.AddFieldHandler("NEWHUD.X1227", this.calObjs.GFECal.CalcSection32 + this.calObjs.LoansubCal.CalcLoansub + this.calObjs.VACal.CalcVARRRWS);
      this.AddFieldHandler("NEWHUD.X1720", this.calObjs.GFECal.CalcSection32);
      this.AddFieldHandler("NEWHUD.X1721", this.calObjs.GFECal.CalcSection32 + this.calObjs.RegzCal.CalcPointsInInitialAdjustedRate);
      this.AddFieldHandler("NEWHUD.X1722", this.calObjs.GFECal.CalcSection32 + this.calObjs.RegzCal.CalcPointsInInitialAdjustedRate);
      this.AddFieldHandler("NEWHUD2.X955", this.calObjs.NewHud2015FeeDetailCal.CalcOnlyOneFeeDetail + this.calObjs.VACal.CalcVARRRWS);
      this.AddFieldHandler("S32DISC.X48", this.calObjs.ATRQMCal.CalcPointsAndFees + this.calObjs.ATRQMCal.CalcEligibility + this.calObjs.HMDACal.CalcTotalPointsAndFees);
      this.AddFieldHandler("VASUMM.X15", this.calObjs.VACal.CalcVARecoupment + this.calObjs.ATRQMCal.CalcEligibility + this.calObjs.VACal.CalcVAStatutoryRecoupment);
      this.AddFieldHandler("VASUMM.X16", this.calObjs.VACal.CalcVARecoupment + this.calObjs.ATRQMCal.CalcEligibility + this.calObjs.VACal.CalcVAStatutoryRecoupment);
      this.AddFieldHandler("VASUMM.X18", this.calObjs.VACal.CalcVARecoupment + this.calObjs.ATRQMCal.CalcEligibility + this.calObjs.VACal.CalcVAStatutoryRecoupment);
      this.AddFieldHandler("VASUMM.X19", this.calObjs.VACal.CalcVARecoupment + this.calObjs.ATRQMCal.CalcEligibility + this.calObjs.VACal.CalcVAStatutoryRecoupment);
      this.AddFieldHandler("VASUMM.X23", this.calObjs.D1003Cal.PopulateOtherFields + this.calObjs.FHACal.CalcExisting23KDebt + this.calObjs.FHACal.CalcFHA203K);
      this.AddFieldHandler("VASUMM.X26", this.calObjs.VACal.CalcVARecoupment + this.calObjs.ATRQMCal.CalcEligibility);
      this.AddFieldHandler("VASUMM.X50", this.calObjs.VACal.CalcVARecoupment + this.calObjs.VACal.CalcVACashOutRefinance + this.calObjs.VACal.CalcVAStatutoryRecoupment);
      this.AddFieldHandler("VASUMM.X100", this.calObjs.VACal.CalcVARecoupment + this.calObjs.VACal.CalcVAStatutoryRecoupment);
      this.AddFieldHandler("VASUMM.X125", this.calObjs.VACal.CalcVARecoupment + this.calObjs.VACal.CalcVAStatutoryRecoupment);
      this.AddFieldHandler("NTB.X1", this.calObjs.VACal.CalcVARecoupment + this.calObjs.VACal.CalcVAStatutoryRecoupment);
      this.AddFieldHandler("NTB.X7", this.calObjs.VACal.CalcVARecoupment + this.calObjs.VACal.CalcVACashOutRefinance + this.calObjs.VACal.CalcVAStatutoryRecoupment);
      this.AddFieldHandler("3969", this.calObjs.NewHud2015FeeDetailCal.ClearTaxStampIndicatorFor1204and1205 + this.calObjs.NewHud2015FeeDetailCal.Update2010ItemizationFrom2015Itemization + this.calObjs.NewHud2015Cal.RefreshFormList + this.calObjs.HMDACal.CalcAllLoanCostsBy2015Indicator + this.calObjs.HMDACal.CalcLenderCredits);
      this.AddFieldHandler("3973", this.calObjs.NewHud2015Cal.SetDisclosureTrackingLogData + this.calObjs.ToolCal.SyncConstructionLinkSyncDefaultData);
      this.AddFieldHandler("3974", this.calObjs.NewHud2015Cal.SetDisclosureTrackingLogData + this.calObjs.ToolCal.SyncConstructionLinkSyncDefaultData);
      this.AddFieldHandler("3975", this.calObjs.NewHud2015Cal.SetDisclosureTrackingLogData + this.calObjs.ToolCal.SyncConstructionLinkSyncDefaultData);
      this.AddFieldHandler("3976", this.calObjs.NewHud2015Cal.SetDisclosureTrackingLogData + this.calObjs.ToolCal.SyncConstructionLinkSyncDefaultData);
      this.AddFieldHandler("3197", this.calObjs.NewHud2015Cal.SetDisclosureTrackingLogData + this.calObjs.ToolCal.SyncConstructionLinkSyncDefaultData);
      this.AddFieldHandler("3152", this.calObjs.NewHud2015FeeDetailCal.Calc_LastDisclosedLE);
      this.AddFieldHandler("3154", this.calObjs.NewHud2015FeeDetailCal.Calc_LastDisclosedLE);
      this.AddFieldHandler("3977", this.calObjs.NewHud2015FeeDetailCal.Calc_LastDisclosedCD);
      this.AddFieldHandler("3979", this.calObjs.NewHud2015FeeDetailCal.Calc_LastDisclosedCD);
      this.AddFieldHandler("3981", this.calObjs.NewHud2015FeeDetailCal.Calc_LastDisclosedCD);
      Routine routine9 = this.RoutineX(new Routine(this.populateSubjectPropertyAddress) + this.calObjs.HMDACal.CalcHmdaSyncaddressfields);
      this.AddFieldHandler("11", routine9);
      this.AddFieldHandler("12", routine9 + this.calObjs.NewHudCal.EnforceGFEAppDateRule + this.calObjs.NewHudCal.CalcInitialDisclosureDueDate + this.CalcHomeCounseling);
      this.AddFieldHandler("13", routine9);
      this.AddFieldHandler("14", routine9);
      this.AddFieldHandler("15", routine9);
      this.AddFieldHandler("HMDA.X91", routine9);
      this.AddFieldHandler("1393", routine9);
      this.AddFieldHandler("L79", this.calObjs.CloserCal.CalClosing + this.calObjs.NewHud2015Cal.CalcLoanEstimate_ClosingCostsFinanced + this.calObjs.NewHud2015Cal.CalcLoanEstimateDownPaymentAndFundsForBorrower + this.calObjs.NewHud2015Cal.CalcCDPage3StdCashToCloseFinal + this.calObjs.NewHud2015Cal.CalcCDPage3TotalK);
      this.AddFieldHandler("L85", this.calObjs.CloserCal.CalClosing + this.calObjs.NewHud2015Cal.CalcCDPage3TotalK);
      this.AddFieldHandler("L89", this.calObjs.CloserCal.CalClosing + this.calObjs.NewHud2015Cal.CalcAdjustmentsLineK05_06_07 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalK + this.calObjs.NewHud2015Cal.CalcAdjustmentsLineKSubtotal);
      this.AddFieldHandler("L94", this.calObjs.CloserCal.CalClosing + this.calObjs.NewHud2015Cal.CalcCDPage3TotalK + this.calObjs.NewHud2015Cal.CalcAdjustmentsLineKSubtotal + this.calObjs.NewHud2015Cal.CalcLoanEstimateEstimatedCashToCloseSV);
      this.AddFieldHandler("L100", this.calObjs.CloserCal.CalClosing + this.calObjs.NewHud2015Cal.CalcCDPage3TotalK + this.calObjs.NewHud2015Cal.CalcAdjustmentsLineKSubtotal + this.calObjs.NewHud2015Cal.CalcLoanEstimateEstimatedCashToCloseSV);
      this.AddFieldHandler("L106", this.calObjs.CloserCal.CalClosing + this.calObjs.NewHud2015Cal.CalcCDPage3TotalK + this.calObjs.NewHud2015Cal.CalcAdjustmentsLineKSubtotal + this.calObjs.NewHud2015Cal.CalcLoanEstimateEstimatedCashToCloseSV);
      this.AddFieldHandler("L111", this.calObjs.CloserCal.CalClosing + this.calObjs.NewHud2015Cal.CalcCDPage3TotalK + this.calObjs.NewHud2015Cal.CalcAdjustmentsLineKSubtotal + this.calObjs.NewHud2015Cal.CalcLoanEstimateEstimatedCashToCloseSV);
      this.AddFieldHandler("L115", this.calObjs.CloserCal.CalClosing + this.calObjs.NewHud2015Cal.CalcCDPage3TotalK + this.calObjs.NewHud2015Cal.CalcAdjustmentsLineKSubtotal + this.calObjs.NewHud2015Cal.CalcLoanEstimateEstimatedCashToCloseSV);
      this.AddFieldHandler("L119", this.calObjs.CloserCal.CalClosing + this.calObjs.NewHud2015Cal.CalcCDPage3TotalK + this.calObjs.NewHud2015Cal.CalcAdjustmentsLineKSubtotal + this.calObjs.NewHud2015Cal.CalcLoanEstimateEstimatedCashToCloseSV);
      this.AddFieldHandler("L123", this.calObjs.CloserCal.CalClosing + this.calObjs.NewHud2015Cal.CalcCDPage3TotalK + this.calObjs.NewHud2015Cal.CalcAdjustmentsLineKSubtotal + this.calObjs.NewHud2015Cal.CalcLoanEstimateEstimatedCashToCloseSV);
      this.AddFieldHandler("L80", this.calObjs.CloserCal.CalClosing + this.calObjs.NewHud2015Cal.CalcCDPage3TotalM);
      this.AddFieldHandler("L82", this.calObjs.CloserCal.CalClosing + this.calObjs.NewHud2015Cal.CalcAdjustmentsLineM03 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalM);
      this.AddFieldHandler("L87", this.calObjs.CloserCal.CalClosing + this.calObjs.NewHud2015Cal.CalcAdjustmentsLineM04 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalM);
      this.AddFieldHandler("L91", this.calObjs.CloserCal.CalClosing + this.calObjs.NewHud2015Cal.CalcAdjustmentsLineM05 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalM);
      this.AddFieldHandler("L97", this.calObjs.CloserCal.CalClosing + this.calObjs.NewHud2015Cal.CalcCDPage3TotalM);
      this.AddFieldHandler("L103", this.calObjs.CloserCal.CalClosing + this.calObjs.NewHud2015Cal.CalcCDPage3TotalM);
      this.AddFieldHandler("L109", this.calObjs.CloserCal.CalClosing + this.calObjs.NewHud2015Cal.CalcCDPage3TotalM);
      this.AddFieldHandler("L113", this.calObjs.CloserCal.CalClosing + this.calObjs.NewHud2015Cal.CalcCDPage3TotalM);
      this.AddFieldHandler("L117", this.calObjs.CloserCal.CalClosing + this.calObjs.NewHud2015Cal.CalcCDPage3TotalM);
      this.AddFieldHandler("L121", this.calObjs.CloserCal.CalClosing + this.calObjs.NewHud2015Cal.CalcCDPage3TotalM);
      this.AddFieldHandler("L125", this.calObjs.CloserCal.CalClosing + this.calObjs.NewHud2015Cal.CalcCDPage3TotalM);
      this.AddFieldHandler("L129", this.calObjs.CloserCal.CalClosing + this.calObjs.NewHud2015Cal.CalcCDPage3TotalN);
      this.AddFieldHandler("L133", this.calObjs.CloserCal.CalClosing + this.calObjs.NewHud2015Cal.CalcCDPage3TotalN);
      this.AddFieldHandler("L136", this.calObjs.CloserCal.CalClosing + this.calObjs.NewHud2015Cal.CalcCDPage3TotalN);
      this.AddFieldHandler("L139", this.calObjs.CloserCal.CalClosing + this.calObjs.NewHud2015Cal.CalcCDPage3TotalN);
      this.AddFieldHandler("L143", this.calObjs.CloserCal.CalClosing + this.calObjs.NewHud2015Cal.CalcCDPage3TotalN);
      this.AddFieldHandler("L147", this.calObjs.CloserCal.CalClosing + this.calObjs.NewHud2015Cal.CalcCDPage3TotalN);
      this.AddFieldHandler("L151", this.calObjs.CloserCal.CalClosing + this.calObjs.NewHud2015Cal.CalcCDPage3TotalN);
      this.AddFieldHandler("L155", this.calObjs.CloserCal.CalClosing + this.calObjs.NewHud2015Cal.CalcCDPage3TotalN);
      this.AddFieldHandler("L161", this.calObjs.CloserCal.CalClosing + this.calObjs.NewHud2015Cal.CalcCDPage3TotalN);
      this.AddFieldHandler("L167", this.calObjs.CloserCal.CalClosing + this.calObjs.NewHud2015Cal.CalcCDPage3TotalN);
      this.AddFieldHandler("L173", this.calObjs.CloserCal.CalClosing + this.calObjs.NewHud2015Cal.CalcCDPage3TotalN);
      this.AddFieldHandler("L177", this.calObjs.CloserCal.CalClosing + this.calObjs.NewHud2015Cal.CalcCDPage3TotalN);
      this.AddFieldHandler("L181", this.calObjs.CloserCal.CalClosing + this.calObjs.NewHud2015Cal.CalcCDPage3TotalN);
      this.AddFieldHandler("L185", this.calObjs.CloserCal.CalClosing + this.calObjs.NewHud2015Cal.CalcCDPage3TotalN);
      this.AddFieldHandler("L128", this.calObjs.D1003URLA2020Cal.CalcTotalOtherAssets + this.calObjs.CloserCal.CalClosing + this.calObjs.NewHud2015Cal.CalcCDPage3TotalL + this.calObjs.NewHud2015Cal.CalcLoanEstimateEstimatedCashToCloseSV + this.calObjs.NewHud2015Cal.CalcLoanEstimateEstimatedCashToCloseAV + this.calObjs.NewHud2015Cal.CalcCDPage3StdCashToCloseFinal + this.calObjs.NewHud2015Cal.CalcLoanEstimate_EstimatedCashToClose + this.calObjs.NewHud2015Cal.CalcClosingDisclosureTotalClosingCosts + this.calObjs.D1003URLA2020Cal.CalcTotalOtherAssets + this.calObjs.D1003Cal.CalcTotalOtherAssets + this.calObjs.D1003URLA2020Cal.CalcOtherCredits + this.calObjs.D1003URLA2020Cal.CalcTotalCredits + this.calObjs.GFECal.CalcClosingCosts);
      this.AddFieldHandler("URLA.X151", this.calObjs.D1003URLA2020Cal.CalcTotalOtherAssets + this.calObjs.D1003Cal.CalcTotalOtherAssets + this.calObjs.D1003URLA2020Cal.CalcOtherCredits + this.calObjs.D1003URLA2020Cal.CalcTotalCredits);
      this.AddFieldHandler("L132", this.calObjs.CloserCal.CalClosing + this.calObjs.NewHud2015Cal.CalcCDPage3TotalL);
      this.AddFieldHandler("L135", this.calObjs.CloserCal.CalClosing + this.calObjs.NewHud2015Cal.CalcCDPage3TotalL);
      this.AddFieldHandler("L158", this.calObjs.CloserCal.CalClosing + this.calObjs.NewHud2015Cal.CalcCDPage3TotalL + this.calObjs.NewHud2015Cal.CalcAdjustmentsLineLSubtotal + this.calObjs.NewHud2015Cal.CalcLoanEstimateEstimatedCashToCloseSV);
      this.AddFieldHandler("L164", this.calObjs.CloserCal.CalClosing + this.calObjs.NewHud2015Cal.CalcCDPage3TotalL + this.calObjs.NewHud2015Cal.CalcAdjustmentsLineLSubtotal + this.calObjs.NewHud2015Cal.CalcLoanEstimateEstimatedCashToCloseSV);
      this.AddFieldHandler("L175", this.calObjs.CloserCal.CalClosing + this.calObjs.NewHud2015Cal.CalcCDPage3TotalL + this.calObjs.NewHud2015Cal.CalcAdjustmentsLineLSubtotal + this.calObjs.NewHud2015Cal.CalcLoanEstimateEstimatedCashToCloseSV);
      this.AddFieldHandler("L179", this.calObjs.CloserCal.CalClosing + this.calObjs.NewHud2015Cal.CalcCDPage3TotalL + this.calObjs.NewHud2015Cal.CalcAdjustmentsLineLSubtotal + this.calObjs.NewHud2015Cal.CalcLoanEstimateEstimatedCashToCloseSV);
      this.AddFieldHandler("L183", this.calObjs.CloserCal.CalClosing + this.calObjs.NewHud2015Cal.CalcCDPage3TotalL + this.calObjs.NewHud2015Cal.CalcAdjustmentsLineLSubtotal + this.calObjs.NewHud2015Cal.CalcLoanEstimateEstimatedCashToCloseSV);
      this.AddFieldHandler("L170", this.calObjs.CloserCal.CalClosing + this.calObjs.NewHud2015Cal.CalcCDPage3TotalL + this.calObjs.NewHud2015Cal.CalcAdjustmentsLineLSubtotal + this.calObjs.NewHud2015Cal.CalcLoanEstimateEstimatedCashToCloseSV);
      this.AddFieldHandler("L726", this.calObjs.NewHud2015Cal.CalcDisclosedSalesPrice + this.calObjs.NewHud2015Cal.CalcLoanEstimate_ClosingCostsFinanced + this.calObjs.NewHud2015Cal.CalcLoanEstimateDownPaymentAndFundsForBorrower + this.calObjs.NewHud2015Cal.CalcCDPage3TotalM + this.calObjs.NewHud2015Cal.CalcCDPage3TotalK + this.calObjs.NewHud2015Cal.CalcCDPage3StdCashToCloseFinal);
      this.AddFieldHandler("CD2.XSTJ", this.calObjs.NewHud2015Cal.CalcClosingDisclosureTotalPayments + this.calObjs.NewHud2015Cal.CalcCDPage3ClosingCosts + this.calObjs.NewHud2015Cal.CalculateLoanEstimate_ComparisonsTable + this.calObjs.NewHud2015Cal.CalcCDPage3AlternateCashToCloseFinal + this.calObjs.NewHud2015Cal.CalcCDPage3StdCashToCloseFinal);
      this.AddFieldHandler("CD2.XSTLC", this.calObjs.NewHud2015Cal.CalcClosingDisclosureTotalClosingCosts + this.calObjs.HMDACal.CalcLenderCredits + this.calObjs.VACal.CalcVARecoupment);
      this.AddFieldHandler("CD2.XSTB", this.calObjs.VACal.CalcVARecoupment + this.calObjs.VACal.CalcVAStatutoryRecoupment);
      this.AddFieldHandler("CD2.XSTC", this.calObjs.VACal.CalcVARecoupment + this.calObjs.VACal.CalcVAStatutoryRecoupment);
      this.AddFieldHandler("CD2.XSTE", this.calObjs.VACal.CalcVARecoupment + this.calObjs.VACal.CalcVAStatutoryRecoupment);
      this.AddFieldHandler("CD2.XSTH", this.calObjs.VACal.CalcVARecoupment + this.calObjs.VACal.CalcVAStatutoryRecoupment);
      this.AddFieldHandler("NEWHUD2.X23", this.calObjs.NewHudCal.CalcClosingCost + this.calObjs.NewHud2015Cal.CalcLoanEstimateEstimatedCashToCloseSV + this.calObjs.NewHud2015Cal.CalcCDPage3TotalL + this.calObjs.NewHud2015Cal.CalcCDPage3TotalN);
      this.AddFieldHandler("CD3.X137", this.calObjs.NewHud2015Cal.CalcCDPage3ClosingCosts + this.calObjs.NewHud2015Cal.CalcCDPage3TotalK);
      this.AddFieldHandler("CD3.X103", this.calObjs.NewHud2015Cal.CalcCDPage3ClosingCosts + this.calObjs.NewHud2015Cal.CalcCDPage3TotalK);
      this.AddFieldHandler("NEWHUD.X337", this.calObjs.NewHud2015Cal.AggrEscrowAcc_EscrowedCosts + this.calObjs.NewHud2015Cal.CalcLoanEstimateTaxesInsuranceInEscrowOrNot + this.calObjs.NewHud2015Cal.CalcWillHaveEscrowAccount);
      this.AddFieldHandler("NEWHUD.X338", this.calObjs.NewHud2015Cal.AggrEscrowAcc_EscrowedCosts + this.calObjs.NewHud2015Cal.CalcLoanEstimateTaxesInsuranceInEscrowOrNot + this.calObjs.NewHud2015Cal.CalcWillHaveEscrowAccount);
      this.AddFieldHandler("NEWHUD.X339", this.calObjs.NewHud2015Cal.AggrEscrowAcc_EscrowedCosts + this.calObjs.NewHud2015Cal.CalcLoanEstimateTaxesInsuranceInEscrowOrNot + this.calObjs.NewHud2015Cal.CalcWillHaveEscrowAccount);
      this.AddFieldHandler("NEWHUD.X340", this.calObjs.NewHud2015Cal.AggrEscrowAcc_EscrowedCosts + this.calObjs.NewHud2015Cal.CalcLoanEstimateTaxesInsuranceInEscrowOrNot + this.calObjs.NewHud2015Cal.CalcPropertyCosts_OtherRow + this.calObjs.NewHud2015Cal.CalcWillHaveEscrowAccount);
      this.AddFieldHandler("NEWHUD.X341", this.calObjs.NewHud2015Cal.AggrEscrowAcc_EscrowedCosts + this.calObjs.NewHud2015Cal.CalcLoanEstimateTaxesInsuranceInEscrowOrNot + this.calObjs.NewHud2015Cal.CalcPropertyCosts_OtherRow + this.calObjs.NewHud2015Cal.CalcWillHaveEscrowAccount);
      this.AddFieldHandler("NEWHUD.X342", this.calObjs.NewHud2015Cal.AggrEscrowAcc_EscrowedCosts + this.calObjs.NewHud2015Cal.CalcLoanEstimateTaxesInsuranceInEscrowOrNot + this.calObjs.NewHud2015Cal.CalcPropertyCosts_OtherRow + this.calObjs.NewHud2015Cal.CalcWillHaveEscrowAccount);
      this.AddFieldHandler("NEWHUD.X343", this.calObjs.NewHud2015Cal.AggrEscrowAcc_EscrowedCosts + this.calObjs.NewHud2015Cal.CalcWillHaveEscrowAccount);
      this.AddFieldHandler("NEWHUD.X1726", this.calObjs.NewHud2015Cal.AggrEscrowAcc_EscrowedCosts + this.calObjs.NewHud2015Cal.CalcLoanEstimateTaxesInsuranceInEscrowOrNot + this.calObjs.NewHud2015Cal.CalcWillHaveEscrowAccount);
      this.AddFieldHandler("NEWHUD.X124", this.calObjs.NewHud2015Cal.AggrEscrowAcc_EscrowedCosts + this.calObjs.NewHud2015Cal.CalcLoanEstimateTaxesInsuranceInEscrowOrNot + this.calObjs.NewHud2015Cal.CalcPropertyCosts_OtherRow + this.calObjs.NewHud2015Cal.CalcEstimatedPropertyCosts + this.calObjs.NewHud2015Cal.CalcWillHaveEscrowAccount);
      this.AddFieldHandler("NEWHUD.X125", this.calObjs.NewHud2015Cal.CalcLoanEstimateTaxesInsuranceInEscrowOrNot + this.calObjs.NewHud2015Cal.CalcPropertyCosts_OtherRow + this.calObjs.NewHud2015Cal.CalcEstimatedPropertyCosts + this.calObjs.NewHud2015Cal.CalcWillHaveEscrowAccount);
      this.AddFieldHandler("NEWHUD.X126", this.calObjs.NewHud2015Cal.CalcLoanEstimateTaxesInsuranceInEscrowOrNot + this.calObjs.NewHud2015Cal.CalcPropertyCosts_OtherRow + this.calObjs.NewHud2015Cal.CalcEstimatedPropertyCosts + this.calObjs.NewHud2015Cal.CalcWillHaveEscrowAccount);
      this.AddFieldHandler("NEWHUD.X127", this.calObjs.NewHud2015Cal.AggrEscrowAcc_EscrowedCosts + this.calObjs.NewHud2015Cal.CalcLoanEstimateTaxesInsuranceInEscrowOrNot + this.calObjs.NewHud2015Cal.CalcPropertyCosts_OtherRow + this.calObjs.NewHud2015Cal.CalcEstimatedPropertyCosts);
      this.AddFieldHandler("NEWHUD.X128", this.calObjs.NewHud2015Cal.CalcLoanEstimateTaxesInsuranceInEscrowOrNot + this.calObjs.NewHud2015Cal.CalcPropertyCosts_OtherRow + this.calObjs.NewHud2015Cal.CalcEstimatedPropertyCosts + this.calObjs.NewHud2015Cal.CalcWillHaveEscrowAccount);
      this.AddFieldHandler("NEWHUD.X129", this.calObjs.NewHud2015Cal.CalcLoanEstimateTaxesInsuranceInEscrowOrNot + this.calObjs.NewHud2015Cal.CalcPropertyCosts_OtherRow + this.calObjs.NewHud2015Cal.CalcEstimatedPropertyCosts + this.calObjs.NewHud2015Cal.CalcWillHaveEscrowAccount);
      this.AddFieldHandler("NEWHUD.X130", this.calObjs.NewHud2015Cal.AggrEscrowAcc_EscrowedCosts + this.calObjs.NewHud2015Cal.CalcLoanEstimateTaxesInsuranceInEscrowOrNot + this.calObjs.NewHud2015Cal.CalcPropertyCosts_OtherRow + this.calObjs.NewHud2015Cal.CalcEstimatedPropertyCosts + this.calObjs.NewHud2015Cal.CalcWillHaveEscrowAccount);
      this.AddFieldHandler("NEWHUD.X131", this.calObjs.NewHud2015Cal.CalcLoanEstimateTaxesInsuranceInEscrowOrNot + this.calObjs.NewHud2015Cal.CalcPropertyCosts_OtherRow + this.calObjs.NewHud2015Cal.CalcEstimatedPropertyCosts + this.calObjs.NewHud2015Cal.CalcWillHaveEscrowAccount);
      this.AddFieldHandler("NEWHUD.X332", this.calObjs.NewHud2015Cal.CalcLoanEstimateTaxesInsuranceInEscrowOrNot + this.calObjs.NewHud2015Cal.CalcPropertyCosts_OtherRow + this.calObjs.NewHud2015Cal.CalcEstimatedPropertyCosts + this.calObjs.NewHud2015Cal.CalcWillHaveEscrowAccount);
      this.AddFieldHandler("NEWHUD2.X4397", this.calObjs.NewHud2015Cal.Calc2015LineItem904 + this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem904 + this.calObjs.NewHud2015Cal.CalcClosingCostSubTotal);
      this.AddFieldHandler("NEWHUD.X591", this.calObjs.NewHud2015Cal.Calc2015LineItem904 + this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem904 + this.calObjs.NewHud2015Cal.CalcClosingCostSubTotal);
      this.AddFieldHandler("NEWHUD.X594", this.calObjs.NewHud2015Cal.Calc2015LineItem904 + this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem904 + this.calObjs.NewHud2015Cal.CalcClosingCostSubTotal);
      this.AddFieldHandler("NEWHUD2.X4399", this.calObjs.NewHud2015Cal.Calc2015LineItem906 + this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem906 + this.calObjs.NewHud2015Cal.CalcClosingCostSubTotal);
      this.AddFieldHandler("643", this.calObjs.NewHud2015Cal.Calc2015LineItem906 + this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem906 + this.calObjs.GFECal.CalcPrepaid + this.calObjs.NewHud2015Cal.CalcClosingCostSubTotal);
      this.AddFieldHandler("NEWHUD2.X4400", this.calObjs.NewHud2015Cal.Calc2015LineItem906 + this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem906 + this.calObjs.NewHud2015Cal.CalcClosingCostSubTotal);
      this.AddFieldHandler("579", this.calObjs.NewHud2015Cal.Calc2015LineItem906 + this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem906 + this.calObjs.NewHud2015Cal.CalcClosingCostSubTotal);
      this.AddFieldHandler("NEWHUD2.X4401", this.calObjs.NewHud2015Cal.Calc2015LineItem907 + this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem907 + this.calObjs.NewHud2015Cal.CalcClosingCostSubTotal);
      this.AddFieldHandler("L260", this.calObjs.NewHud2015Cal.Calc2015LineItem907 + this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem907 + this.calObjs.GFECal.CalcPrepaid + this.calObjs.NewHud2015Cal.CalcClosingCostSubTotal);
      this.AddFieldHandler("NEWHUD2.X4402", this.calObjs.NewHud2015Cal.Calc2015LineItem907 + this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem907 + this.calObjs.NewHud2015Cal.CalcClosingCostSubTotal);
      this.AddFieldHandler("L261", this.calObjs.NewHud2015Cal.Calc2015LineItem907 + this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem907 + this.calObjs.NewHud2015Cal.CalcClosingCostSubTotal);
      this.AddFieldHandler("NEWHUD2.X4403", this.calObjs.NewHud2015Cal.Calc2015LineItem908 + this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem908 + this.calObjs.NewHud2015Cal.CalcClosingCostSubTotal);
      this.AddFieldHandler("NEWHUD2.X4404", this.calObjs.NewHud2015Cal.Calc2015LineItem908 + this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem908 + this.calObjs.NewHud2015Cal.CalcClosingCostSubTotal);
      this.AddFieldHandler("1667", this.calObjs.NewHud2015Cal.Calc2015LineItem908 + this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem908 + this.calObjs.GFECal.CalcPrepaid + this.calObjs.NewHud2015Cal.CalcClosingCostSubTotal);
      this.AddFieldHandler("1668", this.calObjs.NewHud2015Cal.Calc2015LineItem908 + this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem908 + this.calObjs.NewHud2015Cal.CalcClosingCostSubTotal);
      this.AddFieldHandler("NEWHUD2.X4405", this.calObjs.NewHud2015Cal.Calc2015LineItem909 + this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem909 + this.calObjs.NewHud2015Cal.CalcClosingCostSubTotal);
      this.AddFieldHandler("NEWHUD.X592", this.calObjs.NewHud2015Cal.Calc2015LineItem909 + this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem909 + this.calObjs.GFECal.CalcPrepaid + this.calObjs.NewHud2015Cal.CalcClosingCostSubTotal);
      this.AddFieldHandler("NEWHUD2.X4406", this.calObjs.NewHud2015Cal.Calc2015LineItem909 + this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem909 + this.calObjs.NewHud2015Cal.CalcClosingCostSubTotal);
      this.AddFieldHandler("NEWHUD.X595", this.calObjs.NewHud2015Cal.Calc2015LineItem909 + this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem909 + this.calObjs.NewHud2015Cal.CalcClosingCostSubTotal);
      this.AddFieldHandler("NEWHUD2.X4407", this.calObjs.NewHud2015Cal.Calc2015LineItem910 + this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem910 + this.calObjs.NewHud2015Cal.CalcClosingCostSubTotal);
      this.AddFieldHandler("NEWHUD.X593", this.calObjs.NewHud2015Cal.Calc2015LineItem910 + this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem910 + this.calObjs.GFECal.CalcPrepaid + this.calObjs.NewHud2015Cal.CalcClosingCostSubTotal);
      this.AddFieldHandler("NEWHUD2.X4408", this.calObjs.NewHud2015Cal.Calc2015LineItem910 + this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem910 + this.calObjs.NewHud2015Cal.CalcClosingCostSubTotal);
      this.AddFieldHandler("NEWHUD.X596", this.calObjs.NewHud2015Cal.Calc2015LineItem910 + this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem910 + this.calObjs.NewHud2015Cal.CalcClosingCostSubTotal);
      this.AddFieldHandler("NEWHUD2.X4409", this.calObjs.NewHud2015Cal.Calc2015LineItem911 + this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem911 + this.calObjs.NewHud2015Cal.CalcClosingCostSubTotal);
      this.AddFieldHandler("NEWHUD.X1588", this.calObjs.NewHud2015Cal.Calc2015LineItem911 + this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem911 + this.calObjs.GFECal.CalcPrepaid + this.calObjs.NewHud2015Cal.CalcClosingCostSubTotal);
      this.AddFieldHandler("NEWHUD2.X4410", this.calObjs.NewHud2015Cal.Calc2015LineItem911 + this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem911 + this.calObjs.NewHud2015Cal.CalcClosingCostSubTotal);
      this.AddFieldHandler("NEWHUD.X1589", this.calObjs.NewHud2015Cal.Calc2015LineItem911 + this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem911 + this.calObjs.NewHud2015Cal.CalcClosingCostSubTotal);
      this.AddFieldHandler("NEWHUD2.X4411", this.calObjs.NewHud2015Cal.Calc2015LineItem912 + this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem912 + this.calObjs.NewHud2015Cal.CalcClosingCostSubTotal);
      this.AddFieldHandler("NEWHUD.X1596", this.calObjs.NewHud2015Cal.Calc2015LineItem912 + this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem912 + this.calObjs.GFECal.CalcPrepaid + this.calObjs.NewHud2015Cal.CalcClosingCostSubTotal);
      this.AddFieldHandler("NEWHUD2.X4412", this.calObjs.NewHud2015Cal.Calc2015LineItem912 + this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem912 + this.calObjs.NewHud2015Cal.CalcClosingCostSubTotal);
      this.AddFieldHandler("NEWHUD.X1597", this.calObjs.NewHud2015Cal.Calc2015LineItem912 + this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem912 + this.calObjs.NewHud2015Cal.CalcClosingCostSubTotal);
      this.AddFieldHandler("CD2.XBCCBC", this.calObjs.NewHud2015Cal.CalcClosingDisclosureTotalClosingCosts + this.calObjs.NewHud2015Cal.CalcCDPage3StdCashToCloseFinal + this.calObjs.NewHud2015Cal.CalcCDPage3AlternateCashToCloseFinal);
      this.AddFieldHandler("NEWHUD.X1585", this.calObjs.NewHud2015Cal.CalcLoanEstimate_ClosingCostsFinanced + this.calObjs.NewHud2015Cal.CalcCDPage3StdCashToCloseFinal + this.calObjs.USDACal.CalcLoanFundsUsage);
      this.AddFieldHandler("NEWHUD2.X1", this.calObjs.NewHud2015Cal.CalcLineItem701 + this.calObjs.NewHud2015Cal.CalcSection700);
      this.AddFieldHandler("NEWHUD2.X2", this.calObjs.NewHud2015Cal.CalcLineItem701 + this.calObjs.NewHud2015Cal.CalcSection700);
      this.AddFieldHandler("NEWHUD2.X3", this.calObjs.NewHud2015Cal.CalcLineItem702 + this.calObjs.NewHud2015Cal.CalcSection700);
      this.AddFieldHandler("NEWHUD2.X4", this.calObjs.NewHud2015Cal.CalcLineItem702 + this.calObjs.NewHud2015Cal.CalcSection700);
      this.AddFieldHandler("CD3.X84", this.calObjs.NewHud2015Cal.CalcCDPage3StdCashToCloseFinal + this.calObjs.NewHud2015Cal.CalcCDPage3AlternateCashToCloseFinal);
      this.AddFieldHandler("CD3.X104", this.calObjs.NewHud2015Cal.CalcCDPage3StdCashToCloseLoanEstimate + this.calObjs.NewHud2015Cal.CalcCDPage3StdCashToCloseFinal);
      this.AddFieldHandler("CD3.X109", this.calObjs.NewHud2015Cal.CalcCDPage3StdDidChangeCol + this.calObjs.NewHud2015Cal.CalcCDPage3StdCashToCloseFinal);
      this.AddFieldHandler("NEWHUD2.X4769", this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.Hud1Cal.CalcHUD1ES + this.calObjs.Hud1Cal.CalcBiWeeklyEscrowPayment + this.calObjs.NewHud2015Cal.CalculateLoanEstimate_ComparisonsTable + this.calObjs.NewHud2015Cal.CalcEstimatedPropertyCosts);
      this.AddFieldHandler("NEWHUD2.X133", this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.Hud1Cal.CalcHUD1ES + this.calObjs.Hud1Cal.CalcBiWeeklyEscrowPayment + this.calObjs.NewHud2015Cal.CalculateLoanEstimate_ComparisonsTable + this.calObjs.NewHud2015Cal.CalcEstimatedPropertyCosts);
      this.AddFieldHandler("NEWHUD2.X134", this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.Hud1Cal.CalcHUD1ES + this.calObjs.Hud1Cal.CalcBiWeeklyEscrowPayment + this.calObjs.NewHud2015Cal.CalculateLoanEstimate_ComparisonsTable + this.calObjs.NewHud2015Cal.CalcEstimatedPropertyCosts);
      this.AddFieldHandler("NEWHUD2.X135", this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.Hud1Cal.CalcHUD1ES + this.calObjs.Hud1Cal.CalcBiWeeklyEscrowPayment + this.calObjs.NewHud2015Cal.CalculateLoanEstimate_ComparisonsTable + this.calObjs.NewHud2015Cal.CalcEstimatedPropertyCosts);
      this.AddFieldHandler("NEWHUD2.X136", this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.Hud1Cal.CalcHUD1ES + this.calObjs.Hud1Cal.CalcBiWeeklyEscrowPayment + this.calObjs.NewHud2015Cal.CalculateLoanEstimate_ComparisonsTable + this.calObjs.NewHud2015Cal.CalcEstimatedPropertyCosts);
      this.AddFieldHandler("NEWHUD2.X137", this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.Hud1Cal.CalcHUD1ES + this.calObjs.Hud1Cal.CalcBiWeeklyEscrowPayment + this.calObjs.NewHud2015Cal.CalculateLoanEstimate_ComparisonsTable + this.calObjs.NewHud2015Cal.CalcEstimatedPropertyCosts);
      this.AddFieldHandler("NEWHUD2.X138", this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.Hud1Cal.CalcHUD1ES + this.calObjs.Hud1Cal.CalcBiWeeklyEscrowPayment + this.calObjs.NewHud2015Cal.CalculateLoanEstimate_ComparisonsTable + this.calObjs.NewHud2015Cal.CalcEstimatedPropertyCosts);
      this.AddFieldHandler("NEWHUD2.X139", this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.Hud1Cal.CalcHUD1ES + this.calObjs.Hud1Cal.CalcBiWeeklyEscrowPayment + this.calObjs.NewHud2015Cal.CalculateLoanEstimate_ComparisonsTable + this.calObjs.NewHud2015Cal.CalcEstimatedPropertyCosts);
      this.AddFieldHandler("NEWHUD2.X140", this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.Hud1Cal.CalcHUD1ES + this.calObjs.Hud1Cal.CalcBiWeeklyEscrowPayment + this.calObjs.NewHud2015Cal.CalculateLoanEstimate_ComparisonsTable + this.calObjs.NewHud2015Cal.CalcEstimatedPropertyCosts);
      this.AddFieldHandler("3200", this.calObjs.NewHudCal.CalcGFERedisclosureFlag + this.calObjs.NewHud2015Cal.CalcLERedisclosureFlag + this.calObjs.NewHud2015Cal.CalcCDRedisclosureFlag);
      this.AddFieldHandler("NEWHUD2.X4662", this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem2001 + this.calObjs.NewHud2015Cal.CalcSubsequentlyPaidFinanceCharge);
      this.AddFieldHandler("NEWHUD2.X4670", this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem2001 + this.calObjs.NewHud2015Cal.CalcSubsequentlyPaidFinanceCharge);
      this.AddFieldHandler("NEWHUD2.X4672", this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem2001 + this.calObjs.NewHud2015Cal.CalcSubsequentlyPaidFinanceCharge);
      this.AddFieldHandler("NEWHUD2.X4673", this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem2001 + this.calObjs.NewHud2015Cal.CalcSubsequentlyPaidFinanceCharge);
      this.AddFieldHandler("NEWHUD2.X4674", this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem2001 + this.calObjs.NewHud2015Cal.CalcSubsequentlyPaidFinanceCharge);
      this.AddFieldHandler("NEWHUD2.X4752", this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem2001 + this.calObjs.NewHud2015Cal.CalcSubsequentlyPaidFinanceCharge);
      this.AddFieldHandler("NEWHUD2.X4685", this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem2002 + this.calObjs.NewHud2015Cal.CalcSubsequentlyPaidFinanceCharge);
      this.AddFieldHandler("NEWHUD2.X4693", this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem2002 + this.calObjs.NewHud2015Cal.CalcSubsequentlyPaidFinanceCharge);
      this.AddFieldHandler("NEWHUD2.X4695", this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem2002 + this.calObjs.NewHud2015Cal.CalcSubsequentlyPaidFinanceCharge);
      this.AddFieldHandler("NEWHUD2.X4696", this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem2002 + this.calObjs.NewHud2015Cal.CalcSubsequentlyPaidFinanceCharge);
      this.AddFieldHandler("NEWHUD2.X4697", this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem2002 + this.calObjs.NewHud2015Cal.CalcSubsequentlyPaidFinanceCharge);
      this.AddFieldHandler("NEWHUD2.X4753", this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem2002 + this.calObjs.NewHud2015Cal.CalcSubsequentlyPaidFinanceCharge);
      this.AddFieldHandler("NEWHUD2.X4708", this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem2003 + this.calObjs.NewHud2015Cal.CalcSubsequentlyPaidFinanceCharge);
      this.AddFieldHandler("NEWHUD2.X4716", this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem2003 + this.calObjs.NewHud2015Cal.CalcSubsequentlyPaidFinanceCharge);
      this.AddFieldHandler("NEWHUD2.X4718", this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem2003 + this.calObjs.NewHud2015Cal.CalcSubsequentlyPaidFinanceCharge);
      this.AddFieldHandler("NEWHUD2.X4719", this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem2003 + this.calObjs.NewHud2015Cal.CalcSubsequentlyPaidFinanceCharge);
      this.AddFieldHandler("NEWHUD2.X4720", this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem2003 + this.calObjs.NewHud2015Cal.CalcSubsequentlyPaidFinanceCharge);
      this.AddFieldHandler("NEWHUD2.X4754", this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem2003 + this.calObjs.NewHud2015Cal.CalcSubsequentlyPaidFinanceCharge);
      this.AddFieldHandler("NEWHUD2.X4731", this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem2004 + this.calObjs.NewHud2015Cal.CalcSubsequentlyPaidFinanceCharge);
      this.AddFieldHandler("NEWHUD2.X4739", this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem2004 + this.calObjs.NewHud2015Cal.CalcSubsequentlyPaidFinanceCharge);
      this.AddFieldHandler("NEWHUD2.X4741", this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem2004 + this.calObjs.NewHud2015Cal.CalcSubsequentlyPaidFinanceCharge);
      this.AddFieldHandler("NEWHUD2.X4742", this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem2004 + this.calObjs.NewHud2015Cal.CalcSubsequentlyPaidFinanceCharge);
      this.AddFieldHandler("NEWHUD2.X4743", this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem2004 + this.calObjs.NewHud2015Cal.CalcSubsequentlyPaidFinanceCharge);
      this.AddFieldHandler("NEWHUD2.X4755", this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem2004 + this.calObjs.NewHud2015Cal.CalcSubsequentlyPaidFinanceCharge);
      this.AddFieldHandler("NEWHUD2.X4768", this.calObjs.NewHudCal.CalcClosingCost + this.calObjs.NewHud2015Cal.CalcSubsequentlyPaidFinanceCharge + this.calObjs.RegzCal.CalcAPR);
      this.AddFieldHandler("4088", this.calObjs.RegzCal.CalcAPR);
      this.AddFieldHandler("CASASRN.X141", this.calObjs.RegzCal.CleanBuydownFields + this.calObjs.RegzCal.CalcBuydownIndicator + this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.RegzCal.CalcAPR + this.calObjs.GFECal.CalcSection32 + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.D1003Cal.CopyBuyDownToLockRequest + this.calObjs.NewHudCal.CalcREGZGFEHud);
      this.AddFieldHandler("1269", this.calObjs.RegzCal.CalcBuydownIndicator + this.calObjs.D1003Cal.CopyBuyDownToLockRequest);
      this.AddFieldHandler("LE2.X1", this.calObjs.NewHud2015Cal.CalcLoanEstimateDownPaymentAndFundsForBorrower + this.calObjs.NewHud2015Cal.CalcLoanEstimateEstimatedCashToCloseSV + this.calObjs.NewHud2015Cal.CalcLoanEstimate_EstimatedCashToClose);
      this.AddFieldHandler("LE2.X4", this.calObjs.NewHud2015Cal.CalcCDPage3StdCashToCloseFinal + this.calObjs.NewHud2015Cal.CalcCDPage3AlternateCashToCloseFinal + this.calObjs.NewHud2015Cal.CalcLoanEstimateEstimatedCashToCloseSV + this.calObjs.NewHud2015Cal.CalcLoanEstimate_EstimatedCashToClose);
      this.AddFieldHandler("LE2.X28", this.calObjs.NewHud2015Cal.CalcLoanEstimate_ClosingCostsFinanced + this.calObjs.NewHud2015Cal.CalcCDPage3StdCashToCloseFinal + this.calObjs.NewHud2015Cal.CalcCDPage3AlternateCashToCloseFinal + this.calObjs.NewHud2015Cal.CalcLoanEstimateEstimatedCashToCloseSV + this.calObjs.NewHud2015Cal.CalcLoanEstimate_EstimatedCashToClose + this.calObjs.NewHud2015Cal.CalcLoanEstimate_IncludePayoffs + this.calObjs.NewHud2015Cal.CalcLoanEstimateThirdPartyPaymentsNotOtherwiseDisclosed + this.calObjs.NewHud2015Cal.CalcCDPage3TotalPurchasePayoffsIncluded + this.calObjs.NewHud2015Cal.CalcLoanEstimateDownPaymentAndFundsForBorrower + this.calObjs.NewHud2015Cal.CalcAdjustmentsUcdTotal);
      this.AddFieldHandler("LE2.X29", this.calObjs.NewHud2015Cal.CalcLoanEstimate_ClosingCostsFinanced + this.calObjs.NewHud2015Cal.CalcCDPage3StdCashToCloseFinal + this.calObjs.NewHud2015Cal.CalcCDPage3AlternateCashToCloseFinal + this.calObjs.NewHud2015Cal.CalcLoanEstimateDownPaymentAndFundsForBorrower + this.calObjs.NewHud2015Cal.CalcLoanEstimateEstimatedCashToCloseSV + this.calObjs.NewHud2015Cal.CalcLoanEstimate_EstimatedCashToClose);
      this.AddFieldHandler("LE2.X30", this.calObjs.NewHud2015Cal.CalcLoanEstimate_ClosingCostsFinanced + this.calObjs.NewHud2015Cal.CalcCDPage3StdCashToCloseFinal + this.calObjs.NewHud2015Cal.CalcCDPage3AlternateCashToCloseFinal + this.calObjs.NewHud2015Cal.CalcLoanEstimateDownPaymentAndFundsForBorrower + this.calObjs.NewHud2015Cal.CalcLoanEstimateEstimatedCashToCloseSV + this.calObjs.NewHud2015Cal.CalcLoanEstimate_EstimatedCashToClose);
      this.AddFieldHandler("LE2.x31", this.calObjs.GFECal.CalcGFEFees + this.calObjs.GFECal.CalcPrepaid + this.calObjs.NewHudCal.CalcREGZGFE + this.calObjs.NewHudCal.CalcHUD1PG2 + this.calObjs.NewHud2015Cal.PopulateLoanEstimate_LoanPurpose + this.calObjs.NewHud2015Cal.CalcLoanEstimateEstimatedCashToCloseAV + this.calObjs.NewHud2015Cal.CalcLoanEstimateThirdPartyPaymentsNotOtherwiseDisclosed + this.calObjs.NewHud2015Cal.CalcLoanEstimate_ClosingCostsFinanced + this.calObjs.NewHud2015Cal.CalcLoanEstimateDownPaymentAndFundsForBorrower + this.calObjs.NewHud2015Cal.CalcCDPage3StdCashToCloseFinal);
      this.AddFieldHandler("LE2.X100", this.calObjs.NewHud2015Cal.CalcCDPage3StdCashToCloseFinal + this.calObjs.NewHud2015Cal.CalcCDPage3AlternateCashToCloseFinal + this.calObjs.NewHud2015Cal.CalcLoanEstimateEstimatedCashToCloseSV + this.calObjs.NewHud2015Cal.CalcLoanEstimate_EstimatedCashToClose + this.calObjs.NewHud2015Cal.CalcLECashToCloseSellerCredit);
      this.AddFieldHandler("LE2.X101", this.calObjs.NewHud2015Cal.CalcLoanEstimateThirdPartyPaymentsNotOtherwiseDisclosed + this.calObjs.NewHud2015Cal.CalcCDPage3TotalPurchasePayoffsIncluded + this.calObjs.NewHud2015Cal.CalcAdjustmentsUcdTotal + this.calObjs.NewHud2015Cal.CalcTotalAdjustmentAndOtherCredits + this.calObjs.NewHud2015Cal.CalcLoanEstimateDownPaymentAndFundsForBorrower);
      this.AddFieldHandler("CD3.X1543", this.calObjs.NewHud2015Cal.CalcAdjustmentsUcdTotal + this.calObjs.NewHud2015Cal.CalcTotalAdjustmentAndOtherCredits + this.calObjs.NewHud2015Cal.CalcLoanEstimateDownPaymentAndFundsForBorrower);
      this.AddFieldHandler("677", this.calObjs.NewHud2015Cal.CalcLE3OtherConsiderationAssumptions);
      this.AddFieldHandler("220", this.calObjs.FHACal.CalcMACAWP + this.calObjs.NewHud2015Cal.CalcAdjustmentsNonUcdTotal);
      this.AddFieldHandler("MORNET.X40", this.calObjs.FHACal.CalcExisting23KDebt + this.calObjs.FHACal.CalcMAX23K + this.calObjs.FHACal.CalcCAWRefi + this.calObjs.FHACal.CalcEEMWorksheet + this.calObjs.NewHudCal.EnforceGFEAppDateRule + this.calObjs.NewHudCal.CalcInitialDisclosureDueDate);
      this.AddFieldHandler("LOANTERMTABLE.CUSTOMIZE", new Routine(this.calObjs.RegzCal.CalcPaymentSchedule) + this.calObjs.NewHudCal.CalcREGZGFEHudSummary);
      this.AddFieldHandler("948", this.calObjs.ATRQMCal.CalcPointsAndFees + new Routine(this.calObjs.RegzCal.CalcPaymentSchedule));
      this.AddFieldHandler("1206", new Routine(this.calObjs.RegzCal.CalcPaymentSchedule));
      this.AddFieldHandler("FV.X366", this.calObjs.NewHud2015Cal.SetAppliedCureAmount + this.calObjs.NewHud2015Cal.CalcCDPage3AlternateCashToCloseFinal);
      this.AddFieldHandler("FV.X396", this.calObjs.NewHud2015Cal.CalcToleranceCureAppliedCureAmount + this.calObjs.NewHud2015Cal.SetAppliedCureAmount + this.calObjs.NewHud2015Cal.CalcCDPage3AlternateCashToCloseFinal);
      this.AddFieldHandler("FV.X397", this.calObjs.NewHud2015Cal.CalcToleranceCureAppliedCureAmount + this.calObjs.NewHud2015Cal.SetAppliedCureAmount + this.calObjs.NewHud2015Cal.CalcCDPage3AlternateCashToCloseFinal);
      this.AddFieldHandler("NMLS.X11", this.calObjs.PrequalCal.CalcMIP + this.calObjs.HMDACal.CalcLoanAmount);
      this.AddFieldHandler("NMLS.X12", this.calObjs.PrequalCal.CalcMIP);
      this.AddFieldHandler("QM.X23", this.calObjs.ATRQMCal.CalcEligibility + this.calObjs.ATRQMCal.PopulateQMStatusInNMLSForm);
      this.AddFieldHandler("QM.X24", this.calObjs.ATRQMCal.CalcEligibility + this.calObjs.ATRQMCal.PopulateQMStatusInNMLSForm);
      this.AddFieldHandler("QM.X25", this.calObjs.ATRQMCal.CalcEligibility + this.calObjs.ATRQMCal.PopulateQMStatusInNMLSForm);
      this.AddFieldHandler("QM.X61", this.calObjs.ATRQMCal.CalcEligibility + this.calObjs.ATRQMCal.PopulateQMStatusInNMLSForm);
      this.AddFieldHandler("QM.X83", this.calObjs.ATRQMCal.CalcEligibility + this.calObjs.ATRQMCal.PopulateQMStatusInNMLSForm);
      this.AddFieldHandler("QM.X102", this.calObjs.ATRQMCal.CalcEligibility + this.calObjs.ATRQMCal.PopulateQMStatusInNMLSForm);
      this.AddFieldHandler("QM.X103", this.calObjs.ATRQMCal.CalcEligibility + this.calObjs.ATRQMCal.PopulateQMStatusInNMLSForm + this.calObjs.HMDACal.CalcBusinessOrCommercialPurpose + this.calObjs.HMDACal.CalcHmdaCDRequired + this.calObjs.HMDACal.CalcAllLoanCostsBy2015Indicator + this.calObjs.HMDACal.CalcHOEPAStatus);
      this.AddFieldHandler("QM.X104", this.calObjs.ATRQMCal.CalcEligibility + this.calObjs.ATRQMCal.PopulateQMStatusInNMLSForm);
      this.AddFieldHandler("QM.X106", this.calObjs.ATRQMCal.CalcEligibility + this.calObjs.ATRQMCal.PopulateQMStatusInNMLSForm);
      this.AddFieldHandler("QM.X108", this.calObjs.ATRQMCal.CalcEligibility + this.calObjs.ATRQMCal.PopulateQMStatusInNMLSForm);
      this.AddFieldHandler("QM.X110", this.calObjs.ATRQMCal.CalcEligibility + this.calObjs.ATRQMCal.PopulateQMStatusInNMLSForm + this.calObjs.HMDACal.CalcBusinessOrCommercialPurpose + this.calObjs.HMDACal.CalcHmdaCDRequired + this.calObjs.HMDACal.CalcAllLoanCostsBy2015Indicator + this.calObjs.HMDACal.CalcHOEPAStatus + this.calObjs.HMDACal.CalcLenderCredits);
      this.AddFieldHandler("QM.X119", this.calObjs.ATRQMCal.CalcEligibility + this.calObjs.ATRQMCal.PopulateQMStatusInNMLSForm);
      this.AddFieldHandler("QM.X120", this.calObjs.ATRQMCal.CalcEligibility + this.calObjs.ATRQMCal.PopulateQMStatusInNMLSForm);
      this.AddFieldHandler("QM.X121", this.calObjs.ATRQMCal.CalcEligibility + this.calObjs.ATRQMCal.PopulateQMStatusInNMLSForm);
      this.AddFieldHandler("QM.X338", this.calObjs.ATRQMCal.CalcEligibility + this.calObjs.ATRQMCal.PopulateQMStatusInNMLSForm);
      this.AddFieldHandler("QM.X339", this.calObjs.ATRQMCal.CalcEligibility + this.calObjs.ATRQMCal.PopulateQMStatusInNMLSForm);
      this.AddFieldHandler("QM.X340", this.calObjs.ATRQMCal.CalcEligibility + this.calObjs.ATRQMCal.PopulateQMStatusInNMLSForm);
      this.AddFieldHandler("QM.X341", this.calObjs.ATRQMCal.CalcEligibility + this.calObjs.ATRQMCal.PopulateQMStatusInNMLSForm);
      this.AddFieldHandler("QM.X342", this.calObjs.ATRQMCal.CalcEligibility + this.calObjs.ATRQMCal.PopulateQMStatusInNMLSForm);
      this.AddFieldHandler("QM.X343", this.calObjs.ATRQMCal.CalcEligibility + this.calObjs.ATRQMCal.PopulateQMStatusInNMLSForm);
      this.AddFieldHandler("QM.X344", this.calObjs.ATRQMCal.CalcEligibility + this.calObjs.ATRQMCal.PopulateQMStatusInNMLSForm);
      this.AddFieldHandler("QM.X345", this.calObjs.ATRQMCal.CalcEligibility + this.calObjs.ATRQMCal.PopulateQMStatusInNMLSForm);
      this.AddFieldHandler("QM.X346", this.calObjs.ATRQMCal.CalcEligibility + this.calObjs.ATRQMCal.PopulateQMStatusInNMLSForm);
      this.AddFieldHandler("QM.X347", this.calObjs.ATRQMCal.CalcEligibility + this.calObjs.ATRQMCal.PopulateQMStatusInNMLSForm);
      this.AddFieldHandler("QM.X348", this.calObjs.ATRQMCal.CalcEligibility + this.calObjs.ATRQMCal.PopulateQMStatusInNMLSForm);
      this.AddFieldHandler("QM.X349", this.calObjs.ATRQMCal.CalcEligibility + this.calObjs.ATRQMCal.PopulateQMStatusInNMLSForm);
      this.AddFieldHandler("1544", this.calObjs.ATRQMCal.CalcEligibility + this.calObjs.ATRQMCal.PopulateQMStatusInNMLSForm);
      this.AddFieldHandler("1659", this.calObjs.ATRQMCal.CalcEligibility + this.calObjs.ATRQMCal.PopulateQMStatusInNMLSForm);
      this.AddFieldHandler("2982", this.calObjs.ATRQMCal.CalcEligibility + this.calObjs.ATRQMCal.PopulateQMStatusInNMLSForm);
      this.AddFieldHandler("RE88395.X316", this.calObjs.GFECal.CalcSection32 + this.calObjs.ATRQMCal.CalcEligibility + this.calObjs.ATRQMCal.PopulateQMStatusInNMLSForm + this.calObjs.NewHud2015Cal.CalcLoanTermTable_Prepayment + this.calObjs.HMDACal.CalcPrepaymentPenaltyPeriod);
      this.AddFieldHandler("QM.X112", this.calObjs.ATRQMCal.CalcEligibility + this.calObjs.ATRQMCal.PopulateQMStatusInNMLSForm);
      this.AddFieldHandler("799", this.calObjs.ATRQMCal.CalcEligibility + this.calObjs.ATRQMCal.PopulateQMStatusInNMLSForm + this.calObjs.NewHud2015Cal.CalcLECDDisplayFields + this.calObjs.HelocCal.CalcHELOCPeriodicRates);
      this.AddFieldHandler("S32DISC.X16", this.calObjs.ATRQMCal.CalcEligibility + this.calObjs.ATRQMCal.PopulateQMStatusInNMLSForm);
      this.AddFieldHandler("16", new Routine(this.calObjs.GFECal.UpdateValueExistingJuniorLien) + this.calObjs.D1003URLA2020Cal.CalcEstimatedNetMonthlyRent + this.calObjs.ATRQMCal.CalcEligibility + this.calObjs.ATRQMCal.PopulateQMStatusInNMLSForm + this.calObjs.ATRQMCal.CalDisclosureLog2015);
      this.AddFieldHandler("3850", this.calObjs.ATRQMCal.CalcEligibility + this.calObjs.ATRQMCal.PopulateQMStatusInNMLSForm);
      this.AddFieldHandler("3878", this.calObjs.ATRQMCal.CalcEligibility + this.calObjs.ATRQMCal.PopulateQMStatusInNMLSForm);
      this.AddFieldHandler("3879", this.calObjs.ATRQMCal.CalcEligibility + this.calObjs.ATRQMCal.PopulateQMStatusInNMLSForm);
      this.AddFieldHandler("3880", this.calObjs.ATRQMCal.CalcEligibility + this.calObjs.ATRQMCal.PopulateQMStatusInNMLSForm);
      this.AddFieldHandler("3042", this.calObjs.ATRQMCal.CalcEligibility + this.calObjs.ATRQMCal.PopulateQMStatusInNMLSForm);
      this.AddFieldHandler("3029", this.calObjs.ATRQMCal.CalcEligibility + this.calObjs.ATRQMCal.PopulateQMStatusInNMLSForm);
      this.AddFieldHandler("3631", this.calObjs.ATRQMCal.CalcEligibility + this.calObjs.ATRQMCal.PopulateQMStatusInNMLSForm);
      this.AddFieldHandler("3193", this.calObjs.ATRQMCal.CalcEligibility + this.calObjs.ATRQMCal.PopulateQMStatusInNMLSForm);
      this.AddFieldHandler("3195", this.calObjs.ATRQMCal.CalcEligibility + this.calObjs.ATRQMCal.PopulateQMStatusInNMLSForm);
      this.AddFieldHandler("2558", this.calObjs.ATRQMCal.CalcEligibility + this.calObjs.ATRQMCal.PopulateQMStatusInNMLSForm);
      this.AddFieldHandler("1325", this.calObjs.ATRQMCal.CalcEligibility + this.calObjs.ATRQMCal.PopulateQMStatusInNMLSForm);
      this.AddFieldHandler("1340", this.calObjs.ATRQMCal.CalcEligibility + this.calObjs.ATRQMCal.PopulateQMStatusInNMLSForm);
      this.AddFieldHandler("VASUMM.X4", this.calObjs.ATRQMCal.CalcEligibility + this.calObjs.ATRQMCal.PopulateQMStatusInNMLSForm);
      this.AddFieldHandler("VASUMM.X21", this.calObjs.ATRQMCal.CalcEligibility + this.calObjs.ATRQMCal.PopulateQMStatusInNMLSForm);
      this.AddFieldHandler("VASUMM.X30", this.calObjs.ATRQMCal.CalcEligibility + this.calObjs.ATRQMCal.PopulateQMStatusInNMLSForm);
      this.AddFieldHandler("VASUMM.X36", this.calObjs.ATRQMCal.CalcEligibility + this.calObjs.ATRQMCal.PopulateQMStatusInNMLSForm);
      for (int index = 140; index <= 220; index += 4)
      {
        this.AddFieldHandler("CD3.X" + (object) (index - 1), this.calObjs.NewHud2015Cal.CalcAdjustmentsLineK04 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalK);
        this.AddFieldHandler("CD3.X" + (object) index, this.calObjs.NewHud2015Cal.CalcAdjustmentsLineK04 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalK);
      }
      this.AddFieldHandler("CD3.X3", this.calObjs.NewHud2015Cal.CalcAdjustmentsLineK05_06_07 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalK + this.calObjs.NewHud2015Cal.CalcAdjustmentsLineKSubtotal);
      this.AddFieldHandler("L88", this.calObjs.NewHud2015Cal.CalcAdjustmentsLineK05_06_07 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalK);
      this.AddFieldHandler("CD3.X223", this.calObjs.NewHud2015Cal.CalcAdjustmentsLineK05_06_07 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalK);
      this.AddFieldHandler("CD3.X224", this.calObjs.NewHud2015Cal.CalcAdjustmentsLineK05_06_07 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalK + this.calObjs.NewHud2015Cal.CalcAdjustmentsLineKSubtotal);
      this.AddFieldHandler("CD3.X225", this.calObjs.NewHud2015Cal.CalcAdjustmentsLineK05_06_07 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalK + this.calObjs.NewHud2015Cal.CalcAdjustmentsLineKSubtotal);
      this.AddFieldHandler("CD3.X1507", this.calObjs.NewHud2015Cal.CalcAdjustmentsLineK05_06_07 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalK);
      this.AddFieldHandler("CD3.X2", this.calObjs.NewHud2015Cal.CalcAdjustmentsLineK05_06_07 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalK);
      this.AddFieldHandler("CD3.X227", this.calObjs.NewHud2015Cal.CalcAdjustmentsLineK05_06_07 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalK);
      this.AddFieldHandler("CD3.X228", this.calObjs.NewHud2015Cal.CalcAdjustmentsLineK05_06_07 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalK + this.calObjs.NewHud2015Cal.CalcAdjustmentsLineKSubtotal + this.calObjs.NewHud2015Cal.CalcLoanEstimateEstimatedCashToCloseSV);
      this.AddFieldHandler("CD3.X229", this.calObjs.NewHud2015Cal.CalcAdjustmentsLineK05_06_07 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalK + this.calObjs.NewHud2015Cal.CalcAdjustmentsLineKSubtotal);
      this.AddFieldHandler("CD3.X230", this.calObjs.NewHud2015Cal.CalcAdjustmentsLineK05_06_07 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalK);
      this.AddFieldHandler("CD3.X1508", this.calObjs.NewHud2015Cal.CalcAdjustmentsLineK05_06_07 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalK);
      for (int index = 232; index <= 252; index += 4)
      {
        this.AddFieldHandler("CD3.X" + (object) (index - 1), this.calObjs.NewHud2015Cal.CalcAdjustmentsLineK05_06_07 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalK);
        this.AddFieldHandler("CD3.X" + (object) index, this.calObjs.NewHud2015Cal.CalcAdjustmentsLineK05_06_07 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalK + this.calObjs.NewHud2015Cal.CalcAdjustmentsLineKSubtotal);
        this.AddFieldHandler("CD3.X" + (object) (index + 1), this.calObjs.NewHud2015Cal.CalcAdjustmentsLineK05_06_07 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalK + this.calObjs.NewHud2015Cal.CalcAdjustmentsLineKSubtotal);
      }
      this.AddFieldHandler("CD3.X232", this.calObjs.NewHud2015Cal.CalcLoanEstimateEstimatedCashToCloseSV);
      this.AddFieldHandler("CD3.X236", this.calObjs.NewHud2015Cal.CalcLoanEstimateEstimatedCashToCloseSV);
      this.AddFieldHandler("CD3.X240", this.calObjs.NewHud2015Cal.CalcLoanEstimateEstimatedCashToCloseSV);
      for (int index = 1509; index <= 1514; ++index)
        this.AddFieldHandler("CD3.X" + (object) (index - 1), this.calObjs.NewHud2015Cal.CalcAdjustmentsLineK05_06_07 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalK);
      this.AddFieldHandler("CD3.X7", this.calObjs.NewHud2015Cal.CalcCDPage3TotalK + this.calObjs.NewHud2015Cal.CalcAdjustmentsLineKSubtotal + this.calObjs.NewHud2015Cal.CalcLoanEstimateEstimatedCashToCloseSV);
      this.AddFieldHandler("CD3.X279", this.calObjs.NewHud2015Cal.CalcAdjustmentsLineL4_L6_L7 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalL);
      this.AddFieldHandler("CD3.X280", this.calObjs.NewHud2015Cal.CalcAdjustmentsLineL4_L6_L7 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalL + this.calObjs.NewHud2015Cal.CalcAdjustmentsLineLSubtotal + this.calObjs.NewHud2015Cal.CalcLoanEstimateEstimatedCashToCloseSV);
      this.AddFieldHandler("CD3.X282", this.calObjs.NewHud2015Cal.CalcAdjustmentsLineL4_L6_L7 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalL);
      this.AddFieldHandler("CD3.X283", this.calObjs.NewHud2015Cal.CalcAdjustmentsLineL4_L6_L7 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalL + this.calObjs.NewHud2015Cal.CalcAdjustmentsLineLSubtotal + this.calObjs.NewHud2015Cal.CalcLoanEstimateEstimatedCashToCloseSV);
      this.AddFieldHandler("CD3.X1515", this.calObjs.NewHud2015Cal.CalcAdjustmentsLineL4_L6_L7 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalL);
      this.AddFieldHandler("CD3.X1516", this.calObjs.NewHud2015Cal.CalcAdjustmentsLineL4_L6_L7 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalL);
      this.AddFieldHandler("CD3.X1540", this.calObjs.NewHud2015Cal.CalcAdjustmentsLineL4_L6_L7 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalL + this.calObjs.NewHud2015Cal.CalcAdjustmentsLineLSubtotal + this.calObjs.NewHud2015Cal.CalcLoanEstimateEstimatedCashToCloseSV);
      this.AddFieldHandler("CD3.X1541", this.calObjs.NewHud2015Cal.CalcAdjustmentsLineL4_L6_L7 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalL + this.calObjs.NewHud2015Cal.CalcAdjustmentsLineLSubtotal + this.calObjs.NewHud2015Cal.CalcLoanEstimateEstimatedCashToCloseSV);
      this.AddFieldHandler("CD3.X9", this.calObjs.NewHud2015Cal.CalcAdjustmentsLineL4_L6_L7 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalK);
      this.AddFieldHandler("CD3.X10", this.calObjs.NewHud2015Cal.CalcAdjustmentsLineL4_L6_L7 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalK + this.calObjs.NewHud2015Cal.CalcAdjustmentsLineLSubtotal + this.calObjs.NewHud2015Cal.CalcLoanEstimateEstimatedCashToCloseSV);
      this.AddFieldHandler("CD3.X285", this.calObjs.NewHud2015Cal.CalcAdjustmentsLineL4_L6_L7 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalK);
      this.AddFieldHandler("CD3.X286", this.calObjs.NewHud2015Cal.CalcAdjustmentsLineL4_L6_L7 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalL + this.calObjs.NewHud2015Cal.CalcAdjustmentsLineLSubtotal + this.calObjs.NewHud2015Cal.CalcLoanEstimateEstimatedCashToCloseSV);
      this.AddFieldHandler("CD3.X288", this.calObjs.NewHud2015Cal.CalcAdjustmentsLineL4_L6_L7 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalL + this.calObjs.NewHud2015Cal.CalcAdjustmentsLineLSubtotal + this.calObjs.NewHud2015Cal.CalcLoanEstimateEstimatedCashToCloseSV);
      this.AddFieldHandler("CD3.X1517", this.calObjs.NewHud2015Cal.CalcAdjustmentsLineL4_L6_L7 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalK);
      for (int index = 291; index <= 306; index += 5)
      {
        this.AddFieldHandler("CD3.X" + (object) (index - 1), this.calObjs.NewHud2015Cal.CalcAdjustmentsLineL4_L6_L7 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalL);
        this.AddFieldHandler("CD3.X" + (object) index, this.calObjs.NewHud2015Cal.CalcAdjustmentsLineL4_L6_L7 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalL + this.calObjs.NewHud2015Cal.CalcAdjustmentsLineLSubtotal + this.calObjs.NewHud2015Cal.CalcLoanEstimateEstimatedCashToCloseSV);
        this.AddFieldHandler("CD3.X" + (object) (index + 2), this.calObjs.NewHud2015Cal.CalcAdjustmentsLineL4_L6_L7 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalL + this.calObjs.NewHud2015Cal.CalcAdjustmentsLineLSubtotal + this.calObjs.NewHud2015Cal.CalcLoanEstimateEstimatedCashToCloseSV);
      }
      for (int index = 1518; index <= 1521; ++index)
        this.AddFieldHandler("CD3.X" + (object) (index - 1), this.calObjs.NewHud2015Cal.CalcAdjustmentsLineL4_L6_L7 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalL);
      this.AddFieldHandler("CD3.X13", this.calObjs.NewHud2015Cal.CalcAdjustmentsLineL8_L9_L10_L11 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalL);
      this.AddFieldHandler("CD3.X14", this.calObjs.NewHud2015Cal.CalcAdjustmentsLineL8_L9_L10_L11 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalL + this.calObjs.NewHud2015Cal.CalcAdjustmentsLineLSubtotal + this.calObjs.NewHud2015Cal.CalcLoanEstimateEstimatedCashToCloseSV);
      this.AddFieldHandler("CD3.X310", this.calObjs.NewHud2015Cal.CalcAdjustmentsLineL8_L9_L10_L11 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalL);
      this.AddFieldHandler("CD3.X311", this.calObjs.NewHud2015Cal.CalcAdjustmentsLineL8_L9_L10_L11 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalL + this.calObjs.NewHud2015Cal.CalcAdjustmentsLineLSubtotal + this.calObjs.NewHud2015Cal.CalcLoanEstimateEstimatedCashToCloseSV);
      this.AddFieldHandler("CD3.X312", this.calObjs.NewHud2015Cal.CalcAdjustmentsLineL8_L9_L10_L11 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalL + this.calObjs.NewHud2015Cal.CalcAdjustmentsLineLSubtotal + this.calObjs.NewHud2015Cal.CalcLoanEstimateEstimatedCashToCloseSV);
      this.AddFieldHandler("CD3.X1522", this.calObjs.NewHud2015Cal.CalcAdjustmentsLineL8_L9_L10_L11 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalL);
      this.AddFieldHandler("CD3.X15", this.calObjs.NewHud2015Cal.CalcAdjustmentsLineL8_L9_L10_L11 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalL);
      this.AddFieldHandler("CD3.X16", this.calObjs.NewHud2015Cal.CalcAdjustmentsLineL8_L9_L10_L11 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalL + this.calObjs.NewHud2015Cal.CalcAdjustmentsLineLSubtotal + this.calObjs.NewHud2015Cal.CalcLoanEstimateEstimatedCashToCloseSV);
      this.AddFieldHandler("CD3.X314", this.calObjs.NewHud2015Cal.CalcAdjustmentsLineL8_L9_L10_L11 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalL);
      this.AddFieldHandler("CD3.X315", this.calObjs.NewHud2015Cal.CalcAdjustmentsLineL8_L9_L10_L11 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalL + this.calObjs.NewHud2015Cal.CalcAdjustmentsLineLSubtotal + this.calObjs.NewHud2015Cal.CalcLoanEstimateEstimatedCashToCloseSV);
      this.AddFieldHandler("CD3.X316", this.calObjs.NewHud2015Cal.CalcAdjustmentsLineL8_L9_L10_L11 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalL + this.calObjs.NewHud2015Cal.CalcAdjustmentsLineLSubtotal + this.calObjs.NewHud2015Cal.CalcLoanEstimateEstimatedCashToCloseSV);
      this.AddFieldHandler("CD3.X1523", this.calObjs.NewHud2015Cal.CalcAdjustmentsLineL8_L9_L10_L11 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalL);
      this.AddFieldHandler("CD3.X17", this.calObjs.NewHud2015Cal.CalcAdjustmentsLineL8_L9_L10_L11 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalL);
      this.AddFieldHandler("CD3.X18", this.calObjs.NewHud2015Cal.CalcAdjustmentsLineL8_L9_L10_L11 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalL + this.calObjs.NewHud2015Cal.CalcAdjustmentsLineLSubtotal + this.calObjs.NewHud2015Cal.CalcLoanEstimateEstimatedCashToCloseSV);
      this.AddFieldHandler("CD3.X318", this.calObjs.NewHud2015Cal.CalcAdjustmentsLineL8_L9_L10_L11 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalL);
      this.AddFieldHandler("CD3.X319", this.calObjs.NewHud2015Cal.CalcAdjustmentsLineL8_L9_L10_L11 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalL + this.calObjs.NewHud2015Cal.CalcAdjustmentsLineLSubtotal + this.calObjs.NewHud2015Cal.CalcLoanEstimateEstimatedCashToCloseSV);
      this.AddFieldHandler("CD3.X320", this.calObjs.NewHud2015Cal.CalcAdjustmentsLineL8_L9_L10_L11 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalL + this.calObjs.NewHud2015Cal.CalcAdjustmentsLineLSubtotal + this.calObjs.NewHud2015Cal.CalcLoanEstimateEstimatedCashToCloseSV);
      this.AddFieldHandler("CD3.X1524", this.calObjs.NewHud2015Cal.CalcAdjustmentsLineL8_L9_L10_L11 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalL);
      for (int index = 323; index <= 336; index += 4)
      {
        this.AddFieldHandler("CD3.X" + (object) (index - 1), this.calObjs.NewHud2015Cal.CalcAdjustmentsLineL8_L9_L10_L11 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalL);
        this.AddFieldHandler("CD3.X" + (object) index, this.calObjs.NewHud2015Cal.CalcAdjustmentsLineL8_L9_L10_L11 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalL + this.calObjs.NewHud2015Cal.CalcAdjustmentsLineLSubtotal + this.calObjs.NewHud2015Cal.CalcLoanEstimateEstimatedCashToCloseSV);
        this.AddFieldHandler("CD3.X" + (object) (index + 1), this.calObjs.NewHud2015Cal.CalcAdjustmentsLineL8_L9_L10_L11 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalL + this.calObjs.NewHud2015Cal.CalcAdjustmentsLineLSubtotal + this.calObjs.NewHud2015Cal.CalcLoanEstimateEstimatedCashToCloseSV);
      }
      for (int index = 1525; index <= 1528; ++index)
        this.AddFieldHandler("CD3.X" + (object) (index - 1), this.calObjs.NewHud2015Cal.CalcAdjustmentsLineL8_L9_L10_L11 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalL);
      for (int index = 354; index <= 439; index += 5)
      {
        this.AddFieldHandler("CD3.X" + (object) (index - 1), this.calObjs.NewHud2015Cal.CalcAdjustmentsLineL17 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalL);
        this.AddFieldHandler("CD3.X" + (object) index, this.calObjs.NewHud2015Cal.CalcAdjustmentsLineL17 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalL + this.calObjs.NewHud2015Cal.CalcAdjustmentsLineLSubtotal + this.calObjs.NewHud2015Cal.CalcLoanEstimateEstimatedCashToCloseSV);
      }
      this.AddFieldHandler("L81", this.calObjs.NewHud2015Cal.CalcAdjustmentsLineM03 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalM);
      this.AddFieldHandler("CD3.X1529", this.calObjs.NewHud2015Cal.CalcAdjustmentsLineM03 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalM);
      for (int index = 443; index <= 445; ++index)
        this.AddFieldHandler("CD3.X" + (object) index, this.calObjs.NewHud2015Cal.CalcAdjustmentsLineM03 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalM);
      this.AddFieldHandler("L86", this.calObjs.NewHud2015Cal.CalcAdjustmentsLineM04 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalM);
      this.AddFieldHandler("CD3.X1530", this.calObjs.NewHud2015Cal.CalcAdjustmentsLineM04 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalM);
      for (int index = 467; index <= 469; ++index)
        this.AddFieldHandler("CD3.X" + (object) index, this.calObjs.NewHud2015Cal.CalcAdjustmentsLineM04 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalM);
      this.AddFieldHandler("L90", this.calObjs.NewHud2015Cal.CalcAdjustmentsLineM05 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalM);
      this.AddFieldHandler("CD3.X1531", this.calObjs.NewHud2015Cal.CalcAdjustmentsLineM05 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalM);
      for (int index = 491; index <= 493; ++index)
        this.AddFieldHandler("CD3.X" + (object) index, this.calObjs.NewHud2015Cal.CalcAdjustmentsLineM05 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalM);
      this.AddFieldHandler("CD3.X24", this.calObjs.NewHud2015Cal.CalcAdjustmentsLineM06 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalM);
      this.AddFieldHandler("CD3.X25", this.calObjs.NewHud2015Cal.CalcAdjustmentsLineM06 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalM);
      this.AddFieldHandler("CD3.X1532", this.calObjs.NewHud2015Cal.CalcAdjustmentsLineM06 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalM);
      for (int index = 515; index <= 517; ++index)
        this.AddFieldHandler("CD3.X" + (object) index, this.calObjs.NewHud2015Cal.CalcAdjustmentsLineM06 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalM);
      this.AddFieldHandler("CD3.X26", this.calObjs.NewHud2015Cal.CalcAdjustmentsLineM07 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalM);
      this.AddFieldHandler("CD3.X27", this.calObjs.NewHud2015Cal.CalcAdjustmentsLineM07 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalM);
      this.AddFieldHandler("CD3.X1533", this.calObjs.NewHud2015Cal.CalcAdjustmentsLineM07 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalM);
      for (int index = 539; index <= 541; ++index)
        this.AddFieldHandler("CD3.X" + (object) index, this.calObjs.NewHud2015Cal.CalcAdjustmentsLineM07 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalM);
      for (int index = 564; index <= 584; index += 4)
      {
        this.AddFieldHandler("CD3.X" + (object) (index - 1), this.calObjs.NewHud2015Cal.CalcAdjustmentsLineM08 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalM);
        this.AddFieldHandler("CD3.X" + (object) index, this.calObjs.NewHud2015Cal.CalcAdjustmentsLineM08 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalM);
        this.AddFieldHandler("CD3.X" + (object) (index + 1), this.calObjs.NewHud2015Cal.CalcAdjustmentsLineM08 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalM);
      }
      for (int index = 1534; index <= 1539; ++index)
        this.AddFieldHandler("CD3.X" + (object) index, this.calObjs.NewHud2015Cal.CalcAdjustmentsLineM08 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalM);
      for (int index = 957; index <= 1042; index += 5)
      {
        this.AddFieldHandler("CD3.X" + (object) (index - 1), this.calObjs.NewHud2015Cal.CalcAdjustmentsLineM16 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalM);
        this.AddFieldHandler("CD3.X" + (object) index, this.calObjs.NewHud2015Cal.CalcAdjustmentsLineM16 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalM);
      }
      for (int index = 1068; index <= 1086; index += 3)
      {
        this.AddFieldHandler("CD3.X" + (object) (index - 1), this.calObjs.NewHud2015Cal.CalcAdjustmentsLineN07 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalN);
        this.AddFieldHandler("CD3.X" + (object) index, this.calObjs.NewHud2015Cal.CalcAdjustmentsLineN07 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalN);
      }
      for (int index = 1197; index <= 1221; index += 3)
      {
        this.AddFieldHandler("CD3.X" + (object) (index - 1), this.calObjs.NewHud2015Cal.CalcAdjustmentsLineN13 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalN);
        this.AddFieldHandler("CD3.X" + (object) index, this.calObjs.NewHud2015Cal.CalcAdjustmentsLineN13 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalN);
      }
      for (int index = 1413; index <= 1498; index += 5)
      {
        this.AddFieldHandler("CD3.X" + (object) (index - 1), this.calObjs.NewHud2015Cal.CalcAdjustmentsLineN19 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalN);
        this.AddFieldHandler("CD3.X" + (object) index, this.calObjs.NewHud2015Cal.CalcAdjustmentsLineN19 + this.calObjs.NewHud2015Cal.CalcCDPage3TotalN);
      }
      this.AddFieldHandler("CD3.X1504", this.calObjs.NewHud2015Cal.CalcTotalAdjustmentAndOtherCredits + this.calObjs.NewHud2015Cal.CalcCDPage3StdCashToCloseLoanEstimate + this.calObjs.NewHud2015Cal.CalcCDPage3StdCashToCloseFinal + this.calObjs.NewHud2015Cal.CalcCDPage3AlternateCashToCloseFinal + this.calObjs.NewHud2015Cal.CalcLoanEstimateEstimatedCashToCloseSV + this.calObjs.NewHud2015Cal.CalcLoanEstimate_EstimatedCashToClose);
      this.AddFieldHandler("CD3.X1505", this.calObjs.NewHud2015Cal.CalcTotalAdjustmentAndOtherCredits + this.calObjs.NewHud2015Cal.CalcCDPage3StdCashToCloseLoanEstimate + this.calObjs.NewHud2015Cal.CalcCDPage3StdCashToCloseFinal + this.calObjs.NewHud2015Cal.CalcCDPage3AlternateCashToCloseFinal + this.calObjs.NewHud2015Cal.CalcLoanEstimateEstimatedCashToCloseSV + this.calObjs.NewHud2015Cal.CalcLoanEstimate_EstimatedCashToClose);
      this.AddFieldHandler("CONST.X1", this.calObjs.D1003Cal.PopulateOtherFields + this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + new Routine(this.calculateUCD) + this.calObjs.HMDACal.CalcLoanTerm + this.calObjs.RegzCal.CalcPMIMidpointCancelationDate + this.calObjs.NewHud2015Cal.CalculateProductDescription + this.calObjs.NewHud2015Cal.Calc60Payments + this.calObjs.HMDACal.CalcLoanTerm + this.calObjs.RegzCal.CalcPMIMidpointCancelationDate + this.calObjs.D1003URLA2020Cal.CalcEstimatedNetMonthlyRent);
      this.AddFieldHandler("3567", this.calObjs.ToolCal.UpdateCorrespondentPrincipal + this.calObjs.ToolCal.CopyFromCorrespondentPurchaseAdvice);
      this.AddFieldHandler("3579", this.calObjs.ToolCal.UpdateCorrespondentPrincipal);
      this.AddFieldHandler("4106", this.calObjs.ToolCal.CopyFromCorrespondentPrincipal);
      this.AddFieldHandler("2370", this.calObjs.ToolCal.LoadPurchaseAdvicePrincipal + this.calObjs.ToolCal.UpdateCorrespondentPrincipal);
      this.AddFieldHandler("2211", this.calObjs.ToolCal.LoadPurchaseAdvicePrincipal + this.calObjs.ToolCal.UpdateCorrespondentPrincipal);
      this.AddFieldHandler("3514", this.calObjs.ToolCal.LoadPurchaseAdvicePrincipal);
      this.AddFieldHandler("HUD68", this.calObjs.Hud1Cal.CalcEscrowFirstPaymentDate + this.calObjs.GFECal.CalcGFEFees + new Routine(this.calculateUCD) + new Routine(((CalculationBase) this.calObjs.NewHudCal).FormCal) + this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.Hud1Cal.CalcHUD1ES + this.calObjs.NewHud2015Cal.CalcWillHaveEscrowAccount + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + this.calObjs.RegzCal.CalcPMIMidpointCancelationDate);
      this.AddFieldHandler("HUD69", this.calObjs.Hud1Cal.CalcEscrowFirstPaymentDate + this.calObjs.GFECal.CalcGFEFees + new Routine(this.calculateUCD) + new Routine(((CalculationBase) this.calObjs.NewHudCal).FormCal) + this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.Hud1Cal.CalcHUD1ES + this.calObjs.NewHud2015Cal.CalcWillHaveEscrowAccount + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.RegzCal.CalcAPR + this.calObjs.RegzCal.CalcPMIMidpointCancelationDate);
      this.AddFieldHandler("4000", this.UpdateBorrowerVestingName + this.calObjs.ToolCal.SyncConstructionLinkSyncDefaultData + this.calObjs.D1003URLA2020Cal.CalcTitleInWhichPropertyHeld + this.calObjs.NewHudCal.EnforceGFEAppDateRule + this.calObjs.NewHudCal.CalcInitialDisclosureDueDate + this.calObjs.ToolCal.CalcConsumerHIOrderEligible);
      this.AddFieldHandler("4001", this.UpdateBorrowerVestingName + this.calObjs.ToolCal.SyncConstructionLinkSyncDefaultData + this.calObjs.D1003URLA2020Cal.CalcTitleInWhichPropertyHeld);
      this.AddFieldHandler("4002", this.calObjs.D1003Cal.CopyInfoToLockRequest + this.UpdateBorrowerVestingName + this.calObjs.ToolCal.SyncConstructionLinkSyncDefaultData + this.calObjs.D1003URLA2020Cal.CalcTitleInWhichPropertyHeld + this.calObjs.D1003URLA2020Cal.CalcBorrowerCount + this.calObjs.D1003URLA2020Cal.CalcCreditType + this.calObjs.NewHudCal.EnforceGFEAppDateRule + this.calObjs.NewHudCal.CalcInitialDisclosureDueDate + this.calObjs.ToolCal.CalcConsumerHIOrderEligible);
      this.AddFieldHandler("4003", this.UpdateBorrowerVestingName + this.calObjs.ToolCal.SyncConstructionLinkSyncDefaultData + this.calObjs.D1003URLA2020Cal.CalcTitleInWhichPropertyHeld);
      this.AddFieldHandler("4008", this.calObjs.ToolCal.SyncConstructionLinkSyncDefaultData + this.calObjs.D1003URLA2020Cal.CalcTitleInWhichPropertyHeld);
      this.AddFieldHandler("4004", this.UpdateCoborrowerVestingName + this.calObjs.ToolCal.SyncConstructionLinkSyncDefaultData + this.calObjs.D1003URLA2020Cal.CalcTitleInWhichPropertyHeld);
      this.AddFieldHandler("4005", this.UpdateCoborrowerVestingName + this.calObjs.ToolCal.SyncConstructionLinkSyncDefaultData + this.calObjs.D1003URLA2020Cal.CalcTitleInWhichPropertyHeld);
      this.AddFieldHandler("4006", this.calObjs.D1003Cal.CopyInfoToLockRequest + this.UpdateCoborrowerVestingName + this.calObjs.ToolCal.SyncConstructionLinkSyncDefaultData + this.calObjs.D1003URLA2020Cal.CalcTitleInWhichPropertyHeld + this.calObjs.D1003URLA2020Cal.CalcBorrowerCount + this.calObjs.D1003URLA2020Cal.CalcCreditType);
      this.AddFieldHandler("4007", this.UpdateCoborrowerVestingName + this.calObjs.ToolCal.SyncConstructionLinkSyncDefaultData + this.calObjs.D1003URLA2020Cal.CalcTitleInWhichPropertyHeld);
      this.AddFieldHandler("4009", this.calObjs.ToolCal.SyncConstructionLinkSyncDefaultData + this.calObjs.D1003URLA2020Cal.CalcTitleInWhichPropertyHeld);
      this.AddFieldHandler("LE1.X1", this.calObjs.ToolCal.SyncConstructionLinkSyncDefaultData);
      this.AddFieldHandler("LE1.X28", this.calObjs.ToolCal.SyncConstructionLinkSyncDefaultData + this.calObjs.NewHud2015Cal.CalcIntentToProceed);
      this.AddFieldHandler("LE1.X8", this.calObjs.ToolCal.SyncConstructionLinkSyncDefaultData + this.calObjs.NewHud2015Cal.CalcIntentToProceed);
      this.AddFieldHandler("LE1.X9", this.calObjs.ToolCal.SyncConstructionLinkSyncDefaultData + this.calObjs.NewHud2015Cal.CalcIntentToProceed);
      this.AddFieldHandler("CD1.X1", this.calObjs.ToolCal.SyncConstructionLinkSyncDefaultData);
      this.AddFieldHandler("3972", this.calObjs.ToolCal.SyncConstructionLinkSyncDefaultData);
      this.AddFieldHandler("CONST.X2", this.calObjs.RegzCal.CalcLateExemptRightRescissionFlag);
      this.AddFieldHandler("AFF.X3", this.calObjs.ToolCal.UpdatePercentageOwnershipInterest);
      this.AddFieldHandler("ULDD.X1", this.calObjs.HMDACal.CalcLoanAmount);
      this.AddFieldHandler("CASASRN.X168", this.calObjs.VERIFCal.CalcHelocLineTotal + this.calObjs.HMDACal.CalcLoanAmount + this.CalcLTV + this.calObjs.PrequalCal.CalcHELOCLockRequest + this.calObjs.PrequalCal.CalcLockRequestLoan);
      this.AddFieldHandler("4457", this.calObjs.HMDACal.CalcLoanAmount);
      this.AddFieldHandler("4458", this.calObjs.HMDACal.CalcLoanAmount);
      this.AddFieldHandler("4181", this.calObjs.HMDACal.CalcIncome);
      this.AddFieldHandler("1389", this.calObjs.HMDACal.CalcIncome + this.calObjs.PrequalCal.CalcMinMax);
      this.AddFieldHandler("736", this.calObjs.HMDACal.CalcIncome + this.calObjs.NewHudCal.EnforceGFEAppDateRule + this.calObjs.NewHudCal.CalcInitialDisclosureDueDate);
      this.AddFieldHandler("HMDA.X99", this.calObjs.HMDACal.CalcIncome);
      this.AddFieldHandler("HMDA.X11", this.calObjs.HMDACal.CalcIncome);
      this.AddFieldHandler("HMDA.X110", this.calObjs.HMDACal.CalcIncome);
      this.AddFieldHandler("ULDD.X172", this.calObjs.HMDACal.CalcManufacturedProperty);
      this.AddFieldHandler("1543", this.calObjs.ATRQMCal.CalcEligibility + this.CalcOthers + this.calObjs.ULDDExpCal.CalcFannieMaeExportFields + this.calObjs.HMDACal.CalcRiskAssessment);
      this.AddFieldHandler("1544", this.calObjs.HMDACal.CalcRiskAssessment);
      this.AddFieldHandler("1556", this.calObjs.HMDACal.CalcRiskAssessment);
      this.AddFieldHandler("NMLS.X10", this.calObjs.HMDACal.CalcLoanType);
      this.AddFieldHandler("4180", this.calObjs.VACal.CalcMaximumSellerContribution);
      this.AddFieldHandler("VASUMM.X99", this.calObjs.VACal.CalcVARecoupment);
      for (int index = 297; index <= 305; ++index)
        this.AddFieldHandler("RE88395.X" + index.ToString(), this.calObjs.MLDSCal.CalcMLDSScenarios + this.calObjs.HMDACal.CalcPrepaymentPenaltyPeriod);
      this.AddFieldHandler("RE88395.X313", this.calObjs.MLDSCal.CalcMLDSScenarios + this.calObjs.HMDACal.CalcPrepaymentPenaltyPeriod);
      this.AddFieldHandler("RE88395.X314", this.calObjs.MLDSCal.CalcMLDSScenarios + this.calObjs.HMDACal.CalcPrepaymentPenaltyPeriod);
      this.AddFieldHandler("RE88395.X319", this.calObjs.MLDSCal.CalcMLDSScenarios + this.calObjs.HMDACal.CalcPrepaymentPenaltyPeriod);
      this.AddFieldHandler("RE88395.X320", this.calObjs.MLDSCal.CalcMLDSScenarios + this.calObjs.HMDACal.CalcPrepaymentPenaltyPeriod);
      this.AddFieldHandler("RE88395.X289", this.calObjs.MLDSCal.CalcMLDSScenarios + this.calObjs.HMDACal.CalcPrepaymentPenaltyPeriod);
      this.AddFieldHandler("RE88395.X290", this.calObjs.MLDSCal.CalcMLDSScenarios + this.calObjs.HMDACal.CalcPrepaymentPenaltyPeriod);
      this.AddFieldHandler("RE88395.X291", this.calObjs.MLDSCal.CalcMLDSScenarios + this.calObjs.HMDACal.CalcPrepaymentPenaltyPeriod);
      this.AddFieldHandler("RE88395.X292", this.calObjs.MLDSCal.CalcMLDSScenarios + this.calObjs.HMDACal.CalcPrepaymentPenaltyPeriod);
      this.AddFieldHandler("RE88395.X294", this.calObjs.MLDSCal.CalcMLDSScenarios + this.calObjs.HMDACal.CalcPrepaymentPenaltyPeriod);
      this.AddFieldHandler("RE88395.X296", this.calObjs.MLDSCal.CalcMLDSScenarios + this.calObjs.HMDACal.CalcPrepaymentPenaltyPeriod);
      this.AddFieldHandler("RE88395.X333", this.calObjs.MLDSCal.CalcMLDSScenarios + this.calObjs.HMDACal.CalcPrepaymentPenaltyPeriod);
      this.AddFieldHandler("RE88395.X334", this.calObjs.MLDSCal.CalcMLDSScenarios + this.calObjs.HMDACal.CalcPrepaymentPenaltyPeriod);
      this.AddFieldHandler("RE88395.X335", this.calObjs.MLDSCal.CalcMLDSScenarios + this.calObjs.HMDACal.CalcPrepaymentPenaltyPeriod);
      this.AddFieldHandler("NTB.X13", this.calObjs.ToolCal.CalcNetTangibleBenefit + this.calObjs.HMDACal.CalcPrepaymentPenaltyPeriod);
      this.AddFieldHandler("NTB.X20", this.calObjs.ToolCal.CalcNetTangibleBenefit + this.calObjs.HMDACal.CalcPrepaymentPenaltyPeriod);
      this.AddFieldHandler("NTB.X21", this.calObjs.ToolCal.CalcNetTangibleBenefit + this.calObjs.HMDACal.CalcPrepaymentPenaltyPeriod);
      this.AddFieldHandler("NTB.X22", this.calObjs.ToolCal.CalcNetTangibleBenefit + this.calObjs.HMDACal.CalcPrepaymentPenaltyPeriod + this.calObjs.VACal.CalcVACashOutRefinance);
      this.AddFieldHandler("NTB.X30", this.calObjs.ToolCal.CalcNetTangibleBenefit + this.calObjs.HMDACal.CalcPrepaymentPenaltyPeriod);
      this.AddFieldHandler("NTB.X30", this.calObjs.ToolCal.CalcNetTangibleBenefit + this.calObjs.HMDACal.CalcPrepaymentPenaltyPeriod);
      this.AddFieldHandler("NTB.X35", this.calObjs.ToolCal.CalcNetTangibleBenefit + this.calObjs.HMDACal.CalcPrepaymentPenaltyPeriod);
      this.AddFieldHandler("HMDA.X27", this.calObjs.ToolCal.CalcHMDAReportingYear + this.calObjs.D1003Cal.Calc2018DI + this.calObjs.D1003Cal.CalcUli + this.calObjs.HMDACal.CalcTotalLoanCosts + this.calObjs.HMDACal.CalcTotalPointsAndFees + this.calObjs.HMDACal.CalcOriginationCharges + this.calObjs.HMDACal.CalcDiscountPoints + this.calObjs.HMDACal.CalcReasonForDenial + this.calObjs.HMDACal.CalcTypeOfPurchaser + this.calObjs.HMDACal.CalcHOEPAStatus + this.calObjs.HMDACal.CalcHmdaRateSpread + this.calObjs.HMDACal.CalcHmdaSyncaddressfields + this.calObjs.HMDACal.CalcCreditScoreForDecisionMaking + this.calObjs.HMDACal.CalcCreditScoringModel + this.calObjs.HMDACal.CalcHMDAAusRecommendation + this.calObjs.HMDACal.CalcHmdaLoanPurpose + this.calObjs.HMDACal.CalcHmdaLoanPurpose + this.calObjs.HMDACal.CalcIncome);
      this.AddFieldHandler("1523", this.calObjs.D1003Cal.CalcEthnicityRaceSex + this.calObjs.HMDACal.CalcEthnicity);
      this.AddFieldHandler("1537", this.calObjs.D1003Cal.CalcEthnicityRaceSex + this.calObjs.HMDACal.CalcDoNotWish + this.calObjs.HMDACal.CalcSortRaceCategory);
      this.AddFieldHandler("4143", this.calObjs.D1003Cal.CalcEthnicityRaceSex + this.calObjs.HMDACal.CalcDoNotWish + this.calObjs.HMDACal.CalcSortRaceCategory + this.calObjs.HMDACal.CalcSortEthnicityCategory);
      this.AddFieldHandler("4131", this.calObjs.D1003Cal.CalcEthnicityRaceSex + this.calObjs.HMDACal.CalcDoNotWish + this.calObjs.HMDACal.CalcSortRaceCategory + this.calObjs.HMDACal.CalcSortEthnicityCategory);
      Routine routine10 = this.RoutineX(this.calObjs.D1003Cal.CalcEthnicityRaceSex + this.calObjs.HMDACal.CalcSortRaceCategory);
      this.AddFieldHandler("1524", routine10);
      this.AddFieldHandler("1525", routine10);
      this.AddFieldHandler("1526", routine10);
      this.AddFieldHandler("1527", routine10);
      this.AddFieldHandler("1528", routine10);
      this.AddFieldHandler("1530", routine10);
      this.AddFieldHandler("4148", routine10);
      this.AddFieldHandler("4149", routine10);
      this.AddFieldHandler("4150", routine10);
      this.AddFieldHandler("4151", routine10);
      this.AddFieldHandler("4152", routine10);
      this.AddFieldHandler("4153", routine10);
      this.AddFieldHandler("4154", routine10);
      this.AddFieldHandler("4155", routine10);
      this.AddFieldHandler("4156", routine10);
      this.AddFieldHandler("4157", routine10);
      this.AddFieldHandler("4158", routine10);
      this.AddFieldHandler("1532", routine10);
      this.AddFieldHandler("1533", routine10);
      this.AddFieldHandler("1534", routine10);
      this.AddFieldHandler("1535", routine10);
      this.AddFieldHandler("1536", routine10);
      this.AddFieldHandler("1538", routine10);
      this.AddFieldHandler("4163", routine10);
      this.AddFieldHandler("4164", routine10);
      this.AddFieldHandler("4165", routine10);
      this.AddFieldHandler("4166", routine10);
      this.AddFieldHandler("4167", routine10);
      this.AddFieldHandler("4168", routine10);
      this.AddFieldHandler("4169", routine10);
      this.AddFieldHandler("4170", routine10);
      this.AddFieldHandler("4171", routine10);
      this.AddFieldHandler("4172", routine10);
      this.AddFieldHandler("4173", routine10);
      this.AddFieldHandler("4244", routine10);
      this.AddFieldHandler("4247", routine10);
      this.AddFieldHandler("4252", this.calObjs.D1003Cal.CalcEthnicityRaceSex + this.calObjs.HMDACal.CalcDoNotWish + this.calObjs.HMDACal.CalcSortRaceCategory);
      this.AddFieldHandler("4253", this.calObjs.D1003Cal.CalcEthnicityRaceSex + this.calObjs.HMDACal.CalcDoNotWish + this.calObjs.HMDACal.CalcSortRaceCategory);
      this.AddFieldHandler("3174", this.calObjs.D1003Cal.CalcEthnicityRaceSex + this.calObjs.HMDACal.CalcNocoapplicant + this.calObjs.HMDACal.CalcSortRaceCategory);
      this.AddFieldHandler("1529", this.RoutineX(this.calObjs.D1003Cal.CalcEthnicityRaceSex + this.calObjs.HMDACal.CalcDoNotWish + this.calObjs.HMDACal.CalcSortRaceCategory));
      this.AddFieldHandler("CONST.X58", this.CalcLTV + this.calObjs.PrequalCal.CalcCopyConstructionFields + this.calObjs.PrequalCal.CalcLockRequestLoan);
      this.AddFieldHandler("CONST.X59", this.CalcLTV + this.calObjs.PrequalCal.CalcCopyConstructionFields + this.calObjs.PrequalCal.CalcLockRequestLoan + this.calObjs.NewHud2015Cal.CalcAsCompletedAppraisedValue);
      this.AddFieldHandler("VARRRWS.X9", this.calObjs.VACal.CalcVARRRWS);
      this.AddFieldHandler("VARRRWS.X9", this.calObjs.VACal.CalcVARRRWS);
      this.AddFieldHandler("HMDA.X56", this.calObjs.HMDACal.CalcLoanAmount + this.calObjs.HMDACal.CalcLoanTerm + this.calObjs.HMDACal.CalcPrepaymentPenaltyPeriod + this.calObjs.HMDACal.CalcHmdaRateSpread + this.calObjs.HMDACal.CalcHOEPAStatus + this.calObjs.HMDACal.CalcTotalLoanCosts + this.calObjs.HMDACal.CalcTotalPointsAndFees + this.calObjs.HMDACal.CalcOriginationCharges + this.calObjs.HMDACal.CalcHmdaCDRequired + this.calObjs.HMDACal.CalcLenderCredits + this.calObjs.HMDACal.CalcDiscountPoints);
      this.AddFieldHandler("HMDA.X57", this.calObjs.HMDACal.CalcLoanAmount + this.calObjs.HMDACal.CalcLoanTerm + this.calObjs.HMDACal.CalcTotalLoanCosts + this.calObjs.HMDACal.CalcTotalPointsAndFees + this.calObjs.HMDACal.CalcOriginationCharges + this.calObjs.HMDACal.CalcHmdaCDRequired + this.calObjs.HMDACal.CalcLenderCredits + this.calObjs.HMDACal.CalcDiscountPoints);
      this.AddFieldHandler("HMDA.X58", this.calObjs.HMDACal.CalcPrepaymentPenaltyPeriod + this.calObjs.HMDACal.CalcTotalLoanCosts + this.calObjs.HMDACal.CalcTotalPointsAndFees + this.calObjs.HMDACal.CalcOriginationCharges + this.calObjs.HMDACal.CalcHmdaCDRequired + this.calObjs.HMDACal.CalcLenderCredits + this.calObjs.HMDACal.CalcDiscountPoints);
      this.AddFieldHandler("CD2.XSTA", this.calObjs.HMDACal.CalcHmdaOriginationCharges + this.calObjs.HMDACal.CalcDiscountPoints + this.calObjs.VACal.CalcVARecoupment);
      this.AddFieldHandler("1959", this.calObjs.NewHudCal.SetARMIndexDescription + this.calObjs.PrequalCal.CalcHELOCLockRequest);
      this.AddFieldHandler("906", this.calObjs.FHACal.CalcMACAWP + this.calObjs.USDACal.CalcIncomeWorksheet);
      this.AddFieldHandler("4490", new Routine(this.calculateValidDrawAmount) + this.calObjs.VERIFCal.CalcHelocLineTotal + this.calObjs.HMDACal.CalcLoanAmount + this.CalcLTV);
      this.AddFieldHandler("4487", this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.VERIFCal.CalcHelocLineTotal + this.CalcLTV + this.calObjs.PrequalCal.CalcLockRequestLoan);
      this.AddFieldHandler("4488", this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.VERIFCal.CalcHelocLineTotal + this.CalcLTV + this.calObjs.PrequalCal.CalcLockRequestLoan);
      this.AddFieldHandler("4489", new Routine(this.calculateValidDrawAmount) + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.VERIFCal.CalcHelocLineTotal + this.CalcLTV + this.calObjs.PrequalCal.CalcLockRequestLoan);
      this.AddFieldHandler("CASASRN.X167", this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.VERIFCal.CalcHelocLineTotal + this.CalcLTV + this.calObjs.PrequalCal.CalcLockRequestLoan);
      this.AddFieldHandler("4492", this.calObjs.HMDACal.CalcIntroRatePeriod);
      this.AddFieldHandler("52", this.calObjs.D1003URLA2020Cal.UpdateMarriedStatus);
      this.AddFieldHandler("84", this.calObjs.D1003URLA2020Cal.UpdateMarriedStatus);
      Routine routine11 = this.RoutineX(new Routine(this.calculateHelocDrawAmount));
      this.AddFieldHandler("4523", routine11);
      this.AddFieldHandler("4520", routine11 + new Routine(this.calculateHelocLoanAmount));
      this.AddFieldHandler("4521", routine11);
      Routine routine12 = this.RoutineX(new Routine(this.calculateHelocLoanAmount));
      this.AddFieldHandler("4524", routine12);
      this.AddFieldHandler("4522", routine12);
      this.AddFieldHandler("CONST.X13", this.calObjs.HMDACal.CalcIntroRatePeriod);
      this.AddFieldHandler("URLA.X76", this.calObjs.PrequalCal.CalcFHASecondaryResidence);
      this.AddFieldHandler("URLA.X194", this.calObjs.D1003URLA2020Cal.UpdateBorrowerAliasName + this.calObjs.D1003URLA2020Cal.CalcBorrowerCount + this.calObjs.D1003URLA2020Cal.CalcCreditType);
      this.AddFieldHandler("NBOC0001", this.calObjs.ToolCal.CalcNBOtoVesting + this.calObjs.D1003URLA2020Cal.CalcTitleInWhichPropertyHeld);
      this.AddFieldHandler("NBOC0002", this.calObjs.ToolCal.CalcNBOtoVesting + this.calObjs.D1003URLA2020Cal.CalcTitleInWhichPropertyHeld);
      this.AddFieldHandler("NBOC0003", this.calObjs.ToolCal.CalcNBOtoVesting + this.calObjs.D1003URLA2020Cal.CalcTitleInWhichPropertyHeld);
      this.AddFieldHandler("NBOC0004", this.calObjs.ToolCal.CalcNBOtoVesting + this.calObjs.D1003URLA2020Cal.CalcTitleInWhichPropertyHeld);
      this.AddFieldHandler("NBOC0009", this.calObjs.ToolCal.CalcNBOtoVesting + this.calObjs.D1003URLA2020Cal.CalcTitleInWhichPropertyHeld);
      this.AddFieldHandler("NBOC0011", this.calObjs.ToolCal.CalcNBOtoVesting);
      this.AddFieldHandler("NBOC0016", this.calObjs.ToolCal.CalcNBOtoVesting);
      this.AddFieldHandler("1868", this.calObjs.D1003URLA2020Cal.CalcTitleInWhichPropertyHeld);
      this.AddFieldHandler("1871", this.calObjs.D1003URLA2020Cal.CalcTitleInWhichPropertyHeld);
      this.AddFieldHandler("1873", this.calObjs.D1003URLA2020Cal.CalcTitleInWhichPropertyHeld);
      this.AddFieldHandler("1876", this.calObjs.D1003URLA2020Cal.CalcTitleInWhichPropertyHeld);
      this.AddFieldHandler("TR0001", this.calObjs.D1003URLA2020Cal.CalcTitleInWhichPropertyHeld);
      this.AddFieldHandler("TR0004", this.calObjs.ToolCal.CalcNBOtoVesting + this.calObjs.D1003URLA2020Cal.CalcTitleInWhichPropertyHeld);
      this.AddFieldHandler("TR0012", this.calObjs.ToolCal.CalcNBOtoVesting);
      this.AddFieldHandler("TR0012", this.calObjs.ToolCal.CalcNBOtoVesting);
      this.AddFieldHandler("QM.X378", this.calObjs.RegzCal.CalcBuydownSummary + this.calObjs.GFECal.CalcSection32);
      this.AddFieldHandler("S32DISC.X181", this.calObjs.GFECal.CalcSection32);
      this.AddFieldHandler("HMDA.X117", this.calObjs.HMDACal.CalcCreditScoringModel);
      this.AddFieldHandler("HMDA.X119", this.calObjs.HMDACal.CalcCreditScoringModel);
      this.AddFieldHandler("4175", this.calObjs.HMDACal.CalcCreditScoringModel + this.calObjs.HMDACal.CalcOtherCreditScoringModel);
      this.AddFieldHandler("4176", this.calObjs.HMDACal.CalcCreditScoringModel);
      this.AddFieldHandler("4178", this.calObjs.HMDACal.CalcCreditScoringModel + this.calObjs.HMDACal.CalcOtherCreditScoringModel);
      this.AddFieldHandler("4179", this.calObjs.HMDACal.CalcCreditScoringModel);
      Routine routine13 = this.RoutineX(this.calObjs.VERIFCal.CalcOtherIncome + this.calObjs.D1003Cal.CalcOtherIncome + this.calObjs.D1003URLA2020Cal.CalcBackEndRatio + this.calObjs.ComortCal.CalcComortgagor);
      this.AddFieldHandler("URLAROIS0022", routine13);
      this.AddFieldHandler("URLAROIS0002", routine13);
      this.AddFieldHandler("URLAROIS0018", routine13);
      this.AddFieldHandler("URLAROIS0022", routine13);
      this.AddFieldHandler("URLA.X42", routine13);
      this.AddFieldHandler("URLA.X43", routine13);
      Routine routine14 = this.RoutineX(this.calObjs.VERIFCal.CalcMonthlyIncome + this.calObjs.D1003Cal.CalcOtherIncome + this.calObjs.D1003URLA2020Cal.CalcBackEndRatio + this.calObjs.ComortCal.CalcComortgagor + this.calObjs.USDACal.CalcIncomeWorksheet);
      this.AddFieldHandler("BE0053", routine14);
      this.AddFieldHandler("CE0053", routine14);
      this.AddFieldHandler("BE0009", routine14);
      this.AddFieldHandler("CE0009", routine14);
      this.AddFieldHandler("FE0153", routine14);
      this.AddFieldHandler("FE0253", routine14);
      this.AddFieldHandler("FE0353", routine14);
      this.AddFieldHandler("FE0453", routine14);
      Routine routine15 = this.RoutineX(this.calObjs.D1003URLA2020Cal.CalcMilitaryEntitlementIncome + this.calObjs.VERIFCal.CalcMonthlyIncome + this.calObjs.D1003Cal.CalcOtherIncome + this.calObjs.D1003URLA2020Cal.CalcBackEndRatio + this.calObjs.ComortCal.CalcComortgagor + this.calObjs.USDACal.CalcIncomeWorksheet);
      this.AddFieldHandler("BE0065", routine15);
      this.AddFieldHandler("BE0066", routine15);
      this.AddFieldHandler("BE0067", routine15);
      this.AddFieldHandler("BE0068", routine15);
      this.AddFieldHandler("BE0069", routine15);
      this.AddFieldHandler("BE0070", routine15);
      this.AddFieldHandler("BE0071", routine15);
      this.AddFieldHandler("BE0072", routine15);
      this.AddFieldHandler("BE0077", routine15);
      this.AddFieldHandler("CE0065", routine15);
      this.AddFieldHandler("CE0066", routine15);
      this.AddFieldHandler("CE0067", routine15);
      this.AddFieldHandler("CE0068", routine15);
      this.AddFieldHandler("CE0069", routine15);
      this.AddFieldHandler("CE0070", routine15);
      this.AddFieldHandler("CE0071", routine15);
      this.AddFieldHandler("CE0072", routine15);
      this.AddFieldHandler("CE0077", routine15);
      for (int index1 = 1; index1 <= 4; ++index1)
      {
        for (int index2 = 65; index2 <= 72; ++index2)
          this.AddFieldHandler("FE" + index1.ToString("00") + index2.ToString("00"), routine15);
        this.AddFieldHandler("FE" + index1.ToString("00") + "77", routine15);
      }
      this.AddFieldHandler("DISCLOSURE.X1208", this.calObjs.ToolCal.CalcMICancelCondTypeAndRMLA);
      this.AddFieldHandler("DISCLOSURE.X1209", this.calObjs.ToolCal.CalcMICancelCondTypeAndRMLA);
      Routine routine16 = this.RoutineX(this.calObjs.VERIFCal.CalcAppliedToDownpayment + this.calObjs.VERIFCal.CalcAdditionalLoansAmount + this.calObjs.VERIFCal.CalcHelocLineTotal + this.CalcLTV);
      this.AddFieldHandler("URLARAL0020", routine16);
      this.AddFieldHandler("URLARAL0021", routine16);
      this.AddFieldHandler("URLARAL0016", routine16);
      Routine routine17 = this.RoutineX(this.calObjs.VERIFCal.CalcAppliedToDownpayment + this.calObjs.VERIFCal.CalSubFin + this.calObjs.NewHudCal.CalcClosingCost + this.calObjs.GFECal.CalcTotalCosts);
      this.AddFieldHandler("URLARAL0022", routine17);
      this.AddFieldHandler("URLA.X230", routine17);
      this.AddFieldHandler("URLARAL0016", routine17);
      this.AddFieldHandler("URLARAL0017", routine17);
      Routine routine18 = this.RoutineX(this.calObjs.VERIFCal.CalcAdditionalLoansAmount + this.calObjs.VERIFCal.CalcHelocLineTotal + this.CalcLTV + this.calObjs.RegzCal.BuildPaySchedule + this.calObjs.RegzCal.CalculateHELOCPayment + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003URLA2020Cal.CalcTotalMortgageAndTaxes + this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.D1003URLA2020Cal.CalcEstimatedNetMonthlyRent);
      this.AddFieldHandler("URLARAL0017", routine18);
      this.AddFieldHandler("URLARAL0018", routine18);
      this.AddFieldHandler("URLARAL0034", routine18);
      this.AddFieldHandler("URLA.X212", this.calObjs.D1003URLA2020Cal.CalcSupplementalInsuranceAmount + this.calObjs.D1003URLA2020Cal.CalcBackEndRatio + this.calObjs.ComortCal.CalcComortgagor + this.calObjs.D1003Cal.CalcHousingExp);
      this.AddFieldHandler("URLA.X242", this.calObjs.D1003URLA2020Cal.CalculateProductDescription + this.CalcLTV);
      this.AddFieldHandler("URLA.X140", this.CalcLTV);
      this.AddFieldHandler("URLA.X143", this.CalcLTV);
      this.AddFieldHandler("CASASRN.X211", this.CalcLTV);
      Routine routine19 = this.RoutineX(this.calObjs.D1003URLA2020Cal.CalcBackEndRatio + this.calObjs.VERIFCal.CalcLiabilities);
      this.AddFieldHandler("FL0017", routine19);
      this.AddFieldHandler("256", routine19);
      this.AddFieldHandler("1062", routine19);
      this.AddFieldHandler("SYS.X13", routine19);
      this.AddFieldHandler("272", routine19);
      this.AddFieldHandler("VALA.X20", routine19);
      this.AddFieldHandler("VALA.X21", routine19);
      this.AddFieldHandler("VALA.X22", routine19);
      this.AddFieldHandler("VALA.X23", routine19);
      this.AddFieldHandler("VALA.X24", routine19);
      this.AddFieldHandler("VALA.X25", routine19);
      this.AddFieldHandler("VALA.X26", routine19);
      this.AddFieldHandler("VALA.X27", routine19);
      this.AddFieldHandler("VALA.X28", routine19);
      this.AddFieldHandler("VALA.X30", routine19);
      this.AddFieldHandler("HUD1A.X31", routine19);
      this.AddFieldHandler("1835", routine19);
      this.AddFieldHandler("3024", routine19);
      this.AddFieldHandler("19", routine19);
      this.AddFieldHandler("Constr.Refi", routine19);
      this.AddFieldHandler("URLAROL0002", this.RoutineX(this.calObjs.D1003URLA2020Cal.CalcOtherLiabilitiesType + this.calObjs.D1003URLA2020Cal.CalcBackEndRatio + this.calObjs.VERIFCal.CalcLiabilities));
      Routine routine20 = this.RoutineX(this.calObjs.VERIFCal.CalcOtherLiabilityMonthlyIncome + this.calObjs.VERIFCal.CalcLiabilities);
      this.AddFieldHandler("URLAROL0003", this.calObjs.D1003URLA2020Cal.CalcBackEndRatio + routine20);
      this.AddFieldHandler("URLAROL0001", routine20);
      this.AddFieldHandler("3865", new Routine(this.calculateOtherDescription));
      this.AddFieldHandler("3871", new Routine(this.calculateOtherDescription));
      this.AddFieldHandler("4745", this.RoutineX(this.calObjs.D1003Cal.CalcField1109 + this.calObjs.RegzCal.CalculateHELOCPayment + this.calObjs.NewHud2015Cal.CalcLoanEstimateDownPaymentAndFundsForBorrower + this.calObjs.NewHud2015Cal.CalcCDPage3StdCashToCloseFinal + this.calObjs.NewHud2015Cal.Calc2015LineItem904 + this.calObjs.HMDACal.CalcLoanAmount + this.calObjs.D1003URLA2020Cal.UpdateLinkedVoal + this.calObjs.VERIFCal.CalcHelocLineTotal + this.calObjs.NewHudCal.EnforceGFEAppDateRule + this.calObjs.NewHudCal.CalcInitialDisclosureDueDate + this.calObjs.HelocCal.CalcHELOCPeriodicRates + this.calObjs.VACal.CalcVAStatutoryRecoupment + this.calObjs.D1003Cal.CalculatePMIIndicator));
      Routine routine21 = this.RoutineX(this.calObjs.D1003Cal.CalcLoansubAndTSUM + this.calObjs.VERIFCal.CalcLiabilities);
      this.AddFieldHandler("1724", routine21);
      this.AddFieldHandler("1725", routine21);
      Routine calc4506CprintVersion = this.Calc4506CPrintVersion;
      this.AddFieldHandler("IRS4506.X67", calc4506CprintVersion);
      this.AddFieldHandler("IRS4506.X92", calc4506CprintVersion);
      this.AddFieldHandler("4912", this.UpdateIndexRatePrecision);
      this.AddFieldHandler("4913", this.calObjs.HelocCal.CalcHELOCPeriodicRates);
      this.AddFieldHandler("4973", this.calObjs.D1003URLA2020Cal.CalcFirstTimeHomeBuyer + this.calObjs.ULDDExpCal.CalcFannieMaeExportFields);
      this.AddFieldHandler("4974", this.calObjs.D1003URLA2020Cal.CalcFirstTimeHomeBuyer + this.calObjs.ULDDExpCal.CalcFannieMaeExportFields);
      Routine calcAmiLimit = this.CalcAMILimit;
      this.AddFieldHandler("4985", calcAmiLimit);
      this.AddFieldHandler("MORNET.X30", calcAmiLimit);
      this.AddFieldHandler("5027", this.ClearAMILimits + this.PopulateAMILimits + calcAmiLimit);
      this.AddFieldHandler("5028", this.ClearMFILimits + this.PopulateMFILimits);
      this.AddFieldHandler("5002", this.CalcLockinExtensionDays);
      AlertConfigWithDataCompletionFields alertConfig1 = (AlertConfigWithDataCompletionFields) this.loan.Settings.AlertSetupData.GetAlertConfig(72);
      if (alertConfig1 != null && alertConfig1.DataCompletionFields != null)
      {
        foreach (string dataCompletionField in alertConfig1.DataCompletionFields)
          this.AddFieldHandler(dataCompletionField, this.CalcDisclosureReadyDate);
      }
      AlertConfigWithDataCompletionFields alertConfig2 = (AlertConfigWithDataCompletionFields) this.loan.Settings.AlertSetupData.GetAlertConfig(73);
      if (alertConfig2 != null && alertConfig2.DataCompletionFields != null)
      {
        foreach (string dataCompletionField in alertConfig2.DataCompletionFields)
          this.AddFieldHandler(dataCompletionField, this.CalcAtAppDisclosureDate);
      }
      AlertConfigWithDataCompletionFields alertConfig3 = (AlertConfigWithDataCompletionFields) this.loan.Settings.AlertSetupData.GetAlertConfig(47);
      if (alertConfig3 != null && alertConfig3.DataCompletionFields != null)
      {
        foreach (string dataCompletionField in alertConfig3.DataCompletionFields)
          this.AddFieldHandler(dataCompletionField, this.CalcAtLockDisclosureDate);
      }
      AlertConfigWithDataCompletionFields alertConfig4 = (AlertConfigWithDataCompletionFields) this.loan.Settings.AlertSetupData.GetAlertConfig(46);
      if (alertConfig4 != null && alertConfig4.DataCompletionFields != null)
      {
        foreach (string dataCompletionField in alertConfig4.DataCompletionFields)
          this.AddFieldHandler(dataCompletionField, this.CalcChangeCircumstanceRequirementsDate);
      }
      this.AddFieldHandler("CD1.X61", this.CalcChangesReceivedDate + this.calObjs.NewHudCal.CalcRevisedCDDueDate + this.calObjs.NewHud2015Cal.SetReasonForChangeCircumstances);
      this.AddFieldHandler("LE3.X16", new Routine(this.calObjs.RegzCal.CalcPaymentSchedule) + this.calObjs.NewHud2015Cal.CalcLECDDisplayFields);
    }

    private void addFieldValidations()
    {
      this.LE1X9Validation = this.ValidationX(new Validation(this.le1x9Validation));
      this.AddFieldValidation("LE1.X9", this.LE1X9Validation);
      this.AddFieldValidation("LE1.XG9", this.ValidationX(new Validation(this.le1xg9Validation)));
    }

    private void calculateLiabilityAccountName(string id, string val)
    {
      bool flag = false;
      if (Utils.ParseInt((object) id) >= 4000 && Utils.ParseInt((object) id) <= 4009)
        flag = true;
      if (((id == "36" || id == "37" || id == "68" || id == "69" || id == "181" || id.StartsWith("FL") ? 1 : (id.StartsWith("DD") ? 1 : 0)) | (flag ? 1 : 0)) == 0)
        return;
      string str1 = this.Val("36").Trim();
      string str2 = this.Val("37").Trim();
      string str3 = this.Val("68").Trim();
      string str4 = this.Val("69").Trim();
      string str5 = str1 + " " + str2;
      if (str3 != string.Empty || str4 != string.Empty)
      {
        if (str4 == str2)
          str5 = str1 + " & " + str3 + " " + str2;
        else
          str5 = str1 + " " + str2 + " & " + str3 + " " + str4;
      }
      string val1 = str5.Trim();
      int exlcudingAlimonyJobExp = this.loan.GetNumberOfLiabilitesExlcudingAlimonyJobExp();
      for (int index = 1; index <= exlcudingAlimonyJobExp; ++index)
      {
        string str6 = "FL" + index.ToString("00");
        string str7 = this.Val(str6 + "15");
        string str8 = this.Val(str6 + "08").Trim();
        if (str8 != string.Empty && str8 != null)
        {
          switch (str7)
          {
            case "Borrower":
              this.SetVal(str6 + "09", str1 + " " + str2);
              continue;
            case "CoBorrower":
              this.SetVal(str6 + "09", str3 + " " + str4);
              continue;
            case "Both":
              this.SetVal(str6 + "09", val1);
              continue;
            default:
              continue;
          }
        }
        else
          this.SetVal(str6 + "09", "");
      }
      int numberOfDeposits = this.loan.GetNumberOfDeposits();
      for (int index1 = 1; index1 <= numberOfDeposits; ++index1)
      {
        string str9 = "DD" + index1.ToString("00");
        string str10 = this.Val(str9 + "24");
        for (int index2 = 8; index2 <= 20; index2 += 4)
        {
          if (this.Val(str9 + index2.ToString("00")) != string.Empty)
          {
            int num = index2 + 1;
            switch (str10)
            {
              case "Borrower":
                this.SetVal(str9 + num.ToString("00"), str1 + " " + str2);
                continue;
              case "CoBorrower":
                this.SetVal(str9 + num.ToString("00"), str3 + " " + str4);
                continue;
              case "Both":
                this.SetVal(str9 + num.ToString("00"), val1);
                continue;
              default:
                continue;
            }
          }
        }
      }
    }

    private void populateSubjectPropertyAddress(string id, string val)
    {
      string str1 = this.Val("11");
      string str2;
      if (this.Val("12") != string.Empty)
        str2 = str1 + ", " + this.Val("12") + ", " + this.Val("14") + " " + this.Val("15");
      else
        str2 = str1 + (", " + this.Val("12")) + this.Val("14") + " " + this.Val("15");
      string val1 = str2.Trim();
      if (val1 == ",")
        val1 = string.Empty;
      this.SetVal("1603", val1);
    }

    private void calculateHelocDrawAmount(string id, string val)
    {
      double d = this.FltVal("4523") / 100.0 * this.FltVal("4520") - this.FltVal("4521");
      this.SetNum("4525", d < 0.0 ? 0.0 : Math.Floor(d));
    }

    private void calculateHelocLoanAmount(string id, string val)
    {
      double d = this.FltVal("4524") / 100.0 * this.FltVal("4520") - this.FltVal("4522");
      this.SetNum("4526", d < 0.0 ? 0.0 : Math.Floor(d));
    }

    private void calculateValidDrawAmount(string id, string val)
    {
      string str1 = this.Val("4489");
      string str2 = this.Val("4490");
      if (string.IsNullOrEmpty(str1) || string.IsNullOrEmpty(str2) || Utils.ParseDouble((object) str1) <= Utils.ParseDouble((object) str2))
        return;
      this.SetVal("4489", "0.00");
    }

    public void CheckAndAdjustSubjectPropertyValueForConstructionLoans(
      ref double currentPropertyValue)
    {
      string str1 = this.Val("19");
      string str2 = this.Val("1172");
      string str3 = this.Val("1964");
      string str4 = this.Val("Constr.Refi");
      double num1 = this.FltVal("CONST.X58");
      double num2 = this.FltVal("CONST.X59");
      if (!str1.Contains("Construction") || !(str2 == "Conventional"))
        return;
      if (str3 == "Y" || str3 != "Y" && str4 != "Y")
      {
        currentPropertyValue = num1 < num2 ? num1 : num2;
      }
      else
      {
        if (!(str4 == "Y"))
          return;
        currentPropertyValue = num2;
      }
    }

    private void calculateLTV(string id, string val)
    {
      if (Tracing.IsSwitchActive(Calculation.sw, TraceLevel.Info))
        Tracing.Log(Calculation.sw, TraceLevel.Info, nameof (Calculation), "routine: calculateLTV ID: " + id);
      double num1 = this.FltVal("1109");
      double num2 = this.FltVal("356");
      string str1 = this.Val("19");
      string str2 = this.Val("1172");
      bool flag1 = str2 == "FHA" && this.Val("3000") == "Y";
      string doubleValue = "";
      bool flag2 = this.Val("958") != "IRRRL";
      double num3 = !str1.Contains("Construction") ? this.FltVal("136") : this.FltVal("136") + this.FltVal("968");
      double currentPropertyValue;
      if (str2 == "FarmersHomeAdministration")
      {
        num1 = this.FltVal("2");
        currentPropertyValue = num2;
        if (currentPropertyValue == 0.0)
          currentPropertyValue = num3;
      }
      else if (flag1)
      {
        num1 = this.FltVal("MAX23K.X101");
        currentPropertyValue = this.FltVal("MAX23K.X86");
        doubleValue = this.Val("MAX23K.X86");
      }
      else
      {
        double num4 = num2;
        currentPropertyValue = num4 <= num3 || str1.IndexOf("Refinance") >= 0 ? (num4 <= 0.0 ? num3 : num4) : (!(str1 == "Other") || num3 != 0.0 ? num3 : num4);
        if (str1 == "Purchase" && this.Val("1172") == "FHA" && this.Val("3000") == "Y")
          currentPropertyValue = num3 + this.FltVal("967");
      }
      if (currentPropertyValue == 0.0 || str1 == "Purchase" && num2 == 0.0 && !flag1)
      {
        currentPropertyValue = this.FltVal("1821");
        if (currentPropertyValue == 0.0 || currentPropertyValue > num3 && num3 != 0.0)
          currentPropertyValue = num3;
      }
      this.SetCurrentNum("358", currentPropertyValue);
      int num5;
      switch (str2)
      {
        case "Conventional":
label_16:
          num1 = this.FltVal("2");
          goto label_17;
        case "VA":
          num5 = str1.IndexOf("Refinance") > -1 ? 1 : 0;
          break;
        default:
          num5 = 0;
          break;
      }
      int num6 = flag2 ? 1 : 0;
      if ((num5 & num6) != 0)
        goto label_16;
label_17:
      this.CheckAndAdjustSubjectPropertyValueForConstructionLoans(ref currentPropertyValue);
      double num7 = currentPropertyValue;
      if (str2 == "Conventional" && str1 == "Purchase" && this.Val("URLA.X242") == "Y" && this.Val("URLA.X143") == "Affordable LTV")
        num7 = num2 == 0.0 ? this.FltVal("1821") : num2;
      double num8 = currentPropertyValue;
      if (str2 == "Conventional" && !str1.Contains("Construction") && this.Val("URLA.X140") == "LandTrust" && this.Val("1066") == "Leasehold" && this.Val("URLA.X242") == "Y" && this.Val("CASASRN.X211") == "Y")
        num8 = num2 != 0.0 ? num2 : this.FltVal("1821");
      double num9 = 0.0;
      double num10 = 0.0;
      double num11 = 0.0;
      if (currentPropertyValue > 0.0 || num7 > 0.0 || num8 > 0.0)
      {
        if (str2 == "FarmersHomeAdministration" || str2 == "FHA")
        {
          double num12;
          num11 = num12 = this.FltVal("427") + this.FltVal("428") + this.FltVal("CASASRN.X168");
          num10 = num12;
          num9 = num12;
        }
        else if (str2 == "Conventional" || str2 == "VA" || str2 == "HELOC" || str2 == "Other")
        {
          num11 = !(str2 == "VA") ? this.FltVal("427") + this.FltVal("428") + this.FltVal("CASASRN.X167") : this.FltVal("427") + this.FltVal("428") + this.FltVal("CASASRN.X168");
          num9 = this.FltVal("427") + this.FltVal("428") + this.FltVal("CASASRN.X167");
          num10 = this.FltVal("427") + this.FltVal("428") + this.FltVal("CASASRN.X168");
        }
        if (str2 == "Conventional" || str2 == "FarmersHomeAdministration" || ((!(str2 == "VA") ? 0 : (str1.IndexOf("Refinance") > -1 ? 1 : 0)) & (flag2 ? 1 : 0)) != 0)
        {
          num9 += this.FltVal("1045");
          num11 += this.FltVal("1045");
          num10 += this.FltVal("1045");
        }
      }
      if (num7 > 0.0)
      {
        double rate1 = (str2 == "FHA" || str2 == "VA" ? num1 : this.FltVal("2")) / num7 * 100.0;
        this.SetCurrentNum("MORNET.X75", (double) this.formatToInteger(rate1));
        this.SetCurrentNum("ULDD.FNM.MORNET.X75", (double) ULDDExportCalculation.ParseNumeric3((object) rate1));
        double rate2 = num9 / num7 * 100.0;
        this.SetCurrentNum("MORNET.X76", (double) this.formatToInteger(rate2));
        this.SetCurrentNum("ULDD.FNM.MORNET.X76", (double) ULDDExportCalculation.ParseNumeric3((object) rate2));
        double rate3 = num10 / num7 * 100.0;
        this.SetCurrentNum("MORNET.X77", (double) this.formatToInteger(rate3));
        this.SetCurrentNum("ULDD.FNM.MORNET.X77", (double) ULDDExportCalculation.ParseNumeric3((object) rate3));
      }
      else
      {
        this.SetCurrentNum("MORNET.X77", 0.0);
        this.SetCurrentNum("ULDD.FNM.MORNET.X77", (double) ULDDExportCalculation.ParseNumeric3((object) 0));
        this.SetCurrentNum("MORNET.X75", 0.0);
        this.SetCurrentNum("ULDD.FNM.MORNET.X75", (double) ULDDExportCalculation.ParseNumeric3((object) 0));
        this.SetCurrentNum("MORNET.X76", 0.0);
        this.SetCurrentNum("ULDD.FNM.MORNET.X76", (double) ULDDExportCalculation.ParseNumeric3((object) 0));
      }
      if (num8 > 0.0)
      {
        double num13 = (str2 == "FHA" || str2 == "VA" ? num1 : this.FltVal("2")) / num8 * 100.0;
        this.SetNum("CASASRN.X217", num13);
        this.SetNum("ULDD.FRD.CASASRN.X217", num13);
        double num14 = num9 / num8 * 100.0;
        this.SetCurrentNum("CASASRN.X218", num14);
        this.SetCurrentNum("ULDD.FRD.CASASRN.X218", num14);
        double num15 = num10 / num8 * 100.0;
        this.SetCurrentNum("CASASRN.X219", num15);
        this.SetCurrentNum("ULDD.FRD.CASASRN.X219", num15);
      }
      else
      {
        this.SetCurrentNum("CASASRN.X217", 0.0);
        this.SetCurrentNum("ULDD.FRD.CASASRN.X217", 0.0);
        this.SetCurrentNum("CASASRN.X218", 0.0);
        this.SetCurrentNum("ULDD.FRD.CASASRN.X218", 0.0);
        this.SetCurrentNum("CASASRN.X219", 0.0);
        this.SetCurrentNum("ULDD.FRD.CASASRN.X219", 0.0);
      }
      if (currentPropertyValue > 0.0)
      {
        double num16 = num1 / currentPropertyValue * 100.0;
        if (!flag1)
          doubleValue = currentPropertyValue.ToString();
        this.SetNum("353", num16);
        double num17 = 0.0;
        bool flag3 = this.Val("420") == "SecondLien";
        double num18;
        switch (str2)
        {
          case "FarmersHomeAdministration":
            num18 = num17 + (num1 + (flag3 ? this.FltVal("427") : this.FltVal("428")));
            break;
          case "Conventional":
            num18 = num1 + (flag3 ? this.FltVal("427") : this.FltVal("428")) + this.FltVal("CASASRN.X167");
            break;
          case "HELOC":
          case "Other":
            num18 = this.FltVal("427") + this.FltVal("428") + this.FltVal("CASASRN.X167");
            break;
          default:
            num18 = this.FltVal("427") + this.FltVal("428");
            break;
        }
        double num19 = (str2 == "HELOC" || str2 == "Other" ? this.FltVal("427") + this.FltVal("428") + this.FltVal("CASASRN.X167") : (!flag1 ? (!(str2 == "VA") ? (flag3 ? this.FltVal("427") : this.FltVal("428")) + this.FltVal("2") + this.FltVal("CASASRN.X167") : (flag3 ? this.FltVal("427") : this.FltVal("428")) + num1 + this.FltVal("CASASRN.X167")) : (flag3 ? this.FltVal("427") : this.FltVal("428")) + this.FltVal("MAX23K.X101") + this.FltVal("CASASRN.X167"))) / currentPropertyValue * 100.0;
        this.SetCurrentNum("975", num19);
        this.SetCurrentNum("ULDD.FNM.975", (double) ULDDExportCalculation.ParseNumeric3((object) num19));
        this.SetNum("976", num11 / currentPropertyValue * 100.0);
        this.SetNum("1540", num10 / currentPropertyValue * 100.0);
      }
      else
      {
        double num20 = this.FltVal("1821");
        if (num20 > 0.0)
        {
          this.SetNum("353", num1 / num20 * 100.0);
          if (!flag1)
            doubleValue = num20.ToString();
        }
        else
          this.SetNum("353", 0.0);
        this.SetCurrentNum("1540", 0.0);
        this.SetCurrentNum("976", 0.0);
        this.SetCurrentNum("975", 0.0);
        this.SetCurrentNum("ULDD.FNM.975", (double) ULDDExportCalculation.ParseNumeric3((object) 0));
      }
      string str3 = this.Val("1393");
      if (str3 == "Application withdrawn" || str3 == "File Closed for incompleteness" || this.Val("HMDA.X108") == "Y")
        this.SetVal("HMDA.X85", "NA");
      else
        this.SetVal("HMDA.X85", Utils.ArithmeticRounding(Utils.ToDouble(doubleValue), 0).ToString());
      this.calObjs.CloserCal.calcModifiedTerms(id, val);
      this.calObjs.VACal.CalcVACashOutRefinance(id, val);
    }

    private void calculatePiggyBackFields(string id, string val)
    {
      if (!(id == "4493") || this.USEURLA2020)
        return;
      if (this.loan.LinkSyncType == LinkSyncType.PiggybackLinked && this.loan.LinkedData != null && !this.loan.LinkedData.IsLocked("140"))
      {
        this.loan.LinkedData.SetField("140", val);
      }
      else
      {
        if (this.loan.LinkSyncType != LinkSyncType.PiggybackPrimary || this.loan.LinkedData == null || !(this.loan.LinkedData.GetField("1172") == "HELOC") || this.loan.IsLocked("140"))
          return;
        this.SetVal("140", this.loan.LinkedData.GetField("4493"));
      }
    }

    private int formatToInteger(double rate)
    {
      rate -= 0.005;
      rate = Utils.ArithmeticRounding(rate, 2);
      rate += 0.49;
      rate = Utils.ArithmeticRounding(rate, 0);
      return Utils.ParseInt((object) rate.ToString());
    }

    private void calculateLifeInsurance(string id, string val)
    {
      switch (id)
      {
        case "1558":
          double num1 = this.ToDouble(val);
          if (num1 == 0.0)
          {
            this.SetVal("1559", val);
            break;
          }
          this.SetVal("1559", (this.FltVal("5") + num1).ToString("N2"));
          break;
        case "5":
          double num2 = this.ToDouble(this.Val("1558"));
          if (num2 == 0.0)
          {
            this.SetVal("1559", this.Val("1558"));
            break;
          }
          this.SetVal("1559", (this.FltVal("5") + num2).ToString("N2"));
          break;
      }
    }

    private void calculateLockinExtensionDays(string id, string val)
    {
      if ("5002" != id)
        return;
      int num = 0;
      try
      {
        if (!string.IsNullOrEmpty(val))
          num = Convert.ToInt32(double.Parse(val));
      }
      catch (Exception ex)
      {
      }
      if (num >= 0)
        return;
      this.SetVal(id, string.Empty);
    }

    private void calculateLockDate(string id, string val)
    {
      DateTime lockDate = DateTime.MaxValue;
      DateTime rawExpireDate = DateTime.MaxValue;
      LockExpDayCountSetting policySetting = (LockExpDayCountSetting) this.sessionObjects.StartupInfo.PolicySettings[(object) "Policies.LockExpDayCount"];
      string id1 = "762";
      string id2 = "432";
      string id3 = "761";
      if ((id == id1 || id == id2 || id == id3) && this.Val("3941") == "Y")
        return;
      if (id == "2089" || id == "2090" || id == "2091")
      {
        id1 = "2091";
        id2 = "2090";
        id3 = "2089";
      }
      string s1 = this.Val(id1);
      string s2 = this.Val(id3);
      int num1 = this.IntVal(id2);
      if (s2 == "//")
        s2 = string.Empty;
      if (s1 == "//")
        s1 = string.Empty;
      if (s2 == string.Empty && id == id3)
      {
        this.SetVal(id2, "");
        this.SetVal(id1, "");
      }
      else
      {
        DateTime dateTime1;
        try
        {
          if (s2 != string.Empty)
          {
            dateTime1 = DateTime.Parse(s2);
            lockDate = dateTime1.Date;
          }
        }
        catch (Exception ex)
        {
          return;
        }
        try
        {
          if (s1 != string.Empty)
          {
            dateTime1 = DateTime.Parse(s1);
            rawExpireDate = dateTime1.Date;
          }
        }
        catch (Exception ex)
        {
          return;
        }
        bool ignoreSettings = this.Val("3915") != "" && !this.Val("3911").Contains("Individual");
        if (id == id2 || id == id3 && num1 != 0)
        {
          if (num1 > 0)
          {
            if (num1 > 0)
              --num1;
            if (num1 > 9999)
              num1 = 9999;
            if (s2 == string.Empty && s1 != string.Empty && s1 != "//")
            {
              DateTime dateTime2 = rawExpireDate.AddDays((double) (num1 * -1));
              if (policySetting == LockExpDayCountSetting.OneDayAfter)
                dateTime2 = dateTime2.AddDays(-1.0);
              this.SetVal(id3, dateTime2.ToString("MM/dd/yy"));
            }
            else
            {
              if (!(lockDate != DateTime.MaxValue))
                return;
              DateTime lockExpirationDate = this.calObjs.LockRequestCal.CalculateLockExpirationDate(lockDate, this.IntVal(id2), ignoreSettings);
              this.SetVal(id1, lockExpirationDate.ToString("MM/dd/yy"));
            }
          }
          else
          {
            this.SetVal(id1, string.Empty);
            this.SetVal(id2, string.Empty);
          }
        }
        else
        {
          if (!(id == id1) && (!(id == id3) || num1 != 0))
            return;
          if (s1 != string.Empty)
          {
            if (!ignoreSettings)
              rawExpireDate = this.calObjs.LockRequestCal.GetNextClosestLockExpirationDate(rawExpireDate);
            this.SetVal(id1, rawExpireDate.ToString());
            if (s2 == string.Empty && num1 != 0)
            {
              DateTime dateTime3 = rawExpireDate.AddDays((double) (num1 * -1));
              if (policySetting == LockExpDayCountSetting.OneDayAfter)
                dateTime3 = dateTime3.AddDays(-1.0);
              this.SetVal(id3, dateTime3.ToString("MM/dd/yy"));
            }
            else
            {
              if (!(s2 != string.Empty))
                return;
              int num2 = (rawExpireDate - lockDate).Days;
              if (policySetting == LockExpDayCountSetting.OnTheDay)
                ++num2;
              if (num2 > 9999)
                num2 = 9999;
              if (num2 > 0)
                this.SetVal(id2, num2.ToString());
              else
                this.SetVal(id2, "");
            }
          }
          else
            this.SetVal(id2, "");
        }
      }
    }

    internal void PrefixCalculations(string version)
    {
      if (!(version == "3.1"))
        return;
      this.calObjs.VERIFCal.CalcLiabilities((string) null, (string) null);
      this.calObjs.D1003Cal.CalcLoansubAndTSUM((string) null, (string) null);
    }

    internal void ImportCalculation(bool copyUnpaid)
    {
      if (copyUnpaid)
        this.copyUnpaidVOLBalance();
      this.ImportCalculation();
    }

    internal void ImportCalculation() => this.CalculateAll("IMPORT");

    internal void CalculateAll(bool copyUnpaid)
    {
      if (copyUnpaid)
        this.copyUnpaidVOLBalance();
      this.CalculateAll();
    }

    internal void CalculateAll() => this.CalculateAll((string) null);

    private void copyUnpaidVOLBalance()
    {
      int exlcudingAlimonyJobExp = this.loan.GetNumberOfLiabilitesExlcudingAlimonyJobExp();
      string empty = string.Empty;
      for (int index = 1; index <= exlcudingAlimonyJobExp; ++index)
      {
        string str = "FL" + index.ToString("00");
        if (this.Val(str + "18") == "Y")
        {
          double num = this.FltVal(str + "13");
          this.SetCurrentNum(str + "16", num);
        }
      }
    }

    internal void CalculateAll(string id)
    {
      long num1 = 0;
      DateTime now;
      if (Tracing.Debug)
      {
        now = DateTime.Now;
        num1 = now.Ticks;
        Tracing.Log(Calculation.sw, TraceLevel.Info, nameof (Calculation), "** Starting Ticks: " + num1.ToString());
        Tracing.Log(Calculation.sw, TraceLevel.Info, nameof (Calculation), "** routine: CalculateAll");
      }
      if (EnConfigurationSettings.GlobalSettings.AllowCalculationDiagnostic && this.loan.Calculator != null)
        this.loan.Calculator.ResetTriggerCounter();
      this.calObjs.NewHudCal.IsPOCPTCVerified = false;
      bool performanceEnabled = this.calObjs.RegzCal.PerformanceEnabled;
      this.calObjs.RegzCal.PerformanceEnabled = false;
      this.calObjs.ToolCal.UpdateExternalLateFeeSettingFields();
      this.calObjs.HMDACal.UpdateEBSHiddenFields();
      string str = this.Val("1172");
      this.calObjs.D1003Cal.CalcLienPosition(id, (string) null);
      this.calObjs.PreCal.UpdateF19("19", this.Val("19"));
      this.calObjs.NewHud2015Cal.SetCustomizedProjectedPaymentTable(id, (string) null);
      this.calObjs.D1003URLA2020Cal.CalcURLALoanPurposeMapping(id, (string) null);
      this.calObjs.NewHud2015Cal.CalcDisclosedSalesPrice("", "");
      this.calObjs.RegzCal.CalcFloorRate(id, (string) null);
      this.calObjs.PrequalCal.CalcMIP("IMPORT", (string) null);
      if (!this.CalculationObjects.SkipLockRequestSync)
        this.calObjs.PrequalCal.ImportDownPayment();
      this.calObjs.D1003Cal.CopyOtherIncome((string) null, (string) null);
      int numberOfEmployer1 = this.loan.GetNumberOfEmployer(true);
      for (int index = 1; index <= numberOfEmployer1; ++index)
      {
        string id1 = "BE" + index.ToString("00") + "12";
        this.calObjs.D1003URLA2020Cal.CalcMilitaryEntitlementIncome(id1, (string) null);
        this.calObjs.VERIFCal.CalcMonthlyIncome(id1, (string) null);
      }
      int numberOfEmployer2 = this.loan.GetNumberOfEmployer(false);
      for (int index = 1; index <= numberOfEmployer2; ++index)
      {
        string id2 = "CE" + index.ToString("00") + "12";
        this.calObjs.D1003URLA2020Cal.CalcMilitaryEntitlementIncome(id2, (string) null);
        this.calObjs.VERIFCal.CalcMonthlyIncome(id2, (string) null);
      }
      int numberOfMortgages = this.loan.GetNumberOfMortgages();
      for (int index = 1; index <= numberOfMortgages; ++index)
        this.calObjs.VERIFCal.CalculateVOM("FM" + index.ToString("00") + "20", "IMPORT");
      int numberOfDeposits = this.loan.GetNumberOfDeposits();
      for (int index = 1; index <= numberOfDeposits; ++index)
        this.calObjs.VERIFCal.CalculateDeposits("DD" + index.ToString("00") + "34", (string) null);
      this.calObjs.VERIFCal.TotalDeposits(string.Empty, string.Empty);
      this.calObjs.D1003URLA2020Cal.CalcProposedSupplementalPropertyInsurance((string) null, (string) null);
      this.calObjs.D1003URLA2020Cal.CalcSupplementalInsuranceAmount((string) null, (string) null);
      this.calObjs.D1003Cal.CalcHousingExp("IMPORT", (string) null);
      this.calObjs.D1003URLA2020Cal.CalcTotalMortgageAndTaxes((string) null, (string) null);
      this.calObjs.D1003Cal.CopyBrokerInfoToLender((string) null, (string) null);
      this.calObjs.D1003Cal.CalcIntrOnly((string) null, (string) null);
      if (id != "IMPORT")
        this.calObjs.GFECal.PopulateFeeList(false);
      this.calObjs.ToolCal.CalcLOCompensation((string) null, (string) null);
      this.calObjs.GFECal.CalcOthers((string) null, (string) null);
      this.calObjs.GFECal.FormCal("IMPORT", "IMPORT", (string) null);
      this.calObjs.GFECal.CalcClosingCosts((string) null, (string) null);
      if (!this.UseNewGFEHUD && !this.UseNew2015GFEHUD)
      {
        if (id == "IMPORT")
        {
          this.calObjs.GFECal.SyncImportedMLDS();
        }
        else
        {
          for (int index = 0; index < CalculationBase.BorrowerFields.Length; ++index)
            this.calObjs.GFECal.SyncMLDSField(CalculationBase.BorrowerFields[index], (string) null);
        }
      }
      this.calObjs.D1003Cal.PopulateLoanAmountToNmlsApplicationAmounts(this.calObjs.D1003Cal.InitialApplicaitonDateID, this.GetFieldFromCal(this.calObjs.D1003Cal.InitialApplicaitonDateID));
      this.calObjs.D1003Cal.CalcHousingExp("IMPORT", (string) null);
      this.calObjs.D1003URLA2020Cal.CalcTotalMortgageAndTaxes((string) null, (string) null);
      this.calObjs.PrequalCal.CalcMIP("IMPORT", (string) null);
      this.calObjs.GFECal.CalcTotalCosts((string) null, (string) null);
      this.calObjs.D1003URLA2020Cal.CalcOtherCredits((string) null, (string) null);
      this.calObjs.GFECal.CalcBrokerLenderFeeTotals((string) null, (string) null);
      this.calObjs.D1003Cal.AdjConstTotal((string) null, (string) null);
      this.calObjs.PrequalCal.CalcRentOwn((string) null, (string) null);
      if (str == "FHA" || str == "FarmersHomeAdministration")
        this.calObjs.FHACal.CalcMACAWP((string) null, (string) null);
      if (str == "VA")
        this.calObjs.VACal.CalcVALA((string) null, (string) null);
      this.calObjs.LoansubCal.CalcLoansub((string) null, (string) null);
      this.calObjs.Hud1Cal.CalcEscrowFirstPaymentDate((string) null, (string) null);
      this.calObjs.Hud1Cal.CalcHUD1ES("IMPORT", (string) null);
      this.calObjs.NewHudCal.CalcInterestRateChange((string) null, (string) null);
      this.calObjs.RegzCal.CalcAPR((string) null, (string) null);
      this.CalcLTV((string) null, (string) null);
      this.calObjs.ToolCal.CalcRateLock((string) null, (string) null);
      this.calObjs.FM1084Cal.FormCal();
      this.calObjs.PrequalCal.CalcMinMax((string) null, (string) null);
      this.calObjs.PrequalCal.GetQualificationDetail(0, false);
      this.calObjs.PrequalCal.GetQualificationDetail(1, false);
      this.calObjs.PrequalCal.GetQualificationDetail(2, false);
      if (this.Val("1481") == "Y")
        this.SetVal("305", this.Val("364"));
      if (!this.CalculationObjects.SkipLockRequestSync)
        this.calObjs.D1003Cal.PopulateOtherFields("IMPORT", (string) null);
      this.calObjs.NewHudCal.CalcGFEApplicationDate((string) null, (string) null);
      this.calObjs.NewHudCal.FormCal((string) null, (string) null);
      this.calObjs.NewHudCal.CalTradeOffTable((string) null, (string) null);
      this.calObjs.RegzCal.CalcFirstAdjustmentMinimum((string) null, (string) null);
      this.calObjs.RegzCal.CalcIRS1098((string) null, (string) null);
      this.calObjs.D1003Cal.CopyCountyToJurisdiction((string) null, (string) null);
      this.calObjs.ToolCal.CalcSafeHarbor((string) null, (string) null);
      this.calObjs.D1003Cal.RecalculateNMLS();
      this.calObjs.Hud1Cal.CalcBiWeeklyEscrowPayment((string) null, (string) null);
      this.calObjs.USDACal.CalcIncomeWorksheet((string) null, (string) null);
      this.calObjs.USDACal.CalcFirstTimeHomeBuyer((string) null, (string) null);
      if (!this.CalculationObjects.SkipLockRequestSync)
        this.calObjs.ToolCal.UpdatePurchaseAdviceBalance((string) null, (string) null);
      this.calObjs.ToolCal.CalcCorrespondentLateFee((string) null, (string) null);
      if (!this.CalculationObjects.SkipLockRequestSync)
        this.calObjs.ULDDExpCal.CalculateAll();
      this.calObjs.ATRQMCal.FormCal((string) null, (string) null);
      if (this.Val("1868").Trim() == "")
        this.calObjs.Cal.UpdateBorrowerVestingName((string) null, (string) null);
      if (this.Val("1873").Trim() == "")
        this.calObjs.Cal.UpdateCoborrowerVestingName((string) null, (string) null);
      new UCDXmlExporter(this.loan).TriggerCalculation();
      this.calObjs.NewHud2015Cal.CalcLoanTermTable();
      this.calObjs.NewHud2015Cal.CalcEstimatedCashToCloseTable();
      this.calObjs.NewHud2015Cal.CalcLoanTotalPayments();
      this.calObjs.NewHud2015Cal.CalcCDPage3Transactions();
      this.calObjs.NewHud2015Cal.CalculateProductDescription((string) null, (string) null);
      this.calObjs.NewHud2015Cal.CalcConstructionPermPeriod((string) null, (string) null);
      this.calObjs.NewHud2015Cal.SetAIRTableIndexType((string) null, (string) null);
      this.calObjs.NewHud2015FeeDetailCal.calculate_LastDisclosedCD("", "");
      this.calObjs.NewHud2015FeeDetailCal.calculate_LastDisclosedLE("", "");
      this.calObjs.NewHud2015FeeDetailCal.calculate_EarliestSafeHarbor("", "");
      this.calObjs.NewHud2015FeeDetailCal.calculate_EarliestSSPL("", "");
      this.calObjs.NewHud2015Cal.CopyLoanNumToLoanIDNum("", "");
      this.calObjs.NewHud2015Cal.CalcSellerNames((string) null, (string) null);
      this.calObjs.NewHud2015Cal.SetReasonForChangeCircumstances("3169", (string) null);
      this.calObjs.NewHud2015Cal.SetReasonForChangeCircumstances("CD1.X64", (string) null);
      this.calObjs.NewHud2015Cal.CalcMICReference((string) null, (string) null);
      this.calObjs.NewHud2015Cal.CalcCdPage3Liabilities("", "");
      this.calObjs.NewHud2015Cal.CalcCDPage3AlternateCashToCloseLoanEstimate((string) null, (string) null);
      this.calObjs.NewHud2015Cal.CalcTotalAdjustmentAndOtherCredits((string) null, (string) null);
      this.calObjs.NewHud2015Cal.CalcCDPage3StdCashToCloseLoanEstimate((string) null, (string) null);
      this.calObjs.NewHud2015Cal.CalcLoanEstimateThirdPartyPaymentsNotOtherwiseDisclosed((string) null, (string) null);
      this.calObjs.NewHud2015Cal.CalcCDPage3StdCashToCloseFinal((string) null, (string) null);
      this.calObjs.NewHud2015Cal.CalcCDPage3AlternateCashToCloseFinal((string) null, (string) null);
      this.calObjs.NewHud2015Cal.Calc2015LineItem904((string) null, (string) null);
      this.calObjs.NewHud2015Cal.Calc60Payments((string) null, (string) null);
      this.calObjs.RegzCal.CalcConstructionPhaseDisclosedSeparately((string) null, (string) null);
      this.calObjs.ToolCal.CalcValueForIntrestRateConditions((string) null, (string) null);
      this.calObjs.ToolCal.UpdateDisclosureDeterminedField((string) null, (string) null);
      this.calObjs.ToolCal.UpdateDisclosureExplanationFields((string) null, (string) null);
      this.calObjs.ToolCal.UpdateSysIdGuid((string) null, (string) null);
      this.calObjs.RegzCal.CalcInitialInterestRate((string) null, (string) null);
      this.CalcIsLSSecondaryFile((string) null, (string) null);
      this.CalcLoanLinkSyncType((string) null, (string) null);
      this.calObjs.NewHud2015Cal.Calc2015LineItem904((string) null, (string) null);
      if (!this.calObjs.SkipLinkedSync)
        this.calObjs.ToolCal.SyncConstructionLinkSyncDefaultData((string) null, (string) null);
      this.calObjs.HMDACal.CalcSortEthnicityCategory("IMPORT", (string) null);
      this.calObjs.HMDACal.CalcSortRaceCategory("IMPORT", (string) null);
      if (this.loan.Settings != null && this.loan.Settings.HMDAInfo != null && (this.loan.Settings.HMDAInfo.HMDAApplicationDate ?? "") != "")
        this.calObjs.D1003Cal.CopyCitizenshipAndAge(this.loan.Settings.HMDAInfo.HMDAApplicationDate, this.Val(this.loan.Settings.HMDAInfo.HMDAApplicationDate));
      this.calObjs.HMDACal.CalcAll((string) null, (string) null);
      this.calObjs.HMDACal.CalcSortEthnicityCategory("IMPORT", (string) null);
      this.calObjs.HMDACal.CalcSortRaceCategory("IMPORT", (string) null);
      this.calObjs.D1003URLA2020Cal.CalcAll((string) null, (string) null);
      this.calObjs.FHACal.CalcExisting23KDebt((string) null, (string) null);
      if (id == "IMPORT" && this.Val("1825") == "2020")
        this.calObjs.D1003URLA2020Cal.MISMOImport((string) null, (string) null);
      this.calObjs.VACal.CalcVAStatutoryRecoupment((string) null, (string) null);
      this.calObjs.CustomCal.CalculateAll();
      this.calObjs.BillingCal.CalculateAll();
      this.SetCurrentNum("4460", (double) this.loan.GetBorrowerPairs().Length);
      this.calObjs.NewHud2015Cal.CalcLECDDisplayFields("IMPORT", (string) null);
      this.calObjs.RegzCal.PerformanceEnabled = performanceEnabled;
      if (EnConfigurationSettings.GlobalSettings.AllowCalculationDiagnostic && this.loan.Calculator != null)
        Tracing.Log(true, nameof (Calculation), "Calculation Diagnostic", "Calculation Trigger Counts - CalculateAll()\r\n" + this.loan.Calculator.DumpTriggerCounter());
      this.calObjs.PrequalCal.CalcOnSaveCopyBorrowerLenderPaid("4463", this.Val("LCP.x1"));
      this.calObjs.ToolCal.CalcStatementOfDenial((string) null, (string) null);
      this.calObjs.ToolCal.CalcMICancelCondTypeAndRMLA((string) null, (string) null);
      this.calObjs.RegzCal.CalcARMDisclosureSample((string) null, (string) null);
      if (id == "IMPORT")
        this.calObjs.ToolCal.CalcPrepaymentBalance("682", (string) null);
      this.calObjs.Cal.CalcDisclosureReadyDate((string) null, (string) null);
      this.calObjs.Cal.CalcAtAppDisclosureDate((string) null, (string) null);
      this.calObjs.Cal.CalcAtLockDisclosureDate((string) null, (string) null);
      this.calObjs.Cal.CalcChangeCircumstanceRequirementsDate((string) null, (string) null);
      if (!Tracing.Debug)
        return;
      now = DateTime.Now;
      long ticks = now.Ticks;
      Tracing.Log(Calculation.sw, TraceLevel.Info, nameof (Calculation), "** Ending Ticks: " + ticks.ToString());
      long num2 = ticks - num1;
      Tracing.Log(Calculation.sw, TraceLevel.Info, nameof (Calculation), "** Tick Counts: " + num2.ToString());
      double num3 = (double) num2 / 10000000.0;
      Tracing.Log(Calculation.sw, TraceLevel.Error, nameof (Calculation), "** Elapsed: " + num3.ToString() + " second");
      Tracing.Log(Calculation.sw, TraceLevel.Error, nameof (Calculation), "** End of CalculateAll **");
    }

    internal void CalculateTPO()
    {
      long num1 = 0;
      DateTime now;
      if (Tracing.IsSwitchActive(Calculation.sw, TraceLevel.Info))
      {
        now = DateTime.Now;
        num1 = now.Ticks;
        Tracing.Log(Calculation.sw, TraceLevel.Error, nameof (Calculation), "** Starting Ticks: " + num1.ToString());
        Tracing.Log(Calculation.sw, TraceLevel.Error, nameof (Calculation), "** routine: CalculateTPO");
      }
      this.calObjs.NewHudCal.IsPOCPTCVerified = false;
      bool performanceEnabled = this.calObjs.RegzCal.PerformanceEnabled;
      string str = this.Val("1172");
      this.calObjs.RegzCal.PerformanceEnabled = true;
      this.calObjs.RegzCal.CalcFloorRate((string) null, (string) null);
      this.calObjs.D1003Cal.CopyInfoToLockRequest("TPO", (string) null);
      this.calObjs.PrequalCal.CalcMIP("IMPORT", (string) null);
      this.calObjs.PrequalCal.ImportDownPayment();
      this.calObjs.D1003Cal.CopyOtherIncome((string) null, (string) null);
      int numberOfEmployer1 = this.loan.GetNumberOfEmployer(true);
      for (int index = 1; index <= numberOfEmployer1; ++index)
        this.calObjs.VERIFCal.CalcMonthlyIncome("BE" + index.ToString("00") + "12", (string) null);
      int numberOfEmployer2 = this.loan.GetNumberOfEmployer(false);
      for (int index = 1; index <= numberOfEmployer2; ++index)
        this.calObjs.VERIFCal.CalcMonthlyIncome("CE" + index.ToString("00") + "12", (string) null);
      int numberOfMortgages = this.loan.GetNumberOfMortgages();
      for (int index = 1; index <= numberOfMortgages; ++index)
        this.calObjs.VERIFCal.CalculateVOM("FM" + index.ToString("00") + "20", "IMPORT");
      int numberOfDeposits = this.loan.GetNumberOfDeposits();
      for (int index = 1; index <= numberOfDeposits; ++index)
        this.calObjs.VERIFCal.CalculateDeposits("DD" + index.ToString("00") + "34", (string) null);
      this.calObjs.VERIFCal.TotalDeposits(string.Empty, string.Empty);
      this.calObjs.D1003Cal.CalcHousingExp("IMPORT", (string) null);
      this.calObjs.D1003URLA2020Cal.CalcTotalMortgageAndTaxes((string) null, (string) null);
      this.calObjs.D1003Cal.CopyBrokerInfoToLender((string) null, (string) null);
      this.calObjs.GFECal.FormCal("TPO", "TPO", (string) null);
      this.calObjs.D1003Cal.CalcHousingExp("IMPORT", (string) null);
      this.calObjs.D1003URLA2020Cal.CalcTotalMortgageAndTaxes((string) null, (string) null);
      this.calObjs.GFECal.CalcTotalCosts((string) null, (string) null);
      this.calObjs.D1003URLA2020Cal.CalcOtherCredits((string) null, (string) null);
      this.calObjs.D1003Cal.AdjConstTotal((string) null, (string) null);
      if (str == "FHA" || str == "FarmersHomeAdministration")
        this.calObjs.FHACal.CalcMACAWP((string) null, (string) null);
      if (str == "VA")
        this.calObjs.VACal.CalcVALA((string) null, (string) null);
      this.calObjs.LoansubCal.CalcLoansub((string) null, (string) null);
      this.calObjs.Hud1Cal.CalcHUD1ES("IMPORT", (string) null);
      if (this.Val("1481") == "Y")
        this.SetVal("305", this.Val("364"));
      this.calObjs.D1003Cal.PopulateOtherFields("IMPORT", (string) null);
      this.calObjs.NewHudCal.CalcGFEApplicationDate((string) null, (string) null);
      this.calObjs.RegzCal.PerformanceEnabled = false;
      this.calObjs.NewHudCal.FormCal("TPO", (string) null);
      this.CalcLTV("TPO", (string) null);
      this.calObjs.ToolCal.CalcRateLock((string) null, (string) null);
      this.calObjs.PrequalCal.CalcLockRequestLoan((string) null, (string) null);
      this.calObjs.VACal.CalcVAStatutoryRecoupment((string) null, (string) null);
      this.calObjs.CustomCal.CalculateAll();
      this.calObjs.RegzCal.PerformanceEnabled = performanceEnabled;
      if (!Tracing.IsSwitchActive(Calculation.sw, TraceLevel.Info))
        return;
      now = DateTime.Now;
      long ticks = now.Ticks;
      Tracing.Log(Calculation.sw, TraceLevel.Info, nameof (Calculation), "** Ending Ticks: " + ticks.ToString());
      long num2 = ticks - num1;
      Tracing.Log(Calculation.sw, TraceLevel.Info, nameof (Calculation), "** Tick Counts: " + num2.ToString());
      double num3 = (double) num2 / 10000000.0;
      Tracing.Log(Calculation.sw, TraceLevel.Error, nameof (Calculation), "** Elapsed: " + num3.ToString() + " second");
      Tracing.Log(Calculation.sw, TraceLevel.Error, nameof (Calculation), "** End of CalculateTPO **");
    }

    private void calculateUCD(string id, string val)
    {
      new UCDXmlExporter(this.loan).TriggerCalculation();
    }

    private void calculateMortgageInsurance(string id, string val)
    {
      double num1 = 0.0;
      switch (this.Val("1757"))
      {
        case "Loan Amount":
          num1 = this.FltVal("2");
          break;
        case "Purchase Price":
          num1 = this.FltVal("136");
          break;
        case "Appraisal Value":
          num1 = this.FltVal("356");
          break;
        case "Base Loan Amount":
          num1 = this.FltVal("1109");
          break;
      }
      double num2 = this.FltVal("1766");
      double num3 = Utils.ArithmeticRounding(this.FltVal("1199") / 100.0 * num1 / 12.0, 2);
      if (this.Val("1172") != "Conventional" && this.Val("1775") == "Y")
        num3 = num2;
      else if (num3 != 0.0)
        this.SetCurrentNum("1766", num3);
      if (num3 != 0.0)
      {
        if (this.Val("1172") == "FarmersHomeAdministration")
        {
          if (this.FltVal("232") != 0.0)
            this.calObjs.USDACal.CopyPOCPTCAPRFromLine1003ToLine1010(id, val);
          this.SetVal("NEWHUD.X1707", num3.ToString("N2"));
        }
        else
        {
          this.SetVal("232", num3.ToString("N2"));
          this.calObjs.ULDDExpCal.CalcFannieMaeExportFields("232", this.Val("232"));
        }
      }
      else
      {
        this.SetVal("232", "");
        this.calObjs.ULDDExpCal.CalcFannieMaeExportFields("232", "");
      }
      this.calObjs.ToolCal.CalcCPAEscrowDetails(id, val);
      this.SetCurrentNum("1770", Utils.ArithmeticRounding(this.FltVal("1201") / 100.0 * num1 / 12.0, 2));
      this.calObjs.D1003Cal.RecalculateNMLS();
      this.calObjs.NewHud2015Cal.CalcMIEscrowIndicator(id, val);
    }

    private void calculateOthers(string id, string val)
    {
      string empty1 = string.Empty;
      if (id == "14" || id == "15")
      {
        string upper = val.ToUpper();
        if (id == "15")
          upper = this.Val("14").ToUpper();
        string val1 = string.Empty;
        if (upper == "AL")
          val1 = "01";
        if (upper == "AK")
          val1 = "02";
        if (upper == "AR")
          val1 = "05";
        if (upper == "AS")
          val1 = "60";
        if (upper == "AZ")
          val1 = "04";
        if (upper == "CA")
          val1 = "06";
        if (upper == "CO")
          val1 = "08";
        if (upper == "CT")
          val1 = "09";
        if (upper == "DC")
          val1 = "11";
        if (upper == "DE")
          val1 = "10";
        if (upper == "FL")
          val1 = "12";
        if (upper == "GA")
          val1 = "13";
        if (upper == "GU")
          val1 = "66";
        if (upper == "HI")
          val1 = "15";
        if (upper == "IA")
          val1 = "19";
        if (upper == "ID")
          val1 = "16";
        if (upper == "IL")
          val1 = "17";
        if (upper == "IN")
          val1 = "18";
        if (upper == "KS")
          val1 = "20";
        if (upper == "KY")
          val1 = "21";
        if (upper == "LA")
          val1 = "22";
        if (upper == "MA")
          val1 = "25";
        if (upper == "MD")
          val1 = "24";
        if (upper == "ME")
          val1 = "23";
        if (upper == "MI")
          val1 = "26";
        if (upper == "MN")
          val1 = "27";
        if (upper == "MO")
          val1 = "29";
        if (upper == "MS")
          val1 = "28";
        if (upper == "MT")
          val1 = "30";
        if (upper == "NC")
          val1 = "37";
        if (upper == "ND")
          val1 = "38";
        if (upper == "NE")
          val1 = "31";
        if (upper == "NH")
          val1 = "33";
        if (upper == "NJ")
          val1 = "34";
        if (upper == "NM")
          val1 = "35";
        if (upper == "NV")
          val1 = "32";
        if (upper == "NY")
          val1 = "36";
        if (upper == "OH")
          val1 = "39";
        if (upper == "OK")
          val1 = "40";
        if (upper == "OR")
          val1 = "41";
        if (upper == "PA")
          val1 = "42";
        if (upper == "PR")
          val1 = "72";
        if (upper == "RI")
          val1 = "44";
        if (upper == "SC")
          val1 = "45";
        if (upper == "SD")
          val1 = "46";
        if (upper == "TN")
          val1 = "47";
        if (upper == "TX")
          val1 = "48";
        if (upper == "UT")
          val1 = "49";
        if (upper == "VA")
          val1 = "51";
        if (upper == "VI")
          val1 = "78";
        if (upper == "VT")
          val1 = "50";
        if (upper == "WA")
          val1 = "53";
        if (upper == "WI")
          val1 = "55";
        if (upper == "WV")
          val1 = "54";
        if (upper == "WY")
          val1 = "56";
        if (!string.IsNullOrEmpty(val1))
          this.SetVal("1395", val1);
        if (id == "15")
        {
          string str = this.Val("2851");
          if (str == "" || str == "//")
          {
            if (this.Val("15") != string.Empty)
              this.SetVal("2851", DateTime.Today.ToString("MM/dd/yyyy"));
            else
              this.SetVal("2851", "");
          }
          else if (this.Val("15") == string.Empty)
            this.SetVal("2851", "");
        }
      }
      int num1 = this.IntVal("IRS4506.X31");
      if (num1 > 0)
      {
        double num2 = this.FltVal("IRS4506.X52");
        if (num2 == 0.0)
        {
          num2 = 50.0;
          this.SetCurrentNum("IRS4506.X52", num2);
        }
        this.SetCurrentNum("IRS4506.X32", Utils.ArithmeticRounding(num2 * (double) num1, 2));
      }
      else
        this.SetVal("IRS4506.X32", "");
      string empty2 = string.Empty;
      int numberOfTaX4506Ts = this.loan.GetNumberOfTAX4506Ts(true);
      for (int index = 1; index <= numberOfTaX4506Ts; ++index)
      {
        string str = index.ToString("00");
        double num3 = this.FltVal("AR" + str + "52");
        if (num3 == 0.0)
        {
          num3 = 50.0;
          this.SetCurrentNum("AR" + str + "52", num3);
        }
        this.SetCurrentNum("AR" + str + "32", num3 * (double) this.IntVal("AR" + str + "31"));
      }
      if (id.Length == 6 && id.StartsWith("IR") && (id.EndsWith("93") || id.EndsWith("01")))
      {
        if (id.EndsWith("93") && this.Val(id) == "8821" && this.Val(id.Substring(0, 4) + "01") == "Both")
          this.SetVal(id.Substring(0, 4) + "01", "Borrower");
        else if (id.EndsWith("01") && this.Val(id) == "Both" && this.Val(id.Substring(0, 4) + "93") == "8821")
          this.SetVal(id.Substring(0, 4) + "01", "Borrower");
      }
      string empty3 = string.Empty;
      if (id.Length == 6 && (id.StartsWith("IR") || id.StartsWith("AR")) && (id.EndsWith("57") || id.EndsWith("58")))
      {
        string str = id.Substring(0, 4);
        string orgval = id.EndsWith("57") ? this.Val(str + "04") : this.Val(str + "05");
        if (orgval != string.Empty)
        {
          if (val == "Y")
          {
            string val2 = orgval.Replace("-", "");
            if (val2.Length > 2)
              val2 = val2.Substring(0, 2) + "-" + val2.Substring(2);
            this.SetVal(id.EndsWith("57") ? str + "04" : str + "05", val2);
          }
          else
          {
            bool needsUpdate = false;
            string val3 = Utils.FormatInput(orgval, FieldFormat.SSN, ref needsUpdate);
            if (needsUpdate)
              this.SetVal(id.EndsWith("57") ? str + "04" : str + "05", val3);
          }
        }
      }
      else if (id == "IRS4506.X57" || id == "IRS4506.X58")
      {
        string orgval = id == "IRS4506.X57" ? this.Val("IRS4506.X4") : this.Val("IRS4506.X5");
        if (orgval != string.Empty)
        {
          if (val == "Y")
          {
            string val4 = orgval.Replace("-", "");
            if (val4.Length > 2)
              val4 = val4.Substring(0, 2) + "-" + val4.Substring(2);
            this.SetVal(id == "IRS4506.X57" ? "IRS4506.X4" : "IRS4506.X5", val4);
          }
          else
          {
            bool needsUpdate = false;
            string val5 = Utils.FormatInput(orgval, FieldFormat.SSN, ref needsUpdate);
            if (needsUpdate)
              this.SetVal(id == "IRS4506.X57" ? "IRS4506.X4" : "IRS4506.X5", val5);
          }
        }
      }
      else if ((id.StartsWith("IR") || id.StartsWith("AR")) && id.Length == 6 && id.EndsWith("01") && val != "Other")
      {
        this.SetVal(id.Substring(0, 4) + "65", "");
        this.SetVal(id.Substring(0, 4) + "66", "");
      }
      else if (id == "IRS4506.X1" && val != "Other")
      {
        this.SetVal("IRS4506.X63", "");
        this.SetVal("IRS4506.X64", "");
      }
      switch (this.Val("1543"))
      {
        case "DU":
          this.SetVal("DU.LP.ID", this.Val("MORNET.X4"));
          break;
        case "LP":
          this.SetVal("DU.LP.ID", this.Val("CASASRN.X13"));
          break;
      }
      if (!(id == "1393"))
        return;
      this.SetVal("749", DateTime.Now.ToString("MM/dd/yyyy"));
    }

    private void calculateIRS4506T(string id, string val)
    {
      int numberOfTaX4506Ts = this.loan.GetNumberOfTAX4506Ts(false);
      string empty = string.Empty;
      for (int index = 0; index < numberOfTaX4506Ts; ++index)
      {
        string str = index.ToString("00");
        if (!(this.Val("IR" + str + "59") != "Y"))
        {
          DateTime today = DateTime.Today;
          int num = today.Month < 6 || today.Month == 6 && today.Day < 15 ? today.Year - 2 : today.Year - 1;
          this.SetVal("IR" + str + "24", "1040");
          this.SetVal("IR" + str + "48", "Y");
          this.SetVal("IR" + str + "25", string.Format("12/31/{0}", (object) num));
          this.SetVal("IR" + str + "26", string.Format("12/31/{0}", (object) (num - 1)));
        }
      }
    }

    private void calculateIsLSSecondaryFile(string id, string val)
    {
      switch (this.loan.LinkSyncType)
      {
        case LinkSyncType.ConstructionPrimary:
        case LinkSyncType.PiggybackPrimary:
          this.SetVal("4117", "N");
          break;
        case LinkSyncType.ConstructionLinked:
        case LinkSyncType.PiggybackLinked:
          this.SetVal("4117", "Y");
          break;
        default:
          this.SetVal("4117", "");
          break;
      }
    }

    private void calculateLoanLinkSyncType(string id, string val)
    {
      if (this.Val("4185") == "None")
      {
        this.SetVal("4185", "");
      }
      else
      {
        switch (this.loan.LinkSyncType)
        {
          case LinkSyncType.ConstructionPrimary:
            this.SetVal("4185", "ConstructionPrimary");
            break;
          case LinkSyncType.ConstructionLinked:
            this.SetVal("4185", "ConstructionLinked");
            break;
          case LinkSyncType.PiggybackPrimary:
            this.SetVal("4185", "PiggybackPrimary");
            break;
          case LinkSyncType.PiggybackLinked:
            this.SetVal("4185", "PiggybackLinked");
            break;
          default:
            this.SetVal("4185", "");
            break;
        }
      }
    }

    private void calculateUnlinkedRemote(string id, string val) => this.SetVal("4185", "None");

    public void UpdateAccountName(string id, string val)
    {
      bool flag = false;
      if (Utils.ParseInt((object) id) >= 4000 && Utils.ParseInt((object) id) <= 4009)
        flag = true;
      if (((id == "36" || id == "37" || id == "68" || id == "69" ? 1 : (id == "181" ? 1 : 0)) | (flag ? 1 : 0)) != 0)
      {
        string str1 = this.Val("36").Trim();
        string str2 = this.Val("37").Trim();
        string str3 = this.Val("68").Trim();
        string str4 = this.Val("69").Trim();
        string str5 = str1 + " " + str2;
        if (str3 != string.Empty || str4 != string.Empty)
        {
          if (str4 == str2)
            str5 = str1 + " & " + str3 + " " + str2;
          else
            str5 = str1 + " " + str2 + " & " + str3 + " " + str4;
        }
        str5.Trim();
        string str6 = str1 + " " + str2;
        if (((id == "36" ? 1 : (id == "37" ? 1 : 0)) | (flag ? 1 : 0)) != 0)
        {
          this.SetVal("1868", str6.Trim());
          this.SetVal("1871", this.Val("4008").Trim());
        }
        if (((id == "68" ? 1 : (id == "69" ? 1 : 0)) | (flag ? 1 : 0)) != 0)
        {
          this.SetVal("1873", (str3 + " " + str4).Trim());
          this.SetVal("1876", this.Val("4009").Trim());
        }
        this.updateNamesInRequest(id, (string) null);
        this.calculateLiabilityAccountName(id, (string) null);
        this.calObjs.D1003Cal.PopulateOtherFields("updatetitle", (string) null);
      }
      if (id == "11" || id == "12" || id == "14" || id == "15" || id == "356" || id == "URLA.X73" || id == "URLA.X74" || id == "URLA.X75" || id == "16")
      {
        int numberOfMortgages = this.loan.GetNumberOfMortgages();
        for (int index = 1; index <= numberOfMortgages; ++index)
        {
          string str = "FM" + index.ToString("00");
          if (this.Val(str + "28") == "Y")
          {
            this.SetVal(str + "04", this.Val("11"));
            this.SetVal(str + "06", this.Val("12"));
            this.SetVal(str + "07", this.Val("14"));
            this.SetVal(str + "08", this.Val("15"));
            this.SetVal(str + "07", this.Val("14"));
            this.SetVal(str + "19", this.Val("356"));
            this.SetVal(str + "50", this.Val("URLA.X73"));
            this.SetVal(str + "47", this.Val("URLA.X74"));
            this.SetVal(str + "48", this.Val("URLA.X75"));
            this.SetVal(str + "54", this.Val("16"));
          }
        }
      }
      if (((id == "VA" || id == "36" || id == "37" || id == "68" ? 1 : (id == "69" ? 1 : 0)) | (flag ? 1 : 0)) != 0)
      {
        this.updateVAAccount(id, (string) null);
        this.calObjs.USDACal.CalcFirstTimeHomeBuyer(id, this.Val(id));
      }
      this.UpdateUlddFields();
    }

    private void UpdateUlddFields()
    {
      string str1 = this.Val("4000").Trim();
      this.SetVal("ULDD.FNM.4000", str1.Length > 25 ? str1.Substring(0, 25) : str1);
      string str2 = this.Val("4004").Trim();
      if (!string.IsNullOrEmpty(str2))
        this.SetVal("ULDD.FNM.4004", str2.Length > 25 ? str2.Substring(0, 25) : str2);
      string str3 = this.Val("4001");
      if (!string.IsNullOrEmpty(str3))
        this.SetVal("ULDD.FNM.4001", str3.Substring(0, 1));
      string str4 = this.Val("4005");
      if (string.IsNullOrEmpty(str4))
        return;
      this.SetVal("ULDD.FNM.4005", str4.Substring(0, 1));
    }

    private void updateBorrowerVestingName(string id, string val)
    {
      this.SetVal("1868", (this.Val("4000") + (this.Val("4001") != "" ? " " : "") + this.Val("4001") + (this.Val("4002") != "" ? " " : "") + this.Val("4002") + " " + this.Val("4003")).Trim());
    }

    private void updateCoborrowerVestingName(string id, string val)
    {
      this.SetVal("1873", (this.Val("4004") + (this.Val("4005") != "" ? " " : "") + this.Val("4005") + (this.Val("4006") != "" ? " " : "") + this.Val("4006") + " " + this.Val("4007")).Trim());
    }

    private void updateVAAccount(string id, string val)
    {
      string val1 = this.Val("VASUMM.X31");
      this.SetVal("VAELIG.X66", val1);
      this.SetVal("VAVOB.X51", val1);
      string str1 = string.Empty;
      string str2 = string.Empty;
      switch (val1)
      {
        case "Borrower":
          this.SetVal("VAELIG.X71", this.Val("36"));
          this.SetVal("VAELIG.X72", this.Val("37"));
          this.SetVal("VAELIG.X73", this.Val("65"));
          this.SetVal("VAELIG.X1", this.Val("1402"));
          this.SetVal("VAELIG.X74", this.Val("66"));
          this.SetVal("VAELIG.X97", this.Val("1240"));
          bool flag1 = this.Val("BR0123") == "Prior";
          this.SetVal("VAELIG.X75", flag1 ? this.Val("BR0204") : this.Val("BR0104"));
          this.SetVal("VAELIG.X76", flag1 ? this.Val("BR0206") : this.Val("BR0106"));
          this.SetVal("VAELIG.X77", flag1 ? this.Val("BR0207") : this.Val("BR0107"));
          this.SetVal("VAELIG.X78", flag1 ? this.Val("BR0208") : this.Val("BR0108"));
          str1 = this.Val("36") + " " + this.Val("37");
          str2 = this.Val("65");
          break;
        case "CoBorrower":
          this.SetVal("VAELIG.X71", this.Val("68"));
          this.SetVal("VAELIG.X72", this.Val("69"));
          this.SetVal("VAELIG.X73", this.Val("97"));
          this.SetVal("VAELIG.X1", this.Val("1403"));
          this.SetVal("VAELIG.X74", this.Val("98"));
          this.SetVal("VAELIG.X97", this.Val("1268"));
          bool flag2 = this.Val("CR0123") == "Prior";
          this.SetVal("VAELIG.X75", flag2 ? this.Val("CR0204") : this.Val("CR0104"));
          this.SetVal("VAELIG.X76", flag2 ? this.Val("CR0206") : this.Val("CR0106"));
          this.SetVal("VAELIG.X77", flag2 ? this.Val("CR0207") : this.Val("CR0107"));
          this.SetVal("VAELIG.X78", flag2 ? this.Val("CR0208") : this.Val("CR0108"));
          str1 = this.Val("68") + " " + this.Val("69");
          str2 = this.Val("97");
          break;
        default:
          this.SetVal("VAELIG.X71", string.Empty);
          this.SetVal("VAELIG.X72", string.Empty);
          this.SetVal("VAELIG.X73", string.Empty);
          this.SetVal("VAELIG.X75", string.Empty);
          this.SetVal("VAELIG.X76", string.Empty);
          this.SetVal("VAELIG.X77", string.Empty);
          this.SetVal("VAELIG.X78", string.Empty);
          this.SetVal("VAELIG.X97", string.Empty);
          break;
      }
      switch (val1)
      {
        case "Borrower":
          this.SetVal("VASUMM.X32", this.Val("36"));
          this.SetVal("VASUMM.X33", this.Val("37"));
          this.SetVal("VASUMM.X34", this.Val("65"));
          this.SetVal("VASUMM.X35", this.Val("471"));
          this.SetVal("VASUMM.X1", this.Val("1402"));
          break;
        case "CoBorrower":
          this.SetVal("VASUMM.X32", this.Val("68"));
          this.SetVal("VASUMM.X33", this.Val("69"));
          this.SetVal("VASUMM.X34", this.Val("97"));
          this.SetVal("VASUMM.X35", this.Val("478"));
          this.SetVal("VASUMM.X1", this.Val("1403"));
          break;
        default:
          this.SetVal("VASUMM.X32", string.Empty);
          this.SetVal("VASUMM.X33", string.Empty);
          this.SetVal("VASUMM.X34", string.Empty);
          this.SetVal("VASUMM.X35", string.Empty);
          this.SetVal("VASUMM.X1", string.Empty);
          break;
      }
      BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
      switch (val1)
      {
        case "Borrower":
          this.SetVal("VAVOB.X52", this.Val("36"));
          this.SetVal("VAVOB.X53", this.Val("37"));
          this.SetVal("VAVOB.X54", this.Val("65"));
          this.SetVal("VAVOB.X55", this.Val("1402"));
          this.SetVal("VAVOB.X44", this.Val("FR0104", borrowerPairs[0]));
          this.SetVal("VAVOB.X45", this.Val("FR0106", borrowerPairs[0]));
          this.SetVal("VAVOB.X46", this.Val("FR0107", borrowerPairs[0]));
          this.SetVal("VAVOB.X47", this.Val("FR0108", borrowerPairs[0]));
          break;
        case "CoBorrower":
          this.SetVal("VAVOB.X52", this.Val("68"));
          this.SetVal("VAVOB.X53", this.Val("69"));
          this.SetVal("VAVOB.X54", this.Val("97"));
          this.SetVal("VAVOB.X55", this.Val("1403"));
          this.SetVal("VAVOB.X44", this.Val("FR0204", borrowerPairs[0]));
          this.SetVal("VAVOB.X45", this.Val("FR0206", borrowerPairs[0]));
          this.SetVal("VAVOB.X46", this.Val("FR0207", borrowerPairs[0]));
          this.SetVal("VAVOB.X47", this.Val("FR0208", borrowerPairs[0]));
          break;
        default:
          this.SetVal("VAVOB.X52", string.Empty);
          this.SetVal("VAVOB.X53", string.Empty);
          this.SetVal("VAVOB.X54", string.Empty);
          this.SetVal("VAVOB.X55", string.Empty);
          this.SetVal("VAVOB.X44", string.Empty);
          this.SetVal("VAVOB.X45", string.Empty);
          this.SetVal("VAVOB.X46", string.Empty);
          this.SetVal("VAVOB.X47", string.Empty);
          break;
      }
    }

    private void updateNamesInRequest(string id, string val)
    {
      BorrowerPair[] borrowerPairs = this.loan.GetBorrowerPairs();
      if (borrowerPairs == null)
        return;
      if (this.Val("REQUEST.X25") == "Borrower")
      {
        this.SetVal("REQUEST.X29", this.Val("36", borrowerPairs[0]).Trim() + " " + this.Val("37", borrowerPairs[0]).Trim());
        this.SetVal("REQUEST.X30", this.Val("66", borrowerPairs[0]));
        this.SetVal("REQUEST.X31", this.Val("FE0117", borrowerPairs[0]));
        this.SetVal("REQUEST.X32", this.Val("1490", borrowerPairs[0]));
        this.SetVal("REQUEST.X33", this.Val("1240", borrowerPairs[0]));
      }
      else
      {
        if (!(this.Val("REQUEST.X25") == "CoBorrower"))
          return;
        this.SetVal("REQUEST.X29", this.Val("68", borrowerPairs[0]).Trim() + " " + this.Val("69", borrowerPairs[0]).Trim());
        this.SetVal("REQUEST.X30", this.Val("98", borrowerPairs[0]));
        this.SetVal("REQUEST.X31", this.Val("FE0217", borrowerPairs[0]));
        this.SetVal("REQUEST.X32", this.Val("1480", borrowerPairs[0]));
        this.SetVal("REQUEST.X33", this.Val("1268", borrowerPairs[0]));
      }
    }

    private void onBorrowerPairChanged(LoanData loan)
    {
      this.calObjs.Cal.UpdateAccountName("68", (string) null);
      this.calObjs.D1003Cal.CopyInfoToLockRequest((string) null, (string) null);
      if (loan != null)
      {
        int index1 = 0;
        string id = loan.CurrentBorrowerPair.Id;
        BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
        for (int index2 = 0; index2 < borrowerPairs.Length; ++index2)
        {
          loan.SetBorrowerPair(borrowerPairs[index2]);
          this.calObjs.FM1084Cal.FormCal();
          if (id == borrowerPairs[index2].Id)
            index1 = index2;
          if (this.calObjs.D1003URLA2020Cal.USEURLA2020)
          {
            this.calObjs.D1003URLA2020Cal.CalcNonSubjectPropertyDebtsToBePaidOffAmount((string) null, (string) null);
            this.calObjs.VERIFCal.CalcOtherIncome((string) null, (string) null);
            this.calObjs.D1003URLA2020Cal.CalcVODTotalDeposits((string) null, (string) null);
            this.calObjs.D1003URLA2020Cal.CalcVOOATotalOtherAssets((string) null, (string) null);
            this.calObjs.VERIFCal.CalcOtherLiabilityMonthlyIncome((string) null, (string) null);
            this.calObjs.VERIFCal.CalcAdditionalLoansAmount((string) null, (string) null);
            this.calObjs.VERIFCal.CalcAppliedToDownpayment((string) null, (string) null);
            this.calObjs.D1003Cal.CalcTotalAssets((string) null, (string) null);
          }
        }
        loan.SetBorrowerPair(borrowerPairs[index1]);
        this.calObjs.D1003URLA2020Cal.CalcFirstTimeHomeBuyer((string) null, (string) null);
        this.calObjs.ULDDExpCal.CalcFannieMaeExportFields("934", this.Val("934"));
      }
      this.CalculateAll();
      this.LoadeSignConsentDate();
      this.calObjs.D1003URLA2020Cal.CalcTitleInWhichPropertyHeld((string) null, (string) null);
      this.calObjs.D1003URLA2020Cal.CalcBorrowerCount((string) null, (string) null);
      this.calObjs.D1003URLA2020Cal.CalcCreditType((string) null, (string) null);
      this.calObjs.VERIFCal.CalcPaceLoanAmounts((string) null, (string) null);
      this.calObjs.USDACal.CalcBorrowerId((string) null, (string) null);
    }

    private void onVestingChanged(LoanData loan)
    {
      this.calObjs.D1003URLA2020Cal.CalcTitleInWhichPropertyHeld((string) null, (string) null);
    }

    private void onLienPositionChanged(LoanData loan)
    {
      this.calObjs.PrequalCal.CalcDownPay("1109", loan.GetSimpleField("1109"));
      this.CalculateAll();
    }

    internal void LoadeSignConsentDate()
    {
      string[] strArray1 = new string[12]
      {
        "3985",
        "3989",
        "3993",
        "3997",
        "4024",
        "4028",
        "4032",
        "4036",
        "4040",
        "4044",
        "4048",
        "4052"
      };
      string[] strArray2 = new string[12]
      {
        "3984",
        "3988",
        "3992",
        "3996",
        "4023",
        "4027",
        "4031",
        "4035",
        "4039",
        "4043",
        "4047",
        "4051"
      };
      List<int> linkedVestingIdxList = this.loan.GetNBOLinkedVestingIdxList();
      DateTime dateTime1 = new DateTime();
      int index = -2;
      bool flag = true;
      foreach (BorrowerPair borrowerPair in this.loan.GetBorrowerPairs())
      {
        index += 2;
        DateTime dateTime2 = new DateTime();
        if (this.loan.GetField(strArray2[index]) == "Accepted")
        {
          DateTime date1 = Utils.ParseDate((object) this.loan.GetField(strArray1[index]));
          if (date1 > DateTime.MinValue && date1 > dateTime1)
            dateTime1 = date1;
          if ((borrowerPair.CoBorrower.FirstName + " " + borrowerPair.CoBorrower.LastName).Trim() != "")
          {
            if (this.loan.GetField(strArray2[index + 1]) == "Accepted")
            {
              DateTime date2 = Utils.ParseDate((object) this.loan.GetField(strArray1[index + 1]));
              if ((borrowerPair.CoBorrower.FirstName + " " + borrowerPair.CoBorrower.LastName).Trim() != "" && date2 > DateTime.MinValue && date2 > dateTime1)
                dateTime1 = date2;
            }
            else
            {
              dateTime1 = DateTime.MinValue;
              flag = false;
              break;
            }
          }
        }
        else
        {
          dateTime1 = DateTime.MinValue;
          flag = false;
          break;
        }
      }
      if (flag)
      {
        foreach (int num in linkedVestingIdxList)
        {
          string str1 = "NBOC";
          string str2 = num.ToString("00");
          string id1 = str1 + str2 + "17";
          string id2 = str1 + str2 + "18";
          DateTime dateTime3 = new DateTime();
          if (this.loan.GetField(id1) == "Accepted")
          {
            DateTime date = Utils.ParseDate((object) this.loan.GetField(id2));
            if (date > DateTime.MinValue && date > dateTime1)
              dateTime1 = date;
          }
          else
          {
            dateTime1 = DateTime.MinValue;
            break;
          }
        }
      }
      if (dateTime1 > DateTime.MinValue)
        this.loan.SetField("3983", dateTime1.ToString());
      else
        this.loan.SetField("3983", "");
    }

    private void calculateLoanStatusLastUpdatedDateTime(string id, string val)
    {
      this.SetVal("CORRESPONDENT.X54", System.TimeZoneInfo.ConvertTimeToUtc(this.sessionObjects.Session.ServerTime).ToString());
    }

    private void calculateCorrespondentPublishedDate(string id, string val)
    {
      EnhancedConditionLog[] enhancedConditions = this.loan.GetLogList().GetAllEnhancedConditions();
      if (!(this.Val("CORRESPONDENT.X55") == "Y"))
        return;
      foreach (EnhancedConditionLog enhancedConditionLog in enhancedConditions)
      {
        bool? externalPrint = enhancedConditionLog.ExternalPrint;
        bool flag = true;
        if (externalPrint.GetValueOrDefault() == flag & externalPrint.HasValue)
        {
          DateTime? publishedDate = enhancedConditionLog.PublishedDate;
          if (publishedDate.HasValue)
          {
            publishedDate = enhancedConditionLog.PublishedDate;
            DateTime minValue = DateTime.MinValue;
            if ((publishedDate.HasValue ? (publishedDate.HasValue ? (publishedDate.GetValueOrDefault() == minValue ? 1 : 0) : 1) : 0) == 0)
              continue;
          }
          enhancedConditionLog.PublishedDate = new DateTime?(System.TimeZoneInfo.ConvertTimeToUtc(this.sessionObjects.Session.ServerTime));
        }
      }
    }

    private void calculatePropertyEstateType(string id, string val)
    {
      if (this.Val("1825") == "2009" && this.Val("33") == "Other")
        this.SetVal("ULDD.X197", "Other");
      else if (this.Val("1825") == "2020" && this.Val("URLA.X138") == "LifeEstate")
        this.SetVal("ULDD.X197", "Other");
      else
        this.SetVal("ULDD.X197", this.Val("1066"));
    }

    private void calculateLomaOrLomrIndicator(string id, string val)
    {
      if (this.Val("TQL.X108") != "" && this.Val("TQL.X108") != "//")
        this.SetVal("TQL.X107", "Y");
      else
        this.SetVal("TQL.X107", "N");
    }

    private bool le1x9Validation(string id, string val)
    {
      if (Utils.GetTimeZoneInfo(val) == null)
        return false;
      if (!((IEnumerable<IDisclosureTracking2015Log>) this.loan.GetLogList().GetAllIDisclosureTracking2015Log(false)).Any<IDisclosureTracking2015Log>())
        return true;
      string a = this.Val(id);
      return string.Equals(a, val) || a != null && val != null && a.Length == val.Length && (a.Length == 3 && (int) a[0] == (int) val[0] && (int) a[2] == (int) val[2] || a.Length == 4 && (int) a[0] == (int) val[0] && (int) a[1] == (int) val[1] && (int) a[3] == (int) val[3]);
    }

    private bool le1xg9Validation(string id, string val)
    {
      return Utils.GetValidGenericTimeZone(val) != null && (!((IEnumerable<IDisclosureTracking2015Log>) this.loan.GetLogList().GetAllIDisclosureTracking2015Log(false)).Any<IDisclosureTracking2015Log>() || string.Equals(this.Val(id), val));
    }

    private void calculateOtherDescription(string id, string val)
    {
      if (id == "3865" && val != "Y")
      {
        this.SetVal("4719", string.Empty);
      }
      else
      {
        if (!(id == "3871") || !(val != "Y"))
          return;
        this.SetVal("4720", string.Empty);
      }
    }

    private void calculate4506CPrintVersion(string id, string val)
    {
      string str = this.Val("IRS4506.X67");
      if (this.Val("IRS4506.X92") == "" && str == "Y")
      {
        this.SetVal("IRS4506.X92", "Oct2022");
      }
      else
      {
        if (!(str != "Y"))
          return;
        this.SetVal("IRS4506.X92", "");
      }
    }

    private void updateIndexRatePrecision(string id, string val)
    {
      this.loan.SetCurrentField("CORRESPONDENT.X104", this.Val("CORRESPONDENT.X104"));
      this.loan.SetCurrentField("688", this.Val("688"));
      this.loan.SetCurrentField("4513", this.Val("4513"));
      this.loan.SetCurrentField("SERVICE.X16", this.Val("SERVICE.X16"));
      this.loan.SetCurrentField("RE88395.X313", this.Val("RE88395.X313"));
      this.loan.SetCurrentField("S32DISC.X150", this.Val("S32DISC.X150"));
    }

    private void calculateAMILimit(string id, string val)
    {
      if (string.IsNullOrEmpty(this.Val("MORNET.X30")) || string.IsNullOrEmpty(this.Val("4985")))
        this.SetVal("4986", "");
      else
        this.SetVal("4986", string.Concat((object) (this.FltVal("4985") / 100.0 * this.FltVal("MORNET.X30"))));
    }

    private void calculateHomeCounseling(string id, string val)
    {
      string settingFromCache = this.sessionObjects.GetCompanySettingFromCache("Automated Data Completion", "HomeCounseling");
      bool flag = settingFromCache != null && "enabled" == settingFromCache.ToLower();
      if (id == "GETAGENCIES")
        flag = true;
      string str1 = this.Val("FR0108");
      string state = this.Val("FR0107");
      string city = this.Val("FR0106");
      if (this.Val("1825") == "2020" && this.Val("FR0129") == "Y")
      {
        str1 = this.Val("15");
        state = this.Val("14");
        city = this.Val("12");
      }
      if (!flag || this.loan.GetNumberOfHomeCounselingProviders() != 0 || !(str1 != "") || !(state != "") || !(city != "") || !(this.Val("HCSETTING.DISTANCE") != "") || !(this.Val("HCSETTING.SERVICES") != "") || !(this.Val("HCSETTING.LANGUAGES") != ""))
        return;
      bool validGeo;
      string homeCounselingUrl = this.loan.GetHomeCounselingUrl(str1, state, city, out validGeo);
      if (!validGeo)
        return;
      List<List<string[]>> counselingResults = this.sessionObjects.ParseHomeCounselingResults(this.sessionObjects.GetHomeCounseling(homeCounselingUrl, (IWin32Window) null, true));
      if (counselingResults.Count < 10)
        return;
      SortedList<double, List<string[]>> sortedList = new SortedList<double, List<string[]>>();
      Dictionary<string, List<string[]>> dictionary = new Dictionary<string, List<string[]>>();
      List<Tuple<double, string>> source = new List<Tuple<double, string>>();
      GeoCoordinate geoCoordinate = (GeoCoordinate) null;
      double latitude1 = 0.0;
      double num1 = 0.0;
      GeoCoordinate zipGeoCoordinate = ZipCodeUtils.GetZipGeoCoordinate(str1, state, city, "");
      if (zipGeoCoordinate != (GeoCoordinate) null)
      {
        latitude1 = zipGeoCoordinate.Latitude;
        num1 = zipGeoCoordinate.Longitude;
      }
      try
      {
        geoCoordinate = new GeoCoordinate(latitude1, num1 = zipGeoCoordinate.Longitude);
      }
      catch (Exception ex)
      {
        Tracing.Log(Calculation.sw, nameof (Calculation), TraceLevel.Error, ex.ToString());
      }
      foreach (List<string[]> strArrayList in counselingResults)
      {
        double latitude2 = 0.0;
        double longitude = 0.0;
        string key = "";
        foreach (string[] strArray in strArrayList)
        {
          string str2 = strArray[1];
          if (string.Compare(strArray[0], "agc_ADDR_LATITUDE", true) == 0)
            latitude2 = Utils.ParseDouble((object) strArray[1]);
          else if (string.Compare(strArray[0], "agc_ADDR_LONGITUDE", true) == 0)
            longitude = Utils.ParseDouble((object) strArray[1]);
          else if (string.Compare(strArray[0], "agcid", true) == 0)
            key = strArray[1];
        }
        double num2 = Utils.ArithmeticRounding(geoCoordinate.GetDistanceTo(new GeoCoordinate(latitude2, longitude)) / 1609.34, 2);
        source.Add(new Tuple<double, string>(num2, key));
        dictionary.Add(key, strArrayList);
      }
      if (this.serviceList == null || this.languageList == null)
      {
        List<KeyValuePair<string, string>>[] languageSupported = this.sessionObjects.ConfigurationManager.GetHomeCounselingServiceLanguageSupported();
        if (languageSupported != null && languageSupported.Length == 2)
        {
          this.serviceList = languageSupported[0];
          this.languageList = languageSupported[1];
        }
      }
      foreach (Tuple<double, string> tuple in source.OrderBy<Tuple<double, string>, double>((Func<Tuple<double, string>, double>) (t => t.Item1)).Take<Tuple<double, string>>(10))
      {
        List<string[]> strArrayList = dictionary[tuple.Item2];
        int num3 = this.loan.NewHomeCounselingProvider();
        string str3 = "";
        for (int index = 0; index < strArrayList.Count; ++index)
        {
          string[] strArray = strArrayList[index];
          string str4 = strArray[0];
          string str5 = strArray[1];
          switch (str4)
          {
            case "adr1":
              this.SetVal("HC" + num3.ToString("00") + "03", str5);
              break;
            case "agcid":
              this.SetVal("HC" + num3.ToString("00") + "99", str5);
              str3 = str5;
              break;
            case "city":
              if (!string.IsNullOrEmpty(str5))
                str5 = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str5.ToLower());
              this.SetVal("HC" + num3.ToString("00") + "04", str5);
              break;
            case "distance":
              this.SetVal("HC" + num3.ToString("00") + "17", str5);
              break;
            case "email":
              if (string.IsNullOrEmpty(str5) || string.Compare(str5, "N/A", true) == 0)
                str5 = "Not Available";
              this.SetVal("HC" + num3.ToString("00") + "10", str5);
              break;
            case "fax":
              this.SetVal("HC" + num3.ToString("00") + "09", str5);
              break;
            case "languages":
              string val1 = this.sessionObjects.translateServiceLanguageCodes(str5, false, this.serviceList, this.languageList);
              this.SetVal("HC" + num3.ToString("00") + "12", val1);
              break;
            case "nme":
              this.SetVal("HC" + num3.ToString("00") + "02", str5);
              break;
            case "phone1":
              this.SetVal("HC" + num3.ToString("00") + "07", str5);
              break;
            case "phone2":
              this.SetVal("HC" + num3.ToString("00") + "08", str5);
              break;
            case "services":
              string val2 = this.sessionObjects.translateServiceLanguageCodes(str5, true, this.serviceList, this.languageList);
              this.SetVal("HC" + num3.ToString("00") + "13", val2);
              break;
            case "statecd":
              this.SetVal("HC" + num3.ToString("00") + "05", str5);
              break;
            case "weburl":
              if (string.IsNullOrEmpty(str5) || string.Compare(str5, "N/A", true) == 0)
                str5 = "Not Available";
              this.SetVal("HC" + num3.ToString("00") + "11", str5);
              break;
            case "zipcd":
              this.SetVal("HC" + num3.ToString("00") + "06", str5);
              break;
          }
        }
        double val3 = tuple.Item1;
        this.SetVal("HC" + num3.ToString("00") + "17", Utils.ArithmeticRounding(val3, 2).ToString());
        this.SetVal("HC" + num3.ToString("00") + "16", "CFPB Import");
        this.SetVal("HC" + num3.ToString("00") + "01", "Y");
      }
    }

    private void calculateDisclosureReadyDate(string id, string val)
    {
      if (this.USEURLA2020)
      {
        List<string> completionFields = ((AlertConfigWithDataCompletionFields) this.loan.Settings.AlertSetupData.GetAlertConfig(72)).DataCompletionFields;
        List<string> second1 = new List<string>()
        {
          "1419",
          "FR0108",
          "1240",
          "1178",
          "Opening.PlanID",
          "DISCLOSUREPLANCODE"
        };
        List<string> second2 = new List<string>()
        {
          "URLA.X197",
          "1417",
          "1418",
          "FR0126",
          "FR0106",
          "FR0107",
          "5012",
          "3343",
          "3344",
          "3345",
          "3346"
        };
        List<string> list = completionFields.Except<string>((IEnumerable<string>) second1).ToList<string>();
        if (this.isFulfillmentEnabled())
          list = list.Except<string>((IEnumerable<string>) second2).ToList<string>();
        bool flag1 = true;
        BorrowerPair currentBorrowerPair = this.loan.CurrentBorrowerPair;
        this.loan.SetBorrowerPair(0);
        foreach (string id1 in list)
        {
          if (this.Val(id1) == "")
          {
            flag1 = false;
            break;
          }
        }
        if (flag1)
        {
          flag1 = flag1 && (this.Val("1419") != "" || this.Val("FR0108") != "") && (this.Val("1240") != "" || this.Val("1178") != "") && (this.Val("Opening.PlanID") != "" || this.Val("DISCLOSUREPLANCODE") != "");
          if (this.isFulfillmentEnabled())
            flag1 = flag1 && (this.isBorMailAddressComplete() || this.isBorCurrAddressComplete()) && this.isLenderAddressComplete() && this.Val("5012") != "";
        }
        string str = this.Val("5001");
        bool flag2 = str == "" || str == "//";
        if (flag1)
        {
          if (flag2)
          {
            DateTime dateTime = DateTime.Now;
            dateTime = dateTime.Date;
            this.SetVal("5001", dateTime.ToString("MM/dd/yyyy"));
          }
        }
        else
          this.SetVal("5001", "");
        this.loan.SetBorrowerPair(currentBorrowerPair);
      }
      else
        this.SetVal("5001", "");
    }

    private void calculateAtAppDisclosureDate(string id, string val)
    {
      if (this.USEURLA2020)
      {
        List<string> completionFields = ((AlertConfigWithDataCompletionFields) this.loan.Settings.AlertSetupData.GetAlertConfig(73)).DataCompletionFields;
        List<string> second1 = new List<string>()
        {
          "1419",
          "FR0108",
          "1240",
          "1178",
          "Opening.PlanID",
          "DISCLOSUREPLANCODE"
        };
        List<string> second2 = new List<string>()
        {
          "URLA.X197",
          "1417",
          "1418",
          "FR0126",
          "FR0106",
          "FR0107",
          "5012",
          "3343",
          "3344",
          "3345",
          "3346"
        };
        List<string> list = completionFields.Except<string>((IEnumerable<string>) second1).ToList<string>();
        if (this.isFulfillmentEnabled())
          list = list.Except<string>((IEnumerable<string>) second2).ToList<string>();
        bool flag1 = true;
        BorrowerPair currentBorrowerPair = this.loan.CurrentBorrowerPair;
        this.loan.SetBorrowerPair(0);
        foreach (string id1 in list)
        {
          if (this.Val(id1) == "")
          {
            flag1 = false;
            break;
          }
        }
        if (flag1)
        {
          flag1 = flag1 && (this.Val("1419") != "" || this.Val("FR0108") != "") && (this.Val("1240") != "" || this.Val("1178") != "") && (this.Val("Opening.PlanID") != "" || this.Val("DISCLOSUREPLANCODE") != "");
          if (this.isFulfillmentEnabled())
            flag1 = flag1 && (this.isBorMailAddressComplete() || this.isBorCurrAddressComplete()) && this.isLenderAddressComplete() && this.Val("5012") != "";
        }
        string str = this.Val("5005");
        bool flag2 = str == "" || str == "//";
        if (flag1)
        {
          if (flag2)
          {
            DateTime dateTime = DateTime.Now;
            dateTime = dateTime.Date;
            this.SetVal("5005", dateTime.ToString("MM/dd/yyyy"));
          }
        }
        else
          this.SetVal("5005", "");
        this.loan.SetBorrowerPair(currentBorrowerPair);
      }
      else
        this.SetVal("5005", "");
    }

    private void calculateAtLockDisclosureDate(string id, string val)
    {
      if (this.USEURLA2020)
      {
        List<string> completionFields = ((AlertConfigWithDataCompletionFields) this.loan.Settings.AlertSetupData.GetAlertConfig(47)).DataCompletionFields;
        List<string> second1 = new List<string>()
        {
          "1240",
          "1178"
        };
        List<string> second2 = new List<string>()
        {
          "URLA.X197",
          "1417",
          "1418",
          "1419",
          "FR0126",
          "FR0106",
          "FR0107",
          "FR0108",
          "5012",
          "3343",
          "3344",
          "3345",
          "3346"
        };
        List<string> list = completionFields.Except<string>((IEnumerable<string>) second1).ToList<string>();
        if (this.isFulfillmentEnabled())
          list = list.Except<string>((IEnumerable<string>) second2).ToList<string>();
        bool flag1 = true;
        BorrowerPair currentBorrowerPair = this.loan.CurrentBorrowerPair;
        this.loan.SetBorrowerPair(0);
        foreach (string id1 in list)
        {
          if (this.Val(id1) == "")
          {
            flag1 = false;
            break;
          }
        }
        if (flag1)
        {
          flag1 = flag1 && (this.Val("1240") != "" || this.Val("1178") != "");
          if (this.isFulfillmentEnabled())
            flag1 = flag1 && (this.isBorMailAddressComplete() || this.isBorCurrAddressComplete()) && this.isLenderAddressComplete() && this.Val("5012") != "";
        }
        string str = this.Val("5003");
        bool flag2 = str == "" || str == "//";
        if (flag1)
        {
          if (flag2)
          {
            DateTime dateTime = DateTime.Now;
            dateTime = dateTime.Date;
            this.SetVal("5003", dateTime.ToString("MM/dd/yyyy"));
          }
        }
        else
          this.SetVal("5003", "");
        this.loan.SetBorrowerPair(currentBorrowerPair);
      }
      else
        this.SetVal("5003", "");
    }

    private void calculatChangeCircumstanceRequirementsDate(string id, string val)
    {
      if (this.USEURLA2020)
      {
        List<string> completionFields = ((AlertConfigWithDataCompletionFields) this.loan.Settings.AlertSetupData.GetAlertConfig(46)).DataCompletionFields;
        List<string> second1 = new List<string>()
        {
          "1240",
          "1178"
        };
        List<string> second2 = new List<string>()
        {
          "URLA.X197",
          "1417",
          "1418",
          "1419",
          "FR0126",
          "FR0106",
          "FR0107",
          "FR0108",
          "5012",
          "3343",
          "3344",
          "3345",
          "3346"
        };
        List<string> list = completionFields.Except<string>((IEnumerable<string>) second1).ToList<string>();
        if (this.isFulfillmentEnabled())
          list = list.Except<string>((IEnumerable<string>) second2).ToList<string>();
        bool flag1 = true;
        BorrowerPair currentBorrowerPair = this.loan.CurrentBorrowerPair;
        this.loan.SetBorrowerPair(0);
        foreach (string id1 in list)
        {
          if (this.Val(id1) == "")
          {
            flag1 = false;
            break;
          }
        }
        if (flag1)
        {
          flag1 = flag1 && (this.Val("1240") != "" || this.Val("1178") != "");
          if (this.isFulfillmentEnabled())
            flag1 = flag1 && (this.isBorMailAddressComplete() || this.isBorCurrAddressComplete()) && this.isLenderAddressComplete() && this.Val("5012") != "";
        }
        string str = this.Val("5007");
        bool flag2 = str == "" || str == "//";
        if (flag1)
        {
          if (flag2)
          {
            DateTime dateTime = DateTime.Now;
            dateTime = dateTime.Date;
            this.SetVal("5007", dateTime.ToString("MM/dd/yyyy"));
          }
        }
        else
          this.SetVal("5007", "");
        this.loan.SetBorrowerPair(currentBorrowerPair);
      }
      else
        this.SetVal("5007", "");
    }

    private bool isFulfillmentEnabled()
    {
      return this.sessionObjects.GetCompanySettingFromCache("Fulfillment", "ServiceEnabled") == "Y" && this.sessionObjects.GetCompanySettingFromCache("Fulfillment", "AutoFulfillment") == "Y";
    }

    private bool isBorMailAddressComplete()
    {
      return this.Val("URLA.X197") != "" && this.Val("1417") != "" && this.Val("1418") != "" && this.Val("1419") != "";
    }

    private bool isBorCurrAddressComplete()
    {
      return this.Val("FR0126") != "" && this.Val("FR0106") != "" && this.Val("FR0107") != "" && this.Val("FR0108") != "";
    }

    private bool isLenderAddressComplete()
    {
      return this.Val("3343") != "" && this.Val("3344") != "" && this.Val("3345") != "" && this.Val("3346") != "";
    }

    private void populateAMILimits(string id, string val)
    {
      string settingFromCache = this.sessionObjects.GetCompanySettingFromCache("Automated Data Completion", "AreaMedianIncome");
      if (settingFromCache == null || !("enabled" == settingFromCache.ToLower()))
        return;
      this.calObjs.SystemTableCal.ExecuteTable_AMI(false, true);
    }

    private void populateMFILimits(string id, string val)
    {
      string settingFromCache = this.sessionObjects.GetCompanySettingFromCache("Automated Data Completion", "MedianFamilyIncome");
      if (settingFromCache == null || !("enabled" == settingFromCache.ToLower()))
        return;
      this.calObjs.SystemTableCal.ExecuteTable_MFI(false, true);
    }

    private void clearAMILimits(string id, string val)
    {
      if (!(id == "5027") || !(this.Val("5027") != "Y"))
        return;
      this.SetVal("4970", "");
      this.SetVal("MORNET.X30", "");
      this.SetVal("4971", "");
      this.SetVal("4972", "");
    }

    private void clearMFILimits(string id, string val)
    {
      if (!(id == "5028") || !(val != "Y"))
        return;
      this.SetVal("5018", "");
      this.SetVal("5019", "");
      this.SetVal("5020", "");
      this.SetVal("5021", "");
    }

    private void calculateChangesReceivedDate(string id, string val)
    {
      if (!(id == "CD1.X61"))
        return;
      if (this.Val(id) == "Y")
        this.SetVal("CD1.X62", DateTime.Today.ToString("MM/dd/yyyy"));
      else
        this.SetVal("CD1.X62", "");
    }
  }
}
