// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.CorrespondentTradeLoanAssignmentManager
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
  public class CorrespondentTradeLoanAssignmentManager : 
    LoanToTradeAssignmentManagerBase,
    IEnumerable<CorrespondentTradeLoanAssignment>,
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
    public Dictionary<string, CorrespondentTradeLoanAssignment> loans = new Dictionary<string, CorrespondentTradeLoanAssignment>();
    private List<string> loanNumbers = new List<string>();
    public bool UpdateTotalPriceForAllLoans;

    public CorrespondentTradeLoanAssignmentManager(
      SessionObjects sessionObjects,
      int tradeId,
      bool isExternalOrganization)
      : this(sessionObjects, tradeId, sessionObjects.CorrespondentTradeManager.GetAssignedOrPendingLoans(tradeId, CorrespondentTradeLoanAssignmentManager.RequiredFields, isExternalOrganization))
    {
    }

    public CorrespondentTradeLoanAssignmentManager(
      SessionObjects sessionObjects,
      int tradeId,
      PipelineInfo[] assignedLoans)
      : base(sessionObjects, tradeId, TradeType.CorrespondentTrade)
    {
      for (int index = 0; index < assignedLoans.Length; ++index)
      {
        CorrespondentTradeLoanAssignment tradeLoanAssignment = new CorrespondentTradeLoanAssignment(this.sessionObjects, this.tradeId, assignedLoans[index]);
        this.loans[tradeLoanAssignment.Guid] = tradeLoanAssignment;
        if (tradeLoanAssignment.AssignedStatus.Equals((object) CorrespondentTradeLoanStatus.Assigned))
          this.loanNumbers.Add(assignedLoans[index].LoanNumber);
      }
      this.TradeType = TradeType.CorrespondentTrade;
    }

    public IEnumerator<CorrespondentTradeLoanAssignment> GetEnumerator()
    {
      return (IEnumerator<CorrespondentTradeLoanAssignment>) this.loans.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.loans.Values.GetEnumerator();

    public CorrespondentTradeLoanAssignment GetTradeAssignment(string guid)
    {
      CorrespondentTradeLoanAssignment tradeAssignment = (CorrespondentTradeLoanAssignment) null;
      if (this.loans.ContainsKey(guid))
      {
        tradeAssignment = this.loans[guid];
        if (!tradeAssignment.PendingOrAssigned)
          tradeAssignment = (CorrespondentTradeLoanAssignment) null;
      }
      return tradeAssignment;
    }

    public CorrespondentTradeLoanAssignment[] GetCommittedAssignedLoans()
    {
      List<CorrespondentTradeLoanAssignment> tradeLoanAssignmentList = new List<CorrespondentTradeLoanAssignment>();
      foreach (CorrespondentTradeLoanAssignment tradeLoanAssignment in this.loans.Values)
      {
        if (tradeLoanAssignment.AssignedStatus >= CorrespondentTradeLoanStatus.Assigned)
          tradeLoanAssignmentList.Add(tradeLoanAssignment);
      }
      return tradeLoanAssignmentList.ToArray();
    }

    public CorrespondentTradeLoanAssignment[] GetAssignedLoans()
    {
      List<CorrespondentTradeLoanAssignment> tradeLoanAssignmentList = new List<CorrespondentTradeLoanAssignment>();
      foreach (CorrespondentTradeLoanAssignment tradeLoanAssignment in this.loans.Values)
      {
        if (tradeLoanAssignment.Assigned)
          tradeLoanAssignmentList.Add(tradeLoanAssignment);
      }
      return tradeLoanAssignmentList.ToArray();
    }

    public string[] GetAssignedStatusLoanNumbers() => this.loanNumbers.ToArray();

    public CorrespondentTradeLoanAssignment[] GetAllAssignedPendingLoans()
    {
      List<CorrespondentTradeLoanAssignment> tradeLoanAssignmentList = new List<CorrespondentTradeLoanAssignment>();
      foreach (CorrespondentTradeLoanAssignment tradeLoanAssignment in this.loans.Values)
        tradeLoanAssignmentList.Add(tradeLoanAssignment);
      return tradeLoanAssignmentList.ToArray();
    }

    public PipelineInfo[] GetAssignedPipelineData()
    {
      List<PipelineInfo> pipelineInfoList = new List<PipelineInfo>();
      foreach (CorrespondentTradeLoanAssignment assignedLoan in this.GetAssignedLoans())
        pipelineInfoList.Add(assignedLoan.PipelineInfo);
      return pipelineInfoList.ToArray();
    }

    public string[] GetAssignedLoanGuids()
    {
      List<string> stringList = new List<string>();
      foreach (CorrespondentTradeLoanAssignment assignedLoan in this.GetAssignedLoans())
        stringList.Add(assignedLoan.PipelineInfo.GUID);
      return stringList.ToArray();
    }

    public string[] GetAllLoanGuids()
    {
      List<string> stringList = new List<string>();
      foreach (CorrespondentTradeLoanAssignment tradeLoanAssignment in this.loans.Values)
        stringList.Add(tradeLoanAssignment.PipelineInfo.GUID);
      return stringList.ToArray();
    }

    public CorrespondentTradeLoanAssignment[] GetPendingAndAssignedLoans()
    {
      List<CorrespondentTradeLoanAssignment> tradeLoanAssignmentList = new List<CorrespondentTradeLoanAssignment>();
      foreach (CorrespondentTradeLoanAssignment tradeLoanAssignment in this.loans.Values)
      {
        if (tradeLoanAssignment.Assigned || tradeLoanAssignment.Pending)
          tradeLoanAssignmentList.Add(tradeLoanAssignment);
      }
      return tradeLoanAssignmentList.ToArray();
    }

    public string[] GetAssignedAndRejectedLoanGuids()
    {
      List<string> stringList = new List<string>();
      foreach (CorrespondentTradeLoanAssignment tradeLoanAssignment in this.loans.Values)
      {
        if (tradeLoanAssignment.Assigned || tradeLoanAssignment.Rejected)
          stringList.Add(tradeLoanAssignment.PipelineInfo.GUID);
      }
      return stringList.ToArray();
    }

    public string[] GetAssignedAndRejectedLoanNumbers()
    {
      List<string> stringList = new List<string>();
      foreach (CorrespondentTradeLoanAssignment tradeLoanAssignment in this.loans.Values)
      {
        if (tradeLoanAssignment.Assigned || tradeLoanAssignment.Rejected)
          stringList.Add(tradeLoanAssignment.PipelineInfo.LoanNumber);
      }
      return stringList.ToArray();
    }

    public CorrespondentTradeLoanAssignment[] GetRemovedLoans()
    {
      List<CorrespondentTradeLoanAssignment> tradeLoanAssignmentList = new List<CorrespondentTradeLoanAssignment>();
      foreach (CorrespondentTradeLoanAssignment tradeLoanAssignment in this.loans.Values)
      {
        if (tradeLoanAssignment.Removed)
          tradeLoanAssignmentList.Add(tradeLoanAssignment);
      }
      return tradeLoanAssignmentList.ToArray();
    }

    public CorrespondentTradeLoanAssignment[] GetPendingLoans()
    {
      List<CorrespondentTradeLoanAssignment> tradeLoanAssignmentList = new List<CorrespondentTradeLoanAssignment>();
      foreach (CorrespondentTradeLoanAssignment tradeLoanAssignment in this.loans.Values)
      {
        if (tradeLoanAssignment.Pending)
          tradeLoanAssignmentList.Add(tradeLoanAssignment);
      }
      return tradeLoanAssignmentList.ToArray();
    }

    public CorrespondentTradeLoanAssignment[] GetModifiedLoans()
    {
      List<CorrespondentTradeLoanAssignment> tradeLoanAssignmentList = new List<CorrespondentTradeLoanAssignment>();
      foreach (CorrespondentTradeLoanAssignment tradeLoanAssignment in this.loans.Values)
      {
        if (tradeLoanAssignment.Modified)
          tradeLoanAssignmentList.Add(tradeLoanAssignment);
      }
      return tradeLoanAssignmentList.ToArray();
    }

    public bool HasPendingChanges()
    {
      foreach (CorrespondentTradeLoanAssignment tradeLoanAssignment in this.loans.Values)
      {
        if (tradeLoanAssignment.Pending)
          return true;
      }
      return false;
    }

    public bool HasModifiedAssignments()
    {
      foreach (CorrespondentTradeLoanAssignment tradeLoanAssignment in this.loans.Values)
      {
        if (tradeLoanAssignment.Modified)
          return true;
      }
      return false;
    }

    public CorrespondentTradeLoanStatus GetLoanStatus(string guid)
    {
      return this.loans.ContainsKey(guid) ? this.loans[guid].Status : CorrespondentTradeLoanStatus.Unassigned;
    }

    public bool IsAssigned(string guid)
    {
      return this.GetLoanStatus(guid) != CorrespondentTradeLoanStatus.Unassigned;
    }

    public void AssignLoan(PipelineInfo pinfo, Decimal totalPrice = 0M)
    {
      if (!this.loans.ContainsKey(pinfo.GUID))
        this.loans.Add(pinfo.GUID, new CorrespondentTradeLoanAssignment(this.sessionObjects, this.tradeId, pinfo));
      CorrespondentTradeLoanAssignment loan = this.loans[pinfo.GUID];
      loan.PendingStatus = CorrespondentTradeLoanStatus.Assigned;
      loan.TotalPrice = totalPrice;
    }

    public void RemoveLoan(string guid) => this.RemoveLoan(guid, false);

    public void RemoveLoan(string guid, bool rejected)
    {
      if (!this.loans.ContainsKey(guid))
        return;
      CorrespondentTradeLoanAssignment loan = this.loans[guid];
      loan.PendingStatus = CorrespondentTradeLoanStatus.Unassigned;
      if (rejected)
        loan.Rejected = true;
      if (loan.Pending)
        return;
      this.loans.Remove(guid);
    }

    public void ModifyLoanStatus(string guid, CorrespondentTradeLoanStatus newStatus)
    {
      CorrespondentTradeLoanAssignment tradeLoanAssignment = this.loans.ContainsKey(guid) ? this.loans[guid] : throw new Exception("The specified loan is not currently a part of the trade");
      tradeLoanAssignment.PendingStatus = newStatus;
      if (tradeLoanAssignment.Pending || tradeLoanAssignment.Status != CorrespondentTradeLoanStatus.Unassigned)
        return;
      this.loans.Remove(guid);
    }

    public void WritePendingChangesToServer(bool removePendingLoan = false)
    {
      List<string> stringList1 = new List<string>();
      List<CorrespondentTradeLoanStatus> correspondentTradeLoanStatusList = new List<CorrespondentTradeLoanStatus>();
      List<string> stringList2 = new List<string>();
      List<Decimal> numList = new List<Decimal>();
      foreach (CorrespondentTradeLoanAssignment tradeLoanAssignment in !this.UpdateTotalPriceForAllLoans ? this.GetModifiedLoans() : this.GetPendingAndAssignedLoans())
      {
        stringList1.Add(tradeLoanAssignment.Guid);
        correspondentTradeLoanStatusList.Add(tradeLoanAssignment.PendingStatus);
        if (tradeLoanAssignment.Rejected)
          stringList2.Add("voided");
        else
          stringList2.Add(tradeLoanAssignment.RejectedReason);
        numList.Add(tradeLoanAssignment.TotalPrice);
      }
      this.sessionObjects.CorrespondentTradeManager.SetPendingTradeStatuses(this.tradeId, stringList1.ToArray(), correspondentTradeLoanStatusList.ToArray(), stringList2.ToArray(), false, removePendingLoan, numList.ToArray(), this.UpdateTotalPriceForAllLoans);
      foreach (CorrespondentTradeLoanAssignment modifiedLoan in this.GetModifiedLoans())
        modifiedLoan.ClearModifications();
      this.UpdateTotalPriceForAllLoans = false;
    }

    public void ApplyNewTradeID(int tradeId)
    {
      this.tradeId = tradeId;
      foreach (CorrespondentTradeLoanAssignment tradeLoanAssignment in this.loans.Values)
        tradeLoanAssignment.ApplyNewTradeID(tradeId);
    }

    public string ValidateTradeBeforeLoanAssignment(
      string deliveryType,
      string tradeNumber,
      string tpoId,
      double totalLoanAmount)
    {
      Dictionary<int, string> correspondentTradeByLoanInfo = this.sessionObjects.CorrespondentTradeManager.GetEligibleCorrespondentTradeByLoanInfo(tpoId, deliveryType, totalLoanAmount);
      return correspondentTradeByLoanInfo != null && correspondentTradeByLoanInfo.ContainsValue(tradeNumber) ? string.Empty : string.Format("Loans were not assigned because the trade \"{0}\" is not eligible.", (object) tradeNumber);
    }

    public List<TradeLoanUpdateError> ValidateBeforeLoanAssignment(
      string deliveryType,
      string tradeNumber,
      string[] loanNumbers,
      string tpoId,
      bool isIndividual,
      bool isSkipValidation,
      out List<PipelineInfo> validPipelineInfos)
    {
      string[] array = ((IEnumerable<CorrespondentTradeLoanAssignment>) this.GetCommittedAssignedLoans()).Select<CorrespondentTradeLoanAssignment, string>((Func<CorrespondentTradeLoanAssignment, string>) (a => a.PipelineInfo.LoanNumber)).ToArray<string>();
      List<TradeLoanUpdateError> tradeLoanUpdateErrorList = this.ValidateLoanBeforeLoanAssignment(loanNumbers, array, (string) null, tpoId, isIndividual, isSkipValidation, out validPipelineInfos);
      double totalLoanAmount = (double) validPipelineInfos.Sum<PipelineInfo>((Func<PipelineInfo, Decimal>) (p => (Decimal) p.Info[(object) "TotalLoanAmount"]));
      string str = this.ValidateTradeBeforeLoanAssignment(deliveryType, tradeNumber, tpoId, totalLoanAmount);
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

    public List<TradeLoanUpdateError> ValidateLoansBeforeRemoveLoanAssignment(
      string[] loanNumbers,
      CorrespondentTradeInfo tradeInfo,
      SessionObjects sessionObjects,
      out List<PipelineInfo> validPipelineInfos)
    {
      List<TradeLoanUpdateError> tradeLoanUpdateErrorList = new List<TradeLoanUpdateError>();
      validPipelineInfos = new List<PipelineInfo>();
      PipelineInfo[] assignedOrPendingLoans = sessionObjects.CorrespondentTradeManager.GetAssignedOrPendingLoans(tradeInfo.TradeID, (string[]) null, false);
      string[] array = ((IEnumerable<PipelineInfo>) assignedOrPendingLoans).Select<PipelineInfo, string>((Func<PipelineInfo, string>) (l => l.LoanNumber)).ToArray<string>();
      foreach (string loanNumber1 in loanNumbers)
      {
        string loanNumber = loanNumber1;
        if (array != null && !((IEnumerable<string>) array).Contains<string>(loanNumber, (IEqualityComparer<string>) StringComparer.InvariantCultureIgnoreCase))
        {
          string message = string.Format("Loan {0} was not removed because this loan is not assigned to this trade.", (object) loanNumber);
          tradeLoanUpdateErrorList.Add(new TradeLoanUpdateError(loanNumber, (PipelineInfo) null, message));
        }
        else
        {
          PipelineInfo pipelineInfo = ((IEnumerable<PipelineInfo>) assignedOrPendingLoans).Where<PipelineInfo>((Func<PipelineInfo, bool>) (p => p.LoanNumber.Trim().ToLower() == loanNumber.Trim().ToLower())).First<PipelineInfo>();
          validPipelineInfos.Add(pipelineInfo);
        }
      }
      return tradeLoanUpdateErrorList;
    }

    public void VoidAssigedPendingLoanAssignment(int tradeId, string[] loanGuids)
    {
      foreach (string loanGuid in loanGuids)
        this.loans.Remove(loanGuid);
      this.sessionObjects.CorrespondentTradeManager.VoidAssigedPendingLoanAssignment(this.tradeId, loanGuids);
    }

    public void UpdateAssignementsWithTradeExtension(
      int tradeId,
      string[] loanGuids,
      string tradeExtensionInfo)
    {
      this.sessionObjects.CorrespondentTradeManager.UpdateAssignmentsWithTradeExtension(tradeId, loanGuids, tradeExtensionInfo);
    }
  }
}
