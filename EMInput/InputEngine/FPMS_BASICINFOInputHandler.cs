// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.FPMS_BASICINFOInputHandler
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
  public class FPMS_BASICINFOInputHandler : InputHandlerBase
  {
    private ImageButton btnInfoEnter;
    private ImageButton btnInfoLeave;
    private ImageButton btn3093;
    private EllieMae.Encompass.Forms.GroupBox box3093;
    private EllieMae.Encompass.Forms.Label label3093;

    public FPMS_BASICINFOInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public FPMS_BASICINFOInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public FPMS_BASICINFOInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public FPMS_BASICINFOInputHandler(
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
      base.CreateControls();
      this.btn3093 = this.currentForm.FindControl("btn3093") as ImageButton;
      this.box3093 = this.currentForm.FindControl("box3093") as EllieMae.Encompass.Forms.GroupBox;
      this.label3093 = this.currentForm.FindControl("label3093") as EllieMae.Encompass.Forms.Label;
      this.btnInfoEnter = this.currentForm.FindControl("btnInfoEnter") as ImageButton;
      this.btnInfoLeave = this.currentForm.FindControl("btnInfoLeave") as ImageButton;
    }

    public override void onmouseenter(mshtml.IHTMLEventObj pEvtObj)
    {
      if (pEvtObj.srcElement.id == "btn3093")
      {
        this.btn3093.Source = this.btnInfoEnter.Source;
        this.label3093.Text = "The property has been bought less a year ago and is bing sold again!!";
        this.box3093.Visible = true;
      }
      base.onmouseenter(pEvtObj);
    }

    public override void onmouseleave(mshtml.IHTMLEventObj pEvtObj)
    {
      if (!(pEvtObj.srcElement.id == "btn3093"))
        return;
      this.btn3093.Source = this.btnInfoLeave.Source;
      this.box3093.Visible = false;
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      ControlState defaultValue = this.getControlState(ctrl, id, ControlState.Enabled);
      switch (id)
      {
        case "1063":
        case "11":
        case "FR0104":
        case "FR0204":
          return defaultValue;
        case "3052":
          if (this.GetFieldValue("1172") == "FHA" && this.GetFieldValue("19") == "NoCash-Out Refinance" || this.GetFieldValue("19").IndexOf("Refinance") == -1 || this.GetFieldValue("3000") != "Y" && this.GetFieldValue("2997") != "Y")
          {
            defaultValue = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "3093":
          this.btn3093.Visible = false;
          if (this.GetFieldValue("3093") != "Y")
          {
            DateTime dateTime = Utils.ParseDate((object) this.GetFieldValue("3058"));
            DateTime date = Utils.ParseDate((object) this.GetFieldValue("748"));
            if (dateTime != DateTime.MinValue && date != DateTime.MinValue)
            {
              dateTime = dateTime.AddYears(1);
              if (dateTime > date)
              {
                this.btn3093.Visible = true;
                goto case "1063";
              }
              else
                goto case "1063";
            }
            else
              goto case "1063";
          }
          else
            goto case "1063";
        case "364":
          if (this.FormIsForTemplate || InputHandlerBase.LockLoanNumber)
          {
            defaultValue = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "9":
          if (this.loan.GetField("19") != "Other")
          {
            defaultValue = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "MORNET.X41":
          defaultValue = this.GetSpecialControlStatus(id, defaultValue);
          goto case "1063";
        default:
          defaultValue = ControlState.Default;
          goto case "1063";
      }
    }

    public override void ExecAction(string action)
    {
      base.ExecAction(action);
      if (!(action == "copybrw"))
        return;
      this.SetFieldFocus("l_68");
    }
  }
}
