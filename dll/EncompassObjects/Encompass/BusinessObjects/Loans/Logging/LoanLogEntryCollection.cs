// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.LoanLogEntryCollection
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine.Log;
using EllieMae.Encompass.Collections;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
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

    public Loan Loan => this.loan;

    public int Count => this.entries.Count;

    internal LogEntryList LogEntries => this.entries;

    internal virtual bool IsRecordOfType(LogRecordBase logRecord)
    {
      return logRecord != null && logRecord.GetType().Equals(this.logRecordType);
    }

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

    internal LogEntry CreateEntry(LogRecordBase record)
    {
      this.loan.LoanData.GetLogList().AddRecord(record);
      return this.Find(record, true);
    }

    internal void RemoveEntry(LogEntry entry)
    {
      if (entry == null)
        throw new ArgumentNullException("Specified log entry cannot be null");
      if (!this.entries.Contains(entry))
        throw new InvalidOperationException("The specified LogEntry does not belong to the current loan");
      this.Loan.LoanData.GetLogList().RemoveRecord(entry.Unwrap());
    }

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

    internal abstract LogEntry Wrap(LogRecordBase logRecord);

    public IEnumerator GetEnumerator() => this.entries.GetEnumerator();
  }
}
