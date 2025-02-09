// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.Disclosure2015
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  public class Disclosure2015 : DisclosureBase, IDisclosure2015
  {
    private bool allowExtendedDisclosureAccess;
    private bool getExtendedDisclosureAccessSetting;

    internal Disclosure2015(Loan loan, DisclosureTracking2015Log discItem)
      : base(loan, (DisclosureTrackingBase) discItem)
    {
    }

    private bool AllowExtendedDisclosureAccess
    {
      get
      {
        if (!this.getExtendedDisclosureAccessSetting)
        {
          this.allowExtendedDisclosureAccess = SmartClientUtils.GetAttribute(this.loan.Session.ClientID, "EncompassSDK", "EnableSDKExtendedDisclosure") == "1";
          this.getExtendedDisclosureAccessSetting = true;
        }
        return this.allowExtendedDisclosureAccess;
      }
    }

    public override LogEntryType EntryType => LogEntryType.Disclosure2015;

    public StandardDisclosure2015Type DisclosureType
    {
      get
      {
        this.EnsureValid();
        StandardDisclosure2015Type disclosureType = StandardDisclosure2015Type.None;
        if (this.DiscItem.DisclosedForCD)
          disclosureType |= StandardDisclosure2015Type.CD;
        if (this.DiscItem.DisclosedForLE)
          disclosureType |= StandardDisclosure2015Type.LE;
        if (this.DiscItem.ProviderListSent)
          disclosureType |= StandardDisclosure2015Type.PROVIDERLIST;
        if (this.DiscItem.ProviderListNoFeeSent)
          disclosureType |= StandardDisclosure2015Type.PROVIDERLISTNOFEE;
        if (((DisclosureTrackingBase) this.DiscItem).DisclosedForSafeHarbor)
          disclosureType |= StandardDisclosure2015Type.SAFEHARBOR;
        return disclosureType;
      }
    }

    public bool EnabledForCompliance
    {
      get
      {
        this.EnsureValid();
        return this.DiscItem.IsDisclosed;
      }
      set
      {
        this.EnsureEditable();
        this.DiscItem.IsDisclosed = value;
      }
    }

    public DeliveryMethod2015 DeliveryMethod
    {
      get
      {
        this.EnsureValid();
        return (DeliveryMethod2015) this.DiscItem.DisclosureMethod;
      }
      set
      {
        this.EnsureEditable();
        this.DiscItem.DisclosureMethod = (DisclosureTrackingBase.DisclosedMethod) value;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public string DeliveryMethodOther
    {
      get
      {
        if (this.AllowExtendedDisclosureAccess)
          return this.DiscItem.DisclosedMethodOther;
        throw new InvalidOperationException("Extended Disclosure update is not allowed by system policy.");
      }
      set
      {
        if (!this.AllowExtendedDisclosureAccess)
          throw new InvalidOperationException("Extended Disclosure update is not allowed by system policy.");
        this.DiscItem.DisclosedMethodOther = value;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public DateTime BorrowerActualReceivedDate
    {
      get
      {
        if (this.AllowExtendedDisclosureAccess)
          return this.DiscItem.BorrowerActualReceivedDate;
        throw new InvalidOperationException("Extended Disclosure update is not allowed by system policy.");
      }
      set
      {
        if (!this.AllowExtendedDisclosureAccess)
          throw new InvalidOperationException("Extended Disclosure update is not allowed by system policy.");
        this.DiscItem.BorrowerActualReceivedDate = value;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public DeliveryMethod2015 BorrowerReceivedMethod
    {
      get
      {
        if (this.AllowExtendedDisclosureAccess)
          return (DeliveryMethod2015) this.DiscItem.BorrowerDisclosedMethod;
        throw new InvalidOperationException("Extended Disclosure update is not allowed by system policy.");
      }
      set
      {
        if (!this.AllowExtendedDisclosureAccess)
          throw new InvalidOperationException("Extended Disclosure update is not allowed by system policy.");
        this.EnsureValidDisclosedMethod(value);
        this.DiscItem.BorrowerDisclosedMethod = (DisclosureTrackingBase.DisclosedMethod) value;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public string BorrowerReceivedMethodOther
    {
      get
      {
        if (this.AllowExtendedDisclosureAccess)
          return this.DiscItem.BorrowerDisclosedMethodOther;
        throw new InvalidOperationException("Extended Disclosure update is not allowed by system policy.");
      }
      set
      {
        if (!this.AllowExtendedDisclosureAccess)
          throw new InvalidOperationException("Extended Disclosure update is not allowed by system policy.");
        this.DiscItem.BorrowerDisclosedMethodOther = value;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public DateTime CoBorrowerActualReceivedDate
    {
      get
      {
        if (this.AllowExtendedDisclosureAccess)
          return this.DiscItem.CoBorrowerActualReceivedDate;
        throw new InvalidOperationException("Extended Disclosure update is not allowed by system policy.");
      }
      set
      {
        if (!this.AllowExtendedDisclosureAccess)
          throw new InvalidOperationException("Extended Disclosure update is not allowed by system policy.");
        this.DiscItem.CoBorrowerActualReceivedDate = value;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public DeliveryMethod2015 CoBorrowerReceivedMethod
    {
      get
      {
        if (this.AllowExtendedDisclosureAccess)
          return (DeliveryMethod2015) this.DiscItem.CoBorrowerDisclosedMethod;
        throw new InvalidOperationException("Extended Disclosure update is not allowed by system policy.");
      }
      set
      {
        if (!this.AllowExtendedDisclosureAccess)
          throw new InvalidOperationException("Extended Disclosure update is not allowed by system policy.");
        this.EnsureValidDisclosedMethod(value);
        this.DiscItem.CoBorrowerDisclosedMethod = (DisclosureTrackingBase.DisclosedMethod) value;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public string CoBorrowerReceivedMethodOther
    {
      get
      {
        if (this.AllowExtendedDisclosureAccess)
          return this.DiscItem.CoBorrowerDisclosedMethodOther;
        throw new InvalidOperationException("Extended Disclosure update is not allowed by system policy.");
      }
      set
      {
        if (!this.AllowExtendedDisclosureAccess)
          throw new InvalidOperationException("Extended Disclosure update is not allowed by system policy.");
        this.DiscItem.CoBorrowerDisclosedMethodOther = value;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public DisclosureRecordType DisclosureRecordType
    {
      get
      {
        if (this.AllowExtendedDisclosureAccess)
          return (DisclosureRecordType) this.DiscItem.DisclosureType;
        throw new InvalidOperationException("Extended Disclosure update is not allowed by system policy.");
      }
      set
      {
        if (!this.AllowExtendedDisclosureAccess)
          throw new InvalidOperationException("Extended Disclosure update is not allowed by system policy.");
        this.DiscItem.DisclosureType = (DisclosureTracking2015Log.DisclosureTypeEnum) value;
      }
    }

    private bool AllowManualFulfillment
    {
      get
      {
        try
        {
          return !(((IConfigurationManager) this.loan.Session.GetObject("ConfigurationManager")).GetCompanySetting("Fulfillment", "ServiceEnabled") == "Y") & ((IFeaturesAclManager) this.loan.Session.GetAclManager((AclCategory) 0)).CheckPermission((AclFeature) 365, this.loan.Session.UserID) && ((DisclosureTrackingBase) this.DiscItem).FullfillmentProcessedDate == DateTime.MinValue;
        }
        catch (Exception ex)
        {
          return false;
        }
      }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public string ManualFulfilledBy
    {
      get
      {
        if (this.AllowExtendedDisclosureAccess && this.AllowManualFulfillment)
          return ((DisclosureTrackingBase) this.DiscItem).eDisclosureManuallyFulfilledBy;
        throw new InvalidOperationException("Extended Disclosure update is not allowed by system policy.");
      }
      set
      {
        if (!this.AllowExtendedDisclosureAccess || !this.AllowManualFulfillment)
          throw new InvalidOperationException("Extended Disclosure update is not allowed by system policy.");
        ((DisclosureTrackingBase) this.DiscItem).eDisclosureManuallyFulfilledBy = value;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public DateTime ManualFulfilledDateTime
    {
      get
      {
        if (this.AllowExtendedDisclosureAccess && this.AllowManualFulfillment)
          return ((DisclosureTrackingBase) this.DiscItem).eDisclosureManualFulfillmentDate;
        throw new InvalidOperationException("Extended Disclosure update is not allowed by system policy.");
      }
      set
      {
        if (!this.AllowExtendedDisclosureAccess || !this.AllowManualFulfillment)
          throw new InvalidOperationException("Extended Disclosure update is not allowed by system policy.");
        ((DisclosureTrackingBase) this.DiscItem).eDisclosureManualFulfillmentDate = value;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public DeliveryMethod2015 ManualFulfillmentMethod
    {
      get
      {
        if (this.AllowExtendedDisclosureAccess && this.AllowManualFulfillment)
          return (DeliveryMethod2015) ((DisclosureTrackingBase) this.DiscItem).eDisclosureManualFulfillmentMethod;
        throw new InvalidOperationException("Extended Disclosure update is not allowed by system policy.");
      }
      set
      {
        if (!this.AllowExtendedDisclosureAccess || !this.AllowManualFulfillment)
          throw new InvalidOperationException("Extended Disclosure update is not allowed by system policy.");
        ((DisclosureTrackingBase) this.DiscItem).eDisclosureManualFulfillmentMethod = (DisclosureTrackingBase.DisclosedMethod) value;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public string ManualFulfillmentCommets
    {
      get
      {
        if (this.AllowExtendedDisclosureAccess && this.AllowManualFulfillment)
          return ((DisclosureTrackingBase) this.DiscItem).eDisclosureManualFulfillmentComment;
        throw new InvalidOperationException("Extended Disclosure update is not allowed by system policy.");
      }
      set
      {
        if (!this.AllowExtendedDisclosureAccess || !this.AllowManualFulfillment)
          throw new InvalidOperationException("Extended Disclosure update is not allowed by system policy.");
        ((DisclosureTrackingBase) this.DiscItem).eDisclosureManualFulfillmentComment = value;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public DateTime ManualFulfillmentActualReceivedDate
    {
      get
      {
        if (this.AllowExtendedDisclosureAccess && this.AllowManualFulfillment)
          return this.DiscItem.ActualFulfillmentDate;
        throw new InvalidOperationException("Extended Disclosure update is not allowed by system policy.");
      }
      set
      {
        if (!this.AllowExtendedDisclosureAccess || !this.AllowManualFulfillment)
          throw new InvalidOperationException("Extended Disclosure update is not allowed by system policy.");
        this.DiscItem.ActualFulfillmentDate = value;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public DateTime ManualFulfillmentPresumedReceivedDate
    {
      get
      {
        if (this.AllowExtendedDisclosureAccess && this.AllowManualFulfillment)
          return this.DiscItem.PresumedFulfillmentDate;
        throw new InvalidOperationException("Extended Disclosure update is not allowed by system policy.");
      }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public DataObject MergedDoc
    {
      get
      {
        if (!this.AllowExtendedDisclosureAccess || !this.AllowManualFulfillment)
          throw new InvalidOperationException("Extended Disclosure update is not allowed by system policy.");
        if (((DisclosureTrackingBase) this.DiscItem).eDisclosurePackageViewableFile != string.Empty)
        {
          BinaryObject supportingData = this.Loan.Unwrap().GetSupportingData(((DisclosureTrackingBase) this.DiscItem).eDisclosurePackageViewableFile);
          if (supportingData != null)
            return new DataObject(supportingData);
        }
        return (DataObject) null;
      }
    }

    public string BorrowerName => ((DisclosureTrackingBase) this.DiscItem).BorrowerName;

    public string CoBorrowerName => ((DisclosureTrackingBase) this.DiscItem).CoBorrowerName;

    public string eDisclosureBorrowerName
    {
      get => ((DisclosureTrackingBase) this.DiscItem).eDisclosureBorrowerName;
    }

    public string eDisclosureCoBorrowerName
    {
      get => ((DisclosureTrackingBase) this.DiscItem).eDisclosureCoBorrowerName;
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public DateTime DisclosedDate
    {
      get
      {
        if (this.AllowExtendedDisclosureAccess)
          return this.DiscItem.DisclosedDateTime;
        throw new InvalidOperationException("Extended Disclosure update is not allowed by system policy.");
      }
      set
      {
        if (!this.AllowExtendedDisclosureAccess)
          throw new InvalidOperationException("Extended Disclosure update is not allowed by system policy.");
        DateTime dateTime = this.Loan.Unwrap().AddBusinessDates(value, 3);
        if (this.DiscItem.IsBorrowerPresumedDateLocked)
          this.DiscItem.LockedBorrowerPresumedReceivedDate = dateTime;
        else
          this.DiscItem.BorrowerPresumedReceivedDate = dateTime;
        if (!string.IsNullOrEmpty(((DisclosureTrackingBase) this.DiscItem).CoBorrowerName))
        {
          if (this.DiscItem.IsCoBorrowerPresumedDateLocked)
            this.DiscItem.LockedCoBorrowerPresumedReceivedDate = dateTime;
          else
            this.DiscItem.CoBorrowerPresumedReceivedDate = dateTime;
        }
        ((DisclosureTrackingBase) this.DiscItem).DisclosedDate = value;
      }
    }

    public DisclosureRecordType DisclosureRecordType2015
    {
      get => (DisclosureRecordType) this.DiscItem.DisclosureType2015;
    }

    public DateTime DisclosedDate2015 => this.DiscItem.DisclosedDate2015;

    public DeliveryMethod2015 BorrowerReceivedMethod2015
    {
      get => (DeliveryMethod2015) this.DiscItem.BorrowerDisclosedMethod2015;
    }

    public DeliveryMethod2015 CoBorrowerReceivedMethod2015
    {
      get => (DeliveryMethod2015) this.DiscItem.CoBorrowerDisclosedMethod2015;
    }

    public DateTime BorrowerActualReceivedDate2015 => this.DiscItem.BorrowerActualReceivedDate2015;

    public DateTime CoBorrowerActualReceivedDate2015
    {
      get => this.DiscItem.CoBorrowerActualReceivedDate2015;
    }

    private void EnsureValidDisclosedMethod(DeliveryMethod2015 disclosedMethod)
    {
      bool flag = false;
      switch (disclosedMethod)
      {
        case DeliveryMethod2015.Unknown:
          flag = false;
          break;
        case DeliveryMethod2015.Mail:
          flag = true;
          break;
        case DeliveryMethod2015.eDisclosure:
          flag = false;
          break;
        case DeliveryMethod2015.Fax:
          flag = true;
          break;
        case DeliveryMethod2015.InPerson:
          flag = true;
          break;
        case DeliveryMethod2015.Other:
          flag = true;
          break;
        case DeliveryMethod2015.Email:
          flag = true;
          break;
        case DeliveryMethod2015.Phone:
          flag = false;
          break;
        case DeliveryMethod2015.Signature:
          flag = false;
          break;
      }
      if (!flag)
        throw new ArgumentException(disclosedMethod.ToString() + " is not an allowed value.");
    }

    public string UCD
    {
      get
      {
        string str = nameof (UCD) + ((LogRecordBase) this.DiscItem).Guid + ".XML";
        BinaryObject supportingData = this.Loan.Unwrap().GetSupportingData(str);
        return supportingData != null ? supportingData.ToString() : string.Empty;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public bool IsNonBorrowingOwnerExist
    {
      get
      {
        if (this.AllowExtendedDisclosureAccess)
          return this.DiscItem.IsNboExist;
        throw new InvalidOperationException("Extended Disclosure update is not allowed by system policy.");
      }
    }

    public override object ReceivedDate
    {
      get
      {
        this.EnsureValid();
        return !((DisclosureTrackingBase) this.DiscItem).Received ? (object) null : (object) this.discItem.ReceivedDate;
      }
      set
      {
        this.EnsureEditable();
        if (value == null)
        {
          ((DisclosureTrackingBase) this.DiscItem).ReceivedDate = DateTime.MinValue;
        }
        else
        {
          DateTime dateTime = value is DateTime ? Convert.ToDateTime(value) : throw new ArgumentException("The specified value must be a DateTime.");
          if (dateTime.Date < ((LogRecordBase) this.DiscItem).Date.Date)
            throw new ArgumentException("The specified date is prior to the disclosure date.");
          ((DisclosureTrackingBase) this.DiscItem).ReceivedDate = dateTime.Date;
        }
      }
    }

    internal override bool IseDiclosure => this.DeliveryMethod == DeliveryMethod2015.eDisclosure;

    private DisclosureTracking2015Log DiscItem => (DisclosureTracking2015Log) this.discItem;

    public int NumberOfNonBorrowingOwnerContact
    {
      get => Utils.ParseInt((object) this.DiscItem.GetAttribute("NBOcount"), 0);
    }

    public int NumberOfVestingParties
    {
      get => Utils.ParseInt((object) this.DiscItem.GetAttribute("TRcount"), 0);
    }

    public int NumberOfGoodFaithChangeOfCircumstance
    {
      get => Utils.ParseInt((object) this.DiscItem.GetAttribute("XCOCcount"), 0);
    }

    public string GetAttribute(string name) => this.DiscItem.GetAttribute(name);

    [EditorBrowsable(EditorBrowsableState.Never)]
    public List<NonBorrowerOwner> GetAllNBOItems()
    {
      if (!this.AllowExtendedDisclosureAccess)
        throw new InvalidOperationException("Extended Disclosure update is not allowed by system policy.");
      List<NonBorrowerOwner> allNboItems = new List<NonBorrowerOwner>();
      foreach (KeyValuePair<string, INonBorrowerOwnerItem> allnboItem in this.DiscItem.GetAllnboItems())
        allNboItems.Add(new NonBorrowerOwner(allnboItem.Value));
      return allNboItems;
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public ChangeOfCircumstanceItems GetAllCOCItems()
    {
      if (!this.AllowExtendedDisclosureAccess)
        throw new InvalidOperationException("Extended Disclosure update is not allowed by system policy.");
      List<COCCollection> cocCollectionList = new List<COCCollection>();
      return new ChangeOfCircumstanceItems(this.PopulateAttributeToCOCFields(), this.DiscItem.GetAttribute("XCOCChangedCircumstances"), this.DiscItem.GetAttribute("XCOCFeeLevelIndicator"));
    }

    private List<COCCollection> PopulateAttributeToCOCFields()
    {
      List<COCCollection> cocFields = new List<COCCollection>();
      int changeOfCircumstance = this.DiscItem.NumberOfGoodFaithChangeOfCircumstance;
      for (int OrderId = 1; OrderId <= changeOfCircumstance; ++OrderId)
      {
        string str = "XCOC" + OrderId.ToString("00");
        COCCollection cocCollection = new COCCollection(this.DiscItem.GetAttribute(str + "01"), Utils.ParseDate((object) this.DiscItem.GetAttribute(str + "03")), Utils.ParseDate((object) this.DiscItem.GetAttribute(str + "04")), this.DiscItem.GetAttribute(str + "05"), this.DiscItem.GetAttribute(str + "06"), this.DiscItem.GetAttribute(str + "07"), this.DiscItem.GetAttribute(str + "08"), this.DiscItem.GetAttribute(str + "09"), this.DiscItem.GetAttribute(str + "98"), this.DiscItem.GetAttribute(str + "_Description"), Utils.ParseLong((object) this.DiscItem.GetAttribute(str + "_Amount"), 0L), OrderId);
        cocFields.Add(cocCollection);
      }
      return cocFields;
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public List<DisclosedVestingFieldsForDisclosure2015> GetAllVestingItems()
    {
      if (!this.AllowExtendedDisclosureAccess)
        throw new InvalidOperationException("Extended Disclosure update is not allowed by system policy.");
      return this.PopulateAttributeToVestingFields();
    }

    private List<DisclosedVestingFieldsForDisclosure2015> PopulateAttributeToVestingFields()
    {
      List<DisclosedVestingFieldsForDisclosure2015> vestingFields = new List<DisclosedVestingFieldsForDisclosure2015>();
      int ofVestingParties = this.DiscItem.NumberOfVestingParties;
      for (int OrderId = 1; OrderId <= ofVestingParties; ++OrderId)
      {
        string str = "TR" + OrderId.ToString("00");
        string attribute1 = this.DiscItem.GetAttribute(str + "01");
        string attribute2 = this.DiscItem.GetAttribute(str + "02");
        string attribute3 = this.DiscItem.GetAttribute(str + "03");
        VestingType vestingTypeFromString = this.GetVestingTypeFromString(this.DiscItem.GetAttribute(str + "04"));
        string attribute4 = this.DiscItem.GetAttribute(str + "05");
        TrusteeType tType = this.DiscItem.GetAttribute(str + "06").ToLower() == "trust 1" ? TrusteeType.Trust1 : TrusteeType.Trust2;
        string attribute5 = this.DiscItem.GetAttribute(str + "07");
        string attribute6 = this.DiscItem.GetAttribute(str + "08");
        bool boolean = Utils.ParseBoolean((object) this.DiscItem.GetAttribute(str + "09"));
        string attribute7 = this.DiscItem.GetAttribute(str + "10");
        string attribute8 = this.DiscItem.GetAttribute(str + "11");
        DateTime date = Utils.ParseDate((object) this.DiscItem.GetAttribute(str + "12"));
        OccupancyStatus statusFromString = this.GetOccupancyStatusFromString(this.DiscItem.GetAttribute(str + "13"));
        OccupancyIntent intentFromString = this.GetOccupancyIntentFromString(this.DiscItem.GetAttribute(str + "14"));
        string attribute9 = this.DiscItem.GetAttribute(str + "99");
        Utils.ParseInt((object) this.DiscItem.GetAttribute(str + OrderId.ToString("00")), false, -1);
        DisclosedVestingFieldsForDisclosure2015 forDisclosure2015 = new DisclosedVestingFieldsForDisclosure2015(attribute1, attribute2, attribute3, vestingTypeFromString, attribute4, tType, attribute5, attribute6, boolean, attribute7, attribute8, date, statusFromString, intentFromString, attribute9, OrderId);
        vestingFields.Add(forDisclosure2015);
      }
      return vestingFields;
    }

    private VestingType GetVestingTypeFromString(string vestingType)
    {
      if (string.IsNullOrWhiteSpace(vestingType))
        return VestingType.None;
      switch (vestingType.ToLower())
      {
        case "co-signer":
          return VestingType.Cosigner;
        case "individual":
          return VestingType.Individual;
        case "non title spouse":
          return VestingType.NonTitleSpouse;
        case "officer":
          return VestingType.Officer;
        case "settlor":
          return VestingType.Settlor;
        case "settlor trustee":
          return VestingType.SettlorTrustee;
        case "title only":
          return VestingType.TitleOnly;
        case "title only settlor trustee":
          return VestingType.TitleOnlySettlorTrustee;
        case "title only trustee":
          return VestingType.TitleOnlyTrustee;
        case "trustee":
          return VestingType.Trustee;
        default:
          return VestingType.None;
      }
    }

    private OccupancyStatus GetOccupancyStatusFromString(string occStatus)
    {
      if (string.IsNullOrWhiteSpace(occStatus))
        return OccupancyStatus.None;
      switch (occStatus.ToLower())
      {
        case "primary":
          return OccupancyStatus.PrimaryResidence;
        case "secondary":
          return OccupancyStatus.SecondHome;
        case "investment":
          return OccupancyStatus.Investor;
        default:
          return OccupancyStatus.None;
      }
    }

    private OccupancyIntent GetOccupancyIntentFromString(string occIntent)
    {
      if (string.IsNullOrWhiteSpace(occIntent))
        return OccupancyIntent.None;
      switch (occIntent.ToLower())
      {
        case "will occupy":
          return OccupancyIntent.WillOccupy;
        case "will not occupy":
          return OccupancyIntent.WillNotOccupy;
        case "currently occupy":
          return OccupancyIntent.CurrentlyOccupy;
        default:
          return OccupancyIntent.None;
      }
    }
  }
}
