// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.LogRecordBase
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Xml;
using System;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public abstract class LogRecordBase : IComparable, IIdentifiable
  {
    private string guid;
    private LogAlertList alerts;
    protected DateTime date;
    protected string comments;
    private LogList log;
    private bool isDirty;
    private bool isNew;
    private DateTime dateUpdated = DateTime.MinValue.Date;

    public LogRecordBase()
      : this(DateTime.MinValue.Date, "")
    {
    }

    public LogRecordBase(System.Guid guid)
      : this(DateTime.MinValue.Date, "", guid)
    {
    }

    public LogRecordBase(DateTime date, string comments)
      : this(date, comments, System.Guid.NewGuid())
    {
    }

    public LogRecordBase(DateTime date, string comments, System.Guid guid)
    {
      this.guid = guid.ToString();
      this.date = date;
      this.Comments = comments;
      this.alerts = new LogAlertList(this);
      this.log = (LogList) null;
      this.isDirty = true;
      this.isNew = true;
    }

    public LogRecordBase(LogList log)
      : this()
    {
      this.log = log;
      this.alerts = new LogAlertList(this);
    }

    public LogRecordBase(LogList log, XmlElement e)
    {
      this.log = log;
      AttributeReader attributeReader = new AttributeReader(e);
      this.guid = attributeReader.GetString(nameof (Guid));
      this.date = !this.ReadDateInUtc || !attributeReader.GetString(nameof (Date)).EndsWith("Z") ? attributeReader.GetDate(nameof (Date)) : attributeReader.GetUtcDate(nameof (Date));
      this.comments = attributeReader.GetString(nameof (Comments));
      this.dateUpdated = !this.ReadDateInUtc || !attributeReader.GetString("UpdatedDate").EndsWith("Z") ? attributeReader.GetDate("UpdatedDate") : attributeReader.GetUtcDate("UpdatedDate");
      if (this.guid == "")
        this.guid = System.Guid.NewGuid().ToString();
      this.alerts = new LogAlertList(this, e);
      this.isDirty = false;
      this.isNew = false;
    }

    protected virtual bool ReadDateInUtc => false;

    public string Guid => this.guid;

    public void SetGuid(System.Guid assignedGuid) => this.guid = assignedGuid.ToString();

    public void SetGuid(string guidString) => this.guid = guidString;

    public LogList Log => this.log;

    [CLSCompliant(false)]
    public virtual DateTime Date
    {
      get => this.date;
      set
      {
        this.date = value;
        this.MarkAsDirty();
      }
    }

    public virtual bool IsLoanOperationalLog => false;

    [CLSCompliant(false)]
    public string Comments
    {
      get => this.comments;
      set
      {
        this.comments = value;
        this.MarkAsDirty();
      }
    }

    public virtual bool IsDirty() => this.isDirty;

    public bool IsNew => this.isNew;

    public virtual bool DisplayInLog
    {
      get => true;
      set => throw new NotSupportedException("The specified log type cannot be hidden in the log.");
    }

    public bool IsRemoved => this.log.IsRecordRemoved(this.guid);

    public bool IsAttachedToLog => this.log != null;

    protected internal void TrackChange(string details)
    {
      if (this.log != null)
        this.log.Loan.LoanHistoryMonitor.TrackChange(this, details);
      this.MarkLastUpdated();
    }

    protected internal void TrackChange(string details, FileAttachmentReference fileRef)
    {
      if (this.log != null)
        this.log.Loan.LoanHistoryMonitor.TrackChange(this, details, fileRef);
      this.MarkLastUpdated();
    }

    protected internal void TrackChange(string details, LogRecordBase linkedEntry)
    {
      if (this.log != null && this.log.Loan != null && this.log.Loan.LoanHistoryMonitor != null)
        this.log.Loan.LoanHistoryMonitor.TrackChange(this, details, linkedEntry);
      this.MarkLastUpdated();
    }

    protected internal void TrackChanges(string[] detailsList)
    {
      if (this.log != null)
      {
        foreach (string details in detailsList)
          this.log.Loan.LoanHistoryMonitor.TrackChange(this, details);
      }
      this.MarkLastUpdated();
    }

    protected internal void MarkAsDirty() => this.MarkAsDirty((string) null);

    protected internal void MarkAsDirty(string vfield)
    {
      this.isDirty = true;
      this.OnChange(vfield);
    }

    protected internal void MarkAsDirtyNoContentChange() => this.isDirty = true;

    public virtual void MarkAsClean() => this.isDirty = false;

    public DateTime DateUpdated => this.dateUpdated;

    public void MarkLastUpdated()
    {
      this.dateUpdated = this.ReadDateInUtc ? DateTime.UtcNow : DateTime.Now;
      this.MarkAsDirty();
    }

    public LogAlertList AlertList => this.alerts;

    public virtual PipelineInfo.Alert[] GetPipelineAlerts() => new PipelineInfo.Alert[0];

    public virtual DateTime GetSortDate() => this.Date;

    internal virtual void AttachToLog(LogList log)
    {
      this.log = log;
      this.alerts.SetSystemIDs(log.Loan.SystemID);
    }

    internal virtual void RemoveReferencesTo(LogRecordBase record)
    {
    }

    internal virtual bool IncludeInLog() => this.Date != DateTime.MinValue;

    internal virtual bool IsSystemSpecific() => false;

    internal virtual bool SupportsLoanHistory() => false;

    internal void UnsetNew() => this.isNew = false;

    public virtual void ToXml(XmlElement e)
    {
      AttributeWriter attributeWriter = new AttributeWriter(e);
      attributeWriter.Write("Guid", (object) this.guid);
      if (this.date.Kind == DateTimeKind.Utc)
        attributeWriter.Write("Date", (object) this.date);
      else
        attributeWriter.Write("Date", (object) DateTime.SpecifyKind(this.date, DateTimeKind.Unspecified).ToString("yyyy-MM-dd HH:mm:ss.fff tt"));
      attributeWriter.Write("Comments", (object) this.comments);
      attributeWriter.Write("UpdatedDate", (object) this.dateUpdated, AttributeWriterOptions.IncludeMilliSeconds);
      this.alerts.ToXml(e);
    }

    protected void OnChange() => this.OnChange((string) null);

    protected void OnChange(string vfieldId)
    {
      if (this.log == null || this.log.Loan == null || !this.log.Loaded)
        return;
      this.log.Loan.NotifyLogRecordChanged(this);
      if (string.IsNullOrEmpty(vfieldId))
        return;
      this.log.Loan.TriggerCalculation(vfieldId, this.log.Loan.GetField(vfieldId));
    }

    internal virtual void OnRecordAdd()
    {
    }

    public override bool Equals(object obj)
    {
      return obj != null && !(this.GetType() != obj.GetType()) && (obj as LogRecordBase).guid == this.guid;
    }

    public override int GetHashCode() => this.guid.GetHashCode();

    public virtual int CompareTo(object obj)
    {
      if (!(obj is LogRecordBase logRecordBase))
        return -1;
      long num = this.GetSortDate().Ticks - logRecordBase.GetSortDate().Ticks;
      if (num < 0L)
        return -1;
      return num > 0L ? 1 : 0;
    }
  }
}
