// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.CalculationEngine.PrequalCalculation
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.LoanUtils.CalculationServants;
using System;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.CalculationEngine
{
  internal class PrequalCalculation : CalculationBase
  {
    private const string className = "PrequalCalculation�";
    private static readonly string sw = Tracing.SwDataEngine;
    internal Routine CalcDownPay;
    internal Routine CalcRentOwn;
    internal Routine CalcUSDAMIP;
    internal Routine CalcMIP;
    internal Routine CalcMinMax;
    internal Routine CalcLoanComparison;
    internal Routine CalcHomeMaintenance;
    internal Routine CalcMiddleFICO;
    internal Routine CalcLoanAmount;
    internal Routine CalcLockRequestLoan;
    internal Routine CalcCopyConstructionFields;
    internal Routine CalcCopyBorrowerLenderPaid;
    internal Routine CalcOnSaveCopyBorrowerLenderPaid;
    internal Routine CalcFHASecondaryResidence;
    internal Routine CalcHELOCLockRequest;
    internal Routine ClearLockRequestFreeAndClear;
    private readonly PrequalCalculationServant _prequalCalculationServant;

    public PrequalCalculation(LoanData l, EllieMae.EMLite.CalculationEngine.CalculationObjects calObjs)
      : base(l, calObjs)
    {
      this.CalcDownPay = this.RoutineX(new Routine(this.calculateDownPay));
      this.CalcRentOwn = this.RoutineX(new Routine(this.calculateRentOwn));
      this.CalcUSDAMIP = this.RoutineX(new Routine(this.calculateUSDAMIP));
      this.CalcMIP = this.RoutineX(new Routine(this.calculateMIP));
      this.CalcMinMax = this.RoutineX(new Routine(this.calculateMinMax));
      this.CalcLoanComparison = this.RoutineX(new Routine(this.calculateLoanComparison));
      this.CalcHomeMaintenance = this.RoutineX(new Routine(this.calculateHomeMaintenance));
      this.CalcMiddleFICO = this.RoutineX(new Routine(this.calculateMiddleFICO));
      this.CalcLoanAmount = this.RoutineX(new Routine(this.calculateLoanAmount));
      this.CalcLockRequestLoan = this.RoutineX(new Routine(this.calculateLockRequestLoan));
      this.CalcCopyConstructionFields = this.RoutineX(new Routine(this.copyConstructionFields));
      this.CalcCopyBorrowerLenderPaid = this.RoutineX(new Routine(this.copyBorrowerLenderPaid));
      this.CalcOnSaveCopyBorrowerLenderPaid = this.RoutineX(new Routine(this.calcOnSaveCopyBorrowerLenderPaid));
      this.CalcFHASecondaryResidence = this.RoutineX(new Routine(this.calcFHASecondaryResidence));
      this.CalcHELOCLockRequest = this.RoutineX(new Routine(this.calcHELOCLockRequest));
      this.ClearLockRequestFreeAndClear = this.RoutineX(new Routine(this.clearLockRequestFreeAndClear));
      this.addFieldHandlers();
      this._prequalCalculationServant = new PrequalCalculationServant((ILoanModelProvider) this);
    }

    private void addFieldHandlers()
    {
      Routine routine1 = this.RoutineX(new Routine(this.calculateRentOwn));
      this.AddFieldHandler("PREQUAL.X216", routine1);
      this.AddFieldHandler("PREQUAL.X335", routine1);
      this.AddFieldHandler("PREQUAL.X107", routine1);
      this.AddFieldHandler("PREQUAL.X223", routine1);
      this.AddFieldHandler("PREQUAL.X228", routine1);
      this.AddFieldHandler("PREQUAL.X319", routine1);
      Routine routine2 = this.RoutineX(new Routine(this.calculateMinMax));
      this.AddFieldHandler("1790", routine2);
      this.AddFieldHandler("1791", routine2);
      Routine routine3 = this.RoutineX(new Routine(this.calculateHomeMaintenance));
      this.AddFieldHandler("PREQUAL.X334", routine3 + this.CalcRentOwn);
      this.AddFieldHandler("PREQUAL.X322", routine3 + this.CalcRentOwn);
      Routine routine4 = this.RoutineX(new Routine(this.calculateLockRequestLoan));
      this.AddFieldHandler("2949", routine4);
      this.AddFieldHandler("2950", this.CalcFHASecondaryResidence);
      this.AddFieldHandler("2951", this.CalcCopyConstructionFields + routine4 + this.ClearLockRequestFreeAndClear);
      this.AddFieldHandler("2952", routine4 + this.CalcFHASecondaryResidence);
      this.AddFieldHandler("2958", routine4);
      this.AddFieldHandler("3035", routine4);
      this.AddFieldHandler("3036", routine4);
      this.AddFieldHandler("3037", routine4);
      this.AddFieldHandler("3038", routine4);
      this.AddFieldHandler("3043", routine4);
      this.AddFieldHandler("3044", routine4);
      this.AddFieldHandler("3045", routine4);
      this.AddFieldHandler("3046", routine4);
      this.AddFieldHandler("3047", routine4);
      this.AddFieldHandler("3049", routine4);
      this.AddFieldHandler("3056", routine4);
      this.AddFieldHandler("3844", routine4);
      this.AddFieldHandler("3845", routine4);
      this.AddFieldHandler("3846", routine4);
      this.AddFieldHandler("4510", routine4);
      Routine routine5 = this.RoutineX(new Routine(this.calculateMiddleFICO));
      this.AddFieldHandler("USDA.X12", routine5);
      this.AddFieldHandler("USDA.X15", routine5);
      this.AddFieldHandler("4254", this.CalcCopyConstructionFields + this.CalcLockRequestLoan);
      this.AddFieldHandler("4255", this.CalcCopyConstructionFields + this.CalcLockRequestLoan + this.ClearLockRequestFreeAndClear);
      this.AddFieldHandler("LCP.X1", this.CalcCopyBorrowerLenderPaid);
    }

    private void calculateHomeMaintenance(string id, string val)
    {
      double num = this.Flt(val);
      switch (id)
      {
        case "PREQUAL.X322":
          this.SetCurrentNum("PREQUAL.X334", num / this.FltVal("136") * 100.0);
          break;
        case "PREQUAL.X334":
          this.SetCurrentNum("PREQUAL.X322", num * 0.01 * this.FltVal("136"));
          break;
        case "136":
          this.SetCurrentNum("PREQUAL.X322", this.FltVal("PREQUAL.X334") * 0.01 * num);
          break;
      }
    }

    private void calculateDownPay(string id, string val)
    {
      if (Tracing.IsSwitchActive(PrequalCalculation.sw, TraceLevel.Info))
        Tracing.Log(PrequalCalculation.sw, TraceLevel.Info, nameof (PrequalCalculation), "routine: calculateDownPay ID: " + id);
      if (id == "1172" && val == "FHA" && this.Val("USEREGZMI") == string.Empty)
        this.SetVal("USEREGZMI", "Y");
      bool isFHA = this.Val("1172") == "FHA";
      bool flag = this.Val("1172") == "VA";
      double num1 = this.FltVal("136");
      double num2 = this.FltVal("356");
      string str = this.Val("19");
      if (num2 > num1 && str.IndexOf("Refinance") < 0)
        this.SetCurrentNum("358", num1);
      else if (num2 > 0.0)
        this.SetCurrentNum("358", num2);
      double num3 = this.FltVal("1335");
      double num4 = this.FltVal("1771") / 100.0;
      double loanAmount = this.FltVal("1109");
      switch (id)
      {
        case "1172":
        case "136":
          if (num1 == 0.0)
          {
            this.SetCurrentNum("1335", 0.0);
            this.SetCurrentNum("1771", 0.0);
            break;
          }
          if (num3 == 0.0 && loanAmount == 0.0)
          {
            loanAmount = num1;
            this.setLoanAmountField(ref loanAmount, isFHA);
            double num5 = num1 - loanAmount;
            if (num5 != 0.0)
            {
              this.SetCurrentNum("1335", num5);
              this.SetCurrentNum("1771", Math.Round(num5 / num1 * 100.0, 3));
              break;
            }
            this.SetCurrentNum("1771", 0.0);
            break;
          }
          if (num3 != 0.0 && loanAmount == 0.0)
          {
            if (isFHA || flag && str == "Purchase")
              num3 = Math.Round(num3 + 0.499, 0);
            this.SetCurrentNum("1335", num3);
            loanAmount = num1 - num3;
            this.setLoanAmountField(ref loanAmount, isFHA);
            this.SetCurrentNum("1771", Math.Round(num3 / num1 * 100.0, 3));
            break;
          }
          if (num3 == 0.0 && loanAmount != 0.0)
          {
            this.setLoanAmountField(ref loanAmount, isFHA);
            double num6 = num1 - loanAmount;
            this.SetCurrentNum("1335", num6);
            this.SetCurrentNum("1771", Math.Round(num6 / num1 * 100.0, 3));
            this.SetCurrentNum("1109", loanAmount);
            break;
          }
          if (num3 != 0.0 && loanAmount != 0.0)
          {
            this.SetCurrentNum("1335", num3);
            loanAmount = num1 - num3;
            this.setLoanAmountField(ref loanAmount, isFHA);
            this.SetCurrentNum("1771", Math.Round(num3 / num1 * 100.0, 3));
            break;
          }
          break;
        case "1335":
          this.SetCurrentNum("1335", num3);
          if (num1 == 0.0 && loanAmount == 0.0)
          {
            loanAmount = num3 * -1.0;
            this.setLoanAmountField(ref loanAmount, isFHA);
            this.SetCurrentNum("1771", 0.0);
            break;
          }
          if (num1 != 0.0 && loanAmount == 0.0)
          {
            loanAmount = num1 - num3;
            this.setLoanAmountField(ref loanAmount, isFHA);
            this.SetCurrentNum("1771", Math.Round(num3 / num1 * 100.0, 3));
            break;
          }
          if (num1 == 0.0 && loanAmount != 0.0)
          {
            num1 = num3 + loanAmount;
            if (isFHA)
              num1 = Math.Truncate(100.0 * num1) / 100.0;
            this.SetCurrentNum("136", num1);
            this.SetCurrentNum("1771", Math.Round(num3 / num1 * 100.0, 3));
            break;
          }
          if (num1 != 0.0 && loanAmount != 0.0)
          {
            loanAmount = num1 - num3;
            this.setLoanAmountField(ref loanAmount, isFHA);
            this.SetCurrentNum("1771", Math.Round(num3 / num1 * 100.0, 3));
            break;
          }
          break;
        case "1109":
          loanAmount = this.ToDouble(val);
          this.setLoanAmountField(ref loanAmount, isFHA);
          if (num1 == 0.0 && num3 == 0.0)
          {
            this.SetCurrentNum("1771", 0.0);
            break;
          }
          if (num1 != 0.0 && num3 == 0.0)
          {
            double num7 = num1 - loanAmount;
            this.SetCurrentNum("1335", num7);
            this.SetCurrentNum("1771", Math.Round(num7 / num1 * 100.0, 3));
            break;
          }
          if (num1 == 0.0 && num3 != 0.0)
          {
            num1 = loanAmount + num3;
            if (isFHA)
              num1 = Math.Truncate(100.0 * num1) / 100.0;
            this.SetCurrentNum("136", num1);
            this.SetCurrentNum("1771", Math.Round(num3 / num1 * 100.0, 3));
            break;
          }
          if (num1 != 0.0 && num3 != 0.0)
          {
            double num8 = num1 - loanAmount;
            this.SetCurrentNum("1335", num8);
            this.SetCurrentNum("1771", Math.Round(num8 / num1 * 100.0, 3));
            break;
          }
          break;
        case "1771":
          if (num1 == 0.0 && loanAmount != 0.0)
          {
            num1 = loanAmount / (1.0 - num4);
            if (isFHA)
              num1 = Math.Truncate(100.0 * num1) / 100.0;
            this.SetCurrentNum("136", num1);
            this.SetCurrentNum("1335", num1 - loanAmount);
            break;
          }
          if (num1 != 0.0 && loanAmount == 0.0)
          {
            double num9 = num1 * num4;
            if (isFHA || flag && str == "Purchase")
              num9 = Math.Round(num9 + 0.499, 0);
            this.SetCurrentNum("1335", num9);
            loanAmount = num1 - num9;
            this.setLoanAmountField(ref loanAmount, isFHA);
            break;
          }
          if (num1 != 0.0 && loanAmount != 0.0)
          {
            double num10 = num1 * num4;
            if (isFHA || flag && str == "Purchase")
              num10 = Math.Round(num10 + 0.499, 0);
            this.SetCurrentNum("1335", num10);
            loanAmount = num1 - num10;
            this.setLoanAmountField(ref loanAmount, isFHA);
            break;
          }
          break;
      }
      if (!isFHA && loanAmount != 0.0)
      {
        this.setLoanAmountField(ref loanAmount, isFHA);
        double num11 = num1 - loanAmount;
        if (num1 != 0.0 && num11 != 0.0)
        {
          this.SetCurrentNum("1335", num11);
          this.SetCurrentNum("1771", Math.Round(num11 / num1 * 100.0, 3));
        }
      }
      this.calculateMIP(id, val);
      this.calObjs.ULDDExpCal.calculateFannieMaeExportFields("19", this.Val("19"));
    }

    private void setLoanAmountField(ref double loanAmount, bool isFHA)
    {
      if (isFHA)
        loanAmount = (double) (long) loanAmount;
      else if (this.Val("4745") == "Y")
        loanAmount = Utils.ArithmeticRounding(loanAmount, 0);
      this.calObjs.ToolCal.PopulateCountyLimit("1109", loanAmount.ToString());
      this.SetCurrentNum("1109", loanAmount);
    }

    internal void ImportDownPayment()
    {
      double num1 = this.FltVal("136");
      double num2 = this.FltVal("1335");
      double num3 = this.FltVal("1771") / 100.0;
      double loanAmount1 = this.FltVal("1109");
      bool isFHA = this.Val("1172") == "FHA";
      if (num1 != 0.0 && loanAmount1 == 0.0 && num2 == 0.0)
      {
        double loanAmount2 = num1;
        this.setLoanAmountField(ref loanAmount2, isFHA);
        this.SetCurrentNum("1335", 0.0);
        this.SetCurrentNum("1771", 0.0);
      }
      else if (num1 == 0.0 && loanAmount1 == 0.0 && num2 != 0.0)
      {
        double num4 = Math.Round(num2, 0);
        this.SetCurrentNum("1335", num4);
        double loanAmount3 = num4 * -1.0;
        this.setLoanAmountField(ref loanAmount3, isFHA);
        this.SetCurrentNum("1771", 0.0);
      }
      else if (num1 != 0.0 && num2 != 0.0 && loanAmount1 == 0.0)
      {
        double num5 = Math.Round(num2, 0);
        this.SetCurrentNum("1335", num5);
        double loanAmount4 = num1 - num5;
        this.setLoanAmountField(ref loanAmount4, isFHA);
        this.SetCurrentNum("1771", Math.Round(num5 / num1 * 100.0, 3));
      }
      else if (num1 != 0.0 && num2 == 0.0 && loanAmount1 != 0.0)
      {
        this.setLoanAmountField(ref loanAmount1, isFHA);
        double num6 = Math.Round(num1 - loanAmount1, 0);
        this.SetCurrentNum("1335", num6);
        this.SetCurrentNum("1771", Math.Round(num6 / num1 * 100.0, 3));
      }
      else if (num1 == 0.0 && num2 != 0.0 && loanAmount1 != 0.0)
      {
        this.setLoanAmountField(ref loanAmount1, isFHA);
        double num7 = Math.Round(num2, 0);
        this.SetCurrentNum("1335", num7);
        double num8 = Math.Round(loanAmount1 + num7, 0);
        if (isFHA)
          num8 = Math.Truncate(100.0 * num8) / 100.0;
        this.SetCurrentNum("136", num8);
        this.SetCurrentNum("1771", Math.Round(num7 / num8 * 100.0, 3));
      }
      else if (num1 != 0.0 && num2 != 0.0 && loanAmount1 != 0.0)
      {
        double num9 = Math.Round(!isFHA ? Math.Round(num1 - loanAmount1, 0) : (double) (int) (num1 - loanAmount1), 0);
        this.SetCurrentNum("1335", num9);
        this.SetCurrentNum("1771", Math.Round(num9 / num1 * 100.0, 3));
      }
      this.calculateDownPay((string) null, (string) null);
    }

    private void calculateMinMax(string id, string val)
    {
      double num1 = this.FltVal("1790") / 100.0;
      double num2 = this.FltVal("1791") / 100.0;
      string str = this.Val("1811");
      if (str == "SecondHome" || str == "Investor")
      {
        double num3 = 0.0;
        if (num1 > 0.0)
          num3 = (this.FltVal("350") + this.FltVal("737")) / num1;
        this.SetCurrentNum("PREQUAL.X7", num3);
      }
      else
      {
        double num4 = 0.0;
        if (num1 > 0.0)
          num4 = this.FltVal("912") / num1;
        this.SetCurrentNum("PREQUAL.X7", num4);
      }
      this.SetCurrentNum("PREQUAL.X8", this.FltVal("1389") * num2);
    }

    private void calculateUSDAMIP(string id, string val)
    {
      double num1 = this.FltVal("1109");
      double num2 = Utils.ArithmeticRounding(num1 / (1.0 - this.FltVal("3560") / 100.0), 2);
      double num3 = Utils.ArithmeticRounding(num2 % 1.0, 2);
      double num4 = num2;
      double num5 = Utils.ArithmeticRounding(num2 - num1, 2);
      string str = this.Val("3566");
      switch (str)
      {
        case "FinancingAll":
          this.SetVal("3561", num5 != 0.0 ? num5.ToString("N2") : "");
          this.SetVal("3562", num4 != 0.0 ? num4.ToString("N2") : "");
          break;
        case "FinancingPortion":
          double num6 = num1 + this.FltVal("3563");
          this.SetVal("3561", num6 != 0.0 ? num6.ToString("N2") : "");
          num5 = Utils.ArithmeticRounding(num6 * this.FltVal("3560") / 100.0, 2);
          this.SetVal("3564", num5 != 0.0 ? num5.ToString("N2") : "");
          break;
      }
      double num7 = 0.0;
      double num8 = 0.0;
      switch (str)
      {
        case "FinancingAll":
          num7 = num8 = this.FltVal("3561");
          this.SetVal("1760", num3 != 0.0 ? num3.ToString("N2") : "");
          this.SetVal("1765", "");
          break;
        case "FinancingPortion":
          num8 = this.FltVal("3564");
          num3 = num8 - this.FltVal("3563");
          this.SetVal("1760", num3 != 0.0 ? num3.ToString("N2") : "");
          this.SetVal("1765", "Y");
          num7 = this.FltVal("3563");
          break;
      }
      this.SetVal("1826", num8 != 0.0 ? num8.ToString("N2") : "");
      double num9 = num1 != 0.0 ? Utils.ParseDouble((object) (num5 / num1 * 100.0), 6.0) : 0.0;
      this.SetVal("1107", num9 != 0.0 ? num9.ToString("N6") : "");
      this.SetVal("1045", num7 != 0.0 ? num7.ToString("N2") : "");
      if (this.UseNew2015GFEHUD)
      {
        this.SetCurrentNum("NEWHUD2.X1594", num3);
        this.SetCurrentNum("NEWHUD2.X1593", num8 - num3);
        this.SetCurrentNum("NEWHUD.X1301", num8);
        this.SetCurrentNum("NEWHUD2.X1587", num8);
        this.SetVal("NEWHUD.X1299", "Guarantee Fee");
      }
      this.calObjs.USDACal.CalcLoanFundsUsage(id, val);
      this.calObjs.GFECal.PopulateFeeList(id, val);
      if (!this.IsLocked("969"))
        this.SetVal("969", num8 != 0.0 ? num8.ToString("N2") : "");
      this.calObjs.D1003URLA2020Cal.CalcEstimatedClosingCostsAmount(id, val);
      this.calObjs.VACal.CalcVACashOutRefinance(id, val);
    }

    private void calculateMIP(string id, string val)
    {
      string str1 = this.Val("1765");
      double num1 = this.FltVal("1109");
      double num2 = this.FltVal("1107");
      this.FltVal("969");
      double num3 = this.FltVal("1760");
      string str2 = this.Val("1172");
      bool flag1 = this.Val("SYS.X11") == "Y";
      bool flag2 = this.Val("3625") == "Y";
      if (str2 == "FarmersHomeAdministration" && str1 == "Y")
      {
        num3 = this.FltVal("3564") - this.FltVal("3563") - (!this.IsLocked("NEWHUD.X1301") ? this.FltVal("NEWHUD.X1302") : this.FltVal("562"));
        this.SetCurrentNum("1760", num3);
      }
      if (str2 != "Conventional" && this.Val("3248") == "Y")
        this.SetVal("3248", "");
      double num4;
      if (str1 != "Y")
      {
        double val1 = num1 * num2 / 100.0;
        num4 = !(str2 == "FHA") ? Utils.ArithmeticRounding(val1, 2) : Convert.ToDouble(Math.Truncate(100M * Convert.ToDecimal(val1)) / 100M);
        this.SetCurrentNum("1826", num4);
        double num5 = !flag1 ? Utils.ArithmeticRounding(num4 % 1.0, 2) : num4 - (double) ((int) (num4 / 50.0) * 50);
        if (str2 == "FHA" || this.Val("4745") == "Y")
        {
          double num6 = num4 - this.FltVal("562");
          if (num6 >= 0.0)
          {
            double num7 = Utils.ArithmeticRounding(num6 % 1.0, 2);
            num5 = (double) (int) num5 + num7;
          }
        }
        this.SetCurrentNum("1760", num5);
        this.SetCurrentNum("3033", num5);
      }
      else
      {
        num4 = this.FltVal("1826");
        if (!flag2)
        {
          if (num1 != 0.0)
            this.SetCurrentNum("1107", Utils.ArithmeticRounding(num4 / num1 * 100.0, 6));
          else
            this.SetCurrentNum("1107", 0.0);
        }
        if (str2 != "FarmersHomeAdministration")
          num3 -= this.FltVal("562");
        if (num3 < 0.0)
          num3 = 0.0;
        else if (flag1)
          num3 = num4 - (double) ((int) (num4 / 50.0) * 50);
        this.SetCurrentNum("3033", num3);
      }
      double num8 = num4 - this.FltVal("1760");
      if (this.FltVal("1760") + this.FltVal("562") > this.FltVal("1826"))
        num8 -= this.FltVal("1826") - this.FltVal("1760");
      else if (num8 > 0.0)
        num8 -= !(str2 == "FarmersHomeAdministration") || this.IsLocked("NEWHUD.X1301") ? this.FltVal("562") : this.FltVal("NEWHUD.X1302");
      this.SetCurrentNum("1045", num8);
      this.calObjs.USDACal.CalcLoanFundsUsage(id, val);
      switch (str2)
      {
        case "VA":
          this.SetCurrentNum("NEWHUD2.X2286", num8);
          break;
        case "FarmersHomeAdministration":
          if (this.Val("3566") == "FinancingPortion")
          {
            double num9 = Utils.ArithmeticRounding(Utils.ArithmeticRounding(this.FltVal("3563") % 1.0, 2), 2);
            this.SetCurrentNum("NEWHUD2.X2187", this.FltVal("3563") - num9);
            this.SetCurrentNum("NEWHUD2.X2188", this.FltVal("3564") - this.FltVal("3563") + num9);
            break;
          }
          break;
        default:
          this.SetCurrentNum("NEWHUD2.X2187", num8);
          break;
      }
      if (str2 == "FarmersHomeAdministration")
      {
        double num10 = num8 + this.FltVal("1760");
        this.SetVal("1753", "");
        if (!this.IsLocked("NEWHUD.X1301"))
        {
          if (num10 != 0.0 && string.Compare(this.Val("NEWHUD.X1299"), "Guarantee Fee", true) != 0)
          {
            if (id == "NEWHUD.X1302")
              num10 += this.FltVal("NEWHUD.X1302");
            this.calObjs.USDACal.ClearLine819(id, val);
            this.SetVal("NEWHUD.X1299", "Guarantee Fee");
          }
          this.SetCurrentNum("NEWHUD.X1301", num10);
          this.calObjs.NewHudCal.CopyHUD2010ToGFE2010("NEWHUD.X1301", id, false);
          this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem819("NEWHUD.X1301", "");
        }
      }
      double num11 = this.FltVal("1109") + this.FltVal("1045");
      if (str2 == "FHA")
      {
        this.SetCurrentNum("2", (double) (long) num11);
        this.calObjs.NewHud2015Cal.CalcCDPage3Transactions();
      }
      else
      {
        if (this.Val("4745") == "Y")
          num11 = Utils.ArithmeticRounding(num11, 0);
        this.SetCurrentNum("2", num11);
        this.calObjs.NewHud2015Cal.CalcCDPage3TotalL(id, val);
      }
      this.calObjs.D1003URLA2020Cal.CalcTotalNewMortgageLoans(id, val);
      this.calObjs.HMDACal.CalcLoanAmount(id, val);
      this.calObjs.D1003Cal.PopulateLoanAmountToNmlsApplicationAmounts(this.calObjs.D1003Cal.InitialApplicaitonDateID, this.Val(this.calObjs.D1003Cal.InitialApplicaitonDateID));
      this.calObjs.GFECal.PopulateFeeList(id, val);
      if (this.FltVal("NMLS.X11") >= 0.0 && this.FltVal("2") >= 0.0)
        this.SetCurrentNum("NMLS.X12", this.FltVal("2") - this.FltVal("NMLS.X11"));
      this.calObjs.GFECal.CalcDISCLOSUREX694(id, val);
      this.SetCurrentNum("NEWHUD.X4", this.FltVal("2"));
      this.calObjs.Cal.CalcMortgageInsurance(id, val);
      if (str2 != "VA" && str2 != "FarmersHomeAdministration")
      {
        double num12 = this.FltVal("1826");
        if (num12 == 0.0)
          num12 = this.FltVal("1766") * this.FltVal("1209");
        double num13 = num12 - this.FltVal("562");
        this.SetCurrentNum("337", num13);
        this.calObjs.GFECal.SyncMLDSField("337", num13.ToString());
        this.calObjs.NewHudCal.CopyHUD2010ToGFE2010("337", id, false);
      }
      else if (str2 == "FarmersHomeAdministration")
      {
        if (this.FltVal("337") != 0.0 && !this.IsLocked("NEWHUD.X1301"))
          this.calObjs.USDACal.CopyPOCPTCAPRFromLine902ToLine819(id, val);
        if (!this.IsLocked("NEWHUD.X1301"))
        {
          this.SetVal("337", "");
          this.calObjs.GFECal.SyncMLDSField("337", "");
        }
        this.calObjs.NewHudCal.CopyHUD2010ToGFE2010("337", id, false);
      }
      this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem902(id, val);
      if (!this.calObjs.SkipLockRequestSync)
      {
        this.SetCurrentNum("3043", this.FltVal("1109"));
        this.SetCurrentNum("3044", this.FltVal("1107"));
        this.SetCurrentNum("3045", this.FltVal("1826"));
        if (this.Val("1765").ToLower() == "y" || this.Val("1765").ToLower() == "yes")
          this.SetVal("3046", "Y");
        else if (this.Val("1765").ToLower() == "n" || this.Val("1765").ToLower() == "no")
          this.SetVal("3046", "N");
        else
          this.SetVal("3046", this.Val("1765"));
        this.SetVal("3056", this.Val("SYS.X11"));
        this.SetCurrentNum("3047", this.FltVal("1760"));
        this.SetCurrentNum("3049", this.FltVal("562"));
        this.SetCurrentNum("2965", this.FltVal("2"));
      }
      this.calObjs.D1003Cal.CalcConformingLimit(id, val);
      this.calObjs.ATRQMCal.CalcDiscountPointPercent(id, val);
      this.calObjs.VACal.CalcVACashOutRefinance(id, val);
    }

    private void calculateRentOwn(string id, string val)
    {
      Tracing.Log(PrequalCalculation.sw, TraceLevel.Info, nameof (PrequalCalculation), "routine: calculateRentOwn ID: " + id);
      int y = this.IntVal("PREQUAL.X335") * 12;
      if (y == 0)
        y = 1;
      double num1 = this.FltVal("119");
      double num2 = this.FltVal("PREQUAL.X319");
      double num3 = this.FltVal("PREQUAL.X216") / 100.0;
      double num4 = 0.0;
      for (int index = 1; index <= y; ++index)
      {
        if (index % 12 == 1 && index != 1)
          num1 += num1 * num3;
        num4 += num1 + num2;
      }
      this.SetCurrentNum("PREQUAL.X323", num4);
      this.SetCurrentNum("PREQUAL.X324", (this.FltVal("5") + this.FltVal("230") + this.FltVal("1405")) * (double) y + this.FltVal("PREQUAL.X322") / 12.0 * (double) y);
      double num5 = this.FltVal("3");
      double num6 = 0.0;
      if (num5 != 0.0)
      {
        double num7 = this.FltVal("2");
        double num8 = this.FltVal("5");
        double num9 = num5 / 100.0 / 12.0;
        double num10 = Math.Pow(1.0 + num9, (double) y);
        num6 = (num8 - num10 * num8 + num9 * num10 * num7) / num9;
      }
      this.SetCurrentNum("PREQUAL.X329", num6);
      double num11 = this.FltVal("PREQUAL.X228") / 100.0;
      this.SetCurrentNum("PREQUAL.X325", (this.FltVal("1405") * (double) y + (this.FltVal("5") * (double) y - (this.FltVal("2") - this.FltVal("PREQUAL.X329")))) * num11);
      double num12 = this.FltVal("PREQUAL.X107") / 100.0;
      this.SetCurrentNum("PREQUAL.X328", this.FltVal("136") * Math.Pow(1.0 + num12, this.FltVal("PREQUAL.X335")));
      double num13 = this.FltVal("PREQUAL.X223") / 100.0 / 12.0;
      this.SetCurrentNum("PREQUAL.X330", (this.FltVal("1335") + this.FltVal("304")) * Math.Pow(1.0 + num13, (double) y));
      this.SetCurrentNum("PREQUAL.X320", this.FltVal("119") + this.FltVal("PREQUAL.X319"));
      this.SetCurrentNum("PREQUAL.X321", this.FltVal("5") + this.FltVal("230") + this.FltVal("1405"));
      this.SetCurrentNum("PREQUAL.X326", this.FltVal("PREQUAL.X323") / (double) y);
      this.SetCurrentNum("PREQUAL.X327", (this.FltVal("PREQUAL.X324") - this.FltVal("PREQUAL.X325")) / (double) y);
      this.SetCurrentNum("PREQUAL.X317", this.FltVal("PREQUAL.X327") - this.FltVal("PREQUAL.X326"));
      this.SetCurrentNum("PREQUAL.X331", this.FltVal("PREQUAL.X328") - this.FltVal("PREQUAL.X329") - this.FltVal("PREQUAL.X330"));
      this.SetCurrentNum("PREQUAL.X332", this.FltVal("PREQUAL.X324") - this.FltVal("PREQUAL.X325") - this.FltVal("PREQUAL.X323"));
      double num14 = this.FltVal("PREQUAL.X331") - this.FltVal("PREQUAL.X332");
      this.SetCurrentNum("PREQUAL.X333", num14);
      if (num14 >= 0.0)
        this.loan.SetCurrentField("PREQUAL.X316", "Buying");
      else
        this.loan.SetCurrentField("PREQUAL.X316", "Renting");
      this.SetCurrentNum("PREQUAL.X318", Math.Abs(num14));
    }

    private void calculateMiddleFICO(string id, string val)
    {
      this._prequalCalculationServant.CalculateMiddleFico(id);
    }

    private void calculateLoanComparison(string id, string val)
    {
      double num1 = this.FltVal("915") - this.FltVal("142");
      if (num1 < 0.0)
        num1 = 0.0;
      this.SetCurrentNum("PREQUAL.X275", num1);
      double num2 = this.FltVal("PREQUAL.X290") - this.FltVal("PREQUAL.X44");
      if (num2 < 0.0)
        num2 = 0.0;
      this.SetCurrentNum("PREQUAL.X292", num2);
      double num3 = this.FltVal("PREQUAL.X291") - this.FltVal("PREQUAL.X84");
      if (num3 < 0.0)
        num3 = 0.0;
      this.SetCurrentNum("PREQUAL.X293", num3);
      this.SetCurrentNum("PREQUAL.X272", this.FltVal("136") + this.FltVal("967") + this.FltVal("968") + this.FltVal("1092"));
      this.SetCurrentNum("PREQUAL.X294", this.FltVal("1733") + this.FltVal("462"));
      double num4 = 0.0;
      double num5 = 0.0;
      int exlcudingAlimonyJobExp = this.loan.GetNumberOfLiabilitesExlcudingAlimonyJobExp();
      for (int index = 1; index <= exlcudingAlimonyJobExp; ++index)
      {
        string str = "FL" + index.ToString("00");
        if (this.Val(str + "18") == "Y")
        {
          if (this.Val(str + "08") == "MortgageLoan")
            num4 += this.FltVal(str + "13");
          else
            num5 += this.FltVal(str + "13");
        }
      }
      this.SetCurrentNum("PREQUAL.X270", num4);
      this.SetCurrentNum("PREQUAL.X271", num5);
      for (int col = 0; col <= 2; ++col)
        this.qualificationTest(col, false);
    }

    internal string GetQualificationDetail(int column) => this.qualificationTest(column, true);

    internal string GetQualificationDetail(int column, bool getDetail)
    {
      return this.qualificationTest(column, getDetail);
    }

    private string qualificationTest(int col, bool getDetail)
    {
      string empty1 = string.Empty;
      string str1 = string.Empty;
      string id1 = "1484";
      string id2 = "PREQUAL.X202";
      string id3 = "1790";
      string id4 = "1791";
      string id5 = "PREQUAL.X209";
      string id6 = "PREQUAL.X210";
      string id7 = "PREQUAL.X7";
      string id8 = "PREQUAL.X8";
      string id9 = "142";
      string id10 = "2";
      string id11 = "740";
      string id12 = "742";
      string id13 = "353";
      string id14 = "976";
      string id15 = "1389";
      string id16 = "1742";
      string id17 = "915";
      string id18 = "PREQUAL.X274";
      switch (col)
      {
        case 1:
          id1 = "LP0148";
          id2 = "PREQUAL.X260";
          id3 = "LP0149";
          id4 = "LP0150";
          id5 = "LP0146";
          id6 = "LP0147";
          id7 = "PREQUAL.X56";
          id8 = "PREQUAL.X57";
          id9 = "PREQUAL.X44";
          id10 = "PREQUAL.X299";
          id11 = "PREQUAL.X237";
          id12 = "PREQUAL.X238";
          id15 = "PREQUAL.X266";
          id17 = "PREQUAL.X290";
          id16 = "PREQUAL.X258";
          id18 = "PREQUAL.X303";
          break;
        case 2:
          id1 = "LP0248";
          id2 = "PREQUAL.X261";
          id3 = "LP0249";
          id4 = "LP0250";
          id5 = "LP0246";
          id6 = "LP0247";
          id7 = "PREQUAL.X96";
          id8 = "PREQUAL.X97";
          id9 = "PREQUAL.X84";
          id10 = "PREQUAL.X300";
          id11 = "PREQUAL.X243";
          id12 = "PREQUAL.X244";
          id15 = "PREQUAL.X267";
          id16 = "PREQUAL.X259";
          id17 = "PREQUAL.X291";
          id18 = "PREQUAL.X304";
          break;
      }
      this.IntVal(id1);
      double num1 = this.FltVal(id2);
      double num2 = this.FltVal(id3);
      double num3 = this.FltVal(id4);
      double num4 = this.FltVal(id5);
      double num5 = this.FltVal(id6);
      double num6 = this.FltVal(id7);
      double num7 = this.FltVal(id8);
      string empty2 = string.Empty;
      if (getDetail)
      {
        if (num1 == 0.0)
        {
          string str2 = ">> Maximum Loan Amount field is blank.\r\n";
          empty1 += str2;
          str1 = str1 + str2 + "Maximum Loan Amount field\t" + id2 + "\r\n" + "Maximum Loan Limit field\tPREQUAL.X205\r\n";
        }
        if (num2 == 0.0)
        {
          string str3 = ">> Maximum Top Ratio field is blank.\r\n";
          empty1 += str3;
          str1 = str1 + str3 + "Maximum Top Ratio field\t" + id3 + "\r\n";
        }
        if (num3 == 0.0)
        {
          string str4 = ">> Maximum Bottom Ratio field is blank.\r\n";
          empty1 += str4;
          str1 = str1 + str4 + "Maximum Bottom Ratio field\t" + id4 + "\r\n";
        }
        if (num4 == 0.0)
        {
          string str5 = ">> Maximum LTV field is blank.\r\n";
          empty1 += str5;
          str1 = str1 + str5 + "Maximum LTV field field\t" + id5 + "\r\n";
        }
        if (num5 == 0.0)
        {
          string str6 = ">> Maximum CLTV field is blank.\r\n";
          empty1 += str6;
          str1 = str1 + str6 + "Maximum CLTV field field\t" + id6 + "\r\n";
        }
        if (num6 == 0.0)
        {
          string str7 = ">> Minimum Income field is blank.\r\n";
          empty1 += str7;
          str1 = str1 + str7 + "Minimum Income field\t" + id7 + "\r\n" + "Monthly Housing Expense field\t912\r\n";
        }
        if (num7 == 0.0)
        {
          string str8 = ">> Maximum Debt field is blank.\r\n";
          empty1 += str8;
          str1 = str1 + str8 + "Maximum Debt field\t" + id8 + "\r\n" + "Bottom Ratio field\t1791\r\n";
        }
      }
      bool flag = true;
      if (col == 0 && !getDetail)
      {
        this.SetVal("PREQUAL.X307", "");
        this.SetVal("PREQUAL.X308", "");
        this.SetVal("PREQUAL.X309", "");
        this.SetVal("PREQUAL.X310", "");
        this.SetVal("PREQUAL.X311", "");
        this.SetVal("PREQUAL.X312", "");
        this.SetVal("PREQUAL.X313", "");
        this.SetVal("PREQUAL.X314", "");
        this.SetVal("PREQUAL.X315", "");
      }
      if (empty1 != string.Empty)
        empty1 += "\r\n";
      string empty3 = string.Empty;
      if (this.FltVal(id10) > num1)
      {
        string str9 = ">> Borrower's Loan Amount is not qualified. Maximum Required: $" + num1.ToString("N2") + ".\r\n";
        empty1 += str9;
        str1 = str1 + str9 + "Current Loan Amount: $" + this.Val(id10) + "\t" + id10 + "\r\n";
        flag = false;
        if (this.FltVal(id10) != 0.0 && num1 != 0.0)
        {
          if (col == 0 && !getDetail)
            this.SetVal("PREQUAL.X308", "No");
          else
            empty3 += "No\r\n";
        }
        else
          empty3 += "\r\n";
      }
      else if (this.FltVal(id10) != 0.0 && num1 != 0.0)
      {
        if (col == 0 && !getDetail)
          this.SetVal("PREQUAL.X308", "Yes");
        else
          empty3 += "Yes\r\n";
      }
      else
        empty3 += "\r\n";
      if (this.FltVal(id11) > num2)
      {
        string str10 = ">> Borrower's Top Ratio is not qualified. Maximum Required: " + num2.ToString("N3") + "%.\r\n";
        empty1 += str10;
        str1 = str1 + str10 + "Current Top Ratio: " + this.Val(id11) + "%\t" + id11 + "\r\n";
        flag = false;
        if (this.FltVal(id11) != 0.0 && num2 != 0.0)
        {
          if (col == 0 && !getDetail)
            this.SetVal("PREQUAL.X309", "No");
          else
            empty3 += "No\r\n";
        }
        else
          empty3 += "\r\n";
      }
      else if (this.FltVal(id11) != 0.0 && num2 != 0.0)
      {
        if (col == 0 && !getDetail)
          this.SetVal("PREQUAL.X309", "Yes");
        else
          empty3 += "Yes\r\n";
      }
      else
        empty3 += "\r\n";
      if (this.FltVal(id12) > num3)
      {
        string str11 = ">> Borrower's Bottom Ratio is not qualified. Maximum Required: " + num3.ToString("N3") + "%.\r\n";
        empty1 += str11;
        str1 = str1 + str11 + "Current Bottom Ratio: " + this.Val(id12) + "%\t" + id12 + "\r\n";
        flag = false;
        if (this.FltVal(id12) != 0.0 && num3 != 0.0)
        {
          if (col == 0 && !getDetail)
            this.SetVal("PREQUAL.X310", "No");
          else
            empty3 += "No\r\n";
        }
        else
          empty3 += "\r\n";
      }
      else if (this.FltVal(id12) != 0.0 && num3 != 0.0)
      {
        if (col == 0 && !getDetail)
          this.SetVal("PREQUAL.X310", "Yes");
        else
          empty3 += "Yes\r\n";
      }
      else
        empty3 += "\r\n";
      if (col == 0)
      {
        if (this.FltVal(id13) > num4)
        {
          string str12 = ">> Borrower's LTV Ratio is not qualified. Maximum Required: " + num4.ToString("N3") + "%.\r\n";
          empty1 += str12;
          str1 = str1 + str12 + "Current LTV Ratio: " + this.Val(id13) + "%\t" + id13 + "\r\n";
          flag = false;
          if (this.FltVal(id13) != 0.0 && num4 != 0.0)
          {
            if (col == 0 && !getDetail)
              this.SetVal("PREQUAL.X311", "No");
            else
              empty3 += "No\r\n";
          }
          else
            empty3 += "\r\n";
        }
        else if (this.FltVal(id13) != 0.0 && num4 != 0.0)
        {
          if (col == 0 && !getDetail)
            this.SetVal("PREQUAL.X311", "Yes");
          else
            empty3 += "Yes\r\n";
        }
        else
          empty3 += "\r\n";
        if (this.FltVal(id14) > num5)
        {
          string str13 = ">> Borrower's CLTV Ratio is not qualified. Maximum Required: " + num5.ToString("N3") + "%.\r\n";
          empty1 += str13;
          str1 = str1 + str13 + "Current CLTV Ratio: " + this.Val(id14) + "%\t" + id14 + "\r\n";
          flag = false;
          if (this.FltVal(id14) != 0.0 && num5 != 0.0)
          {
            if (col == 0 && !getDetail)
              this.SetVal("PREQUAL.X312", "No");
            else
              empty3 += "No\r\n";
          }
          else
            empty3 += "\r\n";
        }
        else if (this.FltVal(id14) != 0.0 && num5 != 0.0)
        {
          if (col == 0 && !getDetail)
            this.SetVal("PREQUAL.X312", "Yes");
          else
            empty3 += "Yes\r\n";
        }
        else
          empty3 += "\r\n";
      }
      else
      {
        double num8 = col != 1 ? (!(this.Val("PREQUAL.X263") == "") ? this.FltVal("PREQUAL.X263") : this.calcLTVRatio(true, col)) : (!(this.Val("PREQUAL.X262") == "") ? this.FltVal("PREQUAL.X262") : this.calcLTVRatio(true, col));
        if (num8 > num4)
        {
          string str14 = ">> Borrower's LTV Ratio is not qualified. Maximum Required: " + num4.ToString("N3") + "%.\r\n";
          empty1 += str14;
          str1 = str1 + str14 + "Current LTV Ratio: " + num8.ToString("N3") + "%\t" + id13 + "\r\n";
          flag = false;
          if (num8 != 0.0 && num4 != 0.0)
          {
            if (col == 0 && !getDetail)
              this.SetVal("PREQUAL.X311", "No");
            else
              empty3 += "No\r\n";
          }
          else
            empty3 += "\r\n";
        }
        else if (num8 != 0.0 && num4 != 0.0)
        {
          if (col == 0 && !getDetail)
            this.SetVal("PREQUAL.X311", "Yes");
          else
            empty3 += "Yes\r\n";
        }
        else
          empty3 += "\r\n";
        num8 = col != 1 ? (!(this.Val("PREQUAL.X265") == "") ? this.FltVal("PREQUAL.X265") : this.calcLTVRatio(false, col)) : (!(this.Val("PREQUAL.X264") == "") ? this.FltVal("PREQUAL.X264") : this.calcLTVRatio(false, col));
        if (num8 > num5)
        {
          string str15 = ">> Borrower's CLTV Ratio is not qualified. Maximum CLTV: " + num5.ToString("N3") + "%.\r\n";
          empty1 += str15;
          str1 = str1 + str15 + "Current CLTV Ratio: " + num8.ToString("N3") + "%\t" + id14 + "\r\n";
          flag = false;
          if (num8 != 0.0 && num5 != 0.0)
          {
            if (col == 0 && !getDetail)
              this.SetVal("PREQUAL.X312", "No");
            else
              empty3 += "No\r\n";
          }
          else
            empty3 += "\r\n";
        }
        else if (num8 != 0.0 && num5 != 0.0)
        {
          if (col == 0 && !getDetail)
            this.SetVal("PREQUAL.X312", "Yes");
          else
            empty3 += "Yes\r\n";
        }
        else
          empty3 += "\r\n";
      }
      if (this.FltVal(id15) < num6)
      {
        string str16 = ">> Borrower's Income is not qualified. Minimum Required: $" + num6.ToString("N2") + ".\r\n";
        empty1 += str16;
        str1 = str1 + str16 + "Current Income: $" + this.Val(id15) + "\t" + id15 + "\r\n";
        flag = false;
        if (this.FltVal(id15) != 0.0 && num6 != 0.0)
        {
          if (col == 0 && !getDetail)
            this.SetVal("PREQUAL.X313", "No");
          else
            empty3 += "No\r\n";
        }
        else
          empty3 += "\r\n";
      }
      else if (this.FltVal(id15) != 0.0 && num6 != 0.0)
      {
        if (col == 0 && !getDetail)
          this.SetVal("PREQUAL.X313", "Yes");
        else
          empty3 += "Yes\r\n";
      }
      else
        empty3 += "\r\n";
      if (this.FltVal(id16) > num7)
      {
        string str17 = ">> Borrower's Monthly Debt is not qualified. Maximum Debt: $" + num7.ToString("N2") + ".\r\n";
        empty1 += str17;
        str1 = str1 + str17 + "Current Debt: $" + this.Val(id16) + "\t" + id16 + "\r\n";
        flag = false;
        if (this.FltVal(id16) != 0.0 && num7 != 0.0)
        {
          if (col == 0 && !getDetail)
            this.SetVal("PREQUAL.X314", "No");
          else
            empty3 += "No\r\n";
        }
        else
          empty3 += "\r\n";
      }
      else if (this.FltVal(id16) != 0.0 && num7 != 0.0)
      {
        if (col == 0 && !getDetail)
          this.SetVal("PREQUAL.X314", "Yes");
        else
          empty3 += "Yes\r\n";
      }
      else
        empty3 += "\r\n";
      if (this.FltVal(id17) < this.FltVal(id9))
      {
        string str18 = ">> Borrower's Cash and Deposit are not enough. Cash Needed: $" + (object) this.FltVal(id9) + ".\r\n";
        empty1 += str18;
        str1 = str1 + str18 + "Current Cash/Deposit: $" + this.Val(id17) + "\t" + id17 + "\r\n";
        flag = false;
        if (this.FltVal(id17) != 0.0)
        {
          if (col == 0 && !getDetail)
            this.SetVal("PREQUAL.X315", "No");
          else
            empty3 += "No\r\n";
        }
        else
          empty3 += "\r\n";
      }
      else if (this.FltVal(id17) != 0.0)
      {
        if (col == 0 && !getDetail)
          this.SetVal("PREQUAL.X315", "Yes");
        else
          empty3 += "Yes\r\n";
      }
      else
        empty3 += "\r\n";
      if (flag)
      {
        if (num1 == 0.0 || num2 == 0.0 || num3 == 0.0 || num5 == 0.0 || num5 == 0.0 || num6 == 0.0)
        {
          if (!getDetail)
            this.SetVal(id18, "Yellow");
        }
        else if (!getDetail)
          this.SetVal(id18, "Green");
        if (!getDetail)
          empty1 = string.Empty;
      }
      else if (num1 == 0.0 && num2 == 0.0 && num3 == 0.0 && num5 == 0.0 && num5 == 0.0 && num6 == 0.0)
      {
        if (!getDetail)
          this.SetVal(id18, "Yellow");
      }
      else if (!getDetail)
        this.SetVal(id18, "Red");
      if (!getDetail)
        return string.Empty;
      string str19 = "\r\n*** Qualified Check ***\r\n\t" + empty3;
      return !(str1 != string.Empty) ? empty1 + str19 : empty1 + "\r\n\r\n*** Detail ***\r\n\r\n" + str1 + str19;
    }

    private double calcLTVRatio(bool getLTV, int option)
    {
      double num1 = this.FltVal("356");
      double num2 = option != 1 ? this.FltVal("PREQUAL.X73") : this.FltVal("PREQUAL.X33");
      string empty = string.Empty;
      string str = option != 1 ? this.Val("LP0205") : this.Val("LP0105");
      double num3 = num1 <= num2 || str.IndexOf("Refinance") >= 0 ? (num1 <= 0.0 ? num2 : num1) : num2;
      if (num3 <= 0.0)
        return 0.0;
      double num4 = option != 1 ? this.FltVal("PREQUAL.X74") / num3 * 100.0 : this.FltVal("PREQUAL.X34") / num3 * 100.0;
      if (getLTV)
        return num4;
      double num5 = this.FltVal("427") + this.FltVal("428");
      if (this.Val("420") != "SecondLien")
        num5 += this.FltVal("1732");
      return num5 / num3 * 100.0;
    }

    private void calculateLockRequestLoan(string id, string val)
    {
      bool flag1 = this.Val("4745") == "Y";
      bool flag2 = this.Val("2952") == "FHA";
      bool flag3 = this.Val("3046") == "Y";
      double num1 = this.FltVal("3047");
      double num2 = this.FltVal("3043");
      if (id == "3043" || id == "2952")
      {
        if (flag2)
          num2 = (double) (int) Utils.ParseDouble((object) this.FltVal("3043"));
        else if (flag1)
          num2 = Utils.ArithmeticRounding(Utils.ParseDouble((object) this.FltVal("3043")), 0);
        this.SetCurrentNum("3043", num2);
      }
      if (flag3)
      {
        this.SetCurrentNum("3044", num2 == 0.0 ? 0.0 : Utils.ArithmeticRounding(this.FltVal("3045") / num2 * 100.0, 6));
        if (this.Val("3056") == "Y")
        {
          double num3 = this.FltVal("3045");
          num1 = num3 - (double) ((int) (num3 / 50.0) * 50);
          this.SetCurrentNum("3047", num1);
        }
      }
      else
      {
        double num4 = !flag2 ? Utils.ArithmeticRounding(num2 * this.FltVal("3044") / 100.0, 2) : Convert.ToDouble(Math.Truncate(100M * Convert.ToDecimal(num2 * this.FltVal("3044") / 100.0)) / 100M);
        this.SetCurrentNum("3045", num4);
        num1 = !(this.Val("3056") == "Y") ? Math.Round(num4 % 1.0, 2) : num4 - (double) ((int) (num4 / 50.0) * 50);
        this.SetCurrentNum("3047", num1);
      }
      double num5 = num2 + this.FltVal("3045");
      double num6 = this.FltVal("3047") + this.FltVal("3049") <= this.FltVal("3045") ? num5 - (this.FltVal("3047") + this.FltVal("3049")) : num5 - this.FltVal("3045");
      double num7 = num6;
      if (flag2)
        num7 = (double) (int) num6;
      else if (flag1)
        num7 = Utils.ArithmeticRounding(num6, 0);
      if (num7 != num6)
      {
        this.SetCurrentNum("3047", Utils.ArithmeticRounding(num7 >= num6 ? num1 - (num7 - num6) : num1 + num6 - num7, 2));
        num6 = num7;
      }
      this.SetCurrentNum("2965", num6);
      if (!this.calObjs.D1003Cal.IsHELOCOrOtherLoan && id == "2958")
      {
        string str1 = this.Val("420");
        if (string.IsNullOrWhiteSpace(str1))
          str1 = "FirstLien";
        string str2 = val;
        if (string.IsNullOrWhiteSpace(str2))
          str1 = "FirstLien";
        if (str2 != str1)
        {
          this.SetVal("3035", this.Val("428"));
          this.SetVal("3036", this.Val("427"));
        }
        else
        {
          this.SetVal("3035", this.Val("427"));
          this.SetVal("3036", this.Val("428"));
        }
      }
      this.calculateLTV(id, num6);
    }

    private void calculateLoanAmount(string id, string val)
    {
      double num1 = this.FltVal("2");
      if (this.Val("1765") == "Y")
      {
        this.SetNum("1109", num1 - this.FltVal("1045"));
      }
      else
      {
        double num2 = this.FltVal("1107");
        if (num2 != 0.0)
          num1 /= 1.0 + num2 / 100.0;
        this.SetNum("1109", num1);
      }
    }

    private void calculateLTV(string fieldID, double totalLoanAmount)
    {
      string str = this.Val("2951");
      double num1 = this.FltVal("2949");
      double num2 = this.FltVal("3038");
      double num3 = this.FltVal("2948");
      double num4 = this.FltVal("3043");
      double num5 = 0.0;
      double num6 = 0.0;
      bool flag1 = this.Val("2952") == "FarmersHomeAdministration";
      bool flag2 = this.Val("2952") == "FHA" && this.Val("3844") == "Y";
      bool flag3 = this.Val("2952") == "Conventional";
      bool flag4 = this.Val("2952") == "HELOC";
      bool flag5 = this.Val("2952") == "Other";
      bool flag6 = this.Val("2952") == "FHA";
      bool flag7 = this.Val("2952") == "VA";
      bool flag8 = this.Val("2958") == "SecondLien";
      bool flag9 = this.Val("4254") == "Y";
      bool flag10 = this.Val("4255") == "Y";
      double num7 = this.FltVal("CASASRN.X168");
      if (flag4)
      {
        double num8 = this.FltVal("1109");
        num7 += num4 - num8;
        this.SetCurrentNum("4519", num7);
      }
      this.loan.GetNumberOfLiabilitesExlcudingAlimonyJobExp();
      bool flag11 = this.loan.LinkSyncType == LinkSyncType.ConstructionPrimary || this.loan.LinkSyncType == LinkSyncType.ConstructionLinked;
      double num9 = Utils.ParseDouble((object) this.Val("CASASRN.X167"));
      if (flag4)
      {
        double num10 = Utils.ParseDouble((object) this.Val("4510"));
        double num11 = Utils.ParseDouble((object) this.Val("1888"));
        if (num10 != num11)
          num9 += num10 - num11;
      }
      this.SetVal("3846", num9.ToString("N2"));
      if (!(str.IndexOf("Construction") >= 0 & flag10))
      {
        double num12;
        if (flag1)
        {
          num4 = totalLoanAmount;
          num12 = num1;
          if (num12 == 0.0)
            num12 = num2;
        }
        else if (flag2)
        {
          num12 = str.IndexOf("Refinance") <= -1 ? this.FltVal("MAX23K.X86") : this.FltVal("3845");
        }
        else
        {
          num12 = num1 <= num2 || str.IndexOf("Refinance") >= 0 ? (num1 <= 0.0 ? num2 : num1) : (!(str == "Other") || num2 != 0.0 ? num2 : num1);
          if (str == "Purchase" && this.Val("1172") == "FHA" & this.Val("3844") == "Y")
            num12 = num2 + this.FltVal("967");
        }
        if (num12 == 0.0)
          num12 = num3;
        if (flag3)
          num4 = totalLoanAmount;
        if (flag1 | flag7 | flag6)
          num5 = num6 = this.FltVal("3035") + this.FltVal("3036") + num7;
        else if (flag3 | flag4 | flag5)
        {
          num5 = this.FltVal("3035") + this.FltVal("3036") + num9;
          num6 = this.FltVal("3035") + this.FltVal("3036") + num7;
        }
        if (flag3 | flag1 || flag7 && this.Val("958") != "IRRRL" && (this.Val("2951") == "NoCash-Out Refinance" || this.Val("2951") == "Cash-Out Refinance"))
        {
          num5 += this.FltVal("2965") - this.FltVal("3043");
          num6 += this.FltVal("2965") - this.FltVal("3043");
        }
        if (!flag4)
        {
          num5 += this.FltVal("3043") - this.FltVal("1109");
          num6 += this.FltVal("3043") - this.FltVal("1109");
        }
        if (num12 > 0.0)
        {
          if (flag7 && this.Val("958") != "IRRRL" && (this.Val("2951") == "NoCash-Out Refinance" || this.Val("2951") == "Cash-Out Refinance"))
            this.SetCurrentNum("3241", this.FltVal("2965") / num12 * 100.0);
          else
            this.SetCurrentNum("3241", num4 / num12 * 100.0);
          this.SetCurrentNum("3242", num5 / num12 * 100.0);
          this.SetCurrentNum("4514", num6 / num12 * 100.0);
        }
        else
        {
          this.SetCurrentNum("3241", 0.0);
          this.SetCurrentNum("3242", 0.0);
          this.SetCurrentNum("4514", 0.0);
        }
      }
      else
      {
        double num13 = this.FltVal("2965");
        double num14 = this.FltVal("3036") + this.FltVal("3037");
        if (num1 > 0.0 && num13 > 0.0)
        {
          this.SetCurrentNum("3241", num13 / num1 * 100.0);
          this.SetCurrentNum("3242", (num13 + num14) / num1 * 100.0);
        }
        else
        {
          this.SetCurrentNum("3241", 0.0);
          this.SetCurrentNum("3242", 0.0);
        }
      }
    }

    private void copyConstructionFields(string id, string val)
    {
      if (this.Val("2951").IndexOf("Construction") >= 0)
      {
        this.SetCurrentNum("2949", this.FltVal("CONST.X59"));
        this.SetCurrentNum("3038", this.Val("4255") == "Y" ? 0.0 : this.FltVal("136"));
      }
      else
      {
        this.SetCurrentNum("2949", this.FltVal("356"));
        this.SetCurrentNum("3038", this.FltVal("136"));
      }
    }

    private void copyBorrowerLenderPaid(string id, string val)
    {
      this.SetVal("4463", this.Val("LCP.X1"));
    }

    private void calcOnSaveCopyBorrowerLenderPaid(string id, string val)
    {
      if (!string.IsNullOrEmpty(this.Val("4463")))
        return;
      this.SetVal("4463", this.Val("LCP.X1"));
    }

    private void calcFHASecondaryResidence(string id, string val)
    {
      if (this.Val("2952") != "FHA" || this.Val("2950") != "SecondHome")
        this.SetVal("4515", "N");
      else
        this.SetVal("4515", this.Val("URLA.X76"));
    }

    private void calcHELOCLockRequest(string id, string val)
    {
      if (id == "1888")
        this.SetVal("4510", this.Val("1888"));
      if (id == "CASASRN.X168")
        this.SetVal("4519", this.Val("CASASRN.X168"));
      if (id == "1482")
        this.SetVal("4511", this.Val("1482"));
      if (id == "1959")
        this.SetVal("4512", this.Val("1959"));
      if (!(id == "688"))
        return;
      this.SetVal("4513", this.Val("688"));
    }

    private void clearLockRequestFreeAndClear(string id, string val)
    {
      if (this.Val("4255") != "Y")
        this.SetVal("5038", "");
      if (this.Val("2951").StartsWith("Construction"))
        return;
      this.SetVal("5038", "");
    }
  }
}
