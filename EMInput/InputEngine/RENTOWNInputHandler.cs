// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.RENTOWNInputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.Forms;
using mshtml;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class RENTOWNInputHandler : InputHandlerBase
  {
    public RENTOWNInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public RENTOWNInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public RENTOWNInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public RENTOWNInputHandler(
      Sessions.Session session,
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, dataTemplate, htmldoc, form, property)
    {
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      ControlState controlState = this.getControlState(ctrl, id, ControlState.Enabled);
      if (id == "PREQUAL.X335")
      {
        if (this.currentForm.FindControl("lblGainYears") is EllieMae.Encompass.Forms.Label control1)
          control1.Text = "Total Gain Over " + this.loan.GetSimpleField("PREQUAL.X335") + " Years";
        if (this.currentForm.FindControl("lblPmtYears") is EllieMae.Encompass.Forms.Label control2)
          control2.Text = "Total Payment Over " + this.loan.GetSimpleField("PREQUAL.X335") + " Years";
        if (this.currentForm.FindControl("lblTaxYears") is EllieMae.Encompass.Forms.Label control3)
          control3.Text = "Total Tax Savings Over " + this.loan.GetSimpleField("PREQUAL.X335") + " Years";
        if (this.currentForm.FindControl("lblHomeYears") is EllieMae.Encompass.Forms.Label control4)
          control4.Text = "Home Selling Price After " + this.loan.GetSimpleField("PREQUAL.X335") + " Years";
      }
      return controlState;
    }

    public override void ExecAction(string action)
    {
      switch (action)
      {
        case "haz":
          InsuranceDialog insuranceDialog1 = new InsuranceDialog("Hazard Insurance", this.loan, this.session);
          if (insuranceDialog1.ShowDialog((IWin32Window) this.mainScreen) == DialogResult.OK)
          {
            this.UpdateFieldValue("1750", insuranceDialog1.PriceType.ToString());
            double num = insuranceDialog1.RateFactor;
            this.UpdateFieldValue("1322", num.ToString());
            num = insuranceDialog1.Amount;
            this.UpdateFieldValue("230", num.ToString());
            this.UpdateContents();
          }
          this.SetFieldFocus("l_230");
          break;
        case "taxes":
          InsuranceDialog insuranceDialog2 = new InsuranceDialog("Taxes", this.loan, this.session);
          if (insuranceDialog2.ShowDialog((IWin32Window) this.mainScreen) == DialogResult.OK)
          {
            this.UpdateFieldValue("1751", insuranceDialog2.PriceType.ToString());
            double num = insuranceDialog2.RateFactor;
            this.UpdateFieldValue("1752", num.ToString());
            num = insuranceDialog2.Amount;
            this.UpdateFieldValue("1405", num.ToString());
            this.UpdateContents();
          }
          this.SetFieldFocus("l_1405");
          break;
        case "closingcosts":
          if (!Utils.CheckIf2015RespaTila(this.loan.GetField("3969")))
            this.loadPrequalQuickLink("Closing Costs", "PREQUAL_REGZGFE", 688, 512);
          else
            this.loadPrequalQuickLink("Closing Costs", "REGZGFE_2015", 688, 512);
          this.SetFieldFocus("l_1405");
          break;
      }
    }

    private void loadPrequalQuickLink(
      string formTitle,
      string htmFile,
      int sizeWidth,
      int sizeHeight)
    {
      if (this.getAllowedForms().ContainsKey((object) htmFile.ToUpper().ToLower()) || this.session.UserInfo.IsSuperAdministrator())
      {
        using (QuickEntryPopupDialog entryPopupDialog = new QuickEntryPopupDialog((IHtmlInput) this.loan, formTitle, new InputFormInfo(htmFile, htmFile), sizeWidth, sizeHeight, FieldSource.CurrentLoan, "", this.session))
        {
          int num = (int) entryPopupDialog.ShowDialog((IWin32Window) Session.MainForm);
        }
        this.loan.Calculator.FormCalculation("RENTOWN", (string) null, (string) null);
        this.editor.RefreshContents();
      }
      else if (Utils.CheckIf2015RespaTila(this.loan.GetField("3969")))
      {
        int num1 = (int) Utils.Dialog((IWin32Window) null, "You don't have user's rights to access 2015 Itemization input form.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        int num2 = (int) Utils.Dialog((IWin32Window) null, "You don't have user's rights to access Closing Costs input form.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }
  }
}
