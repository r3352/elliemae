// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.CURRENTLOANINFOInputHandler
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
  internal class CURRENTLOANINFOInputHandler : InputHandlerBase
  {
    private bool useURLA2020;
    private bool isFHA;
    private bool isSecondHome;
    private EllieMae.Encompass.Forms.Label lblFHASecResidence;
    private EllieMae.Encompass.Forms.CheckBox chkSecResidence;
    private DropdownBox cboLoanType;
    private DropdownBox cboOcupancyType;
    private EllieMae.Encompass.Forms.Label Label11;
    private DropdownBox DropdownBox7;
    private EllieMae.Encompass.Forms.Label Label13;
    private EllieMae.Encompass.Forms.TextBox TextBox3;
    private EllieMae.Encompass.Forms.Label Label67;
    private EllieMae.Encompass.Forms.Label Label68;
    private EllieMae.Encompass.Forms.TextBox TextBox67;
    private EllieMae.Encompass.Forms.Label Label69;
    private EllieMae.Encompass.Forms.TextBox TextBox68;
    private EllieMae.Encompass.Forms.Label Label70;
    private EllieMae.Encompass.Forms.TextBox TextBox69;
    private ZipCodeLookup ZipCodeLookup1;
    private EllieMae.Encompass.Forms.Label Label71;
    private EllieMae.Encompass.Forms.TextBox TextBox70;
    private EllieMae.Encompass.Forms.TextBox tbURLAX73;
    private EllieMae.Encompass.Forms.Panel pnlURLA2020;
    private EllieMae.Encompass.Forms.Panel pnlNonURLA2020;
    private EllieMae.Encompass.Forms.TextBox tbField11;

    public CURRENTLOANINFOInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public CURRENTLOANINFOInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public CURRENTLOANINFOInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public CURRENTLOANINFOInputHandler(
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
      this.useURLA2020 = this.session.StartupInfo.AllowURLA2020 && this.inputData.GetField("1825") == "2020";
      this.lblFHASecResidence = this.currentForm.FindControl("Label10") as EllieMae.Encompass.Forms.Label;
      this.chkSecResidence = this.currentForm.FindControl("CheckBox4") as EllieMae.Encompass.Forms.CheckBox;
      this.cboLoanType = this.currentForm.FindControl("DropdownBox6") as DropdownBox;
      this.cboOcupancyType = this.currentForm.FindControl("DropdownBox4") as DropdownBox;
      this.Label11 = this.currentForm.FindControl("Label11") as EllieMae.Encompass.Forms.Label;
      this.DropdownBox7 = this.currentForm.FindControl("DropdownBox7") as DropdownBox;
      this.Label13 = this.currentForm.FindControl("Label13") as EllieMae.Encompass.Forms.Label;
      this.TextBox3 = this.currentForm.FindControl("TextBox3") as EllieMae.Encompass.Forms.TextBox;
      this.Label67 = this.currentForm.FindControl("Label67") as EllieMae.Encompass.Forms.Label;
      this.Label68 = this.currentForm.FindControl("Label68") as EllieMae.Encompass.Forms.Label;
      this.TextBox67 = this.currentForm.FindControl("TextBox67") as EllieMae.Encompass.Forms.TextBox;
      this.Label69 = this.currentForm.FindControl("Label69") as EllieMae.Encompass.Forms.Label;
      this.TextBox68 = this.currentForm.FindControl("TextBox68") as EllieMae.Encompass.Forms.TextBox;
      this.Label70 = this.currentForm.FindControl("Label70") as EllieMae.Encompass.Forms.Label;
      this.TextBox69 = this.currentForm.FindControl("TextBox69") as EllieMae.Encompass.Forms.TextBox;
      this.ZipCodeLookup1 = this.currentForm.FindControl("ZipCodeLookup1") as ZipCodeLookup;
      this.Label71 = this.currentForm.FindControl("Label71") as EllieMae.Encompass.Forms.Label;
      this.TextBox70 = this.currentForm.FindControl("TextBox70") as EllieMae.Encompass.Forms.TextBox;
      this.tbField11 = this.currentForm.FindControl("TbField11") as EllieMae.Encompass.Forms.TextBox;
      this.tbURLAX73 = this.currentForm.FindControl("tbURLAX73") as EllieMae.Encompass.Forms.TextBox;
      this.tbURLAX73 = this.currentForm.FindControl("tbURLAX73") as EllieMae.Encompass.Forms.TextBox;
      this.tbURLAX73 = this.currentForm.FindControl("tbURLAX73") as EllieMae.Encompass.Forms.TextBox;
      this.pnlURLA2020 = this.currentForm.FindControl("pnlURLA2020") as EllieMae.Encompass.Forms.Panel;
      this.pnlNonURLA2020 = this.currentForm.FindControl("pnlNonURLA2020") as EllieMae.Encompass.Forms.Panel;
      base.CreateControls();
      if (this.useURLA2020)
      {
        this.cboLoanType.Change += new EventHandler(this.cBox_SelectedIndexChanged);
        this.cboOcupancyType.Change += new EventHandler(this.cBox_SelectedIndexChanged);
        this.pnlURLA2020.Visible = true;
        this.pnlNonURLA2020.Visible = false;
        this.AdjustFHASecondary();
      }
      else
      {
        this.pnlURLA2020.Visible = false;
        this.pnlNonURLA2020.Visible = true;
        this.lblFHASecResidence.Visible = false;
        this.chkSecResidence.Visible = false;
      }
    }

    private void AdjustFHASecondary()
    {
      this.lblFHASecResidence.Enabled = false;
      this.chkSecResidence.Enabled = false;
      this.isFHA = this.inputData.GetField("1172") == "FHA";
      this.isSecondHome = this.inputData.GetField("1811") == "SecondHome";
      if (this.isFHA)
      {
        this.lblFHASecResidence.Enabled = this.isSecondHome;
        this.chkSecResidence.Enabled = this.isSecondHome;
      }
      else
      {
        this.lblFHASecResidence.Visible = false;
        this.chkSecResidence.Visible = false;
      }
      if (!this.chkSecResidence.Checked || this.isFHA && this.isSecondHome)
        return;
      this.chkSecResidence.Checked = false;
    }

    private void cBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (!this.useURLA2020)
        return;
      this.AdjustFHASecondary();
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      ControlState controlState = this.getControlState(ctrl, id, ControlState.Enabled);
      switch (id)
      {
        case "994":
          controlState = !(this.loan.GetField("608") != "OtherAmortizationType") ? ControlState.Enabled : ControlState.Disabled;
          goto case "11";
        case "995":
          controlState = !(this.loan.GetField("608") != "AdjustableRate") || !(this.inputData.GetField("1172") != "HELOC") ? ControlState.Enabled : ControlState.Disabled;
          goto case "11";
        case "1266":
        case "1267":
          controlState = !(this.loan.GetField("608") != "GraduatedPaymentMortgage") ? ControlState.Enabled : ControlState.Disabled;
          goto case "11";
        case "11":
          return controlState;
        default:
          controlState = ControlState.Default;
          goto case "11";
      }
    }

    public override void ExecAction(string action) => base.ExecAction(action);
  }
}
