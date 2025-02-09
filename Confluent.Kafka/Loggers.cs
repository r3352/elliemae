// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.Loggers
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

using System;

#nullable disable
namespace Confluent.Kafka
{
  /// <summary>OnLog callback event handler implementations.</summary>
  /// <remarks>
  ///     Warning: Log handlers are called spontaneously from internal librdkafka
  ///     threads and the application must not call any Confluent.Kafka APIs from
  ///     within a log handler or perform any prolonged operations.
  /// </remarks>
  public static class Loggers
  {
    /// <summary>The method used to log messages by default.</summary>
    public static void ConsoleLogger(LogMessage logInfo)
    {
      string str = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
      Console.Error.WriteLine(string.Format("{0}|{1}|{2}|{3}| {4}", (object) logInfo.Level, (object) str, (object) logInfo.Name, (object) logInfo.Facility, (object) logInfo.Message));
    }
  }
}
