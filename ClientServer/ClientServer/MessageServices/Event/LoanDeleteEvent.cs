// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.MessageServices.Event.LoanDeleteEvent
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
  public class LoanDeleteEvent : MessageQueueEvent
  {
    public const string tenant = "urn:elli:encompass:{0}:user:{1}�";
    private string _instanceId;
    private string _userId;
    private string _siteId;
    private static readonly string Topic = string.Format("{0}.{1}.{2}", (object) KafkaUtils.DeploymentProfile, (object) KafkaUtils.Region, (object) "loan.save");

    public LoanDeleteEvent(
      string serviceId,
      string instanceId,
      string siteId,
      string entityId,
      string userId,
      Enums.Source source,
      DateTime loanModifiedTime)
    {
      this._instanceId = instanceId == null ? string.Empty : instanceId.Trim();
      this._userId = userId;
      this._siteId = siteId;
      this.StandardMessage = this.CreateMessageHeader(serviceId, siteId, entityId, userId, source, loanModifiedTime);
      this.QueueMessages = new List<QueueMessage>();
    }

    private StandardMessage CreateMessageHeader(
      string serviceId,
      string siteId,
      string entityId,
      string userId,
      Enums.Source source,
      DateTime loanModifiedTime)
    {
      return new StandardMessage()
      {
        EntityId = entityId,
        InstanceId = this._instanceId,
        SiteId = siteId,
        UserId = userId,
        Tenant = string.Format("urn:elli:encompass:{0}:user:{1}", (object) this._instanceId, (object) userId),
        Category = Enums.Category.EVENT,
        Source = EnumUtils.StringValueOf((Enum) source),
        CreateAt = loanModifiedTime
      };
    }

    public void AddKafkaMessage(
      string loanId,
      string loanEventType,
      string clientId,
      string eventSequenceNumber,
      string auditUserId)
    {
      this.QueueMessages.Add((QueueMessage) LoanDeleteMessage.createLoanDeleteMessage(loanId, loanEventType, clientId, eventSequenceNumber, auditUserId, this._instanceId));
    }

    public override string GetTopic(string messageType) => LoanDeleteEvent.Topic;
  }
}
