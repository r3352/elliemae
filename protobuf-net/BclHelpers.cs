// Decompiled with JetBrains decompiler
// Type: ProtoBuf.BclHelpers
// Assembly: protobuf-net, Version=2.4.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 4CB0BA27-6305-4C8A-8F23-3BBBCE6B7DCC
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.xml

using System;
using System.Runtime.Serialization;

#nullable disable
namespace ProtoBuf
{
  /// <summary>
  /// Provides support for common .NET types that do not have a direct representation
  /// in protobuf, using the definitions from bcl.proto
  /// </summary>
  public static class BclHelpers
  {
    private const int FieldTimeSpanValue = 1;
    private const int FieldTimeSpanScale = 2;
    private const int FieldTimeSpanKind = 3;
    internal static readonly DateTime[] EpochOrigin = new DateTime[3]
    {
      new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
      new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
      new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local)
    };
    /// <summary>
    /// The default value for dates that are following google.protobuf.Timestamp semantics
    /// </summary>
    private static readonly DateTime TimestampEpoch = BclHelpers.EpochOrigin[1];
    private const int FieldDecimalLow = 1;
    private const int FieldDecimalHigh = 2;
    private const int FieldDecimalSignScale = 3;
    private const int FieldGuidLow = 1;
    private const int FieldGuidHigh = 2;
    private const int FieldExistingObjectKey = 1;
    private const int FieldNewObjectKey = 2;
    private const int FieldExistingTypeKey = 3;
    private const int FieldNewTypeKey = 4;
    private const int FieldTypeName = 8;
    private const int FieldObject = 10;

    /// <summary>
    /// Creates a new instance of the specified type, bypassing the constructor.
    /// </summary>
    /// <param name="type">The type to create</param>
    /// <returns>The new instance</returns>
    /// <exception cref="T:System.NotSupportedException">If the platform does not support constructor-skipping</exception>
    public static object GetUninitializedObject(Type type)
    {
      return FormatterServices.GetUninitializedObject(type);
    }

    /// <summary>
    /// Writes a TimeSpan to a protobuf stream using protobuf-net's own representation, bcl.TimeSpan
    /// </summary>
    public static void WriteTimeSpan(TimeSpan timeSpan, ProtoWriter dest)
    {
      BclHelpers.WriteTimeSpanImpl(timeSpan, dest, DateTimeKind.Unspecified);
    }

    private static void WriteTimeSpanImpl(TimeSpan timeSpan, ProtoWriter dest, DateTimeKind kind)
    {
      if (dest == null)
        throw new ArgumentNullException(nameof (dest));
      switch (dest.WireType)
      {
        case WireType.Fixed64:
          ProtoWriter.WriteInt64(timeSpan.Ticks, dest);
          break;
        case WireType.String:
        case WireType.StartGroup:
          long num = timeSpan.Ticks;
          TimeSpanScale timeSpanScale;
          if (timeSpan == TimeSpan.MaxValue)
          {
            num = 1L;
            timeSpanScale = TimeSpanScale.MinMax;
          }
          else if (timeSpan == TimeSpan.MinValue)
          {
            num = -1L;
            timeSpanScale = TimeSpanScale.MinMax;
          }
          else if (num % 864000000000L == 0L)
          {
            timeSpanScale = TimeSpanScale.Days;
            num /= 864000000000L;
          }
          else if (num % 36000000000L == 0L)
          {
            timeSpanScale = TimeSpanScale.Hours;
            num /= 36000000000L;
          }
          else if (num % 600000000L == 0L)
          {
            timeSpanScale = TimeSpanScale.Minutes;
            num /= 600000000L;
          }
          else if (num % 10000000L == 0L)
          {
            timeSpanScale = TimeSpanScale.Seconds;
            num /= 10000000L;
          }
          else if (num % 10000L == 0L)
          {
            timeSpanScale = TimeSpanScale.Milliseconds;
            num /= 10000L;
          }
          else
            timeSpanScale = TimeSpanScale.Ticks;
          SubItemToken token = ProtoWriter.StartSubItem((object) null, dest);
          if (num != 0L)
          {
            ProtoWriter.WriteFieldHeader(1, WireType.SignedVariant, dest);
            ProtoWriter.WriteInt64(num, dest);
          }
          if (timeSpanScale != TimeSpanScale.Days)
          {
            ProtoWriter.WriteFieldHeader(2, WireType.Variant, dest);
            ProtoWriter.WriteInt32((int) timeSpanScale, dest);
          }
          if (kind != DateTimeKind.Unspecified)
          {
            ProtoWriter.WriteFieldHeader(3, WireType.Variant, dest);
            ProtoWriter.WriteInt32((int) kind, dest);
          }
          ProtoWriter.EndSubItem(token, dest);
          break;
        default:
          throw new ProtoException("Unexpected wire-type: " + dest.WireType.ToString());
      }
    }

    /// <summary>
    /// Parses a TimeSpan from a protobuf stream using protobuf-net's own representation, bcl.TimeSpan
    /// </summary>
    public static TimeSpan ReadTimeSpan(ProtoReader source)
    {
      long num = BclHelpers.ReadTimeSpanTicks(source, out DateTimeKind _);
      switch (num)
      {
        case long.MinValue:
          return TimeSpan.MinValue;
        case long.MaxValue:
          return TimeSpan.MaxValue;
        default:
          return TimeSpan.FromTicks(num);
      }
    }

    /// <summary>
    /// Parses a TimeSpan from a protobuf stream using the standardized format, google.protobuf.Duration
    /// </summary>
    public static TimeSpan ReadDuration(ProtoReader source)
    {
      long seconds = 0;
      int nanos = 0;
      SubItemToken token = ProtoReader.StartSubItem(source);
      int num;
      while ((num = source.ReadFieldHeader()) > 0)
      {
        switch (num)
        {
          case 1:
            seconds = source.ReadInt64();
            continue;
          case 2:
            nanos = source.ReadInt32();
            continue;
          default:
            source.SkipField();
            continue;
        }
      }
      ProtoReader.EndSubItem(token, source);
      return BclHelpers.FromDurationSeconds(seconds, nanos);
    }

    /// <summary>
    /// Writes a TimeSpan to a protobuf stream using the standardized format, google.protobuf.Duration
    /// </summary>
    public static void WriteDuration(TimeSpan value, ProtoWriter dest)
    {
      int nanos;
      BclHelpers.WriteSecondsNanos(BclHelpers.ToDurationSeconds(value, out nanos), nanos, dest);
    }

    private static void WriteSecondsNanos(long seconds, int nanos, ProtoWriter dest)
    {
      SubItemToken token = ProtoWriter.StartSubItem((object) null, dest);
      if (seconds != 0L)
      {
        ProtoWriter.WriteFieldHeader(1, WireType.Variant, dest);
        ProtoWriter.WriteInt64(seconds, dest);
      }
      if (nanos != 0)
      {
        ProtoWriter.WriteFieldHeader(2, WireType.Variant, dest);
        ProtoWriter.WriteInt32(nanos, dest);
      }
      ProtoWriter.EndSubItem(token, dest);
    }

    /// <summary>
    /// Parses a DateTime from a protobuf stream using the standardized format, google.protobuf.Timestamp
    /// </summary>
    public static DateTime ReadTimestamp(ProtoReader source)
    {
      return BclHelpers.TimestampEpoch + BclHelpers.ReadDuration(source);
    }

    /// <summary>
    /// Writes a DateTime to a protobuf stream using the standardized format, google.protobuf.Timestamp
    /// </summary>
    public static void WriteTimestamp(DateTime value, ProtoWriter dest)
    {
      int nanos;
      long durationSeconds = BclHelpers.ToDurationSeconds(value - BclHelpers.TimestampEpoch, out nanos);
      if (nanos < 0)
      {
        --durationSeconds;
        nanos += 1000000000;
      }
      BclHelpers.WriteSecondsNanos(durationSeconds, nanos, dest);
    }

    private static TimeSpan FromDurationSeconds(long seconds, int nanos)
    {
      return TimeSpan.FromTicks(checked (seconds * 10000000L + unchecked (checked ((long) nanos * 10000L) / 1000000L)));
    }

    private static long ToDurationSeconds(TimeSpan value, out int nanos)
    {
      nanos = (int) (value.Ticks % 10000000L * 1000000L / 10000L);
      return value.Ticks / 10000000L;
    }

    /// <summary>Parses a DateTime from a protobuf stream</summary>
    public static DateTime ReadDateTime(ProtoReader source)
    {
      DateTimeKind kind;
      long num = BclHelpers.ReadTimeSpanTicks(source, out kind);
      switch (num)
      {
        case long.MinValue:
          return DateTime.MinValue;
        case long.MaxValue:
          return DateTime.MaxValue;
        default:
          return BclHelpers.EpochOrigin[(int) kind].AddTicks(num);
      }
    }

    /// <summary>
    /// Writes a DateTime to a protobuf stream, excluding the <c>Kind</c>
    /// </summary>
    public static void WriteDateTime(DateTime value, ProtoWriter dest)
    {
      BclHelpers.WriteDateTimeImpl(value, dest, false);
    }

    /// <summary>
    /// Writes a DateTime to a protobuf stream, including the <c>Kind</c>
    /// </summary>
    public static void WriteDateTimeWithKind(DateTime value, ProtoWriter dest)
    {
      BclHelpers.WriteDateTimeImpl(value, dest, true);
    }

    private static void WriteDateTimeImpl(DateTime value, ProtoWriter dest, bool includeKind)
    {
      if (dest == null)
        throw new ArgumentNullException(nameof (dest));
      TimeSpan timeSpan;
      switch (dest.WireType)
      {
        case WireType.String:
        case WireType.StartGroup:
          if (value == DateTime.MaxValue)
          {
            timeSpan = TimeSpan.MaxValue;
            includeKind = false;
            break;
          }
          if (value == DateTime.MinValue)
          {
            timeSpan = TimeSpan.MinValue;
            includeKind = false;
            break;
          }
          timeSpan = value - BclHelpers.EpochOrigin[0];
          break;
        default:
          timeSpan = value - BclHelpers.EpochOrigin[0];
          break;
      }
      BclHelpers.WriteTimeSpanImpl(timeSpan, dest, includeKind ? value.Kind : DateTimeKind.Unspecified);
    }

    private static long ReadTimeSpanTicks(ProtoReader source, out DateTimeKind kind)
    {
      kind = DateTimeKind.Unspecified;
      switch (source.WireType)
      {
        case WireType.Fixed64:
          return source.ReadInt64();
        case WireType.String:
        case WireType.StartGroup:
          SubItemToken token = ProtoReader.StartSubItem(source);
          TimeSpanScale timeSpanScale = TimeSpanScale.Days;
          long num1 = 0;
          int num2;
          while ((num2 = source.ReadFieldHeader()) > 0)
          {
            switch (num2)
            {
              case 1:
                source.Assert(WireType.SignedVariant);
                num1 = source.ReadInt64();
                continue;
              case 2:
                timeSpanScale = (TimeSpanScale) source.ReadInt32();
                continue;
              case 3:
                kind = (DateTimeKind) source.ReadInt32();
                switch (kind)
                {
                  case DateTimeKind.Unspecified:
                  case DateTimeKind.Utc:
                  case DateTimeKind.Local:
                    continue;
                  default:
                    throw new ProtoException("Invalid date/time kind: " + kind.ToString());
                }
              default:
                source.SkipField();
                continue;
            }
          }
          ProtoReader.EndSubItem(token, source);
          switch (timeSpanScale)
          {
            case TimeSpanScale.Days:
              return num1 * 864000000000L;
            case TimeSpanScale.Hours:
              return num1 * 36000000000L;
            case TimeSpanScale.Minutes:
              return num1 * 600000000L;
            case TimeSpanScale.Seconds:
              return num1 * 10000000L;
            case TimeSpanScale.Milliseconds:
              return num1 * 10000L;
            case TimeSpanScale.Ticks:
              return num1;
            case TimeSpanScale.MinMax:
              if (num1 == -1L)
                return long.MinValue;
              if (num1 == 1L)
                return long.MaxValue;
              throw new ProtoException("Unknown min/max value: " + num1.ToString());
            default:
              throw new ProtoException("Unknown timescale: " + timeSpanScale.ToString());
          }
        default:
          throw new ProtoException("Unexpected wire-type: " + source.WireType.ToString());
      }
    }

    /// <summary>Parses a decimal from a protobuf stream</summary>
    public static Decimal ReadDecimal(ProtoReader reader)
    {
      ulong num1 = 0;
      uint hi = 0;
      uint num2 = 0;
      SubItemToken token = ProtoReader.StartSubItem(reader);
      int num3;
      while ((num3 = reader.ReadFieldHeader()) > 0)
      {
        switch (num3)
        {
          case 1:
            num1 = reader.ReadUInt64();
            continue;
          case 2:
            hi = reader.ReadUInt32();
            continue;
          case 3:
            num2 = reader.ReadUInt32();
            continue;
          default:
            reader.SkipField();
            continue;
        }
      }
      ProtoReader.EndSubItem(token, reader);
      return new Decimal((int) ((long) num1 & (long) uint.MaxValue), (int) ((long) (num1 >> 32) & (long) uint.MaxValue), (int) hi, ((int) num2 & 1) == 1, (byte) ((num2 & 510U) >> 1));
    }

    /// <summary>Writes a decimal to a protobuf stream</summary>
    public static void WriteDecimal(Decimal value, ProtoWriter writer)
    {
      int[] bits = Decimal.GetBits(value);
      ulong num1 = (ulong) bits[1] << 32 | (ulong) bits[0] & (ulong) uint.MaxValue;
      uint num2 = (uint) bits[2];
      uint num3 = (uint) (bits[3] >> 15 & 510 | bits[3] >> 31 & 1);
      SubItemToken token = ProtoWriter.StartSubItem((object) null, writer);
      if (num1 != 0UL)
      {
        ProtoWriter.WriteFieldHeader(1, WireType.Variant, writer);
        ProtoWriter.WriteUInt64(num1, writer);
      }
      if (num2 != 0U)
      {
        ProtoWriter.WriteFieldHeader(2, WireType.Variant, writer);
        ProtoWriter.WriteUInt32(num2, writer);
      }
      if (num3 != 0U)
      {
        ProtoWriter.WriteFieldHeader(3, WireType.Variant, writer);
        ProtoWriter.WriteUInt32(num3, writer);
      }
      ProtoWriter.EndSubItem(token, writer);
    }

    /// <summary>Writes a Guid to a protobuf stream</summary>
    public static void WriteGuid(Guid value, ProtoWriter dest)
    {
      byte[] byteArray = value.ToByteArray();
      SubItemToken token = ProtoWriter.StartSubItem((object) null, dest);
      if (value != Guid.Empty)
      {
        ProtoWriter.WriteFieldHeader(1, WireType.Fixed64, dest);
        ProtoWriter.WriteBytes(byteArray, 0, 8, dest);
        ProtoWriter.WriteFieldHeader(2, WireType.Fixed64, dest);
        ProtoWriter.WriteBytes(byteArray, 8, 8, dest);
      }
      ProtoWriter.EndSubItem(token, dest);
    }

    /// <summary>Parses a Guid from a protobuf stream</summary>
    public static Guid ReadGuid(ProtoReader source)
    {
      ulong num1 = 0;
      ulong num2 = 0;
      SubItemToken token = ProtoReader.StartSubItem(source);
      int num3;
      while ((num3 = source.ReadFieldHeader()) > 0)
      {
        switch (num3)
        {
          case 1:
            num1 = source.ReadUInt64();
            continue;
          case 2:
            num2 = source.ReadUInt64();
            continue;
          default:
            source.SkipField();
            continue;
        }
      }
      ProtoReader.EndSubItem(token, source);
      if (num1 == 0UL && num2 == 0UL)
        return Guid.Empty;
      uint b = (uint) (num1 >> 32);
      uint a = (uint) num1;
      uint h = (uint) (num2 >> 32);
      uint d = (uint) num2;
      return new Guid((int) a, (short) b, (short) (b >> 16), (byte) d, (byte) (d >> 8), (byte) (d >> 16), (byte) (d >> 24), (byte) h, (byte) (h >> 8), (byte) (h >> 16), (byte) (h >> 24));
    }

    /// <summary>
    /// Reads an *implementation specific* bundled .NET object, including (as options) type-metadata, identity/re-use, etc.
    /// </summary>
    public static object ReadNetObject(
      object value,
      ProtoReader source,
      int key,
      Type type,
      BclHelpers.NetObjectOptions options)
    {
      SubItemToken token = ProtoReader.StartSubItem(source);
      int num1 = -1;
      int key1 = -1;
      int num2;
      while ((num2 = source.ReadFieldHeader()) > 0)
      {
        switch (num2)
        {
          case 1:
            int key2 = source.ReadInt32();
            value = source.NetCache.GetKeyedObject(key2);
            continue;
          case 2:
            num1 = source.ReadInt32();
            continue;
          case 3:
            int key3 = source.ReadInt32();
            type = (Type) source.NetCache.GetKeyedObject(key3);
            key = source.GetTypeKey(ref type);
            continue;
          case 4:
            key1 = source.ReadInt32();
            continue;
          case 8:
            string str = source.ReadString();
            type = source.DeserializeType(str);
            if (type == (Type) null)
              throw new ProtoException("Unable to resolve type: " + str + " (you can use the TypeModel.DynamicTypeFormatting event to provide a custom mapping)");
            if (type == typeof (string))
            {
              key = -1;
              continue;
            }
            key = source.GetTypeKey(ref type);
            if (key < 0)
              throw new InvalidOperationException("Dynamic type is not a contract-type: " + type.Name);
            continue;
          case 10:
            bool flag1 = type == typeof (string);
            bool flag2 = value == null;
            bool flag3 = flag2 && (flag1 || (options & BclHelpers.NetObjectOptions.LateSet) != 0);
            if (num1 >= 0 && !flag3)
            {
              if (value == null)
                source.TrapNextObject(num1);
              else
                source.NetCache.SetKeyedObject(num1, value);
              if (key1 >= 0)
                source.NetCache.SetKeyedObject(key1, (object) type);
            }
            object obj = value;
            value = !flag1 ? ProtoReader.ReadTypedObject(obj, key, source, type) : (object) source.ReadString();
            if (num1 >= 0)
            {
              if (flag2 && !flag3)
                obj = source.NetCache.GetKeyedObject(num1);
              if (flag3)
              {
                source.NetCache.SetKeyedObject(num1, value);
                if (key1 >= 0)
                  source.NetCache.SetKeyedObject(key1, (object) type);
              }
            }
            if (num1 >= 0 && !flag3 && obj != value)
              throw new ProtoException("A reference-tracked object changed reference during deserialization");
            if (num1 < 0 && key1 >= 0)
            {
              source.NetCache.SetKeyedObject(key1, (object) type);
              continue;
            }
            continue;
          default:
            source.SkipField();
            continue;
        }
      }
      if (num1 >= 0 && (options & BclHelpers.NetObjectOptions.AsReference) == BclHelpers.NetObjectOptions.None)
        throw new ProtoException("Object key in input stream, but reference-tracking was not expected");
      ProtoReader.EndSubItem(token, source);
      return value;
    }

    /// <summary>
    /// Writes an *implementation specific* bundled .NET object, including (as options) type-metadata, identity/re-use, etc.
    /// </summary>
    public static void WriteNetObject(
      object value,
      ProtoWriter dest,
      int key,
      BclHelpers.NetObjectOptions options)
    {
      if (dest == null)
        throw new ArgumentNullException(nameof (dest));
      bool flag1 = (options & BclHelpers.NetObjectOptions.DynamicType) != 0;
      bool flag2 = (options & BclHelpers.NetObjectOptions.AsReference) != 0;
      WireType wireType = dest.WireType;
      SubItemToken token = ProtoWriter.StartSubItem((object) null, dest);
      bool flag3 = true;
      if (flag2)
      {
        bool existing;
        int num = dest.NetCache.AddObjectKey(value, out existing);
        ProtoWriter.WriteFieldHeader(existing ? 1 : 2, WireType.Variant, dest);
        ProtoWriter.WriteInt32(num, dest);
        if (existing)
          flag3 = false;
      }
      if (flag3)
      {
        if (flag1)
        {
          Type type = value.GetType();
          if (!(value is string))
          {
            key = dest.GetTypeKey(ref type);
            if (key < 0)
              throw new InvalidOperationException("Dynamic type is not a contract-type: " + type.Name);
          }
          bool existing;
          int num = dest.NetCache.AddObjectKey((object) type, out existing);
          ProtoWriter.WriteFieldHeader(existing ? 3 : 4, WireType.Variant, dest);
          ProtoWriter.WriteInt32(num, dest);
          if (!existing)
          {
            ProtoWriter.WriteFieldHeader(8, WireType.String, dest);
            ProtoWriter.WriteString(dest.SerializeType(type), dest);
          }
        }
        ProtoWriter.WriteFieldHeader(10, wireType, dest);
        if (value is string)
          ProtoWriter.WriteString((string) value, dest);
        else
          ProtoWriter.WriteObject(value, key, dest);
      }
      ProtoWriter.EndSubItem(token, dest);
    }

    /// <summary>
    /// Optional behaviours that introduce .NET-specific functionality
    /// </summary>
    [Flags]
    public enum NetObjectOptions : byte
    {
      /// <summary>No special behaviour</summary>
      None = 0,
      /// <summary>Enables full object-tracking/full-graph support.</summary>
      AsReference = 1,
      /// <summary>
      /// Embeds the type information into the stream, allowing usage with types not known in advance.
      /// </summary>
      DynamicType = 2,
      /// <summary>
      /// If false, the constructor for the type is bypassed during deserialization, meaning any field initializers
      /// or other initialization code is skipped.
      /// </summary>
      UseConstructor = 4,
      /// <summary>
      /// Should the object index be reserved, rather than creating an object promptly
      /// </summary>
      LateSet = 8,
    }
  }
}
