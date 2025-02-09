// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.LockDenial
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine.Log;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  public class LockDenial : LogEntry, ILockDenial
  {
    private LockDenialLog logItem;
    private LockRequest request;

    internal LockDenial(Loan loan, LockDenialLog logItem)
      : base(loan, (LogRecordBase) logItem)
    {
      this.logItem = logItem;
    }

    public override LogEntryType EntryType => LogEntryType.LockDenial;

    public DateTime Date => this.logItem.DateTimeDenied;

    public bool AlertLO
    {
      get => this.logItem.AlertLoanOfficer;
      set => this.logItem.AlertLoanOfficer = value;
    }

    public string DeniedBy => this.logItem.DeniedBy;

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
