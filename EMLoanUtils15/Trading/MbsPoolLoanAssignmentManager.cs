// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.MbsPoolLoanAssignmentManager
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
  public class MbsPoolLoanAssignmentManager : 
    LoanToTradeAssignmentManagerBase,
    IEnumerable<MbsPoolLoanAssignment>,
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
    public Dictionary<string, MbsPoolLoanAssignment> loans = new Dictionary<string, MbsPoolLoanAssignment>();

    public MbsPoolLoanAssignmentManager(
      SessionObjects sessionObjects,
      int tradeId,
      bool isExternalOrganization)
      : this(sessionObjects, tradeId, sessionObjects.MbsPoolManager.GetAssignedOrPendingLoans(tradeId, MbsPoolLoanAssignmentManager.RequiredFields, isExternalOrganization))
    {
    }

    public MbsPoolLoanAssignmentManager(
      SessionObjects sessionObjects,
      int tradeId,
      PipelineInfo[] assignedLoans)
      : base(sessionObjects, tradeId, TradeType.MbsPool)
    {
      for (int index = 0; index < assignedLoans.Length; ++index)
      {
        MbsPoolLoanAssignment poolLoanAssignment = new MbsPoolLoanAssignment(this.sessionObjects, this.tradeId, assignedLoans[index]);
        this.loans[poolLoanAssignment.Guid] = poolLoanAssignment;
      }
    }

    public IEnumerator<MbsPoolLoanAssignment> GetEnumerator()
    {
      return (IEnumerator<MbsPoolLoanAssignment>) this.loans.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.loans.Values.GetEnumerator();

    public MbsPoolLoanAssignment GetTradeAssignment(string guid)
    {
      MbsPoolLoanAssignment tradeAssignment = (MbsPoolLoanAssignment) null;
      if (this.loans.ContainsKey(guid))
      {
        tradeAssignment = this.loans[guid];
        if (!tradeAssignment.PendingOrAssigned)
          tradeAssignment = (MbsPoolLoanAssignment) null;
      }
      return tradeAssignment;
    }

    public MbsPoolLoanAssignment[] GetCommittedAssignedLoans()
    {
      List<MbsPoolLoanAssignment> poolLoanAssignmentList = new List<MbsPoolLoanAssignment>();
      foreach (MbsPoolLoanAssignment poolLoanAssignment in this.loans.Values)
      {
        if (poolLoanAssignment.AssignedStatus >= MbsPoolLoanStatus.Assigned)
          poolLoanAssignmentList.Add(poolLoanAssignment);
      }
      return poolLoanAssignmentList.ToArray();
    }

    public MbsPoolLoanAssignment[] GetAssignedLoans()
    {
      List<MbsPoolLoanAssignment> poolLoanAssignmentList = new List<MbsPoolLoanAssignment>();
      foreach (MbsPoolLoanAssignment poolLoanAssignment in this.loans.Values)
      {
        if (poolLoanAssignment.Assigned)
          poolLoanAssignmentList.Add(poolLoanAssignment);
      }
      return poolLoanAssignmentList.ToArray();
    }

    public MbsPoolLoanAssignment[] GetAllAssignedPendingLoans()
    {
      List<MbsPoolLoanAssignment> poolLoanAssignmentList = new List<MbsPoolLoanAssignment>();
      foreach (MbsPoolLoanAssignment poolLoanAssignment in this.loans.Values)
        poolLoanAssignmentList.Add(poolLoanAssignment);
      return poolLoanAssignmentList.ToArray();
    }

    public PipelineInfo[] GetAssignedPipelineData()
    {
      List<PipelineInfo> pipelineInfoList = new List<PipelineInfo>();
      foreach (MbsPoolLoanAssignment assignedLoan in this.GetAssignedLoans())
        pipelineInfoList.Add(assignedLoan.PipelineInfo);
      return pipelineInfoList.ToArray();
    }

    public string[] GetAssignedLoanGuids()
    {
      List<string> stringList = new List<string>();
      foreach (MbsPoolLoanAssignment assignedLoan in this.GetAssignedLoans())
        stringList.Add(assignedLoan.PipelineInfo.GUID);
      return stringList.ToArray();
    }

    public string[] GetAllLoanGuids()
    {
      List<string> stringList = new List<string>();
      foreach (MbsPoolLoanAssignment poolLoanAssignment in this.loans.Values)
        stringList.Add(poolLoanAssignment.PipelineInfo.GUID);
      return stringList.ToArray();
    }

    public MbsPoolLoanAssignment[] GetPendingAndAssignedLoans()
    {
      List<MbsPoolLoanAssignment> poolLoanAssignmentList = new List<MbsPoolLoanAssignment>();
      foreach (MbsPoolLoanAssignment poolLoanAssignment in this.loans.Values)
      {
        if (poolLoanAssignment.Assigned || poolLoanAssignment.Pending)
          poolLoanAssignmentList.Add(poolLoanAssignment);
      }
      return poolLoanAssignmentList.ToArray();
    }

    public string[] GetAssignedAndRejectedLoanGuids()
    {
      List<string> stringList = new List<string>();
      foreach (MbsPoolLoanAssignment poolLoanAssignment in this.loans.Values)
      {
        if (poolLoanAssignment.Assigned || poolLoanAssignment.Rejected)
          stringList.Add(poolLoanAssignment.PipelineInfo.GUID);
      }
      return stringList.ToArray();
    }

    public string[] GetAssignedAndRejectedLoanNumbers()
    {
      List<string> stringList = new List<string>();
      foreach (MbsPoolLoanAssignment poolLoanAssignment in this.loans.Values)
      {
        if (poolLoanAssignment.Assigned || poolLoanAssignment.Rejected)
          stringList.Add(poolLoanAssignment.PipelineInfo.LoanNumber);
      }
      return stringList.ToArray();
    }

    public MbsPoolLoanAssignment[] GetRemovedLoans()
    {
      List<MbsPoolLoanAssignment> poolLoanAssignmentList = new List<MbsPoolLoanAssignment>();
      foreach (MbsPoolLoanAssignment poolLoanAssignment in this.loans.Values)
      {
        if (poolLoanAssignment.Removed)
          poolLoanAssignmentList.Add(poolLoanAssignment);
      }
      return poolLoanAssignmentList.ToArray();
    }

    public MbsPoolLoanAssignment[] GetPendingLoans()
    {
      List<MbsPoolLoanAssignment> poolLoanAssignmentList = new List<MbsPoolLoanAssignment>();
      foreach (MbsPoolLoanAssignment poolLoanAssignment in this.loans.Values)
      {
        if (poolLoanAssignment.Pending)
          poolLoanAssignmentList.Add(poolLoanAssignment);
      }
      return poolLoanAssignmentList.ToArray();
    }

    public MbsPoolLoanAssignment[] GetModifiedLoans()
    {
      List<MbsPoolLoanAssignment> poolLoanAssignmentList = new List<MbsPoolLoanAssignment>();
      foreach (MbsPoolLoanAssignment poolLoanAssignment in this.loans.Values)
      {
        if (poolLoanAssignment.Modified)
          poolLoanAssignmentList.Add(poolLoanAssignment);
      }
      return poolLoanAssignmentList.ToArray();
    }

    public bool HasPendingChanges()
    {
      foreach (MbsPoolLoanAssignment poolLoanAssignment in this.loans.Values)
      {
        if (poolLoanAssignment.Pending)
          return true;
      }
      return false;
    }

    public bool HasModifiedAssignments()
    {
      foreach (MbsPoolLoanAssignment poolLoanAssignment in this.loans.Values)
      {
        if (poolLoanAssignment.Modified)
          return true;
      }
      return false;
    }

    public MbsPoolLoanStatus GetLoanStatus(string guid)
    {
      return this.loans.ContainsKey(guid) ? this.loans[guid].Status : MbsPoolLoanStatus.Unassigned;
    }

    public bool IsAssigned(string guid) => this.GetLoanStatus(guid) != MbsPoolLoanStatus.Unassigned;

    public void AssignLoan(PipelineInfo pinfo, string commitmentContractNumber = "�", string productName = "�")
    {
      if (!this.loans.ContainsKey(pinfo.GUID))
        this.loans.Add(pinfo.GUID, new MbsPoolLoanAssignment(this.sessionObjects, this.tradeId, pinfo));
      MbsPoolLoanAssignment loan = this.loans[pinfo.GUID];
      loan.PendingStatus = MbsPoolLoanStatus.Assigned;
      loan.CommitmentContractNumber = commitmentContractNumber;
      loan.ProductName = productName;
    }

    public void RemoveLoan(string guid) => this.RemoveLoan(guid, false);

    public void RemoveLoan(string guid, bool rejected)
    {
      if (!this.loans.ContainsKey(guid))
        return;
      MbsPoolLoanAssignment loan = this.loans[guid];
      loan.PendingStatus = MbsPoolLoanStatus.Unassigned;
      if (rejected)
        loan.Rejected = true;
      if (loan.Pending)
        return;
      this.loans.Remove(guid);
    }

    public void ModifyLoanStatus(string guid, MbsPoolLoanStatus newStatus)
    {
      MbsPoolLoanAssignment poolLoanAssignment = this.loans.ContainsKey(guid) ? this.loans[guid] : throw new Exception("The specified loan is not currently a part of the trade");
      poolLoanAssignment.PendingStatus = newStatus;
      if (poolLoanAssignment.Pending || poolLoanAssignment.Status != MbsPoolLoanStatus.Unassigned)
        return;
      this.loans.Remove(guid);
    }

    public void WritePendingChangesToServer(bool removePendingLoan = false, bool forceUpdate = false)
    {
      List<string> stringList1 = new List<string>();
      List<MbsPoolLoanStatus> mbsPoolLoanStatusList = new List<MbsPoolLoanStatus>();
      List<string> stringList2 = new List<string>();
      List<string> stringList3 = new List<string>();
      List<string> stringList4 = new List<string>();
      MbsPoolLoanAssignment[] poolLoanAssignmentArray = this.GetModifiedLoans();
      if (forceUpdate)
        poolLoanAssignmentArray = this.GetPendingAndAssignedLoans();
      foreach (MbsPoolLoanAssignment poolLoanAssignment in poolLoanAssignmentArray)
      {
        stringList1.Add(poolLoanAssignment.Guid);
        mbsPoolLoanStatusList.Add(poolLoanAssignment.PendingStatus);
        stringList2.Add(poolLoanAssignment.RejectedReason);
        stringList3.Add(poolLoanAssignment.CommitmentContractNumber);
        stringList4.Add(poolLoanAssignment.ProductName);
      }
      this.sessionObjects.MbsPoolManager.SetPendingTradeStatuses(this.tradeId, stringList1.ToArray(), mbsPoolLoanStatusList.ToArray(), stringList2.ToArray(), stringList3.ToArray(), stringList4.ToArray(), false, removePendingLoan);
      foreach (MbsPoolLoanAssignment modifiedLoan in this.GetModifiedLoans())
        modifiedLoan.ClearModifications();
    }

    public void WriteCommitmentChangeToServer()
    {
      List<string> stringList1 = new List<string>();
      List<string> stringList2 = new List<string>();
      List<string> stringList3 = new List<string>();
      foreach (MbsPoolLoanAssignment pendingAndAssignedLoan in this.GetPendingAndAssignedLoans())
      {
        stringList1.Add(pendingAndAssignedLoan.Guid);
        stringList2.Add(pendingAndAssignedLoan.CommitmentContractNumber);
        stringList3.Add(pendingAndAssignedLoan.ProductName);
      }
      this.sessionObjects.MbsPoolManager.SetCommitmentInfo(this.tradeId, stringList1.ToArray(), stringList2.ToArray(), stringList3.ToArray(), false);
      foreach (MbsPoolLoanAssignment modifiedLoan in this.GetModifiedLoans())
        modifiedLoan.ClearModifications();
    }

    public void ApplyNewTradeID(int tradeId)
    {
      this.tradeId = tradeId;
      foreach (MbsPoolLoanAssignment poolLoanAssignment in this.loans.Values)
        poolLoanAssignment.ApplyNewTradeID(tradeId);
    }

    public void ValidateBeforeLoanAssignment(
      string[] loanNumbers,
      string investorName,
      out List<PipelineInfo> validPipelineInfos)
    {
      string[] array = ((IEnumerable<MbsPoolLoanAssignment>) this.GetCommittedAssignedLoans()).Select<MbsPoolLoanAssignment, string>((Func<MbsPoolLoanAssignment, string>) (a => a.PipelineInfo.LoanNumber)).ToArray<string>();
      List<TradeLoanUpdateError> source = this.ValidateLoanBeforeLoanAssignment(loanNumbers, array, investorName, (string) null, false, false, out validPipelineInfos);
      if (source.Count > 0)
      {
        string message = string.Join("; ", source.Select<TradeLoanUpdateError, string>((Func<TradeLoanUpdateError, string>) (e => e.Message)).ToArray<string>());
        throw new TradeLoanUpdateException(new TradeLoanUpdateError(string.Empty, (PipelineInfo) null, message));
      }
    }

    public List<TradeLoanUpdateError> ValidateCommitmentBeforeLoanAssignment(
      List<string[]> addedLoans,
      TradeAssignmentByTradeBase[] commitmentsInPool,
      FannieMaeProducts productNamesInPool,
      ref List<PipelineInfo> eligibleLoanInfos)
    {
      string message = string.Empty;
      List<TradeLoanUpdateError> tradeLoanUpdateErrorList = new List<TradeLoanUpdateError>();
      List<PipelineInfo> pipelineInfoList = new List<PipelineInfo>();
      List<string> commitmentContractNums = new List<string>();
      if (addedLoans == null || addedLoans.Count == 0)
        return tradeLoanUpdateErrorList;
      foreach (PipelineInfo pipelineInfo in eligibleLoanInfos)
      {
        PipelineInfo info = pipelineInfo;
        if (addedLoans.Any<string[]>((Func<string[], bool>) (addedLoan => addedLoan[0] == info.LoanNumber)))
          commitmentContractNums.Add(addedLoans.Where<string[]>((Func<string[], bool>) (addedLoan => addedLoan[0] == info.LoanNumber)).First<string[]>()[1]);
      }
      List<GSECommitmentInfo> source = this.sessionObjects.GseCommitmentManager.ValidateContractNumbers(commitmentContractNums);
      foreach (PipelineInfo pipelineInfo in eligibleLoanInfos)
      {
        PipelineInfo info = pipelineInfo;
        if (addedLoans.Any<string[]>((Func<string[], bool>) (addedLoan => addedLoan[0] == info.LoanNumber)))
        {
          string contractNumber = addedLoans.Where<string[]>((Func<string[], bool>) (addedLoan => addedLoan[0] == info.LoanNumber)).First<string[]>()[1];
          if (contractNumber != null && contractNumber.Trim() != string.Empty && source != null && !source.Any<GSECommitmentInfo>((Func<GSECommitmentInfo, bool>) (commitment => commitment.ContractNumber == contractNumber)))
          {
            message = string.Format("Loan {0} was not assigned because {1} is an invalid Commitment Contract #.", (object) info.LoanNumber, (object) addedLoans.Where<string[]>((Func<string[], bool>) (addedLoan => addedLoan[0] == info.LoanNumber)).First<string[]>()[1]);
            tradeLoanUpdateErrorList.Add(new TradeLoanUpdateError(string.Empty, (PipelineInfo) null, message));
          }
          else
          {
            GSECommitmentInfo theCommitment = new GSECommitmentInfo();
            if (contractNumber != null && contractNumber.Trim() != string.Empty && source != null)
              theCommitment = source.Where<GSECommitmentInfo>((Func<GSECommitmentInfo, bool>) (c => c.ContractNumber == contractNumber)).First<GSECommitmentInfo>();
            if (contractNumber != null && contractNumber.Trim() != string.Empty && (commitmentsInPool != null && !((IEnumerable<TradeAssignmentByTradeBase>) commitmentsInPool).Any<TradeAssignmentByTradeBase>((Func<TradeAssignmentByTradeBase, bool>) (c =>
            {
              int? tradeId1 = c.TradeID;
              int tradeId2 = theCommitment.TradeID;
              return tradeId1.GetValueOrDefault() == tradeId2 & tradeId1.HasValue;
            })) || commitmentsInPool == null))
            {
              message = string.Format("Loan {0} was not assigned because the GSE Commitment {1} is not allocated to this Fannie Mae PE MBS Pool.", (object) info.LoanNumber, (object) contractNumber);
              tradeLoanUpdateErrorList.Add(new TradeLoanUpdateError(string.Empty, (PipelineInfo) null, message));
            }
            else
            {
              string productName = addedLoans.Where<string[]>((Func<string[], bool>) (addedLoan => addedLoan[0] == info.LoanNumber)).First<string[]>()[2];
              if (productName != null && productName.Trim() != string.Empty)
              {
                bool flag1 = false;
                bool flag2 = false;
                if (productNamesInPool != null && !productNamesInPool.Any<FannieMaeProduct>((Func<FannieMaeProduct, bool>) (pn => pn.ProductName == productName)))
                  flag1 = true;
                if (!theCommitment.ProductNames.Any<FannieMaeProduct>((Func<FannieMaeProduct, bool>) (pn => pn.ProductName == productName)))
                  flag2 = true;
                if (flag1 | flag2)
                {
                  if (flag1 && !flag2)
                    message = string.Format("Loan {0} was not assigned because {1} is an invalid Product Name of the Fannie Mae PE MBS Pool.", (object) info.LoanNumber, (object) productName);
                  else if (!flag1 & flag2)
                    message = theCommitment.ProductNames.Count<FannieMaeProduct>() <= 0 ? string.Format("Loan {0} was not assigned because The Commitment Contract# is required when a Product Name is provided.", (object) info.LoanNumber) : string.Format("Loan {0} was not assigned because {1} is an invalid Product Name of the GSE Commitment {2}.", (object) info.LoanNumber, (object) productName, (object) contractNumber);
                  else if (flag1 & flag2)
                    message = string.Format("Loan {0} was not assigned because {1} is an invalid Product Name of both Fannie Mae PE MBS Pool and the GSE Commitment {2}.", (object) info.LoanNumber, (object) productName, (object) contractNumber);
                  tradeLoanUpdateErrorList.Add(new TradeLoanUpdateError(string.Empty, (PipelineInfo) null, message));
                  continue;
                }
              }
              pipelineInfoList.Add(info);
            }
          }
        }
      }
      eligibleLoanInfos = pipelineInfoList;
      return tradeLoanUpdateErrorList;
    }
  }
}
