// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.LoanTradeFilterEvaluator
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class LoanTradeFilterEvaluator(TradeFilter filter) : TradeFilterEvaluator(filter), IFilterEvaluator
  {
    public override bool Evaluate(
      PipelineInfo pinfo,
      TradeViewModel tradeObj,
      FilterEvaluationOption options,
      out string errMsg)
    {
      LoanTradeViewModel loanTradeViewModel = (LoanTradeViewModel) tradeObj;
      errMsg = "One or more loans assigned to this trade do not meet the eligibility criteria. Do you want to continue to save this trade?";
      if (this.UserEvaluator != null && !this.UserEvaluator.Evaluate((object) pinfo, options) || (options & FilterEvaluationOption.NonVolatile) == FilterEvaluationOption.None && (string.Compare(string.Concat(pinfo.GetField("CurrentMilestoneName")), "Completion", true) == 0 || string.Compare(string.Concat(pinfo.GetField("LoanFolder")), SystemSettings.TrashFolder, true) == 0))
        return false;
      if (this.Filter.FilterType == TradeFilterType.Simple && this.Filter.GetSimpleFilter().Milestones.Count > 0)
      {
        if (!this.Filter.GetSimpleFilter().Milestones.Exists((Predicate<string>) (m => string.Equals(m, pinfo.GetField("Loan.CurrentMilestoneName") as string))))
        {
          errMsg = "The trade has one or more loans assigned to it for which the milestone has changed or been removed. Do you want to continue to save this trade?";
          return false;
        }
        if (this.MilestoneSettings != null && this.MilestoneSettings.Where<EllieMae.EMLite.Workflow.Milestone>((Func<EllieMae.EMLite.Workflow.Milestone, bool>) (m => m.Archived)).Any<EllieMae.EMLite.Workflow.Milestone>((Func<EllieMae.EMLite.Workflow.Milestone, bool>) (m => string.Equals(m.Name, pinfo.GetField("Loan.CurrentMilestoneName") as string))))
        {
          errMsg = "The trade has one or more loans assigned to it for which the milestone has changed or been removed. Do you want to continue to save this trade?";
          return false;
        }
      }
      int[] enumValues = Utils.GetEnumValues((Array) LoanStatusMap.GetAdverseStatuses());
      int num = Utils.ParseInt(pinfo.GetField("LoanStatus"));
      for (int index = 0; index < enumValues.Length; ++index)
      {
        if (enumValues[index] == num)
          return false;
      }
      if (string.Concat(pinfo.GetField("TotalLoanAmount")) == "" || string.Concat(pinfo.GetField("BorrowerFirstName")) == "" || string.Concat(pinfo.GetField("BorrowerLastName")) == "" || string.Concat(pinfo.GetField("LoanRate")) == "")
        return false;
      if (pinfo.GetField("WithdrawnDate") != null && Utils.ParseDate(pinfo.GetField("WithdrawnDate")) != DateTime.MinValue)
      {
        errMsg = "The trade cannot be saved due to loan(s) having been withdrawn. You will need to go to the Loans tab and delete the highlighted loan prior to any other updates.";
        return false;
      }
      if (loanTradeViewModel != null && pinfo.IsRejectedByInvestor(loanTradeViewModel.InvestorName))
        return false;
      errMsg = string.Empty;
      return true;
    }

    public override QueryCriterion ToQueryCriterion(
      TradeInfoObj tradeObj,
      FilterEvaluationOption options,
      EligibleLoanFilterOption filterOption)
    {
      LoanTradeInfo loanTradeInfo = (LoanTradeInfo) tradeObj;
      Bitmask bitmask = new Bitmask((object) options);
      QueryCriterion queryCriterion1 = (QueryCriterion) new ListValueCriterion("Loan.LoanStatus", (Array) Utils.ToStringArray((Array) Utils.GetEnumValues((Array) LoanStatusMap.GetNonAdverseStatuses())));
      if (!bitmask.Contains((object) FilterEvaluationOption.NonVolatile))
        queryCriterion1 = queryCriterion1.And((QueryCriterion) new StringValueCriterion("Loan.CurrentMilestoneName", "Completion", StringMatchType.Exact, false));
      QueryCriterion queryCriterion2 = queryCriterion1.And((QueryCriterion) new OrdinalValueCriterion("Loan.TotalLoanAmount", (object) 0, OrdinalMatchType.GreaterThan)).And((QueryCriterion) new StringValueCriterion("Loan.BorrowerFirstName", "", StringMatchType.Exact, false)).And((QueryCriterion) new StringValueCriterion("Loan.BorrowerLastName", "", StringMatchType.Exact, false)).And((QueryCriterion) new OrdinalValueCriterion("Loan.LoanRate", (object) 0, OrdinalMatchType.GreaterThan));
      if (!bitmask.Contains((object) FilterEvaluationOption.NonVolatile))
        queryCriterion2 = queryCriterion2.And((QueryCriterion) new StringValueCriterion("Loan.LoanFolder", SystemSettings.TrashFolder, StringMatchType.Exact, false));
      if ((filterOption == EligibleLoanFilterOption.Rejected || filterOption == EligibleLoanFilterOption.All) && loanTradeInfo != null && (loanTradeInfo.InvestorName ?? "") != "")
        queryCriterion2 = queryCriterion2.And((QueryCriterion) new XRefValueCriterion("Loan.Guid", "TradeRejections.LoanGuid", (QueryCriterion) new StringValueCriterion("TradeRejections.Investor", loanTradeInfo.InvestorName, StringMatchType.CaseInsensitive), false));
      if (filterOption == EligibleLoanFilterOption.AlreadyAssigned)
      {
        QueryCriterion criterion = new StringValueCriterion("Loan.InvestorStatus", "", StringMatchType.Exact, true).Or((QueryCriterion) new StringValueCriterion("Loan.InvestorStatus", "Rejected", StringMatchType.Exact, true));
        queryCriterion2 = queryCriterion2.And(criterion);
      }
      QueryCriterion criterion1 = (QueryCriterion) new DateValueCriterion("Loan.WithdrawnDate", DateTime.MinValue);
      QueryCriterion queryCriterion3 = queryCriterion2.And(criterion1);
      if (filterOption != EligibleLoanFilterOption.AlreadyAssigned)
        ;
      if (filterOption == EligibleLoanFilterOption.All)
      {
        QueryCriterion queryCriterion4 = this.UserEvaluator.ToQueryCriterion(options);
        if (queryCriterion4 != null)
          queryCriterion3 = queryCriterion3.And(queryCriterion4);
      }
      return queryCriterion3;
    }
  }
}
