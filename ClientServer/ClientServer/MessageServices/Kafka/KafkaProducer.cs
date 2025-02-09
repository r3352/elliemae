// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.MessageServices.Kafka.KafkaProducer
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using Confluent.Kafka;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer.MessageServices.Kafka
{
  public static class KafkaProducer
  {
    private static ProducerBuilder<string, string> producer;

    public static ProducerBuilder<string, string> Producer
    {
      get
      {
        if (KafkaProducer.producer == null)
          KafkaProducer.producer = new ProducerBuilder<string, string>((IEnumerable<KeyValuePair<string, string>>) KafkaUtils.ProducerConfigs);
        return KafkaProducer.producer;
      }
    }

    public static IProducer<string, string> StaticProducer { get; } = new ProducerBuilder<string, string>((IEnumerable<KeyValuePair<string, string>>) KafkaUtils.ProducerConfigs).Build();
  }
}
