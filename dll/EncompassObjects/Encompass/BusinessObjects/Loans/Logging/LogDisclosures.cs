// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.LogDisclosures
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine.Log;
using EllieMae.Encompass.BusinessObjects.Users;
using System;
using System.Collections;
using System.Linq;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  public class LogDisclosures : LoanLogEntryCollection, ILogDisclosures, IEnumerable
  {
    internal LogDisclosures(Loan loan)
      : base(loan, typeof (DisclosureTrackingLog))
    {
    }

    public Disclosure this[int index] => (Disclosure) this.LogEntries[index];

    public Disclosure Add(DateTime disclosureDate, StandardDisclosureType disclosureType)
    {
      DisclosureTrackingLog record = new DisclosureTrackingLog(DateTime.Now, this.Loan.Unwrap().LoanData, (disclosureType & StandardDisclosureType.GFE) > StandardDisclosureType.None, (disclosureType & StandardDisclosureType.TIL) > StandardDisclosureType.None, true, (disclosureType & StandardDisclosureType.SAFEHARBOR) > StandardDisclosureType.None);
      ((DisclosureTrackingBase) record).DisclosedBy = this.Loan.Session.UserID;
      ((DisclosureTrackingBase) record).DisclosedByFullName = this.Loan.Session.GetUserInfo().FullName;
      ((DisclosureTrackingBase) record).IsLocked = true;
      ((DisclosureTrackingBase) record).DisclosedDate = disclosureDate;
      ((DisclosureTrackingBase) record).BorrowerPairID = this.Loan.BorrowerPairs.Current.Unwrap().Id;
      return (Disclosure) this.CreateEntry((LogRecordBase) record);
    }

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
      ((DisclosureTrackingBase) record).DisclosedBy = disclosedBy.ID;
      ((DisclosureTrackingBase) record).DisclosedByFullName = disclosedBy.FullName;
      ((DisclosureTrackingBase) record).BorrowerPairID = this.Loan.BorrowerPairs.Current.Unwrap().Id;
      return (Disclosure) this.CreateEntry((LogRecordBase) record);
    }

    public Disclosure GetMostRecentDisclosure()
    {
      return this.Cast<Disclosure>().OrderByDescending<Disclosure, DateTime>((Func<Disclosure, DateTime>) (dis => dis.Date)).FirstOrDefault<Disclosure>();
    }

    public Disclosure GetMostRecentStandardDisclosure(StandardDisclosureType disclosureType)
    {
      return this.Cast<Disclosure>().Where<Disclosure>((Func<Disclosure, bool>) (dis => (dis.DisclosureType & disclosureType) == disclosureType)).OrderByDescending<Disclosure, DateTime>((Func<Disclosure, DateTime>) (dis => dis.Date)).FirstOrDefault<Disclosure>();
    }

    public bool RefreshEDisclosurePackageStatuses()
    {
      return this.Loan.Unwrap().SyncAllEDisclosurePackageStatuses(true);
    }

    internal override LogEntry Wrap(LogRecordBase logRecord)
    {
      return (LogEntry) new Disclosure(this.Loan, (DisclosureTrackingLog) logRecord);
    }
  }
}
