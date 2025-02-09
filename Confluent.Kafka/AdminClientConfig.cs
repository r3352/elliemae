// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.AdminClientConfig
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

using System.Collections.Generic;

#nullable disable
namespace Confluent.Kafka
{
  /// <summary>AdminClient configuration properties</summary>
  public class AdminClientConfig : ClientConfig
  {
    /// <summary>
    ///     Initialize a new empty <see cref="T:Confluent.Kafka.AdminClientConfig" /> instance.
    /// </summary>
    public AdminClientConfig()
    {
    }

    /// <summary>
    ///     Initialize a new <see cref="T:Confluent.Kafka.AdminClientConfig" /> instance wrapping
    ///     an existing <see cref="T:Confluent.Kafka.ClientConfig" /> instance.
    ///     This will change the values "in-place" i.e. operations on this class WILL modify the provided collection
    /// </summary>
    public AdminClientConfig(ClientConfig config)
      : base(config)
    {
    }

    /// <summary>
    ///     Initialize a new <see cref="T:Confluent.Kafka.AdminClientConfig" /> instance wrapping
    ///     an existing key/value pair collection.
    ///     This will change the values "in-place" i.e. operations on this class WILL modify the provided collection
    /// </summary>
    public AdminClientConfig(IDictionary<string, string> config)
      : base(config)
    {
    }
  }
}
