// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.CalculationTriggerOptions
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>
  /// Enumerates the calculation trigger that you can trigger the calculation.
  /// </summary>
  [Guid("D930807D-CB72-4E9E-A79E-6AC866E1E5FC")]
  public enum CalculationTriggerOptions
  {
    /// <summary>Apply DDM Rules</summary>
    ApplyDDM = 1,
    /// <summary>Trigger 2015 RESPA-TILA Calculation</summary>
    Calculation_2015RESPA = 2,
    /// <summary>Trigger Aggregate Escrow Account Calculation</summary>
    Calculation_AggregateEscrow = 3,
    /// <summary>Trigger City, County and State Tax Calculation</summary>
    Calculation_CityCountyStateTax = 4,
    /// <summary>Trigger FHA 203K Calculation</summary>
    Calculation_FHA203K = 5,
    /// <summary>Trigger Calculation for Mortgagae Insurance</summary>
    Calculation_MIP = 6,
    /// <summary>Trigger CA MLDS Calculation</summary>
    Calculation_MLDS = 7,
    /// <summary>Trigger Calculation for PTC and POC Fields in Fee Detail</summary>
    Calculation_PTCPOC = 8,
    /// <summary>Trigger Calculation for USDA Mortgagae Insurance</summary>
    Calculation_USDAMIP = 9,
    /// <summary>Trigger Calculation for TPO</summary>
    Calculation_TPO = 10, // 0x0000000A
    /// <summary>Trigger Calculation Liabilities to copy over to CD pg3</summary>
    Calculation_COPYLIABILIESTOCDPG3 = 11, // 0x0000000B
    /// <summary>Trigger Calculation for re calculating HMDA fields</summary>
    RECALCULATEHMDA = 12, // 0x0000000C
  }
}
