// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Servicing.PaymentReversal
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.DataEngine.InterimServicing;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Servicing
{
  /// <summary>
  /// Represents an escrow disbursement servicing transaction.
  /// </summary>
  public class PaymentReversal : ServicingTransaction, IPaymentReversal
  {
    internal PaymentReversal(Loan loan, PaymentReversalLog trans)
      : base(loan, (ServicingTransactionBase) trans)
    {
    }

    /// <summary>
    /// Gets the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Servicing.Payment" /> that was reversed by this transaction.
    /// </summary>
    public Payment GetPayment()
    {
      foreach (Payment transaction in (CollectionBase) this.Loan.Servicing.Transactions.GetTransactions(ServicingTransactionType.Payment))
      {
        if (transaction.ID == this.baseTrans.PaymentGUID)
          return transaction;
      }
      return (Payment) null;
    }

    private PaymentReversalLog baseTrans => (PaymentReversalLog) this.Unwrap();
  }
}
