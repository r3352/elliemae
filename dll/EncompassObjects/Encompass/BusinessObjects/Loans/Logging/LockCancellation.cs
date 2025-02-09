// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.LockCancellation
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine.Log;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  public class LockCancellation : LogEntry
  {
    private LockCancellationLog logItem;
    private LockCancellationRequest request;

    internal LockCancellation(Loan loan, LockCancellationLog logItem)
      : base(loan, (LogRecordBase) logItem)
    {
      this.logItem = logItem;
    }

    public override LogEntryType EntryType => LogEntryType.LockCancellation;

    public DateTime Date => this.logItem.DateTimeCancelled;

    public bool AlertLO
    {
      get => this.logItem.AlertLoanOfficer;
      set => this.logItem.AlertLoanOfficer = value;
    }

    public string CancelledBy => this.logItem.CancelledBy;

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
