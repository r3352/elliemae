// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.FPMS_REFIInputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.Forms;
using mshtml;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class FPMS_REFIInputHandler : InputHandlerBase
  {
    private ImageButton btnInfoEnter;
    private ImageButton btnInfoLeave;
    private ImageButton btn740;
    private EllieMae.Encompass.Forms.GroupBox box740;
    private EllieMae.Encompass.Forms.Label label740;
    private ImageButton btn742;
    private EllieMae.Encompass.Forms.GroupBox box742;
    private EllieMae.Encompass.Forms.Label label742;
    private ImageButton btn1109;
    private EllieMae.Encompass.Forms.GroupBox box1109;
    private EllieMae.Encompass.Forms.Label label1109;
    private ImageButton btn1045;
    private EllieMae.Encompass.Forms.GroupBox box1045;
    private EllieMae.Encompass.Forms.Label label1045;
    private EllieMae.Encompass.Forms.Panel panelNoCashOut;
    private EllieMae.Encompass.Forms.Panel panelCashOut;
    private EllieMae.Encompass.Forms.Panel panelCashOut1;
    private EllieMae.Encompass.Forms.Panel panelCashOut2;
    private EllieMae.Encompass.Forms.Panel panelNoCashOutNoApp;
    private EllieMae.Encompass.Forms.Panel panelNoCashOutWithApp;
    private EllieMae.Encompass.Forms.Label labelFormName;
    private EllieMae.Encompass.Forms.Panel panelEEM;
    private EllieMae.Encompass.Forms.Panel pnlForm;
    private EllieMae.Encompass.Forms.GroupBox groupMortgage;
    private EllieMae.Encompass.Forms.Panel panelAll;

    public FPMS_REFIInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public FPMS_REFIInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public FPMS_REFIInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public FPMS_REFIInputHandler(
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
      this.btn740 = this.currentForm.FindControl("btn740") as ImageButton;
      this.box740 = this.currentForm.FindControl("box740") as EllieMae.Encompass.Forms.GroupBox;
      this.label740 = this.currentForm.FindControl("label740") as EllieMae.Encompass.Forms.Label;
      this.btn742 = this.currentForm.FindControl("btn742") as ImageButton;
      this.box742 = this.currentForm.FindControl("box742") as EllieMae.Encompass.Forms.GroupBox;
      this.label742 = this.currentForm.FindControl("label742") as EllieMae.Encompass.Forms.Label;
      this.btn1109 = this.currentForm.FindControl("btn1109") as ImageButton;
      this.box1109 = this.currentForm.FindControl("box1109") as EllieMae.Encompass.Forms.GroupBox;
      this.label1109 = this.currentForm.FindControl("label1109") as EllieMae.Encompass.Forms.Label;
      this.btn1045 = this.currentForm.FindControl("btn1045") as ImageButton;
      this.box1045 = this.currentForm.FindControl("box1045") as EllieMae.Encompass.Forms.GroupBox;
      this.label1045 = this.currentForm.FindControl("label1045") as EllieMae.Encompass.Forms.Label;
      this.btnInfoEnter = this.currentForm.FindControl("btnInfoEnter") as ImageButton;
      this.btnInfoLeave = this.currentForm.FindControl("btnInfoLeave") as ImageButton;
      this.panelNoCashOut = this.currentForm.FindControl("panelNoCashOut") as EllieMae.Encompass.Forms.Panel;
      this.panelCashOut = this.currentForm.FindControl("panelCashOut") as EllieMae.Encompass.Forms.Panel;
      this.panelCashOut1 = this.currentForm.FindControl("panelCashOut1") as EllieMae.Encompass.Forms.Panel;
      this.panelCashOut2 = this.currentForm.FindControl("panelCashOut2") as EllieMae.Encompass.Forms.Panel;
      this.panelNoCashOutNoApp = this.currentForm.FindControl("panelNoCashOutNoApp") as EllieMae.Encompass.Forms.Panel;
      this.panelNoCashOutWithApp = this.currentForm.FindControl("panelNoCashOutWithApp") as EllieMae.Encompass.Forms.Panel;
      this.panelEEM = this.currentForm.FindControl("panelEEM") as EllieMae.Encompass.Forms.Panel;
      this.labelFormName = this.currentForm.FindControl("labelFormName") as EllieMae.Encompass.Forms.Label;
      this.groupMortgage = this.currentForm.FindControl("groupMortgage") as EllieMae.Encompass.Forms.GroupBox;
      this.panelAll = this.currentForm.FindControl("panelAll") as EllieMae.Encompass.Forms.Panel;
      this.pnlForm = this.currentForm.FindControl("pnlForm") as EllieMae.Encompass.Forms.Panel;
      this.panelNoCashOut.Position = new Point(0, 0);
      this.panelCashOut.Position = new Point(0, 0);
      this.panelNoCashOutNoApp.Position = new Point(0, 0);
      this.panelNoCashOutWithApp.Position = new Point(0, 0);
      this.panelCashOut2.Position = new Point(this.panelCashOut1.Left, this.panelCashOut1.Top);
    }

    public override void onmouseenter(mshtml.IHTMLEventObj pEvtObj)
    {
      switch (pEvtObj.srcElement.id)
      {
        case "btn740":
          this.btn740.Source = this.btnInfoEnter.Source;
          this.label740.Text = "Borrower's Top Ratio is not qualified. Maximum Allowed: " + this.GetFieldValue("1790") + "%.";
          this.box740.Visible = true;
          break;
        case "btn742":
          this.btn742.Source = this.btnInfoEnter.Source;
          this.label742.Text = "Borrower's Bottom Ratio is not qualified. Maximum Allowed: " + this.GetFieldValue("1791") + "%.";
          this.box742.Visible = true;
          break;
        case "btn1109":
          this.btn1109.Source = this.btnInfoEnter.Source;
          this.label1109.Text = "The base loan amount exceeds FHA County loan limited amount " + this.GetFieldValue("MCAWPUR.X27") + "!";
          this.box1109.Visible = true;
          break;
        case "btn1045":
          this.btn1045.Source = this.btnInfoEnter.Source;
          this.label1045.Text = "The Upfront MIP cannot be blank if loan is FHA!!";
          this.box1045.Visible = true;
          break;
      }
      base.onmouseenter(pEvtObj);
    }

    public override void onmouseleave(mshtml.IHTMLEventObj pEvtObj)
    {
      switch (pEvtObj.srcElement.id)
      {
        case "btn740":
          this.btn740.Source = this.btnInfoLeave.Source;
          this.box740.Visible = false;
          break;
        case "btn742":
          this.btn742.Source = this.btnInfoLeave.Source;
          this.box742.Visible = false;
          break;
        case "btn1109":
          this.btn1109.Source = this.btnInfoLeave.Source;
          this.box1109.Visible = false;
          break;
        case "btn1045":
          this.btn1045.Source = this.btnInfoLeave.Source;
          this.box1045.Visible = false;
          break;
      }
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      ControlState defaultValue = this.getControlState(ctrl, id, ControlState.Enabled);
      switch (id)
      {
        case "1045":
          if (Utils.ParseDouble((object) this.GetFieldValue("1045")) == 0.0 && this.GetFieldValue("1172") == "FHA")
          {
            this.btn1045.Visible = true;
            break;
          }
          this.btn1045.Visible = false;
          break;
        case "1109":
          if (Utils.ParseDouble((object) this.GetFieldValue("MCAWPUR.X27")) != 0.0 && Utils.ParseDouble((object) this.GetFieldValue("MCAWPUR.X27")) < Utils.ParseDouble((object) this.GetFieldValue("1109")))
          {
            this.btn1109.Visible = true;
            break;
          }
          this.btn1109.Visible = false;
          break;
        case "1790":
          if (Utils.ParseDouble((object) this.GetFieldValue("1790")) == 0.0 || Utils.ParseDouble((object) this.GetFieldValue("1790")) >= Utils.ParseDouble((object) this.GetFieldValue("740")))
          {
            this.btn740.Visible = false;
            break;
          }
          this.btn740.Visible = true;
          break;
        case "1791":
          if (Utils.ParseDouble((object) this.GetFieldValue("1791")) == 0.0 || Utils.ParseDouble((object) this.GetFieldValue("1791")) >= Utils.ParseDouble((object) this.GetFieldValue("742")))
          {
            this.btn742.Visible = false;
            break;
          }
          this.btn742.Visible = true;
          break;
        case "3052":
          if (this.GetFieldValue("1172") == "FHA" && this.GetFieldValue("19") == "NoCash-Out Refinance" || this.GetFieldValue("19").IndexOf("Refinance") == -1 || this.GetFieldValue("3000") != "Y" && this.GetFieldValue("2997") != "Y")
          {
            defaultValue = ControlState.Disabled;
            break;
          }
          break;
        case "9":
          if (this.loan.GetField("19") != "Other")
          {
            defaultValue = ControlState.Disabled;
            break;
          }
          break;
        case "MORNET.X41":
          defaultValue = this.GetSpecialControlStatus(id, defaultValue);
          if (this.GetFieldValue("19") == "NoCash-Out Refinance")
          {
            this.panelCashOut1.Visible = this.panelCashOut2.Visible = false;
            if (this.GetFieldValue("MORNET.X40") == "StreamlineWithoutAppraisal")
            {
              this.panelNoCashOut.Visible = this.panelCashOut.Visible = this.panelNoCashOutWithApp.Visible = false;
              this.panelNoCashOutNoApp.Visible = true;
              this.groupMortgage.Size = new Size(this.groupMortgage.Size.Width, this.panelNoCashOutNoApp.Size.Height + 22);
            }
            else if (this.GetFieldValue("MORNET.X40") == "StreamlineWithAppraisal")
            {
              this.panelNoCashOut.Visible = this.panelCashOut.Visible = this.panelNoCashOutNoApp.Visible = false;
              this.panelNoCashOutWithApp.Visible = true;
              this.groupMortgage.Size = new Size(this.groupMortgage.Size.Width, this.panelNoCashOutWithApp.Size.Height + 22);
            }
            else
            {
              this.panelNoCashOutNoApp.Visible = this.panelCashOut.Visible = this.panelNoCashOutWithApp.Visible = false;
              this.panelNoCashOut.Visible = true;
              this.groupMortgage.Size = new Size(this.groupMortgage.Size.Width, this.panelNoCashOut.Size.Height + 22);
            }
          }
          else if (this.GetFieldValue("19") == "Cash-Out Refinance")
          {
            this.panelNoCashOut.Visible = this.panelNoCashOutNoApp.Visible = this.panelNoCashOutWithApp.Visible = false;
            this.panelCashOut.Visible = true;
            this.groupMortgage.Size = new Size(this.groupMortgage.Size.Width, this.panelCashOut.Size.Height + 22);
            int totalTimeSpanMonths = Utils.GetTotalTimeSpanMonths(this.GetField("1518"), this.GetField("745"), false);
            if (totalTimeSpanMonths <= 12 && totalTimeSpanMonths >= 0)
            {
              this.panelCashOut1.Visible = false;
              this.panelCashOut2.Visible = true;
            }
            else if (totalTimeSpanMonths > 12)
            {
              this.panelCashOut1.Visible = true;
              this.panelCashOut2.Visible = false;
            }
            else
            {
              this.panelCashOut1.Visible = false;
              this.panelCashOut2.Visible = false;
            }
          }
          this.panelEEM.Top = this.groupMortgage.Top + this.groupMortgage.Size.Height;
          this.panelAll.Size = new Size(this.pnlForm.Size.Width, this.panelEEM.Top + this.panelEEM.Size.Height + 4);
          EllieMae.Encompass.Forms.Panel pnlForm = this.pnlForm;
          int width = this.pnlForm.Size.Width;
          Size size1 = this.panelAll.Size;
          int height = size1.Height + 2;
          Size size2 = new Size(width, height);
          pnlForm.Size = size2;
          EllieMae.Encompass.Forms.Label labelFormName = this.labelFormName;
          size1 = this.pnlForm.Size;
          int num = size1.Height + 6;
          labelFormName.Top = num;
          break;
        default:
          defaultValue = ControlState.Default;
          break;
      }
      return defaultValue;
    }

    public override void ExecAction(string action)
    {
      base.ExecAction(action);
      switch (action)
      {
        case "fhamaxloan":
          this.SetFieldFocus("l_1720");
          break;
        case "incomeexpense":
          this.SetFieldFocus("l_1791");
          break;
        case "eem":
          try
          {
            this.loan.Calculator.FormCalculation("MCAWREFI", "1228", "");
          }
          catch (Exception ex)
          {
            int num = (int) Utils.Dialog((IWin32Window) System.Windows.Forms.Form.ActiveForm, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }
          this.UpdateContents();
          this.SetFieldFocus("l_1228");
          break;
      }
    }
  }
}
