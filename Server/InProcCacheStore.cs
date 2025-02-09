// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.InProcCacheStore
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class InProcCacheStore : ICacheStore
  {
    private const string ClassName = "InProcCacheStore�";
    private readonly Dictionary<string, object> _cache = new Dictionary<string, object>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
    private readonly Hashtable _locks = new Hashtable((IEqualityComparer) StringComparer.CurrentCultureIgnoreCase);
    private readonly bool enabled;

    public InProcCacheStore(bool enabled)
    {
      TraceLog.WriteDebug(nameof (InProcCacheStore), "CACHE MODE IS SET TO IN PROCESS");
      this.enabled = enabled;
    }

    public CacheStoreSource Source
    {
      get => !this.enabled ? CacheStoreSource.Disabled : CacheStoreSource.InProcess;
    }

    public CacheSetting Setting => !this.enabled ? CacheSetting.Disabled : CacheSetting.Low;

    public bool IsRemoteCache => false;

    public object Get(string name)
    {
      lock (this._cache)
        return this._cache.ContainsKey(name) ? this._cache[name] : (object) null;
    }

    public void Put(string name, object o)
    {
      lock (this._cache)
        this._cache[name] = o;
    }

    public void Remove(string name)
    {
      lock (this._cache)
      {
        if (!this._cache.ContainsKey(name))
          return;
        this._cache.Remove(name);
      }
    }

    public string[] Keys()
    {
      lock (this._cache)
      {
        string[] array = new string[this._cache.Count];
        this._cache.Keys.CopyTo(array, 0);
        return array;
      }
    }

    public T Get<T>(string name) => (T) this.Get(name);

    public IDictionary<string, T> GetAll<T>(string[] names) where T : class
    {
      Dictionary<string, T> all = new Dictionary<string, T>();
      foreach (string name in names)
      {
        T obj = (T) this.Get(name);
        if ((object) obj != null)
          all.Add(name, obj);
      }
      return (IDictionary<string, T>) all;
    }

    public IDisposable Lock(
      string key,
      LockType lockType,
      int timeout,
      bool suppressTimeoutWarning = false)
    {
      try
      {
        if (lockType == LockType.ReaderWriter)
          return this.AcquireWriter(key, TimeSpan.FromMilliseconds((double) timeout));
        return lockType == LockType.ReadOnly ? this.AcquireReader(key, TimeSpan.FromMilliseconds((double) timeout)) : (IDisposable) null;
      }
      catch (ApplicationException ex)
      {
        if (ex.Message.ToLower().Contains("timeout period expired") || ex.InnerException != null && ex.InnerException.Message.ToLower().Contains("timeout period expired"))
          throw new TimeoutException(string.Format("Unable to obtain LOCK[{1}] in specified time Time Taken: {0}", (object) TimeSpan.FromMilliseconds((double) timeout), (object) key), (Exception) ex);
        throw;
      }
    }

    public IDisposable AcquireWriter(string name, TimeSpan timeout)
    {
      ReaderWriterLock innerLock = (ReaderWriterLock) null;
      ReaderWriterLockSlim innerLockSlim = (ReaderWriterLockSlim) null;
      bool readerWriterLockSlim = SmartClientUtils.UseReaderWriterLockSlim;
      if (readerWriterLockSlim)
        innerLockSlim = this.getReaderWriterLockSlim(name, true);
      else
        innerLock = this.getReaderWriterLock(name, true);
      try
      {
        WriterLock innerObject = !readerWriterLockSlim ? new WriterLock(name, innerLock, timeout) : new WriterLock(name, innerLockSlim, timeout);
        TraceLog.WriteDebug(nameof (InProcCacheStore), "Writer Lock acquired on " + name);
        return (IDisposable) new InProcCacheStore.DisposableReferenceTracker(this, name, (IDisposable) innerObject);
      }
      catch
      {
        TraceLog.WriteDebug(nameof (InProcCacheStore), "Failed to acquire Writer Lock on " + name);
        this.DecrementReferenceCount(name);
        throw;
      }
    }

    public IDisposable AcquireReader(string name, TimeSpan timeout)
    {
      ReaderWriterLock innerLock = (ReaderWriterLock) null;
      ReaderWriterLockSlim innerLockSlim = (ReaderWriterLockSlim) null;
      bool readerWriterLockSlim = SmartClientUtils.UseReaderWriterLockSlim;
      if (readerWriterLockSlim)
        innerLockSlim = this.getReaderWriterLockSlim(name, true);
      else
        innerLock = this.getReaderWriterLock(name, true);
      try
      {
        ReaderLock innerObject = !readerWriterLockSlim ? new ReaderLock(name, innerLock, timeout) : new ReaderLock(name, innerLockSlim, timeout);
        TraceLog.WriteDebug(nameof (InProcCacheStore), "Reader Lock acquired on " + name);
        return (IDisposable) new InProcCacheStore.DisposableReferenceTracker(this, name, (IDisposable) innerObject);
      }
      catch
      {
        TraceLog.WriteWarning(nameof (InProcCacheStore), "Failed to acquire Reader Lock on " + name);
        this.DecrementReferenceCount(name);
        throw;
      }
    }

    private ReaderWriterLock getReaderWriterLock(string name, bool addReference)
    {
      lock (this._locks)
      {
        object obj = this._locks[(object) name];
        if (obj == null)
        {
          obj = (object) new InProcCacheStore.ReferenceCountedObject((object) new ReaderWriterLock());
          this._locks[(object) name] = obj;
        }
        if (!(obj is InProcCacheStore.ReferenceCountedObject referenceCountedObject))
          throw new InvalidOperationException("Cache object is not a ReferenceCountedObject");
        if (!(referenceCountedObject.InnerObject is ReaderWriterLock))
          throw new InvalidOperationException("The Acquire and AcquireReader/Writer methods cannot be combined");
        if (addReference)
          ++referenceCountedObject.ReferenceCount;
        return (ReaderWriterLock) referenceCountedObject.InnerObject;
      }
    }

    private ReaderWriterLockSlim getReaderWriterLockSlim(string name, bool addReference)
    {
      lock (this._locks)
      {
        object obj = this._locks[(object) name];
        if (obj == null)
        {
          obj = (object) new InProcCacheStore.ReferenceCountedObject((object) new ReaderWriterLockSlim(!SmartClientUtils.LockSlimNoRecursion ? LockRecursionPolicy.SupportsRecursion : LockRecursionPolicy.NoRecursion));
          this._locks[(object) name] = obj;
        }
        if (!(obj is InProcCacheStore.ReferenceCountedObject referenceCountedObject))
          throw new InvalidOperationException("Cache object is not a ReferenceCountedObject");
        if (!(referenceCountedObject.InnerObject is ReaderWriterLockSlim))
          throw new InvalidOperationException("The Acquire and AcquireReader/Writer methods cannot be combined");
        if (addReference)
          ++referenceCountedObject.ReferenceCount;
        return (ReaderWriterLockSlim) referenceCountedObject.InnerObject;
      }
    }

    private void DecrementReferenceCount(string name)
    {
      lock (this._locks)
      {
        object obj = this._locks[(object) name];
        if (obj == null || !(obj is InProcCacheStore.ReferenceCountedObject referenceCountedObject))
          return;
        --referenceCountedObject.ReferenceCount;
        if (referenceCountedObject.ReferenceCount != 0)
          return;
        this._locks.Remove((object) name);
      }
    }

    public string GetStats() => "Cache Stats only available for Hazelcast";

    public IDisposable EnterContext() => (IDisposable) new InProcCacheStore.NoOpContext();

    public void ClearAll()
    {
      foreach (string key in this.Keys())
        this.Remove(key);
    }

    private class ReferenceCountedObject
    {
      private object innerObject;
      private int refCount;

      public ReferenceCountedObject(object innerObject) => this.innerObject = innerObject;

      public object InnerObject => this.innerObject;

      public int ReferenceCount
      {
        get => this.refCount;
        set => this.refCount = value;
      }
    }

    private class DisposableReferenceTracker : IDisposable
    {
      private InProcCacheStore cache;
      private string name;
      private IDisposable innerObject;
      private string stackTrace;
      private DateTime acquiredDateTime;

      ~DisposableReferenceTracker()
      {
        string className = nameof (DisposableReferenceTracker);
        TraceLog.WriteWarning(className, "The lock on key is being Finalized on Key " + this.name);
        if (EllieMae.EMLite.DataAccess.ServerGlobals.Debug)
        {
          TraceLog.WriteWarning(className, this.stackTrace);
          this.CheckTime();
        }
        this.Dispose();
      }

      public DisposableReferenceTracker(
        InProcCacheStore cache,
        string name,
        IDisposable innerObject)
      {
        this.cache = cache;
        this.name = name;
        this.innerObject = innerObject;
        if (!EllieMae.EMLite.DataAccess.ServerGlobals.Debug)
          return;
        this.stackTrace = new StackTrace(true).ToString();
        this.acquiredDateTime = DateTime.Now;
      }

      public void Dispose()
      {
        if (this.innerObject != null)
        {
          this.innerObject.Dispose();
          this.innerObject = (IDisposable) null;
          this.cache.DecrementReferenceCount(this.name);
        }
        GC.SuppressFinalize((object) this);
        if (!EllieMae.EMLite.DataAccess.ServerGlobals.Debug)
          return;
        TraceLog.WriteDebug(nameof (DisposableReferenceTracker), "The lock on key is being DISPOSED on Key " + this.name);
        this.CheckTime();
      }

      private void CheckTime()
      {
        if ((DateTime.Now - this.acquiredDateTime).TotalSeconds <= 20.0)
          return;
        TraceLog.WriteDebug(nameof (DisposableReferenceTracker), "Lock Time Exceeded the limit of 20 seconds");
        TraceLog.WriteDebug(nameof (DisposableReferenceTracker), this.stackTrace);
      }
    }

    internal class NoOpContext : IDisposable
    {
      public void Dispose()
      {
      }
    }
  }
}
