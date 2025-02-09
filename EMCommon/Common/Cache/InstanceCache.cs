// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Cache.InstanceCache
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using EllieMae.EMLite.Cache;
using System;

#nullable disable
namespace EllieMae.EMLite.Common.Cache
{
  public class InstanceCache : IInstanceCache, IDataCache
  {
    private readonly string _instanceName;
    private readonly IDataCache _lruCache;
    private readonly IDataCache _requestCache;

    public InstanceCache(string instanceName, IDataCache requestCache, IDataCache lruCache)
    {
      this._instanceName = string.IsNullOrEmpty(instanceName) ? "(Default)" : instanceName;
      this._lruCache = lruCache;
      this._requestCache = requestCache;
    }

    public T Get<T>(string key) => throw new NotSupportedException();

    public T Get<T>(string key, Func<T> loaderFunc)
    {
      return this._lruCache == null ? this._requestCache.Get<T>(key, loaderFunc) : this._requestCache.Get<T>(key, (Func<T>) (() => this._lruCache.Get<T>(this._instanceName + "/" + key, loaderFunc)));
    }

    public T Get<T>(string key, Func<T> loaderFunc, Func<DateTime> lastUpdatedTime)
    {
      return this._lruCache == null ? this._requestCache.Get<T>(key, loaderFunc) : this._requestCache.Get<T>(key, (Func<T>) (() =>
      {
        Tuple<DateTime, T> tuple = this._lruCache.Get<Tuple<DateTime, T>>(this._instanceName + "/" + key);
        if (tuple == null || DateTime.Compare(tuple.Item1, lastUpdatedTime()) < 0)
          tuple = new Tuple<DateTime, T>(lastUpdatedTime(), loaderFunc());
        this._lruCache.Set<Tuple<DateTime, T>>(this._instanceName + "/" + key, tuple);
        return tuple.Item2;
      }));
    }

    public void Remove(string key)
    {
      this._requestCache.Remove(key);
      if (this._lruCache == null)
        return;
      this._lruCache.Remove(this._instanceName + "/" + key);
    }

    public void Set<T>(string key, T value)
    {
      this._requestCache.Set<T>(key, value);
      if (this._lruCache == null)
        return;
      this._lruCache.Set<T>(this._instanceName + "/" + key, value);
    }
  }
}
