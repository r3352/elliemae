// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.MilestoneTask
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
  /// Represents a Task associated with a specific Milestone in Encompass.
  /// </summary>
  /// <example>
  ///       The following example adds an alert for the Loan Officer for any milestone
  ///       task which has not been completed for the current milestone.
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
  ///       // Get the LO role
  ///       Role lo = session.Loans.Roles.GetRoleByAbbrev("LO");
  /// 
  ///       // Iterate thru all of the tasks on the selected loan
  ///       foreach (MilestoneTask task in loan.Log.MilestoneTasks)
  ///       {
  ///         // If the task isn't complete and it's for the next milestone,
  ///         // create an alert for the Loan Officer.
  ///         if (!task.Completed && task.MilestoneEvent == loan.Log.MilestoneEvents.NextEvent)
  ///           if (task.DueDate is DateTime)
  ///           {
  ///             // Create an alert and assign to the LO
  ///             task.RoleAlerts.Add(lo, (DateTime)task.DueDate);
  ///           }
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
  ///       </code>
  ///     </example>
  public class MilestoneTask : LogEntry, IMilestoneTask
  {
    private MilestoneTaskLog logItem;
    private MilestoneEvent msEvent;
    private MilestoneTaskContacts contacts;

    internal MilestoneTask(Loan loan, MilestoneTaskLog logItem)
      : base(loan, (LogRecordBase) logItem)
    {
      this.logItem = logItem;
    }

    /// <summary>
    /// Gets the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.LogEntryType" /> for the current entry.
    /// </summary>
    /// <remarks>This property will always return the value
    /// <see cref="F:EllieMae.Encompass.BusinessObjects.Loans.Logging.LogEntryType.MilestoneTask" />.</remarks>
    public override LogEntryType EntryType => LogEntryType.MilestoneTask;

    /// <summary>Gets or sets the title of the task.</summary>
    public string Name
    {
      get
      {
        this.EnsureValid();
        return this.logItem.TaskName;
      }
      set
      {
        this.EnsureEditable();
        this.logItem.TaskName = value ?? "";
      }
    }

    /// <summary>Gets or sets the description of the task.</summary>
    public string Description
    {
      get
      {
        this.EnsureValid();
        return this.logItem.TaskDescription;
      }
      set
      {
        this.EnsureEditable();
        this.logItem.TaskDescription = value ?? "";
      }
    }

    /// <summary>
    /// Gets the ID of the user who added this condition to the loan.
    /// </summary>
    public string AddedBy
    {
      get
      {
        this.EnsureValid();
        return this.logItem.AddedBy;
      }
    }

    /// <summary>
    /// Gets the Date and time on which the condition was added to the loan.
    /// </summary>
    public DateTime DateAdded
    {
      get
      {
        this.EnsureValid();
        return this.logItem.AddDate;
      }
    }

    /// <summary>Gets or sets the priority of the task.</summary>
    public TaskPriority Priority
    {
      get
      {
        this.EnsureValid();
        return (TaskPriority) Enum.Parse(typeof (TaskPriority), this.logItem.TaskPriority, true);
      }
      set
      {
        this.EnsureEditable();
        this.logItem.TaskPriority = value != TaskPriority.None ? value.ToString() : throw new ArgumentException("Invalid priority specified");
      }
    }

    /// <summary>
    /// Gets or sets the number of days until the task is due from the date the task was added.
    /// </summary>
    /// <remarks>Set the property to -1 to clear the number of days for the task to be completed.</remarks>
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
    public int DaysToComplete
    {
      get
      {
        this.EnsureValid();
        return this.logItem.DaysToComplete;
      }
      set
      {
        this.EnsureEditable();
        this.logItem.DaysToComplete = value;
      }
    }

    /// <summary>Gets the due date for the task.</summary>
    /// <remarks>If the <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.MilestoneTask.DaysToComplete" /> property is not set, this property will return
    /// <c>null</c>. Otherwise, the property will return the date the task is due, which is the
    /// <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.MilestoneTask.DateAdded" /> plus the number of days specified by the <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.MilestoneTask.DaysToComplete" />.
    /// </remarks>
    /// <example>
    ///       The following example adds an alert for the Loan Officer for any milestone
    ///       task which has not been completed for the current milestone.
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
    ///       // Get the LO role
    ///       Role lo = session.Loans.Roles.GetRoleByAbbrev("LO");
    /// 
    ///       // Iterate thru all of the tasks on the selected loan
    ///       foreach (MilestoneTask task in loan.Log.MilestoneTasks)
    ///       {
    ///         // If the task isn't complete and it's for the next milestone,
    ///         // create an alert for the Loan Officer.
    ///         if (!task.Completed && task.MilestoneEvent == loan.Log.MilestoneEvents.NextEvent)
    ///           if (task.DueDate is DateTime)
    ///           {
    ///             // Create an alert and assign to the LO
    ///             task.RoleAlerts.Add(lo, (DateTime)task.DueDate);
    ///           }
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
    ///       </code>
    ///     </example>
    public object DueDate
    {
      get
      {
        return !(this.logItem.ExpectedDate == DateTime.MinValue) ? (object) this.logItem.ExpectedDate : (object) null;
      }
    }

    /// <summary>
    /// Gets or sets whether the task has been marked as completed.
    /// </summary>
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
    ///       // Find the insurance task(s)
    ///       LogEntryList tasks = loan.Log.MilestoneTasks.GetTasksByName("Discuss Life Insurance with Borrower");
    /// 
    ///       // If it's not already complete, mark it completed
    ///       foreach (MilestoneTask task in tasks)
    ///         if (!task.Completed)
    ///           task.DateCompleted = DateTime.Now;
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
    public bool Completed
    {
      get
      {
        this.EnsureValid();
        return this.logItem.Completed;
      }
    }

    /// <summary>Gets or sets the date the task was completed.</summary>
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
    ///       // Find the insurance task(s)
    ///       LogEntryList tasks = loan.Log.MilestoneTasks.GetTasksByName("Discuss Life Insurance with Borrower");
    /// 
    ///       // If it's not already complete, mark it completed
    ///       foreach (MilestoneTask task in tasks)
    ///         if (!task.Completed)
    ///           task.DateCompleted = DateTime.Now;
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
    public object DateCompleted
    {
      get => !this.logItem.Completed ? (object) null : (object) this.logItem.CompletedDate;
      set
      {
        if (value == null)
          this.logItem.UnmarkAsCompleted();
        else if (this.logItem.Completed)
          this.logItem.CompletedDate = (DateTime) value;
        else
          this.logItem.MarkAsCompleted((DateTime) value, this.Loan.Session.GetCurrentUser().Unwrap());
      }
    }

    /// <summary>
    /// Gets or sets the ID of the user who has completed this task.
    /// </summary>
    /// <remarks>The <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.MilestoneTask.DateCompleted" /> property must be set prior to setting this property.</remarks>
    public string CompletedBy
    {
      get => !this.logItem.Completed ? "" : this.logItem.CompletedBy;
      set
      {
        if (!this.Completed)
          throw new InvalidOperationException("The DateCompleted property must be set prior to this operation");
        if ((value ?? "") == "")
          throw new ArgumentException("Invalid user ID specified");
        this.logItem.SetCompletedByUser((this.Loan.Session.Users.GetUser(value) ?? throw new ArgumentException("Invalid user ID specified")).Unwrap());
      }
    }

    /// <summary>
    /// Gets or sets the milestone with which the task is associated.
    /// </summary>
    /// <example>
    ///       The following example adds an alert for the Loan Officer for any milestone
    ///       task which has not been completed for the current milestone.
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
    ///       // Get the LO role
    ///       Role lo = session.Loans.Roles.GetRoleByAbbrev("LO");
    /// 
    ///       // Iterate thru all of the tasks on the selected loan
    ///       foreach (MilestoneTask task in loan.Log.MilestoneTasks)
    ///       {
    ///         // If the task isn't complete and it's for the next milestone,
    ///         // create an alert for the Loan Officer.
    ///         if (!task.Completed && task.MilestoneEvent == loan.Log.MilestoneEvents.NextEvent)
    ///           if (task.DueDate is DateTime)
    ///           {
    ///             // Create an alert and assign to the LO
    ///             task.RoleAlerts.Add(lo, (DateTime)task.DueDate);
    ///           }
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
    ///       </code>
    ///     </example>
    public MilestoneEvent MilestoneEvent
    {
      get
      {
        this.EnsureValid();
        if (this.msEvent == null && this.logItem.Stage != "")
          this.msEvent = this.Loan.Log.MilestoneEvents.GetEventByInternalName(this.logItem.Stage);
        else if (this.msEvent != null && this.msEvent.InternalName != this.logItem.Stage)
          this.msEvent = this.Loan.Log.MilestoneEvents.GetEventByInternalName(this.logItem.Stage);
        return this.msEvent;
      }
      set
      {
        this.EnsureEditable();
        if (this.msEvent == null)
          throw new ArgumentNullException("The specified MilestoneEvent cannot be null");
        this.logItem.Stage = this.msEvent.Loan == this.Loan ? this.msEvent.InternalName : throw new ArgumentException("The specified MilestoneEvent must come from the same loan as the task.");
      }
    }

    /// <summary>
    /// Gets the collection of <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.MilestoneTaskContact" /> records associated with the
    /// Milestone Task.
    /// </summary>
    public MilestoneTaskContacts Contacts
    {
      get
      {
        if (this.contacts == null)
          this.contacts = new MilestoneTaskContacts(this);
        return this.contacts;
      }
    }
  }
}
