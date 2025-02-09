// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Query.DateFieldCriterion
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer.Query;
using System;

#nullable disable
namespace EllieMae.Encompass.Query
{
  /// <summary>
  /// Represents a single query criterion based on a DateTime value.
  /// </summary>
  /// <example>
  /// The following example selects all Loans which have closed in the current year
  /// where the property is in California.
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
  ///       // Create the date field Criterion for the Loan Closed data
  ///       DateFieldCriterion cloCri = new DateFieldCriterion();
  ///       cloCri.FieldName = "Loan.DateClosed";
  ///       cloCri.Value = DateTime.Now;
  ///       cloCri.MatchType = OrdinalFieldMatchType.Equals;
  ///       cloCri.Precision = DateFieldMatchPrecision.Year;
  /// 
  ///       StringFieldCriterion staCri = new StringFieldCriterion();
  ///       staCri.FieldName = "Loan.State";
  ///       staCri.Value = "CA";
  ///       staCri.MatchType = StringFieldMatchType.CaseInsensitive;
  /// 
  ///       // Perform the query against the loans
  ///       LoanIdentityList ids = session.Loans.Query(cloCri.And(staCri));
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
  public class DateFieldCriterion : QueryCriterion, IDateFieldCriterion
  {
    private string fieldName = "";
    private DateTime fieldValue = DateTime.Now;
    private OrdinalFieldMatchType matchType;
    private DateFieldMatchPrecision precision;
    /// <summary>
    /// A DateTime value which can be used to match against empty date fields.
    /// </summary>
    /// <remarks>When the <see cref="P:EllieMae.Encompass.Query.DateFieldCriterion.Value" /> of a DateFieldCriterion is set to <c>EmptyDate</c>,
    /// all records that do not contain a valid date value will be matched. To search for
    /// fields with non-empty dates, use the <see cref="F:EllieMae.Encompass.Query.DateFieldCriterion.NonEmptyDate" /> value. In both cases,
    /// the <see cref="P:EllieMae.Encompass.Query.DateFieldCriterion.MatchType" /> property must be set to <see cref="F:EllieMae.Encompass.Query.OrdinalFieldMatchType.Equals" />
    /// </remarks>
    public static readonly DateTime EmptyDate = DateTime.MinValue;
    /// <summary>
    /// A DateTime value which can be used to match against non-empty date fields.
    /// </summary>
    /// <remarks>When the <see cref="P:EllieMae.Encompass.Query.DateFieldCriterion.Value" /> of a DateFieldCriterion is set to <c>NonEmptyDate</c>,
    /// all records that contain a valid date value will be matched. To search for
    /// fields without a date value set, use the <see cref="F:EllieMae.Encompass.Query.DateFieldCriterion.EmptyDate" /> value. In both cases,
    /// the <see cref="P:EllieMae.Encompass.Query.DateFieldCriterion.MatchType" /> property must be set to <see cref="F:EllieMae.Encompass.Query.OrdinalFieldMatchType.Equals" />
    /// </remarks>
    public static readonly DateTime NonEmptyDate = DateTime.MaxValue;

    /// <summary>Constructs an empty DateFieldCriterion object.</summary>
    public DateFieldCriterion()
    {
    }

    /// <summary>
    /// Constructs a new DateFieldCriterion object by providing initial values
    /// for all the fields.
    /// </summary>
    /// <param name="fieldName">The name of the field against which to query</param>
    /// <param name="value">The value again which the test will be performed.</param>
    /// <param name="matchType">The desired ordinal relationship between the specified
    /// value and the field value.</param>
    /// <param name="precision">The precision with which the comparison is made.</param>
    /// <example>
    /// The following example performs a query on the Loan database using the
    /// DateFieldCriterion to retrieve all loans for which the rate lock expires
    /// on or after March 1, 2004.
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
    ///       // Create the date field Criterion for the Lock Expiration Date
    ///       DateFieldCriterion cri = new DateFieldCriterion("Loan.LockExpirationDate",
    ///          new DateTime(2004, 3, 1), OrdinalFieldMatchType.GreaterThanOrEquals,
    ///          DateFieldMatchPrecision.Day);
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
    public DateFieldCriterion(
      string fieldName,
      DateTime value,
      OrdinalFieldMatchType matchType,
      DateFieldMatchPrecision precision)
    {
      this.fieldName = fieldName;
      this.fieldValue = value;
      this.matchType = matchType;
      this.precision = precision;
    }

    /// <summary>
    /// Gets or sets the name of the field to which this criterion applies.
    /// </summary>
    /// <example>
    /// The following example selects all Loans which have closed in the current year
    /// where the property is in California.
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
    ///       // Create the date field Criterion for the Loan Closed data
    ///       DateFieldCriterion cloCri = new DateFieldCriterion();
    ///       cloCri.FieldName = "Loan.DateClosed";
    ///       cloCri.Value = DateTime.Now;
    ///       cloCri.MatchType = OrdinalFieldMatchType.Equals;
    ///       cloCri.Precision = DateFieldMatchPrecision.Year;
    /// 
    ///       StringFieldCriterion staCri = new StringFieldCriterion();
    ///       staCri.FieldName = "Loan.State";
    ///       staCri.Value = "CA";
    ///       staCri.MatchType = StringFieldMatchType.CaseInsensitive;
    /// 
    ///       // Perform the query against the loans
    ///       LoanIdentityList ids = session.Loans.Query(cloCri.And(staCri));
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
    /// <remarks>
    /// <p>To match records that contain any valid date value, set this property
    /// to the value specified by <see cref="F:EllieMae.Encompass.Query.DateFieldCriterion.NonEmptyDate" />. Conversely, to match records which do not
    /// contain a valid date, use <see cref="F:EllieMae.Encompass.Query.DateFieldCriterion.EmptyDate" />. Users of the COM interface will need
    /// to use the methods <see cref="M:EllieMae.Encompass.Query.IDateFieldCriterion.SetEmptyDateValue" /> and
    /// <see cref="M:EllieMae.Encompass.Query.IDateFieldCriterion.SetNonEmptyDateValue" /> for this purpose since the DateTime
    /// values represented by the constants fall outside the range allowed by COM.</p>
    /// </remarks>
    /// <example>
    /// The following example selects all Loans which have closed in the current year
    /// where the property is in California.
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
    ///       // Create the date field Criterion for the Loan Closed data
    ///       DateFieldCriterion cloCri = new DateFieldCriterion();
    ///       cloCri.FieldName = "Loan.DateClosed";
    ///       cloCri.Value = DateTime.Now;
    ///       cloCri.MatchType = OrdinalFieldMatchType.Equals;
    ///       cloCri.Precision = DateFieldMatchPrecision.Year;
    /// 
    ///       StringFieldCriterion staCri = new StringFieldCriterion();
    ///       staCri.FieldName = "Loan.State";
    ///       staCri.Value = "CA";
    ///       staCri.MatchType = StringFieldMatchType.CaseInsensitive;
    /// 
    ///       // Perform the query against the loans
    ///       LoanIdentityList ids = session.Loans.Query(cloCri.And(staCri));
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
    public DateTime Value
    {
      get => this.fieldValue;
      set => this.fieldValue = value;
    }

    /// <summary>
    /// Gets or sets the desired ordinal relation between the specified value and the
    /// value of the field being queried.
    /// </summary>
    /// <example>
    /// The following example selects all Loans which have closed in the current year
    /// where the property is in California.
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
    ///       // Create the date field Criterion for the Loan Closed data
    ///       DateFieldCriterion cloCri = new DateFieldCriterion();
    ///       cloCri.FieldName = "Loan.DateClosed";
    ///       cloCri.Value = DateTime.Now;
    ///       cloCri.MatchType = OrdinalFieldMatchType.Equals;
    ///       cloCri.Precision = DateFieldMatchPrecision.Year;
    /// 
    ///       StringFieldCriterion staCri = new StringFieldCriterion();
    ///       staCri.FieldName = "Loan.State";
    ///       staCri.Value = "CA";
    ///       staCri.MatchType = StringFieldMatchType.CaseInsensitive;
    /// 
    ///       // Perform the query against the loans
    ///       LoanIdentityList ids = session.Loans.Query(cloCri.And(staCri));
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
      get
      {
        return this.fieldValue == DateFieldCriterion.EmptyDate || this.fieldValue == DateFieldCriterion.NonEmptyDate ? OrdinalFieldMatchType.Equals : this.matchType;
      }
      set => this.matchType = value;
    }

    /// <summary>
    /// Gets or sets the precision with which the date comparison is made.
    /// </summary>
    /// <remarks>This field permits dates/times to be compared so that only a
    /// portion of the date is relevant (e.g. to find items which occurred on
    /// the same day or in the same month as the one specified).</remarks>
    /// <example>
    /// The following example selects all Loans which have closed in the current year
    /// where the property is in California.
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
    ///       // Create the date field Criterion for the Loan Closed data
    ///       DateFieldCriterion cloCri = new DateFieldCriterion();
    ///       cloCri.FieldName = "Loan.DateClosed";
    ///       cloCri.Value = DateTime.Now;
    ///       cloCri.MatchType = OrdinalFieldMatchType.Equals;
    ///       cloCri.Precision = DateFieldMatchPrecision.Year;
    /// 
    ///       StringFieldCriterion staCri = new StringFieldCriterion();
    ///       staCri.FieldName = "Loan.State";
    ///       staCri.Value = "CA";
    ///       staCri.MatchType = StringFieldMatchType.CaseInsensitive;
    /// 
    ///       // Perform the query against the loans
    ///       LoanIdentityList ids = session.Loans.Query(cloCri.And(staCri));
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
    public DateFieldMatchPrecision Precision
    {
      get
      {
        return this.fieldValue == DateFieldCriterion.EmptyDate || this.fieldValue == DateFieldCriterion.NonEmptyDate ? DateFieldMatchPrecision.Exact : this.precision;
      }
      set => this.precision = value;
    }

    /// <summary>Sets the criterion to match empty date value.</summary>
    /// <remarks>This method is equivalent to setting the <see cref="P:EllieMae.Encompass.Query.DateFieldCriterion.Value" /> property to the
    /// <see cref="F:EllieMae.Encompass.Query.DateFieldCriterion.EmptyDate" /> value, which is used to represent an empty date field. To
    /// search for non-empty date values, use the <see cref="M:EllieMae.Encompass.Query.IDateFieldCriterion.SetNonEmptyDateValue" /> method.</remarks>
    void IDateFieldCriterion.SetEmptyDateValue()
    {
      this.fieldValue = DateFieldCriterion.EmptyDate;
      this.matchType = OrdinalFieldMatchType.Equals;
    }

    /// <summary>Sets the criterion to match non-empty date value.</summary>
    /// <remarks>This method is equivalent to setting the <see cref="P:EllieMae.Encompass.Query.DateFieldCriterion.Value" /> property to the
    /// <see cref="F:EllieMae.Encompass.Query.DateFieldCriterion.NonEmptyDate" /> value, which is used to represent a non-empty date field. To
    /// search for empty date values, use the <see cref="M:EllieMae.Encompass.Query.IDateFieldCriterion.SetEmptyDateValue" /> method.</remarks>
    void IDateFieldCriterion.SetNonEmptyDateValue()
    {
      this.fieldValue = DateFieldCriterion.NonEmptyDate;
      this.matchType = OrdinalFieldMatchType.Equals;
    }

    /// <summary>
    /// Creates an exact duplicate of the object by performing a deep copy.
    /// </summary>
    /// <returns>Returns a deep copy of the current criterion object.</returns>
    public override QueryCriterion Clone()
    {
      return (QueryCriterion) new DateFieldCriterion(this.FieldName, this.Value, this.MatchType, this.Precision);
    }

    /// <summary>Unwrap the object</summary>
    /// <returns></returns>
    public override EllieMae.EMLite.ClientServer.Query.QueryCriterion Unwrap()
    {
      return (EllieMae.EMLite.ClientServer.Query.QueryCriterion) new DateValueCriterion(this.FieldName, this.Value, (OrdinalMatchType) this.MatchType, (DateMatchPrecision) this.Precision);
    }
  }
}
