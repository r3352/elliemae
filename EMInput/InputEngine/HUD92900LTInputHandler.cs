// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.HUD92900LTInputHandler
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
  public class HUD92900LTInputHandler : InputHandlerBase
  {
    private EllieMae.Encompass.Forms.Panel pnl2009;
    private EllieMae.Encompass.Forms.Panel pnl2020;

    public HUD92900LTInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public HUD92900LTInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public HUD92900LTInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public HUD92900LTInputHandler(
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
        case "101":
        case "102":
        case "103":
        case "104":
        case "107":
        case "108":
        case "11":
        case "110":
        case "111":
        case "112":
        case "113":
        case "116":
        case "117":
          return controlState;
        case "3006":
          if (this.loan.GetField("3005") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        case "3014":
          if (this.loan.GetField("3013") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        case "3021":
          if (this.loan.GetField("3020") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        case "9":
          if (this.loan.GetField("19") != "Other")
          {
            controlState = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        case "995":
        case "zoomarm":
          if (this.loan.GetField("608") != "AdjustableRate" && this.inputData.GetField("1172") != "HELOC")
          {
            controlState = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        case "MORNET.X41":
          if (this.GetField("19") == "")
          {
            controlState = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        case "baseincome":
          if (this.GetField("1825") == "2020")
          {
            controlState = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        default:
          controlState = ControlState.Default;
          goto case "101";
      }
    }

    internal override void CreateControls()
    {
      try
      {
        if (this.pnl2009 == null)
          this.pnl2009 = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnl2009");
        if (this.pnl2020 == null)
          this.pnl2020 = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnl2020");
        if (this.pnl2009 == null || this.pnl2020 == null)
          return;
        if (this.GetField("1825") == "2020")
          this.pnl2009.Visible = !(this.pnl2020.Visible = true);
        else
          this.pnl2009.Visible = !(this.pnl2020.Visible = false);
      }
      catch
      {
      }
    }
  }
}
