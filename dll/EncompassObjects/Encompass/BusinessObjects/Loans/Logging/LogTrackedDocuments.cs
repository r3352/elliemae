// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.LogTrackedDocuments
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
  public class LogTrackedDocuments : LoanLogEntryCollection, ILogTrackedDocuments, IEnumerable
  {
    internal LogTrackedDocuments(Loan loan)
      : base(loan, typeof (DocumentLog))
    {
    }

    public TrackedDocument this[int index] => (TrackedDocument) this.LogEntries[index];

    public TrackedDocument Add(string title, string milestoneName)
    {
      if ((title ?? "") == "")
        throw new ArgumentException("Invalid document title", nameof (title));
      if ((milestoneName ?? "") == "")
        throw new ArgumentException("Invalid milestone name", nameof (milestoneName));
      return (TrackedDocument) this.CreateEntry((LogRecordBase) new DocumentLog(this.Loan.Session.SessionObjects.UserID, this.Loan.BorrowerPairs.Current.Borrower.ID)
      {
        Title = title,
        Stage = (this.Loan.Log.MilestoneEvents.GetEventForMilestone(milestoneName) ?? throw new ArgumentException("Loan does not contain milestone with name \"" + milestoneName + "\"")).InternalName
      });
    }

    public TrackedDocument AddFromTemplate(DocumentTemplate template, string milestoneName)
    {
      if (template == null)
        throw new ArgumentNullException(nameof (template));
      TrackedDocument trackedDocument = this.Add(template.Title, milestoneName);
      trackedDocument.DueDays = template.DaysToReceive;
      trackedDocument.ExpirationDays = template.DaysToExpire;
      trackedDocument.IncludeInEDisclosurePackage = template.IncludeInEDisclosurePackage;
      return trackedDocument;
    }

    public void Remove(TrackedDocument docEntry)
    {
      if (docEntry == null)
        throw new ArgumentNullException(nameof (docEntry));
      if (!this.LogEntries.Contains((LogEntry) docEntry))
        throw new InvalidOperationException("The specified supporting document does not belong to the current loan");
      this.Loan.EnsureExclusive();
      this.RemoveEntry((LogEntry) docEntry);
    }

    public LogEntryList GetDocumentsForMilestone(string milestoneName)
    {
      if ((milestoneName ?? "") == "")
        throw new ArgumentException(nameof (milestoneName));
      LogEntryList documentsForMilestone = new LogEntryList();
      foreach (TrackedDocument trackedDocument in (LoanLogEntryCollection) this)
      {
        if (string.Compare(trackedDocument.MilestoneName, milestoneName, true) == 0)
          documentsForMilestone.Add((LogEntry) trackedDocument);
      }
      return documentsForMilestone;
    }

    public LogEntryList GetDocumentsByTitle(string title)
    {
      if ((title ?? "") == "")
        throw new ArgumentException(nameof (title));
      LogEntryList documentsByTitle = new LogEntryList();
      foreach (TrackedDocument trackedDocument in (LoanLogEntryCollection) this)
      {
        if (string.Compare(trackedDocument.Title, title, true) == 0)
          documentsByTitle.Add((LogEntry) trackedDocument);
      }
      return documentsByTitle;
    }

    internal override bool IsRecordOfType(LogRecordBase logRecord) => logRecord is DocumentLog;

    internal override LogEntry Wrap(LogRecordBase logRecord)
    {
      return (LogEntry) new TrackedDocument(this.Loan, (DocumentLog) logRecord);
    }
  }
}
