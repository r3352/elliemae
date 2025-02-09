// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Reporting.Reports
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.Collections;
using EllieMae.Encompass.Query;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace EllieMae.Encompass.Reporting
{
  /// <summary>
  /// Provides an interface for running reports against the Loans in Encompass.
  /// </summary>
  /// <example>
  ///       The following code demonstrates opening a <see>LoanReportCursor</see> to
  ///       retrieve all loans that match a specified set of criteria. In the example, a very
  ///       small set of field are retrieved for each loan, maximizing the efficiency of the call.
  ///       Additionally, the code demonstrates how to properly include fields from the
  ///       Reporting Database into your report, either in the list of selected fields, as
  ///       part of the query filter or even within the sort order of the returned data set.
  ///       <code>
  /// <![CDATA[
  /// using System;
  /// using System.IO;
  /// using EllieMae.Encompass.Client;
  /// using EllieMae.Encompass.Reporting;
  /// using EllieMae.Encompass.Collections;
  /// using EllieMae.Encompass.Query;
  /// 
  /// class ReportExample
  /// {
  ///     public static void Main()
  ///     {
  ///         // Open the session to the remote server
  ///         Session session = new Session();
  ///         session.Start("myserver", "mary", "maryspwd");
  /// 
  ///         // Load a list with the fields to be selected
  ///         StringList fields = new StringList();
  ///         fields.Add("Loan.BorrowerLastName");
  ///         fields.Add("Loan.LoanNumber");
  ///         fields.Add("Loan.LoanOfficerID");
  /// 
  ///         // You can include a field from the reporting database, if you have
  ///         // previously configured the database to include this field. In this example,
  ///         // we will assume that field 1014 has been previously added to the reporting
  ///         // database.
  ///         string qualRateFieldName = Reports.ReportingDatabaseCanonicalPrefix + "1014";
  ///         fields.Add(qualRateFieldName);
  /// 
  ///         // Now build the criteria for the selection of the loans.
  ///         // This criteria can also include reporting database fields.
  ///         NumericFieldCriterion laCri = new NumericFieldCriterion();
  ///         laCri.FieldName = "Loan.LoanAmount";
  ///         laCri.Value = 100000;
  ///         laCri.MatchType = OrdinalFieldMatchType.GreaterThanOrEquals;
  /// 
  ///         // Our second criterion uses the "Gross Rent" field (field 1005), which we will
  ///         // assume has previously been added to the reporting database.
  ///         NumericFieldCriterion rentCri = new NumericFieldCriterion();
  ///         rentCri.FieldName = Reports.ReportingDatabaseCanonicalPrefix + "1005";
  ///         rentCri.Value = 800;
  ///         rentCri.MatchType = OrdinalFieldMatchType.LessThan;
  /// 
  ///         // Combine into a single criterion
  ///         QueryCriterion queryCri = laCri.And(rentCri);
  /// 
  ///         // Now build the sort order for the result set, which we'll base on the
  ///         // borrower's last name. Again, we could use fields from the reporting database
  ///         // here just as we did above.
  ///         SortCriterionList sortOrder = new SortCriterionList();
  ///         sortOrder.Add(new SortCriterion("Loan.BorrowerLastName"));
  /// 
  ///         // Open a new cursor with the result set
  ///         LoanReportCursor cur = session.Reports.OpenReportCursor(fields, queryCri, sortOrder);
  ///         // LoanReportCursor cur = session.Reports.OpenReportCursor(true, fields, queryCri, sortOrder); // Pull data from External Reporting Database (ERDB)
  /// 
  ///         // Iterate over the results, displaying the relevant field values
  ///         foreach (LoanReportData data in cur)
  ///         {
  ///             Console.WriteLine("Results for loan " + data.Guid + ":");
  ///             Console.WriteLine("  Borrower Last Name = " + data["Loan.BorrowerLastName"]);
  ///             Console.WriteLine("  Loan Number = " + data["Loan.LoanNumber"]);
  ///             Console.WriteLine("  Loan Officer = " + data["Loan.LoanOfficerID"]);
  ///             Console.WriteLine("  Qual Rate = " + data[qualRateFieldName]);
  ///         }
  ///     }
  /// }
  /// ]]>
  ///           </code>
  ///     </example>
  public class Reports : SessionBoundObject, IReports
  {
    /// <summary>
    /// Provides the prefix for a canonical field name that references a field in the
    /// reporting database.
    /// </summary>
    public const string ReportingDatabaseCanonicalPrefix = "Fields.�";
    private Session session;

    internal Reports(Session session)
      : base(session)
    {
      this.session = session;
    }

    /// <summary>
    /// Executes a query against the list of loans an opens a report cursor with the results.
    /// </summary>
    /// <param name="fieldsToRetrieve">A <see cref="T:EllieMae.Encompass.Collections.StringList" /> containing the list of canonical
    /// field names to be retrieved from the server.</param>
    /// <param name="filter">A <see cref="T:EllieMae.Encompass.Query.QueryCriterion" /> defining the filter, if any, to be applied
    /// to the query.</param>
    /// <param name="sortOrder">An option <see cref="T:EllieMae.Encompass.Collections.SortCriterionList" /> specifying the order in
    /// which the results should be returned.</param>
    /// <returns>
    /// Returns a <see cref="T:EllieMae.Encompass.Reporting.LoanReportCursor" /> containing the results for the specified query.
    /// </returns>
    /// <remarks>
    /// <p>This method provides a very efficient means of retrieving a fixed set of field values from
    /// an user-defined set of loans. The list of fields specified in the <c>fieldsToRetrieve</c>
    /// parameter must use the Canonical Field Name format defined in the <i>SDK Programmer's Guide.</i>
    /// As a result, you can only retrieve field values which are represented by one of the
    /// pre-defined canonical field names or which you have added into the Reporting Database feature
    /// of Encompass.</p>
    /// <p>For more information on the use of this method, see the "Advanced Reporting" section of
    /// the Encompass SDK Programmer's Guide.</p>
    /// </remarks>
    /// <example>
    ///       The following code demonstrates opening a <see>LoanReportCursor</see> to
    ///       retrieve all loans that match a specified set of criteria. In the example, a very
    ///       small set of field are retrieved for each loan, maximizing the efficiency of the call.
    ///       Additionally, the code demonstrates how to properly include fields from the
    ///       Reporting Database into your report, either in the list of selected fields, as
    ///       part of the query filter or even within the sort order of the returned data set.
    ///       <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.Reporting;
    /// using EllieMae.Encompass.Collections;
    /// using EllieMae.Encompass.Query;
    /// 
    /// class ReportExample
    /// {
    ///     public static void Main()
    ///     {
    ///         // Open the session to the remote server
    ///         Session session = new Session();
    ///         session.Start("myserver", "mary", "maryspwd");
    /// 
    ///         // Load a list with the fields to be selected
    ///         StringList fields = new StringList();
    ///         fields.Add("Loan.BorrowerLastName");
    ///         fields.Add("Loan.LoanNumber");
    ///         fields.Add("Loan.LoanOfficerID");
    /// 
    ///         // You can include a field from the reporting database, if you have
    ///         // previously configured the database to include this field. In this example,
    ///         // we will assume that field 1014 has been previously added to the reporting
    ///         // database.
    ///         string qualRateFieldName = Reports.ReportingDatabaseCanonicalPrefix + "1014";
    ///         fields.Add(qualRateFieldName);
    /// 
    ///         // Now build the criteria for the selection of the loans.
    ///         // This criteria can also include reporting database fields.
    ///         NumericFieldCriterion laCri = new NumericFieldCriterion();
    ///         laCri.FieldName = "Loan.LoanAmount";
    ///         laCri.Value = 100000;
    ///         laCri.MatchType = OrdinalFieldMatchType.GreaterThanOrEquals;
    /// 
    ///         // Our second criterion uses the "Gross Rent" field (field 1005), which we will
    ///         // assume has previously been added to the reporting database.
    ///         NumericFieldCriterion rentCri = new NumericFieldCriterion();
    ///         rentCri.FieldName = Reports.ReportingDatabaseCanonicalPrefix + "1005";
    ///         rentCri.Value = 800;
    ///         rentCri.MatchType = OrdinalFieldMatchType.LessThan;
    /// 
    ///         // Combine into a single criterion
    ///         QueryCriterion queryCri = laCri.And(rentCri);
    /// 
    ///         // Now build the sort order for the result set, which we'll base on the
    ///         // borrower's last name. Again, we could use fields from the reporting database
    ///         // here just as we did above.
    ///         SortCriterionList sortOrder = new SortCriterionList();
    ///         sortOrder.Add(new SortCriterion("Loan.BorrowerLastName"));
    /// 
    ///         // Open a new cursor with the result set
    ///         LoanReportCursor cur = session.Reports.OpenReportCursor(fields, queryCri, sortOrder);
    ///         // LoanReportCursor cur = session.Reports.OpenReportCursor(true, fields, queryCri, sortOrder); // Pull data from External Reporting Database (ERDB)
    /// 
    ///         // Iterate over the results, displaying the relevant field values
    ///         foreach (LoanReportData data in cur)
    ///         {
    ///             Console.WriteLine("Results for loan " + data.Guid + ":");
    ///             Console.WriteLine("  Borrower Last Name = " + data["Loan.BorrowerLastName"]);
    ///             Console.WriteLine("  Loan Number = " + data["Loan.LoanNumber"]);
    ///             Console.WriteLine("  Loan Officer = " + data["Loan.LoanOfficerID"]);
    ///             Console.WriteLine("  Qual Rate = " + data[qualRateFieldName]);
    ///         }
    ///     }
    /// }
    /// ]]>
    ///           </code>
    ///     </example>
    public LoanReportCursor OpenReportCursor(
      StringList fieldsToRetrieve,
      EllieMae.Encompass.Query.QueryCriterion filter,
      SortCriterionList sortOrder)
    {
      return this.OpenReportCursor(false, fieldsToRetrieve, filter, sortOrder);
    }

    /// <summary>
    /// Executes a query against the list of loans an opens a report cursor with the results.
    /// </summary>
    /// <param name="fromERDB">A boolean flag indicating if the data should be pulled from the External Reporting Database (ERDB).</param>
    /// <param name="fieldsToRetrieve">A <see cref="T:EllieMae.Encompass.Collections.StringList" /> containing the list of canonical
    /// field names to be retrieved from the server.</param>
    /// <param name="filter">A <see cref="T:EllieMae.Encompass.Query.QueryCriterion" /> defining the filter, if any, to be applied
    /// to the query.</param>
    /// <param name="sortOrder">An option <see cref="T:EllieMae.Encompass.Collections.SortCriterionList" /> specifying the order in
    /// which the results should be returned.</param>
    /// <returns>
    /// Returns a <see cref="T:EllieMae.Encompass.Reporting.LoanReportCursor" /> containing the results for the specified query.
    /// </returns>
    /// <remarks>
    /// <p>This method provides a very efficient means of retrieving a fixed set of field values from
    /// an user-defined set of loans. The list of fields specified in the <c>fieldsToRetrieve</c>
    /// parameter must use the Canonical Field Name format defined in the <i>SDK Programmer's Guide.</i>
    /// As a result, you can only retrieve field values which are represented by one of the
    /// pre-defined canonical field names or which you have added into the Reporting Database feature
    /// of Encompass.</p>
    /// <p>For more information on the use of this method, see the "Advanced Reporting" section of
    /// the Encompass SDK Programmer's Guide.</p>
    /// </remarks>
    /// <example>
    ///       The following code demonstrates opening a <see>LoanReportCursor</see> to
    ///       retrieve all loans that match a specified set of criteria. In the example, a very
    ///       small set of field are retrieved for each loan, maximizing the efficiency of the call.
    ///       Additionally, the code demonstrates how to properly include fields from the
    ///       Reporting Database into your report, either in the list of selected fields, as
    ///       part of the query filter or even within the sort order of the returned data set.
    ///       <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.Reporting;
    /// using EllieMae.Encompass.Collections;
    /// using EllieMae.Encompass.Query;
    /// 
    /// class ReportExample
    /// {
    ///     public static void Main()
    ///     {
    ///         // Open the session to the remote server
    ///         Session session = new Session();
    ///         session.Start("myserver", "mary", "maryspwd");
    /// 
    ///         // Load a list with the fields to be selected
    ///         StringList fields = new StringList();
    ///         fields.Add("Loan.BorrowerLastName");
    ///         fields.Add("Loan.LoanNumber");
    ///         fields.Add("Loan.LoanOfficerID");
    /// 
    ///         // You can include a field from the reporting database, if you have
    ///         // previously configured the database to include this field. In this example,
    ///         // we will assume that field 1014 has been previously added to the reporting
    ///         // database.
    ///         string qualRateFieldName = Reports.ReportingDatabaseCanonicalPrefix + "1014";
    ///         fields.Add(qualRateFieldName);
    /// 
    ///         // Now build the criteria for the selection of the loans.
    ///         // This criteria can also include reporting database fields.
    ///         NumericFieldCriterion laCri = new NumericFieldCriterion();
    ///         laCri.FieldName = "Loan.LoanAmount";
    ///         laCri.Value = 100000;
    ///         laCri.MatchType = OrdinalFieldMatchType.GreaterThanOrEquals;
    /// 
    ///         // Our second criterion uses the "Gross Rent" field (field 1005), which we will
    ///         // assume has previously been added to the reporting database.
    ///         NumericFieldCriterion rentCri = new NumericFieldCriterion();
    ///         rentCri.FieldName = Reports.ReportingDatabaseCanonicalPrefix + "1005";
    ///         rentCri.Value = 800;
    ///         rentCri.MatchType = OrdinalFieldMatchType.LessThan;
    /// 
    ///         // Combine into a single criterion
    ///         QueryCriterion queryCri = laCri.And(rentCri);
    /// 
    ///         // Now build the sort order for the result set, which we'll base on the
    ///         // borrower's last name. Again, we could use fields from the reporting database
    ///         // here just as we did above.
    ///         SortCriterionList sortOrder = new SortCriterionList();
    ///         sortOrder.Add(new SortCriterion("Loan.BorrowerLastName"));
    /// 
    ///         // Open a new cursor with the result set
    ///         LoanReportCursor cur = session.Reports.OpenReportCursor(fields, queryCri, sortOrder);
    ///         // LoanReportCursor cur = session.Reports.OpenReportCursor(true, fields, queryCri, sortOrder); // Pull data from External Reporting Database (ERDB)
    /// 
    ///         // Iterate over the results, displaying the relevant field values
    ///         foreach (LoanReportData data in cur)
    ///         {
    ///             Console.WriteLine("Results for loan " + data.Guid + ":");
    ///             Console.WriteLine("  Borrower Last Name = " + data["Loan.BorrowerLastName"]);
    ///             Console.WriteLine("  Loan Number = " + data["Loan.LoanNumber"]);
    ///             Console.WriteLine("  Loan Officer = " + data["Loan.LoanOfficerID"]);
    ///             Console.WriteLine("  Qual Rate = " + data[qualRateFieldName]);
    ///         }
    ///     }
    /// }
    /// ]]>
    ///           </code>
    ///     </example>
    public LoanReportCursor OpenReportCursor(
      bool fromERDB,
      StringList fieldsToRetrieve,
      EllieMae.Encompass.Query.QueryCriterion filter,
      SortCriterionList sortOrder)
    {
      DataQuery query = new DataQuery((IEnumerable) fieldsToRetrieve);
      DataField dataField = new DataField("Loan.Guid");
      if (!query.Selections.Contains((IQueryTerm) dataField))
        query.Selections.Add((IQueryTerm) dataField);
      if (filter != null)
        query.Filter = filter.Unwrap();
      if (sortOrder != null)
      {
        foreach (SortCriterion sortCriterion in (CollectionBase) sortOrder)
          query.SortFields.Add(sortCriterion.Unwrap());
      }
      string fileName = Path.GetFileName(Environment.GetCommandLineArgs()[0]);
      return new LoanReportCursor(this.session, this.session.SessionObjects.LoanManager.OpenQuery(query, fromERDB, fileName, false));
    }

    /// <summary>
    /// Executes a query against the list of loans an opens a report cursor with the results.
    /// </summary>
    /// <param name="fieldsToRetrieve">A <see cref="T:EllieMae.Encompass.Collections.StringList" /> containing the list of canonical
    /// field names to be retrieved from the server.</param>
    /// <param name="filter">A <see cref="T:EllieMae.Encompass.Query.QueryCriterion" /> defining the filter, if any, to be applied
    /// to the query.</param>
    /// <returns>
    /// Returns a <see cref="T:EllieMae.Encompass.Reporting.LoanReportCursor" /> containing the results for the specified query.
    /// </returns>
    /// <remarks>
    /// <p>For information on how to use this method, see <see cref="M:EllieMae.Encompass.Reporting.Reports.OpenReportCursor(EllieMae.Encompass.Collections.StringList,EllieMae.Encompass.Query.QueryCriterion,EllieMae.Encompass.Collections.SortCriterionList)" />.</p>
    /// </remarks>
    /// <example>
    ///       The following code demonstrates opening a <see>LoanReportCursor</see> to
    ///       retrieve all loans that match a specified set of criteria. In the example, a very
    ///       small set of field are retrieved for each loan, maximizing the efficiency of the call.
    ///       Additionally, the code demonstrates how to properly include fields from the
    ///       Reporting Database into your report, either in the list of selected fields, as
    ///       part of the query filter or even within the sort order of the returned data set.
    ///       <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.Reporting;
    /// using EllieMae.Encompass.Collections;
    /// using EllieMae.Encompass.Query;
    /// 
    /// class ReportExample
    /// {
    ///     public static void Main()
    ///     {
    ///         // Open the session to the remote server
    ///         Session session = new Session();
    ///         session.Start("myserver", "mary", "maryspwd");
    /// 
    ///         // Load a list with the fields to be selected
    ///         StringList fields = new StringList();
    ///         fields.Add("Loan.BorrowerLastName");
    ///         fields.Add("Loan.LoanNumber");
    ///         fields.Add("Loan.LoanOfficerID");
    /// 
    ///         // You can include a field from the reporting database, if you have
    ///         // previously configured the database to include this field. In this example,
    ///         // we will assume that field 1014 has been previously added to the reporting
    ///         // database.
    ///         string qualRateFieldName = Reports.ReportingDatabaseCanonicalPrefix + "1014";
    ///         fields.Add(qualRateFieldName);
    /// 
    ///         // Now build the criteria for the selection of the loans.
    ///         // This criteria can also include reporting database fields.
    ///         NumericFieldCriterion laCri = new NumericFieldCriterion();
    ///         laCri.FieldName = "Loan.LoanAmount";
    ///         laCri.Value = 100000;
    ///         laCri.MatchType = OrdinalFieldMatchType.GreaterThanOrEquals;
    /// 
    ///         // Our second criterion uses the "Gross Rent" field (field 1005), which we will
    ///         // assume has previously been added to the reporting database.
    ///         NumericFieldCriterion rentCri = new NumericFieldCriterion();
    ///         rentCri.FieldName = Reports.ReportingDatabaseCanonicalPrefix + "1005";
    ///         rentCri.Value = 800;
    ///         rentCri.MatchType = OrdinalFieldMatchType.LessThan;
    /// 
    ///         // Combine into a single criterion
    ///         QueryCriterion queryCri = laCri.And(rentCri);
    /// 
    ///         // Now build the sort order for the result set, which we'll base on the
    ///         // borrower's last name. Again, we could use fields from the reporting database
    ///         // here just as we did above.
    ///         SortCriterionList sortOrder = new SortCriterionList();
    ///         sortOrder.Add(new SortCriterion("Loan.BorrowerLastName"));
    /// 
    ///         // Open a new cursor with the result set
    ///         LoanReportCursor cur = session.Reports.OpenReportCursor(fields, queryCri, sortOrder);
    ///         // LoanReportCursor cur = session.Reports.OpenReportCursor(true, fields, queryCri, sortOrder); // Pull data from External Reporting Database (ERDB)
    /// 
    ///         // Iterate over the results, displaying the relevant field values
    ///         foreach (LoanReportData data in cur)
    ///         {
    ///             Console.WriteLine("Results for loan " + data.Guid + ":");
    ///             Console.WriteLine("  Borrower Last Name = " + data["Loan.BorrowerLastName"]);
    ///             Console.WriteLine("  Loan Number = " + data["Loan.LoanNumber"]);
    ///             Console.WriteLine("  Loan Officer = " + data["Loan.LoanOfficerID"]);
    ///             Console.WriteLine("  Qual Rate = " + data[qualRateFieldName]);
    ///         }
    ///     }
    /// }
    /// ]]>
    ///           </code>
    ///     </example>
    public LoanReportCursor OpenReportCursor(StringList fieldsToRetrieve, EllieMae.Encompass.Query.QueryCriterion filter)
    {
      return this.OpenReportCursor(fieldsToRetrieve, filter, (SortCriterionList) null);
    }

    /// <summary>
    /// Retrieves the values of a specific set of fields for one or more loans.
    /// </summary>
    /// <param name="loanGuids">The list of GUIDs for the selected set of loans.</param>
    /// <param name="fieldsToRetrieve">The list of canonical fields names to be retrieved from the server.</param>
    /// <returns>Returns a <see cref="T:EllieMae.Encompass.Collections.LoanReportDataList" /> containing the field values for the
    /// specified loans.</returns>
    /// <remarks>
    /// <p>The order of the result set matches the order of the loan GUIDs specified in the
    /// <c>loanGuids</c> parameter.</p>
    /// <p>The list of fields specified in the <c>fieldsToRetrieve</c>
    /// parameter must use the Canonical Field Name format defined in the <i>SDK Programmer's Guide.</i>
    /// As a result, you can only retrieve field values which are represented by one of the
    /// pre-defined canonical field names or which you have added into the Reporting Database feature
    /// of Encompass.</p>
    /// <p>Note that if the number of GUIDs passed to this function becomes large, then it will become
    /// increasingly inefficient. Instead, use the <see cref="M:EllieMae.Encompass.Reporting.Reports.OpenReportCursor(EllieMae.Encompass.Collections.StringList,EllieMae.Encompass.Query.QueryCriterion,EllieMae.Encompass.Collections.SortCriterionList)" /> method for a more
    /// efficient means to selecting field values for a large set of loans.</p>
    /// </remarks>
    /// <example>
    ///       The following code retrieves a set of database fields for a pre-defined
    ///       set of loans. In this example, we include a field from the Reporting
    ///       Database to demonstrate how the reporting can be extended beyond the default
    ///       fields provided by Encompas.
    ///       <code>
    ///         <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.Reporting;
    /// using EllieMae.Encompass.Collections;
    /// using EllieMae.Encompass.Query;
    /// 
    /// class ReportExample
    /// {
    ///     public static void Main()
    ///     {
    ///         // Open the session to the remote server
    ///         Session session = new Session();
    ///         session.Start("myserver", "mary", "maryspwd");
    /// 
    /// 				// Create the list of loan Guids
    /// 				StringList guids = new StringList();
    /// 				guids.Add("{ec0d2d36-a824-4f48-87a1-4c0ab410d545}");
    /// 				guids.Add("{8616a90b-8fdc-43de-aea6-05167a332e82}");
    /// 				guids.Add("{9e28208a-010f-4d88-b10f-da508b22db34}");
    /// 
    /// 				// Load a list with the fields to be selected
    /// 				StringList fields = new StringList();
    /// 				fields.Add("Loan.BorrowerLastName");
    /// 				fields.Add("Loan.LoanNumber");
    /// 				fields.Add("Loan.LoanOfficerID");
    /// 
    /// 				// You can include a field from the reporting database, if you have
    /// 				// previously configured the database to include this field. In this example,
    /// 				// we will assume that field 1014 has been previously added to the reporting
    /// 				// database.
    /// 				string qualRateFieldName = Reports.ReportingDatabaseCanonicalPrefix + "1014";
    /// 				fields.Add(qualRateFieldName);
    /// 
    /// 				// Open a new cursor with the result set
    /// 				LoanReportDataList dataList = session.Reports.SelectReportingFieldsForLoans(guids, fields);
    /// 
    /// 				// Iterate over the results, displaying the relevant field values. Note that
    /// 				// the results are returned in the order that the GUIDs were specified
    /// 				// in the guids collection.
    /// 				foreach (LoanReportData data in dataList)
    /// 				{
    /// 					Console.WriteLine("Results for loan " + data.Guid + ":");
    /// 					Console.WriteLine("  Borrower Last Name = " + data["Loan.BorrowerLastName"]);
    /// 					Console.WriteLine("  Loan Number = " + data["Loan.LoanNumber"]);
    /// 					Console.WriteLine("  Loan Officer = " + data["Loan.LoanOfficerID"]);
    /// 					Console.WriteLine("  Qual Rate = " + data[qualRateFieldName]);
    /// 				}
    ///     }
    /// }
    /// ]]>
    ///       </code>
    ///     </example>
    public LoanReportDataList SelectReportingFieldsForLoans(
      StringList loanGuids,
      StringList fieldsToRetrieve)
    {
      DataQuery query = new DataQuery((IEnumerable) fieldsToRetrieve);
      DataField dataField = new DataField("Loan.Guid");
      if (!query.Selections.Contains((IQueryTerm) dataField))
        query.Selections.Add((IQueryTerm) dataField);
      query.Filter = (EllieMae.EMLite.ClientServer.Query.QueryCriterion) new ListValueCriterion("Loan.Guid", (Array) loanGuids.ToArray());
      QueryResult queryResult = this.session.SessionObjects.LoanManager.QueryPipeline(query, false);
      Dictionary<string, List<LoanReportData>> dictionary = new Dictionary<string, List<LoanReportData>>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
      for (int index = 0; index < queryResult.RecordCount; ++index)
      {
        string key = string.Concat(queryResult[index, "Loan.Guid"]);
        if (!dictionary.ContainsKey(key))
          dictionary[key] = new List<LoanReportData>();
        dictionary[key].Add(new LoanReportData(queryResult.Columns, queryResult.GetRow(index)));
      }
      LoanReportDataList loanReportDataList = new LoanReportDataList();
      for (int index = 0; index < loanGuids.Count; ++index)
      {
        if (dictionary.ContainsKey(loanGuids[index]))
          loanReportDataList.AddRange((ICollection) dictionary[loanGuids[index]]);
      }
      return loanReportDataList;
    }

    /// <summary>Retrieves a set of field values for a single loan.</summary>
    /// <param name="loanGuid">The GUID of the specified loan.</param>
    /// <param name="fieldsToRetrieve">The list of canonical field names to be retrieved.</param>
    /// <returns>
    /// Returns a <see cref="T:EllieMae.Encompass.Reporting.LoanReportData" /> object containing the field values specified or
    /// <c>null</c> if no such loan exists.
    /// </returns>
    /// <example>
    ///       The following code retrieves a set of database fields for a pre-defined
    ///       set of loans. In this example, we include a field from the Reporting
    ///       Database to demonstrate how the reporting can be extended beyond the default
    ///       fields provided by Encompas.
    ///       <code>
    ///         <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.Reporting;
    /// using EllieMae.Encompass.Collections;
    /// using EllieMae.Encompass.Query;
    /// 
    /// class ReportExample
    /// {
    ///     public static void Main()
    ///     {
    ///         // Open the session to the remote server
    ///         Session session = new Session();
    ///         session.Start("myserver", "mary", "maryspwd");
    /// 
    /// 				// Create the list of loan Guids
    /// 				StringList guids = new StringList();
    /// 				guids.Add("{ec0d2d36-a824-4f48-87a1-4c0ab410d545}");
    /// 				guids.Add("{8616a90b-8fdc-43de-aea6-05167a332e82}");
    /// 				guids.Add("{9e28208a-010f-4d88-b10f-da508b22db34}");
    /// 
    /// 				// Load a list with the fields to be selected
    /// 				StringList fields = new StringList();
    /// 				fields.Add("Loan.BorrowerLastName");
    /// 				fields.Add("Loan.LoanNumber");
    /// 				fields.Add("Loan.LoanOfficerID");
    /// 
    /// 				// You can include a field from the reporting database, if you have
    /// 				// previously configured the database to include this field. In this example,
    /// 				// we will assume that field 1014 has been previously added to the reporting
    /// 				// database.
    /// 				string qualRateFieldName = Reports.ReportingDatabaseCanonicalPrefix + "1014";
    /// 				fields.Add(qualRateFieldName);
    /// 
    /// 				// Open a new cursor with the result set
    /// 				LoanReportDataList dataList = session.Reports.SelectReportingFieldsForLoans(guids, fields);
    /// 
    /// 				// Iterate over the results, displaying the relevant field values. Note that
    /// 				// the results are returned in the order that the GUIDs were specified
    /// 				// in the guids collection.
    /// 				foreach (LoanReportData data in dataList)
    /// 				{
    /// 					Console.WriteLine("Results for loan " + data.Guid + ":");
    /// 					Console.WriteLine("  Borrower Last Name = " + data["Loan.BorrowerLastName"]);
    /// 					Console.WriteLine("  Loan Number = " + data["Loan.LoanNumber"]);
    /// 					Console.WriteLine("  Loan Officer = " + data["Loan.LoanOfficerID"]);
    /// 					Console.WriteLine("  Qual Rate = " + data[qualRateFieldName]);
    /// 				}
    ///     }
    /// }
    /// ]]>
    ///       </code>
    ///     </example>
    public LoanReportData SelectReportingFieldsForLoan(string loanGuid, StringList fieldsToRetrieve)
    {
      LoanReportDataList loanReportDataList = this.SelectReportingFieldsForLoans(new StringList()
      {
        loanGuid
      }, fieldsToRetrieve);
      return loanReportDataList.Count > 0 ? loanReportDataList[0] : (LoanReportData) null;
    }

    /// <summary>Returns the list of fields in the Reporting Database.</summary>
    public ReportingFieldDescriptorList GetReportingDatabaseFields()
    {
      return this.GetReportingDatabaseFields(false);
    }

    /// <summary>Returns the list of fields in the Reporting Database.</summary>
    /// <param name="useERDB">Indicates if the list of fields should be pulled from the External
    /// Reporting Database.</param>
    public ReportingFieldDescriptorList GetReportingDatabaseFields(bool useERDB)
    {
      LoanXDBTableList loanXdbTableList = this.Session.SessionObjects.LoanManager.GetLoanXDBTableList(useERDB);
      ReportingFieldDescriptorList reportingDatabaseFields = new ReportingFieldDescriptorList();
      foreach (LoanXDBField allField in loanXdbTableList.GetAllFields())
        reportingDatabaseFields.Add(new ReportingFieldDescriptor(allField));
      return reportingDatabaseFields;
    }
  }
}
