// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.LoanLiabilities
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  public class LoanLiabilities : ILoanLiabilities
  {
    private Loan loan;

    internal LoanLiabilities(Loan loan) => this.loan = loan;

    public int Count => this.loan.LoanData.GetNumberOfLiabilitesExlcudingAlimonyJobExp();

    public int Add(bool requireExclusive)
    {
      if (requireExclusive)
        this.loan.EnsureExclusive();
      return this.loan.LoanData.NewLiability() + 1;
    }

    public int Add() => this.Add(true);

    public void RemoveAt(int index, bool requireExclusive)
    {
      if (index < 1)
        throw new ArgumentOutOfRangeException(nameof (index), (object) index, "The index must be greater than or equal to 1.");
      if (requireExclusive)
        this.loan.EnsureExclusive();
      this.loan.LoanData.RemoveLiabilityAt(index - 1);
    }

    public void RemoveAt(int index) => this.RemoveAt(index, true);

    public int GetMortgage(int liabilityIndex)
    {
      if (liabilityIndex < 1)
        throw new ArgumentException("Invalid index specified. Index must be >= 1.");
      string unformattedValue = this.loan.Fields.GetFieldAt("FL25", liabilityIndex).UnformattedValue;
      if (unformattedValue == "")
        return -1;
      for (int itemIndex = 1; itemIndex <= this.loan.Mortgages.Count; ++itemIndex)
      {
        if (this.loan.Fields.GetFieldAt("FM43", itemIndex).UnformattedValue == unformattedValue)
          return itemIndex;
      }
      return -1;
    }
  }
}
