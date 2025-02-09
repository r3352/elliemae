// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.TimeSpanExtensions
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

using System;

#nullable disable
namespace Confluent.Kafka
{
  /// <summary>
  ///     Extension methods for the <see cref="T:System.TimeSpan" /> class.
  /// </summary>
  internal static class TimeSpanExtensions
  {
    /// <summary>
    ///     Converts the TimeSpan value <paramref name="timespan" /> to an integer number of milliseconds.
    ///     An <see cref="T:System.OverflowException" /> is thrown if the number of milliseconds is greater than Int32.MaxValue.
    /// </summary>
    /// <param name="timespan">
    ///     The TimeSpan value to convert to milliseconds.
    /// </param>
    /// <returns>
    ///     The TimeSpan value <paramref name="timespan" /> in milliseconds.
    /// </returns>
    internal static int TotalMillisecondsAsInt(this TimeSpan timespan)
    {
      return checked ((int) timespan.TotalMilliseconds);
    }
  }
}
