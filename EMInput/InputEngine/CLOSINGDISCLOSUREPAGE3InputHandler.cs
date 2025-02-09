// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.CLOSINGDISCLOSUREPAGE3InputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.Forms;
using mshtml;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class CLOSINGDISCLOSUREPAGE3InputHandler : InputHandlerBase
  {
    private EllieMae.Encompass.Forms.Panel panelform;
    private CategoryBox CatergoryBoxCashToClose;
    private CategoryBox CategoryBoxSumariesofTransaction;
    private CategoryBox CategoryBoxPayoffAndPayment;
    private EllieMae.Encompass.Forms.Label LabelFormName;
    private EllieMae.Encompass.Forms.Panel panelCashToClose;
    private EllieMae.Encompass.Forms.Panel panelAlternateCashToClose;
    private EllieMae.Encompass.Forms.Panel panelAltCashToClose1;
    private EllieMae.Encompass.Forms.Panel panelAltCashToClose2;
    private EllieMae.Encompass.Forms.Panel panelAltLoanAmount;
    private EllieMae.Encompass.Forms.Panel panelAltClosingCostBeforeClosing;
    private EllieMae.Encompass.Forms.Panel panelAltTotalPayoffs;
    private EllieMae.Encompass.Forms.Panel panelAltCashToClose;
    private EllieMae.Encompass.Forms.Panel panelSTDCashToClose1;
    private EllieMae.Encompass.Forms.Panel panelSTDCashToClose2;
    private EllieMae.Encompass.Forms.Panel panelSTDClosingCostBeforeClosing;
    private EllieMae.Encompass.Forms.Panel panelSTDClosingCostFinanced;
    private EllieMae.Encompass.Forms.Panel panelSTDDownPayment;
    private EllieMae.Encompass.Forms.Panel panelSTDDeposit;
    private EllieMae.Encompass.Forms.Panel panelSTDFundsForBorrower;
    private EllieMae.Encompass.Forms.Panel panelSTDSellerCredits;
    private EllieMae.Encompass.Forms.Panel panelSTDAdjustments;
    private EllieMae.Encompass.Forms.Panel panelSTDSellerCreditsNew;

    public CLOSINGDISCLOSUREPAGE3InputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public CLOSINGDISCLOSUREPAGE3InputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public CLOSINGDISCLOSUREPAGE3InputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public CLOSINGDISCLOSUREPAGE3InputHandler(
      Sessions.Session session,
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, dataTemplate, htmldoc, form, property)
    {
    }

    public CLOSINGDISCLOSUREPAGE3InputHandler(
      Sessions.Session session,
      IWin32Window owner,
      IHtmlInput htmlInput,
      HTMLDocument htmlDoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, htmlInput, htmlDoc, form, property)
    {
    }

    internal override void CreateControls()
    {
      this.panelCashToClose = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelCashToClose");
      this.panelAlternateCashToClose = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelAlternateCashToClose");
      this.panelform = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlForm");
      this.CategoryBoxSumariesofTransaction = (CategoryBox) this.currentForm.FindControl("CategoryBoxSumariesOfTransaction");
      this.CategoryBoxPayoffAndPayment = (CategoryBox) this.currentForm.FindControl("ControlBoxPaymentsAndPayoffs");
      this.CatergoryBoxCashToClose = (CategoryBox) this.currentForm.FindControl("CategoryBox1");
      this.LabelFormName = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("LabelFormName");
      this.panelAltCashToClose1 = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelAltTotalClosingCost1");
      this.panelAltCashToClose2 = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelAltTotalClosingCost2");
      this.panelAltLoanAmount = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelAltLoanAmount");
      this.panelAltClosingCostBeforeClosing = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelAltClosingCostBeforeClosing");
      this.panelAltTotalPayoffs = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelAltTotalPayoffs");
      this.panelAltCashToClose = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelAltCashToClose");
      this.panelSTDCashToClose1 = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelSTDTotalClosingCost1");
      this.panelSTDCashToClose2 = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelSTDTotalClosingCost2");
      this.panelSTDClosingCostBeforeClosing = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelSTDClosingCostBeforeClosing");
      this.panelSTDClosingCostFinanced = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelSTDClosingCostFinanced");
      this.panelSTDDownPayment = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelSTDDownPayment");
      this.panelSTDDeposit = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelSTDDeposit");
      this.panelSTDFundsForBorrower = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelSTDFundsForBorrower");
      this.panelSTDSellerCredits = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelSTDSellerCredits");
      this.panelSTDAdjustments = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelSTDAdjustments");
      this.panelSTDSellerCreditsNew = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelSTDSellerCreditsNew");
      this.refreshAlternative();
      if (this.loan != null && this.loan.Calculator != null)
        this.loan.Calculator.FormCalculation("CLOSINGDISCLOSUREPAGE3", "", "");
      this.hideStdCashtoCloseRemarks();
      this.hideAltCashtoCloseRemarks();
      this.showOldOrNewSellerCreditVerbiage();
      if (this.loan == null || !this.loan.IsTemplate)
        return;
      ((RuntimeControl) this.currentForm.FindControl("Button4")).Enabled = false;
    }

    public override void RefreshContents(bool skipButtonFieldLockRules)
    {
      base.RefreshContents(skipButtonFieldLockRules);
      this.refreshPopupIcons();
    }

    protected override void UpdateContents(bool refreshAllFields)
    {
      base.UpdateContents(refreshAllFields);
      this.refreshPopupIcons();
    }

    public override void RefreshContents()
    {
      base.RefreshContents();
      this.refreshAlternative();
      this.hideStdCashtoCloseRemarks();
      this.showOldOrNewSellerCreditVerbiage();
      this.hideAltCashtoCloseRemarks();
    }

    private void refreshPopupIcons()
    {
      if (!(this.inputData is DisclosedCDHandler))
        return;
      this.SetControlState("StdBtnAdjustments", true);
      this.SetControlState("StandardButton1", true);
    }

    private void refreshAlternative()
    {
      if (this.inputData.GetField("LE2.X28") == "Y")
      {
        this.CategoryBoxPayoffAndPayment.Visible = true;
        this.panelAlternateCashToClose.Visible = true;
        this.panelCashToClose.Visible = false;
        this.CategoryBoxSumariesofTransaction.Visible = false;
        this.CategoryBoxPayoffAndPayment.Position = new Point(2, 300);
        this.panelform.Size = new Size(this.panelform.Size.Width, 1000);
        this.LabelFormName.Position = new Point(6, 775);
        this.panelAlternateCashToClose.Position = new Point(this.panelCashToClose.Left, 27);
      }
      else
      {
        this.CategoryBoxPayoffAndPayment.Visible = false;
        this.panelAlternateCashToClose.Visible = false;
        this.panelCashToClose.Visible = true;
        this.CategoryBoxSumariesofTransaction.Visible = true;
        this.panelform.Size = new Size(this.panelform.Size.Width, 1483);
        this.LabelFormName.Position = new Point(6, 1487);
      }
    }

    private void showLenderCredit(string panel1, string panel2, string panel3)
    {
      EllieMae.Encompass.Forms.Panel control1 = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl(panel1);
      EllieMae.Encompass.Forms.Panel control2 = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl(panel3);
      EllieMae.Encompass.Forms.Panel control3 = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl(panel2);
      double num1 = Utils.ParseDouble((object) this.GetField("FV.X366"));
      double num2 = Utils.ParseDouble((object) this.GetField("FV.X396"));
      double num3 = Utils.ParseDouble((object) this.GetField("FV.X397"));
      control2.Visible = control3.Visible = control1.Visible = false;
      if (num1 > 0.0)
        control1.Visible = true;
      if (num2 > 0.0)
      {
        control3.Visible = true;
        if (!control1.Visible)
          control3.Position = control1.Position;
      }
      if (num3 <= 0.0)
        return;
      control2.Visible = true;
      if (!control3.Visible)
        control2.Position = control3.Position;
      if (control1.Visible)
        return;
      control2.Position = control1.Position;
    }

    private void showOldOrNewSellerCreditVerbiage()
    {
      if (this.inputData is DisclosedCDHandler)
      {
        if (this.GetField("CD3.X116") == "YES")
        {
          if (this.inputData.GetField("CD3.X1542") == "Y")
          {
            this.panelSTDSellerCreditsNew.Visible = true;
            this.panelSTDSellerCreditsNew.Position = this.panelSTDSellerCredits.Position;
            this.panelSTDSellerCredits.Visible = false;
          }
          else
          {
            this.panelSTDSellerCreditsNew.Visible = false;
            this.panelSTDSellerCredits.Visible = true;
          }
        }
        else
        {
          this.panelSTDSellerCreditsNew.Visible = false;
          this.panelSTDSellerCredits.Visible = false;
        }
      }
      else if (this.GetField("CD3.X116") == "YES")
      {
        this.panelSTDSellerCreditsNew.Visible = true;
        this.panelSTDSellerCreditsNew.Position = this.panelSTDSellerCredits.Position;
        this.panelSTDSellerCredits.Visible = false;
      }
      else
      {
        this.panelSTDSellerCreditsNew.Visible = false;
        this.panelSTDSellerCredits.Visible = false;
      }
    }

    private void hideStdCashtoCloseRemarks()
    {
      if (this.GetField("CD3.X111") == "YES")
      {
        this.panelSTDCashToClose1.Visible = true;
        this.panelSTDCashToClose2.Visible = true;
        this.showLenderCredit("stdClsPnl1", "stdClsPnl2", "stdClsPnl3");
      }
      else
      {
        this.panelSTDCashToClose1.Visible = false;
        this.panelSTDCashToClose2.Visible = false;
      }
      if (this.GetField("CD3.X112") == "YES")
        this.panelSTDClosingCostBeforeClosing.Visible = true;
      else
        this.panelSTDClosingCostBeforeClosing.Visible = false;
      if (this.GetField("CD3.X134") == "YES")
        this.panelSTDClosingCostFinanced.Visible = true;
      else
        this.panelSTDClosingCostFinanced.Visible = false;
      if (this.GetField("CD3.X113") == "YES")
        this.panelSTDDownPayment.Visible = true;
      else
        this.panelSTDDownPayment.Visible = false;
      if (this.GetField("CD3.X114") == "YES")
        this.panelSTDDeposit.Visible = true;
      else
        this.panelSTDDeposit.Visible = false;
      if (this.GetField("CD3.X115") == "YES")
        this.panelSTDFundsForBorrower.Visible = true;
      else
        this.panelSTDFundsForBorrower.Visible = false;
      if (this.GetField("CD3.X117") == "YES")
        this.panelSTDAdjustments.Visible = true;
      else
        this.panelSTDAdjustments.Visible = false;
    }

    private void hideAltCashtoCloseRemarks()
    {
      if (this.GetField("CD3.X118") == "YES")
        this.panelAltLoanAmount.Visible = true;
      else
        this.panelAltLoanAmount.Visible = false;
      if (this.GetField("CD3.X119") == "YES")
      {
        this.panelAltCashToClose1.Visible = true;
        this.panelAltCashToClose2.Visible = true;
        this.showLenderCredit("altClsPnl1", "altClsPnl2", "altClsPnl3");
      }
      else
      {
        this.panelAltCashToClose1.Visible = false;
        this.panelAltCashToClose2.Visible = false;
      }
      if (this.GetField("CD3.X120") == "YES")
        this.panelAltClosingCostBeforeClosing.Visible = true;
      else
        this.panelAltClosingCostBeforeClosing.Visible = false;
      if (this.GetField("CD3.X121") == "YES")
        this.panelAltTotalPayoffs.Visible = true;
      else
        this.panelAltTotalPayoffs.Visible = false;
      if (this.GetField("CD3.X122") == "YES")
        this.panelAltCashToClose.Visible = true;
      else
        this.panelAltCashToClose.Visible = true;
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      if (this.inputData is DisclosedCDHandler)
        return ControlState.Disabled;
      base.GetControlState(ctrl, id);
      ControlState controlState;
      switch (id)
      {
        case "CD3.X13":
          controlState = this.GetField("CD3.X312") == "Y" ? ControlState.Disabled : ControlState.Enabled;
          break;
        case "CD3.X15":
          controlState = this.GetField("CD3.X316") == "Y" ? ControlState.Disabled : ControlState.Enabled;
          break;
        case "CD3.X17":
          controlState = this.GetField("CD3.X320") == "Y" ? ControlState.Disabled : ControlState.Enabled;
          break;
        case "CD3.X2":
          controlState = this.GetField("CD3.X229") == "Y" ? ControlState.Disabled : ControlState.Enabled;
          break;
        case "CD3.X24":
          controlState = this.GetField("CD3.X517") == "Y" ? ControlState.Disabled : ControlState.Enabled;
          break;
        case "CD3.X26":
          controlState = this.GetField("CD3.X541") == "Y" ? ControlState.Disabled : ControlState.Enabled;
          break;
        case "CD3.X9":
          controlState = this.GetField("CD3.X288") == "Y" ? ControlState.Disabled : ControlState.Enabled;
          break;
        case "L81":
          controlState = this.GetField("CD3.X445") == "Y" ? ControlState.Disabled : ControlState.Enabled;
          break;
        case "L86":
          controlState = this.GetField("CD3.X469") == "Y" ? ControlState.Disabled : ControlState.Enabled;
          break;
        case "L88":
          controlState = this.GetField("CD3.X225") == "Y" ? ControlState.Disabled : ControlState.Enabled;
          break;
        case "L90":
          controlState = this.GetField("CD3.X493") == "Y" ? ControlState.Disabled : ControlState.Enabled;
          break;
        default:
          controlState = ControlState.Default;
          break;
      }
      switch (id)
      {
        case "CD3.X40":
          this.hideStdCashtoCloseRemarks();
          this.hideAltCashtoCloseRemarks();
          break;
        case "CD3.X116":
          this.showOldOrNewSellerCreditVerbiage();
          break;
      }
      return controlState;
    }

    internal void SetFundingControls()
    {
      this.CatergoryBoxCashToClose.Visible = false;
      if (this.CategoryBoxPayoffAndPayment.Visible)
        this.CategoryBoxPayoffAndPayment.Top = this.CatergoryBoxCashToClose.Top;
      else
        this.CategoryBoxSumariesofTransaction.Top = this.CatergoryBoxCashToClose.Top;
      int num1 = 1;
      int num2 = 67;
      if (this.CategoryBoxPayoffAndPayment.Visible)
      {
        num1 = 68;
        num2 = 92;
      }
      for (int index = num1; index <= num2; ++index)
      {
        DropdownBox control1 = (DropdownBox) this.currentForm.FindControl("cboCD" + (object) index);
        if (control1 != null)
          control1.Visible = true;
        if (!this.CategoryBoxPayoffAndPayment.Visible)
        {
          EllieMae.Encompass.Forms.TextBox control2 = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("TextBoxCD" + (object) index);
          if (control2 != null)
            control2.Size = new Size(100, control2.Size.Height);
        }
      }
      EllieMae.Encompass.Forms.Panel panelform = this.panelform;
      int width = this.panelform.Size.Width;
      Size size1;
      int height1;
      if (!this.CategoryBoxPayoffAndPayment.Visible)
      {
        size1 = this.CategoryBoxSumariesofTransaction.Size;
        height1 = size1.Height;
      }
      else
      {
        size1 = this.CategoryBoxPayoffAndPayment.Size;
        height1 = size1.Height;
      }
      int height2 = height1 + 5;
      Size size2 = new Size(width, height2);
      panelform.Size = size2;
      EllieMae.Encompass.Forms.Label labelFormName = this.LabelFormName;
      size1 = this.panelform.Size;
      int num3 = size1.Height + 10;
      labelFormName.Top = num3;
    }

    internal override void UpdateFieldValue(string id, string val)
    {
      base.UpdateFieldValue(id, val);
      switch (id)
      {
        case "L88":
          this.SetField("CD3.X223", val);
          break;
        case "L89":
          this.SetField("CD3.X224", val);
          break;
        case "CD3.X2":
          this.SetField("CD3.X227", val);
          break;
        case "CD3.X3":
          this.SetField("CD3.X228", val);
          break;
      }
      switch (id)
      {
        case "CD3.X10":
          this.SetField("CD3.X286", val);
          break;
        case "CD3.X13":
          this.SetField("CD3.X310", val);
          break;
        case "CD3.X14":
          this.SetField("CD3.X311", val);
          break;
        case "CD3.X15":
          this.SetField("CD3.X314", val);
          break;
        case "CD3.X16":
          this.SetField("CD3.X315", val);
          break;
        case "CD3.X17":
          this.SetField("CD3.X318", val);
          break;
        case "CD3.X18":
          this.SetField("CD3.X319", val);
          break;
        case "CD3.X9":
          this.SetField("CD3.X285", val);
          break;
      }
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(id))
      {
        case 3684098744:
          if (!(id == "CD3.X25"))
            break;
          this.SetField("CD3.X516", val);
          break;
        case 3700876363:
          if (!(id == "CD3.X24"))
            break;
          this.SetField("CD3.X515", val);
          break;
        case 3717653982:
          if (!(id == "CD3.X27"))
            break;
          this.SetField("CD3.X540", val);
          break;
        case 3734431601:
          if (!(id == "CD3.X26"))
            break;
          this.SetField("CD3.X539", val);
          break;
        case 3865122578:
          if (!(id == "L90"))
            break;
          this.SetField("CD3.X491", val);
          break;
        case 3881900197:
          if (!(id == "L91"))
            break;
          this.SetField("CD3.X492", val);
          break;
        case 3949157768:
          if (!(id == "L81"))
            break;
          this.SetField("CD3.X443", val);
          break;
        case 3999490625:
          if (!(id == "L82"))
            break;
          this.SetField("CD3.X444", val);
          break;
        case 4049823482:
          if (!(id == "L87"))
            break;
          this.SetField("CD3.X468", val);
          break;
        case 4066601101:
          if (!(id == "L86"))
            break;
          this.SetField("CD3.X467", val);
          break;
      }
    }
  }
}
