// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Query.NumericFieldCriterion
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer.Query;

#nullable disable
namespace EllieMae.Encompass.Query
{
  /// <summary>
  /// Represents a single query criterion based on a numeric field value.
  /// </summary>
  /// <example>
  /// The following example selects all Loans which have an amount of at least
  /// $200,000 and a term of exactly 30 years.
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
  ///       // Create the criterion for the Loan Amount
  ///       NumericFieldCriterion amtCri = new NumericFieldCriterion();
  ///       amtCri.FieldName = "Loan.LoanAmount";
  ///       amtCri.Value = 200000;
  ///       amtCri.MatchType = OrdinalFieldMatchType.GreaterThanOrEquals;
  /// 
  ///       NumericFieldCriterion termCri = new NumericFieldCriterion();
  ///       termCri.FieldName = "Loan.Term";
  ///       termCri.Value = 360;
  ///       termCri.MatchType = OrdinalFieldMatchType.Equals;
  /// 
  ///       // Perform the query against the loans
  ///       LoanIdentityList ids = session.Loans.Query(termCri.And(amtCri));
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
  public class NumericFieldCriterion : QueryCriterion, INumericFieldCriterion
  {
    private string fieldName = "";
    private double fieldValue;
    private OrdinalFieldMatchType matchType;

    /// <summary>Constructs an empty NumericFieldCriterion object.</summary>
    public NumericFieldCriterion()
    {
    }

    /// <summary>
    /// Constructs a new NumericFieldCriterion object by providing initial values
    /// for all the fields.
    /// </summary>
    /// <param name="fieldName">The name of the field against which to query</param>
    /// <param name="value">The value again which the test will be performed.</param>
    /// <param name="matchType">The desired ordinal relationship between the specified
    /// value and the field value.</param>
    /// <example>
    /// The following example performs a query on the Loan database using the
    /// NumericFieldCriterion to retrieve all loans for which the rate is greater
    /// than 5.75%.
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
    ///       // Create the numeric field criterion for the Loan Rate
    ///       NumericFieldCriterion cri = new NumericFieldCriterion("Loan.LoanRate",
    ///          5.75, OrdinalFieldMatchType.GreaterThan);
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
    public NumericFieldCriterion(string fieldName, double value, OrdinalFieldMatchType matchType)
    {
      this.fieldName = fieldName;
      this.fieldValue = value;
      this.matchType = matchType;
    }

    /// <summary>
    /// Gets or sets the name of the field to which this criterion applies.
    /// </summary>
    /// <example>
    /// The following example selects all Loans which have an amount of at least
    /// $200,000 and a term of exactly 30 years.
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
    ///       // Create the criterion for the Loan Amount
    ///       NumericFieldCriterion amtCri = new NumericFieldCriterion();
    ///       amtCri.FieldName = "Loan.LoanAmount";
    ///       amtCri.Value = 200000;
    ///       amtCri.MatchType = OrdinalFieldMatchType.GreaterThanOrEquals;
    /// 
    ///       NumericFieldCriterion termCri = new NumericFieldCriterion();
    ///       termCri.FieldName = "Loan.Term";
    ///       termCri.Value = 360;
    ///       termCri.MatchType = OrdinalFieldMatchType.Equals;
    /// 
    ///       // Perform the query against the loans
    ///       LoanIdentityList ids = session.Loans.Query(termCri.And(amtCri));
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
    /// The following example selects all Loans which have an amount of at least
    /// $200,000 and a term of exactly 30 years.
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
    ///       // Create the criterion for the Loan Amount
    ///       NumericFieldCriterion amtCri = new NumericFieldCriterion();
    ///       amtCri.FieldName = "Loan.LoanAmount";
    ///       amtCri.Value = 200000;
    ///       amtCri.MatchType = OrdinalFieldMatchType.GreaterThanOrEquals;
    /// 
    ///       NumericFieldCriterion termCri = new NumericFieldCriterion();
    ///       termCri.FieldName = "Loan.Term";
    ///       termCri.Value = 360;
    ///       termCri.MatchType = OrdinalFieldMatchType.Equals;
    /// 
    ///       // Perform the query against the loans
    ///       LoanIdentityList ids = session.Loans.Query(termCri.And(amtCri));
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
    public double Value
    {
      get => this.fieldValue;
      set => this.fieldValue = value;
    }

    /// <summary>
    /// Gets or sets the desired ordinal relation between the specified value and the
    /// value of the field being queried.
    /// </summary>
    /// <example>
    /// The following example selects all Loans which have an amount of at least
    /// $200,000 and a term of exactly 30 years.
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
    ///       // Create the criterion for the Loan Amount
    ///       NumericFieldCriterion amtCri = new NumericFieldCriterion();
    ///       amtCri.FieldName = "Loan.LoanAmount";
    ///       amtCri.Value = 200000;
    ///       amtCri.MatchType = OrdinalFieldMatchType.GreaterThanOrEquals;
    /// 
    ///       NumericFieldCriterion termCri = new NumericFieldCriterion();
    ///       termCri.FieldName = "Loan.Term";
    ///       termCri.Value = 360;
    ///       termCri.MatchType = OrdinalFieldMatchType.Equals;
    /// 
    ///       // Perform the query against the loans
    ///       LoanIdentityList ids = session.Loans.Query(termCri.And(amtCri));
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
    public OrdinalFieldMatchType MatchType
    {
      get => this.matchType;
      set => this.matchType = value;
    }

    /// <summary>
    /// Creates an exact duplicate of the object by performing a deep copy.
    /// </summary>
    /// <returns>Returns a deep copy of the current criterion object.</returns>
    public override QueryCriterion Clone()
    {
      return (QueryCriterion) new NumericFieldCriterion(this.fieldName, this.fieldValue, this.matchType);
    }

    /// <summary>Unwrap the object</summary>
    /// <returns></returns>
    public override EllieMae.EMLite.ClientServer.Query.QueryCriterion Unwrap()
    {
      return (EllieMae.EMLite.ClientServer.Query.QueryCriterion) new OrdinalValueCriterion(this.fieldName, (object) this.fieldValue, (OrdinalMatchType) this.matchType);
    }
  }
}
