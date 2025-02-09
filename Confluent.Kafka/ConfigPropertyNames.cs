// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.ConfigPropertyNames
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

#nullable disable
namespace Confluent.Kafka
{
  /// <summary>
  ///     Names of all configuration properties specific to the
  ///     .NET Client.
  /// </summary>
  public static class ConfigPropertyNames
  {
    /// <summary>
    ///     The maximum length of time (in milliseconds) before a cancellation request
    ///     is acted on. Low values may result in measurably higher CPU usage.
    /// 
    ///     default: 100
    ///     range: 1 &lt;= dotnet.cancellation.delay.max.ms &lt;= 10000
    /// </summary>
    public const string CancellationDelayMaxMs = "dotnet.cancellation.delay.max.ms";

    /// <summary>Producer specific configuration properties.</summary>
    public static class Producer
    {
      /// <summary>
      ///     Specifies whether or not the producer should start a background poll
      ///     thread to receive delivery reports and event notifications. Generally,
      ///     this should be set to true. If set to false, you will need to call
      ///     the Poll function manually.
      /// 
      ///     default: true
      /// </summary>
      public const string EnableBackgroundPoll = "dotnet.producer.enable.background.poll";
      /// <summary>
      ///     Specifies whether to enable notification of delivery reports. Typically
      ///     you should set this parameter to true. Set it to false for "fire and
      ///     forget" semantics and a small boost in performance.
      /// 
      ///     default: true
      /// </summary>
      public const string EnableDeliveryReports = "dotnet.producer.enable.delivery.reports";
      /// <summary>
      ///     A comma separated list of fields that may be optionally set in delivery
      ///     reports. Disabling delivery report fields that you do not require will
      ///     improve maximum throughput and reduce memory usage. Allowed values:
      ///     key, value, timestamp, headers, status, all, none.
      /// 
      ///     default: all
      /// </summary>
      public const string DeliveryReportFields = "dotnet.producer.delivery.report.fields";
    }

    /// <summary>Consumer specific configuration properties.</summary>
    public static class Consumer
    {
      /// <summary>
      ///     A comma separated list of fields that may be optionally set
      ///     in <see cref="T:Confluent.Kafka.ConsumeResult`2" />
      ///     objects returned by the
      ///     <see cref="M:Confluent.Kafka.Consumer`2.Consume(System.TimeSpan)" />
      ///     method. Disabling fields that you do not require will improve
      ///     throughput and reduce memory consumption. Allowed values:
      ///     headers, timestamp, topic, all, none
      /// 
      ///     default: all
      /// </summary>
      public const string ConsumeResultFields = "dotnet.consumer.consume.result.fields";
    }
  }
}
