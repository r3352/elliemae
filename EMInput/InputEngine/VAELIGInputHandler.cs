// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.VAELIGInputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using mshtml;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  internal class VAELIGInputHandler : InputHandlerBase
  {
    public VAELIGInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public VAELIGInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public VAELIGInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public VAELIGInputHandler(
      Sessions.Session session,
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, dataTemplate, htmldoc, form, property)
    {
    }

    internal override void UpdateFieldValue(string id, string val)
    {
      base.UpdateFieldValue(id, val);
      switch (id)
      {
        case "VAELIG.X66":
        case "VASUMM.X31":
          this.loan.Calculator.UpdateAccountName("VA");
          break;
        case "VAELIG.X2":
        case "VAELIG.X7":
        case "VAELIG.X12":
        case "VAELIG.X17":
          this.loan.Calculator.UpdateAccountName("VA");
          break;
      }
    }
  }
}
