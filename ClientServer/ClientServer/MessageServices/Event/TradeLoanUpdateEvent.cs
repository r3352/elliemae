// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.MessageServices.Event.TradeLoanUpdateEvent
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using com.elliemae.services.eventbus.models;
using EllieMae.EMLite.ClientServer.MessageServices.Message;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer.MessageServices.Event
{
  public class TradeLoanUpdateEvent : MessageQueueEvent
  {
    public const string tenant = "urn:elli:encompass:{0}:user:{1}�";
    private static readonly string Topic = string.Format("{0}.{1}.{2}", (object) KafkaUtils.DeploymentProfile, (object) KafkaUtils.Region, (object) "trade.batchJob");
    private string _instanceId;

    public TradeLoanUpdateEvent(string instanceId, string tradeId, string userId)
    {
      this._instanceId = instanceId?.Trim() ?? string.Empty;
      this.StandardMessage = this.CreateMessageHeader(tradeId, userId);
      this.QueueMessages = new List<QueueMessage>();
    }

    private StandardMessage CreateMessageHeader(string tradeId, string userId)
    {
      return new StandardMessage()
      {
        EntityId = tradeId,
        InstanceId = this._instanceId,
        UserId = userId,
        Tenant = string.Format("urn:elli:encompass:{0}:user:{1}", (object) this._instanceId, (object) userId),
        Category = Enums.Category.EVENT,
        Source = EnumUtils.StringValueOf((Enum) Enums.Source.URN_ELLI_SERVICE_EBS),
        CreateAt = DateTime.UtcNow
      };
    }

    public void AddKafkaMessage(
      string instanceId,
      string batchJobId,
      string batchJobStatus,
      string batchJobResult,
      Hashtable failedLoans,
      string tradeId,
      string tradeStatus,
      string sessionId)
    {
      this.QueueMessages.Add((QueueMessage) TradeLoanUpdateMessage.CreateTradeLoanUpdateMessage(instanceId, batchJobId, batchJobStatus, this.BuildBatchJobResult(batchJobResult, failedLoans), tradeId, tradeStatus, sessionId));
    }

    private BatchJobResult BuildBatchJobResult(string result, Hashtable failedLoans)
    {
      BatchJobResult batchJobResult = new BatchJobResult();
      batchJobResult.summary = result;
      if (failedLoans != null && failedLoans.Count > 0)
      {
        batchJobResult.failedLoans = new ArrayList();
        foreach (DictionaryEntry failedLoan in failedLoans)
          batchJobResult.failedLoans.Add((object) new failedLoan()
          {
            loanId = (string) failedLoan.Key,
            failedReason = (string) failedLoan.Value
          });
      }
      return batchJobResult;
    }

    public override string GetTopic(string messageType) => TradeLoanUpdateEvent.Topic;
  }
}
