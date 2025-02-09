// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.ErrorCodeExtensions
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

using Confluent.Kafka.Impl;
using Confluent.Kafka.Internal;

#nullable disable
namespace Confluent.Kafka
{
  /// <summary>
  ///     Provides extension methods on the ErrorCode enumeration.
  /// </summary>
  public static class ErrorCodeExtensions
  {
    /// <summary>
    ///     Returns the static error string associated with
    ///     the particular ErrorCode value.
    /// </summary>
    public static string GetReason(this ErrorCode code)
    {
      Librdkafka.Initialize((string) null);
      return Util.Marshal.PtrToStringUTF8(Librdkafka.err2str(code));
    }
  }
}
