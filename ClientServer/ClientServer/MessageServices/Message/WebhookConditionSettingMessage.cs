// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.MessageServices.Message.WebhookConditionSettingMessage
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
  public class WebhookConditionSettingMessage : QueueMessage
  {
    public Resource Resource { get; set; }

    public Client Client { get; set; }

    public string EventType { get; set; }

    public string UserId { get; set; }

    public WebhookConditionEvent Event { get; set; }

    public List<ConditionSettingContract> Entities { get; set; }

    public static WebhookConditionSettingMessage CreateWebhookConditionTemplateMessage(
      string correlationId,
      string entityId,
      string loanEventType,
      string instanceId,
      string userId,
      string clientId,
      List<ConditionSettingContract> entities)
    {
      WebhookConditionSettingMessage conditionTemplateMessage = new WebhookConditionSettingMessage();
      conditionTemplateMessage.PayloadVersion = "2.0.0";
      conditionTemplateMessage.EnvelopeVersion = "2.0.0";
      conditionTemplateMessage.CorrelationId = correlationId ?? Guid.NewGuid().ToString();
      conditionTemplateMessage.PublishTime = DateTime.Now.ToString("O");
      conditionTemplateMessage.UserId = userId;
      conditionTemplateMessage.Type = ("urn:elli:service:ebs:enhancedconditiontemplate:" + loanEventType).ToLower();
      conditionTemplateMessage.EventType = loanEventType.ToLower();
      conditionTemplateMessage.Resource = new Resource()
      {
        Id = entityId,
        Type = "EnhancedConditionTemplate",
        Reference = "/encompass/v3/settings/loan/conditions/templates"
      };
      conditionTemplateMessage.Client = new Client()
      {
        ClientId = clientId,
        SiteId = "siteId",
        InstanceId = instanceId
      };
      conditionTemplateMessage.Event = new WebhookConditionEvent()
      {
        Entities = entities,
        Event = new WebhookConditionSubEvent()
        {
          DateSubmitted = DateTime.UtcNow,
          ConsumerDirectWebsite = "siteId"
        }
      };
      return conditionTemplateMessage;
    }

    public static WebhookConditionSettingMessage CreateWebhookConditionTypeMessage(
      string correlationId,
      string entityId,
      string loanEventType,
      string instanceId,
      string userId,
      string clientId,
      List<ConditionSettingContract> entities)
    {
      WebhookConditionSettingMessage conditionTypeMessage = new WebhookConditionSettingMessage();
      conditionTypeMessage.PayloadVersion = "2.0.0";
      conditionTypeMessage.EnvelopeVersion = "2.0.0";
      conditionTypeMessage.CorrelationId = correlationId ?? Guid.NewGuid().ToString();
      conditionTypeMessage.PublishTime = DateTime.Now.ToString("O");
      conditionTypeMessage.UserId = userId;
      conditionTypeMessage.Type = ("urn:elli:service:ebs:enhancedconditiontype:" + loanEventType).ToLower();
      conditionTypeMessage.EventType = loanEventType.ToLower();
      conditionTypeMessage.Resource = new Resource()
      {
        Id = entityId,
        Type = "EnhancedConditionType",
        Reference = "/encompass/v3/settings/loan/conditions/types"
      };
      conditionTypeMessage.Client = new Client()
      {
        ClientId = clientId,
        SiteId = "siteId",
        InstanceId = instanceId
      };
      conditionTypeMessage.Event = new WebhookConditionEvent()
      {
        Entities = entities,
        Event = new WebhookConditionSubEvent()
        {
          DateSubmitted = DateTime.UtcNow,
          ConsumerDirectWebsite = "siteId"
        }
      };
      return conditionTypeMessage;
    }
  }
}
