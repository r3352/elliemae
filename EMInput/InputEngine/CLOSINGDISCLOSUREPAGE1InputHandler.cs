// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.CLOSINGDISCLOSUREPAGE1InputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.Forms;
using mshtml;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class CLOSINGDISCLOSUREPAGE1InputHandler : InputHandlerBase
  {
    private List<EllieMae.Encompass.Forms.Panel> panelLocalAddresses;
    private List<EllieMae.Encompass.Forms.Panel> panelForeignAddresses;
    private List<StandardButton> selectCountryButtons;
    private List<EllieMae.Encompass.Forms.Panel> panelForeignPanels;
    private LoanTermTableInputHandler loanTermTableInputHandler;
    private EllieMae.Encompass.Forms.TextBox otherLoanType;
    private EllieMae.Encompass.Forms.TextBox txtAlternateVersion;
    private EllieMae.Encompass.Forms.TextBox prepaymentPenaltyDuringYearsTextbox;
    private EllieMae.Encompass.Forms.TextBox txtChangesReceivedDate;
    private Calendar calChangesReceivedDate;
    private MultilineTextBox txtChangedCircumstance;
    private MultilineTextBox txtComments;
    private StandardButton sbChangedCircumstance;
    private MultilineTextBox toleranceCureTxtComments;
    private EllieMae.Encompass.Forms.TextBox toleranceCureReqCureAmt;
    private FieldLock toleranceCureReqCureAmtLock;
    private EllieMae.Encompass.Forms.TextBox toleranceCureAppCureAmt;
    private EllieMae.Encompass.Forms.TextBox toleranceCureDate;
    private Calendar toleranceCureCalendar;
    private EllieMae.Encompass.Forms.TextBox toleranceCureResolvedBy;
    private EllieMae.Encompass.Forms.CheckBox chkFeeLevelDisclosures;
    private EllieMae.Encompass.Forms.Label lblReason;
    private EllieMae.Encompass.Forms.CheckBox chkChangeInApr;
    private EllieMae.Encompass.Forms.CheckBox chkChangeInLoanProd;
    private EllieMae.Encompass.Forms.CheckBox chkPrepaymentPenaltyAdded;
    private EllieMae.Encompass.Forms.CheckBox chkCocSettlementCharge;
    private EllieMae.Encompass.Forms.CheckBox chkCocEligibility;
    private EllieMae.Encompass.Forms.CheckBox chkRevisionReqByCust;
    private EllieMae.Encompass.Forms.CheckBox chkRateLock;
    private EllieMae.Encompass.Forms.CheckBox chk24hrAdvPreview;
    private EllieMae.Encompass.Forms.CheckBox chkToleranceCure;
    private EllieMae.Encompass.Forms.CheckBox chkCriticalErrorCorrection;
    private EllieMae.Encompass.Forms.CheckBox chkOtherReason;
    private EllieMae.Encompass.Forms.TextBox txtOtherReason;
    private EllieMae.Encompass.Forms.Label lblChangesRcvdDate;
    private EllieMae.Encompass.Forms.Label lblRevisedCdDueDate;
    private EllieMae.Encompass.Forms.TextBox txtRevisedCdDueDate;
    private EllieMae.Encompass.Forms.Label lblChangedCircumstances;
    private EllieMae.Encompass.Forms.Label lblComments;
    private EllieMae.Encompass.Forms.Panel pnlCoc;
    private FieldReformatOnUIHandler fieldReformatOnUIHandler;

    public CLOSINGDISCLOSUREPAGE1InputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public CLOSINGDISCLOSUREPAGE1InputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public CLOSINGDISCLOSUREPAGE1InputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public CLOSINGDISCLOSUREPAGE1InputHandler(
      Sessions.Session session,
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, dataTemplate, htmldoc, form, property)
    {
    }

    public CLOSINGDISCLOSUREPAGE1InputHandler(
      Sessions.Session session,
      IWin32Window owner,
      IHtmlInput htmlInput,
      HTMLDocument htmlDoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, htmlInput, htmlDoc, form, property)
    {
    }

    internal override void CreateControls()
    {
      try
      {
        this.fieldReformatOnUIHandler = new FieldReformatOnUIHandler(this.inputData);
        this.otherLoanType = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("l_1063");
        this.loanTermTableInputHandler = new LoanTermTableInputHandler(this.currentForm, this.inputData, this.ToString(), this.session);
        this.txtAlternateVersion = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("txtAlternateVersion");
        this.txtChangesReceivedDate = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("I_CD1X62");
        this.calChangesReceivedDate = (Calendar) this.currentForm.FindControl("calCD1X62");
        this.txtChangedCircumstance = (MultilineTextBox) this.currentForm.FindControl("I_CD1X64");
        this.txtComments = (MultilineTextBox) this.currentForm.FindControl("I_CD1X65");
        this.sbChangedCircumstance = (StandardButton) this.currentForm.FindControl("stdbtn_CD1X64");
        this.prepaymentPenaltyDuringYearsTextbox = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("l_X27");
        this.txtChangesReceivedDate.Enabled = this.calChangesReceivedDate.Enabled = this.txtComments.Enabled = this.sbChangedCircumstance.Enabled = this.inputData.GetField("CD1.X61") == "Y";
        this.toleranceCureTxtComments = (MultilineTextBox) this.currentForm.FindControl("MultiLineTextBox1");
        this.toleranceCureReqCureAmtLock = (FieldLock) this.currentForm.FindControl("ToleranceCureLock");
        this.toleranceCureReqCureAmt = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("edit4");
        this.toleranceCureAppCureAmt = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("edit1");
        this.toleranceCureCalendar = (Calendar) this.currentForm.FindControl("Calendar4");
        this.toleranceCureDate = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("edit2");
        this.toleranceCureResolvedBy = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("textBox343");
        this.chkFeeLevelDisclosures = (EllieMae.Encompass.Forms.CheckBox) this.currentForm.FindControl("chk_4461");
        this.lblReason = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("Label59");
        this.chkChangeInApr = (EllieMae.Encompass.Forms.CheckBox) this.currentForm.FindControl("chk_CD1X52");
        this.chkChangeInLoanProd = (EllieMae.Encompass.Forms.CheckBox) this.currentForm.FindControl("chk_CD1X53");
        this.chkPrepaymentPenaltyAdded = (EllieMae.Encompass.Forms.CheckBox) this.currentForm.FindControl("chk_CD1X54");
        this.chkCocSettlementCharge = (EllieMae.Encompass.Forms.CheckBox) this.currentForm.FindControl("chk_CD1X55");
        this.chkCocEligibility = (EllieMae.Encompass.Forms.CheckBox) this.currentForm.FindControl("chk_CD1X68");
        this.chkRevisionReqByCust = (EllieMae.Encompass.Forms.CheckBox) this.currentForm.FindControl("chk_CD1X66");
        this.chkRateLock = (EllieMae.Encompass.Forms.CheckBox) this.currentForm.FindControl("chk_CD1X67");
        this.chk24hrAdvPreview = (EllieMae.Encompass.Forms.CheckBox) this.currentForm.FindControl("chk_CD1X56");
        this.chkToleranceCure = (EllieMae.Encompass.Forms.CheckBox) this.currentForm.FindControl("chk_CD1X57");
        this.chkCriticalErrorCorrection = (EllieMae.Encompass.Forms.CheckBox) this.currentForm.FindControl("chk_CD1X58");
        this.chkOtherReason = (EllieMae.Encompass.Forms.CheckBox) this.currentForm.FindControl("chk_CD1X59");
        this.txtOtherReason = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("I_CD1X60");
        this.lblChangesRcvdDate = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("Label65");
        this.lblRevisedCdDueDate = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("Label66");
        this.txtRevisedCdDueDate = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("I_CD1X63");
        this.lblChangedCircumstances = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("Label67");
        this.lblComments = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("Label68");
        this.pnlCoc = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("Panel7");
        if (!((FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.ToolsTab_CureToleranceAlert))
        {
          this.toleranceCureTxtComments.Enabled = false;
          this.toleranceCureReqCureAmtLock.Enabled = false;
          this.toleranceCureReqCureAmt.Enabled = false;
          this.toleranceCureAppCureAmt.Enabled = false;
          this.toleranceCureCalendar.Enabled = false;
          this.toleranceCureDate.Enabled = false;
          this.toleranceCureResolvedBy.Enabled = false;
        }
        if (!this.session.StartupInfo.EnableCoC)
          this.revertToNonCocLayout();
        this.panelLocalAddresses = new List<EllieMae.Encompass.Forms.Panel>();
        this.panelForeignAddresses = new List<EllieMae.Encompass.Forms.Panel>();
        this.panelForeignPanels = new List<EllieMae.Encompass.Forms.Panel>();
        for (int index = 1; index <= 2; ++index)
        {
          this.panelLocalAddresses.Add((EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("localAdr" + (object) index));
          this.panelForeignAddresses.Add((EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("foreignAdr" + (object) index));
          if (this.panelForeignAddresses[index - 1] != null && this.panelLocalAddresses[index - 1] != null)
            this.panelForeignAddresses[index - 1].Position = this.panelLocalAddresses[index - 1].Position;
        }
        for (int index = 1; index <= 4; ++index)
          this.panelForeignPanels.Add((EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("foreignPanel" + (object) index));
        this.selectCountryButtons = new List<StandardButton>();
        this.selectCountryButtons.Add((StandardButton) this.currentForm.FindControl("selectcountry_URLA_X269"));
        this.selectCountryButtons.Add((StandardButton) this.currentForm.FindControl("selectcountry_4681"));
      }
      catch (Exception ex)
      {
      }
      if (this.loan == null || this.loan.Calculator == null)
        return;
      this.loan.Calculator.FormCalculation("638", (string) null, (string) null);
    }

    private bool isRequireCoCPriorDisclosure()
    {
      return this.session.LoanDataMgr.ConfigInfo.RequireCoCPriorDisclosure;
    }

    private ControlState GetControlStateForFeeLevelDisclosures()
    {
      return this.session.StartupInfo.EnableCoC && this.loan != null && (this.isRequireCoCPriorDisclosure() || this.loan.GetLogList().IsThereAny2015DisclosureTracking()) ? ControlState.Disabled : ControlState.Enabled;
    }

    private ControlState getControlStateForCdCocFields()
    {
      return this.session.StartupInfo.EnableCoC && !(this.inputData.GetField("4461") != "Y") ? ControlState.Disabled : ControlState.Enabled;
    }

    protected override string GetFieldValue(string id, FieldSource fieldSource)
    {
      this.loanTermTableInputHandler.SetSectionStatus(id);
      if (id == "CD3.X86")
        this.txtAlternateVersion.Visible = this.inputData.GetField("LE2.X28") == "Y";
      string fieldValue = this.loanTermTableInputHandler.GetFieldValue(id, base.GetFieldValue(id, fieldSource));
      if (id == "LE1.X27")
        this.prepaymentPenaltyDuringYearsTextbox.Alignment = string.Compare(fieldValue, "first", true) == 0 ? TextAlignment.Left : TextAlignment.Right;
      string val = this.inputData is DisclosedCDHandler ? this.loanTermTableInputHandler.GetFieldValue(id, this.inputData.GetField(id)) : this.loanTermTableInputHandler.GetFieldValue(id, base.GetFieldValue(id, fieldSource));
      return this.fieldReformatOnUIHandler.GetFieldValue(id, val);
    }

    private void revertToNonCocLayout()
    {
      int num = 28;
      this.chkFeeLevelDisclosures.Visible = false;
      this.lblReason.Top -= num;
      this.chkChangeInApr.Top -= num;
      this.chkChangeInLoanProd.Top -= num;
      this.chkPrepaymentPenaltyAdded.Top -= num;
      this.chkCocSettlementCharge.Top -= num;
      this.chkCocEligibility.Top -= num;
      this.chkRevisionReqByCust.Top -= num;
      this.chkRateLock.Top -= num;
      this.chk24hrAdvPreview.Top -= num;
      this.chkToleranceCure.Top -= num;
      this.chkCriticalErrorCorrection.Top -= num;
      this.chkOtherReason.Top -= num;
      this.txtOtherReason.Top -= num;
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      if (this.inputData is DisclosedCDHandler)
        return ControlState.Disabled;
      ControlState controlState = this.getControlState(ctrl, id, ControlState.Enabled);
      switch (id)
      {
        case "1063":
        case "11":
        case "1416":
        case "FR0104":
          return controlState;
        case "4461":
          controlState = this.GetControlStateForFeeLevelDisclosures();
          goto case "1063";
        case "4681":
          if (this.loan.GetField("4680") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "CD1.X55":
        case "CD1.X66":
        case "CD1.X67":
        case "CD1.X68":
          controlState = !this.inputData.IsLocked(id) ? this.getControlStateForCdCocFields() : ControlState.Disabled;
          goto case "1063";
        case "CD1.X60":
          controlState = !(this.inputData.GetField("CD1.X59") != "Y") ? ControlState.Enabled : ControlState.Disabled;
          goto case "1063";
        case "CD1.X62":
        case "CD1.X64":
        case "CD1.X65":
          controlState = !(this.inputData.GetField("CD1.X61") == "Y") ? ControlState.Disabled : ControlState.Enabled;
          goto case "1063";
        case "CONST.X1":
          controlState = !(this.inputData.GetField("19") != "ConstructionToPermanent") ? ControlState.Enabled : ControlState.Disabled;
          goto case "1063";
        case "URLA.X269":
          if (this.loan.GetField("URLA.X267") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        default:
          controlState = this.loanTermTableInputHandler == null ? ControlState.Default : this.loanTermTableInputHandler.GetControlState(id, ControlState.Default);
          goto case "1063";
      }
    }

    private void clearAllCdCocFields()
    {
      base.UpdateFieldValue("CD1.X52", "");
      base.UpdateFieldValue("CD1.X53", "");
      base.UpdateFieldValue("CD1.X54", "");
      base.UpdateFieldValue("CD1.X55", "");
      base.UpdateFieldValue("CD1.X56", "");
      base.UpdateFieldValue("CD1.X57", "");
      base.UpdateFieldValue("CD1.X58", "");
      base.UpdateFieldValue("CD1.X59", "");
      base.UpdateFieldValue("CD1.X60", "");
      base.UpdateFieldValue("CD1.X66", "");
      base.UpdateFieldValue("CD1.X67", "");
      base.UpdateFieldValue("CD1.X68", "");
      this.UpdateFieldValue("CD1.X61", "");
      base.UpdateFieldValue("CD1.X62", "//");
      base.UpdateFieldValue("CD1.X63", "//");
      base.UpdateFieldValue("CD1.X64", "");
      base.UpdateFieldValue("CD1.X65", "");
    }

    private void clearAllFeeCocDetails() => this.loan.RemoveAllGoodFaithChangeOfCircumstance();

    public override void RefreshContents(bool skipButtonFieldLockRules)
    {
      base.RefreshContents(skipButtonFieldLockRules);
      if (this.loanTermTableInputHandler == null)
        return;
      this.loanTermTableInputHandler.SetLayout();
    }

    internal override void UpdateFieldValue(string id, string val)
    {
      if (this.loanTermTableInputHandler != null)
        this.loanTermTableInputHandler.UpdateFieldValue(id, val);
      if (id != "4461" && this.inputData.GetField("4461") == "Y")
      {
        if (this.loan.FeeLevel_COC_CD_Warning)
        {
          int num = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "Please note: Any updates made on the Good Faith Variance Violated Alert page or 2015 itemization may overwrite your existing Changed Circumstances.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          this.loan.FeeLevel_COC_CD_Warning = false;
        }
        if (!this.loan.Locked_COC_Fields.Contains((object) id))
          this.loan.Locked_COC_Fields.Add((object) id);
      }
      if (id == "4461")
      {
        if (!string.IsNullOrEmpty(val) && val == "Y")
        {
          if (Utils.Dialog((IWin32Window) this.session.MainForm, "Any changes to Disclosure Information you have made will be cleared. Would you like to continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            return;
          this.clearAllCdCocFields();
          base.UpdateFieldValue(id, val);
        }
        else
        {
          if (Utils.Dialog((IWin32Window) this.session.MainForm, "All fee level changes entered in Alerts will be cleared. Would you like to continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            return;
          base.UpdateFieldValue(id, val);
          this.clearAllCdCocFields();
          this.clearAllFeeCocDetails();
        }
      }
      base.UpdateFieldValue(id, val);
      if (id == "1172")
        this.otherLoanType.Enabled = val == "Other" || val == "FarmersHomeAdministration";
      if (id == "CD1.X59" && val == "N")
        base.UpdateFieldValue("CD1.X60", "");
      if (id == "CD1.X61")
      {
        EllieMae.Encompass.Forms.TextBox changesReceivedDate1 = this.txtChangesReceivedDate;
        Calendar changesReceivedDate2 = this.calChangesReceivedDate;
        MultilineTextBox txtComments = this.txtComments;
        bool flag1;
        this.sbChangedCircumstance.Enabled = flag1 = this.getControlStateForCdCocFields() == ControlState.Enabled && val == "Y";
        int num1;
        bool flag2 = (num1 = flag1 ? 1 : 0) != 0;
        txtComments.Enabled = num1 != 0;
        int num2;
        bool flag3 = (num2 = flag2 ? 1 : 0) != 0;
        changesReceivedDate2.Enabled = num2 != 0;
        int num3 = flag3 ? 1 : 0;
        changesReceivedDate1.Enabled = num3 != 0;
        if (val == "Y")
        {
          this.txtChangesReceivedDate.Enabled = this.calChangesReceivedDate.Enabled = this.txtComments.Enabled = this.sbChangedCircumstance.Enabled = true;
        }
        else
        {
          base.UpdateFieldValue("CD1.X63", "//");
          base.UpdateFieldValue("CD1.X64", "");
          base.UpdateFieldValue("CD1.X70", "");
          base.UpdateFieldValue("CD1.X65", "");
        }
      }
      if (id == "3171" || id == "3172")
      {
        if ((this.GetFieldValue("3171") == string.Empty || this.GetFieldValue("3171") == "//") && this.GetFieldValue("3172") == string.Empty)
          base.UpdateFieldValue("3173", "");
        else
          base.UpdateFieldValue("3173", this.session.UserInfo.FullName + " (" + this.session.UserID + ")");
      }
      List<string> stringList1 = new List<string>()
      {
        "CD1.X52",
        "CD1.X53",
        "CD1.X54"
      };
      List<string> stringList2 = new List<string>()
      {
        "CD1.X55",
        "CD1.X66",
        "CD1.X67",
        "CD1.X68"
      };
      bool flag = false;
      if (stringList2.Contains(id) || stringList1.Contains(id))
      {
        if (val == "Y" && stringList2.Contains(id))
          flag = true;
        if (val == "Y" && stringList1.Contains(id))
          flag = false;
        foreach (string id1 in stringList2)
        {
          if (this.loan.GetField(id1) == "Y")
          {
            flag = true;
            break;
          }
        }
        foreach (string id2 in stringList1)
        {
          if (this.loan.GetField(id2) == "Y")
          {
            flag = false;
            break;
          }
        }
        if (flag)
        {
          BusinessCalendar businessCalendar = this.session.SessionObjects.GetBusinessCalendar(CalendarType.RegZ);
          IDisclosureTracking2015Log tracking2015LogByType = this.loan.GetLogList().GetIDisclosureTracking2015LogByType(DisclosureTracking2015Log.DisclosureTrackingType.CD, DisclosureTracking2015Log.DisclosureTypeEnum.Initial);
          DateTime date1 = Utils.ParseDate((object) tracking2015LogByType.GetDisclosedField("CD1.X1"));
          DateTime dateTime = date1 != DateTime.MinValue ? businessCalendar.AddBusinessDays(date1, 7, true) : DateTime.MinValue;
          DateTime date2 = Utils.ParseDate((object) tracking2015LogByType.GetDisclosedField("748"));
          if (date2 > date1 && date2 <= dateTime && Utils.ParseDate((object) this.loan.GetField("748")) <= date2)
            base.UpdateFieldValue("CD3.X129", "");
        }
        else
          base.UpdateFieldValue("CD3.X129", this.loan.GetField("FV.X366"));
      }
      if (this.loanTermTableInputHandler == null)
        return;
      this.loanTermTableInputHandler.UpdateFieldValue(id, val);
      this.loanTermTableInputHandler.SetLayout();
    }

    public override void ExecAction(string action)
    {
      base.ExecAction(action);
      if (!(action == "prepayment") && !(action == "viewworstcase"))
        return;
      this.SetFieldFocus("l_X348");
    }

    protected override void UpdateContents(
      bool refreshAllFields,
      bool fireChangeEvents,
      bool skipButtonFieldLockRules)
    {
      base.UpdateContents(refreshAllFields, fireChangeEvents, skipButtonFieldLockRules);
      if (this.selectCountryButtons != null && this.selectCountryButtons.Count > 0)
      {
        for (int index = 0; index < this.selectCountryButtons.Count; ++index)
        {
          if (this.selectCountryButtons[index] != null)
          {
            bool flag = this.GetField(index == 0 ? "URLA.X267" : "4680") == "Y";
            this.selectCountryButtons[index].Visible = flag;
            this.panelLocalAddresses[index].Visible = !flag;
            this.panelForeignAddresses[index].Visible = flag;
          }
        }
      }
      bool flag1 = false;
      if (this.loan == null && Session.LoanData != null)
        flag1 = Session.LoanData.GetField("1825") == "2020";
      if (this.GetField("1825") == "2020" | flag1)
      {
        for (int index = 0; index < 4; ++index)
          this.panelForeignPanels[index].Visible = true;
      }
      else
      {
        for (int index = 0; index < 4; ++index)
          this.panelForeignPanels[index].Visible = false;
      }
    }
  }
}
