// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Servicing.EscrowDisbursement
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine.InterimServicing;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Servicing
{
  public class EscrowDisbursement : ServicingTransaction, IEscrowDisbursement
  {
    internal EscrowDisbursement(Loan loan, EscrowDisbursementLog trans)
      : base(loan, (ServicingTransactionBase) trans)
    {
    }

    public int DisbursementNumber => this.baseTrans.DisbursementNo;

    public DateTime DisbursementDueDate
    {
      get => this.baseTrans.DisbursementDueDate;
      set
      {
        this.baseTrans.DisbursementDueDate = value;
        this.setLastModified();
      }
    }

    public ServicingDisbursementType DisbursementType
    {
      get => (ServicingDisbursementType) this.baseTrans.DisbursementType;
      set
      {
        this.baseTrans.DisbursementType = (ServicingDisbursementTypes) value;
        this.setLastModified();
      }
    }

    public string InstitutionName
    {
      get => this.baseTrans.InstitutionName;
      set
      {
        this.baseTrans.InstitutionName = value;
        this.setLastModified();
      }
    }

    public string Comments
    {
      get => this.baseTrans.Comments;
      set
      {
        this.baseTrans.Comments = value;
        this.setLastModified();
      }
    }

    public bool IsReversed() => this.GetReversal() != null;

    public EscrowDisbursementReversal GetReversal()
    {
      foreach (EscrowDisbursementReversal transaction in (CollectionBase) this.Loan.Servicing.Transactions.GetTransactions(ServicingTransactionType.EscrowDisbursementReversal))
      {
        if (((PaymentReversalLog) transaction.Unwrap()).PaymentGUID == this.ID)
          return transaction;
      }
      return (EscrowDisbursementReversal) null;
    }

    public EscrowDisbursementReversal Reverse(DateTime reversalDate)
    {
      if (this.IsReversed())
        throw new Exception("A reversal already exists for the current disbursement");
      PaymentReversalLog servicingTransaction = (PaymentReversalLog) this.Loan.Unwrap().CreateNextServicingTransaction((ServicingTransactionTypes) 2);
      ((ServicingTransactionBase) servicingTransaction).TransactionDate = reversalDate;
      servicingTransaction.PaymentGUID = this.ID;
      ((ServicingTransactionBase) servicingTransaction).TransactionAmount = Convert.ToDouble(-1M * this.TransactionAmount);
      servicingTransaction.ReversalType = (ServicingTransactionTypes) 5;
      this.Loan.Unwrap().LoanData.AddServicingTransaction((ServicingTransactionBase) servicingTransaction);
      return new EscrowDisbursementReversal(this.Loan, servicingTransaction);
    }

    private EscrowDisbursementLog baseTrans => (EscrowDisbursementLog) this.Unwrap();
  }
}
