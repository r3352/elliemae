// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.CorrespondentTradeFilterEvaluator
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using System;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class CorrespondentTradeFilterEvaluator(TradeFilter filter) : 
    TradeFilterEvaluator(filter),
    IFilterEvaluator
  {
    public override bool Evaluate(
      PipelineInfo pinfo,
      TradeViewModel tradeObj,
      FilterEvaluationOption options,
      out string errMsg)
    {
      CorrespondentTradeViewModel correspondentTradeViewModel = (CorrespondentTradeViewModel) tradeObj;
      errMsg = "One or more loans assigned to this trade do not meet the eligibility criteria. Do you want to continue to save this trade?";
      if (this.UserEvaluator != null && !this.UserEvaluator.Evaluate((object) pinfo, options) || (options & FilterEvaluationOption.NonVolatile) == FilterEvaluationOption.None && (string.Compare(string.Concat(pinfo.GetField("CurrentMilestoneName")), "Completion", true) == 0 || string.Compare(string.Concat(pinfo.GetField("LoanFolder")), SystemSettings.TrashFolder, true) == 0))
        return false;
      int[] enumValues = Utils.GetEnumValues((Array) LoanStatusMap.GetAdverseStatuses());
      int num = Utils.ParseInt(pinfo.GetField("LoanStatus"));
      for (int index = 0; index < enumValues.Length; ++index)
      {
        if (enumValues[index] == num)
          return false;
      }
      if (string.Concat(pinfo.GetField("TotalLoanAmount")) == "" || correspondentTradeViewModel != null && !correspondentTradeViewModel.DeliveryType.Contains("Bulk") && correspondentTradeViewModel.DeliveryType != "CoIssue" && (string.Concat(pinfo.GetField("BorrowerFirstName")) == "" || string.Concat(pinfo.GetField("BorrowerLastName")) == "") || string.Concat(pinfo.GetField("LoanRate")) == "")
        return false;
      if (pinfo.GetField("WithdrawnDate") != null && Utils.ParseDate(pinfo.GetField("WithdrawnDate")) != DateTime.MinValue)
      {
        errMsg = "The trade cannot be saved due to loan(s) having been withdrawn. You will need to go to the Loans tab and delete the highlighted loan prior to any other updates.";
        return false;
      }
      errMsg = string.Empty;
      return true;
    }

    public override QueryCriterion ToQueryCriterion(
      TradeInfoObj tradeObj,
      FilterEvaluationOption options,
      EligibleLoanFilterOption filterOption)
    {
      CorrespondentTradeInfo correspondentTradeInfo = (CorrespondentTradeInfo) tradeObj;
      Bitmask bitmask = new Bitmask((object) options);
      QueryCriterion queryCriterion1 = (QueryCriterion) new ListValueCriterion("Loan.LoanStatus", (Array) Utils.ToStringArray((Array) Utils.GetEnumValues((Array) LoanStatusMap.GetNonAdverseStatuses())));
      if (!bitmask.Contains((object) FilterEvaluationOption.NonVolatile))
        queryCriterion1 = queryCriterion1.And((QueryCriterion) new StringValueCriterion("Loan.CurrentMilestoneName", "Completion", StringMatchType.Exact, false));
      QueryCriterion queryCriterion2 = queryCriterion1.And((QueryCriterion) new OrdinalValueCriterion("Loan.TotalLoanAmount", (object) 0, OrdinalMatchType.GreaterThan));
      if (correspondentTradeInfo.DeliveryType != CorrespondentMasterDeliveryType.Bulk && correspondentTradeInfo.DeliveryType != CorrespondentMasterDeliveryType.BulkAOT && correspondentTradeInfo.DeliveryType != CorrespondentMasterDeliveryType.CoIssue)
        queryCriterion2 = queryCriterion2.And((QueryCriterion) new StringValueCriterion("Loan.BorrowerFirstName", "", StringMatchType.Exact, false)).And((QueryCriterion) new StringValueCriterion("Loan.BorrowerLastName", "", StringMatchType.Exact, false));
      QueryCriterion queryCriterion3 = queryCriterion2.And((QueryCriterion) new OrdinalValueCriterion("Loan.LoanRate", (object) 0, OrdinalMatchType.GreaterThan)).And((QueryCriterion) new StringValueCriterion("Loan.TPOCompanyID", correspondentTradeInfo.TPOID, StringMatchType.Exact, true));
      if (correspondentTradeInfo.IsForIndividualLoan())
        queryCriterion3 = queryCriterion3.And((QueryCriterion) new StringValueCriterion("Loan.LockStatus", "Locked", StringMatchType.Exact, true));
      QueryCriterion queryCriterion4 = queryCriterion3.And((QueryCriterion) new StringValueCriterion("Loan.loanChannel", 4.ToString(), StringMatchType.Exact, true));
      QueryCriterion criterion1 = new StringValueCriterion("Loan.CorrespondentTradeNumber", (string) null, StringMatchType.Exact, true).Or((QueryCriterion) new StringValueCriterion("Loan.CorrespondentTradeNumber", "", StringMatchType.Exact, true));
      QueryCriterion criterion2 = new StringValueCriterion("Loan.CorrespondentTradeGuid", (string) null, StringMatchType.Exact, true).Or((QueryCriterion) new StringValueCriterion("Loan.CorrespondentTradeGuid", "", StringMatchType.Exact, true));
      QueryCriterion queryCriterion5 = queryCriterion4.And(criterion1).And(criterion2);
      if (!bitmask.Contains((object) FilterEvaluationOption.NonVolatile))
        queryCriterion5 = queryCriterion5.And((QueryCriterion) new StringValueCriterion("Loan.LoanFolder", SystemSettings.TrashFolder, StringMatchType.Exact, false));
      QueryCriterion criterion3 = (QueryCriterion) new DateValueCriterion("Loan.WithdrawnDate", DateTime.MinValue);
      QueryCriterion queryCriterion6 = queryCriterion5.And(criterion3).And((QueryCriterion) new DateValueCriterion("loanSummaryExtension.VoidedDate", DateTime.MinValue));
      if (filterOption == EligibleLoanFilterOption.All)
      {
        QueryCriterion queryCriterion7 = this.UserEvaluator.ToQueryCriterion(options);
        if (queryCriterion7 != null)
          queryCriterion6 = queryCriterion6.And(queryCriterion7);
      }
      return queryCriterion6;
    }
  }
}
