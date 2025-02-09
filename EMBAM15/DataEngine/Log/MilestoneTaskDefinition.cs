// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.MilestoneTaskDefinition
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using System;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  [Serializable]
  public class MilestoneTaskDefinition
  {
    private string taskGUID = string.Empty;
    private string taskName = string.Empty;
    private string taskDescription = string.Empty;
    private int daysToComplete;
    private MilestoneTaskDefinition.MilestoneTaskPriority taskPriority = MilestoneTaskDefinition.MilestoneTaskPriority.Normal;

    public MilestoneTaskDefinition(
      string taskName,
      string taskDescription,
      int daysToComplete,
      MilestoneTaskDefinition.MilestoneTaskPriority taskPriority)
    {
      this.taskName = taskName;
      this.taskDescription = taskDescription;
      this.daysToComplete = daysToComplete;
      this.taskPriority = taskPriority;
    }

    [PgReady]
    public MilestoneTaskDefinition(DataRow r)
    {
      this.taskGUID = (string) r[nameof (taskGUID)];
      this.taskName = (string) r[nameof (taskName)];
      this.taskDescription = (string) r[nameof (taskDescription)];
      this.daysToComplete = Utils.ParseInt((object) r[nameof (daysToComplete)].ToString());
      this.taskPriority = (MilestoneTaskDefinition.MilestoneTaskPriority) Enum.Parse(typeof (MilestoneTaskDefinition.MilestoneTaskPriority), r[nameof (taskPriority)].ToString(), true);
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

    public int DaysToComplete
    {
      get => this.daysToComplete;
      set => this.daysToComplete = value;
    }

    public MilestoneTaskDefinition.MilestoneTaskPriority TaskPriority
    {
      get => this.taskPriority;
      set => this.taskPriority = value;
    }

    public enum MilestoneTaskPriority
    {
      Low,
      Normal,
      High,
    }
  }
}
