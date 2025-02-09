// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.HUD1PG1InputHandler
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
  public class HUD1PG1InputHandler : InputHandlerBase
  {
    public HUD1PG1InputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public HUD1PG1InputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
      if (!(this.loan.GetSimpleField("L83") == ""))
        return;
      this.loan.SetCurrentField("L83", this.loan.GetSimpleField("1172"));
    }

    public HUD1PG1InputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public HUD1PG1InputHandler(
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
        case "364":
          if (this.FormIsForTemplate)
          {
            controlState = ControlState.Disabled;
            goto case "FR0104";
          }
          else
            goto case "FR0104";
        case "L206":
          EllieMae.Encompass.Forms.Label control1 = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("lblBorFromTo");
          if (control1 != null)
          {
            control1.Text = this.ToDouble(this.loan.GetSimpleField("L206R")) < 0.0 ? "Cash to Borrower" : "Cash from Borrower";
            goto case "FR0104";
          }
          else
            goto case "FR0104";
        case "L207":
          EllieMae.Encompass.Forms.Label control2 = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("lblSelFromTo");
          if (control2 != null)
          {
            control2.Text = this.ToDouble(this.loan.GetSimpleField("L207R")) < 0.0 ? "Cash from Seller" : "Cash to Seller";
            goto case "FR0104";
          }
          else
            goto case "FR0104";
        case "FR0104":
        case "11":
          return controlState;
        default:
          controlState = ControlState.Default;
          goto case "FR0104";
      }
    }
  }
}
