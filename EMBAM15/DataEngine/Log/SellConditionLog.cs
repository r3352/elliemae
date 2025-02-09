// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.SellConditionLog
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.Xml;
using System;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class SellConditionLog : StandardConditionLog
  {
    public static readonly string XmlType = "SellCondition";
    private int forRoleId = -1;
    private string sourceId = string.Empty;
    private bool allowToClear;
    private string priorTo = string.Empty;
    private bool isInternal;
    private bool isExternal = true;
    private DateTime dateFulfilled = DateTime.MinValue;
    private string fulfilledBy = string.Empty;
    private DateTime dateReviewed = DateTime.MinValue;
    private string reviewedBy = string.Empty;
    private DateTime dateRejected = DateTime.MinValue;
    private string rejectedBy = string.Empty;
    private DateTime dateCleared = DateTime.MinValue;
    private string clearedBy = string.Empty;
    private DateTime dateWaived = DateTime.MinValue;
    private string waivedBy = string.Empty;
    private DateTime dateExpires = DateTime.MinValue;
    private string providerURN = string.Empty;
    private string conditionCode = string.Empty;

    public SellConditionLog(string addedBy, string pairId)
      : base(addedBy, pairId)
    {
    }

    public SellConditionLog(SellConditionTemplate template, string addedBy, string pairId)
      : base((ConditionTemplate) template, addedBy, pairId)
    {
      this.allowToClear = template.AllowToClear;
      this.category = template.Category;
      this.forRoleId = template.ForRoleID;
      this.priorTo = template.PriorTo;
      this.isInternal = template.IsInternal;
      this.isExternal = template.IsExternal;
      this.providerURN = template.ProviderURN;
    }

    public SellConditionLog(LogList log, XmlElement e)
      : base(log, e)
    {
      AttributeReader attributeReader = new AttributeReader(e);
      this.category = attributeReader.GetString(nameof (Category));
      this.forRoleId = attributeReader.GetInteger(nameof (ForRoleID));
      this.allowToClear = attributeReader.GetBoolean(nameof (AllowToClear));
      this.priorTo = attributeReader.GetString(nameof (PriorTo));
      this.isInternal = attributeReader.GetBoolean(nameof (IsInternal));
      this.isExternal = attributeReader.GetBoolean(nameof (IsExternal), !this.isInternal);
      this.dateFulfilled = attributeReader.GetDate(nameof (DateFulfilled));
      this.fulfilledBy = attributeReader.GetString(nameof (FulfilledBy));
      this.dateReviewed = attributeReader.GetDate(nameof (DateReviewed));
      this.reviewedBy = attributeReader.GetString(nameof (ReviewedBy));
      this.dateRejected = attributeReader.GetDate(nameof (DateRejected));
      this.rejectedBy = attributeReader.GetString(nameof (RejectedBy));
      this.dateCleared = attributeReader.GetDate(nameof (DateCleared));
      this.clearedBy = attributeReader.GetString(nameof (ClearedBy));
      this.dateWaived = attributeReader.GetDate("DateWaved");
      this.waivedBy = attributeReader.GetString(nameof (WaivedBy));
      this.dateExpires = attributeReader.GetDate("Expires");
      this.providerURN = attributeReader.GetString(nameof (ProviderURN));
      this.conditionCode = attributeReader.GetString(nameof (ConditionCode));
      this.MarkAsClean();
    }

    public string ConditionCode
    {
      get => this.conditionCode;
      set
      {
        if (!(this.conditionCode != value))
          return;
        this.conditionCode = value;
        this.TrackChange("Condition Code changed to \"" + this.conditionCode + "\"");
      }
    }

    public override ConditionType ConditionType => ConditionType.Sell;

    public override string Category
    {
      get => this.category;
      set
      {
        if (!(this.category != value))
          return;
        this.category = value;
        this.TrackChange("Category changed to \"" + this.category + "\"");
      }
    }

    public override string ProviderURN
    {
      get => this.providerURN;
      set
      {
        if (!(this.providerURN != value))
          return;
        this.providerURN = value;
      }
    }

    public string PriorTo
    {
      get => this.priorTo;
      set
      {
        if (!(this.priorTo != value))
          return;
        this.priorTo = value;
        string details = "Prior To changed";
        switch (this.priorTo)
        {
          case "PTA":
            details += " to \"Approval\"";
            break;
          case "PTD":
            details += " to \"Docs\"";
            break;
          case "PTF":
            details += " to \"Funding\"";
            break;
          case "AC":
            details += " to \"Closing\"";
            break;
          case "PTP":
            details += " to \"Purchase\"";
            break;
        }
        this.TrackChange(details);
      }
    }

    public string SourceId
    {
      get => this.sourceId;
      set => this.sourceId = value;
    }

    public int ForRoleID
    {
      get => this.forRoleId;
      set
      {
        if (this.forRoleId == value)
          return;
        this.forRoleId = value;
        string details = "Owner changed";
        if (this.IsAttachedToLog)
        {
          RoleInfo role = this.Log.Loan.Settings.GetRole(this.forRoleId);
          if (role != null)
            details = details + " to " + role.RoleAbbr;
        }
        this.TrackChange(details);
      }
    }

    public bool AllowToClear
    {
      get => this.allowToClear;
      set
      {
        if (this.allowToClear == value)
          return;
        this.allowToClear = value;
        string str = "Allow to Clear";
        this.TrackChange(!this.allowToClear ? str + " unchecked" : str + " checked");
      }
    }

    public bool IsInternal
    {
      get => this.isInternal;
      set
      {
        if (this.isInternal == value)
          return;
        this.isInternal = value;
        string str = "Print Internally";
        this.TrackChange(!this.isInternal ? str + " unchecked" : str + " checked");
      }
    }

    public bool IsExternal
    {
      get => this.isExternal;
      set
      {
        if (this.isExternal == value)
          return;
        this.isExternal = value;
        string str = "Print Externally";
        this.TrackChange(!this.isExternal ? str + " unchecked" : str + " checked");
      }
    }

    public bool Fulfilled => this.dateFulfilled.Date != DateTime.MinValue.Date;

    public DateTime DateFulfilled => this.dateFulfilled;

    public string FulfilledBy => this.fulfilledBy;

    public void MarkAsFulfilled(DateTime date, string user)
    {
      List<string> stringList = new List<string>();
      if (this.dateFulfilled != date)
      {
        if (!this.Fulfilled)
          stringList.Add("Status Fulfilled checked");
        this.dateFulfilled = date;
        stringList.Add("Status Fulfilled Date set to \"" + date.ToString("MM/dd/yy hh:mm tt") + "\"");
      }
      if (this.fulfilledBy != user)
      {
        this.fulfilledBy = user;
        stringList.Add("Status Fulfilled by \"" + user + "\"");
      }
      this.TrackChanges(stringList.ToArray());
    }

    public void UnmarkAsFulfilled()
    {
      this.dateFulfilled = DateTime.MinValue;
      this.fulfilledBy = string.Empty;
      this.TrackChange("Status Fulfilled unchecked");
    }

    public bool Reviewed => this.dateReviewed.Date != DateTime.MinValue.Date;

    public DateTime DateReviewed => this.dateReviewed;

    public string ReviewedBy => this.reviewedBy;

    public void MarkAsReviewed(DateTime date, string user)
    {
      List<string> stringList = new List<string>();
      if (this.dateReviewed != date)
      {
        if (!this.Reviewed)
          stringList.Add("Status Reviewed checked");
        this.dateReviewed = date;
        stringList.Add("Status Reviewed Date set to \"" + date.ToString("MM/dd/yy hh:mm tt") + "\"");
      }
      if (this.reviewedBy != user)
      {
        this.reviewedBy = user;
        stringList.Add("Status Reviewed by \"" + user + "\"");
      }
      this.TrackChanges(stringList.ToArray());
    }

    public void UnmarkAsReviewed()
    {
      this.dateReviewed = DateTime.MinValue;
      this.reviewedBy = string.Empty;
      this.TrackChange("Status Reviewed unchecked");
    }

    public bool Rejected => this.dateRejected.Date != DateTime.MinValue.Date;

    public DateTime DateRejected => this.dateRejected;

    public string RejectedBy => this.rejectedBy;

    public void MarkAsRejected(DateTime date, string user)
    {
      List<string> stringList = new List<string>();
      if (this.dateRejected != date)
      {
        if (!this.Rejected)
          stringList.Add("Status Rejected checked");
        this.dateRejected = date;
        stringList.Add("Status Rejected Date set to \"" + date.ToString("MM/dd/yy hh:mm tt") + "\"");
      }
      if (this.rejectedBy != user)
      {
        this.rejectedBy = user;
        stringList.Add("Status Rejected by \"" + user + "\"");
      }
      this.TrackChanges(stringList.ToArray());
    }

    public void UnmarkAsRejected()
    {
      this.dateRejected = DateTime.MinValue;
      this.rejectedBy = string.Empty;
      this.TrackChange("Status Rejected unchecked");
    }

    public bool Cleared => this.dateCleared.Date != DateTime.MinValue.Date;

    public DateTime DateCleared => this.dateCleared;

    public string ClearedBy => this.clearedBy;

    public void MarkAsCleared(DateTime date, string user)
    {
      List<string> stringList = new List<string>();
      if (this.dateCleared != date)
      {
        if (!this.Cleared)
          stringList.Add("Status Cleared checked");
        this.dateCleared = date;
        stringList.Add("Status Cleared Date set to \"" + date.ToString("MM/dd/yy hh:mm tt") + "\"");
      }
      if (this.clearedBy != user)
      {
        this.clearedBy = user;
        stringList.Add("Status Cleared by \"" + user + "\"");
      }
      this.TrackChanges(stringList.ToArray());
    }

    public void UnmarkAsCleared()
    {
      this.dateCleared = DateTime.MinValue;
      this.clearedBy = string.Empty;
      this.TrackChange("Status Cleared unchecked");
    }

    public bool Waived => this.dateWaived.Date != DateTime.MinValue.Date;

    public DateTime DateWaived => this.dateWaived;

    public string WaivedBy => this.waivedBy;

    public void MarkAsWaived(DateTime date, string user)
    {
      List<string> stringList = new List<string>();
      if (this.dateWaived != date)
      {
        if (!this.Waived)
          stringList.Add("Status Waived checked");
        this.dateWaived = date;
        stringList.Add("Status Waived Date set to \"" + date.ToString("MM/dd/yy hh:mm tt") + "\"");
      }
      if (this.waivedBy != user)
      {
        this.waivedBy = user;
        stringList.Add("Status Waived by \"" + user + "\"");
      }
      this.TrackChanges(stringList.ToArray());
    }

    public void UnmarkAsWaived()
    {
      this.dateWaived = DateTime.MinValue;
      this.waivedBy = string.Empty;
      this.TrackChange("Status Waived unchecked");
    }

    public override void MarkAsReceived(DateTime date, string user)
    {
      List<string> stringList = new List<string>();
      if (this.DateReceived != date)
      {
        if (!this.Received)
          stringList.Add("Status Received checked");
        stringList.Add("Status Received Date set to \"" + date.ToString("MM/dd/yy hh:mm tt") + "\"");
      }
      if (this.ReceivedBy != user)
        stringList.Add("Status Received by \"" + user + "\"");
      this.TrackChanges(stringList.ToArray());
      base.MarkAsReceived(date, user);
    }

    public override void UnmarkAsReceived()
    {
      this.TrackChange("Status Received unchecked");
      base.UnmarkAsReceived();
    }

    public override void MarkAsRequested(DateTime date, string user)
    {
      List<string> stringList = new List<string>();
      if (this.DateRequested != date)
      {
        if (!this.Requested)
          stringList.Add("Status Requested checked");
        stringList.Add("Status Requested Date set to \"" + date.ToString("MM/dd/yy hh:mm tt") + "\"");
      }
      if (this.RequestedBy != user)
        stringList.Add("Status Requested by \"" + user + "\"");
      this.TrackChanges(stringList.ToArray());
      base.MarkAsRequested(date, user);
    }

    public override void UnmarkAsRequested()
    {
      this.TrackChange("Status Requested unchecked");
      base.UnmarkAsRequested();
    }

    public override void MarkAsRerequested(DateTime date, string user)
    {
      List<string> stringList = new List<string>();
      if (this.DateRerequested != date)
      {
        if (!this.Rerequested)
          stringList.Add("Status Re-requested checked");
        stringList.Add("Status Re-requested Date set to \"" + date.ToString("MM/dd/yy hh:mm tt") + "\"");
      }
      if (this.RerequestedBy != user)
        stringList.Add("Status Re-requested by \"" + user + "\"");
      this.TrackChanges(stringList.ToArray());
      base.MarkAsRerequested(date, user);
    }

    public override void UnmarkAsRerequested()
    {
      this.TrackChange("Status Re-requested unchecked");
      base.UnmarkAsRerequested();
    }

    public override ConditionStatus Status
    {
      get
      {
        if (this.Waived)
          return ConditionStatus.Waived;
        if (this.Cleared)
          return ConditionStatus.Cleared;
        if (this.Rejected)
          return ConditionStatus.Rejected;
        if (this.Reviewed)
          return ConditionStatus.Reviewed;
        if (this.Received)
          return ConditionStatus.Received;
        if (this.Fulfilled)
          return ConditionStatus.Fulfilled;
        if (this.IsPastDue)
          return ConditionStatus.PastDue;
        if (this.Expected)
          return ConditionStatus.Expected;
        if (this.Rerequested)
          return ConditionStatus.Rerequested;
        return this.Requested ? ConditionStatus.Requested : ConditionStatus.Added;
      }
    }

    public override string StatusDescription
    {
      get
      {
        switch (this.Status)
        {
          case ConditionStatus.Fulfilled:
            return "Fulfilled on " + this.dateFulfilled.ToShortDateString();
          case ConditionStatus.Requested:
            return "Requested on " + this.DateRequested.ToShortDateString();
          case ConditionStatus.Rerequested:
            return "Re-requested on " + this.DateRerequested.ToShortDateString();
          case ConditionStatus.Expected:
          case ConditionStatus.PastDue:
            return "Expected on " + this.DateExpected.ToShortDateString();
          case ConditionStatus.Received:
            return "Received on " + this.DateReceived.ToShortDateString();
          case ConditionStatus.Reviewed:
            return "Reviewed on " + this.dateReviewed.ToShortDateString();
          case ConditionStatus.Rejected:
            return "Rejected on " + this.dateRejected.ToShortDateString();
          case ConditionStatus.Cleared:
            return "Cleared on " + this.dateCleared.ToShortDateString();
          case ConditionStatus.Waived:
            return "Waived on " + this.dateWaived.ToShortDateString();
          case ConditionStatus.Expired:
            return "Expired on " + this.dateExpires.ToShortDateString();
          default:
            return "Added on " + this.DateAdded.ToShortDateString();
        }
      }
    }

    public string StatusForPrint(bool printDate)
    {
      switch (this.Status)
      {
        case ConditionStatus.Fulfilled:
          if (!printDate)
            return "Fulfilled";
          return !(this.dateFulfilled != DateTime.MinValue) ? "" : this.dateFulfilled.ToString("MM/dd/yyyy");
        case ConditionStatus.Requested:
          if (!printDate)
            return "Requested on";
          return !(this.DateRequested != DateTime.MinValue) ? "" : this.DateRequested.ToString("MM/dd/yyyy");
        case ConditionStatus.Rerequested:
          if (!printDate)
            return "Re-requested on";
          return !(this.DateRerequested != DateTime.MinValue) ? "" : this.DateRerequested.ToString("MM/dd/yyyy");
        case ConditionStatus.Expected:
        case ConditionStatus.PastDue:
          if (!printDate)
            return "Expected on";
          return !(this.DateExpected != DateTime.MinValue) ? "" : this.DateExpected.ToString("MM/dd/yyyy");
        case ConditionStatus.Received:
          if (!printDate)
            return "Received";
          return !(this.DateReceived != DateTime.MinValue) ? "" : this.DateReceived.ToString("MM/dd/yyyy");
        case ConditionStatus.Reviewed:
          if (!printDate)
            return "Reviewed";
          return !(this.dateReviewed != DateTime.MinValue) ? "" : this.dateReviewed.ToString("MM/dd/yyyy");
        case ConditionStatus.Rejected:
          if (!printDate)
            return "Rejected";
          return !(this.dateRejected != DateTime.MinValue) ? "" : this.dateRejected.ToString("MM/dd/yyyy");
        case ConditionStatus.Cleared:
          if (!printDate)
            return "Cleared";
          return !(this.dateCleared != DateTime.MinValue) ? "" : this.dateCleared.ToString("MM/dd/yyyy");
        case ConditionStatus.Waived:
          if (!printDate)
            return "Waived";
          return !(this.dateWaived != DateTime.MinValue) ? "" : this.dateWaived.ToString("MM/dd/yyyy");
        case ConditionStatus.Expired:
          if (!printDate)
            return "Expired";
          return !(this.dateExpires != DateTime.MinValue) ? "" : this.dateExpires.ToString("MM/dd/yyyy");
        default:
          if (!printDate)
            return "Added";
          return !(this.DateAdded != DateTime.MinValue) ? "" : this.DateAdded.ToString("MM/dd/yyyy");
      }
    }

    public override DateTime Date
    {
      get
      {
        switch (this.Status)
        {
          case ConditionStatus.Fulfilled:
            return this.dateFulfilled;
          case ConditionStatus.Requested:
            return this.DateRequested;
          case ConditionStatus.Rerequested:
            return this.DateRerequested;
          case ConditionStatus.Expected:
            return this.DateExpected;
          case ConditionStatus.PastDue:
            return this.DateExpected;
          case ConditionStatus.Received:
            return this.DateReceived;
          case ConditionStatus.Reviewed:
            return this.dateReviewed;
          case ConditionStatus.Rejected:
            return this.dateRejected;
          case ConditionStatus.Cleared:
            return this.dateCleared;
          case ConditionStatus.Waived:
            return this.dateWaived;
          case ConditionStatus.Expired:
            return this.dateExpires;
          default:
            return this.DateAdded;
        }
      }
    }

    public bool SetStatusFulfilled { get; set; }

    public bool SetStatusRequested { get; set; }

    public bool SetStatusRerequested { get; set; }

    public bool SetStatusReceived { get; set; }

    public bool SetStatusReviewed { get; set; }

    public bool SetStatusRejected { get; set; }

    public bool SetStatusCleared { get; set; }

    public bool SetStatusWaived { get; set; }

    public override PipelineInfo.Alert[] GetPipelineAlerts()
    {
      List<PipelineInfo.Alert> alertList = new List<PipelineInfo.Alert>();
      if (this.Expected && !this.Received)
      {
        PipelineInfo.Alert alert = new PipelineInfo.Alert(67, this.Title, "expected", this.DateExpected, this.Guid, this.Guid);
        alertList.Add(alert);
      }
      alertList.AddRange((IEnumerable<PipelineInfo.Alert>) base.GetPipelineAlerts());
      return alertList.ToArray();
    }

    public override void ToXml(XmlElement e)
    {
      base.ToXml(e);
      AttributeWriter attributeWriter = new AttributeWriter(e);
      attributeWriter.Write("Type", (object) SellConditionLog.XmlType);
      attributeWriter.Write("Category", (object) this.category);
      attributeWriter.Write("PriorTo", (object) this.priorTo);
      attributeWriter.Write("ForRoleID", (object) this.forRoleId);
      attributeWriter.Write("AllowToClear", (object) this.allowToClear);
      attributeWriter.Write("IsInternal", (object) this.isInternal);
      attributeWriter.Write("IsExternal", (object) this.isExternal);
      attributeWriter.Write("DateFulfilled", (object) this.dateFulfilled);
      attributeWriter.Write("FulfilledBy", (object) this.fulfilledBy);
      attributeWriter.Write("DateReviewed", (object) this.dateReviewed);
      attributeWriter.Write("ReviewedBy", (object) this.reviewedBy);
      attributeWriter.Write("DateRejected", (object) this.dateRejected);
      attributeWriter.Write("RejectedBy", (object) this.rejectedBy);
      attributeWriter.Write("DateCleared", (object) this.dateCleared);
      attributeWriter.Write("ClearedBy", (object) this.clearedBy);
      attributeWriter.Write("DateWaved", (object) this.dateWaived);
      attributeWriter.Write("WaivedBy", (object) this.waivedBy);
      attributeWriter.Write("Expires", (object) this.dateExpires);
      attributeWriter.Write("ProviderURN", (object) this.providerURN);
      attributeWriter.Write("ConditionCode", (object) this.conditionCode);
    }
  }
}
