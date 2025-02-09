// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.LogPreliminaryConditions
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
  /// Represents the collection of <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.PreliminaryCondition" /> entries held within
  /// a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.LoanLog" />.
  /// </summary>
  public class LogPreliminaryConditions : 
    LoanLogEntryCollection,
    ILogPreliminaryConditions,
    IEnumerable
  {
    internal LogPreliminaryConditions(Loan loan)
      : base(loan, typeof (PreliminaryConditionLog))
    {
    }

    /// <summary>
    /// Gets a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.PreliminaryCondition" /> from the collection based on its index.
    /// </summary>
    public PreliminaryCondition this[int index] => (PreliminaryCondition) this.LogEntries[index];

    /// <summary>
    /// Adds a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.PreliminaryCondition" /> to the loan for the current borrower pair.
    /// </summary>
    /// <param name="title">The title of the new document. This value cannot
    /// be blank or null.</param>
    /// <returns>Returns the newly added <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.PreliminaryCondition" />
    /// object.</returns>
    public PreliminaryCondition Add(string title)
    {
      if ((title ?? "") == "")
        throw new ArgumentException("Invalid document title", nameof (title));
      PreliminaryConditionLog record = new PreliminaryConditionLog(this.Loan.Session.UserID, this.Loan.BorrowerPairs.Current.Borrower.ID);
      record.Title = title;
      return (PreliminaryCondition) this.CreateEntry((LogRecordBase) record);
    }

    /// <summary>
    /// Removes a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.PreliminaryCondition" /> from the log.
    /// </summary>
    /// <param name="cond">The <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.PreliminaryCondition" /> instance to be removed.
    /// The specified instance must belong to the current Loan object.</param>
    public void Remove(PreliminaryCondition cond) => this.RemoveEntry((LogEntry) cond);

    /// <summary>
    /// Gets the set of <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.PreliminaryCondition" /> instances that have the specified title.
    /// </summary>
    /// <param name="title">The title of the conditions to be retrieved. The comparison to the
    /// document titles will be done in a case insensitive manner.</param>
    /// <returns>Returns the list of the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.PreliminaryCondition" /> objects with the
    /// specified title.</returns>
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

    /// <summary>Wraps a LogRecord in a LogEntry object.</summary>
    internal override LogEntry Wrap(LogRecordBase logRecord)
    {
      return (LogEntry) new PreliminaryCondition(this.Loan, (PreliminaryConditionLog) logRecord);
    }
  }
}
