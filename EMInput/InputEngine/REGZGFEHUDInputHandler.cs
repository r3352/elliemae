// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.REGZGFEHUDInputHandler
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
  public class REGZGFEHUDInputHandler : InputHandlerBase
  {
    public REGZGFEHUDInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public REGZGFEHUDInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public REGZGFEHUDInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public REGZGFEHUDInputHandler(
      Sessions.Session session,
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, dataTemplate, htmldoc, form, property)
    {
    }

    public REGZGFEHUDInputHandler(
      IWin32Window owner,
      IHtmlInput htmlInput,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, htmlInput, htmldoc, form, property)
    {
    }

    public REGZGFEHUDInputHandler(
      Sessions.Session session,
      IWin32Window owner,
      IHtmlInput htmlInput,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, htmlInput, htmldoc, form, property)
    {
    }

    internal override void CreateControls()
    {
      if (this.loan == null || this.loan.Calculator == null || !this.loan.Calculator.TriggerTradeOffCalculation)
        return;
      this.loan.Calculator.FormCalculation("TRADEOFFTABLE", (string) null, (string) null);
    }

    public override void onkeyup(mshtml.IHTMLEventObj pEvtObj)
    {
      if (this.currentForm.FindControlForElement(pEvtObj.srcElement) is FieldControl controlForElement && controlForElement.Field.FieldID == "NEWHUD.X1")
      {
        bool needsUpdate = false;
        if (controlForElement.Value.ToLower() != "n" && controlForElement.Value.ToLower() != "na")
        {
          string str = Utils.FormatInput(controlForElement.Value, FieldFormat.DATE, ref needsUpdate);
          if (needsUpdate)
            controlForElement.BindTo(str);
        }
      }
      base.onkeyup(pEvtObj);
    }

    internal override void UpdateFieldValue(string id, string val)
    {
      if (this.inputData is DisclosedHUDGFEHandler)
        return;
      base.UpdateFieldValue(id, val);
      if (this.loan == null)
        return;
      if (id == "3168")
      {
        this.loan.Calculator.FormCalculation("3168", id, val);
      }
      else
      {
        if (!(id != "3166"))
          return;
        this.loan.Calculator.FormCalculation("RegzGFEHUD", id, val);
      }
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      if (this.inputData is DisclosedHUDGFEHandler)
      {
        switch (id)
        {
          case "3165":
            this.SetControlState("Calendar8", false);
            break;
          case "3170":
            this.SetControlState("Calendar1", false);
            break;
          case "3197":
            this.SetControlState("Calendar7", false);
            break;
          case "682":
            this.SetControlState("Calendar3", false);
            break;
          case "761":
            this.SetControlState("Calendar5", false);
            break;
          case "762":
            this.SetControlState("Calendar4", false);
            break;
          case "763":
            this.SetControlState("Calendar2", false);
            break;
          case "NEWHUD.X1":
            this.SetControlState("CalendarX1", false);
            break;
          case "NEWHUD.X2":
            this.SetControlState("Calendar6", false);
            break;
        }
        return ControlState.Disabled;
      }
      ControlState controlState = this.getControlState(ctrl, id, ControlState.Enabled);
      switch (id)
      {
        case "1063":
          return controlState;
        case "1266":
        case "1267":
          if (this.GetFieldValue("608") != "GraduatedPaymentMortgage")
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "3164":
          if (this.loan.GetField("3149") == "" || this.loan.GetField("3149") == "//" || this.loan.GetField("3153") == "" || this.loan.GetField("3153") == "//")
            return ControlState.Disabled;
          goto case "1063";
        case "3165":
        case "3166":
        case "3169":
          if (this.FormIsForTemplate)
          {
            controlState = ControlState.Default;
            goto case "1063";
          }
          else
          {
            if (this.GetFieldValue("3168") != "Y")
              controlState = ControlState.Disabled;
            if (id == "3165")
            {
              this.SetControlState("Calendar8", this.GetFieldValue("3168") == "Y");
              goto case "1063";
            }
            else
              goto case "1063";
          }
        case "3197":
          this.SetControlState("Calendar7", this.GetFieldValue("3164") == "Y");
          if (this.loan.GetField("3164") != "Y")
            return ControlState.Disabled;
          goto case "1063";
        case "432":
          if (this.inputData.GetField("3941") == "Y")
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "761":
          if (this.GetField("3941") == "Y")
          {
            ((RuntimeControl) this.currentForm.FindControl("Calendar5")).Enabled = false;
            return ControlState.Disabled;
          }
          ((RuntimeControl) this.currentForm.FindControl("Calendar5")).Enabled = true;
          goto case "1063";
        case "762":
          if (this.GetField("3941") == "Y")
          {
            ((RuntimeControl) this.currentForm.FindControl("Calendar4")).Enabled = false;
            return ControlState.Disabled;
          }
          ((RuntimeControl) this.currentForm.FindControl("Calendar4")).Enabled = true;
          goto case "1063";
        case "9":
          if (this.GetFieldValue("19") != "Other")
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "994":
          if (this.GetFieldValue("608") != "OtherAmortizationType")
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "995":
        case "zoomarm":
          if (this.GetFieldValue("608") != "AdjustableRate" && this.inputData.GetField("1172") != "HELOC")
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "NEWHUD.X1":
          if (this.loan != null && this.GetFieldValue("2400") == "Y")
          {
            controlState = ControlState.Disabled;
            this.SetControlState("CalendarX1", false);
            goto case "1063";
          }
          else
          {
            this.SetControlState("CalendarX1", true);
            goto case "1063";
          }
        case "NEWHUD.X14":
          if (this.GetFieldValue("NEWHUD.X125") == "Y" || Utils.ParseInt((object) this.GetFieldValue("1847")) != 0)
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "NEWHUD.X3":
        case "NEWHUD.X39":
          this.FormatAlphaNumericField(ctrl, id);
          controlState = ControlState.Default;
          goto case "1063";
        case "NEWHUD.X5":
          if (this.GetFieldValue("608") != "OtherAmortizationType")
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "NEWHUD.X555":
          if (this.GetFieldValue("NEWHUD.X5") != "Yes")
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "NEWHUD.X7":
          if (this.GetFieldValue("NEWHUD.X6") != "Yes")
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "NEWHUD.X719":
          if (this.loan != null && this.GetFieldValue("2400") == "Y")
            controlState = ControlState.Disabled;
          this.FormatAlphaNumericField(ctrl, id);
          goto case "1063";
        case "NEWHUD.X725":
          if (this.loan != null && this.GetFieldValue("2400") == "Y" && (this.GetFieldValue("NEWHUD.X1") == "" || this.GetFieldValue("NEWHUD.X1") == "//"))
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "NEWHUD.X78":
          if (this.GetFieldValue("NEWHUD.X351") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "NEWHUD.X8":
          if (this.GetFieldValue("NEWHUD.X8") != "Y")
          {
            this.SetControlState("lock_X9", false);
            this.SetControlState("lock_X10", false);
            this.SetControlState("lock_X11", false);
            goto case "1063";
          }
          else
          {
            this.SetControlState("lock_X9", true);
            this.SetControlState("lock_X10", true);
            this.SetControlState("lock_X11", true);
            goto case "1063";
          }
        case "lookup3169":
          if (this.GetFieldValue("3168") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "openitemization":
          if (this.FormIsForTemplate)
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "viewworstcase":
          if (this.loan == null || this.FormIsForTemplate || this.GetFieldValue("608") != "AdjustableRate" || this.loan.IsLocked("NEWHUD.X11"))
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

    public override void ExecAction(string action) => base.ExecAction(action);
  }
}
