// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.USDA_RURAL6InputHandler
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
  internal class USDA_RURAL6InputHandler : InputHandlerBase
  {
    public USDA_RURAL6InputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public USDA_RURAL6InputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public USDA_RURAL6InputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public USDA_RURAL6InputHandler(
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
        case "110":
        case "111":
        case "112":
        case "113":
        case "116":
        case "117":
          return controlState;
        case "USDA.X11":
        case "USDA.X45":
          if (this.GetFieldValue("USDA.X10") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        case "USDA.X14":
        case "USDA.X47":
          if (this.GetFieldValue("USDA.X13") != "Y")
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
        case "vor":
          if (this.FormIsForTemplate)
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
  }
}
