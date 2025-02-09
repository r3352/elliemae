// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.LogUnderwritingConditions
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.DataEngine.Log;
using EllieMae.Encompass.BusinessObjects.Loans.Templates;
using EllieMae.Encompass.Collections;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>
  /// Represents the collection of <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.UnderwritingCondition" /> entries held within
  /// a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.LoanLog" />.
  /// </summary>
  public class LogUnderwritingConditions : 
    LoanLogEntryCollection,
    ILogUnderwritingConditions,
    IEnumerable
  {
    internal LogUnderwritingConditions(Loan loan)
      : base(loan, typeof (UnderwritingConditionLog))
    {
    }

    /// <summary>
    /// Gets a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.UnderwritingCondition" /> from the collection based on its index.
    /// </summary>
    public UnderwritingCondition this[int index] => (UnderwritingCondition) this.LogEntries[index];

    /// <summary>
    /// Adds a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.UnderwritingCondition" /> to the loan for the current borrower pair.
    /// </summary>
    /// <param name="title">The title of the new document. This value cannot
    /// be blank or null.</param>
    /// <returns>Returns the newly added <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.UnderwritingCondition" />
    /// object.</returns>
    public UnderwritingCondition Add(string title)
    {
      if ((title ?? "") == "")
        throw new ArgumentException("Invalid document title", nameof (title));
      UnderwritingConditionLog record = new UnderwritingConditionLog(this.Loan.Session.UserID, this.Loan.BorrowerPairs.Current.Borrower.ID);
      record.Title = title;
      return (UnderwritingCondition) this.CreateEntry((LogRecordBase) record);
    }

    /// <summary>
    /// Creates a new UnderwritingCondition based on the specified <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Templates.UnderwritingConditionTemplate" />.
    /// </summary>
    /// <param name="template">The <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Templates.UnderwritingConditionTemplate" /> from which the condition will
    /// be created.</param>
    /// <returns>Returns the newly added <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.UnderwritingCondition" />
    /// object.</returns>
    public UnderwritingCondition AddFromTemplate(UnderwritingConditionTemplate template)
    {
      UnderwritingCondition underwritingCondition = template != null ? this.Add(template.Title) : throw new ArgumentNullException(nameof (template));
      underwritingCondition.ForExternalUse = template.ForExternalUse;
      underwritingCondition.ForInternalUse = template.ForInternalUse;
      underwritingCondition.ForRole = template.ForRole;
      underwritingCondition.AllowToClear = template.AllowToClear;
      underwritingCondition.Description = template.Description;
      underwritingCondition.Category = template.Category;
      underwritingCondition.PriorTo = template.PriorTo;
      return underwritingCondition;
    }

    /// <summary>
    /// Removes a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.UnderwritingCondition" /> from the log.
    /// </summary>
    /// <param name="cond">The <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.UnderwritingCondition" /> instance to be removed.
    /// The specified instance must belong to the current Loan object.</param>
    public void Remove(UnderwritingCondition cond) => this.RemoveEntry((LogEntry) cond);

    /// <summary>
    /// Gets the set of <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.UnderwritingCondition" /> instances that have the specified title.
    /// </summary>
    /// <param name="title">The title of the documents to be retrieved. The comparison to the
    /// document titles will be done in a case insensitive manner.</param>
    /// <returns>Returns the list of the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.UnderwritingCondition" /> objects with the
    /// specified title.</returns>
    public LogEntryList GetConditionsByTitle(string title)
    {
      if ((title ?? "") == "")
        throw new ArgumentException(nameof (title));
      LogEntryList conditionsByTitle = new LogEntryList();
      foreach (UnderwritingCondition underwritingCondition in (LoanLogEntryCollection) this)
      {
        if (string.Compare(underwritingCondition.Title, title, true) == 0)
          conditionsByTitle.Add((LogEntry) underwritingCondition);
      }
      return conditionsByTitle;
    }

    /// <summary>Wraps a LogRecord in a LogEntry object.</summary>
    internal override LogEntry Wrap(LogRecordBase logRecord)
    {
      return (LogEntry) new UnderwritingCondition(this.Loan, (UnderwritingConditionLog) logRecord);
    }
  }
}
