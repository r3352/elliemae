// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.HUD1PG2_2010InputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
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
  public class HUD1PG2_2010InputHandler : InputHandlerBase
  {
    private bool canEditPayment = true;
    private POCInputHandler pocInputHandler;
    private LOCompensationInputHandler loCompensationInputHandler;
    private EllieMae.Encompass.Forms.Panel pnlForm;
    private EllieMae.Encompass.Forms.Label labelFormName;
    private EllieMae.Encompass.Forms.Label labelItemM7;
    private EllieMae.Encompass.Forms.Panel pnlItemM7;
    private EllieMae.Encompass.Forms.Panel pnlISectionMVersion1;
    private EllieMae.Encompass.Forms.Panel pnlISectionMVersion2;
    private EllieMae.Encompass.Forms.Panel pnlISectionN;
    private EllieMae.Encompass.Forms.Label labelSectionLversion1;
    private EllieMae.Encompass.Forms.Label labelSectionLversion2;
    private EllieMae.Encompass.Forms.Label labelM11;
    private EllieMae.Encompass.Forms.GroupBox grpDetailofTransaction;
    private EllieMae.Encompass.Forms.Panel pnlItemINoHeloc;
    private EllieMae.Encompass.Forms.Panel pnlItemIHeloc;
    private EllieMae.Encompass.Forms.Panel pnlBorPaidFees;
    private EllieMae.Encompass.Forms.Panel pnlBorPaidFeesNew;
    private EllieMae.Encompass.Forms.CheckBox itemizedCheckBox;
    private bool firstLoading = true;

    public HUD1PG2_2010InputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public HUD1PG2_2010InputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
      this.canEditPayment = this.HasExclusiveRights(false);
    }

    public HUD1PG2_2010InputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public HUD1PG2_2010InputHandler(
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
        this.pocInputHandler = new POCInputHandler((IHtmlInput) this.loan, this.currentForm, (InputHandlerBase) this, this.session);
        this.loCompensationInputHandler = new LOCompensationInputHandler(this.loCompensationSetting, (IHtmlInput) this.loan, this.currentForm, (InputHandlerBase) this);
        this.pnlItemINoHeloc = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlItemINoHeloc");
        this.pnlItemIHeloc = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlItemIHeloc");
        this.pnlItemIHeloc.Left = 1;
        this.pnlItemIHeloc.Top = this.pnlItemINoHeloc.Top;
        this.pnlForm = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlForm");
        this.labelFormName = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("Label191");
        this.grpDetailofTransaction = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl("grpDetailofTransaction");
        this.pnlItemM7 = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlItemM7");
        this.labelItemM7 = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("Label_M7");
        this.labelM11 = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("Label315");
        this.pnlISectionMVersion1 = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("MSectionVersion1");
        this.pnlISectionMVersion2 = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("MSectionVersion2");
        this.pnlISectionN = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("SectionN");
        this.labelSectionLversion1 = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("SectionLRespaVersion1");
        this.labelSectionLversion2 = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("SectionLRespaVersion2");
        if (this.pnlISectionMVersion1 != null && this.pnlISectionMVersion2 != null)
          this.pnlISectionMVersion2.Position = this.pnlISectionMVersion1.Position;
        if (this.labelSectionLversion1 != null && this.labelSectionLversion2 != null)
          this.labelSectionLversion2.Position = this.labelSectionLversion1.Position;
        this.pnlBorPaidFees = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("BorPaidFeesPanel");
        this.pnlBorPaidFeesNew = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("BorPaidFeesNewPanel");
        this.itemizedCheckBox = (EllieMae.Encompass.Forms.CheckBox) this.currentForm.FindControl("CheckBox301");
        if (this.session.EncompassEdition == EncompassEdition.Banker)
          return;
        EllieMae.Encompass.Forms.CheckBox control = (EllieMae.Encompass.Forms.CheckBox) this.currentForm.FindControl("chkUseLOTool");
        if (control == null)
          return;
        control.Visible = false;
      }
      catch (Exception ex)
      {
      }
    }

    protected override void UpdateContents(
      bool refreshAllFields,
      bool fireChangeEvents,
      bool skipButtonFieldLockRules)
    {
      this.firstLoading = false;
      base.UpdateContents(refreshAllFields, fireChangeEvents, skipButtonFieldLockRules);
      this.updateDetailOfTransctionLayout();
    }

    private void updateDetailOfTransctionLayout()
    {
      if (this.inputData.GetField("1172") == "HELOC")
      {
        this.pnlItemIHeloc.Visible = true;
        this.pnlItemINoHeloc.Visible = false;
      }
      else
      {
        this.pnlItemIHeloc.Visible = false;
        this.pnlItemINoHeloc.Visible = true;
      }
      if (this.inputData.GetField("4796") == "Y")
      {
        if (!this.firstLoading)
        {
          this.pnlISectionMVersion2.Visible = true;
          this.pnlISectionMVersion1.Visible = false;
        }
        this.pnlISectionN.Position = new Point(0, 816);
        this.enableOrDisableDropdownPanels(this.pnlBorPaidFeesNew, this.pnlBorPaidFees);
        this.labelSectionLversion2.Visible = true;
        this.labelSectionLversion1.Visible = false;
        this.labelFormName.Position = new Point(3, 5545);
        this.pnlForm.Size = new Size(710, 5539);
      }
      else
      {
        if (!this.firstLoading)
        {
          this.pnlISectionMVersion1.Visible = true;
          this.pnlISectionMVersion2.Visible = false;
        }
        this.pnlISectionN.Position = new Point(0, 672);
        this.enableOrDisableDropdownPanels(this.pnlBorPaidFees, this.pnlBorPaidFeesNew);
        this.labelSectionLversion2.Visible = false;
        this.labelSectionLversion1.Visible = true;
        this.labelFormName.Position = new Point(3, 5415);
        this.pnlForm.Size = new Size(710, 5409);
      }
      if (!(this.inputData.GetField("1825") == "2020"))
        this.itemizedCheckBox.Visible = false;
      else
        this.itemizedCheckBox.Visible = true;
      EllieMae.Encompass.Forms.GroupBox detailofTransaction = this.grpDetailofTransaction;
      Size size1 = this.grpDetailofTransaction.Size;
      int width = size1.Width;
      int top = this.pnlISectionN.Top;
      size1 = this.pnlISectionN.Size;
      int height1 = size1.Height;
      int height2 = top + height1;
      Size size2 = new Size(width, height2);
      detailofTransaction.Size = size2;
    }

    public override void RefreshContents(bool skipButtonFieldLockRules)
    {
      base.RefreshContents(skipButtonFieldLockRules);
      this.pocInputHandler.SetFieldLock819Status();
      if (this.loCompensationSetting == null)
        return;
      LOCompensationInputHandler.CheckLOCompRuleConfliction(this.loCompensationSetting, this.inputData, (string) null, (string) null, (string) null, false);
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      ControlState controlState = this.getControlState(ctrl, id, ControlState.Enabled);
      switch (id)
      {
        case "1061":
        case "436":
          if (this.GetFieldValue("NEWHUD.X1139") == "Y" || this.GetFieldValue("NEWHUD.X715") != "Include Origination Points" || Utils.ParseDouble((object) this.GetFieldValue("3119")) > 0.0)
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "1063":
          return controlState;
        case "1134":
          if (this.GetField("19") == "Purchase")
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "1401":
        case "ccprog":
        case "loanprog":
          if (this.loan == null || this.FormIsForTemplate)
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "142":
          EllieMae.Encompass.Forms.Label control1 = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("lblCashFromTo");
          if (control1 != null)
            control1.Text = this.ToDouble(this.loan.GetSimpleField("142")) <= 0.0 ? "Cash to borrower" : "Cash from borrower";
          controlState = ControlState.Default;
          goto case "1063";
        case "1619":
          if (this.loan != null && this.loan.GetField("1172") == "VA" && this.loan.GetField("958") == "IRRRL" && this.loan.GetField("19").IndexOf("Refinance") > -1)
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "1663":
          controlState = ControlState.Disabled;
          if (!this.IsUsingTemplate)
          {
            this.FormatAlphaNumericField(ctrl, id);
            goto case "1063";
          }
          else
            goto case "1063";
        case "1847":
        case "NEWHUD.X734":
          if (this.GetFieldValue("NEWHUD.X715") != "Include Origination Credit" || this.GetFieldValue("NEWHUD.X1139") == "Y")
            controlState = ControlState.Disabled;
          this.FormatAlphaNumericField(ctrl, id);
          goto case "1063";
        case "1851":
          EllieMae.Encompass.Forms.Label control2 = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("lblCCFrom");
          if (control2 != null)
            control2.Text = !(this.inputData.GetField("420") == "SecondLien") ? "CC from 2nd" : "CC from 1st";
          controlState = ControlState.Default;
          goto case "1063";
        case "230":
        case "232":
        case "NEWHUD.X39":
        case "NEWHUD.X572":
          this.FormatAlphaNumericField(ctrl, id);
          controlState = ControlState.Default;
          goto case "1063";
        case "9":
          if (this.GetFieldValue("19") != "Other")
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "NEWHUD.X1067":
        case "NEWHUD.X353":
        case "NEWHUD.X627":
        case "NEWHUD.X749":
        case "POPT.X12":
          if (this.GetFieldValue("NEWHUD.X1139") == "Y")
          {
            ctrl.Visible = false;
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
          {
            ctrl.Visible = true;
            if ((id == "NEWHUD.X353" || id == "NEWHUD.X749" || id == "POPT.X12") && this.GetFieldValue("NEWHUD.X715") == "Include Origination Credit")
            {
              controlState = ControlState.Disabled;
              goto case "1063";
            }
            else
              goto case "1063";
          }
        case "NEWHUD.X1139":
        case "NEWHUD.X223":
        case "NEWHUD.X224":
          if (this.GetFieldValue("NEWHUD.X1718") == "Y")
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "NEWHUD.X1140":
        case "locompensationtool":
          if (this.GetFieldValue("NEWHUD.X1139") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "NEWHUD.X1299":
        case "NEWHUD.X1301":
          if (this.pocInputHandler != null)
          {
            this.pocInputHandler.SetLine819Status();
            controlState = this.pocInputHandler.GetLine819Status(id);
            goto case "1063";
          }
          else if (this.GetFieldValue("1172") == "FarmersHomeAdministration")
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "NEWHUD.X715":
          if (this.loan != null && (Utils.ParseDouble((object) this.GetFieldValue("3119")) > 0.0 || this.GetFieldValue("NEWHUD.X1139") == "Y"))
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "NEWHUD.X788":
          if (this.GetFieldValue("NEWHUD.X715") != "Include Origination Points" || this.GetFieldValue("NEWHUD.X1139") == "Y")
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "NEWHUD.X820":
          ctrl.Visible = this.GetFieldValue("NEWHUD.X1139") != "Y";
          if (this.GetFieldValue("NEWHUD.X715") == "Include Origination Credit" || this.GetFieldValue("NEWHUD.X1139") == "Y")
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "hudsetup":
          if (this.loan == null || this.loan != null && !this.canEditPayment)
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

    public override void onblur(mshtml.IHTMLEventObj pEvtObj)
    {
      if (this.loan != null)
        this.pocInputHandler.TurnOffSellerPaidWarning();
      base.onblur(pEvtObj);
    }

    public override bool onkeypress(mshtml.IHTMLEventObj pEvtObj)
    {
      if (this.loan != null)
        this.pocInputHandler.TurnOnSellerPaidWarning(pEvtObj);
      return base.onkeypress(pEvtObj);
    }

    public override void onkeyup(mshtml.IHTMLEventObj pEvtObj)
    {
      if (this.currentForm.FindControlForElement(pEvtObj.srcElement) is FieldControl controlForElement && (controlForElement.Field.FieldID == "NEWHUD.X572" || controlForElement.Field.FieldID == "NEWHUD.X39"))
      {
        bool needsUpdate = false;
        if (controlForElement.Value.ToLower() != "n" && controlForElement.Value.ToLower() != "na" || this.GetFieldValue("420") != "SecondLien" && this.GetFieldValue("19").IndexOf("Refinance") == -1)
        {
          string str = Utils.FormatInput(controlForElement.Value, FieldFormat.DECIMAL_2, ref needsUpdate);
          if (needsUpdate)
            controlForElement.BindTo(str);
        }
      }
      base.onkeyup(pEvtObj);
    }

    internal override void FlipLockField(FieldLock fieldLock)
    {
      base.FlipLockField(fieldLock);
      if (this.loan == null)
        return;
      FieldControl controlToLock = fieldLock.ControlToLock as FieldControl;
      this.loan.Calculator.FormCalculation("REGZGFE_2010", (string) null, (string) null);
    }

    public override void onmouseenter(mshtml.IHTMLEventObj pEvtObj)
    {
      if (this.inputData != null && pEvtObj.srcElement.tagName == "IMG")
        this.loCompensationInputHandler.ShowToolTips(pEvtObj);
      else if (this.loan != null)
        this.pocInputHandler.TurnOnPOCToolTip(pEvtObj);
      base.onmouseenter(pEvtObj);
    }

    public override void onmouseleave(mshtml.IHTMLEventObj pEvtObj)
    {
      if (this.inputData != null && pEvtObj.srcElement.tagName == "IMG")
        this.loCompensationInputHandler.HideToolTips(pEvtObj);
      else if (this.loan != null)
        this.pocInputHandler.TurnOffPOCToolTip();
      base.onmouseleave(pEvtObj);
    }

    internal override void UpdateFieldValue(string id, string val)
    {
      string fieldValue = this.GetFieldValue(id);
      switch (id)
      {
        case "1847":
          double num1 = Utils.ParseDouble((object) val);
          if (num1 != 0.0)
          {
            val = num1.ToString("N3");
            break;
          }
          break;
        case "NEWHUD.X572":
        case "NEWHUD.X39":
        case "1663":
        case "232":
        case "230":
          double num2 = Utils.ParseDouble((object) val);
          if (num2 != 0.0 || val == "0" || val == "." || val.IndexOf("0.") > -1 || val.IndexOf(".0") > -1)
          {
            val = num2.ToString("N2");
            break;
          }
          break;
      }
      base.UpdateFieldValue(id, val);
      if (this.loCompensationSetting != null)
        LOCompensationInputHandler.CheckLOCompRuleConfliction(this.loCompensationSetting, this.inputData, id, val, fieldValue, true);
      if (this.loan != null)
      {
        if (id == "154" || id == "1627" || id == "1838" || id == "1841" || id == "NEWHUD.X732")
          this.loan.Calculator.CopyHUD2010ToGFE2010(id, false);
        this.loan.Calculator.FormCalculation("UpdateCityStateUserFees", id, val);
        this.loan.Calculator.FormCalculation("HUD1PG2_2010", id, val);
        if (this.loan.GetField("14").ToUpper() == "CA")
          this.loan.Calculator.CopyGFEToMLDS(id);
      }
      if (this.loCompensationInputHandler == null)
        return;
      this.loCompensationInputHandler.RefreshContents();
    }

    public override void RefreshContents()
    {
      base.RefreshContents();
      if (this.loCompensationInputHandler == null)
        return;
      this.loCompensationInputHandler.RefreshContents();
    }

    public override void ExecAction(string action)
    {
      string val1 = string.Empty;
      string val2 = string.Empty;
      if (action == "mtginsprem" || action == "mtginsreserv")
      {
        val1 = this.GetFieldValue("NEWHUD.X622");
        val2 = this.GetFieldValue("NEWHUD.X693");
      }
      base.ExecAction(action);
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(action))
      {
        case 287038707:
          if (!(action == "hudsetup"))
            return;
          this.SetFieldFocus("l_254");
          return;
        case 511528202:
          if (!(action == "cityfee"))
            return;
          this.SetFieldFocus("l_1637");
          return;
        case 1084911575:
          if (!(action == "recordingfee2010"))
            return;
          if (this.loan != null)
            this.loan.Calculator.FormCalculation("HUD1PG2_2010", (string) null, (string) null);
          this.SetFieldFocus("l_X604");
          return;
        case 1132459806:
          if (!(action == "statefee"))
            return;
          this.SetFieldFocus("l_1638");
          return;
        case 1181978758:
          if (!(action == "ownercoverage"))
            return;
          this.SetFieldFocus("l_1633");
          return;
        case 1231307916:
          if (!(action == "statetax2010"))
            return;
          if (this.loan != null)
            this.loan.Calculator.FormCalculation("HUD1PG2_2010", (string) null, (string) null);
          this.SetFieldFocus("l_X606");
          return;
        case 1370251432:
          if (!(action == "ownerins"))
            return;
          this.SetFieldFocus("l_578");
          return;
        case 1473801479:
          if (!(action == "statetax"))
            return;
          if (this.loan != null)
            this.loan.Calculator.FormCalculation("HUD1PG2_2010", (string) null, (string) null);
          this.SetFieldFocus("l_1638");
          return;
        case 1628156329:
          if (!(action == "lendercoverage"))
            return;
          this.SetFieldFocus("l_652");
          return;
        case 2536373229:
          if (!(action == "copyhudgfe800"))
            return;
          this.SetFieldFocus("l_X135");
          return;
        case 2717847646:
          if (!(action == "localtax2010"))
            return;
          if (this.loan != null)
            this.loan.Calculator.FormCalculation("HUD1PG2_2010", (string) null, (string) null);
          this.SetFieldFocus("l_X605");
          return;
        case 3050596908:
          if (!(action == "recordingfee"))
            return;
          if (this.loan != null)
            this.loan.Calculator.FormCalculation("HUD1PG2_2010", (string) null, (string) null);
          this.SetFieldFocus("l_1636");
          return;
        case 3100944149:
          if (!(action == "localtax"))
            return;
          if (this.loan != null)
            this.loan.Calculator.FormCalculation("HUD1PG2_2010", (string) null, (string) null);
          this.SetFieldFocus("l_1637");
          return;
        case 3302304154:
          if (!(action == "mtginsreserv"))
            return;
          this.UpdateFieldValue("NEWHUD.X622", val1);
          this.UpdateFieldValue("NEWHUD.X693", val2);
          if (this.GetField("1172") == "FarmersHomeAdministration")
          {
            this.SetFieldFocus("l_X1707");
            return;
          }
          if (this.inputData.IsLocked("232"))
          {
            this.SetFieldFocus("l_232");
            return;
          }
          this.SetFieldFocus("l_1386");
          return;
        case 3582764703:
          if (!(action == "loanprog"))
            return;
          break;
        case 3709982155:
          if (!(action == "mtginsprem"))
            return;
          this.UpdateFieldValue("NEWHUD.X622", val1);
          this.UpdateFieldValue("NEWHUD.X693", val2);
          this.SetFieldFocus("l_562");
          return;
        case 3949279623:
          if (!(action == "taxesreserv"))
            return;
          this.SetFieldFocus("l_231");
          return;
        case 4112030216:
          if (!(action == "edithudgfe802"))
            return;
          this.SetFieldFocus("l_617");
          return;
        case 4113929720:
          if (!(action == "userfee2"))
            return;
          this.SetFieldFocus("l_1640");
          return;
        case 4130707339:
          if (!(action == "userfee3"))
            return;
          this.SetFieldFocus("l_1643");
          return;
        case 4155076599:
          if (!(action == "ccprog"))
            return;
          break;
        case 4162363073:
          if (!(action == "edithudgfe801"))
            return;
          this.SetFieldFocus("l_L228");
          return;
        case 4164262577:
          if (!(action == "userfee1"))
            return;
          this.SetFieldFocus("l_373");
          return;
        default:
          return;
      }
      this.SetFieldFocus("l_364");
    }
  }
}
