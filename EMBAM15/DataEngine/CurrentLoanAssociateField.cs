// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.CurrentLoanAssociateField
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.Log;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class CurrentLoanAssociateField : VirtualField
  {
    internal CurrentLoanAssociateField()
      : base("CurrentTeamMember", "Current Loan Team Member", FieldFormat.STRING)
    {
    }

    public override VirtualFieldType VirtualFieldType => VirtualFieldType.LoanAssociateFields;

    protected override string Evaluate(LoanData loan)
    {
      MilestoneLog milestoneLog = (MilestoneLog) null;
      foreach (MilestoneLog allMilestone in loan.GetLogList().GetAllMilestones())
      {
        if (allMilestone.RoleID >= RoleInfo.FileStarter.ID && (allMilestone.LoanAssociateID ?? "") != "")
          milestoneLog = allMilestone;
        if (!allMilestone.Done)
          break;
      }
      if (milestoneLog == null)
        return "";
      if (milestoneLog.LoanAssociateType == LoanAssociateType.User)
        return milestoneLog.LoanAssociateID;
      return milestoneLog.LoanAssociateType == LoanAssociateType.Group ? milestoneLog.LoanAssociateName : "";
    }
  }
}
