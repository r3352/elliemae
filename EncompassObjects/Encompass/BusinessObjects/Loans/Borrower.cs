// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Borrower
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>
  /// Represents a single borrower within a loan file. Borrowers always exist as part of
  /// a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.BorrowerPair">BorrowerPair</see>.
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
  public class Borrower : IBorrower
  {
    private EllieMae.EMLite.DataEngine.Borrower borrower;
    private Loan loan;

    internal Borrower(Loan loan, EllieMae.EMLite.DataEngine.Borrower borrower)
    {
      this.loan = loan;
      this.borrower = borrower;
    }

    /// <summary>Gets the borrower's first name.</summary>
    public string FirstName => this.borrower.FirstName;

    /// <summary>Gets the borrower's last name.</summary>
    public string LastName => this.borrower.LastName;

    /// <summary>
    /// Gets a unique identifier for this borrower within the current loan file.
    /// </summary>
    public string ID => this.borrower.Id;

    /// <summary>Provides a string representation of the object.</summary>
    /// <returns>The borrower full name (first and last)</returns>
    public override string ToString() => this.FirstName + " " + this.LastName;

    /// <summary>
    /// Compares two Borrower objects to determine if they are the same.
    /// </summary>
    /// <param name="obj">The Borrower to compare to the current object.</param>
    /// <returns><c>true</c> if the Borrower objects represent the same borrower,
    /// <c>false</c> otherwise.</returns>
    public override bool Equals(object obj)
    {
      Borrower objA = obj as Borrower;
      return !object.Equals((object) objA, (object) null) && this.loan.Equals((object) objA.loan) && this.ID == objA.ID;
    }

    /// <summary>Provides a hash code for the borrower.</summary>
    /// <returns>The has code of the borrower's ID.</returns>
    public override int GetHashCode() => this.borrower.Id.GetHashCode();

    /// <summary>Equality comparison for two Borrower objects.</summary>
    /// <param name="borA">The first Borrower to compare.</param>
    /// <param name="borB">The second Borrower to compare.</param>
    /// <returns><c>true</c> if the Borrower objects represent the same borrower,
    /// <c>false</c> otherwise.</returns>
    public static bool operator ==(Borrower borA, Borrower borB)
    {
      return object.Equals((object) borA, (object) borB);
    }

    /// <summary>Inequality operator for two Borrower objects.</summary>
    /// <param name="borA">The first Borrower to compare.</param>
    /// <param name="borB">The second Borrower to compare.</param>
    /// <returns><c>false</c> if the Borrower objects represent the same borrower,
    /// <c>true</c> otherwise.</returns>
    public static bool operator !=(Borrower borA, Borrower borB) => !(borA == borB);

    internal EllieMae.EMLite.DataEngine.Borrower Unwrap() => this.borrower;
  }
}
