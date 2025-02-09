// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.LoanLog
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine.Log;
using EllieMae.Encompass.Collections;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
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

    public LogMilestoneEvents MilestoneEvents
    {
      get => (LogMilestoneEvents) this.logEntries[LogEntryType.MilestoneEvent];
    }

    public LogMilestoneTasks MilestoneTasks
    {
      get => (LogMilestoneTasks) this.logEntries[LogEntryType.MilestoneTask];
    }

    public LogTrackedDocuments TrackedDocuments
    {
      get => (LogTrackedDocuments) this.logEntries[LogEntryType.TrackedDocument];
    }

    public LogConversations Conversations
    {
      get => (LogConversations) this.logEntries[LogEntryType.Conversation];
    }

    public LogPrintEvents PrintEvents => (LogPrintEvents) this.logEntries[LogEntryType.PrintEvent];

    public LogReceivedDownloads ReceivedDownloads
    {
      get => (LogReceivedDownloads) this.logEntries[LogEntryType.ReceivedDownload];
    }

    public LogEDMTransactions EDMTransactions
    {
      get => (LogEDMTransactions) this.logEntries[LogEntryType.EDMTransaction];
    }

    public LogStatusOnlineUpdates StatusOnlineUpdates
    {
      get => (LogStatusOnlineUpdates) this.logEntries[LogEntryType.StatusOnlineUpdate];
    }

    public LogHtmlEmailMessages HtmlEmailMessages
    {
      get => (LogHtmlEmailMessages) this.logEntries[LogEntryType.HtmlEmailMessage];
    }

    public LogUnderwritingConditions UnderwritingConditions
    {
      get => (LogUnderwritingConditions) this.logEntries[LogEntryType.UnderwritingCondition];
    }

    public LogPostClosingConditions PostClosingConditions
    {
      get => (LogPostClosingConditions) this.logEntries[LogEntryType.PostClosingCondition];
    }

    public LogPreliminaryConditions PreliminaryConditions
    {
      get => (LogPreliminaryConditions) this.logEntries[LogEntryType.PreliminaryCondition];
    }

    public LogInvestorRegistrations InvestorRegistrations
    {
      get => (LogInvestorRegistrations) this.logEntries[LogEntryType.InvestorRegistration];
    }

    public LogLockRequests LockRequests
    {
      get => (LogLockRequests) this.logEntries[LogEntryType.LockRequest];
    }

    public LogLockConfirmations LockConfirmations
    {
      get => (LogLockConfirmations) this.logEntries[LogEntryType.LockConfirmation];
    }

    public LogLockDenials LockDenials => (LogLockDenials) this.logEntries[LogEntryType.LockDenial];

    public LogLockCancellationRequests LockCancellationRequests
    {
      get => (LogLockCancellationRequests) this.logEntries[LogEntryType.LockCancellationRequest];
    }

    public LogLockCancellations LockCancellations
    {
      get => (LogLockCancellations) this.logEntries[LogEntryType.LockCancellation];
    }

    public LogDisclosures Disclosures => (LogDisclosures) this.logEntries[LogEntryType.Disclosure];

    public LogDisclosures2015 Disclosures2015
    {
      get => (LogDisclosures2015) this.logEntries[LogEntryType.Disclosure2015];
    }

    public LogDocumentOrders DocumentOrders
    {
      get => (LogDocumentOrders) this.logEntries[LogEntryType.DocumentOrder];
    }

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
