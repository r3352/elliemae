// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.BusinessRules.UserCondition
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Customization;
using EllieMae.EMLite.DataEngine.Log;
using System;

#nullable disable
namespace EllieMae.EMLite.BusinessRules
{
  [Serializable]
  public class UserCondition : PredefinedCondition
  {
    private string milestoneId;
    private string roleID;

    public UserCondition(string milestoneId, string roleID)
    {
      this.milestoneId = milestoneId;
      this.roleID = roleID;
    }

    public string MilestoneID => this.milestoneId;

    public string RoleID => this.roleID;

    public override bool AppliesTo(IExecutionContext icontext)
    {
      MilestoneLog[] allMilestones = ((ExecutionContext) icontext).Loan.GetLogList().GetAllMilestones();
      for (int index = 0; index < allMilestones.Length - 1; ++index)
      {
        if (allMilestones[index].MilestoneID == this.milestoneId)
        {
          if (index > 0 && allMilestones[index].Done && allMilestones[index].RoleID.ToString() == this.roleID && (allMilestones[index].LoanAssociateID ?? "") != "")
            return true;
          break;
        }
      }
      return false;
    }
  }
}
