// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Servicing.ServicingTransaction
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.DataEngine.InterimServicing;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Servicing
{
  /// <summary>
  /// Represents a generic servicing transaction for a loan.
  /// </summary>
  public abstract class ServicingTransaction : IServicingTransaction
  {
    private Loan loan;
    private ServicingTransactionBase trans;

    internal ServicingTransaction(Loan loan, ServicingTransactionBase trans)
    {
      this.loan = loan;
      this.trans = trans;
    }

    /// <summary>Gets the unique GUID for the transaction.</summary>
    public string ID => this.trans.TransactionGUID;

    /// <summary>
    /// Gets the type of transaction represented by the object.
    /// </summary>
    public virtual ServicingTransactionType TransactionType
    {
      get => (ServicingTransactionType) this.trans.TransactionType;
    }

    /// <summary>
    /// Gets or sets the date on which the transaction occurred.
    /// </summary>
    public virtual DateTime TransactionDate
    {
      get => this.trans.TransactionDate;
      set
      {
        this.trans.TransactionDate = value;
        this.setLastModified();
      }
    }

    /// <summary>Gets or sets the dollar amount of the transaction.</summary>
    public virtual Decimal TransactionAmount
    {
      get => Convert.ToDecimal(this.trans.TransactionAmount);
      set
      {
        this.trans.TransactionAmount = Convert.ToDouble(value);
        this.setLastModified();
      }
    }

    /// <summary>
    /// Gets or sets the payment method used for the transaction.
    /// </summary>
    public ServicingPaymentMethod PaymentMethod
    {
      get => (ServicingPaymentMethod) this.trans.PaymentMethod;
      set
      {
        this.trans.PaymentMethod = (ServicingPaymentMethods) value;
        this.setLastModified();
      }
    }

    /// <summary>
    /// Gets the Login ID of the user who created this transaction.
    /// </summary>
    public string CreatedBy => this.trans.CreatedByID;

    /// <summary>
    /// Gets the date and time when the transaction was created.
    /// </summary>
    public DateTime CreationDate => this.trans.CreatedDateTime;

    /// <summary>
    /// Gets the Login ID of the user who last modified this transaction.
    /// </summary>
    public string LastModifiedBy => this.trans.ModifiedByID;

    /// <summary>
    /// Gets the date and time of the last modification to this transaction.
    /// </summary>
    public DateTime LastModifiedDate => this.trans.ModifiedDateTime;

    /// <summary>Verifies equality between two transaction</summary>
    /// <param name="obj">The transaction to compare to.</param>
    /// <returns>Return true if the two objects represent the same transaction, false otherwise.</returns>
    public override bool Equals(object obj)
    {
      ServicingTransaction servicingTransaction = obj as ServicingTransaction;
      return obj != null && this.ID == servicingTransaction.ID;
    }

    /// <summary>
    /// Provides a hash code implementation for a servicing transaction.
    /// </summary>
    /// <returns>Returns a hash code for the object.</returns>
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
      switch (obj.TransactionType)
      {
        case ServicingTransactionTypes.Payment:
          return (ServicingTransaction) new Payment(loan, (PaymentTransactionLog) obj);
        case ServicingTransactionTypes.PaymentReversal:
          PaymentReversalLog trans = (PaymentReversalLog) obj;
          if (trans.ReversalType == ServicingTransactionTypes.PaymentReversal)
            return (ServicingTransaction) new PaymentReversal(loan, trans);
          return trans.ReversalType == ServicingTransactionTypes.EscrowDisbursementReversal ? (ServicingTransaction) new EscrowDisbursementReversal(loan, trans) : (ServicingTransaction) null;
        case ServicingTransactionTypes.EscrowDisbursement:
          return (ServicingTransaction) new EscrowDisbursement(loan, (EscrowDisbursementLog) obj);
        case ServicingTransactionTypes.EscrowInterest:
          return (ServicingTransaction) new EscrowInterest(loan, (EscrowInterestLog) obj);
        case ServicingTransactionTypes.EscrowDisbursementReversal:
          return (ServicingTransaction) new EscrowDisbursementReversal(loan, (PaymentReversalLog) obj);
        case ServicingTransactionTypes.Other:
          return (ServicingTransaction) new OtherTransaction(loan, (OtherTransactionLog) obj);
        case ServicingTransactionTypes.PurchaseAdvice:
          return (ServicingTransaction) new PurchaseAdvice(loan, (LoanPurchaseLog) obj);
        default:
          return (ServicingTransaction) null;
      }
    }
  }
}
