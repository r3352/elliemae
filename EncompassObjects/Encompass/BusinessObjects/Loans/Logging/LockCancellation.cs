// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.LockCancellation
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.DataEngine.Log;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>Provides the details of a rate lock cancellation.</summary>
  public class LockCancellation : LogEntry
  {
    private LockCancellationLog logItem;
    private LockCancellationRequest request;

    internal LockCancellation(Loan loan, LockCancellationLog logItem)
      : base(loan, (LogRecordBase) logItem)
    {
      this.logItem = logItem;
    }

    /// <summary>
    /// Gets the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.LogEntryType" /> for this object.
    /// </summary>
    /// <remarks>This property will return LogEntryType.LockCancellation.</remarks>
    public override LogEntryType EntryType => LogEntryType.LockCancellation;

    /// <summary>
    /// Gets the date and time at which the lock was cancelled.
    /// </summary>
    public DateTime Date => this.logItem.DateTimeCancelled;

    /// <summary>
    /// Gets or sets a flag indicating if the loan's Loan Officer should be alerted.
    /// </summary>
    public bool AlertLO
    {
      get => this.logItem.AlertLoanOfficer;
      set => this.logItem.AlertLoanOfficer = value;
    }

    /// <summary>
    /// Gets the user ID of the user who cancelled the rate lock.
    /// </summary>
    public string CancelledBy => this.logItem.CancelledBy;

    /// <summary>
    /// Gets the <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.LockCancellation.LockCancellationRequest" /> to which this cancellation applies.
    /// </summary>
    public LockCancellationRequest LockCancellationRequest
    {
      get
      {
        if (this.request != null)
          return this.request;
        foreach (LockCancellationRequest cancellationRequest in (LoanLogEntryCollection) this.Loan.Log.LockCancellationRequests)
        {
          if (cancellationRequest.ID == this.logItem.RequestGUID)
          {
            this.request = cancellationRequest;
            break;
          }
        }
        return this.request;
      }
    }
  }
}
