// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.LoanLogEntryCollection
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.DataEngine.Log;
using EllieMae.Encompass.Collections;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>
  /// Represents a collection of a single type of LogEntry objects from the loan.
  /// </summary>
  public abstract class LoanLogEntryCollection : IEnumerable
  {
    private Loan loan;
    private LogEntryList entries;
    private Type logRecordType;

    internal LoanLogEntryCollection(Loan loan, Type recordType)
    {
      this.loan = loan;
      this.logRecordType = recordType;
      this.entries = this.GetLogEntriesFromLoan(this.loan.LoanData.GetLogList());
    }

    /// <summary>
    /// Gets the <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.LoanLogEntryCollection.Loan" /> to which the collection belongs.
    /// </summary>
    public Loan Loan => this.loan;

    /// <summary>Gets the number of entries in the collection.</summary>
    public int Count => this.entries.Count;

    /// <summary>Accessor for the collection of LogEntry objects</summary>
    internal LogEntryList LogEntries => this.entries;

    /// <summary>
    /// Determines if the specified log record belongs to the collection.
    /// </summary>
    internal virtual bool IsRecordOfType(LogRecordBase logRecord)
    {
      return logRecord != null && logRecord.GetType().Equals(this.logRecordType);
    }

    /// <summary>
    /// Locates a LogRecord in a LogEntry if it's the appropriate type
    /// </summary>
    internal LogEntry Find(LogRecordBase logRecord, bool addIfMissing)
    {
      if (logRecord == null)
        return (LogEntry) null;
      if (!this.IsRecordOfType(logRecord))
        return (LogEntry) null;
      foreach (LogEntry entry in (CollectionBase) this.entries)
      {
        if (entry.ID == logRecord.Guid)
          return entry;
      }
      if (!addIfMissing)
        return (LogEntry) null;
      LogEntry logEntry = this.Wrap(logRecord);
      this.entries.Add(logEntry);
      return logEntry;
    }

    /// <summary>Creates a new LogEntry and adds it to the loan.</summary>
    internal LogEntry CreateEntry(LogRecordBase record)
    {
      this.loan.LoanData.GetLogList().AddRecord(record);
      return this.Find(record, true);
    }

    /// <summary>Creates a new LogEntry and adds it to the loan.</summary>
    internal void RemoveEntry(LogEntry entry)
    {
      if (entry == null)
        throw new ArgumentNullException("Specified log entry cannot be null");
      if (!this.entries.Contains(entry))
        throw new InvalidOperationException("The specified LogEntry does not belong to the current loan");
      this.Loan.LoanData.GetLogList().RemoveRecord(entry.Unwrap());
    }

    /// <summary>
    /// Purges an entry from the collection when the underlying record is deleted.
    /// </summary>
    internal void PurgeEntry(string guid)
    {
      foreach (LogEntry entry in (CollectionBase) this.entries)
      {
        if (entry.ID == guid)
        {
          this.entries.Remove(entry);
          entry.Dispose();
          break;
        }
      }
    }

    /// <summary>Loads the log entries into a LogEntryList</summary>
    internal virtual LogEntryList GetLogEntriesFromLoan(LogList log)
    {
      LogEntryList logEntriesFromLoan = new LogEntryList();
      foreach (LogRecordBase logRecord in log.GetAllRecordsOfType(this.logRecordType))
      {
        if (this.IsRecordOfType(logRecord))
        {
          LogEntry logEntry = this.Wrap(logRecord);
          if (logEntry != null)
            logEntriesFromLoan.Add(logEntry);
        }
      }
      logEntriesFromLoan.Sort((IComparer) new LogEntryDateSort());
      return logEntriesFromLoan;
    }

    /// <summary>Wraps a LogRecord in a LogEntry object</summary>
    internal abstract LogEntry Wrap(LogRecordBase logRecord);

    /// <summary>Provides a enumeration interface for the collection</summary>
    public IEnumerator GetEnumerator() => this.entries.GetEnumerator();
  }
}
