// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.HMDA_DENIALInputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.Forms;
using mshtml;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  internal class HMDA_DENIALInputHandler : InputHandlerBase
  {
    public HMDA_DENIALInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public HMDA_DENIALInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public HMDA_DENIALInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public HMDA_DENIALInputHandler(
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
      if (this.allFieldsAreReadonly)
        return ControlState.Disabled;
      ControlState controlState = this.getControlState(ctrl, id, ControlState.Enabled);
      switch (id)
      {
        case "364":
          if (this.FormIsForTemplate || InputHandlerBase.LockLoanNumber)
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "geocode":
        case "editcheck":
        case "ratespread":
          if (this.FormIsForTemplate)
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        default:
          controlState = ControlState.Default;
          break;
      }
      return controlState;
    }

    internal override void UpdateFieldValue(string id, string val)
    {
      if (id == "470" && val != "Other")
        this.loan.SetField("1136", string.Empty);
      else if (id == "477" && val != "Other")
        this.loan.SetField("1300", string.Empty);
      base.UpdateFieldValue(id, val);
    }

    public override void ExecAction(string action)
    {
      IEPass service = Session.Application.GetService<IEPass>();
      switch (action)
      {
        case "ratespread":
          service.ProcessURL("_EPASS_SIGNATURE;PCIWIZ;2;RATE_SPREAD");
          this.UpdateContents();
          break;
        case "geocode":
          service.ProcessURL("_EPASS_SIGNATURE;PCIWIZ;2;GEOCODING;SINGLE");
          this.UpdateContents();
          break;
        case "editcheck":
          service.ProcessURL("_EPASS_SIGNATURE;PCIWIZ;2;EDIT_CHECKS;SINGLE");
          break;
        default:
          base.ExecAction(action);
          break;
      }
    }
  }
}
