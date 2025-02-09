// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.CustomMilestone
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using System;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  [Serializable]
  public class CustomMilestone
  {
    private string guid;
    private string name;
    private int days;
    private bool archived;

    public CustomMilestone(string guid, string name, int days)
    {
      this.guid = guid;
      this.name = name;
      this.days = days;
      this.archived = false;
    }

    public string Name
    {
      get => this.name;
      set => this.name = value;
    }

    public int Days
    {
      get => this.days;
      set => this.days = value;
    }

    public bool Archived
    {
      set => this.archived = value;
      get => this.archived;
    }

    public string Guid => this.MilestoneID;

    public string MilestoneName => this.Name;

    public string DisplayName
    {
      get => this.IsCoreMilestone ? MilestoneUIConfig.GetSettings(this.Name).StageText : this.Name;
    }

    public string MilestoneID => Milestone.GetCoreMilestoneID(this.Name) ?? this.guid;

    public bool IsCoreMilestone => Milestone.IsCoreMilestone(this.Name);

    public CoreMilestone ToCoreMilestone()
    {
      return this.IsCoreMilestone ? new CoreMilestone(this.Name) : (CoreMilestone) null;
    }

    public override string ToString() => this.Name;

    public override bool Equals(object obj)
    {
      return obj is CustomMilestone customMilestone && customMilestone.MilestoneID == this.MilestoneID;
    }

    public override int GetHashCode() => this.MilestoneID.GetHashCode();

    public static CustomMilestone FromCoreMilestone(string name)
    {
      return new CustomMilestone(Milestone.GetCoreMilestoneID(name), name, 0);
    }
  }
}
