// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.SECTION32InputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.Forms;
using mshtml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class SECTION32InputHandler : InputHandlerBase
  {
    private LOCompensationInputHandler loCompensationInputHandler;
    private POCInputHandler pocInputHandler;
    private EllieMae.Encompass.Forms.Panel panelOldRule;
    private EllieMae.Encompass.Forms.Panel panelNewRule;
    private EllieMae.Encompass.Forms.Panel panelNewRule2016;
    private EllieMae.Encompass.Forms.Panel panelNewRule2017;
    private EllieMae.Encompass.Forms.Label yearlyVerbiageLabel;
    private EllieMae.Encompass.Forms.Panel panelX100NoLock;
    private EllieMae.Encompass.Forms.Panel panelX100Lock;
    private static Dictionary<int, double[]> LoanAmtThresholds = new Dictionary<int, double[]>()
    {
      {
        2023,
        new double[2]{ 24866.0, 1243.0 }
      },
      {
        2022,
        new double[2]{ 22969.0, 1148.0 }
      },
      {
        2021,
        new double[2]{ 22052.0, 1103.0 }
      },
      {
        2020,
        new double[2]{ 21980.0, 1099.0 }
      },
      {
        2019,
        new double[2]{ 21549.0, 1077.0 }
      },
      {
        2018,
        new double[2]{ 21032.0, 1052.0 }
      },
      {
        2017,
        new double[2]{ 20579.0, 1029.0 }
      },
      {
        2016,
        new double[2]{ 20350.0, 1017.0 }
      },
      {
        2015,
        new double[2]{ 20391.0, 1020.0 }
      },
      {
        2014,
        new double[2]{ 20000.0, 1000.0 }
      }
    };
    private static List<FedTresholdAdjustment> adjustments = (List<FedTresholdAdjustment>) null;

    public SECTION32InputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public SECTION32InputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public SECTION32InputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public SECTION32InputHandler(
      Sessions.Session session,
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, dataTemplate, htmldoc, form, property)
    {
    }

    private string getYearlyVerbiage()
    {
      int num1 = SECTION32InputHandler.LoanAmtThresholds.Keys.Min();
      DateTime date1 = Utils.ParseDate((object) this.inputData.GetField("763"));
      DateTime date2 = Utils.ParseDate((object) this.inputData.GetField("748"));
      DateTime date3 = Utils.ParseDate((object) this.inputData.GetField("1887"));
      double num2 = 0.0;
      double num3 = 0.0;
      int yearToReturn = 2023;
      bool flag = true;
      int key1 = 0;
      SECTION32InputHandler.adjustments = this != null ? this.session?.SessionObjects?.GetThresholdsFromCache() : (List<FedTresholdAdjustment>) null;
      if (SECTION32InputHandler.adjustments == null || SECTION32InputHandler.adjustments.Count<FedTresholdAdjustment>() == 0)
        flag = false;
      else
        yearToReturn = SECTION32InputHandler.adjustments[0].AdjustmentYear;
      for (; yearToReturn > num1; yearToReturn--)
      {
        DateTime t2 = new DateTime(yearToReturn, 1, 1);
        if (flag && yearToReturn >= 2023 && DateTime.Compare(Utils.ParseDate((object) this.loan?.Calculator?.FindClosingDateToUse()), t2) >= 0)
        {
          FedTresholdAdjustment tresholdAdjustment = SECTION32InputHandler.adjustments.Find((Predicate<FedTresholdAdjustment>) (x => x.AdjustmentYear == yearToReturn && x.RuleIndex == 4));
          if (tresholdAdjustment != null)
          {
            key1 = yearToReturn;
            num2 = Utils.ParseDouble((object) tresholdAdjustment.UpperRange);
            num3 = Utils.ParseDouble((object) tresholdAdjustment.RuleValue);
            break;
          }
        }
        if (SECTION32InputHandler.LoanAmtThresholds.ContainsKey(yearToReturn) && (DateTime.Compare(date1, t2) >= 0 || DateTime.Compare(date2, t2) >= 0 || DateTime.Compare(date3, t2) >= 0))
        {
          key1 = yearToReturn;
          num2 = SECTION32InputHandler.LoanAmtThresholds[key1][0];
          num3 = SECTION32InputHandler.LoanAmtThresholds[key1][1];
          break;
        }
      }
      if (key1 == 0)
      {
        int key2 = SECTION32InputHandler.LoanAmtThresholds.Keys.Max();
        num2 = SECTION32InputHandler.LoanAmtThresholds[key2][0];
        num3 = SECTION32InputHandler.LoanAmtThresholds[key2][1];
      }
      return string.Format("For a loan amount of {0} or more total points and fees cannot exceed 5% of the total loan amount; For Loan amounts less than {0} the total points and fees cannot exceed the lesser of 8% of the total loan amount or {1}; The dollar amounts used in this test may be adjusted each year, effective January 1.", (object) num2.ToString("C0"), (object) num3.ToString("C0"));
    }

    internal override void CreateControls()
    {
      try
      {
        this.pocInputHandler = new POCInputHandler(this.inputData, this.currentForm, (InputHandlerBase) this, this.session);
        this.loCompensationInputHandler = new LOCompensationInputHandler(this.loCompensationSetting, this.inputData, this.currentForm, (InputHandlerBase) this);
        if (this.loan.Use2015RESPA)
        {
          this.yearlyVerbiageLabel = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("LabelYearlyVerbiage");
        }
        else
        {
          this.panelOldRule = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelOldRule");
          this.panelNewRule = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelNewRule");
          this.panelNewRule2016 = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelNewRule2016");
          this.panelNewRule2017 = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelNewRule2017");
          this.panelOldRule.Position = new Point(this.panelNewRule.Left, this.panelNewRule.Top);
          this.panelNewRule2016.Position = new Point(this.panelNewRule.Left, this.panelNewRule.Top);
          this.panelNewRule2017.Position = new Point(this.panelNewRule.Left, this.panelNewRule.Top);
        }
        this.panelX100NoLock = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelX100NoLock");
        this.panelX100Lock = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelX100Lock");
        if (this.panelX100NoLock == null || this.panelX100Lock == null)
          return;
        this.panelX100Lock.Left = this.panelX100NoLock.Left;
      }
      catch (Exception ex)
      {
      }
    }

    public override void RefreshContents(bool skipButtonFieldLockRules)
    {
      base.RefreshContents(skipButtonFieldLockRules);
      this.pocInputHandler.SetFieldLock819Status();
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      ControlState controlState = this.getControlState(ctrl, id, ControlState.Enabled);
      switch (id)
      {
        case "1061":
        case "436":
          if (this.loan != null && (this.loan.Use2010RESPA || this.loan.Use2015RESPA) && (this.GetFieldValue("NEWHUD.X1139") == "Y" || this.GetFieldValue("NEWHUD.X715") != "Include Origination Points" || Utils.ParseDouble((object) this.GetFieldValue("3119")) > 0.0))
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "1619":
          if (this.loan != null && this.loan.GetField("1172") == "VA" && this.loan.GetField("958") == "IRRRL" && this.loan.GetField("19").IndexOf("Refinance") > -1)
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "1663":
          controlState = ControlState.Disabled;
          if (!this.IsUsingTemplate)
          {
            this.FormatAlphaNumericField(ctrl, id);
            break;
          }
          break;
        case "1847":
        case "NEWHUD.X734":
          if (this.GetFieldValue("NEWHUD.X1139") == "Y" || this.GetFieldValue("NEWHUD.X715") != "Include Origination Credit")
            controlState = ControlState.Disabled;
          this.FormatAlphaNumericField(ctrl, id);
          break;
        case "NEWHUD.X1139":
        case "NEWHUD.X1141":
        case "NEWHUD.X1225":
        case "NEWHUD.X223":
        case "NEWHUD.X224":
          if (this.GetFieldValue("NEWHUD.X1718") == "Y")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "NEWHUD.X1299":
        case "NEWHUD.X1301":
          if (this.pocInputHandler != null)
          {
            this.pocInputHandler.SetLine819Status();
            controlState = this.pocInputHandler.GetLine819Status(id);
            break;
          }
          if (this.GetFieldValue("1172") == "FarmersHomeAdministration")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "NEWHUD.X627":
          if (this.GetFieldValue("NEWHUD.X1139") == "Y")
          {
            ctrl.Visible = false;
            controlState = ControlState.Disabled;
            break;
          }
          ctrl.Visible = true;
          break;
        case "NEWHUD.X715":
          if (Utils.ParseDouble((object) this.GetFieldValue("3119")) > 0.0 || this.GetFieldValue("NEWHUD.X1139") == "Y")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "NEWHUD.X788":
          if (this.GetFieldValue("NEWHUD.X715") != "Include Origination Points" || this.GetFieldValue("NEWHUD.X1139") == "Y")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "S32DISC.X100":
          if (DateTime.Compare(Utils.ParseDate((object) this.inputData.GetField("745")), DateTime.Parse("01/01/2018")) >= 0)
          {
            controlState = ControlState.Disabled;
            if (this.panelX100NoLock != null && this.panelX100Lock != null)
            {
              this.panelX100Lock.Visible = true;
              this.panelX100NoLock.Visible = false;
              break;
            }
            break;
          }
          if (this.panelX100NoLock != null && this.panelX100Lock != null)
          {
            this.panelX100Lock.Visible = false;
            this.panelX100NoLock.Visible = true;
            break;
          }
          break;
        case "S32DISC.X48":
          controlState = ControlState.Default;
          if (this.loan.Use2015RESPA)
          {
            if (this.yearlyVerbiageLabel != null)
            {
              this.yearlyVerbiageLabel.Text = this.getYearlyVerbiage();
              break;
            }
            break;
          }
          DateTime t2_1 = DateTime.Parse("01/01/2016");
          DateTime t2_2 = DateTime.Parse("01/01/2015");
          DateTime t2_3 = DateTime.Parse("01/01/2017");
          this.panelNewRule.Visible = false;
          if (DateTime.Compare(Utils.ParseDate((object) this.inputData.GetField("745")), t2_3) >= 0 || DateTime.Compare(Utils.ParseDate((object) this.inputData.GetField("763")), t2_3) >= 0 || DateTime.Compare(Utils.ParseDate((object) this.inputData.GetField("748")), t2_3) >= 0 || DateTime.Compare(Utils.ParseDate((object) this.inputData.GetField("1887")), t2_3) >= 0)
          {
            this.panelNewRule2017.Visible = true;
            break;
          }
          if (DateTime.Compare(Utils.ParseDate((object) this.inputData.GetField("745")), t2_1) >= 0 || DateTime.Compare(Utils.ParseDate((object) this.inputData.GetField("763")), t2_1) >= 0 || DateTime.Compare(Utils.ParseDate((object) this.inputData.GetField("748")), t2_1) >= 0 || DateTime.Compare(Utils.ParseDate((object) this.inputData.GetField("1887")), t2_1) >= 0)
          {
            this.panelNewRule2016.Visible = true;
            break;
          }
          if (DateTime.Compare(Utils.ParseDate((object) this.inputData.GetField("745")), t2_2) >= 0 || DateTime.Compare(Utils.ParseDate((object) this.inputData.GetField("763")), t2_2) >= 0 || DateTime.Compare(Utils.ParseDate((object) this.inputData.GetField("748")), t2_2) >= 0 || DateTime.Compare(Utils.ParseDate((object) this.inputData.GetField("1887")), t2_2) >= 0)
          {
            this.panelNewRule.Visible = true;
            break;
          }
          this.panelOldRule.Visible = true;
          break;
        default:
          controlState = ControlState.Default;
          break;
      }
      return controlState;
    }

    internal override void FlipLockField(FieldLock fieldLock)
    {
      base.FlipLockField(fieldLock);
      if (this.loan == null)
        return;
      FieldControl controlToLock = fieldLock.ControlToLock as FieldControl;
      if (this.loan.Use2010RESPA || this.loan.Use2015RESPA)
        this.loan.Calculator.FormCalculation("HUD1PG2_2010", (string) null, (string) null);
      this.loan.Calculator.FormCalculation("SEC32", (string) null, (string) null);
    }

    internal override void UpdateFieldValue(string id, string val)
    {
      if (this.loan.Use2010RESPA || this.loan.Use2015RESPA)
      {
        switch (id)
        {
          case "1847":
            double num1 = Utils.ParseDouble((object) val);
            if (num1 != 0.0)
            {
              val = num1.ToString("N3");
              break;
            }
            break;
          case "NEWHUD.X572":
          case "NEWHUD.X39":
          case "1663":
          case "232":
          case "230":
            double num2 = Utils.ParseDouble((object) val);
            if (num2 != 0.0)
            {
              val = num2.ToString("N2");
              break;
            }
            break;
        }
      }
      base.UpdateFieldValue(id, val);
      if (!this.loan.Use2010RESPA && !this.loan.Use2015RESPA)
      {
        if (id == "387")
          base.UpdateFieldValue("ESCROW_TABLE", "");
        if (id == "385")
          base.UpdateFieldValue("TITLE_TABLE", "");
        if (id == "1264" || id == "411")
          this.ClearFileContact(id);
      }
      if (this.loan != null)
      {
        if (this.loan.Use2010RESPA || this.loan.Use2015RESPA || this.FormIsForTemplate)
          this.loan.Calculator.FormCalculation("HUD1PG2_2010", id, val);
        this.loan.Calculator.FormCalculation("SEC32", id, val);
      }
      if (this.loCompensationInputHandler == null)
        return;
      this.loCompensationInputHandler.RefreshContents();
    }

    public override void RefreshContents()
    {
      base.RefreshContents();
      if (this.loCompensationInputHandler != null)
        this.loCompensationInputHandler.RefreshContents();
      this.pocInputHandler.SetFieldLock819Status();
    }

    public override void onmouseenter(mshtml.IHTMLEventObj pEvtObj)
    {
      if (this.inputData != null && pEvtObj.srcElement.tagName == "IMG")
        this.loCompensationInputHandler.ShowToolTips(pEvtObj);
      base.onmouseenter(pEvtObj);
    }

    public override void onmouseleave(mshtml.IHTMLEventObj pEvtObj)
    {
      if (this.inputData != null && pEvtObj.srcElement.tagName == "IMG")
        this.loCompensationInputHandler.HideToolTips(pEvtObj);
      base.onmouseleave(pEvtObj);
    }

    public override void ExecAction(string action)
    {
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      if (action == "feedetails")
      {
        if (this.loan.Use2015RESPA)
          this.LoadQuickLinkForm("2015 Itemization", "REGZGFE_2015", 750, 600, FieldSource.CurrentLoan, "2015 Itemization", false);
        else
          this.LoadQuickLinkForm("2010 Itemization", "REGZGFE_2010", 750, 600, FieldSource.CurrentLoan, "2010 Itemization", false);
      }
      else
        base.ExecAction(action);
      if (this.loan.Use2010RESPA || this.loan.Use2015RESPA)
      {
        // ISSUE: reference to a compiler-generated method
        switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(action))
        {
          case 287038707:
            if (!(action == "hudsetup"))
              return;
            this.SetFieldFocus("l_254");
            return;
          case 511528202:
            if (!(action == "cityfee"))
              return;
            this.SetFieldFocus("l_1637");
            return;
          case 869650543:
            if (!(action == "gethoepaapr"))
              return;
            break;
          case 1084911575:
            if (!(action == "recordingfee2010"))
              return;
            if (this.loan != null)
            {
              this.loan.Calculator.FormCalculation("HUD1PG2_2010", (string) null, (string) null);
              this.loan.Calculator.FormCalculation("SEC32", (string) null, (string) null);
            }
            this.SetFieldFocus("l_X604");
            return;
          case 1132459806:
            if (!(action == "statefee"))
              return;
            this.SetFieldFocus("l_1638");
            return;
          case 1181978758:
            if (!(action == "ownercoverage"))
              return;
            this.SetFieldFocus("l_1633");
            return;
          case 1231307916:
            if (!(action == "statetax2010"))
              return;
            if (this.loan != null)
            {
              this.loan.Calculator.FormCalculation("HUD1PG2_2010", (string) null, (string) null);
              this.loan.Calculator.FormCalculation("SEC32", (string) null, (string) null);
            }
            this.SetFieldFocus("l_X606");
            return;
          case 1370251432:
            if (!(action == "ownerins"))
              return;
            this.SetFieldFocus("l_578");
            return;
          case 1473801479:
            if (!(action == "statetax"))
              return;
            if (this.loan != null)
            {
              this.loan.Calculator.FormCalculation("HUD1PG2_2010", (string) null, (string) null);
              this.loan.Calculator.FormCalculation("SEC32", (string) null, (string) null);
            }
            this.SetFieldFocus("l_1638");
            return;
          case 1628156329:
            if (!(action == "lendercoverage"))
              return;
            this.SetFieldFocus("l_652");
            return;
          case 2159858259:
            if (!(action == "feedetails"))
              return;
            this.SetFieldFocus("l_X120");
            return;
          case 2536373229:
            if (!(action == "copyhudgfe800"))
              return;
            this.SetFieldFocus("l_X135");
            return;
          case 2717847646:
            if (!(action == "localtax2010"))
              return;
            if (this.loan != null)
            {
              this.loan.Calculator.FormCalculation("HUD1PG2_2010", (string) null, (string) null);
              this.loan.Calculator.FormCalculation("SEC32", (string) null, (string) null);
            }
            this.SetFieldFocus("l_X605");
            return;
          case 2979799272:
            if (!(action == "url_freddie"))
              return;
            this.SetFieldFocus("l_3134");
            return;
          case 3050596908:
            if (!(action == "recordingfee"))
              return;
            if (this.loan != null)
            {
              this.loan.Calculator.FormCalculation("HUD1PG2_2010", (string) null, (string) null);
              this.loan.Calculator.FormCalculation("SEC32", (string) null, (string) null);
            }
            this.SetFieldFocus("l_1636");
            return;
          case 3100944149:
            if (!(action == "localtax"))
              return;
            if (this.loan != null)
            {
              this.loan.Calculator.FormCalculation("HUD1PG2_2010", (string) null, (string) null);
              this.loan.Calculator.FormCalculation("SEC32", (string) null, (string) null);
            }
            this.SetFieldFocus("l_1637");
            return;
          case 3302304154:
            if (!(action == "mtginsreserv"))
              return;
            if (this.inputData.IsLocked("232"))
            {
              this.SetFieldFocus("l_232");
              return;
            }
            this.SetFieldFocus("l_X1706");
            return;
          case 3709982155:
            if (!(action == "mtginsprem"))
              return;
            this.SetFieldFocus("l_562");
            return;
          case 3949279623:
            if (!(action == "taxesreserv"))
              return;
            this.SetFieldFocus("l_231");
            return;
          case 3995849543:
            if (!(action == "calculatehoepaapr"))
              return;
            break;
          case 4112030216:
            if (!(action == "edithudgfe802"))
              return;
            this.SetFieldFocus("l_617");
            return;
          case 4113929720:
            if (!(action == "userfee2"))
              return;
            this.SetFieldFocus("l_1640");
            return;
          case 4130707339:
            if (!(action == "userfee3"))
              return;
            this.SetFieldFocus("l_1643");
            return;
          case 4162363073:
            if (!(action == "edithudgfe801"))
              return;
            this.SetFieldFocus("l_L228");
            return;
          case 4164262577:
            if (!(action == "userfee1"))
              return;
            this.SetFieldFocus("l_373");
            return;
          default:
            return;
        }
        this.SetFieldFocus("l_X177");
      }
      else
      {
        // ISSUE: reference to a compiler-generated method
        switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(action))
        {
          case 511528202:
            if (!(action == "cityfee"))
              break;
            this.SetFieldFocus("l_1637");
            break;
          case 1132459806:
            if (!(action == "statefee"))
              break;
            this.SetFieldFocus("l_1638");
            break;
          case 1393412611:
            if (!(action == "titletable"))
              break;
            this.SetFieldFocus("l_385");
            break;
          case 1958004838:
            if (!(action == "escrowtable"))
              break;
            this.SetFieldFocus("l_387");
            break;
          case 4113929720:
            if (!(action == "userfee2"))
              break;
            this.SetFieldFocus("l_1640");
            break;
          case 4130707339:
            if (!(action == "userfee3"))
              break;
            this.SetFieldFocus("l_1643");
            break;
          case 4164262577:
            if (!(action == "userfee1"))
              break;
            this.SetFieldFocus("l_373");
            break;
        }
      }
    }
  }
}
