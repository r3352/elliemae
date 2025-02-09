// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.D10031InputHandler
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
  internal class D10031InputHandler : InputHandlerBase
  {
    private FieldLock fl_26;

    public D10031InputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public D10031InputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public D10031InputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public D10031InputHandler(
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
        if (this.fl_26 != null)
          return;
        this.fl_26 = (FieldLock) this.currentForm.FindControl("FieldLock4");
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
        case "1034":
          if (this.loan.GetField("1066") != "Leasehold")
          {
            controlState = ControlState.Disabled;
            this.SetControlState("Calendar1", false);
            goto case "1063";
          }
          else
          {
            this.SetControlState("Calendar1", true);
            goto case "1063";
          }
        case "1063":
        case "11":
        case "1416":
        case "1519":
        case "319":
        case "FE0104":
        case "FE0204":
        case "FR0104":
        case "FR0204":
        case "FR0304":
        case "FR0404":
          return controlState;
        case "1266":
        case "1267":
          if (this.loan.GetField("608") != "GraduatedPaymentMortgage")
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "1417":
        case "1418":
        case "1419":
          controlState = !(this.loan.GetField("1819") == "Y") ? ControlState.Enabled : ControlState.Disabled;
          goto case "1063";
        case "1520":
        case "1521":
        case "1522":
          controlState = !(this.loan.GetField("1820") == "Y") ? ControlState.Enabled : ControlState.Disabled;
          goto case "1063";
        case "205":
        case "24":
        case "25":
        case "27":
        case "29":
        case "30":
          string field1 = this.loan.GetField("19");
          if (field1.IndexOf("Refinance") < 0 && field1.IndexOf("Construction") < 0)
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "26":
          string field2 = this.loan.GetField("19");
          if (field2.IndexOf("Refinance") < 0 && field2.IndexOf("Construction") < 0)
          {
            controlState = ControlState.Disabled;
            if (this.fl_26 != null)
              this.fl_26.Enabled = false;
          }
          if ((field2.IndexOf("Refinance") >= 0 || field2.IndexOf("Construction") >= 0) && this.fl_26 != null)
          {
            this.fl_26.Enabled = true;
            if (!this.fl_26.Locked)
            {
              controlState = ControlState.Disabled;
              goto case "1063";
            }
            else
              goto case "1063";
          }
          else
            goto case "1063";
        case "299":
          controlState = this.loan.GetField("19") == "Purchase" || this.loan.GetField("19") == "Other" || this.loan.GetField("19") == "" ? ControlState.Disabled : ControlState.Enabled;
          goto case "1063";
        case "4494":
          return this.loan.GetField("420") == "FirstLien" || this.loan.GetField("420") == "" ? ControlState.Disabled : ControlState.Enabled;
        case "9":
          if (this.loan.GetField("19") != "Other")
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "994":
          if (this.loan.GetField("608") != "OtherAmortizationType")
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "995":
          if (this.loan.GetField("608") != "AdjustableRate" && this.inputData.GetField("1172") != "HELOC")
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "QM.X2":
          string field3 = this.GetField("19");
          if (field3 != "NoCash-Out Refinance" && field3 != "Cash-Out Refinance")
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "subfin":
        case "voe":
        case "vor":
          if (this.FormIsForTemplate)
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "zoomarm":
          if (this.loan.GetField("608") != "AdjustableRate")
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

    internal override void UpdateFieldValue(string id, string val)
    {
      if (id == "4494" && (val == "" || val == "1") && this.loan.GetField("420") == "SecondLien")
        val = this.loan.GetField("4494");
      base.UpdateFieldValue(id, val);
      if (!(id == "608"))
        return;
      if (val != "OtherAmortizationType")
        base.UpdateFieldValue("994", string.Empty);
      if (!(val != "GraduatedPaymentMortgage"))
        return;
      base.UpdateFieldValue("1266", string.Empty);
      base.UpdateFieldValue("1267", string.Empty);
    }

    public override void ExecAction(string action)
    {
      switch (action)
      {
        case "copybrw":
          base.ExecAction(action);
          this.SetFieldFocus("l_68");
          break;
        case "voe":
        case "vor":
          base.ExecAction(action);
          break;
        case "zoomarm":
          ARMTypeDetails armTypeDetails = new ARMTypeDetails("995", this.loan.GetSimpleField("995"));
          if (armTypeDetails.ShowDialog((IWin32Window) this.mainScreen) == DialogResult.OK)
          {
            this.loan.SetCurrentField("995", armTypeDetails.ArmTypeID);
            this.UpdateContents();
          }
          this.SetFieldFocus("l_995");
          break;
        case "subfin":
          base.ExecAction(action);
          this.SetFieldFocus("I_8");
          break;
        case "mipff":
          base.ExecAction(action);
          this.SetFieldFocus("l_3");
          break;
      }
    }
  }
}
