// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.LockDenial
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.DataEngine.Log;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>
  /// Provides the details of a rate lock denial in response to a request.
  /// </summary>
  public class LockDenial : LogEntry, ILockDenial
  {
    private LockDenialLog logItem;
    private LockRequest request;

    internal LockDenial(Loan loan, LockDenialLog logItem)
      : base(loan, (LogRecordBase) logItem)
    {
      this.logItem = logItem;
    }

    /// <summary>
    /// Gets the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.LogEntryType" /> for this object.
    /// </summary>
    /// <remarks>This property will return LogEntryType.LockDenial.</remarks>
    public override LogEntryType EntryType => LogEntryType.LockDenial;

    /// <summary>Gets the date and time at which the lock was denied.</summary>
    public DateTime Date => this.logItem.DateTimeDenied;

    /// <summary>
    /// Gets or sets a flag indicating if the loan's Loan Officer should be alerted.
    /// </summary>
    public bool AlertLO
    {
      get => this.logItem.AlertLoanOfficer;
      set => this.logItem.AlertLoanOfficer = value;
    }

    /// <summary>
    /// Gets the user ID of the user who denied the rate lock.
    /// </summary>
    public string DeniedBy => this.logItem.DeniedBy;

    /// <summary>
    /// Gets the <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.LockDenial.LockRequest" /> to which this denial applies.
    /// </summary>
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
