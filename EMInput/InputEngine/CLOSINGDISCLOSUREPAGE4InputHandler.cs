// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.CLOSINGDISCLOSUREPAGE4InputHandler
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
  public class CLOSINGDISCLOSUREPAGE4InputHandler : InputHandlerBase
  {
    private AIRInputHandler airInputHandler;
    private APInputHandler apInputHandler;
    private EllieMae.Encompass.Forms.CheckBox chkYouDeclinedIt;
    private EllieMae.Encompass.Forms.CheckBox chkLenderDoesntOffer;
    private EllieMae.Encompass.Forms.Label labelEscrow;
    private FieldReformatOnUIHandler fieldReformatOnUIHandler;

    public CLOSINGDISCLOSUREPAGE4InputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public CLOSINGDISCLOSUREPAGE4InputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public CLOSINGDISCLOSUREPAGE4InputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public CLOSINGDISCLOSUREPAGE4InputHandler(
      Sessions.Session session,
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, dataTemplate, htmldoc, form, property)
    {
    }

    public CLOSINGDISCLOSUREPAGE4InputHandler(
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
      try
      {
        this.fieldReformatOnUIHandler = new FieldReformatOnUIHandler(this.inputData);
        this.labelEscrow = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("LabelEscrow");
        this.chkYouDeclinedIt = (EllieMae.Encompass.Forms.CheckBox) this.currentForm.FindControl("chkYouDeclinedIt");
        this.chkLenderDoesntOffer = (EllieMae.Encompass.Forms.CheckBox) this.currentForm.FindControl("chkLenderDoesntOffer");
        this.airInputHandler = new AIRInputHandler(this.currentForm, this.inputData);
        this.airInputHandler.SetSectionStatus();
        this.apInputHandler = new APInputHandler(this.currentForm, this.inputData);
        this.apInputHandler.SetSectionStatus();
        if (this.loan == null || this.loan.Calculator == null)
          return;
        this.loan.Calculator.CalculateProjectedPaymentTable();
        this.loan.Calculator.FormCalculation("POPULATESUBJECTPROPERTYADDRESS", (string) null, (string) null);
      }
      catch (Exception ex)
      {
      }
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      if (this.inputData is DisclosedCDHandler)
        return ControlState.Disabled;
      ControlState controlState = this.getControlState(ctrl, id, ControlState.Enabled);
      switch (id)
      {
        case "CD4.X7":
        case "DISCLOSURE.X913":
        case "DISCLOSURE.X914":
          if (this.inputData.GetField("CD4.X9") != "N")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "CD4.X31":
          controlState = !(this.inputData.GetField("608") != "AdjustableRate") ? ControlState.Enabled : ControlState.Disabled;
          break;
        case "CD4.X8":
          if (this.inputData.GetField("CD4.X9") != "N")
            this.SetControlState("FieldLock3", false);
          else
            this.SetControlState("FieldLock3", true);
          controlState = ControlState.Default;
          break;
        case "CD4.X10":
          this.labelEscrow.Text = this.GetField("423") == "Biweekly" ? "Bi-weekly Escrow Payment" : "Monthly Escrow Payment";
          controlState = ControlState.Default;
          break;
        default:
          controlState = ControlState.Default;
          break;
      }
      return controlState;
    }

    protected override string GetFieldValue(string id, FieldSource fieldSource)
    {
      string str = base.GetFieldValue(id, fieldSource);
      if (id == "CD4.X25" || id == "CD4.X27" || id == "NEWHUD.X6")
        this.apInputHandler.SetSectionStatus();
      if (this.inputData is DisclosedCDHandler)
      {
        this.chkYouDeclinedIt.Enabled = this.chkLenderDoesntOffer.Enabled = false;
        ((RuntimeControl) this.currentForm.FindControl("txtFirstChangeMix")).Enabled = false;
        ((RuntimeControl) this.currentForm.FindControl("txtSubsequentChanges")).Enabled = false;
        ((RuntimeControl) this.currentForm.FindControl("txtMaxPayAmt")).Enabled = false;
      }
      if (id == "674")
        str = Utils.FormatLEAndCDPercentageValue(str);
      string fieldValue = this.airInputHandler.GetFieldValue(id, str);
      return this.fieldReformatOnUIHandler.GetFieldValue(id, fieldValue);
    }

    internal override void UpdateFieldValue(string id, string val)
    {
      base.UpdateFieldValue(id, val);
      if (this.GetFieldValue("CD4.X9") == "N")
      {
        switch (id)
        {
          case "DISCLOSURE.X913":
            base.UpdateFieldValue("CD4.X7", val == "Y" ? "N" : "Y");
            break;
          case "CD4.X7":
            base.UpdateFieldValue("DISCLOSURE.X913", val == "Y" ? "N" : "Y");
            break;
        }
      }
      if (!(id == "CD4.X9") || !(val == "Y") && !(val == ""))
        return;
      base.UpdateFieldValue("CD4.X7", "N");
      base.UpdateFieldValue("DISCLOSURE.X913", "N");
    }
  }
}
