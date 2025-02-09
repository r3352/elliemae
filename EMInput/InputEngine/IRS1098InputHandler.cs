// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.IRS1098InputHandler
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
  internal class IRS1098InputHandler : InputHandlerBase
  {
    public IRS1098InputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public IRS1098InputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public IRS1098InputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public IRS1098InputHandler(
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
        case "319":
          return controlState;
        case "1195":
        case "4101":
          if (this.loan.GetField("4097").Equals("") && this.loan.GetField("4098").Equals("") && this.loan.GetField("4099").Equals("") && this.loan.GetField("4100").Equals("") && this.loan.GetField("4103") != "Y")
          {
            controlState = ControlState.Enabled;
            goto case "11";
          }
          else
          {
            controlState = ControlState.Disabled;
            this.UpdateFieldValue(id, "");
            goto case "11";
          }
        case "364":
          if (this.FormIsForTemplate || InputHandlerBase.LockLoanNumber)
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "4097":
        case "4098":
        case "4099":
        case "4100":
        case "copyaddrto1098":
          controlState = this.loan.GetField("4103") == "Y" || !this.loan.GetField("1195").Equals("") || !this.loan.GetField("4101").Equals("") ? ControlState.Disabled : ControlState.Enabled;
          goto case "11";
        case "4103":
          if (this.loan.GetField("4097").Equals("") && this.loan.GetField("4098").Equals("") && this.loan.GetField("4099").Equals("") && this.loan.GetField("4100").Equals("") && this.loan.GetField("4104").Equals("Y"))
            this.UpdateFieldValue("4104", "N");
          controlState = this.loan.GetField("4104") == "Y" || !this.loan.GetField("1195").Equals("") || !this.loan.GetField("4101").Equals("") || !this.loan.GetField("4097").Equals("") || !this.loan.GetField("4098").Equals("") || !this.loan.GetField("4099").Equals("") || !this.loan.GetField("4100").Equals("") ? ControlState.Disabled : ControlState.Enabled;
          goto case "11";
        default:
          controlState = ControlState.Default;
          goto case "11";
      }
    }

    public override void ExecAction(string action)
    {
      base.ExecAction(action);
      if (!(action == "copyaddrto1098"))
        return;
      this.SetFieldDisabled("l_4103");
      this.UpdateFieldValue("4104", "Y");
    }
  }
}
