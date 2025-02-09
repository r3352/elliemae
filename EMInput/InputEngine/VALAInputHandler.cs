// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.VALAInputHandler
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
  internal class VALAInputHandler : InputHandlerBase
  {
    private EllieMae.Encompass.Forms.Panel pnl2009;
    private EllieMae.Encompass.Forms.Panel pnl2020;

    public VALAInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public VALAInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public VALAInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public VALAInputHandler(
      Sessions.Session session,
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, dataTemplate, htmldoc, form, property)
    {
    }

    internal override void CreateControls()
    {
      try
      {
        if (this.pnl2009 == null)
          this.pnl2009 = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnl2009");
        if (this.pnl2020 == null)
          this.pnl2020 = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnl2020");
        if (this.pnl2009 == null || this.pnl2020 == null)
          return;
        if (this.GetField("1825") == "2020")
          this.pnl2009.Visible = !(this.pnl2020.Visible = true);
        else
          this.pnl2009.Visible = !(this.pnl2020.Visible = false);
      }
      catch
      {
      }
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      ControlState controlState = this.getControlState(ctrl, id, ControlState.Enabled);
      if (id == "vol")
      {
        if (this.FormIsForTemplate)
          controlState = ControlState.Disabled;
      }
      else
        controlState = ControlState.Default;
      return controlState;
    }

    public override void ExecAction(string action)
    {
      switch (action)
      {
        case "vol":
          base.ExecAction(action);
          this.SetFieldFocus("l_FL0102");
          break;
        case "mtginsu":
          base.ExecAction("mipff");
          this.SetFieldFocus("l_1335");
          break;
      }
    }
  }
}
