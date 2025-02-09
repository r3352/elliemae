// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.NonBorrowerOwner
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.Log;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  public class NonBorrowerOwner : INonBorrowerOwner
  {
    public readonly string FirstName;
    public readonly string MidName;
    public readonly string LastName;
    public readonly string Suffix;
    public readonly string Address;
    public readonly string City;
    public readonly string State;
    public readonly string Zip;
    public readonly string VestingType;
    public readonly string HomePhone;
    public readonly string Email;
    public readonly bool IsNoThirdPartyEmail;
    public readonly string BusiPhone;
    public readonly string Cell;
    public readonly string Fax;
    public readonly DateTime DOB;
    public readonly string TRGuid;
    public readonly string OrderID;
    private string borrowerType;
    private string lockedBorrowerType;
    private IDisclosureTracking2015Log parentLog;
    private bool iseDisclosure;
    private EDisclosureStatusForNBO eDisclosures;

    public DeliveryMethod2015 DisclosedMethod
    {
      get
      {
        return (DeliveryMethod2015) Utils.ParseInt((object) this.parentLog.GetnboAttributeValue("NBOC" + this.OrderID, (DisclosureTracking2015Log.NBOUpdatableFields) 0), false, -1);
      }
      set
      {
        if (this.parentLog == null)
          return;
        this.parentLog.SetnboAttributeValue("NBOC" + this.OrderID, (object) (int) value, (DisclosureTracking2015Log.NBOUpdatableFields) 0);
      }
    }

    public string DisclosedMethodOther
    {
      get
      {
        return this.parentLog.GetnboAttributeValue("NBOC" + this.OrderID, (DisclosureTracking2015Log.NBOUpdatableFields) 1);
      }
      set
      {
        if (this.parentLog == null)
          return;
        this.parentLog.SetnboAttributeValue("NBOC" + this.OrderID, (object) value, (DisclosureTracking2015Log.NBOUpdatableFields) 1);
      }
    }

    public DateTime PresumedReceivedDate
    {
      get
      {
        return Utils.ParseDate((object) this.parentLog.GetnboAttributeValue("NBOC" + this.OrderID, (DisclosureTracking2015Log.NBOUpdatableFields) 2));
      }
      set
      {
        if (this.parentLog == null)
          return;
        this.parentLog.SetnboAttributeValue("NBOC" + this.OrderID, (object) value, (DisclosureTracking2015Log.NBOUpdatableFields) 2);
      }
    }

    public DateTime lockedPresumedReceivedDate
    {
      get
      {
        return Utils.ParseDate((object) this.parentLog.GetnboAttributeValue("NBOC" + this.OrderID, (DisclosureTracking2015Log.NBOUpdatableFields) 3));
      }
      set
      {
        if (this.parentLog == null)
          return;
        this.parentLog.SetnboAttributeValue("NBOC" + this.OrderID, (object) value, (DisclosureTracking2015Log.NBOUpdatableFields) 3);
      }
    }

    public bool IsPresumedDateLocked
    {
      get
      {
        return Utils.ParseBoolean((object) this.parentLog.GetnboAttributeValue("NBOC" + this.OrderID, (DisclosureTracking2015Log.NBOUpdatableFields) 4));
      }
      set
      {
        if (this.parentLog == null)
          return;
        this.parentLog.SetnboAttributeValue("NBOC" + this.OrderID, (object) value, (DisclosureTracking2015Log.NBOUpdatableFields) 4);
      }
    }

    public DateTime ActualReceivedDate
    {
      get
      {
        return Utils.ParseDate((object) this.parentLog.GetnboAttributeValue("NBOC" + this.OrderID, (DisclosureTracking2015Log.NBOUpdatableFields) 5));
      }
      set
      {
        if (this.parentLog == null)
          return;
        this.parentLog.SetnboAttributeValue("NBOC" + this.OrderID, (object) value, (DisclosureTracking2015Log.NBOUpdatableFields) 5);
      }
    }

    public bool IsBorrowerTypeLocked
    {
      get
      {
        return Utils.ParseBoolean((object) this.parentLog.GetnboAttributeValue("NBOC" + this.OrderID, (DisclosureTracking2015Log.NBOUpdatableFields) 6));
      }
      set
      {
        if (this.parentLog == null)
          return;
        this.parentLog.SetnboAttributeValue("NBOC" + this.OrderID, (object) value, (DisclosureTracking2015Log.NBOUpdatableFields) 6);
      }
    }

    public string BorrowerType
    {
      get
      {
        return this.parentLog.GetnboAttributeValue("NBOC" + this.OrderID, (DisclosureTracking2015Log.NBOUpdatableFields) 7);
      }
    }

    public string LockedBorrowerType
    {
      get
      {
        return this.parentLog.GetnboAttributeValue("NBOC" + this.OrderID, (DisclosureTracking2015Log.NBOUpdatableFields) 8);
      }
    }

    public bool IseDisclosure => (int) this.parentLog.DisclosureMethod == 2;

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
      this.iseDisclosure = this.parentLog.DisclosureMethod == 2;
      this.TRGuid = item.TRGuid;
      this.eDisclosures = new EDisclosureStatusForNBO(item);
    }
  }
}
