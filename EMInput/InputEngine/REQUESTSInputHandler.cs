// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.REQUESTSInputHandler
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
  public class REQUESTSInputHandler : InputHandlerBase
  {
    public REQUESTSInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public REQUESTSInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public REQUESTSInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public REQUESTSInputHandler(
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
      if ((id == "REQUEST.X29" || id == "REQUEST.X30" || id == "REQUEST.X31" || id == "REQUEST.X32" || id == "REQUEST.X33") && this.loan != null && (this.GetField("REQUEST.X25") == "Borrower" || this.GetField("REQUEST.X25") == "CoBorrower"))
        controlState = ControlState.Disabled;
      return controlState;
    }

    internal override void UpdateFieldValue(string id, string val)
    {
      base.UpdateFieldValue(id, val);
      if (!(id == "REQUEST.X25") || string.Compare(val, "Borrower", true) == 0 || string.Compare(val, "CoBorrower", true) == 0)
        return;
      base.UpdateFieldValue("REQUEST.X29", "");
      base.UpdateFieldValue("REQUEST.X30", "");
      base.UpdateFieldValue("REQUEST.X31", "");
      base.UpdateFieldValue("REQUEST.X32", "");
      base.UpdateFieldValue("REQUEST.X33", "");
    }
  }
}
