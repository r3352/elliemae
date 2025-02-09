// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.MessageServices.Message.UserWebhookMessage
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.MessageServices.Event;
using EllieMae.EMLite.ClientServer.MessageServices.Message.Common;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer.MessageServices.Message
{
  public class UserWebhookMessage : QueueMessage
  {
    private static string InternalUserUrl = "/encompass/v3/users";
    private static string ExternalUserUrl = "/encompass/v3/externalUsers";

    public string EventType { get; set; }

    public string UserId { get; set; }

    public Resource Resource { get; set; }

    public Client Client { get; set; }

    public UserWebhookSubEvent Event { get; set; }

    public IEnumerable<string> Entities { get; set; }

    public static UserWebhookMessage CreateUserMessage(
      string correlationId,
      string entityId,
      string instanceId,
      string eventType,
      string loggedInUserId,
      string clientId,
      UserWebhookMessage.UserType userType,
      IEnumerable<string> contactIds)
    {
      List<UserContract> userContractList = new List<UserContract>();
      foreach (string contactId in contactIds)
        userContractList.Add(new UserContract()
        {
          Id = contactId
        });
      UserWebhookMessage userMessage = new UserWebhookMessage();
      userMessage.PayloadVersion = "2.0.0";
      userMessage.EnvelopeVersion = "2.0.0";
      userMessage.CorrelationId = correlationId ?? Guid.NewGuid().ToString();
      userMessage.PublishTime = DateTime.Now.ToString("O");
      userMessage.Type = "urn:elli:service:ebs:" + userType.ToString().ToLower() + ":" + eventType.ToLower();
      userMessage.UserId = loggedInUserId;
      userMessage.EventType = eventType.ToLower();
      userMessage.Resource = new Resource()
      {
        Id = entityId,
        Type = userType.ToString(),
        Reference = userType == UserWebhookMessage.UserType.InternalUsers ? UserWebhookMessage.InternalUserUrl : UserWebhookMessage.ExternalUserUrl
      };
      userMessage.Client = new Client()
      {
        ClientId = clientId,
        SiteId = "siteId",
        InstanceId = instanceId.ToUpper()
      };
      userMessage.Event = new UserWebhookSubEvent()
      {
        Entities = (IEnumerable<UserContract>) userContractList
      };
      return userMessage;
    }

    public enum UserType
    {
      InternalUsers,
      ExternalUsers,
    }
  }
}
