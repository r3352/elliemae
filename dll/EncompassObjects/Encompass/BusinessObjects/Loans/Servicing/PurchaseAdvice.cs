// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Servicing.PurchaseAdvice
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

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

    public DateTime PurchaseAdviceDate => this.baseTrans.PurchaseAdviceDate;

    public string Investor => this.baseTrans.Investor;

    public string InvestorLoanNumber => this.baseTrans.InvestorLoanNumber;

    public DateTime FirstPaymenttoInvestor => this.baseTrans.FirstPaymenttoInvestor;

    public double PurchaseAdviceAmount => this.baseTrans.PurchaseAmount;

    private LoanPurchaseLog baseTrans => (LoanPurchaseLog) this.Unwrap();
  }
}
