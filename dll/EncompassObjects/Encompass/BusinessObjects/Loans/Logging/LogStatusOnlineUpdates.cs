// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.LogStatusOnlineUpdates
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine.Log;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  public class LogStatusOnlineUpdates : LoanLogEntryCollection, ILogStatusOnlineUpdates, IEnumerable
  {
    internal LogStatusOnlineUpdates(Loan loan)
      : base(loan, typeof (StatusOnlineLog))
    {
    }

    public StatusOnlineUpdate this[int index] => (StatusOnlineUpdate) this.LogEntries[index];

    public void Remove(StatusOnlineUpdate txn) => this.RemoveEntry((LogEntry) txn);

    internal override LogEntry Wrap(LogRecordBase logRecord)
    {
      return (LogEntry) new StatusOnlineUpdate(this.Loan, (StatusOnlineLog) logRecord);
    }
  }
}
