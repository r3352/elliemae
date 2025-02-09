// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.USDA_SFHLOANInputHandler
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
  internal class USDA_SFHLOANInputHandler : InputHandlerBase
  {
    public USDA_SFHLOANInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public USDA_SFHLOANInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public USDA_SFHLOANInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public USDA_SFHLOANInputHandler(
      Sessions.Session session,
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, dataTemplate, htmldoc, form, property)
    {
    }

    internal override void UpdateFieldValue(string id, string val)
    {
      base.UpdateFieldValue(id, val);
      if (!(id == "2400") || !(val != "Y"))
        return;
      base.UpdateFieldValue("762", "");
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      ControlState controlState = this.getControlState(ctrl, id, ControlState.Enabled);
      switch (id)
      {
        case "11":
          return controlState;
        case "761":
          if (this.GetFieldValue("2400") != "Y" || this.GetField("3941") == "Y")
          {
            controlState = ControlState.Disabled;
            this.SetControlState("Calendar1", false);
            goto case "11";
          }
          else if (this.loan != null && this.loan.IsFieldEditable("761"))
          {
            this.SetControlState("Calendar1", true);
            goto case "11";
          }
          else
            goto case "11";
        case "762":
          if (this.GetFieldValue("2400") != "Y" || this.GetField("3941") == "Y")
          {
            controlState = ControlState.Disabled;
            this.SetControlState("Calendar_762", false);
            goto case "11";
          }
          else if (this.loan != null && this.loan.IsFieldEditable("762"))
          {
            this.SetControlState("Calendar_762", true);
            goto case "11";
          }
          else
            goto case "11";
        case "USDA.X11":
          if (this.GetFieldValue("USDA.X10") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "USDA.X14":
          if (this.GetFieldValue("USDA.X13") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "USDA.X194":
          if (this.GetFieldValue("USDA.X12") == "Y")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "USDA.X195":
          if (this.GetFieldValue("USDA.X15") == "Y")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "USDA.X218":
          if (this.GetFieldValue("USDA.X7") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "USDA.X8":
          if (this.GetFieldValue("USDA.X7") != "Y")
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
  }
}
