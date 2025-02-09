// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.MAX23KInputHandler
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
  public class MAX23KInputHandler : InputHandlerBase
  {
    public MAX23KInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public MAX23KInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public MAX23KInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public MAX23KInputHandler(
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
        case "1061":
          if (this.GetFieldValue("NEWHUD.X1139") == "Y" || this.GetFieldValue("NEWHUD.X715") != "Include Origination Points" || Utils.ParseDouble((object) this.GetFieldValue("3119")) > 0.0)
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "1134":
          if (this.loan.GetField("19") == "Purchase")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "9":
          if (this.loan.GetField("19") != "Other")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "MAX23K.X35":
        case "MAX23K.X36":
          if (this.loan.GetField("19") != "Purchase")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "MAX23K.X40":
        case "MAX23K.X75":
          if (this.loan.GetField("19") == "Purchase" || this.loan.GetField("19") == "NoCash-Out Refinance" || this.loan.GetField("19") == "Cash-Out Refinance")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "MAX23K.X74":
          if (this.loan.GetField("MAX23K.X73") != "Other")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "copymax23kx47":
          if (this.loan.GetField("19") != "NoCash-Out Refinance" && this.loan.GetField("19") != "Cash-Out Refinance")
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

    internal override void UpdateFieldValue(string id, string val)
    {
      base.UpdateFieldValue(id, val);
      if (!(id == "MAX23K.X73") || !(val != "Other"))
        return;
      this.loan.SetField("MAX23K.X74", string.Empty);
    }

    public override void ExecAction(string action)
    {
      switch (action)
      {
        case "mipff":
          base.ExecAction(action);
          this.SetFieldFocus("l_3");
          break;
        case "copymax23kx29":
          base.ExecAction(action);
          this.SetFieldFocus("l_MAX23KX35");
          break;
        case "copymax23kx47":
        case "copymax23kx39":
          base.ExecAction(action);
          this.SetFieldFocus("l_1134");
          break;
      }
    }
  }
}
