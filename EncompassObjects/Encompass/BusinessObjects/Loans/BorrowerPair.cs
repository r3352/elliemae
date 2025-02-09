// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.BorrowerPair
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.Encompass.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>
  /// Represents a borrower/coborrower pair within a loan file.
  /// </summary>
  /// <example>
  /// The following code demonstrates how different borrower pairs can be accessed
  /// when attached to a loan.
  /// <code>
  /// <![CDATA[
  /// using System;
  /// using System.IO;
  /// using EllieMae.Encompass.Client;
  /// using EllieMae.Encompass.BusinessObjects;
  /// using EllieMae.Encompass.BusinessObjects.Loans;
  /// 
  /// class ContactManager
  /// {
  ///    public static void Main(string[] args)
  ///    {
  ///       // Open the session to the remote server
  ///       Session session = new Session();
  ///       session.StartOffline("mary", "maryspwd");
  /// 
  ///       // Open the loan using the GUID specified on the command line
  ///       Loan loan = session.Loans.Open(args[0]);
  /// 
  ///       // Loop over the BorrowerPairs, printing their names
  ///       foreach (BorrowerPair pair in loan.BorrowerPairs)
  ///       {
  ///          Console.WriteLine("Borrower First Name:   " + pair.Borrower.FirstName);
  ///          Console.WriteLine("Borrower Last Name:    " + pair.Borrower.LastName);
  ///          Console.WriteLine("CoBorrower First Name: " + pair.CoBorrower.FirstName);
  ///          Console.WriteLine("CoBorrower Last Name:  " + pair.CoBorrower.LastName);
  ///       }
  /// 
  ///       // Close the loan, discarding all of our changes
  ///       loan.Close();
  /// 
  ///       // End the session to gracefully disconnect from the server
  ///       session.End();
  ///    }
  /// }
  /// ]]>
  /// </code>
  /// </example>
  public class BorrowerPair : IBorrowerPair
  {
    private EllieMae.EMLite.DataEngine.BorrowerPair pair;
    private Borrower borrower;
    private Borrower coborrower;
    private Loan loan;

    internal BorrowerPair(Loan loan, EllieMae.EMLite.DataEngine.BorrowerPair pair)
    {
      this.loan = loan;
      this.pair = pair;
      if (pair.Borrower != null)
        this.borrower = new Borrower(loan, pair.Borrower);
      if (pair.CoBorrower == null)
        return;
      this.coborrower = new Borrower(loan, pair.CoBorrower);
    }

    /// <summary>Gets the primary borrower for the borrower pair.</summary>
    /// <example>
    /// The following code demonstrates how different borrower pairs can be accessed
    /// when attached to a loan.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// 
    /// class ContactManager
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.StartOffline("mary", "maryspwd");
    /// 
    ///       // Open the loan using the GUID specified on the command line
    ///       Loan loan = session.Loans.Open(args[0]);
    /// 
    ///       // Loop over the BorrowerPairs, printing their names
    ///       foreach (BorrowerPair pair in loan.BorrowerPairs)
    ///       {
    ///          Console.WriteLine("Borrower First Name:   " + pair.Borrower.FirstName);
    ///          Console.WriteLine("Borrower Last Name:    " + pair.Borrower.LastName);
    ///          Console.WriteLine("CoBorrower First Name: " + pair.CoBorrower.FirstName);
    ///          Console.WriteLine("CoBorrower Last Name:  " + pair.CoBorrower.LastName);
    ///       }
    /// 
    ///       // Close the loan, discarding all of our changes
    ///       loan.Close();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public Borrower Borrower => this.borrower;

    /// <summary>Gets the secondary borrower for the borrower pair.</summary>
    /// <example>
    /// The following code demonstrates how different borrower pairs can be accessed
    /// when attached to a loan.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// 
    /// class ContactManager
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.StartOffline("mary", "maryspwd");
    /// 
    ///       // Open the loan using the GUID specified on the command line
    ///       Loan loan = session.Loans.Open(args[0]);
    /// 
    ///       // Loop over the BorrowerPairs, printing their names
    ///       foreach (BorrowerPair pair in loan.BorrowerPairs)
    ///       {
    ///          Console.WriteLine("Borrower First Name:   " + pair.Borrower.FirstName);
    ///          Console.WriteLine("Borrower Last Name:    " + pair.Borrower.LastName);
    ///          Console.WriteLine("CoBorrower First Name: " + pair.CoBorrower.FirstName);
    ///          Console.WriteLine("CoBorrower Last Name:  " + pair.CoBorrower.LastName);
    ///       }
    /// 
    ///       // Close the loan, discarding all of our changes
    ///       loan.Close();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    /// <remarks>This value may be <c>null</c> if no coborrower is assigned to the loan.</remarks>
    public Borrower CoBorrower => this.coborrower;

    /// <summary>Provides a string representation of the object.</summary>
    /// <returns>The first and last names of the two borrowers separated by a forward slash.</returns>
    public override string ToString()
    {
      return this.borrower.ToString() + "/" + this.coborrower.ToString();
    }

    /// <summary>
    /// Determines if the current BorrowerPair represents the same pair as another object.
    /// </summary>
    /// <param name="obj">The BorrowerPair to which to compare the current object.</param>
    /// <returns><c>true</c> if the two BorrowerPair objects are equivalent,
    /// <c>false</c> otherwise.</returns>
    public override bool Equals(object obj)
    {
      BorrowerPair objA = obj as BorrowerPair;
      return !object.Equals((object) objA, (object) null) && this.loan.Equals((object) objA.loan) && this.Borrower.ID == objA.Borrower.ID;
    }

    /// <summary>Provides a has code for the BorrowerPair.</summary>
    /// <returns>A hash code generated from the IDs of the borrower and coborrower.</returns>
    public override int GetHashCode()
    {
      return this.coborrower != (Borrower) null ? this.borrower.GetHashCode() ^ this.coborrower.GetHashCode() : this.borrower.GetHashCode();
    }

    /// <summary>Compares two BorrowerPair objects for equivalence.</summary>
    /// <remarks>Two BorrowerPairs are considered equivalent if and only if the
    /// IDs of the borrower and coborrower contained in each are the same.</remarks>
    /// <param name="pairA">The first BorrowerPair to compare.</param>
    /// <param name="pairB">The second BorrowerPair to compare.</param>
    /// <returns><c>true</c> if the two BorrowerPair objects are equivalent,
    /// <c>false</c> otherwise.</returns>
    public static bool operator ==(BorrowerPair pairA, BorrowerPair pairB)
    {
      return object.Equals((object) pairA, (object) pairB);
    }

    /// <summary>Compares two BorrowerPair objects for equivalence.</summary>
    /// <remarks>Two BorrowerPairs are considered equivalent if and only if the
    /// IDs of the borrower and coborrower contained in each are the same.</remarks>
    /// <param name="pairA">The first BorrowerPair to compare.</param>
    /// <param name="pairB">The second BorrowerPair to compare.</param>
    /// <returns><c>false</c> if the two BorrowerPair objects are equivalent,
    /// <c>true</c> otherwise.</returns>
    public static bool operator !=(BorrowerPair pairA, BorrowerPair pairB) => !(pairA == pairB);

    internal EllieMae.EMLite.DataEngine.BorrowerPair Unwrap() => this.pair;

    internal static BorrowerPairList ToList(Loan loan, EllieMae.EMLite.DataEngine.BorrowerPair[] pairs)
    {
      BorrowerPairList list = new BorrowerPairList();
      for (int index = 0; index < pairs.Length; ++index)
        list.Add(new BorrowerPair(loan, pairs[index]));
      return list;
    }
  }
}
