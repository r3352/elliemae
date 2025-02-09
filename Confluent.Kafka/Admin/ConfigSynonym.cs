// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.Admin.ConfigSynonym
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

#nullable disable
namespace Confluent.Kafka.Admin
{
  /// <summary>Describes a synonym of a config entry.</summary>
  public class ConfigSynonym
  {
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
  }
}
