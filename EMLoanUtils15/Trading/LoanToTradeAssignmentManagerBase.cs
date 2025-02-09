// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.LoanToTradeAssignmentManagerBase
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class LoanToTradeAssignmentManagerBase
  {
    protected SessionObjects sessionObjects;
    protected int tradeId = -1;

    public LoanToTradeAssignmentManagerBase(
      SessionObjects sessionObjects,
      int tradeId,
      TradeType tradeType)
    {
      this.sessionObjects = sessionObjects;
      this.tradeId = tradeId;
      this.TradeType = tradeType;
    }

    public int TradeID => this.tradeId;

    public TradeType TradeType { get; set; }

    private string GetNameOfTradeOrPool()
    {
      if (this.TradeType == TradeType.MbsPool)
        return "MBS Pool";
      if (this.TradeType == TradeType.LoanTrade)
        return "Loan Trade";
      return this.TradeType == TradeType.CorrespondentTrade ? "Correspondent Trade" : "trade/pool";
    }

    public List<TradeLoanUpdateError> ValidateLoanBeforeLoanAssignment(
      string[] loanNumbers,
      string[] excludedLoanNumbers,
      string investorName,
      string tpoId,
      bool isIndividual,
      bool isSkipValidation,
      out List<PipelineInfo> validPipelineInfos)
    {
      validPipelineInfos = new List<PipelineInfo>();
      string empty = string.Empty;
      List<TradeLoanUpdateError> tradeLoanUpdateErrorList = new List<TradeLoanUpdateError>();
      List<string> source1 = new List<string>();
      foreach (string loanNumber in loanNumbers)
      {
        if (excludedLoanNumbers != null && ((IEnumerable<string>) excludedLoanNumbers).Contains<string>(loanNumber, (IEqualityComparer<string>) StringComparer.InvariantCultureIgnoreCase))
        {
          string message = string.Format("Loan {0} was not assigned because this loan is already assigned to this {1}.", (object) loanNumber, (object) this.GetNameOfTradeOrPool());
          tradeLoanUpdateErrorList.Add(new TradeLoanUpdateError(string.Empty, (PipelineInfo) null, message));
        }
        else
          source1.Add(loanNumber);
      }
      foreach (IGrouping<int, \u003C\u003Ef__AnonymousType6<string, int>> source2 in source1.Select((s, i) => new
      {
        Value = s,
        Index = i
      }).GroupBy(o => o.Index / 100))
      {
        PipelineInfo[] loansByLoanNumbers = this.sessionObjects.LoanManager.GetLoansByLoanNumbers(source2.Select(l => l.Value).ToList<string>());
        foreach (var data in source2)
        {
          var loan = data;
          if (!((IEnumerable<PipelineInfo>) loansByLoanNumbers).Any<PipelineInfo>((Func<PipelineInfo, bool>) (p => string.Equals(p.LoanNumber, loan.Value, StringComparison.CurrentCultureIgnoreCase))))
          {
            string message = string.Format("Loan {0} was not assigned because {0} is an invalid loan number.", (object) loan.Value);
            tradeLoanUpdateErrorList.Add(new TradeLoanUpdateError(string.Empty, (PipelineInfo) null, message));
          }
        }
        IEnumerable<string> source3 = ((IEnumerable<PipelineInfo>) loansByLoanNumbers).GroupBy<PipelineInfo, string>((Func<PipelineInfo, string>) (p => p.LoanNumber)).Where<IGrouping<string, PipelineInfo>>((Func<IGrouping<string, PipelineInfo>, bool>) (g => g.Count<PipelineInfo>() > 1)).Select<IGrouping<string, PipelineInfo>, string>((Func<IGrouping<string, PipelineInfo>, string>) (g => g.Key));
        if (source3 != null && source3.Count<string>() > 0)
        {
          foreach (object obj in source3)
          {
            string message = string.Format("Loan {0} was not assigned because there are duplicated loans with the same loan number.", (object) obj.ToString());
            tradeLoanUpdateErrorList.Add(new TradeLoanUpdateError(string.Empty, (PipelineInfo) null, message));
          }
        }
        foreach (PipelineInfo pipelineInfo in loansByLoanNumbers)
        {
          string str = string.Format("Loan {0} was not assigned because ", (object) pipelineInfo.LoanNumber.ToString());
          bool flag = true;
          if (!source3.Contains<string>(pipelineInfo.LoanNumber))
          {
            if (!((IEnumerable<string>) Utils.ToStringArray((Array) Utils.GetEnumValues((Array) LoanStatusMap.GetNonAdverseStatuses()))).Contains<string>(pipelineInfo.Info[(object) "LoanStatus"].ToString()))
            {
              str += "the loan is in adverse status";
              flag = false;
            }
            if (pipelineInfo.Info[(object) "CurrentMilestoneName"] != null && string.Equals(pipelineInfo.Info[(object) "CurrentMilestoneName"].ToString(), "Completion", StringComparison.CurrentCultureIgnoreCase))
            {
              if (!flag)
                str += ", and ";
              str += "the loan is in completed milestone";
              flag = false;
            }
            if (Utils.ParseDecimal(pipelineInfo.Info[(object) "TotalLoanAmount"], 0M) <= 0M)
            {
              if (!flag)
                str += ", and ";
              str += "the loan amount is missing";
              flag = false;
            }
            if (!isSkipValidation && (pipelineInfo.Info[(object) "BorrowerFirstName"] != null && string.IsNullOrEmpty(pipelineInfo.Info[(object) "BorrowerFirstName"].ToString()) || pipelineInfo.Info[(object) "BorrowerLastName"] != null && string.IsNullOrEmpty(pipelineInfo.Info[(object) "BorrowerLastName"].ToString())))
            {
              if (!flag)
                str += ", and ";
              str += "borrower's first name or last name is missing";
              flag = false;
            }
            if (Utils.ParseDecimal(pipelineInfo.Info[(object) "LoanRate"], 0M) <= 0M)
            {
              if (!flag)
                str += ", and ";
              str += "note rate of the loan is missing";
              flag = false;
            }
            if (pipelineInfo.Info[(object) "LoanFolder"] != null && string.Equals(pipelineInfo.Info[(object) "LoanFolder"].ToString(), "(Trash)", StringComparison.CurrentCultureIgnoreCase))
            {
              if (!flag)
                str += ", and ";
              str += "the loan is in the trash folder";
              flag = false;
            }
            if (pipelineInfo.Info[(object) "WithdrawnDate"] != null)
            {
              if (!flag)
                str += ", and ";
              str += "the loan has been withdrawn";
              flag = false;
            }
            if (pipelineInfo.Info[(object) "VoidedDate"] != null)
            {
              if (!flag)
                str += ", and ";
              str += "loan file is in a voided status and could not be allocated to the trade";
              flag = false;
            }
            if (this.TradeType == TradeType.LoanTrade || this.TradeType == TradeType.MbsPool)
            {
              if (pipelineInfo.Info[(object) "TradeNumber"] != null && !string.IsNullOrEmpty(pipelineInfo.Info[(object) "TradeNumber"].ToString()) && pipelineInfo.Info[(object) "InvestorStatus"] != null && pipelineInfo.Info[(object) "InvestorStatus"].ToString().Trim() != string.Empty && pipelineInfo.Info[(object) "InvestorStatus"].ToString().Trim() != "Rejected")
              {
                if (!flag)
                  str += ", and ";
                str += "the loan is already assigned to a loan trade or mbs pool";
                flag = false;
              }
              if (!string.IsNullOrEmpty(investorName) && pipelineInfo.Info[(object) "InvestorStatus"] != null && pipelineInfo.Info[(object) "Investor"] != null && pipelineInfo.Info[(object) "InvestorStatus"].ToString().Trim() == "Rejected" && string.Equals(investorName, pipelineInfo.Info[(object) "Investor"].ToString().Trim(), StringComparison.CurrentCultureIgnoreCase))
              {
                if (!flag)
                  str += ", and ";
                str += "the loan is rejected by the current investor";
                flag = false;
              }
            }
            if (this.TradeType == TradeType.CorrespondentTrade)
            {
              if (string.IsNullOrEmpty(pipelineInfo.Info[(object) "TPOCompanyID"].ToString()) || !string.Equals(tpoId, pipelineInfo.Info[(object) "TPOCompanyID"].ToString(), StringComparison.CurrentCultureIgnoreCase))
              {
                if (!flag)
                  str += ", and ";
                str += "the loan's TPO ID is associated with a different Company";
                flag = false;
              }
              if (isIndividual && pipelineInfo.Info[(object) "LockStatus"].ToString() != "Locked")
              {
                if (!flag)
                  str += ", and ";
                str += "the loan is not lock and confirmed";
                flag = false;
              }
              if (Utils.ParseInt((object) pipelineInfo.Info[(object) "loanChannel"].ToString()) != 4)
              {
                if (!flag)
                  str += ", and ";
                str += "the loan's channel is not Correspondent";
                flag = false;
              }
              if (pipelineInfo.Info[(object) "CorrespondentTradeNumber"] != null && !string.IsNullOrEmpty(pipelineInfo.Info[(object) "CorrespondentTradeNumber"].ToString()))
              {
                if (!flag)
                  str += ", and ";
                str += "the loan is already assigned to a Correspondent Trade";
                flag = false;
              }
            }
            if (!flag)
              tradeLoanUpdateErrorList.Add(new TradeLoanUpdateError(string.Empty, (PipelineInfo) null, str + "."));
            else
              validPipelineInfos.Add(pipelineInfo);
          }
        }
      }
      return tradeLoanUpdateErrorList;
    }

    public List<TradeLoanUpdateError> ValidateLoansTotalPriceBeforeLoanAssignment(
      List<string[]> addedLoans,
      ref List<PipelineInfo> eligibleLoanInfos)
    {
      List<TradeLoanUpdateError> tradeLoanUpdateErrorList = new List<TradeLoanUpdateError>();
      List<PipelineInfo> pipelineInfoList = new List<PipelineInfo>();
      if (addedLoans == null || addedLoans.Count == 0)
        return tradeLoanUpdateErrorList;
      foreach (PipelineInfo pipelineInfo in eligibleLoanInfos)
      {
        PipelineInfo info = pipelineInfo;
        string[] strArray = addedLoans.Where<string[]>((Func<string[], bool>) (addedLoan => string.Equals(addedLoan[0], info.LoanNumber, StringComparison.OrdinalIgnoreCase))).First<string[]>();
        Decimal num = 0M;
        if (strArray.Length == 2)
          num = Utils.ParseDecimal((object) strArray[1]);
        if (num != 0M)
        {
          pipelineInfoList.Add(info);
        }
        else
        {
          string message = string.Format("Loan Number {0} does not have a valid Price {1}. Loan has been bypassed.", (object) info.LoanNumber, (object) strArray[1]);
          tradeLoanUpdateErrorList.Add(new TradeLoanUpdateError(string.Empty, (PipelineInfo) null, message));
        }
      }
      eligibleLoanInfos = pipelineInfoList;
      return tradeLoanUpdateErrorList;
    }
  }
}
