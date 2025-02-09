// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.UNDERWRITERSUMMARYP2InputHandler
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
  internal class UNDERWRITERSUMMARYP2InputHandler : InputHandlerBase
  {
    public UNDERWRITERSUMMARYP2InputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public UNDERWRITERSUMMARYP2InputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public UNDERWRITERSUMMARYP2InputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public UNDERWRITERSUMMARYP2InputHandler(
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
      if (id == "541")
      {
        if (this.loan.GetField("2366") != "Y")
          controlState = ControlState.Disabled;
      }
      else
        controlState = ControlState.Default;
      return controlState;
    }

    internal override void UpdateFieldValue(string id, string val)
    {
      base.UpdateFieldValue(id, val);
      if ((id == "2305" || id == "1994") && (this.GetFieldValue("1995") ?? "") == string.Empty)
        base.UpdateFieldValue("1995", this.session.UserInfo.FullName);
      switch (id)
      {
        case "2305":
          base.UpdateFieldValue("1994", val);
          break;
        case "1994":
          base.UpdateFieldValue("2305", val);
          break;
      }
    }
  }
}
