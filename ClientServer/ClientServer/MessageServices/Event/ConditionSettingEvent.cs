// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.MessageServices.Event.ConditionSettingEvent
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
  public class ConditionSettingEvent : MessageQueueEvent
  {
    private static readonly string Topic = string.Format("{0}.{1}.{2}", (object) KafkaUtils.DeploymentProfile, (object) KafkaUtils.Region, (object) "setting.webhooks");
    public const string tenant = "urn:elli:encompass:{0}:user:{1}�";
    private string _instanceId;
    private string _userId;
    private string _entityId;

    public ConditionSettingEvent(
      string instanceId,
      string userId,
      Enums.Source source,
      DateTime createdDateTime)
    {
      this._instanceId = instanceId?.Trim() ?? string.Empty;
      this._userId = userId;
      this._entityId = Guid.NewGuid().ToString();
      this.StandardMessage = this.CreateMessageHeader(source, createdDateTime);
      this.QueueMessages = new List<QueueMessage>();
    }

    private StandardMessage CreateMessageHeader(Enums.Source source, DateTime createdDateTime)
    {
      return new StandardMessage()
      {
        EntityId = this._entityId,
        InstanceId = this._instanceId,
        Tenant = string.Format("urn:elli:encompass:{0}:user:{1}", (object) this._instanceId, (object) this._userId),
        Category = Enums.Category.EVENT,
        Source = EnumUtils.StringValueOf((Enum) source),
        CreateAt = createdDateTime
      };
    }

    public void AddConditionSettingsTemplateKafkaMessage(
      string correlationId,
      string clientId,
      string loanEventType,
      List<ConditionSettingContract> entities)
    {
      this.QueueMessages.Add((QueueMessage) WebhookConditionSettingMessage.CreateWebhookConditionTemplateMessage(correlationId, this._entityId, loanEventType, this._instanceId, this._userId, clientId, entities));
    }

    public void AddConditionSettingsTypeKafkaMessage(
      string correlationId,
      string clientId,
      string loanEventType,
      List<ConditionSettingContract> entities)
    {
      this.QueueMessages.Add((QueueMessage) WebhookConditionSettingMessage.CreateWebhookConditionTypeMessage(correlationId, this._entityId, loanEventType, this._instanceId, this._userId, clientId, entities));
    }

    public override string GetTopic(string messageType) => ConditionSettingEvent.Topic;
  }
}
