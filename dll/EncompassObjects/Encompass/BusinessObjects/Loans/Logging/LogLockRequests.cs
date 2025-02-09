// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.LogLockRequests
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.LoanUtils.RateLocks;
using EllieMae.Encompass.BusinessObjects.Users;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  public class LogLockRequests : LoanLogEntryCollection, ILogLockRequests, IEnumerable
  {
    internal LogLockRequests(Loan loan)
      : base(loan, typeof (LockRequestLog))
    {
    }

    public LockRequest this[int index] => (LockRequest) this.LogEntries[index];

    public LockRequest Add() => this.Add((User) null);

    public LockRequest Add(User requestingUser)
    {
      if (!this.Loan.Session.SessionObjects.ServerLicense.IsBankerEdition)
        throw new NotSupportedException("The specified operation is not supported by the current version of Encompass");
      if (requestingUser == null)
        requestingUser = this.Loan.Session.GetCurrentUser();
      LockRequestLog rateLockRequest;
      try
      {
        rateLockRequest = this.Loan.Unwrap().CreateRateLockRequest(requestingUser.Unwrap(), false);
      }
      catch (RateLockRejectedException ex)
      {
        throw new RejectedRateLockException(((Exception) ex).Message, (Exception) ex);
      }
      return this.Find((LogRecordBase) rateLockRequest, true) as LockRequest;
    }

    public LockRequest GetCurrent()
    {
      foreach (LockRequest logEntry in (CollectionBase) this.LogEntries)
      {
        if (logEntry.IsActive())
          return logEntry;
      }
      return (LockRequest) null;
    }

    public LockRequest GetLastConfirmedLockRequest()
    {
      DateTime dateTime = DateTime.MinValue;
      LockRequest confirmedLockRequest = (LockRequest) null;
      foreach (LockRequest logEntry in (CollectionBase) this.LogEntries)
      {
        LockConfirmation confirmation = logEntry.Confirmation;
        if (confirmation != null && confirmation.Date > dateTime)
        {
          dateTime = confirmation.Date;
          confirmedLockRequest = logEntry;
        }
      }
      return confirmedLockRequest;
    }

    internal override LogEntry Wrap(LogRecordBase logRecord)
    {
      return (LogEntry) new LockRequest(this.Loan, (LockRequestLog) logRecord);
    }

    internal override bool IsRecordOfType(LogRecordBase logRecord)
    {
      return base.IsRecordOfType(logRecord) && !((LockRequestLog) logRecord).IsLockCancellation;
    }
  }
}
