// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.EDMDocument
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>
  /// Represents a single document or page that is included in an EDM transaction.
  /// </summary>
  /// <example>
  /// The following code displays all of the documents that have been requested
  /// from the borrower to be signed and returned, along with the name of the
  /// individual who made the request and the date the request was made.
  /// <code>
  /// <![CDATA[
  /// using System;
  /// using System.IO;
  /// using EllieMae.Encompass.Client;
  /// using EllieMae.Encompass.BusinessObjects;
  /// using EllieMae.Encompass.BusinessObjects.Loans;
  /// using EllieMae.Encompass.BusinessObjects.Loans.Logging;
  /// 
  /// class LoanReader
  /// {
  ///    public static void Main()
  ///    {
  ///       // Open the session to the remote server
  ///       Session session = new Session();
  ///       session.Start("myserver", "mary", "maryspwd");
  /// 
  ///       // Open a loan from the "My Pipeline" folder
  ///       Loan loan = session.Loans.Folders["My Pipeline"].OpenLoan("Sample");
  /// 
  ///       // Iterate over all of the EDM transactions associated with the loan
  ///       foreach (EDMTransaction txn in loan.Log.EDMTransactions)
  ///       {
  ///          // Iterate over the list of documents.
  ///          foreach (EDMDocument doc in txn.Documents)
  ///             Console.WriteLine(doc.Title + " requested on " + txn.Date + " by " + txn.Creator);
  ///       }
  /// 
  ///       loan.Close();
  /// 
  ///       // End the session to gracefully disconnect from the server
  ///       session.End();
  ///    }
  /// }
  /// ]]>
  /// </code>
  /// </example>
  public class EDMDocument : IEDMDocument
  {
    private string title;

    internal EDMDocument(string title) => this.title = title;

    /// <summary>
    /// Gets the name of the document that was sent or requested.
    /// </summary>
    public string Title => this.title;
  }
}
