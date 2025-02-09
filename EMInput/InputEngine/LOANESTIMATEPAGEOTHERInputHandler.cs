// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.LOANESTIMATEPAGEOTHERInputHandler
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
  public class LOANESTIMATEPAGEOTHERInputHandler : InputHandlerBase
  {
    public LOANESTIMATEPAGEOTHERInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public LOANESTIMATEPAGEOTHERInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public LOANESTIMATEPAGEOTHERInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public LOANESTIMATEPAGEOTHERInputHandler(
      Sessions.Session session,
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, dataTemplate, htmldoc, form, property)
    {
    }

    public LOANESTIMATEPAGEOTHERInputHandler(
      Sessions.Session session,
      IWin32Window owner,
      IHtmlInput htmlInput,
      HTMLDocument htmlDoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, htmlInput, htmlDoc, form, property)
    {
    }

    internal override void CreateControls()
    {
      CategoryBox control1 = (CategoryBox) this.currentForm.FindControl("boxCashToClose");
      CategoryBox control2 = (CategoryBox) this.currentForm.FindControl("boxAlternateCashToClose");
      control2.Top = control1.Top;
      if (this.inputData.GetField("LE2.X28") == "Y")
      {
        control2.Visible = true;
        control1.Visible = false;
      }
      else
      {
        control2.Visible = false;
        control1.Visible = true;
      }
    }

    protected override string GetFieldValue(string id, FieldSource fieldSource)
    {
      string fieldValue = base.GetFieldValue(id, fieldSource);
      return fieldValue == "" && id == "CD3.X94" ? "0.00" : fieldValue;
    }
  }
}
