// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.CalculationEngine.ComortCalculation
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.CalculationEngine
{
  internal class ComortCalculation : CalculationBase
  {
    private const string className = "ComortCalculation�";
    private static readonly string sw = Tracing.SwDataEngine;
    internal Routine CalcComortgagor;

    internal ComortCalculation(LoanData l, EllieMae.EMLite.CalculationEngine.CalculationObjects calObjs)
      : base(l, calObjs)
    {
      this.CalcComortgagor = this.RoutineX(new Routine(this.calculateComortgagor));
    }

    private void calculateComortgagor(string id, string val)
    {
      if (Tracing.IsSwitchActive(ComortCalculation.sw, TraceLevel.Info))
        Tracing.Log(ComortCalculation.sw, TraceLevel.Info, nameof (ComortCalculation), "routine: calculateComortgagor ID: " + id);
      double num1 = 0.0;
      BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
      double num2 = 0.0;
      double num3 = 0.0;
      for (int index = 0; index < borrowerPairs.Length; ++index)
      {
        double num4 = this.FltVal("273", borrowerPairs[index]) + this.FltVal("1168", borrowerPairs[index]) + this.FltVal("1171", borrowerPairs[index]) + this.FltVal("1817", borrowerPairs[index]);
        if (index > 0)
        {
          this.SetCurrentNum("1374", 0.0, borrowerPairs[index]);
          this.SetCurrentNum("1389", num4, borrowerPairs[index]);
          num2 += num4;
        }
        else
          num3 = num4;
        this.SetCurrentNum("1758", this.FltVal("101", borrowerPairs[index]) + this.FltVal("1145", borrowerPairs[index]) + this.FltVal("1169", borrowerPairs[index]) + this.FltVal("1374", borrowerPairs[index]) + this.FltVal("1815", borrowerPairs[index]), borrowerPairs[index]);
        this.SetCurrentNum("1759", this.FltVal("110", borrowerPairs[index]) + this.FltVal("1146", borrowerPairs[index]) + this.FltVal("1170", borrowerPairs[index]) + this.FltVal("1816", borrowerPairs[index]), borrowerPairs[index]);
        this.SetCurrentNum("938", this.FltVal("911", borrowerPairs[index]) - this.FltVal("110", borrowerPairs[index]) - this.FltVal("115", borrowerPairs[index]), borrowerPairs[index]);
      }
      this.SetCurrentNum("1374", num2, borrowerPairs[0]);
      if (borrowerPairs.Length > 1)
        this.SetCurrentNum("938", this.FltVal("938", borrowerPairs[0]) + num2, borrowerPairs[0]);
      double num5 = this.FltVal("101", borrowerPairs[0]) + this.FltVal("1145", borrowerPairs[0]) + this.FltVal("1169", borrowerPairs[0]) + this.FltVal("1815", borrowerPairs[0]) + num2;
      this.SetCurrentNum("1758", num5, borrowerPairs[0]);
      this.SetCurrentNum("1389", num5 + this.FltVal("1759", borrowerPairs[0]), borrowerPairs[0]);
      if (this.loan.CurrentBorrowerPair.Id != borrowerPairs[0].Id)
        this.loan.Calculator.FormCalculation("HMDAINCOME");
      this.SetCurrentNum("1389", this.FltVal("1389"));
      this.SetCurrentNum("1374", this.FltVal("1374"));
      this.SetCurrentNum("1758", this.FltVal("1758"));
      this.SetCurrentNum("1759", this.FltVal("1759"));
      double num6 = 0.0;
      for (int index = 0; index < borrowerPairs.Length; ++index)
      {
        double num7 = this.FltVal("915", borrowerPairs[index]) - this.FltVal("183", borrowerPairs[index]) - this.FltVal("1716", borrowerPairs[index]);
        if (num7 > 0.0)
          num6 += num7;
        if (index > 0 && num7 > 0.0)
          this.SetCurrentNum("1094", num7, borrowerPairs[index]);
      }
      this.SetCurrentNum("1094", num6, borrowerPairs[0]);
      this.calObjs.FHACal.CalcFredMac(id, val);
      double num8 = 0.0;
      double num9 = 0.0;
      double num10 = 0.0;
      double num11 = 0.0;
      double num12 = 0.0;
      for (int index = 1; index < borrowerPairs.Length; ++index)
      {
        this.addProposedExpense(borrowerPairs[index], false);
        this.SetCurrentNum("1379", 0.0, borrowerPairs[index]);
        this.SetCurrentNum("1384", 0.0, borrowerPairs[index]);
        string str = this.Val("1811", borrowerPairs[index]);
        double num13 = this.FltVal("1731", borrowerPairs[index]);
        double num14 = this.FltVal("1733", borrowerPairs[index]);
        if (str == string.Empty || str == "N" || str == "Investor" || str == "SecondHome")
          num8 += num13;
        num9 += num14;
        if (str != "PrimaryResidence")
        {
          num10 += num13;
          num12 = num12 + this.FltVal("1742", borrowerPairs[index]) - this.FltVal("462", borrowerPairs[index]);
        }
        else
          num12 += num14;
        if (str == "PrimaryResidence" && (this.Val("418", borrowerPairs[index]) == "No" || this.Val("1343", borrowerPairs[index]) == "No"))
          num11 += this.FltVal("737", borrowerPairs[index]);
      }
      this.SetCurrentNum("1379", num10 + num11, borrowerPairs[0]);
      this.SetCurrentNum("1384", num12 - num10, borrowerPairs[0]);
      this.SetCurrentNum("1731", this.FltVal("1731"));
      this.SetCurrentNum("1742", this.FltVal("1742"));
      this.addProposedExpense(borrowerPairs[0], true);
      this.SetCurrentNum("267", num8, borrowerPairs[0]);
      this.SetCurrentNum("268", num8, borrowerPairs[0]);
      this.SetCurrentNum("CASASRN.X131", num8, borrowerPairs[0]);
      this.SetCurrentNum("CASASRN.X174", num9, borrowerPairs[0]);
      double num15 = 0.0;
      for (int index = 119; index <= 126; ++index)
        num15 += this.FltVal(index.ToString(), borrowerPairs[0]);
      if (this.USEURLA2020)
        num15 += this.FltVal("URLA.X212", borrowerPairs[0]);
      this.SetCurrentNum("737", num15, borrowerPairs[0]);
      this.SetCurrentNum("1299", num15 + this.FltVal("1379", borrowerPairs[0]), borrowerPairs[0]);
      this.SetCurrentNum("1299", this.FltVal("1299"));
      num1 = 0.0;
      for (int index = 0; index < borrowerPairs.Length; ++index)
      {
        if (this.Val("1811", borrowerPairs[index]) == "PrimaryResidence")
          this.SetCurrentNum("1809", this.FltVal("912", borrowerPairs[index]), borrowerPairs[index]);
        else
          this.SetCurrentNum("1809", this.FltVal("1299", borrowerPairs[index]), borrowerPairs[index]);
        double num16 = this.FltVal("1809", borrowerPairs[index]) + this.FltVal("382", borrowerPairs[index]) + this.FltVal("LOANSUB.X1", borrowerPairs[index]) + this.FltVal("272", borrowerPairs[index]) + this.FltVal("1476", borrowerPairs[index]) + this.FltVal("462", borrowerPairs[index]);
        if (index == 0)
          num16 += this.FltVal("1384", borrowerPairs[index]);
        this.SetCurrentNum("1187", num16, borrowerPairs[index]);
      }
      this.SetCurrentNum("1809", this.FltVal("1809"));
      this.SetCurrentNum("1187", this.FltVal("1187"));
      for (int index = borrowerPairs.Length - 1; index >= 0; --index)
      {
        double num17 = this.FltVal("924", borrowerPairs[index]);
        double num18 = num17 >= 0.0 ? 0.0 : num17 * -1.0;
        if (index == 0)
          num18 += this.FltVal("1379", borrowerPairs[index]) + this.FltVal("1384", borrowerPairs[index]);
        if (this.Val("1811", borrowerPairs[index]) != "PrimaryResidence")
          num18 += num18 + this.FltVal("737", borrowerPairs[index]);
        this.SetCurrentNum("1161", num18, borrowerPairs[index]);
        this.SetCurrentNum("1150", this.FltVal("382", borrowerPairs[index]) + this.FltVal("272", borrowerPairs[index]) + this.FltVal("1161", borrowerPairs[index]), borrowerPairs[index]);
      }
      this.SetCurrentNum("1161", this.FltVal("1161"));
      this.SetCurrentNum("1150", this.FltVal("1150"));
      this.calObjs.FHACal.CalcEEMX88AndEEMX89(id, val);
    }

    private void addProposedExpense(BorrowerPair pair, bool isPrimary)
    {
      double num1 = 0.0;
      for (int index = 1723; index <= 1730; ++index)
        num1 += this.FltVal(index.ToString(), pair);
      if (isPrimary)
        num1 += this.FltVal("1379", pair);
      double num2 = num1 + this.FltVal("4947", pair);
      this.SetCurrentNum("1731", num2, pair);
      this.SetCurrentNum("1733", this.FltVal("LOANSUB.X1", pair) + this.FltVal("1476", pair) + this.FltVal("382", pair) + this.FltVal("272", pair), pair);
      double num3 = num2 + (this.FltVal("462", pair) + this.FltVal("1733", pair));
      if (isPrimary)
        num3 += this.FltVal("1384", pair);
      this.SetCurrentNum("1742", num3, pair);
    }
  }
}
