// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine._NY_APPLOGInputHandler
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
  internal class _NY_APPLOGInputHandler : InputHandlerBase
  {
    public _NY_APPLOGInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public _NY_APPLOGInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public _NY_APPLOGInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public _NY_APPLOGInputHandler(
      Sessions.Session session,
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, dataTemplate, htmldoc, form, property)
    {
    }

    public override void ExecAction(string action)
    {
      switch (action)
      {
        case "copygfe":
          this.UpdateFieldValue("NYFEES0108", this.loan.GetSimpleField("L228"));
          this.UpdateFieldValue("NYFEES0208", this.loan.GetSimpleField("641"));
          this.UpdateFieldValue("NYFEES0308", this.loan.GetSimpleField("640"));
          this.UpdateFieldValue("NYFEES0408", this.loan.GetSimpleField("454"));
          this.UpdateContents();
          this.SetFieldFocus("l_NYFEES0101");
          break;
        case "contactapp":
          this.GetContactItem("NYFEES0201");
          this.SetFieldFocus("l_NYFEES0201");
          break;
        case "contactapp2":
          this.GetContactItem("NYFEES1101");
          this.SetFieldFocus("l_NYFEES1101");
          break;
        case "contactcre":
          this.GetContactItem("NYFEES0301");
          this.SetFieldFocus("l_NYFEES0301");
          break;
        case "contactcre2":
          this.GetContactItem("NYFEES1201");
          this.SetFieldFocus("l_NYFEES1201");
          break;
      }
    }
  }
}
