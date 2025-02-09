// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.PurchaseConditionLog
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
  public class PurchaseConditionLog : StandardConditionLog
  {
    public static readonly string XmlType = "PurchaseCondition";
    private string name = string.Empty;
    private string number = string.Empty;
    private int subCategory = -1;
    private int forRoleId = -1;
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

    public PurchaseConditionLog(string addedBy, string pairId)
      : base(addedBy, pairId)
    {
    }

    public PurchaseConditionLog(PurchaseConditionTemplate template, string addedBy, string pairId)
      : base((ConditionTemplate) template, addedBy, pairId)
    {
      this.name = template.Name;
      this.number = template.Number;
      this.allowToClear = template.AllowToClear;
      this.category = template.Category;
      this.subCategory = template.Subcategory;
      this.forRoleId = template.RoleID;
      this.priorTo = template.PriorTo;
      this.isInternal = template.IsInternal;
      this.isExternal = template.IsExternal;
    }

    public PurchaseConditionLog(LogList log, XmlElement e)
      : base(log, e)
    {
      AttributeReader attributeReader = new AttributeReader(e);
      this.name = attributeReader.GetString(nameof (Name));
      this.number = attributeReader.GetString(nameof (Number));
      this.priorTo = attributeReader.GetString(nameof (PriorTo));
      this.category = attributeReader.GetString(nameof (Category));
      this.subCategory = attributeReader.GetInteger(nameof (Subcategory));
      this.forRoleId = attributeReader.GetInteger("RoleID");
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
      this.MarkAsClean();
    }

    public override ConditionType ConditionType => ConditionType.Purchase;

    public string Name
    {
      get => this.name;
      set
      {
        if (!(this.name != value))
          return;
        this.name = value;
        this.TrackChange("Name changed to \"" + this.name + "\"");
      }
    }

    public string Number
    {
      get => this.number;
      set
      {
        if (!(this.number != value))
          return;
        this.number = value;
        this.TrackChange("Number changed to \"" + this.number + "\"");
      }
    }

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

    public int Subcategory
    {
      get => this.subCategory;
      set
      {
        if (this.subCategory == value)
          return;
        this.subCategory = value;
        this.TrackChange("Subcategory changed to \"" + (object) this.subCategory + "\"");
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
      this.dateReviewed = date;
      this.reviewedBy = user;
      this.MarkAsDirty();
    }

    public void UnmarkAsReviewed()
    {
      this.dateReviewed = DateTime.MinValue;
      this.reviewedBy = string.Empty;
      this.MarkAsDirty();
    }

    public bool Rejected => this.dateRejected.Date != DateTime.MinValue.Date;

    public DateTime DateRejected => this.dateRejected;

    public string RejectedBy => this.rejectedBy;

    public void MarkAsRejected(DateTime date, string user)
    {
      this.dateRejected = date;
      this.rejectedBy = user;
      this.MarkAsDirty();
    }

    public void UnmarkAsRejected()
    {
      this.dateRejected = DateTime.MinValue;
      this.rejectedBy = string.Empty;
      this.MarkAsDirty();
    }

    public bool Cleared => this.dateCleared.Date != DateTime.MinValue.Date;

    public DateTime DateCleared => this.dateCleared;

    public string ClearedBy => this.clearedBy;

    public void MarkAsCleared(DateTime date, string user)
    {
      this.dateCleared = date;
      this.clearedBy = user;
      this.MarkAsDirty();
    }

    public void UnmarkAsCleared()
    {
      this.dateCleared = DateTime.MinValue;
      this.clearedBy = string.Empty;
      this.MarkAsDirty();
    }

    public bool Waived => this.dateWaived.Date != DateTime.MinValue.Date;

    public DateTime DateWaived => this.dateWaived;

    public string WaivedBy => this.waivedBy;

    public void MarkAsWaived(DateTime date, string user)
    {
      this.dateWaived = date;
      this.waivedBy = user;
      this.MarkAsDirty();
    }

    public void UnmarkAsWaived()
    {
      this.dateWaived = DateTime.MinValue;
      this.waivedBy = string.Empty;
      this.MarkAsDirty();
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

    public override PipelineInfo.Alert[] GetPipelineAlerts()
    {
      List<PipelineInfo.Alert> alertList = new List<PipelineInfo.Alert>();
      if (this.Expected && !this.Received)
      {
        PipelineInfo.Alert alert = new PipelineInfo.Alert(8, this.Title, "expected", this.DateExpected, this.Guid, this.Guid);
        alertList.Add(alert);
      }
      alertList.AddRange((IEnumerable<PipelineInfo.Alert>) base.GetPipelineAlerts());
      return alertList.ToArray();
    }

    public override void ToXml(XmlElement e)
    {
      base.ToXml(e);
      AttributeWriter attributeWriter = new AttributeWriter(e);
      attributeWriter.Write("Type", (object) PurchaseConditionLog.XmlType);
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
    }
  }
}
