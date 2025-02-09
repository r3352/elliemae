// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.TradeAssignmentManager
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
  public class TradeAssignmentManager : 
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
    public bool UpdateTotalPriceForAllLoans;

    public TradeAssignmentManager(
      SessionObjects sessionObjects,
      int tradeId,
      bool isExternalOrganization)
      : this(sessionObjects, tradeId, sessionObjects.LoanTradeManager.GetAssignedOrPendingLoans(tradeId, TradeAssignmentManager.RequiredFields, isExternalOrganization))
    {
    }

    public TradeAssignmentManager(
      SessionObjects sessionObjects,
      int tradeId,
      PipelineInfo[] assignedLoans)
      : base(sessionObjects, tradeId, TradeType.LoanTrade)
    {
      for (int index = 0; index < assignedLoans.Length; ++index)
      {
        LoanTradeAssignment loanTradeAssignment = new LoanTradeAssignment(this.sessionObjects, this.tradeId, assignedLoans[index]);
        this.loans[loanTradeAssignment.Guid] = loanTradeAssignment;
      }
    }

    public IEnumerator<LoanTradeAssignment> GetEnumerator()
    {
      return (IEnumerator<LoanTradeAssignment>) this.loans.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.loans.Values.GetEnumerator();

    public LoanTradeAssignment GetTradeAssignment(string guid)
    {
      LoanTradeAssignment tradeAssignment = (LoanTradeAssignment) null;
      if (this.loans.ContainsKey(guid))
      {
        tradeAssignment = this.loans[guid];
        if (!tradeAssignment.PendingOrAssigned)
          tradeAssignment = (LoanTradeAssignment) null;
      }
      return tradeAssignment;
    }

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

    public LoanTradeAssignment[] GetAssignedLoans()
    {
      List<LoanTradeAssignment> loanTradeAssignmentList = new List<LoanTradeAssignment>();
      foreach (LoanTradeAssignment loanTradeAssignment in this.loans.Values)
      {
        if (loanTradeAssignment.Assigned)
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

    public PipelineInfo[] GetAssignedPipelineData()
    {
      List<PipelineInfo> pipelineInfoList = new List<PipelineInfo>();
      foreach (LoanTradeAssignment assignedLoan in this.GetAssignedLoans())
        pipelineInfoList.Add(assignedLoan.PipelineInfo);
      return pipelineInfoList.ToArray();
    }

    public string[] GetAssignedLoanGuids()
    {
      List<string> stringList = new List<string>();
      foreach (LoanTradeAssignment assignedLoan in this.GetAssignedLoans())
        stringList.Add(assignedLoan.PipelineInfo.GUID);
      return stringList.ToArray();
    }

    public string[] GetAllLoanGuids()
    {
      List<string> stringList = new List<string>();
      foreach (LoanTradeAssignment loanTradeAssignment in this.loans.Values)
        stringList.Add(loanTradeAssignment.PipelineInfo.GUID);
      return stringList.ToArray();
    }

    public LoanTradeAssignment[] GetPendingAndAssignedLoans()
    {
      List<LoanTradeAssignment> loanTradeAssignmentList = new List<LoanTradeAssignment>();
      foreach (LoanTradeAssignment loanTradeAssignment in this.loans.Values)
      {
        if (loanTradeAssignment.Assigned || loanTradeAssignment.Pending)
          loanTradeAssignmentList.Add(loanTradeAssignment);
      }
      return loanTradeAssignmentList.ToArray();
    }

    public string[] GetAssignedAndRejectedLoanGuids()
    {
      List<string> stringList = new List<string>();
      foreach (LoanTradeAssignment loanTradeAssignment in this.loans.Values)
      {
        if (loanTradeAssignment.Assigned || loanTradeAssignment.Rejected)
          stringList.Add(loanTradeAssignment.PipelineInfo.GUID);
      }
      return stringList.ToArray();
    }

    public string[] GetAssignedAndRejectedLoanNumbers()
    {
      List<string> stringList = new List<string>();
      foreach (LoanTradeAssignment loanTradeAssignment in this.loans.Values)
      {
        if (loanTradeAssignment.Assigned || loanTradeAssignment.Rejected)
          stringList.Add(loanTradeAssignment.PipelineInfo.LoanNumber);
      }
      return stringList.ToArray();
    }

    public LoanTradeAssignment[] GetRemovedLoans()
    {
      List<LoanTradeAssignment> loanTradeAssignmentList = new List<LoanTradeAssignment>();
      foreach (LoanTradeAssignment loanTradeAssignment in this.loans.Values)
      {
        if (loanTradeAssignment.Removed)
          loanTradeAssignmentList.Add(loanTradeAssignment);
      }
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

    public bool HasPendingChanges()
    {
      foreach (LoanTradeAssignment loanTradeAssignment in this.loans.Values)
      {
        if (loanTradeAssignment.Pending)
          return true;
      }
      return false;
    }

    public bool HasModifiedAssignments()
    {
      foreach (LoanTradeAssignment loanTradeAssignment in this.loans.Values)
      {
        if (loanTradeAssignment.Modified)
          return true;
      }
      return false;
    }

    public LoanTradeStatus GetLoanStatus(string guid)
    {
      return this.loans.ContainsKey(guid) ? this.loans[guid].Status : LoanTradeStatus.Unassigned;
    }

    public bool IsAssigned(string guid) => this.GetLoanStatus(guid) != LoanTradeStatus.Unassigned;

    public void AssignLoan(PipelineInfo pinfo, Decimal totalPrice = 0M)
    {
      if (!this.loans.ContainsKey(pinfo.GUID))
        this.loans.Add(pinfo.GUID, new LoanTradeAssignment(this.sessionObjects, this.tradeId, pinfo));
      LoanTradeAssignment loan = this.loans[pinfo.GUID];
      loan.PendingStatus = LoanTradeStatus.Assigned;
      loan.TotalPrice = totalPrice;
    }

    public void RemoveLoan(string guid) => this.RemoveLoan(guid, false);

    public void RemoveLoan(string guid, bool rejected)
    {
      if (!this.loans.ContainsKey(guid))
        return;
      LoanTradeAssignment loan = this.loans[guid];
      loan.PendingStatus = LoanTradeStatus.Unassigned;
      if (rejected)
        loan.Rejected = true;
      if (loan.Pending)
        return;
      this.loans.Remove(guid);
    }

    public void ModifyLoanStatus(string guid, LoanTradeStatus newStatus)
    {
      LoanTradeAssignment loanTradeAssignment = this.loans.ContainsKey(guid) ? this.loans[guid] : throw new Exception("The specified loan is not currently a part of the trade");
      loanTradeAssignment.PendingStatus = newStatus;
      if (loanTradeAssignment.Pending || loanTradeAssignment.Status != LoanTradeStatus.Unassigned)
        return;
      this.loans.Remove(guid);
    }

    public void WritePendingChangesToServer(bool removePendingLoan = false)
    {
      List<string> stringList1 = new List<string>();
      List<LoanTradeStatus> loanTradeStatusList = new List<LoanTradeStatus>();
      List<string> stringList2 = new List<string>();
      List<Decimal> numList = new List<Decimal>();
      foreach (LoanTradeAssignment loanTradeAssignment in !this.UpdateTotalPriceForAllLoans ? this.GetModifiedLoans() : this.GetPendingAndAssignedLoans())
      {
        stringList1.Add(loanTradeAssignment.Guid);
        loanTradeStatusList.Add(loanTradeAssignment.PendingStatus);
        stringList2.Add(loanTradeAssignment.RejectedReason);
        numList.Add(loanTradeAssignment.TotalPrice);
      }
      this.sessionObjects.LoanTradeManager.SetPendingTradeStatuses(this.tradeId, stringList1.ToArray(), loanTradeStatusList.ToArray(), stringList2.ToArray(), false, removePendingLoan, numList.ToArray(), this.UpdateTotalPriceForAllLoans);
      foreach (LoanTradeAssignment modifiedLoan in this.GetModifiedLoans())
        modifiedLoan.ClearModifications();
      this.UpdateTotalPriceForAllLoans = false;
    }

    public void ApplyNewTradeID(int tradeId)
    {
      this.tradeId = tradeId;
      foreach (LoanTradeAssignment loanTradeAssignment in this.loans.Values)
        loanTradeAssignment.ApplyNewTradeID(tradeId);
    }

    public void ValidateBeforeLoanAssignment(
      string[] loanNumbers,
      string investorName,
      out List<PipelineInfo> validPipelineInfos)
    {
      string[] array = ((IEnumerable<LoanTradeAssignment>) this.GetCommittedAssignedLoans()).Select<LoanTradeAssignment, string>((Func<LoanTradeAssignment, string>) (a => a.PipelineInfo.LoanNumber)).ToArray<string>();
      List<TradeLoanUpdateError> source = this.ValidateLoanBeforeLoanAssignment(loanNumbers, array, investorName, (string) null, false, false, out validPipelineInfos);
      if (source.Count > 0)
      {
        string message = string.Join("; ", source.Select<TradeLoanUpdateError, string>((Func<TradeLoanUpdateError, string>) (e => e.Message)).ToArray<string>());
        throw new TradeLoanUpdateException(new TradeLoanUpdateError(string.Empty, (PipelineInfo) null, message));
      }
    }
  }
}
