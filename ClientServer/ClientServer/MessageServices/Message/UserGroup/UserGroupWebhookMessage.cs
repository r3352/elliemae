// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.MessageServices.Message.UserGroup.UserGroupWebhookMessage
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.MessageServices.Event;
using EllieMae.EMLite.ClientServer.MessageServices.Message.Common;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.MessageServices.Message.UserGroup
{
  public class UserGroupWebhookMessage : QueueMessage
  {
    public string EventType { get; set; }

    public string UserId { get; set; }

    public Resource Resource { get; set; }

    public Client Client { get; set; }

    public UserGroupWebhookSubEvent Event { get; set; }

    public static UserGroupWebhookMessage CreateUserGroupMessage(
      string correlationId,
      string instanceId,
      string eventType,
      string loggedInUserId,
      string clientId,
      bool isSourceEncompass,
      int groupId,
      MembersContract members)
    {
      UserGroupWebhookMessage userGroupMessage = new UserGroupWebhookMessage();
      userGroupMessage.PayloadVersion = "2.0.0";
      userGroupMessage.EnvelopeVersion = "2.0.0";
      userGroupMessage.CorrelationId = correlationId ?? Guid.NewGuid().ToString();
      userGroupMessage.PublishTime = DateTime.Now.ToString("O");
      userGroupMessage.Type = string.Format("urn:elli:service:{0}:userGroup:{1}", isSourceEncompass ? (object) "encompass" : (object) "ebs", (object) eventType).ToLower();
      userGroupMessage.UserId = loggedInUserId;
      userGroupMessage.EventType = eventType.ToLower();
      userGroupMessage.Resource = new Resource()
      {
        Id = groupId.ToString(),
        Type = "userGroup",
        Reference = string.Format("/encompass/v3/settings/userGroups/{0}", (object) groupId)
      };
      userGroupMessage.Client = new Client()
      {
        ClientId = clientId,
        SiteId = "siteId",
        InstanceId = instanceId.ToUpper()
      };
      userGroupMessage.Event = new UserGroupWebhookSubEvent()
      {
        Members = members
      };
      return userGroupMessage;
    }
  }
}
