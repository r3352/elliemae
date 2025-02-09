// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.LoanTradeAssignmentManager
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.DataEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class LoanTradeAssignmentManager : 
    LoanToTradeAssignmentManagerBase,
    IEnumerable<LoanTradeAssignment>,
    IEnumerable
  {
    public static readonly string[] RequiredFields = new string[7]
    {
      "Loan.BorrowerFirstName",
      "Loan.BorrowerLastName",
      "Loan.LoanNumber",
      "TradeAssignment.TradeID",
      "TradeAssignment.AssignedStatus",
      "TradeAssignment.PendingStatus",
      "Trade.TradeType"
    };
    public Dictionary<string, LoanTradeAssignment> loans = new Dictionary<string, LoanTradeAssignment>();
    public bool UpdateAllLoans;

    public LoanTradeAssignmentManager(
      SessionObjects sessionObjects,
      int tradeId,
      bool isExternalOrganization)
      : this(sessionObjects, tradeId, sessionObjects.LoanTradeManager.GetAssignedOrPendingLoans(tradeId, LoanTradeAssignmentManager.RequiredFields, isExternalOrganization))
    {
    }

    public LoanTradeAssignmentManager(
      SessionObjects sessionObjects,
      int tradeId,
      PipelineInfo[] assignedLoans)
      : base(sessionObjects, tradeId, TradeType.LoanTrade)
    {
      for (int index = 0; index < assignedLoans.Length; ++index)
      {
        LoanTradeAssignment loanTradeAssignment = new LoanTradeAssignment(sessionObjects, tradeId, assignedLoans[index]);
        this.loans[loanTradeAssignment.Guid] = loanTradeAssignment;
      }
      this.TradeType = TradeType.LoanTrade;
    }

    public IEnumerator<LoanTradeAssignment> GetEnumerator()
    {
      return (IEnumerator<LoanTradeAssignment>) this.loans.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.loans.Values.GetEnumerator();

    public LoanTradeAssignment[] GetCommittedAssignedLoans()
    {
      List<LoanTradeAssignment> loanTradeAssignmentList = new List<LoanTradeAssignment>();
      foreach (LoanTradeAssignment loanTradeAssignment in this.loans.Values)
      {
        if (loanTradeAssignment.AssignedStatus >= LoanTradeStatus.Assigned)
          loanTradeAssignmentList.Add(loanTradeAssignment);
      }
      return loanTradeAssignmentList.ToArray();
    }

    public LoanTradeAssignment[] GetAllAssignedPendingLoans()
    {
      List<LoanTradeAssignment> loanTradeAssignmentList = new List<LoanTradeAssignment>();
      foreach (LoanTradeAssignment loanTradeAssignment in this.loans.Values)
        loanTradeAssignmentList.Add(loanTradeAssignment);
      return loanTradeAssignmentList.ToArray();
    }

    public LoanTradeAssignment[] GetPendingLoans()
    {
      List<LoanTradeAssignment> loanTradeAssignmentList = new List<LoanTradeAssignment>();
      foreach (LoanTradeAssignment loanTradeAssignment in this.loans.Values)
      {
        if (loanTradeAssignment.Pending)
          loanTradeAssignmentList.Add(loanTradeAssignment);
      }
      return loanTradeAssignmentList.ToArray();
    }

    public LoanTradeAssignment[] GetModifiedLoans()
    {
      List<LoanTradeAssignment> loanTradeAssignmentList = new List<LoanTradeAssignment>();
      foreach (LoanTradeAssignment loanTradeAssignment in this.loans.Values)
      {
        if (loanTradeAssignment.Modified)
          loanTradeAssignmentList.Add(loanTradeAssignment);
      }
      return loanTradeAssignmentList.ToArray();
    }

    public void AssignLoan(PipelineInfo pinfo, Decimal totalPrice = 0M)
    {
      if (!this.loans.ContainsKey(pinfo.GUID))
        this.loans.Add(pinfo.GUID, new LoanTradeAssignment(this.sessionObjects, this.tradeId, pinfo));
      LoanTradeAssignment loan = this.loans[pinfo.GUID];
      loan.PendingStatus = LoanTradeStatus.Assigned;
      loan.TotalPrice = totalPrice;
    }

    public void WritePendingChangesToServer(bool removePendingLoan = false)
    {
      List<string> stringList1 = new List<string>();
      List<LoanTradeStatus> loanTradeStatusList = new List<LoanTradeStatus>();
      List<string> stringList2 = new List<string>();
      List<Decimal> numList = new List<Decimal>();
      foreach (LoanTradeAssignment loanTradeAssignment in !this.UpdateAllLoans ? this.GetModifiedLoans() : this.GetPendingLoans())
      {
        stringList1.Add(loanTradeAssignment.Guid);
        loanTradeStatusList.Add(loanTradeAssignment.PendingStatus);
        stringList2.Add(loanTradeAssignment.RejectedReason);
        numList.Add(loanTradeAssignment.TotalPrice);
      }
      this.sessionObjects.LoanTradeManager.SetPendingTradeStatuses(this.tradeId, stringList1.ToArray(), loanTradeStatusList.ToArray(), stringList2.ToArray(), false, removePendingLoan, numList.ToArray(), this.UpdateAllLoans);
      foreach (LoanTradeAssignment modifiedLoan in this.GetModifiedLoans())
        modifiedLoan.ClearModifications();
      this.UpdateAllLoans = false;
    }

    public string ValidateTradeBeforeLoanAssignment(string tradeNumber)
    {
      Dictionary<int, string> loanTradeByLoanInfo = this.sessionObjects.LoanTradeManager.GetEligibleLoanTradeByLoanInfo();
      return loanTradeByLoanInfo != null && loanTradeByLoanInfo.ContainsValue(tradeNumber) ? string.Empty : string.Format("Loans were not assigned because the trade \"{0}\" is not eligible.", (object) tradeNumber);
    }

    public List<TradeLoanUpdateError> ValidateBeforeLoanAssignment(
      string tradeNumber,
      string[] loanNumbers,
      bool isIndividual,
      out List<PipelineInfo> validPipelineInfos)
    {
      string[] array = ((IEnumerable<LoanTradeAssignment>) this.GetCommittedAssignedLoans()).Select<LoanTradeAssignment, string>((Func<LoanTradeAssignment, string>) (a => a.PipelineInfo.LoanNumber)).ToArray<string>();
      List<TradeLoanUpdateError> tradeLoanUpdateErrorList = this.ValidateLoanBeforeLoanAssignment(loanNumbers, array, (string) null, (string) null, isIndividual, false, out validPipelineInfos);
      double num = (double) validPipelineInfos.Sum<PipelineInfo>((Func<PipelineInfo, Decimal>) (p => (Decimal) p.Info[(object) "TotalLoanAmount"]));
      string str = this.ValidateTradeBeforeLoanAssignment(tradeNumber);
      if (!string.IsNullOrEmpty(str))
      {
        tradeLoanUpdateErrorList = new List<TradeLoanUpdateError>()
        {
          new TradeLoanUpdateError()
          {
            LoanGuid = string.Empty,
            LoanInfo = (PipelineInfo) null,
            Message = str
          }
        };
        validPipelineInfos = new List<PipelineInfo>();
      }
      return tradeLoanUpdateErrorList;
    }
  }
}
