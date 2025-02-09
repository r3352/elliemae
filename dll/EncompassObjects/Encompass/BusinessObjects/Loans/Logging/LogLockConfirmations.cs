// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.LogLockConfirmations
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine.Log;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  public class LogLockConfirmations : LoanLogEntryCollection, ILogLockConfirmations, IEnumerable
  {
    internal LogLockConfirmations(Loan loan)
      : base(loan, typeof (LockConfirmLog))
    {
    }

    public LockConfirmation this[int index] => (LockConfirmation) this.LogEntries[index];

    internal override LogEntry Wrap(LogRecordBase logRecord)
    {
      return (LogEntry) new LockConfirmation(this.Loan, (LockConfirmLog) logRecord);
    }
  }
}
