// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.MessageQueueEventParameters
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public class MessageQueueEventParameters
  {
    public string InstanceId { get; private set; }

    public string MessageType { get; private set; }

    public string Topic { get; private set; }

    public string EntityId { get; private set; }

    public QueueMessage QueueMessage { get; private set; }

    public MessageQueueEventParameters(
      string instanceId,
      string messageType,
      string topic,
      string entityId,
      QueueMessage queueMessage)
    {
      this.InstanceId = instanceId;
      this.MessageType = messageType;
      this.Topic = topic;
      this.EntityId = entityId;
      this.QueueMessage = queueMessage;
    }
  }
}
