// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServiceObjects.KafkaEvent.MessageQueueEventService
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Server.ServiceObjects.Repositories;
using EllieMae.EMLite.ServiceInterface;
using EllieMae.EMLite.ServiceInterface.Repositories;
using System;

#nullable disable
namespace EllieMae.EMLite.Server.ServiceObjects.KafkaEvent
{
  public class MessageQueueEventService : IMessageQueueEventService, IContextBoundObject
  {
    public IQueueMessageRepository messageHistoryRepository = (IQueueMessageRepository) new QueueMessageRepository();

    public void MessageQueueProducer(
      MessageQueueEvent messageQueueEvent,
      IMessageQueueProcessor messageQueueProcessor)
    {
      messageQueueProcessor.Publish(messageQueueEvent);
    }

    public void MessageQueueConsumer() => throw new NotImplementedException();

    public IServiceContext ServiceContext => throw new NotImplementedException();
  }
}
