// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.SyslogLevel
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

#nullable disable
namespace Confluent.Kafka
{
  /// <summary>
  ///     Represents enumeration with levels coming from syslog(3)
  /// </summary>
  public enum SyslogLevel
  {
    /// <summary>System is unusable.</summary>
    Emergency,
    /// <summary>Action must be take immediately</summary>
    Alert,
    /// <summary>Critical condition.</summary>
    Critical,
    /// <summary>Error condition.</summary>
    Error,
    /// <summary>Warning condition.</summary>
    Warning,
    /// <summary>Normal, but significant condition.</summary>
    Notice,
    /// <summary>Informational message.</summary>
    Info,
    /// <summary>Debug-level message.</summary>
    Debug,
  }
}
