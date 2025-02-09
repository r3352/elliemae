// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.PipelineInfo
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  [Serializable]
  public class PipelineInfo
  {
    public static readonly string[] AlertRequiredFields = new string[3]
    {
      "Loan.ActionTaken",
      "Loan.CurrentMilestoneName",
      "Loan.NextMilestoneName"
    };
    private PipelineInfo.Borrower[] borrowers;
    private PipelineInfo.Alert[] alerts;
    private PipelineInfo.MilestoneInfo[] milestones;
    private PipelineInfo.AlertSummaryInfo alertSummary;
    private int numAlerts = -1;
    private string alertMsg;
    private Hashtable info = CollectionsUtil.CreateCaseInsensitiveHashtable();
    private LoanIdentity id;
    private LockInfo lockInfo;
    private List<LockInfo> locks;
    private Hashtable rights;
    private PipelineInfo.LoanAssociateInfo[] loanAssociates;
    private ArrayList offerAlerts = new ArrayList();
    private PipelineInfo.TradeInfo assignedTrade;
    private PipelineInfo.TradeInfo[] tradeAssignments;
    private PipelineInfo.TradeInfo[] eligibileTrades;
    private string[] rejectedInvestors;
    private static string minDate = DateTime.MinValue.ToString("M/d/yy");

    public PipelineInfo(
      Hashtable info,
      PipelineInfo.Borrower[] borrowers,
      PipelineInfo.Alert[] alerts,
      PipelineInfo.AlertSummaryInfo alertSummary,
      PipelineInfo.LoanAssociateInfo[] loanAssociates,
      LockInfo lockInfo,
      List<LockInfo> locks,
      Hashtable rights,
      PipelineInfo.MilestoneInfo[] milestones,
      PipelineInfo.TradeInfo assignedTrade,
      PipelineInfo.TradeInfo[] tradeAssignments,
      string[] rejectedInvestors)
      : this(info, borrowers, alerts, alertSummary, loanAssociates, lockInfo, rights, milestones, assignedTrade, tradeAssignments, rejectedInvestors)
    {
      this.locks = locks;
    }

    public PipelineInfo(
      Hashtable info,
      PipelineInfo.Borrower[] borrowers,
      PipelineInfo.Alert[] alerts,
      PipelineInfo.AlertSummaryInfo alertSummary,
      PipelineInfo.LoanAssociateInfo[] loanAssociates,
      LockInfo lockInfo,
      Hashtable rights,
      PipelineInfo.MilestoneInfo[] milestones,
      PipelineInfo.TradeInfo assignedTrade,
      PipelineInfo.TradeInfo[] tradeAssignments,
      string[] rejectedInvestors)
      : this(info, borrowers, alerts, loanAssociates, lockInfo, rights, milestones, assignedTrade, tradeAssignments, rejectedInvestors)
    {
      this.alertSummary = alertSummary;
    }

    public PipelineInfo(
      Hashtable info,
      PipelineInfo.Borrower[] borrowers,
      PipelineInfo.Alert[] alerts,
      PipelineInfo.LoanAssociateInfo[] loanAssociates,
      LockInfo lockInfo,
      Hashtable rights,
      PipelineInfo.MilestoneInfo[] milestones,
      PipelineInfo.TradeInfo assignedTrade,
      PipelineInfo.TradeInfo[] tradeAssignments,
      string[] rejectedInvestors)
      : this(info, borrowers, alerts, loanAssociates, lockInfo, rights, milestones)
    {
      this.assignedTrade = assignedTrade;
      this.tradeAssignments = tradeAssignments;
      this.rejectedInvestors = rejectedInvestors;
    }

    public PipelineInfo(
      Hashtable info,
      PipelineInfo.Borrower[] borrowers,
      PipelineInfo.Alert[] alerts,
      PipelineInfo.LoanAssociateInfo[] loanAssociates,
      LockInfo lockInfo,
      Hashtable rights,
      PipelineInfo.MilestoneInfo[] milestones)
      : this(info, borrowers, alerts, loanAssociates, lockInfo, rights)
    {
      this.milestones = milestones;
    }

    public PipelineInfo(
      Hashtable info,
      PipelineInfo.Borrower[] borrowers,
      PipelineInfo.Alert[] alerts,
      PipelineInfo.LoanAssociateInfo[] loanAssociates,
      LockInfo lockInfo,
      Hashtable rights)
      : this(info, borrowers, alerts, loanAssociates, lockInfo)
    {
      this.rights = rights;
    }

    public PipelineInfo(
      Hashtable info,
      PipelineInfo.Borrower[] borrowers,
      PipelineInfo.Alert[] alerts,
      PipelineInfo.LoanAssociateInfo[] loanAssociates,
      LockInfo lockInfo)
      : this(info, borrowers, alerts, loanAssociates)
    {
      this.lockInfo = lockInfo;
    }

    public PipelineInfo(
      Hashtable info,
      PipelineInfo.Borrower[] borrowers,
      PipelineInfo.Alert[] alerts,
      PipelineInfo.LoanAssociateInfo[] loanAssociates,
      PipelineInfo.MilestoneInfo[] milestones)
      : this(info, borrowers, alerts, loanAssociates)
    {
      this.milestones = milestones;
    }

    public PipelineInfo(
      Hashtable info,
      PipelineInfo.Borrower[] borrowers,
      PipelineInfo.Alert[] alerts,
      PipelineInfo.LoanAssociateInfo[] loanAssociates)
    {
      this.info = info;
      this.borrowers = borrowers;
      this.alerts = alerts;
      this.loanAssociates = loanAssociates;
    }

    public PipelineInfo.Borrower[] Borrowers => this.borrowers;

    public PipelineInfo.Alert[] Alerts => this.alerts;

    public PipelineInfo.AlertSummaryInfo AlertSummary => this.alertSummary;

    public PipelineInfo.TradeInfo AssignedTrade => this.assignedTrade;

    public PipelineInfo.TradeInfo[] TradeAssignments => this.tradeAssignments;

    public PipelineInfo.TradeInfo[] EligibleTrades
    {
      get => this.eligibileTrades;
      set => this.eligibileTrades = value;
    }

    public string[] RejectedInvestors => this.rejectedInvestors;

    public bool IsRejectedByInvestor(string investorName)
    {
      if (this.rejectedInvestors != null)
      {
        for (int index = 0; index < this.rejectedInvestors.Length; ++index)
        {
          if (string.Compare(this.rejectedInvestors[index], investorName, true) == 0)
            return true;
        }
      }
      return false;
    }

    public string AlertMsg => this.alertMsg;

    public void UpdateAlerts(
      UserInfo userInfo,
      AclGroup[] usersAclGroups,
      AlertSetupData alertSetup)
    {
      int[] usersAclGroups1 = new int[usersAclGroups.Length];
      for (int index = 0; index < usersAclGroups1.Length; ++index)
        usersAclGroups1[index] = usersAclGroups[index].ID;
      this.UpdateAlerts(userInfo, usersAclGroups1, alertSetup);
    }

    public void UpdateAlerts(UserInfo userInfo, int[] usersAclGroups, AlertSetupData alertSetup)
    {
      Hashtable info = this.Info;
      ArrayList arrayList1 = new ArrayList();
      string field = (string) this.GetField("ActionTaken");
      if (field != null && !EllieMae.EMLite.Common.LoanStatus.ActiveLoan.Contains((object) field))
      {
        this.alertMsg = "Adverse Loan";
        this.numAlerts = 0;
        this.alerts = new PipelineInfo.Alert[0];
      }
      else if (((string) this.GetField("NextMilestoneName") ?? "") == string.Empty)
      {
        this.alertMsg = string.Empty;
        this.numAlerts = 0;
        this.alerts = new PipelineInfo.Alert[0];
      }
      else
      {
        this.numAlerts = 0;
        string str1 = string.Empty;
        ArrayList arrayList2 = new ArrayList();
        ArrayList arrayList3 = new ArrayList();
        Hashtable hashtable = new Hashtable();
        foreach (PipelineInfo.Alert alert in this.alerts)
        {
          bool flag1 = false;
          string str2 = "";
          if (!this.IsAlertActive(alert, alertSetup))
          {
            arrayList3.Add((object) alert);
          }
          else
          {
            if ((alert.UserID ?? "") != "" || alert.GroupID >= 0)
            {
              bool flag2 = alert.UserID == userInfo.Userid || Array.IndexOf<int>(usersAclGroups, alert.GroupID) >= 0;
              if ((alert.AlertTargetID ?? "") != "" && hashtable.ContainsKey((object) alert.AlertTargetID))
                flag2 = false;
              if (!flag2)
              {
                arrayList3.Add((object) alert);
                continue;
              }
            }
            string prefix = alert.Event;
            string status = alert.Status;
            DateTime date = alert.Date;
            alert.Date.ToString("M/d/yy");
            int days = (date.Date - DateTime.Now.Date).Days;
            switch (alert.AlertID)
            {
              case 0:
                PipelineInfo.MilestoneInfo milestoneById1 = this.GetMilestoneByID(alert.AlertTargetID);
                if (milestoneById1 != null && alert.UserID == userInfo.Userid && string.Concat(alertSetup.MilestoneAlertMessages[(object) milestoneById1.MilestoneName]) != "")
                {
                  str2 = str2 + alertSetup.MilestoneAlertMessages[(object) milestoneById1.MilestoneName] + Environment.NewLine;
                  alert.AlertMessage = string.Concat(alertSetup.MilestoneAlertMessages[(object) milestoneById1.MilestoneName]);
                  flag1 = true;
                  break;
                }
                break;
              case 1:
              case 4:
                alert.AlertMessage = this.formatAlertMessage("Follow up (" + prefix + ")", days, string.Empty, string.Empty);
                str2 = str2 + alert.AlertMessage + Environment.NewLine;
                flag1 = true;
                break;
              case 2:
              case 20:
                alert.AlertMessage = this.formatAlertMessage(prefix, days, "expected", "expected");
                str2 = str2 + alert.AlertMessage + Environment.NewLine;
                flag1 = true;
                break;
              case 3:
                alert.AlertMessage = this.formatAlertMessage("Escrow disbursement", days, "due", "due");
                str2 = str2 + alert.AlertMessage + Environment.NewLine;
                flag1 = true;
                break;
              case 5:
                PipelineInfo.MilestoneInfo milestoneById2 = this.GetMilestoneByID(alert.AlertTargetID);
                if (milestoneById2 != null && !milestoneById2.Finished)
                {
                  alert.AlertMessage = this.formatAlertMessage(milestoneById2.MilestoneName, days, "expected", "expected");
                  str2 = str2 + alert.AlertMessage + Environment.NewLine;
                  flag1 = true;
                  break;
                }
                break;
              case 6:
                alert.AlertMessage = this.formatAlertMessage("Borrower payment past due", days, string.Empty, string.Empty);
                str2 = str2 + alert.AlertMessage + Environment.NewLine;
                flag1 = true;
                break;
              case 7:
                alert.AlertMessage = this.formatAlertMessage("Statement printing/mailing", days, "due", "due");
                str2 = str2 + alert.AlertMessage + Environment.NewLine;
                flag1 = true;
                break;
              case 8:
                alert.AlertMessage = "Purchase Advice Form not reconciled";
                str2 = str2 + alert.AlertMessage + Environment.NewLine;
                flag1 = true;
                break;
              case 9:
                alert.AlertMessage = alert.Event;
                str2 = str2 + alert.AlertMessage + Environment.NewLine;
                flag1 = true;
                break;
              case 10:
                alert.AlertMessage = this.formatAlertMessage("Lock", days, "expires", "expired");
                str2 = str2 + alert.AlertMessage + Environment.NewLine;
                flag1 = true;
                break;
              case 12:
                alert.AlertMessage = this.formatAlertMessage("Investor delivery", days, "due", "due");
                str2 = str2 + alert.AlertMessage + Environment.NewLine;
                flag1 = true;
                break;
              case 14:
                alert.AlertMessage = this.formatAlertMessage("Registration", days, "expires", "expired");
                str2 = str2 + alert.AlertMessage + Environment.NewLine;
                flag1 = true;
                break;
              case 15:
              case 31:
              case 32:
              case 67:
                alert.AlertMessage = !(status == "expected") ? this.formatAlertMessage(prefix, days, "expires", "expired") : this.formatAlertMessage(prefix, days, "expected", "expected");
                str2 = str2 + alert.AlertMessage + Environment.NewLine;
                flag1 = true;
                break;
              case 17:
                alert.AlertMessage = this.formatAlertMessage(prefix, days, "expires", "expired");
                str2 = str2 + alert.AlertMessage + Environment.NewLine;
                flag1 = true;
                break;
              case 18:
                alert.AlertMessage = alert.Event;
                str2 = str2 + alert.AlertMessage + Environment.NewLine;
                flag1 = true;
                break;
              case 19:
                alert.AlertMessage = alert.Event;
                str2 = str2 + alert.AlertMessage + Environment.NewLine;
                flag1 = true;
                break;
              case 28:
                alert.AlertMessage = "Compliance Review - " + alert.Event + " " + alert.Status;
                str2 = str2 + alert.AlertMessage + Environment.NewLine;
                flag1 = true;
                break;
              case 30:
                alert.AlertMessage = "eFolder update - " + alert.Event;
                str2 = str2 + alert.AlertMessage + Environment.NewLine;
                flag1 = true;
                break;
              case 33:
                alert.AlertMessage = alert.Event;
                str2 = str2 + alert.AlertMessage + Environment.NewLine;
                flag1 = true;
                break;
              case 34:
                alert.AlertMessage = alert.Event;
                str2 = str2 + alert.AlertMessage + Environment.NewLine;
                flag1 = true;
                break;
              case 35:
                alert.AlertMessage = alert.Event;
                str2 = str2 + alert.AlertMessage + Environment.NewLine;
                flag1 = true;
                break;
              case 36:
                alert.AlertMessage = alert.Event;
                str2 = str2 + alert.AlertMessage + Environment.NewLine;
                flag1 = true;
                break;
              case 37:
                alert.AlertMessage = alert.Event;
                str2 = str2 + alert.AlertMessage + Environment.NewLine;
                flag1 = true;
                break;
              case 38:
                alert.AlertMessage = alert.Event;
                str2 = str2 + alert.AlertMessage + Environment.NewLine;
                flag1 = true;
                break;
              case 39:
                alert.AlertMessage = alert.Event;
                str2 = str2 + alert.AlertMessage + Environment.NewLine;
                flag1 = true;
                break;
              case 40:
                alert.AlertMessage = alert.Event;
                str2 = str2 + alert.AlertMessage + Environment.NewLine;
                flag1 = true;
                break;
              case 41:
                alert.AlertMessage = alert.Event;
                str2 = str2 + alert.AlertMessage + Environment.NewLine;
                flag1 = true;
                break;
              case 42:
                alert.AlertMessage = alert.Event;
                str2 = str2 + alert.AlertMessage + Environment.NewLine;
                flag1 = true;
                break;
              case 44:
                alert.AlertMessage = alert.Event;
                str2 = str2 + alert.AlertMessage + Environment.NewLine;
                flag1 = true;
                break;
              case 52:
                alert.AlertMessage = "Fannie Mae DU - " + alert.Event + " " + alert.Status;
                str2 = str2 + alert.AlertMessage + Environment.NewLine;
                flag1 = true;
                break;
              case 53:
                alert.AlertMessage = "Fannie Mae EC - " + alert.Event + " " + alert.Status;
                str2 = str2 + alert.AlertMessage + Environment.NewLine;
                flag1 = true;
                break;
              case 56:
                alert.AlertMessage = "Freddie Mac LPA - " + alert.Event + " " + alert.Status;
                str2 = str2 + alert.AlertMessage + Environment.NewLine;
                flag1 = true;
                break;
              case 57:
                alert.AlertMessage = "Freddie Mac LQA - " + alert.Event + " " + alert.Status;
                str2 = str2 + alert.AlertMessage + Environment.NewLine;
                flag1 = true;
                break;
              case 58:
                alert.AlertMessage = alert.Event;
                str2 = str2 + alert.AlertMessage + Environment.NewLine;
                flag1 = true;
                break;
              case 59:
                alert.AlertMessage = "MI Service Arch - " + alert.Event + " " + alert.Status;
                str2 = str2 + alert.AlertMessage + Environment.NewLine;
                flag1 = true;
                break;
              case 60:
                alert.AlertMessage = "MI Service Radian - " + alert.Event + " " + alert.Status;
                str2 = str2 + alert.AlertMessage + Environment.NewLine;
                flag1 = true;
                break;
              case 61:
                alert.AlertMessage = "MI Service MGIC - " + alert.Event + " " + alert.Status;
                str2 = str2 + alert.AlertMessage + Environment.NewLine;
                flag1 = true;
                break;
              case 64:
                alert.AlertMessage = "Credit Limit required if Account Type = HELOC and Will be Paid Off is false";
                str2 = str2 + alert.AlertMessage + Environment.NewLine;
                flag1 = true;
                break;
              case 65:
                alert.AlertMessage = "Current and Proposed Lien position required if Resubordinated Indicator is True";
                str2 = str2 + alert.AlertMessage + Environment.NewLine;
                flag1 = true;
                break;
              case 66:
                alert.AlertMessage = "Current and Proposed Lien position required if Subject Property is true and will be Paid off is False (regardless of resubordinated)";
                str2 = str2 + alert.AlertMessage + Environment.NewLine;
                flag1 = true;
                break;
              case 68:
                alert.AlertMessage = LiborTransitionToSofrAlert.Message;
                str2 = str2 + alert.AlertMessage + Environment.NewLine;
                flag1 = true;
                break;
              case 69:
                alert.AlertMessage = alert.Event;
                str2 = str2 + alert.AlertMessage + Environment.NewLine;
                flag1 = true;
                break;
            }
            AlertConfig alertConfig = alertSetup.GetAlertConfig(alert.AlertID);
            if (alertConfig != null && (alertConfig.Definition is RegulationAlert || alertConfig.Definition is CustomAlert))
            {
              alert.AlertMessage = alertConfig.Definition.Name;
              str2 = str2 + alert.AlertMessage + Environment.NewLine;
              flag1 = true;
            }
            if (flag1)
            {
              ++this.numAlerts;
              str1 += str2;
              if ((alert.AlertTargetID ?? "") != "")
                hashtable[(object) alert.AlertTargetID] = (object) true;
            }
            else
              arrayList3.Add((object) alert);
          }
        }
        if (this.numAlerts != 0)
          str1 = str1.Substring(0, str1.LastIndexOf(Environment.NewLine));
        this.alertMsg = str1;
        PipelineInfo.Alert[] alertArray = new PipelineInfo.Alert[this.alerts.Length - arrayList3.Count + arrayList2.Count];
        arrayList2.CopyTo((Array) alertArray);
        int count = arrayList2.Count;
        foreach (PipelineInfo.Alert alert in this.alerts)
        {
          if (!arrayList3.Contains((object) alert))
            alertArray[count++] = alert;
        }
        this.alerts = alertArray;
      }
    }

    private string formatAlertMessage(
      string prefix,
      int daysToAlert,
      string duePrefix,
      string overduePrefix)
    {
      if (prefix == null)
        prefix = string.Empty;
      if (string.Empty != prefix && !prefix.EndsWith(" "))
        prefix += " ";
      if (duePrefix == null)
        duePrefix = string.Empty;
      if (string.Empty != duePrefix && !duePrefix.EndsWith(" "))
        duePrefix += " ";
      if (overduePrefix == null)
        overduePrefix = string.Empty;
      if (string.Empty != overduePrefix && !overduePrefix.EndsWith(" "))
        overduePrefix += " ";
      if (daysToAlert == 0 || 1 == daysToAlert || 1 < daysToAlert)
        return prefix + duePrefix;
      return prefix + overduePrefix;
    }

    public bool IsAlertActive(PipelineInfo.Alert alert, AlertSetupData alertSetup)
    {
      AlertConfig alertConfig = alertSetup.GetAlertConfig(alert.AlertID);
      if (alertConfig == null || !alertConfig.AlertEnabled || alert.DisplayStatus == 0 || alert.Date == DateTime.MinValue || alert.Date == DateTime.MaxValue)
        return false;
      if (alertConfig.AlertTiming == AlertTiming.DaysBefore)
      {
        int days = (alert.Date - DateTime.Today).Days;
        if (days > 0 && days > alertConfig.DaysBefore)
          return false;
      }
      PipelineInfo.MilestoneInfo milestoneByName = this.GetMilestoneByName(string.Concat(this.GetField("CurrentMilestoneName")));
      if (milestoneByName == null)
        return false;
      return alert.AlertID == 0 ? alertConfig.MilestoneGuidList.Contains(alert.AlertTargetID) : alertConfig.MilestoneGuidList.Contains(milestoneByName.MilestoneID);
    }

    private bool dismissOrSnoozed(PipelineInfo.Alert alert)
    {
      if (alert.DisplayStatus == 3)
        return true;
      if (alert.DisplayStatus != 2 || alert.SnoozeDuration == 0)
        return false;
      DateTime dateTime = alert.SnoozeStartDTTM;
      dateTime = dateTime.AddMinutes((double) alert.SnoozeDuration);
      return dateTime > DateTime.Now;
    }

    private AlertConfig GetAlertConfig(PipelineInfo.Alert alert, AlertConfig[] config)
    {
      foreach (AlertConfig alertConfig in config)
      {
        if (alert.AlertID == alertConfig.AlertID)
          return alertConfig;
      }
      return (AlertConfig) null;
    }

    public PipelineInfo.SpecialOfferAlert[] OfferAlerts
    {
      get
      {
        return (PipelineInfo.SpecialOfferAlert[]) this.offerAlerts.ToArray(typeof (PipelineInfo.SpecialOfferAlert));
      }
    }

    public Hashtable Info => this.info;

    public PipelineInfo.LoanAssociateInfo[] LoanAssociates => this.loanAssociates;

    public string GUID => (string) this.GetField("Guid");

    public string LinkGuid => (string) this.GetField(nameof (LinkGuid));

    public bool IsSecondMortgage => (string) this.GetField("SecondMortgage") == "Y";

    public string LastName => (string) this.GetField("BorrowerLastName");

    public string FirstName => (string) this.GetField("BorrowerFirstName");

    public string LoanFolder => (string) this.GetField(nameof (LoanFolder));

    public string IsArchived
    {
      get
      {
        return Utils.ConvertToNativeValue(this.GetField(nameof (IsArchived)).ToString(), FieldFormat.YN, false).ToString();
      }
    }

    public string LoanName => (string) this.GetField(nameof (LoanName));

    public string LoanNumber => string.Concat(this.GetField(nameof (LoanNumber)));

    public LoanIdentity Identity
    {
      get
      {
        if (this.id == (LoanIdentity) null)
        {
          string str = string.Concat(this.GetField("XrefId"));
          this.id = !Utils.IsInt((object) str) ? new LoanIdentity(this.LoanFolder, this.LoanName, this.GUID) : new LoanIdentity(this.LoanFolder, this.LoanName, this.GUID, Utils.ParseInt((object) str));
        }
        return this.id;
      }
    }

    public DateTime LastModified
    {
      get
      {
        return this.GetField(nameof (LastModified)) == null ? DateTime.MinValue : (DateTime) this.GetField(nameof (LastModified));
      }
    }

    public int LoanVersionNumber
    {
      get
      {
        return this.GetField(nameof (LoanVersionNumber)) == null ? 0 : Convert.ToInt32(this.GetField(nameof (LoanVersionNumber)));
      }
    }

    public int[] GetRolesForLoanAssociate(UserInfo user, int[] usersAclGroups)
    {
      if (this.loanAssociates == null)
        throw new Exception("The PipelineInfo does not contain the LoanAssociate information needed to satisfy this request");
      ArrayList arrayList1 = new ArrayList((ICollection) usersAclGroups);
      ArrayList arrayList2 = new ArrayList();
      foreach (PipelineInfo.LoanAssociateInfo loanAssociate in this.loanAssociates)
      {
        bool flag = false;
        if (loanAssociate.AssociateType == LoanAssociateType.User && loanAssociate.UserID == user.Userid)
          flag = true;
        else if (loanAssociate.AssociateType == LoanAssociateType.Group && arrayList1.Contains((object) loanAssociate.GroupID))
          flag = true;
        if (flag && !arrayList2.Contains((object) loanAssociate.RoleID))
          arrayList2.Add((object) loanAssociate.RoleID);
      }
      return (int[]) arrayList2.ToArray(typeof (int));
    }

    public LockInfo LockInfo
    {
      set => this.lockInfo = value;
      get => this.lockInfo;
    }

    public List<LockInfo> Locks
    {
      set => this.locks = value;
      get => this.locks;
    }

    public Hashtable Rights => this.rights;

    public PipelineInfo.MilestoneInfo[] Milestones => this.milestones;

    public string LoanDisplayString
    {
      get => this.LastName + ", " + this.FirstName + " (# " + this.LoanNumber + ")";
    }

    public override int GetHashCode() => this.GUID.GetHashCode();

    public override bool Equals(object obj)
    {
      return obj != null && obj is PipelineInfo pipelineInfo && this.GUID == pipelineInfo.GUID;
    }

    public string LoanPurpose => (string) this.GetField(nameof (LoanPurpose));

    public string LoanType => (string) this.GetField(nameof (LoanType));

    public object GetField(string name) => this.GetField(name, true);

    public object GetField(string name, bool allowAutoPrefix)
    {
      if (this.info.ContainsKey((object) name))
        return this.info[(object) name];
      if (this.info.ContainsKey((object) name.ToLower()))
        return this.info[(object) name.ToLower()];
      string key1 = "Loan." + name;
      if (this.info.ContainsKey((object) key1))
        return this.info[(object) key1];
      name = name.ToLower();
      foreach (string key2 in (IEnumerable) this.info.Keys)
      {
        if (key2.ToLower().EndsWith("." + name))
          return this.info[(object) key2];
      }
      return (object) null;
    }

    public bool NeedFixMilestoneIDs
    {
      get
      {
        foreach (PipelineInfo.MilestoneInfo milestone in this.Milestones)
        {
          if ((milestone.MilestoneID ?? "").Trim() == "")
            return true;
        }
        return false;
      }
    }

    public void FixMilestoneIDs(Hashtable msGuidMapping)
    {
      foreach (PipelineInfo.MilestoneInfo milestone in this.Milestones)
      {
        if ((milestone.MilestoneID ?? "").Trim() == "")
          milestone.FixMilestoneID(msGuidMapping);
      }
    }

    public PipelineInfo.MilestoneInfo GetMilestoneByName(string milestoneName)
    {
      if (this.milestones != null)
      {
        foreach (PipelineInfo.MilestoneInfo milestone in this.milestones)
        {
          if (milestoneName == milestone.MilestoneName)
            return milestone;
        }
      }
      return (PipelineInfo.MilestoneInfo) null;
    }

    public PipelineInfo.MilestoneInfo GetMilestoneByID(string milestoneID)
    {
      foreach (PipelineInfo.MilestoneInfo milestone in this.milestones)
      {
        if (milestoneID == milestone.MilestoneID)
          return milestone;
      }
      return (PipelineInfo.MilestoneInfo) null;
    }

    public PipelineInfo.MilestoneInfo GetMilestoneByOrder(int milestoneOrder)
    {
      if (-1 < milestoneOrder)
      {
        foreach (PipelineInfo.MilestoneInfo milestone in this.milestones)
        {
          if (milestoneOrder == milestone.Order)
            return milestone;
        }
      }
      return (PipelineInfo.MilestoneInfo) null;
    }

    public PipelineInfo.MilestoneInfo GetCurrentMilestone()
    {
      string field = (string) this.GetField("CurrentMilestoneName");
      return field != null ? this.GetMilestoneByName(field) : (PipelineInfo.MilestoneInfo) null;
    }

    public PipelineInfo.LoanAssociateInfo GetCurrentLoanAssociate()
    {
      PipelineInfo.MilestoneInfo currentMilestone = this.GetCurrentMilestone();
      if (currentMilestone == null)
        return (PipelineInfo.LoanAssociateInfo) null;
      int milestoneOrder = currentMilestone.Order + 1;
      if (milestoneOrder >= this.milestones.Length)
        milestoneOrder = currentMilestone.Order;
      for (; milestoneOrder >= 0; --milestoneOrder)
      {
        PipelineInfo.LoanAssociateInfo loanAssociate = this.GetLoanAssociate(this.GetMilestoneByOrder(milestoneOrder).AssociateGuid);
        if (loanAssociate != null)
          return loanAssociate;
      }
      return (PipelineInfo.LoanAssociateInfo) null;
    }

    public PipelineInfo.LoanAssociateInfo GetTeamMember(
      PipelineTeamMemberDefinition teamMemberDefinition)
    {
      return PipelineTeamMember.SpecifiedRoleId == teamMemberDefinition.TeamMemberEnum ? this.getTeamMemberByRole(teamMemberDefinition.RoleId) : this.getTeamMemberByType(teamMemberDefinition.TeamMemberEnum);
    }

    private PipelineInfo.LoanAssociateInfo getTeamMemberByRole(int roleId)
    {
      if (0 >= roleId)
        return (PipelineInfo.LoanAssociateInfo) null;
      for (int index = this.milestones.Length - 1; index >= 0; --index)
      {
        PipelineInfo.MilestoneInfo milestone = this.milestones[index];
        if (roleId == milestone.RoleID && !string.IsNullOrEmpty(milestone.AssociateGuid))
        {
          PipelineInfo.LoanAssociateInfo loanAssociate = this.GetLoanAssociate(milestone.AssociateGuid);
          if (loanAssociate != null)
            return loanAssociate;
        }
      }
      foreach (PipelineInfo.LoanAssociateInfo loanAssociate in this.loanAssociates)
      {
        if (roleId == loanAssociate.RoleID)
          return loanAssociate;
      }
      return (PipelineInfo.LoanAssociateInfo) null;
    }

    public PipelineInfo.LoanAssociateInfo GetLoanAssociate(string associateGuid)
    {
      if (associateGuid == null || string.Empty == associateGuid)
        return (PipelineInfo.LoanAssociateInfo) null;
      foreach (PipelineInfo.LoanAssociateInfo loanAssociate in this.loanAssociates)
      {
        if (loanAssociate.AssociateGuid == associateGuid)
          return loanAssociate;
      }
      return (PipelineInfo.LoanAssociateInfo) null;
    }

    public PipelineInfo.LoanAssociateInfo GetLoanAssociateForMilestone(string milestoneId)
    {
      if (milestoneId == null || string.Empty == milestoneId)
        return (PipelineInfo.LoanAssociateInfo) null;
      foreach (PipelineInfo.LoanAssociateInfo loanAssociate in this.loanAssociates)
      {
        if (loanAssociate.MilestoneID == milestoneId)
          return loanAssociate;
      }
      return (PipelineInfo.LoanAssociateInfo) null;
    }

    public override string ToString()
    {
      return Utils.JoinName(this.FirstName, this.LastName) + " (" + this.LoanNumber + ")";
    }

    private PipelineInfo.LoanAssociateInfo getTeamMemberByType(PipelineTeamMember teamMemberType)
    {
      if (PipelineTeamMember.FileStartedBy == teamMemberType)
      {
        PipelineInfo.MilestoneInfo milestoneByName = this.GetMilestoneByName("Started");
        return milestoneByName == null ? (PipelineInfo.LoanAssociateInfo) null : this.GetLoanAssociateForMilestone(milestoneByName.MilestoneID);
      }
      int currentMilestoneIndex = this.getCurrentMilestoneIndex();
      if (PipelineTeamMember.PreviousLoanTeamMember == teamMemberType)
        --currentMilestoneIndex;
      for (int index = currentMilestoneIndex; index >= 0; --index)
      {
        if (!string.IsNullOrEmpty(this.milestones[index].AssociateGuid))
        {
          PipelineInfo.LoanAssociateInfo loanAssociate = this.GetLoanAssociate(this.milestones[index].AssociateGuid);
          if (loanAssociate != null)
            return loanAssociate;
        }
      }
      return (PipelineInfo.LoanAssociateInfo) null;
    }

    private int getCurrentMilestoneIndex()
    {
      for (int currentMilestoneIndex = 1; currentMilestoneIndex < this.milestones.Length; ++currentMilestoneIndex)
      {
        if (!this.milestones[currentMilestoneIndex].Finished)
          return currentMilestoneIndex;
      }
      return this.milestones.Length - 1;
    }

    [Serializable]
    public class Borrower
    {
      public int PairIndex;
      public string FirstName;
      public string LastName;
      public string HomePhone;
      public string WorkPhone;
      public string CellPhone;
      public string Email;
      public string WorkEmail;
      public string SSN;
      public LoanBorrowerType BorrowerType;
    }

    [Serializable]
    public class Alert
    {
      public const int DisplayStatus_INACTIVE = 0;
      public const int DisplayStatus_Active = 1;
      public const int DisplayStatus_SNOOZED = 2;
      public const int DisplayStatus_DISMISS = 3;
      public int AlertID;
      public string Event;
      public string Status;
      public DateTime Date;
      public int DisplayStatus;
      public DateTime SnoozeStartDTTM = DateTime.MinValue;
      public int SnoozeDuration;
      public string LoanAlertID = "";
      public string AlertMessage = "";
      public string AlertTargetID = "";
      public string LogRecordID;
      public string UserID;
      public int GroupID = -1;
      public string CurrentMileStoneID;
      public string LoanGuid;

      public Alert()
      {
      }

      public Alert(
        int alertId,
        string eventData,
        string status,
        DateTime date,
        int displayStatus,
        string userId,
        int groupId,
        DateTime snoozeStartDTTM,
        int snoozeDuration,
        string alertTargetID,
        string logRecordID)
      {
        this.AlertID = alertId;
        this.Event = eventData ?? "";
        this.Status = status ?? "";
        this.Date = date;
        this.DisplayStatus = displayStatus;
        this.UserID = userId;
        this.GroupID = groupId;
        this.SnoozeStartDTTM = snoozeStartDTTM;
        this.SnoozeDuration = snoozeDuration;
        this.AlertTargetID = alertTargetID;
        this.LogRecordID = logRecordID;
      }

      public Alert(
        int alertId,
        string eventData,
        string status,
        DateTime date,
        string alertTargetID,
        string logRecordID)
        : this(alertId, eventData, status, date, 1, (string) null, -1, DateTime.MinValue, 0, alertTargetID, logRecordID)
      {
      }

      public Alert(
        int alertId,
        string eventData,
        string status,
        DateTime date,
        string userId,
        int groupId,
        string alertTargetID,
        string logRecordID)
        : this(alertId, eventData, status, date, 1, userId, groupId, DateTime.MinValue, 0, alertTargetID, logRecordID)
      {
      }

      public string MilestoneID
      {
        get => this.AlertID == 5 || this.AlertID == 0 ? this.AlertTargetID : (string) null;
      }

      public string UserID_DefaultEmpty => this.UserID == null ? string.Empty : this.UserID;

      public static int GetDuration(string timePeriod)
      {
        int duration = 0;
        int num = int.Parse(timePeriod.Split(' ')[0]);
        if (timePeriod.IndexOf("minutes") > 0)
          duration = num;
        else if (timePeriod.IndexOf("hour") > 0)
          duration = num * 60;
        else if (timePeriod.IndexOf("day") > 0)
          duration = num * 24 * 60;
        else if (timePeriod.IndexOf("week") > 0)
          duration = num * 7 * 24 * 60;
        else if (timePeriod.IndexOf("month") > 0)
          duration = num * 30 * 24 * 60;
        return duration;
      }
    }

    [Serializable]
    public class AlertSummaryInfo
    {
      public int AlertCount;
      public DateTime EarliestAlertTime = DateTime.MaxValue;

      public AlertSummaryInfo(int count, DateTime earliestTime)
      {
        this.AlertCount = count;
        this.EarliestAlertTime = earliestTime;
      }
    }

    [Serializable]
    public class MilestoneInfo
    {
      private string milestoneID;
      public readonly string MilestoneName;
      public readonly int RoleID;
      public readonly string AssociateGuid;
      public readonly bool Finished;
      public readonly bool Reviewed;
      public readonly int Order;
      public readonly DateTime DateStarted;
      public readonly DateTime DateCompleted;

      public string MilestoneID => this.milestoneID;

      public MilestoneInfo(
        string milestoneID,
        string milestoneName,
        int roleID,
        string associateGuid,
        bool finished,
        bool reviewed,
        int order,
        DateTime dateStarted,
        DateTime dateCompleted)
      {
        this.milestoneID = (milestoneID ?? "").Trim();
        this.MilestoneName = (milestoneName ?? "").Trim();
        this.RoleID = roleID;
        this.AssociateGuid = associateGuid;
        this.Finished = finished;
        this.Reviewed = reviewed;
        this.Order = order;
        this.DateStarted = dateStarted;
        this.DateCompleted = dateCompleted;
      }

      public void FixMilestoneID(Hashtable msGuidMapping)
      {
        if (!((this.milestoneID ?? "").Trim() == ""))
          return;
        this.milestoneID = MilestoneLog.MilestoneNameToID(this.MilestoneName, msGuidMapping);
      }
    }

    [Serializable]
    public class LoanAssociateInfo
    {
      public readonly string AssociateGuid;
      public readonly LoanAssociateType AssociateType;
      public readonly string UserID;
      public readonly string UserFName;
      public readonly string UserLName;
      public readonly int GroupID = -1;
      public readonly string AssociateName;
      public readonly string AssociateTitle;
      public readonly string AssociateEmail;
      public readonly string AssociatePhone;
      public readonly string AssociateFax;
      public readonly int RoleID;
      public readonly string RoleName;
      public readonly string RoleAbbrev;
      public readonly string MilestoneID;
      public readonly string MilestoneName;
      public readonly bool WriteAccess;
      public readonly int Order;
      public readonly string aPIClientID = string.Empty;

      public LoanAssociateInfo(
        string associateGuid,
        string userId,
        string fullname,
        string email,
        string phone,
        string fax,
        string title,
        int roleId,
        string milestoneId,
        bool writeAccess,
        int order)
      {
        this.AssociateGuid = associateGuid;
        this.UserID = userId;
        this.AssociateName = fullname;
        this.AssociateEmail = email;
        this.AssociatePhone = phone;
        this.AssociateFax = fax;
        this.AssociateTitle = title;
        this.RoleID = roleId;
        this.MilestoneID = milestoneId;
        this.WriteAccess = writeAccess;
        this.Order = order;
        this.AssociateType = LoanAssociateType.User;
      }

      public LoanAssociateInfo(
        string associateGuid,
        string userId,
        string fullname,
        string email,
        string phone,
        string fax,
        string title,
        int roleId,
        string milestoneId,
        bool writeAccess,
        int order,
        string aPIClientID)
      {
        this.AssociateGuid = associateGuid;
        this.UserID = userId;
        this.AssociateName = fullname;
        this.AssociateEmail = email;
        this.AssociatePhone = phone;
        this.AssociateFax = fax;
        this.AssociateTitle = title;
        this.RoleID = roleId;
        this.MilestoneID = milestoneId;
        this.WriteAccess = writeAccess;
        this.Order = order;
        this.AssociateType = LoanAssociateType.User;
        this.aPIClientID = aPIClientID;
      }

      public LoanAssociateInfo(
        string associateGuid,
        int groupId,
        string groupname,
        int roleId,
        string milestoneId,
        bool writeAccess,
        int order)
      {
        this.AssociateGuid = associateGuid;
        this.GroupID = groupId;
        this.AssociateName = groupname;
        this.RoleID = roleId;
        this.MilestoneID = milestoneId;
        this.WriteAccess = writeAccess;
        this.Order = order;
        this.AssociateType = LoanAssociateType.Group;
      }

      public LoanAssociateInfo(
        string associateGuid,
        LoanAssociateType associateType,
        string userId,
        int groupId,
        string fname,
        string lname,
        string fullname,
        string email,
        string phone,
        string fax,
        int roleId,
        string roleName,
        string roleAbbrev,
        string milestoneId,
        string milestoneName,
        bool writeAccess,
        int order)
      {
        this.AssociateGuid = associateGuid;
        this.AssociateType = associateType;
        this.UserID = userId;
        this.GroupID = groupId;
        this.UserFName = fname;
        this.UserLName = lname;
        this.AssociateName = fullname;
        this.AssociateEmail = email;
        this.AssociatePhone = phone;
        this.AssociateFax = fax;
        this.RoleID = roleId;
        this.RoleName = roleName;
        this.RoleAbbrev = roleAbbrev;
        this.MilestoneID = milestoneId;
        this.MilestoneName = milestoneName;
        this.WriteAccess = writeAccess;
        this.Order = order;
      }
    }

    [Serializable]
    public class SpecialOfferAlert
    {
      public string OfferID;
      public bool Alert;
    }

    [Serializable]
    public class TradeInfo
    {
      public int TradeID;
      public int AssignedStatus;
      public int PendingStatus;
      public Decimal Profit;

      public int Status => this.PendingStatus <= 0 ? this.AssignedStatus : this.PendingStatus;

      public string CommitmentContractNumber { get; set; }

      public string ProductName { get; set; }

      public int GseCommitmentId { get; set; }

      public Decimal TotalPrice { get; set; }

      public string EPPSLoanProgramName { get; set; }

      public string TradeExtensionInfo { get; set; }
    }
  }
}
