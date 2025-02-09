// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.NETTANGIBLEBENEFITInputHandler
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
  public class NETTANGIBLEBENEFITInputHandler : InputHandlerBase
  {
    public NETTANGIBLEBENEFITInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public NETTANGIBLEBENEFITInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public NETTANGIBLEBENEFITInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public NETTANGIBLEBENEFITInputHandler(
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
        case "1948":
        case "2830":
          if (this.loan.GetField("675") != "Y")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "NTB.X14":
        case "NTB.X26":
        case "NTB.X6":
          if (this.loan.GetField("NTB.X13") != "Y")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "NTB.X31":
          if (this.loan.GetField("NTB.X30") != "Y")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "NTB.X36":
          if (this.loan.GetField("NTB.X35") != "Y")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "RE88395.X316":
          if (this.loan.GetField("675") != "Y")
          {
            controlState = ControlState.Disabled;
            this.SetControlState("FieldLock_X316", false);
            break;
          }
          controlState = ControlState.Default;
          this.SetControlState("FieldLock_X316", true);
          break;
        default:
          controlState = ControlState.Default;
          break;
      }
      return controlState;
    }
  }
}
