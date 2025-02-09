// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.CalculationEngine.ATRQMCalculation
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
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.CalculationEngine
{
  internal class ATRQMCalculation : CalculationBase
  {
    private const string className = "ATRQMCalculation�";
    private static readonly string sw = Tracing.SwDataEngine;
    private SessionObjects sessionObjects;
    internal Routine CalcPrepaymentPenaltyPercent;
    internal Routine CalcMaxTotalPayments;
    internal Routine CalcHigherPricedCheck;
    internal Routine CalcuHousingDebtRatios;
    internal Routine CalcPointsAndFees;
    internal Routine CalcDiscountPointPercent;
    internal Routine CalcMax5YearsPandI;
    internal Routine CalcEligibility;
    internal Routine CalcDiscountPoints;
    internal Routine PopulateQMStatusInNMLSForm;
    internal Routine CalDisclosureLog2015;
    internal Routine CalcAppendixQIncome;
    internal Routine CalcAgencyGSEQMAvailability;
    internal Routine CalcQMX383;
    internal Routine CalculateQmHigherPricedCheck;
    private bool isSmallCreditor;
    private bool isRuralSmallCreditor;
    private DateTime policyExpirationDate = DateTime.MinValue;
    private DateTime policyUsePriceBasedQMStartDate = DateTime.MinValue;
    private static string MEETSSTANDARD = "Meets Standard";
    private static string NOTMEET = "Not Meet";
    private static string REVIEWNEEDED = "Review Needed";
    private static Dictionary<int, double[]> PriceBasedQM_LoanAmtThresholds = new Dictionary<int, double[]>()
    {
      {
        2023,
        new double[2]{ 74599.0, 124331.0 }
      },
      {
        2022,
        new double[2]{ 68908.0, 114847.0 }
      },
      {
        2021,
        new double[2]{ 66156.0, 110260.0 }
      }
    };
    private static double PriceBasedQM_ARP_Threshold1 = 6.5;
    private static double PriceBasedQM_ARP_Threshold2 = 3.5;
    private static double PriceBasedQM_ARP_Threshold3 = 2.25;
    private static string[] penaltyFields = new string[5]
    {
      "1948",
      "1973",
      "1976",
      "1979",
      "1982"
    };

    internal double EffectiveRateForQMAPR { get; set; }

    internal ATRQMCalculation(
      SessionObjects sessionObjects,
      ILoanConfigurationInfo configInfo,
      LoanData l,
      EllieMae.EMLite.CalculationEngine.CalculationObjects calObjs)
      : base(l, calObjs)
    {
      this.sessionObjects = sessionObjects;
      this.CalcPrepaymentPenaltyPercent = this.RoutineX(new Routine(this.calculatePrepaymentPenaltyPercent));
      this.CalcMaxTotalPayments = this.RoutineX(new Routine(this.calculateMaxTotalPayments));
      this.CalcHigherPricedCheck = this.RoutineX(new Routine(this.calculateHigherPricedCheck));
      this.CalcuHousingDebtRatios = this.RoutineX(new Routine(this.calculateHousingDebtRatios));
      this.CalcPointsAndFees = this.RoutineX(new Routine(this.calculatePointsAndFees));
      this.CalcDiscountPointPercent = this.RoutineX(new Routine(this.calculateDiscountPointPercent));
      this.CalcMax5YearsPandI = this.RoutineX(new Routine(this.calculateMax5YearsPandI));
      this.CalcEligibility = this.RoutineX(new Routine(this.calculateEligibility));
      this.CalcDiscountPoints = this.RoutineX(new Routine(this.calculateDiscountPoints));
      this.PopulateQMStatusInNMLSForm = this.RoutineX(new Routine(this.populateQMStatusInNMLSForm));
      this.CalDisclosureLog2015 = this.RoutineX(new Routine(this.CalculateDisclosureLog2015));
      this.CalcAppendixQIncome = this.RoutineX(new Routine(this.calculateAppendixQIncome));
      this.CalcAgencyGSEQMAvailability = this.RoutineX(new Routine(this.calculateAgencyGSEQMAvailability));
      this.CalcQMX383 = this.RoutineX(new Routine(this.calculateQMX383));
      this.CalculateQmHigherPricedCheck = this.RoutineX(new Routine(this.calculateQmHigherPricedCheck));
      this.addFieldHandlers();
      if (configInfo != null && configInfo.DisplayOrganization != null && configInfo.DisplayOrganization.OrgBranchLicensing != null)
      {
        this.isSmallCreditor = configInfo.DisplayOrganization.OrgBranchLicensing.ATRSmallCreditor == BranchLicensing.ATRSmallCreditors.SmallCreditor || configInfo.DisplayOrganization.OrgBranchLicensing.ATRSmallCreditor == BranchLicensing.ATRSmallCreditors.RuralSmallCreditor;
        this.isRuralSmallCreditor = configInfo.DisplayOrganization.OrgBranchLicensing.ATRSmallCreditor == BranchLicensing.ATRSmallCreditors.RuralSmallCreditor;
      }
      this.policyExpirationDate = Utils.ParseDate((object) this.sessionObjects.GetCompanySettingFromCache("Policies", "ATRQMPOLICYEXPIRATIONDATE"));
      this.policyUsePriceBasedQMStartDate = Utils.ParseDate((object) this.sessionObjects.GetCompanySettingFromCache("Policies", "PRICEBASEDQMDEFINATIONDATE"));
    }

    private void addFieldHandlers()
    {
      Routine routine1 = this.RoutineX(new Routine(this.calculateMaxTotalPayments));
      this.AddFieldHandler("1726", routine1);
      this.AddFieldHandler("1727", routine1);
      this.AddFieldHandler("1728", routine1);
      this.AddFieldHandler("1729", routine1);
      this.AddFieldHandler("1730", routine1);
      Routine routine2 = this.RoutineX(new Routine(this.calculateHigherPricedCheck));
      this.AddFieldHandler("799", routine2);
      this.AddFieldHandler("16", routine2);
      Routine routine3 = this.RoutineX(new Routine(this.calculateQmHigherPricedCheck));
      this.AddFieldHandler("QM.X381", routine3);
      this.AddFieldHandler("16", routine3);
      this.AddFieldHandler("420", routine3);
      this.AddFieldHandler("3134", routine3);
      Routine routine4 = this.RoutineX(new Routine(this.calculatePointsAndFees));
      this.AddFieldHandler("QM.X120", routine4);
      this.AddFieldHandler("QM.X121", routine4);
      this.AddFieldHandler("16", routine4);
      Routine routine5 = this.RoutineX(new Routine(this.calculateAppendixQIncome));
      for (int index = 137; index <= 203; ++index)
        this.AddFieldHandler("QM.X" + (object) index, routine5);
      this.AddFieldHandler("QM.X335", routine5);
      this.AddFieldHandler("QM.X336", routine5);
      for (int index = 281; index <= 286; ++index)
        this.AddFieldHandler("QM.X" + (object) index, routine5);
      for (int index = 288; index <= 293; ++index)
        this.AddFieldHandler("QM.X" + (object) index, routine5);
      for (int index = 295; index <= 299; ++index)
        this.AddFieldHandler("QM.X" + (object) index, routine5);
      for (int index = 301; index <= 305; ++index)
        this.AddFieldHandler("QM.X" + (object) index, routine5);
      this.AddFieldHandler("QM.X383", this.RoutineX(new Routine(this.calculateEligibility)));
    }

    public override void FormCal(string id, string val)
    {
      Tracing.Log(ATRQMCalculation.sw, TraceLevel.Info, nameof (ATRQMCalculation), "routine: ATRQMCalculation");
      this.calculatePointsAndFees(id, val);
      this.calculateMaxTotalPayments(id, val);
      this.calculateDiscountPointPercent(id, val);
      this.calculateAppendixQIncome(id, val);
      this.calculateAgencyGSEQMAvailability(id, val);
      this.calculateEligibility(id, val);
      this.calculateDiscountPoints(id, val);
    }

    private void calculateAgencyGSEQMAvailability(string id, string val)
    {
      switch (this.Val("1172"))
      {
        case "Conventional":
          DateTime date = Utils.ParseDate((object) this.Val("745"));
          if (date == DateTime.MinValue)
          {
            this.SetVal("QM.X380", string.Empty);
            break;
          }
          if (DateTime.Compare(this.policyExpirationDate, date) <= 0)
          {
            this.SetVal("QM.X380", ATRQMCalculation.NOTMEET);
            break;
          }
          this.SetVal("QM.X380", ATRQMCalculation.MEETSSTANDARD);
          break;
        case "FarmersHomeAdministration":
        case "FHA":
        case "VA":
          this.SetVal("QM.X380", "NA");
          break;
        default:
          this.SetVal("QM.X380", string.Empty);
          break;
      }
    }

    private double[] getYearlyLoanAmtThresholds()
    {
      int yearToReturn = DateTime.Now.Year;
      bool flag = yearToReturn >= 2023;
      List<FedTresholdAdjustment> source = (List<FedTresholdAdjustment>) null;
      if (flag)
        source = this.sessionObjects?.GetThresholdsFromCache();
      if (source == null || source.Count<FedTresholdAdjustment>() == 0)
        flag = false;
      else
        yearToReturn = source[0].AdjustmentYear;
      int num = ATRQMCalculation.PriceBasedQM_LoanAmtThresholds.Keys.Min();
      DateTime closingDateToUse = this.findClosingDateToUse();
      for (; yearToReturn > num; yearToReturn--)
      {
        DateTime t2 = new DateTime(yearToReturn, 1, 1);
        if (DateTime.Compare(closingDateToUse, t2) >= 0)
        {
          if (flag && yearToReturn >= 2023)
          {
            FedTresholdAdjustment tresholdAdjustment = source.Find((Predicate<FedTresholdAdjustment>) (x => x.AdjustmentYear == yearToReturn && x.RuleIndex == 2));
            if (tresholdAdjustment != null)
              return new double[2]
              {
                Utils.ParseDouble((object) tresholdAdjustment.LowerRange),
                Utils.ParseDouble((object) tresholdAdjustment.UpperRange)
              };
          }
          else if (ATRQMCalculation.PriceBasedQM_LoanAmtThresholds.ContainsKey(yearToReturn))
            break;
        }
      }
      return ATRQMCalculation.PriceBasedQM_LoanAmtThresholds[yearToReturn];
    }

    private bool calculateQMX384()
    {
      string str1 = this.Val("745");
      string str2 = this.Val("QM.X383");
      if (!this.UsePriceBasedQM(Utils.ParseDate((object) str1)) || str2 != "Y")
      {
        this.SetVal("QM.X384", string.Empty);
        return false;
      }
      string str3 = this.Val("1172");
      string str4 = this.Val("420");
      double num1 = this.FltVal("1109");
      double num2 = this.FltVal("3");
      if (str3 == "" || str4 == "" || num1 == 0.0 || num2 == 0.0)
      {
        this.SetVal("QM.X384", ATRQMCalculation.REVIEWNEEDED);
        return true;
      }
      if (str3 != "Conventional" && str3 != "Other")
      {
        this.SetVal("QM.X384", string.Empty);
        return true;
      }
      double[] loanAmtThresholds = this.getYearlyLoanAmtThresholds();
      double num3 = loanAmtThresholds[0];
      double num4 = loanAmtThresholds[1];
      double num5;
      if (str4 == "FirstLien")
      {
        if (num1 < num3)
          num5 = ATRQMCalculation.PriceBasedQM_ARP_Threshold1;
        else if (num1 >= num3 && num1 < num4)
        {
          string str5 = this.Val("1041");
          num5 = str5 == "ManufacturedHousing" || str5 == "ManufacturedHomeCondoPUDCoOp" || str5 == "MHSelect" || str5 == "MHAdvantage" || str5 == "MH HomeChoice" ? ATRQMCalculation.PriceBasedQM_ARP_Threshold1 : ATRQMCalculation.PriceBasedQM_ARP_Threshold2;
        }
        else
          num5 = ATRQMCalculation.PriceBasedQM_ARP_Threshold3;
      }
      else
        num5 = num1 >= num3 ? ATRQMCalculation.PriceBasedQM_ARP_Threshold2 : ATRQMCalculation.PriceBasedQM_ARP_Threshold1;
      if (this.FltVal("QM.X381") < Utils.ArithmeticRounding(this.FltVal("3134") + num5, 3))
        this.SetVal("QM.X384", ATRQMCalculation.MEETSSTANDARD);
      else
        this.SetVal("QM.X384", ATRQMCalculation.NOTMEET);
      return true;
    }

    private void calculateEligibility(string id, string val)
    {
      string str1 = this.Val("1172");
      bool flag1 = str1 == "FHA";
      string str2 = this.Val("19");
      int num1 = this.IntVal("1176");
      DateTime t1_1 = Utils.ParseDate((object) this.Val("3042"));
      if (t1_1 == DateTime.MinValue)
        t1_1 = DateTime.Today;
      bool flag2 = DateTime.Compare(t1_1, DateTime.Parse("01/10/2014")) >= 0;
      DateTime t1_2 = Utils.ParseDate((object) this.Val("745"));
      if (t1_2 == DateTime.MinValue)
        t1_2 = DateTime.Today;
      bool flag3 = str1 == "VA" && DateTime.Compare(t1_2, DateTime.Parse("05/09/2014")) >= 0;
      bool flag4 = this.Val("958") == "IRRRL";
      bool flag5 = flag1 & flag2 | flag3;
      bool flag6 = true;
      for (int index = 338; index <= 348; ++index)
      {
        if (index != 344 && index != 346 && index != 347 && this.Val("QM.X" + (object) index) != "Y")
        {
          flag6 = false;
          break;
        }
      }
      this.SetVal("QM.X27", flag5 ? "" : (flag6 ? ATRQMCalculation.MEETSSTANDARD : ATRQMCalculation.REVIEWNEEDED));
      string strA1 = this.Val("1544");
      bool flag7 = string.Compare(strA1, "Approve Eligible", true) == 0 || string.Compare(strA1, "Accept Eligible", true) == 0 || string.Compare(strA1, "Approve/Eligible", true) == 0 || string.Compare(strA1, "Accept/Eligible", true) == 0 || string.Compare(strA1, "Accept", true) == 0 || string.Compare(strA1, "Approve", true) == 0 || string.Compare(strA1, "ApproveEligible", true) == 0 || string.Compare(strA1, "AcceptEligible", true) == 0;
      if (flag5 || !this.isSmallCreditor)
        this.SetVal("QM.X91", "");
      else if (flag6 && !flag7 && this.FltVal("QM.X119") > 43.0)
        this.SetVal("QM.X91", flag6 ? ATRQMCalculation.MEETSSTANDARD : ATRQMCalculation.NOTMEET);
      else
        this.SetVal("QM.X91", this.isSmallCreditor ? (flag6 ? ATRQMCalculation.MEETSSTANDARD : ATRQMCalculation.REVIEWNEEDED) : "");
      if (this.Val("QM.X349") == "Y")
      {
        this.SetVal("QM.X28", flag5 ? "" : ATRQMCalculation.MEETSSTANDARD);
        this.SetVal("QM.X48", flag5 ? "" : ATRQMCalculation.MEETSSTANDARD);
        if (!flag3)
          this.SetVal("QM.X70", ATRQMCalculation.MEETSSTANDARD);
        this.SetVal("QM.X92", !this.isSmallCreditor || flag5 ? "" : ATRQMCalculation.MEETSSTANDARD);
      }
      else
      {
        this.SetVal("QM.X28", flag5 ? "" : ATRQMCalculation.REVIEWNEEDED);
        this.SetVal("QM.X48", flag5 ? "" : ATRQMCalculation.REVIEWNEEDED);
        if (!flag3)
          this.SetVal("QM.X70", ATRQMCalculation.REVIEWNEEDED);
        this.SetVal("QM.X92", !this.isSmallCreditor || flag5 ? "" : ATRQMCalculation.REVIEWNEEDED);
      }
      if (flag3)
      {
        if (flag4)
          this.SetVal("QM.X70", "");
        else if (this.FltVal("1325") == 0.0 || this.FltVal("1340") == 0.0)
          this.SetVal("QM.X70", ATRQMCalculation.REVIEWNEEDED);
        else if (this.FltVal("1325") >= this.FltVal("1340"))
          this.SetVal("QM.X70", ATRQMCalculation.MEETSSTANDARD);
        else if (this.FltVal("1325") < this.FltVal("1340"))
          this.SetVal("QM.X70", ATRQMCalculation.NOTMEET);
        else
          this.SetVal("QM.X70", "");
      }
      if (this.Val("QM.X338") == "Y")
      {
        this.SetVal("QM.X29", flag5 ? "" : ATRQMCalculation.MEETSSTANDARD);
        this.SetVal("QM.X49", flag5 ? "" : ATRQMCalculation.MEETSSTANDARD);
        this.SetVal("QM.X71", ATRQMCalculation.MEETSSTANDARD);
        this.SetVal("QM.X93", !this.isSmallCreditor || flag5 ? "" : ATRQMCalculation.MEETSSTANDARD);
      }
      else
      {
        this.SetVal("QM.X29", flag5 ? "" : ATRQMCalculation.REVIEWNEEDED);
        this.SetVal("QM.X49", flag5 ? "" : ATRQMCalculation.REVIEWNEEDED);
        this.SetVal("QM.X71", ATRQMCalculation.REVIEWNEEDED);
        this.SetVal("QM.X93", !this.isSmallCreditor || flag5 ? "" : ATRQMCalculation.REVIEWNEEDED);
      }
      string val1;
      if (this.Val("QM.X339") == "Y")
      {
        this.SetVal("QM.X30", flag5 ? "" : ATRQMCalculation.MEETSSTANDARD);
        this.SetVal("QM.X50", flag5 ? "" : ATRQMCalculation.MEETSSTANDARD);
        this.SetVal("QM.X72", ATRQMCalculation.MEETSSTANDARD);
        val1 = !this.isSmallCreditor || flag5 ? "" : ATRQMCalculation.MEETSSTANDARD;
      }
      else
      {
        this.SetVal("QM.X30", flag5 ? "" : ATRQMCalculation.REVIEWNEEDED);
        this.SetVal("QM.X50", flag5 ? "" : ATRQMCalculation.REVIEWNEEDED);
        this.SetVal("QM.X72", ATRQMCalculation.REVIEWNEEDED);
        val1 = !this.isSmallCreditor || flag5 ? "" : ATRQMCalculation.REVIEWNEEDED;
      }
      this.SetVal("QM.X350", val1);
      if (this.Val("QM.X340") == "Y")
      {
        this.SetVal("QM.X31", flag5 ? "" : ATRQMCalculation.MEETSSTANDARD);
        this.SetVal("QM.X51", flag5 ? "" : ATRQMCalculation.MEETSSTANDARD);
        this.SetVal("QM.X73", ATRQMCalculation.MEETSSTANDARD);
        this.SetVal("QM.X94", !this.isSmallCreditor || flag5 ? "" : ATRQMCalculation.MEETSSTANDARD);
      }
      else
      {
        this.SetVal("QM.X31", flag5 ? "" : ATRQMCalculation.REVIEWNEEDED);
        this.SetVal("QM.X51", flag5 ? "" : ATRQMCalculation.REVIEWNEEDED);
        this.SetVal("QM.X73", ATRQMCalculation.REVIEWNEEDED);
        this.SetVal("QM.X94", !this.isSmallCreditor || flag5 ? "" : ATRQMCalculation.REVIEWNEEDED);
      }
      if (this.Val("QM.X344") == "Y")
      {
        this.SetVal("QM.X33", flag5 ? "" : ATRQMCalculation.MEETSSTANDARD);
        this.SetVal("QM.X53", flag5 ? "" : ATRQMCalculation.MEETSSTANDARD);
        this.SetVal("QM.X75", ATRQMCalculation.MEETSSTANDARD);
      }
      else
      {
        this.SetVal("QM.X33", flag5 ? "" : ATRQMCalculation.REVIEWNEEDED);
        this.SetVal("QM.X53", flag5 ? "" : ATRQMCalculation.REVIEWNEEDED);
        this.SetVal("QM.X75", ATRQMCalculation.REVIEWNEEDED);
      }
      if (this.Val("QM.X345") == "Y")
      {
        this.SetVal("QM.X34", flag5 ? "" : ATRQMCalculation.MEETSSTANDARD);
        this.SetVal("QM.X54", flag5 ? "" : ATRQMCalculation.MEETSSTANDARD);
        if (!flag3)
          this.SetVal("QM.X76", ATRQMCalculation.MEETSSTANDARD);
        this.SetVal("QM.X96", !this.isSmallCreditor || flag5 ? "" : ATRQMCalculation.MEETSSTANDARD);
      }
      else
      {
        this.SetVal("QM.X34", flag5 ? "" : ATRQMCalculation.REVIEWNEEDED);
        this.SetVal("QM.X54", flag5 ? "" : ATRQMCalculation.REVIEWNEEDED);
        if (!flag3)
          this.SetVal("QM.X76", ATRQMCalculation.REVIEWNEEDED);
        this.SetVal("QM.X96", !this.isSmallCreditor || flag5 ? "" : ATRQMCalculation.REVIEWNEEDED);
      }
      if (flag3)
      {
        if (!flag4)
          this.SetVal("QM.X76", ATRQMCalculation.MEETSSTANDARD);
        else if (this.Val("QM.X345") == "Y")
          this.SetVal("QM.X76", ATRQMCalculation.MEETSSTANDARD);
        else if (this.FltVal("3") > this.FltVal("VASUMM.X16") || this.Val("VASUMM.X30") == "Y" || this.Val("VASUMM.X36") == "Y")
          this.SetVal("QM.X76", ATRQMCalculation.NOTMEET);
        else
          this.SetVal("QM.X76", ATRQMCalculation.REVIEWNEEDED);
      }
      if (this.Val("QM.X346") == "Y")
      {
        this.SetVal("QM.X35", flag5 ? "" : ATRQMCalculation.MEETSSTANDARD);
        this.SetVal("QM.X55", flag5 ? "" : ATRQMCalculation.MEETSSTANDARD);
        this.SetVal("QM.X77", ATRQMCalculation.MEETSSTANDARD);
        this.SetVal("QM.X97", !this.isSmallCreditor || flag5 ? "" : ATRQMCalculation.MEETSSTANDARD);
      }
      else
      {
        this.SetVal("QM.X35", flag5 ? "" : ATRQMCalculation.REVIEWNEEDED);
        this.SetVal("QM.X55", flag5 ? "" : ATRQMCalculation.REVIEWNEEDED);
        this.SetVal("QM.X77", ATRQMCalculation.REVIEWNEEDED);
        this.SetVal("QM.X97", !this.isSmallCreditor || flag5 ? "" : ATRQMCalculation.REVIEWNEEDED);
      }
      if (this.Val("QM.X341") == "Y")
      {
        this.SetVal("QM.X36", flag5 ? "" : ATRQMCalculation.MEETSSTANDARD);
        this.SetVal("QM.X56", flag5 ? "" : ATRQMCalculation.MEETSSTANDARD);
        this.SetVal("QM.X78", ATRQMCalculation.MEETSSTANDARD);
        this.SetVal("QM.X98", !this.isSmallCreditor || flag5 ? "" : ATRQMCalculation.MEETSSTANDARD);
      }
      else
      {
        this.SetVal("QM.X36", flag5 ? "" : ATRQMCalculation.REVIEWNEEDED);
        this.SetVal("QM.X56", flag5 ? "" : ATRQMCalculation.REVIEWNEEDED);
        this.SetVal("QM.X78", ATRQMCalculation.REVIEWNEEDED);
        this.SetVal("QM.X98", !this.isSmallCreditor || flag5 ? "" : ATRQMCalculation.REVIEWNEEDED);
      }
      if (this.Val("QM.X342") == "Y")
      {
        this.SetVal("QM.X38", flag5 ? "" : ATRQMCalculation.MEETSSTANDARD);
        this.SetVal("QM.X58", flag5 ? "" : ATRQMCalculation.MEETSSTANDARD);
        this.SetVal("QM.X80", ATRQMCalculation.MEETSSTANDARD);
        this.SetVal("QM.X100", !this.isSmallCreditor || flag5 ? "" : ATRQMCalculation.MEETSSTANDARD);
      }
      else
      {
        this.SetVal("QM.X38", flag5 ? "" : ATRQMCalculation.REVIEWNEEDED);
        this.SetVal("QM.X58", flag5 ? "" : ATRQMCalculation.REVIEWNEEDED);
        this.SetVal("QM.X80", ATRQMCalculation.REVIEWNEEDED);
        this.SetVal("QM.X100", !this.isSmallCreditor || flag5 ? "" : ATRQMCalculation.REVIEWNEEDED);
      }
      if (this.Val("QM.X343") == "Y")
      {
        this.SetVal("QM.X37", flag5 ? "" : ATRQMCalculation.MEETSSTANDARD);
        this.SetVal("QM.X57", flag5 ? "" : ATRQMCalculation.MEETSSTANDARD);
        this.SetVal("QM.X79", ATRQMCalculation.MEETSSTANDARD);
        this.SetVal("QM.X99", !this.isSmallCreditor || flag5 ? "" : ATRQMCalculation.MEETSSTANDARD);
      }
      else
      {
        this.SetVal("QM.X37", flag5 ? "" : ATRQMCalculation.REVIEWNEEDED);
        this.SetVal("QM.X57", flag5 ? "" : ATRQMCalculation.REVIEWNEEDED);
        this.SetVal("QM.X79", ATRQMCalculation.REVIEWNEEDED);
        this.SetVal("QM.X99", !this.isSmallCreditor || flag5 ? "" : ATRQMCalculation.REVIEWNEEDED);
      }
      string val2;
      if (this.Val("QM.X347") == "Y")
      {
        this.SetVal("QM.X39", flag5 ? "" : ATRQMCalculation.MEETSSTANDARD);
        this.SetVal("QM.X59", flag5 ? "" : ATRQMCalculation.MEETSSTANDARD);
        this.SetVal("QM.X81", ATRQMCalculation.MEETSSTANDARD);
        val2 = !this.isSmallCreditor || flag5 ? "" : ATRQMCalculation.MEETSSTANDARD;
      }
      else
      {
        this.SetVal("QM.X39", flag5 ? "" : ATRQMCalculation.REVIEWNEEDED);
        this.SetVal("QM.X59", flag5 ? "" : ATRQMCalculation.REVIEWNEEDED);
        this.SetVal("QM.X81", ATRQMCalculation.REVIEWNEEDED);
        val2 = !this.isSmallCreditor || flag5 ? "" : ATRQMCalculation.REVIEWNEEDED;
      }
      this.SetVal("QM.X351", val2);
      int num2 = this.IntVal("4");
      if (num2 > 0 && num2 <= 360)
      {
        this.SetVal("QM.X41", flag5 ? "" : ATRQMCalculation.MEETSSTANDARD);
        this.SetVal("QM.X63", ATRQMCalculation.MEETSSTANDARD);
      }
      else if (num2 > 360)
      {
        this.SetVal("QM.X41", flag5 ? "" : ATRQMCalculation.NOTMEET);
        if (!flag3 || num2 > 361)
          this.SetVal("QM.X63", ATRQMCalculation.NOTMEET);
        else if (num2 <= 361)
          this.SetVal("QM.X63", ATRQMCalculation.MEETSSTANDARD);
      }
      else
      {
        this.SetVal("QM.X41", flag5 ? "" : ATRQMCalculation.REVIEWNEEDED);
        if (!flag3)
          this.SetVal("QM.X63", ATRQMCalculation.REVIEWNEEDED);
        else
          this.SetVal("QM.X63", ATRQMCalculation.NOTMEET);
      }
      if (this.Val("2982") == "N")
      {
        if ((str2 == "ConstructionOnly" || str2 == "ConstructionToPermanent") && num1 > 12)
        {
          this.SetVal("QM.X42", "Not Meet");
          this.SetVal("QM.X64", "Not Meet");
        }
        else
        {
          this.SetVal("QM.X42", flag5 ? "" : ATRQMCalculation.MEETSSTANDARD);
          this.SetVal("QM.X64", ATRQMCalculation.MEETSSTANDARD);
        }
        this.SetVal("QM.X86", !this.isSmallCreditor || flag5 ? "" : ATRQMCalculation.MEETSSTANDARD);
      }
      else if (this.Val("2982") == "Y")
      {
        if ((str2 == "ConstructionOnly" || str2 == "ConstructionToPermanent") && num1 > 12)
        {
          this.SetVal("QM.X42", "Not Meet");
          this.SetVal("QM.X64", "Not Meet");
        }
        else
        {
          this.SetVal("QM.X42", flag5 ? "" : ATRQMCalculation.NOTMEET);
          this.SetVal("QM.X64", ATRQMCalculation.NOTMEET);
        }
        this.SetVal("QM.X86", !this.isSmallCreditor || flag5 ? "" : ATRQMCalculation.NOTMEET);
      }
      else
      {
        if ((str2 == "ConstructionOnly" || str2 == "ConstructionToPermanent") && num1 > 12)
        {
          this.SetVal("QM.X42", "Not Meet");
          this.SetVal("QM.X64", "Not Meet");
        }
        else
        {
          this.SetVal("QM.X42", flag5 ? "" : ATRQMCalculation.REVIEWNEEDED);
          this.SetVal("QM.X64", ATRQMCalculation.REVIEWNEEDED);
        }
        this.SetVal("QM.X86", !this.isSmallCreditor || flag5 ? "" : ATRQMCalculation.REVIEWNEEDED);
      }
      if (this.Val("NEWHUD.X6") == "N")
      {
        this.SetVal("QM.X43", flag5 ? "" : ATRQMCalculation.MEETSSTANDARD);
        this.SetVal("QM.X65", ATRQMCalculation.MEETSSTANDARD);
        this.SetVal("QM.X87", !this.isSmallCreditor || flag5 ? "" : ATRQMCalculation.MEETSSTANDARD);
      }
      else if (this.Val("NEWHUD.X6") == "Y")
      {
        this.SetVal("QM.X43", flag5 ? "" : ATRQMCalculation.NOTMEET);
        this.SetVal("QM.X65", flag3 ? ATRQMCalculation.MEETSSTANDARD : ATRQMCalculation.NOTMEET);
        this.SetVal("QM.X87", !this.isSmallCreditor || flag5 ? "" : ATRQMCalculation.NOTMEET);
      }
      else
      {
        this.SetVal("QM.X43", flag5 ? "" : ATRQMCalculation.REVIEWNEEDED);
        this.SetVal("QM.X65", ATRQMCalculation.REVIEWNEEDED);
        this.SetVal("QM.X87", !this.isSmallCreditor || flag5 ? "" : ATRQMCalculation.REVIEWNEEDED);
      }
      string str3 = this.Val("1659");
      if (str2 == "ConstructionOnly" && num1 > 12)
      {
        this.SetVal("QM.X44", ATRQMCalculation.NOTMEET);
        this.SetVal("QM.X66", ATRQMCalculation.NOTMEET);
      }
      else
      {
        switch (str3)
        {
          case "N":
            this.SetVal("QM.X44", flag5 ? "" : ATRQMCalculation.MEETSSTANDARD);
            this.SetVal("QM.X66", ATRQMCalculation.MEETSSTANDARD);
            break;
          case "Y":
            this.SetVal("QM.X44", flag5 ? "" : ATRQMCalculation.NOTMEET);
            this.SetVal("QM.X66", ATRQMCalculation.NOTMEET);
            break;
          default:
            this.SetVal("QM.X44", flag5 ? "" : ATRQMCalculation.REVIEWNEEDED);
            this.SetVal("QM.X66", ATRQMCalculation.REVIEWNEEDED);
            break;
        }
      }
      if (this.Val("608") == "")
      {
        this.SetVal("QM.X45", flag5 ? "" : ATRQMCalculation.REVIEWNEEDED);
        if (!flag3)
          this.SetVal("QM.X67", ATRQMCalculation.REVIEWNEEDED);
        this.SetVal("QM.X89", !this.isSmallCreditor || flag5 ? "" : ATRQMCalculation.REVIEWNEEDED);
      }
      else if (this.Val("608") != "Fixed" && (this.IntVal("RE88395.X316") > 0 || this.FltVal("1948") > 0.0 || this.FltVal("1973") > 0.0 || this.FltVal("1976") > 0.0 || this.FltVal("1979") > 0.0 || this.FltVal("1982") > 0.0 || this.FltVal("QM.X112") > 0.0) || this.Val("608") == "Fixed" && (this.IntVal("RE88395.X316") > 36 || this.FltVal("1948") > 2.0 || this.FltVal("1973") > 2.0 || this.FltVal("1976") > 2.0 || this.FltVal("1979") > 2.0 || this.FltVal("1982") > 2.0 || this.FltVal("QM.X112") > 2.0))
      {
        this.SetVal("QM.X45", flag5 ? "" : ATRQMCalculation.NOTMEET);
        if (!flag3)
          this.SetVal("QM.X67", ATRQMCalculation.NOTMEET);
        this.SetVal("QM.X89", !this.isSmallCreditor || flag5 ? "" : ATRQMCalculation.NOTMEET);
      }
      else
      {
        this.SetVal("QM.X45", flag5 ? "" : ATRQMCalculation.MEETSSTANDARD);
        if (!flag3)
          this.SetVal("QM.X67", ATRQMCalculation.MEETSSTANDARD);
        this.SetVal("QM.X89", !this.isSmallCreditor || flag5 ? "" : ATRQMCalculation.MEETSSTANDARD);
      }
      if (flag3)
      {
        if (this.IntVal("RE88395.X316") > 0 || this.FltVal("1948") > 0.0 || this.FltVal("1973") > 0.0 || this.FltVal("1976") > 0.0 || this.FltVal("1979") > 0.0 || this.FltVal("1982") > 0.0 || this.FltVal("QM.X112") > 0.0)
          this.SetVal("QM.X67", ATRQMCalculation.NOTMEET);
        else if (this.Val("RE88395.X316") == "" && this.Val("1948") == "" && this.Val("1973") == "" && this.Val("1976") == "" && this.Val("1979") == "" && this.Val("1982") == "" && this.Val("QM.X112") == "")
          this.SetVal("QM.X67", ATRQMCalculation.MEETSSTANDARD);
      }
      bool flag8 = false;
      if (str1 == "VA" && !flag4)
      {
        this.SetVal("QM.X68", ATRQMCalculation.MEETSSTANDARD);
        flag8 = true;
      }
      if (this.FltVal("QM.X120") == 0.0 || this.FltVal("QM.X121") == 0.0)
      {
        this.SetVal("QM.X46", flag5 ? "" : ATRQMCalculation.REVIEWNEEDED);
        if (!flag8)
          this.SetVal("QM.X68", ATRQMCalculation.REVIEWNEEDED);
        this.SetVal("QM.X90", !this.isSmallCreditor || flag5 ? "" : ATRQMCalculation.REVIEWNEEDED);
      }
      else if (this.FltVal("S32DISC.X48") <= this.FltVal("QM.X121"))
      {
        this.SetVal("QM.X46", flag5 ? "" : ATRQMCalculation.MEETSSTANDARD);
        if (!flag8)
          this.SetVal("QM.X68", ATRQMCalculation.MEETSSTANDARD);
        this.SetVal("QM.X90", !this.isSmallCreditor || flag5 ? "" : ATRQMCalculation.MEETSSTANDARD);
      }
      else
      {
        this.SetVal("QM.X46", flag5 ? "" : ATRQMCalculation.NOTMEET);
        if (!flag8)
          this.SetVal("QM.X68", ATRQMCalculation.NOTMEET);
        this.SetVal("QM.X90", !this.isSmallCreditor || flag5 ? "" : ATRQMCalculation.NOTMEET);
      }
      if (flag5)
      {
        this.SetVal("QM.X47", "");
      }
      else
      {
        bool flag9 = true;
        for (int index = 338; index <= 348; ++index)
        {
          if (index != 344 && index != 346 && index != 347 && this.Val("QM.X" + (object) index) != "Y")
          {
            flag9 = false;
            break;
          }
        }
        if (flag9)
        {
          if (this.FltVal("QM.X119") <= 43.0)
            this.SetVal("QM.X47", ATRQMCalculation.MEETSSTANDARD);
          else
            this.SetVal("QM.X47", ATRQMCalculation.NOTMEET);
        }
        else
          this.SetVal("QM.X47", ATRQMCalculation.REVIEWNEEDED);
      }
      bool flag10 = true;
      for (int index = 338; index <= 348; ++index)
      {
        if (index != 344 && index != 346 && index != 347 && this.Val("QM.X" + (object) index) != "Y")
        {
          flag10 = false;
          break;
        }
      }
      if (flag3)
      {
        if (!flag10 || (this.Val("VASUMM.X4") == "" || this.Val("VASUMM.X21") == "") && string.Compare(this.Val("1543"), "Manual Underwriting", true) != 0)
          this.SetVal("QM.X69", ATRQMCalculation.REVIEWNEEDED);
        else if (this.Val("VASUMM.X4") == "Yes" && this.Val("VASUMM.X21") == "1" || this.Val("VASUMM.X4") == "No" && string.Compare(this.Val("1543"), "Manual Underwriting", true) == 0 && (this.Val("3878") == "Y" || this.Val("3879") == "Y" || this.Val("3880") == "Y"))
          this.SetVal("QM.X69", ATRQMCalculation.MEETSSTANDARD);
        else if (this.Val("VASUMM.X4") == "Yes" && this.Val("VASUMM.X21") == "2" || this.Val("VASUMM.X4") == "No" && string.Compare(this.Val("1543"), "Manual Underwriting", true) == 0 && (this.Val("3878") != "Y" || this.Val("3879") != "Y" || this.Val("3880") != "Y"))
          this.SetVal("QM.X69", ATRQMCalculation.NOTMEET);
        else
          this.SetVal("QM.X69", ATRQMCalculation.REVIEWNEEDED);
      }
      else if (flag1)
      {
        if (flag2)
        {
          if ((this.Val("3029") == "A/A" || this.Val("3029") == "Refer") && this.Val("3631") == "Eligible" || this.Val("3193") == "Y" || this.Val("3195") == "Y")
            this.SetVal("QM.X69", ATRQMCalculation.MEETSSTANDARD);
          else if (this.Val("3193") != "Y" && this.Val("3195") != "Y" && this.Val("3029") != "A/A" && this.Val("3029") != "Refer" && this.Val("3631") != "Eligible")
            this.SetVal("QM.X69", ATRQMCalculation.NOTMEET);
          else
            this.SetVal("QM.X69", ATRQMCalculation.REVIEWNEEDED);
        }
        else
          this.SetVal("QM.X69", ATRQMCalculation.REVIEWNEEDED);
      }
      else if (!flag10 || this.Val("1544") == "" && string.Compare(this.Val("1543"), "Manual Underwriting", true) != 0)
        this.SetVal("QM.X69", ATRQMCalculation.REVIEWNEEDED);
      else if (flag7 || string.Compare(this.Val("1543"), "Manual Underwriting", true) == 0 && (this.Val("3878") == "Y" || this.Val("3879") == "Y" || this.Val("3880") == "Y"))
        this.SetVal("QM.X69", ATRQMCalculation.MEETSSTANDARD);
      else
        this.SetVal("QM.X69", ATRQMCalculation.NOTMEET);
      if (this.isSmallCreditor && !flag5)
      {
        int num3 = this.IntVal("325");
        bool flag11 = this.Val("NEWHUD.X5") == "Y";
        if (num2 == 0)
        {
          this.SetVal("QM.X85", ATRQMCalculation.REVIEWNEEDED);
          this.SetVal("QM.X88", ATRQMCalculation.REVIEWNEEDED);
        }
        else if (str3 == "Y" && num2 >= 60 && num2 <= 360 && num3 >= 60 && num3 <= 360 && !flag11 || str3 == "N" && num2 <= 360)
        {
          this.SetVal("QM.X85", ATRQMCalculation.MEETSSTANDARD);
          this.SetVal("QM.X88", ATRQMCalculation.MEETSSTANDARD);
        }
        else if (str3 == "Y" && ((num2 > 360 || num2 < 60 || num3 > 360 ? 1 : (num3 < 60 ? 1 : 0)) | (flag11 ? 1 : 0)) != 0 || str3 == "N" && num2 > 360)
        {
          this.SetVal("QM.X85", ATRQMCalculation.NOTMEET);
          this.SetVal("QM.X88", ATRQMCalculation.NOTMEET);
        }
      }
      else
      {
        this.SetVal("QM.X85", "");
        this.SetVal("QM.X88", "");
      }
      bool flag12 = false;
      bool flag13 = false;
      bool flag14 = false;
      string val3 = "";
      if (!flag5)
      {
        for (int index = 27; index <= 39; ++index)
        {
          if (index != 32)
          {
            string str4 = this.Val("QM.X" + (object) index);
            if (str4 == ATRQMCalculation.NOTMEET)
            {
              flag12 = true;
              break;
            }
            if (index != 28 && str4 == ATRQMCalculation.REVIEWNEEDED)
              flag13 = true;
            else if (str4 == ATRQMCalculation.MEETSSTANDARD)
              flag14 = true;
          }
        }
        val3 = !flag12 ? (!flag13 ? (!flag14 ? "" : ATRQMCalculation.MEETSSTANDARD) : ATRQMCalculation.REVIEWNEEDED) : ATRQMCalculation.NOTMEET;
      }
      this.SetVal("QM.X26", val3);
      bool qmX384 = this.calculateQMX384();
      if (flag5)
      {
        this.SetVal("QM.X40", "");
      }
      else
      {
        bool flag15 = false;
        bool flag16 = false;
        bool flag17 = false;
        for (int index = 41; index <= 59; ++index)
        {
          if (index != 52)
          {
            string str5 = !(index == 47 & qmX384) ? this.Val("QM.X" + (object) index) : this.Val("QM.X384");
            if (str5 == ATRQMCalculation.NOTMEET)
            {
              flag15 = true;
              break;
            }
            if (index != 48 && str5 == ATRQMCalculation.REVIEWNEEDED)
              flag16 = true;
            else if (str5 == ATRQMCalculation.MEETSSTANDARD)
              flag17 = true;
          }
        }
        if (flag15)
          this.SetVal("QM.X40", ATRQMCalculation.NOTMEET);
        else if (flag16)
          this.SetVal("QM.X40", ATRQMCalculation.REVIEWNEEDED);
        else if (flag17)
          this.SetVal("QM.X40", ATRQMCalculation.MEETSSTANDARD);
        else
          this.SetVal("QM.X40", "");
      }
      bool flag18 = false;
      bool flag19 = false;
      bool flag20 = false;
      for (int index = 63; index <= 81; ++index)
      {
        if (index != 74)
        {
          string str6 = this.Val("QM.X" + (object) index);
          if (str6 == ATRQMCalculation.NOTMEET)
          {
            flag18 = true;
            break;
          }
          if (index != 70 && str6 == ATRQMCalculation.REVIEWNEEDED)
            flag19 = true;
          else if (str6 == ATRQMCalculation.MEETSSTANDARD)
            flag20 = true;
        }
      }
      if (this.Val("QM.X380") == ATRQMCalculation.NOTMEET)
        this.SetVal("QM.X62", ATRQMCalculation.NOTMEET);
      else if (flag18)
        this.SetVal("QM.X62", ATRQMCalculation.NOTMEET);
      else if (flag19)
        this.SetVal("QM.X62", ATRQMCalculation.REVIEWNEEDED);
      else if (flag20)
      {
        if (flag1)
        {
          if (flag2)
          {
            if ((this.Val("3029") == "A/A" || this.Val("3029") == "Refer") && this.Val("3631") == "Eligible" || this.Val("3193") == "Y" || this.Val("3195") == "Y")
              this.SetVal("QM.X62", ATRQMCalculation.MEETSSTANDARD);
            else if (this.Val("3193") != "Y" && this.Val("3195") != "Y" && this.Val("3029") != "A/A" && this.Val("3029") != "Refer" && this.Val("3631") != "Eligible")
              this.SetVal("QM.X62", ATRQMCalculation.NOTMEET);
            else
              this.SetVal("QM.X62", ATRQMCalculation.REVIEWNEEDED);
          }
          else
            this.SetVal("QM.X62", ATRQMCalculation.REVIEWNEEDED);
        }
        else if (flag7 || string.Compare(this.Val("1543"), "Manual Underwriting", true) == 0 && (this.Val("3878") == "Y" || this.Val("3879") == "Y" || this.Val("3880") == "Y"))
          this.SetVal("QM.X62", ATRQMCalculation.MEETSSTANDARD);
        else
          this.SetVal("QM.X62", ATRQMCalculation.REVIEWNEEDED);
      }
      else
        this.SetVal("QM.X62", "");
      string val4 = "";
      if (this.isSmallCreditor && !flag5)
      {
        bool flag21 = false;
        bool flag22 = false;
        bool flag23 = false;
        for (int index = 85; index <= 100; ++index)
        {
          if (index != 95)
          {
            string str7 = this.Val("QM.X" + (object) index);
            if (str7 == ATRQMCalculation.NOTMEET)
            {
              flag21 = true;
              break;
            }
            if (index != 92 && str7 == ATRQMCalculation.REVIEWNEEDED)
              flag22 = true;
            else if (str7 == ATRQMCalculation.MEETSSTANDARD)
              flag23 = true;
          }
        }
        if (val1 == ATRQMCalculation.NOTMEET || val2 == ATRQMCalculation.NOTMEET)
          flag21 = true;
        if (val1 == ATRQMCalculation.REVIEWNEEDED || val2 == ATRQMCalculation.REVIEWNEEDED)
          flag22 = true;
        if (val1 == ATRQMCalculation.MEETSSTANDARD || val2 == ATRQMCalculation.MEETSSTANDARD)
          flag23 = true;
        if (flag21)
          val4 = ATRQMCalculation.NOTMEET;
        else if (flag22)
          val4 = ATRQMCalculation.REVIEWNEEDED;
        else if (flag23)
          val4 = ATRQMCalculation.MEETSSTANDARD;
      }
      this.SetVal("QM.X84", val4);
      double num4 = this.FltVal("799");
      int num5 = this.IntVal("16");
      string strA2 = this.Val("420");
      double num6 = this.FltVal("3134");
      string str8 = this.Val("QM.X62");
      double num7 = this.FltVal("1199");
      double num8 = 0.0;
      string str9 = this.Val("QM.X383");
      double num9 = this.FltVal("QM.X381");
      if (flag1)
      {
        if (num4 == 0.0 || num6 == 0.0 || num7 == 0.0 || this.Val("QM.X24") != "FHA QM")
          this.SetVal("QM.X83", "");
        else
          this.SetCurrentNum("QM.X83", num6 + num7 + 1.15);
      }
      else if (flag3 || !flag1 && (str1 == string.Empty || str1 == "Other" || str1 == "HELOC") || num4 == 0.0 || num6 == 0.0 || strA2 == string.Empty || num5 >= 5 || num5 == 0)
        this.SetVal("QM.X83", "");
      else if (string.Compare(strA2, "FirstLien", true) == 0)
        this.SetCurrentNum("QM.X83", num6 + 1.5);
      else if (string.Compare(strA2, "SecondLien", true) == 0)
        this.SetCurrentNum("QM.X83", num6 + 3.5);
      if (flag1 & flag2 | flag3)
        this.SetVal("QM.X61", "");
      else if (num4 == 0.0 || str9 == "Y" && num9 == 0.0 || num6 == 0.0 || strA2 == string.Empty || num5 >= 5 || num5 == 0)
        this.SetVal("QM.X61", "");
      else if (string.Compare(strA2, "FirstLien", true) == 0)
        this.SetCurrentNum("QM.X61", flag5 ? 0.0 : num6 + 1.5);
      else if (string.Compare(strA2, "SecondLien", true) == 0)
        this.SetCurrentNum("QM.X61", flag5 ? 0.0 : num6 + 3.5);
      if (flag1 & flag2 | flag3)
        num8 = 0.0;
      else if (num4 == 0.0 || num6 == 0.0 || strA2 == string.Empty || num5 >= 5 || num5 == 0)
      {
        if (this.isSmallCreditor)
          num8 = 0.0;
      }
      else if (string.Compare(strA2, "FirstLien", true) == 0)
        num8 = this.isSmallCreditor ? num6 + 3.5 : 0.0;
      else if (string.Compare(strA2, "SecondLien", true) == 0)
        num8 = this.isSmallCreditor ? num6 + 3.5 : 0.0;
      this.SetVal("QM.X102", num8 == 0.0 | flag5 ? "" : num8.ToString("N3"));
      double num10 = this.FltVal("QM.X61");
      string str10 = this.Val("QM.X40");
      if (flag5)
        this.SetVal("QM.X60", "");
      else if (str10 == ATRQMCalculation.MEETSSTANDARD)
      {
        if (num10 == 0.0)
          this.SetVal("QM.X60", ATRQMCalculation.REVIEWNEEDED);
        else
          this.SetVal("QM.X60", (str9 == "Y" ? num9 : num4) < num10 ? ATRQMCalculation.MEETSSTANDARD : ATRQMCalculation.NOTMEET);
      }
      else
        this.SetVal("QM.X60", "");
      string val5 = "";
      if (this.isSmallCreditor && !flag5)
      {
        double num11 = num8;
        string str11 = val4;
        if (str11 == ATRQMCalculation.MEETSSTANDARD && num4 < num11 && num11 != 0.0)
          val5 = ATRQMCalculation.MEETSSTANDARD;
        else if (str11 == ATRQMCalculation.MEETSSTANDARD && num4 >= num11 && num11 != 0.0)
          val5 = ATRQMCalculation.NOTMEET;
        else if (str11 == ATRQMCalculation.MEETSSTANDARD && num11 == 0.0)
          val5 = ATRQMCalculation.REVIEWNEEDED;
      }
      this.SetVal("QM.X101", val5);
      if (str2 == "ConstructionOnly" && num1 <= 12)
      {
        this.SetVal("QM.X103", "Y");
        this.SetVal("QM.X106", "Y");
        this.SetVal("QM.X107", "Construction Only");
      }
      if (this.Val("QM.X103") != "Y")
      {
        for (int index = 104; index <= 110; ++index)
        {
          if (index != 109)
            this.SetVal("QM.X" + (object) index, "");
        }
        if (str2 == "ConstructionOnly" && num1 > 12)
          this.SetVal("QM.X23", "General ATR");
        else if (this.Val("QM.X40") == ATRQMCalculation.MEETSSTANDARD || this.Val("QM.X62") == ATRQMCalculation.MEETSSTANDARD || val4 == ATRQMCalculation.MEETSSTANDARD)
          this.SetVal("QM.X23", "Qualified Mortgage");
        else if (val3 == ATRQMCalculation.MEETSSTANDARD)
          this.SetVal("QM.X23", "General ATR");
        else
          this.SetVal("QM.X23", "");
      }
      else
      {
        if (str2 == "ConstructionOnly" && num1 > 12)
          this.SetVal("QM.X23", "General ATR");
        else
          this.SetVal("QM.X23", "Exempt");
        if (this.Val("QM.X104") != "Y")
          this.SetVal("QM.X105", "");
        if (this.Val("QM.X106") != "Y")
          this.SetVal("QM.X107", "");
      }
      this.populateQMStatusInNMLSForm(id, val);
      string str12 = this.Val("QM.X23");
      string val6 = string.Empty;
      if (str2 == "ConstructionOnly" || str12 == "Exempt")
        val6 = "";
      else if (flag3)
        val6 = !(str8 == ATRQMCalculation.MEETSSTANDARD) ? "" : "VA QM";
      else if (((!flag1 ? 0 : (str8 == ATRQMCalculation.MEETSSTANDARD ? 1 : 0)) & (flag2 ? 1 : 0)) != 0)
        val6 = "FHA QM";
      else if (str12 == "Qualified Mortgage")
      {
        if (this.Val("QM.X40") == ATRQMCalculation.MEETSSTANDARD)
          val6 = "General QM";
        else if (str8 == ATRQMCalculation.MEETSSTANDARD)
          val6 = "Agency/GSE QM";
        else if (val4 == ATRQMCalculation.MEETSSTANDARD)
          val6 = !(this.Val("3850") == "Y") ? "Small Creditor QM" : "Small Creditor Rural QM";
      }
      this.SetVal("QM.X24", val6);
      if (this.Val("QM.X380") == ATRQMCalculation.NOTMEET)
        this.SetVal("QM.X82", ATRQMCalculation.NOTMEET);
      else if (flag3)
      {
        if (!flag4 || this.IntVal("VASUMM.X130") <= 36 && this.FltVal("3") < this.FltVal("VASUMM.X16") && this.Val("VASUMM.X30") != "Y" && this.Val("VASUMM.X36") == "N")
          this.SetVal("QM.X82", ATRQMCalculation.MEETSSTANDARD);
        else if (flag4 && (this.IntVal("VASUMM.X130") > 36 || this.FltVal("3") > this.FltVal("VASUMM.X16") || this.Val("VASUMM.X30") == "Y" || this.Val("VASUMM.X36") == "Y"))
          this.SetVal("QM.X82", ATRQMCalculation.NOTMEET);
        else
          this.SetVal("QM.X82", ATRQMCalculation.REVIEWNEEDED);
      }
      else if (flag1)
      {
        double num12 = this.FltVal("QM.X83");
        string str13 = this.Val("QM.X62");
        if (str13 == ATRQMCalculation.MEETSSTANDARD && num4 <= num12 && num12 != 0.0)
          this.SetVal("QM.X82", ATRQMCalculation.MEETSSTANDARD);
        else if (str13 == ATRQMCalculation.MEETSSTANDARD && num4 > num12 && num12 != 0.0)
          this.SetVal("QM.X82", ATRQMCalculation.NOTMEET);
        else if (str13 == ATRQMCalculation.MEETSSTANDARD && num12 == 0.0)
          this.SetVal("QM.X82", ATRQMCalculation.REVIEWNEEDED);
        else
          this.SetVal("QM.X82", "");
      }
      else
      {
        double num13 = this.FltVal("QM.X83");
        string str14 = this.Val("QM.X62");
        if (str14 == ATRQMCalculation.MEETSSTANDARD && num4 < num13 && num13 != 0.0)
          this.SetVal("QM.X82", ATRQMCalculation.MEETSSTANDARD);
        else if (str14 == ATRQMCalculation.MEETSSTANDARD && num4 >= num13 && num13 != 0.0)
          this.SetVal("QM.X82", ATRQMCalculation.NOTMEET);
        else if (str14 == ATRQMCalculation.MEETSSTANDARD && num13 == 0.0)
          this.SetVal("QM.X82", ATRQMCalculation.REVIEWNEEDED);
        else
          this.SetVal("QM.X82", "");
      }
      if (str2 == "ConstructionOnly")
        this.SetVal("QM.X25", "N/A");
      else if (flag3)
      {
        this.SetVal("QM.X25", this.Val("QM.X82") == ATRQMCalculation.MEETSSTANDARD ? "Yes" : "No");
      }
      else
      {
        switch (str12)
        {
          case "Qualified Mortgage":
            switch (val6)
            {
              case "General QM":
                this.SetVal("QM.X25", this.Val("QM.X60") == ATRQMCalculation.MEETSSTANDARD ? "Yes" : "No");
                return;
              case "Agency/GSE QM":
                this.SetVal("QM.X25", this.Val("QM.X82") == ATRQMCalculation.MEETSSTANDARD ? "Yes" : "No");
                return;
              case "Small Creditor QM":
              case "Small Creditor Rural QM":
                this.SetVal("QM.X25", val5 == ATRQMCalculation.MEETSSTANDARD ? "Yes" : "No");
                return;
              case "FHA QM":
                this.SetVal("QM.X25", this.Val("QM.X82") == ATRQMCalculation.MEETSSTANDARD ? "Yes" : "No");
                return;
              default:
                this.SetVal("QM.X25", "No");
                return;
            }
          case "Exempt":
          case "General ATR":
            this.SetVal("QM.X25", "N/A");
            break;
          default:
            this.SetVal("QM.X25", "");
            break;
        }
      }
    }

    internal void CopyAUSToLoan()
    {
      this.SetVal("1543", this.Val("AUSF.X1"));
      this.SetVal("1556", this.Val("AUSF.X2"));
      this.SetVal("1544", this.Val("AUSF.X3"));
      this.SetVal("DU.LP.ID", this.Val("AUSF.X4"));
      this.SetVal("1545", this.Val("AUSF.X5"));
    }

    private void calculatePrepaymentPenaltyPercent(string id, string val)
    {
      double num1 = 0.0;
      if (this.IsLocked("RE88395.X315") && this.FltVal("1109") != 0.0)
        num1 = Utils.ArithmeticRounding(this.FltVal("RE88395.X315") / this.FltVal("1109") * 100.0, 3);
      if (num1 == 0.0)
      {
        for (int index = 0; index < ATRQMCalculation.penaltyFields.Length; ++index)
        {
          double num2 = this.FltVal(ATRQMCalculation.penaltyFields[index]);
          if (num2 > num1)
            num1 = num2;
        }
      }
      this.SetCurrentNum("QM.X112", num1);
    }

    private void calculateMaxTotalPayments(string id, string val)
    {
      BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
      string str1 = this.Val("1811", borrowerPairs[0]);
      bool flag1 = str1 == "SecondHome" || str1 == "Investor";
      bool flag2 = this.Val("420") == "SecondLien";
      bool flag3 = this.Val("608") == "AdjustableRate";
      int num1 = this.IntVal("325");
      int num2 = this.IntVal("1177");
      double num3 = this.FltVal("2");
      string str2 = this.Val("19");
      int num4 = this.IntVal("1176");
      bool flag4 = this.Val("CONST.X1") == "Y";
      bool useSimpleInterest = this.checkIfSimpleInterest();
      string constInterestType = this.findConstInterestType();
      DateTime firstPaymentDate = this.findFirstPaymentDate();
      bool use366ForLeapYear = this.findSimpleInterestUse366ForLeapYear();
      if (flag3)
      {
        double currentRate = this.FltVal("1827");
        if (this.FltVal("3") > currentRate)
          currentRate = this.FltVal("3");
        if (num1 == 0)
          num1 = this.IntVal("4");
        if (num2 > 0 || str2 == "ConstructionToPermanent")
        {
          int unpaidTerm = num1 - num2;
          if (flag4)
            unpaidTerm -= num4;
          double num5 = RegzCalculation.CalcRawMonthlyPayment(unpaidTerm, num3, currentRate, num1, 0, num3, this.Val("4746"), useSimpleInterest, firstPaymentDate, constInterestType, use366ForLeapYear);
          this.SetCurrentNum("QM.X373", num5, this.UseNoPayment(num5));
          double num6 = RegzCalculation.CalcRawMonthlyPayment(unpaidTerm, num3, this.FltVal("3"), num1, 0, num3, this.Val("4746"), useSimpleInterest, firstPaymentDate, constInterestType, use366ForLeapYear);
          this.SetCurrentNum("QM.X374", num6, this.UseNoPayment(num6));
        }
        else
        {
          double num7 = RegzCalculation.CalcRawMonthlyPayment(num1, num3, currentRate, num1, 0, num3, this.Val("4746"), useSimpleInterest, firstPaymentDate, constInterestType, use366ForLeapYear);
          this.SetCurrentNum("QM.X373", num7, this.UseNoPayment(num7));
          if (flag1 && num1 > 0 && num1 < this.IntVal("4") && num1 <= 60)
          {
            string val1 = this.Val("GLOBAL.S5");
            this.SetCurrentNum("QM.X374", this.Flt(val1), !string.IsNullOrEmpty(val1) && this.UseNoPayment(this.Flt(val1)));
          }
          else
          {
            double num8 = RegzCalculation.CalcRawMonthlyPayment(num1, num3, this.FltVal("3"), num1, 0, num3, this.Val("4746"), useSimpleInterest, firstPaymentDate, constInterestType, use366ForLeapYear);
            this.SetCurrentNum("QM.X374", num8, this.UseNoPayment(num8));
          }
        }
      }
      else
      {
        this.SetCurrentNum("QM.X373", 0.0, this.UseNoPayment(0.0));
        int unpaidTerm = num1 - num2;
        if (flag4)
          unpaidTerm -= num4;
        if (num2 > 0 || str2 == "ConstructionToPermanent")
        {
          double num9 = RegzCalculation.CalcRawMonthlyPayment(unpaidTerm, num3, this.FltVal("3"), num1, 0, num3, this.Val("4746"), useSimpleInterest, firstPaymentDate, constInterestType, use366ForLeapYear);
          this.SetCurrentNum("QM.X374", num9, this.UseNoPayment(num9));
        }
        else if (num1 > 0 && num1 < this.IntVal("4") && num1 <= 60)
        {
          double num10 = this.FltVal("GLOBAL.S5") - this.FltVal("1766");
          this.SetCurrentNum("QM.X374", num10, this.UseNoPayment(num10));
        }
        else
        {
          string val2 = this.Val("3290");
          this.SetCurrentNum("QM.X374", this.Flt(val2), !string.IsNullOrEmpty(val2) && this.UseNoPayment(this.Flt(val2)));
        }
      }
      double num11;
      if (flag1)
      {
        double num12 = this.FltVal("URLA.X144") + this.FltVal("230") + this.FltVal("1405") + this.FltVal("232") + this.FltVal("233") + this.FltVal("234");
        num11 = !flag2 ? num12 + this.FltVal("229") : num12 + this.FltVal("228");
      }
      else if (flag2)
      {
        num11 = this.FltVal("1724", borrowerPairs[0]) + this.FltVal("1726", borrowerPairs[0]) + this.FltVal("4947", borrowerPairs[0]) + this.FltVal("1727", borrowerPairs[0]) + this.FltVal("1728", borrowerPairs[0]) + this.FltVal("1729", borrowerPairs[0]) + this.FltVal("1730", borrowerPairs[0]);
        if (str1 != "PrimaryResidence")
          num11 += this.FltVal("737", borrowerPairs[0]);
      }
      else
        num11 = this.FltVal("1725", borrowerPairs[0]) + this.FltVal("1726", borrowerPairs[0]) + this.FltVal("4947", borrowerPairs[0]) + this.FltVal("1727", borrowerPairs[0]) + this.FltVal("1728", borrowerPairs[0]) + this.FltVal("1729", borrowerPairs[0]) + this.FltVal("1730", borrowerPairs[0]);
      this.SetCurrentNum("QM.X113", num11 + this.FltVal("QM.X374"));
      if (!flag3)
        this.SetVal("QM.X114", "");
      else
        this.SetCurrentNum("QM.X114", num11 + this.FltVal("QM.X373"));
      if (!flag3 && this.IntVal("1177") >= 60)
        this.SetVal("QM.X117", "");
      else
        this.SetCurrentNum("QM.X117", num11 + this.FltVal("QM.X337"));
    }

    private void calculateHousingDebtRatios(string id, string val)
    {
      bool flag1 = this.Val("608") == "AdjustableRate";
      int num1 = this.IntVal("1177");
      BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
      string str = this.Val("1811", borrowerPairs[0]);
      bool flag2 = str == "SecondHome" || str == "Investor";
      bool flag3 = this.Val("420") == "SecondLien";
      double num2 = this.FltVal("1389", borrowerPairs[0]);
      double num3;
      if (flag2)
      {
        num3 = this.FltVal("229") + this.FltVal("230") + this.FltVal("URLA.X144") + this.FltVal("1405") + this.FltVal("232") + this.FltVal("234");
      }
      else
      {
        double num4 = flag3 ? this.FltVal("1724", borrowerPairs[0]) : this.FltVal("1725", borrowerPairs[0]);
        for (int index = 26; index <= 30; ++index)
          num4 += this.FltVal("17" + (object) index, borrowerPairs[0]);
        num3 = num4 + this.FltVal("4947", borrowerPairs[0]);
        if (flag3 && str != "PrimaryResidence")
          num3 += this.FltVal("737", borrowerPairs[0]);
      }
      if (num2 == 0.0)
      {
        this.SetVal("QM.X375", "");
        this.SetVal("QM.X115", "");
        this.SetVal("QM.X118", "");
      }
      else
      {
        this.SetCurrentNum("QM.X375", Utils.ArithmeticRounding((num3 + this.FltVal("QM.X374")) / num2 * 100.0, 3));
        this.SetCurrentNum("QM.X115", Utils.ArithmeticRounding((num3 + this.FltVal("QM.X373")) / num2 * 100.0, 3));
        if (!flag1 && num1 >= 60)
          this.SetVal("QM.X118", "");
        else
          this.SetCurrentNum("QM.X118", Utils.ArithmeticRounding((num3 + this.FltVal("QM.X337")) / num2 * 100.0, 3));
      }
      double num5 = !flag2 ? num3 + (this.FltVal("462", borrowerPairs[0]) + this.FltVal("1384", borrowerPairs[0]) + this.FltVal("1733", borrowerPairs[0]) + this.FltVal("1379", borrowerPairs[0])) : this.FltVal("1731", borrowerPairs[0]) + this.FltVal("1384", borrowerPairs[0]) + this.FltVal("1733", borrowerPairs[0]) + this.FltVal("462", borrowerPairs[0]);
      if (num2 == 0.0)
      {
        this.SetVal("QM.X376", "");
        this.SetVal("QM.X116", "");
        this.SetVal("QM.X119", "");
      }
      else
      {
        this.SetCurrentNum("QM.X376", Utils.ArithmeticRounding((this.FltVal("1384", borrowerPairs[0]) + this.FltVal("1733", borrowerPairs[0]) + this.FltVal("QM.X113") + (flag2 ? this.FltVal("462", borrowerPairs[0]) + this.FltVal("1731", borrowerPairs[0]) : this.FltVal("1379", borrowerPairs[0]))) / num2 * 100.0, 3));
        if (flag2)
        {
          if (!flag1)
          {
            this.SetCurrentNum("QM.X116", Utils.ArithmeticRounding(num5 / num2 * 100.0, 3));
            if (num1 >= 60)
              this.SetVal("QM.X119", "");
            else
              this.SetCurrentNum("QM.X119", Utils.ArithmeticRounding((num5 + this.FltVal("QM.X117")) / num2 * 100.0, 3));
          }
          else
          {
            this.SetCurrentNum("QM.X116", Utils.ArithmeticRounding((num5 + this.FltVal("QM.X114")) / num2 * 100.0, 3));
            this.SetCurrentNum("QM.X119", Utils.ArithmeticRounding((num5 + this.FltVal("QM.X117")) / num2 * 100.0, 3));
          }
        }
        else
        {
          this.SetCurrentNum("QM.X116", Utils.ArithmeticRounding((num5 + this.FltVal("QM.X373")) / num2 * 100.0, 3));
          if (num1 >= 60)
            this.SetVal("QM.X119", "");
          else
            this.SetCurrentNum("QM.X119", Utils.ArithmeticRounding((num5 + this.FltVal("QM.X337")) / num2 * 100.0, 3));
        }
      }
      if (flag1)
        return;
      this.SetVal("QM.X115", "");
      this.SetVal("QM.X116", "");
    }

    private void calculateMax5YearsPandI(string id, string val)
    {
      int loanTerm = this.IntVal("325");
      int num1 = this.IntVal("1177");
      double num2 = this.FltVal("2");
      string str = this.Val("19");
      int num3 = this.IntVal("1176");
      int unpaidTerm = loanTerm - num1;
      bool flag = true;
      if (this.Val("608") == "Fixed")
      {
        if (this.Val("423") == "Biweekly")
        {
          double num4 = this.FltVal("HUD51") * 26.0 / 12.0;
          this.SetCurrentNum("QM.X337", num4, this.UseNoPayment(num4));
        }
        else if (num1 >= 60)
          this.SetVal("QM.X337", "");
        else if (this.IntVal("325") > 0 && this.IntVal("325") < this.IntVal("4"))
        {
          double num5 = this.FltVal("GLOBAL.S5") - this.FltVal("1766");
          this.SetCurrentNum("QM.X337", num5, this.UseNoPayment(num5));
        }
        else if (str == "ConstructionToPermanent")
        {
          if (this.Val("CONST.X1") == "Y")
            unpaidTerm -= num3;
          if (num3 + num1 > 60)
          {
            this.SetVal("QM.X337", "");
          }
          else
          {
            double num6 = RegzCalculation.CalcRawMonthlyPayment(unpaidTerm, num2, this.FltVal("3"), loanTerm, 0, num2, this.Val("4746"), this.checkIfSimpleInterest(), this.findFirstPaymentDate(), this.findConstInterestType(), this.findSimpleInterestUse366ForLeapYear());
            this.SetCurrentNum("QM.X337", num6, this.UseNoPayment(num6));
          }
        }
        else
        {
          string val1 = this.Val("5");
          this.SetCurrentNum("QM.X337", this.Flt(val1), !string.IsNullOrEmpty(val1) && this.UseNoPayment(this.Flt(val1)));
        }
      }
      else
      {
        if (str == "ConstructionToPermanent" && (num1 > 0 || num3 > 12))
        {
          this.SetVal("QM.X337", "");
          flag = false;
        }
        if (!flag)
          return;
        string val2 = this.Val("3285");
        this.SetCurrentNum("QM.X337", this.Flt(val2), !string.IsNullOrEmpty(val2) && this.UseNoPayment(this.Flt(val2)));
      }
    }

    private void calculateHigherPricedCheck(string id, string val)
    {
      string str = this.Val("420");
      double num1 = this.FltVal("3134");
      double num2 = this.FltVal("799");
      int num3 = this.IntVal("16");
      if (num3 == 0 || num3 >= 5 || str == string.Empty || num1 == 0.0 || num2 == 0.0)
        this.SetVal("QM.X135", "");
      else if (str == "SecondLien")
      {
        if (Utils.ArithmeticRounding(num2 - 3.5, 3) >= num1)
          this.SetVal("QM.X135", "is");
        else
          this.SetVal("QM.X135", "is not");
      }
      else if (Utils.ArithmeticRounding(num2 - 1.5, 3) >= num1)
        this.SetVal("QM.X135", "is");
      else
        this.SetVal("QM.X135", "is not");
    }

    private void calculateQmHigherPricedCheck(string id, string val)
    {
      string str = this.Val("420");
      double num1 = this.FltVal("3134");
      double num2 = this.FltVal("QM.X381");
      int num3 = this.IntVal("16");
      if (num3 == 0 || num3 >= 5 || str == string.Empty || num1 == 0.0 || num2 == 0.0)
      {
        this.SetVal("QM.X382", "");
      }
      else
      {
        switch (str)
        {
          case "SecondLien":
            if (Utils.ArithmeticRounding(num2 - 3.5, 3) >= num1)
            {
              this.SetVal("QM.X382", "is");
              break;
            }
            this.SetVal("QM.X382", "is not");
            break;
          case "FirstLien":
            if (Utils.ArithmeticRounding(num2 - 1.5, 3) >= num1)
            {
              this.SetVal("QM.X382", "is");
              break;
            }
            this.SetVal("QM.X382", "is not");
            break;
          default:
            this.SetVal("QM.X382", "");
            break;
        }
      }
    }

    private bool CalcPointsAndFeesFromDB(
      int yearToStart,
      List<FedTresholdAdjustment> adjustments,
      double totalLoanAmt,
      double regzTotalAmt)
    {
      DateTime closingDateToUse = this.findClosingDateToUse();
      for (; yearToStart >= 2023; yearToStart--)
      {
        DateTime t2 = new DateTime(yearToStart, 1, 1);
        if (DateTime.Compare(closingDateToUse, t2) >= 0)
        {
          List<FedTresholdAdjustment> list = adjustments.Where<FedTresholdAdjustment>((Func<FedTresholdAdjustment, bool>) (x => x.AdjustmentYear == yearToStart)).OrderBy<FedTresholdAdjustment, int>((Func<FedTresholdAdjustment, int>) (x => x.RuleIndex)).ToList<FedTresholdAdjustment>();
          for (int index = 0; index < list.Count<FedTresholdAdjustment>(); ++index)
          {
            double num1 = Utils.ParseDouble((object) list[index].UpperRange);
            double num2 = Utils.ParseDouble((object) list[index].LowerRange);
            double num3 = Utils.ParseDouble((object) list[index].RuleValue);
            string ruleType = list[index].RuleType;
            int ruleIndex = list[index].RuleIndex;
            double num4 = 0.0;
            if (ruleType.ToLower() == "percent")
              num4 = Utils.TruncateToCents(regzTotalAmt * (num3 / 100.0));
            else if (ruleType.ToLower() == "amount")
              num4 = num3;
            if (index == 0 && ruleIndex == 1)
            {
              if (totalLoanAmt >= num2)
              {
                this.SetCurrentNum("QM.X121", num4);
                return true;
              }
            }
            else if (totalLoanAmt >= num2 && totalLoanAmt < num1)
            {
              this.SetCurrentNum("QM.X121", num4);
              return true;
            }
          }
        }
      }
      return false;
    }

    private void calculatePointsAndFees(string id, string val)
    {
      double num1 = this.FltVal("QM.X120");
      if (!this.IsLocked("QM.X120"))
      {
        num1 = !(this.Val("1172") == "HELOC") ? this.FltVal("2") - this.FltVal("949") - this.calObjs.GFECal.PrepaymentPenaltyAmount() : this.FltVal("2");
        this.SetCurrentNum("QM.X120", num1);
      }
      double totalLoanAmt = this.FltVal("2");
      int yearToStart = 2023;
      bool flag1 = true;
      List<FedTresholdAdjustment> thresholdsFromCache = this.sessionObjects?.GetThresholdsFromCache();
      if (thresholdsFromCache == null || thresholdsFromCache.Count<FedTresholdAdjustment>() == 0)
        flag1 = false;
      else
        yearToStart = thresholdsFromCache[0].AdjustmentYear;
      DateTime t2_1 = DateTime.Parse("01/01/2023");
      DateTime t2_2 = DateTime.Parse("01/01/2022");
      DateTime t2_3 = DateTime.Parse("01/01/2021");
      DateTime t2_4 = DateTime.Parse("01/01/2020");
      DateTime t2_5 = DateTime.Parse("01/01/2019");
      DateTime t2_6 = DateTime.Parse("01/01/2018");
      DateTime t2_7 = DateTime.Parse("01/01/2017");
      DateTime t2_8 = DateTime.Parse("01/01/2016");
      DateTime t2_9 = DateTime.Parse("01/01/2015");
      bool flag2 = false;
      if (this.Val("1172") == "VA" && this.Val("958") == "IRRRL")
      {
        this.SetCurrentNum("QM.X121", Utils.TruncateToCents(num1 * 0.03));
      }
      else
      {
        if (flag1)
          flag2 = this.CalcPointsAndFeesFromDB(yearToStart, thresholdsFromCache, totalLoanAmt, num1);
        if (!flag2)
        {
          if (DateTime.Compare(Utils.ParseDate((object) this.Val("763")), t2_1) >= 0 || DateTime.Compare(Utils.ParseDate((object) this.Val("748")), t2_1) >= 0 || DateTime.Compare(Utils.ParseDate((object) this.Val("1887")), t2_1) >= 0)
          {
            if (totalLoanAmt >= 124331.0)
              this.SetCurrentNum("QM.X121", Utils.TruncateToCents(num1 * 0.03));
            else if (totalLoanAmt >= 74599.0 && totalLoanAmt < 124331.0)
              this.SetCurrentNum("QM.X121", 3730.0);
            else if (totalLoanAmt >= 24866.0 && totalLoanAmt < 74599.0)
              this.SetCurrentNum("QM.X121", Utils.TruncateToCents(num1 * 0.05));
            else if (totalLoanAmt >= 15541.0 && totalLoanAmt < 24866.0)
              this.SetCurrentNum("QM.X121", 1243.0);
            else if (totalLoanAmt < 15541.0)
              this.SetCurrentNum("QM.X121", Utils.TruncateToCents(num1 * 0.08));
          }
          else if (DateTime.Compare(Utils.ParseDate((object) this.Val("763")), t2_2) >= 0 || DateTime.Compare(Utils.ParseDate((object) this.Val("748")), t2_2) >= 0 || DateTime.Compare(Utils.ParseDate((object) this.Val("1887")), t2_2) >= 0)
          {
            if (totalLoanAmt >= 114847.0)
              this.SetCurrentNum("QM.X121", Utils.TruncateToCents(num1 * 0.03));
            else if (totalLoanAmt >= 68908.0 && totalLoanAmt <= 114846.99)
              this.SetCurrentNum("QM.X121", 3445.0);
            else if (totalLoanAmt >= 22969.0 && totalLoanAmt <= 68907.99)
              this.SetCurrentNum("QM.X121", Utils.TruncateToCents(num1 * 0.05));
            else if (totalLoanAmt >= 14356.0 && totalLoanAmt <= 22968.99)
              this.SetCurrentNum("QM.X121", 1148.0);
            else if (totalLoanAmt < 14356.0)
              this.SetCurrentNum("QM.X121", Utils.TruncateToCents(num1 * 0.08));
          }
          else if (DateTime.Compare(Utils.ParseDate((object) this.Val("763")), t2_3) >= 0 || DateTime.Compare(Utils.ParseDate((object) this.Val("748")), t2_3) >= 0 || DateTime.Compare(Utils.ParseDate((object) this.Val("1887")), t2_3) >= 0)
          {
            if (totalLoanAmt >= 110260.0)
              this.SetCurrentNum("QM.X121", Utils.TruncateToCents(num1 * 0.03));
            else if (totalLoanAmt >= 66156.0 && totalLoanAmt <= 110259.99)
              this.SetCurrentNum("QM.X121", 3308.0);
            else if (totalLoanAmt >= 22052.0 && totalLoanAmt <= 66155.99)
              this.SetCurrentNum("QM.X121", Utils.TruncateToCents(num1 * 0.05));
            else if (totalLoanAmt >= 13783.0 && totalLoanAmt <= 22051.99)
              this.SetCurrentNum("QM.X121", 1103.0);
            else if (totalLoanAmt < 13783.0)
              this.SetCurrentNum("QM.X121", Utils.TruncateToCents(num1 * 0.08));
          }
          else if (DateTime.Compare(Utils.ParseDate((object) this.Val("763")), t2_4) >= 0 || DateTime.Compare(Utils.ParseDate((object) this.Val("748")), t2_4) >= 0 || DateTime.Compare(Utils.ParseDate((object) this.Val("1887")), t2_4) >= 0)
          {
            if (totalLoanAmt >= 109898.0)
              this.SetCurrentNum("QM.X121", Utils.TruncateToCents(num1 * 0.03));
            else if (totalLoanAmt >= 65939.0 && totalLoanAmt <= 109897.99)
              this.SetCurrentNum("QM.X121", 3297.0);
            else if (totalLoanAmt >= 21980.0 && totalLoanAmt <= 65938.99)
              this.SetCurrentNum("QM.X121", Utils.TruncateToCents(num1 * 0.05));
            else if (totalLoanAmt >= 13737.0 && totalLoanAmt <= 21979.99)
              this.SetCurrentNum("QM.X121", 1099.0);
            else if (totalLoanAmt < 13737.0)
              this.SetCurrentNum("QM.X121", Utils.TruncateToCents(num1 * 0.08));
          }
          else if (DateTime.Compare(Utils.ParseDate((object) this.Val("745")), t2_5) >= 0 || DateTime.Compare(Utils.ParseDate((object) this.Val("763")), t2_5) >= 0 || DateTime.Compare(Utils.ParseDate((object) this.Val("748")), t2_5) >= 0 || DateTime.Compare(Utils.ParseDate((object) this.Val("1887")), t2_5) >= 0)
          {
            if (totalLoanAmt >= 107747.0)
              this.SetCurrentNum("QM.X121", Utils.TruncateToCents(num1 * 0.03));
            else if (totalLoanAmt >= 64648.0 && totalLoanAmt < 107747.0)
              this.SetCurrentNum("QM.X121", 3232.0);
            else if (totalLoanAmt >= 21549.0 && totalLoanAmt < 64648.0)
              this.SetCurrentNum("QM.X121", Utils.TruncateToCents(num1 * 0.05));
            else if (totalLoanAmt >= 13468.0 && totalLoanAmt < 21549.0)
              this.SetCurrentNum("QM.X121", 1077.0);
            else if (totalLoanAmt < 13468.0)
              this.SetCurrentNum("QM.X121", Utils.TruncateToCents(num1 * 0.08));
          }
          else if (DateTime.Compare(Utils.ParseDate((object) this.Val("745")), t2_6) >= 0 || DateTime.Compare(Utils.ParseDate((object) this.Val("763")), t2_6) >= 0 || DateTime.Compare(Utils.ParseDate((object) this.Val("748")), t2_6) >= 0 || DateTime.Compare(Utils.ParseDate((object) this.Val("1887")), t2_6) >= 0)
          {
            if (totalLoanAmt >= 105158.0)
              this.SetCurrentNum("QM.X121", Utils.TruncateToCents(num1 * 0.03));
            else if (totalLoanAmt >= 63095.0 && totalLoanAmt < 105158.0)
              this.SetCurrentNum("QM.X121", 3155.0);
            else if (totalLoanAmt >= 21032.0 && totalLoanAmt < 63095.0)
              this.SetCurrentNum("QM.X121", Utils.TruncateToCents(num1 * 0.05));
            else if (totalLoanAmt >= 13145.0 && totalLoanAmt < 21032.0)
              this.SetCurrentNum("QM.X121", 1052.0);
            else if (totalLoanAmt < 13145.0)
              this.SetCurrentNum("QM.X121", Utils.TruncateToCents(num1 * 0.08));
          }
          else if (DateTime.Compare(Utils.ParseDate((object) this.Val("745")), t2_7) >= 0 || DateTime.Compare(Utils.ParseDate((object) this.Val("763")), t2_7) >= 0 || DateTime.Compare(Utils.ParseDate((object) this.Val("748")), t2_7) >= 0 || DateTime.Compare(Utils.ParseDate((object) this.Val("1887")), t2_7) >= 0)
          {
            if (totalLoanAmt >= 102894.0)
              this.SetCurrentNum("QM.X121", Utils.TruncateToCents(num1 * 0.03));
            else if (totalLoanAmt >= 61737.0 && totalLoanAmt < 102894.0)
              this.SetCurrentNum("QM.X121", 3087.0);
            else if (totalLoanAmt >= 20579.0 && totalLoanAmt < 61737.0)
              this.SetCurrentNum("QM.X121", Utils.TruncateToCents(num1 * 0.05));
            else if (totalLoanAmt >= 12862.0 && totalLoanAmt < 20579.0)
              this.SetCurrentNum("QM.X121", 1029.0);
            else if (totalLoanAmt < 12862.0)
              this.SetCurrentNum("QM.X121", Utils.TruncateToCents(num1 * 0.08));
          }
          else if (DateTime.Compare(Utils.ParseDate((object) this.Val("745")), t2_8) >= 0 || DateTime.Compare(Utils.ParseDate((object) this.Val("763")), t2_8) >= 0 || DateTime.Compare(Utils.ParseDate((object) this.Val("748")), t2_8) >= 0 || DateTime.Compare(Utils.ParseDate((object) this.Val("1887")), t2_8) >= 0)
          {
            if (totalLoanAmt >= 101749.0)
              this.SetCurrentNum("QM.X121", Utils.TruncateToCents(num1 * 0.03));
            else if (totalLoanAmt >= 61050.0 && totalLoanAmt < 101749.0)
              this.SetCurrentNum("QM.X121", 3052.0);
            else if (totalLoanAmt >= 20350.0 && totalLoanAmt < 61050.0)
              this.SetCurrentNum("QM.X121", Utils.TruncateToCents(num1 * 0.05));
            else if (totalLoanAmt >= 12719.0 && totalLoanAmt < 20350.0)
              this.SetCurrentNum("QM.X121", 1017.0);
            else if (totalLoanAmt < 12719.0)
              this.SetCurrentNum("QM.X121", Utils.TruncateToCents(num1 * 0.08));
          }
          else if (DateTime.Compare(Utils.ParseDate((object) this.Val("745")), t2_9) >= 0 || DateTime.Compare(Utils.ParseDate((object) this.Val("763")), t2_9) >= 0 || DateTime.Compare(Utils.ParseDate((object) this.Val("748")), t2_9) >= 0 || DateTime.Compare(Utils.ParseDate((object) this.Val("1887")), t2_9) >= 0)
          {
            if (totalLoanAmt >= 101953.0)
              this.SetCurrentNum("QM.X121", Utils.TruncateToCents(num1 * 0.03));
            else if (totalLoanAmt >= 61172.0 && totalLoanAmt < 101953.0)
              this.SetCurrentNum("QM.X121", 3059.0);
            else if (totalLoanAmt >= 20391.0 && totalLoanAmt < 61172.0)
              this.SetCurrentNum("QM.X121", Utils.TruncateToCents(num1 * 0.05));
            else if (totalLoanAmt >= 12744.0 && totalLoanAmt < 20391.0)
              this.SetCurrentNum("QM.X121", 1020.0);
            else if (totalLoanAmt < 12744.0)
              this.SetCurrentNum("QM.X121", Utils.TruncateToCents(num1 * 0.08));
          }
          else if (totalLoanAmt >= 100000.0)
            this.SetCurrentNum("QM.X121", Utils.TruncateToCents(num1 * 0.03));
          else if (totalLoanAmt >= 60000.0 && totalLoanAmt < 100000.0)
            this.SetCurrentNum("QM.X121", 3000.0);
          else if (totalLoanAmt >= 20000.0 && totalLoanAmt < 60000.0)
            this.SetCurrentNum("QM.X121", Utils.TruncateToCents(num1 * 0.05));
          else if (totalLoanAmt >= 12500.0 && totalLoanAmt < 20000.0)
            this.SetCurrentNum("QM.X121", 1000.0);
          else if (totalLoanAmt < 12500.0)
            this.SetCurrentNum("QM.X121", Utils.TruncateToCents(num1 * 0.08));
        }
      }
      double num2 = this.FltVal("QM.X121");
      double num3 = this.FltVal("S32DISC.X48");
      double num4 = this.FltVal("16");
      if (num1 == 0.0)
      {
        this.SetVal("QM.X122", "");
        this.SetVal("QM.X123", "");
      }
      else
      {
        this.SetCurrentNum("QM.X122", Utils.ArithmeticRounding(num2 / num1 * 100.0, 3));
        this.SetCurrentNum("QM.X123", Utils.ArithmeticRounding(num3 / num1 * 100.0, 3));
      }
      if (num1 == 0.0 || num2 == 0.0 || num4 >= 5.0 || num4 == 0.0)
        this.SetVal("QM.X124", "");
      else if (num3 > 0.0 && num3 > num2)
        this.SetVal("QM.X124", "does");
      else
        this.SetVal("QM.X124", "does not");
    }

    private void calculateDiscountPointPercent(string id, string val)
    {
      if (this.Val("NEWHUD.X1139") == "Y" && !this.UseNew2015GFEHUD)
        this.SetCurrentNum("QM.X111", this.FltVal("NEWHUD.X1151"));
      else
        this.SetCurrentNum("QM.X111", this.FltVal("NEWHUD.X15"));
      double num = this.FltVal("2");
      if (num == 0.0)
        this.SetVal("QM.X136", "");
      else
        this.SetCurrentNum("QM.X136", Utils.ArithmeticRounding(this.FltVal("NEWHUD.X15") / num * 100.0, 3));
      if (!(id == "QM.X372") || !(this.Val("QM.X372") == "Y"))
        return;
      if (this.FltVal("NEWHUD.X225") != 0.0)
        this.SetCurrentNum("QM.X371", this.FltVal("NEWHUD.X225"));
      else
        this.SetCurrentNum("QM.X371", this.FltVal("439"));
    }

    private void calculateAppendixQIncome(string id, string val)
    {
      double num1 = 0.0;
      for (int index = 137; index <= 143; ++index)
        num1 += this.FltVal("QM.X" + (object) index);
      this.SetCurrentNum("QM.X144", num1);
      double num2 = 0.0;
      for (int index = 145; index <= 151; ++index)
        num2 += this.FltVal("QM.X" + (object) index);
      this.SetCurrentNum("QM.X152", num2);
      double num3 = 0.0;
      for (int index = 155; index <= 158; ++index)
        num3 += this.FltVal("QM.X" + (object) index);
      this.SetCurrentNum("QM.X308", num3);
      double num4 = 0.0;
      for (int index = 159; index <= 162; ++index)
        num4 += this.FltVal("QM.X" + (object) index);
      this.SetCurrentNum("QM.X309", num4);
      if (id == "popup" || id == "QM.X281" || id == "QM.X282" || id == "QM.X283" || id == "QM.X284" || id == "QM.X285" || id == "QM.X286" || id == "QM.X288" || id == "QM.X289" || id == "QM.X290" || id == "QM.X291" || id == "QM.X292" || id == "QM.X293" || id == "QM.X295" || id == "QM.X296" || id == "QM.X297" || id == "QM.X298" || id == "QM.X299" || id == "QM.X301" || id == "QM.X302" || id == "QM.X303" || id == "QM.X304" || id == "QM.X305")
      {
        if (id == "popup" || id == "QM.X281" || id == "QM.X282" || id == "QM.X283" || id == "QM.X284" || id == "QM.X285" || id == "QM.X286")
        {
          double num5 = 0.0;
          for (int index = 281; index <= 286; ++index)
            num5 += this.FltVal("QM.X" + (object) index);
          this.SetCurrentNum("QM.X287", num5);
          this.SetCurrentNum("QM.X163", num5);
        }
        if (id == "popup" || id == "QM.X288" || id == "QM.X289" || id == "QM.X290" || id == "QM.X291" || id == "QM.X292" || id == "QM.X293")
        {
          double num6 = 0.0;
          for (int index = 288; index <= 293; ++index)
            num6 += this.FltVal("QM.X" + (object) index);
          this.SetCurrentNum("QM.X294", num6);
          this.SetCurrentNum("QM.X168", num6);
        }
        if (id == "popup" || id == "QM.X295" || id == "QM.X296" || id == "QM.X297" || id == "QM.X298" || id == "QM.X299")
        {
          double num7 = 0.0;
          for (int index = 295; index <= 299; ++index)
            num7 += this.FltVal("QM.X" + (object) index);
          this.SetCurrentNum("QM.X300", num7);
          this.SetCurrentNum("QM.X180", num7);
        }
        if (id == "popup" || id == "QM.X301" || id == "QM.X302" || id == "QM.X303" || id == "QM.X304" || id == "QM.X305")
        {
          double num8 = 0.0;
          for (int index = 301; index <= 305; ++index)
            num8 += this.FltVal("QM.X" + (object) index);
          this.SetCurrentNum("QM.X306", num8);
          this.SetCurrentNum("QM.X189", num8);
        }
      }
      double num9 = 0.0;
      for (int index = 163; index <= 167; ++index)
        num9 += this.FltVal("QM.X" + (object) index);
      this.SetCurrentNum("QM.X310", num9);
      double num10 = 0.0;
      for (int index = 168; index <= 172; ++index)
        num10 += this.FltVal("QM.X" + (object) index);
      this.SetCurrentNum("QM.X311", num10);
      double num11 = 0.0;
      for (int index = 173; index <= 181; ++index)
        num11 += this.FltVal("QM.X" + (object) index);
      this.SetCurrentNum("QM.X312", num11);
      double num12 = 0.0;
      for (int index = 182; index <= 190; ++index)
        num12 += this.FltVal("QM.X" + (object) index);
      this.SetCurrentNum("QM.X313", num12);
      double num13 = this.FltVal("QM.X153");
      BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
      for (int index = 0; index < borrowerPairs.Length; ++index)
      {
        num13 += this.FltVal("QM.X144", borrowerPairs[index]) + this.FltVal("QM.X152", borrowerPairs[index]) + this.FltVal("QM.X308", borrowerPairs[index]) + this.FltVal("QM.X309", borrowerPairs[index]) + this.FltVal("QM.X310", borrowerPairs[index]) + this.FltVal("QM.X311", borrowerPairs[index]) + this.FltVal("QM.X312", borrowerPairs[index]) + this.FltVal("QM.X313", borrowerPairs[index]);
        if (this.Val("QM.X335", borrowerPairs[index]) == "Y")
          num13 -= this.FltVal("QM.X166", borrowerPairs[index]) + this.FltVal("QM.X171", borrowerPairs[index]);
        if (this.Val("QM.X336", borrowerPairs[index]) == "Y")
          num13 -= this.FltVal("QM.X167", borrowerPairs[index]) + this.FltVal("QM.X172", borrowerPairs[index]);
      }
      this.SetCurrentNum("QM.X154", num13);
      double num14 = 0.0;
      for (int index = 192; index <= 196; index += 2)
        num14 += this.FltVal("QM.X" + (object) index);
      this.SetCurrentNum("QM.X197", num14);
    }

    private void calculateDiscountPoints(string id, string val)
    {
      this.SetCurrentNum("NEWHUD.X1723", this.FltVal("NEWHUD.X1721") / 100.0 * this.FltVal("2") + this.FltVal("NEWHUD.X1722"));
      double num1 = this.FltVal("2");
      if (num1 == 0.0)
        this.SetVal("NEWHUD.X1727", "");
      else
        this.SetVal("NEWHUD.X1727", (this.FltVal("NEWHUD2.X937") / num1 * 100.0).ToString());
      if (this.Val("NEWHUD.X1067") != "Y")
      {
        this.SetVal("QM.X364", "0.000");
        this.SetVal("QM.X367", "0.000");
        this.SetVal("QM.X368", "0.000");
        this.SetVal("QM.X369", "0.000");
        this.SetVal("QM.X370", "0.00");
      }
      else
      {
        double num2 = double.MaxValue;
        double num3 = this.FltVal("3134");
        double num4 = this.FltVal("NEWHUD.X1720");
        double num5 = this.FltVal("3293");
        double num6 = this.FltVal("3");
        double num7 = num4;
        if (num7 <= 0.0)
          num7 = num5;
        if (num7 < 0.0)
          num7 = 0.0;
        if (num3 <= 0.0 || num7 <= 0.0 || num7 > 2.0 + num3 || num7 > 0.001 && num7 <= this.FltVal("3"))
        {
          this.SetVal("QM.X364", "0.000");
          num2 = 0.0;
        }
        else if (num7 <= 1.0 + num3)
        {
          this.SetCurrentNum("QM.X364", 2.0);
          num2 = 2.0;
        }
        else if (num7 > 1.0 + num3 && num7 <= 2.0 + num3)
        {
          this.SetCurrentNum("QM.X364", 1.0);
          num2 = 1.0;
        }
        bool flag = this.Val("NEWHUD.X1139") == "Y";
        double num8 = this.FltVal("2");
        if (num7 == 0.0 || num4 > 0.001 && num4 <= num6 || num4 < 0.0 || num5 < 0.0 || num4 == 0.0 && num5 <= num6)
        {
          this.SetVal("QM.X368", "0.000");
          num2 = 0.0;
        }
        else if (num7 > num6 && num7 > 0.001)
        {
          double num9 = flag ? this.FltVal("NEWHUD.X1727") : this.FltVal("1061");
          double num10 = this.FltVal("NEWHUD.X1721");
          if (num8 != 0.0)
          {
            if (flag && this.FltVal("NEWHUD.X1227") != 0.0)
              num9 = Utils.ArithmeticRounding(this.FltVal("NEWHUD.X1151") / num8 * 100.0, 3);
            else if (!flag && this.FltVal("436") != 0.0)
              num9 = Utils.ArithmeticRounding(this.FltVal("NEWHUD.X15") / num8 * 100.0, 3);
            if (this.FltVal("NEWHUD.X1722") != 0.0)
              num10 = this.FltVal("NEWHUD.X1723") / num8 * 100.0;
          }
          double num11 = Utils.ArithmeticRounding(num9 - num10, 3);
          if (num11 < 0.0)
            num11 = 0.0;
          if (num11 == 0.0)
            this.SetVal("QM.X368", "0.000");
          else
            this.SetCurrentNum("QM.X368", num11);
          if (num11 < num2)
            num2 = num11;
          if (num2 > 2.0)
            num2 = 2.0;
        }
        else
        {
          this.SetVal("QM.X368", "0.000");
          num2 = 0.0;
        }
        double num12 = this.FltVal("QM.X365");
        double num13 = this.FltVal("QM.X366");
        double num14 = this.FltVal("QM.X368");
        double num15;
        if (num13 != 0.0)
        {
          if (num7 - num6 < Utils.ArithmeticRounding(num12 * num14 / num13, 3))
          {
            this.SetVal("QM.X367", "0.000");
            num15 = 0.0;
          }
          else
          {
            this.SetCurrentNum("QM.X367", num14);
            num15 = num14;
          }
        }
        else
        {
          this.SetVal("QM.X367", "0.000");
          num15 = 0.0;
        }
        if (num15 > 0.0)
        {
          if (num15 < num2)
            num2 = num15;
        }
        else if (num12 > 0.0 && num13 > 0.0)
          num2 = 0.0;
        double num16 = flag ? this.FltVal("NEWHUD.X1151") : this.FltVal("NEWHUD.X15");
        this.FltVal("NEWHUD.X1723");
        if (num16 == 0.0 || num7 == 0.0 || num7 <= num6)
        {
          this.SetVal("QM.X369", "0.000");
          this.SetVal("QM.X370", "0.00");
        }
        else
        {
          if (num2 > 2.0)
            num2 = 2.0;
          if (num2 == double.MaxValue || num2 == 0.0)
            this.SetVal("QM.X369", "0.000");
          else
            this.SetCurrentNum("QM.X369", num2);
          double val1 = this.FltVal("QM.X369") / 100.0 * this.FltVal("2");
          if (val1 != 0.0)
            this.SetCurrentNum("QM.X370", Utils.ArithmeticRounding(val1, 2));
          else
            this.SetVal("QM.X370", "0.00");
        }
      }
    }

    private void CalculateDisclosureLog2015(string id, string val)
    {
      this.loan.Calculator.CalculateLatestDisclosure2015((IDisclosureTracking2015Log) null);
    }

    private void populateQMStatusInNMLSForm(string id, string val)
    {
      switch (this.Val("QM.X23"))
      {
        case "Qualified Mortgage":
          this.SetVal("HMDA.X26", "QM");
          break;
        case "General ATR":
          this.SetVal("HMDA.X26", "Non-QM");
          break;
        case "Exempt":
        case "Non-Standard to Standard Refinance":
          this.SetVal("HMDA.X26", "Exempt");
          break;
        default:
          this.SetVal("HMDA.X26", "");
          break;
      }
    }

    internal List<string[]> GetATRQMPaymentSchedule(bool getPaymentSchedule)
    {
      if (!this.isLoanApplicableForQMAPR())
      {
        string val = this.Val("799");
        this.SetCurrentNum("QM.X381", this.Flt(val), !string.IsNullOrEmpty(val) && this.UseNoPayment(this.Flt(val)));
        return (List<string[]>) null;
      }
      if (this.EffectiveRateForQMAPR <= 0.0 & getPaymentSchedule)
        this.calObjs.NewHudCal.CalculateProjectedPaymentTable(false);
      if (this.EffectiveRateForQMAPR <= 0.0)
      {
        string val = this.Val("799");
        this.SetCurrentNum("QM.X381", this.Flt(val), !string.IsNullOrEmpty(val) && this.UseNoPayment(this.Flt(val)));
        return (List<string[]>) null;
      }
      PaymentScheduleSnapshot scheduleSnapshot = (PaymentScheduleSnapshot) null;
      try
      {
        bool calculationDiagnostic = EnConfigurationSettings.GlobalSettings.AllowCalculationDiagnostic;
        if (calculationDiagnostic)
          EnConfigurationSettings.GlobalSettings.AllowCalculationDiagnostic = false;
        LoanData loanData1 = new LoanData(this.loan.XmlDocClone, this.loan.Settings, false, true);
        LoanCalculator loanCalculator = new LoanCalculator(this.sessionObjects, this.calObjs.LoanConfiguration, loanData1, true, this.calObjs.ExternalLateFeeSettings, true);
        loanData1.Calculator.UseBestCaseScenario = false;
        loanData1.Calculator.UseWorstCaseScenario = false;
        double effectiveRateForQmapr;
        if (this.Val("19") == "ConstructionToPermanent")
        {
          loanData1.SetCurrentField("CONST.X13", "Fixed");
          LoanData loanData2 = loanData1;
          effectiveRateForQmapr = this.EffectiveRateForQMAPR;
          string val = effectiveRateForQmapr.ToString();
          loanData2.SetField("1677", val);
        }
        loanData1.SetField("608", "Fixed");
        LoanData loanData3 = loanData1;
        effectiveRateForQmapr = this.EffectiveRateForQMAPR;
        string val1 = effectiveRateForQmapr.ToString();
        loanData3.SetField("3", val1);
        if (getPaymentSchedule)
          scheduleSnapshot = loanData1.Calculator.GetPaymentSchedule(false);
        if (calculationDiagnostic)
          EnConfigurationSettings.GlobalSettings.AllowCalculationDiagnostic = calculationDiagnostic;
        this.SetVal("QM.X381", loanData1.GetField("799"));
      }
      catch (Exception ex)
      {
        Tracing.Log(ATRQMCalculation.sw, TraceLevel.Error, nameof (ATRQMCalculation), "Error calculating QM APR: " + ex.Message);
        return (List<string[]>) null;
      }
      return !getPaymentSchedule ? (List<string[]>) null : this.convertScheduleToGenericObject(scheduleSnapshot.MonthlyPayments);
    }

    private List<string[]> convertScheduleToGenericObject(PaymentSchedule[] qmPayments)
    {
      if (qmPayments == null || qmPayments.Length == 0)
        return (List<string[]>) null;
      List<string[]> genericObject = new List<string[]>();
      for (int index = 0; index < qmPayments.Length && qmPayments[index] != null; ++index)
        genericObject.Add(new List<string>()
        {
          qmPayments[index].PayDate,
          qmPayments[index].CurrentRate.ToString("N3"),
          qmPayments[index].Payment.ToString("N2"),
          qmPayments[index].Principal.ToString("N2"),
          qmPayments[index].Interest.ToString("N2"),
          qmPayments[index].MortgageInsurance.ToString("N2"),
          qmPayments[index].Balance.ToString("N2")
        }.ToArray());
      return genericObject;
    }

    private bool isLoanApplicableForQMAPR()
    {
      if (this.Val("1172") != "Conventional")
        return false;
      switch (this.Val("19"))
      {
        case "ConstructionOnly":
          return false;
        case "ConstructionToPermanent":
          if (this.IntVal("1176") > 12)
            return false;
          break;
      }
      return !(this.Val("608") != "AdjustableRate") && this.IntVal("696") <= 60;
    }

    internal bool UsePriceBasedQM(DateTime originationDate)
    {
      if (!(originationDate == DateTime.MinValue))
      {
        DateTime basedQmStartDate = this.policyUsePriceBasedQMStartDate;
        if (!(this.policyUsePriceBasedQMStartDate == DateTime.MinValue) && DateTime.Compare(this.policyUsePriceBasedQMStartDate, originationDate) <= 0)
          return true;
      }
      return false;
    }

    private void calculateQMX383(string id, string value)
    {
      if (!(id == "745") || this.UsePriceBasedQM(Utils.ParseDate((object) value)))
        return;
      this.SetVal("QM.X383", "N");
    }
  }
}
