// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Servicing.ScheduledPayments
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Servicing
{
  /// <summary>
  /// Represents a collection of <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Servicing.ScheduledPayment" /> objects within a
  /// <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Servicing.PaymentSchedule" />.
  /// </summary>
  public class ScheduledPayments : IScheduledPayments, IEnumerable
  {
    private ScheduledPayment[] payments;

    internal ScheduledPayments(EllieMae.EMLite.DataEngine.PaymentSchedule[] data)
    {
      this.payments = new ScheduledPayment[data.Length];
      for (int index = 0; index < data.Length; ++index)
        this.payments[index] = new ScheduledPayment(index + 1, data[index]);
    }

    /// <summary>Gets the number of payments in the payment schedule.</summary>
    public int Count => this.payments.Length;

    /// <summary>
    /// Gets a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Servicing.ScheduledPayment" /> by index.
    /// </summary>
    /// <param name="index">The 1-based index of the desired payment.</param>
    /// <returns>The specified ScheduledPayment object.</returns>
    public ScheduledPayment this[int index] => this.payments[index - 1];

    /// <summary>
    /// Returns an enumerator for iterating over the collection of payments.
    /// </summary>
    /// <returns>Returns an IEnumerator which iterates over the payments in order based on index.</returns>
    public IEnumerator GetEnumerator() => this.payments.GetEnumerator();
  }
}
