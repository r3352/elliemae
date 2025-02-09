// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.LogDisclosures2015
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.DataEngine.Log;
using EllieMae.Encompass.BusinessObjects.Users;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>
  /// Represents the collection of <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.Disclosure2015" /> entries held within
  /// a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.LoanLog" />.
  /// </summary>
  public class LogDisclosures2015 : LoanLogEntryCollection, ILogDisclosures2015, IEnumerable
  {
    internal LogDisclosures2015(Loan loan)
      : base(loan, typeof (DisclosureTracking2015Log))
    {
    }

    /// <summary>Return Disclosure2015 object using index number</summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public Disclosure2015 this[int index] => (Disclosure2015) this.LogEntries[index];

    /// <summary>Adds a Disclosure to the loan's log.</summary>
    /// <param name="disclosureDate">The date of the disclosure.</param>
    /// <param name="disclosureType">The type(s) of standard disclosures included in this disclosure.</param>
    /// <returns>Returns the newly added <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.Disclosure2015">Disclosure</see>
    /// object.</returns>
    /// <remarks>This method assumes that the user who made the disclosure is the
    /// same as the currently logged in user. To create an entry for a different
    /// user, use the <see cref="M:EllieMae.Encompass.BusinessObjects.Loans.Logging.LogDisclosures2015.AddForUser(System.DateTime,EllieMae.Encompass.BusinessObjects.Loans.Logging.StandardDisclosure2015Type,EllieMae.Encompass.BusinessObjects.Users.User)" /> method.</remarks>
    public Disclosure2015 Add(DateTime disclosureDate, StandardDisclosure2015Type disclosureType)
    {
      DisclosureTracking2015Log disclosureTracking2015Log = new DisclosureTracking2015Log(DateTime.Now, this.Loan.Unwrap().LoanData, (disclosureType & StandardDisclosure2015Type.LE) > StandardDisclosure2015Type.None, (disclosureType & StandardDisclosure2015Type.CD) > StandardDisclosure2015Type.None, true, (disclosureType & StandardDisclosure2015Type.SAFEHARBOR) > StandardDisclosure2015Type.None, (disclosureType & StandardDisclosure2015Type.PROVIDERLIST) > StandardDisclosure2015Type.None, (disclosureType & StandardDisclosure2015Type.PROVIDERLISTNOFEE) > StandardDisclosure2015Type.None);
      disclosureTracking2015Log.DisclosedBy = this.Loan.Session.UserID;
      disclosureTracking2015Log.DisclosedByFullName = this.Loan.Session.GetUserInfo().FullName;
      disclosureTracking2015Log.IsLocked = true;
      disclosureTracking2015Log.DisclosedDate = disclosureDate;
      disclosureTracking2015Log.BorrowerPairID = this.Loan.BorrowerPairs.Current.Unwrap().Id;
      DateTime dateTime = this.Loan.Unwrap().AddBusinessDates(disclosureDate, 3);
      if (disclosureTracking2015Log.IsBorrowerPresumedDateLocked)
        disclosureTracking2015Log.LockedBorrowerPresumedReceivedDate = dateTime;
      else
        disclosureTracking2015Log.BorrowerPresumedReceivedDate = dateTime;
      if (!string.IsNullOrEmpty(this.Loan.BorrowerPairs.Current.CoBorrower.FirstName) || !string.IsNullOrEmpty(this.Loan.BorrowerPairs.Current.CoBorrower.LastName))
      {
        if (disclosureTracking2015Log.IsCoBorrowerPresumedDateLocked)
          disclosureTracking2015Log.LockedCoBorrowerPresumedReceivedDate = dateTime;
        else
          disclosureTracking2015Log.CoBorrowerPresumedReceivedDate = dateTime;
      }
      this.AddDocuments(disclosureTracking2015Log);
      this.Loan.Unwrap().AddDisclosureTracking2015toLoanLog(disclosureTracking2015Log);
      return (Disclosure2015) this.CreateEntry((LogRecordBase) disclosureTracking2015Log);
    }

    /// <summary>Adds a Disclosure to the loan's log.</summary>
    /// <param name="disclosureDate">The date of the conversation.</param>
    /// <param name="disclosureType">The type(s) of standard disclosures included in this disclosure.</param>
    /// <param name="disclosedBy">The <see cref="T:EllieMae.Encompass.BusinessObjects.Users.User" /> who generated the disclosure. This
    /// value must match the currently logged in user unless the logged in user is
    /// an administrator.</param>
    /// <returns>Returns the newly added <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.Disclosure2015">Disclosure</see>
    /// object.</returns>
    public Disclosure2015 AddForUser(
      DateTime disclosureDate,
      StandardDisclosure2015Type disclosureType,
      User disclosedBy)
    {
      if (disclosedBy == null)
        throw new ArgumentNullException(nameof (disclosedBy));
      if (disclosedBy.ID != this.Loan.Session.UserID && !this.Loan.Session.Unwrap().GetUser().GetUserInfo().IsAdministrator())
        throw new InvalidOperationException("User does not have sufficient rights to perform this operation.");
      DisclosureTracking2015Log disclosureTracking2015Log = new DisclosureTracking2015Log(disclosureDate, this.Loan.Unwrap().LoanData, (disclosureType & StandardDisclosure2015Type.LE) > StandardDisclosure2015Type.None, (disclosureType & StandardDisclosure2015Type.CD) > StandardDisclosure2015Type.None, true, (disclosureType & StandardDisclosure2015Type.SAFEHARBOR) > StandardDisclosure2015Type.None, (disclosureType & StandardDisclosure2015Type.PROVIDERLIST) > StandardDisclosure2015Type.None, (disclosureType & StandardDisclosure2015Type.PROVIDERLISTNOFEE) > StandardDisclosure2015Type.None);
      disclosureTracking2015Log.DisclosedBy = disclosedBy.ID;
      disclosureTracking2015Log.DisclosedByFullName = disclosedBy.FullName;
      disclosureTracking2015Log.BorrowerPairID = this.Loan.BorrowerPairs.Current.Unwrap().Id;
      DateTime dateTime = this.Loan.Unwrap().AddBusinessDates(disclosureDate, 3);
      if (disclosureTracking2015Log.IsBorrowerPresumedDateLocked)
        disclosureTracking2015Log.LockedBorrowerPresumedReceivedDate = dateTime;
      else
        disclosureTracking2015Log.BorrowerPresumedReceivedDate = dateTime;
      if (!string.IsNullOrEmpty(this.Loan.BorrowerPairs.Current.CoBorrower.FirstName) || !string.IsNullOrEmpty(this.Loan.BorrowerPairs.Current.CoBorrower.LastName))
      {
        if (disclosureTracking2015Log.IsCoBorrowerPresumedDateLocked)
          disclosureTracking2015Log.LockedCoBorrowerPresumedReceivedDate = dateTime;
        else
          disclosureTracking2015Log.CoBorrowerPresumedReceivedDate = dateTime;
      }
      this.AddDocuments(disclosureTracking2015Log);
      this.Loan.Unwrap().AddDisclosureTracking2015toLoanLog(disclosureTracking2015Log);
      return (Disclosure2015) this.CreateEntry((LogRecordBase) disclosureTracking2015Log);
    }

    /// <summary>Returns most recent disclosure</summary>
    /// <returns></returns>
    public Disclosure2015 GetMostRecentDisclosure()
    {
      return this.Cast<Disclosure2015>().OrderByDescending<Disclosure2015, DateTime>((Func<Disclosure2015, DateTime>) (dis => dis.Date)).FirstOrDefault<Disclosure2015>();
    }

    /// <summary>Returns most recent standard disclosure.</summary>
    /// <param name="disclosureType"></param>
    /// <returns></returns>
    public Disclosure2015 GetMostRecentStandardDisclosure(StandardDisclosure2015Type disclosureType)
    {
      return this.Cast<Disclosure2015>().Where<Disclosure2015>((Func<Disclosure2015, bool>) (dis => (dis.DisclosureType & disclosureType) == disclosureType)).OrderByDescending<Disclosure2015, DateTime>((Func<Disclosure2015, DateTime>) (dis => dis.Date)).FirstOrDefault<Disclosure2015>();
    }

    /// <summary>
    /// Returns borrowerPairIDs distribution using Disclosure tracking type
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public Dictionary<string, int> borrowerPairIDsDistribution(
      DisclosureTracking2015Log.DisclosureTrackingType type)
    {
      IDisclosureTracking2015Log[] idisclosureTracking2015Log = this.Loan.Unwrap().LoanData.GetLogList().GetAllIDisclosureTracking2015Log(true);
      Dictionary<string, int> dictionary = new Dictionary<string, int>();
      if (type == DisclosureTracking2015Log.DisclosureTrackingType.LE)
      {
        foreach (DisclosureTracking2015Log disclosureTracking2015Log in idisclosureTracking2015Log)
        {
          if (disclosureTracking2015Log.DisclosedForLE)
          {
            if (dictionary.ContainsKey(disclosureTracking2015Log.BorrowerPairID))
              dictionary[disclosureTracking2015Log.BorrowerPairID]++;
            else
              dictionary.Add(disclosureTracking2015Log.BorrowerPairID, 1);
          }
        }
      }
      if (type == DisclosureTracking2015Log.DisclosureTrackingType.CD)
      {
        foreach (DisclosureTracking2015Log disclosureTracking2015Log in idisclosureTracking2015Log)
        {
          if (disclosureTracking2015Log.DisclosedForCD)
          {
            if (dictionary.ContainsKey(disclosureTracking2015Log.BorrowerPairID))
              dictionary[disclosureTracking2015Log.BorrowerPairID]++;
            else
              dictionary.Add(disclosureTracking2015Log.BorrowerPairID, 1);
          }
        }
      }
      return dictionary;
    }

    private void AddDocuments(DisclosureTracking2015Log disLog)
    {
      Dictionary<string, int> dictionary1 = this.borrowerPairIDsDistribution(DisclosureTracking2015Log.DisclosureTrackingType.LE);
      Dictionary<string, int> dictionary2 = this.borrowerPairIDsDistribution(DisclosureTracking2015Log.DisclosureTrackingType.CD);
      int num = 0;
      if (disLog.DisclosedForCD)
      {
        disLog.AddDisclosedFormItem("Closing Disclosure", DisclosureTrackingFormItem.FormType.StandardForm);
        disLog.AddDisclosedFormItem("Closing Disclosure (Alternate)", DisclosureTrackingFormItem.FormType.StandardForm);
        disLog.AddDisclosedFormItem("Closing Disclosure (Seller)", DisclosureTrackingFormItem.FormType.StandardForm);
        disLog.DisclosureType = this.Loan.Unwrap().LoanData.GetLogList().GetLatestIDisclosureTracking2015Log(DisclosureTracking2015Log.DisclosureTrackingType.CD) == null || !dictionary1.TryGetValue(disLog.BorrowerPairID, out num) ? DisclosureTracking2015Log.DisclosureTypeEnum.Initial : DisclosureTracking2015Log.DisclosureTypeEnum.Revised;
      }
      if (disLog.DisclosedForLE)
      {
        disLog.AddDisclosedFormItem("Loan Estimate", DisclosureTrackingFormItem.FormType.StandardForm);
        disLog.AddDisclosedFormItem("Loan Estimate (Alternate)", DisclosureTrackingFormItem.FormType.StandardForm);
        disLog.DisclosureType = this.Loan.Unwrap().LoanData.GetLogList().GetLatestIDisclosureTracking2015Log(DisclosureTracking2015Log.DisclosureTrackingType.LE) == null || !dictionary2.TryGetValue(disLog.BorrowerPairID, out num) ? DisclosureTracking2015Log.DisclosureTypeEnum.Initial : DisclosureTracking2015Log.DisclosureTypeEnum.Revised;
      }
      if (disLog.DisclosedForSafeHarbor)
        disLog.AddDisclosedFormItem("2015 Anti-Steering Safe Harbor", DisclosureTrackingFormItem.FormType.StandardForm);
      if (disLog.ProviderListSent)
        disLog.AddDisclosedFormItem("2015 Settlement Service Provider List", DisclosureTrackingFormItem.FormType.StandardForm);
      if (!disLog.ProviderListNoFeeSent)
        return;
      disLog.AddDisclosedFormItem("2015 Settlement Service Provider List - No Fees", DisclosureTrackingFormItem.FormType.StandardForm);
    }

    /// <summary>Wraps a LogRecord in a LogEntry object.</summary>
    internal override LogEntry Wrap(LogRecordBase logRecord)
    {
      return (LogEntry) new Disclosure2015(this.Loan, (DisclosureTracking2015Log) logRecord);
    }
  }
}
