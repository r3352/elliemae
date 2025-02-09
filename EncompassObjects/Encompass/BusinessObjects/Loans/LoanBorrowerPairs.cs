// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.LoanBorrowerPairs
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.Encompass.Collections;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>
  /// Represents the collection of all <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.BorrowerPair">BorrowerPair</see>
  /// associated with the current Loan.
  /// </summary>
  /// <example>
  ///       The following code demonstrates how to add a second Borrower Pair to a loan
  ///       and then manipulate the two pairs independently.
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
  ///          session.StartOffline("mary", "maryspwd");
  /// 
  ///          // Create the empty shell for the new loan.
  ///          Loan loan = session.Loans.CreateNew();
  /// 
  ///          // Set the loan folder and loan name for the loan
  ///          loan.LoanFolder = "My Pipeline";
  ///          loan.LoanName = "BorrowerPairExample";
  /// 
  ///          // The loan is created with one Borrower Pair already, so set
  ///          // the borrower and coborrower names
  ///          loan.Fields["36"].Value = "Howard";        // Borrower First name
  ///          loan.Fields["37"].Value = "Harrison";      // Borrower Last name
  ///          loan.Fields["68"].Value = "Martha";        // CoBorrower First name
  ///          loan.Fields["69"].Value = "Harrison";      // CoBorrower Last name
  /// 
  ///          // Add a new borrower pair to the loan
  ///          BorrowerPair newPair = loan.BorrowerPairs.Add();
  /// 
  ///          // Set the borrower and coborrower information for this pair
  ///          loan.Fields["36"].SetValueForBorrowerPair(newPair, "Caroline");
  ///          loan.Fields["37"].SetValueForBorrowerPair(newPair, "Irving");
  ///          loan.Fields["68"].SetValueForBorrowerPair(newPair, "Thomas");
  ///          loan.Fields["69"].SetValueForBorrowerPair(newPair, "Irving");
  /// 
  ///          // Set the newly created pair as the current (primary) pair
  ///          loan.BorrowerPairs.Current = newPair;
  /// 
  ///          // Set the mailing address for the "current" pair
  ///          loan.Fields["1519"].Value = "20221 Highway 99";
  ///          loan.Fields["1520"].Value = "Maynorsville";
  ///          loan.Fields["1521"].Value = "IA";
  ///          loan.Fields["1522"].Value = "51223";
  /// 
  ///          // Commit the changes to the server
  ///          loan.Commit();
  /// 
  ///          // End the session gracefully
  ///          session.End();
  ///    }
  /// }
  ///         ]]>
  ///       </code>
  ///     </example>
  public class LoanBorrowerPairs : ILoanBorrowerPairs, IEnumerable
  {
    private Loan loan;
    private BorrowerPairList borrowerPairs;
    private BorrowerPair currentPair;

    internal LoanBorrowerPairs(Loan loan)
    {
      this.loan = loan;
      this.RefreshPairs();
    }

    /// <summary>
    /// Provides access to the specified <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.BorrowerPair">BorrowerPair</see>
    /// by index.
    /// </summary>
    /// <example>
    /// The following code demonstrates how to enumerate the Borrower Pairs
    /// associated with a loan.
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
    ///       // Enumerate over the set of BorrowerPairs
    ///       for (int i = 0; i < loan.BorrowerPairs.Count; i++)
    ///       {
    ///          // Fetch the current Borrower Pair
    ///          BorrowerPair pair = loan.BorrowerPairs[i];
    /// 
    ///          // Dump the personal info for this pair
    ///          Console.WriteLine("Borrower Pair " + i);
    ///          Console.WriteLine(loan.Fields["36"].GetValueForBorrowerPair(pair));  // Borrower First Name
    ///          Console.WriteLine(loan.Fields["37"].GetValueForBorrowerPair(pair));  // Borrower Last Name
    ///          Console.WriteLine(loan.Fields["68"].GetValueForBorrowerPair(pair));  // CoBorrower First Name
    ///          Console.WriteLine(loan.Fields["69"].GetValueForBorrowerPair(pair));  // CoBorrower Last Name
    ///       }
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public BorrowerPair this[int index] => this.borrowerPairs[index];

    /// <summary>
    /// Gets the number of <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.BorrowerPair">BorrowerPairs</see> defined in the
    /// Loan.
    /// </summary>
    /// <example>
    /// The following code demonstrates how to enumerate the Borrower Pairs
    /// associated with a loan.
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
    ///       // Enumerate over the set of BorrowerPairs
    ///       for (int i = 0; i < loan.BorrowerPairs.Count; i++)
    ///       {
    ///          // Fetch the current Borrower Pair
    ///          BorrowerPair pair = loan.BorrowerPairs[i];
    /// 
    ///          // Dump the personal info for this pair
    ///          Console.WriteLine("Borrower Pair " + i);
    ///          Console.WriteLine(loan.Fields["36"].GetValueForBorrowerPair(pair));  // Borrower First Name
    ///          Console.WriteLine(loan.Fields["37"].GetValueForBorrowerPair(pair));  // Borrower Last Name
    ///          Console.WriteLine(loan.Fields["68"].GetValueForBorrowerPair(pair));  // CoBorrower First Name
    ///          Console.WriteLine(loan.Fields["69"].GetValueForBorrowerPair(pair));  // CoBorrower Last Name
    ///       }
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public int Count => this.borrowerPairs.Count;

    /// <summary>
    /// Adds a new <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.BorrowerPair">BorrowerPair</see> to the loan.
    /// </summary>
    /// <remarks><p>This method does not set the new borrower pair as the current
    /// pair. Use the <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.LoanBorrowerPairs.Current">Current</see> property to make this
    /// pair current.</p>
    /// <p>This method requires that an exclusive lock be held on the loan.</p>
    /// </remarks>
    /// <example>
    ///       The following code demonstrates how to add a second Borrower Pair to a loan
    ///       and then manipulate the two pairs independently.
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
    ///          session.StartOffline("mary", "maryspwd");
    /// 
    ///          // Create the empty shell for the new loan.
    ///          Loan loan = session.Loans.CreateNew();
    /// 
    ///          // Set the loan folder and loan name for the loan
    ///          loan.LoanFolder = "My Pipeline";
    ///          loan.LoanName = "BorrowerPairExample";
    /// 
    ///          // The loan is created with one Borrower Pair already, so set
    ///          // the borrower and coborrower names
    ///          loan.Fields["36"].Value = "Howard";        // Borrower First name
    ///          loan.Fields["37"].Value = "Harrison";      // Borrower Last name
    ///          loan.Fields["68"].Value = "Martha";        // CoBorrower First name
    ///          loan.Fields["69"].Value = "Harrison";      // CoBorrower Last name
    /// 
    ///          // Add a new borrower pair to the loan
    ///          BorrowerPair newPair = loan.BorrowerPairs.Add();
    /// 
    ///          // Set the borrower and coborrower information for this pair
    ///          loan.Fields["36"].SetValueForBorrowerPair(newPair, "Caroline");
    ///          loan.Fields["37"].SetValueForBorrowerPair(newPair, "Irving");
    ///          loan.Fields["68"].SetValueForBorrowerPair(newPair, "Thomas");
    ///          loan.Fields["69"].SetValueForBorrowerPair(newPair, "Irving");
    /// 
    ///          // Set the newly created pair as the current (primary) pair
    ///          loan.BorrowerPairs.Current = newPair;
    /// 
    ///          // Set the mailing address for the "current" pair
    ///          loan.Fields["1519"].Value = "20221 Highway 99";
    ///          loan.Fields["1520"].Value = "Maynorsville";
    ///          loan.Fields["1521"].Value = "IA";
    ///          loan.Fields["1522"].Value = "51223";
    /// 
    ///          // Commit the changes to the server
    ///          loan.Commit();
    /// 
    ///          // End the session gracefully
    ///          session.End();
    ///    }
    /// }
    ///         ]]>
    ///       </code>
    ///     </example>
    public BorrowerPair Add()
    {
      this.loan.EnsureExclusive();
      if (this.Count >= 6)
        throw new InvalidOperationException("Cannot add more than 6 Borrower Pairs");
      EllieMae.EMLite.DataEngine.BorrowerPair borrowerPair = this.loan.LoanData.CreateBorrowerPair();
      this.RefreshPairs();
      return new BorrowerPair(this.loan, borrowerPair);
    }

    /// <summary>
    /// Removes a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.BorrowerPair">BorrowerPair</see> from the loan.
    /// </summary>
    /// <param name="pair">The BorrowerPair to be removed. This pair must belong
    /// to the current loan.</param>
    /// <remarks>A loan must always have a least one BorrowerPair. Thus, an attempt
    /// to remove the sole BorrowerPair will result in an InvalidOperationException.
    /// <p>This method requires that an exclusive lock be held on the loan.</p>
    /// </remarks>
    /// <example>
    /// The following code removes all BorrowerPairs except the primary pair from
    /// the loan.
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
    ///       // Enumerate over the set of BorrowerPairs
    ///       for (int i = loan.BorrowerPairs.Count - 1; i >= 0; i--)
    ///       {
    ///          // Fetch the current Borrower Pair
    ///          BorrowerPair pair = loan.BorrowerPairs[i];
    /// 
    ///          if (pair != loan.BorrowerPairs.Current)
    ///             loan.BorrowerPairs.Remove(pair);
    ///       }
    /// 
    ///       // Commit the changes and unlock the loan
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
    public void Remove(BorrowerPair pair)
    {
      if (this.Count <= 1)
        throw new InvalidOperationException("Cannot delete all Borrower Pairs from loan");
      this.loan.EnsureExclusive();
      if (pair == this.Current)
      {
        for (int index = 0; index < this.Count; ++index)
        {
          if (this[index] != pair)
          {
            this.Current = this[index];
            break;
          }
        }
      }
      this.loan.LoanData.RemoveBorrowerPair(pair.Unwrap());
      this.RefreshPairs();
    }

    /// <summary>
    /// Swaps the positions of two BorrowerPairs in the list of pairs associated with the
    /// loan.
    /// </summary>
    /// <param name="pairA">The first pair to swap.</param>
    /// <param name="pairB">The second pair to swap.</param>
    /// <remarks>
    /// <p>This method requires that an exclusive lock be held on the loan.</p>
    /// </remarks>
    /// <example>
    /// The following code removes all BorrowerPairs except the primary pair from
    /// the loan.
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
    ///       if (loan.BorrowerPairs.Count > 1)
    ///       {
    ///          // Retrieve the first two borrower pairs from the loan
    ///          BorrowerPair pairA = loan.BorrowerPairs[0];
    ///          BorrowerPair pairB = loan.BorrowerPairs[1];
    /// 
    ///          // Swap their oerder in the loan
    ///          loan.BorrowerPairs.Swap(pairA, pairB);
    /// 
    ///          // Save the changes back to the server
    ///          loan.Commit();
    ///       }
    /// 
    ///       // Close the loan to release its resources
    ///       loan.Close();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public void Swap(BorrowerPair pairA, BorrowerPair pairB)
    {
      this.loan.EnsureExclusive();
      this.loan.LoanData.SwapBorrowerPairs(new EllieMae.EMLite.DataEngine.BorrowerPair[2]
      {
        pairA.Unwrap(),
        pairB.Unwrap()
      });
      this.RefreshPairs();
    }

    /// <summary>Gets or sets the current BorrowerPair for the loan.</summary>
    /// <remarks>All subsequent field accesses that have a borrower or coborrower
    /// component will pertain to the selected pair.</remarks>
    /// <example>
    ///       The following code demonstrates how to add a second Borrower Pair to a loan
    ///       and then manipulate the two pairs independently.
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
    ///          session.StartOffline("mary", "maryspwd");
    /// 
    ///          // Create the empty shell for the new loan.
    ///          Loan loan = session.Loans.CreateNew();
    /// 
    ///          // Set the loan folder and loan name for the loan
    ///          loan.LoanFolder = "My Pipeline";
    ///          loan.LoanName = "BorrowerPairExample";
    /// 
    ///          // The loan is created with one Borrower Pair already, so set
    ///          // the borrower and coborrower names
    ///          loan.Fields["36"].Value = "Howard";        // Borrower First name
    ///          loan.Fields["37"].Value = "Harrison";      // Borrower Last name
    ///          loan.Fields["68"].Value = "Martha";        // CoBorrower First name
    ///          loan.Fields["69"].Value = "Harrison";      // CoBorrower Last name
    /// 
    ///          // Add a new borrower pair to the loan
    ///          BorrowerPair newPair = loan.BorrowerPairs.Add();
    /// 
    ///          // Set the borrower and coborrower information for this pair
    ///          loan.Fields["36"].SetValueForBorrowerPair(newPair, "Caroline");
    ///          loan.Fields["37"].SetValueForBorrowerPair(newPair, "Irving");
    ///          loan.Fields["68"].SetValueForBorrowerPair(newPair, "Thomas");
    ///          loan.Fields["69"].SetValueForBorrowerPair(newPair, "Irving");
    /// 
    ///          // Set the newly created pair as the current (primary) pair
    ///          loan.BorrowerPairs.Current = newPair;
    /// 
    ///          // Set the mailing address for the "current" pair
    ///          loan.Fields["1519"].Value = "20221 Highway 99";
    ///          loan.Fields["1520"].Value = "Maynorsville";
    ///          loan.Fields["1521"].Value = "IA";
    ///          loan.Fields["1522"].Value = "51223";
    /// 
    ///          // Commit the changes to the server
    ///          loan.Commit();
    /// 
    ///          // End the session gracefully
    ///          session.End();
    ///    }
    /// }
    ///         ]]>
    ///       </code>
    ///     </example>
    public BorrowerPair Current
    {
      get
      {
        if (this.loan.Unwrap().LoanData.CurrentBorrowerPair.Id != this.currentPair.Unwrap().Id)
          this.RefreshPairs();
        return this.currentPair;
      }
      set
      {
        this.loan.LoanData.SetBorrowerPair(value.Unwrap());
        this.currentPair = value;
      }
    }

    /// <summary>
    /// Gets an enumerator for iterating over the contents of the collection.
    /// </summary>
    /// <returns>An IEnuerator instance for the collection.</returns>
    public IEnumerator GetEnumerator() => this.borrowerPairs.GetEnumerator();

    /// <summary>
    /// Refreshes the list if it contents have become stale due to fields being
    /// modified on the underlying Loan.
    /// </summary>
    /// <example>
    /// The following code modifies the set of Borrower Pairs and then calls Refresh()
    /// to have changes to the underlying field data reflected in the BorrowerPairs
    /// object.
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
    ///       // Print the current state of the pairs
    ///       dumpBorrowerPairs(loan);
    /// 
    ///       // Lock the loan so we can edit it safely
    ///       loan.Lock();
    /// 
    ///       // Add a new Borrower Pair
    ///       BorrowerPair newPair = loan.BorrowerPairs.Add();
    /// 
    ///       // Set this pair as the current pair
    ///       loan.BorrowerPairs.Current = newPair;
    /// 
    ///       // Set the name for the primary borrower of the pair
    ///       loan.Fields["36"].Value = "Janet";
    ///       loan.Fields["37"].Value = "Hardesty";
    /// 
    ///       // Print the current state of the pairs again. Note that the
    ///       // changes we just made are not reflected.
    ///       dumpBorrowerPairs(loan);
    /// 
    ///       // Force a refresh of the Borrower pairs to reflect the changes we've made on
    ///       // the underlying loan.
    ///       loan.BorrowerPairs.Refresh();
    /// 
    ///       // Now when we print the borrower pairs, the changes from the underlying loan
    ///       // will show.
    ///       dumpBorrowerPairs(loan);
    /// 
    ///       // Close the loan, discarding all of our changes
    ///       loan.Close();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// 
    ///    // Writes all of the BorrowerPair information to the console
    ///    private static void dumpBorrowerPairs(Loan loan)
    ///    {
    ///       // Loop over the BorrowerPairs, printing their names
    ///       foreach (BorrowerPair pair in loan.BorrowerPairs)
    ///       {
    ///          Console.WriteLine("Borrower First Name:   " + pair.Borrower.FirstName);
    ///          Console.WriteLine("Borrower Last Name:    " + pair.Borrower.LastName);
    ///          Console.WriteLine("CoBorrower First Name: " + pair.CoBorrower.FirstName);
    ///          Console.WriteLine("CoBorrower Last Name:  " + pair.CoBorrower.LastName);
    ///       }
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public void Refresh() => this.RefreshPairs();

    /// <summary>
    /// Returns the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.BorrowerPair" /> with the specified ID.
    /// </summary>
    /// <param name="pairId">The ID of the desired borrower pair.</param>
    /// <returns>The <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.BorrowerPair" /> with the specified ID, if it exists;
    /// <c>null</c> otherwise.</returns>
    internal BorrowerPair GetPairByID(string pairId)
    {
      foreach (BorrowerPair pairById in this)
      {
        if (pairById.Borrower.ID == pairId)
          return pairById;
      }
      return (BorrowerPair) null;
    }

    internal BorrowerPair GetPair(EllieMae.EMLite.DataEngine.BorrowerPair nativePair)
    {
      BorrowerPair pair = this.findPair(nativePair);
      if (pair != (BorrowerPair) null)
        return pair;
      this.RefreshPairs();
      return this.findPair(nativePair);
    }

    internal void RefreshPairs()
    {
      this.borrowerPairs = BorrowerPair.ToList(this.loan, this.loan.LoanData.GetBorrowerPairs());
      EllieMae.EMLite.DataEngine.BorrowerPair currentBorrowerPair = this.loan.LoanData.CurrentBorrowerPair;
      if (currentBorrowerPair == null)
        this.currentPair = (BorrowerPair) null;
      else
        this.currentPair = new BorrowerPair(this.loan, currentBorrowerPair);
    }

    private BorrowerPair findPair(EllieMae.EMLite.DataEngine.BorrowerPair nativePair)
    {
      foreach (BorrowerPair borrowerPair in (CollectionBase) this.borrowerPairs)
      {
        if (borrowerPair.Unwrap().Equals((object) nativePair))
          return borrowerPair;
      }
      return (BorrowerPair) null;
    }
  }
}
