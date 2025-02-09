// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.Admin.ConfigSource
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

#nullable disable
namespace Confluent.Kafka.Admin
{
  /// <summary>Enumerates the different config sources.</summary>
  public enum ConfigSource
  {
    /// <summary>Unknown</summary>
    UnknownConfig,
    /// <summary>Dynamic Topic</summary>
    DynamicTopicConfig,
    /// <summary>Dynamic Broker</summary>
    DynamicBrokerConfig,
    /// <summary>Dynamic Default Broker</summary>
    DynamicDefaultBrokerConfig,
    /// <summary>Static</summary>
    StaticBrokerConfig,
    /// <summary>Default</summary>
    DefaultConfig,
  }
}
