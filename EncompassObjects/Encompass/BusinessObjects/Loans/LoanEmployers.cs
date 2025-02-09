// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.LoanEmployers
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>
  /// Provides access to a set of Employers associated with a loan.
  /// </summary>
  /// <remarks>The items within this set are indexed starting with the value
  /// 1. An attempt to access an item in this collection with a value less than
  /// 1 will result in an InvalidArgumentException.
  /// </remarks>
  /// <example>
  ///       The following code demonstrates how to print the names and locations of all of the
  ///       prior employers for both the primary borrower and the coborrower.
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
  ///       for (int i = 1; i <= loan.BorrowerEmployers.Count; i++)
  ///       {
  ///          Console.WriteLine("Borrower Employer " + i + ":");
  ///          Console.WriteLine(loan.Fields.GetFieldAt("BE02", i));   // Employer Name
  ///          Console.WriteLine(loan.Fields.GetFieldAt("BE05", i));   // City
  ///          Console.WriteLine(loan.Fields.GetFieldAt("BE06", i));   // State
  ///       }
  /// 
  ///       // Now the CoBorrower residences
  ///       for (int i = 1; i <= loan.CoBorrowerEmployers.Count; i++)
  ///       {
  ///          Console.WriteLine("CoBorrower Employer " + i + ":");
  ///          Console.WriteLine(loan.Fields.GetFieldAt("CE02", i));   // Employer Name
  ///          Console.WriteLine(loan.Fields.GetFieldAt("CE05", i));   // City
  ///          Console.WriteLine(loan.Fields.GetFieldAt("CE06", i));   // State
  ///       }
  /// 
  ///       // End the session to gracefully disconnect from the server
  ///       session.End();
  ///    }
  /// }
  /// ]]>
  ///       </code>
  ///     </example>
  public class LoanEmployers : ILoanEmployers
  {
    private Loan loan;
    private bool borrower;

    internal LoanEmployers(Loan loan, bool borrower)
    {
      this.loan = loan;
      this.borrower = borrower;
    }

    /// <summary>
    /// Gets the number of employer definitions within the set.
    /// </summary>
    /// <example>
    /// The following code removes all employment verification records for the
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
    ///       for (int i = loan.BorrowerEmployers.Count; i >= 1; i--)
    ///       {
    ///          // Write out the name of the account holder
    ///          Console.WriteLine("Employer Name: " + loan.Fields.GetFieldAt("BE02", i).Value);
    /// 
    ///          // Remove the deposit
    ///          loan.BorrowerEmployers.RemoveAt(i);
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
    public int Count => this.loan.LoanData.GetNumberOfEmployer(this.borrower);

    /// <summary>Adds a new employer definition to the set.</summary>
    /// <param name="current">A flag indicating if the employer represents
    /// the borrower's/coborrower's current employer.</param>
    /// <returns>The index of the newly created employer.</returns>
    /// <example>
    /// The following code adds an employer verification record for the coborrower
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
    ///       // Add a new Employment verification record for the coborrower
    ///       int index = loan.CoBorrowerEmployers.Add(true);
    /// 
    ///       // Populate the fields for the record
    ///       loan.Fields.GetFieldAt("CE02", index).Value = "Megacorp Industries";
    ///       loan.Fields.GetFieldAt("CE05", index).Value = "Rahway";
    ///       loan.Fields.GetFieldAt("CE06", index).Value = "NJ";
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
    public int Add(bool current) => this.loan.LoanData.NewEmployer(this.borrower, current) + 1;

    /// <summary>Removes an employer definition from the set.</summary>
    /// <param name="index">The 1-based index of the employer to remove.</param>
    /// <example>
    /// The following code removes all employment verification records for the
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
    ///       for (int i = loan.BorrowerEmployers.Count; i >= 1; i--)
    ///       {
    ///          // Write out the name of the account holder
    ///          Console.WriteLine("Employer Name: " + loan.Fields.GetFieldAt("BE02", i).Value);
    /// 
    ///          // Remove the deposit
    ///          loan.BorrowerEmployers.RemoveAt(i);
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
      this.loan.LoanData.RemoveEmployerAt(this.borrower, index - 1);
    }
  }
}
