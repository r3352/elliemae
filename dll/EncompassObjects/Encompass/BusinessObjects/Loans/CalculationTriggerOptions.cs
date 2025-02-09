// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.CalculationTriggerOptions
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  [Guid("D930807D-CB72-4E9E-A79E-6AC866E1E5FC")]
  public enum CalculationTriggerOptions
  {
    ApplyDDM = 1,
    Calculation_2015RESPA = 2,
    Calculation_AggregateEscrow = 3,
    Calculation_CityCountyStateTax = 4,
    Calculation_FHA203K = 5,
    Calculation_MIP = 6,
    Calculation_MLDS = 7,
    Calculation_PTCPOC = 8,
    Calculation_USDAMIP = 9,
    Calculation_TPO = 10, // 0x0000000A
    Calculation_COPYLIABILIESTOCDPG3 = 11, // 0x0000000B
    RECALCULATEHMDA = 12, // 0x0000000C
  }
}
