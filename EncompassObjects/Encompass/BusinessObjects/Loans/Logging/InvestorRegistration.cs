// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.InvestorRegistration
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
  /// Encapsulates the registration data when a loan is registered with an investor.
  /// </summary>
  public class InvestorRegistration : LogEntry, IInvestorRegistration
  {
    private RegistrationLog regLog;

    internal InvestorRegistration(Loan loan, LogRecordBase logItem)
      : base(loan, logItem)
    {
      this.regLog = (RegistrationLog) logItem;
    }

    /// <summary>
    /// Gets the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.LogEntryType" /> for this object.
    /// </summary>
    public override LogEntryType EntryType => LogEntryType.InvestorRegistration;

    /// <summary>
    /// Gets the ID of the <see cref="T:EllieMae.Encompass.BusinessObjects.Users.User" /> that created the registration.
    /// </summary>
    public string RegisteredBy => this.regLog.RegisteredByID;

    /// <summary>
    /// Gets the name of the investor with which the loan was registered.
    /// </summary>
    public string InvestorName
    {
      get => this.regLog.InvestorName;
      set => this.regLog.InvestorName = value ?? "";
    }

    /// <summary>Gets the date on which the loan was registered.</summary>
    public DateTime DateRegistered
    {
      get => this.regLog.RegisteredDate;
      set => this.regLog.RegisteredDate = value;
    }

    /// <summary>
    /// Gets or sets the expiration date for the registration.
    /// </summary>
    /// <remarks>If there is no defined expiration date, this property will return <c>null</c>. Otherwise
    /// it will return a DateTime object for the expiration date.</remarks>
    public object ExpirationDate
    {
      get
      {
        return !(this.regLog.ExpiredDate == DateTime.MinValue) ? (object) this.regLog.ExpiredDate : (object) null;
      }
      set => this.regLog.ExpiredDate = value == null ? DateTime.MinValue : (DateTime) value;
    }

    /// <summary>Indicates if the registration is expired.</summary>
    public bool Expired => this.regLog.IsExpired;

    /// <summary>
    /// Indicates if the registration is the current, active registration for the loan.
    /// </summary>
    public bool Current => this.regLog.IsCurrent;

    /// <summary>
    /// Gets or sets the investor's reference number for the loan.
    /// </summary>
    public string ReferenceNumber
    {
      get => this.regLog.Reference;
      set => this.regLog.Reference = value ?? "";
    }
  }
}
