// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.CalculationEngine.FM1084Calculation
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.CalculationEngine
{
  internal class FM1084Calculation : CalculationBase
  {
    private const string className = "FM1084Calculation�";
    private static readonly string sw = Tracing.SwDataEngine;

    internal FM1084Calculation(LoanData l, EllieMae.EMLite.CalculationEngine.CalculationObjects calObjs)
      : base(l, calObjs)
    {
      this.addFieldHandlers();
    }

    private void addFieldHandlers()
    {
      Routine routine1 = this.RoutineX(new Routine(this.calculationYear1IncomeTax)) + this.RoutineX(new Routine(this.calculateYearToDateProfit));
      this.AddFieldHandler("FM1084.X7", routine1);
      for (int index = 23; index <= 48; ++index)
      {
        if (index != 32 && index != 33 && index != 37 && index != 43)
          this.AddFieldHandler("FM1084.X" + (object) index, routine1);
      }
      for (int index = 97; index <= 129; ++index)
        this.AddFieldHandler("FM1084.X" + (object) index, routine1);
      this.AddFieldHandler("FM1084.X178", routine1);
      this.AddFieldHandler("FM1084.X180", routine1);
      Routine routine2 = this.RoutineX(new Routine(this.calculationYear2IncomeTax)) + this.RoutineX(new Routine(this.calculateYearToDateProfit));
      this.AddFieldHandler("FM1084.X51", routine2);
      for (int index = 66; index <= 91; ++index)
      {
        if (index != 75 && index != 76 && index != 80 && index != 86)
          this.AddFieldHandler("FM1084.X" + (object) index, routine2);
      }
      for (int index = 135; index <= 165; ++index)
      {
        if (index != 163)
          this.AddFieldHandler("FM1084.X" + (object) index, routine2);
      }
      this.AddFieldHandler("FM1084.X179", routine2);
      this.AddFieldHandler("FM1084.X181", routine2);
      Routine routine3 = this.RoutineX(new Routine(this.calculateYearToDateProfit));
      this.AddFieldHandler("FM1084.X169", routine3);
      this.AddFieldHandler("FM1084.X170", routine3);
      this.AddFieldHandler("FM1084.X171", routine3);
      this.AddFieldHandler("FM1084.X173", routine3);
      this.AddFieldHandler("FM1084.X174", routine3);
    }

    internal void FormCal()
    {
      if (Tracing.IsSwitchActive(FM1084Calculation.sw, TraceLevel.Info))
        Tracing.Log(FM1084Calculation.sw, TraceLevel.Info, nameof (FM1084Calculation), "routine: FormCal");
      this.calculationYear1IncomeTax((string) null, (string) null);
      this.calculationYear2IncomeTax((string) null, (string) null);
      this.calculateYearToDateProfit((string) null, (string) null);
    }

    private void calculationYear1IncomeTax(string id, string val)
    {
      this.SetCurrentNum("FM1084.X131", this.FltVal("FM1084.X7") + this.FltVal("FM1084.X23") + this.FltVal("FM1084.X24") + this.FltVal("FM1084.X178") + this.FltVal("FM1084.X25") + this.FltVal("FM1084.X26") + this.FltVal("FM1084.X27") - this.FltVal("FM1084.X28") + this.FltVal("FM1084.X29") + this.FltVal("FM1084.X30") + this.FltVal("FM1084.X31") + this.FltVal("FM1084.X34") - this.FltVal("FM1084.X35") + this.FltVal("FM1084.X36") + this.FltVal("FM1084.X180") + this.FltVal("FM1084.X38") + this.FltVal("FM1084.X39") + this.FltVal("FM1084.X40") + this.FltVal("FM1084.X41") + this.FltVal("FM1084.X42") + this.FltVal("FM1084.X44") + this.FltVal("FM1084.X45") + this.FltVal("FM1084.X46") + this.FltVal("FM1084.X47") + this.FltVal("FM1084.X48"));
      double num1 = this.FltVal("FM1084.X97") + this.FltVal("FM1084.X98") + this.FltVal("FM1084.X99") + this.FltVal("FM1084.X100") + this.FltVal("FM1084.X101") - this.FltVal("FM1084.X102") - this.FltVal("FM1084.X103");
      this.SetCurrentNum("FM1084.X104", num1);
      double num2 = this.FltVal("FM1084.X105") / 100.0;
      this.SetCurrentNum("FM1084.X106", Utils.ArithmeticRounding(num1 * (num2 == 0.0 ? 1.0 : num2), 2));
      double num3 = this.FltVal("FM1084.X107") + this.FltVal("FM1084.X108") + this.FltVal("FM1084.X109") + this.FltVal("FM1084.X110") - this.FltVal("FM1084.X111") - this.FltVal("FM1084.X112");
      this.SetCurrentNum("FM1084.X113", num3);
      double num4 = this.FltVal("FM1084.X114") / 100.0;
      this.SetCurrentNum("FM1084.X115", Utils.ArithmeticRounding(num3 * (num4 == 0.0 ? 1.0 : num4), 2));
      double num5 = this.FltVal("FM1084.X116") - this.FltVal("FM1084.X117") + this.FltVal("FM1084.X118") + this.FltVal("FM1084.X119") + this.FltVal("FM1084.X120") + this.FltVal("FM1084.X121") + this.FltVal("FM1084.X122") + this.FltVal("FM1084.X123") - this.FltVal("FM1084.X124") - this.FltVal("FM1084.X125");
      this.SetCurrentNum("FM1084.X126", num5);
      this.SetCurrentNum("FM1084.X130", num5 - this.FltVal("FM1084.X129"));
      double num6 = this.FltVal("FM1084.X106") + this.FltVal("FM1084.X115") + this.FltVal("FM1084.X130");
      this.SetCurrentNum("FM1084.X132", num6);
      this.SetCurrentNum("FM1084.X133", num6 + this.FltVal("FM1084.X131"));
    }

    private void calculationYear2IncomeTax(string id, string val)
    {
      this.SetCurrentNum("FM1084.X166", this.FltVal("FM1084.X51") + this.FltVal("FM1084.X66") + this.FltVal("FM1084.X67") + this.FltVal("FM1084.X179") + this.FltVal("FM1084.X68") + this.FltVal("FM1084.X69") + this.FltVal("FM1084.X70") - this.FltVal("FM1084.X71") + this.FltVal("FM1084.X72") + this.FltVal("FM1084.X73") + this.FltVal("FM1084.X74") + this.FltVal("FM1084.X77") - this.FltVal("FM1084.X78") + this.FltVal("FM1084.X79") + this.FltVal("FM1084.X181") + this.FltVal("FM1084.X81") + this.FltVal("FM1084.X82") + this.FltVal("FM1084.X83") + this.FltVal("FM1084.X84") + this.FltVal("FM1084.X85") + this.FltVal("FM1084.X87") + this.FltVal("FM1084.X88") + this.FltVal("FM1084.X89") + this.FltVal("FM1084.X90") + this.FltVal("FM1084.X91"));
      double num1 = this.FltVal("FM1084.X135") + this.FltVal("FM1084.X136") + this.FltVal("FM1084.X137") + this.FltVal("FM1084.X138") + this.FltVal("FM1084.X139") - this.FltVal("FM1084.X140") - this.FltVal("FM1084.X141");
      this.SetCurrentNum("FM1084.X142", num1);
      double num2 = this.FltVal("FM1084.X105") / 100.0;
      this.SetCurrentNum("FM1084.X143", Utils.ArithmeticRounding(num1 * (num2 == 0.0 ? 1.0 : num2), 2));
      double num3 = this.FltVal("FM1084.X144") + this.FltVal("FM1084.X145") + this.FltVal("FM1084.X146") + this.FltVal("FM1084.X147") - this.FltVal("FM1084.X148") - this.FltVal("FM1084.X149");
      this.SetCurrentNum("FM1084.X150", num3);
      double num4 = this.FltVal("FM1084.X114") / 100.0;
      this.SetCurrentNum("FM1084.X151", Utils.ArithmeticRounding(num3 * (num4 == 0.0 ? 1.0 : num4), 2));
      double num5 = this.FltVal("FM1084.X152") - this.FltVal("FM1084.X153") + this.FltVal("FM1084.X154") + this.FltVal("FM1084.X155") + this.FltVal("FM1084.X156") + this.FltVal("FM1084.X157") + this.FltVal("FM1084.X158") + this.FltVal("FM1084.X159") - this.FltVal("FM1084.X160") - this.FltVal("FM1084.X161");
      this.SetCurrentNum("FM1084.X162", num5);
      this.SetCurrentNum("FM1084.X165", num5 - this.FltVal("FM1084.X164"));
      double num6 = this.FltVal("FM1084.X143") + this.FltVal("FM1084.X151") + this.FltVal("FM1084.X165");
      this.SetCurrentNum("FM1084.X167", num6);
      this.SetCurrentNum("FM1084.X168", num6 + this.FltVal("FM1084.X166"));
    }

    private void calculateYearToDateProfit(string id, string val)
    {
      double num1 = this.FltVal("FM1084.X171") / 100.0;
      this.SetCurrentNum("FM1084.X172", Utils.ArithmeticRounding(this.FltVal("FM1084.X170") * (num1 == 0.0 ? 1.0 : num1), 2));
      double num2 = this.FltVal("FM1084.X174") / 100.0;
      this.SetCurrentNum("FM1084.X175", Utils.ArithmeticRounding(this.FltVal("FM1084.X173") * (num2 == 0.0 ? 1.0 : num2), 2));
      this.SetCurrentNum("FM1084.X176", Utils.ArithmeticRounding(this.FltVal("FM1084.X169") + this.FltVal("FM1084.X172") + this.FltVal("FM1084.X175"), 2));
    }
  }
}
