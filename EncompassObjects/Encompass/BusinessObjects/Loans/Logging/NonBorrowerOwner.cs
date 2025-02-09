// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.NonBorrowerOwner
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.Log;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>
  /// Represents a single NBO object which is part of Disclosure2015 object.
  /// </summary>
  /// <remarks>
  /// There are set of fields which are readonly and user cannot update the values of it.
  /// There are set of other properites which are updatable properties of NBO which can be get/set.
  /// When you set to the updatable properties, it will trigger the Disclosure tracking calc and timeline calculation is performed.
  /// </remarks>
  public class NonBorrowerOwner : INonBorrowerOwner
  {
    /// <summary>Returns first name</summary>
    public readonly string FirstName;
    /// <summary>Returns middle name</summary>
    public readonly string MidName;
    /// <summary>Returns last name</summary>
    public readonly string LastName;
    /// <summary>Returns suffix</summary>
    public readonly string Suffix;
    /// <summary>Returns address</summary>
    public readonly string Address;
    /// <summary>Returns city name</summary>
    public readonly string City;
    /// <summary>Returns state name</summary>
    public readonly string State;
    /// <summary>Returns zip code</summary>
    public readonly string Zip;
    /// <summary>Returns vesting type</summary>
    public readonly string VestingType;
    /// <summary>Returns home phone</summary>
    public readonly string HomePhone;
    /// <summary>Returns email</summary>
    public readonly string Email;
    /// <summary>
    /// Returns true if there is no third party email provided
    /// </summary>
    public readonly bool IsNoThirdPartyEmail;
    /// <summary>Returns business phone number</summary>
    public readonly string BusiPhone;
    /// <summary>Returns cell number</summary>
    public readonly string Cell;
    /// <summary>Returns fax number</summary>
    public readonly string Fax;
    /// <summary>Returns date of birth</summary>
    public readonly DateTime DOB;
    /// <summary>Returns guid</summary>
    public readonly string TRGuid;
    /// <summary>Returns order ID</summary>
    public readonly string OrderID;
    private string borrowerType;
    private string lockedBorrowerType;
    private IDisclosureTracking2015Log parentLog;
    private bool iseDisclosure;
    private EDisclosureStatusForNBO eDisclosures;

    /// <summary>Get or Set the NBO's DisclosedMethod</summary>
    public DeliveryMethod2015 DisclosedMethod
    {
      get
      {
        return (DeliveryMethod2015) Utils.ParseInt((object) this.parentLog.GetnboAttributeValue("NBOC" + this.OrderID, DisclosureTracking2015Log.NBOUpdatableFields.DisclosedMethod));
      }
      set
      {
        if (this.parentLog == null)
          return;
        this.parentLog.SetnboAttributeValue("NBOC" + this.OrderID, (object) (int) value, DisclosureTracking2015Log.NBOUpdatableFields.DisclosedMethod);
      }
    }

    /// <summary>Get or Set the NBO's DisclosedMethodOther</summary>
    public string DisclosedMethodOther
    {
      get
      {
        return this.parentLog.GetnboAttributeValue("NBOC" + this.OrderID, DisclosureTracking2015Log.NBOUpdatableFields.DisclosedMethodOther);
      }
      set
      {
        if (this.parentLog == null)
          return;
        this.parentLog.SetnboAttributeValue("NBOC" + this.OrderID, (object) value, DisclosureTracking2015Log.NBOUpdatableFields.DisclosedMethodOther);
      }
    }

    /// <summary>Get or Set the NBO's PresumedReceivedDate</summary>
    public DateTime PresumedReceivedDate
    {
      get
      {
        return Utils.ParseDate((object) this.parentLog.GetnboAttributeValue("NBOC" + this.OrderID, DisclosureTracking2015Log.NBOUpdatableFields.PresumedReceivedDate));
      }
      set
      {
        if (this.parentLog == null)
          return;
        this.parentLog.SetnboAttributeValue("NBOC" + this.OrderID, (object) value, DisclosureTracking2015Log.NBOUpdatableFields.PresumedReceivedDate);
      }
    }

    /// <summary>Get or Set the NBO's lockedPresumedReceivedDate</summary>
    public DateTime lockedPresumedReceivedDate
    {
      get
      {
        return Utils.ParseDate((object) this.parentLog.GetnboAttributeValue("NBOC" + this.OrderID, DisclosureTracking2015Log.NBOUpdatableFields.lockedPresumedReceivedDate));
      }
      set
      {
        if (this.parentLog == null)
          return;
        this.parentLog.SetnboAttributeValue("NBOC" + this.OrderID, (object) value, DisclosureTracking2015Log.NBOUpdatableFields.lockedPresumedReceivedDate);
      }
    }

    /// <summary>Get or Set the NBO's isPresumedDateLocked</summary>
    public bool IsPresumedDateLocked
    {
      get
      {
        return Utils.ParseBoolean((object) this.parentLog.GetnboAttributeValue("NBOC" + this.OrderID, DisclosureTracking2015Log.NBOUpdatableFields.isPresumedDateLocked));
      }
      set
      {
        if (this.parentLog == null)
          return;
        this.parentLog.SetnboAttributeValue("NBOC" + this.OrderID, (object) value, DisclosureTracking2015Log.NBOUpdatableFields.isPresumedDateLocked);
      }
    }

    /// <summary>Get or Set the NBO's ActualReceivedDate</summary>
    public DateTime ActualReceivedDate
    {
      get
      {
        return Utils.ParseDate((object) this.parentLog.GetnboAttributeValue("NBOC" + this.OrderID, DisclosureTracking2015Log.NBOUpdatableFields.ActualReceivedDate));
      }
      set
      {
        if (this.parentLog == null)
          return;
        this.parentLog.SetnboAttributeValue("NBOC" + this.OrderID, (object) value, DisclosureTracking2015Log.NBOUpdatableFields.ActualReceivedDate);
      }
    }

    /// <summary>Get or Set the NBO's isBorrowerTypeLocked</summary>
    public bool IsBorrowerTypeLocked
    {
      get
      {
        return Utils.ParseBoolean((object) this.parentLog.GetnboAttributeValue("NBOC" + this.OrderID, DisclosureTracking2015Log.NBOUpdatableFields.isBorrowerTypeLocked));
      }
      set
      {
        if (this.parentLog == null)
          return;
        this.parentLog.SetnboAttributeValue("NBOC" + this.OrderID, (object) value, DisclosureTracking2015Log.NBOUpdatableFields.isBorrowerTypeLocked);
      }
    }

    /// <summary>Get or Set the NBO's BorrowerType</summary>
    public string BorrowerType
    {
      get
      {
        return this.parentLog.GetnboAttributeValue("NBOC" + this.OrderID, DisclosureTracking2015Log.NBOUpdatableFields.BorrowerType);
      }
    }

    /// <summary>Get or Set the NBO's LockedBorrowerType</summary>
    public string LockedBorrowerType
    {
      get
      {
        return this.parentLog.GetnboAttributeValue("NBOC" + this.OrderID, DisclosureTracking2015Log.NBOUpdatableFields.LockedBorrowerType);
      }
    }

    /// <summary>
    /// Returns true id eDisclosure delivery method is selected
    /// </summary>
    public bool IseDisclosure
    {
      get => this.parentLog.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.eDisclosure;
    }

    /// <summary>Returns eDisclosure status</summary>
    public EDisclosureStatusForNBO EDisclosureStatus
    {
      get => !this.IseDisclosure ? (EDisclosureStatusForNBO) null : this.eDisclosures;
    }

    internal NonBorrowerOwner(INonBorrowerOwnerItem item)
    {
      this.FirstName = item.FirstName;
      this.MidName = item.MidName;
      this.LastName = item.LastName;
      this.Suffix = item.Suffix;
      this.Address = item.Address;
      this.City = item.City;
      this.State = item.State;
      this.Zip = item.Zip;
      this.VestingType = item.VestingType;
      this.HomePhone = item.HomePhone;
      this.Email = item.Email;
      this.IsNoThirdPartyEmail = item.IsNoThirdPartyEmail;
      this.BusiPhone = item.BusiPhone;
      this.Cell = item.Cell;
      this.Fax = item.Fax;
      this.DOB = item.DOB;
      this.parentLog = item.DTLog;
      this.OrderID = item.OrderID;
      this.DisclosedMethod = (DeliveryMethod2015) item.DisclosedMethod;
      this.DisclosedMethodOther = item.DisclosedMethodOther;
      this.PresumedReceivedDate = item.PresumedReceivedDate;
      this.lockedPresumedReceivedDate = item.lockedPresumedReceivedDate;
      this.IsPresumedDateLocked = item.isPresumedDateLocked;
      this.ActualReceivedDate = item.ActualReceivedDate;
      this.IsBorrowerTypeLocked = item.isBorrowerTypeLocked;
      this.borrowerType = item.BorrowerType;
      this.lockedBorrowerType = item.LockedBorrowerType;
      this.iseDisclosure = this.parentLog.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.eDisclosure;
      this.TRGuid = item.TRGuid;
      this.eDisclosures = new EDisclosureStatusForNBO(item);
    }
  }
}
