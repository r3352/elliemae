// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.REGZ50CLOSER_PREPAYMENTInputHandler
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
  public class REGZ50CLOSER_PREPAYMENTInputHandler : InputHandlerBase
  {
    public REGZ50CLOSER_PREPAYMENTInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public REGZ50CLOSER_PREPAYMENTInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public REGZ50CLOSER_PREPAYMENTInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public REGZ50CLOSER_PREPAYMENTInputHandler(
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
      this.getControlState(ctrl, id, ControlState.Enabled);
      ControlState controlState;
      if (id == "RE88395.X316")
      {
        if (this.loan != null && this.loan.GetField("RE88395.X322") != "Y")
        {
          this.SetControlState("FieldLock_X316", false);
          controlState = ControlState.Disabled;
        }
        else
        {
          this.SetControlState("FieldLock_X316", true);
          controlState = ControlState.Default;
        }
      }
      else
        controlState = ControlState.Default;
      return controlState;
    }
  }
}
