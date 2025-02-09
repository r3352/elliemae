// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.ILoanCalculator
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.DataEngine.InterimServicing;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public interface ILoanCalculator : IDisposable
  {
    void CalculateNewLoan();

    void FormCalculation(string formID, string id, string val);

    void FormCalculation(string calculationID);

    void UpdateCurrentMailingAddress();

    void RecalculateManualInput();

    void CalculateMaxLoanAmt();

    double CalculateFHAMaxLoanAmt(double propValue, bool getfactor);

    void UpdateAccountName(string id);

    string GetQualificationDetail(int column);

    void SpecialCalculation(CalculationActionID actionID);

    void SpecialCalculation(CalculationActionID actionID, string id, string val);

    void InitializingCalculation(string id, string val);

    bool UseWorstCaseScenario { get; set; }

    bool UseBestCaseScenario { get; set; }

    bool UseInterimServicingScenario { get; set; }

    LoanData CalculateWorstCaseScenario();

    LoanData CalculateWorstCaseScenario(bool setField);

    PaymentScheduleSnapshot CalculateInterimServicingPaymentSchedule(bool setField);

    void CalculateProjectedPaymentTable();

    void CalculateProjectedPaymentTable(bool setField);

    PaymentScheduleSnapshot GetBestCaseScenarioPaymentSchedule(LoanData bestCaseLoan);

    bool TriggerTradeOffCalculation { get; }

    string CalculatePrinting(ref NameValueCollection FieldCollect, string formID, string pageDone);

    string CalculatePrinting(
      ref NameValueCollection FieldCollect,
      string formID,
      string pageDone,
      bool printLicense);

    string CalculatePrinting(
      ref NameValueCollection FieldCollect,
      string formID,
      int blockNo,
      string pageDone);

    string CalculatePrinting(
      ref NameValueCollection FieldCollect,
      string formID,
      int blockNo,
      bool isBorrower,
      string pageDone);

    string CalculatePrinting(
      ref NameValueCollection FieldCollect,
      DocumentLog doc,
      string pageDone);

    string CalculatePrinting(
      ref NameValueCollection FieldCollect,
      ConditionLog condition,
      string pageDone);

    string CalculatePrinting(
      ref NameValueCollection FieldCollect,
      ConditionalLetterPrintOption letterOption,
      List<string> selectedConditions,
      string pageDone);

    string CalculatePrinting(
      ref NameValueCollection FieldCollect,
      MilestoneTaskLog taskLog,
      string pageDone);

    string CalculatePrinting(
      ref NameValueCollection FieldCollect,
      LoanData dummyLoan,
      DisclosureTrackingBase disclosureLog,
      int pageNo,
      string pageDone);

    string OutputFormSizeCheck(string formID);

    bool HasNextPrintPage(string formID);

    void PrefixCalculations(string version);

    void CalculateAll(bool copyUnpaid);

    void CalculateAll();

    void ImportCalculation();

    void ImportCalculation(bool copyUnpaid);

    PaymentScheduleSnapshot GetPaymentSchedule(bool needKeyFields);

    PaymentScheduleSnapshot GetWorstCaseScenarioPaymentSchedule(bool needKeyFields);

    bool SkipLockRequestSync { get; set; }

    bool SkipLinkedSync { get; set; }

    RegzSummaryTableType RegzSummaryType { get; }

    bool SkipFieldChangeEvent { get; set; }

    void CopyMLDSToGFE();

    void CopyMLDSToGFE(DataTemplate dataTemplate);

    void CopyGFEToMLDS();

    void CopyGFEToMLDS(string id);

    void CalculateFunder(string id, string val);

    void ShiftPaymentFrequence(DateTime previous1stPayDate, DateTime new1stPayDate);

    void CalculateInterimServicing(bool current);

    ServicingSummaryViewModel GetInterimServicingAnnualSummary(int year);

    void CalculateInvestorStatus();

    void CopyHUD2010ToGFE2010();

    void CopyHUD2010ToGFE2010(string id, bool blankCopyOnly);

    void CopyGFE2010ToHUD2010();

    double CalcTitleEscrowRate(TableFeeListBase.FeeTable t);

    void PopulateFeeList(string id, bool recalculated);

    void PopulateFeeList(bool recalculated);

    bool CalcFundingBalancingWorksheet(object gfeItemObject, ref string paidBy, ref double amount);

    Dictionary<string, object> GetFundingBalancingWorksheet();

    bool ValidateRevolvingVOLs();

    bool UpdateRevolvingLiabilities(
      string id,
      string val,
      bool setRevolvingFactor,
      bool confirmedRequired,
      bool doCalculation);

    void CalculateREGZSummary(string id);

    bool CalcOnDemand();

    bool CalcOnDemand(CalcOnDemandEnum calcOnDemandMethod);

    void EditCalcOnDemandEnum(CalcOnDemandEnum calcOnDemandMethod, bool include);

    void CalcPaymentSchedule();

    Dictionary<string, string> BuildHelocExampleSchedules();

    List<List<string[]>> GetHelocExampleSchedules();

    string GetLastErrorFromHelocExampleSchedules();

    bool PerformanceEnabled { get; set; }

    void CalcRateCap();

    bool IsSyncGFERequired { get; }

    bool IsCalcAllRequired { get; set; }

    void CopySafeHarborToLoan();

    void CopyLoanToSafeHarbor(int optionNumber);

    void SetVerificationTitle(MilestoneLog msLog);

    void SetVerificationTitle(MilestoneFreeRoleLog msfreeRoleLog);

    bool LOCompensationIsApplied(bool checkBroker, bool checkLoanOfficer, bool checkNameField);

    List<FundingFee> GetFundingFees(bool hideZero);

    void GetVerificationContactInformation(
      ref string contactName,
      ref string contactTitle,
      ref string contactPhone,
      ref string contactFax);

    void CalculateLatestDisclosure(DisclosureTrackingLog updatedLog, bool updateAPRFinanceChange = true);

    void CalculateLatestDisclosure2015(IDisclosureTracking2015Log updatedLog);

    void UpdateDisclosureTypeForTimeline(IDisclosureTracking2015Log updatedLog);

    string CurrentFormID { get; set; }

    List<string> ULDDExportChecking(string source);

    DateTime GetInitialBorrowerReceivedDate();

    DateTime CalculateNewDisclosureReceivedDate(
      DisclosureTrackingBase.DisclosedMethod method,
      DateTime disclosedDate,
      DateTime currentReceivedDate);

    DateTime CalculateNeweDisclosureReceivedDate(
      DisclosureTrackingBase log,
      DisclosurePackage detailInfo);

    void CalculateNew2015DisclosureReceivedDate(
      IDisclosureTracking2015Log log,
      DisclosurePackage detailInfo,
      bool isPlatFormLoan);

    Dictionary<string, object> CalculateNew2015DisclosureReceivedDate(
      IDisclosureTracking2015Log log,
      DateTime disclosed,
      DateTime presumed,
      DateTime actual,
      DisclosureTrackingBase.DisclosedMethod method);

    Dictionary<string, object> CalculateNew2015DisclosureNBOReceivedDate(
      IDisclosureTracking2015Log log,
      DateTime disclosed);

    event EventHandler OnUSDACalculationChanged;

    bool OnUSDACalculationChangedHooked { get; set; }

    void ResetULDDBorrowerInformation();

    bool IsTPOInformationEmpty();

    bool IsClosingVendorInformationEmpty(bool checkLender, bool checkInvestor, bool checkBroker);

    void ClearClosingVendorInformation(bool clearLender, bool clearInvestor, bool clearBroker);

    void SetInternalLenderLicense(
      string lenderName,
      string lenderAddress,
      string lenderCity,
      string lenderState,
      string lenderZip,
      string lenderNMLS,
      BranchExtLicensing lenderLicense,
      StateLicenseExtType stateLicense);

    void SetInternalBrokerLicense(
      string brokerName,
      string brokerAddress,
      string brokerCity,
      string brokerState,
      string brokerZip,
      string brokerNMLS,
      BranchExtLicensing brokerLicense,
      StateLicenseExtType stateLicense);

    void SetInternalInvestorLicense(
      string investorName,
      string investorAddress,
      string investorCity,
      string investorState,
      string investorZip,
      StateLicenseExtType stateLicense);

    void SetExternalLenderLicense(
      string lenderName,
      string lenderAddress,
      string lenderCity,
      string lenderState,
      string lenderZip,
      string lenderOrgState,
      string lenderOrgType,
      string lenderNMLS,
      BranchExtLicensing lenderLicense,
      StateLicenseExtType stateLicense);

    void SetExternalBrokerLicense(
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
      StateLicenseExtType stateLicense);

    void PopulateCompanyStateLicense(BranchExtLicensing licenses, bool clearFieldIfNotFound);

    DateTime GetCorrespondentLateFeeLatestBeginDate(
      List<DateTime> datesToCheck,
      int graceDays,
      bool throwException);

    DateTime GetCorrespondentLateFeeLatestBeginDate(bool throwException);

    Decimal CalcCorrespondentLateFeeCharge(
      Decimal lateFeePercent,
      Decimal lateFeeAdditional,
      int lateFeeDays);

    DateTime CalcCorrespondentLateFeeEndDate(DateTime conditionReceivedDate);

    string GetCorrespondentLateFeeOtherDateField();

    bool IsCorrespondentLateFeeDateField(string fieldID);

    void CalcDefaultTpoBankContact();

    void GetIntentToProceedDT2015Log(Dictionary<DisclosureTracking2015Log, bool> log);

    void GetIntentToProceedIDisclosureTracking2015Log(
      Dictionary<IDisclosureTracking2015Log, bool> log);

    bool Calculate2015FeeDetails(string id);

    bool Calculate2015FeeDetails(string id, string paidByID);

    bool Calculate2015FeeDetails(string id, string paidByID, bool run2015FormCalculation);

    string GetUCD(bool forLoanEstimate);

    string GetUCD(bool forLoanEstimate, bool setTotalFields);

    XmlDocument GetUCDXmlDocument(bool forLoanEstimate, bool setTotalFields);

    string GetUCD(bool forLoanEstimate, bool setTotalFields, bool fullUCD);

    Hashtable GetUsers(int[] roleids);

    bool UseNew2015GFEHUD { get; }

    void CalculateLastDisclosedCDorLE(IDisclosureTracking2015Log log);

    void CalculateFeeVariance();

    void ResetFeeVariance();

    string SetTotalLenderCredit(IDisclosureTracking2015Log log);

    string SetCannotIncrease(IDisclosureTracking2015Log log);

    void UpdateLogs();

    bool IsLEBaseLineUsed(string section);

    void CalculateFeeVarianceTotals(string id);

    Hashtable GetGFFVarianceAlertDetails();

    void LoadeSignConsentDate();

    double GetRequiredVarianceCureAmount();

    string RateLockExpirationTimeZoneSetting { get; }

    string ClosingCostExpirationTimeZoneSetting { get; }

    int ClosingCostDaysToExpire { get; }

    void ApplyLoanTypeOtherField();

    DateTime GetISWCutoffDate();

    void AttachIntermServicingFieldHandlers();

    event EventHandler FormCalculationTriggered;

    void CalcGuiDependentLogicForDDM();

    void CalcGuiDependentLogicForDDM(string updatedFieldIDsByDDM);

    bool IsPiggybackHELOC { get; }

    bool IsHELOCOrOtherLoan { get; }

    Hashtable FeeLinePaidToTrigger { get; set; }

    bool AddFeePaidToNameToCache(string fieldId, string originalValue, string newValue);

    void AddFeePaidToNameToLoan();

    bool ValidateHmdaCalculation(string id);

    bool ValidateHmdaActioForDenialReasons();

    bool IsHmdaFieldCalculated(string id);

    CalcOnDemandEnum CalcOnDemandMethod { get; }

    bool UseNewCompliance(Decimal versionToRunNewLogic);

    void CopyAlertCoCToLECDPage1(bool copyToCD);

    string CalculateRevisedDueDate(
      string changeReceivedDateFieldID,
      string revisedDueDateFieldID,
      bool includeDayChange,
      bool setField,
      bool showError);

    bool COCInLECDIsModified(bool forCD);

    void ResetTriggerCounter();

    string DumpTriggerCounter();

    void IncrementTriggerCounter(string funcName, long timeConsumed);

    LoanData CreateEDSCalculator();

    List<string[]> GetCorrespondentPaymentSchedule();

    void UpdateLenderRepresentative(LoanAssociateLog m, string type);

    void CalculateNew2015DisclosureReceivedDate(IDisclosureTracking2015Log log, bool isPlatformLoan);

    List<string[]> GetATRQMPaymentSchedule();

    bool UsePriceBasedQM(DateTime originationDate);

    void GetAMILimits(bool showMessage);

    object GetAMILimitRecords(bool showMessage);

    object GetMFILimitRecords(bool showMessage);

    void GetMFILimits(bool showMessage);

    DateTime FindClosingDateToUse();

    void SetCalculationsToSkip(bool skipTheCalc, string[] functionNamesToSkip);

    void CalcDisclosureReadyDate();

    void CalcAtAppDisclosureDate();

    void CalcAtLockDisclosureDate();

    void CalcChangeCircumstanceRequirementsDate();
  }
}
