// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Servicing.IScheduledDisbursement
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Servicing
{
  [Guid("D1E43AB6-5E85-4e55-8EA5-039DCE9AE17E")]
  public interface IScheduledDisbursement
  {
    int Index { get; }

    Decimal Taxes { get; }

    DateTime TaxesDueDate { get; }

    Decimal HazardInsurance { get; }

    DateTime HazardInsuranceDueDate { get; }

    Decimal MortgageInsurance { get; }

    DateTime MortgageInsuranceDueDate { get; }

    Decimal FloodInsurance { get; }

    DateTime FloodInsuranceDueDate { get; }

    Decimal CityTax { get; }

    DateTime CityTaxDueDate { get; }

    Decimal UserFee1 { get; }

    DateTime UserFee1DueDate { get; }

    Decimal UserFee2 { get; }

    DateTime UserFee2DueDate { get; }

    Decimal UserFee3 { get; }

    DateTime UserFee3DueDate { get; }
  }
}
