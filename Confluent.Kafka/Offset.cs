// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.Offset
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

using System;

#nullable disable
namespace Confluent.Kafka
{
  /// <summary>Represents a Kafka partition offset value.</summary>
  /// <remarks>
  ///     This structure is the same size as a long -
  ///     its purpose is to add some syntactical sugar
  ///     related to special values.
  /// </remarks>
  /// <summary>
  ///     Initializes a new instance of the Offset structure.
  /// </summary>
  /// <param name="offset">The offset value</param>
  public struct Offset(long offset) : IEquatable<Offset>
  {
    private const long RD_KAFKA_OFFSET_BEGINNING = -2;
    private const long RD_KAFKA_OFFSET_END = -1;
    private const long RD_KAFKA_OFFSET_STORED = -1000;
    private const long RD_KAFKA_OFFSET_INVALID = -1001;
    /// <summary>
    ///     A special value that refers to the beginning of a partition.
    /// </summary>
    public static readonly Offset Beginning = new Offset(-2L);
    /// <summary>
    ///     A special value that refers to the end of a partition.
    /// </summary>
    public static readonly Offset End = new Offset(-1L);
    /// <summary>
    ///     A special value that refers to the stored offset for a partition.
    /// </summary>
    public static readonly Offset Stored = new Offset(-1000L);
    /// <summary>
    ///     A special value that refers to an invalid, unassigned or default partition offset.
    /// </summary>
    public static readonly Offset Unset = new Offset(-1001L);

    /// <summary>Gets the long value corresponding to this offset.</summary>
    public long Value { get; } = offset;

    /// <summary>
    ///     Gets whether or not this is one of the special
    ///     offset values.
    /// </summary>
    public bool IsSpecial
    {
      get => this.Value == -2L || this.Value == -1L || this.Value == -1000L || this.Value == -1001L;
    }

    /// <summary>
    ///     Tests whether this Offset value is equal to the specified object.
    /// </summary>
    /// <param name="obj">The object to test.</param>
    /// <returns>
    ///     true if obj is an Offset and has the same value. false otherwise.
    /// </returns>
    public override bool Equals(object obj) => obj is Offset other && this.Equals(other);

    /// <summary>
    ///     Tests whether this Offset value is equal to the specified Offset.
    /// </summary>
    /// <param name="other">The offset to test.</param>
    /// <returns>
    ///     true if other has the same value. false otherwise.
    /// </returns>
    public bool Equals(Offset other) => other.Value == this.Value;

    /// <summary>
    ///     Tests whether Offset value a is equal to Offset value b.
    /// </summary>
    /// <param name="a">The first Offset value to compare.</param>
    /// <param name="b">The second Offset value to compare.</param>
    /// <returns>
    ///     true if Offset value a and b are equal. false otherwise.
    /// </returns>
    public static bool operator ==(Offset a, Offset b) => a.Equals(b);

    /// <summary>
    ///     Tests whether Offset value a is not equal to Offset value b.
    /// </summary>
    /// <param name="a">The first Offset value to compare.</param>
    /// <param name="b">The second Offset value to compare.</param>
    /// <returns>
    ///     true if Offset value a and b are not equal. false otherwise.
    /// </returns>
    public static bool operator !=(Offset a, Offset b) => !(a == b);

    /// <summary>
    ///     Tests whether Offset value a is greater than Offset value b.
    /// </summary>
    /// <param name="a">The first Offset value to compare.</param>
    /// <param name="b">The second Offset value to compare.</param>
    /// <returns>
    ///     true if Offset value a is greater than Offset value b. false otherwise.
    /// </returns>
    public static bool operator >(Offset a, Offset b) => a.Value > b.Value;

    /// <summary>
    ///     Tests whether Offset value a is less than Offset value b.
    /// </summary>
    /// <param name="a">The first Offset value to compare.</param>
    /// <param name="b">The second Offset value to compare.</param>
    /// <returns>
    ///     true if Offset value a is less than Offset value b. false otherwise.
    /// </returns>
    public static bool operator <(Offset a, Offset b) => a.Value < b.Value;

    /// <summary>
    ///     Tests whether Offset value a is greater than or equal to Offset value b.
    /// </summary>
    /// <param name="a">The first Offset value to compare.</param>
    /// <param name="b">The second Offset value to compare.</param>
    /// <returns>
    ///     true if Offset value a is greater than or equal to Offset value b. false otherwise.
    /// </returns>
    public static bool operator >=(Offset a, Offset b) => a.Value >= b.Value;

    /// <summary>
    ///     Tests whether Offset value a is less than or equal to Offset value b.
    /// </summary>
    /// <param name="a">The first Offset value to compare.</param>
    /// <param name="b">The second Offset value to compare.</param>
    /// <returns>
    ///     true if Offset value a is less than or equal to Offset value b. false otherwise.
    /// </returns>
    public static bool operator <=(Offset a, Offset b) => a.Value <= b.Value;

    /// <summary>Add an integer value to an Offset value.</summary>
    /// <param name="a">
    ///     The Offset value to add the integer value to.
    /// </param>
    /// <param name="b">
    ///     The integer value to add to the Offset value.
    /// </param>
    /// <returns>
    ///     The Offset value incremented by the integer value b.
    /// </returns>
    public static Offset operator +(Offset a, int b) => new Offset(a.Value + (long) b);

    /// <summary>Add a long value to an Offset value.</summary>
    /// <param name="a">The Offset value to add the long value to.</param>
    /// <param name="b">The long value to add to the Offset value.</param>
    /// <returns>The Offset value incremented by the long value b.</returns>
    public static Offset operator +(Offset a, long b) => new Offset(a.Value + b);

    /// <summary>Returns a hash code for this Offset.</summary>
    /// <returns>
    ///     An integer that specifies a hash value for this Offset.
    /// </returns>
    public override int GetHashCode() => this.Value.GetHashCode();

    /// <summary>
    ///     Converts the specified long value to an Offset value.
    /// </summary>
    /// <param name="v">The long value to convert.</param>
    public static implicit operator Offset(long v) => new Offset(v);

    /// <summary>
    ///     Converts the specified Offset value to a long value.
    /// </summary>
    /// <param name="o">The Offset value to convert.</param>
    public static implicit operator long(Offset o) => o.Value;

    /// <summary>
    ///     Returns a string representation of the Offset object.
    /// </summary>
    /// <returns>A string that represents the Offset object.</returns>
    public override string ToString()
    {
      switch (this.Value)
      {
        case -1001:
          return string.Format("Unset [{0}]", (object) -1001L);
        case -1000:
          return string.Format("Stored [{0}]", (object) -1000L);
        case -2:
          return string.Format("Beginning [{0}]", (object) -2L);
        case -1:
          return string.Format("End [{0}]", (object) -1L);
        default:
          return this.Value.ToString();
      }
    }
  }
}
