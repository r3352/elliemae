// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.LogLockCancellationRequests
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine.Log;
using EllieMae.Encompass.BusinessObjects.Users;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  public class LogLockCancellationRequests : LoanLogEntryCollection, IEnumerable
  {
    internal LogLockCancellationRequests(Loan loan)
      : base(loan, typeof (LockRequestLog))
    {
    }

    public LockCancellationRequest this[int index]
    {
      get => (LockCancellationRequest) this.LogEntries[index];
    }

    public LockCancellationRequest Add() => this.Add((User) null, "");

    public LockCancellationRequest Add(User requestingUser, string comment)
    {
      if (!this.Loan.Session.SessionObjects.ServerLicense.IsBankerEdition)
        throw new NotSupportedException("The specified operation is not supported by the current version of Encompass");
      if (requestingUser == null)
        requestingUser = this.Loan.Session.GetCurrentUser();
      return this.Find((LogRecordBase) (this.Loan.Unwrap().CreateRateLockCancellationRequest(requestingUser.Unwrap(), DateTime.Now, comment) ?? throw new InvalidOperationException("The cancellation request could not be created. This can happen if the loan does not have a currently active lock.")), true) as LockCancellationRequest;
    }

    public LockCancellationRequest GetCurrent()
    {
      foreach (LockCancellationRequest logEntry in (CollectionBase) this.LogEntries)
      {
        if (logEntry.IsActive())
          return logEntry;
      }
      return (LockCancellationRequest) null;
    }

    internal override LogEntry Wrap(LogRecordBase logRecord)
    {
      return (LogEntry) new LockCancellationRequest(this.Loan, (LockRequestLog) logRecord);
    }

    internal override bool IsRecordOfType(LogRecordBase logRecord)
    {
      return base.IsRecordOfType(logRecord) && ((LockRequestLog) logRecord).IsLockCancellation;
    }
  }
}
