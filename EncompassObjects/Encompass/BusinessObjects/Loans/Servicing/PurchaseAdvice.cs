// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Servicing.PurchaseAdvice
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.DataEngine.InterimServicing;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Servicing
{
  internal class PurchaseAdvice : ServicingTransaction, IPurchaseAdvice
  {
    internal PurchaseAdvice(Loan loan, LoanPurchaseLog trans)
      : base(loan, (ServicingTransactionBase) trans)
    {
    }

    /// <summary>
    /// Gets the date on which this payment was originally scheduled to be due.
    /// </summary>
    /// <remarks>This date will differ from the <see cref="!:PaymentDueDate" /> only if the
    /// PaymetDueDate has been override by the user. In that case, this property will indicate
    /// the original due date of the scheduled payment.</remarks>
    public DateTime PurchaseAdviceDate => this.baseTrans.PurchaseAdviceDate;

    public string Investor => this.baseTrans.Investor;

    public string InvestorLoanNumber => this.baseTrans.InvestorLoanNumber;

    public DateTime FirstPaymenttoInvestor => this.baseTrans.FirstPaymenttoInvestor;

    public double PurchaseAdviceAmount => this.baseTrans.PurchaseAmount;

    private LoanPurchaseLog baseTrans => (LoanPurchaseLog) this.Unwrap();
  }
}
