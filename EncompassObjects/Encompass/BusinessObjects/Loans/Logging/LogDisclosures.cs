// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.LogDisclosures
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.DataEngine.Log;
using EllieMae.Encompass.BusinessObjects.Users;
using System;
using System.Collections;
using System.Linq;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>
  /// Represents the collection of <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.Disclosure" /> entries held within
  /// a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.LoanLog" />.
  /// </summary>
  public class LogDisclosures : LoanLogEntryCollection, ILogDisclosures, IEnumerable
  {
    internal LogDisclosures(Loan loan)
      : base(loan, typeof (DisclosureTrackingLog))
    {
    }

    /// <summary>
    /// Gets a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.Disclosure">Disclosure</see>
    /// from the collection based on its index.
    /// </summary>
    public Disclosure this[int index] => (Disclosure) this.LogEntries[index];

    /// <summary>Adds a Disclosure to the loan's log.</summary>
    /// <param name="disclosureDate">The date of the disclosure.</param>
    /// <param name="disclosureType">The type(s) of standard disclosures included in this disclosure.</param>
    /// <returns>Returns the newly added <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.Disclosure">Disclosure</see>
    /// object.</returns>
    /// <remarks>This method assumes that the user who made the disclosure is the
    /// same as the currently logged in user. To create an entry for a different
    /// user, use the <see cref="M:EllieMae.Encompass.BusinessObjects.Loans.Logging.LogDisclosures.AddForUser(System.DateTime,EllieMae.Encompass.BusinessObjects.Loans.Logging.StandardDisclosureType,EllieMae.Encompass.BusinessObjects.Users.User)" /> method.</remarks>
    public Disclosure Add(DateTime disclosureDate, StandardDisclosureType disclosureType)
    {
      DisclosureTrackingLog record = new DisclosureTrackingLog(DateTime.Now, this.Loan.Unwrap().LoanData, (disclosureType & StandardDisclosureType.GFE) > StandardDisclosureType.None, (disclosureType & StandardDisclosureType.TIL) > StandardDisclosureType.None, true, (disclosureType & StandardDisclosureType.SAFEHARBOR) > StandardDisclosureType.None);
      record.DisclosedBy = this.Loan.Session.UserID;
      record.DisclosedByFullName = this.Loan.Session.GetUserInfo().FullName;
      record.IsLocked = true;
      record.DisclosedDate = disclosureDate;
      record.BorrowerPairID = this.Loan.BorrowerPairs.Current.Unwrap().Id;
      return (Disclosure) this.CreateEntry((LogRecordBase) record);
    }

    /// <summary>Adds a Disclosure to the loan's log.</summary>
    /// <param name="disclosureDate">The date of the conversation.</param>
    /// <param name="disclosureType">The type(s) of standard disclosures included in this disclosure.</param>
    /// <param name="disclosedBy">The <see cref="T:EllieMae.Encompass.BusinessObjects.Users.User" /> who generated the disclosure. This
    /// value must match the currently logged in user unless the logged in user is
    /// an administrator.</param>
    /// <returns>Returns the newly added <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.Disclosure">Disclosure</see>
    /// object.</returns>
    public Disclosure AddForUser(
      DateTime disclosureDate,
      StandardDisclosureType disclosureType,
      User disclosedBy)
    {
      if (disclosedBy == null)
        throw new ArgumentNullException(nameof (disclosedBy));
      if (disclosedBy.ID != this.Loan.Session.UserID && !this.Loan.Session.Unwrap().GetUser().GetUserInfo().IsAdministrator())
        throw new InvalidOperationException("User does not have sufficient rights to perform this operation.");
      DisclosureTrackingLog record = new DisclosureTrackingLog(disclosureDate, this.Loan.Unwrap().LoanData, (disclosureType & StandardDisclosureType.GFE) > StandardDisclosureType.None, (disclosureType & StandardDisclosureType.TIL) > StandardDisclosureType.None, true, (disclosureType & StandardDisclosureType.SAFEHARBOR) > StandardDisclosureType.None);
      record.DisclosedBy = disclosedBy.ID;
      record.DisclosedByFullName = disclosedBy.FullName;
      record.BorrowerPairID = this.Loan.BorrowerPairs.Current.Unwrap().Id;
      return (Disclosure) this.CreateEntry((LogRecordBase) record);
    }

    /// <summary>
    /// Gets the most recent disclosure record from the collection
    /// </summary>
    public Disclosure GetMostRecentDisclosure()
    {
      return this.Cast<Disclosure>().OrderByDescending<Disclosure, DateTime>((Func<Disclosure, DateTime>) (dis => dis.Date)).FirstOrDefault<Disclosure>();
    }

    /// <summary>
    /// Gets the most recent disclosure record from the collection for a standard disclosure type.
    /// </summary>
    /// <remarks>
    /// If an enumeration value is passed in which represents multiple standard disclosure types
    /// (e.g. <see cref="F:EllieMae.Encompass.BusinessObjects.Loans.Logging.StandardDisclosureType.GFETIL" />), a disclosure must include all of the
    /// specified disclosures in order to be considered a match.
    /// </remarks>
    public Disclosure GetMostRecentStandardDisclosure(StandardDisclosureType disclosureType)
    {
      return this.Cast<Disclosure>().Where<Disclosure>((Func<Disclosure, bool>) (dis => (dis.DisclosureType & disclosureType) == disclosureType)).OrderByDescending<Disclosure, DateTime>((Func<Disclosure, DateTime>) (dis => dis.Date)).FirstOrDefault<Disclosure>();
    }

    /// <summary>
    /// Refreshes the status of all eDisclosure Packages that have been requested
    /// </summary>
    public bool RefreshEDisclosurePackageStatuses()
    {
      return this.Loan.Unwrap().SyncAllEDisclosurePackageStatuses(true);
    }

    /// <summary>Wraps a LogRecord in a LogEntry object.</summary>
    internal override LogEntry Wrap(LogRecordBase logRecord)
    {
      return (LogEntry) new Disclosure(this.Loan, (DisclosureTrackingLog) logRecord);
    }
  }
}
