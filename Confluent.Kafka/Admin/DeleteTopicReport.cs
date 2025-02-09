// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.Admin.DeleteTopicReport
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

#nullable disable
namespace Confluent.Kafka.Admin
{
  /// <summary>
  ///     The result of a request to delete a specific topic.
  /// </summary>
  public class DeleteTopicReport
  {
    /// <summary>The topic.</summary>
    public string Topic { get; set; }

    /// <summary>
    ///     The error (or success) of the delete topic request.
    /// </summary>
    public Error Error { get; set; }
  }
}
