// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.ATR_QUALIFICATIONInputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.Forms;
using mshtml;
using System;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class ATR_QUALIFICATIONInputHandler : InputHandlerBase
  {
    private CategoryBox boxMonthlyPay1;
    private CategoryBox boxMonthlyPay2;
    private EllieMae.Encompass.Forms.Label labelFor3290;
    private EllieMae.Encompass.Forms.Label labelForX373;
    private EllieMae.Encompass.Forms.Label labelForX337;
    private EllieMae.Encompass.Forms.Label labelFor1725;
    private EllieMae.Encompass.Forms.TextBox l_1724;
    private EllieMae.Encompass.Forms.TextBox l_1725;
    private FieldLock fl_1724;
    private FieldLock fl_1725;
    private EllieMae.Encompass.Forms.Panel panelPrimaryHome;
    private EllieMae.Encompass.Forms.Label labelFor229;
    private EllieMae.Encompass.Forms.TextBox l_228;
    private EllieMae.Encompass.Forms.TextBox l_229;
    private EllieMae.Encompass.Forms.Panel panelSecondHome;
    private EllieMae.Encompass.Forms.Panel panelSec32SellerPaid;
    private EllieMae.Encompass.Forms.TextBox l_QMX378;
    private FieldLock fl_QMX378;
    private EllieMae.Encompass.Forms.CheckBox chkbx_QMX383;

    public ATR_QUALIFICATIONInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public ATR_QUALIFICATIONInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public ATR_QUALIFICATIONInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public ATR_QUALIFICATIONInputHandler(
      Sessions.Session session,
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, dataTemplate, htmldoc, form, property)
    {
    }

    internal override void CreateControls()
    {
      if (this.loan != null && this.loan.Calculator != null)
      {
        this.loan.Calculator.CalcOnDemand(CalcOnDemandEnum.PaymentSchedule);
        this.loan.TriggerCalculation("QM.X383", (string) null);
      }
      try
      {
        if (this.boxMonthlyPay1 == null)
          this.boxMonthlyPay1 = (CategoryBox) this.currentForm.FindControl("boxMonthlyPay1");
        if (this.boxMonthlyPay2 == null)
          this.boxMonthlyPay2 = (CategoryBox) this.currentForm.FindControl("boxMonthlyPay2");
        if (this.labelFor3290 == null)
          this.labelFor3290 = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("labelFor3290");
        if (this.labelForX373 == null)
          this.labelForX373 = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("labelForX373");
        if (this.labelForX337 == null)
          this.labelForX337 = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("labelForX337");
        if (this.labelFor1725 == null)
          this.labelFor1725 = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("labelFor1725");
        if (this.l_1724 == null)
          this.l_1724 = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("l_1724");
        if (this.fl_1724 == null)
          this.fl_1724 = (FieldLock) this.currentForm.FindControl("FieldLock14");
        if (this.l_1725 == null)
          this.l_1725 = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("l_1725");
        if (this.fl_1725 == null)
          this.fl_1725 = (FieldLock) this.currentForm.FindControl("FieldLock13");
        if (this.l_1724 != null && this.l_1725 != null)
        {
          this.l_1724.Top = this.l_1725.Top;
          this.fl_1724.Top = this.fl_1725.Top;
          this.l_1724.Left = this.l_1725.Left;
          this.fl_1724.Left = this.fl_1725.Left;
        }
        if (this.labelFor229 == null)
          this.labelFor229 = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("labelFor229");
        if (this.l_228 == null)
          this.l_228 = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("l_228");
        if (this.l_229 == null)
          this.l_229 = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("l_229");
        if (this.l_228 != null && this.l_229 != null)
        {
          this.l_228.Top = this.l_229.Top;
          this.l_228.Left = this.l_229.Left;
        }
        if (this.panelPrimaryHome == null)
          this.panelPrimaryHome = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelPrimaryHome");
        if (this.panelSecondHome == null)
        {
          this.panelSecondHome = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelSecondHome");
          this.panelSecondHome.Top = this.panelPrimaryHome.Top;
          this.panelSecondHome.Left = this.panelPrimaryHome.Left;
        }
        this.panelSec32SellerPaid = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelSec32SellerPaid");
        this.fl_QMX378 = (FieldLock) this.currentForm.FindControl("fl_QMX378");
        this.l_QMX378 = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("l_QMX378");
        this.chkbx_QMX383 = (EllieMae.Encompass.Forms.CheckBox) this.currentForm.FindControl("CheckBox_UsePriceBasedGQM");
      }
      catch (Exception ex)
      {
      }
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      ControlState controlState = this.getControlState(ctrl, id, ControlState.Enabled);
      switch (id)
      {
        case "QM.X371":
          if (this.GetField("QM.X372") == "Y")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "761":
          if (this.GetField("3941") == "Y")
          {
            ((RuntimeControl) this.currentForm.FindControl("Calendar2")).Enabled = false;
            return ControlState.Disabled;
          }
          ((RuntimeControl) this.currentForm.FindControl("Calendar2")).Enabled = true;
          break;
        case "QM.X383":
          controlState = !this.loan.Calculator.UsePriceBasedQM(Utils.ParseDate((object) this.GetField("745"))) ? ControlState.Disabled : ControlState.Enabled;
          break;
        default:
          controlState = ControlState.Default;
          break;
      }
      if (id == "QM.X374")
      {
        if (this.inputData.GetField("420") == "SecondLien")
        {
          if (this.boxMonthlyPay1 != null)
            this.boxMonthlyPay1.Title = "Monthly Payment - Second Lien / Total";
          if (this.boxMonthlyPay2 != null)
            this.boxMonthlyPay2.Title = "Monthly Payment - First Lien / Monthly Housing Obligations";
          if (this.labelFor3290 != null)
            this.labelFor3290.Text = "Max Second P & I";
          if (this.labelForX373 != null)
            this.labelForX373.Text = "Max Second P & I";
          if (this.labelForX337 != null)
            this.labelForX337.Text = "Max Second P & I";
        }
        else
        {
          if (this.boxMonthlyPay1 != null)
            this.boxMonthlyPay1.Title = "Monthly Payment - First Lien / Total";
          if (this.boxMonthlyPay2 != null)
            this.boxMonthlyPay2.Title = "Monthly Payment - Second Lien / Monthly Housing Obligations";
          if (this.labelFor3290 != null)
            this.labelFor3290.Text = "Max First P & I";
          if (this.labelForX373 != null)
            this.labelForX373.Text = "Max First P & I";
          if (this.labelForX337 != null)
            this.labelForX337.Text = "Max First P & I";
        }
        if (this.inputData.GetField("1811") == "SecondHome" || this.inputData.GetField("1811") == "Investor")
        {
          if (this.panelSecondHome != null)
            this.panelSecondHome.Visible = true;
          if (this.panelPrimaryHome != null)
            this.panelPrimaryHome.Visible = false;
          if (this.inputData.GetField("420") == "SecondLien")
          {
            if (this.labelFor229 != null)
              this.labelFor229.Text = "First P & I";
            if (this.l_228 != null)
              this.l_228.Visible = true;
            if (this.l_229 != null)
              this.l_229.Visible = false;
          }
          else
          {
            if (this.labelFor229 != null)
              this.labelFor229.Text = "Second P & I";
            if (this.l_228 != null)
              this.l_228.Visible = false;
            if (this.l_229 != null)
              this.l_229.Visible = true;
          }
        }
        else
        {
          if (this.panelSecondHome != null)
            this.panelSecondHome.Visible = false;
          if (this.panelPrimaryHome != null)
            this.panelPrimaryHome.Visible = true;
          if (this.inputData.GetField("420") == "SecondLien")
          {
            if (this.labelFor1725 != null)
              this.labelFor1725.Text = "First P & I";
            if (this.l_1724 != null && this.fl_1724 != null)
              this.fl_1724.Visible = this.l_1724.Visible = true;
            if (this.l_1725 != null && this.fl_1725 != null)
              this.fl_1725.Visible = this.l_1725.Visible = false;
          }
          else
          {
            if (this.labelFor1725 != null)
              this.labelFor1725.Text = "Second P & I";
            if (this.l_1724 != null && this.fl_1724 != null)
              this.fl_1724.Visible = this.l_1724.Visible = false;
            if (this.l_1725 != null && this.fl_1725 != null)
              this.fl_1725.Visible = this.l_1725.Visible = true;
          }
        }
      }
      return controlState;
    }

    internal override void UpdateFieldValue(string id, string val)
    {
      base.UpdateFieldValue(id, val);
      if (this.loan == null || this.loan.Calculator == null)
        return;
      this.loan.Calculator.CalcOnDemand(CalcOnDemandEnum.PaymentSchedule);
    }

    public override void ExecAction(string action)
    {
      if (action == "feedetails")
      {
        if (this.loan != null && this.loan.Use2015RESPA)
          this.LoadQuickLinkForm("2015 Itemization", "REGZGFE_2015", 800, 700, FieldSource.CurrentLoan, "2015 Itemization", false);
        else
          this.LoadQuickLinkForm("2010 Itemization", "REGZGFE_2010", 750, 600, FieldSource.CurrentLoan, "2010 Itemization", false);
      }
      else
        base.ExecAction(action);
      switch (action)
      {
        case "feedetails":
          this.SetFieldFocus("l_X120");
          break;
        case "getresidualincome":
          this.SetFieldFocus("l_1340");
          break;
      }
    }

    protected override void UpdateContents(
      bool refreshAllFields,
      bool fireChangeEvents,
      bool skipButtonFieldLockRules)
    {
      base.UpdateContents(refreshAllFields, fireChangeEvents, skipButtonFieldLockRules);
      this.setBuydownSellerPaidFeeLayout(true);
    }

    public override void RefreshContents(bool skipButtonFieldLockRules)
    {
      base.RefreshContents(skipButtonFieldLockRules);
      this.setBuydownSellerPaidFeeLayout(false);
    }

    private void setBuydownSellerPaidFeeLayout(bool disabledOnly)
    {
      ATR_QUALIFICATIONInputHandler.SetBuydownSellerPaidField(this.inputData, this.panelSec32SellerPaid, this.l_QMX378, this.fl_QMX378, disabledOnly);
    }

    internal static void SetBuydownSellerPaidField(
      IHtmlInput inputData,
      EllieMae.Encompass.Forms.Panel panelSec32SellerPaid,
      EllieMae.Encompass.Forms.TextBox l_QMX378,
      FieldLock fl_QMX378,
      bool disabledOnly)
    {
      if (panelSec32SellerPaid == null)
        return;
      if (inputData.GetField("COMPLIANCEVERSION.CASASRNX141") == "Y" || inputData.GetField("COMPLIANCEVERSION.NEWBUYDOWNENABLED") != "Y")
      {
        panelSec32SellerPaid.Visible = false;
      }
      else
      {
        panelSec32SellerPaid.Visible = true;
        bool flag = inputData.GetField("CASASRN.X141") == "Seller";
        if (!flag)
          l_QMX378.Enabled = flag;
        if (!flag & disabledOnly)
          fl_QMX378.Enabled = false;
        else
          fl_QMX378.Enabled = flag;
      }
    }
  }
}
