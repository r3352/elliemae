// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.NonBorrowingOwner
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using System;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  [Serializable]
  public class NonBorrowingOwner
  {
    public int NboIndex { get; set; }

    public int NBOVestingIndex { get; set; }

    public string FirstName { get; set; }

    public string MiddleName { get; set; }

    public string LastName { get; set; }

    public string SuffixName { get; set; }

    public string AddressStreet { get; set; }

    public string AddressCity { get; set; }

    public string AddressState { get; set; }

    public string AddressPostalCode { get; set; }

    public string BorrowerType { get; set; }

    public string HomePhoneNumber { get; set; }

    public string EmailAddress { get; set; }

    public bool? No3rdPartyEmailIndicator { get; set; }

    public string BusinessPhoneNumber { get; set; }

    public string CellPhoneNumber { get; set; }

    public string FaxNumber { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public string NBOID { get; set; }

    public string BorrowerVestingRecordID { get; set; }

    public string eSignConsentNBOCStatus { get; set; }

    public DateTime? eSignConsentNBOCDateAccepted { get; set; }

    public string eSignConsentNBOCIPAddress { get; set; }

    public string eSignConsentNBOCSource { get; set; }

    public DateTime? eSignConsentNBOCDateSent { get; set; }

    public string VestingBorrowerPairId { get; set; }

    public string EID { get; set; }
  }
}
