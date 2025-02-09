// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.REGZLEInputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

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
  public class REGZLEInputHandler : REGZ50InputHandler
  {
    private AIRInputHandler airInputHandler;
    private APInputHandler apInputHandler;
    private DropdownBox ddAssumption;
    private LoanTermTableInputHandler loanTermTableInputHandler;

    public REGZLEInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public REGZLEInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public REGZLEInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public REGZLEInputHandler(
      Sessions.Session session,
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, owner, dataTemplate, htmldoc, form, property)
    {
    }

    internal override void CreateControls()
    {
      base.CreateControls();
      try
      {
        this.airInputHandler = new AIRInputHandler(this.currentForm, (IHtmlInput) this.loan);
        this.airInputHandler.SetSectionStatus();
        this.apInputHandler = new APInputHandler(this.currentForm, (IHtmlInput) this.loan);
        this.apInputHandler.SetSectionStatus();
        this.ddAssumption = (DropdownBox) this.currentForm.FindControl("dd_Assumption");
        this.ddAssumption.Options.Remove(new DropdownOption("may", "May"));
        this.loanTermTableInputHandler = new LoanTermTableInputHandler(this.currentForm, (IHtmlInput) this.loan, this.ToString(), this.session);
      }
      catch (Exception ex)
      {
      }
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      ControlState currentState = base.GetControlState(ctrl, id);
      switch (id)
      {
        case "CD4.X31":
          currentState = !(this.inputData.GetField("608") != "AdjustableRate") ? ControlState.Enabled : ControlState.Disabled;
          break;
        case "4084":
          currentState = !(this.inputData.GetField("19") == "ConstructionOnly") ? ControlState.Disabled : ControlState.Enabled;
          break;
        case "CONST.X1":
          currentState = !(this.inputData.GetField("19") != "ConstructionToPermanent") ? ControlState.Enabled : ControlState.Disabled;
          break;
        default:
          if (this.loanTermTableInputHandler != null)
          {
            currentState = this.loanTermTableInputHandler.GetControlState(id, currentState);
            break;
          }
          break;
      }
      return currentState;
    }

    internal override void UpdateFieldValue(string id, string val)
    {
      base.UpdateFieldValue(id, val);
      if (id == "423")
        this.apInputHandler.ChangeMonthlyBiweeklyLabel();
      if (this.loanTermTableInputHandler == null)
        return;
      this.loanTermTableInputHandler.SetLayout();
    }

    protected override string GetFieldValue(string id, FieldSource fieldSource)
    {
      if (id == "CD4.X25" || id == "CD4.X27" || id == "NEWHUD.X6")
        this.apInputHandler.SetSectionStatus();
      if (id == "608" || id == "19" || id == "1677" || id == "3")
        this.airInputHandler.SetSectionStatus();
      return id.StartsWith("LE1.X") ? this.loanTermTableInputHandler.GetFieldValue(id, base.GetFieldValue(id, fieldSource)) : this.airInputHandler.GetFieldValue(id, base.GetFieldValue(id, fieldSource));
    }

    public override void RefreshContents(bool skipButtonFieldLockRules)
    {
      if (this.loanTermTableInputHandler != null)
        this.loanTermTableInputHandler.SetLayout();
      base.RefreshContents(skipButtonFieldLockRules);
    }

    public override void ExecAction(string action)
    {
      base.ExecAction(action);
      switch (action)
      {
        case "loanprog":
          this.apInputHandler.ChangeMonthlyBiweeklyLabel();
          this.loanTermTableInputHandler.SetLayout();
          break;
        case "mtgins":
          this.loanTermTableInputHandler.SetLayout();
          break;
      }
    }
  }
}
