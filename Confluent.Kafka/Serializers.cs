// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.Serializers
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
  ///     Serializers for use with <see cref="T:Confluent.Kafka.Producer`2" />.
  /// </summary>
  public static class Serializers
  {
    /// <summary>String (UTF8) serializer.</summary>
    public static ISerializer<string> Utf8 = (ISerializer<string>) new Serializers.Utf8Serializer();
    /// <summary>Null serializer.</summary>
    public static ISerializer<Confluent.Kafka.Null> Null = (ISerializer<Confluent.Kafka.Null>) new Serializers.NullSerializer();
    /// <summary>
    ///     System.Int64 (big endian, network byte order) serializer.
    /// </summary>
    public static ISerializer<long> Int64 = (ISerializer<long>) new Serializers.Int64Serializer();
    /// <summary>
    ///     System.Int32 (big endian, network byte order) serializer.
    /// </summary>
    public static ISerializer<int> Int32 = (ISerializer<int>) new Serializers.Int32Serializer();
    /// <summary>
    ///     System.Single (big endian, network byte order) serializer
    /// </summary>
    public static ISerializer<float> Single = (ISerializer<float>) new Serializers.SingleSerializer();
    /// <summary>
    ///     System.Double (big endian, network byte order) serializer
    /// </summary>
    public static ISerializer<double> Double = (ISerializer<double>) new Serializers.DoubleSerializer();
    /// <summary>System.Byte[] (nullable) serializer.</summary>
    /// <remarks>Byte order is original order.</remarks>
    public static ISerializer<byte[]> ByteArray = (ISerializer<byte[]>) new Serializers.ByteArraySerializer();

    private class Utf8Serializer : ISerializer<string>
    {
      public byte[] Serialize(string data, SerializationContext context)
      {
        return data == null ? (byte[]) null : Encoding.UTF8.GetBytes(data);
      }
    }

    private class NullSerializer : ISerializer<Confluent.Kafka.Null>
    {
      public byte[] Serialize(Confluent.Kafka.Null data, SerializationContext context)
      {
        return (byte[]) null;
      }
    }

    private class Int64Serializer : ISerializer<long>
    {
      public byte[] Serialize(long data, SerializationContext context)
      {
        return new byte[8]
        {
          (byte) (data >> 56),
          (byte) (data >> 48),
          (byte) (data >> 40),
          (byte) (data >> 32),
          (byte) (data >> 24),
          (byte) (data >> 16),
          (byte) (data >> 8),
          (byte) data
        };
      }
    }

    private class Int32Serializer : ISerializer<int>
    {
      public byte[] Serialize(int data, SerializationContext context)
      {
        return new byte[4]
        {
          (byte) (data >> 24),
          (byte) (data >> 16),
          (byte) (data >> 8),
          (byte) data
        };
      }
    }

    private class SingleSerializer : ISerializer<float>
    {
      public unsafe byte[] Serialize(float data, SerializationContext context)
      {
        if (!BitConverter.IsLittleEndian)
          return BitConverter.GetBytes(data);
        byte[] numArray = new byte[4];
        byte* numPtr1 = (byte*) &data;
        byte* numPtr2 = numPtr1 + 1;
        numArray[3] = *numPtr1;
        byte* numPtr3 = numPtr2;
        byte* numPtr4 = numPtr3 + 1;
        numArray[2] = *numPtr3;
        byte* numPtr5 = numPtr4;
        byte* numPtr6 = numPtr5 + 1;
        numArray[1] = *numPtr5;
        byte* numPtr7 = numPtr6;
        byte* numPtr8 = numPtr7 + 1;
        numArray[0] = *numPtr7;
        return numArray;
      }
    }

    private class DoubleSerializer : ISerializer<double>
    {
      public unsafe byte[] Serialize(double data, SerializationContext context)
      {
        if (!BitConverter.IsLittleEndian)
          return BitConverter.GetBytes(data);
        byte[] numArray = new byte[8];
        byte* numPtr1 = (byte*) &data;
        byte* numPtr2 = numPtr1 + 1;
        numArray[7] = *numPtr1;
        byte* numPtr3 = numPtr2;
        byte* numPtr4 = numPtr3 + 1;
        numArray[6] = *numPtr3;
        byte* numPtr5 = numPtr4;
        byte* numPtr6 = numPtr5 + 1;
        numArray[5] = *numPtr5;
        byte* numPtr7 = numPtr6;
        byte* numPtr8 = numPtr7 + 1;
        numArray[4] = *numPtr7;
        byte* numPtr9 = numPtr8;
        byte* numPtr10 = numPtr9 + 1;
        numArray[3] = *numPtr9;
        byte* numPtr11 = numPtr10;
        byte* numPtr12 = numPtr11 + 1;
        numArray[2] = *numPtr11;
        byte* numPtr13 = numPtr12;
        byte* numPtr14 = numPtr13 + 1;
        numArray[1] = *numPtr13;
        byte* numPtr15 = numPtr14;
        byte* numPtr16 = numPtr15 + 1;
        numArray[0] = *numPtr15;
        return numArray;
      }
    }

    private class ByteArraySerializer : ISerializer<byte[]>
    {
      public byte[] Serialize(byte[] data, SerializationContext context) => data;
    }
  }
}
