// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.DisclosedVestingFieldsForDisclosure2015
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>
  /// The object model to represent the Vesting details as part of Disclosure2015
  /// </summary>
  public class DisclosedVestingFieldsForDisclosure2015 : IDisclosedVestingFieldsForDisclosure2015
  {
    private readonly string name;
    private readonly string aliasName;
    private readonly string ssn;
    private readonly VestingType vType;
    private readonly string borrowerPairID;
    private readonly TrusteeType trusteeType;
    private readonly string powerOfAttorney;
    private readonly string vesting;
    private readonly bool isAuthorizedToSign;
    private readonly string vestingGuid;
    private readonly string poaSignatureText;
    private readonly DateTime dob;
    private readonly OccupancyStatus status;
    private readonly OccupancyIntent intent;
    private readonly string nboGuid;
    private readonly int orderID;

    internal DisclosedVestingFieldsForDisclosure2015(
      string Name,
      string Alias,
      string SSN,
      VestingType VType,
      string BorrowerPairID,
      TrusteeType tType,
      string PowerOfAttorney,
      string Vesting,
      bool IsAuthorizedToSign,
      string VestingGuid,
      string POASignatureText,
      DateTime DOB,
      OccupancyStatus Status,
      OccupancyIntent Intent,
      string NBOGuid,
      int OrderId)
    {
      this.name = Name;
      this.aliasName = Alias;
      this.ssn = SSN;
      this.vType = VType;
      this.borrowerPairID = BorrowerPairID;
      this.trusteeType = tType;
      this.powerOfAttorney = PowerOfAttorney;
      this.vesting = Vesting;
      this.isAuthorizedToSign = IsAuthorizedToSign;
      this.vestingGuid = VestingGuid;
      this.poaSignatureText = POASignatureText;
      this.dob = DOB;
      this.status = Status;
      this.intent = Intent;
      this.nboGuid = NBOGuid;
      this.orderID = OrderId;
    }

    /// <summary>Gets the OrderId</summary>
    public int OrderID => this.orderID;

    /// <summary>Gets the Name</summary>
    public string Name => this.name;

    /// <summary>Gets the Alias Name</summary>
    public string AliasName => this.aliasName;

    /// <summary>Gets the SSN Name</summary>
    public string SSN => this.ssn;

    /// <summary>Gets the Vesting Type</summary>
    public VestingType VestingType => this.vType;

    /// <summary>Gets the BorrowerPair Id</summary>
    public string BorrowerPairID => this.borrowerPairID;

    /// <summary>Gets the Trustee Type</summary>
    public TrusteeType TrusteeType => this.trusteeType;

    /// <summary>Gets the PowerOfAttorney value</summary>
    public string PowerOfAttorney => this.powerOfAttorney;

    /// <summary>Gets the value for Vesting</summary>
    public string Vesting => this.vesting;

    /// <summary>Checks if Authorized to Sign</summary>
    public bool IsAuthorizedToSign => this.isAuthorizedToSign;

    /// <summary>Gets the Guid for Vesting details</summary>
    public string VestingGuid => this.vestingGuid;

    /// <summary>gets the POA Signature text</summary>
    public string POASignatureText => this.poaSignatureText;

    /// <summary>Gets the Date Of Birth</summary>
    public DateTime DOB => this.dob;

    /// <summary>Gets the Occupancy Status</summary>
    public OccupancyStatus Status => this.status;

    /// <summary>Gets the Occupancy Intent</summary>
    public OccupancyIntent Intent => this.intent;

    /// <summary>Gets the NBO Guid</summary>
    public string NBOGuid => this.nboGuid;
  }
}
