// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.LOANCOMPInputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.AsmResolver;
using EllieMae.Encompass.Forms;
using mshtml;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class LOANCOMPInputHandler : InputHandlerBase
  {
    public LOANCOMPInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public LOANCOMPInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
      this.refreshStopLights(0);
      this.refreshStopLights(1);
      this.refreshStopLights(2);
    }

    public LOANCOMPInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public LOANCOMPInputHandler(
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
      if (id == "LP0224" || id == "LP01106" || id == "LP02106" || id == "LP0124")
        val = this.GetFieldValue(id);
      base.UpdateFieldValue(id, val);
      this.loan.Calculator.FormCalculation("LOANCOMP", id, val);
      for (int col = 0; col <= 2; ++col)
      {
        this.refreshCashFromTo(col);
        this.refreshStopLights(col);
      }
    }

    protected override string GetFieldValue(string id, FieldSource source)
    {
      if (!(id == "LP0101") && !(id == "LP0201"))
        return base.GetFieldValue(id, source);
      string fieldValue = base.GetFieldValue(id, source);
      if (fieldValue == "FarmersHomeAdministration")
        fieldValue = "USDA-RHS";
      return fieldValue;
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      ControlState defaultValue = this.getControlState(ctrl, id, ControlState.Enabled);
      switch (id)
      {
        case "140":
          defaultValue = this.GetSpecialControlStatus(id, defaultValue);
          break;
        case "142":
          this.refreshCashFromTo(0);
          break;
        case "PREQUAL.X44":
          this.refreshCashFromTo(1);
          break;
        case "PREQUAL.X84":
          this.refreshCashFromTo(2);
          break;
        default:
          defaultValue = ControlState.Default;
          break;
      }
      return defaultValue;
    }

    public override void ExecAction(string action)
    {
      if (action != "loanprog" && action != "ccprog")
        base.ExecAction(action);
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(action))
      {
        case 246033821:
          if (!(action == "basecost"))
            break;
          this.refreshAllStopLights();
          this.SetFieldFocus("l_67");
          break;
        case 371019337:
          if (!(action == "heoblig"))
            break;
          this.refreshAllStopLights();
          this.SetFieldFocus("l_67");
          break;
        case 375884332:
          if (!(action == "detail0"))
            break;
          this.SetFieldFocus("l_67");
          break;
        case 382748654:
          if (!(action == "cashflow"))
            break;
          this.refreshAllStopLights();
          this.SetFieldFocus("l_67");
          break;
        case 392661951:
          if (!(action == "detail1"))
            break;
          this.SetFieldFocus("l_67");
          break;
        case 409439570:
          if (!(action == "detail2"))
            break;
          this.SetFieldFocus("l_67");
          break;
        case 684340119:
          if (!(action == "alt1"))
            break;
          this.copyToScenario(1);
          this.SetFieldFocus("l_1177");
          break;
        case 701117738:
          if (!(action == "alt2"))
            break;
          this.copyToScenario(2);
          this.SetFieldFocus("l_1177");
          break;
        case 787192126:
          if (!(action == "closingcosts"))
            break;
          this.refreshAllStopLights();
          this.SetFieldFocus("l_67");
          break;
        case 1094581887:
          if (!(action == "comments2"))
            break;
          this.SetFieldFocus("l_67");
          break;
        case 1111359506:
          if (!(action == "comments1"))
            break;
          this.SetFieldFocus("l_67");
          break;
        case 1128137125:
          if (!(action == "comments0"))
            break;
          this.SetFieldFocus("l_67");
          break;
        case 1427775171:
          if (!(action == "maxloanamount"))
            break;
          this.refreshAllStopLights();
          this.SetFieldFocus("l_67");
          break;
        case 1611142665:
          if (!(action == "totalcosts"))
            break;
          this.refreshAllStopLights();
          this.SetFieldFocus("l_67");
          break;
        case 2023485534:
          if (!(action == "income"))
            break;
          this.SetFieldFocus("l_67");
          break;
        case 2437312304:
          if (!(action == "clear2"))
            break;
          this.clearScenarios(2);
          this.SetFieldFocus("l_1177");
          break;
        case 2487645161:
          if (!(action == "clear1"))
            break;
          this.clearScenarios(1);
          this.SetFieldFocus("l_1177");
          break;
        case 2775211744:
          if (!(action == "allotherpay"))
            break;
          this.refreshAllStopLights();
          this.SetFieldFocus("l_67");
          break;
        case 3220010881:
          if (!(action == "cashavailable"))
            break;
          this.refreshAllStopLights();
          this.SetFieldFocus("l_67");
          break;
        case 3443531722:
          if (!(action == "subfin"))
            break;
          this.SetFieldFocus("l_140");
          break;
        case 3540010302:
          if (!(action == "totalfinancing"))
            break;
          this.refreshAllStopLights();
          this.SetFieldFocus("l_67");
          break;
        case 3582764703:
          if (!(action == "loanprog"))
            break;
          if (new LoanProgramSelect(this.loan, this.session).ShowDialog((IWin32Window) this.mainScreen) == DialogResult.OK)
          {
            this.loan.Calculator.CalculateMaxLoanAmt();
            this.loan.Calculator.FormCalculation("LOANCOMP", (string) null, (string) null);
            this.refreshStopLights(0);
            this.UpdateContents();
          }
          this.SetFieldFocus("l_1177");
          break;
        case 4155076599:
          if (!(action == "ccprog"))
            break;
          if (new ClosingCostSelect(this.loan).ShowDialog((IWin32Window) this.mainScreen) == DialogResult.OK)
            this.UpdateContents();
          this.SetFieldFocus("l_1177");
          break;
      }
    }

    private void copyToScenario(int col)
    {
      int num = (int) Utils.Dialog((IWin32Window) null, "Values copied to Scenario 2 and Scenario 3 cannot be modified or copied back to Scenario 1.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      this.loan.CopyCurrentLoanScenarioToAlternative(col);
      this.copyCashFromTo(col);
      this.refreshStopLights(col);
      this.RefreshContents();
    }

    private void swapScenarios(int col)
    {
      this.copyCashFromTo(col);
      this.loan.SwapLoanScenario(col);
      this.loan.Calculator.CalculateAll();
      if (this.loan.GetSimpleField("1109") == "")
        this.loan.SetCurrentField("912", "");
      this.refreshStopLights(0);
      this.refreshStopLights(col);
      this.RefreshContents();
    }

    private void clearScenarios(int col)
    {
      this.loan.ClearAlternative(col);
      this.refreshStopLights(col);
      this.RefreshContents();
    }

    private void refreshAllStopLights()
    {
      this.refreshStopLights(0);
      this.refreshStopLights(1);
      this.refreshStopLights(2);
      this.editor.RefreshContents();
    }

    private void refreshStopLights(int col)
    {
      ImageButton control = (ImageButton) this.currentForm.FindControl("imgLight" + (object) col);
      if (control == null)
        return;
      string id = "PREQUAL.X274";
      switch (col)
      {
        case 1:
          id = "PREQUAL.X303";
          break;
        case 2:
          id = "PREQUAL.X304";
          break;
      }
      switch (this.GetFieldValue(id))
      {
        case "Green":
          control.Source = AssemblyResolver.GetResourceFileFullPath(Path.Combine(SystemSettings.FormRelDir, "images\\LightGreen.gif"), SystemSettings.LocalAppDir);
          break;
        case "Red":
          control.Source = AssemblyResolver.GetResourceFileFullPath(Path.Combine(SystemSettings.FormRelDir, "images\\LightRed.gif"), SystemSettings.LocalAppDir);
          break;
        default:
          control.Source = AssemblyResolver.GetResourceFileFullPath(Path.Combine(SystemSettings.FormRelDir, "images\\LightYellow.gif"), SystemSettings.LocalAppDir);
          break;
      }
    }

    private void refreshCashFromTo(int col)
    {
      EllieMae.Encompass.Forms.Label control = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("lblCashFromTo" + (object) col);
      if (control == null)
        return;
      string id = "142";
      switch (col)
      {
        case 1:
          id = "PREQUAL.X44";
          break;
        case 2:
          id = "PREQUAL.X84";
          break;
      }
      if (this.ToDouble(this.loan.GetSimpleField(id)) > 0.0)
        control.Text = "From";
      else
        control.Text = "To";
    }

    private void copyCashFromTo(int col)
    {
      EllieMae.Encompass.Forms.Label control1 = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("lblCashFromTo0");
      if (control1 == null)
        return;
      EllieMae.Encompass.Forms.Label control2 = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("lblCashFromTo" + (object) col);
      if (control2 == null)
        return;
      control2.Text = control1.Text;
    }
  }
}
