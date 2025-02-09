// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.LogUnderwritingConditions
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine.Log;
using EllieMae.Encompass.BusinessObjects.Loans.Templates;
using EllieMae.Encompass.Collections;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  public class LogUnderwritingConditions : 
    LoanLogEntryCollection,
    ILogUnderwritingConditions,
    IEnumerable
  {
    internal LogUnderwritingConditions(Loan loan)
      : base(loan, typeof (UnderwritingConditionLog))
    {
    }

    public UnderwritingCondition this[int index] => (UnderwritingCondition) this.LogEntries[index];

    public UnderwritingCondition Add(string title)
    {
      if ((title ?? "") == "")
        throw new ArgumentException("Invalid document title", nameof (title));
      UnderwritingConditionLog record = new UnderwritingConditionLog(this.Loan.Session.UserID, this.Loan.BorrowerPairs.Current.Borrower.ID);
      ((ConditionLog) record).Title = title;
      return (UnderwritingCondition) this.CreateEntry((LogRecordBase) record);
    }

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

    public void Remove(UnderwritingCondition cond) => this.RemoveEntry((LogEntry) cond);

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

    internal override LogEntry Wrap(LogRecordBase logRecord)
    {
      return (LogEntry) new UnderwritingCondition(this.Loan, (UnderwritingConditionLog) logRecord);
    }
  }
}
