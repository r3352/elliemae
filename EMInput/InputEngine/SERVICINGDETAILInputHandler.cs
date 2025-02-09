// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.SERVICINGDETAILInputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using EllieMae.Encompass.Forms;
using mshtml;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class SERVICINGDETAILInputHandler : InputHandlerBase
  {
    private const string className = "SERVICINGDETAILInputHandler";
    private PaymentScheduleSnapshot paySnapshot;
    private PaymentEscrowInputHandler pmtEscrowInputHandle;
    private FeaturesAclManager aclMgr;
    private bool canEdit = true;
    private bool canEnter = true;
    private bool firstTimeLoad = true;
    private int daysAlertPrint = -1;
    private int daysAlertDue = -1;
    private int daysAlertEscrow = -1;
    private const string noNeedExclusiveLockFields = "|SERVICE.X1|SERVICE.X2|SERVICE.X3|SERVICE.X4|SERVICE.X5|SERVICE.X6|SERVICE.X7|SERVICE.X8|SERVICE.X9|SERVICE.X10|SERVICE.X11|SERVICE.X109|SERVICE.X110|SERVICE.X111|SERVICE.X108";
    private bool calendarIsClicked;
    private string curDateValue = string.Empty;
    private DateTime firstPayDate = DateTime.MinValue;
    private EllieMae.Encompass.Forms.Label labelStatement;
    private EllieMae.Encompass.Forms.Label labelPayDue;
    private EllieMae.Encompass.Forms.Label labelLatePayDue;
    private EllieMae.Encompass.Forms.Label labelTax;
    private EllieMae.Encompass.Forms.Label labelHazard;
    private EllieMae.Encompass.Forms.Label labelMortgage;
    private EllieMae.Encompass.Forms.Label labelFlood;
    private EllieMae.Encompass.Forms.Label labelCity;
    private EllieMae.Encompass.Forms.Label labelOther1;
    private EllieMae.Encompass.Forms.Label labelOther2;
    private EllieMae.Encompass.Forms.Label labelOther3;
    private EllieMae.Encompass.Forms.Label labelUSDAFee;
    private FieldLock indexRateLock;

    public SERVICINGDETAILInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public SERVICINGDETAILInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
      AlertConfig[] alertConfigList = Session.AlertManager.GetAlertConfigList();
      for (int index = 0; index < alertConfigList.Length; ++index)
      {
        if (this.daysAlertPrint == -1 && alertConfigList[index].AlertID == 7)
          this.daysAlertPrint = alertConfigList[index].DaysBefore;
        if (this.daysAlertDue == -1 && alertConfigList[index].AlertID == 6)
          this.daysAlertDue = alertConfigList[index].DaysBefore;
        if (this.daysAlertEscrow == -1 && alertConfigList[index].AlertID == 3)
          this.daysAlertEscrow = alertConfigList[index].DaysBefore;
        if (this.daysAlertPrint != -1 && this.daysAlertDue != -1 && this.daysAlertEscrow != -1)
          break;
      }
      if (this.daysAlertPrint < 0)
        this.daysAlertPrint = 0;
      if (this.daysAlertDue < 0)
        this.daysAlertDue = 0;
      if (this.daysAlertEscrow < 0)
        this.daysAlertEscrow = 0;
      this.populateAlerts();
      this.firstTimeLoad = false;
      this.pmtEscrowInputHandle = new PaymentEscrowInputHandler((IHtmlInput) this.loan, this.currentForm, (InputHandlerBase) this, this.session);
    }

    internal override void CreateControls()
    {
      try
      {
        this.aclMgr = (FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features);
        if (!Session.UserInfo.IsSuperAdministrator() && !this.aclMgr.GetUserApplicationRight(AclFeature.ToolsTab_IS_EditDeleteTransaction))
          this.canEdit = false;
        if (!Session.UserInfo.IsSuperAdministrator() && !this.aclMgr.GetUserApplicationRight(AclFeature.ToolsTab_IS_EnterTransaction))
          this.canEnter = false;
        if (this.loan == null)
          return;
        this.paySnapshot = this.loan.GetPaymentScheduleSnapshot();
      }
      catch (Exception ex)
      {
      }
    }

    public SERVICINGDETAILInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
      this.canEdit = false;
    }

    public SERVICINGDETAILInputHandler(
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
      ControlState controlState1 = this.getControlState(ctrl, id, ControlState.Enabled);
      if (ctrl is FieldLock)
        controlState1 = ControlState.Default;
      ControlState controlState2;
      switch (id)
      {
        case "pastduecalculation":
          controlState2 = !this.canEdit && !this.canEnter || this.paySnapshot == null ? ControlState.Disabled : ControlState.Enabled;
          break;
        case "viewlastpayment":
          controlState2 = !this.canEdit && !this.canEnter || this.GetFieldValue("SERVICE.X30") == string.Empty ? ControlState.Disabled : ControlState.Enabled;
          EllieMae.Encompass.Forms.Button button = (EllieMae.Encompass.Forms.Button) ctrl;
          if (!this.canEdit)
          {
            if (this.GetFieldValue("SERVICE.X30") != string.Empty)
              controlState2 = ControlState.Enabled;
            button.Text = "View Details";
            break;
          }
          button.Text = "View / Edit Details";
          break;
        case "viewsummaryhistory":
          controlState2 = !(this.GetFieldValue("SERVICE.X30") != string.Empty) ? ControlState.Disabled : ControlState.Enabled;
          break;
        case "turnEscrow":
          controlState2 = !(this.loan.GetField("SERVICE.X36") != string.Empty) ? ControlState.Disabled : ControlState.Enabled;
          this.pmtEscrowInputHandle.TurnOFFPaymentEscrowDialog();
          break;
        default:
          if ((this.GetFieldValue("SERVICE.X13") == string.Empty || this.GetFieldValue("SERVICE.X13") == "//") && this.GetFieldValue("SERVICE.X30") == string.Empty)
          {
            controlState2 = ControlState.Disabled;
            if (id == "SERVICE.X9")
              this.SetControlState("CalendarX9", false);
            if (id == "SERVICE.X10")
              this.SetControlState("CalendarX10", false);
            if (id == "SERVICE.X109")
              this.SetControlState("CalendarX13", false);
            if (id == "SERVICE.X140")
              this.SetControlState("ContactButton1", false);
            if (id == "SERVICE.X141")
              this.SetControlState("ContactButton2", false);
            if (id == "SERVICE.X142")
              this.SetControlState("ContactButton3", false);
            if (id == "SERVICE.X143")
              this.SetControlState("ContactButton4", false);
          }
          else if (id == "hudsetup" || this.loan.IsLocked(id) || id == "SERVICE.X2" || id == "SERVICE.X3" || id == "SERVICE.X4" || id == "SERVICE.X5" || id == "SERVICE.X6" || id == "SERVICE.X7" || id == "SERVICE.X8" || id == "SERVICE.X9" || id == "SERVICE.X10" || id == "SERVICE.X11" || id == "SERVICE.X23" || id == "SERVICE.X109" || id == "SERVICE.X110" || id == "VEND.X178" || id == "SERVICE.X111" || id == "SERVICE.X108" || id == "SERVICE.X140" || id == "SERVICE.X141" || id == "SERVICE.X142" || id == "SERVICE.X143")
          {
            controlState2 = ControlState.Enabled;
            if (id == "SERVICE.X9")
              this.SetControlState("CalendarX9", true);
            if (id == "SERVICE.X10")
              this.SetControlState("CalendarX10", true);
            if (id == "SERVICE.X109")
              this.SetControlState("CalendarX13", true);
            if (id == "SERVICE.X140")
              this.SetControlState("ContactButton1", true);
            if (id == "SERVICE.X141")
              this.SetControlState("ContactButton2", true);
            if (id == "SERVICE.X142")
              this.SetControlState("ContactButton3", true);
            if (id == "SERVICE.X142")
              this.SetControlState("ContactButton4", true);
          }
          else
            controlState2 = ControlState.Default;
          if (id == "SERVICE.X110")
            this.SetRolodexCtrlStatus("Rolodex1", controlState2 == ControlState.Enabled);
          if (id == "VEND.X178")
          {
            this.SetRolodexCtrlStatus("Rolodex2", controlState2 == ControlState.Enabled);
            break;
          }
          break;
      }
      return controlState2;
    }

    private void SetRolodexCtrlStatus(string roloID, bool enable)
    {
      if (!(this.currentForm.FindControl(roloID) is Rolodex control))
        return;
      control.Enabled = enable;
    }

    internal override void FlipLockField(FieldLock fieldLock)
    {
      if ((fieldLock.ControlToLock as FieldControl).Field.FieldID != "SERVICE.X1")
      {
        using (CursorActivator.Wait())
        {
          if (!Session.LoanDataMgr.LockLoanWithExclusiveA())
            return;
        }
      }
      base.FlipLockField(fieldLock);
    }

    public override void DisplayDatePicker(EllieMae.Encompass.Forms.Calendar ctrl)
    {
      if (ctrl.DateField.FieldID != "SERVICE.X9" && ctrl.DateField.FieldID != "SERVICE.X10" && ctrl.DateField.FieldID != "SERVICE.X109")
      {
        using (CursorActivator.Wait())
        {
          if (!Session.LoanDataMgr.LockLoanWithExclusiveA())
            return;
        }
      }
      base.DisplayDatePicker(ctrl);
    }

    public override void onCalendarPopupClosed(object sender, FormClosedEventArgs e)
    {
      this.calendarIsClicked = true;
      base.onCalendarPopupClosed(sender, e);
      this.calendarIsClicked = false;
      this.RefreshContents();
    }

    public override bool executeDateSelectedEvent(RuntimeControl control, DateTime selectedDate)
    {
      bool flag = base.executeDateSelectedEvent(control, selectedDate);
      if (control is EllieMae.Encompass.Forms.Calendar)
      {
        EllieMae.Encompass.Forms.Calendar calendar = (EllieMae.Encompass.Forms.Calendar) control;
        if (calendar.DateField.FieldID == "SERVICE.X13" && !this.isDateCorrect(this.curDateValue, this.GetFieldValue("SERVICE.X14")))
        {
          int num = (int) Utils.Dialog((IWin32Window) Session.MainForm, "The Statement Due Date cannot be greater than Payment Due Date.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return flag;
        }
        if (calendar.DateField.FieldID == "SERVICE.X14")
        {
          if (!this.isDateCorrect(this.GetFieldValue("SERVICE.X13"), this.curDateValue))
          {
            int num = (int) Utils.Dialog((IWin32Window) Session.MainForm, "The Statement Due Date cannot be greater than Payment Due Date.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return flag;
          }
          if (!this.isDateCorrect(this.curDateValue, this.GetFieldValue("SERVICE.X15")))
          {
            int num = (int) Utils.Dialog((IWin32Window) Session.MainForm, "The Late Payment Date cannot be earlier than the Payment Due Date.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return flag;
          }
        }
        if (calendar.DateField.FieldID == "SERVICE.X15" && !this.isDateCorrect(this.GetFieldValue("SERVICE.X14"), this.curDateValue))
        {
          int num = (int) Utils.Dialog((IWin32Window) Session.MainForm, "The Late Payment Date cannot be earlier than the Payment Due Date.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return flag;
        }
      }
      return flag;
    }

    internal override void UpdateFieldValue(string id, string val)
    {
      if ("|SERVICE.X1|SERVICE.X2|SERVICE.X3|SERVICE.X4|SERVICE.X5|SERVICE.X6|SERVICE.X7|SERVICE.X8|SERVICE.X9|SERVICE.X10|SERVICE.X11|SERVICE.X109|SERVICE.X110|SERVICE.X111|SERVICE.X108".IndexOf("|" + id + "|") == -1)
      {
        using (CursorActivator.Wait())
        {
          if (!Session.LoanDataMgr.LockLoanWithExclusiveA())
            return;
        }
      }
      this.curDateValue = val;
      if (id == "SERVICE.X13" && !this.isDateCorrect(val, this.GetFieldValue("SERVICE.X14")))
      {
        if (this.calendarIsClicked)
          return;
        int num = (int) Utils.Dialog((IWin32Window) Session.MainForm, "The Statement Due Date cannot be greater than Payment Due Date.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (id == "SERVICE.X14")
        {
          if (!this.isDateCorrect(this.GetFieldValue("SERVICE.X13"), val))
          {
            if (this.calendarIsClicked)
              return;
            int num = (int) Utils.Dialog((IWin32Window) Session.MainForm, "The Statement Due Date cannot be greater than Payment Due Date.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return;
          }
          if (!this.isDateCorrect(val, this.GetFieldValue("SERVICE.X15")))
          {
            if (this.calendarIsClicked)
              return;
            int num = (int) Utils.Dialog((IWin32Window) Session.MainForm, "The Late Payment Date cannot be earlier than the Payment Due Date.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return;
          }
        }
        if (id == "SERVICE.X15" && !this.isDateCorrect(this.GetFieldValue("SERVICE.X14"), val))
        {
          if (this.calendarIsClicked)
            return;
          int num = (int) Utils.Dialog((IWin32Window) Session.MainForm, "The Late Payment Date cannot be earlier than the Payment Due Date.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          base.UpdateFieldValue(id, val);
          int num = Utils.ParseInt((object) id.Replace("SERVICE.X", ""));
          if (num >= 13 && num <= 15 || num >= 59 && num <= 73 || num == 9 || num == 10)
            this.populateAlerts();
          if (num != 9 && num != 10)
            return;
          Session.Application.GetService<ILoanEditor>().RefreshLogPanel();
        }
      }
    }

    private bool isDateCorrect(string startingDate, string endDate)
    {
      return !(Utils.ParseDate((object) startingDate).Date > Utils.ParseDate((object) endDate).Date);
    }

    public override void RefreshContents()
    {
      if (this.loan != null)
        this.paySnapshot = this.loan.GetPaymentScheduleSnapshot();
      if (!this.firstTimeLoad)
        this.populateAlerts();
      base.RefreshContents();
    }

    private void populateAlerts()
    {
      if (this.loan == null || this.FormIsForTemplate)
        return;
      if (this.labelStatement == null)
        this.labelStatement = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("LabelStatementDate");
      if (this.labelPayDue == null)
        this.labelPayDue = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("LabelPaymentDueDate");
      if (this.labelLatePayDue == null)
        this.labelLatePayDue = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("LabelLatePaymentDate");
      if (this.labelTax == null)
        this.labelTax = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("LabelTax");
      if (this.labelHazard == null)
        this.labelHazard = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("LabelHazard");
      if (this.labelMortgage == null)
        this.labelMortgage = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("LabelMortgage");
      if (this.labelFlood == null)
        this.labelFlood = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("LabelFlood");
      if (this.labelCity == null)
        this.labelCity = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("LabelCity");
      if (this.labelOther1 == null)
        this.labelOther1 = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("LabelOther1");
      if (this.labelOther2 == null)
        this.labelOther2 = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("LabelOther2");
      if (this.labelOther3 == null)
        this.labelOther3 = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("LabelOther3");
      if (this.labelUSDAFee == null)
        this.labelUSDAFee = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("LabelUSDAFee");
      if (this.indexRateLock == null)
        this.indexRateLock = (FieldLock) this.currentForm.FindControl("FieldLockIndexRate");
      if (this.paySnapshot == null || this.paySnapshot.IsARMLoan)
        this.indexRateLock.Visible = true;
      else
        this.indexRateLock.Visible = false;
      this.labelStatement.ForeColor = Color.Black;
      this.labelPayDue.ForeColor = Color.Black;
      this.labelLatePayDue.ForeColor = Color.Black;
      this.labelTax.ForeColor = Color.Black;
      this.labelHazard.ForeColor = Color.Black;
      this.labelMortgage.ForeColor = Color.Black;
      this.labelFlood.ForeColor = Color.Black;
      this.labelCity.ForeColor = Color.Black;
      this.labelOther1.ForeColor = Color.Black;
      this.labelOther2.ForeColor = Color.Black;
      this.labelOther3.ForeColor = Color.Black;
      this.labelUSDAFee.ForeColor = Color.Black;
      string empty1 = string.Empty;
      double num = Utils.ParseDouble((object) this.loan.GetField("SERVICE.X57"));
      if (this.paySnapshot == null)
        return;
      DateTime dateTime1 = DateTime.Today;
      DateTime dateTime2 = dateTime1.AddDays((double) this.daysAlertPrint);
      if (num > 0.0 && this.GetFieldValue("SERVICE.X13") != string.Empty && this.GetFieldValue("SERVICE.X14") != this.GetFieldValue("SERVICE.X10"))
      {
        dateTime1 = Utils.ParseDate((object) this.GetFieldValue("SERVICE.X13"));
        if (dateTime1.Date <= dateTime2.Date)
        {
          empty1 += "Statement Due! ";
          this.labelStatement.ForeColor = AppColors.AlertRed;
        }
      }
      DateTime iswCutoffDate = this.loan.Calculator.GetISWCutoffDate();
      if (num > 0.0 && this.GetFieldValue("SERVICE.X14") != string.Empty)
      {
        if (this.GetFieldValue("2626") != "Correspondent")
        {
          dateTime1 = Utils.ParseDate((object) this.GetFieldValue("SERVICE.X14"));
          if (dateTime1.Date <= iswCutoffDate)
            goto label_40;
        }
        if (this.GetFieldValue("2626") == "Correspondent")
        {
          dateTime1 = Utils.ParseDate((object) this.GetFieldValue("SERVICE.X14"));
          if (!(dateTime1.Date < iswCutoffDate))
            goto label_41;
        }
        else
          goto label_41;
label_40:
        this.labelPayDue.ForeColor = AppColors.AlertRed;
      }
label_41:
      DateTime dateTime3 = iswCutoffDate.AddDays((double) this.daysAlertDue);
      if (num > 0.0 && this.GetFieldValue("SERVICE.X15") != string.Empty)
      {
        dateTime1 = Utils.ParseDate((object) this.GetFieldValue("SERVICE.X15"));
        if (dateTime1.Date <= dateTime3.Date)
        {
          empty1 += "Payment Past Due! ";
          this.labelLatePayDue.ForeColor = AppColors.AlertRed;
        }
      }
      bool flag = false;
      dateTime1 = DateTime.Today;
      DateTime dateTime4 = dateTime1.AddDays((double) this.daysAlertEscrow);
      string empty2 = string.Empty;
      for (int index = 59; index <= 75; index += 2)
      {
        string id = "SERVICE.X" + (index < 75 ? index.ToString() : "106");
        if (this.GetFieldValue(id) != string.Empty && this.GetFieldValue(id) != "//" && Utils.ParseDate((object) this.GetFieldValue(id)) <= dateTime4)
        {
          flag = true;
          switch (index)
          {
            case 59:
              this.labelTax.ForeColor = AppColors.AlertRed;
              continue;
            case 61:
              this.labelHazard.ForeColor = AppColors.AlertRed;
              continue;
            case 63:
              this.labelMortgage.ForeColor = AppColors.AlertRed;
              continue;
            case 65:
              this.labelFlood.ForeColor = AppColors.AlertRed;
              continue;
            case 67:
              this.labelCity.ForeColor = AppColors.AlertRed;
              continue;
            case 69:
              this.labelOther1.ForeColor = AppColors.AlertRed;
              continue;
            case 71:
              this.labelOther2.ForeColor = AppColors.AlertRed;
              continue;
            case 73:
              this.labelOther3.ForeColor = AppColors.AlertRed;
              continue;
            case 75:
              this.labelUSDAFee.ForeColor = AppColors.AlertRed;
              continue;
            default:
              continue;
          }
        }
      }
      if (!flag)
        return;
      string str = empty1 + "Disbursement Due!";
    }

    public override void ExecAction(string action)
    {
      using (CursorActivator.Wait())
      {
        if (!Session.LoanDataMgr.LockLoanWithExclusiveA(action != "viewlastpayment"))
          return;
      }
      if (action != "hudsetup")
        base.ExecAction(action);
      if (!(action == "hudsetup"))
        return;
      using (HUD1ESSetupDialog huD1EsSetupDialog = new HUD1ESSetupDialog(this.paySnapshot))
      {
        int num = (int) huD1EsSetupDialog.ShowDialog((IWin32Window) null);
      }
      this.SetFieldFocus("l_610");
    }

    public override bool onclick(mshtml.IHTMLEventObj pEvtObj)
    {
      if (this.loan.GetField("SERVICE.X36") != "" && pEvtObj.srcElement.id != null && pEvtObj.srcElement.id.Equals("EscrowInfo"))
      {
        if (this.pmtEscrowInputHandle.TurnOnPaymentEscrowDialog(pEvtObj))
          this.RefreshAllControls(true);
      }
      else
      {
        this.pmtEscrowInputHandle.TurnOFFPaymentEscrowDialog();
        base.onclick(pEvtObj);
      }
      return true;
    }
  }
}
