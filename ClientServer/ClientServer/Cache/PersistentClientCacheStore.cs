// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Cache.PersistentClientCacheStore
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using Encompass.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Cache
{
  public class PersistentClientCacheStore : IClientCacheStore, IDisposable
  {
    private Dictionary<string, object> memoryCache;
    private IFileCache persistentCache;

    void IDisposable.Dispose()
    {
      this.memoryCache.Clear();
      this.persistentCache?.Dispose();
    }

    public PersistentClientCacheStore(string systemID)
      : this(PersistentClientCacheStore.CreateFileCache(systemID))
    {
    }

    public PersistentClientCacheStore(IFileCache clientCache)
    {
      this.memoryCache = new Dictionary<string, object>();
      this.persistentCache = clientCache;
    }

    public T GetValue<T>(string cacheKey)
    {
      PersistentClientCacheStore.CacheItem<T> cacheItem = this.GetInternal<T>(cacheKey);
      return cacheItem == null ? default (T) : cacheItem.Value;
    }

    public void Store<T>(string cacheKey, string version, T value)
    {
      if (this.persistentCache == null)
        return;
      Stopwatch stopwatch = Stopwatch.StartNew();
      try
      {
        PersistentClientCacheStore.CacheItem<T> cacheItem = new PersistentClientCacheStore.CacheItem<T>()
        {
          Version = version,
          Value = value
        };
        this.memoryCache[cacheKey] = (object) cacheItem;
        EllieMae.EMLite.RemotingServices.BinaryObject data = PersistentClientCacheStore.Serialize((object) cacheItem);
        this.persistentCache.Put(cacheKey, data);
        stopwatch.Stop();
        Tracing.Log(true, "INFO", nameof (PersistentClientCacheStore), string.Format("Saved item \"{0}\" to cache. Time spent {1}ms. Compressed size: {2:N0} bytes", (object) cacheKey, (object) stopwatch.ElapsedMilliseconds, (object) data.Length));
      }
      catch (Exception ex)
      {
        this.Reset(cacheKey);
        stopwatch.Stop();
        Tracing.Log(true, "ERROR", nameof (PersistentClientCacheStore), string.Format("Error while storing item {0}. Time spent {1}ms. Exception: {2}", (object) cacheKey, (object) stopwatch.ElapsedMilliseconds, (object) ex));
      }
    }

    public string GetETag<T>(string cacheKey) => this.GetInternal<T>(cacheKey)?.Version;

    private PersistentClientCacheStore.CacheItem<T> GetInternal<T>(string cacheKey)
    {
      if (this.persistentCache == null)
        return (PersistentClientCacheStore.CacheItem<T>) null;
      Stopwatch stopwatch = Stopwatch.StartNew();
      try
      {
        object obj;
        if (this.memoryCache.TryGetValue(cacheKey, out obj))
          return (PersistentClientCacheStore.CacheItem<T>) obj;
        EllieMae.EMLite.RemotingServices.BinaryObject bo = this.persistentCache.Get(cacheKey);
        if (bo == null)
        {
          this.memoryCache.Add(cacheKey, (object) null);
          stopwatch.Stop();
          Tracing.Log(true, "INFO", nameof (PersistentClientCacheStore), string.Format("Item for \"{0}\" not found in cache. Time spent {1}ms", (object) cacheKey, (object) stopwatch.ElapsedMilliseconds));
          return (PersistentClientCacheStore.CacheItem<T>) null;
        }
        PersistentClientCacheStore.CacheItem<T> cacheItem = PersistentClientCacheStore.Deserialize<T>(bo);
        this.memoryCache.Add(cacheKey, (object) cacheItem);
        stopwatch.Stop();
        Tracing.Log(true, "INFO", nameof (PersistentClientCacheStore), string.Format("Item \"{0}\" retrieved from cache. Time spent {1}ms. Compressed size: {2:N0} bytes", (object) cacheKey, (object) stopwatch.ElapsedMilliseconds, (object) bo.Length));
        return cacheItem;
      }
      catch (Exception ex)
      {
        this.Reset(cacheKey);
        stopwatch.Stop();
        Tracing.Log(true, "ERROR", nameof (PersistentClientCacheStore), string.Format("Error while retrieving cached item value for {0}. Time spent {1}ms. Exception: {2}", (object) cacheKey, (object) stopwatch.ElapsedMilliseconds, (object) ex));
        return (PersistentClientCacheStore.CacheItem<T>) null;
      }
    }

    public void Reset(string cacheKey)
    {
      if (this.persistentCache == null)
        return;
      try
      {
        this.memoryCache.Remove(cacheKey);
        File.Delete(this.persistentCache.GetFilePath(cacheKey));
      }
      catch (Exception ex)
      {
        Tracing.Log(true, "ERROR", nameof (PersistentClientCacheStore), string.Format("Error while clearing cached item {0}: {1}", (object) cacheKey, (object) ex));
      }
    }

    private static EllieMae.EMLite.RemotingServices.BinaryObject Serialize(object obj)
    {
      if (obj == null)
        return (EllieMae.EMLite.RemotingServices.BinaryObject) null;
      using (ByteStream byteStream = new ByteStream(false))
      {
        new BinaryFormatter().Serialize((Stream) byteStream, obj);
        byte[] bytesData = DataProtectionAPI.EncryptStream(FileCompressor.Instance.Zip(byteStream), DataProtectionScope.CurrentUser);
        Tracing.Log(true, "INFO", nameof (PersistentClientCacheStore), "Encrypted the cache data.");
        return new EllieMae.EMLite.RemotingServices.BinaryObject(bytesData, false);
      }
    }

    private static PersistentClientCacheStore.CacheItem<T> Deserialize<T>(EllieMae.EMLite.RemotingServices.BinaryObject bo)
    {
      BinaryFormatter binaryFormatter = new BinaryFormatter();
      object obj = (object) null;
      byte[] bytes1 = bo.GetBytes();
      if (bytes1 != null && bytes1.Length != 0)
      {
        byte[] bytes2 = DataProtectionAPI.DecryptStream(bytes1, DataProtectionScope.CurrentUser);
        Tracing.Log(true, "INFO", nameof (PersistentClientCacheStore), "Decrypted the cache data.");
        using (ByteStream serializationStream = FileCompressor.Instance.Unzip(bytes2))
          obj = binaryFormatter.Deserialize((Stream) serializationStream);
      }
      return (PersistentClientCacheStore.CacheItem<T>) obj;
    }

    private static IFileCache CreateFileCache(string systemID)
    {
      try
      {
        return (IFileCache) new ClientCache(systemID, "ClientCacheStore", 500);
      }
      catch (TimeoutException ex)
      {
        Tracing.Log(true, "INFO", nameof (PersistentClientCacheStore), "Timeout while acquiring mutex lock for PersistentClientCacheStore");
      }
      catch (Exception ex)
      {
        Tracing.Log(true, "INFO", nameof (PersistentClientCacheStore), "Error while initializing PersistentClientCacheStore: " + ex.Message);
      }
      return (IFileCache) null;
    }

    [Serializable]
    private class CacheItem<T>
    {
      public string Version { get; set; }

      public T Value { get; set; }
    }
  }
}
