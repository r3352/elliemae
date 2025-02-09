// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.NonBorrowerOwnerItem
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using System;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class NonBorrowerOwnerItem : INonBorrowerOwnerItem
  {
    private DisclosureTracking2015Log discLog;
    private DateTime _presumedReceivedDate;
    private DateTime _lockedPresumedReceivedDate;
    private DateTime _actualReceivedDate;

    public string FirstName { get; }

    public string MidName { get; }

    public string LastName { get; }

    public string Suffix { get; }

    public string Address { get; }

    public string City { get; }

    public string State { get; }

    public string Zip { get; }

    public string VestingType { get; }

    public string HomePhone { get; }

    public string Email { get; }

    public bool IsNoThirdPartyEmail { get; }

    public string BusiPhone { get; }

    public string Cell { get; }

    public string Fax { get; }

    public DateTime DOB { get; }

    public string OrderID { get; }

    public IDisclosureTracking2015Log DTLog => (IDisclosureTracking2015Log) this.discLog;

    public int DisclosedMethod { get; set; }

    public string DisclosedMethodOther { get; set; }

    public DateTime PresumedReceivedDate
    {
      get => this._presumedReceivedDate;
      set => this._presumedReceivedDate = Utils.TruncateDate(value);
    }

    public DateTime lockedPresumedReceivedDate
    {
      get => this._lockedPresumedReceivedDate;
      set => this._lockedPresumedReceivedDate = Utils.TruncateDate(value);
    }

    public bool isPresumedDateLocked { get; set; }

    public DateTime ActualReceivedDate
    {
      get => this._actualReceivedDate;
      set => this._actualReceivedDate = Utils.TruncateDate(value);
    }

    public bool isBorrowerTypeLocked { get; set; }

    public string BorrowerType { get; set; }

    public string LockedBorrowerType { get; set; }

    public string TRGuid { get; set; }

    public DateTime eDisclosureNBOAuthenticatedDate { get; set; }

    public string eDisclosureNBOAuthenticatedIP { get; set; }

    public DateTime eDisclosureNBOViewMessageDate { get; set; }

    public DateTime eDisclosureNBORejectConsentDate { get; set; }

    public string eDisclosureNBORejectConsentIP { get; set; }

    public DateTime eDisclosureNBOSignedDate { get; set; }

    public string eDisclosureNBOeSignedIP { get; set; }

    public string eDisclosureNBOLoanLevelConsent { get; set; }

    public DateTime eDisclosureNBOAcceptConsentDate { get; set; }

    public string eDisclosureNBOAcceptConsentIP { get; set; }

    public DateTime eDisclosureNBODocumentViewedDate { get; set; }

    public bool eDisclosureNBOeSignatures { get; set; }

    public NonBorrowerOwnerItem(
      string FName,
      string MName,
      string LName,
      string Suffix,
      string Address,
      string City,
      string State,
      string Zip,
      string VestingType,
      string HomePhone,
      string Email,
      bool IsNoThirdPartyEmail,
      string BusiPhone,
      string Cell,
      string Fax,
      DateTime DOB,
      string TRGuid,
      string InstanceId,
      DisclosureTracking2015Log parentLog)
    {
      this.FirstName = FName;
      this.MidName = MName;
      this.LastName = LName;
      this.Suffix = Suffix;
      this.Address = Address;
      this.City = City;
      this.State = State;
      this.Zip = Zip;
      this.VestingType = VestingType;
      this.HomePhone = HomePhone;
      this.Email = Email;
      this.IsNoThirdPartyEmail = IsNoThirdPartyEmail;
      this.BusiPhone = BusiPhone;
      this.Cell = Cell;
      this.Fax = Fax;
      this.DOB = DOB;
      this.TRGuid = TRGuid;
      this.OrderID = InstanceId;
      this.discLog = parentLog;
    }

    public INonBorrowerOwnerItem CloneForDuplicate(NonBorrowerOwnerItem item)
    {
      return (INonBorrowerOwnerItem) new NonBorrowerOwnerItem(item.FirstName, item.MidName, item.LastName, item.Suffix, item.Address, item.City, item.State, item.Zip, item.VestingType, item.HomePhone, item.Email, item.IsNoThirdPartyEmail, item.BusiPhone, item.Cell, item.Fax, item.DOB, item.TRGuid, item.OrderID, item.discLog)
      {
        DisclosedMethod = item.DisclosedMethod,
        DisclosedMethodOther = item.DisclosedMethodOther,
        PresumedReceivedDate = item.PresumedReceivedDate,
        lockedPresumedReceivedDate = item.lockedPresumedReceivedDate,
        isPresumedDateLocked = item.isPresumedDateLocked,
        ActualReceivedDate = item.ActualReceivedDate,
        isBorrowerTypeLocked = item.isBorrowerTypeLocked,
        BorrowerType = item.BorrowerType,
        LockedBorrowerType = item.LockedBorrowerType,
        TRGuid = item.TRGuid,
        eDisclosureNBOAuthenticatedDate = item.eDisclosureNBOAuthenticatedDate,
        eDisclosureNBOAuthenticatedIP = item.eDisclosureNBOAuthenticatedIP,
        eDisclosureNBOViewMessageDate = item.eDisclosureNBOViewMessageDate,
        eDisclosureNBORejectConsentDate = item.eDisclosureNBORejectConsentDate,
        eDisclosureNBORejectConsentIP = item.eDisclosureNBORejectConsentIP,
        eDisclosureNBOSignedDate = item.eDisclosureNBOSignedDate,
        eDisclosureNBOeSignedIP = item.eDisclosureNBOeSignedIP,
        eDisclosureNBOLoanLevelConsent = item.eDisclosureNBOLoanLevelConsent,
        eDisclosureNBODocumentViewedDate = item.eDisclosureNBODocumentViewedDate,
        eDisclosureNBOAcceptConsentDate = item.eDisclosureNBOAcceptConsentDate,
        eDisclosureNBOAcceptConsentIP = item.eDisclosureNBOAcceptConsentIP,
        eDisclosureNBOeSignatures = item.eDisclosureNBOeSignatures
      };
    }

    public INonBorrowerOwnerItem CloneForDuplicate()
    {
      return (INonBorrowerOwnerItem) new NonBorrowerOwnerItem(this.FirstName, this.MidName, this.LastName, this.Suffix, this.Address, this.City, this.State, this.Zip, this.VestingType, this.HomePhone, this.Email, this.IsNoThirdPartyEmail, this.BusiPhone, this.Cell, this.Fax, this.DOB, this.TRGuid, this.OrderID, this.discLog)
      {
        DisclosedMethod = this.DisclosedMethod,
        DisclosedMethodOther = this.DisclosedMethodOther,
        PresumedReceivedDate = this.PresumedReceivedDate,
        lockedPresumedReceivedDate = this.lockedPresumedReceivedDate,
        isPresumedDateLocked = this.isPresumedDateLocked,
        ActualReceivedDate = this.ActualReceivedDate,
        isBorrowerTypeLocked = this.isBorrowerTypeLocked,
        BorrowerType = this.BorrowerType,
        LockedBorrowerType = this.LockedBorrowerType,
        TRGuid = this.TRGuid,
        eDisclosureNBOAuthenticatedDate = this.eDisclosureNBOAuthenticatedDate,
        eDisclosureNBOAuthenticatedIP = this.eDisclosureNBOAuthenticatedIP,
        eDisclosureNBOViewMessageDate = this.eDisclosureNBOViewMessageDate,
        eDisclosureNBORejectConsentDate = this.eDisclosureNBORejectConsentDate,
        eDisclosureNBORejectConsentIP = this.eDisclosureNBORejectConsentIP,
        eDisclosureNBOSignedDate = this.eDisclosureNBOSignedDate,
        eDisclosureNBOeSignedIP = this.eDisclosureNBOeSignedIP,
        eDisclosureNBOAcceptConsentDate = this.eDisclosureNBOAcceptConsentDate,
        eDisclosureNBOAcceptConsentIP = this.eDisclosureNBOAcceptConsentIP,
        eDisclosureNBODocumentViewedDate = this.eDisclosureNBODocumentViewedDate,
        eDisclosureNBOLoanLevelConsent = this.eDisclosureNBOLoanLevelConsent,
        eDisclosureNBOeSignatures = this.eDisclosureNBOeSignatures
      };
    }
  }
}
