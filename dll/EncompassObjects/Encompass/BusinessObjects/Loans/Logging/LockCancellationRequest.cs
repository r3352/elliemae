// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.LockCancellationRequest
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine.Log;
using EllieMae.Encompass.BusinessObjects.Users;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
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

    public override LogEntryType EntryType => LogEntryType.LockCancellationRequest;

    public DateTime Date => this.logItem.DateTimeRequested;

    public string RequestedBy => this.logItem.RequestedBy ?? "";

    public bool IsActive() => this.logItem.LockRequestStatus == 1 && this.Cancellation == null;

    public LockRequestFields Fields
    {
      get
      {
        if (this.fields == null)
          this.fields = new LockRequestFields(new LockRequest(this.Loan, this.logItem), this.logItem.GetLockRequestSnapshot());
        return this.fields;
      }
    }

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

    public LockCancellation Accept(User cancellingUser)
    {
      if (this.logItem.LockRequestStatus != 1)
        throw new InvalidOperationException("The lock cancellation request must be active to accept the cancellation.");
      this.Fields.CommitChanges();
      if (cancellingUser == null)
        cancellingUser = this.Loan.Session.GetCurrentUser();
      this.Loan.Unwrap().CancelRateLock(this.logItem, this.Fields.FieldTable, cancellingUser.Unwrap());
      return this.Cancellation;
    }
  }
}
