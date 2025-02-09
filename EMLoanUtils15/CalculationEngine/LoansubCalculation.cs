// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.CalculationEngine.LoansubCalculation
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.CalculationEngine
{
  internal class LoansubCalculation : CalculationBase
  {
    private const string className = "LoansubCalculation�";
    private static readonly string sw = Tracing.SwDataEngine;
    internal Routine CalcLoansub;

    internal LoansubCalculation(LoanData l, EllieMae.EMLite.CalculationEngine.CalculationObjects calObjs)
      : base(l, calObjs)
    {
      this.CalcLoansub = this.RoutineX(new Routine(this.calculateLoansub));
      this.addFieldHandlers();
    }

    private void addFieldHandlers()
    {
      Routine routine = this.RoutineX(new Routine(this.calculateLoansub));
      this.AddFieldHandler("439", routine);
      this.AddFieldHandler("437", routine);
      this.AddFieldHandler("440", routine);
      this.AddFieldHandler("441", routine);
      this.AddFieldHandler("455", routine);
      this.AddFieldHandler("456", routine);
      this.AddFieldHandler("457", routine);
      this.AddFieldHandler("458", routine);
      this.AddFieldHandler("459", routine);
      this.AddFieldHandler("406", routine);
      this.AddFieldHandler("469", routine);
      this.AddFieldHandler("225", routine);
      this.AddFieldHandler("237", routine);
      this.AddFieldHandler("242", routine);
      this.AddFieldHandler("605", routine);
      this.AddFieldHandler("606", routine);
      this.AddFieldHandler("442", routine);
      this.AddFieldHandler("450", routine);
      this.AddFieldHandler("451", routine);
      this.AddFieldHandler("452", routine);
      this.AddFieldHandler("453", routine);
      this.AddFieldHandler("407", routine);
      this.AddFieldHandler("443", routine);
      this.AddFieldHandler("226", routine);
      this.AddFieldHandler("239", routine);
      this.AddFieldHandler("244", routine);
    }

    private void calculateLoansub(string id, string val)
    {
      if (Tracing.IsSwitchActive(LoansubCalculation.sw, TraceLevel.Info))
        Tracing.Log(LoansubCalculation.sw, TraceLevel.Info, nameof (LoansubCalculation), "routine: calculateLoansub ID: " + id);
      this.SetCurrentNum("438", this.FltVal("1061") / 100.0 * this.FltVal("2") + this.FltVal("436"));
      this.SetCurrentNum("2591", this.FltVal("454") + this.FltVal("1619") + this.FltVal("559"));
      string id1 = string.Empty;
      string id2 = string.Empty;
      string id3 = string.Empty;
      double num1 = 0.0;
      double num2 = 0.0;
      for (int index = 1; index < 15; ++index)
      {
        switch (index - 1)
        {
          case 0:
            id1 = "2591";
            id2 = "439";
            id3 = "330";
            break;
          case 1:
            id1 = "438";
            id2 = "437";
            id3 = "331";
            break;
          case 2:
            id1 = "440";
            id2 = "605";
            id3 = "LOANSUB.X3";
            break;
          case 3:
            id1 = "441";
            id2 = "606";
            id3 = "LOANSUB.X4";
            break;
          case 4:
            id1 = "455";
            id2 = "442";
            id3 = "LOANSUB.X5";
            break;
          case 5:
            id1 = "456";
            id2 = "450";
            id3 = "LOANSUB.X6";
            break;
          case 6:
            id1 = "457";
            id2 = "451";
            id3 = "444";
            break;
          case 7:
            id1 = "458";
            id2 = "452";
            id3 = "LOANSUB.X2";
            break;
          case 8:
            id1 = "459";
            id2 = "453";
            id3 = "446";
            break;
          case 9:
            id1 = "406";
            id2 = "407";
            id3 = "408";
            break;
          case 10:
            id1 = "469";
            id2 = "443";
            id3 = "445";
            break;
          case 11:
            id1 = "225";
            id2 = "226";
            id3 = "227";
            break;
          case 12:
            id1 = "237";
            id2 = "239";
            id3 = "240";
            break;
          case 13:
            id1 = "242";
            id2 = "244";
            id3 = "245";
            break;
        }
        double num3 = this.FltVal(id1);
        double num4 = this.FltVal(id2);
        double num5 = num3 + num4;
        this.SetCurrentNum(id3, num5);
        num1 += num3;
        num2 += num4;
      }
      this.SetCurrentNum("433", num1);
      this.SetCurrentNum("434", num2);
      this.SetCurrentNum("435", num1 + num2);
    }
  }
}
