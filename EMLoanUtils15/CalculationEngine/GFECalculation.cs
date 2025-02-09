// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.CalculationEngine.GFECalculation
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.LoanUtils.CalculationServants;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.CalculationEngine
{
  internal class GFECalculation : CalculationBase
  {
    private const string className = "GFECalculation�";
    private static readonly string sw = Tracing.SwDataEngine;
    internal Routine CalcGFEFees;
    internal Routine CalcPrepaid;
    internal Routine CalcTotalCosts;
    internal Routine CalcClosingCosts;
    internal Routine CalcOthers;
    internal Routine SyncMLDSField;
    internal Routine RecalcForm;
    internal Routine CalcHighPrice;
    internal Routine CalcSection32;
    internal Routine CalcBrokerLenderFeeTotals;
    internal Routine CalcTotalPrepaidFees;
    internal Routine CalcDISCLOSUREX694;
    internal Routine CalcSecondAppraisalRequired;
    private SessionObjects sessionObjects;
    private GfeCalculationServant _gfeCalculationServant;
    private bool excludeOriginationCredit;

    internal GFECalculation(SessionObjects sessionObjects, LoanData l, EllieMae.EMLite.CalculationEngine.CalculationObjects calObjs)
      : base(l, calObjs)
    {
      this.sessionObjects = sessionObjects;
      this.CalcGFEFees = this.RoutineX(new Routine(this.calculateGFEFees));
      this.CalcPrepaid = this.RoutineX(new Routine(this.calculatePrepaid));
      this.CalcClosingCosts = this.RoutineX(new Routine(this.calculateClosingCost));
      this.CalcTotalCosts = this.RoutineX(new Routine(this.calculateTotalCosts));
      this.CalcOthers = this.RoutineX(new Routine(this.calculateOthers));
      this.SyncMLDSField = this.RoutineX(new Routine(this.syncMLDS));
      this.CalcHighPrice = this.RoutineX(new Routine(this.calculateHighPrice));
      this.CalcSection32 = this.RoutineX(new Routine(this.calculateSection32));
      this.RecalcForm = this.RoutineX(new Routine(this.recalculateForm));
      this.CalcBrokerLenderFeeTotals = this.RoutineX(new Routine(this.calcBrokerLenderFeeTotals));
      this.CalcTotalPrepaidFees = this.RoutineX(new Routine(this.calculateTotalPrepaidFees));
      this.CalcDISCLOSUREX694 = this.RoutineX(new Routine(this.calculateDISCLOSUREX694));
      this.CalcSecondAppraisalRequired = this.RoutineX(new Routine(this.calculateSecondAppraisalRequired));
      this.addFieldHandlers();
      this._gfeCalculationServant = new GfeCalculationServant((ILoanModelProvider) this, (ISettingsProvider) new SystemSettings(sessionObjects));
    }

    private void addFieldHandlers()
    {
      Routine routine1 = this.RoutineX(new Routine(this.calculateGFEFees));
      this.AddFieldHandler("560", routine1);
      this.AddFieldHandler("572", routine1);
      this.AddFieldHandler("1050", routine1);
      this.AddFieldHandler("DISCLOSURE.X61", routine1);
      this.AddFieldHandler("DISCLOSURE.X62", routine1);
      this.AddFieldHandler("DISCLOSURE.X63", routine1);
      this.AddFieldHandler("DISCLOSURE.X64", routine1);
      this.AddFieldHandler("DISCLOSURE.X65", routine1);
      this.AddFieldHandler("DISCLOSURE.X66", routine1);
      this.AddFieldHandler("FLMTGCM.X14", routine1);
      this.AddFieldHandler("FLMTGCM.X15", routine1);
      this.AddFieldHandler("2005", routine1);
      this.AddFieldHandler("2833", routine1);
      this.AddFieldHandler("NEWHUD.X223", routine1);
      this.AddFieldHandler("NEWHUD.X224", routine1);
      this.AddFieldHandler("1191", routine1);
      Routine routine2 = this.RoutineX(new Routine(this.calculatePrepaid));
      this.AddFieldHandler("561", routine2);
      this.AddFieldHandler("L251", routine2);
      this.AddFieldHandler("578", routine2);
      this.AddFieldHandler("596", routine2);
      this.AddFieldHandler("563", routine2);
      this.AddFieldHandler("L270", routine2);
      this.AddFieldHandler("595", routine2);
      this.AddFieldHandler("597", routine2);
      this.AddFieldHandler("1632", routine2);
      this.AddFieldHandler("598", routine2);
      this.AddFieldHandler("599", routine2);
      this.AddFieldHandler("334", routine2);
      this.AddFieldHandler("642", routine2);
      this.AddFieldHandler("656", routine2);
      this.AddFieldHandler("338", routine2);
      this.AddFieldHandler("L269", routine2);
      this.AddFieldHandler("655", routine2);
      this.AddFieldHandler("657", routine2);
      this.AddFieldHandler("658", routine2);
      this.AddFieldHandler("659", routine2);
      this.AddFieldHandler("NEWHUD.X1708", routine2);
      this.AddFieldHandler("558", routine2);
      this.AddFieldHandler("562", routine2);
      this.AddFieldHandler("1668", routine2);
      this.AddFieldHandler("L261", routine2);
      this.AddFieldHandler("579", routine2);
      this.AddFieldHandler("SYS.X8", routine2);
      Routine routine3 = this.RoutineX(new Routine(this.calculateTotalCosts) + this.calObjs.D1003URLA2020Cal.CalcOtherCredits);
      this.AddFieldHandler("304", routine3);
      this.AddFieldHandler("BORPCC", routine3);
      this.AddFieldHandler("BKRPCC", routine3);
      this.AddFieldHandler("LENPCC", routine3);
      this.AddFieldHandler("OTHPCC", routine3);
      this.AddFieldHandler("TNBPCC", routine3);
      this.AddFieldHandler("1845", routine3);
      this.AddFieldHandler("URLA.X145", routine3);
      this.AddFieldHandler("URLA.X148", routine3);
      this.AddFieldHandler("URLA.X152", routine3 + this.calObjs.FHACal.CalcFredMac);
      Routine routine4 = this.RoutineX(new Routine(this.calculateOthers));
      this.AddFieldHandler("DISCLOSURE.X157", routine4);
      this.AddFieldHandler("DISCLOSURE.X158", routine4);
      this.AddFieldHandler("DISCLOSURE.X160", routine4);
      this.AddFieldHandler("DISCLOSURE.X162", routine4);
      this.AddFieldHandler("DISCLOSURE.X464", routine4);
      this.AddFieldHandler("DISCLOSURE.X466", routine4);
      this.AddFieldHandler("DISCLOSURE.X468", routine4);
      this.AddFieldHandler("DISCLOSURE.X470", routine4);
      this.AddFieldHandler("DISCLOSURE.X109", routine4);
      this.AddFieldHandler("DISCLOSURE.X76", routine4);
      this.AddFieldHandler("DISCLOSURE.X471", routine4);
      this.AddFieldHandler("DISCLOSURE.X475", routine4);
      this.AddFieldHandler("DISCLOSURE.X476", routine4);
      this.AddFieldHandler("DISCLOSURE.X477", routine4);
      this.AddFieldHandler("DISCLOSURE.X478", routine4);
      this.AddFieldHandler("DISCLOSURE.X480", routine4);
      this.AddFieldHandler("DISCLOSURE.X481", routine4);
      this.AddFieldHandler("DISCLOSURE.X482", routine4);
      this.AddFieldHandler("DISCLOSURE.X483", routine4);
      this.AddFieldHandler("DISCLOSURE.X526", routine4);
      this.AddFieldHandler("DISCLOSURE.X528", routine4);
      this.AddFieldHandler("DISCLOSURE.X564", routine4);
      this.AddFieldHandler("DISCLOSURE.X566", routine4);
      this.AddFieldHandler("DISCLOSURE.X567", routine4);
      this.AddFieldHandler("DISCLOSURE.X568", routine4);
      this.AddFieldHandler("DISCLOSURE.X570", routine4);
      this.AddFieldHandler("DISCLOSURE.X572", routine4);
      this.AddFieldHandler("DISCLOSURE.X575", routine4);
      this.AddFieldHandler("DISCLOSURE.X226", routine4);
      this.AddFieldHandler("DISCLOSURE.X227", routine4);
      this.AddFieldHandler("DISCLOSURE.X367", routine4);
      this.AddFieldHandler("DISCLOSURE.X368", routine4);
      this.AddFieldHandler("DISCLOSURE.X491", routine4);
      this.AddFieldHandler("DISCLOSURE.X222", routine4);
      this.AddFieldHandler("DISCLOSURE.X599", routine4);
      this.AddFieldHandler("DISCLOSURE.X208", routine4);
      this.AddFieldHandler("DISCLOSURE.X97", routine4);
      this.AddFieldHandler("DISCLOSURE.X114", routine4);
      this.AddFieldHandler("DISCLOSURE.X117", routine4);
      this.AddFieldHandler("DISCLOSURE.X619", routine4);
      this.AddFieldHandler("DISCLOSURE.X446", routine4);
      this.AddFieldHandler("DISCLOSURE.X448", routine4);
      this.AddFieldHandler("DISCLOSURE.X450", routine4);
      this.AddFieldHandler("DISCLOSURE.X452", routine4);
      this.AddFieldHandler("DISCLOSURE.X454", routine4);
      this.AddFieldHandler("DISCLOSURE.X456", routine4);
      this.AddFieldHandler("DISCLOSURE.X458", routine4);
      this.AddFieldHandler("DISCLOSURE.X645", routine4);
      this.AddFieldHandler("DISCLOSURE.X646", routine4);
      this.AddFieldHandler("DISCLOSURE.X647", routine4);
      this.AddFieldHandler("DISCLOSURE.X648", routine4);
      this.AddFieldHandler("DISCLOSURE.X651", routine4);
      this.AddFieldHandler("DISCLOSURE.X652", routine4);
      this.AddFieldHandler("DISCLOSURE.X892", routine4);
      this.AddFieldHandler("DISCLOSURE.X896", routine4);
      this.AddFieldHandler("DISCLOSURE.X338", routine4);
      this.AddFieldHandler("DISCLOSURE.X374", routine4);
      this.AddFieldHandler("DISCLOSURE.X924", routine4);
      this.AddFieldHandler("DISCLOSURE.X927", routine4);
      this.AddFieldHandler("DISCLOSURE.X930", routine4);
      this.AddFieldHandler("DISCLOSURE.X933", routine4);
      this.AddFieldHandler("DISCLOSURE.X936", routine4);
      this.AddFieldHandler("DISCLOSURE.X939", routine4);
      this.AddFieldHandler("1847", routine4);
      this.AddFieldHandler("1663", routine4);
      this.AddFieldHandler("1848", routine4);
      this.AddFieldHandler("1665", routine4);
      this.AddFieldHandler("NEWHUD.X734", routine4);
      for (int index = 1139; index <= 1230; ++index)
        this.AddFieldHandler("NEWHUD.X" + (object) index, routine4);
      this.AddFieldHandler("2005", this.RoutineX(new Routine(this.CalculateFunder)));
      this.AddFieldHandler("16", this.RoutineX(new Routine(this.calculateHighPrice)));
      Routine routine5 = this.RoutineX(new Routine(this.calculateDISCLOSUREX694));
      this.AddFieldHandler("DISCLOSURE.X963", routine5);
      this.AddFieldHandler("DISCLOSURE.X965", routine5);
      this.AddFieldHandler("DISCLOSURE.X966", routine5);
      this.AddFieldHandler("DISCLOSURE.X967", routine5);
      this.AddFieldHandler("DISCLOSURE.X968", routine5);
      this.AddFieldHandler("DISCLOSURE.X969", routine5);
      this.AddFieldHandler("DISCLOSURE.X970", routine5);
      this.AddFieldHandler("DISCLOSURE.X971", routine5);
      Routine routine6 = this.RoutineX(new Routine(this.calculateSecondAppraisalRequired));
      this.AddFieldHandler("3853", routine6);
      this.AddFieldHandler("3854", routine6);
      this.AddFieldHandler("3855", routine6);
      Routine routine7 = this.RoutineX(new Routine(this.calculateSection32));
      this.AddFieldHandler("S32DISC.X177", routine7);
      this.AddFieldHandler("NTB.X16", routine7);
      this.AddFieldHandler("NTB.X34", routine7);
      this.AddFieldHandler("QM.X2", routine7);
      this.AddFieldHandler("DISCLOSURE.X1076", new Routine(this.UpdateValueExistingJuniorLien));
      this.AddFieldHandler("DISCLOSURE.X1077", new Routine(this.UpdateValueExistingJuniorLien));
      this.AddFieldHandler("DISCLOSURE.X1078", new Routine(this.UpdateValueExistingJuniorLien));
      this.AddFieldHandler("DISCLOSURE.X1079", new Routine(this.UpdateValueExistingJuniorLien));
      this.AddFieldHandler("HELOC.ParticipationFees", routine7);
      this.AddFieldHandler("HELOC.TransactionFees", routine7);
      Routine routine8 = this.RoutineX(this.calObjs.D1003URLA2020Cal.CalcTotalOtherAssets);
      this.AddFieldHandler("URLAROA0002", routine8 + this.calObjs.D1003Cal.CalcTotalOtherAssets + this.calObjs.D1003URLA2020Cal.CalcOtherCredits + this.calObjs.D1003URLA2020Cal.CalcTotalCredits + this.CalcClosingCosts);
      this.AddFieldHandler("URLA.X149", routine8 + this.calObjs.D1003Cal.CalcTotalOtherAssets + this.calObjs.D1003URLA2020Cal.CalcOtherCredits + this.calObjs.D1003URLA2020Cal.CalcTotalCredits + this.CalcClosingCosts);
      this.AddFieldHandler("URLAROA0003", routine8 + this.calObjs.D1003URLA2020Cal.CalcOtherCredits + this.calObjs.D1003URLA2020Cal.CalcTotalCredits + this.CalcClosingCosts);
    }

    internal bool ExcludeOriginationCredit
    {
      set => this.excludeOriginationCredit = value;
    }

    private void recalculateForm(string id, string val)
    {
      if (this.UseNewGFEHUD || this.UseNew2015GFEHUD)
        return;
      this.FormCal((string) null, id, val);
    }

    internal void FormCal(string formid, string id, string val)
    {
      if (Tracing.IsSwitchActive(GFECalculation.sw, TraceLevel.Info))
        Tracing.Log(GFECalculation.sw, TraceLevel.Info, nameof (GFECalculation), "routine: FormCalForm ID: " + formid);
      if ((id == "81" || id == "82") && this.FltVal("3120") != 0.0)
      {
        double num = this.FltVal("3120");
        switch (id)
        {
          case "81":
            this.SetCurrentNum("82", num - this.FltVal("81"));
            break;
          case "82":
            this.SetCurrentNum("81", num - this.FltVal("82"));
            break;
        }
      }
      this.calculateGFEFees(id, val);
      if (formid == "IMPORT")
        this.calculatePrepaid(formid, (string) null);
      else
        this.calculatePrepaid((string) null, (string) null);
      if (id != null && id != "IMPORT")
      {
        this.syncMLDS(id, val);
        if (id == "230")
          this.syncMLDS("L251", this.Val("L251"));
      }
      if (id != "TPO")
        this.calObjs.RegzCal.CalcAPR((string) null, (string) null);
      this.calculateSection32((string) null, (string) null);
      this.calObjs.MLDSCal.CalcCompensations((string) null, (string) null);
    }

    private void calculateGFEFees(string id, string val)
    {
      if (Tracing.IsSwitchActive(GFECalculation.sw, TraceLevel.Info))
        Tracing.Log(GFECalculation.sw, TraceLevel.Info, nameof (GFECalculation), "routine: calculateGFEFees ID: " + id);
      if (this.FltVal("3") > 100.0)
      {
        this.SetCurrentNum("3", 100.0);
        this.SetVal("KBYO.XD3", "100");
      }
      if (this.Val("2852") == "Y")
      {
        if (id == "L244")
          this.SetVal("2553", this.Val("L244"));
        else
          this.SetVal("L244", this.Val("2553"));
      }
      if (id == "682" || id == "423" || id == "L425" || id == "L245")
      {
        string str = this.Val("682");
        if (Utils.IsDate((object) str))
        {
          DateTime dateTime = Utils.ParseDate((object) str);
          dateTime = !(this.Val("423") == "Biweekly") ? dateTime.AddMonths(-1) : dateTime.AddDays(-14.0);
          this.SetVal("L245", dateTime.ToString("MM/dd/yy"));
        }
        else
          this.SetVal("L245", "");
      }
      string str1 = this.Val("L244");
      string str2 = this.Val("L245");
      if (str1 != "" && str1 != "//" && str2 != "" && str2 != "//")
      {
        DateTime date1 = Utils.ParseDate((object) str1);
        DateTime date2 = Utils.ParseDate((object) str2);
        if (date1 != Utils.DbMinDate && date2 != Utils.DbMinDate)
        {
          int totalDays = (int) (date2 - date1).TotalDays;
          this.SetCurrentNum("332", (double) totalDays);
          double val1 = Utils.ArithmeticRounding(Math.Round(this.FltVal("333"), 4) * (double) totalDays, 2) - this.FltVal("561");
          if (this.IsLocked("334"))
            this.RemoveCurrentLock("334");
          this.SetCurrentNum("334", Utils.ArithmeticRounding(val1, 2));
          if (this.UseNewGFEHUD || this.UseNew2015GFEHUD)
            this.calObjs.NewHudCal.CopyHUD2010ToGFE2010("334", id, false);
        }
      }
      this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem901(id, val);
      if (id == "448" && (val == string.Empty || val == null))
        this.SetVal("GFE1", "");
      if (id == "449" && (val == string.Empty || val == null))
        this.SetVal("GFE2", "");
      if (id == "1878" && (val == string.Empty || val == null))
        this.SetVal("GFE3", "");
      double val2 = 0.0;
      double num1 = this.FltVal("2");
      string str3 = this.Val("1172");
      if (this.IsLocked("454"))
      {
        if (num1 != 0.0)
          val2 = !(str3 != "FHA") ? (this.FltVal("454") + this.FltVal("559")) / this.FltVal("1109") * 100.0 : (this.FltVal("454") + this.FltVal("559")) / num1 * 100.0;
        this.SetCurrentNum("388", Utils.ArithmeticRounding(val2, 3));
      }
      double num2 = this.FltVal("388") / 100.0;
      double num3;
      if (str3 != "FHA")
      {
        if (str3 == "VA" && this.Val("958") == "IRRRL" && this.Val("19").IndexOf("Refinance") > -1)
        {
          this.SetVal("1619", "");
          this.SetVal("559", "");
          num3 = Utils.ArithmeticRounding(this.FltVal("VARRRWS.X2") * num2, 2);
        }
        else
          num3 = Utils.ArithmeticRounding(num1 * num2, 2);
      }
      else
      {
        num3 = Utils.ArithmeticRounding(this.FltVal("1109") * num2, 2);
        if (str3 == "FarmersHomeAdministration")
          this.SetVal("1757", "Loan Amount");
        else
          this.SetVal("1757", "Base Loan Amount");
        this.SetVal("1775", "Y");
      }
      this.SetCurrentNum("454", num3 + (this.UseNewGFEHUD || this.UseNew2015GFEHUD ? 0.0 : this.FltVal("1619")) - this.FltVal("559"));
      this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem801a(id, val);
      if (this.Val("NEWHUD.X1139") == "Y")
      {
        this.calObjs.NewHudCal.CalcLOCompensationTool(id, val);
      }
      else
      {
        double num4 = this.FltVal("1061") / 100.0;
        double num5 = !(this.Val("NEWHUD.X715") == "") || !this.UseNewGFEHUD && !this.UseNew2015GFEHUD ? Utils.ArithmeticRounding(num1 * num4, 2) + this.FltVal("436") - (this.UseNewGFEHUD || this.UseNew2015GFEHUD ? 0.0 : this.FltVal("560")) : 0.0;
        this.SetCurrentNum("1093", num5);
        this.SetCurrentNum("1046", num5);
      }
      double num6 = this.FltVal("389") / 100.0;
      this.SetCurrentNum("439", Utils.ArithmeticRounding(num1 * num6, 2) + this.FltVal("1620") - this.FltVal("572"));
      this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem801e(id, val);
      this.SetVal("SYS.X265", "");
      this.CalculateField_SYSX266(id, val);
      if (this.Val("NEWHUD.X1718") == "Y")
        this.calObjs.ToolCal.CopyLOCompTo2010Itemization(false);
      this.SetCurrentNum("NEWHUD.X225", Utils.ArithmeticRounding(num1 * this.FltVal("NEWHUD.X223") / 100.0, 2) + this.FltVal("NEWHUD.X224") - this.FltVal("NEWHUD.X226"));
      this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem801f(id, val);
      if (this.Val("NEWHUD.X223") != string.Empty || this.Val("NEWHUD.X224") != string.Empty)
      {
        this.SetVal("NEWHUD.X227", "Lender");
        this.SetVal("NEWHUD.X230", "Broker");
        if (this.Val("NEWHUD2.X109") == "")
          this.SetVal("NEWHUD2.X109", this.Val("VEND.X293"));
      }
      else
      {
        this.SetVal("NEWHUD.X227", "");
        this.SetVal("NEWHUD.X230", "");
      }
      double num7 = this.FltVal("1109");
      double num8 = num7 * (this.FltVal("DISCLOSURE.X61") / 100.0) + this.FltVal("DISCLOSURE.X62");
      if (num8 == 0.0 && (this.Val("DISCLOSURE.X61") != "" || this.Val("DISCLOSURE.X62") != ""))
        this.SetVal("FLGFE.X38", "0.00");
      else
        this.SetCurrentNum("FLGFE.X38", num8);
      this.SetCurrentNum("FLGFE.X39", num7 * (this.FltVal("DISCLOSURE.X63") / 100.0) + this.FltVal("DISCLOSURE.X64"));
      this.SetCurrentNum("FLGFE.X40", num7 * (this.FltVal("DISCLOSURE.X65") / 100.0) + this.FltVal("DISCLOSURE.X66"));
      double num9 = this.FltVal("FLMTGCM.X14") / 100.0;
      if (num9 != 0.0)
        this.SetCurrentNum("FLMTGCM.X15", num7 * num9);
      double num10 = this.FltVal("1826");
      double num11;
      if (str3 == "FarmersHomeAdministration")
      {
        if (this.FltVal("337") != 0.0 && this.Val("NEWHUD.X1299") != "Guarantee Fee")
        {
          if (!this.IsLocked("NEWHUD.X1301"))
          {
            this.calObjs.USDACal.CopyPOCPTCAPRFromLine902ToLine819(id, val);
            this.SetVal("NEWHUD.X1299", "Guarantee Fee");
            this.SetCurrentNum("NEWHUD.X1301", this.FltVal("337"));
            this.calObjs.NewHudCal.CopyHUD2010ToGFE2010("NEWHUD.X1301", id, false);
            this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem819("NEWHUD.X1301", "");
          }
        }
        else if (!this.IsLocked("NEWHUD.X1301") && this.Val("NEWHUD.X1299") != "Guarantee Fee" && this.FltVal("NEWHUD.X1301") > 0.0)
          this.SetVal("NEWHUD.X1299", "Guarantee Fee");
        else if (!this.IsLocked("NEWHUD.X1301"))
        {
          this.SetCurrentNum("NEWHUD.X1301", num10 - this.FltVal("NEWHUD.X1302"));
          if (this.FltVal("NEWHUD.X1301") == 0.0 && this.FltVal("NEWHUD.X1302") == 0.0)
            this.SetVal("NEWHUD.X1299", "");
          this.calObjs.NewHudCal.CopyHUD2010ToGFE2010("NEWHUD.X1301", id, false);
          this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem819("NEWHUD.X1301", "");
        }
        if (!this.IsLocked("NEWHUD.X1301"))
        {
          this.SetVal("562", "");
          num11 = 0.0;
        }
        else
          num11 = num10 - this.FltVal("562");
      }
      else
      {
        if (num10 == 0.0)
          num10 = this.FltVal("1766") * this.FltVal("1209");
        num11 = num10 - this.FltVal("562");
      }
      this.SetCurrentNum("337", num11);
      this.calObjs.NewHudCal.CopyHUD2010ToGFE2010("337", id, false);
      if (str3 != "VA")
      {
        this.SetVal("1050", "");
        this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem905("1050", (string) null);
        this.SetVal("571", "");
        this.SetVal("RE88395.X52", "");
        this.SetVal("RE88395.X51", "");
        this.syncMLDS("337", this.IsLocked("337") ? this.Val("337") : num11.ToString());
        this.calObjs.NewHudCal.CopyHUD2010ToGFE2010("337", id, false);
      }
      else
      {
        double num12 = num11 - this.FltVal("571");
        this.SetCurrentNum("1050", num12);
        this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem905("1050", (string) null);
        this.SetVal("337", "");
        this.SetVal("562", "");
        this.SetVal("RE88395.X43", "");
        this.SetVal("RE88395.X42", "");
        this.syncMLDS("1050", num12.ToString());
        if (id == "571")
          this.calObjs.NewHudCal.CopyHUD2010ToGFE2010("571", id, false);
        else
          this.calObjs.NewHudCal.CopyHUD2010ToGFE2010("337", id, false);
      }
      this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem902(id, val);
      if (id == "1172" && (this.UseNewGFEHUD || this.UseNew2015GFEHUD))
      {
        this.calObjs.NewHudCal.CopyHUD2010ToGFE2010("1050", id, false);
        this.calObjs.NewHudCal.CopyHUD2010ToGFE2010("337", id, false);
      }
      string str4 = this.Val("19");
      string str5 = this.Val("1811", this.GetBorrowerPairs()[0]);
      this.SetCurrentNum("1191", !this.UseNew2015GFEHUD || str4.IndexOf("Refinance") != -1 || !(str5 == "PrimaryResidence") ? (!this.UseNewGFEHUD || str4.IndexOf("Refinance") != -1 || !(str5 == "PrimaryResidence") ? 0.0 : this.FltVal("454") + this.FltVal("559") + this.FltVal("1093") + this.FltVal("560") + this.FltVal("439") + this.FltVal("572")) : this.FltVal("454") + this.FltVal("559") + this.FltVal("1093") + this.FltVal("560") + this.FltVal("439") + this.FltVal("572") - this.FltVal("NEWHUD2.X317") - this.FltVal("NEWHUD2.X944") - this.FltVal("NEWHUD2.X960") - this.FltVal("NEWHUD2.X993") - this.FltVal("NEWHUD2.X1026"));
      if (id != "IMPORT" && id != "TPO")
      {
        if (this.UseNewGFEHUD || this.UseNew2015GFEHUD)
          this.calObjs.NewHudCal.CalcClosingCost(id, val);
        else
          this.calculateClosingCost(id, val);
      }
      this.CalculateFunder(id, val);
      this.calcBrokerLenderFeeTotals(id, val);
    }

    internal void CalculateField_SYSX266(string id, string val)
    {
      if (this.Val("389") != string.Empty || this.Val("1620") != string.Empty)
        this.SetVal("SYS.X266", "Broker");
      else
        this.SetVal("SYS.X266", "");
      if (!(this.Val("SYS.X266") == "Broker") || !(this.Val("NEWHUD2.X108") == ""))
        return;
      this.SetVal("NEWHUD2.X108", this.Val("VEND.X293"));
    }

    internal void CalculateField969(string loanType, double creditOffSet)
    {
      if (loanType == null)
        loanType = this.Val("1172");
      if (loanType != "VA")
      {
        if (!this.IsLocked("969"))
        {
          bool flag = this.Val("3551") == "Y";
          double num;
          if (this.IsLocked("NEWHUD.X1301") || loanType != "FarmersHomeAdministration")
          {
            num = this.FltVal("337") + this.FltVal("562");
            if (this.Val("SYS.X158") == "Y")
            {
              if (this.UseNewGFEHUD || this.UseNew2015GFEHUD)
                num -= this.FltVal("NEWHUD.X849");
              else
                num -= this.FltVal("337");
            }
          }
          else
          {
            num = this.FltVal("NEWHUD.X1301") + this.FltVal("NEWHUD.X1302");
            if (this.Val("NEWHUD.X1304") == "Y")
              num -= this.FltVal("NEWHUD.X1441");
          }
          if (flag)
            num = 0.0;
          this.SetCurrentNum("969", num - creditOffSet);
        }
      }
      else if (!this.IsLocked("969"))
      {
        double num = this.FltVal("1050") + this.FltVal("571");
        if (this.Val("SYS.X235") == "Y")
          num = this.UseNewGFEHUD || this.UseNew2015GFEHUD ? this.FltVal("1050") + this.FltVal("571") - this.FltVal("NEWHUD.X852") : this.FltVal("571");
        this.SetCurrentNum("969", num - creditOffSet);
      }
      this.calObjs.VACal.CalcVACashOutRefinance((string) null, (string) null);
      this.calObjs.D1003URLA2020Cal.CalcEstimatedClosingCostsAmount((string) null, (string) null);
    }

    private void calculatePrepaid(string id, string val)
    {
      if (Tracing.IsSwitchActive(GFECalculation.sw, TraceLevel.Info))
        Tracing.Log(GFECalculation.sw, TraceLevel.Info, nameof (GFECalculation), "routine: calculatePrepaid ID: " + id);
      this.calObjs.RegzCal.CalcDailyInterest(id, val);
      double val1 = Utils.ArithmeticRounding(this.FltVal("333") * this.FltVal("332"), 2) - this.FltVal("561");
      if (this.IsLocked("334"))
        this.RemoveCurrentLock("334");
      this.SetCurrentNum("334", Utils.ArithmeticRounding(val1, 2));
      this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem901(id, val);
      if (this.UseNewGFEHUD || this.UseNew2015GFEHUD)
        this.calObjs.NewHudCal.CopyHUD2010ToGFE2010("334", id, false);
      double num1 = this.CalculateInsurance("230");
      if (id == "230" && num1 != this.ToDouble(val))
      {
        this.SetCurrentNum("1322", 0.0);
        num1 = 0.0;
      }
      if (num1 != 0.0 && id != "IMPORT")
        this.SetVal("230", num1.ToString("N2"));
      this.SetCurrentNum("642", this.FltVal("L251") * this.FltVal("230") - this.FltVal("578"));
      this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem903(id, val);
      if (this.UseNewGFEHUD || this.UseNew2015GFEHUD)
        this.calObjs.NewHudCal.CopyHUD2010ToGFE2010("642", id, false);
      bool flag = this.Val("423") == "Biweekly";
      this.SetCurrentNum("656", (flag ? this.FltVal("HUD53") : this.FltVal("230")) * this.FltVal("1387") - this.FltVal("596"));
      this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem1002(id, val);
      if (this.UseNewGFEHUD || this.UseNew2015GFEHUD)
        this.calObjs.NewHudCal.CopyHUD2010ToGFE2010("656", id, false);
      this.calObjs.Cal.CalcMortgageInsurance(id, val);
      double num2 = (flag ? this.FltVal("HUD54") : this.FltVal("232")) * this.FltVal("1296");
      if (id == "1172" && num2 == 0.0)
      {
        this.SetVal("563", "");
        this.SetVal("SYS.X319", "");
        this.SetVal("POPT.X118", "");
        this.SetVal("POPT.X40", "");
      }
      double num3 = num2 - this.FltVal("563");
      this.SetCurrentNum("338", num3);
      this.calObjs.ULDDExpCal.calculateFannieMaeExportFields("338", string.Concat((object) num3));
      this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem1003(id, val);
      if (this.UseNewGFEHUD || this.UseNew2015GFEHUD)
        this.calObjs.NewHudCal.CopyHUD2010ToGFE2010("338", id, false);
      this.SetCurrentNum("L269", (flag ? this.FltVal("HUD56") : this.FltVal("L268")) * this.FltVal("L267") - this.FltVal("L270"));
      this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem1005(id, val);
      if (this.UseNewGFEHUD || this.UseNew2015GFEHUD)
        this.calObjs.NewHudCal.CopyHUD2010ToGFE2010("L269", id, false);
      double num4 = this.CalculateInsurance("231");
      if ((id == "231" || id == "1405") && num4 != this.ToDouble(val))
      {
        this.SetCurrentNum("1752", 0.0);
        num4 = 0.0;
      }
      if (num4 != 0.0)
      {
        this.SetCurrentNum("231", num4);
        if (id != "IMPORT" && string.Compare(this.Val("USEGFETAX"), "Y", true) != 0)
          this.SetVal("1405", num4.ToString("N2"));
      }
      if (string.Compare(this.Val("USEGFETAX"), "Y", true) == 0)
      {
        double num5 = this.FltVal("L268") + this.FltVal("231");
        if (SharedCalculations.IsTaxFee(this.Val("1628")))
          num5 += this.FltVal("1630");
        if (SharedCalculations.IsTaxFee(this.Val("660")))
          num5 += this.FltVal("253");
        if (SharedCalculations.IsTaxFee(this.Val("661")))
          num5 += this.FltVal("254");
        this.SetVal("1405", num5.ToString("N2"));
      }
      this.SetCurrentNum("655", Utils.ArithmeticRounding((flag ? this.FltVal("HUD52") : this.FltVal("231")) * this.FltVal("1386") - this.FltVal("595"), 2));
      this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem1004(id, val);
      if (this.UseNewGFEHUD || this.UseNew2015GFEHUD)
        this.calObjs.NewHudCal.CopyHUD2010ToGFE2010("655", id, false);
      this.SetCurrentNum("657", Utils.ArithmeticRounding((flag ? this.FltVal("HUD55") : this.FltVal("235")) * this.FltVal("1388") - this.FltVal("597"), 2));
      this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem1006(id, val);
      if (this.UseNewGFEHUD || this.UseNew2015GFEHUD)
        this.calObjs.NewHudCal.CopyHUD2010ToGFE2010("657", id, false);
      this.SetCurrentNum("1631", Utils.ArithmeticRounding((flag ? this.FltVal("HUD58") : this.FltVal("1630")) * this.FltVal("1629") - this.FltVal("1632"), 2));
      this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem1007(id, val);
      if (this.UseNewGFEHUD || this.UseNew2015GFEHUD)
        this.calObjs.NewHudCal.CopyHUD2010ToGFE2010("1631", id, false);
      this.SetCurrentNum("658", Utils.ArithmeticRounding((flag ? this.FltVal("HUD60") : this.FltVal("253")) * this.FltVal("340") - this.FltVal("598"), 2));
      this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem1008(id, val);
      if (this.UseNewGFEHUD || this.UseNew2015GFEHUD)
        this.calObjs.NewHudCal.CopyHUD2010ToGFE2010("658", id, false);
      this.SetCurrentNum("659", Utils.ArithmeticRounding((flag ? this.FltVal("HUD62") : this.FltVal("254")) * this.FltVal("341") - this.FltVal("599"), 2));
      this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem1009(id, val);
      if (this.UseNewGFEHUD || this.UseNew2015GFEHUD)
        this.calObjs.NewHudCal.CopyHUD2010ToGFE2010("659", id, false);
      if (this.UseNewGFEHUD || this.UseNew2015GFEHUD)
      {
        if (this.FltVal("NEWHUD.X1706") == 0.0)
          this.SetVal("NEWHUD.X1714", "");
        this.SetCurrentNum("NEWHUD.X1708", Utils.ArithmeticRounding((flag ? this.FltVal("HUD63") : this.FltVal("NEWHUD.X1707")) * this.FltVal("NEWHUD.X1706") - this.FltVal("NEWHUD.X1714"), 2));
        this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem1010(id, val);
        this.calObjs.NewHudCal.CopyHUD2010ToGFE2010("NEWHUD.X1708", id, false);
      }
      this.SetCurrentNum("NEWHUD.X1713", this.FltVal("596") + this.FltVal("563") + this.FltVal("595") + this.FltVal("L270") + this.FltVal("597") + this.FltVal("1632") + this.FltVal("598") + this.FltVal("599") + this.FltVal("NEWHUD.X1714"));
      if (this.UseNewGFEHUD || this.UseNew2015GFEHUD)
      {
        this.SetCurrentNum("NEWHUD.X1719", this.FltVal("656") + this.FltVal("338") + this.FltVal("655") + this.FltVal("L269") + this.FltVal("657") + this.FltVal("1631") + this.FltVal("658") + this.FltVal("659") + this.FltVal("558") + this.FltVal("NEWHUD.X1708"));
        double num6 = 0.0;
        for (int index = 185; index <= 194; ++index)
        {
          if (this.Val("POPT.X" + (object) index) == "Y" && index != 186 && index != 189)
            num6 += this.FltVal("POPT.X" + (object) (index - 78));
        }
        if (this.Val("POPT.X336") == "Y")
          num6 += this.FltVal("POPT.X326");
        if (this.Val("POPT.X337") == "Y")
          num6 += this.FltVal("POPT.X327");
        for (int index = 195; index <= 202; ++index)
        {
          if (this.Val("POPT.X" + (object) index) == "Y")
            num6 += this.FltVal("POPT.X" + (object) (index - 78));
        }
        if (this.Val("POPT.X348") == "Y")
          num6 += this.FltVal("POPT.X347");
        this.SetCurrentNum("NEWHUD.X1704", num6);
      }
      double num7 = this.UseNewGFEHUD || this.UseNew2015GFEHUD ? this.FltVal("334") + this.FltVal("642") + this.FltVal("NEWHUD.X591") + this.FltVal("1050") + this.FltVal("643") + this.FltVal("L260") + this.FltVal("1667") + this.FltVal("NEWHUD.X592") + this.FltVal("NEWHUD.X593") + this.FltVal("NEWHUD.X1719") : this.FltVal("334") + this.FltVal("642") + this.FltVal("1849") + this.FltVal("1050") + this.FltVal("643") + this.FltVal("L260") + this.FltVal("1667") + this.FltVal("656") + this.FltVal("338") + this.FltVal("L269") + this.FltVal("655") + this.FltVal("657") + this.FltVal("1631") + this.FltVal("658") + this.FltVal("659") + this.FltVal("558");
      if (this.Val("1172") == "VA")
        num7 += this.FltVal("337");
      this.SetCurrentNum("61", num7);
      if (id != "IMPORT" && id != "TPO")
      {
        if (this.UseNewGFEHUD || this.UseNew2015GFEHUD)
        {
          this.calObjs.NewHudCal.CalcClosingCost(id, val);
          this.calObjs.NewHudCal.CalcHUD1PG3(id, val);
        }
        else
          this.calculateClosingCost(id, val);
      }
      this.calObjs.FHACal.CalcMACAWP(id, val);
      this.calObjs.RegzCal.CalcIRS1098((string) null, (string) null);
    }

    private void calculateTotalPrepaidFees(string id, string val)
    {
      double num1;
      if (this.UseNewGFEHUD || this.UseNew2015GFEHUD)
      {
        double num2 = this.FltVal("561") + this.FltVal("578") + this.FltVal("NEWHUD.X594") + this.FltVal("579") + this.FltVal("L261") + this.FltVal("1668") + this.FltVal("NEWHUD.X595") + this.FltVal("NEWHUD.X596") + this.FltVal("NEWHUD.X1589") + this.FltVal("NEWHUD.X1597") + this.FltVal("NEWHUD.X1713");
        num1 = (!(this.Val("1172") == "VA") ? num2 + this.FltVal("571") : num2 + this.FltVal("562")) + (this.FltVal("334") - this.FltVal("NEWHUD.X848")) + (this.FltVal("642") - this.FltVal("NEWHUD.X850")) + (this.FltVal("NEWHUD.X591") - this.FltVal("NEWHUD.X851")) + (this.FltVal("643") - this.FltVal("NEWHUD.X853")) + (this.FltVal("L260") - this.FltVal("NEWHUD.X854")) + (this.FltVal("1667") - this.FltVal("NEWHUD.X855")) + (this.FltVal("NEWHUD.X592") - this.FltVal("NEWHUD.X856")) + (this.FltVal("NEWHUD.X593") - this.FltVal("NEWHUD.X857")) + (this.FltVal("NEWHUD.X1588") - this.FltVal("NEWHUD.X1664")) + (this.FltVal("NEWHUD.X1596") - this.FltVal("NEWHUD.X1665")) + this.FltVal("NEWHUD.X1719");
      }
      else
      {
        double num3 = this.FltVal("561") + this.FltVal("578") + this.FltVal("1850") + this.FltVal("579") + this.FltVal("L261") + this.FltVal("1668") + this.FltVal("596") + this.FltVal("563") + this.FltVal("L270") + this.FltVal("595") + this.FltVal("597") + this.FltVal("1632") + this.FltVal("598") + this.FltVal("599") + this.FltVal("558");
        num1 = !(this.Val("1172") == "VA") ? num3 + this.FltVal("571") : num3 + this.FltVal("562");
        if (this.Val("SYS.X157") != "Y")
          num1 += this.FltVal("334");
        if (this.Val("SYS.X159") != "Y")
          num1 += this.FltVal("642");
        if (this.Val("SYS.X388") != "Y")
          num1 += this.FltVal("1849");
        if (this.Val("SYS.X160") != "Y")
          num1 += this.FltVal("643");
        if (this.Val("SYS.X161") != "Y")
          num1 += this.FltVal("L260");
        if (this.Val("SYS.X238") != "Y")
          num1 += this.FltVal("1667");
        if (this.Val("SYS.X162") != "Y")
          num1 += this.FltVal("656");
        if (this.Val("SYS.X163") != "Y")
          num1 += this.FltVal("338");
        if (this.Val("SYS.X164") != "Y")
          num1 += this.FltVal("L269");
        if (this.Val("SYS.X165") != "Y")
          num1 += this.FltVal("655");
        if (this.Val("SYS.X167") != "Y")
          num1 += this.FltVal("657");
        if (this.Val("SYS.X239") != "Y")
          num1 += this.FltVal("1631");
        if (this.Val("SYS.X168") != "Y")
          num1 += this.FltVal("658");
        if (this.Val("SYS.X169") != "Y")
          num1 += this.FltVal("659");
      }
      this.SetCurrentNum("138", num1);
      double num4 = num1;
      if (this.UseNewGFEHUD || this.UseNew2015GFEHUD || this.Val("SYS.X235") != "Y")
        num4 += this.FltVal("1050");
      if (this.UseNewGFEHUD || this.UseNew2015GFEHUD || this.Val("SYS.X158") != "Y")
        num4 += this.FltVal("337");
      this.SetCurrentNum("TOTAL_PREPAID", num4);
      this.calObjs.D1003URLA2020Cal.CalcEstimatedClosingCostsAmount(id, val);
    }

    private void calculateClosingCost(string id, string val)
    {
      if (Tracing.IsSwitchActive(GFECalculation.sw, TraceLevel.Info))
        Tracing.Log(GFECalculation.sw, TraceLevel.Info, nameof (GFECalculation), "routine: CalcClosingCost  ID: " + id);
      if (this.UseNewGFEHUD || this.UseNew2015GFEHUD)
      {
        this.calObjs.NewHudCal.CalcHUD1PG2(id, val);
      }
      else
      {
        int length1 = CalculationBase.BorrowerFields.Length;
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
        for (int index = 0; index < length1; ++index)
        {
          double num11 = this.FltVal(CalculationBase.BorrowerFields[index]);
          if (this.Val("SYS.X" + CalculationBase.POCChecks[index]) != "Y")
          {
            num1 += num11;
            if (this.Val("SYS.X" + CalculationBase.PTBChecks[index]) == "Broker")
              num10 += num11;
          }
          else
            num8 += num11;
          if (CalculationBase.PFCChecks[index] != CalculationBase.nil && this.Val("SYS.X" + CalculationBase.PFCChecks[index]) == "Y")
            num2 += num11;
          if (CalculationBase.FHAChecks[index] != CalculationBase.nil && this.Val("SYS.X" + CalculationBase.FHAChecks[index]) == "Y" && CalculationBase.FHAChecks[index] != "87")
            num3 += num11;
          if ((index <= 16 || index >= 32) && CalculationBase.BorrowerFields[index] != "1849" && this.Val("SYS.X" + CalculationBase.FHAChecks[index]) != "Y")
            num9 += num11;
          if (this.Val("SYS.X" + CalculationBase.POCChecks[index]) != "Y")
          {
            switch (this.Val("SYS.X" + CalculationBase.PaidByChecks[index]))
            {
              case "Broker":
                num5 += num11;
                continue;
              case "Lender":
                num6 += num11;
                continue;
              case "Other":
                num7 += num11;
                continue;
              default:
                num4 += num11;
                continue;
            }
          }
        }
        double num12 = 0.0;
        if (this.IsLocked("BKRPCC"))
          num12 += this.FltVal("BKRPCC") - num5;
        this.SetCurrentNum("BKRPCC", num5);
        if (this.IsLocked("LENPCC"))
          num12 += this.FltVal("LENPCC") - num6;
        this.SetCurrentNum("LENPCC", num6);
        if (this.IsLocked("OTHPCC"))
          num12 += this.FltVal("OTHPCC") - num7;
        this.SetCurrentNum("OTHPCC", num7);
        this.SetCurrentNum("TOTPOC", num8);
        this.SetCurrentNum("1137", num9 - this.FltVal("1093"));
        this.SetCurrentNum("949", num2);
        double num13 = num1 + this.FltVal("558");
        if (!this.IsLocked("304"))
          this.SetCurrentNum("304", num13);
        int length2 = CalculationBase.SellerFields.Length;
        double num14 = 0.0;
        double num15 = 0.0;
        for (int index = 0; index < length2; ++index)
        {
          double num16 = this.FltVal(CalculationBase.SellerFields[index]);
          num14 += num16;
          if (CalculationBase.FHAChecks[index] != CalculationBase.nil && this.Val("SYS.X" + CalculationBase.FHAChecks[index]) == "Y")
            num15 += num16;
          if (this.Val("SYS.X" + CalculationBase.POCChecks[index]) != "Y" && this.Val("SYS.X" + CalculationBase.PTBChecks[index]) == "Broker")
            num10 += num16;
        }
        if (this.IsLocked("143"))
          num12 += this.FltVal("143") - num14;
        double num17 = num10 + (this.FltVal("1663") + this.FltVal("1665"));
        if (this.Val("2833") == "Y")
          num17 -= this.FltVal("2005");
        this.SetCurrentNum("1988", num17);
        this.SetCurrentNum("143", num14);
        this.SetCurrentNum("BORPCC", num4 + this.FltVal("558") - num12);
        this.SetCurrentNum("386", num3 + num15);
        this.SetCurrentNum("1131", num15);
        this.calculateTotalCosts(id, val);
        this.calObjs.D1003URLA2020Cal.CalcOtherCredits(id, val);
        this.calObjs.D1003URLA2020Cal.CalcTotalCredits(id, val);
        this.calObjs.FHACal.CalcMACAWP(id, val);
        this.calObjs.CloserCal.CalClosing(id, val);
      }
    }

    private void calculateTotalCosts(string id, string val)
    {
      if (this.UseNewGFEHUD || this.UseNew2015GFEHUD)
      {
        this.calObjs.NewHudCal.CalcTotalCosts(id, val);
      }
      else
      {
        if (Tracing.IsSwitchActive(GFECalculation.sw, TraceLevel.Info))
          Tracing.Log(GFECalculation.sw, TraceLevel.Info, nameof (GFECalculation), "routine: calculateTotalCosts ID: " + id);
        this.SetCurrentNum("TNBPCC", this.FltVal("143") + this.FltVal("BKRPCC") + this.FltVal("LENPCC") + this.FltVal("OTHPCC"));
        this.SetCurrentNum("TOTPCC", this.FltVal("BORPCC") + this.FltVal("TNBPCC"));
        this.SetCurrentNum("1852", this.FltVal("BKRPCC") + this.FltVal("LENPCC") + this.FltVal("OTHPCC"));
        double num1 = this.FltVal("304") + this.FltVal("LENPCC") - this.FltVal("138");
        double num2 = !this.IsLocked("143") ? num1 + this.FltVal("143") : num1 + this.FltVal("143", true);
        this.SetCurrentNum("TOTAL_CC", num2);
        double num3 = num2 - this.FltVal("LENPCC");
        double num4;
        if (this.Val("1172") == "VA")
        {
          if (this.Val("SYS.X235") != "Y")
            num3 -= this.FltVal("1050");
          num4 = num3 - this.FltVal("571");
        }
        else
        {
          if (this.Val("SYS.X158") != "Y")
            num3 -= this.FltVal("337");
          num4 = num3 - this.FltVal("562");
        }
        double num5 = Utils.ArithmeticRounding(num4 - this.FltVal("1093"), 2);
        if (num5 < 0.0)
          this.SetCurrentNum("137", 0.0);
        else
          this.SetCurrentNum("137", num5);
        double num6 = this.FltVal("136") + this.FltVal("967") + this.FltVal("968");
        double num7 = !this.USEURLA2020 || !this.IsLocked("URLA.X146") ? num6 + (this.FltVal("138") + this.FltVal("137") + this.FltVal("969")) : num6 + this.FltVal("URLA.X146");
        this.SetCurrentNum("1073", Math.Round((!this.USEURLA2020 ? num7 + this.FltVal("1092") : num7 + (this.FltVal("26") + this.FltVal("URLA.X145"))) + this.FltVal("1093") + 0.0001, 2));
        double num8 = this.FltVal("1073");
        if (this.Val("420") == "FirstLien")
          this.SetCurrentNum("1845", 0.0);
        double num9 = Math.Round(!this.USEURLA2020 ? this.FltVal("140") + this.FltVal("143") + this.FltVal("141") + this.FltVal("1095") + this.FltVal("1115") + this.FltVal("1647") + this.FltVal("1845") + this.FltVal("1851") + this.FltVal("1852") + this.FltVal("2") : this.FltVal("URLA.X148") + this.FltVal("URLA.X152"), 2);
        this.SetCurrentNum("1844", num9);
        double num10 = num8 - num9;
        this.SetCurrentNum("743", num10);
        double num11 = this.FltVal("142");
        this.SetCurrentNum("142", num10);
        if (num10 < 0.0)
          this.SetCurrentNum("ULDD.RefinanceCashOutAmount", -num10);
        else
          this.SetCurrentNum("ULDD.RefinanceCashOutAmount", 0.0);
        if (num11 != num10 || this.FltVal("1206") == 0.0)
        {
          this.calObjs.FHACal.CalcFredMac(id, val);
          if (id != "1109")
            this.calObjs.RegzCal.CalcAPR(id, val);
        }
        this.calObjs.VACal.CalcVACashOutRefinance(id, val);
        this.calObjs.D1003URLA2020Cal.CalcEstimatedClosingCostsAmount(id, val);
        this.calObjs.D1003URLA2020Cal.CalcTotalCredits(id, val);
      }
    }

    internal void CalcHOEPAAPR()
    {
      switch (this.Val("608"))
      {
        case "Fixed":
          this.SetCurrentNum("S32DISC.X177", this.FltVal("799"));
          break;
        case "AdjustableRate":
          double num1 = this.FltVal("1827");
          double num2 = this.FltVal("3");
          if (num1 > num2)
          {
            bool calculationDiagnostic = EnConfigurationSettings.GlobalSettings.AllowCalculationDiagnostic;
            if (calculationDiagnostic)
              EnConfigurationSettings.GlobalSettings.AllowCalculationDiagnostic = false;
            LoanData loanData = new LoanData(this.loan.XmlDocClone, this.loan.Settings, false, true);
            LoanCalculator loanCalculator = new LoanCalculator(this.sessionObjects, this.calObjs.LoanConfiguration, loanData, true, this.calObjs.ExternalLateFeeSettings, true);
            loanData.Calculator.PerformanceEnabled = false;
            loanData.SetField("608", "Fixed");
            loanData.SetField("3", num1.ToString());
            if (calculationDiagnostic)
              EnConfigurationSettings.GlobalSettings.AllowCalculationDiagnostic = calculationDiagnostic;
            this.SetVal("S32DISC.X177", loanData.GetField("799"));
            break;
          }
          this.SetCurrentNum("S32DISC.X177", this.FltVal("799"));
          break;
      }
      this.calculateSection32((string) null, (string) null);
    }

    private void calculateSection32(string id, string val)
    {
      if (Tracing.IsSwitchActive(GFECalculation.sw, TraceLevel.Info))
        Tracing.Log(GFECalculation.sw, TraceLevel.Info, nameof (GFECalculation), "routine: calculateSection32");
      string str1 = this.Val("420");
      double num1 = this.FltVal("S32DISC.X177");
      double num2 = this.FltVal("3134");
      string empty1 = string.Empty;
      this.SetVal("S32DISC.X2", !(str1 == "SecondLien") ? (Utils.ArithmeticRounding(num1 - num2, 3) <= 6.5 ? "does not" : "does") : (Utils.ArithmeticRounding(num1 - num2, 3) <= 8.5 ? "does not" : "does"));
      int num3 = this.UseNewGFEHUD || this.UseNew2015GFEHUD ? HUDGFE2010Fields.PAIDTOFORSECTION32.Length : CalculationBase.SEC32SecondChecks.Length;
      double num4 = 0.0;
      double num5 = 0.0;
      string empty2 = string.Empty;
      string str2 = this.Val("1172");
      bool flag1 = this.Val("NEWHUD.X1139") == "Y";
      double num6 = this.FltVal("2");
      bool useNewSec32Rule = false;
      DateTime date1 = Utils.ParseDate((object) this.Val("745"));
      DateTime date2 = Utils.ParseDate((object) "01/10/2014");
      if (date1.Date >= date2.Date)
        useNewSec32Rule = true;
      if (this.UseNew2015GFEHUD || this.UseNewGFEHUD)
      {
        if (this.UseNew2015GFEHUD)
        {
          for (int index = 0; index < HUDGFE2010Fields.WHOLEPOC_FIELDS.Count; ++index)
          {
            string[] strArray = HUDGFE2010Fields.WHOLEPOC_FIELDS[index];
            if (strArray[HUDGFE2010Fields.PTCPOCINDEX_2015SEC32AMT] != "")
              num4 += this.FltVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015SEC32AMT]);
          }
        }
        else if (this.UseNewGFEHUD)
        {
          for (int index = 0; index < num3; ++index)
          {
            if ((this.UseNew2015GFEHUD || !HUDGFE2010Fields.BORROWERFIELDS[index].StartsWith("NEWHUD2.X")) && (!this.UseNew2015GFEHUD || !(HUDGFE2010Fields.BORROWERFIELDS[index] == "NEWHUD.X645")) && !(HUDGFE2010Fields.BORROWERFIELDS[index] == string.Empty) && !(HUDGFE2010Fields.PAIDTOFORSECTION32[index] == string.Empty) && (!flag1 || !(HUDGFE2010Fields.BORROWERFIELDS[index] == "NEWHUD.X687")) && !(HUDGFE2010Fields.PAIDTOFORSECTION32[index] == "SYS.X320"))
            {
              if (useNewSec32Rule)
              {
                if (!(HUDGFE2010Fields.BORROWERFIELDS[index] == "1050") || !(str2 == "VA"))
                {
                  if (str2 == "FarmersHomeAdministration")
                  {
                    if (HUDGFE2010Fields.BORROWERFIELDS[index] == "NEWHUD.X1301" && !this.IsLocked("NEWHUD.X1301") || HUDGFE2010Fields.BORROWERFIELDS[index] == "337" && this.IsLocked("NEWHUD.X1301"))
                      continue;
                  }
                  else if ((str2 == "FHA" || str2 == "Conventional") && HUDGFE2010Fields.BORROWERFIELDS[index] == "337")
                    continue;
                }
                else
                  continue;
              }
              num4 += this.addSection32BorrowerPaid(HUDGFE2010Fields.BORROWERFIELDS[index], HUDGFE2010Fields.PAIDTOFORSECTION32[index], HUDGFE2010Fields.PAIDBYFORSECTION32[index], useNewSec32Rule);
            }
          }
          if (useNewSec32Rule)
          {
            for (int index = 0; index < num3; ++index)
            {
              if (!(HUDGFE2010Fields.SELLERFIELDS[index] == "565") && !(HUDGFE2010Fields.SELLERFIELDS[index] == "NEWHUD.X781") && !(HUDGFE2010Fields.SELLERFIELDS[index] == "561") && !(HUDGFE2010Fields.SELLERFIELDS[index] == "595") && !(HUDGFE2010Fields.SELLERFIELDS[index] == "587") && !(HUDGFE2010Fields.SELLERFIELDS[index] == "NEWHUD.X787") && !(HUDGFE2010Fields.SELLERFIELDS[index] == "593") && !(HUDGFE2010Fields.SELLERFIELDS[index] == "594"))
                num4 += this.addSection32SellerPaid(HUDGFE2010Fields.SELLERFIELDS[index], HUDGFE2010Fields.PAIDTOFORSECTION32[index]);
            }
            num4 = num4 + this.addSection32SellerPaid("572", "SYS.X266") + this.addSection32SellerPaid("NEWHUD.X226", "NEWHUD.X230") + this.addSection32SellerPaid("200", "SYS.X290") + this.addSection32SellerPaid("1626", "SYS.X292") + this.addSection32SellerPaid("1840", "SYS.X297") + this.addSection32SellerPaid("1843", "SYS.X302") + this.addSection32SellerPaid("NEWHUD.X779", "NEWHUD.X690") + this.addSection32SellerPaid("NEWHUD.X1238", "NEWHUD.X1242") + this.addSection32SellerPaid("NEWHUD.X1246", "NEWHUD.X1250") + this.addSection32SellerPaid("NEWHUD.X1254", "NEWHUD.X1258") + this.addSection32SellerPaid("NEWHUD.X1262", "NEWHUD.X1266") + this.addSection32SellerPaid("NEWHUD.X1270", "NEWHUD.X1274") + this.addSection32SellerPaid("NEWHUD.X1278", "NEWHUD.X1282") + this.addSection32SellerPaid("NEWHUD.X1286", "NEWHUD.X1290");
            if (flag1)
              num4 += this.addSection32SellerPaid("NEWHUD.X1152", "NEWHUD.X1178");
            if (this.Val("SYS.X318") == "Affiliate" && this.Val("SYS.X317") != "Lender")
              num4 += this.FltVal("656");
            if (this.Val("SYS.X326") == "Affiliate" && this.Val("SYS.X325") != "Lender")
              num4 += this.FltVal("657");
            if (this.Val("SYS.X328") == "Affiliate" && this.Val("SYS.X327") != "Lender")
              num4 += this.addSection32InsurancePaid("1631", "1628");
            if (this.Val("SYS.X330") == "Affiliate" && this.Val("SYS.X329") != "Lender")
              num4 += this.addSection32InsurancePaid("658", "660");
            if (this.Val("SYS.X332") == "Affiliate" && this.Val("SYS.X331") != "Lender")
              num4 += this.addSection32InsurancePaid("659", "661");
          }
          num4 = num4 + this.addSection32BorrowerPaid("454", "SYS.X252", "SYS.X251", useNewSec32Rule) + this.addSection32BorrowerPaid("L228", "SYS.X262", "SYS.X261", useNewSec32Rule) + this.addSection32BorrowerPaid("1621", "SYS.X270", "SYS.X269", useNewSec32Rule) + this.addSection32BorrowerPaid("367", "SYS.X272", "SYS.X271", useNewSec32Rule) + this.addSection32BorrowerPaid("439", "SYS.X266", "SYS.X265", useNewSec32Rule) + this.addSection32BorrowerPaid("NEWHUD.X225", "NEWHUD.X230", "", useNewSec32Rule) + this.addSection32BorrowerPaid("155", "SYS.X290", "SYS.X289", useNewSec32Rule) + this.addSection32BorrowerPaid("1625", "SYS.X292", "SYS.X291", useNewSec32Rule) + this.addSection32BorrowerPaid("1839", "SYS.X297", "SYS.X296", useNewSec32Rule) + this.addSection32BorrowerPaid("1842", "SYS.X302", "SYS.X301", useNewSec32Rule) + this.addSection32BorrowerPaid("NEWHUD.X733", "NEWHUD.X690", "NEWHUD.X748", useNewSec32Rule) + this.addSection32BorrowerPaid("NEWHUD.X1237", "NEWHUD.X1242", "NEWHUD.X1239", useNewSec32Rule) + this.addSection32BorrowerPaid("NEWHUD.X1245", "NEWHUD.X1250", "NEWHUD.X1247", useNewSec32Rule) + this.addSection32BorrowerPaid("NEWHUD.X1253", "NEWHUD.X1258", "NEWHUD.X1255", useNewSec32Rule) + this.addSection32BorrowerPaid("NEWHUD.X1261", "NEWHUD.X1266", "NEWHUD.X1263", useNewSec32Rule) + this.addSection32BorrowerPaid("NEWHUD.X1269", "NEWHUD.X1274", "NEWHUD.X1271", useNewSec32Rule) + this.addSection32BorrowerPaid("NEWHUD.X1277", "NEWHUD.X1282", "NEWHUD.X1279", useNewSec32Rule) + this.addSection32BorrowerPaid("NEWHUD.X1285", "NEWHUD.X1290", "NEWHUD.X1287", useNewSec32Rule);
          if (flag1)
          {
            if (!useNewSec32Rule)
              num4 = num4 - this.addSection32BorrowerPaid("NEWHUD.X1142", "NEWHUD.X1168", "", useNewSec32Rule) - this.addSection32BorrowerPaid("NEWHUD.X1144", "NEWHUD.X1170", "", useNewSec32Rule) - this.addSection32BorrowerPaid("NEWHUD.X1146", "NEWHUD.X1172", "", useNewSec32Rule) - this.addSection32BorrowerPaid("NEWHUD.X1148", "NEWHUD.X1174", "", useNewSec32Rule);
            num4 = num4 + this.addSection32BorrowerPaid("NEWHUD.X1151", "NEWHUD.X1178", "NEWHUD.X1175", useNewSec32Rule) + this.addSection32BorrowerPaid("NEWHUD.X1155", "NEWHUD.X1182", "NEWHUD.X1179", useNewSec32Rule) + this.addSection32BorrowerPaid("NEWHUD.X1159", "NEWHUD.X1186", "NEWHUD.X1183", useNewSec32Rule) + this.addSection32BorrowerPaid("NEWHUD.X1163", "NEWHUD.X1190", "NEWHUD.X1187", useNewSec32Rule);
          }
          else if (this.Val("NEWHUD.X715") == "Include Origination Credit")
            num4 += this.addSection32BorrowerPaid("1663", "NEWHUD.X627", "NEWHUD.X749", useNewSec32Rule);
          else if (this.Val("NEWHUD.X715") == "Include Origination Points")
            num4 += this.addSection32BorrowerPaid("NEWHUD.X15", "NEWHUD.X627", "NEWHUD.X749", useNewSec32Rule);
          if (!useNewSec32Rule)
          {
            string str3 = str2 != "FarmersHomeAdministration" ? this.Val("SYS.X320") : "";
            if (str3 != "Seller" && str3 != "Investor" && str3 != "Other")
              num4 += str2 == "FarmersHomeAdministration" ? this.FltVal("NEWHUD.X1708") : this.FltVal("338");
          }
          else if (str2 != "FarmersHomeAdministration" && str2 != "FHA" && this.Val("SYS.X319") != "Lender")
          {
            string str4 = this.Val("SYS.X320");
            if (str4 != "Seller" && str4 != "Investor" && str4 != "Other")
              num4 += this.FltVal("338");
          }
        }
      }
      else
      {
        for (int index = 0; index < num3; ++index)
        {
          double num7 = this.FltVal(CalculationBase.SEC32BorFields[index]);
          if (this.Val("S32DISC.X" + CalculationBase.SEC32FirstChecks[index]) == "Y")
            num4 += num7;
          if (this.Val("S32DISC.X" + CalculationBase.SEC32SecondChecks[index]) == "Y" && this.Val("SYS.X" + CalculationBase.PFCChecks[index]) != "Y")
            num5 += num7;
        }
      }
      if (useNewSec32Rule)
      {
        double num8 = num4 + this.PrepaymentPenaltyAmount();
        this.calObjs.ATRQMCal.CalcDiscountPoints(id, val);
        double num9 = this.FltVal("QM.X369") < this.FltVal("QM.X136") ? num8 - this.FltVal("QM.X370") : num8 - this.FltVal("QM.X111");
        if (!this.UseNew2015GFEHUD)
        {
          string str5 = this.Val("SYS.X306");
          if (str5 != "Seller" && str5 != "Investor" && str5 != "Other" && str2 == "Conventional" && this.Val("SYS.X305") != "Lender")
          {
            if (this.FltVal("1107") >= 1.75 && this.Val("3262") == "Y")
              num9 += (this.FltVal("1107") - 1.75) * this.FltVal("1109") / 100.0;
            else
              num9 += this.FltVal("337");
          }
        }
        num4 = num9 + this.FltVal("RE88395.X315");
      }
      if (this.Val("1172") == "HELOC")
        num4 += this.FltVal("HELOC.ParticipationFees") + this.FltVal("HELOC.TransactionFees");
      if (this.Val("COMPLIANCEVERSION.CASASRNX141") != "Y" && this.Val("COMPLIANCEVERSION.NEWBUYDOWNENABLED") == "Y" && this.Val("CASASRN.X141") == "Seller" && this.Val("S32DISC.X181") != "Y")
        num4 += this.FltVal("QM.X378");
      this.SetCurrentNum("S32DISC.X48", num4);
      this.calObjs.ATRQMCal.CalcPointsAndFees(id, val);
      double dollarAmount;
      if (this.UseNewGFEHUD || this.UseNew2015GFEHUD)
      {
        DateTime date3 = Utils.ParseDate((object) this.Val("1887"));
        DateTime date4 = Utils.ParseDate((object) this.Val("748"));
        DateTime date5 = Utils.ParseDate((object) this.Val("763"));
        int yearToReturn = 2023;
        bool flag2 = true;
        List<FedTresholdAdjustment> thresholdsFromCache = this != null ? this.sessionObjects?.GetThresholdsFromCache() : (List<FedTresholdAdjustment>) null;
        if (thresholdsFromCache == null || thresholdsFromCache.Count<FedTresholdAdjustment>() == 0)
          flag2 = false;
        else
          yearToReturn = thresholdsFromCache[0].AdjustmentYear;
        DateTime closingDateToUse = this.findClosingDateToUse();
        bool flag3 = false;
        if (flag2)
        {
          for (; yearToReturn >= 2023; yearToReturn--)
          {
            DateTime t2 = new DateTime(yearToReturn, 1, 1);
            if (DateTime.Compare(closingDateToUse, t2) >= 0)
            {
              FedTresholdAdjustment tresholdAdjustment = thresholdsFromCache.Find((Predicate<FedTresholdAdjustment>) (x => x.AdjustmentYear == yearToReturn && x.RuleIndex == 4));
              if (tresholdAdjustment != null)
              {
                this.SetVal("S32DISC.X100", num6 >= Utils.ParseDouble((object) tresholdAdjustment.UpperRange) ? "5" : "8");
                flag3 = true;
                break;
              }
            }
          }
        }
        if (!flag3)
        {
          if (DateTime.Compare(date3, Utils.ParseDate((object) "01/01/2023")) >= 0 || DateTime.Compare(date4, Utils.ParseDate((object) "01/01/2023")) >= 0 || DateTime.Compare(date5, Utils.ParseDate((object) "01/01/2023")) >= 0)
            this.SetVal("S32DISC.X100", num6 >= 24866.0 ? "5" : "8");
          else if (DateTime.Compare(date3, Utils.ParseDate((object) "01/01/2022")) >= 0 || DateTime.Compare(date4, Utils.ParseDate((object) "01/01/2022")) >= 0 || DateTime.Compare(date5, Utils.ParseDate((object) "01/01/2022")) >= 0)
            this.SetVal("S32DISC.X100", num6 >= 22969.0 ? "5" : "8");
          else if (DateTime.Compare(date3, Utils.ParseDate((object) "01/01/2021")) >= 0 || DateTime.Compare(date4, Utils.ParseDate((object) "01/01/2021")) >= 0 || DateTime.Compare(date5, Utils.ParseDate((object) "01/01/2021")) >= 0)
            this.SetVal("S32DISC.X100", num6 >= 22052.0 ? "5" : "8");
          else if (DateTime.Compare(date3, Utils.ParseDate((object) "01/01/2020")) >= 0 || DateTime.Compare(date4, Utils.ParseDate((object) "01/01/2020")) >= 0 || DateTime.Compare(date5, Utils.ParseDate((object) "01/01/2020")) >= 0)
            this.SetVal("S32DISC.X100", num6 >= 21980.0 ? "5" : "8");
          else if (DateTime.Compare(date1, Utils.ParseDate((object) "01/01/2019")) >= 0)
            this.SetVal("S32DISC.X100", num6 >= 21549.0 ? "5" : "8");
          else if (DateTime.Compare(date1, Utils.ParseDate((object) "01/01/2018")) >= 0)
            this.SetVal("S32DISC.X100", num6 >= 21032.0 ? "5" : "8");
          else if (this.IsLocked("S32DISC.X100"))
            this.RemoveLock("S32DISC.X100");
        }
        dollarAmount = this.FltVal("QM.X120") * (this.FltVal("S32DISC.X100") / 100.0);
        bool flag4 = false;
        if (thresholdsFromCache != null && thresholdsFromCache.Count > 0)
          yearToReturn = thresholdsFromCache[0].AdjustmentYear;
        if (flag2)
        {
          for (; yearToReturn >= 2023; yearToReturn--)
          {
            DateTime t2 = new DateTime(yearToReturn, 1, 1);
            if (DateTime.Compare(closingDateToUse, t2) >= 0)
            {
              FedTresholdAdjustment tresholdAdjustment = thresholdsFromCache.Find((Predicate<FedTresholdAdjustment>) (x => x.AdjustmentYear == yearToReturn && x.RuleIndex == 4));
              if (tresholdAdjustment != null)
              {
                if (num6 < Utils.ParseDouble((object) tresholdAdjustment.UpperRange) && dollarAmount > Utils.ParseDouble((object) tresholdAdjustment.RuleValue))
                  dollarAmount = Utils.ParseDouble((object) tresholdAdjustment.RuleValue);
                flag4 = true;
                break;
              }
            }
          }
        }
        if (!flag4)
        {
          if (DateTime.Compare(date3, Utils.ParseDate((object) "01/01/2023")) >= 0 || DateTime.Compare(date4, Utils.ParseDate((object) "01/01/2023")) >= 0 || DateTime.Compare(date5, Utils.ParseDate((object) "01/01/2023")) >= 0)
          {
            if (num6 < 24866.0 && dollarAmount > 1243.0)
              dollarAmount = 1243.0;
          }
          else if (DateTime.Compare(date3, Utils.ParseDate((object) "01/01/2022")) >= 0 || DateTime.Compare(date4, Utils.ParseDate((object) "01/01/2022")) >= 0 || DateTime.Compare(date5, Utils.ParseDate((object) "01/01/2022")) >= 0)
          {
            if (num6 < 22969.0 && dollarAmount > 1148.0)
              dollarAmount = 1148.0;
          }
          else if (DateTime.Compare(date3, Utils.ParseDate((object) "01/01/2021")) >= 0 || DateTime.Compare(date4, Utils.ParseDate((object) "01/01/2021")) >= 0 || DateTime.Compare(date5, Utils.ParseDate((object) "01/01/2021")) >= 0)
          {
            if (num6 < 22052.0 && dollarAmount > 1103.0)
              dollarAmount = 1103.0;
          }
          else if (DateTime.Compare(date3, Utils.ParseDate((object) "01/01/2020")) >= 0 || DateTime.Compare(date4, Utils.ParseDate((object) "01/01/2020")) >= 0 || DateTime.Compare(date5, Utils.ParseDate((object) "01/01/2020")) >= 0)
          {
            if (num6 < 21980.0 && dollarAmount > 1099.0)
              dollarAmount = 1099.0;
          }
          else if (DateTime.Compare(date1, Utils.ParseDate((object) "01/01/2019")) >= 0)
          {
            if (num6 < 21549.0 && dollarAmount > 1077.0)
              dollarAmount = 1077.0;
          }
          else if (DateTime.Compare(date1, Utils.ParseDate((object) "01/01/2018")) >= 0)
          {
            if (num6 < 21032.0 && dollarAmount > 1052.0)
              dollarAmount = 1052.0;
          }
          else if (DateTime.Compare(date1, Utils.ParseDate((object) "01/01/2017")) >= 0)
          {
            if (num6 < 20579.0 && dollarAmount > 1029.0)
              dollarAmount = 1029.0;
          }
          else if (DateTime.Compare(date1, Utils.ParseDate((object) "01/01/2016")) >= 0)
          {
            if (num6 < 20350.0 && dollarAmount > 1017.0)
              dollarAmount = 1017.0;
          }
          else if (DateTime.Compare(date1, Utils.ParseDate((object) "01/01/2015")) >= 0)
          {
            if (num6 < 20391.0 && dollarAmount > 1020.0)
              dollarAmount = 1020.0;
          }
          else if (useNewSec32Rule && num6 < 20000.0 && dollarAmount > 1000.0)
            dollarAmount = 1000.0;
        }
      }
      else
        dollarAmount = (this.FltVal("948") - num5) * (this.FltVal("S32DISC.X100") / 100.0);
      this.SetCurrentNum("S32DISC.X101", Utils.TruncateToCents(dollarAmount));
      this.SetVal("S32DISC.X49", this.FltVal("S32DISC.X48") <= this.FltVal("S32DISC.X101") ? "does not" : "does");
      double num10 = this.FltVal("RE88395.X315");
      double num11 = this.FltVal("RE88395.X316");
      string str6 = this.Val("675");
      if (num6 == 0.0 || str6 == string.Empty || num10 == 0.0)
        this.SetVal("S32DISC.X180", "");
      else if (num11 > 36.0)
        this.SetVal("S32DISC.X180", "will");
      else if (num11 <= 36.0)
        this.SetVal("S32DISC.X180", "will not");
      double num12 = num6 == 0.0 ? 0.0 : Utils.ArithmeticRounding(num10 / num6 * 100.0, 3);
      this.SetCurrentNum("S32DISC.X178", num12);
      string str7 = this.Val("S32DISC.X49");
      string val1 = num6 == 0.0 || str6 == string.Empty && num10 == 0.0 ? "" : (num12 <= 2.0 ? "does not" : "does");
      this.SetVal("S32DISC.X179", val1);
      string str8 = this.Val("S32DISC.X2");
      if (str8 == "does" || str7 == "does" || val1 == "does")
        this.SetVal("S32DISC.X51", "does");
      else if (str8 != "" || str7 != "" || val1 != "")
        this.SetVal("S32DISC.X51", "does not");
      else
        this.SetVal("S32DISC.X51", "");
      this.calObjs.HMDACal.CalcHOEPAStatus(id, val);
    }

    internal double PrepaymentPenaltyAmount()
    {
      double num = this.FltVal("NTB.X16");
      string str1 = this.Val("299");
      string str2 = this.Val("NTB.X34");
      return num > 0.0 && (this.Val("QM.X2") == "Y" || str1 == "CashOutOriginalLender" || str1 == "NoCashOutOriginalLender" || str2 == "Originated by the same broker and is funded by the same creditor" || str2 == "Made or is currently held by the same creditor or an affiliate of the creditor") ? this.FltVal("NTB.X16") : 0.0;
    }

    private double addSection32BorrowerPaid(
      string borID,
      string paidToID,
      string paidByID,
      bool useNewSec32Rule)
    {
      string strA = this.Val(paidToID);
      string str = paidByID != string.Empty ? this.Val(paidByID) : "";
      if (useNewSec32Rule)
      {
        if (str == "Lender" && borID != "NEWHUD.X225")
          return 0.0;
        switch (paidToID)
        {
          case "SYS.X308":
            return 0.0;
          case "SYS.X318":
          case "SYS.X326":
          case "SYS.X328":
          case "SYS.X330":
          case "SYS.X332":
            return 0.0;
        }
      }
      if (!(strA != "Seller") || !(strA != "Investor") || !(strA != "Other"))
        return 0.0;
      double num1 = this.FltVal(borID);
      if (useNewSec32Rule && (paidToID == "NEWHUD.X804" || paidToID == "NEWHUD.X805") && string.Compare(strA, "Affiliate", true) == 0)
        num1 = this.FltVal(paidToID == "NEWHUD.X804" ? "NEWHUD.X1724" : "NEWHUD.X1725");
      if (num1 == 0.0)
        return 0.0;
      if (borID == "NEWHUD.X225")
      {
        if (useNewSec32Rule)
        {
          num1 = this.FltVal("NEWHUD.X225");
          if (this.Val("QM.X372") == "Y" && this.FltVal("QM.X371") > num1)
            num1 = this.FltVal("QM.X371");
        }
        else
        {
          double num2 = this.FltVal("1663");
          num1 = num2 <= this.FltVal("NEWHUD.X225") ? num2 * -1.0 : this.FltVal("NEWHUD.X225") * -1.0;
        }
      }
      return num1;
    }

    private double addSection32SellerPaid(string sellerID, string paidToID)
    {
      string str = this.Val(paidToID);
      return str == "Broker" || str == "Affiliate" || str == "Lender" ? this.FltVal(sellerID) : 0.0;
    }

    private double addSection32InsurancePaid(string borID, string feeDescriptionID)
    {
      string lower = this.Val(feeDescriptionID).ToLower();
      if (lower.IndexOf("insurance") != -1)
        return this.FltVal(borID);
      return lower.IndexOf("earth quake") != -1 || lower.IndexOf("earthquake") != -1 || lower.IndexOf("windstorm") != -1 || lower.IndexOf("wind storm") != -1 || lower.IndexOf("fire") != -1 ? this.FltVal(borID) : 0.0;
    }

    private void calculateHighPrice(string id, string val)
    {
      string str1 = this.Val("19");
      if (this.Val("1811") != "PrimaryResidence" || str1 != "Purchase" && str1 != "Cash-Out Refinance" && str1 != "NoCash-Out Refinance" && str1 != "ConstructionToPermanent" || this.IntVal("16") >= 5)
      {
        this.SetVal("3135", "");
      }
      else
      {
        string str2 = this.Val("420");
        string str3 = this.Val("3331");
        double num1 = this.FltVal("799");
        double num2 = this.FltVal("3134");
        if (num1 == 0.0 || num2 == 0.0 || str3 == string.Empty || str2 == string.Empty)
        {
          this.SetVal("3135", "");
        }
        else
        {
          switch (str2)
          {
            case "FirstLien":
              if (Utils.ArithmeticRounding(num1 - (str3 == "Conforming" ? 1.5 : 2.5), 3) >= num2)
              {
                this.SetVal("3135", "does");
                break;
              }
              this.SetVal("3135", "does not");
              break;
            case "SecondLien":
              if (Utils.ArithmeticRounding(num1 - 3.5, 3) >= num2)
              {
                this.SetVal("3135", "does");
                break;
              }
              this.SetVal("3135", "does not");
              break;
            default:
              this.SetVal("3135", "");
              break;
          }
        }
      }
    }

    private void calculateSecondAppraisalRequired(string id, string val)
    {
      string str = this.Val("3135");
      if (str == "")
      {
        this.SetVal("3856", "");
      }
      else
      {
        double num1 = this.FltVal("136");
        double num2 = this.FltVal("3854");
        if (num1 == 0.0 || num2 == 0.0)
        {
          this.SetVal("3856", "Does not");
        }
        else
        {
          double num3 = num2 != 0.0 ? Utils.ArithmeticRounding((num1 - num2) / num2, 5) : 0.0;
          DateTime date1 = Utils.ParseDate((object) this.Val("3853"));
          DateTime date2 = Utils.ParseDate((object) this.Val("3855"));
          if (date1 == DateTime.MinValue || date2 == DateTime.MinValue || date1 >= date2)
          {
            this.SetVal("3856", "Does not");
          }
          else
          {
            TimeSpan timeSpan = date2.Date.Subtract(date1.Date);
            if (str == "does" && (num3 > 0.1 && timeSpan.Days <= 90 || num3 > 0.2 && timeSpan.Days > 90 && timeSpan.Days < 181))
              this.SetVal("3856", "Does");
            else
              this.SetVal("3856", "Does not");
          }
        }
      }
    }

    private void calculateOthers(string id, string val)
    {
      string str = this.Val("14");
      if (str == "FL" || str == "CO" || str == "CA" || this.loan.IsTemplate)
      {
        if (str == "FL" || this.loan.IsTemplate)
          this.SetCurrentNum("DISCLOSURE.X157", this.FltVal("DISCLOSURE.X464") + this.FltVal("DISCLOSURE.X466") + this.FltVal("DISCLOSURE.X468") + this.FltVal("DISCLOSURE.X470"));
        if (str == "CO" || this.loan.IsTemplate)
        {
          if (str == "CO")
          {
            this.SetCurrentNum("DISCLOSURE.X228", this.FltVal("DISCLOSURE.X226") + this.FltVal("DISCLOSURE.X227"));
            this.SetCurrentNum("DISCLOSURE.X369", this.FltVal("DISCLOSURE.X367") + this.FltVal("DISCLOSURE.X368"));
          }
          if (this.Val("DISCLOSURE.X208") != "Other")
            this.SetVal("DISCLOSURE.X224", "");
        }
        if (str == "CA" || this.loan.IsTemplate)
        {
          if (this.Val("DISCLOSURE.X222") != "LenderAgree")
          {
            this.SetVal("DISCLOSURE.X599", "");
            this.SetVal("DISCLOSURE.X600", "");
          }
          else if (this.Val("DISCLOSURE.X599") != "Other")
            this.SetVal("DISCLOSURE.X600", "");
        }
        if (this.Val("DISCLOSURE.X478") != "Y")
          this.SetVal("DISCLOSURE.X479", "");
        if (this.Val("DISCLOSURE.X480") != "Y")
          this.SetVal("DISCLOSURE.X365", "");
        if (this.Val("DISCLOSURE.X483") != "Y" && str != "WI")
          this.SetVal("DISCLOSURE.X484", "");
        if (this.Val("DISCLOSURE.X491") != "Y")
          this.SetVal("DISCLOSURE.X492", "");
      }
      if ((str == "WI" || this.loan.IsTemplate) && this.Val("DISCLOSURE.X483") != "Y")
        this.SetVal("DISCLOSURE.X484", "");
      if ((str == "TX" || this.loan.IsTemplate) && this.Val("DISCLOSURE.X76") != "Y")
      {
        this.SetVal("DISCLOSURE.X77", "");
        this.SetVal("DISCLOSURE.X78", "");
      }
      if (str == "NJ" || this.loan.IsTemplate)
      {
        if (this.Val("DISCLOSURE.X528") != "Y")
        {
          for (int index = 529; index <= 540; ++index)
            this.SetVal("DISCLOSURE.X" + (object) index, "");
        }
        if (this.Val("DISCLOSURE.X526") != "Y")
          this.SetVal("DISCLOSURE.X527", "");
      }
      if (this.Val("DISCLOSURE.X109") != "Y")
      {
        this.SetVal("DISCLOSURE.X111", "");
        this.SetVal("DISCLOSURE.X112", "");
        this.SetVal("DISCLOSURE.X569", "");
      }
      if (this.Val("DISCLOSURE.X649") != "Y")
      {
        this.SetVal("DISCLOSURE.X651", "");
        this.loan.RemoveLock("DISCLOSURE.X651");
      }
      if (this.Val("DISCLOSURE.X650") != "Y")
      {
        this.SetVal("DISCLOSURE.X652", "");
        this.loan.RemoveLock("DISCLOSURE.X652");
      }
      if (this.Val("DISCLOSURE.X645") != "Y")
      {
        this.SetVal("DISCLOSURE.X647", "");
        this.loan.RemoveLock("DISCLOSURE.X647");
      }
      if (this.Val("DISCLOSURE.X646") != "Y")
      {
        this.SetVal("DISCLOSURE.X648", "");
        this.loan.RemoveLock("DISCLOSURE.X648");
      }
      if (this.Val("DISCLOSURE.X97") != "Y")
        this.SetVal("DISCLOSURE.X113", "");
      if (this.Val("DISCLOSURE.X114") != "Y")
      {
        this.SetVal("DISCLOSURE.X115", "");
        this.SetVal("DISCLOSURE.X98", "");
        this.SetVal("DISCLOSURE.X116", "");
      }
      if (this.Val("DISCLOSURE.X117") != "Y")
      {
        this.SetVal("DISCLOSURE.X118", "");
        this.SetVal("DISCLOSURE.X119", "");
      }
      if (this.Val("DISCLOSURE.X645") != "Y")
      {
        this.SetVal("DISCLOSURE.X647", "");
        this.loan.RemoveLock("DISCLOSURE.X647");
      }
      if (this.Val("DISCLOSURE.X646") != "Y")
      {
        this.SetVal("DISCLOSURE.X648", "");
        this.loan.RemoveLock("DISCLOSURE.X648");
      }
      if (this.Val("DISCLOSURE.X619") != "Alternative2")
        this.SetVal("DISCLOSURE.X238", "");
      if (id == "DISCLOSURE.X109" && this.Val("DISCLOSURE.X109") != "Y" || str != "VT" && str != "NY" && str != "")
        this.SetVal("DISCLOSURE.X110", "");
      if (this.Val("DISCLOSURE.X446") != "Y")
      {
        this.SetVal("DISCLOSURE.X661", "");
        this.SetVal("DISCLOSURE.X684", "");
        this.SetVal("DISCLOSURE.X447", "");
      }
      if (this.Val("DISCLOSURE.X448") != "Y")
      {
        this.SetVal("DISCLOSURE.X662", "");
        this.SetVal("DISCLOSURE.X449", "");
      }
      if (this.Val("DISCLOSURE.X450") != "Y")
      {
        this.SetVal("DISCLOSURE.X663", "");
        this.SetVal("DISCLOSURE.X451", "");
      }
      if (this.Val("DISCLOSURE.X452") != "Y")
      {
        this.SetVal("DISCLOSURE.X664", "");
        this.SetVal("DISCLOSURE.X453", "");
      }
      if (this.Val("DISCLOSURE.X454") != "Y")
      {
        this.SetVal("DISCLOSURE.X665", "");
        this.SetVal("DISCLOSURE.X666", "");
        this.SetVal("DISCLOSURE.X455", "");
      }
      else if (this.FltVal("HUD24") > 0.0)
        this.SetVal("DISCLOSURE.X671", "Required");
      else
        this.SetVal("DISCLOSURE.X671", "Not Required");
      if (this.Val("DISCLOSURE.X456") != "Y")
      {
        this.SetVal("DISCLOSURE.X667", "");
        this.SetVal("DISCLOSURE.X668", "");
        this.SetVal("DISCLOSURE.X457", "");
      }
      else
        this.SetVal("DISCLOSURE.X672", this.Val("NEWHUD.X357") == "Y" ? "Required" : "Not Required");
      if (this.Val("DISCLOSURE.X458") != "Y")
      {
        this.SetVal("DISCLOSURE.X669", "");
        this.SetVal("DISCLOSURE.X670", "");
        this.SetVal("DISCLOSURE.X459", "");
      }
      else
        this.SetVal("DISCLOSURE.X670", this.Val("NEWHUD.X673"));
      if (str == "MT" && this.Val("DISCLOSURE.X374") != "Y")
      {
        this.SetVal("DISCLOSURE.X529", "");
        this.SetVal("DISCLOSURE.X530", "");
        this.SetVal("DISCLOSURE.X532", "");
        this.SetVal("DISCLOSURE.X533", "");
        this.SetVal("DISCLOSURE.X535", "");
        this.SetVal("DISCLOSURE.X536", "");
      }
      if (this.Val("DISCLOSURE.X338") != "Y")
        this.SetVal("DISCLOSURE.X887", "");
      if (this.Val("DISCLOSURE.X892") != "Y")
      {
        this.SetVal("DISCLOSURE.X893", "");
        this.SetVal("DISCLOSURE.X894", "");
        this.SetVal("DISCLOSURE.X895", "");
      }
      if (this.Val("DISCLOSURE.X896") != "Y")
        this.SetVal("DISCLOSURE.X897", "");
      if (this.Val("DISCLOSURE.X924") != "Y")
      {
        this.SetVal("DISCLOSURE.X925", "");
        this.SetVal("DISCLOSURE.X926", "");
        this.SetVal("DISCLOSURE.X927", "");
      }
      if (this.Val("DISCLOSURE.X927") != "Decreases Interest Rate")
        this.SetVal("DISCLOSURE.X928", "");
      if (this.Val("DISCLOSURE.X927") != "Increases Interest Rate")
        this.SetVal("DISCLOSURE.X929", "");
      if (this.Val("DISCLOSURE.X930") != "Y")
      {
        this.SetVal("DISCLOSURE.X931", "");
        this.SetVal("DISCLOSURE.X932", "");
        this.SetVal("DISCLOSURE.X933", "");
      }
      if (this.Val("DISCLOSURE.X933") != "Decreases Interest Rate")
        this.SetVal("DISCLOSURE.X934", "");
      if (this.Val("DISCLOSURE.X933") != "Increases Interest Rate")
        this.SetVal("DISCLOSURE.X935", "");
      if (this.Val("DISCLOSURE.X936") != "Y")
      {
        this.SetVal("DISCLOSURE.X937", "");
        this.SetVal("DISCLOSURE.X938", "");
      }
      if (this.Val("DISCLOSURE.X939") != "Y")
      {
        this.SetVal("DISCLOSURE.X940", "");
        this.SetVal("DISCLOSURE.X941", "");
        this.SetVal("DISCLOSURE.X942", "");
      }
      if (str == "NY")
      {
        if (id == "DISCLOSURE.X564" && val != string.Empty)
          this.SetVal("DISCLOSURE.X526", "");
        else if (id == "DISCLOSURE.X526" && val == "Y")
          this.SetVal("DISCLOSURE.X564", "");
        if (this.Val("DISCLOSURE.X564") != "Prior to scheduled loan closing")
          this.SetVal("DISCLOSURE.X565", "");
        if (this.Val("DISCLOSURE.X570") != "Y")
          this.SetVal("DISCLOSURE.X571", "");
        if (this.Val("DISCLOSURE.X572") != "Y")
        {
          for (int index = 573; index <= 577; ++index)
            this.SetVal("DISCLOSURE.X" + (object) index, "");
        }
        if (this.Val("DISCLOSURE.X570") != "Y")
          this.SetVal("DISCLOSURE.X121", "");
        if (this.Val("DISCLOSURE.X117") != "Y" && this.Val("DISCLOSURE.X570") != "Y")
          this.SetVal("DISCLOSURE.X120", "");
        if (this.Val("DISCLOSURE.X575") != "Y")
          this.SetVal("DISCLOSURE.X577", "");
      }
      if (id == "DISCLOSURE.X157" || id == "DISCLOSURE.X158" || id == "DISCLOSURE.X160" || id == "DISCLOSURE.X162")
        this.SetCurrentNum("DISCLOSURE.X163", this.FltVal("DISCLOSURE.X157") + this.FltVal("DISCLOSURE.X158") + this.FltVal("DISCLOSURE.X160") + this.FltVal("DISCLOSURE.X162"));
      else if (id == "1663" && !this.UseNewGFEHUD && !this.UseNew2015GFEHUD)
      {
        if (this.ToDouble(this.Val("1847")) != 0.0)
        {
          this.SetVal("1847", "");
          this.SetVal("NEWHUD.X734", "");
        }
        this.SetCurrentNum("GFE824", this.ToDouble(val));
      }
      else if (id == "1665")
      {
        if (this.ToDouble(this.Val("1848")) != 0.0)
          this.SetVal("1848", "");
        this.SetCurrentNum("GFE825", this.ToDouble(val));
      }
      else if (id == "1847" || id == "NEWHUD.X734")
      {
        if (val == "" && this.ToDouble(this.Val("1663")) != 0.0)
        {
          if (this.Val("NEWHUD.X1139") != "Y")
            this.SetVal("1663", "");
          this.SetVal("GFE824", "");
        }
      }
      else if (id == "1848" && val == "" && this.ToDouble(this.Val("1665")) != 0.0)
      {
        this.SetVal("1665", "");
        this.SetVal("GFE825", "");
      }
      double num1 = this.FltVal("1109");
      if (this.Val("1172") == "FHA" || this.Val("1172") == "VA" || this.Val("1172") == "FarmersHomeAdministration")
        num1 = this.FltVal("2");
      double num2 = this.ToDouble(this.Val("1847"));
      if (num2 != 0.0)
      {
        double num3 = num2 / 100.0 * (this.UseNewGFEHUD || this.UseNew2015GFEHUD ? this.FltVal("2") : num1);
        if (this.UseNewGFEHUD || this.UseNew2015GFEHUD)
          num3 += this.FltVal("NEWHUD.X734");
        this.SetVal("1663", num3.ToString("N2"));
        if (this.Val("NEWHUD.X715") == "Include Origination Credit")
          this.SetVal("NEWHUD.X749", "Lender");
        this.SetCurrentNum("GFE824", num3);
      }
      else if ((this.UseNewGFEHUD || this.UseNew2015GFEHUD) && this.FltVal("NEWHUD.X734") != 0.0)
      {
        this.SetVal("1663", this.FltVal("NEWHUD.X734").ToString("N2"));
        if (this.Val("NEWHUD.X715") == "Include Origination Credit")
          this.SetVal("NEWHUD.X749", "Lender");
      }
      else if (id == "NEWHUD.X1139" && val != "Y")
      {
        this.SetVal("1663", "");
        this.SetVal("NEWHUD.X715", "");
        this.SetVal("NEWHUD.X714", "");
        this.SetVal("NEWHUD.X14", "");
        this.SetVal("NEWHUD.X713", "");
      }
      if (id == "NEWHUD.X1139" && val == "Y")
      {
        this.SetVal("NEWHUD.X749", "");
        this.SetVal("NEWHUD.X627", "");
        this.SetVal("NEWHUD.X353", "");
        this.SetVal("NEWHUD.X820", "");
      }
      if ((this.UseNewGFEHUD || this.UseNew2015GFEHUD) && this.Val("NEWHUD.X1139") == "Y")
        this.calObjs.NewHudCal.CalcLOCompensationTool(id, val);
      double num4 = this.ToDouble(this.Val("1848"));
      if (num4 != 0.0)
      {
        double num5 = num4 / 100.0 * num1;
        this.SetVal("1665", num5.ToString("N2"));
        this.SetCurrentNum("GFE825", num5);
      }
      this.SetCurrentNum("FLGFE.X77", this.FltVal("1663") + this.FltVal("1665"));
    }

    public void calculateDISCLOSUREX694(string id, string val)
    {
      this.SetCurrentNum("DISCLOSURE.X964", Utils.ArithmeticRounding(this.FltVal("2") * this.FltVal("DISCLOSURE.X963") / 100.0, 2));
      double num = 0.0;
      for (int index = 965; index <= 971; ++index)
        num += this.FltVal("DISCLOSURE.X" + (object) index);
      this.SetCurrentNum("DISCLOSURE.X972", num);
    }

    public double CalculateInsurance(string targetID)
    {
      string id1 = string.Empty;
      string id2 = string.Empty;
      string id3 = string.Empty;
      switch (targetID)
      {
        case "337":
        case "RE88395.X43":
          id1 = "1806";
          id2 = "1807";
          id3 = "1209";
          break;
        case "230":
          id1 = "1750";
          id2 = "1322";
          break;
        case "232":
          id1 = "1757";
          id2 = "1199";
          if (this.FltVal("1766") != 0.0)
            return this.FltVal("1766");
          break;
        case "231":
          id1 = "1751";
          id2 = "1752";
          break;
      }
      string str1 = this.Val(id1);
      if (id1 == "1750" || id1 == "1751")
      {
        string str2 = this.Val("19");
        if (str2 == "ConstructionOnly" || str2 == "ConstructionToPermanent")
          str1 = !(this.Val("1964") == "Y") ? "As Completed Appraised Value" : "As Completed Purchase Price";
      }
      int num1 = 0;
      if (id3 != string.Empty)
        num1 = this.IntVal(id3);
      double num2 = this.FltVal(id2);
      double num3;
      switch (str1)
      {
        case "Purchase Price":
          num3 = this.FltVal("136");
          break;
        case "Appraisal Value":
          num3 = this.FltVal("356");
          break;
        case "Base Loan Amount":
          num3 = this.FltVal("1109");
          break;
        case "As Completed Purchase Price":
          num3 = this.FltVal("CONST.X58");
          break;
        case "As Completed Appraised Value":
          num3 = this.FltVal("CONST.X59");
          break;
        default:
          num3 = this.FltVal("2");
          break;
      }
      double val;
      switch (num1)
      {
        case 6:
          val = Utils.ArithmeticRounding(num2 / 100.0 * num3 / 2.0, 2);
          break;
        case 12:
          val = Utils.ArithmeticRounding(num2 / 100.0 * num3, 2);
          break;
        default:
          val = Utils.ArithmeticRounding(num2 / 1200.0 * num3, 2);
          if (num1 > 0)
          {
            val *= (double) num1;
            break;
          }
          break;
      }
      return this.EMRounding(val, 2);
    }

    internal void CopyMLDSToGFE() => this.CopyMLDSToGFE((DataTemplate) null);

    internal void CopyMLDSToGFE(DataTemplate dataTemplate)
    {
      List<string> stringList = new List<string>((IEnumerable<string>) new string[11]
      {
        "SYS.X361",
        "SYS.X363",
        "SYS.X365",
        "SYS.X374",
        "SYS.X376",
        "SYS.X378",
        "SYS.X380",
        "SYS.X382",
        "SYS.X384",
        "SYS.X386",
        "SYS.X387"
      });
      for (int index = 0; index < CalculationBase.BorrowerFields.Length; ++index)
      {
        double num1 = this.FltVal("RE88395.X" + CalculationBase.PaidToBroker[index]);
        double num2 = this.FltVal("RE88395.X" + CalculationBase.PaidToOthers[index]);
        string str = this.Val("SYS.X" + CalculationBase.PaidByChecks[index]);
        if ((dataTemplate == null || !this.UseNewGFEHUD && !this.UseNew2015GFEHUD || !stringList.Contains("SYS.X" + CalculationBase.PaidByChecks[index])) && (dataTemplate == null || !this.UseNewGFEHUD && !this.UseNew2015GFEHUD || (!(CalculationBase.PaidByChecks[index] == "357") || !(dataTemplate.GetField("1637") != string.Empty)) && !(dataTemplate.GetField("647") != string.Empty) && !(dataTemplate.GetField("593") != string.Empty) && (!(CalculationBase.PaidByChecks[index] == "359") || !(dataTemplate.GetField("1638") != string.Empty)) && !(dataTemplate.GetField("648") != string.Empty) && !(dataTemplate.GetField("594") != string.Empty)))
        {
          if (str == "Seller")
          {
            this.SetCurrentNum(CalculationBase.SellerFields[index], num1 + num2);
            this.SetVal(CalculationBase.BorrowerFields[index], "");
          }
          else
          {
            this.SetCurrentNum(CalculationBase.BorrowerFields[index], num1 + num2);
            this.SetVal(CalculationBase.SellerFields[index], "");
          }
          if (num1 != 0.0 && num2 == 0.0)
            this.SetVal("SYS.X" + CalculationBase.PTBChecks[index], "Broker");
        }
      }
      if (dataTemplate != null)
      {
        for (int index = 0; index < CalculationBase.SellerFields.Length; ++index)
        {
          if (dataTemplate.GetField(CalculationBase.SellerFields[index]) != string.Empty)
            this.SetVal(CalculationBase.SellerFields[index], dataTemplate.GetField(CalculationBase.SellerFields[index]));
        }
      }
      this.calculateGFEFees((string) null, (string) null);
    }

    internal void SyncImportedMLDS()
    {
      for (int index = 0; index < CalculationBase.BorrowerFields.Length; ++index)
      {
        double num1 = this.FltVal("RE88395.X" + CalculationBase.PaidToBroker[index]);
        double num2 = this.FltVal("RE88395.X" + CalculationBase.PaidToOthers[index]);
        double num3 = this.FltVal(CalculationBase.BorrowerFields[index]);
        double num4 = this.FltVal(CalculationBase.SellerFields[index]);
        if (num1 != 0.0 && num2 == 0.0)
          this.SetVal("SYS.X" + CalculationBase.PTBChecks[index], "Broker");
        else
          this.SetVal("SYS.X" + CalculationBase.PTBChecks[index], "");
        if (num4 > 0.0 && num3 == 0.0)
          this.SetVal("SYS.X" + CalculationBase.PaidByChecks[index], "Seller");
      }
      this.calObjs.MLDSCal.CalcCompensations((string) null, (string) null);
      this.calObjs.GFECal.CalcClosingCosts((string) null, (string) null);
    }

    private void syncMLDS(string id, string val)
    {
      if (CalculationBase.SyncFields.Count == 0)
        CalculationBase.CreateSyncFieldList();
      if (!CalculationBase.SyncFields.ContainsKey((object) id))
        return;
      string[] strArray = CalculationBase.SyncFields[(object) id].ToString().Split('|');
      string str = "";
      switch (strArray[0])
      {
        case "B":
        case "S":
          str = this.Val(strArray[5]);
          if (this.Val(strArray[6]) == "Broker")
          {
            this.SetVal(strArray[3], "");
            this.SetCurrentNum(strArray[4], this.FltVal(strArray[2]) + this.FltVal(strArray[1]));
          }
          else
          {
            this.SetCurrentNum(strArray[3], this.FltVal(strArray[2]) + this.FltVal(strArray[1]));
            this.SetVal(strArray[4], "");
          }
          if (this.FltVal(strArray[1]) == 0.0 && this.FltVal(strArray[2]) != 0.0)
          {
            this.SetVal(strArray[5], "Seller");
            break;
          }
          if ((this.FltVal(strArray[1]) != 0.0 && this.FltVal(strArray[2]) != 0.0 || this.FltVal(strArray[2]) == 0.0) && this.Val(strArray[5]) == "Seller")
          {
            this.SetVal(strArray[5], "");
            break;
          }
          break;
        case "PTBCheck":
          if (this.Val(id) == "Broker")
          {
            this.SetVal(strArray[3], "");
            this.SetCurrentNum(strArray[4], this.FltVal(strArray[1]) + this.FltVal(strArray[2]));
            break;
          }
          this.SetVal(strArray[4], "");
          this.SetCurrentNum(strArray[3], this.FltVal(strArray[1]) + this.FltVal(strArray[2]));
          break;
      }
      if (val == null)
        return;
      this.calObjs.MLDSCal.CalcCompensations(id, val);
    }

    internal void CalculateFunder(string id, string val)
    {
      if (this.UseNewGFEHUD || this.UseNew2015GFEHUD)
      {
        this.calObjs.NewHudCal.CalculateFunder(id, val);
      }
      else
      {
        double num1 = 0.0;
        string empty1 = string.Empty;
        string empty2 = string.Empty;
        string empty3 = string.Empty;
        string val1 = string.Empty;
        string val2 = string.Empty;
        for (int index1 = 0; index1 < GFEItemCollection.GFEItems.Count; ++index1)
        {
          GFEItem gfeItem = GFEItemCollection.GFEItems[index1];
          if (gfeItem.LineNumber >= 100)
          {
            string str1 = !(gfeItem.PaidByFieldID != "") ? string.Empty : this.Val(gfeItem.PaidByFieldID);
            if (gfeItem.LineNumber < 520)
            {
              if (str1 != "Credit" && str1 != "Debt")
                continue;
            }
            else if (gfeItem.POCFieldID != string.Empty && this.Val(gfeItem.POCFieldID) == "Y")
              continue;
            for (int index2 = 1; index2 <= 2; ++index2)
            {
              double num2 = 0.0;
              if (gfeItem.LineNumber >= 520 || index2 != 2)
              {
                if (gfeItem.LineNumber != 824 && gfeItem.LineNumber != 825 || index2 < 2)
                {
                  string str2 = gfeItem.Description.Length > 4 ? gfeItem.Description : this.Val(gfeItem.Description);
                  string str3 = !(gfeItem.PTBFieldID != "") ? (gfeItem.LineNumber == 824 || gfeItem.LineNumber == 825 ? "Broker" : string.Empty) : this.Val(gfeItem.PTBFieldID);
                  if (str3 != "Broker")
                    str3 = !(gfeItem.BorrowerFieldID == "558") ? "Lender/Other" : string.Empty;
                  if (index2 == 1)
                  {
                    if (str1 == "" || gfeItem.LineNumber <= 520)
                      str1 = !(gfeItem.BorrowerFieldID == "558") ? "Borrower" : "";
                    if (gfeItem.BorrowerFieldID != string.Empty)
                      num2 = this.FltVal(gfeItem.BorrowerFieldID);
                    if (num2 != 0.0)
                    {
                      if (val2 != string.Empty)
                        val2 += "\r\n";
                      val2 = val2 + gfeItem.LineNumber.ToString() + ".\t" + str2 + "\t\t" + str1 + "\t\t" + str3 + "\t\t$" + num2.ToString("N2");
                    }
                    if (gfeItem.CheckBorrowerFieldID != string.Empty && this.Val(gfeItem.CheckBorrowerFieldID) != "Y")
                      continue;
                  }
                  else
                  {
                    str1 = "Seller";
                    if (gfeItem.SellerFieldID != string.Empty)
                      num2 = this.FltVal(gfeItem.SellerFieldID);
                    if (num2 != 0.0)
                    {
                      if (val2 != string.Empty)
                        val2 += "\r\n";
                      val2 = val2 + gfeItem.LineNumber.ToString() + ".\t" + str2 + "\t\t" + str1 + "\t\t" + str3 + "\t\t$" + num2.ToString("N2");
                    }
                    if (gfeItem.CheckSellerFieldID != string.Empty && this.Val(gfeItem.CheckSellerFieldID) != "Y")
                      continue;
                  }
                  if (num2 != 0.0)
                  {
                    if (gfeItem.LineNumber < 520 && gfeItem.PaidByFieldID != string.Empty && this.loan.GetField(gfeItem.PaidByFieldID) == "Credit")
                      num2 *= -1.0;
                    num1 += num2;
                    if (val1 != string.Empty)
                      val1 += "\r\n";
                    val1 = val1 + gfeItem.LineNumber.ToString() + ".\t" + str2 + "\t\t" + str1 + "\t\t" + str3 + "\t\t$" + num2.ToString("N2");
                  }
                }
                else
                  break;
              }
            }
          }
        }
        this.SetVal("2971", val1);
        this.SetVal("2972", val2);
        this.SetCurrentNum("1989", num1);
        this.SetCurrentNum("1990", (this.Val("1172") == "HELOC" ? this.FltVal("1888") : this.FltVal("2")) - num1 + this.FltVal("2005"));
      }
    }

    public void PopulateFeeList(string id, string val) => this.PopulateFeeList(id, false);

    public void PopulateFeeList(bool recalculated)
    {
      this.PopulateFeeList((string) null, recalculated);
    }

    public void PopulateFeeList(string id, bool recalculated)
    {
      this._gfeCalculationServant.PopulateFeeList(id, recalculated);
    }

    internal void calcBrokerLenderFeeTotals(string id, string val)
    {
      if (!this.UseNewGFEHUD && !this.UseNew2015GFEHUD)
        return;
      Decimal num1 = 0M;
      Decimal num2 = 0M;
      foreach (GFEItem gfeItem in GFEItemCollection.GFEItems2010)
      {
        if (gfeItem.LineNumber == 801)
        {
          string simpleField = this.loan.GetSimpleField(gfeItem.PTBFieldID);
          Decimal num3 = Utils.ParseDecimal((object) this.loan.GetSimpleField(gfeItem.BorrowerFieldID)) + Utils.ParseDecimal((object) this.loan.GetSimpleField(gfeItem.SellerFieldID));
          switch (simpleField)
          {
            case "Broker":
              num1 += num3;
              continue;
            case "Lender":
            case "":
              num2 += num3;
              continue;
            default:
              continue;
          }
        }
      }
      if (this.Val("NEWHUD.X1139") == "Y")
      {
        foreach (GFEItem gfeItem in GFEItemCollection.GFEItems2010)
        {
          if (gfeItem.LineNumber == 802 && gfeItem.ComponentID != "" && (!this.excludeOriginationCredit || !(gfeItem.ComponentID == "b")))
          {
            string str1 = this.Val(gfeItem.PaidByFieldID);
            string str2 = this.Val(gfeItem.PTBFieldID);
            Decimal num4 = Utils.ParseDecimal((object) this.Val(gfeItem.BorrowerFieldID));
            Decimal num5 = 0M;
            if (gfeItem.SellerFieldID != "")
              num5 = Utils.ParseDecimal((object) this.Val(gfeItem.SellerFieldID));
            if (gfeItem.POCFieldID == "" && (str1 == "Lender" || str1 == ""))
              num2 -= num4 + num5;
            else if (gfeItem.POCFieldID != "" && (str2 == "Lender" || str2 == ""))
              num2 += num4 + num5;
          }
        }
      }
      else
      {
        string simpleField = this.loan.GetSimpleField("NEWHUD.X627");
        Decimal num6 = this.excludeOriginationCredit ? 0M : Utils.ParseDecimal((object) this.loan.GetSimpleField("1663"));
        Decimal num7 = Utils.ParseDecimal((object) this.loan.GetSimpleField("NEWHUD.X15")) + Utils.ParseDecimal((object) this.loan.GetSimpleField("NEWHUD.X788"));
        if (simpleField == "Lender" || simpleField == "")
          num2 += num7 - num6;
      }
      this.SetVal("3310", num1 == 0M ? "" : num1.ToString());
      this.SetVal("3311", num2 == 0M ? "" : num2.ToString());
    }

    public void UpdateValueExistingJuniorLien(string id, string val)
    {
      if (this.Val("DISCLOSURE.X1080") == "N")
        return;
      if (this.Val("DISCLOSURE.X1076") != "Y" || this.Val("DISCLOSURE.X1077") != "Y" || this.Val("DISCLOSURE.X1078") != "Y" || this.Val("DISCLOSURE.X1079") != "Y")
        this.SetVal("DISCLOSURE.X1080", "N");
      if (this.Val("420") != "FirstLien")
        this.SetVal("DISCLOSURE.X1080", "N");
      if (this.Val("19") != "NoCash-Out Refinance")
        this.SetVal("DISCLOSURE.X1080", "N");
      if (!(this.Val("16") != "") || Convert.ToInt32(this.Val("16")) <= 4 && Convert.ToInt32(this.Val("16")) >= 1)
        return;
      this.SetVal("DISCLOSURE.X1080", "N");
    }
  }
}
