// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Servicing.ScheduledPayments
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Servicing
{
  public class ScheduledPayments : IScheduledPayments, IEnumerable
  {
    private ScheduledPayment[] payments;

    internal ScheduledPayments(PaymentSchedule[] data)
    {
      this.payments = new ScheduledPayment[data.Length];
      for (int index = 0; index < data.Length; ++index)
        this.payments[index] = new ScheduledPayment(index + 1, data[index]);
    }

    public int Count => this.payments.Length;

    public ScheduledPayment this[int index] => this.payments[index - 1];

    public IEnumerator GetEnumerator() => this.payments.GetEnumerator();
  }
}
