// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.LogStatusOnlineUpdates
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
  /// Represents the collection of <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.StatusOnlineUpdate" /> objects held within
  /// a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.LoanLog" />.
  /// </summary>
  /// <example>
  /// The following code displays all of events that have been published to the
  /// Status Online web site for a particular loan, along with the date on
  /// which is was published and the user who published it.
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
  ///       // Open a loan and lock it for writing
  ///       Loan loan = session.Loans.Folders["My Pipeline"].OpenLoan("Sample");
  /// 
  ///       // Iterate over all of the Status Online updates associated with the loan
  ///       foreach (StatusOnlineUpdate update in loan.Log.StatusOnlineUpdates)
  ///       {
  ///          // Each Status Online update can contains multiple published events,
  ///          // e.g. the achievement of a milestone and the receipt of a document
  ///          foreach (StatusOnlineEvent e in update.PublishedEvents)
  ///             Console.WriteLine(e.Description + " published on " + update.Date
  ///                + " by " + update.Creator);
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
  public class LogStatusOnlineUpdates : LoanLogEntryCollection, ILogStatusOnlineUpdates, IEnumerable
  {
    internal LogStatusOnlineUpdates(Loan loan)
      : base(loan, typeof (StatusOnlineLog))
    {
    }

    /// <summary>
    /// Gets a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.StatusOnlineUpdate" /> from the collection based on its index.
    /// </summary>
    public StatusOnlineUpdate this[int index] => (StatusOnlineUpdate) this.LogEntries[index];

    /// <summary>
    /// Removes an <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.StatusOnlineUpdate" /> from the log.
    /// </summary>
    /// <param name="txn">The <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.StatusOnlineUpdate" /> to be removed.
    /// The specified entry must be an instance that belongs to the
    /// current Loan object.</param>
    public void Remove(StatusOnlineUpdate txn) => this.RemoveEntry((LogEntry) txn);

    /// <summary>Wraps a LogRecord in a LogEntry object.</summary>
    internal override LogEntry Wrap(LogRecordBase logRecord)
    {
      return (LogEntry) new StatusOnlineUpdate(this.Loan, (StatusOnlineLog) logRecord);
    }
  }
}
