// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.LoanEmployers
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  public class LoanEmployers : ILoanEmployers
  {
    private Loan loan;
    private bool borrower;

    internal LoanEmployers(Loan loan, bool borrower)
    {
      this.loan = loan;
      this.borrower = borrower;
    }

    public int Count => this.loan.LoanData.GetNumberOfEmployer(this.borrower);

    public int Add(bool current) => this.loan.LoanData.NewEmployer(this.borrower, current) + 1;

    public void RemoveAt(int index)
    {
      if (index < 1)
        throw new ArgumentOutOfRangeException(nameof (index), (object) index, "The index must be greater than or equal to 1.");
      this.loan.LoanData.RemoveEmployerAt(this.borrower, index - 1);
    }
  }
}
