// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.EEMCALCULATIONInputHandler
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
  public class EEMCALCULATIONInputHandler : InputHandlerBase
  {
    public EEMCALCULATIONInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public EEMCALCULATIONInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public EEMCALCULATIONInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public EEMCALCULATIONInputHandler(
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
        case "11":
          return controlState;
        case "EEM.X65":
          if (this.loan.GetField("MORNET.X40") == "StreamlineWithAppraisal" || this.loan.GetField("MORNET.X40") == "StreamlineWithoutAppraisal" || this.loan.GetField("1067") == "ProposedConstruction")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "EEM.X66":
          if (this.loan.GetField("1067") != "NewConstruction")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "EEM.X67":
          if (this.loan.GetField("1067") != "ExistingConstruction")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "EEM.X85":
          if (this.loan.GetField("1198") != string.Empty && this.loan.GetField("1199") != string.Empty)
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "MAX23K.X40":
          if (this.loan.GetField("1172") == "FHA")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "MAX23K.X75":
          if (this.loan.GetField("19") == "Purchase" || this.loan.GetField("19") == "NoCash-Out Refinance" || this.loan.GetField("19") == "Cash-Out Refinance")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        default:
          controlState = ControlState.Default;
          goto case "11";
      }
    }

    public override void ExecAction(string action)
    {
      base.ExecAction(action);
      switch (action)
      {
        case "copyeemx75":
          this.SetFieldFocus("l_EEMX75");
          break;
        case "copyeemx77":
          this.SetFieldFocus("l_EEMX75A");
          break;
      }
    }
  }
}
