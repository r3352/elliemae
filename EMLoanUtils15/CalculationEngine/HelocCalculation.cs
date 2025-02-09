// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.CalculationEngine.HelocCalculation
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.CalculationEngine
{
  internal class HelocCalculation : CalculationBase
  {
    private const string className = "HelocCalculation�";
    private static readonly string sw = Tracing.SwDataEngine;
    private SessionObjects sessionObjects;
    public const int Default_HELOC_loan_term_in_year = 15;
    public const double Default_HELOC_loan_amount = 10000.0;
    private HelocCalculation.HelocSchedule[] historicalSchedule;
    private HelocCalculation.HelocSchedule[] minimumSchedule;
    private HelocCalculation.HelocSchedule[] maximumSchedule;
    private HelocCalculation.HelocSchedule[] maximumScheduleOnFullLoanAmount;
    private HelocCalculation.InputsForScheduleCalc helocInfo;
    private string lastErrorFromExampleSchedules;
    internal Routine CalcResetDrawPeriodFields;
    internal Routine CalcHELOCSharedCalculations;
    internal Routine CalcHELOCPeriodicRates;
    internal Routine CalcClearCollectInterimInterest;

    public string GetLastErrorFromExampleSchedules() => this.lastErrorFromExampleSchedules;

    internal HelocCalculation(
      SessionObjects sessionObjects,
      LoanData l,
      EllieMae.EMLite.CalculationEngine.CalculationObjects calObjs)
      : base(l, calObjs)
    {
      this.sessionObjects = sessionObjects;
      this.CalcResetDrawPeriodFields = this.RoutineX(new Routine(this.resetDrawPeriodFields));
      this.CalcHELOCSharedCalculations = this.RoutineX(new Routine(this.calculateHELOCSharedCalculations));
      this.CalcHELOCPeriodicRates = this.RoutineX(new Routine(this.calcHELOCPeriodicRates));
      this.CalcClearCollectInterimInterest = this.RoutineX(new Routine(this.clearCollectInterimInterest));
      this.addFieldHandlers();
    }

    private void addFieldHandlers()
    {
      this.AddFieldHandler("4560", this.RoutineX(new Routine(this.resetDrawPeriodFields)));
      Routine routine = this.RoutineX(new Routine(this.calculateHELOCSharedCalculations));
      this.AddFieldHandler("4600", routine);
      this.AddFieldHandler("4603", routine);
      this.AddFieldHandler("4604", routine);
      this.AddFieldHandler("4612", routine);
      this.AddFieldHandler("4557", routine);
      this.AddFieldHandler("4616", routine);
      this.AddFieldHandler("4621", routine);
      this.AddFieldHandler("4586", routine);
      this.AddFieldHandler("1172", this.RoutineX(new Routine(this.clearCollectInterimInterest)));
    }

    private void resetDrawPeriodFields(string id, string val)
    {
      if (this.Val("1172") != "HELOC")
        return;
      if (this.Val("4560") == "Fraction of Balance")
        this.SetVal("HELOC.MinAdvPct", string.Empty);
      else if (this.Val("4560") == "Percentage of Balance")
      {
        this.SetVal("4564", string.Empty);
        this.SetVal("4565", string.Empty);
      }
      else
      {
        if (!(this.Val("4560") == string.Empty))
          return;
        this.SetVal("HELOC.MinAdvPct", string.Empty);
        this.SetVal("4564", string.Empty);
        this.SetVal("4565", string.Empty);
      }
    }

    private bool collectInputs()
    {
      this.helocInfo.firstPaymentDate = new DateTime(Utils.ParseDate((object) this.Val("363"), false, DateTime.Now.Date).Year, 1, 1);
      object policySetting = this.sessionObjects.StartupInfo.PolicySettings[(object) "Policies.HELOCTableLoanAmount"];
      this.helocInfo.loanAmount = policySetting == null ? 0.0 : Utils.ParseDouble(policySetting);
      if (this.helocInfo.loanAmount == 0.0)
        this.helocInfo.loanAmount = 10000.0;
      bool flag = this.Val("608") == "Fixed";
      this.helocInfo.loanState = this.Val("14");
      if (this.helocInfo.loanState == "FL")
      {
        this.helocInfo.fullLoanAmount = this.FltVal("2");
        this.helocInfo.adjustedFullyIndexRate = this.FltVal("3296");
        if (this.helocInfo.fullLoanAmount <= 0.0)
        {
          this.lastErrorFromExampleSchedules = "Total Loan Amount (2) is required to calculate payment schedule for HELOCs in the property state of Florida.";
          return false;
        }
        if (!flag && this.helocInfo.adjustedFullyIndexRate == 0.0)
        {
          this.lastErrorFromExampleSchedules = "Adjusted fully index rate (3296) is required to calculate payment schedule for non-fixed HELOCs in the property state of Florida.";
          return false;
        }
      }
      this.helocInfo.teaserRate = this.FltVal("1482");
      this.helocInfo.teaserTerm = this.IntVal("4492");
      this.helocInfo.floorRate = this.FltVal("1699");
      this.helocInfo.maxAPR = this.FltVal("1893");
      if (!flag && this.helocInfo.maxAPR == 0.0)
      {
        this.lastErrorFromExampleSchedules = "Maximum APR (1893) Required to calculate Maximum Payment.";
        return false;
      }
      if (this.helocInfo.floorRate > 0.0 && this.helocInfo.maxAPR > 0.0 && this.helocInfo.floorRate >= this.helocInfo.maxAPR)
      {
        this.lastErrorFromExampleSchedules = "Maximum APR (1893) must be greater than Floor Rate (1699).";
        return false;
      }
      this.helocInfo.fixedAPR = this.FltVal("4593");
      if (this.helocInfo.fixedAPR == 0.0 & flag)
      {
        this.lastErrorFromExampleSchedules = "Recent APR Lender Has Charged (4593) required to calculate Maximum Payment for Fixed Rate HELOC.";
        return false;
      }
      this.helocInfo.loanAPR = this.FltVal("799");
      this.helocInfo.drawPeriod = this.IntVal("1889");
      this.helocInfo.repaymentPeriod = this.IntVal("1890");
      if (this.helocInfo.drawPeriod == 0)
      {
        this.lastErrorFromExampleSchedules = "Draw Period (1889) Can’t be 0 or blank for HELOC loans.";
        return false;
      }
      this.helocInfo.drawPeriodPrincipalPerc = 0.0;
      if (this.Val("4560") == "Fraction of Balance")
      {
        double num1 = this.FltVal("4564");
        double num2 = this.FltVal("4565");
        if (num1 != 0.0 && num2 != 0.0)
          this.helocInfo.drawPeriodPrincipalPerc = num1 / num2 * 100.0;
      }
      else if (this.Val("4560") == "Percentage of Balance")
        this.helocInfo.drawPeriodPrincipalPerc = this.FltVal("HELOC.MinAdvPct");
      this.helocInfo.drawPeriodMinPayment = this.FltVal("1483");
      this.helocInfo.numberOfDays = this.Val("1962");
      if (this.helocInfo.numberOfDays == string.Empty)
        this.helocInfo.numberOfDays = "360/360";
      this.helocInfo.repaymentPeriodIOChecked = this.Val("4573") == "Y";
      this.helocInfo.repaymentPeriodPrincipalPerc = 0.0;
      this.helocInfo.repaymentfractionOfBalanceUsed = false;
      if (this.Val("4569") == "Fraction of Balance")
      {
        double num3 = this.FltVal("4574");
        double num4 = this.FltVal("4575");
        if (num3 != 0.0 && num4 != 0.0)
          this.helocInfo.repaymentPeriodPrincipalPerc = num3 / num4 * 100.0;
        this.helocInfo.repaymentfractionOfBalanceUsed = true;
      }
      else if (this.Val("4569") == "Percentage of Balance")
        this.helocInfo.repaymentPeriodPrincipalPerc = this.FltVal("HELOC.MinRepmtPct");
      this.helocInfo.repaymentPeriodMinPayment = this.FltVal("4576");
      this.helocInfo.loanTerm = this.helocInfo.drawPeriod + this.helocInfo.repaymentPeriod;
      this.helocInfo.defaultMargin = this.FltVal("HHI.X3");
      this.helocInfo.useAlternateSchedule = this.Val("HHI.X4") == "Y";
      this.helocInfo.indexRatePrecision = this.Val("HHI.X5");
      if (!flag)
      {
        this.helocInfo.historicalIndicesByYear = this.populateHistoricalIndices();
        if (this.helocInfo.historicalIndicesByYear == null)
        {
          this.lastErrorFromExampleSchedules = "Review Historical Index Table (Dynamic Table required) to view Example Schedule.";
          return false;
        }
      }
      return true;
    }

    private Dictionary<int, double> populateHistoricalIndices()
    {
      int historicalIndices = this.loan.GetNumberOfHelocHistoricalIndices();
      if (historicalIndices == 0)
        return (Dictionary<int, double>) null;
      int num1 = this.helocInfo.firstPaymentDate.Year - 15 + 1;
      Dictionary<int, double> dictionary = (Dictionary<int, double>) null;
      int num2 = 9999;
      bool flag = true;
      for (int index = 0; index < historicalIndices; ++index)
      {
        string str = (index + 1).ToString("00");
        int key = this.IntVal("HHI" + str + "01");
        if (key >= num1)
        {
          if (flag)
          {
            dictionary = new Dictionary<int, double>(this.helocInfo.loanTerm / 12);
            num2 = key;
            flag = false;
          }
          else if (key < num2)
            num2 = key;
          dictionary.Add(key, this.FltVal("HHI" + str + "02"));
        }
      }
      if (flag)
        return (Dictionary<int, double>) null;
      this.helocInfo.historicalIndexStartYear = num2;
      int num3 = num2 + this.helocInfo.loanTerm / 12 - 1;
      if (num3 < this.helocInfo.firstPaymentDate.Year)
        num3 = this.helocInfo.firstPaymentDate.Year;
      for (int key1 = num2 + 1; key1 <= num3; ++key1)
      {
        if (!dictionary.ContainsKey(key1))
        {
          double num4 = 0.0;
          for (int key2 = key1 - 1; key2 >= num2; --key2)
          {
            if (dictionary.ContainsKey(key2))
            {
              num4 = dictionary[key2];
              break;
            }
          }
          dictionary.Add(key1, num4);
        }
      }
      return dictionary;
    }

    private double getActiveRate(int year)
    {
      DateTime scheduleStartDate = this.getScheduleStartDate();
      if ((double) this.helocInfo.teaserTerm / 12.0 > (double) (year - scheduleStartDate.Year))
        return this.helocInfo.teaserRate;
      double val = 0.0;
      if (this.helocInfo.historicalIndicesByYear.ContainsKey(year))
        val = this.helocInfo.historicalIndicesByYear[year] + this.helocInfo.defaultMargin;
      if ("FiveDecimals" == this.helocInfo.indexRatePrecision)
        val = Utils.ArithmeticRounding(val, 3);
      if (this.helocInfo.floorRate > 0.0 && val < this.helocInfo.floorRate)
        return this.helocInfo.floorRate;
      return this.helocInfo.maxAPR > 0.0 && val > this.helocInfo.maxAPR ? this.helocInfo.maxAPR : val;
    }

    private double getMonthlyInterestPayment(double upb, double apr, DateTime payDate)
    {
      if (this.helocInfo.numberOfDays == "360/360" || this.helocInfo.numberOfDays == string.Empty)
        return upb * (apr / 100.0) / 12.0;
      DateTime dateTime = payDate.AddMonths(-1);
      int num = DateTime.DaysInMonth(dateTime.Year, dateTime.Month);
      if (this.helocInfo.numberOfDays == "365/360")
        return upb * (apr / 100.0 / 360.0) * (double) num;
      if (!(this.helocInfo.numberOfDays == "365/365"))
        return 0.0;
      bool flag = DateTime.IsLeapYear(dateTime.Year);
      if (this.Val("4913") == "Y")
      {
        flag = false;
        if (num == 29)
          --num;
      }
      return flag ? upb * (apr / 100.0 / 366.0) * (double) num : upb * (apr / 100.0 / 365.0) * (double) num;
    }

    private double getMonthlyPaymentForAmortSchedule(double upb, double apr, int numberOfPayments)
    {
      double num1 = apr / 1200.0;
      double num2 = Math.Pow(1.0 + num1, (double) numberOfPayments);
      return upb / ((1.0 - 1.0 / num2) / num1);
    }

    private DateTime getScheduleStartDate()
    {
      bool flag = this.Val("608") != "Fixed";
      DateTime scheduleStartDate = this.helocInfo.firstPaymentDate.AddYears(1 - (this.helocInfo.loanTerm / 12 >= 15 || !this.helocInfo.useAlternateSchedule ? 15 : this.helocInfo.loanTerm / 12));
      if (flag && scheduleStartDate.Year < this.helocInfo.historicalIndexStartYear)
        scheduleStartDate = new DateTime(this.helocInfo.historicalIndexStartYear, this.helocInfo.firstPaymentDate.Month, this.helocInfo.firstPaymentDate.Day);
      return scheduleStartDate;
    }

    private HelocCalculation.HelocSchedule[] buildSingleHelocExampleSchedule(
      HelocCalculation.ExampleScheduleTypeEnum scheduleType)
    {
      if (scheduleType == HelocCalculation.ExampleScheduleTypeEnum.MAXIMUM_FULLLOANAMT && this.helocInfo.loanState != "FL")
        return (HelocCalculation.HelocSchedule[]) null;
      bool flag1 = this.Val("608") == "Fixed";
      bool flag2 = scheduleType == HelocCalculation.ExampleScheduleTypeEnum.HISTORICAL && !flag1;
      bool flag3 = !this.helocInfo.repaymentPeriodIOChecked;
      DateTime scheduleStartDate = this.getScheduleStartDate();
      double apr = 0.0;
      if (flag1 && scheduleType != HelocCalculation.ExampleScheduleTypeEnum.MAXIMUM_FULLLOANAMT)
      {
        apr = this.helocInfo.fixedAPR;
      }
      else
      {
        switch (scheduleType)
        {
          case HelocCalculation.ExampleScheduleTypeEnum.MINIMUM:
            int year1 = this.helocInfo.firstPaymentDate.Year;
            if (this.helocInfo.historicalIndicesByYear != null && this.helocInfo.historicalIndicesByYear.ContainsKey(year1))
            {
              apr = Utils.ArithmeticRounding(this.helocInfo.historicalIndicesByYear[year1] + this.helocInfo.defaultMargin, 3);
              break;
            }
            this.lastErrorFromExampleSchedules = "Most recent APR cannot be determined to calculate Minimum Example Schedule.";
            return (HelocCalculation.HelocSchedule[]) null;
          case HelocCalculation.ExampleScheduleTypeEnum.MAXIMUM:
            apr = this.helocInfo.maxAPR;
            break;
          case HelocCalculation.ExampleScheduleTypeEnum.MAXIMUM_FULLLOANAMT:
            apr = !flag1 ? this.helocInfo.adjustedFullyIndexRate : this.helocInfo.loanAPR;
            break;
        }
      }
      if (this.helocInfo.floorRate > 0.0 && apr < this.helocInfo.floorRate)
        apr = this.helocInfo.floorRate;
      else if (this.helocInfo.maxAPR > 0.0 && apr > this.helocInfo.maxAPR)
        apr = this.helocInfo.maxAPR;
      double upb = this.helocInfo.loanAmount;
      if (scheduleType == HelocCalculation.ExampleScheduleTypeEnum.MAXIMUM_FULLLOANAMT)
        upb = this.helocInfo.fullLoanAmount;
      HelocCalculation.HelocSchedule[] helocScheduleArray = new HelocCalculation.HelocSchedule[this.helocInfo.loanTerm];
      int months = 0;
      int year2 = scheduleStartDate.Year;
      int num1 = this.helocInfo.loanTerm / 12;
      double val1 = 0.0;
      double num2 = 0.0;
      bool flag4 = true;
      int numberOfPayments = this.helocInfo.loanTerm;
      for (int index1 = 0; index1 < num1; ++index1)
      {
        if (flag2)
          apr = this.getActiveRate(year2);
        bool flag5 = index1 * 12 < this.helocInfo.drawPeriod;
        if (!flag5 & flag4)
        {
          if (scheduleType == HelocCalculation.ExampleScheduleTypeEnum.MAXIMUM)
            upb = this.helocInfo.loanAmount;
          num2 = upb;
          if (flag3 && !flag2)
          {
            if (months > 0)
              numberOfPayments = this.helocInfo.loanTerm - months - 1;
            val1 = this.getMonthlyPaymentForAmortSchedule(upb, apr, numberOfPayments);
          }
          flag4 = false;
        }
        if (!flag5 & flag3 & flag2)
        {
          if (months > 0)
            numberOfPayments = this.helocInfo.loanTerm - months - 1;
          val1 = this.getMonthlyPaymentForAmortSchedule(upb, apr, numberOfPayments);
        }
        for (int index2 = 0; index2 < 12; ++index2)
        {
          months = index1 * 12 + index2;
          DateTime payDate = scheduleStartDate.AddMonths(months);
          double val2 = Utils.ArithmeticRounding(this.getMonthlyInterestPayment(upb, apr, payDate), 2);
          double val3;
          double val4;
          if (flag5)
          {
            val3 = Utils.ArithmeticRounding(upb * this.helocInfo.drawPeriodPrincipalPerc / 100.0, 2);
            val4 = val3 + val2;
          }
          else if (flag3)
          {
            val4 = Utils.ArithmeticRounding(val1, 2);
            val3 = val4 - val2;
          }
          else
          {
            val3 = Utils.ArithmeticRounding(!this.helocInfo.repaymentfractionOfBalanceUsed ? upb * this.helocInfo.repaymentPeriodPrincipalPerc / 100.0 : num2 * this.helocInfo.repaymentPeriodPrincipalPerc / 100.0, 2);
            val4 = val3 + val2;
          }
          double num3 = flag5 ? this.helocInfo.drawPeriodMinPayment : this.helocInfo.repaymentPeriodMinPayment;
          if (val4 < num3)
          {
            val3 = num3 - val2;
            val4 = num3;
          }
          helocScheduleArray[months] = new HelocCalculation.HelocSchedule();
          helocScheduleArray[months].PayDate = payDate;
          helocScheduleArray[months].PeriodType = flag5 ? HelocCalculation.PeriodTypeEnum.DrawPeriod : HelocCalculation.PeriodTypeEnum.RepaymentPeriod;
          helocScheduleArray[months].CurrentRate = apr;
          helocScheduleArray[months].Principal = Utils.ArithmeticRounding(val3, 2);
          helocScheduleArray[months].Interest = Utils.ArithmeticRounding(val2, 2);
          helocScheduleArray[months].PaymentAmount = Utils.ArithmeticRounding(val4, 2);
          helocScheduleArray[months].Balance = Utils.ArithmeticRounding(upb - helocScheduleArray[months].Principal, 2);
          upb = helocScheduleArray[months].Balance;
          if (upb <= 0.0)
            break;
        }
        if (upb > 0.0)
          ++year2;
        else
          break;
      }
      if (upb != 0.0)
      {
        helocScheduleArray[months].Principal += upb;
        helocScheduleArray[months].PaymentAmount += upb;
        helocScheduleArray[months].Balance -= upb;
      }
      return helocScheduleArray;
    }

    internal Dictionary<string, string> BuildHelocExampleSchedules()
    {
      try
      {
        List<HelocCalculation.HelocSchedule[]> exampleSchedules = this.generateHelocExampleSchedules();
        if (exampleSchedules == null || exampleSchedules.Count == 0)
          return (Dictionary<string, string>) null;
        this.historicalSchedule = exampleSchedules.Count > 0 ? exampleSchedules[0] : (HelocCalculation.HelocSchedule[]) null;
        this.minimumSchedule = exampleSchedules.Count > 1 ? exampleSchedules[1] : (HelocCalculation.HelocSchedule[]) null;
        this.maximumSchedule = exampleSchedules.Count > 2 ? exampleSchedules[2] : (HelocCalculation.HelocSchedule[]) null;
        this.maximumScheduleOnFullLoanAmount = exampleSchedules.Count > 3 ? exampleSchedules[3] : (HelocCalculation.HelocSchedule[]) null;
        return this.populateFieldsForEDSFromExampleSchedules();
      }
      catch (Exception ex)
      {
        Tracing.Log(HelocCalculation.sw, nameof (HelocCalculation), TraceLevel.Error, "Error in BuildHelocExampleSchedules(). " + ex.ToString());
        this.lastErrorFromExampleSchedules = "Error in BuildHelocExampleSchedules(). " + ex.ToString();
        return (Dictionary<string, string>) null;
      }
    }

    internal List<List<string[]>> GetHelocExampleSchedules()
    {
      try
      {
        List<HelocCalculation.HelocSchedule[]> exampleSchedules = this.generateHelocExampleSchedules();
        if (exampleSchedules == null || exampleSchedules.Count == 0)
          return (List<List<string[]>>) null;
        this.historicalSchedule = exampleSchedules.Count > 0 ? exampleSchedules[0] : (HelocCalculation.HelocSchedule[]) null;
        this.minimumSchedule = exampleSchedules.Count > 1 ? exampleSchedules[1] : (HelocCalculation.HelocSchedule[]) null;
        this.maximumSchedule = exampleSchedules.Count > 2 ? exampleSchedules[2] : (HelocCalculation.HelocSchedule[]) null;
        this.maximumScheduleOnFullLoanAmount = exampleSchedules.Count > 3 ? exampleSchedules[3] : (HelocCalculation.HelocSchedule[]) null;
        return new List<List<string[]>>()
        {
          this.convertHELOCScheduleToGenericObject(this.historicalSchedule),
          this.convertHELOCScheduleToGenericObject(this.minimumSchedule),
          this.convertHELOCScheduleToGenericObject(this.maximumSchedule),
          this.convertHELOCScheduleToGenericObject(this.maximumScheduleOnFullLoanAmount)
        };
      }
      catch (Exception ex)
      {
        Tracing.Log(HelocCalculation.sw, nameof (HelocCalculation), TraceLevel.Error, "Error in GetHelocExampleSchedules(). " + ex.ToString());
        this.lastErrorFromExampleSchedules = "Unexpected exception. " + ex.ToString();
        return (List<List<string[]>>) null;
      }
    }

    private List<string[]> convertHELOCScheduleToGenericObject(
      HelocCalculation.HelocSchedule[] helocPayments)
    {
      if (helocPayments == null || helocPayments.Length == 0)
        return (List<string[]>) null;
      List<string[]> genericObject = new List<string[]>();
      for (int index = 0; index < helocPayments.Length && helocPayments[index] != null; ++index)
        genericObject.Add(new List<string>()
        {
          helocPayments[index].PayDate.ToString("MM/dd/yyyy"),
          helocPayments[index].CurrentRate.ToString("N3"),
          helocPayments[index].PaymentAmount.ToString("N2"),
          helocPayments[index].Principal.ToString("N2"),
          helocPayments[index].Interest.ToString("N2"),
          helocPayments[index].Balance.ToString("N2"),
          (helocPayments[index].PeriodType == HelocCalculation.PeriodTypeEnum.DrawPeriod ? "Draw" : "Repayment") + " Period"
        }.ToArray());
      return genericObject;
    }

    private List<HelocCalculation.HelocSchedule[]> generateHelocExampleSchedules()
    {
      this.lastErrorFromExampleSchedules = string.Empty;
      if (this.Val("1172") != "HELOC")
      {
        this.lastErrorFromExampleSchedules = "Loan Type (1172) must be HELOC to view Example Schedule.";
        return (List<HelocCalculation.HelocSchedule[]>) null;
      }
      if (this.Val("4630") != "Y" && this.Val("608") != "Fixed")
      {
        this.lastErrorFromExampleSchedules = "Review selected Historical Index Table (Dynamic Table required).";
        return (List<HelocCalculation.HelocSchedule[]>) null;
      }
      if (!this.collectInputs())
      {
        this.historicalSchedule = (HelocCalculation.HelocSchedule[]) null;
        this.minimumSchedule = (HelocCalculation.HelocSchedule[]) null;
        this.maximumSchedule = (HelocCalculation.HelocSchedule[]) null;
        this.maximumScheduleOnFullLoanAmount = (HelocCalculation.HelocSchedule[]) null;
        return (List<HelocCalculation.HelocSchedule[]>) null;
      }
      HelocCalculation.HelocSchedule[] helocScheduleArray1 = this.buildSingleHelocExampleSchedule(HelocCalculation.ExampleScheduleTypeEnum.HISTORICAL);
      HelocCalculation.HelocSchedule[] helocScheduleArray2 = this.buildSingleHelocExampleSchedule(HelocCalculation.ExampleScheduleTypeEnum.MINIMUM);
      HelocCalculation.HelocSchedule[] helocScheduleArray3 = this.buildSingleHelocExampleSchedule(HelocCalculation.ExampleScheduleTypeEnum.MAXIMUM);
      HelocCalculation.HelocSchedule[] helocScheduleArray4 = this.buildSingleHelocExampleSchedule(HelocCalculation.ExampleScheduleTypeEnum.MAXIMUM_FULLLOANAMT);
      if (helocScheduleArray4 != null)
        return new List<HelocCalculation.HelocSchedule[]>()
        {
          helocScheduleArray1,
          helocScheduleArray2,
          helocScheduleArray3,
          helocScheduleArray4
        };
      return new List<HelocCalculation.HelocSchedule[]>()
      {
        helocScheduleArray1,
        helocScheduleArray2,
        helocScheduleArray3
      };
    }

    private Dictionary<string, string> populateFieldsForEDSFromExampleSchedules()
    {
      Dictionary<string, string> dict = new Dictionary<string, string>();
      double num1 = 0.0;
      double num2 = 0.0;
      double num3 = 0.0;
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
      if (this.helocInfo.loanState == "FL")
      {
        double num14 = 0.0;
        int num15 = 0;
        if (this.maximumScheduleOnFullLoanAmount != null)
          num15 = ((IEnumerable<HelocCalculation.HelocSchedule>) this.maximumScheduleOnFullLoanAmount).Where<HelocCalculation.HelocSchedule>((Func<HelocCalculation.HelocSchedule, bool>) (x => x != null)).Count<HelocCalculation.HelocSchedule>();
        if (num15 >= 2)
        {
          double paymentAmount1 = this.maximumScheduleOnFullLoanAmount[num15 - 1].PaymentAmount;
          double paymentAmount2 = this.maximumScheduleOnFullLoanAmount[num15 - 2].PaymentAmount;
          if (paymentAmount1 / paymentAmount2 > 2.0)
            num14 = paymentAmount1;
        }
        dict.Add("BalloonBalanceDueBasedOnLoanAmount", num14.ToString());
      }
      int num16 = ((IEnumerable<HelocCalculation.HelocSchedule>) this.minimumSchedule).Where<HelocCalculation.HelocSchedule>((Func<HelocCalculation.HelocSchedule, bool>) (x => x != null && x.PeriodType == HelocCalculation.PeriodTypeEnum.RepaymentPeriod)).Count<HelocCalculation.HelocSchedule>();
      int num17 = ((IEnumerable<HelocCalculation.HelocSchedule>) this.minimumSchedule).Where<HelocCalculation.HelocSchedule>((Func<HelocCalculation.HelocSchedule, bool>) (x => x != null && x.PeriodType == HelocCalculation.PeriodTypeEnum.DrawPeriod)).Count<HelocCalculation.HelocSchedule>();
      int num18 = ((IEnumerable<HelocCalculation.HelocSchedule>) this.maximumSchedule).Where<HelocCalculation.HelocSchedule>((Func<HelocCalculation.HelocSchedule, bool>) (x => x != null && x.PeriodType == HelocCalculation.PeriodTypeEnum.RepaymentPeriod)).Count<HelocCalculation.HelocSchedule>();
      int num19 = ((IEnumerable<HelocCalculation.HelocSchedule>) this.maximumSchedule).Where<HelocCalculation.HelocSchedule>((Func<HelocCalculation.HelocSchedule, bool>) (x => x != null && x.PeriodType == HelocCalculation.PeriodTypeEnum.DrawPeriod)).Count<HelocCalculation.HelocSchedule>();
      int num20 = ((IEnumerable<HelocCalculation.HelocSchedule>) this.historicalSchedule).Where<HelocCalculation.HelocSchedule>((Func<HelocCalculation.HelocSchedule, bool>) (x => x != null && x.PeriodType == HelocCalculation.PeriodTypeEnum.RepaymentPeriod)).Count<HelocCalculation.HelocSchedule>();
      int num21 = ((IEnumerable<HelocCalculation.HelocSchedule>) this.historicalSchedule).Where<HelocCalculation.HelocSchedule>((Func<HelocCalculation.HelocSchedule, bool>) (x => x != null && x.PeriodType == HelocCalculation.PeriodTypeEnum.DrawPeriod)).Count<HelocCalculation.HelocSchedule>();
      if (num17 > 0)
      {
        num1 = ((IEnumerable<HelocCalculation.HelocSchedule>) this.minimumSchedule).Where<HelocCalculation.HelocSchedule>((Func<HelocCalculation.HelocSchedule, bool>) (x => x != null && x.PeriodType == HelocCalculation.PeriodTypeEnum.DrawPeriod && x.Balance != 0.0)).Select<HelocCalculation.HelocSchedule, double>((Func<HelocCalculation.HelocSchedule, double>) (x => x.PaymentAmount)).DefaultIfEmpty<double>().Min();
        num2 = ((IEnumerable<HelocCalculation.HelocSchedule>) this.minimumSchedule).Where<HelocCalculation.HelocSchedule>((Func<HelocCalculation.HelocSchedule, bool>) (x => x != null && x.PeriodType == HelocCalculation.PeriodTypeEnum.DrawPeriod && x.Balance != 0.0)).Select<HelocCalculation.HelocSchedule, double>((Func<HelocCalculation.HelocSchedule, double>) (x => x.PaymentAmount)).DefaultIfEmpty<double>().Max();
      }
      if (num16 > 0)
      {
        num3 = ((IEnumerable<HelocCalculation.HelocSchedule>) this.minimumSchedule).Where<HelocCalculation.HelocSchedule>((Func<HelocCalculation.HelocSchedule, bool>) (x => x != null && x.PeriodType == HelocCalculation.PeriodTypeEnum.RepaymentPeriod && x.Balance != 0.0)).Select<HelocCalculation.HelocSchedule, double>((Func<HelocCalculation.HelocSchedule, double>) (x => x.PaymentAmount)).DefaultIfEmpty<double>().Min();
        num4 = ((IEnumerable<HelocCalculation.HelocSchedule>) this.minimumSchedule).Where<HelocCalculation.HelocSchedule>((Func<HelocCalculation.HelocSchedule, bool>) (x => x != null && x.PeriodType == HelocCalculation.PeriodTypeEnum.RepaymentPeriod && x.Balance != 0.0)).Select<HelocCalculation.HelocSchedule, double>((Func<HelocCalculation.HelocSchedule, double>) (x => x.PaymentAmount)).DefaultIfEmpty<double>().Max();
      }
      if (num19 > 0)
      {
        num6 = ((IEnumerable<HelocCalculation.HelocSchedule>) this.maximumSchedule).Where<HelocCalculation.HelocSchedule>((Func<HelocCalculation.HelocSchedule, bool>) (x => x != null && x.PeriodType == HelocCalculation.PeriodTypeEnum.DrawPeriod && x.Balance != 0.0)).Select<HelocCalculation.HelocSchedule, double>((Func<HelocCalculation.HelocSchedule, double>) (x => x.PaymentAmount)).DefaultIfEmpty<double>().Min();
        num7 = ((IEnumerable<HelocCalculation.HelocSchedule>) this.maximumSchedule).Where<HelocCalculation.HelocSchedule>((Func<HelocCalculation.HelocSchedule, bool>) (x => x != null && x.PeriodType == HelocCalculation.PeriodTypeEnum.DrawPeriod && x.Balance != 0.0)).Select<HelocCalculation.HelocSchedule, double>((Func<HelocCalculation.HelocSchedule, double>) (x => x.PaymentAmount)).DefaultIfEmpty<double>().Max();
      }
      if (num18 > 0)
      {
        num8 = ((IEnumerable<HelocCalculation.HelocSchedule>) this.maximumSchedule).Where<HelocCalculation.HelocSchedule>((Func<HelocCalculation.HelocSchedule, bool>) (x => x != null && x.PeriodType == HelocCalculation.PeriodTypeEnum.RepaymentPeriod && x.Balance != 0.0)).Select<HelocCalculation.HelocSchedule, double>((Func<HelocCalculation.HelocSchedule, double>) (x => x.PaymentAmount)).DefaultIfEmpty<double>().Min();
        num9 = ((IEnumerable<HelocCalculation.HelocSchedule>) this.maximumSchedule).Where<HelocCalculation.HelocSchedule>((Func<HelocCalculation.HelocSchedule, bool>) (x => x != null && x.PeriodType == HelocCalculation.PeriodTypeEnum.RepaymentPeriod && x.Balance != 0.0)).Select<HelocCalculation.HelocSchedule, double>((Func<HelocCalculation.HelocSchedule, double>) (x => x.PaymentAmount)).DefaultIfEmpty<double>().Max();
      }
      int num22 = ((IEnumerable<HelocCalculation.HelocSchedule>) this.minimumSchedule).Where<HelocCalculation.HelocSchedule>((Func<HelocCalculation.HelocSchedule, bool>) (x => x != null)).Count<HelocCalculation.HelocSchedule>();
      if (num22 > 0)
      {
        num10 = this.minimumSchedule[num22 - 1].PaymentAmount;
        num13 = this.minimumSchedule[0].CurrentRate;
      }
      int num23 = ((IEnumerable<HelocCalculation.HelocSchedule>) this.maximumSchedule).Where<HelocCalculation.HelocSchedule>((Func<HelocCalculation.HelocSchedule, bool>) (x => x != null)).Count<HelocCalculation.HelocSchedule>();
      if (num23 > 0)
        num11 = this.maximumSchedule[num23 - 1].PaymentAmount;
      int num24 = ((IEnumerable<HelocCalculation.HelocSchedule>) this.historicalSchedule).Where<HelocCalculation.HelocSchedule>((Func<HelocCalculation.HelocSchedule, bool>) (x => x != null)).Count<HelocCalculation.HelocSchedule>();
      if (num24 >= 2)
      {
        double paymentAmount3 = this.historicalSchedule[num24 - 1].PaymentAmount;
        double paymentAmount4 = this.historicalSchedule[num24 - 2].PaymentAmount;
        if (paymentAmount3 / paymentAmount4 > 2.0)
          num5 = paymentAmount3;
      }
      if (this.maximumSchedule.Length != 0 && this.minimumSchedule.Length != 0 && this.maximumSchedule[0] != null && this.minimumSchedule[0] != null)
        num12 = this.maximumSchedule[0].CurrentRate - this.minimumSchedule[0].CurrentRate;
      dict.Add("MinExampleMinDrawAmount", num1.ToString());
      dict.Add("MinExampleMaxDrawAmount", num2.ToString());
      dict.Add("MinExampleMinRepaymentAmount", num3.ToString());
      dict.Add("MinExampleMaxRepaymentAmount", num4.ToString());
      dict.Add("HistoricalExampleBalloonAmount", num5.ToString());
      dict.Add("MaxExampleMinDrawAmount", num6.ToString());
      dict.Add("MaxExampleMaxDrawAmount", num7.ToString());
      dict.Add("MaxExampleMinRepaymentAmount", num8.ToString());
      dict.Add("MaxExampleMaxRepaymentAmount", num9.ToString());
      dict.Add("MaxExampleMaxAPRAbove", num12.ToString());
      dict.Add("MinExampleTotalNumberOfRepayments", num16.ToString());
      dict.Add("MinExampleTotalNumberOfDrawPayments", num17.ToString());
      dict.Add("MaxExampleTotalNumberOfDrawPayments", num19.ToString());
      dict.Add("MaxExampleTotalNumberOfRepayments", num18.ToString());
      dict.Add("HistoricalExampleTotalNumberOfDrawPayments", num21.ToString());
      dict.Add("HistoricalExampleTotalNumberOfRepayments", num20.ToString());
      dict.Add("MinExampleFinalPaymentAmount", num10.ToString());
      dict.Add("MaxExampleFinalPaymentAmount", num11.ToString());
      dict.Add("MinExampleAPR", num13.ToString());
      int totalCount = 0;
      bool isFixed = this.Val("608") == "Fixed";
      int year = this.helocInfo.firstPaymentDate.Year;
      int num25 = this.helocInfo.historicalIndexStartYear;
      if (isFixed)
        num25 = this.helocInfo.firstPaymentDate.Year - 15 + 1;
      int num26 = 0;
      if (num24 > 0)
        num26 = this.historicalSchedule[0].PayDate.Year;
      for (; num25 < num26; ++num25)
      {
        ++totalCount;
        this.paddingYearlyRecordForEDS(dict, isFixed, num25, totalCount);
      }
      for (int index1 = 0; index1 < num24; index1 += 12)
      {
        double num27 = double.MaxValue;
        num25 = this.historicalSchedule[index1].PayDate.Year;
        bool flag = this.historicalSchedule[index1].PeriodType == HelocCalculation.PeriodTypeEnum.DrawPeriod;
        if (num25 <= year && totalCount < 15)
        {
          ++totalCount;
          double num28 = isFixed ? this.historicalSchedule[index1].CurrentRate : this.helocInfo.historicalIndicesByYear[num25];
          double num29 = isFixed ? 0.0 : this.helocInfo.defaultMargin;
          double currentRate = this.historicalSchedule[index1].CurrentRate;
          for (int index2 = index1; index2 < index1 + 12 && this.historicalSchedule[index2] != null; ++index2)
          {
            if (this.historicalSchedule[index2].PaymentAmount < num27)
              num27 = this.historicalSchedule[index2].PaymentAmount;
          }
          string key = "Year" + totalCount.ToString();
          dict.Add(key, num25.ToString());
          dict.Add(key + "_PeriodType", flag ? "Draw" : "Repayment");
          if (!isFixed && this.helocInfo.indexRatePrecision == "FiveDecimals")
            dict.Add(key + "_Index", num28.ToString("N5"));
          else
            dict.Add(key + "_Index", num28.ToString("N3"));
          dict.Add(key + "_APR", currentRate.ToString("N3"));
          dict.Add(key + "_Margin", num29.ToString());
          dict.Add(key + "_MinPayment", num27.ToString());
        }
      }
      while (num25 < year)
      {
        ++num25;
        ++totalCount;
        this.paddingYearlyRecordForEDS(dict, isFixed, num25, totalCount);
      }
      dict.Add("CountOfYearlyRecords", totalCount.ToString());
      return dict;
    }

    private void paddingYearlyRecordForEDS(
      Dictionary<string, string> dict,
      bool isFixed,
      int currentYear,
      int totalCount)
    {
      double num1;
      double num2;
      double num3;
      if (isFixed)
      {
        num2 = num1 = this.helocInfo.fixedAPR;
        num3 = 0.0;
      }
      else
      {
        num2 = this.helocInfo.historicalIndicesByYear[currentYear];
        num3 = this.helocInfo.defaultMargin;
        num1 = num2 + num3;
      }
      string key = "Year" + totalCount.ToString();
      dict.Add(key, currentYear.ToString());
      dict.Add(key + "_PeriodType", "");
      dict.Add(key + "_Index", num2.ToString());
      dict.Add(key + "_Margin", num3.ToString());
      dict.Add(key + "_APR", num1.ToString());
      dict.Add(key + "_MinPayment", "0");
    }

    private void calculateHELOCSharedCalculations(string id, string val)
    {
      SharedCalculations.Execute((IHtmlInput) this.loan, SharedCalculations.SharedCalculationType.HELOC, id, val);
    }

    private void calcHELOCPeriodicRates(string id, string val)
    {
      string str = this.Val("1172");
      double num1 = this.FltVal("799");
      if (str != "HELOC" || num1 == 0.0)
      {
        this.SetVal("4549", string.Empty);
        this.SetVal("4550", string.Empty);
      }
      else
      {
        double num2;
        if (this.Val("1962") == "365/365")
        {
          DateTime date = Utils.ParseDate((object) this.Val("2553"), false, DateTime.Now.Date);
          int month = date.Month;
          int year = date.Year;
          bool flag = false;
          if (this.Val("4913") != "Y")
            flag = month <= 2 ? DateTime.IsLeapYear(year) : DateTime.IsLeapYear(year + 1);
          num2 = !flag ? 365.0 : 366.0;
        }
        else
          num2 = 360.0;
        this.SetCurrentNum("4549", Utils.ArithmeticRounding(num1 / num2, 10));
        this.SetCurrentNum("4550", Utils.ArithmeticRounding(num1 / 12.0, 10));
      }
    }

    private void clearCollectInterimInterest(string id, string val)
    {
      if (!(this.Val("1172") != "HELOC"))
        return;
      this.SetVal("4665", "");
    }

    public enum PeriodTypeEnum
    {
      DrawPeriod,
      RepaymentPeriod,
    }

    public enum ExampleScheduleTypeEnum
    {
      HISTORICAL,
      MINIMUM,
      MAXIMUM,
      MAXIMUM_FULLLOANAMT,
    }

    public class HelocSchedule
    {
      public HelocSchedule()
      {
        this.CurrentRate = 0.0;
        this.Principal = 0.0;
        this.Interest = 0.0;
        this.PaymentAmount = 0.0;
        this.Balance = 0.0;
      }

      public DateTime PayDate { set; get; }

      public HelocCalculation.PeriodTypeEnum PeriodType { set; get; }

      public double CurrentRate { set; get; }

      public double Principal { set; get; }

      public double Interest { set; get; }

      public double PaymentAmount { set; get; }

      public double Balance { set; get; }
    }

    private struct InputsForScheduleCalc
    {
      public DateTime firstPaymentDate;
      public int historicalIndexStartYear;
      public double loanAmount;
      public int loanTerm;
      public string loanState;
      public double fullLoanAmount;
      public double adjustedFullyIndexRate;
      public double teaserRate;
      public int teaserTerm;
      public double floorRate;
      public double maxAPR;
      public double fixedAPR;
      public double loanAPR;
      public int drawPeriod;
      public double drawPeriodPrincipalPerc;
      public double drawPeriodMinPayment;
      public string numberOfDays;
      public int repaymentPeriod;
      public bool repaymentPeriodIOChecked;
      public double repaymentPeriodPrincipalPerc;
      public bool repaymentfractionOfBalanceUsed;
      public double repaymentPeriodMinPayment;
      public double defaultMargin;
      public bool useAlternateSchedule;
      public string indexRatePrecision;
      public Dictionary<int, double> historicalIndicesByYear;
    }
  }
}
