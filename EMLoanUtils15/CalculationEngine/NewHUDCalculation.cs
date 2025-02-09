// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.CalculationEngine.NewHUDCalculation
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.LoanUtils.CalculationServants;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.CalculationEngine
{
  internal class NewHUDCalculation : CalculationBase
  {
    private const string className = "NewHUDCalculation�";
    private static readonly string sw = Tracing.SwDataEngine;
    private SessionObjects sessionObjects;
    private ILoanConfigurationInfo configInfo;
    internal Routine CalcHUD1PG2;
    internal Routine CalcHUD1PG3;
    internal Routine CalcREGZGFEHudSummary;
    internal Routine CalcREGZGFEHud;
    internal Routine CalcREGZGFE;
    internal Routine CalcTotalCosts;
    internal Routine CalcClosingCost;
    internal Routine RecalcForm;
    internal Routine CalcGFEApplicationDate;
    internal Routine CalcInitialDisclosureDueDate;
    internal Routine CalcHUD1ToleranceLimits;
    internal Routine EnforceGFEAppDateRule;
    internal Routine CalcHUDGFEDisclosureInfo;
    internal Routine CalcGFEGoodDate;
    internal Routine CalcRevisedGFEDueDate;
    internal Routine CalcRevisedCDDueDate;
    internal Routine CalcLoanOriginatorName;
    internal Routine CalcGFERedisclosureFlag;
    internal Routine Calc3197;
    internal Routine CalTradeOffTable;
    internal Routine CopySpecialHUD2010ToGFE2010;
    internal Routine CalcInterestRateChange;
    internal Routine SetARMIndexDescription;
    private double totalBorrowerPaidCost;
    private double totalOffsetForPrepaid;
    private double totalOffset900And1000POCPaid;
    private double totalOffsetNon900Non1000POCPaid;
    private double totalOffsetPrepaidBorPaidPOC;
    private double totalOffsetNonPrepaidBorPaidPOC;
    private int gfeDaystoExpire;
    private readonly GfeCalculationServant _gfeCalculationServant;
    private readonly NewHudCalculationServant _newHudCalculationServant;
    private LOCompensationSetting loCompensationSetting;
    private bool triggerTradeOffCalculation;
    internal bool IsPOCPTCVerified;
    private bool usingTableFunded;
    private List<string[]> changeCircumstanceOptions;

    static NewHUDCalculation()
    {
      foreach (string[] strArray in HUDGFE2010Fields.HUD2010ToGFE2010FIELDMAP)
      {
        CalculationBase.BORHUDFIELDS.Add((object) strArray[1], (object) strArray);
        if (strArray[2] != string.Empty && !CalculationBase.SELHUDFIELDS.ContainsKey((object) strArray[2]))
          CalculationBase.SELHUDFIELDS.Add((object) strArray[2], (object) strArray);
      }
      CalculationBase.line801PaidByFields = new string[22]
      {
        "SYS.X251",
        "SYS.X261",
        "SYS.X269",
        "SYS.X271",
        "SYS.X265",
        "NEWHUD.X227",
        "SYS.X289",
        "SYS.X291",
        "SYS.X296",
        "SYS.X301",
        "NEWHUD.X748",
        "NEWHUD.X1239",
        "NEWHUD.X1247",
        "NEWHUD.X1255",
        "NEWHUD.X1263",
        "NEWHUD.X1271",
        "NEWHUD.X1279",
        "NEWHUD.X1287",
        "NEWHUD.X1175",
        "NEWHUD.X1179",
        "NEWHUD.X1183",
        "NEWHUD.X1187"
      };
    }

    internal NewHUDCalculation(
      SessionObjects sessionObjects,
      ILoanConfigurationInfo configInfo,
      LoanData l,
      EllieMae.EMLite.CalculationEngine.CalculationObjects calObjs)
      : base(l, calObjs)
    {
      this.sessionObjects = sessionObjects;
      this.configInfo = configInfo;
      this.CalcHUD1PG2 = this.RoutineX(new Routine(this.calculateHUD1Page2));
      this.CalcHUD1PG3 = this.RoutineX(new Routine(this.calculateHUD1Page3));
      this.CalcREGZGFEHudSummary = this.RoutineX(new Routine(this.calculateHUDGFESummary));
      this.CalcREGZGFEHud = this.RoutineX(new Routine(this.calculateHUDGFE));
      this.CalcREGZGFE = this.RoutineX(new Routine(this.calculateREGZGFE));
      this.CalcLoanOriginatorName = this.RoutineX(new Routine(this.calculateLoanOriginatorName));
      this.CalcTotalCosts = this.RoutineX(new Routine(this.calculateTotalCosts));
      this.CalcClosingCost = this.RoutineX(new Routine(this.calculateClosingCost));
      this.RecalcForm = this.RoutineX(new Routine(this.recalculateForm));
      this.CalcGFEApplicationDate = this.RoutineX(new Routine(this.calculateGFEApplicationDate));
      this.CalcInitialDisclosureDueDate = this.RoutineX(new Routine(this.calculateInitialDisclosureDueDate));
      this.CalcHUD1ToleranceLimits = this.RoutineX(new Routine(this.calculateHUD1ToleranceLimits));
      this.EnforceGFEAppDateRule = this.RoutineX(new Routine(this.enforceGFEAppDateRule));
      this.CalcHUDGFEDisclosureInfo = this.RoutineX(new Routine(this.calculateHUDGFEDisclosureInfo));
      this.CalcRevisedGFEDueDate = this.RoutineX(new Routine(this.calculateRevisedGFEDueDate));
      this.CalcRevisedCDDueDate = this.RoutineX(new Routine(this.calculateRevisedCDDueDate));
      this.CalcGFEGoodDate = this.RoutineX(new Routine(this.calculateGoodGFEDate));
      this.CalcGFERedisclosureFlag = this.RoutineX(new Routine(this.calcGFERedisclosureFlag));
      this.CalTradeOffTable = this.RoutineX(new Routine(this.calculateTradeOffTable));
      this.Calc3197 = this.RoutineX(new Routine(this.calculateEarliestFeeCollectionDate));
      this.CopySpecialHUD2010ToGFE2010 = this.RoutineX(new Routine(this.copySpecialHUD2010ToGFE2010));
      this.CalcInterestRateChange = this.RoutineX(new Routine(this.calculateInterestRateChange));
      this.SetARMIndexDescription = this.RoutineX(new Routine(this.setARMIndexDescription));
      this.gfeDaystoExpire = (int) this.sessionObjects.StartupInfo.PolicySettings[(object) "Policies.GFEDaysToExpire"];
      this.addFieldHandlers(l);
      this.UsingTableFunded = this.Val("NEWHUD.X1068") == "Y" && this.Val("VEND.X293") != string.Empty;
      ISettingsProvider systemSettings = (ISettingsProvider) new SystemSettings(sessionObjects);
      this._gfeCalculationServant = new GfeCalculationServant((ILoanModelProvider) this.calObjs.GFECal, systemSettings);
      this._newHudCalculationServant = new NewHudCalculationServant((ILoanModelProvider) this, systemSettings);
    }

    private void addFieldHandlers(LoanData l)
    {
      AlertConfig alertConfig = l.Settings.AlertSetupData.GetAlertConfig(22);
      foreach (AlertTriggerField triggerField in (List<AlertTriggerField>) ((RegulationAlert) alertConfig.Definition).TriggerFields)
        this.AddFieldHandler(triggerField.FieldID, this.CalcGFEApplicationDate);
      if (alertConfig.TriggerFieldList != null)
      {
        foreach (string triggerField in alertConfig.TriggerFieldList)
          this.AddFieldHandler(triggerField, this.CalcGFEApplicationDate);
      }
      this.AddFieldHandler("958", this.CalcGFEApplicationDate + this.EnforceGFEAppDateRule + this.CalcInitialDisclosureDueDate);
      foreach (string[] strArray in HUDGFE2010Fields.HUD1PG3_EXACTFIELDS)
      {
        this.AddFieldHandler(strArray[0], this.CalcHUD1ToleranceLimits);
        this.AddFieldHandler(strArray[1], this.CalcHUD1ToleranceLimits);
      }
      this.AddFieldHandler("NEWHUD.X312", this.CalcHUD1ToleranceLimits);
      this.AddFieldHandler("NEWHUD.X313", this.CalcHUD1ToleranceLimits);
      this.AddFieldHandler("3200", this.CalcGFERedisclosureFlag);
      this.AddFieldHandler("3140", this.CalcHUDGFEDisclosureInfo);
      this.AddFieldHandler("3165", this.CalcRevisedGFEDueDate);
      this.AddFieldHandler("CD1.X62", this.CalcRevisedCDDueDate);
      this.AddFieldHandler("3168", this.CalcHUDGFEDisclosureInfo);
      this.AddFieldHandler("3170", this.CalcGFEGoodDate + this.CalcHUDGFEDisclosureInfo);
      this.AddFieldHandler("NEWHUD.X2", this.CalcGFEGoodDate + this.CalcHUDGFEDisclosureInfo);
      this.AddFieldHandler("DISCLOSURE.X109", this.CalcClosingCost);
      this.AddFieldHandler("DISCLOSURE.X570", this.CalcClosingCost);
      this.AddFieldHandler("DISCLOSURE.X645", this.CalcClosingCost);
      this.AddFieldHandler("DISCLOSURE.X646", this.CalcClosingCost);
      this.AddFieldHandler("CD3.X129", this.CalcClosingCost);
      this.AddFieldHandler("NEWHUD.X14", new Routine(this.calculateHUDGFE));
      this.AddFieldHandler("3197", this.Calc3197);
      this.AddFieldHandler("NEWHUD.X730", new Routine(((CalculationBase) this).FormCal));
      this.AddFieldHandler("NEWHUD.X605", new Routine(((CalculationBase) this).FormCal));
      this.AddFieldHandler("NEWHUD.X606", new Routine(((CalculationBase) this).FormCal));
      this.AddFieldHandler("NEWHUD.X804", new Routine(((CalculationBase) this).FormCal));
      this.AddFieldHandler("NEWHUD.X805", new Routine(((CalculationBase) this).FormCal));
      this.AddFieldHandler("NEWHUD2.X912", new Routine(this.calculateLenderPAC));
      this.AddFieldHandler("2402", new Routine(this.calculateRecordingFee));
      this.AddFieldHandler("2403", new Routine(this.calculateRecordingFee));
      this.AddFieldHandler("2404", new Routine(this.calculateRecordingFee));
      this.AddFieldHandler("587", new Routine(this.calculateRecordingFee));
      this.AddFieldHandler("390", new Routine(this.calculateRecordingFee));
    }

    private void calculateRecordingFee(string id, string val)
    {
      double num1 = this.FltVal("2402");
      double num2 = this.FltVal("2403");
      double num3 = this.FltVal("2404");
      if (num1 == 0.0 && num2 == 0.0 && num3 == 0.0 && this.Val("1636").IndexOf(";Mortgage", StringComparison.Ordinal) < 0)
      {
        if (!(id == "390"))
          return;
        this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem1202("390", this.Val("390"));
      }
      else
      {
        string val1 = "Deed $" + num1.ToString("N2") + ";Mortgage $" + num2.ToString("N2") + ";Releases $" + num3.ToString("N2");
        double num4 = num1 + num2 + num3;
        if (id == "390")
          this.SetCurrentNum("587", num4 - this.FltVal("390"));
        else
          this.SetCurrentNum("390", num4 - this.FltVal("587"));
        this.SetVal("1636", val1);
        this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem1202("390", this.Val("390"));
        if (this.Val("14").ToUpper() == "CA")
          this.loan.Calculator.CopyGFEToMLDS("390");
        this.loan.Calculator.CopyHUD2010ToGFE2010("2402", false);
        this.loan.Calculator.CopyHUD2010ToGFE2010("390", false);
      }
    }

    private void recalculateForm(string id, string val)
    {
      if (!this.UseNewGFEHUD && !this.UseNew2015GFEHUD)
        return;
      this.FormCal(id, val);
    }

    public override void FormCal(string id, string val)
    {
      if (Tracing.IsSwitchActive(NewHUDCalculation.sw, TraceLevel.Info))
        Tracing.Log(NewHUDCalculation.sw, TraceLevel.Info, nameof (NewHUDCalculation), "routine: New Hud FormCal");
      if (!this.UseNewGFEHUD && !this.UseNew2015GFEHUD || (id == "LCP.X6" || id == "LCP.X7" || id == "LCP.X9" || id == "LCP.X10") && this.Val("NEWHUD.X1718") != "Y")
        return;
      if (id == "NEWHUD.X1718" && val == "Y" && this.Val("NEWHUD.X1139") != "Y")
        this.SetVal("NEWHUD.X1139", "Y");
      this.calObjs.NewHud2015Cal.CalcPropertyInsuranceTaxIndicators(id, val);
      if (this.Val("NEWHUD.X1139") != "Y")
        this.CalcLOCompensationTool(id, val);
      this.calObjs.GFECal.CalcGFEFees(id, val);
      this.calObjs.GFECal.CalcPrepaid(id, val);
      this.calObjs.GFECal.CalcSection32(id, val);
      this.VerifyingPOCPTCFields(id, val);
      this.calculateREGZGFE(id, val);
      this.calculateHUDGFE(id, val);
      this.calculateAffiliate(id, val);
      this.calculateHUD1Page2(id, val);
      this.calculateHUD1Page3(id, val);
      this.CalculateFunder(id, val);
      this.calObjs.FHACal.CalcExisting23KDebt((string) null, (string) null);
      this.calObjs.FHACal.CalcMAX23K((string) null, (string) null);
      this.calObjs.RegzCal.CalcAPR((string) null, (string) null);
      this.triggerTradeOffCalculation = true;
      this.CalcGFEApplicationDate((string) null, (string) null);
      if (this.UseNew2015GFEHUD)
      {
        this.calObjs.NewHud2015Cal.CalcDisclosedSalesPrice((string) null, (string) null);
        this.calObjs.NewHud2015Cal.CalcLoanEstimateEstimatedCashToCloseAV(id, val);
        this.calObjs.NewHud2015Cal.CalcLoanEstimate_ClosingCostsFinanced(id, val);
        this.calObjs.NewHud2015Cal.CalcLECashToCloseSellerCredit((string) null, (string) null);
        this.calObjs.NewHud2015Cal.PopulateLoanEstimate_LenderBrokerInfo((string) null, (string) null);
        this.calObjs.NewHud2015Cal.CalcCdPage3Liabilities(id, val);
        this.calObjs.NewHud2015Cal.CalcCDPage3AlternateCashToCloseLoanEstimate(id, val);
        this.calObjs.NewHud2015Cal.CalcClosingDisclosureTotalClosingCosts(id, val);
      }
      else
        this.calObjs.HMDACal.CalcTotalPointsAndFees(id, val);
    }

    internal LOCompensationSetting LOCompensationSetting
    {
      get => this.loCompensationSetting;
      set => this.loCompensationSetting = value;
    }

    private void calculateAffiliate(string id, string val)
    {
      if (this.Val("NEWHUD.X804") != "Affiliate")
        this.SetVal("NEWHUD.X1724", "");
      else if (this.UseNew2015GFEHUD)
      {
        if (this.FltVal("NEWHUD2.X3313") + this.FltVal("NEWHUD2.X3316") < this.FltVal("NEWHUD.X1724") || this.FltVal("NEWHUD.X1724") == 0.0)
          this.SetCurrentNum("NEWHUD.X1724", this.FltVal("NEWHUD2.X3313") + this.FltVal("NEWHUD2.X3316"));
      }
      else if (this.FltVal("NEWHUD.X572") < this.FltVal("NEWHUD.X1724"))
        this.SetCurrentNum("NEWHUD.X1724", this.FltVal("NEWHUD.X572"));
      if (this.Val("NEWHUD.X805") != "Affiliate")
        this.SetVal("NEWHUD.X1725", "");
      else if (this.UseNew2015GFEHUD)
      {
        if (this.FltVal("NEWHUD2.X3346") + this.FltVal("NEWHUD2.X3349") >= this.FltVal("NEWHUD.X1725") && this.FltVal("NEWHUD.X1725") != 0.0)
          return;
        this.SetCurrentNum("NEWHUD.X1725", this.FltVal("NEWHUD2.X3346") + this.FltVal("NEWHUD2.X3349"));
      }
      else
      {
        if (this.FltVal("NEWHUD.X639") >= this.FltVal("NEWHUD.X1725"))
          return;
        this.SetCurrentNum("NEWHUD.X1725", this.FltVal("NEWHUD.X639"));
      }
    }

    private void calculateREGZGFE(string id, string val)
    {
      if (id == "1172" && this.Val("1172") == "VA")
      {
        this.SetVal("NEWHUD.X750", "Y");
        this.SetVal("NEWHUD.X1017", "Y");
      }
      int count = 0;
      this._newHudCalculationServant.CopySection800(ref count);
      for (int index = count; index < HUDGFE2010Fields.HUDGFE_FIELDS_Block3.Length; ++index)
      {
        string[] strArray = (string[]) HUDGFE2010Fields.HUDGFE_FIELDS_Block3[index];
        this.SetVal(strArray[0], "");
        this.SetVal(strArray[1], "");
      }
      count = 0;
      this._newHudCalculationServant.CopySection900(ref count);
      for (int index = count; index < HUDGFE2010Fields.HUDGFE_FIELDS_Block11.Length; ++index)
      {
        string[] strArray = (string[]) HUDGFE2010Fields.HUDGFE_FIELDS_Block11[index];
        this.SetVal(strArray[0], "");
        this.SetVal(strArray[1], "");
        this.SetVal(strArray[2], "");
      }
      count = 0;
      this._newHudCalculationServant.CopySection1100(ref count);
      for (int index = count; index < HUDGFE2010Fields.HUDGFE_FIELDS_Block4.Length; ++index)
      {
        string[] strArray = (string[]) HUDGFE2010Fields.HUDGFE_FIELDS_Block4[index];
        this.SetVal(strArray[0], "");
        this.SetVal(strArray[1], "");
        this.SetVal(strArray[2], "");
      }
      this.SetCurrentNum("NEWHUD.X214", this.FltVal("NEWHUD.X604") + this.FltVal("NEWHUD.X724") + this.FltVal("NEWHUD.X771") + this.FltVal("NEWHUD.X772"));
      double num1 = !this.UseNew2015GFEHUD ? this.FltVal("POPT.X140") + this.FltVal("POPT.X144") + this.FltVal("POPT.X145") + this.FltVal("POPT.X146") + (this.FltVal("PTC.X140") + this.FltVal("PTC.X144") + this.FltVal("PTC.X145") + this.FltVal("PTC.X146")) : this.FltVal("NEWHUD2.X3641") + this.FltVal("NEWHUD2.X3647") + this.FltVal("NEWHUD2.X3650") + this.FltVal("NEWHUD2.X3653") + this.FltVal("NEWHUD2.X3773") + this.FltVal("NEWHUD2.X3779") + this.FltVal("NEWHUD2.X3782") + this.FltVal("NEWHUD2.X3785") + this.FltVal("NEWHUD2.X3806") + this.FltVal("NEWHUD2.X3812") + this.FltVal("NEWHUD2.X3815") + this.FltVal("NEWHUD2.X3818") + this.FltVal("NEWHUD2.X3839") + this.FltVal("NEWHUD2.X3845") + this.FltVal("NEWHUD2.X3848") + this.FltVal("NEWHUD2.X3851");
      this.SetCurrentNum("NEWHUD.X607", num1);
      double num2 = num1 + (this.addSellerPaidToBorrowerPaid("390", "NEWHUD.X604", "587") + this.addSellerPaidToBorrowerPaid("374", "NEWHUD.X724", "576") + this.addSellerPaidToBorrowerPaid("1641", "NEWHUD.X771", "1642") + this.addSellerPaidToBorrowerPaid("1644", "NEWHUD.X772", "1645"));
      this.SetCurrentNum("NEWHUD.X776", num2);
      this.SetCurrentNum("NEWHUD.X336", num2 - this.FltVal("NEWHUD.X607") + (this.FltVal("390") + this.FltVal("374") + this.FltVal("1641") + this.FltVal("1644")));
      this.SetCurrentNum("NEWHUD.X799", !this.UseNew2015GFEHUD ? this.FltVal("587") + this.FltVal("576") + this.FltVal("1642") + this.FltVal("1645") : this.FltVal("NEWHUD2.X3644") + this.FltVal("NEWHUD2.X3776") + this.FltVal("NEWHUD2.X3809") + this.FltVal("NEWHUD2.X3842"));
      this.SetCurrentNum("NEWHUD.X700", this.FltVal("NEWHUD.X731") + this.FltVal("647") + this.FltVal("648") + this.addSellerPaidToBorrowerPaid("NEWHUD.X731", "NEWHUD.X730", "NEWHUD.X787") + this.addSellerPaidToBorrowerPaid("647", "NEWHUD.X605", "593") + this.addSellerPaidToBorrowerPaid("648", "NEWHUD.X606", "594"));
      this.SetVal("NEWHUD.X41", this.Val("NEWHUD.X251") + (this.Val("NEWHUD.X1085") != string.Empty ? " to " + this.Val("NEWHUD.X1085") : ""));
      this.SetVal("NEWHUD.X43", this.Val("650") + (this.Val("NEWHUD.X1086") != string.Empty ? " to " + this.Val("NEWHUD.X1086") : ""));
      this.SetVal("NEWHUD.X45", this.Val("651") + (this.Val("NEWHUD.X1087") != string.Empty ? " to " + this.Val("NEWHUD.X1087") : ""));
      this.SetVal("NEWHUD.X47", this.Val("40") + (this.Val("NEWHUD.X1088") != string.Empty ? " to " + this.Val("NEWHUD.X1088") : ""));
      this.SetVal("NEWHUD.X49", this.Val("43") + (this.Val("NEWHUD.X1089") != string.Empty ? " to " + this.Val("NEWHUD.X1089") : ""));
      this.SetVal("NEWHUD.X51", this.Val("1782") + (this.Val("NEWHUD.X1090") != string.Empty ? " to " + this.Val("NEWHUD.X1090") : ""));
      this.SetVal("NEWHUD.X53", this.Val("1787") + (this.Val("NEWHUD.X1091") != string.Empty ? " to " + this.Val("NEWHUD.X1091") : ""));
      this.SetVal("NEWHUD.X55", this.Val("1792") + (this.Val("NEWHUD.X1092") != string.Empty ? " to " + this.Val("NEWHUD.X1092") : ""));
    }

    public void calculateHUDGFE(string id, string val)
    {
      if (!this.UseNewGFEHUD && !this.UseNew2015GFEHUD)
        return;
      if (this.Val("2400") == "Y")
      {
        this.SetVal("NEWHUD.X719", this.Val("432"));
        this.SetVal("NEWHUD.X1", this.Val("762"));
      }
      bool flag = this.Val("NEWHUD.X1139") == "Y";
      string str1 = this.Val("1172");
      this.SetCurrentNum("NEWHUD.X4", this.FltVal("2"));
      double num1 = this.FltVal("3119");
      if (num1 != 0.0)
      {
        this.SetVal("NEWHUD.X714", "Include Origination Points");
        this.SetVal("NEWHUD.X715", "Include Origination Points");
        if (!this.UseNew2015GFEHUD)
        {
          this.SetVal("1061", "");
          this.SetVal("436", "");
        }
        this.SetVal("VARRRWS.X4", "");
        this.calObjs.VACal.CalcVARRRWS("1061", "");
        this.SetVal("1847", "");
        this.SetVal("NEWHUD.X734", "");
        this.SetVal("1663", "");
        if (this.Val("CASASRN.X141") == "Borrower")
          this.SetVal("1093", "");
        this.SetVal("NEWHUD.X709", "");
        this.SetVal("NEWHUD.X710", "");
        this.SetVal("NEWHUD.X711", "");
        this.SetVal("NEWHUD.X712", "");
        this.SetVal("NEWHUD.X741", "");
        this.SetVal("NEWHUD.X718", "");
      }
      string str2 = this.Val("NEWHUD.X715");
      string str3 = this.Val("NEWHUD.X713");
      switch (str2)
      {
        case "Include Origination Credit":
          if (this.IsLocked("NEWHUD.X13"))
            this.loan.RemoveLock("NEWHUD.X13");
          if (this.IsLocked("NEWHUD.X720"))
            this.loan.RemoveLock("NEWHUD.X720");
          this.SetVal("NEWHUD.X820", "");
          this.SetVal("NEWHUD.X831", "");
          this.SetVal("NEWHUD.X894", "");
          this.SetVal("NEWHUD.X780", "");
          if (this.UseNew2015GFEHUD)
          {
            this.SetCurrentNum("1061", this.FltVal("NEWHUD.X1150"));
            this.SetCurrentNum("436", this.FltVal("NEWHUD.X1227"));
          }
          else
          {
            this.SetVal("1061", "");
            this.SetVal("436", "");
          }
          this.SetVal("VARRRWS.X4", "");
          this.calObjs.VACal.CalcVARRRWS("1061", "");
          if (!flag)
          {
            this.SetVal("1093", "");
            this.SetVal("NEWHUD.X788", "");
          }
          if (this.UseNew2015GFEHUD)
            this.calculateNEWHUDX15(id, val);
          else
            this.SetVal("NEWHUD.X15", "");
          if (str3 != "Origination Charge")
          {
            this.SetVal("NEWHUD.X713", "Settlement Reduced");
            str3 = "Settlement Reduced";
            break;
          }
          break;
        case "Include Origination Points":
          this.SetVal("1847", "");
          this.SetVal("NEWHUD.X734", "");
          this.SetVal("1663", "");
          if (!flag)
          {
            if (num1 != 0.0)
              this.SetCurrentNum("NEWHUD.X15", num1 - this.FltVal("NEWHUD.X788"));
            else
              this.SetCurrentNum("NEWHUD.X15", this.FltVal("1093") - this.FltVal("NEWHUD.X788"));
            if (str3 != "Origination Charge")
              this.SetCurrentNum("NEWHUD.X780", this.FltVal("NEWHUD.X788"));
            else
              this.SetVal("NEWHUD.X780", "");
          }
          else if (str3 != "Origination Charge")
          {
            if (this.FltVal("NEWHUD.X1149") == 0.0)
              this.SetCurrentNum("NEWHUD.X780", this.FltVal("NEWHUD.X788"));
          }
          else
            this.SetVal("NEWHUD.X780", "");
          if (str3 != "Origination Charge")
          {
            this.SetVal("NEWHUD.X713", "Settlement Increased");
            str3 = "Settlement Increased";
            break;
          }
          break;
        default:
          if (this.IsLocked("NEWHUD.X13"))
            this.loan.RemoveLock("NEWHUD.X13");
          if (this.IsLocked("NEWHUD.X720"))
            this.loan.RemoveLock("NEWHUD.X720");
          this.SetVal("NEWHUD.X780", "");
          if (this.UseNew2015GFEHUD)
          {
            this.SetCurrentNum("1061", this.FltVal("NEWHUD.X1150"));
            this.SetCurrentNum("436", this.FltVal("NEWHUD.X1227"));
          }
          else
          {
            this.SetVal("1061", "");
            this.SetVal("436", "");
          }
          this.SetVal("VARRRWS.X4", "");
          this.calObjs.VACal.CalcVARRRWS("1061", "");
          if (this.UseNew2015GFEHUD)
            this.calculateNEWHUDX15(id, val);
          else
            this.SetVal("NEWHUD.X15", "");
          this.SetVal("1847", "");
          this.SetVal("NEWHUD.X734", "");
          this.SetVal("1663", "");
          if (!flag)
          {
            this.SetVal("NEWHUD.X788", "");
            break;
          }
          break;
      }
      if (str3 != "Origination Charge")
      {
        switch (str2)
        {
          case "Include Origination Credit":
            this.SetCurrentNum("NEWHUD.X687", this.FltVal("1663") * -1.0);
            if (!flag)
            {
              this.SetCurrentNum("PTC.X90", this.FltVal("NEWHUD.X687"));
              break;
            }
            break;
          case "Include Origination Points":
            this.SetCurrentNum("NEWHUD.X687", this.FltVal("NEWHUD.X15"));
            break;
          default:
            this.SetVal("NEWHUD.X687", "");
            break;
        }
      }
      else
        this.SetVal("NEWHUD.X687", "");
      string str4 = this.Val("NEWHUD.X14");
      string str5 = this.Val("NEWHUD.X714");
      switch (str5)
      {
        case "Include Origination Credit":
          this.SetVal("NEWHUD.X709", "");
          this.SetVal("NEWHUD.X710", "");
          if (!flag)
          {
            this.SetVal("NEWHUD.X711", "");
            this.SetVal("NEWHUD.X720", "");
            if (id == "NEWHUD.X718")
            {
              this.SetVal("NEWHUD.X741", "");
              this.SetVal("NEWHUD.X712", "");
            }
            else if ((this.Val("NEWHUD.X712") != string.Empty || this.Val("NEWHUD.X741") != string.Empty) && (!this.calObjs.StopSyncItemization || !this.IsLoanDisclosed() || id == "NEWHUD.X712" || id == "NEWHUD.X741" || id == "NEWHUD.X714"))
              this.SetCurrentNum("NEWHUD.X718", Utils.ArithmeticRounding(this.FltVal("NEWHUD.X712") / 100.0 * this.FltVal("NEWHUD.X4") + this.FltVal("NEWHUD.X741"), 2));
          }
          this.SetCurrentNum("NEWHUD.X13", this.FltVal("NEWHUD.X718"));
          if (str4 != "Origination Charge")
          {
            this.SetVal("NEWHUD.X14", "Settlement Reduced");
            str4 = "Settlement Reduced";
            break;
          }
          break;
        case "Include Origination Points":
          this.SetVal("NEWHUD.X712", "");
          this.SetVal("NEWHUD.X741", "");
          if (!flag)
          {
            this.SetVal("NEWHUD.X718", "");
            this.SetCurrentNum("NEWHUD.X711", Utils.ArithmeticRounding(this.FltVal("2") * this.FltVal("NEWHUD.X709") / 100.0 + this.FltVal("NEWHUD.X710"), 2));
            if (!this.IsLocked("NEWHUD.X720"))
            {
              if (num1 != 0.0)
                this.SetCurrentNum("NEWHUD.X720", num1);
              else if (!this.calObjs.StopSyncItemization || !this.IsLoanDisclosed() || id == "NEWHUD.X709" || id == "NEWHUD.X710" || id == "NEWHUD.X714")
                this.SetCurrentNum("NEWHUD.X720", this.FltVal("NEWHUD.X711"));
            }
          }
          if (str4 != "Origination Charge")
          {
            this.SetVal("NEWHUD.X14", "Settlement Increased");
            str4 = "Settlement Increased";
            break;
          }
          break;
        default:
          this.SetVal("NEWHUD.X709", "");
          this.SetVal("NEWHUD.X710", "");
          this.SetVal("NEWHUD.X711", "");
          this.SetVal("NEWHUD.X712", "");
          this.SetVal("NEWHUD.X741", "");
          this.SetVal("NEWHUD.X718", "");
          this.SetVal("NEWHUD.x720", "");
          if (this.Val("NEWHUD.X713") == string.Empty || this.Val("NEWHUD.X713") == "Settlement Reduced")
          {
            this.SetVal("NEWHUD.X14", "");
            break;
          }
          break;
      }
      if (str4 != "Origination Charge")
      {
        switch (str5)
        {
          case "Include Origination Credit":
            this.SetCurrentNum("NEWHUD.X13", this.FltVal("NEWHUD.X718") * -1.0);
            break;
          case "Include Origination Points":
            this.SetCurrentNum("NEWHUD.X13", this.FltVal("NEWHUD.X720"));
            break;
          default:
            this.SetVal("NEWHUD.X13", "");
            break;
        }
      }
      else
        this.SetVal("NEWHUD.X13", "");
      if (this.FltVal("NEWHUD.X13") < 0.0)
      {
        this.SetCurrentNum("NEWHUD.X721", this.FltVal("NEWHUD.X13") * -1.0);
        this.SetVal("NEWHUD.X722", "");
      }
      else
      {
        this.SetCurrentNum("NEWHUD.X722", this.FltVal("NEWHUD.X13"));
        this.SetVal("NEWHUD.X721", "");
      }
      double num2 = this.FltVal("2");
      if (str1 != "FHA")
      {
        if (str1 == "VA" && this.Val("958") == "IRRRL" && this.Val("19").IndexOf("Refinance") > -1)
          num2 = this.FltVal("VARRRWS.X2");
      }
      else
        num2 = this.FltVal("1109");
      if (!this.calObjs.StopSyncItemization || !this.IsLoanDisclosed() || id == "NEWHUD.X769" || id == "NEWHUD.X770")
      {
        if (!this.IsLocked("NEWHUD.X770"))
        {
          this.SetCurrentNum("NEWHUD.X770", Utils.ArithmeticRounding(this.FltVal("NEWHUD.X769") / 100.0 * num2, 2));
        }
        else
        {
          this.SetCurrentNum("NEWHUD.X769", num2 == 0.0 ? 0.0 : Utils.ArithmeticRounding(this.FltVal("NEWHUD.X770") / num2 * 100.0, 2));
          this.SetCurrentNum("NEWHUD.X770", Utils.ArithmeticRounding(this.FltVal("NEWHUD.X769") / 100.0 * num2, 2));
        }
      }
      if (!this.calObjs.StopSyncItemization || !this.IsLoanDisclosed() || id == "NEWHUD.X705" || id == "NEWHUD.X706")
        this.SetCurrentNum("NEWHUD.X707", Utils.ArithmeticRounding(this.FltVal("NEWHUD.X705") / 100.0 * this.FltVal("2") + this.FltVal("NEWHUD.X706"), 2));
      if (!this.calObjs.StopSyncItemization || !this.IsLoanDisclosed() || id == "NEWHUD.X246" || id == "NEWHUD.X247")
        this.SetCurrentNum("NEWHUD.X250", Utils.ArithmeticRounding(this.FltVal("NEWHUD.X246") / 100.0 * this.FltVal("2") + this.FltVal("NEWHUD.X247"), 2));
      this.VerifyingPOCPTCFields(id, val);
      double num3 = this.FltVal("454") + this.FltVal("L228") + this.FltVal("1621") + this.FltVal("367") + this.FltVal("439") + this.FltVal("NEWHUD.X225") + this.FltVal("155") + this.FltVal("1625") + this.FltVal("1839") + this.FltVal("1842") + this.FltVal("NEWHUD.X733") + this.FltVal("NEWHUD.X1237") + this.FltVal("NEWHUD.X1245") + this.FltVal("NEWHUD.X1253") + this.FltVal("NEWHUD.X1261") + this.FltVal("NEWHUD.X1269") + this.FltVal("NEWHUD.X1277") + this.FltVal("NEWHUD.X1285");
      if (str3 == "Origination Charge" && str2 == "Include Origination Points")
        num3 += this.FltVal("NEWHUD.X15");
      else if (str3 == "Origination Charge" && str2 == "Include Origination Credit")
        num3 -= this.FltVal("1663");
      this.SetCurrentNum("NEWHUD.X686", num3);
      double num4 = this.FltVal("559") + this.FltVal("L229") + this.FltVal("1622") + this.FltVal("569") + this.FltVal("572") + this.FltVal("NEWHUD.X226") + this.FltVal("200") + this.FltVal("1626") + this.FltVal("1840") + this.FltVal("1843") + this.FltVal("NEWHUD.X779") + this.FltVal("NEWHUD.X1238") + this.FltVal("NEWHUD.X1246") + this.FltVal("NEWHUD.X1254") + this.FltVal("NEWHUD.X1262") + this.FltVal("NEWHUD.X1270") + this.FltVal("NEWHUD.X1278") + this.FltVal("NEWHUD.X1286");
      if (!flag || this.FltVal("NEWHUD.X1149") == 0.0)
        num4 += !(str3 == "Origination Charge") || !(str2 == "Include Origination Points") ? 0.0 : this.FltVal("NEWHUD.X788");
      this.SetCurrentNum("NEWHUD.X794", num4);
      this.SetCurrentNum("NEWHUD.X795", num3 + num4);
      this.SetCurrentNum("NEWHUD.X12", this.FltVal("NEWHUD.X770") + this.FltVal("NEWHUD.X702") + this.FltVal("NEWHUD.X703") + this.FltVal("NEWHUD.X704") + this.FltVal("NEWHUD.X707") + this.FltVal("NEWHUD.X250") + this.FltVal("NEWHUD.X727") + this.FltVal("NEWHUD.X729") + this.FltVal("NEWHUD.X736") + this.FltVal("NEWHUD.X738") + this.FltVal("NEWHUD.X740") + this.FltVal("NEWHUD.X1420") + this.FltVal("NEWHUD.X1422") + this.FltVal("NEWHUD.X1424") + this.FltVal("NEWHUD.X1426") + this.FltVal("NEWHUD.X1428") + this.FltVal("NEWHUD.X1430") + this.FltVal("NEWHUD.X1432") + (!(str4 == "Origination Charge") || !(str5 == "Include Origination Points") ? 0.0 : this.FltVal("NEWHUD.X720")) + (!(str4 == "Origination Charge") || !(str5 == "Include Origination Credit") ? 0.0 : this.FltVal("NEWHUD.X718") * -1.0));
      this.SetCurrentNum("NEWHUD.X16", this.FltVal("NEWHUD.X12") + this.FltVal("NEWHUD.X13"));
      this.SetCurrentNum("NEWHUD.X688", this.FltVal("NEWHUD.X686") + this.FltVal("NEWHUD.X687"));
      this.SetCurrentNum("NEWHUD.X797", this.FltVal("NEWHUD.X687") + this.FltVal("NEWHUD.X780"));
      this.SetCurrentNum("NEWHUD.X796", this.FltVal("NEWHUD.X795") + this.FltVal("NEWHUD.X797"));
      this.calObjs.HMDACal.CalcHmdaOriginationCharges(id, val);
      this.SetCurrentNum("NEWHUD.X801", this.FltVal("NEWHUD.X794") + this.FltVal("NEWHUD.X780"));
      double num5 = this.FltVal("NEWHUD.X609") + this.FltVal("NEWHUD.X610") + this.FltVal("NEWHUD.X611") + this.FltVal("NEWHUD.X612") + this.FltVal("NEWHUD.X622");
      for (int index = 0; index < HUDGFE2010Fields.HUDGFE_FIELDS_Block3.Length; ++index)
      {
        string[] strArray = (string[]) HUDGFE2010Fields.HUDGFE_FIELDS_Block3[index];
        num5 += this.FltVal(strArray[1]);
      }
      this.SetCurrentNum("NEWHUD.X17", num5);
      double num6 = 0.0;
      for (int index = 0; index < HUDGFE2010Fields.HUDGFE_FIELDS_Block4.Length; ++index)
      {
        string[] strArray = (string[]) HUDGFE2010Fields.HUDGFE_FIELDS_Block4[index];
        num6 += this.FltVal(strArray[1]);
      }
      this.SetCurrentNum("NEWHUD.X38", num6);
      this.calculateNEWHUDX645(id);
      double num7 = 0.0;
      if (this.UseNew2015GFEHUD)
      {
        for (int index = 2849; index <= 3014; index += 33)
          num7 = num7 + this.FltVal("NEWHUD2.X" + (object) index) + this.FltVal("NEWHUD2.X" + (object) (index + 6)) + this.FltVal("NEWHUD2.X" + (object) (index + 9)) + this.FltVal("NEWHUD2.X" + (object) (index + 12));
        this.SetCurrentNum("NEWHUD.X571", num7);
        for (int index = 3047; index <= 3608; index += 33)
        {
          if (index != 3311)
            num7 = num7 + this.FltVal("NEWHUD2.X" + (object) index) + this.FltVal("NEWHUD2.X" + (object) (index + 6)) + this.FltVal("NEWHUD2.X" + (object) (index + 9)) + this.FltVal("NEWHUD2.X" + (object) (index + 12));
        }
        this.SetCurrentNum("QM.X377", num7);
      }
      else
      {
        for (int index = 125; index <= 131; ++index)
          num7 = num7 + this.FltVal("POPT.X" + (object) index) + this.FltVal("PTC.X" + (object) index);
        double num8 = num7 + (this.FltVal("POPT.X133") + this.FltVal("PTC.X133"));
        for (int index = 134; index <= 139; ++index)
          num8 = num8 + this.FltVal("POPT.X" + (object) index) + this.FltVal("PTC.X" + (object) index);
        this.SetCurrentNum("NEWHUD.X571", num8);
        num7 = num8 + (this.FltVal("POPT.X328") + this.FltVal("PTC.X328")) + (this.FltVal("POPT.X329") + this.FltVal("PTC.X329"));
        this.SetCurrentNum("QM.X377", num7);
      }
      this.SetCurrentNum("NEWHUD.X775", num7 + (this.addSellerPaidToBorrowerPaid("NEWHUD.X952", "NEWHUD.X954", "NEWHUD.X953") + this.addSellerPaidToBorrowerPaid("NEWHUD.X961", "NEWHUD.X963", "NEWHUD.X962") + this.addSellerPaidToBorrowerPaid("NEWHUD.X970", "NEWHUD.X972", "NEWHUD.X971") + this.addSellerPaidToBorrowerPaid("NEWHUD.X979", "NEWHUD.X981", "NEWHUD.X980") + this.addSellerPaidToBorrowerPaid("NEWHUD.X988", "NEWHUD.X990", "NEWHUD.X989") + this.addSellerPaidToBorrowerPaid("NEWHUD.X997", "NEWHUD.X999", "NEWHUD.X998") + this.addSellerPaidToBorrowerPaid("NEWHUD.X215", "NEWHUD.X565", "NEWHUD.X218") + this.addSellerPaidToBorrowerPaid("NEWHUD.X216", "NEWHUD.X566", "NEWHUD.X219") + this.addSellerPaidToBorrowerPaid("NEWHUD.X645", "NEWHUD.X210", "NEWHUD.X782") + this.addSellerPaidToBorrowerPaid("NEWHUD.X639", "NEWHUD.X211", "NEWHUD.X784") + this.addSellerPaidToBorrowerPaid("1763", "NEWHUD.X567", "1764") + this.addSellerPaidToBorrowerPaid("1768", "NEWHUD.X568", "1769") + this.addSellerPaidToBorrowerPaid("1773", "NEWHUD.X569", "1774") + this.addSellerPaidToBorrowerPaid("1778", "NEWHUD.X570", "1779")));
      double num9 = this.addSellerIntendedToLine1101("NEWHUD.X952", "NEWHUD.X954", "NEWHUD.X953") + this.addSellerIntendedToLine1101("NEWHUD.X961", "NEWHUD.X963", "NEWHUD.X962") + this.addSellerIntendedToLine1101("NEWHUD.X970", "NEWHUD.X972", "NEWHUD.X971") + this.addSellerIntendedToLine1101("NEWHUD.X979", "NEWHUD.X981", "NEWHUD.X980") + this.addSellerIntendedToLine1101("NEWHUD.X988", "NEWHUD.X990", "NEWHUD.X989") + this.addSellerIntendedToLine1101("NEWHUD.X997", "NEWHUD.X999", "NEWHUD.X998");
      this.SetVal("NEWHUD.X708", num9 != 0.0 ? num9.ToString("N2") : "");
      double num10;
      if (this.UseNew2015GFEHUD)
      {
        num10 = 0.0;
        for (int index = 2852; index <= 3017; index += 33)
          num10 += this.FltVal("NEWHUD2.X" + (object) index);
      }
      else
        num10 = this.FltVal("NEWHUD.X953") + this.FltVal("NEWHUD.X962") + this.FltVal("NEWHUD.X971") + this.FltVal("NEWHUD.X980") + this.FltVal("NEWHUD.X989") + this.FltVal("NEWHUD.X998") + this.FltVal("NEWHUD.X218") + this.FltVal("NEWHUD.X219") + this.FltVal("NEWHUD.X782") + this.FltVal("NEWHUD.X784") + this.FltVal("1764") + this.FltVal("1769") + this.FltVal("1774") + this.FltVal("1779");
      this.SetCurrentNum("NEWHUD.X798", num10);
      double num11 = 0.0;
      for (int index = 42; index <= 56; index += 2)
        num11 += this.FltVal("NEWHUD.X" + (object) index);
      this.SetCurrentNum("NEWHUD.X40", num11);
      this.SetCurrentNum("NEWHUD.X76", this.FltVal("NEWHUD.X730") + this.FltVal("NEWHUD.X605") + this.FltVal("NEWHUD.X606"));
      this.SetCurrentNum("NEWHUD.X778", this.FltVal("POPT.X141") + this.FltVal("POPT.X142") + this.FltVal("POPT.X143") + this.FltVal("PTC.X141") + this.FltVal("PTC.X142") + this.FltVal("PTC.X143") + this.addSellerPaidToBorrowerPaid("NEWHUD.X731", "NEWHUD.X730", "NEWHUD.X787") + this.addSellerPaidToBorrowerPaid("647", "NEWHUD.X605", "593") + this.addSellerPaidToBorrowerPaid("648", "NEWHUD.X606", "594"));
      double num12 = 0.0;
      for (int index = 692; index <= 699; ++index)
        num12 += this.FltVal("NEWHUD.X" + (object) index);
      if (this.calObjs.CurrentFormID == "REGZGFE_2010")
        this.SetCurrentNum("NEWHUD.X948", this.FltVal("558"));
      this.SetCurrentNum("NEWHUD.X691", num12 + this.FltVal("NEWHUD.X948") + this.FltVal("NEWHUD.X1709"));
      double num13 = 0.0;
      for (int index = 81; index <= 91; index += 2)
        num13 += this.FltVal("NEWHUD.X" + (object) index);
      double num14 = num13 + this.FltVal("NEWHUD.X654");
      if (num14 == 0.0)
        this.SetVal("NEWHUD.X79", "0.00");
      else
        this.SetCurrentNum("NEWHUD.X79", num14);
      this.SetCurrentNum("NEWHUD.X723", this.FltVal("642") + this.addSellerPaidToBorrowerPaid("642", "NEWHUD.X650", "578"));
      this.SetCurrentNum("NEWHUD.X949", this.FltVal("334") + this.addSellerPaidToBorrowerPaid("334", "NEWHUD.X701", "561"));
      this.SetCurrentNum("NEWHUD.X92", this.FltVal("NEWHUD.X17") + this.FltVal("NEWHUD.X38") + this.FltVal("NEWHUD.X39") + this.FltVal("NEWHUD.X40") + this.FltVal("NEWHUD.X214") + this.FltVal("NEWHUD.X76") + this.FltVal("NEWHUD.X691") + this.FltVal("NEWHUD.X701") + this.FltVal("NEWHUD.X79"));
      this.SetCurrentNum("NEWHUD.X93", this.FltVal("NEWHUD.X16") + this.FltVal("NEWHUD.X92"));
      this.triggerTradeOffCalculation = true;
      this.calculateHUD1ToleranceLimits(id, val);
      this.calObjs.ATRQMCal.CalcDiscountPointPercent(id, val);
    }

    internal void CalcLOCompensationTool(string id, string val)
    {
      if (this.Val("NEWHUD.X1139") != "Y")
      {
        this.SetCurrentNum("NEWHUD.X1149", this.Val("NEWHUD.X749") == "Lender" ? this.FltVal("1663") : 0.0);
        this.calculateLenderPAC(id, val);
      }
      else
      {
        this.SetVal("NEWHUD.X780", "");
        this.SetVal("1847", "");
        this.SetVal("NEWHUD.X734", "");
        if (this.UseNew2015GFEHUD)
        {
          this.SetCurrentNum("1061", this.FltVal("NEWHUD.X1150"));
          this.SetCurrentNum("436", this.FltVal("NEWHUD.X1227"));
        }
        else
        {
          this.SetVal("1061", "");
          this.SetVal("436", "");
        }
        if (this.UseNew2015GFEHUD)
          this.calculateNEWHUDX15(id, val);
        else
          this.SetVal("NEWHUD.X15", "");
        if (!this.UseNew2015GFEHUD)
        {
          this.SetVal("NEWHUD.X1156", "");
          this.SetVal("NEWHUD.X1160", "");
          this.SetVal("NEWHUD.X1164", "");
        }
        if (this.Val("NEWHUD.X1718") == "Y")
          this.calObjs.ToolCal.CopyLOCompTo2010Itemization(false);
        this.calculateOriginationAndPoints(id, val, "NEWHUD.X1141", "NEWHUD.X1225", "NEWHUD.X1142", (string) null);
        this.calculateOriginationAndPoints(id, val, "NEWHUD.X1143", "NEWHUD.X1226", "NEWHUD.X1144", (string) null);
        this.SetCurrentNum("NEWHUD.X1149", this.FltVal("NEWHUD.X1142") + this.FltVal("NEWHUD.X1144") + this.FltVal("NEWHUD.X1146") + this.FltVal("NEWHUD.X1148"));
        this.calculateLenderPAC(id, val);
        if (this.FltVal("NEWHUD.X1141") != 0.0 || this.FltVal("NEWHUD.X1225") != 0.0)
        {
          this.SetVal("NEWHUD.X1167", "Lender");
          this.SetVal("NEWHUD.X1168", "Broker");
        }
        else
        {
          this.SetVal("NEWHUD.X1167", "");
          this.SetVal("NEWHUD.X1168", "");
        }
        if (this.FltVal("NEWHUD.X1143") != 0.0 || this.FltVal("NEWHUD.X1226") != 0.0)
          this.SetVal("NEWHUD.X1169", "Lender");
        else
          this.SetVal("NEWHUD.X1169", "");
        if (this.FltVal("NEWHUD.X1146") != 0.0)
          this.SetVal("NEWHUD.X1171", "Lender");
        else
          this.SetVal("NEWHUD.X1171", "");
        if (this.FltVal("NEWHUD.X1148") != 0.0)
          this.SetVal("NEWHUD.X1173", "Lender");
        else
          this.SetVal("NEWHUD.X1173", "");
        this.calculateOriginationAndPoints(id, val, "NEWHUD.X1150", "NEWHUD.X1227", "NEWHUD.X1151", "NEWHUD.X1152");
        this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem802e(id, val);
        this.calculateOriginationAndPoints(id, val, "NEWHUD.X1154", (string) null, "NEWHUD.X1155", "NEWHUD.X1156");
        this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem802f(id, val);
        this.calculateOriginationAndPoints(id, val, "NEWHUD.X1158", (string) null, "NEWHUD.X1159", "NEWHUD.X1160");
        this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem802g(id, val);
        this.calculateOriginationAndPoints(id, val, "NEWHUD.X1162", (string) null, "NEWHUD.X1163", "NEWHUD.X1164");
        this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem802h(id, val);
        double num1 = this.FltVal("NEWHUD.X1151") + this.FltVal("NEWHUD.X1155") + this.FltVal("NEWHUD.X1159") + this.FltVal("NEWHUD.X1163");
        this.SetCurrentNum("NEWHUD.X1165", num1);
        double num2 = this.FltVal("NEWHUD.X1152") + this.FltVal("NEWHUD.X1156") + this.FltVal("NEWHUD.X1160") + this.FltVal("NEWHUD.X1164");
        this.SetCurrentNum("NEWHUD.X1166", num2);
        double num3 = num1 + num2;
        double num4;
        if (this.FltVal("NEWHUD.X1149") > num3)
        {
          this.SetCurrentNum("NEWHUD.X1191", this.FltVal("NEWHUD.X1149") - (this.FltVal("NEWHUD.X1165") + this.FltVal("NEWHUD.X1166")));
          this.SetVal("NEWHUD.X715", "Include Origination Credit");
          if (this.FltVal("NEWHUD.X1191") != 0.0)
          {
            num4 = this.FltVal("NEWHUD.X1191");
            this.SetVal("1663", num4.ToString("N2"));
          }
          else
            this.SetVal("1663", "");
        }
        else if (this.FltVal("NEWHUD.X1149") < num3)
        {
          this.SetVal("1663", "");
          this.SetCurrentNum("NEWHUD.X1191", this.FltVal("NEWHUD.X1165") + this.FltVal("NEWHUD.X1166") - this.FltVal("NEWHUD.X1149"));
          this.SetVal("NEWHUD.X715", "Include Origination Points");
          if (!this.UseNew2015GFEHUD)
          {
            if (this.FltVal("NEWHUD.X1149") == 0.0)
              this.SetCurrentNum("NEWHUD.X15", num1);
            else
              this.SetCurrentNum("NEWHUD.X15", num3 - this.FltVal("NEWHUD.X1149"));
          }
        }
        else if (this.FltVal("NEWHUD.X1149") == 0.0 && num3 == 0.0 && num1 != 0.0 && num2 != 0.0)
        {
          this.SetVal("1663", "");
          this.SetVal("NEWHUD.X1191", "");
          this.SetVal("NEWHUD.X715", "Include Origination Points");
          if (!this.UseNew2015GFEHUD)
            this.SetCurrentNum("NEWHUD.X15", num1);
        }
        else
        {
          this.SetVal("1663", "");
          this.SetVal("NEWHUD.X1191", "");
          this.SetVal("NEWHUD.X715", "");
        }
        this.SetCurrentNum("NEWHUD.X788", num2);
        this.calObjs.MLDSCal.CopyGFEToMLDS("NEWHUD.X15");
        double num5 = num3;
        if (this.UseNew2015GFEHUD)
          num5 -= this.FltVal("NEWHUD2.X956") + this.FltVal("NEWHUD2.X989") + this.FltVal("NEWHUD2.X1022") + this.FltVal("NEWHUD2.X1055");
        this.SetCurrentNum("1093", num5);
        this.SetCurrentNum("1046", num5);
        this.SetVal("NEWHUD.X712", "");
        this.SetVal("NEWHUD.X741", "");
        this.SetVal("NEWHUD.X718", "");
        this.SetVal("NEWHUD.X709", "");
        this.SetVal("NEWHUD.X710", "");
        this.SetVal("NEWHUD.X711", "");
        this.SetVal("NEWHUD.X720", "");
        this.SetVal("NEWHUD.X13", "");
        if (!this.calObjs.StopSyncItemization || !this.IsLoanDisclosed() || id == "NEWHUD.X1200" || id == "NEWHUD.X1228")
          this.calculateOriginationAndPoints(id, val, "NEWHUD.X1200", "NEWHUD.X1228", "NEWHUD.X1201", (string) null);
        if (!this.calObjs.StopSyncItemization || !this.IsLoanDisclosed() || id == "NEWHUD.X1202" || id == "NEWHUD.X1229")
          this.calculateOriginationAndPoints(id, val, "NEWHUD.X1202", "NEWHUD.X1229", "NEWHUD.X1203", (string) null);
        this.SetCurrentNum("NEWHUD.X1206", this.FltVal("NEWHUD.X1201") + this.FltVal("NEWHUD.X1203") + this.FltVal("NEWHUD.X1204") + this.FltVal("NEWHUD.X1205"));
        if (!this.calObjs.StopSyncItemization || !this.IsLoanDisclosed() || id == "NEWHUD.X1207" || id == "NEWHUD.X1230")
          this.calculateOriginationAndPoints(id, val, "NEWHUD.X1207", "NEWHUD.X1230", "NEWHUD.X1208", (string) null);
        if (!this.calObjs.StopSyncItemization || !this.IsLoanDisclosed() || id == "NEWHUD.X1209" || id == "NEWHUD.X1210")
          this.calculateOriginationAndPoints(id, val, "NEWHUD.X1209", (string) null, "NEWHUD.X1210", (string) null);
        if (!this.calObjs.StopSyncItemization || !this.IsLoanDisclosed() || id == "NEWHUD.X1211" || id == "NEWHUD.X1212")
          this.calculateOriginationAndPoints(id, val, "NEWHUD.X1211", (string) null, "NEWHUD.X1212", (string) null);
        if (!this.calObjs.StopSyncItemization || !this.IsLoanDisclosed() || id == "NEWHUD.X1213" || id == "NEWHUD.X1214")
          this.calculateOriginationAndPoints(id, val, "NEWHUD.X1213", (string) null, "NEWHUD.X1214", (string) null);
        this.SetCurrentNum("NEWHUD.X1215", this.FltVal("NEWHUD.X1208") + this.FltVal("NEWHUD.X1210") + this.FltVal("NEWHUD.X1212") + this.FltVal("NEWHUD.X1214"));
        if (this.FltVal("NEWHUD.X1206") > this.FltVal("NEWHUD.X1215"))
        {
          this.SetCurrentNum("NEWHUD.X1216", this.FltVal("NEWHUD.X1206") - this.FltVal("NEWHUD.X1215"));
          this.SetVal("NEWHUD.X714", "Include Origination Credit");
          if (this.FltVal("NEWHUD.X1216") != 0.0)
          {
            num4 = this.FltVal("NEWHUD.X1216");
            this.SetVal("NEWHUD.X718", num4.ToString("N2"));
          }
          this.SetVal("NEWHUD.X13", this.Val("NEWHUD.X718"));
        }
        else if (this.FltVal("NEWHUD.X1206") < this.FltVal("NEWHUD.X1215"))
        {
          this.SetCurrentNum("NEWHUD.X1216", this.FltVal("NEWHUD.X1215") - this.FltVal("NEWHUD.X1206"));
          this.SetVal("NEWHUD.X714", "Include Origination Points");
          this.SetCurrentNum("NEWHUD.X711", this.FltVal("NEWHUD.X1216"));
          this.SetCurrentNum("NEWHUD.X720", this.FltVal("NEWHUD.X1216"));
          this.SetVal("NEWHUD.X13", this.Val("NEWHUD.X720"));
        }
        else
        {
          this.SetVal("NEWHUD.X1216", "");
          this.SetVal("NEWHUD.X714", "");
        }
        this.calculateHUDGFE(id, val);
        this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem802(id, val);
      }
    }

    private void calculateLenderPAC(string id, string val)
    {
      this.SetCurrentNum("NEWHUD2.X911", this.FltVal("NEWHUD.X1149") - this.FltVal("NEWHUD2.X912"));
    }

    private void calculateNEWHUDX15(string id, string val)
    {
      this.SetCurrentNum("NEWHUD.X15", Utils.ArithmeticRounding(this.FltVal("1061") / 100.0 * this.FltVal("2"), 2) + this.FltVal("436") - this.FltVal("NEWHUD.X1152"));
    }

    private void calculateOriginationAndPoints(
      string id,
      string val,
      string rateID,
      string plusFieldID,
      string borID,
      string selID)
    {
      if (this.FltVal("2") == 0.0)
      {
        if (plusFieldID != null)
          this.SetCurrentNum(borID, this.FltVal(plusFieldID) - (selID != null ? this.FltVal(selID) : 0.0));
        else if (selID != null)
          this.SetCurrentNum(borID, this.FltVal(selID) * -1.0);
        else
          this.SetVal(borID, "");
      }
      else if (id == borID)
        this.SetCurrentNum(rateID, (this.FltVal(borID) + (selID != null ? this.FltVal(selID) : 0.0)) / this.FltVal("2") * 100.0);
      else if (selID != null && id == selID)
      {
        if (this.FltVal(rateID) == 0.0 && id != "NEWHUD.X1152")
        {
          double num1 = Utils.ArithmeticRounding((this.FltVal(borID) - (plusFieldID != null ? this.FltVal(plusFieldID) : 0.0) + (selID != null ? this.FltVal(selID) : 0.0)) / this.FltVal("2") * 100.0, 3);
          double num2 = Utils.ArithmeticRounding(num1 / 100.0 * this.FltVal("2") + (plusFieldID != null ? this.FltVal(plusFieldID) : 0.0), 2);
          if (num2 != this.FltVal(borID) + this.FltVal(selID))
          {
            if (!this.loan.IsLocked(borID))
              this.loan.AddLock(borID);
            this.SetCurrentNum(borID, num2 - this.FltVal(selID));
          }
          this.SetCurrentNum(rateID, num1);
        }
        else if (this.IsLocked(borID))
        {
          this.SetCurrentNum(rateID, Utils.ArithmeticRounding((this.FltVal(borID) + this.FltVal(selID)) / this.FltVal("2") * 100.0, 3));
        }
        else
        {
          double num = Utils.ArithmeticRounding(this.FltVal(rateID) / 100.0 * this.FltVal("2") + (plusFieldID != null ? this.FltVal(plusFieldID) : 0.0), 2);
          if (this.FltVal(selID) == num)
            this.SetVal(borID, "");
          else
            this.SetCurrentNum(borID, num - (selID == null ? 0.0 : this.FltVal(selID)));
        }
      }
      else if (this.IsLocked(borID))
        this.SetCurrentNum(rateID, Utils.ArithmeticRounding((this.FltVal(borID) + (selID != null ? this.FltVal(selID) : 0.0)) / this.FltVal("2") * 100.0, 3));
      else
        this.SetCurrentNum(borID, Utils.ArithmeticRounding(this.FltVal(rateID) / 100.0 * this.FltVal("2"), 2) + (plusFieldID != null ? this.FltVal(plusFieldID) : 0.0) - (selID == null ? 0.0 : this.FltVal(selID)));
    }

    internal bool TriggerTradeOffCalculation => this.triggerTradeOffCalculation;

    private void calculateTradeOffTable(string id, string val)
    {
      this.triggerTradeOffCalculation = false;
      if (id == null || id == "1109" || id == "NEWHUD.X95" || id == "NEWHUD.X101" || id == "NEWHUD.X4" || this.IsLocked("NEWHUD.X4") && (id == "NEWHUD.X4" || id == "3"))
      {
        bool calculationDiagnostic = EnConfigurationSettings.GlobalSettings.AllowCalculationDiagnostic;
        if (calculationDiagnostic)
          EnConfigurationSettings.GlobalSettings.AllowCalculationDiagnostic = false;
        LoanData loanData = new LoanData(this.loan.XmlDocClone, this.loan.Settings, false, true);
        LoanCalculator loanCalculator = new LoanCalculator(this.sessionObjects, this.calObjs.LoanConfiguration, loanData, true, this.calObjs.ExternalLateFeeSettings, true);
        loanData.Calculator.PerformanceEnabled = false;
        loanData.Calculator.SetCalculationsToSkip(true, new string[1]
        {
          "NewHUDCalculation:CalculateProjectedPaymentTable"
        });
        if (this.IsLocked("NEWHUD.X4"))
        {
          loanData.RemoveLock("NEWHUD.X4");
          loanData.SetField("1109", this.Val("NEWHUD.X4"));
        }
        if ((this.IsLocked("NEWHUD.X4") && id == "3" || id == "NEWHUD.X4" || id == "1109" || id == null) && this.Val("NEWHUD.X4") != this.Val("1109"))
        {
          loanData.SetFieldFromCal("3", this.Val("3"));
          this.SetCurrentNum("NEWHUD.X217", Utils.ParseDouble((object) loanData.GetField("NEWHUD.X217")));
        }
        if (id == "NEWHUD.X95" || id == "NEWHUD.X4" || id == "1109" || id == null)
        {
          if (this.Val("NEWHUD.X95") == string.Empty)
          {
            this.SetVal("NEWHUD.X96", "");
          }
          else
          {
            loanData.SetFieldFromCal("3", this.Val("NEWHUD.X95"));
            double num = Utils.ParseDouble((object) loanData.GetField("NEWHUD.X217"));
            if (num == 0.0 && this.Val("NEWHUD.X96") != "0.00" || num != 0.0)
              this.SetCurrentNum("NEWHUD.X96", num);
          }
        }
        if (id == "NEWHUD.X101" || id == "NEWHUD.X4" || id == "1109" || id == null)
        {
          if (this.Val("NEWHUD.X101") == string.Empty)
          {
            this.SetVal("NEWHUD.X102", "");
          }
          else
          {
            loanData.SetFieldFromCal("3", this.Val("NEWHUD.X101"));
            double num = Utils.ParseDouble((object) loanData.GetField("NEWHUD.X217"));
            if (num == 0.0 && this.Val("NEWHUD.X102") != "0.00" || num != 0.0)
              this.SetCurrentNum("NEWHUD.X102", num);
          }
        }
        if (calculationDiagnostic)
          EnConfigurationSettings.GlobalSettings.AllowCalculationDiagnostic = calculationDiagnostic;
      }
      double num1 = this.FltVal("NEWHUD.X96");
      if (num1 > 0.0)
      {
        double num2 = num1 - this.FltVal("NEWHUD.X217");
        this.SetCurrentNum("NEWHUD.X97", num2 < 0.0 ? 0.0 : num2);
      }
      else
        this.SetVal("NEWHUD.X97", "");
      double num3 = this.FltVal("NEWHUD.X99");
      if (num3 > 0.0)
      {
        double num4 = this.FltVal("NEWHUD.X93") - num3;
        this.SetCurrentNum("NEWHUD.X98", num4 < 0.0 ? 0.0 : num4);
      }
      else
        this.SetVal("NEWHUD.X98", "");
      double num5 = this.FltVal("NEWHUD.X102");
      if (num5 > 0.0)
      {
        double num6 = this.FltVal("NEWHUD.X217") - num5;
        this.SetCurrentNum("NEWHUD.X103", num6 < 0.0 ? 0.0 : num6);
      }
      else
        this.SetVal("NEWHUD.X103", "");
      if (this.FltVal("NEWHUD.X105") > 0.0)
      {
        double num7 = this.FltVal("NEWHUD.X105") - this.FltVal("NEWHUD.X93");
        this.SetCurrentNum("NEWHUD.X104", num7 < 0.0 ? 0.0 : num7);
      }
      else
        this.SetVal("NEWHUD.X104", "");
    }

    private void calculateNEWHUDX645(string id)
    {
      if (this.Val("1172") == "VA" && !this.UseNew2015GFEHUD)
      {
        for (int index = 809; index <= 818; ++index)
          this.SetVal("NEWHUD.X" + (object) index, "");
      }
      double num = this.FltVal("NEWHUD.X808") + this.FltVal("NEWHUD.X810") + this.FltVal("NEWHUD.X812") + this.FltVal("NEWHUD.X814") + this.FltVal("NEWHUD.X816") + this.FltVal("NEWHUD.X818");
      if (this.Val("14") == "CA" && (this.UseNewGFEHUD || this.UseNew2015GFEHUD) && this.calObjs.CurrentFormID == "REGZGFE_2010")
        this.calObjs.MLDSCal.CopyGFEToMLDS("NEWHUD.X782");
      this.SetCurrentNum("NEWHUD.X645", num);
    }

    private void calculateLoanOriginatorName(string id, string val)
    {
      if (this.Val("3136") == "Y")
        this.SetVal("NEWHUD.X806", this.Val("VEND.X293"));
      else
        this.SetVal("NEWHUD.X806", this.Val("315"));
    }

    private void calculateHUDGFESummary(string id, string val)
    {
      string str1 = this.Val("608");
      string str2 = this.Val("19");
      bool flag1 = this.Val("LOANTERMTABLE.CUSTOMIZE") == "Y" && this.UseNew2015GFEHUD;
      double? nullable1 = new double?();
      string val1 = (string) null;
      double? nullable2 = new double?();
      double? nullable3 = new double?();
      this.SetVal("NEWHUD.X11", "");
      int? nullable4 = new int?();
      string val2 = (string) null;
      double? nullable5 = new double?();
      string val3 = (string) null;
      double? nullable6 = new double?();
      double? nullable7 = new double?();
      bool flag2 = this.Val("423") == "Biweekly";
      int num1 = this.IntVal("1176");
      int num2 = this.IntVal("4");
      int num3 = this.IntVal("325");
      string str3;
      if (!flag2 && str2 == "ConstructionOnly")
      {
        if (num1 < 24 && num1 % 12 != 0)
        {
          nullable4 = new int?(num1);
          str3 = "Month";
        }
        else
        {
          int num4 = (num1 - num1 % 12) / 12;
          if (num1 % 12 > 0)
            ++num4;
          nullable4 = new int?(num4);
          str3 = "Year";
        }
      }
      else if (!flag2 && str2 != "ConstructionOnly" && num2 > num3 && num3 < 24 && num3 % 12 != 0)
      {
        nullable4 = new int?(num3);
        str3 = "Month";
      }
      else if (num3 > 0)
      {
        int num5 = (num3 - num3 % 12) / 12;
        if (num3 % 12 > 0)
          ++num5;
        nullable4 = new int?(num5);
        str3 = "Year";
      }
      else
      {
        nullable4 = new int?(0);
        str3 = "";
      }
      bool flag3 = this.Val("1659") == "Y";
      int? nullable8;
      string val4;
      if (flag3)
      {
        nullable8 = nullable4;
        int num6 = 0;
        if (nullable8.GetValueOrDefault() > num6 & nullable8.HasValue)
        {
          val4 = nullable4.ToString();
          goto label_17;
        }
      }
      val4 = "";
label_17:
      this.SetVal("NEWHUD.X348", val4);
      string val5;
      if (flag3)
      {
        nullable8 = nullable4;
        int num7 = 0;
        if (nullable8.GetValueOrDefault() > num7 & nullable8.HasValue)
        {
          val5 = str3;
          goto label_21;
        }
      }
      val5 = "";
label_21:
      this.SetVal("LE1.X98", val5);
      int num8 = this.IntVal("4");
      int num9 = this.IntVal("325");
      int num10 = this.IntVal("1177");
      switch (str1)
      {
        case "Fixed":
          if (!str2.Contains("Construction") && num10 > 0 && num8 > num9 && (num10 == num9 || num10 == num9 - 1))
            this.SetVal("NEWHUD.X6", "");
          else if (!flag1)
            this.SetVal("NEWHUD.X6", "N");
          if (!flag1)
          {
            bool flag4 = (this.Val("CASASRN.X141") == "Borrower" || this.Val("COMPLIANCEVERSION.CASASRNX141") == "Y" || this.Val("COMPLIANCEVERSION.NEWBUYDOWNENABLED") != "Y") && this.FltVal("1269") > 0.0 && this.IntVal("1613") > 0;
            if (str2 == "ConstructionToPermanent" && this.FltVal("1677") <= this.FltVal("3") || this.Val("425") == "Y" & flag4)
            {
              this.SetVal("NEWHUD.X8", "Y");
              if (str2 == "ConstructionToPermanent" && this.FltVal("1677") == this.FltVal("3"))
                this.SetVal("NEWHUD.X5", "N");
              else
                this.SetVal("NEWHUD.X5", "Y");
              nullable1 = new double?(this.FltVal("3"));
              this.SetVal("LE1.X18", string.Concat((object) (num1 + 1)));
              break;
            }
            this.SetVal("NEWHUD.X5", "N");
            if (str2 == "ConstructionToPermanent" && this.FltVal("4085") < this.FltVal("LE1.X52") || str2 == "ConstructionOnly" && this.Val("SYS.X6") != "B")
            {
              this.SetVal("NEWHUD.X8", "Y");
              break;
            }
            if (this.IntVal("1177") == 0)
            {
              this.SetVal("NEWHUD.X8", "N");
              break;
            }
            this.SetVal("NEWHUD.X8", "Y");
            break;
          }
          break;
        case "AdjustableRate":
          if (!flag1)
            this.SetVal("NEWHUD.X5", "Y");
          if (!str2.Contains("Construction") && num10 > 0 && num8 > num9 && (num10 == num9 || num10 == num9 - 1))
            this.SetVal("NEWHUD.X6", "");
          else if (this.IntVal("1712") != 0 && this.IntVal("1713") != 0)
          {
            if (!flag1)
              this.SetVal("NEWHUD.X6", "Y");
            nullable2 = new double?(this.FltVal("698") / 100.0 * this.FltVal("NEWHUD.X4"));
          }
          else if (!flag1)
            this.SetVal("NEWHUD.X6", "N");
          nullable1 = new double?(this.FltVal("2625"));
          val1 = this.Val("696");
          val2 = this.Val("694");
          nullable5 = new double?(this.FltVal("695"));
          if (!flag1)
            this.SetVal("NEWHUD.X8", "Y");
          nullable6 = new double?(this.FltVal("1699"));
          nullable7 = new double?(this.FltVal("2625"));
          DateTime date = Utils.ParseDate((object) this.Val("682"));
          if (date != DateTime.MinValue && this.IntVal("696") > 0 && this.IntVal("694") > 0)
          {
            val3 = date.AddMonths(this.IntVal("696") - 1).ToString("MM/dd/yyyy");
            break;
          }
          break;
        default:
          if (!(this.Val("NEWHUD.X5") == "Y"))
          {
            if (str1 != "OtherAmortizationType" && !flag1)
            {
              this.SetVal("NEWHUD.X5", "");
              break;
            }
            break;
          }
          goto case "AdjustableRate";
      }
      this.SetVal("HMDA.X115", this.Val("NEWHUD.X6"));
      if (!nullable1.HasValue)
        this.SetVal("NEWHUD.X555", "");
      else
        this.SetCurrentNum("NEWHUD.X555", nullable1.Value);
      this.calObjs.NewHud2015Cal.CalcLECDDisplayFields("NEWHUD.X555", (string) null);
      if (val1 == null)
        this.SetVal("NEWHUD.X556", "");
      else
        this.SetVal("NEWHUD.X556", val1);
      if (val2 == null)
        this.SetVal("NEWHUD.X557", "");
      else
        this.SetVal("NEWHUD.X557", val2);
      if (!nullable5.HasValue)
        this.SetVal("NEWHUD.X558", "");
      else
        this.SetCurrentNum("NEWHUD.X558", nullable5.Value);
      if (!nullable2.HasValue)
        this.SetVal("NEWHUD.X7", "");
      else
        this.SetCurrentNum("NEWHUD.X7", nullable2.Value);
      if (val3 == null)
        this.SetVal("NEWHUD.X332", "");
      else
        this.SetVal("NEWHUD.X332", val3);
      if (!nullable6.HasValue)
        this.SetVal("NEWHUD.X333", "");
      else
        this.SetCurrentNum("NEWHUD.X333", nullable6.Value);
      if (!nullable7.HasValue)
        this.SetVal("NEWHUD.X334", "");
      else
        this.SetCurrentNum("NEWHUD.X334", nullable7.Value);
      if (this.Val("NEWHUD.X8") == "Y")
      {
        int num11 = this.IntVal("1177");
        if (num11 % 6 == 0 & flag2)
          num11 = num11 / 6 * 13;
        double after1stAdjustment = this.calObjs.RegzCal.MortgageInsuranceAfter1stAdjustment;
        int num12 = num2;
        if (flag2)
          num12 = Utils.ParseInt((object) ((double) num12 / 12.0 * 26.0).ToString());
        string val6;
        if (str1 == "Fixed")
        {
          int unpaidTerm = num12 - num11;
          if (unpaidTerm > 0)
          {
            nullable3 = new double?(after1stAdjustment + this.calObjs.RegzCal.CalcRawMonthlyPayment(unpaidTerm, this.calObjs.RegzCal.OutstandingBalanceAfter1stAdjustment, this.FltVal("3"), false));
            double num13 = after1stAdjustment + this.calObjs.RegzCal.CalcRawMonthlyPayment(unpaidTerm, this.calObjs.RegzCal.OutstandingBalanceAfter1stAdjustment, this.FltVal("2625"), false);
            this.SetCurrentNum("NEWHUD.X11", num13, this.UseNoPayment(num13));
            this.calObjs.NewHud2015Cal.CalcLoanTermTable_MonthlyPrincipalAndInterest(id, val);
          }
          val6 = string.Concat((object) num11);
        }
        else
        {
          int num14 = this.IntVal("696");
          if (flag2)
            num14 = Utils.ParseInt((object) ((double) num14 / 12.0 * 26.0).ToString());
          int unpaidTerm = num12 - num14;
          if (unpaidTerm > 0)
          {
            double num15 = after1stAdjustment;
            double num16 = !flag2 ? num15 + this.calObjs.RegzCal.CalcRawMonthlyPayment(unpaidTerm, this.calObjs.RegzCal.OutstandingBalanceAfter1stAdjustment, this.FltVal("3") + this.FltVal("697"), false) : num15 + this.calObjs.RegzCal.CalcRawBiweeklyPayment(unpaidTerm, this.calObjs.RegzCal.OutstandingBalanceAfter1stAdjustment, this.FltVal("3") + this.FltVal("697"), this.Val("4746"));
            if (num11 > num14)
              num16 = Utils.ArithmeticRounding((this.FltVal("3") + this.FltVal("697")) / 100.0 / 12.0 * this.calObjs.RegzCal.OutstandingBalanceAfter1stAdjustment, 2);
            nullable3 = new double?(num16);
          }
          val6 = this.Val("696");
        }
        if (val6 == null)
          this.SetVal("NEWHUD.X9", "");
        else
          this.SetVal("NEWHUD.X9", val6);
        if (!nullable3.HasValue)
          this.SetVal("NEWHUD.X10", "");
        else
          this.SetCurrentNum("NEWHUD.X10", nullable3.Value, this.UseNoPayment(nullable3.Value));
      }
      if (this.FltVal("231") > 0.0 && this.Val("HUD0141") != "" || this.FltVal("235") > 0.0 && this.Val("HUD0144") != "" || this.FltVal("230") > 0.0 && this.Val("HUD0142") != "" || this.FltVal("1630") > 0.0 && this.Val("HUD0146") != "" || this.FltVal("253") > 0.0 && this.Val("HUD0147") != "" || this.FltVal("254") > 0.0 && this.Val("HUD0148") != "" || this.FltVal("L268") > 0.0 && this.Val("HUD0145") != "" || this.FltVal("NEWHUD.X1707") > 0.0 && this.Val("HUD0149") != "")
      {
        this.SetCurrentNum("NEWHUD.X802", this.FltVal("5") + this.FltVal("HUD24"));
        double num17 = this.FltVal("NEWHUD.X802") - this.FltVal("NEWHUD.X217");
        this.SetCurrentNum("NEWHUD.X950", num17 > 0.0 ? num17 : 0.0);
        if (num17 <= 0.0)
        {
          this.SetVal("NEWHUD.X335", "No");
          this.SetVal("NEWHUD.X802", "");
          if (!this.UseNew2015GFEHUD)
          {
            this.SetVal("NEWHUD.X337", "");
            this.SetVal("NEWHUD.X338", "");
            this.SetVal("NEWHUD.X339", "");
            this.SetVal("NEWHUD.X340", "");
            this.SetVal("NEWHUD.X341", "");
            this.SetVal("NEWHUD.X342", "");
            this.SetVal("NEWHUD.X343", "");
            this.SetVal("NEWHUD.X1726", "");
          }
        }
      }
      else
      {
        this.SetVal("NEWHUD.X802", "");
        this.SetVal("NEWHUD.X950", "");
      }
      this.calObjs.NewHud2015Cal.CalcLoanTermTable();
      this.calObjs.NewHud2015Cal.CalcEstimatedTaxesTable();
    }

    public LoanData CalculateWorstCaseScenario() => this.CalculateWorstCaseScenario(true);

    public LoanData CalculateWorstCaseScenario(bool setField)
    {
      bool calculationDiagnostic = EnConfigurationSettings.GlobalSettings.AllowCalculationDiagnostic;
      if (calculationDiagnostic)
        EnConfigurationSettings.GlobalSettings.AllowCalculationDiagnostic = false;
      LoanData loanData = new LoanData(this.loan.XmlDocClone, this.loan.Settings, false, true);
      LoanCalculator loanCalculator = new LoanCalculator(this.sessionObjects, this.calObjs.LoanConfiguration, loanData, true, this.calObjs.ExternalLateFeeSettings, true);
      loanData.Calculator.UseWorstCaseScenario = true;
      loanData.Calculator.UseBestCaseScenario = false;
      PaymentScheduleSnapshot paymentSchedule = loanData.Calculator.GetPaymentSchedule(false);
      if (calculationDiagnostic)
        EnConfigurationSettings.GlobalSettings.AllowCalculationDiagnostic = calculationDiagnostic;
      if (paymentSchedule == null || paymentSchedule.MonthlyPayments == null || paymentSchedule.MonthlyPayments.Length == 0)
      {
        loanData.Calculator.Dispose();
        if (setField)
          this.SetCurrentNum("NEWHUD.X11", 0.0, this.UseNoPayment(0.0));
        return (LoanData) null;
      }
      double num = 0.0;
      PaymentSchedule[] monthlyPayments = paymentSchedule.MonthlyPayments;
      for (int index = 0; index < monthlyPayments.Length && monthlyPayments[index] != null; ++index)
      {
        if (monthlyPayments[index].Payment > num && index < paymentSchedule.ActualNumberOfTerm - 1)
          num = monthlyPayments[index].Payment;
      }
      if (setField)
        this.SetCurrentNum("NEWHUD.X11", num, this.UseNoPayment(num));
      return loanData;
    }

    public PaymentScheduleSnapshot CalculateInterimServicingPaymentSchedule(bool setField)
    {
      bool calculationDiagnostic = EnConfigurationSettings.GlobalSettings.AllowCalculationDiagnostic;
      if (calculationDiagnostic)
        EnConfigurationSettings.GlobalSettings.AllowCalculationDiagnostic = false;
      LoanData loanData = new LoanData(this.loan.XmlDocClone, this.loan.Settings, false, true);
      LoanCalculator loanCalculator = new LoanCalculator(this.sessionObjects, this.calObjs.LoanConfiguration, loanData, true, this.calObjs.ExternalLateFeeSettings, true);
      loanData.Calculator.UseInterimServicingScenario = true;
      loanData.Calculator.UseWorstCaseScenario = false;
      loanData.Calculator.UseBestCaseScenario = false;
      PaymentScheduleSnapshot paymentSchedule = loanData.Calculator.GetPaymentSchedule(setField);
      if (calculationDiagnostic)
        EnConfigurationSettings.GlobalSettings.AllowCalculationDiagnostic = calculationDiagnostic;
      if (paymentSchedule != null && paymentSchedule.MonthlyPayments != null && paymentSchedule.MonthlyPayments.Length != 0)
        return paymentSchedule;
      loanData.Calculator.Dispose();
      return (PaymentScheduleSnapshot) null;
    }

    public void CalculateProjectedPaymentTable(bool setField)
    {
      if (!this.sessionObjects.StartupInfo.UseFullProjectPaymentTriggers && this.calObjs.CalculationIsSkipped("NewHUDCalculation:CalculateProjectedPaymentTable"))
        return;
      using (PerformanceMeter performanceMeter = PerformanceMeter.StartNew(nameof (CalculateProjectedPaymentTable), "Calculate Worst/Best Scenario calculations", true, 2029, nameof (CalculateProjectedPaymentTable), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\CalculationEngine\\NewHUDCalculation.cs"))
      {
        bool flag1 = this.Val("PAYMENTTABLE.USEACTUALPAYMENTCHANGE") == "Y";
        bool flag2 = this.Val("PAYMENTTABLE.CUSTOMIZE") == "Y";
        string str1 = this.Val("608");
        PaymentScheduleSnapshot scheduleSnapshot1 = (PaymentScheduleSnapshot) null;
        bool calculationDiagnostic = EnConfigurationSettings.GlobalSettings.AllowCalculationDiagnostic;
        if (calculationDiagnostic)
          EnConfigurationSettings.GlobalSettings.AllowCalculationDiagnostic = false;
        LoanData loanData = new LoanData(this.loan.XmlDocClone, this.loan.Settings, false, true);
        LoanCalculator loanCalculator = new LoanCalculator(this.sessionObjects, this.calObjs.LoanConfiguration, loanData, true, this.calObjs.ExternalLateFeeSettings, true);
        this.calObjs.ATRQMCal.EffectiveRateForQMAPR = 0.0;
        PaymentScheduleSnapshot paymentSchedule;
        if (str1 == "Fixed")
        {
          loanData.Calculator.UseWorstCaseScenario = true;
          paymentSchedule = loanData.Calculator.GetPaymentSchedule(false);
        }
        else
        {
          performanceMeter.AddCheckpoint("Created Loan Data for Worst/Best Case Scenario", 2055, nameof (CalculateProjectedPaymentTable), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\CalculationEngine\\NewHUDCalculation.cs");
          scheduleSnapshot1 = !this.UseNew2015GFEHUD || !(str1 != "Fixed") ? (PaymentScheduleSnapshot) null : this.GetBestCaseScenarioPaymentSchedule(loanData);
          performanceMeter.AddCheckpoint("Create Best Case Scenario Payment Schedule", 2059, nameof (CalculateProjectedPaymentTable), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\CalculationEngine\\NewHUDCalculation.cs");
          loanData.Calculator.UseWorstCaseScenario = true;
          loanData.Calculator.UseBestCaseScenario = false;
          paymentSchedule = loanData.Calculator.GetPaymentSchedule(false);
          performanceMeter.AddCheckpoint("Create Worst Case Scenario Payment Schedule", 2065, nameof (CalculateProjectedPaymentTable), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\CalculationEngine\\NewHUDCalculation.cs");
        }
        if (calculationDiagnostic)
          EnConfigurationSettings.GlobalSettings.AllowCalculationDiagnostic = calculationDiagnostic;
        if (paymentSchedule != null && paymentSchedule.MonthlyPayments != null)
        {
          PaymentSchedule[] monthlyPayments = paymentSchedule.MonthlyPayments;
          int num = 61;
          if (this.Val("423") == "Biweekly")
            num = 131;
          if (this.Val("19") == "ConstructionToPermanent")
            num += this.IntVal("1176");
          if (monthlyPayments.Length < num)
            num = monthlyPayments.Length;
          if (monthlyPayments[num - 1] != null)
            this.calObjs.ATRQMCal.EffectiveRateForQMAPR = monthlyPayments[num - 1].CurrentRate;
        }
        if (setField)
          this.calObjs.NewHud2015Cal.CalcInterestOnlyPayments();
        performanceMeter.AddCheckpoint("Calculated CalcInterestOnlyPayments", 2096, nameof (CalculateProjectedPaymentTable), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\CalculationEngine\\NewHUDCalculation.cs");
        if (setField)
          this.calObjs.NewHud2015Cal.Calc60Payments((string) null, (string) null);
        this.SetCurrentNum("RE88395.X121", Utils.ParseDouble((object) loanData.GetField("RE88395.X121")));
        this.SetVal("RE88395.X122", loanData.GetField("RE88395.X122"));
        if (!flag2 && (paymentSchedule == null || paymentSchedule.MonthlyPayments == null || paymentSchedule.MonthlyPayments.Length == 0))
        {
          loanData?.Calculator.Dispose();
          if (!setField)
            return;
          this.SetCurrentNum("NEWHUD.X11", 0.0, this.UseNoPayment(0.0));
          this.SetVal("LE1.X12", "");
          this.SetVal("LE1.X18", "");
          this.SetVal("LE1.X25", "");
          this.SetVal("LE1.X88", "");
          this.SetVal("CD4.X30", "");
          this.SetVal("CD4.X34", "");
          this.SetCurrentNum("CD4.X35", 0.0);
          this.SetCurrentNum("CD4.X36", 0.0);
          this.SetVal("CD4.X37", "");
          this.SetCurrentNum("CD4.X38", 0.0);
          this.SetVal("CD4.X39", "");
          for (int index = 7; index <= 43; ++index)
            this.SetVal("CD1.X" + (object) index, "");
          this.calObjs.NewHud2015Cal.CalcLoanTermTable();
        }
        else
        {
          string str2 = this.Val("19");
          double num1 = 0.0;
          double val1 = 0.0;
          double num2 = 0.0;
          int num3 = 0;
          int num4 = 0;
          int num5 = paymentSchedule.ActualNumberOfTerm - 1;
          if (str2 == "ConstructionOnly" && this.Val("SYS.X6") != "B")
            num5 = paymentSchedule.ActualNumberOfTerm;
          int num6 = this.Val("423") == "Biweekly" ? 26 : 12;
          int num7 = this.IntVal("1200") > 0 ? this.IntVal("1198") : 0;
          if (num6 == 26 && num7 > 0)
          {
            int num8 = num7 % 6 != 0 ? 0 : num7 / 6 * 13;
          }
          double num9 = double.MinValue;
          double num10 = double.MaxValue;
          double num11 = double.MinValue;
          int month1 = -1;
          double num12 = double.MinValue;
          double num13 = double.MinValue;
          int num14 = -1;
          List<int> intList1 = new List<int>();
          int num15 = this.IntVal("1177");
          int num16 = this.IntVal("1176");
          int month2 = this.IntVal("1177");
          if (str2 == "ConstructionToPermanent" && num16 > 0)
            month2 += num16;
          if (month2 > 0 && num6 == 26 && month2 % 12 == 0)
            month2 = month2 / 12 * 26;
          bool flag3 = this.Val("CD4.X25") == "Y" || this.Val("CD4.X27") == "Y" || this.Val("NEWHUD.X6") == "Y";
          List<int> intList2 = new List<int>();
          PaymentSchedule[] monthlyPayments1 = paymentSchedule.MonthlyPayments;
          for (int index = 0; index < monthlyPayments1.Length && monthlyPayments1[index] != null; ++index)
          {
            double num17 = Utils.ArithmeticRounding(monthlyPayments1[index].Principal + monthlyPayments1[index].Interest, 2);
            if (index > 0)
            {
              if (monthlyPayments1[index].CurrentRate != monthlyPayments1[index - 1].CurrentRate || num17 != Utils.ArithmeticRounding(monthlyPayments1[index - 1].Principal + monthlyPayments1[index - 1].Interest, 2))
              {
                if (str2 != "ConstructionToPermanent" || monthlyPayments1[index - 1].Principal > 0.0 || monthlyPayments1[index - 1].Principal == 0.0 && monthlyPayments1[index].Principal == 0.0 || str2 == "ConstructionToPermanent" && monthlyPayments1[index - 1].Principal == 0.0 && monthlyPayments1[index].Principal > 0.0 && (index == num16 || num15 > 0 && (num16 % 12 == 0 && index == num16 + num15 || index == num16 + num15 && num15 > 12 - num16 % 12)))
                  intList2.Add(index);
              }
              else if (index == paymentSchedule.ActualNumberOfTerm - 2 && monthlyPayments1[paymentSchedule.ActualNumberOfTerm - 1].Principal + monthlyPayments1[paymentSchedule.ActualNumberOfTerm - 1].Interest > 2.0 * (monthlyPayments1[paymentSchedule.ActualNumberOfTerm - 2].Principal + monthlyPayments1[paymentSchedule.ActualNumberOfTerm - 2].Interest))
                intList2.Add(index);
              else if (index == paymentSchedule.ActualNumberOfTerm - 1 && monthlyPayments1[paymentSchedule.ActualNumberOfTerm - 1].Payment == monthlyPayments1[paymentSchedule.ActualNumberOfTerm - 2].Payment)
                intList2.Add(paymentSchedule.ActualNumberOfTerm - 1);
            }
            if (monthlyPayments1[index].CurrentRate > num2 && index < paymentSchedule.ActualNumberOfTerm - 1)
            {
              num2 = monthlyPayments1[index].CurrentRate;
              num3 = index + 1;
            }
            if (monthlyPayments1[index].Payment > num1 && index < num5)
            {
              num1 = monthlyPayments1[index].Payment;
              num4 = index + 1;
              val1 = num17;
            }
            if (!flag3 && (month2 == 0 || index >= month2))
            {
              if (monthlyPayments1[index].CurrentRate > num9)
              {
                num13 = num11 = num17;
                num14 = month1 = index + 1;
                num12 = num9 = monthlyPayments1[index].CurrentRate;
              }
              if (num17 < num10)
                num10 = num17;
            }
            else if (monthlyPayments1 != null && index < monthlyPayments1.Length && monthlyPayments1[index].CurrentRate > num12)
            {
              num13 = num17;
              num14 = index + 1;
              num12 = monthlyPayments1[index].CurrentRate;
            }
          }
          if (!flag1 && !flag2 && (intList2.Count < 4 || intList2.Count == 4 && month2 % num6 > 0) && str1 == "AdjustableRate" && paymentSchedule != null && monthlyPayments1 != null && scheduleSnapshot1 != null)
          {
            int index1 = Utils.ParseInt((object) this.Val("696"), 0);
            int num18 = Utils.ParseInt((object) this.Val("694"), 0);
            if (num6 > 12)
            {
              index1 = index1 % 6 == 0 ? index1 / 6 * 13 : 0;
              num18 = num18 % 6 == 0 ? num18 / 6 * 13 : 0;
            }
            if (index1 > 0 && num18 > 0 && monthlyPayments1.Length > index1 && monthlyPayments1[index1] != null && paymentSchedule.ActualNumberOfTerm > 0 && monthlyPayments1[paymentSchedule.ActualNumberOfTerm - 1] != null)
            {
              if (month2 % num6 > 0 && month2 + (num6 - month2 % num6) == index1)
              {
                for (int index2 = 0; index2 < intList2.Count; ++index2)
                {
                  if (intList2[index2] == month2)
                  {
                    intList2.RemoveAt(index2);
                    break;
                  }
                }
              }
              int num19 = 4;
              int num20 = 1;
              if (intList2.Count >= 3 && monthlyPayments1[paymentSchedule.ActualNumberOfTerm - 1].Principal + monthlyPayments1[paymentSchedule.ActualNumberOfTerm - 1].Interest > 2.0 * (monthlyPayments1[paymentSchedule.ActualNumberOfTerm - 2].Principal + monthlyPayments1[paymentSchedule.ActualNumberOfTerm - 2].Interest))
                num19 = 3;
              int index3 = 0;
              if (str2 == "ConstructionToPermanent" && str1 == "AdjustableRate")
                index1 += num16;
              for (int index4 = 1; index4 < intList2.Count && intList2[index4] <= index1; ++index4)
                index3 = index4;
              while (intList2.Count <= num19)
              {
                int num21 = intList2[index3] + num20 * num18;
                if (num21 > index1 && !intList2.Contains(num21))
                  intList2.Add(num21);
                ++num20;
              }
              intList2.Sort();
            }
          }
          PaymentSchedule[] paymentScheduleArray = (PaymentSchedule[]) null;
          PaymentScheduleSnapshot scheduleSnapshot2 = (PaymentScheduleSnapshot) null;
          if (monthlyPayments1 != null && monthlyPayments1.Length != 0 && monthlyPayments1[0] != null && this.FltVal("1199") > 0.0 && this.IntVal("1198") > 0)
          {
            scheduleSnapshot2 = this.loan.Calculator.GetPaymentSchedule(false);
            if (scheduleSnapshot2 != null)
            {
              paymentScheduleArray = scheduleSnapshot2.MonthlyPayments;
              if (paymentScheduleArray != null && intList2.Count < 4 && paymentScheduleArray.Length != 0 && scheduleSnapshot2.ActualNumberOfTerm > 0 && paymentScheduleArray[scheduleSnapshot2.ActualNumberOfTerm - 1] != null && paymentScheduleArray[scheduleSnapshot2.ActualNumberOfTerm - 1].MortgageInsurance == 0.0)
              {
                for (int index = 1; index < paymentScheduleArray.Length && paymentScheduleArray[index] != null; ++index)
                {
                  double num22 = Utils.ArithmeticRounding(paymentScheduleArray[index].Principal + paymentScheduleArray[index].Interest, 2);
                  if (paymentScheduleArray[index].MortgageInsurance == 0.0 && paymentScheduleArray[index - 1].MortgageInsurance > 0.0 && paymentScheduleArray[index].CurrentRate == paymentScheduleArray[index - 1].CurrentRate && num22 == Utils.ArithmeticRounding(paymentScheduleArray[index - 1].Principal + paymentScheduleArray[index - 1].Interest, 2))
                  {
                    intList2.Add(index);
                    intList2.Sort();
                    break;
                  }
                }
              }
            }
          }
          performanceMeter.AddCheckpoint("Created paymentChangedIndixes", 2415, nameof (CalculateProjectedPaymentTable), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\CalculationEngine\\NewHUDCalculation.cs");
          if (!setField)
            return;
          if (!flag2)
          {
            for (int index = 7; index <= 43; ++index)
              this.SetVal("CD1.X" + (object) index, "");
            for (int index = 41; index <= 75; ++index)
              this.SetVal("LE1.X" + (object) index, "");
          }
          Hashtable xdFields = new Hashtable();
          xdFields.Add((object) "LE1.XD42", (object) "");
          xdFields.Add((object) "LE1.XD43", (object) "");
          for (int index = 45; index <= 48; ++index)
            xdFields.Add((object) ("LE1.XD" + (object) index), (object) "");
          xdFields.Add((object) "LE1.XD51", (object) "");
          xdFields.Add((object) "LE1.XD52", (object) "");
          for (int index = 54; index <= 57; ++index)
            xdFields.Add((object) ("LE1.XD" + (object) index), (object) "");
          xdFields.Add((object) "LE1.XD60", (object) "");
          xdFields.Add((object) "LE1.XD61", (object) "");
          for (int index = 63; index <= 66; ++index)
            xdFields.Add((object) ("LE1.XD" + (object) index), (object) "");
          xdFields.Add((object) "LE1.XD69", (object) "");
          xdFields.Add((object) "LE1.XD70", (object) "");
          for (int index = 72; index <= 75; ++index)
            xdFields.Add((object) ("LE1.XD" + (object) index), (object) "");
          xdFields.Add((object) "CD1.XD8", (object) "");
          xdFields.Add((object) "CD1.XD9", (object) "");
          xdFields.Add((object) "CD1.XD13", (object) "");
          xdFields.Add((object) "CD1.XD14", (object) "");
          xdFields.Add((object) "CD1.XD17", (object) "");
          xdFields.Add((object) "CD1.XD18", (object) "");
          xdFields.Add((object) "CD1.XD22", (object) "");
          xdFields.Add((object) "CD1.XD23", (object) "");
          xdFields.Add((object) "CD1.XD26", (object) "");
          xdFields.Add((object) "CD1.XD27", (object) "");
          xdFields.Add((object) "CD1.XD31", (object) "");
          xdFields.Add((object) "CD1.XD32", (object) "");
          xdFields.Add((object) "CD1.XD35", (object) "");
          xdFields.Add((object) "CD1.XD36", (object) "");
          xdFields.Add((object) "CD1.XD40", (object) "");
          xdFields.Add((object) "CD1.XD41", (object) "");
          xdFields.Add((object) "CD1.XD11", (object) "");
          xdFields.Add((object) "CD1.XD12", (object) "");
          xdFields.Add((object) "CD1.XD20", (object) "");
          xdFields.Add((object) "CD1.XD21", (object) "");
          xdFields.Add((object) "CD1.XD29", (object) "");
          xdFields.Add((object) "CD1.XD30", (object) "");
          xdFields.Add((object) "CD1.XD38", (object) "");
          xdFields.Add((object) "CD1.XD39", (object) "");
          if (!flag2)
          {
            for (int index = 1; index <= 4; ++index)
            {
              xdFields.Add((object) ("LE1.XD" + (object) index), (object) "");
              xdFields.Add((object) ("CD1.XD" + (object) index), (object) "");
            }
          }
          if (!flag3)
            this.SetVal("CD4.X33", "");
          this.calObjs.NewHud2015Cal.CalcSubsequentChanges();
          performanceMeter.AddCheckpoint("Calculated CalcSubsequentChanges", 2529, nameof (CalculateProjectedPaymentTable), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\CalculationEngine\\NewHUDCalculation.cs");
          this.SetCurrentNum("NEWHUD.X11", num1, this.UseNoPayment(num1));
          int num23 = (num4 + num6 - 1) / num6;
          this.SetVal("LE1.X12", num23 == 0 ? "" : string.Concat((object) num23));
          string str3;
          if (num6 == 12 && num3 < 24 && num3 % 12 != 0)
          {
            str3 = "Month";
          }
          else
          {
            num3 = (num3 - 1 + num6) / num6;
            str3 = "Year";
          }
          this.SetVal("LE1.X18", num3 == 0 ? "" : string.Concat((object) num3));
          this.SetVal("LE1.X99", num3 == 0 ? "" : str3);
          if (str2 == "ConstructionOnly" && this.Val("SYS.X6") != "B")
          {
            this.SetVal("LE1.X23", "Can go");
            if (str3 == "Month" && num4 == num5)
              this.SetVal("LE1.X25", "1");
            else
              this.SetVal("LE1.X25", num23 == 0 ? "" : string.Concat((object) num23));
            this.SetVal("LE1.X88", "Month");
          }
          else
          {
            string val2;
            switch (num4)
            {
              case 0:
                val2 = "";
                break;
              case 13:
                val2 = string.Concat((object) num23);
                break;
              default:
                if (num4 <= 24)
                {
                  val2 = string.Concat((object) num4);
                  break;
                }
                goto case 13;
            }
            this.SetVal("LE1.X25", val2);
            string val3;
            switch (num4)
            {
              case 0:
                val3 = "";
                break;
              case 13:
                val3 = "Year";
                break;
              default:
                if (num4 <= 24)
                {
                  val3 = "Month";
                  break;
                }
                goto case 13;
            }
            this.SetVal("LE1.X88", val3);
          }
          this.calObjs.NewHud2015Cal.CalcLoanTermTable();
          performanceMeter.AddCheckpoint("Calculated CalcLoanTermTable", 2608, nameof (CalculateProjectedPaymentTable), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\CalculationEngine\\NewHUDCalculation.cs");
          PaymentSchedule[] monthlyPayments2 = scheduleSnapshot1?.MonthlyPayments;
          bool flag4 = this.Val("4746") != "NoPayment";
          bool flag5 = this.UseNoPayment(0.0);
          bool flag6 = this.Val("1659") == "Y" && (!flag5 || flag5 & flag4);
          if (monthlyPayments1 != null && paymentSchedule.ActualNumberOfTerm > 0 && (str1 == "Fixed" || monthlyPayments2 != null && scheduleSnapshot1.ActualNumberOfTerm > 0))
          {
            this.FltVal("3");
            int num24 = this.IntVal("4");
            this.IntVal("325");
            int num25 = Utils.ParseInt((object) this.Val("696"), 0);
            string str4 = this.Val("1962");
            int num26 = 0;
            int num27 = 0;
            bool flag7 = str2 == "ConstructionToPermanent" && num16 % 12 != 0;
            bool flag8 = false;
            int num28;
            if (!flag2)
            {
              for (int index5 = 0; index5 < intList2.Count; ++index5)
              {
                int num29 = intList2[index5];
                if (((num29 != paymentSchedule.ActualNumberOfTerm - 1 ? 0 : (monthlyPayments1[paymentSchedule.ActualNumberOfTerm - 1].Payment >= monthlyPayments1[paymentSchedule.ActualNumberOfTerm - 2].Payment * 2.0 ? 1 : 0)) & (flag4 ? 1 : 0)) != 0)
                {
                  flag8 = true;
                  break;
                }
                int index6 = 0;
                if (num26 > 0)
                {
                  num27 = this.IntVal("CD1.X" + (object) (7 + (num26 - 1) * 9)) + 1;
                  this.SetVal("CD1.X" + (object) (6 + num26 * 9), num27.ToString());
                  index6 = num27 * num6 - num6;
                }
                if (num29 == paymentSchedule.ActualNumberOfTerm - 1 || num26 == 3)
                {
                  int num30 = paymentSchedule.ActualNumberOfTerm % num6;
                  num28 = (paymentSchedule.ActualNumberOfTerm + (num30 > 0 ? num6 - num30 : 0)) / num6;
                }
                else
                {
                  int num31 = num29 % num6;
                  if ((str2 != "ConstructionToPermanent" && index5 > 0 || str2 == "ConstructionToPermanent" && index5 > 1) && num29 - intList2[index5 - 1] < num6 && intList2[index5 - 1] % num6 > 0)
                    num29 += num6;
                  num28 = (num29 + (num31 > 0 ? num6 - num31 : 0)) / num6;
                  if (str2 == "ConstructionOnly" && str1 != "Fixed" && num26 == 0 && num16 > 12 && num16 % 12 != 0 && num25 > 12)
                    num28 = (num16 - num16 % 12) / 12 + 1;
                  else if ((str2 == "ConstructionToPermanent" || str2 == "ConstructionOnly" && str1 == "Fixed") && num26 == 0 && num16 > 12 && num16 % 12 != 0)
                    num28 = (num16 - num16 % 12) / 12 + 1;
                  else if (num28 < num27 && num27 != 0)
                    num28 = num27;
                }
                if (index6 < paymentSchedule.ActualNumberOfTerm)
                {
                  this.SetVal("CD1.X" + (object) (7 + num26 * 9), num28.ToString());
                  int num32 = num28 == 0 ? 0 : num28 * num6 - num6;
                  if (num26 == 3 || index5 == intList2.Count - 1)
                  {
                    if ((flag6 || num6 == 26) && Utils.ArithmeticRounding(monthlyPayments1[paymentSchedule.ActualNumberOfTerm - 1].Principal + monthlyPayments1[paymentSchedule.ActualNumberOfTerm - 1].Interest, 2) > Utils.ArithmeticRounding(monthlyPayments1[paymentSchedule.ActualNumberOfTerm - 2].Principal + monthlyPayments1[paymentSchedule.ActualNumberOfTerm - 2].Interest, 2))
                    {
                      double num33 = Utils.ArithmeticRounding(monthlyPayments1[paymentSchedule.ActualNumberOfTerm - 1].Principal + monthlyPayments1[paymentSchedule.ActualNumberOfTerm - 1].Interest, 2);
                      this.SetCurrentNum("CD1.X" + (object) (9 + num26 * 9), num33, this.UseNoPayment(num33));
                    }
                    else
                    {
                      double num34 = Utils.ArithmeticRounding(monthlyPayments1[paymentSchedule.ActualNumberOfTerm - 2].Principal + monthlyPayments1[paymentSchedule.ActualNumberOfTerm - 2].Interest, 2);
                      this.SetCurrentNum("CD1.X" + (object) (9 + num26 * 9), num34, this.UseNoPayment(num34));
                    }
                    if (str1 != "Fixed")
                    {
                      if (scheduleSnapshot1 != null && scheduleSnapshot1.ActualNumberOfTerm >= 2 && (flag6 || num6 == 26) && Utils.ArithmeticRounding(monthlyPayments2[scheduleSnapshot1.ActualNumberOfTerm - 1].Principal + monthlyPayments2[scheduleSnapshot1.ActualNumberOfTerm - 1].Interest, 2) < Utils.ArithmeticRounding(monthlyPayments2[scheduleSnapshot1.ActualNumberOfTerm - 2].Principal + monthlyPayments2[scheduleSnapshot1.ActualNumberOfTerm - 2].Interest, 2))
                      {
                        if (Utils.ArithmeticRounding(monthlyPayments2[scheduleSnapshot1.ActualNumberOfTerm - 2].Principal + monthlyPayments2[scheduleSnapshot1.ActualNumberOfTerm - 2].Interest, 2) < Utils.ArithmeticRounding(monthlyPayments2[index6].Principal + monthlyPayments2[index6].Interest, 2))
                        {
                          double num35 = Utils.ArithmeticRounding(monthlyPayments2[scheduleSnapshot1.ActualNumberOfTerm - 2].Principal + monthlyPayments2[scheduleSnapshot1.ActualNumberOfTerm - 2].Interest, 2);
                          this.SetCurrentNum("CD1.X" + (object) (8 + num26 * 9), num35, this.UseNoPayment(num35));
                        }
                        else
                        {
                          double num36 = Utils.ArithmeticRounding(monthlyPayments2[index6].Principal + monthlyPayments2[index6].Interest, 2);
                          this.SetCurrentNum("CD1.X" + (object) (8 + num26 * 9), num36, this.UseNoPayment(num36));
                        }
                      }
                      else if (scheduleSnapshot1 != null && scheduleSnapshot1.ActualNumberOfTerm >= 2 && Utils.ArithmeticRounding(monthlyPayments2[scheduleSnapshot1.ActualNumberOfTerm - 2].Principal + monthlyPayments2[scheduleSnapshot1.ActualNumberOfTerm - 2].Interest, 2) < Utils.ArithmeticRounding(monthlyPayments2[index6].Principal + monthlyPayments2[index6].Interest, 2))
                      {
                        double num37 = Utils.ArithmeticRounding(monthlyPayments2[scheduleSnapshot1.ActualNumberOfTerm - 2].Principal + monthlyPayments2[scheduleSnapshot1.ActualNumberOfTerm - 2].Interest, 2);
                        this.SetCurrentNum("CD1.X" + (object) (8 + num26 * 9), num37, this.UseNoPayment(num37));
                      }
                      else if (scheduleSnapshot1 != null && index6 < monthlyPayments2.Length)
                      {
                        double num38 = Utils.ArithmeticRounding(monthlyPayments2[index6].Principal + monthlyPayments2[index6].Interest, 2);
                        this.SetCurrentNum("CD1.X" + (object) (8 + num26 * 9), num38, this.UseNoPayment(num38));
                      }
                    }
                    else if (index5 == intList2.Count - 1 && !flag6 && num6 != 26 && str1 == "Fixed")
                    {
                      double num39 = Utils.ArithmeticRounding(monthlyPayments1[paymentSchedule.ActualNumberOfTerm - 2].Principal + monthlyPayments1[paymentSchedule.ActualNumberOfTerm - 2].Interest, 2);
                      this.SetCurrentNum("CD1.X" + (object) (8 + num26 * 9), num39, this.UseNoPayment(num39));
                    }
                  }
                  else
                  {
                    double num40 = Utils.ArithmeticRounding(monthlyPayments1[index6 == 0 ? num32 : index6].Principal + monthlyPayments1[index6 == 0 ? num32 : index6].Interest, 2);
                    if (num26 == 0 && str2 == "ConstructionOnly" && str1 != "Fixed" && num16 > 12 && num25 >= 12)
                    {
                      double num41 = Utils.ArithmeticRounding(num28 * num6 - 1 >= paymentSchedule.ActualNumberOfTerm ? monthlyPayments1[paymentSchedule.ActualNumberOfTerm - 2].Principal + monthlyPayments1[paymentSchedule.ActualNumberOfTerm - 1].Interest : monthlyPayments1[num28 * num6 - 2].Principal + monthlyPayments1[num28 * num6 - 1].Interest, 2);
                      if (num41 > num40)
                        num40 = num41;
                    }
                    else if (num28 * num6 - 1 < paymentSchedule.ActualNumberOfTerm && monthlyPayments1[num28 * num6 - 1] != null && (num15 != 0 || num26 != 0 || num25 <= 12 || num25 % 12 == 0 || str2.StartsWith("Construction")) && (!(str1 == "Fixed") || !(str2 == "ConstructionToPermanent") || num26 != 0 || num16 <= 12 || num15 != 0) && (!(str1 != "Fixed") || !(str2 == "ConstructionOnly") || num26 != 0 || num16 != 12 || num15 != 0 || num25 != 12) && (num26 > 0 || num26 == 0 && (num15 <= 12 || num15 % 12 == 0 || num25 <= 12 || num25 % 12 == 0)))
                    {
                      double num42 = Utils.ArithmeticRounding(monthlyPayments1[num28 * num6 - 1].Principal + monthlyPayments1[num28 * num6 - 1].Interest, 2);
                      if (num42 > num40)
                        num40 = num42;
                    }
                    this.SetCurrentNum("CD1.X" + (object) (9 + num26 * 9), num40, this.UseNoPayment(num40));
                    double num43 = 0.0;
                    if (str2 == "ConstructionOnly" && str1 != "Fixed" && num16 > 12)
                      num43 = num28 * num6 - 1 >= scheduleSnapshot1.ActualNumberOfTerm ? Utils.ArithmeticRounding(monthlyPayments2[scheduleSnapshot1.ActualNumberOfTerm - 1].Interest, 2) : Utils.ArithmeticRounding(monthlyPayments2[num28 * num6 - 1].Interest, 2);
                    else if (flag7 && num26 == 0 && (num16 < 12 || num16 > 12 && num16 % 12 != 0))
                      num43 = monthlyPayments1[0].Interest;
                    else if (str1 != "Fixed")
                    {
                      if (num26 == 0 && (num15 == 0 || num15 >= 12) && (num25 == 0 || num25 >= 12))
                      {
                        num43 = num40;
                      }
                      else
                      {
                        num43 = Utils.ArithmeticRounding(monthlyPayments2[index6 == 0 ? num32 : index6].Principal + monthlyPayments2[index6 == 0 ? num32 : index6].Interest, 2);
                        if (num28 * num6 - 1 < scheduleSnapshot1.ActualNumberOfTerm && monthlyPayments2[num28 * num6 - 1] != null && (str2 != "ConstructionOnly" || num16 <= 12 || num26 != 0))
                        {
                          double num44 = Utils.ArithmeticRounding(monthlyPayments2[num28 * num6 - 1].Principal + monthlyPayments2[num28 * num6 - 1].Interest, 2);
                          if (num44 < num43)
                            num43 = num44;
                        }
                        if (num15 > 0 && num15 % 12 > 0 && num15 >= index6 && num15 <= num28 * num6 - 1 && monthlyPayments2[num15 - 1] != null)
                        {
                          double num45 = Utils.ArithmeticRounding(monthlyPayments2[num15 - 1].Principal + monthlyPayments2[num15 - 1].Interest, 2);
                          if (num45 < num43)
                            num43 = num45;
                        }
                      }
                    }
                    else if (num26 == 0)
                    {
                      if ((num15 == 0 || num15 > 0 && num15 % 12 > 0) && monthlyPayments1[index6 == 0 ? num32 : index6] != null)
                        num43 = Utils.ArithmeticRounding(monthlyPayments1[index6 == 0 ? num32 : index6].Principal + monthlyPayments1[index6 == 0 ? num32 : index6].Interest, 2);
                      else if (str2 == "ConstructionToPermanent" && num15 > 0 && num15 % 12 == 0 && monthlyPayments1[index6] != null)
                        num43 = Utils.ArithmeticRounding(monthlyPayments1[index6].Principal + monthlyPayments1[index6].Interest, 2);
                    }
                    else if (str2 == "ConstructionToPermanent" && monthlyPayments1[index6] != null)
                      num43 = Utils.ArithmeticRounding(monthlyPayments1[index6].Principal + monthlyPayments1[index6].Interest, 2);
                    this.SetCurrentNum("CD1.X" + (object) (8 + num26 * 9), num43, this.UseNoPayment(num43));
                  }
                  this.SetVal("CD1.X" + (object) (10 + num26 * 9), monthlyPayments1[index6 == 0 ? num32 : index6].Principal == 0.0 ? "Y" : "N");
                  int index7 = index6 != 0 || num26 <= 0 ? index6 : num32;
                  if (paymentScheduleArray != null && index7 < paymentScheduleArray.Length && paymentScheduleArray[index7] != null)
                  {
                    double mortgageInsurance = paymentScheduleArray[index7].MortgageInsurance;
                    if (mortgageInsurance == 0.0 && str2 == "ConstructionToPermanent" && num28 * num6 - 1 < paymentScheduleArray.Length && paymentScheduleArray[num28 * num6 - 1] != null)
                      mortgageInsurance = paymentScheduleArray[num28 * num6 - 1].MortgageInsurance;
                    this.SetCurrentNum("CD1.X" + (object) (11 + num26 * 9), mortgageInsurance);
                  }
                  else if (str2 == "ConstructionOnly" && monthlyPayments1 != null && index7 < monthlyPayments1.Length && monthlyPayments1[index7] != null)
                    this.SetCurrentNum("CD1.X" + (object) (11 + num26 * 9), monthlyPayments1[index7].MortgageInsurance);
                  else
                    this.SetCurrentNum("CD1.X" + (object) (11 + num26 * 9), 0.0);
                  ++num26;
                  if (num26 >= 4 || paymentSchedule.ActualNumberOfTerm - num32 < num6 && paymentSchedule.ActualNumberOfTerm % num6 > 0)
                    break;
                }
                else
                  break;
              }
            }
            if (((flag8 ? 0 : (str2 == "ConstructionOnly" ? 1 : 0)) & (flag4 ? 1 : 0)) != 0)
              flag8 = true;
            performanceMeter.AddCheckpoint("Calculated Min/Max Payments", 2891, nameof (CalculateProjectedPaymentTable), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\CalculationEngine\\NewHUDCalculation.cs");
            if (!flag2)
            {
              this.SetVal("NEWHUD2.XPJT", "");
              if (intList2.Count == 0)
              {
                this.SetVal("CD1.X7", "1");
                double num46 = Utils.ArithmeticRounding(monthlyPayments1[0].Principal + monthlyPayments1[0].Interest, 2);
                this.SetCurrentNum("CD1.X9", num46, this.UseNoPayment(num46));
                this.SetCurrentNum("CD1.X14", monthlyPayments1[0].Payment, this.UseNoPayment(monthlyPayments1[0].Payment));
                ++num26;
              }
              else if (flag6 | flag8)
              {
                if (flag6)
                  this.SetVal("NEWHUD2.XPJT", "Balloon_Final_Payment");
                else if (flag8)
                  this.SetVal("NEWHUD2.XPJT", "Final_Payment");
                if (intList2.Count == 1 && paymentSchedule.ActualNumberOfTerm <= num6)
                {
                  this.SetVal("CD1.X16", "1");
                  double num47 = Utils.ArithmeticRounding(monthlyPayments1[paymentSchedule.ActualNumberOfTerm - 1].Principal + monthlyPayments1[paymentSchedule.ActualNumberOfTerm - 1].Interest, 2);
                  this.SetCurrentNum("CD1.X18", num47, this.UseNoPayment(num47));
                  this.SetCurrentNum("CD1.X20", monthlyPayments1[paymentSchedule.ActualNumberOfTerm - 1].MortgageInsurance);
                  this.SetCurrentNum("CD1.X23", monthlyPayments1[paymentSchedule.ActualNumberOfTerm - 1].Payment, this.UseNoPayment(monthlyPayments1[paymentSchedule.ActualNumberOfTerm - 1].Payment));
                }
                else
                {
                  if (num26 >= 4)
                    num26 = 3;
                  this.SetVal("CD1.X" + (object) (7 + num26 * 9), "1");
                  if (str1 != "Fixed")
                  {
                    if (str2 == "ConstructionOnly" && num16 <= 12)
                    {
                      double interest = monthlyPayments2[0].Interest;
                      if (monthlyPayments2[scheduleSnapshot1.ActualNumberOfTerm - 1].Interest < interest)
                        interest = monthlyPayments2[scheduleSnapshot1.ActualNumberOfTerm - 1].Interest;
                      double num48 = Utils.ArithmeticRounding(monthlyPayments2[scheduleSnapshot1.ActualNumberOfTerm - 1].Principal + interest, 2);
                      this.SetCurrentNum("CD1.X" + (object) (8 + num26 * 9), num48, this.UseNoPayment(num48));
                    }
                    else
                    {
                      double num49 = Utils.ArithmeticRounding(monthlyPayments2[scheduleSnapshot1.ActualNumberOfTerm - 1].Principal + monthlyPayments2[scheduleSnapshot1.ActualNumberOfTerm - 1].Interest, 2);
                      this.SetCurrentNum("CD1.X" + (object) (8 + num26 * 9), num49, this.UseNoPayment(num49));
                    }
                  }
                  else if (str2 == "ConstructionToPermanent")
                  {
                    double num50 = Utils.ArithmeticRounding(monthlyPayments1[paymentSchedule.ActualNumberOfTerm - 1].Principal + monthlyPayments1[paymentSchedule.ActualNumberOfTerm - 1].Interest, 2);
                    this.SetCurrentNum("CD1.X" + (object) (8 + num26 * 9), num50, this.UseNoPayment(num50));
                  }
                  double num51 = Utils.ArithmeticRounding(monthlyPayments1[paymentSchedule.ActualNumberOfTerm - 1].Principal + monthlyPayments1[paymentSchedule.ActualNumberOfTerm - 1].Interest, 2);
                  this.SetCurrentNum("CD1.X" + (object) (9 + num26 * 9), num51, this.UseNoPayment(num51));
                  if (paymentScheduleArray != null && scheduleSnapshot2.ActualNumberOfTerm > 0 && paymentScheduleArray[scheduleSnapshot2.ActualNumberOfTerm - 1] != null)
                    this.SetCurrentNum("CD1.X" + (object) (11 + num26 * 9), paymentScheduleArray[scheduleSnapshot2.ActualNumberOfTerm - 1].MortgageInsurance);
                  else
                    this.SetCurrentNum("CD1.X" + (object) (11 + num26 * 9), 0.0);
                  int num52 = paymentSchedule.ActualNumberOfTerm % num6;
                  num28 = (paymentSchedule.ActualNumberOfTerm + (num52 > 0 ? num6 - num52 : 0)) / num6;
                  this.SetVal("CD1.X" + (object) (7 + (num26 - 1) * 9), string.Concat((object) num28));
                  if (str2 == "ConstructionOnly" && str1 != "Fixed")
                  {
                    if (monthlyPayments1[paymentSchedule.ActualNumberOfTerm - 1] != null && (num16 < 12 || num25 < 12))
                    {
                      double interest1 = monthlyPayments2[scheduleSnapshot1.ActualNumberOfTerm - 1].Interest;
                      double interest2 = monthlyPayments1[paymentSchedule.ActualNumberOfTerm - 1].Interest;
                      if (str4.StartsWith("365/"))
                      {
                        int num53 = scheduleSnapshot1.ActualNumberOfTerm - 2;
                        for (int index = 0; index <= num53; ++index)
                        {
                          if (monthlyPayments2[index].Interest < interest1)
                            interest1 = monthlyPayments2[index].Interest;
                        }
                        int num54 = paymentSchedule.ActualNumberOfTerm - 2;
                        for (int index = 0; index <= num54; ++index)
                        {
                          if (monthlyPayments1[index].Interest > interest2)
                            interest2 = monthlyPayments1[index].Interest;
                        }
                      }
                      this.SetCurrentNum("CD1.X" + (object) (8 + (num26 - 1) * 9), interest1, this.UseNoPayment(interest1));
                      this.SetCurrentNum("CD1.X" + (object) (9 + (num26 - 1) * 9), interest2, this.UseNoPayment(interest2));
                    }
                  }
                  else
                  {
                    if (str1 != "Fixed" && scheduleSnapshot1 != null && scheduleSnapshot1.ActualNumberOfTerm >= 2)
                    {
                      double num55 = Utils.ArithmeticRounding(monthlyPayments2[scheduleSnapshot1.ActualNumberOfTerm - 2].Principal + monthlyPayments2[scheduleSnapshot1.ActualNumberOfTerm - 2].Interest, 2);
                      this.SetCurrentNum("CD1.X" + (object) (8 + (num26 - 1) * 9), num55, this.UseNoPayment(num55));
                    }
                    double num56 = Utils.ArithmeticRounding(monthlyPayments1[paymentSchedule.ActualNumberOfTerm - 2].Principal + monthlyPayments1[paymentSchedule.ActualNumberOfTerm - 2].Interest, 2);
                    this.SetCurrentNum("CD1.X" + (object) (9 + (num26 - 1) * 9), num56, this.UseNoPayment(num56));
                  }
                  if (str2 == "ConstructionOnly" || str2 == "ConstructionToPermanent")
                  {
                    this.SetVal("3287", monthlyPayments1[paymentSchedule.ActualNumberOfTerm - 1].PayDate);
                    this.SetCurrentNum("3288", monthlyPayments1[paymentSchedule.ActualNumberOfTerm - 1].Payment);
                  }
                }
                ++num26;
                if (flag6 && this.Val("CD1.X" + (object) (10 + (num26 - 1) * 9)) == "Y")
                  this.SetVal("CD1.X" + (object) (10 + (num26 - 1) * 9), "N");
              }
              else if (num26 < 4 && str1 != "Fixed" && paymentSchedule.ActualNumberOfTerm < num24 && Utils.ArithmeticRounding(monthlyPayments1[paymentSchedule.ActualNumberOfTerm - 1].Principal + monthlyPayments1[paymentSchedule.ActualNumberOfTerm - 1].Interest, 2) > this.FltVal("CD1.X" + (object) (9 + (num26 - 1) * 9)))
              {
                if (str1 != "Fixed")
                {
                  double num57 = Utils.ArithmeticRounding(monthlyPayments2[scheduleSnapshot1.ActualNumberOfTerm - 1].Principal + monthlyPayments2[scheduleSnapshot1.ActualNumberOfTerm - 1].Interest, 2);
                  this.SetCurrentNum("CD1.X" + (object) (8 + (num26 - 1) * 9), num57, this.UseNoPayment(num57));
                }
                double num58 = Utils.ArithmeticRounding(monthlyPayments1[paymentSchedule.ActualNumberOfTerm - 1].Principal + monthlyPayments1[paymentSchedule.ActualNumberOfTerm - 1].Interest, 2);
                this.SetCurrentNum("CD1.X" + (object) (9 + (num26 - 1) * 9), num58, this.UseNoPayment(num58));
              }
            }
            if (!flag2)
              this.SetVal("NEWHUD2.XPJTCOLUMNS", string.Concat((object) num26));
            else
              num26 = this.IntVal("NEWHUD2.XPJTCOLUMNS");
            if (!flag2 && str1 == "Fixed" && str2 != "ConstructionToPermanent")
            {
              for (int index = 0; index < num26; ++index)
              {
                if (index != 0 || num15 <= 0 || num15 % 12 <= 0)
                  this.SetCurrentNum("CD1.X" + (object) (8 + index * 9), this.FltVal("CD1.X" + (object) (9 + index * 9)), this.UseNoPayment(this.FltVal("CD1.X" + (object) (9 + index * 9))));
              }
            }
            double num59;
            double num60;
            if (num6 == 26)
            {
              double num61 = (this.Val("NEWHUD.X337") == "Y" ? this.FltVal("HUD52") : 0.0) + (this.Val("NEWHUD.X339") == "Y" ? this.FltVal("HUD53") : 0.0) + (this.Val("NEWHUD.X338") == "Y" ? this.FltVal("HUD55") : 0.0) + (this.Val("NEWHUD.X1726") == "Y" ? this.FltVal("HUD56") : 0.0) + (this.Val("NEWHUD.X340") == "Y" ? this.FltVal("HUD58") : 0.0) + (this.Val("NEWHUD.X341") == "Y" ? this.FltVal("HUD60") : 0.0);
              double num62 = this.Val("NEWHUD.X342") == "Y" ? this.FltVal("HUD62") : 0.0;
              num60 = num59 = num61 + num62;
              if (this.calObjs.NewHud2015Cal.UseNewCompliance(18.3M))
                num59 += (this.Val("NEWHUD.X1728") == "Y" ? this.FltVal("HUD54") : 0.0) + (this.Val("NEWHUD.X343") == "Y" ? this.FltVal("HUD63") : 0.0);
            }
            else
            {
              double num63 = (this.Val("NEWHUD.X339") == "Y" ? this.FltVal("230") : 0.0) + (this.Val("NEWHUD.X337") == "Y" ? this.FltVal("231") : 0.0) + (this.Val("NEWHUD.X1726") == "Y" ? this.FltVal("L268") : 0.0) + (this.Val("NEWHUD.X338") == "Y" ? this.FltVal("235") : 0.0) + (this.Val("NEWHUD.X340") == "Y" ? this.FltVal("1630") : 0.0) + (this.Val("NEWHUD.X341") == "Y" ? this.FltVal("253") : 0.0);
              double num64 = this.Val("NEWHUD.X342") == "Y" ? this.FltVal("254") : 0.0;
              num60 = num59 = num63 + num64;
              if (this.calObjs.NewHud2015Cal.UseNewCompliance(18.3M))
                num59 += (this.Val("NEWHUD.X1728") == "Y" ? this.FltVal("232") : 0.0) + (this.Val("NEWHUD.X343") == "Y" ? this.FltVal("NEWHUD.X1707") : 0.0);
            }
            if (str2 == "ConstructionToPermanent" && this.Val("HUD68") != this.Val("682") && num16 >= 12)
              this.SetVal("CD4.X10", string.Empty);
            else
              this.SetCurrentNum("CD4.X10", num59, true);
            if (!flag2)
            {
              if (num60 > 0.0)
              {
                int num65 = 0;
                if (str2 == "ConstructionToPermanent" && monthlyPayments1 != null && monthlyPayments1[0] != null)
                {
                  DateTime date1 = Utils.ParseDate((object) monthlyPayments1[0].PayDate);
                  DateTime date2 = Utils.ParseDate((object) this.Val("HUD68"));
                  for (int index = 0; index < num26; ++index)
                  {
                    DateTime dateTime = date1.AddMonths(this.IntVal("CD1.X" + (object) (7 + index * 9)) * 12 - 1);
                    if (date2 <= dateTime)
                    {
                      num65 = index;
                      break;
                    }
                  }
                }
                this.SetVal("CD1.X43", "Y");
                for (int index = 0; index < num26; ++index)
                {
                  if (index < num65)
                    this.SetVal("CD1.X" + (object) (12 + index * 9), "0");
                  else
                    this.SetCurrentNum("CD1.X" + (object) (12 + index * 9), num60);
                }
              }
              else
              {
                for (int index = 0; index < num26; ++index)
                  this.SetVal("CD1.X" + (object) (12 + index * 9), "0");
              }
            }
            else
              this.SetVal("CD1.X43", this.FltVal("CD1.X12") > 0.0 || num26 > 1 && this.FltVal("CD1.X21") > 0.0 || num26 > 2 && this.FltVal("CD1.X30") > 0.0 || num26 > 3 && this.FltVal("CD1.X39") > 0.0 ? "Y" : "N");
            if (flag6 && !flag2)
            {
              this.SetCurrentNum("CD1.X" + (object) (11 + (num26 - 1) * 9), 0.0);
              this.SetCurrentNum("CD1.X" + (object) (12 + (num26 - 1) * 9), 0.0);
            }
            if (!flag2)
            {
              for (int index = 0; index < num26; ++index)
              {
                double num66 = this.FltVal("CD1.X" + (object) (8 + index * 9));
                double num67 = this.FltVal("CD1.X" + (object) (9 + index * 9));
                if (num66 != num67)
                {
                  this.SetCurrentNum("CD1.X" + (object) (8 + index * 9), Utils.ArithmeticRounding(num66, 0), this.UseNoPayment(num66));
                  this.SetCurrentNum("CD1.X" + (object) (9 + index * 9), Utils.ArithmeticRounding(num67, 0), this.UseNoPayment(num67));
                }
                double num68 = this.FltVal("CD1.X" + (object) (8 + index * 9)) + this.FltVal("CD1.X" + (object) (11 + index * 9)) + this.FltVal("CD1.X" + (object) (12 + index * 9));
                this.SetCurrentNum("CD1.X" + (object) (13 + index * 9), num66 != num67 ? Utils.ArithmeticRounding(num68, 0) : num68, this.UseNoPayment(num68));
                double num69 = this.FltVal("CD1.X" + (object) (9 + index * 9)) + this.FltVal("CD1.X" + (object) (11 + index * 9)) + this.FltVal("CD1.X" + (object) (12 + index * 9));
                this.SetCurrentNum("CD1.X" + (object) (14 + index * 9), num66 != num67 ? Utils.ArithmeticRounding(num69, 0) : num69, this.UseNoPayment(num69));
              }
            }
            this.SetVal("CD1.X42", this.FltVal("CD1.X11") > 0.0 || num26 > 1 && this.FltVal("CD1.X20") > 0.0 || num26 > 2 && this.FltVal("CD1.X29") > 0.0 || num26 > 3 && this.FltVal("CD1.X38") > 0.0 ? "Y" : "N");
            if (!flag2)
              this.SetVal("CD1.X10", num15 > 0 || str2 == "ConstructionOnly" || str2 == "ConstructionToPermanent" ? "Y" : "N");
            double num70 = (this.Val("NEWHUD2.X133") == "Y" ? this.FltVal("230") : 0.0) + (this.Val("NEWHUD2.X134") == "Y" ? this.FltVal("231") : 0.0) + (this.Val("NEWHUD2.X135") == "Y" ? this.FltVal("L268") : 0.0) + (this.Val("NEWHUD2.X136") == "Y" ? this.FltVal("235") : 0.0) + (this.Val("NEWHUD2.X137") == "Y" ? this.FltVal("1630") : 0.0) + (this.Val("NEWHUD2.X138") == "Y" ? this.FltVal("253") : 0.0) + (this.Val("NEWHUD2.X139") == "Y" ? this.FltVal("254") : 0.0);
            if (num6 == 26 && num70 != 0.0)
              num70 = Utils.ArithmeticRounding(num70 * 12.0 / (double) num6, 2);
            if (flag2)
              this.SetVal("LE1.X77", this.FltVal("CD1.X12") != 0.0 ? "Y" : "N");
            else
              this.SetVal("LE1.X77", num70 != 0.0 ? "Y" : "N");
            for (int index = 0; index < num26; ++index)
            {
              if (index > 0)
                this.SetVal("LE1.X" + (object) (40 + index * 9), this.Val("CD1.X" + (object) (6 + index * 9)));
              this.SetVal("LE1.X" + (object) (41 + index * 9), this.Val("CD1.X" + (object) (7 + index * 9)));
              double num71 = this.FltVal("CD1.X" + (object) (8 + index * 9));
              double num72 = this.FltVal("CD1.X" + (object) (9 + index * 9));
              if (num71 != num72)
              {
                num71 = Utils.ArithmeticRounding(num71, 0);
                num72 = Utils.ArithmeticRounding(num72, 0);
                this.setTempHash(xdFields, "LE1.XD" + (object) (index + 1), "Y");
                this.setTempHash(xdFields, "CD1.XD" + (object) (index + 1), "Y");
              }
              else if (!flag2)
              {
                this.setTempHash(xdFields, "LE1.XD" + (object) (index + 1), "N");
                this.setTempHash(xdFields, "CD1.XD" + (object) (index + 1), "N");
              }
              this.SetCurrentNum("LE1.X" + (object) (42 + index * 9), num71, this.UseNoPayment(num71));
              this.SetCurrentNum("LE1.X" + (object) (43 + index * 9), num72, this.UseNoPayment(num72));
              this.SetVal("LE1.X" + (object) (44 + index * 9), this.Val("CD1.X" + (object) (10 + index * 9)));
              double num73 = Utils.ArithmeticRounding(this.FltVal("CD1.X" + (object) (11 + index * 9)), 0);
              if (flag2)
              {
                if (num26 > 1 && index == num26 - 1 && this.Val("CD1.X" + (object) (15 + (num26 - 2) * 9)) == "")
                  this.SetCurrentNum("LE1.X" + (object) (45 + index * 9), 0.0);
                else
                  this.SetCurrentNum("LE1.X" + (object) (45 + index * 9), num73);
              }
              else if (flag6 && index == num26 - 1)
                this.SetCurrentNum("LE1.X" + (object) (45 + index * 9), 0.0);
              else
                this.SetCurrentNum("LE1.X" + (object) (45 + index * 9), num73);
              double num74 = Utils.ArithmeticRounding(this.FltVal("CD1.X" + (object) (12 + index * 9)), 0);
              if (flag2)
              {
                if (num26 > 1 && index == num26 - 1 && this.Val("CD1.X" + (object) (15 + (num26 - 2) * 9)) == "")
                  this.SetCurrentNum("LE1.X" + (object) (46 + index * 9), 0.0);
                else
                  this.SetCurrentNum("LE1.X" + (object) (46 + index * 9), num74);
              }
              else if (flag6 && index == num26 - 1)
                this.SetCurrentNum("LE1.X" + (object) (46 + index * 9), 0.0);
              else if (this.FltVal("CD1.X" + (object) (12 + index * 9)) == 0.0)
                this.SetVal("LE1.X" + (object) (46 + index * 9), "0");
              else
                this.SetCurrentNum("LE1.X" + (object) (46 + index * 9), num74);
              double val4 = !flag2 ? (!flag6 || index != num26 - 1 ? this.FltVal("LE1.X" + (object) (42 + index * 9)) + num73 + Utils.ArithmeticRounding(num74, 0) : this.FltVal("LE1.X" + (object) (42 + index * 9))) : this.FltVal("LE1.X" + (object) (42 + index * 9)) + num73 + this.FltVal("LE1.X" + (object) (46 + index * 9));
              double num75 = num71 != num72 || (num73 > 0.0 || num74 > 0.0) && (!flag6 || index != num26 - 1) ? Utils.ArithmeticRounding(val4, 0) : Utils.ArithmeticRounding(val4, 2);
              this.SetCurrentNum("LE1.X" + (object) (47 + index * 9), num75, this.UseNoPayment(num75));
              double val5 = !flag2 ? (!flag6 || index != num26 - 1 ? this.FltVal("LE1.X" + (object) (43 + index * 9)) + num73 + Utils.ArithmeticRounding(num74, 0) : this.FltVal("LE1.X" + (object) (43 + index * 9))) : this.FltVal("LE1.X" + (object) (43 + index * 9)) + num73 + this.FltVal("LE1.X" + (object) (46 + index * 9));
              double num76 = num71 != num72 || (num73 > 0.0 || num74 > 0.0) && (!flag6 || index != num26 - 1) ? Utils.ArithmeticRounding(val5, 0) : Utils.ArithmeticRounding(val5, 2);
              this.SetCurrentNum("LE1.X" + (object) (48 + index * 9), num76, this.UseNoPayment(num76));
            }
            performanceMeter.AddCheckpoint("Set Projected Payment Table Fields", 3359, nameof (CalculateProjectedPaymentTable), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\CalculationEngine\\NewHUDCalculation.cs");
            if (this.FltVal("LE1.X43") != this.FltVal("LE1.X42"))
            {
              this.setTempHash(xdFields, "LE1.XD42", Utils.ApplyFieldFormatting(Utils.RemoveEndingZeros(this.Val("LE1.X42")), FieldFormat.INTEGER));
              this.setTempHash(xdFields, "LE1.XD43", Utils.ApplyFieldFormatting(Utils.RemoveEndingZeros(this.Val("LE1.X43")), FieldFormat.INTEGER));
              this.setTempHash(xdFields, "LE1.XD47", Utils.ApplyFieldFormatting(this.Val("LE1.X47"), FieldFormat.INTEGER));
              this.setTempHash(xdFields, "LE1.XD48", Utils.ApplyFieldFormatting(this.Val("LE1.X48"), FieldFormat.INTEGER));
            }
            else
            {
              this.setTempHash(xdFields, "LE1.XD42", Utils.ApplyFieldFormatting(this.Val("LE1.X42"), FieldFormat.DECIMAL_2));
              this.setTempHash(xdFields, "LE1.XD43", Utils.ApplyFieldFormatting(this.Val("LE1.X43"), FieldFormat.DECIMAL_2));
              if (this.FltVal("LE1.X45") > 0.0 || this.FltVal("LE1.X46") > 0.0)
              {
                this.setTempHash(xdFields, "LE1.XD47", Utils.ApplyFieldFormatting(this.Val("LE1.X47"), FieldFormat.INTEGER));
                this.setTempHash(xdFields, "LE1.XD48", Utils.ApplyFieldFormatting(this.Val("LE1.X48"), FieldFormat.INTEGER));
              }
              else
              {
                this.setTempHash(xdFields, "LE1.XD47", Utils.ApplyFieldFormatting(this.Val("LE1.X47"), FieldFormat.DECIMAL_2));
                this.setTempHash(xdFields, "LE1.XD48", Utils.ApplyFieldFormatting(this.Val("LE1.X48"), FieldFormat.DECIMAL_2));
              }
            }
            if (this.FltVal("LE1.X52") != this.FltVal("LE1.X51"))
            {
              this.setTempHash(xdFields, "LE1.XD51", Utils.ApplyFieldFormatting(Utils.RemoveEndingZeros(this.Val("LE1.X51")), FieldFormat.INTEGER));
              this.setTempHash(xdFields, "LE1.XD52", Utils.ApplyFieldFormatting(Utils.RemoveEndingZeros(this.Val("LE1.X52")), FieldFormat.INTEGER));
              this.setTempHash(xdFields, "LE1.XD56", Utils.ApplyFieldFormatting(this.Val("LE1.X56"), FieldFormat.INTEGER));
              this.setTempHash(xdFields, "LE1.XD57", Utils.ApplyFieldFormatting(this.Val("LE1.X57"), FieldFormat.INTEGER));
            }
            else
            {
              this.setTempHash(xdFields, "LE1.XD51", Utils.ApplyFieldFormatting(this.Val("LE1.X51"), FieldFormat.DECIMAL_2));
              this.setTempHash(xdFields, "LE1.XD52", Utils.ApplyFieldFormatting(this.Val("LE1.X52"), FieldFormat.DECIMAL_2));
              if (this.FltVal("LE1.X54") > 0.0 || this.FltVal("LE1.X55") > 0.0)
              {
                this.setTempHash(xdFields, "LE1.XD56", Utils.ApplyFieldFormatting(this.Val("LE1.X56"), FieldFormat.INTEGER));
                this.setTempHash(xdFields, "LE1.XD57", Utils.ApplyFieldFormatting(this.Val("LE1.X57"), FieldFormat.INTEGER));
              }
              else
              {
                this.setTempHash(xdFields, "LE1.XD56", Utils.ApplyFieldFormatting(this.Val("LE1.X56"), FieldFormat.DECIMAL_2));
                this.setTempHash(xdFields, "LE1.XD57", Utils.ApplyFieldFormatting(this.Val("LE1.X57"), FieldFormat.DECIMAL_2));
              }
            }
            if (this.FltVal("LE1.X60") != this.FltVal("LE1.X61"))
            {
              this.setTempHash(xdFields, "LE1.XD60", Utils.ApplyFieldFormatting(Utils.RemoveEndingZeros(this.Val("LE1.X60")), FieldFormat.INTEGER));
              this.setTempHash(xdFields, "LE1.XD61", Utils.ApplyFieldFormatting(Utils.RemoveEndingZeros(this.Val("LE1.X61")), FieldFormat.INTEGER));
              this.setTempHash(xdFields, "LE1.XD65", Utils.ApplyFieldFormatting(this.Val("LE1.X65"), FieldFormat.INTEGER));
              this.setTempHash(xdFields, "LE1.XD66", Utils.ApplyFieldFormatting(this.Val("LE1.X66"), FieldFormat.INTEGER));
            }
            else
            {
              this.setTempHash(xdFields, "LE1.XD60", Utils.ApplyFieldFormatting(this.Val("LE1.X60"), FieldFormat.DECIMAL_2));
              this.setTempHash(xdFields, "LE1.XD61", Utils.ApplyFieldFormatting(this.Val("LE1.X61"), FieldFormat.DECIMAL_2));
              if (this.FltVal("LE1.X63") > 0.0 || this.FltVal("LE1.X64") > 0.0)
              {
                this.setTempHash(xdFields, "LE1.XD65", Utils.ApplyFieldFormatting(this.Val("LE1.X65"), FieldFormat.INTEGER));
                this.setTempHash(xdFields, "LE1.XD66", Utils.ApplyFieldFormatting(this.Val("LE1.X66"), FieldFormat.INTEGER));
              }
              else
              {
                this.setTempHash(xdFields, "LE1.XD65", Utils.ApplyFieldFormatting(this.Val("LE1.X65"), FieldFormat.DECIMAL_2));
                this.setTempHash(xdFields, "LE1.XD66", Utils.ApplyFieldFormatting(this.Val("LE1.X66"), FieldFormat.DECIMAL_2));
              }
            }
            if (this.FltVal("LE1.X69") != this.FltVal("LE1.X70"))
            {
              this.setTempHash(xdFields, "LE1.XD69", Utils.ApplyFieldFormatting(Utils.RemoveEndingZeros(this.Val("LE1.X69")), FieldFormat.INTEGER));
              this.setTempHash(xdFields, "LE1.XD70", Utils.ApplyFieldFormatting(Utils.RemoveEndingZeros(this.Val("LE1.X70")), FieldFormat.INTEGER));
              this.setTempHash(xdFields, "LE1.XD74", Utils.ApplyFieldFormatting(this.Val("LE1.X74"), FieldFormat.INTEGER));
              this.setTempHash(xdFields, "LE1.XD75", Utils.ApplyFieldFormatting(this.Val("LE1.X75"), FieldFormat.INTEGER));
            }
            else
            {
              this.setTempHash(xdFields, "LE1.XD69", Utils.ApplyFieldFormatting(this.Val("LE1.X69"), FieldFormat.DECIMAL_2));
              this.setTempHash(xdFields, "LE1.XD70", Utils.ApplyFieldFormatting(this.Val("LE1.X70"), FieldFormat.DECIMAL_2));
              if (this.FltVal("LE1.X72") > 0.0 || this.FltVal("LE1.X73") > 0.0)
              {
                this.setTempHash(xdFields, "LE1.XD74", Utils.ApplyFieldFormatting(this.Val("LE1.X74"), FieldFormat.INTEGER));
                this.setTempHash(xdFields, "LE1.XD75", Utils.ApplyFieldFormatting(this.Val("LE1.X75"), FieldFormat.INTEGER));
              }
              else
              {
                this.setTempHash(xdFields, "LE1.XD74", Utils.ApplyFieldFormatting(this.Val("LE1.X74"), FieldFormat.DECIMAL_2));
                this.setTempHash(xdFields, "LE1.XD75", Utils.ApplyFieldFormatting(this.Val("LE1.X75"), FieldFormat.DECIMAL_2));
              }
            }
            if (str2 != "ConstructionToPermanent" && this.FltVal("LE1.X45") == 0.0 || str2 == "ConstructionToPermanent" && this.FltVal("LE1.X45") == 0.0 && this.FltVal("LE1.X54") == 0.0)
            {
              this.setTempHash(xdFields, "LE1.XD45", "0");
              this.setTempHash(xdFields, "CD1.XD11", "0");
              if (num26 > 1)
              {
                this.setTempHash(xdFields, "LE1.XD54", flag2 ? (this.FltVal("LE1.X54") == 0.0 ? "0" : this.Val("LE1.X54")) : "0");
                this.setTempHash(xdFields, "CD1.XD20", flag2 ? (this.FltVal("CD1.X20") == 0.0 ? "0" : this.Val("CD1.X20")) : "0");
              }
              if (num26 > 2)
              {
                this.setTempHash(xdFields, "LE1.XD63", flag2 ? (this.FltVal("LE1.X63") == 0.0 ? "0" : this.Val("LE1.X63")) : "0");
                this.setTempHash(xdFields, "CD1.XD29", flag2 ? (this.FltVal("CD1.X29") == 0.0 ? (this.FltVal("CD1.X20") != 0.0 ? "-" : "0") : this.Val("CD1.X29")) : "0");
              }
              if (num26 > 3)
              {
                this.setTempHash(xdFields, "LE1.XD72", flag2 ? (this.FltVal("LE1.X72") == 0.0 ? "0" : this.Val("LE1.X72")) : "0");
                this.setTempHash(xdFields, "CD1.XD38", flag2 ? (this.FltVal("CD1.X38") == 0.0 ? (this.FltVal("CD1.X29") != 0.0 ? "-" : "0") : this.Val("CD1.X38")) : "0");
              }
              if (flag2)
              {
                for (int index = 0; index < num26; ++index)
                {
                  if (this.FltVal("LE1.X" + (object) (45 + index * 9)) != 0.0 && this.FltVal("LE1.X" + (object) (45 + (num26 - 1) * 9)) == 0.0)
                  {
                    this.setTempHash(xdFields, "LE1.XD" + (object) (45 + (num26 - 1) * 9), "-");
                    this.setTempHash(xdFields, "CD1.XD" + (object) (11 + (num26 - 1) * 9), "-");
                    break;
                  }
                }
              }
            }
            else
            {
              this.setTempHash(xdFields, "LE1.XD45", this.FltVal("LE1.X45") == 0.0 ? "0" : Utils.ApplyFieldFormatting(this.Val("LE1.X45"), FieldFormat.INTEGER));
              this.setTempHash(xdFields, "CD1.XD11", this.FltVal("CD1.X11") == 0.0 ? "0" : this.Val("CD1.X11"));
              if (num26 > 1)
              {
                this.setTempHash(xdFields, "LE1.XD54", this.FltVal("LE1.X54") == 0.0 ? "-" : this.Val("LE1.X54"));
                this.setTempHash(xdFields, "CD1.XD20", this.FltVal("CD1.X20") == 0.0 ? "-" : this.Val("CD1.X20"));
              }
              if (num26 > 2)
              {
                this.setTempHash(xdFields, "LE1.XD63", this.FltVal("LE1.X63") == 0.0 ? "-" : this.Val("LE1.X63"));
                this.setTempHash(xdFields, "CD1.XD29", this.FltVal("CD1.X29") == 0.0 ? "-" : this.Val("CD1.X29"));
              }
              if (num26 > 3)
              {
                this.setTempHash(xdFields, "LE1.XD72", this.FltVal("LE1.X72") == 0.0 ? "-" : this.Val("LE1.X72"));
                this.setTempHash(xdFields, "CD1.XD38", this.FltVal("CD1.X38") == 0.0 ? "-" : this.Val("CD1.X38"));
              }
            }
            if (flag6 && num26 > 0)
            {
              this.setTempHash(xdFields, "LE1.XD" + (object) (45 + (num26 - 1) * 9), this.FltVal("LE1.X45") != 0.0 || this.FltVal("LE1.X54") != 0.0 ? "-" : "0");
              this.setTempHash(xdFields, "CD1.XD" + (object) (11 + (num26 - 1) * 9), this.FltVal("LE1.X45") != 0.0 || this.FltVal("LE1.X54") != 0.0 ? "-" : "0");
            }
            if (str2 != "ConstructionToPermanent" && this.FltVal("LE1.X46") == 0.0 || str2 == "ConstructionToPermanent" && this.FltVal("LE1.X46") == 0.0 && this.FltVal("LE1.X55") == 0.0)
            {
              if (str2 == "ConstructionToPermanent" && this.Val("HUD69") == "FirstAmortDate" && num16 % 12 == 0)
              {
                if (this.Val("LE1.X30") != "No" || this.Val("LE1.X31") != "No" || this.Val("LE1.X32") != "No" || this.Val("CD1.X4") != "No" || this.Val("CD1.X5") != "No" || this.Val("CD1.X6") != "No")
                {
                  this.setTempHash(xdFields, "CD1.XD12", "-");
                  this.setTempHash(xdFields, "LE1.XD46", "-");
                }
                else
                {
                  this.setTempHash(xdFields, "LE1.XD46", "0");
                  this.setTempHash(xdFields, "CD1.XD12", "0");
                }
              }
              else
              {
                this.setTempHash(xdFields, "LE1.XD46", "0");
                this.setTempHash(xdFields, "CD1.XD12", "0");
              }
              if (num26 > 1)
              {
                this.setTempHash(xdFields, "LE1.XD55", flag2 ? Utils.ApplyFieldFormatting(string.Concat((object) Utils.ParseDecimal((object) this.Val("LE1.X55"))), FieldFormat.INTEGER) : "0");
                this.setTempHash(xdFields, "CD1.XD21", flag2 ? (this.FltVal("CD1.X21") == 0.0 ? "0" : this.Val("CD1.X21")) : "0");
              }
              if (num26 > 2)
              {
                this.setTempHash(xdFields, "LE1.XD64", flag2 ? Utils.ApplyFieldFormatting(string.Concat((object) Utils.ParseDecimal((object) this.Val("LE1.X64"))), FieldFormat.INTEGER) : "0");
                this.setTempHash(xdFields, "CD1.XD30", flag2 ? (this.FltVal("CD1.X30") == 0.0 ? (this.FltVal("CD1.X21") != 0.0 ? "-" : "0") : this.Val("CD1.X30")) : "0");
              }
              if (num26 > 3)
              {
                this.setTempHash(xdFields, "LE1.XD73", flag2 ? Utils.ApplyFieldFormatting(string.Concat((object) Utils.ParseDecimal((object) this.Val("LE1.X73"))), FieldFormat.INTEGER) : "0");
                this.setTempHash(xdFields, "CD1.XD39", flag2 ? (this.FltVal("CD1.X39") == 0.0 ? (this.FltVal("CD1.X30") != 0.0 ? "-" : "0") : this.Val("CD1.X39")) : "0");
              }
              if (flag2)
              {
                for (int index = 0; index < num26; ++index)
                {
                  if (this.FltVal("LE1.X" + (object) (46 + index * 9)) != 0.0 && this.FltVal("LE1.X" + (object) (46 + (num26 - 1) * 9)) == 0.0)
                  {
                    this.setTempHash(xdFields, "LE1.XD" + (object) (46 + (num26 - 1) * 9), "-");
                    this.setTempHash(xdFields, "CD1.XD" + (object) (12 + (num26 - 1) * 9), "-");
                    break;
                  }
                }
              }
            }
            else
            {
              bool flag9 = str2 == "ConstructionToPermanent" && this.Val("HUD69") == "FirstAmortDate";
              if (str2 == "ConstructionToPermanent" && this.Val("HUD69") == "FirstAmortDate" && num16 % 12 == 0)
              {
                if (this.Val("LE1.X30") != "No" || this.Val("LE1.X31") != "No" || this.Val("LE1.X32") != "No" || this.Val("CD1.X4") != "No" || this.Val("CD1.X5") != "No" || this.Val("CD1.X6") != "No")
                {
                  this.setTempHash(xdFields, "CD1.XD12", "-");
                  this.setTempHash(xdFields, "LE1.XD46", "-");
                }
                else
                {
                  this.setTempHash(xdFields, "LE1.XD46", "0");
                  this.setTempHash(xdFields, "CD1.XD12", "0");
                }
              }
              else
              {
                this.setTempHash(xdFields, "LE1.XD46", Utils.ApplyFieldFormatting(this.Val("LE1.X46"), FieldFormat.INTEGER));
                this.setTempHash(xdFields, "CD1.XD12", this.Val("CD1.X12") == "0.00" ? "0" : this.Val("CD1.X12"));
              }
              if (num26 > 1)
              {
                if (flag9)
                {
                  this.setTempHash(xdFields, "LE1.XD55", flag2 ? Utils.ApplyFieldFormatting(this.Val("LE1.X55"), FieldFormat.INTEGER) : Utils.ApplyFieldFormatting(this.Val("LE1.X55"), FieldFormat.INTEGER));
                  this.setTempHash(xdFields, "CD1.XD21", flag2 ? this.Val("CD1.X21") : (this.Val("CD1.X21") == "0.00" ? "0" : this.Val("CD1.X21")));
                }
                else
                {
                  this.setTempHash(xdFields, "LE1.XD55", flag2 ? Utils.ApplyFieldFormatting(this.Val("LE1.X55"), FieldFormat.INTEGER) : Utils.ApplyFieldFormatting(this.Val("LE1.X46"), FieldFormat.INTEGER));
                  this.setTempHash(xdFields, "CD1.XD21", flag2 ? this.Val("CD1.X21") : (this.Val("CD1.X12") == "0.00" ? "0" : this.Val("CD1.X12")));
                }
              }
              if (num26 > 2)
              {
                if (flag9)
                {
                  this.setTempHash(xdFields, "LE1.XD64", flag2 ? Utils.ApplyFieldFormatting(this.Val("LE1.X64"), FieldFormat.INTEGER) : Utils.ApplyFieldFormatting(this.Val("LE1.X55"), FieldFormat.INTEGER));
                  this.setTempHash(xdFields, "CD1.XD30", flag2 ? this.Val("CD1.X30") : (this.Val("CD1.X21") == "0.00" ? "0" : this.Val("CD1.X21")));
                }
                else
                {
                  this.setTempHash(xdFields, "LE1.XD64", flag2 ? Utils.ApplyFieldFormatting(this.Val("LE1.X64"), FieldFormat.INTEGER) : Utils.ApplyFieldFormatting(this.Val("LE1.X46"), FieldFormat.INTEGER));
                  this.setTempHash(xdFields, "CD1.XD30", flag2 ? this.Val("CD1.X30") : (this.Val("CD1.X12") == "0.00" ? "0" : this.Val("CD1.X12")));
                }
              }
              if (num26 > 3)
              {
                if (flag9)
                {
                  this.setTempHash(xdFields, "LE1.XD73", flag2 ? Utils.ApplyFieldFormatting(this.Val("LE1.X73"), FieldFormat.INTEGER) : Utils.ApplyFieldFormatting(this.Val("LE1.X55"), FieldFormat.INTEGER));
                  this.setTempHash(xdFields, "CD1.XD39", flag2 ? this.Val("CD1.X39") : (this.Val("CD1.X21") == "0.00" ? "0" : this.Val("CD1.X21")));
                }
                else
                {
                  this.setTempHash(xdFields, "LE1.XD73", flag2 ? Utils.ApplyFieldFormatting(this.Val("LE1.X73"), FieldFormat.INTEGER) : Utils.ApplyFieldFormatting(this.Val("LE1.X46"), FieldFormat.INTEGER));
                  this.setTempHash(xdFields, "CD1.XD39", flag2 ? this.Val("CD1.X39") : (this.Val("CD1.X12") == "0.00" ? "0" : this.Val("CD1.X12")));
                }
              }
            }
            if (flag6 && num26 > 0)
            {
              this.setTempHash(xdFields, "LE1.XD" + (object) (46 + (num26 - 1) * 9), this.FltVal("LE1.X46") != 0.0 || this.FltVal("LE1.X55") != 0.0 ? "-" : "0");
              this.setTempHash(xdFields, "CD1.XD" + (object) (12 + (num26 - 1) * 9), this.FltVal("LE1.X46") != 0.0 || this.FltVal("LE1.X55") != 0.0 ? "-" : "0");
            }
            if (num26 > 0)
            {
              if (this.FltVal("CD1.X8") != this.FltVal("CD1.X9"))
              {
                this.setTempHash(xdFields, "CD1.XD8", Utils.ApplyFieldFormatting(this.Val("CD1.X8"), FieldFormat.INTEGER));
                this.setTempHash(xdFields, "CD1.XD9", Utils.ApplyFieldFormatting(this.Val("CD1.X9"), FieldFormat.INTEGER));
                this.setTempHash(xdFields, "CD1.XD13", Utils.ApplyFieldFormatting(this.Val("CD1.X13"), FieldFormat.INTEGER));
                this.setTempHash(xdFields, "CD1.XD14", Utils.ApplyFieldFormatting(this.Val("CD1.X14"), FieldFormat.INTEGER));
              }
              else
              {
                this.setTempHash(xdFields, "CD1.XD8", Utils.ApplyFieldFormatting(this.Val("CD1.X8"), FieldFormat.DECIMAL_2));
                this.setTempHash(xdFields, "CD1.XD9", Utils.ApplyFieldFormatting(this.Val("CD1.X9"), FieldFormat.DECIMAL_2));
                this.setTempHash(xdFields, "CD1.XD13", Utils.ApplyFieldFormatting(this.Val("CD1.X13"), FieldFormat.DECIMAL_2));
                this.setTempHash(xdFields, "CD1.XD14", Utils.ApplyFieldFormatting(this.Val("CD1.X14"), FieldFormat.DECIMAL_2));
              }
            }
            if (num26 > 1)
            {
              if (this.FltVal("LE1.X51") == this.FltVal("LE1.X52") && this.FltVal("LE1.X54") == 0.0 && this.FltVal("LE1.X55") == 0.0)
              {
                this.setTempHash(xdFields, "LE1.XD56", Utils.ApplyFieldFormatting(this.Val("LE1.X56"), FieldFormat.DECIMAL_2));
                this.setTempHash(xdFields, "LE1.XD57", Utils.ApplyFieldFormatting(this.Val("LE1.X57"), FieldFormat.DECIMAL_2));
              }
              else
              {
                this.setTempHash(xdFields, "LE1.XD56", Utils.ApplyFieldFormatting(this.Val("LE1.X56"), FieldFormat.INTEGER));
                this.setTempHash(xdFields, "LE1.XD57", Utils.ApplyFieldFormatting(this.Val("LE1.X57"), FieldFormat.INTEGER));
              }
              if (this.FltVal("CD1.X17") != this.FltVal("CD1.X18"))
              {
                this.setTempHash(xdFields, "CD1.XD17", Utils.ApplyFieldFormatting(this.Val("CD1.X17"), FieldFormat.INTEGER));
                this.setTempHash(xdFields, "CD1.XD18", Utils.ApplyFieldFormatting(this.Val("CD1.X18"), FieldFormat.INTEGER));
                this.setTempHash(xdFields, "CD1.XD22", Utils.ApplyFieldFormatting(this.Val("CD1.X22"), FieldFormat.INTEGER));
                this.setTempHash(xdFields, "CD1.XD23", Utils.ApplyFieldFormatting(this.Val("CD1.X23"), FieldFormat.INTEGER));
              }
              else
              {
                this.setTempHash(xdFields, "CD1.XD17", Utils.ApplyFieldFormatting(this.Val("CD1.X17"), FieldFormat.DECIMAL_2));
                this.setTempHash(xdFields, "CD1.XD18", Utils.ApplyFieldFormatting(this.Val("CD1.X18"), FieldFormat.DECIMAL_2));
                this.setTempHash(xdFields, "CD1.XD22", Utils.ApplyFieldFormatting(this.Val("CD1.X22"), FieldFormat.DECIMAL_2));
                this.setTempHash(xdFields, "CD1.XD23", Utils.ApplyFieldFormatting(this.Val("CD1.X23"), FieldFormat.DECIMAL_2));
              }
            }
            if (num26 > 2)
            {
              if (this.FltVal("LE1.X60") == this.FltVal("LE1.X61") && this.FltVal("LE1.X63") == 0.0 && this.FltVal("LE1.X64") == 0.0)
              {
                this.setTempHash(xdFields, "LE1.XD65", Utils.ApplyFieldFormatting(this.Val("LE1.X65"), FieldFormat.DECIMAL_2));
                this.setTempHash(xdFields, "LE1.XD66", Utils.ApplyFieldFormatting(this.Val("LE1.X66"), FieldFormat.DECIMAL_2));
              }
              else
              {
                this.setTempHash(xdFields, "LE1.XD65", Utils.ApplyFieldFormatting(this.Val("LE1.X65"), FieldFormat.INTEGER));
                this.setTempHash(xdFields, "LE1.XD66", Utils.ApplyFieldFormatting(this.Val("LE1.X66"), FieldFormat.INTEGER));
              }
              if (this.FltVal("CD1.X26") != this.FltVal("CD1.X27"))
              {
                this.setTempHash(xdFields, "CD1.XD26", Utils.ApplyFieldFormatting(this.Val("CD1.X26"), FieldFormat.INTEGER));
                this.setTempHash(xdFields, "CD1.XD27", Utils.ApplyFieldFormatting(this.Val("CD1.X27"), FieldFormat.INTEGER));
                this.setTempHash(xdFields, "CD1.XD31", Utils.ApplyFieldFormatting(this.Val("CD1.X31"), FieldFormat.INTEGER));
                this.setTempHash(xdFields, "CD1.XD32", Utils.ApplyFieldFormatting(this.Val("CD1.X32"), FieldFormat.INTEGER));
              }
              else
              {
                this.setTempHash(xdFields, "CD1.XD26", Utils.ApplyFieldFormatting(this.Val("CD1.X26"), FieldFormat.DECIMAL_2));
                this.setTempHash(xdFields, "CD1.XD27", Utils.ApplyFieldFormatting(this.Val("CD1.X27"), FieldFormat.DECIMAL_2));
                this.setTempHash(xdFields, "CD1.XD31", Utils.ApplyFieldFormatting(this.Val("CD1.X31"), FieldFormat.DECIMAL_2));
                this.setTempHash(xdFields, "CD1.XD32", Utils.ApplyFieldFormatting(this.Val("CD1.X32"), FieldFormat.DECIMAL_2));
              }
            }
            if (num26 > 3)
            {
              if (this.FltVal("LE1.X69") == this.FltVal("LE1.X70") && this.FltVal("LE1.X72") == 0.0 && this.FltVal("LE1.X73") == 0.0)
              {
                this.setTempHash(xdFields, "LE1.XD74", Utils.ApplyFieldFormatting(this.Val("LE1.X74"), FieldFormat.DECIMAL_2));
                this.setTempHash(xdFields, "LE1.XD75", Utils.ApplyFieldFormatting(this.Val("LE1.X75"), FieldFormat.DECIMAL_2));
              }
              else
              {
                this.setTempHash(xdFields, "LE1.XD74", Utils.ApplyFieldFormatting(this.Val("LE1.X74"), FieldFormat.INTEGER));
                this.setTempHash(xdFields, "LE1.XD75", Utils.ApplyFieldFormatting(this.Val("LE1.X75"), FieldFormat.INTEGER));
              }
              if (this.FltVal("CD1.X35") != this.FltVal("CD1.X36"))
              {
                this.setTempHash(xdFields, "CD1.XD35", Utils.ApplyFieldFormatting(this.Val("CD1.X35"), FieldFormat.INTEGER));
                this.setTempHash(xdFields, "CD1.XD36", Utils.ApplyFieldFormatting(this.Val("CD1.X36"), FieldFormat.INTEGER));
                this.setTempHash(xdFields, "CD1.XD40", Utils.ApplyFieldFormatting(this.Val("CD1.X40"), FieldFormat.INTEGER));
                this.setTempHash(xdFields, "CD1.XD41", Utils.ApplyFieldFormatting(this.Val("CD1.X41"), FieldFormat.INTEGER));
              }
              else
              {
                this.setTempHash(xdFields, "CD1.XD35", Utils.ApplyFieldFormatting(this.Val("CD1.X35"), FieldFormat.DECIMAL_2));
                this.setTempHash(xdFields, "CD1.XD36", Utils.ApplyFieldFormatting(this.Val("CD1.X36"), FieldFormat.DECIMAL_2));
                this.setTempHash(xdFields, "CD1.XD40", Utils.ApplyFieldFormatting(this.Val("CD1.X40"), FieldFormat.DECIMAL_2));
                this.setTempHash(xdFields, "CD1.XD41", Utils.ApplyFieldFormatting(this.Val("CD1.X41"), FieldFormat.DECIMAL_2));
              }
            }
            foreach (DictionaryEntry dictionaryEntry in xdFields)
              this.SetVal(dictionaryEntry.Key.ToString(), dictionaryEntry.Value.ToString());
            performanceMeter.AddCheckpoint("Rounding Projected Payment Table", 3973, nameof (CalculateProjectedPaymentTable), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\CalculationEngine\\NewHUDCalculation.cs");
            double num77 = num10;
            double num78 = num11;
            if (str1 != "Fixed")
            {
              this.SetVal("CD4.X49", num13 > 0.0 ? Utils.RemoveEndingZeros(string.Concat((object) num13), true) : "");
              this.SetVal("CD4.X50", num14 > 0 ? string.Concat((object) num14) : "");
            }
            else
            {
              this.SetVal("CD4.X49", "");
              this.SetVal("CD4.X50", "");
            }
            if (flag3)
            {
              this.SetCurrentNum("CD4.X35", 0.0);
              this.SetCurrentNum("CD4.X36", 0.0);
              this.SetVal("CD4.X37", "");
              this.SetCurrentNum("CD4.X38", 0.0);
              this.SetVal("CD4.X39", "");
            }
            else
            {
              this.SetVal("CD4.X30", "");
              this.SetVal("CD4.X34", "");
              if (this.Val("CD4.X23") == "Y")
              {
                string val6;
                string val7;
                if (month2 > 0)
                {
                  int month3 = month2 + 1;
                  val6 = !(str1 == "AdjustableRate") ? month3.ToString() + Utils.GetMonthSuffix(month3) : (month1 > -1 ? month1.ToString() + Utils.GetMonthSuffix(month1) ?? "" : "");
                  val7 = !(str2 == "ConstructionToPermanent") || !(this.Val("SYS.X6") != "B") ? month3.ToString() + Utils.GetMonthSuffix(month3) : (this.IntVal("1176") + 1).ToString() + Utils.GetMonthSuffix(this.IntVal("1176") + 1);
                }
                else
                {
                  val6 = !(str1 == "AdjustableRate") ? month2.ToString() + Utils.GetMonthSuffix(month2) : (month1 > -1 ? month1.ToString() + Utils.GetMonthSuffix(month1) ?? "" : "");
                  val7 = !(str2 == "ConstructionToPermanent") || !(this.Val("SYS.X6") != "B") ? month2.ToString() + Utils.GetMonthSuffix(month2) : (this.IntVal("1176") + 1).ToString() + Utils.GetMonthSuffix(this.IntVal("1176") + 1);
                }
                if (num26 > 1)
                {
                  num77 = this.FltVal("CD1.X17");
                  num78 = this.FltVal("CD1.X18");
                  if (month2 > 0 && (month2 <= 11 || month2 % 12 == 0 || str1 != "Fixed"))
                  {
                    int num79 = month2 % 12 == 0 ? month2 : month2 + (12 - month2 % 12);
                    if (this.Val("CD4.X31") == "Y" || str1 != "AdjustableRate" || num25 >= month2)
                    {
                      if (num25 >= month2 && str1 == "AdjustableRate" && month2 > 0)
                      {
                        if (monthlyPayments1 != null && month2 <= paymentSchedule.ActualNumberOfTerm && monthlyPayments1[month2] != null && monthlyPayments2 != null && month2 <= scheduleSnapshot1.ActualNumberOfTerm && monthlyPayments2[month2] != null)
                        {
                          num77 = monthlyPayments2[month2].Interest + monthlyPayments2[month2].Principal;
                          num78 = monthlyPayments1[month2].Interest + monthlyPayments1[month2].Principal;
                        }
                      }
                      else if (num25 < month2 && str1 == "AdjustableRate" && month1 > 0)
                      {
                        if (monthlyPayments1 != null && month1 <= paymentSchedule.ActualNumberOfTerm && monthlyPayments1[month1 - 1] != null && monthlyPayments2 != null && month1 <= scheduleSnapshot1.ActualNumberOfTerm && monthlyPayments2[month1 - 1] != null)
                        {
                          num77 = monthlyPayments2[month1 - 1].Interest + monthlyPayments2[month1 - 1].Principal;
                          num78 = monthlyPayments1[month1 - 1].Interest + monthlyPayments1[month1 - 1].Principal;
                        }
                      }
                      else
                      {
                        for (int index = 0; index < num26; ++index)
                        {
                          int num80 = this.IntVal("CD1.X" + (object) (index * 9 + 7)) * 12;
                          if (num79 < num80)
                          {
                            num77 = this.FltVal("CD1.X" + (object) (index * 9 + 8));
                            num78 = this.FltVal("CD1.X" + (object) (index * 9 + 9));
                            break;
                          }
                        }
                      }
                    }
                    else if ((this.Val("CD4.X31") == "N" || this.Val("CD4.X31") == "") && num25 < month2 && str1 == "AdjustableRate")
                    {
                      int month4 = (!(str2 == "ConstructionToPermanent") || !(this.Val("SYS.X6") != "B") ? num25 : this.IntVal("1176")) + 1;
                      if (monthlyPayments1 != null && month4 <= paymentSchedule.ActualNumberOfTerm && monthlyPayments1[month4 - 1] != null && monthlyPayments2 != null && month4 <= scheduleSnapshot1.ActualNumberOfTerm && monthlyPayments2[month4 - 1] != null)
                      {
                        num77 = monthlyPayments2[month4 - 1].Interest + monthlyPayments2[month4 - 1].Principal;
                        num78 = monthlyPayments1[month4 - 1].Interest + monthlyPayments1[month4 - 1].Principal;
                        val7 = month4.ToString() + Utils.GetMonthSuffix(month4);
                      }
                    }
                  }
                }
                string s1 = num77 != double.MaxValue ? Utils.RemoveEndingZeros(string.Concat((object) num77), true) ?? "" : "0";
                string s2 = num78 != double.MinValue ? Utils.RemoveEndingZeros(string.Concat((object) num78), true) ?? "" : "0";
                if (str2.Contains("Construction") && this.Val("SYS.X6") != "B")
                {
                  int index = this.IntVal("1176");
                  if (str2 == "ConstructionToPermanent")
                  {
                    if (monthlyPayments2 != null && monthlyPayments2.Length > index && monthlyPayments2[index] != null)
                      s1 = Utils.RemoveEndingZeros(string.Concat((object) (monthlyPayments2[index].Interest + monthlyPayments2[index].Principal)), true) ?? "";
                    if (monthlyPayments1 != null && monthlyPayments1.Length > index && monthlyPayments1[index] != null)
                      s2 = Utils.RemoveEndingZeros(string.Concat((object) (monthlyPayments1[index].Interest + monthlyPayments1[index].Principal)), true) ?? "";
                  }
                  else if (monthlyPayments1 != null && index <= paymentSchedule.ActualNumberOfTerm && index > 0 && monthlyPayments1[index - 1] != null)
                    s2 = Utils.RemoveEndingZeros(string.Concat((object) (monthlyPayments1[index - 1].Interest + monthlyPayments1[index - 1].Principal)), true) ?? "";
                }
                string str5 = Utils.RemoveEndingZeros(Decimal.Parse(s1).ToString("N"), false);
                string str6 = Utils.RemoveEndingZeros(Decimal.Parse(s2).ToString("N"), false);
                string val8;
                if (str2.Contains("Construction") && this.Val("SYS.X6") == "A")
                  val8 = "1st Payment";
                else if (str2 == "ConstructionToPermanent" && str1 == "Fixed")
                {
                  val8 = "$" + str6 + " at " + val7 + " payment";
                }
                else
                {
                  int num81 = this.IntVal("4");
                  int num82 = this.IntVal("325");
                  int num83 = this.IntVal("1177");
                  if (!str2.Contains("Construction") && num83 > 0 && num81 > num82 && (num83 == num82 || num83 == num82 - 1))
                    val8 = "";
                  else if (str5 == str6)
                    val8 = "$" + str5 + " at " + val7 + " payment";
                  else
                    val8 = "$" + str5 + " - $" + str6 + " at " + val7 + " payment";
                }
                this.SetVal("CD4.X30", val8);
                string val9;
                if (str2 == "ConstructionOnly" && this.Val("SYS.X6") != "B")
                {
                  val9 = "$" + Utils.RemoveEndingZeros(Utils.ArithmeticRounding(paymentSchedule.F_le1x24_cd4x34, 0).ToString("N"), false) + " as early as the 1st Payment ";
                }
                else
                {
                  int num84 = this.IntVal("4");
                  int num85 = this.IntVal("325");
                  int num86 = this.IntVal("1177");
                  if (!str2.Contains("Construction") && num86 > 0 && num84 > num85 && (num86 == num85 || num86 == num85 - 1))
                    val9 = "";
                  else
                    val9 = "$" + Utils.RemoveEndingZeros(Decimal.Parse(num11 != double.MinValue ? Utils.RemoveEndingZeros(string.Concat((object) num11), true) ?? "" : "0").ToString("N"), false) + " starting at " + val6 + " payment";
                }
                this.SetVal("CD4.X34", val9);
                this.SetCurrentNum("CD4.X35", num77 != double.MaxValue ? num77 : 0.0);
                this.SetCurrentNum("CD4.X36", num78 != double.MinValue ? num78 : 0.0);
                this.SetVal("CD4.X37", val7);
                this.SetCurrentNum("CD4.X38", num11 != double.MinValue ? num11 : 0.0);
                this.SetVal("CD4.X39", val6);
              }
            }
            if (str2 == "ConstructionOnly" && this.Val("SYS.X6") != "B")
              this.SetVal("LE1.X24", string.Concat((object) Utils.ArithmeticRounding(paymentSchedule.F_le1x24_cd4x34, 0)));
            else
              this.SetVal("LE1.X24", string.Concat((object) Utils.ArithmeticRounding(val1, 0)));
            performanceMeter.AddCheckpoint("Set First Change Period", 4227, nameof (CalculateProjectedPaymentTable), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\CalculationEngine\\NewHUDCalculation.cs");
          }
          if (flag2 && this.FltVal("3") == 0.0)
          {
            int num87 = this.IntVal("NEWHUD2.XPJTCOLUMNS");
            if (num87 > 0)
            {
              for (int index8 = 0; index8 < num87; ++index8)
              {
                if (index8 > 0)
                  this.SetVal("LE1.X" + (object) (40 + index8 * 9), this.Val("CD1.X" + (object) (6 + index8 * 9)));
                this.SetVal("LE1.X" + (object) (41 + index8 * 9), this.Val("CD1.X" + (object) (7 + index8 * 9)));
                this.SetCurrentNum("LE1.X" + (object) (42 + index8 * 9), this.FltVal("CD1.X" + (object) (8 + index8 * 9)), true);
                this.SetCurrentNum("LE1.X" + (object) (43 + index8 * 9), this.FltVal("CD1.X" + (object) (9 + index8 * 9)), true);
                this.SetVal("LE1.X" + (object) (44 + index8 * 9), this.Val("CD1.X" + (object) (10 + index8 * 9)));
                this.SetCurrentNum("LE1.X" + (object) (45 + index8 * 9), this.FltVal("CD1.X" + (object) (11 + index8 * 9)), true);
                this.SetCurrentNum("LE1.X" + (object) (46 + index8 * 9), this.FltVal("CD1.X" + (object) (12 + index8 * 9)), true);
                this.SetCurrentNum("LE1.X" + (object) (47 + index8 * 9), this.FltVal("CD1.X" + (object) (13 + index8 * 9)), true);
                this.SetCurrentNum("LE1.X" + (object) (48 + index8 * 9), this.FltVal("CD1.X" + (object) (14 + index8 * 9)), true);
                for (int index9 = 8; index9 < 15; ++index9)
                {
                  if (index9 != 10)
                    this.SetCurrentNum("CD1.XD" + (object) (index9 + index8 * 9), this.FltVal("CD1.X" + (object) (index9 + index8 * 9)), true);
                }
                for (int index10 = 42; index10 < 49; ++index10)
                {
                  if (index10 != 44)
                    this.SetCurrentNum("LE1.XD" + (object) (index10 + index8 * 9), this.FltVal("LE1.X" + (object) (index10 + index8 * 9)), true);
                }
              }
            }
          }
          if (!(this.Val("608") == "AdjustableRate"))
            return;
          this.loan.Calculator.EditCalcOnDemandEnum(CalcOnDemandEnum.MaxPaymentSample, true);
        }
      }
    }

    private void calculateInterestRateChange(string id, string val)
    {
      PaymentScheduleSnapshot scheduleSnapshot = (PaymentScheduleSnapshot) null;
      try
      {
        bool calculationDiagnostic = EnConfigurationSettings.GlobalSettings.AllowCalculationDiagnostic;
        if (calculationDiagnostic)
          EnConfigurationSettings.GlobalSettings.AllowCalculationDiagnostic = false;
        LoanData loanData = new LoanData(this.loan.XmlDocClone, this.loan.Settings, false, true);
        LoanCalculator loanCalculator = new LoanCalculator(this.sessionObjects, this.configInfo, loanData, true, this.calObjs.ExternalLateFeeSettings, true);
        loanData.Calculator.UseBestCaseScenario = false;
        loanData.Calculator.UseWorstCaseScenario = true;
        string str = this.Val("1172");
        loanData.SetCurrentField("1765", "");
        if (str == "FarmersHomeAdministration")
        {
          loanData.SetField("3560", "");
          loanData.SetField("3561", "");
          loanData.SetField("3562", "");
          loanData.SetField("3563", "");
          loanData.SetField("3564", "");
          loanData.SetField("3565", "");
          loanData.SetField("3566", "");
        }
        else
        {
          loanData.SetCurrentField("1199", "");
          loanData.SetCurrentField("1198", "");
          loanData.SetCurrentField("1201", "");
          loanData.SetCurrentField("1200", "");
          loanData.SetCurrentField("1205", "");
        }
        loanData.SetField("1107", "");
        loanData.SetCurrentField("1109", "10000");
        loanData.SetCurrentField("2", "10000");
        scheduleSnapshot = loanData.Calculator.GetPaymentSchedule(false);
        if (calculationDiagnostic)
          EnConfigurationSettings.GlobalSettings.AllowCalculationDiagnostic = calculationDiagnostic;
      }
      catch (Exception ex)
      {
      }
      if (scheduleSnapshot != null)
      {
        PaymentSchedule[] monthlyPayments = scheduleSnapshot.MonthlyPayments;
        if (monthlyPayments.Length > 1)
        {
          double payment = monthlyPayments[0].Payment;
          double num1 = 0.0;
          int num2 = 0;
          for (int index = 0; index < monthlyPayments.Length - 1; ++index)
          {
            if (monthlyPayments[index].Payment > num1 && monthlyPayments.Length - 2 != index)
            {
              num1 = monthlyPayments[index].Payment;
              if (monthlyPayments.Length - 2 != index)
                num2 = index + 1;
            }
          }
          this.SetVal("LE1.X92", payment.ToString());
          this.SetVal("LE1.X95", (6.0 * payment).ToString());
          this.SetVal("LE1.X93", num1.ToString());
          this.SetVal("LE1.X94", num2.ToString());
          return;
        }
      }
      this.SetVal("LE1.X92", "");
      this.SetVal("LE1.X95", "");
      this.SetVal("LE1.X93", "");
      this.SetVal("LE1.X94", "");
    }

    private void setARMIndexDescription(string id, string val)
    {
      if (id != "1959")
        return;
      string val1 = this.Val("666");
      switch (this.Val("1959"))
      {
        case "":
          val1 = "";
          break;
        case "3MoCD(12MoAvg)":
          val1 = "the average rate on 3-month negotiable certificates of deposit (secondary market), quoted on an investment basis, as made available by the Federal Reserve Board.";
          break;
        case "6MCDW":
          val1 = "the average rate on 6-month negotiable certificates of deposit (secondary market), quoted on an investment basis, as made available by the Federal Reserve Board.";
          break;
        case "CMR":
          val1 = "the contract interest rate on 30-year, fixed-rate conventional home mortgage commitments, as made available by the Federal Reserve Board in the Federal Reserve Statistical Release entitled \"Selected Interest Rates(H.15).\"";
          break;
        case "FHFB_NACMR":
          val1 = "derived from the Federal Housing Finance Agency's Monthly Interest Rate Survey (MIRS), as made available by the Federal Housing Finance Agency.";
          break;
        case "FHLBSFCOFI":
          val1 = "the monthly weighted average cost of savings, borrowings and advances of members of the Federal Home Loan Bank of San Francisco (the \"Bank\"), as made available by the Bank. ";
          break;
        case "FHLMC30Y30D":
          val1 = "the Federal Home Loan Mortgage Corporation’s required net yield for 30-year fixed rate mortgages subject to a 30-day mandatory delivery commitment, as published by Federal Home Loan Mortgage Corporation.";
          break;
        case "FHLMC30Y60D":
          val1 = "the Federal Home Loan Mortgage Corporation’s required net yield for 30-year fixed rate mortgages subject to a 60-day mandatory delivery commitment, as published by Federal Home Loan Mortgage Corporation.";
          break;
        case "FHLMC30Y90D":
          val1 = "the Federal Home Loan Mortgage Corporation’s required net yield for 30-year fixed rate mortgages subject to a 90-day mandatory delivery commitment, as published by Federal Home Loan Mortgage Corporation.";
          break;
        case "FHLMCLIBOR1M":
        case "FHLMCLIBOR3M":
        case "FHLMCLIBOR6M":
        case "FNMA12MAVG":
        case "FNMA15Y60D":
        case "FNMA30Y30D":
        case "FNMA30Y60D":
        case "FNMA30Y90D":
        case "FNMALIBOR1M":
        case "FNMALIBOR1Y":
        case "FNMALIBOR3M":
        case "FNMALIBOR6M":
        case "FREDDIE_US15Y":
        case "FREDDIE_US30Y":
        case "FREDDIE_US5YARM":
        case "WSJPrimeMtly":
          val1 = "";
          break;
        case "FHLMCLIBOR1Y":
          val1 = "a benchmark, known as the one-year U.S. dollar (USD) LIBOR index. The Index is currently published in, or on the website of, The Wall Street Journal. ";
          break;
        case "FRBBankPrime":
          val1 = "the average majority prime rate charged by banks on short-term loans to business, quoted on an investment basis, as made available by the Federal Reserve Board in the Federal Reserve Statistical Release entitled \"Selected Interest Rates(H.15).\"";
          break;
        case "FRBCommercial3M":
          val1 = "the 90-Day AA nonfinancial commercial paper interest rate, as made available by the Federal Reserve Board in the Federal Reserve Statistical Release entitled \"Selected Interest Rates(H.15).\"";
          break;
        case "FRBDiscount":
          val1 = "the rate charged for discounts made and advances extended under the Federal Reserve's primary credit discount window program, as made available by the Federal Reserve Board in the Federal Reserve Statistical Release entitled \"Selected Interest Rates(H.15).\"";
          break;
        case "LIBOR12M":
          val1 = "a benchmark, known as the one-year U.S. dollar (USD) LIBOR index. The Index is currently published in, or on the website of, The Wall Street Journal.   ";
          break;
        case "LIBOR1M":
          val1 = "a benchmark, known as the one-month U.S. dollar (USD) LIBOR index. The Index is currently published in, or on the website of, The Wall Street Journal. ";
          break;
        case "LIBOR3M":
          val1 = "a benchmark, known as the three-month U.S. dollar (USD) LIBOR index. The Index is currently published in, or on the website of, The Wall Street Journal.  ";
          break;
        case "LIBOR6M":
          val1 = "a benchmark, known as the six-month U.S. dollar (USD) LIBOR index. The Index is currently published in, or on the website of, The Wall Street Journal.  ";
          break;
        case "MTA":
          val1 = "the \"Twelve - Month Average\" of the annual yields on actively traded United States Treasury Securities adjusted to a constant maturity of one year as published by the Federal Reserve Board in the Federal Reserve Statistical Release entitled \"Selected Interest Rates(H.15)\" (the \"Monthly Yields\"). The Twelve Month Average is determined by adding together the Monthly Yields for the most recently available twelve months and dividing by 12.";
          break;
        case "SOFR_30DayAvg":
          val1 = "is a benchmark, known as the 30-day Average SOFR index.  The Index is currently published by the Federal Reserve Bank of New York. ";
          break;
        case "TNMax":
          val1 = "the maximum effective rate of interest per annum for home loans as set by the Tennessee General Assembly in 1987, Public Chapter 291. The rate as set by the said law is an amount equal to four percentage points above the index of market yields of long-term government bonds adjusted to a thirty (30) year maturity by the U. S. Department of the Treasury.";
          break;
        case "UST10Y":
          val1 = "the daily yield on United States Treasury securities adjusted to a constant maturity of ten years, as made available by the Board of Governors of the Federal Reserve System.  ";
          break;
        case "UST10YW":
          val1 = "the weekly average yield on United States Treasury securities adjusted to a constant maturity of ten years, as made available by the Board of Governors of the Federal Reserve System. ";
          break;
        case "UST1Y":
          val1 = "the daily yield on United States Treasury securities adjusted to a constant maturity of one year, as made available by the Board of Governors of the Federal Reserve System.  ";
          break;
        case "UST1YW":
          val1 = "the weekly average yield on United States Treasury securities adjusted to a constant maturity of one year, as made available by the Board of Governors of the Federal Reserve System. ";
          break;
        case "UST20Y":
          val1 = "the daily yield on United States Treasury securities adjusted to a constant maturity of twenty years, as made available by the Board of Governors of the Federal Reserve System. ";
          break;
        case "UST20YW":
          val1 = "the weekly average yield on United States Treasury securities adjusted to a constant maturity of twenty years, as made available by the Board of Governors of the Federal Reserve System.  ";
          break;
        case "UST2Y":
          val1 = "the daily average yield on United States Treasury securities adjusted to a constant maturity of two years, as made available by the Board of Governors of the Federal Reserve System.";
          break;
        case "UST2YM":
          val1 = "the monthly average yield on United States Treasury securities adjusted to a constant maturity of two years, as made available by the Board of Governors of the Federal Reserve System.";
          break;
        case "UST2YW":
          val1 = "the weekly average yield on United States Treasury securities adjusted to a constant maturity of two years, as made available by the Board of Governors of the Federal Reserve System.";
          break;
        case "UST30Y":
          val1 = "the daily yield on United States Treasury securities adjusted to a constant maturity of thirty years, as made available by the Board of Governors of the Federal Reserve System.  ";
          break;
        case "UST30YW":
          val1 = "the weekly average yield on United States Treasury securities adjusted to a constant maturity of thirty years, as made available by the Board of Governors of the Federal Reserve System.  ";
          break;
        case "UST3Y":
          val1 = "the daily yield on United States Treasury securities adjusted to a constant maturity of three years, as made available by the Board of Governors of the Federal Reserve System. ";
          break;
        case "UST3YW":
          val1 = "the weekly average yield on United States Treasury securities adjusted to a constant maturity of three years, as made available by the Board of Governors of the Federal Reserve System.  ";
          break;
        case "UST5Y":
          val1 = "the daily yield on United States Treasury securities adjusted to a constant maturity of five years, as made available by the Board of Governors of the Federal Reserve System.  ";
          break;
        case "UST5YW":
          val1 = "the weekly average yield on United States Treasury securities adjusted to a constant maturity of five years, as made available by the Board of Governors of the Federal Reserve System.  ";
          break;
        case "UST6M":
          val1 = "the daily yield on United States Treasury securities adjusted to a constant maturity of six months, as made available by the Board of Governors of the Federal Reserve System.  ";
          break;
        case "UST7Y":
          val1 = "the daily yield on United States Treasury securities adjusted to a constant maturity of seven years, as made available by the Board of Governors of the Federal Reserve System.  ";
          break;
        case "UST7YW":
          val1 = "the weekly average yield on United States Treasury securities adjusted to a constant maturity of seven years, as made available by the Board of Governors of the Federal Reserve System.  ";
          break;
        case "WSJPrime":
          val1 = "the Wall Street Journal Prime Rate published in the Money Rates section of the Wall Street Journal as \"The base rate on corporate loans posted by at least 70 % of the 10 largest U.S.banks.It is not the 'best' rate offered by banks.\" If more than one such prime rate is published on any given day, the highest of such published rates shall be the Wall Street Journal Prime Rate.";
          break;
        case "WSJPrimeWkly":
          val1 = "the Wall Street Journal Prime Rate published in the Money Rates section of the Wall Street Journal as \"The base rate on corporate loans posted by at least 70 % of the 10 largest U.S.banks.It is not the 'best' rate offered by banks.\" If more than one such prime rate is published on any given day, the highest of such published rates shall be the Wall Street Journal Prime Rate.";
          break;
      }
      this.SetVal("666", val1);
    }

    private void setTempHash(Hashtable xdFields, string key, string value)
    {
      if (xdFields.ContainsKey((object) key))
        xdFields[(object) key] = (object) value;
      else
        xdFields.Add((object) key, (object) value);
    }

    public PaymentScheduleSnapshot GetBestCaseScenarioPaymentSchedule(LoanData bestCaseLoan)
    {
      bool calculationDiagnostic = EnConfigurationSettings.GlobalSettings.AllowCalculationDiagnostic;
      if (bestCaseLoan == null)
      {
        if (calculationDiagnostic)
          EnConfigurationSettings.GlobalSettings.AllowCalculationDiagnostic = false;
        bestCaseLoan = new LoanData(this.loan.XmlDocClone, this.loan.Settings, false, true);
        LoanCalculator loanCalculator = new LoanCalculator(this.sessionObjects, this.calObjs.LoanConfiguration, bestCaseLoan, true, this.calObjs.ExternalLateFeeSettings, true);
      }
      bestCaseLoan.Calculator.UseWorstCaseScenario = false;
      bestCaseLoan.Calculator.UseBestCaseScenario = true;
      PaymentScheduleSnapshot paymentSchedule = bestCaseLoan.Calculator.GetPaymentSchedule(false);
      if (calculationDiagnostic)
        EnConfigurationSettings.GlobalSettings.AllowCalculationDiagnostic = calculationDiagnostic;
      return paymentSchedule;
    }

    private void calculateHUD1Page2(string id, string val)
    {
      this.calculateClosingCost(id, val);
      this.calculateTotalCosts(id, val);
      this.calObjs.D1003URLA2020Cal.CalcOtherCredits(id, val);
    }

    private void calculateHUD1Page3(string id, string val)
    {
      this.CalcHud1Page3PredependentFields();
      this.calObjs.NewHud2015Cal.AggrEscrowAcc_EscrowedCosts(id, val);
      this._newHudCalculationServant.PopulateHud1Page3Page();
      this._newHudCalculationServant.SetTotalCharges();
      this._newHudCalculationServant.calculateHUD1ToleranceLimits();
      this.calObjs.NewHud2015Cal.CalcEstimatedTaxesTable();
      this.calculateHUD1ToleranceLimits(id, val);
    }

    internal void CalcHud1Page3PredependentFields()
    {
      bool flag1 = false;
      bool flag2 = true;
      for (int index = 41; index <= 49; ++index)
      {
        if (this.Val("HUD01" + (object) index) != "")
        {
          flag2 = false;
          break;
        }
      }
      if (!flag2)
      {
        if (this.FltVal("231") > 0.0 && this.Val("HUD0141") != "")
        {
          this.SetVal("NEWHUD.X337", "Y");
          flag1 = true;
        }
        else if (this.FltVal("231") > 0.0)
          this.SetVal("NEWHUD.X337", "N");
        else
          this.SetVal("NEWHUD.X337", "");
        if (this.FltVal("235") > 0.0 && this.Val("HUD0144") != "")
        {
          this.SetVal("NEWHUD.X338", "Y");
          flag1 = true;
        }
        else if (this.FltVal("235") > 0.0)
          this.SetVal("NEWHUD.X338", "N");
        else
          this.SetVal("NEWHUD.X338", "");
        if (this.FltVal("L268") > 0.0 && this.Val("HUD0145") != "")
        {
          this.SetVal("NEWHUD.X1726", "Y");
          flag1 = true;
        }
        else if (this.FltVal("L268") > 0.0)
          this.SetVal("NEWHUD.X1726", "N");
        else
          this.SetVal("NEWHUD.X1726", "");
        if (this.FltVal("230") > 0.0 && this.Val("HUD0142") != "")
        {
          this.SetVal("NEWHUD.X339", "Y");
          flag1 = true;
        }
        else if (this.FltVal("230") > 0.0)
          this.SetVal("NEWHUD.X339", "N");
        else
          this.SetVal("NEWHUD.X339", "");
        if (this.FltVal("232") > 0.0 && this.Val("HUD0143") != "")
        {
          this.SetVal("NEWHUD.X1728", "Y");
          flag1 = true;
        }
        else if (this.FltVal("232") > 0.0)
          this.SetVal("NEWHUD.X1728", "N");
        else
          this.SetVal("NEWHUD.X1728", "");
        if (this.FltVal("1630") > 0.0 && this.Val("HUD0146") != "")
        {
          this.SetVal("NEWHUD.X340", "Y");
          flag1 = true;
        }
        else if (this.FltVal("1630") > 0.0)
          this.SetVal("NEWHUD.X340", "N");
        else
          this.SetVal("NEWHUD.X340", "");
        if (this.FltVal("253") > 0.0 && this.Val("HUD0147") != "")
        {
          this.SetVal("NEWHUD.X341", "Y");
          flag1 = true;
        }
        else if (this.FltVal("253") > 0.0)
          this.SetVal("NEWHUD.X341", "N");
        else
          this.SetVal("NEWHUD.X341", "");
        if (this.FltVal("254") > 0.0 && this.Val("HUD0148") != "")
        {
          this.SetVal("NEWHUD.X342", "Y");
          flag1 = true;
        }
        else if (this.FltVal("254") > 0.0)
          this.SetVal("NEWHUD.X342", "N");
        else
          this.SetVal("NEWHUD.X342", "");
        if (this.FltVal("NEWHUD.X1707") > 0.0 && this.Val("HUD0149") != "")
        {
          if (this.Val("NEWHUD.X357") != "Y")
            this.SetVal("NEWHUD.X343", "Y");
          else
            this.SetVal("NEWHUD.X343", "");
          flag1 = true;
        }
        else if (this.FltVal("NEWHUD.X1707") > 0.0)
          this.SetVal("NEWHUD.X343", "N");
        else
          this.SetVal("NEWHUD.X343", "");
        this.SetVal("NEWHUD2.X134", this.Val("NEWHUD.X337"));
        this.SetVal("NEWHUD2.X133", this.Val("NEWHUD.X339"));
        this.SetVal("NEWHUD2.X136", this.Val("NEWHUD.X338"));
        this.SetVal("NEWHUD2.X135", this.Val("NEWHUD.X1726"));
        this.SetVal("NEWHUD2.X137", this.Val("NEWHUD.X340"));
        this.SetVal("NEWHUD2.X138", this.Val("NEWHUD.X341"));
        this.SetVal("NEWHUD2.X139", this.Val("NEWHUD.X342"));
        this.SetVal("NEWHUD2.X140", this.Val("NEWHUD.X343"));
        this.SetVal("NEWHUD2.X4769", this.Val("NEWHUD.X1728"));
      }
      else
      {
        if (this.Val("NEWHUD2.X134") == "Y")
          this.SetVal("NEWHUD.X337", "Y");
        else if (this.FltVal("231") != 0.0)
          this.SetVal("NEWHUD.X337", "N");
        else
          this.SetVal("NEWHUD.X337", "");
        if (this.Val("NEWHUD2.X133") == "Y")
          this.SetVal("NEWHUD.X339", "Y");
        else if (this.FltVal("230") != 0.0)
          this.SetVal("NEWHUD.X339", "N");
        else
          this.SetVal("NEWHUD.X339", "");
        if (this.Val("NEWHUD2.X4769") == "Y")
          this.SetVal("NEWHUD.X1728", "Y");
        else if (this.FltVal("232") != 0.0)
        {
          this.SetVal("NEWHUD.X1728", "N");
          this.SetVal("NEWHUD2.X4769", "N");
        }
        else
          this.SetVal("NEWHUD.X1728", "");
        if (this.Val("NEWHUD2.X136") == "Y")
          this.SetVal("NEWHUD.X338", "Y");
        else if (this.FltVal("235") != 0.0)
          this.SetVal("NEWHUD.X338", "N");
        else
          this.SetVal("NEWHUD.X338", "");
        if (this.Val("NEWHUD2.X135") == "Y")
          this.SetVal("NEWHUD.X1726", "Y");
        else if (this.FltVal("L268") != 0.0)
          this.SetVal("NEWHUD.X1726", "N");
        else
          this.SetVal("NEWHUD.X1726", "");
        if (this.Val("NEWHUD2.X137") == "Y")
          this.SetVal("NEWHUD.X340", "Y");
        else if (this.FltVal("1630") != 0.0)
          this.SetVal("NEWHUD.X340", "N");
        else
          this.SetVal("NEWHUD.X340", "");
        if (this.Val("NEWHUD2.X138") == "Y")
          this.SetVal("NEWHUD.X341", "Y");
        else if (this.FltVal("253") != 0.0)
          this.SetVal("NEWHUD.X341", "N");
        else
          this.SetVal("NEWHUD.X341", "");
        if (this.Val("NEWHUD2.X139") == "Y")
          this.SetVal("NEWHUD.X342", "Y");
        else if (this.FltVal("254") != 0.0)
          this.SetVal("NEWHUD.X342", "N");
        else
          this.SetVal("NEWHUD.X342", "");
        if (this.Val("NEWHUD2.X140") == "Y")
          this.SetVal("NEWHUD.X343", "Y");
        else if (this.FltVal("NEWHUD.X1707") != 0.0)
          this.SetVal("NEWHUD.X343", "N");
        else
          this.SetVal("NEWHUD.X343", "");
      }
      this.SetVal("NEWHUD.X335", flag1 ? "Yes" : "No");
      if (!flag1)
      {
        this.SetVal("NEWHUD.X802", "");
        this.SetVal("NEWHUD.X950", "");
      }
      this._newHudCalculationServant.PopulateHud1Page3Page();
    }

    private bool skipLenderObligatedFee(string lineNumber)
    {
      int lineNumber1 = Utils.ParseInt((object) lineNumber);
      return Utils.IsLenderObligatedFee(lineNumber1) && this.Val(Utils.GetLenderObligatedIndicatorFieldID(lineNumber1)) == "Y";
    }

    private void calculateClosingCost(string id, string val)
    {
      Tracing.Log(NewHUDCalculation.sw, TraceLevel.Info, nameof (NewHUDCalculation), "NEW HUD: CalcClosingCost  ID: " + id);
      bool useNew2015Gfehud = this.UseNew2015GFEHUD;
      if (this.Val("420") != "SecondLien" && this.Val("19").IndexOf("Refinance") == -1)
      {
        if (this.Val("NEWHUD.X572") == "NA")
        {
          this.SetVal("NEWHUD.X572", "");
          if (this.UseNewGFEHUD | useNew2015Gfehud)
            this.CopyHUD2010ToGFE2010("NEWHUD.X572", id, false);
        }
        if (this.Val("NEWHUD.X39") == "NA")
          this.SetVal("NEWHUD.X39", "");
      }
      this.SetCurrentNum("NEWHUD.X1716", this.FltVal("NEWHUD.X1719") + (this.addSellerPaidToBorrowerPaid("656", "NEWHUD.X692", "596") + this.addSellerPaidToBorrowerPaid("338", "NEWHUD.X693", "563") + this.addSellerPaidToBorrowerPaid("655", "NEWHUD.X694", "595") + this.addSellerPaidToBorrowerPaid("L269", "NEWHUD.X695", "L270") + this.addSellerPaidToBorrowerPaid("657", "NEWHUD.X696", "597") + this.addSellerPaidToBorrowerPaid("1631", "NEWHUD.X697", "1632") + this.addSellerPaidToBorrowerPaid("658", "NEWHUD.X698", "598") + this.addSellerPaidToBorrowerPaid("659", "NEWHUD.X699", "599") + this.addSellerPaidToBorrowerPaid("NEWHUD.X1708", "NEWHUD.X1709", "NEWHUD.X1714")));
      double num1 = 0.0;
      if (useNew2015Gfehud)
      {
        for (int index = 3938; index <= 4169; index += 33)
          num1 = num1 + this.FltVal("NEWHUD2.X" + (object) index) + this.FltVal("NEWHUD2.X" + (object) (index + 6)) + this.FltVal("NEWHUD2.X" + (object) (index + 9)) + this.FltVal("NEWHUD2.X" + (object) (index + 12));
      }
      else
      {
        for (int index = 147; index <= 154; ++index)
          num1 = num1 + this.FltVal("POPT.X" + (object) index) + this.FltVal("PTC.X" + (object) index);
      }
      this.SetCurrentNum("NEWHUD.X603", num1);
      this.SetCurrentNum("NEWHUD.X777", num1 + (this.addSellerPaidToBorrowerPaid("NEWHUD.X254", "NEWHUD.X42", "NEWHUD.X258") + this.addSellerPaidToBorrowerPaid("644", "NEWHUD.X44", "590") + this.addSellerPaidToBorrowerPaid("645", "NEWHUD.X46", "591") + this.addSellerPaidToBorrowerPaid("41", "NEWHUD.X48", "42") + this.addSellerPaidToBorrowerPaid("44", "NEWHUD.X50", "55") + this.addSellerPaidToBorrowerPaid("1783", "NEWHUD.X52", "1784") + this.addSellerPaidToBorrowerPaid("1788", "NEWHUD.X54", "1789") + this.addSellerPaidToBorrowerPaid("1793", "NEWHUD.X56", "1794")));
      double num2;
      if (useNew2015Gfehud)
      {
        num2 = 0.0;
        for (int index = 3941; index <= 4172; index += 33)
          num2 += this.FltVal("NEWHUD2.X" + (object) index);
      }
      else
        num2 = this.FltVal("NEWHUD.X258") + this.FltVal("590") + this.FltVal("591") + this.FltVal("42") + this.FltVal("55") + this.FltVal("1784") + this.FltVal("1789") + this.FltVal("1794");
      this.SetCurrentNum("NEWHUD.X800", num2);
      int length1 = HUDGFE2010Fields.BORROWERFIELDS.Length;
      double num3 = 0.0;
      double num4 = 0.0;
      double num5 = 0.0;
      double totalBorrowerPaid = 0.0;
      double totalBrokerPaid = 0.0;
      double totalLenderPaid = 0.0;
      double totalOtherPaid = 0.0;
      double num6 = 0.0;
      double num7 = 0.0;
      double totalFinancedAmt = 0.0;
      double totalDisclosureX651 = 0.0;
      double totalDisclosureX652 = 0.0;
      double totalDisclosureX647 = 0.0;
      double totalDisclosureX648 = 0.0;
      double totalDisclosureX670 = 0.0;
      double num8 = 0.0;
      double num9 = 0.0;
      double num10 = 0.0;
      string str1 = string.Empty;
      string str2 = string.Empty;
      string empty1 = string.Empty;
      string str3 = string.Empty;
      string empty2 = string.Empty;
      string empty3 = string.Empty;
      bool flag1 = this.Val("NEWHUD.X1139") == "Y";
      for (int index = 0; index < length1; ++index)
      {
        if (!HUDGFE2010Fields.TOTALEXCLUDEFIELDS.Contains(HUDGFE2010Fields.BORROWERFIELDS[index]) && (useNew2015Gfehud || !HUDGFE2010Fields.BORROWERFIELDS[index].StartsWith("NEWHUD2.X") && !(HUDGFE2010Fields.BORROWERFIELDS[index] == "NEWHUD.X808") && !(HUDGFE2010Fields.BORROWERFIELDS[index] == "NEWHUD.X810") && !(HUDGFE2010Fields.BORROWERFIELDS[index] == "NEWHUD.X812") && !(HUDGFE2010Fields.BORROWERFIELDS[index] == "NEWHUD.X814") && !(HUDGFE2010Fields.BORROWERFIELDS[index] == "NEWHUD.X816") && !(HUDGFE2010Fields.BORROWERFIELDS[index] == "NEWHUD.X818")) && (!useNew2015Gfehud || !(HUDGFE2010Fields.BORROWERFIELDS[index] == "NEWHUD.X645") && !(HUDGFE2010Fields.BORROWERFIELDS[index] == "L215")) && (!flag1 || !(HUDGFE2010Fields.BORROWERFIELDS[index] == "NEWHUD.X687")) && (flag1 || !(HUDGFE2010Fields.BORROWERFIELDS[index] == "NEWHUD.X687") || !(this.Val("NEWHUD.X715") == "Include Origination Credit")) && (flag1 || !(HUDGFE2010Fields.BORROWERFIELDS[index] == "NEWHUD.X687") || !(this.Val("NEWHUD.X713") == "Origination Charge")))
        {
          double num11 = this.FltVal(HUDGFE2010Fields.BORROWERFIELDS[index]);
          bool flag2 = HUDGFE2010Fields.APRFIELDS[index] != string.Empty && this.Val(HUDGFE2010Fields.APRFIELDS[index]) == "Y";
          string paidToID = useNew2015Gfehud || !HUDGFE2010Fields.PAIDTOFIELDS[index].StartsWith("NEWHUD2.X") ? HUDGFE2010Fields.PAIDTOFIELDS[index] : "";
          string str4 = HUDGFE2010Fields.PAIDBYFIELDS[index];
          if (str4 == "" & useNew2015Gfehud)
          {
            if (HUDGFE2010Fields.BORROWERFIELDS[index] == "NEWHUD2.X11")
            {
              str4 = "NEWHUD2.X72";
              flag2 = this.Val("NEWHUD2.X73") == "Y";
            }
            else if (HUDGFE2010Fields.BORROWERFIELDS[index] == "NEWHUD2.X14")
            {
              str4 = "NEWHUD2.X75";
              flag2 = this.Val("NEWHUD2.X76") == "Y";
            }
            else if (HUDGFE2010Fields.BORROWERFIELDS[index] == "NEWHUD.X808")
            {
              str4 = "NEWHUD2.X78";
              flag2 = this.Val("NEWHUD2.X79") == "Y";
            }
            else if (HUDGFE2010Fields.BORROWERFIELDS[index] == "NEWHUD.X810")
            {
              str4 = "NEWHUD2.X81";
              flag2 = this.Val("NEWHUD2.X82") == "Y";
            }
            else if (HUDGFE2010Fields.BORROWERFIELDS[index] == "NEWHUD.X812")
            {
              str4 = "NEWHUD2.X84";
              flag2 = this.Val("NEWHUD2.X85") == "Y";
            }
            else if (HUDGFE2010Fields.BORROWERFIELDS[index] == "NEWHUD.X814")
            {
              str4 = "NEWHUD2.X87";
              flag2 = this.Val("NEWHUD2.X88") == "Y";
            }
            else if (HUDGFE2010Fields.BORROWERFIELDS[index] == "NEWHUD.X816")
            {
              str4 = "NEWHUD2.X90";
              flag2 = this.Val("NEWHUD2.X91") == "Y";
            }
            else if (HUDGFE2010Fields.BORROWERFIELDS[index] == "NEWHUD.X818")
            {
              str4 = "NEWHUD2.X93";
              flag2 = this.Val("NEWHUD2.X94") == "Y";
            }
          }
          str3 = str4 != "" ? this.Val(str4) : "";
          if (!flag1 && HUDGFE2010Fields.BORROWERFIELDS[index] == "NEWHUD.X686" && this.Val("NEWHUD.X713") == "Origination Charge")
            num11 += this.FltVal("1663");
          empty3 = HUDGFE2010Fields.POCFIELDS[index];
          string[] pocPTCFields = !HUDGFE2010Fields.PAIDBYPOPTFIELDS.ContainsKey((object) str4) ? (string[]) null : (string[]) HUDGFE2010Fields.PAIDBYPOPTFIELDS[(object) str4];
          double num12;
          double num13;
          double num14;
          if (pocPTCFields == null || !useNew2015Gfehud && HUDGFE2010Fields.BORROWERFIELDS[index] == "L218")
          {
            num12 = num11;
            str1 = string.Empty;
            num13 = 0.0;
            num10 = 0.0;
            str2 = string.Empty;
            num14 = 0.0;
          }
          else if (useNew2015Gfehud)
          {
            num13 = this.FltVal(pocPTCFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORPOCAMT]) + this.FltVal(pocPTCFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPOCAMT]) + this.FltVal(pocPTCFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPOCAMT]) + this.FltVal(pocPTCFields[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPOCAMT]) + this.FltVal(pocPTCFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELPOCAMT]);
            num14 = this.FltVal(pocPTCFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPACAMT]) + this.FltVal(pocPTCFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPACAMT]) + this.FltVal(pocPTCFields[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPACAMT]);
            num12 = pocPTCFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORPACAMT] != string.Empty ? this.FltVal(pocPTCFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORPACAMT]) : 0.0;
            if (!this.skipLenderObligatedFee(pocPTCFields[HUDGFE2010Fields.PTCPOCINDEX_LINENUMBER]))
              num10 = this.FltVal(pocPTCFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPOCAMT]);
          }
          else
          {
            str1 = pocPTCFields[HUDGFE2010Fields.PTCPOCINDEX_POCPAIDBY] != string.Empty ? this.Val(pocPTCFields[HUDGFE2010Fields.PTCPOCINDEX_POCPAIDBY]) : "";
            num13 = pocPTCFields[HUDGFE2010Fields.PTCPOCINDEX_POCAMT] != string.Empty ? this.FltVal(pocPTCFields[HUDGFE2010Fields.PTCPOCINDEX_POCAMT]) : 0.0;
            str2 = pocPTCFields[HUDGFE2010Fields.PTCPOCINDEX_PTCPAIDBY] != string.Empty ? this.Val(pocPTCFields[HUDGFE2010Fields.PTCPOCINDEX_PTCPAIDBY]) : "";
            num14 = pocPTCFields[HUDGFE2010Fields.PTCPOCINDEX_PTCAMT] != string.Empty ? this.FltVal(pocPTCFields[HUDGFE2010Fields.PTCPOCINDEX_PTCAMT]) : 0.0;
            num12 = pocPTCFields[HUDGFE2010Fields.PTCPOCINDEX_BORTOPAY] != string.Empty ? this.FltVal(pocPTCFields[HUDGFE2010Fields.PTCPOCINDEX_BORTOPAY]) : 0.0;
            num10 = 0.0;
          }
          if (flag2)
          {
            num4 += num12;
            if (useNew2015Gfehud)
            {
              num4 += this.FltVal(pocPTCFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORPOCAMT]) + this.FltVal(pocPTCFields[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPACAMT]) + this.FltVal(pocPTCFields[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPOCAMT]);
              if (!this.UsingTableFunded)
                num4 += this.FltVal(pocPTCFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPACAMT]) + this.FltVal(pocPTCFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPOCAMT]);
            }
            else
            {
              if (str1 == string.Empty || this.UseNewGFEHUD && (str1 == "Other" || !this.UsingTableFunded && str1 == "Broker"))
                num4 += num13;
              if (str2 == string.Empty || this.UseNewGFEHUD && (str2 == "Other" || !this.UsingTableFunded && str2 == "Broker"))
                num4 += num14;
            }
          }
          num3 += num12 + num14;
          num9 += this.isPaidToBroker(paidToID) ? num12 + num14 : 0.0;
          num6 += num13;
          num7 += num10;
          if (HUDGFE2010Fields.BORROWERFIELDS[index] != "337")
            num5 += num11;
          if (!HUDGFE2010Fields.BORROWER900FIELDS.Contains(HUDGFE2010Fields.BORROWERFIELDS[index]))
            num8 += num11;
          this.calculatePaidBy(pocPTCFields, useNew2015Gfehud, ref totalBrokerPaid, ref totalLenderPaid, ref totalOtherPaid, ref totalBorrowerPaid, ref totalDisclosureX651, ref totalDisclosureX652, ref totalDisclosureX647, ref totalDisclosureX648, ref totalDisclosureX670, ref totalFinancedAmt);
        }
      }
      if (useNew2015Gfehud)
      {
        if (this.IsLocked("NEWHUD2.X5"))
          num3 = num3 - (this.FltVal("NEWHUD2.X1") + this.FltVal("NEWHUD2.X3")) + this.FltVal("NEWHUD2.X5");
        for (int index = 1; index <= 11; ++index)
        {
          if (index != 6)
            num4 += this.check2015PrepaidCharge((string[]) HUDGFE2010Fields.POCPTCFIELDS[(object) ("POPT.X" + (object) index)]);
        }
        for (int index = 247; index <= 253; ++index)
          num4 += this.check2015PrepaidCharge((string[]) HUDGFE2010Fields.POCPTCFIELDS[(object) ("POPT.X" + (object) index)]);
        for (int index = 235; index <= 238; ++index)
          num4 += this.check2015PrepaidCharge((string[]) HUDGFE2010Fields.POCPTCFIELDS[(object) ("POPT.X" + (object) index)]);
        for (int index = 309; index <= 870; index += 33)
        {
          num6 += this.FltVal("NEWHUD2.X" + (object) index) + this.FltVal("NEWHUD2.X" + (object) (index + 3)) + this.FltVal("NEWHUD2.X" + (object) (index + 6)) + this.FltVal("NEWHUD2.X" + (object) (index + 9)) + this.FltVal("NEWHUD2.X" + (object) (index + 12));
          num7 += this.FltVal("NEWHUD2.X" + (object) (index + 9));
        }
        for (int index = 936; index <= 1035; index += 33)
        {
          num6 += this.FltVal("NEWHUD2.X" + (object) index) + this.FltVal("NEWHUD2.X" + (object) (index + 3)) + this.FltVal("NEWHUD2.X" + (object) (index + 6)) + this.FltVal("NEWHUD2.X" + (object) (index + 9)) + this.FltVal("NEWHUD2.X" + (object) (index + 12));
          num7 += this.FltVal("NEWHUD2.X" + (object) (index + 9));
        }
      }
      else
        num4 += this.checkPrepaidCharge("SYS.X17", "SYS.X251") + this.checkPrepaidCharge("SYS.X116", "SYS.X261") + this.checkPrepaidCharge("SYS.X201", "SYS.X269") + this.checkPrepaidCharge("SYS.X23", "SYS.X271") + this.checkPrepaidCharge("SYS.X28", "SYS.X265") + this.checkPrepaidCharge("SYS.X37", "SYS.X289") + this.checkPrepaidCharge("SYS.X215", "SYS.X291") + this.checkPrepaidCharge("SYS.X294", "SYS.X296") + this.checkPrepaidCharge("SYS.X299", "SYS.X301") + this.checkPrepaidCharge("NEWHUD.X689", "NEWHUD.X748") + this.checkPrepaidCharge("NEWHUD.X1241", "NEWHUD.X1239") + this.checkPrepaidCharge("NEWHUD.X1249", "NEWHUD.X1247") + this.checkPrepaidCharge("NEWHUD.X1257", "NEWHUD.X1255") + this.checkPrepaidCharge("NEWHUD.X1265", "NEWHUD.X1263") + this.checkPrepaidCharge("NEWHUD.X1273", "NEWHUD.X1271") + this.checkPrepaidCharge("NEWHUD.X1281", "NEWHUD.X1279") + this.checkPrepaidCharge("NEWHUD.X1289", "NEWHUD.X1287");
      if (flag1)
      {
        if (this.Val("NEWHUD.X713") != "Origination Charge")
        {
          num3 = num3 - (this.FltVal("NEWHUD.X1142") + this.FltVal("NEWHUD.X1144") + this.FltVal("NEWHUD.X1146") + this.FltVal("NEWHUD.X1148")) + (this.FltVal("NEWHUD.X1151") + this.FltVal("NEWHUD.X1155") + this.FltVal("NEWHUD.X1159") + this.FltVal("NEWHUD.X1163"));
          if (this.FltVal("NEWHUD.X1149") != 0.0 && !this.UseNew2015GFEHUD)
            num3 += this.FltVal("NEWHUD.X1152") + this.FltVal("NEWHUD.X1156") + this.FltVal("NEWHUD.X1160") + this.FltVal("NEWHUD.X1164");
        }
        if (!useNew2015Gfehud)
          num4 += this.checkPrepaidCharge("NEWHUD.X1177", "NEWHUD.X1175") + this.checkPrepaidCharge("NEWHUD.X1181", "NEWHUD.X1179") + this.checkPrepaidCharge("NEWHUD.X1185", "NEWHUD.X1183") + this.checkPrepaidCharge("NEWHUD.X1189", "NEWHUD.X1187");
      }
      else
      {
        bool flag3 = this.Val("NEWHUD.X713") == "Origination Charge";
        string str5 = this.Val("NEWHUD.X715");
        if (flag3)
        {
          switch (this.Val("NEWHUD.X894"))
          {
            case "Lender":
              totalLenderPaid += this.FltVal("NEWHUD.X831");
              break;
            case "Broker":
              totalBrokerPaid += this.FltVal("NEWHUD.X831");
              break;
            case "Other":
              totalOtherPaid += this.FltVal("NEWHUD.X831");
              break;
            case "":
              totalBorrowerPaid += this.FltVal("NEWHUD.X831");
              break;
          }
          switch (this.Val("PTC.X168"))
          {
            case "Lender":
              totalLenderPaid += this.FltVal("PTC.X90");
              break;
            case "Broker":
              totalBrokerPaid += this.FltVal("PTC.X90");
              break;
            case "Other":
              totalOtherPaid += this.FltVal("PTC.X90");
              break;
            case "":
              totalBorrowerPaid += this.FltVal("PTC.X90");
              break;
          }
          totalBorrowerPaid += this.FltVal("POPT.X90");
          if (!useNew2015Gfehud && this.Val("NEWHUD.X353") == "Y")
          {
            switch (str5)
            {
              case "Include Origination Credit":
                num4 -= this.Val("NEWHUD.X749") == "" ? this.FltVal("1663") : 0.0;
                break;
              case "Include Origination Points":
                num4 += this.checkPrepaidCharge("NEWHUD.X353", "NEWHUD.X749");
                break;
            }
          }
        }
        else
        {
          switch (str5)
          {
            case "Include Origination Points":
              num3 += this.FltVal("NEWHUD.X831");
              break;
            case "Include Origination Credit":
              num3 -= this.FltVal("1663");
              break;
          }
        }
      }
      if (this.Val("19") == "ConstructionToPermanent")
        num4 += this.FltVal("NEWHUD2.X4768");
      double num15 = totalDisclosureX651;
      for (int index = 0; index < CalculationBase.line801PaidByFields.Length; ++index)
      {
        string[] pocPTCFields = (string[]) HUDGFE2010Fields.PAIDBYPOPTFIELDS[(object) CalculationBase.line801PaidByFields[index]];
        if (flag1 || !(CalculationBase.line801PaidByFields[index] == "NEWHUD.X1175"))
          this.calculatePaidBy(pocPTCFields, useNew2015Gfehud, ref totalBrokerPaid, ref totalLenderPaid, ref totalOtherPaid, ref totalBorrowerPaid, ref totalDisclosureX651, ref totalDisclosureX652, ref totalDisclosureX647, ref totalDisclosureX648, ref totalDisclosureX670, ref totalFinancedAmt);
        else
          break;
      }
      this.calculateTotalLenderCredits(totalLenderPaid);
      this.calObjs.HMDACal.CalcLenderCredits(id, val);
      double tLenderPaid = 0.0;
      double currentCreditLeft = flag1 ? this.FltVal("NEWHUD.X1144") + this.FltVal("NEWHUD.X1146") + this.FltVal("NEWHUD.X1148") : this.FltVal("1663");
      if (flag1)
      {
        if (this.FltVal("NEWHUD.X225") != this.FltVal("NEWHUD.X1142"))
          currentCreditLeft = this.calculateLenderCredit("NEWHUD.X227", currentCreditLeft + this.FltVal("NEWHUD.X1142"), ref tLenderPaid);
        currentCreditLeft = this.calculateLenderCredit("NEWHUD.X1187", this.calculateLenderCredit("NEWHUD.X1183", this.calculateLenderCredit("NEWHUD.X1179", this.calculateLenderCredit("NEWHUD.X1175", currentCreditLeft, ref tLenderPaid), ref tLenderPaid), ref tLenderPaid), ref tLenderPaid);
      }
      else if (this.Val("NEWHUD.X715") == "Include Origination Points")
      {
        if (this.Val("PTC.X168") == "Lender")
          tLenderPaid += this.FltVal("PTC.X90");
        if (this.Val("NEWHUD.X894") == "Lender")
          tLenderPaid += this.FltVal("NEWHUD.X831");
      }
      this.Val("1172");
      double num16 = 0.0;
      double num17 = 0.0;
      double creditOffSet = 0.0;
      double num18 = 0.0;
      double num19 = 0.0;
      this.totalOffset900And1000POCPaid = this.totalOffsetNon900Non1000POCPaid = 0.0;
      this.totalOffsetNonPrepaidBorPaidPOC = this.totalOffsetPrepaidBorPaidPOC = 0.0;
      double num20 = 0.0;
      double num21 = 0.0;
      for (int index = 0; index < HUDGFE2010Fields.WHOLEPOC_FIELDS.Count; ++index)
      {
        string[] pocFields = HUDGFE2010Fields.WHOLEPOC_FIELDS[index];
        if (!(pocFields[HUDGFE2010Fields.PTCPOCINDEX_LINENUMBER] == "2001"))
        {
          if ((this.UseNew2015GFEHUD || !(pocFields[HUDGFE2010Fields.PTCPOCINDEX_FOR2015] == "Y")) && !(pocFields[HUDGFE2010Fields.PTCPOCINDEX_PAIDBY] == "NEWHUD.X227") && !(pocFields[HUDGFE2010Fields.PTCPOCINDEX_LINENUMBER] == "0802"))
          {
            if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_LINENUMBER] == "0901")
              num16 = currentCreditLeft;
            if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_LINENUMBER] == "0902" && currentCreditLeft > 0.0 && (this.UseNewGFEHUD && this.Val("PTC.X186") == "Lender" || this.UseNew2015GFEHUD && this.FltVal("NEWHUD2.X2200") > 0.0))
              creditOffSet = this.UseNew2015GFEHUD && this.FltVal("NEWHUD2.X2200") > currentCreditLeft || this.UseNewGFEHUD && this.FltVal("PTC.X108") > currentCreditLeft ? (num18 = currentCreditLeft) : (num18 = this.UseNew2015GFEHUD ? this.FltVal("NEWHUD2.X2200") : this.FltVal("PTC.X108"));
            else if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_LINENUMBER] == "0905" && currentCreditLeft > 0.0 && (this.UseNewGFEHUD && this.Val("PTC.X189") == "Lender" || this.UseNew2015GFEHUD && this.FltVal("NEWHUD2.X2299") > 0.0))
              creditOffSet = this.UseNew2015GFEHUD && this.FltVal("NEWHUD2.X2299") > currentCreditLeft || this.UseNewGFEHUD && this.FltVal("PTC.X111") > currentCreditLeft ? (num18 = currentCreditLeft) : (num18 = this.UseNew2015GFEHUD ? this.FltVal("NEWHUD2.X2299") : this.FltVal("PTC.X111"));
            int num22 = Utils.ParseInt((object) pocFields[HUDGFE2010Fields.PTCPOCINDEX_LINENUMBER]);
            if (num22 >= 804 && num22 <= 833)
            {
              string str6 = this.Val(pocFields[HUDGFE2010Fields.PTCPOCINDEX_PAIDBY]);
              if (str6 == "Other" || str6 == string.Empty)
                num20 += this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_BORPAID]);
            }
            if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATEDAMT] != string.Empty)
              num21 += this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATEDAMT]);
            currentCreditLeft = this.calculateLenderCredit(pocFields, currentCreditLeft, ref tLenderPaid);
            if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_LINENUMBER] == "1010")
              num17 = currentCreditLeft;
          }
        }
        else
          break;
      }
      this.SetCurrentNum("QM.X125", num20);
      this.calObjs.GFECal.CalculateField969((string) null, creditOffSet);
      this.calObjs.GFECal.CalcTotalPrepaidFees(id, val);
      double num23 = 0.0;
      double num24 = 0.0;
      if (useNew2015Gfehud)
      {
        for (int index = 0; index < HUDGFE2010Fields.WHOLEPOC_FIELDS.Count; ++index)
        {
          string[] strArray = HUDGFE2010Fields.WHOLEPOC_FIELDS[index];
          if (900 <= Utils.ParseInt((object) strArray[HUDGFE2010Fields.PTCPOCINDEX_LINENUMBER]) && Utils.ParseInt((object) strArray[HUDGFE2010Fields.PTCPOCINDEX_LINENUMBER]) <= 1100 && strArray[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATEDAMT] != "")
            num23 += this.FltVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATEDAMT]);
        }
      }
      this.totalOffsetForPrepaid = num16 - num17 + num23 + num24;
      this.SetCurrentNum("138", this.FltVal("138") - this.totalOffsetForPrepaid + num18);
      this.totalBorrowerPaidCost = totalBorrowerPaid + tLenderPaid + this.FltVal("558");
      double num25 = tLenderPaid - num19;
      if (currentCreditLeft > 0.0)
        num25 += currentCreditLeft;
      this.SetCurrentNum("LENPCCINDOT", num25);
      double num26 = 0.0;
      if (this.IsLocked("BKRPCC"))
        num26 += this.FltVal("BKRPCC") - totalBrokerPaid;
      this.SetCurrentNum("BKRPCC", totalBrokerPaid);
      this.SetCurrentNum("RE882.X66", totalDisclosureX651 - num15);
      this.SetCurrentNum("RE882.X67", totalBrokerPaid);
      if (this.IsLocked("LENPCC"))
        num26 += this.FltVal("LENPCC") - totalLenderPaid;
      this.SetCurrentNum("LENPCC", totalLenderPaid - num7);
      if (this.IsLocked("OTHPCC"))
        num26 += this.FltVal("OTHPCC") - totalOtherPaid;
      this.SetCurrentNum("OTHPCC", totalOtherPaid);
      this.SetCurrentNum("TOTPOC", num6);
      this.SetCurrentNum("1137", num8 - this.FltVal("1093"));
      this.SetCurrentNum("949", num4);
      double num27 = num3 + this.FltVal("558");
      if (!this.IsLocked("NEWHUD.X277"))
        this.SetCurrentNum("NEWHUD.X277", num27);
      if (!this.IsLocked("NEWHUD.X1585"))
        this.SetCurrentNum("NEWHUD.X1585", totalFinancedAmt);
      this.calObjs.USDACal.CalcLoanFundsUsage(id, val);
      this.calObjs.NewHud2015Cal.CalcCDPage3AltDidChangeCol((string) null, (string) null);
      this.calObjs.NewHud2015Cal.CalcLoanEstimateEstimatedCashToCloseSV((string) null, (string) null);
      double num28 = num27;
      int length2 = HUDGFE2010Fields.SELLERFIELDS.Length;
      double num29 = 0.0;
      double num30 = 0.0;
      double num31 = 0.0;
      for (int index = 0; index < length2; ++index)
      {
        if (!(HUDGFE2010Fields.SELLERFIELDS[index] == string.Empty) && (this.UseNew2015GFEHUD || !HUDGFE2010Fields.SELLERFIELDS[index].StartsWith("NEWHUD2.X")) && (!this.UseNew2015GFEHUD || !(HUDGFE2010Fields.SELLERFIELDS[index] == "NEWHUD.X782")))
        {
          double num32 = this.FltVal(HUDGFE2010Fields.SELLERFIELDS[index]);
          if (HUDGFE2010Fields.SELLERFIELDS[index] == "NEWHUD.X780")
          {
            if (!flag1)
            {
              if (this.Val("NEWHUD.X713") == "Origination Charge")
                num32 = this.FltVal("NEWHUD.X788");
            }
            else
              continue;
          }
          num29 += num32;
          double num33 = HUDGFE2010Fields.BORROWERFIELDS[index] != string.Empty ? this.FltVal(HUDGFE2010Fields.BORROWERFIELDS[index]) : 0.0;
          double num34 = HUDGFE2010Fields.GFEFIELDS[index] != string.Empty ? this.FltVal(HUDGFE2010Fields.GFEFIELDS[index]) : 0.0;
          if (!this.UseNew2015GFEHUD && num34 > 0.0 && num34 != num33 && HUDGFE2010Fields.SELLERFIELDS[index] != "L216" && HUDGFE2010Fields.SELLERFIELDS[index] != "L219")
            num28 += num32;
          else
            num31 += num32;
          num30 += num32;
          bool broker = this.isPaidToBroker(HUDGFE2010Fields.PAIDTOFIELDS[index]);
          num9 += broker ? num32 : 0.0;
          totalDisclosureX648 += broker ? num32 : 0.0;
        }
      }
      double num35 = this.FltVal("559") + this.FltVal("L229") + this.FltVal("1622") + this.FltVal("569") + this.FltVal("572") + this.FltVal("NEWHUD.X226") + this.FltVal("200") + this.FltVal("1626") + this.FltVal("1840") + this.FltVal("1843") + this.FltVal("NEWHUD.X779");
      for (int index = 1238; index <= 1286; index += 8)
        num35 += this.FltVal("NEWHUD.X" + (object) index);
      double num36 = num29 + num35;
      double num37 = num30 + num35;
      double num38 = num28 + (this.FltVal("559") + this.FltVal("L229") + this.FltVal("1622") + this.FltVal("569") + this.FltVal("572") + this.FltVal("NEWHUD.X226") + this.FltVal("200") + this.FltVal("1626") + this.FltVal("1840") + this.FltVal("1843") + this.FltVal("NEWHUD.X779") + this.FltVal("NEWHUD.X1238") + this.FltVal("NEWHUD.X1246") + this.FltVal("NEWHUD.X1254") + this.FltVal("NEWHUD.X1262") + this.FltVal("NEWHUD.X1270") + this.FltVal("NEWHUD.X1278") + this.FltVal("NEWHUD.X1286"));
      if (flag1)
        num38 += this.FltVal("NEWHUD.X1152");
      if (!this.IsLocked("NEWHUD.X773"))
        this.SetCurrentNum("NEWHUD.X773", num38);
      if (!this.IsLocked("NEWHUD.X774"))
        this.SetCurrentNum("NEWHUD.X774", num31);
      if (useNew2015Gfehud)
      {
        int num39 = 0;
        if (this.isPaidToBroker("SYS.X252"))
          num9 += this.FltVal("NEWHUD2.X308") + this.FltVal("NEWHUD2.X311") + this.FltVal("NEWHUD2.X314") + this.FltVal("NEWHUD2.X317") + this.FltVal("NEWHUD2.X320");
        int num40 = num39 + 1;
        if (this.isPaidToBroker("SYS.X262"))
          num9 += this.FltVal("NEWHUD2.X" + (object) (308 + num40 * 33)) + this.FltVal("NEWHUD2.X" + (object) (311 + num40 * 33)) + this.FltVal("NEWHUD2.X" + (object) (314 + num40 * 33)) + this.FltVal("NEWHUD2.X" + (object) (317 + num40 * 33)) + this.FltVal("NEWHUD2.X" + (object) (320 + num40 * 33));
        int num41 = num40 + 1;
        if (this.isPaidToBroker("SYS.X270"))
          num9 += this.FltVal("NEWHUD2.X" + (object) (308 + num41 * 33)) + this.FltVal("NEWHUD2.X" + (object) (311 + num41 * 33)) + this.FltVal("NEWHUD2.X" + (object) (314 + num41 * 33)) + this.FltVal("NEWHUD2.X" + (object) (317 + num41 * 33)) + this.FltVal("NEWHUD2.X" + (object) (320 + num41 * 33));
        int num42 = num41 + 1;
        if (this.isPaidToBroker("SYS.X272"))
          num9 += this.FltVal("NEWHUD2.X" + (object) (308 + num42 * 33)) + this.FltVal("NEWHUD2.X" + (object) (311 + num42 * 33)) + this.FltVal("NEWHUD2.X" + (object) (314 + num42 * 33)) + this.FltVal("NEWHUD2.X" + (object) (317 + num42 * 33)) + this.FltVal("NEWHUD2.X" + (object) (320 + num42 * 33));
        int num43 = num42 + 1;
        if (this.isPaidToBroker("SYS.X266"))
          num9 += this.FltVal("NEWHUD2.X" + (object) (308 + num43 * 33)) + this.FltVal("NEWHUD2.X" + (object) (311 + num43 * 33)) + this.FltVal("NEWHUD2.X" + (object) (314 + num43 * 33)) + this.FltVal("NEWHUD2.X" + (object) (317 + num43 * 33)) + this.FltVal("NEWHUD2.X" + (object) (320 + num43 * 33));
        int num44 = num43 + 1;
        if (this.isPaidToBroker("NEWHUD.X230"))
          num9 += this.FltVal("NEWHUD2.X" + (object) (308 + num44 * 33)) + this.FltVal("NEWHUD2.X" + (object) (311 + num44 * 33)) + this.FltVal("NEWHUD2.X" + (object) (314 + num44 * 33)) + this.FltVal("NEWHUD2.X" + (object) (317 + num44 * 33)) + this.FltVal("NEWHUD2.X" + (object) (320 + num44 * 33));
        int num45 = num44 + 1;
        if (this.isPaidToBroker("SYS.X290"))
          num9 += this.FltVal("NEWHUD2.X" + (object) (308 + num45 * 33)) + this.FltVal("NEWHUD2.X" + (object) (311 + num45 * 33)) + this.FltVal("NEWHUD2.X" + (object) (314 + num45 * 33)) + this.FltVal("NEWHUD2.X" + (object) (317 + num45 * 33)) + this.FltVal("NEWHUD2.X" + (object) (320 + num45 * 33));
        int num46 = num45 + 1;
        if (this.isPaidToBroker("SYS.X292"))
          num9 += this.FltVal("NEWHUD2.X" + (object) (308 + num46 * 33)) + this.FltVal("NEWHUD2.X" + (object) (311 + num46 * 33)) + this.FltVal("NEWHUD2.X" + (object) (314 + num46 * 33)) + this.FltVal("NEWHUD2.X" + (object) (317 + num46 * 33)) + this.FltVal("NEWHUD2.X" + (object) (320 + num46 * 33));
        int num47 = num46 + 1;
        if (this.isPaidToBroker("SYS.X297"))
          num9 += this.FltVal("NEWHUD2.X" + (object) (308 + num47 * 33)) + this.FltVal("NEWHUD2.X" + (object) (311 + num47 * 33)) + this.FltVal("NEWHUD2.X" + (object) (314 + num47 * 33)) + this.FltVal("NEWHUD2.X" + (object) (317 + num47 * 33)) + this.FltVal("NEWHUD2.X" + (object) (320 + num47 * 33));
        int num48 = num47 + 1;
        if (this.isPaidToBroker("SYS.X302"))
          num9 += this.FltVal("NEWHUD2.X" + (object) (308 + num48 * 33)) + this.FltVal("NEWHUD2.X" + (object) (311 + num48 * 33)) + this.FltVal("NEWHUD2.X" + (object) (314 + num48 * 33)) + this.FltVal("NEWHUD2.X" + (object) (317 + num48 * 33)) + this.FltVal("NEWHUD2.X" + (object) (320 + num48 * 33));
        int num49 = num48 + 1;
        if (this.isPaidToBroker("NEWHUD.X690"))
          num9 += this.FltVal("NEWHUD2.X" + (object) (308 + num49 * 33)) + this.FltVal("NEWHUD2.X" + (object) (311 + num49 * 33)) + this.FltVal("NEWHUD2.X" + (object) (314 + num49 * 33)) + this.FltVal("NEWHUD2.X" + (object) (317 + num49 * 33)) + this.FltVal("NEWHUD2.X" + (object) (320 + num49 * 33));
        int num50 = num49 + 1;
        for (int index = 1242; index <= 1290; index += 8)
        {
          if (this.isPaidToBroker("NEWHUD.X" + (object) index))
            num9 += this.FltVal("NEWHUD2.X" + (object) (308 + num50 * 33)) + this.FltVal("NEWHUD2.X" + (object) (311 + num50 * 33)) + this.FltVal("NEWHUD2.X" + (object) (314 + num50 * 33)) + this.FltVal("NEWHUD2.X" + (object) (317 + num50 * 33)) + this.FltVal("NEWHUD2.X" + (object) (320 + num50 * 33));
          ++num50;
        }
      }
      else
        num9 = num9 + (this.isPaidToBroker("SYS.X252") ? this.FltVal("454") + this.FltVal("559") - this.FltVal("NEWHUD.X821") : 0.0) + (this.isPaidToBroker("SYS.X262") ? this.FltVal("L228") + this.FltVal("L229") - this.FltVal("NEWHUD.X822") : 0.0) + (this.isPaidToBroker("SYS.X270") ? this.FltVal("1621") + this.FltVal("1622") - this.FltVal("NEWHUD.X823") : 0.0) + (this.isPaidToBroker("SYS.X272") ? this.FltVal("367") + this.FltVal("569") - this.FltVal("NEWHUD.X824") : 0.0) + (this.isPaidToBroker("SYS.X266") ? this.FltVal("439") + this.FltVal("572") - this.FltVal("NEWHUD.X825") : 0.0) + (this.isPaidToBroker("NEWHUD.X230") ? this.FltVal("NEWHUD.X225") + this.FltVal("NEWHUD.X226") - this.FltVal("NEWHUD.X231") : 0.0) + (this.isPaidToBroker("SYS.X290") ? this.FltVal("155") + this.FltVal("200") - this.FltVal("NEWHUD.X826") : 0.0) + (this.isPaidToBroker("SYS.X292") ? this.FltVal("1625") + this.FltVal("1626") - this.FltVal("NEWHUD.X827") : 0.0) + (this.isPaidToBroker("SYS.X297") ? this.FltVal("1839") + this.FltVal("1840") - this.FltVal("NEWHUD.X828") : 0.0) + (this.isPaidToBroker("SYS.X302") ? this.FltVal("1842") + this.FltVal("1843") - this.FltVal("NEWHUD.X829") : 0.0) + (this.isPaidToBroker("NEWHUD.X690") ? this.FltVal("NEWHUD.X733") + this.FltVal("NEWHUD.X779") - this.FltVal("NEWHUD.X830") : 0.0) + (this.isPaidToBroker("NEWHUD.X1242") ? this.FltVal("NEWHUD.X1237") + this.FltVal("NEWHUD.X1238") - this.FltVal("NEWHUD.X1433") : 0.0) + (this.isPaidToBroker("NEWHUD.X1250") ? this.FltVal("NEWHUD.X1245") + this.FltVal("NEWHUD.X1246") - this.FltVal("NEWHUD.X1434") : 0.0) + (this.isPaidToBroker("NEWHUD.X1258") ? this.FltVal("NEWHUD.X1253") + this.FltVal("NEWHUD.X1254") - this.FltVal("NEWHUD.X1435") : 0.0) + (this.isPaidToBroker("NEWHUD.X1266") ? this.FltVal("NEWHUD.X1261") + this.FltVal("NEWHUD.X1262") - this.FltVal("NEWHUD.X1436") : 0.0) + (this.isPaidToBroker("NEWHUD.X1274") ? this.FltVal("NEWHUD.X1269") + this.FltVal("NEWHUD.X1270") - this.FltVal("NEWHUD.X1437") : 0.0) + (this.isPaidToBroker("NEWHUD.X1282") ? this.FltVal("NEWHUD.X1277") + this.FltVal("NEWHUD.X1278") - this.FltVal("NEWHUD.X1438") : 0.0) + (this.isPaidToBroker("NEWHUD.X1290") ? this.FltVal("NEWHUD.X1285") + this.FltVal("NEWHUD.X1286") - this.FltVal("NEWHUD.X1439") : 0.0);
      if (flag1)
      {
        if (useNew2015Gfehud)
        {
          int num51 = 0;
          for (int index = 1178; index <= 1190; index += 4)
          {
            if (this.isPaidToBroker("NEWHUD.X" + (object) index))
              num9 += this.FltVal("NEWHUD2.X" + (object) (935 + num51 * 33)) + this.FltVal("NEWHUD2.X" + (object) (938 + num51 * 33)) + this.FltVal("NEWHUD2.X" + (object) (941 + num51 * 33)) + this.FltVal("NEWHUD2.X" + (object) (944 + num51 * 33)) + this.FltVal("NEWHUD2.X" + (object) (947 + num51 * 33));
            ++num51;
          }
        }
        else
          num9 += (this.isPaidToBroker("NEWHUD.X1178") ? this.FltVal("NEWHUD.X1151") + this.FltVal("NEWHUD.X1152") - this.FltVal("NEWHUD.X1192") : 0.0) + (this.isPaidToBroker("NEWHUD.X1182") ? this.FltVal("NEWHUD.X1155") + this.FltVal("NEWHUD.X1156") - this.FltVal("NEWHUD.X1194") : 0.0) + (this.isPaidToBroker("NEWHUD.X1186") ? this.FltVal("NEWHUD.X1159") + this.FltVal("NEWHUD.X1160") - this.FltVal("NEWHUD.X1196") : 0.0) + (this.isPaidToBroker("NEWHUD.X1190") ? this.FltVal("NEWHUD.X1163") + this.FltVal("NEWHUD.X1164") - this.FltVal("NEWHUD.X1198") : 0.0);
        num36 += this.FltVal("NEWHUD.X1166");
        num37 += this.FltVal("NEWHUD.X1166");
      }
      else
      {
        if (this.Val("NEWHUD.X713") != "Origination Charge" && this.Val("NEWHUD.X715") == "Include Origination Credit")
          num9 += this.isPaidToBroker("NEWHUD.X627") ? this.FltVal("1663") - this.FltVal("NEWHUD.X831") : 0.0;
        if (this.Val("NEWHUD.X713") == "Origination Charge" && this.Val("NEWHUD.X715") == "Include Origination Points")
          num9 += this.isPaidToBroker("NEWHUD.X627") ? this.FltVal("NEWHUD.X15") - this.FltVal("NEWHUD.X831") : 0.0;
      }
      if (this.IsLocked("NEWHUD.X278"))
        num26 += this.FltVal("NEWHUD.X278") - num36;
      if (this.Val("2833") == "Y")
        num9 -= this.FltVal("2005");
      this.SetCurrentNum("1988", num9);
      totalDisclosureX648 += this.isPaidToBroker("SYS.X252") ? this.FltVal("559") : 0.0;
      totalDisclosureX648 += this.isPaidToBroker("SYS.X262") ? this.FltVal("L229") : 0.0;
      totalDisclosureX648 += this.isPaidToBroker("SYS.X270") ? this.FltVal("1622") : 0.0;
      totalDisclosureX648 += this.isPaidToBroker("SYS.X272") ? this.FltVal("569") : 0.0;
      totalDisclosureX648 += this.isPaidToBroker("SYS.X266") ? this.FltVal("572") : 0.0;
      totalDisclosureX648 += this.isPaidToBroker("NEWHUD.X230") ? this.FltVal("NEWHUD.X226") : 0.0;
      totalDisclosureX648 += this.isPaidToBroker("SYS.X290") ? this.FltVal("200") : 0.0;
      totalDisclosureX648 += this.isPaidToBroker("SYS.X292") ? this.FltVal("1626") : 0.0;
      totalDisclosureX648 += this.isPaidToBroker("SYS.X297") ? this.FltVal("1840") : 0.0;
      totalDisclosureX648 += this.isPaidToBroker("SYS.X302") ? this.FltVal("1843") : 0.0;
      totalDisclosureX648 += this.isPaidToBroker("NEWHUD.X690") ? this.FltVal("NEWHUD.X779") : 0.0;
      totalDisclosureX648 += this.isPaidToBroker("NEWHUD.X1242") ? this.FltVal("NEWHUD.X1238") : 0.0;
      totalDisclosureX648 += this.isPaidToBroker("NEWHUD.X1250") ? this.FltVal("NEWHUD.X1246") : 0.0;
      totalDisclosureX648 += this.isPaidToBroker("NEWHUD.X1258") ? this.FltVal("NEWHUD.X1254") : 0.0;
      totalDisclosureX648 += this.isPaidToBroker("NEWHUD.X1266") ? this.FltVal("NEWHUD.X1262") : 0.0;
      totalDisclosureX648 += this.isPaidToBroker("NEWHUD.X1274") ? this.FltVal("NEWHUD.X1270") : 0.0;
      totalDisclosureX648 += this.isPaidToBroker("NEWHUD.X1282") ? this.FltVal("NEWHUD.X1278") : 0.0;
      totalDisclosureX648 += this.isPaidToBroker("NEWHUD.X1290") ? this.FltVal("NEWHUD.X1286") : 0.0;
      this.SetCurrentNum("RE88395.X152", num9);
      this.SetCurrentNum("RE88395.X153", this.FltVal("NEWHUD.X773") + this.FltVal("NEWHUD.X774") - num9);
      this.calObjs.MLDSCal.CalcCompensations(id, val);
      if (this.IsLocked("NEWHUD2.X6"))
        num36 = num36 - (this.FltVal("NEWHUD2.X2") + this.FltVal("NEWHUD2.X4")) + this.FltVal("NEWHUD2.X6");
      this.SetCurrentNum("NEWHUD.X278", num36);
      double num52 = 0.0;
      if (useNew2015Gfehud)
      {
        for (int index = 0; index < HUDGFE2010Fields.WHOLEPOC_FIELDS.Count; ++index)
        {
          string[] strArray = HUDGFE2010Fields.WHOLEPOC_FIELDS[index];
          if (!(strArray[HUDGFE2010Fields.PTCPOCINDEX_LINENUMBER] == "2001"))
          {
            if (strArray[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATEDAMT] != "")
              num52 += this.FltVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATEDAMT]);
          }
          else
            break;
        }
        num36 -= num52;
      }
      this.SetCurrentNum("143", num36);
      double num53 = 0.0;
      if (this.Val("4796") == "Y")
      {
        num53 += this.FltVal("4795");
      }
      else
      {
        if (this.Val("202") == "SellerCredit")
          num53 += this.FltVal("141");
        if (this.Val("1091") == "SellerCredit")
          num53 += this.FltVal("1095");
        if (this.Val("1106") == "SellerCredit")
          num53 += this.FltVal("1115");
        if (this.Val("1646") == "SellerCredit")
          num53 += this.FltVal("1647");
      }
      this.SetCurrentNum("CD3.X108", num53, true);
      this.SetCurrentNum("BORPCC", totalBorrowerPaid + (this.UseNew2015GFEHUD ? 0.0 : this.FltVal("L215") + this.FltVal("L218")) + this.FltVal("558") - num26);
      if (this.Val("NEWHUD.X715") == "Include Origination Points")
      {
        if (!flag1)
          num5 -= this.FltVal("NEWHUD.X15");
        num37 -= this.FltVal("NEWHUD.X788");
      }
      this.SetCurrentNum("1132", num5);
      this.SetCurrentNum("386", num5 + num37 - num21);
      this.SetCurrentNum("1131", num37);
      if (this.Val("DISCLOSURE.X649") == "Y")
        this.SetCurrentNum("DISCLOSURE.X651", totalDisclosureX651);
      else
        this.SetVal("DISCLOSURE.X651", "");
      if (this.Val("DISCLOSURE.X650") == "Y")
        this.SetCurrentNum("DISCLOSURE.X652", totalDisclosureX652);
      else
        this.SetVal("DISCLOSURE.X652", "");
      if (this.Val("DISCLOSURE.X645") == "Y")
        this.SetCurrentNum("DISCLOSURE.X647", totalDisclosureX647);
      else
        this.SetVal("DISCLOSURE.X647", "");
      if (this.Val("DISCLOSURE.X646") == "Y")
        this.SetCurrentNum("DISCLOSURE.X648", totalDisclosureX648);
      else
        this.SetVal("DISCLOSURE.X648", "");
      this.SetCurrentNum("DISCLOSURE.X673", totalDisclosureX670);
      if (this.Val("DISCLOSURE.X458") == "Y")
        this.SetCurrentNum("DISCLOSURE.X670", totalDisclosureX670);
      else
        this.SetVal("DISCLOSURE.X670", "");
      if (this.UseNew2015GFEHUD)
        this.calObjs.NewHud2015Cal.CalcClosingCostSubTotal(id, val);
      this.calculateTotalCosts(id, val);
      this.calObjs.D1003URLA2020Cal.CalcOtherCredits(id, val);
      this.calObjs.FHACal.CalcMACAWP(id, val);
      this.calObjs.CloserCal.CalClosing(id, val);
      this.calObjs.D1003URLA2020Cal.CalcEstimatedClosingCostsAmount(id, val);
    }

    private void calculateTotalCosts(string id, string val)
    {
      bool useNew2015Gfehud = this.UseNew2015GFEHUD;
      Tracing.Log(NewHUDCalculation.sw, TraceLevel.Info, nameof (NewHUDCalculation), "New HUD: calculateTotalCosts ID: " + id);
      this.SetCurrentNum("TNBPCC", this.FltVal("143") + this.FltVal("BKRPCC") + this.FltVal("LENPCC") + this.FltVal("OTHPCC"));
      this.SetCurrentNum("TOTPCC", this.FltVal("BORPCC") + this.FltVal("TNBPCC"));
      bool flag = this.Val("NEWHUD.X1139") == "Y";
      double num1 = this.FltVal("BKRPCC") + this.FltVal("LENPCCINDOT") + this.FltVal("OTHPCC") - this.totalOffset900And1000POCPaid - this.totalOffsetNon900Non1000POCPaid;
      double num2 = 0.0;
      if (this.UseNew2015GFEHUD)
      {
        for (int index = 317; index <= 878; index += 33)
          num2 += this.FltVal("NEWHUD2.X" + (object) (index + 1));
        for (int index = 944; index <= 1043; index += 33)
          num1 += this.FltVal("NEWHUD2.X" + (object) (index + 1));
      }
      else
      {
        for (int index = 884; index <= 893; ++index)
        {
          if (this.Val("NEWHUD.X" + (object) index) == "Lender")
            num2 += this.FltVal("NEWHUD.X" + (object) (index - 63));
        }
        for (int index = 1456; index <= 1462; ++index)
        {
          if (this.Val("NEWHUD.X" + (object) index) == "Lender")
            num2 += this.FltVal("NEWHUD.X" + (object) (index - 23));
        }
        if (flag)
        {
          for (int index = 1193; index <= 1199; index += 2)
          {
            if (this.Val("NEWHUD.X" + (object) index) == "Lender")
              num1 += this.FltVal("NEWHUD.X" + (object) (index - 1));
          }
        }
      }
      this.SetCurrentNum("1852", num1 + num2);
      double num3 = this.totalBorrowerPaidCost + num2 + this.FltVal("BKRPCC") + this.FltVal("OTHPCC");
      double num4 = !this.IsLocked("138") ? num3 - this.FltVal("138") : num3 - this.FltVal("138", true);
      double num5 = !this.IsLocked("143") ? num4 + this.FltVal("143") : num4 + this.FltVal("143", true);
      this.SetCurrentNum("TOTAL_CC", num5);
      double num6 = num5 - this.FltVal("969");
      if (flag)
      {
        if (this.UseNew2015GFEHUD)
        {
          for (int index = 937; index <= 1036; index += 33)
            num6 = num6 - this.FltVal("NEWHUD2.X" + (object) index) - this.FltVal("NEWHUD2.X" + (object) (index + 13));
        }
        else
        {
          for (int index = 239; index <= 242; ++index)
            num6 -= this.FltVal("PTC.X" + (object) index) + this.FltVal("POPT.X" + (object) index);
          for (int index = 1193; index <= 1199; index += 2)
          {
            if (this.Val("NEWHUD.X" + (object) index) != "Lender" && this.FltVal("NEWHUD.X" + (object) (index - 1)) != 0.0)
              num6 -= this.FltVal("NEWHUD.X" + (object) (index - 1));
          }
        }
      }
      else if (this.Val("NEWHUD.X715") == "Include Origination Points")
        num6 -= this.FltVal("1093");
      if (((this.UseNewGFEHUD ? 1 : (this.UseNew2015GFEHUD ? 1 : 0)) & (flag ? 1 : 0)) != 0)
      {
        if (this.UseNew2015GFEHUD)
          num6 -= this.FltVal("NEWHUD.X1152") - this.FltVal("NEWHUD2.X956") + (this.FltVal("NEWHUD.X1156") - this.FltVal("NEWHUD2.X989")) + (this.FltVal("NEWHUD.X1160") - this.FltVal("NEWHUD2.X1022")) + (this.FltVal("NEWHUD.X1164") - this.FltVal("NEWHUD2.X1055"));
        else
          num6 -= this.FltVal("NEWHUD.X1152") + this.FltVal("NEWHUD.X1156") + this.FltVal("NEWHUD.X1160") + this.FltVal("NEWHUD.X1164");
      }
      double num7 = Utils.ArithmeticRounding(num6 - (this.totalOffsetNon900Non1000POCPaid + this.totalOffset900And1000POCPaid + this.totalOffsetNonPrepaidBorPaidPOC + this.totalOffsetPrepaidBorPaidPOC), 2);
      if (!this.UseNew2015GFEHUD)
        num7 += this.FltVal("L215") + this.FltVal("L218");
      if (num7 < 0.0)
        this.SetCurrentNum("137", 0.0);
      else
        this.SetCurrentNum("137", num7);
      double num8 = this.FltVal("136") + this.FltVal("967") + this.FltVal("968");
      double num9 = (!this.USEURLA2020 || !this.IsLocked("URLA.X146") ? num8 + (this.FltVal("138") + this.FltVal("137") + this.FltVal("969")) : num8 + this.FltVal("URLA.X146")) + this.FltVal("1093");
      this.SetCurrentNum("1073", Utils.ArithmeticRounding(!this.USEURLA2020 ? num9 + this.FltVal("1092") : num9 + (this.FltVal("26") + this.FltVal("URLA.X145")), 2));
      double num10 = this.FltVal("1073");
      if (this.Val("420") == "FirstLien")
        this.SetCurrentNum("1845", 0.0);
      double num11 = Utils.ArithmeticRounding(!this.USEURLA2020 ? this.FltVal("140") + this.FltVal("143") + this.FltVal("141") + this.FltVal("1095") + this.FltVal("1115") + this.FltVal("1647") + this.FltVal("1845") + this.FltVal("1851") + this.FltVal("1852") + this.FltVal("2") : this.FltVal("URLA.X148") + this.FltVal("URLA.X152"), 2);
      this.SetCurrentNum("1844", num11);
      double num12 = num10 - num11;
      this.SetCurrentNum("743", num12);
      double num13 = this.FltVal("142");
      this.SetCurrentNum("142", num12);
      if (this.Val("19") == "Cash-Out Refinance")
      {
        BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
        BorrowerPair currentBorrowerPair = this.loan.CurrentBorrowerPair;
        double num14 = 0.0;
        this.loan.SetBorrowerPair(borrowerPairs[0]);
        int exlcudingAlimonyJobExp = this.loan.GetNumberOfLiabilitesExlcudingAlimonyJobExp();
        for (int index = 1; index <= exlcudingAlimonyJobExp; ++index)
        {
          string str1 = this.Val("FL" + index.ToString("00") + "27");
          string str2 = this.Val("FL" + index.ToString("00") + "28");
          if (str1 == "Y" && !string.IsNullOrWhiteSpace(str2) && str2 != "1")
            num14 += this.FltVal("FL" + index.ToString("00") + "16");
        }
        this.loan.SetBorrowerPair(currentBorrowerPair);
        this.SetCurrentNum("ULDD.RefinanceCashOutAmount", Math.Max(0.0, Utils.ArithmeticRounding(this.FltVal("2") - this.FltVal("26") - this.FltVal("137") - this.FltVal("138") - this.FltVal("969") - this.FltVal("1093") + this.FltVal("URLA.X152") + num14, 2)), true);
      }
      else
        this.SetCurrentNum("ULDD.RefinanceCashOutAmount", 0.0);
      if (num13 != num12 || this.FltVal("1206") == 0.0)
      {
        this.calObjs.FHACal.CalcFredMac(id, val);
        if (id != "1109" && id != "TPO")
          this.calObjs.RegzCal.CalcAPR(id, val);
      }
      this.calObjs.VACal.CalcVARecoupment(id, val);
      this.calObjs.D1003URLA2020Cal.CalcEstimatedClosingCostsAmount(id, val);
      this.calObjs.D1003URLA2020Cal.CalcTotalCredits(id, val);
      this.calObjs.VACal.CalcVACashOutRefinance(id, val);
    }

    private double calculateLenderCredit(
      string paidByID,
      double currentCreditLeft,
      ref double tLenderPaid)
    {
      return !HUDGFE2010Fields.PAIDBYPOPTFIELDS.ContainsKey((object) paidByID) ? currentCreditLeft : this.calculateLenderCredit((string[]) HUDGFE2010Fields.PAIDBYPOPTFIELDS[(object) paidByID], currentCreditLeft, ref tLenderPaid);
    }

    private double calculateLenderCredit(
      string[] pocFields,
      double currentCreditLeft,
      ref double tLenderPaid)
    {
      if (pocFields == null)
        return currentCreditLeft;
      if (this.UseNew2015GFEHUD)
      {
        if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_LINENUMBER] != "0801")
        {
          string str = pocFields[HUDGFE2010Fields.PTCPOCINDEX_LINENUMBER].Substring(0, 2);
          if (str == "09" || str == "10")
          {
            if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORPOCAMT] != "")
              this.totalOffsetPrepaidBorPaidPOC += this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORPOCAMT]);
            if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPOCAMT] != "")
              this.totalOffset900And1000POCPaid += this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPOCAMT]);
            if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPOCAMT] != "")
              this.totalOffset900And1000POCPaid += this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPOCAMT]);
          }
          else
          {
            if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORPOCAMT] != "")
              this.totalOffsetNonPrepaidBorPaidPOC += this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORPOCAMT]);
            if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPOCAMT] != "")
              this.totalOffsetNon900Non1000POCPaid += this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPOCAMT]);
            if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPOCAMT] != "")
              this.totalOffsetNon900Non1000POCPaid += this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPOCAMT]);
          }
        }
        double num1 = 0.0;
        double num2 = 0.0;
        if (!this.skipLenderObligatedFee(pocFields[HUDGFE2010Fields.PTCPOCINDEX_LINENUMBER]))
          num2 = this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPACAMT]);
        double num3 = num1 + num2;
        if (num3 == 0.0)
          return currentCreditLeft;
        if (currentCreditLeft == 0.0)
        {
          tLenderPaid += num3;
          return currentCreditLeft;
        }
        if (currentCreditLeft >= num3)
        {
          currentCreditLeft -= num3;
          if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_BORPAID] == "NEWHUD.X1151" || pocFields[HUDGFE2010Fields.PTCPOCINDEX_BORPAID] == "NEWHUD.X1155" || pocFields[HUDGFE2010Fields.PTCPOCINDEX_BORPAID] == "NEWHUD.X1159" || pocFields[HUDGFE2010Fields.PTCPOCINDEX_BORPAID] == "NEWHUD.X1163")
            tLenderPaid += num3;
        }
        else
        {
          tLenderPaid += num3 - currentCreditLeft;
          if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_BORPAID] == "NEWHUD.X1151" || pocFields[HUDGFE2010Fields.PTCPOCINDEX_BORPAID] == "NEWHUD.X1155" || pocFields[HUDGFE2010Fields.PTCPOCINDEX_BORPAID] == "NEWHUD.X1159" || pocFields[HUDGFE2010Fields.PTCPOCINDEX_BORPAID] == "NEWHUD.X1163")
            tLenderPaid += num3 - currentCreditLeft;
          currentCreditLeft = 0.0;
        }
      }
      else
      {
        string str1 = this.Val(pocFields[HUDGFE2010Fields.PTCPOCINDEX_POCPAIDBY]);
        string str2 = this.Val(pocFields[HUDGFE2010Fields.PTCPOCINDEX_PTCPAIDBY]);
        double num4 = this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_POCAMT]);
        if (str1 != "Lender" && num4 > 0.0 && pocFields[HUDGFE2010Fields.PTCPOCINDEX_LINENUMBER] != "0801")
        {
          string str3 = pocFields[HUDGFE2010Fields.PTCPOCINDEX_LINENUMBER].Substring(0, 2);
          if (str3 == "09" || str3 == "10")
          {
            if (str1 == string.Empty)
              this.totalOffsetPrepaidBorPaidPOC += num4;
            else
              this.totalOffset900And1000POCPaid += num4;
          }
          else if (str1 == string.Empty)
            this.totalOffsetNonPrepaidBorPaidPOC += num4;
          else
            this.totalOffsetNon900Non1000POCPaid += num4;
        }
        if (str1 == "Lender" || str2 == "Lender")
        {
          double num5 = 0.0 + (str2 == "Lender" ? this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_PTCAMT]) : 0.0);
          if (num5 == 0.0)
            return currentCreditLeft;
          if (currentCreditLeft == 0.0)
          {
            tLenderPaid += num5;
            return currentCreditLeft;
          }
          if (currentCreditLeft >= num5)
          {
            currentCreditLeft -= num5;
            if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_BORPAID] == "NEWHUD.X1151" || pocFields[HUDGFE2010Fields.PTCPOCINDEX_BORPAID] == "NEWHUD.X1155" || pocFields[HUDGFE2010Fields.PTCPOCINDEX_BORPAID] == "NEWHUD.X1159" || pocFields[HUDGFE2010Fields.PTCPOCINDEX_BORPAID] == "NEWHUD.X1163")
              tLenderPaid += num5;
          }
          else
          {
            tLenderPaid += num5 - currentCreditLeft;
            if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_BORPAID] == "NEWHUD.X1151" || pocFields[HUDGFE2010Fields.PTCPOCINDEX_BORPAID] == "NEWHUD.X1155" || pocFields[HUDGFE2010Fields.PTCPOCINDEX_BORPAID] == "NEWHUD.X1159" || pocFields[HUDGFE2010Fields.PTCPOCINDEX_BORPAID] == "NEWHUD.X1163")
              tLenderPaid += num5 - currentCreditLeft;
            currentCreditLeft = 0.0;
          }
        }
      }
      return currentCreditLeft <= 0.0 ? 0.0 : currentCreditLeft;
    }

    private double calculateLenderCharge(
      string paidByID,
      double currentCreditLeft,
      ref double tLenderPaid)
    {
      return !HUDGFE2010Fields.PAIDBYPOPTFIELDS.ContainsKey((object) paidByID) ? currentCreditLeft : this.calculateLenderCharge((string[]) HUDGFE2010Fields.PAIDBYPOPTFIELDS[(object) paidByID], currentCreditLeft, ref tLenderPaid);
    }

    private double calculateLenderCharge(
      string[] pocFields,
      double currentCreditLeft,
      ref double tLenderPaid)
    {
      string str1 = this.Val(pocFields[HUDGFE2010Fields.PTCPOCINDEX_POCPAIDBY]);
      string str2 = this.Val(pocFields[HUDGFE2010Fields.PTCPOCINDEX_PTCPAIDBY]);
      if (str1 == "Lender" || str2 == "Lender")
      {
        double num = (str1 == "Lender" ? this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_POCAMT]) : 0.0) + (str2 == "Lender" ? this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_PTCAMT]) : 0.0);
        if (currentCreditLeft == 0.0)
          tLenderPaid += num;
        if (currentCreditLeft >= num)
          currentCreditLeft -= num;
        else if (currentCreditLeft > 0.0)
        {
          tLenderPaid += num - currentCreditLeft;
          currentCreditLeft = 0.0;
        }
      }
      return currentCreditLeft <= 0.0 ? 0.0 : currentCreditLeft;
    }

    private double checkPrepaidCharge(string aprID, string paidByID)
    {
      if (this.Val(aprID) != "Y")
        return 0.0;
      string[] strArray = (string[]) HUDGFE2010Fields.PAIDBYPOPTFIELDS[(object) paidByID];
      double num = this.FltVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_BORTOPAY]);
      string str1 = this.Val(strArray[HUDGFE2010Fields.PTCPOCINDEX_POCPAIDBY]);
      if (str1 == string.Empty || (this.UseNewGFEHUD || this.UseNew2015GFEHUD) && (str1 == "Other" || !this.UsingTableFunded && str1 == "Broker"))
        num += this.FltVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_POCAMT]);
      string str2 = this.Val(strArray[HUDGFE2010Fields.PTCPOCINDEX_PTCPAIDBY]);
      if (str2 == string.Empty || (this.UseNewGFEHUD || this.UseNew2015GFEHUD) && (str2 == "Other" || !this.UsingTableFunded && str2 == "Broker"))
        num += this.FltVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_PTCAMT]);
      return num;
    }

    private double check2015PrepaidCharge(string[] pocPTCFields)
    {
      if (pocPTCFields[HUDGFE2010Fields.PTCPOCINDEX_APR] == "" || this.Val(pocPTCFields[HUDGFE2010Fields.PTCPOCINDEX_APR]) != "Y")
        return 0.0;
      double num = (pocPTCFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORPACAMT] != string.Empty ? this.FltVal(pocPTCFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORPACAMT]) : 0.0) + (this.FltVal(pocPTCFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORPOCAMT]) + this.FltVal(pocPTCFields[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPACAMT]) + this.FltVal(pocPTCFields[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPOCAMT]));
      if (!this.UsingTableFunded)
        num += this.FltVal(pocPTCFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPACAMT]) + this.FltVal(pocPTCFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPOCAMT]);
      return num;
    }

    private double addSellerPaidToBorrowerPaid(string borID, string gfeID, string selID)
    {
      double num1 = this.FltVal(borID);
      double num2 = this.FltVal(gfeID);
      return num1 == num2 || num2 == 0.0 || num1 == 0.0 && num2 == 0.0 ? 0.0 : this.FltVal(selID);
    }

    private double addSellerIntendedToLine1101(string borID, string gfeID, string selID)
    {
      double num1 = this.FltVal(borID);
      double num2 = this.FltVal(gfeID);
      return num1 == num2 || num2 == 0.0 || num1 == 0.0 && num2 == 0.0 ? this.FltVal(selID) : 0.0;
    }

    private void calculatePaidBy(
      string[] pocPTCFields,
      bool use2015RESPA,
      ref double totalBrokerPaid,
      ref double totalLenderPaid,
      ref double totalOtherPaid,
      ref double totalBorrowerPaid,
      ref double totalDisclosureX651,
      ref double totalDisclosureX652,
      ref double totalDisclosureX647,
      ref double totalDisclosureX648,
      ref double totalDisclosureX670,
      ref double totalFinancedAmt)
    {
      if (pocPTCFields == null)
        return;
      string str = this.Val(pocPTCFields[HUDGFE2010Fields.PTCPOCINDEX_PAIDTO]);
      if (use2015RESPA)
      {
        totalBorrowerPaid += this.FltVal(pocPTCFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORAMTPAID]);
        totalBrokerPaid += this.FltVal(pocPTCFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKAMTPAID]);
        if (!this.skipLenderObligatedFee(pocPTCFields[HUDGFE2010Fields.PTCPOCINDEX_LINENUMBER]))
          totalLenderPaid += this.FltVal(pocPTCFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERAMTPAID]);
        totalOtherPaid += this.FltVal(pocPTCFields[HUDGFE2010Fields.PTCPOCINDEX_2015OTHAMTPAID]);
        if (str == "Broker")
        {
          totalDisclosureX647 += this.FltVal(pocPTCFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORPACAMT]);
          totalDisclosureX648 += this.FltVal(pocPTCFields[HUDGFE2010Fields.PTCPOCINDEX_2015OTHAMTPAID]);
          totalDisclosureX651 += this.FltVal(pocPTCFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERAMTPAID]);
          totalDisclosureX652 += this.FltVal(pocPTCFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKAMTPAID]);
        }
        if (str == "Broker" || str == "Lender")
          totalDisclosureX670 += this.FltVal(pocPTCFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORPACAMT]);
        totalFinancedAmt += this.FltVal(pocPTCFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORFINANCEDAMT]);
      }
      else
      {
        double num1 = this.FltVal(pocPTCFields[HUDGFE2010Fields.PTCPOCINDEX_POCAMT]);
        if (num1 != 0.0)
        {
          switch (this.Val(pocPTCFields[HUDGFE2010Fields.PTCPOCINDEX_POCPAIDBY]))
          {
            case "Broker":
              totalBrokerPaid += num1;
              break;
            case "Lender":
              totalLenderPaid += num1;
              if (str == "Broker")
              {
                totalDisclosureX651 += num1;
                break;
              }
              break;
            case "Other":
              totalOtherPaid += num1;
              if (str == "Broker")
              {
                totalDisclosureX648 += num1;
                break;
              }
              break;
            default:
              totalBorrowerPaid += num1;
              if (str == "Broker")
              {
                totalDisclosureX652 += num1;
                break;
              }
              break;
          }
        }
        double num2 = this.FltVal(pocPTCFields[HUDGFE2010Fields.PTCPOCINDEX_PTCAMT]);
        if (num2 != 0.0)
        {
          switch (this.Val(pocPTCFields[HUDGFE2010Fields.PTCPOCINDEX_PTCPAIDBY]))
          {
            case "Broker":
              totalBrokerPaid += num2;
              break;
            case "Lender":
              if (str == "Broker")
                totalDisclosureX651 += num2;
              if (pocPTCFields[HUDGFE2010Fields.PTCPOCINDEX_PTCAMT] == "PTC.X90" && num2 < 0.0)
                num2 = 0.0;
              totalLenderPaid += num2;
              break;
            case "Other":
              if (str == "Broker")
                totalDisclosureX648 += num2;
              totalOtherPaid += num2;
              break;
            default:
              totalBorrowerPaid += num2;
              if (str == "Broker")
              {
                totalDisclosureX652 += num2;
                break;
              }
              break;
          }
        }
        double num3 = this.FltVal(pocPTCFields[HUDGFE2010Fields.PTCPOCINDEX_BORTOPAY]);
        if (pocPTCFields[HUDGFE2010Fields.PTCPOCINDEX_LINENUMBER] == "0802" && this.Val("NEWHUD.X1139") != "Y" && this.Val("NEWHUD.X715") == "Include Origination Credit")
          num3 = 0.0;
        totalBorrowerPaid += num3;
        if (str == "Broker")
        {
          totalDisclosureX652 += num3;
          if (num3 < 0.0 && pocPTCFields[HUDGFE2010Fields.PTCPOCINDEX_PAIDTO] != "NEWHUD.X627" || num3 > 0.0)
            totalDisclosureX647 += num3;
        }
        if ((str == "Broker" || str == "Lender") && (num3 < 0.0 && pocPTCFields[HUDGFE2010Fields.PTCPOCINDEX_PAIDTO] != "NEWHUD.X627" || num3 > 0.0))
          totalDisclosureX670 += num3;
        if (num3 == 0.0 || !(this.Val(pocPTCFields[HUDGFE2010Fields.PTCPOCINDEX_FINANCED]) == "Y"))
          return;
        totalFinancedAmt += num3;
      }
    }

    private bool isPaidToBroker(string paidToID)
    {
      return paidToID != string.Empty && this.Val(paidToID) == "Broker";
    }

    internal void CalculateFunder(string id, string val)
    {
      this.loan.Calculator.EditCalcOnDemandEnum(CalcOnDemandEnum.FundingFees, true);
      double num1 = 0.0;
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      string empty3 = string.Empty;
      string val1 = string.Empty;
      string val2 = string.Empty;
      bool flag1 = this.Val("NEWHUD.X1139") == "Y";
      bool flag2 = this.Val("LE2.X28") == "Y";
      string[] strArray = (string[]) null;
      double num2 = 0.0;
      double num3 = 0.0;
      double num4 = 0.0;
      double num5 = 0.0;
      double num6 = 0.0;
      double num7 = 0.0;
      double num8;
      for (int index1 = 0; index1 < GFEItemCollection.GFEItems2010.Count; ++index1)
      {
        GFEItem gfeItem = GFEItemCollection.GFEItems2010[index1];
        if (gfeItem.LineNumber < 2001 && (gfeItem.LineNumber >= 100 && !(gfeItem.For2015 == "Y") || this.UseNew2015GFEHUD) && (!this.UseNew2015GFEHUD || !GFEItemCollection.Excluded2015GFEItem.Contains(gfeItem.LineNumber)) && (!(this.UseNew2015GFEHUD & flag2) || gfeItem.LineNumber >= 520 || gfeItem.LineNumber >= 21 && gfeItem.LineNumber <= 45) && (!this.UseNew2015GFEHUD || flag2 || gfeItem.LineNumber < 21 || gfeItem.LineNumber > 45) && (!this.UseNew2015GFEHUD || (gfeItem.LineNumber != 1101 || !(gfeItem.ComponentID == "x")) && (gfeItem.LineNumber != 1102 || !(gfeItem.ComponentID == "")) && (gfeItem.LineNumber != 1002 || !(this.Val("NEWHUD2.X133") != "Y")) && (gfeItem.LineNumber != 1003 || !(this.Val("NEWHUD2.X4769") != "Y")) && (gfeItem.LineNumber != 1004 || !(this.Val("NEWHUD2.X134") != "Y")) && (gfeItem.LineNumber != 1005 || !(this.Val("NEWHUD2.X135") != "Y")) && (gfeItem.LineNumber != 1006 || !(this.Val("NEWHUD2.X136") != "Y")) && (gfeItem.LineNumber != 1007 || !(this.Val("NEWHUD2.X137") != "Y")) && (gfeItem.LineNumber != 1008 || !(this.Val("NEWHUD2.X138") != "Y")) && (gfeItem.LineNumber != 1009 || !(this.Val("NEWHUD2.X139") != "Y")) && (gfeItem.LineNumber != 1010 || !(this.Val("NEWHUD2.X140") != "Y"))) && (!flag1 || gfeItem.LineNumber != 802 || !(gfeItem.ComponentID == string.Empty)) && (flag1 || gfeItem.LineNumber != 802 || !(gfeItem.ComponentID != string.Empty)))
        {
          if (gfeItem.LineNumber == 802 && (this.UseNewGFEHUD || this.UseNew2015GFEHUD) && this.Val("NEWHUD.X713") == "Origination Charge")
          {
            if (flag1)
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
            else if (this.loan.GetField("NEWHUD.X715") == "Include Origination Credit")
              gfeItem = new GFEItem(801, "s", "", "1663", "", "NEWHUD.X749", "", "NEWHUD.X627", "NEWHUD.X398", "", "Origination Credit");
            else if (this.loan.GetField("NEWHUD.X715") == "Include Origination Points")
              gfeItem = new GFEItem(801, "s", "", "NEWHUD.X15", "NEWHUD.X788", "NEWHUD.X749", "", "NEWHUD.X627", "NEWHUD.X398", "NEWHUD.X790", "Origination Points");
          }
          string str1 = !(gfeItem.PaidByFieldID != "") ? string.Empty : this.Val(gfeItem.PaidByFieldID);
          if (gfeItem.LineNumber < 520)
          {
            if (str1 != "Credit" && str1 != "Debt")
              continue;
          }
          else if (!this.UseNewGFEHUD && !this.UseNew2015GFEHUD && gfeItem.POCFieldID != string.Empty && this.Val(gfeItem.POCFieldID) == "Y")
            continue;
          double num9;
          num7 = num9 = 0.0;
          num6 = num9;
          num5 = num9;
          num4 = num9;
          num3 = num9;
          num2 = num9;
          for (int index2 = 1; index2 <= 2; ++index2)
          {
            num8 = 0.0;
            if (gfeItem.LineNumber >= 520 || index2 != 2)
            {
              if (this.UseNewGFEHUD || this.UseNew2015GFEHUD || gfeItem.LineNumber != 824 && gfeItem.LineNumber != 825 || index2 < 2)
              {
                if ((this.UseNewGFEHUD || this.UseNew2015GFEHUD) && index2 == 1 && gfeItem.LineNumber >= 520)
                {
                  strArray = HUDGFE2010Fields.PAIDBYPOPTFIELDS.ContainsKey((object) gfeItem.PaidByFieldID) ? (string[]) HUDGFE2010Fields.PAIDBYPOPTFIELDS[(object) gfeItem.PaidByFieldID] : (string[]) null;
                  if (strArray == null && gfeItem.LineNumber != 802 && gfeItem.LineNumber != 1011)
                    continue;
                }
                string str2 = gfeItem.Description.Length <= 4 || gfeItem.Description.StartsWith("NEWHUD.X") || gfeItem.Description.StartsWith("NEWHUD2.X") || gfeItem.Description.StartsWith("CD3.X") ? this.Val(gfeItem.Description) : gfeItem.Description;
                string str3 = !(gfeItem.PTBFieldID != "") ? (this.UseNewGFEHUD || this.UseNew2015GFEHUD || gfeItem.LineNumber != 824 && gfeItem.LineNumber != 825 ? string.Empty : "Broker") : this.Val(gfeItem.PTBFieldID);
                if (str3 != "Broker")
                  str3 = !(gfeItem.BorrowerFieldID == "558") ? "Lender/Other" : string.Empty;
                if (index2 == 1)
                {
                  if (str1 == "" || gfeItem.LineNumber <= 520)
                    str1 = !(gfeItem.BorrowerFieldID == "558") ? "Borrower" : "";
                  if (gfeItem.BorrowerFieldID != string.Empty)
                  {
                    num8 = this.FltVal(gfeItem.BorrowerFieldID);
                    if (flag1 && (gfeItem.LineNumber == 802 && (gfeItem.ComponentID == "a" || gfeItem.ComponentID == "b" || gfeItem.ComponentID == "c" || gfeItem.ComponentID == "d") || gfeItem.LineNumber == 801 && (gfeItem.ComponentID == "s" || gfeItem.ComponentID == "t" || gfeItem.ComponentID == "u" || gfeItem.ComponentID == "v")))
                      num8 *= -1.0;
                    if (this.UseNew2015GFEHUD)
                    {
                      if (strArray != null)
                      {
                        double num10 = strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORPOCAMT] != "" ? this.FltVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORPOCAMT]) : 0.0;
                        if (num8 != 0.0)
                        {
                          num2 = strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPOCAMT] != "" ? this.FltVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPOCAMT]) : 0.0;
                          double num11 = strArray[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPOCAMT] != "" ? this.FltVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPOCAMT]) : 0.0;
                          double num12 = strArray[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPOCAMT] != "" ? this.FltVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPOCAMT]) : 0.0;
                          double num13 = strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPACAMT] != "" ? this.FltVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPACAMT]) : 0.0;
                          double num14 = strArray[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPACAMT] != "" ? this.FltVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPACAMT]) : 0.0;
                          double num15 = strArray[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPACAMT] != "" ? this.FltVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPACAMT]) : 0.0;
                          if (gfeItem.LineNumber == 801 && gfeItem.ComponentID == "f")
                            num8 -= num11;
                          else if (this.FltVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORAMTPAID]) == 0.0)
                            num8 = num13 + num14 + num15;
                          else
                            num8 -= num10 + num11 + num12;
                        }
                      }
                    }
                    else if (this.UseNewGFEHUD)
                    {
                      double num16 = strArray == null || !(strArray[HUDGFE2010Fields.PTCPOCINDEX_POCAMT] != string.Empty) ? 0.0 : this.FltVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_POCAMT]);
                      double num17 = strArray == null || !(strArray[HUDGFE2010Fields.PTCPOCINDEX_PTCAMT] != string.Empty) ? 0.0 : this.FltVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_PTCAMT]);
                      if (num16 != 0.0)
                        num8 -= num16;
                      if (num8 == 0.0)
                        continue;
                    }
                  }
                  if (num8 != 0.0)
                  {
                    if (val2 != string.Empty)
                      val2 += "\r\n";
                    val2 = val2 + gfeItem.LineNumber.ToString() + gfeItem.ComponentID + ".\t" + str2 + "\t\t" + str1 + "\t\t" + str3 + "\t\t$" + num8.ToString("N2");
                  }
                  if (gfeItem.CheckBorrowerFieldID != string.Empty && this.Val(gfeItem.CheckBorrowerFieldID) != "Y")
                    continue;
                }
                else
                {
                  str1 = "Seller";
                  if (gfeItem.SellerFieldID != string.Empty)
                    num8 = this.FltVal(gfeItem.SellerFieldID) - (!this.UseNew2015GFEHUD || strArray == null || !(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015SELPOCAMT] != string.Empty) ? 0.0 : this.FltVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015SELPOCAMT]));
                  if (num8 != 0.0)
                  {
                    if (val2 != string.Empty)
                      val2 += "\r\n";
                    val2 = val2 + gfeItem.LineNumber.ToString() + gfeItem.ComponentID + ".\t" + str2 + "\t\t" + str1 + "\t\t" + str3 + "\t\t$" + num8.ToString("N2");
                  }
                  if (gfeItem.CheckSellerFieldID != string.Empty && this.Val(gfeItem.CheckSellerFieldID) != "Y")
                    continue;
                }
                if (num8 != 0.0)
                {
                  if (gfeItem.LineNumber < 520 && gfeItem.PaidByFieldID != string.Empty && this.Val(gfeItem.PaidByFieldID) == "Credit")
                    num8 *= -1.0;
                  if (!flag1 && gfeItem.LineNumber == 801 && gfeItem.ComponentID == "s" && gfeItem.BorrowerFieldID == "1663")
                    num8 *= -1.0;
                  num1 += num8;
                  if (val1 != string.Empty)
                    val1 += "\r\n";
                  val1 = val1 + gfeItem.LineNumber.ToString() + gfeItem.ComponentID + ".\t" + str2 + "\t\t" + str1 + "\t\t" + str3 + "\t\t$" + num8.ToString("N2");
                }
              }
              else
                break;
            }
          }
        }
      }
      this.SetVal("2971", val1);
      this.SetVal("2972", val2);
      this.SetCurrentNum("1989", num1);
      num8 = (this.Val("1172") == "HELOC" ? this.FltVal("1888") : this.FltVal("2")) - num1 + this.FltVal("2005") + (this.UseNew2015GFEHUD ? this.FltVal("4083") : 0.0);
      this.SetCurrentNum("1990", num8);
    }

    private void copySpecialHUD2010ToGFE2010(string id, string val)
    {
      if (this.calObjs.CurrentFormID == "HUD1PG2_2010")
        return;
      string currentFormId = this.calObjs.CurrentFormID;
      this.calObjs.CurrentFormID = "REGZGFE_2010";
      this.CopyHUD2010ToGFE2010("572", "572", false);
      this.CopyHUD2010ToGFE2010("337", "337", false);
      this.CopyHUD2010ToGFE2010("642", "642", false);
      this.CopyHUD2010ToGFE2010("338", "1296", false);
      this.CopyHUD2010ToGFE2010("655", "1386", false);
      this.CopyHUD2010ToGFE2010("NEWHUD.X1708", "NEWHUD.X1708", false);
      if (this.Val("1750").IndexOf("Loan Amount") > -1)
        this.CopyHUD2010ToGFE2010("656", "1109", false);
      this.CopyHUD2010ToGFE2010("NEWHUD.X639", "NEWHUD.X639", false);
      this._gfeCalculationServant.UpdateCityStateUserFees(id, val);
      this.calObjs.CurrentFormID = currentFormId;
    }

    public void CopyHUD2010ToGFE2010()
    {
      this.CopyHUD2010ToGFE2010((string) null, (string) null, false);
    }

    public void CopyHUD2010ToGFE2010(string sourceID, string currentID, bool blankCopyOnly)
    {
      this._newHudCalculationServant.CopyHud2010ToGfe2010(sourceID, currentID, blankCopyOnly);
    }

    public void CopyGFE2010ToHUD2010()
    {
      foreach (string[] strArray in HUDGFE2010Fields.HUD2010ToGFE2010FIELDMAP)
        this.SetCurrentNum(strArray[1], this.FltVal(strArray[0]));
      this.FormCal((string) null, (string) null);
    }

    internal void VerifyingPOCPTCFields(string id, string val)
    {
      if (this.IsPOCPTCVerified)
        return;
      if (this.UseNew2015GFEHUD)
      {
        this.verify2015PTCPOCFields(id, val);
      }
      else
      {
        bool flag = false;
        if (this.LOCompensationSetting != null && this.LOCompensationSetting.EnableLOCompensationRule((IHtmlInput) this.loan))
          flag = true;
        string empty = string.Empty;
        string str1 = string.Empty;
        for (int index = 0; index < HUDGFE2010Fields.WHOLEPOC_FIELDS.Count; ++index)
        {
          string[] strArray = HUDGFE2010Fields.WHOLEPOC_FIELDS[index];
          if (!(strArray[HUDGFE2010Fields.PTCPOCINDEX_LINENUMBER] == "2001"))
          {
            if (!(strArray[HUDGFE2010Fields.PTCPOCINDEX_FOR2015] == "Y"))
            {
              string val1 = this.Val(strArray[HUDGFE2010Fields.PTCPOCINDEX_PAIDBY]);
              if (val1 == "Seller")
              {
                this.SetVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_PAIDBY], "");
                val1 = "";
              }
              if (this.Val(strArray[HUDGFE2010Fields.PTCPOCINDEX_POCPAIDBY]) == "Seller")
                this.SetVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_POCPAIDBY], "");
              double num1 = this.FltVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_BORPAID]);
              if (strArray[HUDGFE2010Fields.PTCPOCINDEX_BORPAID] == "NEWHUD.X15" && num1 == 0.0 && this.Val("NEWHUD.X1139") != "Y" && this.Val("NEWHUD.X715") == "Include Origination Credit")
              {
                num1 = this.FltVal("1663") * -1.0;
                this.SetVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_APR], "");
              }
              double num2 = strArray[HUDGFE2010Fields.PTCPOCINDEX_POCAMT] != string.Empty ? this.FltVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_POCAMT]) : 0.0;
              double num3 = this.FltVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_PTCAMT]);
              if (num1 > 0.0)
              {
                if (id == strArray[HUDGFE2010Fields.PTCPOCINDEX_PAIDBY])
                {
                  if (val != "" && num1 >= num3 + num2)
                  {
                    num3 = num1 - num2;
                    this.SetVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_PTC], "Y");
                    this.SetCurrentNum(strArray[HUDGFE2010Fields.PTCPOCINDEX_PTCAMT], num3);
                    this.SetVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_PTCPAIDBY], val);
                  }
                  else if (val == string.Empty)
                  {
                    this.SetVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_PTC], "");
                    this.SetVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_PTCAMT], "");
                    this.SetVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_PTCPAIDBY], "");
                    num3 = 0.0;
                  }
                }
                if (num2 > num1)
                {
                  num2 = num1;
                  num3 = 0.0;
                  if (strArray[HUDGFE2010Fields.PTCPOCINDEX_POC] != string.Empty)
                  {
                    this.SetVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_POC], "Y");
                    this.SetCurrentNum(strArray[HUDGFE2010Fields.PTCPOCINDEX_POCAMT], num2);
                    this.SetVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_POCPAIDBY], val1);
                  }
                  this.SetVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_PTC], "");
                  this.SetVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_PTCAMT], "");
                  this.SetVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_PTCPAIDBY], "");
                }
                else if (num3 + num2 > num1)
                {
                  num3 = num1 - num2;
                  if (num3 > 0.0)
                  {
                    this.SetVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_PTC], "Y");
                    this.SetCurrentNum(strArray[HUDGFE2010Fields.PTCPOCINDEX_PTCAMT], num3);
                  }
                  else
                  {
                    this.SetVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_PTC], "");
                    this.SetVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_PTCAMT], "");
                    this.SetVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_PTCPAIDBY], "");
                  }
                }
                else if (num3 == num1)
                {
                  this.SetVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_PTC], "Y");
                  this.SetCurrentNum(strArray[HUDGFE2010Fields.PTCPOCINDEX_PTCAMT], num3);
                  this.SetVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_PTCPAIDBY], val1);
                  if (strArray[HUDGFE2010Fields.PTCPOCINDEX_POC] != string.Empty)
                  {
                    this.SetVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_POC], "");
                    this.SetVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_POCAMT], "");
                    this.SetVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_POCPAIDBY], "");
                  }
                  if (val1 == string.Empty)
                  {
                    val1 = "Lender";
                    this.SetVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_PAIDBY], val1);
                  }
                }
                else if (num1 > 0.0 && (num2 > 0.0 || num3 > 0.0) && Utils.ArithmeticRounding(num2 + num3, 2) < num1 && val1 != string.Empty && (strArray == null || strArray[HUDGFE2010Fields.PTCPOCINDEX_LINENUMBER] != "0801" && strArray[HUDGFE2010Fields.PTCPOCINDEX_COMPONENTID] != "f"))
                {
                  val1 = "";
                  this.SetVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_PAIDBY], val1);
                }
              }
              else if (num1 < 0.0)
              {
                num2 = 0.0;
                if (strArray[HUDGFE2010Fields.PTCPOCINDEX_POC] != string.Empty)
                {
                  this.SetVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_POC], "");
                  this.SetVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_POCAMT], "");
                  this.SetVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_POCPAIDBY], "");
                }
                if (id == strArray[HUDGFE2010Fields.PTCPOCINDEX_PAIDBY])
                {
                  if (val == "")
                  {
                    this.SetVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_PTC], "");
                    this.SetVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_PTCAMT], "");
                    this.SetVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_PTCPAIDBY], "");
                    num3 = 0.0;
                  }
                  else
                  {
                    this.SetVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_PTC], "Y");
                    this.SetCurrentNum(strArray[HUDGFE2010Fields.PTCPOCINDEX_PTCAMT], num1);
                    this.SetVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_PTCPAIDBY], val1);
                    this.SetVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_BORTOPAY], "");
                    num3 = num1;
                  }
                }
              }
              else
              {
                if (strArray[HUDGFE2010Fields.PTCPOCINDEX_POC] != string.Empty)
                {
                  this.SetVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_POC], "");
                  this.SetVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_POCAMT], "");
                  this.SetVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_POCPAIDBY], "");
                }
                this.SetVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_PTCPOC], "");
                this.SetVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_PTC], "");
                this.SetVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_PTCAMT], "");
                this.SetVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_PTCPAIDBY], "");
                this.SetVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_BORTOPAY], "");
                this.SetVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_FINANCED], "");
                if (strArray[HUDGFE2010Fields.PTCPOCINDEX_BORPAID] == "NEWHUD.X225" && (this.Val("NEWHUD.X223") != string.Empty || this.Val("NEWHUD.X224") != string.Empty))
                  this.SetVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_PAIDBY], "Lender");
                else
                  this.SetVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_PAIDBY], "");
                val1 = "";
                num2 = 0.0;
                num3 = 0.0;
              }
              if (strArray[HUDGFE2010Fields.PTCPOCINDEX_BORPAID] == "NEWHUD.X225" && num1 > 0.0)
              {
                num3 = num1 - num2;
                this.SetVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_PTC], "Y");
                this.SetCurrentNum(strArray[HUDGFE2010Fields.PTCPOCINDEX_PTCAMT], num3);
                this.SetVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_PTCPAIDBY], "Lender");
                this.SetVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_PTCPOC], "Y");
              }
              string str2 = strArray[HUDGFE2010Fields.PTCPOCINDEX_POCPAIDBY] != string.Empty ? this.Val(strArray[HUDGFE2010Fields.PTCPOCINDEX_POCPAIDBY]) : "";
              str1 = this.Val(strArray[HUDGFE2010Fields.PTCPOCINDEX_PTCPAIDBY]);
              if (val1 != string.Empty && num2 == 0.0 && num3 == 0.0)
              {
                string val2 = val1;
                num3 = num1;
                this.SetVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_PTC], "Y");
                this.SetCurrentNum(strArray[HUDGFE2010Fields.PTCPOCINDEX_PTCAMT], num3);
                this.SetVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_PTCPAIDBY], val2);
              }
              if (flag && this.LOCompensationSetting.IsPaidByFieldCompensationEnabled(strArray[HUDGFE2010Fields.PTCPOCINDEX_PAIDBY], (IHtmlInput) this.loan))
              {
                if (val1 == string.Empty)
                {
                  this.SetVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_PTC], "");
                  this.SetVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_PTCAMT], "");
                  this.SetVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_PTCPAIDBY], "");
                  if (num2 != 0.0)
                    this.SetVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_POCPAIDBY], "");
                }
                else
                {
                  if (num2 != 0.0 && str2 == string.Empty)
                    this.SetVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_POCPAIDBY], val1);
                  num3 = num1 - num2;
                  if (num3 != 0.0)
                  {
                    this.SetVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_PTC], "Y");
                    this.SetCurrentNum(strArray[HUDGFE2010Fields.PTCPOCINDEX_PTCAMT], num3);
                    this.SetVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_PTCPAIDBY], val1);
                  }
                  this.SetVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_BORTOPAY], "");
                }
              }
              double num4 = num1 - num2 - num3;
              this.SetCurrentNum(strArray[HUDGFE2010Fields.PTCPOCINDEX_BORTOPAY], num4);
              if (num3 != 0.0 || num2 != 0.0)
                this.SetVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_PTCPOC], "Y");
              else
                this.SetVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_PTCPOC], "");
            }
          }
          else
            break;
        }
        this.IsPOCPTCVerified = true;
      }
    }

    private void verify2015PTCPOCFields(string id, string val)
    {
      this.calObjs.NewHud2015FeeDetailCal.Calc_2015AllFeeDetails(id, val);
      this.IsPOCPTCVerified = true;
    }

    private void calculateGFEApplicationDate(string id, string val)
    {
      bool flag = true;
      AlertConfig alertConfig = this.loan.Settings.AlertSetupData.GetAlertConfig(22);
      foreach (AlertTriggerField triggerField in (List<AlertTriggerField>) ((RegulationAlert) alertConfig.Definition).TriggerFields)
      {
        string id1 = triggerField.FieldID;
        if (triggerField.FieldID == "11" && this.USEURLA2020)
          id1 = "URLA.X73";
        string simpleField = this.loan.GetSimpleField(id1);
        if (!(triggerField.FieldID == "MORNET.X40"))
        {
          if (triggerField.FieldID == "736" && simpleField == "")
          {
            if ((this.loan.GetField("19") == "NoCash-Out Refinance" || this.loan.GetField("19") == "Cash-Out Refinance") && this.loan.GetSimpleField("1172") != "VA")
            {
              if (!(this.loan.GetSimpleField("1172") == "FHA") && !(this.loan.GetSimpleField("1172") == "FarmersHomeAdministration") || !(this.loan.GetSimpleField("MORNET.X40") == "StreamlineWithoutAppraisal") && !(this.loan.GetSimpleField("MORNET.X40") == "StreamlineWithAppraisal"))
              {
                flag = false;
                break;
              }
            }
            else if (this.loan.GetSimpleField("1172") == "VA")
            {
              if (!(this.loan.GetSimpleField("958") == "IRRRL") && !(this.loan.GetSimpleField("MORNET.X40") == "InterestRateReductionRefinanceLoan"))
              {
                flag = false;
                break;
              }
            }
            else
            {
              flag = false;
              break;
            }
          }
          else
          {
            if (simpleField == "")
            {
              flag = false;
              break;
            }
            if ((id1 == "11" || id1 == "URLA.X73") && simpleField.Trim().StartsWith("TBD", StringComparison.CurrentCultureIgnoreCase))
            {
              flag = false;
              break;
            }
          }
        }
      }
      if (alertConfig.TriggerFieldList != null & flag)
      {
        foreach (string triggerField in alertConfig.TriggerFieldList)
        {
          if (EncompassFields.GetField(triggerField, this.loan.Settings.FieldSettings) != null && this.loan.GetSimpleField(triggerField) == "")
          {
            flag = false;
            break;
          }
        }
      }
      if (!flag)
        return;
      if (Utils.ParseDate((object) this.loan.GetFieldFromCal("3142")) == DateTime.MinValue)
        this.loan.SetFieldFromCal("3142", DateTime.Today.ToString("MM/dd/yyyy"));
      this.enforceGFEAppDateRule(id, val);
    }

    private void enforceGFEAppDateRule(string id, string val)
    {
      if (!this.IsLocked("3142"))
        return;
      DateTime date1 = Utils.ParseDate((object) this.loan.GetFieldFromCal("3142"));
      DateTime date2 = Utils.ParseDate((object) this.loan.GetField("3142"));
      if (!(date1 != DateTime.MinValue) || !(date2 == DateTime.MinValue) && !(date2.Date > date1.Date))
        return;
      this.loan.SetField("3142", date1.ToString("MM/dd/yyyy"));
    }

    private void calculateInitialDisclosureDueDate(string id, string val)
    {
      string simpleField = this.loan.GetSimpleField("3142");
      if (!Utils.IsDate((object) simpleField))
      {
        this.loan.SetFieldFromCal("3143", "");
      }
      else
      {
        DateTime date = Utils.ParseDate((object) simpleField);
        bool complianceSetting = (bool) this.loan.Settings.ComplianceSettings[(object) "Compliance.CountDay0ForDisclosureDate"];
        try
        {
          this.loan.SetFieldFromCal("3143", this.sessionObjects.ConfigurationManager.AddBusinessDays(CalendarType.Business, date, complianceSetting ? 2 : 3, false).ToString("MM/dd/yyyy"));
        }
        catch (Exception ex)
        {
          if (ex.InnerException is ComplianceCalendarException)
          {
            this.loan.SetFieldFromCal("3143", "");
            RemoteLogger.Write(TraceLevel.Warning, "calculateInitialDisclosureDueDate ComplianceCalendarException - FieldID 3143 is set to empty. Loan Guid: " + this.loan.GUID + " Exception : " + ex.ToString());
            throw new ComplianceCalendarException((ComplianceCalendarException) ex.InnerException, "3143");
          }
          RemoteLogger.Write(TraceLevel.Warning, "calculateInitialDisclosureDueDate Exception - FieldID 3143 is set to empty. Loan Guid: " + this.loan.GUID + " Exception : " + ex.ToString());
          throw ex;
        }
      }
    }

    private void calculateGFERedisclosureFlag(string id, string val)
    {
    }

    private void calculateRevisedGFEDueDate(string id, string val)
    {
      this.CalculateRevisedDueDate("3165", "3167", (bool) this.loan.Settings.ComplianceSettings[(object) "Compliance.CountDay0ForRediscloseDueDate"], true, true);
    }

    private void calculateRevisedCDDueDate(string id, string val)
    {
      this.CalculateRevisedDueDate("CD1.X62", "CD1.X63", (bool) this.loan.Settings.ComplianceSettings[(object) "Compliance.CountDay0ForRediscloseDueDate"], true, true);
    }

    internal string CalculateRevisedDueDate(
      string changeReceivedDateFieldID,
      string revisedDueDateFieldID,
      bool includeDayChange,
      bool setField,
      bool showError)
    {
      DateTime date = Utils.ParseDate((object) this.Val(changeReceivedDateFieldID), DateTime.MinValue);
      if (date == DateTime.MinValue)
      {
        if (setField)
          this.SetVal(revisedDueDateFieldID, "");
        return "";
      }
      try
      {
        DateTime dateTime = this.sessionObjects.ConfigurationManager.AddBusinessDays(CalendarType.Business, date, includeDayChange ? 2 : 3, false);
        if (setField)
          this.SetVal(revisedDueDateFieldID, dateTime.ToString("MM/dd/yyyy"));
        return dateTime.ToString("MM/dd/yyyy");
      }
      catch (Exception ex)
      {
        if (ex.InnerException is ComplianceCalendarException)
        {
          if (setField)
            this.SetVal(revisedDueDateFieldID, "");
          if (showError)
            throw new ComplianceCalendarException((ComplianceCalendarException) ex.InnerException, "CD1.X63");
        }
        else if (showError)
          throw ex;
        return "";
      }
    }

    public void CalculateBorrowerIntentToProceedDate()
    {
      if (this.loan.GetField("3164") == "Y")
        this.SetVal("3197", DateTime.Today.ToString("MM/dd/yyyy"));
      else
        this.SetVal("3197", "//");
    }

    private void calculateEarliestFeeCollectionDate(string id, string val)
    {
      this.CalculateEarliestFeeCollectionDate();
    }

    public void CalculateEarliestFeeCollectionDate()
    {
      if (this.loan.GetField("3164") != "Y")
      {
        this.loan.SetFieldFromCal("3145", "//");
      }
      else
      {
        DateTime dateTime1 = new DateTime();
        DateTime dateTime2 = Utils.CheckIf2015RespaTila(this.loan.GetField("3969")) ? this.calObjs.NewHud2015Cal.GetEarliestFeeCollectionDate() : this.GetInitialBorrowerReceivedDate();
        if (dateTime2 == DateTime.MinValue)
        {
          this.loan.SetField("3145", "//");
        }
        else
        {
          DateTime date = Utils.ParseDate((object) this.Val("3197"), DateTime.MinValue);
          if (dateTime2 > date)
            this.loan.SetFieldFromCal("3145", dateTime2.ToString("MM/dd/yyyy"));
          else
            this.loan.SetFieldFromCal("3145", date.ToString("MM/dd/yyyy"));
        }
      }
    }

    public DateTime GetInitialBorrowerReceivedDate()
    {
      string field1 = this.loan.GetField("3149");
      string field2 = this.loan.GetField("3153");
      if (field1 == "" || !Utils.IsDate((object) field1) || field2 == "" || !Utils.IsDate((object) field2))
        return DateTime.MinValue;
      DisclosureTrackingLog disclosureTrackingLog1 = this.loan.GetLogList().GetInitialDisclosureTrackingLog(DisclosureTrackingLog.DisclosureTrackingType.GFE);
      DisclosureTrackingLog disclosureTrackingLog2 = this.loan.GetLogList().GetInitialDisclosureTrackingLog(DisclosureTrackingLog.DisclosureTrackingType.TIL);
      DateTime date1 = DateTime.Parse(field1);
      DateTime date2 = DateTime.Parse(field2);
      if (disclosureTrackingLog1 != null && disclosureTrackingLog1.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.ByMail)
        date1 = this.sessionObjects.ConfigurationManager.AddBusinessDays(CalendarType.Postal, date1, 1, true);
      if (disclosureTrackingLog2 != null && disclosureTrackingLog2.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.ByMail)
        date2 = this.sessionObjects.ConfigurationManager.AddBusinessDays(CalendarType.Postal, date2, 1, true);
      return date1 > date2 ? date1 : date2;
    }

    private void calculateHUDGFEDisclosureInfo(string id, string val)
    {
      GFEDaysToExpireException toExpireException = (GFEDaysToExpireException) null;
      ComplianceCalendarException calendarException = (ComplianceCalendarException) null;
      if (id == "3164" && !this.UseNew2015GFEHUD)
      {
        this.CalculateBorrowerIntentToProceedDate();
        this.CalculateEarliestFeeCollectionDate();
      }
      if (this.Val("3168") == "N")
      {
        this.SetVal("3169", "");
        this.SetVal("3166", "");
        this.loan.RemoveLock("3165");
        this.loan.SetFieldFromCal("3165", "//");
        this.SetVal("3167", "//");
        this.SetVal("3627", "");
      }
      else if (this.Val("3168") == "Y" && this.loan.GetFieldFromCal("3165") == "//")
        this.loan.SetFieldFromCal("3165", DateTime.Today.ToString("MM/dd/yyyy"));
      if (this.Val("2400") == "Y")
        this.SetVal("3140", this.Val("762"));
      else if (this.Val("3164") != "Y")
        this.SetVal("3140", this.Val("NEWHUD.X2"));
      else if (this.Val("762") == "")
        this.SetVal("3140", "");
      else
        this.SetVal("3140", this.Val("762"));
      if (id != "3168")
        this.calculateHUDGFE(id, val);
      if (toExpireException != null)
        throw toExpireException;
      if (calendarException != null)
        throw calendarException;
    }

    private void calculateHUD1ToleranceLimits(string id, string val)
    {
      this._newHudCalculationServant.calculateHUD1ToleranceLimits();
    }

    private void calculateGoodGFEDate(string id, string val)
    {
      GFEDaysToExpireException toExpireException = (GFEDaysToExpireException) null;
      ComplianceCalendarException calendarException = (ComplianceCalendarException) null;
      bool complianceSetting = (bool) this.loan.Settings.ComplianceSettings[(object) "Compliance.CountDay0ForGFEExpirationDate"];
      DateTime date = Utils.ParseDate((object) this.Val("3170"), DateTime.MinValue);
      if (date == DateTime.MinValue && this.Val("3164") != "Y")
      {
        this.SetVal("NEWHUD.X2", "//");
      }
      else
      {
        DateTime dateTime1 = DateTime.MinValue;
        try
        {
          if (!(date != DateTime.MinValue))
            throw new ComplianceCalendarException(date, complianceSetting ? this.gfeDaystoExpire - 1 : this.gfeDaystoExpire);
          dateTime1 = this.sessionObjects.ConfigurationManager.AddBusinessDays(CalendarType.Business, date, complianceSetting ? this.gfeDaystoExpire - 1 : this.gfeDaystoExpire, false);
        }
        catch (Exception ex)
        {
          if (ex.InnerException is ComplianceCalendarException)
            calendarException = new ComplianceCalendarException((ComplianceCalendarException) ex.InnerException, "NEWHUD.X2");
          else
            calendarException = ex is ComplianceCalendarException ? new ComplianceCalendarException((ComplianceCalendarException) ex, "NEWHUD.X2") : throw ex;
        }
        DateTime dateTime2 = dateTime1;
        if (this.gfeDaystoExpire != 10 || dateTime1 == DateTime.MinValue)
        {
          try
          {
            if (!(date != DateTime.MinValue))
              throw new ComplianceCalendarException(date, complianceSetting ? 9 : 10);
            dateTime2 = this.sessionObjects.ConfigurationManager.AddBusinessDays(CalendarType.Business, date, complianceSetting ? 9 : 10, false);
            if (dateTime1 == DateTime.MinValue)
              dateTime1 = dateTime2;
          }
          catch (Exception ex)
          {
            if (ex.InnerException is ComplianceCalendarException)
            {
              if (calendarException == null)
                calendarException = new ComplianceCalendarException((ComplianceCalendarException) ex.InnerException, "NEWHUD.X2");
            }
            else
            {
              if (!(ex is ComplianceCalendarException))
                throw ex;
              if (calendarException == null)
                calendarException = new ComplianceCalendarException((ComplianceCalendarException) ex, "NEWHUD.X2");
            }
          }
        }
        if (this.Val("3164") != "Y")
        {
          if (this.IsLocked("NEWHUD.X2"))
          {
            this.loan.SetCurrentFieldFromCal("NEWHUD.X2", dateTime1.ToString("MM/dd/yyyy"));
            if (Utils.ParseDate((object) this.Val("NEWHUD.X2"), DateTime.MinValue) < dateTime2)
            {
              this.loan.RemoveLock("NEWHUD.X2");
              toExpireException = new GFEDaysToExpireException("The disclosed GFE must be available to borrower at least 10 days.");
            }
          }
          else if (dateTime1 != DateTime.MinValue)
            this.SetVal("NEWHUD.X2", dateTime1.ToString("MM/dd/yyyy"));
        }
        if (toExpireException != null)
          throw toExpireException;
        if (calendarException != null && !this.IsLocked("NEWHUD.X2"))
          throw calendarException;
      }
    }

    private void calcGFERedisclosureFlag(string id, string val)
    {
      if (Utils.CheckIf2015RespaTila(this.loan.GetField("3969")))
        return;
      DateTime date1 = Utils.ParseDate((object) this.loan.GetSimpleField("761"));
      DateTime date2 = Utils.ParseDate((object) this.loan.GetSimpleField("3137"));
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
        DisclosureTrackingLog disclosureTrackingLog = this.loan.GetLogList().GetLatestDisclosureTrackingLog(DisclosureTrackingLog.DisclosureTrackingType.GFE);
        DateTime date3 = Utils.ParseDate((object) this.loan.GetSimpleField("3200"));
        if (disclosureTrackingLog == null)
          this.SetVal("3201", "N");
        else if (date3 == DateTime.MinValue)
          this.SetVal("3201", "N");
        else if (date3 > Utils.ParseDate((object) Utils.DateTimeToUTCString(disclosureTrackingLog.DateAdded)))
          this.SetVal("3201", "Y");
        else
          this.SetVal("3201", "N");
      }
    }

    internal bool UsingTableFunded
    {
      set => this.usingTableFunded = value;
      get => this.usingTableFunded;
    }

    internal string GetAllChangeCircumstance()
    {
      int changeOfCircumstance = this.loan.GetNumberOfGoodFaithChangeOfCircumstance();
      if (this.changeCircumstanceOptions == null)
        this.getFirstChangeCircumstance(0);
      if (this.changeCircumstanceOptions == null)
        return string.Empty;
      List<string> stringList1 = new List<string>();
      for (int index = 1; index <= changeOfCircumstance; ++index)
      {
        string str = this.Val("XCOC" + index.ToString("00") + "05");
        if (!string.IsNullOrEmpty(str) && !stringList1.Contains(str))
          stringList1.Add(str);
      }
      if (stringList1.Count == 0)
        return string.Empty;
      List<string> stringList2 = new List<string>();
      for (int index = 0; index < this.changeCircumstanceOptions.Count; ++index)
      {
        if (stringList1.Contains(this.changeCircumstanceOptions[index][1]) && !stringList2.Contains(this.changeCircumstanceOptions[index][0]))
          stringList2.Add(this.changeCircumstanceOptions[index][0]);
      }
      if (stringList2.Count == 0)
        return string.Empty;
      string changeCircumstance = "";
      for (int index = 0; index < stringList2.Count; ++index)
        changeCircumstance = changeCircumstance + (changeCircumstance != "" ? "," : "") + stringList2[index];
      return changeCircumstance;
    }

    private string getFirstChangeCircumstance(int index)
    {
      if (this.changeCircumstanceOptions == null)
      {
        List<ChangeCircumstanceSettings> circumstanceSettings1 = this.sessionObjects.ConfigurationManager.GetAllChangeCircumstanceSettings();
        List<string[]> strArrayList = new List<string[]>();
        foreach (ChangeCircumstanceSettings circumstanceSettings2 in circumstanceSettings1)
        {
          string[] strArray = new string[4]
          {
            circumstanceSettings2.Code,
            circumstanceSettings2.Description,
            circumstanceSettings2.Comment,
            circumstanceSettings2.Reason.ToString()
          };
          strArrayList.Add(strArray);
        }
        this.changeCircumstanceOptions = strArrayList;
      }
      return this.changeCircumstanceOptions == null || this.changeCircumstanceOptions.Count <= 0 ? string.Empty : this.changeCircumstanceOptions[0][index];
    }

    private void calculateTotalLenderCredits(double totalLenderPaid)
    {
      string[] strArray = new string[2]
      {
        "Lender Credit",
        "LenderCredit"
      };
      double num1 = 0.0;
      bool flag = string.IsNullOrEmpty(this.Val("1393")) || this.Val("1393") == "Active Loan";
      if (this.Val("4796") == "Y")
      {
        num1 += this.FltVal("4794");
      }
      else
      {
        if (string.Compare(this.Val("202"), strArray[0], true) == 0 || string.Compare(this.Val("202"), strArray[1], true) == 0)
          num1 += this.FltVal("141");
        if (string.Compare(this.Val("1091"), strArray[0], true) == 0 || string.Compare(this.Val("1091"), strArray[1], true) == 0)
          num1 += this.FltVal("1095");
        if (string.Compare(this.Val("1106"), strArray[0], true) == 0 || string.Compare(this.Val("1106"), strArray[1], true) == 0)
          num1 += this.FltVal("1115");
        if (string.Compare(this.Val("1646"), strArray[0], true) == 0 || string.Compare(this.Val("1646"), strArray[1], true) == 0)
          num1 += this.FltVal("1647");
      }
      double num2 = this.FltVal("NEWHUD.X1144") + this.FltVal("NEWHUD.X1146") + this.FltVal("NEWHUD.X1148");
      double num3 = this.FltVal("NEWHUD.X225");
      double num4 = 0.0;
      if (this.Val("NEWHUD2.X4780") != "Y")
        num4 += this.Val("SYS.X285") == "Lender" ? this.FltVal("554") : 0.0;
      if (this.Val("NEWHUD2.X4781") != "Y")
        num4 += this.Val("NEWHUD.X162") == "Lender" ? this.FltVal("NEWHUD.X657") : 0.0;
      if (this.Val("NEWHUD2.X4782") != "Y")
        num4 += this.Val("NEWHUD.X1606") == "Lender" ? this.FltVal("NEWHUD.X1604") : 0.0;
      if (this.Val("NEWHUD2.X4783") != "Y")
        num4 += this.Val("NEWHUD.X1614") == "Lender" ? this.FltVal("NEWHUD.X1612") : 0.0;
      if (this.Val("NEWHUD2.X4784") != "Y")
        num4 += this.Val("NEWHUD.X1622") == "Lender" ? this.FltVal("NEWHUD.X1620") : 0.0;
      if (this.Val("NEWHUD2.X4785") != "Y")
        num4 += this.Val("NEWHUD.X1629") == "Lender" ? this.FltVal("NEWHUD.X1627") : 0.0;
      totalLenderPaid -= num3 + num4;
      if (num2 >= totalLenderPaid)
      {
        double num5 = num1 + num2;
        this.SetCurrentNum("LE2.XLC", Utils.ArithmeticRounding(num5, 0));
        this.SetCurrentNum("LE2.XLCDV", num5);
        this.SetCurrentNum("CD2.XSTLC", num1 + num2 - totalLenderPaid + this.FltVal("CD3.X129"));
      }
      else
      {
        double num6 = num1 + totalLenderPaid;
        this.SetCurrentNum("LE2.XLC", Utils.ArithmeticRounding(num6, 0));
        this.SetCurrentNum("LE2.XLCDV", num6);
        this.SetCurrentNum("CD2.XSTLC", num1 + this.FltVal("CD3.X129"));
      }
      if (this.loan.IsLocked("4083"))
        return;
      if (flag)
        this.SetCurrentNum("4083", this.FltVal("CD2.XSTLC") + this.FltVal("LENPCC"));
      else
        this.loan.AddLock("4083");
    }
  }
}
