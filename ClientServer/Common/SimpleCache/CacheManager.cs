// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.SimpleCache.CacheManager
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

#nullable disable
namespace EllieMae.EMLite.Common.SimpleCache
{
  public class CacheManager
  {
    private static Dictionary<string, ICache> _caches = new Dictionary<string, ICache>();

    static CacheManager()
    {
      CacheManager._caches.Add("AccessTokenCache", (ICache) new CacheManager.SimpleCacheThreadSafe());
      CacheManager._caches.Add("PartnerResponseCache", (ICache) new CacheManager.SimpleCacheThreadSafe());
      CacheManager._caches.Add("SsoTokenCache", (ICache) new CacheManager.SimpleCacheThreadSafe());
      CacheManager._caches.Add("OAuth2Cache", (ICache) new CacheManager.SimpleCacheThreadSafe());
      CacheManager._caches.Add("UserInfoCache", (ICache) new CacheManager.SimpleCacheThreadSafe());
    }

    public static ICache GetSimpleCache(string cacheName) => CacheManager._caches[cacheName];

    internal class SimpleCacheThreadSafe : ICache
    {
      private const string className = "SimpleCacheThreadSafe�";
      protected static string sw = Tracing.SwOutsideLoan;
      private static object _lockObject = new object();
      private Dictionary<string, CacheItem> _keyValueCollection = new Dictionary<string, CacheItem>();

      public object Get(string key)
      {
        CacheItem cacheItem;
        if (!this._keyValueCollection.TryGetValue(key, out cacheItem))
          return (object) null;
        if (!cacheItem.IsExpired())
          return cacheItem.Value;
        Tracing.Log(CacheManager.SimpleCacheThreadSafe.sw, TraceLevel.Verbose, nameof (SimpleCacheThreadSafe), "CacheItem Expired : " + key);
        bool lockTaken = false;
        Monitor.TryEnter(CacheManager.SimpleCacheThreadSafe._lockObject, ref lockTaken);
        try
        {
          if (lockTaken)
            this._keyValueCollection.Remove(key);
        }
        finally
        {
          if (lockTaken)
            Monitor.Exit(CacheManager.SimpleCacheThreadSafe._lockObject);
        }
        return (object) null;
      }

      public T Get<T>(
        string key,
        Func<T> dbCall,
        CacheItemRetentionPolicy cacheItemRetentionPolicy)
      {
        T obj1 = default (T);
        T obj2 = (T) this.Get(key);
        if ((object) obj2 != null)
        {
          Tracing.Log(CacheManager.SimpleCacheThreadSafe.sw, TraceLevel.Verbose, nameof (SimpleCacheThreadSafe), "Value returned from Cache for : " + key);
          return obj2;
        }
        lock (CacheManager.SimpleCacheThreadSafe._lockObject)
        {
          if (this._keyValueCollection.ContainsKey(key))
            return (T) this._keyValueCollection[key].Value;
          Tracing.Log(CacheManager.SimpleCacheThreadSafe.sw, TraceLevel.Verbose, nameof (SimpleCacheThreadSafe), "Get value from DB for : " + key);
          obj2 = dbCall();
          if ((object) obj2 != null)
            this.Put(key, new CacheItem((object) obj2, cacheItemRetentionPolicy, (ICacheItemExpirationBehavior) new SimpleCacheItemExpirationBehavior()));
        }
        return obj2;
      }

      public void Put(string key, CacheItem cacheItem)
      {
        lock (CacheManager.SimpleCacheThreadSafe._lockObject)
        {
          if (this._keyValueCollection.ContainsKey(key))
          {
            this._keyValueCollection[key] = cacheItem;
          }
          else
          {
            this._keyValueCollection.Add(key, cacheItem);
            Tracing.Log(CacheManager.SimpleCacheThreadSafe.sw, TraceLevel.Verbose, nameof (SimpleCacheThreadSafe), "Value added to Cache for : " + key);
          }
        }
      }

      public void Remove(string key)
      {
        if (!this._keyValueCollection.ContainsKey(key))
          return;
        bool lockTaken = false;
        Monitor.TryEnter(CacheManager.SimpleCacheThreadSafe._lockObject, ref lockTaken);
        try
        {
          if (!lockTaken)
            return;
          this._keyValueCollection.Remove(key);
          Tracing.Log(CacheManager.SimpleCacheThreadSafe.sw, TraceLevel.Verbose, nameof (SimpleCacheThreadSafe), "Value removed from Cache for  : " + key);
        }
        finally
        {
          if (lockTaken)
            Monitor.Exit(CacheManager.SimpleCacheThreadSafe._lockObject);
        }
      }

      public void Flush()
      {
        lock (CacheManager.SimpleCacheThreadSafe._lockObject)
          this._keyValueCollection.Clear();
      }
    }
  }
}
