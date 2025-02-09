// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.WebHooksMessage
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using com.elliemae.services.eventbus.models;
using EllieMae.EMLite.ClientServer.MessageServices.Message.Common;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public class WebHooksMessage : QueueMessage
  {
    public string UserId { get; set; }

    public Resource Resource { get; set; }

    public Client Client { get; set; }

    public LoanFileSequenceNumber LoanFileSequenceNumber { get; set; }

    public InstanceSettings InstanceSettings { get; set; }

    public string EventType { get; set; }

    public static WebHooksMessage CreateWebhookKafkaMessage(
      string correlationId,
      string loanId,
      string resourceType,
      bool webhookEnabled,
      string instanceId,
      string userId,
      string siteId,
      string clientId,
      Enums.Type actionType,
      int loanVersionNumber,
      string loanFileLocation,
      string source,
      string type,
      bool enableAIQLicense)
    {
      WebHooksMessage webhookKafkaMessage = new WebHooksMessage();
      webhookKafkaMessage.PayloadVersion = "2.0.0";
      webhookKafkaMessage.EnvelopeVersion = "2.0.0";
      webhookKafkaMessage.CorrelationId = correlationId;
      webhookKafkaMessage.PublishTime = DateTime.Now.ToString("O");
      webhookKafkaMessage.UserId = userId;
      webhookKafkaMessage.Type = ("urn:elli:service:" + source + ":" + resourceType + ":" + type).ToLower();
      webhookKafkaMessage.EventType = type.ToLower();
      webhookKafkaMessage.InstanceSettings = new InstanceSettings()
      {
        WebhooksEnabled = webhookEnabled,
        EnableAIQLicense = enableAIQLicense
      };
      webhookKafkaMessage.Resource = new Resource()
      {
        Id = loanId,
        Type = "Loan",
        Reference = "/encompass/v3/loans/" + loanId,
        LoanPath = loanFileLocation
      };
      webhookKafkaMessage.Client = new Client()
      {
        ClientId = clientId,
        SiteId = siteId,
        InstanceId = instanceId
      };
      WebHooksMessage webHooksMessage = webhookKafkaMessage;
      LoanFileSequenceNumber fileSequenceNumber;
      if (loanVersionNumber <= 0)
      {
        fileSequenceNumber = (LoanFileSequenceNumber) null;
      }
      else
      {
        fileSequenceNumber = new LoanFileSequenceNumber();
        fileSequenceNumber.current = loanVersionNumber.ToString();
        fileSequenceNumber.previous = (loanVersionNumber - 1).ToString();
      }
      webHooksMessage.LoanFileSequenceNumber = fileSequenceNumber;
      return webhookKafkaMessage;
    }
  }
}
