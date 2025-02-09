// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.IClient
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

using System;

#nullable disable
namespace Confluent.Kafka
{
  /// <summary>Defines methods common to all client types.</summary>
  public interface IClient : IDisposable
  {
    /// <summary>
    ///     An opaque reference to the underlying
    ///     librdkafka client instance. This can be used
    ///     to construct an AdminClient that utilizes the
    ///     same underlying librdkafka client as this
    ///     instance.
    /// </summary>
    Handle Handle { get; }

    /// <summary>
    ///     Gets the name of this client instance.
    /// 
    ///     Contains (but is not equal to) the client.id
    ///     configuration parameter.
    /// </summary>
    /// <remarks>
    ///     This name will be unique across all client
    ///     instances in a given application which allows
    ///     log messages to be associated with the
    ///     corresponding instance.
    /// </remarks>
    string Name { get; }

    /// <summary>
    ///     Adds one or more brokers to the Client's list
    ///     of initial bootstrap brokers.
    /// 
    ///     Note: Additional brokers are discovered
    ///     automatically as soon as the Client connects
    ///     to any broker by querying the broker metadata.
    ///     Calling this method is only required in some
    ///     scenarios where the address of all brokers in
    ///     the cluster changes.
    /// </summary>
    /// <param name="brokers">
    ///     Comma-separated list of brokers in
    ///     the same format as the bootstrap.server
    ///     configuration parameter.
    /// </param>
    /// <remarks>
    ///     There is currently no API to remove existing
    ///     configured, added or learnt brokers.
    /// </remarks>
    /// <returns>
    ///     The number of brokers added. This value
    ///     includes brokers that may have been specified
    ///     a second time.
    /// </returns>
    int AddBrokers(string brokers);
  }
}
