// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.LogEntry
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.DataEngine.Log;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>
  /// A LogEntry represents a single item in a loan's <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.LoanLog" />. This
  /// class is an abstract base class for all possible entries that can be stored
  /// in the log.
  /// </summary>
  /// <remarks>LogEntry instances become invalid
  /// when the Loan's Refresh method is invoked. Attempting
  /// to access this object after invoking refresh will result in an
  /// exception.
  /// </remarks>
  public abstract class LogEntry : ILogEntry
  {
    private Loan loan;
    private LogRecordBase logItem;
    private LogAlerts alerts;

    internal LogEntry(Loan loan, LogRecordBase logItem)
    {
      this.loan = loan;
      this.logItem = logItem;
    }

    /// <summary>Gets the unique indentifier for this log entry.</summary>
    public string ID
    {
      get
      {
        this.EnsureValid();
        return this.logItem.Guid;
      }
    }

    /// <summary>
    /// Gets the relevant date of the LogEntry. This date is used to sequence
    /// the entry in the loan's <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.LoanLog" />.
    /// </summary>
    /// <remarks>This date may represent different values dependeing on the derived
    /// class implementation. For example, the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.Conversation" /> entry type uses
    /// this property to represent that date on which the conversation occurred. The
    /// <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.TrackedDocument" /> class uses this field to indicate either the date on
    /// which the associated document is due to be received or the actual date it was received.
    /// <p>Depending on the derived class's implementation, this date may be null if, for example,
    /// the log entry represents an event which has not yet
    /// been sequenced into the loan's lifetime. For example, if a loan has not yet been
    /// sent to processing, the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.MilestoneEvent" /> for the Completion milestone will
    /// have a null Date. Once the loan is sent for processing, this date will be set according
    /// to the rules defined for the current Encompass Server.</p>
    /// </remarks>
    public object Date
    {
      get
      {
        this.EnsureValid();
        DateTime dateTime = this.logItem.Date;
        int year1 = dateTime.Year;
        dateTime = DateTime.MaxValue;
        int year2 = dateTime.Year;
        if (year1 == year2)
          return (object) null;
        return this.logItem.Date.Year == DateTime.MinValue.Year ? (object) null : (object) this.logItem.Date;
      }
    }

    /// <summary>
    /// Gets or sets the comments associated with the LogEntry.
    /// </summary>
    public string Comments
    {
      get
      {
        this.EnsureValid();
        return this.logItem.Comments;
      }
      set
      {
        this.EnsureEditable();
        this.logItem.Comments = value ?? "";
      }
    }

    /// <summary>
    /// Gets the set of <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.LogAlert" /> objects associated with the log entry.
    /// </summary>
    public LogAlerts RoleAlerts
    {
      get
      {
        this.EnsureValid();
        if (this.alerts == null)
          this.alerts = new LogAlerts(this);
        return this.alerts;
      }
    }

    /// <summary>
    /// Indicates is this log entry should be treated as past due or otherwise an alert.
    /// </summary>
    public virtual bool IsAlert => false;

    /// <summary>Gets the type of entry represented by this object.</summary>
    public abstract LogEntryType EntryType { get; }

    internal Loan Loan => this.loan;

    internal LogRecordBase Unwrap()
    {
      this.EnsureValid();
      return this.logItem;
    }

    internal void EnsureEditable() => this.EnsureValid();

    internal void EnsureValid()
    {
      if (this.logItem == null)
        throw new ObjectDisposedException("The object reference is no longer associated with a valid Loan instance.");
    }

    internal virtual void Dispose() => this.logItem = (LogRecordBase) null;
  }
}
