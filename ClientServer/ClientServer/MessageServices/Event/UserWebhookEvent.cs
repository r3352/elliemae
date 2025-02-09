// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.MessageServices.Event.UserWebhookEvent
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
  public class UserWebhookEvent : MessageQueueEvent
  {
    private static readonly string Topic = string.Format("{0}.{1}.{2}", (object) KafkaUtils.DeploymentProfile, (object) KafkaUtils.Region, (object) "user.webhooks");
    public const string tenant = "urn:elli:encompass:�";
    private string _instanceId;
    private string _userId;
    private string _entityId;

    public UserWebhookEvent(
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

    private StandardMessage CreateMessageHeader(Enums.Source source, DateTime createdDate)
    {
      return new StandardMessage()
      {
        EntityId = this._entityId,
        InstanceId = this._instanceId,
        Tenant = "urn:elli:encompass:" + this._instanceId + ":user:" + this._userId,
        Category = Enums.Category.EVENT,
        Source = EnumUtils.StringValueOf((Enum) source),
        CreateAt = createdDate
      };
    }

    public void CreateUserMessage(
      string entityId,
      string correlationId,
      string instanceId,
      string userId,
      string eventType,
      UserWebhookMessage.UserType usertype,
      string clientId,
      IEnumerable<string> contactIds)
    {
      this.QueueMessages.Add((QueueMessage) UserWebhookMessage.CreateUserMessage(correlationId, entityId, instanceId, eventType, userId, clientId, usertype, contactIds));
    }

    public override string GetTopic(string messageType) => UserWebhookEvent.Topic;
  }
}
