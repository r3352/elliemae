// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.REGZCDInputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.Forms;
using mshtml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class REGZCDInputHandler : REGZ50InputHandler
  {
    private EllieMae.Encompass.Forms.Label labelCurrentAPR;
    private ImageButton imgAPRAlert;
    private EllieMae.Encompass.Forms.GroupBox boxAlertForFix;
    private EllieMae.Encompass.Forms.GroupBox boxAlertForNonFix;
    private ImageButton imgAPRAlertOver;
    private ImageButton imgAPRAlertNotOver;
    private DropdownBox mersOrgId;
    private FieldLock fl_39;
    private Color colorCurrentAPR;
    private AIRInputHandler airInputHandler;
    private APInputHandler apInputHandler;
    private DropdownBox ddAssumption;
    private LoanTermTableInputHandler loanTermTableInputHandler;
    private BranchMERSMINNumberingInfo[] orgIdInfo;
    private List<DropdownOption> optionsOrgId = new List<DropdownOption>();

    public REGZCDInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public REGZCDInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public REGZCDInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public REGZCDInputHandler(
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
        if (this.labelCurrentAPR == null)
        {
          this.labelCurrentAPR = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("LabelCurrentAPR");
          this.colorCurrentAPR = this.labelCurrentAPR.ForeColor;
        }
        if (this.imgAPRAlert == null)
          this.imgAPRAlert = (ImageButton) this.currentForm.FindControl("aprAlert");
        if (this.imgAPRAlertOver == null)
          this.imgAPRAlertOver = (ImageButton) this.currentForm.FindControl("aprAlertOver");
        if (this.imgAPRAlertNotOver == null)
          this.imgAPRAlertNotOver = (ImageButton) this.currentForm.FindControl("aprAlertNotOver");
        if (this.boxAlertForFix == null)
        {
          this.boxAlertForFix = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl("boxAlertForFix");
          this.boxAlertForNonFix = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl("boxAlertForNonFix");
        }
        if (this.fl_39 == null)
          this.fl_39 = (FieldLock) this.currentForm.FindControl("FieldLock39");
        if (this.mersOrgId == null)
          this.mersOrgId = (DropdownBox) this.currentForm.FindControl("DropdownBox18");
        this.createMersOrgIdOptions(this.mersOrgId);
      }
      catch (Exception ex)
      {
      }
    }

    internal override void UpdateFieldValue(string id, string val)
    {
      base.UpdateFieldValue(id, val);
      if (id == "423")
        this.apInputHandler.ChangeMonthlyBiweeklyLabel();
      if (id == "L770")
        this.inputData.SetCurrentField("3925", val);
      if (this.loanTermTableInputHandler == null)
        return;
      this.loanTermTableInputHandler.SetLayout();
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      ControlState currentState = base.GetControlState(ctrl, id);
      switch (id)
      {
        case "799":
          this.refreshAPRAlert();
          currentState = ControlState.Default;
          break;
        case "CD4.X31":
          currentState = !(this.inputData.GetField("608") != "AdjustableRate") ? ControlState.Enabled : ControlState.Disabled;
          break;
        case "4084":
          currentState = !(this.inputData.GetField("19") == "ConstructionOnly") ? ControlState.Disabled : ControlState.Enabled;
          break;
        case "CONST.X1":
          currentState = !(this.inputData.GetField("19") != "ConstructionToPermanent") ? ControlState.Enabled : ControlState.Disabled;
          break;
        case "4722":
          currentState = !this.inputData.IsLocked("4722") ? ControlState.Disabled : ControlState.Enabled;
          break;
        case "4723":
          currentState = !string.IsNullOrEmpty(this.inputData.GetField("1051")) ? ControlState.Enabled : ControlState.Disabled;
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

    private void refreshAPRAlert()
    {
      double num1 = Utils.ParseDouble((object) this.loan.GetField("3121"));
      double num2 = Utils.ParseDouble((object) this.loan.GetField("799"));
      bool flag1 = true;
      bool flag2;
      if (num1 == 0.0)
      {
        flag2 = flag1;
      }
      else
      {
        double num3 = 0.125;
        bool complianceSetting1 = (bool) this.loan.Settings.ComplianceSettings[(object) "Compliance.ApplyFixedAPRToleranceToARMs2015"];
        string field = this.loan.GetField("19");
        if (!complianceSetting1 && (this.loan.GetField("608") == "AdjustableRate" || field == "ConstructionOnly" || field == "ConstructionToPermanent"))
          num3 = 0.25;
        bool complianceSetting2 = (bool) this.loan.Settings.ComplianceSettings[(object) "Compliance.SuppressNegativeAPRAlert2015"];
        double num4 = Utils.ArithmeticRounding(num2 - num1, 3);
        flag2 = !complianceSetting2 || num4 > num3 ? (complianceSetting2 || Math.Abs(num4) > num3 ? !Utils.IsDate((object) this.loan.GetField("CD1.X47")) && flag1 : flag1) : flag1;
      }
      if (!flag2)
      {
        if (this.labelCurrentAPR != null)
          this.labelCurrentAPR.ForeColor = AppColors.AlertRed;
        if (this.imgAPRAlert == null)
          return;
        this.imgAPRAlert.Visible = true;
      }
      else
      {
        if (this.labelCurrentAPR != null)
          this.labelCurrentAPR.ForeColor = this.colorCurrentAPR;
        if (this.imgAPRAlert == null)
          return;
        this.imgAPRAlert.Visible = false;
      }
    }

    protected override string GetFieldValue(string id, FieldSource fieldSource)
    {
      if (id == "CD4.X25" || id == "CD4.X27" || id == "NEWHUD.X6")
        this.apInputHandler.SetSectionStatus();
      if (id == "608" || id == "19" || id == "1677" || id == "3")
        this.airInputHandler.SetSectionStatus();
      return id.StartsWith("CD1.X") ? this.loanTermTableInputHandler.GetFieldValue(id, base.GetFieldValue(id, fieldSource)) : this.airInputHandler.GetFieldValue(id, base.GetFieldValue(id, fieldSource));
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

    public override void onmouseenter(mshtml.IHTMLEventObj pEvtObj)
    {
      if (pEvtObj.srcElement.id == "aprAlert")
      {
        this.imgAPRAlert.Source = this.imgAPRAlertOver.Source;
        if (this.labelCurrentAPR != null && this.labelCurrentAPR.ForeColor == AppColors.AlertRed)
        {
          if (this.GetFieldValue("608") == "Fixed")
            this.boxAlertForFix.Visible = true;
          else
            this.boxAlertForNonFix.Visible = true;
        }
      }
      base.onmouseenter(pEvtObj);
    }

    public override void onmouseleave(mshtml.IHTMLEventObj pEvtObj)
    {
      if (!(pEvtObj.srcElement.id == "aprAlert"))
        return;
      this.imgAPRAlert.Source = this.imgAPRAlertNotOver.Source;
      if (this.boxAlertForFix == null)
        return;
      this.boxAlertForFix.Visible = false;
      this.boxAlertForNonFix.Visible = false;
    }

    private void createMersOrgIdOptions(DropdownBox mersOrgId)
    {
      this.orgIdInfo = Session.ConfigurationManager.GetAllBranchMERSNumberingInfo(true);
      if (this.orgIdInfo == null)
        return;
      this.optionsOrgId.Clear();
      mersOrgId.Options.Clear();
      foreach (BranchMERSMINNumberingInfo mersminNumberingInfo in this.orgIdInfo)
      {
        if (mersminNumberingInfo.Enabled)
          this.optionsOrgId.Add(new DropdownOption(mersminNumberingInfo.MERSMINCode, mersminNumberingInfo.MERSMINCode));
      }
      mersOrgId.Options.AddRange((ICollection) this.optionsOrgId);
      MersNumberingInfo mersNumberingInfo = Session.ConfigurationManager.GetMersNumberingInfo();
      if (mersNumberingInfo == null)
        return;
      string companyId = mersNumberingInfo.CompanyID;
      if (mersOrgId.Options.Contains(new DropdownOption(companyId, companyId)))
        return;
      mersOrgId.Options.Add(new DropdownOption(companyId, companyId));
    }
  }
}
