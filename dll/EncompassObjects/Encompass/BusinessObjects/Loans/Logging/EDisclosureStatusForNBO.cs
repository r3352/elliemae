// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.EDisclosureStatusForNBO
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.Log;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  public class EDisclosureStatusForNBO : IEDisclosureStatusForNBO
  {
    private IDisclosureTracking2015Log parentLog;
    private INonBorrowerOwnerItem item;

    internal EDisclosureStatusForNBO(INonBorrowerOwnerItem nboItem)
    {
      this.parentLog = nboItem.DTLog;
      this.item = nboItem;
    }

    public DateTime eDisclosureNBOCAuthenticatedDate
    {
      get
      {
        return Utils.ParseDate((object) this.parentLog.GetnboAttributeValue("NBOC" + this.item.OrderID, (DisclosureTracking2015Log.NBOUpdatableFields) 20));
      }
    }

    public string eDisclosureNBOCAuthenticatedIP
    {
      get
      {
        return this.parentLog.GetnboAttributeValue("NBOC" + this.item.OrderID, (DisclosureTracking2015Log.NBOUpdatableFields) 21);
      }
    }

    public DateTime eDisclosureNBOCViewMessageDate
    {
      get
      {
        return Utils.ParseDate((object) this.parentLog.GetnboAttributeValue("NBOC" + this.item.OrderID, (DisclosureTracking2015Log.NBOUpdatableFields) 26));
      }
    }

    public DateTime eDisclosureNBOCRejectConsentDate
    {
      get
      {
        return Utils.ParseDate((object) this.parentLog.GetnboAttributeValue("NBOC" + this.item.OrderID, (DisclosureTracking2015Log.NBOUpdatableFields) 23));
      }
    }

    public string eDisclosureNBOCRejectConsentIP
    {
      get
      {
        return this.parentLog.GetnboAttributeValue("NBOC" + this.item.OrderID, (DisclosureTracking2015Log.NBOUpdatableFields) 24);
      }
    }

    public DateTime eDisclosureNBOCSignedDate
    {
      get
      {
        return Utils.ParseDate((object) this.parentLog.GetnboAttributeValue("NBOC" + this.item.OrderID, (DisclosureTracking2015Log.NBOUpdatableFields) 25));
      }
    }

    public string eDisclosureNBOCeSignedIP
    {
      get
      {
        return this.parentLog.GetnboAttributeValue("NBOC" + this.item.OrderID, (DisclosureTracking2015Log.NBOUpdatableFields) 22);
      }
    }

    public string eDisclosureNBOLoanLevelConsent
    {
      get
      {
        return this.parentLog.GetnboAttributeValue("NBOC" + this.item.OrderID, (DisclosureTracking2015Log.NBOUpdatableFields) 16);
      }
    }

    public DateTime eDisclosureNBOAcceptConsentDate
    {
      get
      {
        return Utils.ParseDate((object) this.parentLog.GetnboAttributeValue("NBOC" + this.item.OrderID, (DisclosureTracking2015Log.NBOUpdatableFields) 17));
      }
    }

    public string eDisclosureNBOAcceptConsentIP
    {
      get
      {
        return this.parentLog.GetnboAttributeValue("NBOC" + this.item.OrderID, (DisclosureTracking2015Log.NBOUpdatableFields) 18);
      }
    }

    public DateTime eDisclosureNBODocumentViewedDate
    {
      get
      {
        return Utils.ParseDate((object) this.parentLog.GetnboAttributeValue("NBOC" + this.item.OrderID, (DisclosureTracking2015Log.NBOUpdatableFields) 19));
      }
    }
  }
}
