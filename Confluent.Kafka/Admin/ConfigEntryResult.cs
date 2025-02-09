// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.Admin.ConfigEntryResult
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

using System.Collections.Generic;

#nullable disable
namespace Confluent.Kafka.Admin
{
  /// <summary>
  ///     A config property entry, as reported by the Kafka admin api.
  /// </summary>
  public class ConfigEntryResult
  {
    /// <summary>
    ///     Whether or not the config value is the default or was
    ///     explicitly set.
    /// </summary>
    public bool IsDefault { get; set; }

    /// <summary>
    ///     Whether or not the config is read-only (cannot be updated).
    /// </summary>
    public bool IsReadOnly { get; set; }

    /// <summary>
    ///     Whether or not the config value is sensitive. The value
    ///     for sensitive configuration values is always returned
    ///     as null.
    /// </summary>
    public bool IsSensitive { get; set; }

    /// <summary>The config name.</summary>
    public string Name { get; set; }

    /// <summary>The config value.</summary>
    public string Value { get; set; }

    /// <summary>
    ///     The config source. Refer to
    ///     <see cref="T:Confluent.Kafka.Admin.ConfigSource" /> for
    ///     more information.
    /// </summary>
    public ConfigSource Source { get; set; }

    /// <summary>
    ///     All config values that may be used as the value of this
    ///     config along with their source, in the order of precedence.
    /// </summary>
    public List<ConfigSynonym> Synonyms { get; set; }
  }
}
