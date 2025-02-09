// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.GroupInfo
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

using System.Collections.Generic;

#nullable disable
namespace Confluent.Kafka
{
  /// <summary>
  ///     Encapsulates information describing a particular
  ///     Kafka group.
  /// </summary>
  public class GroupInfo
  {
    /// <summary>
    ///     Initializes a new instance of the GroupInfo class.
    /// </summary>
    /// <param name="broker">Originating broker info.</param>
    /// <param name="group">The group name.</param>
    /// <param name="error">
    ///     A rich <see cref="P:Confluent.Kafka.GroupInfo.Error" /> value associated with the information encapsulated by this class.
    /// </param>
    /// <param name="state">The group state.</param>
    /// <param name="protocolType">The group protocol type.</param>
    /// <param name="protocol">The group protocol.</param>
    /// <param name="members">The group members.</param>
    public GroupInfo(
      BrokerMetadata broker,
      string group,
      Error error,
      string state,
      string protocolType,
      string protocol,
      List<GroupMemberInfo> members)
    {
      this.Broker = broker;
      this.Group = group;
      this.Error = error;
      this.State = state;
      this.ProtocolType = protocolType;
      this.Protocol = protocol;
      this.Members = members;
    }

    /// <summary>Gets the originating-broker info.</summary>
    public BrokerMetadata Broker { get; }

    /// <summary>Gets the group name</summary>
    public string Group { get; }

    /// <summary>
    ///     Gets a rich <see cref="P:Confluent.Kafka.GroupInfo.Error" /> value associated with the information encapsulated by this class.
    /// </summary>
    public Error Error { get; }

    /// <summary>Gets the group state</summary>
    public string State { get; }

    /// <summary>Gets the group protocol type</summary>
    public string ProtocolType { get; }

    /// <summary>Gets the group protocol</summary>
    public string Protocol { get; }

    /// <summary>Gets the group members</summary>
    public List<GroupMemberInfo> Members { get; }
  }
}
