// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.MessageServices.Kafka.KafkaEnums
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System.ComponentModel;

#nullable disable
namespace EllieMae.EMLite.ClientServer.MessageServices.Kafka
{
  public static class KafkaEnums
  {
    public enum ProducerConfigsEnum
    {
      [Description("bootstrap.servers")] BOOTSTRAP_SERVERS,
      [Description("request.timeout.ms")] REQUEST_TIMEOUT_MS,
      [Description("message.timeout.ms")] MESSAGE_TIMEOUT_MS,
      [Description("max.in.flight.requests.per.connection")] MAX_IN_FLIGHT_REQUESTS_PER_CONNECTION,
      [Description("retries")] RETRIES,
      [Description("security.protocol")] SECURITY_PROTOCOL,
      [Description("sasl.mechanisms")] SASL_MECHANISMS,
      [Description("sasl.username")] SASL_USERNAME,
      [Description("sasl.password")] SASL_PASSWORD,
      [Description("ssl.ca.location")] SSL_CA_LOCATION,
      [Description("acks")] ACKS,
      [Description("retry.backoff.ms")] RETRY_BACKOFF_MS,
      [Description("linger.ms")] LINGER_MS,
      [Description("debug")] DEBUG,
      [Description("log_level")] LOG_LEVEL,
    }

    public enum ProducerSettingsEnum
    {
      [Description("Kafka.Deployment.Profile")] KAFKA_DEPLOYMENT_PROFILE,
      [Description("Kafka.Aws.Region")] KAFKA_AWS_REGION,
      [Description("Kafka.Region")] KAFKA_REGION,
      [Description("Kafka.CACert.Profile")] KAFKA_CACERT_PROFILE,
      [Description("DisableQueueMessageLog")] DISABLEQUEUEMESSAGELOG,
      [Description("Kafka.EnableDebugLog")] KAFKA_ENABLEDEBUGLOG,
      [Description("Kafka.EnableWarningLog")] KAFKA_ENABLEWARNINGLOG,
      [Description("Kafka.Debug.Options")] KAFKA_DEBUG_OPTIONS,
      [Description("Kafka.Producer.EnableNoKey")] KAFKA_PRODUCER_ENABLENOKEY,
      [Description("Kafka.Producer.Async.Enabled")] KAFKA_PRODUCER_ASYNC_ENABLED,
      [Description("Kafka.Producer.Sync.Timeout")] KAFKA_PRODUCERf_SYNC_TIMEOUT,
      [Description("Kafka.Producer.EnableFlush")] KAFKA_PRODUCER_ENABLEFLUSH,
      [Description("Kafka.Producer.SyncProduceMethod")] KAFKA_PRODUCE_METHOD_FLAG,
      [Description("Kafka.Producer.StaticProducer")] KAFKA_STATIC_PRODUCER,
      [Description("Kafka.Producer.EnableAsyncProducer")] KAFKA_ENABLE_ASYNC_PRODUCER,
      [Description("Kafka.Producer.EnableAggregatedLogging")] KAFKA_ENABLE_AGGREGATED_LOGGING,
    }
  }
}
