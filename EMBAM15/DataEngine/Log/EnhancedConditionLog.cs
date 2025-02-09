// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.EnhancedConditionLog
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class EnhancedConditionLog : ConditionLog
  {
    public static readonly string XmlType = "EnhancedCondition";
    private string enhancedConditionType;
    private EnhancedConditionDefinition definition;
    private string internalId;
    private string internalDescription;
    private bool? internalPrint;
    private bool? externalPrint;
    private string priorTo;
    private string externalId;
    private DateTime? externalPrintDate;
    private string externalDescription;
    private string recipient;
    private DateTime? startDate;
    private DateTime? endDate;
    private string requestedFrom;
    private int? daysToReceive;
    private string updatedBy;
    private StatusTrackingList trackings;
    private DateTime? publishedDate;
    private DateTime? documentReceiptDate;
    private EllieMae.EMLite.DataEngine.Log.SourceOfCondition? sourceOfCondition;
    private int? owner;
    private string partner;

    public EnhancedConditionLog()
    {
    }

    public EnhancedConditionLog(
      string conditionType,
      string title,
      string addedBy,
      string pairId,
      EnhancedConditionDefinition conditionDefinition,
      StatusTrackingList statusTracking)
    {
      this.enhancedConditionType = conditionType;
      this.title = title;
      this.addedBy = addedBy;
      this.dateAdded = DateTime.UtcNow;
      this.pairId = pairId;
      this.commentList = new CommentEntryCollection((LogRecordBase) this);
      this.trackings = statusTracking ?? new StatusTrackingList((LogRecordBase) this);
      this.definition = conditionDefinition ?? new EnhancedConditionDefinition();
    }

    public EnhancedConditionLog(LogList log, XmlElement e)
      : base(log, e)
    {
      AttributeReader attributeReader = new AttributeReader(e);
      this.enhancedConditionType = attributeReader.GetString(nameof (ConditionType));
      this.title = attributeReader.GetString("Title");
      this.internalId = attributeReader.GetString(nameof (InternalId));
      this.internalDescription = attributeReader.GetString(nameof (InternalDescription));
      this.externalId = attributeReader.GetString(nameof (ExternalId));
      this.externalDescription = attributeReader.GetString(nameof (ExternalDescription));
      this.publishedDate = attributeReader.GetNullableUtcDate(nameof (PublishedDate));
      this.documentReceiptDate = attributeReader.GetNullableUtcDate(nameof (DocumentReceiptDate));
      EllieMae.EMLite.DataEngine.Log.SourceOfCondition result;
      if (Enum.TryParse<EllieMae.EMLite.DataEngine.Log.SourceOfCondition>(attributeReader.GetString(nameof (SourceOfCondition)), out result))
        this.sourceOfCondition = new EllieMae.EMLite.DataEngine.Log.SourceOfCondition?(result);
      this.internalPrint = new bool?(attributeReader.GetBoolean(nameof (InternalPrint)));
      this.externalPrint = new bool?(attributeReader.GetBoolean(nameof (ExternalPrint)));
      this.externalPrintDate = attributeReader.GetNullableUtcDate(nameof (ExternalPrintDate));
      this.source = attributeReader.GetString("Source");
      this.pairId = attributeReader.GetString("PairId");
      this.category = attributeReader.GetString("Category");
      this.priorTo = attributeReader.GetString(nameof (PriorTo));
      this.recipient = attributeReader.GetString(nameof (Recipient));
      this.startDate = attributeReader.GetNullableUtcDate(nameof (StartDate));
      this.endDate = attributeReader.GetNullableUtcDate(nameof (EndDate));
      this.dateAdded = attributeReader.GetUtcDate("DateAdded");
      this.addedBy = attributeReader.GetString("AddedBy");
      this.updatedBy = attributeReader.GetString(nameof (UpdatedBy));
      this.requestedFrom = attributeReader.GetString(nameof (RequestedFrom));
      this.daysToReceive = attributeReader.GetNullableInteger(nameof (DaysToReceive));
      this.owner = attributeReader.GetNullableInteger(nameof (Owner));
      this.partner = attributeReader.GetString(nameof (Partner));
      this.commentList = new CommentEntryCollection((LogRecordBase) this, e, "Comments");
      this.trackings = new StatusTrackingList((LogRecordBase) this, e, nameof (Trackings));
      this.definition = new EnhancedConditionDefinition(e, nameof (Definitions));
    }

    public override void ToXml(XmlElement e)
    {
      base.ToXml(e);
      AttributeWriter attributeWriter = new AttributeWriter(e);
      attributeWriter.Write("Type", (object) EnhancedConditionLog.XmlType);
      attributeWriter.Write("ConditionType", (object) this.enhancedConditionType);
      attributeWriter.Write("Title", (object) this.title);
      attributeWriter.Write("InternalId", (object) this.internalId);
      attributeWriter.Write("InternalDescription", (object) this.internalDescription);
      attributeWriter.Write("ExternalId", (object) this.externalId);
      attributeWriter.Write("ExternalDescription", (object) this.externalDescription);
      attributeWriter.Write("PublishedDate", (object) this.publishedDate);
      attributeWriter.Write("DocumentReceiptDate", (object) this.documentReceiptDate);
      attributeWriter.Write("SourceOfCondition", (object) this.sourceOfCondition);
      attributeWriter.Write("InternalPrint", (object) this.internalPrint);
      attributeWriter.Write("ExternalPrint", (object) this.externalPrint);
      attributeWriter.Write("ExternalPrintDate", (object) this.externalPrintDate);
      attributeWriter.Write("Source", (object) this.source);
      attributeWriter.Write("PairId", (object) this.pairId);
      attributeWriter.Write("Category", (object) this.category);
      attributeWriter.Write("PriorTo", (object) this.priorTo);
      attributeWriter.Write("Recipient", (object) this.recipient);
      attributeWriter.Write("StartDate", (object) this.startDate);
      attributeWriter.Write("EndDate", (object) this.endDate);
      attributeWriter.Write("RequestedFrom", (object) this.requestedFrom);
      attributeWriter.Write("DaysToReceive", (object) this.daysToReceive);
      attributeWriter.Write("Owner", (object) this.owner);
      attributeWriter.Write("Partner", (object) this.partner);
      attributeWriter.Write("DateAdded", (object) this.dateAdded);
      attributeWriter.Write("AddedBy", (object) this.addedBy);
      attributeWriter.Write("UpdatedBy", (object) this.updatedBy);
      this.commentList.ToXml(e, "Comments");
      this.trackings.ToXml(e, "Trackings");
      this.definition.ToXml(e, "Definitions");
    }

    public string EnhancedConditionType
    {
      get => this.enhancedConditionType;
      set
      {
        if (!(this.enhancedConditionType != value))
          return;
        this.enhancedConditionType = value;
        this.TrackChange("Condition Type changed to \"" + this.enhancedConditionType + "\"");
      }
    }

    public string InternalId
    {
      get => this.internalId;
      set
      {
        if (!(this.internalId != value))
          return;
        this.internalId = value;
        this.TrackChange("Internal Id changed to \"" + this.internalId + "\"");
      }
    }

    public string InternalDescription
    {
      get => this.internalDescription;
      set
      {
        if (!(this.internalDescription != value))
          return;
        this.internalDescription = value;
        this.TrackChange("Internal Description changed to \"" + this.internalDescription + "\"");
      }
    }

    public string ExternalId
    {
      get => this.externalId;
      set
      {
        if (!(this.externalId != value))
          return;
        this.externalId = value;
        this.TrackChange("External Id changed to \"" + this.externalId + "\"");
      }
    }

    public string ExternalDescription
    {
      get => this.externalDescription;
      set
      {
        if (!(this.externalDescription != value))
          return;
        this.externalDescription = value;
        this.TrackChange("External Description changed to \"" + this.externalDescription + "\"");
      }
    }

    public bool? InternalPrint
    {
      get => this.internalPrint;
      set
      {
        bool? internalPrint = this.internalPrint;
        bool? nullable = value;
        if (internalPrint.GetValueOrDefault() == nullable.GetValueOrDefault() & internalPrint.HasValue == nullable.HasValue)
          return;
        this.internalPrint = value;
        this.TrackChange("Internal Print changed to \"" + (object) this.internalPrint + "\"");
      }
    }

    public bool? ExternalPrint
    {
      get => this.externalPrint;
      set
      {
        bool? externalPrint = this.externalPrint;
        bool? nullable = value;
        if (externalPrint.GetValueOrDefault() == nullable.GetValueOrDefault() & externalPrint.HasValue == nullable.HasValue)
          return;
        this.externalPrint = value;
        this.TrackChange("External Print changed to \"" + (object) this.externalPrint + "\"");
        this.UpdateExternalPrintDependentFields(value);
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
        this.TrackChange("Prior To changed to \"" + this.priorTo + "\"");
      }
    }

    public string Recipient
    {
      get => this.recipient;
      set
      {
        if (!(this.recipient != value))
          return;
        this.recipient = value;
        this.TrackChange("Recipient Description changed to \"" + this.recipient + "\"");
      }
    }

    public DateTime? StartDate
    {
      get => this.startDate;
      set
      {
        DateTime? startDate = this.startDate;
        DateTime? nullable = value;
        if ((startDate.HasValue == nullable.HasValue ? (startDate.HasValue ? (startDate.GetValueOrDefault() != nullable.GetValueOrDefault() ? 1 : 0) : 0) : 1) == 0)
          return;
        this.startDate = value;
        this.TrackChange("Start Date changed to \"" + value?.ToShortDateString() + "\"");
      }
    }

    public DateTime? EndDate
    {
      get => this.endDate;
      set
      {
        DateTime? endDate = this.endDate;
        DateTime? nullable = value;
        if ((endDate.HasValue == nullable.HasValue ? (endDate.HasValue ? (endDate.GetValueOrDefault() != nullable.GetValueOrDefault() ? 1 : 0) : 0) : 1) == 0)
          return;
        this.endDate = value;
        this.TrackChange("End Date changed to \"" + value?.ToShortDateString() + "\"");
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
        this.TrackChange("Requested From changed to \"" + this.requestedFrom + "\"");
      }
    }

    public int? DaysToReceive
    {
      get => this.daysToReceive;
      set
      {
        int? daysToReceive = this.daysToReceive;
        int? nullable = value;
        if (daysToReceive.GetValueOrDefault() == nullable.GetValueOrDefault() & daysToReceive.HasValue == nullable.HasValue)
          return;
        this.daysToReceive = value;
        this.TrackChange("Days To Receive changed to \"" + (object) this.daysToReceive + "\"");
      }
    }

    public string Status
    {
      get
      {
        List<StatusTrackingEntry> statusTrackingEntries = this.trackings.GetStatusTrackingEntries();
        return (statusTrackingEntries != null ? statusTrackingEntries.LastOrDefault<StatusTrackingEntry>()?.Status : (string) null) ?? ConditionStatus.Added.ToString();
      }
    }

    public DateTime? StatusDate
    {
      get
      {
        List<StatusTrackingEntry> statusTrackingEntries = this.trackings.GetStatusTrackingEntries();
        return new DateTime?((statusTrackingEntries != null ? statusTrackingEntries.LastOrDefault<StatusTrackingEntry>()?.Date : new DateTime?()) ?? this.DateAdded);
      }
    }

    public string StatusUser
    {
      get
      {
        List<StatusTrackingEntry> statusTrackingEntries = this.trackings.GetStatusTrackingEntries();
        return (statusTrackingEntries != null ? statusTrackingEntries.LastOrDefault<StatusTrackingEntry>()?.UserId : (string) null) ?? this.AddedBy;
      }
    }

    public bool StatusOpen => this.IsConditionStatusOpen();

    public virtual int? Age
    {
      get
      {
        DateTime? ageStartDate = this.AgeStartDate;
        if (!ageStartDate.HasValue)
          return new int?();
        DateTime d2 = this.AgeClosedDate ?? DateTime.UtcNow;
        return new int?(Utils.GetTotalTimeSpanDays(ageStartDate.GetValueOrDefault(), d2));
      }
    }

    public virtual DateTime? AgeStartDate => this.PublishedDate;

    public virtual DateTime? AgeClosedDate => this.CalculateAgeClosedDate();

    public string UpdatedBy
    {
      get => this.updatedBy;
      set
      {
        if (!(this.updatedBy != value))
          return;
        this.updatedBy = value;
        this.TrackChange("Updated By changed to \"" + this.updatedBy + "\"");
      }
    }

    public DateTime? ExternalPrintDate
    {
      get => this.externalPrintDate;
      set
      {
        DateTime? externalPrintDate = this.externalPrintDate;
        DateTime? nullable = value;
        if ((externalPrintDate.HasValue == nullable.HasValue ? (externalPrintDate.HasValue ? (externalPrintDate.GetValueOrDefault() != nullable.GetValueOrDefault() ? 1 : 0) : 0) : 1) == 0)
          return;
        this.externalPrintDate = value;
        this.TrackChange("External Print Date changed to \"" + (object) this.externalPrintDate + "\"");
      }
    }

    public DateTime? PublishedDate
    {
      get => this.publishedDate;
      set
      {
        DateTime? publishedDate = this.publishedDate;
        DateTime? nullable = value;
        if ((publishedDate.HasValue == nullable.HasValue ? (publishedDate.HasValue ? (publishedDate.GetValueOrDefault() != nullable.GetValueOrDefault() ? 1 : 0) : 0) : 1) == 0)
          return;
        this.publishedDate = value;
        this.TrackChange("Published Date changed to \"" + (object) this.publishedDate + "\"");
      }
    }

    public virtual DateTime? DocumentReceiptDate
    {
      get => this.documentReceiptDate;
      set
      {
        DateTime? documentReceiptDate = this.documentReceiptDate;
        DateTime? nullable = value;
        if ((documentReceiptDate.HasValue == nullable.HasValue ? (documentReceiptDate.HasValue ? (documentReceiptDate.GetValueOrDefault() != nullable.GetValueOrDefault() ? 1 : 0) : 0) : 1) == 0)
          return;
        this.documentReceiptDate = value;
        this.TrackChange("Document Receipt Date changed to \"" + value?.ToShortDateString() + "\"");
      }
    }

    public virtual EllieMae.EMLite.DataEngine.Log.SourceOfCondition? SourceOfCondition
    {
      get => this.sourceOfCondition;
      set
      {
        EllieMae.EMLite.DataEngine.Log.SourceOfCondition? sourceOfCondition = this.sourceOfCondition;
        EllieMae.EMLite.DataEngine.Log.SourceOfCondition? nullable = value;
        if (sourceOfCondition.GetValueOrDefault() == nullable.GetValueOrDefault() & sourceOfCondition.HasValue == nullable.HasValue)
          return;
        this.sourceOfCondition = value;
        this.TrackChange("Source Of Condition changed to \"" + (object) this.sourceOfCondition + "\"");
      }
    }

    public virtual int? Owner
    {
      get => this.owner;
      set
      {
        int? owner = this.owner;
        int? nullable = value;
        if (owner.GetValueOrDefault() == nullable.GetValueOrDefault() & owner.HasValue == nullable.HasValue)
          return;
        this.owner = value;
        this.TrackChange("Owner changed to \"" + (object) this.owner + "\"");
      }
    }

    public virtual string Partner
    {
      get => this.partner;
      set
      {
        if (!(this.partner != value))
          return;
        this.partner = value;
        this.TrackChange("Partner changed to \"" + this.partner + "\"");
      }
    }

    public EnhancedConditionDefinition Definitions => this.definition;

    public StatusTrackingList Trackings => this.trackings;

    public override ConditionType ConditionType => ConditionType.Enhanced;

    public DocumentLog[] GetLinkedDocuments(bool isExternalUser = false)
    {
      List<DocumentLog> documentLogList = new List<DocumentLog>();
      foreach (DocumentLog documentLog in ((IEnumerable<DocumentLog>) this.Log.GetAllDocuments()).Where<DocumentLog>((Func<DocumentLog, bool>) (c => !isExternalUser || c.IsTPOWebcenterPortal)))
      {
        if (documentLog.Conditions.Contains(this.Guid))
          documentLogList.Add(documentLog);
      }
      return documentLogList.ToArray();
    }

    private bool IsConditionStatusOpen()
    {
      List<StatusTrackingEntry> trackingRefs = this.trackings.GetStatusTrackingEntries();
      if (this.definition != null)
      {
        List<StatusTrackingEntry> source = trackingRefs;
        if ((source != null ? (!source.Any<StatusTrackingEntry>() ? 1 : 0) : 1) == 0)
        {
          StatusTrackingDefinition trackingDefinition = this.definition.TrackingDefinitions.FirstOrDefault<StatusTrackingDefinition>((Func<StatusTrackingDefinition, bool>) (td => string.Equals(td.Name, trackingRefs.LastOrDefault<StatusTrackingEntry>().Status, StringComparison.OrdinalIgnoreCase)));
          return trackingDefinition == null || trackingDefinition.Open;
        }
      }
      return true;
    }

    private void UpdateExternalPrintDependentFields(bool? externalPrint)
    {
      bool? nullable = externalPrint;
      bool flag = true;
      if (nullable.GetValueOrDefault() == flag & nullable.HasValue)
      {
        DateTime utcNow = DateTime.UtcNow;
        this.externalPrintDate = new DateTime?(this.externalPrintDate ?? utcNow);
        if (this.publishedDate.HasValue || this.Log.Loan == null || !(this.Log.Loan.GetField("CORRESPONDENT.X55") == "Y"))
          return;
        this.publishedDate = new DateTime?(utcNow);
      }
      else
      {
        if (!this.IsNew)
          return;
        this.publishedDate = new DateTime?();
        this.externalPrintDate = new DateTime?();
      }
    }

    private DateTime? CalculateAgeClosedDate()
    {
      List<StatusTrackingEntry> statusTrackingEntries = this.trackings.GetStatusTrackingEntries();
      DateTime? ageClosedDate = new DateTime?();
      if (!this.publishedDate.HasValue || this.StatusOpen || (statusTrackingEntries != null ? (!statusTrackingEntries.Any<StatusTrackingEntry>() ? 1 : 0) : 1) != 0)
        return ageClosedDate;
      IEnumerable<StatusTrackingDefinition> source1 = this.definition.TrackingDefinitions.Where<StatusTrackingDefinition>((Func<StatusTrackingDefinition, bool>) (c => !c.Open));
      IEnumerable<string> source2 = source1 != null ? source1.Select<StatusTrackingDefinition, string>((Func<StatusTrackingDefinition, string>) (c => c.Name)) : (IEnumerable<string>) null;
      for (int index = statusTrackingEntries.Count - 1; index >= 0; --index)
      {
        StatusTrackingEntry trackingEntry = statusTrackingEntries[index];
        if (source2 != null && source2.Any<string>((Func<string, bool>) (trackingDefName => string.Equals(trackingDefName, trackingEntry.Status, StringComparison.OrdinalIgnoreCase))))
          ageClosedDate = new DateTime?(trackingEntry.Date);
        else
          break;
      }
      return ageClosedDate;
    }

    public string UniqueKey => this.EnhancedConditionType + "-*-" + this.title;
  }
}
