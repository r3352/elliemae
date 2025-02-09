// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.LoanLog
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.DataEngine.Log;
using EllieMae.Encompass.Collections;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>
  /// Provides access to all of the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.LogEntry" /> objects
  /// associated with the current loan.
  /// </summary>
  /// <remarks>
  /// <p>The LoanLog is used to access all of the historical information
  /// (both past and future) for a loan as well as information regarding milestones,
  /// conditions, tasks, etc., each of which is represented by its own class in the
  /// Logging namespace. For each such class, the LoanLog exposes a property which allows
  /// access to the collection of records for that event type. For example,
  /// the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.MilestoneEvent" /> class represents a single milestone in the lifetime of the loan,
  /// and the collection of all MilestoneEvents can be accessed through the LoanLog's
  /// <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.LoanLog.MilestoneEvents" /> property.</p>
  /// </remarks>
  /// <example>
  /// The following code produces a report of all the loans in the My Pipeline folder
  /// which are currently waiting to be sent to processing.
  /// <code>
  /// <![CDATA[
  /// using System;
  /// using System.IO;
  /// using EllieMae.Encompass.Client;
  /// using EllieMae.Encompass.BusinessEnums;
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
  ///       // Get the Processing Milestone
  ///       Milestone processing = session.Loans.Milestones.Processing;
  /// 
  ///       // Open each loan in the folder and check the expected closing date
  ///       for (int i = 0; i < ids.Count; i++)
  ///       {
  ///          // Open the next loan in the loop
  ///          Loan loan = fol.OpenLoan(ids[i].LoanName);
  /// 
  ///          // Check if this is in the Processing stage
  ///          if ((loan.Log.MilestoneEvents.NextEvent != null) &&
  ///             (loan.Log.MilestoneEvents.NextEvent.MilestoneName == processing.Name))
  ///             Console.WriteLine("The loan \"" + loan.LoanName + "\" is waiting to be sent for processing.");
  /// 
  ///          // Close the loan
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
  public class LoanLog : ILoanLog
  {
    private Loan loan;
    private Dictionary<LogEntryType, LoanLogEntryCollection> logEntries = new Dictionary<LogEntryType, LoanLogEntryCollection>();

    internal LoanLog(Loan loan)
    {
      this.loan = loan;
      loan.OnLoanRefreshedFromServer += new EventHandler(this.Loan_OnLoanRefreshedFromServer);
      this.populateLogEntries(loan);
    }

    private void Loan_OnLoanRefreshedFromServer(object sender, EventArgs e)
    {
      this.populateLogEntries(this.loan);
    }

    private void populateLogEntries(Loan loan)
    {
      this.logEntries[LogEntryType.Conversation] = (LoanLogEntryCollection) new LogConversations(loan);
      this.logEntries[LogEntryType.EDMTransaction] = (LoanLogEntryCollection) new LogEDMTransactions(loan);
      this.logEntries[LogEntryType.InvestorRegistration] = (LoanLogEntryCollection) new LogInvestorRegistrations(loan);
      this.logEntries[LogEntryType.MilestoneTask] = (LoanLogEntryCollection) new LogMilestoneTasks(loan);
      this.logEntries[LogEntryType.LockConfirmation] = (LoanLogEntryCollection) new LogLockConfirmations(loan);
      this.logEntries[LogEntryType.LockDenial] = (LoanLogEntryCollection) new LogLockDenials(loan);
      this.logEntries[LogEntryType.LockRequest] = (LoanLogEntryCollection) new LogLockRequests(loan);
      this.logEntries[LogEntryType.LockCancellationRequest] = (LoanLogEntryCollection) new LogLockCancellationRequests(loan);
      this.logEntries[LogEntryType.LockCancellation] = (LoanLogEntryCollection) new LogLockCancellations(loan);
      this.logEntries[LogEntryType.MilestoneEvent] = (LoanLogEntryCollection) new LogMilestoneEvents(loan);
      this.logEntries[LogEntryType.PreliminaryCondition] = (LoanLogEntryCollection) new LogPreliminaryConditions(loan);
      this.logEntries[LogEntryType.PostClosingCondition] = (LoanLogEntryCollection) new LogPostClosingConditions(loan);
      this.logEntries[LogEntryType.PrintEvent] = (LoanLogEntryCollection) new LogPrintEvents(loan);
      this.logEntries[LogEntryType.ReceivedDownload] = (LoanLogEntryCollection) new LogReceivedDownloads(loan);
      this.logEntries[LogEntryType.StatusOnlineUpdate] = (LoanLogEntryCollection) new LogStatusOnlineUpdates(loan);
      this.logEntries[LogEntryType.HtmlEmailMessage] = (LoanLogEntryCollection) new LogHtmlEmailMessages(loan);
      this.logEntries[LogEntryType.TrackedDocument] = (LoanLogEntryCollection) new LogTrackedDocuments(loan);
      this.logEntries[LogEntryType.UnderwritingCondition] = (LoanLogEntryCollection) new LogUnderwritingConditions(loan);
      this.logEntries[LogEntryType.Disclosure] = (LoanLogEntryCollection) new LogDisclosures(loan);
      this.logEntries[LogEntryType.DocumentOrder] = (LoanLogEntryCollection) new LogDocumentOrders(loan);
      this.logEntries[LogEntryType.Disclosure2015] = (LoanLogEntryCollection) new LogDisclosures2015(loan);
    }

    /// <summary>
    /// Gets the set of all miletone-related events from the log.
    /// </summary>
    /// <remarks>The returned <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.LogMilestoneEvents" /> object can also
    /// be used to get information on the most recently completed milestone for
    /// the current loan.</remarks>
    /// <example>
    /// The following code produces a report of all the loans in the My Pipeline folder
    /// which are currently waiting to be sent to processing.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessEnums;
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
    ///       // Get the Processing Milestone
    ///       Milestone processing = session.Loans.Milestones.Processing;
    /// 
    ///       // Open each loan in the folder and check the expected closing date
    ///       for (int i = 0; i < ids.Count; i++)
    ///       {
    ///          // Open the next loan in the loop
    ///          Loan loan = fol.OpenLoan(ids[i].LoanName);
    /// 
    ///          // Check if this is in the Processing stage
    ///          if ((loan.Log.MilestoneEvents.NextEvent != null) &&
    ///             (loan.Log.MilestoneEvents.NextEvent.MilestoneName == processing.Name))
    ///             Console.WriteLine("The loan \"" + loan.LoanName + "\" is waiting to be sent for processing.");
    /// 
    ///          // Close the loan
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
    public LogMilestoneEvents MilestoneEvents
    {
      get => (LogMilestoneEvents) this.logEntries[LogEntryType.MilestoneEvent];
    }

    /// <summary>
    /// Gets the set of all <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.MilestoneTask" /> records in the log.
    /// </summary>
    public LogMilestoneTasks MilestoneTasks
    {
      get => (LogMilestoneTasks) this.logEntries[LogEntryType.MilestoneTask];
    }

    /// <summary>
    /// Gets the set of all document tracking entries from the log.
    /// </summary>
    /// <example>
    /// The following code adds a new TrackedDocument to every loan in the
    /// "My Pipeline" folder, sets the document's ordered day to today and allows
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
    public LogTrackedDocuments TrackedDocuments
    {
      get => (LogTrackedDocuments) this.logEntries[LogEntryType.TrackedDocument];
    }

    /// <summary>
    /// Gets the set of all conversation-related entries from the log.
    /// </summary>
    /// <example>
    /// The following code creates a new Conversation in a loan.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// using EllieMae.Encompass.BusinessObjects.Loans.Logging;
    /// using EllieMae.Encompass.BusinessObjects.Users;
    /// 
    /// class LoanReader
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server -- we must log in as an
    ///       // administrator in order to invoke AddForUser().
    ///       Session session = new Session();
    ///       session.Start("myserver", "admin", "adminpwd");
    /// 
    ///       // Open a loan and lock it for writing
    ///       Loan loan = session.Loans.Folders["My Pipeline"].OpenLoan("Sample");
    ///       loan.Lock();
    /// 
    ///       // Fetch the user we'll be adding the conversation for
    ///       User mary = session.Users.GetUser("mary");
    /// 
    ///       // Add a new conversation event to the log
    ///       Conversation conv = loan.Log.Conversations.AddForUser(DateTime.Now, mary);
    ///       conv.HeldWith = "James Hartley";
    ///       conv.Company = "Harley Appraisal Services";
    ///       conv.EmailAddress = "jhartl@hartleyappraisals.com";
    ///       conv.ContactMethod = ConversationContactMethod.Email;
    ///       conv.Comments = "Mary contacted James and scheduled the appraisal for 6/16";
    ///       conv.DisplayInLog = false;
    /// 
    ///       // Save the loan
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
    public LogConversations Conversations
    {
      get => (LogConversations) this.logEntries[LogEntryType.Conversation];
    }

    /// <summary>Gets the set of all print event entries from the log.</summary>
    public LogPrintEvents PrintEvents => (LogPrintEvents) this.logEntries[LogEntryType.PrintEvent];

    /// <summary>
    /// Gets the set of all log entries corresponding to received and improted downloads.
    /// </summary>
    public LogReceivedDownloads ReceivedDownloads
    {
      get => (LogReceivedDownloads) this.logEntries[LogEntryType.ReceivedDownload];
    }

    /// <summary>
    /// Gets the set of all EDM-related transactions that are recorded in the log.
    /// </summary>
    public LogEDMTransactions EDMTransactions
    {
      get => (LogEDMTransactions) this.logEntries[LogEntryType.EDMTransaction];
    }

    /// <summary>
    /// Gets the set of all updates made to the Status Online web site for the loan.
    /// </summary>
    public LogStatusOnlineUpdates StatusOnlineUpdates
    {
      get => (LogStatusOnlineUpdates) this.logEntries[LogEntryType.StatusOnlineUpdate];
    }

    /// <summary>Gets the set of all html messages sent for the loan</summary>
    public LogHtmlEmailMessages HtmlEmailMessages
    {
      get => (LogHtmlEmailMessages) this.logEntries[LogEntryType.HtmlEmailMessage];
    }

    /// <summary>
    /// Gets the set of all Underwriting Conditions on the loan.
    /// </summary>
    public LogUnderwritingConditions UnderwritingConditions
    {
      get => (LogUnderwritingConditions) this.logEntries[LogEntryType.UnderwritingCondition];
    }

    /// <summary>
    /// Gets the set of all Post-closing Conditions on the loan.
    /// </summary>
    public LogPostClosingConditions PostClosingConditions
    {
      get => (LogPostClosingConditions) this.logEntries[LogEntryType.PostClosingCondition];
    }

    /// <summary>
    /// Gets the set of all Preliminary Conditions on the loan.
    /// </summary>
    public LogPreliminaryConditions PreliminaryConditions
    {
      get => (LogPreliminaryConditions) this.logEntries[LogEntryType.PreliminaryCondition];
    }

    /// <summary>Gets the set of Investor Registrations from the loan.</summary>
    public LogInvestorRegistrations InvestorRegistrations
    {
      get => (LogInvestorRegistrations) this.logEntries[LogEntryType.InvestorRegistration];
    }

    /// <summary>Gets the set of all Lock Requests on the loan.</summary>
    public LogLockRequests LockRequests
    {
      get => (LogLockRequests) this.logEntries[LogEntryType.LockRequest];
    }

    /// <summary>Gets the set of all Lock Requests on the loan.</summary>
    public LogLockConfirmations LockConfirmations
    {
      get => (LogLockConfirmations) this.logEntries[LogEntryType.LockConfirmation];
    }

    /// <summary>Gets the set of all Lock Requests on the loan.</summary>
    public LogLockDenials LockDenials => (LogLockDenials) this.logEntries[LogEntryType.LockDenial];

    /// <summary>
    /// Gets the set of all Lock Cancellation Requests on the loan.
    /// </summary>
    public LogLockCancellationRequests LockCancellationRequests
    {
      get => (LogLockCancellationRequests) this.logEntries[LogEntryType.LockCancellationRequest];
    }

    /// <summary>Gets the set of all Lock Cancellations on the loan.</summary>
    public LogLockCancellations LockCancellations
    {
      get => (LogLockCancellations) this.logEntries[LogEntryType.LockCancellation];
    }

    /// <summary>
    /// Gets the set of all Disclosure Tracking records on the loan.
    /// </summary>
    public LogDisclosures Disclosures => (LogDisclosures) this.logEntries[LogEntryType.Disclosure];

    /// <summary>
    /// Gets the set of all Disclosure Tracking 2015 records on the loan.
    /// </summary>
    public LogDisclosures2015 Disclosures2015
    {
      get => (LogDisclosures2015) this.logEntries[LogEntryType.Disclosure2015];
    }

    /// <summary>
    /// Gets the set of all Document Order records on the loan.
    /// </summary>
    public LogDocumentOrders DocumentOrders
    {
      get => (LogDocumentOrders) this.logEntries[LogEntryType.DocumentOrder];
    }

    /// <summary>
    /// Retrieves a list of the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.LogEntry" /> objects in chronological order.
    /// </summary>
    /// <param name="entryTypes">A bitmask indicating the types of entries to
    /// be included in the returned set.</param>
    /// <returns>A <see cref="T:EllieMae.Encompass.Collections.LogEntryList" />
    /// containing the specified entries in chronological order (earliest to latest).</returns>
    /// <remarks>This function returns all log entrues of the specified type(s) ordered
    /// based by the <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.LogEntry.Date" /> values of the entries.
    /// </remarks>
    public LogEntryList GetLogEntrySequence(LogEntryType entryTypes)
    {
      LogEntryList logEntrySequence = new LogEntryList();
      foreach (LogEntryType key in this.logEntries.Keys)
      {
        if ((key & entryTypes) != (LogEntryType) 0)
        {
          foreach (LogEntry logEntry in this.logEntries[key])
            logEntrySequence.Add(logEntry);
        }
      }
      logEntrySequence.Sort((IComparer) new LogEntryDateSort());
      return logEntrySequence;
    }

    /// <summary>Gets a log entry using its unique ID.</summary>
    /// <param name="entryId">The ID of teh desired log entry.</param>
    /// <returns>The entry with the specified ID, or <c>null</c> if not found.</returns>
    public LogEntry GetEntryByID(string entryId)
    {
      LogRecordBase recordById = this.loan.LoanData.GetLogList().GetRecordByID(entryId);
      return recordById == null ? (LogEntry) null : this.Find(recordById, true);
    }

    internal LogEntry Find(LogRecordBase rec, bool addIfMissing)
    {
      foreach (LoanLogEntryCollection logEntryCollection in this.logEntries.Values)
      {
        if (logEntryCollection.IsRecordOfType(rec))
          return logEntryCollection.Find(rec, addIfMissing);
      }
      return (LogEntry) null;
    }

    internal void PurgeEntry(LogEntry e) => this.logEntries[e.EntryType].PurgeEntry(e.ID);

    internal LogEntry Wrap(LogRecordBase rec)
    {
      foreach (LoanLogEntryCollection logEntryCollection in this.logEntries.Values)
      {
        if (logEntryCollection.IsRecordOfType(rec))
          return logEntryCollection.Wrap(rec);
      }
      return (LogEntry) null;
    }
  }
}
