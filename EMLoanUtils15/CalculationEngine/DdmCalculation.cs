// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.CalculationEngine.DdmCalculation
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.CalculationEngine
{
  internal class DdmCalculation : CalculationBase
  {
    private const string className = "DdmCalculation�";
    private static readonly string sw = Tracing.SwDataEngine;
    private SessionObjects sessionObjects;
    private DdmCalculation.CalcRunner calcRfdDetails;
    private DdmCalculation.CalcRunner calcRfdCommon;
    private DdmCalculation.CalcRunner calcCsfDetails;
    private DdmCalculation.CalcRunner calcMiUsdaDialog;
    private DdmCalculation.CalcRunner clearMIFieldsByDDM;
    private string updatedFieldIDsByDDM = "";

    internal DdmCalculation(SessionObjects sessionObjects, LoanData l, EllieMae.EMLite.CalculationEngine.CalculationObjects calObjs)
      : base(l, calObjs)
    {
      this.sessionObjects = sessionObjects;
      this.constructCalcs();
    }

    private void constructCalcs()
    {
      this.calcRfdDetails = new DdmCalculation.CalcRunner();
      this.calcRfdDetails.Add(this.calcRecordingFees());
      this.calcRfdDetails.Add(this.calcStateTax());
      this.calcRfdDetails.Add(this.calcLocalTax());
      this.calcRfdCommon = new DdmCalculation.CalcRunner();
      this.calcRfdCommon.Add(this.calcMLDSFromRecordingDialog());
      this.calcCsfDetails = new DdmCalculation.CalcRunner();
      this.calcCsfDetails.Add(this.calcCityFeeDialog());
      this.calcCsfDetails.Add(this.calcStateFeeDialog());
      this.calcMiUsdaDialog = new DdmCalculation.CalcRunner();
      this.calcMiUsdaDialog.Add(this.calcMiMonthlyPaymentLevel1());
      this.clearMIFieldsByDDM = new DdmCalculation.CalcRunner();
      this.calcMiUsdaDialog.Add(this.clearMIPFields());
    }

    internal void CalcGuiDependentLogicForDDM(string id, string val)
    {
      this.CalcRecordingFeesDialogForDDM(id, val);
      this.CalcCityStateFeeDialog(id, val);
      this.CalcMiUsdaMissingDetails(id, val);
      this.ClearMIFieldsByDDM(id, val);
    }

    internal void CalcRecordingFeesDialogForDDM(string id, string val)
    {
      this.calcRfdDetails.Execute(id, val, true);
      if (!this.calcRfdDetails.AnyCalcExecuted)
        return;
      this.calcRfdCommon.Execute(id, val, true);
    }

    internal void CalcCityStateFeeDialog(string id, string val)
    {
      this.calcCsfDetails.Execute(id, val, true);
    }

    internal void CalcMiUsdaMissingDetails(string id, string val)
    {
      this.calcMiUsdaDialog.Execute(id, val, true);
    }

    internal void ClearMIFieldsByDDM(string id, string val)
    {
      this.clearMIFieldsByDDM.Execute(id, val, true);
    }

    internal string UpdatedFieldIDsByDDM
    {
      set => this.updatedFieldIDsByDDM = value;
    }

    private bool isFieldUpdated(string[] fieldsUpdatedByDDM)
    {
      if (this.updatedFieldIDsByDDM == "")
        return false;
      for (int index = 0; index < fieldsUpdatedByDDM.Length; ++index)
      {
        if (this.updatedFieldIDsByDDM.IndexOf("|" + fieldsUpdatedByDDM[index] + "|") > -1)
          return true;
      }
      return false;
    }

    private DdmCalculation.CalcFunc calcRecordingFees()
    {
      DdmCalculation.CalcFunc calcFunc = new DdmCalculation.CalcFunc((Func<string, string, bool>) ((id, val) =>
      {
        string field1 = this.loan.GetField("2402");
        string field2 = this.loan.GetField("2403");
        string field3 = this.loan.GetField("2404");
        bool flag1 = this.isFieldUpdated(new string[3]
        {
          "2402",
          "2403",
          "2404"
        });
        bool flag2 = flag1 && (!string.IsNullOrEmpty(field1) || !string.IsNullOrEmpty(field2) || !string.IsNullOrEmpty(field3));
        if (flag1)
        {
          if (!this.isFieldUpdated(new string[1]{ "1636" }) && (!string.IsNullOrEmpty(field1) || !string.IsNullOrEmpty(field2) || !string.IsNullOrEmpty(field3)))
            this.SetVal("1636", this.getDescription(field1, field2, field3));
        }
        double num = Utils.ParseDouble((object) field1) + Utils.ParseDouble((object) field2) + Utils.ParseDouble((object) field3) - Utils.ParseDouble((object) this.loan.GetField("587"));
        if (flag2)
          this.SetVal("390", num.ToString("N2"));
        this.loan.Calculator.FormCalculation("390", (string) null, (string) null);
        this.canSyncMldsPaidToOthers();
        if (this.canSyncGFE())
          this.loan.Calculator.CopyGFEToMLDS("390");
        this.loan.Calculator.CopyHUD2010ToGFE2010("2402", false);
        this.loan.Calculator.CopyHUD2010ToGFE2010("390", false);
        return true;
      }));
      calcFunc.AddTriggerField("2402", "2403", "2404");
      return calcFunc;
    }

    private DdmCalculation.CalcFunc calcStateTax()
    {
      DdmCalculation.CalcFunc calcFunc = new DdmCalculation.CalcFunc((Func<string, string, bool>) ((id, val) =>
      {
        string field1 = this.loan.GetField("2407");
        string field2 = this.loan.GetField("2408");
        bool flag1 = this.isFieldUpdated(new string[2]
        {
          "2407",
          "2408"
        });
        bool flag2 = flag1 && (!string.IsNullOrEmpty(field1) || !string.IsNullOrEmpty(field2));
        if (flag1)
        {
          if (!this.isFieldUpdated(new string[1]{ "1638" }) && (!string.IsNullOrEmpty(field1) || !string.IsNullOrEmpty(field2)))
            this.SetVal("1638", this.getDescription(field1, field2));
        }
        if (flag2 & flag1)
        {
          if (!this.isFieldUpdated(new string[1]
          {
            "DDM:SYSTEMTABLE:1638"
          }) && (!string.IsNullOrEmpty(field1) || !string.IsNullOrEmpty(field2)))
            this.SetVal("648", (Utils.ParseDouble((object) field1) + Utils.ParseDouble((object) field2) - Utils.ParseDouble((object) this.loan.GetField("594"))).ToString("N2"));
        }
        this.loan.Calculator.FormCalculation("648", (string) null, (string) null);
        this.loan.Calculator.CopyHUD2010ToGFE2010("2407", false);
        this.loan.Calculator.CopyHUD2010ToGFE2010("648", false);
        return true;
      }));
      calcFunc.AddTriggerField("2407", "2408");
      return calcFunc;
    }

    private DdmCalculation.CalcFunc calcLocalTax()
    {
      DdmCalculation.CalcFunc calcFunc = new DdmCalculation.CalcFunc((Func<string, string, bool>) ((id, val) =>
      {
        string field1 = this.loan.GetField("2405");
        string field2 = this.loan.GetField("2406");
        bool flag1 = this.isFieldUpdated(new string[2]
        {
          "2405",
          "2406"
        });
        bool flag2 = flag1 && (!string.IsNullOrEmpty(field1) || !string.IsNullOrEmpty(field2));
        if (flag1)
        {
          if (!this.isFieldUpdated(new string[1]{ "1637" }) && (!string.IsNullOrEmpty(field1) || !string.IsNullOrEmpty(field2)))
            this.SetVal("1637", this.getDescription(field1, field2));
        }
        if (flag2 & flag1)
        {
          if (!this.isFieldUpdated(new string[1]
          {
            "DDM:SYSTEMTABLE:1637"
          }) && (!string.IsNullOrEmpty(field1) || !string.IsNullOrEmpty(field2)))
            this.SetVal("647", (Utils.ParseDouble((object) field1) + Utils.ParseDouble((object) field2) - Utils.ParseDouble((object) this.loan.GetField("593"))).ToString("N2"));
        }
        this.loan.Calculator.FormCalculation("647", (string) null, (string) null);
        if (this.canSyncGFE())
          this.loan.Calculator.CopyGFEToMLDS("647");
        this.loan.Calculator.CopyHUD2010ToGFE2010("2405", false);
        this.loan.Calculator.CopyHUD2010ToGFE2010("647", false);
        return true;
      }));
      calcFunc.AddTriggerField("2405", "2406");
      return calcFunc;
    }

    private DdmCalculation.CalcFunc calcMLDSFromRecordingDialog()
    {
      return new DdmCalculation.CalcFunc((Func<string, string, bool>) ((id, val) =>
      {
        this.canSyncMldsPaidToOthers();
        this.loan.Calculator.FormCalculation("REGZ", (string) null, (string) null);
        if (this.loan.Use2010RESPA)
          this.loan.Calculator.FormCalculation("REGZGFE_2010", (string) null, (string) null);
        return true;
      }));
    }

    private DdmCalculation.CalcFunc calcCityFeeDialog()
    {
      return new DdmCalculation.CalcFunc((Func<string, string, bool>) ((id, val) =>
      {
        if (!string.IsNullOrEmpty(this.loan.GetField("2405")) || !string.IsNullOrEmpty(this.loan.GetField("2406")) || string.IsNullOrEmpty(this.loan.GetField("647")))
          return false;
        this.canSyncMldsPaidToOthers();
        if (this.canSyncGFE())
          this.loan.Calculator.CopyGFEToMLDS("647");
        this.loan.Calculator.CopyHUD2010ToGFE2010("647", false);
        this.loan.Calculator.FormCalculation("REGZ", (string) null, (string) null);
        this.loan.Calculator.FormCalculation("SEC32", (string) null, (string) null);
        this.loan.Calculator.FormCalculation("HUD1PG2_2010", (string) null, (string) null);
        return true;
      }));
    }

    private DdmCalculation.CalcFunc calcStateFeeDialog()
    {
      return new DdmCalculation.CalcFunc((Func<string, string, bool>) ((id, val) =>
      {
        if (!string.IsNullOrEmpty(this.loan.GetField("2407")) || !string.IsNullOrEmpty(this.loan.GetField("2408")) || string.IsNullOrEmpty(this.loan.GetField("648")))
          return false;
        if (this.canSyncGFE())
          this.loan.Calculator.CopyHUD2010ToGFE2010("648", false);
        this.loan.Calculator.FormCalculation("REGZ", (string) null, (string) null);
        this.loan.Calculator.FormCalculation("SEC32", (string) null, (string) null);
        this.loan.Calculator.FormCalculation("HUD1PG2_2010", (string) null, (string) null);
        return true;
      }));
    }

    private DdmCalculation.CalcFunc calcMiMonthlyPaymentLevel1()
    {
      return new DdmCalculation.CalcFunc((Func<string, string, bool>) ((id, val) =>
      {
        if (this.loan.GetField("1172") == "FarmersHomeAdministration")
        {
          double num1 = !(this.loan.GetField("3566") == "FinancingAll") ? Utils.ParseDouble((object) this.loan.GetField("3564")) : Utils.ParseDouble((object) this.loan.GetField("3562"));
          double num2 = Utils.ArithmeticRounding(Utils.ParseDouble((object) this.loan.GetField("1199")) / 100.0 * num1 / 12.0, 2);
          this.SetVal("1766", num2 != 0.0 ? num2.ToString("N2") : "");
        }
        return true;
      }));
    }

    private DdmCalculation.CalcFunc clearMIPFields()
    {
      DdmCalculation.CalcFunc calcFunc = new DdmCalculation.CalcFunc((Func<string, string, bool>) ((id, val) =>
      {
        if (this.loan == null)
          return false;
        if (this.isFieldUpdated(new string[1]{ "3533" }) && this.loan.GetField("3533") == "Y")
        {
          this.loan.SetCurrentField("1107", "", true);
          this.loan.SetCurrentField("1826", "", true);
          this.loan.SetCurrentField("1760", "", true);
          this.loan.SetCurrentField("3531", "", true);
          this.loan.SetCurrentField("3532", "", true);
          this.loan.SetCurrentField("1198", "", true);
          this.loan.SetCurrentField("1199", "", true);
          this.loan.SetCurrentField("1200", "", true);
          this.loan.SetCurrentField("1201", "", true);
          this.loan.SetCurrentField("1205", "", true);
          this.loan.SetCurrentField("1765", "", true);
          this.loan.SetCurrentField("1766", "", true);
          this.loan.SetCurrentField("1770", "", true);
          this.loan.SetCurrentField("232", "", true);
          this.loan.SetCurrentField("NEWHUD.X1707", "", true);
        }
        else
        {
          if (!this.isFieldUpdated(new string[1]{ "3531" }) || !(this.loan.GetField("3531") == "Y"))
          {
            if (!this.isFieldUpdated(new string[1]{ "3532" }) || !(this.loan.GetField("3532") == "Y"))
              goto label_7;
          }
          this.loan.SetCurrentField("3533", "", true);
        }
label_7:
        if (this.isFieldUpdated(new string[1]{ "1199" }) && this.loan.GetField("1199") == "")
        {
          this.loan.SetCurrentField("1766", "", true);
          this.loan.SetCurrentField("232", "", true);
          this.loan.SetCurrentField("NEWHUD.X1707", "", true);
        }
        return true;
      }));
      calcFunc.AddTriggerField("3533", "3531", "3532", "1199");
      return calcFunc;
    }

    private bool canSyncMldsPaidToOthers()
    {
      return (this.UseNewGFEHUD || this.UseNew2015GFEHUD) && !this.loan.GetField("SYS.X266").Equals("Broker");
    }

    private bool canSyncGFE() => this.loan.GetField("14").ToUpper() == "CA";

    private string getDescription(string val1, string val2, string val3 = null)
    {
      string str1 = "Deed $";
      string str2 = (!(val1.Trim() != string.Empty) ? str1 + "0.00" : str1 + val1.Trim()) + ";Mortgage $";
      string description = !(val2.Trim() != string.Empty) ? str2 + "0.00" : str2 + val2.Trim();
      if (val3 != null)
      {
        string str3 = description + ";Releases $";
        description = !(val3.Trim() != string.Empty) ? str3 + "0.00" : str3 + val3.Trim();
      }
      return description;
    }

    private class CalcRunner
    {
      private List<DdmCalculation.CalcFunc> _calcFuncs;

      public bool AnyCalcExecuted
      {
        get
        {
          return this._calcFuncs.Any<DdmCalculation.CalcFunc>((Func<DdmCalculation.CalcFunc, bool>) (t => t.LastResult));
        }
      }

      public List<string> TriggerFields
      {
        get
        {
          List<string> source = new List<string>();
          foreach (DdmCalculation.CalcFunc calcFunc in this._calcFuncs)
          {
            List<string> list = calcFunc.TriggerFields.Keys.ToList<string>();
            source.AddRange((IEnumerable<string>) list);
          }
          return source.GroupBy<string, string>((Func<string, string>) (t => t)).Select<IGrouping<string, string>, string>((Func<IGrouping<string, string>, string>) (g => g.First<string>())).ToList<string>();
        }
      }

      public CalcRunner() => this._calcFuncs = new List<DdmCalculation.CalcFunc>();

      public void Add(DdmCalculation.CalcFunc calcFunc) => this._calcFuncs.Add(calcFunc);

      public void Execute(string id, string val, bool forceCalc = false)
      {
        foreach (DdmCalculation.CalcFunc calcFunc in this._calcFuncs)
          calcFunc.Execute(id, val, forceCalc);
      }
    }

    private class CalcFunc
    {
      private Func<string, string, bool> _calc;
      private bool _hasBeenExecuted;
      private bool _turnOffForceCalc;
      public Dictionary<string, string> TriggerFields;

      public bool LastResult { get; set; }

      public CalcFunc() => this.TriggerFields = new Dictionary<string, string>();

      public CalcFunc(Func<string, string, bool> calc)
        : this()
      {
        this._calc = calc;
      }

      public void AddTriggerField(params string[] ids)
      {
        foreach (string id in ids)
          this.TriggerFields.Add(id, "");
      }

      public void Execute(string id, string val, bool forceCalc = false)
      {
        if (this._hasBeenExecuted && (id == null || !this.TriggerFields.ContainsKey(id)) && !(!this._turnOffForceCalc & forceCalc))
          return;
        this.LastResult = this._calc(id, val);
        if (!this.LastResult)
          return;
        this._hasBeenExecuted = true;
      }

      public DdmCalculation.CalcFunc TurnOffForceCalc()
      {
        this._turnOffForceCalc = true;
        return this;
      }

      public void ResetCalc() => this._hasBeenExecuted = false;
    }
  }
}
