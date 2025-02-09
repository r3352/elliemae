// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine._NY_PREAPPInputHandler
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
  internal class _NY_PREAPPInputHandler : InputHandlerBase
  {
    public _NY_PREAPPInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public _NY_PREAPPInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public _NY_PREAPPInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public _NY_PREAPPInputHandler(
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
        case "DISCLOSURE.X110":
        case "DISCLOSURE.X111":
        case "DISCLOSURE.X112":
          if (this.GetFieldValue("DISCLOSURE.X109") != "Y")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "DISCLOSURE.X113":
          if (this.GetFieldValue("DISCLOSURE.X97") != "Y")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "DISCLOSURE.X115":
        case "DISCLOSURE.X116":
        case "DISCLOSURE.X98":
          if (this.GetFieldValue("DISCLOSURE.X114") != "Y")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "DISCLOSURE.X118":
        case "DISCLOSURE.X119":
        case "DISCLOSURE.X120":
        case "DISCLOSURE.X121":
          if (this.GetFieldValue("DISCLOSURE.X117") != "Y")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        default:
          controlState = ControlState.Default;
          break;
      }
      return controlState;
    }

    public override void ExecAction(string action)
    {
      if (!(action == "copygfe"))
        return;
      this.UpdateFieldValue("DISCLOSURE.X122", this.loan.GetSimpleField("L228"));
      this.UpdateFieldValue("DISCLOSURE.X123", this.loan.GetSimpleField("641"));
      this.UpdateFieldValue("DISCLOSURE.X124", this.loan.GetSimpleField("640"));
      this.UpdateContents();
      this.SetFieldFocus("l_DISCLOSURE_X122");
    }
  }
}
