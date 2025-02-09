// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Servicing.PaymentReversal
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine.InterimServicing;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Servicing
{
  public class PaymentReversal : ServicingTransaction, IPaymentReversal
  {
    internal PaymentReversal(Loan loan, PaymentReversalLog trans)
      : base(loan, (ServicingTransactionBase) trans)
    {
    }

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
