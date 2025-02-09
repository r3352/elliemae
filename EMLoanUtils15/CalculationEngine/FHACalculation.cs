// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.CalculationEngine.FHACalculation
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.LoanUtils.CalculationServants;
using System;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.CalculationEngine
{
  internal class FHACalculation : CalculationBase
  {
    private const string className = "FHACalculation�";
    private static readonly string sw = Tracing.SwDataEngine;
    internal Routine CalcMAX23K;
    internal Routine CalcMACAWP;
    internal Routine CalcCAWRefi;
    internal Routine CalcFredMac;
    internal Routine CalcDraw203K;
    internal Routine CalcEEM;
    internal Routine CalcEEMWorksheet;
    internal Routine CalcEEMX88AndEEMX89;
    internal Routine CalcFHA203K;
    internal Routine CalcExisting23KDebt;
    private SessionObjects sessionObjects;
    private readonly FHACalculationServant fhaCalculationServant;

    internal FHACalculation(SessionObjects sessionObjects, LoanData l, EllieMae.EMLite.CalculationEngine.CalculationObjects calObjs)
      : base(l, calObjs)
    {
      this.sessionObjects = sessionObjects;
      this.CalcMAX23K = this.RoutineX(new Routine(this.calculateMAX23K));
      this.CalcMACAWP = this.RoutineX(new Routine(this.calculateMACAWPurchase));
      this.CalcCAWRefi = this.RoutineX(new Routine(this.calculateMCAWRefinance));
      this.CalcFredMac = this.RoutineX(new Routine(this.calculateFreddieMac));
      this.CalcDraw203K = this.RoutineX(new Routine(this.calculateDraw203K));
      this.CalcEEM = this.RoutineX(new Routine(this.calculateEEM));
      this.CalcEEMWorksheet = this.RoutineX(new Routine(this.calculateEEMWorksheet));
      this.CalcEEMX88AndEEMX89 = this.RoutineX(new Routine(this.calculateEEMX88AndEEMX89));
      this.CalcFHA203K = this.RoutineX(new Routine(this.calculateFHA203K));
      this.CalcExisting23KDebt = this.RoutineX(new Routine(this.calculateExisting23KDebt));
      this.addFieldHandlers();
      this.fhaCalculationServant = new FHACalculationServant((ILoanModelProvider) this);
    }

    private void addFieldHandlers()
    {
      Routine routine1 = this.RoutineX(new Routine(this.calculateMAX23K));
      this.AddFieldHandler("MAX23K.X20", routine1);
      this.AddFieldHandler("3052", routine1);
      this.AddFieldHandler("2996", routine1);
      this.AddFieldHandler("MAX23K.X80", routine1);
      this.AddFieldHandler("MAX23K.X19", routine1);
      this.AddFieldHandler("MAX23K.X113", routine1);
      this.AddFieldHandler("MAX23K.X114", routine1);
      this.AddFieldHandler("MAX23K.X21", routine1);
      this.AddFieldHandler("MAX23K.X81", routine1);
      this.AddFieldHandler("MAX23K.X10", routine1);
      this.AddFieldHandler("MAX23K.X17", routine1);
      this.AddFieldHandler("MAX23K.X26", routine1);
      this.AddFieldHandler("MAX23K.X40", routine1);
      this.AddFieldHandler("MCAWPUR.X29", routine1);
      this.AddFieldHandler("MAX23K.X5", routine1);
      this.AddFieldHandler("MAX23K.X6", routine1);
      this.AddFieldHandler("MAX23K.X89", routine1);
      this.AddFieldHandler("MAX23K.X8", routine1);
      this.AddFieldHandler("MAX23K.X97", routine1);
      this.AddFieldHandler("MAX23K.X95", routine1);
      this.AddFieldHandler("MAX23K.X91", routine1);
      this.AddFieldHandler("MAX23K.X92", routine1);
      this.AddFieldHandler("MAX23K.X93", routine1);
      this.AddFieldHandler("MAX23K.X103", routine1);
      this.AddFieldHandler("MAX23K.X108", routine1);
      this.AddFieldHandler("MAX23K.X117", routine1);
      this.AddFieldHandler("MAX23K.X118", routine1);
      this.AddFieldHandler("MAX23K.X119", routine1);
      this.AddFieldHandler("MAX23K.X120", routine1);
      this.AddFieldHandler("MAX23K.X121", routine1);
      this.AddFieldHandler("MAX23K.X122", routine1);
      this.AddFieldHandler("MAX23K.X123", routine1);
      this.AddFieldHandler("4091", routine1);
      this.AddFieldHandler("MAX23K.X132", routine1);
      Routine routine2 = this.RoutineX(new Routine(this.calculateMACAWPurchase));
      this.AddFieldHandler("61", routine2);
      this.AddFieldHandler("135", routine2);
      this.AddFieldHandler("201", routine2);
      this.AddFieldHandler("386", routine2);
      this.AddFieldHandler("1094", routine2);
      this.AddFieldHandler("1131", routine2);
      this.AddFieldHandler("1140", routine2);
      this.AddFieldHandler("1145", routine2);
      this.AddFieldHandler("1146", routine2);
      this.AddFieldHandler("1720", routine2);
      this.AddFieldHandler("MCAWPUR.X3", routine2);
      this.AddFieldHandler("MCAWPUR.X7", routine2);
      this.AddFieldHandler("MCAWPUR.X14", routine2);
      this.AddFieldHandler("MCAWPUR.X12", routine2);
      this.AddFieldHandler("MCAWPUR.X5", routine2);
      this.AddFieldHandler("MCAWPUR.X29", routine2);
      this.AddFieldHandler("1116", routine2);
      this.AddFieldHandler("4181", routine2);
      this.AddFieldHandler("2999", routine2);
      Routine routine3 = this.RoutineX(new Routine(this.calculateMCAWRefinance));
      this.AddFieldHandler("1137", routine3);
      this.AddFieldHandler("1669", routine3);
      this.AddFieldHandler("1983", routine3);
      this.AddFieldHandler("1518", routine3);
      this.AddFieldHandler("EEM.X63", routine3);
      this.AddFieldHandler("1154", routine3);
      Routine routine4 = this.RoutineX(new Routine(this.calculateFreddieMac));
      this.AddFieldHandler("CASASRN.X19", routine4);
      this.AddFieldHandler("CASASRN.X79", routine4);
      Routine routine5 = this.RoutineX(new Routine(this.calculateEEMWorksheet));
      for (int index = 1; index <= 90; ++index)
        this.AddFieldHandler("EEM.X" + index.ToString(), routine5);
      this.AddFieldHandler("2997", routine5);
      this.AddFieldHandler("1067", routine5);
      this.AddFieldHandler("MAX23K.X73", this.RoutineX(new Routine(this.calculateBorrowerAcknowledgement)));
      Routine routine6 = this.RoutineX(new Routine(this.calculateExisting23KDebt)) + new Routine(this.calculateMAX23K);
      this.AddFieldHandler("MAX23K.X124", routine6);
      this.AddFieldHandler("MAX23K.X125", routine6);
      this.AddFieldHandler("MAX23K.X126", routine6);
      this.AddFieldHandler("MAX23K.X127", routine6);
      this.AddFieldHandler("MAX23K.X128", routine6);
      this.AddFieldHandler("MAX23K.X129", routine6);
      this.AddFieldHandler("MAX23K.X130", routine6);
      this.AddFieldHandler("MAX23K.X131", routine6);
    }

    private void calculateExisting23KDebt(string id, string val)
    {
      this.SetCurrentNum("MAX23K.X132", Utils.ArithmeticRounding(this.FltVal("MAX23K.X131") + this.FltVal("MAX23K.X130") + this.FltVal("MAX23K.X129") + this.FltVal("MAX23K.X128") + this.FltVal("MAX23K.X127") + this.FltVal("MAX23K.X126") + this.FltVal("MAX23K.X125") + this.FltVal("MAX23K.X124"), 2));
    }

    private void calculateMAX23K(string id, string val)
    {
      if (Tracing.IsSwitchActive(FHACalculation.sw, TraceLevel.Info))
        Tracing.Log(FHACalculation.sw, TraceLevel.Info, nameof (FHACalculation), "routine: FHACalculation.calculateMAX23K ID: " + id);
      string str = this.Val("19");
      if (this.Val("1172") == "FHA" && str == "NoCash-Out Refinance")
      {
        if (this.Val("MORNET.X40") == "StreamlineWithoutAppraisal")
          this.SetCurrentNum("3052", this.FltVal("26") - this.FltVal("1134"));
        else
          this.SetCurrentNum("3052", this.FltVal("26") + this.FltVal("137") + this.FltVal("1093") + this.FltVal("138") + this.FltVal("29") - this.FltVal("1134"));
      }
      else if (str.IndexOf("Refinance") == -1 || this.Val("3000") != "Y" && this.Val("2997") != "Y")
        this.SetVal("3052", "");
      if (str.IndexOf("Refinance") > -1)
        this.SetCurrentNum("MAX23K.X40", this.FltVal("MAX23K.X132"));
      else if (str == "Purchase")
        this.SetCurrentNum("MAX23K.X40", this.FltVal("136"));
      this.calculateFHA203K(id, val);
    }

    private void calculateMACAWPurchase(string id, string val)
    {
      if (Tracing.IsSwitchActive(FHACalculation.sw, TraceLevel.Info))
        Tracing.Log(FHACalculation.sw, TraceLevel.Info, nameof (FHACalculation), "routine: FHACalculation.calculateMACAWPurchase ID: " + id);
      bool flag1 = this.Val("CASASRN.X141") == "Borrower";
      bool flag2 = this.Val("COMPLIANCEVERSION.CASASRNX141") == "Y" || this.Val("COMPLIANCEVERSION.NEWBUYDOWNENABLED") != "Y";
      double num1 = this.FltVal("3") - (flag1 | flag2 ? this.FltVal("1269") : this.FltVal("4535"));
      if (num1 < 0.0 || flag1 | flag2 && this.FltVal("1269") == 0.0 || !flag1 && !flag2 && this.FltVal("4535") == 0.0)
        num1 = 0.0;
      this.SetCurrentNum("964", num1);
      int num2 = this.IntVal("4");
      int num3 = this.IntVal("325");
      if (num3 > 0 && num3 < num2)
        num2 = num3;
      int num4 = num2 % 12;
      this.SetCurrentNum("1347", num4 <= 0 ? (double) (num2 / 12) : (double) ((num2 - num4) / 12));
      this.SetCurrentNum("1392", (double) num4);
      double num5 = this.FltVal("136");
      double num6 = this.FltVal("356");
      this.SetCurrentNum("MCAWPUR.X12", num5);
      double num7 = num5 + this.FltVal("MCAWPUR.X3");
      this.SetCurrentNum("MCAWPUR.X28", num7);
      double num8 = this.FltVal("MCAWPUR.X12") + this.FltVal("1132");
      this.SetCurrentNum("MCAWPUR.X1", num8);
      double num9 = 0.035;
      double num10;
      if (num7 < num6 && num7 != 0.0)
      {
        num10 = num9 * num7;
        this.SetCurrentNum("MCAWPUR.X13", num7);
      }
      else if (num6 < num7 && num6 != 0.0)
      {
        num10 = num9 * num6;
        this.SetCurrentNum("MCAWPUR.X13", num6);
      }
      else
      {
        num10 = num9 * this.FltVal("MCAWPUR.X12");
        this.SetCurrentNum("MCAWPUR.X13", this.FltVal("MCAWPUR.X12"));
      }
      this.SetCurrentNum("MCAWPUR.X2", num10);
      double propertyValue = this.FltVal("358");
      if (this.Val("2999") != "Y")
        this.SetCurrentNum("1090", this.FltVal("MCAWPUR.X13") + this.FltVal("MCAWPUR.X29"));
      else
        this.SetCurrentNum("1090", this.FltVal("MCAWPUR.X13"));
      this.SetCurrentNum("MCAWPUR.X5", this.calObjs.PreCal.CalculateFHAMaxLoanAmt(propertyValue, true) * 100.0);
      double num11 = this.FltVal("MCAWPUR.X5") / 100.0 * this.FltVal("1090");
      int num12 = (int) num11;
      if (this.Val("2999") == "Y")
        this.SetCurrentNum("4801", num11);
      if (this.Val("2999") != "Y")
        this.SetCurrentNum("MCAWPUR.X25", (double) num12);
      else
        this.SetCurrentNum("MCAWPUR.X25", (double) num12 + this.FltVal("MCAWPUR.X29"));
      this.SetCurrentNum("MCAWPUR.X14", (double) num12 <= this.FltVal("1109") ? (double) num12 : this.FltVal("1109"));
      double num13 = num8 - this.FltVal("MCAWPUR.X14");
      if (num13 > num10)
        num13 = num10;
      this.SetCurrentNum("1117", num13);
      if (this.Val("1765") == "Y")
      {
        double num14 = this.FltVal("1760") - this.FltVal("562");
        if (num14 < 0.0)
          num14 = 0.0;
        this.SetCurrentNum("3033", num14);
      }
      double num15 = this.FltVal("1117") + this.FltVal("61") + this.FltVal("1046") + this.FltVal("3033") + this.FltVal("MCAWPUR.X7") + this.FltVal("1137");
      this.SetCurrentNum("MCAWPUR.X8", num15);
      if (id == "428" && this.Val("1172") == "FHA" && this.Val("19") == "Purchase")
        this.SetCurrentNum("1140", this.FltVal("428"));
      this.SetCurrentNum("MCAWPUR.X11", this.FltVal("201") + this.FltVal("220") + this.FltVal("1094") + this.FltVal("1140") - num15);
      BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
      for (int index = 0; index < borrowerPairs.Length; ++index)
        this.SetCurrentNum("1761", this.FltVal("101", borrowerPairs[index]) + this.FltVal("936", borrowerPairs[index]) + this.FltVal("110", borrowerPairs[index]) + this.FltVal("938", borrowerPairs[index]) + this.FltVal("906", borrowerPairs[index]), borrowerPairs[index]);
      this.SetCurrentNum("1116", num7 * 0.06);
      double num16 = 0.0;
      if (num7 != 0.0)
        num16 = this.FltVal("135") / num7 * 100.0;
      this.SetCurrentNum("3048", num16);
      for (int index = 0; index < borrowerPairs.Length; ++index)
      {
        double num17 = this.FltVal("MCAWPUR.X13", borrowerPairs[index]);
        if (num17 != 0.0)
          this.SetCurrentNum("MCAWPUR.X21", this.FltVal("MCAWPUR.X14", borrowerPairs[index]) / num17 * 100.0, borrowerPairs[index]);
        else
          this.SetCurrentNum("MCAWPUR.X21", 0.0, borrowerPairs[index]);
        double num18 = this.FltVal("1761", borrowerPairs[index]);
        double num19 = this.FltVal("1724", borrowerPairs[index]) + this.FltVal("1728", borrowerPairs[index]) + this.FltVal("1729", borrowerPairs[index]) + this.FltVal("1730", borrowerPairs[index]) + this.FltVal("1725", borrowerPairs[index]) + this.FltVal("1726", borrowerPairs[index]) + this.FltVal("4947", borrowerPairs[index]) + this.FltVal("1727", borrowerPairs[index]);
        this.SetCurrentNum("739", num19, borrowerPairs[index], this.UseNoPayment(num19));
        if (num18 != 0.0)
          this.SetCurrentNum("MCAWPUR.X22", num19 / num18 * 100.0, borrowerPairs[index]);
        else
          this.SetCurrentNum("MCAWPUR.X22", 0.0, borrowerPairs[index]);
        this.SetCurrentNum("MCAWPUR.X24", num19 + this.FltVal("1150", borrowerPairs[index]), borrowerPairs[index]);
        if (num18 != 0.0)
          this.SetCurrentNum("MCAWPUR.X23", this.FltVal("1742") / num18 * 100.0, borrowerPairs[index]);
        else
          this.SetCurrentNum("MCAWPUR.X23", 0.0, borrowerPairs[index]);
      }
      if (this.Val("19") == "Purchase")
      {
        double num20 = !(this.Val("1172") == "VA") ? this.FltVal("135") - this.FltVal("1116") : this.FltVal("135") - this.FltVal("4180");
        if (num20 < 0.0)
          num20 = 0.0;
        this.SetCurrentNum("3053", num20);
        this.SetVal("1134", "");
      }
      else
        this.SetVal("3053", "");
      this.calculateMCAWRefinance(id, val);
    }

    private void calculateMCAWRefinance(string id, string val)
    {
      if (Tracing.IsSwitchActive(FHACalculation.sw, TraceLevel.Info))
        Tracing.Log(FHACalculation.sw, TraceLevel.Info, nameof (FHACalculation), "routine: FHACalculation.calculateMCAWRefinance ID: " + id);
      string str1 = this.Val("19");
      if (this.Val("4796") != "Y")
        this.fhaCalculationServant.CalculateMCAWRefinance(id, val);
      this.SetCurrentNum("1154", this.calculateRefiHiLo("1154"));
      double num;
      if (this.Val("1172") == "FHA" && str1.IndexOf("Refinance") > -1)
      {
        string str2 = this.Val("MORNET.X40");
        if (str1 == "NoCash-Out Refinance")
        {
          switch (str2)
          {
            case "StreamlineWithoutAppraisal":
              num = this.FltVal("1045") + this.FltVal("3052");
              break;
            case "StreamlineWithAppraisal":
              num = this.FltVal("26") + this.FltVal("1045");
              break;
            default:
              num = this.FltVal("1154") >= this.FltVal("3052") ? this.FltVal("3052") : this.FltVal("1154");
              break;
          }
        }
        else
        {
          num = this.FltVal("1154");
          this.SetCurrentNum("MORNET.X42", this.FltVal("356") > this.FltVal("EEM.X63") ? this.FltVal("EEM.X63") : this.FltVal("356"));
        }
      }
      else
      {
        if (str1.IndexOf("Refinance") == -1)
          this.SetVal("MORNET.X41", "");
        num = this.FltVal("26") + this.FltVal("29") + this.FltVal("1132") - this.FltVal("1134");
      }
      this.SetCurrentNum("GMCAW.X1", num);
      this.calObjs.D1003URLA2020Cal.CalcTotalCredits(id, val);
    }

    private void calculateFreddieMac(string id, string val)
    {
      if (Tracing.IsSwitchActive(FHACalculation.sw, TraceLevel.Info))
        Tracing.Log(FHACalculation.sw, TraceLevel.Info, nameof (FHACalculation), "routine: FHACalculation.calculateFreddieMac ID: " + id);
      this.SetCurrentNum("CASASRN.X169", this.FltVal("1045"));
      this.SetCurrentNum("CASASRN.X142", this.FltVal("3"));
      this.SetCurrentNum("CASASRN.X109", this.FltVal("136") - this.FltVal("CASASRN.X19"));
      if (this.FltVal("1742") < this.FltVal("912"))
        this.SetCurrentNum("CASASRN.X99", this.FltVal("912"));
      else
        this.SetCurrentNum("CASASRN.X99", this.FltVal("1742"));
      BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
      double val1;
      if (this.FltVal("142") > 0.0)
      {
        double num = this.FltVal("1094", borrowerPairs[0]);
        if (this.FltVal("183") != 0.0)
        {
          string strA = this.Val("202");
          if (string.Compare(strA, "CashDepositOnSalesContract", true) != 0 && string.Compare(strA, "Cash deposit on sales contract", true) != 0)
            num += this.FltVal("183", borrowerPairs[0]);
        }
        if (this.FltVal("1716") != 0.0)
        {
          string strA = this.Val("1091");
          if (string.Compare(strA, "CashDepositOnSalesContract", true) != 0 && string.Compare(strA, "Cash deposit on sales contract", true) != 0)
            num += this.FltVal("1716", borrowerPairs[0]);
        }
        val1 = num - this.FltVal("142");
      }
      else
        val1 = this.FltVal("1094", borrowerPairs[0]) + this.FltVal("183", borrowerPairs[0]) + this.FltVal("1716", borrowerPairs[0]);
      if (!this.USEURLA2020 && this.Val("19").Equals("Purchase"))
        val1 -= this.FltVal("CD3.X80", borrowerPairs[0]);
      if (this.Val("1172") != "FHA")
      {
        int numberOfDeposits = this.loan.GetNumberOfDeposits();
        for (int index1 = 1; index1 <= numberOfDeposits; ++index1)
        {
          for (int index2 = 8; index2 <= 20; index2 += 4)
          {
            string str = index1.ToString("00");
            int num = index2 + 3;
            if (this.loan.GetSimpleField("DD" + str + index2.ToString("00")) == "GiftsNotDeposited")
              val1 -= this.FltVal("DD" + str + num.ToString("00"));
          }
        }
      }
      this.SetCurrentNum("CASASRN.X78", Utils.ArithmeticRounding(val1, 2));
      this.SetCurrentNum("CASASRN.X16", this.FltVal("268") + this.FltVal("737"));
      if (this.Val("19") == "Cash-Out Refinance")
      {
        BorrowerPair currentBorrowerPair = this.loan.CurrentBorrowerPair;
        double num = 0.0;
        this.loan.SetBorrowerPair(borrowerPairs[0]);
        int exlcudingAlimonyJobExp = this.loan.GetNumberOfLiabilitesExlcudingAlimonyJobExp();
        for (int index = 1; index <= exlcudingAlimonyJobExp; ++index)
        {
          string str1 = this.Val("FL" + index.ToString("00") + "27");
          string str2 = this.Val("FL" + index.ToString("00") + "28");
          if (str1 == "Y" && !string.IsNullOrWhiteSpace(str2) && str2 != "1")
            num += this.FltVal("FL" + index.ToString("00") + "16");
        }
        this.loan.SetBorrowerPair(currentBorrowerPair);
        this.SetCurrentNum("CASASRN.X79", Math.Max(0.0, Utils.ArithmeticRounding(this.FltVal("2") - this.FltVal("26") - this.FltVal("137") - this.FltVal("138") - this.FltVal("969") - this.FltVal("1093") + this.FltVal("URLA.X152") + num, 2)), true);
      }
      else
        this.SetCurrentNum("CASASRN.X79", 0.0);
    }

    private void calculateDraw203K(string id, string val)
    {
      if (Tracing.IsSwitchActive(FHACalculation.sw, TraceLevel.Info))
        Tracing.Log(FHACalculation.sw, TraceLevel.Info, nameof (FHACalculation), "routine: FHACalculation.calculateDraw203K ID: " + id);
      double num1 = 0.0;
      double num2 = 0.0;
      double num3 = 0.0;
      double num4 = 0.0;
      for (int index = 1; index <= 35; ++index)
      {
        int num5 = (index - 1) * 4;
        int num6 = num5 + 3;
        num1 += this.FltVal("DRW123K.X" + num6.ToString());
        int num7 = num5 + 4;
        num2 += this.FltVal("DRW123K.X" + num7.ToString());
        int num8 = num5 + 5;
        num3 += this.FltVal("DRW123K.X" + num8.ToString());
        int num9 = num5 + 6;
        num4 += this.FltVal("DRW123K.X" + num9.ToString());
      }
      this.SetCurrentNum("DRW123K.X143", num1);
      this.SetCurrentNum("DRW123K.X144", num2);
      this.SetCurrentNum("DRW123K.X145", num3);
      this.SetCurrentNum("DRW123K.X146", num4);
      this.SetCurrentNum("DRW123K.X149", num3 * 0.1);
      this.SetCurrentNum("DRW123K.X150", num2 * 0.1);
      this.SetCurrentNum("DRW123K.X151", num3 - num3 * 0.1);
      this.SetCurrentNum("DRW123K.X152", num2 - num2 * 0.1);
    }

    private double calculateRefiHiLo(string id)
    {
      string str1 = this.Val("1172");
      if (str1 != "FHA" && str1 != "FarmersHomeAdministration")
        return 0.0;
      string str2 = this.Val("19");
      this.Val("MORNET.X40");
      double refiHiLo1 = 0.0;
      if (id == "1157" || id == "1983")
      {
        if (str2 == "NoCash-Out Refinance" || str2 == "Cash-Out Refinance")
        {
          double num = this.FltVal("356");
          refiHiLo1 = !(this.Val("1983") == "Y") ? num * (391.0 / 400.0) : num * 0.85;
        }
        return refiHiLo1;
      }
      double refiHiLo2;
      switch (str2)
      {
        case "NoCash-Out Refinance":
          refiHiLo2 = this.FltVal("356") * (391.0 / 400.0);
          break;
        case "Cash-Out Refinance":
          double num1 = this.FltVal("356");
          if (Utils.GetTotalTimeSpanMonths(this.Val("1518"), this.Val("745"), false) <= 12 && num1 > this.FltVal("EEM.X63"))
            num1 = this.FltVal("EEM.X63");
          refiHiLo2 = num1 * 0.85;
          break;
        default:
          refiHiLo2 = 391.0 / 400.0 * this.FltVal("356");
          break;
      }
      return refiHiLo2;
    }

    private void calculateEEMWorksheet(string id, string val)
    {
      if (this.Val("2997") == "Y" && this.Val("3000") == "Y")
        this.SetCurrentNum("EEM.X64", this.FltVal("MAX23K.X6"));
      else
        this.SetCurrentNum("EEM.X64", this.FltVal("356"));
      if (this.Val("MORNET.X40") == "StreamlineWithAppraisal" || this.Val("MORNET.X40") == "StreamlineWithoutAppraisal" || this.Val("1067") == "ProposedConstruction")
        this.SetVal("EEM.X65", "");
      string str = this.Val("1067");
      if (str != "NewConstruction")
        this.SetVal("EEM.X66", "");
      if (str != "ExistingConstruction")
        this.SetVal("EEM.X67", "");
      switch (str)
      {
        case "NewConstruction":
          this.SetCurrentNum("EEM.X69", this.FltVal("EEM.X66"));
          break;
        case "ExistingConstruction":
          this.SetCurrentNum("EEM.X69", this.FltVal("EEM.X67"));
          break;
        default:
          this.SetVal("EEM.X69", "");
          break;
      }
      this.SetCurrentNum("EEM.X72", this.FltVal("EEM.X69") + this.FltVal("EEM.X70") + this.FltVal("EEM.X71"));
      this.SetCurrentNum("EEM.X74", this.FltVal("EEM.X72") < this.FltVal("EEM.X73") ? this.FltVal("EEM.X72") : this.FltVal("EEM.X73"));
      double num1 = this.FltVal("EEM.X75") + this.FltVal("EEM.X74") + this.FltVal("EEM.X76");
      this.SetCurrentNum("EEM.X77", num1);
      double num2 = this.FltVal("1107") / 100.0;
      double num3;
      if (num2 != 0.0)
      {
        num3 = Utils.ArithmeticRounding(num1 * num2 - 0.5, 0);
        this.SetCurrentNum("EEM.X78", num3);
      }
      else
      {
        num3 = 0.0;
        this.SetVal("EEM.X78", "");
      }
      double num4 = num3 + this.FltVal("EEM.X77");
      if (this.Val("1172") == "FHA")
        num4 = (double) (long) num4;
      else if (this.Val("4745") == "Y")
        num4 = Utils.ArithmeticRounding(num4, 0);
      this.SetCurrentNum("EEM.X79", num4);
      double num5;
      if (num2 != 0.0)
      {
        num5 = Utils.ArithmeticRounding(num2 * this.FltVal("EEM.X75") - 0.5, 0);
        this.SetCurrentNum("EEM.X81", num5);
      }
      else
      {
        num5 = 0.0;
        this.SetVal("EEM.X81", "");
      }
      double num6 = num5 + this.FltVal("EEM.X75");
      if (num6 != 0.0)
        num6 = Utils.ArithmeticRounding(num6 - 0.5, 0);
      this.SetCurrentNum("EEM.X82", num6);
      double num7 = this.calObjs.RegzCal.CalcRawMonthlyPayment(this.IntVal("4"), num6, this.FltVal("3"), false) + (this.FltVal("230") + this.FltVal("1405"));
      this.SetCurrentNum("EEM.X83", num7, this.UseNoPayment(num7));
      if (this.Val("1198") != string.Empty && this.Val("1199") != string.Empty)
      {
        if (this.FltVal("EEM.X82") == this.FltVal("2"))
          this.SetCurrentNum("EEM.X85", this.FltVal("1766"));
        else if (id == "EEM.X75" || id == "EEM.X76" || id == "EEM.X85" || id == "2997")
        {
          if (this.Val("2997") == "Y" && this.Val("EEM.X75") != string.Empty)
          {
            bool calculationDiagnostic = EnConfigurationSettings.GlobalSettings.AllowCalculationDiagnostic;
            if (calculationDiagnostic)
              EnConfigurationSettings.GlobalSettings.AllowCalculationDiagnostic = false;
            LoanData loanData = new LoanData(this.loan.XmlDocClone, this.loan.Settings, false);
            LoanCalculator loanCalculator = new LoanCalculator(this.sessionObjects, this.calObjs.LoanConfiguration, loanData, true, this.calObjs.ExternalLateFeeSettings, true);
            loanData.Calculator.PerformanceEnabled = false;
            loanData.SetFieldFromCal("1109", this.Val("EEM.X75"));
            if (calculationDiagnostic)
              EnConfigurationSettings.GlobalSettings.AllowCalculationDiagnostic = calculationDiagnostic;
            this.SetCurrentNum("EEM.X85", Utils.ParseDouble((object) loanData.GetField("1766")));
          }
          else
            this.SetVal("EEM.X85", "");
        }
      }
      this.SetCurrentNum("EEM.X87", this.FltVal("EEM.X83") + this.FltVal("EEM.X84") + this.FltVal("EEM.X85") + this.FltVal("EEM.X86"));
      this.calculateEEMX88AndEEMX89(id, val);
    }

    private void calculateEEMX88AndEEMX89(string id, string val)
    {
      double num = this.FltVal("1389");
      if (num != 0.0)
      {
        this.SetCurrentNum("EEM.X88", Utils.ArithmeticRounding(this.FltVal("EEM.X87") / num * 100.0, 3));
        this.SetCurrentNum("EEM.X89", Utils.ArithmeticRounding(this.FltVal("1742") / num * 100.0, 3));
      }
      else
      {
        this.SetVal("EEM.X88", "");
        this.SetVal("EEM.X89", "");
      }
    }

    private void calculateEEM(string id, string val)
    {
      string empty = string.Empty;
      double num1 = !id.StartsWith("MCAWPUR") ? this.FltVal("1109") : this.FltVal("MCAWPUR.X14");
      if (num1 == 0.0)
      {
        if (id.StartsWith("MCAWPUR"))
          throw new ApplicationException("The EEM Qualifying Ratios can't be calculated. Please make sure you have correct Mortgage Amount on field 2d (MCAWPUR.X14) of FHA Maximum Mortgage and Cash Needed Worksheet input form.");
        throw new ApplicationException("The EEM Qualifying Ratios can't be calculated. Please make sure you have correct Mortgage Amount on field 3a or 10g (1109).");
      }
      if (num1 == this.FltVal("1109"))
        num1 = this.FltVal("2");
      double num2 = this.FltVal("1721");
      double d = num1 + num2;
      double num3 = this.FltVal("4");
      double num4 = this.FltVal("1014") <= 0.0 ? this.FltVal("3") / 1200.0 : this.FltVal("1014") / 1200.0;
      double y = num3 - this.FltVal("1177");
      double num5 = 0.0;
      if (y > 360.0 || y < 1.0 || num4 == 0.0 || d < 0.0)
      {
        num5 = 0.0;
      }
      else
      {
        double num6 = Math.Pow(1.0 + num4, y);
        if (num6 > 0.0)
          num5 = Math.Round(d / ((1.0 - 1.0 / num6) / num4), 2);
      }
      double num7 = this.FltVal("1761");
      double num8;
      double num9;
      if (num7 > 0.0)
      {
        double num10 = this.FltVal("232") + this.FltVal("233") + this.FltVal("234") + this.FltVal("229") + this.FltVal("230") + this.FltVal("1405");
        num8 = (num5 + num10) / num7 * 100.0;
        num9 = (num5 + num10 + this.FltVal("1150")) / num7 * 100.0;
      }
      else
      {
        num8 = 0.0;
        num9 = 0.0;
      }
      double num11 = this.FltVal("358");
      double num12 = 0.0;
      double num13 = 0.0;
      if (num11 > 0.0)
      {
        num12 = d / num11 * 100.0;
        double num14 = d;
        num13 = ((!(this.Val("420") == "FirstLien") ? num14 + this.FltVal("427") : num14 + this.FltVal("428")) + this.FltVal("1732")) / num11 * 100.0;
      }
      this.SetVal("1228", "EEM mortgage amount is $" + Math.Floor(d).ToString("N2") + ".\r\n$" + num2.ToString("N2") + " in Energy Efficient Upgrades added to the loan amount.\r\nQualifying ratios for the new EEM loan amount of " + d.ToString("N2") + ": Top: " + num8.ToString("N3") + "%   Bottom: " + num9.ToString("N3") + "%\r\nLTV/CLTV ratios for the new EEM loan amount of " + d.ToString("N2") + ":  " + num12.ToString("N3") + "/" + num13.ToString("N3"));
    }

    private void calculateFHA203K(string id, string val)
    {
      Tracing.Log(FHACalculation.sw, TraceLevel.Info, nameof (FHACalculation), "routine: FHACalculation.calculateFHA203K ID: " + id);
      bool flag1 = this.Val("MAX23K.X117") == "Y";
      bool flag2 = this.Val("19").EndsWith("Refinance");
      if (flag1)
      {
        this.SetVal("MAX23K.X19", "");
        this.SetVal("MAX23K.X81", "");
      }
      double num1 = this.FltVal("MAX23K.X80") + this.FltVal("MAX23K.X113") + this.FltVal("MAX23K.X114") + this.FltVal("MAX23K.X21") + this.FltVal("MAX23K.X20");
      if (!flag1)
        num1 += this.FltVal("MAX23K.X19") + this.FltVal("MAX23K.X81");
      this.SetCurrentNum("MAX23K.X70", num1);
      double num2 = this.FltVal("MAX23K.X10") + this.FltVal("MAX23K.X70");
      if (!flag1)
        num2 += this.FltVal("MAX23K.X17");
      this.SetCurrentNum("MAX23K.X27", Utils.ArithmeticRounding(num2 * this.FltVal("MAX23K.X26") / 100.0, 2));
      double num3 = num2 * 0.015;
      this.SetCurrentNum("MAX23K.X44", num3 >= 350.0 ? num3 : 350.0);
      this.SetCurrentNum("MAX23K.X82", this.FltVal("MAX23K.X44") + this.FltVal("MAX23K.X27"));
      double num4 = this.FltVal("MAX23K.X70") + this.FltVal("MAX23K.X10") + this.FltVal("MAX23K.X82");
      if (!flag1)
        num4 += this.FltVal("MAX23K.X17");
      this.SetCurrentNum("MAX23K.X83", num4);
      if (flag2)
        this.SetCurrentNum("MAX23K.X109", this.FltVal("MAX23K.X132") + this.FltVal("MAX23K.X83") + this.FltVal("MAX23K.X108"));
      this.SetCurrentNum("MAX23K.X84", this.FltVal("MAX23K.X40") - this.FltVal("MCAWPUR.X29"));
      if (this.Val("4091") == "As Is")
        this.SetCurrentNum("MAX23K.X30", this.FltVal("MAX23K.X5"));
      else
        this.SetCurrentNum("MAX23K.X30", flag2 ? this.FltVal("MAX23K.X132") + this.FltVal("MAX23K.X108") : this.FltVal("MAX23K.X84"));
      this.SetCurrentNum("MAX23K.X85", this.FltVal("MAX23K.X30") + this.FltVal("MAX23K.X83"));
      this.SetCurrentNum("MAX23K.X7", this.FltVal("MAX23K.X6") * (this.Val("2996").IndexOf("Condominium") > -1 ? 1.0 : 1.1));
      this.SetCurrentNum("MAX23K.X86", this.FltVal("MAX23K.X7") < this.FltVal("MAX23K.X85") ? this.FltVal("MAX23K.X7") : this.FltVal("MAX23K.X85"));
      if (this.Val("1811") == "SecondHome")
      {
        this.SetCurrentNum("MAX23K.X87", 85.0);
        this.SetVal("MAX23K.X118", "With HOC Approval");
      }
      else
      {
        double num5 = this.FltVal("VASUMM.X23");
        if (num5 >= 500.0 && num5 <= 579.0)
        {
          this.SetCurrentNum("MAX23K.X87", 90.0);
          this.SetVal("MAX23K.X118", "Between 500 and 579");
        }
        else if (num5 >= 580.0)
        {
          this.SetCurrentNum("MAX23K.X87", flag2 ? 97.75 : 96.5);
          this.SetVal("MAX23K.X118", "At or above 580");
        }
        else
        {
          this.SetCurrentNum("MAX23K.X87", flag2 ? 97.75 : 96.5);
          this.SetVal("MAX23K.X118", "Manual Underwriting Required");
        }
      }
      double num6 = this.FltVal("MAX23K.X86") * this.FltVal("MAX23K.X87") / 100.0;
      if (!flag2)
        num6 -= this.FltVal("MAX23K.X119");
      this.SetCurrentNum("MAX23K.X88", num6);
      double num7;
      if (flag2)
      {
        num7 = this.FltVal("MAX23K.X109");
        if (num7 > this.FltVal("MAX23K.X88"))
          num7 = this.FltVal("MAX23K.X88");
        if (num7 > this.FltVal("MAX23K.X89"))
          num7 = this.FltVal("MAX23K.X89");
      }
      else
      {
        num7 = this.FltVal("MAX23K.X88");
        if (num7 > this.FltVal("MAX23K.X89"))
          num7 = this.FltVal("MAX23K.X89");
      }
      this.SetCurrentNum("MAX23K.X90", num7);
      this.SetCurrentNum("MAX23K.X96", this.FltVal("MAX23K.X90") + this.FltVal("MAX23K.X8"));
      this.SetCurrentNum("MAX23K.X98", this.FltVal("MAX23K.X6") * 0.2);
      this.SetCurrentNum("MAX23K.X99", this.FltVal("MAX23K.X97") < this.FltVal("MAX23K.X98") ? this.FltVal("MAX23K.X97") : this.FltVal("MAX23K.X98"));
      this.SetCurrentNum("MAX23K.X100", this.FltVal("MAX23K.X89") * 1.2);
      double num8 = this.FltVal("MAX23K.X96") + this.FltVal("MAX23K.X99");
      this.SetCurrentNum("MAX23K.X101", num8 < this.FltVal("MAX23K.X100") ? num8 : this.FltVal("MAX23K.X100"));
      if (this.FltVal("MAX23K.X6") != 0.0)
        this.SetCurrentNum("MAX23K.X104", Utils.ArithmeticRounding(this.FltVal("MAX23K.X101") / this.FltVal("MAX23K.X6") * 100.0, 2));
      else
        this.SetVal("MAX23K.X104", "");
      if (this.FltVal("MAX23K.X86") != 0.0)
        this.SetCurrentNum("MAX23K.X120", Utils.ArithmeticRounding(this.FltVal("MAX23K.X101") / this.FltVal("MAX23K.X86") * 100.0, 2));
      else
        this.SetVal("MAX23K.X120", "");
      this.SetCurrentNum("MAX23K.X103", this.FltVal("MAX23K.X83") + this.FltVal("MAX23K.X8") + this.FltVal("MAX23K.X95") + this.FltVal("MAX23K.X99"));
      this.SetCurrentNum("MAX23K.X93", this.FltVal("MAX23K.X121") + this.FltVal("MAX23K.X122") + this.FltVal("MAX23K.X123") + this.FltVal("MAX23K.X44") + this.FltVal("MAX23K.X27") + this.FltVal("MAX23K.X91") + this.FltVal("MAX23K.X92"));
      this.SetCurrentNum("MAX23K.X115", this.FltVal("MAX23K.X103") - this.FltVal("MAX23K.X93"));
      this.calObjs.Cal.CalcLTV(id, val);
    }

    private void calculateBorrowerAcknowledgement(string id, string val)
    {
      if (!(this.Val("MAX23K.X73") != "Other") || !(this.Val("MAX23K.X74") != ""))
        return;
      this.SetVal("MAX23K.X74", string.Empty);
    }
  }
}
