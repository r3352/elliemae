// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.LOANSUBInputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.Forms;
using mshtml;
using System.Collections.Generic;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class LOANSUBInputHandler : InputHandlerBase
  {
    private EllieMae.Encompass.Forms.Panel pnl2009;
    private EllieMae.Encompass.Forms.Panel pnl2020;
    private List<RuntimeControl> panel_Exp_Controls;

    public LOANSUBInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public LOANSUBInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public LOANSUBInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public LOANSUBInputHandler(
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
        case "101":
        case "102":
        case "103":
        case "104":
        case "107":
        case "108":
        case "11":
        case "110":
        case "111":
        case "112":
        case "113":
        case "116":
        case "117":
        case "319":
          return controlState;
        case "1061":
        case "436":
          if (this.loan != null && (this.loan.Use2010RESPA || this.loan.Use2015RESPA) && (this.GetFieldValue("NEWHUD.X1139") == "Y" || this.GetFieldValue("NEWHUD.X715") != "Include Origination Points" || Utils.ParseDouble((object) this.GetFieldValue("3119")) > 0.0))
          {
            controlState = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        case "1198":
        case "1199":
        case "1200":
        case "1201":
          if (this.loan.GetField("3533") == "Y")
          {
            controlState = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        case "1205":
          if (this.loan.GetField("1172") == "FarmersHomeAdministration" || this.loan.GetField("3533") == "Y")
          {
            controlState = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        case "1619":
          if (this.loan != null && this.loan.GetField("1172") == "VA" && this.loan.GetField("958") == "IRRRL" && this.loan.GetField("19").IndexOf("Refinance") > -1)
          {
            controlState = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        case "1stmor":
          if (this.GetField("420") != "SecondLien")
          {
            controlState = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        case "229":
          if (this.GetField("1825") == "2020")
          {
            controlState = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        case "432":
        case "761":
        case "762":
          if (this.GetField("3941") == "Y")
          {
            ((RuntimeControl) this.currentForm.FindControl("Calendar2")).Enabled = false;
            ((RuntimeControl) this.currentForm.FindControl("Calendar3")).Enabled = false;
            return ControlState.Disabled;
          }
          ((RuntimeControl) this.currentForm.FindControl("Calendar2")).Enabled = true;
          ((RuntimeControl) this.currentForm.FindControl("Calendar3")).Enabled = true;
          goto case "101";
        case "9":
          if (this.loan.GetField("19") != "Other")
          {
            controlState = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        case "LOANSUB.X15":
          if (this.loan.GetField("LOANSUB.X14") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        case "baseincome":
          if (this.GetField("1825") == "2020")
          {
            controlState = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        case "otherf":
          if (this.GetField("420") == "SecondLien")
          {
            controlState = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        case "subfin":
          if (this.FormIsForTemplate)
          {
            controlState = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        default:
          controlState = ControlState.Default;
          goto case "101";
      }
    }

    internal override void UpdateFieldValue(string id, string val)
    {
      if (id == "230")
      {
        double num = this.ToDouble(val);
        if (num != 0.0)
          val = num.ToString("N2");
      }
      base.UpdateFieldValue(id, val);
    }

    public override void ExecAction(string action)
    {
      base.ExecAction(action);
      switch (action)
      {
        case "mipff":
          this.SetFieldFocus("l_430");
          break;
        case "loanprog":
        case "subfin":
          this.SetFieldFocus("l_1041");
          break;
        case "mtginsreserv":
          if (this.inputData.IsLocked("232"))
          {
            this.SetFieldFocus("l_232");
            break;
          }
          this.SetFieldFocus("l_124");
          break;
      }
    }

    internal override void CreateControls()
    {
      try
      {
        if (this.pnl2009 == null)
          this.pnl2009 = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnl2009");
        if (this.pnl2020 == null)
          this.pnl2020 = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnl2020");
        if (this.pnl2009 != null && this.pnl2020 != null)
        {
          if (this.GetField("1825") == "2020")
            this.pnl2009.Visible = !(this.pnl2020.Visible = true);
          else
            this.pnl2009.Visible = !(this.pnl2020.Visible = false);
        }
        this.panel_Exp_Controls = SharedURLAUIHandler.GetControls(this.currentForm, "panel_Exp_2009", "panel_Exp_2020");
      }
      catch
      {
      }
    }

    public override void RefreshContents(bool skipButtonFieldLockRules)
    {
      base.RefreshContents(skipButtonFieldLockRules);
      SharedURLAUIHandler.DisplayControls(this.loan.Use2020URLA, this.panel_Exp_Controls);
    }
  }
}
