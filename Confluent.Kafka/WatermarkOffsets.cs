// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.WatermarkOffsets
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

#nullable disable
namespace Confluent.Kafka
{
  /// <summary>
  ///     Represents the low and high watermark offsets of a Kafka
  ///     topic/partition.
  /// </summary>
  /// <remarks>
  ///     You can identify a partition that has not yet been written
  ///     to by checking if the high watermark equals 0.
  /// </remarks>
  public class WatermarkOffsets
  {
    /// <summary>
    ///     Initializes a new instance of the WatermarkOffsets class
    ///     with the specified offsets.
    /// </summary>
    /// <param name="low">
    ///     The offset of the earliest message in the topic/partition. If
    ///     no messages have been written to the topic, the low watermark
    ///     offset is set to 0. The low watermark will also be 0 if
    ///     one message has been written to the partition (with offset 0).
    /// </param>
    /// <param name="high">
    ///     The high watermark offset, which is the offset of the latest
    ///     message in the topic/partition available for consumption + 1.
    /// </param>
    public WatermarkOffsets(Offset low, Offset high)
    {
      this.Low = low;
      this.High = high;
    }

    /// <summary>
    ///     Gets the offset of the earliest message in the topic/partition. If
    ///     no messages have been written to the topic, the low watermark
    ///     offset is set to 0. The low watermark will also be 0 if
    ///     one message has been written to the partition (with offset 0).
    /// </summary>
    public Offset Low { get; }

    /// <summary>
    ///     Gets the high watermark offset, which is the offset of the latest
    ///     message in the topic/partition available for consumption + 1.
    /// </summary>
    public Offset High { get; }

    /// <summary>
    ///     Returns a string representation of the WatermarkOffsets object.
    /// </summary>
    /// <returns>
    ///     A string representation of the WatermarkOffsets object.
    /// </returns>
    public override string ToString()
    {
      return string.Format("{0} .. {1}", (object) this.Low, (object) this.High);
    }
  }
}
