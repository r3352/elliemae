// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.LockCancellationRequest
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.Encompass.BusinessObjects.Users;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>Represents a lock cancellation request.</summary>
  /// <remarks>
  /// A LockCancellationRequest contains a snapshot of lock-related fields.
  /// See <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.LockRequest" /> for more info.
  /// </remarks>
  public class LockCancellationRequest : LogEntry
  {
    private LockRequestLog logItem;
    private LockRequestFields fields;
    private LockCancellation cancellation;

    internal LockCancellationRequest(Loan loan, LockRequestLog logItem)
      : base(loan, (LogRecordBase) logItem)
    {
      this.logItem = logItem;
    }

    /// <summary>
    /// Gets the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.LogEntryType" /> for this object.
    /// </summary>
    /// <remarks>This property will return LogEntryType.LockCancellationRequest.</remarks>
    public override LogEntryType EntryType => LogEntryType.LockCancellationRequest;

    /// <summary>
    /// Gets the date and time on which this cancellation request was made.
    /// </summary>
    public DateTime Date => this.logItem.DateTimeRequested;

    /// <summary>Gets the user who made the lock request.</summary>
    public string RequestedBy => this.logItem.RequestedBy ?? "";

    /// <summary>
    /// Determines if this cancellation request represents an active request.
    /// </summary>
    public bool IsActive()
    {
      return this.logItem.LockRequestStatus == RateLockRequestStatus.Requested && this.Cancellation == null;
    }

    /// <summary>
    /// Gets the snapshot fields for this lock cancellation request.
    /// </summary>
    /// <remarks>If changes are made to the values in the lock request snapshot, the
    /// <see cref="M:EllieMae.Encompass.BusinessObjects.Loans.Logging.LockRequestFields.CommitChanges" /> method must be called to save those changes
    /// into the loan file. Otherwise, those changes will not appear in the Encompass user interface
    /// or be saved as part of the loan.</remarks>
    public LockRequestFields Fields
    {
      get
      {
        if (this.fields == null)
          this.fields = new LockRequestFields(new LockRequest(this.Loan, this.logItem), this.logItem.GetLockRequestSnapshot());
        return this.fields;
      }
    }

    /// <summary>
    /// Gets the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.LockCancellation" /> which has been provided for this lock cancellation request.
    /// </summary>
    /// <remarks>If the request is in the Active state, then there is no lock cancellation
    /// and this property will always return <c>null</c>
    /// </remarks>
    public LockCancellation Cancellation
    {
      get
      {
        if (this.cancellation == null)
        {
          foreach (LockCancellation lockCancellation in (LoanLogEntryCollection) this.Loan.Log.LockCancellations)
          {
            if (lockCancellation.LockCancellationRequest == this && this.cancellation == null)
              this.cancellation = lockCancellation;
          }
        }
        return this.cancellation;
      }
    }

    /// <summary>Accepts the request, and cancels the lock.</summary>
    /// <param name="cancellingUser"></param>
    /// <returns></returns>
    public LockCancellation Accept(User cancellingUser)
    {
      if (this.logItem.LockRequestStatus != RateLockRequestStatus.Requested)
        throw new InvalidOperationException("The lock cancellation request must be active to accept the cancellation.");
      this.Fields.CommitChanges();
      if (cancellingUser == null)
        cancellingUser = this.Loan.Session.GetCurrentUser();
      this.Loan.Unwrap().CancelRateLock(this.logItem, this.Fields.FieldTable, cancellingUser.Unwrap());
      return this.Cancellation;
    }
  }
}
