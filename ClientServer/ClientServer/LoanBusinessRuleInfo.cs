// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.LoanBusinessRuleInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public class LoanBusinessRuleInfo
  {
    private const string className = "LoanBusinessRuleInfo�";
    private static readonly string sw = Tracing.SwDataEngine;
    public static readonly string[] RuleFields = new string[11]
    {
      "Loan.LoanType",
      "Loan.LoanPurpose",
      "Loan.RateIsLocked",
      "Loan.LoanStatus",
      "Loan.State",
      "Loan.LoanDocTypeCode",
      "Loan.LoanProgram",
      "Loan.CurrentMilestoneID",
      "Loan.CurrentMilestoneName",
      "Loan.CurrentCoreMilestoneName",
      "Loan.loanChannel"
    };
    private MilestoneLog[] milestonesInLoan;
    private MilestoneLog[] milestonesInLinkedLoan;
    private LoanData loanData;

    public LoanBusinessRuleInfo(LoanData loanData)
    {
      this.loanData = loanData;
      this.milestonesInLoan = this.loanData.GetLogList().GetAllMilestones();
      if (this.loanData.LinkedData == null)
        return;
      this.milestonesInLinkedLoan = this.loanData.LinkedData.GetLogList().GetAllMilestones();
    }

    public LoanConditions CurrentLoanForBusinessRule()
    {
      MilestoneLog msCheck = (MilestoneLog) null;
      MilestoneLog msToBeFinished = (MilestoneLog) null;
      this.getLoanMilestoneStatus(ref msCheck, ref msToBeFinished, this.loanData.GetLogList());
      return this.getCurrentLoanForBusinessRule(msCheck, msToBeFinished, this.loanData, this.milestonesInLoan);
    }

    public LoanConditions CurrentLoanForBusinessRule(
      MilestoneLog msCheck,
      MilestoneLog msToBeFinished)
    {
      return this.getCurrentLoanForBusinessRule(msCheck, msToBeFinished, this.loanData, this.milestonesInLoan);
    }

    public LoanConditions CurrentLinkedLoanForBusinessRule(
      MilestoneLog msCheck,
      MilestoneLog msToBeFinished)
    {
      return this.loanData.LinkedData == null ? (LoanConditions) null : this.getCurrentLoanForBusinessRule(msCheck, msToBeFinished, this.loanData.LinkedData, this.milestonesInLinkedLoan);
    }

    private LoanConditions getCurrentLoanForBusinessRule(
      MilestoneLog msCheck,
      MilestoneLog msToBeFinished,
      LoanData loanToCheck,
      MilestoneLog[] milestonesInThisLoan)
    {
      LoanConditions loanForBusinessRule = new LoanConditions();
      loanForBusinessRule.LoanTypeValue = BizRule.GetLoanTypeEnum(loanToCheck.GetField("1172"));
      loanForBusinessRule.LoanPurposeValue = BizRule.GetLoanPurposeEnum(loanToCheck.GetField("19"));
      loanForBusinessRule.RateLockValue = BizRule.GetRateLockEnum(loanToCheck.GetField("2400"));
      loanForBusinessRule.LoanStatusValue = BizRule.GetLoanStatusEnum(loanToCheck.GetField("1393"));
      loanForBusinessRule.ChannelValue = BizRule.GetChannelValue(loanToCheck.GetField("2626"));
      loanForBusinessRule.LoanProgramName = loanToCheck.GetField("1401") ?? "";
      loanForBusinessRule.StateCodeValue = USPS.StateCode.Unknown;
      if (USPS.StateCodes.ContainsKey((object) loanToCheck.GetField("14").Trim().ToUpper()))
        loanForBusinessRule.StateCodeValue = (USPS.StateCode) USPS.StateCodes[(object) loanToCheck.GetField("14").Trim().ToUpper()];
      loanForBusinessRule.DocTypeCodeValue = LoanDocTypeMap.GetCode(loanToCheck.GetField("MORNET.X67"));
      ArrayList arrayList1 = new ArrayList();
      ArrayList arrayList2 = new ArrayList();
      ArrayList arrayList3 = new ArrayList();
      int num = -1;
      for (int index = 0; index < milestonesInThisLoan.Length; ++index)
      {
        arrayList1.Add((object) milestonesInThisLoan[index].MilestoneID);
        if (msCheck != null && string.Compare(milestonesInThisLoan[index].MilestoneID, msCheck.MilestoneID, true) == 0 && index < milestonesInThisLoan.Length && milestonesInThisLoan[index].RoleID > RoleInfo.FileStarter.ID)
          num = milestonesInThisLoan[index].RoleID;
        if (milestonesInThisLoan[index].Done)
        {
          if (index < milestonesInThisLoan.Length)
          {
            arrayList2.Add((object) milestonesInThisLoan[index].MilestoneID);
            if (milestonesInThisLoan[index].RoleID < RoleInfo.FileStarter.ID || milestonesInThisLoan[index].LoanAssociateID != string.Empty)
              arrayList3.Add((object) milestonesInThisLoan[index].RoleID.ToString());
            else
              arrayList3.Add((object) "");
          }
          else
          {
            arrayList2.Add((object) milestonesInThisLoan[index].MilestoneID);
            if (index < milestonesInThisLoan.Length)
              arrayList3.Add((object) milestonesInThisLoan[index].RoleID.ToString());
            else
              arrayList3.Add((object) "");
          }
        }
        if (string.Compare(msToBeFinished.MilestoneID, milestonesInThisLoan[index].MilestoneID, true) == 0)
          break;
      }
      loanForBusinessRule.MilestonesToCheck = (string[]) arrayList1.ToArray(typeof (string));
      loanForBusinessRule.FinishedMilestones = (string[]) arrayList2.ToArray(typeof (string));
      loanForBusinessRule.FinishedRoleIDs = (string[]) arrayList3.ToArray(typeof (string));
      if (msCheck == null)
      {
        loanForBusinessRule.CurrentMilestoneID = "0";
        loanForBusinessRule.CurrentRoleID = (string) null;
      }
      else
      {
        loanForBusinessRule.CurrentMilestoneID = msToBeFinished.MilestoneID;
        loanForBusinessRule.CurrentRoleID = num <= RoleInfo.FileStarter.ID ? (string) null : num.ToString();
      }
      return loanForBusinessRule;
    }

    public static LoanConditions PipelineInfoForBusinessRule(PipelineInfo pInfo)
    {
      LoanConditions loanConditions = new LoanConditions();
      if (pInfo == null)
        return loanConditions;
      loanConditions.DataFromPlinth = true;
      loanConditions.LoanTypeValue = BizRule.GetLoanTypeEnum(pInfo.LoanType);
      loanConditions.LoanPurposeValue = BizRule.GetLoanPurposeEnum(pInfo.LoanPurpose);
      loanConditions.RateLockValue = BizRule.GetRateLockEnum(pInfo.GetField("RateIsLocked").ToString());
      loanConditions.LoanStatusValue = BizRule.GetLoanStatusEnumForPlinth(pInfo.GetField("loanStatus").ToString());
      loanConditions.LoanProgramName = string.Concat(pInfo.GetField("LoanProgram"));
      loanConditions.ChannelValue = string.Concat(pInfo.GetField("loanChannel"));
      string upper = pInfo.GetField("State").ToString().Trim().ToUpper();
      loanConditions.StateCodeValue = USPS.StateCode.Unknown;
      if (USPS.StateCodes.ContainsKey((object) upper))
        loanConditions.StateCodeValue = (USPS.StateCode) USPS.StateCodes[(object) upper];
      loanConditions.DocTypeCodeValue = LoanDocTypeMap.GetCodeForCodeString(string.Concat(pInfo.GetField("loanDocTypeCode")));
      string strB = "";
      string str1 = "1";
      if (pInfo.GetField("CurrentMilestoneName") != null && pInfo.GetField("CurrentMilestoneName").ToString() != "")
        strB = pInfo.GetField("CurrentMilestoneName").ToString();
      ArrayList arrayList1 = new ArrayList();
      ArrayList arrayList2 = new ArrayList();
      ArrayList arrayList3 = new ArrayList();
      string str2 = (string) null;
      for (int index = 0; index < pInfo.Milestones.Length; ++index)
      {
        if (string.Compare(pInfo.Milestones[index].MilestoneName, strB, true) == 0)
        {
          str1 = pInfo.Milestones[index].MilestoneID;
          if (index < pInfo.Milestones.Length && pInfo.Milestones[index].RoleID > RoleInfo.FileStarter.ID)
            str2 = pInfo.Milestones[index].RoleID.ToString();
        }
        if (pInfo.Milestones[index].Finished)
        {
          arrayList1.Add((object) pInfo.Milestones[index].MilestoneID);
          arrayList2.Add((object) pInfo.Milestones[index].MilestoneID);
          if (index < pInfo.Milestones.Length)
          {
            PipelineInfo.LoanAssociateInfo loanAssociate = pInfo.GetLoanAssociate(pInfo.Milestones[index].AssociateGuid);
            if (pInfo.Milestones[index].RoleID == RoleInfo.FileStarter.ID && loanAssociate != null)
              arrayList3.Add((object) loanAssociate.RoleID.ToString());
            else if (pInfo.Milestones[index].RoleID > RoleInfo.FileStarter.ID && loanAssociate != null)
              arrayList3.Add((object) pInfo.Milestones[index].RoleID.ToString());
            else if (index - 1 >= 0)
              arrayList3.Add(arrayList3[index - 1]);
            else
              arrayList3.Add((object) "");
          }
          else
            arrayList3.Add((object) "");
        }
      }
      loanConditions.MilestonesToCheck = (string[]) arrayList1.ToArray(typeof (string));
      loanConditions.FinishedMilestones = (string[]) arrayList2.ToArray(typeof (string));
      loanConditions.FinishedRoleIDs = (string[]) arrayList3.ToArray(typeof (string));
      loanConditions.CurrentMilestoneID = str1;
      loanConditions.CurrentRoleID = str2;
      return loanConditions;
    }

    public static LoanConditions PipelineInfoForBusinessRule(
      PipelineInfo pInfo,
      IEnumerable<EllieMae.EMLite.Workflow.Milestone> milestones)
    {
      LoanConditions loanConditions = new LoanConditions();
      if (pInfo == null)
        return loanConditions;
      loanConditions.DataFromPlinth = true;
      loanConditions.LoanTypeValue = BizRule.GetLoanTypeEnum(pInfo.LoanType);
      loanConditions.LoanPurposeValue = BizRule.GetLoanPurposeEnum(pInfo.LoanPurpose);
      loanConditions.RateLockValue = BizRule.GetRateLockEnum(string.Concat(pInfo.GetField("RateIsLocked")));
      loanConditions.LoanStatusValue = BizRule.GetLoanStatusEnumForPlinth(string.Concat(pInfo.GetField("loanStatus")));
      loanConditions.LoanProgramName = string.Concat(pInfo.GetField("LoanProgram"));
      loanConditions.ChannelValue = pInfo.GetField("loanChannel").ToString();
      string upper = string.Concat(pInfo.GetField("State")).Trim().ToUpper();
      loanConditions.StateCodeValue = USPS.StateCode.Unknown;
      if (USPS.StateCodes.ContainsKey((object) upper))
        loanConditions.StateCodeValue = (USPS.StateCode) USPS.StateCodes[(object) upper];
      loanConditions.DocTypeCodeValue = LoanDocTypeMap.GetCodeForCodeString(string.Concat(pInfo.GetField("loanDocTypeCode")));
      string field = (string) pInfo.GetField("CurrentMilestoneID");
      string coreMilestoneId = EllieMae.EMLite.Common.Milestone.GetCoreMilestoneID((string) pInfo.GetField("CurrentCoreMilestoneName"));
      string str1 = (string) null;
      ArrayList arrayList1 = new ArrayList();
      ArrayList arrayList2 = new ArrayList();
      ArrayList arrayList3 = new ArrayList();
      foreach (EllieMae.EMLite.Workflow.Milestone milestone in milestones)
      {
        int roleId;
        if (Utils.ParseInt((object) milestone.RoleID) > 0)
        {
          roleId = milestone.RoleID;
          str1 = roleId.ToString();
        }
        arrayList1.Add((object) milestone.MilestoneID);
        arrayList2.Add((object) milestone.MilestoneID);
        if (Utils.ParseInt((object) milestone.RoleID) > 0)
        {
          ArrayList arrayList4 = arrayList3;
          roleId = milestone.RoleID;
          string str2 = roleId.ToString();
          arrayList4.Add((object) str2);
        }
        else
          arrayList3.Add((object) "");
        if (string.Compare(milestone.MilestoneID, field, true) != 0)
        {
          if (EllieMae.EMLite.Common.Milestone.IsCoreMilestone(milestone.MilestoneID))
          {
            if (Utils.ParseInt((object) milestone.MilestoneID) > Utils.ParseInt((object) coreMilestoneId))
              break;
          }
        }
        else
          break;
      }
      loanConditions.MilestonesToCheck = (string[]) arrayList1.ToArray(typeof (string));
      loanConditions.FinishedMilestones = (string[]) arrayList2.ToArray(typeof (string));
      loanConditions.FinishedRoleIDs = (string[]) arrayList3.ToArray(typeof (string));
      loanConditions.CurrentMilestoneID = field;
      loanConditions.CurrentRoleID = str1;
      return loanConditions;
    }

    private void getLoanMilestoneStatus(
      ref MilestoneLog msCheck,
      ref MilestoneLog msToBeFinished,
      LogList logList)
    {
      MilestoneLog[] allMilestones = logList.GetAllMilestones();
      for (int index = 0; index < allMilestones.Length && allMilestones[index].Done; ++index)
      {
        if (index == allMilestones.Length - 1)
        {
          msCheck = allMilestones[index];
          msToBeFinished = msCheck;
        }
        else if (allMilestones[index + 1].RoleID < RoleInfo.FileStarter.ID || allMilestones[index + 1].LoanAssociateID != "")
        {
          msCheck = allMilestones[index];
          msToBeFinished = allMilestones[index + 1];
        }
      }
      msToBeFinished = logList.GetCurrentMilestone();
      if (msToBeFinished != null)
        return;
      msToBeFinished = msCheck;
    }
  }
}
