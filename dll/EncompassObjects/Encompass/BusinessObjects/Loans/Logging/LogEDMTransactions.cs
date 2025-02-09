// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.LogEDMTransactions
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine.Log;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  public class LogEDMTransactions : LoanLogEntryCollection, ILogEDMTransactions, IEnumerable
  {
    internal LogEDMTransactions(Loan loan)
      : base(loan, typeof (EDMLog))
    {
    }

    public EDMTransaction this[int index] => (EDMTransaction) this.LogEntries[index];

    public void Remove(EDMTransaction txn) => this.RemoveEntry((LogEntry) txn);

    internal override LogEntry Wrap(LogRecordBase logRecord)
    {
      return (LogEntry) new EDMTransaction(this.Loan, (EDMLog) logRecord);
    }
  }
}
