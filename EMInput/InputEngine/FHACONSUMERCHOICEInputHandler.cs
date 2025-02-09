// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.FHACONSUMERCHOICEInputHandler
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
  public class FHACONSUMERCHOICEInputHandler : InputHandlerBase
  {
    public FHACONSUMERCHOICEInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public FHACONSUMERCHOICEInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public FHACONSUMERCHOICEInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public FHACONSUMERCHOICEInputHandler(
      Sessions.Session session,
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, dataTemplate, htmldoc, form, property)
    {
    }

    public FHACONSUMERCHOICEInputHandler(
      IWin32Window owner,
      IHtmlInput input,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, input, htmldoc, form, property)
    {
    }

    public FHACONSUMERCHOICEInputHandler(
      Sessions.Session session,
      IWin32Window owner,
      IHtmlInput input,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, input, htmldoc, form, property)
    {
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      this.getControlState(ctrl, id, ControlState.Enabled);
      if (id == "refreshfromsetting")
      {
        ctrl.Visible = this.loan != null;
        return this.loan == null ? ControlState.Disabled : ControlState.Default;
      }
      return this.loan == null ? ControlState.Default : ControlState.Disabled;
    }
  }
}
