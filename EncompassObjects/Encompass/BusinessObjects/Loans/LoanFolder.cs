// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.LoanFolder
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using Elli.Metrics.Client;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.DataEngine;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.Collections;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>
  /// Represents a loan folder in which loans may be created or saved.
  /// </summary>
  /// <example>
  /// The following code displays the contents of a Loan Folder.
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
  ///       // Fetch the "My Pipeline" folder
  ///       LoanFolder folder = session.Loans.Folders["My Pipeline"];
  /// 
  ///       // Retrieve the list of all loans from this folder visible to the logged in user
  ///       LoanIdentityList ids = folder.GetContents();
  /// 
  ///       foreach (LoanIdentity id in ids)
  ///       {
  ///          // Open the Loan
  ///          Loan loan = folder.OpenLoan(id.LoanName);
  /// 
  ///          // Display the address of the property
  ///          Console.WriteLine(loan.Fields["11"].Value);     // Street Addr
  ///          Console.WriteLine(loan.Fields["12"].Value);     // City
  ///          Console.WriteLine(loan.Fields["14"].Value);     // State
  /// 
  ///          // Close the loan to release its resources
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
  public class LoanFolder : SessionBoundObject, ILoanFolder
  {
    private ILoanManager mngr;
    private string folderName;
    private LoanFolderInfo info;
    private int size = -1;
    private bool canAccessArchiveLoans;
    private bool loanSoftArchivalenabled;

    internal LoanFolder(Session session, string folderName)
      : base(session)
    {
      this.mngr = session.Loans.LoanManager;
      this.folderName = folderName;
      this.canAccessArchiveLoans = this.Session.GetUserInfo().IsAdministrator() || (bool) session.SessionObjects.StartupInfo.UserAclFeatureRights[(object) AclFeature.LoanMgmt_AccessToArchiveLoans];
      this.loanSoftArchivalenabled = this.Session.SessionObjects.StartupInfo.EnableLoanSoftArchival;
    }

    /// <summary>Gets the name of the folder.</summary>
    public string Name => this.folderName;

    /// <summary>Returns the display name of the folder.</summary>
    /// <remarks>For archive folders, the display name will include the foldername surrounded by
    /// the &lt;&gt; symbols. For non-archive folders, the Display name should match the Name.</remarks>
    public string DisplayName
    {
      get
      {
        this.ensureLoaded();
        return this.info.DisplayName;
      }
    }

    /// <summary>Indicates if the folder is an archive folder.</summary>
    public bool IsArchive
    {
      get
      {
        this.ensureLoaded();
        return this.info.Type == LoanFolderInfo.LoanFolderType.Archive;
      }
    }

    /// <summary>Indicates if the folder is the (Trash) folder.</summary>
    public bool IsTrash
    {
      get
      {
        this.ensureLoaded();
        return this.info.Type == LoanFolderInfo.LoanFolderType.Trash;
      }
    }

    /// <summary>
    /// Gets the approximate number of loans stored in the loan folder.
    /// </summary>
    /// <remarks>This count represents the total number of subdirectories beneath the
    /// current loan folder. Generally, this should be the number of loans stored in
    /// the folder, but its value should always be considered approximate.</remarks>
    public int Size
    {
      get
      {
        if (this.size == -1)
          this.size = this.mngr.GetLoanFolderPhysicalSize(this.folderName);
        return this.size;
      }
    }

    /// <summary>
    /// Returns the list of loans stored in the current folder. Only loans to which
    /// the logged in user has access will be included in the list.
    /// </summary>
    /// <returns>Returns a LoanIdentityList containing one <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.LoanIdentity">LoanIdentity</see>
    /// object for each loan in the folder.</returns>
    /// <example>
    /// The following code displays the contents of a Loan Folder.
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
    ///       // Fetch the "My Pipeline" folder
    ///       LoanFolder folder = session.Loans.Folders["My Pipeline"];
    /// 
    ///       // Retrieve the list of all loans from this folder visible to the logged in user
    ///       LoanIdentityList ids = folder.GetContents();
    /// 
    ///       foreach (LoanIdentity id in ids)
    ///       {
    ///          // Open the Loan
    ///          Loan loan = folder.OpenLoan(id.LoanName);
    /// 
    ///          // Display the address of the property
    ///          Console.WriteLine(loan.Fields["11"].Value);     // Street Addr
    ///          Console.WriteLine(loan.Fields["12"].Value);     // City
    ///          Console.WriteLine(loan.Fields["14"].Value);     // State
    /// 
    ///          // Close the loan to release its resources
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
    public LoanIdentityList GetContents()
    {
      return LoanIdentity.ToList(this.mngr.GetLoanFolderContents(this.folderName, LoanInfo.Right.Access, false));
    }

    /// <summary>
    /// Opens a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.PipelineCursor" /> for the current folder.
    /// </summary>
    /// <param name="sortOrder">The sort order to be applied to the elements in the cursor.</param>
    /// <returns>Returns a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.PipelineCursor" /> representing the list of loans.
    /// A cursor provides an efficient means of retrieving large data sets by holding the
    /// data on the server until needed by the client. Because the cursor consumes server
    /// resources, you should call the cursor's Close() method when you are done using it.
    /// </returns>
    public PipelineCursor OpenPipeline(PipelineSortOrder sortOrder)
    {
      return this.OpenPipeline(sortOrder, true);
    }

    /// <summary>
    /// Opens a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.PipelineCursor" /> for the current folder.
    /// </summary>
    /// <param name="sortOrder">The sort order to be applied to the elements in the cursor.</param>
    /// <param name="excludeArchivedLoans">To exclude archive loans, default is true.</param>
    /// <returns>Returns a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.PipelineCursor" /> representing the list of loans.
    /// A cursor provides an efficient means of retrieving large data sets by holding the
    /// data on the server until needed by the client. Because the cursor consumes server
    /// resources, you should call the cursor's Close() method when you are done using it.
    /// </returns>
    public PipelineCursor OpenPipeline(PipelineSortOrder sortOrder, bool excludeArchivedLoans = true)
    {
      ClientMetricsProviderFactory.IncrementCounter("PipelineOpenIncCounter", (SFxTag) new SFxSdkTag());
      if (this.loanSoftArchivalenabled && !this.canAccessArchiveLoans && !excludeArchivedLoans || this.loanSoftArchivalenabled && !this.canAccessArchiveLoans && this.IsArchive)
        throw new Exception("User doesn't have access to archive loans.");
      using (ClientMetricsProviderFactory.GetIncrementalTimer("PipelineOpenIncTimer", (SFxTag) new SFxSdkTag()))
        return new PipelineCursor(this.Session, this.mngr.OpenPipeline(this.folderName, LoanInfo.Right.Access, (string[]) null, EllieMae.EMLite.DataEngine.PipelineData.All, (EllieMae.EMLite.ClientServer.PipelineSortOrder) sortOrder, (EllieMae.EMLite.ClientServer.Query.QueryCriterion) null, false, this.loanSoftArchivalenabled && (!this.canAccessArchiveLoans || excludeArchivedLoans)));
    }

    /// <summary>
    /// Opens a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.PipelineCursor" /> for the current folder using a selection
    /// query to filter the items in the pipeline.
    /// </summary>
    /// <param name="criterion">The selection criteria to be used to filter the cursor.</param>
    /// <param name="sortOrder">The sort order to be applied to the elements in the cursor.</param>
    /// <returns>Returns a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.PipelineCursor" /> representing the list of loans.
    /// A cursor provides an efficient means of retrieving large data sets by holding the
    /// data on the server until needed by the client. Because the cursor consumes server
    /// resources, you should call the cursor's Close() method when you are done using it.
    /// </returns>
    public PipelineCursor QueryPipeline(EllieMae.Encompass.Query.QueryCriterion criterion, PipelineSortOrder sortOrder)
    {
      return this.QueryPipeline(criterion, sortOrder, true);
    }

    /// <summary>
    /// Opens a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.PipelineCursor" /> for the current folder using a selection
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
    public PipelineCursor QueryPipeline(
      EllieMae.Encompass.Query.QueryCriterion criterion,
      PipelineSortOrder sortOrder,
      bool excludeArchivedLoans = true)
    {
      if (criterion == null)
        throw new ArgumentNullException(nameof (criterion));
      if (this.loanSoftArchivalenabled && !this.canAccessArchiveLoans && !excludeArchivedLoans || this.loanSoftArchivalenabled && !this.canAccessArchiveLoans && this.IsArchive)
        throw new Exception("User doesn't have access to archive loans.");
      ClientMetricsProviderFactory.IncrementCounter("PipelineQueryIncCounter", (SFxTag) new SFxSdkTag());
      using (ClientMetricsProviderFactory.GetIncrementalTimer("PipelineQueryIncTimer", (SFxTag) new SFxSdkTag()))
        return new PipelineCursor(this.Session, this.mngr.OpenPipeline(this.folderName, LoanInfo.Right.Access, (string[]) null, EllieMae.EMLite.DataEngine.PipelineData.All, (EllieMae.EMLite.ClientServer.PipelineSortOrder) sortOrder, criterion.Unwrap(), false, this.loanSoftArchivalenabled && (!this.canAccessArchiveLoans || excludeArchivedLoans)));
    }

    /// <summary>
    /// Opens the loan with the specified name which resides in the current folder.
    /// </summary>
    /// <param name="name">The name of the loan to open.</param>
    /// <returns>A reference to the specified <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Loan">Loan</see>, or null
    /// if no loan is found with the given name.</returns>
    /// <example>
    /// The following code displays the contents of a Loan Folder.
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
    ///       // Fetch the "My Pipeline" folder
    ///       LoanFolder folder = session.Loans.Folders["My Pipeline"];
    /// 
    ///       // Retrieve the list of all loans from this folder visible to the logged in user
    ///       LoanIdentityList ids = folder.GetContents();
    /// 
    ///       foreach (LoanIdentity id in ids)
    ///       {
    ///          // Open the Loan
    ///          Loan loan = folder.OpenLoan(id.LoanName);
    /// 
    ///          // Display the address of the property
    ///          Console.WriteLine(loan.Fields["11"].Value);     // Street Addr
    ///          Console.WriteLine(loan.Fields["12"].Value);     // City
    ///          Console.WriteLine(loan.Fields["14"].Value);     // State
    /// 
    ///          // Close the loan to release its resources
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
    public Loan OpenLoan(string name)
    {
      try
      {
        ClientMetricsProviderFactory.IncrementCounter("LoanOpenIncCounter", (SFxTag) new SFxSdkTag());
        using (ClientMetricsProviderFactory.GetIncrementalTimer("LoanOpenIncTimer", (SFxTag) new SFxSdkTag()))
          return new Loan(this.Session, LoanDataMgr.OpenLoan(this.Session.SessionObjects, this.Name, name ?? "", false));
      }
      catch (ObjectNotFoundException ex)
      {
        return (Loan) null;
      }
    }

    /// <summary>Initializes a new loan in the current folder.</summary>
    /// <param name="name">The name of the new loan file.</param>
    /// <returns>A reference to the new <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Loan">Loan</see> object.
    /// This object is not saved to the server until the caller invokes the
    /// Loan's <c>Commit()</c> method.</returns>
    /// <example>
    /// The following code retrieves a specific loan folder from the system in
    /// order to create a new loan in that folder.
    /// <code>
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
    ///       // Fetch the "My Pipeline" folder
    ///       LoanFolder folder = session.Loans.Folders["My Pipeline"];
    /// 
    ///       // If the folder doesn't exist, create it
    ///       if (folder == null)
    ///          folder = session.Loans.Folders.Add("My Pipeline");
    /// 
    ///       // Create a new loan in the specified folder
    ///       Loan newLoan = folder.NewLoan("MyNewLoan");
    /// 
    ///       // Set the property address for the new loan
    ///       newLoan.Fields["11"].Value = "10877 Deer Hollow Lane";
    ///       newLoan.Fields["12"].Value = "Carson";
    ///       newLoan.Fields["14"].Value = "NV";
    /// 
    ///       // Save and close the loan
    ///       newLoan.Commit();
    ///       newLoan.Close();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// </code>
    /// </example>
    public Loan NewLoan(string name)
    {
      Loan loan = this.Session.Loans.CreateNew();
      loan.LoanFolder = this.folderName;
      loan.LoanName = name ?? "";
      return loan;
    }

    /// <summary>
    /// Returns a flag indicating if a loan with the specified name exists in the
    /// current loan folder.
    /// </summary>
    /// <param name="name">The name of the loan to locate.</param>
    /// <returns>A flag indicating if a loan exists with the specified name.</returns>
    /// <example>
    /// The following code demonstrates how to delete one or more loans from
    /// a loan folder using the loan's name as its identifying characteristic.
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
    ///       // Fetch the "My Pipeline" folder
    ///       LoanFolder folder = session.Loans.Folders["My Pipeline"];
    /// 
    ///       for (int i = 0; i < args.Length; i++)
    ///       {
    ///          // Ensure the loan exists and, if so, delete it
    ///          if (folder.LoanExists(args[i]))
    ///             folder.DeleteLoan(args[i]);
    ///       }
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public bool LoanExists(string name)
    {
      return this.mngr.GetLoanIdentity(this.Name, name ?? "") != (EllieMae.EMLite.DataEngine.LoanIdentity) null;
    }

    /// <summary>Deletes a loan from the current loan folder.</summary>
    /// <param name="name">The name of the loan to be deleted.</param>
    /// <example>
    /// The following code demonstrates how to delete one or more loans from
    /// a loan folder using the loan's name as its identifying characteristic.
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
    ///       // Fetch the "My Pipeline" folder
    ///       LoanFolder folder = session.Loans.Folders["My Pipeline"];
    /// 
    ///       for (int i = 0; i < args.Length; i++)
    ///       {
    ///          // Ensure the loan exists and, if so, delete it
    ///          if (folder.LoanExists(args[i]))
    ///             folder.DeleteLoan(args[i]);
    ///       }
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public void DeleteLoan(string name)
    {
      EllieMae.EMLite.DataEngine.LoanIdentity loanIdentity = this.mngr.GetLoanIdentity(this.Name, name ?? "");
      if (!(loanIdentity != (EllieMae.EMLite.DataEngine.LoanIdentity) null))
        return;
      this.mngr.DeleteLoan(loanIdentity.Guid);
    }

    /// <summary>
    /// Rebuilds the contents of the current Loan Folder on the server.
    /// </summary>
    /// <remarks>This method rebuilds the loan database from the information
    /// stored in the loan files within this folder. This process occurs synchronously, so
    /// this method may take several minutes before it returns if the number of
    /// loans in the folder is large.
    /// <p>The logged in user must have administrative privileges to invoke this method.
    /// </p>
    /// </remarks>
    /// <example>
    /// The following code rebuilds the "My Pipeline" folder from the loan files
    /// stored on disk.
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
    ///       // Fetch the "My Pipeline" folder
    ///       LoanFolder folder = session.Loans.Folders["My Pipeline"];
    /// 
    ///       if (folder != null)
    ///          folder.Rebuild();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public void Rebuild()
    {
      ClientMetricsProviderFactory.IncrementCounter("PipelineRefreshIncCounter", (SFxTag) new SFxSdkTag());
      using (ClientMetricsProviderFactory.GetIncrementalTimer("PipelineRefreshIncTimer", (SFxTag) new SFxSdkTag()))
        this.mngr.RebuildPipeline(this.Name, (IServerProgressFeedback) null, DatabaseToRebuild.Both);
    }

    /// <summary>
    /// Refreshes the loan folder information, such as the size.
    /// </summary>
    public void Refresh()
    {
      this.info = (LoanFolderInfo) null;
      this.size = -1;
    }

    /// <summary>
    /// Provides a string representation of the LoanFolder object.
    /// </summary>
    /// <returns>This method returns the name of the folder.</returns>
    public override string ToString() => this.folderName;

    private void ensureLoaded()
    {
      if (this.info != null)
        return;
      this.info = this.Session.SessionObjects.LoanManager.GetLoanFolder(this.folderName);
    }
  }
}
