// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.WORKSHEETInputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.Forms;
using mshtml;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class WORKSHEETInputHandler : InputHandlerBase
  {
    public WORKSHEETInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public WORKSHEETInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public WORKSHEETInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public WORKSHEETInputHandler(
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
      ControlState controlState = this.getControlState(ctrl, id, ControlState.Enabled);
      switch (id)
      {
        case "1061":
        case "436":
          if (this.loan != null && (this.loan.Use2010RESPA || this.loan.Use2015RESPA) && (this.GetFieldValue("NEWHUD.X1139") == "Y" || this.GetFieldValue("NEWHUD.X715") != "Include Origination Points" || Utils.ParseDouble((object) this.GetFieldValue("3119")) > 0.0))
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "1063":
          return controlState;
        case "ccprog":
          if (this.FormIsForTemplate)
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "clearcc":
          if (this.loan.GetField("1785") == string.Empty)
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "clearlp":
          if (this.loan.GetField("1401") == string.Empty)
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "loanprog":
        case "vol":
          if (this.FormIsForTemplate || this.loan.IsFieldReadOnly("1401"))
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        default:
          controlState = ControlState.Default;
          goto case "1063";
      }
    }

    public override void ExecAction(string action)
    {
      switch (action)
      {
        case "clearlp":
          this.UpdateFieldValue("1401", "");
          this.UpdateFieldValue("2861", "");
          this.UpdateContents();
          this.SetFieldFocus("l_388");
          break;
        case "clearcc":
          this.UpdateFieldValue("1785", "");
          this.UpdateFieldValue("2862", "");
          this.UpdateContents();
          this.SetFieldFocus("l_1109");
          break;
        case "vol":
          this.loadPrequalQuickLink("Other Payoffs", "VOLPanel", 700, 512);
          break;
        case "calcnew":
          this.loan.Calculator.CalculateNewLoan();
          this.UpdateContents();
          this.SetFieldFocus("l_1109");
          break;
        case "loanprog":
          if (new LoanProgramSelect(this.loan, this.session).ShowDialog((IWin32Window) this.mainScreen) == DialogResult.OK)
          {
            this.loan.Calculator.CalculateAll();
            this.UpdateContents();
          }
          this.SetFieldFocus("l_388");
          break;
        case "ccprog":
          if (new ClosingCostSelect(this.loan).ShowDialog((IWin32Window) this.mainScreen) == DialogResult.OK)
          {
            this.loan.Calculator.CalculateAll();
            this.UpdateContents();
          }
          this.SetFieldFocus("l_1109");
          break;
      }
    }

    private void loadPrequalQuickLink(
      string formTitle,
      string htmFile,
      int sizeWidth,
      int sizeHeight)
    {
      using (QuickEntryPopupDialog entryPopupDialog = new QuickEntryPopupDialog((IHtmlInput) this.loan, formTitle, new InputFormInfo(htmFile, htmFile), sizeWidth, sizeHeight, FieldSource.CurrentLoan, "", this.session))
      {
        int num = (int) entryPopupDialog.ShowDialog((IWin32Window) Session.MainForm);
      }
      this.loan.Calculator.FormCalculation("LOANCOMP", (string) null, (string) null);
      this.editor.RefreshContents();
    }
  }
}
