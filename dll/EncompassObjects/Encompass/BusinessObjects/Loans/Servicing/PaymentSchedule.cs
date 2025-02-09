// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Servicing.PaymentSchedule
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Servicing
{
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

    public ScheduledPayments Payments => this.payments;

    public ScheduledDisbursements Disbursements => this.disbursements;

    public int DaysPerYear => this.snapshot.DaysPerYear;

    public int LoanTerm => this.snapshot.LoanTerm;

    public int ActualTerm => this.snapshot.ActualNumberOfTerm;

    public bool IsARM => this.snapshot.IsARMLoan;

    public bool IsBiweekly => this.snapshot.IsBiweekly;

    public bool IsNegativeARM => this.snapshot.IsNegativeARM;

    public Decimal LoanAmount => Convert.ToDecimal(this.snapshot.LoanAmount);

    public Decimal MarginRate => Convert.ToDecimal(this.snapshot.MarginRate);

    public Decimal MinLateFee => Convert.ToDecimal(this.snapshot.MinLateCharge);

    public Decimal MaxLateFee
    {
      get
      {
        return this.snapshot.MaxLateCharge < double.MaxValue && this.snapshot.MaxLateCharge != double.PositiveInfinity ? Convert.ToDecimal(this.snapshot.MaxLateCharge) : 0M;
      }
    }

    public Decimal NoteRate => Convert.ToDecimal(this.snapshot.NoteRate);

    public int PaymentsPerYear => this.snapshot.NumberPayPerYear;

    public int PaymentDueDays => this.snapshot.PaymentDueDays;

    public int StatementDueDays => this.snapshot.StatementPrintDueDay;
  }
}
