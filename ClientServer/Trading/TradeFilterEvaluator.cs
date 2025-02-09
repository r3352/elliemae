// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.TradeFilterEvaluator
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public abstract class TradeFilterEvaluator : IFilterEvaluator
  {
    public static string[] RequiredFields = new string[14]
    {
      "Loan.CurrentMilestoneName",
      "Loan.LoanStatus",
      "Loan.TotalLoanAmount",
      "Loan.BorrowerFirstName",
      "Loan.BorrowerLastName",
      "Loan.LoanRate",
      "Loan.Investor",
      "TradeAssignment.Status",
      "TradeAssignment.TradeID",
      "Trade.Status",
      "Trade.Locked",
      "Loan.LoanFolder",
      "Trade.TradeType",
      "Loan.WithdrawnDate"
    };

    public TradeFilter Filter { get; private set; }

    public IFilterEvaluator UserEvaluator { get; protected set; }

    public TradeFilterEvaluator(TradeFilter filter)
    {
      this.Filter = filter;
      if (filter == null || filter.FilterType == TradeFilterType.None)
        this.UserEvaluator = (IFilterEvaluator) null;
      else if (filter.FilterType == TradeFilterType.Advanced)
        this.UserEvaluator = (IFilterEvaluator) filter.GetAdvancedFilter().CreateEvaluator();
      else
        this.UserEvaluator = (IFilterEvaluator) filter.GetSimpleFilter();
    }

    public List<EllieMae.EMLite.Workflow.Milestone> MilestoneSettings { get; set; }

    bool IFilterEvaluator.Evaluate(object value)
    {
      PipelineInfo pinfo = (PipelineInfo) value;
      return pinfo != null && this.Evaluate(pinfo);
    }

    bool IFilterEvaluator.Evaluate(object value, FilterEvaluationOption options)
    {
      PipelineInfo pinfo = (PipelineInfo) value;
      return pinfo != null && this.Evaluate(pinfo, options);
    }

    public QueryCriterion ToQueryCriterion()
    {
      return this.ToQueryCriterion((TradeInfoObj) null, FilterEvaluationOption.None);
    }

    public QueryCriterion ToQueryCriterion(FilterEvaluationOption options)
    {
      return this.ToQueryCriterion((TradeInfoObj) null, options);
    }

    public virtual QueryCriterion ToQueryCriterion(
      TradeInfoObj trade,
      EligibleLoanFilterOption filterOption = EligibleLoanFilterOption.All)
    {
      return this.ToQueryCriterion(trade, FilterEvaluationOption.None, filterOption);
    }

    public bool Evaluate(PipelineInfo pinfo)
    {
      string errMsg = string.Empty;
      return this.Evaluate(pinfo, (TradeViewModel) null, FilterEvaluationOption.None, out errMsg);
    }

    public bool Evaluate(PipelineInfo pinfo, FilterEvaluationOption options)
    {
      string errMsg = string.Empty;
      return this.Evaluate(pinfo, (TradeViewModel) null, options, out errMsg);
    }

    public bool Evaluate(PipelineInfo pinfo, FilterEvaluationOption options, out string errMsg)
    {
      return this.Evaluate(pinfo, (TradeViewModel) null, options, out errMsg);
    }

    public virtual bool Evaluate(
      PipelineInfo pinfo,
      TradeViewModel tradeObj,
      FilterEvaluationOption options,
      out string errMsg)
    {
      errMsg = string.Empty;
      return false;
    }

    public virtual QueryCriterion ToQueryCriterion(
      TradeInfoObj tradeObj,
      FilterEvaluationOption options,
      EligibleLoanFilterOption filterOption = EligibleLoanFilterOption.All)
    {
      return (QueryCriterion) null;
    }

    public bool MatchesAllCriteria(string fieldName, object value, FilterEvaluationOption options)
    {
      if (!this.Filter.MatchesAllUserCriteria(fieldName, value, options))
        return false;
      switch (fieldName.ToLower())
      {
        case "loan.currentmilestonename":
          return (options & FilterEvaluationOption.NonVolatile) != FilterEvaluationOption.None || string.Concat(value) != "Completion";
        case "loan.loanstatus":
          return Array.IndexOf<LoanStatusMap.LoanStatus>(LoanStatusMap.GetAdverseStatuses(), (LoanStatusMap.LoanStatus) Utils.ParseInt(value)) >= 0;
        case "loan.totalloanamount":
        case "loan.borrowerfirstname":
        case "loan.borrowerlastname":
        case "loan.loanrate":
          return string.Concat(value) != "";
        default:
          return true;
      }
    }
  }
}
