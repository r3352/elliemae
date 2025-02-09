// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.TradeBatchService
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.LoanUtils.Trading;
using EllieMae.EMLite.Trading;
using EllieMae.Encompass.BusinessObjects.TradeManagement;
using EllieMae.Encompass.Client;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects
{
  public class TradeBatchService : SessionBoundObject, ITradeBatchService
  {
    internal TradeBatchService(Session session)
      : base(session)
    {
    }

    public TradeQueueJob GetJobStatus(int jobId)
    {
      BatchJobInfo batchJob = ((IBatchJobsManager) this.Session.GetObject("BatchJobsManager")).GetBatchJob(jobId);
      TradeQueueJob jobStatus = (TradeQueueJob) null;
      if (batchJob != null)
      {
        jobStatus = new TradeQueueJob();
        jobStatus.BatchJobId = batchJob.BatchJobId;
        jobStatus.Channel = (ApplicationChannel) batchJob.ApplicationChannel;
        jobStatus.CreatedBy = batchJob.CreatedBy;
        jobStatus.CreatedDate = batchJob.CreatedDate;
        jobStatus.Status = this.ConvertJobStatus(batchJob.Status);
        jobStatus.TradeId = new int?(int.Parse(batchJob.EntityId));
        jobStatus.TradeType = this.GetTradeType(batchJob.EntityType);
        jobStatus.LoanCount = batchJob.BatchJobItems.Count<BatchJobItemInfo>();
        jobStatus.Result = batchJob.Result;
        foreach (BatchJobItemInfo batchJobItem in batchJob.BatchJobItems)
          jobStatus.JobItems.Add(new TradeQueueJobDetails()
          {
            BatchJobItemId = batchJobItem.BatchJobItemId,
            LoanGUID = batchJobItem.EntityId,
            Result = batchJobItem.Result,
            Status = this.ConvertJobItemStatus(batchJobItem.Status)
          });
      }
      return jobStatus;
    }

    public int CreateLoanUpdateJob(int tradeId, TradeType tradeType, bool allOrPendingLoans)
    {
      if (tradeType != TradeType.CorrespondentTrade)
        throw new Exception(tradeType.ToString() + " is not supported.");
      CorrespondentTradeManager.ValidateCorrespondentTradeSettings(this.Session.SessionObjects);
      ICorrespondentTradeManager icorrespondentTradeManager = (ICorrespondentTradeManager) this.Session.GetObject("CorrespondentTradeManager");
      CorrespondentTradeInfo trade = icorrespondentTradeManager.GetTrade(tradeId);
      this.ValidateTrade((TradeInfoObj) trade);
      PipelineInfo[] assignedOrPendingLoans = icorrespondentTradeManager.GetAssignedOrPendingLoans(tradeId, TradeLoanAssignment.GetFieldList().ToArray(), false);
      CorrespondentTradeLoanAssignmentManager assignmentManager = new CorrespondentTradeLoanAssignmentManager(this.Session.SessionObjects, tradeId, assignedOrPendingLoans);
      CorrespondentTradeLoanAssignment[] tradeLoanAssignmentArray = assignmentManager.GetAllAssignedPendingLoans();
      if (!allOrPendingLoans)
        tradeLoanAssignmentArray = assignmentManager.GetPendingLoans();
      return BatchJobManager.SubmitTradeBatchUpdateJob(trade, tradeLoanAssignmentArray, (BatchJobApplicationChannel) 2, this.Session.SessionObjects);
    }

    public int CreateLoanUpdateJob(int tradeId, TradeType tradeType, List<string> loanNumbers)
    {
      if (tradeType != TradeType.CorrespondentTrade)
        throw new Exception(tradeType.ToString() + " is not supported.");
      CorrespondentTradeManager.ValidateCorrespondentTradeSettings(this.Session.SessionObjects);
      ICorrespondentTradeManager icorrespondentTradeManager = (ICorrespondentTradeManager) this.Session.GetObject("CorrespondentTradeManager");
      CorrespondentTradeInfo trade = icorrespondentTradeManager.GetTrade(tradeId);
      this.ValidateTrade((TradeInfoObj) trade);
      PipelineInfo[] assignedOrPendingLoans = icorrespondentTradeManager.GetAssignedOrPendingLoans(tradeId, TradeLoanAssignment.GetFieldList().ToArray(), false);
      CorrespondentTradeLoanAssignment[] array = ((IEnumerable<CorrespondentTradeLoanAssignment>) new CorrespondentTradeLoanAssignmentManager(this.Session.SessionObjects, tradeId, assignedOrPendingLoans).GetAllAssignedPendingLoans()).Where<CorrespondentTradeLoanAssignment>((Func<CorrespondentTradeLoanAssignment, bool>) (x => loanNumbers.Contains(((LoanToTradeAssignmentBase) x).PipelineInfo.LoanNumber))).ToArray<CorrespondentTradeLoanAssignment>();
      return BatchJobManager.SubmitTradeBatchUpdateJob(trade, array, (BatchJobApplicationChannel) 2, this.Session.SessionObjects);
    }

    public TradeQueueJobSummary[] GetAllJobStatus()
    {
      BatchJobSummaryInfo[] allJobStatus = this.Session.SessionObjects.TradeSynchronizationManager.GetAllJobStatus();
      List<TradeQueueJobSummary> tradeQueueJobSummaryList = new List<TradeQueueJobSummary>();
      foreach (BatchJobSummaryInfo batchJobSummaryInfo in allJobStatus)
        tradeQueueJobSummaryList.Add(new TradeQueueJobSummary()
        {
          BatchJobId = batchJobSummaryInfo.BatchJobId,
          Channel = (ApplicationChannel) batchJobSummaryInfo.ApplicationChannel,
          CreatedBy = batchJobSummaryInfo.CreatedBy,
          CreatedDate = batchJobSummaryInfo.CreatedDate,
          Status = this.ConvertJobStatus(batchJobSummaryInfo.Status),
          TotalLoanCount = batchJobSummaryInfo.TotalJobItemsCount,
          TotalLoansUnProcessed = batchJobSummaryInfo.TotalJobItemsUnProcessed,
          TotalLoansCancelled = batchJobSummaryInfo.TotalJobItemsCancelled,
          TotalLoansErrored = batchJobSummaryInfo.TotalJobItemsErrored,
          TotalLoansInProgress = batchJobSummaryInfo.TotalJobItemsInProgress,
          TotalLoansSucceeded = batchJobSummaryInfo.TotalJobItemsSucceeded,
          TradeId = int.Parse(batchJobSummaryInfo.EntityId),
          TradeType = this.GetTradeType(batchJobSummaryInfo.EntityType)
        });
      return tradeQueueJobSummaryList.ToArray();
    }

    public bool CancelJob(int jobId)
    {
      return this.Session.SessionObjects.TradeSynchronizationManager.Cancel(jobId);
    }

    public bool RemoveJob(int jobId)
    {
      return this.Session.SessionObjects.TradeSynchronizationManager.Remove(jobId);
    }

    private bool ValidateTrade(TradeInfoObj tradeInfo)
    {
      if (tradeInfo == null)
        throw new Exception("Trade with Id " + (object) ((TradeBase) tradeInfo).TradeID + " does not exist.");
      if (tradeInfo.Status == 6 || tradeInfo.Status == 5)
        throw new Exception("Trade Trade with Id " + (object) ((TradeBase) tradeInfo).TradeID + " does is in Pending or Archived status.");
      return true;
    }

    private TradeType GetTradeType(BatchJobEntityType entityType)
    {
      if (entityType == 1)
        return TradeType.CorrespondentTrade;
      if (entityType == 2)
        return TradeType.LoanTrade;
      return entityType == 3 ? TradeType.MbsPool : TradeType.None;
    }

    private JobStatus ConvertJobStatus(BatchJobStatus status)
    {
      JobStatus jobStatus = JobStatus.None;
      if (status == 4)
        jobStatus = JobStatus.Cancelled;
      if (status == 3)
        jobStatus = JobStatus.Completed;
      if (status == 8 || status == 5)
        jobStatus = JobStatus.Error;
      if (status == 1)
        jobStatus = JobStatus.Created;
      if (status == 2)
        jobStatus = JobStatus.InProgress;
      if (status == 7)
        jobStatus = JobStatus.Created;
      if (status == 6)
        jobStatus = JobStatus.None;
      return jobStatus;
    }

    private JobItemStatus ConvertJobItemStatus(BatchJobItemStatus status)
    {
      JobItemStatus jobItemStatus = JobItemStatus.None;
      if (status == 4)
        jobItemStatus = JobItemStatus.Cancelled;
      if (status == 3)
        jobItemStatus = JobItemStatus.Completed;
      if (status == 5)
        jobItemStatus = JobItemStatus.Error;
      if (status == 1)
        jobItemStatus = JobItemStatus.Created;
      if (status == 2)
        jobItemStatus = JobItemStatus.InProgress;
      return jobItemStatus;
    }
  }
}
