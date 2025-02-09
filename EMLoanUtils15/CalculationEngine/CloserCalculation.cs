// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.CalculationEngine.CloserCalculation
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.LoanUtils.CalculationServants;
using System;

#nullable disable
namespace EllieMae.EMLite.CalculationEngine
{
  internal class CloserCalculation : CalculationBase
  {
    private const string className = "CloserCalculation�";
    private static readonly string sw = Tracing.SwDataEngine;
    internal Routine CalClosing;
    internal Routine CalOther;
    internal Routine calcModifiedTerms;
    private readonly CloserCalculationServant _closerCalculationServant;

    internal CloserCalculation(LoanData l, EllieMae.EMLite.CalculationEngine.CalculationObjects calObjs)
      : base(l, calObjs)
    {
      this.CalClosing = this.RoutineX(new Routine(this.calculateCloser));
      this.CalOther = this.RoutineX(new Routine(this.calculateOthers));
      this.calcModifiedTerms = this.RoutineX(new Routine(this.calculateModifiedTerms));
      this.addFieldHandlers();
      this._closerCalculationServant = new CloserCalculationServant((ILoanModelProvider) this);
    }

    private void addFieldHandlers()
    {
      Routine routine1 = this.RoutineX(new Routine(this.calculateCloser));
      this.AddFieldHandler("L138", routine1);
      this.AddFieldHandler("L141", routine1);
      this.AddFieldHandler("L145", routine1);
      this.AddFieldHandler("L149", routine1);
      this.AddFieldHandler("L153", routine1);
      this.AddFieldHandler("L187", routine1);
      this.AddFieldHandler("L189", routine1);
      this.AddFieldHandler("L191", routine1);
      this.AddFieldHandler("L193", routine1);
      this.AddFieldHandler("L195", routine1);
      this.AddFieldHandler("L197", routine1);
      this.AddFieldHandler("L199", routine1);
      this.AddFieldHandler("L201", routine1);
      this.AddFieldHandler("L209", routine1);
      this.AddFieldHandler("L215", routine1);
      this.AddFieldHandler("L216", routine1);
      this.AddFieldHandler("L218", routine1);
      this.AddFieldHandler("L219", routine1);
      this.AddFieldHandler("L351", routine1);
      this.AddFieldHandler("L352", routine1);
      this.AddFieldHandler("L725", routine1);
      Routine routine2 = this.RoutineX(new Routine(this.calculateModifiedTerms));
      this.AddFieldHandler("2991", routine2);
      this.AddFieldHandler("2992", routine2);
      this.AddFieldHandler("2993", routine2);
    }

    private void calculateCloser(string id, string val)
    {
      this.SetCurrentNum("L210", Math.Round(this.FltVal("L209") / 100.0 * this.FltVal("L725"), 2));
      this.SetCurrentNum("L351", this.UseNewGFEHUD || this.UseNew2015GFEHUD ? this.FltVal("NEWHUD.X773") : this.FltVal("304") + this.FltVal("L215") + this.FltVal("L218"));
      this.SetCurrentNum("HUD1A.X32", this.FltVal("2") + this.FltVal("HUD1A.X33") - this.FltVal("L351") - this.FltVal("HUD1A.X31"));
      this.SetCurrentNum("L352", this.UseNewGFEHUD || this.UseNew2015GFEHUD ? this.FltVal("NEWHUD.X774") : this.FltVal("143") + this.FltVal("L216") + this.FltVal("L219"));
      this.SetCurrentNum("L126", Math.Round(this.FltVal("136") + this.FltVal("L79") + this.FltVal("L351") + this.FltVal("L85") + this.FltVal("L89") + this.FltVal("L94") + this.FltVal("L100") + this.FltVal("L106") + this.FltVal("L111") + this.FltVal("L115") + this.FltVal("L119") + this.FltVal("L123"), 2));
      this.SetCurrentNum("L127", Math.Round(this.FltVal("136") + this.FltVal("L80") + this.FltVal("L82") + this.FltVal("L87") + this.FltVal("L91") + this.FltVal("L97") + this.FltVal("L103") + this.FltVal("L109") + this.FltVal("L113") + this.FltVal("L117") + this.FltVal("L121") + this.FltVal("L125"), 2));
      this.SetCurrentNum("L202", Math.Round(this.FltVal("2") + this.FltVal("L128") + this.FltVal("L132") + this.FltVal("L135") + this.FltVal("L138") + this.FltVal("L141") + this.FltVal("L145") + this.FltVal("L149") + this.FltVal("L153") + this.FltVal("L158") + this.FltVal("L164") + this.FltVal("L170") + this.FltVal("L175") + this.FltVal("L179") + this.FltVal("L183") + this.FltVal("L187") + this.FltVal("L191") + this.FltVal("L195") + this.FltVal("L199"), 2));
      this.SetCurrentNum("L203", Math.Round(this.FltVal("L129") + this.FltVal("L352") + this.FltVal("L133") + this.FltVal("L136") + this.FltVal("L139") + this.FltVal("L143") + this.FltVal("L147") + this.FltVal("L151") + this.FltVal("L155") + this.FltVal("L161") + this.FltVal("L167") + this.FltVal("L173") + this.FltVal("L177") + this.FltVal("L181") + this.FltVal("L185") + this.FltVal("L189") + this.FltVal("L193") + this.FltVal("L197") + this.FltVal("L201"), 2));
      double num1 = Math.Round(this.FltVal("L126") - this.FltVal("L202"), 2);
      this.SetCurrentNum("L206R", num1);
      this.SetCurrentNum("L206", Math.Abs(num1));
      double num2 = Math.Round(this.FltVal("L127") - this.FltVal("L203"), 2);
      this.SetCurrentNum("L207R", num2);
      this.SetCurrentNum("L207", Math.Abs(num2));
    }

    private void calculateOthers(string id, string val)
    {
      this._closerCalculationServant.CalculateCloserOthers(id, val);
    }

    private void calculateModifiedTerms(string id, string val)
    {
      double num1 = this.FltVal("2991");
      double num2 = RegzCalculation.CalcRawMonthlyPayment(this.IntVal("2993"), num1, this.FltVal("2992"), this.IntVal("2993"), 0, num1, this.Val("4746"), this.checkIfSimpleInterest(), this.findFirstPaymentDate(), this.findConstInterestType(), this.findSimpleInterestUse366ForLeapYear());
      this.SetCurrentNum("2994", num2, this.UseNoPayment(num2));
      double num3 = this.FltVal("358");
      if (num3 > 0.0)
        this.SetCurrentNum("2995", num1 / num3 * 100.0);
      else
        this.SetVal("2995", "");
    }
  }
}
