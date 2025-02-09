// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.CalculationEngine.RegzCalculation
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.CalculationEngine
{
  internal class RegzCalculation : CalculationBase
  {
    private const string className = "RegzCalculation�";
    private static readonly string sw = Tracing.SwDataEngine;
    private SessionObjects sessionObjects;
    private ILoanConfigurationInfo configInfo;
    internal Routine CalcAPR;
    internal Routine BuildPaySchedule;
    internal Routine CalcIRS1098;
    internal Routine CalcREGZSummary;
    internal Routine CalcARMDisclosureSample;
    internal EventHandler CurrentAPRChanged;
    internal Routine CalcPMIMidpointCancelationDate;
    internal Routine CalcFloorRate;
    internal Routine CalcFirstAdjustmentMinimum;
    internal Routine CalcLateFeePaymentInRegz;
    internal Routine CalcLateExemptRightRescissionFlag;
    internal Routine CalcOnDemand;
    internal Routine CalcConstructionPhaseDisclosedSeparately;
    internal Routine CalcLotLandStatus;
    internal Routine CalcConstructionDates;
    internal Routine CalcInitialInterestRate;
    internal Routine CalDisclosureLog2015;
    internal Routine SychConstructionRateNoteRate;
    internal Routine CalculateHELOCPayment;
    internal Routine CalcHELOCInitialPayment;
    internal Routine CleanBuydownFields;
    internal Routine CalcBuydownSummary;
    internal Routine CalcMaturityDate;
    internal Routine CalcBuydownIndicator;
    internal Routine CalcDailyInterest;
    internal Routine CalcLenderRepresentative;
    internal Routine UpdateLenderRepresentative;
    internal Routine CalcRoleName;
    internal Routine SetLenderRepFromUserId;
    internal Routine CalculateMersOriginalMortgagee;
    internal Routine CalcZeroPercentPaymentOption;
    internal Routine CalcPointsInInitialAdjustedRate;
    internal Routine CleareSignCustomizeIndicator;
    internal Routine ReCalculateLenderRep;
    internal Routine SeteSignerCuztomizeIndicator;
    private double totalPrincipalIn5Years;
    private double totalInterestIn5Years;
    private double totalMIIn5Years;
    private double outstandingBalanceAfter1stAdjustment;
    private double mortgageInsuranceAfter1stAdjustment;
    private double highestLoanRate;
    private double maximumPayment;
    private double lowestLoanRate;
    private double f_le1x24_cd4x34;
    private bool useWorstCaseScenario;
    private bool useInterimServicingScenario;
    private bool useBestCaseScenario;
    private const int MAXPAYMENTS = 1300;
    private PaymentSchedule[] paySchedule = new PaymentSchedule[1300];
    private PaymentSchedule[] payFHASchedule = new PaymentSchedule[1300];
    private Dictionary<string, string> cacheFieldValues = new Dictionary<string, string>();
    private RegzCalculation.loanInformation loanInfo;
    private static Dictionary<string, int> helocInitialfields = new Dictionary<string, int>()
    {
      {
        "4476",
        1
      },
      {
        "4477",
        1
      },
      {
        "4478",
        1
      },
      {
        "4479",
        1
      },
      {
        "4480",
        2
      },
      {
        "4481",
        2
      },
      {
        "4482",
        3
      }
    };
    private static Dictionary<string, int> helocQualfields = new Dictionary<string, int>()
    {
      {
        "4465",
        1
      },
      {
        "4466",
        1
      },
      {
        "4467",
        1
      },
      {
        "4468",
        1
      },
      {
        "4469",
        2
      },
      {
        "4470",
        2
      },
      {
        "4471",
        3
      }
    };
    private static Dictionary<string, int> helocRepaymentfields = new Dictionary<string, int>()
    {
      {
        "4573",
        1
      },
      {
        "4574",
        2
      },
      {
        "4575",
        2
      },
      {
        "HELOC.MinRepmtPct",
        3
      }
    };
    private bool performanceEnabled;
    private bool skipPaymentScheduleCalculation;

    internal PaymentSchedule[] Schedule
    {
      get
      {
        bool performanceEnabled = this.performanceEnabled;
        this.performanceEnabled = false;
        if (this.Val("1678") == "Y")
          this.buildSchedule("calcmanual", (string) null);
        else
          this.buildSchedule((string) null, (string) null);
        this.populatePaymentSchedule();
        PaymentSchedule[] schedule = new PaymentSchedule[this.loanInfo.LoanPeriod + 1];
        for (int index = 0; index < this.loanInfo.LoanPeriod; ++index)
          schedule[index] = this.paySchedule[index];
        if (this.loanInfo.InterestOnly > 0 && (this.loanInfo.InterestOnly >= this.loanInfo.BallonTerm && this.loanInfo.BallonTerm > 0 || this.loanInfo.InterestOnly >= this.loanInfo.LoanTerm && this.loanInfo.LoanTerm > 0) && this.paySchedule[this.loanInfo.LoanPeriod] != null)
          schedule[this.loanInfo.LoanPeriod] = this.paySchedule[this.loanInfo.LoanPeriod];
        this.performanceEnabled = performanceEnabled;
        return schedule;
      }
    }

    internal PaymentSchedule[] FHAPaymentSchedule
    {
      get
      {
        bool performanceEnabled = this.performanceEnabled;
        this.performanceEnabled = false;
        this.buildSchedule((string) null, (string) null);
        PaymentSchedule[] fhaPaymentSchedule = new PaymentSchedule[this.loanInfo.LoanPeriod + 1];
        for (int index = 0; index < this.loanInfo.LoanPeriod; ++index)
          fhaPaymentSchedule[index] = this.payFHASchedule[index];
        this.performanceEnabled = performanceEnabled;
        return fhaPaymentSchedule;
      }
    }

    internal int NumberOfTerm => this.loanInfo.LoanPeriod;

    internal string DateCutOff78 => this.loanInfo.DateCutOff78;

    internal string DateCutOff80 => this.loanInfo.DateCutOff80;

    internal double TotalPrincipalIn5Years => this.totalPrincipalIn5Years;

    internal double TotalInterestIn5Years => this.totalInterestIn5Years;

    internal double TotalMIIn5Years => this.totalMIIn5Years;

    internal double OutstandingBalanceAfter1stAdjustment
    {
      get => this.outstandingBalanceAfter1stAdjustment;
    }

    internal double MortgageInsuranceAfter1stAdjustment => this.mortgageInsuranceAfter1stAdjustment;

    internal double HighestLoanRate => this.highestLoanRate;

    internal double MaximumPayment => this.maximumPayment;

    internal double LowestLoanRate => this.lowestLoanRate;

    internal double F_le1x24_cd4x34 => this.f_le1x24_cd4x34;

    internal bool UseWorstCaseScenario
    {
      get => this.useWorstCaseScenario;
      set => this.useWorstCaseScenario = value;
    }

    internal bool UseInterimServicingScenario
    {
      get => this.useInterimServicingScenario;
      set => this.useInterimServicingScenario = value;
    }

    internal bool UseBestCaseScenario
    {
      get => this.useBestCaseScenario;
      set => this.useBestCaseScenario = value;
    }

    internal RegzCalculation(
      SessionObjects sessionObjects,
      ILoanConfigurationInfo configInfo,
      LoanData l,
      EllieMae.EMLite.CalculationEngine.CalculationObjects calObjs)
      : base(l, calObjs)
    {
      this.sessionObjects = sessionObjects;
      this.configInfo = configInfo;
      this.CalcAPR = this.RoutineX(new Routine(this.calculateAPR));
      this.BuildPaySchedule = this.RoutineX(new Routine(this.buildSchedule));
      this.CalcIRS1098 = this.RoutineX(new Routine(this.calculateIRS1098));
      this.CalcREGZSummary = this.RoutineX(new Routine(this.calculateREGZSummary));
      this.CalcARMDisclosureSample = this.RoutineX(new Routine(this.calculateARMDisclosureSample));
      this.CalcPMIMidpointCancelationDate = this.RoutineX(new Routine(this.calculatePMIMidpointCancelationDate));
      this.CalcFloorRate = this.RoutineX(new Routine(this.calculateFloorRate));
      this.CalcFirstAdjustmentMinimum = this.RoutineX(new Routine(this.calculateFirstAdjustmentMinimum));
      this.CalcLateFeePaymentInRegz = this.RoutineX(new Routine(this.calculateLateFeePaymentInRegz));
      this.CalcLateExemptRightRescissionFlag = this.RoutineX(new Routine(this.calculateLateExemptRightRescissionFlag));
      this.CalcOnDemand = this.RoutineX(new Routine(this.calcOnDemand));
      this.CalcConstructionPhaseDisclosedSeparately = this.RoutineX(new Routine(this.calculateConstructionPhaseDisclosedSeparately));
      this.CalcLotLandStatus = this.RoutineX(new Routine(this.calculateLotLandStatus));
      this.CalcConstructionDates = this.RoutineX(new Routine(this.calculateConstructionDates));
      this.CalcInitialInterestRate = this.RoutineX(new Routine(this.calculateInitialInterestRate));
      this.CalDisclosureLog2015 = this.RoutineX(new Routine(this.CalculateLatestDisclosure2015));
      this.SychConstructionRateNoteRate = this.RoutineX(new Routine(this.sychronizeConstructionRateNoteRate));
      this.CalculateHELOCPayment = this.RoutineX(new Routine(this.calculateHELOCQualifyingPayment));
      this.CalcHELOCInitialPayment = this.RoutineX(new Routine(this.calculateHELOCInitialPayment));
      this.CleanBuydownFields = this.RoutineX(new Routine(this.cleanUpBuydownFields));
      this.CalcBuydownSummary = this.RoutineX(new Routine(this.calculateBuydownSummary));
      this.CalcMaturityDate = this.RoutineX(new Routine(this.calculateMaturityDate));
      this.CalcBuydownIndicator = this.RoutineX(new Routine(this.calculateBuydownIndicator));
      this.CalcDailyInterest = this.RoutineX(new Routine(this.calculateDailyInterest));
      this.CalcLenderRepresentative = this.RoutineX(new Routine(this.calculateLenderRepresentative));
      this.UpdateLenderRepresentative = this.RoutineX(new Routine(this.updateLenderRepresentative));
      this.CalcRoleName = this.RoutineX(new Routine(this.calculateRoleName));
      this.SetLenderRepFromUserId = this.RoutineX(new Routine(this.setLenderRepFromUserId));
      this.CalculateMersOriginalMortgagee = this.RoutineX(new Routine(this.calculateMersOriginalMortgagee));
      this.CalcZeroPercentPaymentOption = this.RoutineX(new Routine(this.calculateZeroPercentPaymentOption));
      this.CalcPointsInInitialAdjustedRate = this.RoutineX(new Routine(this.calculatePointsInInitialAdjustedRate));
      this.CleareSignCustomizeIndicator = this.RoutineX(new Routine(this.cleareSignCustomizeIndicator));
      this.ReCalculateLenderRep = this.RoutineX(new Routine(this.reCalculateLenderRep));
      this.SeteSignerCuztomizeIndicator = this.RoutineX(new Routine(this.seteSignerCuztomizeIndicator));
      this.AddFieldHandler("2845", this.RoutineX(new Routine(this.calculateIRS1098)));
      Routine routine1 = this.RoutineX(new Routine(this.calculateARMDisclosureSample));
      this.AddFieldHandler("1895", routine1);
      this.AddFieldHandler("1896", routine1);
      this.AddFieldHandler("1172", this.RoutineX(new Routine(this.calculateUSDAGovtLoanType)));
      Routine routine2 = this.RoutineX(new Routine(this.calculateLateFeePaymentInRegz));
      this.AddFieldHandler("674", routine2);
      this.AddFieldHandler("1719", routine2);
      this.AddFieldHandler("2831", routine2);
      this.AddFieldHandler("2832", routine2);
      this.AddFieldHandler("3942", this.RoutineX(new Routine(this.calculateLateExemptRightRescissionFlag)));
      Routine routine3 = this.RoutineX(new Routine(this.calculateMaturityDate));
      this.AddFieldHandler("1889", routine3);
      this.AddFieldHandler("1890", routine3);
      Routine routine4 = new Routine(this.calculateHELOCQualifyingPayment);
      for (int index = 4464; index <= 4474; ++index)
      {
        if (index == 4468 || index == 4464)
          this.AddFieldHandler(index.ToString(), new Routine(this.calculateQualifingPaymentBasis) + routine4 + calObjs.VERIFCal.CalcLiabilities + calObjs.D1003Cal.CalcHousingExp + calObjs.D1003URLA2020Cal.CalcEstimatedNetMonthlyRent);
        else
          this.AddFieldHandler(index.ToString(), routine4 + calObjs.VERIFCal.CalcLiabilities + calObjs.D1003Cal.CalcHousingExp + calObjs.D1003URLA2020Cal.CalcEstimatedNetMonthlyRent);
      }
      this.AddFieldHandler("4491", routine4 + calObjs.VERIFCal.CalcLiabilities + calObjs.D1003Cal.CalcHousingExp + calObjs.D1003URLA2020Cal.CalcEstimatedNetMonthlyRent);
      this.AddFieldHandler("4531", new Routine(this.calculateQualifingPaymentBasis) + new Routine(this.calculateHELOCQualifyingPayment) + calObjs.VERIFCal.CalcLiabilities + calObjs.D1003Cal.CalcHousingExp + calObjs.D1003URLA2020Cal.CalcEstimatedNetMonthlyRent);
      Routine routine5 = new Routine((new Routine(this.calculateHELOCInitialPayment) + calObjs.D1003Cal.CalcHousingExp).Invoke);
      for (int index = 4475; index <= 4485; ++index)
      {
        if (index == 4479 || index == 4475)
          this.AddFieldHandler(index.ToString(), new Routine(this.calculatePaymentBasis) + routine5);
        else
          this.AddFieldHandler(index.ToString(), routine5);
      }
      this.AddFieldHandler("4665", routine5);
      this.AddFieldHandler("4530", new Routine(this.calculatePaymentBasis) + new Routine(this.calculateHELOCInitialPayment) + calObjs.D1003Cal.CalcHousingExp);
      Routine routine6 = new Routine(this.calculateRepaymentBasis);
      this.AddFieldHandler("4568", routine6);
      this.AddFieldHandler("4569", routine6);
      this.AddFieldHandler("4573", routine6);
      Routine routine7 = new Routine(this.calculateHELOCRepayment);
      for (int index = 4573; index <= 4575; ++index)
        this.AddFieldHandler(index.ToString(), routine7);
      this.AddFieldHandler("4568", routine7);
      this.AddFieldHandler("4569", routine7);
      this.AddFieldHandler("HELOC.MinRepmtPct", routine7);
      Routine routine8 = this.RoutineX(new Routine(this.calculateLenderRepresentative));
      this.AddFieldHandler("4672", this.CleareSignCustomizeIndicator + new Routine(this.calculateRoleName) + routine8);
      this.AddFieldHandler("4802", this.CleareSignCustomizeIndicator + new Routine(this.calculateRoleName) + routine8);
      this.AddFieldHandler("4806", this.CleareSignCustomizeIndicator + new Routine(this.calculateRoleName) + routine8);
      this.AddFieldHandler("4809", this.CleareSignCustomizeIndicator + new Routine(this.calculateRoleName) + routine8);
      this.AddFieldHandler("4811", this.CleareSignCustomizeIndicator + new Routine(this.calculateRoleName) + routine8);
      this.AddFieldHandler("4814", this.CleareSignCustomizeIndicator + new Routine(this.calculateRoleName) + routine8);
      this.AddFieldHandler("4818", this.CleareSignCustomizeIndicator + new Routine(this.calculateRoleName) + routine8);
      this.AddFieldHandler("4824", this.CleareSignCustomizeIndicator + new Routine(this.calculateRoleName) + routine8);
      Routine routine9 = this.RoutineX(new Routine(this.updateLenderRepresentative));
      this.AddFieldHandler("VEND.X55", routine9);
      this.AddFieldHandler("VEND.X63", routine9);
      this.AddFieldHandler("VEND.X65", routine9);
      this.AddFieldHandler("VEND.X73", routine9);
      this.AddFieldHandler("VEND.X75", routine9);
      this.AddFieldHandler("VEND.X83", routine9);
      this.AddFieldHandler("VEND.X2", routine9);
      this.AddFieldHandler("VEND.X10", routine9);
      this.AddFieldHandler("VEND.X509", routine9);
      this.AddFieldHandler("VEND.X510", routine9);
      this.AddFieldHandler("VEND.X511", routine9);
      this.AddFieldHandler("VEND.X512", routine9);
      this.AddFieldHandler("VEND.X61", routine9);
      this.AddFieldHandler("VEND.X71", routine9);
      this.AddFieldHandler("VEND.X81", routine9);
      this.AddFieldHandler("VEND.X8", routine9);
      this.AddFieldHandler("VEND.X1049", routine9);
      this.AddFieldHandler("VEND.X1050", routine9);
      this.AddFieldHandler("VEND.X1051", routine9);
      this.AddFieldHandler("VEND.X1052", routine9);
      this.AddFieldHandler("4682", this.SetLenderRepFromUserId);
      this.AddFieldHandler("4804", this.SetLenderRepFromUserId);
      this.AddFieldHandler("4807", this.SetLenderRepFromUserId);
      this.AddFieldHandler("4810", this.SetLenderRepFromUserId);
      this.AddFieldHandler("4813", this.SetLenderRepFromUserId);
      this.AddFieldHandler("4816", this.SetLenderRepFromUserId);
      this.AddFieldHandler("4821", this.SetLenderRepFromUserId);
      this.AddFieldHandler("4827", this.SetLenderRepFromUserId);
      this.AddFieldHandler("1051", this.RoutineX(new Routine(this.calculateMersOriginalMortgagee)));
      Routine routine10 = this.RoutineX(new Routine(this.reCalculateLenderRep));
      this.AddFieldHandler("4840", routine10);
      this.AddFieldHandler("4831", routine10);
      this.AddFieldHandler("4832", routine10);
      this.AddFieldHandler("4833", routine10);
      this.AddFieldHandler("4834", routine10);
      this.AddFieldHandler("4835", routine10);
      this.AddFieldHandler("4836", routine10);
      this.AddFieldHandler("4837", routine10);
      Routine routine11 = this.RoutineX(new Routine(this.seteSignerCuztomizeIndicator));
      this.AddFieldHandler("4673", routine11);
      this.AddFieldHandler("4674", routine11);
      this.AddFieldHandler("4683", routine11);
      this.AddFieldHandler("4676", routine11);
      this.AddFieldHandler("4677", routine11);
      this.AddFieldHandler("1612", routine11);
      this.AddFieldHandler("3968", routine11);
      this.AddFieldHandler("4803", routine11);
      this.AddFieldHandler("1823", routine11);
      this.AddFieldHandler("4805", routine11);
      this.AddFieldHandler("1256", routine11);
      this.AddFieldHandler("95", routine11);
      this.AddFieldHandler("Lender.CntctTitle", routine11);
      this.AddFieldHandler("1262", routine11);
      this.AddFieldHandler("4808", routine11);
      this.AddFieldHandler("VEND.X302", routine11);
      this.AddFieldHandler("VEND.X305", routine11);
      this.AddFieldHandler("Broker.CntctTitle", routine11);
      this.AddFieldHandler("VEND.X303", routine11);
      this.AddFieldHandler("VEND.X304", routine11);
      this.AddFieldHandler("USDA.X31", routine11);
      this.AddFieldHandler("4812", routine11);
      this.AddFieldHandler("USDA.X30", routine11);
      this.AddFieldHandler("4838", routine11);
      this.AddFieldHandler("4839", routine11);
      this.AddFieldHandler("1754", routine11);
      this.AddFieldHandler("4815", routine11);
      this.AddFieldHandler("1755", routine11);
      this.AddFieldHandler("1756", routine11);
      this.AddFieldHandler("4817", routine11);
      this.AddFieldHandler("3194", routine11);
      this.AddFieldHandler("4819", routine11);
      this.AddFieldHandler("4820", routine11);
      this.AddFieldHandler("4822", routine11);
      this.AddFieldHandler("4823", routine11);
      this.AddFieldHandler("NOTICES.X31", routine11);
      this.AddFieldHandler("4825", routine11);
      this.AddFieldHandler("4826", routine11);
      this.AddFieldHandler("NOTICES.X37", routine11);
      this.AddFieldHandler("4829", routine11);
    }

    public double FindHELOCQualifyingPaymentFromQualifyingBasisInputs()
    {
      double num1 = 0.0;
      double num2 = 0.0;
      double rate = this.FltVal(this.Val("4465"));
      double num3 = this.FltVal(this.Val("4473"));
      string rateSign = this.Val("4466");
      string paymentBasis = this.Val("4464");
      string paymentBasisType = this.Val("4531");
      string perDiemCalculationMethodType = this.Val("4491");
      DateTime date = Utils.ToDate(this.Val("682"));
      this.clearHelocFields(paymentBasis, "qualifying", paymentBasisType);
      if (!string.IsNullOrEmpty(paymentBasisType) && num3 > 0.0 || paymentBasis == "Rate" && num3 > 0.0 && rate > 0.0 && (!(this.Val("4468") != "Y") || this.IntVal("4474") >= 0))
      {
        if (paymentBasis == "Rate")
        {
          double helocRate = this.calculateHelocRate(rateSign, rate, this.FltVal("4467"));
          if (helocRate > 0.0)
            num1 = !(this.Val("4468") == "Y") ? RegzCalculation.CalculateMonthlyPayment(this.IntVal("4474"), 0, num3, helocRate) : this.calculateIntPerDay(helocRate, perDiemCalculationMethodType, num3) * this.calculateHelocNoOfDays(perDiemCalculationMethodType, helocRate, num3, false, date);
        }
        switch (paymentBasisType)
        {
          case "Fraction of Balance":
            num2 = this.calculateHelocFractionOfBalance(this.FltVal("4469"), this.FltVal("4470"), num3);
            break;
          case "Percentage of Balance":
            num2 = this.calculateHelocPercentageOfBalance(this.FltVal("4471"), num3);
            break;
        }
        num1 += num2;
        if (this.Val("4472") == "Y" && this.FltVal("1483") > num1)
          num1 = this.FltVal("1483");
      }
      this.SetCurrentNum("5025", num1);
      return num1;
    }

    private void calculateHELOCQualifyingPayment(string id, string val)
    {
      double qualifyingBasisInputs = this.FindHELOCQualifyingPaymentFromQualifyingBasisInputs();
      if (this.Val("1172") != "HELOC")
        return;
      string str1 = this.Val("1811");
      bool flag1 = str1 == "PrimaryResidence" || string.IsNullOrEmpty(str1);
      string str2 = this.Val("420");
      bool flag2 = this.USEURLA2020 && str1 == "PrimaryResidence";
      switch (str2)
      {
        case "FirstLien":
          this.SetCurrentNum("1724", flag1 ? qualifyingBasisInputs : this.FltVal("120"), this.UseNoPayment(qualifyingBasisInputs));
          if (flag2)
          {
            double num = this.calObjs.D1003URLA2020Cal.GetVOALLienAmount(false, true) + this.calObjs.D1003Cal.GetVOLLienAmount(false);
            this.SetCurrentNum("1725", num, this.UseNoPayment(num));
            break;
          }
          this.SetCurrentNum("1725", flag1 ? 0.0 : this.FltVal("121"), this.UseNoPayment(0.0));
          break;
        case "SecondLien":
          if (flag2)
          {
            double num1 = qualifyingBasisInputs + this.calObjs.D1003URLA2020Cal.GetVOALLienAmount(false, true) + this.calObjs.D1003Cal.GetVOLLienAmount(false);
            this.SetCurrentNum("1725", num1, this.UseNoPayment(num1));
            double num2 = this.calObjs.D1003URLA2020Cal.GetVOALLienAmount(true, true) + this.calObjs.D1003Cal.GetVOLLienAmount(true);
            this.SetCurrentNum("1724", num2, this.UseNoPayment(num2));
            break;
          }
          this.SetCurrentNum("1725", flag1 ? qualifyingBasisInputs : this.FltVal("121"), this.UseNoPayment(qualifyingBasisInputs));
          this.SetCurrentNum("1724", flag1 ? 0.0 : this.FltVal("120"), this.UseNoPayment(0.0));
          break;
      }
    }

    private void calculateQualifingPaymentBasis(string id, string val)
    {
      if (!(this.Val("4468") != "Y"))
        return;
      switch (id)
      {
        case "4464":
          if (!(val == "Rate"))
            break;
          this.SetVal("4531", string.Empty);
          break;
        case "4531":
          if (string.IsNullOrEmpty(val))
            break;
          this.SetVal("4464", string.Empty);
          break;
        case "4468":
          this.SetVal("4531", string.Empty);
          break;
      }
    }

    private void calculateRepaymentBasis(string id, string val)
    {
      if (!(this.Val("4573") != "Y"))
        return;
      switch (id)
      {
        case "4568":
          if (!(val == "Y"))
            break;
          this.SetVal("4569", string.Empty);
          break;
        case "4569":
          if (string.IsNullOrEmpty(val))
            break;
          this.SetVal("4569", string.Empty);
          break;
        case "4573":
          this.SetVal("4569", string.Empty);
          break;
      }
    }

    private void calculateHELOCRepayment(string id, string val)
    {
      this.clearHelocFields(this.Val("4568"), "repayment", this.Val("4569"));
    }

    private double calculateHelocRate(string rateSign, double rate, double varianceRate)
    {
      switch (rateSign)
      {
        case "+":
          rate += varianceRate;
          break;
        case "-":
          rate -= varianceRate;
          break;
      }
      return rate;
    }

    private double calculateHelocNoOfDays(
      string perDiemCalculationMethodType,
      double rate,
      double balance,
      bool intitialcalc,
      DateTime scheduledFirstPaymentDate)
    {
      double helocNoOfDays;
      if (perDiemCalculationMethodType == "365/365" || perDiemCalculationMethodType == "365/360")
      {
        if (intitialcalc)
        {
          DateTime minValue = DateTime.MinValue;
          DateTime dateTime = !("Y" == this.Val("4665")) ? Utils.ParseDate((object) this.Val("748")) : Utils.ParseDate((object) this.Val("2553"));
          DateTime date = Utils.ParseDate((object) this.Val("682"));
          helocNoOfDays = dateTime == DateTime.MinValue || date == DateTime.MinValue ? 30.0 : (double) date.Subtract(dateTime).Days;
        }
        else
          helocNoOfDays = !(scheduledFirstPaymentDate == DateTime.MinValue) ? (double) DateTime.DaysInMonth(scheduledFirstPaymentDate.Year, scheduledFirstPaymentDate.Month) : 30.0;
      }
      else
        helocNoOfDays = 30.0;
      return helocNoOfDays;
    }

    private double calculateHelocFractionOfBalance(double dividend, double divisor, double Balance)
    {
      double d = dividend / divisor * Balance;
      return !double.IsNaN(d) ? d : 0.0;
    }

    private double calculateHelocPercentageOfBalance(double PercentageofBalance, double Balance)
    {
      return PercentageofBalance * Balance / 100.0;
    }

    private void clearHelocFields(int category, bool initialPayment)
    {
      foreach (KeyValuePair<string, int> keyValuePair in initialPayment ? RegzCalculation.helocInitialfields : RegzCalculation.helocQualfields)
      {
        if (keyValuePair.Value != category)
          this.SetVal(keyValuePair.Key, "");
      }
    }

    private void clearHelocFields(List<int> category, string helocSection)
    {
      Dictionary<string, int> dictionary = (Dictionary<string, int>) null;
      switch (helocSection)
      {
        case "qualifying":
          dictionary = RegzCalculation.helocQualfields;
          break;
        case "initial":
          dictionary = RegzCalculation.helocInitialfields;
          break;
        case "repayment":
          dictionary = RegzCalculation.helocRepaymentfields;
          break;
      }
      foreach (KeyValuePair<string, int> keyValuePair in dictionary)
      {
        if (category.Count > 1)
        {
          if (keyValuePair.Value != category[0] && keyValuePair.Value != category[1])
            this.SetVal(keyValuePair.Key, "");
        }
        else if (keyValuePair.Value != category[0])
          this.SetVal(keyValuePair.Key, "");
      }
    }

    private void clearHelocFields(
      string paymentBasis,
      string helocSection,
      string paymentBasisType = "�")
    {
      List<int> category = new List<int>();
      if (paymentBasis == "Rate" || paymentBasis == "Y")
        category.Add(1);
      switch (paymentBasisType)
      {
        case "Fraction of Balance":
          category.Add(2);
          break;
        case "Percentage of Balance":
          category.Add(3);
          break;
      }
      if (category.Count <= 0)
        category.Add(0);
      this.clearHelocFields(category, helocSection);
    }

    private double calculateIntPerDay(
      double rate,
      string perDiemCalculationMethodType,
      double balance)
    {
      return rate / 100.0 / (perDiemCalculationMethodType == "365/365" ? 365.0 : 360.0) * balance;
    }

    private void calculateDailyInterest(string id, string val)
    {
      double val1 = 0.0;
      string str = this.Val("1172");
      double rate1 = this.FltVal("3");
      if (rate1 > 100.0)
      {
        this.SetCurrentNum("3", 100.0);
        this.SetVal("KBYO.XD3", "100");
        rate1 = 100.0;
      }
      if ("HELOC".Equals(str))
      {
        double balance = this.FltVal(this.Val("4484"));
        if (balance > 0.0)
        {
          string perDiemCalculationMethodType = this.Val("1962");
          if (this.Val("4475") == "Rate")
          {
            string rateSign = this.Val("4477");
            string id1 = this.Val("4476");
            if (id1 == "1827")
              this.SetCurrentNum("1827", this.FltVal("688") + this.FltVal("689"));
            double rate2 = this.FltVal(id1);
            if (rate2 > 0.0)
            {
              double helocRate = this.calculateHelocRate(rateSign, rate2, this.FltVal("4478"));
              if (helocRate > 0.0)
                val1 = this.calculateIntPerDay(helocRate, perDiemCalculationMethodType, balance);
            }
          }
          else
            val1 = this.calculateIntPerDay(rate1, perDiemCalculationMethodType, balance);
        }
      }
      else
      {
        double num = this.FltVal("2");
        switch (this.IntVal("SYS.X2"))
        {
          case 364:
            if (this.Val("423") == "Biweekly")
            {
              val1 = num * rate1 / 36400.0;
              goto label_16;
            }
            else
              break;
          case 365:
            val1 = num * rate1 / 36500.0;
            goto label_16;
        }
        val1 = num * rate1 / 36000.0;
      }
label_16:
      this.SetCurrentNum("335", Math.Truncate(val1 * 100.0) / 100.0);
      this.SetCurrentNum("333", !(this.Val("SYS.X8") == "Y") ? Utils.ArithmeticRounding(val1, 4) : Utils.ArithmeticRounding(val1, 2));
    }

    private void SetField4085(string val)
    {
      if (this.Val("PAYMENTTABLE.CUSTOMIZE") == "Y" && this.FltVal("3") == 0.0 && val == string.Empty)
        this.SetVal("4085", "0.00");
      else
        this.SetVal("4085", val);
    }

    private void calculateHELOCInitialPayment(string id, string val)
    {
      if (this.Val("1172") != "HELOC")
        return;
      double num1 = 0.0;
      double val1 = 0.0;
      double num2 = 0.0;
      double rate = this.FltVal(this.Val("4476"));
      double num3 = this.FltVal(this.Val("4484"));
      string rateSign = this.Val("4477");
      string paymentBasis = this.Val("4475");
      string paymentBasisType = this.Val("4530");
      string perDiemCalculationMethodType = this.Val("1962");
      this.Val("748");
      this.Val("682");
      string str = this.Val("4479");
      int loanTerm = this.IntVal("4485");
      this.clearHelocFields(paymentBasis, "initial", paymentBasisType);
      if (!string.IsNullOrEmpty(paymentBasisType) && num3 > 0.0 || paymentBasis == "Rate" && num3 > 0.0 && rate > 0.0 && (!(str == "Y") || loanTerm >= 0))
      {
        try
        {
          if (paymentBasis == "Rate")
          {
            double helocRate = this.calculateHelocRate(rateSign, rate, this.FltVal("4478"));
            if (helocRate > 0.0)
            {
              if (str != "Y")
              {
                val1 = this.calculateIntPerDay(helocRate, perDiemCalculationMethodType, num3);
                double helocNoOfDays = this.calculateHelocNoOfDays(perDiemCalculationMethodType, helocRate, num3, true, DateTime.MinValue);
                num1 = val1 * helocNoOfDays;
              }
              else
                num1 = RegzCalculation.CalculateMonthlyPayment(loanTerm, 0, num3, helocRate);
            }
          }
          switch (paymentBasisType)
          {
            case "Fraction of Balance":
              num2 = this.calculateHelocFractionOfBalance(this.FltVal("4480"), this.FltVal("4481"), num3);
              break;
            case "Percentage of Balance":
              num2 = this.calculateHelocPercentageOfBalance(this.FltVal("4482"), num3);
              break;
          }
          num1 += num2;
          if (this.Val("4483") == "Y")
          {
            if (this.FltVal("1483") > num1)
              num1 = this.FltVal("1483");
          }
        }
        catch (Exception ex)
        {
          Tracing.Log(RegzCalculation.sw, TraceLevel.Error, nameof (RegzCalculation), "can't Monthly payment for HELOC. Error: " + ex.Message);
          return;
        }
      }
      if (this.Val("4665") == "Y" && paymentBasis == "Rate" && str != "Y" && val1 > 0.0)
      {
        double num4 = Utils.ArithmeticRounding(Utils.ArithmeticRounding((!(this.Val("SYS.X8") == "Y") ? Utils.ArithmeticRounding(val1, 4) : Utils.ArithmeticRounding(val1, 2)) * this.FltVal("332"), 2) - this.FltVal("561"), 2);
        if (num4 > 0.0)
          num1 -= num4;
      }
      this.SetNum("5", num1);
      this.SetField4085(this.Val("5"));
      this.calObjs.ToolCal.CalcNetTangibleBenefit("5", this.Val("5"));
      this.calObjs.HMDACal.CalcPrepaymentPenaltyPeriod((string) null, (string) null);
    }

    private void calculatePaymentBasis(string id, string val)
    {
      if (!(this.Val("4479") == "Y"))
        return;
      switch (id)
      {
        case "4475":
          if (!(val == "Rate"))
            break;
          this.SetVal("4530", string.Empty);
          break;
        case "4530":
          if (string.IsNullOrEmpty(val))
            break;
          this.SetVal("4475", string.Empty);
          break;
        case "4479":
          this.SetVal("4530", string.Empty);
          break;
      }
    }

    private void calculateZeroPercentPaymentOption(string id, string val)
    {
      if (!this.UseNoPayment(0.0) || !(this.Val("608") != "Fixed"))
        return;
      this.SetVal("4746", "AmortizingPayment");
    }

    private void calculateUSDAGovtLoanType(string id, string val)
    {
      if (!(this.Val("1172") != "FarmersHomeAdministration"))
        return;
      this.SetVal("Terms.USDAGovtType", "");
    }

    private void calculateAPR(string id, string val)
    {
      if (Tracing.IsSwitchActive(RegzCalculation.sw, TraceLevel.Info))
        Tracing.Log(RegzCalculation.sw, TraceLevel.Info, nameof (RegzCalculation), "routine: calculateAPR ID: " + id);
      double num1 = this.FltVal("3");
      this.SetCurrentNum("2625", num1 + this.FltVal("247"));
      this.calObjs.NewHud2015Cal.CalcLECDDisplayFields("2625", (string) null);
      this.SetCurrentNum("1827", this.FltVal("688") + this.FltVal("689"));
      this.calculateHELOCInitialPayment(id, val);
      string str = this.Val("1172");
      if (str == "FarmersHomeAdministration")
      {
        if (this.FltVal("NEWHUD.X1707") == 0.0)
          this.SetVal("3266", "");
      }
      else if (str != "FHA")
        this.SetVal("3266", "");
      if (id != "TPO")
        this.buildSchedule(id, val);
      double finalPayment = this.FltVal("2");
      double num2 = 0.0;
      double num3 = this.FltVal("949");
      double num4 = this.FltVal("NEWHUD2.X4768");
      double num5 = this.FltVal("799");
      double num6 = this.FltVal("3121");
      if (finalPayment <= 0.0)
      {
        this.SetCurrentNum("799", 0.0);
        this.calObjs.GFECal.CalcHighPrice(id, val);
        this.calObjs.GFECal.CalcSecondAppraisalRequired(id, val);
        this.calObjs.ATRQMCal.CalcHigherPricedCheck(id, val);
        this.calObjs.ATRQMCal.CalcEligibility(id, val);
        if (this.loanInfo.LoanType == "HELOC")
        {
          this.calObjs.ATRQMCal.GetATRQMPaymentSchedule(false);
          this.calObjs.HelocCal.CalcHELOCPeriodicRates(id, val);
        }
        if (num6 == 0.0 || num5 == 0.0 || this.CurrentAPRChanged == null)
          return;
        this.CurrentAPRChanged((object) null, new EventArgs());
      }
      else
      {
        if (this.loanInfo.LoanPurpose == "ConstructionOnly")
        {
          double num7 = this.FltVal("4088");
          double num8 = finalPayment / 2.0 - num3;
          if (num8 > 0.0)
            num2 = (num7 + num3 + num4) / num8;
          if (this.loanInfo.ConstInterestType == "365/365" || this.loanInfo.ConstInterestType == "365/360")
          {
            if (this.loanInfo.ConstSpanStartDate == DateTime.MinValue)
              this.loanInfo.ConstSpanStartDate = this.loanInfo.ConstEstClosingDate;
            TimeSpan timeSpan = this.loanInfo.ConstFinalPayDate.Subtract(this.loanInfo.ConstSpanStartDate);
            if (timeSpan.Days > 0)
              num2 = num2 / (double) timeSpan.Days * 365.0 * 100.0;
          }
          else if (this.loanInfo.ConstPeriod > 0)
            num2 = num2 * 100.0 / (double) this.loanInfo.ConstPeriod * (double) this.loanInfo.NumberofPayPerYear;
          if (num2 == 0.0)
            num2 = this.loanInfo.ConstIntRate == 0.0 ? num1 : this.loanInfo.ConstIntRate;
        }
        else if (this.loanInfo.LoanType == "HELOC")
        {
          num2 = !(this.loanInfo.ARMType == "Fixed") ? this.FltVal("3296") : num1;
        }
        else
        {
          double num9 = finalPayment - num3;
          if (this.loanInfo.LoanPurpose == "ConstructionToPermanent")
          {
            if (this.loanInfo.ConstMethod == "B")
              num9 -= this.loanInfo.ConstTotalFinCharge;
            else if (this.loanInfo.ConstInterestType == "360/360")
              num9 -= Utils.ArithmeticRounding(this.loanInfo.LoanAmount / 2.0 * (this.loanInfo.ConstIntRate / 1200.0) * (double) this.loanInfo.ConstPeriod, 2);
            else
              num9 -= this.loanInfo.ConstTotalFinCharge;
            if (this.IntVal("1198") > 0 && this.Val("HUD69") != "FirstAmortDate")
              num9 -= this.FltVal("1766") * (double) this.loanInfo.ConstPeriod;
          }
          double targetFinanceAmount = Math.Round(num9, 2);
          double num10 = 1.0;
          double num11 = 0.0;
          double firstPaymentLength = 1.0;
          int startPayment = 0;
          int numberPayments = this.loanInfo.LoanPeriod;
          if (this.skipPaymentScheduleCalculation)
            numberPayments = this.loanInfo.BallonTerm;
          if (this.loanInfo.LoanPurpose == "ConstructionToPermanent" && this.loanInfo.ConstMethod == "B")
          {
            firstPaymentLength = this.FltVal("1176") / 2.0 + 1.0;
            startPayment = this.loanInfo.ConstPeriod;
            numberPayments -= startPayment;
          }
          num2 = 0.0;
          if (this.loanInfo.LoanPurpose == "ConstructionToPermanent")
          {
            num2 = this.calculateConstructionPermAPR(targetFinanceAmount);
            if (double.IsInfinity(num2) || double.IsNaN(num2))
              num2 = num1;
          }
          else if (this.paySchedule[0] != null && numberPayments >= 0 && !this.UseWorstCaseScenario && !this.UseBestCaseScenario)
          {
            if (finalPayment == 0.0 || num3 / finalPayment < 0.99)
            {
              bool flag = this.Val("4746") == "NoPayment";
              while (targetFinanceAmount > 0.0)
              {
                double perRate = (num10 + num11) / 2.0;
                double num12 = this.calcFinanceAmount(perRate, firstPaymentLength, startPayment, numberPayments, num1 == 0.0 & flag, finalPayment);
                if (num1 != 0.0 && num12 == 0.0)
                {
                  num2 = 0.0;
                  break;
                }
                if (num12 < targetFinanceAmount)
                  num10 = perRate;
                else if (num12 > targetFinanceAmount)
                {
                  num11 = perRate;
                }
                else
                {
                  num2 = perRate;
                  break;
                }
                if (num10 == 0.0 && num1 == 0.0)
                {
                  num2 = 0.0;
                  break;
                }
              }
            }
            else if (finalPayment > 0.0)
              num2 = 1.0;
          }
          if (num2 == 0.0)
            num2 = num1;
          else if (this.loanInfo.LoanPurpose != "ConstructionToPermanent")
          {
            if (this.loanInfo.IsBiWeekly)
              num2 *= 100.0 * ((double) this.loanInfo.DaysPerYearBiWeekly / 14.0);
            else
              num2 *= 1200.0;
          }
        }
        double num13 = Utils.ArithmeticRounding(num2, 3);
        this.SetCurrentNum("799", num13, this.loanInfo.LoanRate == 0.0 && num13 == 0.0);
        this.calObjs.NewHud2015Cal.CalcLECDDisplayFields("799", (string) null);
        this.calObjs.GFECal.CalcHighPrice(id, val);
        this.calObjs.GFECal.CalcSecondAppraisalRequired(id, val);
        this.calObjs.ATRQMCal.CalcHigherPricedCheck(id, val);
        this.calObjs.ATRQMCal.CalcEligibility(id, val);
        if (this.loanInfo.LoanType == "HELOC")
        {
          this.calObjs.ATRQMCal.GetATRQMPaymentSchedule(false);
          this.calObjs.HelocCal.CalcHELOCPeriodicRates(id, val);
        }
        this.populatePaymentSchedule();
        if (num6 == 0.0 || num5 == num13 || this.CurrentAPRChanged == null)
          return;
        this.CurrentAPRChanged((object) null, new EventArgs());
      }
    }

    private double calculateConstructionPermAPR(double targetFinanceAmount)
    {
      try
      {
        List<double[]> payList = new List<double[]>();
        double num1 = 0.0;
        double num2 = 0.0;
        int num3 = 0;
        int num4 = this.loanInfo.LoanPeriod;
        if (this.skipPaymentScheduleCalculation)
          num4 = this.loanInfo.BallonTerm;
        int num5 = 0;
        if (this.loanInfo.LoanPurpose == "ConstructionToPermanent" && this.loanInfo.ConstPeriod > 0)
          num5 = this.loanInfo.ConstPeriod;
        for (int index = num5; index < num4 && this.paySchedule[index] != null; ++index)
        {
          if (num2 == 0.0 || num1 == 0.0)
          {
            num2 = this.paySchedule[index].Payment;
            num1 = this.paySchedule[index].CurrentRate;
          }
          if (this.paySchedule[index].Payment != num2 || this.paySchedule[index].CurrentRate != num1)
          {
            payList.Add(new double[2]{ num2, (double) num3 });
            num2 = this.paySchedule[index].Payment;
            num1 = this.paySchedule[index].CurrentRate;
            num3 = 0;
          }
          ++num3;
        }
        if (num4 > 0)
          payList.Add(new double[2]{ num2, (double) num3 });
        DateTime dateTime = this.loanInfo.ConstFirstAmortDate.AddMonths(-1);
        double num6 = this.convertDateDiffToDouble(this.loanInfo.ConstEstClosingDate, dateTime);
        int num7 = (int) num6;
        int num8 = (int) ((double) ((int) ((num6 - (double) num7) * 100.0) / 2) + 0.5);
        if (num7 % 2 > 0)
          num8 += 15;
        int num9 = (int) ((double) num7 / 2.0);
        double num10 = this.convertDateDiffToDouble(dateTime, this.loanInfo.ConstFirstAmortDate);
        int t = num9 + (int) num10;
        int f;
        for (f = num8 + (int) ((num10 - (double) (int) num10) * 100.0); f >= 30; f -= 30)
          ++t;
        double loanRate = this.loanInfo.LoanRate;
        double num11 = 0.0;
        double constructionCost1;
        double constructionCost2;
        for (; Math.Abs(loanRate - num11) > 1E-06; loanRate += 0.1 * ((targetFinanceAmount - constructionCost1) / (constructionCost2 - constructionCost1)))
        {
          double monthlyRate1 = loanRate / 1200.0;
          constructionCost1 = this.calculateConstructionCost(t, f, monthlyRate1, payList);
          double monthlyRate2 = (loanRate + 0.1) / 1200.0;
          constructionCost2 = this.calculateConstructionCost(t, f, monthlyRate2, payList);
          num11 = loanRate;
        }
        return Utils.ArithmeticRounding(loanRate, 3);
      }
      catch (Exception ex)
      {
        Tracing.Log(RegzCalculation.sw, TraceLevel.Error, nameof (RegzCalculation), "can't calculate Construction-Perm APR. Error: " + ex.Message);
        return 0.0;
      }
    }

    private double calculateConstructionCost(
      int t,
      int f,
      double monthlyRate,
      List<double[]> payList)
    {
      int y = t;
      double constructionCost = 0.0;
      for (int index1 = 0; index1 < payList.Count; ++index1)
      {
        double num1 = 0.0;
        double[] pay = payList[index1];
        int num2 = (int) pay[1];
        double num3 = pay[0];
        for (int index2 = 1; index2 <= num2; ++index2)
          num1 += 1.0 / Math.Pow(1.0 + monthlyRate, (double) (index2 - 1));
        constructionCost += num3 * num1 / ((1.0 + (double) f / 30.0 * monthlyRate) * Math.Pow(1.0 + monthlyRate, (double) y));
        y += num2;
      }
      return constructionCost;
    }

    private double convertDateDiffToDouble(DateTime dateStarting, DateTime dateEnding)
    {
      int num1 = 0;
      for (; dateEnding > dateStarting; dateEnding = dateEnding.AddMonths(-1))
        ++num1;
      if (dateStarting == dateEnding)
        return (double) num1;
      int num2 = num1 - 1;
      dateEnding = dateEnding.AddMonths(1);
      TimeSpan timeSpan = dateEnding.Subtract(dateStarting);
      return (double) num2 + (double) timeSpan.Days / 100.0;
    }

    private void buildSchedule(string id, string val)
    {
      if (Tracing.IsSwitchActive(RegzCalculation.sw, TraceLevel.Info))
        Tracing.Log(RegzCalculation.sw, TraceLevel.Info, nameof (RegzCalculation), "routine: buildSchedule ID: " + id);
      if (id == "2982" && val != "Y")
        this.SetVal("1177", "");
      else if (id == "1177")
      {
        if (Utils.ParseInt((object) (val ?? "")) > 0)
          this.SetVal("2982", "Y");
        else
          this.SetVal("2982", "N");
      }
      if (this.Val("19").StartsWith("Construction"))
        this.calculateConstructionDates(id, val);
      this.collectLoanInfo();
      if (this.UseNoPayment(0.0) && this.Val("4746") == "NoPaymentwithBalloon")
        this.SetVal("1659", "Y");
      else if (this.Val("LOANTERMTABLE.CUSTOMIZE") != "Y" && this.UseNew2015GFEHUD && this.loanInfo.LoanPurpose == "ConstructionOnly")
        this.SetVal("1659", "Y");
      else if (this.Val("LOANTERMTABLE.CUSTOMIZE") != "Y" || !this.UseNew2015GFEHUD)
        this.SetVal("1659", this.loanInfo.BallonTerm <= 0 || this.loanInfo.BallonTerm >= this.loanInfo.LoanTerm ? "N" : "Y");
      this.SetVal("HMDA.X114", this.Val("1659"));
      if (this.loanInfo.LoanType == "HELOC")
        this.buildHELOCSchedule();
      else if (id == "calcmanual" || this.Val("1678") == "Y")
      {
        this.buildManualPayment(id, val);
      }
      else
      {
        if (this.loanInfo.MICutOffAmount >= 0.0 && this.loanInfo.LoanType == "FHA")
        {
          this.loanInfo.TestOnly = true;
          this.loanInfo.LoanAmount = this.FltVal("1109");
          this.buildPaymentSchedule(id, val);
          this.payFHASchedule = (PaymentSchedule[]) this.paySchedule.Clone();
          this.addRemainingLTVPercToPaymentSchedule(this.payFHASchedule);
          this.loanInfo.LoanAmount = this.FltVal("2");
          this.collectLoanInfo();
        }
        this.loanInfo.TestOnly = false;
        this.buildPaymentSchedule(id, val);
      }
      this.addRemainingLTVPercToPaymentSchedule(this.paySchedule);
    }

    private void addRemainingLTVPercToPaymentSchedule(PaymentSchedule[] pSchedules)
    {
      if (pSchedules == null || pSchedules.Length == 0)
        return;
      double valueForRemainingLtv = this.getSubjectPropertyValueForRemainingLTV();
      if (valueForRemainingLtv == 0.0)
        return;
      for (int index = 0; index < pSchedules.Length && pSchedules[index] != null; ++index)
        pSchedules[index].RemainingLTV = Utils.ArithmeticRounding(pSchedules[index].Balance / valueForRemainingLtv * 100.0, 3);
    }

    private void CalculateLatestDisclosure2015(string id, string val)
    {
      if (!Utils.CheckIf2015RespaTila(this.loan.GetField("3969")))
        return;
      this.loan.Calculator.CalculateLatestDisclosure2015((IDisclosureTracking2015Log) null);
    }

    private void calculateConstructionDates(string id, string val)
    {
      string str = this.Val("19");
      this.Val("1962");
      if (str == "ConstructionOnly")
        this.SetVal("1963", "");
      if (id == "1961" && str == "ConstructionOnly")
      {
        DateTime date1 = Utils.ParseDate((object) val);
        DateTime date2 = Utils.ParseDate((object) this.calcFinalPaymentDate(Utils.ParseDate((object) this.Val("682")), -1));
        if (date2 == DateTime.MinValue)
          val = string.Empty;
        else if (date1 < date2)
          val = date2.ToString("MM/dd/yyyy");
        this.SetVal("1961", val);
      }
      else if (id == "1963" && str == "ConstructionToPermanent")
      {
        DateTime date3 = Utils.ParseDate((object) val);
        DateTime date4 = Utils.ParseDate((object) this.calcFinalPaymentDate(Utils.ParseDate((object) this.Val("682")), 0));
        if (date4 == DateTime.MinValue)
        {
          val = string.Empty;
        }
        else
        {
          DateTime date5 = Utils.ParseDate((object) this.Val("763"));
          if (date5 != DateTime.MinValue)
          {
            DateTime dateTime1 = date5.AddMonths(this.IntVal("1176") + 1);
            DateTime dateTime2 = dateTime1.AddMonths(1);
            if (date3 >= dateTime2 || date3 < dateTime1)
              val = date4.ToString("MM/dd/yyyy");
          }
        }
        this.SetVal("1963", val);
      }
      if (!(id == "682") && !(id == "1176") && !(id == "19") && !(id == "1962"))
        return;
      switch (str)
      {
        case "ConstructionOnly":
          if (this.Val("682") == string.Empty)
          {
            this.SetVal("1961", string.Empty);
            break;
          }
          this.SetVal("1961", this.calcFinalPaymentDate(Utils.ParseDate((object) this.Val("682")), -1));
          break;
        case "ConstructionToPermanent":
          if (this.Val("682") == string.Empty)
          {
            this.SetVal("1963", string.Empty);
            break;
          }
          this.SetVal("1963", this.calcFinalPaymentDate(Utils.ParseDate((object) this.Val("682")), 0));
          break;
      }
    }

    private string calculateFirstPaymenDate(DateTime closingDate)
    {
      if (closingDate == DateTime.MinValue)
        return string.Empty;
      closingDate = closingDate.AddMonths(1);
      if (this.loanInfo.ConstInterestType != "360/360")
        closingDate = closingDate.AddDays((double) (closingDate.Day * -1 + 1));
      return closingDate.ToString("MM/dd/yyyy");
    }

    private string calcFinalPaymentDate(DateTime firstPaymentDate, int extra)
    {
      if (firstPaymentDate == DateTime.MinValue)
        return string.Empty;
      int num = Utils.ParseInt((object) this.Val("1176"));
      if (num == -1)
        return string.Empty;
      firstPaymentDate = firstPaymentDate.AddMonths(num + extra);
      return firstPaymentDate.ToString("MM/dd/yyyy");
    }

    internal void CalcRateCap()
    {
      this.collectLoanInfo();
      this.SetCurrentNum("3296", this.loanInfo.IsARM ? (this.loanInfo.LoanType != "HELOC" ? Utils.ArithmeticRounding(this.loanInfo.AdjustedFullyIndexedRate, 3) : Utils.ArithmeticRounding(this.loanInfo.RateCap, 3)) : 0.0);
    }

    private void collectLoanInfo()
    {
      this.loanInfo.LoanType = this.Val("1172");
      this.loanInfo.LoanAmount = this.FltVal("2");
      this.loanInfo.BaseLoanAmount = this.FltVal("1109");
      this.loanInfo.LoanTerm = this.IntVal("4");
      this.loanInfo.LoanRate = this.FltVal("3");
      this.loanInfo.LoanPurpose = this.Val("19");
      this.loanInfo.BallonTerm = this.IntVal("325");
      this.loanInfo.ExtraPayment = this.FltVal("312");
      this.loanInfo.IsConstPhaseDisclosedSeparately = this.Val("4084") == "Y";
      this.loanInfo.ConstPeriod = this.IntVal("1176");
      this.loanInfo.ConstIntRate = this.FltVal("1677");
      this.loanInfo.ConstReqRsv = this.FltVal("1265");
      this.loanInfo.InterestOnly = this.IntVal("1177");
      this.loanInfo.HelocTeaserRate = this.FltVal("1482");
      this.loanInfo.HelocDaysPerYear = this.IntVal("SYS.X2");
      this.loanInfo.ARMType = this.Val("608");
      bool flag1 = this.loanInfo.ARMType == "Fixed";
      this.loanInfo.IsARM = this.loanInfo.ARMType == "AdjustableRate";
      if (flag1)
      {
        this.loanInfo.ARMfirstAdjCap = 0.0;
        this.loanInfo.ARMfirstChange = 0;
        this.loanInfo.ARMadjCap = 0.0;
        this.loanInfo.ARMadjPeriod = 0;
        this.loanInfo.ARMlifeCap = 0.0;
        this.loanInfo.ARMmargin = 0.0;
        this.loanInfo.ARMindex = 0.0;
        this.loanInfo.ARMfloor = 0.0;
        this.loanInfo.ARMround = 0.0;
      }
      else
      {
        this.loanInfo.ARMfirstAdjCap = this.FltVal("697");
        this.loanInfo.ARMfirstChange = this.IntVal("696");
        this.loanInfo.ARMadjCap = this.FltVal("695");
        this.loanInfo.ARMadjPeriod = this.IntVal("694");
        this.loanInfo.ARMlifeCap = this.FltVal("247");
        this.loanInfo.ARMmargin = this.FltVal("689");
        this.loanInfo.ARMindex = this.FltVal("688");
        this.loanInfo.ARMfloor = this.FltVal("1699");
        this.loanInfo.ARMround = this.FltVal("1700");
        this.loanInfo.HelocTeaserMargin = this.loanInfo.ARMfirstAdjCap;
        this.loanInfo.HelocMiniPayment = this.FltVal("1483");
        if (this.loanInfo.LoanPurpose == "ConstructionToPermanent")
          this.loanInfo.ARMfirstChange += this.IntVal("1176");
      }
      if (this.loanInfo.HelocDaysPerYear != 365)
        this.loanInfo.HelocDaysPerYear = 360;
      if (this.loanInfo.InterestOnly > this.loanInfo.LoanTerm)
        this.loanInfo.InterestOnly = this.loanInfo.LoanTerm;
      if (this.loanInfo.LoanPurpose == "ConstructionOnly" && this.loanInfo.ConstPeriod == 0)
        this.loanInfo.ConstPeriod = this.loanInfo.LoanTerm;
      if (this.loanInfo.BallonTerm == 0 || this.loanInfo.BallonTerm > this.loanInfo.LoanTerm)
        this.loanInfo.BallonTerm = this.loanInfo.LoanTerm;
      if (this.loanInfo.LoanPurpose == "ConstructionToPermanent")
      {
        this.loanInfo.LoanTermConstPlusPerm = this.Val("CONST.X1") != "Y" ? this.loanInfo.LoanTerm + this.loanInfo.ConstPeriod : this.loanInfo.LoanTerm;
        this.loanInfo.BallonTerm = this.IntVal("325") <= 0 ? this.loanInfo.LoanTermConstPlusPerm : (this.Val("CONST.X1") != "Y" ? this.IntVal("325") + this.loanInfo.ConstPeriod : this.IntVal("325"));
      }
      double num1 = this.FltVal("358");
      if (this.loanInfo.LoanPurpose.Contains("Construction") && this.loanInfo.LoanType == "Conventional")
      {
        if (this.Val("1964") == "Y" || this.Val("1964") != "Y" && this.Val("Constr.Refi") != "Y")
          num1 = this.FltVal("CONST.X58") < this.FltVal("CONST.X59") ? this.FltVal("CONST.X58") : this.FltVal("CONST.X59");
        else if (this.Val("Constr.Refi") == "Y")
          num1 = this.FltVal("CONST.X59");
      }
      if (num1 == 0.0 && this.loanInfo.LoanPurpose.IndexOf("Construction") > -1)
        num1 = this.FltVal("356");
      this.loanInfo.MICutOffAmount = 0.0;
      if (this.Val("3000") == "Y")
        this.loanInfo.MICutOffAmount = Utils.ArithmeticRounding(this.FltVal("MAX23K.X6") * (this.FltVal("1205") / 100.0), 2);
      if (this.loanInfo.MICutOffAmount == 0.0)
        this.loanInfo.MICutOffAmount = this.loanInfo.LoanPurpose.IndexOf("Refinance") <= -1 ? (num1 != 0.0 ? num1 * (this.FltVal("1205") / 100.0) : this.FltVal("1821") * (this.FltVal("1205") / 100.0)) : (this.FltVal("356") != 0.0 ? this.FltVal("356") * (this.FltVal("1205") / 100.0) : this.FltVal("1821") * (this.FltVal("1205") / 100.0));
      this.loanInfo.DecliningRenewal = this.Val("3248") == "Y" && this.loanInfo.LoanType == "Conventional";
      this.loanInfo.mthPMIPrepaid = !(this.Val("2978") == "Y") ? (!(this.Val("HUD49") == "Y") ? 0 : this.IntVal("HUD32")) : this.IntVal("1209");
      this.loanInfo.PMIAmout = this.FltVal("1045");
      this.loanInfo.AmtCutOff80 = this.loanInfo.LoanType != "FarmersHomeAdministration" ? num1 * 0.8 : 0.0;
      this.loanInfo.AmtCutOff78 = this.loanInfo.LoanType != "FarmersHomeAdministration" ? num1 * 0.78 : 0.0;
      this.loanInfo.DateCutOff80 = string.Empty;
      this.loanInfo.DateCutOff78 = string.Empty;
      this.loanInfo.MIUseRemainBalance = this.Val("1775") == "Y" && this.Val("1172") != "Conventional";
      this.loanInfo.MIBasedOn = this.Val("1757");
      this.loanInfo.MIRatePMI = !(this.Val("1765") == "Y") || this.FltVal("1045") <= 0.0 ? this.FltVal("1107") / 100.0 : Utils.ArithmeticRounding(this.FltVal("1045") / this.FltVal("1109"), 5);
      this.loanInfo.ARMisRound = !this.loanInfo.IsARM || !this.useWorstCaseScenario ? (!this.loanInfo.IsARM || !this.useBestCaseScenario ? this.Val("SYS.X1") : "Down") : "Up";
      if (flag1)
      {
        this.loanInfo.YearsForGPM = 0;
        this.loanInfo.RateForGPM = 0.0;
        this.loanInfo.NegAdjCap = 0.0;
        this.loanInfo.NegAdjPeriod = 0;
        this.loanInfo.NegAdjRecast = 0.0;
        this.loanInfo.NegAdjStopAt = 0;
        this.loanInfo.NegMaxBalance = 0.0;
        this.loanInfo.UsingSteadyOptionLoan = false;
      }
      else
      {
        this.loanInfo.DiscountRate = this.FltVal("2551");
        this.loanInfo.DiscountPeriod = this.IntVal("2552");
        if (this.loanInfo.DiscountRate > 0.0 && this.loanInfo.DiscountPeriod > 0)
        {
          this.loanInfo.UsingSteadyOptionLoan = true;
          if (this.Val("2307") == "ofThePayment")
            this.loanInfo.DiscountRate /= 100.0;
        }
        else
          this.loanInfo.UsingSteadyOptionLoan = false;
        this.loanInfo.YearsForGPM = this.IntVal("1266");
        this.loanInfo.RateForGPM = this.FltVal("1267");
        this.loanInfo.NegAdjCap = this.FltVal("691") / 100.0;
        this.loanInfo.NegAdjPeriod = this.IntVal("690");
        this.loanInfo.NegAdjRecast = this.FltVal("1712");
        this.loanInfo.NegAdjStopAt = this.IntVal("1713");
        this.loanInfo.NegMaxBalance = Math.Round(this.FltVal("698") / 100.0 * this.loanInfo.LoanAmount, 2);
      }
      this.loanInfo.RateFactor = this.calculateRateFactorMonthly(1, this.loanInfo.LoanRate);
      this.loanInfo.year5Index = 60;
      this.loanInfo.DaysPerYearBiWeekly = this.IntVal("SYS.X2");
      if (this.loanInfo.DaysPerYearBiWeekly == 0)
        this.loanInfo.DaysPerYearBiWeekly = 360;
      this.loanInfo.RateFactorBiWeekly = this.loanInfo.LoanRate / (100.0 * ((double) this.loanInfo.DaysPerYearBiWeekly / 14.0));
      if (this.Val("423") == "Biweekly")
      {
        this.loanInfo.IsBiWeekly = true;
        this.loanInfo.NumberofPayPerYear = 26;
        if (this.loanInfo.ARMfirstChange > 0)
        {
          this.loanInfo.ARMfirstChange = this.loanInfo.ARMfirstChange / 6 * 13;
          if (this.loanInfo.ARMfirstChange == 0)
            this.loanInfo.ARMfirstChange = 13;
        }
        if (this.loanInfo.ARMadjPeriod > 0)
        {
          this.loanInfo.ARMadjPeriod = this.loanInfo.ARMadjPeriod / 6 * 13;
          if (this.loanInfo.ARMadjPeriod == 0)
            this.loanInfo.ARMadjPeriod = 13;
        }
        if (this.loanInfo.NegAdjPeriod > 0)
        {
          this.loanInfo.NegAdjPeriod = this.loanInfo.NegAdjPeriod / 6 * 13;
          if (this.loanInfo.NegAdjPeriod == 0)
            this.loanInfo.NegAdjPeriod = 13;
        }
        this.loanInfo.year5Index = 130;
        if (this.loanInfo.LoanTerm > 0)
          this.loanInfo.LoanTerm = Utils.ParseInt((object) ((double) (this.loanInfo.LoanTerm - this.loanInfo.LoanTerm % 12) / 12.0 * 26.0).ToString());
        if (this.loanInfo.BallonTerm > 0)
          this.loanInfo.BallonTerm = Utils.ParseInt((object) ((double) (this.loanInfo.BallonTerm / 12 * 26)).ToString());
        this.loanInfo.TotalMonthMI = 0;
        int num2 = this.IntVal("1198");
        if (num2 % 6 == 0)
          this.loanInfo.TotalMonthMI = num2 / 6 * 13;
        int num3 = this.IntVal("1200");
        if (num3 % 6 == 0)
          this.loanInfo.TotalMonthMI += num3 / 6 * 13;
        if (this.loanInfo.TotalMonthMI > this.loanInfo.LoanTerm)
          this.loanInfo.TotalMonthMI = this.loanInfo.LoanTerm;
        if (this.loanInfo.InterestOnly % 6 == 0)
          this.loanInfo.InterestOnly = this.loanInfo.InterestOnly / 6 * 13;
      }
      else
      {
        this.loanInfo.IsBiWeekly = false;
        this.loanInfo.NumberofPayPerYear = 12;
        this.loanInfo.TotalMonthMI = this.IntVal("1198") + this.IntVal("1200");
        if (this.loanInfo.TotalMonthMI > this.loanInfo.LoanTerm)
          this.loanInfo.TotalMonthMI = this.loanInfo.LoanTerm;
      }
      this.loanInfo.MImidpointCutoff = !(this.Val("1753") == "Y") ? 0 : Convert.ToInt32(Math.Ceiling((Decimal) this.loanInfo.LoanTerm / 2M));
      bool flag2 = this.Val("CASASRN.X141") == "Borrower";
      bool flag3 = this.Val("COMPLIANCEVERSION.CASASRNX141") == "Y" || this.Val("COMPLIANCEVERSION.NEWBUYDOWNENABLED") != "Y";
      this.loanInfo.TotalBuydownMonths = 0;
      if (flag2 | flag3 || !flag2 && this.useInterimServicingScenario)
      {
        if (!flag2 && this.useInterimServicingScenario)
        {
          for (int index = 4541; index <= 4546; ++index)
            this.loanInfo.TotalBuydownMonths += this.IntVal(string.Concat((object) index));
        }
        else
        {
          for (int index = 1613; index <= 1618; ++index)
            this.loanInfo.TotalBuydownMonths += this.IntVal(string.Concat((object) index));
        }
      }
      for (int index = 1269; index <= 1274; ++index)
      {
        double num4 = this.FltVal(flag2 | flag3 ? index.ToString() : (index + 3266).ToString());
        int num5 = (flag2 | flag3 ? index - 1269 : index + 3266 - 4535) * 4 + 3095;
        if (num4 == 0.0)
          this.SetVal(num5.ToString(), "");
        else
          this.SetCurrentNum(num5.ToString(), this.loanInfo.LoanRate - num4);
      }
      this.loanInfo.FullyIndexedRate = this.loanInfo.ARMmargin + this.loanInfo.ARMindex;
      this.loanInfo.AdjustedFullyIndexedRate = this.loanInfo.FullyIndexedRate;
      if (this.useWorstCaseScenario)
        this.loanInfo.RateCap = this.loanInfo.LoanRate + this.loanInfo.ARMlifeCap;
      else if (this.useBestCaseScenario)
        this.loanInfo.RateCap = this.loanInfo.ARMfloor;
      else if (this.loanInfo.LoanRate >= this.loanInfo.FullyIndexedRate && this.loanInfo.FullyIndexedRate != 0.0)
      {
        this.loanInfo.RateCap = this.loanInfo.ARMfloor > this.loanInfo.FullyIndexedRate ? this.loanInfo.ARMfloor : (this.loanInfo.ARMindex == 0.0 || this.loanInfo.UsingSteadyOptionLoan ? this.loanInfo.LoanRate + this.loanInfo.ARMlifeCap : this.loanInfo.FullyIndexedRate);
        if (this.loanInfo.RateCap > this.loanInfo.LoanRate + this.loanInfo.ARMlifeCap)
          this.loanInfo.RateCap = this.loanInfo.LoanRate + this.loanInfo.ARMlifeCap;
      }
      else
      {
        this.loanInfo.RateCap = this.loanInfo.LoanRate + this.loanInfo.ARMlifeCap <= this.loanInfo.FullyIndexedRate || this.loanInfo.FullyIndexedRate == 0.0 ? this.loanInfo.LoanRate + this.loanInfo.ARMlifeCap : (this.loanInfo.ARMindex != 0.0 ? this.loanInfo.FullyIndexedRate : this.loanInfo.LoanRate + this.loanInfo.ARMlifeCap);
        if (this.loanInfo.ARMfloor != 0.0 && this.loanInfo.ARMindex != 0.0)
        {
          if (this.loanInfo.LoanRate + this.loanInfo.ARMlifeCap < this.loanInfo.ARMfloor)
            this.loanInfo.RateCap = this.loanInfo.LoanRate + this.loanInfo.ARMlifeCap;
          else if (this.loanInfo.ARMfloor > this.loanInfo.FullyIndexedRate)
            this.loanInfo.RateCap = this.loanInfo.ARMfloor;
        }
      }
      this.loanInfo.HelocWorstCase = false;
      if (this.loanInfo.ARMround != 0.0 && this.loanInfo.RateCap != 0.0 && !this.useWorstCaseScenario && !this.useBestCaseScenario)
      {
        double rateCap = this.loanInfo.RateCap;
        double num6 = Math.Round(this.loanInfo.FullyIndexedRate - 0.49, 0);
        if (this.loanInfo.ARMisRound == "Up" || this.loanInfo.ARMisRound == "Down")
        {
          while (true)
          {
            while (!(this.loanInfo.ARMisRound == "Up"))
            {
              num6 += this.loanInfo.ARMround;
              if (num6 > this.loanInfo.FullyIndexedRate)
              {
                num6 -= this.loanInfo.ARMround;
                goto label_91;
              }
            }
            if (num6 < this.loanInfo.FullyIndexedRate)
              num6 += this.loanInfo.ARMround;
            else
              break;
          }
label_91:
          this.loanInfo.AdjustedFullyIndexedRate = num6;
          if (this.loanInfo.ARMisRound == "Up")
          {
            if (this.loanInfo.RateCap <= num6)
              this.loanInfo.RateCap = num6;
          }
          else
            this.loanInfo.RateCap = this.loanInfo.ARMfloor > num6 ? this.loanInfo.ARMfloor : num6;
        }
        else if (this.loanInfo.ARMindex > 0.0)
        {
          this.loanInfo.HelocWorstCase = true;
          double num7 = num6;
          double num8 = num6;
          do
          {
            num7 += this.loanInfo.ARMround;
          }
          while (num7 < this.loanInfo.FullyIndexedRate);
          do
          {
            num8 += this.loanInfo.ARMround;
          }
          while (num8 <= this.loanInfo.FullyIndexedRate);
          double num9 = num8 - this.loanInfo.ARMround;
          double num10 = num7 - this.loanInfo.FullyIndexedRate <= this.loanInfo.FullyIndexedRate - num9 ? num7 : num9;
          this.loanInfo.AdjustedFullyIndexedRate = num10;
          this.loanInfo.RateCap = this.loanInfo.ARMfloor >= this.loanInfo.FullyIndexedRate ? (num10 <= this.loanInfo.ARMfloor ? this.loanInfo.ARMfloor : num10) : (num10 < this.loanInfo.ARMfloor ? this.loanInfo.ARMfloor : num10);
        }
        if (this.loanInfo.LoanRate + this.loanInfo.ARMlifeCap < this.loanInfo.FullyIndexedRate)
          this.loanInfo.RateCap = this.loanInfo.LoanRate + this.loanInfo.ARMlifeCap;
      }
      if (this.loanInfo.ConstIntRate == 0.0)
        this.loanInfo.ConstIntRate = this.loanInfo.LoanRate;
      this.loanInfo.ConstTotalFinCharge = 0.0;
      this.loanInfo.DisbursementDate = Utils.ParseDate((object) this.Val("2553"));
      this.loanInfo.ConstInterestType = this.Val("1962");
      if (this.loanInfo.ConstInterestType == "")
        this.loanInfo.ConstInterestType = "360/360";
      if (this.loanInfo.LoanPurpose.IndexOf("Construction") > -1)
      {
        this.loanInfo.ConstMethod = this.Val("SYS.X6");
        if (this.loanInfo.ConstMethod == "")
          this.loanInfo.ConstMethod = "A";
        this.loanInfo.ConstEstClosingDate = Utils.ParseDate((object) this.Val("763"));
        if (this.loanInfo.ConstEstClosingDate == DateTime.MinValue)
          this.loanInfo.ConstEstClosingDate = DateTime.Today;
        if (this.loanInfo.LoanPurpose == "ConstructionOnly")
          this.loanInfo.ConstFinalPayDate = Utils.ParseDate((object) this.Val("1961"));
        if (this.loanInfo.ConstFinalPayDate == DateTime.MinValue)
          this.loanInfo.ConstFinalPayDate = this.loanInfo.ConstEstClosingDate.AddMonths(this.loanInfo.ConstPeriod);
        this.loanInfo.ConstFirstPayDate = Utils.ParseDate((object) this.Val("682"));
        if (this.loanInfo.ConstFirstPayDate == DateTime.MinValue)
        {
          DateTime dateTime = this.loanInfo.ConstEstClosingDate;
          if ((this.loanInfo.ConstInterestType == "365/365" || this.loanInfo.ConstInterestType == "365/360") && this.loanInfo.DisbursementDate != DateTime.MinValue)
            dateTime = this.loanInfo.DisbursementDate;
          this.loanInfo.ConstFirstPayDate = dateTime.AddMonths(1);
          if (this.loanInfo.ConstMethod == "B" && this.loanInfo.ConstInterestType.IndexOf("365") > -1)
          {
            this.loanInfo.ConstFirstPayDate = new DateTime(this.loanInfo.ConstFirstPayDate.Year, this.loanInfo.ConstFirstPayDate.Month, 1);
            this.loanInfo.ConstFinalPayDate = this.loanInfo.ConstFirstPayDate.AddMonths(this.loanInfo.ConstPeriod - 1);
          }
        }
        this.loanInfo.ConstFirstAmortDate = Utils.ParseDate((object) this.Val("1963"));
        if (this.loanInfo.ConstFirstAmortDate == DateTime.MinValue)
          this.loanInfo.ConstFirstAmortDate = this.loanInfo.ConstEstClosingDate.AddMonths(this.loanInfo.ConstPeriod + 1);
      }
      this.loanInfo.ConstUseLastDay = false;
      this.loanInfo.NeedToCalcEstConstIntrest = this.Val("4086") == "Y" && this.Val("4087") == "Y";
      this.loanInfo.UseSimpleInterest = this.Val("4749") == "Y" && !this.loanInfo.IsBiWeekly && this.loanInfo.LoanPurpose != "ConstructionOnly" && this.loanInfo.LoanType != "HELOC";
      this.loanInfo.SimpleInterestUse366ForLeapYear = this.Val("4748") == "Y";
      this.loanInfo.FirstPaymentDate = Utils.ParseDate((object) this.Val("682"));
      if (this.loanInfo.FirstPaymentDate == DateTime.MinValue)
        this.loanInfo.FirstPaymentDate = DateTime.Now;
      this.loanInfo.ZeroPercentPaymentOption = this.Val("4746");
    }

    internal bool PerformanceEnabled
    {
      get => this.performanceEnabled;
      set => this.performanceEnabled = value;
    }

    internal bool SkipPaymentScheduleCalculation
    {
      set => this.skipPaymentScheduleCalculation = value;
    }

    private void buildPaymentSchedule(string id, string val)
    {
      this.skipPaymentScheduleCalculation = this.performanceEnabled;
      this.calculatePaymentSchedule(id, val);
      if (!this.performanceEnabled)
        return;
      this.loan.Calculator.EditCalcOnDemandEnum(CalcOnDemandEnum.PaymentSchedule, true);
    }

    internal void CalcPaymentSchedule(string id, string val)
    {
      bool performanceEnabled = this.performanceEnabled;
      this.performanceEnabled = false;
      this.calObjs.D1003Cal.CalcField1109(id, val);
      this.performanceEnabled = performanceEnabled;
    }

    private double getSubjectPropertyValueForRemainingLTV()
    {
      double currentPropertyValue = this.FltVal("358");
      string str1 = this.Val("1172");
      string str2 = this.Val("19");
      if (str1 == "FHA")
      {
        if (currentPropertyValue == 0.0 && str2.Contains("Construction"))
          currentPropertyValue = this.FltVal("356");
      }
      else
        this.calObjs.Cal.CheckAndAdjustSubjectPropertyValueForConstructionLoans(ref currentPropertyValue);
      return currentPropertyValue;
    }

    private void calculatePaymentSchedule(string id, string val)
    {
      if (this.cacheFieldValues != null)
        this.cacheFieldValues.Clear();
      this.f_le1x24_cd4x34 = 0.0;
      if (!this.loanInfo.TestOnly)
      {
        this.setFieldValueToCache("5", "");
        this.setFieldValueToCache("4085", "");
        this.setFieldValueToCache("3054", "");
        this.setFieldValueToCache("RE88395.X323", "");
        this.setFieldValueToCache("RE88395.X324", "");
        this.setFieldValueToCache("RE88395.X325", "");
        this.setFieldValueToCache("RE88395.X327", "");
        this.setFieldValueToCache("LE3.X16", "");
        if (this.Val("1659") != "Y")
        {
          this.SetVal("RE88395.X121", "");
          this.SetVal("RE88395.X122", "");
          this.setFieldValueToCache("CalcNetTangibleBenefit", "1659");
          this.setFieldValueToCache("CalcPrepaymentPenaltyPeriod", "");
        }
        this.maximumPayment = 0.0;
        this.highestLoanRate = 0.0;
        this.lowestLoanRate = 9999.0;
        this.outstandingBalanceAfter1stAdjustment = 0.0;
        this.mortgageInsuranceAfter1stAdjustment = 0.0;
        this.SetVal("REGZ_TABLETYPE", this.RegzSummaryType.ToString());
        if (this.loanInfo.LoanType == "FarmersHomeAdministration")
        {
          this.setFieldValueToCache("109", "");
          this.setFieldValueToCache("118", "");
        }
      }
      this.SetCurrentNum("3296", this.loanInfo.IsARM ? (this.loanInfo.LoanType != "HELOC" ? Utils.ArithmeticRounding(this.loanInfo.AdjustedFullyIndexedRate, 3) : Utils.ArithmeticRounding(this.loanInfo.RateCap, 3)) : 0.0);
      this.loanInfo.LoanPeriod = 0;
      if (this.loanInfo.LoanAmount == 0.0 || this.loanInfo.LoanTerm == 0)
      {
        this.paySchedule[1] = new PaymentSchedule();
        this.paySchedule[1].Payment = 0.0;
        if (!this.loanInfo.TestOnly)
        {
          this.setFieldValueToCache("1206", "");
          this.SetCurrentNum("3290", 0.0, this.UseNoPayment(0.0));
          this.SetCurrentNum("3268", this.FltVal("3290") + this.FltVal("3267"));
          if (!this.skipPaymentScheduleCalculation)
            this.loan.Calculator.EditCalcOnDemandEnum(CalcOnDemandEnum.PaymentSchedule, false);
        }
        this.setCacheValuesToLoan();
      }
      else
      {
        double num1 = RegzCalculation.CalcRawMonthlyPayment(this.loanInfo.LoanTerm, this.loanInfo.LoanAmount, this.loanInfo.LoanRate, false, true, this.loanInfo.RateFactorBiWeekly, this.loanInfo.RateForGPM, (double) this.loanInfo.YearsForGPM, 26, this.IntVal("4"), this.loanInfo.LoanPeriod, this.loanInfo.LoanAmount, this.loanInfo.ZeroPercentPaymentOption, this.loanInfo.UseSimpleInterest, !(this.loanInfo.LoanPurpose == "ConstructionToPermanent") || this.loanInfo.LoanTermConstPlusPerm == this.loanInfo.LoanTerm ? this.loanInfo.FirstPaymentDate : this.loanInfo.ConstFirstAmortDate, this.loanInfo.ConstInterestType, this.loanInfo.SimpleInterestUse366ForLeapYear);
        if (this.loanInfo.InterestOnly > 0)
          num1 = Utils.ArithmeticRounding(this.loanInfo.RateFactorBiWeekly * this.loanInfo.LoanAmount, 2);
        this.SetCurrentNum("HUD51", num1);
        this.calObjs.Hud1Cal.CalcBiWeeklyEscrowPayment(id, val);
        double monthlyPayment = !this.loanInfo.UsingSteadyOptionLoan ? (!this.loanInfo.IsBiWeekly ? (!(this.loanInfo.LoanPurpose == "ConstructionToPermanent") || !this.UseNoPayment(0.0) ? (!(this.loanInfo.LoanPurpose == "ConstructionToPermanent") || this.loanInfo.LoanTermConstPlusPerm != this.loanInfo.LoanTerm ? this.CalcRawMonthlyPayment(this.loanInfo.LoanTerm - this.loanInfo.InterestOnly, this.loanInfo.LoanAmount, this.loanInfo.LoanRate, false) : this.CalcRawMonthlyPayment(this.loanInfo.LoanTerm - this.loanInfo.ConstPeriod - this.loanInfo.InterestOnly, this.loanInfo.LoanAmount, this.loanInfo.LoanRate, false)) : this.CalcRawMonthlyPayment(this.loanInfo.LoanTermConstPlusPerm, this.loanInfo.LoanAmount, this.loanInfo.LoanRate, false)) : RegzCalculation.CalcRawMonthlyPayment(this.loanInfo.LoanTerm - this.loanInfo.InterestOnly, this.loanInfo.LoanAmount, this.loanInfo.LoanRate, false, this.loanInfo.IsBiWeekly, this.loanInfo.RateFactorBiWeekly, this.loanInfo.RateForGPM, (double) this.loanInfo.YearsForGPM, this.loanInfo.NumberofPayPerYear, this.IntVal("4"), this.loanInfo.LoanPeriod, this.loanInfo.LoanAmount, this.loanInfo.ZeroPercentPaymentOption, this.loanInfo.UseSimpleInterest, this.loanInfo.FirstPaymentDate, this.loanInfo.ConstInterestType, this.loanInfo.SimpleInterestUse366ForLeapYear)) : this.CalcRawMonthlyPayment(this.loanInfo.LoanTerm, this.loanInfo.LoanAmount, this.loanInfo.LoanRate, false);
        this.loanInfo.BiweeklyPayment = monthlyPayment;
        this.loanInfo.MonthlyPayment = monthlyPayment;
        double finCharge = 0.0;
        int num2 = this.IntVal("1266") * this.loanInfo.NumberofPayPerYear;
        string str = this.Val("682");
        DateTime duePayDate = DateTime.Now;
        if (str != CalculationBase.nil)
        {
          if (str != "//")
          {
            try
            {
              duePayDate = Convert.ToDateTime(str);
            }
            catch (Exception ex)
            {
              this.setCacheValuesToLoan();
              Tracing.Log(RegzCalculation.sw, TraceLevel.Error, nameof (RegzCalculation), "exception in first payment date (Field 682): " + ex.Message);
              return;
            }
          }
        }
        if (this.loanInfo.LoanPurpose == "ConstructionToPermanent")
        {
          this.loanInfo.ConstSpanStartDate = DateTime.MinValue;
          duePayDate = this.loanInfo.ConstFirstPayDate;
          double val1 = this.calcConstructionInterest(this.loanInfo.ConstIntRate);
          if (this.loanInfo.ConstMethod == "B")
          {
            this.loanInfo.ConstProjectedMonthlyPayment = Utils.ArithmeticRounding(val1, 2);
          }
          else
          {
            this.loanInfo.ConstTotalFinCharge = val1;
            this.loanInfo.ConstProjectedMonthlyPayment = Utils.ArithmeticRounding(this.loanInfo.ConstTotalFinCharge / (double) this.loanInfo.ConstPeriod, 2);
          }
          this.loanInfo.ConstInPeriod = true;
        }
        else if (this.loanInfo.LoanPurpose == "ConstructionOnly")
        {
          this.loanInfo.ConstSpanStartDate = DateTime.MinValue;
          this.loanInfo.ConstTotalFinCharge = this.calcConstructionInterest(this.loanInfo.ConstIntRate);
          if (this.loanInfo.ConstMethod == "B")
          {
            duePayDate = this.loanInfo.ConstFirstPayDate;
            this.loanInfo.ConstProjectedMonthlyPayment = Utils.ArithmeticRounding(this.loanInfo.ConstTotalFinCharge, 2);
          }
          else
            this.loanInfo.ConstProjectedMonthlyPayment = Utils.ArithmeticRounding(this.loanInfo.ConstTotalFinCharge / (double) this.loanInfo.ConstPeriod, 2);
          this.loanInfo.ConstInPeriod = true;
        }
        double num3 = 0.0;
        double buydownRate = 0.0;
        double num4 = 0.0;
        int interestOnlyMonth = 1;
        int num5 = 1;
        double unpaidBalance = this.loanInfo.LoanAmount;
        double currentRate = this.loanInfo.LoanRate;
        if (this.loanInfo.LoanPurpose.StartsWith("Construction"))
          currentRate = this.loanInfo.ConstIntRate;
        if (this.loanInfo.LoanPurpose == "ConstructionToPermanent" && this.loanInfo.ConstIntRate == 0.0)
          this.loanInfo.ConstInPeriod = false;
        int idx = 0;
        bool flag1 = false;
        double num6 = 0.0;
        if (this.loanInfo.UsingSteadyOptionLoan)
          idx = this.buildOptionPayment(ref unpaidBalance, ref monthlyPayment, ref finCharge, ref duePayDate, ref interestOnlyMonth, ref currentRate);
        double val2 = unpaidBalance;
        double num7 = this.loanInfo.IsBiWeekly ? this.loanInfo.RateFactorBiWeekly : this.loanInfo.RateFactor;
        double num8 = this.loanInfo.IsBiWeekly ? this.loanInfo.BiweeklyPayment : this.loanInfo.MonthlyPayment;
        double val3 = 0.0;
        bool flag2 = this.paySchedule[1] == null;
        DateTime dateTime1 = DateTime.MaxValue;
        int day = duePayDate.Day;
        while (unpaidBalance > 0.0 && (!this.skipPaymentScheduleCalculation || !this.loanInfo.TestOnly || flag2))
        {
          ++this.loanInfo.LoanPeriod;
          ++idx;
          this.paySchedule[idx - 1] = new PaymentSchedule();
          if (this.loanInfo.UseSimpleInterest && (this.loanInfo.ConstInterestType == "365/365" || this.loanInfo.ConstInterestType == "365/360") && (!(this.loanInfo.LoanPurpose == "ConstructionToPermanent") || !this.loanInfo.ConstInPeriod))
            this.calculateRateFactor(idx, currentRate);
          DateTime dateTime2;
          if (!this.loanInfo.TestOnly && this.loanInfo.LoanPeriod >= this.loanInfo.ARMfirstChange + 1 && this.loanInfo.ARMfirstChange > 0 && (this.loanInfo.LoanRate != 0.0 || this.loanInfo.LoanRate == 0.0 && (string.IsNullOrEmpty(this.loanInfo.ZeroPercentPaymentOption) || this.loanInfo.ZeroPercentPaymentOption == "AmortizingPayment")))
          {
            double num9 = this.checkARMSettings(idx, currentRate);
            if (num9 > 0.0)
            {
              currentRate = num9;
              if (this.loanInfo.NegAdjPeriod > 0 && (double) this.loanInfo.LoanPeriod <= this.loanInfo.NegAdjRecast)
              {
                if (this.loanInfo.LoanPeriod % this.loanInfo.NegAdjPeriod == 1 || unpaidBalance > this.loanInfo.NegMaxBalance && this.loanInfo.NegMaxBalance > 0.0)
                  monthlyPayment = this.CalcRawMonthlyPayment(this.loanInfo.LoanTerm - this.loanInfo.LoanPeriod + 1, unpaidBalance, currentRate, false);
              }
              else
                monthlyPayment = this.loanInfo.LoanTermConstPlusPerm <= 0 ? this.CalcRawMonthlyPayment(this.loanInfo.LoanTerm - this.loanInfo.LoanPeriod + 1, unpaidBalance, currentRate, false) : this.CalcRawMonthlyPayment(this.loanInfo.LoanTermConstPlusPerm - this.loanInfo.LoanPeriod + 1, unpaidBalance, currentRate, false);
              if (this.loanInfo.LoanPeriod == this.loanInfo.ARMfirstChange + 1 && duePayDate != DateTime.MinValue)
              {
                if (!(dateTime1 == DateTime.MaxValue))
                {
                  DateTime date1 = dateTime1.Date;
                  dateTime2 = duePayDate.AddMonths(-1);
                  DateTime date2 = dateTime2.Date;
                  if (!(date1 > date2))
                    goto label_49;
                }
                dateTime1 = duePayDate.AddMonths(-1);
              }
            }
            else if (this.loanInfo.InterestOnly == this.loanInfo.LoanPeriod && this.loanInfo.InterestOnly > 0)
              monthlyPayment = this.CalcRawMonthlyPayment(this.loanInfo.LoanTerm - this.loanInfo.LoanPeriod, unpaidBalance, currentRate, false);
label_49:
            double num10 = this.checkNegativeARM(this.paySchedule[idx - 2].Payment, unpaidBalance, currentRate);
            if (num10 > 0.0)
              monthlyPayment = num10;
            if ((double) this.loanInfo.LoanPeriod == this.loanInfo.NegAdjRecast + 1.0 && this.loanInfo.NegAdjRecast != 0.0)
              monthlyPayment = this.CalcRawMonthlyPayment(this.loanInfo.LoanTerm - this.loanInfo.LoanPeriod + 1, unpaidBalance, currentRate, false);
          }
          else if (this.loanInfo.LoanTerm >= this.loanInfo.LoanPeriod && this.loanInfo.TotalBuydownMonths > 0 && this.loanInfo.LoanPeriod == this.loanInfo.TotalBuydownMonths + 1)
            monthlyPayment = this.CalcRawMonthlyPayment(this.loanInfo.LoanTerm - this.loanInfo.LoanPeriod + 1, unpaidBalance, currentRate, false);
          double val4;
          if (this.loanInfo.LoanPeriod <= this.loanInfo.TotalBuydownMonths)
          {
            if (this.loanInfo.LoanPeriod == 1 && !this.loanInfo.TestOnly)
              this.SetCurrentNum("BUYDOWNPAYMENT", monthlyPayment);
            val4 = this.calcBuydownInterest(this.loanInfo.LoanPeriod, unpaidBalance, ref buydownRate);
            this.paySchedule[idx - 1].CurrentRate = buydownRate;
            monthlyPayment = this.loanInfo.BuydownMonthlyPayment;
            this.paySchedule[idx - 1].BuydownSubsidyAmount = Utils.ArithmeticRounding(this.loanInfo.MonthlyPayment - this.loanInfo.BuydownMonthlyPayment, 2);
          }
          else
          {
            if (this.loanInfo.TotalBuydownMonths == 0 && this.loanInfo.LoanPeriod == 1 && !this.loanInfo.TestOnly)
              this.SetCurrentNum("BUYDOWNPAYMENT", 0.0);
            double val5 = !this.loanInfo.IsBiWeekly ? unpaidBalance * this.loanInfo.RateFactor : unpaidBalance * this.loanInfo.RateFactorBiWeekly;
            if (this.loanInfo.LoanPurpose == "ConstructionOnly" && this.loanInfo.ConstMethod != "B" && this.loanInfo.IsARM)
            {
              if (this.f_le1x24_cd4x34 < val5)
                this.f_le1x24_cd4x34 = val5;
              val5 /= 2.0;
            }
            val4 = Utils.ArithmeticRounding(val5, 2);
            if (this.loanInfo.YearsForGPM == 0 && this.loanInfo.ARMfirstChange == 0)
              monthlyPayment = this.loanInfo.MonthlyPayment;
            this.paySchedule[idx - 1].CurrentRate = currentRate;
          }
          if (this.loanInfo.YearsForGPM > 0 && this.loanInfo.LoanPeriod == this.loanInfo.YearsForGPM * this.loanInfo.NumberofPayPerYear + 1)
            monthlyPayment = this.CalcRawMonthlyPayment(this.loanInfo.LoanTerm - this.loanInfo.YearsForGPM * 12, unpaidBalance, currentRate, false);
          if (this.loanInfo.LoanPeriod <= num2)
          {
            if (this.loanInfo.LoanPeriod == 1)
              num3 = monthlyPayment;
            if (this.loanInfo.LoanPeriod % this.loanInfo.NumberofPayPerYear == 1 && this.loanInfo.RateForGPM != 0.0)
            {
              num3 = this.CalcRawMonthlyPayment(this.loanInfo.LoanTerm, this.loanInfo.LoanAmount, this.loanInfo.LoanRate, true);
              if (this.loanInfo.IsBiWeekly)
              {
                num3 = Utils.ArithmeticRounding(num3 / 2.0, 2);
                this.loanInfo.BiweeklyPayment = num3;
              }
            }
            monthlyPayment = num3;
            ++num5;
          }
          if (num4 == 0.0 && this.loanInfo.InterestOnly > 0)
            num4 = val4;
          if (this.loanInfo.LoanPurpose.StartsWith("Construction") && this.loanInfo.ConstPeriod > 0 && this.loanInfo.ConstInPeriod)
          {
            if (this.loanInfo.IsARM && this.loanInfo.LoanPurpose == "ConstructionOnly")
            {
              this.paySchedule[idx - 1].CurrentRate = currentRate;
            }
            else
            {
              this.paySchedule[idx - 1].CurrentRate = this.loanInfo.ConstIntRate;
              val4 = this.calcConstructionInterest(this.paySchedule[idx - 1].CurrentRate);
            }
            if (this.loanInfo.LoanPeriod == 1 && !this.loanInfo.TestOnly)
            {
              if (this.loanInfo.LoanType != "HELOC")
                this.setFieldValueToCache("5", string.Concat((object) Utils.ArithmeticRounding(this.loanInfo.ConstMonthlyPayment, 2)));
              this.setFieldValueToCache("4085", this.getFieldValueFromCache("5"));
              this.setFieldValueToCache("CalcNetTangibleBenefit", "5");
              this.setFieldValueToCache("CalcPrepaymentPenaltyPeriod", "");
            }
          }
          double val6 = !this.loanInfo.IsBiWeekly ? monthlyPayment - val4 + this.loanInfo.ExtraPayment : monthlyPayment - val4 + this.loanInfo.ExtraPayment;
          if (val6 < 0.0)
            val6 = 0.0;
          else if (val6 > unpaidBalance && this.loanInfo.UseSimpleInterest && this.loanInfo.InterestOnly > 0 && interestOnlyMonth <= this.loanInfo.InterestOnly)
            val6 = unpaidBalance;
          double val7 = Utils.ArithmeticRounding(val6, 2);
          if (this.loanInfo.NegMaxBalance > 0.0 && !this.loanInfo.UsingSteadyOptionLoan && unpaidBalance - val7 > this.loanInfo.NegMaxBalance)
          {
            val7 = Utils.ArithmeticRounding(unpaidBalance - this.loanInfo.NegMaxBalance, 2);
            monthlyPayment = val7 + val4 + this.loanInfo.ExtraPayment;
          }
          if (this.loanInfo.LoanPurpose.StartsWith("Construction") && this.loanInfo.ConstPeriod > 0)
          {
            if (this.loanInfo.LoanPurpose == "ConstructionOnly" || this.loanInfo.LoanPurpose == "ConstructionToPermanent" && this.loanInfo.ConstInPeriod)
            {
              if (this.loanInfo.LoanPurpose == "ConstructionToPermanent")
              {
                this.paySchedule[idx - 1].Interest = Utils.ArithmeticRounding(this.loanInfo.ConstMonthlyPayment, 2);
                val7 = 0.0;
              }
              else if (this.loanInfo.ConstMethod == "A")
              {
                this.paySchedule[idx - 1].Interest = 0.0;
                val7 = unpaidBalance;
              }
              else
              {
                this.paySchedule[idx - 1].Interest = val4;
                val7 = this.loanInfo.LoanPeriod < this.loanInfo.ConstPeriod ? 0.0 : this.loanInfo.LoanAmount;
              }
            }
            else
            {
              this.paySchedule[idx - 1].Interest = val4;
              if (this.loanInfo.ConstInPeriod)
                val7 = 0.0;
            }
          }
          else
            this.paySchedule[idx - 1].Interest = val4;
          if (this.loanInfo.LoanPurpose == "ConstructionToPermanent" && this.loanInfo.ConstInPeriod && this.loanInfo.ConstMethod == "B")
            this.loanInfo.ConstTotalFinCharge += val4;
          this.paySchedule[idx - 1].MortgageInsurance = 0.0;
          if (val7 <= unpaidBalance && this.loanInfo.LoanPeriod <= this.loanInfo.ConstPeriod && this.loanInfo.LoanPurpose == "ConstructionOnly")
          {
            if (this.loanInfo.LoanPeriod >= this.loanInfo.ConstPeriod)
            {
              val7 = unpaidBalance;
              unpaidBalance = 0.0;
            }
          }
          else if (val7 <= unpaidBalance && this.loanInfo.LoanPeriod < this.loanInfo.BallonTerm)
          {
            if (interestOnlyMonth > this.loanInfo.InterestOnly)
            {
              if (this.loanInfo.IsBiWeekly || !this.loanInfo.IsBiWeekly && (this.loanInfo.LoanPeriod < this.loanInfo.LoanTerm || this.loanInfo.LoanTermConstPlusPerm > 0 && this.loanInfo.LoanPeriod < this.loanInfo.LoanTermConstPlusPerm))
              {
                unpaidBalance = Utils.ArithmeticRounding(unpaidBalance - val7, 2);
              }
              else
              {
                val7 = unpaidBalance;
                unpaidBalance = 0.0;
              }
            }
          }
          else
          {
            val7 = unpaidBalance;
            unpaidBalance = 0.0;
          }
          this.paySchedule[idx - 1].Balance = Utils.ArithmeticRounding(unpaidBalance, 2);
          if (this.loanInfo.IsARM && this.loanInfo.LoanType != "Conventional")
          {
            val2 -= Utils.ArithmeticRounding(num8 - val2 * num7, 2);
            if (val2 < 0.0)
              val2 = 0.0;
            this.paySchedule[idx - 1].BalanceForMI = Utils.ArithmeticRounding(val2, 2);
          }
          else
            this.paySchedule[idx - 1].BalanceForMI = this.paySchedule[idx - 1].Balance;
          if (unpaidBalance == 0.0 && this.loanInfo.LoanRate == 0.0 && this.loanInfo.ZeroPercentPaymentOption == "NoPayment")
          {
            this.paySchedule[idx - 1].Payment = 0.0;
            this.paySchedule[idx - 1].Interest = 0.0;
            this.paySchedule[idx - 1].Principal = 0.0;
          }
          else if (this.loanInfo.ConstPeriod > 0 && (this.loanInfo.LoanPurpose == "ConstructionOnly" || this.loanInfo.LoanPurpose == "ConstructionToPermanent" && this.loanInfo.ConstInPeriod))
          {
            if (this.loanInfo.LoanPurpose == "ConstructionOnly" && this.loanInfo.LoanPeriod >= this.loanInfo.ConstPeriod)
            {
              this.paySchedule[idx - 1].Principal = val7;
              if (this.loanInfo.IsARM)
              {
                this.paySchedule[idx - 1].Interest = !(this.loanInfo.ConstMethod == "B") ? val4 : Utils.ArithmeticRounding(val4, 2);
                this.paySchedule[idx - 1].Payment = val7 + this.paySchedule[idx - 1].Interest;
              }
              else
              {
                this.paySchedule[idx - 1].Payment = val7 + this.loanInfo.ConstProjectedMonthlyPayment;
                this.paySchedule[idx - 1].Interest = this.loanInfo.ConstProjectedMonthlyPayment;
              }
              val3 += this.paySchedule[idx - 1].Interest;
            }
            else if (this.loanInfo.ConstPeriod > 0 && this.loanInfo.ConstInPeriod)
            {
              this.paySchedule[idx - 1].Principal = 0.0;
              this.paySchedule[idx - 1].Payment = !this.loanInfo.IsARM || !(this.loanInfo.LoanPurpose == "ConstructionOnly") && this.paySchedule[idx - 1].CurrentRate == this.loanInfo.ConstIntRate ? (this.paySchedule[idx - 1].Interest = this.loanInfo.ConstProjectedMonthlyPayment) : (!(this.loanInfo.ConstMethod == "B") ? (this.paySchedule[idx - 1].Interest = val4) : (this.paySchedule[idx - 1].Interest = Utils.ArithmeticRounding(val4, 2)));
              val3 += this.paySchedule[idx - 1].Interest;
            }
          }
          else if (interestOnlyMonth > this.loanInfo.InterestOnly || !this.loanInfo.IsBiWeekly && this.loanInfo.LoanPeriod >= this.loanInfo.LoanTerm)
          {
            this.paySchedule[idx - 1].Principal = Utils.ArithmeticRounding(val7, 2);
            this.paySchedule[idx - 1].Payment = this.loanInfo.ConstPeriod <= 0 || !(this.loanInfo.ConstMethod == "A") || !(this.loanInfo.LoanPurpose == "ConstructionOnly") ? (this.loanInfo.ConstPeriod <= 0 || !(this.loanInfo.ConstMethod == "A") || !(this.loanInfo.LoanPurpose == "ConstructionToPermanent") || !this.loanInfo.ConstInPeriod ? Utils.ArithmeticRounding(val7 + val4, 2) : this.loanInfo.ConstMonthlyPayment) : val7;
          }
          else
          {
            this.paySchedule[idx - 1].Principal = 0.0;
            this.paySchedule[idx - 1].Payment = Utils.ArithmeticRounding(val4, 2);
          }
          if (this.maximumPayment < this.paySchedule[idx - 1].Payment && this.loanInfo.LoanPeriod < this.loanInfo.LoanTerm)
            this.maximumPayment = this.paySchedule[idx - 1].Payment;
          this.paySchedule[idx - 1].PayDate = duePayDate.ToString("MM/dd/yyyy");
          if (this.loanInfo.LoanPurpose != "ConstructionToPermanent" && interestOnlyMonth <= this.loanInfo.InterestOnly || this.loanInfo.ConstPeriod > 0 && this.loanInfo.LoanPurpose == "ConstructionToPermanent" && !this.loanInfo.ConstInPeriod && interestOnlyMonth <= this.loanInfo.InterestOnly)
            ++interestOnlyMonth;
          if (this.loanInfo.InterestOnly > 0 && this.loanInfo.LoanPeriod == this.loanInfo.InterestOnly + 1 && this.paySchedule[idx - 1].Principal > 0.0)
          {
            if (!(dateTime1 == DateTime.MaxValue))
            {
              DateTime date3 = dateTime1.Date;
              dateTime2 = duePayDate.AddMonths(-1);
              DateTime date4 = dateTime2.Date;
              if (!(date3 > date4))
                goto label_140;
            }
            dateTime1 = duePayDate.AddMonths(-1);
          }
label_140:
          if (this.loanInfo.LoanType != "FarmersHomeAdministration")
            this.findCutoffDates();
          duePayDate = !this.loanInfo.IsBiWeekly ? duePayDate.AddMonths(1) : duePayDate.AddDays(14.0);
          if (!this.loanInfo.IsBiWeekly && duePayDate.Day != day && duePayDate.Day < day)
          {
            dateTime2 = duePayDate.AddDays((double) (day - duePayDate.Day));
            if (dateTime2.Month == duePayDate.Month)
              duePayDate = duePayDate.AddDays((double) (day - duePayDate.Day));
          }
          if (this.loanInfo.LoanPurpose == "ConstructionOnly" && this.loanInfo.ConstMethod == "A" && this.loanInfo.IsARM)
            finCharge += this.paySchedule == null || this.paySchedule[idx - 1] == null ? 0.0 : this.paySchedule[idx - 1].Interest + this.paySchedule[idx - 1].MortgageInsurance;
          else if (this.loanInfo.ConstPeriod > 0 && this.loanInfo.LoanPurpose == "ConstructionToPermanent")
          {
            if (this.loanInfo.ConstMethod == "B" || this.loanInfo.ConstInPeriod && this.loanInfo.LoanPeriod == 1 || !this.loanInfo.ConstInPeriod)
              finCharge += Utils.ArithmeticRounding(val4, 2);
          }
          else if (this.loanInfo.LoanPurpose != "ConstructionOnly" || this.loanInfo.ConstMethod != "A")
            finCharge += Utils.ArithmeticRounding(val4, 2);
          if (this.loanInfo.LoanPeriod == 1 && !this.loanInfo.TestOnly)
          {
            if (!this.loanInfo.IsBiWeekly)
            {
              if (this.loanInfo.LoanType != "HELOC")
                this.setFieldValueToCache("5", string.Concat((object) this.paySchedule[idx - 1].Payment));
              this.setFieldValueToCache("4085", this.getFieldValueFromCache("5"));
              this.setFieldValueToCache("3034", "");
            }
            else
            {
              if (this.loanInfo.LoanType != "HELOC")
                this.setFieldValueToCache("5", string.Concat((object) Utils.ArithmeticRounding(Utils.ParseDouble((object) this.getFieldValueFromCache("HUD51")) * 26.0 / 12.0, 2)));
              this.setFieldValueToCache("4085", this.getFieldValueFromCache("5"));
              this.setFieldValueToCache("3034", string.Concat((object) this.paySchedule[idx - 1].Payment));
            }
            this.setFieldValueToCache("CalcNetTangibleBenefit", "5");
            this.setFieldValueToCache("CalcPrepaymentPenaltyPeriod", "");
          }
          if (!flag1 && this.loanInfo.ARMType != "Fixed" && idx > 1 && !this.loanInfo.TestOnly && (this.paySchedule[idx - 1].CurrentRate != this.paySchedule[idx - 2].CurrentRate && this.paySchedule[idx - 1].CurrentRate > 0.0 || unpaidBalance > this.loanInfo.NegMaxBalance && this.loanInfo.NegMaxBalance > 0.0))
          {
            flag1 = true;
            this.setFieldValueToCache("RE88395.X323", string.Concat((object) this.paySchedule[idx - 1].CurrentRate));
            this.setFieldValueToCache("RE88395.X324", string.Concat((object) this.paySchedule[idx - 1].Payment));
            this.setFieldValueToCache("RE88395.X325", string.Concat((object) this.CalcRawMonthlyPayment(this.loanInfo.LoanTerm - this.loanInfo.LoanPeriod + 1, this.paySchedule[idx - 2].Balance, this.FltVal("2625"), false)));
            this.setFieldValueToCache("RE88395.X327", string.Concat((object) (this.loanInfo.LoanPeriod - 1)));
          }
          if (!this.loanInfo.TestOnly)
          {
            if (this.loanInfo.LoanPeriod == this.loanInfo.ARMfirstChange && this.loanInfo.ARMfirstChange > 0)
              this.outstandingBalanceAfter1stAdjustment = unpaidBalance;
            if (this.paySchedule[idx - 1].CurrentRate > this.highestLoanRate)
              this.highestLoanRate = this.paySchedule[idx - 1].CurrentRate;
            if (this.paySchedule[idx - 1].CurrentRate < this.lowestLoanRate)
              this.lowestLoanRate = this.paySchedule[idx - 1].CurrentRate;
            if (this.loanInfo.LoanPurpose != "ConstructionToPermanent" || this.loanInfo.ConstPeriod > 0 && this.loanInfo.LoanPurpose == "ConstructionToPermanent" && this.loanInfo.LoanPeriod > this.loanInfo.ConstPeriod)
              num6 += this.paySchedule[idx - 1].Interest;
          }
          if ((this.loanInfo.IsBiWeekly || this.loanInfo.LoanPeriod <= this.loanInfo.LoanTerm || this.loanInfo.LoanTermConstPlusPerm <= 0 || this.loanInfo.LoanPeriod <= this.loanInfo.LoanTermConstPlusPerm) && this.loanInfo.LoanPeriod < 1300 && (this.loanInfo.InterestOnly <= 0 || idx < this.loanInfo.InterestOnly || idx < this.loanInfo.BallonTerm))
          {
            if (this.loanInfo.ConstPeriod > 0 && this.loanInfo.LoanPurpose.StartsWith("Construction") && this.loanInfo.ConstInPeriod)
            {
              if (this.loanInfo.ConstMethod == "A" && this.loanInfo.LoanPeriod >= 1)
              {
                if (this.loanInfo.LoanPurpose == "ConstructionToPermanent" && this.loanInfo.LoanPeriod >= this.loanInfo.ConstPeriod)
                {
                  this.loanInfo.ConstInPeriod = false;
                  currentRate = this.loanInfo.LoanRate;
                  duePayDate = this.loanInfo.ConstFirstAmortDate;
                }
              }
              else if (this.loanInfo.ConstMethod == "B" && this.loanInfo.LoanPeriod >= this.loanInfo.ConstPeriod)
              {
                if (this.loanInfo.LoanPurpose != "ConstructionToPermanent")
                {
                  this.loanInfo.ConstInPeriod = false;
                  duePayDate = this.loanInfo.ConstFirstAmortDate;
                  this.loanInfo.LoanPeriod = 0;
                  interestOnlyMonth = 1;
                }
                else
                {
                  this.loanInfo.ConstInPeriod = false;
                  currentRate = this.loanInfo.LoanRate;
                  duePayDate = this.loanInfo.ConstFirstAmortDate;
                }
              }
            }
            if (this.skipPaymentScheduleCalculation && idx == 1 && !flag2)
            {
              this.loan.Calculator.EditCalcOnDemandEnum(CalcOnDemandEnum.PaymentSchedule, true);
              break;
            }
          }
          else
            break;
        }
        if (this.loanInfo.NeedToCalcEstConstIntrest && this.loanInfo.LoanPurpose == "ConstructionOnly" && this.loanInfo.IsARM)
        {
          double num11 = 0.0;
          idx = 0;
          while (this.paySchedule[idx] != null && idx < this.loanInfo.ConstPeriod)
          {
            ++idx;
            double rateFactorMonthly = this.calculateRateFactorMonthly(idx, this.paySchedule[idx - 1].CurrentRate);
            double num12 = Utils.ArithmeticRounding(0.5 * val3 * rateFactorMonthly, 2);
            this.paySchedule[idx - 1].Interest += num12;
            this.paySchedule[idx - 1].Payment += num12;
            num11 += num12;
          }
          val3 += num11;
        }
        if (!this.skipPaymentScheduleCalculation)
          this.loan.Calculator.EditCalcOnDemandEnum(CalcOnDemandEnum.PaymentSchedule, false);
        this.calculateLateFeePaymentInRegz(id, val);
        this.loanInfo.LoanPeriod = idx;
        if (this.loanInfo.TestOnly)
          return;
        if (dateTime1 != DateTime.MaxValue)
          this.setFieldValueToCache("3054", dateTime1.ToString("MM/dd/yyyy"));
        if (idx >= 2 && (this.loanInfo.BallonTerm > 0 && this.loanInfo.InterestOnly >= this.loanInfo.BallonTerm || this.loanInfo.LoanTerm > 0 && this.loanInfo.InterestOnly >= this.loanInfo.LoanTerm))
        {
          this.paySchedule[idx - 1].Balance = 0.0;
          this.paySchedule[idx - 1].Principal = this.paySchedule[idx - 2].Balance;
          this.paySchedule[idx - 1].Payment = Utils.ArithmeticRounding(this.paySchedule[idx - 1].Interest + this.paySchedule[idx - 1].MortgageInsurance + this.paySchedule[idx - 1].Principal, 2);
        }
        double val8 = num6 + this.FltVal("NEWHUD2.X2158");
        if (this.loanInfo.LoanPurpose == "ConstructionOnly" || this.loanInfo.LoanPurpose == "ConstructionToPermanent")
        {
          if (this.loanInfo.LoanPurpose == "ConstructionOnly")
          {
            if (!this.loanInfo.IsARM)
              val3 = this.loanInfo.ConstTotalFinCharge;
          }
          else
            val3 = this.calcConstructionInterest(this.loanInfo.ConstIntRate);
          if (this.loanInfo.ConstMethod == "B" && (!(this.loanInfo.LoanPurpose == "ConstructionOnly") || !this.loanInfo.IsARM))
            val3 *= (double) this.loanInfo.ConstPeriod;
          double num13 = Utils.ArithmeticRounding(val3, 2);
          this.setFieldValueToCache("4088", string.Concat((object) num13));
          this.loanInfo.ConstReqRsv = num13;
        }
        else
        {
          this.setFieldValueToCache("4088", "");
          this.loanInfo.ConstReqRsv = this.FltVal("1265");
        }
        if (val8 != 0.0 || this.loanInfo.LoanAmount != 0.0)
        {
          if (this.loanInfo.LoanPurpose == "ConstructionOnly")
            this.setFieldValueToCache("LE3.X16", string.Concat((object) Utils.ArithmeticRounding((Utils.ParseDouble((object) this.getFieldValueFromCache("4088")) + this.FltVal("334")) / this.FltVal("2") * 100.0, 3)));
          else
            this.setFieldValueToCache("LE3.X16", string.Concat((object) Utils.ArithmeticRounding((Utils.ArithmeticRounding(val8, 2) + (this.loanInfo.LoanPurpose == "ConstructionToPermanent" ? Utils.ParseDouble((object) this.getFieldValueFromCache("4088")) : 0.0)) / this.FltVal("2") * 100.0, 3)));
        }
        this.calObjs.VACal.CalcVALA(id, val);
        this.calObjs.Cal.CalcLifeInsurance(id, val);
        if (this.loanInfo.DateCutOff78 == string.Empty && this.loanInfo.AmtCutOff78 > 0.0)
          this.loanInfo.DateCutOff78 = "balloon";
        if (this.loanInfo.DateCutOff80 == string.Empty && this.loanInfo.AmtCutOff80 > 0.0)
          this.loanInfo.DateCutOff80 = "balloon";
        if (!this.skipPaymentScheduleCalculation && this.loanInfo.LoanPeriod > 0)
          this.SetVal("78", this.paySchedule[idx - 1].PayDate);
        if (!this.skipPaymentScheduleCalculation && this.loanInfo.LoanPurpose == "ConstructionToPermanent" && idx > 0)
          this.SetVal("1961", this.paySchedule[idx - 1].PayDate);
        if (this.loanInfo.LoanPurpose == "ConstructionOnly")
        {
          this.setFieldValueToCache("109", "");
          this.setFieldValueToCache("118", "");
        }
        else
        {
          if (!this.skipPaymentScheduleCalculation && this.loanInfo.DateCutOff80 != null && this.loanInfo.LoanType != "FarmersHomeAdministration")
            this.setFieldValueToCache("109", this.loanInfo.DateCutOff80);
          if (!this.skipPaymentScheduleCalculation && this.loanInfo.DateCutOff78 != null && this.loanInfo.LoanType != "FarmersHomeAdministration")
            this.setFieldValueToCache("118", this.loanInfo.DateCutOff78);
        }
        double num14;
        if (!this.skipPaymentScheduleCalculation | flag2)
        {
          double val9 = finCharge + this.calcMortgageInsurance(id, this.loanInfo.LoanPeriod);
          if (this.Val("1172") != "FarmersHomeAdministration")
            val9 -= this.shiftPremiumMI();
          this.totalPrincipalIn5Years = 0.0;
          this.totalInterestIn5Years = 0.0;
          this.totalMIIn5Years = 0.0;
          int num15 = this.Val("423") == "Biweekly" ? (this.loanInfo.LoanPeriod > 130 ? 130 : this.loanInfo.LoanPeriod) : (this.loanInfo.LoanPeriod > 60 ? 60 : this.loanInfo.LoanPeriod);
          for (int index = 0; index < num15; ++index)
          {
            this.totalPrincipalIn5Years += this.paySchedule[index].Principal;
            if (this.loanInfo.LoanPurpose != "ConstructionToPermanent" || this.loanInfo.LoanPurpose == "ConstructionToPermanent" && index >= this.loanInfo.ConstPeriod)
              this.totalInterestIn5Years += this.paySchedule[index].Interest;
            this.totalMIIn5Years += this.paySchedule[index].MortgageInsurance;
          }
          if (this.loanInfo.LoanPurpose == "ConstructionOnly" && this.loanInfo.ConstMethod == "A" && !this.loanInfo.IsARM)
            val9 += this.loanInfo.ConstTotalFinCharge;
          if (this.loanInfo.ARMfirstChange > 0 && this.paySchedule[this.loanInfo.ARMfirstChange] != null)
            this.mortgageInsuranceAfter1stAdjustment = this.paySchedule[this.loanInfo.ARMfirstChange].MortgageInsurance;
          num14 = Utils.ArithmeticRounding(val9, 2) + this.FltVal("949");
          if (this.loanInfo.LoanPurpose == "ConstructionOnly")
            num14 += this.FltVal("NEWHUD2.X4768");
        }
        else
          num14 = 0.0;
        this.setFieldValueToCache("1206", string.Concat((object) num14));
        this.setCacheValuesToLoan();
        this.calObjs.NewHud2015Cal.CalcLECDDisplayFields("LE3.X16", (string) null);
        if ((this.UseNewGFEHUD || this.UseNew2015GFEHUD) && !this.useWorstCaseScenario && !this.useBestCaseScenario)
        {
          this.calObjs.NewHudCal.CalcREGZGFEHudSummary(id, val);
          this.calObjs.MLDSCal.CalcRE882(id, val);
        }
        if (this.loanInfo.LoanPeriod > this.loanInfo.year5Index && this.paySchedule.Length >= this.loanInfo.year5Index - 1 && this.paySchedule[this.loanInfo.year5Index - 1] != null && this.Val("14") == "CA" && (this.Val("608") != "" || this.Val("1172") != ""))
        {
          bool showZero = this.UseNoPayment(this.paySchedule[this.loanInfo.year5Index].Payment);
          if (this.Val("608") == "Fixed")
          {
            this.SetCurrentNum("RE88395.X290", this.paySchedule[this.loanInfo.year5Index].Payment, showZero);
            this.SetCurrentNum("RE88395.X291", this.paySchedule[this.loanInfo.year5Index].Payment, showZero);
            this.SetCurrentNum("RE88395.X294", this.paySchedule[this.loanInfo.year5Index].Payment);
            this.SetCurrentNum("RE88395.X296", this.paySchedule[this.loanInfo.year5Index - 1].Balance);
          }
          else
          {
            this.SetCurrentNum("RE88395.X290", this.paySchedule[this.loanInfo.year5Index].Payment, showZero);
            monthlyPayment = RegzCalculation.CalcRawMonthlyPayment(this.loanInfo.LoanTerm - 60, this.paySchedule[this.loanInfo.year5Index - 1].Balance, this.paySchedule[this.loanInfo.year5Index - 1].CurrentRate + 2.0, this.IntVal("4"), 61, this.loanInfo.LoanAmount, this.loanInfo.ZeroPercentPaymentOption, this.loanInfo.UseSimpleInterest, !(this.loanInfo.LoanPurpose == "ConstructionToPermanent") || this.loanInfo.LoanTermConstPlusPerm == this.loanInfo.LoanTerm ? this.loanInfo.FirstPaymentDate : this.loanInfo.ConstFirstAmortDate, this.loanInfo.ConstInterestType, this.loanInfo.SimpleInterestUse366ForLeapYear);
            this.SetCurrentNum("RE88395.X291", monthlyPayment, this.UseNoPayment(monthlyPayment));
            monthlyPayment = RegzCalculation.CalcRawMonthlyPayment(this.loanInfo.LoanTerm - 60, this.paySchedule[this.loanInfo.year5Index - 1].Balance, this.paySchedule[this.loanInfo.year5Index - 1].CurrentRate + 5.0, this.IntVal("4"), 61, this.loanInfo.LoanAmount, this.loanInfo.ZeroPercentPaymentOption, this.loanInfo.UseSimpleInterest, !(this.loanInfo.LoanPurpose == "ConstructionToPermanent") || this.loanInfo.LoanTermConstPlusPerm == this.loanInfo.LoanTerm ? this.loanInfo.FirstPaymentDate : this.loanInfo.ConstFirstAmortDate, this.loanInfo.ConstInterestType, this.loanInfo.SimpleInterestUse366ForLeapYear);
            this.SetCurrentNum("RE88395.X294", monthlyPayment);
            this.SetCurrentNum("RE88395.X296", this.paySchedule[59].Balance);
          }
        }
        if (!this.skipPaymentScheduleCalculation)
          this.calculateBuydownSummary(id, val);
        this.calculateIRS1098(id, val);
        if (!this.useWorstCaseScenario && !this.useBestCaseScenario && !this.skipPaymentScheduleCalculation)
          this.calculateREGZSummary(id, val);
        if (!(this.loanInfo.LoanType != "HELOC") || !(this.loanInfo.LoanPurpose == "ConstructionToPermanent") || this.FltVal("1014") != 0.0 || !(this.Val("1811") == "PrimaryResidence") || this.paySchedule.Length <= this.loanInfo.ConstPeriod)
          return;
        PaymentSchedule paymentSchedule = this.paySchedule[this.loanInfo.ConstPeriod];
        if (paymentSchedule == null)
          return;
        double num16 = Utils.ArithmeticRounding(paymentSchedule.Principal + paymentSchedule.Interest, 2);
        switch (this.Val("420"))
        {
          case "FirstLien":
            this.SetCurrentNum("1724", num16, this.UseNoPayment(num16));
            break;
          case "SecondLien":
            double num17 = num16 + this.calObjs.D1003URLA2020Cal.GetVOALLienAmount(false, true) + this.calObjs.D1003Cal.GetVOLLienAmount(false);
            this.SetCurrentNum("1725", num17, this.UseNoPayment(num17));
            break;
        }
      }
    }

    private void calculateREGZSummary(string id, string val)
    {
      RegzSummaryTableType regzSummaryType = this.RegzSummaryType;
      Utils.ParseDate((object) this.Val("682"));
      double num1 = (double) this.IntVal("696");
      bool flag = this.Val("423") == "Biweekly";
      this.SetVal("3291", flag ? "Biweekly" : "Monthly");
      PaymentScheduleSnapshot scheduleSnapshot = (PaymentScheduleSnapshot) null;
      if (this.skipPaymentScheduleCalculation)
        return;
      if (regzSummaryType == RegzSummaryTableType.ARMLess5Years || regzSummaryType == RegzSummaryTableType.ARMGreater5Years || regzSummaryType == RegzSummaryTableType.ARMIntOnly || regzSummaryType == RegzSummaryTableType.ARMIntOnly31 || regzSummaryType == RegzSummaryTableType.ARMIntOnly51 || regzSummaryType == RegzSummaryTableType.ARMIO_L60 || regzSummaryType == RegzSummaryTableType.ARMIntOnly7_1or10_1 || regzSummaryType == RegzSummaryTableType.ARMIntOnly3C)
        scheduleSnapshot = this.useWorstCaseScenario || this.useBestCaseScenario ? this.loan.Calculator.GetPaymentSchedule(false) : this.calObjs.NewHudCal.CalculateWorstCaseScenario(!this.skipPaymentScheduleCalculation).Calculator.GetPaymentSchedule(false);
      string str1 = this.Val("1172");
      if (str1 != "Conventional")
        this.SetVal("3265", "");
      if (str1 == "FarmersHomeAdministration")
      {
        if (this.FltVal("NEWHUD.X1707") == 0.0)
          this.SetVal("3266", "");
      }
      else if (str1 != "FHA")
        this.SetVal("3266", "");
      double num2 = 0.0;
      bool useNew2015Gfehud = this.UseNew2015GFEHUD;
      for (int index = 41; index <= 49; ++index)
      {
        if (this.Val("HUD01" + (object) index) != "" && this.Val("HUD01" + (object) index) != "//")
        {
          switch (index)
          {
            case 41:
              num2 += useNew2015Gfehud && this.Val("NEWHUD2.X134") == "Y" || !useNew2015Gfehud ? (flag ? this.FltVal("HUD52") : this.FltVal("231")) : 0.0;
              continue;
            case 42:
              num2 += useNew2015Gfehud && this.Val("NEWHUD2.X133") == "Y" || !useNew2015Gfehud ? (flag ? this.FltVal("HUD53") : this.FltVal("230")) : 0.0;
              continue;
            case 44:
              num2 += useNew2015Gfehud && this.Val("NEWHUD2.X136") == "Y" || !useNew2015Gfehud ? (flag ? this.FltVal("HUD55") : this.FltVal("235")) : 0.0;
              continue;
            case 45:
              num2 += useNew2015Gfehud && this.Val("NEWHUD2.X135") == "Y" || !useNew2015Gfehud ? (flag ? this.FltVal("HUD56") : this.FltVal("L268")) : 0.0;
              continue;
            case 46:
              num2 += useNew2015Gfehud && this.Val("NEWHUD2.X137") == "Y" || !useNew2015Gfehud ? (flag ? this.FltVal("HUD58") : this.FltVal("1630")) : 0.0;
              continue;
            case 47:
              num2 += useNew2015Gfehud && this.Val("NEWHUD2.X138") == "Y" || !useNew2015Gfehud ? (flag ? this.FltVal("HUD60") : this.FltVal("253")) : 0.0;
              continue;
            case 48:
              num2 += useNew2015Gfehud && this.Val("NEWHUD2.X139") == "Y" || !useNew2015Gfehud ? (flag ? this.FltVal("HUD62") : this.FltVal("254")) : 0.0;
              continue;
            case 49:
              num2 += useNew2015Gfehud && this.Val("NEWHUD2.X140") == "Y" || !useNew2015Gfehud ? (flag ? this.FltVal("HUD63") : this.FltVal("NEWHUD.X1707")) : 0.0;
              continue;
            default:
              continue;
          }
        }
      }
      if (num2 == 0.0 && (this.Val("HUD0143") == "" || this.Val("HUD0143") == "//"))
      {
        if (this.IntVal("1386") > 0)
          num2 += useNew2015Gfehud && this.Val("NEWHUD2.X134") == "Y" || !useNew2015Gfehud ? (flag ? this.FltVal("HUD52") : this.FltVal("231")) : 0.0;
        if (this.IntVal("1387") > 0)
          num2 += useNew2015Gfehud && this.Val("NEWHUD2.X133") == "Y" || !useNew2015Gfehud ? (flag ? this.FltVal("HUD53") : this.FltVal("230")) : 0.0;
        if (this.IntVal("1388") > 0)
          num2 += useNew2015Gfehud && this.Val("NEWHUD2.X136") == "Y" || !useNew2015Gfehud ? (flag ? this.FltVal("HUD55") : this.FltVal("235")) : 0.0;
        if (this.IntVal("L267") > 0)
          num2 += useNew2015Gfehud && this.Val("NEWHUD2.X135") == "Y" || !useNew2015Gfehud ? (flag ? this.FltVal("HUD56") : this.FltVal("L268")) : 0.0;
        if (this.IntVal("1629") > 0)
          num2 += useNew2015Gfehud && this.Val("NEWHUD2.X137") == "Y" || !useNew2015Gfehud ? (flag ? this.FltVal("HUD58") : this.FltVal("1630")) : 0.0;
        if (this.IntVal("340") > 0)
          num2 += useNew2015Gfehud && this.Val("NEWHUD2.X138") == "Y" || !useNew2015Gfehud ? (flag ? this.FltVal("HUD60") : this.FltVal("253")) : 0.0;
        if (this.IntVal("341") > 0)
          num2 += useNew2015Gfehud && this.Val("NEWHUD2.X139") == "Y" || !useNew2015Gfehud ? (flag ? this.FltVal("HUD62") : this.FltVal("254")) : 0.0;
        num2 += useNew2015Gfehud && this.Val("NEWHUD2.X140") == "Y" || !useNew2015Gfehud ? (flag ? this.FltVal("HUD63") : this.FltVal("NEWHUD.X1707")) : 0.0;
      }
      if (num2 > 0.0)
        this.SetVal("3264", "Y");
      else
        this.SetVal("3264", "");
      double num3 = 0.0;
      if (this.paySchedule != null && this.paySchedule.Length != 0 && this.paySchedule[0] != null && this.paySchedule[0].MortgageInsurance > 0.0)
      {
        switch (str1)
        {
          case "Conventional":
            if (useNew2015Gfehud && this.Val("NEWHUD2.X4769") == "Y" || !useNew2015Gfehud)
            {
              this.SetVal("3265", "Y");
              break;
            }
            this.SetVal("3265", "");
            break;
          case "FHA":
            this.SetVal("3266", "Y");
            break;
        }
        num3 = str1 == "FarmersHomeAdministration" ? 0.0 : this.FltVal("232");
      }
      else
      {
        this.SetVal("3265", "");
        if (str1 == "FarmersHomeAdministration" && this.FltVal("NEWHUD.X1707") == 0.0 || str1 != "FarmersHomeAdministration")
          this.SetVal("3266", "");
      }
      this.SetVal("3267", "");
      this.SetVal("3269", "");
      this.SetVal("3270", "");
      this.SetVal("3271", "");
      this.SetVal("3272", "");
      this.SetVal("3273", "");
      this.SetVal("3274", "");
      this.SetVal("3275", "");
      this.SetVal("3278", "");
      this.SetVal("3285", "");
      this.SetVal("3289", "");
      this.SetVal("3294", "");
      string str2 = this.Val("608");
      if (regzSummaryType != RegzSummaryTableType.ConstOnlyA && regzSummaryType != RegzSummaryTableType.ConstOnlyB)
      {
        this.SetVal("3287", "");
        this.SetVal("3288", "");
      }
      double num4 = this.paySchedule[0] != null ? this.paySchedule[0].Principal + this.paySchedule[0].Interest : 0.0;
      this.SetCurrentNum("3290", num4, this.UseNoPayment(num4));
      double num5 = 0.0;
      double num6 = 0.0;
      int num7 = this.loanInfo.year5Index + 1;
      switch (regzSummaryType - 1)
      {
        case RegzSummaryTableType.None:
          this.SetCurrentNum("3267", str1 == "FarmersHomeAdministration" ? num2 : num2 + this.paySchedule[0].MortgageInsurance);
          break;
        case RegzSummaryTableType.Fixed:
          if (scheduleSnapshot != null)
          {
            string payDate1 = scheduleSnapshot.MonthlyPayments[0] != null ? scheduleSnapshot.MonthlyPayments[0].PayDate : "";
            string payDate2 = scheduleSnapshot.MonthlyPayments[0] != null ? scheduleSnapshot.MonthlyPayments[0].PayDate : "";
            PaymentSchedule paymentSchedule1 = (PaymentSchedule) null;
            PaymentSchedule paymentSchedule2 = (PaymentSchedule) null;
            int index = !(this.Val("19") == "ConstructionToPermanent") || this.IntVal("1176") > 12 ? 0 : this.IntVal("1176");
            int num8 = num7;
            if (index > 0)
              num8 += index;
            for (; index < scheduleSnapshot.ActualNumberOfTerm; ++index)
            {
              if (paymentSchedule1 == null)
                paymentSchedule1 = scheduleSnapshot.MonthlyPayments[index];
              if (paymentSchedule2 == null)
                paymentSchedule2 = scheduleSnapshot.MonthlyPayments[index];
              if (index < num8 && (scheduleSnapshot.MonthlyPayments[index].CurrentRate > paymentSchedule1.CurrentRate || scheduleSnapshot.MonthlyPayments[index].Interest + scheduleSnapshot.MonthlyPayments[index].Principal > paymentSchedule1.Interest + paymentSchedule1.Principal))
              {
                paymentSchedule1 = scheduleSnapshot.MonthlyPayments[index];
                if (index > 0)
                  payDate1 = scheduleSnapshot.MonthlyPayments[index - 1].PayDate;
              }
              if (scheduleSnapshot.MonthlyPayments[index].CurrentRate > paymentSchedule2.CurrentRate && index + 1 != scheduleSnapshot.ActualNumberOfTerm)
              {
                paymentSchedule2 = scheduleSnapshot.MonthlyPayments[index];
                if (index > 0)
                  payDate2 = scheduleSnapshot.MonthlyPayments[index - 1].PayDate;
              }
            }
            this.SetCurrentNum("3267", num2 + (scheduleSnapshot.MonthlyPayments[0] != null ? scheduleSnapshot.MonthlyPayments[0].MortgageInsurance : 0.0));
            if (this.loanInfo.ARMfirstChange > num7 - 1)
            {
              string val1 = this.Val("763");
              string val2 = this.Val("1887");
              if (val2 != string.Empty && val2 != "//")
                this.SetVal("3274", val2);
              else if (val1 != string.Empty && val1 != "//")
                this.SetVal("3274", val1);
              else
                this.SetVal("3274", paymentSchedule1 != null ? payDate1 : "");
            }
            else
              this.SetVal("3274", paymentSchedule1 != null ? payDate1 : "");
            this.SetCurrentNum("3275", paymentSchedule1 != null ? paymentSchedule1.CurrentRate : 0.0);
            this.SetCurrentNum("3285", paymentSchedule1 != null ? paymentSchedule1.Interest + paymentSchedule1.Principal : 0.0);
            this.SetCurrentNum("3278", paymentSchedule1 != null ? paymentSchedule1.MortgageInsurance + num2 : 0.0);
            this.SetVal("3280", paymentSchedule2 != null ? payDate2 : "");
            this.SetCurrentNum("3286", paymentSchedule2 != null ? paymentSchedule2.Interest + paymentSchedule2.Principal : 0.0);
            this.SetCurrentNum("3283", paymentSchedule2 != null ? paymentSchedule2.MortgageInsurance + num2 : 0.0);
          }
          this.SetCurrentNum("3279", this.FltVal("3285") + this.FltVal("3278"));
          this.SetCurrentNum("3284", this.FltVal("3286") + this.FltVal("3283"));
          this.SetCurrentNum("3289", num1 > 0.0 ? num1 + 1.0 : 0.0);
          break;
        case RegzSummaryTableType.ARMLess5Years:
          if (scheduleSnapshot != null)
          {
            string payDate = scheduleSnapshot.MonthlyPayments[0].PayDate;
            PaymentSchedule paymentSchedule = (PaymentSchedule) null;
            for (int index = 0; index < scheduleSnapshot.ActualNumberOfTerm; ++index)
            {
              if (paymentSchedule == null)
                paymentSchedule = scheduleSnapshot.MonthlyPayments[index];
              if (scheduleSnapshot.MonthlyPayments[index].CurrentRate > paymentSchedule.CurrentRate && index + 1 != scheduleSnapshot.ActualNumberOfTerm)
              {
                paymentSchedule = scheduleSnapshot.MonthlyPayments[index];
                if (index > 0)
                  payDate = scheduleSnapshot.MonthlyPayments[index - 1].PayDate;
              }
            }
            this.SetCurrentNum("3267", num2 + scheduleSnapshot.MonthlyPayments[0].MortgageInsurance);
            this.SetVal("3280", paymentSchedule != null ? payDate : "");
            this.SetCurrentNum("3286", paymentSchedule != null ? paymentSchedule.Interest + paymentSchedule.Principal : 0.0);
            this.SetCurrentNum("3283", paymentSchedule != null ? paymentSchedule.MortgageInsurance + num2 : 0.0);
          }
          this.SetCurrentNum("3284", this.FltVal("3286") + this.FltVal("3283"));
          this.SetCurrentNum("3289", num1 > 0.0 ? num1 + 1.0 : 0.0);
          break;
        case RegzSummaryTableType.ARMGreater5Years:
          this.SetCurrentNum("3267", num2 + this.paySchedule[0].MortgageInsurance);
          if (this.loanInfo.InterestOnly > 0 && this.loanInfo.InterestOnly >= this.loanInfo.BallonTerm)
          {
            this.SetVal("3280", "");
            this.SetVal("3281", "");
            this.SetVal("3282", "");
            this.SetVal("3283", "");
            this.SetVal("3284", "");
            this.SetVal("3287", this.paySchedule[this.loanInfo.InterestOnly - 1] != null ? this.paySchedule[this.loanInfo.InterestOnly - 1].PayDate : "");
            this.SetCurrentNum("3288", this.paySchedule[this.loanInfo.InterestOnly - 1] != null ? this.paySchedule[this.loanInfo.InterestOnly - 1].Payment : 0.0);
            break;
          }
          if (this.loanInfo.InterestOnly > 0 && this.loanInfo.InterestOnly < this.loanInfo.LoanPeriod)
          {
            this.SetVal("3280", this.paySchedule[this.loanInfo.InterestOnly] != null ? this.paySchedule[this.loanInfo.InterestOnly].PayDate : "");
            this.SetCurrentNum("3281", this.paySchedule[this.loanInfo.InterestOnly] != null ? this.paySchedule[this.loanInfo.InterestOnly].Principal : 0.0);
            this.SetCurrentNum("3282", this.paySchedule[this.loanInfo.InterestOnly] != null ? this.paySchedule[this.loanInfo.InterestOnly].Interest : 0.0);
            this.SetCurrentNum("3283", (this.paySchedule[this.loanInfo.InterestOnly] != null ? this.paySchedule[this.loanInfo.InterestOnly].MortgageInsurance : 0.0) + num2);
            this.SetCurrentNum("3284", this.FltVal("3281") + this.FltVal("3282") + this.FltVal("3283"));
            break;
          }
          break;
        case RegzSummaryTableType.FixedIntOnly:
        case RegzSummaryTableType.ARMIntOnly:
        case RegzSummaryTableType.ARMIntOnly31:
        case RegzSummaryTableType.ARMIntOnly51:
        case RegzSummaryTableType.ARMIntOnly3C:
        case RegzSummaryTableType.ARMIntOnly7_1or10_1:
          if (scheduleSnapshot != null && scheduleSnapshot.ActualNumberOfTerm > 0)
          {
            this.SetCurrentNum("3267", scheduleSnapshot.MonthlyPayments[0].MortgageInsurance + num2);
            string payDate3 = scheduleSnapshot.MonthlyPayments[0].PayDate;
            string payDate4 = scheduleSnapshot.MonthlyPayments[0].PayDate;
            string payDate5 = scheduleSnapshot.MonthlyPayments[0].PayDate;
            PaymentSchedule paymentSchedule3 = (PaymentSchedule) null;
            PaymentSchedule paymentSchedule4 = (PaymentSchedule) null;
            PaymentSchedule paymentSchedule5 = (PaymentSchedule) null;
            for (int index = 0; index < scheduleSnapshot.ActualNumberOfTerm; ++index)
            {
              if (paymentSchedule5 == null && scheduleSnapshot.MonthlyPayments[index].Principal > 0.0 && scheduleSnapshot.MonthlyPayments[index].Interest > 0.0)
              {
                paymentSchedule5 = scheduleSnapshot.MonthlyPayments[index];
                payDate5 = scheduleSnapshot.MonthlyPayments[index].PayDate;
              }
              if (paymentSchedule3 == null)
                paymentSchedule3 = scheduleSnapshot.MonthlyPayments[index];
              if (paymentSchedule4 == null)
                paymentSchedule4 = scheduleSnapshot.MonthlyPayments[index];
              if (index < num7 && scheduleSnapshot.MonthlyPayments[index].CurrentRate > paymentSchedule3.CurrentRate)
              {
                paymentSchedule3 = scheduleSnapshot.MonthlyPayments[index];
                if (index > 0)
                  payDate3 = scheduleSnapshot.MonthlyPayments[index - 1].PayDate;
              }
              if (scheduleSnapshot.MonthlyPayments[index].CurrentRate > paymentSchedule4.CurrentRate && index + 1 != scheduleSnapshot.ActualNumberOfTerm)
              {
                paymentSchedule4 = scheduleSnapshot.MonthlyPayments[index];
                if (index > 0)
                  payDate4 = scheduleSnapshot.MonthlyPayments[index - 1].PayDate;
              }
            }
            if (paymentSchedule5 != null)
            {
              this.SetVal("3269", payDate5);
              this.SetCurrentNum("3294", paymentSchedule5.CurrentRate);
              this.SetCurrentNum("3270", paymentSchedule5.Principal);
              this.SetCurrentNum("3271", paymentSchedule5.Interest);
              this.SetCurrentNum("3272", paymentSchedule5.MortgageInsurance + num2);
            }
            else
            {
              this.SetVal("3269", "");
              this.SetVal("3294", "");
              this.SetVal("3270", "");
              this.SetVal("3271", "");
              this.SetVal("3272", "");
            }
            if (this.loanInfo.ARMfirstChange > num7 - 1)
            {
              string val3 = this.Val("2553");
              if (val3 == string.Empty || val3 == "//")
              {
                val3 = this.Val("748");
                if (val3 == string.Empty || val3 == "//")
                  val3 = this.Val("763");
              }
              this.SetVal("3274", val3);
            }
            else if (paymentSchedule3.Interest == 0.0)
              this.SetVal("3274", paymentSchedule3 != null ? paymentSchedule3.PayDate : "");
            else
              this.SetVal("3274", paymentSchedule3 != null ? payDate3 : "");
            this.SetCurrentNum("3275", paymentSchedule3 != null ? paymentSchedule3.CurrentRate : 0.0);
            this.SetCurrentNum("3276", paymentSchedule3 != null ? paymentSchedule3.Principal : 0.0);
            this.SetCurrentNum("3277", paymentSchedule3 != null ? paymentSchedule3.Interest : 0.0);
            this.SetCurrentNum("3278", paymentSchedule3 != null ? paymentSchedule3.MortgageInsurance + num2 : 0.0);
            if (paymentSchedule4.Interest == 0.0)
              this.SetVal("3280", paymentSchedule4 != null ? paymentSchedule4.PayDate : "");
            else
              this.SetVal("3280", paymentSchedule4 != null ? payDate4 : "");
            this.SetCurrentNum("3281", paymentSchedule4 != null ? paymentSchedule4.Principal : 0.0);
            this.SetCurrentNum("3282", paymentSchedule4 != null ? paymentSchedule4.Interest : 0.0);
            this.SetCurrentNum("3283", paymentSchedule4 != null ? paymentSchedule4.MortgageInsurance + num2 : 0.0);
            if (this.loanInfo.InterestOnly >= this.loanInfo.BallonTerm)
            {
              if (scheduleSnapshot.MonthlyPayments[scheduleSnapshot.ActualNumberOfTerm] == null)
              {
                this.SetVal("3287", scheduleSnapshot.MonthlyPayments[scheduleSnapshot.ActualNumberOfTerm - 1].PayDate);
                this.SetCurrentNum("3288", scheduleSnapshot.MonthlyPayments[scheduleSnapshot.ActualNumberOfTerm - 1].Payment);
              }
              else if (scheduleSnapshot.MonthlyPayments[scheduleSnapshot.ActualNumberOfTerm].PayDate == scheduleSnapshot.MonthlyPayments[scheduleSnapshot.ActualNumberOfTerm - 1].PayDate)
              {
                this.SetVal("3287", scheduleSnapshot.MonthlyPayments[scheduleSnapshot.ActualNumberOfTerm].PayDate);
                this.SetCurrentNum("3288", Utils.ArithmeticRounding(scheduleSnapshot.MonthlyPayments[scheduleSnapshot.ActualNumberOfTerm].Payment + scheduleSnapshot.MonthlyPayments[scheduleSnapshot.ActualNumberOfTerm - 1].Payment, 2));
              }
            }
          }
          this.SetCurrentNum("3289", num1 > 0.0 ? num1 + 1.0 : 0.0);
          this.SetCurrentNum("3268", this.FltVal("3290") + this.FltVal("3267"));
          this.SetCurrentNum("3273", this.FltVal("3270") + this.FltVal("3271") + this.FltVal("3272"));
          this.SetCurrentNum("3279", this.FltVal("3276") + this.FltVal("3277") + this.FltVal("3278"));
          this.SetCurrentNum("3284", this.FltVal("3281") + this.FltVal("3282") + this.FltVal("3283"));
          break;
        case RegzSummaryTableType.ARMIO_L60:
          this.SetCurrentNum("3267", num2 + (this.paySchedule[0] == null || this.loanInfo.LoanPeriod <= 0 ? 0.0 : this.paySchedule[0].MortgageInsurance));
          if (this.loanInfo.BallonTerm > 0 && this.loanInfo.BallonTerm < this.loanInfo.LoanTerm && this.loanInfo.LoanPeriod > 0 && this.paySchedule[this.loanInfo.LoanPeriod - 1] != null)
          {
            this.SetVal("3287", this.paySchedule[this.loanInfo.LoanPeriod - 1].PayDate);
            this.SetCurrentNum("3288", this.paySchedule[this.loanInfo.LoanPeriod - 1].Payment);
            break;
          }
          this.SetVal("3287", "");
          this.SetVal("3288", "");
          break;
        case RegzSummaryTableType.FixedBalloon:
        case RegzSummaryTableType.FixedBalloonIntOnlyGreater:
          this.SetCurrentNum("3267", num2 + (this.paySchedule[0] == null || this.paySchedule.Length == 0 ? 0.0 : this.paySchedule[0].MortgageInsurance));
          if (regzSummaryType == RegzSummaryTableType.FixedBalloonIntOnlyLesser && this.loanInfo.InterestOnly != this.loanInfo.LoanPeriod && this.paySchedule[this.loanInfo.InterestOnly] != null)
          {
            this.SetVal("3280", this.paySchedule[this.loanInfo.InterestOnly].PayDate);
            this.SetCurrentNum("3295", this.paySchedule[this.loanInfo.InterestOnly].CurrentRate);
            this.SetCurrentNum("3281", this.paySchedule[this.loanInfo.InterestOnly].Principal);
            this.SetCurrentNum("3282", this.paySchedule[this.loanInfo.InterestOnly].Interest);
            this.SetCurrentNum("3283", num2 + this.paySchedule[this.loanInfo.InterestOnly].MortgageInsurance);
          }
          else
          {
            this.SetVal("3280", "");
            this.SetVal("3295", "");
            this.SetVal("3281", "");
            this.SetVal("3282", "");
            this.SetVal("3283", "");
          }
          if (this.loanInfo.InterestOnly >= this.loanInfo.BallonTerm && this.loanInfo.BallonTerm > 0 || this.loanInfo.BallonTerm > 0 && this.loanInfo.BallonTerm <= this.loanInfo.LoanPeriod)
          {
            this.SetVal("3287", this.paySchedule[this.loanInfo.BallonTerm - 1] != null ? this.paySchedule[this.loanInfo.BallonTerm - 1].PayDate : "");
            this.SetCurrentNum("3288", this.paySchedule[this.loanInfo.BallonTerm - 1] != null ? this.paySchedule[this.loanInfo.BallonTerm - 1].Payment : 0.0);
          }
          else
          {
            this.SetVal("3287", "");
            this.SetVal("3288", "");
          }
          this.SetCurrentNum("3284", this.FltVal("3281") + this.FltVal("3282") + this.FltVal("3283"));
          break;
        case RegzSummaryTableType.FixedBalloonIntOnlyLesser:
        case RegzSummaryTableType.ConstOnlyA:
          this.SetCurrentNum("3267", num2 + num3);
          if (str2 != "AdjustableRate" && this.loanInfo.LoanPurpose != "ConstructionOnly")
            this.SetVal("3287", this.Val("1961"));
          if (regzSummaryType == RegzSummaryTableType.ConstOnlyB)
          {
            if (str2 != "AdjustableRate")
            {
              this.SetCurrentNum("3288", this.paySchedule[this.loanInfo.LoanPeriod - 1] != null ? this.paySchedule[this.loanInfo.LoanPeriod - 1].Payment : 0.0);
              break;
            }
            break;
          }
          string val4 = this.Val("5");
          this.SetCurrentNum("3290", this.Flt(val4), !string.IsNullOrEmpty(val4) && this.UseNoPayment(this.Flt(val4)));
          this.SetVal("3280", "");
          this.SetVal("3281", "");
          this.SetVal("3282", "");
          this.SetVal("3283", "");
          this.SetVal("3284", "");
          this.SetVal("3295", "");
          if (str2 != "AdjustableRate")
          {
            if (this.paySchedule[0] != null)
            {
              this.SetCurrentNum("3288", Utils.ArithmeticRounding(this.FltVal("1109") + this.FltVal("5") + this.paySchedule[0].MortgageInsurance, 2));
              break;
            }
            this.SetCurrentNum("3288", Utils.ArithmeticRounding(this.FltVal("1109") + this.FltVal("5") + num3, 2));
            break;
          }
          break;
        case RegzSummaryTableType.ConstOnlyB:
          if (this.paySchedule != null)
          {
            string payDate6 = this.paySchedule[0].PayDate;
            string payDate7 = this.paySchedule[0].PayDate;
            PaymentSchedule paymentSchedule6 = (PaymentSchedule) null;
            PaymentSchedule paymentSchedule7 = (PaymentSchedule) null;
            for (int index = 0; index < this.paySchedule.Length; ++index)
            {
              if (this.paySchedule[index] != null)
              {
                if (paymentSchedule6 == null)
                {
                  paymentSchedule6 = this.paySchedule[index];
                  num5 = Utils.ArithmeticRounding(this.paySchedule[index].Principal + this.paySchedule[index].Interest, 2);
                }
                if (paymentSchedule7 == null)
                {
                  paymentSchedule7 = this.paySchedule[index];
                  num6 = Utils.ArithmeticRounding(this.paySchedule[index].Principal + this.paySchedule[index].Interest, 2);
                }
                double num9 = Utils.ArithmeticRounding(this.paySchedule[index].Principal + this.paySchedule[index].Interest, 2);
                if (index < num7 && num9 > num5)
                {
                  paymentSchedule6 = this.paySchedule[index];
                  num5 = Utils.ArithmeticRounding(this.paySchedule[index].Principal + this.paySchedule[index].Interest, 2);
                  if (index > 0)
                    payDate6 = this.paySchedule[index - 1].PayDate;
                }
                if (num9 > num6 && index + 1 != this.loanInfo.LoanPeriod)
                {
                  paymentSchedule7 = this.paySchedule[index];
                  num6 = Utils.ArithmeticRounding(this.paySchedule[index].Principal + this.paySchedule[index].Interest, 2);
                  if (index > 0)
                    payDate7 = this.paySchedule[index - 1].PayDate;
                }
              }
            }
            this.SetCurrentNum("3267", num2 + this.paySchedule[0].MortgageInsurance);
            this.SetVal("3274", paymentSchedule6 != null ? payDate6 : "");
            this.SetCurrentNum("3275", paymentSchedule6 != null ? paymentSchedule6.CurrentRate : 0.0);
            this.SetCurrentNum("3285", paymentSchedule6 != null ? paymentSchedule6.Interest + paymentSchedule6.Principal : 0.0);
            this.SetCurrentNum("3278", paymentSchedule6 != null ? paymentSchedule6.MortgageInsurance + num2 : 0.0);
            this.SetVal("3280", paymentSchedule7 != null ? payDate7 : "");
            this.SetCurrentNum("3286", paymentSchedule7 != null ? paymentSchedule7.Interest + paymentSchedule7.Principal : 0.0);
            this.SetCurrentNum("3283", paymentSchedule7 != null ? paymentSchedule7.MortgageInsurance + num2 : 0.0);
          }
          this.SetCurrentNum("3279", this.FltVal("3285") + this.FltVal("3278"));
          this.SetCurrentNum("3284", this.FltVal("3286") + this.FltVal("3283"));
          break;
      }
      switch (regzSummaryType - 2)
      {
        case RegzSummaryTableType.None:
        case RegzSummaryTableType.Fixed:
        case RegzSummaryTableType.ARMGreater5Years:
        case RegzSummaryTableType.FixedIntOnly:
        case RegzSummaryTableType.ARMIntOnly:
        case RegzSummaryTableType.ARMIntOnly31:
        case RegzSummaryTableType.ARMIntOnly51:
        case RegzSummaryTableType.ARMIntOnly3C:
          if (this.loanInfo.BallonTerm > 0 && this.loanInfo.BallonTerm < this.IntVal("4") && scheduleSnapshot != null && this.loanInfo.BallonTerm <= scheduleSnapshot.MonthlyPayments.Length)
          {
            this.SetVal("3287", scheduleSnapshot.MonthlyPayments[this.loanInfo.BallonTerm - 1].PayDate);
            this.SetCurrentNum("3288", scheduleSnapshot.MonthlyPayments[this.loanInfo.BallonTerm - 1].Payment);
            break;
          }
          break;
        case RegzSummaryTableType.ConstOnlyA:
          if (this.paySchedule != null && this.loanInfo.LoanPeriod > 0 && this.loanInfo.BallonTerm < this.loanInfo.LoanTerm)
          {
            this.SetVal("3287", this.paySchedule[this.loanInfo.LoanPeriod - 1] != null ? this.paySchedule[this.loanInfo.LoanPeriod - 1].PayDate : "");
            this.SetCurrentNum("3288", this.paySchedule[this.loanInfo.LoanPeriod - 1] != null ? this.paySchedule[this.loanInfo.LoanPeriod - 1].Payment : 0.0);
            break;
          }
          break;
      }
      if (this.Val("4746") == "NoPaymentwithBalloon" && this.Val("3") == "0.000" && this.FltVal("3288") == 0.0 && this.paySchedule != null && this.paySchedule.Length > this.loanInfo.BallonTerm - 1)
      {
        this.SetVal("3287", this.paySchedule[this.loanInfo.BallonTerm - 1] != null ? this.paySchedule[this.loanInfo.BallonTerm - 1].PayDate : "");
        this.SetCurrentNum("3288", this.paySchedule[this.loanInfo.BallonTerm - 1] != null ? this.paySchedule[this.loanInfo.BallonTerm - 1].Payment : 0.0);
      }
      this.SetCurrentNum("3268", this.FltVal("3290") + this.FltVal("3267"));
      this.calObjs.ATRQMCal.CalcMax5YearsPandI(id, val);
      this.calObjs.D1003URLA2020Cal.UpdateLinkedVoal("QM.X337", (string) null);
      this.calObjs.VERIFCal.CalcHelocLineTotal(id, val);
    }

    private void calculateMaturityDate(string id, string val)
    {
      string str = this.Val("608");
      if (!(this.Val("1172") == "HELOC") || !(str == "AdjustableRate") && !(str == "Fixed") && !(str == ""))
        return;
      DateTime date = Utils.ParseDate((object) this.Val("682"));
      int num1 = this.IntVal("1889");
      int num2 = this.IntVal("1890");
      this.SetVal("78", date == DateTime.MinValue || num1 == 0 ? "" : date.AddMonths(num1 + num2 - 1).ToShortDateString());
    }

    private void calculateBuydownSummary(string id, string val)
    {
      if (this.loan.IsClonedLoan)
        return;
      double num1 = 0.0;
      double num2 = 0.0;
      bool flag1 = false;
      int num3 = 0;
      bool flag2 = this.Val("CASASRN.X141") == "Borrower";
      bool flag3 = this.Val("COMPLIANCEVERSION.CASASRNX141") == "Y" || this.Val("COMPLIANCEVERSION.NEWBUYDOWNENABLED") != "Y";
      PaymentSchedule[] paymentScheduleArray = (PaymentSchedule[]) null;
      if (!flag2 && !flag3)
      {
        if (this.Val("4535") != "" && this.Val("4541") != "")
        {
          LoanData loanData = new LoanData(this.loan.XmlDocClone, this.loan.Settings, false, true);
          loanData.IsClonedLoan = true;
          LoanCalculator loanCalculator = new LoanCalculator(this.sessionObjects, this.calObjs.LoanConfiguration, loanData, true, this.calObjs.ExternalLateFeeSettings, true);
          loanData.Calculator.UseWorstCaseScenario = false;
          loanData.Calculator.UseBestCaseScenario = false;
          loanData.Calculator.SkipLockRequestSync = true;
          loanData.SkipCountyLimitCalculation = true;
          for (int index = 4535; index <= 4540; ++index)
            loanData.SetCurrentField(string.Concat((object) (index - 3266)), loanData.GetSimpleField(string.Concat((object) index)));
          for (int index = 4541; index <= 4546; ++index)
            loanData.SetCurrentField(string.Concat((object) (index - 2928)), loanData.GetSimpleField(string.Concat((object) index)));
          loanData.SetCurrentField("CASASRN.X141", "Borrower");
          PaymentScheduleSnapshot paymentSchedule = loanData.Calculator.GetPaymentSchedule(false);
          paymentScheduleArray = paymentSchedule == null || paymentSchedule.MonthlyPayments == null || paymentSchedule.MonthlyPayments.Length == 0 ? (PaymentSchedule[]) null : paymentSchedule.MonthlyPayments;
          loanData.Close();
        }
      }
      else
        paymentScheduleArray = this.paySchedule;
      for (int index = 1613; index <= 1618; ++index)
      {
        int num4 = this.IntVal(flag2 | flag3 ? index.ToString() : (index + 2928).ToString());
        int num5 = (flag2 | flag3 ? index - 1613 : index + 2928 - 4541) * 4 + 3096;
        num3 += num4;
        if (num4 == 0 || paymentScheduleArray == null || this.paySchedule == null || num3 > paymentScheduleArray.Length || num3 > this.paySchedule.Length || this.paySchedule[num3 - 1] == null || paymentScheduleArray[num3 - 1] == null)
        {
          this.SetVal(num5.ToString(), "");
          ++num5;
          this.SetVal(num5.ToString(), "");
          ++num5;
          this.SetVal(num5.ToString(), "");
          this.SetVal(string.Concat((object) (index + 1511)), "");
        }
        else
        {
          double num6;
          if (this.Val("2982") == "Y" && num3 <= this.IntVal("1177"))
          {
            double num7 = paymentScheduleArray[num3 - 1].Interest - paymentScheduleArray[num3 - 1].MortgageInsurance;
            num6 = Utils.ArithmeticRounding(this.paySchedule[num3 - 1].Interest - num7, 2);
          }
          else
            num6 = Utils.ArithmeticRounding(this.loanInfo.MonthlyPayment - (paymentScheduleArray[num3 - 1].Payment - paymentScheduleArray[num3 - 1].MortgageInsurance), 2);
          this.SetCurrentNum(num5.ToString(), num6);
          double num8 = (double) num4 * num6;
          ++num5;
          this.SetCurrentNum(num5.ToString(), num8);
          ++num5;
          this.SetCurrentNum(num5.ToString(), num8);
          num1 += num8;
          num2 += num8;
          this.SetVal(string.Concat((object) (index + 1511)), string.Concat((object) num4));
          flag1 = true;
        }
      }
      this.SetCurrentNum("3119", num1);
      this.SetCurrentNum("3120", num2);
      bool flag4 = this.Val("CASASRN.X141") == "Seller";
      if (flag3 || !flag4)
      {
        if (this.IsLocked("QM.X378"))
          this.RemoveLock("QM.X378");
        this.SetVal("QM.X378", "");
      }
      else
        this.SetCurrentNum("QM.X378", num1);
      if (flag1)
      {
        this.SetVal("1391", "Buydown Fund Amount");
        this.SetCurrentNum("81", num1 - this.FltVal("82"));
      }
      else
      {
        if (!(this.Val("1391") == "Buydown Fund Amount"))
          return;
        this.SetVal("1391", "");
        this.SetCurrentNum("81", 0.0);
        this.SetCurrentNum("82", 0.0);
      }
    }

    private void calculateIRS1098(string id, string val)
    {
      double num1 = this.FltVal("1766");
      DateTime date1 = Utils.ParseDate((object) this.Val("682"));
      DateTime date2 = Utils.ParseDate((object) this.Val("748"));
      if (date2 != DateTime.MinValue && date1 != DateTime.MinValue && date2.Year == date1.Year)
      {
        double val1;
        if (this.loanInfo.IsBiWeekly)
        {
          TimeSpan timeSpan = Utils.ParseDate((object) (date1.Year.ToString() + "/12/31")).Subtract(date1);
          int num2 = timeSpan.Days % 14;
          val1 = (double) ((timeSpan.Days - num2) / 14 + 1) * num1 + num1 / 14.0 * (double) num2;
        }
        else
          val1 = (double) (12 - date1.Month + 1) * num1 - (double) (date1.Day - 1) * num1 / 31.0;
        if (val1 != 0.0)
          this.SetCurrentNum("2845", Utils.ArithmeticRounding(val1, 2));
        else
          this.SetVal("2845", "");
      }
      else
        this.SetVal("2845", "");
      double num3 = this.FltVal("1045") + this.FltVal("2845");
      if (num3 >= 600.0)
        this.SetCurrentNum("2846", num3);
      else
        this.SetVal("2846", "");
      this.calObjs.ToolCal.CalcIRS1098((string) null, (string) null);
    }

    private int buildOptionPayment(
      ref double unpaidBalance,
      ref double monthlyPayment,
      ref double finCharge,
      ref DateTime duePayDate,
      ref int interestOnlyMonth,
      ref double currentRate)
    {
      int num1 = 0;
      if (this.Val("2307") == "ofThePayment")
      {
        this.loanInfo.SteadyOptionInitialPayment = this.EMRounding(monthlyPayment * this.loanInfo.DiscountRate, 2);
      }
      else
      {
        if (!(this.Val("2307") == "BelowNoteRate"))
          return 0;
        this.loanInfo.SteadyOptionInitialPayment = this.loanInfo.InterestOnly <= 0 ? this.EMRounding(this.CalcRawMonthlyPayment(this.loanInfo.LoanTerm, unpaidBalance, this.loanInfo.DiscountRate, false), 2) : this.EMRounding(this.loanInfo.DiscountRate / 1200.0 * unpaidBalance, 2);
      }
      bool flag = false;
      if (this.loanInfo.NegAdjCap != 0.0 && this.loanInfo.NegAdjPeriod != 0 && this.loanInfo.ARMfirstChange > 0 && this.loanInfo.ARMfirstAdjCap > 0.0)
        flag = true;
      int day = duePayDate.Day;
      while (unpaidBalance >= 0.0 && this.loanInfo.LoanPeriod < this.loanInfo.DiscountPeriod)
      {
        ++this.loanInfo.LoanPeriod;
        ++num1;
        if (flag)
        {
          if (this.loanInfo.LoanPeriod == this.loanInfo.ARMfirstChange + 1)
          {
            currentRate = this.loanInfo.FullyIndexedRate != 0.0 ? this.loanInfo.FullyIndexedRate + this.loanInfo.ARMfirstAdjCap : this.loanInfo.LoanRate + this.loanInfo.ARMfirstAdjCap;
            this.loanInfo.RateFactor = currentRate / 1200.0;
          }
          if (this.loanInfo.LoanPeriod % this.loanInfo.NegAdjPeriod == 1 && this.loanInfo.LoanPeriod > 1)
          {
            this.loanInfo.SteadyOptionInitialPayment *= 1.0 + this.loanInfo.NegAdjCap;
            this.loanInfo.SteadyOptionInitialPayment = this.EMRounding(this.loanInfo.SteadyOptionInitialPayment, 2);
          }
        }
        double num2 = this.EMRounding(unpaidBalance * this.loanInfo.RateFactor, 2);
        this.paySchedule[num1 - 1] = new PaymentSchedule();
        this.paySchedule[num1 - 1].Interest = num2;
        this.paySchedule[num1 - 1].CurrentRate = currentRate;
        double num3 = this.EMRounding(this.loanInfo.SteadyOptionInitialPayment - num2 + this.loanInfo.ExtraPayment, 2);
        if (unpaidBalance - num3 > this.loanInfo.NegMaxBalance)
          num3 = 0.0;
        this.paySchedule[num1 - 1].Principal = num3;
        unpaidBalance -= num3;
        this.paySchedule[num1 - 1].MortgageInsurance = 0.0;
        this.paySchedule[num1 - 1].Balance = unpaidBalance;
        this.paySchedule[num1 - 1].Payment = this.EMRounding(num3 + num2, 2);
        this.paySchedule[num1 - 1].PayDate = duePayDate.ToString("MM/dd/yyyy");
        this.findCutoffDates();
        duePayDate = duePayDate.AddMonths(1);
        if (duePayDate.Day != day && duePayDate.Day < day && duePayDate.AddDays((double) (day - duePayDate.Day)).Month == duePayDate.Month)
          duePayDate = duePayDate.AddDays((double) (day - duePayDate.Day));
        finCharge += num2;
        if (this.loanInfo.LoanPeriod == 1)
        {
          if (this.loanInfo.LoanType != "HELOC")
            this.SetCurrentNum("5", this.paySchedule[num1 - 1].Payment);
          this.SetField4085(this.Val("5"));
        }
        if (interestOnlyMonth <= this.loanInfo.InterestOnly)
          ++interestOnlyMonth;
        if (num3 == 0.0 || this.loanInfo.LoanPeriod > this.loanInfo.LoanTerm || this.loanInfo.LoanPeriod >= 1300)
          break;
      }
      if (flag)
      {
        this.loanInfo.ARMfirstChange = 0;
        this.loanInfo.ARMadjPeriod = 0;
      }
      monthlyPayment = this.CalcRawMonthlyPayment(this.loanInfo.LoanTerm - this.loanInfo.InterestOnly, unpaidBalance, currentRate, false);
      this.loanInfo.MonthlyPayment = monthlyPayment;
      return num1;
    }

    private void buildManualPayment(string id, string val)
    {
      if (Tracing.IsSwitchActive(RegzCalculation.sw, TraceLevel.Info))
        Tracing.Log(RegzCalculation.sw, TraceLevel.Info, nameof (RegzCalculation), "routine: buildManualPayment");
      this.loanInfo.LoanPeriod = 0;
      string str = this.Val("682");
      DateTime dateTime = DateTime.Now;
      if (str != CalculationBase.nil)
        dateTime = Convert.ToDateTime(str);
      int num1 = 0;
      for (int index = 0; index < 9; ++index)
        num1 += this.IntVal("GLOBAL.S" + (object) (index * 3 + 1));
      double num2 = this.FltVal("2");
      double val1 = 0.0;
      double num3 = 0.0;
      int num4 = num1;
      int num5 = this.IntVal("1198");
      int num6 = this.IntVal("1200");
      double num7 = this.FltVal("1766");
      double num8 = this.FltVal("1770");
      for (int index1 = 0; index1 < 9; ++index1)
      {
        int num9 = this.IntVal("GLOBAL.S" + (object) (index1 * 3 + 1));
        num3 = this.FltVal(Convert.ToString(index1 * 2 + 1679));
        double num10 = this.FltVal("GLOBAL.S" + (object) (index1 * 3 + 2));
        if (num9 != 0 && num3 != 0.0)
        {
          double num11 = !this.loanInfo.IsBiWeekly ? num3 / 1200.0 : num3 / (double) (100 * (this.loanInfo.DaysPerYearBiWeekly / 14));
          num4 -= num9;
          for (int index2 = 1; index2 <= num9; ++index2)
          {
            ++this.loanInfo.LoanPeriod;
            if (this.paySchedule[this.loanInfo.LoanPeriod - 1] == null)
              this.paySchedule[this.loanInfo.LoanPeriod - 1] = new PaymentSchedule();
            double num12 = this.loanInfo.LoanPeriod != 1 ? this.paySchedule[this.loanInfo.LoanPeriod - 2].Balance : this.loanInfo.LoanAmount;
            this.paySchedule[this.loanInfo.LoanPeriod - 1].MortgageInsurance = num5 <= 0 || num7 <= 0.0 || this.loanInfo.LoanPeriod > num5 || num12 < this.loanInfo.MICutOffAmount ? (num6 <= 0 || num8 <= 0.0 || this.loanInfo.LoanPeriod <= num5 || this.loanInfo.LoanPeriod > num6 || num12 < this.loanInfo.MICutOffAmount ? 0.0 : num8) : num7;
            double num13 = val1 + this.paySchedule[this.loanInfo.LoanPeriod - 1].MortgageInsurance;
            double num14 = Utils.ArithmeticRounding(num2 * num11, 2);
            val1 = num13 + num14;
            double num15;
            if (this.loanInfo.LoanPeriod >= num1)
            {
              num15 = num2;
              num10 = num2 + num14 + this.paySchedule[this.loanInfo.LoanPeriod - 1].MortgageInsurance;
            }
            else
              num15 = Utils.ArithmeticRounding(num10 - num14 - this.paySchedule[this.loanInfo.LoanPeriod - 1].MortgageInsurance, 2);
            if (num15 > num2)
            {
              num15 = num2;
              num10 = num2 + num14 + this.paySchedule[this.loanInfo.LoanPeriod - 1].MortgageInsurance;
            }
            this.paySchedule[this.loanInfo.LoanPeriod - 1].CurrentRate = num3;
            this.paySchedule[this.loanInfo.LoanPeriod - 1].Payment = num10;
            this.paySchedule[this.loanInfo.LoanPeriod - 1].PayDate = dateTime.ToString("MM/dd/yyyy");
            this.paySchedule[this.loanInfo.LoanPeriod - 1].Interest = num14;
            this.paySchedule[this.loanInfo.LoanPeriod - 1].Principal = num15;
            num2 = Utils.ArithmeticRounding(num2 - num15, 2);
            this.paySchedule[this.loanInfo.LoanPeriod - 1].Balance = num2;
            if (num2 > 0.0)
              dateTime = !this.loanInfo.IsBiWeekly ? dateTime.AddMonths(1) : dateTime.AddDays(14.0);
            else
              break;
          }
          if (num2 <= 0.0)
            break;
        }
        else
          break;
      }
      if (num2 > 0.0)
      {
        ++this.loanInfo.LoanPeriod;
        if (this.paySchedule[this.loanInfo.LoanPeriod - 1] == null)
          this.paySchedule[this.loanInfo.LoanPeriod - 1] = new PaymentSchedule();
        double num16 = this.loanInfo.LoanPeriod != 1 ? this.paySchedule[this.loanInfo.LoanPeriod - 2].Balance : this.loanInfo.LoanAmount;
        this.paySchedule[this.loanInfo.LoanPeriod - 1].MortgageInsurance = num5 <= 0 || num7 <= 0.0 || this.loanInfo.LoanPeriod > num5 || num16 < this.loanInfo.MICutOffAmount ? (num6 <= 0 || num8 <= 0.0 || this.loanInfo.LoanPeriod <= num5 || this.loanInfo.LoanPeriod > num6 || num16 < this.loanInfo.MICutOffAmount ? 0.0 : num8) : num7;
        double num17 = val1 + this.paySchedule[this.loanInfo.LoanPeriod - 1].MortgageInsurance;
        double num18 = !this.loanInfo.IsBiWeekly ? Utils.ArithmeticRounding(num2 * this.loanInfo.RateFactor, 2) : Utils.ArithmeticRounding(num2 * this.loanInfo.RateFactorBiWeekly, 2);
        val1 = num17 + num18;
        double num19 = num18 + num2 + this.paySchedule[this.loanInfo.LoanPeriod - 1].MortgageInsurance;
        if (num3 == 0.0)
        {
          if (this.loanInfo.LoanPeriod > 1)
            this.paySchedule[this.loanInfo.LoanPeriod - 2].CurrentRate = num3;
        }
        else
          this.paySchedule[this.loanInfo.LoanPeriod - 1].CurrentRate = num3;
        this.paySchedule[this.loanInfo.LoanPeriod - 1].Payment = num19;
        this.paySchedule[this.loanInfo.LoanPeriod - 1].PayDate = dateTime.ToString("MM/dd/yyyy");
        this.paySchedule[this.loanInfo.LoanPeriod - 1].Interest = num18;
        this.paySchedule[this.loanInfo.LoanPeriod - 1].Principal = num2;
      }
      double num20 = Utils.ArithmeticRounding(val1, 2) + this.FltVal("949");
      if (this.loanInfo.LoanPurpose == "ConstructionOnly")
        num20 += this.FltVal("NEWHUD2.X4768");
      this.SetCurrentNum("1206", num20, this.UseNoPayment(num20));
      double num21 = this.Val("1172") == "" ? this.FltVal("NEWHUD.X1707") : 0.0;
      double num22 = this.FltVal("GLOBAL.S2") != 0.0 ? this.FltVal("GLOBAL.S2") - num21 : (this.paySchedule == null || this.paySchedule[0] == null ? this.FltVal("5") - num21 : this.paySchedule[0].Payment - num21);
      this.SetCurrentNum("NEWHUD.X217", num22, this.UseNoPayment(num22));
      this.calculateIRS1098(id, val);
      this.calObjs.ATRQMCal.CalcMaxTotalPayments(id, val);
      this.calObjs.ATRQMCal.CalcuHousingDebtRatios(id, val);
      this.calObjs.VACal.CalcVACashOutRefinance(id, val);
    }

    private void populatePaymentSchedule()
    {
      Tracing.Log(RegzCalculation.sw, TraceLevel.Info, nameof (RegzCalculation), "routine: populatePaymentSchedule");
      int periodCount = 0;
      int line = 0;
      this.FltVal("3");
      double num1 = 0.0;
      double num2 = 0.0;
      double num3 = 0.0;
      double num4 = this.FltVal("1766");
      int num5 = this.IntVal("1198");
      double num6 = this.FltVal("1770");
      int num7 = this.IntVal("1200");
      int num8 = 0;
      string dueInDate = CalculationBase.nil;
      double monthlyPay = 0.0;
      double num9 = 0.0;
      int loanPeriod = this.loanInfo.LoanPeriod;
      if (this.Val("1659") == "Y" && loanPeriod > 0 && this.paySchedule[loanPeriod - 1] != null)
      {
        this.SetCurrentNum("RE88395.X121", this.paySchedule[loanPeriod - 1].Payment);
        this.SetVal("RE88395.X122", this.paySchedule[loanPeriod - 1].PayDate);
      }
      int num10 = -1;
      double num11 = 0.0;
      double num12 = 0.0;
      if (!this.skipPaymentScheduleCalculation)
      {
        for (int currentMonth = 0; currentMonth < loanPeriod; ++currentMonth)
        {
          if (this.paySchedule[currentMonth] != null)
          {
            if (currentMonth == 0)
            {
              dueInDate = this.paySchedule[currentMonth].PayDate;
              monthlyPay = this.paySchedule[currentMonth].Payment;
              num9 = this.paySchedule[currentMonth].CurrentRate;
              if (num9 < 0.0)
                break;
            }
            if (currentMonth > 0 && num10 == -1 && this.paySchedule[currentMonth].Balance < this.paySchedule[currentMonth - 1].Balance)
              num10 = currentMonth - 1;
            num1 = Utils.ArithmeticRounding(!(this.loanInfo.LoanPurpose == "ConstructionOnly") || !(this.loanInfo.ConstMethod == "A") || !this.loanInfo.IsARM ? (!this.loanInfo.LoanPurpose.StartsWith("Construction") || this.loanInfo.ConstMethod != "A" || this.loanInfo.LoanPurpose.StartsWith("Construction") && currentMonth >= this.loanInfo.ConstPeriod ? num1 + this.paySchedule[currentMonth].Payment : num1 + this.paySchedule[currentMonth].MortgageInsurance) : num1 + this.paySchedule[currentMonth].Payment, 2);
            num2 += this.paySchedule[currentMonth].Principal + this.paySchedule[currentMonth].Interest + this.paySchedule[currentMonth].MortgageInsurance;
            num11 += this.paySchedule[currentMonth].MortgageInsurance;
            if (currentMonth >= this.loanInfo.ConstPeriod)
              num12 += this.paySchedule[currentMonth].Interest;
            if ((monthlyPay != this.paySchedule[currentMonth].Payment || num9 != this.paySchedule[currentMonth].CurrentRate) && currentMonth > 0)
            {
              if (this.loanInfo.LoanPurpose != "ConstructionToPermanent" || currentMonth > this.loanInfo.ConstPeriod)
              {
                if (this.loanInfo.LoanPurpose != "ConstructionOnly" || this.loanInfo.ConstMethod != "A" || !this.loanInfo.IsARM)
                  line = this.populateTable(line, currentMonth, periodCount, monthlyPay, dueInDate, this.paySchedule[currentMonth - 1].Balance, this.paySchedule[currentMonth - 1].CurrentRate);
                num8 += periodCount;
              }
              dueInDate = this.paySchedule[currentMonth].PayDate;
              periodCount = 0;
            }
            ++periodCount;
            monthlyPay = this.paySchedule[currentMonth].Payment;
            num9 = this.paySchedule[currentMonth].CurrentRate;
            if (!this.loanInfo.UseSimpleInterest && line > 35)
              break;
          }
        }
        num8 += periodCount;
        if (this.loanInfo.LoanPeriod > 0 && line < 35)
        {
          if (this.loanInfo.LoanPurpose == "ConstructionOnly" && this.loanInfo.ConstMethod != "B")
          {
            line = this.populateTable(0, 0, 1, this.paySchedule[this.loanInfo.LoanPeriod - 1].Principal + this.paySchedule[this.loanInfo.LoanPeriod - 1].MortgageInsurance, this.paySchedule[this.loanInfo.LoanPeriod - 1].PayDate, 0.0, this.loanInfo.ConstIntRate);
            num8 = 1;
          }
          else
            line = this.populateTable(line, this.loanInfo.LoanPeriod - 1, periodCount, monthlyPay, dueInDate, this.paySchedule[loanPeriod - 1].Balance, this.paySchedule[loanPeriod - 1].CurrentRate);
        }
        if ((this.loanInfo.LoanPurpose == "ConstructionToPermanent" || this.loanInfo.LoanPurpose == "ConstructionOnly" && !this.loanInfo.IsARM) && this.loanInfo.ConstMethod != "B")
          num1 = Utils.ArithmeticRounding(num1 + this.loanInfo.ConstTotalFinCharge, 2);
        if (this.loanInfo.LoanPurpose == "ConstructionOnly" && this.loanInfo.ConstMethod != "B" && !this.loanInfo.IsARM)
          num1 += this.loanInfo.LoanAmount;
      }
      if (this.IsLocked("1206") || this.IsLocked("948"))
        num1 = this.FltVal("1206") + this.FltVal("948");
      this.SetCurrentNum("1207", Utils.ArithmeticRounding(num1, 2), this.UseNoPayment(num1));
      if (this.loanInfo.LoanPurpose == "ConstructionOnly")
      {
        double num13;
        if (this.loanInfo.ConstPeriod >= num5)
        {
          double num14 = num4 * (double) num5;
          num13 = this.loanInfo.ConstPeriod < num5 + num7 ? num14 + num6 * (double) (this.loanInfo.ConstPeriod - num5) : num14 + num6 * (double) num7;
        }
        else
          num13 = num3 + num4 * (double) this.loanInfo.ConstPeriod;
        this.SetCurrentNum("4071", Math.Round(this.FltVal("2") + this.FltVal("4088") + num13, 2));
      }
      else if (this.loanInfo.LoanPurpose == "ConstructionToPermanent")
        this.SetCurrentNum("4071", Math.Round(this.FltVal("2") + this.FltVal("4088") + num11 + num12, 2));
      else
        this.SetCurrentNum("4071", Math.Round(num2, 2));
      this.calObjs.NewHud2015Cal.CalcLoanTotalPayments();
      this.SetCurrentNum("948", this.FltVal("2") - this.FltVal("949"));
      this.SetCurrentNum("1701", (double) num8);
      this.calculatePMIMidpointCancelationDate((string) null, (string) null);
      for (int index = line + 1; index < 36; ++index)
      {
        this.SetVal("GLOBAL.S" + (object) ((index - 1) * 3 + 1), "");
        this.SetVal("GLOBAL.S" + (object) ((index - 1) * 3 + 2), "");
        if (index > 1)
          this.SetVal("GLOBAL.S" + (object) ((index - 1) * 3 + 3), "");
        if (index < 10)
        {
          this.SetVal(Convert.ToString((index - 1) * 2 + 1680), "");
          this.SetVal(Convert.ToString((index - 1) * 2 + 1679), "");
        }
        else
        {
          this.SetVal("GLOBAL.S" + Convert.ToString(index + 99), "");
          this.SetVal("GLOBAL.S" + Convert.ToString(index + 125), "");
        }
      }
      if (!this.IsLocked("NEWHUD.X4"))
      {
        double num15 = this.Val("1172") == "FarmersHomeAdministration" ? this.FltVal("NEWHUD.X1707") : 0.0;
        if (this.Val("423") == "Biweekly")
        {
          double num16 = this.FltVal("5") - num15;
          this.SetCurrentNum("NEWHUD.X217", num16, this.UseNoPayment(num16));
          this.SetVal("NEWHUD.X357", "N");
        }
        else
        {
          double num17 = this.FltVal("GLOBAL.S2") != 0.0 ? this.FltVal("GLOBAL.S2") - num15 : (this.paySchedule == null || this.paySchedule[0] == null ? this.FltVal("5") - num15 : (this.paySchedule[0].USDAMonthlyFee > 0.0 ? this.paySchedule[0].Payment - num15 : this.paySchedule[0].Payment));
          this.SetCurrentNum("NEWHUD.X217", num17, this.UseNoPayment(num17));
          if (this.FltVal("1199") > 0.0 && this.IntVal("1198") > 0 && num15 == 0.0)
            this.SetVal("NEWHUD.X357", "Y");
          else
            this.SetVal("NEWHUD.X357", "");
        }
        string val = this.Val("NEWHUD.X217");
        if (this.Flt(val) <= 0.0)
        {
          this.SetVal("NEWHUD.X355", "");
          this.SetVal("NEWHUD.X356", "");
          if (!string.IsNullOrEmpty(val) && this.UseNoPayment(this.Flt(val)))
            this.SetCurrentNum("NEWHUD.X217", 0.0, true);
          else
            this.SetVal("NEWHUD.X217", "");
        }
        else
        {
          this.SetVal("NEWHUD.X355", "Y");
          this.SetVal("NEWHUD.X356", "Y");
        }
      }
      this.SetVal("DISCLOSURE.X672", this.Val("NEWHUD.X357") == "Y" ? "Required" : "Not Required");
      if (this.Val("DISCLOSURE.X456") != "Y")
        this.SetVal("DISCLOSURE.X668", "");
      this.SetVal("ULDD.X185", this.Val("GLOBAL.S6"));
      this.calObjs.ATRQMCal.CalcMaxTotalPayments((string) null, (string) null);
      this.calObjs.ATRQMCal.CalcuHousingDebtRatios((string) null, (string) null);
      this.calObjs.VACal.CalcVACashOutRefinance((string) null, (string) null);
    }

    private int populateTable(
      int line,
      int currentMonth,
      int periodCount,
      double monthlyPay,
      string dueInDate,
      double unpaidBalance,
      double currentRate)
    {
      if (line >= 35)
        return 36;
      ++line;
      this.SetCurrentNum("GLOBAL.S" + (object) ((line - 1) * 3 + 1), (double) periodCount);
      if (monthlyPay != 0.0)
        this.SetCurrentNum("GLOBAL.S" + (object) ((line - 1) * 3 + 2), monthlyPay);
      else
        this.SetVal("GLOBAL.S" + (object) ((line - 1) * 3 + 2), "0.00");
      this.SetVal("GLOBAL.S" + (object) ((line - 1) * 3 + 3), dueInDate);
      if (line < 10)
      {
        if (unpaidBalance != 0.0)
          this.SetCurrentNum(Convert.ToString((line - 1) * 2 + 1680), unpaidBalance);
        else
          this.SetVal(Convert.ToString((line - 1) * 2 + 1680), "0.00");
        if (currentRate != 0.0)
          this.SetCurrentNum(Convert.ToString((line - 1) * 2 + 1679), currentRate);
        else
          this.SetVal(Convert.ToString((line - 1) * 2 + 1679), "0.000");
      }
      else
      {
        if (currentRate != 0.0)
          this.SetCurrentNum("GLOBAL.S" + Convert.ToString(line + 99), currentRate);
        else
          this.SetVal("GLOBAL.S" + Convert.ToString(line + 99), "0.000");
        if (unpaidBalance != 0.0)
          this.SetCurrentNum("GLOBAL.S" + Convert.ToString(line + 125), unpaidBalance);
        else
          this.SetVal("GLOBAL.S" + Convert.ToString(line + 125), "0.00");
      }
      return line;
    }

    private double calcFinanceAmount(
      double perRate,
      double firstPaymentLength,
      int startPayment,
      int numberPayments,
      bool noPayment,
      double finalPayment)
    {
      int y = 0;
      double num1 = 0.0;
      int num2 = (int) (firstPaymentLength - 1.0);
      double num3 = 1.0 + (firstPaymentLength - (double) (int) firstPaymentLength) * perRate;
      double x = perRate + 1.0;
      double num4 = this.paySchedule[startPayment].Payment;
      int num5 = startPayment + numberPayments;
      for (int index = startPayment; index < num5; ++index)
      {
        if (this.paySchedule[index] != null)
        {
          bool flag = noPayment && index == num5 - 1;
          if (this.paySchedule[index].Payment != num4 | flag)
          {
            double num6 = num3 * Math.Pow(x, (double) (index - startPayment + num2));
            double num7 = perRate == 0.0 ? 0.0 : (1.0 - Math.Pow(1.0 + perRate, (double) y)) / (-1.0 * perRate);
            if (num6 != 0.0)
              num1 += num4 * num7 / num6;
            num4 = !flag ? this.paySchedule[index].Payment : finalPayment;
            y = 1;
          }
          else
            ++y;
        }
      }
      double num8 = num3 * Math.Pow(x, (double) (numberPayments + num2));
      double num9 = perRate == 0.0 ? 0.0 : (1.0 - Math.Pow(1.0 + perRate, (double) y)) / (-1.0 * perRate);
      if (num8 != 0.0)
        num1 += num4 * num9 / num8;
      return Math.Round(num1, 2);
    }

    private double calcBuydownInterest(int curMonth, double loanBalance, ref double buydownRate)
    {
      double num1 = this.loanInfo.LoanRate / 1200.0;
      double num2 = Math.Round(loanBalance * num1, 2);
      int[] numArray1 = new int[6];
      double[] numArray2 = new double[6];
      bool flag = this.Val("CASASRN.X141") == "Borrower";
      if (flag || !flag && this.useInterimServicingScenario || this.Val("COMPLIANCEVERSION.CASASRNX141") == "Y" || this.Val("COMPLIANCEVERSION.NEWBUYDOWNENABLED") != "Y")
      {
        for (int index1 = 0; index1 < 6; ++index1)
        {
          int[] numArray3 = numArray1;
          int index2 = index1;
          int num3;
          int num4;
          if (flag || !this.useInterimServicingScenario)
          {
            num3 = index1 + 1613;
            num4 = this.IntVal(num3.ToString());
          }
          else
          {
            num3 = index1 + 4541;
            num4 = this.IntVal(num3.ToString());
          }
          numArray3[index2] = num4;
          double[] numArray4 = numArray2;
          int index3 = index1;
          double num5;
          if (flag || !this.useInterimServicingScenario)
          {
            num3 = index1 + 1269;
            num5 = this.FltVal(num3.ToString());
          }
          else
          {
            num3 = index1 + 4535;
            num5 = this.FltVal(num3.ToString());
          }
          numArray4[index3] = num5;
        }
      }
      int num6 = 0;
      for (int index = 0; index < 6 && numArray1[index] != 0; ++index)
      {
        num6 += numArray1[index];
        if (curMonth <= num6)
        {
          buydownRate = this.loanInfo.LoanRate - numArray2[index];
          break;
        }
      }
      if (curMonth <= this.loanInfo.InterestOnly && this.loanInfo.InterestOnly > 0)
      {
        double num7 = buydownRate / 1200.0;
        return Math.Round(loanBalance * num7, 2);
      }
      if (num1 < 0.0)
        ;
      this.loanInfo.BuydownMonthlyPayment = this.CalcRawMonthlyPayment(this.loanInfo.LoanTerm - this.loanInfo.InterestOnly, this.loanInfo.LoanAmount, buydownRate, false);
      return Math.Round(this.loanInfo.BuydownMonthlyPayment - Math.Round(this.loanInfo.MonthlyPayment - num2, 2), 2);
    }

    private double calcMortgageInsurance(string id, int totalMonths)
    {
      if (totalMonths == 0 || totalMonths > this.loanInfo.LoanTerm && this.Val("19") != "ConstructionToPermanent")
        return 0.0;
      double num1 = 0.0;
      double num2 = 0.0;
      int currentMonth = 1;
      int num3 = 0;
      int num4 = 60;
      string dateValue = "";
      int loanTerm = this.loanInfo.LoanTerm;
      DateTime date1 = Utils.ParseDate((object) this.Val("HUD68"));
      if (date1 != DateTime.MinValue)
      {
        while (currentMonth <= totalMonths && !(Utils.ParseDate((object) this.paySchedule[currentMonth - 1].PayDate) >= date1))
        {
          ++currentMonth;
          ++num4;
          ++loanTerm;
        }
      }
      int firstPermIndex = currentMonth;
      if (this.loanInfo.MIUseRemainBalance || this.loanInfo.DecliningRenewal)
      {
        double num5 = 0.0;
        double num6 = 0.0;
        for (; currentMonth <= totalMonths && (currentMonth <= this.loanInfo.MImidpointCutoff || this.loanInfo.MImidpointCutoff == 0); ++currentMonth)
        {
          if (this.loanInfo.MICutOffAmount > 0.0 && this.loanInfo.LoanType == "FHA")
          {
            if (currentMonth == firstPermIndex)
              num2 = this.loanInfo.BaseLoanAmount;
            else if (this.payFHASchedule[currentMonth - 2] != null)
              num2 = this.payFHASchedule[currentMonth - 2].Balance;
          }
          else
          {
            if (currentMonth == firstPermIndex)
              num2 = this.loanInfo.LoanAmount;
            else if (this.loanInfo.LoanType == "FarmersHomeAdministration" && this.paySchedule[currentMonth - 2] != null)
              num2 = this.paySchedule[currentMonth - 2].Balance;
            else if (this.payFHASchedule[currentMonth - 2] == null && this.paySchedule[currentMonth - 2] != null)
              num2 = this.paySchedule[currentMonth - 2].Balance;
            else if (this.payFHASchedule[currentMonth - 2] != null)
              num2 = this.paySchedule[currentMonth - 2].Balance;
            if (this.loanInfo.LoanType == "FHA" && this.loanInfo.MICutOffAmount != 0.0)
              num2 -= this.loanInfo.PMIAmout;
          }
          if (this.loanInfo.LoanType == "FHA" && this.loanInfo.LoanTerm > 180 && currentMonth <= num4 || num2 >= this.loanInfo.MICutOffAmount || currentMonth == firstPermIndex)
          {
            if ((currentMonth - firstPermIndex + 1) % 12 == 1)
            {
              if (this.loanInfo.DecliningRenewal && currentMonth <= 12)
              {
                num6 = this.findMtgInsuranceAmt(currentMonth, totalMonths, 0.0, firstPermIndex);
              }
              else
              {
                double yearlyTotal = 0.0;
                if (this.loanInfo.LoanPurpose == "ConstructionOnly" && this.loanInfo.MIUseRemainBalance)
                {
                  yearlyTotal = this.loanInfo.LoanAmount * 12.0;
                }
                else
                {
                  for (int index = 1; index <= 12; ++index)
                  {
                    if (currentMonth + index - 3 < 0)
                      num5 = this.loanInfo.LoanAmount;
                    else if (currentMonth + index - 3 >= loanTerm)
                    {
                      if (this.paySchedule[loanTerm - 1] != null)
                        num5 = this.paySchedule[loanTerm - 1].BalanceForMI;
                    }
                    else if (this.paySchedule[currentMonth + index - 3] != null)
                      num5 = this.paySchedule[currentMonth + index - 3].BalanceForMI;
                    yearlyTotal += num5;
                    if (this.loanInfo.DecliningRenewal)
                    {
                      yearlyTotal *= 12.0;
                      break;
                    }
                  }
                }
                num6 = yearlyTotal <= 0.0 ? 0.0 : this.findMtgInsuranceAmt(currentMonth, totalMonths, yearlyTotal, firstPermIndex);
              }
              if (this.loanInfo.LoanType == "FHA" && currentMonth == firstPermIndex && this.loanInfo.TotalMonthMI > 0)
                this.SetCurrentNum("1766", num6);
            }
            if (num2 >= this.loanInfo.MICutOffAmount || this.loanInfo.LoanType == "FHA" && this.loanInfo.LoanTerm > 180 && currentMonth <= num4)
            {
              this.paySchedule[currentMonth - 1].MortgageInsurance = num6;
              this.paySchedule[currentMonth - 1].Payment += num6;
              if (num6 > 0.0)
              {
                num3 = currentMonth;
                dateValue = this.paySchedule[currentMonth - 1].PayDate;
                num1 += num6;
              }
            }
            if (currentMonth == firstPermIndex)
              this.SetCurrentNum("1766", num6);
          }
          else
            break;
        }
      }
      else
      {
        for (; currentMonth <= totalMonths; ++currentMonth)
        {
          if (currentMonth <= this.loanInfo.MImidpointCutoff || this.loanInfo.MImidpointCutoff == 0)
          {
            double num7;
            if (this.loanInfo.MICutOffAmount > 0.0 && this.loanInfo.LoanType == "FHA")
            {
              num7 = currentMonth != firstPermIndex ? this.payFHASchedule[currentMonth - 2].Balance : this.loanInfo.BaseLoanAmount;
            }
            else
            {
              num7 = currentMonth != firstPermIndex ? this.paySchedule[currentMonth - 2].Balance : this.loanInfo.LoanAmount;
              if (this.loanInfo.LoanType == "FHA" && this.loanInfo.MICutOffAmount != 0.0)
                num7 -= this.loanInfo.PMIAmout;
            }
            if (num7 >= this.loanInfo.MICutOffAmount || this.loanInfo.LoanType == "FHA" && this.loanInfo.LoanTerm > 180 && currentMonth <= num4)
            {
              this.paySchedule[currentMonth - 1].MortgageInsurance = this.findMtgInsuranceAmt(currentMonth, totalMonths, 0.0, firstPermIndex);
              this.paySchedule[currentMonth - 1].Payment += this.paySchedule[currentMonth - 1].MortgageInsurance;
              if (this.paySchedule[currentMonth - 1].MortgageInsurance > 0.0)
              {
                num1 += this.paySchedule[currentMonth - 1].MortgageInsurance;
                num3 = currentMonth;
                dateValue = this.paySchedule[currentMonth - 1].PayDate;
              }
            }
            else
              break;
          }
        }
      }
      this.SetCurrentNum("409", (double) num3);
      if (num3 > 0)
      {
        DateTime date2 = Utils.ToDate(dateValue);
        DateTime dateTime1 = this.loanInfo.IsBiWeekly ? date2.AddDays(14.0) : date2.AddMonths(1);
        if (this.loanInfo.mthPMIPrepaid > 0)
          dateTime1 = this.loanInfo.IsBiWeekly ? dateTime1.AddDays((double) (this.loanInfo.mthPMIPrepaid * -14)) : dateTime1.AddMonths(this.loanInfo.mthPMIPrepaid * -1);
        if (!this.loanInfo.IsBiWeekly && dateTime1.Day >= 28 && this.paySchedule != null && this.paySchedule[0] != null && !string.IsNullOrEmpty(this.paySchedule[0].PayDate))
        {
          DateTime date3 = Utils.ParseDate((object) this.paySchedule[0].PayDate);
          if (date3.Day > dateTime1.Day)
          {
            for (int index = date3.Day - dateTime1.Day; index > 0; --index)
            {
              DateTime dateTime2 = dateTime1.AddDays((double) index);
              if (dateTime2.Month == dateTime1.Month)
              {
                dateTime1 = dateTime2;
                break;
              }
            }
          }
        }
        DateTime date4 = dateTime1.Date;
        if (date4.CompareTo(Utils.ToDate(this.paySchedule[this.loanInfo.LoanPeriod - 1].PayDate)) < 1)
        {
          date4 = dateTime1.Date;
          this.SetVal("CORRESPONDENT.X475", date4.ToString("MM/dd/yyyy"));
        }
        else
          this.SetVal("CORRESPONDENT.X475", "");
      }
      else
        this.SetVal("CORRESPONDENT.X475", "");
      this.calObjs.D1003Cal.RecalculateNMLS();
      return num1;
    }

    private double findMtgInsuranceAmt(
      int currentMonth,
      int totalMonth,
      double yearlyTotal,
      int firstPermIndex)
    {
      if (currentMonth > totalMonth)
        return 0.0;
      double mtgInsuranceAmt1 = this.FltVal("1766");
      int num1 = this.IntVal("1198");
      double num2 = this.FltVal("1199") / 100.0;
      if (this.loanInfo.LoanType == "FHA" && this.loanInfo.LoanTerm > 180 && num1 < 60)
        num1 = 60;
      if (this.loanInfo.IsBiWeekly)
      {
        num1 = num1 % 6 != 0 ? 0 : num1 / 6 * 13;
        mtgInsuranceAmt1 = mtgInsuranceAmt1 * 12.0 / 26.0;
      }
      if (num1 != 0 && currentMonth <= num1 + firstPermIndex - 1)
      {
        if (yearlyTotal > 0.0)
        {
          double num3;
          if (this.loanInfo.LoanType == "FarmersHomeAdministration")
          {
            double num4 = Utils.ArithmeticRounding(yearlyTotal / 12.0, 2);
            this.paySchedule[currentMonth - 1].USDAAnnualUPB = num4;
            num3 = Utils.ArithmeticRounding(num4 * num2, 2);
            this.paySchedule[currentMonth - 1].USDAAnnualFee = num3;
            if (currentMonth == firstPermIndex)
              this.SetCurrentNum("HUD.YearlyUSDAFee", num3);
          }
          else
            num3 = Utils.ArithmeticRounding(yearlyTotal / 12.0 * num2, 2);
          if (this.loanInfo.MIBasedOn == "Base Loan Amount" && this.loanInfo.MIRatePMI > 0.0 && this.loanInfo.PMIAmout > 0.0)
            num3 = Utils.ArithmeticRounding(num3 / (1.0 + this.loanInfo.MIRatePMI), 2);
          if (this.loanInfo.LoanType == "FarmersHomeAdministration")
          {
            mtgInsuranceAmt1 = Utils.ArithmeticRounding(num3 / (double) this.loanInfo.NumberofPayPerYear, 2);
            this.paySchedule[currentMonth - 1].USDAMonthlyFee = mtgInsuranceAmt1;
          }
          else
            mtgInsuranceAmt1 = Utils.ArithmeticRounding(num3 / (double) this.loanInfo.NumberofPayPerYear, 2);
        }
        return mtgInsuranceAmt1;
      }
      double mtgInsuranceAmt2 = this.FltVal("1770");
      int num5 = this.IntVal("1200");
      double num6 = this.FltVal("1201") / 100.0;
      if (this.loanInfo.IsBiWeekly)
      {
        num5 = num5 % 6 != 0 ? 0 : num5 / 6 * 13;
        mtgInsuranceAmt2 = mtgInsuranceAmt2 * 12.0 / 26.0;
      }
      if (mtgInsuranceAmt2 == 0.0 || num5 == 0 || currentMonth > num5 + num1 + firstPermIndex - 1)
        return 0.0;
      if (yearlyTotal > 0.0)
        mtgInsuranceAmt2 = this.EMRounding(yearlyTotal * (num6 / (double) this.loanInfo.NumberofPayPerYear) / (double) this.loanInfo.NumberofPayPerYear, 2);
      return mtgInsuranceAmt2;
    }

    public static double CalcInterestPayment(
      double unpaidBalance,
      double currentRate,
      bool isBiweekly,
      int daysPerYear)
    {
      double num = !isBiweekly ? currentRate / 1200.0 : currentRate / (100.0 * ((double) daysPerYear / 14.0));
      return Math.Round(unpaidBalance * num + 0.0001, 2);
    }

    public double CalcRawBiweeklyPayment(
      int unpaidTerm,
      double unpaidBalance,
      double currentRate,
      string zeroPercentPaymentOption)
    {
      double rateFactorBiWeekly = currentRate / (100.0 * ((double) this.loanInfo.DaysPerYearBiWeekly / 14.0));
      int paidTerm = this.loanInfo.LoanTerm - unpaidTerm + 1;
      if (paidTerm < 0)
        paidTerm = 0;
      return RegzCalculation.CalcRawMonthlyPayment(unpaidTerm, unpaidBalance, currentRate, false, true, rateFactorBiWeekly, this.loanInfo.RateForGPM, (double) this.loanInfo.YearsForGPM, this.loanInfo.NumberofPayPerYear, this.loanInfo.LoanTerm, paidTerm, this.loanInfo.LoanAmount, zeroPercentPaymentOption, this.loanInfo.UseSimpleInterest, !(this.loanInfo.LoanPurpose == "ConstructionToPermanent") || this.loanInfo.LoanTermConstPlusPerm == this.loanInfo.LoanTerm ? this.loanInfo.FirstPaymentDate : this.loanInfo.ConstFirstAmortDate, this.loanInfo.ConstInterestType, this.loanInfo.SimpleInterestUse366ForLeapYear);
    }

    public double CalcRawMonthlyPayment(
      int unpaidTerm,
      double unpaidBalance,
      double currentRate,
      bool isGPM)
    {
      return RegzCalculation.CalcRawMonthlyPayment(unpaidTerm, unpaidBalance, currentRate, isGPM, this.loanInfo.IsBiWeekly, this.loanInfo.RateFactorBiWeekly, this.loanInfo.RateForGPM, (double) this.loanInfo.YearsForGPM, this.loanInfo.NumberofPayPerYear, this.loanInfo.LoanTerm, this.loanInfo.LoanPeriod, this.loanInfo.LoanAmount, this.loanInfo.ZeroPercentPaymentOption, this.loanInfo.UseSimpleInterest, !(this.loanInfo.LoanPurpose == "ConstructionToPermanent") || this.loanInfo.LoanTermConstPlusPerm == this.loanInfo.LoanTerm ? this.loanInfo.FirstPaymentDate : this.loanInfo.ConstFirstAmortDate, this.loanInfo.ConstInterestType, this.loanInfo.SimpleInterestUse366ForLeapYear);
    }

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
      return RegzCalculation.CalcRawMonthlyPayment(unpaidTerm, unpaidBalance, currentRate, false, false, 0.0, 0.0, 0.0, 12, loanTerm, paidTerm, loanAmount, zeroPercentPaymentOption, useSimpleInterest, firstPaymentDate, constInterestType, simpleInterestUse366ForLeapYear);
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
      return RegzCalculation.CalcRawMonthlyPayment(unpaidTerm, unpaidBalance, currentRate, false, isBiweekly, rateFactorBiWeekly, 0.0, 0.0, numberofPayPerYear, loanTerm, paidTerm, loanAmount, zeroPercentPaymentOption, useSimpleInterest, firstPaymentDate, constInterestType, simpleInterestUse366ForLeapYear);
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
      if (unpaidTerm > 1300 || unpaidTerm < 1 || currentRate == 0.0 && zeroPercentPaymentOption != "AmortizingPayment" || unpaidBalance < 0.0)
        return 0.0;
      double num1 = !isBiweekly ? currentRate / 1200.0 : rateFactorBiWeekly;
      double num2 = Math.Pow(1.0 + num1, (double) unpaidTerm);
      double val1 = 0.0;
      if (currentRate == 0.0)
        val1 = unpaidBalance / (double) unpaidTerm;
      else if (num2 > 0.0)
      {
        if (isGPM)
        {
          double num3 = rateForGPM / 100.0;
          double num4 = (1.0 - 1.0 / Math.Pow(1.0 + num1, 12.0)) / num1;
          double num5 = Math.Pow(1.0 + num1, 12.0) / (1.0 + num3) - 1.0;
          double num6 = (1.0 - 1.0 / Math.Pow(1.0 + num5, yearsForGPM)) / num5;
          double num7 = num4 * num6 * (1.0 + num5);
          double y1 = (double) unpaidTerm - yearsForGPM * 12.0;
          double num8 = (1.0 - 1.0 / Math.Pow(1.0 + num1, y1)) / num1 * Math.Pow(1.0 + num3, yearsForGPM) / Math.Pow(1.0 + num1, yearsForGPM * 12.0);
          double num9 = loanAmount / (num7 + num8);
          if (numberofPayPerYear != 0)
          {
            int y2 = paidTerm / numberofPayPerYear;
            val1 = num9 * Math.Pow(1.0 + num3, (double) y2);
          }
          else
            val1 = 0.0;
        }
        else if (isBiweekly)
        {
          double num10 = currentRate / 1200.0;
          double val2 = (unpaidBalance / ((1.0 - 1.0 / Math.Pow(1.0 + num10, (double) loanTerm)) / num10) * 100.0 + 0.5) / 2.0 / 100.0;
          double num11 = paidTerm != 0 ? -1.0 * Math.Log(1.0 - num1 * unpaidBalance / val2) / Math.Log(1.0 + num1) - (double) paidTerm + 1.0 : -1.0 * Math.Log(1.0 - num1 * unpaidBalance / val2) / Math.Log(1.0 + num1);
          if (double.IsNaN(num11))
            return Utils.ArithmeticRounding(val2, 2);
          val1 = unpaidBalance / ((1.0 - 1.0 / Math.Pow(1.0 + num1, num11)) / num1);
        }
        else
          val1 = unpaidBalance / ((1.0 - 1.0 / num2) / num1);
      }
      if (val1 <= 0.0)
        return 0.0;
      if (useSimpleInterest)
        val1 = RegzCalculation.calcSimpleInterestMonthlyPayment(unpaidTerm, unpaidBalance, currentRate, numberofPayPerYear, loanTerm, paidTerm, loanAmount, firstPaymentDate, constInterestType, simpleInterestUse366ForLeapYear, Utils.ArithmeticRounding(val1, 2));
      return Utils.ArithmeticRounding(val1, 2);
    }

    public static double calcSimpleInterestMonthlyPayment(
      int unpaidTerm,
      double unpaidBalance,
      double currentRate,
      int numberofPayPerYear,
      int loanTerm,
      int paidTerm,
      double loanAmount,
      DateTime firstPaymentDate,
      string constInterestType,
      bool SimpleInterestUse366ForLeapYear,
      double payment)
    {
      double principal = unpaidBalance;
      double rate = currentRate / 100.0;
      double usRuleAccruedInterest = 0.0;
      if (principal + usRuleAccruedInterest == 0.0 || unpaidTerm == 0)
        return 0.0;
      if (rate == 0.0)
        return Utils.ArithmeticRounding((principal + usRuleAccruedInterest) / (double) unpaidTerm, 2);
      double lastPaymentVariance = RegzCalculation.estimateUnpaidPrincipal(payment, principal, usRuleAccruedInterest, currentRate, loanTerm, unpaidTerm, firstPaymentDate, constInterestType, SimpleInterestUse366ForLeapYear);
      double num1 = 0.0;
      int num2 = 0;
      while (Math.Abs(lastPaymentVariance) > (double) unpaidTerm * 0.005 && num2 <= 100)
      {
        ++num2;
        double num3 = Utils.ArithmeticRounding(RegzCalculation.guesssNextPaymentAdjustment(lastPaymentVariance, rate, unpaidTerm, (double) numberofPayPerYear), 2);
        if (num3 != 0.0 && num3 + num1 != 0.0)
        {
          payment += num3;
          lastPaymentVariance = RegzCalculation.estimateUnpaidPrincipal(payment, principal, usRuleAccruedInterest, currentRate, loanTerm, unpaidTerm, firstPaymentDate, constInterestType, SimpleInterestUse366ForLeapYear);
          num1 = num3;
        }
        else
          break;
      }
      return Utils.ArithmeticRounding(payment, 2);
    }

    private static double estimateUnpaidPrincipal(
      double payment,
      double principal,
      double usRuleAccruedInterest,
      double rate,
      int loanTerm,
      int unpaidTerm,
      DateTime startDate,
      string constInterestType,
      bool SimpleInterestUse366ForLeapYear)
    {
      if (unpaidTerm == 0 || principal + usRuleAccruedInterest == 0.0)
        return 0.0;
      double num1 = usRuleAccruedInterest;
      double num2 = principal;
      for (int index = 1; index <= unpaidTerm; ++index)
      {
        DateTime periodFirstDate = startDate.AddMonths(loanTerm - unpaidTerm + index - 2);
        DateTime periodLastDate = startDate.AddMonths(loanTerm - unpaidTerm + index - 1);
        double forSimpleInterest = RegzCalculation.calculateMonthlyInterestForSimpleInterest(num2, rate, periodFirstDate, periodLastDate, constInterestType, SimpleInterestUse366ForLeapYear);
        double num3 = payment - forSimpleInterest;
        if (num1 == 0.0)
        {
          if (num3 > 0.0)
            num2 -= num3;
          else
            num1 -= num3;
        }
        else
        {
          num1 -= num3;
          if (num1 < 0.0)
          {
            num2 += num1;
            num1 = 0.0;
          }
        }
      }
      return Utils.ArithmeticRounding(num2, 2);
    }

    private static double guesssNextPaymentAdjustment(
      double lastPaymentVariance,
      double rate,
      int term,
      double paymentPerRatePeriod)
    {
      double num = lastPaymentVariance / ((double) term * Math.Pow(1.0 + rate, (double) term / (2.0 * paymentPerRatePeriod)));
      if (num == 0.0)
        num = lastPaymentVariance >= 0.0 ? 0.01 : -0.01;
      return num;
    }

    private double checkARMSettings(int idx, double currentRate)
    {
      if (this.loanInfo.RateCap == currentRate)
      {
        this.calculateRateFactor(idx, currentRate);
        return 0.0;
      }
      if (this.loanInfo.LoanPeriod != this.loanInfo.ARMfirstChange + 1 && this.loanInfo.ARMadjPeriod != 1 && this.loanInfo.ARMadjPeriod != 0 && (this.loanInfo.LoanPeriod - this.loanInfo.ARMfirstChange) % this.loanInfo.ARMadjPeriod != 1 || this.loanInfo.ARMadjPeriod == 0 && this.loanInfo.LoanPeriod > this.loanInfo.ARMfirstChange + 1)
        return 0.0;
      double num = this.loanInfo.LoanPeriod != this.loanInfo.ARMfirstChange + 1 ? this.loanInfo.ARMadjCap : this.loanInfo.ARMfirstAdjCap;
      if (num == 0.0)
        currentRate = this.loanInfo.RateCap;
      if (this.useBestCaseScenario)
      {
        currentRate -= num;
        if (currentRate < this.loanInfo.RateCap)
          currentRate = this.loanInfo.RateCap;
      }
      else if (this.useWorstCaseScenario)
      {
        currentRate += num;
        if (currentRate > this.loanInfo.RateCap)
          currentRate = this.loanInfo.RateCap;
      }
      else if (this.loanInfo.LoanRate <= this.loanInfo.FullyIndexedRate || this.loanInfo.ARMindex == 0.0 || this.loanInfo.LoanRate < this.loanInfo.RateCap)
      {
        currentRate += num;
        if (currentRate > this.loanInfo.RateCap)
          currentRate = this.loanInfo.RateCap;
      }
      else
      {
        currentRate -= num;
        if (currentRate < this.loanInfo.RateCap)
          currentRate = this.loanInfo.RateCap;
      }
      this.calculateRateFactor(idx, currentRate);
      return currentRate;
    }

    private void calculateRateFactor(int idx, double currentRate)
    {
      if (currentRate <= 0.0)
        return;
      if (this.loanInfo.IsBiWeekly)
        this.loanInfo.RateFactorBiWeekly = currentRate / (100.0 * ((double) this.loanInfo.DaysPerYearBiWeekly / 14.0));
      else
        this.loanInfo.RateFactor = this.calculateRateFactorMonthly(idx, currentRate);
    }

    private double calculateRateFactorMonthly(int idx, double currentRate)
    {
      double rateFactorMonthly;
      if (idx >= 1 && this.loanInfo.LoanPurpose == "ConstructionOnly" && this.loanInfo.IsARM && (this.loanInfo.ConstInterestType == "365/360" || this.loanInfo.ConstInterestType == "365/365"))
      {
        DateTime dateTime = this.loanInfo.ConstFirstPayDate.AddMonths(-1).AddMonths(idx - 1);
        int num1 = DateTime.DaysInMonth(dateTime.Year, dateTime.Month);
        int num2 = this.loanInfo.ConstInterestType == "365/365" ? 365 : 360;
        rateFactorMonthly = currentRate / 100.0 / (double) num2 * (double) num1;
      }
      else if (this.loanInfo.UseSimpleInterest && (this.loanInfo.ConstInterestType == "365/365" || this.loanInfo.ConstInterestType == "365/360"))
      {
        DateTime paymentDate = this.loanInfo.FirstPaymentDate.AddMonths(idx - 2);
        TimeSpan timeSpan = this.loanInfo.FirstPaymentDate.AddMonths(idx - 1) - paymentDate;
        int forSimpleInterest = RegzCalculation.getDaysInYearForSimpleInterest(paymentDate, this.loanInfo.SimpleInterestUse366ForLeapYear, this.loanInfo.ConstInterestType);
        rateFactorMonthly = currentRate / 100.0 / (double) forSimpleInterest * timeSpan.TotalDays;
      }
      else
        rateFactorMonthly = currentRate / 1200.0;
      return rateFactorMonthly;
    }

    private static int getDaysInYearForSimpleInterest(
      DateTime paymentDate,
      bool simpleInterestUse366ForLeapYear,
      string constInterestType)
    {
      if (!(constInterestType == "365/365"))
        return 360;
      return simpleInterestUse366ForLeapYear && DateTime.IsLeapYear(paymentDate.Year) ? 366 : 365;
    }

    public static double calculateMonthlyInterestForSimpleInterest(
      double unpaidPricinpal,
      double currentRate,
      DateTime periodFirstDate,
      DateTime periodLastDate,
      string constInterestType,
      bool SimpleInterestUse366ForLeapYear)
    {
      double num;
      if (constInterestType == "365/365" || constInterestType == "365/360")
      {
        TimeSpan timeSpan = periodLastDate - periodFirstDate;
        int forSimpleInterest = RegzCalculation.getDaysInYearForSimpleInterest(periodFirstDate, SimpleInterestUse366ForLeapYear, constInterestType);
        num = currentRate / 100.0 / (double) forSimpleInterest * timeSpan.TotalDays;
      }
      else
        num = currentRate / 1200.0;
      return Utils.ArithmeticRounding(num * unpaidPricinpal, 2);
    }

    private double checkNegativeARM(double prePayment, double balance, double curRate)
    {
      if (this.loanInfo.NegAdjCap == 0.0 || this.loanInfo.NegAdjPeriod == 0 || (double) this.loanInfo.LoanPeriod > this.loanInfo.NegAdjRecast + 1.0 && this.loanInfo.NegAdjRecast != 0.0)
        return 0.0;
      if (this.loanInfo.LoanPeriod > this.loanInfo.NegAdjStopAt + 1 && this.loanInfo.NegAdjStopAt != 0)
        return this.loanInfo.NegPayment;
      if (this.loanInfo.LoanPeriod % this.loanInfo.NegAdjPeriod != 1 || balance >= this.loanInfo.NegMaxBalance && this.loanInfo.NegMaxBalance > 0.0)
        return 0.0;
      double num1 = this.CalcRawMonthlyPayment(this.loanInfo.LoanTerm - this.loanInfo.LoanPeriod + 1, balance, curRate, false);
      double num2 = Math.Round(prePayment + prePayment * this.loanInfo.NegAdjCap, 2);
      if (num2 <= num1)
        num1 = num2;
      this.loanInfo.NegPayment = num1;
      return num1;
    }

    public static double CalculateMonthlyPayment(
      int loanTerm,
      int interestOnly,
      double loanAmount,
      double annualRate)
    {
      loanTerm -= interestOnly;
      if (loanTerm > 1300 || loanTerm < 1 || annualRate == 0.0 || loanAmount < 0.0)
        return 0.0;
      double num1 = annualRate / 1200.0;
      double num2 = Math.Pow(1.0 + num1, (double) loanTerm);
      double num3 = 0.0;
      if (num2 > 0.0 && num1 > 0.0)
        num3 = loanAmount / ((1.0 - 1.0 / num2) / num1);
      return Math.Round(num3, 2);
    }

    private double buildHELOCSchedule()
    {
      string str = this.Val("608");
      double num = 0.0;
      this.SetCurrentNum("3296", this.loanInfo.IsARM ? Utils.ArithmeticRounding(this.loanInfo.RateCap, 3) : 0.0);
      switch (str)
      {
        case "Fixed":
          num = this.calculateHomeEquity();
          break;
        case "AdjustableRate":
          num = this.calculateHELOC();
          break;
      }
      return num;
    }

    private double calculateHELOC()
    {
      this.loanInfo.LoanPeriod = 0;
      if (this.loanInfo.LoanAmount == 0.0 || this.loanInfo.FullyIndexedRate == 0.0)
        return 0.0;
      double num1 = this.FltVal("1413") / 100.0;
      if (num1 <= 0.0 && this.loanInfo.HelocMiniPayment <= 0.0 || this.loanInfo.ARMlifeCap <= 0.0 && this.loanInfo.HelocWorstCase || this.loanInfo.LoanTerm == 0 && this.loanInfo.InterestOnly > 0)
        return 0.0;
      if (this.loanInfo.BallonTerm == 0)
        this.loanInfo.BallonTerm = this.loanInfo.LoanTerm;
      DateTime dateTime1 = Utils.ParseDate((object) this.Val("682"));
      if (dateTime1 == DateTime.MinValue)
        dateTime1 = DateTime.Today;
      DateTime dateTime2 = dateTime1;
      if ((this.loanInfo.HelocTeaserRate > 0.0 || this.loanInfo.HelocTeaserMargin > 0.0) && this.loanInfo.ARMfirstChange < 1)
        return 0.0;
      bool flag = this.loanInfo.HelocDaysPerYear == 365;
      int num2 = this.loanInfo.NumberofPayPerYear * (this.loanInfo.LoanTerm / 12);
      if (this.loanInfo.BallonTerm != 0)
        num2 = this.loanInfo.NumberofPayPerYear * (this.loanInfo.BallonTerm / 12);
      if (num2 > 1300)
        num2 = 1299;
      if (this.loanInfo.ARMfirstChange == 0)
        this.loanInfo.ARMfirstChange = 1;
      if (this.loanInfo.ARMadjPeriod == 0)
        this.loanInfo.ARMadjPeriod = 1;
      double heloc = 0.0;
      double num3 = 0.0;
      double loanAmount = this.loanInfo.LoanAmount;
      while (loanAmount > 0.0)
      {
        ++this.loanInfo.LoanPeriod;
        double num4 = this.loanInfo.LoanPeriod != num2 ? Math.Round(num1 * loanAmount, 2) : loanAmount;
        DateTime dateTime3;
        switch (this.loanInfo.NumberofPayPerYear)
        {
          case 2:
            dateTime3 = dateTime2.AddMonths(-6);
            break;
          case 4:
            dateTime3 = dateTime2.AddMonths(-3);
            break;
          case 6:
            dateTime3 = dateTime2.AddMonths(-2);
            break;
          case 10:
            dateTime3 = dateTime2.AddDays(-36.0);
            break;
          case 26:
            dateTime3 = dateTime2.AddDays(-14.0);
            break;
          default:
            dateTime3 = dateTime2.AddMonths(-1);
            break;
        }
        int days = dateTime3.Subtract(dateTime2.Date).Days;
        if (this.loanInfo.LoanPeriod % this.loanInfo.ARMfirstChange == 1 || this.loanInfo.LoanPeriod > this.loanInfo.ARMfirstChange)
        {
          if (this.loanInfo.LoanPeriod > this.loanInfo.ARMfirstChange + this.loanInfo.ARMadjPeriod)
          {
            if (this.loanInfo.ARMadjCap > 0.0 && this.loanInfo.ARMadjPeriod > 0 && (this.loanInfo.LoanPeriod - this.loanInfo.ARMfirstChange) % this.loanInfo.ARMadjPeriod == 1 && this.loanInfo.LoanPeriod > 1)
              num3 += this.loanInfo.ARMadjCap;
          }
          else if (this.loanInfo.LoanPeriod % this.loanInfo.ARMfirstChange == 1 && this.loanInfo.LoanPeriod > 1 && this.loanInfo.ARMfirstAdjCap > 0.0 && this.loanInfo.ARMfirstChange > 0)
            num3 += this.loanInfo.ARMfirstAdjCap;
        }
        double num5 = this.loanInfo.HelocTeaserRate <= 0.0 || this.loanInfo.LoanPeriod > this.loanInfo.ARMfirstChange ? this.loanInfo.FullyIndexedRate : this.loanInfo.HelocTeaserRate;
        if (this.loanInfo.HelocWorstCase && this.loanInfo.LoanPeriod > 1)
          num5 = this.loanInfo.ARMlifeCap;
        double num6 = num5 + num3;
        if (num6 > this.loanInfo.ARMlifeCap + this.loanInfo.LoanRate)
          num6 = this.loanInfo.ARMlifeCap + this.loanInfo.LoanRate;
        double num7 = num6 / 100.0 / (double) this.loanInfo.HelocDaysPerYear * loanAmount;
        double num8 = !flag ? num7 * (double) (this.loanInfo.HelocDaysPerYear / this.loanInfo.NumberofPayPerYear) : num7 * (double) (days * -1);
        if (num4 < this.loanInfo.HelocMiniPayment)
          num4 = this.loanInfo.HelocMiniPayment;
        double num9 = Math.Round(num8, 2);
        double num10 = this.loanInfo.InterestOnly <= 0 || this.loanInfo.InterestOnly < this.loanInfo.LoanPeriod || this.loanInfo.LoanPeriod == num2 || this.loanInfo.HelocMiniPayment >= num4 ? (this.loanInfo.LoanPeriod != num2 ? num4 - num9 + this.loanInfo.ExtraPayment : loanAmount) : this.loanInfo.ExtraPayment;
        if (num10 < 0.0)
          num10 = 0.0;
        double num11 = Math.Round(num10, 2);
        if (num11 + num9 > loanAmount)
          num11 = loanAmount;
        loanAmount -= num11;
        this.paySchedule[this.loanInfo.LoanPeriod - 1] = new PaymentSchedule();
        this.paySchedule[this.loanInfo.LoanPeriod - 1].PayDate = dateTime2.ToString("MM/dd/yyyy");
        this.paySchedule[this.loanInfo.LoanPeriod - 1].Interest = num9;
        this.paySchedule[this.loanInfo.LoanPeriod - 1].Principal = num11;
        this.paySchedule[this.loanInfo.LoanPeriod - 1].MortgageInsurance = 0.0;
        this.paySchedule[this.loanInfo.LoanPeriod - 1].Payment = num9 + num11;
        this.paySchedule[this.loanInfo.LoanPeriod - 1].Balance = loanAmount;
        this.paySchedule[this.loanInfo.LoanPeriod - 1].CurrentRate = num6;
        if (this.loanInfo.LoanPeriod == 1 && string.IsNullOrEmpty(this.Val("4475")) && string.IsNullOrEmpty(this.Val("4530")))
        {
          this.SetCurrentNum("5", this.paySchedule[this.loanInfo.LoanPeriod - 1].Payment);
          this.SetField4085(this.Val("5"));
          this.calObjs.ToolCal.CalcNetTangibleBenefit("5", this.Val("5"));
          this.calObjs.HMDACal.CalcPrepaymentPenaltyPeriod((string) null, (string) null);
        }
        heloc += num9;
        switch (this.loanInfo.NumberofPayPerYear)
        {
          case 2:
            dateTime2 = dateTime1.AddMonths(this.loanInfo.LoanPeriod * 6);
            break;
          case 4:
            dateTime2 = dateTime1.AddMonths(this.loanInfo.LoanPeriod * 3);
            break;
          case 6:
            dateTime2 = dateTime1.AddMonths(this.loanInfo.LoanPeriod * 2);
            break;
          case 10:
            dateTime2 = dateTime1.AddDays((double) (this.loanInfo.LoanPeriod * 36));
            break;
          case 12:
            dateTime2 = dateTime1.AddMonths(this.loanInfo.LoanPeriod);
            break;
          case 26:
            dateTime2 = dateTime1.AddDays((double) (14 * this.loanInfo.LoanPeriod));
            break;
        }
        if (this.loanInfo.LoanPeriod >= num2)
          break;
      }
      this.SetCurrentNum("1206", heloc, this.UseNoPayment(heloc));
      return heloc;
    }

    private double calculateHomeEquity()
    {
      this.loanInfo.LoanPeriod = 0;
      if (this.loanInfo.LoanTerm == 0 || this.loanInfo.LoanRate == 0.0 || this.loanInfo.LoanAmount == 0.0)
        return 0.0;
      DateTime dateTime1 = Utils.ParseDate((object) this.Val("682"));
      if (dateTime1 == DateTime.MinValue)
        dateTime1 = DateTime.Today;
      DateTime dateTime2 = dateTime1;
      double helocTeaserRate = this.loanInfo.HelocTeaserRate;
      if (helocTeaserRate > 0.0 && this.loanInfo.ARMfirstChange < 1)
        return 0.0;
      int num1 = this.loanInfo.NumberofPayPerYear * (this.loanInfo.LoanTerm / 12);
      double num2 = this.loanInfo.LoanRate / 100.0 / (double) this.loanInfo.NumberofPayPerYear;
      double num3 = helocTeaserRate / 100.0 / (double) this.loanInfo.NumberofPayPerYear;
      double num4 = 1.0;
      for (int index = 1; index <= num1; ++index)
        num4 *= 1.0 + num2;
      double num5 = Math.Round(this.loanInfo.LoanAmount * num2 * num4 / (num4 - 1.0) + 0.0045, 2);
      if (num1 >= 1300)
        num1 = 1299;
      double homeEquity = 0.0;
      double loanAmount = this.loanInfo.LoanAmount;
      while (loanAmount > 0.0)
      {
        ++this.loanInfo.LoanPeriod;
        double num6 = helocTeaserRate <= 0.0 || this.loanInfo.LoanPeriod > this.loanInfo.ARMfirstChange ? Math.Round(num2 * loanAmount, 2) : Math.Round(num3 * loanAmount, 2);
        double num7 = num5 + this.loanInfo.ExtraPayment - num6;
        if (this.loanInfo.InterestOnly > 0 && this.loanInfo.LoanPeriod != num1)
          num7 = this.loanInfo.ExtraPayment;
        else if (this.loanInfo.LoanPeriod == num1)
          num7 = loanAmount;
        if (num7 + num6 > loanAmount)
          num7 = loanAmount;
        loanAmount -= num7;
        this.paySchedule[this.loanInfo.LoanPeriod - 1] = new PaymentSchedule();
        this.paySchedule[this.loanInfo.LoanPeriod - 1].PayDate = dateTime2.ToString("MM/dd/yyyy");
        this.paySchedule[this.loanInfo.LoanPeriod - 1].Interest = num6;
        this.paySchedule[this.loanInfo.LoanPeriod - 1].Principal = num7;
        this.paySchedule[this.loanInfo.LoanPeriod - 1].Payment = Math.Round(num6 + num7, 2);
        this.paySchedule[this.loanInfo.LoanPeriod - 1].MortgageInsurance = 0.0;
        this.paySchedule[this.loanInfo.LoanPeriod - 1].CurrentRate = this.loanInfo.LoanRate;
        this.paySchedule[this.loanInfo.LoanPeriod - 1].Balance = loanAmount;
        if (this.loanInfo.LoanPeriod == 1 && string.IsNullOrEmpty(this.Val("4475")) && string.IsNullOrEmpty(this.Val("4530")))
        {
          this.SetCurrentNum("5", this.paySchedule[this.loanInfo.LoanPeriod - 1].Payment);
          this.SetField4085(this.Val("5"));
          this.calObjs.ToolCal.CalcNetTangibleBenefit("5", this.Val("5"));
          this.calObjs.HMDACal.CalcPrepaymentPenaltyPeriod((string) null, (string) null);
        }
        homeEquity += num6;
        switch (this.loanInfo.NumberofPayPerYear)
        {
          case 2:
            dateTime2 = dateTime1.AddMonths(this.loanInfo.LoanPeriod * 6);
            break;
          case 4:
            dateTime2 = dateTime1.AddMonths(this.loanInfo.LoanPeriod * 3);
            break;
          case 6:
            dateTime2 = dateTime1.AddMonths(this.loanInfo.LoanPeriod * 2);
            break;
          case 10:
            dateTime2 = dateTime1.AddDays((double) (this.loanInfo.LoanPeriod * 36));
            break;
          case 12:
            dateTime2 = dateTime1.AddMonths(this.loanInfo.LoanPeriod);
            break;
          case 26:
            dateTime2 = dateTime1.AddDays((double) (14 * this.loanInfo.LoanPeriod));
            break;
        }
        if (this.loanInfo.LoanPeriod >= num1)
          break;
      }
      return homeEquity;
    }

    private double shiftPremiumMI()
    {
      if (this.loanInfo.mthPMIPrepaid == 0)
        return 0.0;
      double num = 0.0;
      for (int index = 0; index < this.loanInfo.mthPMIPrepaid; ++index)
      {
        if (index < this.paySchedule.Length && this.paySchedule[index] != null)
          num += this.paySchedule[index].MortgageInsurance;
      }
      for (int mthPmiPrepaid = this.loanInfo.mthPMIPrepaid; mthPmiPrepaid < this.loanInfo.LoanPeriod; ++mthPmiPrepaid)
      {
        this.paySchedule[mthPmiPrepaid - this.loanInfo.mthPMIPrepaid].MortgageInsurance = this.paySchedule[mthPmiPrepaid].MortgageInsurance;
        this.paySchedule[mthPmiPrepaid].MortgageInsurance = 0.0;
      }
      for (int index = 0; index < this.loanInfo.LoanPeriod; ++index)
        this.paySchedule[index].Payment = Math.Round(this.paySchedule[index].Principal + this.paySchedule[index].Interest + this.paySchedule[index].MortgageInsurance, 2);
      return num;
    }

    private void findCutoffDates()
    {
      string empty = string.Empty;
      if (this.loanInfo.AmtCutOff80 > 0.0 && this.loanInfo.DateCutOff80 == string.Empty && this.paySchedule[this.loanInfo.LoanPeriod - 1].Balance <= this.loanInfo.AmtCutOff80)
      {
        string payDate = this.paySchedule[this.loanInfo.LoanPeriod - 1].PayDate;
        try
        {
          DateTime dateTime = DateTime.Parse(this.paySchedule[this.loanInfo.LoanPeriod - 1].PayDate).Date;
          dateTime = dateTime.AddMonths(1);
          if (this.loanInfo.mthPMIPrepaid > 0)
            payDate = dateTime.AddMonths(this.loanInfo.mthPMIPrepaid * -1).ToString("MM/dd/yyyy");
        }
        catch (Exception ex)
        {
        }
        this.loanInfo.DateCutOff80 = payDate;
      }
      if (this.loanInfo.AmtCutOff78 <= 0.0 || !(this.loanInfo.DateCutOff78 == string.Empty) || this.paySchedule[this.loanInfo.LoanPeriod - 1].Balance > this.loanInfo.AmtCutOff78)
        return;
      string payDate1 = this.paySchedule[this.loanInfo.LoanPeriod - 1].PayDate;
      try
      {
        DateTime dateTime = DateTime.Parse(this.paySchedule[this.loanInfo.LoanPeriod - 1].PayDate).Date;
        dateTime = dateTime.AddMonths(1);
        if (this.loanInfo.mthPMIPrepaid > 0)
          payDate1 = dateTime.AddMonths(this.loanInfo.mthPMIPrepaid * -1).ToString("MM/dd/yyyy");
      }
      catch (Exception ex)
      {
      }
      this.loanInfo.DateCutOff78 = payDate1;
    }

    private double calcConstructionInterest(double currentRate)
    {
      if (this.loanInfo.LoanAmount == 0.0 || this.loanInfo.ConstPeriod == 0)
        return 0.0;
      double num1 = 0.0;
      double num2;
      if (this.loanInfo.ConstMethod != "B")
      {
        if (this.loanInfo.ConstInterestType != "360/360")
        {
          if (this.loanInfo.ConstInterestType == "365/365")
            num1 = currentRate / 100.0 / 365.0 * (this.loanInfo.LoanAmount / 2.0);
          else if (this.loanInfo.ConstInterestType == "365/360")
            num1 = currentRate / 100.0 / 360.0 * (this.loanInfo.LoanAmount / 2.0);
          TimeSpan timeSpan;
          if (this.loanInfo.LoanPurpose == "ConstructionOnly")
          {
            this.loanInfo.ConstSpanStartDate = !(this.loanInfo.DisbursementDate != DateTime.MinValue) ? this.loanInfo.ConstEstClosingDate : this.loanInfo.DisbursementDate;
            timeSpan = this.loanInfo.ConstFinalPayDate.Subtract(this.loanInfo.ConstSpanStartDate);
          }
          else
          {
            DateTime dateTime = this.loanInfo.ConstFirstAmortDate.AddMonths(-1);
            this.loanInfo.ConstSpanStartDate = !(this.loanInfo.DisbursementDate != DateTime.MinValue) ? this.loanInfo.ConstEstClosingDate : this.loanInfo.DisbursementDate;
            timeSpan = dateTime.Subtract(this.loanInfo.ConstSpanStartDate);
          }
          num2 = (double) timeSpan.Days * num1;
          if (this.loanInfo.ConstFirstPayDate.Day == 1)
          {
            this.loanInfo.ConstMonthlyPayment = (double) DateTime.DaysInMonth(this.loanInfo.ConstFirstPayDate.Year, this.loanInfo.ConstFirstPayDate.Month) * num1;
          }
          else
          {
            timeSpan = this.loanInfo.ConstFirstPayDate.AddMonths(1).Subtract(this.loanInfo.ConstFirstPayDate);
            this.loanInfo.ConstMonthlyPayment = (double) timeSpan.Days * num1;
          }
          if (this.loanInfo.NeedToCalcEstConstIntrest)
          {
            this.loanInfo.ConstPeriodicRate = !(this.loanInfo.ConstInterestType == "365/360") ? this.loanInfo.ConstIntRate / 365.0 * (double) timeSpan.Days : this.loanInfo.ConstIntRate / 360.0 * (double) timeSpan.Days;
            this.loanInfo.ConstEstInterestReserve = this.loanInfo.ConstPeriodicRate / 100.0 * num2 / 2.0;
            num2 = this.loanInfo.ConstEstInterestReserve + num2;
          }
        }
        else
        {
          this.loanInfo.ConstMonthlyPayment = this.loanInfo.LoanAmount / 2.0 * (currentRate / 100.0) / 12.0;
          num2 = this.loanInfo.ConstMonthlyPayment * (double) this.loanInfo.ConstPeriod;
          if (this.loanInfo.NeedToCalcEstConstIntrest)
          {
            this.loanInfo.ConstPeriodicRate = this.loanInfo.ConstIntRate / 12.0 * (double) this.loanInfo.ConstPeriod;
            this.loanInfo.ConstEstInterestReserve = this.loanInfo.ConstPeriodicRate / 100.0 * num2 / 2.0;
            num2 = this.loanInfo.ConstEstInterestReserve + num2;
          }
        }
        if (this.loanInfo.LoanPurpose == "ConstructionOnly")
        {
          double num3 = num2 * 2.0 / (double) this.loanInfo.ConstPeriod;
          if (this.f_le1x24_cd4x34 < num3)
            this.f_le1x24_cd4x34 = num3;
        }
      }
      else
      {
        double num4 = !(this.loanInfo.ConstInterestType != "365/365") ? currentRate / 100.0 / 365.0 * this.loanInfo.LoanAmount : currentRate / 100.0 / 360.0 * this.loanInfo.LoanAmount;
        DateTime dateTime1 = this.loanInfo.ConstEstClosingDate;
        if ((this.loanInfo.ConstInterestType == "365/365" || this.loanInfo.ConstInterestType == "365/360") && this.loanInfo.DisbursementDate != DateTime.MinValue)
          dateTime1 = this.loanInfo.DisbursementDate;
        if (this.loanInfo.LoanPeriod <= 1 || currentRate != this.loanInfo.ConstIntRate)
        {
          DateTime dateTime2 = dateTime1.AddMonths(1);
          if (this.loanInfo.ConstFirstPayDate.Day == DateTime.DaysInMonth(this.loanInfo.ConstFirstPayDate.Year, this.loanInfo.ConstFirstPayDate.Month))
            this.loanInfo.ConstUseLastDay = true;
          TimeSpan timeSpan;
          if (this.loanInfo.ConstFirstPayDate < dateTime2)
          {
            this.loanInfo.ConstSpanStartDate = dateTime1;
            this.loanInfo.ConstFinalPayDate = this.loanInfo.ConstFirstPayDate.AddMonths(this.loanInfo.ConstPeriod - 1);
            timeSpan = this.loanInfo.ConstFinalPayDate.Subtract(this.loanInfo.ConstSpanStartDate);
          }
          else
          {
            this.loanInfo.ConstFinalPayDate = this.loanInfo.ConstFirstPayDate.AddMonths(this.loanInfo.ConstPeriod - 1);
            if (dateTime1.Day == DateTime.DaysInMonth(dateTime1.Year, dateTime1.Month))
            {
              this.loanInfo.ConstFinalPayDate = new DateTime(this.loanInfo.ConstFinalPayDate.Year, this.loanInfo.ConstFinalPayDate.Month, DateTime.DaysInMonth(this.loanInfo.ConstFinalPayDate.Year, this.loanInfo.ConstFinalPayDate.Month));
              this.loanInfo.ConstUseLastDay = true;
            }
            this.loanInfo.ConstSpanStartDate = dateTime1.Day == this.loanInfo.ConstFirstPayDate.Day ? dateTime1 : (this.loanInfo.IsARM || !(this.loanInfo.ConstInterestType == "365/365") && !(this.loanInfo.ConstInterestType == "365/360") ? this.loanInfo.ConstFirstPayDate.AddMonths(-1) : dateTime1);
            timeSpan = this.loanInfo.ConstFinalPayDate.Subtract(this.loanInfo.ConstSpanStartDate);
          }
          this.loanInfo.ConstMonthlyPayment = !this.loanInfo.IsBiWeekly ? (this.loanInfo.ConstInterestType == "365/365" || this.loanInfo.ConstInterestType == "365/360" ? num4 * (double) timeSpan.Days / (double) this.loanInfo.ConstPeriod : this.loanInfo.LoanAmount * (currentRate / 1200.0)) : (this.loanInfo.ConstInterestType == "365/365" || this.loanInfo.ConstInterestType == "365/360" ? num4 * 14.0 : this.loanInfo.LoanAmount * currentRate / 100.0 / 26.0);
          num2 = this.loanInfo.ConstMonthlyPayment;
          if (this.loanInfo.NeedToCalcEstConstIntrest && !this.loanInfo.IsARM)
          {
            this.loanInfo.ConstPeriodicRate = !(this.loanInfo.ConstInterestType == "365/360") ? (!(this.loanInfo.ConstInterestType == "365/365") ? this.loanInfo.ConstIntRate / 12.0 * (double) this.loanInfo.ConstPeriod : this.loanInfo.ConstIntRate / 365.0 * (double) timeSpan.Days) : this.loanInfo.ConstIntRate / 360.0 * (double) timeSpan.Days;
            this.loanInfo.ConstEstInterestReserve = this.loanInfo.ConstPeriodicRate / 100.0 * num2 / 2.0;
            num2 += this.loanInfo.ConstEstInterestReserve;
          }
        }
        else
        {
          num2 = this.loanInfo.ConstMonthlyPayment;
          if (this.loanInfo.NeedToCalcEstConstIntrest && !this.loanInfo.IsARM)
            num2 += this.loanInfo.ConstEstInterestReserve;
        }
      }
      return num2;
    }

    private void calculateFloorRate(string id, string val)
    {
      if (this.Val("ARM.FlrBasis") == "NoteRate")
        this.SetCurrentNum("1699", this.FltVal("3"));
      else if (this.Val("ARM.ApplyLfCpLow") == "Y")
      {
        if (this.FltVal("3") - this.FltVal("247") < this.FltVal("689"))
          this.SetCurrentNum("1699", this.FltVal("689"));
        else
          this.SetCurrentNum("1699", this.FltVal("3") - this.FltVal("247"));
      }
      else
        this.SetCurrentNum("1699", this.FltVal("689"));
      this.calObjs.NewHud2015Cal.CalcLECDDisplayFields("1699", (string) null);
    }

    private void calculateARMDisclosureSample(string id, string val)
    {
      double num1 = this.FltVal("3") / 1200.0;
      double num2 = Math.Pow(1.0 + num1, (double) this.IntVal("4"));
      if (num2 == 1.0)
      {
        this.SetCurrentNum("1895", 0.0);
        this.SetCurrentNum("1958", 0.0);
      }
      else
      {
        double num3 = Math.Round(10000.0 / ((1.0 - 1.0 / num2) / num1), 2);
        this.SetCurrentNum("1895", num3);
        this.SetCurrentNum("1958", num3 * 6.0);
      }
      bool calculationDiagnostic = EnConfigurationSettings.GlobalSettings.AllowCalculationDiagnostic;
      if (calculationDiagnostic)
        EnConfigurationSettings.GlobalSettings.AllowCalculationDiagnostic = false;
      LoanData loanData = new LoanData(this.loan.XmlDocClone, this.loan.Settings, false, true);
      LoanCalculator loanCalculator = new LoanCalculator(this.sessionObjects, this.calObjs.LoanConfiguration, loanData, true, this.calObjs.ExternalLateFeeSettings, true);
      loanData.Calculator.PerformanceEnabled = false;
      loanData.Calculator.UseWorstCaseScenario = true;
      loanData.Calculator.UseBestCaseScenario = false;
      string str1 = this.Val("1172");
      loanData.SetCurrentField("1765", "");
      if (str1 == "FarmersHomeAdministration")
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
      PaymentScheduleSnapshot paymentSchedule = loanData.Calculator.GetPaymentSchedule(false);
      if (calculationDiagnostic)
        EnConfigurationSettings.GlobalSettings.AllowCalculationDiagnostic = calculationDiagnostic;
      if (paymentSchedule == null || paymentSchedule.MonthlyPayments == null || paymentSchedule.MonthlyPayments.Length == 0)
      {
        this.SetVal("1896", "");
        this.SetVal("1957", "1st");
      }
      else
      {
        double num4 = 0.0;
        double val1 = 0.0;
        int num5 = 0;
        for (int index = 0; index < paymentSchedule.MonthlyPayments.Length && paymentSchedule.MonthlyPayments[index] != null; ++index)
        {
          if (paymentSchedule.MonthlyPayments[index].CurrentRate > num4)
          {
            num5 = index + 1;
            num4 = paymentSchedule.MonthlyPayments[index].CurrentRate;
            val1 = paymentSchedule.MonthlyPayments[index].Payment;
          }
        }
        this.SetCurrentNum("1896", Utils.ArithmeticRounding(val1, 2));
        int num6 = num5 % 12;
        int num7 = num6 <= 0 ? num5 / 12 : (num5 - num6) / 12 + 1;
        int num8 = num7 % 10;
        string str2 = "th";
        if (num7 < 11 || num7 > 13)
        {
          switch (num8)
          {
            case 1:
              str2 = "st";
              break;
            case 2:
              str2 = "nd";
              break;
            case 3:
              str2 = "rd";
              break;
          }
        }
        if (num7 < 1)
          this.SetVal("1957", "");
        else
          this.SetVal("1957", num7.ToString() + str2);
      }
    }

    private void calculatePMIMidpointCancelationDate(string id, string val)
    {
      int num1 = this.IntVal("4");
      int num2 = this.IntVal("325");
      DateTime dateTime1 = Utils.ParseDate((object) this.Val("682"));
      Decimal val1 = (Decimal) (num2 != 0 ? num2 : num1);
      string str1 = this.Val("19");
      DateTime date = Utils.ParseDate((object) this.Val("HUD68"));
      Decimal months = (Decimal) Utils.ArithmeticRoundingUp((int) val1, 2);
      if (months == 0M || dateTime1 == DateTime.MinValue || str1 == "ConstructionToPermanent" && date == DateTime.MinValue)
      {
        this.SetVal("3548", "");
      }
      else
      {
        if (str1 == "ConstructionToPermanent")
        {
          dateTime1 = date;
          string str2 = this.Val("HUD69");
          if (str2 == "FirstPaymentDate" && this.Val("CONST.X1") != "Y")
            months = (Decimal) Utils.ArithmeticRoundingUp((num2 != 0 ? num2 : num1) + this.IntVal("1176"), 2);
          else if (str2 == "FirstAmortDate" && this.Val("CONST.X1") == "Y")
            months = (Decimal) Utils.ArithmeticRoundingUp((num2 != 0 ? num2 : num1) - this.IntVal("1176"), 2);
        }
        else if (this.Val("423") == "Biweekly")
        {
          if (this.paySchedule == null || this.paySchedule[Utils.ArithmeticRoundingUp(this.IntVal("1701"), 2)] == null)
            return;
          this.SetVal("3548", this.paySchedule[Utils.ArithmeticRoundingUp(this.IntVal("1701"), 2)].PayDate);
          return;
        }
        DateTime dateTime2 = dateTime1.AddMonths((int) months);
        dateTime2 = new DateTime(dateTime2.Year, dateTime2.Month, 1);
        this.SetVal("3548", dateTime2.ToString("MM/dd/yyyy"));
      }
    }

    public RegzSummaryTableType RegzSummaryType
    {
      get
      {
        string str1 = this.Val("3292");
        string str2 = this.Val("745");
        DateTime date = Utils.ParseDate((object) "01/30/2011");
        DateTime dateTime = DateTime.MinValue;
        if (str1 != "" && str1 != "//")
          dateTime = Utils.ParseDate((object) str1);
        if (dateTime == DateTime.MinValue && str2 != "" && str2 != "//")
          dateTime = Utils.ParseDate((object) str2);
        if (dateTime != DateTime.MinValue && date.Date.CompareTo(dateTime.Date) > 0)
          return RegzSummaryTableType.None;
        string str3 = this.Val("19");
        string str4 = this.Val("608");
        int num1 = this.IntVal("1177");
        int num2 = this.IntVal("4");
        int num3 = this.IntVal("325");
        if (str3 == "ConstructionToPermanent" && this.Val("SYS.X6") == "B")
          return RegzSummaryTableType.InvalidPermB;
        if (this.Val("2551") != string.Empty || this.Val("2552") != string.Empty)
          return RegzSummaryTableType.InvalidNegARM;
        if ((str3 == "ConstructionToPermanent" || str3 == "ConstructionOnly") && num2 > num3 && num1 > 0 && num2 > 0)
          return RegzSummaryTableType.InvalidConstIO;
        if (str3 == "ConstructionOnly")
          return this.Val("SYS.X6") == "B" ? RegzSummaryTableType.ConstOnlyB : RegzSummaryTableType.ConstOnlyA;
        switch (str4)
        {
          case "Fixed":
            if ((this.Val("CASASRN.X141") == "Borrower" || this.Val("COMPLIANCEVERSION.CASASRNX141") == "Y" || this.Val("COMPLIANCEVERSION.NEWBUYDOWNENABLED") != "Y") && this.Val("1269") != string.Empty && this.Val("1613") != string.Empty)
              return RegzSummaryTableType.Buydown;
            if (num1 == 0 && (num3 == 0 || num3 == num2))
              return RegzSummaryTableType.Fixed;
            if (num1 == 0 && num3 > 0)
              return RegzSummaryTableType.FixedBalloon;
            if (num1 > 0 && (num3 == 0 || num3 == num2))
              return RegzSummaryTableType.FixedIntOnly;
            if (num1 > 0 && num1 > num3)
              return RegzSummaryTableType.FixedBalloonIntOnlyGreater;
            return num1 > 0 && num1 <= num3 ? RegzSummaryTableType.FixedBalloonIntOnlyLesser : RegzSummaryTableType.Fixed;
          case "AdjustableRate":
            int num4 = this.IntVal("696");
            if (num4 == 36 && num1 == 0 && this.FltVal("697") < this.FltVal("247"))
              return RegzSummaryTableType.ARMLess5Years;
            if (num4 == 36 && num1 == 36 && this.FltVal("697") < this.FltVal("247"))
              return RegzSummaryTableType.ARMIntOnly;
            if (num4 == 60 && num1 == 0 && this.FltVal("697") < this.FltVal("247"))
              return RegzSummaryTableType.ARMLess5Years;
            if (num4 == 60 && num1 == 60 && this.FltVal("697") < this.FltVal("247"))
              return RegzSummaryTableType.ARMIO_L60;
            if (num4 == 60 && num1 == 0 && this.FltVal("697") == this.FltVal("247"))
              return RegzSummaryTableType.ARMLess5Years;
            if (num4 == 60 && num1 == 60 && this.FltVal("697") == this.FltVal("247") || num4 == 60 && num1 == 120 && this.FltVal("697") == this.FltVal("247"))
              return RegzSummaryTableType.ARMIntOnly51;
            if (num4 == 84 && num1 == 0 && this.FltVal("697") < this.FltVal("247") || num4 == 84 && num1 == 0 && this.FltVal("697") == this.FltVal("247"))
              return RegzSummaryTableType.ARMLess5Years;
            if (num4 == 84 && num1 == 84 && this.FltVal("697") < this.FltVal("247"))
              return RegzSummaryTableType.ARMIO_L60;
            if (num4 == 84 && num1 == 84 && this.FltVal("697") == this.FltVal("247") || num4 == 84 && num1 > num4 && this.FltVal("697") == this.FltVal("247") || num4 == 120 && num1 == num4 && this.FltVal("697") == this.FltVal("247"))
              return RegzSummaryTableType.ARMIntOnly51;
            if (num4 == 120 && num1 > 0 && num1 < num4 && this.FltVal("697") < this.FltVal("247"))
              return RegzSummaryTableType.ARMIO_L60;
            if (num4 == 120 && num1 == 0 && this.FltVal("697") <= this.FltVal("247"))
              return RegzSummaryTableType.ARMLess5Years;
            if (num1 > 60 && num1 >= num4 && this.FltVal("697") == this.FltVal("247"))
              return RegzSummaryTableType.ARMIntOnly51;
            if (num1 > 0 && num1 < 60 && num1 >= num4 && this.FltVal("697") == this.FltVal("247"))
              return RegzSummaryTableType.ARMIntOnly;
            if (num1 == 60 && num1 >= num4 && this.FltVal("697") == this.FltVal("247"))
              return RegzSummaryTableType.ARMIntOnly51;
            if (num1 > 0 && num1 >= num4 && this.FltVal("697") == this.FltVal("247"))
              return RegzSummaryTableType.ARMIntOnly3C;
            if (num4 == 36 && num1 > 0 && num1 > num4 && this.FltVal("697") < this.FltVal("247"))
              return RegzSummaryTableType.ARMIntOnly31;
            if (num4 == 60 && num1 == 60 && this.FltVal("697") < this.FltVal("247"))
              return RegzSummaryTableType.ARMIO_L60;
            if (num4 == 60 && num1 > 0 && num1 > num4 && this.FltVal("697") < this.FltVal("247"))
              return RegzSummaryTableType.ARMIntOnly51;
            if ((num4 == 84 || num4 == 120) && num1 > 0 && this.FltVal("697") < this.FltVal("247"))
              return RegzSummaryTableType.ARMIntOnly7_1or10_1;
            return num1 > 0 ? RegzSummaryTableType.ARMIntOnly : RegzSummaryTableType.ARMLess5Years;
          default:
            return RegzSummaryTableType.None;
        }
      }
    }

    private void calculateFirstAdjustmentMinimum(string id, string val)
    {
      double num1 = this.FltVal("3") - this.FltVal("697");
      double num2 = this.FltVal("1699");
      if (num1 > num2)
        this.SetCurrentNum("3557", num1);
      else
        this.SetCurrentNum("3557", num2);
    }

    private void calculateLateFeePaymentInRegz(string id, string val)
    {
      double num1 = this.Val("420") == "FirstLien" ? this.FltVal("5") + this.FltVal("230") + this.FltVal("1405") : this.FltVal("5");
      double overduePayment = this.FltVal("5");
      double interest = this.FltVal("3") / 100.0 * this.FltVal("2");
      double principalAndInterest = this.FltVal("5");
      if (this.paySchedule != null && this.paySchedule.Length != 0 && this.paySchedule[0] != null)
      {
        interest = this.paySchedule[0].Interest;
        principalAndInterest = this.paySchedule[0].Interest + this.paySchedule[0].Principal;
      }
      this.SetCurrentNum("4186", num1);
      double num2 = this.calObjs.ToolCal.CalcLateCharge(num1, overduePayment, interest, principalAndInterest, this.FltVal("674"), this.Val("1719"), this.FltVal("2831"), this.FltVal("2832"));
      if (num2 != 0.0)
      {
        this.SetCurrentNum("3876", num2);
        this.SetCurrentNum("3877", this.FltVal("5") + num2);
      }
      else
      {
        this.SetVal("3876", "");
        this.SetVal("3877", "");
      }
    }

    private void calculateLateExemptRightRescissionFlag(string id, string val)
    {
      string str1 = this.Val("19");
      string str2 = this.Val("CONST.X2");
      switch (str1)
      {
        case "Purchase":
          this.SetVal("3942", "");
          break;
        case "ConstructionOnly":
        case "ConstructionToPermanent":
          if (!(str2 != "Y"))
            break;
          goto case "Purchase";
      }
      if (this.Val("3942") == "Y")
      {
        this.SetVal("L724", "");
        this.SetVal("4907", "");
        this.SetVal("4908", "");
        this.SetVal("4909", "");
        this.SetVal("4910", "");
        this.SetVal("4911", "");
      }
      this.loan.Calculator.CalculateLatestDisclosure2015((IDisclosureTracking2015Log) null);
    }

    private void calcOnDemand(string id, string val)
    {
      this.calObjs.RegzCal.CalcPaymentSchedule((string) null, (string) null);
      this.loan.Calculator.EditCalcOnDemandEnum(CalcOnDemandEnum.PaymentSchedule, false);
      this.calObjs.NewHud2015FeeDetailCal.Update2010ItemizationFrom2015Itemization((string) null, (string) null);
      this.loan.Calculator.EditCalcOnDemandEnum(CalcOnDemandEnum.Update2010ItemizationFrom2015Itemization, false);
      this.SetVal("CALCREQUIRED", "");
    }

    private void calculateConstructionPhaseDisclosedSeparately(string id, string val)
    {
      if (!(this.Val("19") != "ConstructionOnly"))
        return;
      this.SetVal("4084", "");
    }

    private void calculateInitialInterestRate(string id, string val)
    {
      if (this.Val("19") == "ConstructionToPermanent")
        this.SetCurrentNum("4113", this.FltVal("1677"));
      else
        this.SetCurrentNum("4113", this.FltVal("3"));
      this.calObjs.NewHud2015Cal.CalcLECDDisplayFields("4113", (string) null);
    }

    private void calculateLotLandStatus(string id, string val)
    {
      string str = this.Val("19");
      if (str == "ConstructionOnly" || str == "ConstructionToPermanent")
      {
        if (id == "1964" && val == "Y" && this.Val("Constr.Refi") == "Y")
          this.SetVal("Constr.Refi", "N");
        else if (id == "Constr.Refi" && val == "Y" && this.Val("1964") == "Y")
          this.SetVal("1964", "N");
        else if (id == "19" && this.Val("1964") == "Y" && this.Val("Constr.Refi") == "Y")
        {
          this.SetVal("1964", "Y");
          this.SetVal("Constr.Refi", "N");
        }
      }
      this.SetVal("4254", this.Val("1964"));
      this.SetVal("4255", this.Val("Constr.Refi"));
    }

    private void sychronizeConstructionRateNoteRate(string id, string val)
    {
      if (!(this.Val("19") == "ConstructionOnly"))
        return;
      switch (id)
      {
        case "1677":
          if (this.FltVal("1677") > 100.0)
            this.SetCurrentNum("1677", 100.0);
          this.SetCurrentNum("3", this.FltVal("1677"));
          this.SetVal("KBYO.XD3", Utils.RemoveEndingZeros(this.FltVal("3").ToString()));
          break;
        case "3":
        case "19":
          if (this.FltVal("3") > 100.0)
          {
            this.SetCurrentNum("3", 100.0);
            this.SetVal("KBYO.XD3", "100");
          }
          this.SetCurrentNum("1677", this.FltVal("3"));
          break;
      }
    }

    private void calculateBuydownIndicator(string id, string val)
    {
      string str = this.Val("CASASRN.X141");
      if (id == "425")
        this.SetVal("ULDD.X181", this.Val("425"));
      else if ((id == "1269" || id == "CASASRN.X141") && str == "Borrower")
      {
        this.SetVal("425", this.FltVal("1269") != 0.0 ? "Y" : "N");
        this.SetVal("ULDD.X181", this.Val("425"));
      }
      else if ((id == "4535" || id == "CASASRN.X141") && str != "Borrower")
      {
        this.SetVal("425", this.FltVal("4535") != 0.0 ? "Y" : "N");
        this.SetVal("ULDD.X181", this.Val("425"));
      }
      else
      {
        if (!(id == "CASASRN.X141"))
          return;
        this.SetVal("425", "N");
        this.SetVal("ULDD.X181", this.Val("425"));
      }
    }

    private void cleanUpBuydownFields(string id, string val)
    {
      bool flag = this.Val("COMPLIANCEVERSION.NEWBUYDOWNENABLED") == "Y";
      if (!flag)
        return;
      if (((!(id == "CASASRN.X141") ? 0 : (this.Val("COMPLIANCEVERSION.CASASRNX141") == "Y" ? 1 : 0)) & (flag ? 1 : 0)) != 0)
        this.SetVal("COMPLIANCEVERSION.CASASRNX141", "");
      if (this.Val("COMPLIANCEVERSION.CASASRNX141") == "Y")
        return;
      string str = this.Val("CASASRN.X141");
      string empty = string.Empty;
      this.SetVal("ULDD.X137", str != null && str.Length == 0 || str == "Lender" || str == "Borrower" ? str : "Other");
      if (str == "")
        this.SetVal("4645", "");
      if (str == "Borrower")
      {
        for (int index = 4535; index <= 4546; ++index)
          this.SetVal(string.Concat((object) index), "");
      }
      else
      {
        for (int index = 1269; index <= 1274; ++index)
        {
          if (this.IsLocked(string.Concat((object) index)))
            this.RemoveCurrentLock(string.Concat((object) index));
          this.SetVal(string.Concat((object) index), "");
        }
        for (int index = 1613; index <= 1618; ++index)
        {
          if (this.IsLocked(string.Concat((object) index)))
            this.RemoveCurrentLock(string.Concat((object) index));
          this.SetVal(string.Concat((object) index), "");
        }
      }
    }

    private void setFieldValueToCache(string id, string val)
    {
      if (!this.cacheFieldValues.ContainsKey(id))
        this.cacheFieldValues.Add(id, val);
      else
        this.cacheFieldValues[id] = val;
    }

    private string getFieldValueFromCache(string id)
    {
      return this.IsLocked(id) || !this.cacheFieldValues.ContainsKey(id) ? this.Val(id) : this.cacheFieldValues[id];
    }

    private void setCacheValuesToLoan()
    {
      if (this.cacheFieldValues == null)
        return;
      foreach (KeyValuePair<string, string> cacheFieldValue in this.cacheFieldValues)
      {
        if (!(cacheFieldValue.Key == "CalcNetTangibleBenefit") && !(cacheFieldValue.Key == "CalcPrepaymentPenaltyPeriod"))
        {
          string fieldValueFromCache = this.getFieldValueFromCache(cacheFieldValue.Key);
          if (this.Val(cacheFieldValue.Key) != fieldValueFromCache)
          {
            if (cacheFieldValue.Key == "4085")
              this.SetField4085(fieldValueFromCache);
            else
              this.SetVal(cacheFieldValue.Key, fieldValueFromCache);
          }
        }
      }
      if (this.getFieldValueFromCache("CalcNetTangibleBenefit") == null)
        return;
      string fieldValueFromCache1 = this.getFieldValueFromCache("CalcNetTangibleBenefit");
      this.calObjs.ToolCal.CalcNetTangibleBenefit(fieldValueFromCache1, fieldValueFromCache1 == "1659" ? "N" : this.Val("5"));
      this.calObjs.HMDACal.CalcPrepaymentPenaltyPeriod((string) null, (string) null);
    }

    private void calculateLenderRepresentative(string id, string val)
    {
      string str = this.Val(this.getLenderRepRoleId(id));
      string val1 = "";
      string val2 = "";
      string val3 = "";
      string val4 = "";
      string val5 = "";
      string val6 = "";
      string val7 = "";
      string[] lenderRepFields = this.getLenderRepFields(id);
      if (str == "None" || str == "")
      {
        if (lenderRepFields == null)
          return;
        this.SetVal(lenderRepFields[1], "");
        this.SetVal(lenderRepFields[0], "");
        this.SetVal(lenderRepFields[4], "");
        this.SetVal(lenderRepFields[3], "");
        this.SetVal(lenderRepFields[6], "");
        this.SetVal(lenderRepFields[2], "");
        this.SetVal(lenderRepFields[5], "");
      }
      else
      {
        if (!str.StartsWith("CUSTOM"))
        {
          string lenderRepRoleId = this.getLenderRepRoleId(id);
          if (this.setLenderRepFieldsFromMilestone(false, lenderRepRoleId, this.Val(lenderRepRoleId)))
            return;
        }
        else if (str.StartsWith("CUSTOM"))
        {
          switch (str)
          {
            case "CUSTOM1":
              val1 = this.Val("VEND.X63");
              val2 = this.Val("VEND.X55");
              val3 = this.Val("VEND.X509");
              val4 = this.Val("VEND.X61");
              val6 = this.Val("VEND.X1049");
              val7 = "Custom1";
              break;
            case "CUSTOM2":
              val1 = this.Val("VEND.X73");
              val2 = this.Val("VEND.X65");
              val3 = this.Val("VEND.X510");
              val4 = this.Val("VEND.X71");
              val6 = this.Val("VEND.X1050");
              val7 = "Custom2";
              break;
            case "CUSTOM3":
              val1 = this.Val("VEND.X83");
              val2 = this.Val("VEND.X75");
              val3 = this.Val("VEND.X511");
              val4 = this.Val("VEND.X81");
              val6 = this.Val("VEND.X1051");
              val7 = "Custom3";
              break;
            case "CUSTOM4":
              val1 = this.Val("VEND.X10");
              val2 = this.Val("VEND.X2");
              val3 = this.Val("VEND.X512");
              val4 = this.Val("VEND.X8");
              val6 = this.Val("VEND.X1052");
              val7 = "Custom4";
              break;
          }
        }
        if (this.Val(lenderRepFields[7]) == "Y")
          return;
        this.SetVal(lenderRepFields[1], val1);
        this.SetVal(lenderRepFields[0], val2);
        this.SetVal(lenderRepFields[4], val3);
        this.SetVal(lenderRepFields[3], val4);
        this.SetVal(lenderRepFields[6], val5);
        this.SetVal(lenderRepFields[2], val6);
        this.SetVal(lenderRepFields[5], val7);
      }
    }

    private bool setLenderRepFieldsFromMilestone(
      bool checkUserId,
      string roleIdField,
      string roleIdValue)
    {
      int result = -1;
      int.TryParse(roleIdValue, out result);
      string val1 = "";
      string val2 = "";
      string val3 = "";
      string val4 = "";
      string val5 = "";
      string val6 = "";
      string val7 = "";
      bool flag = false;
      int.TryParse(roleIdValue, out result);
      string str = "";
      string[] lenderRepFields = this.getLenderRepFields(roleIdField);
      if (lenderRepFields != null)
        str = this.Val(lenderRepFields[6]);
      MilestoneLog currentMilestone1 = this.loan.GetLogList().GetCurrentMilestone();
      if (currentMilestone1 != null && currentMilestone1.RoleID == result)
      {
        if (checkUserId && currentMilestone1.LoanAssociateID == str || !checkUserId)
        {
          val1 = currentMilestone1.LoanAssociateEmail;
          val2 = currentMilestone1.LoanAssociateName;
          val3 = currentMilestone1.LoanAssociateCellPhone;
          val4 = currentMilestone1.LoanAssociatePhone;
          if (!checkUserId)
            val7 = currentMilestone1.LoanAssociateID;
          val5 = currentMilestone1.LoanAssociateTitle;
          val6 = currentMilestone1.LoanAssociateType.ToString();
          flag = true;
        }
      }
      else
      {
        MilestoneLog currentMilestone2 = this.loan.GetLogList().GetCurrentMilestone();
        int num = this.loan.GetLogList().GetNumberOfMilestones();
        if (currentMilestone2 != null)
          num = this.loan.GetLogList().GetMilestoneIndex(currentMilestone2.Stage);
        LoanAssociateType loanAssociateType;
        for (int i = num - 1; i >= 0; --i)
        {
          MilestoneLog milestoneAt = this.loan.GetLogList().GetMilestoneAt(i);
          if (milestoneAt != null && milestoneAt.RoleID == result && (checkUserId && milestoneAt.LoanAssociateID == str || !checkUserId))
          {
            val1 = milestoneAt.LoanAssociateEmail;
            val2 = milestoneAt.LoanAssociateName;
            val3 = milestoneAt.LoanAssociateCellPhone;
            val4 = milestoneAt.LoanAssociatePhone;
            val7 = milestoneAt.LoanAssociateID;
            val5 = milestoneAt.LoanAssociateTitle;
            loanAssociateType = milestoneAt.LoanAssociateType;
            val6 = loanAssociateType.ToString();
            flag = true;
            break;
          }
        }
        if (!flag)
        {
          for (int i = num + 1; i < this.loan.GetLogList().GetNumberOfMilestones(); ++i)
          {
            MilestoneLog milestoneAt = this.loan.GetLogList().GetMilestoneAt(i);
            if (milestoneAt != null && milestoneAt.RoleID == result && (checkUserId && milestoneAt.LoanAssociateID == str || !checkUserId))
            {
              val1 = milestoneAt.LoanAssociateEmail;
              val2 = milestoneAt.LoanAssociateName;
              val3 = milestoneAt.LoanAssociateCellPhone;
              val4 = milestoneAt.LoanAssociatePhone;
              val7 = milestoneAt.LoanAssociateID;
              val5 = milestoneAt.LoanAssociateTitle;
              loanAssociateType = milestoneAt.LoanAssociateType;
              val6 = loanAssociateType.ToString();
              flag = true;
              break;
            }
          }
        }
        if (!flag)
        {
          foreach (MilestoneFreeRoleLog milestoneFreeRole in this.loan.GetLogList().GetAllMilestoneFreeRoles())
          {
            if (milestoneFreeRole.RoleID == result && (checkUserId && milestoneFreeRole.LoanAssociateID == str || !checkUserId))
            {
              val1 = milestoneFreeRole.LoanAssociateEmail;
              val2 = milestoneFreeRole.LoanAssociateName;
              val3 = milestoneFreeRole.LoanAssociateCellPhone;
              val4 = milestoneFreeRole.LoanAssociatePhone;
              val7 = milestoneFreeRole.LoanAssociateID;
              val5 = milestoneFreeRole.LoanAssociateTitle;
              loanAssociateType = milestoneFreeRole.LoanAssociateType;
              val6 = loanAssociateType.ToString();
              flag = true;
              break;
            }
          }
        }
      }
      if (flag && this.Val(lenderRepFields[7]) != "Y")
      {
        this.SetVal(lenderRepFields[1], val1);
        this.SetVal(lenderRepFields[0], val2);
        this.SetVal(lenderRepFields[4], val3);
        this.SetVal(lenderRepFields[3], val4);
        this.SetVal(lenderRepFields[6], val7);
        this.SetVal(lenderRepFields[2], val5);
        this.SetVal(lenderRepFields[5], val6);
      }
      return flag;
    }

    private void updateLenderRepresentative(string id, string val)
    {
      string id1 = "";
      if (id.StartsWith("VEND"))
      {
        for (int index = 0; index < 8; ++index)
        {
          if (index == 0)
            id1 = "4675";
          else if (index == 1)
            id1 = "4841";
          else if (index == 2)
            id1 = "4842";
          else if (index == 3)
            id1 = "4843";
          else if (index == 4)
            id1 = "4844";
          else if (index == 5)
            id1 = "4845";
          else if (index == 6)
            id1 = "4846";
          else if (index == 7)
            id1 = "4847";
          string[] lenderRepFields = this.getLenderRepFields(id1);
          if (!(this.Val(lenderRepFields[7]) == "Y"))
          {
            id1 = this.Val(id1);
            if (id1.StartsWith("CUSTOM"))
            {
              switch (id1)
              {
                case "CUSTOM1":
                  switch (id)
                  {
                    case "VEND.X55":
                      this.SetVal(lenderRepFields[0], val);
                      continue;
                    case "VEND.X63":
                      this.SetVal(lenderRepFields[1], val);
                      continue;
                    case "VEND.X509":
                      this.SetVal(lenderRepFields[4], val);
                      continue;
                    case "VEND.X61":
                      this.SetVal(lenderRepFields[3], val);
                      continue;
                    case "VEND.X1049":
                      this.SetVal(lenderRepFields[2], val);
                      continue;
                    default:
                      continue;
                  }
                case "CUSTOM2":
                  switch (id)
                  {
                    case "VEND.X65":
                      this.SetVal(lenderRepFields[0], val);
                      continue;
                    case "VEND.X73":
                      this.SetVal(lenderRepFields[1], val);
                      continue;
                    case "VEND.X510":
                      this.SetVal(lenderRepFields[4], val);
                      continue;
                    case "VEND.X71":
                      this.SetVal(lenderRepFields[3], val);
                      continue;
                    case "VEND.X1050":
                      this.SetVal(lenderRepFields[2], val);
                      continue;
                    default:
                      continue;
                  }
                case "CUSTOM3":
                  switch (id)
                  {
                    case "VEND.X75":
                      this.SetVal(lenderRepFields[0], val);
                      continue;
                    case "VEND.X83":
                      this.SetVal(lenderRepFields[1], val);
                      continue;
                    case "VEND.X511":
                      this.SetVal(lenderRepFields[4], val);
                      continue;
                    case "VEND.X81":
                      this.SetVal(lenderRepFields[3], val);
                      continue;
                    case "VEND.X1051":
                      this.SetVal(lenderRepFields[2], val);
                      continue;
                    default:
                      continue;
                  }
                case "CUSTOM4":
                  switch (id)
                  {
                    case "VEND.X2":
                      this.SetVal(lenderRepFields[0], val);
                      continue;
                    case "VEND.X10":
                      this.SetVal(lenderRepFields[1], val);
                      continue;
                    case "VEND.X512":
                      this.SetVal(lenderRepFields[4], val);
                      continue;
                    case "VEND.X8":
                      this.SetVal(lenderRepFields[3], val);
                      continue;
                    case "VEND.X1052":
                      this.SetVal(lenderRepFields[2], val);
                      continue;
                    default:
                      continue;
                  }
                default:
                  continue;
              }
            }
          }
        }
      }
      else
      {
        if (!id.StartsWith("email") && !id.StartsWith("name") && !id.StartsWith("cell") && !id.StartsWith("work") && !id.StartsWith(nameof (id)) && !id.StartsWith("title") && !id.StartsWith("userType"))
          return;
        string[] strArray = id.Split('|');
        string[] lenderRepFields = this.getLenderRepFields(strArray[1]);
        if (this.Val(lenderRepFields[7]) == "Y")
          return;
        id = strArray[0];
        if (id == "email")
          this.SetVal(lenderRepFields[1], val);
        if (id == "name")
          this.SetVal(lenderRepFields[0], val);
        if (id == "cell")
          this.SetVal(lenderRepFields[4], val);
        if (id == "work")
          this.SetVal(lenderRepFields[3], val);
        if (id == nameof (id))
          this.SetVal(lenderRepFields[6], val);
        if (id == "title")
          this.SetVal(lenderRepFields[2], val);
        if (!(id == "userType"))
          return;
        this.SetVal(lenderRepFields[5], val);
      }
    }

    private void setLenderRepFromUserId(string id, string val)
    {
      string lenderRepRoleId = this.getLenderRepRoleId(id);
      string roleIdValue = this.Val(lenderRepRoleId);
      if (string.IsNullOrEmpty(roleIdValue) || roleIdValue.Equals("None") || roleIdValue.StartsWith("CUSTOM"))
      {
        this.SetVal(id, "");
      }
      else
      {
        if (!(id == "4682") && !(id == "4804") && !(id == "4807") && !(id == "4810") && !(id == "4813") && !(id == "4816") && !(id == "4821") && !(id == "4827"))
          return;
        bool flag = this.setLenderRepFieldsFromMilestone(true, lenderRepRoleId, roleIdValue);
        if (!flag)
        {
          UserInfo user = this.sessionObjects.OrganizationManager.GetUser(val);
          if (user != (UserInfo) null)
          {
            foreach (UserInfo userInfo in this.sessionObjects.OrganizationManager.GetUsersWithRole(Convert.ToInt32(roleIdValue)))
            {
              if (userInfo.Userid == user.Userid)
              {
                string[] lenderRepFields = this.getLenderRepFields(id);
                if (lenderRepFields == null || this.Val(lenderRepFields[7]) == "Y")
                  return;
                this.SetVal(lenderRepFields[1], user.Email);
                this.SetVal(lenderRepFields[0], user.FullName);
                this.SetVal(lenderRepFields[4], user.CellPhone);
                this.SetVal(lenderRepFields[3], user.Phone);
                this.SetVal(lenderRepFields[2], user.JobTitle);
                this.SetVal(lenderRepFields[5], "User");
                return;
              }
            }
          }
        }
        if (flag)
          return;
        this.SetVal(id, "");
      }
    }

    private void calculateRoleName(string id, string val)
    {
      string str = val;
      if (val.StartsWith("Role"))
        str = val.Substring(7);
      string val1 = "";
      bool flag = false;
      string id1 = "";
      foreach (RoleInfo allRole in this.configInfo.AllRoles)
      {
        if (str == allRole.RoleName)
        {
          val1 = allRole.RoleID.ToString();
          flag = true;
          break;
        }
      }
      if (!flag)
      {
        switch (str)
        {
          case "":
            val1 = "";
            break;
          case "Custom Category #1":
            val1 = "CUSTOM1";
            break;
          default:
            if (!(str == this.Val("VEND.X84")))
            {
              if (str == "Custom Category #2" || str == this.Val("VEND.X85"))
              {
                val1 = "CUSTOM2";
                break;
              }
              if (str == "Custom Category #3" || str == this.Val("VEND.X86"))
              {
                val1 = "CUSTOM3";
                break;
              }
              if (str == "Custom Category #4" || str == this.Val("VEND.X11"))
              {
                val1 = "CUSTOM4";
                break;
              }
              break;
            }
            goto case "Custom Category #1";
        }
        if (str == "None")
          val1 = "None";
      }
      switch (id)
      {
        case "4672":
          id1 = "4675";
          break;
        case "4802":
          id1 = "4841";
          break;
        case "4806":
          id1 = "4842";
          break;
        case "4809":
          id1 = "4843";
          break;
        case "4811":
          id1 = "4844";
          break;
        case "4814":
          id1 = "4845";
          break;
        case "4818":
          id1 = "4846";
          break;
        case "4824":
          id1 = "4847";
          break;
      }
      this.SetVal(id1, val1);
    }

    private void calculateMersOriginalMortgagee(string id, string val)
    {
      if (!string.IsNullOrEmpty(val))
        this.SetVal("4723", "Y");
      else
        this.SetVal("4723", "N");
    }

    private void calculatePointsInInitialAdjustedRate(string id, string val)
    {
      string empty = string.Empty;
      double num1 = this.FltVal("2");
      double num2 = this.FltVal("NEWHUD.X1721");
      double num3 = this.FltVal("NEWHUD.X1722");
      if (num1 > 0.0 && (num2 > 0.0 || num3 > 0.0))
        empty = (num3 * 100.0 / num1 + num2).ToString();
      this.SetVal("CORRESPONDENT.X505", empty);
    }

    private string getLenderRepRoleId(string id)
    {
      string lenderRepRoleId = "";
      switch (id)
      {
        case "4672":
        case "4682":
          lenderRepRoleId = "4675";
          break;
        case "4802":
        case "4804":
          lenderRepRoleId = "4841";
          break;
        case "4806":
        case "4807":
          lenderRepRoleId = "4842";
          break;
        case "4809":
        case "4810":
          lenderRepRoleId = "4843";
          break;
        case "4811":
        case "4813":
          lenderRepRoleId = "4844";
          break;
        case "4814":
        case "4816":
          lenderRepRoleId = "4845";
          break;
        case "4818":
        case "4821":
          lenderRepRoleId = "4846";
          break;
        case "4824":
        case "4827":
          lenderRepRoleId = "4847";
          break;
      }
      return lenderRepRoleId;
    }

    public string[] getLenderRepFields(string id)
    {
      string[] lenderRepFields1 = new string[8]
      {
        "4673",
        "4674",
        "4683",
        "4676",
        "4677",
        "4684",
        "4682",
        "4840"
      };
      string[] lenderRepFields2 = new string[8]
      {
        "1612",
        "3968",
        "4803",
        "1823",
        "4805",
        "4848",
        "4804",
        "4831"
      };
      string[] lenderRepFields3 = new string[8]
      {
        "1256",
        "95",
        "Lender.CntctTitle",
        "1262",
        "4808",
        "4849",
        "4807",
        "4832"
      };
      string[] lenderRepFields4 = new string[8]
      {
        "VEND.X302",
        "VEND.X305",
        "Broker.CntctTitle",
        "VEND.X303",
        "VEND.X304",
        "4850",
        "4810",
        "4833"
      };
      string[] lenderRepFields5 = new string[8]
      {
        "USDA.X31",
        "4812",
        "USDA.X30",
        "4838",
        "4839",
        "4851",
        "4813",
        "4834"
      };
      string[] lenderRepFields6 = new string[8]
      {
        "1754",
        "4815",
        "1755",
        "1756",
        "4817",
        "4852",
        "4816",
        "4835"
      };
      string[] lenderRepFields7 = new string[8]
      {
        "3194",
        "4819",
        "4820",
        "4822",
        "4823",
        "4853",
        "4821",
        "4836"
      };
      string[] lenderRepFields8 = new string[8]
      {
        "NOTICES.X31",
        "4825",
        "4826",
        "NOTICES.X37",
        "4829",
        "4854",
        "4827",
        "4837"
      };
      switch (id)
      {
        case "4682":
        case "4675":
        case "4672":
          return lenderRepFields1;
        case "4804":
        case "4841":
        case "4802":
          return lenderRepFields2;
        case "4807":
        case "4842":
        case "4806":
          return lenderRepFields3;
        case "4810":
        case "4843":
        case "4809":
          return lenderRepFields4;
        case "4813":
        case "4844":
        case "4811":
          return lenderRepFields5;
        case "4816":
        case "4845":
        case "4814":
          return lenderRepFields6;
        case "4821":
        case "4846":
        case "4818":
          return lenderRepFields7;
        case "4827":
        case "4847":
        case "4824":
          return lenderRepFields8;
        default:
          return (string[]) null;
      }
    }

    private void cleareSignCustomizeIndicator(string id, string val)
    {
      string id1 = "";
      switch (id)
      {
        case "4672":
          id1 = "4840";
          break;
        case "4802":
          id1 = "4831";
          break;
        case "4806":
          id1 = "4832";
          break;
        case "4809":
          id1 = "4833";
          break;
        case "4811":
          id1 = "4834";
          break;
        case "4814":
          id1 = "4835";
          break;
        case "4818":
          id1 = "4836";
          break;
        case "4824":
          id1 = "4837";
          break;
      }
      this.SetVal(id1, "");
    }

    private void reCalculateLenderRep(string id, string val)
    {
      if (val != "N")
        return;
      string id1 = "";
      switch (id)
      {
        case "4840":
          id1 = "4672";
          break;
        case "4831":
          id1 = "4802";
          break;
        case "4832":
          id1 = "4806";
          break;
        case "4833":
          id1 = "4809";
          break;
        case "4834":
          id1 = "4811";
          break;
        case "4835":
          id1 = "4814";
          break;
        case "4836":
          id1 = "4818";
          break;
        case "4837":
          id1 = "4824";
          break;
      }
      this.calculateLenderRepresentative(id1, this.Val(id1));
    }

    private void seteSignerCuztomizeIndicator(string id, string val)
    {
      switch (id)
      {
        case "4673":
        case "4674":
        case "4683":
        case "4676":
        case "4677":
          string str1 = this.Val("4672");
          if (!(str1 != "") || !(str1 != "None"))
            break;
          this.SetVal("4840", "Y");
          break;
        case "1612":
        case "3968":
        case "4803":
        case "1823":
        case "4805":
          string str2 = this.Val("4802");
          if (!(str2 != "") || !(str2 != "None"))
            break;
          this.SetVal("4831", "Y");
          break;
        case "1256":
        case "95":
        case "Lender.CntctTitle":
        case "1262":
        case "4808":
          string str3 = this.Val("4806");
          if (!(str3 != "") || !(str3 != "None"))
            break;
          this.SetVal("4832", "Y");
          break;
        case "VEND.X302":
        case "VEND.X305":
        case "Broker.CntctTitle":
        case "VEND.X303":
        case "VEND.X304":
          string str4 = this.Val("4809");
          if (!(str4 != "") || !(str4 != "None"))
            break;
          this.SetVal("4833", "Y");
          break;
        case "USDA.X31":
        case "4812":
        case "USDA.X30":
        case "4838":
        case "4839":
          string str5 = this.Val("4811");
          if (!(str5 != "") || !(str5 != "None"))
            break;
          this.SetVal("4834", "Y");
          break;
        case "1754":
        case "4815":
        case "1755":
        case "1756":
        case "4817":
          string str6 = this.Val("4814");
          if (!(str6 != "") || !(str6 != "None"))
            break;
          this.SetVal("4835", "Y");
          break;
        case "3194":
        case "4819":
        case "4820":
        case "4822":
        case "4823":
          string str7 = this.Val("4818");
          if (!(str7 != "") || !(str7 != "None"))
            break;
          this.SetVal("4836", "Y");
          break;
        case "NOTICES.X31":
        case "4825":
        case "4826":
        case "NOTICES.X37":
        case "4829":
          string str8 = this.Val("4824");
          if (!(str8 != "") || !(str8 != "None"))
            break;
          this.SetVal("4837", "Y");
          break;
      }
    }

    private struct loanInformation
    {
      public double LoanRate;
      public string LoanType;
      public double BaseLoanAmount;
      public double LoanAmount;
      public int LoanTerm;
      public int BallonTerm;
      public int LoanPeriod;
      public double MonthlyPayment;
      public int InterestOnly;
      public double BuydownMonthlyPayment;
      public int TotalBuydownMonths;
      public bool IsBiWeekly;
      public string LoanPurpose;
      public int NumberofPayPerYear;
      public int DaysPerYearBiWeekly;
      public double BiweeklyPayment;
      public double RateFactorBiWeekly;
      public double RateFactor;
      public int YearsForGPM;
      public double RateForGPM;
      public string ARMType;
      public double ARMfirstAdjCap;
      public int ARMfirstChange;
      public double ARMadjCap;
      public int ARMadjPeriod;
      public double ARMlifeCap;
      public double ARMmargin;
      public double ARMindex;
      public double ARMfloor;
      public double ARMround;
      public string ARMisRound;
      public double NegAdjCap;
      public int NegAdjPeriod;
      public double NegAdjRecast;
      public int NegAdjStopAt;
      public double NegMaxBalance;
      public double NegPayment;
      public double FullyIndexedRate;
      public double RateCap;
      public double AdjustedFullyIndexedRate;
      public double MICutOffAmount;
      public int MImidpointCutoff;
      public bool MIUseRemainBalance;
      public string MIBasedOn;
      public double MIRatePMI;
      public double PMIAmout;
      public int TotalMonthMI;
      public int ConstPeriod;
      public double ConstIntRate;
      public double ConstReqRsv;
      public double ConstTotalFinCharge;
      public int mthPMIPrepaid;
      public double AmtCutOff80;
      public double AmtCutOff78;
      public string DateCutOff80;
      public string DateCutOff78;
      public double ExtraPayment;
      public bool DecliningRenewal;
      public string ConstInterestType;
      public string ConstMethod;
      public DateTime ConstEstClosingDate;
      public DateTime ConstFinalPayDate;
      public DateTime ConstFirstPayDate;
      public DateTime ConstSpanStartDate;
      public DateTime ConstFirstAmortDate;
      public DateTime DisbursementDate;
      public double ConstMonthlyPayment;
      public double ConstProjectedMonthlyPayment;
      public bool ConstUseLastDay;
      public bool ConstInPeriod;
      public double ConstPeriodicRate;
      public double ConstEstInterestReserve;
      public int LoanTermConstPlusPerm;
      public bool HelocWorstCase;
      public double HelocTeaserMargin;
      public double HelocTeaserRate;
      public double HelocMiniPayment;
      public int HelocDaysPerYear;
      public bool UsingSteadyOptionLoan;
      public double DiscountRate;
      public int DiscountPeriod;
      public double SteadyOptionInitialPayment;
      public bool IsARM;
      public bool TestOnly;
      public int year5Index;
      public bool IsConstPhaseDisclosedSeparately;
      public bool NeedToCalcEstConstIntrest;
      public bool UseSimpleInterest;
      public bool SimpleInterestUse366ForLeapYear;
      public DateTime FirstPaymentDate;
      public string ZeroPercentPaymentOption;
    }
  }
}
