// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.StatusOnlineUpdate
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.DataEngine.Log;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>
  /// Provides a record of an update to the Status Online website for a loan.
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
  public class StatusOnlineUpdate : LogEntry, IStatusOnlineUpdate
  {
    private Loan loan;
    private StatusOnlineLog logItem;
    private StatusOnlineEvents events;

    internal StatusOnlineUpdate(Loan loan, StatusOnlineLog logItem)
      : base(loan, (LogRecordBase) logItem)
    {
      this.loan = loan;
      this.logItem = logItem;
      this.events = new StatusOnlineEvents((IEnumerable) logItem.ItemList);
    }

    /// <summary>
    /// Gets the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.LogEntryType" /> for the current entry.
    /// </summary>
    /// <remarks>This property will always return the value
    /// <see cref="F:EllieMae.Encompass.BusinessObjects.Loans.Logging.LogEntryType.StatusOnlineUpdate" />.</remarks>
    public override LogEntryType EntryType => LogEntryType.StatusOnlineUpdate;

    /// <summary>Gets the date on which the update occurred.</summary>
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
    public DateTime Date => Convert.ToDateTime(base.Date);

    /// <summary>Gets a description of the update.</summary>
    public string Description
    {
      get
      {
        this.EnsureValid();
        return this.logItem.Description;
      }
    }

    /// <summary>Gets the name of the user who performed the update.</summary>
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
    public string Creator
    {
      get
      {
        this.EnsureValid();
        return this.logItem.Creator;
      }
    }

    /// <summary>
    /// Gets the collection of events that were published to the Status Online web site
    /// as part of this update.
    /// </summary>
    /// <remarks>This collection may be empty in the event that no new items
    /// were published. This can occur if items are instead unpublished from
    /// the Status Online site.</remarks>
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
    public StatusOnlineEvents PublishedEvents
    {
      get
      {
        this.EnsureValid();
        return this.events;
      }
    }
  }
}
