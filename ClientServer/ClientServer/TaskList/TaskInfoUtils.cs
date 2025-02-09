// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.TaskList.TaskInfoUtils
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System.Collections;

#nullable disable
namespace EllieMae.EMLite.ClientServer.TaskList
{
  public class TaskInfoUtils
  {
    public const int NumValidStatuses = 5;
    public static readonly string[] TaskStatusStrings = new string[5]
    {
      "Not Started",
      "In Progress",
      "Completed",
      "Waiting on someone else",
      "Deferred"
    };
    private static readonly Hashtable taskStatusEnumValues = new Hashtable();
    public const int NumPriorities = 3;
    public static readonly string[] TaskPriorityStrings = new string[3]
    {
      "Low",
      "Normal",
      "High"
    };
    private static readonly Hashtable taskPriorityEnumValues = new Hashtable();

    private TaskInfoUtils()
    {
    }

    static TaskInfoUtils()
    {
      for (int index = 0; index < 5; ++index)
        TaskInfoUtils.taskStatusEnumValues.Add((object) TaskInfoUtils.TaskStatusStrings[index], (object) (TaskStatus) index);
      for (int index = 0; index < 3; ++index)
        TaskInfoUtils.taskPriorityEnumValues.Add((object) TaskInfoUtils.TaskPriorityStrings[index], (object) (TaskPriority) index);
    }

    public static TaskStatus TaskStatusEnumValue(string statusString)
    {
      object taskStatusEnumValue = TaskInfoUtils.taskStatusEnumValues[(object) statusString];
      return taskStatusEnumValue == null ? TaskStatus.Invalid : (TaskStatus) taskStatusEnumValue;
    }

    public static TaskPriority TaskPriorityEnumValue(string priorityString)
    {
      object priorityEnumValue = TaskInfoUtils.taskPriorityEnumValues[(object) priorityString];
      return priorityEnumValue == null ? TaskPriority.Invalid : (TaskPriority) priorityEnumValue;
    }
  }
}
