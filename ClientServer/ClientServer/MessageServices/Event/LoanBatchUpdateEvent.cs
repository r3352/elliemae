// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.MessageServices.Event.LoanBatchUpdateEvent
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using com.elliemae.services.eventbus.models;
using EllieMae.EMLite.ClientServer.MessageServices.Message;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer.MessageServices.Event
{
  public class LoanBatchUpdateEvent : MessageQueueEvent
  {
    private string _instanceId;
    private string _userId;
    private string _siteId;
    public const string tenant = "urn:elli:encompass:{0}:user:{1}�";
    private static readonly string Topic = string.Format("{0}.{1}.{2}", (object) KafkaUtils.DeploymentProfile, (object) KafkaUtils.Region, (object) "loan.batch");

    public LoanBatchUpdateEvent(
      string instanceId,
      string siteId,
      string eventId,
      string userId,
      Enums.Source source,
      DateTime loanModifiedTime)
    {
      this._instanceId = instanceId == null ? string.Empty : instanceId.Trim();
      this._userId = userId;
      this._siteId = siteId;
      this.StandardMessage = this.CreateMessageHeader(siteId, eventId, userId, source, loanModifiedTime);
      this.QueueMessages = new List<QueueMessage>();
    }

    private StandardMessage CreateMessageHeader(
      string siteId,
      string eventId,
      string userId,
      Enums.Source source,
      DateTime loanModifiedTime)
    {
      return new StandardMessage()
      {
        EntityId = eventId,
        InstanceId = this._instanceId,
        SiteId = siteId,
        UserId = userId,
        Tenant = string.Format("urn:elli:encompass:{0}:user:{1}", (object) this._instanceId, (object) userId),
        Category = Enums.Category.EVENT,
        Source = EnumUtils.StringValueOf((Enum) source),
        CreateAt = loanModifiedTime
      };
    }

    public void AddKafkaMessage(string correlationId, string batchId, IEnumerable<string> loanIds)
    {
      this.QueueMessages.Add((QueueMessage) LoanBatchUpdateQueueMessage.CreateLoanBatchUpdateQueueMessage(correlationId, batchId, loanIds));
    }

    public override string GetTopic(string messageType) => LoanBatchUpdateEvent.Topic;
  }
}
