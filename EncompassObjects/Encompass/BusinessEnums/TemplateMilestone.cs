// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessEnums.TemplateMilestone
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

#nullable disable
namespace EllieMae.Encompass.BusinessEnums
{
  /// <summary>
  /// The TemplateMilestone represents a single Milestone that is part of a <see cref="T:EllieMae.Encompass.BusinessEnums.MilestoneTemplate" />.
  /// <remarks>The ID and Name properties refer to the ID and Name of the associated <see cref="T:EllieMae.Encompass.BusinessEnums.Milestone" />.
  /// As a result the ID and Name properties could appear out of order.</remarks>
  /// </summary>
  public class TemplateMilestone : EnumItem, ITemplateMilestone
  {
    private TemplateMilestone next;
    private TemplateMilestone prev;
    private string milestoneID = "";
    private EllieMae.EMLite.Workflow.TemplateMilestone templateMilestone;

    internal TemplateMilestone(
      EllieMae.EMLite.Workflow.TemplateMilestone templateMilestone,
      int id,
      string name,
      string milestoneID)
      : base(id, name)
    {
      this.templateMilestone = templateMilestone;
      this.milestoneID = milestoneID;
    }

    /// <summary>MilestoneID for the Milestone</summary>
    public string MilestoneID => this.milestoneID;

    /// <summary>
    /// Returns the number of days to completeion for the Milestone as part of the  <see cref="T:EllieMae.Encompass.BusinessEnums.MilestoneTemplate" />.
    /// </summary>
    public int DaysToComplete => this.templateMilestone.DaysToComplete;

    /// <summary>
    /// Gets the previous TemplateMilestone in the <see cref="T:EllieMae.Encompass.BusinessEnums.MilestoneTemplate" /> sequence that defines the lifetime of a loan.
    /// </summary>
    public TemplateMilestone Previous => this.prev;

    /// <summary>
    /// Gets the next TemplateMilestone in the <see cref="T:EllieMae.Encompass.BusinessEnums.MilestoneTemplate" /> sequence that defines the lifetime of a loan.
    /// </summary>
    public TemplateMilestone Next => this.next;

    /// <summary>
    /// Compares two TemplateMilestone to determine if one is after the other in the
    /// MilestoneTemplate.
    /// </summary>
    /// <param name="tm1">The first TemplateMilestone to compare.</param>
    /// <param name="tm2">The second TemplateMilestone to compare.</param>
    /// <returns>Returns true if <code>tm1</code> occurs after <code>tm2</code>
    /// in the MilestoneTemplate.</returns>
    /// <remarks>This operator will return false if either operand is null.</remarks>
    public static bool operator >(TemplateMilestone tm1, TemplateMilestone tm2)
    {
      if ((EnumItem) tm1 == (EnumItem) null || (EnumItem) tm2 == (EnumItem) null)
        return false;
      for (TemplateMilestone previous = tm1.Previous; (EnumItem) previous != (EnumItem) null; previous = previous.Previous)
      {
        if ((EnumItem) previous == (EnumItem) tm2)
          return true;
      }
      return false;
    }

    /// <summary>
    /// Compares two TemplateMilestone to determine if they are equal or if one occurs after
    /// the other in the MilestoneTemplate.
    /// </summary>
    /// <param name="tm1">The first TemplateMilestone to compare.</param>
    /// <param name="tm2">The second TemplateMilestone to compare.</param>
    /// <returns>Returns true if <code>tm1</code> and <code>tm2</code> are the same
    /// milestone or if <code>tm1</code> occurs after <code>tm2</code>
    /// in the MilestoneTemplate.</returns>
    public static bool operator >=(TemplateMilestone tm1, TemplateMilestone tm2)
    {
      return (EnumItem) tm1 == (EnumItem) tm2 || tm1 > tm2;
    }

    /// <summary>
    /// Compares two TemplateMilestone to determine if one occurs prior
    /// the other in the MilestoneTemplate.
    /// </summary>
    /// <param name="tm1">The first MilestoneTemplate to compare.</param>
    /// <param name="tm2">The second MilestoneTemplate to compare.</param>
    /// <returns>Returns true if <code>tm1</code> occurs before <code>tm2</code>
    /// in the MilestoneTemplate.</returns>
    /// <remarks>This operator will return true if either operand is null.</remarks>
    public static bool operator <(TemplateMilestone tm1, TemplateMilestone tm2) => !(tm1 >= tm2);

    /// <summary>
    /// Compares two TemplateMilestone to determine if they are equal or if one occurs before
    /// the MilestoneTemplate.
    /// </summary>
    /// <param name="tm1">The first MilestoneTemplate to compare.</param>
    /// <param name="tm2">The second MilestoneTemplate to compare.</param>
    /// <returns>Returns true if <code>tm1</code> and <code>tm2</code> are the same
    /// milestone or if <code>tm1</code> occurs before <code>tm2</code>
    /// in the MilestoneTemplate.</returns>
    public static bool operator <=(TemplateMilestone tm1, TemplateMilestone tm2) => !(tm1 > tm2);

    internal void SetNextTemplateMilestone(TemplateMilestone next) => this.next = next;

    internal void SetPreviousTemplateMilestone(TemplateMilestone prev) => this.prev = prev;
  }
}
