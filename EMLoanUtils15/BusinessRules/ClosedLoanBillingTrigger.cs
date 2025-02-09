// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.BusinessRules.ClosedLoanBillingTrigger
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Customization;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.WebServices;
using System;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.BusinessRules
{
  public class ClosedLoanBillingTrigger : DelayedTrigger
  {
    private static readonly string[] ApplicationFields = new string[10]
    {
      "4000",
      "4002",
      "736",
      "11",
      "12",
      "14",
      "15",
      "1821",
      "1109",
      "65"
    };
    private SessionObjects sessionObjects;
    private ILoanConfigurationInfo loanConfig;
    private ClosedLoanBillingTrigger.ReasonCode currentReason;
    private DateTime closingDate = DateTime.MinValue;
    private string billingCategory = string.Empty;
    private bool currentLinkStatus;
    private string currentTotalLoanAmount = string.Empty;

    public ClosedLoanBillingTrigger(
      SessionObjects sessionObjects,
      ILoanConfigurationInfo loanConfig,
      ExecutionContext context)
      : base(context)
    {
      this.sessionObjects = sessionObjects;
      this.loanConfig = loanConfig;
      this.Reset();
    }

    public override bool SupportsDirectExecution => true;

    public override void Reset()
    {
      this.currentReason = this.getActivationReason();
      this.closingDate = Utils.ParseDate((object) this.Context.Loan.GetSimpleField("3260"));
      this.billingCategory = this.Context.Loan.GetSimpleField("BILLINGCATEGORY");
      this.currentLinkStatus = this.Context.Loan.LinkedData != null;
      this.currentTotalLoanAmount = this.Context.Loan.GetSimpleField("2");
    }

    public override void ResetFieldValue(string fieldId, string val)
    {
    }

    public override void ResubscribeToFieldEvents()
    {
    }

    public override bool IsActivated()
    {
      if (this.getActivationReason() != this.currentReason || this.closingDate != Utils.ParseDate((object) this.Context.Loan.GetSimpleField("3260")) || this.billingCategory != this.Context.Loan.GetSimpleField("BILLINGCATEGORY") || this.currentTotalLoanAmount != this.Context.Loan.GetSimpleField("2") || this.currentLinkStatus && this.Context.Loan.LinkedData == null)
        return true;
      return !this.currentLinkStatus && this.Context.Loan.LinkedData != null;
    }

    public override bool Execute(LoanDataMgr dataMgr)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(ClosedLoanBillingTrigger.getActivationCodeString(this.getActivationReason()));
      MilestoneLog triggerMilestoneLog = this.getBillingTriggerMilestoneLog();
      stringBuilder.Append(this.padText(triggerMilestoneLog != null ? triggerMilestoneLog.Stage : "", 30));
      if (triggerMilestoneLog != null && triggerMilestoneLog.Done)
        stringBuilder.Append(this.padText(triggerMilestoneLog.Date.ToString("MM/dd/yyyy"), 10));
      else
        stringBuilder.Append(this.padText("", 10));
      DateTime closingDocsOrderDate = this.getClosingDocsOrderDate();
      if (closingDocsOrderDate != DateTime.MinValue)
        stringBuilder.Append(this.padText(closingDocsOrderDate.ToString("MM/dd/yyyy"), 10));
      else
        stringBuilder.Append(this.padText("", 10));
      DateTime date1 = Utils.ParseDate((object) this.Context.Loan.GetField("2014"));
      if (date1 != DateTime.MinValue)
        stringBuilder.Append(this.padText(date1.ToString("MM/dd/yyyy"), 10));
      else
        stringBuilder.Append(this.padText("", 10));
      DateTime date2 = Utils.ParseDate((object) this.Context.Loan.GetField("1996"));
      if (date2 != DateTime.MinValue)
        stringBuilder.Append(this.padText(date2.ToString("MM/dd/yyyy"), 10));
      else
        stringBuilder.Append(this.padText("", 10));
      DateTime date3 = Utils.ParseDate((object) this.Context.Loan.GetField("748"));
      if (date3 != DateTime.MinValue)
        stringBuilder.Append(this.padText(date3.ToString("MM/dd/yyyy"), 10));
      else
        stringBuilder.Append(this.padText("", 10));
      stringBuilder.Append(this.padText(this.Context.Loan.GetLogList().GetLastCompletedMilestone().Stage, 12));
      stringBuilder.Append(this.getChannelIndicator(this.Context.Loan.GetSimpleField("2626")));
      stringBuilder.Append(this.getActionTakenIndicator(this.Context.Loan.GetSimpleField("1393")));
      DateTime date4 = Utils.ParseDate((object) this.Context.Loan.GetField("749"));
      if (date4 != DateTime.MinValue)
        stringBuilder.Append(this.padText(date4.ToString("MM/dd/yyyy"), 10));
      else
        stringBuilder.Append(this.padText("", 10));
      DateTime date5 = Utils.ParseDate((object) this.Context.Loan.GetField("3142"));
      if (date5 != DateTime.MinValue)
        stringBuilder.Append(this.padText(date5.ToString("MM/dd/yyyy"), 10));
      else
        stringBuilder.Append(this.padText("", 10));
      DateTime date6 = Utils.ParseDate((object) this.Context.Loan.GetField("3260"));
      if (date6 != DateTime.MinValue)
        stringBuilder.Append(this.padText(date6.ToString("MM/dd/yyyy"), 10));
      else
        stringBuilder.Append(this.padText("", 10));
      string field = this.Context.Loan.GetField("BILLINGCATEGORY");
      stringBuilder.Append(this.padText(field, 30));
      if (this.Context.Loan.LinkedData != null && this.Context.Loan.GetField("4084") == "Y" && this.Context.Loan.GetField("19") == "ConstructionOnly" && this.Context.Loan.LinkGUID == this.Context.Loan.LinkedData.GUID)
        stringBuilder.Append(this.padText("Y", 1));
      else if (this.Context.Loan.LinkedData != null && this.Context.Loan.LinkGUID == this.Context.Loan.LinkedData.GUID)
        stringBuilder.Append(this.padText("N", 1));
      else
        stringBuilder.Append(this.padText("", 1));
      new EpassTransactionService(dataMgr).SendTransactionLog("CLOSEDLOAN", Guid.NewGuid().ToString(), stringBuilder.ToString());
      return true;
    }

    private string getActionTakenIndicator(string action)
    {
      switch (action)
      {
        case "":
          return "AL";
        case "Application approved but not accepted":
          return "AA";
        case "Application denied":
          return "AD";
        case "Application withdrawn":
          return "AW";
        case "File Closed for incompleteness":
          return "IN";
        case "Loan Originated":
          return "LO";
        case "Loan purchased by your institution":
          return "PU";
        case "Preapproval request approved but not accepted":
          return "PR";
        case "Preapproval request denied by financial institution":
          return "PD";
        default:
          return "OT";
      }
    }

    private string getChannelIndicator(string channel)
    {
      switch (channel)
      {
        case "Banked - Retail":
          return "BA";
        case "Banked - Wholesale":
          return "BW";
        case "Brokered":
          return "BR";
        case "Correspondent":
          return "CO";
        case "":
          return "  ";
        default:
          return "OT";
      }
    }

    private string padText(string text, int length)
    {
      if (text.Length < length)
        return text + new string(' ', length - text.Length);
      return text.Length > length ? text.Substring(0, length) : text;
    }

    private ClosedLoanBillingTrigger.ReasonCode getActivationReason()
    {
      if (this.Context.Loan.IsAdverse())
        return ClosedLoanBillingTrigger.ReasonCode.None;
      ClosedLoanBillingTrigger.ReasonCode activationReason = ClosedLoanBillingTrigger.ReasonCode.None;
      if (this.isApplicationComplete())
        activationReason |= ClosedLoanBillingTrigger.ReasonCode.Application;
      if (this.getClosingDocsOrderDate() != DateTime.MinValue)
        activationReason |= ClosedLoanBillingTrigger.ReasonCode.DocsOrdered;
      if (Utils.IsDate((object) this.Context.Loan.GetSimpleField("1996")))
        activationReason |= ClosedLoanBillingTrigger.ReasonCode.Funded;
      if (Utils.IsDate((object) this.Context.Loan.GetSimpleField("748")))
        activationReason |= ClosedLoanBillingTrigger.ReasonCode.Closed;
      if (this.Context.Loan.GetSimpleField("1393") == "Loan Originated")
        activationReason |= ClosedLoanBillingTrigger.ReasonCode.Originated;
      if (Utils.IsDate((object) this.Context.Loan.GetSimpleField("3260")))
        activationReason |= ClosedLoanBillingTrigger.ReasonCode.BillingTrigger;
      return activationReason;
    }

    private bool isApplicationComplete()
    {
      for (int index = 0; index < ClosedLoanBillingTrigger.ApplicationFields.Length; ++index)
      {
        if (this.Context.Loan.GetSimpleField(ClosedLoanBillingTrigger.ApplicationFields[index]) == "")
          return false;
      }
      return string.Compare(this.Context.Loan.GetSimpleField("11"), "TBD", true) != 0;
    }

    private DateTime getClosingDocsOrderDate()
    {
      foreach (DocumentLog documentLog in this.Context.Loan.GetLogList().GetDocumentsByTitle("Document Preparation"))
      {
        if (documentLog.IsePASS)
          return documentLog.Date;
      }
      return DateTime.MinValue;
    }

    private MilestoneLog getBillingTriggerMilestoneLog()
    {
      string billingMilestoneId = this.loanConfig.ClosedLoanBillingMilestoneID;
      LogList logList = this.Context.Loan.GetLogList();
      return logList.GetMilestoneByID(billingMilestoneId) ?? logList.GetMilestoneByID(billingMilestoneId);
    }

    private static string getActivationCodeString(ClosedLoanBillingTrigger.ReasonCode reasonCode)
    {
      string activationCodeString = "";
      int num = (int) reasonCode;
      for (int index = 0; index < 10; ++index)
        activationCodeString += (num & 1 << index) == 0 ? "0" : "1";
      return activationCodeString;
    }

    [Flags]
    private enum ReasonCode
    {
      None = 0,
      Application = 1,
      DocsOrdered = 2,
      Milestone = 4,
      Originated = 8,
      Funded = 16, // 0x00000010
      Closed = 32, // 0x00000020
      BillingTrigger = 64, // 0x00000040
    }
  }
}
