// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.LockConfirmation
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine.Log;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  public class LockConfirmation : LogEntry, ILockConfirmation
  {
    private LockConfirmLog logItem;
    private LockRequest request;

    internal LockConfirmation(Loan loan, LockConfirmLog logItem)
      : base(loan, (LogRecordBase) logItem)
    {
      this.logItem = logItem;
    }

    public DateTime Date => this.logItem.DateTimeConfirmed;

    public override LogEntryType EntryType => LogEntryType.LockConfirmation;

    public bool AlertLO
    {
      get => this.logItem.AlertLoanOfficer;
      set => this.logItem.AlertLoanOfficer = value;
    }

    public object BuySideExpirationDate
    {
      get
      {
        return !(this.logItem.BuySideExpirationDate == DateTime.MinValue) ? (object) this.logItem.BuySideExpirationDate : (object) null;
      }
    }

    public object SellSideDeliveryDate
    {
      get
      {
        return !(this.logItem.SellSideDeliveryDate == DateTime.MinValue) ? (object) this.logItem.SellSideDeliveryDate : (object) null;
      }
    }

    public object SellSideExpirationDate
    {
      get
      {
        return !(this.logItem.SellSideExpirationDate == DateTime.MinValue) ? (object) this.logItem.SellSideExpirationDate : (object) null;
      }
    }

    public string ConfirmedBy => this.logItem.ConfirmedBy;

    public LockRequest LockRequest
    {
      get
      {
        if (this.request != null)
          return this.request;
        foreach (LockRequest lockRequest in (LoanLogEntryCollection) this.Loan.Log.LockRequests)
        {
          if (lockRequest.ID == this.logItem.RequestGUID)
          {
            this.request = lockRequest;
            break;
          }
        }
        return this.request;
      }
    }
  }
}
