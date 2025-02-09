// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessEnums.Milestone
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System;

#nullable disable
namespace EllieMae.Encompass.BusinessEnums
{
  public class Milestone : EnumItem, IMilestone
  {
    private Milestone next;
    private Milestone prev;
    private string internalName = "";
    private bool isArchived;
    private Milestone coreMilestone;
    private string milestoneID = "";

    internal Milestone(int id, string name, bool isArchived, string milestoneID)
      : this(id, name)
    {
      this.isArchived = isArchived;
      this.milestoneID = milestoneID;
    }

    internal Milestone(int id, string name)
      : base(id, Milestone.fixCapitalization(name))
    {
      this.internalName = name;
    }

    internal Milestone(int id, string name, Milestone coreMilestone, bool isArchived)
      : this(id, name)
    {
      this.coreMilestone = coreMilestone;
      this.isArchived = isArchived;
    }

    public string MilestoneID => this.milestoneID;

    public Milestone Next => this.next;

    public Milestone Previous => this.prev;

    [Obsolete("There are no longer custom Milestones in Encompass 9.0 so all Milestones are basically custom Milestones. For backward compatibility this will return false for the previous core Milestones and true for all others.")]
    public bool IsCustom
    {
      get
      {
        return this.Name.ToUpper() != "STARTED" && this.Name.ToUpper() != "PROCESSING" && this.Name.ToUpper() != "SUBMITTAL" && this.Name.ToUpper() != "APPROVAL" && this.Name.ToUpper() != "DOCS SIGNING" && this.Name.ToUpper() != "FUNDING" && this.Name.ToUpper() != "COMPLETION";
      }
    }

    public bool IsArchived => this.isArchived;

    [Obsolete("Starting from Encompass 9.0 there is no longer a concept of \"Core\" Milestones. This property will always return null.")]
    public Milestone CoreMilestone => this.coreMilestone;

    public bool OccursBefore(Milestone ms) => !((EnumItem) ms == (EnumItem) null) && this < ms;

    public bool OccursOnOrBefore(Milestone ms) => !((EnumItem) ms == (EnumItem) null) && this <= ms;

    public bool OccursAfter(Milestone ms) => !((EnumItem) ms == (EnumItem) null) && this > ms;

    public bool OccursOnOrAfter(Milestone ms) => !((EnumItem) ms == (EnumItem) null) && this >= ms;

    public static bool operator >(Milestone m1, Milestone m2)
    {
      if ((EnumItem) m1 == (EnumItem) null || (EnumItem) m2 == (EnumItem) null)
        return false;
      for (Milestone previous = m1.Previous; (EnumItem) previous != (EnumItem) null; previous = previous.Previous)
      {
        if ((EnumItem) previous == (EnumItem) m2)
          return true;
      }
      return false;
    }

    public static bool operator >=(Milestone m1, Milestone m2)
    {
      return (EnumItem) m1 == (EnumItem) m2 || m1 > m2;
    }

    public static bool operator <(Milestone m1, Milestone m2) => !(m1 >= m2);

    public static bool operator <=(Milestone m1, Milestone m2) => !(m1 > m2);

    internal void SetNextMilestone(Milestone next) => this.next = next;

    internal void SetPreviousMilestone(Milestone prev) => this.prev = prev;

    internal string InternalName => this.internalName;

    private static string fixCapitalization(string name)
    {
      return name.Substring(0, 1).ToUpper() + name.Substring(1);
    }
  }
}
