// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Deferrables.DeferrableDataBag`1
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.Deferrables
{
  public class DeferrableDataBag<T>
  {
    private readonly ConcurrentDictionary<string, object> _data = new ConcurrentDictionary<string, object>();

    public static DeferrableDataBag<T> GetInstance()
    {
      IRequestContext currentRequest = ClientContext.CurrentRequest;
      if (currentRequest == null)
        throw new InvalidOperationException("There is no current request context established");
      string key = string.Format("{0}", (object) typeof (T).ToString());
      DeferrableDataBag<T> instance1 = currentRequest.RequestCache.Get<DeferrableDataBag<T>>(key);
      if (instance1 != null)
        return instance1;
      DeferrableDataBag<T> instance2 = new DeferrableDataBag<T>();
      currentRequest.RequestCache.Set<DeferrableDataBag<T>>(key, instance2);
      return instance2;
    }

    public DeferrableDataBag()
    {
    }

    public DeferrableDataBag(ConcurrentDictionary<string, object> data)
    {
      this._data = new ConcurrentDictionary<string, object>((IEnumerable<KeyValuePair<string, object>>) data);
    }

    public ConcurrentDictionary<string, object> Data => this._data;

    public DeferrableDataBag<T> Set(string key, object val)
    {
      if (this._data.ContainsKey(key))
        this._data[key] = val;
      else
        this._data.TryAdd(key, val);
      return this;
    }

    public TVal Get<TVal>(string key)
    {
      object obj;
      if (!this._data.TryGetValue(key, out obj))
        return default (TVal);
      return obj == null ? default (TVal) : (TVal) obj;
    }

    public void Clear() => this._data.Clear();
  }
}
