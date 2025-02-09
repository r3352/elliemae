// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.SERVICINGDISCLOSUREInputHandler
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
  public class SERVICINGDISCLOSUREInputHandler : InputHandlerBase
  {
    public SERVICINGDISCLOSUREInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public SERVICINGDISCLOSUREInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public SERVICINGDISCLOSUREInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public SERVICINGDISCLOSUREInputHandler(
      Sessions.Session session,
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, dataTemplate, htmldoc, form, property)
    {
    }

    public SERVICINGDISCLOSUREInputHandler(
      IWin32Window owner,
      IHtmlInput input,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, input, htmldoc, form, property)
    {
    }

    public SERVICINGDISCLOSUREInputHandler(
      Sessions.Session session,
      IWin32Window owner,
      IHtmlInput input,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, input, htmldoc, form, property)
    {
    }

    internal override void UpdateFieldValue(string id, string val)
    {
      base.UpdateFieldValue(id, val);
      if (id == "RESPA.X1" && val == "Y")
      {
        base.UpdateFieldValue("RESPA.X6", string.Empty);
        base.UpdateFieldValue("RESPA.X28", string.Empty);
      }
      else if (id == "RESPA.X6" && val == "Y")
      {
        base.UpdateFieldValue("RESPA.X1", string.Empty);
        base.UpdateFieldValue("RESPA.X28", string.Empty);
      }
      else
      {
        if (!(id == "RESPA.X28") || !(val == "Y"))
          return;
        base.UpdateFieldValue("RESPA.X1", string.Empty);
        base.UpdateFieldValue("RESPA.X6", string.Empty);
      }
    }
  }
}
