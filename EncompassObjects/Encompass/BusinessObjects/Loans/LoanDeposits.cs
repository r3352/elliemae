// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.LoanDeposits
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>
  /// Provides access to the set of Assets/Deposits associated with a loan.
  /// </summary>
  /// <remarks>The items within this set are indexed starting with the value
  /// 1. An attempt to access an item in this collection with a value less than
  /// 1 will result in an InvalidArgumentException.
  /// </remarks>
  /// <example>
  ///       The following code demonstrates how to add a new asset/deposit to an existing
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
  ///          // Add a new deposit and save off it index in the deposits list
  ///          int newIndex = loan.Deposits.Add();
  /// 
  ///          // Set the value of some of theliability fields
  ///          loan.Fields.GetFieldAt("FD02", newIndex).Value = "Thomas Olden";    // Asset Holder
  ///          loan.Fields.GetFieldAt("FD10", newIndex).Value = "2220001-003";     // Account #
  ///          loan.Fields.GetFieldAt("FD26", newIndex).Value = "(555) 555-0233";  // Holder Phone
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
  public class LoanDeposits : ILoanDeposits
  {
    private Loan loan;

    internal LoanDeposits(Loan loan) => this.loan = loan;

    /// <summary>
    /// Gets the number of Deposits defined for the current loan.
    /// </summary>
    /// <example>
    /// The following code removes all deposit verification records from a loan file.
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
    ///       // Loop over all of the currently defined deposits attached to the loan.
    ///       // The desposits are indexed starting at 1, so the loop must adjust
    ///       // accordingly.
    ///       for (int i = loan.Deposits.Count; i >= 1; i--)
    ///       {
    ///          // Write out the name of the account holder
    ///          Console.WriteLine("Deposit Holder: " + loan.Fields.GetFieldAt("DD02", i).Value);
    /// 
    ///          // Remove the deposit
    ///          loan.Deposits.RemoveAt(i);
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
    public int Count => this.loan.LoanData.GetNumberOfDeposits();

    /// <summary>Adds a new deposit to the loan.</summary>
    /// <returns>The function returns the index of the newly created deposit.
    /// This value should be used to access deposit-related loan fields
    /// using the GetFieldAt() method of the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.LoanFields">LoanFields</see>
    /// object.
    /// </returns>
    /// <example>
    /// The following code demonstrates how to add a new asset/deposit to an existing
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
    ///       // Add a new deposit and save off it index in the deposits list
    ///       int newIndex = loan.Deposits.Add();
    /// 
    ///       // Set the value of some of theliability fields
    ///       loan.Fields.GetFieldAt("FD02", newIndex).Value = "Thomas Olden";    // Asset Holder
    ///       loan.Fields.GetFieldAt("FD10", newIndex).Value = "2220001-003";     // Account #
    ///       loan.Fields.GetFieldAt("FD26", newIndex).Value = "(555) 555-0233";  // Holder Phone
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
    public int Add() => this.loan.LoanData.NewDeposit() + 1;

    /// <summary>Removes a deposit from the current loan.</summary>
    /// <param name="index">The 1-based index of the deposit to remove.</param>
    /// <example>
    /// The following code removes all deposit verification records from a loan file.
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
    ///       // Loop over all of the currently defined deposits attached to the loan.
    ///       // The desposits are indexed starting at 1, so the loop must adjust
    ///       // accordingly.
    ///       for (int i = loan.Deposits.Count; i >= 1; i--)
    ///       {
    ///          // Write out the name of the account holder
    ///          Console.WriteLine("Deposit Holder: " + loan.Fields.GetFieldAt("DD02", i).Value);
    /// 
    ///          // Remove the deposit
    ///          loan.Deposits.RemoveAt(i);
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
      this.loan.LoanData.RemoveDepositAt(index - 1);
    }
  }
}
