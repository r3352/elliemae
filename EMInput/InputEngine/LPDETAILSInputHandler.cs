// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.LPDETAILSInputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DocEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.Forms;
using mshtml;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class LPDETAILSInputHandler : InputHandlerBase
  {
    private const string className = "LPDETAILSInputHandler";
    private IWin32Window owner;
    private LoanProgramFieldSettings fieldSettings;
    private LoanProgram loanProgram;
    private DropdownBox closingTypeDropdown;

    public LPDETAILSInputHandler(
      IWin32Window owner,
      IHtmlInput input,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, input, htmldoc, form, property)
    {
    }

    public LPDETAILSInputHandler(
      Sessions.Session session,
      IWin32Window owner,
      IHtmlInput input,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, input, htmldoc, form, property)
    {
      this.owner = owner;
    }

    internal override void CreateControls()
    {
      this.loanProgram = (LoanProgram) this.inputData;
      this.fieldSettings = this.session.ConfigurationManager.GetLoanProgramFieldSettings();
      this.loanProgram.ApplyFieldSettings(this.fieldSettings);
      if (this.closingTypeDropdown == null)
        this.closingTypeDropdown = (DropdownBox) this.currentForm.FindControl("DropdownBox6");
      if (string.Equals(this.session.ServerManager.GetServerSetting("eClose.AllowHybridWithENoteClosing").ToString(), "Disabled", StringComparison.InvariantCultureIgnoreCase) && !string.Equals(this.GetField("4689"), "HybridWithENote", StringComparison.InvariantCultureIgnoreCase))
        this.closingTypeDropdown.Options.Remove(new DropdownOption("Hybrid With eNote", "HybridWithENote"));
      if (this.loanProgram.IsLinkedToDocEnginePlan)
        this.refreshPlanCodeData();
      if (this.loanProgram.IsLinkedToDocEnginePlan)
        return;
      this.hidePlanCodeFields();
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string fieldIdOrAction)
    {
      switch (fieldIdOrAction)
      {
        case "Terms.USDAGovtType":
          return this.GetField("1172") != "FarmersHomeAdministration" ? ControlState.Disabled : ControlState.Enabled;
        case "1063":
          return ControlState.Enabled;
        case "4665":
          return this.GetField("1172") == "HELOC" ? ControlState.Enabled : ControlState.Disabled;
        default:
          ControlState sharedFieldStatus = LOANPROGInputHandler.GetSharedFieldStatus((InputHandlerBase) this, fieldIdOrAction);
          return sharedFieldStatus != ControlState.Default ? sharedFieldStatus : base.GetControlState(ctrl, fieldIdOrAction);
      }
    }

    internal override void UpdateFieldValue(string id, string val)
    {
      base.UpdateFieldValue(id, val);
      LOANPROGInputHandler.UpdateSharedFieldValue((InputHandlerBase) this, this.inputData, id, val);
      if (!(id == "1172"))
        return;
      LOANPROGInputHandler.RefreshHELOCSections((InputHandlerBase) this, this.currentForm);
    }

    private void hidePlanCodeFields()
    {
      EllieMae.Encompass.Forms.GroupBox control1 = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl("grpPlanCode");
      if (control1 == null)
        throw new Exception("PlanCode GroupBox is missing");
      int height = control1.Size.Height;
      control1.Visible = false;
      foreach (EllieMae.Encompass.Forms.GroupBox groupBox in this.currentForm.FindControlsByType(typeof (EllieMae.Encompass.Forms.GroupBox)))
      {
        if (groupBox.ControlID != control1.ControlID)
          groupBox.Top -= height;
      }
      foreach (CategoryBox categoryBox in this.currentForm.FindControlsByType(typeof (CategoryBox)))
      {
        if (categoryBox.ControlID != control1.ControlID)
          categoryBox.Top -= height;
      }
      ((VerticalRule) this.currentForm.FindControl("VerticalRule1")).Top -= height;
      ((EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("lblFormName")).Top -= height;
      EllieMae.Encompass.Forms.Panel control2 = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlForm");
      if (control2 == null)
        throw new Exception("Form panel is missing");
      control2.Size = new Size(control2.Size.Width, control2.Size.Height - height);
    }

    private void refreshPlanCodeData()
    {
      try
      {
        Plan.Synchronize(this.session.SessionObjects, this.loanProgram);
      }
      catch (Exception ex)
      {
        Tracing.Log(InputHandlerBase.sw, nameof (LPDETAILSInputHandler), TraceLevel.Warning, "Failed to sync plan code for loan program '" + this.loanProgram.TemplateName + "': " + (object) ex);
      }
      this.RefreshAllControls(true);
    }

    public override void RefreshContents(bool skipButtonFieldLockRules)
    {
      base.RefreshContents(skipButtonFieldLockRules);
      LOANPROGInputHandler.RefreshHELOCSections((InputHandlerBase) this, this.currentForm);
    }

    public override void onfocus(mshtml.IHTMLEventObj pEvtObj)
    {
      if (!(this.currentForm.FindControlForElement(pEvtObj.srcElement) is FieldControl controlForElement) || controlForElement.Field == null || controlForElement.Field.FieldID == "")
        return;
      Session.Application.GetService<IStatusDisplay>().DisplayFieldID(controlForElement.Field.FieldID);
    }

    public override void ExecAction(string action)
    {
      switch (action)
      {
        case "cleartable":
          this.inputData.SetCurrentField("1985", "");
          this.RefreshAllControls(true);
          break;
        case "historicalindex":
          base.ExecAction(action);
          if (!(this.inputData.GetField("1985") != ""))
            break;
          this.inputData.SetCurrentField("1889", "");
          this.inputData.SetCurrentField("1890", "");
          break;
        default:
          base.ExecAction(action);
          break;
      }
    }
  }
}
