// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ClientCache
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.Cache;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.Server.Cache;
using Encompass.Diagnostics;
using Encompass.Diagnostics.Logging;
using Encompass.Diagnostics.Logging.Schema;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class ClientCache : IClientCache
  {
    private const string className = "ClientCache�";
    private const int LockTimeoutDuringGetCache = 600;
    private IClientContext context;
    private ICacheStore cacheStore;
    private object maxConcurrentLoginsLockObj = new object();
    private int maxConcurrentLogins = -1;
    private object numConcurrentLoginsLockObj = new object();
    private int numConcurrentLogins;

    public ClientCache(IClientContext context, ICacheStore cacheStore)
    {
      this.cacheStore = cacheStore;
      this.context = context;
    }

    public CacheStoreSource CacheStoreSource
    {
      get => this.cacheStore == null ? CacheStoreSource.Unknown : this.cacheStore.Source;
    }

    public CacheSetting CacheSetting
    {
      get => this.cacheStore == null ? CacheSetting.Disabled : this.cacheStore.Setting;
    }

    public void ResetCacheStore(ICacheStore cacheStore) => this.cacheStore = cacheStore;

    public object Get(string name)
    {
      name = name.ToUpper();
      object obj1 = (object) null;
      if (this.context.CurrentRequestCache != null)
        obj1 = this.context.CurrentRequestCache.Get<object>(name);
      if (obj1 != null)
      {
        this.LogMetricsForCache(name, CacheAction.GetFromRequestCache);
        return obj1;
      }
      object obj2 = this.cacheStore.Get(name);
      if (this.context.CurrentRequestCache != null)
        this.context.CurrentRequestCache.Set<object>(name, obj2);
      this.LogMetricsForCache(name, CacheAction.GetFromCacheStore);
      return obj2;
    }

    public T Get<T>(string name)
    {
      name = name.ToUpper();
      T obj1 = default (T);
      if (this.context.CurrentRequestCache != null)
        obj1 = this.context.CurrentRequestCache.Get<T>(name);
      if ((object) obj1 != null)
      {
        this.LogMetricsForCache(name, CacheAction.GetFromRequestCache);
        return obj1;
      }
      T obj2 = this.cacheStore.Get<T>(name);
      if (this.context.CurrentRequestCache != null)
        this.context.CurrentRequestCache.Set<T>(name, obj2);
      this.LogMetricsForCache(name, CacheAction.GetFromCacheStore);
      return obj2;
    }

    public T Get<T>(string classname, string name)
    {
      return this.Get<T>(string.Format("{0}_{1}", (object) classname, (object) name));
    }

    public T Get<T>(string key, Func<T> dbCall) => this.Get<T>(key, dbCall, CacheSetting.Disabled);

    public T Get<T>(string classname, string name, Func<T> dbCall)
    {
      return this.Get<T>(string.Format("{0}_{1}", (object) classname, (object) name), dbCall);
    }

    public T Get<T>(string key, Func<T> dbCall, CacheSetting minCacheLevel)
    {
      key = key.ToUpper();
      T obj = default (T);
      CacheAction valueSource1 = CacheAction.GetFromRequestCache;
      if (this.context.CurrentRequestCache != null)
        obj = this.context.CurrentRequestCache.Get<T>(key);
      if ((object) obj != null)
      {
        this.LogMetricsForCache(key, valueSource1);
        return obj;
      }
      T o;
      CacheAction valueSource2;
      if (minCacheLevel > this.CacheSetting)
      {
        o = dbCall();
        valueSource2 = CacheAction.GetFromBackend;
      }
      else
      {
        try
        {
          o = this.cacheStore.Get<T>(key);
          valueSource2 = CacheAction.GetFromCacheStore;
          if ((object) o == null)
          {
            using (this.cacheStore.Lock(key, LockType.ReaderWriter, 600))
            {
              o = this.cacheStore.Get<T>(key);
              if ((object) o == null)
              {
                o = dbCall();
                valueSource2 = CacheAction.GetFromBackend;
                if ((object) o != null)
                  this.cacheStore.Put(key, (object) o);
              }
            }
          }
        }
        catch (TimeoutException ex)
        {
          try
          {
            TraceLog.WriteWarning(nameof (ClientCache), "Timeout expired while acquiring lock on " + key);
          }
          catch
          {
          }
          o = dbCall();
          valueSource2 = CacheAction.GetFromBackend;
        }
        catch (ApplicationException ex)
        {
          if (ex.Message.IndexOf("timeout period expired") > 0 || ex.HResult == -2147023436)
          {
            try
            {
              TraceLog.WriteWarning(nameof (ClientCache), "Timeout expired while acquiring lock on " + key);
            }
            catch
            {
            }
            o = dbCall();
            valueSource2 = CacheAction.GetFromBackend;
          }
          else
            throw;
        }
      }
      if (this.context.CurrentRequestCache != null)
        this.context.CurrentRequestCache.Set<T>(key, o);
      this.LogMetricsForCache(key, valueSource2);
      return o;
    }

    public IDictionary<string, T> GetAll<T>(
      string[] keys,
      Func<string[], IDictionary<string, T>> dbCall,
      CacheSetting minCacheLevel)
      where T : class
    {
      keys = ((IEnumerable<string>) keys).Select<string, string>((Func<string, string>) (x => x.ToUpper())).ToArray<string>();
      Dictionary<string, T> all = new Dictionary<string, T>();
      if (keys == null || keys.Length == 0)
        return (IDictionary<string, T>) all;
      keys = ((IEnumerable<string>) keys).Distinct<string>().ToArray<string>();
      List<string> source = new List<string>();
      CacheAction valueSource = CacheAction.GetFromRequestCache;
      if (this.context.CurrentRequestCache != null)
      {
        foreach (string key in keys)
        {
          T obj = this.context.CurrentRequestCache.Get<T>(key);
          if ((object) obj == null)
            source.Add(key);
          else
            all.Add(key, obj);
        }
      }
      if (!source.Any<string>())
      {
        foreach (string key in keys)
          this.LogMetricsForCache(key, valueSource);
        return (IDictionary<string, T>) all;
      }
      foreach (string key in all.Keys)
        this.LogMetricsForCache(key, valueSource);
      if (minCacheLevel > this.CacheSetting)
      {
        valueSource = CacheAction.GetFromBackend;
        foreach (KeyValuePair<string, T> keyValuePair in (IEnumerable<KeyValuePair<string, T>>) dbCall(source.ToArray()))
        {
          all.Add(keyValuePair.Key, keyValuePair.Value);
          this.context.CurrentRequestCache?.Set<T>(keyValuePair.Key, keyValuePair.Value);
          this.LogMetricsForCache(keyValuePair.Key, valueSource);
        }
      }
      else
      {
        valueSource = CacheAction.GetFromCacheStore;
        foreach (KeyValuePair<string, T> keyValuePair in (IEnumerable<KeyValuePair<string, T>>) this.cacheStore.GetAll<T>(source.ToArray()))
        {
          all.Add(keyValuePair.Key, keyValuePair.Value);
          this.context.CurrentRequestCache?.Set<T>(keyValuePair.Key, keyValuePair.Value);
          this.LogMetricsForCache(keyValuePair.Key, valueSource);
          source.Remove(keyValuePair.Key);
        }
        if (!source.Any<string>())
          return (IDictionary<string, T>) all;
        ConcurrentDictionary<string, T> remainingValues = new ConcurrentDictionary<string, T>();
        Parallel.ForEach<string>((IEnumerable<string>) source, (Action<string>) (key =>
        {
          valueSource = CacheAction.GetFromCacheStore;
          T obj = default (T);
          T o;
          try
          {
            o = this.cacheStore.Get<T>(key);
            if ((object) o == null)
            {
              using (this.cacheStore.Lock(key, LockType.ReaderWriter, 600))
              {
                o = this.cacheStore.Get<T>(key);
                if ((object) o == null)
                {
                  o = ((Func<string[], IDictionary<string, T>>) dbCall)(new string[1]
                  {
                    key
                  })[key];
                  valueSource = CacheAction.GetFromBackend;
                  if ((object) o != null)
                    this.cacheStore.Put(key, (object) o);
                }
              }
            }
          }
          catch (TimeoutException ex)
          {
            try
            {
              TraceLog.WriteWarning(nameof (ClientCache), "Timeout expired while acquiring lock on " + key);
            }
            catch
            {
            }
            o = ((Func<string[], IDictionary<string, T>>) dbCall)(new string[1]
            {
              key
            })[key];
            valueSource = CacheAction.GetFromBackend;
          }
          catch (ApplicationException ex)
          {
            if (ex.Message.IndexOf("timeout period expired") > 0 || ex.HResult == -2147023436)
            {
              try
              {
                TraceLog.WriteWarning(nameof (ClientCache), "Timeout expired while acquiring lock on " + key);
              }
              catch
              {
              }
              o = ((Func<string[], IDictionary<string, T>>) dbCall)(new string[1]
              {
                key
              })[key];
              valueSource = CacheAction.GetFromBackend;
            }
            else
              throw;
          }
          remainingValues.TryAdd(key, o);
          if (this.context.CurrentRequestCache != null)
            this.context.CurrentRequestCache.Set<T>(key, o);
          this.LogMetricsForCache(key, valueSource);
        }));
        foreach (KeyValuePair<string, T> keyValuePair in remainingValues)
          all.Add(keyValuePair.Key, keyValuePair.Value);
      }
      return (IDictionary<string, T>) all;
    }

    private void LogMetricsForCache(string key, CacheAction valueSource)
    {
      ILogger logger = DiagUtility.LogManager.GetLogger("Metrics.CacheStore");
      logger.When(Encompass.Diagnostics.Logging.LogLevel.DEBUG, (Action) (() =>
      {
        LogFields info = new LogFields().Set<string>(Log.CommonFields.CacheKey, key).Set<string>(Log.CommonFields.CacheAction, valueSource.ToString());
        logger.Write(Encompass.Diagnostics.Logging.LogLevel.DEBUG, nameof (ClientCache), string.Format("Key: '{0}' CacheAction: '{1}'", (object) key, (object) valueSource), info);
      }));
    }

    public T Get<T>(string classname, string name, Func<T> dbCall, CacheSetting minCacheLevel)
    {
      return this.Get<T>(string.Format("{0}_{1}", (object) classname, (object) name), dbCall, minCacheLevel);
    }

    public void Put(string name, object o, CacheSetting minCacheSetting)
    {
      name = name.ToUpper();
      if (this.CacheSetting >= minCacheSetting)
      {
        this.Put(name, o);
      }
      else
      {
        if (this.context.CurrentRequestCache == null)
          return;
        this.context.CurrentRequestCache.Set<object>(name, o);
      }
    }

    public void Put(string name, object o)
    {
      name = name.ToUpper();
      this.cacheStore.Put(name, o);
      this.LogMetricsForCache(name, CacheAction.Put);
      if (this.context.CurrentRequestCache == null)
        return;
      this.context.CurrentRequestCache.Set<object>(name, o);
    }

    public void Remove(string name)
    {
      name = name.ToUpper();
      this.cacheStore.Remove(name);
      this.LogMetricsForCache(name, CacheAction.Remove);
      if (this.context.CurrentRequestCache == null)
        return;
      this.context.CurrentRequestCache.Remove(name);
    }

    public IDisposable Lock(string name, LockType lockType = LockType.ReaderWriter, int timeout = 20000, bool supressWarning = false)
    {
      name = name.ToUpper();
      return this.cacheStore.Lock(name, lockType, timeout, supressWarning);
    }

    public bool TryGetLock(
      string key,
      LockType lockType,
      int timeout,
      out IDisposable hzcLockObj,
      bool suppressTimeoutWarning = false)
    {
      key = key.ToUpper();
      hzcLockObj = (IDisposable) null;
      try
      {
        hzcLockObj = this.Lock(key, lockType, timeout, suppressTimeoutWarning);
        return hzcLockObj != null;
      }
      catch (Exception ex)
      {
        if (!suppressTimeoutWarning || timeout > 0)
          TraceLog.WriteError(nameof (ClientCache), "Exception while acquiring the lock : " + ex.Message);
        return false;
      }
    }

    public ICacheLock<T> CheckOut<T>(
      string classname,
      string key,
      object identifier,
      int timeout = 20000,
      bool toAcquireLock = true)
    {
      if (this.CacheSetting == CacheSetting.Disabled)
        return this.CheckOutWithNull<T>(classname, key, identifier, timeout);
      string upper = string.Format("{0}_{1}", (object) classname, (object) key).ToUpper();
      IDisposable lockObj = (IDisposable) null;
      if (toAcquireLock)
        lockObj = this.Lock(upper, LockType.ReaderWriter, timeout, false);
      try
      {
        return (ICacheLock<T>) new CacheLock<T>(upper, identifier, this.Get<T>(upper), lockObj, this.cacheStore, true);
      }
      catch
      {
        lockObj?.Dispose();
        throw;
      }
    }

    public ICacheLock<T> CheckOutWithNull<T>(
      string classname,
      string key,
      object identifier,
      int timeout = 20000)
    {
      string upper = string.Format("{0}_{1}", (object) classname, (object) key).ToUpper();
      IDisposable lockObj = this.Lock(upper, LockType.ReaderWriter, timeout, false);
      try
      {
        return (ICacheLock<T>) new CacheLock<T>(upper, identifier, default (T), lockObj, this.cacheStore, false);
      }
      catch
      {
        lockObj.Dispose();
        throw;
      }
    }

    public void Clear()
    {
      if (this.context == null)
        return;
      this.context.TraceLog.WriteDebug(nameof (ClientCache), "Clearing cache for context '" + this.context.InstanceName + "'");
      foreach (string name in new ArrayList((ICollection) this.GetKeys()))
      {
        this.cacheStore.Remove(name);
        this.context.TraceLog.WriteDebug(nameof (ClientCache), "Removed value '" + name + "' from cache");
      }
      lock (this.maxConcurrentLoginsLockObj)
        this.maxConcurrentLogins = -1;
    }

    public void ClearAll()
    {
      if (this.context == null)
        return;
      this.context.TraceLog.WriteDebug(nameof (ClientCache), "Clearing all cache for context '" + this.context.InstanceName + "'");
      this.cacheStore.ClearAll();
      lock (this.maxConcurrentLoginsLockObj)
        this.maxConcurrentLogins = -1;
    }

    public int GetMaxConcurrentLogins(IClientContext context = null)
    {
      lock (this.maxConcurrentLoginsLockObj)
      {
        if (this.maxConcurrentLogins < 0 && context != null)
          this.maxConcurrentLogins = (int) context.Settings.GetServerSetting("Internal.MaxConcurrentLogins");
        return this.maxConcurrentLogins;
      }
    }

    public void SetMaxConcurrentLogins(int val, IClientContext context = null)
    {
      lock (this.maxConcurrentLoginsLockObj)
      {
        this.maxConcurrentLogins = val;
        context?.Settings.SetServerSetting("Internal.MaxConcurrentLogins", (object) val);
      }
    }

    public int IncrementNumConcurrentLogins()
    {
      lock (this.numConcurrentLoginsLockObj)
      {
        if (this.numConcurrentLogins < 0)
          this.numConcurrentLogins = 1;
        else
          ++this.numConcurrentLogins;
        return this.numConcurrentLogins;
      }
    }

    public void DecrementNumConcurrentLogins()
    {
      lock (this.numConcurrentLoginsLockObj)
      {
        if (this.numConcurrentLogins > 0)
          --this.numConcurrentLogins;
        else
          this.numConcurrentLogins = 0;
      }
    }

    public int GetNumConcurrentLogins()
    {
      lock (this.numConcurrentLoginsLockObj)
        return this.numConcurrentLogins;
    }

    public T Put<T>(
      string name,
      Func<T> dbCallPutAndGet,
      CacheSetting minCacheLevelm,
      int timeout = 20000)
    {
      return this.Put<T>(name, (Action) (() => { }), dbCallPutAndGet, minCacheLevelm, timeout);
    }

    public T Put<T>(
      string name,
      Action dbCallPut,
      Func<T> dbCallGet,
      CacheSetting minCacheLevelm,
      int timeout = 20000)
    {
      name = name.ToUpper();
      T o = default (T);
      if (minCacheLevelm <= this.CacheSetting)
      {
        using (this.Lock(name, LockType.ReaderWriter, timeout, false))
        {
          this.Remove(name);
          using (this.context.MakeCurrent(forcePrimaryDB: new bool?(true)))
          {
            dbCallPut();
            o = dbCallGet();
          }
          this.Put(name, (object) o);
        }
      }
      else
      {
        if (this.context.CurrentRequestCache != null)
          this.context.CurrentRequestCache.Remove(name);
        using (ClientContext.GetCurrent().MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?(true)))
        {
          dbCallPut();
          o = dbCallGet();
        }
        if (this.context.CurrentRequestCache != null)
          this.context.CurrentRequestCache.Set<T>(name, o);
      }
      return o;
    }

    public T PutIf<T>(
      string name,
      Func<bool> condition,
      Action dbCallPut,
      Func<T> dbCallGet,
      CacheSetting minCacheLevelm,
      int timeout = 20000)
    {
      name = name.ToUpper();
      T o = default (T);
      if (minCacheLevelm <= this.CacheSetting)
      {
        using (this.Lock(name, LockType.ReaderWriter, timeout, false))
        {
          if (condition())
          {
            this.Remove(name);
            dbCallPut();
            using (ClientContext.GetCurrent().MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?(true)))
              o = dbCallGet();
            this.Put(name, (object) o);
          }
          else
            o = this.Get<T>(name, dbCallGet, minCacheLevelm);
        }
      }
      else if (condition())
      {
        if (this.context.CurrentRequestCache != null)
          this.context.CurrentRequestCache.Remove(name);
        dbCallPut();
        using (this.context.MakeCurrent(forcePrimaryDB: new bool?(true)))
          o = dbCallGet();
        if (this.context.CurrentRequestCache != null)
          this.context.CurrentRequestCache.Set<T>(name, o);
      }
      else
        o = this.Get<T>(name, dbCallGet, minCacheLevelm);
      return o;
    }

    public void Remove(string name, Action dbCallPut, CacheSetting minCacheLevelm, int timeout = 20000)
    {
      name = name.ToUpper();
      if (minCacheLevelm <= this.CacheSetting)
      {
        using (this.Lock(name, LockType.ReaderWriter, timeout, false))
        {
          this.Remove(name);
          dbCallPut();
        }
      }
      else
      {
        if (this.context.CurrentRequestCache != null)
          this.context.CurrentRequestCache.Remove(name);
        dbCallPut();
      }
    }

    public string GetStats()
    {
      return this.cacheStore != null ? this.cacheStore.GetStats() : (string) null;
    }

    public IDisposable EnterContext()
    {
      return this.cacheStore != null ? this.cacheStore.EnterContext() : (IDisposable) null;
    }

    public string[] GetKeys() => this.cacheStore.Keys();
  }
}
