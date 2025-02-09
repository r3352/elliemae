// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.LoanFolders
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer;
using EllieMae.Encompass.Client;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>Summary description for LoanFolders.</summary>
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
  public class LoanFolders : SessionBoundObject, ILoanFolders, IEnumerable
  {
    private ILoanManager mngr;
    private Hashtable folders;

    internal LoanFolders(Session session)
      : base(session)
    {
      this.mngr = (ILoanManager) session.GetObject("LoanManager");
      this.Refresh();
    }

    /// <summary>
    /// Gets the number of Loan Folders defined on the server.
    /// </summary>
    public int Count => this.folders.Count;

    /// <summary>Returns a LoanFolder using its name.</summary>
    /// <remarks>The name of a loan folder is case insensitive.</remarks>
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
    public LoanFolder this[string name] => (LoanFolder) this.folders[(object) (name ?? "")];

    /// <summary>Adds a new loan folder to the server.</summary>
    /// <param name="name">The name of the new folder.</param>
    /// <returns>The new <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.LoanFolder">LoanFolder</see> object.</returns>
    /// <example>
    /// The following code creates a new Loan Folder on the server based on the
    /// parameters passed to the application.
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
    ///       // Create a new Loan Folder for each parameter passed in
    ///       for (int i = 0; i < args.Length; i++)
    ///       {
    ///          try
    ///          {
    ///             // This operation may cause an exception if a loan folder already exists
    ///             // with the specified name.
    ///             session.Loans.Folders.Add(args[i]);
    ///          }
    ///          catch (Exception ex)
    ///          {
    ///             Console.WriteLine("Error creating folder " + args[i] + ": " + ex.Message);
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
    public LoanFolder Add(string name)
    {
      if (this.folders.ContainsKey((object) (name ?? "")))
        throw new InvalidOperationException("A folder with this name already exists");
      this.mngr.CreateLoanFolder(name ?? "");
      LoanFolder loanFolder = new LoanFolder(this.Session, name);
      this.folders.Add((object) name, (object) loanFolder);
      return loanFolder;
    }

    /// <summary>Deletes an existing loan folder from the server.</summary>
    /// <param name="folder">The <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.LoanFolder">LoanFolder</see> to be deleted.</param>
    /// <remarks>The loan folder must be empty in order to be deleted.</remarks>
    /// <example>
    /// The following code example deletes all empty loan folders from the server.
    /// <code>
    /// using System;
    /// using System.Collections;
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
    ///       ArrayList toBeDeleted = new ArrayList();
    /// 
    ///       // Iterate over all defined loan folders, saving off any folder which is empty
    ///       foreach (LoanFolder folder in session.Loans.Folders)
    ///       {
    ///          if (folder.Size == 0)
    ///             toBeDeleted.Add(folder);
    ///       }
    /// 
    ///       // Now delete the folders from the set
    ///       foreach (LoanFolder folder in toBeDeleted)
    ///          session.Loans.Folders.Remove(folder);
    /// 
    ///       // Refresh the list so we can see any changes made by other users
    ///       session.Loans.Folders.Refresh();
    /// 
    ///       // Dump the list of folder names
    ///       foreach (LoanFolder folder in session.Loans.Folders)
    ///          Console.WriteLine(folder.Name);
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// </code>
    /// </example>
    public void Remove(LoanFolder folder)
    {
      this.mngr.DeleteLoanFolder(folder.Name, false);
      this.folders.Remove((object) folder.Name);
    }

    /// <summary>
    /// Rebuilds the contents of all of the Loan Folders on the server.
    /// </summary>
    /// <remarks>This method rebuilds the loan database from the information
    /// stored in the loan files on disk. This process occurs synchronously, so
    /// this method may take several minutes before it returns if the number of
    /// loans in the system is large.
    /// <p>The logged in user must have administrative privileges to invoke this method.
    /// </p>
    /// </remarks>
    /// <example>
    /// The following code rebuilds all loan folders from the loan files on disk.
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
    ///       // Rebuild all folders -- this may take a while if there are a large
    ///       // number of loans.
    ///       session.Loans.Folders.RebuildAll();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public void RebuildAll()
    {
      this.mngr.RebuildPipeline((IServerProgressFeedback) null, DatabaseToRebuild.Both);
    }

    /// <summary>
    /// Refreshes the list of loan folders from the server. Any loan folders added by other
    /// users will be visible after calling this method.
    /// </summary>
    /// <example>
    /// The following code example deletes all empty loan folders from the server.
    /// <code>
    /// using System;
    /// using System.Collections;
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
    ///       ArrayList toBeDeleted = new ArrayList();
    /// 
    ///       // Iterate over all defined loan folders, saving off any folder which is empty
    ///       foreach (LoanFolder folder in session.Loans.Folders)
    ///       {
    ///          if (folder.Size == 0)
    ///             toBeDeleted.Add(folder);
    ///       }
    /// 
    ///       // Now delete the folders from the set
    ///       foreach (LoanFolder folder in toBeDeleted)
    ///          session.Loans.Folders.Remove(folder);
    /// 
    ///       // Refresh the list so we can see any changes made by other users
    ///       session.Loans.Folders.Refresh();
    /// 
    ///       // Dump the list of folder names
    ///       foreach (LoanFolder folder in session.Loans.Folders)
    ///          Console.WriteLine(folder.Name);
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// </code>
    /// </example>
    public void Refresh()
    {
      this.folders = new Hashtable((IEqualityComparer) StringComparer.OrdinalIgnoreCase);
      string[] allLoanFolderNames = this.mngr.GetAllLoanFolderNames(false);
      for (int index = 0; index < allLoanFolderNames.Length; ++index)
        this.folders.Add((object) allLoanFolderNames[index], (object) new LoanFolder(this.Session, allLoanFolderNames[index]));
    }

    /// <summary>Allows for enumeration over the set of loan folders.</summary>
    /// <returns>An enumerator for the set of folder.</returns>
    public IEnumerator GetEnumerator() => this.folders.Values.GetEnumerator();
  }
}
