// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.IClientCache
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.Configuration;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public interface IClientCache
  {
    CacheStoreSource CacheStoreSource { get; }

    CacheSetting CacheSetting { get; }

    void ResetCacheStore(ICacheStore cacheStore);

    object Get(string name);

    T Get<T>(string name);

    T Get<T>(string classname, string name);

    T Get<T>(string name, Func<T> dbCall);

    T Get<T>(string classname, string name, Func<T> dbCall);

    T Get<T>(string key, Func<T> dbCall, CacheSetting minCacheLevel);

    IDictionary<string, T> GetAll<T>(
      string[] keys,
      Func<string[], IDictionary<string, T>> dbCall,
      CacheSetting minCacheLevel)
      where T : class;

    T Get<T>(string classname, string name, Func<T> dbCall, CacheSetting minCacheLevel);

    void Put(string name, object o, CacheSetting minCacheSetting);

    void Put(string name, object o);

    T Put<T>(string key, Func<T> dbCallPutAndGet, CacheSetting minCacheLevel, int timeout = 20000);

    T Put<T>(
      string key,
      Action dbCallPut,
      Func<T> dbCallGet,
      CacheSetting minCacheLevel,
      int timeout = 20000);

    T PutIf<T>(
      string key,
      Func<bool> condition,
      Action dbCallPut,
      Func<T> dbCallGet,
      CacheSetting minCacheLevel,
      int timeout = 20000);

    void Remove(string name);

    void Remove(string key, Action dbCall, CacheSetting minCacheLevel, int timeout = 20000);

    IDisposable Lock(string key, LockType lockType = LockType.ReaderWriter, int timeout = 20000, bool supressWarning = false);

    bool TryGetLock(
      string key,
      LockType lockType,
      int timeout,
      out IDisposable hzcLockObj,
      bool suppressTimeoutWarning = false);

    ICacheLock<T> CheckOutWithNull<T>(
      string classname,
      string key,
      object identifier,
      int timeout = 20000);

    void Clear();

    int GetMaxConcurrentLogins(IClientContext context = null);

    void SetMaxConcurrentLogins(int val, IClientContext context = null);

    int IncrementNumConcurrentLogins();

    void DecrementNumConcurrentLogins();

    int GetNumConcurrentLogins();

    string[] GetKeys();

    ICacheLock<T> CheckOut<T>(
      string classname,
      string key,
      object identifier,
      int timeout = 20000,
      bool toAcquireLock = true);

    string GetStats();

    IDisposable EnterContext();

    void ClearAll();
  }
}
