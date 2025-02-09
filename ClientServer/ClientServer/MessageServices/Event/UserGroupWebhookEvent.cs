// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.MessageServices.Event.UserGroupWebhookEvent
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using com.elliemae.services.eventbus.models;
using EllieMae.EMLite.ClientServer.MessageServices.Message.UserGroup;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer.MessageServices.Event
{
  public class UserGroupWebhookEvent : MessageQueueEvent
  {
    private static readonly string Topic = string.Format("{0}.{1}.{2}", (object) KafkaUtils.DeploymentProfile, (object) KafkaUtils.Region, (object) "usergroup.webhooks");
    public const string tenant = "urn:elli:encompass:�";
    private string _instanceId;
    private string _userId;
    private string _entityId;

    public UserGroupWebhookEvent(string instanceId, string userId, Enums.Source source)
    {
      this._instanceId = instanceId?.Trim() ?? string.Empty;
      this._userId = userId;
      this._entityId = Guid.NewGuid().ToString();
      this.StandardMessage = this.CreateMessageHeader(source);
      this.QueueMessages = new List<QueueMessage>();
    }

    private StandardMessage CreateMessageHeader(Enums.Source source)
    {
      return new StandardMessage()
      {
        EntityId = this._entityId,
        InstanceId = this._instanceId,
        Tenant = "urn:elli:encompass:" + this._instanceId + ":user:" + this._userId,
        Category = Enums.Category.EVENT,
        Source = EnumUtils.StringValueOf((Enum) source),
        CreateAt = DateTime.UtcNow
      };
    }

    public void CreateUserGroupMessage(
      string correlationId,
      string instanceId,
      string userId,
      string eventType,
      string clientId,
      bool isSourceEncompass,
      int groupId,
      MembersContract members)
    {
      this.QueueMessages.Add((QueueMessage) UserGroupWebhookMessage.CreateUserGroupMessage(correlationId, instanceId, eventType, userId, clientId, isSourceEncompass, groupId, members));
    }

    public override string GetTopic(string messageType) => UserGroupWebhookEvent.Topic;
  }
}
