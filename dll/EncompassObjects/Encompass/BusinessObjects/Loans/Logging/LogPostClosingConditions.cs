// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.LogPostClosingConditions
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
  public class LogPostClosingConditions : 
    LoanLogEntryCollection,
    ILogPostClosingConditions,
    IEnumerable
  {
    internal LogPostClosingConditions(Loan loan)
      : base(loan, typeof (PostClosingConditionLog))
    {
    }

    public PostClosingCondition this[int index] => (PostClosingCondition) this.LogEntries[index];

    public PostClosingCondition Add(string title)
    {
      if ((title ?? "") == "")
        throw new ArgumentException("Invalid document title", nameof (title));
      PostClosingConditionLog record = new PostClosingConditionLog(this.Loan.Session.UserID, this.Loan.BorrowerPairs.Current.Borrower.ID);
      ((ConditionLog) record).Title = title;
      return (PostClosingCondition) this.CreateEntry((LogRecordBase) record);
    }

    public PostClosingCondition AddFromTemplate(PostClosingConditionTemplate template)
    {
      PostClosingCondition closingCondition = template != null ? this.Add(template.Title) : throw new ArgumentNullException(nameof (template));
      closingCondition.DaysToReceive = template.DaysToReceive;
      closingCondition.Description = template.Description;
      closingCondition.Recipient = template.Recipient;
      closingCondition.Source = template.Source;
      return closingCondition;
    }

    public void Remove(PostClosingCondition cond) => this.RemoveEntry((LogEntry) cond);

    public LogEntryList GetConditionsByTitle(string title)
    {
      if ((title ?? "") == "")
        throw new ArgumentException(nameof (title));
      LogEntryList conditionsByTitle = new LogEntryList();
      foreach (PostClosingCondition closingCondition in (LoanLogEntryCollection) this)
      {
        if (string.Compare(closingCondition.Title, title, true) == 0)
          conditionsByTitle.Add((LogEntry) closingCondition);
      }
      return conditionsByTitle;
    }

    internal override LogEntry Wrap(LogRecordBase logRecord)
    {
      return (LogEntry) new PostClosingCondition(this.Loan, (PostClosingConditionLog) logRecord);
    }
  }
}
