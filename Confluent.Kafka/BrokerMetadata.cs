// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.BrokerMetadata
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

#nullable disable
namespace Confluent.Kafka
{
  /// <summary>Metadata pertaining to a single Kafka broker.</summary>
  public class BrokerMetadata
  {
    /// <summary>Initializes a new BrokerMetadata class instance.</summary>
    /// <param name="brokerId">The Kafka broker id.</param>
    /// <param name="host">The Kafka broker hostname.</param>
    /// <param name="port">The Kafka broker port.</param>
    public BrokerMetadata(int brokerId, string host, int port)
    {
      this.BrokerId = brokerId;
      this.Host = host;
      this.Port = port;
    }

    /// <summary>Gets the Kafka broker id.</summary>
    public int BrokerId { get; }

    /// <summary>Gets the Kafka broker hostname.</summary>
    public string Host { get; }

    /// <summary>Gets the Kafka broker port.</summary>
    public int Port { get; }

    /// <summary>
    ///     Returns a JSON representation of the BrokerMetadata object.
    /// </summary>
    /// <returns>
    ///     A JSON representation of the BrokerMetadata object.
    /// </returns>
    public override string ToString()
    {
      return string.Format("{{ \"BrokerId\": {0}, \"Host\": \"{1}\", \"Port\": {2} }}", (object) this.BrokerId, (object) this.Host, (object) this.Port);
    }
  }
}
