// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.LogMilestoneTasks
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.DataEngine.Log;
using EllieMae.Encompass.BusinessObjects.Loans.Templates;
using EllieMae.Encompass.Collections;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>Summary description for ReceivedFaxes.</summary>
  /// <example>
  ///       The following code demonstrates how to add a new MilestoneTask to a loan.
  ///       <code>
  ///         <![CDATA[
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
  ///       Loan loan = session.Loans.Open("{4c1cd774-96dd-4a92-b703-df8a07b8fc98}");
  ///       loan.Lock();
  /// 
  ///       // Retrieve the "Processing" milestone
  ///       Milestone ms = session.Loans.Milestones.Processing;
  /// 
  ///       // Fetch the MilestoneEvent from the loan corresponding to the selected Milestone
  ///       MilestoneEvent msEvent = loan.Log.MilestoneEvents.GetEventForMilestone(ms.Name);
  /// 
  ///       // Create the new task on this milestone
  ///       MilestoneTask task = loan.Log.MilestoneTasks.Add("Discuss Life Insurance with Borrower", msEvent);
  /// 
  ///       // Set some basic properties on the task
  ///       task.Priority = TaskPriority.High;
  ///       task.DaysToComplete = 10;
  /// 
  ///       // Look up the life insurer contact
  ///       StringFieldCriterion cri = new StringFieldCriterion();
  ///       cri.FieldName = "Contact.CompanyName";
  ///       cri.Value = "Met Life Insurance";
  /// 
  ///       ContactList insurers = session.Contacts.Query(cri, ContactLoanMatchType.None, ContactType.Biz);
  /// 
  ///       // Add a contact to the task
  ///       if (insurers.Count > 0)
  ///         task.Contacts.Add((BizContact) insurers[0]);
  /// 
  ///       // Save and close the loan
  ///       loan.Commit();
  ///       loan.Close();
  /// 
  ///       // End the session to gracefully disconnect from the server
  ///       session.End();
  ///    }
  /// }
  /// ]]>
  ///       </code>
  ///     </example>
  public class LogMilestoneTasks : LoanLogEntryCollection, ILogMilestoneTasks, IEnumerable
  {
    internal LogMilestoneTasks(Loan loan)
      : base(loan, typeof (MilestoneTaskLog))
    {
    }

    /// <summary>
    /// Gets a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.MilestoneTask" /> from the collection based on its index.
    /// </summary>
    public MilestoneTask this[int index] => (MilestoneTask) this.LogEntries[index];

    /// <summary>
    /// Adds a new <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.MilestoneTask" /> to the log.
    /// </summary>
    /// <returns>Returns the new <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.MilestoneTask" /> object.</returns>
    /// <example>
    ///       The following code demonstrates how to add a new MilestoneTask to a loan.
    ///       <code>
    ///         <![CDATA[
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
    ///       Loan loan = session.Loans.Open("{4c1cd774-96dd-4a92-b703-df8a07b8fc98}");
    ///       loan.Lock();
    /// 
    ///       // Retrieve the "Processing" milestone
    ///       Milestone ms = session.Loans.Milestones.Processing;
    /// 
    ///       // Fetch the MilestoneEvent from the loan corresponding to the selected Milestone
    ///       MilestoneEvent msEvent = loan.Log.MilestoneEvents.GetEventForMilestone(ms.Name);
    /// 
    ///       // Create the new task on this milestone
    ///       MilestoneTask task = loan.Log.MilestoneTasks.Add("Discuss Life Insurance with Borrower", msEvent);
    /// 
    ///       // Set some basic properties on the task
    ///       task.Priority = TaskPriority.High;
    ///       task.DaysToComplete = 10;
    /// 
    ///       // Look up the life insurer contact
    ///       StringFieldCriterion cri = new StringFieldCriterion();
    ///       cri.FieldName = "Contact.CompanyName";
    ///       cri.Value = "Met Life Insurance";
    /// 
    ///       ContactList insurers = session.Contacts.Query(cri, ContactLoanMatchType.None, ContactType.Biz);
    /// 
    ///       // Add a contact to the task
    ///       if (insurers.Count > 0)
    ///         task.Contacts.Add((BizContact) insurers[0]);
    /// 
    ///       // Save and close the loan
    ///       loan.Commit();
    ///       loan.Close();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    ///       </code>
    ///     </example>
    public MilestoneTask Add(string taskName, MilestoneEvent msEvent)
    {
      if ((taskName ?? "") == "")
        throw new ArgumentException(nameof (taskName));
      if (msEvent == null)
        throw new ArgumentNullException(nameof (msEvent));
      TaskTemplate templateByName = this.Loan.Session.Loans.Templates.Tasks.GetTemplateByName(taskName);
      MilestoneTaskLog record = new MilestoneTaskLog(this.Loan.Session.GetCurrentUser().Unwrap(), taskName, "");
      record.Stage = msEvent.InternalName;
      if (templateByName != null)
        record.TaskGUID = templateByName.ID;
      return (MilestoneTask) this.CreateEntry((LogRecordBase) record);
    }

    /// <summary>
    /// Removes a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.MilestoneTask" /> from the loan.
    /// </summary>
    /// <param name="taskToRemove">The <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.MilestoneTask" /> to be removed.
    /// The specified entry must be an instance that belongs to the
    /// current Loan object.</param>
    public void Remove(MilestoneTask taskToRemove) => this.RemoveEntry((LogEntry) taskToRemove);

    /// <summary>
    /// Returns the collection of <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.MilestoneTask" /> objects associated with the specified
    /// <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.MilestoneEvent" />.
    /// </summary>
    /// <param name="msEvent">The <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.MilestoneEvent" /> for which the tasks will be retrieved.</param>
    /// <returns>Returns a <see cref="T:EllieMae.Encompass.Collections.LogEntryList" /> containing the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.MilestoneTask" /> objects
    /// for the specified milestone.</returns>
    /// <example>
    ///       The following locates all outstanding Milestone Tasks for the next expected
    ///       milestone. If no task are outstanding, it marks the milestone as completed.
    ///       <code>
    ///         <![CDATA[
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
    ///       Loan loan = session.Loans.Open("{4c1cd774-96dd-4a92-b703-df8a07b8fc98}");
    ///       loan.Lock();
    /// 
    ///       // Get the next expected milestone event and retrieve all tasks for that event
    ///       MilestoneEvent nextMs = loan.Log.MilestoneEvents.NextEvent;
    ///       LogEntryList tasks = loan.Log.MilestoneTasks.GetTasksForMilestone(nextMs);
    /// 
    ///       foreach (MilestoneTask task in tasks)
    ///         if (!task.Completed)
    ///         {
    ///           Console.WriteLine("The task '" + task.Name + "' has not been completed");
    ///           return;
    ///         }
    /// 
    ///       // If there are no outstanding milestone tasks, mark the milestone as completed
    ///       nextMs.Completed = true;
    /// 
    ///       // Save and close the loan
    ///       loan.Commit();
    ///       loan.Close();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    ///       </code>
    ///     </example>
    public LogEntryList GetTasksForMilestone(MilestoneEvent msEvent)
    {
      if (msEvent == null)
        throw new ArgumentNullException(nameof (msEvent));
      LogEntryList tasksForMilestone = new LogEntryList();
      foreach (MilestoneTask milestoneTask in (LoanLogEntryCollection) this)
      {
        if (milestoneTask.MilestoneEvent != null && milestoneTask.MilestoneEvent.InternalName == msEvent.InternalName)
          tasksForMilestone.Add((LogEntry) milestoneTask);
      }
      return tasksForMilestone;
    }

    /// <summary>
    /// Returns a collection of <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.MilestoneTask" /> objects with the specified name.
    /// </summary>
    /// <param name="taskName">The name of the tasks to be retrieved.</param>
    /// <returns>Returns a collection of <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.MilestoneTask" /> objects whose name matches
    /// the specified value.</returns>
    /// <example>
    ///       The following locates all outstanding Milestone Tasks for the next expected
    ///       milestone. If no task are outstanding, it marks the milestone as completed.
    ///       <code>
    ///         <![CDATA[
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
    ///       Loan loan = session.Loans.Open("{4c1cd774-96dd-4a92-b703-df8a07b8fc98}");
    ///       loan.Lock();
    /// 
    ///       // Get all tasks of the specified type from the loan
    ///       foreach (MilestoneTask task in loan.Log.MilestoneTasks.GetTasksByName("Discuss Life Insurance with Borrower"))
    ///       {
    ///         // If the task isn't complete and it's for the next milestone, set the priority to high
    ///         if (!task.Completed && task.MilestoneEvent == loan.Log.MilestoneEvents.NextEvent)
    ///           task.Priority = TaskPriority.High;
    ///       }
    /// 
    ///       // Save and close the loan
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
    public LogEntryList GetTasksByName(string taskName)
    {
      if ((taskName ?? "") == "")
        throw new ArgumentException(nameof (taskName));
      LogEntryList tasksByName = new LogEntryList();
      foreach (MilestoneTask milestoneTask in (LoanLogEntryCollection) this)
      {
        if (string.Compare(milestoneTask.Name, taskName, true) == 0)
          tasksByName.Add((LogEntry) milestoneTask);
      }
      return tasksByName;
    }

    /// <summary>Wraps a LogRecord in a LogEntry object.</summary>
    internal override LogEntry Wrap(LogRecordBase logRecord)
    {
      return (LogEntry) new MilestoneTask(this.Loan, (MilestoneTaskLog) logRecord);
    }
  }
}
