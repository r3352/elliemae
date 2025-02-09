// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Servicing.EscrowDisbursementReversal
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
  public class EscrowDisbursementReversal : ServicingTransaction, IEscrowDisbursementReversal
  {
    internal EscrowDisbursementReversal(Loan loan, PaymentReversalLog trans)
      : base(loan, (ServicingTransactionBase) trans)
    {
    }

    /// <summary>
    /// Gets the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Servicing.ServicingTransactionType" /> for the transaction.
    /// </summary>
    public override ServicingTransactionType TransactionType
    {
      get => ServicingTransactionType.EscrowDisbursementReversal;
    }

    /// <summary>
    /// Gets the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Servicing.EscrowDisbursement" /> that was reversed by this transaction.
    /// </summary>
    public EscrowDisbursement GetDisbursement()
    {
      foreach (EscrowDisbursement transaction in (CollectionBase) this.Loan.Servicing.Transactions.GetTransactions(ServicingTransactionType.EscrowDisbursement))
      {
        if (transaction.ID == this.baseTrans.PaymentGUID)
          return transaction;
      }
      return (EscrowDisbursement) null;
    }

    private PaymentReversalLog baseTrans => (PaymentReversalLog) this.Unwrap();
  }
}
