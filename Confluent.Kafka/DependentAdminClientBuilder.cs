// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.DependentAdminClientBuilder
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

#nullable disable
namespace Confluent.Kafka
{
  /// <summary>
  ///     A builder class for <see cref="T:Confluent.Kafka.IAdminClient" /> instance
  ///     implementations that leverage an existing client handle.
  /// </summary>
  public class DependentAdminClientBuilder
  {
    /// <summary>The configured client handle.</summary>
    public Handle Handle { get; set; }

    /// <summary>
    ///     An underlying librdkafka client handle that the AdminClient.
    /// </summary>
    public DependentAdminClientBuilder(Handle handle) => this.Handle = handle;

    /// <summary>Build a new IAdminClient implementation instance.</summary>
    public virtual IAdminClient Build() => (IAdminClient) new AdminClient(this.Handle);
  }
}
