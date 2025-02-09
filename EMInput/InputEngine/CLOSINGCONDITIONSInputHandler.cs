// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.CLOSINGCONDITIONSInputHandler
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
  public class CLOSINGCONDITIONSInputHandler : InputHandlerBase
  {
    private ImageButton btnNew;
    private ImageButton btnNewOver;
    private string sourceNew = string.Empty;
    private string sourceOver = string.Empty;

    public CLOSINGCONDITIONSInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public CLOSINGCONDITIONSInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
      this.btnNew = (ImageButton) this.currentForm.FindControl(nameof (btnNew));
      this.btnNewOver = (ImageButton) this.currentForm.FindControl(nameof (btnNewOver));
      this.btnNewOver.Left = this.btnNew.Left;
      this.sourceNew = this.btnNew.Source;
      this.sourceOver = this.btnNewOver.Source;
      this.btnNewOver.Visible = false;
    }

    public CLOSINGCONDITIONSInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public CLOSINGCONDITIONSInputHandler(
      Sessions.Session session,
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, dataTemplate, htmldoc, form, property)
    {
      this.btnNew = (ImageButton) this.currentForm.FindControl(nameof (btnNew));
      this.btnNewOver = (ImageButton) this.currentForm.FindControl(nameof (btnNewOver));
      this.btnNew.Visible = this.btnNewOver.Visible = false;
    }

    public override void onmouseenter(mshtml.IHTMLEventObj pEvtObj)
    {
      if (pEvtObj.srcElement.id == "btnNew")
        this.btnNew.Source = this.sourceOver;
      base.onmouseenter(pEvtObj);
    }

    public override void onmouseleave(mshtml.IHTMLEventObj pEvtObj)
    {
      if (!(pEvtObj.srcElement.id == "btnNew"))
        return;
      this.btnNew.Source = this.sourceNew;
    }

    public override void ExecAction(string action)
    {
      if (!(action == "condlist"))
        return;
      base.ExecAction(action);
      this.SetFieldFocus("l_1952");
    }
  }
}
