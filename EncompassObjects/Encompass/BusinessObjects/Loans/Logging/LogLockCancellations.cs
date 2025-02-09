// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.LogLockCancellations
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.Log;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>
  /// Represents the collection of <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.LockCancellation">LockCancellations</see>
  /// held within a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.LoanLog" />.
  /// </summary>
  public class LogLockCancellations : LoanLogEntryCollection, IEnumerable
  {
    internal LogLockCancellations(Loan loan)
      : base(loan, typeof (LockCancellationLog))
    {
    }

    /// <summary>
    /// Gets a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.LockCancellation">LockCancellation</see>
    /// from the collection based on its index.
    /// </summary>
    public LockCancellation this[int index] => (LockCancellation) this.LogEntries[index];

    /// <summary>
    /// Returns the currently active lock cancellation,
    /// or null if there is no active lock cancellation.
    /// </summary>
    public LockCancellation GetCurrent()
    {
      foreach (LockCancellation logEntry in (CollectionBase) this.LogEntries)
      {
        if (((LockRequestLog) logEntry.LockCancellationRequest.Unwrap()).LockRequestStatus == RateLockRequestStatus.Cancelled)
          return logEntry;
      }
      return (LockCancellation) null;
    }

    /// <summary>Wraps a LogRecord in a LogEntry object.</summary>
    internal override LogEntry Wrap(LogRecordBase logRecord)
    {
      return (LogEntry) new LockCancellation(this.Loan, (LockCancellationLog) logRecord);
    }
  }
}
