// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.EDisclosureStatusForNBO
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
  /// <summary>Represents the Edisclosure properties of NBO.</summary>
  /// 
  ///             The eDisclosure properties are only getters. That is readonly.
  ///             There is no updatable eDisclosure properties exposed to SDK users.
  public class EDisclosureStatusForNBO : IEDisclosureStatusForNBO
  {
    private IDisclosureTracking2015Log parentLog;
    private INonBorrowerOwnerItem item;

    internal EDisclosureStatusForNBO(INonBorrowerOwnerItem nboItem)
    {
      this.parentLog = nboItem.DTLog;
      this.item = nboItem;
    }

    /// <summary>Get the NBO's eDisclosureNBOCAuthenticatedDate</summary>
    public DateTime eDisclosureNBOCAuthenticatedDate
    {
      get
      {
        return Utils.ParseDate((object) this.parentLog.GetnboAttributeValue("NBOC" + this.item.OrderID, DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBOCAuthenticatedDate));
      }
    }

    /// <summary>Get the NBO's eDisclosureNBOCAuthenticatedIP</summary>
    public string eDisclosureNBOCAuthenticatedIP
    {
      get
      {
        return this.parentLog.GetnboAttributeValue("NBOC" + this.item.OrderID, DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBOCAuthenticatedIP);
      }
    }

    /// <summary>Get the NBO's eDisclosureNBOCViewMessageDate</summary>
    public DateTime eDisclosureNBOCViewMessageDate
    {
      get
      {
        return Utils.ParseDate((object) this.parentLog.GetnboAttributeValue("NBOC" + this.item.OrderID, DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBOCViewMessageDate));
      }
    }

    /// <summary>Get the NBO's eDisclosureNBOCRejectConsentDate</summary>
    public DateTime eDisclosureNBOCRejectConsentDate
    {
      get
      {
        return Utils.ParseDate((object) this.parentLog.GetnboAttributeValue("NBOC" + this.item.OrderID, DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBOCRejectConsentDate));
      }
    }

    /// <summary>Get the NBO's eDisclosureNBOCRejectConsentIP</summary>
    public string eDisclosureNBOCRejectConsentIP
    {
      get
      {
        return this.parentLog.GetnboAttributeValue("NBOC" + this.item.OrderID, DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBOCRejectConsentIP);
      }
    }

    /// <summary>Get the NBO's eDisclosureNBOCSignedDate</summary>
    public DateTime eDisclosureNBOCSignedDate
    {
      get
      {
        return Utils.ParseDate((object) this.parentLog.GetnboAttributeValue("NBOC" + this.item.OrderID, DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBOCSignedDate));
      }
    }

    /// <summary>Get the NBO's eDisclosureNBOCeSignedIP</summary>
    public string eDisclosureNBOCeSignedIP
    {
      get
      {
        return this.parentLog.GetnboAttributeValue("NBOC" + this.item.OrderID, DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBOCeSignedIP);
      }
    }

    /// <summary>Get the NBO's eDisclosure Loan Level Consent</summary>
    public string eDisclosureNBOLoanLevelConsent
    {
      get
      {
        return this.parentLog.GetnboAttributeValue("NBOC" + this.item.OrderID, DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBOLoanLevelConsent);
      }
    }

    /// <summary>Get NBO's eDisclosure Accept Consent Date</summary>
    public DateTime eDisclosureNBOAcceptConsentDate
    {
      get
      {
        return Utils.ParseDate((object) this.parentLog.GetnboAttributeValue("NBOC" + this.item.OrderID, DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBOAcceptConsentDate));
      }
    }

    /// <summary>Get NBO's eDisclosure AcceptConsentIP</summary>
    public string eDisclosureNBOAcceptConsentIP
    {
      get
      {
        return this.parentLog.GetnboAttributeValue("NBOC" + this.item.OrderID, DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBOAcceptConsentIP);
      }
    }

    /// <summary>Get NBO's eDisclosure DocuementViewedDate</summary>
    public DateTime eDisclosureNBODocumentViewedDate
    {
      get
      {
        return Utils.ParseDate((object) this.parentLog.GetnboAttributeValue("NBOC" + this.item.OrderID, DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBODocumentViewedDate));
      }
    }
  }
}
