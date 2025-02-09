// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Servicing.LoanServicingTransactions
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine.InterimServicing;
using EllieMae.Encompass.Collections;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Servicing
{
  public class LoanServicingTransactions : ILoanServicingTransactions, IEnumerable
  {
    private Loan loan;

    internal LoanServicingTransactions(Loan loan) => this.loan = loan;

    public ServicingTransaction GetTransactionByID(string transId)
    {
      foreach (ServicingTransaction transactionById in this)
      {
        if (transactionById.ID == transId)
          return transactionById;
      }
      return (ServicingTransaction) null;
    }

    public ServicingTransactionList GetTransactions(ServicingTransactionType transType)
    {
      ServicingTransactionList transactions = new ServicingTransactionList();
      foreach (ServicingTransaction servicingTransaction in this)
      {
        if (servicingTransaction.TransactionType == transType)
          transactions.Add(servicingTransaction);
      }
      return transactions;
    }

    public Payment AddPayment(DateTime dateReceived)
    {
      this.loan.EnsureExclusive();
      PaymentTransactionLog servicingTransaction = (PaymentTransactionLog) this.loan.Unwrap().CreateNextServicingTransaction((ServicingTransactionTypes) 1);
      this.loan.Unwrap().PopulateNextServicingPaymentInformation(servicingTransaction);
      servicingTransaction.PaymentReceivedDate = dateReceived;
      this.loan.Unwrap().LoanData.AddServicingTransaction((ServicingTransactionBase) servicingTransaction);
      return new Payment(this.loan, servicingTransaction);
    }

    public EscrowDisbursement AddDisbursement(
      DateTime transDate,
      Decimal disAmount,
      ServicingDisbursementType disType)
    {
      this.loan.EnsureExclusive();
      EscrowDisbursementLog servicingTransaction = (EscrowDisbursementLog) this.loan.Unwrap().CreateNextServicingTransaction((ServicingTransactionTypes) 3);
      servicingTransaction.DisbursementType = (ServicingDisbursementTypes) disType;
      ((ServicingTransactionBase) servicingTransaction).TransactionDate = transDate;
      ((ServicingTransactionBase) servicingTransaction).TransactionAmount = Convert.ToDouble(disAmount);
      this.loan.Unwrap().LoanData.AddServicingTransaction((ServicingTransactionBase) servicingTransaction);
      return new EscrowDisbursement(this.loan, servicingTransaction);
    }

    public EscrowInterest AddEscrowInterest(DateTime transDate, Decimal transAmount)
    {
      this.loan.EnsureExclusive();
      EscrowInterestLog servicingTransaction = (EscrowInterestLog) this.loan.Unwrap().CreateNextServicingTransaction((ServicingTransactionTypes) 4);
      ((ServicingTransactionBase) servicingTransaction).TransactionDate = transDate;
      ((ServicingTransactionBase) servicingTransaction).TransactionAmount = Convert.ToDouble(transAmount);
      this.loan.Unwrap().LoanData.AddServicingTransaction((ServicingTransactionBase) servicingTransaction);
      return new EscrowInterest(this.loan, servicingTransaction);
    }

    public OtherTransaction AddOtherTransaction(DateTime transDate, Decimal transAmount)
    {
      this.loan.EnsureExclusive();
      OtherTransactionLog servicingTransaction = (OtherTransactionLog) this.loan.Unwrap().CreateNextServicingTransaction((ServicingTransactionTypes) 6);
      ((ServicingTransactionBase) servicingTransaction).TransactionDate = transDate;
      ((ServicingTransactionBase) servicingTransaction).TransactionAmount = Convert.ToDouble(transAmount);
      this.loan.Unwrap().LoanData.AddServicingTransaction((ServicingTransactionBase) servicingTransaction);
      return new OtherTransaction(this.loan, servicingTransaction);
    }

    public void Remove(ServicingTransaction trans)
    {
      this.loan.EnsureExclusive();
      this.loan.Unwrap().LoanData.RemoveServicingTransaction(trans.Unwrap());
    }

    public IEnumerator GetEnumerator() => this.createTransactionCollection().GetEnumerator();

    private ArrayList createTransactionCollection()
    {
      ArrayList transactionCollection = new ArrayList();
      ServicingTransactionBase[] servicingTransactions = this.loan.Unwrap().LoanData.GetServicingTransactions(true);
      if (servicingTransactions != null)
      {
        foreach (ServicingTransactionBase servicingTransactionBase in servicingTransactions)
        {
          ServicingTransaction servicingTransaction = ServicingTransaction.Wrap(this.loan, servicingTransactionBase);
          if (servicingTransaction != null)
            transactionCollection.Add((object) servicingTransaction);
        }
      }
      return transactionCollection;
    }
  }
}
