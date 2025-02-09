// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.LogLockCancellations
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine.Log;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  public class LogLockCancellations : LoanLogEntryCollection, IEnumerable
  {
    internal LogLockCancellations(Loan loan)
      : base(loan, typeof (LockCancellationLog))
    {
    }

    public LockCancellation this[int index] => (LockCancellation) this.LogEntries[index];

    public LockCancellation GetCurrent()
    {
      foreach (LockCancellation logEntry in (CollectionBase) this.LogEntries)
      {
        if (((LockRequestLog) logEntry.LockCancellationRequest.Unwrap()).LockRequestStatus == 10)
          return logEntry;
      }
      return (LockCancellation) null;
    }

    internal override LogEntry Wrap(LogRecordBase logRecord)
    {
      return (LogEntry) new LockCancellation(this.Loan, (LockCancellationLog) logRecord);
    }
  }
}
