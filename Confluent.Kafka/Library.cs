// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.Library
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

using Confluent.Kafka.Impl;
using Confluent.Kafka.Internal;
using System.Collections.Generic;
using System.Threading;

#nullable disable
namespace Confluent.Kafka
{
  /// <summary>
  ///     Methods that relate to the native librdkafka library itself
  ///     (do not require a Producer or Consumer broker connection).
  /// </summary>
  public static class Library
  {
    private static int kafkaHandleCreateCount;
    private static int kafkaHandleDestroyCount;

    /// <summary>
    ///     Gets the librdkafka version as an integer.
    /// 
    ///     Interpreted as hex MM.mm.rr.xx:
    ///         - MM = Major
    ///         - mm = minor
    ///         - rr = revision
    ///         - xx = pre-release id (0xff is the final release)
    /// 
    ///     E.g.: 0x000901ff = 0.9.1
    /// </summary>
    public static int Version
    {
      get
      {
        Librdkafka.Initialize((string) null);
        return (int) Librdkafka.version();
      }
    }

    /// <summary>Gets the librdkafka version as string.</summary>
    public static string VersionString
    {
      get
      {
        Librdkafka.Initialize((string) null);
        return Util.Marshal.PtrToStringUTF8(Librdkafka.version_str());
      }
    }

    /// <summary>Gets a list of the supported debug contexts.</summary>
    public static string[] DebugContexts
    {
      get
      {
        Librdkafka.Initialize((string) null);
        return Util.Marshal.PtrToStringUTF8(Librdkafka.get_debug_contexts()).Split(',');
      }
    }

    /// <summary>
    ///     true if librdkafka has been successfully loaded, false if not.
    /// </summary>
    public static bool IsLoaded => Librdkafka.IsInitialized;

    /// <summary>
    ///     Loads the native librdkafka library. Does nothing if the library is
    ///     already loaded.
    /// </summary>
    /// <returns>
    ///     true if librdkafka was loaded as a result of this call, false if the
    ///     library has already been loaded.
    /// </returns>
    /// <remarks>
    ///     You will not typically need to call this method - librdkafka is loaded
    ///     automatically on first use of a Producer or Consumer instance.
    /// </remarks>
    public static bool Load() => Library.Load((string) null);

    /// <summary>
    ///     Loads the native librdkafka library from the specified path (note: the
    ///     specified path needs to include the filename). Does nothing if the
    ///     library is already loaded.
    /// </summary>
    /// <returns>
    ///     true if librdkafka was loaded as a result of this call, false if the
    ///     library has already been loaded.
    /// </returns>
    /// <remarks>
    ///     You will not typically need to call this method - librdkafka is loaded
    ///     automatically on first use of a Producer or Consumer instance.
    /// </remarks>
    public static bool Load(string path) => Librdkafka.Initialize(path);

    internal static void IncrementKafkaHandleCreateCount()
    {
      Interlocked.Increment(ref Library.kafkaHandleCreateCount);
    }

    internal static void IncrementKafkaHandleDestroyCount()
    {
      Interlocked.Increment(ref Library.kafkaHandleDestroyCount);
    }

    /// <summary>
    ///     The total number librdkafka client instances that have been
    ///     created and not yet disposed.
    /// </summary>
    public static int HandleCount
    {
      get => Library.kafkaHandleCreateCount - Library.kafkaHandleDestroyCount;
    }

    internal static List<KeyValuePair<string, string>> NameAndVersionConfig
    {
      get
      {
        return new List<KeyValuePair<string, string>>()
        {
          new KeyValuePair<string, string>("client.software.name", "confluent-kafka-dotnet"),
          new KeyValuePair<string, string>("client.software.version", Library.VersionString)
        };
      }
    }
  }
}
