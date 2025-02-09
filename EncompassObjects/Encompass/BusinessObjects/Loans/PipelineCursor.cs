// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.PipelineCursor
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.DataEngine;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.Collections;
using EllieMae.Encompass.Cursors;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>
  /// A PipelineCursor represents a server-side cursor for a collection of
  /// <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.PipelineData" /> objects.
  /// </summary>
  /// <remarks>Like all objects that derive from <see cref="T:EllieMae.Encompass.Cursors.Cursor" />,
  /// you should invoke the Close() method on this object when you are done using
  /// it. Otherwise, a memory leak will occur on the Encompass Server.
  /// </remarks>
  /// <example>
  /// The following code retrieves a PipelineCursor for the "My Pipeline" folder
  /// and then displays the first 20 items to the user.
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
  ///       // Query for the pipeline data from the "My Pipeline" folder. Use the
  ///       // "using" statement to ensure that the cursor is properly disposed
  ///       // so all server-side resources are released.
  ///       LoanFolder fol = session.Loans.Folders["My Pipeline"];
  /// 
  ///       using (PipelineCursor pc = fol.OpenPipeline(PipelineSortOrder.LastName))
  ///       {
  ///          // Fetch the first 20 items to display to the user (or fewer of there are
  ///          // less than 20 in the cursor).
  ///          int numItems = pc.Count;
  ///          if (numItems > 20) numItems = 20;
  /// 
  ///          foreach (PipelineData pdata in pc.GetItems(0, numItems))
  ///             Console.WriteLine(pdata.LoanIdentity.LoanName + " is in the "
  ///                + pdata["CurrentMilestoneName"] + " milestone"
  ///                + " and was last modified " + pdata["LastModified"]);
  ///       }
  /// 
  ///       // End the session to gracefully disconnect from the server
  ///       session.End();
  ///    }
  /// }
  /// ]]>
  /// </code>
  /// </example>
  public class PipelineCursor : Cursor, IPipelineCursor
  {
    private bool canAccessArchiveLoans;
    private bool loanSoftArchivalenabled;

    internal PipelineCursor(Session session, ICursor cursor)
      : base(session, cursor)
    {
      this.canAccessArchiveLoans = this.Session.GetUserInfo().IsAdministrator() || (bool) session.SessionObjects.StartupInfo.UserAclFeatureRights[(object) AclFeature.LoanMgmt_AccessToArchiveLoans];
      this.loanSoftArchivalenabled = this.Session.SessionObjects.StartupInfo.EnableLoanSoftArchival;
    }

    /// <summary>
    /// Retrieves the item from the cursor at the specified index.
    /// </summary>
    /// <param name="index">Index of the item to be retrieved (with 0 as the first
    /// index).</param>
    /// <returns>Returns the specified <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.PipelineData" /> object.</returns>
    /// <example>
    /// The following code opens a PipelineCursor to find a specific loan using
    /// the loan's GUID. It then retrieves the folder and loan name of the loan.
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
    ///       // Construct the criteria to retrieve the pipeline information for a loan
    ///       // with a specific GUID.
    ///       StringFieldCriterion cri = new StringFieldCriterion();
    ///       cri.FieldName = "Loan.Guid";
    ///       cri.Value = "{1fcf4303-4759-4a50-9eaa-22a6faf94e02}";
    /// 
    ///       // Open the cursor using the selected criteria. There's no need to
    ///       // sorth the results since there should be at most one hit.
    ///       using (PipelineCursor pc = session.Loans.QueryPipeline(cri, PipelineSortOrder.None))
    ///       {
    ///          // Check if we got a result
    ///          if (pc.Count == 0)
    ///             Console.WriteLine("Loan not found");
    ///          else
    ///          {
    ///             // Retrieve the one and only match for this query
    ///             PipelineData pdata = pc.GetItem(0);
    /// 
    ///             Console.WriteLine("The loan " + pdata.LoanIdentity.LoanName + " is in the "
    ///                + "folder '" + pdata.LoanIdentity.LoanFolder + "'.");
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
    public PipelineData GetItem(int index) => (PipelineData) base.GetItem(index);

    /// <summary>
    /// Retrieves a subset of the cursor items starting at a specified index.
    /// </summary>
    /// <param name="startIndex">The index at which to start the subset.</param>
    /// <param name="count">The number of items to retrieve</param>
    /// <returns>Returns an array containing the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.PipelineData" /> objects
    /// within the specified range</returns>
    /// <example>
    /// The following code retrieves a PipelineCursor for the "My Pipeline" folder
    /// and then displays the first 20 items to the user.
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
    ///       // Query for the pipeline data from the "My Pipeline" folder. Use the
    ///       // "using" statement to ensure that the cursor is properly disposed
    ///       // so all server-side resources are released.
    ///       LoanFolder fol = session.Loans.Folders["My Pipeline"];
    /// 
    ///       using (PipelineCursor pc = fol.OpenPipeline(PipelineSortOrder.LastName))
    ///       {
    ///          // Fetch the first 20 items to display to the user (or fewer of there are
    ///          // less than 20 in the cursor).
    ///          int numItems = pc.Count;
    ///          if (numItems > 20) numItems = 20;
    /// 
    ///          foreach (PipelineData pdata in pc.GetItems(0, numItems))
    ///             Console.WriteLine(pdata.LoanIdentity.LoanName + " is in the "
    ///                + pdata["CurrentMilestoneName"] + " milestone"
    ///                + " and was last modified " + pdata["LastModified"]);
    ///       }
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public PipelineDataList GetItems(int startIndex, int count)
    {
      return (PipelineDataList) base.GetItems(startIndex, count);
    }

    internal override object ConvertToItemType(object item)
    {
      return (object) new PipelineData(this.Session, (PipelineInfo) item);
    }

    internal override ListBase ConvertToItemList(object[] items)
    {
      return (ListBase) PipelineData.ToList(this.Session, (PipelineInfo[]) items);
    }
  }
}
