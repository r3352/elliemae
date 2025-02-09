// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServiceObjects.KafkaEvent.KafkaEventMessage
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;

#nullable disable
namespace EllieMae.EMLite.Server.ServiceObjects.KafkaEvent
{
  internal class KafkaEventMessage
  {
    public string SourceName { get; set; }

    public QueueMessage Message { get; set; }

    public string Topic { get; set; }

    public string PublishTime { get; set; }

    public string Payload { get; set; }

    public string CorrelationId { get; set; }

    public string EntityId { get; set; }

    public string InstanceId { get; private set; }

    public string MessageType { get; private set; }

    public KafkaEventMessage(
      string sourceName,
      string topic,
      string entityId,
      string instanceId,
      string messageType,
      string payload,
      QueueMessage message)
    {
      this.SourceName = sourceName;
      this.Topic = topic;
      this.PublishTime = message.PublishTime;
      this.CorrelationId = message.CorrelationId;
      this.EntityId = entityId;
      this.Message = message;
      this.Payload = payload;
      this.InstanceId = instanceId;
      this.MessageType = messageType;
    }
  }
}
