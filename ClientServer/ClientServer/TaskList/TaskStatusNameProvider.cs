// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.TaskList.TaskStatusNameProvider
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.ClientServer.TaskList
{
  public class TaskStatusNameProvider : CustomEnumNameProvider
  {
    private static Hashtable valueToNameMap = new Hashtable();

    static TaskStatusNameProvider()
    {
      TaskStatusNameProvider.valueToNameMap.Add((object) TaskStatus.NotStarted, (object) TaskInfoUtils.TaskStatusStrings[0]);
      TaskStatusNameProvider.valueToNameMap.Add((object) TaskStatus.InProgress, (object) TaskInfoUtils.TaskStatusStrings[1]);
      TaskStatusNameProvider.valueToNameMap.Add((object) TaskStatus.Completed, (object) TaskInfoUtils.TaskStatusStrings[2]);
      TaskStatusNameProvider.valueToNameMap.Add((object) TaskStatus.WaitingOnSomeoneElse, (object) TaskInfoUtils.TaskStatusStrings[3]);
      TaskStatusNameProvider.valueToNameMap.Add((object) TaskStatus.Deferred, (object) TaskInfoUtils.TaskStatusStrings[4]);
    }

    public TaskStatusNameProvider()
      : base(typeof (TaskStatus), TaskStatusNameProvider.valueToNameMap)
    {
    }
  }
}
