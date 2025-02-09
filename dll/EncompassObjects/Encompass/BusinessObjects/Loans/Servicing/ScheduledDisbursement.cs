// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Servicing.ScheduledDisbursement
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Servicing
{
  public class ScheduledDisbursement : IScheduledDisbursement
  {
    private int index;
    private EscrowSchedule data;

    public ScheduledDisbursement(int index, EscrowSchedule data)
    {
      this.index = index;
      this.data = data;
    }

    public int Index => this.index;

    public Decimal Taxes => Convert.ToDecimal(this.data.Tax);

    public DateTime TaxesDueDate => this.data.PayDate_Tax;

    public Decimal HazardInsurance => Convert.ToDecimal(this.data.HazardInsurance);

    public DateTime HazardInsuranceDueDate => this.data.PayDate_Hazard;

    public Decimal MortgageInsurance => Convert.ToDecimal(this.data.MortgageInsurance);

    public DateTime MortgageInsuranceDueDate => this.data.PayDate_Mortgage;

    public Decimal FloodInsurance => Convert.ToDecimal(this.data.FloodInsurance);

    public DateTime FloodInsuranceDueDate => this.data.PayDate_Flood;

    public Decimal CityTax => Convert.ToDecimal(this.data.CityTax);

    public DateTime CityTaxDueDate => this.data.PayDate_City;

    public Decimal UserFee1 => Convert.ToDecimal(this.data.UserTax1);

    public DateTime UserFee1DueDate => this.data.PayDate_User1;

    public Decimal UserFee2 => Convert.ToDecimal(this.data.UserTax2);

    public DateTime UserFee2DueDate => this.data.PayDate_User2;

    public Decimal UserFee3 => Convert.ToDecimal(this.data.UserTax3);

    public DateTime UserFee3DueDate => this.data.PayDate_User3;
  }
}
