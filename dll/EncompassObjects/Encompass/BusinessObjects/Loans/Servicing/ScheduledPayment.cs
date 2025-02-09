// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Servicing.ScheduledPayment
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Servicing
{
  public class ScheduledPayment : IScheduledPayment
  {
    private int index;
    private PaymentSchedule data;

    public ScheduledPayment(int index, PaymentSchedule data)
    {
      this.index = index;
      this.data = data;
    }

    public int Index => this.index;

    public DateTime DueDate => Convert.ToDateTime(this.data.PayDate);

    public Decimal InterestRate => Convert.ToDecimal(this.data.CurrentRate);

    public Decimal Principal => Convert.ToDecimal(this.data.Principal);

    public Decimal Interest => Convert.ToDecimal(this.data.Interest);

    public Decimal MortgageInsurance => Convert.ToDecimal(this.data.MortgageInsurance);

    public Decimal Balance => Convert.ToDecimal(this.data.Balance);

    public Decimal Total => Convert.ToDecimal(this.data.Payment);
  }
}
