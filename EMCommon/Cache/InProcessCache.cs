// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Cache.InProcessCache
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Collections.Generic;
using System.Threading;

#nullable disable
namespace EllieMae.EMLite.Cache
{
  public class InProcessCache : IDataCache
  {
    private Dictionary<string, object> cacheLocks = new Dictionary<string, object>();
    private Dictionary<string, object> cacheValues = new Dictionary<string, object>();

    public InProcessCache(bool threadSafe) => this.ThreadSafe = threadSafe;

    public bool ThreadSafe { get; private set; }

    public T Get<T>(string key)
    {
      object lockObject = this.acquireLock(key);
      try
      {
        object obj;
        if (!this.cacheValues.TryGetValue(key, out obj))
          return default (T);
        try
        {
          return (T) obj;
        }
        catch (InvalidCastException ex)
        {
          return default (T);
        }
      }
      finally
      {
        this.releaseLock(lockObject);
      }
    }

    public T Get<T>(string key, Func<T> loaderFunc)
    {
      object lockObject = this.acquireLock(key);
      try
      {
        object obj;
        if (this.cacheValues.TryGetValue(key, out obj))
        {
          try
          {
            return (T) obj;
          }
          catch (InvalidCastException ex)
          {
            return default (T);
          }
        }
        else
        {
          obj = (object) loaderFunc();
          this.cacheValues.Add(key, obj);
          return (T) obj;
        }
      }
      finally
      {
        this.releaseLock(lockObject);
      }
    }

    public void Set<T>(string key, T value)
    {
      object lockObject = this.acquireLock(key);
      try
      {
        this.cacheValues[key] = (object) value;
      }
      finally
      {
        this.releaseLock(lockObject);
      }
    }

    public void Remove(string key)
    {
      object lockObject = this.acquireLock(key);
      try
      {
        if (!this.cacheValues.ContainsKey(key))
          return;
        this.cacheValues.Remove(key);
      }
      finally
      {
        this.releaseLock(lockObject);
      }
    }

    private object acquireLock(string key)
    {
      object obj = (object) null;
      if (this.ThreadSafe)
      {
        lock (this.cacheLocks)
        {
          if (!this.cacheLocks.TryGetValue(key, out obj))
          {
            obj = new object();
            this.cacheLocks.Add(key, obj);
          }
        }
        Monitor.Enter(obj);
      }
      return obj;
    }

    private void releaseLock(object lockObject)
    {
      if (lockObject == null)
        return;
      Monitor.Exit(lockObject);
    }

    private object getLockObject(string key)
    {
      object lockObject = (object) null;
      lock (this.cacheLocks)
      {
        if (!this.cacheLocks.TryGetValue(key, out lockObject))
        {
          lockObject = new object();
          this.cacheLocks.Add(key, lockObject);
        }
      }
      return lockObject;
    }
  }
}
