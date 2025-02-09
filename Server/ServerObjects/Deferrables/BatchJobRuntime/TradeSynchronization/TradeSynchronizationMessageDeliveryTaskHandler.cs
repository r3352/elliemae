// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Deferrables.BatchJobRuntime.TradeSynchronization.TradeSynchronizationMessageDeliveryTaskHandler
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using com.elliemae.services.eventbus.models;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Server.ServiceObjects.KafkaEvent;
using EllieMae.EMLite.ServiceInterface;
using EllieMae.EMLite.Trading;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.Deferrables.BatchJobRuntime.TradeSynchronization
{
  public class TradeSynchronizationMessageDeliveryTaskHandler : IDeferrableMessageTaskHandler
  {
    private const string className = "TradeSynchronizationMessageDeliveryTaskHandler�";
    public const string BaseRoutingKey = "Elli.BatchJobMessage.TradeSynchronizationProcess�";

    public void Handle(DeferrableMessageDeliveryTask task, ClientContext context)
    {
      throw new NotImplementedException();
    }

    public void Handle<T>(
      DeferrableMessageDeliveryTask[] tasks,
      ClientContext context,
      DeferrableDataBag<T> threadSafeDataBag = null)
    {
      DeferrableDataBag<TradeSynchronizationMessage> dataBag = threadSafeDataBag != null ? threadSafeDataBag as DeferrableDataBag<TradeSynchronizationMessage> : DeferrableDataBag<TradeSynchronizationMessage>.GetInstance();
      TradeInfoObj tradeInfo = dataBag.Get<TradeInfoObj>("TradeInfo");
      UserInfo userInfo = dataBag.Get<UserInfo>("UserInfo");
      BatchJobInfo batchJob = (BatchJobInfo) null;
      try
      {
        batchJob = TradeSynchronizationMessageDeliveryTaskHandler.SaveBatchJob(dataBag, tradeInfo);
        List<Tuple<TradeSynchronizationMessage, string>> list = ((IEnumerable<DeferrableMessageDeliveryTask>) tasks).Select<DeferrableMessageDeliveryTask, Tuple<TradeSynchronizationMessage, string>>((Func<DeferrableMessageDeliveryTask, Tuple<TradeSynchronizationMessage, string>>) (task => new Tuple<TradeSynchronizationMessage, string>(TradeSynchronizationMessageDeliveryTaskHandler.Setup((TradeSynchronizationMessage) task.Message, dataBag, batchJob), TradeSynchronizationMessageDeliveryTaskHandler.FormatRoutingKey(task.RoutingKey)))).ToList<Tuple<TradeSynchronizationMessage, string>>();
        if (list.Count <= 0)
        {
          TraceLog.WriteInfo(nameof (TradeSynchronizationMessageDeliveryTaskHandler), "Message counts is 0. No message to be sent to Kafka");
        }
        else
        {
          TradeSynchronizationMessageDeliveryTaskHandler.UpdateTradeStatus(batchJob, tradeInfo, userInfo);
          TraceLog.WriteInfo(nameof (TradeSynchronizationMessageDeliveryTaskHandler), "Use Kafka.");
          new MessageQueueEventService().MessageQueueProducer((MessageQueueEvent) new TradeSyncEvent(this.buildKafkaStandardMessage(context, dataBag), this.buildKakfaPayload(list, dataBag)), (IMessageQueueProcessor) new KafkaProcessor());
        }
      }
      catch (Exception ex)
      {
        throw new Exception("TradeSynchronizationMessageDeliveryTaskHandler\\Handle<T>" + ex.Message);
      }
    }

    private StandardMessage buildKafkaStandardMessage(
      ClientContext context,
      DeferrableDataBag<TradeSynchronizationMessage> _dataBag = null)
    {
      return new StandardMessage()
      {
        CreateAt = DateTime.Now,
        Category = Enums.Category.TASK,
        Source = !_dataBag.Get<BatchJobApplicationChannel>("ApplicationChannel").ToDescription().Equals(BatchJobApplicationChannel.SDK.ToDescription(), StringComparison.InvariantCultureIgnoreCase) ? Enums.Source.URN_ELLI_SERVICE_EBS.ToDescription() : Enums.Source.URN_ELLI_SERVICE_SDK.ToDescription(),
        InstanceId = _dataBag.Get<string>("InstanceId"),
        UserId = _dataBag.Get<UserInfo>("UserInfo").Userid,
        EntityId = _dataBag.Get<string>("EventId"),
        Tenant = "urn:elli:encompass:" + _dataBag.Get<string>("InstanceId") + ":user:" + _dataBag.Get<UserInfo>("UserInfo").Userid
      };
    }

    private Dictionary<string, object> buildKakfaPayload(
      List<Tuple<TradeSynchronizationMessage, string>> mesgList,
      DeferrableDataBag<TradeSynchronizationMessage> _dataBag = null)
    {
      Dictionary<string, object> dictionary = new Dictionary<string, object>();
      foreach (Tuple<TradeSynchronizationMessage, string> mesg in mesgList)
      {
        TradeSyncQueueMessage syncQueueMessage1 = new TradeSyncQueueMessage();
        syncQueueMessage1.CorrelationId = Guid.NewGuid().ToString();
        syncQueueMessage1.PublishTime = DateTime.Now.ToString("o");
        TradeSyncQueueMessage syncQueueMessage2 = syncQueueMessage1;
        int num = mesg.Item1.BatchJobId;
        BatchJobEntity batchJobEntity1 = new BatchJobEntity(num.ToString(), "BatchJob");
        syncQueueMessage2.BatchJobEntity = batchJobEntity1;
        TradeSyncQueueMessage syncQueueMessage3 = syncQueueMessage1;
        num = mesg.Item1.BatchJobItemId;
        BatchJobEntity batchJobEntity2 = new BatchJobEntity(num.ToString(), "BatchJobItem");
        syncQueueMessage3.BatchJobItemEntity = batchJobEntity2;
        syncQueueMessage1.LoanId = mesg.Item1.BatchJobItemEntityId;
        syncQueueMessage1.LoanPath = "";
        syncQueueMessage1.PriorLoanFileName = "";
        syncQueueMessage1.AfterLoanFileName = "";
        syncQueueMessage1.TradeId = mesg.Item1.BatchJobEntityId;
        syncQueueMessage1.Action = !mesg.Item1.BatchJobItemAction.Equals(BatchJobItemAction.AssignToTrade.ToDescription(), StringComparison.InvariantCultureIgnoreCase) ? (!mesg.Item1.BatchJobItemAction.Equals(BatchJobItemAction.RemoveFromTrade.ToDescription(), StringComparison.InvariantCultureIgnoreCase) ? (!mesg.Item1.BatchJobItemAction.Equals(BatchJobItemAction.TradeExtension.ToDescription(), StringComparison.InvariantCultureIgnoreCase) ? "Refresh" : "Extend") : "Remove") : "Add";
        syncQueueMessage1.Type = Enums.Type.BATCH_JOB_LOAN_TRADE_SYNC.ToDescription();
        syncQueueMessage1.InstanceId = _dataBag.Get<string>("InstanceId");
        syncQueueMessage1.EventId = "TradeSynchronization";
        syncQueueMessage1.DeferredLoanId = "";
        dictionary.Add(mesg.Item1.BatchJobItemEntityId, (object) syncQueueMessage1);
      }
      return dictionary;
    }

    private static TradeSynchronizationMessage Setup(
      TradeSynchronizationMessage orgMessage,
      DeferrableDataBag<TradeSynchronizationMessage> dataBag,
      BatchJobInfo batchJob)
    {
      TradeSynchronizationMessage synchronizationMessage = new TradeSynchronizationMessage(dataBag.Get<string>("ApplicationId"), dataBag.Get<string>("ServiceId"), dataBag.Get<string>("InstanceId"), dataBag.Get<string>("SiteId"), dataBag.Get<string>("EventId"), dataBag.Get<string>("UserId"));
      synchronizationMessage.ServerMode = EncompassServer.ServerMode;
      synchronizationMessage.PublishTime = DateTime.Now;
      synchronizationMessage.BatchJobId = batchJob.BatchJobId;
      synchronizationMessage.BatchJobEntityId = batchJob.EntityId;
      synchronizationMessage.BatchJobEntityType = batchJob.EntityType.ToString();
      BatchJobItemInfo batchJobItemInfo = batchJob.BatchJobItems.Where<BatchJobItemInfo>((Func<BatchJobItemInfo, bool>) (i => i.EntityId == orgMessage.BatchJobItemEntityId)).FirstOrDefault<BatchJobItemInfo>();
      if (batchJobItemInfo != null)
      {
        synchronizationMessage.BatchJobItemId = batchJobItemInfo.BatchJobItemId;
        synchronizationMessage.BatchJobItemEntityId = batchJobItemInfo.EntityId;
        synchronizationMessage.BatchJobItemAction = batchJobItemInfo.Action;
      }
      return synchronizationMessage;
    }

    private static BatchJobInfo SaveBatchJob(
      DeferrableDataBag<TradeSynchronizationMessage> dataBag,
      TradeInfoObj tradeInfo)
    {
      try
      {
        string str = dataBag.Get<string>("UserId");
        List<TradeAssignmentItem> source = dataBag.Get<List<TradeAssignmentItem>>("EntityList");
        List<string> skipFieldList = dataBag.Get<List<string>>("skipFieldList");
        BatchJobApplicationChannel applicationChannel = dataBag.Get<BatchJobApplicationChannel>("ApplicationChannel");
        TradeAssignment assigment = (TradeAssignment) null;
        switch (tradeInfo)
        {
          case CorrespondentTradeInfo _:
            assigment = (TradeAssignment) new CorrespondentTradeAssignment(tradeInfo as CorrespondentTradeInfo, skipFieldList);
            break;
          case MbsPoolInfo _:
            assigment = (TradeAssignment) new MbsPoolTradeAssignment(tradeInfo as MbsPoolInfo, skipFieldList);
            break;
          case LoanTradeInfo _:
            assigment = (TradeAssignment) new LoanTradeLoanAssignment(tradeInfo as LoanTradeInfo, skipFieldList);
            break;
        }
        assigment.LoanSyncOption = dataBag.Get<string>("DataBagKeyValueDataSyncOption");
        assigment.FinalStatus = dataBag.Get<string>("DataBagKeyValueFinalStatus");
        assigment.TradeExtensionInfo = dataBag.Get<string>("DataBagKeyValueTradeExtensionInfo");
        assigment.SessionId = dataBag.Get<string>("DataBagKeyValueSessionId");
        DateTime now = DateTime.UtcNow;
        List<BatchJobItemInfo> list = source.Select<TradeAssignmentItem, BatchJobItemInfo>((Func<TradeAssignmentItem, BatchJobItemInfo>) (i => new BatchJobItemInfo(0, 0, BatchJobItemStatus.Created, now, i.Action.ToString(), i.EntityId, i.Type, "", TradeAssignmentItem.SerializeToJSON(i)))).ToList<BatchJobItemInfo>();
        BatchJobInfo batchJobInfo = BatchJobsAndItemsAccessor.CreateBatchJobInfo(new BatchJobInfo(0, str, now, applicationChannel, BatchJobStatus.InProgress, now, BatchJobType.TradeSynchronization, str, tradeInfo.TradeID.ToString(), assigment.BatchJobEntityType, TradeAssignment.SerializeToJSON(assigment), list));
        dataBag.Set("BatchJobId", (object) batchJobInfo.BatchJobId);
        return batchJobInfo;
      }
      catch (Exception ex)
      {
        throw new Exception("TradeSynchronizationMessageDeliveryTaskHandler\\Setup<T>" + ex.Message);
      }
    }

    private static void UpdateTradeStatus(
      BatchJobInfo batchJob,
      TradeInfoObj tradeInfo,
      UserInfo userInfo)
    {
      switch (batchJob.EntityType)
      {
        case BatchJobEntityType.CorrespondentTrade:
          tradeInfo.TradeID = Utils.ParseInt((object) batchJob.EntityId);
          tradeInfo.Status = TradeStatus.Pending;
          tradeInfo.PendingBy = userInfo.FullName;
          CorrespondentTrades.UpdateTradeStatus(tradeInfo.TradeID, TradeStatus.Pending, userInfo);
          break;
        case BatchJobEntityType.LoanTrade:
          tradeInfo.TradeID = Utils.ParseInt((object) batchJob.EntityId);
          tradeInfo.Status = TradeStatus.Pending;
          tradeInfo.PendingBy = userInfo.FullName;
          LoanTrades.UpdateTradeStatus(tradeInfo.TradeID, TradeStatus.Pending, userInfo);
          break;
        case BatchJobEntityType.MBSPool:
          tradeInfo.TradeID = Utils.ParseInt((object) batchJob.EntityId);
          tradeInfo.Status = TradeStatus.Pending;
          tradeInfo.PendingBy = userInfo.FullName;
          MbsPools.UpdateTradeStatus(tradeInfo.TradeID, TradeStatus.Pending, userInfo);
          break;
      }
    }

    public static string FormatRoutingKey(string subRoutingKey)
    {
      return string.Format("{0}.{1}", (object) "Elli.BatchJobMessage.TradeSynchronizationProcess", (object) subRoutingKey);
    }
  }
}
