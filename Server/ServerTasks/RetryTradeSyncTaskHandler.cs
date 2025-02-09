// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerTasks.RetryTradeSyncTaskHandler
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using com.elliemae.services.eventbus.models;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Server.ServerObjects;
using EllieMae.EMLite.Server.ServiceObjects.KafkaEvent;
using EllieMae.EMLite.Server.Tasks;
using EllieMae.EMLite.ServiceInterface;
using EllieMae.EMLite.Trading;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.Server.ServerTasks
{
  internal class RetryTradeSyncTaskHandler : ITaskHandler
  {
    private static readonly string _className = "RetryTradeAsyncTaskHandler";
    private ClientContext _ctx;

    public void ProcessTask(ServerTask taskInfo)
    {
      this._ctx = ClientContext.GetCurrent();
      DateTime now = DateTime.Now;
      this._ctx.TraceLog.WriteInfo(RetryTradeSyncTaskHandler._className, string.Format("RetryTradeAsync Start at: {0}", (object) now.ToString()));
      User latestVersion = UserStore.GetLatestVersion("admin");
      if (!latestVersion.Exists)
      {
        this._ctx.TraceLog.WriteError(RetryTradeSyncTaskHandler._className, string.Format("RetryTradeAsync user dosen't exist"));
      }
      else
      {
        UserInfo userInfo = latestVersion.UserInfo;
        this._ctx.TraceLog.WriteInfo(RetryTradeSyncTaskHandler._className, string.Format("RetryTradeAsync user is : {0}", (object) userInfo.FullName));
        List<BatchJobInfo> stuckedBatchJobs = BatchJobsAndItemsAccessor.GetStuckedBatchJobs();
        if (stuckedBatchJobs == null)
          return;
        if (stuckedBatchJobs.Count <= 0)
          return;
        try
        {
          this._ctx.TraceLog.WriteInfo(RetryTradeSyncTaskHandler._className, string.Format("RetryTradeAsync - CancelBatchJobs at: {0}", (object) DateTime.Now.ToString()));
          BatchJobsAndItemsAccessor.CancelBatchJobs(stuckedBatchJobs.Select<BatchJobInfo, int>((Func<BatchJobInfo, int>) (j => j.BatchJobId)).ToArray<int>(), "admin", DateTime.UtcNow);
          this._ctx.TraceLog.WriteInfo(RetryTradeSyncTaskHandler._className, string.Format("RetryTradeAsync - AddTradeHistory at: {0}", (object) DateTime.Now.ToString()));
          this.AddTradeHistory(stuckedBatchJobs, userInfo);
          this._ctx.TraceLog.WriteInfo(RetryTradeSyncTaskHandler._className, string.Format("RetryTradeAsync - UpdateTradeStatus at: {0}", (object) DateTime.Now.ToString()));
          this.UpdateTradeStatus(stuckedBatchJobs.Where<BatchJobInfo>((Func<BatchJobInfo, bool>) (j => j.Result == "6")).ToList<BatchJobInfo>(), userInfo);
        }
        catch (Exception ex)
        {
          this._ctx.TraceLog.WriteError(RetryTradeSyncTaskHandler._className, string.Format("RetryTradeAsync - unlock trade error: {0} - {1}", (object) ex.Message, (object) ex.StackTrace));
        }
        this._ctx.TraceLog.WriteInfo(RetryTradeSyncTaskHandler._className, string.Format("RetryTradeAsync - PublishKafkaEvents at: {0}", (object) DateTime.Now.ToString()));
        this.PublishKafkaEvents(stuckedBatchJobs, userInfo);
        foreach (BatchJobInfo batchJobInfo in stuckedBatchJobs)
          this._ctx.TraceLog.WriteWarning(RetryTradeSyncTaskHandler._className, string.Format("RetryTradeAsync Execution Summary: {0}", (object) BatchJobInfo.SerializeToJSON(batchJobInfo)));
        this._ctx.TraceLog.WriteInfo(RetryTradeSyncTaskHandler._className, string.Format("RetryTradeAsync End at: {0}", (object) DateTime.Now.ToString()));
        this._ctx.TraceLog.WriteInfo(RetryTradeSyncTaskHandler._className, string.Format("RetryTradeAsync Execution Time (minutes): {0}", (object) (DateTime.Now - now).TotalMinutes));
      }
    }

    private void AddTradeHistory(List<BatchJobInfo> trades, UserInfo userInfo)
    {
      foreach (BatchJobInfo trade1 in trades)
      {
        string comment = string.Format("Processed: {0}, Successful: {1}, Errors: {2}, Cancelled: {3}. Trade was unlocked and batch job was cancelled from Encompass Scheduled Task 'RetryTrdeSync' by {4}", (object) trade1.BatchJobItems.Count<BatchJobItemInfo>(), (object) trade1.BatchJobItems.Where<BatchJobItemInfo>((Func<BatchJobItemInfo, bool>) (i => i.Status == BatchJobItemStatus.Completed)).Count<BatchJobItemInfo>(), (object) trade1.BatchJobItems.Where<BatchJobItemInfo>((Func<BatchJobItemInfo, bool>) (i => i.Status == BatchJobItemStatus.Error)).Count<BatchJobItemInfo>(), (object) (trade1.BatchJobItems.Where<BatchJobItemInfo>((Func<BatchJobItemInfo, bool>) (i => i.Status == BatchJobItemStatus.Cancelled)).Count<BatchJobItemInfo>() + trade1.BatchJobItems.Where<BatchJobItemInfo>((Func<BatchJobItemInfo, bool>) (i => i.Status == BatchJobItemStatus.Created)).Count<BatchJobItemInfo>() + trade1.BatchJobItems.Where<BatchJobItemInfo>((Func<BatchJobItemInfo, bool>) (i => i.Status == BatchJobItemStatus.InProgress)).Count<BatchJobItemInfo>()), (object) "admin");
        switch (trade1.EntityType)
        {
          case BatchJobEntityType.CorrespondentTrade:
            CorrespondentTradeInfo trade2 = new CorrespondentTradeInfo();
            trade2.TradeID = Utils.ParseInt((object) trade1.EntityId);
            CorrespondentTrades.AddTradeHistoryItem(new CorrespondentTradeHistoryItem(trade2, TradeHistoryAction.UnlockPendingTrade, comment, userInfo));
            continue;
          case BatchJobEntityType.LoanTrade:
            LoanTradeInfo trade3 = new LoanTradeInfo();
            trade3.TradeID = Utils.ParseInt((object) trade1.EntityId);
            LoanTrades.AddTradeHistoryItem(new LoanTradeHistoryItem(trade3, TradeHistoryAction.UnlockPendingTrade, comment, userInfo));
            continue;
          case BatchJobEntityType.MBSPool:
            MbsPoolInfo trade4 = new MbsPoolInfo();
            trade4.TradeID = Utils.ParseInt((object) trade1.EntityId);
            MbsPools.AddTradeHistoryItem(new MbsPoolHistoryItem(trade4, TradeHistoryAction.UnlockPendingTrade, comment, userInfo));
            continue;
          default:
            continue;
        }
      }
    }

    private void UpdateTradeStatus(List<BatchJobInfo> trades, UserInfo userInfo)
    {
      if (trades == null || trades.Count<BatchJobInfo>() <= 0)
        return;
      Dictionary<TradeStatus, List<int>> dictionary1 = new Dictionary<TradeStatus, List<int>>();
      foreach (BatchJobInfo batchJobInfo in trades.Where<BatchJobInfo>((Func<BatchJobInfo, bool>) (s => s.EntityType == BatchJobEntityType.CorrespondentTrade)).Select<BatchJobInfo, BatchJobInfo>((Func<BatchJobInfo, BatchJobInfo>) (s => s)).ToList<BatchJobInfo>())
      {
        if (!string.IsNullOrEmpty(batchJobInfo.MetaData))
        {
          TradeStatus tradeStatus = TradeUtils.GetTradeStatus(new CorrespondentTradeAssignment(batchJobInfo.MetaData).FinalStatus);
          if (tradeStatus != TradeStatus.None)
          {
            if (!dictionary1.ContainsKey(tradeStatus))
              dictionary1.Add(tradeStatus, new List<int>());
            if (!dictionary1[tradeStatus].Contains(Utils.ParseInt((object) batchJobInfo.EntityId)))
              dictionary1[tradeStatus].Add(Utils.ParseInt((object) batchJobInfo.EntityId));
          }
        }
      }
      foreach (KeyValuePair<TradeStatus, List<int>> keyValuePair in dictionary1)
        CorrespondentTrades.SetTradeStatus(keyValuePair.Value.ToArray(), keyValuePair.Key, TradeHistoryAction.TradeStatusChanged, userInfo, false);
      List<BatchJobInfo> list1 = trades.Where<BatchJobInfo>((Func<BatchJobInfo, bool>) (s => s.EntityType == BatchJobEntityType.LoanTrade)).Select<BatchJobInfo, BatchJobInfo>((Func<BatchJobInfo, BatchJobInfo>) (s => s)).ToList<BatchJobInfo>();
      Dictionary<TradeStatus, List<int>> dictionary2 = new Dictionary<TradeStatus, List<int>>();
      foreach (BatchJobInfo batchJobInfo in list1)
      {
        if (!string.IsNullOrEmpty(batchJobInfo.MetaData))
        {
          TradeStatus tradeStatus = TradeUtils.GetTradeStatus(new LoanTradeLoanAssignment(batchJobInfo.MetaData).FinalStatus);
          if (tradeStatus != TradeStatus.None)
          {
            if (!dictionary2.ContainsKey(tradeStatus))
              dictionary2.Add(tradeStatus, new List<int>());
            if (!dictionary2[tradeStatus].Contains(Utils.ParseInt((object) batchJobInfo.EntityId)))
              dictionary2[tradeStatus].Add(Utils.ParseInt((object) batchJobInfo.EntityId));
          }
        }
      }
      foreach (KeyValuePair<TradeStatus, List<int>> keyValuePair in dictionary2)
        LoanTrades.SetTradeStatus(keyValuePair.Value.ToArray(), keyValuePair.Key, TradeHistoryAction.TradeStatusChanged, userInfo, false);
      List<BatchJobInfo> list2 = trades.Where<BatchJobInfo>((Func<BatchJobInfo, bool>) (s => s.EntityType == BatchJobEntityType.MBSPool)).Select<BatchJobInfo, BatchJobInfo>((Func<BatchJobInfo, BatchJobInfo>) (s => s)).ToList<BatchJobInfo>();
      Dictionary<TradeStatus, List<int>> dictionary3 = new Dictionary<TradeStatus, List<int>>();
      foreach (BatchJobInfo batchJobInfo in list2)
      {
        if (!string.IsNullOrEmpty(batchJobInfo.MetaData))
        {
          TradeStatus tradeStatus = TradeUtils.GetTradeStatus(new MbsPoolTradeAssignment(batchJobInfo.MetaData).FinalStatus);
          if (tradeStatus != TradeStatus.None)
          {
            if (!dictionary3.ContainsKey(tradeStatus))
              dictionary3.Add(tradeStatus, new List<int>());
            if (!dictionary3[tradeStatus].Contains(Utils.ParseInt((object) batchJobInfo.EntityId)))
              dictionary3[tradeStatus].Add(Utils.ParseInt((object) batchJobInfo.EntityId));
          }
        }
      }
      foreach (KeyValuePair<TradeStatus, List<int>> keyValuePair in dictionary3)
        MbsPools.SetTradeStatus(keyValuePair.Value.ToArray(), keyValuePair.Key, TradeHistoryAction.TradeStatusChanged, userInfo, false);
    }

    private void PublishKafkaEvents(List<BatchJobInfo> trades, UserInfo userInfo)
    {
      foreach (BatchJobInfo trade in trades)
      {
        if (!trade.BatchJobItems.Any<BatchJobItemInfo>((Func<BatchJobItemInfo, bool>) (i => i.Status == BatchJobItemStatus.Created || i.Status == BatchJobItemStatus.InProgress)))
        {
          TraceLog.WriteInfo(RetryTradeSyncTaskHandler._className, "RetryTradeAsync PublishKafkaEvents - Message counts is 0. No message to be sent to Kafka");
        }
        else
        {
          bool isSucess = true;
          try
          {
            BatchJobInfo batchJobInfo = this.SaveBatchJob(trade);
            RetryTradeSyncTaskHandler.UpdateTradeStatus(batchJobInfo, isSucess, userInfo);
            TraceLog.WriteInfo(RetryTradeSyncTaskHandler._className, "RetryTradeAsync PublishKafkaEvents - Use Kafka.");
            new MessageQueueEventService().MessageQueueProducer((MessageQueueEvent) new TradeSyncEvent(this.buildKafkaStandardMessage(this._ctx, batchJobInfo), this.buildKakfaPayload(batchJobInfo)), (IMessageQueueProcessor) new KafkaProcessor());
          }
          catch (Exception ex)
          {
            throw new Exception("RetryTradeAsyncTaskHandler\\Handle<T>" + ex.Message);
          }
        }
      }
    }

    private BatchJobInfo SaveBatchJob(BatchJobInfo oldJob)
    {
      DateTime now = DateTime.UtcNow;
      List<BatchJobItemInfo> list = oldJob.BatchJobItems.Where<BatchJobItemInfo>((Func<BatchJobItemInfo, bool>) (i => i.Status == BatchJobItemStatus.Created || i.Status == BatchJobItemStatus.InProgress)).Select<BatchJobItemInfo, BatchJobItemInfo>((Func<BatchJobItemInfo, BatchJobItemInfo>) (i => new BatchJobItemInfo(0, 0, BatchJobItemStatus.Created, now, i.Action.ToString(), i.EntityId, i.EntityType, "", i.MetaData))).ToList<BatchJobItemInfo>();
      return BatchJobsAndItemsAccessor.CreateBatchJobInfo(new BatchJobInfo(0, "admin", now, BatchJobApplicationChannel.TaskScheduler, BatchJobStatus.InProgress, now, BatchJobType.TradeSynchronization, "admin", oldJob.EntityId, oldJob.EntityType, oldJob.MetaData, list));
    }

    private StandardMessage buildKafkaStandardMessage(ClientContext context, BatchJobInfo newJob)
    {
      return new StandardMessage()
      {
        CreateAt = DateTime.Now,
        Category = Enums.Category.TASK,
        Source = !newJob.ApplicationChannel.ToDescription().Equals(BatchJobApplicationChannel.SDK.ToDescription(), StringComparison.InvariantCultureIgnoreCase) ? Enums.Source.URN_ELLI_SERVICE_EBS.ToDescription() : Enums.Source.URN_ELLI_SERVICE_SDK.ToDescription(),
        InstanceId = context.InstanceName,
        UserId = "admin",
        EntityId = newJob.EntityId,
        Tenant = "urn:elli:encompass:" + context.InstanceName + ":user:admin"
      };
    }

    private Dictionary<string, object> buildKakfaPayload(BatchJobInfo job)
    {
      Dictionary<string, object> dictionary = new Dictionary<string, object>();
      foreach (BatchJobItemInfo batchJobItem in job.BatchJobItems)
      {
        TradeSyncQueueMessage syncQueueMessage = new TradeSyncQueueMessage();
        syncQueueMessage.CorrelationId = Guid.NewGuid().ToString();
        syncQueueMessage.PublishTime = DateTime.Now.ToString("o");
        syncQueueMessage.BatchJobEntity = new BatchJobEntity(job.BatchJobId.ToString(), "BatchJob");
        syncQueueMessage.BatchJobItemEntity = new BatchJobEntity(batchJobItem.BatchJobItemId.ToString(), "BatchJobItem");
        syncQueueMessage.LoanId = batchJobItem.EntityId;
        syncQueueMessage.LoanPath = "";
        syncQueueMessage.PriorLoanFileName = "";
        syncQueueMessage.AfterLoanFileName = "";
        syncQueueMessage.TradeId = job.EntityId;
        syncQueueMessage.Action = !batchJobItem.Action.Equals(BatchJobItemAction.AssignToTrade.ToDescription(), StringComparison.InvariantCultureIgnoreCase) ? (!batchJobItem.Action.Equals(BatchJobItemAction.RemoveFromTrade.ToDescription(), StringComparison.InvariantCultureIgnoreCase) ? "Refresh" : "Remove") : "Add";
        syncQueueMessage.Type = Enums.Type.BATCH_JOB_LOAN_TRADE_SYNC.ToDescription();
        syncQueueMessage.InstanceId = this._ctx.InstanceName;
        syncQueueMessage.EventId = "TradeSynchronization";
        syncQueueMessage.DeferredLoanId = "";
        dictionary.Add(batchJobItem.EntityId, (object) syncQueueMessage);
      }
      return dictionary;
    }

    private static void UpdateTradeStatus(BatchJobInfo batchJob, bool isSucess, UserInfo userInfo)
    {
      switch (batchJob.EntityType)
      {
        case BatchJobEntityType.CorrespondentTrade:
          CorrespondentTradeInfo correspondentTradeInfo = new CorrespondentTradeInfo();
          correspondentTradeInfo.TradeID = Utils.ParseInt((object) batchJob.EntityId);
          correspondentTradeInfo.Status = TradeStatus.Pending;
          correspondentTradeInfo.PendingBy = userInfo.FullName;
          CorrespondentTrades.UpdateTradeStatus(correspondentTradeInfo.TradeID, TradeStatus.Pending, userInfo);
          break;
        case BatchJobEntityType.LoanTrade:
          LoanTradeInfo loanTradeInfo = new LoanTradeInfo();
          loanTradeInfo.TradeID = Utils.ParseInt((object) batchJob.EntityId);
          loanTradeInfo.Status = TradeStatus.Pending;
          loanTradeInfo.PendingBy = userInfo.FullName;
          LoanTrades.UpdateTradeStatus(loanTradeInfo.TradeID, TradeStatus.Pending, userInfo);
          break;
        case BatchJobEntityType.MBSPool:
          MbsPoolInfo mbsPoolInfo = new MbsPoolInfo();
          mbsPoolInfo.TradeID = Utils.ParseInt((object) batchJob.EntityId);
          mbsPoolInfo.Status = TradeStatus.Pending;
          mbsPoolInfo.PendingBy = userInfo.FullName;
          MbsPools.UpdateTradeStatus(mbsPoolInfo.TradeID, TradeStatus.Pending, userInfo);
          break;
      }
    }
  }
}
