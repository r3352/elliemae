// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Calendar.AppointmentInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Calendar
{
  [Serializable]
  public class AppointmentInfo : IPropertyDictionary
  {
    private string subject = string.Empty;
    private DateTime startDateTime = DateTime.MinValue;
    private DateTime endDateTime = DateTime.MinValue;
    private string description = string.Empty;
    private bool allDayEvent;
    private bool reminderEnabled;
    private int reminderInterval;
    private int reminderUnits;
    private string ownerKey = string.Empty;
    private string recurrenceId = string.Empty;
    private byte[] recurrence;
    private bool isRemoved;
    private DateTime originalStartDateTime = DateTime.MinValue;
    private string dataKey = string.Empty;
    private byte[] allProperties;
    private string userID = string.Empty;
    private DateTime lastModified = DateTime.MinValue;
    private CSMessage.AccessLevel accessLevel;

    public AppointmentInfo()
    {
    }

    public AppointmentInfo(
      string subject,
      DateTime startDateTime,
      DateTime endDateTime,
      string description,
      bool allDayEvent,
      bool reminderEnabled,
      int reminderInterval,
      int reminderUnits,
      string ownerKey,
      string recurrenceId,
      byte[] recurrence,
      bool isRemoved,
      DateTime originalStartDateTime,
      string dataKey,
      byte[] allProperties,
      string userID,
      DateTime lastModified,
      CSMessage.AccessLevel accessLevel)
    {
      this.subject = subject;
      this.startDateTime = startDateTime;
      this.endDateTime = endDateTime;
      this.description = description;
      this.allDayEvent = allDayEvent;
      this.reminderEnabled = reminderEnabled;
      this.reminderInterval = reminderInterval;
      this.reminderUnits = reminderUnits;
      this.ownerKey = ownerKey;
      this.recurrenceId = recurrenceId;
      this.recurrence = recurrence;
      this.isRemoved = isRemoved;
      this.originalStartDateTime = originalStartDateTime;
      this.dataKey = dataKey;
      this.allProperties = allProperties;
      this.userID = userID;
      this.lastModified = lastModified;
      this.accessLevel = accessLevel;
    }

    public object this[string columnName]
    {
      get
      {
        columnName = columnName.ToLower();
        switch (columnName)
        {
          case "alldayevent":
            return (object) this.allDayEvent;
          case "allproperties":
            return (object) this.allProperties;
          case "datakey":
            return (object) this.dataKey;
          case "description":
            return (object) this.description;
          case "enddatetime":
            return (object) this.endDateTime;
          case "isremoved":
            return (object) this.isRemoved;
          case "lastmodified":
            return (object) this.lastModified;
          case "originalstartdatetime":
            return (object) this.originalStartDateTime;
          case "ownerkey":
            return (object) this.ownerKey;
          case "recurrence":
            return (object) this.recurrence;
          case "recurrenceid":
            return (object) this.recurrenceId;
          case "reminderenabled":
            return (object) this.reminderEnabled;
          case "reminderinterval":
            return (object) this.reminderInterval;
          case "reminderunits":
            return (object) this.reminderUnits;
          case "startdatetime":
            return (object) this.startDateTime;
          case "subject":
            return (object) this.subject;
          case "userid":
            return (object) this.userID;
          default:
            return (object) null;
        }
      }
      set
      {
        columnName = columnName.ToLower();
        switch (columnName)
        {
          case "alldayevent":
            this.allDayEvent = Utils.ParseBoolean(value);
            break;
          case "datakey":
            this.dataKey = string.Concat(value);
            break;
          case "description":
            this.description = string.Concat(value);
            break;
          case "enddatetime":
            this.endDateTime = Utils.ParseDate(value);
            break;
          case "isremoved":
            this.isRemoved = Utils.ParseBoolean(value);
            break;
          case "lastmodified":
            this.lastModified = Utils.ParseDate(value);
            break;
          case "originalstartdatetime":
            this.originalStartDateTime = Utils.ParseDate(value);
            break;
          case "ownerkey":
            this.ownerKey = string.Concat(value);
            break;
          case "reminderenabled":
            this.reminderEnabled = Utils.ParseBoolean(value);
            break;
          case "reminderinterval":
            this.reminderInterval = Utils.ParseInt(value);
            break;
          case "reminderunits":
            this.reminderUnits = Utils.ParseInt(value);
            break;
          case "startdatetime":
            this.startDateTime = Utils.ParseDate(value);
            break;
          case "subject":
            this.subject = string.Concat(value);
            break;
          case "userid":
            this.userID = string.Concat(value);
            break;
          default:
            throw new NotSupportedException("The specified property name is not supported");
        }
      }
    }

    public string Subject
    {
      get => this.subject;
      set => this.subject = value;
    }

    public DateTime StartDateTime
    {
      get => this.startDateTime;
      set => this.startDateTime = value;
    }

    public DateTime EndDateTime
    {
      get => this.endDateTime;
      set => this.endDateTime = value;
    }

    public string Description
    {
      get => this.description;
      set => this.description = value;
    }

    public bool AllDayEvent
    {
      get => this.allDayEvent;
      set => this.allDayEvent = value;
    }

    public bool ReminderEnabled
    {
      get => this.reminderEnabled;
      set => this.reminderEnabled = value;
    }

    public int ReminderInterval
    {
      get => this.reminderInterval;
      set => this.reminderInterval = value;
    }

    public int ReminderUnits
    {
      get => this.reminderUnits;
      set => this.reminderUnits = value;
    }

    public string OwnerKey
    {
      get => this.ownerKey;
      set => this.ownerKey = value;
    }

    public string RecurrenceId
    {
      get => this.recurrenceId;
      set => this.recurrenceId = value;
    }

    public byte[] Recurrence
    {
      get => this.recurrence;
      set => this.recurrence = value;
    }

    public bool IsRemoved
    {
      get => this.isRemoved;
      set => this.isRemoved = value;
    }

    public bool IsVariance => this.recurrenceId != "";

    public DateTime OriginalStartDateTime
    {
      get => this.originalStartDateTime;
      set => this.originalStartDateTime = value;
    }

    public string DataKey
    {
      get => this.dataKey;
      set => this.dataKey = value;
    }

    public byte[] AllProperties
    {
      get => this.allProperties;
      set => this.allProperties = value;
    }

    public string UserID
    {
      get => this.userID;
      set => this.userID = value;
    }

    public DateTime LastModified => this.lastModified;

    public CSMessage.AccessLevel AccessLevel
    {
      get => this.accessLevel;
      set => this.accessLevel = value;
    }
  }
}
