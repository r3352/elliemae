// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.LogPreliminaryConditions
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
  public class LogPreliminaryConditions : 
    LoanLogEntryCollection,
    ILogPreliminaryConditions,
    IEnumerable
  {
    internal LogPreliminaryConditions(Loan loan)
      : base(loan, typeof (PreliminaryConditionLog))
    {
    }

    public PreliminaryCondition this[int index] => (PreliminaryCondition) this.LogEntries[index];

    public PreliminaryCondition Add(string title)
    {
      if ((title ?? "") == "")
        throw new ArgumentException("Invalid document title", nameof (title));
      PreliminaryConditionLog record = new PreliminaryConditionLog(this.Loan.Session.UserID, this.Loan.BorrowerPairs.Current.Borrower.ID);
      ((ConditionLog) record).Title = title;
      return (PreliminaryCondition) this.CreateEntry((LogRecordBase) record);
    }

    public void Remove(PreliminaryCondition cond) => this.RemoveEntry((LogEntry) cond);

    public LogEntryList GetConditionsByTitle(string title)
    {
      if ((title ?? "") == "")
        throw new ArgumentException(nameof (title));
      LogEntryList conditionsByTitle = new LogEntryList();
      foreach (PreliminaryCondition preliminaryCondition in (LoanLogEntryCollection) this)
      {
        if (string.Compare(preliminaryCondition.Title, title, true) == 0)
          conditionsByTitle.Add((LogEntry) preliminaryCondition);
      }
      return conditionsByTitle;
    }

    internal override LogEntry Wrap(LogRecordBase logRecord)
    {
      return (LogEntry) new PreliminaryCondition(this.Loan, (PreliminaryConditionLog) logRecord);
    }
  }
}
