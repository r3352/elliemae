// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Cache.LruCache
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using EllieMae.EMLite.Cache;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.Caching;
using System.Threading;

#nullable disable
namespace EllieMae.EMLite.Common.Cache
{
  public class LruCache : ILruDataCache, IDataCache
  {
    private static readonly object NULL_SUBSTITUTE = new object();
    private readonly Dictionary<string, object> _cacheLocks = new Dictionary<string, object>();
    private readonly MemoryCache _cacheValues;
    private readonly TimeSpan _slidingExpiration;

    public LruCache()
      : this(nameof (LruCache))
    {
    }

    public LruCache(string cacheName, bool useAbsoluteExpiration = false)
    {
      this._cacheValues = new MemoryCache(cacheName);
      if (useAbsoluteExpiration)
      {
        this._slidingExpiration = ObjectCache.NoSlidingExpiration;
      }
      else
      {
        int result;
        if (int.TryParse(ConfigurationManager.AppSettings["LruCache.ExpirationMinutes"], out result) && result >= 0)
          this._slidingExpiration = TimeSpan.FromMinutes((double) result);
        else
          this._slidingExpiration = ObjectCache.NoSlidingExpiration;
      }
    }

    public T Get<T>(string key)
    {
      if (!this._cacheValues.Contains(key, (string) null))
        return default (T);
      object obj = this._cacheValues.Get(key, (string) null);
      if (obj == LruCache.NULL_SUBSTITUTE)
        obj = (object) null;
      return (T) obj;
    }

    public T Get<T>(string key, Func<T> loaderFunc)
    {
      return this.Get<T>(key, loaderFunc, ObjectCache.InfiniteAbsoluteExpiration);
    }

    public T Get<T>(string key, Func<T> loaderFunc, int absoluteExpirationMinutes)
    {
      return this.Get<T>(key, loaderFunc, absoluteExpirationMinutes > 0 ? DateTimeOffset.Now.AddMinutes((double) absoluteExpirationMinutes) : ObjectCache.InfiniteAbsoluteExpiration);
    }

    private T Get<T>(string key, Func<T> loaderFunc, DateTimeOffset absoluteExpiration)
    {
      object obj = this._cacheValues.Get(key, (string) null);
      if (obj == null)
      {
        object lockObject = this.AcquireLock(key);
        if ((obj = this._cacheValues.Get(key, (string) null)) == null)
        {
          try
          {
            obj = (object) loaderFunc();
            this.SetValue<object>(key, obj, absoluteExpiration);
          }
          finally
          {
            this.ReleaseLock(lockObject);
          }
        }
        else
          Tracing.Log(Tracing.SwCommon, TraceLevel.Verbose, nameof (LruCache), "Serving from data from Cache for key :" + key);
      }
      else
        Tracing.Log(Tracing.SwCommon, TraceLevel.Verbose, nameof (LruCache), "Serving from data from Cache for key :" + key);
      if (obj == LruCache.NULL_SUBSTITUTE)
        obj = (object) null;
      return (T) obj;
    }

    public void Set<T>(string key, T value)
    {
      object lockObject = this.AcquireLock(key);
      try
      {
        this.SetValue<T>(key, value, ObjectCache.InfiniteAbsoluteExpiration);
      }
      finally
      {
        this.ReleaseLock(lockObject);
      }
    }

    private void SetValue<T>(string key, T value, DateTimeOffset absoluteExpiration)
    {
      object obj = (object) value;
      if ((object) value == null)
        obj = LruCache.NULL_SUBSTITUTE;
      this._cacheValues.Set(key, obj, new CacheItemPolicy()
      {
        Priority = CacheItemPriority.Default,
        AbsoluteExpiration = absoluteExpiration,
        SlidingExpiration = this._slidingExpiration,
        RemovedCallback = (CacheEntryRemovedCallback) (args =>
        {
          if (args.RemovedReason != CacheEntryRemovedReason.Evicted && args.RemovedReason != CacheEntryRemovedReason.Expired)
            return;
          lock (this._cacheLocks)
            this._cacheLocks.Remove(args.CacheItem.Key);
        })
      }, (string) null);
    }

    private object AcquireLock(string key)
    {
      object obj = (object) null;
      lock (this._cacheLocks)
      {
        if (!this._cacheLocks.TryGetValue(key, out obj))
        {
          obj = new object();
          this._cacheLocks.Add(key, obj);
        }
      }
      Monitor.Enter(obj);
      return obj;
    }

    private void ReleaseLock(object lockObject)
    {
      if (lockObject == null)
        return;
      Monitor.Exit(lockObject);
    }

    public void Remove(string key)
    {
      object lockObject = this.AcquireLock(key);
      try
      {
        if (!this._cacheValues.Contains(key, (string) null))
          return;
        this._cacheValues.Remove(key, (string) null);
      }
      finally
      {
        this.ReleaseLock(lockObject);
      }
    }
  }
}
