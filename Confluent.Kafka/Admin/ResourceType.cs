// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.Admin.ResourceType
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

#nullable disable
namespace Confluent.Kafka.Admin
{
  /// <summary>
  ///     Enumerates the set of configuration resource types.
  /// </summary>
  public enum ResourceType
  {
    /// <summary>Unknown resource</summary>
    Unknown,
    /// <summary>Any resource</summary>
    Any,
    /// <summary>Topic resource</summary>
    Topic,
    /// <summary>Group resource</summary>
    Group,
    /// <summary>Broker resource</summary>
    Broker,
  }
}
