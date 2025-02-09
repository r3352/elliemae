// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.CalculationEngine.LoanCalculator
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.InterimServicing;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.LoanUtils.CalculationServants;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.CalculationEngine
{
  public class LoanCalculator : ILoanCalculator, IDisposable
  {
    private CalculationObjects calObjs;
    private LoanData loanData;
    private bool inDisclosureCal;
    private SessionObjects sessionObjects;
    private CalcOnDemandEnum calcOnDemandMethod;
    private GfeCalculationServant _gfeCalculationServant;
    public readonly bool NoUSDAWarning;
    private Hashtable feeLinePaidToTrigger = CollectionsUtil.CreateCaseInsensitiveHashtable();
    private bool onUSDACalculationChangedHooked;
    private bool isCalcAllRequired = true;

    public event EventHandler OnUSDACalculationChanged;

    public event EventHandler FormCalculationTriggered;

    public CalcOnDemandEnum CalcOnDemandMethod => this.calcOnDemandMethod;

    public Hashtable FeeLinePaidToTrigger
    {
      set => this.feeLinePaidToTrigger = value;
      get => this.feeLinePaidToTrigger;
    }

    public LoanCalculator(
      SessionObjects sessionObjects,
      ILoanConfigurationInfo configInfo,
      LoanData loanData,
      ExternalLateFeeSettings externalOrgLateFeeSettings)
      : this(sessionObjects, configInfo, loanData, false, externalOrgLateFeeSettings)
    {
    }

    public LoanCalculator(
      SessionObjects sessionObjects,
      ILoanConfigurationInfo configInfo,
      LoanData loanData)
      : this(sessionObjects, configInfo, loanData, false, (ExternalLateFeeSettings) null)
    {
    }

    public LoanCalculator(
      SessionObjects sessionObjects,
      ILoanConfigurationInfo configInfo,
      LoanData loanData,
      bool noUSDAWarning,
      ExternalLateFeeSettings externalOrgLateFeeSettings,
      bool skipLogListCalcs = false)
    {
      if (loanData.Calculator != null)
        throw new InvalidOperationException("LoanData object already has a calculator attached");
      this.sessionObjects = sessionObjects;
      this.loanData = loanData;
      PerformanceMeter.Current.AddCheckpoint("Starting to create CalculationObjects", 69, ".ctor", "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\CalculationEngine\\LoanCalculator.cs");
      this.calObjs = new CalculationObjects(sessionObjects, configInfo, loanData, skipLogListCalcs);
      PerformanceMeter.Current.AddCheckpoint("Finished creating CalculationObjects", 71, ".ctor", "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\CalculationEngine\\LoanCalculator.cs");
      this.calObjs.ExternalLateFeeSettings = externalOrgLateFeeSettings != null ? externalOrgLateFeeSettings : this.sessionObjects.GetCachedExternalOrgLateFeeSettings(this.loanData.GetField("TPO.X15"));
      this.calObjs.ToolCal.ExceededCountyLimitEvent += new EventHandler(this.loanData.ExceededLoanAmountEvent);
      this.calObjs.RegzCal.CurrentAPRChanged += new EventHandler(this.loanData.CurrentAPRChanged);
      this.NoUSDAWarning = noUSDAWarning;
      if (!noUSDAWarning)
        this.calObjs.USDACal.OnUSDAChanged += new EventHandler(this.usdaCal_OnUSDAChanged);
      this.loanData.AttachCalculator((ILoanCalculator) this);
      this._gfeCalculationServant = new GfeCalculationServant((ILoanModelProvider) this.calObjs.GFECal, (ISettingsProvider) new EllieMae.EMLite.LoanUtils.CalculationServants.SystemSettings(sessionObjects));
    }

    private void usdaCal_OnUSDAChanged(object sender, EventArgs e)
    {
      if (this.OnUSDACalculationChanged == null)
        return;
      this.OnUSDACalculationChanged(sender, e);
    }

    public void SetExcludeBorrowerClosingCost()
    {
      this.calObjs.NewHud2015Cal.CalcSetExcludeBorrowerClosingCosts((string) null, (string) null);
    }

    public void CalculateNewLoan() => this.calObjs.VERIFCal.CalculateNewLoan();

    public void UpdateCurrentMailingAddress()
    {
      this.calObjs.D1003Cal.UpdateCurrentMailingAddress();
    }

    public void CalculateFunder(string id, string val)
    {
      this.calObjs.GFECal.CalculateFunder(id, val);
    }

    public void CalculateInterimServicing(bool current)
    {
      this.calObjs.ToolCal.CalcInterimServicing(current);
    }

    public void AttachIntermServicingFieldHandlers()
    {
      this.calObjs.ToolCal.AttachIntermServicingFieldHandlers();
    }

    public ServicingSummaryViewModel GetInterimServicingAnnualSummary(int year)
    {
      return this.calObjs.ToolCal.GetInterimServicingAnnualSummary(year);
    }

    public List<List<string[]>> GetHelocExampleSchedules()
    {
      return this.calObjs.HelocCal.GetHelocExampleSchedules();
    }

    public List<string[]> GetCorrespondentPaymentSchedule()
    {
      return this.calObjs.ToolCal.getPaymentSchedule();
    }

    public void CalculateHELOCQualifyingPayment()
    {
      this.calObjs.RegzCal.CalculateHELOCPayment((string) null, (string) null);
    }

    public bool UseWorstCaseScenario
    {
      get => this.calObjs.RegzCal.UseWorstCaseScenario;
      set => this.calObjs.RegzCal.UseWorstCaseScenario = value;
    }

    public bool UseInterimServicingScenario
    {
      get => this.calObjs.RegzCal.UseInterimServicingScenario;
      set => this.calObjs.RegzCal.UseInterimServicingScenario = value;
    }

    public bool UseBestCaseScenario
    {
      get => this.calObjs.RegzCal.UseBestCaseScenario;
      set => this.calObjs.RegzCal.UseBestCaseScenario = value;
    }

    public bool UseNew2015GFEHUD => this.calObjs.NewHud2015Cal.UseNew2015GFEHUD;

    public bool OnUSDACalculationChangedHooked
    {
      get => this.onUSDACalculationChangedHooked;
      set => this.onUSDACalculationChangedHooked = value;
    }

    public void SpecialCalculation(CalculationActionID action)
    {
      this.SpecialCalculation(action, (string) null, (string) null);
    }

    public void AttachLOCompensation(LOCompensationSetting loCompensationSetting)
    {
      this.calObjs.NewHudCal.LOCompensationSetting = loCompensationSetting;
    }

    public bool LOCompensationIsApplied(
      bool checkBroker,
      bool checkLoanOfficer,
      bool checkNameField)
    {
      return this.calObjs.ToolCal.LOCompensationIsApplied(checkBroker, checkLoanOfficer, checkNameField);
    }

    public void InitializingCalculation(string id, string val)
    {
      if (id != null && string.Compare(id, "VerifyingPOCPTCFields", true) != 0)
        return;
      this.calObjs.NewHudCal.IsPOCPTCVerified = false;
    }

    public void SpecialCalculation(CalculationActionID action, string id, string val)
    {
      switch (action)
      {
        case CalculationActionID.UpdatePurchaseAdviceBalance:
          this.calObjs.ToolCal.UpdatePurchaseAdviceBalance(id, val);
          break;
        case CalculationActionID.UpdateTotalLoanAmountFromLock:
          this.calObjs.PrequalCal.CalcLoanAmount(id, val);
          break;
        case CalculationActionID.UpdateBorrowerOnLockRequestForm:
          this.calObjs.D1003Cal.CopyInfoToLockRequest(id, val);
          break;
        case CalculationActionID.CopyLoanToLockRequestAdditionalFields:
          this.calObjs.D1003Cal.CopyLockRequestAdditionalFields(id, val);
          break;
        case CalculationActionID.GFEFormCalculation:
          this.calObjs.GFECal.FormCal((string) null, id, val);
          break;
        case CalculationActionID.UpdateCorrespondentPurchaseAdviceBalance:
          this.calObjs.ToolCal.UpdateCorrespondentPurchaseAdvice(id, val);
          break;
        case CalculationActionID.UpdateCorrespondentFees:
          this.calObjs.ToolCal.UpdateCorrespondentFees(id, val);
          break;
      }
    }

    public void CalculateInvestorStatus()
    {
      this.calObjs.ToolCal.CalcInvestorStatus((string) null, (string) null);
    }

    public LoanData CalculateWorstCaseScenario() => this.CalculateWorstCaseScenario(true);

    public LoanData CreateEDSCalculator()
    {
      LoanData loanData = new LoanData(this.loanData, this.loanData.Settings, false);
      if (loanData.SnapshotProvider == null && this.loanData.SnapshotProvider != null)
      {
        loanData.AttachSnapshotProvider(this.loanData.SnapshotProvider);
        loanData.IncludeSnapshotInXML = true;
        loanData.GetCalculationDTSnapshotForLoan();
      }
      LoanCalculator loanCalculator = new LoanCalculator(this.sessionObjects, this.calObjs.LoanConfiguration, loanData, true, this.calObjs.ExternalLateFeeSettings);
      return loanData;
    }

    public LoanData CalculateWorstCaseScenario(bool setField)
    {
      return this.calObjs.NewHudCal.CalculateWorstCaseScenario(setField);
    }

    public PaymentScheduleSnapshot CalculateInterimServicingPaymentSchedule(bool setField)
    {
      return this.calObjs.NewHudCal.CalculateInterimServicingPaymentSchedule(setField);
    }

    public PaymentScheduleSnapshot GetBestCaseScenarioPaymentSchedule(LoanData bestCaseLoan)
    {
      return this.calObjs.NewHudCal.GetBestCaseScenarioPaymentSchedule(bestCaseLoan);
    }

    public void CalculateProjectedPaymentTable() => this.CalculateProjectedPaymentTable(true);

    public void CalculateProjectedPaymentTable(bool setField)
    {
      this.calObjs.NewHudCal.CalculateProjectedPaymentTable(setField);
    }

    public void SetInternalLenderLicense(
      string lenderName,
      string lenderAddress,
      string lenderCity,
      string lenderState,
      string lenderZip,
      string lenderNMLS,
      BranchExtLicensing lenderLicense,
      StateLicenseExtType stateLicense)
    {
      this.calObjs.ToolCal.SetInternalLenderLicense(lenderName, lenderAddress, lenderCity, lenderState, lenderZip, lenderNMLS, lenderLicense, stateLicense);
    }

    public void SetInternalBrokerLicense(
      string brokerName,
      string brokerAddress,
      string brokerCity,
      string brokerState,
      string brokerZip,
      string brokerNMLS,
      BranchExtLicensing brokerLicense,
      StateLicenseExtType stateLicense)
    {
      this.calObjs.ToolCal.SetInternalBrokerLicense(brokerName, brokerAddress, brokerCity, brokerState, brokerZip, brokerNMLS, brokerLicense, stateLicense);
    }

    public void SetInternalInvestorLicense(
      string investorName,
      string investorAddress,
      string investorCity,
      string investorState,
      string investorZip,
      StateLicenseExtType stateLicense)
    {
      this.calObjs.ToolCal.SetInternalInvestorLicense(investorName, investorAddress, investorCity, investorState, investorZip, stateLicense);
    }

    public void SetExternalLenderLicense(
      string lenderName,
      string lenderAddress,
      string lenderCity,
      string lenderState,
      string lenderZip,
      string lenderOrgState,
      string lenderOrgType,
      string lenderNMLS,
      BranchExtLicensing lenderLicense,
      StateLicenseExtType stateLicense)
    {
      this.calObjs.ToolCal.SetExternalLenderLicense(lenderName, lenderAddress, lenderCity, lenderState, lenderZip, lenderOrgState, lenderOrgType, lenderNMLS, lenderLicense, stateLicense);
    }

    public void SetExternalBrokerLicense(
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
      this.calObjs.ToolCal.SetExternalBrokerLicense(brokerName, brokerAddress, brokerCity, brokerState, brokerZip, brokerOrgState, brokerOrgType, brokerTaxID, brokerNMLS, brokerLicense, stateLicense);
    }

    public bool IsTPOInformationEmpty() => this.calObjs.ToolCal.IsTPOInformationEmpty();

    public bool IsClosingVendorInformationEmpty(
      bool checkLender,
      bool checkInvestor,
      bool checkBroker)
    {
      return this.calObjs.ToolCal.IsClosingVendorInformationEmpty(checkLender, checkInvestor, checkBroker);
    }

    public void PopulateCompanyStateLicense(BranchExtLicensing licenses, bool clearFieldIfNotFound)
    {
      this.calObjs.D1003Cal.PopulateCompanyStateLicense(licenses, clearFieldIfNotFound);
    }

    public Decimal CalcCorrespondentLateFeeCharge(
      Decimal lateFeePercent,
      Decimal lateFeeAdditional,
      int lateFeeDays)
    {
      return this.calObjs.ToolCal.CalcCorrespondentLateFeeCharge(lateFeePercent, lateFeeAdditional, lateFeeDays);
    }

    public DateTime GetCorrespondentLateFeeLatestBeginDate(bool throwException)
    {
      return this.calObjs.ToolCal.GetCorrespondentLateFeeLatestBeginDate(throwException);
    }

    public void CalcDisclosureReadyDate()
    {
      this.calObjs.Cal.CalcDisclosureReadyDate((string) null, (string) null);
    }

    public void CalcAtAppDisclosureDate()
    {
      this.calObjs.Cal.CalcAtAppDisclosureDate((string) null, (string) null);
    }

    public void CalcAtLockDisclosureDate()
    {
      this.calObjs.Cal.CalcAtLockDisclosureDate((string) null, (string) null);
    }

    public void CalcChangeCircumstanceRequirementsDate()
    {
      this.calObjs.Cal.CalcChangeCircumstanceRequirementsDate((string) null, (string) null);
    }

    public DateTime GetCorrespondentLateFeeLatestBeginDate(
      List<DateTime> datesToCheck,
      int graceDays,
      bool throwException)
    {
      return this.calObjs.ToolCal.GetCorrespondentLateFeeLatestBeginDate(datesToCheck, graceDays, throwException);
    }

    public DateTime CalcCorrespondentLateFeeEndDate(DateTime conditionReceivedDate)
    {
      return this.calObjs.ToolCal.CalcCorrespondentLateFeeEndDate(conditionReceivedDate);
    }

    public bool IsCorrespondentLateFeeDateField(string fieldID)
    {
      if (this.calObjs.ExternalLateFeeSettings == null)
        return false;
      if (fieldID == "3918" && (this.calObjs.ExternalLateFeeSettings.GracePeriodLaterOf & 1) == 1 || fieldID == "3919" && (this.calObjs.ExternalLateFeeSettings.GracePeriodLaterOf & 8) == 8 || fieldID == "3920" && (this.calObjs.ExternalLateFeeSettings.GracePeriodLaterOf & 2) == 2 || fieldID == "3926" && (this.calObjs.ExternalLateFeeSettings.GracePeriodLaterOf & 4) == 4)
        return true;
      if ((this.calObjs.ExternalLateFeeSettings.GracePeriodLaterOf & 16) == 16 && this.calObjs.ExternalLateFeeSettings.OtherDate.StartsWith("Fields."))
      {
        string strB = this.calObjs.ExternalLateFeeSettings.OtherDate.Substring(7);
        if (string.Compare(fieldID, strB, true) == 0)
          return true;
      }
      return false;
    }

    public string GetCorrespondentLateFeeOtherDateField()
    {
      if (this.calObjs.ExternalLateFeeSettings == null && this.loanData != null && this.loanData.GetField("TPO.X15") != string.Empty)
        this.calObjs.ExternalLateFeeSettings = this.sessionObjects.ConfigurationManager.GetExternalOrgLateFeeSettings(this.loanData.GetField("TPO.X15"), true);
      if (this.calObjs.ExternalLateFeeSettings == null)
        return (string) null;
      if ((this.calObjs.ExternalLateFeeSettings.GracePeriodLaterOf & 16) == 16 && this.calObjs.ExternalLateFeeSettings.OtherDate.StartsWith("Fields."))
      {
        string feeOtherDateField = this.calObjs.ExternalLateFeeSettings.OtherDate.Substring(7);
        if (feeOtherDateField != string.Empty)
          return feeOtherDateField;
      }
      return (string) null;
    }

    public bool Calculate2015FeeDetails(string id)
    {
      return this.Calculate2015FeeDetails(id, (string) null, true);
    }

    public bool Calculate2015FeeDetails(string id, string paidByID)
    {
      return this.Calculate2015FeeDetails(id, paidByID, true);
    }

    public bool Calculate2015FeeDetails(string id, string paidByID, bool run2015FormCalculation)
    {
      string[] pocFields = (string[]) null;
      if (paidByID == null)
      {
        switch (id)
        {
          case "NEWHUD.X808":
            paidByID = "NEWHUD2.X78";
            break;
          case "NEWHUD.X572":
            paidByID = "NEWHUD.X744";
            break;
          case "NEWHUD.X639":
            paidByID = "NEWHUD.X745";
            break;
        }
      }
      if (paidByID != null && HUDGFE2010Fields.PAIDBYPOPTFIELDS.ContainsKey((object) paidByID))
        pocFields = (string[]) HUDGFE2010Fields.PAIDBYPOPTFIELDS[(object) paidByID];
      if (pocFields == null)
      {
        for (int index1 = 0; index1 < HUDGFE2010Fields.WHOLEPOC_FIELDS.Count; ++index1)
        {
          string[] strArray = HUDGFE2010Fields.WHOLEPOC_FIELDS[index1];
          for (int index2 = 0; index2 < strArray.Length; ++index2)
          {
            if (string.Compare(strArray[index2], id, true) == 0 || id == "388" && strArray[HUDGFE2010Fields.PTCPOCINDEX_BORPAID] == "454" || (id == "389" || id == "1620") && strArray[HUDGFE2010Fields.PTCPOCINDEX_BORPAID] == "439" || (id == "NEWHUD.X223" || id == "NEWHUD.X224") && strArray[HUDGFE2010Fields.PTCPOCINDEX_BORPAID] == "NEWHUD.X225" || (id == "NEWHUD.X1141" || id == "NEWHUD.X1225") && strArray[HUDGFE2010Fields.PTCPOCINDEX_BORPAID] == "NEWHUD.X1149" || (id == "NEWHUD.X1143" || id == "NEWHUD.X1226") && strArray[HUDGFE2010Fields.PTCPOCINDEX_BORPAID] == "NEWHUD.X1149" || (id == "NEWHUD.X1150" || id == "NEWHUD.X1227") && strArray[HUDGFE2010Fields.PTCPOCINDEX_BORPAID] == "NEWHUD.X1151" || id == "NEWHUD.X1154" && strArray[HUDGFE2010Fields.PTCPOCINDEX_BORPAID] == "NEWHUD.X1155" || id == "NEWHUD.X1158" && strArray[HUDGFE2010Fields.PTCPOCINDEX_BORPAID] == "NEWHUD.X1159" || id == "NEWHUD.X1162" && strArray[HUDGFE2010Fields.PTCPOCINDEX_BORPAID] == "NEWHUD.X1163" || (id == "332" || id == "L244" || id == "L245") && strArray[HUDGFE2010Fields.PTCPOCINDEX_BORPAID] == "334" || id == "L251" && strArray[HUDGFE2010Fields.PTCPOCINDEX_BORPAID] == "642" || (id == "1387" || id == "230") && strArray[HUDGFE2010Fields.PTCPOCINDEX_BORPAID] == "656" || id == "1296" && strArray[HUDGFE2010Fields.PTCPOCINDEX_BORPAID] == "338" || (id == "1386" || id == "231") && strArray[HUDGFE2010Fields.PTCPOCINDEX_BORPAID] == "655" || (id == "L267" || id == "L268") && strArray[HUDGFE2010Fields.PTCPOCINDEX_BORPAID] == "L269" || (id == "1629" || id == "1630") && strArray[HUDGFE2010Fields.PTCPOCINDEX_BORPAID] == "1631" || (id == "340" || id == "253") && strArray[HUDGFE2010Fields.PTCPOCINDEX_BORPAID] == "658" || (id == "341" || id == "254") && strArray[HUDGFE2010Fields.PTCPOCINDEX_BORPAID] == "659" || (id == "NEWHUD.X1706" || id == "NEWHUD.X1707") && strArray[HUDGFE2010Fields.PTCPOCINDEX_BORPAID] == "NEWHUD.X1708" || (id == "NEWHUD.X1141" || id == "NEWHUD.X1225" || id == "NEWHUD.X1143" || id == "NEWHUD.X1226" || id == "NEWHUD.X1146" || id == "NEWHUD.X1148") && strArray[HUDGFE2010Fields.PTCPOCINDEX_BORPAID] == "NEWHUD.X1149")
            {
              pocFields = strArray;
              break;
            }
          }
          if (pocFields != null)
            break;
        }
      }
      bool flag = pocFields != null && this.calObjs.NewHud2015FeeDetailCal.CalcFeeDetail(id, pocFields);
      if (flag & run2015FormCalculation)
        this.calObjs.NewHudCal.FormCal(id, this.loanData.GetField(id));
      return flag;
    }

    public string CurrentFormID
    {
      get => this.calObjs.CurrentFormID;
      set => this.calObjs.CurrentFormID = value;
    }

    public bool ForceDefaultLinkSync
    {
      get => this.calObjs.ForceDefaultLinkSync;
      set => this.calObjs.ForceDefaultLinkSync = value;
    }

    public bool IsPiggybackHELOC => this.calObjs.D1003Cal.IsPiggybackHELOC;

    public bool IsHELOCOrOtherLoan => this.calObjs.D1003Cal.IsHELOCOrOtherLoan;

    public void CopyAlertCoCToLECDPage1(bool copyToCD)
    {
      this.calObjs.AlertCoCLECDCalc.CopyAlertCoCToLECDPage1(copyToCD);
    }

    public bool COCInLECDIsModified(bool forCD)
    {
      return this.calObjs.AlertCoCLECDCalc.COCInLECDIsModified(forCD);
    }

    public string CalculateRevisedDueDate(
      string changeReceivedDateFieldID,
      string revisedDueDateFieldID,
      bool includeDayChange,
      bool setField,
      bool showError)
    {
      return this.calObjs.NewHudCal.CalculateRevisedDueDate(changeReceivedDateFieldID, revisedDueDateFieldID, includeDayChange, setField, showError);
    }

    public void FormCalculation(string calculationID)
    {
      this.FormCalculation(calculationID, (string) null, (string) null);
    }

    public void FormCalculation(string formID, string id, string val)
    {
      this.loanData.Dirty = true;
      this.InitializingCalculation((string) null, (string) null);
      if (formID.ToUpper().StartsWith("GETESCROWTITLE;") || formID.ToUpper().StartsWith("GETTAX;"))
      {
        string[] strArray = formID.Split(';');
        if (strArray != null && strArray.Length == 3)
        {
          formID = strArray[0];
          id = strArray[1];
          val = strArray[2];
        }
      }
      else if (formID.ToUpper().StartsWith("2015FEEDETAILS:"))
      {
        id = formID.Substring(formID.IndexOf(":") + 1);
        formID = "2015FEEDETAILS";
      }
      formID = formID.ToUpper();
      switch (formID)
      {
        case "2015FEEDETAILS":
          this.Calculate2015FeeDetails(id);
          break;
        case "906":
        case "USDAWORKSHEET":
          this.calObjs.USDACal.CalcIncomeWorksheet(id, val);
          break;
        case "ADJUSTMENTS_AND_OTHER_CREDITS":
          this.calObjs.NewHud2015Cal.CalcAdjustmentsDetailTotalMigration((string) null, (string) null);
          break;
        case "APPLYPARTIALEXEMPTION":
          this.calObjs.HMDACal.CalcDefaultFields((string) null, (string) null);
          this.calObjs.HMDACal.CalcCreditScoreForDecisionMaking((string) null, (string) null);
          this.calObjs.HMDACal.CalcCreditScoringModel((string) null, (string) null);
          break;
        case "CALCAUTOMATEDDISCLOSURES":
          this.calObjs.Cal.CalcDisclosureReadyDate((string) null, (string) null);
          this.calObjs.Cal.CalcAtAppDisclosureDate((string) null, (string) null);
          this.calObjs.Cal.CalcAtLockDisclosureDate((string) null, (string) null);
          this.calObjs.Cal.CalcChangeCircumstanceRequirementsDate((string) null, (string) null);
          break;
        case "CALCFEEDETAILINDICATORS":
          this.calObjs.NewHud2015FeeDetailCal.calculate_TaxesAndInsuranceIndicators(id, val);
          break;
        case "CALCMATURITY":
          this.calObjs.RegzCal.CalcMaturityDate((string) null, (string) null);
          break;
        case "CALCULATEHOEPAAPR":
          this.calObjs.GFECal.CalcHOEPAAPR();
          break;
        case "CALCULATEULI":
          this.calObjs.D1003Cal.CalcUli((string) null, (string) null);
          break;
        case "CALCURLALOANIDENTIFIER":
          this.calObjs.D1003URLA2020Cal.CalcURLALoanIdentifier((string) null, (string) null);
          break;
        case "CD2.XSTB":
          this.calObjs.VACal.CalcVARecoupment((string) null, (string) null);
          break;
        case "CLEAR801LOCOMP":
          this.calObjs.ToolCal.Clear801LOCompensationFields(!(id == "NEWHUD.X1718"));
          break;
        case "CLEARBORROWERBUYDOWN":
          this.calObjs.RegzCal.CleanBuydownFields("CLEARBORROWERBUYDOWN", (string) null);
          break;
        case "CLEARLINE819":
          this.calObjs.USDACal.ClearLine819(id, val);
          break;
        case "CLEARLOCOMP":
          this.calObjs.ToolCal.ClearLOCompensation(true, true, false, false);
          break;
        case "CLEARLOCOMPINBROKER":
          this.calObjs.ToolCal.ClearLOCompensation(true, false, false, true);
          break;
        case "CLEARLOCOMPINBROKERKEEPNAME":
          this.calObjs.ToolCal.ClearLOCompensation(true, false, false, false);
          break;
        case "CLEARLOCOMPINOFFICER":
          this.calObjs.ToolCal.ClearLOCompensation(false, true, false, true);
          break;
        case "CLEARLOCOMPINOFFICERKEEPNAME":
          this.calObjs.ToolCal.ClearLOCompensation(false, true, false, false);
          break;
        case "CLEARTPOINFORMATION":
          this.calObjs.ToolCal.ClearTPOInformationTool();
          break;
        case "CLEARULDD":
          this.calObjs.ULDDExpCal.ULDDCLEAR();
          break;
        case "CLOSINGCOSTANDRATELOCKEXPIRATION":
          this.calObjs.NewHud2015Cal.SetDefaultValuesForClosingCostExpiration(id, val);
          this.calObjs.NewHud2015Cal.SetDefaultValuesForRateLockExpiration(id, val);
          break;
        case "CLOSINGDISCLOSUREPAGE3":
          this.calObjs.NewHud2015Cal.CalcCdPage3Liabilities("", "");
          this.calObjs.D1003URLA2020Cal.CalcNonSubjectPropertyDebtsToBePaidOffAmount((string) null, (string) null);
          this.calObjs.NewHud2015Cal.CalcCDPage3AlternateCashToCloseLoanEstimate((string) null, (string) null);
          this.calObjs.NewHud2015Cal.CalcCDPage3TotalPurchasePayoffsIncluded((string) null, (string) null);
          this.calObjs.NewHud2015Cal.CalcAdjustmentsUcdTotal((string) null, (string) null);
          this.calObjs.NewHud2015Cal.CalcTotalAdjustmentAndOtherCredits((string) null, (string) null);
          this.calObjs.NewHud2015Cal.CalcLoanEstimate_EstimatedCashToClose((string) null, (string) null);
          this.calObjs.NewHud2015Cal.CalcCDPage3StdCashToCloseLoanEstimate((string) null, (string) null);
          this.calObjs.NewHud2015Cal.CalcCDPage3TotalK((string) null, (string) null);
          this.calObjs.NewHud2015Cal.CalcCDPage3TotalN((string) null, (string) null);
          this.calObjs.NewHud2015Cal.CalcCDPage3TotalL((string) null, (string) null);
          this.calObjs.NewHud2015Cal.CalcCDPage3TotalM((string) null, (string) null);
          this.calObjs.NewHud2015Cal.CalcLoanEstimateThirdPartyPaymentsNotOtherwiseDisclosed((string) null, (string) null);
          this.calObjs.NewHud2015Cal.CalcCDPage3StdCashToCloseFinal((string) null, (string) null);
          this.calObjs.NewHud2015Cal.CalcCDPage3AlternateCashToCloseFinal((string) null, (string) null);
          break;
        case "COPYALERTCOCTOCD":
          this.CopyAlertCoCToLECDPage1(true);
          break;
        case "COPYALERTCOCTOLE":
          this.CopyAlertCoCToLECDPage1(false);
          break;
        case "COPYATRQMINCOME":
          this.calObjs.D1003URLA2020Cal.CopyATRQMIncome((string) null, (string) null);
          this.calObjs.ATRQMCal.CalcAppendixQIncome("popup", (string) null);
          break;
        case "COPYAUSTOLOAN":
          this.calObjs.ATRQMCal.CopyAUSToLoan();
          break;
        case "COPYFROM1033":
          this.calObjs.NewHud2015Cal.Copy1033ToLender(false);
          break;
        case "COPYFROMREALESTATEBROKER(B)":
          this.calObjs.NewHud2015Cal.CopyBusinessToFileContact("RealEstateBroker(B)");
          break;
        case "COPYFROMREALESTATEBROKER(S)":
          this.calObjs.NewHud2015Cal.CopyBusinessToFileContact("RealEstateBroker(S)");
          break;
        case "COPYHUD2010TOGFE2010":
          this.calObjs.NewHudCal.CopyHUD2010ToGFE2010(id, id, false);
          break;
        case "COPYITEMIZATIONTOMLDS":
          this.CopyGFEToMLDS();
          break;
        case "COPYLIABILIESTOCDPG3":
          this.calObjs.NewHud2015Cal.CalcGenrateCD3Liabilities((string) null, (string) null);
          break;
        case "COPYLINE1003TOLINE1010":
          this.calObjs.USDACal.CopyPOCPTCAPRFromLine1003ToLine1010(id, val);
          break;
        case "COPYLINE819TOLINE902":
          this.calObjs.USDACal.CopyPOCPTCAPRFromLine819ToLine902(id, val);
          break;
        case "COPYLINE902TOLINE819":
          this.calObjs.USDACal.CopyPOCPTCAPRFromLine902ToLine819(id, val);
          break;
        case "COPYLOCOMPTO2010ITEMIZATION":
          this.calObjs.ToolCal.CopyLOCompTo2010Itemization(!(id == "NEWHUD.X1718"));
          break;
        case "COPYLOCOMPTOLR":
          this.calObjs.PrequalCal.CalcOnSaveCopyBorrowerLenderPaid((string) null, (string) null);
          break;
        case "CORRESPONDENTPURCHASEADVICE":
          this.calObjs.ToolCal.UpdateCorrespondentPurchaseAdvice(id, val);
          break;
        case "CPA_ESCROWDETAILS":
          this.calObjs.ToolCal.CalcCPAEscrowDetails(id, val);
          break;
        case "DRW123K":
          this.calObjs.FHACal.CalcDraw203K(id, val);
          break;
        case "EXPORTUCD":
          new UCDXmlExporter(this.loanData).TriggerCalculation();
          break;
        case "FHA203K":
          this.calObjs.FHACal.CalcFHA203K(id, val);
          break;
        case "FM1084":
          this.calObjs.FM1084Cal.FormCal();
          break;
        case "GETAGENCIES":
          this.calObjs.Cal.CalcHomeCounseling("GETAGENCIES", (string) null);
          break;
        case "GETAMI":
          this.GetAMILimits(false);
          break;
        case "GETESCROWTITLE":
          this.calObjs.SystemTableCal.ExecuteTable_TitleEscrow(id, val);
          break;
        case "GETMFI":
          this.GetMFILimits(false);
          break;
        case "GETMI":
          this.calObjs.SystemTableCal.ExecuteTable_MI(false);
          break;
        case "GETTAX":
          this.calObjs.SystemTableCal.ExecuteTable_CityStateTax(id, val);
          break;
        case "GFE":
        case "REGZ":
        case "SEC32":
          this.calObjs.GFECal.FormCal(formID, id, val);
          break;
        case "HELOCDRAWCALC":
          this.calObjs.Cal.CalcHelocDrawAmount((string) null, (string) null);
          break;
        case "HELOCLOANCALC":
          this.calObjs.Cal.CalcHelocLoanAmount((string) null, (string) null);
          break;
        case "HMDACREDITSCOREDECISION":
          this.calObjs.HMDACal.CalcCreditScoreForDecisionMaking((string) null, (string) null);
          break;
        case "HMDACREDITSCOREMODEL":
          this.calObjs.HMDACal.CalcCreditScoringModel((string) null, (string) null);
          break;
        case "HMDAINCOME":
          this.calObjs.HMDACal.CalcIncome((string) null, (string) null);
          break;
        case "HMDASORTING":
          this.calObjs.HMDACal.CalcSortEthnicityCategory("IMPORT", (string) null);
          this.calObjs.HMDACal.CalcSortRaceCategory("IMPORT", (string) null);
          break;
        case "HUD1ES":
          this.calObjs.Hud1Cal.FormCal();
          break;
        case "HUD1PG2_2010":
        case "HUD1PG3_2010":
        case "REGZGFEHUD":
        case "REGZGFE_2010":
          this.calObjs.NewHudCal.FormCal(id, val);
          if (this.UseNew2015GFEHUD)
          {
            new UCDXmlExporter(this.loanData).TriggerCalculation();
            this.calObjs.NewHud2015Cal.CalcCDPage3AlternateCashToCloseLoanEstimate(id, val);
            this.calObjs.NewHud2015Cal.CalcLoanEstimateEstimatedCashToCloseAV(id, val);
            this.calObjs.NewHud2015Cal.CalcLoanEstimate_ClosingCostsFinanced(id, val);
            this.calObjs.NewHud2015Cal.CalcLECashToCloseSellerCredit((string) null, (string) null);
            this.calObjs.NewHud2015Cal.PopulateLoanEstimate_LenderBrokerInfo((string) null, (string) null);
            this.calObjs.NewHud2015Cal.CalcCdPage3Liabilities(id, val);
            this.calObjs.NewHud2015Cal.Calc60Payments(id, val);
            this.calObjs.NewHud2015Cal.CalcCDPage3AlternateCashToCloseLoanEstimate(id, val);
            this.calObjs.NewHud2015Cal.CalcCDPage3StdCashToCloseFinal(id, val);
            this.calObjs.NewHud2015Cal.CalcClosingDisclosureTotalClosingCosts(id, val);
            this.calObjs.HMDACal.CalcAllLoanCostsBy2015Indicator(id, val);
            this.calObjs.ATRQMCal.PopulateQMStatusInNMLSForm(id, val);
          }
          this.calObjs.NewHudCal.CalTradeOffTable(id, val);
          this.calObjs.PrequalCal.CalcLockRequestLoan(id, val);
          break;
        case "IRPCEXAMPLE":
          this.calObjs.NewHudCal.CalcInterestRateChange((string) null, (string) null);
          break;
        case "IRS1098":
          this.calObjs.RegzCal.CalcIRS1098((string) null, (string) null);
          break;
        case "ISLSECONDARYFILE":
          this.calObjs.Cal.CalcIsLSSecondaryFile((string) null, (string) null);
          break;
        case "ITEMIZATION_SECTION1000":
          this.calObjs.GFECal.CalcPrepaid("ITEMIZATION_SECTION1000", (string) null);
          break;
        case "ITEMIZEESCROW":
          this.calObjs.Hud1Cal.PopulateItemizedFields();
          break;
        case "LENDERBROKERDATA":
          this.calObjs.D1003Cal.CopyLenderToSSNCompany(id, val);
          break;
        case "LINKCONSTLINKLOAN":
        case "LINKLOAN":
        case "LINKPIGGYBACKLOAN":
          this.calObjs.Cal.CalcIsLSSecondaryFile((string) null, (string) null);
          this.calObjs.Cal.CalcLoanLinkSyncType((string) null, (string) null);
          this.calObjs.ToolCal.SyncConstructionLinkSyncDefaultData((string) null, (string) null);
          this.calObjs.VERIFCal.CalcHelocLineTotal((string) null, (string) null);
          this.calObjs.HMDACal.CalcHMDAReporting((string) null, (string) null);
          break;
        case "LINKVOAL":
          this.calObjs.D1003URLA2020Cal.CreateLinkedVoal((string) null, (string) null);
          this.calObjs.VERIFCal.CalcAppliedToDownpayment((string) null, (string) null);
          this.calObjs.VERIFCal.CalSubFin((string) null, (string) null);
          this.calObjs.NewHudCal.CalcClosingCost((string) null, (string) null);
          break;
        case "LINKVOALEXISTING":
          this.calObjs.D1003URLA2020Cal.CreateLinkedVoal("LINKEXISTING", (string) null);
          this.calObjs.VERIFCal.CalcAppliedToDownpayment((string) null, (string) null);
          this.calObjs.VERIFCal.CalSubFin((string) null, (string) null);
          this.calObjs.NewHudCal.CalcClosingCost((string) null, (string) null);
          break;
        case "LOANCOMP":
          this.calObjs.PrequalCal.CalcLoanComparison(id, val);
          break;
        case "LOANESTIMATEPAGE2":
          this.calObjs.NewHud2015Cal.CalcEstimatedCashToCloseTable();
          break;
        case "LOANLINKSYNCTYPE":
          this.calObjs.Cal.CalcLoanLinkSyncType((string) null, (string) null);
          break;
        case "MCAWPUR":
          if (id == "1228")
          {
            this.calObjs.FHACal.CalcEEMWorksheet(id, val);
            this.calObjs.FHACal.CalcEEM("MCAWPUR" + id, val);
            break;
          }
          break;
        case "MCAWREFI":
          if (id == "1228")
          {
            this.calObjs.FHACal.CalcEEMWorksheet(id, val);
            this.calObjs.FHACal.CalcEEM("MCAWREFI" + id, val);
            break;
          }
          break;
        case "MIGRATECDPAGE3LIABILITIES":
          this.calObjs.NewHud2015Cal.MigrateCdPage3Liabilities();
          break;
        case "MIP":
          this.calObjs.PrequalCal.CalcMIP(id, val);
          break;
        case "MLDS":
          this.calObjs.MLDSCal.FormCal(id, val);
          break;
        case "NMLSINFO":
          this.calObjs.D1003Cal.PopulateLoanAmountToNmlsApplicationAmounts(id, val);
          break;
        case "POPULATESUBJECTPROPERTYADDRESS":
          this.calObjs.Cal.PopulateSubjectPropertyAddress(id, val);
          break;
        case "PURCHASEADVICE":
          this.calObjs.ToolCal.CalcRateLock(id, val);
          this.calObjs.ToolCal.UpdatePurchaseAdviceBalance(id, val);
          break;
        case "QMAPR":
          this.CalcATRQMPaymentSchedule();
          break;
        case "RECALCULATEHMDA":
          this.calObjs.HMDACal.CalcHMDAfields((string) null, (string) null);
          this.calObjs.HMDACal.CalcDefaultFields((string) null, (string) null);
          break;
        case "REMOVELINK":
          this.calObjs.Cal.CalcIsLSSecondaryFile((string) null, (string) null);
          this.calObjs.Cal.CalcLoanLinkSyncType((string) null, (string) null);
          this.calObjs.VERIFCal.CalcHelocLineTotal((string) null, (string) null);
          this.calObjs.D1003URLA2020Cal.RemovelinkedVoal((string) null, (string) null);
          break;
        case "RENTOWN":
          this.calObjs.PrequalCal.CalcRentOwn(id, val);
          break;
        case "SETTLEMENTAGENT":
          this.calObjs.NewHud2015Cal.CopyBusinessToFileContact("SettlementAgent");
          break;
        case "SHIFTPAYMENTFREQUENCE":
          if (!this.calObjs.EnableTriggerToRunEscrowDateCalc)
          {
            DateTime date1 = Utils.ParseDate((object) this.loanData.GetField("682"));
            DateTime date2 = Utils.ParseDate((object) val);
            if (date1 != DateTime.MinValue && date2 != DateTime.MinValue)
            {
              this.calObjs.Hud1Cal.ShiftPaymentFrequence(date1, date2);
              break;
            }
            break;
          }
          break;
        case "SWAPBORROWERS":
          this.calObjs.D1003Cal.CalcBorrowerDependentFields();
          break;
        case "SWITCHTO2010":
          this.calObjs.NewHud2015Cal.MoveDataFrom2015to2010("", "");
          break;
        case "SWITCHTO2015":
          this.calObjs.NewHud2015Cal.MoveDataFrom2010to2015("", "");
          break;
        case "SYNCLINKDEFAULTDATA":
          this.calObjs.ToolCal.SyncConstructionLinkSyncDefaultData((string) null, (string) null);
          break;
        case "TPO":
          this.calObjs.Cal.CalculateTPO();
          break;
        case "TRADEOFFTABLE":
          this.calObjs.NewHudCal.CalTradeOffTable(id, val);
          break;
        case "UNLINKEDREMOTE":
          this.calObjs.Cal.CalcUnlinkedRemote((string) null, (string) null);
          break;
        case "UPDATEBORROWERVESTINGNAME":
          this.calObjs.Cal.UpdateBorrowerVestingName((string) null, (string) null);
          break;
        case "UPDATECITYSTATEUSERFEES":
          this._gfeCalculationServant.UpdateCityStateUserFees(id, val);
          break;
        case "UPDATECLOSINGVENDORLICENSE":
          this.calObjs.ToolCal.UpdateClosingVendorInformation(id, val);
          break;
        case "UPDATECOBORROWERVESTINGNAME":
          this.calObjs.Cal.UpdateCoborrowerVestingName((string) null, (string) null);
          break;
        case "UPDATECOMP":
          this.calObjs.ToolCal.UpdateLOCompensation(id, val);
          if (this.loanData.GetField("NEWHUD.X1718") == "Y")
          {
            this.calObjs.GFECal.CalcGFEFees(id, val);
            break;
          }
          break;
        case "UPDATECORRESPONDENTLOANSTATUS":
          this.calObjs.ExternalLateFeeSettings = (ExternalLateFeeSettings) null;
          this.calObjs.ToolCal.CalcCorrespondentLateFee(id, val);
          break;
        case "UPDATEGROSSINCOME":
          this.calObjs.VERIFCal.CalcPreviousGrossIncome("FE0515", (string) null);
          this.calObjs.VERIFCal.CalcPreviousGrossIncome("FE0615", (string) null);
          break;
        case "UPDATEHMDA2018":
          if (this.loanData.Settings != null && this.loanData.Settings.HMDAInfo != null && this.loanData.Settings.HMDAInfo.HMDAApplicationDate != null)
            this.calObjs.HMDACal.AddInitialApplicationDateTrigger(this.loanData.Settings.HMDAInfo.HMDAApplicationDate);
          this.calObjs.HMDACal.UpdateHMDA2018Profile();
          break;
        case "UPDATEHMDAREPORTING":
          this.calObjs.HMDACal.CalcHMDAReporting((string) null, (string) null);
          break;
        case "USDA":
          this.calObjs.USDACal.CalcRefinanceIndicator("USDA.X7", this.loanData.GetField("19"));
          break;
        case "USDAMIP":
          this.calObjs.PrequalCal.CalcUSDAMIP(id, val);
          break;
        case "VALIDATEFHACOUNTYLIMIT":
          this.calObjs.ToolCal.ValidateCountyLimit("1172", (string) null, true);
          break;
        case "VERIFYINGPOCPTCFIELDS":
          this.calObjs.NewHudCal.VerifyingPOCPTCFields(id, val);
          break;
        default:
          this.copyCustomFields(formID, id, val);
          this.updateLOCompensationByTPO(formID, id, val);
          if (formID != string.Empty)
          {
            FieldDefinition field = EncompassFields.GetField(formID);
            if (field != null)
            {
              this.loanData.TriggerCalculation(field.FieldID, this.loanData.GetField(field.FieldID));
              break;
            }
            break;
          }
          break;
      }
      if (this.FormCalculationTriggered == null)
        return;
      this.FormCalculationTriggered((object) formID, new EventArgs());
    }

    private void copyCustomFields(string formID, string id, string val)
    {
      if (!formID.ToUpper().StartsWith("COPYTPOCUSTOMFIELDSTOLOANFIELDS"))
        return;
      if (formID.ToUpper().Contains("COPYTPOCUSTOMFIELDSTOLOANFIELDS."))
      {
        this.calObjs.ToolCal.CopyTPOCustomFieldsToLoanFields(Utils.ParseInt((object) formID.ToUpper().Replace("COPYTPOCUSTOMFIELDSTOLOANFIELDS.", "")));
      }
      else
      {
        if (val == null)
          val = "both";
        ContactCustomField[] companyTpoCustomFields = this.calObjs.ToolCal.FindCompanyTPOCustomFields();
        ContactCustomField[] branchTpoCustomFields = this.calObjs.ToolCal.FindBranchTPOCustomFields();
        if (companyTpoCustomFields != null && companyTpoCustomFields.Length != 0 && (val == "company" || val == "both"))
        {
          foreach (ContactCustomField contactCustomField in companyTpoCustomFields)
          {
            if (contactCustomField.FieldValue != "")
            {
              if (Utils.Dialog((IWin32Window) null, "This TPO company uses custom TPO fields mapped to Encompass loan fields. Do you want to overwrite your loan data with the selected TPO's custom field information?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
              {
                this.calObjs.ToolCal.CopyTPOCustomFieldsToLoanFields(companyTpoCustomFields);
                break;
              }
              break;
            }
          }
        }
        if (branchTpoCustomFields == null || branchTpoCustomFields.Length == 0)
          return;
        foreach (ContactCustomField contactCustomField in branchTpoCustomFields)
        {
          if (contactCustomField.FieldValue != "")
          {
            if (Utils.Dialog((IWin32Window) null, "This TPO branch uses custom TPO fields mapped to Encompass loan fields. Do you want to overwrite your loan data with the selected TPO's custom field information?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
              break;
            this.calObjs.ToolCal.CopyTPOCustomFieldsToLoanFields(branchTpoCustomFields);
            break;
          }
        }
      }
    }

    private void updateLOCompensationByTPO(string formID, string id, string val)
    {
      if (!formID.ToUpper().StartsWith("UPDATECOMPFORTPO") && !formID.ToUpper().StartsWith("UPDATECOMPFORTPOBYOID"))
        return;
      ExternalOriginatorManagementData originatorManagementData = (ExternalOriginatorManagementData) null;
      if (formID.ToUpper().StartsWith("UPDATECOMPFORTPOBYOID."))
      {
        int oid = Utils.ParseInt((object) formID.ToUpper().Replace("UPDATECOMPFORTPOBYOID.", ""));
        if (oid != -1)
          originatorManagementData = this.sessionObjects.ConfigurationManager.GetExternalOrganization(this.loanData.GetField("2626") == "Brokered", oid);
      }
      else if (formID.ToUpper().StartsWith("UPDATECOMPFORTPO."))
      {
        string TPOId = formID.ToUpper().Replace("UPDATECOMPFORTPO.", "");
        if (TPOId != string.Empty)
          originatorManagementData = this.sessionObjects.ConfigurationManager.GetExternalCompanyByTPOID(this.loanData.GetField("2626") == "Brokered", TPOId);
      }
      else if (formID.ToUpper().StartsWith("UPDATECOMPFORTPO"))
      {
        string field1 = this.loanData.GetField("TPO.X62");
        if (field1 != string.Empty)
        {
          ExternalUserInfo userInfoByContactId = this.sessionObjects.ConfigurationManager.GetExternalUserInfoByContactId(field1);
          if ((UserInfo) userInfoByContactId != (UserInfo) null)
            originatorManagementData = this.sessionObjects.ConfigurationManager.GetExternalOrganization(this.loanData.GetField("2626") == "Brokered", userInfoByContactId.ExternalOrgID);
        }
        if (originatorManagementData == null)
        {
          string field2 = this.loanData.GetField("TPO.X39");
          if (field2 == string.Empty)
            field2 = this.loanData.GetField("TPO.X15");
          if (field2 != string.Empty)
            originatorManagementData = this.sessionObjects.ConfigurationManager.GetExternalCompanyByTPOID(this.loanData.GetField("2626") == "Brokered", field2);
        }
      }
      if (originatorManagementData != null)
      {
        this.loanData.SetField("LCP.X2", originatorManagementData.CompanyLegalName);
        this.loanData.SetField("LCP.X18", string.Concat((object) originatorManagementData.oid));
      }
      else
      {
        this.loanData.SetField("LCP.X2", "");
        this.loanData.SetField("LCP.X18", "");
      }
      this.calObjs.ToolCal.UpdateLOCompensation(id, val);
      if (!(this.loanData.GetField("NEWHUD.X1718") == "Y"))
        return;
      this.calObjs.GFECal.CalcGFEFees(id, val);
    }

    public void RecalculateManualInput()
    {
      this.loanData.Dirty = true;
      this.calObjs.RegzCal.BuildPaySchedule("calcmanual", (string) null);
      this.calObjs.RegzCal.CalcAPR((string) null, (string) null);
    }

    public PaymentScheduleSnapshot GetPaymentSchedule(bool needKeyFields)
    {
      string[] strArray = new string[57]
      {
        "2",
        "19",
        "1109",
        "3",
        "4",
        "682",
        "688",
        "689",
        "14",
        "672",
        "674",
        "1719",
        "SYS.X2",
        "423",
        "690",
        "691",
        "1712",
        "1713",
        "698",
        "608",
        "230",
        "232",
        "L268",
        "231",
        "235",
        "1630",
        "253",
        "254",
        "2831",
        "2832",
        "HUD24",
        "3119",
        "1613",
        "1614",
        "1615",
        "1616",
        "1617",
        "1618",
        "3097",
        "3101",
        "3105",
        "3109",
        "3113",
        "3117",
        "3124",
        "3125",
        "3126",
        "3127",
        "3128",
        "3129",
        "3096",
        "3100",
        "3104",
        "3108",
        "3112",
        "3116",
        "NEWHUD.X1707"
      };
      bool performanceEnabled = this.loanData.Calculator.PerformanceEnabled;
      this.loanData.Calculator.PerformanceEnabled = false;
      bool flag = (this.calcOnDemandMethod & CalcOnDemandEnum.PaymentSchedule) == CalcOnDemandEnum.PaymentSchedule;
      if (needKeyFields)
        this.loanData.Calculator.FormCalculation("HUD1ES", (string) null, (string) null);
      PaymentScheduleSnapshot paymentSchedule = new PaymentScheduleSnapshot();
      paymentSchedule.MonthlyPayments = this.calObjs.RegzCal.Schedule;
      paymentSchedule.ActualNumberOfTerm = this.calObjs.RegzCal.NumberOfTerm;
      paymentSchedule.F_le1x24_cd4x34 = this.calObjs.RegzCal.F_le1x24_cd4x34;
      this.loanData.Calculator.PerformanceEnabled = performanceEnabled;
      if (needKeyFields)
      {
        string empty = string.Empty;
        for (int index = 0; index < strArray.Length; ++index)
          paymentSchedule.SetField(strArray[index], this.loanData.GetField(strArray[index]));
        double num1 = 0.0;
        for (int index1 = 1; index1 <= 48; ++index1)
        {
          for (int index2 = 1; index2 <= 11; ++index2)
          {
            string id = "HUD" + index1.ToString("00") + index2.ToString("00");
            if (index2 == 11)
              id = "HUD" + index1.ToString("00") + "60";
            paymentSchedule.SetField(id, this.loanData.GetField(id));
            if (index2 == 10)
              num1 += Utils.ParseDouble((object) this.loanData.GetField(id));
          }
        }
        for (int index = 30; index <= 38; ++index)
          paymentSchedule.SetField("HUD" + index.ToString(), this.loanData.GetField("HUD" + index.ToString()));
        for (int index3 = 1; index3 <= 4; ++index3)
        {
          string str = "HUD" + index3.ToString("00");
          for (int index4 = 41; index4 <= 49; ++index4)
            paymentSchedule.SetField(str + index4.ToString(), this.loanData.GetField(str + index4.ToString()));
        }
        for (int index5 = 1; index5 <= 48; ++index5)
        {
          string str = "HUD" + index5.ToString("00");
          for (int index6 = 13; index6 <= 21; ++index6)
            paymentSchedule.SetField(str + index6.ToString(), this.loanData.GetField(str + index6.ToString()));
        }
        if (num1 != 0.0)
          paymentSchedule.SetField("ESCROWTOTAL", num1.ToString());
        for (int index7 = 1; index7 <= 4; ++index7)
        {
          for (int index8 = 41; index8 <= 49; ++index8)
          {
            string id = "HUD" + index7.ToString("00") + index8.ToString("00");
            paymentSchedule.SetField(id, this.loanData.GetField(id));
          }
        }
        double num2 = Utils.ParseDouble((object) this.loanData.GetField("656")) + Utils.ParseDouble((object) this.loanData.GetField("338")) + Utils.ParseDouble((object) this.loanData.GetField("L269")) + Utils.ParseDouble((object) this.loanData.GetField("655")) + Utils.ParseDouble((object) this.loanData.GetField("657")) + Utils.ParseDouble((object) this.loanData.GetField("1631")) + Utils.ParseDouble((object) this.loanData.GetField("658")) + Utils.ParseDouble((object) this.loanData.GetField("659")) + Utils.ParseDouble((object) this.loanData.GetField("596")) + Utils.ParseDouble((object) this.loanData.GetField("563")) + Utils.ParseDouble((object) this.loanData.GetField("L270")) + Utils.ParseDouble((object) this.loanData.GetField("595")) + Utils.ParseDouble((object) this.loanData.GetField("597")) + Utils.ParseDouble((object) this.loanData.GetField("1632")) + Utils.ParseDouble((object) this.loanData.GetField("598")) + Utils.ParseDouble((object) this.loanData.GetField("599")) + Utils.ParseDouble((object) this.loanData.GetField("558")) + Utils.ParseDouble((object) this.loanData.GetField("NEWHUD.X1708"));
        if (num2 > 0.0)
          paymentSchedule.SetField("ESCROWBEGINBALANCE", num2.ToString());
      }
      if (flag)
        this.loanData.Calculator.EditCalcOnDemandEnum(CalcOnDemandEnum.PaymentSchedule, true);
      return paymentSchedule;
    }

    public PaymentScheduleSnapshot GetWorstCaseScenarioPaymentSchedule(bool needKeyFields)
    {
      this.PerformanceEnabled = false;
      PaymentScheduleSnapshot scheduleSnapshot = this.RegzSummaryType == RegzSummaryTableType.ARMGreater5Years || this.RegzSummaryType == RegzSummaryTableType.ARMIntOnly || this.RegzSummaryType == RegzSummaryTableType.ARMIntOnly31 || this.RegzSummaryType == RegzSummaryTableType.ARMIntOnly51 || this.RegzSummaryType == RegzSummaryTableType.ARMIntOnly7_1or10_1 || this.RegzSummaryType == RegzSummaryTableType.ARMIntOnly3C || this.RegzSummaryType == RegzSummaryTableType.ARMIO_L60 || this.RegzSummaryType == RegzSummaryTableType.ARMLess5Years ? this.CalculateWorstCaseScenario(false).Calculator.GetPaymentSchedule(needKeyFields) : this.GetPaymentSchedule(needKeyFields);
      return scheduleSnapshot == null || scheduleSnapshot.MonthlyPayments == null || scheduleSnapshot.MonthlyPayments.Length == 0 ? (PaymentScheduleSnapshot) null : scheduleSnapshot;
    }

    public string GetUCD(bool forLoanEstimate) => this.GetUCD(forLoanEstimate, false);

    public string GetUCD(bool forLoanEstimate, bool setTotalFields)
    {
      return new UCDXmlExporter(this.loanData, forLoanEstimate, setTotalFields, false).BuildXML();
    }

    public XmlDocument GetUCDXmlDocument(bool forLoanEstimate, bool setTotalFields)
    {
      return new UCDXmlExporter(this.loanData, forLoanEstimate, setTotalFields, false).BuildXMLDocument();
    }

    public string GetUCD(bool forLoanEstimate, bool setTotalFields, bool fullUCD)
    {
      return new UCDXmlExporter(this.loanData, forLoanEstimate, setTotalFields, fullUCD).BuildXML();
    }

    public void CalculateREGZSummary(string id)
    {
      this.calObjs.RegzCal.CalcREGZSummary(id, (string) null);
    }

    public PaymentSchedule[] GetFHAPaymentSchedule() => this.calObjs.RegzCal.FHAPaymentSchedule;

    public void UpdateAccountName(string id)
    {
      this.loanData.Dirty = true;
      this.calObjs.Cal.UpdateAccountName(id, (string) null);
    }

    public string GetQualificationDetail(int column)
    {
      return this.calObjs.PrequalCal.GetQualificationDetail(column);
    }

    public void CalculateMaxLoanAmt()
    {
      this.loanData.Dirty = true;
      this.calObjs.PreCal.CalculateMaxLoanAmt();
    }

    public double CalculateFHAMaxLoanAmt(double propValue, bool getfactor)
    {
      this.loanData.Dirty = true;
      return this.calObjs.PreCal.CalculateFHAMaxLoanAmt(propValue, getfactor);
    }

    public string CalculatePrinting(
      ref NameValueCollection FieldCollect,
      string formID,
      string pageDone,
      bool printLicense)
    {
      this.calObjs.PrintCal.PageDone = pageDone;
      this.calObjs.PrintCal.PrintLicense = printLicense;
      this.calObjs.PrintCal.FieldCollect = FieldCollect;
      this.calObjs.PrintCal.CalcPrinting(formID);
      FieldCollect = this.calObjs.PrintCal.FieldCollect;
      return this.calObjs.PrintCal.PageDone;
    }

    public string CalculatePrinting(
      ref NameValueCollection FieldCollect,
      string formID,
      string pageDone)
    {
      return this.CalculatePrinting(ref FieldCollect, formID, pageDone, false);
    }

    public string CalculatePrinting(
      ref NameValueCollection FieldCollect,
      string formID,
      int blockNo,
      string pageDone)
    {
      this.calObjs.PrintCal.PageDone = pageDone;
      this.calObjs.PrintCal.FieldCollect = FieldCollect;
      this.calObjs.PrintCal.CalcPrinting(formID, blockNo);
      FieldCollect = this.calObjs.PrintCal.FieldCollect;
      return this.calObjs.PrintCal.PageDone;
    }

    public string CalculatePrinting(
      ref NameValueCollection FieldCollect,
      string formID,
      int blockNo,
      bool isBorrower,
      string pageDone)
    {
      this.calObjs.PrintCal.PageDone = pageDone;
      this.calObjs.PrintCal.FieldCollect = FieldCollect;
      this.calObjs.PrintCal.CalcPrinting(formID, blockNo, isBorrower);
      FieldCollect = this.calObjs.PrintCal.FieldCollect;
      return this.calObjs.PrintCal.PageDone;
    }

    public string CalculatePrinting(
      ref NameValueCollection FieldCollect,
      DocumentLog doc,
      string pageDone)
    {
      this.calObjs.PrintCal.PageDone = pageDone;
      this.calObjs.PrintCal.FieldCollect = FieldCollect;
      this.calObjs.PrintCal.CalcPrinting(doc);
      FieldCollect = this.calObjs.PrintCal.FieldCollect;
      return this.calObjs.PrintCal.PageDone;
    }

    public string CalculatePrinting(
      ref NameValueCollection FieldCollect,
      MilestoneTaskLog taskLog,
      string pageDone)
    {
      this.calObjs.PrintCal.PageDone = pageDone;
      this.calObjs.PrintCal.FieldCollect = FieldCollect;
      this.calObjs.PrintCal.CalcPrinting(taskLog);
      FieldCollect = this.calObjs.PrintCal.FieldCollect;
      return this.calObjs.PrintCal.PageDone;
    }

    public string CalculatePrinting(
      ref NameValueCollection FieldCollect,
      ConditionalLetterPrintOption letterOption,
      List<string> selectedConditions,
      string pageDone)
    {
      this.calObjs.PrintCal.PageDone = pageDone;
      this.calObjs.PrintCal.FieldCollect = FieldCollect;
      this.calObjs.PrintCal.CalcPrinting(letterOption, selectedConditions);
      FieldCollect = this.calObjs.PrintCal.FieldCollect;
      return this.calObjs.PrintCal.PageDone;
    }

    public string CalculatePrinting(
      ref NameValueCollection FieldCollect,
      ConditionLog condition,
      string pageDone)
    {
      this.calObjs.PrintCal.PageDone = pageDone;
      this.calObjs.PrintCal.FieldCollect = FieldCollect;
      this.calObjs.PrintCal.CalcPrinting(condition);
      FieldCollect = this.calObjs.PrintCal.FieldCollect;
      return this.calObjs.PrintCal.PageDone;
    }

    public string CalculatePrinting(
      ref NameValueCollection FieldCollect,
      LoanData dummyLoan,
      DisclosureTrackingBase disclosureLog,
      int pageNo,
      string pageDone)
    {
      this.calObjs.PrintCal.PageDone = pageDone;
      this.calObjs.PrintCal.FieldCollect = FieldCollect;
      this.calObjs.PrintCal.CalcPrinting(dummyLoan, pageNo);
      FieldCollect = this.calObjs.PrintCal.FieldCollect;
      return this.calObjs.PrintCal.PageDone;
    }

    public string OutputFormSizeCheck(string formID)
    {
      return this.calObjs.PrintCal.OutputFormSizeCheck(formID);
    }

    public bool HasNextPrintPage(string formID)
    {
      return formID.ToLower() == "funding worksheet additional" && this.calObjs.PrintCal.PrintFundingWorksheet(true, false) > 0;
    }

    public void PrefixCalculations(string version) => this.calObjs.Cal.PrefixCalculations(version);

    public void CalculateAll(bool copyUnpaid)
    {
      this.loanData.Dirty = true;
      this.calObjs.Cal.CalculateAll(copyUnpaid);
    }

    public void CalculateAll()
    {
      this.loanData.Dirty = true;
      this.calObjs.Cal.CalculateAll();
      if (Utils.CheckIf2015RespaTila(this.loanData.GetField("3969")))
        this.CalculateLatestDisclosure2015((IDisclosureTracking2015Log) null);
      else
        this.CalculateLatestDisclosure((DisclosureTrackingLog) null, false);
    }

    public void ImportCalculation()
    {
      this.loanData.Dirty = true;
      this.calObjs.Cal.ImportCalculation();
    }

    public void ImportCalculation(bool copyUnpaid)
    {
      this.loanData.Dirty = true;
      this.calObjs.Cal.ImportCalculation(copyUnpaid);
    }

    public void CopyMLDSToGFE() => this.calObjs.GFECal.CopyMLDSToGFE((DataTemplate) null);

    public void CopyMLDSToGFE(DataTemplate dataTemplate)
    {
      this.loanData.Dirty = true;
      this.calObjs.GFECal.CopyMLDSToGFE(dataTemplate);
    }

    public void CopyGFEToMLDS() => this.CopyGFEToMLDS((string) null);

    public void CopyGFEToMLDS(string id)
    {
      this.loanData.Dirty = true;
      this.calObjs.MLDSCal.CopyGFEToMLDS(id);
    }

    public void PopulateFeeList(bool recalculated)
    {
      this.calObjs.GFECal.PopulateFeeList((string) null, recalculated);
    }

    public void PopulateFeeList(string id, bool recalculated)
    {
      this.calObjs.GFECal.PopulateFeeList(id, recalculated);
    }

    public void CalculateBrokerLenderFeeTotals()
    {
      this.calObjs.GFECal.CalcBrokerLenderFeeTotals((string) null, (string) null);
    }

    public double CalcTitleEscrowRate(TableFeeListBase.FeeTable t)
    {
      return this.calObjs.ToolCal.CalcTitleEscrowRate(t);
    }

    public Dictionary<string, object> GetFundingBalancingWorksheet()
    {
      return this.calObjs.ToolCal.GetFundingBalancingWorksheet();
    }

    public bool CalcFundingBalancingWorksheet(
      object gfeItemObject,
      ref string paidBy,
      ref double amount)
    {
      return this.calObjs.ToolCal.CalcFundingBalancingWorksheet(gfeItemObject, ref paidBy, ref amount);
    }

    public void ShiftPaymentFrequence(DateTime previous1stPayDate, DateTime new1stPayDate)
    {
      this.loanData.Dirty = true;
      this.calObjs.Hud1Cal.ShiftPaymentFrequence(previous1stPayDate, new1stPayDate);
    }

    public bool PerformanceEnabled
    {
      get => this.calObjs.RegzCal.PerformanceEnabled;
      set => this.calObjs.RegzCal.PerformanceEnabled = value;
    }

    public void EditCalcOnDemandEnum(CalcOnDemandEnum calcOnDemandMethod, bool include)
    {
      if (include)
        this.calcOnDemandMethod |= calcOnDemandMethod;
      else
        this.calcOnDemandMethod &= ~calcOnDemandMethod;
      if (this.calcOnDemandMethod != CalcOnDemandEnum.None)
      {
        if (!(this.loanData.GetField("CALCREQUIRED") != "Y"))
          return;
        this.loanData.SetCurrentFieldFromCal("CALCREQUIRED", "Y");
      }
      else
      {
        if (!(this.loanData.GetField("CALCREQUIRED") == "Y"))
          return;
        this.loanData.SetCurrentFieldFromCal("CALCREQUIRED", "");
      }
    }

    public bool CalcOnDemand() => this.CalcOnDemand(this.calcOnDemandMethod);

    public bool CalcOnDemand(CalcOnDemandEnum calcOnDemandMethod)
    {
      bool flag = false;
      if (this.loanData.GetField("CALCREQUIRED") == "Y")
      {
        if ((this.calcOnDemandMethod & CalcOnDemandEnum.PaymentSchedule) == CalcOnDemandEnum.PaymentSchedule && (calcOnDemandMethod & CalcOnDemandEnum.PaymentSchedule) == CalcOnDemandEnum.PaymentSchedule)
        {
          bool skipLockRequestSync = this.SkipLockRequestSync;
          this.SkipLockRequestSync = true;
          this.CalcPaymentSchedule();
          this.CalcATRQMPaymentSchedule();
          this.SkipLockRequestSync = skipLockRequestSync;
          this.calcOnDemandMethod &= ~CalcOnDemandEnum.PaymentSchedule;
          flag = true;
        }
        if ((this.calcOnDemandMethod & CalcOnDemandEnum.Update2010ItemizationFrom2015Itemization) == CalcOnDemandEnum.Update2010ItemizationFrom2015Itemization && (calcOnDemandMethod & CalcOnDemandEnum.Update2010ItemizationFrom2015Itemization) == CalcOnDemandEnum.Update2010ItemizationFrom2015Itemization)
        {
          this.Update2010ItemizationFrom2015Itemization();
          this.calcOnDemandMethod &= ~CalcOnDemandEnum.Update2010ItemizationFrom2015Itemization;
          flag = true;
          this.loanData.Calculator.GetGFFVarianceAlertDetails();
        }
        if ((this.calcOnDemandMethod & CalcOnDemandEnum.FundingFees) == CalcOnDemandEnum.FundingFees && (calcOnDemandMethod & CalcOnDemandEnum.FundingFees) == CalcOnDemandEnum.FundingFees)
        {
          this.GetFundingFees(true);
          this.calcOnDemandMethod &= ~CalcOnDemandEnum.FundingFees;
          flag = true;
        }
        if ((this.calcOnDemandMethod & CalcOnDemandEnum.CreateConstructionLinkedSubsetFields) == CalcOnDemandEnum.CreateConstructionLinkedSubsetFields && (calcOnDemandMethod & CalcOnDemandEnum.CreateConstructionLinkedSubsetFields) == CalcOnDemandEnum.CreateConstructionLinkedSubsetFields)
          this.loanData.CreateSubsetConstructionLinkedFields();
        if ((this.calcOnDemandMethod & CalcOnDemandEnum.MaxPaymentSample) == CalcOnDemandEnum.MaxPaymentSample)
          this.calObjs.NewHudCal.CalcInterestRateChange((string) null, (string) null);
      }
      if (this.calcOnDemandMethod == CalcOnDemandEnum.None && this.loanData.GetField("CALCREQUIRED") == "Y")
        this.loanData.SetCurrentFieldFromCal("CALCREQUIRED", "");
      return flag;
    }

    public void CalcPaymentSchedule()
    {
      this.calObjs.RegzCal.CalcPaymentSchedule((string) null, (string) null);
    }

    public void CalcATRQMPaymentSchedule()
    {
      this.calObjs.ATRQMCal.GetATRQMPaymentSchedule(false);
      this.calObjs.ATRQMCal.CalculateQmHigherPricedCheck((string) null, (string) null);
      this.calObjs.ATRQMCal.CalcEligibility((string) null, (string) null);
    }

    public List<string[]> GetATRQMPaymentSchedule()
    {
      return this.calObjs.ATRQMCal.GetATRQMPaymentSchedule(true);
    }

    public Dictionary<string, string> BuildHelocExampleSchedules()
    {
      return this.calObjs.HelocCal.BuildHelocExampleSchedules();
    }

    public string GetLastErrorFromHelocExampleSchedules()
    {
      return this.calObjs.HelocCal.GetLastErrorFromExampleSchedules();
    }

    public void CalcRateCap() => this.calObjs.RegzCal.CalcRateCap();

    public void CopySpecialHUD2010ToGFE2010()
    {
      this.calObjs.NewHudCal.CopySpecialHUD2010ToGFE2010((string) null, (string) null);
    }

    public void CopyHUD2010ToGFE2010() => this.CopyHUD2010ToGFE2010("copytohudgfe", false);

    public void CopyHUD2010ToGFE2010(string id, bool blankCopyOnly)
    {
      this.loanData.Dirty = true;
      this.calObjs.NewHudCal.CopyHUD2010ToGFE2010(id, id, blankCopyOnly);
    }

    public void Update2010ItemizationFrom2015Itemization()
    {
      this.calObjs.NewHud2015FeeDetailCal.Update2010ItemizationFrom2015Itemization((string) null, (string) null);
    }

    public bool ValidateRevolvingVOLs() => this.calObjs.VERIFCal.ValidateRevolvingVOLs();

    public void GetVerificationContactInformation(
      ref string contactName,
      ref string contactTitle,
      ref string contactPhone,
      ref string contactFax)
    {
      this.calObjs.VERIFCal.GetVerificationContactInformation(ref contactName, ref contactTitle, ref contactPhone, ref contactFax);
    }

    public void SetVerificationTitle(MilestoneLog msLog)
    {
      this.calObjs.VERIFCal.SetVerificationTitle(msLog);
    }

    public void SetVerificationTitle(MilestoneFreeRoleLog msfreeRoleLog)
    {
      this.calObjs.VERIFCal.SetVerificationTitle(msfreeRoleLog);
    }

    public bool UpdateRevolvingLiabilities(
      string id,
      string val,
      bool setRevolvingFactor,
      bool confirmedRequired,
      bool doCalculation)
    {
      if (confirmedRequired && Utils.Dialog((IWin32Window) null, "Do you want to adjust the revolving debt information fields to meet Fannie Mae guidelines? Click Yes to set the Payment amount to $10 or the Factor for Revolving Debt to 5%, whichever is greater. Click No to keep the current field entries.", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
        return false;
      if (setRevolvingFactor)
        this.loanData.SetCurrentField("SYS.X13", "5");
      if (!doCalculation)
        return true;
      this.calObjs.VERIFCal.UpdateRevolvingLiabilities(id != "SYS.X13");
      return true;
    }

    public bool IsSyncGFERequired => this.calObjs.NewHudCal.IsSyncGFERequired;

    public bool UseNewCompliance(Decimal versionToRunNewLogic)
    {
      return this.calObjs.NewHud2015Cal.UseNewCompliance(versionToRunNewLogic);
    }

    public bool IsCalcAllRequired
    {
      get
      {
        PerformanceMeter.Current.AddCheckpoint("GET IsCalcAllRequired " + this.isCalcAllRequired.ToString(), 1821, nameof (IsCalcAllRequired), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\CalculationEngine\\LoanCalculator.cs");
        return this.isCalcAllRequired;
      }
      set
      {
        PerformanceMeter.Current.AddCheckpoint("SET IsCalcAllRequired " + value.ToString(), 1826, nameof (IsCalcAllRequired), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\CalculationEngine\\LoanCalculator.cs");
        this.isCalcAllRequired = value;
      }
    }

    public void CopySafeHarborToLoan()
    {
      this.loanData.Dirty = true;
      this.calObjs.ToolCal.CopySafeHarborToLoan();
    }

    public void CopyLoanToSafeHarbor(int optionNumber)
    {
      this.loanData.Dirty = true;
      this.calObjs.ToolCal.CopyLoanToSafeHarbor(optionNumber);
    }

    public void CopyGFE2010ToHUD2010()
    {
      this.loanData.Dirty = true;
      this.calObjs.NewHudCal.CopyGFE2010ToHUD2010();
    }

    public void RecalculateCustomFields() => this.calObjs.CustomCal.CalculateAll();

    public void RecalculateBillingFields() => this.calObjs.BillingCal.CalculateAll();

    public void RecalculateNMLS() => this.calObjs.D1003Cal.RecalculateNMLS();

    public List<FundingFee> GetFundingFees(bool hideZero)
    {
      return this.calObjs.ToolCal.GetFundingFees(hideZero);
    }

    public void ClearClosingVendorInformation(
      bool clearLender,
      bool clearInvestor,
      bool clearBroker)
    {
      this.calObjs.ToolCal.ClearClosingVendorInformation(clearLender, clearInvestor, clearBroker);
    }

    public void Dispose()
    {
    }

    public void ApplyLoanTypeOtherField() => this.calObjs.NewHud2015Cal.CalcLoanTypeOtherField();

    private DateTime geteDisclosureReceivedDate(DisclosurePackage detailInfo)
    {
      if (!detailInfo.ContainCoBorrower)
        return detailInfo.DocumentViewedDate_Borrower;
      DateTime viewedDateBorrower = detailInfo.DocumentViewedDate_Borrower;
      DateTime viewedDateCoBorrower = detailInfo.DocumentViewedDate_CoBorrower;
      if (!(viewedDateBorrower != DateTime.MinValue) || !(viewedDateCoBorrower != DateTime.MinValue))
        return DateTime.MinValue;
      return !(viewedDateBorrower > viewedDateCoBorrower) ? viewedDateCoBorrower : viewedDateBorrower;
    }

    private DateTime geteDisclosureReceivedDate(DisclosureTrackingLog dtLog)
    {
      int num = dtLog.IsWetSigned ? 1 : 0;
      return dtLog.EDisclosureBorrowerDocumentViewedDate;
    }

    public void CalculateNew2015DisclosureReceivedDate(
      IDisclosureTracking2015Log log,
      DisclosurePackage detailInfo,
      bool isPlatform)
    {
      DisclosureTrackingBase.DisclosedMethod disclosedMethod = log.eDisclosureManualFulfillmentMethod;
      if (log.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.eClose)
        disclosedMethod = DisclosureTrackingBase.DisclosedMethod.eClose;
      else if (disclosedMethod == DisclosureTrackingBase.DisclosedMethod.None)
        disclosedMethod = log.DisclosureMethod;
      BusinessCalendar businessCalendar = this.sessionObjects.GetBusinessCalendar(CalendarType.RegZ);
      DateTime dateTime1 = new DateTime();
      DateTime dateTime2 = new DateTime();
      bool policySetting = (bool) this.sessionObjects.StartupInfo.PolicySettings[(object) "Policies.eSignReceivedDate"];
      DateTime dateTime3 = new DateTime();
      DateTime dateTime4 = new DateTime();
      bool flag1 = this.UseNewCompliance(20.1M);
      bool flag2 = this.UseNewCompliance(19.1M);
      string field1 = this.loanData.GetField("3152");
      string field2 = this.loanData.GetField("3143");
      string field3 = this.loanData.GetField("3977");
      DateTime dateTime5 = string.IsNullOrEmpty(field3) ? DateTime.MinValue : Utils.ParseDate((object) field3);
      DateTime dateTime6 = string.IsNullOrEmpty(field1) ? DateTime.MinValue : Utils.ParseDate((object) field1);
      DateTime dateTime7 = string.IsNullOrEmpty(field2) ? DateTime.MinValue : Utils.ParseDate((object) field2);
      DateTime dateTime8 = businessCalendar.AddBusinessDays(log.DisclosedDate, 3, true);
      DateTime dateTime9;
      DateTime dateTime10;
      if (log.IsWetSigned)
      {
        dateTime9 = detailInfo.DocumentViewedDate_Borrower;
        dateTime10 = detailInfo.DocumentViewedDate_CoBorrower;
      }
      else if (policySetting)
      {
        dateTime9 = detailInfo.BorrowereSignedDate;
        dateTime10 = detailInfo.CoborrowereSignedDate;
      }
      else
      {
        dateTime9 = detailInfo.DocumentViewedDate_Borrower;
        dateTime10 = detailInfo.DocumentViewedDate_CoBorrower;
      }
      if (log.GetAllnboItems().Count > 0)
      {
        foreach (KeyValuePair<string, INonBorrowerOwnerItem> allnboItem in log.GetAllnboItems())
        {
          string key;
          if (isPlatform)
            key = string.Join(" ", ((IEnumerable<string>) new string[4]
            {
              allnboItem.Value.FirstName,
              allnboItem.Value.MidName,
              allnboItem.Value.LastName,
              allnboItem.Value.Suffix
            }).Where<string>((Func<string, bool>) (str => !string.IsNullOrEmpty(str)))) + " " + allnboItem.Value.Email;
          else
            key = allnboItem.Value.TRGuid;
          if (detailInfo.NBODetail.ContainsKey(key))
          {
            DateTime dateTime11 = new DateTime();
            DateTime dateTime12 = allnboItem.Value.eDisclosureNBOeSignatures ? (!policySetting ? detailInfo.NBODetail[key].documentViewedDate : detailInfo.NBODetail[key].eSignedDate) : detailInfo.NBODetail[key].documentViewedDate;
            if (allnboItem.Value.ActualReceivedDate == DateTime.MinValue || dateTime12 != DateTime.MinValue)
              log.SetnboAttributeValue(allnboItem.Key, (object) dateTime12, DisclosureTracking2015Log.NBOUpdatableFields.ActualReceivedDate);
          }
        }
      }
      DateTime fullfillmentProcessedDate = log.FullfillmentProcessedDate;
      DateTime processedDateCoBorrower = log.FullfillmentProcessedDate_CoBorrower;
      if (log.PresumedFulfillmentDate == DateTime.MinValue && log.eDisclosureManualFulfillmentDate != DateTime.MinValue)
        log.PresumedFulfillmentDate = businessCalendar.AddBusinessDays(log.eDisclosureManualFulfillmentDate, 3, true);
      DateTime dateTime13 = log.PresumedFulfillmentDate > log.ActualFulfillmentDate ? log.ActualFulfillmentDate : log.PresumedFulfillmentDate;
      DateTime dateTime14 = log.PresumedFulfillmentDate > log.ActualFulfillmentDate ? log.ActualFulfillmentDate : log.PresumedFulfillmentDate;
      DateTime dateTime15;
      DateTime dateTime16;
      if (log.EDisclosureBorrowerLoanLevelConsent == "Accepted" && (log.CoBorrowerName.Trim() == "" || log.EDisclosureCoBorrowerLoanLevelConsent == "Accepted"))
      {
        if (log.IsWetSigned | policySetting && (dateTime9 != DateTime.MinValue || dateTime10 != DateTime.MinValue) || !log.IsWetSigned && !policySetting && (dateTime9 != DateTime.MinValue || dateTime10 != DateTime.MinValue))
        {
          if (dateTime9 == DateTime.MinValue)
          {
            dateTime15 = dateTime13;
            log.BorrowerDisclosedMethod = disclosedMethod;
            log.BorrowerFulfillmentMethodDescription = "Fulfillment";
          }
          else if (dateTime13 == DateTime.MinValue)
          {
            dateTime15 = dateTime9;
            log.BorrowerDisclosedMethod = log.DisclosureMethod;
            log.BorrowerFulfillmentMethodDescription = "";
          }
          else if (dateTime13 < dateTime9)
          {
            dateTime15 = dateTime13;
            log.BorrowerDisclosedMethod = disclosedMethod;
            log.BorrowerFulfillmentMethodDescription = "Fulfillment";
          }
          else
          {
            dateTime15 = dateTime9;
            log.BorrowerDisclosedMethod = log.DisclosureMethod;
            log.BorrowerFulfillmentMethodDescription = "";
          }
          if (dateTime10 == DateTime.MinValue)
          {
            dateTime16 = dateTime14;
            log.CoBorrowerDisclosedMethod = disclosedMethod;
            log.CoBorrowerFulfillmentMethodDescription = "Fulfillment";
          }
          else if (dateTime14 == DateTime.MinValue)
          {
            dateTime16 = dateTime10;
            log.CoBorrowerDisclosedMethod = log.DisclosureMethod;
            log.CoBorrowerFulfillmentMethodDescription = "";
          }
          else if (dateTime14 < dateTime10)
          {
            dateTime16 = dateTime14;
            log.CoBorrowerDisclosedMethod = disclosedMethod;
            log.CoBorrowerFulfillmentMethodDescription = "Fulfillment";
          }
          else
          {
            dateTime16 = dateTime10;
            log.CoBorrowerDisclosedMethod = log.DisclosureMethod;
            log.CoBorrowerFulfillmentMethodDescription = "";
          }
        }
        else
        {
          dateTime15 = dateTime13;
          dateTime16 = dateTime14;
          log.BorrowerDisclosedMethod = log.CoBorrowerDisclosedMethod = disclosedMethod;
          log.BorrowerFulfillmentMethodDescription = log.CoBorrowerFulfillmentMethodDescription = "Fulfillment";
        }
      }
      else if (log.EDisclosureBorrowerLoanLevelConsent == "Accepted" || log.EDisclosureCoBorrowerLoanLevelConsent == "Accepted")
      {
        if (log.IsWetSigned | policySetting && (dateTime9 != DateTime.MinValue || dateTime10 != DateTime.MinValue) || !log.IsWetSigned && !policySetting && (dateTime9 != DateTime.MinValue || dateTime10 != DateTime.MinValue))
        {
          if (log.EDisclosureBorrowerLoanLevelConsent == "Accepted")
          {
            if (dateTime9 == DateTime.MinValue)
            {
              dateTime15 = dateTime13;
              log.BorrowerDisclosedMethod = disclosedMethod;
              log.BorrowerFulfillmentMethodDescription = "Fulfillment";
            }
            else if (dateTime13 == DateTime.MinValue)
            {
              dateTime15 = dateTime9;
              log.BorrowerDisclosedMethod = log.DisclosureMethod;
              log.BorrowerFulfillmentMethodDescription = "";
            }
            else if (dateTime13 < dateTime9)
            {
              dateTime15 = dateTime13;
              log.BorrowerDisclosedMethod = disclosedMethod;
              log.BorrowerFulfillmentMethodDescription = "Fulfillment";
            }
            else
            {
              dateTime15 = dateTime9;
              log.BorrowerDisclosedMethod = log.DisclosureMethod;
              log.BorrowerFulfillmentMethodDescription = "";
            }
          }
          else
          {
            dateTime15 = log.ActualFulfillmentDate;
            log.BorrowerDisclosedMethod = disclosedMethod;
            log.BorrowerFulfillmentMethodDescription = "Fulfillment";
          }
          if (log.EDisclosureCoBorrowerLoanLevelConsent == "Accepted")
          {
            if (dateTime10 == DateTime.MinValue)
            {
              dateTime16 = dateTime14;
              log.CoBorrowerDisclosedMethod = disclosedMethod;
              log.CoBorrowerFulfillmentMethodDescription = "Fulfillment";
            }
            else if (dateTime14 == DateTime.MinValue)
            {
              dateTime16 = dateTime10;
              log.CoBorrowerDisclosedMethod = log.DisclosureMethod;
              log.CoBorrowerFulfillmentMethodDescription = "";
            }
            else if (dateTime14 < dateTime10)
            {
              dateTime16 = dateTime14;
              log.CoBorrowerDisclosedMethod = disclosedMethod;
              log.CoBorrowerFulfillmentMethodDescription = "Fulfillment";
            }
            else
            {
              dateTime16 = dateTime10;
              log.CoBorrowerDisclosedMethod = log.DisclosureMethod;
              log.CoBorrowerFulfillmentMethodDescription = "";
            }
          }
          else
          {
            dateTime16 = log.ActualFulfillmentDate;
            log.CoBorrowerDisclosedMethod = disclosedMethod;
            log.CoBorrowerFulfillmentMethodDescription = "Fulfillment";
          }
        }
        else
        {
          if (log.EDisclosureBorrowerLoanLevelConsent == "Accepted")
          {
            dateTime15 = dateTime13;
            log.BorrowerDisclosedMethod = disclosedMethod;
            log.BorrowerFulfillmentMethodDescription = "Fulfillment";
          }
          else
          {
            dateTime15 = log.ActualFulfillmentDate;
            log.BorrowerDisclosedMethod = disclosedMethod;
            log.BorrowerFulfillmentMethodDescription = "Fulfillment";
          }
          if (log.EDisclosureCoBorrowerLoanLevelConsent == "Accepted")
          {
            dateTime16 = dateTime14;
            log.CoBorrowerDisclosedMethod = disclosedMethod;
            log.CoBorrowerFulfillmentMethodDescription = "Fulfillment";
          }
          else
          {
            dateTime16 = log.ActualFulfillmentDate;
            log.CoBorrowerDisclosedMethod = disclosedMethod;
            log.CoBorrowerFulfillmentMethodDescription = "Fulfillment";
          }
        }
      }
      else
      {
        dateTime15 = dateTime16 = log.ActualFulfillmentDate;
        log.BorrowerDisclosedMethod = log.CoBorrowerDisclosedMethod = disclosedMethod;
        log.BorrowerFulfillmentMethodDescription = log.CoBorrowerFulfillmentMethodDescription = "Fulfillment";
        if (log.eDisclosureManualFulfillmentDate == DateTime.MinValue && log.FullfillmentProcessedDate == DateTime.MinValue)
          log.BorrowerFulfillmentMethodDescription = "";
        if (log.eDisclosureManualFulfillmentDate == DateTime.MinValue && log.FullfillmentProcessedDate_CoBorrower == DateTime.MinValue)
          log.CoBorrowerFulfillmentMethodDescription = "";
      }
      if (((!log.DisclosedForLE ? 0 : (!log.IsWetSigned ? 1 : 0)) & (flag2 ? 1 : 0)) != 0 && (log.EDisclosureBorrowerLoanLevelConsent != "Accepted" || log.EDisclosureCoBorrowerLoanLevelConsent != "Accepted"))
      {
        if (dateTime7 != DateTime.MinValue)
        {
          if (log.EDisclosureBorrowerLoanLevelConsent != "Accepted" && detailInfo.DocumentViewedDate_Borrower.Date >= dateTime6.Date && detailInfo.DocumentViewedDate_Borrower.Date <= dateTime7.Date)
            dateTime15 = detailInfo.DocumentViewedDate_Borrower;
          if (log.EDisclosureCoBorrowerLoanLevelConsent != "Accepted" && detailInfo.DocumentViewedDate_CoBorrower.Date >= dateTime6.Date && detailInfo.DocumentViewedDate_CoBorrower.Date <= dateTime7.Date)
            dateTime16 = detailInfo.DocumentViewedDate_CoBorrower;
        }
        else
          dateTime15 = dateTime16 = DateTime.MinValue;
      }
      if (((!log.DisclosedForCD ? 0 : (!log.IsWetSigned ? 1 : 0)) & (flag1 ? 1 : 0)) != 0 && (log.EDisclosureBorrowerLoanLevelConsent != "Accepted" || log.EDisclosureCoBorrowerLoanLevelConsent != "Accepted"))
      {
        if (log.EDisclosureBorrowerLoanLevelConsent != "Accepted" && detailInfo.DocumentViewedDate_Borrower.Date >= dateTime5.Date)
          dateTime15 = detailInfo.DocumentViewedDate_Borrower;
        if (log.EDisclosureCoBorrowerLoanLevelConsent != "Accepted" && detailInfo.DocumentViewedDate_CoBorrower.Date >= dateTime5.Date)
          dateTime16 = detailInfo.DocumentViewedDate_CoBorrower;
      }
      log.BorrowerPresumedReceivedDate = !(log.EDisclosureBorrowerLoanLevelConsent == "Accepted") ? log.PresumedFulfillmentDate : dateTime8;
      log.CoBorrowerPresumedReceivedDate = !(log.EDisclosureCoBorrowerLoanLevelConsent == "Accepted") ? log.PresumedFulfillmentDate : dateTime8;
      if (log.GetAllnboItems().Count > 0)
      {
        foreach (KeyValuePair<string, INonBorrowerOwnerItem> allnboItem in log.GetAllnboItems())
        {
          if (detailInfo.NBODetail.ContainsKey(allnboItem.Value.TRGuid) && allnboItem.Value.eDisclosureNBOLoanLevelConsent == "Accepted")
          {
            DateTime dateTime17 = businessCalendar.AddBusinessDays(log.DisclosedDate, 3, true);
            log.SetnboAttributeValue(allnboItem.Key, (object) dateTime17, DisclosureTracking2015Log.NBOUpdatableFields.PresumedReceivedDate);
          }
        }
      }
      if ((log.BorrowerDisclosedMethod == DisclosureTrackingBase.DisclosedMethod.ByMail || log.BorrowerDisclosedMethod == DisclosureTrackingBase.DisclosedMethod.eDisclosure || log.BorrowerDisclosedMethod == DisclosureTrackingBase.DisclosedMethod.eClose) && (log.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.eDisclosure || log.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.eClose) && log.eDisclosureManualFulfillmentDate == DateTime.MinValue && log.FullfillmentProcessedDate == DateTime.MinValue)
      {
        log.BorrowerDisclosedMethod = log.DisclosureMethod;
        log.BorrowerFulfillmentMethodDescription = "";
      }
      if ((log.CoBorrowerDisclosedMethod == DisclosureTrackingBase.DisclosedMethod.ByMail || log.CoBorrowerDisclosedMethod == DisclosureTrackingBase.DisclosedMethod.eDisclosure || log.CoBorrowerDisclosedMethod == DisclosureTrackingBase.DisclosedMethod.eClose) && (log.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.eDisclosure || log.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.eClose) && log.eDisclosureManualFulfillmentDate == DateTime.MinValue && log.FullfillmentProcessedDate_CoBorrower == DateTime.MinValue)
      {
        log.CoBorrowerDisclosedMethod = log.DisclosureMethod;
        log.CoBorrowerFulfillmentMethodDescription = "";
      }
      if (!flag2 || !flag1 || log.IsWetSigned)
      {
        if (dateTime15 != DateTime.MinValue)
          log.BorrowerActualReceivedDate = dateTime15;
        if (!(dateTime16 != DateTime.MinValue))
          return;
        log.CoBorrowerActualReceivedDate = dateTime16;
      }
      else if (dateTime7 == DateTime.MinValue && log.DisclosedForLE)
      {
        log.BorrowerActualReceivedDate = log.CoBorrowerActualReceivedDate = DateTime.MinValue;
      }
      else
      {
        if (dateTime15 != DateTime.MinValue)
          log.BorrowerActualReceivedDate = dateTime15;
        if (!(dateTime16 != DateTime.MinValue))
          return;
        log.CoBorrowerActualReceivedDate = dateTime16;
      }
    }

    public void CalculateNew2015DisclosureReceivedDate(
      IDisclosureTracking2015Log log,
      bool isPlatform)
    {
      DisclosureTrackingBase.DisclosedMethod disclosedMethod = log.eDisclosureManualFulfillmentMethod;
      if (log.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.eClose)
        disclosedMethod = DisclosureTrackingBase.DisclosedMethod.eClose;
      else if (disclosedMethod == DisclosureTrackingBase.DisclosedMethod.None)
        disclosedMethod = log.DisclosureMethod;
      BusinessCalendar businessCalendar = this.sessionObjects.GetBusinessCalendar(CalendarType.RegZ);
      DateTime dateTime1 = new DateTime();
      DateTime dateTime2 = new DateTime();
      bool policySetting = (bool) this.sessionObjects.StartupInfo.PolicySettings[(object) "Policies.eSignReceivedDate"];
      DateTime dateTime3 = new DateTime();
      DateTime dateTime4 = new DateTime();
      bool flag1 = this.UseNewCompliance(20.1M);
      bool flag2 = this.UseNewCompliance(19.1M);
      string field1 = this.loanData.GetField("3152");
      string field2 = this.loanData.GetField("3143");
      string field3 = this.loanData.GetField("3977");
      DateTime dateTime5 = string.IsNullOrEmpty(field3) ? DateTime.MinValue : Utils.ParseDate((object) field3);
      DateTime dateTime6 = string.IsNullOrEmpty(field1) ? DateTime.MinValue : Utils.ParseDate((object) field1);
      DateTime dateTime7 = string.IsNullOrEmpty(field2) ? DateTime.MinValue : Utils.ParseDate((object) field2);
      DateTime dateTime8 = businessCalendar.AddBusinessDays(log.DisclosedDate, 3, true);
      DateTime dateTime9;
      DateTime dateTime10;
      if (log.IsWetSigned)
      {
        dateTime9 = log.EDisclosureBorrowerDocumentViewedDate;
        dateTime10 = log.EDisclosureCoborrowerDocumentViewedDate;
      }
      else if (policySetting)
      {
        dateTime9 = log.eDisclosureBorrowereSignedDate;
        dateTime10 = log.eDisclosureCoBorrowereSignedDate;
      }
      else
      {
        dateTime9 = log.EDisclosureBorrowerDocumentViewedDate;
        dateTime10 = log.EDisclosureCoborrowerDocumentViewedDate;
      }
      if (log.GetAllnboItems().Count > 0)
      {
        foreach (KeyValuePair<string, INonBorrowerOwnerItem> allnboItem in log.GetAllnboItems())
        {
          string str1 = "";
          if (isPlatform)
            str1 = string.Join(" ", ((IEnumerable<string>) new string[4]
            {
              allnboItem.Value.FirstName,
              allnboItem.Value.MidName,
              allnboItem.Value.LastName,
              allnboItem.Value.Suffix
            }).Where<string>((Func<string, bool>) (str => !string.IsNullOrEmpty(str)))) + " " + allnboItem.Value.Email;
          else
            str1 = allnboItem.Value.TRGuid;
          if (log.NonBorrowerOwnerCollections.ContainsKey(allnboItem.Key))
          {
            DateTime dateTime11 = new DateTime();
            DateTime dateTime12 = allnboItem.Value.eDisclosureNBOeSignatures ? (!policySetting ? Utils.ParseDate((object) log.NonBorrowerOwnerCollections[allnboItem.Key].eDisclosureNBODocumentViewedDate) : Utils.ParseDate((object) log.NonBorrowerOwnerCollections[allnboItem.Key].eDisclosureNBOSignedDate)) : Utils.ParseDate((object) log.NonBorrowerOwnerCollections[allnboItem.Key].eDisclosureNBODocumentViewedDate);
            if (allnboItem.Value.ActualReceivedDate == DateTime.MinValue || dateTime12 != DateTime.MinValue)
              log.SetnboAttributeValue(allnboItem.Key, (object) dateTime12, DisclosureTracking2015Log.NBOUpdatableFields.ActualReceivedDate);
          }
        }
      }
      DateTime fullfillmentProcessedDate = log.FullfillmentProcessedDate;
      DateTime processedDateCoBorrower = log.FullfillmentProcessedDate_CoBorrower;
      if (log.PresumedFulfillmentDate == DateTime.MinValue && log.eDisclosureManualFulfillmentDate != DateTime.MinValue)
        log.PresumedFulfillmentDate = businessCalendar.AddBusinessDays(log.eDisclosureManualFulfillmentDate, 3, true);
      DateTime dateTime13 = log.PresumedFulfillmentDate > log.ActualFulfillmentDate ? log.ActualFulfillmentDate : log.PresumedFulfillmentDate;
      DateTime dateTime14 = log.PresumedFulfillmentDate > log.ActualFulfillmentDate ? log.ActualFulfillmentDate : log.PresumedFulfillmentDate;
      DateTime dateTime15;
      DateTime dateTime16;
      if (log.EDisclosureBorrowerLoanLevelConsent == "Accepted" && (log.CoBorrowerName.Trim() == "" || log.EDisclosureCoBorrowerLoanLevelConsent == "Accepted"))
      {
        if (log.IsWetSigned | policySetting && (dateTime9 != DateTime.MinValue || dateTime10 != DateTime.MinValue) || !log.IsWetSigned && !policySetting && (dateTime9 != DateTime.MinValue || dateTime10 != DateTime.MinValue))
        {
          if (dateTime9 == DateTime.MinValue)
          {
            dateTime15 = dateTime13;
            log.BorrowerDisclosedMethod = disclosedMethod;
            log.BorrowerFulfillmentMethodDescription = "Fulfillment";
          }
          else if (dateTime13 == DateTime.MinValue)
          {
            dateTime15 = dateTime9;
            log.BorrowerDisclosedMethod = log.DisclosureMethod;
            log.BorrowerFulfillmentMethodDescription = "";
          }
          else if (dateTime13 < dateTime9)
          {
            dateTime15 = dateTime13;
            log.BorrowerDisclosedMethod = disclosedMethod;
            log.BorrowerFulfillmentMethodDescription = "Fulfillment";
          }
          else
          {
            dateTime15 = dateTime9;
            log.BorrowerDisclosedMethod = log.DisclosureMethod;
            log.BorrowerFulfillmentMethodDescription = "";
          }
          if (dateTime10 == DateTime.MinValue)
          {
            dateTime16 = dateTime14;
            log.CoBorrowerDisclosedMethod = disclosedMethod;
            log.CoBorrowerFulfillmentMethodDescription = "Fulfillment";
          }
          else if (dateTime14 == DateTime.MinValue)
          {
            dateTime16 = dateTime10;
            log.CoBorrowerDisclosedMethod = log.DisclosureMethod;
            log.CoBorrowerFulfillmentMethodDescription = "";
          }
          else if (dateTime14 < dateTime10)
          {
            dateTime16 = dateTime14;
            log.CoBorrowerDisclosedMethod = disclosedMethod;
            log.CoBorrowerFulfillmentMethodDescription = "Fulfillment";
          }
          else
          {
            dateTime16 = dateTime10;
            log.CoBorrowerDisclosedMethod = log.DisclosureMethod;
            log.CoBorrowerFulfillmentMethodDescription = "";
          }
        }
        else
        {
          dateTime15 = dateTime13;
          dateTime16 = dateTime14;
          log.BorrowerDisclosedMethod = log.CoBorrowerDisclosedMethod = disclosedMethod;
          log.BorrowerFulfillmentMethodDescription = log.CoBorrowerFulfillmentMethodDescription = "Fulfillment";
        }
      }
      else if (log.EDisclosureBorrowerLoanLevelConsent == "Accepted" || log.EDisclosureCoBorrowerLoanLevelConsent == "Accepted")
      {
        if (log.IsWetSigned | policySetting && (dateTime9 != DateTime.MinValue || dateTime10 != DateTime.MinValue) || !log.IsWetSigned && !policySetting && (dateTime9 != DateTime.MinValue || dateTime10 != DateTime.MinValue))
        {
          if (log.EDisclosureBorrowerLoanLevelConsent == "Accepted")
          {
            if (dateTime9 == DateTime.MinValue)
            {
              dateTime15 = dateTime13;
              log.BorrowerDisclosedMethod = disclosedMethod;
              log.BorrowerFulfillmentMethodDescription = "Fulfillment";
            }
            else if (dateTime13 == DateTime.MinValue)
            {
              dateTime15 = dateTime9;
              log.BorrowerDisclosedMethod = log.DisclosureMethod;
              log.BorrowerFulfillmentMethodDescription = "";
            }
            else if (dateTime13 < dateTime9)
            {
              dateTime15 = dateTime13;
              log.BorrowerDisclosedMethod = disclosedMethod;
              log.BorrowerFulfillmentMethodDescription = "Fulfillment";
            }
            else
            {
              dateTime15 = dateTime9;
              log.BorrowerDisclosedMethod = log.DisclosureMethod;
              log.BorrowerFulfillmentMethodDescription = "";
            }
          }
          else
          {
            dateTime15 = log.ActualFulfillmentDate;
            log.BorrowerDisclosedMethod = disclosedMethod;
            log.BorrowerFulfillmentMethodDescription = "Fulfillment";
          }
          if (log.EDisclosureCoBorrowerLoanLevelConsent == "Accepted")
          {
            if (dateTime10 == DateTime.MinValue)
            {
              dateTime16 = dateTime14;
              log.CoBorrowerDisclosedMethod = disclosedMethod;
              log.CoBorrowerFulfillmentMethodDescription = "Fulfillment";
            }
            else if (dateTime14 == DateTime.MinValue)
            {
              dateTime16 = dateTime10;
              log.CoBorrowerDisclosedMethod = log.DisclosureMethod;
              log.CoBorrowerFulfillmentMethodDescription = "";
            }
            else if (dateTime14 < dateTime10)
            {
              dateTime16 = dateTime14;
              log.CoBorrowerDisclosedMethod = disclosedMethod;
              log.CoBorrowerFulfillmentMethodDescription = "Fulfillment";
            }
            else
            {
              dateTime16 = dateTime10;
              log.CoBorrowerDisclosedMethod = log.DisclosureMethod;
              log.CoBorrowerFulfillmentMethodDescription = "";
            }
          }
          else
          {
            dateTime16 = log.ActualFulfillmentDate;
            log.CoBorrowerDisclosedMethod = disclosedMethod;
            log.CoBorrowerFulfillmentMethodDescription = "Fulfillment";
          }
        }
        else
        {
          if (log.EDisclosureBorrowerLoanLevelConsent == "Accepted")
          {
            dateTime15 = dateTime13;
            log.BorrowerDisclosedMethod = disclosedMethod;
            log.BorrowerFulfillmentMethodDescription = "Fulfillment";
          }
          else
          {
            dateTime15 = log.ActualFulfillmentDate;
            log.BorrowerDisclosedMethod = disclosedMethod;
            log.BorrowerFulfillmentMethodDescription = "Fulfillment";
          }
          if (log.EDisclosureCoBorrowerLoanLevelConsent == "Accepted")
          {
            dateTime16 = dateTime14;
            log.CoBorrowerDisclosedMethod = disclosedMethod;
            log.CoBorrowerFulfillmentMethodDescription = "Fulfillment";
          }
          else
          {
            dateTime16 = log.ActualFulfillmentDate;
            log.CoBorrowerDisclosedMethod = disclosedMethod;
            log.CoBorrowerFulfillmentMethodDescription = "Fulfillment";
          }
        }
      }
      else
      {
        dateTime15 = dateTime16 = log.ActualFulfillmentDate;
        log.BorrowerDisclosedMethod = log.CoBorrowerDisclosedMethod = disclosedMethod;
        log.BorrowerFulfillmentMethodDescription = log.CoBorrowerFulfillmentMethodDescription = "Fulfillment";
        if (log.eDisclosureManualFulfillmentDate == DateTime.MinValue && log.FullfillmentProcessedDate == DateTime.MinValue)
          log.BorrowerFulfillmentMethodDescription = "";
        if (log.eDisclosureManualFulfillmentDate == DateTime.MinValue && log.FullfillmentProcessedDate_CoBorrower == DateTime.MinValue)
          log.CoBorrowerFulfillmentMethodDescription = "";
      }
      DateTime dateTime17;
      if (((!log.DisclosedForLE ? 0 : (!log.IsWetSigned ? 1 : 0)) & (flag2 ? 1 : 0)) != 0 && (log.EDisclosureBorrowerLoanLevelConsent != "Accepted" || log.EDisclosureCoBorrowerLoanLevelConsent != "Accepted"))
      {
        if (dateTime7 != DateTime.MinValue)
        {
          if (log.EDisclosureBorrowerLoanLevelConsent != "Accepted" && log.EDisclosureBorrowerDocumentViewedDate.Date >= dateTime6.Date && log.EDisclosureBorrowerDocumentViewedDate.Date <= dateTime7.Date)
          {
            dateTime17 = log.EDisclosureBorrowerDocumentViewedDate;
            dateTime15 = dateTime17.Date;
          }
          if (log.EDisclosureCoBorrowerLoanLevelConsent != "Accepted")
          {
            dateTime17 = log.EDisclosureCoborrowerDocumentViewedDate;
            if (dateTime17.Date >= dateTime6.Date)
            {
              dateTime17 = log.EDisclosureCoborrowerDocumentViewedDate;
              if (dateTime17.Date <= dateTime7.Date)
                dateTime16 = log.EDisclosureCoborrowerDocumentViewedDate;
            }
          }
        }
        else
          dateTime15 = dateTime16 = DateTime.MinValue;
      }
      if (((!log.DisclosedForCD ? 0 : (!log.IsWetSigned ? 1 : 0)) & (flag1 ? 1 : 0)) != 0 && (log.EDisclosureBorrowerLoanLevelConsent != "Accepted" || log.EDisclosureCoBorrowerLoanLevelConsent != "Accepted"))
      {
        if (log.EDisclosureBorrowerLoanLevelConsent != "Accepted")
        {
          dateTime17 = log.EDisclosureBorrowerDocumentViewedDate;
          if (dateTime17.Date >= dateTime5.Date)
            dateTime15 = log.EDisclosureBorrowerDocumentViewedDate;
        }
        if (log.EDisclosureCoBorrowerLoanLevelConsent != "Accepted")
        {
          dateTime17 = log.EDisclosureCoborrowerDocumentViewedDate;
          if (dateTime17.Date >= dateTime5.Date)
            dateTime16 = log.EDisclosureCoborrowerDocumentViewedDate;
        }
      }
      log.BorrowerPresumedReceivedDate = !(log.EDisclosureBorrowerLoanLevelConsent == "Accepted") ? log.PresumedFulfillmentDate : dateTime8;
      log.CoBorrowerPresumedReceivedDate = !(log.EDisclosureCoBorrowerLoanLevelConsent == "Accepted") ? log.PresumedFulfillmentDate : dateTime8;
      if (log.GetAllnboItems().Count > 0)
      {
        foreach (KeyValuePair<string, INonBorrowerOwnerItem> allnboItem in log.GetAllnboItems())
        {
          if (log.NonBorrowerOwnerCollections.ContainsKey(allnboItem.Key) && allnboItem.Value.eDisclosureNBOLoanLevelConsent == "Accepted")
          {
            DateTime dateTime18 = businessCalendar.AddBusinessDays(log.DisclosedDate, 3, true);
            log.SetnboAttributeValue(allnboItem.Key, (object) dateTime18, DisclosureTracking2015Log.NBOUpdatableFields.PresumedReceivedDate);
          }
        }
      }
      if ((log.BorrowerDisclosedMethod == DisclosureTrackingBase.DisclosedMethod.ByMail || log.BorrowerDisclosedMethod == DisclosureTrackingBase.DisclosedMethod.eDisclosure || log.BorrowerDisclosedMethod == DisclosureTrackingBase.DisclosedMethod.eClose) && (log.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.eDisclosure || log.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.eClose) && log.eDisclosureManualFulfillmentDate == DateTime.MinValue && log.FullfillmentProcessedDate == DateTime.MinValue)
      {
        log.BorrowerDisclosedMethod = log.DisclosureMethod;
        log.BorrowerFulfillmentMethodDescription = "";
      }
      if ((log.CoBorrowerDisclosedMethod == DisclosureTrackingBase.DisclosedMethod.ByMail || log.CoBorrowerDisclosedMethod == DisclosureTrackingBase.DisclosedMethod.eDisclosure || log.CoBorrowerDisclosedMethod == DisclosureTrackingBase.DisclosedMethod.eClose) && (log.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.eDisclosure || log.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.eClose) && log.eDisclosureManualFulfillmentDate == DateTime.MinValue && log.FullfillmentProcessedDate_CoBorrower == DateTime.MinValue)
      {
        log.CoBorrowerDisclosedMethod = log.DisclosureMethod;
        log.CoBorrowerFulfillmentMethodDescription = "";
      }
      if (!flag2 || !flag1 || log.IsWetSigned)
      {
        if (dateTime15 != DateTime.MinValue)
          log.BorrowerActualReceivedDate = dateTime15;
        if (!(dateTime16 != DateTime.MinValue))
          return;
        log.CoBorrowerActualReceivedDate = dateTime16;
      }
      else if (dateTime7 == DateTime.MinValue && log.DisclosedForLE)
      {
        IDisclosureTracking2015Log disclosureTracking2015Log = log;
        log.CoBorrowerActualReceivedDate = dateTime17 = DateTime.MinValue;
        DateTime dateTime19 = dateTime17;
        disclosureTracking2015Log.BorrowerActualReceivedDate = dateTime19;
      }
      else
      {
        if (dateTime15 != DateTime.MinValue)
          log.BorrowerActualReceivedDate = dateTime15;
        if (!(dateTime16 != DateTime.MinValue))
          return;
        log.CoBorrowerActualReceivedDate = dateTime16;
      }
    }

    public Dictionary<string, object> CalculateNew2015DisclosureReceivedDate(
      IDisclosureTracking2015Log log,
      DateTime disclosed,
      DateTime presumed,
      DateTime actual,
      DisclosureTrackingBase.DisclosedMethod method)
    {
      if (method == DisclosureTrackingBase.DisclosedMethod.None)
        method = log.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.eClose ? DisclosureTrackingBase.DisclosedMethod.eClose : log.DisclosureMethod;
      Dictionary<string, object> disclosureReceivedDate = new Dictionary<string, object>();
      bool policySetting = (bool) this.sessionObjects.StartupInfo.PolicySettings[(object) "Policies.eSignReceivedDate"];
      bool flag1 = this.UseNewCompliance(19.1M);
      string field1 = this.loanData.GetField("3152");
      string field2 = this.loanData.GetField("3143");
      DateTime dateTime1 = string.IsNullOrEmpty(field1) ? DateTime.MinValue : Utils.ParseDate((object) field1);
      DateTime dateTime2 = string.IsNullOrEmpty(field2) ? DateTime.MinValue : Utils.ParseDate((object) field2);
      bool flag2 = this.UseNewCompliance(19.4M);
      string field3 = this.loanData.GetField("3977");
      DateTime dateTime3 = string.IsNullOrEmpty(field3) ? DateTime.MinValue : Utils.ParseDate((object) field3);
      disclosureReceivedDate.Add("BorrowerActualReceivedDate", (object) null);
      disclosureReceivedDate.Add("CoBorrowerActualReceivedDate", (object) null);
      disclosureReceivedDate.Add("BorrowerDisclosedMethod", (object) null);
      disclosureReceivedDate.Add("CoBorrowerDisclosedMethod", (object) null);
      disclosureReceivedDate.Add("BorrowerPresumedReceivedDate", (object) null);
      disclosureReceivedDate.Add("CoBorrowerPresumedReceivedDate", (object) null);
      disclosureReceivedDate.Add("BorrowerFulfillmentMethodDescription", (object) "");
      disclosureReceivedDate.Add("CoBorrowerFulfillmentMethodDescription", (object) "");
      BusinessCalendar businessCalendar = this.sessionObjects.GetBusinessCalendar(CalendarType.RegZ);
      DateTime dateTime4 = new DateTime();
      DateTime dateTime5 = new DateTime();
      DateTime dateTime6 = businessCalendar.AddBusinessDays(disclosed, 3, true);
      DateTime dateTime7;
      DateTime dateTime8;
      if (log.IsWetSigned)
      {
        dateTime7 = log.EDisclosureBorrowerDocumentViewedDate;
        dateTime8 = log.EDisclosureCoborrowerDocumentViewedDate;
      }
      else if (policySetting)
      {
        dateTime7 = log.eDisclosureBorrowereSignedDate;
        dateTime8 = log.eDisclosureCoBorrowereSignedDate;
      }
      else
      {
        dateTime7 = log.EDisclosureBorrowerDocumentViewedDate;
        dateTime8 = log.EDisclosureCoborrowerDocumentViewedDate;
      }
      DateTime fullfillmentProcessedDate = log.FullfillmentProcessedDate;
      DateTime processedDateCoBorrower = log.FullfillmentProcessedDate_CoBorrower;
      if (presumed == DateTime.MinValue && log.eDisclosureManualFulfillmentDate != DateTime.MinValue)
        presumed = businessCalendar.AddBusinessDays(log.eDisclosureManualFulfillmentDate, 3, true);
      DateTime dateTime9 = presumed > actual ? actual : presumed;
      DateTime dateTime10 = presumed > actual ? actual : presumed;
      if (log.EDisclosureBorrowerLoanLevelConsent == "Accepted" && (log.CoBorrowerName.Trim() == "" || log.EDisclosureCoBorrowerLoanLevelConsent == "Accepted"))
      {
        if (log.IsWetSigned | policySetting && (dateTime7 != DateTime.MinValue || dateTime8 != DateTime.MinValue) || !log.IsWetSigned && !policySetting && (dateTime7 != DateTime.MinValue || dateTime8 != DateTime.MinValue))
        {
          if (dateTime7 == DateTime.MinValue)
          {
            disclosureReceivedDate["BorrowerActualReceivedDate"] = (object) dateTime9;
            disclosureReceivedDate["BorrowerDisclosedMethod"] = (object) method;
            disclosureReceivedDate["BorrowerFulfillmentMethodDescription"] = (object) "Fulfillment";
          }
          else if (dateTime9 == DateTime.MinValue)
          {
            disclosureReceivedDate["BorrowerActualReceivedDate"] = (object) dateTime7;
            disclosureReceivedDate["BorrowerDisclosedMethod"] = (object) log.DisclosureMethod;
            disclosureReceivedDate["BorrowerFulfillmentMethodDescription"] = (object) "";
          }
          else if (dateTime9 < dateTime7)
          {
            disclosureReceivedDate["BorrowerActualReceivedDate"] = (object) dateTime9;
            disclosureReceivedDate["BorrowerDisclosedMethod"] = (object) method;
            disclosureReceivedDate["BorrowerFulfillmentMethodDescription"] = (object) "Fulfillment";
          }
          else
          {
            disclosureReceivedDate["BorrowerActualReceivedDate"] = (object) dateTime7;
            disclosureReceivedDate["BorrowerDisclosedMethod"] = (object) log.DisclosureMethod;
            disclosureReceivedDate["BorrowerFulfillmentMethodDescription"] = (object) "";
          }
          if (dateTime8 == DateTime.MinValue)
          {
            disclosureReceivedDate["CoBorrowerActualReceivedDate"] = (object) dateTime10;
            disclosureReceivedDate["CoBorrowerDisclosedMethod"] = (object) method;
            disclosureReceivedDate["CoBorrowerFulfillmentMethodDescription"] = (object) "Fulfillment";
          }
          else if (dateTime10 == DateTime.MinValue)
          {
            disclosureReceivedDate["CoBorrowerActualReceivedDate"] = (object) dateTime8;
            disclosureReceivedDate["CoBorrowerDisclosedMethod"] = (object) log.DisclosureMethod;
            disclosureReceivedDate["CoBorrowerFulfillmentMethodDescription"] = (object) "";
          }
          else if (dateTime10 < dateTime8)
          {
            disclosureReceivedDate["CoBorrowerActualReceivedDate"] = (object) dateTime10;
            disclosureReceivedDate["CoBorrowerDisclosedMethod"] = (object) method;
            disclosureReceivedDate["CoBorrowerFulfillmentMethodDescription"] = (object) "Fulfillment";
          }
          else
          {
            disclosureReceivedDate["CoBorrowerActualReceivedDate"] = (object) dateTime8;
            disclosureReceivedDate["CoBorrowerDisclosedMethod"] = (object) log.DisclosureMethod;
            disclosureReceivedDate["CoBorrowerFulfillmentMethodDescription"] = (object) "";
          }
        }
        else
        {
          disclosureReceivedDate["BorrowerActualReceivedDate"] = (object) dateTime9;
          disclosureReceivedDate["CoBorrowerActualReceivedDate"] = (object) dateTime10;
          disclosureReceivedDate["BorrowerDisclosedMethod"] = disclosureReceivedDate["CoBorrowerDisclosedMethod"] = (object) method;
          disclosureReceivedDate["BorrowerFulfillmentMethodDescription"] = disclosureReceivedDate["CoBorrowerFulfillmentMethodDescription"] = (object) "Fulfillment";
        }
      }
      else if (log.EDisclosureBorrowerLoanLevelConsent == "Accepted" || log.EDisclosureCoBorrowerLoanLevelConsent == "Accepted")
      {
        if (log.IsWetSigned | policySetting && (dateTime7 != DateTime.MinValue || dateTime8 != DateTime.MinValue) || !log.IsWetSigned && !policySetting && (dateTime7 != DateTime.MinValue || dateTime8 != DateTime.MinValue))
        {
          if (log.EDisclosureBorrowerLoanLevelConsent == "Accepted")
          {
            if (dateTime7 == DateTime.MinValue)
            {
              disclosureReceivedDate["BorrowerActualReceivedDate"] = (object) dateTime9;
              disclosureReceivedDate["BorrowerDisclosedMethod"] = (object) method;
              disclosureReceivedDate["BorrowerFulfillmentMethodDescription"] = (object) "Fulfillment";
            }
            else if (dateTime9 == DateTime.MinValue)
            {
              disclosureReceivedDate["BorrowerActualReceivedDate"] = (object) dateTime7;
              disclosureReceivedDate["BorrowerDisclosedMethod"] = (object) log.DisclosureMethod;
              disclosureReceivedDate["BorrowerFulfillmentMethodDescription"] = (object) "";
            }
            else if (dateTime9 < dateTime7)
            {
              disclosureReceivedDate["BorrowerActualReceivedDate"] = (object) dateTime9;
              disclosureReceivedDate["BorrowerDisclosedMethod"] = (object) method;
              disclosureReceivedDate["BorrowerFulfillmentMethodDescription"] = (object) "Fulfillment";
            }
            else
            {
              disclosureReceivedDate["BorrowerActualReceivedDate"] = (object) dateTime7;
              disclosureReceivedDate["BorrowerDisclosedMethod"] = (object) log.DisclosureMethod;
              disclosureReceivedDate["BorrowerFulfillmentMethodDescription"] = (object) "";
            }
          }
          else
          {
            disclosureReceivedDate["BorrowerActualReceivedDate"] = (object) actual;
            disclosureReceivedDate["BorrowerDisclosedMethod"] = (object) method;
            disclosureReceivedDate["BorrowerFulfillmentMethodDescription"] = (object) "Fulfillment";
          }
          if (log.EDisclosureCoBorrowerLoanLevelConsent == "Accepted")
          {
            if (dateTime8 == DateTime.MinValue)
            {
              disclosureReceivedDate["CoBorrowerActualReceivedDate"] = (object) dateTime10;
              disclosureReceivedDate["CoBorrowerDisclosedMethod"] = (object) method;
              disclosureReceivedDate["CoBorrowerFulfillmentMethodDescription"] = (object) "Fulfillment";
            }
            else if (dateTime10 == DateTime.MinValue)
            {
              disclosureReceivedDate["CoBorrowerActualReceivedDate"] = (object) dateTime8;
              disclosureReceivedDate["CoBorrowerDisclosedMethod"] = (object) log.DisclosureMethod;
            }
            else if (dateTime10 < dateTime8)
            {
              disclosureReceivedDate["CoBorrowerActualReceivedDate"] = (object) dateTime10;
              disclosureReceivedDate["CoBorrowerDisclosedMethod"] = (object) method;
              disclosureReceivedDate["CoBorrowerFulfillmentMethodDescription"] = (object) "Fulfillment";
            }
            else
            {
              disclosureReceivedDate["CoBorrowerActualReceivedDate"] = (object) dateTime8;
              disclosureReceivedDate["CoBorrowerDisclosedMethod"] = (object) log.DisclosureMethod;
              disclosureReceivedDate["CoBorrowerFulfillmentMethodDescription"] = (object) "";
            }
          }
          else
          {
            disclosureReceivedDate["CoBorrowerActualReceivedDate"] = (object) actual;
            disclosureReceivedDate["CoBorrowerDisclosedMethod"] = (object) method;
            disclosureReceivedDate["CoBorrowerFulfillmentMethodDescription"] = !(dateTime10 != DateTime.MinValue) ? (object) "" : (object) "Fulfillment";
          }
        }
        else
        {
          if (log.EDisclosureBorrowerLoanLevelConsent == "Accepted")
          {
            if (dateTime9 != DateTime.MinValue)
            {
              disclosureReceivedDate["BorrowerActualReceivedDate"] = (object) dateTime9;
              disclosureReceivedDate["BorrowerDisclosedMethod"] = (object) method;
              disclosureReceivedDate["BorrowerFulfillmentMethodDescription"] = (object) "Fulfillment";
            }
            else
            {
              disclosureReceivedDate["BorrowerDisclosedMethod"] = (object) log.DisclosureMethod;
              disclosureReceivedDate["BorrowerFulfillmentMethodDescription"] = (object) "";
            }
          }
          else
          {
            disclosureReceivedDate["BorrowerActualReceivedDate"] = (object) actual;
            disclosureReceivedDate["BorrowerDisclosedMethod"] = (object) method;
            disclosureReceivedDate["BorrowerFulfillmentMethodDescription"] = !(dateTime9 != DateTime.MinValue) ? (object) "" : (object) "Fulfillment";
          }
          if (log.EDisclosureCoBorrowerLoanLevelConsent == "Accepted")
          {
            if (dateTime10 != DateTime.MinValue)
            {
              disclosureReceivedDate["CoBorrowerActualReceivedDate"] = (object) dateTime10;
              disclosureReceivedDate["CoBorrowerDisclosedMethod"] = (object) method;
              disclosureReceivedDate["CoBorrowerFulfillmentMethodDescription"] = (object) "Fulfillment";
            }
            else
            {
              disclosureReceivedDate["CoBorrowerDisclosedMethod"] = (object) log.DisclosureMethod;
              disclosureReceivedDate["CoBorrowerFulfillmentMethodDescription"] = (object) "";
            }
          }
          else
          {
            disclosureReceivedDate["CoBorrowerActualReceivedDate"] = (object) actual;
            disclosureReceivedDate["CoBorrowerDisclosedMethod"] = (object) method;
            disclosureReceivedDate["CoBorrowerFulfillmentMethodDescription"] = !(dateTime10 != DateTime.MinValue) ? (object) "" : (object) "Fulfillment";
          }
        }
      }
      else
      {
        disclosureReceivedDate["BorrowerActualReceivedDate"] = disclosureReceivedDate["CoBorrowerActualReceivedDate"] = (object) actual;
        disclosureReceivedDate["BorrowerDisclosedMethod"] = disclosureReceivedDate["CoBorrowerDisclosedMethod"] = (object) method;
        disclosureReceivedDate["BorrowerFulfillmentMethodDescription"] = disclosureReceivedDate["CoBorrowerFulfillmentMethodDescription"] = (object) "Fulfillment";
      }
      if (((!log.DisclosedForLE ? 0 : (!log.IsWetSigned ? 1 : 0)) & (flag1 ? 1 : 0)) != 0 && (log.EDisclosureBorrowerLoanLevelConsent != "Accepted" || log.EDisclosureCoBorrowerLoanLevelConsent != "Accepted"))
      {
        if (dateTime2 != DateTime.MinValue)
        {
          if (log.EDisclosureBorrowerLoanLevelConsent != "Accepted" && log.EDisclosureBorrowerDocumentViewedDate.Date >= dateTime1.Date && log.EDisclosureBorrowerDocumentViewedDate.Date <= dateTime2.Date)
            disclosureReceivedDate["BorrowerActualReceivedDate"] = (object) log.EDisclosureBorrowerDocumentViewedDate;
          if (log.EDisclosureCoBorrowerLoanLevelConsent != "Accepted" && log.EDisclosureCoborrowerDocumentViewedDate.Date >= dateTime1.Date && log.EDisclosureCoborrowerDocumentViewedDate.Date <= dateTime2.Date)
            disclosureReceivedDate["CoBorrowerActualReceivedDate"] = (object) log.EDisclosureCoborrowerDocumentViewedDate;
        }
        else
          disclosureReceivedDate["BorrowerActualReceivedDate"] = disclosureReceivedDate["CoBorrowerActualReceivedDate"] = (object) DateTime.MinValue;
      }
      if (((!log.DisclosedForCD ? 0 : (!log.IsWetSigned ? 1 : 0)) & (flag2 ? 1 : 0)) != 0 && (log.EDisclosureBorrowerLoanLevelConsent != "Accepted" || log.EDisclosureCoBorrowerLoanLevelConsent != "Accepted"))
      {
        if (log.EDisclosureBorrowerLoanLevelConsent != "Accepted" && log.EDisclosureBorrowerDocumentViewedDate.Date >= dateTime3.Date)
          disclosureReceivedDate["BorrowerActualReceivedDate"] = (object) log.EDisclosureBorrowerDocumentViewedDate;
        if (log.EDisclosureCoBorrowerLoanLevelConsent != "Accepted" && log.EDisclosureCoborrowerDocumentViewedDate.Date >= dateTime3.Date)
          disclosureReceivedDate["CoBorrowerActualReceivedDate"] = (object) log.EDisclosureCoborrowerDocumentViewedDate;
      }
      disclosureReceivedDate["BorrowerPresumedReceivedDate"] = !(log.EDisclosureBorrowerLoanLevelConsent == "Accepted") ? (object) presumed : (object) dateTime6;
      disclosureReceivedDate["CoBorrowerPresumedReceivedDate"] = !(log.EDisclosureCoBorrowerLoanLevelConsent == "Accepted") ? (object) presumed : (object) dateTime6;
      if ((method == DisclosureTrackingBase.DisclosedMethod.None || method == DisclosureTrackingBase.DisclosedMethod.eDisclosure || method == DisclosureTrackingBase.DisclosedMethod.eClose) && (log.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.eDisclosure || log.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.eClose) && log.eDisclosureManualFulfillmentDate == DateTime.MinValue && log.FullfillmentProcessedDate == DateTime.MinValue)
      {
        disclosureReceivedDate["BorrowerDisclosedMethod"] = (object) log.DisclosureMethod;
        disclosureReceivedDate["BorrowerFulfillmentMethodDescription"] = (object) "";
      }
      if ((method == DisclosureTrackingBase.DisclosedMethod.None || method == DisclosureTrackingBase.DisclosedMethod.eDisclosure || method == DisclosureTrackingBase.DisclosedMethod.eClose) && (log.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.eDisclosure || log.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.eClose) && log.eDisclosureManualFulfillmentDate == DateTime.MinValue && log.FullfillmentProcessedDate_CoBorrower == DateTime.MinValue)
      {
        disclosureReceivedDate["CoBorrowerDisclosedMethod"] = (object) log.DisclosureMethod;
        disclosureReceivedDate["CoBorrowerFulfillmentMethodDescription"] = (object) "";
      }
      if (((!log.DisclosedForLE ? 0 : (dateTime2 == DateTime.MinValue ? 1 : 0)) & (flag1 ? 1 : 0)) != 0)
      {
        disclosureReceivedDate["BorrowerActualReceivedDate"] = (object) DateTime.MinValue;
        disclosureReceivedDate["CoBorrowerActualReceivedDate"] = (object) DateTime.MinValue;
      }
      return disclosureReceivedDate;
    }

    public Dictionary<string, object> CalculateNew2015DisclosureNBOReceivedDate(
      IDisclosureTracking2015Log log,
      DateTime disclosed)
    {
      Dictionary<string, object> disclosureNboReceivedDate = new Dictionary<string, object>();
      bool policySetting = (bool) this.sessionObjects.StartupInfo.PolicySettings[(object) "Policies.eSignReceivedDate"];
      DateTime dateTime1 = this.sessionObjects.GetBusinessCalendar(CalendarType.RegZ).AddBusinessDays(disclosed, 3, true);
      if (log.GetAllnboItems().Count > 0)
      {
        foreach (KeyValuePair<string, INonBorrowerOwnerItem> allnboItem in log.GetAllnboItems())
        {
          DateTime dateTime2 = new DateTime();
          DateTime dateTime3 = !log.IsWetSigned ? (!policySetting ? Utils.ParseDate((object) allnboItem.Value.eDisclosureNBODocumentViewedDate) : Utils.ParseDate((object) allnboItem.Value.eDisclosureNBOSignedDate)) : Utils.ParseDate((object) allnboItem.Value.eDisclosureNBODocumentViewedDate);
          string key = allnboItem.Key + "_ActualReceivedDate";
          disclosureNboReceivedDate.Add(key, (object) dateTime3);
        }
      }
      if (log.GetAllnboItems().Count > 0)
      {
        foreach (KeyValuePair<string, INonBorrowerOwnerItem> allnboItem in log.GetAllnboItems())
        {
          DateTime dateTime4 = new DateTime();
          if (allnboItem.Value.eDisclosureNBOLoanLevelConsent == "Accepted")
            dateTime4 = Utils.ParseDate((object) dateTime1);
          string key = allnboItem.Key + "_PresumedReceiveDate";
          disclosureNboReceivedDate.Add(key, (object) dateTime4);
        }
      }
      return disclosureNboReceivedDate;
    }

    public DateTime CalculateNeweDisclosureReceivedDate(
      DisclosureTrackingBase log,
      DisclosurePackage detailInfo)
    {
      if (log.eDisclosureManualFulfillmentDate == DateTime.MinValue && log.FullfillmentProcessedDate == DateTime.MinValue)
        return this.geteDisclosureReceivedDate(detailInfo);
      DateTime dateTime1 = this.geteDisclosureReceivedDate(detailInfo);
      DateTime dateTime2 = !(log.FulfillmentOrderedBy_CoBorrower == "") ? (log.FullfillmentProcessedDate > log.FullfillmentProcessedDate_CoBorrower ? log.FullfillmentProcessedDate : log.FullfillmentProcessedDate_CoBorrower) : log.FullfillmentProcessedDate;
      DateTime disclosureReceivedDate = this.CalculateNewDisclosureReceivedDate(DisclosureTrackingBase.DisclosedMethod.ByMail, log.eDisclosureManualFulfillmentDate == DateTime.MinValue ? dateTime2 : log.eDisclosureManualFulfillmentDate, log.ReceivedDate);
      return dateTime1 == DateTime.MinValue || !(disclosureReceivedDate == DateTime.MinValue) && disclosureReceivedDate < dateTime1 ? disclosureReceivedDate : dateTime1;
    }

    public DateTime CalculateNeweDisclosureReceivedDate(DisclosureTrackingLog log)
    {
      if (log.eDisclosureManualFulfillmentDate == DateTime.MinValue && log.FullfillmentProcessedDate == DateTime.MinValue)
        return this.geteDisclosureReceivedDate(log);
      DateTime dateTime1 = this.geteDisclosureReceivedDate(log);
      DateTime dateTime2 = !(log.FulfillmentOrderedBy_CoBorrower == "") ? (log.FullfillmentProcessedDate > log.FullfillmentProcessedDate_CoBorrower ? log.FullfillmentProcessedDate : log.FullfillmentProcessedDate_CoBorrower) : log.FullfillmentProcessedDate;
      DateTime disclosureReceivedDate = this.CalculateNewDisclosureReceivedDate(log.eDisclosureManualFulfillmentMethod, log.eDisclosureManualFulfillmentDate == DateTime.MinValue ? dateTime2 : log.eDisclosureManualFulfillmentDate, log.ReceivedDate);
      return dateTime1 == DateTime.MinValue || !(disclosureReceivedDate == DateTime.MinValue) && disclosureReceivedDate < dateTime1 ? disclosureReceivedDate : dateTime1;
    }

    public DateTime CalculateNewDisclosureReceivedDate(
      DisclosureTrackingBase.DisclosedMethod method,
      DateTime disclosedDate,
      DateTime currentReceivedDate)
    {
      switch (method)
      {
        case DisclosureTrackingBase.DisclosedMethod.ByMail:
          return this.sessionObjects.ConfigurationManager.AddBusinessDays(CalendarType.Postal, disclosedDate, 3, true);
        case DisclosureTrackingBase.DisclosedMethod.Fax:
        case DisclosureTrackingBase.DisclosedMethod.InPerson:
        case DisclosureTrackingBase.DisclosedMethod.Email:
        case DisclosureTrackingBase.DisclosedMethod.Phone:
        case DisclosureTrackingBase.DisclosedMethod.Signature:
          return disclosedDate;
        case DisclosureTrackingBase.DisclosedMethod.Other:
          return currentReceivedDate != DateTime.MinValue && disclosedDate.ToString("MM/dd/yyyy") != disclosedDate.ToString("MM/dd/yyyy") && currentReceivedDate < disclosedDate ? DateTime.MinValue : currentReceivedDate;
        default:
          return currentReceivedDate;
      }
    }

    public DateTime CalculateNewDisclosure2015ReceivedDate(
      DisclosureTrackingBase.DisclosedMethod method,
      DateTime disclosedDate,
      DateTime currentReceivedDate)
    {
      switch (method)
      {
        case DisclosureTrackingBase.DisclosedMethod.ByMail:
          return this.sessionObjects.ConfigurationManager.AddBusinessDays(CalendarType.RegZ, disclosedDate, 3, true);
        case DisclosureTrackingBase.DisclosedMethod.Fax:
        case DisclosureTrackingBase.DisclosedMethod.InPerson:
        case DisclosureTrackingBase.DisclosedMethod.Email:
        case DisclosureTrackingBase.DisclosedMethod.Phone:
        case DisclosureTrackingBase.DisclosedMethod.Signature:
          return disclosedDate;
        case DisclosureTrackingBase.DisclosedMethod.Other:
          return currentReceivedDate != DateTime.MinValue && disclosedDate.ToString("MM/dd/yyyy") != disclosedDate.ToString("MM/dd/yyyy") && currentReceivedDate < disclosedDate ? DateTime.MinValue : currentReceivedDate;
        default:
          return currentReceivedDate;
      }
    }

    public void CalculateLatestDisclosure(
      DisclosureTrackingLog updatedLog,
      bool updateAPRFinanceChange = true)
    {
      if (this.inDisclosureCal)
        return;
      this.inDisclosureCal = true;
      try
      {
        if (updatedLog != null)
        {
          switch (updatedLog.DisclosureMethod)
          {
            case DisclosureTrackingBase.DisclosedMethod.ByMail:
            case DisclosureTrackingBase.DisclosedMethod.Fax:
            case DisclosureTrackingBase.DisclosedMethod.InPerson:
              updatedLog.SetReceivedDateFromCalc = this.CalculateNewDisclosureReceivedDate(updatedLog.DisclosureMethod, updatedLog.DisclosedDate, updatedLog.ReceivedDate);
              break;
            case DisclosureTrackingBase.DisclosedMethod.eDisclosure:
              updatedLog.SetReceivedDateFromCalc = this.CalculateNeweDisclosureReceivedDate(updatedLog);
              break;
            case DisclosureTrackingBase.DisclosedMethod.Other:
              updatedLog.ReceivedDate = this.CalculateNewDisclosureReceivedDate(updatedLog.DisclosureMethod, updatedLog.DisclosedDate, updatedLog.ReceivedDate);
              break;
          }
        }
        DisclosureTrackingLog disclosureTrackingLog1 = this.loanData.GetLogList().GetInitialDisclosureTrackingLog(DisclosureTrackingLog.DisclosureTrackingType.GFE);
        DisclosureTrackingLog disclosureTrackingLog2 = this.loanData.GetLogList().GetInitialDisclosureTrackingLog(DisclosureTrackingLog.DisclosureTrackingType.TIL);
        DisclosureTrackingLog redisclosureTrackingLog1 = this.loanData.GetLogList().GetRedisclosureTrackingLog(DisclosureTrackingLog.DisclosureTrackingType.GFE);
        DisclosureTrackingLog redisclosureTrackingLog2 = this.loanData.GetLogList().GetRedisclosureTrackingLog(DisclosureTrackingLog.DisclosureTrackingType.TIL);
        DateTime dateTime;
        if (disclosureTrackingLog1 != null)
        {
          LoanData loanData1 = this.loanData;
          dateTime = disclosureTrackingLog1.DisclosedDate;
          string val1 = dateTime.ToString("MM/dd/yyyy");
          loanData1.SetCurrentField("3148", val1);
          if (disclosureTrackingLog1.ReceivedDate != DateTime.MinValue)
          {
            LoanData loanData2 = this.loanData;
            dateTime = disclosureTrackingLog1.ReceivedDate;
            string val2 = dateTime.ToString("MM/dd/yyyy");
            loanData2.SetCurrentField("3149", val2);
          }
          else
            this.loanData.SetCurrentField("3149", "");
        }
        else
        {
          this.loanData.SetCurrentField("3148", "");
          this.loanData.SetCurrentField("3149", "");
        }
        this.updateBorrowerIntentToProceed();
        if (disclosureTrackingLog2 != null)
        {
          LoanData loanData3 = this.loanData;
          dateTime = disclosureTrackingLog2.DisclosedDate;
          string val3 = dateTime.ToString("MM/dd/yyyy");
          loanData3.SetCurrentField("3152", val3);
          if (disclosureTrackingLog2.ReceivedDate != DateTime.MinValue)
          {
            LoanData loanData4 = this.loanData;
            dateTime = disclosureTrackingLog2.ReceivedDate;
            string val4 = dateTime.ToString("MM/dd/yyyy");
            loanData4.SetCurrentField("3153", val4);
          }
          else
            this.loanData.SetCurrentField("3153", "");
        }
        else
        {
          this.loanData.SetCurrentField("3152", "");
          this.loanData.SetCurrentField("3153", "");
        }
        if (redisclosureTrackingLog1 != null)
        {
          LoanData loanData5 = this.loanData;
          dateTime = redisclosureTrackingLog1.DisclosedDate;
          string val5 = dateTime.ToString("MM/dd/yyyy");
          loanData5.SetCurrentField("3150", val5);
          if (redisclosureTrackingLog1.ReceivedDate != DateTime.MinValue)
          {
            LoanData loanData6 = this.loanData;
            dateTime = redisclosureTrackingLog1.ReceivedDate;
            string val6 = dateTime.ToString("MM/dd/yyyy");
            loanData6.SetCurrentField("3151", val6);
          }
          else
            this.loanData.SetCurrentField("3151", "");
        }
        else
        {
          this.loanData.SetCurrentField("3150", "");
          this.loanData.SetCurrentField("3151", "");
        }
        if (redisclosureTrackingLog2 != null)
        {
          LoanData loanData7 = this.loanData;
          dateTime = redisclosureTrackingLog2.DisclosedDate;
          string val7 = dateTime.ToString("MM/dd/yyyy");
          loanData7.SetCurrentField("3154", val7);
          if (redisclosureTrackingLog2.ReceivedDate != DateTime.MinValue)
          {
            LoanData loanData8 = this.loanData;
            dateTime = redisclosureTrackingLog2.ReceivedDate;
            string val8 = dateTime.ToString("MM/dd/yyyy");
            loanData8.SetCurrentField("3155", val8);
          }
          else
            this.loanData.SetCurrentField("3155", "");
        }
        else
        {
          this.loanData.SetCurrentField("3154", "");
          this.loanData.SetCurrentField("3155", "");
        }
        DisclosureTrackingLog disclosureTrackingLog3 = this.loanData.GetLogList().GetLatestDisclosureTrackingLog(DisclosureTrackingLog.DisclosureTrackingType.GFE);
        if (disclosureTrackingLog3 == null)
        {
          this.loanData.SetCurrentField("3137", "");
          this.loanData.SetCurrentField("3138", "");
          this.loanData.SetCurrentField("3139", "");
          this.loanData.SetCurrentField("3141", "");
          this.loanData.SetCurrentField("3163", "//");
        }
        else
        {
          LoanData loanData9 = this.loanData;
          dateTime = disclosureTrackingLog3.DisclosedDate;
          string val9 = dateTime.ToString("MM/dd/yyyy");
          loanData9.SetCurrentField("3137", val9);
          this.loanData.SetCurrentField("3138", disclosureTrackingLog3.DisclosedMethodName);
          if (disclosureTrackingLog3.IsDisclosedByLocked)
            this.loanData.SetCurrentField("3139", disclosureTrackingLog3.DisclosedByFullName);
          else
            this.loanData.SetCurrentField("3139", disclosureTrackingLog3.DisclosedByFullName + "(" + disclosureTrackingLog3.DisclosedBy + ")");
          this.loanData.SetCurrentField("3141", disclosureTrackingLog3.Comments);
          if (disclosureTrackingLog3.ReceivedDate != DateTime.MinValue)
          {
            LoanData loanData10 = this.loanData;
            dateTime = disclosureTrackingLog3.ReceivedDate;
            string val10 = dateTime.ToString("MM/dd/yyyy");
            loanData10.SetCurrentField("3163", val10);
          }
          else
            this.loanData.SetCurrentField("3163", "");
        }
        this.calObjs.NewHudCal.CalcGFERedisclosureFlag((string) null, (string) null);
        DisclosureTrackingLog disclosureTrackingLog4 = this.loanData.GetLogList().GetLatestDisclosureTrackingLog(DisclosureTrackingLog.DisclosureTrackingType.TIL);
        if (disclosureTrackingLog4 == null)
        {
          this.loanData.SetCurrentFieldFromCal("3156", "");
          this.loanData.SetCurrentField("3157", "");
          this.loanData.SetCurrentField("3158", "");
          this.loanData.SetCurrentField("3159", "");
          this.loanData.SetCurrentFieldFromCal("3121", "");
          this.loanData.SetCurrentFieldFromCal("3246", "");
          this.loanData.SetCurrentFieldFromCal("3247", "");
          this.loanData.SetCurrentFieldFromCal("3887", "");
        }
        else
        {
          if (updateAPRFinanceChange)
          {
            LoanData loanData = this.loanData;
            dateTime = disclosureTrackingLog4.DisclosedDate;
            string val = dateTime.ToString("MM/dd/yyyy");
            loanData.SetCurrentFieldFromCal("3156", val);
            this.loanData.SetCurrentFieldFromCal("3121", disclosureTrackingLog4.DisclosedAPR);
            this.loanData.SetCurrentFieldFromCal("3246", disclosureTrackingLog4.FinanceCharge);
            this.loanData.SetCurrentFieldFromCal("3887", disclosureTrackingLog4.DisclosedDailyInterest);
          }
          this.loanData.SetCurrentField("3157", disclosureTrackingLog4.DisclosedMethodName);
          if (disclosureTrackingLog4.IsDisclosedByLocked)
            this.loanData.SetCurrentField("3158", disclosureTrackingLog4.DisclosedByFullName);
          else
            this.loanData.SetCurrentField("3158", disclosureTrackingLog4.DisclosedByFullName + "(" + disclosureTrackingLog4.DisclosedBy + ")");
          this.loanData.SetCurrentField("3159", disclosureTrackingLog4.Comments);
          if (disclosureTrackingLog4.ReceivedDate != DateTime.MinValue && disclosureTrackingLog4.ReceivedDate != DateTime.MaxValue)
          {
            LoanData loanData = this.loanData;
            dateTime = disclosureTrackingLog4.ReceivedDate;
            string val = dateTime.ToString("MM/dd/yyyy");
            loanData.SetCurrentFieldFromCal("3247", val);
          }
          else
            this.loanData.SetCurrentFieldFromCal("3247", "");
        }
        this.calculateDisclosureComplicanceTimeline();
      }
      catch (ComplianceCalendarException ex)
      {
        throw ex;
      }
      catch (Exception ex)
      {
        if (ex.InnerException is ComplianceCalendarException)
          throw ex.InnerException;
        throw ex;
      }
      finally
      {
        this.inDisclosureCal = false;
      }
    }

    private DateTime getReceivedDateNew(IDisclosureTracking2015Log cdLog, bool isRescindable)
    {
      if (!cdLog.DisclosedForCD && !cdLog.DisclosedForLE)
        return DateTime.MinValue;
      DateTime receivedDateNew1 = new DateTime();
      if (((!this.isValidBorrowerTypeForReceivedDate(cdLog, true) ? 0 : (!isRescindable ? 1 : 0)) | (isRescindable ? 1 : 0)) != 0)
      {
        receivedDateNew1 = cdLog.BorrowerActualReceivedDate;
        if (cdLog.IsBorrowerPresumedDateLocked)
        {
          if (cdLog.LockedBorrowerPresumedReceivedDate != DateTime.MinValue && (cdLog.LockedBorrowerPresumedReceivedDate < receivedDateNew1 || receivedDateNew1 == DateTime.MinValue))
            receivedDateNew1 = cdLog.LockedBorrowerPresumedReceivedDate;
        }
        else if (cdLog.BorrowerPresumedReceivedDate != DateTime.MinValue && (cdLog.BorrowerPresumedReceivedDate < receivedDateNew1 || receivedDateNew1 == DateTime.MinValue))
          receivedDateNew1 = cdLog.BorrowerPresumedReceivedDate;
      }
      DateTime dateTime1 = new DateTime();
      if (cdLog.IsNboExist & isRescindable && cdLog.DisclosedForCD)
      {
        foreach (KeyValuePair<string, INonBorrowerOwnerItem> allnboItem in cdLog.GetAllnboItems())
        {
          DateTime dateTime2 = new DateTime();
          if (this.isValidNBOTypeForReceivedDate(allnboItem.Value))
          {
            INonBorrowerOwnerItem borrowerOwnerItem = allnboItem.Value;
            DateTime dateTime3 = borrowerOwnerItem.ActualReceivedDate;
            if (borrowerOwnerItem.isPresumedDateLocked)
            {
              if (borrowerOwnerItem.lockedPresumedReceivedDate != DateTime.MinValue && (borrowerOwnerItem.lockedPresumedReceivedDate < dateTime3 || dateTime3 == DateTime.MinValue))
                dateTime3 = borrowerOwnerItem.lockedPresumedReceivedDate;
            }
            else if (borrowerOwnerItem.PresumedReceivedDate != DateTime.MinValue && (borrowerOwnerItem.PresumedReceivedDate < dateTime3 || dateTime3 == DateTime.MinValue))
              dateTime3 = borrowerOwnerItem.PresumedReceivedDate;
            if (dateTime1 == DateTime.MinValue || dateTime1 < dateTime3)
              dateTime1 = dateTime3;
          }
        }
      }
      if (cdLog.CoBorrowerName.Trim() == "")
      {
        if (!isRescindable || !cdLog.IsNboExist || !cdLog.DisclosedForCD)
          return receivedDateNew1;
        if (receivedDateNew1 == DateTime.MinValue || dateTime1 == DateTime.MinValue)
          return DateTime.MinValue;
        return !(receivedDateNew1 > dateTime1) ? dateTime1 : receivedDateNew1;
      }
      DateTime dateTime4 = new DateTime();
      if (((!this.isValidBorrowerTypeForReceivedDate(cdLog, false) ? 0 : (!isRescindable ? 1 : 0)) | (isRescindable ? 1 : 0)) != 0)
      {
        dateTime4 = cdLog.CoBorrowerActualReceivedDate;
        if (cdLog.IsCoBorrowerPresumedDateLocked)
        {
          if (cdLog.LockedCoBorrowerPresumedReceivedDate != DateTime.MinValue && (cdLog.LockedCoBorrowerPresumedReceivedDate < dateTime4 || dateTime4 == DateTime.MinValue))
            dateTime4 = cdLog.LockedCoBorrowerPresumedReceivedDate;
        }
        else if (cdLog.CoBorrowerPresumedReceivedDate != DateTime.MinValue && (cdLog.CoBorrowerPresumedReceivedDate < dateTime4 || dateTime4 == DateTime.MinValue))
          dateTime4 = cdLog.CoBorrowerPresumedReceivedDate;
      }
      if (isRescindable)
      {
        if (receivedDateNew1 == DateTime.MinValue || dateTime4 == DateTime.MinValue)
          return DateTime.MinValue;
        DateTime dateTime5 = new DateTime();
        DateTime receivedDateNew2 = !(receivedDateNew1 > dateTime4) ? dateTime4 : receivedDateNew1;
        if (cdLog.IsNboExist && cdLog.DisclosedForCD && dateTime1 > receivedDateNew2)
          receivedDateNew2 = dateTime1;
        return receivedDateNew2;
      }
      return dateTime4 == DateTime.MinValue || !(receivedDateNew1 == DateTime.MinValue) && receivedDateNew1 < dateTime4 ? receivedDateNew1 : dateTime4;
    }

    private bool isValidNBOTypeForReceivedDate(INonBorrowerOwnerItem nboItem)
    {
      string empty = string.Empty;
      string str = nboItem.isBorrowerTypeLocked ? nboItem.LockedBorrowerType : nboItem.VestingType;
      if (string.IsNullOrWhiteSpace(str))
        return false;
      string lower = str.ToLower();
      return lower == "title only" || lower == "title only trustee" || lower == "title only settlor trustee" || lower == "non title spouse";
    }

    private bool isValidBorrowerTypeForReceivedDate(IDisclosureTracking2015Log log, bool isBorrower)
    {
      string empty = string.Empty;
      if (this.IsSingleBorrowerOnlyPair())
        return true;
      string str;
      bool flag;
      if (isBorrower)
      {
        str = log.IsBorrowerTypeLocked ? log.LockedBorrowerType : log.BorrowerType;
        flag = !string.IsNullOrEmpty(log.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.eDisclosure ? log.eDisclosureBorrowerName : log.BorrowerName);
      }
      else
      {
        str = log.IsCoBorrowerTypeLocked ? log.LockedCoBorrowerType : log.CoBorrowerType;
        flag = !string.IsNullOrEmpty(log.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.eDisclosure ? log.eDisclosureCoBorrowerName : log.CoBorrowerName);
      }
      return ((string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str) || str == "Individual" || str == "Trustee" ? 1 : (str == "Settlor Trustee" ? 1 : 0)) & (flag ? 1 : 0)) != 0;
    }

    private DateTime getReceivedDate(IDisclosureTracking2015Log updatedLog)
    {
      if (updatedLog.DisclosedForCD)
      {
        DateTime dateTime1 = new DateTime();
        DateTime receivedDate = updatedLog.BorrowerActualReceivedDate;
        if (updatedLog.IsBorrowerPresumedDateLocked)
        {
          if (updatedLog.LockedBorrowerPresumedReceivedDate != DateTime.MinValue && (updatedLog.LockedBorrowerPresumedReceivedDate < receivedDate || receivedDate == DateTime.MinValue))
            receivedDate = updatedLog.LockedBorrowerPresumedReceivedDate;
        }
        else if (updatedLog.BorrowerPresumedReceivedDate != DateTime.MinValue && (updatedLog.BorrowerPresumedReceivedDate < receivedDate || receivedDate == DateTime.MinValue))
          receivedDate = updatedLog.BorrowerPresumedReceivedDate;
        if (updatedLog.CoBorrowerName.Trim() == "")
          return receivedDate;
        DateTime dateTime2 = new DateTime();
        DateTime dateTime3 = updatedLog.CoBorrowerActualReceivedDate;
        if (updatedLog.IsCoBorrowerPresumedDateLocked)
        {
          if (updatedLog.LockedCoBorrowerPresumedReceivedDate != DateTime.MinValue && (updatedLog.LockedCoBorrowerPresumedReceivedDate < dateTime3 || dateTime3 == DateTime.MinValue))
            dateTime3 = updatedLog.LockedCoBorrowerPresumedReceivedDate;
        }
        else if (updatedLog.CoBorrowerPresumedReceivedDate != DateTime.MinValue && (updatedLog.CoBorrowerPresumedReceivedDate < dateTime3 || dateTime3 == DateTime.MinValue))
          dateTime3 = updatedLog.CoBorrowerPresumedReceivedDate;
        return receivedDate > dateTime3 ? receivedDate : dateTime3;
      }
      DateTime receivedDate1 = new DateTime();
      if (this.isValidBorrowerTypeForReceivedDate(updatedLog, true))
      {
        DateTime actualReceivedDate = updatedLog.BorrowerActualReceivedDate;
        receivedDate1 = !updatedLog.IsBorrowerPresumedDateLocked ? (!(updatedLog.BorrowerActualReceivedDate == DateTime.MinValue) || !(updatedLog.BorrowerPresumedReceivedDate != DateTime.MinValue) ? (!(updatedLog.BorrowerPresumedReceivedDate == DateTime.MinValue) || !(updatedLog.BorrowerActualReceivedDate != DateTime.MinValue) ? (updatedLog.BorrowerPresumedReceivedDate > updatedLog.BorrowerActualReceivedDate ? updatedLog.BorrowerActualReceivedDate : updatedLog.BorrowerPresumedReceivedDate) : updatedLog.BorrowerActualReceivedDate) : updatedLog.BorrowerPresumedReceivedDate) : (!(updatedLog.BorrowerActualReceivedDate == DateTime.MinValue) || !(updatedLog.LockedBorrowerPresumedReceivedDate != DateTime.MinValue) ? (!(updatedLog.LockedBorrowerPresumedReceivedDate == DateTime.MinValue) || !(updatedLog.BorrowerActualReceivedDate != DateTime.MinValue) ? (updatedLog.LockedBorrowerPresumedReceivedDate > updatedLog.BorrowerActualReceivedDate ? updatedLog.BorrowerActualReceivedDate : updatedLog.LockedBorrowerPresumedReceivedDate) : updatedLog.BorrowerActualReceivedDate) : updatedLog.LockedBorrowerPresumedReceivedDate);
      }
      if (updatedLog.CoBorrowerName.Trim() == "")
        return receivedDate1;
      DateTime dateTime = new DateTime();
      if (this.isValidBorrowerTypeForReceivedDate(updatedLog, false))
      {
        DateTime actualReceivedDate = updatedLog.CoBorrowerActualReceivedDate;
        dateTime = !updatedLog.IsCoBorrowerPresumedDateLocked ? (!(updatedLog.CoBorrowerActualReceivedDate == DateTime.MinValue) || !(updatedLog.CoBorrowerPresumedReceivedDate != DateTime.MinValue) ? (!(updatedLog.CoBorrowerPresumedReceivedDate == DateTime.MinValue) || !(updatedLog.CoBorrowerActualReceivedDate != DateTime.MinValue) ? (updatedLog.CoBorrowerPresumedReceivedDate > updatedLog.CoBorrowerActualReceivedDate ? updatedLog.CoBorrowerActualReceivedDate : updatedLog.CoBorrowerPresumedReceivedDate) : updatedLog.CoBorrowerActualReceivedDate) : updatedLog.CoBorrowerPresumedReceivedDate) : (!(updatedLog.CoBorrowerActualReceivedDate == DateTime.MinValue) || !(updatedLog.LockedCoBorrowerPresumedReceivedDate != DateTime.MinValue) ? (!(updatedLog.LockedCoBorrowerPresumedReceivedDate == DateTime.MinValue) || !(updatedLog.CoBorrowerActualReceivedDate != DateTime.MinValue) ? (updatedLog.LockedCoBorrowerPresumedReceivedDate > updatedLog.CoBorrowerActualReceivedDate ? updatedLog.CoBorrowerActualReceivedDate : updatedLog.LockedCoBorrowerPresumedReceivedDate) : updatedLog.CoBorrowerActualReceivedDate) : updatedLog.LockedCoBorrowerPresumedReceivedDate);
      }
      return dateTime == DateTime.MinValue || !(receivedDate1 == DateTime.MinValue) && receivedDate1 < dateTime ? receivedDate1 : dateTime;
    }

    public Dictionary<string, int> borrowerPairIDsDistribution(
      DisclosureTracking2015Log.DisclosureTrackingType type)
    {
      IDisclosureTracking2015Log[] idisclosureTracking2015Log = this.loanData.GetLogList().GetAllIDisclosureTracking2015Log(true);
      Dictionary<string, int> dictionary = new Dictionary<string, int>();
      if (type == DisclosureTracking2015Log.DisclosureTrackingType.LE)
      {
        foreach (IDisclosureTracking2015Log disclosureTracking2015Log in idisclosureTracking2015Log)
        {
          if (disclosureTracking2015Log.DisclosedForLE)
          {
            if (dictionary.ContainsKey(disclosureTracking2015Log.BorrowerPairID))
              dictionary[disclosureTracking2015Log.BorrowerPairID]++;
            else
              dictionary.Add(disclosureTracking2015Log.BorrowerPairID, 1);
          }
        }
      }
      if (type == DisclosureTracking2015Log.DisclosureTrackingType.CD)
      {
        foreach (IDisclosureTracking2015Log disclosureTracking2015Log in idisclosureTracking2015Log)
        {
          if (disclosureTracking2015Log.DisclosedForCD)
          {
            if (dictionary.ContainsKey(disclosureTracking2015Log.BorrowerPairID))
              dictionary[disclosureTracking2015Log.BorrowerPairID]++;
            else
              dictionary.Add(disclosureTracking2015Log.BorrowerPairID, 1);
          }
        }
      }
      return dictionary;
    }

    public void UpdateDisclosureTypeForTimeline(IDisclosureTracking2015Log updatedLog)
    {
      IDisclosureTracking2015Log[] tracking2015LogsByType1 = this.loanData.GetLogList().GetInitialIDisclosureTracking2015LogsByType(DisclosureTracking2015Log.DisclosureTrackingType.LE);
      IDisclosureTracking2015Log[] tracking2015LogsByType2 = this.loanData.GetLogList().GetInitialIDisclosureTracking2015LogsByType(DisclosureTracking2015Log.DisclosureTrackingType.CD);
      foreach (IDisclosureTracking2015Log disclosureTracking2015Log1 in this.loanData.GetLogList().GetAllIDisclosureTracking2015Log(true))
      {
        if (updatedLog.DisclosedForLE && disclosureTracking2015Log1.DisclosedForLE)
        {
          bool flag = false;
          foreach (IDisclosureTracking2015Log disclosureTracking2015Log2 in tracking2015LogsByType1)
          {
            if (disclosureTracking2015Log1 == disclosureTracking2015Log2)
              flag = true;
          }
          if (flag || tracking2015LogsByType1.Length == 0)
            disclosureTracking2015Log1.DisclosureType = DisclosureTracking2015Log.DisclosureTypeEnum.Initial;
          else if (disclosureTracking2015Log1.DisclosureType != DisclosureTracking2015Log.DisclosureTypeEnum.Final)
            disclosureTracking2015Log1.DisclosureType = DisclosureTracking2015Log.DisclosureTypeEnum.Revised;
        }
        if (updatedLog.DisclosedForCD && disclosureTracking2015Log1.DisclosedForCD)
        {
          bool flag = false;
          if (disclosureTracking2015Log1.DisclosureType != DisclosureTracking2015Log.DisclosureTypeEnum.PostConsummation)
          {
            foreach (IDisclosureTracking2015Log disclosureTracking2015Log3 in tracking2015LogsByType2)
            {
              if (disclosureTracking2015Log1 == disclosureTracking2015Log3)
                flag = true;
            }
            if (flag || tracking2015LogsByType2.Length == 0)
              disclosureTracking2015Log1.DisclosureType = DisclosureTracking2015Log.DisclosureTypeEnum.Initial;
            else if (disclosureTracking2015Log1.DisclosureType != DisclosureTracking2015Log.DisclosureTypeEnum.Final)
              disclosureTracking2015Log1.DisclosureType = DisclosureTracking2015Log.DisclosureTypeEnum.Revised;
          }
        }
      }
    }

    private bool IsInitialLogsExistForEachBorrowerPair(
      IDisclosureTracking2015Log[] dtlogs,
      BorrowerPair[] brPairs,
      bool isRescindable)
    {
      Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
      if (dtlogs.Length == 0 || brPairs.Length == 0)
        return false;
      foreach (BorrowerPair brPair in brPairs)
        dictionary.Add(brPair.Id, false);
      foreach (IDisclosureTrackingLog dtlog in dtlogs)
      {
        string borrowerPairId = dtlog.BorrowerPairID;
        if (dictionary.ContainsKey(borrowerPairId))
        {
          dictionary[borrowerPairId] = true;
          if (!isRescindable)
            return true;
        }
      }
      return !dictionary.ContainsValue(false);
    }

    private bool IsSingleBorrowerOnlyPair()
    {
      BorrowerPair[] borrowerPairs = this.loanData.GetBorrowerPairs();
      return ((IEnumerable<BorrowerPair>) borrowerPairs).Count<BorrowerPair>() <= 1 && string.IsNullOrEmpty(borrowerPairs[0].CoBorrower.FirstName);
    }

    public DateTime GetReceivedDateFromAllLogs(
      IDisclosureTracking2015Log[] cdLogs,
      bool isRescindable,
      ref string receivedDateLogGuid)
    {
      DateTime minValue = DateTime.MinValue;
      DateTime receivedDateFromAllLogs = DateTime.MinValue;
      foreach (IDisclosureTracking2015Log cdLog in cdLogs)
      {
        DateTime receivedDateNew = this.getReceivedDateNew(cdLog, isRescindable);
        if (!(receivedDateNew == DateTime.MinValue))
        {
          if (receivedDateFromAllLogs == DateTime.MinValue)
          {
            receivedDateFromAllLogs = receivedDateNew;
            receivedDateLogGuid = cdLog.Guid;
          }
          else if (isRescindable)
          {
            if (receivedDateNew > receivedDateFromAllLogs)
            {
              receivedDateFromAllLogs = receivedDateNew;
              receivedDateLogGuid = cdLog.Guid;
            }
          }
          else if (receivedDateNew < receivedDateFromAllLogs)
          {
            receivedDateFromAllLogs = receivedDateNew;
            receivedDateLogGuid = cdLog.Guid;
          }
        }
      }
      return receivedDateFromAllLogs;
    }

    private IDisclosureTracking2015Log GetEarliestSentDateFromAllLogs(
      IDisclosureTracking2015Log[] Logs)
    {
      IDisclosureTracking2015Log log1 = (IDisclosureTracking2015Log) null;
      foreach (IDisclosureTracking2015Log log2 in Logs)
      {
        if (this.isValidBorrowerTypeForReceivedDate(log2, true) || this.isValidBorrowerTypeForReceivedDate(log2, false))
        {
          IDisclosureTracking2015Log log3 = log2;
          if (this.GetSentDateforLog(log1) == DateTime.MinValue)
            log1 = log3;
          if (this.GetSentDateforLog(log3) < this.GetSentDateforLog(log1))
            log1 = log3;
        }
      }
      return log1;
    }

    private DateTime GetSentDateforLog(IDisclosureTracking2015Log log)
    {
      return log != null ? log.DisclosedDate : DateTime.MinValue;
    }

    public DateTime GetInitialCDReceivedDate()
    {
      DateTime dateTime = new DateTime();
      bool isRescindable = this.loanData.IsRescindable();
      IDisclosureTracking2015Log[] tracking2015LogsByType = this.loanData.GetLogList().GetInitialIDisclosureTracking2015LogsByType(DisclosureTracking2015Log.DisclosureTrackingType.CD);
      BorrowerPair[] borrowerPairs = this.loanData.GetBorrowerPairs();
      if (!this.IsInitialLogsExistForEachBorrowerPair(tracking2015LogsByType, borrowerPairs, isRescindable))
        return DateTime.MinValue;
      string receivedDateLogGuid = "";
      DateTime receivedDateFromAllLogs = this.GetReceivedDateFromAllLogs(tracking2015LogsByType, isRescindable, ref receivedDateLogGuid);
      this.loanData.SetField("FV.X394", receivedDateLogGuid);
      return receivedDateFromAllLogs;
    }

    private DateTime GetInitialLEReceivedDate()
    {
      DateTime dateTime = new DateTime();
      IDisclosureTracking2015Log[] tracking2015LogsByType = this.loanData.GetLogList().GetInitialIDisclosureTracking2015LogsByType(DisclosureTracking2015Log.DisclosureTrackingType.LE);
      string receivedDateLogGuid = "";
      DateTime receivedDateFromAllLogs = this.GetReceivedDateFromAllLogs(tracking2015LogsByType, false, ref receivedDateLogGuid);
      this.loanData.SetField("FV.X392", receivedDateLogGuid);
      return receivedDateFromAllLogs;
    }

    private IDisclosureTracking2015Log GetInitialLESentDate()
    {
      return this.GetEarliestSentDateFromAllLogs(this.loanData.GetLogList().GetInitialIDisclosureTracking2015LogsByType(DisclosureTracking2015Log.DisclosureTrackingType.LE));
    }

    private IDisclosureTracking2015Log GetRevisedLESentDate()
    {
      return this.GetEarliestSentDateFromAllLogs(this.GetRevisedLEDTLogs(DisclosureTracking2015Log.DisclosureTrackingType.LE));
    }

    public void CalculateLatestDisclosure2015(IDisclosureTracking2015Log updatedLog)
    {
      this.loanData.SetCurrentFieldFromCal("3121", "");
      this.loanData.SetCurrentFieldFromCal("3246", "");
      this.loanData.SetCurrentFieldFromCal("3887", "");
      this.loanData.SetCurrentFieldFromCal("4017", "");
      this.loanData.SetCurrentFieldFromCal("4018", "");
      IDisclosureTracking2015Log idisclosureTracking2015Log1 = this.loanData.GetLogList().GetLatestIDisclosureTracking2015Log(DisclosureTracking2015Log.DisclosureTrackingType.LE);
      if (idisclosureTracking2015Log1 == null)
      {
        this.loanData.SetCurrentField("LE1.X33", "//");
        this.loanData.SetCurrentField("LE1.X34", "");
        this.loanData.SetCurrentField("LE1.X35", "");
        this.loanData.SetCurrentField("LE1.X36", "//");
        this.loanData.SetCurrentField("LE1.X37", "");
        this.loanData.SetCurrentField("LE1.X38", "");
        this.loanData.SetCurrentField("LE1.X39", "//");
        this.loanData.SetCurrentField("LE1.X40", "");
      }
      else
      {
        this.loanData.SetCurrentField("LE1.X33", idisclosureTracking2015Log1.DisclosedDate != DateTime.MinValue ? idisclosureTracking2015Log1.DisclosedDate.ToString("MM/dd/yyyy") : "//");
        this.loanData.SetCurrentField("LE1.X34", idisclosureTracking2015Log1.IsDisclosedByLocked ? idisclosureTracking2015Log1.LockedDisclosedByField : idisclosureTracking2015Log1.DisclosedByFullName + "(" + idisclosureTracking2015Log1.DisclosedBy + ")");
        if (idisclosureTracking2015Log1.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.Other)
          this.loanData.SetCurrentField("LE1.X35", idisclosureTracking2015Log1.DisclosedMethodOther);
        else
          this.loanData.SetCurrentField("LE1.X35", idisclosureTracking2015Log1.DisclosedMethodName);
        this.loanData.SetCurrentField("LE1.X40", idisclosureTracking2015Log1.ChangeInCircumstanceComments);
        this.loanData.SetCurrentFieldFromCal("3121", idisclosureTracking2015Log1.DisclosedAPR);
        this.loanData.SetCurrentFieldFromCal("3246", idisclosureTracking2015Log1.FinanceCharge);
        this.loanData.SetCurrentFieldFromCal("3887", idisclosureTracking2015Log1.DisclosedDailyInterest);
        this.loanData.SetCurrentFieldFromCal("4017", idisclosureTracking2015Log1.GetDisclosedField("LE1.X5"));
        this.loanData.SetCurrentFieldFromCal("4018", idisclosureTracking2015Log1.GetDisclosedField("675"));
      }
      IDisclosureTracking2015Log idisclosureTracking2015Log2 = this.loanData.GetLogList().GetLatestIDisclosureTracking2015Log(DisclosureTracking2015Log.DisclosureTrackingType.CD);
      if (idisclosureTracking2015Log2 == null)
      {
        this.loanData.SetCurrentField("CD1.X47", "//");
        this.loanData.SetCurrentField("CD1.X48", "");
        this.loanData.SetCurrentField("CD1.X49", "");
        this.loanData.SetCurrentField("CD1.X50", "");
        this.loanData.SetCurrentField("CD1.X51", "//");
      }
      else
      {
        this.loanData.SetCurrentField("CD1.X47", idisclosureTracking2015Log2.DisclosedDate != DateTime.MinValue ? idisclosureTracking2015Log2.DisclosedDate.ToString("MM/dd/yyyy") : "//");
        this.loanData.SetCurrentField("CD1.X48", idisclosureTracking2015Log2.IsDisclosedByLocked ? idisclosureTracking2015Log2.LockedDisclosedByField : idisclosureTracking2015Log2.DisclosedByFullName + "(" + idisclosureTracking2015Log2.DisclosedBy + ")");
        if (idisclosureTracking2015Log2.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.Other)
          this.loanData.SetCurrentField("CD1.X49", idisclosureTracking2015Log2.DisclosedMethodOther);
        else
          this.loanData.SetCurrentField("CD1.X49", idisclosureTracking2015Log2.DisclosedMethodName);
        this.loanData.SetCurrentField("CD1.X50", idisclosureTracking2015Log2.ChangeInCircumstanceComments);
        this.loanData.SetCurrentFieldFromCal("3121", idisclosureTracking2015Log2.DisclosedAPR);
        this.loanData.SetCurrentFieldFromCal("3246", idisclosureTracking2015Log2.FinanceCharge);
        this.loanData.SetCurrentFieldFromCal("3887", idisclosureTracking2015Log2.DisclosedDailyInterest);
        this.loanData.SetCurrentFieldFromCal("4017", idisclosureTracking2015Log2.GetDisclosedField("LE1.X5"));
        this.loanData.SetCurrentFieldFromCal("4018", idisclosureTracking2015Log2.GetDisclosedField("675"));
      }
      try
      {
        BusinessCalendar businessCalendar = this.sessionObjects.GetBusinessCalendar(CalendarType.RegZ);
        this.loanData.GetLogList().GetIDisclosureTracking2015LogByType(DisclosureTracking2015Log.DisclosureTrackingType.LE, DisclosureTracking2015Log.DisclosureTypeEnum.Revised, true);
        IDisclosureTracking2015Log tracking2015LogByType1 = this.loanData.GetLogList().GetIDisclosureTracking2015LogByType(DisclosureTracking2015Log.DisclosureTrackingType.CD, DisclosureTracking2015Log.DisclosureTypeEnum.Revised, true);
        IDisclosureTracking2015Log tracking2015LogByType2 = this.loanData.GetLogList().GetIDisclosureTracking2015LogByType(DisclosureTracking2015Log.DisclosureTrackingType.CD, DisclosureTracking2015Log.DisclosureTypeEnum.PostConsummation, true);
        IDisclosureTracking2015Log disclosureTracking2015Log1 = this.loanData.GetLogList().GetEarliestSSPLIDisclosureTracking2015Log();
        IDisclosureTracking2015Log idisclosureTracking2015Log3 = this.loanData.GetLogList().GetEarliestSafeHarborIDisclosureTracking2015Log();
        this.loanData.SetField("FV.X385", "");
        this.loanData.SetField("FV.X391", "");
        this.loanData.SetField("FV.X392", "");
        this.loanData.SetField("FV.X393", "");
        this.loanData.SetField("FV.X394", "");
        this.loanData.SetField("FV.X395", "");
        this.loanData.SetField("FV.X398", "");
        this.loanData.SetField("FV.X399", "");
        this.loanData.SetField("FV.X400", "");
        IDisclosureTracking2015Log initialLeSentDate = this.GetInitialLESentDate();
        if (initialLeSentDate != null)
        {
          if (updatedLog != null)
          {
            IDisclosureTracking2015Log tracking2015LogByType3 = this.loanData.GetLogList().GetIDisclosureTracking2015LogByType(DisclosureTracking2015Log.DisclosureTrackingType.LE, DisclosureTracking2015Log.DisclosureTypeEnum.Initial, true, updatedLog.BorrowerPairID);
            if (tracking2015LogByType3 != null && updatedLog.DisclosedForLE)
            {
              if (updatedLog.DisclosureType == DisclosureTracking2015Log.DisclosureTypeEnum.Revised && updatedLog.Guid == tracking2015LogByType3.Guid)
              {
                updatedLog.AdjustDisclosedType(DisclosureTracking2015Log.DisclosureTypeEnum.Initial);
                foreach (IDisclosureTracking2015Log disclosureTracking2015Log2 in this.loanData.GetLogList().GetIDisclosureTracking2015LogsByBorrowerPairId(true, DisclosureTracking2015Log.DisclosureTrackingType.LE, updatedLog.BorrowerPairID))
                {
                  if (!(updatedLog.Guid == disclosureTracking2015Log2.Guid) && disclosureTracking2015Log2.DisclosureType == DisclosureTracking2015Log.DisclosureTypeEnum.Initial)
                    disclosureTracking2015Log2.AdjustDisclosedType(DisclosureTracking2015Log.DisclosureTypeEnum.Revised);
                }
              }
              if (updatedLog.Guid != tracking2015LogByType3.Guid && updatedLog.IsDisclosed)
              {
                if (tracking2015LogByType3.DisclosureType == DisclosureTracking2015Log.DisclosureTypeEnum.Revised)
                  tracking2015LogByType3.AdjustDisclosedType(DisclosureTracking2015Log.DisclosureTypeEnum.Initial);
                if (updatedLog.DisclosureType == DisclosureTracking2015Log.DisclosureTypeEnum.Initial)
                  updatedLog.AdjustDisclosedType(DisclosureTracking2015Log.DisclosureTypeEnum.Revised);
              }
            }
          }
          DateTime dateTime1 = new DateTime();
          DateTime sentDateforLog = this.GetSentDateforLog(initialLeSentDate);
          this.loanData.SetField("FV.X385", initialLeSentDate.Guid);
          if (sentDateforLog != DateTime.MinValue)
            this.loanData.SetField("3152", sentDateforLog.ToString("MM/dd/yyyy"));
          else
            this.loanData.SetField("3152", "");
          DateTime dateTime2 = new DateTime();
          DateTime initialLeReceivedDate = this.GetInitialLEReceivedDate();
          if (initialLeReceivedDate != DateTime.MinValue)
            this.loanData.SetCurrentField("3153", initialLeReceivedDate.ToString("MM/dd/yyyy"));
          else
            this.loanData.SetCurrentField("3153", "");
          this.loanData.SetCurrentField("LE1.X39", this.loanData.GetField("3153"));
        }
        else
        {
          this.loanData.SetField("3152", "");
          this.loanData.SetCurrentField("3153", "");
        }
        this.updateBorrowerIntentToProceed();
        IDisclosureTracking2015Log revisedLeSentDate = this.GetRevisedLESentDate();
        if (revisedLeSentDate != null)
        {
          DateTime dateTime3 = new DateTime();
          DateTime sentDateforLog = this.GetSentDateforLog(revisedLeSentDate);
          this.loanData.SetField("FV.X391", revisedLeSentDate.Guid);
          if (sentDateforLog != DateTime.MinValue)
            this.loanData.SetField("3154", sentDateforLog.ToString("MM/dd/yyyy"));
          else
            this.loanData.SetField("3154", "");
          DateTime dateTime4 = new DateTime();
          DateTime revisedLeReceivedDate = this.GetRevisedLEReceivedDate();
          if (revisedLeReceivedDate != DateTime.MinValue)
            this.loanData.SetCurrentField("3155", revisedLeReceivedDate.ToString("MM/dd/yyyy"));
          else
            this.loanData.SetCurrentField("3155", "");
          this.loanData.SetCurrentField("LE1.X39", this.loanData.GetField("3155"));
        }
        else
        {
          this.loanData.SetField("3154", "");
          this.loanData.SetCurrentField("3155", "");
        }
        this.calObjs.NewHud2015Cal.CalcLERedisclosureFlag((string) null, (string) null);
        IDisclosureTracking2015Log tracking2015LogByType4 = this.loanData.GetLogList().GetIDisclosureTracking2015LogByType(DisclosureTracking2015Log.DisclosureTrackingType.CD, DisclosureTracking2015Log.DisclosureTypeEnum.Initial, true);
        if (tracking2015LogByType4 != null)
        {
          if (updatedLog != null)
          {
            IDisclosureTracking2015Log tracking2015LogByType5 = this.loanData.GetLogList().GetIDisclosureTracking2015LogByType(DisclosureTracking2015Log.DisclosureTrackingType.CD, DisclosureTracking2015Log.DisclosureTypeEnum.Initial, true, updatedLog.BorrowerPairID);
            if (tracking2015LogByType5 != null && updatedLog.DisclosedForCD)
            {
              if (updatedLog.DisclosureType == DisclosureTracking2015Log.DisclosureTypeEnum.Revised && updatedLog.Guid == tracking2015LogByType5.Guid)
              {
                updatedLog.AdjustDisclosedType(DisclosureTracking2015Log.DisclosureTypeEnum.Initial);
                foreach (IDisclosureTracking2015Log disclosureTracking2015Log3 in this.loanData.GetLogList().GetIDisclosureTracking2015LogsByBorrowerPairId(true, DisclosureTracking2015Log.DisclosureTrackingType.CD, updatedLog.BorrowerPairID))
                {
                  if (!(updatedLog.Guid == disclosureTracking2015Log3.Guid) && disclosureTracking2015Log3.DisclosureType == DisclosureTracking2015Log.DisclosureTypeEnum.Initial)
                    disclosureTracking2015Log3.AdjustDisclosedType(DisclosureTracking2015Log.DisclosureTypeEnum.Revised);
                }
              }
              if (updatedLog.Guid != tracking2015LogByType5.Guid && updatedLog.IsDisclosed)
              {
                if (tracking2015LogByType5.DisclosureType == DisclosureTracking2015Log.DisclosureTypeEnum.Revised)
                  tracking2015LogByType5.AdjustDisclosedType(DisclosureTracking2015Log.DisclosureTypeEnum.Initial);
                if (updatedLog.DisclosureType == DisclosureTracking2015Log.DisclosureTypeEnum.Initial)
                  updatedLog.AdjustDisclosedType(DisclosureTracking2015Log.DisclosureTypeEnum.Revised);
              }
            }
          }
          this.loanData.SetField("FV.X398", tracking2015LogByType4.Guid);
          DateTime sentDateforLog = this.GetSentDateforLog(tracking2015LogByType4);
          if (sentDateforLog != DateTime.MinValue)
            this.loanData.SetField("3977", sentDateforLog.ToString("MM/dd/yyyy"));
          else
            this.loanData.SetField("3977", "");
          DateTime dateTime = new DateTime();
          DateTime initialCdReceivedDate = this.GetInitialCDReceivedDate();
          if (initialCdReceivedDate != DateTime.MinValue)
            this.loanData.SetCurrentField("3978", initialCdReceivedDate.ToString("MM/dd/yyyy"));
          else
            this.loanData.SetCurrentField("3978", "");
          this.loanData.SetCurrentField("CD1.X51", this.loanData.GetField("3978"));
          this.loanData.SetCurrentFieldFromCal("4017", tracking2015LogByType4.GetDisclosedField("LE1.X5"));
          this.loanData.SetCurrentFieldFromCal("4018", tracking2015LogByType4.GetDisclosedField("675"));
        }
        else
        {
          this.loanData.SetField("3977", "");
          this.loanData.SetCurrentField("3978", "");
        }
        if (tracking2015LogByType1 != null)
        {
          this.loanData.SetField("FV.X399", tracking2015LogByType1.Guid);
          if (tracking2015LogByType1.DisclosedDate != DateTime.MinValue)
            this.loanData.SetField("3979", tracking2015LogByType1.DisclosedDate.ToString("MM/dd/yyyy"));
          else
            this.loanData.SetField("3979", "");
          if (tracking2015LogByType1.DisclosedDate == DateTime.MinValue)
          {
            this.loanData.SetField("3980", "");
          }
          else
          {
            DateTime dateTime = new DateTime();
            DateTime revisedCdReceivedDate = this.GetRevisedCDReceivedDate(false);
            if (revisedCdReceivedDate != DateTime.MinValue)
              this.loanData.SetCurrentField("3980", revisedCdReceivedDate.ToString("MM/dd/yyyy"));
            else
              this.loanData.SetCurrentField("3980", "");
          }
          this.loanData.SetCurrentField("CD1.X51", this.loanData.GetField("3980"));
        }
        else
        {
          this.loanData.SetField("3979", "");
          this.loanData.SetCurrentField("3980", "");
        }
        if (idisclosureTracking2015Log2 != null)
        {
          this.loanData.SetCurrentFieldFromCal("4017", idisclosureTracking2015Log2.GetDisclosedField("LE1.X5"));
          this.loanData.SetCurrentFieldFromCal("4018", idisclosureTracking2015Log2.GetDisclosedField("675"));
        }
        if (tracking2015LogByType2 != null)
        {
          this.loanData.SetField("FV.X400", tracking2015LogByType2.Guid);
          if (tracking2015LogByType2.DisclosedDate != DateTime.MinValue)
            this.loanData.SetField("3981", tracking2015LogByType2.DisclosedDate.ToString("MM/dd/yyyy"));
          if (this.getReceivedDate(tracking2015LogByType2) != DateTime.MinValue)
            this.loanData.SetCurrentField("3982", this.getReceivedDate(tracking2015LogByType2).ToString("MM/dd/yyyy"));
          else if (tracking2015LogByType2.DisclosureMethod != DisclosureTrackingBase.DisclosedMethod.InPerson && tracking2015LogByType2.DisclosureMethod != DisclosureTrackingBase.DisclosedMethod.eDisclosure && tracking2015LogByType2.DisclosedDate != DateTime.MinValue && !tracking2015LogByType2.IsBorrowerPresumedDateLocked)
            this.loanData.SetCurrentField("3982", businessCalendar.AddBusinessDays(tracking2015LogByType2.DisclosedDate, 3, true).ToString());
          else
            this.loanData.SetCurrentField("3982", "");
        }
        else
        {
          this.loanData.SetField("3981", "");
          this.loanData.SetCurrentField("3982", "");
        }
        this.calObjs.NewHud2015Cal.CalcCDRedisclosureFlag((string) null, (string) null);
        this.loanData.SetField("4014", "");
        this.loanData.SetField("4015", "");
        if (disclosureTracking2015Log1 != null && disclosureTracking2015Log1.DisclosedDate != DateTime.MinValue)
          this.loanData.SetField("4014", disclosureTracking2015Log1.DisclosedDate.ToString("MM/dd/yyyy"));
        if (idisclosureTracking2015Log3 != null && idisclosureTracking2015Log3.DisclosedDate != DateTime.MinValue)
          this.loanData.SetField("4015", idisclosureTracking2015Log3.DisclosedDate.ToString("MM/dd/yyyy"));
        DateTime earliestClosingDate2015 = this.calculateEarliestClosingDate2015(initialLeSentDate, tracking2015LogByType4, revisedLeSentDate, tracking2015LogByType1);
        if (earliestClosingDate2015 != DateTime.MinValue)
          this.loanData.SetCurrentField("3147", earliestClosingDate2015.ToString());
        else
          this.loanData.SetCurrentField("3147", "");
        this.calObjs.NewHudCal.CalculateEarliestFeeCollectionDate();
      }
      catch (ComplianceCalendarException ex)
      {
        throw ex;
      }
      catch (Exception ex)
      {
        if (ex.InnerException is ComplianceCalendarException)
          throw ex.InnerException;
        throw ex;
      }
      finally
      {
        this.inDisclosureCal = false;
      }
    }

    public IDisclosureTracking2015Log[] GetRevisedDTLogs(
      DisclosureTracking2015Log.DisclosureTrackingType leORcd,
      bool forECD)
    {
      BorrowerPair[] borrowerPairs = this.loanData.GetBorrowerPairs();
      this.loanData.IsRescindable();
      IDisclosureTracking2015Log[] idisclosureTracking2015Log = this.loanData.GetLogList().GetAllIDisclosureTracking2015Log(true, DisclosureTracking2015Log.DisclosureTrackingType.CD);
      List<IDisclosureTracking2015Log> list = ((IEnumerable<IDisclosureTracking2015Log>) this.loanData.GetLogList().GetInitialIDisclosureTracking2015LogsByType(DisclosureTracking2015Log.DisclosureTrackingType.CD)).ToList<IDisclosureTracking2015Log>();
      IDisclosureTracking2015Log[] array = ((IEnumerable<IDisclosureTracking2015Log>) idisclosureTracking2015Log).OrderBy<IDisclosureTracking2015Log, DateTime>((Func<IDisclosureTracking2015Log, DateTime>) (x => x.DisclosedDateTime)).ThenBy<IDisclosureTracking2015Log, DateTime>((Func<IDisclosureTracking2015Log, DateTime>) (x => x.Date)).ToArray<IDisclosureTracking2015Log>();
      List<IDisclosureTracking2015Log> disclosureTracking2015LogList = new List<IDisclosureTracking2015Log>();
      Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
      if (array.Length == 0 || borrowerPairs.Length == 0)
        return disclosureTracking2015LogList.ToArray();
      foreach (BorrowerPair borrowerPair in borrowerPairs)
        dictionary.Add(borrowerPair.Id, false);
      DateTime dateTime = new DateTime();
      string str1 = "";
      string str2 = "";
      bool flag = false;
      for (int index = array.Length - 1; index >= 0; --index)
      {
        if ((!forECD || array[index].DisclosureType == DisclosureTracking2015Log.DisclosureTypeEnum.Revised && array[index].DisclosureType != DisclosureTracking2015Log.DisclosureTypeEnum.PostConsummation && array[index].DisclosureMethod != DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder && array[index].DisclosureMethod != DisclosureTrackingBase.DisclosedMethod.eClose) && (!forECD || array[index].CDReasonIsChangeInAPR || array[index].CDReasonIsChangeInLoanProduct || array[index].CDReasonIsPrepaymentPenaltyAdded) && array[index].DisclosureType != DisclosureTracking2015Log.DisclosureTypeEnum.PostConsummation && (leORcd != DisclosureTracking2015Log.DisclosureTrackingType.CD || array[index].DisclosureMethod != DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder && array[index].DisclosureMethod != DisclosureTrackingBase.DisclosedMethod.eClose) && !list.Contains(array[index]))
        {
          if (!flag)
          {
            dateTime = array[index].DisclosedDate;
            str1 = array[index].GetDisclosedField("CD1.X69");
            str2 = array[index].GetDisclosedField("CD1.X1");
            flag = true;
          }
          if (dictionary.ContainsKey(array[index].BorrowerPairID) && !dictionary[array[index].BorrowerPairID] && array[index].GetDisclosedField("CD1.X69") == str1 && array[index].GetDisclosedField("CD1.X1") == str2)
          {
            disclosureTracking2015LogList.Add(array[index]);
            dictionary[array[index].BorrowerPairID] = true;
          }
          if (!dictionary.ContainsValue(false) && array[index].DisclosedDate <= dateTime)
            break;
        }
      }
      return disclosureTracking2015LogList.ToArray();
    }

    public IDisclosureTracking2015Log[] GetRevisedLEDTLogs(
      DisclosureTracking2015Log.DisclosureTrackingType leORcd)
    {
      BorrowerPair[] borrowerPairs = this.loanData.GetBorrowerPairs();
      IDisclosureTracking2015Log[] idisclosureTracking2015Log = this.loanData.GetLogList().GetAllIDisclosureTracking2015Log(true, DisclosureTracking2015Log.DisclosureTrackingType.LE);
      List<IDisclosureTracking2015Log> list = ((IEnumerable<IDisclosureTracking2015Log>) this.loanData.GetLogList().GetInitialIDisclosureTracking2015LogsByType(DisclosureTracking2015Log.DisclosureTrackingType.LE)).ToList<IDisclosureTracking2015Log>();
      IDisclosureTracking2015Log[] array = ((IEnumerable<IDisclosureTracking2015Log>) idisclosureTracking2015Log).OrderBy<IDisclosureTracking2015Log, DateTime>((Func<IDisclosureTracking2015Log, DateTime>) (x => x.DisclosedDateTime)).ThenBy<IDisclosureTracking2015Log, DateTime>((Func<IDisclosureTracking2015Log, DateTime>) (x => x.Date)).ToArray<IDisclosureTracking2015Log>();
      List<IDisclosureTracking2015Log> disclosureTracking2015LogList = new List<IDisclosureTracking2015Log>();
      Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
      if (array.Length == 0 || borrowerPairs.Length == 0)
        return disclosureTracking2015LogList.ToArray();
      foreach (BorrowerPair borrowerPair in borrowerPairs)
        dictionary.Add(borrowerPair.Id, false);
      DateTime dateTime = new DateTime();
      string str1 = "";
      string str2 = "";
      bool flag = false;
      for (int index = array.Length - 1; index >= 0; --index)
      {
        if (array[index].DisclosureType != DisclosureTracking2015Log.DisclosureTypeEnum.PostConsummation && !list.Contains(array[index]))
        {
          if (!flag)
          {
            dateTime = array[index].DisclosedDate;
            str1 = array[index].GetDisclosedField("LE1.X87");
            str2 = array[index].GetDisclosedField("LE1.X1");
            flag = true;
          }
          if (dictionary.ContainsKey(array[index].BorrowerPairID) && !dictionary[array[index].BorrowerPairID] && array[index].GetDisclosedField("LE1.X87") == str1 && array[index].GetDisclosedField("LE1.X1") == str2)
          {
            disclosureTracking2015LogList.Add(array[index]);
            dictionary[array[index].BorrowerPairID] = true;
          }
          if (!dictionary.ContainsValue(false) && array[index].DisclosedDate < dateTime)
            break;
        }
      }
      return disclosureTracking2015LogList.ToArray();
    }

    public DateTime GetRevisedCDReceivedDate(bool forECD)
    {
      DateTime dateTime = new DateTime();
      bool isRescindable = this.loanData.IsRescindable();
      BorrowerPair[] borrowerPairs = this.loanData.GetBorrowerPairs();
      this.loanData.GetLogList().GetInitialIDisclosureTracking2015LogsByType(DisclosureTracking2015Log.DisclosureTrackingType.CD);
      IDisclosureTracking2015Log[] revisedDtLogs = this.GetRevisedDTLogs(DisclosureTracking2015Log.DisclosureTrackingType.CD, forECD);
      if (!this.IsRevisedLogsInGroup(revisedDtLogs, borrowerPairs.Length) & isRescindable)
        return DateTime.MinValue;
      string receivedDateLogGuid = "";
      DateTime receivedDateFromAllLogs = this.GetReceivedDateFromAllLogs(revisedDtLogs, isRescindable, ref receivedDateLogGuid);
      this.loanData.SetField("FV.X395", receivedDateLogGuid);
      return receivedDateFromAllLogs;
    }

    private DateTime GetRevisedLEReceivedDate()
    {
      DateTime dateTime = new DateTime();
      bool isRescindable = false;
      BorrowerPair[] borrowerPairs = this.loanData.GetBorrowerPairs();
      this.loanData.GetLogList().GetInitialIDisclosureTracking2015LogsByType(DisclosureTracking2015Log.DisclosureTrackingType.LE);
      IDisclosureTracking2015Log[] revisedLedtLogs = this.GetRevisedLEDTLogs(DisclosureTracking2015Log.DisclosureTrackingType.LE);
      if (!this.IsRevisedLogsInGroup(revisedLedtLogs, borrowerPairs.Length) & isRescindable)
        return DateTime.MinValue;
      string receivedDateLogGuid = "";
      DateTime receivedDateFromAllLogs = this.GetReceivedDateFromAllLogs(revisedLedtLogs, isRescindable, ref receivedDateLogGuid);
      this.loanData.SetField("FV.X393", receivedDateLogGuid);
      return receivedDateFromAllLogs;
    }

    public bool IsRevisedLogsInGroup(IDisclosureTracking2015Log[] cdRevisedLogs, int bpCount)
    {
      return cdRevisedLogs.Length >= bpCount;
    }

    public DateTime GetInitialBorrowerReceivedDate()
    {
      return !Utils.CheckIf2015RespaTila(this.loanData.GetField("3969")) ? this.calObjs.NewHudCal.GetInitialBorrowerReceivedDate() : this.calObjs.NewHud2015Cal.GetInitialBorrowerReceivedDate();
    }

    private void updateBorrowerIntentToProceed()
    {
      DateTime dateTime1 = new DateTime();
      DateTime dateTime2 = Utils.CheckIf2015RespaTila(this.loanData.GetField("3969")) ? this.calObjs.NewHud2015Cal.GetInitialBorrowerReceivedDate() : this.calObjs.NewHudCal.GetInitialBorrowerReceivedDate();
      if (dateTime2 == DateTime.MinValue)
      {
        this.loanData.SetCurrentField("3164", "N");
        if (!Utils.CheckIf2015RespaTila(this.loanData.GetField("3969")))
          this.calObjs.NewHudCal.CalculateBorrowerIntentToProceedDate();
        else
          this.calObjs.NewHud2015Cal.CalculateBorrowerIntentToProceedDate();
      }
      else
      {
        if (!(this.loanData.GetField("3164") == "Y") || !Utils.IsDate((object) this.loanData.GetField("3197")))
          return;
        DateTime date = Utils.ParseDate((object) this.loanData.GetField("3197"));
        if (!(dateTime2 > date))
          return;
        this.loanData.SetCurrentField("3164", "N");
        if (!Utils.CheckIf2015RespaTila(this.loanData.GetField("3969")))
          this.calObjs.NewHudCal.CalculateBorrowerIntentToProceedDate();
        else
          this.calObjs.NewHud2015Cal.CalculateBorrowerIntentToProceedDate();
      }
    }

    private void calculateDisclosureComplicanceTimeline()
    {
      string field1 = this.loanData.GetField("3148");
      string field2 = this.loanData.GetField("3152");
      if (field1 == "" || field2 == "")
        this.loanData.SetField("3144", "");
      else if (!Utils.IsDate((object) field1) || !Utils.IsDate((object) field2))
        this.loanData.SetField("3144", "");
      else if (DateTime.Parse(field1) > DateTime.Parse(field2))
        this.loanData.SetField("3144", field1);
      else
        this.loanData.SetField("3144", field2);
      this.calObjs.NewHudCal.CalculateEarliestFeeCollectionDate();
      this.calculateDisclosureEstimatedClosingDate();
    }

    private void calculateDisclosureEstimatedClosingDate()
    {
      bool complianceSetting = (bool) this.loanData.Settings.ComplianceSettings[(object) "Compliance.CountDay0ForClosingDate"];
      if (!this.disclosureEstimateClosingDatePreValidate())
      {
        this.loanData.SetField("3147", "");
      }
      else
      {
        if (Utils.IsDate((object) this.loanData.GetField("3154")) && !this.recalcClosingDateWithTILRedisclosure())
          return;
        DateTime minValue = DateTime.MinValue;
        DateTime dateTime1;
        try
        {
          dateTime1 = this.sessionObjects.ConfigurationManager.AddBusinessDays(CalendarType.Postal, DateTime.Parse(this.loanData.GetField("3152")), complianceSetting ? 6 : 7, false);
        }
        catch (ComplianceCalendarException ex)
        {
          throw new ComplianceCalendarException(ex, "3152");
        }
        if (Utils.IsDate((object) this.loanData.GetField("3155")))
        {
          try
          {
            DateTime dateTime2 = this.sessionObjects.ConfigurationManager.AddBusinessDays(CalendarType.Postal, DateTime.Parse(this.loanData.GetField("3155")), complianceSetting ? 2 : 3, false);
            if (dateTime2 > dateTime1)
              dateTime1 = dateTime2;
          }
          catch (ComplianceCalendarException ex)
          {
            throw new ComplianceCalendarException(ex, "3155");
          }
        }
        if (dateTime1 > DateTime.MinValue)
          this.loanData.SetField("3147", dateTime1.ToString("MM/dd/yyyy"));
        else
          this.loanData.SetField("3147", "");
      }
    }

    private void checkIfCDValidForEarliestClosingDate()
    {
      BorrowerPair[] borrowerPairs = this.loanData.GetBorrowerPairs();
      Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
      foreach (BorrowerPair borrowerPair in borrowerPairs)
        dictionary.Add(borrowerPair.Id, false);
      foreach (IDisclosureTracking2015Log updatedLog in this.loanData.GetLogList().GetAllIDisclosureTracking2015Log(true, DisclosureTracking2015Log.DisclosureTrackingType.CD))
      {
        if (dictionary.ContainsKey(updatedLog.BorrowerPairID) && this.getReceivedDate(updatedLog) < DateTime.Now)
          dictionary[updatedLog.BorrowerPairID] = true;
      }
      foreach (KeyValuePair<string, bool> keyValuePair in dictionary)
      {
        if (!keyValuePair.Value)
        {
          this.loanData.SetField("4082", "N");
          return;
        }
      }
      this.loanData.SetField("4082", "Y");
    }

    private DateTime calculateEarliestClosingDate2015(
      IDisclosureTracking2015Log leInitLog,
      IDisclosureTracking2015Log cdInitLog,
      IDisclosureTracking2015Log leRevisedLog,
      IDisclosureTracking2015Log cdRevisedLog)
    {
      DateTime dateTime1 = new DateTime();
      string val = "";
      bool flag1 = false;
      bool flag2 = false;
      DateTime dateTime2 = new DateTime();
      DateTime result1 = DateTime.MinValue;
      DateTime result2 = DateTime.MinValue;
      DateTime result3 = DateTime.MinValue;
      DateTime minValue = DateTime.MinValue;
      DateTime.TryParse(this.loanData.GetField("3153"), out result1);
      DateTime.TryParse(this.loanData.GetField("3978"), out result2);
      DateTime.TryParse(this.loanData.GetField("3155"), out result3);
      BusinessCalendar businessCalendar = this.sessionObjects.GetBusinessCalendar(CalendarType.RegZ);
      bool complianceSetting1 = (bool) this.loanData.Settings.ComplianceSettings[(object) "Compliance.CountDay0ForRESPASunHoliday"];
      bool complianceSetting2 = (bool) this.loanData.Settings.ComplianceSettings[(object) "Compliance.CountDay0ForRESPAFedHoliday"];
      this.loanData.SetField("FV.X387", "");
      this.loanData.SetField("FV.X389", "");
      DateTime dateTime3 = new DateTime();
      if (leInitLog != null)
        dateTime3 = businessCalendar.AddBusinessDays(leInitLog.DisclosedDate, 7, true, complianceSetting1, complianceSetting2, true);
      if (leRevisedLog != null)
      {
        if (result3 != DateTime.MinValue && result3 <= DateTime.Now)
          dateTime1 = businessCalendar.AddBusinessDays(result3, 4, true, complianceSetting1, complianceSetting2, true);
        if (dateTime3 <= dateTime1)
          dateTime3 = dateTime1;
      }
      this.checkIfCDValidForEarliestClosingDate();
      BorrowerPair[] borrowerPairs = this.loanData.GetBorrowerPairs();
      bool isRescindable = this.loanData.IsRescindable();
      IDisclosureTracking2015Log[] revisedDtLogs = this.GetRevisedDTLogs(DisclosureTracking2015Log.DisclosureTrackingType.CD, true);
      bool flag3 = !(!this.IsRevisedLogsInGroup(revisedDtLogs, borrowerPairs.Length) & isRescindable);
      bool flag4 = false;
      DateTime dateTime4 = DateTime.MinValue;
      for (int index = revisedDtLogs.Length - 1; index >= 0; --index)
      {
        cdRevisedLog = revisedDtLogs[index];
        if (cdRevisedLog != null)
        {
          DateTime receivedDateNew = this.getReceivedDateNew(cdRevisedLog, isRescindable);
          if (isRescindable)
          {
            if (receivedDateNew != DateTime.MinValue)
            {
              if (receivedDateNew > DateTime.Now || !flag3)
              {
                if (cdRevisedLog.DisclosedDate != DateTime.MinValue)
                {
                  DateTime dateTime5 = businessCalendar.AddBusinessDays(cdRevisedLog.DisclosedDate, 6, true, complianceSetting1, complianceSetting2, true);
                  if (dateTime5 > dateTime1)
                  {
                    dateTime1 = dateTime5;
                    val = cdRevisedLog.Guid;
                    flag1 = false;
                  }
                  flag2 = true;
                }
              }
              else
              {
                DateTime dateTime6 = businessCalendar.AddBusinessDays(receivedDateNew, 3, true, complianceSetting1, complianceSetting2, true);
                if (dateTime6 > dateTime1)
                {
                  dateTime1 = dateTime6;
                  val = cdRevisedLog.Guid;
                  flag1 = true;
                }
                flag2 = true;
              }
            }
            else if (cdRevisedLog.DisclosedDate != DateTime.MinValue)
            {
              DateTime dateTime7 = businessCalendar.AddBusinessDays(cdRevisedLog.DisclosedDate, 6, true, complianceSetting1, complianceSetting2, true);
              if (dateTime7 > dateTime1)
              {
                dateTime1 = dateTime7;
                val = cdRevisedLog.Guid;
                flag1 = false;
              }
              flag2 = true;
            }
          }
          else if (receivedDateNew != DateTime.MinValue)
          {
            if (receivedDateNew > DateTime.Now || !flag3)
            {
              if (!flag4)
              {
                dateTime4 = businessCalendar.AddBusinessDays(cdRevisedLog.DisclosedDate, 6, true, complianceSetting1, complianceSetting2, true);
                val = cdRevisedLog.Guid;
                flag1 = false;
                flag4 = true;
              }
              if (cdRevisedLog.DisclosedDate != DateTime.MinValue)
              {
                DateTime dateTime8 = businessCalendar.AddBusinessDays(cdRevisedLog.DisclosedDate, 6, true, complianceSetting1, complianceSetting2, true);
                if (dateTime8 < dateTime4)
                {
                  dateTime4 = dateTime8;
                  val = cdRevisedLog.Guid;
                  flag1 = false;
                }
                flag2 = true;
              }
            }
            else
            {
              if (!flag4)
              {
                dateTime4 = businessCalendar.AddBusinessDays(receivedDateNew, 3, true, complianceSetting1, complianceSetting2, true);
                val = cdRevisedLog.Guid;
                flag1 = true;
                flag4 = true;
              }
              DateTime dateTime9 = businessCalendar.AddBusinessDays(receivedDateNew, 3, true, complianceSetting1, complianceSetting2, true);
              if (dateTime9 < dateTime4)
              {
                dateTime4 = dateTime9;
                val = cdRevisedLog.Guid;
                flag1 = true;
              }
              flag2 = true;
            }
          }
          else if (cdRevisedLog.DisclosedDate != DateTime.MinValue)
          {
            if (!flag4)
            {
              dateTime4 = businessCalendar.AddBusinessDays(cdRevisedLog.DisclosedDate, 6, true, complianceSetting1, complianceSetting2, true);
              val = cdRevisedLog.Guid;
              flag1 = false;
              flag4 = true;
            }
            DateTime dateTime10 = businessCalendar.AddBusinessDays(cdRevisedLog.DisclosedDate, 6, true, complianceSetting1, complianceSetting2, true);
            if (dateTime10 < dateTime4)
            {
              dateTime4 = dateTime10;
              val = cdRevisedLog.Guid;
              flag1 = false;
            }
            flag2 = true;
          }
        }
      }
      if (!isRescindable && dateTime4 > dateTime1)
        dateTime1 = dateTime4;
      if (flag2)
      {
        if (flag1)
          this.loanData.SetField("FV.X389", val);
        else
          this.loanData.SetField("FV.X387", val);
        return dateTime3 <= dateTime1 ? dateTime1 : dateTime3;
      }
      if (cdInitLog != null)
      {
        if (result2 != DateTime.MinValue)
        {
          if (result2 > DateTime.Now || this.loanData.GetField("4082") != "Y" & isRescindable)
          {
            if (cdInitLog.DisclosedDate != DateTime.MinValue)
              dateTime1 = businessCalendar.AddBusinessDays(cdInitLog.DisclosedDate, 6, true, complianceSetting1, complianceSetting2, true);
          }
          else
            dateTime1 = businessCalendar.AddBusinessDays(result2, 3, true, complianceSetting1, complianceSetting2, true);
        }
        else if (cdInitLog.DisclosedDate != DateTime.MinValue)
          dateTime1 = businessCalendar.AddBusinessDays(cdInitLog.DisclosedDate, 6, true, complianceSetting1, complianceSetting2, true);
        return dateTime3 <= dateTime1 ? dateTime1 : dateTime3;
      }
      if (leRevisedLog != null)
      {
        if (result3 != DateTime.MinValue)
        {
          if (result3 > DateTime.Now)
          {
            if (leRevisedLog.DisclosedDate != DateTime.MinValue)
              dateTime1 = businessCalendar.AddBusinessDays(leRevisedLog.DisclosedDate, 7, true, complianceSetting1, complianceSetting2, true);
          }
          else
            dateTime1 = businessCalendar.AddBusinessDays(result3, 4, true, complianceSetting1, complianceSetting2, true);
        }
        else if (leRevisedLog.DisclosedDate != DateTime.MinValue)
          dateTime1 = businessCalendar.AddBusinessDays(leRevisedLog.DisclosedDate, 7, true, complianceSetting1, complianceSetting2, true);
        return dateTime3 <= dateTime1 ? dateTime1 : dateTime3;
      }
      if (leInitLog != null)
      {
        if (leInitLog.DisclosedDate != DateTime.MinValue)
          dateTime1 = businessCalendar.AddBusinessDays(leInitLog.DisclosedDate, 7, true, complianceSetting1, complianceSetting2, true);
        return dateTime3 <= dateTime1 ? dateTime1 : dateTime3;
      }
      return !(dateTime3 > dateTime1) ? dateTime1 : dateTime3;
    }

    private bool recalcClosingDateWithTILRedisclosure()
    {
      bool flag = false;
      DisclosureTrackingLog[] disclosureTrackingLog1 = this.loanData.GetLogList().GetAllDisclosureTrackingLog(true, DisclosureTrackingLog.DisclosureTrackingType.TIL);
      if (disclosureTrackingLog1.Length > 1)
      {
        DisclosureTrackingLog disclosureTrackingLog2 = disclosureTrackingLog1[disclosureTrackingLog1.Length - 1];
        DisclosureTrackingLog disclosureTrackingLog3 = disclosureTrackingLog1[disclosureTrackingLog1.Length - 2];
        if (RegulationAlerts.GetRediscloseTILAlertRateChange(this.loanData, Utils.ParseDouble((object) disclosureTrackingLog2.DisclosedAPR), Utils.ParseDouble((object) disclosureTrackingLog3.DisclosedAPR)) != null)
          flag = true;
      }
      return flag;
    }

    private bool disclosureEstimateClosingDatePreValidate()
    {
      return Utils.IsDate((object) this.loanData.GetField("3152")) && (!Utils.IsDate((object) this.loanData.GetField("3154")) || !this.recalcClosingDateWithTILRedisclosure() || Utils.IsDate((object) this.loanData.GetField("3155")));
    }

    public bool SkipFieldChangeEvent
    {
      get => this.calObjs.SkipFieldChangeEvent;
      set => this.calObjs.SkipFieldChangeEvent = value;
    }

    public bool SkipLockRequestSync
    {
      get => this.calObjs.SkipLockRequestSync;
      set => this.calObjs.SkipLockRequestSync = value;
    }

    public bool SkipLinkedSync
    {
      get => this.calObjs.SkipLinkedSync;
      set => this.calObjs.SkipLinkedSync = value;
    }

    public RegzSummaryTableType RegzSummaryType => this.calObjs.RegzCal.RegzSummaryType;

    public List<string> ULDDExportChecking(string source)
    {
      if (source.ToLower() == "fannie")
        return this.calObjs.ULDDExpCal.FannieInvalidFields();
      return source.ToLower() == "freddie" ? this.calObjs.ULDDExpCal.FreddieInvalidFields() : this.calObjs.ULDDExpCal.GinnieInvalidFields();
    }

    public void ResetULDDBorrowerInformation()
    {
    }

    public bool TriggerTradeOffCalculation => this.calObjs.NewHudCal.TriggerTradeOffCalculation;

    public static double CalcRawMonthlyPayment(
      int unpaidTerm,
      double unpaidBalance,
      double currentRate,
      int loanTerm,
      int paidTerm,
      double loanAmount,
      string zeroPercentPaymentOption,
      bool useSimpleInterest,
      DateTime firstPaymentDate,
      string constInterestType,
      bool simpleInterestUse366ForLeapYear)
    {
      return LoanCalculator.CalcRawMonthlyPayment(unpaidTerm, unpaidBalance, currentRate, false, false, 0.0, 0.0, 0.0, 12, loanTerm, paidTerm, loanAmount, zeroPercentPaymentOption, useSimpleInterest, firstPaymentDate, constInterestType, simpleInterestUse366ForLeapYear);
    }

    public static double CalcRawMonthlyPayment(
      int unpaidTerm,
      double unpaidBalance,
      double currentRate,
      bool isBiweekly,
      double rateFactorBiWeekly,
      int numberofPayPerYear,
      int loanTerm,
      int paidTerm,
      double loanAmount,
      string zeroPercentPaymentOption,
      bool useSimpleInterest,
      DateTime firstPaymentDate,
      string constInterestType,
      bool simpleInterestUse366ForLeapYear)
    {
      return LoanCalculator.CalcRawMonthlyPayment(unpaidTerm, unpaidBalance, currentRate, false, isBiweekly, rateFactorBiWeekly, 0.0, 0.0, numberofPayPerYear, loanTerm, paidTerm, loanAmount, zeroPercentPaymentOption, useSimpleInterest, firstPaymentDate, constInterestType, simpleInterestUse366ForLeapYear);
    }

    public static double CalcRawMonthlyPayment(
      int unpaidTerm,
      double unpaidBalance,
      double currentRate,
      bool isGPM,
      bool isBiweekly,
      double rateFactorBiWeekly,
      double rateForGPM,
      double yearsForGPM,
      int numberofPayPerYear,
      int loanTerm,
      int paidTerm,
      double loanAmount,
      string zeroPercentPaymentOption,
      bool useSimpleInterest,
      DateTime firstPaymentDate,
      string constInterestType,
      bool simpleInterestUse366ForLeapYear)
    {
      return RegzCalculation.CalcRawMonthlyPayment(unpaidTerm, unpaidBalance, currentRate, isGPM, isBiweekly, rateFactorBiWeekly, rateForGPM, yearsForGPM, numberofPayPerYear, loanTerm, paidTerm, loanAmount, zeroPercentPaymentOption, useSimpleInterest, firstPaymentDate, constInterestType, simpleInterestUse366ForLeapYear);
    }

    public void CalcDefaultTpoBankContact() => this.calObjs.ToolCal.CalcDefaultTpoBankContact();

    public void GetIntentToProceedDT2015Log(Dictionary<DisclosureTracking2015Log, bool> log)
    {
      this.calObjs.NewHud2015Cal.GetIntentToProceedDT2015Log(log);
    }

    public void GetIntentToProceedIDisclosureTracking2015Log(
      Dictionary<IDisclosureTracking2015Log, bool> log)
    {
      this.calObjs.NewHud2015Cal.GetIntentToProceedIDisclosureTracking2015Log(log);
    }

    public void CalculateLastDisclosedCDorLE(IDisclosureTracking2015Log log)
    {
      if (log.DisclosedForCD)
        this.calObjs.NewHud2015FeeDetailCal.calculate_LastDisclosedCD("", "");
      else if (log.DisclosedForLE)
      {
        this.calObjs.NewHud2015FeeDetailCal.calculate_LastDisclosedLE("", "");
        this.calObjs.NewHud2015Cal.CalculateCDPage3StdCashToCloseLoanEstimate("", "");
        this.calObjs.NewHud2015Cal.CalculateCDPage3AlternateCashToCloseLoanEstimate("", "");
      }
      if ((log.DisclosedForCD || log.DisclosedForLE) && this.loanData.GetField("958") == "IRRRL")
        this.calObjs.VACal.CalcVARecoupment("", "");
      if (log.DisclosedForSafeHarbor)
        this.calObjs.NewHud2015FeeDetailCal.calculate_EarliestSafeHarbor("", "");
      if (!log.ProviderListSent && !log.ProviderListNoFeeSent)
        return;
      this.calObjs.NewHud2015FeeDetailCal.calculate_EarliestSSPL("", "");
    }

    public void UpdateLogs()
    {
      this.calObjs.FeeVarianceToolCal.UpdateLogs();
      this.calObjs.FeeVarianceToolCal.CalculateFeeVariance();
    }

    public void CalculateFeeVariance() => this.calObjs.FeeVarianceToolCal.CalculateFeeVariance();

    public void ResetFeeVariance()
    {
    }

    public string SetTotalLenderCredit(IDisclosureTracking2015Log log)
    {
      return this.calObjs.FeeVarianceToolCal.SetTotalLenderCredit(log);
    }

    public string SetCannotIncrease(IDisclosureTracking2015Log log)
    {
      return this.calObjs.FeeVarianceToolCal.SetCannotIncrease(log);
    }

    public bool IsLEBaseLineUsed(string section)
    {
      switch (section)
      {
        case "cannotdecrease":
          return this.calObjs.FeeVarianceToolCal.LEBaselineUsed_CannotDecrease;
        case "cannotincrease":
          return this.calObjs.FeeVarianceToolCal.LEBaselineUsed_CannotIncrease;
        case "cannotincrease10":
          return this.calObjs.FeeVarianceToolCal.LEBaselineUsed_CannotIncrease10;
        default:
          return false;
      }
    }

    public void CalculateFeeVarianceTotals(string id)
    {
      this.calObjs.FeeVarianceToolCal.CalculateFeeVarianceTotals(id);
    }

    public Hashtable GetGFFVarianceAlertDetails()
    {
      return this.calObjs.FeeVarianceToolCal.GetGFFVarianceAlertDetails();
    }

    public void LoadeSignConsentDate() => this.calObjs.Cal.LoadeSignConsentDate();

    public double GetRequiredVarianceCureAmount()
    {
      double varianceCureAmount = 0.0;
      if (!Utils.CheckIf2015RespaTila(this.loanData.GetField("3969")))
        return varianceCureAmount;
      if (this.loanData.GetField("FV.X339") != "" && Utils.ParseDouble((object) this.loanData.GetField("FV.X339")) != 0.0 || this.loanData.GetField("FV.X327") != "" && Utils.ParseDouble((object) this.loanData.GetField("FV.X327")) != 0.0)
      {
        if (Utils.ParseDouble((object) this.loanData.GetField("FV.X347")) > 0.0)
          varianceCureAmount = Utils.ParseDouble((object) this.loanData.GetField("FV.X347"));
      }
      else if (this.loanData.GetField("FV.X345") != "" && Utils.ParseDouble((object) this.loanData.GetField("FV.X345")) != 0.0)
        varianceCureAmount = Utils.ParseDouble((object) this.loanData.GetField("FV.X345"));
      return varianceCureAmount;
    }

    public string RateLockExpirationTimeZoneSetting
    {
      get => this.calObjs.NewHud2015Cal.RateLockExpirationTimeZoneSetting;
    }

    public string ClosingCostExpirationTimeZoneSetting
    {
      get => this.calObjs.NewHud2015Cal.ClosingCostExpirationTimeZoneSetting;
    }

    public int ClosingCostDaysToExpire => this.calObjs.NewHud2015Cal.ClosingCostDaysToExpire;

    public DateTime GetISWCutoffDate() => this.calObjs.ToolCal.GetISWCutoffDate();

    public void CalcGuiDependentLogicForDDM() => this.CalcGuiDependentLogicForDDM("");

    public void CalcGuiDependentLogicForDDM(string updatedFieldIDsByDDM)
    {
      this.calObjs.DDMCal.UpdatedFieldIDsByDDM = updatedFieldIDsByDDM;
      this.calObjs.DDMCal.CalcGuiDependentLogicForDDM((string) null, (string) null);
    }

    public bool AddFeePaidToNameToCache(string fieldId, string originalValue, string newValue)
    {
      bool flag = false;
      string[] strArray;
      if (HUDGFE2010Fields.FeeLineNumberLookupTable.ContainsKey((object) (fieldId + "/PaidTo")))
      {
        strArray = (string[]) HUDGFE2010Fields.FeeLineNumberLookupTable[(object) (fieldId + "/PaidTo")];
        flag = true;
      }
      else
      {
        if (!HUDGFE2010Fields.FeeLineNumberLookupTable.ContainsKey((object) (fieldId + "/PaidToName")))
          return false;
        strArray = (string[]) HUDGFE2010Fields.FeeLineNumberLookupTable[(object) (fieldId + "/PaidToName")];
      }
      string key = strArray[HUDGFE2010Fields.PTCPOCINDEX_LINENUMBER] + strArray[HUDGFE2010Fields.PTCPOCINDEX_COMPONENTID];
      if (this.feeLinePaidToTrigger == null)
        this.feeLinePaidToTrigger = CollectionsUtil.CreateCaseInsensitiveHashtable();
      if (!this.feeLinePaidToTrigger.ContainsKey((object) key))
        this.feeLinePaidToTrigger.Add((object) key, (object) new object[5]);
      object[] objArray = (object[]) this.feeLinePaidToTrigger[(object) key];
      if (objArray[0] == null)
        objArray[0] = (object) strArray;
      if (flag)
      {
        objArray[1] = (object) originalValue;
        objArray[2] = (object) newValue;
      }
      else
      {
        objArray[3] = (object) originalValue;
        objArray[4] = (object) newValue;
      }
      return true;
    }

    public void AddFeePaidToNameToLoan()
    {
      if (this.feeLinePaidToTrigger == null || this.feeLinePaidToTrigger.Count == 0)
        return;
      string fieldID = (string) null;
      object obj1 = (object) "";
      object obj2 = (object) "";
      foreach (DictionaryEntry dictionaryEntry in this.feeLinePaidToTrigger)
      {
        bool flag = false;
        object[] objArray = (object[]) dictionaryEntry.Value;
        if (objArray != null && objArray.Length >= 5)
        {
          string[] pocFields = (string[]) objArray[0];
          if (pocFields != null)
          {
            obj1 = objArray[1];
            object obj3 = objArray[2];
            obj2 = objArray[3];
            object obj4 = objArray[4];
            if (obj3 == null && obj4 != null)
            {
              this.loanData.SetCurrentField(pocFields[HUDGFE2010Fields.PTCPOCINDEX_PAIDTONAME], obj4.ToString());
              flag = false;
            }
            else if (obj3 != null && obj4 == null)
            {
              this.loanData.SetCurrentField(pocFields[HUDGFE2010Fields.PTCPOCINDEX_PAIDTO], obj3.ToString());
              fieldID = pocFields[HUDGFE2010Fields.PTCPOCINDEX_PAIDTO];
              flag = true;
            }
            else if (obj3 != null && obj4 != null)
            {
              this.loanData.SetCurrentField(pocFields[HUDGFE2010Fields.PTCPOCINDEX_PAIDTO], obj3.ToString());
              this.loanData.SetCurrentField(pocFields[HUDGFE2010Fields.PTCPOCINDEX_PAIDTONAME], obj4.ToString());
              flag = false;
            }
            if (flag)
              this.calObjs.NewHud2015FeeDetailCal.CalcFeeDetail(fieldID, pocFields);
          }
        }
      }
    }

    public bool ValidateHmdaCalculation(string id) => this.calObjs.HMDACal.IsHmdaActionValid(id);

    public bool ValidateHmdaActioForDenialReasons()
    {
      return this.calObjs.HMDACal.IsHmdaActionValidForDenialReasons();
    }

    public void MapLenderBorrowerPaidFieldToLR()
    {
      this.loanData.SetCurrentField("4463", this.loanData.GetField("LCP.X1"));
    }

    public bool IsHmdaFieldCalculated(string id)
    {
      switch (id)
      {
        case "HMDA.X39":
        case "HMDA.X40":
        case "HMDA.X42":
        case "HMDA.X44":
        case "HMDA.X50":
          return this.calObjs.HMDACal.IsHmdaActionValid(id);
        case "HMDA.X86":
          return this.calObjs.HMDACal.IsNMLSLoanOriginatorIDCalculated();
        default:
          return true;
      }
    }

    public void ResetTriggerCounter() => this.calObjs.ResetTriggerCounter();

    public string DumpTriggerCounter() => this.calObjs.DumpTriggerCounter();

    public void IncrementTriggerCounter(string funcName, long timeConsumed)
    {
      this.calObjs.IncreaseTriggerCounter(funcName, timeConsumed);
    }

    public void UpdateLenderRepresentative(LoanAssociateLog m, string type)
    {
      LoanAssociateLog loanAssociateLog = (LoanAssociateLog) null;
      switch (m)
      {
        case MilestoneLog _:
          loanAssociateLog = m;
          break;
        case MilestoneFreeRoleLog _:
          loanAssociateLog = m;
          break;
      }
      for (int index = 0; index < 8; ++index)
      {
        string id = "";
        if (index == 0)
          id = "4672";
        else if (index == 1)
          id = "4802";
        else if (index == 2)
          id = "4806";
        else if (index == 3)
          id = "4809";
        else if (index == 4)
          id = "4811";
        else if (index == 5)
          id = "4814";
        else if (index == 6)
          id = "4818";
        else if (index == 7)
          id = "4824";
        if (this.loanData.GetField(id) != "")
          this.calObjs.RegzCal.CalcLenderRepresentative(id, (string) null);
      }
    }

    public Hashtable GetUsers(int[] roleids)
    {
      Hashtable users = new Hashtable();
      try
      {
        List<string> usersWithRoles = this.sessionObjects.OrganizationManager.GetUsersWithRoles(roleids);
        int num = 0;
        if (usersWithRoles != null)
        {
          foreach (string str in usersWithRoles)
            users.Add((object) ++num, (object) str);
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(Tracing.SwDataEngine, nameof (LoanCalculator), TraceLevel.Verbose, "GetUsers function failed due to error: " + ex.Message);
      }
      return users;
    }

    public bool UsePriceBasedQM(DateTime originationDate)
    {
      return this.calObjs.ATRQMCal.UsePriceBasedQM(originationDate);
    }

    public void GetAMILimits(bool showMessage)
    {
      this.calObjs.SystemTableCal.ExecuteTable_AMI(showMessage);
    }

    public object GetAMILimitRecords(bool showMessage)
    {
      return (object) this.calObjs.SystemTableCal.GetAMILimitRecords(showMessage);
    }

    public void GetMFILimits(bool showMessage)
    {
      this.calObjs.SystemTableCal.ExecuteTable_MFI(showMessage);
    }

    public object GetMFILimitRecords(bool showMessage)
    {
      return (object) this.calObjs.SystemTableCal.GetMFILimitRecords(showMessage);
    }

    public DateTime FindClosingDateToUse() => this.calObjs.Cal.findClosingDateToUse();

    public void SetCalculationsToSkip(bool skipTheCalc, string[] functionNamesToSkip)
    {
      this.calObjs.SetCalculationsToSkip(skipTheCalc, functionNamesToSkip);
    }
  }
}
