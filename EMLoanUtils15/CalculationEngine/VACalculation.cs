// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.CalculationEngine.VACalculation
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using System;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.CalculationEngine
{
  internal class VACalculation : CalculationBase
  {
    private const string className = "VACalculation�";
    private static readonly string sw = Tracing.SwDataEngine;
    internal Routine CalcVALA;
    internal Routine CalcVARRRWS;
    internal Routine CalcVARecoupment;
    internal Routine CalcMaximumSellerContribution;
    internal Routine CalcVACashOutRefinance;
    internal Routine CalcVAStatutoryRecoupment;
    internal Routine CalcIRRRLClosingCosts;

    internal VACalculation(LoanData l, EllieMae.EMLite.CalculationEngine.CalculationObjects calObjs)
      : base(l, calObjs)
    {
      this.CalcVALA = this.RoutineX(new Routine(this.calculationVALA));
      this.CalcVARRRWS = this.RoutineX(new Routine(this.calculateVARRRWS));
      this.CalcVARecoupment = this.RoutineX(new Routine(this.calculateVARecoupment));
      this.CalcMaximumSellerContribution = this.RoutineX(new Routine(this.calculateMaximumSellerContribution));
      this.CalcVACashOutRefinance = this.RoutineX(new Routine(this.calculateVACashOutRefinance));
      this.CalcVAStatutoryRecoupment = this.RoutineX(new Routine(this.calculateVAStatutoryRecoupment));
      this.CalcIRRRLClosingCosts = this.RoutineX(new Routine(this.calculateIRRRLClosingCosts));
      this.addFieldHandlers();
    }

    private void addFieldHandlers()
    {
      Routine routine1 = this.RoutineX(new Routine(this.calculationVA261820));
      this.AddFieldHandler("VASUMM.X55", routine1);
      this.AddFieldHandler("VASUMM.X57", routine1);
      this.AddFieldHandler("VASUMM.X65", routine1);
      this.AddFieldHandler("1497", routine1);
      Routine routine2 = this.RoutineX(new Routine(this.calculationVALA));
      this.AddFieldHandler("1346", routine2);
      this.AddFieldHandler("1147", routine2);
      this.AddFieldHandler("1148", routine2);
      this.AddFieldHandler("1348", routine2);
      this.AddFieldHandler("1089", routine2);
      this.AddFieldHandler("1088", routine2);
      this.AddFieldHandler("1306", routine2);
      this.AddFieldHandler("1156", routine2);
      this.AddFieldHandler("1307", routine2);
      this.AddFieldHandler("1158", routine2);
      this.AddFieldHandler("1308", routine2);
      this.AddFieldHandler("1159", routine2);
      this.AddFieldHandler("1309", routine2);
      this.AddFieldHandler("VALA.X19", routine2);
      this.AddFieldHandler("1316", routine2);
      this.AddFieldHandler("1317", routine2);
      this.AddFieldHandler("1318", routine2);
      this.AddFieldHandler("198", routine2);
      this.AddFieldHandler("1341", routine2);
      this.AddFieldHandler("1008", routine2);
      Routine routine3 = this.RoutineX(new Routine(this.calculateVARRRWS));
      this.AddFieldHandler("VARRRWS.X1", routine3);
      this.AddFieldHandler("VARRRWS.X6", routine3);
      this.AddFieldHandler("NEWHUD.X1154", routine3);
      this.AddFieldHandler("NEWHUD.X1158", routine3);
      this.AddFieldHandler("NEWHUD.X1162", routine3);
      this.AddFieldHandler("NEWHUD2.X956", routine3);
      Routine routine4 = this.RoutineX(new Routine(this.calculateHUD1003Addendum));
      this.AddFieldHandler("3178", routine4);
      this.AddFieldHandler("3179", routine4);
      this.AddFieldHandler("3180", routine4);
      this.AddFieldHandler("3181", routine4);
      this.AddFieldHandler("3175", routine4);
      Routine routine5 = this.RoutineX(new Routine(this.calculateVACashOutRefinance));
      this.AddFieldHandler("NTB.X4", routine5);
      this.AddFieldHandler("NTB.X17", routine5);
      this.AddFieldHandler("VASUMM.X102", routine5);
      this.AddFieldHandler("VASUMM.X106", routine5);
      this.AddFieldHandler("VASUMM.X107", routine5);
      this.AddFieldHandler("VASUMM.X110", routine5);
      Routine routine6 = this.RoutineX(new Routine(this.calculateVAStatutoryRecoupment));
      this.AddFieldHandler("961", routine6);
      this.AddFieldHandler("VASUMM.X132", routine6);
      this.AddFieldHandler("VASUMM.X133", routine6);
      this.AddFieldHandler("VASUMM.X134", routine6);
      this.AddFieldHandler("VASUMM.X127", routine6);
      this.AddFieldHandler("VASUMM.X128", routine6);
      this.AddFieldHandler("VASUMM.X129", routine6);
      Routine routine7 = this.RoutineX(new Routine(this.calculateVARecoupment));
      this.AddFieldHandler("NTB.X7", routine7);
      this.AddFieldHandler("VASUMM.X125", routine7);
      this.AddFieldHandler("VASUMM.X25", routine7);
      this.AddFieldHandler("VASUMM.X50", routine7);
      this.AddFieldHandler("VASUMM.X124", routine7);
      Routine routine8 = this.RoutineX(new Routine(this.calculateIRRRLClosingCosts)) + this.RoutineX(new Routine(this.calculateRecoupmentMonths));
      this.AddFieldHandler("VASUMM.X140", routine8);
      this.AddFieldHandler("VASUMM.X141", routine8);
      this.AddFieldHandler("VASUMM.X142", routine8);
      this.AddFieldHandler("VASUMM.X143", routine8);
      this.AddFieldHandler("VASUMM.X144", routine8);
      this.AddFieldHandler("VASUMM.X145", routine8);
      this.AddFieldHandler("VASUMM.X146", routine8);
      this.AddFieldHandler("VASUMM.X147", routine8);
      this.AddFieldHandler("VASUMM.X148", routine8);
      Routine routine9 = this.RoutineX(new Routine(this.calculateRecoupmentMonths));
      this.AddFieldHandler("VASUMM.X25", routine9);
      this.AddFieldHandler("VASUMM.X138", routine9);
      this.AddFieldHandler("VASUMM.X22", routine9);
    }

    private void calculationVALA(string id, string val)
    {
      if (Tracing.IsSwitchActive(VACalculation.sw, TraceLevel.Info))
        Tracing.Log(VACalculation.sw, TraceLevel.Info, nameof (VACalculation), "routine: calculationVALA ID: " + id);
      this.SetCurrentNum("1088", this.FltVal("101") + this.FltVal("102") + this.FltVal("103") + this.FltVal("104") + this.FltVal("105"));
      this.SetCurrentNum("1089", this.FltVal("110") + this.FltVal("111") + this.FltVal("112") + this.FltVal("113") + this.FltVal("114"));
      this.SetCurrentNum("1349", this.FltVal("5") + this.FltVal("1405") + this.FltVal("230") + this.FltVal("1346") + this.FltVal("1147") + this.FltVal("1148") + this.FltVal("1348"));
      this.SetCurrentNum("1810", this.FltVal("1089") + this.FltVal("1088"));
      double num1 = this.FltVal("1306") + this.FltVal("1307") + this.FltVal("1308") + this.FltVal("1309");
      this.SetCurrentNum("1310", num1);
      double num2 = num1;
      this.SetCurrentNum("1313", this.FltVal("1089") - num1);
      double num3 = this.FltVal("1156") + this.FltVal("1158") + this.FltVal("1159") + this.FltVal("VALA.X19");
      this.SetCurrentNum("1311", num3);
      this.SetCurrentNum("1312", num2 + num3);
      this.SetCurrentNum("1314", this.FltVal("1088") - num3);
      this.SetCurrentNum("1316", this.FltVal("115") + this.FltVal("116") + this.FltVal("117"));
      this.SetCurrentNum("1319", this.FltVal("1316") + this.FltVal("1313"));
      this.SetCurrentNum("1008", this.FltVal("911"));
      this.SetCurrentNum("1317", this.FltVal("106") + this.FltVal("107") + this.FltVal("108"));
      this.SetCurrentNum("1320", this.FltVal("1317") + this.FltVal("1314"));
      this.SetCurrentNum("1315", this.FltVal("1313") + this.FltVal("1314"));
      this.SetCurrentNum("1318", this.FltVal("1316") + this.FltVal("1317"));
      double num4 = this.FltVal("1315") + this.FltVal("1318");
      this.SetCurrentNum("1321", num4);
      double num5 = num4 - this.FltVal("198");
      this.SetCurrentNum("1323", num5);
      this.SetCurrentNum("1325", num5 - this.FltVal("1349"));
      double num6 = this.FltVal("1810") + this.FltVal("1318");
      this.SetCurrentNum("993", num6);
      this.SetCurrentNum("1341", num6 == 0.0 ? 0.0 : Math.Round((this.FltVal("5") + this.FltVal("1405") + this.FltVal("230") + this.FltVal("1346") + this.FltVal("1348") + this.FltVal("198")) / num6 * 100.0, 3));
      if (!(id == "26"))
        return;
      this.calculateVARRRWS(id, val);
    }

    private void calculateMaximumSellerContribution(string id, string val)
    {
      this.SetCurrentNum("4180", (double) this.IntVal("356") * 0.04);
    }

    private void calculateHUD1003Addendum(string id, string val)
    {
      string field = this.loan.GetField("3175");
      if (field != "Approved" && field != "Modified And Approved")
      {
        this.SetVal("3176", "");
        this.SetVal("3177", "");
      }
      if (field != "Modified And Approved")
      {
        this.RemoveCurrentLock("3182");
        this.SetVal("3178", "");
        this.SetVal("3179", "");
        this.SetVal("3180", "");
        this.SetVal("3181", "");
        this.SetVal("3182", "");
        this.SetVal("3183", "");
        this.SetVal("3184", "");
        this.SetVal("3196", "");
      }
      else
      {
        double num = this.calObjs.RegzCal.CalcRawMonthlyPayment(this.IntVal("3180") * 12 + this.IntVal("3181"), this.FltVal("3178"), this.FltVal("3179"), false);
        this.SetCurrentNum("3182", Utils.ArithmeticRounding(num, 2), this.UseNoPayment(num));
      }
    }

    private void calculateVARRRWS(string id, string val)
    {
      if (Tracing.IsSwitchActive(VACalculation.sw, TraceLevel.Info))
        Tracing.Log(VACalculation.sw, TraceLevel.Info, nameof (VACalculation), "routine: calculateVARRRWS ID: " + id);
      double num1 = this.FltVal("26") - this.FltVal("VARRRWS.X1");
      this.SetCurrentNum("VARRRWS.X2", num1);
      double num2 = num1;
      this.SetCurrentNum("VARRRWS.X9", Utils.ParseDouble((object) UCDXmlExporterBase.CalculateOriginationCharges(Utils.ParseDecimal((object) this.FltVal("2")), Utils.ParseDecimal((object) this.FltVal("NEWHUD2.X927")), Utils.ParseDecimal((object) this.FltVal("NEWHUD2.X956")))));
      double num3 = this.FltVal("VARRRWS.X9") / 100.0 * num1;
      this.SetCurrentNum("VARRRWS.X3", num3);
      double num4 = num2 + num3;
      double num5 = this.FltVal("388") / 100.0 * num1;
      this.SetCurrentNum("VARRRWS.X4", num5);
      double num6 = num4 + num5;
      double num7 = this.FltVal("1107") / 100.0 * num1;
      this.SetCurrentNum("VARRRWS.X5", num7);
      double num8 = num6 + (num7 + this.FltVal("VARRRWS.X6"));
      this.SetCurrentNum("VARRRWS.X7", num8);
      this.SetCurrentNum("VARRRWS.X8", this.FltVal("VARRRWS.X9") / 100.0 * num8);
      double num9 = (this.FltVal("VARRRWS.X7") + this.FltVal("VARRRWS.X8") - this.FltVal("VARRRWS.X3") - this.FltVal("VARRRWS.X5")) * (this.FltVal("1107") / 100.0);
      this.SetCurrentNum("VARRRWS.X11", num9);
      this.SetCurrentNum("VARRRWS.X13", num9 + (this.FltVal("VARRRWS.X7") + this.FltVal("VARRRWS.X8") - this.FltVal("VARRRWS.X3")) - this.FltVal("VARRRWS.X5"));
    }

    private void calculationVA261820(string id, string val)
    {
      if (this.Val("VASUMM.X55") != "Y")
        this.SetVal("VASUMM.X56", "");
      if (this.Val("1497") != "Other")
        this.SetVal("1064", "");
      if (this.Val("VASUMM.X57") != "Y")
        this.SetVal("VASUMM.X58", "");
      if (!(this.Val("VASUMM.X65") != "Escrow") || !(this.Val("VASUMM.X65") != "Earmarked Account"))
        return;
      this.SetVal("VASUMM.X66", "");
    }

    private void calculateVARecoupment(string id, string val)
    {
      this.SetCurrentNum("VASUMM.X98", this.FltVal("737") - this.FltVal("121") - this.FltVal("125") - this.FltVal("126"));
      double num1 = this.FltVal("228") + this.FltVal("230") + this.FltVal("1405") + this.FltVal("232") + this.FltVal("235");
      string useGFETax = this.Val("USEGFETAX");
      if (useGFETax != "Y")
        num1 += this.FltVal("L268");
      double num2 = num1 + this.includeTaxInsurance("1628", "1630", useGFETax) + this.includeTaxInsurance("660", "253", useGFETax) + this.includeTaxInsurance("661", "254", useGFETax);
      this.SetCurrentNum("VASUMM.X99", num2, this.UseNoPayment(num2));
      string str1 = this.Val("VASUMM.X125");
      if (str1 == "Type I Cash-Out Refinance")
        this.SetCurrentNum("VASUMM.X19", RegzCalculation.CalculateMonthlyPayment(this.IntVal("VASUMM.X18"), 0, this.FltVal("VASUMM.X15"), this.FltVal("NTB.X7")));
      else
        this.SetCurrentNum("VASUMM.X19", RegzCalculation.CalculateMonthlyPayment(this.IntVal("VASUMM.X18"), 0, this.FltVal("VASUMM.X15"), this.FltVal("VASUMM.X16")));
      DateTime date1 = Utils.ParseDate((object) "04/01/2018").Date;
      this.SetCurrentNum("VASUMM.X22", str1 == "Type I Cash-Out Refinance" || this.Val("958") == "IRRRL" && (Utils.ParseDate((object) this.Val("763")).Date >= date1 || Utils.ParseDate((object) this.Val("748")).Date >= date1) ? Utils.ArithmeticRounding(this.FltVal("VASUMM.X19") - this.FltVal("5"), 2) : (!(str1 == "Type II Cash-Out Refinance") ? Utils.ArithmeticRounding(this.FltVal("VASUMM.X98") - this.FltVal("VASUMM.X99"), 2) : 0.0));
      this.SetCurrentNum("VASUMM.X137", this.FltVal("CD2.XSTB") - this.FltVal("NEWHUD2.X2290"));
      string str2 = this.Val("958");
      if (str2 == "IRRRL")
      {
        this.calculateIRRRLClosingCosts(id, val);
        this.SetVal("VASUMM.X100", "");
        this.SetVal("VASUMM.X26", "");
      }
      else if (this.Val("VASUMM.X26") == "Y")
        this.SetCurrentNum("VASUMM.X25", this.FltVal("137") + this.FltVal("969") + this.FltVal("1093"));
      else
        this.SetCurrentNum("VASUMM.X25", this.FltVal("BORPCC"));
      if (str2 != "IRRRL")
        this.SetVal("VASUMM.X138", "");
      this.calculateRecoupmentMonths(id, val);
      DateTime dateTime = Utils.ParseDate((object) this.Val("NTB.X1"));
      DateTime date2 = Utils.ParseDate((object) this.Val("748"));
      if (dateTime == DateTime.MinValue || date2 == DateTime.MinValue || DateTime.Compare(dateTime.Date, date2.Date) > 0)
      {
        this.SetVal("VASUMM.X28", "");
        this.SetVal("VASUMM.X29", "");
      }
      else
      {
        int num3 = 0;
        while (DateTime.Compare(dateTime.Date, date2.Date) <= 0)
        {
          dateTime = dateTime.AddMonths(1);
          ++num3;
        }
        if (num3 > 0)
          --num3;
        int num4 = (num3 - num3 % 12) / 12;
        this.SetVal("VASUMM.X28", num4 > 0 ? num4.ToString() : "");
        int num5 = num3 - num4 * 12;
        this.SetVal("VASUMM.X29", num5 > 0 ? num5.ToString() : "");
      }
      this.calObjs.VACal.calculateVACashOutRefinance(id, val);
    }

    private void calculateRecoupmentMonths(string id, string val)
    {
      DateTime date1 = Utils.ParseDate((object) this.Val("745"));
      DateTime date2 = Utils.ParseDate((object) "02/15/2019");
      string str1 = this.Val("VASUMM.X50");
      double num = this.FltVal("VASUMM.X22");
      string str2 = this.Val("VASUMM.X125");
      if (num > 0.0 && DateTime.Compare(date2.Date, date1.Date) <= 0 && (str1 == "VA-Fixed" || str1 == "VA-ARM/HARM") && str2 == "Type I Cash-Out Refinance")
      {
        if (this.FltVal("VASUMM.X124") > 0.0)
          this.SetCurrentNum("VASUMM.X27", (double) Utils.ParseInt((object) Math.Ceiling(Utils.ParseDecimal((object) this.FltVal("VASUMM.X124")) / Utils.ParseDecimal((object) num)), 0));
        else
          this.SetVal("VASUMM.X27", "");
      }
      else if (num > 0.0 && this.FltVal("VASUMM.X25") > 0.0)
        this.SetCurrentNum("VASUMM.X27", (double) Utils.ParseInt((object) Math.Ceiling(Utils.ParseDecimal((object) this.FltVal("VASUMM.X25")) / Utils.ParseDecimal((object) num)), 0));
      else
        this.SetVal("VASUMM.X27", "");
      if (this.Val("958") == "IRRRL" && num > 0.0)
        this.SetCurrentNum("VASUMM.X139", (double) Utils.ParseInt((object) Math.Ceiling(Utils.ParseDecimal((object) this.FltVal("VASUMM.X138")) / Utils.ParseDecimal((object) num)), 0));
      else
        this.SetVal("VASUMM.X139", "");
    }

    private void calculateIRRRLClosingCosts(string id, string val)
    {
      if (this.Val("958") != "IRRRL")
        return;
      string str1 = this.Val("VASUMM.X140");
      string str2 = this.Val("VASUMM.X141");
      string str3 = this.Val("VASUMM.X142");
      string str4 = this.Val("VASUMM.X143");
      string str5 = this.Val("VASUMM.X144");
      string str6 = this.Val("VASUMM.X145");
      string str7 = this.Val("VASUMM.X146");
      string str8 = this.Val("VASUMM.X147");
      string str9 = this.Val("VASUMM.X148");
      double num1 = 0.0;
      double num2 = 0.0;
      IDisclosureTracking2015Log idisclosureTracking2015Log1 = this.loan.GetLogList().GetLatestIDisclosureTracking2015Log(DisclosureTracking2015Log.DisclosureTrackingType.CD);
      IDisclosureTracking2015Log idisclosureTracking2015Log2 = this.loan.GetLogList().GetLatestIDisclosureTracking2015Log(DisclosureTracking2015Log.DisclosureTrackingType.LE);
      if (str1 == "Y")
      {
        num1 += this.FltVal("CD2.XSTA");
        double num3 = idisclosureTracking2015Log1 == null || idisclosureTracking2015Log2 == null ? this.FltVal("LE2.XSTA") : Utils.ParseDouble((object) idisclosureTracking2015Log2.GetDisclosedField("LE2.XSTA"));
        num2 += num3;
      }
      double num4 = this.FltVal("1050");
      double num5 = num4;
      if (idisclosureTracking2015Log1 != null && idisclosureTracking2015Log2 != null)
        num5 = Utils.ParseDouble((object) idisclosureTracking2015Log2.GetDisclosedField("1050"));
      if (str2 == "Y")
      {
        num1 += num4;
        num2 += num5;
      }
      if (str3 == "Y")
      {
        num1 += this.FltVal("VASUMM.X137");
        double num6 = idisclosureTracking2015Log1 == null || idisclosureTracking2015Log2 == null ? this.FltVal("LE2.XSTB") : Utils.ParseDouble((object) idisclosureTracking2015Log2.GetDisclosedField("LE2.XSTB"));
        num2 += num6 - num5;
      }
      if (str4 == "Y")
      {
        num1 += this.FltVal("CD2.XSTC");
        double num7 = idisclosureTracking2015Log1 == null || idisclosureTracking2015Log2 == null ? this.FltVal("LE2.XSTC") : Utils.ParseDouble((object) idisclosureTracking2015Log2.GetDisclosedField("LE2.XSTC"));
        num2 += num7;
      }
      if (str5 == "Y")
      {
        num1 += this.FltVal("CD2.XSTE");
        double num8 = idisclosureTracking2015Log1 == null || idisclosureTracking2015Log2 == null ? this.FltVal("LE2.XSTE") : Utils.ParseDouble((object) idisclosureTracking2015Log2.GetDisclosedField("LE2.XSTE"));
        num2 += num8;
      }
      if (str6 == "Y")
      {
        num1 += this.FltVal("CD2.XSTF");
        double num9 = idisclosureTracking2015Log1 == null || idisclosureTracking2015Log2 == null ? this.FltVal("LE2.XSTF") : Utils.ParseDouble((object) idisclosureTracking2015Log2.GetDisclosedField("LE2.XSTF"));
        num2 += num9;
      }
      if (str7 == "Y")
      {
        num1 += this.FltVal("CD2.XSTG");
        double num10 = idisclosureTracking2015Log1 == null || idisclosureTracking2015Log2 == null ? this.FltVal("LE2.XSTG") : Utils.ParseDouble((object) idisclosureTracking2015Log2.GetDisclosedField("LE2.XSTG"));
        num2 += num10;
      }
      if (str8 == "Y")
      {
        num1 += this.FltVal("CD2.XSTH");
        double num11 = idisclosureTracking2015Log1 == null || idisclosureTracking2015Log2 == null ? this.FltVal("LE2.XSTH") : Utils.ParseDouble((object) idisclosureTracking2015Log2.GetDisclosedField("LE2.XSTH"));
        num2 += num11;
      }
      if (str9 == "Y")
      {
        num1 -= this.FltVal("CD2.XSTLC");
        double num12 = idisclosureTracking2015Log1 == null || idisclosureTracking2015Log2 == null ? this.FltVal("LE2.XLC") : Utils.ParseDouble((object) idisclosureTracking2015Log2.GetDisclosedField("LE2.XLC"));
        num2 -= num12;
      }
      this.SetCurrentNum("VASUMM.X25", num1);
      this.SetCurrentNum("VASUMM.X138", num2);
    }

    private double includeTaxInsurance(string feeDescriptionID, string amountID, string useGFETax)
    {
      string lower = this.Val(feeDescriptionID).ToLower();
      return useGFETax != "Y" && SharedCalculations.IsTaxFee(lower) || SharedCalculations.IsInsuranceFee(lower) ? this.FltVal(amountID) : 0.0;
    }

    private void calculateVACashOutRefinance(string id, string val)
    {
      double num1 = Utils.ArithmeticRounding(this.FltVal("VASUMM.X102") - this.FltVal("2"), 2);
      this.SetVal("VASUMM.X103", this.Val("VASUMM.X102") == "" || this.Val("2") == "" ? "" : string.Concat((object) num1));
      this.SetVal("VASUMM.X116", this.Val("VASUMM.X102") == "" || this.Val("2") == "" ? "" : (num1 < 0.0 ? "Increase" : (num1 > 0.0 ? "Decrease" : "None")));
      int num2 = this.IntVal("NTB.X4") - this.IntVal("4");
      this.SetVal("VASUMM.X104", this.Val("NTB.X4") == "" || this.Val("4") == "" ? "" : string.Concat((object) num2));
      this.SetVal("VASUMM.X117", !(this.Val("NTB.X4") != "") || !(this.Val("4") != "") ? "" : (num2 < 0 ? "Increase" : (num2 > 0 ? "Decrease" : "None")));
      double num3 = Utils.ArithmeticRounding(this.FltVal("NTB.X7") - this.FltVal("3"), 3);
      this.SetVal("VASUMM.X105", this.Val("NTB.X7") == "" || this.Val("3") == "" ? "" : string.Concat((object) num3));
      this.SetVal("VASUMM.X118", !(this.Val("NTB.X7") != "") || !(this.Val("3") != "") ? "" : (num3 < 0.0 ? "Increase" : (num3 > 0.0 ? "Decrease" : "None")));
      double num4 = Utils.ArithmeticRounding(this.FltVal("VASUMM.X106") - this.FltVal("VASUMM.X107"), 2);
      this.SetVal("VASUMM.X108", this.Val("VASUMM.X106") == "" || this.Val("VASUMM.X107") == "" ? "" : string.Concat((object) num4));
      this.SetVal("VASUMM.X119", !(this.Val("VASUMM.X106") != "") || !(this.Val("VASUMM.X107") != "") ? "" : (num4 < 0.0 ? "Increase" : (num4 > 0.0 ? "Decrease" : "None")));
      double num5 = Utils.ArithmeticRounding(this.FltVal("NTB.X17") - this.FltVal("353"), 3);
      this.SetVal("VASUMM.X109", this.Val("NTB.X17") == "" || this.Val("353") == "" ? "" : string.Concat((object) num5));
      this.SetVal("VASUMM.X120", !(this.Val("NTB.X17") != "") || !(this.Val("353") != "") ? "" : (num5 < 0.0 ? "Increase" : (num5 > 0.0 ? "Decrease" : "None")));
      double num6 = Utils.ArithmeticRounding((double) this.IntVal("VASUMM.X110") - this.FltVal("VASUMM.X149"), 2);
      this.SetVal("VASUMM.X111", this.Val("VASUMM.X110") == "" ? "" : string.Concat((object) num6));
      double num7 = 0.0;
      if (this.Val("356") == "" && this.Val("1821") != "")
        num7 = Utils.ArithmeticRounding(this.FltVal("1821") - this.FltVal("VASUMM.X126"), 2);
      else if (this.Val("356") != "")
        num7 = Utils.ArithmeticRounding(this.FltVal("356") - this.FltVal("VASUMM.X126"), 2);
      this.SetVal("VASUMM.X112", !(this.Val("356") == "") || !(this.Val("VASUMM.X126") == "") || !(this.Val("1821") == "") ? string.Concat((object) num7) : "");
      double num8 = Utils.ArithmeticRounding(this.FltVal("VASUMM.X111") - this.FltVal("VASUMM.X112"), 2);
      this.SetVal("VASUMM.X113", this.Val("VASUMM.X111") == "" || this.Val("VASUMM.X112") == "" ? "" : string.Concat((object) num8));
      this.SetVal("VASUMM.X121", !(this.Val("VASUMM.X111") != "") || !(this.Val("VASUMM.X112") != "") ? "" : (num8 < 0.0 ? "Increase" : (num8 > 0.0 ? "Decrease" : "None")));
      double num9 = this.FltVal("142");
      this.SetVal("VASUMM.X114", this.Val("142") == "" ? "" : (num9 < 0.0 ? string.Concat((object) (num9 * -1.0)) : "0.00"));
      double num10 = this.FltVal("CD3.X80") - this.FltVal("26");
      this.SetVal("VASUMM.X115", !(this.Val("CD3.X80") == "") || !(this.Val("26") == "") ? string.Concat((object) num10) : "");
      double num11 = this.FltVal("VASUMM.X25") - this.FltVal("969");
      this.SetVal("VASUMM.X124", !(this.Val("VASUMM.X25") == "") || !(this.Val("969") == "") ? string.Concat((object) num11) : "");
    }

    private void calculateVAStatutoryRecoupment(string id, string val)
    {
      if (SharedCalculations.UseNewVAIRRRL(this.Val("1172"), this.Val("958"), this.Val("1887"), this.Val("2553"), this.Val("748"), this.Val("763")))
      {
        this.SetCurrentNum("VASUMM.X127", this.FltVal("CD2.XSTD") + this.FltVal("CD2.XSTE") + this.FltVal("CD2.XSTH") - this.FltVal("NEWHUD2.X2290") - this.FltVal("CD2.XSTLC"));
        if (this.Val("VASUMM.X133") != "Y" && this.IsLocked("VASUMM.X132"))
          this.RemoveCurrentLock("VASUMM.X132");
        if (this.IsLocked("VASUMM.X132"))
        {
          if (this.FltVal("VASUMM.X132") > this.FltVal("961"))
            this.SetCurrentNum("VASUMM.X132", this.FltVal("961"));
        }
        else if (this.Val("VASUMM.X133") == "Y")
          this.SetCurrentNum("VASUMM.X132", this.FltVal("961"));
        else
          this.SetVal("VASUMM.X132", "");
        double loanAmount = this.FltVal("2") - this.FltVal("VASUMM.X132");
        this.SetCurrentNum("VASUMM.X128", RegzCalculation.CalculateMonthlyPayment(this.IntVal("4"), 0, loanAmount, this.FltVal("3")));
        this.SetCurrentNum("VASUMM.X129", Utils.ArithmeticRounding(this.FltVal("VASUMM.X19") - this.FltVal("VASUMM.X128"), 2));
        if (this.FltVal("VASUMM.X127") <= 0.0 || this.FltVal("VASUMM.X129") <= 0.0)
          this.SetVal("VASUMM.X130", "");
        else
          this.SetCurrentNum("VASUMM.X130", (double) Utils.ParseInt((object) Math.Ceiling(Utils.ParseDecimal((object) this.FltVal("VASUMM.X127")) / Utils.ParseDecimal((object) this.FltVal("VASUMM.X129"))), 0));
      }
      else
      {
        if (this.IsLocked("VASUMM.X132"))
          this.RemoveCurrentLock("VASUMM.X132");
        for (int maxValue = (int) sbyte.MaxValue; maxValue <= 134; ++maxValue)
          this.SetVal("VASUMM.X" + (object) maxValue, "");
      }
    }
  }
}
