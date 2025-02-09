// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.LoanMortgages
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.Collections;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  public class LoanMortgages : ILoanMortgages
  {
    private Loan loan;

    internal LoanMortgages(Loan loan) => this.loan = loan;

    public int Count => this.loan.LoanData.GetNumberOfMortgages();

    public int Add(IntegerList liabilities)
    {
      return this.loan.LoanData.NewMortgage(this.formatIntegerList(liabilities)) + 1;
    }

    public void RemoveAt(int index)
    {
      if (index < 1)
        throw new ArgumentOutOfRangeException(nameof (index), (object) index, "The index must be greater than or equal to 1.");
      this.loan.LoanData.RemoveMortgageAt(index - 1);
    }

    public void AttachMortgage(int index, IntegerList liabilities)
    {
      if (index < 1)
        throw new ArgumentOutOfRangeException(nameof (index), (object) index, "The index must be greater than or equal to 1.");
      this.loan.LoanData.AttachMortgage(index.ToString("00"), this.formatIntegerList(liabilities));
    }

    public IntegerList GetLiabilities(int index)
    {
      if (index < 1)
        throw new ArgumentException("Invalid index specified. Index must be >= 1.");
      IntegerList liabilities = new IntegerList();
      string unformattedValue = this.loan.Fields.GetFieldAt("FM43", index).UnformattedValue;
      for (int itemIndex = 1; itemIndex <= this.loan.Liabilities.Count; ++itemIndex)
      {
        if (this.loan.Fields.GetFieldAt("FL25", itemIndex).UnformattedValue == unformattedValue)
          liabilities.Add(itemIndex);
      }
      return liabilities;
    }

    private string formatIntegerList(IntegerList values)
    {
      string str = "";
      for (int index = 0; index < values.Count; ++index)
        str = str + (index > 0 ? "|" : "") + values[index].ToString();
      return str;
    }
  }
}
