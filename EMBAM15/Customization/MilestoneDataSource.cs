// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Customization.MilestoneDataSource
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using System;

#nullable disable
namespace EllieMae.EMLite.Customization
{
  public class MilestoneDataSource(LoanData loan, UserInfo currentUser, bool readOnly) : 
    LoanDataSource(loan, currentUser, readOnly),
    IMilestoneDataSource
  {
    public bool IsComplete(string milestoneName)
    {
      MilestoneLog milestoneByName = this.getMilestoneByName(milestoneName);
      return milestoneByName != null && milestoneByName.Done;
    }

    public bool SetComplete(string milestoneName) => this.SetComplete(milestoneName, true);

    public bool SetComplete(string milestoneName, bool isComplete)
    {
      this.EnsureWritable();
      MilestoneLog milestoneByName = this.getMilestoneByName(milestoneName);
      if (milestoneByName == null)
        return false;
      if (milestoneByName.Done && !isComplete)
        milestoneByName.Done = false;
      else if (!milestoneByName.Done & isComplete)
      {
        milestoneByName.AdjustDate(DateTime.Now, true, true);
        milestoneByName.Done = true;
      }
      return true;
    }

    private MilestoneLog getMilestoneByName(string milestoneName)
    {
      foreach (MilestoneLog allMilestone in this.Loan.GetLogList().GetAllMilestones())
      {
        if (string.Compare(allMilestone.Stage, milestoneName, true) == 0)
          return allMilestone;
      }
      return (MilestoneLog) null;
    }
  }
}
