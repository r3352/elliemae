// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Query.StringFieldCriterion
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer.Query;

#nullable disable
namespace EllieMae.Encompass.Query
{
  /// <summary>
  /// Represents a single query criterion based on a string field value.
  /// </summary>
  /// <example>
  /// The following example selects all Loans for which the Borrower's last
  /// name or the Coborrower's last name begins with the character 'S'.
  /// <code>
  /// <![CDATA[
  /// using System;
  /// using System.IO;
  /// using EllieMae.Encompass.Client;
  /// using EllieMae.Encompass.BusinessObjects;
  /// using EllieMae.Encompass.Query;
  /// 
  /// class LoanReader
  /// {
  ///    public static void Main()
  ///    {
  ///       // Open the session to the remote server
  ///       Session session = new Session();
  ///       session.Start("myserver", "mary", "maryspwd");
  /// 
  ///       // Create the string field Criterion for the Loan Purpose
  ///       StringFieldCriterion borCri = new StringFieldCriterion();
  ///       borCri.FieldName = "Loan.BorrowerLastName";
  ///       borCri.Value = "S";
  ///       borCri.MatchType = StringFieldMatchType.StartsWith;
  /// 
  ///       // Create the string field Criterion for the Loan Purpose
  ///       StringFieldCriterion cobCri = new StringFieldCriterion();
  ///       cobCri.FieldName = "Loan.CoBorrowerLastName";
  ///       cobCri.Value = "S";
  ///       cobCri.MatchType = StringFieldMatchType.StartsWith;
  /// 
  ///       // Perform the query against the loans
  ///       LoanIdentityList ids = session.Loans.Query(borCri.Or(cobCri));
  /// 
  ///       // Print the names of the matching loans
  ///       for (int i = 0; i < ids.Count; i++)
  ///          Console.WriteLine(ids[i].ToString());
  /// 
  ///       // End the session to gracefully disconnect from the server
  ///       session.End();
  ///    }
  /// }
  /// ]]>
  /// </code>
  /// </example>
  public class StringFieldCriterion : QueryCriterion, IStringFieldCriterion
  {
    private string fieldName = "";
    private string fieldValue = "";
    private StringFieldMatchType matchType;
    private bool include = true;

    /// <summary>Constructs an empty NumericFieldCriterion object.</summary>
    public StringFieldCriterion()
    {
    }

    /// <summary>
    /// Constructs a new NumericFieldCriterion object by providing initial values
    /// for all the fields.
    /// </summary>
    /// <param name="fieldName">The name of the field against which to query</param>
    /// <param name="value">The value again which the test will be performed.</param>
    /// <param name="matchType">The desired string relationship between the specified
    /// value and the field value.</param>
    /// <param name="include">A flag indicating if the criterion represents an
    /// inclusion rule or an exclusion rule.</param>
    /// <example>
    /// The following example performs a query on the Loan database using the
    /// StringFieldCriterion to specify that all loans that are for the purpose
    /// of a no-cash-out refinance should be selected.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.Query;
    /// 
    /// class LoanReader
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Create the string field Criterion for the Loan Purpose
    ///       StringFieldCriterion cri = new StringFieldCriterion("Loan.LoanPurpose",
    ///          "NoCash-Out Refinance", StringFieldMatchType.Exact, true);
    /// 
    ///       // Perform the query against the loans
    ///       LoanIdentityList ids = session.Loans.Query(cri);
    /// 
    ///       // Print the names of the matching loans
    ///       for (int i = 0; i < ids.Count; i++)
    ///          Console.WriteLine(ids[i].ToString());
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public StringFieldCriterion(
      string fieldName,
      string value,
      StringFieldMatchType matchType,
      bool include)
    {
      this.fieldName = fieldName;
      this.fieldValue = value;
      this.matchType = matchType;
      this.include = include;
    }

    /// <summary>
    /// Gets or sets the name of the field to which this criterion applies.
    /// </summary>
    /// <example>
    /// The following example selects all Loans for which the Borrower's last
    /// name or the Coborrower's last name begins with the character 'S'.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.Query;
    /// 
    /// class LoanReader
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Create the string field Criterion for the Loan Purpose
    ///       StringFieldCriterion borCri = new StringFieldCriterion();
    ///       borCri.FieldName = "Loan.BorrowerLastName";
    ///       borCri.Value = "S";
    ///       borCri.MatchType = StringFieldMatchType.StartsWith;
    /// 
    ///       // Create the string field Criterion for the Loan Purpose
    ///       StringFieldCriterion cobCri = new StringFieldCriterion();
    ///       cobCri.FieldName = "Loan.CoBorrowerLastName";
    ///       cobCri.Value = "S";
    ///       cobCri.MatchType = StringFieldMatchType.StartsWith;
    /// 
    ///       // Perform the query against the loans
    ///       LoanIdentityList ids = session.Loans.Query(borCri.Or(cobCri));
    /// 
    ///       // Print the names of the matching loans
    ///       for (int i = 0; i < ids.Count; i++)
    ///          Console.WriteLine(ids[i].ToString());
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public string FieldName
    {
      get => this.fieldName;
      set => this.fieldName = value;
    }

    /// <summary>
    /// Gets or sets the value against which the field will be tested.
    /// </summary>
    /// <example>
    /// The following example selects all Loans for which the Borrower's last
    /// name or the Coborrower's last name begins with the character 'S'.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.Query;
    /// 
    /// class LoanReader
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Create the string field Criterion for the Loan Purpose
    ///       StringFieldCriterion borCri = new StringFieldCriterion();
    ///       borCri.FieldName = "Loan.BorrowerLastName";
    ///       borCri.Value = "S";
    ///       borCri.MatchType = StringFieldMatchType.StartsWith;
    /// 
    ///       // Create the string field Criterion for the Loan Purpose
    ///       StringFieldCriterion cobCri = new StringFieldCriterion();
    ///       cobCri.FieldName = "Loan.CoBorrowerLastName";
    ///       cobCri.Value = "S";
    ///       cobCri.MatchType = StringFieldMatchType.StartsWith;
    /// 
    ///       // Perform the query against the loans
    ///       LoanIdentityList ids = session.Loans.Query(borCri.Or(cobCri));
    /// 
    ///       // Print the names of the matching loans
    ///       for (int i = 0; i < ids.Count; i++)
    ///          Console.WriteLine(ids[i].ToString());
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public string Value
    {
      get => this.fieldValue;
      set => this.fieldValue = value;
    }

    /// <summary>
    /// Gets or sets the desired string comparison method to be applied between the
    /// specified value and the field value.
    /// </summary>
    /// <example>
    /// The following example selects all Loans for which the Borrower's last
    /// name or the Coborrower's last name begins with the character 'S'.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.Query;
    /// 
    /// class LoanReader
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Create the string field Criterion for the Loan Purpose
    ///       StringFieldCriterion borCri = new StringFieldCriterion();
    ///       borCri.FieldName = "Loan.BorrowerLastName";
    ///       borCri.Value = "S";
    ///       borCri.MatchType = StringFieldMatchType.StartsWith;
    /// 
    ///       // Create the string field Criterion for the Loan Purpose
    ///       StringFieldCriterion cobCri = new StringFieldCriterion();
    ///       cobCri.FieldName = "Loan.CoBorrowerLastName";
    ///       cobCri.Value = "S";
    ///       cobCri.MatchType = StringFieldMatchType.StartsWith;
    /// 
    ///       // Perform the query against the loans
    ///       LoanIdentityList ids = session.Loans.Query(borCri.Or(cobCri));
    /// 
    ///       // Print the names of the matching loans
    ///       for (int i = 0; i < ids.Count; i++)
    ///          Console.WriteLine(ids[i].ToString());
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public StringFieldMatchType MatchType
    {
      get => this.matchType;
      set => this.matchType = value;
    }

    /// <summary>
    /// A flag indicating if this criterion represents an inclusion or exclusion rule.
    /// </summary>
    /// <example>
    /// The following example selects all Loans which are not for the purchase of
    /// a property.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.Query;
    /// 
    /// class LoanReader
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Create the string field Criterion for the Loan Purpose
    ///       StringFieldCriterion cri = new StringFieldCriterion();
    ///       cri.FieldName = "Loan.LoanPurpose";
    ///       cri.Value = "Purchase";
    ///       cri.MatchType = StringFieldMatchType.Exact;
    ///       cri.Include = false;
    /// 
    ///       // Perform the query against the loans
    ///       LoanIdentityList ids = session.Loans.Query(cri);
    /// 
    ///       // Print the names of the matching loans
    ///       for (int i = 0; i < ids.Count; i++)
    ///          Console.WriteLine(ids[i].ToString());
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public bool Include
    {
      get => this.include;
      set => this.include = value;
    }

    /// <summary>
    /// Creates an exact duplicate of the object by performing a deep copy.
    /// </summary>
    /// <returns>Returns a deep copy of the current criterion object.</returns>
    public override QueryCriterion Clone()
    {
      return (QueryCriterion) new StringFieldCriterion(this.fieldName, this.fieldValue, this.matchType, this.include);
    }

    /// <summary>Unwrap the object</summary>
    /// <returns></returns>
    public override EllieMae.EMLite.ClientServer.Query.QueryCriterion Unwrap()
    {
      return (EllieMae.EMLite.ClientServer.Query.QueryCriterion) new StringValueCriterion(this.fieldName, this.fieldValue, (StringMatchType) this.matchType, this.include);
    }
  }
}
