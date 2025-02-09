// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ServerTasks.ServerTaskProcessorStatus
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.ServerTasks
{
  [Serializable]
  public class ServerTaskProcessorStatus
  {
    private string processorID;
    private TaskProcessorStatus status;
    private string instanceName;
    private int taskID = -1;
    private int scheduleID = -1;
    private ServerTaskType taskType;
    private DateTime statusStartTime = DateTime.Now;

    public ServerTaskProcessorStatus(string processorID, TaskProcessorStatus status)
    {
      this.processorID = processorID;
      this.status = status;
    }

    public ServerTaskProcessorStatus(
      string processorID,
      string instanceName,
      int taskID,
      int scheduleID,
      ServerTaskType taskType)
    {
      this.processorID = processorID;
      this.status = TaskProcessorStatus.ProcessingTask;
      this.instanceName = instanceName;
      this.taskID = taskID;
      this.scheduleID = scheduleID;
      this.taskType = taskType;
    }

    public string ProcessorID => this.processorID;

    public TaskProcessorStatus Status => this.status;

    public string InstanceName => this.instanceName;

    public int TaskID => this.taskID;

    public int ScheduleID => this.scheduleID;

    public ServerTaskType TaskType => this.taskType;

    public DateTime StatusStartTime => this.statusStartTime;
  }
}
