// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Servicing.PaymentSchedule
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.DataEngine;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Servicing
{
  /// <summary>Represents a payment schedule for paying off a loan.</summary>
  public class PaymentSchedule : IPaymentSchedule
  {
    private PaymentScheduleSnapshot snapshot;
    private ScheduledPayments payments;
    private ScheduledDisbursements disbursements;

    internal PaymentSchedule(PaymentScheduleSnapshot snapshot)
    {
      this.snapshot = snapshot;
      this.payments = new ScheduledPayments(snapshot.MonthlyPayments);
      this.disbursements = new ScheduledDisbursements(snapshot.EscrowPayments);
    }

    /// <summary>
    /// Gets the collection of principal and interest payments based on the schedule.
    /// </summary>
    public ScheduledPayments Payments => this.payments;

    /// <summary>
    /// Gets the collection of escrow disbursements based on the schedule.
    /// </summary>
    public ScheduledDisbursements Disbursements => this.disbursements;

    /// <summary>
    /// Gets the number of days per year used to calculate the payment schedule.
    /// </summary>
    public int DaysPerYear => this.snapshot.DaysPerYear;

    /// <summary>
    /// Gets the term of the loan as specified in the loan contract.
    /// </summary>
    public int LoanTerm => this.snapshot.LoanTerm;

    /// <summary>
    /// Gets the total number of monthly payments based on the payment schedule.
    /// </summary>
    public int ActualTerm => this.snapshot.ActualNumberOfTerm;

    /// <summary>
    /// Gets a flag indicating if the payment schedule is for an adjustable rate mortgage.
    /// </summary>
    public bool IsARM => this.snapshot.IsARMLoan;

    /// <summary>
    /// Gets a flag indicating if the payment schedule is based on biweekly payments.
    /// </summary>
    public bool IsBiweekly => this.snapshot.IsBiweekly;

    /// <summary>
    /// Gets a flag indicating if the payment schedule is for a negative ARM.
    /// </summary>
    public bool IsNegativeARM => this.snapshot.IsNegativeARM;

    /// <summary>
    /// Gets the loan amount on which the payment schedule is based.
    /// </summary>
    public Decimal LoanAmount => Convert.ToDecimal(this.snapshot.LoanAmount);

    /// <summary>Gets the margin rate for an adjustable rate mortgage.</summary>
    public Decimal MarginRate => Convert.ToDecimal(this.snapshot.MarginRate);

    /// <summary>Gets the minimum late fee for a late payment.</summary>
    /// <remarks>This property returns 0 if no minimum fee is set.</remarks>
    public Decimal MinLateFee => Convert.ToDecimal(this.snapshot.MinLateCharge);

    /// <summary>Gets the maximum late fee for a late payment.</summary>
    /// <remarks>This property returns 0 if no maximum fee is set.</remarks>
    public Decimal MaxLateFee
    {
      get
      {
        return this.snapshot.MaxLateCharge < double.MaxValue && this.snapshot.MaxLateCharge != double.PositiveInfinity ? Convert.ToDecimal(this.snapshot.MaxLateCharge) : 0M;
      }
    }

    /// <summary>Gets the initial note rate of the loan.</summary>
    public Decimal NoteRate => Convert.ToDecimal(this.snapshot.NoteRate);

    /// <summary>Gets the number of scheduled payments per year.</summary>
    public int PaymentsPerYear => this.snapshot.NumberPayPerYear;

    /// <summary>
    /// Gets the number of days in which payment is due from the time of the scheduled payment.
    /// </summary>
    public int PaymentDueDays => this.snapshot.PaymentDueDays;

    /// <summary>
    /// Gets the number of days from the time of the scheduled payment to the time the statement should be printed.
    /// </summary>
    public int StatementDueDays => this.snapshot.StatementPrintDueDay;
  }
}
