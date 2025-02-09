// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.Admin.PartitionsSpecification
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

using System.Collections.Generic;

#nullable disable
namespace Confluent.Kafka.Admin
{
  /// <summary>
  ///     Specification for new partitions to be added to a topic.
  /// </summary>
  public class PartitionsSpecification
  {
    /// <summary>
    ///     The topic that the new partitions specification corresponds to.
    /// </summary>
    public string Topic { get; set; }

    /// <summary>
    ///     The replica assignments for the new partitions, or null if the assignment
    ///     will be done by the controller. The outer list is indexed by the new
    ///     partitions relative index, and the inner list contains the broker ids.
    /// </summary>
    public List<List<int>> ReplicaAssignments { get; set; }

    /// <summary>
    ///     The partition count for the specified topic is increased to this value.
    /// </summary>
    public int IncreaseTo { get; set; }
  }
}
