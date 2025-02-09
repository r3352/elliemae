// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.LogEDMTransactions
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.DataEngine.Log;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>
  /// Represents the collection of <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.EDMTransaction" /> objects held within
  /// a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.LoanLog" />.
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
  public class LogEDMTransactions : LoanLogEntryCollection, ILogEDMTransactions, IEnumerable
  {
    internal LogEDMTransactions(Loan loan)
      : base(loan, typeof (EDMLog))
    {
    }

    /// <summary>
    /// Gets a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.EDMTransaction" /> from the collection based on its index.
    /// </summary>
    public EDMTransaction this[int index] => (EDMTransaction) this.LogEntries[index];

    /// <summary>
    /// Removes an <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.EDMTransaction" /> from the log.
    /// </summary>
    /// <param name="txn">The <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.EDMTransaction" /> to be removed.
    /// The specified entry must be an instance that belongs to the
    /// current Loan object.</param>
    public void Remove(EDMTransaction txn) => this.RemoveEntry((LogEntry) txn);

    /// <summary>Wraps a LogRecord in a LogEntry object.</summary>
    internal override LogEntry Wrap(LogRecordBase logRecord)
    {
      return (LogEntry) new EDMTransaction(this.Loan, (EDMLog) logRecord);
    }
  }
}
