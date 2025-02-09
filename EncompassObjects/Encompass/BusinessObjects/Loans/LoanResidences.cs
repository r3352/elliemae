// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.LoanResidences
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>
  /// Provides access to a set of residences associated with a loan.
  /// </summary>
  /// <remarks>The items within this set are indexed starting with the value
  /// 1. An attempt to access an item in this collection with a value less than
  /// 1 will result in an InvalidArgumentException.
  /// </remarks>
  /// <example>
  ///       The following code demonstrates how to print the addresses of all of the
  ///       prior residences of both the primary borrower and the coborrower.
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
  ///       // Open the session to the remote server
  ///       Session session = new Session();
  ///       session.Start("myserver", "mary", "maryspwd");
  /// 
  ///       // Open an existing loan using the GUID from the command line
  ///       Loan loan = session.Loans.Open(args[0]);
  /// 
  ///       // Loop over the set of residences, printing the addresses
  ///       for (int i = 1; i <= loan.BorrowerResidences.Count; i++)
  ///       {
  ///          Console.WriteLine("Borrower Residence " + i + ":");
  ///          Console.WriteLine(loan.Fields.GetFieldAt("BR04", i));   // Street Addr
  ///          Console.WriteLine(loan.Fields.GetFieldAt("BR06", i));   // City
  ///          Console.WriteLine(loan.Fields.GetFieldAt("BR07", i));   // State
  ///          Console.WriteLine(loan.Fields.GetFieldAt("BR08", i));   // Zip
  ///       }
  /// 
  ///       // Now the CoBorrower residences
  ///       for (int i = 1; i <= loan.CoBorrowerResidences.Count; i++)
  ///       {
  ///          Console.WriteLine("CoBorrower Residence " + i + ":");
  ///          Console.WriteLine(loan.Fields.GetFieldAt("CR04", i));   // Street Addr
  ///          Console.WriteLine(loan.Fields.GetFieldAt("CR06", i));   // City
  ///          Console.WriteLine(loan.Fields.GetFieldAt("CR07", i));   // State
  ///          Console.WriteLine(loan.Fields.GetFieldAt("CR08", i));   // Zip
  ///       }
  /// 
  ///       // End the session to gracefully disconnect from the server
  ///       session.End();
  ///    }
  /// }
  /// ]]>
  ///       </code>
  ///     </example>
  public class LoanResidences : ILoanResidences
  {
    private Loan loan;
    private bool borrower;

    internal LoanResidences(Loan loan, bool borrower)
    {
      this.loan = loan;
      this.borrower = borrower;
    }

    /// <summary>Gets the number of residences within the set.</summary>
    /// <example>
    /// The following code removes all residence verification records for the
    /// primary borrower from a loan file.
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
    ///       // Create the empty shell for the new loan.
    ///       Loan loan = session.Loans.Open(args[0]);
    /// 
    ///       // Lock the loan to edit it
    ///       loan.Lock();
    /// 
    ///       // Loop over all of the currently defined employer records for
    ///       // the primary borrower. The records are indexed starting at 1,
    ///       // so the loop be set adjust accordingly.
    ///       for (int i = loan.BorrowerResidences.Count; i >= 1; i--)
    ///       {
    ///          // Write out the name of the account holder
    ///          Console.WriteLine("Address: " + loan.Fields.GetFieldAt("BR04", i).Value);
    /// 
    ///          // Remove the deposit
    ///          loan.BorrowerResidences.RemoveAt(i);
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
    public int Count => this.loan.LoanData.GetNumberOfResidence(this.borrower);

    /// <summary>Adds a new residence to the set.</summary>
    /// <param name="current">A flag indicating if the residence represents
    /// the borrower's/coborrower's current residence.</param>
    /// <returns>The index of the newly created residence.</returns>
    /// <example>
    /// The following code adds a residence verification record for the primary borrower
    /// associated with the current loan.
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
    ///       // Add a new residence verification record for the coborrower
    ///       int index = loan.BorrowerResidences.Add(false);
    /// 
    ///       // Populate the address fields for the residence
    ///       loan.Fields.GetFieldAt("BR04", index).Value = "3099 Glen Canyon Ct.";
    ///       loan.Fields.GetFieldAt("BR06", index).Value = "Farmington Square";
    ///       loan.Fields.GetFieldAt("BR07", index).Value = "IN";
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
    public int Add(bool current) => this.loan.LoanData.NewResidence(this.borrower, current) + 1;

    /// <summary>Removes an existing residence from the set.</summary>
    /// <param name="index">The 1-based index of the residence to remove.</param>
    /// <example>
    /// The following code removes all residence verification records for the
    /// primary borrower from a loan file.
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
    ///       // Create the empty shell for the new loan.
    ///       Loan loan = session.Loans.Open(args[0]);
    /// 
    ///       // Lock the loan to edit it
    ///       loan.Lock();
    /// 
    ///       // Loop over all of the currently defined employer records for
    ///       // the primary borrower. The records are indexed starting at 1,
    ///       // so the loop be set adjust accordingly.
    ///       for (int i = loan.BorrowerResidences.Count; i >= 1; i--)
    ///       {
    ///          // Write out the name of the account holder
    ///          Console.WriteLine("Address: " + loan.Fields.GetFieldAt("BR04", i).Value);
    /// 
    ///          // Remove the deposit
    ///          loan.BorrowerResidences.RemoveAt(i);
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
    public void RemoveAt(int index)
    {
      if (index < 1)
        throw new ArgumentOutOfRangeException(nameof (index), (object) index, "The index must be greater than or equal to 1.");
      this.loan.LoanData.RemoveResidenceAt(this.borrower, index - 1);
    }
  }
}
