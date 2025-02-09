// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.HUD1PG3_2010InputHandler
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
  public class HUD1PG3_2010InputHandler : InputHandlerBase
  {
    public HUD1PG3_2010InputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public HUD1PG3_2010InputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public HUD1PG3_2010InputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public HUD1PG3_2010InputHandler(
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
        case "NEWHUD.X5":
          if (this.GetFieldValue("608") != "OtherAmortizationType")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "NEWHUD.X355":
        case "NEWHUD.X356":
        case "NEWHUD.X357":
          if (this.loan != null && !this.loan.IsLocked("NEWHUD.X217"))
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
      if (id == "NEWHUD.X319" || id == "NEWHUD.X320" || id == "NEWHUD.X321")
      {
        int num1 = Utils.ParseInt((object) val);
        if ((num1 < 703 || num1 > 704) && (num1 < 801 || num1 > 819) && (num1 < 901 || num1 > 910) && (num1 < 1001 || num1 > 1011) && (num1 < 1101 || num1 > 1114) && (num1 < 1201 || num1 > 1208) && (num1 < 1301 || num1 > 1311))
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this.mainScreen, "The line number '" + val + "' is invalid.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return;
        }
      }
      base.UpdateFieldValue(id, val);
      if (id == "3171" || id == "3172")
      {
        if ((this.GetFieldValue("3171") == string.Empty || this.GetFieldValue("3171") == "//") && this.GetFieldValue("3172") == string.Empty)
          base.UpdateFieldValue("3173", "");
        else
          base.UpdateFieldValue("3173", this.session.UserInfo.FullName + " (" + this.session.UserID + ")");
      }
      if (this.loan == null)
        return;
      this.loan.Calculator.FormCalculation("HUD1PG3_2010", id, val);
    }
  }
}
