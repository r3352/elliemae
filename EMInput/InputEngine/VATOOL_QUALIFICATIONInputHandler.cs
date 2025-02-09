// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.VATOOL_QUALIFICATIONInputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.CalculationEngine;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.Forms;
using mshtml;
using System;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  internal class VATOOL_QUALIFICATIONInputHandler : InputHandlerBase
  {
    private FieldLock l_VASUMMX132;

    public VATOOL_QUALIFICATIONInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public VATOOL_QUALIFICATIONInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public VATOOL_QUALIFICATIONInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public VATOOL_QUALIFICATIONInputHandler(
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
        this.l_VASUMMX132 = (FieldLock) this.currentForm.FindControl("fl_VASUMMX132");
      }
      catch (Exception ex)
      {
      }
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      ControlState controlState = this.getControlState(ctrl, id, ControlState.Enabled);
      switch (id)
      {
        case "VASUMM.X132":
          if (!SharedCalculations.UseNewVAIRRRL(this.inputData.GetField("1172"), this.inputData.GetField("958"), this.inputData.GetField("1887"), this.inputData.GetField("2553"), this.inputData.GetField("748"), this.inputData.GetField("763")))
          {
            this.l_VASUMMX132.Enabled = false;
            controlState = ControlState.Disabled;
            break;
          }
          this.l_VASUMMX132.Enabled = this.inputData.GetField("VASUMM.X133") == "Y";
          break;
        case "VASUMM.X133":
        case "VASUMM.X134":
        case "VASUMM.X131":
          if (!SharedCalculations.UseNewVAIRRRL(this.inputData.GetField("1172"), this.inputData.GetField("958"), this.inputData.GetField("1887"), this.inputData.GetField("2553"), this.inputData.GetField("748"), this.inputData.GetField("763")))
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
      base.UpdateFieldValue(id, val);
      if (!(id == "VASUMM.X31"))
        return;
      this.loan.Calculator.UpdateAccountName("VA");
    }

    public override void ExecAction(string action) => base.ExecAction(action);
  }
}
