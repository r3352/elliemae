// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.CalculationEngine.PreCalculation
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using System;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.CalculationEngine
{
  internal class PreCalculation : CalculationBase
  {
    private const string className = "PreCalculation�";
    private static readonly string sw = Tracing.SwDataEngine;
    internal Routine UpdateF19;

    internal PreCalculation(LoanData l, EllieMae.EMLite.CalculationEngine.CalculationObjects calObjs)
      : base(l, calObjs)
    {
      this.UpdateF19 = this.RoutineX(new Routine(this.updateField19));
    }

    internal void CalculateMaxLoanAmt()
    {
      if (Tracing.IsSwitchActive(PreCalculation.sw, TraceLevel.Info))
        Tracing.Log(PreCalculation.sw, TraceLevel.Info, nameof (PreCalculation), "routine: CalculateFHAMaxLoanAmt");
      string str1 = this.Val("420");
      double num1 = this.FltVal("3") / 1200.0;
      double y = (double) this.IntVal("4");
      double num2 = this.FltVal("1389");
      double num3 = this.FltVal("136");
      string str2 = this.Val("1811");
      if (this.Val("19").IndexOf("Refinance") > -1 && this.FltVal("356") > 0.0)
        num3 = this.FltVal("356");
      double num4 = 0.0;
      if (y > 0.0)
        num4 = Math.Pow(1.0 + num1, y);
      double num5 = this.FltVal("PREQUAL.X209") / 100.0 * num3;
      double num6 = this.FltVal("PREQUAL.X210") / 100.0 * num3;
      double num7 = !(str1 != "SecondLien") ? num6 - this.FltVal("427") : num6 - this.FltVal("428");
      double num8 = 0.0;
      if (str2 != "SecondHome" && str2 != "Investor")
      {
        double num9 = this.FltVal("1790") / 100.0 * num2 - this.FltVal("912");
        double num10 = !(str1 != "SecondLien") ? num9 + this.FltVal("229") : num9 + this.FltVal("228");
        num8 = num1 <= 0.0 || num4 <= 0.0 ? 0.0 : num10 / (num1 * num4 / (num4 - 1.0));
      }
      double num11 = this.FltVal("1791") / 100.0 * num2;
      double num12;
      if (str2 == "SecondHome" || str2 == "Investor")
      {
        double num13 = num11 - this.FltVal("1742");
        num12 = num1 <= 0.0 || num4 <= 0.0 ? 0.0 : num13 / (num1 * num4 / (num4 - 1.0));
      }
      else
      {
        double num14 = num11 - this.FltVal("912");
        double num15 = (!(str1 == "SecondLien") ? num14 + this.FltVal("228") : num14 + this.FltVal("229")) - (this.FltVal("1742") - this.FltVal("1731"));
        num12 = num1 <= 0.0 || num4 <= 0.0 ? 0.0 : num15 / (num1 * num4 / (num4 - 1.0));
      }
      double[] numArray = new double[5]
      {
        num5,
        num7,
        num8,
        num12,
        this.FltVal("PREQUAL.X205")
      };
      for (int index1 = 0; index1 < 4; ++index1)
      {
        for (int index2 = index1 + 1; index2 < 5; ++index2)
        {
          if (numArray[index1] > numArray[index2])
          {
            double num16 = numArray[index1];
            numArray[index1] = numArray[index2];
            numArray[index2] = num16;
          }
        }
      }
      double num17 = 0.0;
      for (int index = 0; index < 5; ++index)
      {
        if (numArray[index] > 0.0)
        {
          num17 = numArray[index];
          break;
        }
      }
      this.SetCurrentNum("PREQUAL.X202", num17);
      double num18 = this.FltVal("1335");
      this.SetCurrentNum("PREQUAL.X203", num18);
      this.SetCurrentNum("PREQUAL.X204", num17 + num18);
    }

    internal double CalculateFHAMaxLoanAmt(double propertyValue, bool getFactor)
    {
      if (Tracing.IsSwitchActive(PreCalculation.sw, TraceLevel.Info))
        Tracing.Log(PreCalculation.sw, TraceLevel.Info, nameof (PreCalculation), "routine: CalculateFHAMaxLoanAmt");
      if (this.Val("1172") != "FHA")
        return 0.0;
      double fhaMaxLoanAmt1 = 0.965;
      if (this.Val("1811", this.GetBorrowerPairs()[0]) == "Investor" && this.Val("2999") == "Y")
        fhaMaxLoanAmt1 = 0.75;
      if (getFactor)
        return fhaMaxLoanAmt1;
      double fhaMaxLoanAmt2 = propertyValue * fhaMaxLoanAmt1;
      double num = this.FltVal("PREQUAL.X205");
      if (fhaMaxLoanAmt2 > num && num > 0.0)
        fhaMaxLoanAmt2 = num;
      return fhaMaxLoanAmt2;
    }

    private void updateField19(string id, string val)
    {
      if (Tracing.IsSwitchActive(PreCalculation.sw, TraceLevel.Info))
        Tracing.Log(PreCalculation.sw, TraceLevel.Info, nameof (PreCalculation), "routine: updateField19 ID: " + id);
      string str = this.Val("19");
      if (str.IndexOf("Refinance") < 0 && str.IndexOf("Construction") < 0)
      {
        this.SetVal("24", string.Empty);
        this.SetVal("25", string.Empty);
        this.SetVal("26", string.Empty);
        this.SetVal("VASUMM.X149", string.Empty);
        this.SetVal("299", string.Empty);
        this.SetVal("30", string.Empty);
        this.SetVal("205", string.Empty);
        this.SetVal("29", string.Empty);
        this.SetVal("URLA.X165", string.Empty);
        this.SetVal("URLA.X166", string.Empty);
        this.SetVal("URLA.X167", string.Empty);
      }
      if (str.IndexOf("Refinance") < 0)
        this.SetVal("QM.X2", string.Empty);
      if (!(str != "Other"))
        return;
      if (str == "ConstructionOnly" && this.Val("Constr.Refi") != "Y" && this.Val("1964") != "Y")
        this.SetVal("9", "Construction Only");
      else
        this.SetVal("9", string.Empty);
    }
  }
}
