// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.TransactionLog
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Version;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects
{
  internal class TransactionLog
  {
    private readonly LoanData _loanData;
    private readonly ClientContext _clientContext;
    private readonly string _closedLoanMilestoneId;
    private readonly TransactionLog.ReasonCode _currentReasonCode;
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

    public TransactionLog(
      LoanData loanData,
      UserInfo currentUser,
      LoanIdentity loanIdentity,
      TransactionLog.ReasonCode reasonCode)
    {
      this._loanData = loanData;
      this._clientContext = ClientContext.GetCurrent();
      this._closedLoanMilestoneId = (string) this._clientContext.Settings.GetServerSetting("License.ClosedLoanMilestone");
      this._currentReasonCode = reasonCode;
      this.GenerateTransactionLog(currentUser, loanIdentity);
    }

    private void GenerateTransactionLog(UserInfo currentUser, LoanIdentity loanIdentity)
    {
      this.URLSignature = "CLOSEDLOAN";
      this.TransactionID = Guid.NewGuid().ToString();
      this.LOSName = "Encompass";
      this.LOSVersion = VersionInformation.CurrentVersion.Version.FullVersion;
      this.ClientID = this._clientContext.ClientID ?? "";
      this.UserName = currentUser?.Userid ?? "";
      this.LoanFileName = loanIdentity?.LoanName;
      if (this._loanData == null)
        return;
      this.LoanGUID = this._loanData.GetSimpleField("GUID");
      this.LoanNumber = this._loanData.GetSimpleField("364");
      this.BorrowerName = this._loanData.GetSimpleField("36") + " " + this._loanData.GetSimpleField("37");
      this.PropertyAddress = this._loanData.GetSimpleField("11") + ", " + this._loanData.GetSimpleField("12") + ", " + this._loanData.GetSimpleField("14") + "  " + this._loanData.GetSimpleField("15");
      this.LoanAmount = this._loanData.GetSimpleField("2");
      this.NoteRate = this._loanData.GetSimpleField("3");
      this.MortgageType = this._loanData.GetSimpleField("1172");
      this.LoanPurpose = this._loanData.GetSimpleField("19");
      this.AmortizationType = this._loanData.GetSimpleField("608");
      this.AppraisedAmount = this._loanData.GetSimpleField("356");
      this.LienPosition = this._loanData.GetSimpleField("420");
      this.Miscellaneous = this.BuildMiscellaneousData();
      if (this.MortgageType == "FarmersHomeAdministration")
        this.MortgageType = "FmHA";
      switch (this.LoanPurpose)
      {
        case "ConstructionToPermanent":
          this.LoanPurpose = "Construction-Permanent";
          break;
        case "ConstructionOnly":
          this.LoanPurpose = "Construction";
          break;
      }
      switch (this.AmortizationType)
      {
        case "AdjustableRate":
          this.AmortizationType = "ARM";
          break;
        case "GraduatedPaymentMortgage":
          this.AmortizationType = "GPM";
          break;
        case "OtherAmortizationType":
          this.AmortizationType = "Other";
          break;
      }
    }

    public string URLSignature { get; private set; }

    public string TransactionID { get; private set; }

    public string LOSName { get; private set; }

    public string LOSVersion { get; private set; }

    public string ClientID { get; private set; }

    public string UserName { get; private set; }

    public string LoanFileName { get; private set; }

    public string LoanGUID { get; private set; }

    public string LoanNumber { get; private set; }

    public string BorrowerName { get; private set; }

    public string PropertyAddress { get; private set; }

    public string LoanAmount { get; private set; }

    public string NoteRate { get; private set; }

    public string MortgageType { get; private set; }

    public string LoanPurpose { get; private set; }

    public string AmortizationType { get; private set; }

    public string AppraisedAmount { get; private set; }

    public string LienPosition { get; private set; }

    public string Miscellaneous { get; private set; }

    private string BuildMiscellaneousData()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(this.GetActivationCodeString(this._currentReasonCode));
      MilestoneLog triggerMilestoneLog = this.GetBillingTriggerMilestoneLog();
      stringBuilder.Append(this.PadText(triggerMilestoneLog?.Stage ?? "", 30));
      if (triggerMilestoneLog != null && triggerMilestoneLog.Done)
        stringBuilder.Append(this.PadText(triggerMilestoneLog.Date.ToString("MM/dd/yyyy"), 10));
      else
        stringBuilder.Append(this.PadText("", 10));
      DateTime closingDocsOrderDate = TransactionLog.GetClosingDocsOrderDate(this._loanData);
      if (closingDocsOrderDate != DateTime.MinValue)
        stringBuilder.Append(this.PadText(closingDocsOrderDate.ToString("MM/dd/yyyy"), 10));
      else
        stringBuilder.Append(this.PadText("", 10));
      DateTime date1 = Utils.ParseDate((object) this._loanData.GetField("2014"));
      if (date1 != DateTime.MinValue)
        stringBuilder.Append(this.PadText(date1.ToString("MM/dd/yyyy"), 10));
      else
        stringBuilder.Append(this.PadText("", 10));
      DateTime date2 = Utils.ParseDate((object) this._loanData.GetField("1996"));
      if (date2 != DateTime.MinValue)
        stringBuilder.Append(this.PadText(date2.ToString("MM/dd/yyyy"), 10));
      else
        stringBuilder.Append(this.PadText("", 10));
      DateTime date3 = Utils.ParseDate((object) this._loanData.GetField("748"));
      if (date3 != DateTime.MinValue)
        stringBuilder.Append(this.PadText(date3.ToString("MM/dd/yyyy"), 10));
      else
        stringBuilder.Append(this.PadText("", 10));
      stringBuilder.Append(this.PadText(this._loanData.GetLogList().GetLastCompletedMilestone().Stage, 12));
      stringBuilder.Append(this.GetChannelIndicator(this._loanData.GetSimpleField("2626")));
      stringBuilder.Append(this.GetActionTakenIndicator(this._loanData.GetSimpleField("1393")));
      DateTime date4 = Utils.ParseDate((object) this._loanData.GetField("749"));
      if (date4 != DateTime.MinValue)
        stringBuilder.Append(this.PadText(date4.ToString("MM/dd/yyyy"), 10));
      else
        stringBuilder.Append(this.PadText("", 10));
      DateTime date5 = Utils.ParseDate((object) this._loanData.GetField("3142"));
      if (date5 != DateTime.MinValue)
        stringBuilder.Append(this.PadText(date5.ToString("MM/dd/yyyy"), 10));
      else
        stringBuilder.Append(this.PadText("", 10));
      DateTime date6 = Utils.ParseDate((object) this._loanData.GetField("3260"));
      if (date6 != DateTime.MinValue)
        stringBuilder.Append(this.PadText(date6.ToString("MM/dd/yyyy"), 10));
      else
        stringBuilder.Append(this.PadText("", 10));
      stringBuilder.Append(this.PadText(this._loanData.GetField("BILLINGCATEGORY"), 30));
      return stringBuilder.ToString();
    }

    private string GetActivationCodeString(TransactionLog.ReasonCode reasonCode)
    {
      string activationCodeString = "";
      int num = (int) reasonCode;
      for (int index = 0; index < 10; ++index)
        activationCodeString += (num & 1 << index) == 0 ? "0" : "1";
      return activationCodeString;
    }

    private MilestoneLog GetBillingTriggerMilestoneLog()
    {
      return this._loanData.GetLogList().GetMilestoneByID(this._closedLoanMilestoneId);
    }

    private string PadText(string text, int length)
    {
      if (text.Length < length)
        return text + new string(' ', length - text.Length);
      return text.Length > length ? text.Substring(0, length) : text;
    }

    internal static TransactionLog.ReasonCode GetActivationReason(LoanData loanData)
    {
      if (loanData == null || loanData.IsAdverse())
        return TransactionLog.ReasonCode.None;
      TransactionLog.ReasonCode activationReason = TransactionLog.ReasonCode.None;
      if (TransactionLog.IsApplicationComplete(loanData))
        activationReason |= TransactionLog.ReasonCode.Application;
      if (TransactionLog.GetClosingDocsOrderDate(loanData) != DateTime.MinValue)
        activationReason |= TransactionLog.ReasonCode.DocsOrdered;
      if (Utils.IsDate((object) loanData.GetSimpleField("1996")))
        activationReason |= TransactionLog.ReasonCode.Funded;
      if (Utils.IsDate((object) loanData.GetSimpleField("748")))
        activationReason |= TransactionLog.ReasonCode.Closed;
      if (loanData.GetSimpleField("1393") == "Loan Originated")
        activationReason |= TransactionLog.ReasonCode.Originated;
      if (Utils.IsDate((object) loanData.GetSimpleField("3260")))
        activationReason |= TransactionLog.ReasonCode.BillingTrigger;
      return activationReason;
    }

    private static bool IsApplicationComplete(LoanData loanData)
    {
      if (loanData == null)
        return false;
      foreach (string applicationField in TransactionLog.ApplicationFields)
      {
        if (string.IsNullOrEmpty(loanData.GetSimpleField(applicationField)))
          return false;
      }
      return string.Compare(loanData.GetSimpleField("11"), "TBD", true) != 0;
    }

    private static DateTime GetClosingDocsOrderDate(LoanData loanData)
    {
      if (loanData != null)
      {
        foreach (DocumentLog documentLog in loanData.GetLogList().GetDocumentsByTitle("Document Preparation"))
        {
          if (documentLog.IsePASS)
            return documentLog.Date;
        }
      }
      return DateTime.MinValue;
    }

    private string GetChannelIndicator(string channel)
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

    private string GetActionTakenIndicator(string action)
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

    internal static EllieMae.EMLite.ClientServer.Classes.TransactionLog MapTransactionLog(
      TransactionLog log)
    {
      return new EllieMae.EMLite.ClientServer.Classes.TransactionLog()
      {
        URLSignature = log.URLSignature,
        TransactionID = log.TransactionID,
        LOSName = log.LOSName,
        LOSVersion = log.LOSVersion,
        ClientID = log.ClientID,
        UserName = log.UserName,
        LoanFileName = log.LoanFileName,
        LoanGUID = log.LoanGUID,
        LoanNumber = log.LoanNumber,
        BorrowerName = log.BorrowerName,
        PropertyAddress = log.PropertyAddress,
        LoanAmount = log.LoanAmount,
        NoteRate = log.NoteRate,
        MortgageType = log.MortgageType,
        LoanPurpose = log.LoanPurpose,
        AmortizationType = log.AmortizationType,
        AppraisedAmount = log.AppraisedAmount,
        LienPosition = log.LienPosition,
        Miscellaneous = log.Miscellaneous
      };
    }

    [Flags]
    internal enum ReasonCode
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
