// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.NonBorrowingOwnerContacts
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>Non Borrowing Owner Contacts</summary>
  public class NonBorrowingOwnerContacts : INonBorrowingOwnerContacts
  {
    private Loan loan;

    internal NonBorrowingOwnerContacts(Loan loan) => this.loan = loan;

    /// <summary>
    /// Gets the number of non-borrowing owner parties defined in File Contact for the current loan.
    /// </summary>
    public int Count => this.loan.LoanData.GetNumberOfNonBorrowingOwnerContact();

    /// <summary>
    /// Adds a new Non-Borrowing Owner Contact to File Contact List.
    /// </summary>
    /// <returns>The function returns the index of the newly created Non-Borrowing Owner Contact.
    /// This value should be used to access Non-Borrowing Owner Contact-related loan fields
    /// using the GetFieldAt() method of the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.LoanFields">LoanFields</see>
    /// object.
    /// </returns>
    public int Add() => this.loan.LoanData.NewNonBorrowingOwnerContact() + 1;

    /// <summary>
    /// Removes a Non-Borrowing Owner Contact from File Contact List in the current loan. The vesting party linked to this Non-Borrowing Owner Contact will be removed together.
    /// </summary>
    /// <param name="index">The 1-based index of the Non-Borrowing Owner Contact to remove.</param>
    public void RemoveAt(int index)
    {
      if (index < 1)
        throw new ArgumentOutOfRangeException(nameof (index), (object) index, "The index must be greater than or equal to 1.");
      this.loan.LoanData.RemoveNonBorrowingOwnerContactAt(index - 1);
    }

    /// <summary>
    /// Get the Vesting record index that is lined to Non-Borrowing Owner Contact by passing the Vesting GUID read from NBOCxx99.
    /// </summary>
    /// <param name="vestingGUID">The Vesting GUID stored in Non-Borrowing Owner Contact. The 1-based index will be returned.</param>
    public int GetVestingLinkedNBO(string vestingGUID)
    {
      return this.loan.LoanData.GetVestingLinkedNBO(vestingGUID);
    }

    /// <summary>
    /// Get the Non-Borrowing Owner Contact record index that is lined to Vesting record by passing the NBO GUID read from TRxx99.
    /// </summary>
    /// <param name="nboGUID">The Vesting GUID stored in Non-Borrowing Owner Contact. The 1-based index will be returned.</param>
    public int GetNBOLinkedVesting(string nboGUID)
    {
      return this.loan.LoanData.GetNBOLinkedVesting(nboGUID);
    }
  }
}
