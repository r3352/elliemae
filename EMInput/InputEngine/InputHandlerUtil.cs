// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.InputHandlerUtil
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.BusinessRules;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.InputEngine.LockRequest;
using EllieMae.EMLite.LoanUtils.RateLocks;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class InputHandlerUtil
  {
    private Sessions.Session session;
    private IHtmlInput inputData;
    private bool applyBusinessRule;
    private StatusReport statusReport;

    public InputHandlerUtil(Sessions.Session session) => this.session = session;

    public string SendLockRequest(bool alwaysKeepLoanOpen)
    {
      return this.SendLockRequest(alwaysKeepLoanOpen, false);
    }

    public string SendLockRequest(
      bool alwaysKeepLoanOpen,
      bool suppressAutoLock,
      bool suppressLockDeskHour = false,
      bool isLockExtension = false,
      bool isCurrent = true,
      RateLockAction rateLockAction = RateLockAction.UnKnown)
    {
      bool relock = false;
      if (LockUtils.IsRelock(this.session.LoanData.GetField("3841")))
        relock = true;
      return this.sendLockRequest(this.session.LoanData.GetField("2088"), this.session.LoanData.GetField("2090"), this.session.LoanData.GetField("2091"), this.session.LoanData.GetField("2089"), this.session.LoanData.GetField("3039"), this.session.LoanData.GetField("4187"), this.session.LoanData.GetField("4120"), this.session.LoanData.GetField("LOCKRATE.RATESTATUS"), alwaysKeepLoanOpen, false, relock, suppressAutoLock, isGetPricing: true, suppressLockDeskHour: suppressLockDeskHour, isLockExtension: isLockExtension, isCurrent: isCurrent, rateLockAction: rateLockAction);
    }

    private string sendLockRequest(
      string field2088,
      string field2090,
      string field2091,
      string field2089,
      string field3039,
      string field4186,
      string field4120,
      string lockStatus,
      bool alwaysKeepLoanOpen,
      bool historyFromCurrentLock,
      bool relock,
      bool suppressAutoLock,
      bool deliveryTypeValidation = false,
      bool isGetPricing = false,
      bool suppressLockDeskHour = false,
      bool isLockExtension = false,
      bool isCurrent = true,
      RateLockAction rateLockAction = RateLockAction.UnKnown)
    {
      if (field2091 == string.Empty || field2091 == "//" || field2089 == string.Empty || field2089 == "//")
      {
        int num = (int) Utils.Dialog((IWin32Window) this.session.Application, "Lock request data cannot be blank. Please enter data values to the Lock Date and the Lock Expiration Date fields.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return "";
      }
      if (deliveryTypeValidation && field4186 == string.Empty)
      {
        int num = (int) Utils.Dialog((IWin32Window) this.session.Application, "Commitment Type is required.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return "";
      }
      if (!relock && this.session.LoanDataMgr.ExcludeWithdrawnLoan())
      {
        int num = (int) Utils.Dialog((IWin32Window) this.session.Application, "Loans that have been \"Withdrawn\" are no longer eligible to be locked. Lock request has not been accepted.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return "";
      }
      if (this.session.LoanDataMgr == null)
        return "";
      if (!this.session.LoanDataMgr.Writable)
      {
        int num = (int) Utils.Dialog((IWin32Window) this.session.Application, "You opened this loan in read-only mode. You cannot create a lock request.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return "";
      }
      bool flag1 = lockStatus == "Cancelled" || lockStatus == "Expired";
      bool policySetting1 = (bool) this.session.StartupInfo.PolicySettings[(object) "Policies.EnforceOnlyWhenNoCurrentLock"];
      bool flag2 = false;
      if (policySetting1 && this.session.LoanData.GetLogList().GetCurrentLockRequest() != null || relock && !flag1 || LockUtils.IsWorstCaseHistoricalPricingTrans(this.session.LoanData.GetField("Optimal.History")))
        flag2 = true;
      if (!flag2)
      {
        int policySetting2 = (int) this.session.StartupInfo.PolicySettings[(object) "Policies.Pricing_ElapsedTime"];
        if (field3039 != "//" && Utils.IsDate((object) field3039) && policySetting2 > 0)
        {
          DateTime date = Utils.ParseDate((object) field3039);
          if (this.session.ConfigurationManager.IsProductAndPricingExpired(date))
          {
            int num = (int) Utils.Dialog((IWin32Window) this.session.Application, "Lock requests must be placed within " + (object) policySetting2 + " minutes of the last product and pricing search which occurred at " + date.ToString("hh:mm:ss tt") + " on " + date.ToString("MM/dd/yyyy") + ".  Use the Get Pricing button to retrieve updated product and pricing information prior to requesting a lock on this loan.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return "";
          }
        }
      }
      bool flag3 = lockStatus == "Cancelled" || lockStatus == "Expired";
      if (flag3)
      {
        int num1 = 10;
        if (this.session.StartupInfo.PolicySettings[(object) "Policies.RelockAllowTotalCap"] != null && (bool) this.session.StartupInfo.PolicySettings[(object) "Policies.RelockAllowTotalCap"])
        {
          int num2 = Utils.ParseInt(this.session.StartupInfo.PolicySettings[(object) "Policies.RelockAllowTotalCapTimes"] != null ? (object) this.session.StartupInfo.PolicySettings[(object) "Policies.RelockAllowTotalCapTimes"].ToString() : (object) "");
          num1 = num2 < num1 ? num2 : num1;
        }
        if (LockUtils.GetReLockSequenceNumberForInactiveLock(this.session.LoanData) == num1)
        {
          int num3 = (int) Utils.Dialog((IWin32Window) this.session.Application, "The Re-Lock Limit for Inactive Locks has been exceeded. The Re-Lock Request cannot be processed.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return "";
        }
      }
      string str = (!relock || flag3 ? (!isLockExtension || isCurrent ? (LogRecordBase) this.session.LoanDataMgr.CreateRateLockRequest(historyFromCurrentLock, suppressAutoLock, suppressLockDeskHour, rateLockAction) : (LogRecordBase) this.session.LoanDataMgr.CreateHistoricalExtendedRateLockRequest(Session.UserInfo, rateLockAction)) : (LogRecordBase) this.session.LoanDataMgr.CreateRateRelockRequest(isGetPricing)).Guid.ToString();
      if (this.applyBusinessRule)
      {
        TriggerImplDef def = this.session.LoanDataMgr.ApplyLoanTemplateTrigger(TriggerConditionType.LockRequested);
        if (!string.IsNullOrEmpty(str) && def != null)
        {
          this.ShowApplyLoanTemplateProgress();
          if (this.session.LoanDataMgr != null)
            this.session.LoanDataMgr.ApplyLoanTemplate(def);
          this.CloseProgress();
        }
      }
      if (alwaysKeepLoanOpen)
      {
        if (!this.session.Application.GetService<ILoanConsole>().SaveLoan())
          return "";
      }
      else
      {
        using (LockRequestProcessDialog requestProcessDialog = new LockRequestProcessDialog())
        {
          if (requestProcessDialog.ShowDialog((IWin32Window) this.session.Application) == DialogResult.OK)
          {
            ILoanConsole service = this.session.Application.GetService<ILoanConsole>();
            if (!service.SaveLoan())
              return "";
            if (requestProcessDialog.CloseLoanFile)
              service.CloseLoan(false);
          }
        }
      }
      return str;
    }

    internal string sendLockRequest(
      IHtmlInput inputData,
      bool historyFromCurrentLock,
      bool relock,
      bool suppressAutoLock)
    {
      return this.sendLockRequest(inputData, historyFromCurrentLock, relock, suppressAutoLock, false);
    }

    internal string sendLockRequest(
      IHtmlInput inputData,
      bool historyFromCurrentLock,
      bool relock,
      bool suppressAutoLock,
      bool applyBusinessRule)
    {
      this.inputData = inputData;
      this.applyBusinessRule = applyBusinessRule;
      if (this.GetFieldValue("3841") == string.Empty)
      {
        int num = (int) Utils.Dialog((IWin32Window) this.session.Application, "Request type cannot be blank. Please select a lock request type.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return string.Empty;
      }
      bool deliveryTypeValidation = this.GetFieldValue("3966") == "Y";
      if (this.session != null && this.session.CompanyInfo != null && this.session.CompanyInfo.ClientID != null && LockDeskHoursUtils.IsQAONRPSettingEnabled(this.session.CompanyInfo.ClientID))
      {
        DateTime result;
        DateTime.TryParse(this.GetFieldValue("4069") + " " + this.GetFieldValue("4060"), out result);
        if (result == DateTime.MinValue)
          result = DateTime.Now;
        LockRequestQAOverrideServerTimeDialog serverTimeDialog = new LockRequestQAOverrideServerTimeDialog(result);
        int num = (int) serverTimeDialog.ShowDialog();
        EncompassLockDeskHoursHelper.QAOverrideServerEasternTime = new DateTime?(serverTimeDialog.overrideTime);
      }
      return this.sendLockRequest(this.GetFieldValue("2088"), this.GetFieldValue("2090"), this.GetFieldValue("2091"), this.GetFieldValue("2089"), this.GetFieldValue("3039"), this.GetFieldValue("4187"), this.GetFieldValue("4120"), this.session.LoanData.GetField("LOCKRATE.RATESTATUS"), false, historyFromCurrentLock, relock, suppressAutoLock, deliveryTypeValidation);
    }

    protected virtual string GetFieldValue(string id)
    {
      if (this.inputData == null)
        return string.Empty;
      string field = this.inputData.GetField(id);
      if (id == "333" && this.inputData.GetField("SYS.X8") == "Y" && field != string.Empty)
        field = Utils.ArithmeticRounding(Utils.ParseDouble((object) field), 2).ToString("N2");
      return field;
    }

    public void ShowApplyLoanTemplateProgress()
    {
      new Thread(new ParameterizedThreadStart(this.threadStart))
      {
        IsBackground = true
      }.Start((object) "Please wait. Applying loan template is in progress.");
    }

    private void threadStart(object message)
    {
      this.statusReport = new StatusReport(string.Concat(message));
      this.statusReport.Text = "Applying loan template";
      Application.Run((Form) this.statusReport);
    }

    public void CloseProgress()
    {
      if (this.statusReport == null)
        return;
      if (this.statusReport.InvokeRequired)
      {
        this.statusReport.Invoke((Delegate) new MethodInvoker(this.CloseProgress));
      }
      else
      {
        try
        {
          this.statusReport.Close();
        }
        catch
        {
        }
      }
    }
  }
}
