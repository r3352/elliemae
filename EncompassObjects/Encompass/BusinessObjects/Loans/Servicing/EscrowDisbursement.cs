// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Servicing.EscrowDisbursement
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.DataEngine.InterimServicing;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Servicing
{
  /// <summary>Represents an disbursement from the escrow account.</summary>
  /// <remarks>A single EscrowDisbursement object is used to indicate a payment of
  /// the borrower's insurance, taxes, etc. from the escrow account.</remarks>
  public class EscrowDisbursement : ServicingTransaction, IEscrowDisbursement
  {
    internal EscrowDisbursement(Loan loan, EscrowDisbursementLog trans)
      : base(loan, (ServicingTransactionBase) trans)
    {
    }

    /// <summary>Gets the index of this disbursement.</summary>
    public int DisbursementNumber => this.baseTrans.DisbursementNo;

    /// <summary>
    /// Gets or sets the date on which the disbursement is due.
    /// </summary>
    public DateTime DisbursementDueDate
    {
      get => this.baseTrans.DisbursementDueDate;
      set
      {
        this.baseTrans.DisbursementDueDate = value;
        this.setLastModified();
      }
    }

    /// <summary>Gets or sets the type of disbursement being made.</summary>
    public ServicingDisbursementType DisbursementType
    {
      get => (ServicingDisbursementType) this.baseTrans.DisbursementType;
      set
      {
        this.baseTrans.DisbursementType = (ServicingDisbursementTypes) value;
        this.setLastModified();
      }
    }

    /// <summary>
    /// Gets or sets the name of the institution to which the disbursement is being made.
    /// </summary>
    public string InstitutionName
    {
      get => this.baseTrans.InstitutionName;
      set
      {
        this.baseTrans.InstitutionName = value;
        this.setLastModified();
      }
    }

    /// <summary>Gets or sets additional comments for the transaction.</summary>
    public string Comments
    {
      get => this.baseTrans.Comments;
      set
      {
        this.baseTrans.Comments = value;
        this.setLastModified();
      }
    }

    /// <summary>
    /// Determines if the current disbursement has been reversed.
    /// </summary>
    /// <returns>Returns <c>true</c> if the disbursement has been reversed, <c>false</c> otherwise.</returns>
    /// <remarks>You can use the <see cref="M:EllieMae.Encompass.BusinessObjects.Loans.Servicing.EscrowDisbursement.GetReversal" /> method to retrieve the
    /// <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Servicing.EscrowDisbursementReversal" /> object if this property returns <c>true</c>.</remarks>
    public bool IsReversed() => this.GetReversal() != null;

    /// <summary>
    /// Returns the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Servicing.EscrowDisbursementReversal" /> object for a disbursement which has been reversed.
    /// </summary>
    /// <returns>Returns the reversal object if the disbursement has been reversed or <c>null</c> if the
    /// disbursement has not been reversed.</returns>
    public EscrowDisbursementReversal GetReversal()
    {
      foreach (EscrowDisbursementReversal transaction in (CollectionBase) this.Loan.Servicing.Transactions.GetTransactions(ServicingTransactionType.EscrowDisbursementReversal))
      {
        if (((PaymentReversalLog) transaction.Unwrap()).PaymentGUID == this.ID)
          return transaction;
      }
      return (EscrowDisbursementReversal) null;
    }

    /// <summary>
    /// Reverses the disbursement and restores the balance of the escrow account.
    /// </summary>
    /// <param name="reversalDate">The date on which the reversal took place.</param>
    /// <returns>Returns an <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Servicing.EscrowDisbursementReversal" /> object representing
    /// the current reversal.</returns>
    public EscrowDisbursementReversal Reverse(DateTime reversalDate)
    {
      if (this.IsReversed())
        throw new Exception("A reversal already exists for the current disbursement");
      PaymentReversalLog servicingTransaction = (PaymentReversalLog) this.Loan.Unwrap().CreateNextServicingTransaction(ServicingTransactionTypes.PaymentReversal);
      servicingTransaction.TransactionDate = reversalDate;
      servicingTransaction.PaymentGUID = this.ID;
      servicingTransaction.TransactionAmount = Convert.ToDouble(-1M * this.TransactionAmount);
      servicingTransaction.ReversalType = ServicingTransactionTypes.EscrowDisbursementReversal;
      this.Loan.Unwrap().LoanData.AddServicingTransaction((ServicingTransactionBase) servicingTransaction);
      return new EscrowDisbursementReversal(this.Loan, servicingTransaction);
    }

    private EscrowDisbursementLog baseTrans => (EscrowDisbursementLog) this.Unwrap();
  }
}
