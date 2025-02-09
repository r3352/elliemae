// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.Tasks.ServerTask
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer.ServerTasks;
using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.Server.Tasks
{
  public class ServerTask
  {
    private int taskId;
    private int scheduleId;
    private ServerTaskType taskType;
    private string data;

    public ServerTask(int taskId, int scheduleId, ServerTaskType taskType, string data)
    {
      this.taskId = taskId;
      this.scheduleId = scheduleId;
      this.taskType = taskType;
      this.data = data;
    }

    public ServerTask(ServerTaskType taskType, string data)
      : this(-1, -1, taskType, data)
    {
    }

    public ServerTask(int scheduleId, ServerTaskType taskType, string data)
      : this(-1, -1, taskType, data)
    {
    }

    public ServerTask(ServerTaskType taskType, IXmlSerializable data)
      : this(taskType, new XmlSerializer().Serialize((object) data))
    {
    }

    public int TaskID => this.taskId;

    public int ScheduleID => this.scheduleId;

    public ServerTaskType TaskType => this.taskType;

    public string Data => this.data;

    public object GetDataObject(Type objectType)
    {
      return new XmlSerializer().Deserialize(this.data, objectType);
    }
  }
}
