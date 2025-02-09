// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.LockConfirmation
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.DataEngine.Log;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>Summary description for LockConfirmation.</summary>
  public class LockConfirmation : LogEntry, ILockConfirmation
  {
    private LockConfirmLog logItem;
    private LockRequest request;

    internal LockConfirmation(Loan loan, LockConfirmLog logItem)
      : base(loan, (LogRecordBase) logItem)
    {
      this.logItem = logItem;
    }

    /// <summary>Gets the date on which the confirmation was made.</summary>
    public DateTime Date => this.logItem.DateTimeConfirmed;

    /// <summary>
    /// Gets the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.LogEntryType" /> for this object.
    /// </summary>
    /// <remarks>Thie property will return LogEntryType.LockConfirmation.</remarks>
    public override LogEntryType EntryType => LogEntryType.LockConfirmation;

    /// <summary>
    /// Gets or sets a flag indicating if the loan's Loan Officer should be alerted.
    /// </summary>
    public bool AlertLO
    {
      get => this.logItem.AlertLoanOfficer;
      set => this.logItem.AlertLoanOfficer = value;
    }

    /// <summary>
    /// Gets or sets the Buy Side expiration date for the request.
    /// </summary>
    public object BuySideExpirationDate
    {
      get
      {
        return !(this.logItem.BuySideExpirationDate == DateTime.MinValue) ? (object) this.logItem.BuySideExpirationDate : (object) null;
      }
    }

    /// <summary>
    /// Gets or sets the sell-side delivery date on the request.
    /// </summary>
    public object SellSideDeliveryDate
    {
      get
      {
        return !(this.logItem.SellSideDeliveryDate == DateTime.MinValue) ? (object) this.logItem.SellSideDeliveryDate : (object) null;
      }
    }

    /// <summary>
    /// Gets or sets the sell-side expiration date on the request.
    /// </summary>
    public object SellSideExpirationDate
    {
      get
      {
        return !(this.logItem.SellSideExpirationDate == DateTime.MinValue) ? (object) this.logItem.SellSideExpirationDate : (object) null;
      }
    }

    /// <summary>
    /// Gets the user ID of the user who confirmed the rate lock.
    /// </summary>
    public string ConfirmedBy => this.logItem.ConfirmedBy;

    /// <summary>
    /// Gets the <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.LockConfirmation.LockRequest" /> to which this confirmation applies.
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
