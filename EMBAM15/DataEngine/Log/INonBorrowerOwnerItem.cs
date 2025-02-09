// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.INonBorrowerOwnerItem
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using System;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public interface INonBorrowerOwnerItem
  {
    string FirstName { get; }

    string MidName { get; }

    string LastName { get; }

    string Suffix { get; }

    string Address { get; }

    string City { get; }

    string State { get; }

    string Zip { get; }

    string VestingType { get; }

    string HomePhone { get; }

    string Email { get; }

    bool IsNoThirdPartyEmail { get; }

    string BusiPhone { get; }

    string Cell { get; }

    string Fax { get; }

    DateTime DOB { get; }

    string OrderID { get; }

    DateTime ActualReceivedDate { get; set; }

    string BorrowerType { get; set; }

    int DisclosedMethod { get; set; }

    string DisclosedMethodOther { get; set; }

    DateTime PresumedReceivedDate { get; set; }

    DateTime lockedPresumedReceivedDate { get; set; }

    bool isPresumedDateLocked { get; set; }

    bool isBorrowerTypeLocked { get; set; }

    string LockedBorrowerType { get; set; }

    DateTime eDisclosureNBOAuthenticatedDate { get; set; }

    string eDisclosureNBOAuthenticatedIP { get; set; }

    string eDisclosureNBOeSignedIP { get; set; }

    DateTime eDisclosureNBORejectConsentDate { get; set; }

    string eDisclosureNBORejectConsentIP { get; set; }

    DateTime eDisclosureNBOSignedDate { get; set; }

    DateTime eDisclosureNBOViewMessageDate { get; set; }

    string eDisclosureNBOLoanLevelConsent { get; set; }

    DateTime eDisclosureNBOAcceptConsentDate { get; set; }

    string eDisclosureNBOAcceptConsentIP { get; set; }

    DateTime eDisclosureNBODocumentViewedDate { get; set; }

    bool eDisclosureNBOeSignatures { get; set; }

    string TRGuid { get; }

    IDisclosureTracking2015Log DTLog { get; }

    INonBorrowerOwnerItem CloneForDuplicate();
  }
}
