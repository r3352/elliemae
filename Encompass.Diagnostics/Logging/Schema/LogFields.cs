// Decompiled with JetBrains decompiler
// Type: Encompass.Diagnostics.Logging.Schema.LogFields
// Assembly: Encompass.Diagnostics, Version=1.0.0.1, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E8A3B074-7BF0-4187-B0D2-083265232A16
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Encompass.Diagnostics.dll

using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.Serialization;

#nullable disable
namespace Encompass.Diagnostics.Logging.Schema
{
  [Serializable]
  public class LogFields : ISerializable
  {
    private static readonly ConcurrentDictionary<string, ILogField> KeysRegistry = new ConcurrentDictionary<string, ILogField>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
    private static readonly CamelCaseNamingStrategy NamingStrategy;
    private readonly Dictionary<ILogField, object> _attrs = new Dictionary<ILogField, object>();

    static LogFields()
    {
      CamelCaseNamingStrategy caseNamingStrategy = new CamelCaseNamingStrategy();
      caseNamingStrategy.ProcessDictionaryKeys = true;
      LogFields.NamingStrategy = caseNamingStrategy;
      Log.CommonFields.Init();
    }

    public LogFields()
    {
    }

    protected LogFields(LogFields log)
      : this()
    {
      log.MapTo(this);
    }

    protected LogFields(SerializationInfo info, StreamingContext context)
    {
      this._attrs = (Dictionary<ILogField, object>) info.GetValue(nameof (_attrs), typeof (Dictionary<ILogField, object>));
    }

    public IEnumerable<ILogField> GetKeys() => (IEnumerable<ILogField>) this._attrs.Keys;

    public T Get<T>(LogFieldName<T> key)
    {
      return this.GetInternal((ILogField) key) is T obj ? obj : default (T);
    }

    internal object GetInternal(ILogField key)
    {
      object obj;
      return this._attrs.TryGetValue(key, out obj) ? obj : (object) null;
    }

    public bool TryGet<T>(LogFieldName<T> key, out T tvalue)
    {
      object obj;
      if (this._attrs.TryGetValue((ILogField) key, out obj) && obj is T)
      {
        tvalue = (T) obj;
        return true;
      }
      tvalue = default (T);
      return false;
    }

    public LogFields Set<T>(LogFieldName<T> key, T value)
    {
      return this.SetInternal((ILogField) key, (object) value);
    }

    internal LogFields SetInternal(ILogField key, object value)
    {
      if (value == null)
        this._attrs.Remove(key);
      else
        this._attrs[key] = value;
      return this;
    }

    public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      info.AddValue("_attrs", (object) this._attrs);
    }

    public void MapTo(LogFields log)
    {
      foreach (KeyValuePair<ILogField, object> attr in this._attrs)
        log._attrs[attr.Key] = attr.Value;
    }

    public static LogFieldName<T> Field<T>(string name)
    {
      if (LogFields.KeysRegistry.GetOrAdd(name, (Func<string, ILogField>) (n => (ILogField) new LogFieldName<T>(LogFields.NamingStrategy.GetDictionaryKey(name)))) is LogFieldName<T> orAdd)
        return orAdd;
      throw new Exception("Key already registered for name " + name + " for different type.");
    }

    internal static ILogField GetForDeserialize(string name)
    {
      return LogFields.KeysRegistry.GetOrAdd(name, (Func<string, ILogField>) (n => (ILogField) new LogFieldName<JToken>(LogFields.NamingStrategy.GetDictionaryKey(name))));
    }
  }
}
