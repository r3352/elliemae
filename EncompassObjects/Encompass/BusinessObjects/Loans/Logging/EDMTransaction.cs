// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.EDMTransaction
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.DataEngine.Log;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>
  /// Represents a transaction from within the Electronic Document Management (EDM) features
  /// of Encompass.
  /// </summary>
  /// <remarks>
  /// An EDM transaction is made to either send a document to the borrower for review or
  /// to request that a borrower submit a document for the loan package. Each transaction
  /// can consist of multiple document requests, which are presented in the
  /// <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.EDMTransaction.Documents" /> collection. The <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.EDMTransaction.Date" /> on the EDMTransaction
  /// represents the date on which the request was made of the borrower.
  /// </remarks>
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
  public class EDMTransaction : LogEntry, IEDMTransaction
  {
    private Loan loan;
    private EDMLog logItem;
    private EDMDocuments docs;

    internal EDMTransaction(Loan loan, EDMLog logItem)
      : base(loan, (LogRecordBase) logItem)
    {
      this.loan = loan;
      this.logItem = logItem;
      this.docs = new EDMDocuments(logItem.Documents);
    }

    /// <summary>
    /// Gets the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.LogEntryType" /> for the current entry.
    /// </summary>
    /// <remarks>This property will always return the value
    /// <see cref="F:EllieMae.Encompass.BusinessObjects.Loans.Logging.LogEntryType.EDMTransaction" />.</remarks>
    public override LogEntryType EntryType => LogEntryType.EDMTransaction;

    /// <summary>Gets the date on which the EDM transaction occurred.</summary>
    public DateTime Date => Convert.ToDateTime(base.Date);

    /// <summary>Gets the description of the EDM Transaction.</summary>
    public string Description
    {
      get
      {
        this.EnsureValid();
        return this.logItem.Description;
      }
    }

    /// <summary>
    /// Gets the name of the user who initiated this transaction.
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
    public string Creator
    {
      get
      {
        this.EnsureValid();
        return this.logItem.CreatedBy;
      }
    }

    /// <summary>
    /// Gets the collection of documents that were included in this transaction.
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
    public EDMDocuments Documents
    {
      get
      {
        this.EnsureValid();
        return this.docs;
      }
    }
  }
}
