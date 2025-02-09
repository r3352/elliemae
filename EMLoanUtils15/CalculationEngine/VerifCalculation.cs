// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.CalculationEngine.VerifCalculation
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Bpm;
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
  internal class VerifCalculation : CalculationBase
  {
    private const string className = "VerifCalculation�";
    private static readonly string sw = Tracing.SwDataEngine;
    private SessionObjects sessionObjects;
    internal Routine CalcMonthlyIncome;
    internal Routine CalcAllMonthlyIncome;
    internal Routine CalcLiabilities;
    internal Routine CalcHelocLineTotal;
    internal Routine CalcOtherLiabilityMonthlyIncome;
    internal Routine CalcOtherIncome;
    internal Routine CalcVOMOwnedBy;
    internal Routine CalcAdditionalLoansAmount;
    internal Routine CalcAppliedToDownpayment;
    internal Routine CalcPreviousGrossIncome;
    internal Routine CalSubFin;
    internal Routine CalcMaintenanceExpenseAmount;
    internal Routine CalcPaceLoanAmounts;
    internal Routine CalcDisasterDeclared;
    internal Dictionary<string, int> triggerList = new Dictionary<string, int>()
    {
      {
        "3",
        1
      },
      {
        "4",
        2
      },
      {
        "19",
        3
      },
      {
        "120",
        4
      },
      {
        "121",
        5
      },
      {
        "136",
        6
      },
      {
        "228",
        7
      },
      {
        "229",
        8
      },
      {
        "325",
        9
      },
      {
        "420",
        10
      },
      {
        "423",
        11
      },
      {
        "682",
        12
      },
      {
        "1014",
        13
      },
      {
        "1109",
        14
      },
      {
        "1172",
        15
      },
      {
        "1176",
        16
      },
      {
        "1177",
        17
      },
      {
        "1335",
        18
      },
      {
        "1771",
        19
      },
      {
        "1724",
        20
      },
      {
        "1725",
        21
      },
      {
        "1811",
        22
      },
      {
        "1825",
        23
      },
      {
        "1853",
        24
      },
      {
        "4464",
        25
      },
      {
        "4465",
        26
      },
      {
        "4466",
        27
      },
      {
        "4467",
        28
      },
      {
        "4469",
        29
      },
      {
        "4470",
        30
      },
      {
        "4471",
        31
      },
      {
        "4473",
        32
      },
      {
        "4491",
        33
      },
      {
        "4531",
        34
      },
      {
        "SYS.X2",
        35
      },
      {
        "CONST.X1",
        36
      },
      {
        "IMPORT",
        37
      }
    };
    private string verifContactID = string.Empty;

    internal VerifCalculation(
      SessionObjects sessionObjects,
      LoanData l,
      EllieMae.EMLite.CalculationEngine.CalculationObjects calObjs)
      : base(l, calObjs)
    {
      this.sessionObjects = sessionObjects;
      this.CalcMonthlyIncome = this.RoutineX(new Routine(this.calculateMonthlyIncome));
      this.CalcAllMonthlyIncome = this.RoutineX(new Routine(this.calculateAllMonthlyIncome));
      this.CalcLiabilities = this.RoutineX(new Routine(this.CalculateLiabilities));
      this.CalcHelocLineTotal = this.RoutineX(new Routine(this.calculateHelocLineTotal));
      this.CalcOtherLiabilityMonthlyIncome = this.RoutineX(new Routine(this.calculateOtherLiabilityMonthlyIncome));
      this.CalcOtherIncome = this.RoutineX(new Routine(this.calculateOtherIncome));
      this.CalcAdditionalLoansAmount = this.RoutineX(new Routine(this.calculateAdditionalLoansAmount));
      this.CalcAppliedToDownpayment = this.RoutineX(new Routine(this.calculateAppliedToDownpayment));
      this.CalcPreviousGrossIncome = this.RoutineX(new Routine(this.calculatePreviousGrossIncome));
      this.CalcVOMOwnedBy = this.RoutineX(new Routine(this.calculateVOMOwnedBy));
      l.VerificationsChanged += new LoanVerifOperationEventHandler(this.onVerifOperation);
      this.CalSubFin = this.RoutineX(new Routine(this.calculateSubFin));
      this.CalcMaintenanceExpenseAmount = this.RoutineX(new Routine(this.calculateMaintenanceExpenseAmount));
      this.CalcPaceLoanAmounts = this.RoutineX(new Routine(this.calculatePaceLoanAmounts));
      this.CalcDisasterDeclared = this.RoutineX(new Routine(this.calculateDisasterDeclared));
      this.addFieldHandlers();
    }

    private void addFieldHandlers()
    {
      Routine routine1 = this.RoutineX(new Routine(this.CalculateDeposits));
      this.AddFieldHandler("DD0011", routine1);
      this.AddFieldHandler("DD0015", routine1);
      this.AddFieldHandler("DD0019", routine1);
      this.AddFieldHandler("DD0023", routine1);
      this.AddFieldHandler("DD0008", routine1);
      this.AddFieldHandler("DD0012", routine1);
      this.AddFieldHandler("DD0016", routine1);
      this.AddFieldHandler("DD0020", routine1);
      this.AddFieldHandler("DD0048", routine1);
      this.AddFieldHandler("DD0049", routine1);
      this.AddFieldHandler("DD0050", routine1);
      this.AddFieldHandler("DD0051", routine1);
      this.AddFieldHandler("URLAROA0003", routine1);
      this.AddFieldHandler("URLAROA0002", routine1);
      this.AddFieldHandler("URLARGG0019", routine1);
      this.AddFieldHandler("URLARGG0018", routine1);
      this.AddFieldHandler("URLARGG0020", routine1);
      this.AddFieldHandler("URLARGG0021", routine1);
      Routine routine2 = this.RoutineX(new Routine(this.CalculateVOM));
      this.AddFieldHandler("FM0014", routine2);
      this.AddFieldHandler("FM0016", routine2);
      this.AddFieldHandler("FM0020", routine2);
      this.AddFieldHandler("FM0026", routine2);
      this.AddFieldHandler("FM0017", routine2);
      this.AddFieldHandler("FM0019", routine2);
      this.AddFieldHandler("FM0024", routine2);
      this.AddFieldHandler("FM0032", routine2);
      this.AddFieldHandler("FM0055", routine2);
      this.AddFieldHandler("FM0018", routine2);
      Routine routine3 = this.RoutineX(new Routine(this.calculateMonthlyIncome));
      this.AddFieldHandler("BE0019", routine3);
      this.AddFieldHandler("BE0020", routine3);
      this.AddFieldHandler("BE0021", routine3);
      this.AddFieldHandler("BE0022", routine3);
      this.AddFieldHandler("BE0023", routine3);
      this.AddFieldHandler("CE0019", routine3);
      this.AddFieldHandler("CE0020", routine3);
      this.AddFieldHandler("CE0021", routine3);
      this.AddFieldHandler("CE0022", routine3);
      this.AddFieldHandler("CE0023", routine3);
      this.AddFieldHandler("FE0119", routine3);
      this.AddFieldHandler("FE0120", routine3);
      this.AddFieldHandler("FE0121", routine3);
      this.AddFieldHandler("FE0122", routine3);
      this.AddFieldHandler("FE0123", routine3);
      this.AddFieldHandler("FE0219", routine3);
      this.AddFieldHandler("FE0220", routine3);
      this.AddFieldHandler("FE0221", routine3);
      this.AddFieldHandler("FE0222", routine3);
      this.AddFieldHandler("FE0223", routine3);
      this.AddFieldHandler("FE0319", routine3);
      this.AddFieldHandler("FE0320", routine3);
      this.AddFieldHandler("FE0321", routine3);
      this.AddFieldHandler("FE0322", routine3);
      this.AddFieldHandler("FE0323", routine3);
      this.AddFieldHandler("FE0419", routine3);
      this.AddFieldHandler("FE0420", routine3);
      this.AddFieldHandler("FE0421", routine3);
      this.AddFieldHandler("FE0422", routine3);
      this.AddFieldHandler("FE0423", routine3);
      Routine routine4 = this.RoutineX(new Routine(this.calculateMilitaryEmployment));
      this.AddFieldHandler("BE0063", routine4);
      this.AddFieldHandler("CE0063", routine4);
      Routine routine5 = this.RoutineX(new Routine(this.setVerificationContactInformation));
      this.AddFieldHandler("FL0038", routine5);
      this.AddFieldHandler("FL0064", routine5);
      this.AddFieldHandler("FM0038", routine5);
      this.AddFieldHandler("FM0064", routine5);
      this.AddFieldHandler("BR0038", routine5);
      this.AddFieldHandler("BR0064", routine5);
      this.AddFieldHandler("CR0038", routine5);
      this.AddFieldHandler("CR0064", routine5);
      this.AddFieldHandler("DD0038", routine5);
      this.AddFieldHandler("DD0064", routine5);
      this.AddFieldHandler("BE0038", routine5);
      this.AddFieldHandler("BE0064", routine5);
      this.AddFieldHandler("CE0038", routine5);
      this.AddFieldHandler("CE0064", routine5);
      this.AddFieldHandler("URLAROA0020", routine5);
      this.AddFieldHandler("URLAROA0015", routine5);
      this.AddFieldHandler("URLAROL0015", routine5);
      this.AddFieldHandler("URLAROL0016", routine5);
      this.AddFieldHandler("URLAROL0017", routine5);
      this.AddFieldHandler("URLAROL0064", routine5);
      this.AddFieldHandler("URLARGG0015", routine5);
      this.AddFieldHandler("URLARGG0064", routine5);
      this.AddFieldHandler("URLAROIS0015", routine5);
      this.AddFieldHandler("URLAROIS0064", routine5);
      this.AddFieldHandler("URLARAL0012", routine5);
      this.AddFieldHandler("URLARAL0064", routine5);
      this.AddFieldHandler("URLA.X230", this.RoutineX(this.CalSubFin));
      this.AddFieldHandler("URLAROIS0018", this.RoutineX(new Routine(this.clearOtherIncomeDescription)));
      this.AddFieldHandler("FM0027", this.RoutineX(new Routine(this.calculateMaintenanceExpenseAmount)));
      Routine routine6 = this.RoutineX(new Routine(this.calculatePaceLoanAmounts));
      this.AddFieldHandler("FL0069", routine6);
      this.AddFieldHandler("FL0016", routine6);
      this.AddFieldHandler("FL0027", routine6);
      this.AddFieldHandler("FEMA0006", this.RoutineX(new Routine(this.calculateDisasterDeclared)));
    }

    internal void CalculateMultipleVOMs(string id, string val)
    {
      BorrowerPair currentBorrowerPair = this.loan.CurrentBorrowerPair;
      foreach (BorrowerPair borrowerPair in this.GetBorrowerPairs())
      {
        this.loan.SetBorrowerPair(borrowerPair);
        int numberOfMortgages = this.loan.GetNumberOfMortgages();
        for (int index = 1; index <= numberOfMortgages; ++index)
          this.CalculateVOM("FM" + index.ToString("00") + "24", val);
      }
      this.loan.SetBorrowerPair(currentBorrowerPair);
    }

    internal void CalculateVOM(string id, string val)
    {
      Tracing.Log(VerifCalculation.sw, TraceLevel.Info, nameof (VerifCalculation), "routine: CalculateVOM ID: " + id);
      string str1 = CalculationBase.nil;
      if (id != CalculationBase.nil && id.Length > 3)
        str1 = this.GetVerifBlock(id);
      if (!string.IsNullOrEmpty(str1))
      {
        if (this.USEURLA2020)
        {
          string str2 = this.Val(str1 + "24");
          string str3 = this.Val(str1 + "55");
          string str4 = this.Val(str1 + "18");
          if (str2 == "PendingSale")
          {
            string str5 = this.Val("2553");
            string str6 = this.Val(str1 + "59");
            string str7 = this.Val("748");
            if (str3 == "PrimaryResidence" && str4 != "TwoToFourUnitProperty")
            {
              this.SetCurrentNum(str1 + "32", 0.0);
              return;
            }
            if (!string.IsNullOrEmpty(str6) && (!string.IsNullOrEmpty(str5) && DateTime.Compare(Utils.ParseDate((object) str5), Utils.ParseDate((object) str6)) > 0 || !string.IsNullOrEmpty(str7) && DateTime.Compare(Utils.ParseDate((object) str7), Utils.ParseDate((object) str6)) > 0))
            {
              this.SetCurrentNum(str1 + "32", 0.0);
              return;
            }
          }
          if (str2 == "Sold" || (str2 == "RetainForPrimaryOrSecondaryResidence" || str2 == "PendingSale" || str2 == "RetainForRental") && str3 == "PrimaryResidence" && str4 != "TwoToFourUnitProperty")
          {
            this.SetCurrentNum(str1 + "32", 0.0);
            return;
          }
        }
        double num1 = this.FltVal(str1 + "26") / 100.0;
        if (num1 == 0.0)
          num1 = 1.0;
        double num2 = this.FltVal(str1 + "14") / 100.0;
        if (num2 == 0.0)
          num2 = 1.0;
        double num3 = this.FltVal(str1 + "20") * num1;
        string empty = string.Empty;
        if ((!this.USEURLA2020 ? this.Val(str1 + "41") : this.Val(str1 + "55")) != "PrimaryResidence")
        {
          num3 -= this.FltVal(str1 + "16");
          if (this.ToDouble(this.Val(str1 + "21")) != 0.0)
            num3 -= this.FltVal(str1 + "21");
        }
        double num4 = Utils.ArithmeticRounding(num3 * num2, 2);
        this.SetCurrentNum(str1 + "32", num4);
      }
      if (!(val != "IMPORT"))
        return;
      this.CalcRealEstate(CalculationBase.nil, CalculationBase.nil);
    }

    internal void CalcRealEstate(string id, string val)
    {
      Tracing.Log(VerifCalculation.sw, TraceLevel.Info, nameof (VerifCalculation), "routine: CalcRealEstate ID: " + id);
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
      Dictionary<string, List<int>> reoidFromVoLs = this.getREOIDFromVOLs();
      int numberOfMortgages = this.loan.GetNumberOfMortgages();
      bool useurlA2020 = this.USEURLA2020;
      for (int index = 1; index <= numberOfMortgages; ++index)
      {
        string str1 = "FM" + index.ToString("00");
        string empty = string.Empty;
        string str2 = !useurlA2020 ? this.Val(str1 + "41") : this.Val(str1 + "55");
        num1 += this.FltVal(str1 + "19");
        num2 += this.FltVal(str1 + "17");
        num3 += this.FltVal(str1 + "20");
        num4 += this.FltVal(str1 + "16");
        if (this.ToDouble(this.Val(str1 + "21")) != 0.0)
          num5 += this.FltVal(str1 + "21");
        double num11 = this.FltVal(str1 + "32");
        if (str2 != "SecondHome")
          num6 += num11;
        if (this.Val(str1 + "28") != "Y")
        {
          if (str2 == "SecondHome")
            num10 += num11;
          else
            num7 += num11;
        }
        num9 += this.FltVal(str1 + "17");
        num8 += this.FltVal(str1 + "16");
        if (reoidFromVoLs != null && reoidFromVoLs.Count > 0)
          this.updatePropertyIndicatorInVOLs(this.Val(str1 + "43"), this.Val(str1 + "28"), reoidFromVoLs);
      }
      string propertyType = this.Val("1811");
      double num12 = 0.0;
      if (this.IsPrimaryPair())
        num12 = this.calObjs.D1003Cal.FindCashflowAmount(propertyType);
      double num13 = 0.0;
      if (num12 > 0.0 && propertyType != "PrimaryResidence")
        num13 = num12;
      if (num7 > 0.0)
        num13 += num7;
      if (propertyType != "PrimaryResidence")
      {
        if (num13 <= 0.0)
          this.SetCurrentNum("106", 0.0);
        else
          this.SetCurrentNum("106", num13);
        if (num7 < 0.0)
          this.SetCurrentNum("LOANSUB.X1", num7 * -1.0);
        else
          this.SetCurrentNum("LOANSUB.X1", 0.0);
      }
      else
      {
        double num14 = 0.0;
        if (num12 > 0.0)
          num14 += num12;
        if (num7 > 0.0)
          num14 += num7;
        this.SetCurrentNum("106", num14);
        if (num7 <= 0.0)
          this.SetCurrentNum("LOANSUB.X1", num7 * -1.0);
        else
          this.SetCurrentNum("LOANSUB.X1", 0.0);
      }
      this.calObjs.D1003Cal.CalcBorrowerIncome(id, val);
      this.calObjs.FHACal.CalcMACAWP(id, val);
      this.SetCurrentNum("919", num1);
      this.calObjs.D1003Cal.CalcTotalAssets(id, val);
      this.SetCurrentNum("920", num2);
      this.SetCurrentNum("921", num3);
      this.SetCurrentNum("922", num4);
      this.SetCurrentNum("923", num5);
      this.SetCurrentNum("924", num6);
      this.SetCurrentNum("909", num8);
      this.SetCurrentNum("941", num9);
      if (num10 < 0.0)
        this.SetCurrentNum("1476", num10 * -1.0);
      else
        this.SetCurrentNum("1476", 0.0);
      this.calObjs.D1003URLA2020Cal.CalcBackEndRatio(id, val);
      this.CalculateLiabilities(id, val);
    }

    private void calculateMonthlyIncome(string id, string val)
    {
      Tracing.Log(VerifCalculation.sw, TraceLevel.Info, nameof (VerifCalculation), "routine: calculateMonthlyIncome ID: " + id);
      if (id == null || !id.StartsWith("BE") && !id.StartsWith("CE") && !id.StartsWith("FE"))
        return;
      string verifBlock = this.GetVerifBlock(id);
      double num = 0.0;
      for (int index = 19; index <= 23; ++index)
        num += this.FltVal(verifBlock + index.ToString("00"));
      if (this.Val("1825") == "2020" && this.Val(verifBlock + "63") == "Y")
        num += this.FltVal(verifBlock + "53");
      this.SetNum(verifBlock + "12", num);
    }

    private void calculateMilitaryEmployment(string id, string val)
    {
      if (Tracing.IsSwitchActive(VerifCalculation.sw, TraceLevel.Info))
        Tracing.Log(VerifCalculation.sw, TraceLevel.Info, nameof (VerifCalculation), "routine: calculateMilitaryEmployment ID: " + id);
      if (!(val == "N"))
        return;
      string verifBlock = this.GetVerifBlock(id);
      this.SetCurrentNum(verifBlock + "53", 0.0);
      this.SetCurrentNum(verifBlock + "65", 0.0);
      this.SetCurrentNum(verifBlock + "66", 0.0);
      this.SetCurrentNum(verifBlock + "67", 0.0);
      this.SetCurrentNum(verifBlock + "68", 0.0);
      this.SetCurrentNum(verifBlock + "69", 0.0);
      this.SetCurrentNum(verifBlock + "70", 0.0);
      this.SetCurrentNum(verifBlock + "71", 0.0);
      this.SetCurrentNum(verifBlock + "72", 0.0);
      this.SetCurrentNum(verifBlock + "74", 0.0);
      this.SetCurrentNum(verifBlock + "77", 0.0);
      this.calculateMonthlyIncome(id, val);
    }

    private void calculateAllMonthlyIncome(string id, string val)
    {
      int numberOfEmployer1 = this.loan.GetNumberOfEmployer(true);
      for (int index = 1; index <= numberOfEmployer1; ++index)
        this.calculateMonthlyIncome("BE" + index.ToString("00") + "19", "");
      int numberOfEmployer2 = this.loan.GetNumberOfEmployer(false);
      for (int index = 1; index <= numberOfEmployer2; ++index)
        this.calculateMonthlyIncome("CE" + index.ToString("00") + "19", "");
    }

    internal void UpdateRevolvingLiabilities(bool skipPaymentWithMinimum)
    {
      double revolvingRate = this.FltVal("SYS.X13") / 100.0;
      string empty = string.Empty;
      int exlcudingAlimonyJobExp = this.loan.GetNumberOfLiabilitesExlcudingAlimonyJobExp();
      for (int index = 1; index <= exlcudingAlimonyJobExp; ++index)
        this.calculateRevolving("FL" + index.ToString("00"), revolvingRate, skipPaymentWithMinimum);
      this.calObjs.D1003URLA2020Cal.CalcBackEndRatio((string) null, (string) null);
      this.CalculateLiabilities((string) null, (string) null);
    }

    internal void CalculateLiabilities(string id, string val)
    {
      Tracing.Log(VerifCalculation.sw, TraceLevel.Info, nameof (VerifCalculation), "routine: CalculateLiabilities ID: " + id);
      double num1 = 0.0;
      double num2 = 0.0;
      string str1 = CalculationBase.nil;
      if (id != null && id.StartsWith("FL") && id.Length >= 6)
      {
        string verifBlock = this.GetVerifBlock(id);
        string verifFieldId = this.GetVerifFieldID(id);
        double num3 = this.FltVal(verifBlock + "13");
        switch (verifFieldId)
        {
          case "18":
            if (val == "Y")
            {
              this.SetCurrentNum(verifBlock + "16", num3);
              this.SetVal(verifBlock + "63", "Y");
              break;
            }
            this.SetVal(verifBlock + "16", string.Empty);
            this.SetVal(verifBlock + "63", "N");
            this.SetVal(verifBlock + "61", string.Empty);
            break;
          case "13":
            if (this.Val(verifBlock + "18") == "Y")
            {
              this.SetCurrentNum(verifBlock + "16", num3);
              break;
            }
            this.SetVal(verifBlock + "16", string.Empty);
            break;
          case "08":
            if (val != "OtherLiability")
              this.SetVal(verifBlock + "65", string.Empty);
            if (val != "HELOC" && val != "MortgageLoan")
            {
              this.SetVal(verifBlock + "66", "N");
              break;
            }
            break;
        }
        if (this.Val(verifBlock + "18") != "Y")
          this.SetVal(verifBlock + "62", "");
        else if (this.Val(verifBlock + "18") == "Y" && (verifFieldId == "08" || verifFieldId == "18" && this.Val(verifBlock + "62") == ""))
        {
          switch (this.Val(verifBlock + "08"))
          {
            case "CollectionsJudgementsAndLiens":
              this.SetVal(verifBlock + "62", "CollectionsJudgmentsAndLiens");
              break;
            case "HELOC":
              this.SetVal(verifBlock + "62", "HELOC");
              break;
            case "Installment":
              this.SetVal(verifBlock + "62", "Installment");
              break;
            case "LeasePayments":
              this.SetVal(verifBlock + "62", "LeasePayment");
              break;
            case "MortgageLoan":
              this.SetVal(verifBlock + "62", "MortgageLoan");
              break;
            case "Open30DayChargeAccount":
              this.SetVal(verifBlock + "62", "Open30DayChargeAccount");
              break;
            case "OtherExpense":
            case "OtherLiability":
              this.SetVal(verifBlock + "62", "Other");
              break;
            case "Revolving":
              this.SetVal(verifBlock + "62", "Revolving");
              break;
            case "Taxes":
              this.SetVal(verifBlock + "62", "Taxes");
              break;
            default:
              this.SetVal(verifBlock + "62", "");
              break;
          }
        }
        if (verifFieldId == "13" || verifFieldId == "08")
          this.calculateRevolving(verifBlock, this.FltVal("SYS.X13") / 100.0, false);
        this.recalculateVOM(this.Val(verifBlock + "25"));
        this.calObjs.Cal.CalcVerifAccountName(id, val);
      }
      else if (id != null && id.StartsWith("FM") && id.Length >= 6 && id.EndsWith("28"))
        this.recalculateVOM(this.Val(id.Substring(0, id.Length > 6 ? 5 : 4) + "43"));
      else if (id != null && (id.StartsWith("URLARAL") || this.triggerList.ContainsKey(id)))
        this.recalculateVOMsFromLiabilities(id);
      int exlcudingAlimonyJobExp1 = this.loan.GetNumberOfLiabilitesExlcudingAlimonyJobExp();
      string nil = CalculationBase.nil;
      if (id == "SYS.X13")
      {
        double revolvingRate = this.FltVal("SYS.X13") / 100.0;
        for (int index = 1; index <= exlcudingAlimonyJobExp1; ++index)
          this.calculateRevolving("FL" + index.ToString("00"), revolvingRate, false);
      }
      double num4 = 0.0;
      double num5 = 0.0;
      double num6 = 0.0;
      double num7 = 0.0;
      double num8 = 0.0;
      double num9 = 0.0;
      double num10 = 0.0;
      double num11 = 0.0;
      double num12 = 0.0;
      bool flag = false;
      string str2 = this.Val("19");
      if (str2.IndexOf("Refinance") > -1 || this.Val("Constr.Refi") == "Y" && str2.Contains("Construction"))
        flag = true;
      double num13 = 0.0;
      for (int blockNo = 1; blockNo <= exlcudingAlimonyJobExp1; ++blockNo)
      {
        string str3 = "FL" + blockNo.ToString("00");
        double num14 = this.FltVal(str3 + "11");
        double num15 = this.FltVal(str3 + "13");
        double num16 = this.FltVal(str3 + "16");
        string str4 = this.Val(str3 + "18");
        string str5 = this.Val(str3 + "17");
        num13 += num14;
        str1 = this.Val(str3 + "08");
        if (str5 != "Y")
          num2 += num15;
        if (this.isLiabilityIncluded(blockNo))
        {
          if (str4 != "Y" && str5 != "Y")
          {
            num1 += num14;
            num8 += this.FltVal(str3 + "12") * num14;
          }
          if (str1 != "MortgageLoan" && str1 != "HELOC" && str4 != "Y")
            num12 += num14;
        }
        if (str4 == "Y")
        {
          if (flag)
            num5 += num16;
          num9 += num16;
          num6 += num14;
          num7 += this.FltVal(str3 + "12") * num14;
        }
        if (blockNo > 7)
        {
          num10 += num14;
          num11 += num15;
        }
      }
      double num17 = 0.0;
      double num18 = 0.0;
      if (str2.Contains("Refinance") || str2.Contains("Construction"))
      {
        BorrowerPair currentBorrowerPair = this.loan.CurrentBorrowerPair;
        BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
        double num19 = 0.0;
        double num20 = 0.0;
        for (int index1 = 0; index1 < borrowerPairs.Length; ++index1)
        {
          this.loan.SetBorrowerPair(borrowerPairs[index1]);
          int exlcudingAlimonyJobExp2 = this.loan.GetNumberOfLiabilitesExlcudingAlimonyJobExp();
          double num21 = 0.0;
          for (int index2 = 1; index2 <= exlcudingAlimonyJobExp2; ++index2)
          {
            string str6 = "FL" + index2.ToString("00");
            if (this.Val(str6 + "18", borrowerPairs[index1]) == "Y")
            {
              double num22 = this.FltVal(str6 + "16", borrowerPairs[index1]);
              str1 = this.Val(str6 + "08", borrowerPairs[index1]);
              if ((str1 == "MortgageLoan" || str1 == "HELOC") && this.Val(str6 + "27", borrowerPairs[index1]) == "Y" && this.Val(str6 + "63", borrowerPairs[index1]) == "Y")
                num21 += num22;
            }
            else if (this.Val(str6 + "27") == "Y")
              num18 += this.FltVal(str6 + "13");
            if ((str1 == "MortgageLoan" || str1 == "HELOC") && this.Val(str6 + "27", borrowerPairs[index1]) == "Y")
              num20 += this.FltVal(str6 + "13", borrowerPairs[index1]);
          }
          num19 += num21;
          num17 += num18;
        }
        this.SetCurrentNum("26", num19);
        this.SetCurrentNum("VASUMM.X149", num20);
        this.SetCurrentNum("VASUMM.X126", num17 + this.FltVal("2"));
        this.loan.SetBorrowerPair(currentBorrowerPair);
      }
      else
      {
        this.SetVal("26", string.Empty);
        this.SetVal("VASUMM.X149", string.Empty);
        this.SetVal("VASUMM.X126", string.Empty);
      }
      this.SetCurrentNum("HUD1A.X31", num9);
      this.SetCurrentNum("HUD1A.X32", this.FltVal("2") + this.FltVal("HUD1A.X33") - this.FltVal("L351") - num9);
      this.SetCurrentNum("350", num1 + (this.FltVal("272") + this.FltVal("256") + this.FltVal("1062")));
      this.SetCurrentNum("VALA.X29", num13 + (this.FltVal("256") + this.FltVal("272") + this.FltVal("1062") + this.FltVal("VALA.X30")));
      if (id == "272" || id == "1163")
      {
        this.calObjs.D1003Cal.CalcLoansubAndTSUM(id, val);
        this.calObjs.FHACal.CalcMACAWP(id, val);
      }
      this.SetCurrentNum("733", num2);
      double num23 = (double) this.IntVal("1835") * this.FltVal("272");
      this.SetCurrentNum("3023", num23);
      this.SetCurrentNum("3025", num2 + (num23 + this.FltVal("3024") + this.FltVal("1163")));
      this.calObjs.D1003Cal.CalcNetWorth((string) null, (string) null);
      this.SetCurrentNum("1648", num10);
      this.SetCurrentNum("1649", num11);
      int num24 = 0;
      for (int index = 20; index <= 26; ++index)
      {
        ++num24;
        if (this.Val("VALA.X" + index.ToString()) == "Y")
          num4 += this.FltVal("FL" + num24.ToString("00") + "11");
      }
      if (this.Val("VALA.X27") == "Y")
        num4 += this.FltVal("256") + this.FltVal("272") + this.FltVal("1062") + this.FltVal("VALA.X30");
      if (this.Val("VALA.X28") == "Y")
        num4 += this.FltVal("1648");
      this.SetCurrentNum("198", num4);
      this.calObjs.VACal.CalcVALA((string) null, (string) null);
      this.SetCurrentNum("1092", num5);
      this.calObjs.NewHud2015Cal.CalcLoanEstimateDownPaymentAndFundsForBorrower((string) null, (string) null);
      this.calObjs.NewHud2015Cal.CalcCDPage3StdCashToCloseFinal((string) null, (string) null);
      if (flag)
        this.SetCurrentNum("RE88395.X193", num5);
      else if (str2 == "Purchase")
        this.SetCurrentNum("RE88395.X193", this.FltVal("1335"));
      if (id != "IMPORT")
      {
        if (this.UseNewGFEHUD || this.UseNew2015GFEHUD)
        {
          this.calObjs.NewHudCal.CalcHUD1PG2((string) null, (string) null);
        }
        else
        {
          this.calObjs.GFECal.CalcTotalCosts((string) null, (string) null);
          this.calObjs.D1003URLA2020Cal.CalcOtherCredits((string) null, (string) null);
        }
      }
      this.SetCurrentNum("382", num12 + (this.FltVal("256") + this.FltVal("1062")));
      this.calObjs.D1003Cal.CalcLoansubAndTSUM(id, val);
      this.calObjs.PrequalCal.CalcMiddleFICO(id, val);
      this.SetCurrentNum("PREQUAL.X248", num9);
      this.SetCurrentNum("PREQUAL.X249", num6);
      this.SetCurrentNum("PREQUAL.X253", num7);
      this.SetCurrentNum("PREQUAL.X252", num8);
      this.SetCurrentNum("PREQUAL.X245", this.FltVal("PREQUAL.X249") - this.FltVal("5"));
      this.SetCurrentNum("PREQUAL.X247", this.FltVal("PREQUAL.X253") - this.FltVal("1207"));
    }

    private void calculateRevolving(
      string blockID,
      double revolvingRate,
      bool skipPaymentWithMinimum)
    {
      if (!(this.Val(blockID + "08") == "Revolving"))
        return;
      double num1 = this.FltVal(blockID + "11");
      if (skipPaymentWithMinimum && num1 >= 10.0)
        return;
      this.SetCurrentNum(blockID + "12", 0.0);
      double num2 = this.FltVal(blockID + "13");
      double num3 = Utils.ArithmeticRounding(num2 * revolvingRate, 2);
      if (num2 >= 10.0 && num3 < 10.0 && this.FltVal("SYS.X13") == 5.0)
        num3 = 10.0;
      else if (num2 < 10.0)
        num3 = num2;
      if (this.FltVal("SYS.X13") != 0.0)
        this.SetCurrentNum(blockID + "11", num3);
      else
        this.SetCurrentNum(blockID + "11", 0.0);
    }

    private bool isLiabilityIncluded(int blockNo)
    {
      string str = "FL" + blockNo.ToString("00");
      if (this.Val(str + "17") == "Y")
        return false;
      if (this.Val(str + "08") != "Installment")
        return true;
      double num1 = this.FltVal(str + "12");
      double num2 = this.FltVal(str + "14");
      return num2 < num1 || num2 <= 0.0;
    }

    internal void CalculateDepositsForTogglingURLA2020(string id, string val)
    {
      Tracing.Log(VerifCalculation.sw, TraceLevel.Info, nameof (VerifCalculation), "routine: CalculateDepositsForTogglingURLA2020 ID: " + id);
      this.calculateDepositsOnly(id, val);
    }

    internal void CalculateDeposits(string id, string val)
    {
      Tracing.Log(VerifCalculation.sw, TraceLevel.Info, nameof (VerifCalculation), "routine: CalculateDeposits ID: " + id);
      this.calculateDepositsOnly(id, val);
      this.calObjs.Cal.CalcVerifAccountName(id, val);
    }

    private void calculateDepositsOnly(string id, string val)
    {
      Tracing.Log(VerifCalculation.sw, TraceLevel.Info, nameof (VerifCalculation), "routine: calculateDepositsOnly ID: " + id);
      if (id != null && (id.StartsWith("URLARGG") || id.StartsWith("URLAROA")))
      {
        this.TotalDeposits((string) null, (string) null);
      }
      else
      {
        if (id == null || !id.StartsWith("DD"))
          return;
        string verifBlock = this.GetVerifBlock(id);
        double num = !(this.Val("1825") == "2020") ? this.FltVal(verifBlock + "11") + this.FltVal(verifBlock + "15") + this.FltVal(verifBlock + "19") + this.FltVal(verifBlock + "23") : this.FltVal(verifBlock + "48") + this.FltVal(verifBlock + "49") + this.FltVal(verifBlock + "50") + this.FltVal(verifBlock + "51");
        this.SetCurrentNum(verifBlock + "34", num);
        if (id == null || val == null)
          return;
        this.TotalDeposits((string) null, (string) null);
      }
    }

    internal void TotalDeposits(string id, string val)
    {
      Tracing.Log(VerifCalculation.sw, TraceLevel.Info, nameof (VerifCalculation), "routine: TotalDeposits ID: " + id);
      double num1 = 0.0;
      BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
      string id1 = this.loan.CurrentBorrowerPair.Id;
      BorrowerPair pair = borrowerPairs[0];
      for (int index1 = 0; index1 < borrowerPairs.Length; ++index1)
      {
        this.loan.SetBorrowerPair(borrowerPairs[index1]);
        int numberOfDeposits = this.loan.GetNumberOfDeposits();
        double num2 = 0.0;
        for (int index2 = 1; index2 <= numberOfDeposits; ++index2)
          num2 += this.FltVal("DD" + index2.ToString("00") + "34");
        this.SetCurrentNum("979", num2);
        num1 += num2;
        if (id1 == borrowerPairs[index1].Id)
          pair = borrowerPairs[index1];
      }
      this.loan.SetBorrowerPair(pair);
      this.SetCurrentNum("1547", num1 + this.calculateTotalOtherLiquidAssets());
      this.calObjs.D1003Cal.CalcLiquidAssets(CalculationBase.nil, CalculationBase.nil);
    }

    private double calculateTotalOtherLiquidAssets()
    {
      int index1 = 0;
      string id = this.loan.CurrentBorrowerPair.Id;
      BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
      double otherLiquidAssets = 0.0;
      for (int index2 = 0; index2 < borrowerPairs.Length; ++index2)
      {
        this.loan.SetBorrowerPair(borrowerPairs[index2]);
        int numberOfOtherAssets = this.loan.GetNumberOfOtherAssets();
        for (int index3 = 1; index3 <= numberOfOtherAssets; ++index3)
        {
          string str = "URLAROA" + index3.ToString("00");
          if (this.Val(str + "02") == "PendingNetSaleProceedsFromRealEstateAssets")
            otherLiquidAssets += this.FltVal(str + "03");
        }
        int ofGiftsAndGrants = this.loan.GetNumberOfGiftsAndGrants();
        for (int index4 = 1; index4 <= ofGiftsAndGrants; ++index4)
        {
          string str = "URLARGG" + index4.ToString("00");
          if (this.Val(str + "20") != "Y" && this.Val(str + "18") == "GiftOfCash")
            otherLiquidAssets += this.FltVal(str + "21");
        }
        if (id == borrowerPairs[index2].Id)
          index1 = index2;
      }
      this.loan.SetBorrowerPair(borrowerPairs[index1]);
      return otherLiquidAssets;
    }

    private void calculateHelocLineTotal(string id, string val)
    {
      LoanData loanData = (LoanData) null;
      if (this.loan.LinkSyncType == LinkSyncType.None || this.loan.LinkSyncType == LinkSyncType.PiggybackPrimary)
        loanData = this.loan;
      else if (this.loan.LinkSyncType == LinkSyncType.PiggybackLinked)
        loanData = this.loan.LinkedData;
      bool flag1 = false;
      if (this.loan.LinkSyncType == LinkSyncType.ConstructionPrimary || this.loan.LinkSyncType == LinkSyncType.ConstructionLinked)
      {
        flag1 = true;
        loanData = this.loan;
      }
      if (loanData == null)
        return;
      string field1 = loanData.GetField("1172");
      bool flag2 = loanData.GetField("420") != "SecondLien";
      bool flag3 = loanData.LinkedData != null && loanData.LinkedData.GetField("420") != "SecondLien";
      double num1 = 0.0;
      double num2 = 0.0;
      double num3 = 0.0;
      double num4 = 0.0;
      bool flag4 = false;
      if (field1 != "HELOC")
      {
        if (flag2)
          num1 += Utils.ParseDouble((object) loanData.GetField("1109"));
        else
          num2 += Utils.ParseDouble((object) loanData.GetField("1109"));
      }
      else
      {
        num4 += Utils.ParseDouble((object) loanData.GetField("1888"));
        num3 += Utils.ParseDouble((object) loanData.GetField("1109"));
      }
      if (loanData.LinkedData != null && !flag1)
      {
        if (loanData.LinkedData.GetField("1172") != "HELOC")
        {
          if (flag3)
            num1 += Utils.ParseDouble((object) loanData.LinkedData.GetField("1109"));
          else
            num2 += Utils.ParseDouble((object) loanData.LinkedData.GetField("1109"));
        }
        else
        {
          num3 += Utils.ParseDouble((object) loanData.LinkedData.GetField("1109"));
          num4 += Utils.ParseDouble((object) loanData.LinkedData.GetField("1888"));
        }
      }
      BorrowerPair pair = (BorrowerPair) null;
      BorrowerPair[] borrowerPairs = loanData.GetBorrowerPairs();
      if (borrowerPairs.Length > 1 && loanData.CurrentBorrowerPair.Id != borrowerPairs[0].Id)
      {
        pair = loanData.CurrentBorrowerPair;
        loanData.SetBorrowerPair(borrowerPairs[0]);
      }
      int exlcudingAlimonyJobExp = loanData.GetNumberOfLiabilitesExlcudingAlimonyJobExp();
      for (int index = 1; index <= exlcudingAlimonyJobExp; ++index)
      {
        string str = "FL" + index.ToString("00");
        string field2 = loanData.GetField(str + "08");
        bool flag5 = loanData.GetField(str + "26") == "Y";
        bool flag6 = loanData.GetField(str + "18") == "Y";
        bool flag7 = loanData.GetField(str + "27") == "Y";
        int num5 = Utils.ParseInt((object) loanData.GetField(str + "29"));
        if (((flag4 ? 0 : (field2 == "HELOC" ? 1 : 0)) & (flag7 ? 1 : 0)) != 0 && (!flag6 || flag6 & flag5))
          flag4 = true;
        if (!flag6)
        {
          if (((field2 == "MortgageLoan" ? 1 : (field2 == "Mortgage" ? 1 : 0)) & (flag7 ? 1 : 0)) != 0)
          {
            if (num5 <= 1)
              num1 += Utils.ParseDouble((object) loanData.GetField(str + "13"));
            else
              num2 += Utils.ParseDouble((object) loanData.GetField(str + "13"));
          }
          else if (field2 == "HELOC")
          {
            if (flag7 & flag5)
              num4 += Utils.ParseDouble((object) loanData.GetField(str + "13"));
            if (flag7 && num5 >= 2)
              num3 += Utils.ParseDouble((object) loanData.GetField(str + "31"));
          }
        }
      }
      double num6 = 0.0;
      double num7 = 0.0;
      double num8 = 0.0;
      double num9 = 0.0;
      double num10;
      double num11;
      double val1;
      double val2;
      if (this.USEURLA2020)
      {
        int ofAdditionalLoans = loanData.GetNumberOfAdditionalLoans();
        for (int index = 1; index <= ofAdditionalLoans; ++index)
        {
          string str = "URLARAL" + index.ToString("00");
          string field3 = loanData.GetField(str + "16");
          if (!flag4 && field3 == "HELOC")
            flag4 = true;
          if (!(loanData.GetField(str + "25") == "Y"))
          {
            switch (field3)
            {
              case "Mortgage":
                if (loanData.GetField(str + "17") == "1")
                {
                  num6 += Utils.ParseDouble((object) loanData.GetField(str + "20"));
                  continue;
                }
                num7 += Utils.ParseDouble((object) loanData.GetField(str + "20"));
                continue;
              case "HELOC":
                num8 += Utils.ParseDouble((object) loanData.GetField(str + "21"));
                num9 += Utils.ParseDouble((object) loanData.GetField(str + "20"));
                continue;
              default:
                continue;
            }
          }
        }
        num10 = num1 + num6;
        num11 = num2 + num7;
        val1 = num4 + num8;
        val2 = num3 + num9;
      }
      else
      {
        num10 = num1 + Utils.ParseDouble((object) loanData.GetField("4487"));
        num11 = num2 + Utils.ParseDouble((object) loanData.GetField("4488"));
        val1 = num4 + Utils.ParseDouble((object) loanData.GetField("4489"));
        val2 = num3 + Utils.ParseDouble((object) loanData.GetField("4490"));
      }
      double num12 = Utils.ArithmeticRounding(val1, 2);
      double num13 = Utils.ArithmeticRounding(val2, 2);
      loanData.SetCurrentFieldFromCal("427", num10 != 0.0 ? num10.ToString() : "");
      loanData.SetCurrentFieldFromCal("428", num11 != 0.0 ? num11.ToString() : "");
      loanData.SetCurrentFieldFromCal("CASASRN.X167", num12 != 0.0 || !string.IsNullOrEmpty(loanData.GetField("4489")) ? num12.ToString("N2") : (flag4 ? "0.00" : ""));
      loanData.SetCurrentFieldFromCal("1732", num12 != 0.0 ? num12.ToString("N2") : "");
      loanData.SetCurrentFieldFromCal("CASASRN.X168", num13 != 0.0 ? num13.ToString("N2") : "");
      if (loanData.GetField("2958") != loanData.GetField("420"))
      {
        loanData.SetCurrentFieldFromCal("3036", num10 != 0.0 ? num10.ToString() : "");
        loanData.SetCurrentFieldFromCal("3035", num11 != 0.0 ? num11.ToString() : "");
      }
      else
      {
        loanData.SetCurrentFieldFromCal("3035", num10 != 0.0 ? num10.ToString() : "");
        loanData.SetCurrentFieldFromCal("3036", num11 != 0.0 ? num11.ToString() : "");
      }
      loanData.SetCurrentFieldFromCal("3037", "");
      if (pair != null)
        loanData.SetBorrowerPair(pair);
      double num14 = Utils.ParseDouble((object) loanData.GetField("CASASRN.X167"));
      bool flag8 = loanData.GetField("1172") == "HELOC";
      if (flag8)
      {
        double num15 = Utils.ParseDouble((object) loanData.GetField("4510"));
        double num16 = Utils.ParseDouble((object) loanData.GetField("1888"));
        if (num15 != num16)
          num14 += num15 - num16;
      }
      loanData.SetCurrentFieldFromCal("3846", num14 != 0.0 ? num14.ToString("N2") : "");
      double num17 = Utils.ParseDouble((object) loanData.GetField("CASASRN.X168"));
      if (flag8)
      {
        double num18 = Utils.ParseDouble((object) loanData.GetField("3043"));
        double num19 = Utils.ParseDouble((object) loanData.GetField("1109"));
        if (num18 != num19)
          num17 += num18 - num19;
      }
      loanData.SetCurrentFieldFromCal("4519", num17.ToString("N2"));
      if (loanData.LinkedData != null && !flag1)
      {
        loanData.LinkedData.SetCurrentFieldFromCal("427", num10 != 0.0 ? num10.ToString() : "");
        loanData.LinkedData.SetCurrentFieldFromCal("428", num11 != 0.0 ? num11.ToString() : "");
        loanData.LinkedData.SetCurrentFieldFromCal("CASASRN.X167", num12 != 0.0 || !string.IsNullOrEmpty(loanData.GetField("4489")) ? num12.ToString("N2") : (flag4 ? "0.00" : ""));
        loanData.LinkedData.SetCurrentFieldFromCal("1732", num12 != 0.0 ? num12.ToString("N2") : "");
        loanData.LinkedData.SetCurrentFieldFromCal("CASASRN.X168", num13 != 0.0 ? num13.ToString("N2") : "");
        loanData.LinkedData.SetCurrentFieldFromCal("3037", "");
        double num20 = Utils.ParseDouble((object) loanData.LinkedData.GetField("CASASRN.X167"));
        loanData.LinkedData.SetCurrentFieldFromCal("3846", num20 != 0.0 ? num20.ToString("N2") : "");
        loanData.LinkedData.SetCurrentFieldFromCal("4519", loanData.LinkedData.GetField("CASASRN.X168"));
      }
      loanData.CalculateSubordinate(loanData.GetField("420") == "FirstLien", loanData, loanData.LinkedData, loanData.Calculator != null && loanData.Calculator.IsPiggybackHELOC);
    }

    private void updatePropertyIndicatorInVOLs(
      string selectedREOID,
      string subjectIndicator,
      Dictionary<string, List<int>> allReoIDs)
    {
      if (selectedREOID == CalculationBase.nil || selectedREOID == null || !allReoIDs.ContainsKey(selectedREOID))
        return;
      List<int> allReoId = allReoIDs[selectedREOID];
      for (int index = 0; index < allReoId.Count; ++index)
        this.SetVal("FL" + allReoId[index].ToString("00") + "27", subjectIndicator);
    }

    private Dictionary<string, List<int>> getREOIDFromVOLs()
    {
      int exlcudingAlimonyJobExp = this.loan.GetNumberOfLiabilitesExlcudingAlimonyJobExp();
      Dictionary<string, List<int>> reoidFromVoLs = (Dictionary<string, List<int>>) null;
      for (int index = 1; index <= exlcudingAlimonyJobExp; ++index)
      {
        string key = this.Val("FL" + index.ToString("00") + "25");
        if (!(key == ""))
        {
          if (reoidFromVoLs == null)
            reoidFromVoLs = new Dictionary<string, List<int>>();
          if (!reoidFromVoLs.ContainsKey(key))
            reoidFromVoLs.Add(key, new List<int>());
          reoidFromVoLs[key].Add(index);
        }
      }
      return reoidFromVoLs;
    }

    private void recalculateVOMsFromLiabilities(string id)
    {
      double num1 = 0.0;
      bool useurlA2020 = this.USEURLA2020;
      bool flag = !id.StartsWith("URLARAL");
      BorrowerPair currentBorrowerPair = flag ? this.loan.CurrentBorrowerPair : (BorrowerPair) null;
      BorrowerPair[] borrowerPairs = flag ? this.GetBorrowerPairs() : (BorrowerPair[]) null;
      int num2 = borrowerPairs != null ? borrowerPairs.Length : 1;
      for (int index1 = 0; index1 < num2; ++index1)
      {
        Dictionary<string, double> dictionary1 = new Dictionary<string, double>();
        Dictionary<string, double> dictionary2 = new Dictionary<string, double>();
        if (flag)
          this.loan.SetBorrowerPair(borrowerPairs[index1]);
        int exlcudingAlimonyJobExp = this.loan.GetNumberOfLiabilitesExlcudingAlimonyJobExp();
        for (int index2 = 1; index2 <= exlcudingAlimonyJobExp; ++index2)
        {
          string str = "FL" + index2.ToString("00");
          string key = this.Val(str + "25");
          if (!string.IsNullOrEmpty(key))
          {
            if (!dictionary1.ContainsKey(key))
              dictionary1[key] = 0.0;
            if (!dictionary2.ContainsKey(key))
              dictionary2[key] = 0.0;
            dictionary1[key] += this.FltVal(str + "13");
            dictionary2[key] += this.FltVal(str + "11");
          }
        }
        if (useurlA2020)
          num1 = !(this.Val("1811") == "PrimaryResidence") ? this.FltVal("228") + this.FltVal("229") : this.FltVal("1724") + this.FltVal("1725");
        int numberOfMortgages = this.loan.GetNumberOfMortgages();
        for (int index3 = 1; index3 <= numberOfMortgages; ++index3)
        {
          string str = "FM" + index3.ToString("00");
          string key = this.Val(str + "43");
          if (!string.IsNullOrEmpty(key))
          {
            if (dictionary1.ContainsKey(key))
              this.SetCurrentNum(str + "17", dictionary1[key]);
            double num3 = this.FltVal(str + "16");
            if (dictionary2.ContainsKey(key))
            {
              this.SetCurrentNum(str + "16", this.Val(str + "28") == "Y" & useurlA2020 ? num1 : dictionary2[key]);
              if (num3 != this.FltVal(str + "16"))
                this.CalculateVOM(str + "16", CalculationBase.nil);
            }
            else if (useurlA2020 && this.Val(str + "28") == "Y")
            {
              this.SetCurrentNum(str + "16", num1);
              if (num3 != num1)
                this.CalculateVOM(str + "16", CalculationBase.nil);
            }
          }
        }
      }
      if (!flag)
        return;
      this.loan.SetBorrowerPair(currentBorrowerPair);
    }

    private void recalculateVOM(string selectedREOID)
    {
      if (selectedREOID == CalculationBase.nil || selectedREOID == null)
        return;
      int exlcudingAlimonyJobExp = this.loan.GetNumberOfLiabilitesExlcudingAlimonyJobExp();
      double num1 = 0.0;
      double num2 = 0.0;
      double num3 = 0.0;
      for (int index = 1; index <= exlcudingAlimonyJobExp; ++index)
      {
        string str = "FL" + index.ToString("00");
        if (this.Val(str + "25") == selectedREOID)
        {
          num1 += this.FltVal(str + "13");
          num2 += this.FltVal(str + "11");
        }
      }
      if (this.USEURLA2020)
        num3 = !(this.Val("1811") == "PrimaryResidence") ? this.FltVal("228") + this.FltVal("229") : this.FltVal("1724") + this.FltVal("1725");
      int numberOfMortgages = this.loan.GetNumberOfMortgages();
      for (int index = 1; index <= numberOfMortgages; ++index)
      {
        string str = "FM" + index.ToString("00");
        if (this.Val(str + "43") == selectedREOID)
        {
          this.SetCurrentNum(str + "17", num1);
          this.SetCurrentNum(str + "16", !(this.Val(str + "28") == "Y") || !this.USEURLA2020 ? num2 : num3);
          this.CalculateVOM(str + "16", CalculationBase.nil);
          break;
        }
      }
    }

    public void CalculateNewLoan()
    {
      int exlcudingAlimonyJobExp = this.loan.GetNumberOfLiabilitesExlcudingAlimonyJobExp();
      double num = 0.0;
      for (int index = 1; index <= exlcudingAlimonyJobExp; ++index)
      {
        if (this.Val("FL" + index.ToString("00") + "18") == "Y")
          num += this.FltVal("FL" + index.ToString("00") + "16");
      }
      this.SetNum("1109", this.UseNewGFEHUD || this.UseNew2015GFEHUD ? num + (this.FltVal("NEWHUD.X277") + this.FltVal("PREQUAL.X250")) : num + (this.FltVal("304") + this.FltVal("PREQUAL.X250")));
      this.SetCurrentNum("PREQUAL.X245", this.FltVal("PREQUAL.X249") - this.FltVal("5"));
      this.SetCurrentNum("PREQUAL.X247", this.FltVal("PREQUAL.X253") - this.FltVal("1207"));
    }

    private void onVerifOperation(
      LoanData loan,
      VerifType type,
      int verifIndex,
      VerifOperation op)
    {
      if (type == VerifType.Deposit && op == VerifOperation.Remove || type == VerifType.GiftGrant && op == VerifOperation.Remove)
      {
        this.TotalDeposits((string) null, (string) null);
        this.calObjs.D1003URLA2020Cal.CalcVODTotalDeposits((string) null, (string) null);
      }
      else if (type == VerifType.Liability && op == VerifOperation.Remove)
      {
        if (this.Val("1172") == "HELOC")
          this.calObjs.RegzCal.CalculateHELOCPayment((string) null, (string) null);
        this.calObjs.D1003URLA2020Cal.CalcNonSubjectPropertyDebtsToBePaidOffAmount((string) null, (string) null);
        this.calObjs.D1003URLA2020Cal.CalcBackEndRatio((string) null, (string) null);
        this.CalculateLiabilities((string) null, (string) null);
        this.calObjs.D1003Cal.CalcHousingExp((string) null, (string) null);
        this.calObjs.NewHud2015Cal.CalcCdPage3Liabilities((string) null, (string) null);
        this.calObjs.NewHud2015Cal.CalcLoanEstimateThirdPartyPaymentsNotOtherwiseDisclosed((string) null, (string) null);
        this.calObjs.NewHud2015Cal.CalcCDPage3TotalPurchasePayoffsIncluded((string) null, (string) null);
        this.calObjs.NewHud2015Cal.CalcAdjustmentsUcdTotal((string) null, (string) null);
        this.calObjs.NewHud2015Cal.CalcTotalAdjustmentAndOtherCredits((string) null, (string) null);
        this.calObjs.NewHud2015Cal.CalcLoanEstimateDownPaymentAndFundsForBorrower((string) null, (string) null);
        this.calculateHelocLineTotal((string) null, (string) null);
        this.calObjs.VACal.CalcVACashOutRefinance((string) null, (string) null);
        this.calObjs.FHACal.CalcMAX23K((string) null, (string) null);
        this.calculatePaceLoanAmounts((string) null, (string) null);
        this.calObjs.D1003URLA2020Cal.CalcEstimatedNetMonthlyRent((string) null, (string) null);
      }
      else if (type == VerifType.Mortgage)
      {
        if (op == VerifOperation.Attach)
          this.CalcRealEstate((string) null, (string) null);
        this.calObjs.D1003URLA2020Cal.CalcNonSubjectPropertyDebtsToBePaidOffAmount((string) null, (string) null);
        if (op == VerifOperation.Remove || op == VerifOperation.Detach || op == VerifOperation.Attach)
        {
          if (this.Val("1172") == "HELOC")
            this.calObjs.RegzCal.CalculateHELOCPayment((string) null, (string) null);
          this.loan.Calculator.FormCalculation("FM" + (verifIndex + 1).ToString("00") + "28");
          if (this.Val("1172") == "HELOC")
            this.calObjs.D1003Cal.CalcHousingExp((string) null, (string) null);
        }
        else if (op == VerifOperation.Add || op == VerifOperation.Attach)
          this.loan.Calculator.FormCalculation("FM" + (verifIndex + 1).ToString("00") + "28");
        this.calculateHelocLineTotal((string) null, (string) null);
      }
      else if (type == VerifType.OtherLiability && (op == VerifOperation.Remove || op == VerifOperation.Add))
      {
        this.calculateOtherLiabilityMonthlyIncome((string) null, (string) null);
        this.calObjs.D1003URLA2020Cal.CalcBackEndRatio((string) null, (string) null);
        this.CalculateLiabilities((string) null, (string) null);
      }
      else if (type == VerifType.OtherIncomeSource && (op == VerifOperation.Remove || op == VerifOperation.Add))
      {
        this.calculateOtherIncome((string) null, (string) null);
        this.calObjs.D1003Cal.CalcOtherIncome((string) null, (string) null);
        this.calObjs.D1003URLA2020Cal.CalcBackEndRatio((string) null, (string) null);
        this.calObjs.ComortCal.CalcComortgagor((string) null, (string) null);
      }
      else if (type == VerifType.OtherAsset && op == VerifOperation.Remove)
      {
        this.TotalDeposits((string) null, (string) null);
        this.calObjs.D1003URLA2020Cal.CalcVODTotalDeposits((string) null, (string) null);
        this.calObjs.D1003URLA2020Cal.CalcVOOATotalOtherAssets((string) null, (string) null);
        this.calObjs.D1003URLA2020Cal.CalcTotalOtherAssets((string) null, (string) null);
        this.calObjs.D1003URLA2020Cal.CalcOtherCredits((string) null, (string) null);
        this.calObjs.D1003URLA2020Cal.CalcTotalCredits((string) null, (string) null);
        this.calObjs.GFECal.CalcClosingCosts((string) null, (string) null);
      }
      else if (type == VerifType.AdditionalLoan && (op == VerifOperation.Remove || op == VerifOperation.Add))
      {
        this.calculateAdditionalLoansAmount((string) null, (string) null);
        this.calculateAppliedToDownpayment((string) null, (string) null);
        this.calculateSubFin((string) null, (string) null);
        if (op == VerifOperation.Remove)
        {
          if (this.Val("1172") == "HELOC")
            this.calObjs.RegzCal.CalculateHELOCPayment((string) null, (string) null);
          this.calObjs.D1003Cal.CalcHousingExp("URLARAL" + (verifIndex + 1).ToString("00") + "17", (string) null);
          this.calObjs.D1003URLA2020Cal.CalcTotalMortgageAndTaxes((string) null, (string) null);
          this.calObjs.D1003Cal.CalcHousingExp((string) null, (string) null);
          this.calObjs.D1003URLA2020Cal.CalcEstimatedNetMonthlyRent((string) null, (string) null);
        }
        else
          this.calObjs.D1003Cal.CalcHousingExp((string) null, (string) null);
        this.calObjs.GFECal.CalcTotalCosts((string) null, (string) null);
        this.calculateHelocLineTotal((string) null, (string) null);
        this.calObjs.VACal.CalcVACashOutRefinance((string) null, (string) null);
        this.calObjs.Cal.CalcLTV((string) null, (string) null);
      }
      else if (type == VerifType.Employer)
      {
        if (op == VerifOperation.Remove || op == VerifOperation.Add)
        {
          this.calculatePreviousGrossIncome((string) null, (string) null);
          this.calObjs.D1003URLA2020Cal.CalcAggregateIncome((string) null, (string) null);
          this.calObjs.D1003Cal.CalcOtherIncome((string) null, (string) null);
        }
        if (op == VerifOperation.Add)
        {
          this.calObjs.D1003URLA2020Cal.CalcVOEDoesNotApply("FE0101", (string) null);
          this.calObjs.D1003URLA2020Cal.CalcVOEDoesNotApply("FE0201", (string) null);
        }
      }
      else if (type == VerifType.Residence && op == VerifOperation.Add)
        this.calObjs.D1003URLA2020Cal.CalcPriorResidenceDoesNotApply((string) null, (string) null);
      else if (type == VerifType.Residence && op == VerifOperation.Remove)
      {
        this.calObjs.D1003URLA2020Cal.CalcPresentHousingExpenseRent((string) null, (string) null);
        this.calObjs.D1003Cal.CalcHousingExp((string) null, (string) null);
        this.calObjs.D1003URLA2020Cal.CalcTotalMortgageAndTaxes((string) null, (string) null);
      }
      if (op == VerifOperation.Add)
        this.calObjs.VERIFCal.setVerificationContactInformation(verifIndex, (string) null, (string) null, (string) null);
      if (type != VerifType.GiftGrant || op != VerifOperation.Remove)
        return;
      this.calObjs.D1003Cal.CalcTotalOtherAssets((string) null, (string) null);
      this.calObjs.D1003URLA2020Cal.CalcTotalGiftGrants((string) null, (string) null);
    }

    private void calculateOtherLiabilityMonthlyIncome(string id, string val)
    {
      Tracing.Log(VerifCalculation.sw, TraceLevel.Info, nameof (VerifCalculation), "routine: calculateMonthlyIncome ID: " + id);
      int ofOtherLiability = this.loan.GetNumberOfOtherLiability();
      double num1 = 0.0;
      double num2 = 0.0;
      double num3 = 0.0;
      for (int index = 1; index <= ofOtherLiability; ++index)
      {
        string str1 = "URLAROL" + index.ToString("00");
        string str2 = this.Val(str1 + "01");
        double num4 = this.FltVal(str1 + "03");
        switch (str2)
        {
          case "Both":
            num1 += num4;
            num2 += num4;
            break;
          case "Borrower":
            num1 += num4;
            break;
          case "CoBorrower":
            num2 += num4;
            break;
        }
        num3 += num4;
      }
      this.SetCurrentNum("URLA.X65", num1);
      this.SetCurrentNum("URLA.X66", num2);
      this.SetCurrentNum("URLA.X68", num3);
    }

    private void calculateOtherIncome(string id, string val)
    {
      Tracing.Log(VerifCalculation.sw, TraceLevel.Info, nameof (VerifCalculation), "routine: calculateOtherIncome ID: " + id);
      int otherIncomeSources = this.loan.GetNumberOfOtherIncomeSources();
      double num1 = 0.0;
      double num2 = 0.0;
      for (int index = 1; index <= otherIncomeSources; ++index)
      {
        string str = "URLAROIS" + index.ToString("00");
        switch (this.Val(str + "02"))
        {
          case "CoBorrower":
            num2 += this.FltVal(str + "22");
            break;
          case "Borrower":
            num1 += this.FltVal(str + "22");
            break;
        }
      }
      this.SetCurrentNum("URLA.X42", num1);
      this.SetCurrentNum("URLA.X43", num2);
      this.SetCurrentNum("URLA.X44", this.FltVal("URLA.X42") + this.FltVal("URLA.X43"));
    }

    private void calculateAdditionalLoansAmount(string id, string val)
    {
      Tracing.Log(VerifCalculation.sw, TraceLevel.Info, nameof (VerifCalculation), "routine: calculateAdditionalLoansAmount ID: " + id);
      int ofAdditionalLoans = this.loan.GetNumberOfAdditionalLoans();
      double num = 0.0;
      for (int index = 1; index <= ofAdditionalLoans; ++index)
      {
        string str = "URLARAL" + index.ToString("00");
        num += this.FltVal(str + "20");
      }
      this.SetCurrentNum("URLA.X229", num);
    }

    private void calculateAppliedToDownpayment(string id, string val)
    {
      Tracing.Log(VerifCalculation.sw, TraceLevel.Info, nameof (VerifCalculation), "routine: calculateAppliedToDownpayment ID: " + id);
      int ofAdditionalLoans = this.loan.GetNumberOfAdditionalLoans();
      double num = 0.0;
      for (int index = 1; index <= ofAdditionalLoans; ++index)
      {
        string str = "URLARAL" + index.ToString("00");
        switch (this.Val(str + "16"))
        {
          case "HELOC":
            num += this.FltVal(str + "21");
            break;
          case "Mortgage":
            num += this.FltVal(str + "20");
            break;
          default:
            num += this.FltVal(str + "22");
            break;
        }
      }
      this.SetCurrentNum("URLA.X230", num);
      this.calObjs.D1003URLA2020Cal.CalcTotalNewMortgageLoans(id, val);
    }

    private void calculateSubFin(string id, string val)
    {
      if (!this.USEURLA2020)
        return;
      this.SetCurrentNum("140", this.FltVal("URLA.X230"));
    }

    private void calculatePreviousGrossIncome(string id, string val)
    {
      string str1 = string.Empty;
      string val1 = string.Empty;
      string val2 = string.Empty;
      if (id != null)
        str1 = this.GetVerifBlock(id);
      if (id != null && id.EndsWith("12"))
      {
        int num1 = 1;
        if (str1.StartsWith("BE"))
        {
          int numberOfEmployer = this.loan.GetNumberOfEmployer(true);
          for (int index = 1; index <= numberOfEmployer; ++index)
          {
            if (num1 <= 3 && this.Val("BE" + index.ToString("00") + "09") == "Y")
            {
              if (this.IsLocked("BE" + index.ToString("00") + "12") && !this.IsLocked("FE" + num1.ToString("00") + "12"))
                this.AddLock("FE" + num1.ToString("00") + "12");
              else if (!this.IsLocked("BE" + index.ToString("00") + "12") && this.IsLocked("FE" + num1.ToString("00") + "12"))
                this.RemoveLock("FE" + num1.ToString("00") + "12");
              num1 += 2;
            }
          }
        }
        else if (str1.StartsWith("CE"))
        {
          int num2 = 2;
          int numberOfEmployer = this.loan.GetNumberOfEmployer(false);
          for (int index = 1; index <= numberOfEmployer; ++index)
          {
            if (num2 <= 4 && this.Val("CE" + index.ToString("00") + "09") == "Y")
            {
              if (this.IsLocked("CE" + index.ToString("00") + "12") && !this.IsLocked("FE" + num2.ToString("00") + "12"))
                this.AddLock("FE" + num2.ToString("00") + "12");
              else if (!this.IsLocked("CE" + index.ToString("00") + "12") && this.IsLocked("FE" + num2.ToString("00") + "12"))
                this.RemoveLock("FE" + num2.ToString("00") + "12");
              num2 += 2;
            }
          }
        }
        else if (str1.StartsWith("FE"))
        {
          string str2 = id.Substring(2, 2);
          switch (str2)
          {
            case "01":
            case "03":
              int numberOfEmployer1 = this.loan.GetNumberOfEmployer(true);
              int num3 = 0;
              for (int index = 1; index <= numberOfEmployer1; ++index)
              {
                if (this.Val("BE" + index.ToString("00") + "09") == "Y")
                  ++num3;
                if (str2 == "01" && num3 == 1 || str2 == "03" && num3 == 2)
                {
                  if (this.IsLocked(id) && !this.IsLocked("BE" + index.ToString("00") + "12"))
                  {
                    this.AddLock("BE" + index.ToString("00") + "12");
                    break;
                  }
                  if (!this.IsLocked(id) && this.IsLocked("BE" + index.ToString("00") + "12"))
                  {
                    this.RemoveLock("BE" + index.ToString("00") + "12");
                    break;
                  }
                  break;
                }
              }
              break;
            case "02":
            case "04":
              int numberOfEmployer2 = this.loan.GetNumberOfEmployer(false);
              int num4 = 0;
              for (int index = 1; index <= numberOfEmployer2; ++index)
              {
                if (this.Val("CE" + index.ToString("00") + "09") == "Y")
                  ++num4;
                if (str2 == "02" && num4 == 1 || str2 == "04" && num4 == 2)
                {
                  if (this.IsLocked(id) && !this.IsLocked("CE" + index.ToString("00") + "12"))
                  {
                    this.AddLock("CE" + index.ToString("00") + "12");
                    break;
                  }
                  if (!this.IsLocked(id) && this.IsLocked("CE" + index.ToString("00") + "12"))
                  {
                    this.RemoveLock("CE" + index.ToString("00") + "12");
                    break;
                  }
                  break;
                }
              }
              break;
          }
        }
      }
      int numberOfEmployer3 = this.loan.GetNumberOfEmployer(true);
      if (id == null || str1 == "FE05" || id.StartsWith("BE"))
      {
        for (int index = 1; index <= numberOfEmployer3; ++index)
        {
          if (this.Val("BE" + index.ToString("00") + "09") == "N")
          {
            val1 = !(this.Val("BE" + index.ToString("00") + "15") == "Y") ? this.Val("BE" + index.ToString("00") + "12") : this.Val("BE" + index.ToString("00") + "56");
            break;
          }
        }
        this.SetVal("URLA.X245", val1);
      }
      int numberOfEmployer4 = this.loan.GetNumberOfEmployer(false);
      if (id != null && !(str1 == "FE06") && !id.StartsWith("CE"))
        return;
      for (int index = 1; index <= numberOfEmployer4; ++index)
      {
        if (this.Val("CE" + index.ToString("00") + "09") == "N")
        {
          val2 = !(this.Val("CE" + index.ToString("00") + "15") == "Y") ? this.Val("CE" + index.ToString("00") + "12") : this.Val("CE" + index.ToString("00") + "56");
          break;
        }
      }
      this.SetVal("URLA.X246", val2);
    }

    private void calculateVOMOwnedBy(string id, string val)
    {
      if (this.Val("1825") != "2020")
        return;
      int num1 = -1;
      int num2 = int.Parse(id.Substring(2, 2));
      if (id.Length > 6)
        num2 = int.Parse(id.Substring(2, 3));
      string str = this.Val("FL" + num2.ToString("00") + "25");
      if (str == "")
        return;
      for (int index = 1; index <= this.loan.GetNumberOfMortgages(); ++index)
      {
        if (this.Val("FM" + index.ToString("00") + "43") == str)
        {
          num1 = index;
          break;
        }
      }
      List<int> intList = new List<int>();
      for (int index = 1; index <= this.loan.GetNumberOfLiabilitesExlcudingAlimonyJobExp(); ++index)
      {
        if (this.Val("FL" + index.ToString("00") + "25") == str)
          intList.Add(index);
      }
      string val1 = "";
      bool flag = true;
      if (intList.Count > 0)
      {
        val1 = this.Val("FL" + intList[0].ToString("00") + "15");
        foreach (int num3 in intList)
        {
          if (this.Val("FL" + num3.ToString("00") + "15") != val1)
            flag = false;
        }
      }
      if (flag)
        this.SetVal("FM" + num1.ToString("00") + "46", val1);
      else
        this.SetVal("FM" + num1.ToString("00") + "46", "");
    }

    public bool ValidateRevolvingVOLs()
    {
      int exlcudingAlimonyJobExp = this.loan.GetNumberOfLiabilitesExlcudingAlimonyJobExp();
      double num = this.FltVal("SYS.X13");
      for (int index = 1; index <= exlcudingAlimonyJobExp; ++index)
      {
        if (string.Compare(this.Val("FL" + index.ToString("00") + "08"), "revolving", true) == 0 && (num != 5.0 || this.FltVal("FL" + index.ToString("00") + "13") > 10.0 && this.FltVal("FL" + index.ToString("00") + "11") < 10.0))
          return false;
      }
      return true;
    }

    internal string VerifContactID
    {
      get => this.verifContactID;
      set => this.verifContactID = value;
    }

    internal void SetVerificationTitle(MilestoneFreeRoleLog msfreeRoleLog)
    {
      if (this.verifContactID == "" || msfreeRoleLog == null || string.Compare(this.verifContactID, msfreeRoleLog.RoleID.ToString() + ":", true) != 0)
        return;
      this.setVerificationContactInformation(-1, msfreeRoleLog.RoleName, msfreeRoleLog.LoanAssociatePhone, msfreeRoleLog.LoanAssociateFax);
    }

    internal void SetVerificationTitle(MilestoneLog msLog)
    {
      if (this.verifContactID == "" || msLog == null || !this.verifContactID.EndsWith(":" + msLog.MilestoneID))
        return;
      this.setVerificationContactInformation(-1, msLog.RoleName, msLog.LoanAssociatePhone, msLog.LoanAssociateFax);
    }

    private void setVerificationContactInformation(string id, string val)
    {
      this.setVerificationContactInformation(-1, (string) null, (string) null, (string) null);
    }

    private void setVerificationContactInformation(
      int verifIndex,
      string contactTitle,
      string contactPhone,
      string contactFax)
    {
      string empty = string.Empty;
      this.GetVerificationContactInformation(ref empty, ref contactTitle, ref contactPhone, ref contactFax);
      BorrowerPair currentBorrowerPair = this.loan.CurrentBorrowerPair;
      foreach (BorrowerPair borrowerPair in this.GetBorrowerPairs())
      {
        this.loan.SetBorrowerPair(borrowerPair);
        int exlcudingAlimonyJobExp = this.loan.GetNumberOfLiabilitesExlcudingAlimonyJobExp();
        for (int index = 1; index <= exlcudingAlimonyJobExp; ++index)
        {
          if ((verifIndex == -1 || verifIndex >= 0 && this.Val("FL" + index.ToString("00") + "37") == "") && this.Val("FL" + index.ToString("00") + "38") != "Y" && this.Val("FL" + index.ToString("00") + "64") != "Y")
            this.SetVal("FL" + index.ToString("00") + "37", contactTitle);
          if (verifIndex == -1 || verifIndex >= 0 && this.Val("FL" + index.ToString("00") + "44") == "")
            this.SetVal("FL" + index.ToString("00") + "44", contactPhone);
          if (verifIndex == -1 || verifIndex >= 0 && this.Val("FL" + index.ToString("00") + "45") == "")
            this.SetVal("FL" + index.ToString("00") + "45", contactFax);
        }
        int numberOfMortgages = this.loan.GetNumberOfMortgages();
        for (int index = 1; index <= numberOfMortgages; ++index)
        {
          if ((verifIndex == -1 || verifIndex >= 0 && this.Val("FM" + index.ToString("00") + "37") == "") && this.Val("FM" + index.ToString("00") + "38") != "Y" && this.Val("FM" + index.ToString("00") + "64") != "Y")
            this.SetVal("FM" + index.ToString("00") + "37", contactTitle);
          if (verifIndex == -1 || verifIndex >= 0 && this.Val("FM" + index.ToString("00") + "44") == "")
            this.SetVal("FM" + index.ToString("00") + "44", contactPhone);
          if (verifIndex == -1 || verifIndex >= 0 && this.Val("FM" + index.ToString("00") + "45") == "")
            this.SetVal("FM" + index.ToString("00") + "45", contactFax);
        }
        int numberOfEmployer1 = this.loan.GetNumberOfEmployer(true);
        for (int index = 1; index <= numberOfEmployer1; ++index)
        {
          if ((verifIndex == -1 || verifIndex >= 0 && this.Val("BE" + index.ToString("00") + "37") == "") && this.Val("BE" + index.ToString("00") + "38") != "Y" && this.Val("BE" + index.ToString("00") + "64") != "Y")
            this.SetVal("BE" + index.ToString("00") + "37", contactTitle);
          if (verifIndex == -1 || verifIndex >= 0 && this.Val("BE" + index.ToString("00") + "44") == "")
            this.SetVal("BE" + index.ToString("00") + "44", contactPhone);
          if (verifIndex == -1 || verifIndex >= 0 && this.Val("BE" + index.ToString("00") + "45") == "")
            this.SetVal("BE" + index.ToString("00") + "45", contactFax);
        }
        int numberOfEmployer2 = this.loan.GetNumberOfEmployer(false);
        for (int index = 1; index <= numberOfEmployer2; ++index)
        {
          if ((verifIndex == -1 || verifIndex >= 0 && this.Val("CE" + index.ToString("00") + "37") == "") && this.Val("CE" + index.ToString("00") + "38") != "Y" && this.Val("CE" + index.ToString("00") + "64") != "Y")
            this.SetVal("CE" + index.ToString("00") + "37", contactTitle);
          if (verifIndex == -1 || verifIndex >= 0 && this.Val("CE" + index.ToString("00") + "44") == "")
            this.SetVal("CE" + index.ToString("00") + "44", contactPhone);
          if (verifIndex == -1 || verifIndex >= 0 && this.Val("CE" + index.ToString("00") + "45") == "")
            this.SetVal("CE" + index.ToString("00") + "45", contactFax);
        }
        int numberOfResidence1 = this.loan.GetNumberOfResidence(true);
        for (int index = 1; index <= numberOfResidence1; ++index)
        {
          if ((verifIndex == -1 || verifIndex >= 0 && this.Val("BR" + index.ToString("00") + "37") == "") && this.Val("BR" + index.ToString("00") + "38") != "Y" && this.Val("BR" + index.ToString("00") + "64") != "Y")
            this.SetVal("BR" + index.ToString("00") + "37", contactTitle);
          if (verifIndex == -1 || verifIndex >= 0 && this.Val("BR" + index.ToString("00") + "44") == "")
            this.SetVal("BR" + index.ToString("00") + "44", contactPhone);
          if (verifIndex == -1 || verifIndex >= 0 && this.Val("BR" + index.ToString("00") + "45") == "")
            this.SetVal("BR" + index.ToString("00") + "45", contactFax);
        }
        int numberOfResidence2 = this.loan.GetNumberOfResidence(false);
        for (int index = 1; index <= numberOfResidence2; ++index)
        {
          if ((verifIndex == -1 || verifIndex >= 0 && this.Val("CR" + index.ToString("00") + "37") == "") && this.Val("CR" + index.ToString("00") + "38") != "Y" && this.Val("CR" + index.ToString("00") + "64") != "Y")
            this.SetVal("CR" + index.ToString("00") + "37", contactTitle);
          if (verifIndex == -1 || verifIndex >= 0 && this.Val("CR" + index.ToString("00") + "44") == "")
            this.SetVal("CR" + index.ToString("00") + "44", contactPhone);
          if (verifIndex == -1 || verifIndex >= 0 && this.Val("CR" + index.ToString("00") + "45") == "")
            this.SetVal("CR" + index.ToString("00") + "45", contactFax);
        }
        int numberOfDeposits = this.loan.GetNumberOfDeposits();
        for (int index = 1; index <= numberOfDeposits; ++index)
        {
          if ((verifIndex == -1 || verifIndex >= 0 && this.Val("DD" + index.ToString("00") + "37") == "") && this.Val("DD" + index.ToString("00") + "38") != "Y" && this.Val("DD" + index.ToString("00") + "64") != "Y")
            this.SetVal("DD" + index.ToString("00") + "37", contactTitle);
          if (verifIndex == -1 || verifIndex >= 0 && this.Val("DD" + index.ToString("00") + "44") == "")
            this.SetVal("DD" + index.ToString("00") + "44", contactPhone);
          if (verifIndex == -1 || verifIndex >= 0 && this.Val("DD" + index.ToString("00") + "45") == "")
            this.SetVal("DD" + index.ToString("00") + "45", contactFax);
        }
        int ofGiftsAndGrants = this.loan.GetNumberOfGiftsAndGrants();
        for (int index = 1; index <= ofGiftsAndGrants; ++index)
        {
          if ((verifIndex == -1 || verifIndex >= 0 && this.Val("URLARGG" + index.ToString("00") + "14") == "") && this.Val("URLARGG" + index.ToString("00") + "15") != "Y" && this.Val("URLARGG" + index.ToString("00") + "64") != "Y")
            this.SetVal("URLARGG" + index.ToString("00") + "14", contactTitle);
          if (verifIndex == -1 || verifIndex >= 0 && this.Val("URLARGG" + index.ToString("00") + "16") == "")
            this.SetVal("URLARGG" + index.ToString("00") + "16", contactPhone);
          if (verifIndex == -1 || verifIndex >= 0 && this.Val("URLARGG" + index.ToString("00") + "17") == "")
            this.SetVal("URLARGG" + index.ToString("00") + "17", contactFax);
        }
        int otherIncomeSources = this.loan.GetNumberOfOtherIncomeSources();
        for (int index = 1; index <= otherIncomeSources; ++index)
        {
          if ((verifIndex == -1 || verifIndex >= 0 && this.Val("URLAROIS" + index.ToString("00") + "14") == "") && this.Val("URLAROIS" + index.ToString("00") + "15") != "Y" && this.Val("URLAROIS" + index.ToString("00") + "64") != "Y")
            this.SetVal("URLAROIS" + index.ToString("00") + "14", contactTitle);
          if (verifIndex == -1 || verifIndex >= 0 && this.Val("URLAROIS" + index.ToString("00") + "16") == "")
            this.SetVal("URLAROIS" + index.ToString("00") + "16", contactPhone);
          if (verifIndex == -1 || verifIndex >= 0 && this.Val("URLAROIS" + index.ToString("00") + "17") == "")
            this.SetVal("URLAROIS" + index.ToString("00") + "17", contactFax);
        }
        int numberOfOtherAssets = this.loan.GetNumberOfOtherAssets();
        for (int index = 1; index <= numberOfOtherAssets; ++index)
        {
          if ((verifIndex == -1 || verifIndex >= 0 && this.Val("URLAROA" + index.ToString("00") + "14") == "") && this.Val("URLAROA" + index.ToString("00") + "15") != "Y" && this.Val("URLAROA" + index.ToString("00") + "20") != "Y")
            this.SetVal("URLAROA" + index.ToString("00") + "14", contactTitle);
          if (verifIndex == -1 || verifIndex >= 0 && this.Val("URLAROA" + index.ToString("00") + "16") == "")
            this.SetVal("URLAROA" + index.ToString("00") + "16", contactPhone);
          if (verifIndex == -1 || verifIndex >= 0 && this.Val("URLAROA" + index.ToString("00") + "17") == "")
            this.SetVal("URLAROA" + index.ToString("00") + "17", contactFax);
        }
        int ofOtherLiability = this.loan.GetNumberOfOtherLiability();
        for (int index = 1; index <= ofOtherLiability; ++index)
        {
          if ((verifIndex == -1 || verifIndex >= 0 && this.Val("URLAROL" + index.ToString("00") + "14") == "") && this.Val("URLAROL" + index.ToString("00") + "15") != "Y" && this.Val("URLAROL" + index.ToString("00") + "64") != "Y")
            this.SetVal("URLAROL" + index.ToString("00") + "14", contactTitle);
          if (verifIndex == -1 || verifIndex >= 0 && this.Val("URLAROL" + index.ToString("00") + "16") == "")
            this.SetVal("URLAROL" + index.ToString("00") + "16", contactPhone);
          if (verifIndex == -1 || verifIndex >= 0 && this.Val("URLAROL" + index.ToString("00") + "17") == "")
            this.SetVal("URLAROL" + index.ToString("00") + "17", contactFax);
        }
        int ofAdditionalLoans = this.loan.GetNumberOfAdditionalLoans();
        for (int index = 1; index <= ofAdditionalLoans; ++index)
        {
          if ((verifIndex == -1 || verifIndex >= 0 && this.Val("URLARAL" + index.ToString("00") + "11") == "") && this.Val("URLARAL" + index.ToString("00") + "12") != "Y" && this.Val("URLARAL" + index.ToString("00") + "64") != "Y")
            this.SetVal("URLARAL" + index.ToString("00") + "11", contactTitle);
          if (verifIndex == -1 || verifIndex >= 0 && this.Val("URLARAL" + index.ToString("00") + "13") == "")
            this.SetVal("URLARAL" + index.ToString("00") + "13", contactPhone);
          if (verifIndex == -1 || verifIndex >= 0 && this.Val("URLARAL" + index.ToString("00") + "14") == "")
            this.SetVal("URLARAL" + index.ToString("00") + "14", contactFax);
        }
      }
      this.loan.SetBorrowerPair(currentBorrowerPair);
    }

    private void clearOtherIncomeDescription(string id, string val)
    {
      if (val == "Other")
        return;
      this.SetVal("URLAROIS" + this.GetVerifBlockNo(id, 8) + "19", "");
    }

    internal void GetVerificationContactInformation(
      ref string contactName,
      ref string contactTitle,
      ref string contactPhone,
      ref string contactFax)
    {
      string userId = string.Empty;
      UserInfo userInfo = (UserInfo) null;
      if (string.IsNullOrEmpty(this.verifContactID))
      {
        userInfo = this.sessionObjects.CurrentUser.GetUserInfo();
        if (userInfo != (UserInfo) null)
        {
          contactName = userInfo.FullName;
          contactTitle = userInfo.JobTitle;
          contactPhone = userInfo.Phone;
          contactFax = userInfo.Fax;
          userId = userInfo.Userid;
        }
      }
      else
      {
        if (!this.verifContactID.EndsWith(":"))
        {
          MilestoneLog[] allMilestones = this.loan.GetLogList().GetAllMilestones();
          if (allMilestones != null)
          {
            for (int index = 0; index < allMilestones.Length; ++index)
            {
              if (string.Compare(this.verifContactID, allMilestones[index].RoleID.ToString() + ":" + allMilestones[index].MilestoneID, true) == 0)
              {
                contactName = allMilestones[index].LoanAssociateName;
                contactTitle = allMilestones[index].RoleName;
                contactPhone = allMilestones[index].LoanAssociatePhone;
                contactFax = allMilestones[index].LoanAssociateFax;
                userId = allMilestones[index].LoanAssociateID;
                break;
              }
            }
          }
        }
        if (this.verifContactID.EndsWith(":"))
        {
          MilestoneFreeRoleLog[] milestoneFreeRoles = this.loan.GetLogList().GetAllMilestoneFreeRoles();
          if (milestoneFreeRoles != null)
          {
            for (int index = 0; index < milestoneFreeRoles.Length; ++index)
            {
              if (string.Compare(this.verifContactID, milestoneFreeRoles[index].RoleID.ToString() + ":", true) == 0)
              {
                contactName = milestoneFreeRoles[index].LoanAssociateName;
                contactTitle = milestoneFreeRoles[index].RoleName;
                contactPhone = milestoneFreeRoles[index].LoanAssociatePhone;
                contactFax = milestoneFreeRoles[index].LoanAssociateFax;
                userId = milestoneFreeRoles[index].LoanAssociateID;
                break;
              }
            }
          }
        }
      }
      if (userId == "")
      {
        MilestoneLog[] allMilestones = this.loan.GetLogList().GetAllMilestones();
        RolesMappingInfo rolesMappingInfo = ((IEnumerable<RolesMappingInfo>) this.sessionObjects.StartupInfo.RoleMappings).FirstOrDefault<RolesMappingInfo>((Func<RolesMappingInfo, bool>) (mapInfo => mapInfo.RealWorldRoleID == RealWorldRoleID.LoanProcessor));
        if (rolesMappingInfo != null)
        {
          foreach (MilestoneLog milestoneLog in allMilestones)
          {
            if (milestoneLog.RoleID == rolesMappingInfo.RoleIDList[0])
            {
              contactName = milestoneLog.LoanAssociateName;
              contactTitle = milestoneLog.RoleName;
              contactPhone = milestoneLog.LoanAssociatePhone;
              contactFax = milestoneLog.LoanAssociateFax;
              userId = milestoneLog.LoanAssociateID;
              break;
            }
          }
        }
      }
      if (userId == "")
        userInfo = this.sessionObjects.CurrentUser.GetUserInfo();
      else if (userInfo == (UserInfo) null || userInfo.Userid != userId)
        userInfo = this.sessionObjects.OrganizationManager.GetUser(userId);
      if (userInfo != (UserInfo) null)
        contactTitle = userInfo.JobTitle;
      if (userId == "" && userInfo != (UserInfo) null)
      {
        contactName = userInfo.FullName;
        contactPhone = userInfo.Phone;
        contactFax = userInfo.Fax;
        userId = userInfo.Userid;
      }
      if (userId != string.Empty && userInfo != (UserInfo) null && (contactPhone == string.Empty || contactFax == string.Empty))
      {
        OrgInfo avaliableOrganization = this.sessionObjects.OrganizationManager.GetFirstAvaliableOrganization(userInfo.OrgId, true);
        if (avaliableOrganization != null)
        {
          if ((contactPhone ?? "") == string.Empty)
            contactPhone = avaliableOrganization.CompanyPhone;
          if ((contactFax ?? "") == string.Empty)
            contactFax = avaliableOrganization.CompanyFax;
        }
      }
      if ((contactPhone == string.Empty || contactFax == string.Empty) && this.sessionObjects != null && this.sessionObjects.CompanyInfo != null)
      {
        if ((contactPhone ?? "") == string.Empty)
          contactPhone = this.sessionObjects.CompanyInfo.Phone;
        if ((contactFax ?? "") == string.Empty)
          contactFax = this.sessionObjects.CompanyInfo.Fax;
      }
      this.SetVal("EDS.X1", contactName);
      this.SetVal("EDS.X2", contactTitle);
      this.SetVal("EDS.X3", contactPhone);
      this.SetVal("EDS.X4", contactFax);
    }

    private void calculateMaintenanceExpenseAmount(string id, string val)
    {
      if (!this.USEURLA2020)
        return;
      double num = this.FltVal("230") + this.FltVal("URLA.X144") + this.FltVal("1405") + this.FltVal("232") + this.FltVal("233") + this.FltVal("234");
      if (id != null && id.StartsWith("FM"))
      {
        string verifBlock = this.GetVerifBlock(id);
        if (this.Val(verifBlock + "28") == "Y")
          this.SetCurrentNum(verifBlock + "21", num);
        else
          this.RemoveCurrentLock(verifBlock + "21");
      }
      else
      {
        BorrowerPair currentBorrowerPair = this.loan.CurrentBorrowerPair;
        foreach (BorrowerPair borrowerPair in this.GetBorrowerPairs())
        {
          this.loan.SetBorrowerPair(borrowerPair);
          int numberOfMortgages = this.loan.GetNumberOfMortgages();
          for (int index = 1; index <= numberOfMortgages; ++index)
          {
            string str = "FM" + index.ToString("00");
            if (this.Val(str + "28") == "Y")
              this.SetCurrentNum(str + "21", num);
            else
              this.RemoveCurrentLock(str + "21");
          }
        }
        this.loan.SetBorrowerPair(currentBorrowerPair);
      }
    }

    private void calculatePaceLoanAmounts(string id, string val)
    {
      BorrowerPair currentBorrowerPair = this.loan.CurrentBorrowerPair;
      BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
      double num1 = 0.0;
      double num2 = 0.0;
      for (int index1 = 0; index1 < borrowerPairs.Length; ++index1)
      {
        this.loan.SetBorrowerPair(borrowerPairs[index1]);
        int exlcudingAlimonyJobExp = this.loan.GetNumberOfLiabilitesExlcudingAlimonyJobExp();
        for (int index2 = 1; index2 <= exlcudingAlimonyJobExp; ++index2)
        {
          string str1 = "FL" + index2.ToString("00");
          string str2 = this.Val(str1 + "69");
          if (this.Val(str1 + "27") == "Y" && this.Val(str1 + "18") == "Y" && !string.IsNullOrEmpty(this.Val(str1 + "16")))
          {
            switch (str2)
            {
              case "Y":
                num1 += this.FltVal(str1 + "16");
                continue;
              case "N":
                num2 += this.FltVal(str1 + "16");
                continue;
              default:
                continue;
            }
          }
        }
      }
      this.loan.SetBorrowerPair(currentBorrowerPair);
      this.SetCurrentNum("4785", num2);
      this.SetCurrentNum("4786", num1);
    }

    private void calculateDisasterDeclared(string id, string val)
    {
      if (!(id == "FEMA0106") || !(this.Val(id) != string.Empty) || !(this.Val("4953") == string.Empty))
        return;
      this.SetVal("4953", "Y");
    }
  }
}
