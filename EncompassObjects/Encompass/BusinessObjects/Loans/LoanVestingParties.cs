// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.LoanVestingParties
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>
  /// Provides access to the set of non-borrower Vesting Parties associated with a loan.
  /// </summary>
  /// <remarks>The items within this set are indexed starting with the value
  /// 1. An attempt to access an item in this collection with a value less than
  /// 1 will result in an InvalidArgumentException.
  /// </remarks>
  public class LoanVestingParties : ILoanVestingParties
  {
    private Loan loan;

    internal LoanVestingParties(Loan loan) => this.loan = loan;

    /// <summary>
    /// Gets the number of non-borrower vesting parties defined for the current loan.
    /// </summary>
    /// <remarks>This count does not includes borrowers and co-borrowers entered on the 1003.</remarks>
    public int Count => this.loan.LoanData.GetNumberOfAdditionalVestingParties();

    /// <summary>Adds a new vesting party to the loan.</summary>
    /// <returns>The function returns the index of the newly created trustee.
    /// This value should be used to access trustee-related loan fields
    /// using the GetFieldAt() method of the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.LoanFields">LoanFields</see>
    /// object.
    /// </returns>
    public int Add() => this.loan.LoanData.NewAdditionalVestingParty() + 1;

    /// <summary>Removes a vesting party from the current loan.</summary>
    /// <param name="index">The 1-based index of the trustee to remove.</param>
    public void RemoveAt(int index)
    {
      if (index < 1)
        throw new ArgumentOutOfRangeException(nameof (index), (object) index, "The index must be greater than or equal to 1.");
      this.loan.LoanData.RemoveAdditionalVestingPartyAt(index - 1);
    }
  }
}
