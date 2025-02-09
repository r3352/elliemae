// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.PIGGYBACKInputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.LoanUtils.Workflow;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.Forms;
using mshtml;
using System;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class PIGGYBACKInputHandler : InputHandlerBase
  {
    private const string className = "PIGGYBACKInputHandler";
    private EllieMae.Encompass.Forms.Label labelLienPOS;
    private EllieMae.Encompass.Forms.Label labelLienPOS2;
    private EllieMae.Encompass.Forms.Button btnNewLink;
    private EllieMae.Encompass.Forms.Button btnMakeCurrent;
    private EllieMae.Encompass.Forms.Button linkloanbutton;
    private EllieMae.Encompass.Forms.Label labelCCfrom;
    private EllieMae.Encompass.Forms.Label labelCCfrom2;
    private EllieMae.Encompass.Forms.Panel pnlFInitialDraw;
    private EllieMae.Encompass.Forms.Panel pnlFDownPay;
    private EllieMae.Encompass.Forms.Panel pnlFBottom;
    private EllieMae.Encompass.Forms.Panel pnlSInitialDraw;
    private EllieMae.Encompass.Forms.Panel pnlSDownPay;
    private EllieMae.Encompass.Forms.Panel pnlSBottom;
    private EllieMae.Encompass.Forms.Panel pnlBorPaidFeesCurrent;
    private EllieMae.Encompass.Forms.Panel pnlBorPaidFeesNewCurrent;
    private EllieMae.Encompass.Forms.Panel pnlBorPaidFeesPiggy;
    private EllieMae.Encompass.Forms.Panel pnlBorPaidFeesNewPiggy;
    private bool disabledRemoveLineButton;

    public PIGGYBACKInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
      this.refreshLienInformation();
    }

    public PIGGYBACKInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
      this.refreshLienInformation();
    }

    public PIGGYBACKInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
      this.refreshLienInformation();
    }

    public PIGGYBACKInputHandler(
      Sessions.Session session,
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, dataTemplate, htmldoc, form, property)
    {
      this.refreshLienInformation();
    }

    internal override void CreateControls()
    {
      try
      {
        this.labelLienPOS = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("lienpos");
        this.labelLienPOS2 = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("lienpos2");
        this.btnNewLink = (EllieMae.Encompass.Forms.Button) this.currentForm.FindControl("newlinkbutton");
        this.btnMakeCurrent = (EllieMae.Encompass.Forms.Button) this.currentForm.FindControl("makecurrentbutton");
        this.labelCCfrom = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("ccfrom");
        this.labelCCfrom2 = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("ccfrom2");
        this.linkloanbutton = (EllieMae.Encompass.Forms.Button) this.currentForm.FindControl("linkloanbutton");
        this.pnlFInitialDraw = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlFirstInitDraw");
        this.pnlFDownPay = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlFirstAppliedDownpay");
        this.pnlFBottom = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlfirstDown");
        this.pnlSInitialDraw = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlSecondInitDraw");
        this.pnlSDownPay = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlSecAppliedDownpay");
        this.pnlSBottom = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlSecDown");
        if (this.loan != null && this.loan.IsTemplate)
        {
          StandardButton control1 = (StandardButton) this.currentForm.FindControl("StandardButton9");
          if (control1 != null)
            control1.Visible = false;
          StandardButton control2 = (StandardButton) this.currentForm.FindControl("StandardButton10");
          if (control2 != null)
            control2.Visible = false;
        }
        this.pnlBorPaidFeesCurrent = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("BorPaidFeesPanelCurrent");
        this.pnlBorPaidFeesNewCurrent = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("BorPaidFeesNewPanelCurrent");
        this.pnlBorPaidFeesPiggy = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("BorPaidFeesPanelPiggy");
        this.pnlBorPaidFeesNewPiggy = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("BorPaidFeesNewPanelPiggy");
        if (this.inputData.GetField("4796") == "Y")
        {
          this.enableOrDisableDropdownPanels(this.pnlBorPaidFeesNewCurrent, this.pnlBorPaidFeesCurrent);
          this.enableOrDisableDropdownPanels(this.pnlBorPaidFeesNewPiggy, this.pnlBorPaidFeesPiggy);
          this.pnlBorPaidFeesNewCurrent.Position = this.pnlBorPaidFeesCurrent.Position;
          this.pnlBorPaidFeesNewPiggy.Position = this.pnlBorPaidFeesPiggy.Position;
        }
        else
        {
          this.enableOrDisableDropdownPanels(this.pnlBorPaidFeesCurrent, this.pnlBorPaidFeesNewCurrent);
          this.enableOrDisableDropdownPanels(this.pnlBorPaidFeesPiggy, this.pnlBorPaidFeesNewPiggy);
        }
      }
      catch (Exception ex)
      {
      }
      try
      {
        if (this.loan != null && (this.loan.LinkSyncType == LinkSyncType.ConstructionLinked || this.loan.LinkSyncType == LinkSyncType.ConstructionPrimary))
          this.disabledRemoveLineButton = this.loan.LinkedData != null && (this.loan.GetLogList().GetAllIDisclosureTracking2015Log(true).Length != 0 || this.loan.LinkedData.GetLogList().GetAllIDisclosureTracking2015Log(true).Length != 0);
        else
          this.disabledRemoveLineButton = false;
      }
      catch (Exception ex)
      {
      }
    }

    protected override string GetFieldValue(string id, FieldSource fieldSource)
    {
      return fieldSource == FieldSource.LinkedLoan && this.loan != null && this.loan.LinkedData == null ? string.Empty : base.GetFieldValue(id, fieldSource);
    }

    internal override void UpdateFieldValue(string fieldId, string val)
    {
      if (fieldId == "4494" && (val == "" || val == "1") && this.loan.GetField("420") == "SecondLien")
        val = this.loan.GetField("4494");
      base.UpdateFieldValue(fieldId, val);
      if (fieldId == "420" && this.loan != null && (this.loan.LinkSyncType == LinkSyncType.PiggybackPrimary || this.loan.LinkSyncType == LinkSyncType.PiggybackLinked))
      {
        this.refreshLienInformation();
        this.SwitchLienPosition(this.currentField.FieldSource, val);
      }
      if (!(fieldId == "136") || this.loan.LinkedData == null)
        return;
      this.RefreshContents("1335");
    }

    internal override void FlipLockField(FieldLock fieldLock)
    {
      base.FlipLockField(fieldLock);
      if (fieldLock.Locked || this.loan == null || this.loan.LinkSyncType != LinkSyncType.PiggybackPrimary && this.loan.LinkSyncType != LinkSyncType.PiggybackLinked)
        return;
      this.loan.SyncPiggyBackFiles((string[]) null, true, false, (string) null, (string) null, false);
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      ControlState defaultValue = this.getControlState(ctrl, id, ControlState.Enabled);
      switch (id)
      {
        case "1172":
          this.showHidePanel();
          defaultValue = ControlState.Enabled;
          break;
        case "140":
          defaultValue = this.GetSpecialControlStatus(id, defaultValue);
          if (((FieldControl) ctrl).FieldSource == FieldSource.LinkedLoan && this.loan != null && this.loan.LinkedData == null)
          {
            defaultValue = ControlState.Disabled;
            break;
          }
          break;
        case "142":
          defaultValue = ControlState.Default;
          if (((FieldControl) ctrl).FieldSource == FieldSource.LinkedLoan)
          {
            EllieMae.Encompass.Forms.Label control = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("lblCashFromTo2");
            if (control != null && this.loan != null && this.loan.LinkedData != null)
              control.Text = this.ToDouble(this.loan.LinkedData.GetSimpleField("142")) <= 0.0 ? "Cash to borrower" : "Cash from borrower";
            if (this.loan != null && this.loan.LinkedData == null)
            {
              defaultValue = ControlState.Disabled;
              break;
            }
            break;
          }
          EllieMae.Encompass.Forms.Label control1 = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("lblCashFromTo");
          if (control1 != null)
          {
            control1.Text = this.ToDouble(this.loan.GetSimpleField("142")) <= 0.0 ? "Cash to borrower" : "Cash from borrower";
            break;
          }
          break;
        case "1845":
          defaultValue = !(this.GetFieldValue("420", ((FieldControl) ctrl).FieldSource) == "SecondLien") ? ControlState.Disabled : ControlState.Default;
          break;
        case "4494":
          string fieldValue = this.GetFieldValue("420", ((FieldControl) ctrl).FieldSource);
          this.showHidePanel();
          return fieldValue == "FirstLien" || fieldValue == "" ? ControlState.Disabled : ControlState.Enabled;
        case "ccprog2":
        case "closingcosts2":
        case "hidl":
        case "loanprog2":
        case "mipff2":
        case "payoffmortgages2":
        case "regz2":
        case "subfin2":
          if (this.loan != null && this.loan.LinkedData == null)
          {
            defaultValue = ControlState.Disabled;
            break;
          }
          break;
        case "linktoloan":
        case "newlink":
          if (this.loan != null && (this.loan.LinkedData != null || this.inputData.GetField("19") == "ConstructionOnly" && this.inputData.GetField("4084") == "Y" && this.loan.GetLogList().GetAllIDisclosureTracking2015Log(true).Length != 0 || this.loan.IsTemplate))
          {
            defaultValue = ControlState.Disabled;
            break;
          }
          break;
        case "maketocurrent":
        case "syncdata":
          if (this.loan != null && (this.loan.LinkedData == null || this.loan.IsTemplate))
          {
            defaultValue = ControlState.Disabled;
            break;
          }
          break;
        case "newheloc":
          if (this.loan != null && this.loan.LinkedData != null || this.loan.IsTemplate)
          {
            defaultValue = ControlState.Disabled;
            break;
          }
          break;
        case "removelink":
          if (this.loan != null && (this.loan.LinkedData == null || this.loan.IsTemplate || this.disabledRemoveLineButton))
          {
            defaultValue = ControlState.Disabled;
            break;
          }
          break;
        default:
          defaultValue = ControlState.Default;
          if (ctrl is FieldControl && ((FieldControl) ctrl).FieldSource == FieldSource.LinkedLoan && this.loan != null && this.loan.LinkedData == null)
          {
            defaultValue = ControlState.Disabled;
            break;
          }
          break;
      }
      return defaultValue;
    }

    public override void ExecAction(string action)
    {
      base.ExecAction(action);
      bool flag = false;
      if (action == "syncdata" || action == "newlink" || action == "newheloc" || action == "linktoloan")
      {
        flag = this.session.LoanData.Calculator.SkipLockRequestSync;
        this.session.LoanData.Calculator.SkipLockRequestSync = true;
        if (this.session.LoanData.LinkedData != null && this.session.LoanData.LinkedData.Calculator != null)
          this.session.LoanData.LinkedData.Calculator.SkipLockRequestSync = true;
      }
      switch (action)
      {
        case "ccprog":
          this.SetFieldFocus("l_136");
          break;
        case "ccprog2":
          this.SetFieldFocus("l_136_link");
          break;
        case "closingcosts":
          this.SetFieldFocus("l_140");
          break;
        case "closingcosts2":
          if (this.loan == null || this.loan.GetField("NEWHUD.X354") == "Y" || Utils.CheckIf2015RespaTila(this.loan.GetField("3969")))
          {
            if (this.loan == null || Utils.CheckIf2015RespaTila(this.loan.GetField("3969")))
              this.LoadQuickLinkForm("2015 Itemization", "REGZGFE_2015", 750, 600, FieldSource.LinkedLoan);
            else
              this.LoadQuickLinkForm("2010 Itemization", "REGZGFE_2010", 750, 600, FieldSource.LinkedLoan);
          }
          else
            this.LoadQuickLinkForm("Closing Costs", "PREQUAL_REGZGFE", 710, 512, FieldSource.LinkedLoan);
          this.SetFieldFocus("l_140_link");
          break;
        case "hidl":
          this.RefreshContents();
          break;
        case "linktoloan":
          Cursor.Current = Cursors.WaitCursor;
          this.linkToLoan();
          Cursor.Current = Cursors.Default;
          break;
        case "loanprog":
          this.SetFieldFocus("l_136");
          break;
        case "loanprog2":
          this.SetFieldFocus("l_136_link");
          break;
        case "maketocurrent":
          this.makeToCurrentInput();
          break;
        case "mipff":
          this.SetFieldFocus("l_1109");
          break;
        case "mipff2":
          this.SetFieldFocus("l_1109_link");
          break;
        case "newheloc":
          Cursor.Current = Cursors.WaitCursor;
          this.addNewHelocLoan();
          Cursor.Current = Cursors.Default;
          break;
        case "newlink":
          Cursor.Current = Cursors.WaitCursor;
          this.addNewLinkedLoan();
          Cursor.Current = Cursors.Default;
          break;
        case "payoffmortgages":
          this.SetFieldFocus("l_140");
          break;
        case "payoffmortgages2":
          this.LoadQuickLinkForm("Payoff Mortgages", "VOLPanel/VOMPanel", 688, 512, FieldSource.LinkedLoan);
          this.SetFieldFocus("l_140_link");
          break;
        case "regz":
          this.SetFieldFocus("l_3");
          break;
        case "regz2":
          if (this.loan != null && this.loan.Use2015RESPA)
            this.LoadQuickLinkForm("REGZ - LE", "REGZLE", 730, 512, FieldSource.LinkedLoan);
          else
            this.LoadQuickLinkForm("REGZ - TIL", "PREQUAL_REGZ50", 688, 512, FieldSource.LinkedLoan);
          this.SetFieldFocus("l_3_link");
          break;
        case "removelink":
          this.RemoveLinkedLoan(true);
          break;
        case "subfin":
          this.SetFieldFocus("l_3");
          break;
        case "subfin2":
          this.SetFieldFocus("l_3_link");
          break;
        case "syncdata":
          Cursor.Current = Cursors.WaitCursor;
          this.syncFields(false);
          Cursor.Current = Cursors.Default;
          break;
      }
      if (!(action == "syncdata") && !(action == "newlink") && !(action == "newheloc") && !(action == "linktoloan"))
        return;
      this.session.LoanData.Calculator.SkipLockRequestSync = flag;
      if (this.session.LoanData.LinkedData == null || this.session.LoanData.LinkedData.Calculator == null)
        return;
      this.session.LoanData.LinkedData.Calculator.SkipLockRequestSync = flag;
    }

    private void refreshLienInformation()
    {
      if (this.loan == null || this.loan.LinkSyncType == LinkSyncType.None)
        return;
      bool flag = this.loan.LinkSyncType == LinkSyncType.ConstructionPrimary || this.loan.LinkSyncType == LinkSyncType.PiggybackPrimary;
      if ((this.loan.LinkSyncType == LinkSyncType.ConstructionPrimary || this.loan.LinkSyncType == LinkSyncType.ConstructionLinked) && this.currentForm.Name != "Piggyback Loans")
      {
        this.labelLienPOS.Text = (flag ? "Construction" : "Perm") + " Loan (Current Input)";
        this.labelLienPOS2.Text = (flag ? "Perm" : "Construction") + " Loan";
        this.linkloanbutton.Text = "Link to " + (flag ? "Perm" : "Construction");
        this.btnNewLink.Text = "New " + (flag ? "Perm" : "Construction");
        this.btnMakeCurrent.Text = "Go to " + (flag ? "Perm" : "Construction");
        this.labelCCfrom.Text = "CC From 2nd";
        this.labelCCfrom2.Text = "CC From 2nd";
      }
      else
      {
        if (this.loan.LinkSyncType != LinkSyncType.PiggybackPrimary && this.loan.LinkSyncType != LinkSyncType.PiggybackLinked)
          return;
        this.labelLienPOS.Text = (flag ? "1st Loan Position" : "2nd Loan Position") + " (Current Input)";
        this.labelLienPOS2.Text = (flag ? "2nd" : "1st") + " Loan Position";
        this.btnNewLink.Text = "New " + (flag ? "2nd" : "1st");
        this.btnMakeCurrent.Text = "Go to " + (flag ? "2nd" : "1st");
        this.labelCCfrom.Text = "CC From " + (flag ? "2nd" : "1st");
        this.labelCCfrom2.Text = "CC From " + (flag ? "1st" : "2nd");
      }
    }

    private void syncFields(bool doYou)
    {
      LoanData loanData = (LoanData) null;
      bool flag = false;
      if (this.inputData is LoanData)
      {
        loanData = (LoanData) this.inputData;
        flag = loanData.Calculator.SkipLockRequestSync;
        loanData.Calculator.SkipLockRequestSync = true;
        if (loanData.LinkedData != null)
          loanData.LinkedData.Calculator.SkipLockRequestSync = true;
      }
      try
      {
        PIGGYBACKInputHandler.SyncFields(doYou, this.session, this.inputData, this.PiggyFields, (InputHandlerBase) this);
      }
      catch (Exception ex)
      {
        Tracing.Log(InputHandlerBase.sw, nameof (PIGGYBACKInputHandler), TraceLevel.Info, "PiggybackInputHandler:syncFields: cannot sync fields. Error: " + ex.Message);
      }
      finally
      {
        if (loanData != null)
        {
          loanData.Calculator.SkipLockRequestSync = flag;
          if (loanData.LinkedData != null)
            loanData.LinkedData.Calculator.SkipLockRequestSync = flag;
        }
      }
    }

    private void makeToCurrentInput()
    {
      string loanFolder = Session.LoanDataMgr.LinkedLoan.LoanFolder;
      string loanName = Session.LoanDataMgr.LinkedLoan.LoanName;
      string guid = Session.LoanDataMgr.LinkedLoan.LoanData.GUID;
      if (Session.LoanDataMgr.Dirty || Session.LoanDataMgr.LinkedLoan.Dirty)
      {
        if (Utils.Dialog((IWin32Window) this.session.MainForm, "You must save both loans before you can switch the input position. Do you want to save the loans now?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
          return;
        try
        {
          try
          {
            Session.LoanDataMgr.Save();
          }
          catch (Exception ex)
          {
            int num = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "Error saving loan file: " + ex.Message);
            return;
          }
          loanFolder = Session.LoanDataMgr.LinkedLoan.LoanFolder;
          loanName = Session.LoanDataMgr.LinkedLoan.LoanName;
          Session.LoanDataMgr.LoanData.Dirty = false;
          Session.LoanDataMgr.LinkedLoan.LoanData.Dirty = false;
        }
        catch (Exception ex)
        {
          int num = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "Both loans cannot be saved. Error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return;
        }
      }
      else
      {
        Session.LoanDataMgr.LoanData.Dirty = false;
        Session.LoanDataMgr.LinkedLoan.LoanData.Dirty = false;
      }
      if (string.Compare(Session.WorkingFolder, loanFolder, true) != 0)
        Session.WorkingFolder = loanFolder;
      ILoanConsole service1 = Session.Application.GetService<ILoanConsole>();
      ILoanEditor service2 = Session.Application.GetService<ILoanEditor>();
      if (service1.OpenLoan(guid, LoanInfo.LockReason.OpenForWork, true))
      {
        if (Session.LoanDataMgr.LinkedLoan == null)
        {
          int num1 = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "The linked loan cannot be reloaded.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else if (this.loan.LinkSyncType == LinkSyncType.ConstructionPrimary || this.loan.LinkSyncType == LinkSyncType.ConstructionLinked)
        {
          service2.OpenForm("Construction Management : Linked Loans");
          int num2 = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "The " + (string.Compare(Session.LoanDataMgr.LoanData.GetField("19"), "ConstructionOnly", true) == 0 ? "construction" : "permanent") + " loan has been switched to current input screen.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        else
        {
          service2.OpenForm("Piggyback Loans");
          int num3 = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "The " + (string.Compare(Session.LoanDataMgr.LoanData.GetField("420"), "SecondLien", true) == 0 ? "2nd" : "1st") + " loan has been switched to current input screen.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
      }
      else
      {
        int num4 = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "The " + (this.loan.LinkSyncType == LinkSyncType.PiggybackPrimary || this.loan.LinkSyncType == LinkSyncType.PiggybackLinked ? "Piggyback" : "Linked") + " loan cannot be reloaded.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void addNewLinkedLoan()
    {
      PIGGYBACKInputHandler.AddNewLinkedLoan(this.session, this.inputData, this.PiggyFields, this, false);
    }

    private void addNewHelocLoan()
    {
      PIGGYBACKInputHandler.AddNewLinkedLoan(this.session, this.inputData, this.PiggyFields, this, true);
    }

    private void linkToLoan()
    {
      PIGGYBACKInputHandler.LinkToLoan(this.session, this.inputData, this.PiggyFields, this);
    }

    private void showHidePanel()
    {
      if (this.pnlFInitialDraw == null || this.pnlFDownPay == null || this.pnlFBottom == null || this.pnlSInitialDraw == null || this.pnlSDownPay == null || this.pnlSBottom == null)
        return;
      this.adjPanel(this.pnlFInitialDraw, this.pnlFDownPay, this.pnlFBottom, this.loan.GetField("1172"), this.loan.GetField("4494"));
      if (this.loan.LinkedData == null)
        return;
      this.adjPanel(this.pnlSInitialDraw, this.pnlSDownPay, this.pnlSBottom, this.loan.LinkedData.GetField("1172"), this.loan.LinkedData.GetField("4494"));
    }

    private void adjPanel(
      EllieMae.Encompass.Forms.Panel pnl1,
      EllieMae.Encompass.Forms.Panel pnl2,
      EllieMae.Encompass.Forms.Panel pnl3,
      string loanType,
      string lienPosition)
    {
      if (loanType == "HELOC")
      {
        pnl2.Visible = pnl1.Visible = true;
        pnl2.Top = pnl1.Bottom;
        pnl3.Top = pnl2.Bottom + 3;
      }
      else
      {
        pnl2.Visible = !(pnl1.Visible = false);
        pnl3.Position = pnl2.Position;
        pnl2.Position = pnl1.Position;
      }
      if (!(lienPosition == "1"))
        return;
      pnl1.Visible = !(pnl2.Visible = false);
      pnl3.Position = pnl2.Position;
    }

    internal static void RemoveLinkedLoan(
      Sessions.Session session,
      IHtmlInput inputData,
      InputHandlerBase handler,
      bool askConfirmed)
    {
      if (session.LoanDataMgr.LinkedLoan == null || !session.LoanDataMgr.LinkedLoan.LockLoanWithExclusiveA(true, (string) null, false) || !session.LoanDataMgr.LockLoanWithExclusiveA(true, (string) null, false) || askConfirmed && Utils.Dialog((IWin32Window) session.MainForm, "Both loans will be saved after the link is removed. Do you want to proceed?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      Cursor.Current = Cursors.WaitCursor;
      if (inputData.GetField("HMDA.X24") == "Y")
      {
        if (DialogResult.OK != Utils.Dialog((IWin32Window) session.MainForm, "Removing this link will no longer exclude the Construction loan from HMDA reporting. Do you wish to proceed?", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation))
          return;
        Session.LoanDataMgr.LoanData.SetCurrentField("HMDA.X24", "");
      }
      try
      {
        if (session.LoanDataMgr.LinkedLoan.LoanData.ContentAccess != LoanContentAccess.FullAccess)
          session.LoanDataMgr.LinkedLoan.LoanData.ContentAccess = LoanContentAccess.FullAccess;
        session.LoanDataMgr.LinkedLoan.LoanData.SetCurrentField("LINKGUID", "");
        session.LoanDataMgr.LoanData.Calculator.EditCalcOnDemandEnum(CalcOnDemandEnum.CreateConstructionLinkedSubsetFields, false);
        session.LoanDataMgr.LoanData.RemoveSubsetConstructionLinkedFieldValues();
        session.LoanDataMgr.LinkedLoan.LoanData.Calculator.EditCalcOnDemandEnum(CalcOnDemandEnum.CreateConstructionLinkedSubsetFields, false);
        session.LoanDataMgr.LinkedLoan.LoanData.RemoveSubsetConstructionLinkedFieldValues();
        try
        {
          session.LoanDataMgr.LinkedLoan.Calculator.FormCalculation("UNLINKEDREMOTE", (string) null, (string) null);
          if (session.LoanDataMgr.LoanData.GetField("420") == "SecondLien")
          {
            session.LoanDataMgr.LinkedLoan.Calculator.FormCalculation("REMOVELINK", (string) null, (string) null);
            session.LoanDataMgr.Calculator.FormCalculation("REMOVELINK", (string) null, (string) null);
          }
          else
          {
            session.LoanDataMgr.Calculator.FormCalculation("REMOVELINK", (string) null, (string) null);
            session.LoanDataMgr.LinkedLoan.Calculator.FormCalculation("REMOVELINK", (string) null, (string) null);
          }
          session.LoanDataMgr.LinkedLoan.LoanData.LinkedData = (LoanData) null;
          session.LoanDataMgr.LinkedLoan.Calculator.FormCalculation("428");
          session.LoanDataMgr.LinkedLoan.SaveLoan(false, false, false, false, (ILoanMilestoneTemplateOrchestrator) null, false, out bool _);
        }
        catch (Exception ex)
        {
          int num = (int) Utils.Dialog((IWin32Window) session.MainForm, "Error saving loan file: " + ex.Message);
          return;
        }
        Session.LoanDataMgr.Unlink();
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) session.MainForm, "The current link information cannot be removed. Error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return;
      }
      Session.LoanDataMgr.LoanData.SetCurrentField("LINKGUID", "");
      if (Session.LoanDataMgr.LoanData.IsLocked("1851"))
        Session.LoanDataMgr.LoanData.RemoveLock("1851");
      try
      {
        Session.LoanDataMgr.Save();
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) session.MainForm, "Error saving loan file: " + ex.Message);
        return;
      }
      handler?.RefreshContents();
      if (inputData.GetField("19") != "ConstructionOnly" || inputData.GetField("4084") != "Y")
        session.Application.GetService<ILoanEditor>().RefreshContents();
      Cursor.Current = Cursors.Default;
      int num1 = (int) Utils.Dialog((IWin32Window) session.MainForm, "The current link information has been removed and both loan have been saved.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    }

    private static LoanFolderRuleInfo getLoanFolderRule(LoanFolderInfo loanFolder)
    {
      if (loanFolder.Name == SystemSettings.AllFolders)
        return new LoanFolderRuleInfo(loanFolder.Name, false);
      return loanFolder.Type == LoanFolderInfo.LoanFolderType.Trash ? new LoanFolderRuleInfo(loanFolder.Name, false) : ((LoanFolderRuleManager) Session.BPM.GetBpmManager(BpmCategory.LoanFolder)).GetRule(loanFolder.Name);
    }

    internal static bool AddNewLinkedLoan(
      Sessions.Session session,
      IHtmlInput inputData,
      PiggybackFields piggybackFields,
      PIGGYBACKInputHandler piggybackHandler,
      bool setLoanTypeToHELOC)
    {
      LoanData inputData1 = inputData is LoanData ? (LoanData) inputData : (LoanData) null;
      if (inputData1 == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) session.MainForm, "This function cannot be used only in Piggyback tool in a loan.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      if (session.LoanDataMgr.LinkedLoan != null)
      {
        int num = (int) Utils.Dialog((IWin32Window) session.MainForm, "This current loan is a Piggyback loan. Please remove current link first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      if (!Session.LoanDataMgr.LockLoanWithExclusiveA(true))
        return false;
      Tracing.Log(InputHandlerBase.sw, TraceLevel.Info, nameof (PIGGYBACKInputHandler), "trying to creat a new blank loan for a linked loan. Loan folder: " + Session.LoanDataMgr.LoanFolder);
      Cursor.Current = Cursors.WaitCursor;
      if (!PIGGYBACKInputHandler.getLoanFolderRule(new LoanFolderInfo(session.LoanDataMgr.LoanFolder)).CanPiggybackLoans)
      {
        int num = (int) Utils.Dialog((IWin32Window) session.MainForm, "You don't have permission to create a new loan file in folder \"" + session.LoanDataMgr.LoanFolder + "\"!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      LoanDataMgr mgr = LoanDataMgr.NewLoan(session.SessionObjects, session.LoanDataMgr.LoanFolder);
      if (mgr == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) session.MainForm, "Cannot create a new piggyback loan due to an error.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      Tracing.Log(InputHandlerBase.sw, TraceLevel.Info, nameof (PIGGYBACKInputHandler), "trying to link the newly created linked loan.");
      try
      {
        session.LoanDataMgr.LinkTo(mgr);
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) session.MainForm, "Cannot link the new Piggyback loan.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        Tracing.Log(InputHandlerBase.sw, TraceLevel.Info, nameof (PIGGYBACKInputHandler), "trying to link the newly created linked loan. Error: " + ex.Message);
        return false;
      }
      bool skipLockRequestSync = inputData1.Calculator.SkipLockRequestSync;
      inputData1.Calculator.SkipLockRequestSync = true;
      if (session.LoanDataMgr.LinkedLoan != null)
      {
        session.LoanDataMgr.LinkedLoan.Calculator.SkipLockRequestSync = true;
        try
        {
          if (inputData1 != null && (inputData1.LinkSyncType == LinkSyncType.PiggybackPrimary || inputData1.LinkSyncType == LinkSyncType.PiggybackLinked))
          {
            if (session.LoanDataMgr.LoanData.GetField("19") == "ConstructionOnly")
              session.LoanDataMgr.LinkedLoan.LoanData.SetCurrentField("19", "ConstructionToPermanent");
            else if (session.LoanDataMgr.LoanData.GetField("19") == "ConstructionToPermanent")
              session.LoanDataMgr.LinkedLoan.LoanData.SetCurrentField("19", "ConstructionOnly");
          }
          if (inputData1.LinkSyncType == LinkSyncType.ConstructionLinked && inputData1.LinkedData.LinkSyncType == LinkSyncType.ConstructionPrimary || inputData1.LinkSyncType == LinkSyncType.ConstructionPrimary && inputData1.LinkedData.LinkSyncType == LinkSyncType.ConstructionLinked)
          {
            inputData1.LinkedData.SetCurrentField("136", inputData1.GetSimpleField("136"));
            inputData1.LinkedData.SetCurrentField("1771", inputData1.GetSimpleField("1771"));
            inputData1.LinkedData.SetCurrentField("1335", inputData1.GetSimpleField("1335"));
            inputData1.LinkedData.SetCurrentField("1109", inputData1.GetSimpleField("1109"));
          }
          string simpleField1 = inputData1.GetSimpleField("1851");
          string simpleField2 = inputData1.LinkedData.GetSimpleField("1851");
          PIGGYBACKInputHandler.SyncFields(true, session, (IHtmlInput) inputData1, piggybackFields, (InputHandlerBase) piggybackHandler);
          if (inputData1.LinkSyncType == LinkSyncType.PiggybackPrimary || inputData1.LinkSyncType == LinkSyncType.PiggybackLinked)
          {
            if (!string.IsNullOrEmpty(simpleField1) && simpleField1 != inputData1.GetSimpleField("1851"))
            {
              inputData1.AddLock("1851");
              inputData1.SetCurrentField("1851", simpleField1);
            }
            if (!string.IsNullOrEmpty(simpleField2) && simpleField2 != inputData1.LinkedData.GetSimpleField("1851"))
            {
              inputData1.LinkedData.AddLock("1851");
              inputData1.LinkedData.SetCurrentField("1851", simpleField2);
            }
          }
          session.LoanDataMgr.LinkedLoan.LoanData.SetCurrentField("NEWHUD.X354", session.LoanDataMgr.LoanData.GetField("NEWHUD.X354"));
          session.LoanDataMgr.LinkedLoan.LoanData.SetCurrentField("3969", session.LoanDataMgr.LoanData.GetField("3969"));
          session.LoanDataMgr.LinkedLoan.LoanData.SetCurrentField("LINKGUID", session.LoanDataMgr.LoanData.GUID);
          session.LoanDataMgr.LoanData.SetCurrentField("LINKGUID", session.LoanDataMgr.LinkedLoan.LoanData.GUID);
          if (setLoanTypeToHELOC)
            session.LoanDataMgr.LinkedLoan.LoanData.SetCurrentField("1172", "HELOC");
          if (inputData1.LinkSyncType != LinkSyncType.ConstructionLinked && inputData1.LinkSyncType != LinkSyncType.ConstructionPrimary && (setLoanTypeToHELOC || session.LoanDataMgr.LoanData.GetField("420") == "FirstLien"))
          {
            session.LoanDataMgr.LinkedLoan.LoanData.SetCurrentField("1335", session.LoanDataMgr.LoanData.GetField("1109"));
            session.LoanDataMgr.LinkedLoan.LoanData.SetField("1109", session.LoanDataMgr.LoanData.GetField("1335"));
            if (Utils.ParseDecimal((object) session.LoanDataMgr.LinkedLoan.LoanData.GetField("136")) != 0M && Utils.ParseDecimal((object) session.LoanDataMgr.LinkedLoan.LoanData.GetField("1335")) != 0M)
            {
              Decimal d = Utils.ParseDecimal((object) session.LoanDataMgr.LinkedLoan.LoanData.GetSimpleField("1335")) / Utils.ParseDecimal((object) session.LoanDataMgr.LinkedLoan.LoanData.GetSimpleField("136")) * 100M;
              session.LoanDataMgr.LinkedLoan.LoanData.SetCurrentField("1771", Math.Round(d, 3).ToString());
            }
          }
          session.LoanDataMgr.LinkedLoan.LoanData.CalculateSubordinate(session.LoanDataMgr.LoanData.GetField("420") == "FirstLien", session.LoanDataMgr.LoanData, session.LoanDataMgr.LinkedLoan.LoanData, session.LoanDataMgr.LoanData.Calculator != null && session.LoanDataMgr.LoanData.Calculator.IsPiggybackHELOC);
          if (session.LoanDataMgr.LoanData.Calculator != null)
          {
            session.LoanDataMgr.LoanData.Calculator.FormCalculation("4487");
            session.LoanDataMgr.LoanData.Calculator.FormCalculation("LinkVoal");
          }
          session.LoanDataMgr.LoanData.Calculator.EditCalcOnDemandEnum(CalcOnDemandEnum.CreateConstructionLinkedSubsetFields, true);
          session.LoanDataMgr.LinkedLoan.LoanData.Calculator.EditCalcOnDemandEnum(CalcOnDemandEnum.CreateConstructionLinkedSubsetFields, true);
        }
        catch (Exception ex)
        {
          Tracing.Log(InputHandlerBase.sw, nameof (PIGGYBACKInputHandler), TraceLevel.Info, "PiggybackInputHandler:AddNewLinkedLoan: cannot add new linked loan. Error: " + ex.Message);
        }
        finally
        {
          inputData1.Calculator.SkipLockRequestSync = session.LoanDataMgr.LinkedLoan.Calculator.SkipLockRequestSync = skipLockRequestSync;
        }
      }
      if (inputData1.LinkSyncType == LinkSyncType.PiggybackPrimary || inputData1.LinkSyncType == LinkSyncType.PiggybackPrimary)
        PIGGYBACKInputHandler.loadPiggybackInputScreen(session);
      else
        piggybackHandler?.RefreshContents();
      Cursor.Current = Cursors.Default;
      return true;
    }

    internal static void LinkToLoan(
      Sessions.Session session,
      IHtmlInput inputData,
      PiggybackFields piggyFields,
      PIGGYBACKInputHandler piggybackHandler)
    {
      if (!(inputData is LoanData))
      {
        int num1 = (int) Utils.Dialog((IWin32Window) session.MainForm, "This function can be used in Piggyback tool only.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (!session.LoanDataMgr.LockLoanWithExclusiveA(true) || session.LoanDataMgr.LinkedLoan != null && (!session.LoanDataMgr.LinkedLoan.LockLoanWithExclusiveA(true) || Utils.Dialog((IWin32Window) session.MainForm, "This loan currently is linking to another loan. Do you want to remove current link?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes))
          return;
        Cursor.Current = Cursors.WaitCursor;
        LoanData loanData = (LoanData) inputData;
        bool forLinkSync = loanData.GetField("19") == "ConstructionOnly" && loanData.GetField("4084") == "Y";
        using (LinkLoanDialog linkLoanDialog = new LinkLoanDialog(Session.LoanDataMgr.LoanFolder, loanData.GUID, forLinkSync))
        {
          Cursor.Current = Cursors.Default;
          if (linkLoanDialog.ShowDialog((IWin32Window) Session.MainForm) != DialogResult.OK)
            return;
          Cursor.Current = Cursors.WaitCursor;
          LoanDataMgr mgr = !forLinkSync ? LoanDataMgr.OpenLoan(Session.SessionObjects, linkLoanDialog.SelectedInfo.LoanFolder, linkLoanDialog.SelectedInfo.LoanName, false) : linkLoanDialog.SelectedLinkLoanDataMgr;
          if (mgr == null)
          {
            int num2 = (int) Utils.Dialog((IWin32Window) session.MainForm, "The selected Loan cannot be opened.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
          else
          {
            if (Session.LoanDataMgr.LinkedLoan != null)
            {
              try
              {
                Session.LoanDataMgr.LinkedLoan.LoanData.SetCurrentField("LINKGUID", "");
                try
                {
                  Session.LoanDataMgr.LinkedLoan.Calculator.ForceDefaultLinkSync = false;
                  Session.LoanDataMgr.LinkedLoan.Save();
                  Session.LoanDataMgr.LinkedLoan.Calculator.ForceDefaultLinkSync = true;
                }
                catch (Exception ex)
                {
                  int num3 = (int) Utils.Dialog((IWin32Window) session.MainForm, "Error saving loan file: " + ex.Message);
                  return;
                }
                session.LoanDataMgr.LinkedLoan.Close();
              }
              catch (Exception ex)
              {
                int num4 = (int) Utils.Dialog((IWin32Window) session.MainForm, "The current link information cannot be removed. Error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
              }
            }
            try
            {
              mgr.Lock(LoanInfo.LockReason.OpenForWork, LockInfo.ExclusiveLock.ExclusiveA);
            }
            catch (Exception ex)
            {
              int num5 = (int) Utils.Dialog((IWin32Window) session.MainForm, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
              return;
            }
            Session.LoanDataMgr.LoanData.SetCurrentField("LINKGUID", linkLoanDialog.SelectedInfo.GUID);
            Tracing.Log(InputHandlerBase.sw, TraceLevel.Info, nameof (PIGGYBACKInputHandler), "trying to link the newly created linked loan.");
            try
            {
              Session.LoanDataMgr.LinkTo(mgr);
            }
            catch (Exception ex)
            {
              if (loanData.LinkSyncType == LinkSyncType.ConstructionPrimary || loanData.LinkSyncType == LinkSyncType.ConstructionLinked)
              {
                int num6 = (int) Utils.Dialog((IWin32Window) session.MainForm, "Can not link the new loan.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              }
              else
              {
                int num7 = (int) Utils.Dialog((IWin32Window) session.MainForm, "Can not link the new Piggyback loan.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              }
              Tracing.Log(InputHandlerBase.sw, TraceLevel.Info, nameof (PIGGYBACKInputHandler), "trying to link the newly created linked loan. Error: " + ex.Message);
              return;
            }
            if (Session.LoanDataMgr.LinkedLoan != null)
            {
              if (!Session.LoanDataMgr.LinkedLoan.LockLoanWithExclusiveA(true))
                return;
              Session.LoanDataMgr.LinkedLoan.LoanData.SetCurrentField("LINKGUID", Session.LoanDataMgr.LoanData.GUID);
              Session.LoanDataMgr.LoanData.Calculator.EditCalcOnDemandEnum(CalcOnDemandEnum.CreateConstructionLinkedSubsetFields, true);
              Session.LoanDataMgr.LinkedLoan.LoanData.Calculator.EditCalcOnDemandEnum(CalcOnDemandEnum.CreateConstructionLinkedSubsetFields, true);
              try
              {
                Session.LoanDataMgr.LinkedLoan.Calculator.ForceDefaultLinkSync = false;
                Session.LoanDataMgr.LinkedLoan.Save();
                Session.LoanDataMgr.LinkedLoan.Calculator.ForceDefaultLinkSync = true;
              }
              catch (Exception ex)
              {
                int num8 = (int) Utils.Dialog((IWin32Window) session.MainForm, "Error saving loan file: " + ex.Message);
                return;
              }
              Session.LoanDataMgr.LinkedLoan.LoanData.ToPipelineInfo();
              LoanContentAccess loanContentAccess = ((LoanAccessBpmManager) Session.BPM.GetBpmManager(BpmCategory.LoanAccess)).GetLoanContentAccess(Session.LoanDataMgr.LinkedLoan.LoanData);
              if (Session.LoanDataMgr.LinkedLoan.AccessRules.AllowFullAccess())
                loanContentAccess = LoanContentAccess.FullAccess;
              Session.LoanDataMgr.LinkedLoan.LoanData.ContentAccess = loanContentAccess;
              if (Session.LoanDataMgr.LinkedLoan.LoanData.ContentAccess != LoanContentAccess.FullAccess)
              {
                if (loanData.LinkSyncType == LinkSyncType.ConstructionPrimary || loanData.LinkSyncType == LinkSyncType.ConstructionLinked)
                {
                  piggybackHandler?.RefreshContents();
                  int num9 = (int) Utils.Dialog((IWin32Window) session.MainForm, "You don't have full rights to link selected loan. The linked loan cannot be sychronized.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                  if (piggybackHandler != null)
                    PIGGYBACKInputHandler.loadPiggybackInputScreen(session);
                  int num10 = (int) Utils.Dialog((IWin32Window) session.MainForm, "You don't have full rights to piggyback loan. The piggyback loan cannot be sychronized.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
              }
            }
            bool skipLockRequestSync = loanData.Calculator.SkipLockRequestSync;
            loanData.Calculator.SkipLockRequestSync = loanData.LinkedData.Calculator.SkipLockRequestSync = true;
            string simpleField1 = loanData.GetSimpleField("1851");
            string simpleField2 = loanData.LinkedData.GetSimpleField("1851");
            try
            {
              PIGGYBACKInputHandler.SyncFields(true, session, inputData, piggyFields, (InputHandlerBase) piggybackHandler);
            }
            catch (Exception ex)
            {
              Tracing.Log(InputHandlerBase.sw, nameof (PIGGYBACKInputHandler), TraceLevel.Info, "PiggybackInputHandler:LinkToLoan: cannot sync fields. Error: " + ex.Message);
            }
            finally
            {
              loanData.Calculator.SkipLockRequestSync = loanData.LinkedData.Calculator.SkipLockRequestSync = skipLockRequestSync;
            }
            if (loanData.LinkSyncType == LinkSyncType.PiggybackPrimary || loanData.LinkSyncType == LinkSyncType.PiggybackLinked)
            {
              if (!string.IsNullOrEmpty(simpleField1) && simpleField1 != loanData.GetSimpleField("1851"))
              {
                loanData.AddLock("1851");
                loanData.SetCurrentField("1851", simpleField1);
              }
              if (!string.IsNullOrEmpty(simpleField2) && simpleField2 != loanData.LinkedData.GetSimpleField("1851"))
              {
                loanData.LinkedData.AddLock("1851");
                loanData.LinkedData.SetCurrentField("1851", simpleField2);
              }
            }
            if (session.LoanDataMgr.LoanData.Calculator != null)
              session.LoanDataMgr.LoanData.Calculator.FormCalculation("LINKVOALEXISTING");
            session.LoanDataMgr.LinkedLoan.LoanData.CalculateSubordinate(session.LoanDataMgr.LoanData.GetField("420") == "FirstLien", session.LoanDataMgr.LoanData, session.LoanDataMgr.LinkedLoan.LoanData, session.LoanDataMgr.LoanData.Calculator != null && session.LoanDataMgr.LoanData.Calculator.IsPiggybackHELOC);
            if (loanData.LinkSyncType == LinkSyncType.PiggybackPrimary || loanData.LinkSyncType == LinkSyncType.PiggybackPrimary)
              PIGGYBACKInputHandler.loadPiggybackInputScreen(session);
            else
              piggybackHandler?.RefreshContents();
            Cursor.Current = Cursors.Default;
          }
        }
      }
    }

    internal static void SyncFields(
      bool doYou,
      Sessions.Session session,
      IHtmlInput inputData,
      PiggybackFields piggyFields,
      InputHandlerBase handlerBase)
    {
      LoanData loanData = inputData is LoanData ? (LoanData) inputData : (LoanData) null;
      bool runPostSyncOnly = false;
      string[] ids;
      if (loanData != null && (loanData.LinkSyncType == LinkSyncType.ConstructionPrimary || loanData.LinkSyncType == LinkSyncType.ConstructionLinked))
      {
        ids = session.Application.GetService<ILoanEditor>().SelectLinkAndSyncTemplate();
        if (ids == null)
          return;
      }
      else
        ids = piggyFields.GetSyncFields();
      if (ids == null || ids.Length == 0)
      {
        runPostSyncOnly = true;
        if (loanData != null && (loanData.LinkSyncType == LinkSyncType.ConstructionPrimary || loanData.LinkSyncType == LinkSyncType.ConstructionLinked))
        {
          int num = (int) Utils.Dialog((IWin32Window) session.MainForm, "Synchronization Field List is empty. Both loans won't be synchronized.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return;
        }
        int num1 = (int) Utils.Dialog((IWin32Window) session.MainForm, "Synchronization Field List is empty. Only some default fields will be synchronized.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      if (!runPostSyncOnly)
      {
        if (doYou)
        {
          if (Utils.Dialog((IWin32Window) session.MainForm, "Do you want to synchronize data between two loans?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            runPostSyncOnly = true;
        }
        else if (Utils.Dialog((IWin32Window) session.MainForm, "Are you sure you want to synchronize data between two loans?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
          runPostSyncOnly = true;
      }
      if (((loanData == null ? 0 : (loanData.LinkSyncType == LinkSyncType.ConstructionPrimary ? 1 : (loanData.LinkSyncType == LinkSyncType.ConstructionLinked ? 1 : 0))) & (runPostSyncOnly ? 1 : 0)) != 0)
        return;
      Cursor.Current = Cursors.WaitCursor;
      ArrayList arrayList = loanData.SyncPiggyBackFiles(ids, runPostSyncOnly, true, (string) null, (string) null, false);
      Cursor.Current = Cursors.Default;
      handlerBase?.RefreshContents();
      if (runPostSyncOnly)
        return;
      if (arrayList.Count == 0)
      {
        if (doYou)
          return;
        int num = (int) Utils.Dialog((IWin32Window) session.MainForm, "Both loans have been synchronized.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        string str = "";
        for (int index = 0; index < arrayList.Count; ++index)
          str = !(str == "") ? str + ", " + (string) arrayList[index] : (string) arrayList[index];
        int num = (int) Utils.Dialog((IWin32Window) session.MainForm, "The following fields cannot be synchronized because those fields are locked fields:\r\n\r\n" + str, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }

    private static void loadPiggybackInputScreen(Sessions.Session session)
    {
      session.Application.GetService<ILoanEditor>().OpenForm("Piggyback Loans");
    }
  }
}
