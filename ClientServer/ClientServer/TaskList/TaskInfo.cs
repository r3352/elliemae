// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.TaskList.TaskInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.Calendar;
using EllieMae.EMLite.Common;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.TaskList
{
  [Serializable]
  public class TaskInfo : IPropertyDictionary
  {
    public readonly int TaskID;
    public readonly string UserID;
    public TaskType TaskType;
    private TaskStatus taskStatus;
    public string Subject;
    public string Notes;
    public DateTime DueDate;
    public DateTime StartDate;
    public TaskPriority Priority;
    public readonly int CampaignStepID;
    public readonly DateTime CreationTime;
    private DateTime lastModified;
    public ContactInfo[] Contacts;
    public readonly string CampaignInfo;
    private TaskStatus prevNonCompletedStatus = TaskStatus.Invalid;

    public TaskInfo(
      int taskID,
      string userid,
      TaskType taskType,
      TaskStatus taskStatus,
      string subject,
      string notes,
      DateTime dueDate,
      DateTime startDate,
      TaskPriority priority,
      DateTime creationTime,
      DateTime lastModified,
      ContactInfo[] contacts,
      int campaignStepID,
      string campaignInfo)
    {
      this.TaskID = taskID;
      this.UserID = userid;
      this.TaskType = taskType;
      this.TaskStatus = taskStatus;
      this.Subject = subject;
      this.Notes = notes;
      this.DueDate = dueDate;
      this.StartDate = startDate;
      this.Priority = priority;
      this.CreationTime = creationTime;
      this.lastModified = lastModified;
      this.CampaignStepID = campaignStepID;
      if (this.taskStatus != TaskStatus.Completed)
        this.prevNonCompletedStatus = this.taskStatus;
      this.CampaignInfo = campaignInfo;
      this.Contacts = contacts;
    }

    public TaskInfo(
      int taskID,
      string userid,
      TaskType taskType,
      TaskStatus taskStatus,
      string subject,
      string notes,
      DateTime dueDate,
      DateTime startDate,
      TaskPriority priority,
      DateTime creationTime,
      DateTime lastModified,
      ContactInfo[] contacts,
      int campaignStepID)
      : this(taskID, userid, taskType, taskStatus, subject, notes, dueDate, startDate, priority, creationTime, lastModified, contacts, campaignStepID, "")
    {
    }

    public TaskInfo(string userid, TaskType taskType, int campaignStepID)
    {
      this.TaskID = 0;
      this.UserID = userid;
      this.TaskType = taskType;
      this.TaskStatus = TaskStatus.NotStarted;
      this.Subject = "";
      this.Notes = "";
      this.DueDate = DateTime.MaxValue;
      this.StartDate = DateTime.MinValue;
      this.Priority = TaskPriority.Normal;
      this.CreationTime = DateTime.Now;
      this.lastModified = DateTime.MinValue;
      this.CampaignStepID = campaignStepID;
      if (this.taskStatus != TaskStatus.Completed)
        this.prevNonCompletedStatus = this.taskStatus;
      this.Contacts = new ContactInfo[0];
    }

    public TaskInfo(string userid, TaskType taskType)
      : this(userid, taskType, -1)
    {
    }

    public TaskStatus TaskStatus
    {
      get => this.taskStatus;
      set
      {
        this.taskStatus = value;
        if (this.taskStatus == TaskStatus.Completed)
          return;
        this.prevNonCompletedStatus = this.taskStatus;
      }
    }

    public TaskStatus PrevNonCompletedStatus => this.prevNonCompletedStatus;

    public DateTime LastModified => this.lastModified;

    public object this[string propertyName]
    {
      get
      {
        switch (propertyName.ToLower())
        {
          case "campaigninfo":
            return (object) this.CampaignInfo;
          case "campaignstepid":
            return (object) this.CampaignStepID;
          case "creationtime":
            return (object) this.CreationTime;
          case "duedate":
            return (object) this.DueDate;
          case "lastmodified":
            return (object) this.LastModified;
          case "notes":
            return (object) this.Notes;
          case "priority":
            return (object) this.Priority;
          case "startdate":
            return (object) this.StartDate;
          case "subject":
            return (object) this.Subject;
          case "taskid":
            return (object) this.TaskID;
          case "taskstatus":
            return (object) this.TaskStatus;
          case "tasktype":
            return (object) this.TaskType;
          case "userid":
            return (object) this.UserID;
          default:
            throw new ArgumentException("Invalid property name '" + propertyName + "'");
        }
      }
      set
      {
        switch (propertyName.ToLower())
        {
          case "taskstatus":
            this.TaskStatus = (TaskStatus) Enum.Parse(typeof (TaskStatus), string.Concat(value), true);
            break;
          case "subject":
            this.Subject = string.Concat(value);
            break;
          case "notes":
            this.Notes = string.Concat(value);
            break;
          case "duedate":
            this.DueDate = value == null ? DateTime.MaxValue : (DateTime) value;
            break;
          case "startdate":
            this.StartDate = value == null ? DateTime.MinValue : (DateTime) value;
            break;
          case "priority":
            this.Priority = (TaskPriority) Enum.Parse(typeof (TaskPriority), string.Concat(value), true);
            break;
          default:
            throw new ArgumentException("Invalid property name '" + propertyName + "'");
        }
      }
    }
  }
}
