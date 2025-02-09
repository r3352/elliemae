// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.SimpleCache.CacheItem
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Common.SimpleCache
{
  public class CacheItem
  {
    public DateTime CreatedDate { get; private set; }

    public DateTime LastAccessedDate { get; private set; }

    public CacheItemRetentionPolicy CacheItemPolicy { get; private set; }

    public ICacheItemExpirationBehavior CacheItemExpirationBehavior { get; private set; }

    public object Value { get; private set; }

    public CacheItem(object value)
      : this(value, CacheItemRetentionPolicy.Default, (ICacheItemExpirationBehavior) new SimpleCacheItemExpirationBehavior())
    {
    }

    public CacheItem(object value, CacheItemRetentionPolicy cacheItemPolicy)
      : this(value, cacheItemPolicy, (ICacheItemExpirationBehavior) new SimpleCacheItemExpirationBehavior())
    {
    }

    public CacheItem(
      object value,
      CacheItemRetentionPolicy cacheItemPolicy,
      ICacheItemExpirationBehavior cacheItemExpirationBehavior)
    {
      this.Value = value;
      this.CacheItemPolicy = cacheItemPolicy;
      this.CacheItemExpirationBehavior = cacheItemExpirationBehavior;
      this.LastAccessedDate = this.CreatedDate = DateTime.Now;
    }

    public bool IsExpired()
    {
      if (this.CreatedDate.AddDays(2.0) <= DateTime.Now)
      {
        this.CacheItemExpirationBehavior.OnExpiration(this);
        return true;
      }
      if (this.CacheItemPolicy.AbsoluteExpiration == TimeSpan.MinValue)
      {
        if (this.CacheItemPolicy.SlidingExpiration == TimeSpan.MinValue || this.CreatedDate.Add(this.CacheItemPolicy.SlidingExpiration) <= DateTime.Now)
        {
          this.CacheItemExpirationBehavior.OnExpiration(this);
          return true;
        }
      }
      else if (this.CacheItemPolicy.SlidingExpiration == TimeSpan.MinValue)
      {
        if (this.CreatedDate.Add(this.CacheItemPolicy.AbsoluteExpiration) <= DateTime.Now)
        {
          this.CacheItemExpirationBehavior.OnExpiration(this);
          return true;
        }
      }
      else if (this.LastAccessedDate.Add(this.CacheItemPolicy.SlidingExpiration) <= DateTime.Now || this.CreatedDate.Add(this.CacheItemPolicy.AbsoluteExpiration) <= DateTime.Now)
      {
        this.CacheItemExpirationBehavior.OnExpiration(this);
        return true;
      }
      return false;
    }

    public void ResetSlidingWindow() => this.LastAccessedDate = DateTime.Now;
  }
}
