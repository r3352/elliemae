// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Servicing.ServicingDisbursementType
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Servicing
{
  /// <summary>
  /// Enumeration for the support interim servicing disbursement types.
  /// </summary>
  [Guid("62835443-6B8E-491c-B1E2-7E7D68FEE296")]
  public enum ServicingDisbursementType
  {
    /// <summary>No disbursement type sepcified.</summary>
    None,
    /// <summary>Disbursement to pay property taxes.</summary>
    Taxes,
    /// <summary>Disbursement to pay property/hazard insurance.</summary>
    HazardInsurance,
    /// <summary>Disbursement to pay mortgage insurance.</summary>
    MortgageInsurance,
    /// <summary>Disbursement to pay flood insurance.</summary>
    FloodInsurance,
    /// <summary>Disbursement to pay city-specific property taxes.</summary>
    CityPropertyTax,
    /// <summary>User-defined disbursement type.</summary>
    User1,
    /// <summary>User-defined disbursement type.</summary>
    User2,
    /// <summary>User-defined disbursement type.</summary>
    User3,
  }
}
