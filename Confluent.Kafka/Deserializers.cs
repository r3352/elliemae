// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.Deserializers
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

using System;
using System.Text;

#nullable disable
namespace Confluent.Kafka
{
  /// <summary>
  ///     Deserializers for use with <see cref="T:Confluent.Kafka.Consumer`2" />.
  /// </summary>
  public static class Deserializers
  {
    /// <summary>String (UTF8 encoded) deserializer.</summary>
    public static IDeserializer<string> Utf8 = (IDeserializer<string>) new Deserializers.Utf8Deserializer();
    /// <summary>Null value deserializer.</summary>
    public static IDeserializer<Confluent.Kafka.Null> Null = (IDeserializer<Confluent.Kafka.Null>) new Deserializers.NullDeserializer();
    /// <summary>Deserializer that deserializes any value to null.</summary>
    public static IDeserializer<Confluent.Kafka.Ignore> Ignore = (IDeserializer<Confluent.Kafka.Ignore>) new Deserializers.IgnoreDeserializer();
    /// <summary>
    ///     System.Int64 (big endian encoded, network byte ordered) deserializer.
    /// </summary>
    public static IDeserializer<long> Int64 = (IDeserializer<long>) new Deserializers.Int64Deserializer();
    /// <summary>
    ///     System.Int32 (big endian encoded, network byte ordered) deserializer.
    /// </summary>
    public static IDeserializer<int> Int32 = (IDeserializer<int>) new Deserializers.Int32Deserializer();
    /// <summary>
    ///     System.Single (big endian encoded, network byte ordered) deserializer.
    /// </summary>
    public static IDeserializer<float> Single = (IDeserializer<float>) new Deserializers.SingleDeserializer();
    /// <summary>
    ///     System.Double (big endian encoded, network byte ordered) deserializer.
    /// </summary>
    public static IDeserializer<double> Double = (IDeserializer<double>) new Deserializers.DoubleDeserializer();
    /// <summary>System.Byte[] (nullable) deserializer.</summary>
    /// <remarks>Byte ordering is original order.</remarks>
    public static IDeserializer<byte[]> ByteArray = (IDeserializer<byte[]>) new Deserializers.ByteArrayDeserializer();

    private class Utf8Deserializer : IDeserializer<string>
    {
      public string Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
      {
        return isNull ? (string) null : Encoding.UTF8.GetString(data.ToArray());
      }
    }

    private class NullDeserializer : IDeserializer<Confluent.Kafka.Null>
    {
      public Confluent.Kafka.Null Deserialize(
        ReadOnlySpan<byte> data,
        bool isNull,
        SerializationContext context)
      {
        if (!isNull)
          throw new ArgumentException("Deserializer<Null> may only be used to deserialize data that is null.");
        return (Confluent.Kafka.Null) null;
      }
    }

    private class IgnoreDeserializer : IDeserializer<Confluent.Kafka.Ignore>
    {
      public Confluent.Kafka.Ignore Deserialize(
        ReadOnlySpan<byte> data,
        bool isNull,
        SerializationContext context)
      {
        return (Confluent.Kafka.Ignore) null;
      }
    }

    private class Int64Deserializer : IDeserializer<long>
    {
      public long Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
      {
        if (isNull)
          throw new ArgumentNullException("Null data encountered deserializing Int64 value.");
        if (data.Length != 8)
          throw new ArgumentException(string.Format("Deserializer<Long> encountered data of length {0}. Expecting data length to be 8.", (object) data.Length));
        return (long) data[0] << 56 | (long) data[1] << 48 | (long) data[2] << 40 | (long) data[3] << 32 | (long) data[4] << 24 | (long) data[5] << 16 | (long) data[6] << 8 | (long) data[7];
      }
    }

    private class Int32Deserializer : IDeserializer<int>
    {
      public int Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
      {
        if (isNull)
          throw new ArgumentNullException("Null data encountered deserializing Int32 value");
        if (data.Length != 4)
          throw new ArgumentException(string.Format("Deserializer<Int32> encountered data of length {0}. Expecting data length to be 4.", (object) data.Length));
        return (int) data[0] << 24 | (int) data[1] << 16 | (int) data[2] << 8 | (int) data[3];
      }
    }

    private class SingleDeserializer : IDeserializer<float>
    {
      public unsafe float Deserialize(
        ReadOnlySpan<byte> data,
        bool isNull,
        SerializationContext context)
      {
        if (isNull)
          throw new ArgumentNullException("Null data encountered deserializing float value.");
        if (data.Length != 4)
          throw new ArgumentException(string.Format("Deserializer<float> encountered data of length {0}. Expecting data length to be 4.", (object) data.Length));
        if (!BitConverter.IsLittleEndian)
          return BitConverter.ToSingle(data.ToArray(), 0);
        float num1 = 0.0f;
        byte* numPtr1 = (byte*) &num1;
        byte* numPtr2 = numPtr1 + 1;
        int num2 = (int) data[3];
        *numPtr1 = (byte) num2;
        byte* numPtr3 = numPtr2;
        byte* numPtr4 = numPtr3 + 1;
        int num3 = (int) data[2];
        *numPtr3 = (byte) num3;
        byte* numPtr5 = numPtr4;
        byte* numPtr6 = numPtr5 + 1;
        int num4 = (int) data[1];
        *numPtr5 = (byte) num4;
        byte* numPtr7 = numPtr6;
        byte* numPtr8 = numPtr7 + 1;
        int num5 = (int) data[0];
        *numPtr7 = (byte) num5;
        return num1;
      }
    }

    private class DoubleDeserializer : IDeserializer<double>
    {
      public unsafe double Deserialize(
        ReadOnlySpan<byte> data,
        bool isNull,
        SerializationContext context)
      {
        if (isNull)
          throw new ArgumentNullException("Null data encountered deserializing double value.");
        if (data.Length != 8)
          throw new ArgumentException(string.Format("Deserializer<double> encountered data of length {0}. Expecting data length to be 8.", (object) data.Length));
        if (!BitConverter.IsLittleEndian)
          return BitConverter.ToDouble(data.ToArray(), 0);
        double num1 = 0.0;
        byte* numPtr1 = (byte*) &num1;
        byte* numPtr2 = numPtr1 + 1;
        int num2 = (int) data[7];
        *numPtr1 = (byte) num2;
        byte* numPtr3 = numPtr2;
        byte* numPtr4 = numPtr3 + 1;
        int num3 = (int) data[6];
        *numPtr3 = (byte) num3;
        byte* numPtr5 = numPtr4;
        byte* numPtr6 = numPtr5 + 1;
        int num4 = (int) data[5];
        *numPtr5 = (byte) num4;
        byte* numPtr7 = numPtr6;
        byte* numPtr8 = numPtr7 + 1;
        int num5 = (int) data[4];
        *numPtr7 = (byte) num5;
        byte* numPtr9 = numPtr8;
        byte* numPtr10 = numPtr9 + 1;
        int num6 = (int) data[3];
        *numPtr9 = (byte) num6;
        byte* numPtr11 = numPtr10;
        byte* numPtr12 = numPtr11 + 1;
        int num7 = (int) data[2];
        *numPtr11 = (byte) num7;
        byte* numPtr13 = numPtr12;
        byte* numPtr14 = numPtr13 + 1;
        int num8 = (int) data[1];
        *numPtr13 = (byte) num8;
        byte* numPtr15 = numPtr14;
        byte* numPtr16 = numPtr15 + 1;
        int num9 = (int) data[0];
        *numPtr15 = (byte) num9;
        return num1;
      }
    }

    private class ByteArrayDeserializer : IDeserializer<byte[]>
    {
      public byte[] Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
      {
        return isNull ? (byte[]) null : data.ToArray();
      }
    }
  }
}
