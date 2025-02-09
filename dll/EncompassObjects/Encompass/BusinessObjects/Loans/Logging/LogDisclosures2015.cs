// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.LogDisclosures2015
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine.Log;
using EllieMae.Encompass.BusinessObjects.Users;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  public class LogDisclosures2015 : LoanLogEntryCollection, ILogDisclosures2015, IEnumerable
  {
    internal LogDisclosures2015(Loan loan)
      : base(loan, typeof (DisclosureTracking2015Log))
    {
    }

    public Disclosure2015 this[int index] => (Disclosure2015) this.LogEntries[index];

    public Disclosure2015 Add(DateTime disclosureDate, StandardDisclosure2015Type disclosureType)
    {
      DisclosureTracking2015Log disclosureTracking2015Log = new DisclosureTracking2015Log(DateTime.Now, this.Loan.Unwrap().LoanData, (disclosureType & StandardDisclosure2015Type.LE) > StandardDisclosure2015Type.None, (disclosureType & StandardDisclosure2015Type.CD) > StandardDisclosure2015Type.None, true, (disclosureType & StandardDisclosure2015Type.SAFEHARBOR) > StandardDisclosure2015Type.None, (disclosureType & StandardDisclosure2015Type.PROVIDERLIST) > StandardDisclosure2015Type.None, (disclosureType & StandardDisclosure2015Type.PROVIDERLISTNOFEE) > StandardDisclosure2015Type.None);
      ((DisclosureTrackingBase) disclosureTracking2015Log).DisclosedBy = this.Loan.Session.UserID;
      ((DisclosureTrackingBase) disclosureTracking2015Log).DisclosedByFullName = this.Loan.Session.GetUserInfo().FullName;
      ((DisclosureTrackingBase) disclosureTracking2015Log).IsLocked = true;
      ((DisclosureTrackingBase) disclosureTracking2015Log).DisclosedDate = disclosureDate;
      ((DisclosureTrackingBase) disclosureTracking2015Log).BorrowerPairID = this.Loan.BorrowerPairs.Current.Unwrap().Id;
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
      ((DisclosureTrackingBase) disclosureTracking2015Log).DisclosedBy = disclosedBy.ID;
      ((DisclosureTrackingBase) disclosureTracking2015Log).DisclosedByFullName = disclosedBy.FullName;
      ((DisclosureTrackingBase) disclosureTracking2015Log).BorrowerPairID = this.Loan.BorrowerPairs.Current.Unwrap().Id;
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

    public Disclosure2015 GetMostRecentDisclosure()
    {
      return this.Cast<Disclosure2015>().OrderByDescending<Disclosure2015, DateTime>((Func<Disclosure2015, DateTime>) (dis => dis.Date)).FirstOrDefault<Disclosure2015>();
    }

    public Disclosure2015 GetMostRecentStandardDisclosure(StandardDisclosure2015Type disclosureType)
    {
      return this.Cast<Disclosure2015>().Where<Disclosure2015>((Func<Disclosure2015, bool>) (dis => (dis.DisclosureType & disclosureType) == disclosureType)).OrderByDescending<Disclosure2015, DateTime>((Func<Disclosure2015, DateTime>) (dis => dis.Date)).FirstOrDefault<Disclosure2015>();
    }

    public Dictionary<string, int> borrowerPairIDsDistribution(
      DisclosureTracking2015Log.DisclosureTrackingType type)
    {
      IDisclosureTracking2015Log[] idisclosureTracking2015Log = this.Loan.Unwrap().LoanData.GetLogList().GetAllIDisclosureTracking2015Log(true);
      Dictionary<string, int> dictionary = new Dictionary<string, int>();
      if (type == 1)
      {
        foreach (DisclosureTracking2015Log disclosureTracking2015Log in idisclosureTracking2015Log)
        {
          if (disclosureTracking2015Log.DisclosedForLE)
          {
            if (dictionary.ContainsKey(((DisclosureTrackingBase) disclosureTracking2015Log).BorrowerPairID))
              dictionary[((DisclosureTrackingBase) disclosureTracking2015Log).BorrowerPairID]++;
            else
              dictionary.Add(((DisclosureTrackingBase) disclosureTracking2015Log).BorrowerPairID, 1);
          }
        }
      }
      if (type == 2)
      {
        foreach (DisclosureTracking2015Log disclosureTracking2015Log in idisclosureTracking2015Log)
        {
          if (disclosureTracking2015Log.DisclosedForCD)
          {
            if (dictionary.ContainsKey(((DisclosureTrackingBase) disclosureTracking2015Log).BorrowerPairID))
              dictionary[((DisclosureTrackingBase) disclosureTracking2015Log).BorrowerPairID]++;
            else
              dictionary.Add(((DisclosureTrackingBase) disclosureTracking2015Log).BorrowerPairID, 1);
          }
        }
      }
      return dictionary;
    }

    private void AddDocuments(DisclosureTracking2015Log disLog)
    {
      Dictionary<string, int> dictionary1 = this.borrowerPairIDsDistribution((DisclosureTracking2015Log.DisclosureTrackingType) 1);
      Dictionary<string, int> dictionary2 = this.borrowerPairIDsDistribution((DisclosureTracking2015Log.DisclosureTrackingType) 2);
      int num = 0;
      if (disLog.DisclosedForCD)
      {
        ((DisclosureTrackingBase) disLog).AddDisclosedFormItem("Closing Disclosure", (DisclosureTrackingFormItem.FormType) 2);
        ((DisclosureTrackingBase) disLog).AddDisclosedFormItem("Closing Disclosure (Alternate)", (DisclosureTrackingFormItem.FormType) 2);
        ((DisclosureTrackingBase) disLog).AddDisclosedFormItem("Closing Disclosure (Seller)", (DisclosureTrackingFormItem.FormType) 2);
        disLog.DisclosureType = this.Loan.Unwrap().LoanData.GetLogList().GetLatestIDisclosureTracking2015Log((DisclosureTracking2015Log.DisclosureTrackingType) 2, false) == null || !dictionary1.TryGetValue(((DisclosureTrackingBase) disLog).BorrowerPairID, out num) ? (DisclosureTracking2015Log.DisclosureTypeEnum) 1 : (DisclosureTracking2015Log.DisclosureTypeEnum) 2;
      }
      if (disLog.DisclosedForLE)
      {
        ((DisclosureTrackingBase) disLog).AddDisclosedFormItem("Loan Estimate", (DisclosureTrackingFormItem.FormType) 2);
        ((DisclosureTrackingBase) disLog).AddDisclosedFormItem("Loan Estimate (Alternate)", (DisclosureTrackingFormItem.FormType) 2);
        disLog.DisclosureType = this.Loan.Unwrap().LoanData.GetLogList().GetLatestIDisclosureTracking2015Log((DisclosureTracking2015Log.DisclosureTrackingType) 1, false) == null || !dictionary2.TryGetValue(((DisclosureTrackingBase) disLog).BorrowerPairID, out num) ? (DisclosureTracking2015Log.DisclosureTypeEnum) 1 : (DisclosureTracking2015Log.DisclosureTypeEnum) 2;
      }
      if (((DisclosureTrackingBase) disLog).DisclosedForSafeHarbor)
        ((DisclosureTrackingBase) disLog).AddDisclosedFormItem("2015 Anti-Steering Safe Harbor", (DisclosureTrackingFormItem.FormType) 2);
      if (disLog.ProviderListSent)
        ((DisclosureTrackingBase) disLog).AddDisclosedFormItem("2015 Settlement Service Provider List", (DisclosureTrackingFormItem.FormType) 2);
      if (!disLog.ProviderListNoFeeSent)
        return;
      ((DisclosureTrackingBase) disLog).AddDisclosedFormItem("2015 Settlement Service Provider List - No Fees", (DisclosureTrackingFormItem.FormType) 2);
    }

    internal override LogEntry Wrap(LogRecordBase logRecord)
    {
      return (LogEntry) new Disclosure2015(this.Loan, (DisclosureTracking2015Log) logRecord);
    }
  }
}
