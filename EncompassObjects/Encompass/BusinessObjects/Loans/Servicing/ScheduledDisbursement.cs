// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Servicing.ScheduledDisbursement
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.DataEngine;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Servicing
{
  /// <summary>
  /// Represents a single escrow disbursement within a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Servicing.PaymentSchedule" />.
  /// </summary>
  public class ScheduledDisbursement : IScheduledDisbursement
  {
    private int index;
    private EscrowSchedule data;

    /// <summary>Constructor</summary>
    /// <param name="index"></param>
    /// <param name="data"></param>
    public ScheduledDisbursement(int index, EscrowSchedule data)
    {
      this.index = index;
      this.data = data;
    }

    /// <summary>
    /// Gets the index of this disbursement in the payment schedule.
    /// </summary>
    public int Index => this.index;

    /// <summary>
    /// Gets the amount of taxes payable for this disbursement.
    /// </summary>
    public Decimal Taxes => Convert.ToDecimal(this.data.Tax);

    /// <summary>Gets the date on which the tax payment is due.</summary>
    public DateTime TaxesDueDate => this.data.PayDate_Tax;

    /// <summary>
    /// Gets the amount of hazard insurance payable for this disbursement.
    /// </summary>
    public Decimal HazardInsurance => Convert.ToDecimal(this.data.HazardInsurance);

    /// <summary>
    /// Gets the date on which the hazard insurance payment is due.
    /// </summary>
    public DateTime HazardInsuranceDueDate => this.data.PayDate_Hazard;

    /// <summary>
    /// Gets the amount of mortgage insurance payable for this disbursement.
    /// </summary>
    public Decimal MortgageInsurance => Convert.ToDecimal(this.data.MortgageInsurance);

    /// <summary>
    /// Gets the date on which the mortgage insurance payment is due.
    /// </summary>
    public DateTime MortgageInsuranceDueDate => this.data.PayDate_Mortgage;

    /// <summary>
    /// Gets the amount of flood insurance payable for this disbursement.
    /// </summary>
    public Decimal FloodInsurance => Convert.ToDecimal(this.data.FloodInsurance);

    /// <summary>
    /// Gets the date on which the flood insurance payment is due.
    /// </summary>
    public DateTime FloodInsuranceDueDate => this.data.PayDate_Flood;

    /// <summary>
    /// Gets the amount of city property tax payable for this disbursement.
    /// </summary>
    public Decimal CityTax => Convert.ToDecimal(this.data.CityTax);

    /// <summary>
    /// Gets the date on which the city property tax payment is due.
    /// </summary>
    public DateTime CityTaxDueDate => this.data.PayDate_City;

    /// <summary>
    /// Gets the first amount of user-defined fees for this disbursement.
    /// </summary>
    public Decimal UserFee1 => Convert.ToDecimal(this.data.UserTax1);

    /// <summary>
    /// Gets the date on which the first user-defined fee is due.
    /// </summary>
    public DateTime UserFee1DueDate => this.data.PayDate_User1;

    /// <summary>
    /// Gets the second amount of user-defined fees for this disbursement.
    /// </summary>
    public Decimal UserFee2 => Convert.ToDecimal(this.data.UserTax2);

    /// <summary>
    /// Gets the date on which the second user-defined fee is due.
    /// </summary>
    public DateTime UserFee2DueDate => this.data.PayDate_User2;

    /// <summary>
    /// Gets the third amount of user-defined fees for this disbursement.
    /// </summary>
    public Decimal UserFee3 => Convert.ToDecimal(this.data.UserTax3);

    /// <summary>
    /// Gets the date on which the third user-defined fee is due.
    /// </summary>
    public DateTime UserFee3DueDate => this.data.PayDate_User3;
  }
}
