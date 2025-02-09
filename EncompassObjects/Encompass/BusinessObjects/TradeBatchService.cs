// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.TradeBatchService
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

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
  /// <summary>TradeBatchService</summary>
  public class TradeBatchService : SessionBoundObject, ITradeBatchService
  {
    internal TradeBatchService(Session session)
      : base(session)
    {
    }

    /// <summary>Returns status of submitted Job.</summary>
    /// <param name="jobId">Job Id</param>
    /// <returns><see cref="T:EllieMae.Encompass.BusinessObjects.TradeQueueJob" />Returns Complete Job status based on jobId.</returns>
    /// <remarks>Returns status of submiited job.</remarks>
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

    /// <summary>Returns Id of Job if submitted successfully.</summary>
    /// <param name="tradeId">Trade Id</param>
    /// <param name="tradeType">Trade Type</param>
    /// <param name="allOrPendingLoans">Set to true for All assinged loans, false for only Pending loans.</param>
    /// <returns><see cref="T:System.Int32" />Job Id.</returns>
    /// <remarks>Returns Id of Job if submitted successfully.</remarks>
    public int CreateLoanUpdateJob(int tradeId, EllieMae.Encompass.BusinessObjects.TradeManagement.TradeType tradeType, bool allOrPendingLoans)
    {
      if (tradeType != EllieMae.Encompass.BusinessObjects.TradeManagement.TradeType.CorrespondentTrade)
        throw new Exception(tradeType.ToString() + " is not supported.");
      CorrespondentTradeManager.ValidateCorrespondentTradeSettings(this.Session.SessionObjects);
      ICorrespondentTradeManager correspondentTradeManager = (ICorrespondentTradeManager) this.Session.GetObject("CorrespondentTradeManager");
      CorrespondentTradeInfo trade = correspondentTradeManager.GetTrade(tradeId);
      this.ValidateTrade((TradeInfoObj) trade);
      PipelineInfo[] assignedOrPendingLoans = correspondentTradeManager.GetAssignedOrPendingLoans(tradeId, TradeLoanAssignment.GetFieldList().ToArray(), false);
      CorrespondentTradeLoanAssignmentManager assignmentManager = new CorrespondentTradeLoanAssignmentManager(this.Session.SessionObjects, tradeId, assignedOrPendingLoans);
      CorrespondentTradeLoanAssignment[] assignments = assignmentManager.GetAllAssignedPendingLoans();
      if (!allOrPendingLoans)
        assignments = assignmentManager.GetPendingLoans();
      return BatchJobManager.SubmitTradeBatchUpdateJob(trade, assignments, BatchJobApplicationChannel.SDK, this.Session.SessionObjects);
    }

    /// <summary>Returns Id of Job if submitted successfully.</summary>
    /// <param name="tradeId">Trade Id</param>
    /// <param name="tradeType">Trade Type</param>
    /// <param name="loanNumbers">Set of assinged loans to be updated.</param>
    /// <returns><see cref="T:System.Int32" />Job Id.</returns>
    /// <remarks>Returns Id of Job if submitted successfully.</remarks>
    public int CreateLoanUpdateJob(int tradeId, EllieMae.Encompass.BusinessObjects.TradeManagement.TradeType tradeType, List<string> loanNumbers)
    {
      if (tradeType != EllieMae.Encompass.BusinessObjects.TradeManagement.TradeType.CorrespondentTrade)
        throw new Exception(tradeType.ToString() + " is not supported.");
      CorrespondentTradeManager.ValidateCorrespondentTradeSettings(this.Session.SessionObjects);
      ICorrespondentTradeManager correspondentTradeManager = (ICorrespondentTradeManager) this.Session.GetObject("CorrespondentTradeManager");
      CorrespondentTradeInfo trade = correspondentTradeManager.GetTrade(tradeId);
      this.ValidateTrade((TradeInfoObj) trade);
      PipelineInfo[] assignedOrPendingLoans = correspondentTradeManager.GetAssignedOrPendingLoans(tradeId, TradeLoanAssignment.GetFieldList().ToArray(), false);
      CorrespondentTradeLoanAssignment[] array = ((IEnumerable<CorrespondentTradeLoanAssignment>) new CorrespondentTradeLoanAssignmentManager(this.Session.SessionObjects, tradeId, assignedOrPendingLoans).GetAllAssignedPendingLoans()).Where<CorrespondentTradeLoanAssignment>((Func<CorrespondentTradeLoanAssignment, bool>) (x => loanNumbers.Contains(x.PipelineInfo.LoanNumber))).ToArray<CorrespondentTradeLoanAssignment>();
      return BatchJobManager.SubmitTradeBatchUpdateJob(trade, array, BatchJobApplicationChannel.SDK, this.Session.SessionObjects);
    }

    /// <summary>Returns Id of Job if submitted successfully.</summary>
    /// <returns>Returns summary of all Jobs submitted.</returns>
    /// <remarks>Returns summary of all Jobs submitted.</remarks>
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

    /// <summary>Cancel the job.</summary>
    /// <param name="jobId">Job Id</param>
    /// <returns><see cref="T:System.Boolean" />Returns true of cancellation is done successfully.</returns>
    /// <remarks>Returns true of cancellation is done successfully.</remarks>
    public bool CancelJob(int jobId)
    {
      return this.Session.SessionObjects.TradeSynchronizationManager.Cancel(jobId);
    }

    /// <summary>Remove the job.</summary>
    /// <param name="jobId">Job Id</param>
    /// <returns><see cref="T:System.Boolean" />Returns true if removal is done successfully.</returns>
    /// <remarks>Returns true if removal is done successfully.</remarks>
    public bool RemoveJob(int jobId)
    {
      return this.Session.SessionObjects.TradeSynchronizationManager.Remove(jobId);
    }

    private bool ValidateTrade(TradeInfoObj tradeInfo)
    {
      if (tradeInfo == null)
        throw new Exception("Trade with Id " + (object) tradeInfo.TradeID + " does not exist.");
      if (tradeInfo.Status == EllieMae.EMLite.Trading.TradeStatus.Pending || tradeInfo.Status == EllieMae.EMLite.Trading.TradeStatus.Archived)
        throw new Exception("Trade Trade with Id " + (object) tradeInfo.TradeID + " does is in Pending or Archived status.");
      return true;
    }

    private EllieMae.Encompass.BusinessObjects.TradeManagement.TradeType GetTradeType(
      BatchJobEntityType entityType)
    {
      switch (entityType)
      {
        case BatchJobEntityType.CorrespondentTrade:
          return EllieMae.Encompass.BusinessObjects.TradeManagement.TradeType.CorrespondentTrade;
        case BatchJobEntityType.LoanTrade:
          return EllieMae.Encompass.BusinessObjects.TradeManagement.TradeType.LoanTrade;
        case BatchJobEntityType.MBSPool:
          return EllieMae.Encompass.BusinessObjects.TradeManagement.TradeType.MbsPool;
        default:
          return EllieMae.Encompass.BusinessObjects.TradeManagement.TradeType.None;
      }
    }

    private JobStatus ConvertJobStatus(BatchJobStatus status)
    {
      JobStatus jobStatus = JobStatus.None;
      if (status == BatchJobStatus.Cancelled)
        jobStatus = JobStatus.Cancelled;
      if (status == BatchJobStatus.Completed)
        jobStatus = JobStatus.Completed;
      if (status == BatchJobStatus.CompletedWithError || status == BatchJobStatus.Error)
        jobStatus = JobStatus.Error;
      if (status == BatchJobStatus.Created)
        jobStatus = JobStatus.Created;
      if (status == BatchJobStatus.InProgress)
        jobStatus = JobStatus.InProgress;
      if (status == BatchJobStatus.Ready)
        jobStatus = JobStatus.Created;
      if (status == BatchJobStatus.Deleted)
        jobStatus = JobStatus.None;
      return jobStatus;
    }

    private JobItemStatus ConvertJobItemStatus(BatchJobItemStatus status)
    {
      JobItemStatus jobItemStatus = JobItemStatus.None;
      if (status == BatchJobItemStatus.Cancelled)
        jobItemStatus = JobItemStatus.Cancelled;
      if (status == BatchJobItemStatus.Completed)
        jobItemStatus = JobItemStatus.Completed;
      if (status == BatchJobItemStatus.Error)
        jobItemStatus = JobItemStatus.Error;
      if (status == BatchJobItemStatus.Created)
        jobItemStatus = JobItemStatus.Created;
      if (status == BatchJobItemStatus.InProgress)
        jobItemStatus = JobItemStatus.InProgress;
      return jobItemStatus;
    }
  }
}
