// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.Impl.SafeTopicConfigHandle
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

using Confluent.Kafka.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#nullable disable
namespace Confluent.Kafka.Impl
{
  internal sealed class SafeTopicConfigHandle : SafeHandleZeroIsInvalid
  {
    public SafeTopicConfigHandle()
      : base("kafka topic config", false)
    {
    }

    internal static SafeTopicConfigHandle Create()
    {
      SafeTopicConfigHandle topicConfigHandle = Librdkafka.topic_conf_new();
      return !topicConfigHandle.IsInvalid ? topicConfigHandle : throw new Exception("Failed to create TopicConfig");
    }

    internal IntPtr Dup() => Librdkafka.topic_conf_dup(this.handle);

    internal Dictionary<string, string> Dump()
    {
      UIntPtr cntp = (UIntPtr) 0UL;
      IntPtr num = Librdkafka.topic_conf_dump(this.handle, out cntp);
      if (num == IntPtr.Zero)
        throw new Exception("Zero data");
      try
      {
        if (((int) (uint) cntp & 1) != 0)
          throw new Exception("Invalid number of config entries");
        Dictionary<string, string> source = new Dictionary<string, string>();
        for (int index = 0; index < (int) (uint) cntp / 2; ++index)
          source.Add(Util.Marshal.PtrToStringUTF8(System.Runtime.InteropServices.Marshal.ReadIntPtr(num, 2 * index * Util.Marshal.SizeOf<IntPtr>())), Util.Marshal.PtrToStringUTF8(System.Runtime.InteropServices.Marshal.ReadIntPtr(num, (2 * index + 1) * Util.Marshal.SizeOf<IntPtr>())));
        return source.Where<KeyValuePair<string, string>>((Func<KeyValuePair<string, string>, bool>) (kv => !kv.Key.EndsWith("_cb"))).ToDictionary<KeyValuePair<string, string>, string, string>((Func<KeyValuePair<string, string>, string>) (kv => kv.Key), (Func<KeyValuePair<string, string>, string>) (kv => kv.Value));
      }
      finally
      {
        Librdkafka.conf_dump_free(num, cntp);
      }
    }

    internal void Set(string name, string value)
    {
      StringBuilder errstr = new StringBuilder(512);
      switch (Librdkafka.topic_conf_set(this.handle, name, value, errstr, (UIntPtr) (ulong) errstr.Capacity))
      {
        case ConfRes.Unknown:
          throw new InvalidOperationException(errstr.ToString());
        case ConfRes.Invalid:
          throw new InvalidOperationException(errstr.ToString());
        case ConfRes.Ok:
          break;
        default:
          throw new Exception("Unknown error while setting configuration property");
      }
    }

    internal string Get(string name)
    {
      UIntPtr zero = UIntPtr.Zero;
      StringBuilder dest = (StringBuilder) null;
      ConfRes confRes = Librdkafka.topic_conf_get(this.handle, name, (StringBuilder) null, ref zero);
      if (confRes == ConfRes.Ok)
      {
        dest = new StringBuilder((int) (uint) zero);
        confRes = Librdkafka.topic_conf_get(this.handle, name, dest, ref zero);
      }
      if (confRes != ConfRes.Ok)
      {
        if (confRes == ConfRes.Unknown)
          throw new InvalidOperationException("No such configuration property: " + name);
        throw new Exception("Unknown error while getting configuration property");
      }
      return dest?.ToString();
    }

    protected override bool ReleaseHandle() => false;
  }
}
