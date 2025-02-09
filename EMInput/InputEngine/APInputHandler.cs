// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.APInputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.DataEngine;
using EllieMae.Encompass.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class APInputHandler
  {
    private LoanData loan;
    private IHtmlInput inputData;
    private Form currentForm;
    private TextBox txtInterestPay;
    private TextBox txtOptionalPay;
    private TextBox txtFirstChangeMix;
    private TextBox txtSubsequentChanges;
    private TextBox txtMaxPayAmt;
    private Label lblPrincipalIntPayments;

    public APInputHandler(Form currentForm, IHtmlInput inputData)
    {
      this.inputData = inputData;
      if (inputData is LoanData)
        this.loan = (LoanData) inputData;
      this.currentForm = currentForm;
      this.txtInterestPay = (TextBox) this.currentForm.FindControl(nameof (txtInterestPay));
      this.txtOptionalPay = (TextBox) this.currentForm.FindControl(nameof (txtOptionalPay));
      this.txtFirstChangeMix = (TextBox) this.currentForm.FindControl(nameof (txtFirstChangeMix));
      this.txtSubsequentChanges = (TextBox) this.currentForm.FindControl(nameof (txtSubsequentChanges));
      this.txtMaxPayAmt = (TextBox) this.currentForm.FindControl(nameof (txtMaxPayAmt));
      this.lblPrincipalIntPayments = (Label) this.currentForm.FindControl(nameof (lblPrincipalIntPayments));
    }

    public void SetSectionStatus()
    {
      this.txtInterestPay.Enabled = false;
      this.txtOptionalPay.Enabled = false;
      this.lblPrincipalIntPayments.Text = this.inputData.GetField("423") == "Biweekly" ? "Biweekly Principal and Interest Payments" : "Monthly Principal and Interest Payments";
    }

    public void ChangeMonthlyBiweeklyLabel()
    {
      this.lblPrincipalIntPayments.Text = this.inputData.GetField("423") == "Biweekly" ? "Biweekly Principal and Interest Payments" : "Monthly Principal and Interest Payments";
    }
  }
}
