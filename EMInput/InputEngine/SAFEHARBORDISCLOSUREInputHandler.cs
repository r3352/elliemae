// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.SAFEHARBORDISCLOSUREInputHandler
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
  public class SAFEHARBORDISCLOSUREInputHandler : InputHandlerBase
  {
    public SAFEHARBORDISCLOSUREInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public SAFEHARBORDISCLOSUREInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public SAFEHARBORDISCLOSUREInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public SAFEHARBORDISCLOSUREInputHandler(
      Sessions.Session session,
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, dataTemplate, htmldoc, form, property)
    {
    }

    public SAFEHARBORDISCLOSUREInputHandler(
      Sessions.Session session,
      IWin32Window owner,
      IHtmlInput htmlInput,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, htmlInput, htmldoc, form, property)
    {
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      if (this.inputData is DisclosedSafeHarborHandler)
      {
        if (id == "DISCLOSURE.X688")
        {
          foreach (RuntimeControl runtimeControl in this.currentForm.FindControlsByType(typeof (Rolodex)))
            runtimeControl.Enabled = false;
        }
        return ControlState.Disabled;
      }
      ControlState controlState = this.getControlState(ctrl, id, ControlState.Enabled);
      switch (id)
      {
        case "11":
          return controlState;
        case "DISCLOSURE.X692":
        case "DISCLOSURE.X704":
        case "DISCLOSURE.X711":
        case "DISCLOSURE.X723":
          if (this.GetField("DISCLOSURE.X689") == "Fixed" || this.GetField("DISCLOSURE.X689") == "")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "DISCLOSURE.X733":
          if (this.GetField("DISCLOSURE.X732") != "Other Option")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "copyfromsafeharbor":
          if (this.GetField("DISCLOSURE.X732") == string.Empty || this.GetField("DISCLOSURE.X732") == "Other Option" || this.loan != null && !this.loan.Calculator.IsSyncGFERequired)
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
        case "copytosafeharbor":
        case "copyfromsafeharbor":
          this.SetFieldFocus("l_X688");
          break;
        case "safeharboroption1":
          this.SetFieldFocus("l_X695");
          break;
        case "safeharboroption2":
          this.SetFieldFocus("l_X707");
          break;
        case "safeharboroption3":
          this.SetFieldFocus("l_X714");
          break;
        case "safeharboroption4":
          this.SetFieldFocus("l_X726");
          break;
      }
    }
  }
}
