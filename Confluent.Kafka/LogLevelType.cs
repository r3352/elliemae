// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.LogLevelType
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

#nullable disable
namespace Confluent.Kafka
{
  /// <summary>Enumerates different log level enumerations.</summary>
  public enum LogLevelType
  {
    /// <summary>
    ///     Confluent.Kafka.SysLogLevel (severity
    ///     levels correspond to syslog)
    /// </summary>
    SysLogLevel = 1,
    /// <summary>Microsoft.Extensions.Logging.LogLevel</summary>
    MicrosoftExtensionsLogging = 2,
    /// <summary>System.Diagnostics.TraceLevel</summary>
    SystemDiagnostics = 3,
  }
}
