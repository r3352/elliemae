// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.Config
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Confluent.Kafka
{
  /// <summary>
  ///     Base functionality common to all configuration classes.
  /// </summary>
  public class Config : IEnumerable<KeyValuePair<string, string>>, IEnumerable
  {
    private static Dictionary<string, string> EnumNameToConfigValueSubstitutes = new Dictionary<string, string>()
    {
      {
        "saslplaintext",
        "sasl_plaintext"
      },
      {
        "saslssl",
        "sasl_ssl"
      },
      {
        "consistentrandom",
        "consistent_random"
      },
      {
        "murmur2random",
        "murmur2_random"
      },
      {
        "readcommitted",
        "read_committed"
      },
      {
        "readuncommitted",
        "read_uncommitted"
      }
    };
    /// <summary>The configuration properties.</summary>
    protected IDictionary<string, string> properties;
    private const int DefaultCancellationDelayMaxMs = 100;

    /// <summary>
    ///     Initialize a new empty <see cref="T:Confluent.Kafka.Config" /> instance.
    /// </summary>
    public Config()
    {
      this.properties = (IDictionary<string, string>) new Dictionary<string, string>();
    }

    /// <summary>
    ///     Initialize a new <see cref="T:Confluent.Kafka.Config" /> instance based on
    ///     an existing <see cref="T:Confluent.Kafka.Config" /> instance.
    ///     This will change the values "in-place" i.e. operations on this class WILL modify the provided collection
    /// </summary>
    public Config(Config config) => this.properties = config.properties;

    /// <summary>
    ///     Initialize a new <see cref="T:Confluent.Kafka.Config" /> wrapping
    ///     an existing key/value dictionary.
    ///     This will change the values "in-place" i.e. operations on this class WILL modify the provided collection
    /// </summary>
    public Config(IDictionary<string, string> config) => this.properties = config;

    /// <summary>
    ///     Set a configuration property using a string key / value pair.
    /// </summary>
    /// <remarks>
    ///     Two scenarios where this is useful: 1. For setting librdkafka
    ///     plugin config properties. 2. You are using a different version of
    ///     librdkafka to the one provided as a dependency of the Confluent.Kafka
    ///     package and the configuration properties have evolved.
    /// </remarks>
    /// <param name="key">The configuration property name.</param>
    /// <param name="val">The property value.</param>
    public void Set(string key, string val) => this.properties[key] = val;

    /// <summary>
    ///     Gets a configuration property value given a key. Returns null if
    ///     the property has not been set.
    /// </summary>
    /// <param name="key">The configuration property to get.</param>
    /// <returns>The configuration property value.</returns>
    public string Get(string key)
    {
      string str;
      return this.properties.TryGetValue(key, out str) ? str : (string) null;
    }

    /// <summary>
    ///     Gets a configuration property int? value given a key.
    /// </summary>
    /// <param name="key">The configuration property to get.</param>
    /// <returns>The configuration property value.</returns>
    protected int? GetInt(string key)
    {
      string s = this.Get(key);
      return s == null ? new int?() : new int?(int.Parse(s));
    }

    /// <summary>
    ///     Gets a configuration property bool? value given a key.
    /// </summary>
    /// <param name="key">The configuration property to get.</param>
    /// <returns>The configuration property value.</returns>
    protected bool? GetBool(string key)
    {
      string str = this.Get(key);
      return str == null ? new bool?() : new bool?(bool.Parse(str));
    }

    /// <summary>
    ///     Gets a configuration property double? value given a key.
    /// </summary>
    /// <param name="key">The configuration property to get.</param>
    /// <returns>The configuration property value.</returns>
    protected double? GetDouble(string key)
    {
      string s = this.Get(key);
      return s == null ? new double?() : new double?(double.Parse(s));
    }

    /// <summary>
    ///     Gets a configuration property enum value given a key.
    /// </summary>
    /// <param name="key">The configuration property to get.</param>
    /// <param name="type">
    ///     The enum type of the configuration property.
    /// </param>
    /// <returns>The configuration property value.</returns>
    protected object GetEnum(Type type, string key)
    {
      string result = this.Get(key);
      if (result == null)
        return (object) null;
      return Config.EnumNameToConfigValueSubstitutes.Values.Count<string>((Func<string, bool>) (v => v == result)) > 0 ? Enum.Parse(type, Config.EnumNameToConfigValueSubstitutes.First<KeyValuePair<string, string>>((Func<KeyValuePair<string, string>, bool>) (v => v.Value == result)).Key, true) : Enum.Parse(type, result, true);
    }

    /// <summary>
    ///     Set a configuration property using a key / value pair (null checked).
    /// </summary>
    protected void SetObject(string name, object val)
    {
      if (val == null)
        this.properties.Remove(name);
      else if (val is Enum)
      {
        string lowerInvariant = val.ToString().ToLowerInvariant();
        string str;
        if (Config.EnumNameToConfigValueSubstitutes.TryGetValue(lowerInvariant, out str))
          this.properties[name] = str;
        else
          this.properties[name] = lowerInvariant;
      }
      else
        this.properties[name] = val.ToString();
    }

    /// <summary>
    ///     	Returns an enumerator that iterates through the property collection.
    /// </summary>
    /// <returns>
    ///         An enumerator that iterates through the property collection.
    /// </returns>
    public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
    {
      return this.properties.GetEnumerator();
    }

    /// <summary>
    ///     	Returns an enumerator that iterates through the property collection.
    /// </summary>
    /// <returns>
    ///         An enumerator that iterates through the property collection.
    /// </returns>
    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.properties.GetEnumerator();

    /// <summary>
    ///     The maximum length of time (in milliseconds) before a cancellation request
    ///     is acted on. Low values may result in measurably higher CPU usage.
    /// 
    ///     default: 100
    ///     range: 1 &lt;= dotnet.cancellation.delay.max.ms &lt;= 10000
    ///     importance: low
    /// </summary>
    public int CancellationDelayMaxMs
    {
      set => this.SetObject("dotnet.cancellation.delay.max.ms", (object) value);
    }

    internal static IEnumerable<KeyValuePair<string, string>> ExtractCancellationDelayMaxMs(
      IEnumerable<KeyValuePair<string, string>> config,
      out int cancellationDelayMaxMs)
    {
      string s = config.Where<KeyValuePair<string, string>>((Func<KeyValuePair<string, string>, bool>) (prop => prop.Key == "dotnet.cancellation.delay.max.ms")).Select<KeyValuePair<string, string>, string>((Func<KeyValuePair<string, string>, string>) (a => a.Value)).FirstOrDefault<string>();
      if (s != null)
      {
        if (!int.TryParse(s, out cancellationDelayMaxMs))
          throw new ArgumentException("dotnet.cancellation.delay.max.ms must be a valid integer value.");
        if (cancellationDelayMaxMs < 1 || cancellationDelayMaxMs > 10000)
          throw new ArgumentOutOfRangeException("dotnet.cancellation.delay.max.ms must be in the range 1 <= dotnet.cancellation.delay.max.ms <= 10000");
      }
      else
        cancellationDelayMaxMs = 100;
      return config.Where<KeyValuePair<string, string>>((Func<KeyValuePair<string, string>, bool>) (prop => prop.Key != "dotnet.cancellation.delay.max.ms"));
    }
  }
}
