// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.PaymentEscrowInputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.Forms;
using mshtml;
using System;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  internal class PaymentEscrowInputHandler
  {
    private IHtmlInput inputData;
    private LoanData loan;
    private InputHandlerBase currentHandler;
    private Sessions.Session session = Session.DefaultInstance;
    private Form currentForm;
    private Image imageForPaymentEscrow;
    private Panel panelPaymentEscrow;
    private Label lblTaxEscrow;
    private Label lblHazardInsurance;
    private Label lblMortgageInsurance;
    private Label lblFloodInsurance;
    private Label lblCityPropertyTax;
    private Label lblEscrowOther1;
    private Label lblEscrowOther2;
    private Label lblEscrowOther3;
    private Label lblUSDAMonthlyPreimum;
    private Label lblTE;
    private Label lblHI;
    private Label lblMI;
    private Label lblFI;
    private Label lblCPT;
    private Label lblEO1;
    private Label lblEO2;
    private Label lblEO3;
    private Label lblUMP;

    public PaymentEscrowInputHandler(
      IHtmlInput inputData,
      Form currentForm,
      InputHandlerBase currentHandler,
      Sessions.Session session)
    {
      this.inputData = inputData;
      this.session = session;
      if (this.inputData is LoanData)
        this.loan = (LoanData) this.inputData;
      this.currentForm = currentForm;
      this.currentHandler = currentHandler;
      this.imageForPaymentEscrow = (Image) this.currentForm.FindControl("imgPaymentEscrow");
      this.panelPaymentEscrow = (Panel) this.currentForm.FindControl(nameof (panelPaymentEscrow));
      this.lblTaxEscrow = (Label) this.currentForm.FindControl("lblTax");
      this.lblHazardInsurance = (Label) this.currentForm.FindControl(nameof (lblHazardInsurance));
      this.lblMortgageInsurance = (Label) this.currentForm.FindControl(nameof (lblMortgageInsurance));
      this.lblFloodInsurance = (Label) this.currentForm.FindControl(nameof (lblFloodInsurance));
      this.lblCityPropertyTax = (Label) this.currentForm.FindControl(nameof (lblCityPropertyTax));
      this.lblEscrowOther1 = (Label) this.currentForm.FindControl(nameof (lblEscrowOther1));
      this.lblEscrowOther2 = (Label) this.currentForm.FindControl(nameof (lblEscrowOther2));
      this.lblEscrowOther3 = (Label) this.currentForm.FindControl(nameof (lblEscrowOther3));
      this.lblUSDAMonthlyPreimum = (Label) this.currentForm.FindControl(nameof (lblUSDAMonthlyPreimum));
      this.lblTE = (Label) this.currentForm.FindControl("Label68");
      this.lblHI = (Label) this.currentForm.FindControl("Label74");
      this.lblMI = (Label) this.currentForm.FindControl("Label76");
      this.lblFI = (Label) this.currentForm.FindControl("Label78");
      this.lblCPT = (Label) this.currentForm.FindControl("Label80");
      this.lblEO1 = (Label) this.currentForm.FindControl("Label121");
      this.lblEO2 = (Label) this.currentForm.FindControl("Label123");
      this.lblEO3 = (Label) this.currentForm.FindControl("Label75");
      this.lblUMP = (Label) this.currentForm.FindControl("Label77");
      this.imageForPaymentEscrow.ZIndex = this.panelPaymentEscrow.ZIndex = "-1";
      this.lblTaxEscrow.ZIndex = this.lblHazardInsurance.ZIndex = this.lblFloodInsurance.ZIndex = this.lblMortgageInsurance.ZIndex = this.lblCityPropertyTax.ZIndex = this.lblEscrowOther1.ZIndex = "-1";
      this.lblEscrowOther2.ZIndex = this.lblEscrowOther3.ZIndex = this.lblUSDAMonthlyPreimum.ZIndex = this.lblHI.ZIndex = this.lblMI.ZIndex = this.lblFI.ZIndex = this.lblCPT.ZIndex = this.lblEO1.ZIndex = this.lblEO2.ZIndex = this.lblEO3.ZIndex = this.lblTE.ZIndex = this.lblUMP.ZIndex = "-1";
    }

    public bool TurnOnPaymentEscrowDialog(IHTMLEventObj pEvtObj)
    {
      try
      {
        if (pEvtObj.srcElement.id.Equals("EscrowInfo"))
        {
          if (this.imageForPaymentEscrow.ZIndex != "-1")
          {
            this.TurnOFFPaymentEscrowDialog();
            return true;
          }
        }
      }
      catch (Exception ex)
      {
        return true;
      }
      RuntimeControl controlForElement = this.currentForm.FindControlForElement(pEvtObj.srcElement) as RuntimeControl;
      this.imageForPaymentEscrow.Visible = true;
      this.panelPaymentEscrow.Visible = true;
      this.lblTaxEscrow.Visible = this.lblHazardInsurance.Visible = this.lblFloodInsurance.Visible = true;
      this.lblMortgageInsurance.Visible = this.lblCityPropertyTax.Visible = this.lblEscrowOther1.Visible = true;
      this.lblEscrowOther2.Visible = this.lblEscrowOther3.Visible = this.lblUSDAMonthlyPreimum.Visible = true;
      this.lblHI.Visible = this.lblMI.Visible = this.lblFI.Visible = this.lblCPT.Visible = this.lblEO1.Visible = this.lblEO2.Visible = this.lblEO3.Visible = this.lblTE.Visible = this.lblUMP.Visible = true;
      this.imageForPaymentEscrow.ZIndex = this.panelPaymentEscrow.ZIndex = "5";
      this.lblTaxEscrow.ZIndex = this.lblHazardInsurance.ZIndex = this.lblFloodInsurance.ZIndex = this.lblMortgageInsurance.ZIndex = this.lblCityPropertyTax.ZIndex = this.lblEscrowOther1.ZIndex = "6";
      this.lblEscrowOther2.ZIndex = this.lblEscrowOther3.ZIndex = this.lblUSDAMonthlyPreimum.ZIndex = this.lblHI.ZIndex = this.lblMI.ZIndex = this.lblFI.ZIndex = this.lblCPT.ZIndex = this.lblEO1.ZIndex = this.lblEO2.ZIndex = this.lblEO3.ZIndex = this.lblTE.ZIndex = this.lblUMP.ZIndex = "6";
      this.lblTaxEscrow.Text = "$ " + this.loan.GetField("SERVICE.X121");
      this.lblHazardInsurance.Text = "$ " + this.loan.GetField("SERVICE.X122");
      this.lblMortgageInsurance.Text = "$ " + this.loan.GetField("SERVICE.X123");
      this.lblFloodInsurance.Text = "$ " + this.loan.GetField("SERVICE.X124");
      this.lblCityPropertyTax.Text = "$ " + this.loan.GetField("SERVICE.X125");
      this.lblEscrowOther1.Text = "$ " + this.loan.GetField("SERVICE.X126");
      this.lblEscrowOther2.Text = "$ " + this.loan.GetField("SERVICE.X127");
      this.lblEscrowOther3.Text = "$ " + this.loan.GetField("SERVICE.X128");
      this.lblUSDAMonthlyPreimum.Text = "$ " + this.loan.GetField("SERVICE.X129");
      return false;
    }

    public void TurnOFFPaymentEscrowDialog()
    {
      this.imageForPaymentEscrow.ZIndex = this.panelPaymentEscrow.ZIndex = "-1";
      this.lblTaxEscrow.ZIndex = this.lblHazardInsurance.ZIndex = this.lblFloodInsurance.ZIndex = this.lblMortgageInsurance.ZIndex = this.lblCityPropertyTax.ZIndex = this.lblEscrowOther1.ZIndex = "-1";
      this.lblEscrowOther2.ZIndex = this.lblEscrowOther3.ZIndex = this.lblUSDAMonthlyPreimum.ZIndex = this.lblHI.ZIndex = this.lblMI.ZIndex = this.lblFI.ZIndex = this.lblCPT.ZIndex = this.lblEO1.ZIndex = this.lblEO2.ZIndex = this.lblEO3.ZIndex = this.lblTE.ZIndex = this.lblUMP.ZIndex = "-1";
    }
  }
}
