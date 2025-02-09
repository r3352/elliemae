// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.MessageServices.Kafka.KafkaConsumer
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using Confluent.Kafka;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer.MessageServices.Kafka
{
  public class KafkaConsumer
  {
    private ConsumerBuilder<string, string> consumer;
    private string groupId = string.Empty;

    public KafkaConsumer(string groupId) => this.groupId = groupId;

    private List<KeyValuePair<string, string>> GetConsumerConfig()
    {
      List<KeyValuePair<string, string>> consumerConfig = new List<KeyValuePair<string, string>>();
      consumerConfig.AddRange((IEnumerable<KeyValuePair<string, string>>) KafkaUtils.ProducerConfigs);
      consumerConfig.Add(new KeyValuePair<string, string>("group.id", this.groupId));
      return consumerConfig;
    }

    public ConsumerBuilder<string, string> Consumer
    {
      get
      {
        if (this.consumer == null)
          this.consumer = new ConsumerBuilder<string, string>((IEnumerable<KeyValuePair<string, string>>) this.GetConsumerConfig());
        return this.consumer;
      }
    }
  }
}
