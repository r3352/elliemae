// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.LoanLiabilities
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>
  /// Represents the set of liabilities associated with a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Loan">Loan</see>.
  /// </summary>
  /// <remarks>The items within this set are indexed starting with the value
  /// 1. An attempt to access an item in this collection with a value less than
  /// 1 will result in an InvalidArgumentException.
  /// </remarks>
  /// <example>
  ///       The following code demonstrates how to add a new liability to an existing
  ///       loan and then set its field values.
  ///       <code>
  ///         <![CDATA[
  /// using System;
  /// using System.IO;
  /// using EllieMae.Encompass.Client;
  /// using EllieMae.Encompass.BusinessObjects;
  /// using EllieMae.Encompass.BusinessObjects.Loans;
  /// 
  /// class LoanManager
  /// {
  ///    public static void Main(string[] args)
  ///    {
  ///          // Open the session to the remote server
  ///          Session session = new Session();
  ///          session.Start("myserver", "mary", "maryspwd");
  /// 
  ///          // Open an existing loan using the GUID from the command line
  ///          Loan loan = session.Loans.Open(args[0]);
  /// 
  ///          // Lock the loan so we can modify it safely
  ///          loan.Lock();
  /// 
  ///          // Add a new liability and save off it index in the liabilities list
  ///          int newIndex = loan.Liabilities.Add();
  /// 
  ///          // Set the value of some of theliability fields
  ///          loan.Fields.GetFieldAt("FL02", newIndex).Value = "Bank of Havasu";  // Liability Holder
  ///          loan.Fields.GetFieldAt("FL10", newIndex).Value = "2220001-003";     // Account #
  ///          loan.Fields.GetFieldAt("FL20", newIndex).Value = "(555) 555-0233";  // Holder Phone
  /// 
  ///          // Commit the changes to the server
  ///          loan.Commit();
  /// 
  ///          // Release the lock on the loan
  ///          loan.Unlock();
  /// 
  ///          // End the session to gracefully disconnect from the server
  ///          session.End();
  ///    }
  /// }
  ///         ]]>
  ///       </code>
  ///     </example>
  public class LoanLiabilities : ILoanLiabilities
  {
    private Loan loan;

    internal LoanLiabilities(Loan loan) => this.loan = loan;

    /// <summary>Gets the number of liabilities in the set.</summary>
    /// <example>
    /// The following code removes all liability verification records from a loan file.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// 
    /// class LoanManager
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Open an existing loan using the GUID from the command line
    ///       Loan loan = session.Loans.Open(args[0]);
    /// 
    ///       // Lock the loan to edit it
    ///       loan.Lock();
    /// 
    ///       // Loop over all of the currently defined liabilities attached to the loan.
    ///       // The liabilities are indexed starting at 1, so the loop must adjust
    ///       // accordingly.
    ///       for (int i = loan.Liabilities.Count; i >= 1; i--)
    ///       {
    ///          // Write out the name of the account holder
    ///          Console.WriteLine("Liability Holder: " + loan.Fields.GetFieldAt("FL02", i).Value);
    /// 
    ///          // Remove the deposit
    ///          loan.Liabilities.RemoveAt(i);
    ///       }
    /// 
    ///       // Commit the changes to the server and unlock the loan
    ///       loan.Commit();
    ///       loan.Unlock();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public int Count => this.loan.LoanData.GetNumberOfLiabilitesExlcudingAlimonyJobExp();

    /// <summary>Adds a new liability to the set for the current loan.</summary>
    /// <param name="requireExclusive">Passing the value <c>true</c> causes a check to confirm there is
    /// an exclusive lock on the loan. Passing the value <c>false</c> will bypass the exclusive check.
    /// If running in SDK Concurrent editing mode the value <c>false</c> should be used.</param>
    /// <returns>The return value is the index of the new liability, which
    /// can be later used to access the loan fields associated with this
    /// liability.</returns>
    public int Add(bool requireExclusive)
    {
      if (requireExclusive)
        this.loan.EnsureExclusive();
      return this.loan.LoanData.NewLiability() + 1;
    }

    /// <summary>Adds a new liability to the set for the current loan.</summary>
    /// <returns>The return value is the index of the new liability, which
    /// can be later used to access the loan fields associated with this
    /// liability.</returns>
    /// <example>
    /// The following code demonstrates how to add a new liability to an existing
    /// loan and then set its field values.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// 
    /// class LoanManager
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Open an existing loan using the GUID from the command line
    ///       Loan loan = session.Loans.Open(args[0]);
    /// 
    ///       // Lock the loan so we can modify it safely
    ///       loan.Lock();
    /// 
    ///       // Add a new liability and save off it index in the liabilities list
    ///       int newIndex = loan.Liabilities.Add();
    /// 
    ///       // Set the value of some of theliability fields
    ///       loan.Fields.GetFieldAt("FL02", newIndex).Value = "Bank of Havasu";  // Liability Holder
    ///       loan.Fields.GetFieldAt("FL10", newIndex).Value = "2220001-003";     // Account #
    ///       loan.Fields.GetFieldAt("FL20", newIndex).Value = "(555) 555-0233";  // Holder Phone
    /// 
    ///       // Commit the changes to the server
    ///       loan.Commit();
    /// 
    ///       // Release the lock on the loan
    ///       loan.Unlock();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public int Add() => this.Add(true);

    /// <summary>Removes a single liability from the set.</summary>
    /// <param name="index">The 1-based index of the liability to remove.</param>
    /// <param name="requireExclusive">Passing the value <c>true</c> causes a check to confirm there is
    /// an exclusive lock on the loan. Passing the value <c>false</c> will bypass the exclusive check.
    /// If running in SDK Concurrent editing mode the value <c>false</c> should be used.</param>
    public void RemoveAt(int index, bool requireExclusive)
    {
      if (index < 1)
        throw new ArgumentOutOfRangeException(nameof (index), (object) index, "The index must be greater than or equal to 1.");
      if (requireExclusive)
        this.loan.EnsureExclusive();
      this.loan.LoanData.RemoveLiabilityAt(index - 1);
    }

    /// <summary>Removes a single liability from the set.</summary>
    /// <param name="index">The 1-based index of the liability to remove.</param>
    /// <example>
    /// The following code removes all liability verification records from a loan file.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// 
    /// class LoanManager
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Open an existing loan using the GUID from the command line
    ///       Loan loan = session.Loans.Open(args[0]);
    /// 
    ///       // Lock the loan to edit it
    ///       loan.Lock();
    /// 
    ///       // Loop over all of the currently defined liabilities attached to the loan.
    ///       // The liabilities are indexed starting at 1, so the loop must adjust
    ///       // accordingly.
    ///       for (int i = loan.Liabilities.Count; i >= 1; i--)
    ///       {
    ///          // Write out the name of the account holder
    ///          Console.WriteLine("Liability Holder: " + loan.Fields.GetFieldAt("FL02", i).Value);
    /// 
    ///          // Remove the deposit
    ///          loan.Liabilities.RemoveAt(i);
    ///       }
    /// 
    ///       // Commit the changes to the server and unlock the loan
    ///       loan.Commit();
    ///       loan.Unlock();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public void RemoveAt(int index) => this.RemoveAt(index, true);

    /// <summary>
    /// Returns the index of a linked mortgage, if any, for the specified liability.
    /// </summary>
    /// <param name="liabilityIndex">The 1-based index of the liability.</param>
    /// <returns>The index of the linked mortgage, or -1 if no mortgage is linked.</returns>
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
