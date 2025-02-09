// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Servicing.ScheduledDisbursements
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Servicing
{
  public class ScheduledDisbursements : IScheduledDisbursements, IEnumerable
  {
    private ScheduledDisbursement[] payments;

    internal ScheduledDisbursements(EscrowSchedule[] data)
    {
      ArrayList arrayList = new ArrayList();
      for (int index = 0; index < data.Length && data[index] != null; ++index)
        arrayList.Add((object) new ScheduledDisbursement(index + 1, data[index]));
      this.payments = (ScheduledDisbursement[]) arrayList.ToArray(typeof (ScheduledDisbursement));
    }

    public int Count => this.payments.Length;

    public ScheduledDisbursement this[int index] => this.payments[index - 1];

    public IEnumerator GetEnumerator() => this.payments.GetEnumerator();
  }
}
