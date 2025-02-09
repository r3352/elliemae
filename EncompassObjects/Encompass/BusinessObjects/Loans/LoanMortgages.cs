// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.LoanMortgages
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.Encompass.Collections;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>
  /// Represents the set of mortgages associated with a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Loan">Loan</see>.
  /// </summary>
  /// <remarks>The items within this set are indexed starting with the value
  /// 1. An attempt to access an item in this collection with a value less than
  /// 1 will result in an InvalidArgumentException.
  /// </remarks>
  /// <example>
  ///       The following code demonstrates how to add an existing mortgage to a loan file
  ///       and then set a subset of its field values.
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
  ///          loan.Fields.GetFieldAt("FL02", newIndex).Value = "Thomas Olden";    // Asset Holder
  ///          loan.Fields.GetFieldAt("FL10", newIndex).Value = "2220001-003";     // Account #
  ///          loan.Fields.GetFieldAt("FL20", newIndex).Value = "(555) 555-0233";  // Holder Phone
  /// 
  ///          // Create an IntegerList to hold the ID of the liability
  ///          IntegerList liabIds = new IntegerList();
  ///          liabIds.Add(newIndex);
  /// 
  ///           // Create the new Mortgage, attaching the liability to it
  ///          int newMort = loan.Mortgages.Add(liabIds);
  /// 
  ///          // Set some Mortgage-related fields
  ///          loan.Fields.GetFieldAt("FM04", newMort).Value = "2056 Blue Hollow Lane";  // Street Addr
  ///          loan.Fields.GetFieldAt("FM06", newMort).Value = "Lake Mary";              // City
  ///          loan.Fields.GetFieldAt("FM07", newMort).Value = "FL";                     // State
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
  public class LoanMortgages : ILoanMortgages
  {
    private Loan loan;

    internal LoanMortgages(Loan loan) => this.loan = loan;

    /// <summary>Gets the number of liabilities in the set.</summary>
    /// <example>
    /// The following code removes all mortgage verification records from a loan file.
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
    ///       // Loop over all of the currently defined mortgages attached to the loan.
    ///       // The mortgages are indexed starting at 1, so the loop must adjust
    ///       // accordingly.
    ///       for (int i = loan.Mortgages.Count; i >= 1; i--)
    ///       {
    ///          // Write out the name of the account holder
    ///          Console.WriteLine("Property Address: " + loan.Fields.GetFieldAt("FM04", i).Value);
    /// 
    ///          // Remove the deposit
    ///          loan.Mortgages.RemoveAt(i);
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
    public int Count => this.loan.LoanData.GetNumberOfMortgages();

    /// <summary>Adds a new mortgage to the set.</summary>
    /// <param name="liabilities">The indices of the liabilities with which the
    /// mortgage is associated. At least one liability must be specified.</param>
    /// <returns />
    /// <example>
    /// The following code demonstrates how to add an existing mortgage to a loan file
    /// and then set a subset of its field values.
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
    ///       loan.Fields.GetFieldAt("FL02", newIndex).Value = "Thomas Olden";    // Asset Holder
    ///       loan.Fields.GetFieldAt("FL10", newIndex).Value = "2220001-003";     // Account #
    ///       loan.Fields.GetFieldAt("FL20", newIndex).Value = "(555) 555-0233";  // Holder Phone
    /// 
    ///       // Create an IntegerList to hold the ID of the liability
    ///       IntegerList liabIds = new IntegerList();
    ///       liabIds.Add(newIndex);
    /// 
    ///       // Create the new Mortgage, attaching the liability to it
    ///       int newMort = loan.Mortgages.Add(liabIds);
    /// 
    ///       // Set some Mortgage-related fields
    ///       loan.Fields.GetFieldAt("FM04", newMort).Value = "2056 Blue Hollow Lane";  // Street Addr
    ///       loan.Fields.GetFieldAt("FM06", newMort).Value = "Lake Mary";              // City
    ///       loan.Fields.GetFieldAt("FM07", newMort).Value = "FL";                     // State
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
    public int Add(IntegerList liabilities)
    {
      return this.loan.LoanData.NewMortgage(this.formatIntegerList(liabilities)) + 1;
    }

    /// <summary>Removes an existing mortgage from the set.</summary>
    /// <param name="index">The 1-based index of the mortgage to remove.</param>
    /// <example>
    /// The following code removes all mortgage verification records from a loan file.
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
    ///       // Loop over all of the currently defined mortgages attached to the loan.
    ///       // The mortgages are indexed starting at 1, so the loop must adjust
    ///       // accordingly.
    ///       for (int i = loan.Mortgages.Count; i >= 1; i--)
    ///       {
    ///          // Write out the name of the account holder
    ///          Console.WriteLine("Property Address: " + loan.Fields.GetFieldAt("FM04", i).Value);
    /// 
    ///          // Remove the deposit
    ///          loan.Mortgages.RemoveAt(i);
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
      this.loan.LoanData.RemoveMortgageAt(index - 1);
    }

    /// <summary>
    /// Attaches a mortgage to one or more liabilities within the loan.
    /// </summary>
    /// <param name="index">The index of the mortgage to which the liabilities
    /// will be attached.</param>
    /// <param name="liabilities">The indices of the liabilities to attach to
    /// the mortgage.</param>
    /// <example>
    /// The following code attaches two new liability records to an existing mortgage.
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
    ///       // Create two new liabilities to be attached to the mortgage
    ///       int index1 = loan.Liabilities.Add();
    ///       int index2 = loan.Liabilities.Add();
    /// 
    ///       // Set the holds of the liabilities
    ///       loan.Fields.GetFieldAt("FL02", index1).Value = "First Bank";
    ///       loan.Fields.GetFieldAt("FL02", index2).Value = "Second Bank";
    /// 
    ///       // Now attach the first mortgage record to these new liabilities
    ///       loan.Mortgages.AttachMortgage(1, new IntegerList(new int[] { index1, index2 }));
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
    public void AttachMortgage(int index, IntegerList liabilities)
    {
      if (index < 1)
        throw new ArgumentOutOfRangeException(nameof (index), (object) index, "The index must be greater than or equal to 1.");
      this.loan.LoanData.AttachMortgage(index.ToString("00"), this.formatIntegerList(liabilities));
    }

    /// <summary>
    /// Returns the liabilities linked to a specific mortgage.
    /// </summary>
    /// <param name="index">The index of the mortgage for which you need the list of liabilities.</param>
    /// <returns>Returns an <see cref="T:EllieMae.Encompass.Collections.IntegerList" /> containing the indices of the liabilities
    /// associated with the mortgage.</returns>
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
