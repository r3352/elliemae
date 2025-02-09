// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Servicing.ServicingTransaction
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine.InterimServicing;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Servicing
{
  public abstract class ServicingTransaction : IServicingTransaction
  {
    private Loan loan;
    private ServicingTransactionBase trans;

    internal ServicingTransaction(Loan loan, ServicingTransactionBase trans)
    {
      this.loan = loan;
      this.trans = trans;
    }

    public string ID => this.trans.TransactionGUID;

    public virtual ServicingTransactionType TransactionType
    {
      get => (ServicingTransactionType) this.trans.TransactionType;
    }

    public virtual DateTime TransactionDate
    {
      get => this.trans.TransactionDate;
      set
      {
        this.trans.TransactionDate = value;
        this.setLastModified();
      }
    }

    public virtual Decimal TransactionAmount
    {
      get => Convert.ToDecimal(this.trans.TransactionAmount);
      set
      {
        this.trans.TransactionAmount = Convert.ToDouble(value);
        this.setLastModified();
      }
    }

    public ServicingPaymentMethod PaymentMethod
    {
      get => (ServicingPaymentMethod) this.trans.PaymentMethod;
      set
      {
        this.trans.PaymentMethod = (ServicingPaymentMethods) value;
        this.setLastModified();
      }
    }

    public string CreatedBy => this.trans.CreatedByID;

    public DateTime CreationDate => this.trans.CreatedDateTime;

    public string LastModifiedBy => this.trans.ModifiedByID;

    public DateTime LastModifiedDate => this.trans.ModifiedDateTime;

    public override bool Equals(object obj)
    {
      ServicingTransaction servicingTransaction = obj as ServicingTransaction;
      return obj != null && this.ID == servicingTransaction.ID;
    }

    public override int GetHashCode() => this.ID.GetHashCode();

    internal ServicingTransactionBase Unwrap() => this.trans;

    internal Loan Loan => this.loan;

    internal void setLastModified()
    {
      this.trans.ModifiedByID = this.loan.Session.SessionObjects.UserInfo.Userid;
      this.trans.ModifiedByName = this.loan.Session.SessionObjects.UserInfo.FullName;
      this.trans.ModifiedDateTime = DateTime.Now;
      this.loan.Unwrap().LoanData.AddServicingTransaction(this.trans);
    }

    internal static ServicingTransaction Wrap(Loan loan, ServicingTransactionBase obj)
    {
      switch (obj.TransactionType - 1)
      {
        case 0:
          return (ServicingTransaction) new Payment(loan, (PaymentTransactionLog) obj);
        case 1:
          PaymentReversalLog trans = (PaymentReversalLog) obj;
          if (trans.ReversalType == 2)
            return (ServicingTransaction) new PaymentReversal(loan, trans);
          return trans.ReversalType == 5 ? (ServicingTransaction) new EscrowDisbursementReversal(loan, trans) : (ServicingTransaction) null;
        case 2:
          return (ServicingTransaction) new EscrowDisbursement(loan, (EscrowDisbursementLog) obj);
        case 3:
          return (ServicingTransaction) new EscrowInterest(loan, (EscrowInterestLog) obj);
        case 4:
          return (ServicingTransaction) new EscrowDisbursementReversal(loan, (PaymentReversalLog) obj);
        case 5:
          return (ServicingTransaction) new OtherTransaction(loan, (OtherTransactionLog) obj);
        case 7:
          return (ServicingTransaction) new PurchaseAdvice(loan, (LoanPurchaseLog) obj);
        default:
          return (ServicingTransaction) null;
      }
    }
  }
}
