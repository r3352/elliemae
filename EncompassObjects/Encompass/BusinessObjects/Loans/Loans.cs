// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Loans
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using Elli.Metrics.Client;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.DataEngine;
using EllieMae.Encompass.BusinessEnums;
using EllieMae.Encompass.BusinessObjects.Users;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.Collections;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>
  /// Provides an interface for accessing the loans and loan folders defined on
  /// the Encompass server.
  /// </summary>
  /// <example>
  /// The following code opens a session to a remote Encompass Server using a TCP/IP
  /// connection on port 11081. It then opens a loan using the specified GUID value.
  /// <code>
  /// <![CDATA[
  /// using System;
  /// using System.IO;
  /// using EllieMae.Encompass.Client;
  /// using EllieMae.Encompass.BusinessObjects;
  /// using EllieMae.Encompass.BusinessObjects.Loans;
  /// 
  /// class LoanReader
  /// {
  ///    public static void Main()
  ///    {
  ///       // Open the session to the remote server
  ///       Session session = new Session();
  ///       session.Start("myserver", "mary", "maryspwd");
  /// 
  ///       // Fetch a loan from the session
  ///       Loan loan = session.Loans.Open("{9885d88a-78af-44a7-977d-5d5fd6e41a96}");
  /// 
  ///       if (loan == null)
  ///          Console.WriteLine("Loan not found");
  ///       else
  ///          Console.WriteLine("Successfully opened loan " + loan.LoanNumber);
  /// 
  ///       // End the session to gracefully disconnect from the server
  ///       session.End();
  ///    }
  /// }
  /// ]]>
  /// </code>
  /// </example>
  public class Loans : SessionBoundObject, ILoans
  {
    private ILoanManager mngr;
    private LoanFolders folders;
    private Milestones milestones;
    private MilestoneTemplates milestoneTemplates;
    private EllieMae.Encompass.BusinessObjects.Loans.Templates.Templates templates;
    private LoanFieldDescriptors fieldDescriptors;
    private Roles roles;
    private AdjustableRateTypes rateTypes;
    private bool canAccessArchiveLoans;
    private bool loanSoftArchivalenabled;

    internal Loans(Session session)
      : base(session)
    {
      this.mngr = (ILoanManager) session.GetObject(nameof (LoanManager));
      this.canAccessArchiveLoans = this.Session.GetUserInfo().IsAdministrator() || (bool) session.SessionObjects.StartupInfo.UserAclFeatureRights[(object) AclFeature.LoanMgmt_AccessToArchiveLoans];
      this.loanSoftArchivalenabled = this.Session.SessionObjects.StartupInfo.EnableLoanSoftArchival;
    }

    /// <summary>Create a new BatchReassign object</summary>
    /// <returns></returns>
    public BatchReassign CreateBatchReassign() => new BatchReassign(this.Session);

    /// <summary>
    /// Opens a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Loan">Loan</see> using the specified globally unique identifier.
    /// </summary>
    /// <param name="guid">The globally unique identifier of the loan to be opened.</param>
    /// <returns>A reference to the requested Loan, or null if the specified
    /// loan cannot be found.</returns>
    /// <example>
    /// The following code opens a session to a remote Encompass Server using a TCP/IP
    /// connection on port 11081. It then opens a loan using the specified GUID value.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// 
    /// class LoanReader
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Fetch a loan from the session
    ///       Loan loan = session.Loans.Open("{9885d88a-78af-44a7-977d-5d5fd6e41a96}");
    /// 
    ///       if (loan == null)
    ///          Console.WriteLine("Loan not found");
    ///       else
    ///          Console.WriteLine("Successfully opened loan " + loan.LoanNumber);
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public Loan Open(string guid)
    {
      try
      {
        ClientMetricsProviderFactory.IncrementCounter("LoanOpenIncCounter", (SFxTag) new SFxSdkTag());
        using (ClientMetricsProviderFactory.GetIncrementalTimer("LoanOpenIncTimer", (SFxTag) new SFxSdkTag()))
          return new Loan(this.Session, LoanDataMgr.OpenLoan(this.Session.SessionObjects, guid ?? "", false));
      }
      catch (ObjectNotFoundException ex)
      {
        return (Loan) null;
      }
    }

    /// <summary>
    /// Opens a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Loan">Loan</see> using the specified globally unique identifier.
    /// </summary>
    /// <param name="guid">The globally unique identifier of the loan to be opened.</param>
    /// <param name="loanlock">boolean parameter to lock loan on open </param>
    /// <param name="exclusive">boolean parameter to apply exclusive lock, this parameter is meaning less if loanlock param is false </param>
    /// <returns>A reference to the requested Loan, or null if the specified
    /// loan cannot be found.</returns>
    /// <example>
    ///       The following code opens a session to a remote Encompass Server using a TCP/IP
    ///       connection on port 11081. It then opens a loan using the specified GUID value with loanlock as false and
    ///       execlusive flag as false.
    ///       <code>
    ///         <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// 
    /// class LoanReader
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Fetch a loan from the session with loanlock=false , exclusive=false
    ///       Loan loan = session.Loans.Open("{DA06DD0B-BB48-45D1-A7AA-9D1543D10FF5}",false,false);
    /// 
    ///       if (loan == null)
    ///          Console.WriteLine("Loan not found");
    ///       else
    ///          Console.WriteLine("Successfully opened loan " + loan.LoanNumber);
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    ///       </code>
    ///     </example>
    public Loan Open(string guid, bool loanlock, bool exclusive)
    {
      try
      {
        ClientMetricsProviderFactory.IncrementCounter("LoanOpenIncCounter", (SFxTag) new SFxSdkTag());
        using (ClientMetricsProviderFactory.GetIncrementalTimer("LoanOpenIncTimer", (SFxTag) new SFxSdkTag()))
        {
          if (!loanlock)
            return new Loan(this.Session, LoanDataMgr.OpenLoan(this.Session.SessionObjects, guid ?? "", false));
          lock (this)
          {
            try
            {
              LoanDataMgr.ImmediateExclusiveLockType immediateLockType = exclusive ? LoanDataMgr.ImmediateExclusiveLockType.Exclusive : LoanDataMgr.ImmediateExclusiveLockType.NonExclusive;
              return new Loan(this.Session, LoanDataMgr.OpenLoan(this.Session.SessionObjects, guid ?? "", false, 0, immediateLockType: immediateLockType));
            }
            catch (EllieMae.EMLite.ClientServer.Exceptions.LockException ex)
            {
              throw new LockException(ex);
            }
          }
        }
      }
      catch (ObjectNotFoundException ex)
      {
        return (Loan) null;
      }
      catch (LockException ex)
      {
        throw ex;
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    /// <summary>
    /// Performs a query on the set of all loans to which the current user has access
    /// rights.
    /// </summary>
    /// <param name="criterion">The query criterion (or criteria) used to determine
    /// the set of loans to be returned.</param>
    /// <returns>A LoanIdentityList containing the
    /// <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.LoanIdentity">LoanIdentities</see>
    /// of the matching loans. If no loans matched the specified criteria, the list is empty.
    /// </returns>
    /// <remarks>If the currently logged in user does not have administrative rights,
    /// the Query method will only return loans to which the current user or his
    /// subordinates (based on the organization chart) have explicitly defined access rights.
    /// Because users with the Administrator persona can access all loans, admin users will
    /// query against the entire set of loans on the server.
    /// </remarks>
    /// <example>
    /// The following code queries the server for all loans which have a loan amount
    /// of at least $200,000 for the purpose of an initial purchase.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.Query;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// 
    /// class LoanReader
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Build the LoanAmount criterion (>= $200,000)
    ///       NumericFieldCriterion amtCriterion = new NumericFieldCriterion();
    ///       amtCriterion.FieldName = "Loan.LoanAmount";
    ///       amtCriterion.Value = 200000;
    ///       amtCriterion.MatchType = OrdinalFieldMatchType.GreaterThanOrEquals;
    /// 
    ///       // Build the LoanPurpose criterion
    ///       StringFieldCriterion purposeCriterion = new StringFieldCriterion();
    ///       purposeCriterion.FieldName = "Loan.LoanPurpose";
    ///       purposeCriterion.Value = "Purchase";
    ///       purposeCriterion.MatchType = StringFieldMatchType.Exact;
    /// 
    ///       // Join the criteria together using AND logic
    ///       QueryCriterion jointCriteria = amtCriterion.And(purposeCriterion);
    /// 
    ///       // Perform the query, retrieveing the identities of the matching loans
    ///       LoanIdentityList ids = session.Loans.Query(jointCriteria);
    /// 
    ///       // Dump the results to the console
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
    public LoanIdentityList Query(EllieMae.Encompass.Query.QueryCriterion criterion)
    {
      return LoanIdentity.ToList(this.mngr.QueryLoans(new EllieMae.EMLite.ClientServer.Query.QueryCriterion[1]
      {
        criterion.Unwrap()
      }, false));
    }

    /// <summary>Selects a set of field values from a specified loan.</summary>
    /// <param name="guid">The unique identifier of the loan from which the field
    /// data will be drawn.</param>
    /// <param name="fieldIds">A list of the field IDs for the desired fields.</param>
    /// <returns>A <see cref="T:EllieMae.Encompass.Collections.StringList">StringList</see>
    /// containing the requested field values in the same order as was specified in
    /// the <c>fieldIds</c> parameter.</returns>
    /// <remarks>This method provides a more bandwidth-efficient manner to
    /// retrieve a small subset of the fields of a loan without having to open the entire
    /// loan. Note that because there is no facility for indicating from which Borrower
    /// Pair the data will be drawn, the "Current" pair will always be used for
    /// borrower-specific field values.</remarks>
    /// <example>
    /// The following example demonstrates how to generate a report using the SelectFields
    /// function to grab only the desired data from a loan.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.Query;
    /// using EllieMae.Encompass.Collections;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// 
    /// class LoanReader
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Build the query criterion for all loans that were opened this year
    ///       DateFieldCriterion dateCri = new DateFieldCriterion();
    ///       dateCri.FieldName = "Loan.DateFileOpened";
    ///       dateCri.Value = DateTime.Now;
    ///       dateCri.Precision = DateFieldMatchPrecision.Year;
    /// 
    ///       // Perform the query to get the IDs of the loans
    ///       LoanIdentityList ids = session.Loans.Query(dateCri);
    /// 
    ///       // Create a list of the specific fields we want to print from each loan.
    ///       // In this case, we'll select the Loan Amount and Interest Rate.
    ///       StringList fieldIds = new StringList();
    ///       fieldIds.Add("2");          // Loan Amount
    ///       fieldIds.Add("3");          // Rate
    /// 
    ///       // For each loan, select the desired fields
    ///       foreach (LoanIdentity id in ids)
    ///       {
    ///          // Select the field values for the current loan
    ///          StringList fieldValues = session.Loans.SelectFields(id.Guid, fieldIds);
    /// 
    ///          // Print out the returned values
    ///          Console.WriteLine("Fields for loan " + id.ToString());
    ///          Console.WriteLine("Amount:  " + fieldValues[0]);
    ///          Console.WriteLine("Rate:    " + fieldValues[1]);
    ///       }
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public StringList SelectFields(string guid, StringList fieldIds)
    {
      EllieMae.EMLite.ClientServer.ILoan loan = this.mngr.OpenLoan(guid ?? "");
      if (loan == null)
        return (StringList) null;
      try
      {
        return new StringList((IList) loan.SelectFields(fieldIds.ToArray()));
      }
      finally
      {
        loan.Close();
      }
    }

    /// <summary>
    /// Opens a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.PipelineCursor" /> for all loans accessible by current user.
    /// </summary>
    /// <param name="sortOrder">The sort order to be applied to the elements in the cursor.</param>
    /// <returns>Returns a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.PipelineCursor" /> representing the list of loans.
    /// A cursor provides an efficient means of retrieving large data sets by holding the
    /// data on the server until needed by the client. Because the cursor consumes server
    /// resources, you should call the cursor's Close() method when you are done using it.
    /// </returns>
    /// <example>
    /// The following code opens the full pipeline for the current user and generates
    /// a list of the borrowers' names.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessEnums;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// 
    /// class LoanReader
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Open a PipelineCursor so it's sorted by the borrower's last name
    ///       // Using the "using" syntax will ensure the cursor is properly closed
    ///       // on the server when it's no longer needed.
    ///       using (PipelineCursor pc = session.Loans.OpenPipeline(PipelineSortOrder.LastName))
    ///       {
    ///          // Using the foreach syntax will allow for efficient enumeration over the
    ///          // items in the cursor.
    ///          foreach (PipelineData data in pc)
    ///          {
    ///             Console.WriteLine(data["BorrowerLastName"] + ", " + data["BorrowerFirstName"]
    ///                + " for loan amount of " + data["LoanAmount"]);
    ///          }
    ///       }
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public PipelineCursor OpenPipeline(PipelineSortOrder sortOrder)
    {
      return this.OpenPipeline(sortOrder, true);
    }

    /// <summary>
    /// Opens a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.PipelineCursor" /> for all loans accessible by current user.
    /// </summary>
    /// <param name="sortOrder">The sort order to be applied to the elements in the cursor.</param>
    /// <param name="excludeArchivedLoans">To exclude archive loans, default is true.</param>
    /// <returns>Returns a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.PipelineCursor" /> representing the list of loans.
    /// A cursor provides an efficient means of retrieving large data sets by holding the
    /// data on the server until needed by the client. Because the cursor consumes server
    /// resources, you should call the cursor's Close() method when you are done using it.
    /// </returns>
    /// <example>
    ///       The following code opens the full pipeline for the current user and generates
    ///       a list of the borrowers' names.
    ///       <code>
    ///         <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessEnums;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// 
    /// class LoanReader
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Open a PipelineCursor so it's sorted by the borrower's last name
    ///       // Using the "using" syntax will ensure the cursor is properly closed
    ///       // on the server when it's no longer needed.
    ///       using (PipelineCursor pc = session.Loans.OpenPipeline(PipelineSortOrder.LastName, false))
    ///       {
    ///          // Using the foreach syntax will allow for efficient enumeration over the
    ///          // items in the cursor.
    ///          foreach (PipelineData data in pc)
    ///          {
    ///             Console.WriteLine(data["BorrowerLastName"] + ", " + data["BorrowerFirstName"]
    ///                + " for loan amount of " + data["LoanAmount"]);
    ///          }
    ///       }
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    ///       </code>
    ///     </example>
    public PipelineCursor OpenPipeline(PipelineSortOrder sortOrder, bool excludeArchivedLoans = true)
    {
      ClientMetricsProviderFactory.IncrementCounter("PipelineOpenIncCounter", (SFxTag) new SFxSdkTag());
      using (ClientMetricsProviderFactory.GetIncrementalTimer("PipelineOpenIncTimer", (SFxTag) new SFxSdkTag()))
      {
        if (this.loanSoftArchivalenabled && !this.canAccessArchiveLoans && !excludeArchivedLoans)
          throw new Exception("User doesn't have access to archive loans.");
        return new PipelineCursor(this.Session, this.mngr.OpenPipeline((string) null, LoanInfo.Right.Access, (string[]) null, EllieMae.EMLite.DataEngine.PipelineData.All, (EllieMae.EMLite.ClientServer.PipelineSortOrder) sortOrder, (EllieMae.EMLite.ClientServer.Query.QueryCriterion) null, false, this.loanSoftArchivalenabled && (!this.canAccessArchiveLoans || excludeArchivedLoans)));
      }
    }

    /// <summary>
    /// Opens a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.PipelineCursor" /> for all loans accessible by current user.
    /// </summary>
    /// <param name="sortCriteria">The sort criteria used to order the pipeline results.
    /// Passing a value of <c>null</c> (use <c>Nothing</c> in Visual Basic) will
    /// result in an unsorted list.</param>
    /// <returns>Returns a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.PipelineCursor" /> representing the list of loans.
    /// A cursor provides an efficient means of retrieving large data sets by holding the
    /// data on the server until needed by the client. Because the cursor consumes server
    /// resources, you should call the cursor's Close() method when you are done using it.
    /// </returns>
    /// <example>
    /// The following code opens the full pipeline for the current user and generates
    /// a list of the borrowers' names. It applies a custom sort order based on the
    /// loan amount.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessEnums;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
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
    ///       // Create the sort criteria for the pipeline
    ///       SortCriterionList criteria = new SortCriterionList();
    ///       criteria.Add(new SortCriterion("Loan.LoanAmount"));
    /// 
    ///       // Open a PipelineCursor so it's sorted by the loan amount.
    ///       // Using the "using" syntax will ensure the cursor is properly closed
    ///       // on the server when it's no longer needed.
    ///       using (PipelineCursor pc = session.Loans.OpenPipelineEx(criteria))
    ///       {
    ///          // Using the foreach syntax will allow for efficient enumeration over the
    ///          // items in the cursor.
    ///          foreach (PipelineData data in pc)
    ///          {
    ///             Console.WriteLine(data["BorrowerLastName"] + ", " + data["BorrowerFirstName"]
    ///                + " for loan amount of " + data["LoanAmount"]);
    ///          }
    ///       }
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public PipelineCursor OpenPipelineEx(SortCriterionList sortCriteria)
    {
      return this.OpenPipelineEx(sortCriteria, true);
    }

    /// <summary>
    /// Opens a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.PipelineCursor" /> for all loans accessible by current user.
    /// </summary>
    /// <param name="sortCriteria">The sort criteria used to order the pipeline results.
    /// Passing a value of <c>null</c> (use <c>Nothing</c> in Visual Basic) will
    /// result in an unsorted list.</param>
    /// <param name="excludeArchivedLoans">To exclude archive loans, default is true.</param>
    /// <returns>Returns a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.PipelineCursor" /> representing the list of loans.
    /// A cursor provides an efficient means of retrieving large data sets by holding the
    /// data on the server until needed by the client. Because the cursor consumes server
    /// resources, you should call the cursor's Close() method when you are done using it.
    /// </returns>
    /// <example>
    ///       The following code opens the full pipeline for the current user and generates
    ///       a list of the borrowers' names. It applies a custom sort order based on the
    ///       loan amount.
    ///       <code>
    ///         <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessEnums;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
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
    ///       // Create the sort criteria for the pipeline
    ///       SortCriterionList criteria = new SortCriterionList();
    ///       criteria.Add(new SortCriterion("Loan.LoanAmount"));
    /// 
    ///       // Open a PipelineCursor so it's sorted by the loan amount.
    ///       // Using the "using" syntax will ensure the cursor is properly closed
    ///       // on the server when it's no longer needed.
    ///       using (PipelineCursor pc = session.Loans.OpenPipelineEx(criteria, false))
    ///       {
    ///          // Using the foreach syntax will allow for efficient enumeration over the
    ///          // items in the cursor.
    ///          foreach (PipelineData data in pc)
    ///          {
    ///             Console.WriteLine(data["BorrowerLastName"] + ", " + data["BorrowerFirstName"]
    ///                + " for loan amount of " + data["LoanAmount"]);
    ///          }
    ///       }
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    ///       </code>
    ///     </example>
    public PipelineCursor OpenPipelineEx(SortCriterionList sortCriteria, bool excludeArchivedLoans = true)
    {
      ClientMetricsProviderFactory.IncrementCounter("PipelineOpenIncCounter", (SFxTag) new SFxSdkTag());
      using (ClientMetricsProviderFactory.GetIncrementalTimer("PipelineOpenIncTimer", (SFxTag) new SFxSdkTag()))
      {
        if (sortCriteria == null)
          sortCriteria = new SortCriterionList();
        if (this.loanSoftArchivalenabled && !this.canAccessArchiveLoans && !excludeArchivedLoans)
          throw new Exception("User doesn't have access to archive loans.");
        return new PipelineCursor(this.Session, this.mngr.OpenPipeline((string[]) null, LoanInfo.Right.Access, (string[]) null, EllieMae.EMLite.DataEngine.PipelineData.All, sortCriteria.ToSortFieldList(), (EllieMae.EMLite.ClientServer.Query.QueryCriterion) null, false, 0, false, this.loanSoftArchivalenabled && (!this.canAccessArchiveLoans || excludeArchivedLoans)));
      }
    }

    /// <summary>
    /// Opens a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.PipelineCursor" /> using a selection
    /// query to filter the items in the pipeline.
    /// </summary>
    /// <param name="criterion">The selection criteria to be used to filter the cursor.</param>
    /// <param name="sortOrder">The sort order to be applied to the elements in the cursor.</param>
    /// <returns>Returns a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.PipelineCursor" /> representing the list of loans.
    /// A cursor provides an efficient means of retrieving large data sets by holding the
    /// data on the server until needed by the client. Because the cursor consumes server
    /// resources, you should call the cursor's Close() method when you are done using it.
    /// </returns>
    /// <example>
    /// The following code queries the current user's accessible loans for all items
    /// which have a loan amount of at least $200,000 and for which the user "amy" is
    /// the Loan Officer. Each loan is then displayed to the user.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessEnums;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
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
    ///       // Create the query criteria to locate loans above $200K
    ///       // for which the user "amy" is the Loan Officer
    ///       NumericFieldCriterion amtcri = new NumericFieldCriterion("Loan.LoanAmount",
    ///          200000, OrdinalFieldMatchType.GreaterThanOrEquals);
    ///       StringFieldCriterion locri = new StringFieldCriterion("Loan.LoanOfficerID",
    ///          "amy", StringFieldMatchType.Exact, true);
    /// 
    ///       // Open a PipelineCursor using the logical AND of the two criteria and
    ///       // applying the desired sort order
    ///       PipelineCursor pc = session.Loans.QueryPipeline(amtcri.And(locri), PipelineSortOrder.LastName);
    /// 
    ///       try
    ///       {
    ///          // Using the foreach syntax will allow for efficient enumeration over the
    ///          // items in the cursor.
    ///          foreach (PipelineData data in pc)
    ///          {
    ///             Console.WriteLine(data["BorrowerLastName"] + ", " + data["BorrowerFirstName"]
    ///                + " for loan amount of " + data["LoanAmount"]);
    ///          }
    ///       }
    ///       finally
    ///       {
    ///          // Close the cursor to ensure its resources are released
    ///          pc.Close();
    ///       }
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public PipelineCursor QueryPipeline(EllieMae.Encompass.Query.QueryCriterion criterion, PipelineSortOrder sortOrder)
    {
      return this.QueryPipeline(criterion, sortOrder, true);
    }

    /// <summary>
    /// Opens a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.PipelineCursor" /> using a selection
    /// query to filter the items in the pipeline.
    /// </summary>
    /// <param name="criterion">The selection criteria to be used to filter the cursor.</param>
    /// <param name="sortOrder">The sort order to be applied to the elements in the cursor.</param>
    /// <param name="excludeArchivedLoans">To exclude archive loans, default is true.</param>
    /// <returns>Returns a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.PipelineCursor" /> representing the list of loans.
    /// A cursor provides an efficient means of retrieving large data sets by holding the
    /// data on the server until needed by the client. Because the cursor consumes server
    /// resources, you should call the cursor's Close() method when you are done using it.
    /// </returns>
    /// <example>
    ///       The following code queries the current user's accessible loans for all items
    ///       which have a loan amount of at least $200,000 and for which the user "amy" is
    ///       the Loan Officer. Each loan is then displayed to the user.
    ///       <code>
    ///         <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessEnums;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
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
    ///       // Create the query criteria to locate loans above $200K
    ///       // for which the user "amy" is the Loan Officer
    ///       NumericFieldCriterion amtcri = new NumericFieldCriterion("Loan.LoanAmount",
    ///          200000, OrdinalFieldMatchType.GreaterThanOrEquals);
    ///       StringFieldCriterion locri = new StringFieldCriterion("Loan.LoanOfficerID",
    ///          "amy", StringFieldMatchType.Exact, true);
    /// 
    ///       // Open a PipelineCursor using the logical AND of the two criteria and
    ///       // applying the desired sort order
    ///       PipelineCursor pc = session.Loans.QueryPipeline(amtcri.And(locri), PipelineSortOrder.LastName, false);
    /// 
    ///       try
    ///       {
    ///          // Using the foreach syntax will allow for efficient enumeration over the
    ///          // items in the cursor.
    ///          foreach (PipelineData data in pc)
    ///          {
    ///             Console.WriteLine(data["BorrowerLastName"] + ", " + data["BorrowerFirstName"]
    ///                + " for loan amount of " + data["LoanAmount"]);
    ///          }
    ///       }
    ///       finally
    ///       {
    ///          // Close the cursor to ensure its resources are released
    ///          pc.Close();
    ///       }
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    ///       </code>
    ///     </example>
    public PipelineCursor QueryPipeline(
      EllieMae.Encompass.Query.QueryCriterion criterion,
      PipelineSortOrder sortOrder,
      bool excludeArchivedLoans = true)
    {
      if (criterion == null)
        throw new ArgumentNullException(nameof (criterion));
      ClientMetricsProviderFactory.IncrementCounter("PipelineQueryIncCounter", (SFxTag) new SFxSdkTag());
      using (ClientMetricsProviderFactory.GetIncrementalTimer("PipelineQueryIncTimer", (SFxTag) new SFxSdkTag()))
      {
        if (this.loanSoftArchivalenabled && !this.canAccessArchiveLoans && !excludeArchivedLoans)
          throw new Exception("User doesn't have access to archive loans.");
        return new PipelineCursor(this.Session, this.mngr.OpenPipeline((string) null, LoanInfo.Right.Access, (string[]) null, EllieMae.EMLite.DataEngine.PipelineData.All, (EllieMae.EMLite.ClientServer.PipelineSortOrder) sortOrder, criterion.Unwrap(), false, this.loanSoftArchivalenabled && (!this.canAccessArchiveLoans || excludeArchivedLoans)));
      }
    }

    /// <summary>
    /// Opens a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.PipelineCursor" /> using a selection
    /// query to filter the items in the pipeline.
    /// </summary>
    /// <param name="criterion">The selection criteria to be used to filter the cursor.</param>
    /// <param name="sortCriteria">The sort criteria used to order the pipeline results.
    /// Passing a value of <c>null</c> (use <c>Nothing</c> in Visual Basic) will
    /// result in an unsorted list.</param>
    /// <returns>Returns a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.PipelineCursor" /> representing the list of loans.
    /// A cursor provides an efficient means of retrieving large data sets by holding the
    /// data on the server until needed by the client. Because the cursor consumes server
    /// resources, you should call the cursor's Close() method when you are done using it.
    /// </returns>
    /// <example>
    /// The following code queries the current user's accessible loans for all items
    /// which have a loan amount of at least $200,000 and for which the user "amy" is
    /// the Loan Officer. Each loan is then displayed to the user.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessEnums;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
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
    ///       // Create the sort criteria for the pipeline
    ///       SortCriterionList sort = new SortCriterionList();
    ///       sort.Add(new SortCriterion("Loan.LoanAmount"));
    /// 
    ///       // Create the query criteria to locate loans above $200K
    ///       // for which the user "amy" is the Loan Officer
    ///       NumericFieldCriterion amtcri = new NumericFieldCriterion("Loan.LoanAmount",
    ///         200000, OrdinalFieldMatchType.GreaterThanOrEquals);
    ///       StringFieldCriterion locri = new StringFieldCriterion("Loan.LoanOfficerID",
    ///          "amy", StringFieldMatchType.Exact, true);
    /// 
    ///       // Open a PipelineCursor using the logical AND of the two criteria and
    ///       // applying the desired sort order
    ///       PipelineCursor pc = session.Loans.QueryPipelineEx(amtcri.And(locri), sort);
    /// 
    ///       try
    ///       {
    ///          // Using the foreach syntax will allow for efficient enumeration over the
    ///          // items in the cursor.
    ///          foreach (PipelineData data in pc)
    ///          {
    ///             Console.WriteLine(data["BorrowerLastName"] + ", " + data["BorrowerFirstName"]
    ///                + " for loan amount of " + data["LoanAmount"]);
    ///          }
    ///       }
    ///       finally
    ///       {
    ///          // Close the cursor to ensure its resources are released
    ///          pc.Close();
    ///       }
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public PipelineCursor QueryPipelineEx(EllieMae.Encompass.Query.QueryCriterion criterion, SortCriterionList sortCriteria)
    {
      return this.QueryPipelineEx(criterion, sortCriteria, true);
    }

    /// <summary>
    /// Opens a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.PipelineCursor" /> using a selection
    /// query to filter the items in the pipeline.
    /// </summary>
    /// <param name="criterion">The selection criteria to be used to filter the cursor.</param>
    /// <param name="sortCriteria">The sort criteria used to order the pipeline results.
    /// Passing a value of <c>null</c> (use <c>Nothing</c> in Visual Basic) will
    /// result in an unsorted list.</param>
    /// <param name="excludeArchivedLoans">To exclude archive loans, default is true.</param>
    /// <returns>Returns a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.PipelineCursor" /> representing the list of loans.
    /// A cursor provides an efficient means of retrieving large data sets by holding the
    /// data on the server until needed by the client. Because the cursor consumes server
    /// resources, you should call the cursor's Close() method when you are done using it.
    /// </returns>
    /// <example>
    ///       The following code queries the current user's accessible loans for all items
    ///       which have a loan amount of at least $200,000 and for which the user "amy" is
    ///       the Loan Officer. Each loan is then displayed to the user.
    ///       <code>
    ///         <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessEnums;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
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
    ///       // Create the sort criteria for the pipeline
    ///       SortCriterionList sort = new SortCriterionList();
    ///       sort.Add(new SortCriterion("Loan.LoanAmount"));
    /// 
    ///       // Create the query criteria to locate loans above $200K
    ///       // for which the user "amy" is the Loan Officer
    ///       NumericFieldCriterion amtcri = new NumericFieldCriterion("Loan.LoanAmount",
    ///         200000, OrdinalFieldMatchType.GreaterThanOrEquals);
    ///       StringFieldCriterion locri = new StringFieldCriterion("Loan.LoanOfficerID",
    ///          "amy", StringFieldMatchType.Exact, true);
    /// 
    ///       // Open a PipelineCursor using the logical AND of the two criteria and
    ///       // applying the desired sort order
    ///       PipelineCursor pc = session.Loans.QueryPipelineEx(amtcri.And(locri), sort, false);
    /// 
    ///       try
    ///       {
    ///          // Using the foreach syntax will allow for efficient enumeration over the
    ///          // items in the cursor.
    ///          foreach (PipelineData data in pc)
    ///          {
    ///             Console.WriteLine(data["BorrowerLastName"] + ", " + data["BorrowerFirstName"]
    ///                + " for loan amount of " + data["LoanAmount"]);
    ///          }
    ///       }
    ///       finally
    ///       {
    ///          // Close the cursor to ensure its resources are released
    ///          pc.Close();
    ///       }
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    ///       </code>
    ///     </example>
    public PipelineCursor QueryPipelineEx(
      EllieMae.Encompass.Query.QueryCriterion criterion,
      SortCriterionList sortCriteria,
      bool excludeArchivedLoans = true)
    {
      if (criterion == null)
        throw new ArgumentNullException(nameof (criterion));
      if (sortCriteria == null)
        sortCriteria = new SortCriterionList();
      if (this.loanSoftArchivalenabled && !this.canAccessArchiveLoans && !excludeArchivedLoans)
        throw new Exception("User doesn't have access to archive loans.");
      ClientMetricsProviderFactory.IncrementCounter("PipelineOpenIncCounter", (SFxTag) new SFxSdkTag());
      using (ClientMetricsProviderFactory.GetIncrementalTimer("PipelineOpenIncTimer", (SFxTag) new SFxSdkTag()))
        return new PipelineCursor(this.Session, this.mngr.OpenPipeline((string[]) null, LoanInfo.Right.Access, (string[]) null, EllieMae.EMLite.DataEngine.PipelineData.All, sortCriteria.ToSortFieldList(), criterion.Unwrap(), false, 0, false, this.loanSoftArchivalenabled && (!this.canAccessArchiveLoans || excludeArchivedLoans)));
    }

    /// <summary>
    /// Creates and initializes a new <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Loan">Loan</see>.
    /// </summary>
    /// <returns>A newly created Loan object.</returns>
    /// <remarks>The returned Loan will not be saved to the server until the <c>Commit()</c>
    /// method is invoked.</remarks>
    /// <example>
    /// The following code creates a new loan, sets the value of several fields
    /// and then commits the loan to the database.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// 
    /// class LoanReader
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Create the empty shell for the new loan. At this point,
    ///       // the loan has not been saved to the Encompass server.
    ///       Loan loan = session.Loans.CreateNew();
    /// 
    ///       // Set the loan folder and loan name for the loan
    ///       loan.LoanFolder = "My Pipeline";
    ///       loan.LoanName = "Harrison";
    /// 
    ///       // Set the borrower's name and property address
    ///       loan.Fields["36"].Value = "Howard";        // First name
    ///       loan.Fields["37"].Value = "Harrison";      // Last name
    ///       loan.Fields["11"].Value = "235 Main St.";  // Street Address
    ///       loan.Fields["12"].Value = "Anycity";       // City
    ///       loan.Fields["13"].Value = "Anycounty";     // County
    ///       loan.Fields["14"].Value = "CA";            // State
    ///       loan.Fields["15"].Value = "94432";         // Zip code
    /// 
    ///       // Save the loan to the server
    ///       loan.Commit();
    /// 
    ///       // Write out the GUID of the newly created loan
    ///       Console.WriteLine(loan.Guid);
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public Loan CreateNew()
    {
      return new Loan(this.Session, LoanDataMgr.NewLoan(this.Session.SessionObjects));
    }

    /// <summary>
    /// Determines if the loan with the specified Guid exists.
    /// </summary>
    /// <param name="guid">The globally unique identifier (Guid) for the loan.</param>
    /// <returns>A flag indicating if the specified loan exists.</returns>
    /// <example>
    /// The following code checks if a loan exists and, if so, opens it.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// 
    /// class LoanReader
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // If the loan exists, open it
    ///       string guid = "{230b652c-5f60-44a1-bff2-4f4d63926c14}";
    /// 
    ///       if (session.Loans.Exists(guid))
    ///       {
    ///          // Open the loan
    ///          Loan loan = session.Loans.Open(guid);
    ///          Console.WriteLine("Opened loan " + loan.LoanFolder + "/" + loan.LoanName);
    ///       }
    ///       else
    ///       {
    ///          Console.WriteLine("Loan with GUID " + guid + " does not exist.");
    ///       }
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public bool Exists(string guid) => this.mngr.GetLoanIdentity(guid ?? "") != (EllieMae.EMLite.DataEngine.LoanIdentity) null;

    /// <summary>Imports the data from a file into a new loan.</summary>
    /// <param name="filePath">The path of the import file.</param>
    /// <param name="format">The format of the data within the import file.</param>
    /// <example>
    /// The following code imports a loan from an external Fannie Mae 3.x-formatted
    /// file.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// 
    /// class LoanReader
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Import the loan file specified on the command line.
    ///       // The returned loan must be committed to the database for
    ///       // the import to be completed.
    ///       Loan newLoan = session.Loans.Import(args[0], LoanImportFormat.FNMA3X);
    /// 
    ///       // Set the name and loan folder
    ///       newLoan.LoanName = "ImportTest";
    ///       newLoan.LoanFolder = "My Pipeline";
    /// 
    ///       // Commit the loan
    ///       newLoan.Commit();
    /// 
    ///       // Dump the GUID of the new loan
    ///       Console.WriteLine(newLoan.Guid);
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public Loan Import(string filePath, LoanImportFormat format)
    {
      return this.ImportWithTemplate(filePath, format, (EllieMae.Encompass.BusinessObjects.Loans.Templates.LoanTemplate) null);
    }

    /// <summary>Imports the data from a file into a new loan.</summary>
    /// <param name="filePath">The path of the import file.</param>
    /// <param name="format">The format of the data within the import file.</param>
    /// <param name="template">The <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Templates.LoanTemplate" />
    /// to be used during the import to initialize the loan file.</param>
    /// <example>
    /// The following code imports a loan from an external Fannie Mae 3.x-formatted
    /// file.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// 
    /// class LoanReader
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Import the loan file specified on the command line.
    ///       // The returned loan must be committed to the database for
    ///       // the import to be completed.
    ///       Loan newLoan = session.Loans.Import(args[0], LoanImportFormat.FNMA3X);
    /// 
    ///       // Set the name and loan folder
    ///       newLoan.LoanName = "ImportTest";
    ///       newLoan.LoanFolder = "My Pipeline";
    /// 
    ///       // Commit the loan
    ///       newLoan.Commit();
    /// 
    ///       // Dump the GUID of the new loan
    ///       Console.WriteLine(newLoan.Guid);
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public Loan ImportWithTemplate(string filePath, LoanImportFormat format, EllieMae.Encompass.BusinessObjects.Loans.Templates.LoanTemplate template)
    {
      return this.ImportWithLoanOfficer(filePath, format, template, (User) null);
    }

    /// <summary>
    /// Imports the data from a file into a new loan using a specified template.
    /// Assigns the specified user as the loan officer when there is no match between the
    /// loan officer name in the loan data and loan officer names in the organization.
    /// </summary>
    /// <param name="filePath">The path of the import file.</param>
    /// <param name="format">The format of the data within the import file.</param>
    /// <param name="template">The <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Templates.LoanTemplate" />
    /// to be used during the import to initialize the loan file.</param>
    /// <param name="user">The user to assign as the loan officer.</param>
    /// <example>
    /// The following code imports a loan from an external Fannie Mae 3.x-formatted
    /// file.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// 
    /// class LoanReader
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Import the loan file specified on the command line.
    ///       // The returned loan must be committed to the database for
    ///       // the import to be completed.
    ///       Loan newLoan = session.Loans.Import(args[0], LoanImportFormat.FNMA3X);
    /// 
    ///       // Set the name and loan folder
    ///       newLoan.LoanName = "ImportTest";
    ///       newLoan.LoanFolder = "My Pipeline";
    /// 
    ///       // Commit the loan
    ///       newLoan.Commit();
    /// 
    ///       // Dump the GUID of the new loan
    ///       Console.WriteLine(newLoan.Guid);
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public Loan ImportWithLoanOfficer(
      string filePath,
      LoanImportFormat format,
      EllieMae.Encompass.BusinessObjects.Loans.Templates.LoanTemplate template,
      User user)
    {
      string loanOfficerId = user != null ? user.ID : string.Empty;
      return this.openImportedLoan(Loan.ImportFile(filePath, format, template, this.Session, loanOfficerId));
    }

    /// <summary>Imports data from byte array into a new loan.</summary>
    /// <param name="importData">A byte array containing the data to be imported. This parameter
    /// is passed by reference solely for compatibility with Visual Basic 6.0 clients.
    /// The array passed to this function will not be modified.</param>
    /// <param name="format">The format of the data within the import file.</param>
    /// <remarks><note type="implementnotes">Because of language restrictions, this method
    /// cannot be used from using a weakly-typed language such as VBScript or
    /// JScript. Use the <see cref="M:EllieMae.Encompass.BusinessObjects.Loans.Loans.Import(System.String,EllieMae.Encompass.BusinessObjects.Loans.LoanImportFormat)" /> method instead.</note></remarks>
    public Loan ImportFromBytes(ref byte[] importData, LoanImportFormat format)
    {
      return this.openImportedLoan(Loan.Import(importData, format, (EllieMae.Encompass.BusinessObjects.Loans.Templates.LoanTemplate) null, this.Session, string.Empty));
    }

    /// <summary>
    /// Imports data from byte array into a new loan and applies a specified loan template.
    /// </summary>
    /// <param name="importData">A byte array containing the data to be imported. This parameter
    /// is passed by reference solely for compatibility with Visual Basic 6.0 clients.
    /// The array passed to this function will not be modified.</param>
    /// <param name="format">The format of the data within the import file.</param>
    /// <param name="template">The <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Templates.LoanTemplate" /> to be applied to the imported loan.</param>
    /// <remarks><note type="implementnotes">Because of language restrictions, this method
    /// cannot be used from using a weakly-typed language such as VBScript or
    /// JScript. Use the <see cref="M:EllieMae.Encompass.BusinessObjects.Loans.Loans.Import(System.String,EllieMae.Encompass.BusinessObjects.Loans.LoanImportFormat)" /> method instead.</note></remarks>
    public Loan ImportFromBytesWithTemplate(
      ref byte[] importData,
      LoanImportFormat format,
      EllieMae.Encompass.BusinessObjects.Loans.Templates.LoanTemplate template)
    {
      return this.openImportedLoan(Loan.Import(importData, format, template, this.Session, string.Empty));
    }

    /// <summary>
    /// Imports data from a third-party originator. This method is for internal use only.
    /// </summary>
    /// <param name="importData">A byte array containing the data to be imported. This parameter
    /// is passed by reference solely for compatibility with Visual Basic 6.0 clients.
    /// The array passed to this function will not be modified.</param>
    /// <param name="format">The format of the data within the import file.</param>
    /// <param name="template">The <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Templates.LoanTemplate" /> to be applied to the imported loan.</param>
    /// <param name="suppressCalcs">Indicates if calculations should be suppressed.</param>
    /// <remarks>This method is intended for internal use only and should not be called by external
    /// applications.</remarks>
    public Loan ImportFromTPO(
      ref byte[] importData,
      LoanImportFormat format,
      EllieMae.Encompass.BusinessObjects.Loans.Templates.LoanTemplate template,
      bool suppressCalcs)
    {
      return this.openImportedLoan(Loan.Import(importData, format, template, this.Session, string.Empty, suppressCalcs, true));
    }

    /// <summary>Deletes a loan from the server.</summary>
    /// <param name="guid">The unique identifier for the loan to delete.</param>
    /// <example>
    /// The following code deletes a loan using a GUID passed in on the command line.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// 
    /// class LoanReader
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Delete the specified loan
    ///       session.Loans.Delete(args[0]);
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public void Delete(string guid) => this.mngr.DeleteLoan(guid ?? "");

    /// <summary>
    /// Submits a batch update to the server for one or more loans.
    /// </summary>
    /// <param name="batch">The <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.BatchUpdate" /> containing the data to be updated.</param>
    /// <remarks>
    /// <p>Batch updates allow you to quickly update the value of one or more fields in a set of loans.
    /// It is significantly faster than opening the individual loans and setting the field values, but
    /// it has several potential drawbacks:
    /// <list type="bullet">
    /// <item>No standard or custom calculations will be invoked. Thus, if the batch modifies
    /// fields which are used to calculate other loan values, those values will not be updated.</item>
    /// <item>Triggers associated with the fields will not be invoked.</item>
    /// <item>Business rules, such as Field Access and Loan Access rules, are not applied. All
    /// field values will be updated without regard to the rules in place.</item>
    /// <item>Any locks held on the loan are ignored. The changes made by the batch updated will
    /// supercede any changes made by users who have the loan currently locked.</item>
    /// </list>
    /// </p>
    /// <p>Because this method can be used to bypass rules, it can only be invoked by a user
    /// with Administrator access to the system.</p>
    /// </remarks>
    /// <example>
    ///     The code below demonstrates how to update the broker name and address
    ///     fields on a batch of loans in a single call to the Encompass Server.
    ///     <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessEnums;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// using EllieMae.Encompass.Query;
    /// 
    /// class LoanReader
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "admin", "password");
    /// 
    ///       // Generate the list of loan GUIDs that will be updated
    ///       StringList guids = new StringList();
    ///       guids.Add("{55fbe34f-055f-48d0-ade5-2ec5ccfc555a}");
    ///       guids.Add("{78b61507-c4da-4051-9283-a9e6650318eb}");
    ///       guids.Add("{2c680754-816d-4826-a161-bb1b8f2fc51b}");
    /// 
    ///       // We will update the broker company information on the 1003 form
    ///       BatchUpdate batch = new BatchUpdate(guids);
    ///       batch.Fields.Add("315", "Encompass Loan Specialists, Inc.");
    ///       batch.Fields.Add("319", "123 Main Street");
    ///       batch.Fields.Add("313", "Anywhereville");
    ///       batch.Fields.Add("321", "MO");
    ///       batch.Fields.Add("323", "24432");
    /// 
    ///       // Submit the batch to the server
    ///       session.Loans.SubmitBatchUpdate(batch); // Requires administrator user
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public void SubmitBatchUpdate(BatchUpdate batch)
    {
      if (batch == null)
        throw new ArgumentNullException(nameof (batch));
      if (batch.Fields.Count == 0)
        throw new ArgumentException("BatchUpdate must contain at least one field to be updated");
      this.LoanManager.SubmitBatch(batch.Unwrap(), false);
    }

    /// <summary>
    /// Gets the list of all <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.LoanFolder">LoanFolders</see>
    /// defined on the server.
    /// </summary>
    /// <example>
    /// The following code uses the Folders property to generate a list of all
    /// of the Loan Folders defined on the server.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// 
    /// class LoanReader
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Iterate over the loan folders
    ///       foreach (LoanFolder folder in session.Loans.Folders)
    ///          Console.WriteLine("There are " + folder.Size + " loans in folder " + folder.Name);
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public LoanFolders Folders
    {
      get
      {
        if (this.folders == null)
          this.folders = new LoanFolders(this.Session);
        return this.folders;
      }
    }

    /// <summary>
    /// Gets the collection of all <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Role">Roles</see> defined in the Encompass system.
    /// </summary>
    public Roles Roles
    {
      get
      {
        if (this.roles == null)
          this.roles = new Roles(this.Session);
        return this.roles;
      }
    }

    /// <summary>
    /// Provides access to the set of defined Milestones through which loans pass
    /// as they go from start to completion.
    /// </summary>
    /// <example>
    /// The following code writes the actual or expected
    /// closing date for every loan in the "My Pipeline" folder that has been
    /// sent for processing.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessEnums;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// 
    /// class LoanReader
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Get the "My Pipeline" folder
    ///       LoanFolder fol = session.Loans.Folders["My Pipeline"];
    /// 
    ///       // Retrieve the folder's contents
    ///       LoanIdentityList ids = fol.GetContents();
    /// 
    ///       // Open each loan in the folder and check the expected closing date
    ///       for (int i = 0; i < ids.Count; i++)
    ///       {
    ///          // Open the next loan in the loop
    ///          Loan loan = fol.OpenLoan(ids[i].LoanName);
    /// 
    ///          // Get the "Closing" event from the loan
    ///          MilestoneEvent msEvent = loan.Log.MilestoneEvents.GetEventForMilestone("Closing");
    /// 
    ///          if ((msEvent != null) && (msEvent.Date != null))
    ///          {
    ///             if (msEvent.Completed)
    ///                Console.WriteLine("The loan \"" + loan.LoanName + "\" was closed on " + msEvent.Date);
    ///             else
    ///                Console.WriteLine("The loan \"" + loan.LoanName + "\" has an expected close date of " + msEvent.Date);
    ///          }
    /// 
    ///          // Close the loan
    ///          loan.Close();
    ///       }
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public Milestones Milestones
    {
      get
      {
        if (this.milestones == null)
          this.milestones = new Milestones(this.Session);
        return this.milestones;
      }
    }

    /// <summary>Readonly - returns milestone templates</summary>
    public MilestoneTemplates MilestoneTemplates
    {
      get
      {
        if (this.milestoneTemplates == null)
          this.milestoneTemplates = new MilestoneTemplates(this.Session);
        return this.milestoneTemplates;
      }
    }

    /// <summary>
    /// Provides access to the set of defined Adjustable Rate types that can be assigned to a loan.
    /// </summary>
    public AdjustableRateTypes AdjustableRateTypes
    {
      get
      {
        if (this.rateTypes == null)
          this.rateTypes = new AdjustableRateTypes();
        return this.rateTypes;
      }
    }

    /// <summary>
    /// Provides access to the set of Templates defined in the Encompass system.
    /// </summary>
    /// <example>
    /// The following code creates a new loan using an existing Loan Template.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessEnums;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// using EllieMae.Encompass.BusinessObjects.Loans.Templates;
    /// 
    /// class LoanReader
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Fetch the example purchase loan template from the server
    ///       LoanTemplate template = (LoanTemplate) session.Loans.Templates.GetTemplate(TemplateType.LoanTemplate,
    ///          @"public:\Example Puchase Loan Template");
    /// 
    ///       // Create a new loan from the template
    ///       Loan loan = template.NewLoan();
    /// 
    ///       // Set the name and folder
    ///       loan.LoanName = "TemplateLoan";
    ///       loan.LoanFolder = "My Pipeline";
    /// 
    ///       // Commit the loan to save it to the server
    ///       loan.Commit();
    ///       loan.Close();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public EllieMae.Encompass.BusinessObjects.Loans.Templates.Templates Templates
    {
      get
      {
        lock (this)
        {
          if (this.templates == null)
            this.templates = new EllieMae.Encompass.BusinessObjects.Loans.Templates.Templates(this.Session);
        }
        return this.templates;
      }
    }

    /// <summary>
    /// Provides access to the field definitions for the Encompass system.
    /// </summary>
    public LoanFieldDescriptors FieldDescriptors
    {
      get
      {
        lock (this)
        {
          if (this.fieldDescriptors == null)
            this.fieldDescriptors = new LoanFieldDescriptors(this.Session);
        }
        return this.fieldDescriptors;
      }
    }

    internal ILoanManager LoanManager => this.mngr;

    private Loan openImportedLoan(LoanDataMgr dataMgr)
    {
      if (dataMgr.LoanData.GUID != "")
      {
        Loan loan = this.Open(dataMgr.LoanData.GUID);
        if (loan != null)
        {
          loan.Attach(dataMgr.LoanData);
          dataMgr.Close();
          return loan;
        }
      }
      return new Loan(this.Session, dataMgr);
    }
  }
}
