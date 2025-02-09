// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.VATOOL_BORROWERINFOInputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.Forms;
using mshtml;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  internal class VATOOL_BORROWERINFOInputHandler : InputHandlerBase
  {
    public VATOOL_BORROWERINFOInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public VATOOL_BORROWERINFOInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public VATOOL_BORROWERINFOInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public VATOOL_BORROWERINFOInputHandler(
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
      switch (id)
      {
        case "1063":
          return controlState;
        case "1064":
          if (this.inputData.GetField("1497") != "Other")
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "1266":
        case "1267":
          if (this.loan.GetField("608") != "GraduatedPaymentMortgage")
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "364":
          if (this.FormIsForTemplate || InputHandlerBase.LockLoanNumber)
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "432":
        case "761":
        case "762":
          if (this.GetField("3941") == "Y")
          {
            ((RuntimeControl) this.currentForm.FindControl("Calendar3")).Enabled = false;
            ((RuntimeControl) this.currentForm.FindControl("Calendar4")).Enabled = false;
            return ControlState.Disabled;
          }
          ((RuntimeControl) this.currentForm.FindControl("Calendar3")).Enabled = true;
          ((RuntimeControl) this.currentForm.FindControl("Calendar4")).Enabled = true;
          goto case "1063";
        case "9":
          if (this.loan.GetField("19") != "Other")
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "994":
          if (this.loan.GetField("608") != "OtherAmortizationType")
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "995":
        case "zoomarm":
          if (this.loan.GetField("608") != "AdjustableRate" && this.inputData.GetField("1172") != "HELOC")
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "accesslenders":
        case "productandpricing":
          if (this.loan.IsTemplate)
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        default:
          controlState = ControlState.Default;
          goto case "1063";
      }
    }

    internal override void UpdateFieldValue(string id, string val)
    {
      base.UpdateFieldValue(id, val);
      if (!(id == "VASUMM.X31"))
        return;
      this.loan.Calculator.UpdateAccountName("VA");
    }

    public override void ExecAction(string action) => base.ExecAction(action);
  }
}
