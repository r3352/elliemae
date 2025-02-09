// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.StandardConditionLog
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.Xml;
using System;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public abstract class StandardConditionLog : ConditionLog
  {
    private string description = string.Empty;
    private string details = string.Empty;
    private int daysTillDue;
    private string requestedFrom = string.Empty;
    private DateTime dateRequested = DateTime.MinValue;
    private string requestedBy = string.Empty;
    private DateTime dateRerequested = DateTime.MinValue;
    private string rerequestedBy = string.Empty;
    private DateTime dateReceived = DateTime.MinValue;
    private string receivedBy = string.Empty;
    private string providerUrn = string.Empty;
    private DateTimeType dayCountSetting;
    private DateTime dateExpected = DateTime.MinValue.Date;

    public StandardConditionLog(string addedBy, string pairId)
    {
      this.addedBy = addedBy;
      this.dateAdded = DateTime.Now;
      this.pairId = pairId;
      this.commentList = new CommentEntryCollection((LogRecordBase) this);
    }

    public StandardConditionLog(ConditionTemplate template, string addedBy, string pairId)
      : this(addedBy, pairId)
    {
      this.title = template.Name;
      this.description = template.Description;
      this.daysTillDue = template.DaysTillDue;
    }

    public StandardConditionLog(LogList log, XmlElement e)
      : base(log, e)
    {
      AttributeReader attributeReader = new AttributeReader(e);
      this.title = attributeReader.GetString("Title");
      this.description = attributeReader.GetString(nameof (Description));
      this.details = attributeReader.GetString(nameof (Details));
      this.daysTillDue = attributeReader.GetInteger(nameof (DaysTillDue));
      this.requestedFrom = attributeReader.GetString(nameof (RequestedFrom));
      this.dateRequested = attributeReader.GetDate(nameof (DateRequested));
      this.requestedBy = attributeReader.GetString(nameof (RequestedBy));
      this.dateRerequested = attributeReader.GetDate(nameof (DateRerequested));
      this.rerequestedBy = attributeReader.GetString(nameof (RerequestedBy));
      this.dateReceived = attributeReader.GetDate(nameof (DateReceived));
      this.receivedBy = attributeReader.GetString(nameof (ReceivedBy));
      this.pairId = attributeReader.GetString("PairId");
      if (this.pairId == string.Empty)
        this.pairId = BorrowerPair.All.Id;
      this.dateAdded = attributeReader.GetDate("DateAdded");
      this.addedBy = attributeReader.GetString("AddedBy");
      this.source = attributeReader.GetString("Source");
      this.commentList = new CommentEntryCollection((LogRecordBase) this, e, "Comments");
      this.MarkAsClean();
      this.dayCountSetting = log.Loan.DocumentDateTimeType;
      this.calculateExpectedDate();
    }

    public string Description
    {
      get => this.description;
      set
      {
        if (!(this.description != value))
          return;
        this.description = value;
        this.TrackChange("Condition Description changed to \"" + this.description + "\"");
      }
    }

    public string Details
    {
      get => this.details;
      set
      {
        if (!(this.details != value))
          return;
        this.details = value;
        this.MarkAsDirty();
      }
    }

    public string RequestedFrom
    {
      get => this.requestedFrom;
      set
      {
        if (!(this.requestedFrom != value))
          return;
        this.requestedFrom = value;
        this.MarkAsDirty();
      }
    }

    public bool Requested => this.dateRequested.Date != DateTime.MinValue.Date;

    public DateTime DateRequested => this.dateRequested;

    public string RequestedBy => this.requestedBy;

    public virtual void MarkAsRequested(DateTime date, string user)
    {
      this.dateRequested = date;
      this.requestedBy = user;
      this.calculateExpectedDate();
      this.MarkAsDirty();
    }

    public virtual void UnmarkAsRequested()
    {
      this.dateRequested = DateTime.MinValue;
      this.requestedBy = string.Empty;
      this.calculateExpectedDate();
      this.MarkAsDirty();
    }

    public bool Rerequested => this.dateRerequested.Date != DateTime.MinValue.Date;

    public DateTime DateRerequested => this.dateRerequested;

    public string RerequestedBy => this.rerequestedBy;

    public virtual void MarkAsRerequested(DateTime date, string user)
    {
      this.dateRerequested = date;
      this.rerequestedBy = user;
      this.calculateExpectedDate();
      this.MarkAsDirty();
    }

    public virtual void UnmarkAsRerequested()
    {
      this.dateRerequested = DateTime.MinValue;
      this.rerequestedBy = string.Empty;
      this.calculateExpectedDate();
      this.MarkAsDirty();
    }

    public int DaysTillDue
    {
      get => this.daysTillDue;
      set
      {
        int num = Math.Max(0, value);
        if (this.daysTillDue == num)
          return;
        this.daysTillDue = num;
        this.calculateExpectedDate();
        this.MarkAsDirty();
      }
    }

    public bool Expected => this.dateExpected.Date != DateTime.MinValue.Date;

    public DateTime DateExpected => this.dateExpected.Date;

    public bool IsPastDue
    {
      get => !this.Received && this.Expected && this.dateExpected.Date <= DateTime.Today;
    }

    private void calculateExpectedDate()
    {
      this.dateExpected = DateTime.MinValue;
      if (this.daysTillDue <= 0)
        return;
      if (this.Rerequested)
      {
        this.dateExpected = DatetimeUtils.GetDateTime(this.dateRerequested, this.dayCountSetting, this.daysTillDue);
      }
      else
      {
        if (!this.Requested)
          return;
        this.dateExpected = DatetimeUtils.GetDateTime(this.dateRequested, this.dayCountSetting, this.daysTillDue);
      }
    }

    public bool Received => this.dateReceived.Date != DateTime.MinValue.Date;

    public DateTime DateReceived => this.dateReceived;

    public string ReceivedBy => this.receivedBy;

    public virtual void MarkAsReceived(DateTime date, string user)
    {
      this.dateReceived = date;
      this.receivedBy = user;
      this.MarkAsDirty();
    }

    public virtual void UnmarkAsReceived()
    {
      this.dateReceived = DateTime.MinValue;
      this.receivedBy = string.Empty;
      this.MarkAsDirty();
    }

    public virtual string ProviderURN
    {
      get => this.providerUrn;
      set
      {
        if (!(this.providerUrn != value))
          return;
        this.providerUrn = value;
      }
    }

    public abstract ConditionStatus Status { get; }

    public abstract string StatusDescription { get; }

    public static string GetStatusString(ConditionStatus status)
    {
      switch (status)
      {
        case ConditionStatus.Added:
          return "Added";
        case ConditionStatus.Fulfilled:
          return "Fulfilled";
        case ConditionStatus.Requested:
          return "Requested";
        case ConditionStatus.Rerequested:
          return "Re-requested";
        case ConditionStatus.Expected:
          return "Expected";
        case ConditionStatus.PastDue:
          return "Past Due";
        case ConditionStatus.Received:
          return "Received";
        case ConditionStatus.Reviewed:
          return "Reviewed";
        case ConditionStatus.Sent:
          return "Sent";
        case ConditionStatus.Rejected:
          return "Rejected";
        case ConditionStatus.Cleared:
          return "Cleared";
        case ConditionStatus.Waived:
          return "Waived";
        case ConditionStatus.Expired:
          return "Expired";
        default:
          return string.Empty;
      }
    }

    public bool HasLinkedDocuments()
    {
      foreach (DocumentLog allDocument in this.Log.GetAllDocuments())
      {
        if (allDocument.Conditions.Contains((ConditionLog) this))
          return true;
      }
      return false;
    }

    internal override void AttachToLog(LogList log)
    {
      base.AttachToLog(log);
      log.Loan.LoanHistoryMonitor.TrackChange((LogRecordBase) this, "Condition created");
      this.dayCountSetting = log.Loan.DocumentDateTimeType;
      this.calculateExpectedDate();
    }

    public override PipelineInfo.Alert[] GetPipelineAlerts()
    {
      return this.commentList.GetPipelineAlerts();
    }

    public override void ToXml(XmlElement e)
    {
      base.ToXml(e);
      AttributeWriter attributeWriter = new AttributeWriter(e);
      attributeWriter.Write("Title", (object) this.title);
      attributeWriter.Write("Description", (object) this.description);
      attributeWriter.Write("Details", (object) this.details);
      attributeWriter.Write("DaysTillDue", (object) this.daysTillDue);
      attributeWriter.Write("RequestedFrom", (object) this.requestedFrom);
      attributeWriter.Write("DateRequested", (object) this.dateRequested);
      attributeWriter.Write("RequestedBy", (object) this.requestedBy);
      attributeWriter.Write("DateRerequested", (object) this.dateRerequested);
      attributeWriter.Write("RerequestedBy", (object) this.rerequestedBy);
      attributeWriter.Write("DateReceived", (object) this.dateReceived);
      attributeWriter.Write("ReceivedBy", (object) this.receivedBy);
      attributeWriter.Write("PairId", (object) this.pairId);
      attributeWriter.Write("DateAdded", (object) this.dateAdded);
      attributeWriter.Write("AddedBy", (object) this.addedBy);
      attributeWriter.Write("Source", (object) this.source);
      attributeWriter.Write("ProviderURN", (object) this.providerUrn);
      this.commentList.ToXml(e, "Comments");
    }
  }
}
