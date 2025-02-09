// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.CASHINFOInputHandler
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
  public class CASHINFOInputHandler : InputHandlerBase
  {
    private EllieMae.Encompass.Forms.Panel pnlBorPaidFees;
    private EllieMae.Encompass.Forms.Panel pnlBorPaidFeesNew;

    public CASHINFOInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public CASHINFOInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public CASHINFOInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public CASHINFOInputHandler(
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
        this.pnlBorPaidFees = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("BorPaidFeesPanel");
        this.pnlBorPaidFeesNew = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("BorPaidFeesNewPanel");
        if (this.inputData.GetField("4796") == "Y")
        {
          this.enableOrDisableDropdownPanels(this.pnlBorPaidFeesNew, this.pnlBorPaidFees);
          this.pnlBorPaidFeesNew.Position = this.pnlBorPaidFees.Position;
        }
        else
          this.enableOrDisableDropdownPanels(this.pnlBorPaidFees, this.pnlBorPaidFeesNew);
      }
      catch
      {
      }
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      ControlState controlState = this.getControlState(ctrl, id, ControlState.Enabled);
      return !(id == "140") ? ControlState.Default : this.GetSpecialControlStatus(id, controlState);
    }

    public override void ExecAction(string action)
    {
      base.ExecAction(action);
      if (!(action == "mipff"))
        return;
      this.SetFieldFocus("l_1109");
    }
  }
}
