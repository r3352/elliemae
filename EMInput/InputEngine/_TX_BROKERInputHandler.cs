// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine._TX_BROKERInputHandler
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
  internal class _TX_BROKERInputHandler : InputHandlerBase
  {
    public _TX_BROKERInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public _TX_BROKERInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public _TX_BROKERInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public _TX_BROKERInputHandler(
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
      if (id == "DISCLOSURE.X96" && val == "N")
        val = string.Empty;
      base.UpdateFieldValue(id, val);
    }

    public override void ExecAction(string action)
    {
      switch (action)
      {
        case "copygfe":
          this.UpdateFieldValue("DISCLOSURE.X82", this.loan.GetSimpleField("L228"));
          this.UpdateFieldValue("DISCLOSURE.X84", this.loan.GetSimpleField("617"));
          this.UpdateFieldValue("DISCLOSURE.X85", this.loan.GetSimpleField("641"));
          this.UpdateFieldValue("DISCLOSURE.X86", this.loan.GetSimpleField("624"));
          this.UpdateFieldValue("DISCLOSURE.X87", this.loan.GetSimpleField("640"));
          this.UpdateContents();
          this.SetFieldFocus("l_DISCLOSURE_X82");
          break;
        case "contactapp":
          this.GetContactItem("DISCLOSURE.X84");
          this.SetFieldFocus("l_DISCLOSURE_X84");
          break;
        case "contactcre":
          this.GetContactItem("DISCLOSURE.X86");
          this.SetFieldFocus("l_DISCLOSURE_X86");
          break;
        case "contactund":
          this.GetContactItem("DISCLOSURE.X88");
          this.SetFieldFocus("l_DISCLOSURE_X88");
          break;
      }
    }
  }
}
