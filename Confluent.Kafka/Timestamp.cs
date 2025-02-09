// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.Timestamp
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

using System;

#nullable disable
namespace Confluent.Kafka
{
  /// <summary>Encapsulates a Kafka timestamp and its type.</summary>
  public struct Timestamp : IEquatable<Timestamp>
  {
    private const long RD_KAFKA_NO_TIMESTAMP = 0;
    /// <summary>
    ///     Unix epoch as a UTC DateTime. Unix time is defined as
    ///     the number of seconds past this UTC time, excluding
    ///     leap seconds.
    /// </summary>
    public static readonly DateTime UnixTimeEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    private const long UnixTimeEpochMilliseconds = 62135596800000;

    /// <summary>
    ///     A read-only field representing an unspecified timestamp.
    /// </summary>
    public static Timestamp Default => new Timestamp(0L, TimestampType.NotAvailable);

    /// <summary>
    ///     Initializes a new instance of the Timestamp structure.
    /// </summary>
    /// <param name="unixTimestampMs">
    ///     The unix millisecond timestamp.
    /// </param>
    /// <param name="type">The type of the timestamp.</param>
    public Timestamp(long unixTimestampMs, TimestampType type)
    {
      this.Type = type;
      this.UnixTimestampMs = unixTimestampMs;
    }

    /// <summary>
    ///     Initializes a new instance of the Timestamp structure.
    ///     Note: <paramref name="dateTime" /> is first converted to UTC
    ///     if it is not already.
    /// </summary>
    /// <param name="dateTime">
    ///     The DateTime value corresponding to the timestamp.
    /// </param>
    /// <param name="type">The type of the timestamp.</param>
    public Timestamp(DateTime dateTime, TimestampType type)
    {
      this.Type = type;
      this.UnixTimestampMs = Timestamp.DateTimeToUnixTimestampMs(dateTime);
    }

    /// <summary>
    ///     Initializes a new instance of the Timestamp structure.
    ///     Note: <paramref name="dateTime" /> is first converted
    ///     to UTC if it is not already and TimestampType is set
    ///     to CreateTime.
    /// </summary>
    /// <param name="dateTime">
    ///     The DateTime value corresponding to the timestamp.
    /// </param>
    public Timestamp(DateTime dateTime)
      : this(dateTime, TimestampType.CreateTime)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the Timestamp structure.
    ///     Note: TimestampType is set to CreateTime.
    /// </summary>
    /// <param name="dateTimeOffset">
    ///     The DateTimeOffset value corresponding to the timestamp.
    /// </param>
    public Timestamp(DateTimeOffset dateTimeOffset)
      : this(dateTimeOffset.UtcDateTime, TimestampType.CreateTime)
    {
    }

    /// <summary>Gets the timestamp type.</summary>
    public TimestampType Type { get; }

    /// <summary>Get the Unix millisecond timestamp.</summary>
    public long UnixTimestampMs { get; }

    /// <summary>
    ///     Gets the UTC DateTime corresponding to the <see cref="P:Confluent.Kafka.Timestamp.UnixTimestampMs" />.
    /// </summary>
    public DateTime UtcDateTime => Timestamp.UnixTimestampMsToDateTime(this.UnixTimestampMs);

    /// <summary>
    ///     Determines whether two Timestamps have the same value.
    /// </summary>
    /// <param name="obj">
    ///     Determines whether this instance and a specified object,
    ///     which must also be a Timestamp object, have the same value.
    /// </param>
    /// <returns>
    ///     true if obj is a Timestamp and its value is the same as
    ///     this instance; otherwise, false. If obj is null, the method
    ///     returns false.
    /// </returns>
    public override bool Equals(object obj) => obj is Timestamp other && this.Equals(other);

    /// <summary>
    ///     Determines whether two Timestamps have the same value.
    /// </summary>
    /// <param name="other">The timestamp to test.</param>
    /// <returns>
    ///     true if other has the same value. false otherwise.
    /// </returns>
    public bool Equals(Timestamp other)
    {
      return other.Type == this.Type && other.UnixTimestampMs == this.UnixTimestampMs;
    }

    /// <summary>Returns the hashcode for this Timestamp.</summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    public override int GetHashCode()
    {
      return this.Type.GetHashCode() * 251 + this.UnixTimestampMs.GetHashCode();
    }

    /// <summary>
    ///     Determines whether two specified Timestamps have the same value.
    /// </summary>
    /// <param name="a">The first Timestamp to compare.</param>
    /// <param name="b">The second Timestamp to compare</param>
    /// <returns>
    ///     true if the value of a is the same as the value of b; otherwise, false.
    /// </returns>
    public static bool operator ==(Timestamp a, Timestamp b) => a.Equals(b);

    /// <summary>
    ///     Determines whether two specified Timestamps have different values.
    /// </summary>
    /// <param name="a">The first Timestamp to compare.</param>
    /// <param name="b">The second Timestamp to compare</param>
    /// <returns>
    ///     true if the value of a is different from the value of b; otherwise, false.
    /// </returns>
    public static bool operator !=(Timestamp a, Timestamp b) => !(a == b);

    /// <summary>
    ///     Convert a DateTime instance to a milliseconds unix timestamp.
    ///     Note: <paramref name="dateTime" /> is first converted to UTC
    ///     if it is not already.
    /// </summary>
    /// <param name="dateTime">The DateTime value to convert.</param>
    /// <returns>
    ///     The milliseconds unix timestamp corresponding to <paramref name="dateTime" />
    ///     rounded down to the previous millisecond.
    /// </returns>
    public static long DateTimeToUnixTimestampMs(DateTime dateTime)
    {
      return dateTime.ToUniversalTime().Ticks / 10000L - 62135596800000L;
    }

    /// <summary>
    ///     Convert a milliseconds unix timestamp to a DateTime value.
    /// </summary>
    /// <param name="unixMillisecondsTimestamp">
    ///     The milliseconds unix timestamp to convert.
    /// </param>
    /// <returns>
    ///     The DateTime value associated with <paramref name="unixMillisecondsTimestamp" /> with Utc Kind.
    /// </returns>
    public static DateTime UnixTimestampMsToDateTime(long unixMillisecondsTimestamp)
    {
      return Timestamp.UnixTimeEpoch + TimeSpan.FromMilliseconds((double) unixMillisecondsTimestamp);
    }
  }
}
