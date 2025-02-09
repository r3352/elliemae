// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.IDisclosedVestingFieldsForDisclosure2015
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  [Guid("EFA5960C-67F0-4813-BEF8-E5FE208C0D36")]
  public interface IDisclosedVestingFieldsForDisclosure2015
  {
    string Name { get; }

    string AliasName { get; }

    string SSN { get; }

    VestingType VestingType { get; }

    string BorrowerPairID { get; }

    TrusteeType TrusteeType { get; }

    string PowerOfAttorney { get; }

    string Vesting { get; }

    bool IsAuthorizedToSign { get; }

    string VestingGuid { get; }

    string POASignatureText { get; }

    DateTime DOB { get; }

    OccupancyStatus Status { get; }

    OccupancyIntent Intent { get; }

    string NBOGuid { get; }
  }
}
