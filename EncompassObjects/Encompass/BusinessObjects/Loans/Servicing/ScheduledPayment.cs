// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Servicing.ScheduledPayment
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Servicing
{
  /// <summary>
  /// Represents a single payment within a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Servicing.PaymentSchedule" />.
  /// </summary>
  public class ScheduledPayment : IScheduledPayment
  {
    private int index;
    private EllieMae.EMLite.DataEngine.PaymentSchedule data;

    /// <summary>ScheduledPayment</summary>
    /// <param name="index"></param>
    /// <param name="data"></param>
    public ScheduledPayment(int index, EllieMae.EMLite.DataEngine.PaymentSchedule data)
    {
      this.index = index;
      this.data = data;
    }

    /// <summary>Gets the index of the payment in the schedule.</summary>
    public int Index => this.index;

    /// <summary>Gets the date on which the payment is due.</summary>
    public DateTime DueDate => Convert.ToDateTime(this.data.PayDate);

    /// <summary>
    /// Gets the interest rate on the loan at the time of the payment.
    /// </summary>
    public Decimal InterestRate => Convert.ToDecimal(this.data.CurrentRate);

    /// <summary>Gets the amount of principal included in the payment.</summary>
    public Decimal Principal => Convert.ToDecimal(this.data.Principal);

    /// <summary>Gets the amount of interest included in the payment.</summary>
    public Decimal Interest => Convert.ToDecimal(this.data.Interest);

    /// <summary>
    /// Gets the amount of mortgage insurance included in the payment.
    /// </summary>
    public Decimal MortgageInsurance => Convert.ToDecimal(this.data.MortgageInsurance);

    /// <summary>Gets the remaining balance on the loan.</summary>
    public Decimal Balance => Convert.ToDecimal(this.data.Balance);

    /// <summary>Gets the total amount due for the payment.</summary>
    public Decimal Total => Convert.ToDecimal(this.data.Payment);
  }
}
