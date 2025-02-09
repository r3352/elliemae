// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.LastCompletedMilestoneNameField
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.Log;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class LastCompletedMilestoneNameField : VirtualField
  {
    private FieldOptionCollection options = new FieldOptionCollection();

    internal LastCompletedMilestoneNameField()
      : base("Log.MS.LastCompletedName", "Last Finished Milestone Name", FieldFormat.STRING)
    {
    }

    public override FieldOptionCollection Options => this.options;

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
      return milestoneLog == null ? "" : milestoneLog.Stage;
    }
  }
}
