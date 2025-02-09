// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.MilestoneTask
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine.Log;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
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

    public override LogEntryType EntryType => LogEntryType.MilestoneTask;

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

    public string AddedBy
    {
      get
      {
        this.EnsureValid();
        return this.logItem.AddedBy;
      }
    }

    public DateTime DateAdded
    {
      get
      {
        this.EnsureValid();
        return this.logItem.AddDate;
      }
    }

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

    public object DueDate
    {
      get
      {
        return !(this.logItem.ExpectedDate == DateTime.MinValue) ? (object) this.logItem.ExpectedDate : (object) null;
      }
    }

    public bool Completed
    {
      get
      {
        this.EnsureValid();
        return this.logItem.Completed;
      }
    }

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
