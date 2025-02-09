// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.CoreMilestoneField
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.Log;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class CoreMilestoneField : VirtualField
  {
    private FieldOptionCollection options;

    internal CoreMilestoneField()
      : base("CoreMilestone", "Current Core Milestone", FieldFormat.STRING)
    {
      this.options = new FieldOptionCollection();
      foreach (string stage in Milestone.Stages)
        this.options.AddOption(MilestoneUIConfig.GetDisplayName(stage), stage);
      this.options.RequireValueFromList = true;
    }

    public override VirtualFieldType VirtualFieldType => VirtualFieldType.MilestoneFields;

    protected override string Evaluate(LoanData loan)
    {
      MilestoneLog milestoneLog = (MilestoneLog) null;
      foreach (MilestoneLog allMilestone in loan.GetLogList().GetAllMilestones())
      {
        if (allMilestone.Done)
          milestoneLog = allMilestone;
        else
          break;
      }
      if (milestoneLog == null)
        return "";
      if (!Utils.IsInt((object) milestoneLog.MilestoneID))
        return milestoneLog.Stage;
      switch (milestoneLog.MilestoneID)
      {
        case "1":
          return "Started";
        case "2":
          return "Processing";
        case "3":
          return "Submittal";
        case "4":
          return "Approval";
        case "5":
          return "Doc Signing";
        case "6":
          return "Funding";
        case "7":
          return "Completion";
        default:
          return milestoneLog.Stage;
      }
    }

    public override FieldOptionCollection Options => this.options;
  }
}
