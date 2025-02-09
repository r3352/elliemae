// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.LogMessage
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

using System;

#nullable disable
namespace Confluent.Kafka
{
  /// <summary>
  ///     Encapsulates information provided to the
  ///     Producer/Consumer OnLog event.
  /// </summary>
  public class LogMessage
  {
    private static int[] SystemDiagnosticsLevelLookup = new int[8]
    {
      1,
      1,
      1,
      1,
      2,
      3,
      3,
      4
    };
    private static int[] MicrosoftExtensionsLoggingLevelLookup = new int[8]
    {
      5,
      5,
      5,
      4,
      3,
      2,
      2,
      1
    };

    /// <summary>Instantiates a new LogMessage class instance.</summary>
    /// <param name="name">The librdkafka client instance name.</param>
    /// <param name="level">
    ///     The log level (levels correspond to syslog(3)), lower is worse.
    /// </param>
    /// <param name="facility">
    ///     The facility (section of librdkafka code) that produced the message.
    /// </param>
    /// <param name="message">The log message.</param>
    public LogMessage(string name, SyslogLevel level, string facility, string message)
    {
      this.Name = name;
      this.Level = level;
      this.Facility = facility;
      this.Message = message;
    }

    /// <summary>Gets the librdkafka client instance name.</summary>
    public string Name { get; }

    /// <summary>
    ///     Gets the log level (levels correspond to syslog(3)), lower is worse.
    /// </summary>
    public SyslogLevel Level { get; }

    /// <summary>
    ///     Gets the facility (section of librdkafka code) that produced the message.
    /// </summary>
    public string Facility { get; }

    /// <summary>Gets the log message.</summary>
    public string Message { get; }

    /// <summary>
    ///     Convert the syslog message severity
    ///     level to correspond to the values of
    ///     a different log level enumeration type.
    /// </summary>
    public int LevelAs(LogLevelType type)
    {
      switch (type)
      {
        case LogLevelType.SysLogLevel:
          return (int) this.Level;
        case LogLevelType.MicrosoftExtensionsLogging:
          return LogMessage.MicrosoftExtensionsLoggingLevelLookup[(int) this.Level];
        case LogLevelType.SystemDiagnostics:
          return LogMessage.SystemDiagnosticsLevelLookup[(int) this.Level];
        default:
          throw new ArgumentException(string.Format("Unexpected log level type: {0}", (object) type));
      }
    }
  }
}
