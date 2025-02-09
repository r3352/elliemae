// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.TaskMilestonePair
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using Elli.ElliEnum;
using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class TaskMilestonePair : MilestoneBase
  {
    public readonly string TaskGuid;
    public readonly bool isRequired;

    public string TaskName { get; set; }

    public string TaskDescription { get; set; }

    public int DaysToComplete { get; set; }

    public int TaskPriority { get; set; }

    public MilestoneTaskType TaskType { get; set; }

    public DateTime LastModifiedDate { get; set; }

    public TaskMilestonePair(
      string taskGuid,
      string milestoneID,
      bool isRequired,
      MilestoneTaskType taskType = MilestoneTaskType.MilestoneRequired,
      DateTime? lastModifiedDate = null)
      : base(milestoneID)
    {
      this.TaskGuid = taskGuid;
      this.isRequired = isRequired;
      this.TaskType = taskType;
      this.LastModifiedDate = (ValueType) lastModifiedDate == null ? DateTime.MinValue : Convert.ToDateTime((object) lastModifiedDate);
    }

    public TaskMilestonePair(
      string taskGuid,
      string milestoneId,
      bool isRequired,
      string taskName,
      string taskDescription,
      int daysToComplete,
      int taskPriority,
      MilestoneTaskType taskType = MilestoneTaskType.MilestoneRequired,
      DateTime? lastModifiedDate = null)
      : base(milestoneId)
    {
      this.TaskGuid = taskGuid;
      this.isRequired = isRequired;
      this.TaskType = taskType;
      this.LastModifiedDate = (ValueType) lastModifiedDate == null ? DateTime.MinValue : Convert.ToDateTime((object) lastModifiedDate);
      this.TaskName = taskName;
      this.TaskDescription = taskDescription;
      this.DaysToComplete = daysToComplete;
      this.TaskPriority = taskPriority;
    }

    public TaskMilestonePair(XmlSerializationInfo info)
      : base(info)
    {
      this.TaskGuid = info.GetString(nameof (TaskGuid));
      this.isRequired = info.GetBoolean("IsRequired");
    }

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      base.GetXmlObjectData(info);
      info.AddValue("TaskGuid", (object) this.TaskGuid);
      info.AddValue("IsRequired", (object) this.isRequired);
    }
  }
}
