// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.MilestoneTaskLog
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Xml;
using System;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class MilestoneTaskLog : LogRecordBase
  {
    public static readonly string XmlType = "Task";
    public static string TASKREQUIRED = "Required";
    public static string TASKOPTIONAL = "Optional";
    public static string TASKPRIORITYNORMAL = "Normal";
    private List<MilestoneTaskLog.TaskContact> contactList;
    private string taskGUID = string.Empty;
    private string taskName = string.Empty;
    private string taskDescription = string.Empty;
    private string stage = string.Empty;
    private DateTime completedDate;
    private string addedBy = string.Empty;
    private string addedByFullName = string.Empty;
    private string completedBy = string.Empty;
    private string completedByFullName = string.Empty;
    private bool isRequired;
    private string taskPriority = string.Empty;
    private int daysToComplete;
    private int daysToCompleteFromSetting = -1;
    private bool addedToLog = true;

    public MilestoneTaskLog(UserInfo addingUser, string taskName, string taskDescription)
    {
      this.taskName = taskName;
      this.taskDescription = taskDescription;
      this.date = DateTime.Now;
      this.addedBy = addingUser.Userid;
      this.addedByFullName = addingUser.FullName;
      this.contactList = new List<MilestoneTaskLog.TaskContact>();
    }

    public MilestoneTaskLog(LogList log, XmlElement e)
      : base(log, e)
    {
      AttributeReader attributeReader1 = new AttributeReader(e);
      this.taskGUID = attributeReader1.GetString(nameof (TaskGUID));
      this.taskName = attributeReader1.GetString(nameof (TaskName));
      this.taskDescription = attributeReader1.GetString(nameof (TaskDescription));
      this.completedDate = attributeReader1.GetDate(nameof (CompletedDate));
      this.stage = attributeReader1.GetString(nameof (Stage));
      this.addedByFullName = attributeReader1.GetString(nameof (AddedBy));
      this.addedBy = attributeReader1.GetString("AddedByUserID");
      this.completedByFullName = attributeReader1.GetString(nameof (CompletedBy));
      this.completedBy = attributeReader1.GetString("CompletedByUserID");
      this.isRequired = attributeReader1.GetBoolean(nameof (IsRequired));
      this.taskPriority = attributeReader1.GetString("Priority");
      this.daysToComplete = attributeReader1.GetInteger(nameof (DaysToComplete), 0);
      this.daysToCompleteFromSetting = attributeReader1.GetInteger(nameof (DaysToCompleteFromSetting), -1);
      this.contactList = new List<MilestoneTaskLog.TaskContact>();
      XmlNodeList xmlNodeList = e.SelectNodes("ContactList");
      for (int i = 0; i < xmlNodeList.Count; ++i)
      {
        AttributeReader attributeReader2 = new AttributeReader((XmlElement) xmlNodeList[i]);
        this.contactList.Add(new MilestoneTaskLog.TaskContact(attributeReader2.GetInteger("ContactID"), attributeReader2.GetString("ContactName"), attributeReader2.GetString("ContactRole"), attributeReader2.GetString("ContactPhone"), attributeReader2.GetString("ContactEmail"), attributeReader2.GetString("ContactAddress"), attributeReader2.GetString("ContactCity"), attributeReader2.GetString("ContactState"), attributeReader2.GetString("ContactZip"), attributeReader2.GetString("ContactGUID")));
      }
      this.MarkAsClean();
    }

    public override void ToXml(XmlElement e)
    {
      base.ToXml(e);
      AttributeWriter attributeWriter = new AttributeWriter(e);
      attributeWriter.Write("Type", (object) MilestoneTaskLog.XmlType);
      attributeWriter.Write("TaskGUID", (object) this.taskGUID);
      attributeWriter.Write("TaskName", (object) this.taskName);
      attributeWriter.Write("TaskDescription", (object) this.taskDescription);
      attributeWriter.Write("CompletedDate", (object) this.completedDate);
      attributeWriter.Write("Stage", (object) this.stage);
      attributeWriter.Write("AddedBy", (object) this.addedByFullName);
      attributeWriter.Write("AddedByUserID", (object) this.addedBy);
      attributeWriter.Write("CompletedBy", (object) this.completedByFullName);
      attributeWriter.Write("CompletedByUserID", (object) this.completedBy);
      attributeWriter.Write("IsRequired", (object) this.isRequired);
      attributeWriter.Write("Priority", (object) this.taskPriority);
      attributeWriter.Write("DaysToComplete", (object) this.daysToComplete);
      attributeWriter.Write("DaysToCompleteFromSetting", (object) this.daysToCompleteFromSetting);
      for (int index = 0; index < this.contactList.Count; ++index)
      {
        MilestoneTaskLog.TaskContact contact = this.contactList[index];
        XmlElement element = e.OwnerDocument.CreateElement("ContactList");
        element.SetAttribute("ContactID", contact.ContactID.ToString());
        element.SetAttribute("ContactName", contact.ContactName);
        element.SetAttribute("ContactRole", contact.ContactRole);
        element.SetAttribute("ContactPhone", contact.ContactPhone);
        element.SetAttribute("ContactEmail", contact.ContactEmail);
        element.SetAttribute("ContactAddress", contact.ContactAddress);
        element.SetAttribute("ContactCity", contact.ContactCity);
        element.SetAttribute("ContactState", contact.ContactState);
        element.SetAttribute("ContactZip", contact.ContactZip);
        element.SetAttribute("ContactGUID", contact.ContactGUID);
        e.AppendChild((XmlNode) element);
      }
    }

    internal override bool IsSystemSpecific() => true;

    public override PipelineInfo.Alert[] GetPipelineAlerts()
    {
      List<PipelineInfo.Alert> alertList = new List<PipelineInfo.Alert>();
      if (this.completedDate == DateTime.MinValue && this.ExpectedDate != DateTime.MinValue)
        alertList.Add(new PipelineInfo.Alert(20, this.taskName, "expected", this.ExpectedDate, this.Guid, this.Guid));
      PipelineInfo.Alert[] pipelineAlerts = this.AlertList.ToPipelineAlerts(StandardAlertID.TaskFollowUp, this.TaskName);
      if (pipelineAlerts != null && pipelineAlerts.Length != 0)
      {
        foreach (PipelineInfo.Alert alert in pipelineAlerts)
          alertList.Add(alert);
      }
      return alertList.ToArray();
    }

    public bool PastDue => this.ExpectedDate.Date > DateTime.Today;

    public void ClearContactList()
    {
      if (this.contactList != null)
        this.contactList.Clear();
      this.contactList = new List<MilestoneTaskLog.TaskContact>();
    }

    public void AddContact(MilestoneTaskLog.TaskContact contact)
    {
      if (contact == null)
        return;
      this.contactList.Add(contact);
    }

    public void AddContact(
      int contactID,
      string contactName,
      string contactRole,
      string contactPhone,
      string contactEmail,
      string contactAddress,
      string contactCity,
      string contactState,
      string contactZip)
    {
      this.contactList.Add(new MilestoneTaskLog.TaskContact(contactID, contactName, contactRole, contactPhone, contactEmail, contactAddress, contactCity, contactState, contactZip, ""));
    }

    public void RemoveContact(MilestoneTaskLog.TaskContact contact)
    {
      if (contact == null)
        return;
      this.contactList.Remove(contact);
    }

    public int ContactCount => this.contactList == null ? 0 : this.contactList.Count;

    public string GetAllContactNames()
    {
      string empty = string.Empty;
      for (int index = 0; index < this.contactList.Count; ++index)
      {
        if (empty != string.Empty)
          empty += ",";
        MilestoneTaskLog.TaskContact contact = this.contactList[index];
        empty += contact.ContactName;
      }
      return empty;
    }

    public string GetFirstContactName()
    {
      if (this.contactList == null || this.contactList.Count == 0)
        return string.Empty;
      MilestoneTaskLog.TaskContact contact = this.contactList[0];
      return this.contactList.Count > 1 ? contact.ContactName + ",..." : contact.ContactName;
    }

    public MilestoneTaskLog.TaskContact GetTaskContactAt(int i)
    {
      if (this.contactList == null)
        return (MilestoneTaskLog.TaskContact) null;
      return i + 1 > this.contactList.Count ? (MilestoneTaskLog.TaskContact) null : this.contactList[i];
    }

    public DateTime AddDate => this.date;

    public override DateTime Date
    {
      get
      {
        if (this.completedDate != DateTime.MinValue)
          return this.completedDate;
        return this.ExpectedDate != DateTime.MinValue ? this.ExpectedDate : this.date;
      }
      set
      {
        this.date = value;
        this.MarkAsDirty();
      }
    }

    public string TaskGUID
    {
      get => this.taskGUID;
      set => this.taskGUID = value;
    }

    public string TaskName
    {
      get => this.taskName;
      set => this.taskName = value;
    }

    public string TaskDescription
    {
      get => this.taskDescription;
      set => this.taskDescription = value;
    }

    public string Stage
    {
      get => this.stage;
      set => this.stage = value;
    }

    public DateTime CompletedDate
    {
      get => this.completedDate;
      set => this.completedDate = value;
    }

    public bool Completed => this.completedDate != DateTime.MinValue;

    public string AddedBy => this.addedBy;

    public string AddedByFullName => this.addedByFullName;

    public string CompletedBy => this.completedBy;

    public string CompletedByFullName => this.completedByFullName;

    public void SetCompletedByUser(UserInfo user)
    {
      if (user == (UserInfo) null)
      {
        this.completedBy = "";
        this.completedByFullName = "";
      }
      else
      {
        this.completedBy = user.Userid;
        this.completedByFullName = user.FullName;
      }
    }

    public bool IsRequired
    {
      get => this.isRequired;
      set => this.isRequired = value;
    }

    public string TaskPriority
    {
      get => this.taskPriority;
      set => this.taskPriority = value;
    }

    public int DaysToComplete
    {
      get => this.daysToComplete;
      set
      {
        this.daysToComplete = value;
        if (this.daysToComplete <= 0)
          return;
        this.daysToCompleteFromSetting = -1;
      }
    }

    public int DaysToCompleteFromSetting
    {
      get => this.daysToCompleteFromSetting;
      set => this.daysToCompleteFromSetting = value;
    }

    public bool AddedToLog
    {
      get => this.addedToLog;
      set => this.addedToLog = value;
    }

    public DateTime ExpectedDate
    {
      get
      {
        return this.daysToComplete > 0 ? this.AddDate.AddDays((double) this.daysToComplete) : DateTime.MinValue;
      }
    }

    public void MarkAsCompleted(DateTime dateCompleted, UserInfo completedBy)
    {
      this.completedBy = completedBy.Userid;
      this.completedByFullName = completedBy.FullName;
      this.completedDate = dateCompleted;
      this.MarkAsDirty();
    }

    public void UnmarkAsCompleted()
    {
      this.completedBy = "";
      this.completedByFullName = "";
      this.completedDate = DateTime.MinValue;
      this.MarkAsDirty();
    }

    public class TaskContact
    {
      private int contactID = -1;
      private string contactName = string.Empty;
      private string contactRole = string.Empty;
      private string contactPhone = string.Empty;
      private string contactEmail = string.Empty;
      private string contactAddress = string.Empty;
      private string contactCity = string.Empty;
      private string contactState = string.Empty;
      private string contactZip = string.Empty;
      private string contactGUID = string.Empty;

      public TaskContact(
        int contactID,
        string contactName,
        string contactRole,
        string contactPhone,
        string contactEmail,
        string contactAddress,
        string contactCity,
        string contactState,
        string contactZip,
        string contactGUID)
      {
        this.contactID = contactID;
        this.contactName = contactName;
        this.contactRole = contactRole;
        this.contactPhone = contactPhone;
        this.contactEmail = contactEmail;
        this.contactAddress = contactAddress;
        this.contactCity = contactCity;
        this.contactState = contactState;
        this.contactZip = contactZip;
        this.contactGUID = contactGUID == string.Empty ? System.Guid.NewGuid().ToString() : contactGUID;
      }

      public int ContactID
      {
        get => this.contactID;
        set => this.contactID = value;
      }

      public string ContactName
      {
        get => this.contactName;
        set => this.contactName = value;
      }

      public string ContactRole
      {
        get => this.contactRole;
        set => this.contactRole = value;
      }

      public string ContactPhone
      {
        get => this.contactPhone;
        set => this.contactPhone = value;
      }

      public string ContactEmail
      {
        get => this.contactEmail;
        set => this.contactEmail = value;
      }

      public string ContactAddress
      {
        get => this.contactAddress;
        set => this.contactAddress = value;
      }

      public string ContactCity
      {
        get => this.contactCity;
        set => this.contactCity = value;
      }

      public string ContactState
      {
        get => this.contactState;
        set => this.contactState = value;
      }

      public string ContactZip
      {
        get => this.contactZip;
        set => this.contactZip = value;
      }

      public string ContactGUID
      {
        get => this.contactGUID;
        set => this.contactGUID = value;
      }
    }
  }
}
