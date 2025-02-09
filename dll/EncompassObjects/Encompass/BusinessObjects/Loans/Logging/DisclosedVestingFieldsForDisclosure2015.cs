// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.DisclosedVestingFieldsForDisclosure2015
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
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

    public int OrderID => this.orderID;

    public string Name => this.name;

    public string AliasName => this.aliasName;

    public string SSN => this.ssn;

    public VestingType VestingType => this.vType;

    public string BorrowerPairID => this.borrowerPairID;

    public TrusteeType TrusteeType => this.trusteeType;

    public string PowerOfAttorney => this.powerOfAttorney;

    public string Vesting => this.vesting;

    public bool IsAuthorizedToSign => this.isAuthorizedToSign;

    public string VestingGuid => this.vestingGuid;

    public string POASignatureText => this.poaSignatureText;

    public DateTime DOB => this.dob;

    public OccupancyStatus Status => this.status;

    public OccupancyIntent Intent => this.intent;

    public string NBOGuid => this.nboGuid;
  }
}
