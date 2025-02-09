// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.HzcLoanLockFactory
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Server.Cache;
using EllieMae.EMLite.Server.Configuration;
using System;
using System.Collections.Concurrent;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class HzcLoanLockFactory
  {
    private readonly ConcurrentDictionary<string, ICacheStore> caches = new ConcurrentDictionary<string, ICacheStore>();
    public static readonly HzcLoanLockFactory Instance;
    public static bool UseHzcLock;

    static HzcLoanLockFactory()
    {
      try
      {
        HzcLoanLockFactory.UseHzcLock = CacheStoreConfiguration.CurrentConfiguration.UseHzcForLoanLock;
        if (!HzcLoanLockFactory.UseHzcLock)
          return;
        HzcLoanLockFactory.Instance = new HzcLoanLockFactory();
      }
      catch
      {
        HzcLoanLockFactory.UseHzcLock = false;
        HzcLoanLockFactory.Instance = (HzcLoanLockFactory) null;
      }
    }

    public ICacheLock<bool?> CheckOutLoan(string classname, string key, int timeout = 20000)
    {
      string key1 = string.Format("{0}_{1}", (object) classname, (object) Guid.Parse(key).ToString());
      ICacheStore cacheSore = this.GetCacheSore((IClientContext) ClientContext.GetCurrent());
      IDisposable lockObj = cacheSore.Lock(key1, LockType.ReaderWriter, timeout);
      try
      {
        return (ICacheLock<bool?>) new CacheLock<bool?>(key1, (object) key, new bool?(), lockObj, cacheSore, false);
      }
      catch
      {
        lockObj.Dispose();
        throw;
      }
    }

    private ICacheStore GetCacheSore(IClientContext context)
    {
      return this.caches.GetOrAdd(context.InstanceName, (Func<string, ICacheStore>) (i => (ICacheStore) new HazelCastCacheStore(context.InstanceName, "CACHE")));
    }

    public IDisposable EnterContext(IClientContext context)
    {
      return this.GetCacheSore(context).EnterContext();
    }
  }
}
