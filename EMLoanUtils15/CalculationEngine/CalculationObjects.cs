// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.CalculationEngine.CalculationObjects
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.LoanUtils.CalculationServants;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

#nullable disable
namespace EllieMae.EMLite.CalculationEngine
{
  public class CalculationObjects : ICalculationObjects
  {
    private Calculation cal;
    private VerifCalculation verifCal;
    private D1003Calculation d1003Cal;
    private D1003URLA2020Calculation d1003URLA2020Cal;
    private CloserCalculation closerCal;
    private GFECalculation gfeCal;
    private MLDSCalculation mldsCal;
    private RegzCalculation regzCal;
    private FHACalculation fhaCal;
    private VACalculation vaCal;
    private HelocCalculation helocCal;
    private FM1084Calculation fm1084Cal;
    private ComortCalculation comortCal;
    private PreCalculation preCal;
    private PrequalCalculation prequalCal;
    private LoansubCalculation loansubCal;
    private HUD1ESCalculation hud1Cal;
    private NewHUDCalculation newHudCal;
    private NewHUD2015Calculation newHud2015Cal;
    private NewHUD2015FeeDetailCalculation newHud2015FeeDetailCal;
    private FeeVarianceToolCalculation feeVarianceToolCal;
    private ToolCalculation toolCal;
    private PrintingCalculation printCal;
    private CustomCalculations customCal;
    private BillingCalculations billingCal;
    private USDACalculation usdaCal;
    private ATRQMCalculation atrQMCal;
    private AlertCoCLECDCalculation alertCoCLECDCalc;
    private HMDACalculation hmdaCal;
    private SystemTableCalculation systemTableCal;
    private DdmCalculation ddmCal;
    private bool needLoanAmountRounding = true;
    private bool stopSyncItemization;
    private bool manualSyncItemization;
    private bool allowURLA2020;
    private bool enableTriggerToRunEscrowDateCalc;
    private bool useItemizeEscrow;
    private bool skipFieldChangeEvent;
    private bool skipLockRequestSync;
    private bool skipLinkedSync;
    private string currentFormID = string.Empty;
    private bool forceDefaultLinkSync = true;
    private ILoanConfigurationInfo configInfo;
    private ULDDExportCalculation ulddExpCal;
    private SessionObjects sessionObjects;
    private LockRequestCalculator lockRequestCal;
    private ExternalLateFeeSettings externalLateFeeSettings;
    private bool allowCalculationDiagnostic = EnConfigurationSettings.GlobalSettings.AllowCalculationDiagnostic;
    private Dictionary<string, Tuple<int, long>> triggerCounter = new Dictionary<string, Tuple<int, long>>();
    private Hashtable calcSkipTable = CollectionsUtil.CreateCaseInsensitiveHashtable();

    internal Calculation Cal => this.cal;

    internal VerifCalculation VERIFCal => this.verifCal;

    internal D1003Calculation D1003Cal => this.d1003Cal;

    internal D1003URLA2020Calculation D1003URLA2020Cal => this.d1003URLA2020Cal;

    internal CloserCalculation CloserCal => this.closerCal;

    internal GFECalculation GFECal => this.gfeCal;

    internal MLDSCalculation MLDSCal => this.mldsCal;

    internal RegzCalculation RegzCal => this.regzCal;

    internal FHACalculation FHACal => this.fhaCal;

    internal VACalculation VACal => this.vaCal;

    internal HelocCalculation HelocCal => this.helocCal;

    internal FM1084Calculation FM1084Cal => this.fm1084Cal;

    internal ComortCalculation ComortCal => this.comortCal;

    internal PreCalculation PreCal => this.preCal;

    internal PrequalCalculation PrequalCal => this.prequalCal;

    internal LoansubCalculation LoansubCal => this.loansubCal;

    internal HUD1ESCalculation Hud1Cal => this.hud1Cal;

    internal NewHUDCalculation NewHudCal => this.newHudCal;

    internal NewHUD2015Calculation NewHud2015Cal => this.newHud2015Cal;

    internal NewHUD2015FeeDetailCalculation NewHud2015FeeDetailCal => this.newHud2015FeeDetailCal;

    internal FeeVarianceToolCalculation FeeVarianceToolCal => this.feeVarianceToolCal;

    internal ToolCalculation ToolCal => this.toolCal;

    internal PrintingCalculation PrintCal => this.printCal;

    internal CustomCalculations CustomCal => this.customCal;

    internal BillingCalculations BillingCal => this.billingCal;

    internal USDACalculation USDACal => this.usdaCal;

    internal ATRQMCalculation ATRQMCal => this.atrQMCal;

    internal AlertCoCLECDCalculation AlertCoCLECDCalc => this.alertCoCLECDCalc;

    internal HMDACalculation HMDACal => this.hmdaCal;

    internal SystemTableCalculation SystemTableCal => this.systemTableCal;

    internal DdmCalculation DDMCal => this.ddmCal;

    internal bool NeedLoanAmountRounding => this.needLoanAmountRounding;

    public bool StopSyncItemization => this.stopSyncItemization;

    public bool ManualSyncItemization => this.manualSyncItemization;

    internal bool AllowURLA2020 => this.allowURLA2020;

    internal bool EnableTriggerToRunEscrowDateCalc => this.enableTriggerToRunEscrowDateCalc;

    internal bool UseItemizeEscrow => this.useItemizeEscrow;

    internal bool SkipFieldChangeEvent
    {
      get => this.skipFieldChangeEvent;
      set => this.skipFieldChangeEvent = value;
    }

    internal bool SkipLockRequestSync
    {
      get => this.skipLockRequestSync;
      set => this.skipLockRequestSync = value;
    }

    internal bool SkipLinkedSync
    {
      get => this.skipLinkedSync;
      set => this.skipLinkedSync = value;
    }

    internal string CurrentFormID
    {
      get => this.currentFormID;
      set => this.currentFormID = value;
    }

    internal bool ForceDefaultLinkSync
    {
      get => this.forceDefaultLinkSync;
      set => this.forceDefaultLinkSync = value;
    }

    internal ILoanConfigurationInfo LoanConfiguration => this.configInfo;

    internal ULDDExportCalculation ULDDExpCal => this.ulddExpCal;

    internal SessionObjects SessionObjects => this.sessionObjects;

    internal LockRequestCalculator LockRequestCal => this.lockRequestCal;

    internal ExternalLateFeeSettings ExternalLateFeeSettings
    {
      get => this.externalLateFeeSettings;
      set
      {
        this.externalLateFeeSettings = value;
        this.ToolCal.AddFieldHandlerBasedOnExtLateFeeSettings();
        this.ToolCal.UpdateExternalLateFeeSettingFields();
      }
    }

    internal SyncItemizationSetting SyncCheck { get; private set; }

    public string CurrentFormId => this.CurrentFormID;

    public string CurrentFieldId { get; set; }

    public string CurrentFieldValue { get; set; }

    internal void IncreaseTriggerCounter(string functionName, long timeConsumed)
    {
      if (!this.allowCalculationDiagnostic)
        return;
      if (!this.triggerCounter.ContainsKey(functionName))
      {
        this.triggerCounter.Add(functionName, new Tuple<int, long>(1, timeConsumed));
      }
      else
      {
        Tuple<int, long> tuple = this.triggerCounter[functionName];
        this.triggerCounter[functionName] = new Tuple<int, long>(tuple.Item1 + 1, tuple.Item2 + timeConsumed);
      }
    }

    public void ResetTriggerCounter()
    {
      if (!this.allowCalculationDiagnostic)
        return;
      this.triggerCounter = new Dictionary<string, Tuple<int, long>>();
    }

    public string DumpTriggerCounter()
    {
      if (!this.allowCalculationDiagnostic || this.triggerCounter == null)
        return (string) null;
      string str = "";
      int num = 1;
      foreach (KeyValuePair<string, Tuple<int, long>> keyValuePair in this.triggerCounter)
        str = str + (str != "" ? (object) "\r\n" : (object) "") + "    " + (object) num++ + ".\t" + keyValuePair.Key + ": " + (object) keyValuePair.Value.Item1 + " : " + (object) keyValuePair.Value.Item2 + " ms";
      return str;
    }

    internal void SetCalculationsToSkip(bool skipTheCalc, string[] functionNamesToSkip)
    {
      for (int index = 0; index < functionNamesToSkip.Length; ++index)
      {
        if (skipTheCalc && !this.calcSkipTable.ContainsKey((object) functionNamesToSkip[index]))
          this.calcSkipTable.Add((object) functionNamesToSkip[index], (object) "");
        else if (!skipTheCalc && this.calcSkipTable.ContainsKey((object) functionNamesToSkip[index]))
          this.calcSkipTable.Remove((object) functionNamesToSkip[index]);
      }
    }

    internal bool CalculationIsSkipped(string functionName)
    {
      return this.calcSkipTable.ContainsKey((object) functionName);
    }

    internal CalculationObjects(
      SessionObjects sessionObjects,
      ILoanConfigurationInfo configInfo,
      LoanData l,
      bool skipLogCalcs = false)
    {
      this.sessionObjects = sessionObjects;
      this.configInfo = configInfo;
      this.preCal = new PreCalculation(l, this);
      this.prequalCal = new PrequalCalculation(l, this);
      this.fhaCal = new FHACalculation(sessionObjects, l, this);
      this.vaCal = new VACalculation(l, this);
      this.usdaCal = new USDACalculation(l, this);
      this.fm1084Cal = new FM1084Calculation(l, this);
      this.verifCal = new VerifCalculation(sessionObjects, l, this);
      this.d1003Cal = new D1003Calculation(sessionObjects, l, this);
      this.d1003URLA2020Cal = new D1003URLA2020Calculation(l, this);
      this.comortCal = new ComortCalculation(l, this);
      this.hud1Cal = new HUD1ESCalculation(l, this);
      this.gfeCal = new GFECalculation(sessionObjects, l, this);
      this.newHudCal = new NewHUDCalculation(sessionObjects, configInfo, l, this);
      this.newHud2015Cal = new NewHUD2015Calculation(sessionObjects, configInfo, l, this);
      this.newHud2015FeeDetailCal = new NewHUD2015FeeDetailCalculation(sessionObjects, configInfo, l, this);
      this.feeVarianceToolCal = new FeeVarianceToolCalculation(sessionObjects, configInfo, l, this, skipLogCalcs);
      this.mldsCal = new MLDSCalculation(l, this);
      this.regzCal = new RegzCalculation(sessionObjects, configInfo, l, this);
      this.helocCal = new HelocCalculation(sessionObjects, l, this);
      this.loansubCal = new LoansubCalculation(l, this);
      this.toolCal = new ToolCalculation(sessionObjects, configInfo, l, this);
      this.closerCal = new CloserCalculation(l, this);
      this.atrQMCal = new ATRQMCalculation(sessionObjects, configInfo, l, this);
      this.alertCoCLECDCalc = new AlertCoCLECDCalculation(sessionObjects, configInfo, l, this);
      this.printCal = new PrintingCalculation(sessionObjects, l, this);
      this.hmdaCal = new HMDACalculation(l, this);
      this.ulddExpCal = new ULDDExportCalculation(sessionObjects, l, this);
      this.cal = new Calculation(sessionObjects, l, this);
      this.customCal = new CustomCalculations(sessionObjects, configInfo, l, this);
      this.billingCal = new BillingCalculations(sessionObjects, configInfo, l, this);
      this.lockRequestCal = new LockRequestCalculator(sessionObjects, l);
      this.systemTableCal = new SystemTableCalculation(sessionObjects, configInfo, l, this);
      this.ddmCal = new DdmCalculation(sessionObjects, l, this);
      this.allowURLA2020 = sessionObjects.StartupInfo.AllowURLA2020;
      this.enableTriggerToRunEscrowDateCalc = sessionObjects.StartupInfo.EnableTriggerToRunEscrowDateCalc;
      this.needLoanAmountRounding = l.GetField("4745") == "Y";
      this.useItemizeEscrow = (EnableDisableSetting) sessionObjects.StartupInfo.PolicySettings[(object) "Policies.ItemizeEscrow"] == EnableDisableSetting.Enabled;
      this.SyncCheck = (SyncItemizationSetting) sessionObjects.StartupInfo.PolicySettings[(object) "Policies.SyncItemization"];
      this.stopSyncItemization = this.SyncCheck != 0;
      this.manualSyncItemization = this.SyncCheck == SyncItemizationSetting.ManualUpdate;
      this.verifCal.VerifContactID = sessionObjects.GetCompanySettingFromCache("VerifContact", "VerifContactID");
      Hashtable settingsFromCache = sessionObjects.GetCompanySettingsFromCache("NMLS");
      this.gfeCal.ExcludeOriginationCredit = settingsFromCache != null && settingsFromCache.ContainsKey((object) "ExcludeOriginationCredit") && (string) settingsFromCache[(object) "ExcludeOriginationCredit"] == "Y";
      this.d1003Cal.AddInitialApplicationDateTrigger(settingsFromCache == null || !settingsFromCache.ContainsKey((object) "AppDateFieldID") ? "3261" : (string) settingsFromCache[(object) "AppDateFieldID"]);
      this.newHud2015Cal.RateLockExpirationTimeZoneSetting = (string) sessionObjects.StartupInfo.PolicySettings[(object) "Policies.RateLockExpirationTimeZone"];
      this.newHud2015Cal.ClosingCostExpirationTimeZoneSetting = (string) sessionObjects.StartupInfo.PolicySettings[(object) "Policies.CDExpirationTimeZone"];
      this.newHud2015Cal.ClosingCostDaysToExpire = (int) sessionObjects.StartupInfo.PolicySettings[(object) "Policies.LEDaysToExpire"];
      this.toolCal.CreditorOverride = (bool) sessionObjects.StartupInfo.PolicySettings[(object) "Policies.CreditorOverride"];
      if (l.Settings.HMDAInfo != null && l.Settings.HMDAInfo.HMDAApplicationDate != null)
      {
        if (l.Settings.HMDAInfo.HMDAApplicationDate != "745")
          this.hmdaCal.AddInitialApplicationDateTrigger(l.Settings.HMDAInfo.HMDAApplicationDate);
        this.hmdaCal.AddInitialHMDAIncomeTrigger(l.Settings.HMDAInfo.HMDAIncome);
      }
      this.toolCal.CalcHMDAReportingYear((string) null, (string) null);
      this.hmdaCal.SetDefaultHMDACalculation();
      this.d1003Cal.Calc2018DI("HMDA.X27", (string) null);
      this.lockRequestCal.AddInitialLockValidationTrigger();
    }

    void ICalculationObjects.NewHudCalCopyHud2010ToGfe2010(
      string sourceId,
      string currentId,
      bool blankCopyOnly)
    {
      this.NewHudCal.CopyHUD2010ToGFE2010(sourceId, currentId, blankCopyOnly);
    }

    void ICalculationObjects.NewHudCalFormCal(string id, string val)
    {
      this.NewHudCal.FormCal(id, val);
    }

    void ICalculationObjects.GfeCalFormCal(string formid, string id, string val)
    {
      this.GFECal.FormCal(formid, id, val);
    }

    void ICalculationObjects.GfeCalCalcGfeFees(string id, string val)
    {
      this.GFECal.CalcGFEFees(id, val);
    }

    void ICalculationObjects.NewHudCalCalculateHudgfe(string id, string val)
    {
      this.NewHudCal.calculateHUDGFE(id, val);
    }

    public void MldsCalCopyGfetoMlds(string id) => this.MLDSCal.CopyGFEToMLDS(id);

    public void GfeCalCalcPrepaid(string id, string val) => this.GFECal.CalcPrepaid(id, val);

    bool ICalculationObjects.SkipLockRequestSync => this.skipLockRequestSync;

    public void Calc_2015TitleFees(string id, string val)
    {
      this.NewHud2015FeeDetailCal.Calc_2015TitleFees(id, val);
    }

    public void Calc_2015CityStateTaxFees(string id, string val)
    {
      this.NewHud2015FeeDetailCal.Calc_2015CityStateTaxFees(id, val);
    }
  }
}
