// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.LogTrackedDocuments
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
  /// Represents the collection of <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.TrackedDocument" /> entries held within
  /// a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.LoanLog" />.
  /// </summary>
  /// <example>
  /// The following code adds a new TrackedDocument to every loan in the
  /// My Pipeline folder, sets the document's ordered day to today and allows
  /// 10 days for it to be received.
  /// <code>
  /// <![CDATA[
  /// using System;
  /// using System.IO;
  /// using EllieMae.Encompass.Client;
  /// using EllieMae.Encompass.BusinessObjects;
  /// using EllieMae.Encompass.BusinessObjects.Loans;
  /// using EllieMae.Encompass.BusinessObjects.Loans.Logging;
  /// 
  /// class LoanReader
  /// {
  ///    public static void Main()
  ///    {
  ///       // Open the session to the remote server
  ///       Session session = new Session();
  ///       session.Start("myserver", "mary", "maryspwd");
  /// 
  ///       // Get the "My Pipeline" folder
  ///       LoanFolder fol = session.Loans.Folders["My Pipeline"];
  /// 
  ///       // Retrieve the folder's contents
  ///       LoanIdentityList ids = fol.GetContents();
  /// 
  ///       // Open each loan in the folder and check the expected closing date
  ///       for (int i = 0; i < ids.Count; i++)
  ///       {
  ///          // Open and lock the next loan in the loop
  ///          Loan loan = fol.OpenLoan(ids[i].LoanName);
  ///          loan.Lock();
  /// 
  ///          // Add a new supporting document to the loan
  ///          TrackedDocument doc = loan.Log.TrackedDocuments.Add("My Custom Document",
  ///             session.Loans.Milestones.Submittal.Name);
  /// 
  ///          doc.OrderDate = DateTime.Now;
  ///          doc.DueDays = 10;
  /// 
  ///          // Close the loan
  ///          loan.Commit();
  ///          loan.Unlock();
  ///          loan.Close();
  ///       }
  /// 
  ///       // End the session to gracefully disconnect from the server
  ///       session.End();
  ///    }
  /// }
  /// ]]>
  /// </code>
  /// </example>
  public class LogTrackedDocuments : LoanLogEntryCollection, ILogTrackedDocuments, IEnumerable
  {
    internal LogTrackedDocuments(Loan loan)
      : base(loan, typeof (DocumentLog))
    {
    }

    /// <summary>
    /// Gets a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.TrackedDocument" /> from the collection based on its index.
    /// </summary>
    /// <example>
    /// The following code opens a loan and removes all supporting documents which
    /// have not been ordered.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// using EllieMae.Encompass.BusinessObjects.Loans.Logging;
    /// 
    /// class LoanReader
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Open and lock the next loan in the loop
    ///       Loan loan = session.Loans.Folders["My Pipeline"].OpenLoan("Sample");
    ///       loan.Lock();
    /// 
    ///       // Cache the supporting documents into an array
    ///       TrackedDocument[] docs = new TrackedDocument[loan.Log.TrackedDocuments.Count];
    /// 
    ///       for (int i = 0; i < loan.Log.TrackedDocuments.Count; i++)
    ///          docs[i] = loan.Log.TrackedDocuments[i];
    /// 
    ///       // Remove every supporting document from the loan
    ///       foreach (TrackedDocument doc in docs)
    ///          if (doc.OrderDate == null)
    ///             loan.Log.TrackedDocuments.Remove(doc);
    /// 
    ///       // Close the loan
    ///       loan.Commit();
    ///       loan.Unlock();
    ///       loan.Close();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public TrackedDocument this[int index] => (TrackedDocument) this.LogEntries[index];

    /// <summary>
    /// Adds a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.TrackedDocument" /> to the loan for the current borrower pair.
    /// </summary>
    /// <param name="title">The title of the new document. This value cannot
    /// be blank or null.</param>
    /// <param name="milestoneName">The name of the <see cref="T:EllieMae.Encompass.BusinessEnums.Milestone" />
    /// with which the document is associated. This value cannot be blank or null.</param>
    /// <returns>Returns the newly added <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.TrackedDocument" />
    /// object.</returns>
    /// <example>
    /// The following code adds a new TrackedDocument to every loan in the
    /// My Pipeline folder, sets the document's ordered day to today and allows
    /// 10 days for it to be received.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// using EllieMae.Encompass.BusinessObjects.Loans.Logging;
    /// 
    /// class LoanReader
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Get the "My Pipeline" folder
    ///       LoanFolder fol = session.Loans.Folders["My Pipeline"];
    /// 
    ///       // Retrieve the folder's contents
    ///       LoanIdentityList ids = fol.GetContents();
    /// 
    ///       // Open each loan in the folder and check the expected closing date
    ///       for (int i = 0; i < ids.Count; i++)
    ///       {
    ///          // Open and lock the next loan in the loop
    ///          Loan loan = fol.OpenLoan(ids[i].LoanName);
    ///          loan.Lock();
    /// 
    ///          // Add a new supporting document to the loan
    ///          TrackedDocument doc = loan.Log.TrackedDocuments.Add("My Custom Document",
    ///             session.Loans.Milestones.Submittal.Name);
    /// 
    ///          doc.OrderDate = DateTime.Now;
    ///          doc.DueDays = 10;
    /// 
    ///          // Close the loan
    ///          loan.Commit();
    ///          loan.Unlock();
    ///          loan.Close();
    ///       }
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
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

    /// <summary>
    /// Adds a new document based on an existing <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Templates.DocumentTemplate" />.
    /// </summary>
    /// <param name="template">The <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Templates.DocumentTemplate" /> on which the tracked document is based.</param>
    /// <param name="milestoneName">The name of the <see cref="T:EllieMae.Encompass.BusinessEnums.Milestone" />
    /// with which the document is associated. This value cannot be blank or null.</param>
    /// <returns>Returns the newly added <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.TrackedDocument" />
    /// object.</returns>
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

    /// <summary>
    /// Removes a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.TrackedDocument" /> from the log.
    /// </summary>
    /// <param name="docEntry">The <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.TrackedDocument" /> instance to be removed.
    /// The specified instance must belong to the current Loan object.</param>
    /// <example>
    /// The following code opens a loan and removes all supporting documents which
    /// have not been ordered.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// using EllieMae.Encompass.BusinessObjects.Loans.Logging;
    /// 
    /// class LoanReader
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Open and lock the next loan in the loop
    ///       Loan loan = session.Loans.Folders["My Pipeline"].OpenLoan("Sample");
    ///       loan.Lock();
    /// 
    ///       // Cache the supporting documents into an array
    ///       TrackedDocument[] docs = new TrackedDocument[loan.Log.TrackedDocuments.Count];
    /// 
    ///       for (int i = 0; i < loan.Log.TrackedDocuments.Count; i++)
    ///          docs[i] = loan.Log.TrackedDocuments[i];
    /// 
    ///       // Remove every supporting document from the loan
    ///       foreach (TrackedDocument doc in docs)
    ///          if (doc.OrderDate == null)
    ///             loan.Log.TrackedDocuments.Remove(doc);
    /// 
    ///       // Close the loan
    ///       loan.Commit();
    ///       loan.Unlock();
    ///       loan.Close();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public void Remove(TrackedDocument docEntry)
    {
      if (docEntry == null)
        throw new ArgumentNullException(nameof (docEntry));
      if (!this.LogEntries.Contains((LogEntry) docEntry))
        throw new InvalidOperationException("The specified supporting document does not belong to the current loan");
      this.Loan.EnsureExclusive();
      this.RemoveEntry((LogEntry) docEntry);
    }

    /// <summary>
    /// Gets the set of <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.TrackedDocument" /> instances associated with a given
    /// <see cref="T:EllieMae.Encompass.BusinessEnums.Milestone" />.
    /// </summary>
    /// <param name="milestoneName">The name of the milestone for which the documents
    /// are to be retrieved. This value is case insenstitive.</param>
    /// <returns>Returns the list of the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.TrackedDocument" /> objects for the
    /// given <see cref="T:EllieMae.Encompass.BusinessEnums.Milestone" />.</returns>
    /// <remarks>
    /// For a list of the names of the pre-defined Milestones which are part of every loan's
    /// lifetime sequence, see the Remarks section for the <see cref="T:EllieMae.Encompass.BusinessEnums.Milestones" />
    /// object.
    /// </remarks>
    /// <example>
    /// The following code opens a loan and displays all of the supporting documents
    /// associated with the loan's next milestone which have not yet been received.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// using EllieMae.Encompass.BusinessObjects.Loans.Logging;
    /// 
    /// class LoanReader
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Open the desired loan
    ///       Loan loan = session.Loans.Folders["My Pipeline"].OpenLoan("Sample");
    /// 
    ///       // Retrieve all of the supporting documents associated with the loan's
    ///       // next milestone.
    ///       LogEntryList docList =
    ///          loan.Log.TrackedDocuments.GetDocumentsForMilestone(loan.Log.MilestoneEvents.NextEvent.MilestoneName);
    /// 
    ///       // Loop over the list's contents and display any that have not been received
    ///       foreach (TrackedDocument doc in docList)
    ///          if (doc.ReceivedDate == null)
    ///             Console.WriteLine("The document \"" + doc.Title + "\" has not been received.");
    /// 
    ///       loan.Close();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
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

    /// <summary>
    /// Gets the set of <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.TrackedDocument" /> instances that have the specified title.
    /// </summary>
    /// <param name="title">The title of the documents to be retrieved. The comparison to the
    /// document titles will be done in a case insensitive manner.</param>
    /// <returns>Returns the list of the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.TrackedDocument" /> objects with the
    /// specified title.</returns>
    /// <example>
    /// The following code opens a loan and displays all of the credit report information
    /// associated with the loan.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// using EllieMae.Encompass.BusinessObjects.Loans.Logging;
    /// 
    /// class LoanReader
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Open the desired loan
    ///       Loan loan = session.Loans.Folders["My Pipeline"].OpenLoan("Sample");
    /// 
    ///       // Retrieve all of the supporting documents with the title "Credit Report"
    ///       LogEntryList docList =
    ///          loan.Log.TrackedDocuments.GetDocumentsByTitle("Credit Report");
    /// 
    ///       // Loop over the list's contents and display any that have not been received
    ///       foreach (TrackedDocument doc in docList)
    ///          if (doc.ReceivedDate == null)
    ///             Console.WriteLine("The document \"" + doc.Title + "\" has not been received.");
    /// 
    ///       loan.Close();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
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

    /// <summary>Determines if the record is a Document Log</summary>
    internal override bool IsRecordOfType(LogRecordBase logRecord) => logRecord is DocumentLog;

    /// <summary>Wraps a LogRecord in a LogEntry object.</summary>
    internal override LogEntry Wrap(LogRecordBase logRecord)
    {
      return (LogEntry) new TrackedDocument(this.Loan, (DocumentLog) logRecord);
    }
  }
}
