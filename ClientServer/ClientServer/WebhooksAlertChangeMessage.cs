// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.WebhooksAlertChangeMessage
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.MessageServices.Message.Alerts;
using EllieMae.EMLite.ClientServer.MessageServices.Message.Common;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public class WebhooksAlertChangeMessage : QueueMessage
  {
    public Resource Resource { get; set; }

    public Client Client { get; set; }

    public string UserId { get; set; }

    public string ClientId { get; set; }

    public string EventType { get; set; }

    public AlertChangeEvent Event { get; set; }

    public static WebhooksAlertChangeMessage CreateWebhookKafkaMessage(
      string loanId,
      string correlationId,
      string userId,
      string clientId,
      string instanceId,
      AlertChangeEvent alertChangeEvent,
      bool isSourceEncompass)
    {
      string str = "/encompass/v1/loans/" + loanId + "/alerts";
      WebhooksAlertChangeMessage webhookKafkaMessage = new WebhooksAlertChangeMessage();
      webhookKafkaMessage.EnvelopeVersion = "2.0.0";
      webhookKafkaMessage.PayloadVersion = "2.0.0";
      webhookKafkaMessage.CorrelationId = correlationId;
      webhookKafkaMessage.PublishTime = DateTime.Now.ToString("O");
      webhookKafkaMessage.UserId = userId;
      webhookKafkaMessage.EventType = "alertchange";
      webhookKafkaMessage.Type = string.Format("urn:elli:service:{0}:loan:alertchange", isSourceEncompass ? (object) "encompass" : (object) "ebs").ToLower();
      webhookKafkaMessage.Resource = new Resource()
      {
        Type = "Loan",
        Id = loanId,
        Reference = str
      };
      webhookKafkaMessage.Client = new Client()
      {
        ClientId = clientId,
        InstanceId = instanceId
      };
      webhookKafkaMessage.Event = alertChangeEvent;
      return webhookKafkaMessage;
    }
  }
}
