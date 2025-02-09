// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Servicing.ScheduledDisbursements
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.DataEngine;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Servicing
{
  /// <summary>
  /// Represents a collection of <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Servicing.ScheduledDisbursement" /> objects within a
  /// <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Servicing.PaymentSchedule" />.
  /// </summary>
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

    /// <summary>
    /// Gets the number of disbursements in the payment schedule.
    /// </summary>
    public int Count => this.payments.Length;

    /// <summary>
    /// Gets a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Servicing.ScheduledDisbursement" /> by index.
    /// </summary>
    /// <param name="index">The 1-based index of the desired disbursements.</param>
    /// <returns>The specified ScheduledDisbursement object.</returns>
    public ScheduledDisbursement this[int index] => this.payments[index - 1];

    /// <summary>
    /// Returns an enumerator for iterating over the collection of disbursements.
    /// </summary>
    /// <returns>Returns an IEnumerator which iterates over the disbursements in order based on index.</returns>
    public IEnumerator GetEnumerator() => this.payments.GetEnumerator();
  }
}
