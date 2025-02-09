// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.MessageServices.Message.TradeChangeMessage
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.MessageServices.Message.Common;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.MessageServices.Message
{
  internal class TradeChangeMessage : QueueMessage
  {
    public Resource Resource { get; set; }

    public Client Client { get; set; }

    public string UserId { get; set; }

    public string EventType { get; set; }

    public string clientId { get; set; }

    public string resourceType { get; set; }

    public string eventId { get; set; }

    public string instanceId { get; set; }

    public DateTime time { get; set; }

    public string source { get; set; }

    public object payload { get; set; }

    public object Event { get; set; }

    public static TradeChangeMessage CreateTradeChangeMessage_V1(
      string tradeId,
      string instanceId,
      string clientId,
      string userId,
      string eventType,
      bool isSourceEncompass,
      WebhookEventContract webhookEvent)
    {
      TradeChangeMessage tradeChangeMessageV1 = new TradeChangeMessage();
      tradeChangeMessageV1.EnvelopeVersion = "1.0.0";
      tradeChangeMessageV1.PayloadVersion = "1.0.0";
      tradeChangeMessageV1.UserId = userId;
      tradeChangeMessageV1.EventType = eventType;
      tradeChangeMessageV1.Type = string.Format("urn:elli:{0}:{1}:trade:{2}", (object) (SourceEnum) (isSourceEncompass ? 0 : 1), (object) instanceId, (object) eventType).ToLower();
      tradeChangeMessageV1.clientId = clientId;
      tradeChangeMessageV1.resourceType = webhookEvent.ResourceType;
      tradeChangeMessageV1.eventId = webhookEvent.EventId;
      tradeChangeMessageV1.instanceId = webhookEvent.InstanceId;
      tradeChangeMessageV1.time = webhookEvent.Time;
      tradeChangeMessageV1.source = webhookEvent.Source;
      tradeChangeMessageV1.payload = webhookEvent.payload;
      return tradeChangeMessageV1;
    }

    public static TradeChangeMessage CreateTradeChangeMessage(
      string tradeId,
      string instanceId,
      string clientId,
      string userId,
      string eventType,
      bool isSourceEncompass,
      WebhookEventContract webhookEvent)
    {
      TradeChangeMessage tradeChangeMessage = new TradeChangeMessage();
      tradeChangeMessage.EnvelopeVersion = "2.0.0";
      tradeChangeMessage.PayloadVersion = "2.0.0";
      tradeChangeMessage.PublishTime = DateTime.Now.ToString("O");
      tradeChangeMessage.UserId = userId;
      tradeChangeMessage.EventType = eventType;
      tradeChangeMessage.Type = string.Format("urn:elli:{0}:{1}:trade:{2}", (object) (SourceEnum) (isSourceEncompass ? 0 : 1), (object) instanceId, (object) eventType).ToLower();
      tradeChangeMessage.Resource = new Resource()
      {
        Type = "trade",
        Id = tradeId,
        Reference = string.Format("secondary/v1/trades/correspondent/{0}", (object) tradeId)
      };
      tradeChangeMessage.Client = new Client()
      {
        ClientId = clientId,
        InstanceId = instanceId
      };
      tradeChangeMessage.Event = webhookEvent.payload;
      return tradeChangeMessage;
    }
  }
}
