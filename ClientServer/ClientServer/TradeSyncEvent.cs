// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.TradeSyncEvent
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public class TradeSyncEvent : MessageQueueEvent
  {
    private static readonly string Topic = string.Format("{0}.{1}.{2}", (object) KafkaUtils.DeploymentProfile, (object) KafkaUtils.AwsRegion, (object) "loan.tradesync");
    private List<TradeSyncQueueMessage> messages = new List<TradeSyncQueueMessage>();

    public TradeSyncEvent(StandardMessage standardMessage, Dictionary<string, object> databag)
    {
      if (this.QueueMessages == null)
        this.QueueMessages = new List<QueueMessage>();
      this.StandardMessage = standardMessage;
      this.DataBag = databag;
      this.GenerateMessage();
    }

    private void GenerateMessage()
    {
      foreach (QueueMessage queueMessage in this.DataBag.Values)
        this.QueueMessages.Add(queueMessage);
    }

    public override string GetTopic(string messageType) => TradeSyncEvent.Topic;
  }
}
