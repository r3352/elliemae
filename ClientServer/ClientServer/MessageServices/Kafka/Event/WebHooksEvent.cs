// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.MessageServices.Kafka.Event.WebHooksEvent
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using com.elliemae.services.eventbus.models;
using EllieMae.EMLite.ClientServer.MessageServices.Message;
using EllieMae.EMLite.ClientServer.MessageServices.Message.Alerts;
using EllieMae.EMLite.ClientServer.MessageServices.Message.Attachment;
using EllieMae.EMLite.ClientServer.MessageServices.Message.Document;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer.MessageServices.Kafka.Event
{
  public class WebHooksEvent : MessageQueueEvent
  {
    private static readonly string Topic = string.Format("{0}.{1}.{2}", (object) KafkaUtils.DeploymentProfile, (object) KafkaUtils.AwsRegion, (object) "loan.webhooks");
    public const string tenant = "urn:elli:encompass:{0}:user:{1}�";
    private string _instanceId;
    private string _userId;
    private string _siteId;

    public WebHooksEvent(
      string serviceId,
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
      this.StandardMessage = this.CreateMessageHeader(serviceId, siteId, eventId, userId, source, loanModifiedTime);
      this.QueueMessages = new List<QueueMessage>();
    }

    private StandardMessage CreateMessageHeader(
      string serviceId,
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

    public void AddKafkaMessage(
      string correlationId,
      string resourceType,
      bool webHookEnabled,
      string loanId,
      string clientId,
      Enums.Type actionType,
      int loanVersionNumber,
      string loanFileLocation,
      string source,
      string type,
      bool enableAIQLicense)
    {
      this.QueueMessages.Add((QueueMessage) WebHooksMessage.CreateWebhookKafkaMessage(correlationId, loanId, resourceType, webHookEnabled, this._instanceId, this._userId, this._siteId, clientId, actionType, loanVersionNumber, loanFileLocation, source, type, enableAIQLicense));
    }

    public void AddAttachmentKafkaMessage(
      string loanId,
      string correlationId,
      string clientId,
      string instanceId,
      AttachmentEvent resourceList,
      string loanFileLocation,
      bool isSourceEncompass)
    {
      this.QueueMessages.Add((QueueMessage) WebHooksAttachmentMessage.CreateWebhookKafkaMessage(loanId, correlationId, this._userId, clientId, instanceId, resourceList, loanFileLocation, isSourceEncompass));
    }

    public void AddAlertChangeKafkaMessage(
      string loanId,
      string correlationId,
      string clientId,
      string instanceId,
      AlertChangeEvent resourceList,
      bool isSourceEncompass)
    {
      this.QueueMessages.Add((QueueMessage) WebhooksAlertChangeMessage.CreateWebhookKafkaMessage(loanId, correlationId, this._userId, clientId, instanceId, resourceList, isSourceEncompass));
    }

    public void AddConditionKafkaMessage(
      string correlationId,
      string resourceType,
      string clientId,
      Enums.Type actionType,
      List<object> resource)
    {
      this.QueueMessages.Add((QueueMessage) WebHooksConditionMessage.CreateWebhookKafkaMessage(correlationId, resourceType, this._userId, this._siteId, clientId, actionType, resource));
    }

    public void AddDocumentReceivedMessage(
      string loanId,
      string correlationId,
      string userId,
      string clientId,
      string instanceId,
      DocumentReceivedEvent documentReceived,
      bool isSourceEncompass)
    {
      this.QueueMessages.Add((QueueMessage) DocumentReceivedMessage.CreateDocumentReceivedMessage(loanId, correlationId, userId, clientId, instanceId, documentReceived, isSourceEncompass));
    }

    public void AddLockUnlockKafkaMessage(
      string loanId,
      string correlationId,
      string userId,
      string clientId,
      string instanceId,
      bool isSourceEncompass,
      string type)
    {
      this.QueueMessages.Add(LoanLockUnlockMessage.CreateLockUnlockMessage(loanId, correlationId, userId, clientId, instanceId, isSourceEncompass, type));
    }

    public override string GetTopic(string messageType) => WebHooksEvent.Topic;
  }
}
