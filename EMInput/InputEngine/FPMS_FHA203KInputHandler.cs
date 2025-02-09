// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.FPMS_FHA203KInputHandler
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
  public class FPMS_FHA203KInputHandler : InputHandlerBase
  {
    public FPMS_FHA203KInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public FPMS_FHA203KInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public FPMS_FHA203KInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public FPMS_FHA203KInputHandler(
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
        case "3052":
          if (this.GetFieldValue("1172") == "FHA" && this.GetFieldValue("19") == "NoCash-Out Refinance" || this.GetFieldValue("19").IndexOf("Refinance") == -1 || this.GetFieldValue("3000") != "Y" && this.GetFieldValue("2997") != "Y")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "MAX23K.X40":
          if (this.GetField("19") == "Purchase" || this.GetField("19").IndexOf("Refinance") > -1)
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "MAX23K.X74":
          if (this.GetField("MAX23K.X73") != "Other")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "MAX23K.X19":
          if (this.GetField("MAX23K.X117") == "Y")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "MAX23K.X81":
          if (this.GetField("MAX23K.X117") == "Y")
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
      base.ExecAction(action);
      switch (action)
      {
        case "copymax23kx29":
          this.SetFieldFocus(this.GetField("MAX23K.X117") == "Y" ? "l_max23kx113" : "l_max23kx19");
          break;
        case "copymax23kx39":
          this.SetFieldFocus(this.GetField("MAX23K.X117") == "Y" ? "l_max23kx8_2" : "l_max23kx8");
          break;
        case "copymax23kx47":
          this.SetFieldFocus(this.GetField("MAX23K.X117") == "Y" ? "l_max23kx8_2" : "l_max23kx8");
          break;
        case "existing23kdebt":
          this.SetFieldFocus("l_max23kx132");
          break;
      }
    }
  }
}
