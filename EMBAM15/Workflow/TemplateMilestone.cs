// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Workflow.TemplateMilestone
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Xml;
using System;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Workflow
{
  [Serializable]
  public class TemplateMilestone : IComparable<TemplateMilestone>
  {
    private string milestoneId;
    private int sortIndex;
    private int daysToComplete;
    private int roleId;
    private string milestoneName;
    private bool isArchived;

    public TemplateMilestone(string milestoneId, int sortIndex, int daysToComplete)
    {
      this.milestoneId = milestoneId;
      this.sortIndex = sortIndex;
      this.daysToComplete = daysToComplete;
    }

    public TemplateMilestone(string milestoneId, int sortIndex, int daysToComplete, int roleID)
    {
      this.milestoneId = milestoneId;
      this.sortIndex = sortIndex;
      this.daysToComplete = daysToComplete;
      this.roleId = roleID;
    }

    public TemplateMilestone(
      string milestoneId,
      int sortIndex,
      int daysToComplete,
      int roleID,
      string milestoneName,
      bool isArchived)
    {
      this.milestoneId = milestoneId;
      this.sortIndex = sortIndex;
      this.daysToComplete = daysToComplete;
      this.roleId = roleID;
      this.milestoneName = milestoneName;
      this.isArchived = isArchived;
    }

    public TemplateMilestone(XmlElement e)
    {
      AttributeReader attributeReader = new AttributeReader(e);
      this.milestoneId = attributeReader.GetString(nameof (MilestoneID));
      this.sortIndex = attributeReader.GetInteger(nameof (SortIndex));
      this.daysToComplete = attributeReader.GetInteger("Days");
      this.roleId = attributeReader.GetInteger(nameof (RoleID));
    }

    public string MilestoneID => this.milestoneId;

    public string MilestoneName => this.milestoneName;

    public bool Archived => this.isArchived;

    public int SortIndex => this.sortIndex;

    public int DaysToComplete
    {
      get => this.daysToComplete;
      set => this.daysToComplete = value;
    }

    public int RoleID
    {
      get => this.roleId;
      set => this.roleId = value;
    }

    internal void SetSortIndex(int index) => this.sortIndex = index;

    public override bool Equals(object obj)
    {
      return obj is TemplateMilestone templateMilestone && string.Compare(templateMilestone.MilestoneID, this.MilestoneID, true) == 0;
    }

    public override int GetHashCode() => this.milestoneId.ToLower().GetHashCode();

    int IComparable<TemplateMilestone>.CompareTo(TemplateMilestone other)
    {
      return other == null ? -1 : this.sortIndex - other.sortIndex;
    }
  }
}
