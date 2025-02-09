// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.HazelCastCacheStore
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Elli.Metrics;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Version;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Server.Configuration;
using EllieMae.EMLite.VersionInterface15;
using Encompass.Diagnostics;
using Encompass.Diagnostics.Logging;
using Encompass.Diagnostics.Logging.Schema;
using Hazelcast;
using Hazelcast.Core;
using Hazelcast.DistributedObjects;
using Hazelcast.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace EllieMae.EMLite.Server
{
  internal class HazelCastCacheStore : ICacheStore, IHealthCheckCacheStore
  {
    internal readonly string _mapName;
    private const string ClassName = "HazelCastCacheStore�";
    private const string HZC_LEASE_EXPIRED_ERROR_MESSAGE = "Current thread is not owner of the lock�";
    private static readonly string MapSuffix;
    internal static readonly int leaseTime;
    private static readonly int largeObjectSize;
    private static readonly int nearCacheObjectSize;
    private readonly string instanceId;
    private readonly InProcCacheStore nearCache = new InProcCacheStore(true);
    private IMetricsFactory metricsFactory;
    private IHazelCastMetricsRecorder hazelCastRecorder;
    public static readonly string VersionNumber;
    private static HazelcastConnectionPool _connectionPool = new HazelcastConnectionPool((CacheStoreConfiguration) ConfigurationManager.GetSection("CacheStoreSettings"));

    static HazelCastCacheStore()
    {
      TraceLog.WriteDebug(nameof (HazelCastCacheStore), "CACHE MODE IS SET TO HAZELCAST");
      CacheStoreConfiguration section = (CacheStoreConfiguration) ConfigurationManager.GetSection("CacheStoreSettings");
      HazelCastCacheStore.MapSuffix = section.MapSuffix;
      HazelCastCacheStore.leaseTime = section.LeaseTime;
      HazelCastCacheStore.largeObjectSize = section.LargeObjectSize;
      HazelCastCacheStore.nearCacheObjectSize = section.NearCacheObjectSize;
      if (VersionInformation.CurrentVersion != null)
      {
        JedVersion version = VersionInformation.CurrentVersion.Version;
        HazelCastCacheStore.VersionNumber = VersionInformation.CurrentVersion.Version.FullVersion;
      }
      else
        HazelCastCacheStore.VersionNumber = string.Empty;
    }

    public HazelCastCacheStore(string instanceId, string mapPrefix = "CACHE�")
    {
      this.metricsFactory = (IMetricsFactory) new MetricsFactory();
      this.hazelCastRecorder = this.metricsFactory.CreateHazelCastMetricsRecorder();
      this.instanceId = instanceId;
      this._mapName = string.Format("{2}_{0}{1}", string.IsNullOrEmpty(instanceId) ? (object) "DefaultInstance" : (object) instanceId.ToUpper(), (object) HazelCastCacheStore.MapSuffix, (object) mapPrefix);
    }

    public CacheStoreSource Source => CacheStoreSource.HazelCast;

    public CacheSetting Setting => CacheSetting.Low;

    public bool IsRemoteCache => true;

    internal T HazelCastStoreOperation<T>(
      string operationName,
      HazelcastConnection conn,
      Func<IHazelcastClient, Task<T>> operation)
    {
      T result = default (T);
      Task.Run<T>((Func<Task<T>>) (async () =>
      {
        T obj = await conn.HazelCastStoreOperation<T>(operationName, operation);
        return result = obj;
      })).WaitOrThrowOnError();
      this.hazelCastRecorder.IncrementHazelCastOperationCount(operationName, this.instanceId, HazelCastCacheStore.VersionNumber);
      return result;
    }

    public object Get(string name)
    {
      using (PerformanceMeter.Current.BeginOperation("HazelCastCacheStore.Get"))
      {
        Stopwatch sw = Stopwatch.StartNew();
        try
        {
          using (HazelcastConnection conn = HazelCastCacheStore._connectionPool.Open())
          {
            byte[] cacheValue = this.HazelCastStoreOperation<byte[]>(nameof (Get), conn, (Func<IHazelcastClient, Task<byte[]>>) (async client => await (await client.GetMapAsync<string, byte[]>(this._mapName)).GetAsync(name)));
            sw.Stop();
            ILogger logger = DiagUtility.LogManager.GetLogger("Metrics.CacheStore");
            logger.When(Encompass.Diagnostics.Logging.LogLevel.DEBUG, (Action) (() =>
            {
              LogFields logFields = new LogFields().Set<string>(HazelCastCacheStore.LogFieldNames.Action, nameof (Get)).Set<string>(HazelCastCacheStore.LogFieldNames.Key, name).Set<int>(HazelCastCacheStore.LogFieldNames.QueuedInstanceId, conn.QueuedHazelCastInstance.Id).Set<int>(HazelCastCacheStore.LogFieldNames.QueuedInstanceRefCount, conn.QueuedHazelCastInstance.Refcount).Set<double>(Encompass.Diagnostics.Logging.Schema.Log.CommonFields.DurationMS, (double) sw.ElapsedMilliseconds);
              if (cacheValue != null)
                logFields.Set<int>(HazelCastCacheStore.LogFieldNames.DataBytes, cacheValue.Length);
              ILogger logger1 = logger;
              byte[] numArray = cacheValue;
              string message = string.Format("Hazelcast Get operation transferred {0} bytes", (object) (numArray != null ? numArray.Length : 0));
              LogFields info = logFields;
              logger1.Write(Encompass.Diagnostics.Logging.LogLevel.DEBUG, "HazelcastCacheStore", message, info);
            }));
            object obj;
            try
            {
              obj = cacheValue == null ? (object) null : HazelCastCacheStore.Deserialize(cacheValue);
            }
            catch (SerializationException ex)
            {
              this.Remove(name);
              return (object) null;
            }
            catch (InvalidDataException ex)
            {
              this.Remove(name);
              return (object) null;
            }
            catch (TargetInvocationException ex)
            {
              if (!(ex.InnerException is SerializationException))
                throw ex;
              this.Remove(name);
              return (object) null;
            }
            if (obj is HazelCastCacheStore.ObjectHash)
            {
              HazelCastCacheStore.NearCacheItem nearCacheItem = this.nearCache.Get<HazelCastCacheStore.NearCacheItem>(name);
              obj = nearCacheItem == null || !nearCacheItem.Hash.Equals(obj) ? (nearCacheItem == null ? (object) null : (object) null) : HazelCastCacheStore.Deserialize(nearCacheItem.ObjectData);
            }
            return obj;
          }
        }
        catch (Exception ex)
        {
          sw.Stop();
          this.hazelCastRecorder.IncrementErrorCount(nameof (Get), this.instanceId, HazelCastCacheStore.VersionNumber);
          TraceLog.WriteError(nameof (HazelCastCacheStore), string.Format("HazelCast Get for key={0} failed with error={1} CT={2}_{3} Trace={4}", (object) name, (object) ex.Message, (object) Thread.CurrentThread.ManagedThreadId, (object) Process.GetCurrentProcess().Id, (object) Environment.StackTrace));
          throw ex;
        }
      }
    }

    public IDictionary<string, T> GetAll<T>(string[] names) where T : class
    {
      using (PerformanceMeter.Current.BeginOperation("HazelCastCacheStore.Get"))
      {
        Stopwatch sw = Stopwatch.StartNew();
        try
        {
          using (HazelcastConnection conn = HazelCastCacheStore._connectionPool.Open())
          {
            IReadOnlyDictionary<string, byte[]> cacheValues = this.HazelCastStoreOperation<IReadOnlyDictionary<string, byte[]>>("Get", conn, (Func<IHazelcastClient, Task<IReadOnlyDictionary<string, byte[]>>>) (async client => await (await client.GetMapAsync<string, byte[]>(this._mapName)).GetAllAsync((ICollection<string>) names)));
            sw.Stop();
            ILogger logger = DiagUtility.LogManager.GetLogger("Metrics.CacheStore");
            logger.When(Encompass.Diagnostics.Logging.LogLevel.DEBUG, (Action) (() =>
            {
              int num = 0;
              foreach (KeyValuePair<string, byte[]> keyValuePair in (IEnumerable<KeyValuePair<string, byte[]>>) cacheValues)
                num += keyValuePair.Value.Length;
              LogFields info = new LogFields().Set<string>(HazelCastCacheStore.LogFieldNames.Action, nameof (GetAll)).Set<string>(Encompass.Diagnostics.Logging.Schema.Log.CommonFields.CacheKey, string.Join(", ", names)).Set<int>(HazelCastCacheStore.LogFieldNames.QueuedInstanceId, conn.QueuedHazelCastInstance.Id).Set<int>(HazelCastCacheStore.LogFieldNames.QueuedInstanceRefCount, conn.QueuedHazelCastInstance.Refcount).Set<double>(Encompass.Diagnostics.Logging.Schema.Log.CommonFields.DurationMS, (double) sw.ElapsedMilliseconds).Set<int>(HazelCastCacheStore.LogFieldNames.DataBytes, num);
              logger.Write(Encompass.Diagnostics.Logging.LogLevel.DEBUG, "HazelcastCacheStore", string.Format("Hazelcast GetAll operation transferred {0} bytes", (object) num), info);
            }));
            return (IDictionary<string, T>) cacheValues.ToDictionary<KeyValuePair<string, byte[]>, string, T>((Func<KeyValuePair<string, byte[]>, string>) (item => item.Key), (Func<KeyValuePair<string, byte[]>, T>) (item =>
            {
              object all = item.Value == null ? (object) null : HazelCastCacheStore.Deserialize(item.Value);
              if (all is HazelCastCacheStore.ObjectHash)
              {
                HazelCastCacheStore.NearCacheItem nearCacheItem = this.nearCache.Get<HazelCastCacheStore.NearCacheItem>(item.Key);
                if (nearCacheItem != null && nearCacheItem.Hash.Equals(all))
                {
                  sw.Restart();
                  all = HazelCastCacheStore.Deserialize(nearCacheItem.ObjectData);
                  sw.Stop();
                }
                else
                  all = nearCacheItem == null ? (object) null : (object) null;
              }
              return (T) all;
            }));
          }
        }
        catch (Exception ex)
        {
          sw.Stop();
          this.hazelCastRecorder.IncrementErrorCount("Get", this.instanceId, HazelCastCacheStore.VersionNumber);
          TraceLog.WriteError(nameof (HazelCastCacheStore), string.Format("HazelCast Get for keys={0} failed with error={1} CT={2}_{3} Trace={4}", (object) string.Join(", ", names), (object) ex.Message, (object) Thread.CurrentThread.ManagedThreadId, (object) Process.GetCurrentProcess().Id, (object) Environment.StackTrace));
          throw ex;
        }
      }
    }

    public T Get<T>(string name) => (T) this.Get(name);

    public void Put(string name, object o)
    {
      using (PerformanceMeter.Current.BeginOperation("HazelCastCacheStore.Put"))
      {
        if (o == null)
          return;
        HazelcastConnection conn = (HazelcastConnection) null;
        byte[] value = HazelCastCacheStore.Serialize(o);
        Stopwatch sw = Stopwatch.StartNew();
        try
        {
          conn = HazelCastCacheStore._connectionPool.Open();
          if (HazelCastCacheStore.nearCacheObjectSize >= 0 && value.Length > HazelCastCacheStore.nearCacheObjectSize)
          {
            HazelCastCacheStore.ObjectHash objectHash = HazelCastCacheStore.ObjectHash.Create(value);
            this.nearCache.Put(name, (object) new HazelCastCacheStore.NearCacheItem()
            {
              Hash = objectHash,
              ObjectData = value
            });
            value = HazelCastCacheStore.Serialize((object) objectHash);
          }
          bool flag = this.HazelCastStoreOperation<bool>(nameof (Put), conn, (Func<IHazelcastClient, Task<bool>>) (async client => await (await client.GetMapAsync<string, byte[]>(this._mapName)).TryPutAsync(name, value, TimeSpan.FromSeconds(2.0))));
          ILogger logger = DiagUtility.LogManager.GetLogger("Metrics.CacheStore");
          logger.When(Encompass.Diagnostics.Logging.LogLevel.DEBUG, (Action) (() =>
          {
            LogFields logFields = new LogFields().Set<string>(HazelCastCacheStore.LogFieldNames.Action, nameof (Put)).Set<string>(Encompass.Diagnostics.Logging.Schema.Log.CommonFields.CacheKey, name).Set<int>(HazelCastCacheStore.LogFieldNames.QueuedInstanceId, conn.QueuedHazelCastInstance.Id).Set<int>(HazelCastCacheStore.LogFieldNames.QueuedInstanceRefCount, conn.QueuedHazelCastInstance.Refcount).Set<double>(Encompass.Diagnostics.Logging.Schema.Log.CommonFields.DurationMS, (double) sw.ElapsedMilliseconds);
            if (value != null)
              logFields.Set<int>(HazelCastCacheStore.LogFieldNames.DataBytes, value.Length);
            ILogger logger1 = logger;
            byte[] numArray = value;
            string message = string.Format("Hazelcast Put operation transferred {0} bytes", (object) (numArray != null ? numArray.Length : 0));
            LogFields info = logFields;
            logger1.Write(Encompass.Diagnostics.Logging.LogLevel.DEBUG, "HazelcastCacheStore", message, info);
          }));
          if (flag)
            return;
          TraceLog.WriteWarning(nameof (HazelCastCacheStore), string.Format("Put for key='{0}' failed with error={2} CT={5}_{6} conn={1}, Client={4} Trace={3}", (object) name, (object) HazelCastCacheStore.CheckConnection(conn), (object) ("Unable to PUT in time =" + (object) sw.ElapsedMilliseconds), (object) Environment.StackTrace, (object) HazelCastCacheStore.CheckClient(conn), (object) Thread.CurrentThread.ManagedThreadId, (object) Process.GetCurrentProcess().Id));
        }
        catch (Exception ex)
        {
          this.hazelCastRecorder.IncrementErrorCount(nameof (Put), this.instanceId, HazelCastCacheStore.VersionNumber);
          TraceLog.WriteWarning(nameof (HazelCastCacheStore), string.Format("Put for key='{0}' failed with error={2} CT={5}_{6} conn={1}, Client={4} Trace={3}", (object) name, (object) HazelCastCacheStore.CheckConnection(conn), (object) ("Unable to PUT in time =" + (object) sw.ElapsedMilliseconds), (object) Environment.StackTrace, (object) HazelCastCacheStore.CheckClient(conn), (object) Thread.CurrentThread.ManagedThreadId, (object) Process.GetCurrentProcess().Id));
          throw ex;
        }
        finally
        {
          sw.Stop();
          if (conn != null)
            conn.Dispose();
        }
      }
    }

    private static string CheckConnection(HazelcastConnection conn)
    {
      return conn != null ? "not null" : "null";
    }

    private static string CheckClient(HazelcastConnection conn)
    {
      return conn != null && conn.QueuedHazelCastInstance != null && conn.Client != null ? "not null" : "null";
    }

    public void Remove(string name)
    {
      using (PerformanceMeter.Current.BeginOperation("HazelCastCacheStore.Remove"))
      {
        HazelcastConnection conn = (HazelcastConnection) null;
        Stopwatch stopwatch = Stopwatch.StartNew();
        try
        {
          conn = HazelCastCacheStore._connectionPool.Open();
          this.HazelCastStoreOperation<bool>(nameof (Remove), conn, (Func<IHazelcastClient, Task<bool>>) (async client =>
          {
            await (await client.GetMapAsync<string, byte[]>(this._mapName)).DeleteAsync(name);
            return true;
          }));
          this.nearCache.Remove(name);
        }
        catch (Exception ex)
        {
          this.hazelCastRecorder.IncrementErrorCount(nameof (Remove), this.instanceId, HazelCastCacheStore.VersionNumber);
          TraceLog.WriteError(nameof (HazelCastCacheStore), string.Format("HazelCast Remove for key='{0}' failed with error={2} CT={5}_{6} conn={1}, Client={3} Trace={4}", (object) name, (object) HazelCastCacheStore.CheckConnection(conn), (object) ex.Message, (object) Environment.StackTrace, (object) HazelCastCacheStore.CheckClient(conn), (object) Thread.CurrentThread.ManagedThreadId, (object) Process.GetCurrentProcess().Id));
          throw ex;
        }
        finally
        {
          stopwatch.Stop();
          conn?.Dispose();
        }
      }
    }

    public string[] Keys()
    {
      using (PerformanceMeter.Current.BeginOperation("HazelCastCacheStore.Keys"))
      {
        string[] strArray = (string[]) null;
        Stopwatch stopwatch = Stopwatch.StartNew();
        try
        {
          using (HazelcastConnection conn = HazelCastCacheStore._connectionPool.Open())
            strArray = this.HazelCastStoreOperation<string[]>("GetKeys", conn, (Func<IHazelcastClient, Task<string[]>>) (async client => (await (await client.GetMapAsync<string, byte[]>(this._mapName)).GetKeysAsync()).ToArray<string>()));
          stopwatch.Stop();
          return strArray;
        }
        catch (Exception ex)
        {
          stopwatch.Stop();
          this.hazelCastRecorder.IncrementErrorCount(nameof (Keys), this.instanceId, HazelCastCacheStore.VersionNumber);
          TraceLog.WriteError(nameof (HazelCastCacheStore), string.Format("HazelCast operation Keys failed with error={0} CT={1}_{2} Trace={3}", (object) ex.Message, (object) Thread.CurrentThread.ManagedThreadId, (object) Process.GetCurrentProcess().Id, (object) Environment.StackTrace));
          throw ex;
        }
      }
    }

    public IDisposable Lock(
      string key,
      LockType lockType,
      int timeout,
      bool suppressTimeoutWarning = false)
    {
      LockTracer.Create(nameof (HazelCastCacheStore), this.instanceId, key, LockType.ReaderWriter);
      using (PerformanceMeter.Current.BeginOperation("HazelCastCacheStore.Lock"))
      {
        HazelcastConnection conn = (HazelcastConnection) null;
        bool flag = false;
        Stopwatch stopwatch = Stopwatch.StartNew();
        try
        {
          conn = HazelCastCacheStore._connectionPool.Open();
          flag = this.HazelCastStoreOperation<bool>(nameof (Lock), conn, (Func<IHazelcastClient, Task<bool>>) (async client => await (await client.GetMapAsync<string, byte[]>(this._mapName)).TryLockAsync(key, TimeSpan.FromMilliseconds((double) timeout), TimeSpan.FromSeconds((double) HazelCastCacheStore.leaseTime))));
          if (flag)
          {
            TraceLog.WriteVerbose(nameof (HazelCastCacheStore), string.Format("Acquired Lock. Key:{0}", (object) key));
            LockTracer.SetTracerStatus(nameof (HazelCastCacheStore), this.instanceId, key, LockStatus.Acquired);
            return (IDisposable) new HazelCastCacheStore.HazelCastLock(conn)
            {
              Key = key,
              CacheStore = this
            };
          }
          LockTracer.SetTracerStatus(nameof (HazelCastCacheStore), this.instanceId, key, LockStatus.Failed);
          throw new TimeoutException("Unable to obtain LOCK in specified time Taken: " + (object) stopwatch.ElapsedMilliseconds);
        }
        catch (TimeoutException ex)
        {
          this.hazelCastRecorder.IncrementErrorCount("LockTimeout", this.instanceId, HazelCastCacheStore.VersionNumber);
          if (!suppressTimeoutWarning)
            TraceLog.WriteWarning(nameof (HazelCastCacheStore), string.Format("HazelCast Lock for key='{0}' failed with error={2} CT={5}_{6} conn={1}, Client={4} Trace={3}", (object) key, (object) HazelCastCacheStore.CheckConnection(conn), (object) ex.Message, (object) Environment.StackTrace, (object) HazelCastCacheStore.CheckClient(conn), (object) Thread.CurrentThread.ManagedThreadId, (object) Process.GetCurrentProcess().Id));
          throw ex;
        }
        catch (Exception ex)
        {
          this.hazelCastRecorder.IncrementErrorCount(nameof (Lock), this.instanceId, HazelCastCacheStore.VersionNumber);
          TraceLog.WriteWarning(nameof (HazelCastCacheStore), string.Format("HazelCast Lock for key='{0}' failed with error={2} CT={5}_{6} conn={1}, Client={4} Trace={3}", (object) key, (object) HazelCastCacheStore.CheckConnection(conn), (object) ex.Message, (object) Environment.StackTrace, (object) HazelCastCacheStore.CheckClient(conn), (object) Thread.CurrentThread.ManagedThreadId, (object) Process.GetCurrentProcess().Id));
          throw ex;
        }
        finally
        {
          stopwatch.Stop();
          if (!flag && conn != null)
            conn.Dispose();
        }
      }
    }

    private void Unlock(string key)
    {
      LockTracer.SetTracerStatus(nameof (HazelCastCacheStore), this.instanceId, key, LockStatus.Completed);
      Stopwatch stopwatch = Stopwatch.StartNew();
      try
      {
        HazelcastConnection conn = HazelCastCacheStore._connectionPool.Open();
        using (PerformanceMeter.Current.BeginOperation("HazelCastCacheStore.Unlock"))
        {
          this.HazelCastStoreOperation<bool>(nameof (Unlock), conn, (Func<IHazelcastClient, Task<bool>>) (async client =>
          {
            await (await client.GetMapAsync<string, byte[]>(this._mapName)).UnlockAsync(key);
            return true;
          }));
          TraceLog.WriteVerbose(nameof (HazelCastCacheStore), string.Format("Released Lock. Key:{0}", (object) key));
        }
        stopwatch.Stop();
        LockTracer.SetTracerStatus(nameof (HazelCastCacheStore), this.instanceId, key, LockStatus.Released);
      }
      catch (Exception ex)
      {
        stopwatch.Stop();
        this.hazelCastRecorder.IncrementErrorCount(nameof (Unlock), this.instanceId, HazelCastCacheStore.VersionNumber);
        string str = ex.Message;
        if (ex.Message != null && ex.Message.StartsWith("Current thread is not owner of the lock"))
        {
          LockTracer.SetTracerStatus(nameof (HazelCastCacheStore), this.instanceId, key, LockStatus.EvictionDetected);
          str = "Lease time expired for the lock. " + str;
        }
        else
          LockTracer.SetTracerStatus(nameof (HazelCastCacheStore), this.instanceId, key, LockStatus.ReleaseErrored);
        TraceLog.WriteError(nameof (HazelCastCacheStore), string.Format("HazelCast Unlock for key='{0}' failed with error={1} CT={3}_{4}, Trace={2}", (object) key, (object) str, (object) Environment.StackTrace, (object) Thread.CurrentThread.ManagedThreadId, (object) Process.GetCurrentProcess().Id));
        throw ex;
      }
    }

    internal static byte[] Serialize(object obj)
    {
      if (obj == null)
        return (byte[]) null;
      Stopwatch stopwatch = Stopwatch.StartNew();
      BinaryFormatter binaryFormatter = new BinaryFormatter();
      byte[] array;
      using (ByteStream byteStream = new ByteStream(false))
      {
        binaryFormatter.Serialize((Stream) byteStream, obj);
        if (HazelCastCacheStore.largeObjectSize >= 0 && byteStream.Length > (long) HazelCastCacheStore.largeObjectSize)
        {
          HazelCastCacheStore.LargeObject graph = new HazelCastCacheStore.LargeObject(HazelCastCacheStore.Zip(byteStream));
          using (ByteStream serializationStream = new ByteStream(false))
          {
            binaryFormatter.Serialize((Stream) serializationStream, (object) graph);
            array = serializationStream.ToArray();
          }
        }
        else
          array = byteStream.ToArray();
      }
      stopwatch.Stop();
      return array;
    }

    internal static object Deserialize(byte[] data)
    {
      Stopwatch stopwatch = Stopwatch.StartNew();
      BinaryFormatter binaryFormatter = new BinaryFormatter();
      object obj = (object) null;
      using (MemoryStream serializationStream = new MemoryStream(data))
        obj = binaryFormatter.Deserialize((Stream) serializationStream);
      if (obj is HazelCastCacheStore.LargeObject largeObject)
      {
        using (ByteStream serializationStream = HazelCastCacheStore.Unzip(largeObject.content))
          obj = binaryFormatter.Deserialize((Stream) serializationStream);
      }
      stopwatch.Stop();
      return obj;
    }

    private static byte[] Zip(ByteStream inputStream)
    {
      using (ByteStream byteStream = new ByteStream(false))
      {
        using (GZipStream destination = new GZipStream((Stream) byteStream, CompressionLevel.Fastest, true))
        {
          inputStream.Position = 0L;
          inputStream.CopyTo((Stream) destination);
        }
        return byteStream.ToArray();
      }
    }

    private static ByteStream Unzip(byte[] bytes)
    {
      ByteStream destination = new ByteStream(false);
      using (GZipStream gzipStream = new GZipStream((Stream) new MemoryStream(bytes), CompressionMode.Decompress, true))
        gzipStream.CopyTo((Stream) destination);
      destination.Position = 0L;
      return destination;
    }

    private static int compareEntry(
      IMapEntryStats<string, byte[]> x,
      IMapEntryStats<string, byte[]> y)
    {
      if (x.Hits > y.Hits)
        return -1;
      if (x.Hits < y.Hits)
        return 1;
      if (x.Value.Length > y.Value.Length)
        return -1;
      return x.Value.Length < y.Value.Length ? 1 : 0;
    }

    public string GetStats()
    {
      HazelcastConnection conn = (HazelcastConnection) null;
      string str1 = "#Key,Hits,Size,CreationTime,LastAccessTime,LastUpdateTime" + Environment.NewLine;
      try
      {
        conn = HazelCastCacheStore._connectionPool.Open();
        IHMap<string, byte[]> _cache = (IHMap<string, byte[]>) null;
        Task.Run((Func<Task>) (async () => _cache = await conn.Client.GetMapAsync<string, byte[]>(this._mapName))).WaitOrThrowOnError();
        List<IMapEntryStats<string, byte[]>> mapEntryStatsList = new List<IMapEntryStats<string, byte[]>>();
        foreach (string key1 in this.Keys())
        {
          string key = key1;
          IMapEntryStats<string, byte[]> mapEntryStats = this.HazelCastStoreOperation<IMapEntryStats<string, byte[]>>("GetEntryView", conn, (Func<IHazelcastClient, Task<IMapEntryStats<string, byte[]>>>) (async client => await _cache.GetEntryViewAsync(key)));
          if (mapEntryStats != null)
            mapEntryStatsList.Add(mapEntryStats);
        }
        mapEntryStatsList.Sort(new Comparison<IMapEntryStats<string, byte[]>>(HazelCastCacheStore.compareEntry));
        foreach (IMapEntryStats<string, byte[]> mapEntryStats in mapEntryStatsList)
        {
          string str2 = mapEntryStats.Key + "," + (object) mapEntryStats.Hits + "," + (object) mapEntryStats.Value.Length;
          DateTime dateTime = HazelCastCacheStore.UnixTimeToDateTime(mapEntryStats.CreationTime);
          string str3 = dateTime.ToString("MM-dd-yyyy hh:mm:ss");
          string str4 = str2 + "," + str3;
          dateTime = HazelCastCacheStore.UnixTimeToDateTime(mapEntryStats.LastAccessTime);
          string str5 = dateTime.ToString("MM-dd-yyyy hh:mm:ss");
          string str6 = str4 + "," + str5;
          dateTime = HazelCastCacheStore.UnixTimeToDateTime(mapEntryStats.LastUpdateTime);
          string str7 = dateTime.ToString("MM-dd-yyyy hh:mm:ss");
          string str8 = str6 + "," + str7;
          str1 = str1 + str8 + Environment.NewLine;
        }
        return str1 + Environment.NewLine;
      }
      catch (Exception ex)
      {
        this.hazelCastRecorder.IncrementErrorCount(nameof (GetStats), this.instanceId, HazelCastCacheStore.VersionNumber);
        TraceLog.WriteError(nameof (HazelCastCacheStore), string.Format("HazelCast GetStats failed with error={0} CT={1}_{2} Trace={3}", (object) ex.Message, (object) Thread.CurrentThread.ManagedThreadId, (object) Process.GetCurrentProcess().Id, (object) Environment.StackTrace));
        throw ex;
      }
      finally
      {
        if (conn != null)
          conn.Dispose();
      }
    }

    public virtual async Task DoHealthCheck(Func<HazelcastConnection, Task> action)
    {
      await action(HazelCastCacheStore._connectionPool.Open());
    }

    public static DateTime UnixTimeToDateTime(long unixtime)
    {
      DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
      dateTime = dateTime.AddMilliseconds((double) unixtime);
      return dateTime;
    }

    public IDisposable EnterContext() => AsyncContext.New();

    public void ClearAll()
    {
      try
      {
        HazelcastConnection conn = HazelCastCacheStore._connectionPool.Open();
        Task.Run((Func<Task>) (async () =>
        {
          IHMap<string, byte[]> _cache = await conn.Client.GetMapAsync<string, byte[]>(this._mapName);
          await _cache.ClearAsync();
          foreach (string key in (IEnumerable<string>) await _cache.GetKeysAsync())
            await _cache.DeleteAsync(key);
        })).WaitOrThrowOnError();
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (HazelCastCacheStore), string.Format("HazelCast operation ClearAll failed with error={0} CT={1}_{2} Trace={3}", (object) ex.Message, (object) Thread.CurrentThread.ManagedThreadId, (object) Process.GetCurrentProcess().Id, (object) Environment.StackTrace));
      }
    }

    public static class LogFieldNames
    {
      public static readonly LogFieldName<string> Action = LogFields.Field<string>("hzcAction");
      public static readonly LogFieldName<string> Key = LogFields.Field<string>("hzcKey");
      public static readonly LogFieldName<int> QueuedInstanceId = LogFields.Field<int>("hzcQueuedInstanceId");
      public static readonly LogFieldName<int> QueuedInstanceRefCount = LogFields.Field<int>("hzcQueuedInstanceRefCount");
      public static readonly LogFieldName<int> DataBytes = LogFields.Field<int>("hzcDataBytes");
    }

    [Serializable]
    private class LargeObject
    {
      public byte[] content;

      public LargeObject(byte[] obj) => this.content = obj;
    }

    internal class HazelCastLock : IDisposable
    {
      private string className = nameof (HazelCastLock);
      private bool isDisposed;
      private Thread lockThread;
      private string stackTrace = "";
      private HazelcastConnection conn;
      private IClientContext clientContext;

      public string Key { get; set; }

      public HazelCastCacheStore CacheStore { get; set; }

      public Thread LockThread => this.lockThread;

      public HazelcastConnection Conn => this.conn;

      ~HazelCastLock() => this.Dispose();

      public HazelCastLock(HazelcastConnection conn)
      {
        this.conn = conn;
        if (EllieMae.EMLite.DataAccess.ServerGlobals.Debug)
          this.stackTrace = new StackTrace(true).ToString();
        this.lockThread = Thread.CurrentThread;
        this.clientContext = ClientContext.GetCurrent(false);
      }

      public void Dispose()
      {
        try
        {
          if (this.isDisposed)
            return;
          try
          {
            if (Thread.CurrentThread != this.LockThread)
            {
              string message = string.Format("Current thread is not owner of the lock '{0}' CurrentThread = {1}, Lock Thread = {2}, LockThread Trace = {3}, CurrentThread  Trace= {4}", (object) this.Key, (object) Thread.CurrentThread.ManagedThreadId, (object) this.LockThread.ManagedThreadId, (object) this.stackTrace, (object) Environment.StackTrace);
              if (this.clientContext != null)
                this.clientContext.TraceLog.WriteWarning(this.className, message);
              else
                TraceLog.WriteWarning(this.className, message);
            }
            else
              this.CacheStore.Unlock(this.Key);
          }
          catch
          {
          }
          this.isDisposed = true;
          GC.SuppressFinalize((object) this);
        }
        catch
        {
        }
      }
    }

    [Serializable]
    private class ObjectHash
    {
      public readonly string Base64Hash;

      private ObjectHash(string hash) => this.Base64Hash = hash;

      public bool Compare(byte[] data)
      {
        return this.Base64Hash == Convert.ToBase64String(new SHA1CryptoServiceProvider().ComputeHash(data));
      }

      public override bool Equals(object obj)
      {
        return obj is HazelCastCacheStore.ObjectHash objectHash && objectHash.Base64Hash == this.Base64Hash;
      }

      public override int GetHashCode() => this.Base64Hash.GetHashCode();

      public static HazelCastCacheStore.ObjectHash Create(byte[] data)
      {
        return new HazelCastCacheStore.ObjectHash(Convert.ToBase64String(new SHA1CryptoServiceProvider().ComputeHash(data)));
      }
    }

    private class NearCacheItem
    {
      public HazelCastCacheStore.ObjectHash Hash;
      public byte[] ObjectData;
    }
  }
}
