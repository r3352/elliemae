// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.REGZ50InputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.DocEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.Forms;
using mshtml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class REGZ50InputHandler : InputHandlerBase
  {
    private Hashtable armList = new Hashtable();
    private EllieMae.Encompass.Forms.Label labelCurrentAPR;
    private Color colorCurrentAPR;
    private Plan currentPlan;
    private ImageButton imgAPRAlert;
    private ImageButton imgAPRAlertOver;
    private ImageButton imgAPRAlertNotOver;
    private StandardButton btnPlanCodeAlert;
    private StandardButton btnInvestorLookup;
    private EllieMae.Encompass.Forms.Image imgRefreshPlanCodeData;
    private EllieMae.Encompass.Forms.GroupBox boxAlertForFix;
    private EllieMae.Encompass.Forms.GroupBox boxAlertForNonFix;
    private EllieMae.Encompass.Forms.Button btnDisclosureAPR;
    private EllieMae.Encompass.Forms.CheckBox chkDocsEngine;
    private DropdownBox field_LenderRep;
    private EllieMae.Encompass.Forms.Panel PanelFixedRate;
    private EllieMae.Encompass.Forms.Panel PanelARMRate;
    private EllieMae.Encompass.Forms.Panel PanelARMIntroductory;
    private EllieMae.Encompass.Forms.Panel PanelIntOnly;
    private EllieMae.Encompass.Forms.Panel PanelARMIntOnly;
    private EllieMae.Encompass.Forms.Panel PanelFixBalloonIOBalloon;
    private EllieMae.Encompass.Forms.Panel PanelFixIOBalloon;
    private EllieMae.Encompass.Forms.Panel PanelMethodA;
    private EllieMae.Encompass.Forms.Panel PanelMethodB;
    private EllieMae.Encompass.Forms.Panel PanelBuydown;
    private EllieMae.Encompass.Forms.Panel PanelTableAlert;
    private EllieMae.Encompass.Forms.Panel PanelARMStatement;
    private EllieMae.Encompass.Forms.Panel PanelBalloonStatement;
    private EllieMae.Encompass.Forms.Panel PanelARMIntOnly51;
    private EllieMae.Encompass.Forms.Panel PanelARMIntOnly3C;
    private EllieMae.Encompass.Forms.Panel pnlForm;
    private EllieMae.Encompass.Forms.Panel panelTop;
    private EllieMae.Encompass.Forms.Panel panelBottom;
    private EllieMae.Encompass.Forms.Label labelFormName;
    private EllieMae.Encompass.Forms.Label labelAlert;
    private Calendar f761Control;
    private Calendar f762Control;
    private Calendar calCDIndexDate;
    private Calendar calLEIndexDate;

    public REGZ50InputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public REGZ50InputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
      this.armList = LoanDataMgr.LoadARMDisclosureList();
    }

    public REGZ50InputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public REGZ50InputHandler(
      Sessions.Session session,
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, dataTemplate, htmldoc, form, property)
    {
      this.armList = LoanDataMgr.LoadARMDisclosureList();
    }

    internal override void CreateControls()
    {
      if (this.loan != null && this.loan.Calculator != null)
      {
        this.loan.Calculator.CalcOnDemand(CalcOnDemandEnum.PaymentSchedule);
        if (this.loan.GetField("3296") == string.Empty && this.loan.GetField("608") == "AdjustableRate")
          this.loan.Calculator.CalcRateCap();
        this.loan.Calculator.FormCalculation("POPULATESUBJECTPROPERTYADDRESS", (string) null, (string) null);
      }
      try
      {
        if (this.chkDocsEngine == null)
          this.chkDocsEngine = (EllieMae.Encompass.Forms.CheckBox) this.currentForm.FindControl("chkDocsEngine");
        if (this.btnDisclosureAPR == null)
          this.btnDisclosureAPR = (EllieMae.Encompass.Forms.Button) this.currentForm.FindControl("btnDisclosureAPR");
        if (this.btnPlanCodeAlert == null)
          this.btnPlanCodeAlert = (StandardButton) this.currentForm.FindControl("btnPlanCodeAlert");
        if (this.btnInvestorLookup == null)
          this.btnInvestorLookup = (StandardButton) this.currentForm.FindControl("btnInvestorLookup");
        if (this.imgRefreshPlanCodeData == null)
          this.imgRefreshPlanCodeData = (EllieMae.Encompass.Forms.Image) this.currentForm.FindControl("imgRefreshPlanCodeData");
        if (this.labelCurrentAPR == null)
        {
          this.labelCurrentAPR = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("labelCurrentAPR");
          this.colorCurrentAPR = this.labelCurrentAPR.ForeColor;
        }
        if (this.imgAPRAlert == null)
          this.imgAPRAlert = (ImageButton) this.currentForm.FindControl("aprAlert");
        if (this.imgAPRAlertOver == null)
          this.imgAPRAlertOver = (ImageButton) this.currentForm.FindControl("aprAlertOver");
        if (this.imgAPRAlertNotOver == null)
          this.imgAPRAlertNotOver = (ImageButton) this.currentForm.FindControl("aprAlertNotOver");
        if (this.boxAlertForFix == null)
        {
          this.boxAlertForFix = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl("boxAlertForFix");
          this.boxAlertForNonFix = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl("boxAlertForNonFix");
        }
        if (this.boxAlertForFix != null)
        {
          this.boxAlertForFix.Top = this.imgAPRAlert.Top + this.imgAPRAlert.Size.Height;
          this.boxAlertForFix.Left = this.imgAPRAlert.Left + this.imgAPRAlert.Size.Width;
          this.boxAlertForNonFix.Top = this.boxAlertForFix.Top;
          this.boxAlertForNonFix.Left = this.boxAlertForFix.Left;
        }
        EllieMae.Encompass.Forms.TextBox control = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("l_1603");
        if (this.pnlForm == null)
          this.pnlForm = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlForm");
        if (this.labelFormName == null)
          this.labelFormName = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("LabelFormName");
        if (this.panelTop == null)
          this.panelTop = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("PanelTop");
        if (this.panelBottom == null)
          this.panelBottom = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("PanelBottom");
        if (!(this is REGZLEInputHandler) && !(this is REGZCDInputHandler))
        {
          if (this.PanelFixedRate == null)
          {
            this.PanelFixedRate = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("PanelFixedRate");
            this.PanelFixedRate.Left = 1;
          }
          if (this.PanelARMRate == null)
          {
            this.PanelARMRate = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("PanelARMRate");
            this.PanelARMRate.Left = 1;
          }
          if (this.PanelARMIntroductory == null)
          {
            this.PanelARMIntroductory = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("PanelARMIntroductory");
            this.PanelARMIntroductory.Left = 1;
          }
          if (this.PanelIntOnly == null)
          {
            this.PanelIntOnly = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("PanelIntOnly");
            this.PanelIntOnly.Left = 1;
          }
          if (this.PanelARMIntOnly == null)
          {
            this.PanelARMIntOnly = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("PanelARMIntOnly");
            this.PanelARMIntOnly.Left = 1;
          }
          if (this.PanelFixBalloonIOBalloon == null)
          {
            this.PanelFixBalloonIOBalloon = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("PanelFixBalloonIOBalloon");
            this.PanelFixBalloonIOBalloon.Left = 1;
          }
          if (this.PanelFixIOBalloon == null)
          {
            this.PanelFixIOBalloon = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("PanelFixIOBalloon");
            this.PanelFixIOBalloon.Left = 1;
          }
          if (this.PanelMethodA == null)
          {
            this.PanelMethodA = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("PanelMethodA");
            this.PanelMethodA.Left = 1;
          }
          if (this.PanelMethodB == null)
          {
            this.PanelMethodB = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("PanelMethodB");
            this.PanelMethodB.Left = 1;
          }
          if (this.PanelBuydown == null)
          {
            this.PanelBuydown = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("PanelBuydown");
            this.PanelBuydown.Left = 1;
          }
          if (this.PanelTableAlert == null)
          {
            this.PanelTableAlert = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("PanelTableAlert");
            this.PanelTableAlert.Left = 1;
          }
          if (this.PanelARMStatement == null)
          {
            this.PanelARMStatement = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("PanelARMStatement");
            this.PanelARMStatement.Left = 1;
          }
          if (this.PanelBalloonStatement == null)
          {
            this.PanelBalloonStatement = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("PanelBalloonStatement");
            this.PanelBalloonStatement.Left = 1;
          }
          if (this.PanelARMIntOnly51 == null)
          {
            this.PanelARMIntOnly51 = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("PanelARMIntOnly51");
            this.PanelARMIntOnly51.Left = 1;
          }
          if (this.PanelARMIntOnly3C == null)
          {
            this.PanelARMIntOnly3C = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("PanelARMIntOnly3C");
            this.PanelARMIntOnly3C.Left = 1;
          }
        }
        if (this.labelAlert == null)
          this.labelAlert = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("LabelAlert");
        if (this.loan != null && this.loan.IsTemplate)
        {
          switch (this)
          {
            case REGZLEInputHandler _:
              this.hideStdControl(new string[2]
              {
                "StandardButton8",
                "StandardButton3"
              });
              break;
            case REGZCDInputHandler _:
              this.hideStdControl(new string[2]
              {
                "StandardButton10",
                "StandardButton3"
              });
              break;
            case null:
              break;
            default:
              this.hideStdControl(new string[2]
              {
                "StandardButton6",
                "StandardButton9"
              });
              break;
          }
        }
        this.field_LenderRep = (DropdownBox) this.currentForm.FindControl("LenderRepDropdownBox");
        this.populateLenderValues();
        if (this.inputData.GetField("3969") == "RESPA-TILA 2015 LE and CD")
        {
          this.f761Control = (Calendar) this.currentForm.FindControl("Calendar4");
          this.f762Control = (Calendar) this.currentForm.FindControl("Calendar5");
        }
        else
        {
          this.f761Control = (Calendar) this.currentForm.FindControl("Calendar5");
          this.f762Control = (Calendar) this.currentForm.FindControl("Calendar6");
        }
        if (this.calCDIndexDate == null)
          this.calCDIndexDate = (Calendar) this.currentForm.FindControl("Calendar14");
        if (this.calLEIndexDate == null)
          this.calLEIndexDate = (Calendar) this.currentForm.FindControl("Calendar3");
      }
      catch (Exception ex)
      {
      }
      if (!this.loan.IsTemplate && !this.loan.IsInFindFieldForm)
      {
        this.subscribeToPlanCodeFieldChanges();
        this.refreshPlanComparison(true);
      }
      else
      {
        if (this.imgRefreshPlanCodeData == null)
          return;
        this.imgRefreshPlanCodeData.Visible = false;
      }
    }

    private void hideStdControl(string[] ctrlList)
    {
      foreach (string ctrl in ctrlList)
      {
        StandardButton control = (StandardButton) this.currentForm.FindControl(ctrl);
        if (control != null)
          control.Visible = false;
      }
    }

    private void subscribeToPlanCodeFieldChanges()
    {
      foreach (string planCodeField in LoanProgram.PlanCodeFields)
        this.loan.RegisterCustomFieldValueChangeEventHandler(planCodeField, new Routine(this.onPlanCodeFieldChanged));
    }

    public override void Unload()
    {
      foreach (string planCodeField in LoanProgram.PlanCodeFields)
        this.loan.UnregisterCustomFieldValueChangeEventHandler(planCodeField, new Routine(this.onPlanCodeFieldChanged));
      base.Unload();
    }

    private void onPlanCodeFieldChanged(string fieldId, string value)
    {
      if (this.currentPlan == null)
        return;
      this.refreshPlanComparison(false);
    }

    private void retrievePlanData(object planCodeObj)
    {
      REGZ50InputHandler.PlanComparisonDelegate method = new REGZ50InputHandler.PlanComparisonDelegate(this.comparePlanToLoan);
      string str = string.Concat(planCodeObj);
      try
      {
        Plan plan = (Plan) null;
        if (str != "")
          plan = (Plan) new DocEngineService(Session.SessionObjects).GetPlan(planCodeObj.ToString());
        Session.Application.Invoke((Delegate) method, new object[3]
        {
          (object) str,
          (object) plan,
          null
        });
      }
      catch (Exception ex)
      {
        Session.Application.Invoke((Delegate) method, new object[3]
        {
          (object) str,
          null,
          (object) ex.ToString()
        });
      }
    }

    private void comparePlanToLoan(string planCodeID, Plan plan, string errMsg)
    {
      if (this.loan.GetField("PlanCode.ID") != planCodeID)
        return;
      this.currentPlan = plan;
      this.imgRefreshPlanCodeData.Visible = false;
      if ((errMsg ?? "") != "")
      {
        int num1 = (int) Utils.Dialog((IWin32Window) Session.Application, "An error occurred while attempting to validate the loan's data against the selected Plan Code: " + errMsg, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (plan == null && (planCodeID ?? "") != "")
      {
        int num2 = (int) Utils.Dialog((IWin32Window) Session.Application, "The selected plan code '" + planCodeID + "' is no longer valid. You should correct this issue before ordering closing documents.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else if (plan != null && plan.CompareTo((IHtmlInput) this.loan).Length != 0)
        this.btnPlanCodeAlert.Visible = true;
      else if (plan != null)
        this.btnPlanCodeAlert.Visible = false;
      this.btnInvestorLookup.Visible = this.currentPlan != null && this.currentPlan.GetInvestor().IsGeneric;
    }

    private void refreshPlanComparison(bool async)
    {
      if (this.loan.IsTemplate || this.btnPlanCodeAlert == null)
        return;
      this.btnPlanCodeAlert.Visible = false;
      this.imgRefreshPlanCodeData.Visible = true;
      this.btnInvestorLookup.Visible = false;
      if (!this.session.Application.GetService<ILoanServices>().IsEncompassDocServiceAvailable(DocumentOrderType.Closing))
      {
        this.imgRefreshPlanCodeData.Visible = false;
      }
      else
      {
        string field = this.loan.GetField("PlanCode.ID");
        if ((field ?? "") == "")
          this.comparePlanToLoan("", (Plan) null, (string) null);
        else if (this.currentPlan != null && this.currentPlan.PlanID == field)
          this.comparePlanToLoan(field, this.currentPlan, (string) null);
        else if (async)
          ThreadPool.QueueUserWorkItem(new WaitCallback(this.retrievePlanData), (object) field);
        else
          this.retrievePlanData((object) field);
      }
    }

    protected override string GetFieldValue(string id, FieldSource fieldSource)
    {
      if (!(id == "694") && !(id == "696") || this.FormIsForTemplate)
        return base.GetFieldValue(id, fieldSource);
      return fieldSource == FieldSource.LinkedLoan && this.loan != null && this.loan.LinkedData != null ? this.loan.LinkedData.GetSimpleField(id) : this.loan.GetSimpleField(id);
    }

    internal override void UpdateFieldValue(string id, string val)
    {
      if (this.loan != null && this.loan.Calculator != null)
        this.loan.Calculator.PerformanceEnabled = false;
      if (id == "4494" && (val == "" || val == "1") && this.loan.GetField("420") == "SecondLien")
        val = this.loan.GetField("4494");
      if (id == "1959")
      {
        if (this.armList.ContainsKey((object) val))
          base.UpdateFieldValue("1960", this.armList[(object) val].ToString());
        else
          base.UpdateFieldValue("1960", "");
      }
      else if (id == "SYS.X1" && val == "N")
        val = string.Empty;
      else if (id == "608")
      {
        if (val != "OtherAmortizationType")
          base.UpdateFieldValue("994", string.Empty);
        if (val != "GraduatedPaymentMortgage")
        {
          base.UpdateFieldValue("1266", string.Empty);
          base.UpdateFieldValue("1267", string.Empty);
        }
      }
      if (id == "Docs.Engine" && val != this.inputData.GetField("Docs.Engine") && !this.validateDocEngineChange(val))
        return;
      base.UpdateFieldValue(id, val);
      if (id == "3121")
        Session.Application.GetService<ILoanEditor>().RefreshLogPanel();
      else if (id == "3942" && val == "Y")
      {
        base.UpdateFieldValue("L724", "");
        base.UpdateFieldValue("4907", "");
        base.UpdateFieldValue("4908", "");
        base.UpdateFieldValue("4909", "");
        base.UpdateFieldValue("4910", "");
        base.UpdateFieldValue("4911", "");
      }
      if (this.loan == null || this.loan.Calculator == null)
        return;
      this.loan.Calculator.FormCalculation("QMAPR");
    }

    internal override void FlipLockField(FieldLock fieldLock)
    {
      bool flag = false;
      if (this.loan != null && this.loan.Calculator != null)
      {
        flag = this.loan.Calculator.PerformanceEnabled;
        this.loan.Calculator.PerformanceEnabled = false;
      }
      base.FlipLockField(fieldLock);
      if (this.loan == null || this.loan.Calculator == null)
        return;
      this.loan.Calculator.PerformanceEnabled = flag;
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      ControlState controlState = this.getControlState(ctrl, id, ControlState.Enabled);
      switch (id)
      {
        case "1063":
        case "FR0104":
label_94:
          return controlState;
        case "1198":
        case "1200":
          if (this.loan.GetField("3533") == "Y")
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "1205":
          if (this.loan.GetField("1172") == "FarmersHomeAdministration" || this.loan.GetField("3533") == "Y")
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "1209":
        case "2978":
          controlState = !(this.loan.GetField("1172") == "FHA") ? ControlState.Enabled : ControlState.Disabled;
          goto case "1063";
        case "1266":
        case "1267":
          controlState = !(this.loan.GetField("608") != "GraduatedPaymentMortgage") ? ControlState.Enabled : ControlState.Disabled;
          goto case "1063";
        case "1413":
        case "1482":
        case "1483":
        case "1888":
        case "1889":
        case "1890":
        case "1891":
        case "1892":
        case "1893":
        case "1965":
        case "1966":
        case "1967":
        case "1968":
        case "1986":
        case "1987":
        case "2":
        case "4464":
        case "4472":
        case "4473":
        case "4475":
        case "4483":
        case "4484":
        case "4492":
        case "4530":
        case "4531":
        case "HELOC.MinAdvPct":
        case "HELOC.MinPmtLessThanAmt":
        case "HELOC.MinPmtUPB":
        case "HELOC.MinPmtUnpdBalAmt":
        case "HELOC.MinRepmtPct":
        case "HELOC.OvrLmtChg":
        case "HELOC.OvrLmtRtnChg":
        case "HELOC.ParticipationFees":
        case "HELOC.RlsRecgChg":
        case "HELOC.RtdChkChgAmt":
        case "HELOC.RtdChkChgMax":
        case "HELOC.RtdChkChgMin":
        case "HELOC.RtdChkChgRat":
        case "HELOC.StopPmtChrg":
        case "HELOC.TransactionFees":
        case "HELOC.WireFee":
        case "hid":
          controlState = !(this.loan.GetField("1172") != "HELOC") ? ControlState.Enabled : ControlState.Disabled;
          goto case "1063";
        case "1679":
        case "1681":
        case "1683":
        case "1685":
        case "1687":
        case "1689":
        case "1691":
        case "1693":
        case "1695":
        case "GLOBAL.S1":
        case "GLOBAL.S10":
        case "GLOBAL.S11":
        case "GLOBAL.S13":
        case "GLOBAL.S14":
        case "GLOBAL.S16":
        case "GLOBAL.S17":
        case "GLOBAL.S19":
        case "GLOBAL.S2":
        case "GLOBAL.S20":
        case "GLOBAL.S22":
        case "GLOBAL.S23":
        case "GLOBAL.S25":
        case "GLOBAL.S26":
        case "GLOBAL.S4":
        case "GLOBAL.S5":
        case "GLOBAL.S7":
        case "GLOBAL.S8":
          if (this.loan.GetField("1678") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "1951":
          this.SetControlState("StandardButton3", this.loan.GetField(id) != string.Empty);
          goto case "1063";
        case "1961":
          controlState = this.loan.GetField("1962") == "360/360" && this.loan.GetField("SYS.X6") == "A" || this.loan.GetField("19") == "ConstructionToPermanent" ? ControlState.Disabled : ControlState.Enabled;
          goto case "1063";
        case "1963":
          controlState = !(this.loan.GetField("19") != "ConstructionToPermanent") ? ControlState.Enabled : ControlState.Disabled;
          goto case "1063";
        case "1964":
        case "Constr.Refi":
          if (this.inputData.GetField("19") != "ConstructionOnly" && this.inputData.GetField("19") != "ConstructionToPermanent")
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "299":
          controlState = this.loan.GetField("19") == "Purchase" || this.loan.GetField("19") == "Other" || this.loan.GetField("19") == "" ? ControlState.Disabled : ControlState.Enabled;
          goto case "1063";
        case "3121":
          if (this.loan != null && !this.loan.Use2010RESPA && !this.loan.Use2015RESPA)
            this.btnDisclosureAPR.Visible = true;
          controlState = ControlState.Default;
          goto case "1063";
        case "3266":
          controlState = !(this.GetField("1172") == "FarmersHomeAdministration") || Utils.ParseDouble((object) this.GetField("NEWHUD.X1707")) <= 0.0 ? ControlState.Disabled : ControlState.Enabled;
          goto case "1063";
        case "364":
          if (this.FormIsForTemplate || InputHandlerBase.LockLoanNumber)
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "3942":
          switch (this.GetField("19"))
          {
            case "Purchase":
              controlState = ControlState.Disabled;
              goto label_94;
            case "ConstructionOnly":
            case "ConstructionToPermanent":
              if (!(this.GetField("CONST.X2") != "Y"))
                break;
              goto case "Purchase";
          }
          controlState = ControlState.Enabled;
          goto case "1063";
        case "4465":
        case "4466":
        case "4467":
        case "4468":
          controlState = !(this.loan.GetField("4464") == "Rate") || !(this.loan.GetField("1172") == "HELOC") ? ControlState.Disabled : ControlState.Enabled;
          goto case "1063";
        case "4469":
        case "4470":
          controlState = !(this.loan.GetField("4531") == "Fraction of Balance") || !(this.loan.GetField("1172") == "HELOC") ? ControlState.Disabled : ControlState.Enabled;
          goto case "1063";
        case "4471":
          controlState = !(this.loan.GetField("4531") == "Percentage of Balance") || !(this.loan.GetField("1172") == "HELOC") ? ControlState.Disabled : ControlState.Enabled;
          goto case "1063";
        case "4474":
          controlState = this.loan.GetField("4464") == "Rate" && this.loan.GetField("4468") == "Y" || this.loan.GetField("1172") != "HELOC" ? ControlState.Disabled : ControlState.Enabled;
          goto case "1063";
        case "4476":
        case "4477":
        case "4478":
        case "4479":
          controlState = !(this.loan.GetField("4475") == "Rate") || !(this.loan.GetField("1172") == "HELOC") ? ControlState.Disabled : ControlState.Enabled;
          goto case "1063";
        case "4480":
        case "4481":
          controlState = !(this.loan.GetField("4530") == "Fraction of Balance") || !(this.loan.GetField("1172") == "HELOC") ? ControlState.Disabled : ControlState.Enabled;
          goto case "1063";
        case "4482":
          controlState = !(this.loan.GetField("4530") == "Percentage of Balance") || !(this.loan.GetField("1172") == "HELOC") ? ControlState.Disabled : ControlState.Enabled;
          goto case "1063";
        case "4485":
          controlState = !(this.loan.GetField("4475") == "Rate") || !(this.loan.GetField("4479") == "Y") || !(this.loan.GetField("1172") == "HELOC") ? ControlState.Disabled : ControlState.Enabled;
          goto case "1063";
        case "4491":
          controlState = !(this.loan.GetField("4464") == "Rate") || !(this.loan.GetField("4468") == "Y") || this.loan.GetField("1172") != "HELOC" ? ControlState.Disabled : ControlState.Enabled;
          goto case "1063";
        case "4494":
          controlState = this.loan.GetField("420") == "FirstLien" || this.loan.GetField("420") == "" ? ControlState.Disabled : ControlState.Enabled;
          goto case "1063";
        case "4645":
        case "buydowntypelookup":
          if (this.inputData.GetField("CASASRN.X141") == "")
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "4673":
          controlState = this.inputData.GetField("4672") == "" || this.inputData.GetField("4672") == "None" ? ControlState.Enabled : ControlState.Disabled;
          goto case "1063";
        case "4674":
        case "4676":
        case "4677":
        case "4683":
          controlState = this.inputData.GetField("4672") == "" || this.inputData.GetField("4672") == "None" || this.inputData.GetField("4840") == "Y" ? ControlState.Enabled : ControlState.Disabled;
          goto case "1063";
        case "4746":
          string field1 = this.inputData.GetField("3");
          string field2 = this.inputData.GetField("608");
          return !string.IsNullOrEmpty(field1) && Utils.ParseDouble((object) field1) == 0.0 && field2 != "Fixed" ? ControlState.Disabled : ControlState.Enabled;
        case "4840":
        case "lenderreplookup":
          controlState = this.inputData.GetField("4672") == "" || this.inputData.GetField("4672") == "None" || id == "lenderreplookup" && this.inputData.GetField("4672").StartsWith("Custom") ? ControlState.Disabled : ControlState.Enabled;
          goto case "1063";
        case "4898":
          if (this.loan.IsTemplate)
          {
            controlState = ControlState.Disabled;
            if (this.calCDIndexDate != null)
              this.calCDIndexDate.Enabled = false;
            if (this.calLEIndexDate != null)
            {
              this.calLEIndexDate.Enabled = false;
              goto case "1063";
            }
            else
              goto case "1063";
          }
          else
            goto case "1063";
        case "4907":
        case "4908":
        case "4909":
        case "4910":
        case "4911":
        case "L724":
          controlState = !(this.GetField("3942") == "Y") ? ControlState.Enabled : ControlState.Disabled;
          this.SetControlState("Calendar_L724", controlState == ControlState.Enabled);
          this.SetControlState("Calendar_L724_2", controlState == ControlState.Enabled);
          this.SetControlState("Calendar_4908", controlState == ControlState.Enabled);
          goto case "1063";
        case "5015":
          controlState = !(this.inputData.GetField("Constr.Refi") == "Y") || !(this.inputData.GetField("19") == "ConstructionOnly") && !(this.inputData.GetField("19") == "ConstructionToPermanent") ? ControlState.Disabled : ControlState.Enabled;
          goto case "1063";
        case "608":
          this.SetControlState("StandardButton4", this.loan.GetField(id) == "AdjustableRate");
          goto case "1063";
        case "761":
          if (this.GetField("3941") == "Y")
          {
            if (this.f761Control != null)
              this.f761Control.Enabled = false;
            return ControlState.Disabled;
          }
          if (this.f761Control != null)
          {
            this.f761Control.Enabled = true;
            goto case "1063";
          }
          else
            goto case "1063";
        case "762":
          if (this.GetField("3941") == "Y")
          {
            if (this.f762Control != null)
              this.f762Control.Enabled = false;
            return ControlState.Disabled;
          }
          if (this.f762Control != null)
          {
            this.f762Control.Enabled = true;
            goto case "1063";
          }
          else
            goto case "1063";
        case "799":
          this.refreshAPRAlert();
          controlState = ControlState.Default;
          goto case "1063";
        case "9":
          controlState = !(this.loan.GetField("19") != "Other") ? ControlState.Enabled : ControlState.Disabled;
          goto case "1063";
        case "994":
          controlState = !(this.loan.GetField("608") != "OtherAmortizationType") ? ControlState.Enabled : ControlState.Disabled;
          goto case "1063";
        case "995":
          controlState = !(this.loan.GetField("608") != "AdjustableRate") || !(this.inputData.GetField("1172") != "HELOC") ? ControlState.Enabled : ControlState.Disabled;
          goto case "1063";
        case "Docs.Engine":
          ILoanServices service = Session.Application.GetService<ILoanServices>();
          if (this.loan.IsTemplate)
          {
            ctrl.Visible = false;
            goto case "1063";
          }
          else if (!service.IsEncompassDocServiceAvailable(DocumentOrderType.Closing))
          {
            ctrl.Visible = false;
            goto case "1063";
          }
          else
            goto case "1063";
        case "QM.X2":
          string field3 = this.GetField("19");
          if (field3 != "NoCash-Out Refinance" && field3 != "Cash-Out Refinance")
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "REGZ-TILAudit":
        case "REGZ-TILOrder Docs":
        case "REGZ-TILView":
          if (this.loan == null || !this.loan.IsInFindFieldForm)
          {
            ctrl.Visible = false;
            goto case "1063";
          }
          else
            goto case "1063";
        case "Terms.USDAGovtType":
          if (this.GetField("1172") != "FarmersHomeAdministration")
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "altlender":
        case "docsigningdate":
        case "plancode":
        case "subfin":
        case "transferto":
          if (this.FormIsForTemplate)
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "calcmanual":
          controlState = !(this.loan.GetField("1678") != "Y") ? ControlState.Enabled : ControlState.Disabled;
          goto case "1063";
        case "ccprog":
        case "copymers":
        case "loanprog":
        case "loanprog1":
          controlState = !this.FormIsForTemplate ? ControlState.Enabled : ControlState.Disabled;
          goto case "1063";
        case "clearaltlender":
          if (this.loan.IsTemplate || this.loan.GetField("1882") == "")
            return ControlState.Disabled;
          goto case "1063";
        case "clearlp":
          if (this.loan.IsTemplate || this.loan.GetField("1401") == "")
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "clearplancode":
          if (this.loan.IsTemplate || this.loan.GetField("PlanCode.ID") == "" && this.loan.GetField("1881") == "")
            return ControlState.Disabled;
          goto case "1063";
        case "docdataviewer":
          ctrl.Visible = !this.loan.IsTemplate && EncompassDocs.IsUsingEncompassDocsSolution((IHtmlInput) this.loan) && this.session.ACL.IsAuthorizedForFeature(AclFeature.LoanTab_EMClosingDocs_ViewDocData);
          goto case "1063";
        case "drawrepayperiod":
          controlState = this.loan.GetField("1172") != "HELOC" || this.loan.GetField("4630") == "Y" ? ControlState.Disabled : ControlState.Enabled;
          goto case "1063";
        case "getindex":
          if (this.loan.IsTemplate)
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "paymentexample":
          return controlState;
        default:
          controlState = ControlState.Default;
          goto case "1063";
      }
    }

    public override void onmouseenter(mshtml.IHTMLEventObj pEvtObj)
    {
      if (pEvtObj.srcElement.id == "aprAlert")
      {
        this.imgAPRAlert.Source = this.imgAPRAlertOver.Source;
        if (this.labelCurrentAPR != null && this.labelCurrentAPR.ForeColor == AppColors.AlertRed)
        {
          if (this.GetFieldValue("608") == "Fixed")
            this.boxAlertForFix.Visible = true;
          else
            this.boxAlertForNonFix.Visible = true;
        }
      }
      base.onmouseenter(pEvtObj);
    }

    public override void onmouseleave(mshtml.IHTMLEventObj pEvtObj)
    {
      if (!(pEvtObj.srcElement.id == "aprAlert"))
        return;
      this.imgAPRAlert.Source = this.imgAPRAlertNotOver.Source;
      if (this.boxAlertForFix == null)
        return;
      this.boxAlertForFix.Visible = false;
      this.boxAlertForNonFix.Visible = false;
    }

    public override void RefreshContents(bool skipButtonFieldLockRules)
    {
      base.RefreshContents(skipButtonFieldLockRules);
    }

    private void refreshAPRAlert()
    {
      if (this.loan != null && !this.FormIsForTemplate)
      {
        switch (this)
        {
          case REGZLEInputHandler _:
          case REGZCDInputHandler _:
label_34:
            if (RegulationAlerts.GetRediscloseTILAlertRateChange(this.session.LoanData) != null)
            {
              if (this.labelCurrentAPR != null)
                this.labelCurrentAPR.ForeColor = AppColors.AlertRed;
              if (this.imgAPRAlert != null)
                this.imgAPRAlert.Visible = true;
            }
            else
            {
              if (this.labelCurrentAPR != null)
                this.labelCurrentAPR.ForeColor = this.colorCurrentAPR;
              if (this.imgAPRAlert != null)
                this.imgAPRAlert.Visible = false;
            }
            if (this.loan == null || this.loan.RediscloseAPRRequired == null)
              break;
            this.loan.RediscloseAPRRequired((object) null, (EventArgs) null);
            break;
          default:
            this.PanelFixedRate.Visible = false;
            this.PanelARMRate.Visible = false;
            this.PanelARMIntroductory.Visible = false;
            this.PanelIntOnly.Visible = false;
            this.PanelARMIntOnly.Visible = false;
            this.PanelFixBalloonIOBalloon.Visible = false;
            this.PanelFixIOBalloon.Visible = false;
            this.PanelMethodA.Visible = false;
            this.PanelMethodB.Visible = false;
            this.PanelTableAlert.Visible = false;
            this.PanelARMStatement.Visible = false;
            this.PanelBalloonStatement.Visible = false;
            this.PanelARMIntOnly51.Visible = false;
            this.PanelARMIntOnly3C.Visible = false;
            EllieMae.Encompass.Forms.Panel panel = (EllieMae.Encompass.Forms.Panel) null;
            RegzSummaryTableType regzSummaryType = this.loan.Calculator.RegzSummaryType;
            switch (regzSummaryType - 1)
            {
              case RegzSummaryTableType.None:
                panel = this.PanelFixedRate;
                break;
              case RegzSummaryTableType.Fixed:
                panel = this.PanelARMRate;
                break;
              case RegzSummaryTableType.ARMLess5Years:
                panel = this.PanelARMIntroductory;
                break;
              case RegzSummaryTableType.ARMGreater5Years:
                panel = this.PanelIntOnly;
                break;
              case RegzSummaryTableType.FixedIntOnly:
                panel = this.PanelARMIntOnly;
                break;
              case RegzSummaryTableType.ARMIntOnly:
              case RegzSummaryTableType.ARMIntOnly31:
              case RegzSummaryTableType.ARMIntOnly3C:
              case RegzSummaryTableType.ARMIntOnly7_1or10_1:
                panel = this.PanelARMIntOnly51;
                break;
              case RegzSummaryTableType.ARMIntOnly51:
                panel = this.PanelARMIntOnly3C;
                break;
              case RegzSummaryTableType.ARMIO_L60:
                panel = this.PanelFixBalloonIOBalloon;
                break;
              case RegzSummaryTableType.FixedBalloon:
              case RegzSummaryTableType.FixedBalloonIntOnlyGreater:
                panel = this.PanelFixIOBalloon;
                break;
              case RegzSummaryTableType.FixedBalloonIntOnlyLesser:
                panel = this.PanelMethodA;
                break;
              case RegzSummaryTableType.ConstOnlyA:
                panel = this.PanelMethodB;
                break;
              case RegzSummaryTableType.ConstOnlyB:
                panel = this.PanelBuydown;
                break;
              case RegzSummaryTableType.Buydown:
              case RegzSummaryTableType.InvalidPermB:
              case RegzSummaryTableType.InvalidNegARM:
                panel = this.PanelTableAlert;
                break;
            }
            switch (regzSummaryType - 2)
            {
              case RegzSummaryTableType.None:
              case RegzSummaryTableType.Fixed:
              case RegzSummaryTableType.ARMGreater5Years:
              case RegzSummaryTableType.FixedIntOnly:
              case RegzSummaryTableType.ARMIntOnly:
              case RegzSummaryTableType.ARMIntOnly31:
              case RegzSummaryTableType.ARMIntOnly51:
              case RegzSummaryTableType.ARMIntOnly3C:
                if (Utils.ParseDouble((object) this.GetField("3")) < Utils.ParseDouble((object) this.GetField("3296")))
                {
                  this.PanelARMStatement.Visible = true;
                  break;
                }
                break;
              case RegzSummaryTableType.ConstOnlyB:
                this.labelAlert.Text = "The Payment Summary is not available because the Purpose of Loan (field 19) is Construction Perm AND the Est. Interest On (field SYS.X6) value is Full Loan.";
                break;
              case RegzSummaryTableType.Buydown:
                this.labelAlert.Text = "The Payment Summary is not available because the Discount Rate (field 2551) OR Discount Period has a value specified.";
                break;
              case RegzSummaryTableType.InvalidPermB:
                this.labelAlert.Text = "The Payment Summary is not available because the Purpose of Loan (field 19) is Construction or Construction Perm AND the Term (field 4) is greater than the Due In period (field 325) AND the Interest Only time period has a value specified.";
                break;
            }
            if (regzSummaryType != RegzSummaryTableType.InvalidConstIO && regzSummaryType != RegzSummaryTableType.InvalidNegARM && regzSummaryType != RegzSummaryTableType.InvalidPermB && Utils.ParseDouble((object) this.GetField("3288")) > 0.0)
              this.PanelBalloonStatement.Visible = true;
            int num1 = this.panelTop.Top + this.panelTop.Size.Height;
            if (panel == null)
            {
              this.panelBottom.Top = num1 + 1;
            }
            else
            {
              panel.Top = num1 + 1;
              int num2 = panel.Top + panel.Size.Height;
              if (this.PanelBalloonStatement.Visible)
              {
                this.PanelBalloonStatement.Top = num2 - 2;
                num2 = this.PanelBalloonStatement.Top + this.PanelBalloonStatement.Size.Height;
              }
              if (this.PanelARMStatement.Visible)
              {
                this.PanelARMStatement.Top = num2 - 2;
                num2 = this.PanelARMStatement.Top + this.PanelARMStatement.Size.Height;
              }
              this.panelBottom.Top = num2 + 1;
            }
            EllieMae.Encompass.Forms.Panel pnlForm = this.pnlForm;
            int width = this.pnlForm.Size.Width;
            int top = this.panelBottom.Top;
            Size size1 = this.panelBottom.Size;
            int height1 = size1.Height;
            int height2 = top + height1 + 5;
            Size size2 = new Size(width, height2);
            pnlForm.Size = size2;
            EllieMae.Encompass.Forms.Label labelFormName = this.labelFormName;
            size1 = this.pnlForm.Size;
            int num3 = size1.Height + 5;
            labelFormName.Top = num3;
            if (panel != null)
            {
              panel.Visible = true;
              goto label_34;
            }
            else
              goto label_34;
        }
      }
      else
      {
        switch (this)
        {
          case REGZLEInputHandler _:
            break;
          case REGZCDInputHandler _:
            break;
          default:
            this.panelBottom.Top = this.panelTop.Top + this.panelTop.Size.Height + 1;
            this.pnlForm.Size = new Size(this.pnlForm.Size.Width, this.panelBottom.Top + this.panelBottom.Size.Height + 5);
            this.labelFormName.Top = this.pnlForm.Size.Height + 5;
            break;
        }
      }
    }

    public override void ExecAction(string action)
    {
      switch (action)
      {
        case "altlender":
        case "transferto":
          base.ExecAction(action);
          break;
        case "calcmanual":
          this.loan.Calculator.RecalculateManualInput();
          this.UpdateContents();
          break;
        case "ccprog":
          if (new ClosingCostSelect(this.loan).ShowDialog((IWin32Window) this.mainScreen) == DialogResult.OK)
            this.UpdateContents();
          this.SetFieldFocus("l_363");
          break;
        case "clearaltlender":
          base.ExecAction(action);
          break;
        case "clearplancode":
          this.clearPlanCode();
          break;
        case "cleartransferto":
          this.loan.SetCurrentField("1951", "");
          this.UpdateContents();
          this.SetFieldFocus("l_136");
          break;
        case "copymers":
          LockRequestLog[] allLockRequests = this.loan.GetLogList().GetAllLockRequests();
          for (int index = 0; index < allLockRequests.Length; ++index)
          {
            if (allLockRequests[index].RequestedStatus == "Locked")
            {
              Hashtable lockRequestSnapshot = allLockRequests[index].GetLockRequestSnapshot();
              if (lockRequestSnapshot.ContainsKey((object) "2290") && lockRequestSnapshot[(object) "2290"].ToString() != string.Empty)
                this.loan.SetCurrentField("1051", lockRequestSnapshot[(object) "2290"].ToString());
            }
          }
          break;
        case "discloseapr":
          base.ExecAction(action);
          this.SetFieldFocus("l_1401");
          break;
        case "docsigningdate":
          base.ExecAction(action);
          break;
        case "drawrepayperiod":
          HelocRateTable drawRepayPeriod = this.loan.GetDrawRepayPeriod();
          string field = this.loan.GetField("1985");
          using (HelocTableContainer helocTableContainer = new HelocTableContainer(drawRepayPeriod, field))
          {
            if (helocTableContainer.ShowDialog((IWin32Window) this.mainScreen) == DialogResult.OK)
            {
              if (this.loan.SetDrawRepayPeriod(helocTableContainer.HelocTable))
              {
                this.loan.SetCurrentField("1985", helocTableContainer.HelocTableName);
                if (drawRepayPeriod.IsNewHELOC)
                  this.loan.SetCurrentField("4630", "Y");
                else
                  this.loan.SetCurrentField("4630", "");
                this.UpdateContents();
              }
            }
          }
          this.SetFieldFocus("l_1891");
          break;
        case "loanprog":
          base.ExecAction(action);
          break;
        case "loanprog1":
          base.ExecAction(action);
          break;
        case "mtgins":
          base.ExecAction(action);
          this.SetFieldFocus("l_1198");
          break;
        case "plancode":
          base.ExecAction(action);
          this.refreshPlanComparison(true);
          break;
        case "prepayment":
          base.ExecAction(action);
          this.SetFieldFocus("l_672");
          break;
        case "subfin":
          base.ExecAction(action);
          this.SetFieldFocus("I_8");
          break;
        case "viewworstcaseinregz":
          base.ExecAction(action);
          this.SetFieldFocus("l_1205");
          break;
        case "zoomarm":
          ARMTypeDetails armTypeDetails = new ARMTypeDetails("995", this.loan.GetSimpleField("995"));
          if (armTypeDetails.ShowDialog((IWin32Window) this.mainScreen) == DialogResult.OK)
          {
            this.loan.SetCurrentField("995", armTypeDetails.ArmTypeID);
            this.UpdateContents();
          }
          this.SetFieldFocus("l_995");
          break;
        default:
          base.ExecAction(action);
          break;
      }
    }

    private void clearPlanCode()
    {
      base.ExecAction("clearplancode");
      this.refreshPlanComparison(false);
    }

    private void populatePropertyAddress(object sender, EventArgs e)
    {
      string field = this.loan.GetField("1603");
      if (field != string.Empty && field != null)
        return;
      string val = (this.loan.GetField("11") + ", " + this.loan.GetField("12") + " " + this.loan.GetField("14") + " " + this.loan.GetField("15")).Trim();
      if (val == ",")
        val = string.Empty;
      this.loan.SetCurrentField("1603", val);
      this.UpdateContents();
    }

    private bool validateDocEngineChange(string newValue)
    {
      string str = !(newValue == "New_Encompass_Docs_Solution") ? "Old ODI Encompass Docs" : "New Encompass Docs Solutions";
      ILoanServices service = Session.Application.GetService<ILoanServices>();
      List<string> stringList = new List<string>();
      if (service.IsEncompassDocServiceAvailable(DocumentOrderType.Opening))
        stringList.Add("eDisclosures");
      if (service.IsEncompassDocServiceAvailable(DocumentOrderType.Closing))
        stringList.Add("Closing Documents");
      if (Utils.Dialog((IWin32Window) Session.Application, "You have elected to use the " + str + " to generate " + string.Join(" and ", stringList.ToArray()) + ". By switching to this service, you will required to re-select Plan Code and, if applicable, Alt Lender information for this loan." + Environment.NewLine + Environment.NewLine + "Are you sure you want to proceed with this change?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
        return false;
      DocEngineUtils.ChangeEncompassDocEngine(this.inputData, newValue);
      return true;
    }

    private void populateLenderValues()
    {
      this.field_LenderRep.Options.Add("");
      this.field_LenderRep.Options.Add("None");
      RoleInfo[] roleInfoArray = (RoleInfo[]) null;
      if (this.session != null)
      {
        if (this.session.LoanDataMgr != null)
          roleInfoArray = this.session.LoanDataMgr.SystemConfiguration.AllRoles;
        else if (this.loan.IsTemplate && this.session.BPM != null)
          roleInfoArray = ((WorkflowManager) this.session.BPM.GetBpmManager(BpmCategory.Workflow)).GetAllRoleFunctions();
      }
      if (roleInfoArray != null)
      {
        foreach (RoleSummaryInfo roleSummaryInfo in roleInfoArray)
          this.field_LenderRep.Options.Add("Role - " + roleSummaryInfo.RoleName);
      }
      string text1 = this.loan.GetField("VEND.X84");
      if (string.IsNullOrEmpty(text1))
        text1 = "Custom Category #1";
      this.field_LenderRep.Options.Add(text1);
      string text2 = this.loan.GetField("VEND.X85");
      if (string.IsNullOrEmpty(text2))
        text2 = "Custom Category #2";
      this.field_LenderRep.Options.Add(text2);
      string text3 = this.loan.GetField("VEND.X86");
      if (string.IsNullOrEmpty(text3))
        text3 = "Custom Category #3";
      this.field_LenderRep.Options.Add(text3);
      string text4 = this.loan.GetField("VEND.X11");
      if (string.IsNullOrEmpty(text4))
        text4 = "Custom Category #4";
      this.field_LenderRep.Options.Add(text4);
    }

    private delegate void PlanComparisonDelegate(string planCodeID, Plan plan, string errMsg);
  }
}
