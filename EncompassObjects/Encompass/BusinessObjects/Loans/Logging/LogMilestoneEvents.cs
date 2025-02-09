// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.LogMilestoneEvents
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.DataEngine.Log;
using EllieMae.Encompass.Collections;
using System;
using System.Collections;
using System.Collections.Specialized;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>
  /// Represents the collection of <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.MilestoneEvent" /> objects stored
  /// within a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.LoanLog" />.
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
  ///          // Get the "Completion" event from the loan
  ///          MilestoneEvent msEvent =
  ///             loan.Log.MilestoneEvents.GetEventForMilestone(session.Loans.Milestones.Completion.Name);
  /// 
  ///          if ((msEvent != null) && (msEvent.Date != null))
  ///          {
  ///             if (msEvent.Completed)
  ///                Console.WriteLine("The loan \"" + loan.LoanName + "\" was completed on " + msEvent.Date);
  ///             else
  ///                Console.WriteLine("The loan \"" + loan.LoanName + "\" has an expected completion date of " + msEvent.Date);
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
  public class LogMilestoneEvents : LoanLogEntryCollection, ILogMilestoneEvents, IEnumerable
  {
    private Hashtable msLookup;

    internal LogMilestoneEvents(Loan loan)
      : base(loan, typeof (MilestoneLog))
    {
    }

    /// <summary>
    /// Gets a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.MilestoneEvent" /> by index.
    /// </summary>
    /// <remarks>The <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.MilestoneEvent" /> instances held in this collection
    /// are ordered according to the order defined by the <see cref="T:EllieMae.Encompass.BusinessEnums.Milestone">Milestone</see>
    /// class.
    /// </remarks>
    public MilestoneEvent this[int index] => (MilestoneEvent) this.LogEntries[index];

    /// <summary>
    /// Gets a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.MilestoneEvent">MilestoneEvent</see>
    /// from the collection based on the name of the corresponding
    /// <see cref="T:EllieMae.Encompass.BusinessEnums.Milestone">Milestone</see> object.
    /// </summary>
    /// <param name="milestoneName">The name of the milestone. This value is case
    /// insenstitive but must not be null or the empty string.</param>
    /// <returns>Returns the event for the specified milestone, if defined; <c>null</c>
    /// otherwise.
    /// </returns>
    /// <remarks>This method should always return a non-null value when the
    /// <c>milestoneName</c> is that of a "core" milestone, such as Started or Completion.
    /// However, for custom milestones, this method will return null if the current
    /// loan does not have an event defined for the specified milestone.
    /// <p>For a list of the names of the pre-defined Milestones which are part of every loan's
    /// lifetime sequence, see the Remarks section for the <see cref="T:EllieMae.Encompass.BusinessEnums.Milestones" />
    /// object.</p>
    /// </remarks>
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
    ///          // Get the "Completion" event from the loan
    ///          MilestoneEvent msEvent =
    ///             loan.Log.MilestoneEvents.GetEventForMilestone(session.Loans.Milestones.Completion.Name);
    /// 
    ///          if ((msEvent != null) && (msEvent.Date != null))
    ///          {
    ///             if (msEvent.Completed)
    ///                Console.WriteLine("The loan \"" + loan.LoanName + "\" was completed on " + msEvent.Date);
    ///             else
    ///                Console.WriteLine("The loan \"" + loan.LoanName + "\" has an expected completion date of " + msEvent.Date);
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
    public MilestoneEvent GetEventForMilestone(string milestoneName)
    {
      return (milestoneName ?? "") != null ? (MilestoneEvent) this.msLookup[(object) milestoneName] : throw new ArgumentException("Invalid or null milestone name");
    }

    /// <summary>
    /// Gets a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.MilestoneEvent">MilestoneEvent</see>
    /// from the collection based on the ID of the corresponding
    /// <see cref="T:EllieMae.Encompass.BusinessEnums.Milestone">Milestone</see> object.
    /// </summary>
    /// <param name="milestoneID">The MilestoneID of the Milestone.
    /// This can be a number value or a GUID value.</param>
    /// <returns>Returns the MilestoneEvent for the specified milestone, if defined; <c>null</c>
    /// otherwise.
    /// </returns>
    /// <remarks>This method should always return a non-null value when the
    /// <c>milestoneID</c> is that of a "core" milestone, such as Started or Completion.
    /// However, for custom milestones, this method will return null if the current
    /// loan does not have an event defined for the specified milestone.
    /// <p>For a list of the names of the pre-defined Milestones which are part of every loan's
    /// lifetime sequence, see the Remarks section for the <see cref="T:EllieMae.Encompass.BusinessEnums.Milestones" />
    /// object.</p>
    /// </remarks>
    public MilestoneEvent GetEventByMilestoneID(string milestoneID)
    {
      if ((milestoneID ?? "") == null)
        throw new ArgumentException("Invalid or null milestone ID");
      foreach (MilestoneEvent eventByMilestoneId in (IEnumerable) this.msLookup.Values)
      {
        if (eventByMilestoneId.MilestoneID == milestoneID)
          return eventByMilestoneId;
      }
      return (MilestoneEvent) null;
    }

    /// <summary>
    /// Gets the last <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.MilestoneEvent" /> in the loan's lifetime sequence
    /// which has been completed.
    /// </summary>
    /// <remarks> This property is always guaranteed to return a non-null
    /// <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.MilestoneEvent" />. Use the <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.LogMilestoneEvents.NextEvent">NextEvent</see>
    /// property to retrieve the next event in the loan's lifetime, if any.
    /// </remarks>
    /// <example>
    /// The following code produces a report of all the loans in the My Pipeline folder
    /// which have been closed.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessEnums;
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
    ///       // Get the "My Pipeline" folder
    ///       LoanFolder fol = session.Loans.Folders["My Pipeline"];
    /// 
    ///       // Retrieve the folder's contents
    ///       LoanIdentityList ids = fol.GetContents();
    /// 
    ///       // Get the Completion Milestone
    ///       Milestone completion = session.Loans.Milestones.Completion;
    /// 
    ///       // Open each loan in the folder and check the expected closing date
    ///       for (int i = 0; i < ids.Count; i++)
    ///       {
    ///          // Open the next loan in the loop
    ///          Loan loan = fol.OpenLoan(ids[i].LoanName);
    /// 
    ///          // Check if this loan finished the Completion stage
    ///          if (loan.Log.MilestoneEvents.LastCompletedEvent.MilestoneName == completion.Name)
    ///             Console.WriteLine("The loan \"" + loan.LoanName + "\" has been completed.");
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
    public MilestoneEvent LastCompletedEvent
    {
      get
      {
        LogList logList = this.Loan.LoanData.GetLogList();
        int milestoneIndex = logList.GetMilestoneIndex(logList.NextStage);
        MilestoneEvent milestoneEvent = this[milestoneIndex];
        return milestoneEvent.Completed ? milestoneEvent : this[milestoneIndex - 1];
      }
    }

    /// <summary>
    /// Gets the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.MilestoneEvent" /> for the first
    /// <see cref="T:EllieMae.Encompass.BusinessEnums.Milestone">Milestone</see> in the
    /// loan's lifetime sequence which has not yet been completed.
    /// </summary>
    /// <remarks>
    /// If the loan has been closed and all Milestones completed,
    /// this property will return <c>null</c> (<c>Nothing</c> in Visual Basic).
    /// </remarks>
    /// <example>
    /// The following code produces a report of all the loans in the My Pipeline folder
    /// which are currently waiting to be sent to processing.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessEnums;
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
    ///       // Get the "My Pipeline" folder
    ///       LoanFolder fol = session.Loans.Folders["My Pipeline"];
    /// 
    ///       // Retrieve the folder's contents
    ///       LoanIdentityList ids = fol.GetContents();
    /// 
    ///       // Get the Processing Milestone
    ///       Milestone processing = session.Loans.Milestones.Processing;
    /// 
    ///       // Open each loan in the folder and check the expected closing date
    ///       for (int i = 0; i < ids.Count; i++)
    ///       {
    ///          // Open the next loan in the loop
    ///          Loan loan = fol.OpenLoan(ids[i].LoanName);
    /// 
    ///          // Check if this is in the Processing stage
    ///          if ((loan.Log.MilestoneEvents.NextEvent != null) &&
    ///             (loan.Log.MilestoneEvents.NextEvent.MilestoneName == processing.Name))
    ///             Console.WriteLine("The loan \"" + loan.LoanName + "\" is waiting to be sent for processing.");
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
    public MilestoneEvent NextEvent
    {
      get
      {
        MilestoneEvent milestoneEvent = (MilestoneEvent) this.msLookup[(object) this.Loan.LoanData.GetLogList().NextStage];
        return !milestoneEvent.Completed ? milestoneEvent : (MilestoneEvent) null;
      }
    }

    /// <summary>
    /// Returns the specified MilestoneEvent using the internal stage name of the milestone.
    /// </summary>
    internal MilestoneEvent GetEventByInternalName(string stageName)
    {
      foreach (MilestoneEvent eventByInternalName in (LoanLogEntryCollection) this)
      {
        if (eventByInternalName.InternalName == stageName)
          return eventByInternalName;
      }
      return (MilestoneEvent) null;
    }

    /// <summary>
    /// Loads the list of milestones from the underlying loan file.
    /// </summary>
    internal override LogEntryList GetLogEntriesFromLoan(LogList log)
    {
      LogEntryList logEntriesFromLoan = new LogEntryList();
      this.msLookup = CollectionsUtil.CreateCaseInsensitiveHashtable();
      int numberOfMilestones = log.GetNumberOfMilestones();
      for (int i = 0; i < numberOfMilestones; ++i)
      {
        MilestoneLog milestoneAt = log.GetMilestoneAt(i);
        this.msLookup.Add((object) milestoneAt.Stage, (object) this.Wrap((LogRecordBase) milestoneAt));
        logEntriesFromLoan.Add((LogEntry) this.msLookup[(object) milestoneAt.Stage]);
      }
      return logEntriesFromLoan;
    }

    /// <summary>Wraps a MilestoneLog object in a MilestoneEvent</summary>
    internal override LogEntry Wrap(LogRecordBase logRecord)
    {
      return (LogEntry) new MilestoneEvent(this.Loan, (MilestoneLog) logRecord);
    }
  }
}
